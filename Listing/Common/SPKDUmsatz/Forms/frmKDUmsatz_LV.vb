
Imports DevExpress.LookAndFeel

Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports System.ComponentModel
Imports SPKDUmsatz.ClsDataDetail
Imports DevExpress.XtraGrid.Views.Base
Imports SPProgUtility.CommonXmlUtility
Imports SP.Infrastructure.UI
Imports System.IO
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages

Public Class frmKDUmsatz_LV

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private _ClsFunc As New ClsDivFunc
	Private Stopwatch As Stopwatch = New Stopwatch()

	Private m_utility As Utilities
	Private m_utilityUI As UtilityUI

	Private m_md As Mandant

	Public Property RecCount As Integer
	Private Property Sql2Open As String

	Private Property WithSecendYear As Boolean
	Private Property WithDetails As Boolean

	Private Property _dBetrag_1 As Decimal
	Private Property _dBetrag_2 As Decimal
	Private Property _dBetrag_3 As Decimal
	Private Property _dBetrag_4 As Decimal

	Private m_GridSettingPath As String
	Private UserGridSettingsXml As SettingsXml

	Private m_GVSearchSettingfilename As String

	Private m_xmlSettingRestoreSearchSetting As String
	Private m_xmlSettingSearchFilter As String



#Region "Private Consts"


	Private Const MODUL_NAME_SETTING = "kdumsatzsearch"

	Private Const USER_XML_SETTING_SPUTNIK_SEARCH_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/kdumsatzsearch/{1}/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_SEARCH_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/kdumsatzsearch/{1}/keepfilter"


#End Region



#Region "Constructor"

	Public Sub New(ByVal strQuery As String, ByVal _withSecYear As Boolean, ByVal _asDetail As Boolean)

		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		m_md = New Mandant
		m_utility = New Utilities
		m_utilityUI = New UtilityUI

		_dBetrag_1 = 0
		_dBetrag_2 = 0
		_dBetrag_3 = 0
		_dBetrag_4 = 0

		_ClsFunc.GetSearchQuery = strQuery
		Me.Sql2Open = strQuery
		Me.WithDetails = _asDetail
		Me.WithSecendYear = _withSecYear

		Try
			m_GridSettingPath = String.Format("{0}KDUmsatzSearch\", m_md.GetGridSettingPath(ClsDataDetail.MDData.MDNr))
			If Not Directory.Exists(m_GridSettingPath) Then Directory.CreateDirectory(m_GridSettingPath)

			m_GVSearchSettingfilename = String.Format("{0}{1}{2}.xml", m_GridSettingPath, Me.grdRP.Name, ClsDataDetail.UserData.UserNr)

			m_xmlSettingRestoreSearchSetting = String.Format(USER_XML_SETTING_SPUTNIK_SEARCH_GRIDSETTING_RESTORE, ClsDataDetail.UserData.UserNr, MODUL_NAME_SETTING)
			m_xmlSettingSearchFilter = String.Format(USER_XML_SETTING_SPUTNIK_SEARCH_GRIDSETTING_FILTER, ClsDataDetail.UserData.UserNr, MODUL_NAME_SETTING)

			UserGridSettingsXml = New SettingsXml(m_md.GetAllUserGridSettingXMLFilename(ClsDataDetail.MDData.MDNr))

		Catch ex As Exception

		End Try

		ResetGridRPData()

	End Sub

#End Region


	''' <summary>
	''' Gets the selected employee.
	''' </summary>
	''' <returns>The selected employee or nothing if none is selected.</returns>
	Public ReadOnly Property SelectedRecord As FoundedData
		Get
			Dim gvRP = TryCast(grdRP.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvRP Is Nothing) Then

				Dim selectedRows = gvRP.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim employee = CType(gvRP.GetRow(selectedRows(0)), FoundedData)
					Return employee
				End If

			End If

			Return Nothing
		End Get

	End Property

	Function GetDbData4Show() As IEnumerable(Of FoundedData)
		Dim result As List(Of FoundedData) = Nothing
		m_utility = New Utilities

		Dim sql As String

		sql = Sql2Open

		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ClsDataDetail.GetSelectedMDConnstring, sql, Nothing)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of FoundedData)

				While reader.Read()
					Dim overviewData As New FoundedData

					overviewData.KDNr = CInt(m_utility.SafeGetInteger(reader, "KDNr", 0))

					overviewData.customername = String.Format("{0}", m_utility.SafeGetString(reader, "r_Name1"))
					overviewData.customeraddress = String.Format("{0}, {1} {2}", m_utility.SafeGetString(reader, "r_strasse"),
																											 m_utility.SafeGetString(reader, "r_plz"),
																											 m_utility.SafeGetString(reader, "r_ort"))

					overviewData.betragohne_1 = CDec(m_utility.SafeGetDecimal(reader, "fBetragOhne", 0))
					overviewData.betragex_1 = CDec(m_utility.SafeGetDecimal(reader, "fBetragex", 0))
					overviewData.mwSt_1 = CDec(m_utility.SafeGetDecimal(reader, "fbetragmwst", 0))
					overviewData.total_1 = CDec(m_utility.SafeGetDecimal(reader, "fbetragink", 0))

					_dBetrag_1 += overviewData.total_1

					overviewData.betragohne_2 = CDec(m_utility.SafeGetDecimal(reader, "sBetragOhne", 0))
					overviewData.betragex_2 = CDec(m_utility.SafeGetDecimal(reader, "sBetragex", 0))
					overviewData.mwSt_2 = CDec(m_utility.SafeGetDecimal(reader, "sbetragmwst", 0))
					overviewData.total_2 = CDec(m_utility.SafeGetDecimal(reader, "sbetragink", 0))

					_dBetrag_2 += overviewData.total_2

					overviewData.kst_1 = String.Format("{0}", m_utility.SafeGetString(reader, "rekst1"))
					overviewData.kst_2 = String.Format("{0}", m_utility.SafeGetString(reader, "rekst2"))
					overviewData.kst = String.Format("{0}", m_utility.SafeGetString(reader, "kst"))

					result.Add(overviewData)

				End While

			End If

		Catch e As Exception
			result = Nothing
			m_Logger.LogError(e.ToString())

		Finally
			m_utility.CloseReader(reader)

		End Try

		Return result
	End Function

	Private Function LoadFoundedList() As Boolean

		Dim listOfEmployees = GetDbData4Show()

		Dim responsiblePersonsGridData = (From person In listOfEmployees
																			Select New FoundedData With
																						 {.KDNr = person.KDNr,
																							.customername = person.customername,
																							.customeraddress = person.customeraddress,
																							.betragohne_1 = person.betragohne_1,
																							.betragex_1 = person.betragex_1,
																							.mwSt_1 = person.mwSt_1,
																							.total_1 = person.total_1,
																							.betragohne_2 = person.betragohne_2,
																							.betragex_2 = person.betragex_2,
																							.mwSt_2 = person.mwSt_2,
																							.total_2 = person.total_2,
																							.kst_1 = person.kst_1,
																							.kst_2 = person.kst_2,
																							.kst = person.kst}).ToList()

		Dim listDataSource As BindingList(Of FoundedData) = New BindingList(Of FoundedData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdRP.DataSource = listDataSource

		Return Not listOfEmployees Is Nothing
	End Function


	Private Sub ResetGridRPData()

		gvRP.OptionsView.ShowIndicator = False
		gvRP.OptionsView.ShowAutoFilterRow = True
		gvRP.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvRP.OptionsView.ShowFooter = True

		Dim AutofilterconditionNumber = ClsDataDetail.MDData.AutoFilterConditionNumber
		Dim AutofilterconditionDate = ClsDataDetail.MDData.AutoFilterConditionDate

		gvRP.Columns.Clear()

		Dim columnKDNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnKDNr.OptionsFilter.AutoFilterCondition = AutofilterconditionNumber
		columnKDNr.Caption = "KDNr"
		columnKDNr.Name = "KDNr"
		columnKDNr.FieldName = "KDNr"
		columnKDNr.Visible = False
		gvRP.Columns.Add(columnKDNr)

		Dim columnCustomer As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomer.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCustomer.Caption = "Kunde"
		columnCustomer.Name = "customername"
		columnCustomer.FieldName = "customername"
		columnCustomer.Visible = True
		gvRP.Columns.Add(columnCustomer)

		Dim columnAddresse As New DevExpress.XtraGrid.Columns.GridColumn()
		columnAddresse.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnAddresse.Caption = "Adresse"
		columnAddresse.Name = "customeraddress"
		columnAddresse.FieldName = "customeraddress"
		columnAddresse.Visible = True
		gvRP.Columns.Add(columnAddresse)

		Dim fbetagohne As New DevExpress.XtraGrid.Columns.GridColumn()
		fbetagohne.OptionsFilter.AutoFilterCondition = AutofilterconditionNumber
		fbetagohne.Caption = "1. Betrag MwSt.-frei"
		fbetagohne.Name = "betragohne_1"
		fbetagohne.FieldName = "betragohne_1"
		fbetagohne.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		fbetagohne.DisplayFormat.FormatString = "n2"
		fbetagohne.Visible = True
		fbetagohne.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		fbetagohne.SummaryItem.DisplayFormat = "{0:n2}"
		gvRP.Columns.Add(fbetagohne)

		Dim fbetagex As New DevExpress.XtraGrid.Columns.GridColumn()
		fbetagex.OptionsFilter.AutoFilterCondition = AutofilterconditionNumber
		fbetagex.Caption = "1. Betrag exkl. MwSt."
		fbetagex.Name = "betragex_1"
		fbetagex.FieldName = "betragex_1"
		fbetagex.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		fbetagex.DisplayFormat.FormatString = "n2"
		fbetagex.Visible = False
		fbetagex.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		fbetagex.SummaryItem.DisplayFormat = "{0:n2}"
		gvRP.Columns.Add(fbetagex)

		Dim fbetagmwst As New DevExpress.XtraGrid.Columns.GridColumn()
		fbetagmwst.OptionsFilter.AutoFilterCondition = AutofilterconditionNumber
		fbetagmwst.Caption = "1. MwSt.-Betrag"
		fbetagmwst.Name = "mwSt_1"
		fbetagmwst.FieldName = "mwSt_1"
		fbetagmwst.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		fbetagmwst.DisplayFormat.FormatString = "n2"
		fbetagmwst.Visible = False
		fbetagmwst.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		fbetagmwst.SummaryItem.DisplayFormat = "{0:n2}"
		gvRP.Columns.Add(fbetagmwst)

		Dim fbetagTotal As New DevExpress.XtraGrid.Columns.GridColumn()
		fbetagTotal.OptionsFilter.AutoFilterCondition = AutofilterconditionNumber
		fbetagTotal.Caption = "1. Total inkl. MwSt."
		fbetagTotal.Name = "total_1"
		fbetagTotal.FieldName = "total_1"
		fbetagTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		fbetagTotal.DisplayFormat.FormatString = "n2"
		fbetagTotal.Visible = True
		fbetagTotal.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		fbetagTotal.SummaryItem.DisplayFormat = "{0:n2}"
		gvRP.Columns.Add(fbetagTotal)

		Dim sbetagohne As New DevExpress.XtraGrid.Columns.GridColumn()
		sbetagohne.OptionsFilter.AutoFilterCondition = AutofilterconditionNumber
		sbetagohne.Caption = "2. Betrag MwSt.-frei"
		sbetagohne.Name = "betragohne_2"
		sbetagohne.FieldName = "betragohne_2"
		sbetagohne.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		sbetagohne.DisplayFormat.FormatString = "n2"
		sbetagohne.Visible = Me.WithSecendYear
		sbetagohne.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		sbetagohne.SummaryItem.DisplayFormat = "{0:n2}"
		gvRP.Columns.Add(sbetagohne)

		Dim sbetagex As New DevExpress.XtraGrid.Columns.GridColumn()
		sbetagex.Caption = "2. Betrag exkl. MwSt."
		sbetagex.Name = "betragex_2"
		sbetagex.FieldName = "betragex_2"
		sbetagex.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		sbetagex.DisplayFormat.FormatString = "n2"
		sbetagex.Visible = Me.WithSecendYear
		sbetagex.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		sbetagex.SummaryItem.DisplayFormat = "{0:n2}"
		gvRP.Columns.Add(sbetagex)

		Dim sbetagmwst As New DevExpress.XtraGrid.Columns.GridColumn()
		sbetagmwst.OptionsFilter.AutoFilterCondition = AutofilterconditionNumber
		sbetagmwst.Caption = "2. MwSt.-Betrag"
		sbetagmwst.Name = "mwSt_2"
		sbetagmwst.FieldName = "mwSt_2"
		sbetagmwst.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		sbetagmwst.DisplayFormat.FormatString = "n2"
		sbetagmwst.Visible = Me.WithSecendYear
		sbetagmwst.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		sbetagmwst.SummaryItem.DisplayFormat = "{0:n2}"
		gvRP.Columns.Add(sbetagmwst)

		Dim sbetagTotal As New DevExpress.XtraGrid.Columns.GridColumn()
		sbetagTotal.OptionsFilter.AutoFilterCondition = AutofilterconditionNumber
		sbetagTotal.Caption = "2. Total inkkl. MwSt."
		sbetagTotal.Name = "total_2"
		sbetagTotal.FieldName = "total_2"
		sbetagTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		sbetagTotal.DisplayFormat.FormatString = "n2"
		sbetagTotal.Visible = Me.WithSecendYear
		sbetagTotal.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		sbetagTotal.SummaryItem.DisplayFormat = "{0:n2}"
		gvRP.Columns.Add(sbetagTotal)


		Dim ColumnfKST As New DevExpress.XtraGrid.Columns.GridColumn()
		ColumnfKST.Caption = "1. KST."
		ColumnfKST.Name = "kst_1"
		ColumnfKST.FieldName = "kst_1"
		ColumnfKST.Visible = Me.WithDetails
		gvRP.Columns.Add(ColumnfKST)

		Dim ColumnsKST As New DevExpress.XtraGrid.Columns.GridColumn()
		ColumnsKST.Caption = "2. KST."
		ColumnsKST.Name = "kst_2"
		ColumnsKST.FieldName = "kst_2"
		ColumnsKST.Visible = WithDetails
		gvRP.Columns.Add(ColumnsKST)

		Dim ColumnKST As New DevExpress.XtraGrid.Columns.GridColumn()
		ColumnKST.Caption = "KST."
		ColumnKST.Name = "kst"
		ColumnKST.FieldName = "kst"
		ColumnKST.Visible = WithDetails
		gvRP.Columns.Add(ColumnKST)


		RestoreGridLayoutFromXml()

		grdRP.DataSource = Nothing

	End Sub


#Region "Funktionen für Listview..."

	'Sub OpenMyOP(ByVal sender As Object, ByVal e As EventArgs)
	'	Dim iOPNr As Integer = 0

	'	iOPNr = CInt(_ClsFunc.GetOPNr)
	'	RunOpenOPForm(iOPNr)

	'End Sub

	'Sub OpenMyKD(ByVal sender As Object, ByVal e As EventArgs)
	'	Dim iKDNr As Integer = 0

	'	iKDNr = CInt(_ClsFunc.GetKDNr)
	'	RunOpenKDForm(iKDNr)

	'End Sub


#End Region

	Private Sub frmKDUmsatz_LV_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
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

	Sub TranslateControls()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)
			Me.bbiTotalbetrag.Caption = m_Translate.GetSafeTranslationValue(Me.bbiTotalbetrag.Caption)
			bbiPrintList.Caption = m_Translate.GetSafeTranslationValue(Me.bbiPrintList.Caption)


		Catch ex As Exception
			m_Logger.LogError(String.Format("1=>{0}.{1}", strMethodeName, ex.Message))
		End Try

	End Sub

	Private Sub frmOnLoad(sender As Object, e As System.EventArgs) Handles Me.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim m_md As New Mandant

		Dim strStyleName As String = m_md.GetSelectedUILayoutName(m_InitialData.MDData.MDNr, 0, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		TranslateControls()

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
			m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.Message))

		End Try

		Try
			LoadFoundedList()

			Me.RecCount = gvRP.RowCount
			Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), Me.RecCount)
			If Me.RecCount > 0 Then CreateExportPopupMenu()

			AddHandler gvRP.RowCellClick, AddressOf Ongv_RowCellClick
			AddHandler Me.gvRP.ColumnPositionChanged, AddressOf OngvColumnPositionChanged
			AddHandler Me.gvRP.ColumnWidthChanged, AddressOf OngvColumnPositionChanged

		Catch ex As Exception

		End Try

	End Sub


	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvRP.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, FoundedData)

				Select Case column.Name.ToLower
					Case Else
						If viewData.KDNr > 0 Then loadSelectedCustomer(viewData.KDNr)

				End Select

			End If

		End If

	End Sub

	Private Sub bbiPrintList_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrintList.ItemClick
		If gvRP.RowCount > 0 Then
			' Opens the Preview window. 
			grdRP.ShowPrintPreview()
		End If

	End Sub

	Private Sub OnGvCurrentList_CustomColumnDisplayText(ByVal sender As Object, ByVal e As CustomColumnDisplayTextEventArgs) Handles gvRP.CustomColumnDisplayText

		If e.Column.FieldName = "betragohne_1" Or e.Column.FieldName = "betragex_1" Or e.Column.FieldName = "mwSt_1" Or e.Column.FieldName = "total_1" Or
				e.Column.FieldName = "betragohne_2" Or e.Column.FieldName = "betragex_2" Or e.Column.FieldName = "mwSt_2" Or e.Column.FieldName = "total_2" Then
			If Val(e.Value) = 0 Then e.DisplayText = String.Empty
		End If

	End Sub

	Private Sub OngvColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvRP.OptionsView.ShowFooter = (gvRP.Columns("betragohne_1").Visible OrElse gvRP.Columns("betragex_1").Visible OrElse
																	gvRP.Columns("mwSt_1").Visible OrElse gvRP.Columns("total_1").Visible OrElse
																	gvRP.Columns("betragohne_2").Visible OrElse gvRP.Columns("betragex_2").Visible OrElse
																	gvRP.Columns("mwSt_2").Visible OrElse gvRP.Columns("total_2").Visible)
		gvRP.SaveLayoutToXml(m_GVSearchSettingfilename)

	End Sub

	Private Sub RestoreGridLayoutFromXml()
		Dim keepFilter = False
		Dim restoreLayout = True

		Try
			restoreLayout = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreSearchSetting), True)
			keepFilter = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingSearchFilter), False)
		Catch ex As Exception

		End Try
		If File.Exists(m_GVSearchSettingfilename) Then gvRP.RestoreLayoutFromXml(m_GVSearchSettingfilename)

		If restoreLayout AndAlso Not keepFilter Then gvRP.ActiveFilterCriteria = Nothing

	End Sub

	Sub loadSelectedCustomer(ByVal iCustomerNr As Integer)

		Dim hub = MessageService.Instance.Hub
		Dim openMng As New OpenCustomerMngRequest(Me, ClsDataDetail.UserData.UserNr, ClsDataDetail.MDData.MDNr, iCustomerNr)
		hub.Publish(openMng)

	End Sub



	Private Sub bbiTotalbetrag_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiTotalbetrag.ItemClick
		Dim popupMenu As DevExpress.XtraBars.PopupMenu = CType(Me.bbiTotalbetrag.DropDownControl, DevExpress.XtraBars.PopupMenu)

		If Not (popupMenu Is Nothing) Then popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

	End Sub

	Private Sub CreateExportPopupMenu()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim bshowMnu As Boolean = True
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim liMnu As List(Of String) = GetMenuItems4Currencies(_dBetrag_1, _dBetrag_2)

		Try
			bbiTotalbetrag.Manager = Me.BarManager1
			BarManager1.ForceInitialize()
			Me.bbiTotalbetrag.ActAsDropDown = False
			Me.bbiTotalbetrag.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
			Me.bbiTotalbetrag.DropDownEnabled = True
			Me.bbiTotalbetrag.DropDownControl = popupMenu
			Me.bbiTotalbetrag.Enabled = True

			For i As Integer = 0 To liMnu.Count - 1
				Dim myValue As String() = liMnu(i).Split(CChar("#"))
				If bshowMnu Then
					popupMenu.Manager = BarManager1

					Dim itm As New DevExpress.XtraBars.BarButtonItem
					itm.Caption = m_Translate.GetSafeTranslationValue(myValue(0).ToString).Replace("-", "")
					'itm.Name = m_xml.GetSafeTranslationValue(myValue(1).ToString)
					'itm.AccessibleName = myValue(2).ToString
					If myValue(0).ToString.ToLower.Contains("-".ToLower) Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)

				End If
			Next

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try
	End Sub


	Function GetMenuItems4Currencies(ByVal dBetrag_1 As Double, ByVal dBetrag_2 As Double) As List(Of String)
		Dim liResult As New List(Of String)

		Try
			liResult.Add(String.Format(m_Translate.GetSafeTranslationValue("1. Jahr Totalbetrag inkl. MwSt.: {0}"), Format(dBetrag_1, "c2")))
			If dBetrag_2 <> 0 Then
				liResult.Add(String.Format(m_Translate.GetSafeTranslationValue("2. Jahr Totalbetrag inkl. MwSt.: {0}"), Format(dBetrag_2, "c2")))
			End If

		Catch e As Exception
			MsgBox(Err.GetException.ToString)

		Finally

		End Try

		Return liResult

	End Function

End Class