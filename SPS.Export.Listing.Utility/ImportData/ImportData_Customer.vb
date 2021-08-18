

Imports SP.DatabaseAccess.Listing.DataObjects
Imports DevExpress.XtraEditors.Controls
Imports System.ComponentModel
Imports SP.DatabaseAccess.Customer.DataObjects
Imports DevExpress.Utils.Animation

Namespace UI

	Partial Class frmImportData


		''' <summary>
		''' Gets the selected employee.
		''' </summary>
		''' <returns>The selected customer or nothing if none is selected.</returns>
		Private ReadOnly Property SelectedInvalidCustomerRecord As CustomerMasterViewData
			Get
				Dim gvRP = TryCast(grdInvalidCustomer.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (gvRP Is Nothing) Then

					Dim selectedRows = gvRP.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim customer = CType(gvRP.GetRow(selectedRows(0)), CustomerMasterViewData)
						Return customer
					End If

				End If

				Return Nothing
			End Get

		End Property


		Private Sub OnGVInvalidCustomer_UpdateButtonClick(sender As Object, e As ButtonPressedEventArgs)
			Dim result As Boolean = True

			m_EventLog.WriteTempLogFile(String.Format("Programmstart: {0}", Now.ToString), m_LOGFileName)
			Dim invalidData = SelectedInvalidCustomerRecord
			If invalidData Is Nothing Then Return

			If tgsSputnikTables.EditValue Then
				result = result AndAlso UpdateAssignedCustomerWithSourceData(invalidData)

			Else
				result = result AndAlso UpdateAssignedCustomerWithTwixSourceData(invalidData)

			End If

			If result Then m_invalidCustomerData.Remove(invalidData)
			RefreshCustomerModifiedDataSources()

			m_EventLog.WriteTempLogFile(String.Format("***Ende der Update: {0}", Now.ToString), m_LOGFileName)
			m_EventLog.WriteTempLogFile(String.Join("", Enumerable.Repeat("=", 150)), m_LOGFileName)

		End Sub

		Private Sub OnGVInvalidCustomer_AddButtonClick(sender As Object, e As ButtonPressedEventArgs)
			Dim result As Boolean = True

			m_EventLog.WriteTempLogFile(String.Format("Programmstart: {0}", Now.ToString), m_LOGFileName)
			Dim invalidData = SelectedInvalidCustomerRecord
			If invalidData Is Nothing Then Return

			If tgsSputnikTables.EditValue Then
				result = result AndAlso AddAssignedCustomerFromSourceData(invalidData)

			Else
				result = result AndAlso AddAssignedTwixCustomerFromSourceData(invalidData)

			End If


			If result Then m_invalidCustomerData.Remove(invalidData)
			RefreshCustomerModifiedDataSources()

			m_EventLog.WriteTempLogFile(String.Format("***Ende der Import: {0}", Now.ToString), m_LOGFileName)
			m_EventLog.WriteTempLogFile(String.Join("", Enumerable.Repeat("=", 150)), m_LOGFileName)

		End Sub

		Sub OnInvalidCustomer_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

			'If (e.Clicks = 2) Then

			'	Dim column = e.Column
			'	Dim invalidData = SelectedInvalidCustomerRecord
			'	If invalidData Is Nothing Then Return

			'	Select Case column.Name.ToLower
			'		Case "add"
			'			AddAssignedCustomerFromSourceData(invalidData)

			'		Case "update"
			'			UpdateAssignedCustomerWithSourceData(invalidData)


			'		Case Else
			'			Return

			'	End Select

			'	grdInvalidCustomer.DataSource = m_invalidCustomerData
			'	grdInvalidCustomer.ForceInitialize()

			'	grdImportedCustomer.DataSource = m_importedCustomerData
			'	grdImportedCustomer.ForceInitialize()

			'End If

		End Sub

		Private Function ImportCustomersIntoCurrentDatabase() As Boolean
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


						m_invalidCustomerData.Add(customer)

						m_SourceCustomerData(m_SourceCustomerData.IndexOf(customer)).Selected = False
						customer.Selected = False

						Continue For
					End If
					Dim tranferData = New CustomerTranferData With {.DestCustomerID = m_InitializationData.MDData.MDGuid, .DestMDNumber = m_InitializationData.MDData.MDNr,
						.SourceCustomerNumber = customer.CustomerNumber, .DestCustomerOffsetNumber = offsetNumber, .SourceDataBaseName = m_SourceInitializationData.MDData.MDDbName}

					result = result AndAlso m_ListingDatabaseAccess.AddAssignedCustomerMasterDataFromAnotherDatabase(tranferData)
					If Not result Then m_Logger.LogError(String.Format("error during AddAssignedCustomerMasterDataFromAnotherDatabase. SourceCustomerNumber: {0}", customer.CustomerNumber))
					If Not result Then m_EventLog.WriteTempLogFile(String.Format("***Master konnte nicht übernommen werden: Nummer: {0} >>> Company1: {1} | Postcode: {2}", customer.CustomerNumber, customer.Company1, customer.Postcode), m_LOGFileName)


					result = result AndAlso m_ListingDatabaseAccess.UpdateAssignedCustomerPeripherieDataFromAnotherDatabase(tranferData)
					m_EventLog.WriteTempLogFile(String.Format("Update der (Peripherie) Kundendaten ({0}) {1} {2}: {3}", customer.CustomerNumber, customer.Company1, customer.Postcode, result), m_LOGFileName)




					result = result AndAlso ImportCResponsiblePersonIntoCurrentDatabase(customer.CustomerNumber, tranferData.DestNewCustomerNumber)
					If Not result Then m_EventLog.WriteTempLogFile(String.Format("***Zuständige Personen konnten nicht übernommen werden: SourceCustomerNumber: {0} >>> DestCustomerNumber: {1}",
																				 customer.CustomerNumber, tranferData.DestNewCustomerNumber.GetValueOrDefault(0)), m_LOGFileName)

					If Not result Then
						If tranferData.DestNewCustomerNumber.GetValueOrDefault(0) > 0 Then Dim deleteResult = m_ListingDatabaseAccess.DeleteAssignedImportedCustomerData(tranferData.DestNewCustomerNumber.GetValueOrDefault(0))
						m_EventLog.WriteTempLogFile(String.Format("***Datensatz wurde NICHT erfolgreich importiert: SourceNummer: {0} >>> Company1: {1} | Postcode: {2}", customer.CustomerNumber, customer.Company1, customer.Postcode), m_LOGFileName)
						m_Logger.LogError(String.Format("error during import. DestNewCustomerNumber: {0}", tranferData.DestNewCustomerNumber.GetValueOrDefault(0)))

						'm_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Fehler während Import von Kunden Daten. DestNewCustomerNumber: {0}. Der Vorgang wird abgebrochen."), tranferData.DestNewCustomerNumber.GetValueOrDefault(0)))

						' should not return false!
						Continue For
					Else
						Dim data = m_CustomerDatabaseAccess.LoadCustomerMasterData(tranferData.DestNewCustomerNumber.GetValueOrDefault(0), String.Empty)
						m_EventLog.WriteTempLogFile(String.Format("Datensatz wurde erfolgreich importiert: Neue Nummer: {0} >>> Company1: {1} | Postcode: {2}",
																  tranferData.DestNewCustomerNumber.GetValueOrDefault(0), customer.Company1, customer.Postcode), m_LOGFileName)

						m_CustomerData.Add(data)
						m_importedCustomerData.Add(data)

						m_SourceCustomerData(m_SourceCustomerData.IndexOf(customer)).Selected = False
						customer.Selected = False
					End If
				Next
				grdSourceCustomer.RefreshDataSource()


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				m_EventLog.WriteTempLogFile(String.Format("ImportCustomersIntoCurrentDatabase: {0}", ex.ToString), m_LOGFileName)

				Return False

			Finally
				RefreshCustomerModifiedDataSources()
				manager.EndTransition()
			End Try

			Return result
		End Function

		Private Function ImportCustomersPeripherieDataIntoCurrentDatabase() As Boolean
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
				m_Logger.LogInfo(String.Format("starting Importing customer peripherie data. DestCustomerTable: {0} >>> SourceDatabase: {1}", m_CustomerData.Count, m_SourceInitializationData.MDData.MDDbName))

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


						result = result AndAlso UpdateAssignedCustomerPeripherieWithSourceData(customer)
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

		Private Function UpdateAssignedCustomerPeripherieWithSourceData(ByVal sourceData As CustomerMasterViewData) As Boolean
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

		Private Function UpdateInvalidCustomersWithSourceDatabase() As Boolean
			Dim result As Boolean = True

			m_EventLog.WriteTempLogFile(String.Format("Programmstart: {0}", Now.ToString), m_LOGFileName)
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
					m_EventLog.WriteTempLogFile(String.Format("UpdateInvalidEmployeesWithSourceDatabase: Keine Daten für Kundenupdate ausgewählt!"), m_LOGFileName)

					Return False
				End If

				For Each customer In selectedData
					result = result AndAlso UpdateAssignedCustomerWithSourceData(customer)

					m_invalidCustomerData(m_invalidCustomerData.IndexOf(customer)).Selected = False
				Next
				If result Then tpCustomer.SelectedPage = tnpImportedCustomer


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				m_EventLog.WriteTempLogFile(String.Format("UpdateInvalidCustomerssWithSourceDatabase: {0}", ex.ToString), m_LOGFileName)

				Return False

			Finally
				RefreshCustomerModifiedDataSources()
				manager.EndTransition()

				m_EventLog.WriteTempLogFile(String.Format("***Ende der Update: {0}", Now.ToString), m_LOGFileName)
				m_EventLog.WriteTempLogFile(String.Join("", Enumerable.Repeat("=", 150)), m_LOGFileName)
			End Try

			Return result
		End Function

		Private Function AddInvalidCustomersIntoSourceDatabase() As Boolean
			Dim result As Boolean = True

			m_EventLog.WriteTempLogFile(String.Format("Programmstart: {0}", Now.ToString), m_LOGFileName)
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
					m_EventLog.WriteTempLogFile(String.Format("AddInvalidCustomersIntoSourceDatabase: Keine Daten für Kundenimport ausgewählt!"), m_LOGFileName)

					Return False
				End If
				For Each customer In selectedData
					result = result AndAlso AddAssignedCustomerFromSourceData(customer)
					m_invalidCustomerData(m_invalidCustomerData.IndexOf(customer)).Selected = False
				Next
				If result Then tpCustomer.SelectedPage = tnpImportedCustomer


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				m_EventLog.WriteTempLogFile(String.Format("AddInvalidCustomersIntoSourceDatabase: {0}", ex.ToString), m_LOGFileName)

				Return False

			Finally
				RefreshCustomerModifiedDataSources()
				manager.EndTransition()

				m_EventLog.WriteTempLogFile(String.Format("***Ende der Import: {0}", Now.ToString), m_LOGFileName)
				m_EventLog.WriteTempLogFile(String.Join("", Enumerable.Repeat("=", 150)), m_LOGFileName)

			End Try

			Return result
		End Function

		Private Function RefreshCustomerModifiedDataSources() As Boolean
			Dim result As Boolean = True

			grdInvalidCustomer.RefreshDataSource()

			grdInvalidCustomer.DataSource = m_invalidCustomerData
			grdInvalidCustomer.ForceInitialize()

			grdImportedCustomer.DataSource = m_importedCustomerData
			grdImportedCustomer.ForceInitialize()

			Return result
		End Function

		Private Function UpdateAssignedCustomerWithSourceData(ByVal sourceData As CustomerMasterViewData) As Boolean
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

			result = result AndAlso m_CustomerDatabaseAccess.UpdateCustomerMasterData(sourceMasterData)
			m_EventLog.WriteTempLogFile(String.Format("Update der (Stamm) Kundendaten ({0}) {1} {2}: {3}", sourceMasterData.CustomerNumber, sourceMasterData.Company1, sourceMasterData.Postcode, result), m_LOGFileName)


			Dim tranferData = New CustomerTranferData With {.DestCustomerID = m_InitializationData.MDData.MDGuid, .DestMDNumber = m_InitializationData.MDData.MDNr,
						.SourceCustomerNumber = sourceData.CustomerNumber, .DestNewCustomerNumber = sourceMasterData.CustomerNumber, .SourceDataBaseName = m_SourceInitializationData.MDData.MDDbName}

			result = result AndAlso m_ListingDatabaseAccess.UpdateAssignedCustomerPeripherieDataFromAnotherDatabase(tranferData)
			m_EventLog.WriteTempLogFile(String.Format("Update der (Peripherie) Kundendaten ({0}) {1} {2}: {3}", sourceMasterData.CustomerNumber, sourceMasterData.Company1, sourceMasterData.Postcode, result), m_LOGFileName)

			result = result AndAlso UpdateCResponsiblePersonWithSourceData(sourceData.CustomerNumber, searchCustomer.CustomerNumber)
			m_EventLog.WriteTempLogFile(String.Format("Update der (Peripherie) Zustaändige Personendaten ({0}) {1} {2}: {3}", sourceData.CustomerNumber, sourceMasterData.Company1, sourceMasterData.Postcode, result), m_LOGFileName)


			If result Then
				m_importedCustomerData.Add(sourceMasterData)
				'm_invalidCustomerData.Remove(sourceData)

			Else
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ihre Daten konnten nicht importiert werden."))

				Return False
			End If


			Return result
		End Function

		Private Function AddAssignedCustomerFromSourceData(ByVal sourceData As CustomerMasterViewData) As Boolean
			Dim result As Boolean = True
			Dim offsetNumber As Integer = ReadCustomerOffsetFromSettings()

			' read source data
			Dim sourceMasterData = m_SourceCustomerDatabaseAccess.LoadCustomerMasterData(sourceData.CustomerNumber, True)
			Dim tranferData = New CustomerTranferData With {.DestCustomerID = m_InitializationData.MDData.MDGuid, .DestMDNumber = m_InitializationData.MDData.MDNr,
						.SourceCustomerNumber = sourceData.CustomerNumber, .DestCustomerOffsetNumber = offsetNumber, .SourceDataBaseName = m_SourceInitializationData.MDData.MDDbName}

			result = result AndAlso m_ListingDatabaseAccess.AddAssignedCustomerMasterDataFromAnotherDatabase(tranferData)
			result = result AndAlso ImportCResponsiblePersonIntoCurrentDatabase(tranferData.SourceCustomerNumber, tranferData.DestNewCustomerNumber)
			m_EventLog.WriteTempLogFile(String.Format("Hinzufügen der (Source) Kundendaten ({0}): {1}", tranferData.SourceCustomerNumber, result), m_LOGFileName)

			If Not result Then
				Dim deleteResult = m_ListingDatabaseAccess.DeleteAssignedImportedCustomerData(tranferData.DestNewCustomerNumber)
				m_Logger.LogError(String.Format("AddAssignedCustomerFromSourceData was not successfull:{0}SourceCustomerNumber: {1} | sourcePostcode: {2}", vbNewLine, sourceData.CustomerNumber, sourceMasterData.Postcode))
				m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Fehler während Import von Kunden Daten. SourceCustomerNumber: {0}. Der Vorgang wird abgebrochen."), sourceData.CustomerNumber))

				Return False

			Else
				m_CustomerData.Add(sourceMasterData)
				m_importedCustomerData.Add(sourceMasterData)

			End If


			Return result
		End Function

		Private Function UpdateCResponsiblePersonWithSourceData(ByVal sourceCustomerNumber As Integer, ByVal destCustomerNumber As Integer) As Boolean
			Dim result As Boolean = True

			Try
				Dim sourceResponsiblePersonData = m_SourceCustomerDatabaseAccess.LoadResponsiblePersonData(sourceCustomerNumber)
				Dim descResponsiblePersonData = m_CustomerDatabaseAccess.LoadResponsiblePersonData(destCustomerNumber)
				m_EventLog.WriteTempLogFile(String.Format("Update der Zuständige Personendaten. sourceCustomerNumber: {0} destCustomerNumber: {1}", sourceCustomerNumber, destCustomerNumber), m_LOGFileName)

				For Each person In sourceResponsiblePersonData
					Dim searchData = descResponsiblePersonData.Where(Function(x) x.Firstname = person.Firstname And x.Lastname = person.Lastname).FirstOrDefault

					If searchData Is Nothing Then
						m_EventLog.WriteTempLogFile(String.Format("Zuständige Personendaten nicht gefunden wird neu angelegt. Firstname: {0} Lastname: {1}", person.Firstname, person.Lastname), m_LOGFileName)

						result = result AndAlso ImportCResponsiblePersonIntoCurrentDatabase(sourceCustomerNumber, destCustomerNumber)
						m_Logger.LogWarning(String.Format("destcResponsible was not founded and will be added:{0}sourceCustomerNumber: {1} | destCustomerNumber: {2}", vbNewLine, sourceCustomerNumber, destCustomerNumber))

						Continue For
					End If

					Dim searchMasterData = m_SourceCustomerDatabaseAccess.LoadResponsiblePersonMasterData(sourceCustomerNumber, person.RecordNumber)

					searchMasterData.CustomerNumber = destCustomerNumber
					searchMasterData.RecordNumber = searchData.RecordNumber

					result = result AndAlso m_CustomerDatabaseAccess.UpdateResponsiblePersonMasterData(searchMasterData)
					m_EventLog.WriteTempLogFile(String.Format("Update der (Master) Zuständige Personendaten. Firstname: {0} Lastname: {1} >>> {2}", person.Firstname, person.Lastname, result), m_LOGFileName)


					Dim tranferCResponsibleData = New CResponsiblePersonTranferData With {.DestCustomerID = m_InitializationData.MDData.MDGuid, .DestMDNumber = m_InitializationData.MDData.MDNr,
						.SourceCustomerNumber = sourceCustomerNumber, .SourceCResponsibleNumber = person.RecordNumber, .DestCustomerNumber = destCustomerNumber,
						.DestNewCResponsibleNumber = searchData.RecordNumber, .SourceDataBaseName = m_SourceInitializationData.MDData.MDDbName}

					result = result AndAlso m_ListingDatabaseAccess.UpdateAssignedCResponsiblePeripherieDataFromAnotherDatabase(tranferCResponsibleData)
					m_EventLog.WriteTempLogFile(String.Format("Update der (Peripherie) Zuständige Personendaten. Firstname: {0} Lastname: {1} >>> {2}", person.Firstname, person.Lastname, result), m_LOGFileName)

				Next


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				m_EventLog.WriteTempLogFile(String.Format("UpdateCResponsiblePersonWithSourceData: {0}", ex.ToString), m_LOGFileName)

				Return False
			End Try

			Return result
		End Function

		Private Function ImportCResponsiblePersonIntoCurrentDatabase(ByVal sourceCustomerNumber As Integer, ByVal destCustomerNumber As Integer) As Boolean
			Dim result As Boolean = True

			Try
				Dim sourceResponsiblePersonData = m_SourceCustomerDatabaseAccess.LoadResponsiblePersonData(sourceCustomerNumber)
				For Each person In sourceResponsiblePersonData

					Dim tranferCResponsibleData = New CResponsiblePersonTranferData With {.DestCustomerID = m_InitializationData.MDData.MDGuid, .DestMDNumber = m_InitializationData.MDData.MDNr,
						.SourceCustomerNumber = sourceCustomerNumber, .SourceCResponsibleNumber = person.RecordNumber, .DestCustomerNumber = destCustomerNumber, .SourceDataBaseName = m_SourceInitializationData.MDData.MDDbName}

					result = result AndAlso m_ListingDatabaseAccess.AddAssignedCResponsibleDataFromAnotherDatabase(tranferCResponsibleData)
					m_EventLog.WriteTempLogFile(String.Format("Hinzufügen der (Source) Zuständige Personen. sourceCustomerNumber: {0} >>> destCustomerNumber: {1}: {2}", sourceCustomerNumber, destCustomerNumber, result), m_LOGFileName)

					If Not result Then m_Logger.LogError(String.Format("AddAssignedCResponsibleDataFromAnotherDatabase was not successfull. sourceCustomerNumber: {0} | SourceCResponsibleNumber: {1} >>> destCustomerNumber: {2}",
																	   sourceCustomerNumber, person.RecordNumber, destCustomerNumber))

					result = result AndAlso m_ListingDatabaseAccess.AddAssignedCResponsiblePeripherieDataFromAnotherDatabase(tranferCResponsibleData)
					m_EventLog.WriteTempLogFile(String.Format("Hinzufügen der (Peripherie) Zuständige Personen ({0}). sourceCustomerNumber: {1} >>> destCustomerNumber: {2}: {3}", tranferCResponsibleData.DestNewCResponsibleNumber, sourceCustomerNumber, destCustomerNumber, result), m_LOGFileName)

				Next


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				m_EventLog.WriteTempLogFile(String.Format("ImportCResponsiblePersonIntoCurrentDatabase: {0}", ex.ToString), m_LOGFileName)

				Return False
			End Try

			Return result
		End Function


	End Class


End Namespace
