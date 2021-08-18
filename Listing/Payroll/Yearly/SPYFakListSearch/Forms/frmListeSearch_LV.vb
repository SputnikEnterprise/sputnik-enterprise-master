

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
Imports SPYFakListSearch.ClsDataDetail

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

	Private Const MODUL_NAME_SETTING = "yfaksearchlist"

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

					'Select Db.MANr, Db.Jahr, Db.USNr, Db._3600, Db._3602, Db._3650, Db._3700, Db._3750, Db._3800, Db._3850, Db._3900, Db._3900_1, Db._3901, Db._3901_1, DB.MDNr, MA.Nachname As MANachname, MA.Vorname As MAVorname, MA.Zivilstand, MA.GebDat, MA.AHV_Nr, MA.AHV_Nr_New From _KiAuZulage_1 Db Left Join Mitarbeiter MA On Db.MANr = MA.MANr Where DB.MDNr = 120 And Db.USNr = 1 And Db.Jahr = 2014  Order By MA.Nachname, MA.Vorname



					overviewData.MANr = CInt(m_utility.SafeGetInteger(reader, "MANr", Nothing))
					overviewData.jahr = m_utility.SafeGetInteger(reader, "Jahr", Nothing)

					overviewData.employeefirstname = m_utility.SafeGetString(reader, "MAVorname")
					overviewData.employeelastname = m_utility.SafeGetString(reader, "MANachname")

					overviewData._3600 = m_utility.SafeGetDecimal(reader, "_3600", Nothing)
					overviewData._3602 = m_utility.SafeGetDecimal(reader, "_3602", Nothing)

					overviewData._3650 = m_utility.SafeGetDecimal(reader, "_3650", Nothing)

					overviewData._3700 = m_utility.SafeGetDecimal(reader, "_3700", Nothing)
					overviewData._3750 = m_utility.SafeGetDecimal(reader, "_3750", Nothing)
					overviewData._3800 = m_utility.SafeGetDecimal(reader, "_3800", Nothing)
					overviewData._3850 = m_utility.SafeGetDecimal(reader, "_3850", Nothing)

					overviewData._3900 = m_utility.SafeGetDecimal(reader, "_3900", Nothing)
					overviewData._3900_1 = m_utility.SafeGetDecimal(reader, "_3900_1", Nothing)
					overviewData._3901_1 = m_utility.SafeGetDecimal(reader, "_3901_1", Nothing)

					overviewData.zivilstand = m_utility.SafeGetString(reader, "Zivilstand")
					overviewData.gebdat = m_utility.SafeGetDateTime(reader, "GebDat", Nothing)

					overviewData.ahv_nr = m_utility.SafeGetString(reader, "AHV_Nr")
					overviewData.ahv_nr_new = m_utility.SafeGetString(reader, "AHV_Nr_New")


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
						.ahv_nr = person.ahv_nr,
						.ahv_nr_new = person.ahv_nr_new,
						.gebdat = person.gebdat,
						._3600 = person._3600,
						._3650 = person._3650,
						._3602 = person._3602,
						._3700 = person._3700,
						._3750 = person._3750,
						._3800 = person._3800,
						._3850 = person._3850,
						._3900 = person._3900,
						._3900_1 = person._3900_1,
						._3901 = person._3901,
						._3901_1 = person._3901_1,
						.zivilstand = person.zivilstand,
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
		columnemployeelastname.Caption = m_Translate.GetSafeTranslationValue("Vorname")
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
		columnemployeefullname.BestFit()
		gvRP.Columns.Add(columnemployeefullname)


		Dim columnlaname As New DevExpress.XtraGrid.Columns.GridColumn()
		columnlaname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnlaname.Caption = m_Translate.GetSafeTranslationValue("Geburtsdatum")
		columnlaname.Name = "gebdat"
		columnlaname.FieldName = "gebdat"
		columnlaname.Visible = False
		columnlaname.BestFit()
		gvRP.Columns.Add(columnlaname)

		Dim columnlanr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnlanr.Caption = m_Translate.GetSafeTranslationValue("AHV-Nr")
		columnlanr.Name = "ahv_nr"
		columnlanr.FieldName = "ahv_nr"
		columnlanr.Visible = False
		columnlanr.BestFit()
		gvRP.Columns.Add(columnlanr)

		Dim columnahvnrnew As New DevExpress.XtraGrid.Columns.GridColumn()
		columnahvnrnew.Caption = m_Translate.GetSafeTranslationValue("AHV-Nr-Neu")
		columnahvnrnew.Name = "ahv_nr_new"
		columnahvnrnew.FieldName = "ahv_nr_new"
		columnahvnrnew.Visible = False
		columnahvnrnew.BestFit()
		gvRP.Columns.Add(columnahvnrnew)

		Dim columnzivilstand As New DevExpress.XtraGrid.Columns.GridColumn()
		columnzivilstand.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnzivilstand.Caption = m_Translate.GetSafeTranslationValue("Zivilstand")
		columnzivilstand.Name = "zivilstand"
		columnzivilstand.FieldName = "zivilstand"
		columnzivilstand.Visible = False
		columnzivilstand.BestFit()
		gvRP.Columns.Add(columnzivilstand)

		Dim column_3600 As New DevExpress.XtraGrid.Columns.GridColumn()
		column_3600.Caption = m_Translate.GetSafeTranslationValue("_3600")
		column_3600.Name = "_3600"
		column_3600.FieldName = "_3600"
		column_3600.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		column_3600.AppearanceHeader.Options.UseTextOptions = True
		column_3600.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		column_3600.DisplayFormat.FormatString = "N2"
		column_3600.Visible = False
		column_3600.BestFit()
		column_3600.SummaryItem.DisplayFormat = "{0:n2}"
		column_3600.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		column_3600.SummaryItem.Tag = "Sum_3600"
		gvRP.Columns.Add(column_3600)

		Dim column_3602 As New DevExpress.XtraGrid.Columns.GridColumn()
		column_3602.Caption = m_Translate.GetSafeTranslationValue("_3602")
		column_3602.Name = "_3602"
		column_3602.FieldName = "_3602"
		column_3602.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		column_3602.AppearanceHeader.Options.UseTextOptions = True
		column_3602.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		column_3602.DisplayFormat.FormatString = "N2"
		column_3602.Visible = False
		column_3602.BestFit()
		column_3602.SummaryItem.DisplayFormat = "{0:n2}"
		column_3602.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		column_3602.SummaryItem.Tag = "Sum_3602"
		gvRP.Columns.Add(column_3602)

		Dim column_3650 As New DevExpress.XtraGrid.Columns.GridColumn()
		column_3650.Caption = m_Translate.GetSafeTranslationValue("_3650")
		column_3650.Name = "_3650"
		column_3650.FieldName = "_3650"
		column_3650.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		column_3650.AppearanceHeader.Options.UseTextOptions = True
		column_3650.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		column_3650.DisplayFormat.FormatString = "N2"
		column_3650.Visible = False
		column_3650.BestFit()
		column_3650.SummaryItem.DisplayFormat = "{0:n2}"
		column_3650.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		column_3650.SummaryItem.Tag = "Sum_3650"
		gvRP.Columns.Add(column_3650)

		Dim column_3700 As New DevExpress.XtraGrid.Columns.GridColumn()
		column_3700.Caption = m_Translate.GetSafeTranslationValue("_3700")
		column_3700.Name = "_3700"
		column_3700.FieldName = "_3700"
		column_3700.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		column_3700.AppearanceHeader.Options.UseTextOptions = True
		column_3700.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		column_3700.DisplayFormat.FormatString = "N2"
		column_3700.Visible = False
		column_3700.BestFit()
		column_3700.SummaryItem.DisplayFormat = "{0:n2}"
		column_3700.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		column_3700.SummaryItem.Tag = "Sum_3700"
		gvRP.Columns.Add(column_3700)

		Dim column_3750 As New DevExpress.XtraGrid.Columns.GridColumn()
		column_3750.Caption = m_Translate.GetSafeTranslationValue("_3750")
		column_3750.Name = "_3750"
		column_3750.FieldName = "_3750"
		column_3750.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		column_3750.AppearanceHeader.Options.UseTextOptions = True
		column_3750.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		column_3750.DisplayFormat.FormatString = "N2"
		column_3750.Visible = True
		column_3750.BestFit()
		column_3750.SummaryItem.DisplayFormat = "{0:n2}"
		column_3750.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		column_3750.SummaryItem.Tag = "Sum_3750"
		gvRP.Columns.Add(column_3750)

		Dim column_3800 As New DevExpress.XtraGrid.Columns.GridColumn()
		column_3800.Caption = m_Translate.GetSafeTranslationValue("_3800")
		column_3800.Name = "_3800"
		column_3800.FieldName = "_3800"
		column_3800.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		column_3800.AppearanceHeader.Options.UseTextOptions = True
		column_3800.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		column_3800.Visible = False
		column_3800.BestFit()
		column_3800.SummaryItem.DisplayFormat = "{0:n2}"
		column_3800.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		column_3800.SummaryItem.Tag = "Sum_3800"
		gvRP.Columns.Add(column_3800)

		Dim column_3850 As New DevExpress.XtraGrid.Columns.GridColumn()
		column_3850.Caption = m_Translate.GetSafeTranslationValue("_3850")
		column_3850.Name = "_3850"
		column_3850.FieldName = "_3850"
		column_3850.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		column_3850.AppearanceHeader.Options.UseTextOptions = True
		column_3850.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		column_3850.DisplayFormat.FormatString = "N2"
		column_3850.Visible = False
		column_3850.BestFit()
		column_3850.SummaryItem.DisplayFormat = "{0:n2}"
		column_3850.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		column_3850.SummaryItem.Tag = "Sum_3850"
		gvRP.Columns.Add(column_3850)

		Dim column_3900 As New DevExpress.XtraGrid.Columns.GridColumn()
		column_3900.Caption = m_Translate.GetSafeTranslationValue("_3900")
		column_3900.Name = "_3900"
		column_3900.FieldName = "_3900"
		column_3900.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		column_3900.AppearanceHeader.Options.UseTextOptions = True
		column_3900.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		column_3900.DisplayFormat.FormatString = "N2"
		column_3900.Visible = False
		column_3900.BestFit()
		column_3900.SummaryItem.DisplayFormat = "{0:n2}"
		column_3900.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		column_3900.SummaryItem.Tag = "Sum_3900"
		gvRP.Columns.Add(column_3900)

		Dim column3901 As New DevExpress.XtraGrid.Columns.GridColumn()
		column3901.Caption = m_Translate.GetSafeTranslationValue("_3901")
		column3901.Name = "_3901"
		column3901.FieldName = "_3901"
		column3901.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		column3901.AppearanceHeader.Options.UseTextOptions = True
		column3901.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		column3901.DisplayFormat.FormatString = "N2"
		column3901.Visible = False
		column3901.BestFit()
		column3901.SummaryItem.DisplayFormat = "{0:n2}"
		column3901.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		column3901.SummaryItem.Tag = "Sum_3901"
		gvRP.Columns.Add(column3901)

		Dim column_3900_1 As New DevExpress.XtraGrid.Columns.GridColumn()
		column_3900_1.Caption = m_Translate.GetSafeTranslationValue("_3900_1")
		column_3900_1.Name = "_3900_1"
		column_3900_1.FieldName = "_3900_1"
		column_3900_1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		column_3900_1.AppearanceHeader.Options.UseTextOptions = True
		column_3900_1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		column_3900_1.DisplayFormat.FormatString = "N2"
		column_3900_1.Visible = False
		column_3900_1.BestFit()
		column_3900_1.SummaryItem.DisplayFormat = "{0:n2}"
		column_3900_1.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		column_3900_1.SummaryItem.Tag = "Sum_3900_1"
		gvRP.Columns.Add(column_3900_1)

		Dim colum_3901_1 As New DevExpress.XtraGrid.Columns.GridColumn()
		colum_3901_1.Caption = m_Translate.GetSafeTranslationValue("_3901_1")
		colum_3901_1.Name = "_3901_1"
		colum_3901_1.FieldName = "_3901_1"
		colum_3901_1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		colum_3901_1.AppearanceHeader.Options.UseTextOptions = True
		colum_3901_1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		colum_3901_1.DisplayFormat.FormatString = "N2"
		colum_3901_1.Visible = False
		colum_3901_1.BestFit()
		colum_3901_1.SummaryItem.DisplayFormat = "{0:n2}"
		colum_3901_1.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		colum_3901_1.SummaryItem.Tag = "Sum_3901_1"
		gvRP.Columns.Add(colum_3901_1)

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

	Private Sub OnGV_CustomColumnDisplayText(ByVal sender As Object, ByVal e As CustomColumnDisplayTextEventArgs) Handles gvRP.CustomColumnDisplayText

		If e.Column.FieldName = "_3600" Or e.Column.FieldName = "_3602" Or e.Column.FieldName = "_3650" Or
			e.Column.FieldName = "_3700" Or e.Column.FieldName = "_3750" Or
			e.Column.FieldName = "_3800" Or e.Column.FieldName = "_3850" Or
			e.Column.FieldName = "_3900" Or e.Column.FieldName = "_3901" Or
			e.Column.FieldName = "_3900_1" Or e.Column.FieldName = "_3901_1" Then
			If e.Value = 0 Then e.DisplayText = String.Empty
		End If

	End Sub

	Private Sub OngvColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvRP.OptionsView.ShowFooter = (gvRP.Columns("_3600").Visible OrElse gvRP.Columns("_3602").Visible OrElse gvRP.Columns("_3650").Visible OrElse
																	 gvRP.Columns("_3700").Visible OrElse gvRP.Columns("_3750").Visible OrElse
																	 gvRP.Columns("_3800").Visible OrElse gvRP.Columns("_3850").Visible OrElse
																	 gvRP.Columns("_3900").Visible OrElse
																	 gvRP.Columns("_3901").Visible OrElse
																	 gvRP.Columns("_3900_1").Visible OrElse
																	 gvRP.Columns("_3901_1").Visible)
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
