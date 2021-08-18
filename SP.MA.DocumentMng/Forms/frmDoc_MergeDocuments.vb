

Imports System.ComponentModel
Imports System.IO
Imports DevExpress.XtraBars
Imports DevExpress.XtraSplashScreen
Imports SP.DatabaseAccess.Employee.DataObjects.DocumentMng
Imports SP.Infrastructure
Imports SPSSendMail.RichEditSendMail

Partial Class frmDoc

	Private m_SortedEmployeeDocumentData As New BindingList(Of DocumentViewData)
	Private m_EmployeeDocumentData As New BindingList(Of DocumentViewData)

	Private m_PDFUtility As PDFUtilities.Utilities


#Region "private properties"

	''' <summary>
	''' Gets the selected documentlist to select.
	''' </summary>
	''' <returns>The selected document or nothing if none is selected.</returns>
	Private ReadOnly Property SelectedRecordToSelect As DocumentViewData
		Get
			Dim gv = TryCast(grdFileToSelect.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gv Is Nothing) Then

				Dim selectedRows = gv.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim doc = CType(gv.GetRow(selectedRows(0)), DocumentViewData)

					Return doc
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedRecord As DocumentViewData
		Get
			Dim gv = TryCast(grdSelectedFile.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gv Is Nothing) Then

				Dim selectedRows = gv.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim doc = CType(gv.GetRow(selectedRows(0)), DocumentViewData)

					Return doc
				End If

			End If

			Return Nothing
		End Get

	End Property


#End Region


#Region "reset"

	Private Sub ResetDocumentToSelectGrid()

		gvFileToSelect.OptionsView.ShowIndicator = False
		gvFileToSelect.OptionsView.ShowAutoFilterRow = False
		gvFileToSelect.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvFileToSelect.OptionsView.ShowFooter = False
		gvFileToSelect.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
		gvFileToSelect.OptionsBehavior.Editable = True

		' Reset the grid
		gvFileToSelect.Columns.Clear()

		Dim columnID As New DevExpress.XtraGrid.Columns.GridColumn()
		columnID.OptionsColumn.AllowEdit = False
		columnID.Caption = m_Translate.GetSafeTranslationValue("ID")
		columnID.Name = "ID"
		columnID.FieldName = "ID"
		columnID.Visible = False
		gvFileToSelect.Columns.Add(columnID)

		Dim columnName As New DevExpress.XtraGrid.Columns.GridColumn()
		columnName.OptionsColumn.AllowEdit = False
		columnName.Caption = m_Translate.GetSafeTranslationValue("Vorhandene Dateien")
		columnName.Name = "Name"
		columnName.FieldName = "Name"
		columnName.Visible = True
		gvFileToSelect.Columns.Add(columnName)


		grdFileToSelect.DataSource = Nothing

	End Sub

	''' <summary>
	''' Resets the selected document grid.
	''' </summary>
	Private Sub ResetSelectedDocumentGrid()

		gvSelectedFile.OptionsView.ShowIndicator = False
		gvSelectedFile.OptionsView.ShowAutoFilterRow = False
		gvSelectedFile.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvSelectedFile.OptionsView.ShowFooter = False
		gvSelectedFile.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
		gvSelectedFile.OptionsBehavior.Editable = True

		' Reset the grid
		gvSelectedFile.Columns.Clear()

		Dim columnID As New DevExpress.XtraGrid.Columns.GridColumn()
		columnID.OptionsColumn.AllowEdit = False
		columnID.Caption = m_Translate.GetSafeTranslationValue("ID")
		columnID.Name = "ID"
		columnID.FieldName = "ID"
		columnID.Visible = False
		gvSelectedFile.Columns.Add(columnID)

		Dim columnName As New DevExpress.XtraGrid.Columns.GridColumn()
		columnName.OptionsColumn.AllowEdit = False
		columnName.Caption = m_Translate.GetSafeTranslationValue("Vorhandene Dateien")
		columnName.Name = "Name"
		columnName.FieldName = "Name"
		columnName.Visible = True
		gvSelectedFile.Columns.Add(columnName)


		grdSelectedFile.DataSource = Nothing

	End Sub


	Private Function LoadEMailAttachmentData() As Boolean
		Dim result As Boolean = True
		Dim listDataSource As BindingList(Of DocumentViewData) = New BindingList(Of DocumentViewData)

		Try
			Dim docList = CType(gridDocuments.DataSource, BindingList(Of DocumentViewData))
			If docList Is Nothing OrElse docList.Count = 0 Then Return False

			For Each itm In docList
				If Not String.IsNullOrWhiteSpace(itm.ScanExtension) Then
					Dim documentViewData = New DocumentViewData With {.ID = itm.ID, .Name = itm.Name, .Checked = True}

					listDataSource.Add(documentViewData)

				End If

			Next

			grdFileToSelect.DataSource = listDataSource

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

			Return False
		End Try


		Return Not listDataSource Is Nothing

	End Function


#End Region

	Private Sub ShowMergeFiles()
		Dim barmgm As New BarManager

		Dim strZipfile2Send As String = String.Format(m_Translate.GetSafeTranslationValue("Unterlagen von {0} {1}.{2}"), m_CurrentEmployeeMasterData.Firstname, m_CurrentEmployeeMasterData.Lastname, ".PDF")

		Me.beFilename2Zip.Text = String.Empty

		LoadAllFileForSelect()

		Me.pcc_1Filename.Manager = barmgm
		Me.pcc_1Filename.ShowPopup(Control.MousePosition)
		Me.sbtnCreateOnePDF.Enabled = Me.gvSelectedFile.RowCount > 0

	End Sub

	Private Sub LoadAllFileForSelect()

		ResetDocumentToSelectGrid()
		ResetSelectedDocumentGrid()
		m_SortedEmployeeDocumentData.Clear()

		Dim data = gridDocuments.DataSource
		grdFileToSelect.DataSource = data

	End Sub

	Private Sub sbtnSend2Sortedclst_Click(sender As System.Object, e As System.EventArgs) Handles btnSend2SortedFiles.Click

		Dim selectedDocument = SelectedRecordToSelect

		If m_SortedEmployeeDocumentData Is Nothing Then
			If Not selectedDocument Is Nothing Then
				m_SortedEmployeeDocumentData.Add(selectedDocument)
			End If

		Else
			If Not selectedDocument Is Nothing AndAlso Not m_SortedEmployeeDocumentData.Any(Function(data) data.ID = selectedDocument.ID) Then
				m_SortedEmployeeDocumentData.Add(selectedDocument)
			End If

		End If
		grdSelectedFile.DataSource = m_SortedEmployeeDocumentData

		Dim existSortedData = grdSelectedFile.DataSource
		Dim listDataSource As BindingList(Of DocumentViewData) = New BindingList(Of DocumentViewData)
		Dim selectdata = CType(grdFileToSelect.DataSource, BindingList(Of DocumentViewData))

		For Each itm In selectdata
			If Not m_SortedEmployeeDocumentData.Any(Function(data) data.ID = itm.ID) Then
				listDataSource.Add(itm)
			End If
		Next

		grdFileToSelect.DataSource = listDataSource
		Me.sbtnCreateOnePDF.Enabled = m_SortedEmployeeDocumentData.Count > 0

	End Sub

	Private Sub sbtnLoadclst_Click(sender As System.Object, e As System.EventArgs) Handles btnLoadFilesToSelect.Click

		grdSelectedFile.DataSource = Nothing
		grdFileToSelect.DataSource = Nothing
		m_SortedEmployeeDocumentData.Clear()

		grdFileToSelect.DataSource = gridDocuments.DataSource

		Me.sbtnCreateOnePDF.Enabled = False

	End Sub



	Private Sub OnsbtnCreateOnePDF_Click(sender As System.Object, e As System.EventArgs) Handles sbtnCreateOnePDF.Click
		Dim success As Boolean = True
		Dim strTempFielname As String = String.Empty
		Dim liFiles2Merge As New List(Of String)
		Dim finalFielname As String = Me.beFilename2Zip.Text

		If m_SortedEmployeeDocumentData.Count = 0 Then Return

		Try

			If String.IsNullOrWhiteSpace(finalFielname) Then
				'Dim strZipfile2Send As String = Path.Combine(m_InitializationData.UserData.spTempEmployeePath, Path.GetRandomFileName)
				Dim strZipfile2Send As String = Path.GetRandomFileName

				finalFielname = strZipfile2Send
			Else
				finalFielname = String.Join("_", finalFielname.Split(Path.GetInvalidFileNameChars()))
			End If

			finalFielname = Path.ChangeExtension(finalFielname, "pdf")

			For Each itm In m_SortedEmployeeDocumentData
				Dim tempFileName = System.IO.Path.GetTempFileName()
				Dim bytes() = m_EmployeeDbAccess.LoadEmployeeDocumentBytesData(itm.ID)

				success = success AndAlso m_Utility.WriteFileBytes(tempFileName, bytes)
				If success Then liFiles2Merge.Add(tempFileName)
			Next

			If liFiles2Merge Is Nothing OrElse liFiles2Merge.Count = 0 OrElse liFiles2Merge(0) = String.Empty Then Return
		Catch ex As Exception

		End Try

		Try
			strTempFielname = Path.Combine(m_InitializationData.UserData.spTempEmployeePath, finalFielname)
			If File.Exists(strTempFielname) Then File.Delete(strTempFielname)

		Catch ex As Exception
			pcc_1Filename.HidePopup()

			strTempFielname = Path.Combine(m_InitializationData.UserData.spTempEmployeePath, Path.GetRandomFileName)
			strTempFielname = Path.ChangeExtension(strTempFielname, "pdf")

		End Try

		Dim strMessage As String = "Die Datei wurde erfolgreich zusammengeführt."
		Try

			success = success AndAlso m_PDFUtility.MergePdfFiles(liFiles2Merge.ToArray, strTempFielname)
			If Not success Then
				pcc_1Filename.HidePopup()
				m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Möglicheweise sind die Dateien nicht PDF-Konform. Sie können nur PDF-Dateien zusammenfügen."), m_Translate.GetSafeTranslationValue("Dateien zusammenfügen"), MessageBoxIcon.Error)

				Return
			End If

			Dim documentData As New EmployeeDocumentData

			documentData.EmployeeNumber = m_EmployeeNumber
			documentData.CategorieNumber = 0
			'documentData.Name = txtTitle.Text
			documentData.Description = txtDescription.Text
			documentData.USNr = m_InitializationData.UserData.UserNr
			documentData.CreatedFrom = m_InitializationData.UserData.UserFullName
			documentData.Description = m_Translate.GetSafeTranslationValue("Zusammengeführte Dokumente")
			documentData.Name = m_Translate.GetSafeTranslationValue("Zusammengeführte Dokumente")

			' Insert or update document
			success = m_EmployeeDbAccess.AddEmployeeDocument(documentData)
			m_CurrentDocumentRecordNumber = documentData.DocumentRecordNumber

			' Check if the document bytes must also be saved.
			Dim currentFileByte = m_Utility.LoadFileBytes(strTempFielname)
			success = success AndAlso m_EmployeeDbAccess.UpdateEmployeeDocumentByteData(documentData.ID, currentFileByte, "pdf")

			If Not success Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))
			End If
			LoadDocumentData(m_EmployeeNumber, Nothing)

			FocusDocument(m_EmployeeNumber, documentData.DocumentRecordNumber)

			pcc_1Filename.HidePopup()
			m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue(strMessage))


		Catch ex As Exception
			pcc_1Filename.HidePopup()

			strMessage = String.Format("Fehler: Möglicherweise sind die Dateien fehlerhaft!{0}{1}", vbNewLine, ex.ToString)
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(strMessage))

		End Try

	End Sub

	Private Function SendAssignedDocumentToEMail() As Boolean
		Dim result As Boolean = True

		Try
			SplashScreenManager.CloseForm(False)
			SplashScreenManager.ShowForm(GetType(WaitForm1), True, False)
			SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Mail-Versand") & Space(20))
			SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Die Daten werden zusammengestellt") & "...")


			gvDocuments.FocusedColumn = gvDocuments.VisibleColumns(1)
			gridDocuments.RefreshDataSource()
			Dim printList As BindingList(Of DocumentViewData) = gridDocuments.DataSource
			Dim docListToSend = (From r In printList Where r.Checked = True).ToList()
			Dim listtoSend = New List(Of String)
			Dim showWarning As Boolean = False

			For Each doc In docListToSend
				Dim filename As String = doc.Name
				If String.IsNullOrWhiteSpace(filename) Then filename = Path.GetRandomFileName
				If doc.ScanExtension.ToString.ToLower = "msg" Then
					filename = Path.ChangeExtension(filename, doc.ScanExtension)
					showWarning = True
				Else
					filename = Path.ChangeExtension(filename, "pdf")
				End If

				filename = String.Join("_", filename.Split(Path.GetInvalidFileNameChars()))
				filename = Path.Combine(m_InitializationData.UserData.spTempEmployeePath, filename)
				Dim bytes() = m_EmployeeDbAccess.LoadEmployeeDocumentBytesData(doc.ID)
				If m_Utility.WriteFileBytes(filename, bytes) Then listtoSend.Add(filename)

			Next

			If listtoSend Is Nothing OrElse listtoSend.Count = 0 Then
				m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Sie haben keine Dokumente zum Versand ausgewählt!"), m_Translate.GetSafeTranslationValue("Dokumente senden"), MessageBoxIcon.Warning)

				Return False
			End If

			If showWarning Then
				m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("<b>Achtung:</b><br>Sie versuchen Email-Dateien als Anhang zu senden!"), m_Translate.GetSafeTranslationValue("Datei-Anhang"), MessageBoxIcon.Exclamation)
			End If
			Dim frmMail = New frmMailTpl(m_InitializationData)

			Dim preselectionSetting As New PreselectionMailData
			preselectionSetting = New PreselectionMailData With {.MailType = MailTypeEnum.NOTDEFINED, .EmployeeNumber = m_EmployeeNumber, .PDFFilesToSend = listtoSend}

			frmMail.PreselectionData = preselectionSetting

			frmMail.LoadData()

			frmMail.Show()
			frmMail.BringToFront()


		Catch ex As Exception


		Finally
			SplashScreenManager.CloseForm(False)

		End Try



		Return result
	End Function


End Class
