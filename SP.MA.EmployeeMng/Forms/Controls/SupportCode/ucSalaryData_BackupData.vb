
Imports System.ComponentModel
Imports DevExpress.Utils

Namespace UI

	Partial Class ucSalaryData


		Private Sub ResetBackupBeforeEQuestHistoryGrid()

			gvBackupBeforeEQuestHistory.OptionsView.ShowIndicator = False
			gvBackupBeforeEQuestHistory.OptionsView.ShowAutoFilterRow = False
			gvBackupBeforeEQuestHistory.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvBackupBeforeEQuestHistory.OptionsView.ShowFooter = False
			gvBackupBeforeEQuestHistory.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False

			gvBackupBeforeEQuestHistory.Columns.Clear()

			Dim columnDivAddressFullName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDivAddressFullName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDivAddressFullName.OptionsColumn.AllowEdit = False
			columnDivAddressFullName.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnDivAddressFullName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			columnDivAddressFullName.Name = "LabelValue"
			columnDivAddressFullName.FieldName = "LabelValue"
			columnDivAddressFullName.Visible = True
			columnDivAddressFullName.Width = 200
			gvBackupBeforeEQuestHistory.Columns.Add(columnDivAddressFullName)

			Dim columnForEmployment As New DevExpress.XtraGrid.Columns.GridColumn()
			columnForEmployment.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnForEmployment.OptionsColumn.AllowEdit = False
			columnForEmployment.Caption = m_Translate.GetSafeTranslationValue("Alt")
			columnForEmployment.Name = "OldValue"
			columnForEmployment.FieldName = "OldValue"
			columnForEmployment.Width = 50
			columnForEmployment.Visible = True
			gvBackupBeforeEQuestHistory.Columns.Add(columnForEmployment)

			Dim columnForReport As New DevExpress.XtraGrid.Columns.GridColumn()
			columnForReport.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnForReport.OptionsColumn.AllowEdit = False
			columnForReport.Caption = m_Translate.GetSafeTranslationValue("Neu")
			columnForReport.Name = "NewValue"
			columnForReport.FieldName = "NewValue"
			columnForReport.Visible = True
			columnForReport.Width = 50
			gvBackupBeforeEQuestHistory.Columns.Add(columnForReport)


			grdBackupBeforeEQuestHistory.DataSource = Nothing

		End Sub

		Private Sub OnfpBackupData_Hidden(sender As Object, e As FlyoutPanelEventArgs) Handles fpBackupData.Hidden
			'grpQST.Visible = True
			'Me.Enabled = True
		End Sub

		Private Sub OnfpBackupData_Shown(sender As Object, e As FlyoutPanelEventArgs) Handles fpBackupData.Shown
			For Each button In fpBackupData.OptionsButtonPanel.Buttons
				button.caption = m_Translate.GetSafeTranslationValue(button.caption)
			Next
		End Sub

		Private Sub LoadAdornerBackupDataForm()

			AdornerUIManager1.Elements.Remove(m_BackupBadge)
			AdornerUIManager1.Hide()
			ResetBeakControls()

			fpBackupData.OwnerControl = txtZemisNumber ' grpBewilligung
			fpBackupData.OptionsBeakPanel.CloseOnOuterClick = False
			fpBackupData.OptionsBeakPanel.AnimationType = Win.PopupToolWindowAnimation.Fade
			fpBackupData.OptionsBeakPanel.BeakLocation = BeakPanelBeakLocation.Left

			fpBackupData.ShowBeakForm(Control.MousePosition)
			LoadEmployeePayrollRelevantBeforeEQuestData()

			m_SuppressUIEvents = False

		End Sub

		Private Sub LoadEmployeePayrollRelevantBeforeEQuestData()

			Dim success As Boolean = True

			Dim employeeMasterData = m_EmployeeDataAccess.LoadEmployeeMasterData(m_EmployeeNumber, False)
			Dim backupHistoryData = m_EmployeeDataAccess.LoadEmployeeBeforeEQuestBackup(employeeMasterData.EmployeeNumber)

			If backupHistoryData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Backup-Daten konnten nicht geladen werden."))

				Return
			End If
			Dim data = backupHistoryData.Where(Function(x) x.EmployeeNumber = employeeMasterData.EmployeeNumber).FirstOrDefault
			If data Is Nothing Then Return

			Try
				grdBackupBeforeEQuestHistory.DataSource = Nothing

				Dim backupListData = New List(Of BackupViewData)
				Dim backupData As New BackupViewData

				If data.CheckedOn.HasValue Then
					backupData = New BackupViewData With {.LabelValue = m_Translate.GetSafeTranslationValue("Kontrolliert")}
					backupData.OldValue = data.CheckedOn
					backupData.NewValue = data.CheckedFrom
					backupListData.Add(backupData)
				End If

				backupData = New BackupViewData With {.LabelValue = m_Translate.GetSafeTranslationValue("Bewilligung")}
				backupData.OldValue = data.Permission
				backupData.NewValue = employeeMasterData.Permission
				backupListData.Add(backupData)

				backupData = New BackupViewData With {.LabelValue = m_Translate.GetSafeTranslationValue("CH-Partner")}
				backupData.OldValue = m_Translate.GetSafeTranslationValue(If(data.CHPartner, "Ja", "Nein"))
				backupData.NewValue = m_Translate.GetSafeTranslationValue(If(employeeMasterData.CHPartner, "Ja", "Nein"))
				backupListData.Add(backupData)

				backupData = New BackupViewData With {.LabelValue = m_Translate.GetSafeTranslationValue("Flüchtling ohne Sicherheitsleistung")}
				backupData.OldValue = m_Translate.GetSafeTranslationValue(If(data.NoSpecialTax, "Ja", "Nein"))
				backupData.NewValue = m_Translate.GetSafeTranslationValue(If(employeeMasterData.NoSpecialTax, "Ja", "Nein"))
				backupListData.Add(backupData)

				backupData = New BackupViewData With {.LabelValue = m_Translate.GetSafeTranslationValue("Quellensteuer Kanton")}
				backupData.OldValue = data.S_Canton
				backupData.NewValue = employeeMasterData.S_Canton
				backupListData.Add(backupData)

				backupData = New BackupViewData With {.LabelValue = m_Translate.GetSafeTranslationValue("Quellensteuer Gemeinde")}
				backupData.OldValue = data.QSTCommunity
				backupData.NewValue = employeeMasterData.QSTCommunity
				backupListData.Add(backupData)

				backupData = New BackupViewData With {.LabelValue = m_Translate.GetSafeTranslationValue("Kirchensteuer")}
				backupData.OldValue = data.ChurchTax
				backupData.NewValue = employeeMasterData.ChurchTax
				backupListData.Add(backupData)

				backupData = New BackupViewData With {.LabelValue = m_Translate.GetSafeTranslationValue("Anzahl Kinder")}
				backupData.OldValue = String.Format("{0}", data.ChildsCount)
				backupData.NewValue = String.Format("{0}", employeeMasterData.ChildsCount)
				backupListData.Add(backupData)

				backupData = New BackupViewData With {.LabelValue = m_Translate.GetSafeTranslationValue("Bescheinigung für Ansässigkeit")}
				backupData.OldValue = m_Translate.GetSafeTranslationValue(If(data.Residence.GetValueOrDefault(False), "Ja", "Nein"))
				backupData.NewValue = m_Translate.GetSafeTranslationValue(If(employeeMasterData.Residence.GetValueOrDefault(False), "Ja", "Nein"))
				backupListData.Add(backupData)

				backupData = New BackupViewData With {.LabelValue = m_Translate.GetSafeTranslationValue("Gültigkeit für Ansässigkeit")}
				backupData.OldValue = String.Format("{0:dd.MM.yyyy}", data.ANS_OST_Bis)
				backupData.NewValue = String.Format("{0:dd.MM.yyyy}", employeeMasterData.ANS_OST_Bis)
				backupListData.Add(backupData)


				Dim listDataSource As BindingList(Of BackupViewData) = New BindingList(Of BackupViewData)

				Dim supressUIEventState = m_SuppressUIEvents
				m_SuppressUIEvents = True

				For Each p In backupListData
					listDataSource.Add(p)
				Next
				grdBackupBeforeEQuestHistory.DataSource = listDataSource


			Catch ex As Exception

			End Try


			errorProvider.Clear()

		End Sub

		Private Sub OnfpBackupData_ButtonClick(sender As Object, e As FlyoutPanelButtonClickEventArgs) Handles fpBackupData.ButtonClick
			Dim result As Boolean = True
			Dim tag As Integer = Val(e.Button.Tag)

			Select Case tag
				Case 0
					fpBackupData.HideBeakForm(True)

				Case 1
					UpdateBackupBeforeEQuestData()

				Case Else
					Return

			End Select

		End Sub

		Private Sub UpdateBackupBeforeEQuestData()

			Dim success As Boolean = True

			success = success AndAlso m_EmployeeDataAccess.UpdateEmployeeBeforeEQuestBackup(m_EmployeeNumber, m_InitializationData.UserData.UserNr)
			If Not success Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ihre Backup-Daten konnten nicht gespeichert werden."))

				Return
			End If
			lblPayrollRelevantBackupInfo.ForeColor = Color.Green
			fpBackupData.HideBeakForm(True)

		End Sub

		Private Class BackupViewData
			Public Property LabelValue As String
			Public Property OldValue As String
			Public Property NewValue As String

		End Class

	End Class

End Namespace
