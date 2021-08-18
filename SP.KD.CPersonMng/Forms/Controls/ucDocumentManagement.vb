﻿
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Customer.DataObjects
Imports DevExpress.XtraEditors.Repository
Imports System.ComponentModel
Imports SP.KD.DocumentMng
Imports SP.Infrastructure
Imports System.IO

Namespace UI

	''' <summary>
	''' Document management data.
	''' </summary>
	Public Class ucDocumentManagement


#Region "Private Fields"

		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As Utility

		''' <summary>
		''' The document management detail form.
		''' </summary>
		Private m_DocumentMagementDetailForm As frmDoc

#End Region

#Region "Constructor"

		''' <summary>
		''' The constructor.
		''' </summary>
		Public Sub New()

			m_Utility = New Utility

			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
			gvDocuments.OptionsView.ShowIndicator = False

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


#Region "Public Methods"

		''' <summary>
		''' Activates the control.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <param name="recordNumber">The record number.</param>
		''' <returns>Boolean value indicating success.</returns>
		Public Overrides Function Activate(ByVal customerNumber As Integer, ByVal recordNumber As Integer?) As Boolean
			m_SuppressUIEvents = True
			Dim success As Boolean = True

			If (recordNumber.HasValue) Then
				If (Not IsResponsiblePersonDataLoaded) Then
					success = success AndAlso LoadResponsiblePersonData(customerNumber, recordNumber)
				ElseIf Not customerNumber = m_CustomerNumber Or
							 Not m_RecordNumber = recordNumber Then
					CleanUp()
					success = success AndAlso LoadResponsiblePersonData(customerNumber, recordNumber)
				End If
			Else
				CleanUp()
				Reset()
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

			m_CustomerNumber = 0
			m_RecordNumber = Nothing

			ResetDocumentsGrid()

		End Sub

		''' <summary>
		''' Validated data.
		''' </summary>
		Public Overrides Function ValidateData() As Boolean
			' Do nothing
			Return True
		End Function

		''' <summary>
		''' Merges the responsible person master data.
		''' </summary>
		''' <param name="responsiblePersonMasterData">The responsible person master data object where the data gets filled into.</param>
		Public Overrides Sub MergeResponsiblePersonMasterData(ByVal responsiblePersonMasterData As ResponsiblePersonMasterData, Optional forceMerge As Boolean = False)
			' Do nothing
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

#Region "Private Methods"

		''' <summary>
		'''  Translate controls.
		''' </summary>
		Protected Overrides Sub TranslateControls()

			Me.grpDokumente.Text = m_Translate.GetSafeTranslationValue(Me.grpDokumente.Text)

		End Sub

		''' <summary>
		'''  Loads responsible person data.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <param name="recordNumber">The record number.</param>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadResponsiblePersonData(ByVal customerNumber As Integer, ByVal recordNumber As Integer) As Boolean

			Dim success As Boolean = True

			success = success AndAlso LoadAssignedResponsiblePersonDocumentData(customerNumber, recordNumber)

			m_CustomerNumber = IIf(success, customerNumber, 0)
			m_RecordNumber = IIf(success, recordNumber, Nothing)

			Return success

		End Function

		''' <summary>
		''' Loads assigned customer document data.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <param name="responsiblePersonRecordNumber">The responsible person record number.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadAssignedResponsiblePersonDocumentData(ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer) As Boolean

			Dim documentData = m_DataAccess.LoadAssignedResponsiblePersonDocumentData(customerNumber, responsiblePersonRecordNumber)

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

				Dim responsiblePerson = String.Empty

				If Not String.IsNullOrEmpty(document.Firstname) AndAlso
						Not String.IsNullOrEmpty(document.Lastname) Then
					responsiblePerson = String.Format("{0} {1},{2}", document.TranslatedSalutation, document.Lastname, document.Firstname)
				Else
					responsiblePerson = String.Empty
				End If

				Dim documentViewData = New DocumentViewData() With {
						.Id = document.ID,
						.CreatedOn = document.CreatedOn,
						.CustomerNumber = document.CustomerNumber,
						.ResponsiblePersonRecordNumber = document.ResponsiblePersonRecordNumber,
						.ResponsiblePerson = responsiblePerson,
						.Name = document.Name,
						.Image = fileIcon,
						.DocumentRecordNumber = document.DocumentRecordNumber,
						.FileFullPath = document.FileFullPath,
						.ScanExtension = document.ScanExtension
				}

				listDataSource.Add(documentViewData)
			Next

			gridDocuments.DataSource = listDataSource

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
		''' Handles keydown on documents grid.
		''' </summary>
		Private Sub OnGridDocuments_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles gridDocuments.KeyDown

			If Not IsResponsiblePersonDataLoaded Then
				Return
			End If

			If (e.KeyCode = Keys.Delete) Then

				Dim grdView = TryCast(sender.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (grdView Is Nothing) Then

					Dim selectedRows = grdView.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim documentData = CType(grdView.GetRow(selectedRows(0)), DocumentViewData)

						If (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Möchten Sie die ausgewählen Daten wirklich löschen?"),
																						m_Translate.GetSafeTranslationValue("Daten entgültig löschen?"))) Then
							Dim success = m_DataAccess.DeleteCustomerOrRespPersonDocumentAssignment(documentData.Id)

							If Not success Then
								m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Das Dokument konnte nicht gelöscht werden."))
							End If

							LoadAssignedResponsiblePersonDocumentData(m_CustomerNumber, m_RecordNumber)

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
								ShowDocumentDetailForm(m_CustomerNumber, viewData.ResponsiblePersonRecordNumber, Nothing)

							End If

					End Select

				End If

			End If

		End Sub

		'''' <summary>
		'''' Handles double click on document.
		'''' </summary>
		'Private Sub OnDocument_DoubleClick(sender As System.Object, e As System.EventArgs) Handles gvDocuments.DoubleClick
		'  Dim selectedRows = gvDocuments.GetSelectedRows()

		'  If (selectedRows.Count > 0) Then
		'    Dim documentData = CType(gvDocuments.GetRow(selectedRows(0)), DocumentViewData)
		'    ShowDocumentDetailForm(m_CustomerNumber, m_RecordNumber, documentData.DocumentRecordNumber)
		'  End If
		'End Sub

		''' <summary>
		''' Handles click on button new document.
		''' </summary>
		Private Sub OnBtnAddDocument_Click(sender As System.Object, e As System.EventArgs) Handles btnAddDocument.Click
			If (IsResponsiblePersonDataLoaded) Then
				ShowDocumentDetailForm(m_CustomerNumber, m_RecordNumber, Nothing)
			End If
		End Sub

		Private Sub OpenDocument()
			Dim selectedDocumentData = SelectedDocumentViewData
			Dim currentExtension As String = "PDF"

			If Not selectedDocumentData Is Nothing Then
				Dim bytes() = m_DataAccess.LoadAssignedResponsiblePersonDocumentBytesData(selectedDocumentData.Id)
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
		''' <param name="customerNumber">The customer number.</param>
		''' <param name="responsiblePersonRecordNumber">The responsible person number.</param>
		''' <param name="documentRecordNumber">The document record number to select.</param>
		Private Sub ShowDocumentDetailForm(ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer, ByVal documentRecordNumber As Integer?)

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
			m_DocumentMagementDetailForm.LoadDocumentData(m_CustomerNumber, responsiblePersonRecordNumber, documentRecordNumber)
			m_DocumentMagementDetailForm.BringToFront()

		End Sub

		''' <summary>
		''' Handles close of document form.
		''' </summary>
		Private Sub OnDocumentFormClosed(sender As System.Object, e As System.EventArgs)
			LoadAssignedResponsiblePersonDocumentData(m_CustomerNumber, m_RecordNumber)

			Dim documentForm = CType(sender, frmDoc)

			If Not documentForm.SelectedDocumentViewData Is Nothing Then
				FocusDocument(m_CustomerNumber, m_RecordNumber, documentForm.SelectedDocumentViewData.DocumentRecorNumber)
			End If

		End Sub

		''' <summary>
		''' Handles document form data saved.
		''' </summary>
		Private Sub OnDocumentFormDocumentDataSaved(ByVal sender As Object, ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer?, ByVal documentRecordNumber As Integer)

			LoadAssignedResponsiblePersonDocumentData(m_CustomerNumber, m_RecordNumber)

			FocusDocument(m_CustomerNumber, responsiblePersonRecordNumber, documentRecordNumber)

		End Sub

		''' <summary>
		''' Handles document form data deleted saved.
		''' </summary>
		Private Sub OnDocumentFormDocumentDataDeleted(ByVal sender As Object, ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer?, ByVal documentRecordNumber As Integer)
			LoadAssignedResponsiblePersonDocumentData(m_CustomerNumber, m_RecordNumber)
		End Sub


		''' <summary>
		''' Focuses a document.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <param name="responsiblePersonRecordNumber">The responsible person record number.</param>
		''' <param name="documentRecordNumber">The document record number.</param>
		Private Sub FocusDocument(ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer, ByVal documentRecordNumber As Integer)

			Dim listDataSource As BindingList(Of DocumentViewData) = gridDocuments.DataSource

			Dim documentViewData = listDataSource.Where(Function(data) data.CustomerNumber = customerNumber AndAlso
																															 data.ResponsiblePersonRecordNumber = responsiblePersonRecordNumber AndAlso
																															 data.DocumentRecordNumber = documentRecordNumber).FirstOrDefault()

			If Not documentViewData Is Nothing Then
				Dim sourceIndex = listDataSource.IndexOf(documentViewData)
				Dim rowHandle = gvDocuments.GetRowHandle(sourceIndex)
				gvDocuments.FocusedRowHandle = rowHandle
			End If

		End Sub

#End Region

#Region "View helper classes"

		''' <summary>
		'''  Document view data.
		''' </summary>
		Class DocumentViewData

			Public Property Id As Integer
			Public Property CreatedOn As DateTime
			Public Property CustomerNumber As Integer
			Public Property ResponsiblePersonRecordNumber As Integer
			Public Property ResponsiblePerson As String
			Public Property Name As String
			Public Property Image As Image
			Public Property DocumentRecordNumber As Integer
			Public Property ScanExtension As String
			Public Property FileFullPath As String

		End Class

#End Region

	End Class

End Namespace
