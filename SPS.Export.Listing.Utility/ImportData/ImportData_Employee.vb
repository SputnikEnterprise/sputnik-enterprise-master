

Imports SP.DatabaseAccess.Listing.DataObjects
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Employee.DataObjects.Salary
Imports DevExpress.XtraEditors.Controls
Imports System.ComponentModel
Imports DevExpress.Utils.Animation

Namespace UI

	Partial Class frmImportData


		''' <summary>
		''' Gets the selected employee.
		''' </summary>
		''' <returns>The selected employee or nothing if none is selected.</returns>
		Private ReadOnly Property SelectedInvalidEmployeeRecord As EmployeeMasterViewData
			Get
				Dim gvRP = TryCast(grdInvalidEmployee.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (gvRP Is Nothing) Then

					Dim selectedRows = gvRP.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim employee = CType(gvRP.GetRow(selectedRows(0)), EmployeeMasterViewData)
						Return employee
					End If

				End If

				Return Nothing
			End Get

		End Property


		Private Sub OnGVInvalidEmployee_UpdateButtonClick(sender As Object, e As ButtonPressedEventArgs)
			Dim result As Boolean = True

			m_EventLog.WriteTempLogFile(String.Format("Programmstart: {0}", Now.ToString), m_LOGFileName)

			Dim invalidData = SelectedInvalidEmployeeRecord
			If invalidData Is Nothing Then Return

			result = result AndAlso UpdateAssignedEmployeeWithSourceData(invalidData)
			If result Then m_invalidEmployeeData.Remove(invalidData)

			RefreshEmployeeModifiedDataSources()

			m_EventLog.WriteTempLogFile(String.Format("***Ende der Update: {0}", Now.ToString), m_LOGFileName)
			m_EventLog.WriteTempLogFile(String.Join("", Enumerable.Repeat("=", 150)), m_LOGFileName)

		End Sub

		Private Sub OnGVInvalidEmployee_AddButtonClick(sender As Object, e As ButtonPressedEventArgs)
			Dim result As Boolean = True

			m_EventLog.WriteTempLogFile(String.Format("Programmstart: {0}", Now.ToString), m_LOGFileName)
			Dim invalidData = SelectedInvalidEmployeeRecord
			If invalidData Is Nothing Then Return

			result = result AndAlso AddAssignedEmployeeFromSourceData(invalidData)
			m_invalidEmployeeData.Remove(invalidData)
			RefreshEmployeeModifiedDataSources()

			m_EventLog.WriteTempLogFile(String.Format("***Ende der Import: {0}", Now.ToString), m_LOGFileName)
			m_EventLog.WriteTempLogFile(String.Join("", Enumerable.Repeat("=", 150)), m_LOGFileName)

		End Sub


		Sub OnInvalidEmployee_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

			'Dim column = e.Column
			'Dim invalidData = SelectedInvalidEmployeeRecord
			'If invalidData Is Nothing Then Return

			'Select Case column.Name.ToLower
			'	Case "add"
			'		AddAssignedEmployeeFromSourceData(invalidData)

			'	Case "update"
			'		UpdateAssignedEmployeeWithSourceData(invalidData)


			'	Case Else
			'		Return

			'End Select

			'grdInvalidEmployee.DataSource = m_invalidEmployeeData
			'grdInvalidEmployee.ForceInitialize()

			'grdImportedEmployee.DataSource = m_importedEmployeeData
			'grdImportedEmployee.ForceInitialize()

		End Sub

		Private Function ImportEmployeesIntoCurrentDatabase() As Boolean
			Dim result As Boolean = True
			Dim offsetNumber As Integer = ReadEmployeeOffsetFromSettings()
			m_invalidEmployeeData = New BindingList(Of EmployeeMasterViewData)
			m_importedEmployeeData = New BindingList(Of EmployeeMasterData)

			Dim transiton As Transition = New Transition()
			transiton.Control = nfMain
			transiton.TransitionType = New SlideFadeTransition()
			Dim manager As TransitionManager = New TransitionManager()
			manager.Transitions.Add(transiton)

			manager.StartTransition(nfMain)

			Try
				result = result AndAlso LoadDestEmployeeData()
				m_EventLog.WriteTempLogFile(String.Format("Ziel-Kandidatendaten {0}", m_EmployeeData.Count), m_LOGFileName)
				m_Logger.LogInfo(String.Format("starting import. DestEmployeeTable: {0} >>> SourceDatabase: {1}", m_EmployeeData.Count, m_SourceInitializationData.MDData.MDDbName))

				For Each employee In m_SourceEmployeeData.Where(Function(x) x.Selected = True)
					result = True
					m_EventLog.WriteTempLogFile(String.Format("Es wird gesucht nach: Vorname: {0} | Nachname: {1}", employee.Firstname, employee.Lastname), m_LOGFileName)
					Dim searchEmployee As EmployeeMasterData
					Try
						searchEmployee = m_EmployeeData.Where(Function(x) x.Firstname = employee.Firstname And x.Lastname = employee.Lastname And x.Birthdate = employee.Birthdate).FirstOrDefault

					Catch ex As Exception
						m_EventLog.WriteTempLogFile(String.Format("Source Employee wird übersprungen ({0}): Firstname: {1} | Lastname: {2} >>> {3}", employee.EmployeeNumber, employee.Firstname, employee.Lastname, ex.ToString), m_LOGFileName)
						Continue For

					End Try

					If Not searchEmployee Is Nothing Then
						m_EventLog.WriteTempLogFile(String.Format("wurde als Duplikat gefunden: Nummer: {0} >>> Vorname: {1} | Nachname: {2}", searchEmployee.EmployeeNumber, employee.Firstname, employee.Lastname), m_LOGFileName)

						employee.SourceChangedOn = employee.ChangedOn
						employee.SourceCreatedOn = employee.CreatedOn
						employee.DestChangedOn = searchEmployee.ChangedOn
						employee.DestCreatedOn = searchEmployee.CreatedOn
						employee.DestCompleteAddress = searchEmployee.EmployeeCompleteAddress

						m_invalidEmployeeData.Add(employee)

						m_SourceEmployeeData(m_SourceEmployeeData.IndexOf(employee)).Selected = False
						employee.Selected = False

						Continue For
					End If
					Dim tranferData = New EmployeeTranferData With {.DestCustomerID = m_InitializationData.MDData.MDGuid, .DestMDNumber = m_InitializationData.MDData.MDNr,
						.SourceEmployeeNumber = employee.EmployeeNumber, .DestEmployeeOffsetNumber = offsetNumber,
						.SourceDataBaseName = m_SourceInitializationData.MDData.MDDbName}

					result = result AndAlso m_ListingDatabaseAccess.AddAssignedEmployeeMasterDataFromAnotherDatabase(tranferData)
					If Not result Then m_EventLog.WriteTempLogFile(String.Format("***Master konnte nicht übernommen werden: Nummer: {0} >>> Vorname: {1} | Nachname: {2}", employee.EmployeeNumber, employee.Firstname, employee.Lastname), m_LOGFileName)

					result = result AndAlso m_ListingDatabaseAccess.AddAssignedEmployeePeripherieDataFromAnotherDatabase(tranferData)
					If Not result Then m_EventLog.WriteTempLogFile(String.Format("***Peripherie konnte nicht übernommen werden: Nummer: {0} >>> Vorname: {1} | Nachname: {2}", employee.EmployeeNumber, employee.Firstname, employee.Lastname), m_LOGFileName)

					If Not result Then
						m_EventLog.WriteTempLogFile(String.Format("***Datensatz wurde NICHT erfolgreich importiert: SourceNummer: {0} >>> Vorname: {1} | Nachname: {2}", employee.EmployeeNumber, employee.Firstname, employee.Lastname), m_LOGFileName)

						If tranferData.DestNewEmployeeNumber.GetValueOrDefault(0) > 0 Then Dim deleteResult = m_ListingDatabaseAccess.DeleteAssignedImportedEmployeeData(tranferData.DestNewEmployeeNumber.GetValueOrDefault(0))
						m_Logger.LogError(String.Format("error during import. SourceEmployeeNumber: {0} >>> DestNewEmployeeNumber: {1}", employee.EmployeeNumber, tranferData.DestNewEmployeeNumber.GetValueOrDefault(0)))

						'm_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Fehler während Import von Kandidaten Daten. SourceEmployeeNumber: {0} >>> DestNewEmployeeNumber: {1}. Der Vorgang wird abgebrochen."),
						'employee.EmployeeNumber, tranferData.DestNewEmployeeNumber.GetValueOrDefault(0)))

						' should not return false!
						Continue For
					Else
						Dim data = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(tranferData.DestNewEmployeeNumber, False)
						m_EventLog.WriteTempLogFile(String.Format("Datensatz wurde erfolgreich importiert: Neue Nummer: {0} >>> Vorname: {1} | Nachname: {2}", tranferData.DestNewEmployeeNumber, employee.Firstname, employee.Lastname), m_LOGFileName)

						m_EmployeeData.Add(data)
						m_importedEmployeeData.Add(data)

						m_SourceEmployeeData(m_SourceEmployeeData.IndexOf(employee)).Selected = False
						employee.Selected = False

					End If
				Next
				grdSourceEmployee.RefreshDataSource()


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				m_EventLog.WriteTempLogFile(String.Format("ImportEmployeesIntoCurrentDatabase: {0}", ex.ToString), m_LOGFileName)

				Return False

			Finally
				RefreshEmployeeModifiedDataSources()
				manager.EndTransition()
			End Try

			Return result
		End Function

		Private Function UpdateInvalidEmployeesWithSourceDatabase() As Boolean
			Dim result As Boolean = True

			m_EventLog.WriteTempLogFile(String.Format("Programmstart: {0}", Now.ToString), m_LOGFileName)
			m_importedEmployeeData = New BindingList(Of EmployeeMasterData)

			Dim transiton As Transition = New Transition()
			transiton.Control = nfMain
			transiton.TransitionType = New SlideFadeTransition()
			Dim manager As TransitionManager = New TransitionManager()
			manager.Transitions.Add(transiton)

			manager.StartTransition(nfMain)

			Try
				Dim selectedData = m_invalidEmployeeData.Where(Function(x) x.Selected = True)
				If selectedData Is Nothing OrElse selectedData.Count = 0 Then
					m_UtilityUI.ShowErrorDialog("Sie haben keine Datensätze für Kandidaten-Update ausgewählt!")
					m_EventLog.WriteTempLogFile(String.Format("UpdateInvalidEmployeesWithSourceDatabase: Keine Daten für Kandidatenupdate ausgewählt!"), m_LOGFileName)

					Return False
				End If
				For Each employee In selectedData
					result = result AndAlso UpdateAssignedEmployeeWithSourceData(employee)
					m_invalidEmployeeData(m_invalidEmployeeData.IndexOf(employee)).Selected = False
				Next
				If result Then tpEmployee.SelectedPage = tnpImportedEmployee


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				m_EventLog.WriteTempLogFile(String.Format("UpdateInvalidEmployeesWithSourceDatabase: {0}", ex.ToString), m_LOGFileName)

				Return False

			Finally
				RefreshEmployeeModifiedDataSources()
				manager.EndTransition()

				m_EventLog.WriteTempLogFile(String.Format("***Ende der Update: {0}", Now.ToString), m_LOGFileName)
				m_EventLog.WriteTempLogFile(String.Join("", Enumerable.Repeat("=", 150)), m_LOGFileName)
			End Try

			Return result
		End Function

		Private Function AddInvalidEmployeesIntoSourceDatabase() As Boolean
			Dim result As Boolean = True

			m_EventLog.WriteTempLogFile(String.Format("***Ende der Import: {0}", Now.ToString), m_LOGFileName)
			m_importedEmployeeData = New BindingList(Of EmployeeMasterData)

			Dim transiton As Transition = New Transition()
			transiton.Control = nfMain
			transiton.TransitionType = New SlideFadeTransition()
			Dim manager As TransitionManager = New DevExpress.Utils.Animation.TransitionManager()
			manager.Transitions.Add(transiton)

			manager.StartTransition(nfMain)

			Try
				Dim selectedData = m_invalidEmployeeData.Where(Function(x) x.Selected = True)
				If selectedData Is Nothing OrElse selectedData.Count = 0 Then
					m_UtilityUI.ShowErrorDialog("Sie haben keine Datensätze für Kandidaten-Import ausgewählt!")
					m_EventLog.WriteTempLogFile(String.Format("UpdateInvalidEmployeesWithSourceDatabase: Keine Daten für Kandidatenimport ausgewählt!"), m_LOGFileName)

					Return False
				End If
				For Each employee In selectedData
					result = result AndAlso AddAssignedEmployeeFromSourceData(employee)
					m_invalidEmployeeData(m_invalidEmployeeData.IndexOf(employee)).Selected = False
				Next
				If result Then tpEmployee.SelectedPage = tnpImportedEmployee


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				m_EventLog.WriteTempLogFile(String.Format("AddInvalidEmployeesIntoSourceDatabase: {0}", ex.ToString), m_LOGFileName)

				Return False

			Finally
				RefreshEmployeeModifiedDataSources()
				manager.EndTransition()

				m_EventLog.WriteTempLogFile(String.Format("***Ende der Import: {0}", Now.ToString), m_LOGFileName)
				m_EventLog.WriteTempLogFile(String.Join("", Enumerable.Repeat("=", 150)), m_LOGFileName)
			End Try

			Return result
		End Function

		Private Function RefreshEmployeeModifiedDataSources() As Boolean
			Dim result As Boolean = True

			grdInvalidEmployee.RefreshDataSource()

			grdInvalidEmployee.DataSource = m_invalidEmployeeData
			grdInvalidEmployee.ForceInitialize()

			grdImportedEmployee.DataSource = m_importedEmployeeData
			grdImportedEmployee.ForceInitialize()

			Return result
		End Function

		Private Function UpdateAssignedEmployeeWithSourceData(ByVal sourceData As EmployeeMasterViewData) As Boolean
			Dim result As Boolean = True

			' read source data
			Dim sourceMasterData = m_SourceEmployeeDatabaseAccess.LoadEmployeeMasterData(sourceData.EmployeeNumber, True)
			Dim employeeOtherData As EmployeeOtherData = m_SourceEmployeeDatabaseAccess.LoadEmployeeOtherData(sourceData.EmployeeNumber)
			Dim employeeContactCommData As EmployeeContactComm = m_SourceEmployeeDatabaseAccess.LoadEmployeeContactCommData(sourceData.EmployeeNumber)
			Dim employeeLOSettingData As EmployeeLOSettingsData = m_SourceEmployeeDatabaseAccess.LoadEmployeeLOSettings(sourceData.EmployeeNumber)


			' writing destination data
			Dim searchEmployee = m_EmployeeData.Where(Function(x) x.Firstname = sourceMasterData.Firstname And x.Lastname = sourceMasterData.Lastname And x.Birthdate = sourceMasterData.Birthdate).FirstOrDefault
			m_EventLog.WriteTempLogFile(String.Format("Update der Kandidatendaten {0} {1}", sourceMasterData.Firstname, sourceMasterData.Lastname), m_LOGFileName)

			If searchEmployee Is Nothing Then
				m_EventLog.WriteTempLogFile(String.Format("Kandidatendaten nicht gefunden! {0} {1}", sourceMasterData.Firstname, sourceMasterData.Lastname), m_LOGFileName)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Zieldaten wurden nicht gefunden. Der Vorgang wird abgebrochen."))
				m_Logger.LogWarning(String.Format("destEmployee was not founded:{0}Firstname: {1} | Lastname: {2} | Birthdate: {3}",
												  vbNewLine, sourceMasterData.Firstname, sourceMasterData.Lastname, sourceMasterData.Birthdate))

				Return False
			End If
			sourceMasterData.EmployeeNumber = searchEmployee.EmployeeNumber
			employeeOtherData.EmployeeNumber = searchEmployee.EmployeeNumber
			employeeContactCommData.EmployeeNumber = searchEmployee.EmployeeNumber
			employeeLOSettingData.EmployeeNumber = searchEmployee.EmployeeNumber

			sourceMasterData.ShowAsApplicant = False
			sourceMasterData.Transfered_Guid = String.Empty
			sourceMasterData.Transfered_On = Nothing
			sourceMasterData.Transfered_User = String.Empty
			sourceMasterData.MDNr = m_InitializationData.MDData.MDNr
			sourceMasterData.WOSGuid = String.Empty
			sourceMasterData.ApplicantID = Nothing
			sourceMasterData.ApplicantLifecycle = Nothing
			sourceMasterData.CVLProfileID = Nothing

			result = result AndAlso m_EmployeeDatabaseAccess.UpdateEmployeeMasterData(sourceMasterData)
			result = result AndAlso m_EmployeeDatabaseAccess.UpdateEmployeeOtherData(employeeOtherData)
			result = result AndAlso m_EmployeeDatabaseAccess.UpdateEmployeeConactCommData(employeeContactCommData)
			result = result AndAlso m_EmployeeDatabaseAccess.UpdateEmployeeLOSettings(employeeLOSettingData)
			m_EventLog.WriteTempLogFile(String.Format("Update der (Stamm) Kandidatendaten ({0}) {1} {2}: {3}", searchEmployee.EmployeeNumber, sourceMasterData.Firstname, sourceMasterData.Lastname, result), m_LOGFileName)


			Dim tranferData = New EmployeeTranferData With {.DestCustomerID = m_InitializationData.MDData.MDGuid, .DestMDNumber = m_InitializationData.MDData.MDNr,
						.SourceEmployeeNumber = sourceData.EmployeeNumber, .DestNewEmployeeNumber = searchEmployee.EmployeeNumber, .SourceDataBaseName = m_SourceInitializationData.MDData.MDDbName}

			result = result AndAlso m_ListingDatabaseAccess.UpdateAssignedEmployeePeripherieDataFromAnotherDatabase(tranferData)
			m_EventLog.WriteTempLogFile(String.Format("Update der (Peripherie) Kandidatendaten ({0}) {1} {2}: {3}", searchEmployee.EmployeeNumber, sourceMasterData.Firstname, sourceMasterData.Lastname, result), m_LOGFileName)


			If result Then
				m_importedEmployeeData.Add(sourceMasterData)
				'm_invalidEmployeeData.Remove(sourceData)

			Else
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ihre Daten konnten nicht importiert werden."))

				Return False
			End If


			Return result
		End Function

		Private Function AddAssignedEmployeeFromSourceData(ByVal sourceData As EmployeeMasterViewData) As Boolean
			Dim result As Boolean = True
			Dim offsetNumber As Integer = ReadEmployeeOffsetFromSettings()

			' read source data
			Dim sourceMasterData = m_SourceEmployeeDatabaseAccess.LoadEmployeeMasterData(sourceData.EmployeeNumber, True)
			Dim tranferData = New EmployeeTranferData With {.DestCustomerID = m_InitializationData.MDData.MDGuid, .DestMDNumber = m_InitializationData.MDData.MDNr,
						.SourceEmployeeNumber = sourceData.EmployeeNumber, .DestEmployeeOffsetNumber = offsetNumber, .SourceDataBaseName = m_SourceInitializationData.MDData.MDDbName}

			result = result AndAlso m_ListingDatabaseAccess.AddAssignedEmployeeMasterDataFromAnotherDatabase(tranferData)
			result = result AndAlso m_ListingDatabaseAccess.AddAssignedEmployeePeripherieDataFromAnotherDatabase(tranferData)
			m_EventLog.WriteTempLogFile(String.Format("Hinzufügen der (Source) Kandidatendaten ({0}): {1}", sourceData.EmployeeNumber, result), m_LOGFileName)

			If Not result Then
				Dim deleteResult = m_ListingDatabaseAccess.DeleteAssignedImportedEmployeeData(tranferData.DestNewEmployeeNumber)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Fehler während Import von Kandidaten Daten. Der Vorgang wird abgebrochen."))

				Return False
			Else
				m_EmployeeData.Add(sourceMasterData)
				m_importedEmployeeData.Add(sourceMasterData)

			End If


			Return result
		End Function


	End Class

End Namespace
