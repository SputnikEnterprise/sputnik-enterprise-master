
Imports System.ComponentModel
Imports DevExpress.XtraEditors
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Common
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SPProgUtility.Mandanten
Imports DevExpress.XtraEditors.Controls
Imports SPProgUtility.SPTranslation
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Common.DataObjects
Imports System.IO
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Customer.DataObjects
Imports DevExpress.Utils.Animation
Imports System.Drawing
Imports System.Threading
Imports SP.DatabaseAccess.Listing.DataObjects
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraBars.Navigation
Imports System.Security
Imports DevExpress.XtraGrid.Views.Grid

Namespace UI

	Public Class frmImportData

#Region "private consts"

		Private Const MANDANT_XML_SETTING_STARTNUMBER As String = "MD_{0}/StartNr"

#End Region


		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		''' <summary>
		''' The Initialization data.
		''' </summary>
		Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass
		Private m_SourceInitializationData As SP.Infrastructure.Initialization.InitializeClass

		''' <summary>
		''' The translation value helper.
		''' </summary>
		Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

		''' <summary>
		''' The common database access.
		''' </summary>
		Protected m_CommonDatabaseAccess As ICommonDatabaseAccess
		Protected m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess
		Protected m_CustomerDatabaseAccess As ICustomerDatabaseAccess
		Protected m_SourceCommonDatabaseAccess As ICommonDatabaseAccess

		''' <summary>
		''' The employee data access object.
		''' </summary>
		Private m_ListingDatabaseAccess As IListingDatabaseAccess
		Private m_SourceListingDatabaseAccess As IListingDatabaseAccess
		Private m_SourceEmployeeDatabaseAccess As IEmployeeDatabaseAccess
		Private m_SourceCustomerDatabaseAccess As ICustomerDatabaseAccess


		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI
		Private m_EventLog As SPProgUtility.ClsEventLog
		Private m_LOGFileName As String

		''' <summary>
		''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
		''' </summary>
		Private m_SuppressUIEvents As Boolean = False

		Private m_MandantSettingsXml As SPProgUtility.CommonXmlUtility.SettingsXml
		Private m_StartNumberSetting As String

		Private m_MandantData As Mandant
		Private m_ConnectionString As String
		Private m_SourceConnectionString As String

		Private m_SourceMandantData As BindingList(Of RootMandantData)

		Private m_EmployeeData As BindingList(Of EmployeeMasterData)
		Private m_CustomerData As BindingList(Of CustomerMasterData)

		Private m_SourceEmployeeData As BindingList(Of EmployeeMasterViewData)
		Private m_SourceCustomerData As BindingList(Of CustomerMasterViewData)

		Private m_invalidEmployeeData As New BindingList(Of EmployeeMasterViewData)
		Private m_importedEmployeeData As New BindingList(Of EmployeeMasterData)
		Private m_invalidCustomerData As New BindingList(Of CustomerMasterViewData)
		Private m_importedCustomerData As New BindingList(Of CustomerMasterData)


		Private m_CurrentSourceEmployeeData As EmployeeMasterViewData
		Private m_CurrentSourceCustomerData As CustomerMasterViewData



		Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			DevExpress.UserSkins.BonusSkins.Register()
			DevExpress.Skins.SkinManager.EnableFormSkins()

			m_InitializationData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

			m_MandantData = New Mandant
			m_UtilityUI = New UtilityUI
			m_EventLog = New SPProgUtility.ClsEventLog


			m_SuppressUIEvents = True

			InitializeComponent()

			Reset()
			TranslateControls()

			Dim m_MandantXMLFile = m_InitializationData.MDData.MandantCurrentXMLFileName
			If System.IO.File.Exists(m_MandantXMLFile) Then
				m_MandantSettingsXml = New SPProgUtility.CommonXmlUtility.SettingsXml(m_MandantXMLFile)
			End If
			m_StartNumberSetting = String.Format(MANDANT_XML_SETTING_STARTNUMBER, m_InitializationData.MDData.MDNr)


			m_ConnectionString = m_InitializationData.MDData.MDDbConn
			m_CommonDatabaseAccess = New CommonDatabaseAccess(m_ConnectionString, m_InitializationData.UserData.UserLanguage)
			m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(m_ConnectionString, m_InitializationData.UserData.UserLanguage)
			m_CustomerDatabaseAccess = New CustomerDatabaseAccess(m_ConnectionString, m_InitializationData.UserData.UserLanguage)
			m_ListingDatabaseAccess = New ListingDatabaseAccess(m_ConnectionString, m_InitializationData.UserData.UserLanguage)

			If m_SourceMandantData Is Nothing Then
				Dim result = LoadMandantenData()
				result = result AndAlso LoadMandantenDropDown()

				lueSourceMD.EditValue = m_InitializationData.MDData.MDNr
				'lueDestMD.EditValue = m_InitializationData.MDData.MDNr

				'result = result AndAlso LoadDestEmployeeData()
				'result = result AndAlso LoadDestCustomerData()

			End If
			m_SuppressUIEvents = False

			AddHandler gvSourceEmployee.RowCountChanged, AddressOf OnGVSourceEmployee_RowCountChanged
			AddHandler gvInvalidEmployee.RowCountChanged, AddressOf OnGVSourceEmployee_RowCountChanged
			AddHandler gvImportedEmployee.RowCountChanged, AddressOf OnGVSourceEmployee_RowCountChanged
			AddHandler gvInvalidEmployee.RowCellClick, AddressOf OnInvalidEmployee_RowCellClick

			AddHandler gvSourceCustomer.RowCountChanged, AddressOf OnGVSourceCustomer_RowCountChanged
			AddHandler gvInvalidCustomer.RowCountChanged, AddressOf OnGVSourceCustomer_RowCountChanged
			AddHandler gvImportedCustomer.RowCountChanged, AddressOf OnGVSourceCustomer_RowCountChanged
			AddHandler gvInvalidCustomer.RowCellClick, AddressOf OnInvalidCustomer_RowCellClick

			AddHandler lueSourceMD.ButtonClick, AddressOf OnDropDown_ButtonClick

		End Sub


		Public Function LoadData() As Boolean
			Dim result As Boolean = True

			tgsCustomerBerufe.Visible = False
			If m_SourceMandantData Is Nothing Then
				result = result AndAlso LoadMandantenData()
				result = result AndAlso LoadMandantenDropDown()

				lueSourceMD.EditValue = m_InitializationData.MDData.MDNr
			End If

			If lueSourceMD.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(lueSourceMD.EditValue) Then Return False
			result = result AndAlso OpenSourceDatabaseConnection()

			Dim transiton As Transition = New Transition()
			transiton.Control = nfMain
			transiton.TransitionType = New SlideFadeTransition()
			Dim manager As TransitionManager = New TransitionManager()
			manager.Transitions.Add(transiton)

			manager.StartTransition(nfMain)
			If tileBar.SelectedItem Is employeesTileBarItem Then
				ResetSourceEmployeesGrid()
				tpEmployee.SelectedPage = tnpSourceEmployee

				LoadSourceEmployeeData()

			ElseIf tileBar.SelectedItem Is customersTileBarItem Then
				ResetSourceCustomerGrid()
				tpCustomer.SelectedPage = tnpSourceCustomer
				tgsCustomerBerufe.Visible = True

				If tgsSputnikTables.EditValue Then LoadSourceCustomerData() Else LoadSourceTwixCustomerData()

			End If
			nfMain.SelectedPageIndex = tileBarGroupTables.Items.IndexOf(tileBar.SelectedItem)

			manager.EndTransition()

			Return result
		End Function

		Private Sub Reset()

			tpEmployee.SelectedPage = tnpSourceEmployee

			ResetSourceMandantenDropDown()

			ResetSourceEmployeesGrid()
			ResetInvalidEmployeesGrid()
			ResetImportedEmployeesGrid()

			ResetSourceCustomerGrid()
			ResetInvalidCustomersGrid()
			ResetImportedCustomersGrid()

		End Sub

		''' <summary>
		'''  Translate controls
		''' </summary>
		Private Sub TranslateControls()

			Text = m_Translate.GetSafeTranslationValue(Text)

		End Sub


#Region "Reset"

		''' <summary>
		''' Resets the Mandanten drop down.
		''' </summary>
		Private Sub ResetSourceMandantenDropDown()

			lueSourceMD.Properties.DisplayMember = "MandantName1"
			lueSourceMD.Properties.ValueMember = "MandantNumber"

			lueSourceMD.Properties.Columns.Clear()
			lueSourceMD.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "MandantName1",
																							 .Width = 100,
																							 .Caption = m_Translate.GetSafeTranslationValue("Mandant")})

			lueSourceMD.Properties.ShowHeader = False
			lueSourceMD.Properties.ShowFooter = False

			lueSourceMD.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueSourceMD.Properties.SearchMode = SearchMode.AutoComplete
			lueSourceMD.Properties.AutoSearchColumnIndex = 0

			lueSourceMD.Properties.NullText = String.Empty
			lueSourceMD.EditValue = Nothing

		End Sub


		''' <summary>
		''' Resets the source employees grid.
		''' </summary>
		Private Sub ResetSourceEmployeesGrid()

			gvSourceEmployee.OptionsView.ShowIndicator = False
			gvSourceEmployee.OptionsBehavior.Editable = True
			gvSourceEmployee.OptionsView.ShowAutoFilterRow = True
			gvSourceEmployee.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvSourceEmployee.OptionsView.ShowFooter = False
			gvSourceEmployee.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False


			gvSourceEmployee.Columns.Clear()

			Dim columnSelected As New DevExpress.XtraGrid.Columns.GridColumn()
			columnSelected.OptionsColumn.AllowEdit = True
			columnSelected.Caption = m_Translate.GetSafeTranslationValue("Auswahl")
			columnSelected.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			columnSelected.Name = "Selected"
			columnSelected.FieldName = "Selected"
			columnSelected.Visible = True
			gvSourceEmployee.Columns.Add(columnSelected)

			Dim columnName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnName.OptionsColumn.AllowEdit = False
			columnName.Caption = m_Translate.GetSafeTranslationValue("Name")
			columnName.Name = "EmployeeFullnameWithComma"
			columnName.FieldName = "EmployeeFullnameWithComma"
			columnName.Visible = True
			gvSourceEmployee.Columns.Add(columnName)

			Dim columnPostcode As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPostcode.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnPostcode.OptionsColumn.AllowEdit = False
			columnPostcode.Caption = m_Translate.GetSafeTranslationValue("PLZ")
			columnPostcode.Name = "Postcode"
			columnPostcode.FieldName = "Postcode"
			columnPostcode.Visible = False
			gvSourceEmployee.Columns.Add(columnPostcode)

			Dim columnAddress As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAddress.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnAddress.OptionsColumn.AllowEdit = False
			columnAddress.Caption = m_Translate.GetSafeTranslationValue("Adresse")
			columnAddress.Name = "EmployeeCompleteAddress"
			columnAddress.FieldName = "EmployeeCompleteAddress"
			columnAddress.Visible = True
			gvSourceEmployee.Columns.Add(columnAddress)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCreatedOn.OptionsColumn.AllowEdit = False
			columnCreatedOn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default
			columnCreatedOn.AppearanceHeader.Options.UseTextOptions = True
			columnCreatedOn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
			columnCreatedOn.DisplayFormat.FormatString = "G"
			columnCreatedOn.Caption = m_Translate.GetSafeTranslationValue("Quelle erstellt")
			columnCreatedOn.Name = "CreatedOn"
			columnCreatedOn.FieldName = "CreatedOn"
			columnCreatedOn.Visible = True
			gvSourceEmployee.Columns.Add(columnCreatedOn)

			Dim columnChangedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnChangedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnChangedOn.OptionsColumn.AllowEdit = False
			columnChangedOn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default
			columnChangedOn.AppearanceHeader.Options.UseTextOptions = True
			columnChangedOn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
			columnChangedOn.DisplayFormat.FormatString = "G"
			columnChangedOn.Caption = m_Translate.GetSafeTranslationValue("Quelle geändert")
			columnChangedOn.Name = "ChangedOn"
			columnChangedOn.FieldName = "ChangedOn"
			columnChangedOn.Visible = True
			gvSourceEmployee.Columns.Add(columnChangedOn)


			grdSourceEmployee.DataSource = Nothing

		End Sub

		Private Sub ResetInvalidEmployeesGrid()

			gvInvalidEmployee.OptionsView.ShowIndicator = False
			gvInvalidEmployee.OptionsBehavior.Editable = True
			gvInvalidEmployee.OptionsView.ShowAutoFilterRow = True
			gvInvalidEmployee.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvInvalidEmployee.OptionsView.ShowFooter = False
			gvInvalidEmployee.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False


			gvInvalidEmployee.Columns.Clear()

			Dim columnSelected As New DevExpress.XtraGrid.Columns.GridColumn()
			columnSelected.OptionsColumn.AllowEdit = True
			columnSelected.Caption = m_Translate.GetSafeTranslationValue("Auswahl")
			columnSelected.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			columnSelected.Name = "Selected"
			columnSelected.FieldName = "Selected"
			columnSelected.Visible = True
			gvInvalidEmployee.Columns.Add(columnSelected)

			Dim columnName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnName.OptionsColumn.AllowEdit = False
			columnName.Caption = m_Translate.GetSafeTranslationValue("Name")
			columnName.Name = "EmployeeFullnameWithComma"
			columnName.FieldName = "EmployeeFullnameWithComma"
			columnName.Visible = True
			gvInvalidEmployee.Columns.Add(columnName)

			Dim columnPostcode As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPostcode.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnPostcode.OptionsColumn.AllowEdit = False
			columnPostcode.Caption = m_Translate.GetSafeTranslationValue("PLZ")
			columnPostcode.Name = "Postcode"
			columnPostcode.FieldName = "Postcode"
			columnPostcode.Visible = False
			gvInvalidEmployee.Columns.Add(columnPostcode)

			Dim columnAddress As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAddress.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnAddress.OptionsColumn.AllowEdit = False
			columnAddress.Caption = m_Translate.GetSafeTranslationValue("Quelle Adresse")
			columnAddress.Name = "EmployeeCompleteAddress"
			columnAddress.FieldName = "EmployeeCompleteAddress"
			columnAddress.Visible = True
			gvInvalidEmployee.Columns.Add(columnAddress)

			Dim columnDestCompleteAddress As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDestCompleteAddress.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDestCompleteAddress.OptionsColumn.AllowEdit = False
			columnDestCompleteAddress.AppearanceCell.BackColor = Color.LightGray
			columnDestCompleteAddress.AppearanceCell.Options.UseBackColor = True
			columnDestCompleteAddress.Caption = m_Translate.GetSafeTranslationValue("Ziel-Adresse")
			columnDestCompleteAddress.Name = "DestCompleteAddress"
			columnDestCompleteAddress.FieldName = "DestCompleteAddress"
			columnDestCompleteAddress.Visible = True
			gvInvalidEmployee.Columns.Add(columnDestCompleteAddress)

			Dim columnSourceCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnSourceCreatedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnSourceCreatedOn.OptionsColumn.AllowEdit = False
			columnSourceCreatedOn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default
			columnSourceCreatedOn.AppearanceHeader.Options.UseTextOptions = True
			columnSourceCreatedOn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
			columnSourceCreatedOn.DisplayFormat.FormatString = "G"
			columnSourceCreatedOn.Caption = m_Translate.GetSafeTranslationValue("Quelle erstellt")
			columnSourceCreatedOn.Name = "SourceCreatedOn"
			columnSourceCreatedOn.FieldName = "SourceCreatedOn"
			columnSourceCreatedOn.Visible = True
			gvInvalidEmployee.Columns.Add(columnSourceCreatedOn)

			Dim columnDestCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDestCreatedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDestCreatedOn.OptionsColumn.AllowEdit = False
			columnDestCreatedOn.AppearanceCell.BackColor = Color.LightGray
			columnDestCreatedOn.AppearanceCell.Options.UseBackColor = True
			columnDestCreatedOn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default
			columnDestCreatedOn.AppearanceHeader.Options.UseTextOptions = True
			columnDestCreatedOn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
			columnDestCreatedOn.DisplayFormat.FormatString = "G"
			columnDestCreatedOn.Caption = m_Translate.GetSafeTranslationValue("Ziel erstellt")
			columnDestCreatedOn.Name = "DestCreatedOn"
			columnDestCreatedOn.FieldName = "DestCreatedOn"
			columnDestCreatedOn.Visible = True
			gvInvalidEmployee.Columns.Add(columnDestCreatedOn)

			Dim columnSourceChangedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnSourceChangedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnSourceChangedOn.OptionsColumn.AllowEdit = False
			columnSourceChangedOn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default
			columnSourceChangedOn.AppearanceHeader.Options.UseTextOptions = True
			columnSourceChangedOn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
			columnSourceChangedOn.DisplayFormat.FormatString = "G"
			columnSourceChangedOn.Caption = m_Translate.GetSafeTranslationValue("Quelle geändert")
			columnSourceChangedOn.Name = "SourceChangedOn"
			columnSourceChangedOn.FieldName = "SourceChangedOn"
			columnSourceChangedOn.Visible = True
			gvInvalidEmployee.Columns.Add(columnSourceChangedOn)

			Dim columnDestChangedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDestChangedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDestChangedOn.OptionsColumn.AllowEdit = False
			columnDestChangedOn.AppearanceCell.BackColor = Color.LightGray
			columnDestChangedOn.AppearanceCell.Options.UseBackColor = True
			columnDestChangedOn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default
			columnDestChangedOn.AppearanceHeader.Options.UseTextOptions = True
			columnDestChangedOn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
			columnDestChangedOn.DisplayFormat.FormatString = "G"
			columnDestChangedOn.Caption = m_Translate.GetSafeTranslationValue("Ziel geändert")
			columnDestChangedOn.Name = "DestChangedOn"
			columnDestChangedOn.FieldName = "DestChangedOn"
			columnDestChangedOn.Visible = True
			gvInvalidEmployee.Columns.Add(columnDestChangedOn)


			Dim m_CheckEditNew = New RepositoryItemButtonEdit With {.Name = "m_CheckEditNew", .Tag = 1, .ButtonsStyle = BorderStyles.Default, .HideSelection = True, .TextEditStyle = TextEditStyles.HideTextEditor}
			m_CheckEditNew.Buttons.Clear()
			m_CheckEditNew.Buttons.AddRange(New EditorButton() {New EditorButton(ButtonPredefines.Glyph, "Hinzufügen", -1, True, True, True, ImageLocation.MiddleRight, DemoHelper.GetEditImage())})
			m_CheckEditNew.Buttons(0).ImageOptions.Image = My.Resources.newcontact_16x16
			grdInvalidEmployee.RepositoryItems.Add(m_CheckEditNew)
			AddHandler m_CheckEditNew.ButtonClick, AddressOf OnGVInvalidEmployee_AddButtonClick

			Dim m_CheckEditUpdate = New RepositoryItemButtonEdit With {.Name = "m_CheckEditUpdate", .Tag = 2, .ButtonsStyle = BorderStyles.Default, .HideSelection = True, .TextEditStyle = TextEditStyles.HideTextEditor}
			m_CheckEditUpdate.Buttons.Clear()
			m_CheckEditUpdate.Buttons.AddRange(New EditorButton() {New EditorButton(ButtonPredefines.Glyph, "Überschreiben", -1, True, True, True, ImageLocation.MiddleRight, DemoHelper.GetEditImage())})
			m_CheckEditUpdate.Buttons(0).ImageOptions.Image = My.Resources.editcontact_16x16
			grdInvalidEmployee.RepositoryItems.Add(m_CheckEditUpdate)
			AddHandler m_CheckEditUpdate.ButtonClick, AddressOf OnGVInvalidEmployee_UpdateButtonClick

			'Dim commandsEdit = New RepositoryItemButtonEdit With {.Name = "commandsEdit", .ButtonsStyle = BorderStyles.Default, .HideSelection = True, .TextEditStyle = TextEditStyles.HideTextEditor}
			'commandsEdit.Buttons.Clear()
			'commandsEdit.Buttons.AddRange(New EditorButton() {New EditorButton(ButtonPredefines.Glyph, "Überschreiben", -1, True, True, True, ImageLocation.MiddleRight, DemoHelper.GetEditImage()),
			'							  New EditorButton(ButtonPredefines.Glyph, "Hinzufügen", -1, True, True, False, ImageLocation.MiddleLeft, DemoHelper.GetEditImage())
			'							  })
			'commandsEdit.Buttons(0).ImageOptions.Image = My.Resources.editcontact_16x16
			'commandsEdit.Buttons(1).ImageOptions.Image = My.Resources.newcontact_16x16
			'grdInvalidEmployee.RepositoryItems.Add(commandsEdit)
			'AddHandler commandsEdit.ButtonClick, AddressOf OnGVInvalidEmployee_ButtonClick

			'Dim columnUpdate_OLD As New DevExpress.XtraGrid.Columns.GridColumn()
			'columnUpdate_OLD.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			'columnUpdate_OLD.OptionsColumn.AllowEdit = True
			'columnUpdate_OLD.Caption = " "
			'columnUpdate_OLD.Name = "Update_OLD"
			'columnUpdate_OLD.FieldName = "Update_OLD"
			'columnUpdate_OLD.Visible = False
			'columnUpdate_OLD.Width = 200
			''columnUpdate_OLD.BestFit()
			'columnUpdate_OLD.ColumnEdit = commandsEdit
			'gvInvalidEmployee.Columns.Add(columnUpdate_OLD)
			'columnUpdate_OLD.ShowButtonMode = ShowButtonModeEnum.ShowAlways

			Dim columnUpdate As New DevExpress.XtraGrid.Columns.GridColumn()
			columnUpdate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			columnUpdate.OptionsColumn.AllowEdit = True
			columnUpdate.Caption = " "
			columnUpdate.Name = "Update"
			columnUpdate.FieldName = "Update"
			columnUpdate.Visible = True
			columnUpdate.BestFit()
			columnUpdate.ColumnEdit = m_CheckEditUpdate
			gvInvalidEmployee.Columns.Add(columnUpdate)
			columnUpdate.ShowButtonMode = ShowButtonModeEnum.ShowAlways

			Dim columnAdd As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAdd.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			columnAdd.OptionsColumn.AllowEdit = True
			columnAdd.Caption = " "
			columnAdd.Name = "Add"
			columnAdd.FieldName = "Add"
			columnAdd.Visible = True
			columnAdd.BestFit()
			columnAdd.ColumnEdit = m_CheckEditNew
			gvInvalidEmployee.Columns.Add(columnAdd)
			columnAdd.ShowButtonMode = ShowButtonModeEnum.ShowAlways


			grdInvalidEmployee.DataSource = Nothing

		End Sub

		Private Sub ResetImportedEmployeesGrid()

			gvImportedEmployee.OptionsView.ShowIndicator = False
			gvImportedEmployee.OptionsBehavior.Editable = True
			gvImportedEmployee.OptionsView.ShowAutoFilterRow = True
			gvImportedEmployee.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvImportedEmployee.OptionsView.ShowFooter = False
			gvImportedEmployee.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False


			gvImportedEmployee.Columns.Clear()

			Dim columnEmployeeNumber As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeeNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnEmployeeNumber.OptionsColumn.AllowEdit = True
			columnEmployeeNumber.Caption = m_Translate.GetSafeTranslationValue("MANr")
			columnEmployeeNumber.Name = "EmployeeNumber"
			columnEmployeeNumber.FieldName = "EmployeeNumber"
			columnEmployeeNumber.Visible = True
			gvImportedEmployee.Columns.Add(columnEmployeeNumber)

			Dim columnName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnName.OptionsColumn.AllowEdit = False
			columnName.Caption = m_Translate.GetSafeTranslationValue("Name")
			columnName.Name = "EmployeeFullnameWithComma"
			columnName.FieldName = "EmployeeFullnameWithComma"
			columnName.Visible = True
			gvImportedEmployee.Columns.Add(columnName)

			Dim columnPostcode As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPostcode.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnPostcode.OptionsColumn.AllowEdit = False
			columnPostcode.Caption = m_Translate.GetSafeTranslationValue("PLZ")
			columnPostcode.Name = "Postcode"
			columnPostcode.FieldName = "Postcode"
			columnPostcode.Visible = False
			gvImportedEmployee.Columns.Add(columnPostcode)

			Dim columnAddress As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAddress.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnAddress.OptionsColumn.AllowEdit = False
			columnAddress.Caption = m_Translate.GetSafeTranslationValue("Adresse")
			columnAddress.Name = "EmployeeCompleteAddress"
			columnAddress.FieldName = "EmployeeCompleteAddress"
			columnAddress.Visible = True
			gvImportedEmployee.Columns.Add(columnAddress)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCreatedOn.OptionsColumn.AllowEdit = False
			columnCreatedOn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default
			columnCreatedOn.AppearanceHeader.Options.UseTextOptions = True
			columnCreatedOn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
			columnCreatedOn.DisplayFormat.FormatString = "G"
			columnCreatedOn.Caption = m_Translate.GetSafeTranslationValue("Quelle erstellt")
			columnCreatedOn.Name = "CreatedOn"
			columnCreatedOn.FieldName = "CreatedOn"
			columnCreatedOn.Visible = True
			gvImportedEmployee.Columns.Add(columnCreatedOn)

			Dim columnChangedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnChangedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnChangedOn.OptionsColumn.AllowEdit = False
			columnChangedOn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default
			columnChangedOn.AppearanceHeader.Options.UseTextOptions = True
			columnChangedOn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
			columnChangedOn.DisplayFormat.FormatString = "G"
			columnChangedOn.Caption = m_Translate.GetSafeTranslationValue("Quelle geändert")
			columnChangedOn.Name = "ChangedOn"
			columnChangedOn.FieldName = "ChangedOn"
			columnChangedOn.Visible = True
			gvImportedEmployee.Columns.Add(columnChangedOn)


			grdImportedEmployee.DataSource = Nothing

		End Sub

		''' <summary>
		''' Resets the source Customer grid.
		''' </summary>
		Private Sub ResetSourceCustomerGrid()

			gvSourceCustomer.OptionsView.ShowIndicator = False
			gvSourceCustomer.OptionsBehavior.Editable = True
			gvSourceCustomer.OptionsView.ShowAutoFilterRow = True
			gvSourceCustomer.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvSourceCustomer.OptionsView.ShowFooter = False
			gvSourceCustomer.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False


			gvSourceCustomer.Columns.Clear()

			Dim columnSelected As New DevExpress.XtraGrid.Columns.GridColumn()
			columnSelected.OptionsColumn.AllowEdit = True
			columnSelected.Caption = m_Translate.GetSafeTranslationValue("Auswahl")
			columnSelected.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			columnSelected.Name = "Selected"
			columnSelected.FieldName = "Selected"
			columnSelected.Visible = True
			gvSourceCustomer.Columns.Add(columnSelected)

			Dim columnName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnName.OptionsColumn.AllowEdit = False
			columnName.Caption = m_Translate.GetSafeTranslationValue("Kunde")
			columnName.Name = "Company1"
			columnName.FieldName = "Company1"
			columnName.Visible = True
			gvSourceCustomer.Columns.Add(columnName)

			Dim columnPostcode As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPostcode.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnPostcode.OptionsColumn.AllowEdit = False
			columnPostcode.Caption = m_Translate.GetSafeTranslationValue("PLZ")
			columnPostcode.Name = "Postcode"
			columnPostcode.FieldName = "Postcode"
			columnPostcode.Visible = False
			gvSourceCustomer.Columns.Add(columnPostcode)

			Dim columnAddress As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAddress.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnAddress.OptionsColumn.AllowEdit = False
			columnAddress.Caption = m_Translate.GetSafeTranslationValue("Adresse")
			columnAddress.Name = "CustomerCompleteAddress"
			columnAddress.FieldName = "CustomerCompleteAddress"
			columnAddress.Visible = True
			gvSourceCustomer.Columns.Add(columnAddress)

			Dim columnAdvisor As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAdvisor.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnAdvisor.OptionsColumn.AllowEdit = False
			columnAdvisor.Caption = m_Translate.GetSafeTranslationValue("Berater")
			columnAdvisor.Name = "KST"
			columnAdvisor.FieldName = "KST"
			columnAdvisor.Visible = True
			gvSourceCustomer.Columns.Add(columnAdvisor)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCreatedOn.OptionsColumn.AllowEdit = False
			columnCreatedOn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default
			columnCreatedOn.AppearanceHeader.Options.UseTextOptions = True
			columnCreatedOn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
			columnCreatedOn.DisplayFormat.FormatString = "G"
			columnCreatedOn.Caption = m_Translate.GetSafeTranslationValue("Quelle erstellt")
			columnCreatedOn.Name = "CreatedOn"
			columnCreatedOn.FieldName = "CreatedOn"
			columnCreatedOn.Visible = True
			gvSourceCustomer.Columns.Add(columnCreatedOn)

			Dim columnChangedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnChangedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnChangedOn.OptionsColumn.AllowEdit = False
			columnChangedOn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default
			columnChangedOn.AppearanceHeader.Options.UseTextOptions = True
			columnChangedOn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
			columnChangedOn.DisplayFormat.FormatString = "G"
			columnChangedOn.Caption = m_Translate.GetSafeTranslationValue("Quelle geändert")
			columnChangedOn.Name = "ChangedOn"
			columnChangedOn.FieldName = "ChangedOn"
			columnChangedOn.Visible = True
			gvSourceCustomer.Columns.Add(columnChangedOn)


			grdSourceCustomer.DataSource = Nothing

		End Sub

		Private Sub ResetInvalidCustomersGrid()

			gvInvalidCustomer.OptionsView.ShowIndicator = False
			gvInvalidCustomer.OptionsBehavior.Editable = True
			gvInvalidCustomer.OptionsView.ShowAutoFilterRow = True
			gvInvalidCustomer.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvInvalidCustomer.OptionsView.ShowFooter = False
			gvInvalidCustomer.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False


			gvInvalidCustomer.Columns.Clear()

			Dim columnSelected As New DevExpress.XtraGrid.Columns.GridColumn()
			columnSelected.OptionsColumn.AllowEdit = True
			columnSelected.Caption = m_Translate.GetSafeTranslationValue("Auswahl")
			columnSelected.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			columnSelected.Name = "Selected"
			columnSelected.FieldName = "Selected"
			columnSelected.Visible = True
			gvInvalidCustomer.Columns.Add(columnSelected)

			Dim columnName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnName.OptionsColumn.AllowEdit = False
			columnName.Caption = m_Translate.GetSafeTranslationValue("Kunde")
			columnName.Name = "Company1"
			columnName.FieldName = "Company1"
			columnName.Visible = True
			gvInvalidCustomer.Columns.Add(columnName)

			Dim columnPostcode As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPostcode.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnPostcode.OptionsColumn.AllowEdit = False
			columnPostcode.Caption = m_Translate.GetSafeTranslationValue("PLZ")
			columnPostcode.Name = "Postcode"
			columnPostcode.FieldName = "Postcode"
			columnPostcode.Visible = False
			gvInvalidCustomer.Columns.Add(columnPostcode)

			Dim columnAddress As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAddress.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnAddress.OptionsColumn.AllowEdit = False
			columnAddress.Caption = m_Translate.GetSafeTranslationValue("Quelle Adresse")
			columnAddress.Name = "CustomerCompleteAddress"
			columnAddress.FieldName = "CustomerCompleteAddress"
			columnAddress.Visible = True
			gvInvalidCustomer.Columns.Add(columnAddress)

			Dim columnDestCompleteAddress As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDestCompleteAddress.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDestCompleteAddress.OptionsColumn.AllowEdit = False
			columnDestCompleteAddress.AppearanceCell.BackColor = Color.LightGray
			columnDestCompleteAddress.AppearanceCell.Options.UseBackColor = True
			columnDestCompleteAddress.Caption = m_Translate.GetSafeTranslationValue("Ziel Adresse")
			columnDestCompleteAddress.Name = "DestCompleteAddress"
			columnDestCompleteAddress.FieldName = "DestCompleteAddress"
			columnDestCompleteAddress.Visible = True
			gvInvalidCustomer.Columns.Add(columnDestCompleteAddress)

			Dim columnAdvisor As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAdvisor.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnAdvisor.OptionsColumn.AllowEdit = False
			columnAdvisor.Caption = m_Translate.GetSafeTranslationValue("Berater")
			columnAdvisor.Name = "KST"
			columnAdvisor.FieldName = "KST"
			columnAdvisor.Visible = True
			gvInvalidCustomer.Columns.Add(columnAdvisor)

			Dim columnSourceCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnSourceCreatedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnSourceCreatedOn.OptionsColumn.AllowEdit = False
			columnSourceCreatedOn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default
			columnSourceCreatedOn.AppearanceHeader.Options.UseTextOptions = True
			columnSourceCreatedOn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
			columnSourceCreatedOn.DisplayFormat.FormatString = "G"
			columnSourceCreatedOn.Caption = m_Translate.GetSafeTranslationValue("Quelle erstellt")
			columnSourceCreatedOn.Name = "SourceCreatedOn"
			columnSourceCreatedOn.FieldName = "SourceCreatedOn"
			columnSourceCreatedOn.Visible = True
			gvInvalidCustomer.Columns.Add(columnSourceCreatedOn)

			Dim columnDestCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDestCreatedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDestCreatedOn.OptionsColumn.AllowEdit = False
			columnDestCreatedOn.AppearanceCell.BackColor = Color.LightGray
			columnDestCreatedOn.AppearanceCell.Options.UseBackColor = True
			columnDestCreatedOn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default
			columnDestCreatedOn.AppearanceHeader.Options.UseTextOptions = True
			columnDestCreatedOn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
			columnDestCreatedOn.DisplayFormat.FormatString = "G"
			columnDestCreatedOn.Caption = m_Translate.GetSafeTranslationValue("Ziel erstellt")
			columnDestCreatedOn.Name = "DestCreatedOn"
			columnDestCreatedOn.FieldName = "DestCreatedOn"
			columnDestCreatedOn.Visible = True
			gvInvalidCustomer.Columns.Add(columnDestCreatedOn)

			Dim columnSourceChangedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnSourceChangedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnSourceChangedOn.OptionsColumn.AllowEdit = False
			columnSourceChangedOn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default
			columnSourceChangedOn.AppearanceHeader.Options.UseTextOptions = True
			columnSourceChangedOn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
			columnSourceChangedOn.DisplayFormat.FormatString = "G"
			columnSourceChangedOn.Caption = m_Translate.GetSafeTranslationValue("Quelle geändert")
			columnSourceChangedOn.Name = "SourceChangedOn"
			columnSourceChangedOn.FieldName = "SourceChangedOn"
			columnSourceChangedOn.Visible = True
			gvInvalidCustomer.Columns.Add(columnSourceChangedOn)

			Dim columnDestChangedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDestChangedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDestChangedOn.OptionsColumn.AllowEdit = False
			columnDestChangedOn.AppearanceCell.BackColor = Color.LightGray
			columnDestChangedOn.AppearanceCell.Options.UseBackColor = True
			columnDestChangedOn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default
			columnDestChangedOn.AppearanceHeader.Options.UseTextOptions = True
			columnDestChangedOn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
			columnDestChangedOn.DisplayFormat.FormatString = "G"
			columnDestChangedOn.Caption = m_Translate.GetSafeTranslationValue("Ziel geändert")
			columnDestChangedOn.Name = "DestChangedOn"
			columnDestChangedOn.FieldName = "DestChangedOn"
			columnDestChangedOn.Visible = True
			gvInvalidCustomer.Columns.Add(columnDestChangedOn)


			Dim m_CheckEditNew = New RepositoryItemButtonEdit With {.Name = "m_CheckEditNew", .Tag = 1, .ButtonsStyle = BorderStyles.Default, .HideSelection = True, .TextEditStyle = TextEditStyles.HideTextEditor}
			m_CheckEditNew.Buttons.Clear()
			m_CheckEditNew.Buttons.AddRange(New EditorButton() {New EditorButton(ButtonPredefines.Glyph, "Hinzufügen", -1, True, True, True, ImageLocation.MiddleRight, DemoHelper.GetEditImage())})
			m_CheckEditNew.Buttons(0).ImageOptions.Image = My.Resources.newcontact_16x16
			grdInvalidCustomer.RepositoryItems.Add(m_CheckEditNew)
			AddHandler m_CheckEditNew.ButtonClick, AddressOf OnGVInvalidCustomer_AddButtonClick

			Dim m_CheckEditUpdate = New RepositoryItemButtonEdit With {.Name = "m_CheckEditUpdate", .Tag = 2, .ButtonsStyle = BorderStyles.Default, .HideSelection = True, .TextEditStyle = TextEditStyles.HideTextEditor}
			m_CheckEditUpdate.Buttons.Clear()
			m_CheckEditUpdate.Buttons.AddRange(New EditorButton() {New EditorButton(ButtonPredefines.Glyph, "Überschreiben", -1, True, True, True, ImageLocation.MiddleRight, DemoHelper.GetEditImage())})
			m_CheckEditUpdate.Buttons(0).ImageOptions.Image = My.Resources.editcontact_16x16
			grdInvalidCustomer.RepositoryItems.Add(m_CheckEditUpdate)
			AddHandler m_CheckEditUpdate.ButtonClick, AddressOf OnGVInvalidCustomer_UpdateButtonClick



			'Dim commandsEdit = New RepositoryItemButtonEdit With {.Name = "commandsEdit", .ButtonsStyle = BorderStyles.Default, .HideSelection = True, .TextEditStyle = TextEditStyles.HideTextEditor}
			'commandsEdit.Buttons.Clear()
			'commandsEdit.Buttons.AddRange(New EditorButton() {New EditorButton(ButtonPredefines.Glyph, "Überschreiben", -1, True, True, True, ImageLocation.MiddleRight, DemoHelper.GetEditImage()),
			'							  New EditorButton(ButtonPredefines.Glyph, "Hinzufügen", -1, True, True, False, ImageLocation.MiddleLeft, DemoHelper.GetEditImage())
			'							  })
			'commandsEdit.Buttons(0).ImageOptions.Image = My.Resources.editcontact_16x16
			'commandsEdit.Buttons(1).ImageOptions.Image = My.Resources.newcontact_16x16
			'grdInvalidCustomer.RepositoryItems.Add(commandsEdit)
			'AddHandler commandsEdit.ButtonClick, AddressOf OnGVInvalidCustomer_ButtonClick




			'Dim columnUpdate_OLD As New DevExpress.XtraGrid.Columns.GridColumn()
			'columnUpdate_OLD.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			'columnUpdate_OLD.OptionsColumn.AllowEdit = True
			'columnUpdate_OLD.Caption = " "
			'columnUpdate_OLD.Name = "Update_OLD"
			'columnUpdate_OLD.FieldName = "Update_OLD"
			'columnUpdate_OLD.Visible = False
			'columnUpdate_OLD.Width = 200
			''columnUpdate_OLD.BestFit()
			'columnUpdate_OLD.ColumnEdit = commandsEdit
			'gvInvalidCustomer.Columns.Add(columnUpdate_OLD)
			'columnUpdate_OLD.ShowButtonMode = ShowButtonModeEnum.ShowAlways

			Dim columnUpdate As New DevExpress.XtraGrid.Columns.GridColumn()
			columnUpdate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			columnUpdate.OptionsColumn.AllowEdit = True
			columnUpdate.Caption = " "
			columnUpdate.Name = "Update"
			columnUpdate.FieldName = "Update"
			columnUpdate.Visible = True
			columnUpdate.BestFit()
			columnUpdate.ColumnEdit = m_CheckEditUpdate
			gvInvalidCustomer.Columns.Add(columnUpdate)
			columnUpdate.ShowButtonMode = ShowButtonModeEnum.ShowAlways

			Dim columnAdd As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAdd.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			columnAdd.OptionsColumn.AllowEdit = True
			columnAdd.Caption = " "
			columnAdd.Name = "Add"
			columnAdd.FieldName = "Add"
			columnAdd.Visible = True
			columnAdd.BestFit()
			columnAdd.ColumnEdit = m_CheckEditNew
			gvInvalidCustomer.Columns.Add(columnAdd)
			columnAdd.ShowButtonMode = ShowButtonModeEnum.ShowAlways


			grdInvalidCustomer.DataSource = Nothing

		End Sub

		Private Sub ResetImportedCustomersGrid()

			gvImportedCustomer.OptionsView.ShowIndicator = False
			gvImportedCustomer.OptionsBehavior.Editable = True
			gvImportedCustomer.OptionsView.ShowAutoFilterRow = True
			gvImportedCustomer.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvImportedCustomer.OptionsView.ShowFooter = False
			gvImportedCustomer.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False


			gvImportedCustomer.Columns.Clear()

			Dim columnNummer As New DevExpress.XtraGrid.Columns.GridColumn()
			columnNummer.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnNummer.OptionsColumn.AllowEdit = False
			columnNummer.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnNummer.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			columnNummer.Name = "CustomerNumber"
			columnNummer.FieldName = "CustomerNumber"
			columnNummer.Visible = True
			gvImportedCustomer.Columns.Add(columnNummer)

			Dim columnName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnName.OptionsColumn.AllowEdit = False
			columnName.Caption = m_Translate.GetSafeTranslationValue("Kunde")
			columnName.Name = "Company1"
			columnName.FieldName = "Company1"
			columnName.Visible = True
			gvImportedCustomer.Columns.Add(columnName)

			Dim columnPostcode As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPostcode.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnPostcode.OptionsColumn.AllowEdit = False
			columnPostcode.Caption = m_Translate.GetSafeTranslationValue("PLZ")
			columnPostcode.Name = "Postcode"
			columnPostcode.FieldName = "Postcode"
			columnPostcode.Visible = False
			gvImportedCustomer.Columns.Add(columnPostcode)

			Dim columnAddress As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAddress.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnAddress.OptionsColumn.AllowEdit = False
			columnAddress.Caption = m_Translate.GetSafeTranslationValue("Adresse")
			columnAddress.Name = "CustomerCompleteAddress"
			columnAddress.FieldName = "CustomerCompleteAddress"
			columnAddress.Visible = True
			gvImportedCustomer.Columns.Add(columnAddress)

			Dim columnAdvisor As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAdvisor.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnAdvisor.OptionsColumn.AllowEdit = False
			columnAdvisor.Caption = m_Translate.GetSafeTranslationValue("Berater")
			columnAdvisor.Name = "KST"
			columnAdvisor.FieldName = "KST"
			columnAdvisor.Visible = True
			gvImportedCustomer.Columns.Add(columnAdvisor)


			grdImportedCustomer.DataSource = Nothing

		End Sub

#End Region


		Private Function LoadMandantenDropDown() As Boolean

			If m_SourceMandantData Is Nothing Then Return False

			lueSourceMD.Properties.DataSource = m_SourceMandantData
			lueSourceMD.Properties.ForceInitialize()


			Return Not m_SourceMandantData Is Nothing

		End Function

		Private Function LoadMandantenData() As Boolean

			Dim data = m_CommonDatabaseAccess.LoadUpdateMandantData

			If data Is Nothing Then
				m_Logger.LogError("md data could not be founded!")

				Return False
			End If

			m_SourceMandantData = New BindingList(Of RootMandantData)
			For Each itm In data
				Dim mdPath As String = itm.MDPath
				If String.IsNullOrWhiteSpace(mdPath) Then Continue For

				If mdPath.EndsWith(Path.DirectorySeparatorChar) Then
					itm.DbConnectionstr = GetDbConnectionString(itm.MandantNumber, Path.GetDirectoryName(mdPath.Remove(Len(mdPath) - 1, 1)))
				Else
					itm.DbConnectionstr = GetDbConnectionString(itm.MandantNumber, Path.GetDirectoryName(mdPath))
				End If


				'Trace.WriteLine(String.Format("itm.DbConnectionstr: {0}", itm.DbConnectionstr))


				If Not String.IsNullOrWhiteSpace(itm.DbConnectionstr) Then
					Dim tempFile = Path.Combine(itm.MDPath, Year(Now), Path.GetRandomFileName)
					Try

						File.Create(tempFile).Dispose()
						File.Delete(tempFile)
						m_Logger.LogDebug(String.Format("searching for security: {0}", tempFile))

						m_SourceMandantData.Add(itm)

					Catch ex As Exception
						m_Logger.LogWarning(String.Format("directory access denied: {0}", itm.MDPath))
						m_Logger.LogDebug(String.Format("denied write and delete security: {0}", tempFile))

					End Try
				End If
			Next

			Return Not m_SourceMandantData Is Nothing

		End Function

		Function LoadDestEmployeeData() As Boolean
			Dim result As Boolean = True

			Dim data = m_ListingDatabaseAccess.LoadAllEmployeeMasterData(False)

			If data Is Nothing Then
				m_UtilityUI.ShowErrorDialog("Keine Kandidaten Daten im Ziel wurden gefunden.")

				Return False
			End If

			m_EmployeeData = New BindingList(Of EmployeeMasterData)
			For Each employeer In data
				m_EmployeeData.Add(employeer)
			Next

			Return Not m_EmployeeData Is Nothing
		End Function

		Function LoadDestCustomerData() As Boolean
			Dim result As Boolean = True

			Dim data = m_ListingDatabaseAccess.LoadAllCustomerMasterData()

			If data Is Nothing Then
				m_UtilityUI.ShowErrorDialog("Keine Kunden Daten im Ziel wurden gefunden.")

				Return False
			End If

			m_CustomerData = New BindingList(Of CustomerMasterData)
			For Each customer In data
				m_CustomerData.Add(customer)
			Next

			Return Not m_CustomerData Is Nothing
		End Function


		Private Sub lueSourceMD_EditValueChanged(sender As Object, e As EventArgs) Handles lueSourceMD.EditValueChanged
			If m_SuppressUIEvents Then Return

			OpenSourceDatabaseConnection()
			LoadData()

		End Sub

		Private Sub deDate_EditValueChanged(sender As Object, e As EventArgs) Handles deDate.EditValueChanged
			If m_SuppressUIEvents Then Return

			OpenSourceDatabaseConnection()
			LoadData()

		End Sub

		Private Sub OnDropDown_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

			Const ID_OF_RELOAD_BUTTON As Int32 = 1

			If e.Button.Index = ID_OF_RELOAD_BUTTON Then

				If TypeOf sender Is BaseEdit Then
					If CType(sender, BaseEdit).Properties.ReadOnly Then
						' nothing

					ElseIf TypeOf sender Is DateEdit Then
						Dim dateEdit As DateEdit = CType(sender, DateEdit)
						dateEdit.EditValue = Nothing

					Else
						OpenSourceDatabaseConnection()
						LoadData()

					End If
				End If

			End If
		End Sub

		Private Sub OnbtnImportData_Click(sender As Object, e As EventArgs) Handles btnImportData.Click

			Dim processGuid As String = Guid.NewGuid().ToString
			Dim logFilename As String = String.Format("{0}.{1}", processGuid, "tmp")
			m_LOGFileName = Path.Combine(m_InitializationData.UserData.SPTempPath, logFilename)


			m_EventLog.WriteTempLogFile(String.Format("Programmstart: {0}", Now.ToString), m_LOGFileName)

			If tileBar.SelectedItem Is employeesTileBarItem Then
				m_Logger.LogInfo(String.Format("Import Kandidatendaten von {0} >>> {1}", m_SourceInitializationData.MDData.MDDbName, m_InitializationData.MDData.MDDbName))
				m_EventLog.WriteTempLogFile(String.Format("Import Kandidatendaten von {0} >>> {1}", m_SourceInitializationData.MDData.MDDbName, m_InitializationData.MDData.MDDbName), m_LOGFileName)
				ImportEmployeesIntoCurrentDatabase()

			ElseIf tileBar.SelectedItem Is customersTileBarItem Then
				m_Logger.LogInfo(String.Format("Import Kundendaten von {0} >>> {1}", m_SourceInitializationData.MDData.MDDbName, m_InitializationData.MDData.MDDbName))
				m_EventLog.WriteTempLogFile(String.Format("Import Kundendaten von {0} >>> {1}", m_SourceInitializationData.MDData.MDDbName, m_InitializationData.MDData.MDDbName), m_LOGFileName)

				If tgsSputnikTables.EditValue Then
					If tgsCustomerBerufe.Visible AndAlso tgsCustomerBerufe.EditValue Then
						ImportCustomersPeripherieDataIntoCurrentDatabase()

					Else
						ImportCustomersIntoCurrentDatabase()

					End If

				Else
					ImportTwixCustomersIntoCurrentDatabase()

				End If

			End If
			tgsSourceSelection.EditValue = False
			m_Logger.LogInfo(String.Format("***Ende der Import: {0}", Now.ToString))
			m_EventLog.WriteTempLogFile(String.Format("***Ende der Import: {0}", Now.ToString), m_LOGFileName)
			m_EventLog.WriteTempLogFile(String.Join("", Enumerable.Repeat("=", 150)), m_LOGFileName)

			Process.Start("explorer.exe", "/select," & m_LOGFileName)

		End Sub

		Private Sub OnbtnUpData_Click(sender As Object, e As EventArgs) Handles btnUpdateData.Click

			If tileBar.SelectedItem Is employeesTileBarItem Then
				UpdateInvalidEmployeesWithSourceDatabase()

			ElseIf tileBar.SelectedItem Is customersTileBarItem Then

				If tgsSputnikTables.EditValue Then
					UpdateInvalidCustomersWithSourceDatabase()

				Else
					UpdateInvalidTwixCustomersWithSourceDatabase()

				End If

			End If

		End Sub

		Private Sub OnbtnNewData_Click(sender As Object, e As EventArgs) Handles btnNewData.Click

			If tileBar.SelectedItem Is employeesTileBarItem Then
				AddInvalidEmployeesIntoSourceDatabase()

			ElseIf tileBar.SelectedItem Is customersTileBarItem Then

				If tgsSputnikTables.EditValue Then
					AddInvalidCustomersIntoSourceDatabase()
				Else
					AddInvalidTwixCustomersIntoSourceDatabase()
				End If

			End If

		End Sub

		Private Sub OntileBar_ItemClick(sender As Object, e As TileItemEventArgs) Handles tileBar.ItemClick
			LoadData()
		End Sub

		Function LoadSourceEmployeeData() As Boolean
			Dim result As Boolean = True

			Dim data = m_SourceListingDatabaseAccess.LoadAllEmployeeMasterData(False)

			If data Is Nothing Then
				m_UtilityUI.ShowErrorDialog("Keine Kandidaten Daten im Source wurden gefunden.")

				Return False
			End If

			Dim existingEmployeesGridData = (From employeeData In data
											 Select New EmployeeMasterViewData With
												 {.EmployeeNumber = employeeData.EmployeeNumber,
												 .Lastname = employeeData.Lastname,
												 .Firstname = employeeData.Firstname,
												 .Street = employeeData.Street,
												 .Country = employeeData.Country,
												 .Postcode = employeeData.Postcode,
												 .Location = employeeData.Location,
												 .Birthdate = employeeData.Birthdate,
												 .KST = employeeData.KST,
												 .CreatedOn = employeeData.CreatedOn,
												 .ChangedOn = employeeData.ChangedOn,
												 .Selected = tgsSourceSelection.EditValue}).ToList()


			m_SourceEmployeeData = New BindingList(Of EmployeeMasterViewData)

			For Each employeerGridData In existingEmployeesGridData
				If employeerGridData.CreatedOn Is Nothing OrElse deDate.EditValue Is Nothing OrElse employeerGridData.CreatedOn > deDate.EditValue Then
					m_SourceEmployeeData.Add(employeerGridData)
				End If

			Next
			grdSourceEmployee.DataSource = m_SourceEmployeeData


			Return Not m_SourceEmployeeData Is Nothing
		End Function

		Function LoadSourceCustomerData() As Boolean
			Dim result As Boolean = True

			Dim data = m_SourceListingDatabaseAccess.LoadAllCustomerMasterData()

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
												 .KST = customerData.KST,
												 .CreatedOn = customerData.CreatedOn,
												 .ChangedOn = customerData.ChangedOn,
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



		Private Sub tpEmployee_SelectedPageChanged(sender As Object, e As SelectedPageChangedEventArgs) Handles tpEmployee.SelectedPageChanged

			btnImportData.Visible = False
			pnlUpdateAddButton.Visible = False
			tgsSourceSelection.Visible = False

			If tpEmployee.SelectedPage Is tnpSourceEmployee Then
				btnImportData.Visible = True
				tgsSourceSelection.Visible = True

			ElseIf tpEmployee.SelectedPage Is tnpInvalidEmployee Then
				pnlUpdateAddButton.Visible = True
				tgsSourceSelection.Visible = True


			Else

			End If

		End Sub

		Private Sub tpCustomer_SelectedPageChanged(sender As Object, e As SelectedPageChangedEventArgs) Handles tpCustomer.SelectedPageChanged

			btnImportData.Visible = False
			pnlUpdateAddButton.Visible = False
			tgsSourceSelection.Visible = False

			If tpCustomer.SelectedPage Is tnpSourceCustomer Then
				btnImportData.Visible = True
				tgsSourceSelection.Visible = True

			ElseIf tpCustomer.SelectedPage Is tnpInvalidCustomer Then
				pnlUpdateAddButton.Visible = True
				tgsSourceSelection.Visible = True

			Else

			End If

		End Sub


#Region "Helpers"


		Private Function OpenSourceDatabaseConnection() As Boolean
			Dim result As Boolean = True

			m_SourceInitializationData = CreateInitialData(lueSourceMD.EditValue, m_InitializationData.UserData.UserNr)

			m_SourceConnectionString = m_SourceInitializationData.MDData.MDDbConn
			m_SourceCommonDatabaseAccess = New CommonDatabaseAccess(m_SourceConnectionString, m_InitializationData.UserData.UserLanguage)
			m_SourceListingDatabaseAccess = New ListingDatabaseAccess(m_SourceConnectionString, m_InitializationData.UserData.UserLanguage)
			m_SourceEmployeeDatabaseAccess = New EmployeeDatabaseAccess(m_SourceConnectionString, m_InitializationData.UserData.UserLanguage)
			m_SourceCustomerDatabaseAccess = New CustomerDatabaseAccess(m_SourceConnectionString, m_InitializationData.UserData.UserLanguage)

			Return result
		End Function

		Protected Function GetDbConnectionString(ByVal mandantNumber As Integer, ByVal sputnikRootPath As String) As String
			Dim result As String = String.Empty
			Dim GetFileServerXMLFullFilename = Path.Combine(sputnikRootPath, "Bin\Programm.xml")

			Try
				m_Logger.LogInfo(String.Format("programm.xml is located on: {0} | mandantNumber: {1} | sputnikRootPath: {2}", GetFileServerXMLFullFilename, mandantNumber, sputnikRootPath))
				If Not File.Exists(GetFileServerXMLFullFilename) Then
					Throw New Exception(String.Format("setting file was not founded: {0}", GetFileServerXMLFullFilename))
				End If
				Dim xDoc As XDocument = XDocument.Load(GetFileServerXMLFullFilename)
				Dim ConfigQuery = (From exportSetting In xDoc.Root.Elements("databaseconnections").Elements(String.Format("md_{0}", mandantNumber))
								   Select New With {.mdconnstr = GetSafeStringFromXElement(exportSetting.Element("connstr")),
													 .mddbname = GetSafeStringFromXElement(exportSetting.Element("dbname")),
													 .mddbserver = GetSafeStringFromXElement(exportSetting.Element("dbname"))
													 }).FirstOrDefault()
				If ConfigQuery Is Nothing Then Return result

				result = FromBase64(ConfigQuery.mdconnstr)


			Catch ex As Exception
				m_Logger.LogError(String.Format("mandantNumber: {0} | sputnikRootPath: {1} | {2}", mandantNumber, sputnikRootPath, ex.ToString()))
				Return result

			End Try


			Return result

		End Function

		Private Function GetSafeStringFromXElement(ByVal xelment As XElement)

			If xelment Is Nothing Then
				Return String.Empty
			Else

				Return xelment.Value
			End If

		End Function

		Private Function FromBase64(ByVal sText As String) As String
			' Base64-String zunächst in ByteArray konvertieren
			Dim nBytes() As Byte = System.Convert.FromBase64String(sText)

			' ByteArray in String umwandeln
			Return System.Text.Encoding.Default.GetString(nBytes)
		End Function


		Private Function ReadEmployeeOffsetFromSettings() As Integer

			Dim settingValue As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/Mitarbeiter", m_StartNumberSetting))

			Dim intVal As Integer
			If Integer.TryParse(settingValue, intVal) Then
				Return intVal
			Else
				Return 0
			End If

		End Function

		Private Function ReadCustomerOffsetFromSettings() As Integer

			Dim settingValue As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/Kunden", m_StartNumberSetting))

			Dim intVal As Integer
			If Integer.TryParse(settingValue, intVal) Then
				Return intVal
			Else
				Return 0
			End If

		End Function

		Private Sub OntgsSourceSelection_Toggled(sender As Object, e As EventArgs) Handles tgsSourceSelection.Toggled
			If tileBar.SelectedItem Is employeesTileBarItem Then

				If tpEmployee.SelectedPage Is tnpSourceEmployee Then
					SelDeSelectEmployeeItems(tgsSourceSelection.EditValue)
				ElseIf tpEmployee.SelectedPage Is tnpInvalidEmployee Then
					SelDeSelectInvalidEmployeeItems(tgsSourceSelection.EditValue)

				Else
					Return
				End If

			Else
				If tpCustomer.SelectedPage Is tnpSourceCustomer Then
					SelDeSelectCustomerItems(tgsSourceSelection.EditValue)
				ElseIf tpCustomer.SelectedPage Is tnpInvalidCustomer Then
					SelDeSelectInvalidCustomerItems(tgsSourceSelection.EditValue)

				Else
					Return
				End If

			End If

		End Sub

		Private Sub SelDeSelectEmployeeItems(ByVal selectItem As Boolean)
			Dim data As BindingList(Of EmployeeMasterViewData) = grdSourceEmployee.DataSource

			If Not data Is Nothing Then
				For Each item In data
					item.Selected = selectItem
				Next
			End If

			gvSourceEmployee.RefreshData()

		End Sub

		Private Sub SelDeSelectInvalidEmployeeItems(ByVal selectItem As Boolean)
			Dim data As BindingList(Of EmployeeMasterViewData) = grdInvalidEmployee.DataSource

			If Not data Is Nothing Then
				For Each item In data
					item.Selected = selectItem
				Next
			End If

			gvInvalidEmployee.RefreshData()

		End Sub

		Private Sub SelDeSelectCustomerItems(ByVal selectItem As Boolean)
			Dim data As BindingList(Of CustomerMasterViewData) = grdSourceCustomer.DataSource

			If Not data Is Nothing Then
				For Each item In data
					item.Selected = selectItem
				Next
			End If

			gvSourceCustomer.RefreshData()

		End Sub

		Private Sub SelDeSelectInvalidCustomerItems(ByVal selectItem As Boolean)
			Dim data As BindingList(Of CustomerMasterViewData) = grdInvalidCustomer.DataSource

			If Not data Is Nothing Then
				For Each item In data
					item.Selected = selectItem
				Next
			End If

			gvInvalidCustomer.RefreshData()

		End Sub

		Private Sub OnGVSourceEmployee_RowCountChanged(sender As Object, e As System.EventArgs)

			tnpSourceEmployee.Caption = String.Format("Kandidaten (Quelle): {0}", gvSourceEmployee.RowCount)
			tnpInvalidEmployee.Caption = String.Format("Duplikate: {0}", gvInvalidEmployee.RowCount)
			tnpImportedEmployee.Caption = String.Format("Importierte Kandidaten: {0}", gvImportedEmployee.RowCount)

		End Sub

		Private Sub OnGVSourceCustomer_RowCountChanged(sender As Object, e As System.EventArgs)

			tnpSourceCustomer.Caption = String.Format("Kunden (Quelle): {0}", gvSourceCustomer.RowCount)
			tnpInvalidCustomer.Caption = String.Format("Duplikate: {0}", gvInvalidCustomer.RowCount)
			tnpImportedCustomer.Caption = String.Format("Importierte Kunden: {0}", gvImportedCustomer.RowCount)

		End Sub

		Private Function HasAccess(ByVal ltFullPath As String)
			Try
				Using inputstreamreader As New StreamReader(ltFullPath)
					inputstreamreader.Close()
				End Using
				Using inputStream As FileStream = File.Open(ltFullPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None)
					inputStream.Close()
					Return True
				End Using
			Catch ex As Exception
				Return False
			End Try
		End Function

		Private Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

			Dim m_md As New SPProgUtility.Mandanten.Mandant
			Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
			Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
			Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)
			Dim clsTransalation As New ClsTranslation
			Dim translate = clsTransalation.GetTranslationInObject

			Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

		End Function




#End Region




	End Class


	Public Class EmployeeMasterViewData
		Inherits EmployeeMasterData
		Public Property Selected As Boolean
		Public Property SourceCreatedOn As DateTime?
		Public Property SourceChangedOn As DateTime?
		Public Property DestCreatedOn As DateTime?
		Public Property DestChangedOn As DateTime?
		Public Property DestCompleteAddress As String

	End Class

	Public Class CustomerMasterViewData
		Inherits CustomerMasterData
		Public Property Selected As Boolean
		Public Property SourceCreatedOn As DateTime?
		Public Property SourceChangedOn As DateTime?
		Public Property DestCreatedOn As DateTime?
		Public Property DestChangedOn As DateTime?
		Public Property DestCompleteAddress As String

	End Class

	Module DemoHelper
		Function GetDeleteImage() As Image
			Return GetImage(Brushes.Red)
		End Function

		Function GetEditImage() As Image
			Return GetImage(Brushes.Green)
		End Function

		Function GetImage(ByVal b As Brush) As Image
			Dim img As Image = New Bitmap(16, 16)

			Using g As Graphics = Graphics.FromImage(img)
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias
				g.FillEllipse(b, New Rectangle(0, 0, img.Width - 1, img.Height - 1))
			End Using

			Return img
		End Function
	End Module


End Namespace
