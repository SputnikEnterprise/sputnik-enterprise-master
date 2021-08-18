Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects.MonthlySalary
Imports SP.Infrastructure
Imports System.ComponentModel
Imports SP.Infrastructure.UI
Imports DevExpress.XtraEditors.Controls

''' <summary>
''' User control to add and remove LM_Doc's via drag and drop.
''' </summary>
Public Class frmLMDocs

#Region "Private Fields"

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' The employee data access object.
	''' </summary>
	Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

	''' <summary>
	''' The SPProgUtility object.
	''' </summary>
	Private m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As Utility

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' The LMNr.
	''' </summary>
	Private m_LMnr As Integer

	''' <summary>
	''' The current document record number.
	''' </summary>
	Private m_CurrentDoctRecordNumber As Integer?

	''' <summary>
	''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
	''' </summary>
	Private m_SuppressUIEvents As Boolean = False

	''' <summary>
	''' The candidate name.
	''' </summary>
	Private m_CandidateName As String

#End Region

#Region "Events"

	Public Event NumberDocumentsChanged As EventHandler

#End Region

#Region "Constructor"

	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New(ByVal candidateName As String, ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		m_CandidateName = candidateName

		m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_Utility = New Utility
		m_UtilityUI = New UtilityUI


		TranslateControls()

		Reset()

	End Sub

#End Region

#Region "Public Properties"

	''' <summary>
	''' Gets the selected document view data.
	''' </summary>
	''' <returns>The selected document or nothing if none is selected.</returns>
	Public ReadOnly Property SelectedDocumentViewData As LMDocDataViewData
		Get
			Dim grdView = TryCast(grdLMDocument.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim document = CType(grdView.GetRow(selectedRows(0)), LMDocDataViewData)
					Return document
				End If

			End If

			Return Nothing
		End Get

	End Property

	''' <summary>
	''' Gets the first document in the list of documents.
	''' </summary>
	''' <returns>First document in list or nothing.</returns>
	Public ReadOnly Property FirstDocumentInListOfDocuments As LMDocDataViewData
		Get
			If gvLMDocument.RowCount > 0 Then

				Dim rowHandle = gvLMDocument.GetVisibleRowHandle(0)
				Return CType(gvLMDocument.GetRow(rowHandle), LMDocDataViewData)
			Else
				Return Nothing
			End If

		End Get
	End Property

#End Region

#Region "Public Methods"

	''' <summary>
	''' Loads the document data.
	''' </summary>
	''' <param name="lmNr">The LM number.</param>
	''' <returns>Boolean flag indicating success.</returns>
	Public Function LoadDocumentsData(ByVal lmNr As Integer) As Boolean

		m_LMnr = lmNr

		Reset()

		Dim success As Boolean = True

		success = success AndAlso LoadDocumentsAndLoadFirstDocDetails(lmNr)

		Return success

	End Function

	''' <summary>
	''' Resets the user control.
	''' </summary>
	Public Sub Reset()

		txtLMFile.Text = String.Empty
		txtLMFile.Properties.MaxLength = 255
		txtLMFile.Enabled = True

		ResetDocumentGrid()

	End Sub

	''' <summary>
	''' Resets the document grid.
	''' </summary>
	Private Sub ResetDocumentGrid()

		' Reset the grid
		gvLMDocument.OptionsView.ShowIndicator = False

		' Reset the grid
		gvLMDocument.Columns.Clear()

		Dim columnDocDescription As New DevExpress.XtraGrid.Columns.GridColumn()
		columnDocDescription.Caption = m_Translate.GetSafeTranslationValue("Beschreibung")
		columnDocDescription.Name = "DocDescription"
		columnDocDescription.FieldName = "DocDescription"
		columnDocDescription.Visible = True
		columnDocDescription.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvLMDocument.Columns.Add(columnDocDescription)

		Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCreatedOn.Caption = m_Translate.GetSafeTranslationValue("Datum")
		columnCreatedOn.Name = "CreatedOn"
		columnCreatedOn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columnCreatedOn.DisplayFormat.FormatString = "dd.MM.yyyy HH:mm"
		columnCreatedOn.FieldName = "CreatedOn"
		columnCreatedOn.Visible = True
		gvLMDocument.Columns.Add(columnCreatedOn)

		Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCreatedFrom.Caption = m_Translate.GetSafeTranslationValue("Erstellt von")
		columnCreatedFrom.Name = "CreatedFrom"
		columnCreatedFrom.FieldName = "CreatedFrom"
		columnCreatedFrom.Visible = True
		columnCreatedFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvLMDocument.Columns.Add(columnCreatedFrom)

	End Sub

	''' <summary>
	''' Loads the documents 
	''' </summary>
	''' <param name="lmNr">The LM number.</param>
	''' <returns>Boolean flag indicating success.</returns>
	Private Function LoadDocumentsAndLoadFirstDocDetails(ByVal lmNr As Integer) As Boolean

		Dim success As Boolean = True

		success = success AndAlso LoadDocuments(lmNr)

		Dim firstDocument = FirstDocumentInListOfDocuments

		LoadDocumentDetails(firstDocument)

		Return success

	End Function

	''' <summary>
	''' Loads the documents.
	''' </summary>
	''' <param name="lmNr">The LM number.</param>
	''' <returns>Boolean flag indicating success.</returns>
	Private Function LoadDocuments(ByVal lmNr As Integer) As Boolean
		Dim listOfDocuments = m_EmployeeDatabaseAccess.LoadLMDocListForLM(lmNr, Nothing, Nothing)

		If (listOfDocuments Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(Me, m_Translate.GetSafeTranslationValue("Dokumente konnten nicht geladen werden."))
			Return False
		End If

		Dim listDataSource As BindingList(Of LMDocDataViewData) = New BindingList(Of LMDocDataViewData)

		' Convert the data to view data.
		For Each interview In listOfDocuments

			Dim viewData = New LMDocDataViewData() With {
				.ID = interview.ID,
				.RecordNumber = interview.RecordNumber,
				.DocDescription = interview.DocDescription,
				.CreatedOn = interview.CreatedOn,
				.CreatedFrom = interview.CreatedFrom
			}

			listDataSource.Add(viewData)
		Next

		Dim suppressUIEventsState = m_SuppressUIEvents
		m_SuppressUIEvents = True
		grdLMDocument.DataSource = listDataSource
		m_SuppressUIEvents = suppressUIEventsState

		Return True
	End Function

#End Region

#Region "Privte Methods"

	''' <summary>
	''' Translate the controls.
	''' </summary>
	Private Sub TranslateControls()

		Me.Text = String.Format(m_Translate.GetSafeTranslationValue(Me.Text), m_CandidateName)
		Me.btnSaveLMDocument.Text = m_Translate.GetSafeTranslationValue(Me.btnSaveLMDocument.Text)
		Me.lblBezeichnung.Text = m_Translate.GetSafeTranslationValue(Me.lblBezeichnung.Text)

	End Sub

	''' <summary>
	''' Loads document details.
	''' </summary>
	''' <param name="documentViewData">The document view data.</param>
	Private Sub LoadDocumentDetails(ByVal documentViewData As LMDocDataViewData)

		If Not documentViewData Is Nothing Then
			m_CurrentDoctRecordNumber = documentViewData.RecordNumber
			txtLMFile.Text = documentViewData.DocDescription
		Else
			m_CurrentDoctRecordNumber = Nothing
			txtLMFile.Text = String.Empty
		End If

	End Sub

	Private Sub OnTxtFilePath_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles txtLMFile.ButtonClick
		Dim fileName = m_UtilityUI.ShowOpenFileDialog(Me, "PDF-Dateien (*.PDF)|*.pdf")

		Me.txtLMFile.EditValue = fileName
		If fileName <> String.Empty Then SaveSelectedDocToList(New String() {fileName})

	End Sub

	''' <summary>
	''' Handles click on save document button.
	''' </summary>
	Private Sub OnBtnSaveLMDocument_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveLMDocument.Click

		If Not m_CurrentDoctRecordNumber Is Nothing Then

			Dim success As Boolean = False

			' Load record data.
			Dim dataList = m_EmployeeDatabaseAccess.LoadLMDocListForLM(m_LMnr, m_CurrentDoctRecordNumber, False)

			If dataList.Count = 1 Then
				Dim lmDoc = dataList(0)
				lmDoc.DocDescription = txtLMFile.Text

				' Update record data.
				success = m_EmployeeDatabaseAccess.UdateLMDoc(lmDoc, False)
			End If

			If Not success Then
				m_UtilityUI.ShowErrorDialog(Me, m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))
			Else
				LoadDocuments(m_LMnr)
				FocusDocument(m_CurrentDoctRecordNumber)
			End If

		End If

	End Sub

	''' <summary>
	''' Handles focus change of employee row.
	''' </summary>
	Private Sub OnEmployee_FocusedRowChanged(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles gvLMDocument.FocusedRowChanged

		If m_SuppressUIEvents Then
			Return
		End If

		Dim selectedDocument = SelectedDocumentViewData

		If Not selectedDocument Is Nothing Then
			LoadDocumentDetails(selectedDocument)
		End If

	End Sub

	''' <summary>
	''' Handles the form drag enter event.
	''' </summary>
	Private Sub OnADdLMDoc_DragEnter(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles Me.DragEnter
		Dim files() As String = e.Data.GetData(DataFormats.FileDrop)

		If Not files Is Nothing AndAlso files.Count > 0 Then

			For Each file In files

				Dim fileInfo As New IO.FileInfo(file)

				If fileInfo.Extension.ToLower() = ".pdf" Then
					' At least on pdf file must be in the collection.
					e.Effect = DragDropEffects.Copy
					Return
				End If

			Next

		End If

		e.Effect = DragDropEffects.None
	End Sub

	''' <summary>
	''' Handles the form dragdrop event.
	''' </summary>
	Private Sub OnADdLMDoc_DragDrop(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles Me.DragDrop
		Dim files() As String = e.Data.GetData(DataFormats.FileDrop)

		SaveSelectedDocToList(files)

	End Sub

	Private Sub SaveSelectedDocToList(ByVal files() As String)
		Dim comleteSuccess As Boolean = True
		If files Is Nothing Then Return

		Dim dt As DateTime = DateTime.Now

		Dim successfullySavedDocs As New List(Of LMDocData)

		If Not files Is Nothing AndAlso files.Count > 0 Then

			For Each file In files

				Dim fileInfo As New IO.FileInfo(file)

				If fileInfo.Extension.ToLower() = ".pdf" Then

					Dim lmDoc As New LMDocData
					lmDoc.LMNr = m_LMnr
					lmDoc.DocDescription = fileInfo.FullName
					lmDoc.CreatedOn = dt
					lmDoc.CreatedFrom = m_ClsProgSetting.GetUserName()

					' Load the file bytes
					Dim bytes() = m_Utility.LoadFileBytes(fileInfo.FullName)

					If bytes Is Nothing Then
						comleteSuccess = False
					Else
						' Save the LM_Doc.
						lmDoc.DocScan = bytes
						If (m_EmployeeDatabaseAccess.AddLMDoc(lmDoc)) Then
							successfullySavedDocs.Add(lmDoc)
						End If
					End If

				End If

			Next

			' Select first redcord ordererd by CreatedOn Desc, DocDescription Asc
			If successfullySavedDocs.Count > 0 Then
				Dim sortedDocs = successfullySavedDocs.OrderByDescending(Function(data) data.CreatedOn).ThenBy(Function(data) data.DocDescription).ToArray()

				' Reload documents and focus document and load detail data.
				LoadDocuments(m_LMnr)
				FocusDocument(sortedDocs(0).RecordNumber)

				Dim selectedDoc = SelectedDocumentViewData
				LoadDocumentDetails(selectedDoc)
			Else
				' Load the first document.
				LoadDocumentsAndLoadFirstDocDetails(m_LMnr)
			End If

			If Not comleteSuccess Then
				m_UtilityUI.ShowErrorDialog(Me, m_Translate.GetSafeTranslationValue("Ein oder mehrere Dateien konnten nicht gespeichert werden."))
			End If

			RaiseEvent NumberDocumentsChanged(Me, New EventArgs())

		End If

	End Sub


	''' <summary>
	''' Handles double click on documents grid.
	''' </summary>
	Private Sub OnLMDocument_DoubleClick(sender As System.Object, e As System.EventArgs) Handles gvLMDocument.DoubleClick

		Dim selectedDocumentData = SelectedDocumentViewData

		If Not selectedDocumentData Is Nothing Then
			Dim dataList = m_EmployeeDatabaseAccess.LoadLMDocListForLM(m_LMnr, selectedDocumentData.RecordNumber, True)

			If (dataList.Count = 1) Then
				Dim documentData = dataList(0)

				Dim bytes() = documentData.DocScan

				Dim tempFileName = System.IO.Path.GetTempFileName()
				Dim tempFileFinal = System.IO.Path.ChangeExtension(tempFileName, "pdf")

				If (Not bytes Is Nothing AndAlso m_Utility.WriteFileBytes(tempFileFinal, bytes)) Then

					m_Utility.OpenFileWithDefaultProgram(tempFileFinal)

				End If

			End If

		End If

	End Sub

	''' <summary>
	''' Handles key down on selected employees grid.
	''' </summary>
	Private Sub OnGridSelectedEmployees_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles grdLMDocument.KeyDown

		If (e.KeyCode = Keys.Delete) Then

			Dim selectedDocument = SelectedDocumentViewData

			If Not selectedDocument Is Nothing Then

				If (m_UtilityUI.ShowYesNoDialog(Me, m_Translate.GetSafeTranslationValue("Wollen Sie den Datensatz wirklich löschen?"),
																	 m_Translate.GetSafeTranslationValue("Datensatz löschen")) = False) Then
					Return
				End If

				Dim success = m_EmployeeDatabaseAccess.DeleteLMDoc(selectedDocument.ID)

				LoadDocumentsAndLoadFirstDocDetails(m_LMnr)

				If Not success Then
					m_UtilityUI.ShowErrorDialog(Me, m_Translate.GetSafeTranslationValue("Das Dokument konnte nicht gelöscht werden."))
				End If

				RaiseEvent NumberDocumentsChanged(Me, New EventArgs())

			End If

		End If

	End Sub

	''' <summary>
	''' Focuses a document.
	''' </summary>
	''' <param name="documentRecordNumber">The document record number.</param>
	Private Sub FocusDocument(ByVal documentRecordNumber As Integer)

		If Not grdLMDocument.DataSource Is Nothing Then

			Dim documentViewData = CType(grdLMDocument.DataSource, BindingList(Of LMDocDataViewData))

			Dim index = documentViewData.ToList().FindIndex(Function(data) data.RecordNumber = documentRecordNumber)

			m_SuppressUIEvents = True
			Dim rowHandle = gvLMDocument.GetRowHandle(index)
			gvLMDocument.FocusedRowHandle = rowHandle
			m_SuppressUIEvents = False
		End If

	End Sub

#End Region

#Region "View helper classes"

	''' <summary>
	''' LM_Doc data view data.
	''' </summary>
	Public Class LMDocDataViewData

		Public Property ID As Integer
		Public Property RecordNumber As Integer?
		Public Property LMNr As Integer?
		Public Property DocDescription As String
		Public Property DocScan As Byte()
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String

	End Class

#End Region


End Class
