
Imports DevExpress.XtraGrid.Columns
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports DevExpress.LookAndFeel


Imports DevExpress.XtraPrinting.Links
Imports DevExpress.XtraPrintingLinks

Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports System.ComponentModel
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SP.DatabaseAccess.Listing.DataObjects
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraPrinting
Imports System.Drawing.Printing
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.Data
Imports DevExpress.Utils

Public Class frmSearch_LV
	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()


	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As SP.Infrastructure.Utility

	Private m_mandant As Mandant

	Private mySummaryValue As Decimal = 0

	Public Property EmployeeHoursToPrintData As BindingList(Of SuvaTableListData)


#Region "Constructor..."

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		m_mandant = New Mandant
		m_UtilityUI = New UtilityUI
		m_Utility = New SP.Infrastructure.Utility
		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		Me.KeyPreview = True
		Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		ResetGridData4EmployeeContact()
		AddHandler gvRP.PopupMenuShowing, AddressOf gridView1_PopupMenuShowing

	End Sub

#End Region


	''' <summary>
	''' Gets the selected employee.
	''' </summary>
	''' <returns>The selected employee or nothing if none is selected.</returns>
	Public ReadOnly Property SelectedRecord As SuvaTableListData
		Get
			Dim gvRP = TryCast(grdRP.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvRP Is Nothing) Then

				Dim selectedRows = gvRP.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim employee = CType(gvRP.GetRow(selectedRows(0)), SuvaTableListData)
					Return employee
				End If

			End If

			Return Nothing
		End Get

	End Property


#Region "Load Data"


	Private Function LoadCallList() As Boolean

		grdRP.DataSource = EmployeeHoursToPrintData

		Return Not EmployeeHoursToPrintData Is Nothing
	End Function

#End Region



#Region "Reset Column"

	Private Sub ResetGridData4EmployeeContact()

		gvRP.OptionsView.ShowIndicator = False
		gvRP.OptionsView.ShowAutoFilterRow = True
		gvRP.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvRP.OptionsView.ShowFooter = True

		gvRP.Columns.Clear()

		Dim columnMDNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMDNr.Caption = "maNr"
		columnMDNr.Name = "MDNr"
		columnMDNr.FieldName = "MDNr"
		columnMDNr.Visible = False
		gvRP.Columns.Add(columnMDNr)

		Dim columnReportNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnReportNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnReportNumber.Caption = m_Translate.GetSafeTranslationValue("Rapport-Nr.")
		columnReportNumber.Name = "ReportNumber"
		columnReportNumber.FieldName = "ReportNumber"
		columnReportNumber.MaxWidth = 100
		columnReportNumber.Visible = True
		gvRP.Columns.Add(columnReportNumber)

		Dim columnCustomerNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCustomerNumber.Caption = m_Translate.GetSafeTranslationValue("Kunden-Nr.")
		columnCustomerNumber.Name = "CustomerNumber"
		columnCustomerNumber.FieldName = "CustomerNumber"
		columnCustomerNumber.MaxWidth = 100
		columnCustomerNumber.Visible = True
		gvRP.Columns.Add(columnCustomerNumber)

		Dim columnEmploymentNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmploymentNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnEmploymentNumber.Caption = m_Translate.GetSafeTranslationValue("Einsatz-Nr.")
		columnEmploymentNumber.Name = "EmploymentNumber"
		columnEmploymentNumber.FieldName = "EmploymentNumber"
		columnEmploymentNumber.MaxWidth = 100
		columnEmploymentNumber.Visible = True
		gvRP.Columns.Add(columnEmploymentNumber)

		Dim columnCalendarYear As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCalendarYear.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCalendarYear.Caption = m_Translate.GetSafeTranslationValue("Jahr")
		columnCalendarYear.Name = "CalendarYear"
		columnCalendarYear.FieldName = "CalendarYear"
		columnCalendarYear.Visible = False
		gvRP.Columns.Add(columnCalendarYear)

		Dim columnMondayDate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMondayDate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnMondayDate.Caption = m_Translate.GetSafeTranslationValue("Montag")
		columnMondayDate.Name = "MondayDate"
		columnMondayDate.FieldName = "MondayDate"
		columnMondayDate.MaxWidth = 100
		columnMondayDate.Visible = True
		columnMondayDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columnMondayDate.DisplayFormat.FormatString = "d"
		gvRP.Columns.Add(columnMondayDate)

		Dim columnTuesdayDate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnTuesdayDate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnTuesdayDate.Caption = m_Translate.GetSafeTranslationValue("Dienstag")
		columnTuesdayDate.Name = "TuesdayDate"
		columnTuesdayDate.FieldName = "TuesdayDate"
		columnTuesdayDate.MaxWidth = 100
		columnTuesdayDate.Visible = True
		columnTuesdayDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columnTuesdayDate.DisplayFormat.FormatString = "d"
		gvRP.Columns.Add(columnTuesdayDate)

		Dim columnWednesdayDate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnWednesdayDate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnWednesdayDate.Caption = m_Translate.GetSafeTranslationValue("Mittwoch")
		columnWednesdayDate.Name = "WednesdayDate"
		columnWednesdayDate.FieldName = "WednesdayDate"
		columnWednesdayDate.MaxWidth = 100
		columnWednesdayDate.Visible = True
		columnWednesdayDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columnWednesdayDate.DisplayFormat.FormatString = "d"
		gvRP.Columns.Add(columnWednesdayDate)

		Dim columnThursdayDate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnThursdayDate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnThursdayDate.Caption = m_Translate.GetSafeTranslationValue("Donnerstag")
		columnThursdayDate.Name = "ThursdayDate"
		columnThursdayDate.FieldName = "ThursdayDate"
		columnThursdayDate.MaxWidth = 100
		columnThursdayDate.Visible = True
		columnThursdayDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columnThursdayDate.DisplayFormat.FormatString = "d"
		gvRP.Columns.Add(columnThursdayDate)

		Dim columnFridayDate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFridayDate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnFridayDate.Caption = m_Translate.GetSafeTranslationValue("Freitag")
		columnFridayDate.Name = "FridayDate"
		columnFridayDate.FieldName = "FridayDate"
		columnFridayDate.MaxWidth = 100
		columnFridayDate.Visible = True
		columnFridayDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columnFridayDate.DisplayFormat.FormatString = "d"
		gvRP.Columns.Add(columnFridayDate)

		Dim columnSaturdayDate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnSaturdayDate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnSaturdayDate.Caption = m_Translate.GetSafeTranslationValue("Samstag")
		columnSaturdayDate.Name = "SaturdayDate"
		columnSaturdayDate.FieldName = "SaturdayDate"
		columnSaturdayDate.MaxWidth = 100
		columnSaturdayDate.Visible = True
		columnSaturdayDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columnSaturdayDate.DisplayFormat.FormatString = "d"
		gvRP.Columns.Add(columnSaturdayDate)

		Dim columnSundayDate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnSundayDate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnSundayDate.Caption = m_Translate.GetSafeTranslationValue("Sonntag")
		columnSundayDate.Name = "SundayDate"
		columnSundayDate.FieldName = "SundayDate"
		columnSundayDate.MaxWidth = 100
		columnSundayDate.Visible = True
		columnSundayDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columnSundayDate.DisplayFormat.FormatString = "d"
		gvRP.Columns.Add(columnSundayDate)

		Dim columnTag1 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnTag1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnTag1.Caption = m_Translate.GetSafeTranslationValue("Montag Stunden")
		columnTag1.Name = "Tag1"
		columnTag1.FieldName = "Tag1"
		columnTag1.MaxWidth = 70
		columnTag1.Visible = True
		gvRP.Columns.Add(columnTag1)

		Dim columnTag2 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnTag2.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnTag2.Caption = m_Translate.GetSafeTranslationValue("Dienstag Stunden")
		columnTag2.Name = "Tag2"
		columnTag2.FieldName = "Tag2"
		columnTag2.MaxWidth = 70
		columnTag2.Visible = True
		gvRP.Columns.Add(columnTag2)

		Dim columnTag3 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnTag3.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnTag3.Caption = m_Translate.GetSafeTranslationValue("Mittwoch Stunden")
		columnTag3.Name = "Tag3"
		columnTag3.FieldName = "Tag3"
		columnTag3.MaxWidth = 70
		columnTag3.Visible = True
		gvRP.Columns.Add(columnTag3)

		Dim columnTag4 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnTag4.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnTag4.Caption = m_Translate.GetSafeTranslationValue("Donnerstag Stunden")
		columnTag4.Name = "Tag4"
		columnTag4.FieldName = "Tag4"
		columnTag4.MaxWidth = 70
		columnTag4.Visible = True
		gvRP.Columns.Add(columnTag4)

		Dim columnTag5 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnTag5.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnTag5.Caption = m_Translate.GetSafeTranslationValue("Freitag Stunden")
		columnTag5.Name = "Tag5"
		columnTag5.FieldName = "Tag5"
		columnTag5.MaxWidth = 70
		columnTag5.Visible = True
		gvRP.Columns.Add(columnTag5)

		Dim columnTag6 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnTag6.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnTag6.Caption = m_Translate.GetSafeTranslationValue("Samstag Stunden")
		columnTag6.Name = "Tag6"
		columnTag6.FieldName = "Tag6"
		columnTag6.MaxWidth = 70
		columnTag6.Visible = True
		gvRP.Columns.Add(columnTag6)

		Dim columnTag7 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnTag7.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnTag7.Caption = m_Translate.GetSafeTranslationValue("Sonntag Stunden")
		columnTag7.Name = "Tag7"
		columnTag7.FieldName = "Tag7"
		columnTag7.MaxWidth = 70
		columnTag7.Visible = True
		gvRP.Columns.Add(columnTag7)

		Dim columnWorkedHourCount As New DevExpress.XtraGrid.Columns.GridColumn()
		columnWorkedHourCount.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnWorkedHourCount.Caption = m_Translate.GetSafeTranslationValue("Anszahl Stunden")
		columnWorkedHourCount.Name = "WorkedHourCount"
		columnWorkedHourCount.FieldName = "WorkedHourCount"
		columnWorkedHourCount.MaxWidth = 100
		columnWorkedHourCount.Visible = True
		columnWorkedHourCount.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnWorkedHourCount.SummaryItem.DisplayFormat = "{0:n2}"
		Dim customSummary As GridColumnSummaryItem = columnWorkedHourCount.Summary.Add(DevExpress.Data.SummaryItemType.Custom, "WorkedHourCount", m_Translate.GetSafeTranslationValue("{0:n2}"))
		customSummary.Tag = "total" ' to distinguish between several summaries
		gvRP.Columns.Add(columnWorkedHourCount)

		Dim columnWorkedDayCount As New DevExpress.XtraGrid.Columns.GridColumn()
		columnWorkedDayCount.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnWorkedDayCount.Caption = m_Translate.GetSafeTranslationValue("Anszahl Tage")
		columnWorkedDayCount.Name = "WorkedDayCount"
		columnWorkedDayCount.FieldName = "WorkedDayCount"
		columnWorkedDayCount.MaxWidth = 100
		columnWorkedDayCount.Visible = True
		columnWorkedDayCount.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnWorkedDayCount.SummaryItem.DisplayFormat = "{0:n2}"
		gvRP.Columns.Add(columnWorkedDayCount)

		'Dim columnAverageHour As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnAverageHour.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		'columnAverageHour.Caption = m_Translate.GetSafeTranslationValue("Durchschnitt")
		'columnAverageHour.Name = "AverageHour"
		'columnAverageHour.FieldName = "AverageHour"
		'columnAverageHour.MaxWidth = 100
		'columnAverageHour.Visible = True
		'columnAverageHour.DisplayFormat.FormatType = FormatType.Numeric
		'columnAverageHour.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		'columnAverageHour.SummaryItem.DisplayFormat = "{0:n2}"
		''Dim customSummary As GridColumnSummaryItem = columnAverageHour.Summary.Add(DevExpress.Data.SummaryItemType.Custom, "AverageHour", m_Translate.GetSafeTranslationValue("{0:n2}"))
		''customSummary.Tag = "total" ' to distinguish between several summaries
		'gvRP.Columns.Add(columnAverageHour)

		'Dim chkColumn_TotalValue As GridColumn = gvRP.Columns("AverageHour")
		'If chkColumn_TotalValue Is Nothing Then

		'	Dim column As GridColumn = gvRP.Columns.AddField("AverageHour")

		'	column.UnboundType = UnboundColumnType.Decimal
		'	column.VisibleIndex = gvRP.Columns.Count
		'	column.Visible = True
		'	column.DisplayFormat.FormatType = FormatType.Numeric
		'	column.DisplayFormat.FormatString = "n2"
		'End If

		'Dim customSummary As GridColumnSummaryItem = gvRP.Columns("AverageHour").Summary.Add(DevExpress.Data.SummaryItemType.Custom, "AverageHour", m_Translate.GetSafeTranslationValue("{0:n2}"))
		'customSummary.Tag = "total" ' to distinguish between several summaries


		grdRP.DataSource = Nothing

	End Sub


#End Region

	Sub TranslateControls()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)
			bbiPrintList.Caption = m_Translate.GetSafeTranslationValue(Me.bbiPrintList.Caption)

		Catch ex As Exception
			m_Logger.LogError(String.Format("1=>{0}.{1}", strMethodeName, ex.Message))
		End Try

	End Sub

	Private Sub frmOnDisposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.frm_LVLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.ifrmLVHeight = Me.Height
				My.Settings.ifrmLVWidth = Me.Width

				My.Settings.Save()
			End If


		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub

	Private Sub frmDataSel_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

		Try
			Me.Width = Math.Max(My.Settings.ifrmLVWidth, Me.Width)
			Me.Height = Math.Max(My.Settings.ifrmLVHeight, Me.Height)

			If My.Settings.frm_LVLocation <> String.Empty Then
				Dim aLoc As String() = My.Settings.frm_LVLocation.Split(CChar(";"))

				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = CStr(0)
				End If
				Me.Location = New System.Drawing.Point(CInt(Math.Max(Val(aLoc(0)), 0)), CInt(Math.Max(Val(aLoc(1)), 0)))
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		End Try

		Try

			LoadCallList()
			Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), gvRP.RowCount)

			AddHandler gvRP.RowCellClick, AddressOf Ongv_RowCellClick

		Catch ex As Exception

		End Try

	End Sub


	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvRP.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, SuvaTableListData)

				Select Case column.Name.ToLower
					Case "CustomerNumber".ToLower
						If viewData.CustomerNumber > 0 Then LoadSelectedCustomer(viewData.CustomerNumber)

					Case "EmploymentNumber".ToLower
						If viewData.EmploymentNumber > 0 Then LoadSelectedEmployment(viewData.EmploymentNumber)

					Case Else
						If viewData.ReportNumber > 0 Then LoadSelectedReport(viewData.ReportNumber)


				End Select

			End If

		End If

	End Sub

	Private Sub PreviewPrintableComponent(component As IPrintable, lookAndFeel As UserLookAndFeel)
		' Create a link that will print a control.  
		Dim link As New PrintableComponentLink() With {
						.PrintingSystemBase = New PrintingSystem(),
						.Component = component,
						.Landscape = True,
						.PaperKind = PaperKind.A4,
						.Margins = New Margins(20, 20, 20, 20)
				}
		' Show the report. 
		link.ShowRibbonPreview(lookAndFeel)
	End Sub

	Private Sub bbiPrintList_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrintList.ItemClick
		If gvRP.RowCount > 0 Then

			PreviewPrintableComponent(grdRP, grdRP.LookAndFeel)
			'grdRP.ExportToText("", New DevExpress.XtraPrinting.TextExportOptions(";"))
			'grdRP.ShowPrintPreview()
		End If

	End Sub

	Private Sub OnGvCurrentList_CustomColumnDisplayText(ByVal sender As Object, ByVal e As CustomColumnDisplayTextEventArgs) Handles gvRP.CustomColumnDisplayText

		If e.Column.FieldName = "WorkedDayCount" OrElse e.Column.FieldName = "WorkedHourCount" OrElse e.Column.FieldName = "AverageHour" Then
			If Val(e.Value) = 0 Then e.DisplayText = String.Empty
		End If

	End Sub

	Private Sub gridView1_PopupMenuShowing(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs)
		If e.MenuType <> DevExpress.XtraGrid.Views.Grid.GridMenuType.Summary Then
			Return
		End If

		Dim footerMenu As DevExpress.XtraGrid.Menu.GridViewFooterMenu = TryCast(e.Menu, DevExpress.XtraGrid.Menu.GridViewFooterMenu)
		Dim check As Boolean = e.HitInfo.Column.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Custom AndAlso Equals("Count", e.HitInfo.Column.SummaryItem.Tag)
		Dim menuItem As DevExpress.Utils.Menu.DXMenuItem = New DevExpress.Utils.Menu.DXMenuCheckItem("Active Count", check, Nothing, New EventHandler(AddressOf MyMenuItem))
		menuItem.Tag = e.HitInfo.Column
		For Each item As DevExpress.Utils.Menu.DXMenuItem In footerMenu.Items
			item.Enabled = True
		Next item
		footerMenu.Items.Add(menuItem)


	End Sub

	Private Sub MyMenuItem(ByVal sender As Object, ByVal e As EventArgs)
		Dim Item As DevExpress.Utils.Menu.DXMenuItem = TryCast(sender, DevExpress.Utils.Menu.DXMenuItem)
		Dim col As GridColumn = TryCast(Item.Tag, GridColumn)
		col.SummaryItem.Tag = "Count"
		col.SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Custom, String.Empty)
	End Sub

	Private validRowCount As Integer = 0

	Private Sub OngvRP_CustomSummaryCalculate(ByVal sender As Object, ByVal e As DevExpress.Data.CustomSummaryEventArgs) Handles gvRP.CustomSummaryCalculate
		Dim item As GridColumnSummaryItem = TryCast(e.Item, GridColumnSummaryItem)
		Dim view As GridView = TryCast(sender, GridView)
		'If Equals("Count", item.Tag) Then
		'	If e.SummaryProcess = DevExpress.Data.CustomSummaryProcess.Start Then
		'		validRowCount = 0
		'	End If
		'	If e.SummaryProcess = DevExpress.Data.CustomSummaryProcess.Calculate Then
		'		If (Not Convert.ToBoolean(view.GetRowCellValue(e.RowHandle, "Discontinued"))) Then
		'			validRowCount += 1
		'		End If
		'	End If
		'	If e.SummaryProcess = DevExpress.Data.CustomSummaryProcess.Finalize Then
		'		e.TotalValue = validRowCount
		'	End If
		'End If


		Try

			Dim customSummary As GridColumnSummaryItem = CType(e.Item, GridColumnSummaryItem)
			If customSummary.Tag.ToString = "total" Then
				'If e.SummaryProcess = CustomSummaryProcess.Start Then
				'	mySummaryValue = 0
				'End If
				'If e.SummaryProcess = CustomSummaryProcess.Calculate Then
				'	Dim totalValue As Decimal = Convert.ToDecimal(view.GetRowCellValue(e.RowHandle, customSummary.FieldName))
				'	mySummaryValue += totalValue
				'End If
				'If e.SummaryProcess = CustomSummaryProcess.Finalize Then
				Dim totalWorkHour As Decimal = EmployeeHoursToPrintData.Sum(Function(x) x.WorkedHourCount)
				Dim totalDay As Decimal = EmployeeHoursToPrintData.Sum(Function(x) x.WorkedDayCount)
				e.TotalValue = String.Format("Ø: {0:n2}", totalWorkHour / totalDay)
				'	e.TotalValue = mySummaryValue * 7.5
				'End If

			End If

		Catch ex As Exception

		End Try


	End Sub

	Private Sub gvRP_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles gvRP.CustomUnboundColumnData

		If e.Column.FieldName = "AverageHour" AndAlso e.IsGetData Then
			Dim Cell_Price_Value As Object = gvRP.GetListSourceRowCellValue(e.ListSourceRowIndex, "WorkedHourCount")
			Dim Cell_Salery_Value As Object = gvRP.GetListSourceRowCellValue(e.ListSourceRowIndex, "WorkedDayCount")
			If Cell_Salery_Value > 0 AndAlso (Cell_Price_Value IsNot Nothing AndAlso Cell_Salery_Value IsNot Nothing) Then
				e.Value = Math.Abs(Convert.ToDecimal(Cell_Price_Value) / Convert.ToDecimal(Cell_Salery_Value))
			Else
				e.Value = 0
			End If
		End If

	End Sub

	Sub LoadSelectedEmployment(ByVal employmentNumber As Integer)

		Try
			Dim hub = MessageService.Instance.Hub
			Dim openMng As New OpenEinsatzMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, employmentNumber)
			hub.Publish(openMng)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
		End Try

	End Sub

	Sub LoadSelectedCustomer(ByVal _iKDNr As Integer)

		Try
			Dim hub = MessageService.Instance.Hub
			Dim openMng As New OpenCustomerMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, _iKDNr)
			hub.Publish(openMng)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

	End Sub

	Sub LoadSelectedReport(ByVal reportNumber As Integer)

		Dim hub = MessageService.Instance.Hub
		Dim openMng As New OpenReportsMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, reportNumber)
		hub.Publish(openMng)

	End Sub

End Class