
Option Strict Off

Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities

Imports DevExpress.LookAndFeel
Imports System.ComponentModel
Imports System.IO
Imports SPProgUtility.CommonXmlUtility

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SPLOStdListSearch.ClsDataDetail

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports DevExpress.XtraGrid.Views.Base


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

	Private Property m_AnzStd As Decimal?



#Region "Private Consts"

	Private Const MODUL_NAME_SETTING = "lostdsearchlist"

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
		m_AnzStd = 0

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

					overviewData.MDNr = CInt(m_utility.SafeGetInteger(reader, "MDNr", Nothing))
					overviewData.LONr = CInt(m_utility.SafeGetInteger(reader, "LONr", Nothing))

					overviewData.MANr = CInt(m_utility.SafeGetInteger(reader, "MANr", Nothing))

					overviewData.employeefirstname = m_utility.SafeGetString(reader, "Vorname")
					overviewData.employeelastname = m_utility.SafeGetString(reader, "Nachname")

					overviewData.lanr = m_utility.SafeGetDecimal(reader, "lanr", Nothing)
					overviewData.lp = m_utility.SafeGetInteger(reader, "lp", Nothing)
					overviewData.jahr = m_utility.SafeGetInteger(reader, "Jahr", Nothing)

					overviewData.m_btr = m_utility.SafeGetDecimal(reader, "m_btr", Nothing)

					overviewData.gav_kanton = m_utility.SafeGetString(reader, "gav_kanton")
					overviewData.gav_beruf = m_utility.SafeGetString(reader, "gav_beruf")

					m_AnzStd += overviewData.m_btr


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
						.LONr = person.LONr,
						.MANr = person.MANr,
						.employeefirstname = person.employeefirstname,
						.employeelastname = person.employeelastname,
						.lanr = person.lanr,
						.lp = person.lp,
						.jahr = person.jahr,
						.m_btr = person.m_btr,
						.gav_kanton = person.gav_kanton,
						.gav_beruf = person.gav_beruf
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

		Dim columnMDNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMDNr.Caption = m_Translate.GetSafeTranslationValue("MDNr")
		columnMDNr.Name = "MDNr"
		columnMDNr.FieldName = "MDNr"
		columnMDNr.Visible = False
		columnMDNr.BestFit()
		gvRP.Columns.Add(columnMDNr)

		Dim columnLONr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLONr.Caption = m_Translate.GetSafeTranslationValue("Lohn-Nr.")
		columnLONr.Name = "LONr"
		columnLONr.FieldName = "LONr"
		columnLONr.Visible = False
		columnLONr.BestFit()
		gvRP.Columns.Add(columnLONr)



		Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMANr.Caption = m_Translate.GetSafeTranslationValue("MA-Nr.")
		columnMANr.Name = "MANr"
		columnMANr.FieldName = "MANr"
		columnMANr.Visible = False
		columnMANr.BestFit()
		gvRP.Columns.Add(columnMANr)

		Dim columnemployeefirstname As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeefirstname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemployeefirstname.Caption = m_Translate.GetSafeTranslationValue("Vorname")
		columnemployeefirstname.Name = "employeefirstname"
		columnemployeefirstname.FieldName = "employeefirstname"
		columnemployeefirstname.Visible = False
		columnemployeefirstname.BestFit()
		gvRP.Columns.Add(columnemployeefirstname)

		Dim columnemployeelastname As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeelastname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemployeelastname.Caption = m_Translate.GetSafeTranslationValue("Nachname")
		columnemployeelastname.Name = "employeelastname"
		columnemployeelastname.FieldName = "employeelastname"
		columnemployeelastname.Visible = False
		columnemployeelastname.BestFit()
		gvRP.Columns.Add(columnemployeelastname)

		Dim columnemployeefullname As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeefullname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemployeefullname.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
		columnemployeefullname.Name = "employeename"
		columnemployeefullname.FieldName = "employeename"
		columnemployeefullname.Visible = True
		columnemployeefullname.BestFit()
		gvRP.Columns.Add(columnemployeefullname)


		Dim columnlp As New DevExpress.XtraGrid.Columns.GridColumn()
		columnlp.Caption = m_Translate.GetSafeTranslationValue("Monat")
		columnlp.Name = "lp"
		columnlp.FieldName = "lp"
		columnlp.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnlp.AppearanceHeader.Options.UseTextOptions = True
		columnlp.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnlp.Visible = False
		columnlp.BestFit()
		gvRP.Columns.Add(columnlp)

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

		Dim columnm_btr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnm_btr.Caption = m_Translate.GetSafeTranslationValue("Anzahl Stunden")
		columnm_btr.Name = "m_btr"
		columnm_btr.FieldName = "m_btr"
		columnm_btr.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnm_btr.AppearanceHeader.Options.UseTextOptions = True
		columnm_btr.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnm_btr.DisplayFormat.FormatString = "N2"
		columnm_btr.Visible = True
		columnm_btr.BestFit()
		columnm_btr.SummaryItem.DisplayFormat = "{0:n2}"
		columnm_btr.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnm_btr.SummaryItem.Tag = "Summ_btr"
		gvRP.Columns.Add(columnm_btr)

		Dim columnlanr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnlanr.Caption = m_Translate.GetSafeTranslationValue("LANr")
		columnlanr.Name = "lanr"
		columnlanr.FieldName = "lanr"
		columnlanr.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnlanr.AppearanceHeader.Options.UseTextOptions = True
		columnlanr.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnlanr.DisplayFormat.FormatString = "F3"
		columnlanr.Visible = False
		columnlanr.BestFit()
		gvRP.Columns.Add(columnlanr)

		Dim columngav_kanton As New DevExpress.XtraGrid.Columns.GridColumn()
		columngav_kanton.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columngav_kanton.Caption = m_Translate.GetSafeTranslationValue("GAV-Kanton")
		columngav_kanton.Name = "gav_kanton"
		columngav_kanton.FieldName = "gav_kanton"
		columngav_kanton.Visible = False
		columngav_kanton.BestFit()
		gvRP.Columns.Add(columngav_kanton)

		Dim columngav_beruf As New DevExpress.XtraGrid.Columns.GridColumn()
		columngav_beruf.Caption = m_Translate.GetSafeTranslationValue("GAV-Beruf")
		columngav_beruf.Name = "gav_beruf"
		columngav_beruf.FieldName = "gav_beruf"
		columngav_beruf.Visible = False
		columngav_beruf.BestFit()
		gvRP.Columns.Add(columngav_beruf)


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


	Private Sub OnGV_CustomColumnDisplayText(ByVal sender As Object, ByVal e As CustomColumnDisplayTextEventArgs) Handles gvRP.CustomColumnDisplayText

		If e.Column.FieldName = "m_btr" Then
			If e.Value = 0 Then e.DisplayText = String.Empty
		End If

	End Sub


	Private Sub OnGVDetail_ColumnFilterChanged(sender As Object, e As System.EventArgs)

		Me.bsiInfo.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True
		Me.RecCount = gvRP.RowCount
		Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), RecCount)

	End Sub



	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvRP.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, FoundedData)

				Select Case column.Name.ToLower

					Case Else
						If viewData.MANr > 0 Then OpenSelectedEmployee(viewData.MANr)

				End Select

			End If

		End If

	End Sub


	Private Sub OngvColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvRP.OptionsView.ShowFooter = gvRP.Columns("m_btr").Visible
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
