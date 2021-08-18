
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities

Imports DevExpress.LookAndFeel
Imports System.IO
Imports SPProgUtility.CommonXmlUtility

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SPLOLKontiSearch.ClsDataDetail

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SP.DatabaseAccess.Listing.DataObjects
Imports DevExpress.XtraGrid

Public Class frmListeSearch_LV

	Protected Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private m_md As Mandant
	Private m_utility As Utilities
	Private m_UtilityUi As SP.Infrastructure.UI.UtilityUI

	Public Property RecCount As Integer
	Private Property Sql2Open As String
	Private Property Modul2Open As String

	Private m_GridSettingPath As String
	Private UserGridSettingsXml As SettingsXml

	Private m_GVSearchSettingfilename As String

	Private m_xmlSettingRestoreLoLOHNKONTISearchSetting As String
	Private m_xmlSettingLoLOHNKONTISearchFilter As String



#Region "Private Consts"

	Private Const MODUL_NAME_SETTING = "lokontisearchlist"

	Private Const USER_XML_SETTING_SPUTNIK_LOLOHNKONTI_SEARCH_LIST_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/lolisting/{1}/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_LOLOHNKONTI_SEARCH_LIST_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/lolisting/{1}/keepfilter"

#End Region


#Region "public properties"

	Public Property LohnKontiData As IEnumerable(Of ListingPayrollLohnkontiData)

#End Region


#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_md = New Mandant
		m_utility = New Utilities
		m_UtilityUi = New UtilityUI

		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		Dim strStyleName As String = m_md.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, 0, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		InitializeComponent()

		Try
			m_GridSettingPath = String.Format("{0}{1}\", m_md.GetGridSettingPath(m_InitializationData.MDData.MDNr), MODUL_NAME_SETTING)

			If Not Directory.Exists(m_GridSettingPath) Then Directory.CreateDirectory(m_GridSettingPath)

			m_GVSearchSettingfilename = String.Format("{0}{1}{2}.xml", m_GridSettingPath, Me.grdRP.Name, m_InitializationData.UserData.UserNr)

			m_xmlSettingRestoreLoLOHNKONTISearchSetting = String.Format(USER_XML_SETTING_SPUTNIK_LOLOHNKONTI_SEARCH_LIST_GRIDSETTING_RESTORE, m_InitializationData.UserData.UserNr, MODUL_NAME_SETTING)
			m_xmlSettingLoLOHNKONTISearchFilter = String.Format(USER_XML_SETTING_SPUTNIK_LOLOHNKONTI_SEARCH_LIST_GRIDSETTING_FILTER, m_InitializationData.UserData.UserNr, MODUL_NAME_SETTING)

			UserGridSettingsXml = New SettingsXml(m_md.GetAllUserGridSettingXMLFilename(m_InitializationData.MDData.MDNr))

		Catch ex As Exception

		End Try

		ResetGridSalaryData()
		TranslateControls()

		AddHandler gvRP.RowCellClick, AddressOf Ongv_RowCellClick
		AddHandler gvRP.ColumnFilterChanged, AddressOf OnGVDetail_ColumnFilterChanged
		AddHandler gvRP.ColumnPositionChanged, AddressOf OngvColumnPositionChanged
		AddHandler gvRP.ColumnWidthChanged, AddressOf OngvColumnPositionChanged

	End Sub

#End Region


	''' <summary>
	''' Gets the selected employee.
	''' </summary>
	''' <returns>The selected employee or nothing if none is selected.</returns>
	Public ReadOnly Property SelectedRecord As ListingPayrollLohnkontiData
		Get
			Dim gvRP = TryCast(grdRP.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvRP Is Nothing) Then

				Dim selectedRows = gvRP.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim employee = CType(gvRP.GetRow(selectedRows(0)), ListingPayrollLohnkontiData)
					Return employee
				End If

			End If

			Return Nothing
		End Get

	End Property

	Public Function LoadFoundedSalaryList() As Boolean

		Dim listOfEmployees = LohnKontiData

		Dim gridData = (From person In listOfEmployees
										Select New ListingPayrollLohnkontiData With
			 {.MANr = person.MANr,
				.AHVNumber = person.AHVNumber,
				.EmployeeLastname = person.EmployeeLastname,
				.EmployeeFirstname = person.EmployeeFirstname,
				.GebDat = person.GebDat,
				.SatrtOfEmployment = person.SatrtOfEmployment,
				.EndOfEmployment = person.EndOfEmployment,
				.Januar = person.Januar,
				.Februar = person.Februar,
				.Maerz = person.Maerz,
				.April = person.April,
				.Mai = person.Mai,
				.Juni = person.Juni,
				.Juli = person.Juli,
				.August = person.August,
				.September = person.September,
				.Oktober = person.Oktober,
				.November = person.November,
				.Dezember = person.Dezember,
				.Kumulativ = person.Kumulativ,
				.LALabel = person.LALabel
			 }).ToList()

		grdRP.DataSource = listOfEmployees

		Me.RecCount = gvRP.RowCount
		Me.bsiInfo.Caption = String.Format("Anzahl Datensätze: {0}", Me.RecCount)

		Return Not listOfEmployees Is Nothing
	End Function


	Sub TranslateControls()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)

		Catch ex As Exception
			m_Logger.LogError(String.Format("1=>{0}.{1}", strMethodeName, ex.ToString))
		End Try

	End Sub

	Private Sub ResetGridSalaryData()

		gvRP.OptionsView.ShowIndicator = False
		gvRP.OptionsView.ShowAutoFilterRow = True
		gvRP.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvRP.OptionsSelection.EnableAppearanceFocusedRow = False
		gvRP.OptionsView.ShowFooter = True

		gvRP.Columns.Clear()

		Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMANr.Caption = m_Translate.GetSafeTranslationValue("MANr")
		columnMANr.Name = "MANr"
		columnMANr.FieldName = "MANr"
		columnMANr.Visible = False
		columnMANr.BestFit()
		gvRP.Columns.Add(columnMANr)

		Dim columnEmployeeFullname As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeeFullname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnEmployeeFullname.Caption = m_Translate.GetSafeTranslationValue("Nach, Vorname")
		columnEmployeeFullname.Name = "EmployeeFullname"
		columnEmployeeFullname.FieldName = "EmployeeFullname"
		columnEmployeeFullname.Visible = True
		columnEmployeeFullname.BestFit()
		gvRP.Columns.Add(columnEmployeeFullname)

		Dim columnlanr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnlanr.Caption = m_Translate.GetSafeTranslationValue("LANr")
		columnlanr.Name = "LANr"
		columnlanr.FieldName = "LANr"
		columnlanr.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnlanr.AppearanceHeader.Options.UseTextOptions = True
		columnlanr.DisplayFormat.FormatString = "N3"
		columnlanr.Visible = True
		columnlanr.BestFit()
		gvRP.Columns.Add(columnlanr)

		Dim columnLALabel As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLALabel.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnLALabel.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
		columnLALabel.Name = "LALabel"
		columnLALabel.FieldName = "LALabel"
		columnLALabel.Visible = True
		columnLALabel.BestFit()
		gvRP.Columns.Add(columnLALabel)

		Dim columnjanuar As New DevExpress.XtraGrid.Columns.GridColumn()
		columnjanuar.Caption = m_Translate.GetSafeTranslationValue("Januar")
		columnjanuar.Name = "Januar"
		columnjanuar.FieldName = "Januar"
		columnjanuar.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnjanuar.AppearanceHeader.Options.UseTextOptions = True
		columnjanuar.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnjanuar.DisplayFormat.FormatString = "N2"
		columnjanuar.Visible = False
		columnjanuar.BestFit()
		gvRP.Columns.Add(columnjanuar)

		Dim columnfebruar As New DevExpress.XtraGrid.Columns.GridColumn()
		columnfebruar.Caption = m_Translate.GetSafeTranslationValue("Februar")
		columnfebruar.Name = "Februar"
		columnfebruar.FieldName = "Februar"
		columnfebruar.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnfebruar.AppearanceHeader.Options.UseTextOptions = True
		columnfebruar.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnfebruar.DisplayFormat.FormatString = "N2"
		columnfebruar.Visible = False
		columnfebruar.BestFit()
		gvRP.Columns.Add(columnfebruar)

		Dim columnmgmarch As New DevExpress.XtraGrid.Columns.GridColumn()
		columnmgmarch.Caption = m_Translate.GetSafeTranslationValue("März")
		columnmgmarch.Name = "Maerz"
		columnmgmarch.FieldName = "Maerz"
		columnmgmarch.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnmgmarch.AppearanceHeader.Options.UseTextOptions = True
		columnmgmarch.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnmgmarch.DisplayFormat.FormatString = "N2"
		columnmgmarch.Visible = False
		columnmgmarch.BestFit()
		gvRP.Columns.Add(columnmgmarch)

		Dim columnapril As New DevExpress.XtraGrid.Columns.GridColumn()
		columnapril.Caption = m_Translate.GetSafeTranslationValue("April")
		columnapril.Name = "April"
		columnapril.FieldName = "April"
		columnapril.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnapril.AppearanceHeader.Options.UseTextOptions = True
		columnapril.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnapril.DisplayFormat.FormatString = "N2"
		columnapril.Visible = False
		columnapril.BestFit()
		gvRP.Columns.Add(columnapril)

		Dim columnmai As New DevExpress.XtraGrid.Columns.GridColumn()
		columnmai.Caption = m_Translate.GetSafeTranslationValue("Mai")
		columnmai.Name = "Mai"
		columnmai.FieldName = "Mai"
		columnmai.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnmai.AppearanceHeader.Options.UseTextOptions = True
		columnmai.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnmai.DisplayFormat.FormatString = "N2"
		columnmai.Visible = False
		columnmai.BestFit()
		gvRP.Columns.Add(columnmai)

		Dim columnjuni As New DevExpress.XtraGrid.Columns.GridColumn()
		columnjuni.Caption = m_Translate.GetSafeTranslationValue("Juni")
		columnjuni.Name = "Juni"
		columnjuni.FieldName = "Juni"
		columnjuni.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnjuni.AppearanceHeader.Options.UseTextOptions = True
		columnjuni.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnjuni.DisplayFormat.FormatString = "N2"
		columnjuni.Visible = False
		columnjuni.BestFit()
		gvRP.Columns.Add(columnjuni)

		Dim columnjuli As New DevExpress.XtraGrid.Columns.GridColumn()
		columnjuli.Caption = m_Translate.GetSafeTranslationValue("Juli")
		columnjuli.Name = "juli"
		columnjuli.FieldName = "juli"
		columnjuli.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnjuli.AppearanceHeader.Options.UseTextOptions = True
		columnjuli.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnjuli.DisplayFormat.FormatString = "N2"
		columnjuli.Visible = False
		columnjuli.BestFit()
		gvRP.Columns.Add(columnjuli)


		Dim columnaugust As New DevExpress.XtraGrid.Columns.GridColumn()
		columnaugust.Caption = m_Translate.GetSafeTranslationValue("August")
		columnaugust.Name = "August"
		columnaugust.FieldName = "August"
		columnaugust.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnaugust.AppearanceHeader.Options.UseTextOptions = True
		columnaugust.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnaugust.DisplayFormat.FormatString = "N2"
		columnaugust.Visible = False
		columnaugust.BestFit()
		gvRP.Columns.Add(columnaugust)


		Dim columnseptember As New DevExpress.XtraGrid.Columns.GridColumn()
		columnseptember.Caption = m_Translate.GetSafeTranslationValue("September")
		columnseptember.Name = "September"
		columnseptember.FieldName = "September"
		columnseptember.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnseptember.AppearanceHeader.Options.UseTextOptions = True
		columnseptember.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnseptember.DisplayFormat.FormatString = "N2"
		columnseptember.Visible = False
		columnseptember.BestFit()
		gvRP.Columns.Add(columnseptember)

		Dim columnoktober As New DevExpress.XtraGrid.Columns.GridColumn()
		columnoktober.Caption = m_Translate.GetSafeTranslationValue("Oktober")
		columnoktober.Name = "Oktober"
		columnoktober.FieldName = "Oktober"
		columnoktober.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnoktober.AppearanceHeader.Options.UseTextOptions = True
		columnoktober.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnoktober.DisplayFormat.FormatString = "N2"
		columnoktober.Visible = False
		columnoktober.BestFit()
		gvRP.Columns.Add(columnoktober)

		Dim columnnovember As New DevExpress.XtraGrid.Columns.GridColumn()
		columnnovember.Caption = m_Translate.GetSafeTranslationValue("November")
		columnnovember.Name = "November"
		columnnovember.FieldName = "November"
		columnnovember.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnnovember.AppearanceHeader.Options.UseTextOptions = True
		columnnovember.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnnovember.DisplayFormat.FormatString = "N2"
		columnnovember.Visible = False
		columnnovember.BestFit()
		gvRP.Columns.Add(columnnovember)

		Dim columndezember As New DevExpress.XtraGrid.Columns.GridColumn()
		columndezember.Caption = m_Translate.GetSafeTranslationValue("Dezember")
		columndezember.Name = "Dezember"
		columndezember.FieldName = "Dezember"
		columndezember.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columndezember.AppearanceHeader.Options.UseTextOptions = True
		columndezember.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columndezember.DisplayFormat.FormatString = "N2"
		columndezember.Visible = False
		columndezember.BestFit()
		gvRP.Columns.Add(columndezember)

		Dim columnkumulativ As New DevExpress.XtraGrid.Columns.GridColumn()
		columnkumulativ.Caption = m_Translate.GetSafeTranslationValue("Total")
		columnkumulativ.Name = "Kumulativ"
		columnkumulativ.FieldName = "Kumulativ"
		columnkumulativ.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnkumulativ.AppearanceHeader.Options.UseTextOptions = True
		columnkumulativ.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnkumulativ.DisplayFormat.FormatString = "N2"
		columnkumulativ.SummaryItem.DisplayFormat = "{0:n2}"
		columnkumulativ.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnkumulativ.SummaryItem.Tag = "SumEachKumulativ"
		columnkumulativ.Visible = true
		columnkumulativ.BestFit()
		gvRP.Columns.Add(columnkumulativ)

		Dim columnahvnew As New DevExpress.XtraGrid.Columns.GridColumn()
		columnahvnew.Caption = m_Translate.GetSafeTranslationValue("AHV-Nr")
		columnahvnew.Name = "AHVNumber"
		columnahvnew.FieldName = "AHVNumber"
		columnahvnew.Visible = False
		columnahvnew.BestFit()
		gvRP.Columns.Add(columnahvnew)

		Dim grpAuthorizedItems = New GridGroupSummaryItem()
		grpAuthorizedItems.FieldName = "TotalAmountOfHours"
		grpAuthorizedItems.SummaryType = DevExpress.Data.SummaryItemType.Count
		grpAuthorizedItems.DisplayFormat = m_Translate.GetSafeTranslationValue("") & ": {0:F0}"
		gvRP.GroupFormat = "{1}: [Anzahl Lohnarten]: {2}"
		gvRP.GroupSummary.Add(grpAuthorizedItems)


		gvRP.BeginSort()
		Try
			gvRP.ClearGrouping()
			gvRP.Columns("EmployeeFullname").GroupIndex = 0
			'gvRP.Columns("Category").GroupIndex = 1
		Finally
			gvRP.EndSort()
		End Try


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

	Private Sub frmOnLoad(sender As Object, e As System.EventArgs) Handles Me.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

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
			m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.ToString))

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
				Dim viewData = CType(dataRow, ListingPayrollLohnkontiData)

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
			restoreLayout = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreLoLOHNKONTISearchSetting, False), True)
			keepFilter = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingLoLOHNKONTISearchFilter, False), False)

		Catch ex As Exception

		End Try

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
