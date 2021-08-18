Option Strict Off

Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities

Imports DevExpress.LookAndFeel
Imports System.ComponentModel
Imports System.IO
Imports SPProgUtility.CommonXmlUtility

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SPLOGUSearch.ClsDataDetail

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

	Private m_xmlSettingRestoreLoGUSearchSetting As String
	Private m_xmlSettingLoGUSearchFilter As String



#Region "Private Consts"

	Private Const MODUL_NAME_SETTING = "logusearchlist"

	Private Const USER_XML_SETTING_SPUTNIK_LOGU_SEARCH_LIST_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/lolisting/{1}/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_LOGU_SEARCH_LIST_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/lolisting/{1}/keepfilter"

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

			m_xmlSettingRestoreLoGUSearchSetting = String.Format(USER_XML_SETTING_SPUTNIK_LOGU_SEARCH_LIST_GRIDSETTING_RESTORE, ClsDataDetail.m_InitialData.UserData.UserNr, MODUL_NAME_SETTING)
			m_xmlSettingLoGUSearchFilter = String.Format(USER_XML_SETTING_SPUTNIK_LOGU_SEARCH_LIST_GRIDSETTING_FILTER, ClsDataDetail.m_InitialData.UserData.UserNr, MODUL_NAME_SETTING)

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
			' SELECT LANr, LALotext, Betrag FROM _Fremdleistungen_1 ORDER BY LANR

			If (Not reader Is Nothing) Then

				result = New List(Of FoundedData)

				While reader.Read()
					Dim overviewData As New FoundedData

					overviewData.MANr = CInt(m_utility.SafeGetDecimal(reader, "MANr", Nothing))

					overviewData.employeefirstname = m_utility.SafeGetString(reader, "MAVorname")
					overviewData.employeelastname = m_utility.SafeGetString(reader, "MANachname")
					overviewData.employeefullname = String.Format("{0}, {1}", m_utility.SafeGetString(reader, "MANachname"), m_utility.SafeGetString(reader, "MAVorname"))

					overviewData.g500 = m_utility.SafeGetDecimal(reader, "g500", Nothing)
					overviewData.g600 = m_utility.SafeGetDecimal(reader, "g600", Nothing)
					overviewData.g700 = m_utility.SafeGetDecimal(reader, "g700", Nothing)

					overviewData.g529 = m_utility.SafeGetDecimal(reader, "g529", Nothing)
					overviewData.g629 = m_utility.SafeGetDecimal(reader, "g629", Nothing)
					overviewData.g729 = m_utility.SafeGetDecimal(reader, "g729", Nothing)

					overviewData.gdar = m_utility.SafeGetDecimal(reader, "gDar", Nothing)
					overviewData.ggtotal = m_utility.SafeGetDecimal(reader, "ggtotal", Nothing)
					overviewData.ggtime = m_utility.SafeGetDecimal(reader, "ggtime", Nothing)

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
																							.employeefullname = person.employeefullname,
																							.g500 = person.g500,
																							.g600 = person.g600,
																							.g700 = person.g700,
																							.g529 = person.g529,
																							.g629 = person.g629,
																							.g729 = person.g729,
																							.gdar = person.gdar,
																							.ggtotal = person.ggtotal,
																							.ggtime = person.ggtime
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

		Dim columnLANr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLANr.Caption = m_Translate.GetSafeTranslationValue("MANr")
		columnLANr.Name = "MANr"
		columnLANr.FieldName = "MANr"
		columnLANr.Visible = False
		columnLANr.BestFit()
		gvRP.Columns.Add(columnLANr)

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
		columnemployeelastname.Caption = m_Translate.GetSafeTranslationValue("Vorname")
		columnemployeelastname.Name = "employeelastname"
		columnemployeelastname.FieldName = "employeelastname"
		columnemployeelastname.Visible = False
		columnemployeelastname.BestFit()
		gvRP.Columns.Add(columnemployeelastname)

		Dim columnemployeefullname As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeefullname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemployeefullname.Caption = m_Translate.GetSafeTranslationValue("Nach, Vorname")
		columnemployeefullname.Name = "employeefullname"
		columnemployeefullname.FieldName = "employeefullname"
		columnemployeefullname.Visible = True
		columnemployeefullname.BestFit()
		gvRP.Columns.Add(columnemployeefullname)

		Dim columnmg500 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnmg500.Caption = m_Translate.GetSafeTranslationValue("Feiertag")
		columnmg500.Name = "g500"
		columnmg500.FieldName = "g500"
		columnmg500.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnmg500.AppearanceHeader.Options.UseTextOptions = True
		columnmg500.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnmg500.DisplayFormat.FormatString = "N2"
		columnmg500.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnmg500.SummaryItem.DisplayFormat = "{0:n2}"
		columnmg500.Visible = False
		columnmg500.BestFit()
		gvRP.Columns.Add(columnmg500)

		Dim columnmg600 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnmg600.Caption = m_Translate.GetSafeTranslationValue("Ferien")
		columnmg600.Name = "g600"
		columnmg600.FieldName = "g600"
		columnmg600.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnmg600.AppearanceHeader.Options.UseTextOptions = True
		columnmg600.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnmg600.DisplayFormat.FormatString = "N2"
		columnmg600.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnmg600.SummaryItem.DisplayFormat = "{0:n2}"
		columnmg600.Visible = True
		columnmg600.BestFit()
		gvRP.Columns.Add(columnmg600)

		Dim columnmg700 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnmg700.Caption = m_Translate.GetSafeTranslationValue("13. Lohn")
		columnmg700.Name = "g700"
		columnmg700.FieldName = "g700"
		columnmg700.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnmg700.AppearanceHeader.Options.UseTextOptions = True
		columnmg700.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnmg700.DisplayFormat.FormatString = "N2"
		columnmg700.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnmg700.SummaryItem.DisplayFormat = "{0:n2}"
		columnmg700.Visible = False
		columnmg700.BestFit()
		gvRP.Columns.Add(columnmg700)

		Dim columnmg529 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnmg529.Caption = m_Translate.GetSafeTranslationValue("Feiertag (Altjahr)")
		columnmg529.Name = "g529"
		columnmg529.FieldName = "g529"
		columnmg529.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnmg529.AppearanceHeader.Options.UseTextOptions = True
		columnmg529.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnmg529.DisplayFormat.FormatString = "N2"
		columnmg529.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnmg529.SummaryItem.DisplayFormat = "{0:n2}"
		columnmg529.Visible = False
		columnmg529.BestFit()
		gvRP.Columns.Add(columnmg529)

		Dim columnmg629 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnmg629.Caption = m_Translate.GetSafeTranslationValue("Ferien (Altjahr)")
		columnmg629.Name = "g629"
		columnmg629.FieldName = "g629"
		columnmg629.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnmg629.AppearanceHeader.Options.UseTextOptions = True
		columnmg629.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnmg629.DisplayFormat.FormatString = "N2"
		columnmg629.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnmg629.SummaryItem.DisplayFormat = "{0:n2}"
		columnmg629.Visible = False
		columnmg629.BestFit()
		gvRP.Columns.Add(columnmg629)

		Dim columnmg729 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnmg729.Caption = m_Translate.GetSafeTranslationValue("13. Lohn (Altjahr)")
		columnmg729.Name = "g729"
		columnmg729.FieldName = "g729"
		columnmg729.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnmg729.AppearanceHeader.Options.UseTextOptions = True
		columnmg729.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnmg729.DisplayFormat.FormatString = "N2"
		columnmg729.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnmg729.SummaryItem.DisplayFormat = "{0:n2}"
		columnmg729.Visible = False
		columnmg729.BestFit()
		gvRP.Columns.Add(columnmg729)

		Dim columngdar As New DevExpress.XtraGrid.Columns.GridColumn()
		columngdar.Caption = m_Translate.GetSafeTranslationValue("Darlehen")
		columngdar.Name = "gdar"
		columngdar.FieldName = "gdar"
		columngdar.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columngdar.AppearanceHeader.Options.UseTextOptions = True
		columngdar.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columngdar.DisplayFormat.FormatString = "N2"
		columngdar.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columngdar.SummaryItem.DisplayFormat = "{0:n2}"
		columngdar.Visible = False
		columngdar.BestFit()
		gvRP.Columns.Add(columngdar)


		Dim columnggtotal As New DevExpress.XtraGrid.Columns.GridColumn()
		columnggtotal.Caption = m_Translate.GetSafeTranslationValue("Gleitzeit-Betrag")
		columnggtotal.Name = "ggtotal"
		columnggtotal.FieldName = "ggtotal"
		columnggtotal.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnggtotal.AppearanceHeader.Options.UseTextOptions = True
		columnggtotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnggtotal.DisplayFormat.FormatString = "N2"
		columnggtotal.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnggtotal.SummaryItem.DisplayFormat = "{0:n2}"
		columnggtotal.Visible = True
		columnggtotal.BestFit()
		gvRP.Columns.Add(columnggtotal)


		Dim columnggtime As New DevExpress.XtraGrid.Columns.GridColumn()
		columnggtime.Caption = m_Translate.GetSafeTranslationValue("Gleitzeit-Stundenanzahl")
		columnggtime.Name = "ggtime"
		columnggtime.FieldName = "ggtime"
		columnggtime.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnggtime.AppearanceHeader.Options.UseTextOptions = True
		columnggtime.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnggtime.DisplayFormat.FormatString = "N2"
		columnggtime.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnggtime.SummaryItem.DisplayFormat = "{0:n2}"
		columnggtime.Visible = False
		columnggtime.BestFit()
		gvRP.Columns.Add(columnggtime)


		RestoreGridLayoutFromXml()

		grdRP.DataSource = Nothing

	End Sub


#Region "Form Properties..."


	Private Sub frmSearchKD_LV_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

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

	Private Sub frmOnLoad(sender As Object, e As System.EventArgs) Handles Me.Load
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

	Private Sub OnGvCurrentList_CustomColumnDisplayText(ByVal sender As Object, ByVal e As CustomColumnDisplayTextEventArgs) Handles gvRP.CustomColumnDisplayText

		If e.Column.FieldName = "g500" OrElse e.Column.FieldName = "g600" OrElse e.Column.FieldName = "g700" OrElse
			e.Column.FieldName = "g529" OrElse e.Column.FieldName = "g629" OrElse e.Column.FieldName = "g729" OrElse
			e.Column.FieldName = "gdar" OrElse e.Column.FieldName = "ggtotal" OrElse e.Column.FieldName = "ggtime" Then
			If Val(e.Value) = 0 Then e.DisplayText = String.Empty
		End If

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

		gvRP.SaveLayoutToXml(m_GVSearchSettingfilename)

	End Sub

	Private Sub RestoreGridLayoutFromXml()
		Dim keepFilter = False
		Dim restoreLayout = True

		Try
			restoreLayout = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreLoGUSearchSetting, False), True)
			keepFilter = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingLoGUSearchFilter, False), False)

		Catch ex As Exception

		End Try

		If restoreLayout AndAlso Not keepFilter Then gvRP.ActiveFilterCriteria = Nothing

	End Sub

	Sub OpenSelectedEmployee(ByVal Employeenumber As Integer)

		Try
			Dim hub = MessageService.Instance.Hub
			Dim openMng As New OpenEmployeeMngRequest(Me, m_InitialData.UserData.UserNr, m_InitialData.MDData.MDNr, Employeenumber)
			hub.Publish(openMng)

			'Dim frm As frmEmployees = CType(ClsDataDetail.GetModuleCach.GetModuleForm(ClsDataDetail.m_InitialData.MDData.MDNr, SP.ModuleCaching.ModuleName.EmployeeMng), frmEmployees)
			'frm.LoadEmployeeData(Employeenumber)

			'If frm.IsEmployeeDataLoaded Then
			'	frm.Show()
			'	frm.BringToFront()
			'End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
		End Try

	End Sub

End Class
