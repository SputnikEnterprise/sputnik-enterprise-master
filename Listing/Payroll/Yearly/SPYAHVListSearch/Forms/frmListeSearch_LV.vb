

Option Strict Off

Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports System.Threading

Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities

Imports DevExpress.LookAndFeel
Imports System.ComponentModel
Imports System.IO
Imports SPProgUtility.CommonXmlUtility

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SPYAHVListSearch.ClsDataDetail

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraPivotGrid


Public Class frmListeSearch_LV
	Inherits DevExpress.XtraEditors.XtraForm
	Protected Shared m_Logger As ILogger = New Logger()

	Private m_md As Mandant
	Private m_utility As Utilities
	Private m_UtilityUi As SP.Infrastructure.UI.UtilityUI

	Public Property RecCount As Integer
	Private Property Sql2Open As String
	Private Property Modul2Open As String

	Private m_GridSettingPath As String
	Private UserGridSettingsXml As SettingsXml

	Private m_GVSearchSettingfilename As String

	Private m_xmlSettingRestoreLoYFAKSearchSetting As String
	Private m_xmlSettingLoYFAKSearchFilter As String



#Region "Private Consts"

	Private Const MODUL_NAME_SETTING = "lojahvsearchlist"

	Private Const USER_XML_SETTING_SPUTNIK_LOYFAK_SEARCH_LIST_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/lolisting/{1}/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_LOYFAK_SEARCH_LIST_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/lolisting/{1}/keepfilter"

#End Region


#Region "Constructor"

	Public Sub New(ByVal strQuery As String, ByVal _modulname As String)

		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		m_md = New Mandant
		m_utility = New Utilities
		m_UtilityUi = New UtilityUI

		Me.Sql2Open = strQuery
		Me.Modul2Open = _modulname

		Try
			m_GridSettingPath = String.Format("{0}{1}\", m_md.GetGridSettingPath(ClsDataDetail.m_InitialData.MDData.MDNr), MODUL_NAME_SETTING)

			If Not Directory.Exists(m_GridSettingPath) Then Directory.CreateDirectory(m_GridSettingPath)

			m_GVSearchSettingfilename = String.Format("{0}{1}{2}.xml", m_GridSettingPath, Me.grdRP.Name, ClsDataDetail.m_InitialData.UserData.UserNr)

			m_xmlSettingRestoreLoYFAKSearchSetting = String.Format(USER_XML_SETTING_SPUTNIK_LOYFAK_SEARCH_LIST_GRIDSETTING_RESTORE, ClsDataDetail.m_InitialData.UserData.UserNr, MODUL_NAME_SETTING)
			m_xmlSettingLoYFAKSearchFilter = String.Format(USER_XML_SETTING_SPUTNIK_LOYFAK_SEARCH_LIST_GRIDSETTING_FILTER, ClsDataDetail.m_InitialData.UserData.UserNr, MODUL_NAME_SETTING)

			UserGridSettingsXml = New SettingsXml(m_md.GetAllUserGridSettingXMLFilename(ClsDataDetail.m_InitialData.MDData.MDNr))

		Catch ex As Exception

		End Try

		ResetGridSalaryData()

		AddHandler Me.gvRP.ColumnFilterChanged, AddressOf OnGVDetail_ColumnFilterChanged
		AddHandler Me.gvRP.ColumnPositionChanged, AddressOf OngvColumnPositionChanged
		AddHandler Me.gvRP.ColumnWidthChanged, AddressOf OngvColumnPositionChanged

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

	Function GetDbSalaryData4Show() As IEnumerable(Of FoundedData)
		Dim result As List(Of FoundedData) = Nothing

		Dim sql As String

		sql = Sql2Open

		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ClsDataDetail.m_InitialData.MDData.MDDbConn, sql, Nothing)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of FoundedData)

				While reader.Read()
					Dim overviewData As New FoundedData

					overviewData.manr = m_utility.SafeGetInteger(reader, "manr", Nothing)

					overviewData.employeename = m_utility.SafeGetString(reader, "maname", Nothing)

					overviewData.ablp = m_utility.SafeGetInteger(reader, "ablp", Nothing)
					overviewData.bislp = m_utility.SafeGetInteger(reader, "bislp", Nothing)

					overviewData._7100 = m_utility.SafeGetDecimal(reader, "_7100", Nothing)
					overviewData._7110 = m_utility.SafeGetDecimal(reader, "_7110", Nothing)
					overviewData._7120 = m_utility.SafeGetDecimal(reader, "_7120", Nothing)
					overviewData._7220 = m_utility.SafeGetDecimal(reader, "_7220", Nothing)
					overviewData._7240 = m_utility.SafeGetDecimal(reader, "_7240", Nothing)

					overviewData.ahvnr = m_utility.SafeGetString(reader, "ahvnr", Nothing)

					overviewData.mageschlecht = m_utility.SafeGetString(reader, "mageschlecht", Nothing)
					overviewData.ahvgebdat = m_utility.SafeGetDateTime(reader, "ahvgebdat", Nothing)

					overviewData.filiale = m_utility.SafeGetString(reader, "filiale")
					overviewData.kst = m_utility.SafeGetString(reader, "kst")

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


	Private Function LoadFoundedSalaryList() As Boolean

		Dim listOfEmployees = GetDbSalaryData4Show()
		If listOfEmployees Is Nothing Then
			m_UtilityUi.ShowErrorDialog("Fehler in der Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
			Return False
		End If

		Dim responsiblePersonsGridData = (From person In listOfEmployees
		Select New FoundedData With
					 {.MDNr = person.MDNr,
						.manr = person.manr,
						.employeename = person.employeename,
						.ablp = person.ablp,
						.bislp = person.bislp,
						.jahr = person.jahr,
						._7100 = person._7100,
						._7110 = person._7110,
						._7120 = person._7120,
						._7220 = person._7220,
						._7240 = person._7240,
						.ahvnr = person.ahvnr,
						.kst = person.kst,
						.filiale = person.filiale,
						.mageschlecht = person.mageschlecht,
						.ahvgebdat = person.ahvgebdat
					 }).ToList()

		Dim listDataSource As BindingList(Of FoundedData) = New BindingList(Of FoundedData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdRP.DataSource = listDataSource

		Return Not listOfEmployees Is Nothing
	End Function

	Private Sub ResetGridSalaryData()

		gvRP.OptionsView.ShowIndicator = False
		gvRP.OptionsView.ShowAutoFilterRow = True
		gvRP.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvRP.OptionsView.ShowFooter = True

		gvRP.Columns.Clear()

		Dim columnmanr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnmanr.Caption = m_Translate.GetSafeTranslationValue("Kandidaten-Nr.")
		columnmanr.Name = "manr"
		columnmanr.FieldName = "manr"
		columnmanr.Visible = False
		columnmanr.BestFit()
		gvRP.Columns.Add(columnmanr)

		Dim columnemployeename As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeename.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemployeename.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
		columnemployeename.Name = "employeename"
		columnemployeename.FieldName = "employeename"
		columnemployeename.Visible = True
		columnemployeename.BestFit()
		gvRP.Columns.Add(columnemployeename)

		Dim columnabbislp As New DevExpress.XtraGrid.Columns.GridColumn()
		columnabbislp.Caption = m_Translate.GetSafeTranslationValue("Ab-Bis")
		columnabbislp.Name = "abbislp"
		columnabbislp.FieldName = "abbislp"
		columnabbislp.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
		columnabbislp.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
		columnabbislp.AppearanceHeader.Options.UseTextOptions = True
		columnabbislp.AppearanceCell.Options.UseTextOptions = True
		columnabbislp.Visible = True
		columnabbislp.BestFit()
		gvRP.Columns.Add(columnabbislp)


		Dim columnjahr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnjahr.Caption = m_Translate.GetSafeTranslationValue("Jahr")
		columnjahr.Name = "jahr"
		columnjahr.FieldName = "jahr"
		columnjahr.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnjahr.AppearanceHeader.Options.UseTextOptions = True
		columnjahr.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnjahr.Visible = False
		columnjahr.BestFit()
		gvRP.Columns.Add(columnjahr)


		Dim column_7100 As New DevExpress.XtraGrid.Columns.GridColumn()
		column_7100.Caption = m_Translate.GetSafeTranslationValue("AHV-Basis")
		column_7100.Name = "_7100"
		column_7100.FieldName = "_7100"
		column_7100.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		column_7100.AppearanceHeader.Options.UseTextOptions = True
		column_7100.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		column_7100.DisplayFormat.FormatString = "N2"
		column_7100.Visible = True
		column_7100.BestFit()
		column_7100.SummaryItem.DisplayFormat = "{0:n2}"
		column_7100.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		column_7100.SummaryItem.Tag = "Sum_7100"
		gvRP.Columns.Add(column_7100)


		Dim column_7110 As New DevExpress.XtraGrid.Columns.GridColumn()
		column_7110.Caption = m_Translate.GetSafeTranslationValue("AHV-Lohn")
		column_7110.Name = "_7110"
		column_7110.FieldName = "_7110"
		column_7110.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		column_7110.AppearanceHeader.Options.UseTextOptions = True
		column_7110.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		column_7110.DisplayFormat.FormatString = "N2"
		column_7110.Visible = True
		column_7110.BestFit()
		column_7110.SummaryItem.DisplayFormat = "{0:n2}"
		column_7110.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		column_7110.SummaryItem.Tag = "Sum_7110"
		gvRP.Columns.Add(column_7110)

		Dim column_7120 As New DevExpress.XtraGrid.Columns.GridColumn()
		column_7120.Caption = m_Translate.GetSafeTranslationValue("AHV-Freibetrag")
		column_7120.Name = "_7120"
		column_7120.FieldName = "_7120"
		column_7120.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		column_7120.AppearanceHeader.Options.UseTextOptions = True
		column_7120.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		column_7120.DisplayFormat.FormatString = "N2"
		column_7120.Visible = True
		column_7120.BestFit()
		column_7120.SummaryItem.DisplayFormat = "{0:n2}"
		column_7120.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		column_7120.SummaryItem.Tag = "Sum_7120"
		gvRP.Columns.Add(column_7120)

		Dim column_7220 As New DevExpress.XtraGrid.Columns.GridColumn()
		column_7220.Caption = m_Translate.GetSafeTranslationValue("ALV1-Lohn")
		column_7220.Name = "_7220"
		column_7220.FieldName = "_7220"
		column_7220.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		column_7220.AppearanceHeader.Options.UseTextOptions = True
		column_7220.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		column_7220.DisplayFormat.FormatString = "N2"
		column_7220.Visible = True
		column_7220.BestFit()
		column_7220.SummaryItem.DisplayFormat = "{0:n2}"
		column_7220.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		column_7220.SummaryItem.Tag = "Sum_7220"
		gvRP.Columns.Add(column_7220)

		Dim column_7240 As New DevExpress.XtraGrid.Columns.GridColumn()
		column_7240.Caption = m_Translate.GetSafeTranslationValue("ALV2-Lohn")
		column_7240.Name = "_7240"
		column_7240.FieldName = "_7240"
		column_7240.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		column_7240.AppearanceHeader.Options.UseTextOptions = True
		column_7240.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		column_7240.DisplayFormat.FormatString = "N2"
		column_7240.Visible = True
		column_7240.BestFit()
		column_7240.SummaryItem.DisplayFormat = "{0:n2}"
		column_7240.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		column_7240.SummaryItem.Tag = "Sum_7240"
		gvRP.Columns.Add(column_7240)

		Dim columnahvnr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnahvnr.Caption = m_Translate.GetSafeTranslationValue("AHV-Nr")
		columnahvnr.Name = "ahvnr"
		columnahvnr.FieldName = "ahvnr"
		columnahvnr.Visible = True
		columnahvnr.BestFit()
		gvRP.Columns.Add(columnahvnr)

		Dim columnmageschlecht As New DevExpress.XtraGrid.Columns.GridColumn()
		columnmageschlecht.Caption = m_Translate.GetSafeTranslationValue("Geschlecht")
		columnmageschlecht.Name = "mageschlecht"
		columnmageschlecht.FieldName = "mageschlecht"
		columnmageschlecht.Visible = False
		columnmageschlecht.BestFit()
		gvRP.Columns.Add(columnmageschlecht)

		Dim columnahvgebdat As New DevExpress.XtraGrid.Columns.GridColumn()
		columnahvgebdat.Caption = m_Translate.GetSafeTranslationValue("Geburtsdatum")
		columnahvgebdat.Name = "ahvgebdat"
		columnahvgebdat.FieldName = "ahvgebdat"
		columnahvgebdat.Visible = True
		columnahvgebdat.BestFit()
		gvRP.Columns.Add(columnahvgebdat)

		Dim columnfiliale As New DevExpress.XtraGrid.Columns.GridColumn()
		columnfiliale.Caption = m_Translate.GetSafeTranslationValue("Filiale")
		columnfiliale.Name = "filiale"
		columnfiliale.FieldName = "filiale"
		columnfiliale.Visible = False
		columnfiliale.BestFit()
		gvRP.Columns.Add(columnfiliale)


		RestoreGridLayoutFromXml()

		grdRP.DataSource = Nothing

	End Sub


#Region "Form Properties..."


	Private Sub OnFrmDisposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

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

	Sub StartTranslation()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)


		Catch ex As Exception
			m_Logger.LogError(String.Format("1=>{0}.{1}", strMethodeName, ex.ToString))
		End Try

	End Sub

	Private Sub OnFrmLoad(sender As Object, e As System.EventArgs) Handles Me.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim m_md As New Mandant

		Dim strStyleName As String = m_md.GetSelectedUILayoutName(ClsDataDetail.m_InitialData.MDData.MDNr, 0, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		StartTranslation()
		Try
			Me.Width = Math.Max(My.Settings.ifrmLVWidth, 100)
			Me.Height = Math.Max(My.Settings.ifrmLVHeight, 100)

			If My.Settings.frm_LVLocation <> String.Empty Then
				Dim aLoc As String() = My.Settings.frm_LVLocation.Split(CChar(";"))

				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = CStr(0)
				End If
				Me.Location = New System.Drawing.Point(CInt(Math.Max(Val(aLoc(0)), 0)), CInt(Math.Max(Val(aLoc(1)), 0)))
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.ToString))

		End Try

		Try

			LoadFoundedSalaryList()

			Me.RecCount = gvRP.RowCount
			Me.bsiInfo.Caption = String.Format("Anzahl Datensätze: {0}", Me.RecCount)

			AddHandler gvRP.RowCellClick, AddressOf Ongv_RowCellClick
			AddHandler Me.gvRP.ColumnFilterChanged, AddressOf OnGVDetail_ColumnFilterChanged
			AddHandler Me.gvRP.ColumnPositionChanged, AddressOf OngvColumnPositionChanged
			AddHandler Me.gvRP.ColumnWidthChanged, AddressOf OngvColumnPositionChanged
			AddHandler Me.gvRP.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground


		Catch ex As Exception

		End Try

	End Sub


#End Region

	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvRP.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, FoundedData)

				Select Case column.Name.ToLower
					Case "employeename", "manr"
						If viewData.manr.HasValue Then
							Dim hub = MessageService.Instance.Hub
							Dim openMng As New OpenEmployeeMngRequest(Me, m_InitialData.UserData.UserNr, m_InitialData.MDData.MDNr, viewData.manr)
							hub.Publish(openMng)
						End If

					Case Else
						' do nothing

				End Select

			End If

		End If

	End Sub

	Private Sub OnGV_CustomColumnDisplayText(ByVal sender As Object, ByVal e As CustomColumnDisplayTextEventArgs) Handles gvRP.CustomColumnDisplayText

		If e.Column.FieldName = "_7100" Or e.Column.FieldName = "_7110" Or e.Column.FieldName = "_7120" Or e.Column.FieldName = "_7220" Or e.Column.FieldName = "_7240" Then
			If e.Value = 0 Then e.DisplayText = String.Empty
		End If

	End Sub


	Private Sub OnGVDetail_ColumnFilterChanged(sender As Object, e As System.EventArgs)

		Me.bsiInfo.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True
		Me.RecCount = gvRP.RowCount
		Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), RecCount)

	End Sub


	Private Sub OngvColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvRP.OptionsView.ShowFooter = (gvRP.Columns("_7100").Visible OrElse gvRP.Columns("_7110").Visible OrElse gvRP.Columns("_7120").Visible OrElse
															 gvRP.Columns("_7220").Visible OrElse gvRP.Columns("_7240").Visible )
		gvRP.SaveLayoutToXml(m_GVSearchSettingfilename)

	End Sub

	Private Sub RestoreGridLayoutFromXml()
		Dim keepFilter = False
		Dim restoreLayout = True

		Try
			restoreLayout = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreLoYFAKSearchSetting, False), True)
			keepFilter = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingLoYFAKSearchFilter, False), False)

		Catch ex As Exception

		End Try
		If File.Exists(m_GVSearchSettingfilename) Then gvRP.RestoreLayoutFromXml(m_GVSearchSettingfilename)

		If restoreLayout AndAlso Not keepFilter Then gvRP.ActiveFilterCriteria = Nothing

	End Sub

	Private Sub CustomDrawEmptyForeground(ByVal sender As Object, ByVal e As CustomDrawEventArgs)
		Dim s As String = "Keine Daten sind vorhanden"

		Try
			s = m_Translate.GetSafeTranslationValue(s)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		Dim font As Font = New Font("Calibri", 8, FontStyle.Regular)
		Dim r As RectangleF = New RectangleF(e.Bounds.Left + 5, e.Bounds.Top + 5, e.Bounds.Width - 5, e.Bounds.Height - 5)
		e.Graphics.DrawString(s, font, Brushes.Black, r)

	End Sub


End Class
