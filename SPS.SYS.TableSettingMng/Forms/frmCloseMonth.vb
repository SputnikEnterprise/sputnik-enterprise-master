
Imports SPS.SYS.TableSettingMng.UI

Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Common.DataObjects

Imports System.ComponentModel
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraNavBar
Imports DevExpress.XtraEditors
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors.Repository

Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.TableSetting
Imports SP.DatabaseAccess.TableSetting.DataObjects
Imports SP.DatabaseAccess.TableSetting.DataObjects.EmployeeContactData

Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Infrastructure

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages


Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.Mandanten
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraSplashScreen
Imports SP


Public Class frmCloseMonth


#Region "Private Fields"

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The common database access.
	''' </summary>
	Protected m_CommonDatabaseAccess As ICommonDatabaseAccess


	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' The settings manager.
	''' </summary>
	Private m_SettingsManager As ISettingsManager

	Private m_mandant As Mandant

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As Utility

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
	''' </summary>
	Private m_SuppressUIEvents As Boolean = False

	''' <summary>
	''' The mandant.
	''' </summary>
	Private m_MandantData As Mandant

	Private m_Years As List(Of IntegerValueViewWrapper)
	Private m_Month As List(Of IntegerValueViewWrapper)


#End Region


#Region "private consts"

	Private Const MODUL_NAME_SETTING = "deletedreclist"

	Private Const USER_XML_SETTING_SPUTNIK_DELETED_SEARCH_LIST_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/deletedreclist/{1}/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_DELETED_SEARCH_LIST_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/deletedreclist/{1}/keepfilter"

#End Region


#Region "Constructor"

	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_InitializationData = _setting
		m_MandantData = New Mandant
		m_UtilityUI = New UtilityUI
		m_Utility = New Utility
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		m_CommonDatabaseAccess = New DatabaseAccess.Common.CommonDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_SuppressUIEvents = True
		InitializeComponent()
		m_SuppressUIEvents = False


		Me.KeyPreview = True
		Dim strStyleName As String = m_MandantData.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		' Translate controls.
		TranslateControls()


		'lueMandant.EditValue = m_InitializationData.MDData.MDNr

		'LoadData()


		Reset()


	End Sub


#End Region

	''' <summary>
	''' Gets the selected employee.
	''' </summary>
	''' <returns>The selected employee or nothing if none is selected.</returns>
	Public ReadOnly Property SelectedRecord As MonthCloseData
		Get
			Dim gvRP = TryCast(gridBank.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvRP Is Nothing) Then

				Dim selectedRows = gvRP.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim employee = CType(gvRP.GetRow(selectedRows(0)), MonthCloseData)
					Return employee
				End If

			End If

			Return Nothing
		End Get

	End Property


#Region "Lookup Edit Reset und Load..."

	''' <summary>
	''' Resets the Mandanten drop down.
	''' </summary>
	Private Sub ResetMandantenDropDown()

		lueMandant.Properties.DisplayMember = "MandantName1"
		lueMandant.Properties.ValueMember = "MandantNumber"

		lueMandant.Properties.Columns.Clear()
		lueMandant.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "MandantName1",
																					 .Width = 100,
																					 .Caption = m_Translate.GetSafeTranslationValue("Mandant")})

		lueMandant.Properties.ShowHeader = False
		lueMandant.Properties.ShowFooter = False

		lueMandant.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueMandant.Properties.SearchMode = SearchMode.AutoComplete
		lueMandant.Properties.AutoSearchColumnIndex = 0

		lueMandant.Properties.NullText = String.Empty
		lueMandant.EditValue = Nothing

	End Sub

	''' <summary>
	''' Load Mandanten drop down
	''' </summary>
	''' <remarks></remarks>
	Private Function LoadMandantDropDownData() As Boolean

		Dim mandantData = m_CommonDatabaseAccess.LoadCompaniesListData

		If (mandantData Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mandantendaten konnten nicht geladen werden."))
		End If

		lueMandant.EditValue = Nothing
		lueMandant.Properties.DataSource = mandantData
		lueMandant.Properties.ForceInitialize()

		Return mandantData IsNot Nothing

	End Function


	''' <summary>
	''' Handles edit change event of lueMandant.
	''' </summary>
	Private Sub OnLueMandant_EditValueChanged(sender As Object, e As EventArgs) Handles lueMandant.EditValueChanged

		If m_SuppressUIEvents Then
			Return
		End If

		If Not lueMandant.EditValue Is Nothing Then

			Dim conStr = m_MandantData.GetSelectedMDData(lueMandant.EditValue).MDDbConn
			m_CommonDatabaseAccess = New DatabaseAccess.Common.CommonDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)

			m_InitializationData = ClsDataDetail.ChangeMandantData(lueMandant.EditValue, m_InitializationData.UserData.UserNr)

		End If

		LoadMandantYearDropDownData()

		PreselectYearAndMonth()


		LoadMonthCloseData()

	End Sub


	''' <summary>
	''' Handles edit change event of lueYear.
	''' </summary>
	Private Sub OnLueYear_EditValueChanged(sender As Object, e As EventArgs) Handles lueYear.EditValueChanged

		If m_SuppressUIEvents Then
			Return
		End If

		LoadMonthDropDownData()

	End Sub


#End Region



#Region "private methodes"

	''' <summary>
	'''  Trannslate controls.
	''' </summary>
	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

		Me.btnClose.Text = m_Translate.GetSafeTranslationValue(Me.btnClose.Text)
		Me.lblHeaderFett.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderFett.Text)
		Me.lblHeaderNormal.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderNormal.Text)

		Me.btnCloseMonth.Text = m_Translate.GetSafeTranslationValue(Me.btnCloseMonth.Text)
		Me.grpExistsrecs.Text = m_Translate.GetSafeTranslationValue(Me.grpExistsrecs.Text)

		Me.bbiOpenMonth.Caption = m_Translate.GetSafeTranslationValue(Me.bbiOpenMonth.Caption)
		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)

	End Sub

	''' <summary>
	''' Resets the form.
	''' </summary>
	Private Sub Reset()

		m_SuppressUIEvents = True

		ResetMandantenDropDown()
		ResetYearDropDown()
		ResetMonthDropDown()

		ClearGrids()


		m_SuppressUIEvents = False

		errorProviderMonthCloseMng.ClearErrors()

	End Sub

	''' <summary>
	''' Clears the grids.
	''' </summary>
	Private Sub ClearGrids()
		ResetMonthClsoeGrid()
	End Sub

	''' <summary>
	''' Resets the year drop down.
	''' </summary>
	Private Sub ResetYearDropDown()

		lueYear.Properties.DisplayMember = "Value"
		lueYear.Properties.ValueMember = "Value"
		lueYear.Properties.ShowHeader = False

		lueYear.Properties.Columns.Clear()
		lueYear.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "Value",
																					 .Width = 100,
																					 .Caption = m_Translate.GetSafeTranslationValue("Value")})

		lueYear.Properties.ShowFooter = False
		lueYear.Properties.DropDownRows = 10
		lueYear.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueYear.Properties.SearchMode = SearchMode.AutoComplete
		lueYear.Properties.AutoSearchColumnIndex = 0

		lueYear.Properties.NullText = String.Empty
		lueYear.EditValue = Nothing
	End Sub

	''' <summary>
	''' Resets the month drop down.
	''' </summary>
	Private Sub ResetMonthDropDown()

		lueMonth.Properties.DisplayMember = "Value"
		lueMonth.Properties.ValueMember = "Value"
		lueMonth.Properties.ShowHeader = False

		lueMonth.Properties.Columns.Clear()
		lueMonth.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "Value",
																					 .Width = 100,
																					 .Caption = m_Translate.GetSafeTranslationValue("Value")})

		lueMonth.Properties.ShowFooter = False
		lueMonth.Properties.DropDownRows = 10
		lueMonth.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueMonth.Properties.SearchMode = SearchMode.AutoComplete
		lueMonth.Properties.AutoSearchColumnIndex = 0

		lueMonth.Properties.NullText = String.Empty
		lueMonth.EditValue = Nothing
	End Sub

	''' <summary>
	''' Resets the grid.
	''' </summary>
	Private Sub ResetMonthClsoeGrid()

		' Reset the grid
		gvBank.FocusRectStyle = DrawFocusRectStyle.RowFocus

		gvBank.OptionsView.ShowIndicator = False
		gvBank.OptionsView.ShowAutoFilterRow = True
		gvBank.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never

		gvBank.Columns.Clear()

		Dim columnClearningNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnClearningNr.Caption = m_Translate.GetSafeTranslationValue("ID")
		columnClearningNr.Name = "ID"
		columnClearningNr.FieldName = "ID"
		columnClearningNr.Visible = False
		gvBank.Columns.Add(columnClearningNr)

		Dim columnBankname As New DevExpress.XtraGrid.Columns.GridColumn()
		columnBankname.Caption = m_Translate.GetSafeTranslationValue("Monat")
		columnBankname.Name = "monat"
		columnBankname.FieldName = "monat"
		columnBankname.Width = 20
		columnBankname.Visible = True
		gvBank.Columns.Add(columnBankname)

		Dim columnPostCodeAndLocation As New DevExpress.XtraGrid.Columns.GridColumn()
		columnPostCodeAndLocation.Caption = m_Translate.GetSafeTranslationValue("Jahr")
		columnPostCodeAndLocation.Name = "jahr"
		columnPostCodeAndLocation.FieldName = "jahr"
		columnPostCodeAndLocation.Width = 20
		columnPostCodeAndLocation.Visible = True
		gvBank.Columns.Add(columnPostCodeAndLocation)

		Dim columnSwift As New DevExpress.XtraGrid.Columns.GridColumn()
		columnSwift.Caption = m_Translate.GetSafeTranslationValue("BeraterIn")
		columnSwift.Name = "UserName"
		columnSwift.FieldName = "UserName"
		'columnSwift.BestFit()
		columnSwift.Visible = True
		gvBank.Columns.Add(columnSwift)

		Dim columnTelephone As New DevExpress.XtraGrid.Columns.GridColumn()
		columnTelephone.Caption = m_Translate.GetSafeTranslationValue("Erstellt am")
		columnTelephone.Name = "CreatedOn"
		columnTelephone.FieldName = "CreatedOn"
		'columnTelephone.BestFit()
		columnTelephone.Visible = True
		gvBank.Columns.Add(columnTelephone)

		Dim columnTelefax As New DevExpress.XtraGrid.Columns.GridColumn()
		columnTelefax.Caption = m_Translate.GetSafeTranslationValue("Mandant")
		columnTelefax.Name = "MDName"
		columnTelefax.FieldName = "MDName"
		'columnTelefax.BestFit()
		columnTelefax.Visible = True
		gvBank.Columns.Add(columnTelefax)


		gridBank.DataSource = Nothing

	End Sub



	''' <summary>
	''' Loads the mandant drop down data.
	''' </summary>
	Private Function LoadMandantYearDropDownData() As Boolean

		Dim success As Boolean = True

		Dim mandantNumber = lueMandant.EditValue

		Dim wrappedValues As List(Of IntegerValueViewWrapper) = Nothing

		If Not mandantNumber Is Nothing Then

			Dim yearData = m_CommonDatabaseAccess.LoadMandantYears(mandantNumber)

			If (yearData Is Nothing) Then
				success = False
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Jahre (Mandanten) konnten nicht geladen werden."))
			End If

			If Not yearData Is Nothing Then
				wrappedValues = New List(Of IntegerValueViewWrapper)

				For Each yearValue In yearData
					wrappedValues.Add(New IntegerValueViewWrapper With {.Value = yearValue})
				Next

			End If

		End If

		m_Years = wrappedValues

		lueYear.EditValue = Nothing
		lueYear.Properties.DataSource = m_Years
		lueYear.Properties.ForceInitialize()

		Return success
	End Function

	''' <summary>
	''' Loads the month drop down data.
	''' </summary>
	Private Function LoadMonthDropDownData() As Boolean

		Dim success As Boolean = True

		Dim mandantNumber = lueMandant.EditValue
		Dim year = lueYear.EditValue
		'If year Is Nothing Then year = Now.Year

		Dim wrappedValues As List(Of IntegerValueViewWrapper) = Nothing

		If Not mandantNumber Is Nothing And
			 Not year Is Nothing Then

			Dim closedMonth = m_CommonDatabaseAccess.LoadClosedMonthOfYear(year, mandantNumber)

			If (closedMonth Is Nothing) Then
				success = False
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verrechnete Monate konnten nicht geladen werden."))
			End If

			If Not closedMonth Is Nothing Then
				wrappedValues = New List(Of IntegerValueViewWrapper)

				For i As Integer = 1 To 12

					If Not closedMonth.Contains(i) Then
						wrappedValues.Add(New IntegerValueViewWrapper With {.Value = i})
					End If

				Next

			End If

		End If

		m_Month = wrappedValues
		lueMonth.EditValue = Nothing
		lueMonth.Properties.DataSource = m_Month
		lueMonth.Properties.ForceInitialize()

		Return success
	End Function


	Public Sub LoadData()

		Dim success As Boolean = True

		'success = success And LoadMandantDropDownData()

		If m_SuppressUIEvents Then

			LoadMonthDropDownData()

		Else
			success = success And LoadMandantDropDownData()

		End If


		LoadMonthCloseData()
		PreselectsMandantYearAndMonth()

		bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Bereit")

	End Sub

	''' <summary>
	''' Preselects mandant, year and month.
	''' </summary>
	Private Sub PreselectsMandantYearAndMonth()

		lueMandant.EditValue = m_InitializationData.MDData.MDNr

		PreselectYearAndMonth()

	End Sub

	''' <summary>
	''' Preslects year and month.
	''' </summary>
	Private Sub PreselectYearAndMonth()

		Dim isDateBefore10thDay = (DateTime.Now.Day <= 10)
		Dim today = DateTime.Now.Date
		Dim yearMonthToSelect As DateTime = New DateTime(today.Year, today.Month, 1)

		If isDateBefore10thDay Then
			' Take previous month if date is before 10th.
			yearMonthToSelect = yearMonthToSelect.AddMonths(-1)
		End If

		SelectYear(yearMonthToSelect.Year)
		SelectMonth(yearMonthToSelect.Month)

	End Sub

	''' <summary>
	''' Selects a year.
	''' </summary>
	''' <param name="year">The year to select.</param>
	Private Sub SelectYear(ByVal year As Integer)

		If m_Years Is Nothing Then
			m_Years = New List(Of IntegerValueViewWrapper)
			lueYear.Properties.DataSource = m_Years
			lueYear.Properties.ForceInitialize()
		End If

		If Not m_Years.Any(Function(data) data.Value = year) Then
			m_Years.Add(New IntegerValueViewWrapper With {.Value = year})
		End If

		lueYear.EditValue = year

	End Sub

	''' <summary>
	''' Selects a month.
	''' </summary>
	''' <param name="month">The month to select.</param>
	Private Sub SelectMonth(ByVal month As Integer)

		If m_Month Is Nothing Then
			m_Month = New List(Of IntegerValueViewWrapper)
			lueMonth.Properties.DataSource = m_Month
			lueMonth.Properties.ForceInitialize()
		End If
		lueMonth.EditValue = If(m_Month.Count > 0, m_Month(0).Value, month)



		'If Not m_Month.Any(Function(data) data.Value = month) Then
		'	m_Month.Add(New IntegerValueViewWrapper With {.Value = month})
		'End If

		'lueMonth.EditValue = month

	End Sub



	''' <summary>
	''' loads monthclose data.
	''' </summary>
	Private Sub LoadMonthCloseData()
		If lueMandant.EditValue Is Nothing Then Return

		Dim searchResult = m_CommonDatabaseAccess.LoadAllMonthCloseData(lueMandant.EditValue, If(chkThisYear.Checked, Now.Year, Nothing), Nothing)

		If (searchResult Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Daten konnten nicht aufgelistet werden."))
			Return
		End If

		Dim listDataSource As BindingList(Of MonthCloseData) = New BindingList(Of MonthCloseData)

		For Each result In searchResult

			Dim viewData = New MonthCloseData With {
				.ID = result.ID,
				.jahr = result.jahr,
				.monat = result.monat,
				.UserName = result.UserName,
				.CreatedOn = result.CreatedOn,
				.MandantNumber = result.MandantNumber,
				.MDName = result.MDName
			}

			listDataSource.Add(viewData)

		Next

		gridBank.DataSource = listDataSource

	End Sub

	''' <summary>
	''' Handles click on close button.
	''' </summary>
	Private Sub OnBtnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
		DialogResult = DialogResult.None
		Close()
	End Sub

	''' <summary>
	''' Handles the form disposed event.
	''' </summary>
	Private Sub OnSearchBankData_FormClosing(sender As Object, e As System.EventArgs) Handles Me.FormClosing

		' Save form location, width and height in setttings
		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.frmLocationMonthClose = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.ifrmWidthMonthClose = Me.Width
				My.Settings.ifrmHeightMonthClose = Me.Height

				My.Settings.Save()

			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())

		End Try

	End Sub

	''' <summary>
	''' Handles form load event.
	''' </summary>
	Private Sub OnForm_Load(sender As Object, e As System.EventArgs) Handles Me.Load

		Dim strStyleName As String = m_MandantData.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_MandantData.GetDefaultUSNr, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		Try
			Dim setting_form_search_height = My.Settings.ifrmHeightMonthClose
			Dim setting_form_search_width = My.Settings.ifrmWidthMonthClose
			Dim setting_form_search_location = My.Settings.frmLocationMonthClose

			If setting_form_search_height > 0 Then Me.Height = Math.Max(Me.Height, setting_form_search_height)
			If setting_form_search_width > 0 Then Me.Width = Math.Max(Me.Width, setting_form_search_width)

			If Not String.IsNullOrEmpty(setting_form_search_location) Then
				Dim aLoc As String() = setting_form_search_location.Split(CChar(";"))
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
	''' Handles click on save bank data buton.
	''' </summary>
	Private Sub OnbtnCloseMonth_Click(sender As System.Object, e As System.EventArgs) Handles btnCloseMonth.Click

		If ValidateBankInputData() Then

			Dim InvalidData = m_CommonDatabaseAccess.LoadNotInvalidDataForClosingMonth(lueMandant.EditValue, lueYear.EditValue, lueMonth.EditValue)
			If Not InvalidData Is Nothing Then
				If InvalidData.Count > 0 Then
					Dim frmTest As New frmInvalidRecordNumbers(lueMandant.EditValue, m_InitializationData)
					frmTest.LoadData(lueMandant.EditValue, lueYear.EditValue, lueMonth.EditValue)

					frmTest.Show()
					frmTest.BringToFront()
					Return

				End If
			End If


			Dim monthClose As MonthCloseData = Nothing

			Dim dt = DateTime.Now
			monthClose = New MonthCloseData With {.MandantNumber = lueMandant.EditValue,
																							.CreatedOn = dt,
																							.UserName = m_InitializationData.UserData.UserFullName}

			monthClose.jahr = lueYear.EditValue
			monthClose.monat = lueMonth.EditValue

			Dim success As Boolean = True

			' Insert data
			success = m_CommonDatabaseAccess.SaveMonthCloseData(monthClose)

			If Not success Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))
			Else
				m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Daten wurden gespeichert."))

				m_SuppressUIEvents = True
				LoadData()
				m_SuppressUIEvents = False

			End If

		End If

	End Sub

	''' <summary>
	''' Handles checked change event for current year checkbox..
	''' </summary>
	Private Sub OnchkThisYear_CheckedChanged(sender As Object, e As EventArgs) Handles chkThisYear.CheckedChanged
		LoadMonthCloseData()
	End Sub


	Private Sub OnbbiOpenMonth_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiOpenMonth.ItemClick

		Dim data = SelectedRecord
		If Not data Is Nothing Then
			If data.ID > 0 Then
				' delete data
				Dim success = m_CommonDatabaseAccess.DeleteMonthCloseData(data)
				Dim msg As String = String.Empty

				If Not success Then
					msg = m_Translate.GetSafeTranslationValue("Monat konnte nicht geöffnet werden.")
					m_UtilityUI.ShowErrorDialog(msg)

				Else
					msg = m_Translate.GetSafeTranslationValue("Monat wurde geöffnet.")
					m_UtilityUI.ShowInfoDialog(msg)

					m_SuppressUIEvents = True
					LoadData()
					m_SuppressUIEvents = False

				End If

				bsiInfo.Caption = msg

			End If
		End If

	End Sub


#End Region


#Region "Error Handle"

	''' <summary>
	''' Validates bank data input data.
	''' </summary>
	Private Function ValidateBankInputData() As Boolean

		Dim missingFieldText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")

		Dim isValid As Boolean = True

		isValid = isValid And SetErrorIfInvalid(lueMonth, errorProviderMonthCloseMng, (lueMonth.EditValue Is Nothing), missingFieldText)
		isValid = isValid And SetErrorIfInvalid(lueYear, errorProviderMonthCloseMng, (lueYear.EditValue Is Nothing), missingFieldText)
		isValid = isValid And SetErrorIfInvalid(lueMandant, errorProviderMonthCloseMng, (lueMandant.EditValue Is Nothing), missingFieldText)


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
	Private Function SetErrorIfInvalid(ByVal control As Control, ByVal errorProvider As DXErrorProvider.DXErrorProvider, ByVal invalid As Boolean, ByVal errorText As String) As Boolean

		errorProvider.SetIconAlignment(control, ErrorIconAlignment.MiddleLeft)
		If (invalid) Then
			errorProvider.SetError(control, errorText, DXErrorProvider.ErrorType.Critical)
		Else
			errorProvider.SetError(control, String.Empty)
		End If

		Return Not invalid

	End Function

	''' <summary>
	''' Clears the errors.
	''' </summary>
	Private Sub ClearErrors()
		errorProviderMonthCloseMng.ClearErrors()
	End Sub

#End Region


#Region "Helper Classes"

	''' <summary>
	''' Wraps an integer value.
	''' </summary>
	Class IntegerValueViewWrapper
		Public Property Value As Integer
	End Class


#End Region

End Class