

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
Imports SPFARListeSearch.ClsDataDetail

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

	Private Const MODUL_NAME_SETTING = "lofarsearchlist"

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

					' MANr, Nachname, Vorname, Gebdat, AHV_Nr, Ablp, BisLP, anzahlstd, Lohnsumme 
					overviewData.manr = m_utility.SafeGetInteger(reader, "manr", Nothing)

					overviewData.employeeLastname = m_utility.SafeGetString(reader, "nachname", Nothing)
					overviewData.employeeFirstname = m_utility.SafeGetString(reader, "Vorname", Nothing)

					overviewData.FirstESNr = m_utility.SafeGetInteger(reader, "FirstESNr", 0)
					overviewData.LastESNr = m_utility.SafeGetInteger(reader, "LastESNr", 0)
					overviewData.ablp = m_utility.SafeGetInteger(reader, "ablp", Nothing)
					overviewData.bislp = m_utility.SafeGetInteger(reader, "bislp", Nothing)
					overviewData.ES_Ab = m_utility.SafeGetDateTime(reader, "ES_Ab", Nothing)
					overviewData.ES_Ende = m_utility.SafeGetDateTime(reader, "ES_Ende", Nothing)

					overviewData.FirstESAs = m_utility.SafeGetString(reader, "FirstESAs")
					overviewData.LastESAs = m_utility.SafeGetString(reader, "LastESAs")
					overviewData.FirstGAVName = m_utility.SafeGetString(reader, "FirstGAVName")
					overviewData.LastGAVName = m_utility.SafeGetString(reader, "LastGAVName")

					overviewData.gebdat = m_utility.SafeGetDateTime(reader, "gebdat", Nothing)
					overviewData.lohnsumme = m_utility.SafeGetDecimal(reader, "lohnsumme", Nothing)
					overviewData.anzahlstd = m_utility.SafeGetDecimal(reader, "anzahlStd", Nothing)

					overviewData.ahvnr = m_utility.SafeGetString(reader, "ahv_nr")
					overviewData.ahvnrNew = m_utility.SafeGetString(reader, "AHV_Nr_New")

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
						.ES_Ab = person.ES_Ab,
						.FirstESNr = person.FirstESNr,
						.LastESNr = person.LastESNr,
						.ES_Ende = person.ES_Ende,
						.FirstESAs = person.FirstESAs,
						.LastESAs = person.LastESAs,
						.FirstGAVName = person.FirstGAVName,
						.LastGAVName = person.LastGAVName,
						.lohnsumme = person.lohnsumme,
						.anzahlstd = person.anzahlstd,
						.ahvnrNew = person.ahvnrNew,
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

		Dim columnFirstESNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFirstESNr.Caption = "1. " & m_Translate.GetSafeTranslationValue("Einsatz-Nr.")
		columnFirstESNr.Name = "FirstESNr"
		columnFirstESNr.FieldName = "FirstESNr"
		columnFirstESNr.Visible = False
		columnFirstESNr.BestFit()
		gvRP.Columns.Add(columnFirstESNr)

		Dim columnLastESNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLastESNr.Caption = "2. " & m_Translate.GetSafeTranslationValue("Einsatz-Nr.")
		columnLastESNr.Name = "LastESNr"
		columnLastESNr.FieldName = "LastESNr"
		columnLastESNr.Visible = False
		columnLastESNr.BestFit()
		gvRP.Columns.Add(columnLastESNr)

		Dim columnES_Ab As New DevExpress.XtraGrid.Columns.GridColumn()
		columnES_Ab.Caption = m_Translate.GetSafeTranslationValue("Einsatz-Ab")
		columnES_Ab.Name = "ES_Ab"
		columnES_Ab.FieldName = "ES_Ab"
		columnES_Ab.Visible = False
		columnES_Ab.BestFit()
		gvRP.Columns.Add(columnES_Ab)

		Dim columnES_Ende As New DevExpress.XtraGrid.Columns.GridColumn()
		columnES_Ende.Caption = m_Translate.GetSafeTranslationValue("Einsatz-Ende")
		columnES_Ende.Name = "ES_Ende"
		columnES_Ende.FieldName = "ES_Ende"
		columnES_Ende.Visible = False
		columnES_Ende.BestFit()
		gvRP.Columns.Add(columnES_Ende)

		Dim columnFirstESAs As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFirstESAs.Caption = "1. " & m_Translate.GetSafeTranslationValue("Einsatz-Als")
		columnFirstESAs.Name = "FirstESAs"
		columnFirstESAs.FieldName = "FirstESAs"
		columnFirstESAs.Visible = False
		columnFirstESAs.BestFit()
		gvRP.Columns.Add(columnFirstESAs)

		Dim columnLastESAs As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLastESAs.Caption = "2. " & m_Translate.GetSafeTranslationValue("Einsatz-Als")
		columnLastESAs.Name = "LastESAs"
		columnLastESAs.FieldName = "LastESAs"
		columnLastESAs.Visible = False
		columnLastESAs.BestFit()
		gvRP.Columns.Add(columnLastESAs)

		Dim columnFirstGAVName As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFirstGAVName.Caption = "1. " & m_Translate.GetSafeTranslationValue("GAV-Beruf")
		columnFirstGAVName.Name = "FirstGAVName"
		columnFirstGAVName.FieldName = "FirstGAVName"
		columnFirstGAVName.Visible = False
		columnFirstGAVName.BestFit()
		gvRP.Columns.Add(columnFirstGAVName)

		Dim columnLastGAVName As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLastGAVName.Caption = "2. " & m_Translate.GetSafeTranslationValue("GAV-Beruf")
		columnLastGAVName.Name = "LastGAVName"
		columnLastGAVName.FieldName = "LastGAVName"
		columnLastGAVName.Visible = False
		columnLastGAVName.BestFit()
		gvRP.Columns.Add(columnLastGAVName)

		Dim columnahvnr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnahvnr.Caption = m_Translate.GetSafeTranslationValue("AHV-Nr")
		columnahvnr.Name = "ahvnrNew"
		columnahvnr.FieldName = "ahvnrNew"
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

		If e.Column.FieldName = "lohnsumme" OrElse e.Column.FieldName = "anzahlstd" OrElse e.Column.FieldName = "FirstESNr" OrElse e.Column.FieldName = "LastESNr" Then
			If e.Value = 0 Then e.DisplayText = String.Empty
		End If

	End Sub

	Private Sub OnGVDetail_ColumnFilterChanged(sender As Object, e As System.EventArgs)

		Me.bsiInfo.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True
		Me.RecCount = gvRP.RowCount
		Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), RecCount)

	End Sub


	Private Sub OngvColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvRP.OptionsView.ShowFooter = (gvRP.Columns("lohnsumme").Visible OrElse gvRP.Columns("anzahlstd").Visible )
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
		If restoreLayout AndAlso File.Exists(m_GVSearchSettingfilename) Then gvRP.RestoreLayoutFromXml(m_GVSearchSettingfilename)
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
