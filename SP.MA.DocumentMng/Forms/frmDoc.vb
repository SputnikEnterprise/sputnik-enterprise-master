
Imports System.IO

Imports System.Reflection.Assembly

Imports SP.DatabaseAccess
Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports DevExpress.XtraEditors.Controls
Imports System.ComponentModel
Imports SP.Infrastructure
Imports DevExpress.XtraEditors
Imports DevExpress.LookAndFeel
Imports SP.MA.DocumentMng.Settings
Imports DevExpress.XtraEditors.Repository
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects.DocumentMng

Imports SPProgUtility.Mandanten
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports DevExpress.XtraBars
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng

''' <summary>
''' Document management.
''' </summary>
Public Class frmDoc

	Public Delegate Sub DocumentDataSavedHandler(ByVal sender As Object, ByVal employeeNumber As Integer, ByVal documentRecordNumber As Integer)
	Public Delegate Sub DocumentDataDeletedHandler(ByVal sender As Object, ByVal employeeNumber As Integer, ByVal documentRecordNumber As Integer)

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
	Private m_EmployeeDbAccess As IEmployeeDatabaseAccess

	''' <summary>
	''' The settings manager.
	''' </summary>
	Protected m_SettingsManager As ISettingsManager

	''' <summary>
	''' The SPProgUtility object.
	''' </summary>
	Private m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As Utility

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' Contains the employee number.
	''' </summary>
	Private m_EmployeeNumber As Integer

	''' <summary>
	''' Record number of selected document.
	''' </summary>
	Private m_CurrentDocumentRecordNumber As Integer?

	''' <summary>
	''' Current file bytes.
	''' </summary>
	Private m_CurrentFileBytes As Byte()

	''' <summary>
	''' The extension of the current file.
	''' </summary>
	Private m_CurrentFileExtension As String
	Private m_CurrentEmployeeMasterData As EmployeeMasterData


	''' <summary>
	''' Boolean flag indicating if initial data has been loaded.
	''' </summary>
	Private m_IsInitialDataLoaded As Boolean = False

	''' <summary>
	''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
	''' </summary>
	Private m_SuppressUIEvents As Boolean = False
	Private m_md As Mandant

#End Region


#Region "Events"

	Public Event DocumentDataSaved As DocumentDataSavedHandler
	Public Event DocumentDataDeleted As DocumentDataDeletedHandler

#End Region

#Region "Constructor"

	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		Try
			' Mandantendaten
			m_md = New Mandant
			m_InitializationData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

			If Not Directory.Exists(m_InitializationData.UserData.spTempEmployeePath) Then Directory.CreateDirectory(m_InitializationData.UserData.spTempEmployeePath)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		gvDocuments.OptionsView.ShowIndicator = False

		m_EmployeeDbAccess = New DatabaseAccess.Employee.EmployeeDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_SettingsManager = New SettingsManager
		m_UtilityUI = New UtilityUI
		m_Utility = New Utility
		m_PDFUtility = New PDFUtilities.Utilities


		AddHandler gvDocuments.RowCellClick, AddressOf Ongv_RowCellClick
		AddHandler lueCategory.ButtonClick, AddressOf OnDropDown_ButtonClick

		Reset()

	End Sub

#End Region

#Region "Public Properties"

	''' <summary>
	''' Gets the selected document view data.
	''' </summary>
	''' <returns>The selected document or nothing if none is selected.</returns>
	Public ReadOnly Property SelectedDocumentViewData As DocumentViewData
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

	''' <summary>
	''' Gets the first document in the list of documents
	''' </summary>
	''' <returns>First document in list or nothing.</returns>
	Public ReadOnly Property FirstDocumentInListOfDocuments As DocumentViewData
		Get
			If gvDocuments.RowCount > 0 Then

				Dim rowHandle = gvDocuments.GetVisibleRowHandle(0)
				Return CType(gvDocuments.GetRow(rowHandle), DocumentViewData)
			Else
				Return Nothing
			End If

		End Get
	End Property

	''' <summary>
	''' Gets the selected category filter data.
	''' </summary>
	''' <returns>The selected category filter data.</returns>
	Public ReadOnly Property SelectedCategoryFilterData As CategoryVieData
		Get
			Return lstCategorie.SelectedItem
		End Get
	End Property

#End Region

#Region "Public Methods"

	''' <summary>
	''' Loads the document data of an employee.
	''' </summary>
	''' <param name="employeeNumber">The employee number.</param>
	''' <param name="documentRecordNumber">The document record number (optional).</param>
	''' <returns>Boolean truth value indicating success.</returns>
	Public Function LoadDocumentData(ByVal employeeNumber As Integer, ByVal documentRecordNumber As Integer?) As Boolean

		Dim success As Boolean = True

		If Not m_IsInitialDataLoaded Then
			success = success AndAlso LoadDropDownData()
			m_IsInitialDataLoaded = True
		End If

		' Reset the form
		'Reset()

		m_EmployeeNumber = employeeNumber
		success = success AndAlso LoadEmployeeNameData(employeeNumber)

		' Load the category filters
		success = success AndAlso LoadDocumentCategoryFilterData(employeeNumber, m_ClsProgSetting.GetUSLanguage())

		If documentRecordNumber.HasValue Then

			' Load the document list.
			success = success AndAlso LoadFilteredDocumentData(employeeNumber, Nothing)
			FocusDocument(employeeNumber, documentRecordNumber)
			success = success AndAlso LoadDocumentDetailData(employeeNumber, documentRecordNumber)
		Else
			success = success AndAlso LoadFilteredDocumentData(employeeNumber, Nothing)
			PrepareForNew()
		End If

		If Not success Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Dokumente konnten nicht geladen werden."))
		End If

		Return success
	End Function

#End Region

#Region "Private Methods"

	''' <summary>
	'''  trannslate controls
	''' </summary>
	''' <remarks></remarks>
	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

		Me.grpdetails.Text = m_Translate.GetSafeTranslationValue(Me.grpdetails.Text)

		Me.btnSave.Text = m_Translate.GetSafeTranslationValue(Me.btnSave.Text)
		Me.btnNewDocument.Text = m_Translate.GetSafeTranslationValue(Me.btnNewDocument.Text)
		Me.btnDeleteDocument.Text = m_Translate.GetSafeTranslationValue(Me.btnDeleteDocument.Text)

		Me.lblkategorie.Text = m_Translate.GetSafeTranslationValue(Me.lblkategorie.Text)
		Me.lblBezeichnung.Text = m_Translate.GetSafeTranslationValue(Me.lblBezeichnung.Text)
		Me.lblBeschreibung.Text = m_Translate.GetSafeTranslationValue(Me.lblBeschreibung.Text)
		Me.lblDatei.Text = m_Translate.GetSafeTranslationValue(Me.lblDatei.Text)

		Me.lblerstellt.Text = m_Translate.GetSafeTranslationValue(Me.lblerstellt.Text)
		Me.lblgaendert.Text = m_Translate.GetSafeTranslationValue(Me.lblgaendert.Text)

	End Sub

	''' <summary>
	''' Loads the employee name data.
	''' </summary>
	''' <param name="employeeNumber">The employee number.</param>
	''' <returns>Boolean flag indicating success</returns>
	Private Function LoadEmployeeNameData(ByVal employeeNumber As Integer) As Boolean

		m_CurrentEmployeeMasterData = m_EmployeeDbAccess.LoadEmployeeMasterData(employeeNumber)

		If (m_CurrentEmployeeMasterData Is Nothing) Then
			Return False
		End If

		Dim salutation As String = String.Empty

		If String.IsNullOrEmpty(m_CurrentEmployeeMasterData.Gender) Then
			salutation = "Frau/Herr"
		Else
			Select Case m_CurrentEmployeeMasterData.Gender.Trim().ToUpper()
				Case "M"
					salutation = "Herr"
				Case "W"
					salutation = "Frau"

			End Select
		End If
		salutation = m_Translate.GetSafeTranslationValue(salutation)

		Dim strformCaption As String = m_Translate.GetSafeTranslationValue(Me.Text)
		If String.IsNullOrEmpty(m_CurrentEmployeeMasterData.Firstname) Then
			Text = String.Format("{0}: [{1} {2}]", strformCaption, salutation, m_CurrentEmployeeMasterData.Lastname)

		Else
			Text = String.Format("{0}: [{1} {2} {3}]", strformCaption, salutation, m_CurrentEmployeeMasterData.Firstname, m_CurrentEmployeeMasterData.Lastname)
		End If

		Return True
	End Function

	''' <summary>
	''' Loads the drop down data.
	''' </summary>
	Private Function LoadDropDownData() As Boolean

		Dim success = True
		success = success AndAlso LoadCategoriesDropDownData(m_ClsProgSetting.GetUSLanguage())

		Return success
	End Function

	''' <summary>
	''' Loads the employee document category data.
	''' </summary>
	Private Function LoadCategoriesDropDownData(ByVal language As String) As Boolean
		Dim categoryData = m_EmployeeDbAccess.LoadEmployeeDocumentCategories()

		Dim categoryViewData = Nothing
		If Not categoryData Is Nothing Then

			categoryViewData = New List(Of CategoryVieData)

			For Each category In categoryData

				Dim categoryDescription As String = String.Empty
				Select Case language.ToLower().Trim()
					Case "d", "de"
						categoryDescription = category.DescriptionGerman
					Case "f", "fr"
						categoryDescription = category.DescriptionFrench
					Case "i", "it"
						categoryDescription = category.DescriptionItalian
					Case Else
						categoryDescription = category.DescriptionGerman
				End Select

				categoryViewData.Add(New CategoryVieData With {.CategoryNumber = category.CategoryNumber,
															   .Description = categoryDescription})
			Next

		Else
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kategorieauswahldaten konnten nicht geladen werden."))
		End If

		lueCategory.Properties.DataSource = categoryViewData
		lueCategory.Properties.ForceInitialize()

		Return Not categoryViewData Is Nothing
	End Function

	''' <summary>
	''' Loads category filter data.
	''' </summary>
	''' <param name="employeeNumber">The employee number.</param>
	''' <param name="language">The language.</param>
	''' <returns>Boolean value indicating success.</returns>
	Private Function LoadDocumentCategoryFilterData(ByVal employeeNumber As Integer, ByVal language As String) As Boolean

		Dim categoryData = m_EmployeeDbAccess.LoadDistinctDocumentCategoriesOfEmployee(employeeNumber)

		Dim categoryViewData = Nothing
		If Not categoryData Is Nothing Then

			categoryViewData = New List(Of CategoryVieData)

			categoryViewData.Add(New CategoryVieData With {.CategoryNumber = Nothing, .Description = m_Translate.GetSafeTranslationValue("--Alle Dokumente--")})

			For Each category In categoryData

				Dim categoryDescription As String = String.Empty
				Select Case language.ToLower().Trim()
					Case "d", "de"
						categoryDescription = category.DescriptionGerman
					Case "f", "fr"
						categoryDescription = category.DescriptionFrench
					Case "i", "it"
						categoryDescription = category.DescriptionItalian
					Case Else
						categoryDescription = category.DescriptionGerman
				End Select

				categoryViewData.Add(New CategoryVieData With {.CategoryNumber = category.CategoryNumber, .Description = categoryDescription})
			Next

		Else
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kategoriefilterdaten konnten nicht geladen werden."))
		End If

		lstCategorie.DisplayMember = "Description"
		lstCategorie.ValueMember = "CategoryNumber"

		m_SuppressUIEvents = True
		lstCategorie.DataSource = categoryViewData
		m_SuppressUIEvents = False

		Return Not categoryData Is Nothing
	End Function

	''' <summary>
	''' Resets the from.
	''' </summary>
	Private Sub Reset()

		m_EmployeeNumber = 0
		m_CurrentDocumentRecordNumber = Nothing
		m_CurrentFileBytes = Nothing
		m_CurrentFileExtension = Nothing

		lueCategory.EditValue = Nothing
		txtTitle.Text = String.Empty
		txtTitle.Properties.MaxLength = 100

		txtDescription.Text = String.Empty
		txtDescription.Properties.MaxLength = 1000

		txtFilePath.Text = String.Empty

		' ---Reset drop downs, grids and lists---

		ResetDocumentCategoryDropDown()

		m_SuppressUIEvents = True
		lstCategorie.DataSource = Nothing
		m_SuppressUIEvents = False
		ResetDocumentGrid()

		TranslateControls()

		btnDeleteDocument.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 138)

		' Clear errors
		errorProviderDocumentMangement.Clear()
	End Sub

	''' <summary>
	''' Resets the document category drop down.
	''' </summary>
	Private Sub ResetDocumentCategoryDropDown()

		lueCategory.Properties.DisplayMember = "Description"
		lueCategory.Properties.ValueMember = "CategoryNumber"

		Dim columns = lueCategory.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("Description", 0))

		lueCategory.Properties.ShowFooter = False
		lueCategory.Properties.DropDownRows = 10
		lueCategory.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueCategory.Properties.SearchMode = SearchMode.AutoComplete
		lueCategory.Properties.AutoSearchColumnIndex = 0

		lueCategory.Properties.NullText = String.Empty
		lueCategory.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets the document grid.
	''' </summary>
	Private Sub ResetDocumentGrid()

		' Reset the grid
		gvDocuments.OptionsView.ShowIndicator = False
		gvDocuments.OptionsView.ShowAutoFilterRow = False
		gvDocuments.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		'gvDocuments.OptionsView.ShowColumnHeaders = False
		gvDocuments.OptionsView.ShowFooter = False
		gvDocuments.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
		gvDocuments.OptionsBehavior.Editable = True


		gvDocuments.Columns.Clear()

		Dim columnChecked As New DevExpress.XtraGrid.Columns.GridColumn()
		columnChecked.OptionsColumn.AllowEdit = True
		columnChecked.Caption = m_Translate.GetSafeTranslationValue("Auswahl")
		columnChecked.Name = "Checked"
		columnChecked.FieldName = "Checked"
		columnChecked.Width = 5
		columnChecked.Visible = True

		gvDocuments.Columns.Add(columnChecked)

		Dim columnDate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnDate.OptionsColumn.AllowEdit = False
		columnDate.Caption = m_Translate.GetSafeTranslationValue("Datum")
		columnDate.Name = "CreatedOn"
		columnDate.FieldName = "CreatedOn"
		columnDate.Visible = True
		gvDocuments.Columns.Add(columnDate)

		Dim columnDocumenTitle As New DevExpress.XtraGrid.Columns.GridColumn()
		columnDocumenTitle.OptionsColumn.AllowEdit = False
		columnDocumenTitle.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
		columnDocumenTitle.Name = "Name"
		columnDocumenTitle.FieldName = "Name"
		columnDocumenTitle.Visible = True
		gvDocuments.Columns.Add(columnDocumenTitle)

		Dim docType As New DevExpress.XtraGrid.Columns.GridColumn()
		docType.OptionsColumn.AllowEdit = False
		docType.Caption = m_Translate.GetSafeTranslationValue("Typ")
		docType.Name = "docType"
		docType.FieldName = "docType"
		docType.Visible = True
		docType.ColumnEdit = New RepositoryItemPictureEdit()
		docType.UnboundType = DevExpress.Data.UnboundColumnType.Object
		gvDocuments.Columns.Add(docType)

		m_SuppressUIEvents = True
		gridDocuments.DataSource = Nothing

		m_SuppressUIEvents = False

	End Sub



	''' <summary>
	''' Hands category selected index changed event.
	''' </summary>
	Private Sub OnLstCategorie_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lstCategorie.SelectedIndexChanged

		If m_SuppressUIEvents Then
			Return
		End If

		Dim categoryFilter = If(SelectedCategoryFilterData Is Nothing, Nothing, SelectedCategoryFilterData.CategoryNumber)
		LoadFilteredDocumentData(m_EmployeeNumber, categoryFilter)
		Dim selectedDoc = SelectedDocumentViewData
		If Not selectedDoc Is Nothing Then
			LoadDocumentDetailData(m_EmployeeNumber, selectedDoc.DocumentRecorNumber)
		End If

	End Sub


	Private Sub grpAnhaenge_CustomButtonClick(sender As Object, e As Docking2010.BaseButtonEventArgs) Handles grpAnhaenge.CustomButtonClick

		If e.Button.Properties.GroupIndex = 0 Then
			SendAssignedDocumentToEMail()

		ElseIf e.Button.Properties.GroupIndex = 1 Then
			pcc_1Filename.HidePopup()
			ShowMergeFiles()

		End If

	End Sub



	''' <summary>
	''' Handles focus change of document row.
	''' </summary>
	Private Sub OnDocuments_FocusedRowChanged(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles gvDocuments.FocusedRowChanged

		If m_SuppressUIEvents Then
			Return
		End If

		Dim selectedDocument = SelectedDocumentViewData

		If Not selectedDocument Is Nothing Then
			LoadDocumentDetailData(selectedDocument.EmployeeNumber, selectedDocument.DocumentRecorNumber)
		End If

	End Sub

	''' <summary>
	''' Handles click on new document button.
	''' </summary>
	Private Sub OnBtnNewDocument_Click(sender As System.Object, e As System.EventArgs) Handles btnNewDocument.Click
		PrepareForNew()
	End Sub

	''' <summary>
	''' Handle click on save button.
	''' </summary>
	Private Sub OnBtnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click

		saveAssignedDocument()

	End Sub

	Private Function SaveAssignedDocument() As Boolean
		Dim success As Boolean = True

		If ValidateDocumentInputData() Then

			Dim documentData As EmployeeDocumentData = Nothing

			Dim dt = DateTime.Now
			If Not m_CurrentDocumentRecordNumber.HasValue Then
				documentData = New EmployeeDocumentData With {.EmployeeNumber = m_EmployeeNumber,
																			   .CreatedOn = dt,
																			   .CreatedFrom = m_ClsProgSetting.GetUserName()}
			Else

				Dim documentList = m_EmployeeDbAccess.LoadEmployeeDocuments(m_EmployeeNumber, m_CurrentDocumentRecordNumber, Nothing)

				If documentList Is Nothing OrElse Not documentList.Count = 1 Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))

					Return False
				End If

				documentData = documentList(0)
			End If

			documentData.EmployeeNumber = m_EmployeeNumber
			documentData.CategorieNumber = If(lueCategory.EditValue Is Nothing, 0, lueCategory.EditValue)
			documentData.Name = txtTitle.Text
			documentData.Description = txtDescription.Text
			documentData.ChangedFrom = m_ClsProgSetting.GetUserName()
			documentData.ChangedOn = dt
			documentData.USNr = m_InitializationData.UserData.UserNr

			If Not m_CurrentFileBytes Is Nothing Then documentData.DocPath = txtFilePath.Text

			' Insert or update document
			If documentData.ID = 0 Then
				success = m_EmployeeDbAccess.AddEmployeeDocument(documentData)
				m_CurrentDocumentRecordNumber = documentData.DocumentRecordNumber
			Else
				success = m_EmployeeDbAccess.UpdateEmployeedDocument(documentData)
			End If

			' Check if the document bytes must also be saved.
			If Not (m_CurrentFileBytes Is Nothing) Then
				success = success AndAlso m_EmployeeDbAccess.UpdateEmployeeDocumentByteData(documentData.ID, m_CurrentFileBytes, m_CurrentFileExtension)
			End If
			m_CurrentFileBytes = Nothing
			m_CurrentFileExtension = Nothing
			txtFilePath.Text = String.Empty

			If Not success Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))
			Else

				' Update document dates.
				lblDocumentCreated.Text = String.Format("{0:f}, {1}", documentData.CreatedOn, documentData.CreatedFrom)
				lblDocumentChanged.Text = String.Format("{0:f}, {1}", documentData.ChangedOn, documentData.ChangedFrom)

				Dim wasAllFilterActiveBeforeReloadOfFilters = Not SelectedCategoryFilterData Is Nothing AndAlso SelectedCategoryFilterData.CategoryNumber Is Nothing

				' Reload document filter
				LoadDocumentCategoryFilterData(m_EmployeeNumber, m_ClsProgSetting.GetUSLanguage())

				' If the '--All--' filter was not selected -> then select the category of the added file.
				If (Not wasAllFilterActiveBeforeReloadOfFilters) Then
					FocusCategoryFilter(documentData.CategorieNumber)
				End If

				' Load the document list data again.
				Dim categoryFilter = If(SelectedCategoryFilterData Is Nothing, Nothing, SelectedCategoryFilterData.CategoryNumber)
				LoadFilteredDocumentData(m_EmployeeNumber, categoryFilter)
				FocusDocument(m_EmployeeNumber, documentData.DocumentRecordNumber)

				RaiseEvent DocumentDataSaved(Me, m_EmployeeNumber, documentData.DocumentRecordNumber)

			End If

		End If


		Return success

	End Function


	''' <summary>
	''' Handle click on delete button.
	''' </summary>
	Private Sub OnBtnDeleteDocument_Click(sender As System.Object, e As System.EventArgs) Handles btnDeleteDocument.Click

		'If Not m_CurrentDocumentRecordNumber Is Nothing Then

		'	Dim recordToDeleteList = m_EmployeeDbAccess.LoadEmployeeDocuments(m_EmployeeNumber, m_CurrentDocumentRecordNumber, Nothing)

		'	If recordToDeleteList Is Nothing OrElse Not recordToDeleteList.Count = 1 Then
		'		m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Das Dokument konnte nicht gelöscht werden."))
		'		Return
		'	End If

		'	Dim recordToDelete = recordToDeleteList(0)

		If Not (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Möchten Sie die ausgewählen Daten wirklich löschen?"),
												m_Translate.GetSafeTranslationValue("Daten entgültig löschen?"))) Then Return

		Dim success = DeleteSelectedDocumentData()
		If Not success Then Return
		'Dim success = m_EmployeeDbAccess.DeleteEmployeeDocument(recordToDelete.ID)

		'If Not success Then
		'	m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Das Dokument konnte nicht gelöscht werden."))
		'	Return
		'End If

		'Dim wasAllFilterActiveBeforeReloadOfFilters = Not SelectedCategoryFilterData Is Nothing AndAlso SelectedCategoryFilterData.CategoryNumber Is Nothing
		'LoadDocumentCategoryFilterData(m_EmployeeNumber, m_ClsProgSetting.GetUSLanguage())

		'' If the '--All--' filter was not selected -> then we do select the category of the deleted file (if possible)
		'If Not wasAllFilterActiveBeforeReloadOfFilters Then
		'	FocusCategoryFilter(recordToDelete.CategorieNumber)
		'End If

		' Load the document list data again.
		Dim categoryFilter = If(SelectedCategoryFilterData Is Nothing, Nothing, SelectedCategoryFilterData.CategoryNumber)
			LoadFilteredDocumentData(m_EmployeeNumber, categoryFilter)

			' Load the document detail data of the first document in the list.
			Dim firstDoc = FirstDocumentInListOfDocuments

			If Not firstDoc Is Nothing Then
				LoadDocumentDetailData(m_EmployeeNumber, firstDoc.DocumentRecorNumber)
			Else
				m_CurrentDocumentRecordNumber = Nothing
			End If

			m_CurrentFileBytes = Nothing
			m_CurrentFileExtension = Nothing
			txtFilePath.Text = String.Empty

		'RaiseEvent DocumentDataDeleted(Me, m_EmployeeNumber, recordToDelete.DocumentRecordNumber)

		'End If

		'End If

	End Sub

	Private Function DeleteSelectedDocumentData() As Boolean
		Dim result As Boolean = True

		gvDocuments.FocusedColumn = gvDocuments.VisibleColumns(1)
		gridDocuments.RefreshDataSource()
		Dim printList As BindingList(Of DocumentViewData) = gridDocuments.DataSource
		Dim docListToSend = (From r In printList Where r.Checked = True).ToList()
		If docListToSend Is Nothing OrElse docListToSend.Count = 0 Then
			m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Sie haben keine Dokumente zum Löschen ausgewählt!"), m_Translate.GetSafeTranslationValue("Dokumente löschen"), MessageBoxIcon.Warning)

			Return False
		End If


		For Each doc In docListToSend
			result = result AndAlso m_EmployeeDbAccess.DeleteEmployeeDocument(doc.ID)

			If Not result Then
				m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Das Dokument konnte nicht gelöscht werden:<br>{0}"), doc.Name))

				Return False
			End If
			RaiseEvent DocumentDataDeleted(Me, m_EmployeeNumber, doc.DocumentRecorNumber)

		Next

		Return result
	End Function

	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim viewData = SelectedDocumentViewData
			If viewData Is Nothing Then Return
			'Dim dataRow = gvDocuments.GetRow(e.RowHandle)
			'If Not dataRow Is Nothing Then
			'	Dim viewData = CType(dataRow, DocumentViewData)

			OpenDocument()

			'Select Case column.Name.ToLower
			'		Case "rname1", "kdnr", "rname2", "rname3"
			'			If viewData.KDNr > 0 Then RunOpenKDForm(viewData.KDNr)


			'		Case Else
			'			If viewData.RENr > 0 Then
			'				RunOpenOPForm(viewData.RENr, CInt(viewData.mdnr))
			'			End If


			'	End Select

			'End If

		End If

	End Sub

	'''' <summary>
	'''' Handles double click on documents grid.
	'''' </summary>
	'Private Sub OnGvDocuments_DoubleClick(sender As System.Object, e As System.EventArgs) Handles gvDocuments.DoubleClick

	'	OpenDocument()
	'	'Dim selectedDocumentData = SelectedDocumentViewData

	'	'If Not selectedDocumentData Is Nothing Then
	'	'  Dim bytes() = m_EmployeeDbAccess.LoadEmployeeDocumentBytesData(selectedDocumentData.ID)
	'	'  Dim tempFileName = System.IO.Path.GetTempFileName()
	'	'  Dim tempFileFinal = System.IO.Path.ChangeExtension(tempFileName, selectedDocumentData.ScanExtension)

	'	'  If (Not bytes Is Nothing AndAlso m_Utility.WriteFileBytes(tempFileFinal, bytes)) Then

	'	'    m_Utility.OpenFileWithDefaultProgram(tempFileFinal)

	'	'  End If

	'	'End If

	'End Sub

	''' <summary>
	''' Handles drop down button clicks.
	''' </summary>
	Private Sub OnDropDown_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

		Const ID_OF_DELETE_BUTTON As Int32 = 1

		' If delete button has been clicked reset the drop down.
		If e.Button.Index = ID_OF_DELETE_BUTTON Then

			If TypeOf sender Is LookUpEdit Then
				Dim lookupEdit As LookUpEdit = CType(sender, LookUpEdit)
				lookupEdit.EditValue = Nothing
			End If
		End If
	End Sub

	''' <summary>
	''' Handles click on file path button.
	''' </summary>
	Private Sub OnTxtFilePath_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtFilePath.ButtonClick

		If e.Button.Index = 0 Then

			LoadFileBytesWithFileDialg()

		End If

	End Sub


	''' <summary>
	''' Loads file bytes with file dialog.
	''' </summary>
	Private Sub LoadFileBytesWithFileDialg()

		With OpenFileDialog1
			.Filter =
			"Word-Dokumente (*.doc)|*.doc|Excel-Dokumente (*.xls)|*.xls|PDF-Dokumente (*.pdf)|*.pdf|Alle Dateien (*.*)|*.*"
			.FilterIndex = 3
			.InitialDirectory = If(txtFilePath.Text = String.Empty, m_ClsProgSetting.GetUserHomePath, txtFilePath.Text)
			.Title = m_Translate.GetSafeTranslationValue("Dokument öffnen")
			.FileName = String.Empty

			If .ShowDialog() = DialogResult.OK Then

				txtFilePath.Text = String.Empty
				m_CurrentFileBytes = m_Utility.LoadFileBytes(.FileName)

				If m_CurrentFileBytes Is Nothing Then
					m_CurrentFileExtension = String.Empty

					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Datei konnte nicht geöffnet werden."))
				Else
					m_CurrentFileExtension = System.IO.Path.GetExtension(.FileName)

					If Not m_CurrentFileExtension Is Nothing Then
						m_CurrentFileExtension = m_CurrentFileExtension.Replace(".", "")
					End If

					txtFilePath.Text = .FileName
					errorProviderDocumentMangement.SetError(txtFilePath, String.Empty)
				End If

			End If
		End With

	End Sub

	''' <summary>
	''' Handles text change of open file name combo box.
	''' </summary>
	Private Sub OntxtFilePath_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtFilePath.TextChanged

		If m_SuppressUIEvents Then
			Return
		End If

		If String.IsNullOrEmpty(txtFilePath.Text) Then
			Me.txtFilePath.Properties.Buttons(1).Enabled = False
		End If
	End Sub

	''' <summary>
	''' Loads document bytes from file system.
	''' </summary>
	''' <param name="filepath">The file path.</param>
	Private Sub LoadDocumentBytesFormFileSystem(ByVal filepath As String)

		txtFilePath.Text = String.Empty
		m_CurrentFileBytes = m_Utility.LoadFileBytes(filepath)
		m_CurrentFileExtension = System.IO.Path.GetExtension(filepath)

		If Not m_CurrentFileExtension Is Nothing Then
			m_CurrentFileExtension = m_CurrentFileExtension.Replace(".", "")
		End If

		If m_CurrentFileBytes Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Datei konnte nicht geöffnet werden."))
		Else
			txtFilePath.Text = filepath
			txtFilePath.Properties.Buttons(1).Enabled = True
		End If

	End Sub

	Private Sub OpenDocument()
		Dim selectedDocumentData = SelectedDocumentViewData

		If Not selectedDocumentData Is Nothing Then
			Dim bytes() = m_EmployeeDbAccess.LoadEmployeeDocumentBytesData(selectedDocumentData.ID)
			Dim tempFileName = System.IO.Path.GetTempFileName()

			If selectedDocumentData.ScanExtension = String.Empty Then
				If selectedDocumentData.FileFullPath <> String.Empty AndAlso File.Exists(selectedDocumentData.FileFullPath) Then
					m_CurrentFileExtension = System.IO.Path.GetExtension(selectedDocumentData.FileFullPath)
					m_CurrentFileExtension = m_CurrentFileExtension.Replace(".", "")
				End If

			Else

				m_CurrentFileExtension = selectedDocumentData.ScanExtension

			End If
			Dim tempFileFinal = System.IO.Path.ChangeExtension(tempFileName, m_CurrentFileExtension)

			If (Not bytes Is Nothing AndAlso m_Utility.WriteFileBytes(tempFileFinal, bytes)) Then

				m_Utility.OpenFileWithDefaultProgram(tempFileFinal)

			End If

		End If

	End Sub

	''' <summary>
	''' Handles form load event.
	''' </summary>
	Private Sub OnFrmDoc_Load(sender As Object, e As System.EventArgs) Handles Me.Load

		Me.KeyPreview = True
		Dim strStyleName As String = m_md.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_md.GetDefaultUSNr, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		sccMain.AllowDrop = True

		Try
			Dim setting_form_height = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_HEIGHT)
			Dim setting_form_width = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_WIDTH)
			Dim setting_form_location = m_SettingsManager.ReadString(SettingKeys.SETTING_FORM_LOCATION)
			Dim setting_form_mainsplitter = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_MAINSPLITTER)

			If setting_form_height > 0 Then Me.Height = Math.Max(Me.Height, setting_form_height)
			If setting_form_width > 0 Then Me.Width = Math.Max(Me.Width, setting_form_width)
			If setting_form_mainsplitter > 0 Then Me.sccMain.SplitterPosition = Math.Max(Me.sccMain.SplitterPosition, setting_form_mainsplitter)

			If Not String.IsNullOrEmpty(setting_form_location) Then
				Dim aLoc As String() = setting_form_location.Split(CChar(";"))
				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Val(aLoc(0)), Val(aLoc(1)))
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())

		End Try

	End Sub

	''' <summary>
	''' Handles the form disposed event.
	''' </summary>
	Private Sub OnFrmDoc_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed

		' Save form location, width and height in setttings
		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				m_SettingsManager.WriteString(SettingKeys.SETTING_FORM_LOCATION, String.Format("{0};{1}", Me.Location.X, Me.Location.Y))
				m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_WIDTH, Me.Width)
				m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_HEIGHT, Me.Height)
				m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_MAINSPLITTER, Me.sccMain.SplitterPosition)

				m_SettingsManager.SaveSettings()
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())

		End Try

	End Sub

	''' <summary>
	''' Keypreview for Modul-version
	''' </summary>
	Private Sub OnForm_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
		If e.KeyCode = Keys.F12 And m_InitializationData.UserData.UserNr = 1 Then
			Dim strRAssembly As String = ""
			Dim strMsg As String = "Information über Modul: {0}Bibliothek: {1}{0}Ort: {2}{0}{0}ReferencedAssamblies:{0}{3}{0}"
			For Each a In AppDomain.CurrentDomain.GetAssemblies()
				strRAssembly &= String.Format("-->> {2}{0}{1}({3}){0}", vbNewLine, vbTab, a, a.CodeBase)
			Next
			strMsg = String.Format(strMsg, vbNewLine,
								   GetExecutingAssembly().FullName,
								   GetExecutingAssembly().Location,
								   strRAssembly)
			DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, "Modul-Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
		End If
	End Sub

	''' <summary>
	''' Handles the form dragdrop event.
	''' </summary>
	Private Sub Ongrpdetails_DragDrop(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles grpdetails.DragDrop
		Dim files() As String = e.Data.GetData(DataFormats.FileDrop)

		If e.Data.GetDataPresent("FileGroupDescriptor") Then
			'supports a drop of a Outlook message 
			Dim m_Path As New SPProgUtility.ClsProgSettingPath
			Dim objOL As Object = Nothing
			objOL = CreateObject("Outlook.Application")
			Dim myobj As Object

			For i As Integer = 1 To objOL.ActiveExplorer.Selection.Count
				myobj = objOL.ActiveExplorer.Selection.Item(i)

				'hardcode a destination path for testing
				Dim strFilename As String = myobj.Subject
				Try
					txtTitle.Text = strFilename
					txtDescription.Text = myobj.body

				Catch ex As Exception

				End Try

				strFilename = System.Text.RegularExpressions.Regex.Replace(myobj.Subject, "[\\/:*?""<>|\r\n]", "", System.Text.RegularExpressions.RegexOptions.Singleline)
				strFilename &= ".msg"
				Dim strFile As String = IO.Path.Combine(m_Path.GetSpS2DeleteHomeFolder, strFilename)

				myobj.SaveAs(strFile)
				files = New String() {strFile}
			Next

		Else

		End If

		If Not files Is Nothing AndAlso files.Count > 0 Then

			For Each myfile In files

				Dim fileInfo As New FileInfo(files(0))

				'If fileInfo.Extension.ToLower() = ".pdf" Then
				LoadDocumentBytesFormFileSystem(fileInfo.FullName)
				'End If

			Next

		End If

	End Sub

	''' <summary>
	''' Handles the form drag enter event.
	''' </summary>
	Private Sub Ongrpdetails_DragEnter(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles grpdetails.DragEnter

		e.Effect = DragDropEffects.Copy

		'Dim files() As String = e.Data.GetData(DataFormats.FileDrop)

		'  If Not files Is Nothing AndAlso files.Count > 0 Then
		'    Dim fileInfo As New IO.FileInfo(files(0))

		'    If fileInfo.Extension.ToLower() = ".pdf" Then
		'      e.Effect = DragDropEffects.Copy
		'    Else
		'      e.Effect = DragDropEffects.None
		'    End If

		'  End If
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

	''' <summary>
	''' Load filterd document data.
	''' </summary>
	''' <param name="employeeNumber">The employee number.</param>
	''' <param name="categoryNumberFilter">The optioinal category number.</param>
	''' <returns>Boolean value indicating success.</returns>
	Private Function LoadFilteredDocumentData(ByVal employeeNumber As Integer, ByVal categoryNumberFilter As Integer?) As Boolean

		Dim documentSearchResult = m_EmployeeDbAccess.LoadEmployeeDocuments(employeeNumber, Nothing, categoryNumberFilter)

		If (documentSearchResult Is Nothing) Then

			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Dokumente konnten nicht geladen werden."))

			Return False
		End If

		Dim defaultIcon As Image = My.Resources.document

		Dim icons As New Dictionary(Of String, Image)
		icons.Add("doc", My.Resources.word)
		icons.Add("docx", My.Resources.word)
		icons.Add("xls", My.Resources.excel)
		icons.Add("xlsx", My.Resources.excel)
		icons.Add("pdf", My.Resources.pdf)
		icons.Add("msg", My.Resources.Mail2)

		Dim fileIcon As Image = Nothing

		Dim listDataSource As BindingList(Of DocumentViewData) = New BindingList(Of DocumentViewData)

		' Convert the data to view data.
		For Each docResult In documentSearchResult

			docResult.ScanExtension = If(docResult.ScanExtension Is Nothing, String.Empty, docResult.ScanExtension)

			Dim cViewData = New DocumentViewData() With {
				.ID = docResult.ID,
				.EmployeeNumber = docResult.EmployeeNumber,
				.DocumentRecorNumber = docResult.DocumentRecordNumber,
				.Name = docResult.Name,
				.ScanExtension = docResult.ScanExtension,
				.FileFullPath = docResult.FileFullPath,
				.CategoryNumber = docResult.CategorieNumber,
				.CreatedOn = docResult.CreatedOn,
				.Checked = False
			}

			Dim extension As String = docResult.ScanExtension.ToLower().Trim()

			If icons.Keys.Contains(extension) Then
				fileIcon = icons(extension)
			Else
				fileIcon = defaultIcon
			End If

			cViewData.Image = fileIcon

			listDataSource.Add(cViewData)
		Next

		grpAnhaenge.CustomHeaderButtons(0).Properties.Enabled = Not listDataSource Is Nothing AndAlso listDataSource.Count > 1

		m_SuppressUIEvents = True
		gridDocuments.DataSource = listDataSource
		m_SuppressUIEvents = False

		Return True
	End Function

	''' <summary>
	''' Loads document detail data.
	''' </summary>
	''' <param name="employeeNumber">The employee number.</param>
	''' <param name="documentNumber">The document number.</param>
	Private Function LoadDocumentDetailData(ByVal employeeNumber As Integer, ByVal documentNumber As Integer) As Boolean

		' Clear errors
		errorProviderDocumentMangement.Clear()

		Dim documentSearchResult = m_EmployeeDbAccess.LoadEmployeeDocuments(employeeNumber, documentNumber, Nothing)

		If Not documentSearchResult Is Nothing AndAlso documentSearchResult.Count = 1 Then

			Dim documentData = documentSearchResult(0)

			lueCategory.EditValue = documentData.CategorieNumber
			txtTitle.Text = documentData.Name
			txtDescription.Text = documentData.Description
			txtFilePath.Text = documentData.FileFullPath

			m_CurrentFileBytes = Nothing
			m_CurrentFileExtension = Nothing

			If File.Exists(documentData.FileFullPath) Then
				m_CurrentFileBytes = m_Utility.LoadFileBytes(documentData.FileFullPath)
				m_CurrentFileExtension = System.IO.Path.GetExtension(documentData.FileFullPath)
				If Not m_CurrentFileExtension Is Nothing Then
					m_CurrentFileExtension = m_CurrentFileExtension.Replace(".", "")
				End If
			End If

			lblDocumentCreated.Text = String.Format("{0:f}, {1}", documentData.CreatedOn, documentData.CreatedFrom)
			lblDocumentChanged.Text = String.Format("{0:f}, {1}", documentData.ChangedOn, documentData.ChangedFrom)

			m_CurrentDocumentRecordNumber = documentData.DocumentRecordNumber

			Return True
		Else
			Return False
		End If

	End Function

	''' <summary>
	''' Focuses a document.
	''' </summary>
	''' <param name="employeeNumber">The employee number.</param>
	''' <param name="documentRecordNumber">The document record number</param>
	Private Sub FocusDocument(ByVal employeeNumber As Integer, ByVal documentRecordNumber As Integer)

		If Not gridDocuments.DataSource Is Nothing Then

			Dim documentViewData = CType(gvDocuments.DataSource, BindingList(Of DocumentViewData))

			Dim index = documentViewData.ToList().FindIndex(Function(data) data.EmployeeNumber = employeeNumber And data.DocumentRecorNumber = documentRecordNumber)

			m_SuppressUIEvents = True
			Dim rowHandle = gvDocuments.GetRowHandle(index)
			gvDocuments.FocusedRowHandle = rowHandle
			m_SuppressUIEvents = False
		End If

	End Sub

	''' <summary>
	''' Focus a category filter.
	''' </summary>
	''' <param name="categoryNumber">The category filter number.</param>
	Private Sub FocusCategoryFilter(ByVal categoryNumber As Integer)

		If Not lstCategorie.DataSource Is Nothing Then

			Dim categoryViewData = (CType(lstCategorie.DataSource, List(Of CategoryVieData)))

			Dim index = categoryViewData.FindIndex(Function(data) data.CategoryNumber.HasValue AndAlso data.CategoryNumber = categoryNumber)

			If index = -1 Then
				index = categoryViewData.FindIndex(Function(data) Not data.CategoryNumber.HasValue)
			End If

			m_SuppressUIEvents = True
			lstCategorie.SelectedIndex = index
			m_SuppressUIEvents = False

		End If

	End Sub

	''' <summary>
	''' Prepare form for new document.
	''' </summary>
	Private Sub PrepareForNew()

		m_CurrentFileBytes = Nothing
		m_CurrentFileExtension = Nothing
		m_CurrentDocumentRecordNumber = Nothing
		lueCategory.EditValue = Nothing
		txtTitle.Text = String.Empty
		txtDescription.Text = String.Empty
		txtFilePath.Text = String.Empty

		lblDocumentCreated.Text = "-"
		lblDocumentChanged.Text = "-"

		' Clear errors
		errorProviderDocumentMangement.Clear()

	End Sub

	''' <summary>
	''' Validates document input data.
	''' </summary>
	Private Function ValidateDocumentInputData() As Boolean

		errorProviderDocumentMangement.Clear()

		Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")
		Dim errorTextMissingFile As String = m_Translate.GetSafeTranslationValue("Bitte wählen Sie ein Datei aus.")

		Dim isValid As Boolean = True

		isValid = isValid And SetErrorIfInvalid(txtTitle, errorProviderDocumentMangement, String.IsNullOrEmpty(txtTitle.Text), errorText)

		' New documents must have an attached file.
		If Not m_CurrentDocumentRecordNumber.HasValue AndAlso m_CurrentFileBytes Is Nothing Then

			SetErrorIfInvalid(txtFilePath, errorProviderDocumentMangement, True, errorTextMissingFile)
			LoadFileBytesWithFileDialg()

			If m_CurrentFileBytes Is Nothing Then
				isValid = False
			End If

		End If

		Return isValid
	End Function

	''' <summary>
	''' Validates a control.
	''' </summary>
	''' <param name="control">The control to validate.</param>
	''' <param name="errorProvider">The error providor.</param>
	''' <param name="invalid">Boolean flag if data is invalid.</param>
	''' <param name="errorText">The error text.</param>
	''' <returns>Valid flag</returns>
	Private Function SetErrorIfInvalid(ByVal control As Control, ByVal errorProvider As ErrorProvider, ByVal invalid As Boolean, ByVal errorText As String) As Boolean

		If (invalid) Then
			errorProvider.SetError(control, errorText)
		Else
			errorProvider.SetError(control, String.Empty)
		End If

		Return Not invalid

	End Function

#End Region

#Region "View helper classes"

	''' <summary>
	''' Responsible person view data.
	''' </summary>
	Class ResponsiblePersonViewData

		Public Property Name As String
		Public Property ResponsiblePersonRecordNumber As Integer?

	End Class

	''' <summary>
	''' Category view data.
	''' </summary>
	Class CategoryVieData

		Public Property CategoryNumber As Integer?
		Public Property Description As String

	End Class

	Class DocumentViewData
		Public Property ID As Integer
		Public Property EmployeeNumber As Integer
		Public Property DocumentRecorNumber As Integer
		Public Property Name As String
		Public Property ScanExtension As String
		Public Property FileFullPath As String
		Public Property CategoryNumber As Integer
		Public Property CreatedOn As DateTime
		Public Property Image As Image
		Public Property Checked As Boolean

	End Class


#End Region

End Class