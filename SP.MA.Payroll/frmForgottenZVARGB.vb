
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.PayrollMng
Imports SPProgUtility.Mandanten
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports SP.DatabaseAccess.PayrollMng.DataObjects
Imports SPS.MA.Guthaben
Imports System.Reflection
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports DevExpress.XtraPrinting
Imports DevExpress.LookAndFeel
Imports System.Drawing.Printing
Imports System.IO
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors.Controls
Imports SPProgUtility.SPUserSec.ClsUserSec

Namespace UI

	Public Class frmForgottenZVARGB

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
		''' The common database access.
		''' </summary>
		Private m_CommonDatabaseAccess As ICommonDatabaseAccess

		''' <summary>
		''' The Payroll data access object.
		''' </summary>
		Private m_PayrollDatabaseAccess As IPayrollDatabaseAccess

		''' <summary>
		''' The mandant.
		''' </summary>
		Private m_md As Mandant

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
		''' Boolean flag indicating if initial data has been loaded.
		''' </summary>
		Private m_IsInitialDataLoaded As Boolean = False

		''' <summary>
		''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
		''' </summary>
		Private m_SuppressUIEvents As Boolean = False

		''' <summary>
		''' The Mandant number.
		''' </summary>
		Private m_MDNr As Integer
		Private m_Years As List(Of IntegerValueViewWrapper)
		Private m_Month As List(Of IntegerValueViewWrapper)

#End Region


#Region "public properties"

		Public Property AssignedYear As Integer?
		Public Property AssignedMonth As Integer?

#End Region

#Region "Constructor"


		''' <summary>
		''' The constructor.
		''' </summary>
		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			DevExpress.UserSkins.BonusSkins.Register()
			DevExpress.Skins.SkinManager.EnableFormSkins()

			m_InitializationData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

			m_UtilityUI = New UtilityUI
			m_Utility = New Utility
			m_md = New Mandant

			' Dieser Aufruf ist für den Designer erforderlich.
			m_SuppressUIEvents = True
			InitializeComponent()
			m_SuppressUIEvents = False

			Me.KeyPreview = True
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

			Dim conStr = m_md.GetSelectedMDData(m_MDNr).MDDbConn
			m_CommonDatabaseAccess = New DatabaseAccess.Common.CommonDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
			m_PayrollDatabaseAccess = New DatabaseAccess.PayrollMng.PayrollDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)

			' Translate controls.
			TranslateControls()

			Reset()

		End Sub

#End Region

#Region "Private Properties"

		''' <summary>
		''' Gets the selected employee data.
		''' </summary>
		''' <returns>The selected employee data or nothing if none is selected.</returns>
		Private ReadOnly Property SelectedZVARGBEmployeeData As EmployeeDataForZV
			Get
				Dim grdView = TryCast(grdZVARGBEmployees.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (grdView Is Nothing) Then

					Dim selectedRows = grdView.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim employeeDataForZV = CType(grdView.GetRow(selectedRows(0)), EmployeeDataForZV)
						Return employeeDataForZV
					End If

				End If

				Return Nothing
			End Get

		End Property

#End Region


#Region "Public Methods"

		''' <summary>
		''' Loads the data.
		''' </summary>
		Public Function LoadData() As Boolean

			Dim success As Boolean = True

			m_MDNr = m_InitializationData.MDData.MDNr

			success = success AndAlso LoadMandantDropDownData()

			m_SuppressUIEvents = True
			PreselectsMandantYearAndMonth()

			'LoadMandantYearDropDownData()
			'LoadMonthDropDownData()

			PreselectYearAndMonth()


			If AssignedYear.GetValueOrDefault(0) = 0 Then lueYear.EditValue = m_InitializationData.MDData.MDYear Else lueYear.EditValue = AssignedYear
			If AssignedMonth.GetValueOrDefault(0) = 0 Then lueMonth.EditValue = Now.Month Else lueMonth.EditValue = AssignedMonth

			m_SuppressUIEvents = False

			success = success AndAlso LoadEmployeeDataForZV()

			Return success
		End Function


#End Region


#Region "Private Methods"

		''' <summary>
		'''  Trannslate controls.
		''' </summary>
		Private Sub TranslateControls()
			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

			Me.lblMandant.Text = m_Translate.GetSafeTranslationValue(Me.lblMandant.Text)
			Me.lblJahr.Text = m_Translate.GetSafeTranslationValue(Me.lblJahr.Text)

			Me.lblMonat.Text = m_Translate.GetSafeTranslationValue(Me.lblMonat.Text)

		End Sub

		''' <summary>
		''' Resets the form.
		''' </summary>
		Private Sub Reset()

			pnlFilter.Dock = DockStyle.Fill

			btnOpenZVForm.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 108, m_InitializationData.MDData.MDNr)
			btnOpenAGForm.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 110, m_InitializationData.MDData.MDNr)

			' ---Reset drop downs, grids and lists---
			ResetMandantDropDown()
			ResetYearDropDown()
			ResetMonthDropDown()

			ResetZVARGBEmployeeGrid()

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
		''' Resets the Mandant drop down.
		''' </summary>
		Private Sub ResetMandantDropDown()

			lueMandant.Properties.DisplayMember = "MandantName1"
			lueMandant.Properties.ValueMember = "MandantNumber"

			lueMandant.Properties.Columns.Clear()
			lueMandant.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "MandantName1",
																							 .Width = 100,
																							 .Caption = m_Translate.GetSafeTranslationValue("Mandant")})

			lueMandant.Properties.ShowFooter = False
			lueMandant.Properties.DropDownRows = 10
			lueMandant.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueMandant.Properties.SearchMode = SearchMode.AutoComplete
			lueMandant.Properties.AutoSearchColumnIndex = 0

			lueMandant.Properties.NullText = String.Empty
			lueMandant.EditValue = Nothing
		End Sub


		''' <summary>
		''' Resets the employee data grid.
		''' </summary>
		Private Sub ResetZVARGBEmployeeGrid()

			gvZVARGBEmployees.OptionsView.ShowIndicator = False
			gvZVARGBEmployees.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvZVARGBEmployees.Columns.Clear()


			Dim checkEditReExisting As RepositoryItemCheckEdit
			checkEditReExisting = CType(grdZVARGBEmployees.RepositoryItems.Add("CheckEdit"), RepositoryItemCheckEdit)
			checkEditReExisting.PictureChecked = My.Resources.Checked
			checkEditReExisting.PictureUnchecked = Nothing
			checkEditReExisting.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined


			Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMANr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnMANr.Caption = m_Translate.GetSafeTranslationValue("Kandidaten-Nr.")
			columnMANr.Name = "MANr"
			columnMANr.FieldName = "MANr"
			columnMANr.Width = 50
			columnMANr.Visible = True
			gvZVARGBEmployees.Columns.Add(columnMANr)

			Dim columnLastName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLastName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnLastName.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
			columnLastName.Name = "EmployeeFullnameWithComma"
			columnLastName.FieldName = "EmployeeFullnameWithComma"
			columnLastName.Width = 200
			columnLastName.Visible = True
			gvZVARGBEmployees.Columns.Add(columnLastName)

			Dim columnZVPrinted As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZVPrinted.Caption = m_Translate.GetSafeTranslationValue("ZV.-Gedruckt?")
			columnZVPrinted.Name = "ZVPrinted"
			columnZVPrinted.FieldName = "ZVPrinted"
			columnZVPrinted.Width = 50
			columnZVPrinted.Visible = True
			columnZVPrinted.ColumnEdit = checkEditReExisting
			columnZVPrinted.UnboundType = DevExpress.Data.UnboundColumnType.Boolean
			gvZVARGBEmployees.Columns.Add(columnZVPrinted)

			Dim columnARGBPrinted As New DevExpress.XtraGrid.Columns.GridColumn()
			columnARGBPrinted.Caption = m_Translate.GetSafeTranslationValue("ARGB-Gedruckt?")
			columnARGBPrinted.Name = "ARGBPrinted"
			columnARGBPrinted.FieldName = "ARGBPrinted"
			columnARGBPrinted.Width = 50
			columnARGBPrinted.Visible = True
			columnARGBPrinted.ColumnEdit = checkEditReExisting
			columnARGBPrinted.UnboundType = DevExpress.Data.UnboundColumnType.Boolean
			gvZVARGBEmployees.Columns.Add(columnARGBPrinted)


			grdZVARGBEmployees.DataSource = Nothing

		End Sub


		Private Sub PreselectsMandantYearAndMonth()

			lueMandant.EditValue = m_InitializationData.MDData.MDNr
			LoadMandantYearDropDownData()
			LoadMonthDropDownData()

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

			If Not m_Month.Any(Function(data) data.Value = month) Then
				m_Month.Add(New IntegerValueViewWrapper With {.Value = month})
			End If

			lueMonth.EditValue = month

		End Sub

		Private Function LoadMandantDropDownData() As Boolean
			Dim mandantData = m_CommonDatabaseAccess.LoadCompaniesListData()

			If (mandantData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mandantendaten konnten nicht geladen werden."))
			End If

			lueMandant.EditValue = Nothing
			lueMandant.Properties.DataSource = mandantData
			lueMandant.Properties.ForceInitialize()

			Return mandantData IsNot Nothing
		End Function

		Private Function LoadMandantYearDropDownData() As Boolean

			Dim success As Boolean = True

			Dim mandantNumber As Integer? = lueMandant.EditValue

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
			lueYear.Properties.DropDownRows = m_Years.Count

			lueYear.Properties.ForceInitialize()

			Return success
		End Function

		Private Function LoadMonthDropDownData() As Boolean

			Dim success As Boolean = True

			Dim mandantNumber = lueMandant.EditValue
			Dim year = lueYear.EditValue

			Dim wrappedValues As List(Of IntegerValueViewWrapper) = Nothing
			If year Is Nothing Then year = m_InitializationData.MDData.MDYear

			If Not mandantNumber Is Nothing AndAlso Not year Is Nothing Then

				Dim monthData = m_CommonDatabaseAccess.LoadPayrollMonthOfYear(year, mandantNumber)

				If (monthData Is Nothing) Then
					success = False
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Monaten (Mandanten) konnten nicht geladen werden."))
				End If

				If Not monthData Is Nothing Then
					wrappedValues = New List(Of IntegerValueViewWrapper)

					For Each monthValue In monthData
						wrappedValues.Add(New IntegerValueViewWrapper With {.Value = monthValue})
					Next

				End If

			End If

			m_Month = wrappedValues
			lueMonth.EditValue = Nothing
			lueMonth.Properties.DataSource = m_Month
			lueMonth.Properties.DropDownRows = m_Month.Count

			lueMonth.Properties.ForceInitialize()

			Return success
		End Function

		''' <summary>
		''' Loads employee data for ZV.
		''' </summary>
		Private Function LoadEmployeeDataForZV() As Boolean

			Dim listOfEmployeeData As List(Of EmployeeDataForZV) = Nothing

			listOfEmployeeData = m_PayrollDatabaseAccess.LoadEmployeesForForgottenZV(m_MDNr, lueMonth.EditValue, lueYear.EditValue, m_InitializationData.UserData.UserFullName)

			grdZVARGBEmployees.DataSource = listOfEmployeeData

			If listOfEmployeeData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mitarbeiterdaten für ZV und Arbeitgeberbescheinigung konnten nicht geladnen werden."))
				Return False
			End If


			Return Not listOfEmployeeData Is Nothing

		End Function


#End Region

#Region "Event Handlers"

		Private Sub OnLueMandant_EditValueChanged(sender As Object, e As EventArgs) Handles lueMandant.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			ResetZVARGBEmployeeGrid()

			If Not lueMandant.EditValue Is Nothing Then

				If m_InitializationData.MDData.MDNr <> lueMandant.EditValue Then
					Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation

					Dim clsMandant = m_md.GetSelectedMDData(lueMandant.EditValue)
					Dim logedUserData = m_md.GetSelectedUserData(clsMandant.MDNr, m_InitializationData.UserData.UserNr)
					Dim personalizedData = m_InitializationData.ProsonalizedData
					Dim translate = m_InitializationData.TranslationData

					m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)
				End If


				Dim conStr = m_md.GetSelectedMDData(lueMandant.EditValue).MDDbConn
				m_PayrollDatabaseAccess = New DatabaseAccess.PayrollMng.PayrollDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
				m_CommonDatabaseAccess = New DatabaseAccess.Common.CommonDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
			End If

			LoadMandantYearDropDownData()
			LoadMonthDropDownData()

			PreselectYearAndMonth()

		End Sub

		Private Sub OnLueYear_EditValueChanged(sender As Object, e As EventArgs) Handles lueYear.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			ResetZVARGBEmployeeGrid()

			LoadMonthDropDownData()

		End Sub


		Private Sub OnlueMonth_EditValueChanged(sender As Object, e As EventArgs) Handles lueMonth.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			ResetZVARGBEmployeeGrid()

			LoadEmployeeDataForZV()

		End Sub

		'''' <summary>
		'''' Handles click on close button.
		'''' </summary>
		'Private Sub OnBtnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
		'	Close()
		'End Sub

		''' <summary>
		'''Handles clik on btnOpenZVForm.
		''' </summary>
		Private Sub OnBtnOpenZVForm_Click(sender As Object, e As EventArgs) Handles btnOpenZVForm.Click
			Dim employeeData = SelectedZVARGBEmployeeData

			If Not employeeData Is Nothing Then

				Try
					Dim frmZV = New SPS.MA.Guthaben.frmZV(m_InitializationData)

					Dim preselectionSetting As New PreselectionZVData With {.MDNr = m_InitializationData.MDData.MDNr, .EmployeeNumber = employeeData.MANr, .Year = lueYear.EditValue, .Month = lueMonth.EditValue}
					frmZV.PreselectionData = preselectionSetting

					frmZV.LoadData()
					frmZV.DisplayEmployeeData()

					frmZV.Show()
					frmZV.BringToFront()

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}", ex.ToString))
					m_UtilityUI.ShowErrorDialog(String.Format("{0}", ex.ToString))

				End Try

			End If

		End Sub

		Private Sub OngvZVARGBEmployees_CustomUnboundColumnData(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs) Handles gvZVARGBEmployees.CustomUnboundColumnData

			If e.Column.Name = "ARGBPrinted" Then
				If (e.IsGetData()) Then
					Dim data = CType(e.Row, EmployeeDataForZV)

					e.Value = If(data.ARGBPrinted.GetValueOrDefault(False), My.Resources.Checked, Nothing)
				End If
			End If

		End Sub


		''' <summary>
		'''Handles clik on btnOpenAGForm.
		''' </summary>
		Private Sub OnBtnOpenAGForm_Click(sender As Object, e As EventArgs) Handles btnOpenAGForm.Click
			Dim employeeData = SelectedZVARGBEmployeeData

			If Not employeeData Is Nothing Then

				Try

					Dim frmARGB = New SPS.MA.Guthaben.frmARGB(m_InitializationData)

					Dim preselectionSetting As New SPS.MA.Guthaben.PreselectionARGBData With {.MDNr = m_InitializationData.MDData.MDNr, .EmployeeNumber = employeeData.MANr, .Year = lueYear.EditValue, .Month = lueMonth.EditValue}
					frmARGB.PreselectionData = preselectionSetting

					frmARGB.LoadData()
					frmARGB.DisplayEmployeeData()

					frmARGB.Show()
					frmARGB.BringToFront()

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}", ex.ToString))
					m_UtilityUI.ShowErrorDialog(String.Format("{0}", ex.ToString))

				End Try

			End If

		End Sub

		Private Sub OngvZVARGBEmployees_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvZVARGBEmployees.RowCellClick

			If (e.Clicks = 2) Then

				Dim column = e.Column
				Dim employeeData = SelectedZVARGBEmployeeData

				' Send a request to open a employeeMng form.
				Dim hub = MessageService.Instance.Hub
				Dim openEmployeeMng As New OpenEmployeeMngRequest(Me, m_InitializationData.UserData.UserNr, m_MDNr, employeeData.MANr)
				hub.Publish(openEmployeeMng)

			End If

		End Sub

#End Region


#Region "helpers"

		Class IntegerValueViewWrapper
			Public Property Value As Integer
		End Class

#End Region

	End Class

End Namespace

