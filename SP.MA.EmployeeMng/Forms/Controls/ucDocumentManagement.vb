
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SP.MA.DocumentMng
Imports System.ComponentModel
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports DevExpress.XtraEditors.Repository
Imports SP.DatabaseAccess.Employee.DataObjects.DocumentMng

Imports SPMALLUtility.ClsMain_Net
Imports System.IO

Namespace UI

	Public Class ucDocumentManagement

#Region "Private Fields"

		''' <summary>
		''' The document management detail form.
		''' </summary>
		Private m_DocumentMagementDetailForm As frmDoc
		Private m_CVMagementDetailForm As SPMALLUtility.ClsMain_Net


#End Region

#Region "Constructor"

		''' <summary>
		''' The constructor.
		''' </summary>
		Public Sub New()

			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
			gvDocuments.OptionsView.ShowIndicator = False

		End Sub

#End Region

#Region "Public Methods"

		''' <summary>
		''' Activates the control.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <returns>Boolean value indicating success.</returns>
		Public Overrides Function Activate(ByVal employeeNumber As Integer) As Boolean
			m_SuppressUIEvents = True

			Dim success = True

			m_EmployeeNumber = Nothing
			If (Not IsEmployeeDataLoaded OrElse (Not m_EmployeeNumber = employeeNumber)) Then
				CleanUp()
				success = success AndAlso LoadEmployeeData(employeeNumber)

				m_EmployeeNumber = IIf(success, employeeNumber, 0)

			End If

			m_SuppressUIEvents = False

			Return success
		End Function


		''' <summary>
		''' Deactivates the control.
		''' </summary>
		Public Overrides Sub Deactivate()
			' Do nothing
		End Sub

		''' <summary>
		''' Resets the control.
		''' </summary>
		Public Overrides Sub Reset()

			m_EmployeeNumber = Nothing

			chkGotQualificationCertificate.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 130, m_InitializationData.MDData.MDNr)
			ResetDocumentsGrid()
			ResetCVGrid()

			grpEmployeeCV.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 122, m_InitializationData.MDData.MDNr)

		End Sub

		''' <summary>
		''' Validated data.
		''' </summary>
		Public Overrides Function ValidateData() As Boolean
			' Do nothing
			Return True
		End Function


		''' <summary>
		''' Merges the employee master data.
		''' </summary>
		''' <param name="employeeMasterData">The employee master data object where the data gets filled into.</param>
		''' <param name="forceMerge">Optional flag indicating if the merge should be forced altough no data has been loaded. </param>
		Public Overrides Sub MergeEmployeeMasterData(ByVal employeeMasterData As EmployeeMasterData, Optional forceMerge As Boolean = False)
			If ((IsEmployeeDataLoaded AndAlso
					m_EmployeeNumber = employeeMasterData.EmployeeNumber) Or forceMerge) Then
				' No employee master data (table Mitarbeiter) to merge.
			End If
		End Sub

		''' <summary>
		'''  Merges the employee contact other data (MASonstiges).
		''' </summary>
		''' <param name="employeeOtherData">The employee other data.</param>
		Public Overrides Sub MergeEmployeeOtherData(ByVal employeeOtherData As EmployeeOtherData)
			If (IsEmployeeDataLoaded AndAlso m_EmployeeNumber = employeeOtherData.EmployeeNumber) Then
				' No employee other data (MASonstiges) to merge
			End If
		End Sub

		''' <summary>
		'''  Merges the employee contact comm data.
		''' </summary>
		''' <param name="employeeContactCommData">The employee contact comm data.</param>
		Public Overrides Sub MergeEmployeeContactCommData(ByVal employeeContactCommData As EmployeeContactComm)
			If (IsEmployeeDataLoaded AndAlso m_EmployeeNumber = employeeContactCommData.EmployeeNumber) Then
				employeeContactCommData.GotDocs = chkGotQualificationCertificate.Checked
			End If
		End Sub

		''' <summary>
		''' Cleanup control.
		''' </summary>
		Public Overrides Sub CleanUp()

			If Not m_DocumentMagementDetailForm Is Nothing AndAlso
			 Not m_DocumentMagementDetailForm.IsDisposed Then

				Try
					m_DocumentMagementDetailForm.Close()
					m_DocumentMagementDetailForm.Dispose()
				Catch
					' Do nothing
				End Try
			End If

		End Sub

#End Region


		''' <summary>
		''' Gets the selected document view data.
		''' </summary>
		''' <returns>The selected document or nothing if none is selected.</returns>
		Private ReadOnly Property SelectedDocumentViewData As DocumentViewData
			Get
				Dim grdView = TryCast(gridDocuments.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (grdView Is Nothing) Then

					Dim selectedRows = grdView.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim document = CType(grdView.GetRow(selectedRows(0)), DocumentViewData)
						Return document
					End If

				End If

				Return Nothing
			End Get

		End Property


#Region "Private Methods"

		''' <summary>
		'''  Translate controls.
		''' </summary>
		Protected Overrides Sub TranslateControls()

			Me.grpDokumente.Text = m_Translate.GetSafeTranslationValue(Me.grpDokumente.Text)
			Me.chkGotQualificationCertificate.Text = m_Translate.GetSafeTranslationValue(Me.chkGotQualificationCertificate.Text)
			Me.grpEmployeeCV.Text = m_Translate.GetSafeTranslationValue(Me.grpEmployeeCV.Text)

		End Sub

		''' <summary>
		'''  Loads employee data.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadEmployeeData(ByVal employeeNumber As Integer) As Boolean

			Dim success As Boolean = True

			success = success AndAlso LoadEmployeeAssignedDocumentData(employeeNumber)
			success = success AndAlso LoadEmployeeContactCommData(employeeNumber)
			success = success AndAlso LoadEmployeeAssignedCVData(employeeNumber)

			Return success

		End Function

		''' <summary>
		''' Loads assigned employee document data.
		''' </summary>
		''' <param name="employeeNumber">The employeenumber number.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadEmployeeAssignedDocumentData(ByVal employeeNumber As Integer) As Boolean

			Dim documentData = m_EmployeeDataAccess.LoadEmployeeDocuments(employeeNumber)

			If (documentData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Zugeordnete Dokumente konnten nicht geladen werden."))
				Return False
			End If

			Dim listDataSource As BindingList(Of DocumentViewData) = New BindingList(Of DocumentViewData)

			Dim defaultIcon As Image = My.Resources.document

			Dim icons As New Dictionary(Of String, Image)
			icons.Add("doc", My.Resources.word)
			icons.Add("docx", My.Resources.word)
			icons.Add("xls", My.Resources.excel)
			icons.Add("xlsx", My.Resources.excel)
			icons.Add("pdf", My.Resources.pdf)
			icons.Add("msg", My.Resources.Mail2)

			Dim fileIcon As Image = Nothing

			' Convert the data to view data.
			For Each document In documentData

				document.ScanExtension = If(document.ScanExtension Is Nothing, String.Empty, document.ScanExtension)

				Dim extension As String = document.ScanExtension.ToLower().Trim()

				If icons.Keys.Contains(extension) Then
					fileIcon = icons(extension)
				Else
					fileIcon = defaultIcon
				End If

				Dim documentViewData = New DocumentViewData() With {
						.Id = document.ID,
						.CreatedOn = document.CreatedOn,
						.EmployeeNumber = document.EmployeeNumber,
						.Name = document.Name,
						.Image = fileIcon,
						.DocumentRecordNumber = document.DocumentRecordNumber,
						.Category = document.CategoryName,
						.FileFullPath = document.FileFullPath,
						.ScanExtension = document.ScanExtension
				}

				listDataSource.Add(documentViewData)
			Next

			gridDocuments.DataSource = listDataSource

			Return True

		End Function

		''' <summary>
		''' Loads assigned employee CV data.
		''' </summary>
		''' <param name="employeeNumber">The employeenumber number.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadEmployeeAssignedCVData(ByVal employeeNumber As Integer) As Boolean

			Dim cvData = m_EmployeeDataAccess.LoadEmployeeCV(employeeNumber)

			If (cvData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Der Lebenslauf konnten nicht geladen werden."))
				Return False
			End If

			Dim cvGridData = (From person In cvData
												Select New EmployeeCVData With
															 {.ID = person.ID,
																.Name = person.Name,
																.CreatedOn = person.CreatedOn,
																.CreatedFrom = person.CreatedFrom,
																.ChangedOn = person.ChangedOn,
																.ChangedFrom = person.ChangedFrom
															 }).ToList()

			Dim listDataSource As BindingList(Of EmployeeCVData) = New BindingList(Of EmployeeCVData)

			For Each p In cvGridData
				listDataSource.Add(p)
			Next

			grdCV.DataSource = listDataSource

			Return True

		End Function

		''' <summary>
		''' Loads the employee contact comm data.
		''' </summary>
		''' <param name="enmployeeNumber">The employee number.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadEmployeeContactCommData(ByVal enmployeeNumber As Integer) As Boolean

			Dim suppressUIEventsState As Boolean = m_SuppressUIEvents
			m_SuppressUIEvents = True

			Dim employeeContactCommData As EmployeeContactComm = m_EmployeeDataAccess.LoadEmployeeContactCommData(enmployeeNumber)

			If (employeeContactCommData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mitarbeiter-Kontaktdaten konnten nicht geladen werden."))
				Return False
			End If

			chkGotQualificationCertificate.Checked = employeeContactCommData.GotDocs.HasValue AndAlso employeeContactCommData.GotDocs.Value = True

			m_SuppressUIEvents = suppressUIEventsState

			Return True
		End Function

		''' <summary>
		''' Resets the documents grid.
		''' </summary>
		Private Sub ResetDocumentsGrid()

			' Reset the grid
			gvDocuments.Columns.Clear()

			Dim columnDate As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDate.Caption = m_Translate.GetSafeTranslationValue("Datum")
			columnDate.Name = "CreatedOn"
			columnDate.FieldName = "CreatedOn"
			columnDate.Visible = True
			gvDocuments.Columns.Add(columnDate)

			Dim columnCategory As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCategory.Caption = m_Translate.GetSafeTranslationValue("Kategorie")
			columnCategory.Name = "Category"
			columnCategory.FieldName = "Category"
			columnCategory.Visible = True
			gvDocuments.Columns.Add(columnCategory)

			Dim columnDescription As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDescription.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnDescription.Name = "Name"
			columnDescription.FieldName = "Name"
			columnDescription.Visible = True
			gvDocuments.Columns.Add(columnDescription)

			Dim docType As New DevExpress.XtraGrid.Columns.GridColumn()
			docType.Caption = m_Translate.GetSafeTranslationValue("Typ")
			docType.Name = "docType"
			docType.FieldName = "docType"
			docType.Visible = True
			docType.ColumnEdit = New RepositoryItemPictureEdit()
			docType.UnboundType = DevExpress.Data.UnboundColumnType.Object
			gvDocuments.Columns.Add(docType)

			gridDocuments.DataSource = Nothing

		End Sub

		''' <summary>
		''' Resets the CV grid.
		''' </summary>
		''' <remarks></remarks>
		Private Sub ResetCVGrid()

			' Reset the grid
			gvCV.Columns.Clear()

			Dim columnDate As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDate.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnDate.Name = "ID"
			columnDate.FieldName = "ID"
			columnDate.Visible = False
			gvCV.Columns.Add(columnDate)

			Dim columnName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnName.Caption = m_Translate.GetSafeTranslationValue("Lebenslauf Art")
			columnName.Name = "Name"
			columnName.FieldName = "Name"
			columnName.Visible = True
			gvCV.Columns.Add(columnName)

			Dim columnCreatedon As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedon.Caption = m_Translate.GetSafeTranslationValue("Erstellt am")
			columnCreatedon.Name = "CreatedOn"
			columnCreatedon.FieldName = "CreatedOn"
			columnCreatedon.Visible = True
			gvCV.Columns.Add(columnCreatedon)

			Dim columnCreatedfrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedfrom.Caption = m_Translate.GetSafeTranslationValue("Erstellt durch")
			columnCreatedfrom.Name = "CreatedFrom"
			columnCreatedfrom.FieldName = "CreatedFrom"
			columnCreatedfrom.Visible = True
			gvCV.Columns.Add(columnCreatedfrom)

			Dim columnChangedon As New DevExpress.XtraGrid.Columns.GridColumn()
			columnChangedon.Caption = m_Translate.GetSafeTranslationValue("Geändert am")
			columnChangedon.Name = "ChangedOn"
			columnChangedon.FieldName = "ChangedOn"
			columnChangedon.Visible = True
			gvCV.Columns.Add(columnChangedon)

			Dim columnChangedfrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnChangedfrom.Caption = m_Translate.GetSafeTranslationValue("Geändert durch")
			columnChangedfrom.Name = "ChangedFrom"
			columnChangedfrom.FieldName = "ChangedFrom"
			columnChangedfrom.Visible = True
			gvCV.Columns.Add(columnChangedfrom)

			grdCV.DataSource = Nothing

		End Sub

		''' <summary>
		''' Handles keydown on documents grid.
		''' </summary>
		Private Sub OnGridDocuments_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles gridDocuments.KeyDown

			If Not IsEmployeeDataLoaded Then
				Return
			End If

			If (e.KeyCode = Keys.Delete) Then
				If Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 138, m_InitializationData.MDData.MDNr) Then Return

				Dim grdView = TryCast(sender.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (grdView Is Nothing) Then

					Dim selectedRows = grdView.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim documentData = CType(grdView.GetRow(selectedRows(0)), DocumentViewData)

						If (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Möchten Sie die ausgewählen Daten wirklich löschen?"),
																						m_Translate.GetSafeTranslationValue("Daten entgültig löschen?"))) Then
							Dim success = m_EmployeeDataAccess.DeleteEmployeeDocument(documentData.Id)

							If Not success Then
								m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Das Dokument konnte nicht gelöscht werden."))
							End If

							LoadEmployeeAssignedDocumentData(m_EmployeeNumber)

						End If

					End If

				End If

			End If
		End Sub

		''' <summary>
		''' Handles unbound column data event.
		''' </summary>
		Private Sub OnGvDocuments_CustomUnboundColumnData(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs) Handles gvDocuments.CustomUnboundColumnData

			If e.Column.Name = "docType" Then
				If (e.IsGetData()) Then
					e.Value = CType(e.Row, DocumentViewData).Image
				End If
			End If
		End Sub

		Sub OngvDocuments_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvDocuments.RowCellClick

			If (e.Clicks = 2) Then

				Dim column = e.Column
				Dim dataRow = gvDocuments.GetRow(e.RowHandle)
				If Not dataRow Is Nothing Then
					Dim viewData = CType(dataRow, DocumentViewData)

					Select Case column.Name.ToLower
						Case "docType".ToLower
							OpenDocument()

						Case Else
							If viewData.DocumentRecordNumber > 0 Then
								ShowDocumentDetailForm(viewData.DocumentRecordNumber)

							End If

					End Select

				End If

			End If

		End Sub

		'''' <summary>
		'''' Handles double click on document.
		'''' </summary>
		'Private Sub OnDocument_DoubleClick(sender As System.Object, e As System.EventArgs) Handles gvDocuments.DoubleClick
		'	Dim selectedRows = gvDocuments.GetSelectedRows()

		'	If (selectedRows.Count > 0) Then
		'		Dim documentData = CType(gvDocuments.GetRow(selectedRows(0)), DocumentViewData)
		'		ShowDocumentDetailForm(m_EmployeeNumber, documentData.DocumentRecordNumber)
		'	End If
		'End Sub

		''' <summary>
		''' Handles click on button new document.
		''' </summary>
		Private Sub OnBtnAddDocument_Click(sender As System.Object, e As System.EventArgs) Handles btnAddDocument.Click
			If (IsEmployeeDataLoaded) Then
				ShowDocumentDetailForm(Nothing)
			End If
		End Sub

		Private Sub OpenDocument()
			Dim selectedDocumentData = SelectedDocumentViewData
			Dim currentExtension As String = "PDF"

			If Not selectedDocumentData Is Nothing Then
				Dim bytes() = m_EmployeeDataAccess.LoadEmployeeDocumentBytesData(selectedDocumentData.Id)
				Dim tempFileName = System.IO.Path.GetTempFileName()

				If selectedDocumentData.ScanExtension = String.Empty Then
					If selectedDocumentData.FileFullPath <> String.Empty AndAlso File.Exists(selectedDocumentData.FileFullPath) Then
						currentExtension = System.IO.Path.GetExtension(selectedDocumentData.FileFullPath)
						currentExtension = currentExtension.Replace(".", "")
					End If

				Else

					currentExtension = selectedDocumentData.ScanExtension

				End If
				Dim tempFileFinal = System.IO.Path.ChangeExtension(tempFileName, currentExtension)

				If (Not bytes Is Nothing AndAlso m_Utility.WriteFileBytes(tempFileFinal, bytes)) Then

					m_Utility.OpenFileWithDefaultProgram(tempFileFinal)

				End If

			End If

		End Sub


		''' <summary>
		''' Shows the document management form.
		''' </summary>
		''' <param name="documentRecordNumber">The document record number to select.</param>
		Private Sub ShowDocumentDetailForm(ByVal documentRecordNumber As Integer?)

			If m_DocumentMagementDetailForm Is Nothing OrElse m_DocumentMagementDetailForm.IsDisposed Then

				If Not m_DocumentMagementDetailForm Is Nothing Then
					'First cleanup handlers of old form before new form is created.
					RemoveHandler m_DocumentMagementDetailForm.FormClosed, AddressOf OnDocumentFormClosed
					RemoveHandler m_DocumentMagementDetailForm.DocumentDataSaved, AddressOf OnDocumentFormDocumentDataSaved
					RemoveHandler m_DocumentMagementDetailForm.DocumentDataDeleted, AddressOf OnDocumentFormDocumentDataDeleted
				End If

				m_DocumentMagementDetailForm = New frmDoc(m_InitializationData)
				AddHandler m_DocumentMagementDetailForm.FormClosed, AddressOf OnDocumentFormClosed
				AddHandler m_DocumentMagementDetailForm.DocumentDataSaved, AddressOf OnDocumentFormDocumentDataSaved
				AddHandler m_DocumentMagementDetailForm.DocumentDataDeleted, AddressOf OnDocumentFormDocumentDataDeleted
			End If

			m_DocumentMagementDetailForm.Show()
			m_DocumentMagementDetailForm.LoadDocumentData(m_EmployeeNumber, documentRecordNumber)
			m_DocumentMagementDetailForm.BringToFront()

		End Sub

		''' <summary>
		''' Handles close of document form.
		''' </summary>
		Private Sub OnDocumentFormClosed(sender As System.Object, e As System.EventArgs)
			LoadEmployeeAssignedDocumentData(m_EmployeeNumber)

			Dim documentForm = CType(sender, frmDoc)

			If Not documentForm.SelectedDocumentViewData Is Nothing Then
				FocusDocument(m_EmployeeNumber, documentForm.SelectedDocumentViewData.DocumentRecorNumber)
			End If

		End Sub

		''' <summary>
		''' Handles document form data saved.
		''' </summary>
		Private Sub OnDocumentFormDocumentDataSaved(ByVal sender As Object, ByVal employeeNumber As Integer, ByVal documentRecordNumber As Integer)

			LoadEmployeeAssignedDocumentData(m_EmployeeNumber)

			FocusDocument(m_EmployeeNumber, documentRecordNumber)

		End Sub

		''' <summary>
		''' Handles document form data deleted saved.
		''' </summary>
		Private Sub OnDocumentFormDocumentDataDeleted(ByVal sender As Object, ByVal employeeNumber As Integer, ByVal documentRecordNumber As Integer)
			LoadEmployeeAssignedDocumentData(m_EmployeeNumber)
		End Sub


		''' <summary>
		''' Focuses a document.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <param name="documentRecordNumber">The document record number.</param>
		Private Sub FocusDocument(ByVal employeeNumber As Integer, ByVal documentRecordNumber As Integer)

			Dim listDataSource As BindingList(Of DocumentViewData) = gridDocuments.DataSource

			Dim documentViewData = listDataSource.Where(Function(data) data.EmployeeNumber = employeeNumber AndAlso
																															 data.DocumentRecordNumber = documentRecordNumber).FirstOrDefault()

			If Not documentViewData Is Nothing Then
				Dim sourceIndex = listDataSource.IndexOf(documentViewData)
				Dim rowHandle = gvDocuments.GetRowHandle(sourceIndex)
				gvDocuments.FocusedRowHandle = rowHandle
			End If

		End Sub


		''' <summary>
		''' Handles double click on CV.
		''' </summary>
		Private Sub OnCV_DoubleClick(sender As System.Object, e As System.EventArgs) Handles gvCV.DoubleClick
			Dim selectedRows = gvCV.GetSelectedRows()

			If (selectedRows.Count > 0) Then
				Dim cvData = CType(gvCV.GetRow(selectedRows(0)), EmployeeCVData)
				ShowCVDetailForm(cvData.Name)
			End If
		End Sub

		''' <summary>
		''' Handles click on button new document.
		''' </summary>
		Private Sub OnBtnAddCV_Click(sender As System.Object, e As System.EventArgs) Handles btnAddCV.Click
			If (IsEmployeeDataLoaded) Then
				ShowCVDetailForm(Nothing)
			End If
		End Sub

		Private Sub OnBtnRefreshCV_Click(sender As System.Object, e As System.EventArgs) Handles btnRefreshCV.Click
			If (IsEmployeeDataLoaded) Then
				LoadEmployeeAssignedCVData(m_EmployeeNumber)
			End If
		End Sub

		''' <summary>
		''' Shows the CV management form.
		''' </summary>
		''' <param name="llName">The CV Name to select.</param>
		Private Sub ShowCVDetailForm(ByVal llName As String)

			'If m_CVMagementDetailForm Is Nothing OrElse m_CVMagementDetailForm.IsDisposed Then

			If Not m_CVMagementDetailForm Is Nothing Then
				'First cleanup handlers of old form before new form is created.
				'RemoveHandler m_DocumentMagementDetailForm.FormClosed, AddressOf OnDocumentFormClosed
				'RemoveHandler m_DocumentMagementDetailForm.DocumentDataSaved, AddressOf OnDocumentFormDocumentDataSaved
				'RemoveHandler m_DocumentMagementDetailForm.DocumentDataDeleted, AddressOf OnDocumentFormDocumentDataDeleted
			End If

			m_CVMagementDetailForm = New SPMALLUtility.ClsMain_Net
			'AddHandler m_DocumentMagementDetailForm.FormClosed, AddressOf OnDocumentFormClosed
			'AddHandler m_DocumentMagementDetailForm.DocumentDataSaved, AddressOf OnDocumentFormDocumentDataSaved
			'AddHandler m_DocumentMagementDetailForm.DocumentDataDeleted, AddressOf OnDocumentFormDocumentDataDeleted
			'End If
			If llName Is Nothing Then
				m_CVMagementDetailForm.Showfrmll(m_EmployeeNumber)
			Else
				m_CVMagementDetailForm.ShowfrmllWithTemplate(m_EmployeeNumber, llName)
			End If

			'm_CVMagementDetailForm.LoadDocumentData(m_EmployeeNumber, llName)
			'm_CVMagementDetailForm.BringToFront()

		End Sub

#End Region

#Region "View helper classes"

		''' <summary>
		'''  Document view data.
		''' </summary>
		Class DocumentViewData

			Public Property Id As Integer
			Public Property CreatedOn As DateTime
			Public Property EmployeeNumber As Integer
			Public Property ResponsiblePerson As String
			Public Property Name As String
			Public Property Image As Image
			Public Property DocumentRecordNumber As Integer
			Public Property Category As String
			Public Property ScanExtension As String
			Public Property FileFullPath As String

		End Class

#End Region

	End Class

End Namespace
