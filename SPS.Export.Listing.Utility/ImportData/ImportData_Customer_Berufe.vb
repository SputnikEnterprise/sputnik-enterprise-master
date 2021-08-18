
Imports SP.DatabaseAccess.Listing.DataObjects
Imports System.ComponentModel
Imports SP.DatabaseAccess.Customer.DataObjects
Imports DevExpress.Utils.Animation


Namespace UI


	Partial Class frmImportData


		Private Function ImportCustomersBerufeIntoCurrentDatabase_2() As Boolean
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
				m_EventLog.WriteTempLogFile(String.Format("Ziel-Kundendatan {0}", m_CustomerData.Count), m_LOGFileName)
				m_Logger.LogInfo(String.Format("starting import. DestCustomerTable: {0} >>> SourceDatabase: {1}", m_CustomerData.Count, m_SourceInitializationData.MDData.MDDbName))

				For Each customer In m_SourceCustomerData.Where(Function(x) x.Selected = True)
					result = True
					m_EventLog.WriteTempLogFile(String.Format("Es wird gesucht nach: Company1: {0} | Postcode: {1}", customer.Company1, customer.Postcode), m_LOGFileName)

					Dim searchCustomer As CustomerMasterData
					Try
						searchCustomer = m_CustomerData.Where(Function(x) x.Company1 = customer.Company1 And x.Postcode = customer.Postcode).FirstOrDefault

					Catch ex As Exception
						m_EventLog.WriteTempLogFile(String.Format("Source Customer wird übersprungen ({0}): Company1: {1} | Postcode: {2} >>> {3}", customer.CustomerNumber, customer.Company1, customer.Postcode, ex.ToString), m_LOGFileName)
						Continue For

					End Try

					If Not searchCustomer Is Nothing Then
						m_EventLog.WriteTempLogFile(String.Format("wurde als Duplikat gefunden: Nummer: {0} >>> Company1: {1} | Postcode: {2}", searchCustomer.CustomerNumber, customer.Company1, customer.Postcode), m_LOGFileName)

						customer.SourceChangedOn = customer.ChangedOn
						customer.SourceCreatedOn = customer.CreatedOn
						customer.DestChangedOn = searchCustomer.ChangedOn
						customer.DestCreatedOn = searchCustomer.CreatedOn
						customer.DestCompleteAddress = searchCustomer.CustomerCompleteAddress


						result = result AndAlso UpdateAssignedCustomerPeripherieWithSourceData_2(customer)
						m_EventLog.WriteTempLogFile(String.Format("Hinzufügen der (Peripherie): sourceCustomerNumber: {0} >>> destCustomerNumber: {1}: {2}", customer.CustomerNumber, searchCustomer.CustomerNumber, result), m_LOGFileName)


						m_invalidCustomerData.Add(customer)

						m_SourceCustomerData(m_SourceCustomerData.IndexOf(customer)).Selected = False
						customer.Selected = False

						Continue For
					End If
				Next
				grdSourceCustomer.RefreshDataSource()


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				m_EventLog.WriteTempLogFile(String.Format("ImportCustomersBerufeIntoCurrentDatabase: {0}", ex.ToString), m_LOGFileName)

				Return False

			Finally
				RefreshCustomerModifiedDataSources()
				manager.EndTransition()
			End Try

			Return result
		End Function

		Private Function UpdateAssignedCustomerPeripherieWithSourceData_2(ByVal sourceData As CustomerMasterViewData) As Boolean
			Dim result As Boolean = True

			' read source data
			Dim sourceMasterData = m_SourceCustomerDatabaseAccess.LoadCustomerMasterData(sourceData.CustomerNumber, True)
			m_EventLog.WriteTempLogFile(String.Format("Update der Kundendaten ({0}) >>> {1} {2}", sourceData.CustomerNumber, sourceData.Company1, sourceData.Postcode), m_LOGFileName)


			' writing destination data
			Dim searchCustomer = m_CustomerData.Where(Function(x) x.Company1 = sourceMasterData.Company1 And x.Postcode = sourceMasterData.Postcode).FirstOrDefault
			m_EventLog.WriteTempLogFile(String.Format("Update der Kundendaten {0} {1}", sourceMasterData.Company1, sourceMasterData.Postcode), m_LOGFileName)

			If searchCustomer Is Nothing Then
				m_EventLog.WriteTempLogFile(String.Format("Kundendaten nicht gefunden! {0} {1}", sourceMasterData.Company1, sourceMasterData.Postcode), m_LOGFileName)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Zieldaten wurden nicht gefunden. Der Vorgang wird abgebrochen."))
				m_Logger.LogWarning(String.Format("destCustomer was not founded:{0}sourceCompany1: {1} | sourcePostcode: {2}", vbNewLine, sourceMasterData.Company1, sourceMasterData.Postcode))

				Return False
			End If
			sourceMasterData.CustomerNumber = searchCustomer.CustomerNumber

			sourceMasterData.Transfered_Guid = String.Empty
			sourceMasterData.CustomerMandantNumber = m_InitializationData.MDData.MDNr
			sourceMasterData.WOSGuid = String.Empty

			Dim tranferData = New CustomerTranferData With {.DestCustomerID = m_InitializationData.MDData.MDGuid, .DestMDNumber = m_InitializationData.MDData.MDNr,
						.SourceCustomerNumber = sourceData.CustomerNumber, .DestNewCustomerNumber = searchCustomer.CustomerNumber, .SourceDataBaseName = m_SourceInitializationData.MDData.MDDbName}

			result = result AndAlso m_ListingDatabaseAccess.UpdateAssignedCustomerPeripherieDataFromAnotherDatabase(tranferData)
			m_EventLog.WriteTempLogFile(String.Format("Update der (Peripherie) Kundendaten ({0}) {1} {2}: {3}", sourceMasterData.CustomerNumber, sourceMasterData.Company1, sourceMasterData.Postcode, result), m_LOGFileName)


			If result Then
				m_importedCustomerData.Add(sourceMasterData)

			Else
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ihre Daten konnten nicht importiert werden."))

				Return False
			End If


			Return result
		End Function


	End Class


End Namespace
