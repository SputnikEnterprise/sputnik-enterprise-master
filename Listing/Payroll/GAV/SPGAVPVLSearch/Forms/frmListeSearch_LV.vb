

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
Imports SPGAVPVLSearch.ClsDataDetail

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

	Private Const MODUL_NAME_SETTING = "lopvlsearchlist"

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
		If String.IsNullOrWhiteSpace(sql) Then Return result
		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ClsDataDetail.m_InitialData.MDData.MDDbConn, sql, Nothing)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of FoundedData)

				While reader.Read()
					Dim overviewData As New FoundedData

					' MANr, Nachname, Vorname, GebDat, AHV_Nr_New, AnzahlStd, BeitragLohnAN, BeitragLohnAG, Lohnsumme, EinsatzAls, Ablp, BisLp
					overviewData.manr = m_utility.SafeGetInteger(reader, "manr", Nothing)

					overviewData.employeeLastname = m_utility.SafeGetString(reader, "nachname", Nothing)
					overviewData.employeeFirstname = m_utility.SafeGetString(reader, "Vorname", Nothing)

					overviewData.ablp = m_utility.SafeGetInteger(reader, "ablp", Nothing)
					overviewData.bislp = m_utility.SafeGetInteger(reader, "bislp", Nothing)
					overviewData.gebdat = m_utility.SafeGetDateTime(reader, "gebdat", Nothing)

					overviewData.beitraglohnan = m_utility.SafeGetDecimal(reader, "BeitragLohnAN", Nothing)
					overviewData.beitraglohnag = m_utility.SafeGetDecimal(reader, "BeitragLohnAG", Nothing)
					overviewData.lohnsumme = m_utility.SafeGetDecimal(reader, "lohnsumme", Nothing)
					overviewData.lohnsumme = m_utility.SafeGetDecimal(reader, "lohnsumme", Nothing)
					overviewData.anzahlstd = m_utility.SafeGetDecimal(reader, "anzahlStd", Nothing)
					overviewData.einsatzals = m_utility.SafeGetString(reader, "EinsatzAls", Nothing)

					overviewData.ahvnr = m_utility.SafeGetString(reader, "ahv_nr", Nothing)

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
						.employeeLastname = person.employeeLastname,
						.employeeFirstname = person.employeeFirstname,
						.gebdat = person.gebdat,
						.ablp = person.ablp,
						.bislp = person.bislp,
						.beitraglohnan = person.beitraglohnan,
						.beitraglohnag = person.beitraglohnag,
						.lohnsumme = person.lohnsumme,
						.anzahlstd = person.anzahlstd,
						.einsatzals = person.einsatzals,
						.ahvnr = person.ahvnr
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


		Dim columnanzahlstd As New DevExpress.XtraGrid.Columns.GridColumn()
		columnanzahlstd.Caption = m_Translate.GetSafeTranslationValue("Anzahl Stunden")
		columnanzahlstd.Name = "anzahlstd"
		columnanzahlstd.FieldName = "anzahlstd"
		columnanzahlstd.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnanzahlstd.AppearanceHeader.Options.UseTextOptions = True
		columnanzahlstd.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnanzahlstd.DisplayFormat.FormatString = "N2"
		columnanzahlstd.Visible = True
		columnanzahlstd.BestFit()
		columnanzahlstd.SummaryItem.DisplayFormat = "{0:n2}"
		columnanzahlstd.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnanzahlstd.SummaryItem.Tag = "Sumanzahlstd"
		gvRP.Columns.Add(columnanzahlstd)

		Dim columnbeitraglohnan As New DevExpress.XtraGrid.Columns.GridColumn()
		columnbeitraglohnan.Caption = m_Translate.GetSafeTranslationValue("Beitrag AN")
		columnbeitraglohnan.Name = "beitraglohnan"
		columnbeitraglohnan.FieldName = "beitraglohnan"
		columnbeitraglohnan.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnbeitraglohnan.AppearanceHeader.Options.UseTextOptions = True
		columnbeitraglohnan.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnbeitraglohnan.DisplayFormat.FormatString = "N2"
		columnbeitraglohnan.Visible = True
		columnbeitraglohnan.BestFit()
		columnbeitraglohnan.SummaryItem.DisplayFormat = "{0:n2}"
		columnbeitraglohnan.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnbeitraglohnan.SummaryItem.Tag = "Sumbeitraglohnan"
		gvRP.Columns.Add(columnbeitraglohnan)

		Dim columnbeitraglohnag As New DevExpress.XtraGrid.Columns.GridColumn()
		columnbeitraglohnag.Caption = m_Translate.GetSafeTranslationValue("Beitrag AG")
		columnbeitraglohnag.Name = "beitraglohnag"
		columnbeitraglohnag.FieldName = "beitraglohnag"
		columnbeitraglohnag.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnbeitraglohnag.AppearanceHeader.Options.UseTextOptions = True
		columnbeitraglohnag.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnbeitraglohnag.DisplayFormat.FormatString = "N2"
		columnbeitraglohnag.Visible = True
		columnbeitraglohnag.BestFit()
		columnbeitraglohnag.SummaryItem.DisplayFormat = "{0:n2}"
		columnbeitraglohnag.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnbeitraglohnag.SummaryItem.Tag = "Sumbeitraglohnag"
		gvRP.Columns.Add(columnbeitraglohnag)

		Dim columnlohnsumme As New DevExpress.XtraGrid.Columns.GridColumn()
		columnlohnsumme.Caption = m_Translate.GetSafeTranslationValue("Lohnsumme")
		columnlohnsumme.Name = "lohnsumme"
		columnlohnsumme.FieldName = "lohnsumme"
		columnlohnsumme.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnlohnsumme.AppearanceHeader.Options.UseTextOptions = True
		columnlohnsumme.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnlohnsumme.DisplayFormat.FormatString = "N2"
		columnlohnsumme.Visible = True
		columnlohnsumme.BestFit()
		columnlohnsumme.SummaryItem.DisplayFormat = "{0:n2}"
		columnlohnsumme.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnlohnsumme.SummaryItem.Tag = "Sumlohnsumme"
		gvRP.Columns.Add(columnlohnsumme)


		Dim columngebdat As New DevExpress.XtraGrid.Columns.GridColumn()
		columngebdat.Caption = m_Translate.GetSafeTranslationValue("Geburtsdatum")
		columngebdat.Name = "gebdat"
		columngebdat.FieldName = "gebdat"
		columngebdat.Visible = True
		columngebdat.BestFit()
		gvRP.Columns.Add(columngebdat)

		Dim columnahvnr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnahvnr.Caption = m_Translate.GetSafeTranslationValue("AHV-Nr")
		columnahvnr.Name = "ahvnr"
		columnahvnr.FieldName = "ahvnr"
		columnahvnr.Visible = True
		columnahvnr.BestFit()
		gvRP.Columns.Add(columnahvnr)


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
						If viewData.manr.HasValue Then OpenSelectedEmployee(viewData.manr)


					Case Else
						' do nothing

				End Select

			End If

		End If

	End Sub

	Private Sub OnGV_CustomColumnDisplayText(ByVal sender As Object, ByVal e As CustomColumnDisplayTextEventArgs) Handles gvRP.CustomColumnDisplayText

		If e.Column.FieldName = "lohnsumme" Or e.Column.FieldName = "anzahlstd" Or e.Column.FieldName = "beitraglohnan" Or e.Column.FieldName = "beitraglohnag" Then
			If e.Value = 0 Then e.DisplayText = String.Empty
		End If

	End Sub

	Private Sub OnGVDetail_ColumnFilterChanged(sender As Object, e As System.EventArgs)

		Me.bsiInfo.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True
		Me.RecCount = gvRP.RowCount
		Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), RecCount)

	End Sub


	Private Sub OngvColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvRP.OptionsView.ShowFooter = (gvRP.Columns("lohnsumme").Visible OrElse gvRP.Columns("anzahlstd").Visible OrElse gvRP.Columns("beitraglohnan").Visible OrElse
											 gvRP.Columns("beitraglohnag").Visible)
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

	Sub OpenSelectedEmployee(ByVal Employeenumber As Integer)

		Try
			Dim hub = MessageService.Instance.Hub
			Dim openMng As New OpenEmployeeMngRequest(Me, m_InitialData.UserData.UserNr, m_InitialData.MDData.MDNr, Employeenumber)
			hub.Publish(openMng)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
		End Try

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
