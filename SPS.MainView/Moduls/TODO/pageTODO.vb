
Imports SPS.MainView.DataBaseAccess
Imports SPS.MainView.ModulConstants

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging

Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.DXperience.Demos.TutorialControlBase
Imports System.Data.SqlClient
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports System.Xml
Imports DevExpress.XtraEditors.Repository
Imports System.IO

Imports SPProgUtility.SPTranslation.ClsTranslation
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.LookAndFeel
Imports System.ComponentModel

Imports System.Runtime.Serialization
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings
Imports DevExpress.XtraRichEdit

Public Class pageTODO


#Region "private consts"

	Private Const MODUL_NAME = "TODO"
	Private Const MODUL_NAME_SETTING = "TODO"

	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/mainview/{1}/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/mainview/{1}/keepfilter"

	Private Const DATE_COLUMN_NAME As String = "magetdat;createdon;date;es_ab;es_ende;datum;fakdate;birthdate;aus_dat;paidon;krediton;printdate;reminder_1date;reminder_2date;reminder_3date;buchungdate"
	Private Const INTEGER_COLUMN_NAME As String = "vacancynumber;ofnr;vaknr;fopnr;manr;kdnr;zhdnr;esnr;rpnr;zgnr;lonr;renr;id;vgnr;monat;jahr"
	Private Const DECIMAL_COLUMN_NAME As String = "betragex;bezahlt;betragink;betragmwst;betragtotal;zebetrag;lobetrag;betrag;aus_dat"

#End Region


#Region "private fields"

	Private LiGridProperty As New Dictionary(Of String, String)
	Private aColFieldName As String()
	Private aColCaption As String()
	Private aColWidth As String()

	Private aPropertyColFieldName As String()
	Private aPropertyColCaption As String()

	Private m_md As Mandant
	Private m_utility As Utilities
	Private m_common As CommonSetting
	Private m_path As ClsProgPath
	Private m_translate As TranslateValues
	Private Shared m_Logger As ILogger = New Logger()

	Private m_griddata As GridData
	Private m_PropertyGriddata As PropertyGridData

	Private m_Setting As ClsFOPSetting

	Private m_UtilityUI As UtilityUI
	Private m_MainUtility As SP.Infrastructure.Utility

	Private m_GVMainSettingfilename As String
	Private m_xmlSettingMainFilter As String
	Private m_xmlSettingRestoreMainSetting As String
	Private m_MainViewGridData As SuportedCodes


#End Region


#Region "Private Consts"

	Private Const ProgHeaderName As String = "Sputnik Enterprise Suite - [{0}]"
	Private Const UsercontrolCaption As String = "TODO"

#End Region


#Region "Private property"


	Private ReadOnly Property SelectedRowViewData As FoundedFOPData
		Get
			Dim grdView = TryCast(grdMainTodo.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim contact = CType(grdView.GetRow(selectedRows(0)), FoundedFOPData)
					Return contact
				End If

			End If

			Return Nothing
		End Get

	End Property

#End Region


#Region "Contructor"

	Public Sub New()

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		Try
			Me.m_Setting = New ClsFOPSetting

			m_md = New Mandant
			m_utility = New Utilities
			m_common = New CommonSetting
			m_path = New ClsProgPath

			m_MainUtility = New SP.Infrastructure.Utility
			m_UtilityUI = New UtilityUI
			m_translate = New TranslateValues
			m_griddata = New GridData

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		Try
			m_MainViewGridData = New SuportedCodes
			m_MainViewGridData.ChangeColumnNamesToLowercase = False
			m_MainViewGridData.LoadMainModulGridPropertiesFromXML(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr, MODUL_NAME)
			m_griddata = m_MainViewGridData.LoadMainGridData

			ModulConstants.GridSettingPath = String.Format("{0}MainView\", m_md.GetGridSettingPath(ModulConstants.MDData.MDNr))

			m_GVMainSettingfilename = String.Format("{0}TODO\{1}{2}.xml", ModulConstants.GridSettingPath, gvMainTodo.Name, ModulConstants.UserData.UserNr)

			m_xmlSettingRestoreMainSetting = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_RESTORE, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)
			m_xmlSettingMainFilter = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_FILTER, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
		End Try

		AddHandler gvMainTodo.RowCellClick, AddressOf OngvMain_RowCellClick
		AddHandler gvMainTodo.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground

		AddHandler Me.gvMainTodo.ColumnPositionChanged, AddressOf OngvMainColumnPositionChanged
		AddHandler Me.gvMainTodo.ColumnWidthChanged, AddressOf OngvMainColumnPositionChanged


	End Sub

#End Region


#Region "public methods"

	Public Sub LoadXMLDataForSelectedModule()

		'm_griddata = GetUserGridPropertiesFromXML(ModulConstants.MDData.MDNr)
		'If m_griddata.SQLQuery Is Nothing Then
		'	m_griddata = GetGridPropertiesFromXML(ModulConstants.MDData.MDNr)
		'End If

		'aColFieldName = m_griddata.GridColFieldName.Split(CChar(";"))
		'aColCaption = m_griddata.GridColCaption.Split(CChar(";"))
		'aColWidth = m_griddata.GridColWidth.Split(CChar(";"))

		'aPropertyColFieldName = m_griddata.FieldsInHeaderToShow.Split(CChar(";"))
		'aPropertyColCaption = m_griddata.CaptionsInHeaderToShow.Split(CChar(";"))

		ResetMainGrid()

	End Sub

	Public Function LoadData() As Boolean
		Dim result As Boolean = True

		result = result AndAlso LoadFoundedData()

		Return result

	End Function

#End Region

	'Private Function GetGridPropertiesFromXML(ByVal iMDNr As Integer) As GridData
	'	Dim result As New GridData
	'	If iMDNr = 0 Then iMDNr = ModulConstants.MDData.MDNr
	'	m_path = New ClsProgPath

	'	Try
	'		Dim xDoc As XDocument = XDocument.Load(m_md.GetSelectedMDMainViewXMLFilename(ModulConstants.MDData.MDNr))
	'		Dim ConfigQuery = (From exportSetting In xDoc.Root.Elements("Modul")
	'						   Where Not (exportSetting.Attribute("ID") Is Nothing) _
	'								   And exportSetting.Attribute("ID").Value = String.Format("{0}", MODUL_NAME)
	'						   Select New With {
	'											   .SQLQuery = m_utility.GetSafeStringFromXElement(exportSetting.Element("SQL")),
	'											   .GridColFieldName = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColFieldName")),
	'											   .DisplayMember = m_utility.GetSafeStringFromXElement(exportSetting.Element("DisplayMember")),
	'											   .GridColCaption = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColCaption")),
	'											   .GridColWidth = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColWidth")),
	'											   .BackColor = m_utility.GetSafeStringFromXElement(exportSetting.Element("BackColor")),
	'											   .ForeColor = m_utility.GetSafeStringFromXElement(exportSetting.Element("ForeColor")),
	'											   .PopupFields = m_utility.GetSafeStringFromXElement(exportSetting.Element("PopupFieldName")),
	'											   .PopupCaptions = m_utility.GetSafeStringFromXElement(exportSetting.Element("PopupFieldCaption")),
	'											   .CountOfFieldsInHeader = m_utility.GetSafeStringFromXElement(exportSetting.Element("CountOfFieldsInHeaderToShow")),
	'											   .FieldsInHeaderToShow = m_utility.GetSafeStringFromXElement(exportSetting.Element("FieldsInHeaderToShow")),
	'											   .CaptionsInHeaderToShow = m_utility.GetSafeStringFromXElement(exportSetting.Element("CaptionInHeaderToShow"))
	'												   }).FirstOrDefault()



	'		result.SQLQuery = ConfigQuery.SQLQuery
	'		result.GridColFieldName = ConfigQuery.GridColFieldName
	'		result.DisplayMember = ConfigQuery.DisplayMember
	'		result.GridColCaption = ConfigQuery.GridColCaption
	'		result.GridColWidth = ConfigQuery.GridColWidth
	'		result.BackColor = ConfigQuery.BackColor
	'		result.ForeColor = ConfigQuery.ForeColor

	'		result.PopupFields = ConfigQuery.PopupFields
	'		result.PopupCaptions = ConfigQuery.PopupCaptions

	'		result.CountOfFieldsInHeader = CShort(ConfigQuery.CountOfFieldsInHeader)
	'		result.FieldsInHeaderToShow = ConfigQuery.FieldsInHeaderToShow
	'		result.CaptionsInHeaderToShow = ConfigQuery.CaptionsInHeaderToShow
	'		result.IsUserProperty = False

	'	Catch ex As Exception
	'		m_Logger.LogError(ex.ToString)

	'	End Try

	'	Return result
	'End Function

	'Private Function GetUserGridPropertiesFromXML(ByVal iMDNr As Integer) As GridData
	'	Dim result As New GridData
	'	If iMDNr = 0 Then iMDNr = ModulConstants.MDData.MDNr
	'	m_path = New ClsProgPath

	'	Try
	'		Dim xDoc As XDocument = XDocument.Load(m_md.GetSelectedMDUserProfileXMLFilename(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr))
	'		Dim ConfigQuery = (From exportSetting In xDoc.Root.Elements("Modul")
	'						   Where Not (exportSetting.Attribute("ID") Is Nothing) _
	'								   And exportSetting.Attribute("ID").Value = String.Format("{0}", MODUL_NAME)
	'						   Select New With {
	'											   .SQLQuery = m_utility.GetSafeStringFromXElement(exportSetting.Element("SQL")),
	'											   .GridColFieldName = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColFieldName")),
	'											   .DisplayMember = m_utility.GetSafeStringFromXElement(exportSetting.Element("DisplayMember")),
	'											   .GridColCaption = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColCaption")),
	'											   .GridColWidth = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColWidth")),
	'											   .BackColor = m_utility.GetSafeStringFromXElement(exportSetting.Element("BackColor")),
	'											   .ForeColor = m_utility.GetSafeStringFromXElement(exportSetting.Element("ForeColor")),
	'											   .PopupFields = m_utility.GetSafeStringFromXElement(exportSetting.Element("PopupFieldName")),
	'											   .PopupCaptions = m_utility.GetSafeStringFromXElement(exportSetting.Element("PopupFieldCaption")),
	'											   .CountOfFieldsInHeader = m_utility.GetSafeStringFromXElement(exportSetting.Element("CountOfFieldsInHeaderToShow")),
	'											   .FieldsInHeaderToShow = m_utility.GetSafeStringFromXElement(exportSetting.Element("FieldsInHeaderToShow")),
	'											   .CaptionsInHeaderToShow = m_utility.GetSafeStringFromXElement(exportSetting.Element("CaptionInHeaderToShow"))
	'												   }).FirstOrDefault()


	'		If Not ConfigQuery Is Nothing Then
	'			result.SQLQuery = ConfigQuery.SQLQuery
	'			result.GridColFieldName = ConfigQuery.GridColFieldName
	'			result.DisplayMember = ConfigQuery.DisplayMember
	'			result.GridColCaption = ConfigQuery.GridColCaption
	'			result.GridColWidth = ConfigQuery.GridColWidth
	'			result.BackColor = ConfigQuery.BackColor
	'			result.ForeColor = ConfigQuery.ForeColor

	'			result.PopupFields = ConfigQuery.PopupFields
	'			result.PopupCaptions = ConfigQuery.PopupCaptions

	'			Dim scountofitems As Short = Short.TryParse(ConfigQuery.CountOfFieldsInHeader, scountofitems)
	'			result.CountOfFieldsInHeader = Math.Max(scountofitems, 3)
	'			result.FieldsInHeaderToShow = ConfigQuery.FieldsInHeaderToShow
	'			result.CaptionsInHeaderToShow = ConfigQuery.CaptionsInHeaderToShow

	'			result.IsUserProperty = True

	'		End If


	'	Catch ex As Exception
	'		m_Logger.LogError(ex.ToString)

	'	End Try

	'	Return result
	'End Function

	Private Sub SetFormLayout()

		Me.Parent.Text = String.Format("{0}", m_translate.GetSafeTranslationValue(UsercontrolCaption))
		'bsiFilteredRecCount.Caption = String.Format("{0}, {1} | {1}", ModulConstants.MDData.MDName, ModulConstants.MDData.MDCity, ModulConstants.MDData.MDDbName)

		Try
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(ModulConstants.MDData.MDNr, m_md.GetDefaultUSNr, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If
		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		Me.gvMainTodo.OptionsView.ShowIndicator = False

	End Sub



	Private Sub ResetMainGrid()

		m_MainViewGridData.ResetMainGrid(gvMainTodo, grdMainTodo, MODUL_NAME)
		RestoreGridLayoutFromXml(gvMainTodo.Name.ToLower)

		Return

		gvMainTodo.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvMainTodo.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvMainTodo.OptionsView.ShowGroupPanel = False
		gvMainTodo.OptionsView.ShowIndicator = False
		gvMainTodo.OptionsView.ShowAutoFilterRow = False
		gvMainTodo.OptionsFilter.AllowFilterEditor = False
		gvMainTodo.OptionsView.ShowColumnHeaders = True

		gvMainTodo.Columns.Clear()

		Try
			Dim AutofilterconditionNumber = ModulConstants.MDData.AutoFilterConditionNumber
			Dim AutofilterconditionDate = ModulConstants.MDData.AutoFilterConditionDate

			Dim repoHTML = New RepositoryItemHypertextLabel
			repoHTML.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True
			repoHTML.Appearance.TextOptions.WordWrap = DevExpress.Utils.DefaultBoolean.True
			repoHTML.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
			repoHTML.Appearance.Options.UseTextOptions = True
			grdMainTodo.RepositoryItems.Add(repoHTML)

			Dim richHtml As RepositoryItemRichTextEdit = New RepositoryItemRichTextEdit
			richHtml.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.Default
			richHtml.DocumentFormat = DocumentFormat.Html
			grdMainTodo.RepositoryItems.Add(richHtml)


			For i = 0 To aColCaption.Length - 1

				Dim column As New DevExpress.XtraGrid.Columns.GridColumn()
				Dim columnCaption As String = m_translate.GetSafeTranslationValue(aColCaption(i).Trim)
				Dim columnName As String = aColFieldName(i).Trim 'ToLower.Trim

				column.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
				column.Caption = columnCaption
				column.Name = columnName
				column.FieldName = columnName
				If columnName = "body".ToLower Then
					Trace.WriteLine(columnName)
				End If
				If DATE_COLUMN_NAME.ToLower.Contains(columnName) Then
					column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
					column.DisplayFormat.FormatString = "d"
					column.OptionsFilter.AutoFilterCondition = AutofilterconditionDate

				ElseIf DECIMAL_COLUMN_NAME.ToLower.Contains(columnName) Then
					column.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
					column.AppearanceHeader.Options.UseTextOptions = True
					column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
					column.DisplayFormat.FormatString = "N2"
					column.OptionsFilter.AutoFilterCondition = AutofilterconditionNumber

				ElseIf INTEGER_COLUMN_NAME.ToLower.Contains(columnName) Then
					column.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
					column.AppearanceHeader.Options.UseTextOptions = True
					column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
					column.DisplayFormat.FormatString = "F0"
					column.OptionsFilter.AutoFilterCondition = AutofilterconditionNumber


				Else
					column.ColumnEdit = richHtml

				End If


				column.Visible = If(aColWidth(i) > 0, True, False)
				gvMainTodo.Columns.Add(column)

			Next

			RestoreGridLayoutFromXml(gvMainTodo.Name.ToLower)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdMainTodo.DataSource = Nothing

	End Sub

	Private Function LoadFoundedData() As Boolean
		Dim m_DataAccess As New MainGrid
		Dim listDataSource As BindingList(Of FoundedTODOData) = New BindingList(Of FoundedTODOData)
		'Dim listOfCustomerBill As BindingList(Of FoundedTODOData) = New BindingList(Of FoundedTODOData)


		Try

			Dim listOfCustomerBill = m_DataAccess.GetDbTODOData4Show(m_griddata.SQLQuery, ModulConstants.UserData.UserNr)
			If listOfCustomerBill Is Nothing Then Return False

			Dim responsiblePersonsGridData = (From person In listOfCustomerBill
											  Select New FoundedTODOData With
												  {.id = person.id,
												  .recnr = person.recnr,
												  .renr = person.renr,
												  .mdnr = person.mdnr,
												  .usnr = person.usnr,
												  .manr = person.manr,
												  .kdnr = person.kdnr,
												  .zhdnr = person.zhdnr,
												  .esnr = person.esnr,
												  .rpnr = person.rpnr,
												  .zenr = person.zenr,
												  .EmployeeLastname = person.EmployeeLastname,
												  .EmployeeFirstname = person.EmployeeFirstname,
												  .Customername = person.Customername,
												  .ZLastname = person.ZLastname,
												  .ZFirstname = person.ZFirstname,
												  .ProposeLabel = person.ProposeLabel,
												  .subject = person.subject,
												  .body = person.body,
												  .es_ab = person.es_ab,
												  .es_ende = person.es_ende,
												  .es_als = person.es_als,
												  .createdon = person.createdon,
												  .createdfrom = person.createdfrom,
												  .advisor = person.advisor,
												  .done = person.done,
												  .importand = person.importand,
												  .proposenr = person.proposenr,
												  .schedulebegins = person.schedulebegins,
												  .scheduleends = person.scheduleends,
												  .scheduleremember = person.scheduleremember,
												  .schedulerememberin = person.schedulerememberin
												  }).ToList()

			For Each p In responsiblePersonsGridData
				listDataSource.Add(p)
			Next

			grdMainTodo.DataSource = listDataSource

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

			Return Nothing
		End Try

		grpFunction.BackColor = If(Not String.IsNullOrWhiteSpace(ModulConstants.MDData.MandantColorName), System.Drawing.Color.FromName(ModulConstants.MDData.MandantColorName), Nothing)

		Return Not listDataSource Is Nothing
	End Function


	Private Sub frm_Load(sender As Object, e As System.EventArgs) Handles Me.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		SetFormLayout()

		LoadFoundedData()
		Me.bsiRecCount.Caption = String.Format(m_translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), Me.gvMainTodo.RowCount)

	End Sub

	Sub OngvMain_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvMainTodo.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, FoundedTODOData)

				Me.m_Setting.SelectedKDNr = viewData.kdnr
				Me.m_Setting.SelectedMANr = viewData.manr
				Me.m_Setting.SelectedESNr = viewData.esnr

				Select Case column.Name.ToLower
					Case "kdnr", "customername"
						If viewData.kdnr > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedKDNr = viewData.kdnr})
							_ClsKD.OpenSelectedCustomer(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)
						End If

					Case "manr", "employeename"
						If viewData.manr > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMANr = viewData.manr})
							_ClsKD.OpenSelectedEmployee(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)
						End If

					Case "esnr", "es_als"
						If viewData.esnr > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedESNr = viewData.esnr})
							_ClsKD.OpenSelectedES(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)
						End If

					Case "rpnr"
						If viewData.rpnr > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedRPNr = viewData.rpnr})
							_ClsKD.OpenSelectedReport(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)
						End If


					Case Else
						If viewData.id > 0 Then
							Dim _Clsre As New ClsOpenModul(New ClsSetting With {.SelectedTODONr = viewData.id})
							_Clsre.OpenTODOList()
						End If


				End Select

			End If

		End If

	End Sub

	Private Sub grpFunction_CustomButtonClick(sender As Object, e As DevExpress.XtraBars.Docking2010.BaseButtonEventArgs) Handles grpFunction.CustomButtonClick

		If e.Button.Properties.GroupIndex = 0 Then

			LoadXMLDataForSelectedModule()
			'ResetGrid()
			LoadFoundedData()

		End If

	End Sub


#Region "Helpers"

	Private Sub CustomDrawEmptyForeground(ByVal sender As Object, ByVal e As CustomDrawEventArgs)
		Dim s As String = "Keine Daten sind vorhanden"

		Try
			s = TranslateText(s)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		Dim font As Font = New Font("Calibri", 8, FontStyle.Regular)
		Dim r As RectangleF = New RectangleF(e.Bounds.Left + 5, e.Bounds.Top + 5, e.Bounds.Width - 5, e.Bounds.Height - 5)
		e.Graphics.DrawString(s, font, Brushes.Black, r)

	End Sub

#End Region


#Region "GridSettings"

	Private Sub RestoreGridLayoutFromXml(ByVal GridName As String)
		Dim keepFilter = False
		Dim restoreLayout = True

		Select Case GridName.ToLower
			Case "gvmaintodo".ToLower
				Try
					keepFilter = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingMainFilter), False)
					restoreLayout = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreMainSetting), True)

				Catch ex As Exception

				End Try

				If restoreLayout AndAlso File.Exists(m_GVMainSettingfilename) Then gvMainTodo.RestoreLayoutFromXml(m_GVMainSettingfilename)
				If restoreLayout AndAlso Not keepFilter Then gvMainTodo.ActiveFilterCriteria = Nothing

			Case Else

				Exit Sub


		End Select


	End Sub

	Private Sub OngvMainColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvMainTodo.SaveLayoutToXml(m_GVMainSettingfilename)

	End Sub


#End Region


End Class


