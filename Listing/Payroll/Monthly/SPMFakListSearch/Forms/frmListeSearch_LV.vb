

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
Imports SPMFakListSearch.ClsDataDetail

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



#Region "Private Consts"

	Private Const MODUL_NAME_SETTING = "mfaksearchlist"

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

					'Select LOL.LONr, LOL.MANr, LOL.LP, LOL.LANr, LOL.m_Anz, LOL.m_Bas, LOL.m_Bas, LOL.m_Ans, LOL.m_Btr, LOL.Jahr, LOL.S_Kanton, LOL.RPText, MA.Nachname As MANachname, MA.Vorname As MAVorname, (Select Count(*) From MA_KIAddress Where MANr = MA.MANr) As MAKIAnz From LOL Left Join Mitarbeiter MA On LOL.MANr = MA.MANr  Where  LOL.Lanr In (3600, 3602, 3650, 3700, 3750, 3800, 3850, 3900, 3901) And LOL.LP like 4 And LOL.Jahr In (2014) And  LOL.m_Btr <> 0  Order By MA.Nachname ASC, MA.Vorname ASC, LOL.LP ASC



					overviewData.MANr = CInt(m_utility.SafeGetInteger(reader, "MANr", Nothing))
					overviewData.monat = m_utility.SafeGetInteger(reader, "LP", Nothing)
					overviewData.jahr = m_utility.SafeGetInteger(reader, "Jahr", Nothing)

					overviewData.employeefirstname = m_utility.SafeGetString(reader, "MAVorname")
					overviewData.employeelastname = m_utility.SafeGetString(reader, "MANachname")

					overviewData.LANr = m_utility.SafeGetDecimal(reader, "LANr", Nothing)
					overviewData.m_Bas = m_utility.SafeGetDecimal(reader, "m_Bas", Nothing)
					overviewData.m_Ans = m_utility.SafeGetDecimal(reader, "m_Ans", Nothing)
					overviewData.m_Btr = m_utility.SafeGetDecimal(reader, "m_Btr", Nothing)
					overviewData.MAKIAnz = m_utility.SafeGetInteger(reader, "MAKIAnz", Nothing)

					'overviewData._3850 = m_utility.SafeGetDecimal(reader, "_3850", Nothing)
					'overviewData._3900 = m_utility.SafeGetDecimal(reader, "_3900", Nothing)
					'overviewData._3900_1 = m_utility.SafeGetDecimal(reader, "_3900_1", Nothing)
					'overviewData._3901_1 = m_utility.SafeGetDecimal(reader, "_3901_1", Nothing)

					overviewData.S_Kanton = m_utility.SafeGetString(reader, "S_Kanton")
					overviewData.RPText = m_utility.SafeGetString(reader, "RPText")

					'overviewData.ahv_nr_new = m_utility.SafeGetString(reader, "AHV_Nr_New")
					'overviewData.gebdat = m_utility.SafeGetDateTime(reader, "GebDat", Nothing)


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

		Dim responsiblePersonsGridData = (From person In listOfEmployees
		Select New FoundedData With
					 {.MANr = person.MANr,
						.employeefirstname = person.employeefirstname,
						.employeelastname = person.employeelastname,
						.RPText = person.RPText,
						.S_Kanton = person.S_Kanton,
						.LANr = person.LANr,
						.MAKIAnz = person.MAKIAnz,
						.m_Ans = person.m_Ans,
						.m_Bas = person.m_Bas,
						.m_Btr = person.m_Btr,
						.monat = person.monat,
						.jahr = person.jahr
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

		Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMANr.Caption = m_Translate.GetSafeTranslationValue("MANr")
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
		columnemployeefullname.Caption = m_Translate.GetSafeTranslationValue("Nach, Vorname")
		columnemployeefullname.Name = "LastnameFirstname"
		columnemployeefullname.FieldName = "LastnameFirstname"
		columnemployeefullname.Visible = True
		columnemployeefullname.Width = 500
		gvRP.Columns.Add(columnemployeefullname)


		Dim columnRPText As New DevExpress.XtraGrid.Columns.GridColumn()
		columnRPText.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
		columnRPText.Name = "RPText"
		columnRPText.FieldName = "RPText"
		columnRPText.Visible = True
		columnRPText.Width = 500
		gvRP.Columns.Add(columnRPText)


		Dim columnS_Kanton As New DevExpress.XtraGrid.Columns.GridColumn()
		columnS_Kanton.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnS_Kanton.Caption = m_Translate.GetSafeTranslationValue("Kanton")
		columnS_Kanton.Name = "S_Kanton"
		columnS_Kanton.FieldName = "S_Kanton"
		columnS_Kanton.Visible = False
		columnS_Kanton.Width = 50
		gvRP.Columns.Add(columnS_Kanton)

		Dim columnm_Ans As New DevExpress.XtraGrid.Columns.GridColumn()
		columnm_Ans.Caption = m_Translate.GetSafeTranslationValue("Ansatz")
		columnm_Ans.Name = "m_Ans"
		columnm_Ans.FieldName = "m_Ans"
		columnm_Ans.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnm_Ans.AppearanceHeader.Options.UseTextOptions = True
		columnm_Ans.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnm_Ans.DisplayFormat.FormatString = "N2"
		columnm_Ans.Visible = False
		columnm_Ans.BestFit()
		columnm_Ans.SummaryItem.DisplayFormat = "{0:n2}"
		gvRP.Columns.Add(columnm_Ans)

		Dim columnm_Bas As New DevExpress.XtraGrid.Columns.GridColumn()
		columnm_Bas.Caption = m_Translate.GetSafeTranslationValue("Basis")
		columnm_Bas.Name = "m_Bas"
		columnm_Bas.FieldName = "m_Bas"
		columnm_Bas.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnm_Bas.AppearanceHeader.Options.UseTextOptions = True
		columnm_Bas.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnm_Bas.DisplayFormat.FormatString = "N2"
		columnm_Bas.Visible = False
		columnm_Bas.BestFit()
		columnm_Bas.SummaryItem.DisplayFormat = "{0:n2}"
		gvRP.Columns.Add(columnm_Bas)

		Dim columnm_Btr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnm_Btr.Caption = m_Translate.GetSafeTranslationValue("Betrag")
		columnm_Btr.Name = "m_Btr"
		columnm_Btr.FieldName = "m_Btr"
		columnm_Btr.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnm_Btr.AppearanceHeader.Options.UseTextOptions = True
		columnm_Btr.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnm_Btr.DisplayFormat.FormatString = "N2"
		columnm_Btr.Visible = True
		columnm_Btr.Width = 100
		columnm_Btr.SummaryItem.DisplayFormat = "{0:n2}"
		columnm_Btr.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnm_Btr.SummaryItem.Tag = "Sum_m_Btr"
		gvRP.Columns.Add(columnm_Btr)


		Dim columnMAKIAnz As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMAKIAnz.Caption = m_Translate.GetSafeTranslationValue("Anzahlkinder")
		columnMAKIAnz.Name = "MAKIAnz"
		columnMAKIAnz.FieldName = "MAKIAnz"
		columnMAKIAnz.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnMAKIAnz.AppearanceHeader.Options.UseTextOptions = True
		columnMAKIAnz.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnMAKIAnz.DisplayFormat.FormatString = "N0"
		columnMAKIAnz.Visible = False
		columnMAKIAnz.BestFit()
		columnMAKIAnz.SummaryItem.DisplayFormat = "{0:n0}"
		gvRP.Columns.Add(columnMAKIAnz)

		Dim column_LANr As New DevExpress.XtraGrid.Columns.GridColumn()
		column_LANr.Caption = m_Translate.GetSafeTranslationValue("Lohnart")
		column_LANr.Name = "LANr"
		column_LANr.FieldName = "LANr"
		column_LANr.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		column_LANr.AppearanceHeader.Options.UseTextOptions = True
		column_LANr.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		column_LANr.DisplayFormat.FormatString = "F3"
		column_LANr.Visible = True
		column_LANr.BestFit()
		column_LANr.SummaryItem.DisplayFormat = "{0:f3}"
		gvRP.Columns.Add(column_LANr)

		Dim columnmonat As New DevExpress.XtraGrid.Columns.GridColumn()
		columnmonat.Caption = m_Translate.GetSafeTranslationValue("Monat")
		columnmonat.Name = "monat"
		columnmonat.FieldName = "monat"
		columnmonat.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnmonat.AppearanceHeader.Options.UseTextOptions = True
		columnmonat.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnmonat.Visible = False
		columnmonat.BestFit()
		gvRP.Columns.Add(columnmonat)

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


		RestoreGridLayoutFromXml()
		gvRP.OptionsView.ShowFooter = gvRP.Columns("m_Btr").Visible

		grdRP.DataSource = Nothing

	End Sub


#Region "Form Properties..."


	Private Sub Onfrm_LV_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

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

	Private Sub OnfrmOnLoad(sender As Object, e As System.EventArgs) Handles Me.Load
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

		Catch ex As Exception

		End Try

	End Sub


#End Region


	Private Sub OnGVDetail_ColumnFilterChanged(sender As Object, e As System.EventArgs)

		Me.bsiInfo.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True
		Me.RecCount = gvRP.RowCount
		Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), RecCount)

		OngvColumnPositionChanged(sender, New System.EventArgs)

	End Sub

	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvRP.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, FoundedData)

				Select Case column.Name.ToLower
					Case column.Name.ToLower.Contains("name".ToLower)
						If viewData.MANr > 0 Then OpenSelectedEmployee(viewData.MANr)

					Case Else
						Return

				End Select

			End If

		End If

	End Sub

	Private Sub OnGV_CustomColumnDisplayText(ByVal sender As Object, ByVal e As CustomColumnDisplayTextEventArgs) Handles gvRP.CustomColumnDisplayText

		If e.Column.FieldName = "MAKIAnz" Or e.Column.FieldName = "m_Ans" Or e.Column.FieldName = "m_Bas" Or
			e.Column.FieldName = "m_Btr"  Then
			If e.Value = 0 Then e.DisplayText = String.Empty
		End If

	End Sub

	Private Sub OngvColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvRP.OptionsView.ShowFooter = gvRP.Columns("m_Btr").Visible
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

End Class
