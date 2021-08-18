

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
Imports SPLOKigaYSearch.ClsDataDetail

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

	Private Const MODUL_NAME_SETTING = "lojkigasearchlist"

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


					' jahr, anzsmaennner, anzsfrauen, anzamaenner, anzafrauen, totalsmaenner, totalsfrauen, totalamaenner, totalafrauen
					overviewData.jahr = m_utility.SafeGetInteger(reader, "Jahr", Nothing)

					overviewData.anzsmaenner = m_utility.SafeGetDecimal(reader, "anzsmaenner", Nothing)
					overviewData.anzsfrauen = m_utility.SafeGetDecimal(reader, "anzsfrauen", Nothing)

					overviewData.anzamaenner = m_utility.SafeGetDecimal(reader, "anzamaenner", Nothing)
					overviewData.anzafrauen = m_utility.SafeGetDecimal(reader, "anzafrauen", Nothing)

					overviewData.totalsmaenner = m_utility.SafeGetDecimal(reader, "totalsmaenner", Nothing)
					overviewData.totalsfrauen = m_utility.SafeGetDecimal(reader, "totalsfrauen", Nothing)

					overviewData.totalamaenner = m_utility.SafeGetDecimal(reader, "totalamaenner", Nothing)
					overviewData.totalafrauen = m_utility.SafeGetDecimal(reader, "totalafrauen", Nothing)


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
						.jahr = person.jahr,
						.anzsmaenner = person.anzsmaenner,
						.anzsfrauen = person.anzsfrauen,
						.anzamaenner = person.anzamaenner,
						.anzafrauen = person.anzafrauen,
						.totalsmaenner = person.totalsmaenner,
						.totalsfrauen = person.totalsfrauen,
						.totalamaenner = person.totalamaenner,
						.totalafrauen = person.totalafrauen
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

		gvRP.Columns.Clear()

		Dim columnMDNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMDNr.Caption = m_Translate.GetSafeTranslationValue("MDNr")
		columnMDNr.Name = "MDNr"
		columnMDNr.FieldName = "MDNr"
		columnMDNr.Visible = False
		columnMDNr.BestFit()
		gvRP.Columns.Add(columnMDNr)

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


		Dim columnjanuar As New DevExpress.XtraGrid.Columns.GridColumn()
		columnjanuar.Caption = m_Translate.GetSafeTranslationValue("Anzahl Schweizer")
		columnjanuar.Name = "anzsmaenner"
		columnjanuar.FieldName = "anzsmaenner"
		columnjanuar.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnjanuar.AppearanceHeader.Options.UseTextOptions = True
		columnjanuar.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnjanuar.DisplayFormat.FormatString = "N2"
		columnjanuar.Visible = True
		columnjanuar.BestFit()
		gvRP.Columns.Add(columnjanuar)

		Dim columnfebruar As New DevExpress.XtraGrid.Columns.GridColumn()
		columnfebruar.Caption = m_Translate.GetSafeTranslationValue("Anzahl Schweizerinnen")
		columnfebruar.Name = "anzsfrauen"
		columnfebruar.FieldName = "anzsfrauen"
		columnfebruar.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnfebruar.AppearanceHeader.Options.UseTextOptions = True
		columnfebruar.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnfebruar.DisplayFormat.FormatString = "N2"
		columnfebruar.Visible = True
		columnfebruar.BestFit()
		gvRP.Columns.Add(columnfebruar)

		Dim columnmaerz As New DevExpress.XtraGrid.Columns.GridColumn()
		columnmaerz.Caption = m_Translate.GetSafeTranslationValue("Anzahl Ausländer")
		columnmaerz.Name = "anzamaenner"
		columnmaerz.FieldName = "anzamaenner"
		columnmaerz.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnmaerz.AppearanceHeader.Options.UseTextOptions = True
		columnmaerz.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnmaerz.DisplayFormat.FormatString = "N2"
		columnmaerz.Visible = True
		columnmaerz.BestFit()
		gvRP.Columns.Add(columnmaerz)

		Dim columnapril As New DevExpress.XtraGrid.Columns.GridColumn()
		columnapril.Caption = m_Translate.GetSafeTranslationValue("Anzahl Ausländerinnen")
		columnapril.Name = "anzafrauen"
		columnapril.FieldName = "anzafrauen"
		columnapril.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnapril.AppearanceHeader.Options.UseTextOptions = True
		columnapril.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnapril.DisplayFormat.FormatString = "N2"
		columnapril.Visible = True
		columnapril.BestFit()
		gvRP.Columns.Add(columnapril)

		Dim columnmai As New DevExpress.XtraGrid.Columns.GridColumn()
		columnmai.Caption = m_Translate.GetSafeTranslationValue("Totalstunden Schweizer")
		columnmai.Name = "totalsmaenner"
		columnmai.FieldName = "totalsmaenner"
		columnmai.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnmai.AppearanceHeader.Options.UseTextOptions = True
		columnmai.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnmai.DisplayFormat.FormatString = "N2"
		columnmai.Visible = True
		columnmai.BestFit()
		gvRP.Columns.Add(columnmai)

		Dim columnjuni As New DevExpress.XtraGrid.Columns.GridColumn()
		columnjuni.Caption = m_Translate.GetSafeTranslationValue("Totalstunden Schweizerinnen")
		columnjuni.Name = "totalsfrauen"
		columnjuni.FieldName = "totalsfrauen"
		columnjuni.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnjuni.AppearanceHeader.Options.UseTextOptions = True
		columnjuni.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnjuni.DisplayFormat.FormatString = "N2"
		columnjuni.Visible = True
		columnjuni.BestFit()
		gvRP.Columns.Add(columnjuni)

		Dim columnjuli As New DevExpress.XtraGrid.Columns.GridColumn()
		columnjuli.Caption = m_Translate.GetSafeTranslationValue("Totalstunden Ausländer")
		columnjuli.Name = "totalamaenner"
		columnjuli.FieldName = "totalamaenner"
		columnjuli.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnjuli.AppearanceHeader.Options.UseTextOptions = True
		columnjuli.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnjuli.DisplayFormat.FormatString = "N2"
		columnjuli.Visible = True
		columnjuli.BestFit()
		gvRP.Columns.Add(columnjuli)

		Dim columnaugust As New DevExpress.XtraGrid.Columns.GridColumn()
		columnaugust.Caption = m_Translate.GetSafeTranslationValue("Totalstunden Ausländerinnen")
		columnaugust.Name = "totalafrauen"
		columnaugust.FieldName = "totalafrauen"
		columnaugust.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnaugust.AppearanceHeader.Options.UseTextOptions = True
		columnaugust.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnaugust.DisplayFormat.FormatString = "N2"
		columnaugust.Visible = True
		columnaugust.BestFit()
		gvRP.Columns.Add(columnaugust)


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

			AddHandler Me.gvRP.ColumnFilterChanged, AddressOf OnGVDetail_ColumnFilterChanged
			AddHandler Me.gvRP.ColumnPositionChanged, AddressOf OngvColumnPositionChanged
			AddHandler Me.gvRP.ColumnWidthChanged, AddressOf OngvColumnPositionChanged
			AddHandler Me.gvRP.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground


		Catch ex As Exception

		End Try

	End Sub


#End Region


	Private Sub OnGV_CustomColumnDisplayText(ByVal sender As Object, ByVal e As CustomColumnDisplayTextEventArgs) Handles gvRP.CustomColumnDisplayText

		If e.Column.FieldName = "anzsmaenner" Or e.Column.FieldName = "anzsfrauen" Or
			e.Column.FieldName = "anzamaenner" Or e.Column.FieldName = "anzafrauen" Or
			e.Column.FieldName = "totalsmaenner" Or e.Column.FieldName = "totalsfrauen" Or
			e.Column.FieldName = "totalamaenner" Or e.Column.FieldName = "totalafrauen" Then
			If e.Value = 0 Then e.DisplayText = String.Empty
		End If

	End Sub

	Private Sub OnGVDetail_ColumnFilterChanged(sender As Object, e As System.EventArgs)

		Me.bsiInfo.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True
		Me.RecCount = gvRP.RowCount
		Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), RecCount)

	End Sub


	Private Sub OngvColumnPositionChanged(sender As Object, e As System.EventArgs)

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
