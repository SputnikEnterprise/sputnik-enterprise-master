Imports SP.DatabaseAccess.Listing.DataObjects
Imports System.ComponentModel
Imports SP.DatabaseAccess.Customer.DataObjects
Imports DevExpress.Utils.Animation
Imports SP.Infrastructure

Namespace UI

	Partial Class frmImportData


		Private m_Utility As SP.Infrastructure.Utility

		Function LoadSourceTwixCustomerData() As Boolean
			Dim result As Boolean = True

			Dim data = m_SourceListingDatabaseAccess.LoadAllTwixCustomerMasterData(m_InitializationData.MDData.MDNr)

			If data Is Nothing Then
				m_UtilityUI.ShowErrorDialog("Keine Kunden Daten im Source wurden gefunden.")

				Return False
			End If

			Dim existingCustomerGridData = (From customerData In data
											Select New CustomerMasterViewData With
													 {.CustomerNumber = customerData.CustomerNumber,
													.Company1 = customerData.Company1,
													 .Street = customerData.Street,
													 .CountryCode = customerData.CountryCode,
													 .Postcode = customerData.Postcode,
													 .Location = customerData.Location,
													 .KDBusinessBranch = customerData.KDBusinessBranch,
													 .Telephone = customerData.Telephone,
													 .EMail = customerData.EMail,
													 .Hompage = customerData.Hompage,
													 .facebook = customerData.facebook,
													 .KST = m_InitializationData.UserData.UserKST,
													 .CreatedOn = customerData.CreatedOn,
													 .ChangedOn = customerData.CreatedOn,
													 .Selected = tgsSourceSelection.EditValue}).ToList()


			m_SourceCustomerData = New BindingList(Of CustomerMasterViewData)

			For Each customerGridData In existingCustomerGridData

				If customerGridData.CreatedOn Is Nothing OrElse deDate.EditValue Is Nothing OrElse customerGridData.CreatedOn > deDate.EditValue Then
					m_SourceCustomerData.Add(customerGridData)
				End If

			Next

			grdSourceCustomer.DataSource = m_SourceCustomerData


			Return Not m_SourceCustomerData Is Nothing
		End Function

		Private Function ImportTwixCustomersIntoCurrentDatabase() As Boolean
			Dim result As Boolean = True
			Dim offsetNumber As Integer = ReadCustomerOffsetFromSettings()
			m_invalidCustomerData = New BindingList(Of CustomerMasterViewData)
			m_importedCustomerData = New BindingList(Of CustomerMasterData)

			Dim transiton As Transition = New Transition()
			transiton.Control = nfMain
			transiton.TransitionType = New SlideFadeTransition()
			Dim manager As TransitionManager = New TransitionManager()
			manager.Transitions.Add(transiton)

			manager.StartTransition(nfMain)

			Try
				result = result AndAlso LoadDestCustomerData()
				m_EventLog.WriteTempLogFile(String.Format("Twix-Import: Ziel-Kundendatan {0}", m_CustomerData.Count), m_LOGFileName)
				m_Logger.LogInfo(String.Format("starting import. DestCustomerTable: {0} >>> SourceDatabase: {1}", m_CustomerData.Count, m_SourceInitializationData.MDData.MDDbName))

				For Each Customer In m_SourceCustomerData.Where(Function(x) x.Selected = True)
					result = True
					m_EventLog.WriteTempLogFile(String.Format("Twix-Import: Es wird gesucht nach: Company1: {0} | Postcode: {1}", Customer.Company1, Customer.Postcode), m_LOGFileName)

					Dim searchCustomer As CustomerMasterData
					Try
						searchCustomer = m_CustomerData.Where(Function(x) x.Company1 = Customer.Company1 And x.Postcode = Customer.Postcode).FirstOrDefault

					Catch ex As Exception
						m_EventLog.WriteTempLogFile(String.Format("***Twix-Import: Source Customer wird übersprungen ({0}): Company1: {1} | Postcode: {2} >>> {3}", Customer.CustomerNumber, Customer.Company1, Customer.Postcode, ex.ToString), m_LOGFileName)
						Continue For

					End Try

					If Not searchCustomer Is Nothing Then
						m_EventLog.WriteTempLogFile(String.Format("***Twix-Import: Wurde als Duplikat gefunden: Nummer: {0} >>> Company1: {1} | Postcode: {2}", searchCustomer.CustomerNumber, Customer.Company1, Customer.Postcode), m_LOGFileName)

						Customer.SourceChangedOn = Customer.ChangedOn
						Customer.SourceCreatedOn = Customer.CreatedOn
						Customer.DestChangedOn = searchCustomer.ChangedOn
						Customer.DestCreatedOn = searchCustomer.CreatedOn
						Customer.DestCompleteAddress = searchCustomer.CustomerCompleteAddress


						m_invalidCustomerData.Add(Customer)

						m_SourceCustomerData(m_SourceCustomerData.IndexOf(Customer)).Selected = False
						Customer.Selected = False

						Continue For
					End If
					Dim tranferData = New CustomerTranferData With {.DestCustomerID = m_InitializationData.MDData.MDGuid, .DestMDNumber = m_InitializationData.MDData.MDNr,
						.SourceCustomerNumber = Customer.CustomerNumber, .DestCustomerOffsetNumber = offsetNumber, .SourceDataBaseName = m_SourceInitializationData.MDData.MDDbName}

					result = result AndAlso AddTwixCustomerIntoSputik(Customer)

					If Not result Then m_Logger.LogError(String.Format("error during AddTwixCustomerIntoSputik. SourceCustomerNumber: {0}", Customer.CustomerNumber))
					If Not result Then m_EventLog.WriteTempLogFile(String.Format("***Twix-Import: Master konnte nicht übernommen werden: Nummer: {0} >>> Company1: {1} | Postcode: {2}", Customer.CustomerNumber, Customer.Company1, Customer.Postcode), m_LOGFileName)


					If Not result Then
						'If tranferData.DestNewCustomerNumber.GetValueOrDefault(0) > 0 Then Dim deleteResult = m_ListingDatabaseAccess.DeleteAssignedImportedCustomerData(tranferData.DestNewCustomerNumber.GetValueOrDefault(0))
						m_EventLog.WriteTempLogFile(String.Format("***Twix-Import: Datensatz wurde NICHT erfolgreich importiert: SourceNummer: {0} >>> Company1: {1} | Postcode: {2}", Customer.CustomerNumber, Customer.Company1, Customer.Postcode), m_LOGFileName)
						m_Logger.LogError(String.Format("error during import. DestNewCustomerNumber: {0}", tranferData.DestNewCustomerNumber.GetValueOrDefault(0)))

						Continue For
					Else
						Dim data = m_CustomerDatabaseAccess.LoadCustomerMasterData(Customer.CustomerNumber, String.Empty)
						m_EventLog.WriteTempLogFile(String.Format("Twix-Import: Datensatz wurde erfolgreich importiert: Neue Nummer: {0} >>> Company1: {1} | Postcode: {2}",
																  Customer.CustomerNumber, Customer.Company1, Customer.Postcode), m_LOGFileName)

						m_CustomerData.Add(data)
						m_importedCustomerData.Add(data)

						m_SourceCustomerData(m_SourceCustomerData.IndexOf(Customer)).Selected = False
						Customer.Selected = False
					End If
				Next
				grdSourceCustomer.RefreshDataSource()


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				m_EventLog.WriteTempLogFile(String.Format("***Twix-Import: ImportTwixCustomersIntoCurrentDatabase: {0}", ex.ToString), m_LOGFileName)

				Return False

			Finally
				RefreshCustomerModifiedDataSources()
				manager.EndTransition()
			End Try

			Return result
		End Function

		Private Function AddTwixCustomerIntoSputik(ByVal customer As CustomerMasterViewData) As Boolean
			Dim result As Boolean = True

			m_Utility = New SP.Infrastructure.Utility


			Dim FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"

			Dim customerNumberOffsetFromSettings As Integer = ReadCustomerOffsetFromSettings()
			Dim kst = m_InitializationData.UserData.UserKST
			Dim currencyvalue As String = "CHF"
			Dim m_path As New SPProgUtility.ProgPath.ClsProgPath
			Dim xmlFilename As String = m_InitializationData.MDData.MDFormXMLFileName '  m_md.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr)

			Dim invoiceremindercode As String = m_path.GetXMLNodeValue(xmlFilename, String.Format("{0}/invoiceremindercode", FORM_XML_MAIN_KEY))
			Dim PaymentCondition As String = m_path.GetXMLNodeValue(xmlFilename, String.Format("{0}/conditionalcash", FORM_XML_MAIN_KEY))
			Dim InvoiceOption As String = m_path.GetXMLNodeValue(xmlFilename, String.Format("{0}/invoicetype", FORM_XML_MAIN_KEY))
			Dim NoUse As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(xmlFilename, String.Format("{0}/customernotuse", FORM_XML_MAIN_KEY)), False)
			Dim CreditWarning As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(xmlFilename, String.Format("{0}/warnbycreditlimitexceeded", FORM_XML_MAIN_KEY)), False)
			Dim CreditLimit1 As Decimal? = m_Utility.ParseToDec(m_path.GetXMLNodeValue(xmlFilename, String.Format("{0}/firstcreditlimitamount", FORM_XML_MAIN_KEY)), 0)
			Dim CreditLimit2 As Decimal? = m_Utility.ParseToDec(m_path.GetXMLNodeValue(xmlFilename, String.Format("{0}/secondcreditlimitamount", FORM_XML_MAIN_KEY)), 0)
			Dim OneInvoicePerMail As String = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(xmlFilename, String.Format("{0}/oneinvoicepermail", FORM_XML_MAIN_KEY)), False)

			Dim initCustomerData = New NewCustomerInitData With {.CustomerNumberOffset = customerNumberOffsetFromSettings}
			initCustomerData.CustomerMandantNumber = m_InitializationData.MDData.MDNr
			initCustomerData.Company1 = customer.Company1
			initCustomerData.Street = customer.Street
			initCustomerData.CountryCode = "CH"
			initCustomerData.Postcode = customer.Postcode
			initCustomerData.Location = customer.Location
			initCustomerData.KST = m_InitializationData.UserData.UserKST
			initCustomerData.CurrencyCode = currencyvalue
			initCustomerData.ReminderCode = invoiceremindercode
			initCustomerData.PaymentCondition = PaymentCondition
			initCustomerData.InvoiceOption = InvoiceOption
			initCustomerData.NoUse = NoUse
			initCustomerData.CreditWarning = CreditWarning

			initCustomerData.OneInvoicePerMail = OneInvoicePerMail
			initCustomerData.CreditLimit1 = CreditLimit1
			initCustomerData.CreditLimit2 = CreditLimit2
			initCustomerData.KDBusinessBranch = m_InitializationData.UserData.UserFiliale
			initCustomerData.CreatedFrom = m_InitializationData.UserData.UserFullName
			initCustomerData.CreatedUserNumber = m_InitializationData.UserData.UserNr

			result = result AndAlso m_CustomerDatabaseAccess.AddNewCustomer(initCustomerData)
			If Not result Then Return result
			customer.CustomerNumber = initCustomerData.CustomerNumber


			If Not (String.IsNullOrWhiteSpace(customer.Telephone) AndAlso String.IsNullOrWhiteSpace(customer.EMail) AndAlso String.IsNullOrWhiteSpace(customer.Hompage)) Then
				Dim newCustomer = m_CustomerDatabaseAccess.LoadCustomerMasterData(initCustomerData.CustomerNumber, String.Empty)

				newCustomer.Telephone = customer.Telephone
				newCustomer.EMail = customer.EMail
				newCustomer.Hompage = customer.Hompage

				result = result AndAlso m_CustomerDatabaseAccess.UpdateCustomerMasterData(newCustomer)
			End If


			If result AndAlso Not String.IsNullOrWhiteSpace(customer.KDBusinessBranch) Then
				Dim sectorData = New CustomerAssignedSectorData With {.CustomerNumber = initCustomerData.CustomerNumber, .Description = customer.KDBusinessBranch}

				result = result AndAlso m_CustomerDatabaseAccess.AddCustomerSectorAssignment(sectorData)
			End If

			' facebook is berufe!!!
			If result AndAlso Not String.IsNullOrWhiteSpace(customer.facebook) Then
				Dim professionToInsert = New CustomerAssignedProfessionData With {.CustomerNumber = initCustomerData.CustomerNumber, .Description = customer.facebook, .ProfessionCodeInteger = 0}

				result = result AndAlso m_CustomerDatabaseAccess.AddCustomerProfessionAssignment(professionToInsert)
			End If

			Return result
		End Function

		Private Function UpdateAssignedCustomerWithTwixSourceData(ByVal sourceData As CustomerMasterViewData) As Boolean
			Dim result As Boolean = True


			' writing destination data
			Dim searchCustomer = m_CustomerData.Where(Function(x) x.Company1 = sourceData.Company1 And x.Postcode = sourceData.Postcode).FirstOrDefault
			m_EventLog.WriteTempLogFile(String.Format("Twix-Import: Update der Kundendaten {0} {1}", sourceData.Company1, sourceData.Postcode), m_LOGFileName)

			If searchCustomer Is Nothing Then
				m_EventLog.WriteTempLogFile(String.Format("T***wix-Import: Kundendaten nicht gefunden! {0} {1}", sourceData.Company1, sourceData.Postcode), m_LOGFileName)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Zieldaten wurden nicht gefunden. Der Vorgang wird abgebrochen."))
				m_Logger.LogWarning(String.Format("destCustomer was not founded:{0}sourceCompany1: {1} | sourcePostcode: {2}", vbNewLine, sourceData.Company1, sourceData.Postcode))

				Return False
			End If
			Dim sourceMasterData = m_CustomerDatabaseAccess.LoadCustomerMasterData(searchCustomer.CustomerNumber, m_InitializationData.UserData.UserFiliale)
			If sourceMasterData Is Nothing Then Return False

			sourceMasterData.Company1 = sourceData.Company1
			sourceMasterData.Street = sourceData.Street
			sourceMasterData.Postcode = sourceData.Postcode
			sourceMasterData.Location = sourceData.Location
			sourceMasterData.Telephone = sourceData.Telephone
			sourceMasterData.EMail = sourceData.EMail
			sourceMasterData.Hompage = sourceData.Hompage

			result = result AndAlso m_CustomerDatabaseAccess.UpdateCustomerMasterData(sourceMasterData)
			m_EventLog.WriteTempLogFile(String.Format("Twix-Import: Update der (Stamm) Kundendaten ({0}) {1} {2}: {3}", sourceMasterData.CustomerNumber, sourceMasterData.Company1, sourceMasterData.Postcode, result), m_LOGFileName)


			If result Then
				m_importedCustomerData.Add(sourceMasterData)

			Else
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ihre Daten konnten nicht importiert werden."))

				Return False
			End If


			Return result
		End Function

		Private Function AddAssignedTwixCustomerFromSourceData(ByVal sourceData As CustomerMasterViewData) As Boolean
			Dim result As Boolean = True

			result = result AndAlso AddTwixCustomerIntoSputik(sourceData)
			m_EventLog.WriteTempLogFile(String.Format("Twix-Import: Hinzufügen der (Source) Kundendaten ({0}): {1}", sourceData.CustomerNumber, result), m_LOGFileName)

			If Not result Then
				Dim deleteResult = m_ListingDatabaseAccess.DeleteAssignedImportedCustomerData(sourceData.CustomerNumber)
				m_Logger.LogError(String.Format("***AddAssignedTwixCustomerFromSourceData was not successfull:{0}SourceCustomerNumber: {1} | sourcePostcode: {2}", vbNewLine, sourceData.CustomerNumber, sourceData.Postcode))
				m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Fehler während Import von Kunden Daten. SourceCustomerNumber: {0}. Der Vorgang wird abgebrochen."), sourceData.CustomerNumber))

				Return False

			Else
				Dim data = m_CustomerDatabaseAccess.LoadCustomerMasterData(sourceData.CustomerNumber, String.Empty)
				m_EventLog.WriteTempLogFile(String.Format("Twix-Import: Datensatz wurde erfolgreich importiert: Neue Nummer: {0} >>> Company1: {1} | Postcode: {2}",
																  sourceData.CustomerNumber, sourceData.Company1, sourceData.Postcode), m_LOGFileName)

				m_CustomerData.Add(data)
				m_importedCustomerData.Add(data)

			End If


			Return result
		End Function


#Region "handel more records"

		Private Function UpdateInvalidTwixCustomersWithSourceDatabase() As Boolean
			Dim result As Boolean = True

			m_EventLog.WriteTempLogFile(String.Format("Twix-Import: Programmstart: {0}", Now.ToString), m_LOGFileName)
			m_importedCustomerData = New BindingList(Of CustomerMasterData)

			Dim transiton As Transition = New Transition()
			transiton.Control = nfMain
			transiton.TransitionType = New SlideFadeTransition()
			Dim manager As TransitionManager = New TransitionManager()
			manager.Transitions.Add(transiton)

			manager.StartTransition(nfMain)

			Try
				Dim selectedData = m_invalidCustomerData.Where(Function(x) x.Selected = True)
				If selectedData Is Nothing OrElse selectedData.Count = 0 Then
					m_UtilityUI.ShowErrorDialog("Sie haben keine Datensätze für Kunden-Update ausgewählt!")
					m_EventLog.WriteTempLogFile(String.Format("***Twix-Import: UpdateInvalidTwixCustomersWithSourceDatabase: Keine Daten für Kundenupdate ausgewählt!"), m_LOGFileName)

					Return False
				End If

				For Each customer In selectedData
					result = result AndAlso UpdateAssignedCustomerWithTwixSourceData(customer)

					m_invalidCustomerData(m_invalidCustomerData.IndexOf(customer)).Selected = False
				Next
				If result Then tpCustomer.SelectedPage = tnpImportedCustomer


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				m_EventLog.WriteTempLogFile(String.Format("***Twix-Import: UpdateInvalidTwixCustomersWithSourceDatabase: {0}", ex.ToString), m_LOGFileName)

				Return False

			Finally
				RefreshCustomerModifiedDataSources()
				manager.EndTransition()

				m_EventLog.WriteTempLogFile(String.Format("***Twix-Import: Ende der Update: {0}", Now.ToString), m_LOGFileName)
				m_EventLog.WriteTempLogFile(String.Join("", Enumerable.Repeat("=", 150)), m_LOGFileName)
			End Try

			Return result
		End Function

		Private Function AddInvalidTwixCustomersIntoSourceDatabase() As Boolean
			Dim result As Boolean = True

			m_EventLog.WriteTempLogFile(String.Format("Twix-Import: Programmstart: {0}", Now.ToString), m_LOGFileName)
			m_importedCustomerData = New BindingList(Of CustomerMasterData)

			Dim transiton As Transition = New Transition()
			transiton.Control = nfMain
			transiton.TransitionType = New SlideFadeTransition()
			Dim manager As TransitionManager = New TransitionManager()
			manager.Transitions.Add(transiton)

			manager.StartTransition(nfMain)

			Try
				Dim selectedData = m_invalidCustomerData.Where(Function(x) x.Selected = True)
				If selectedData Is Nothing OrElse selectedData.Count = 0 Then
					m_UtilityUI.ShowErrorDialog("Sie haben keine Datensätze für Kunden-Import ausgewählt!")
					m_EventLog.WriteTempLogFile(String.Format("***Twix-Import: AddInvalidTwixCustomersIntoSourceDatabase: Keine Daten für Kundenimport ausgewählt!"), m_LOGFileName)

					Return False
				End If
				For Each customer In selectedData
					result = result AndAlso AddAssignedTwixCustomerFromSourceData(customer)
					m_invalidCustomerData(m_invalidCustomerData.IndexOf(customer)).Selected = False
				Next
				If result Then tpCustomer.SelectedPage = tnpImportedCustomer


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				m_EventLog.WriteTempLogFile(String.Format("***Twix-Import: AddInvalidTwixCustomersIntoSourceDatabase: {0}", ex.ToString), m_LOGFileName)

				Return False

			Finally
				RefreshCustomerModifiedDataSources()
				manager.EndTransition()

				m_EventLog.WriteTempLogFile(String.Format("***Twix-Import: Ende der Import: {0}", Now.ToString), m_LOGFileName)
				m_EventLog.WriteTempLogFile(String.Join("", Enumerable.Repeat("=", 150)), m_LOGFileName)

			End Try

			Return result
		End Function

#End Region




	End Class

End Namespace
