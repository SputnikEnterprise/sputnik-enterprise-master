
Imports SPS.MainView.DataBaseAccess
Imports SPS.MainView.ModulConstants

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging

Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings

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

Imports SP.Infrastructure.Settings
Imports SPS.MainView.CustomerSettings
Imports System.ComponentModel
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports DevExpress.XtraSplashScreen


Public Class pgOffers

	Private m_communicationHub As TinyMessenger.ITinyMessengerHub
	Private Property LoadedMDNr As Integer

	Protected m_SettingsManager As ISettingsManager
	Private m_GVMainSettingfilename As String

	Private aColCaption As String()
	Private aColFieldName As String()
	Private aColWidth As String()

	Private m_md As Mandant
	Private m_utility As Utilities
	Private m_common As CommonSetting
	Private m_path As ClsProgPath
	Private m_translate As TranslateValues

	Private Shared m_Logger As ILogger = New Logger()
	Private m_griddata As GridData

	Dim _iSelectedRecNr As Integer

	Dim _OSetting As ClsOfferSetting

	Private m_xmlSettingMainFilter As String
	Private m_xmlSettingRestoreMainSetting As String
	Private m_AllowedChangeMandant As Boolean
	Private m_MainViewGridData As SuportedCodes

#Region "private consts"

	Private Const MODUL_NAME = "Massen-Versand"
	Private Const MODUL_NAME_SETTING = "offers"
	Private Const ProgHeaderName As String = "Sputnik Enterprise Suite - [{0}]"
	Private Const UsercontrolCaption As String = "Massenversand"

	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/mainview/{1}/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/mainview/{1}/keepfilter"

	Private Const DATE_COLUMN_NAME As String = "magetdat;createdon;date;es_ab;es_ende;datum;fakdate;birthdate;aus_dat;paidon;krediton;printdate;reminder_1date;reminder_2date;reminder_3date;buchungdate"
	Private Const INTEGER_COLUMN_NAME As String = "vacancynumber;ofnr;vaknr;fopnr;manr;kdnr;zhdnr;esnr;rpnr;zgnr;lonr;renr;id;vgnr;monat;jahr"
	Private Const DECIMAL_COLUMN_NAME As String = "betragex;bezahlt;betragink;betragmwst;betragtotal;zebetrag;lobetrag;betrag;aus_dat"

#End Region

#Region "Private Properties"

	Private ReadOnly Property SelectedRowViewData As FoundedOfferData
		Get
			Dim grdView = TryCast(grdMain.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim contact = CType(grdView.GetRow(selectedRows(0)), FoundedOfferData)
					Return contact
				End If

			End If

			Return Nothing
		End Get

	End Property

#End Region


#Region "Constructor"

	Public Sub New()

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		BarManager1.Form = Me
		LoadedMDNr = ModulConstants.MDData.MDNr
		m_SettingsManager = New SettingsKDManager
		Me._OSetting = New ClsOfferSetting

		m_md = New Mandant
		m_utility = New Utilities
		m_common = New CommonSetting
		m_path = New ClsProgPath

		m_translate = New TranslateValues

		Try
			m_communicationHub = MessageService.Instance.Hub

			m_GVMainSettingfilename = String.Format("{0}Offers\{1}{2}.xml", ModulConstants.GridSettingPath, gvMain.Name, ModulConstants.UserData.UserNr)

			m_xmlSettingRestoreMainSetting = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_RESTORE, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)
			m_xmlSettingMainFilter = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_FILTER, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
		End Try
		m_AllowedChangeMandant = IsUserActionAllowed(ModulConstants.UserData.UserNr, 200003)

		AddHandler Me.gvMain.RowCellClick, AddressOf OngvMain_RowCellClick
		AddHandler Me.gvMain.ColumnFilterChanged, AddressOf OnGVMain_ColumnFilterChanged
		AddHandler Me.gvMain.FocusedRowChanged, AddressOf OngvMain_FocusedRowChanged
		AddHandler Me.gvMain.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground

		m_MainViewGridData = New SuportedCodes
		m_MainViewGridData.ChangeColumnNamesToLowercase = True
		m_MainViewGridData.LoadMainModulGridPropertiesFromXML(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr, MODUL_NAME)
		m_griddata = m_MainViewGridData.LoadMainGridData

		ResetMandantenDropDown()
		LoadFoundedMDList()
		cboMD.EditValue = ModulConstants.MDData.MDNr

		'LoadXMLDataForSelectedModule()
		ResetMainGrid()

	End Sub

#End Region


	'Sub LoadXMLDataForSelectedModule()

	'	m_griddata = GetUserGridPropertiesFromXML(ModulConstants.MDData.MDNr)
	'	If m_griddata.SQLQuery Is Nothing Then
	'		m_griddata = GetGridPropertiesFromXML(ModulConstants.MDData.MDNr)
	'	End If

	'	aColCaption = m_griddata.GridColCaption.Split(CChar(";"))
	'	aColFieldName = m_griddata.GridColFieldName.Split(CChar(";"))
	'	aColWidth = m_griddata.GridColWidth.Split(CChar(";"))

	'End Sub


	'Private Function GetGridPropertiesFromXML(ByVal iMDNr As Integer) As GridData
	'	Dim result As New GridData
	'	If iMDNr = 0 Then iMDNr = ModulConstants.MDData.MDNr
	'	m_path = New ClsProgPath

	'	Try
	'		Dim xDoc As XDocument = XDocument.Load(m_md.GetSelectedMDMainViewXMLFilename(ModulConstants.MDData.MDNr))
	'		Dim ConfigQuery = (From exportSetting In xDoc.Root.Elements("Modul")
	'													Where Not (exportSetting.Attribute("ID") Is Nothing) _
	'															And exportSetting.Attribute("ID").Value = String.Format("{0}", MODUL_NAME) _
	'															Select New With {
	'																				.SQLQuery = m_utility.GetSafeStringFromXElement(exportSetting.Element("SQL")),
	'																				.GridColFieldName = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColFieldName")),
	'																				.DisplayMember = m_utility.GetSafeStringFromXElement(exportSetting.Element("DisplayMember")),
	'																				.GridColCaption = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColCaption")),
	'																				.GridColWidth = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColWidth")),
	'																				.BackColor = m_utility.GetSafeStringFromXElement(exportSetting.Element("BackColor")),
	'																				.ForeColor = m_utility.GetSafeStringFromXElement(exportSetting.Element("ForeColor")),
	'																				.PopupFields = m_utility.GetSafeStringFromXElement(exportSetting.Element("PopupFieldName")),
	'																				.PopupCaptions = m_utility.GetSafeStringFromXElement(exportSetting.Element("PopupFieldCaption")),
	'																				.CountOfFieldsInHeader = m_utility.GetSafeStringFromXElement(exportSetting.Element("CountOfFieldsInHeaderToShow")),
	'																				.FieldsInHeaderToShow = m_utility.GetSafeStringFromXElement(exportSetting.Element("FieldsInHeaderToShow")),
	'																				.CaptionsInHeaderToShow = m_utility.GetSafeStringFromXElement(exportSetting.Element("CaptionInHeaderToShow"))
	'																					}).FirstOrDefault()



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
	'													Where Not (exportSetting.Attribute("ID") Is Nothing) _
	'															And exportSetting.Attribute("ID").Value = String.Format("{0}", MODUL_NAME) _
	'															Select New With {
	'																				.SQLQuery = m_utility.GetSafeStringFromXElement(exportSetting.Element("SQL")),
	'																				.GridColFieldName = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColFieldName")),
	'																				.DisplayMember = m_utility.GetSafeStringFromXElement(exportSetting.Element("DisplayMember")),
	'																				.GridColCaption = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColCaption")),
	'																				.GridColWidth = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColWidth")),
	'																				.BackColor = m_utility.GetSafeStringFromXElement(exportSetting.Element("BackColor")),
	'																				.ForeColor = m_utility.GetSafeStringFromXElement(exportSetting.Element("ForeColor")),
	'																				.PopupFields = m_utility.GetSafeStringFromXElement(exportSetting.Element("PopupFieldName")),
	'																				.PopupCaptions = m_utility.GetSafeStringFromXElement(exportSetting.Element("PopupFieldCaption")),
	'																				.CountOfFieldsInHeader = m_utility.GetSafeStringFromXElement(exportSetting.Element("CountOfFieldsInHeaderToShow")),
	'																				.FieldsInHeaderToShow = m_utility.GetSafeStringFromXElement(exportSetting.Element("FieldsInHeaderToShow")),
	'																				.CaptionsInHeaderToShow = m_utility.GetSafeStringFromXElement(exportSetting.Element("CaptionInHeaderToShow"))
	'																					}).FirstOrDefault()


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
		Try
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(ModulConstants.MDData.MDNr, m_md.GetDefaultUSNr, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If
		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		Dim setting_splitterpos = m_SettingsManager.ReadInteger(SettingVacancyKeys.SETTING_VAK_SCC_MAIN_SPLITTERPOSION)
		If setting_splitterpos > 0 Then sccMain.SplitterPosition = Math.Max(Me.sccMain.SplitterPosition, setting_splitterpos)

		setting_splitterpos = m_SettingsManager.ReadInteger(SettingVacancyKeys.SETTING_VAK_SCC_HEADERINFO_SPLITTERPOSION)
		sccHeaderInfo.SplitterPosition = sccHeaderInfo.Width - 250

		Me.pcHeader.Dock = DockStyle.Fill
		Me.sccMain.Dock = DockStyle.Fill
		Me.sccHeaderInfo.Dock = DockStyle.Fill

		Me.gvLProperty.OptionsView.ShowIndicator = False

		Me.gvMain.OptionsView.ShowIndicator = False

		gvMain.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvMain.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvMain.OptionsView.ShowGroupPanel = False
		gvMain.OptionsView.ShowAutoFilterRow = True

		AddHandler Me.sccMain.SplitterPositionChanged, AddressOf SaveFormProperties
		AddHandler Me.sccHeaderInfo.SplitterPositionChanged, AddressOf SaveFormProperties
		AddHandler Me.sccHeaderInfo.SizeChanged, AddressOf SaveFormProperties

	End Sub

	Sub SaveFormProperties()

		sccHeaderInfo.SplitterPosition = sccHeaderInfo.Width - 250
		m_SettingsManager.WriteInteger(SettingOfferKeys.SETTING_OFFER_SCC_MAIN_SPLITTERPOSION, Me.sccMain.SplitterPosition)
		m_SettingsManager.WriteInteger(SettingOfferKeys.SETTING_OFFER_SCC_HEADERINFO_SPLITTERPOSION, Me.sccHeaderInfo.SplitterPosition)

		m_SettingsManager.SaveSettings()

	End Sub

	Private Sub ResetMainGrid()

		m_MainViewGridData.ResetMainGrid(gvMain, grdMain, MODUL_NAME)
		RestoreGridLayoutFromXml(gvMain.Name.ToLower)

		Return

		gvMain.Columns.Clear()
		grdLProperty.DataSource = Nothing

		Try
			Dim AutofilterconditionNumber = ModulConstants.MDData.AutoFilterConditionNumber
			Dim AutofilterconditionDate = ModulConstants.MDData.AutoFilterConditionDate

			For i = 0 To aColCaption.Length - 1

				Dim column As New DevExpress.XtraGrid.Columns.GridColumn()
				Dim columnCaption As String = m_translate.GetSafeTranslationValue(aColCaption(i).Trim)
				Dim columnName As String = aColFieldName(i).ToLower.Trim

				column.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
				column.Caption = columnCaption
				column.Name = columnName
				column.FieldName = columnName

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

				End If


				column.Visible = If(aColWidth(i) > 0, True, False)
				gvMain.Columns.Add(column)

			Next

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdMain.DataSource = Nothing

	End Sub

	Private Function LoadFoundedOffersList() As Boolean
		Dim m_DataAccess As New MainGrid

		Dim listOfFoundedData = m_DataAccess.GetDbOfferData4Show(m_griddata.SQLQuery)
		If listOfFoundedData Is Nothing Then
			SplashScreenManager.CloseForm(False)
			ShowErrDetail(m_translate.GetSafeTranslationValue("Keine Datenstätze wurden gefunden."))

			Exit Function
		End If

		Dim responsiblePersonsGridData = (From person In listOfFoundedData
										  Select New FoundedOfferData With
					 {
						._res = person._res,
						.mdnr = person.mdnr,
						.ofnr = person.ofnr,
						.kdnr = person.kdnr,
						.zhdnr = person.zhdnr,
						.manr = person.manr,
						.employeename = person.employeename,
						.bezeichnung = person.bezeichnung,
						.createdon = person.createdon,
						.createdfrom = person.createdfrom,
						.offerstate = person.offerstate,
						.customername = person.customername,
						.customerstreet = person.customerstreet,
						.customeraddress = person.customeraddress,
						.customertelefon = person.customertelefon,
						.customertelefax = person.customertelefax,
						.customeremail = person.customeremail,
						.zname = person.zname,
						.ztelefon = person.ztelefon,
						.zmobile = person.zmobile,
						.zemail = person.zemail,
						.advisor = person.advisor,
						.zfiliale = person.zfiliale
					}).ToList()

		Dim listDataSource As BindingList(Of FoundedOfferData) = New BindingList(Of FoundedOfferData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdMain.DataSource = listDataSource

		RefreshMainViewStateBar()
		grpFunction.BackColor = If(Not String.IsNullOrWhiteSpace(ModulConstants.MDData.MandantColorName), System.Drawing.Color.FromName(ModulConstants.MDData.MandantColorName), Nothing)

		Return Not listOfFoundedData Is Nothing
	End Function

	Private Sub OnGVMain_ColumnFilterChanged(sender As Object, e As System.EventArgs)

		RefreshMainViewStateBar()

	End Sub


	Private Sub pg_Load(sender As Object, e As System.EventArgs) Handles Me.Load

		TranslateControls()

		SetFormLayout()
		BuildNewPrintContextMenu()

		LoadFoundedOffersList()

	End Sub

	Sub TranslateControls()

		Me.cmdNew.Text = m_translate.GetSafeTranslationValue(Me.cmdNew.Text)
		Me.cmdPrint.Text = m_translate.GetSafeTranslationValue(Me.cmdPrint.Text)

		Me.grpFunction.Text = m_translate.GetSafeTranslationValue(Me.grpFunction.Text)

	End Sub


	Sub OngvMain_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		pccMandant.HidePopup()
		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvMain.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, FoundedOfferData)

				Me._OSetting.SelectedOfferNr = viewData.ofnr
				Me._OSetting.SelectedMANr = viewData.manr
				Me._OSetting.SelectedKDNr = viewData.kdnr
				Me._OSetting.SelectedZHDNr = viewData.zhdnr

				Select Case column.Name.ToLower
					Case "ztelefon"
						If viewData.ztelefon.Length > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedKDNr = viewData.kdnr, .SelectedZHDNr = viewData.zhdnr})
							_ClsKD.TelefonCallToCustomer(viewData.ztelefon)
						End If

					Case "zmobile"
						If viewData.zmobile.Length > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedKDNr = viewData.kdnr, .SelectedZHDNr = viewData.zhdnr})
							_ClsKD.TelefonCallToCustomer(viewData.zmobile)
						End If

					Case "customertelefon"
						If viewData.customertelefon.Length > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedKDNr = viewData.kdnr, .SelectedZHDNr = 0})
							_ClsKD.TelefonCallToCustomer(viewData.customertelefon)
						End If

					Case "zname", "zhdnr"
						If viewData.zhdnr > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedKDNr = viewData.kdnr, .SelectedZHDNr = viewData.zhdnr})
							_ClsKD.OpenSelectedCPerson()
						End If

					Case "employeename", "manr"
						If viewData.manr > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMANr = viewData.manr})
							_ClsKD.OpenSelectedEmployee(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)
						End If

					Case "customername", "kdnr"
						If viewData.kdnr > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedKDNr = viewData.kdnr})
							_ClsKD.OpenSelectedCustomer(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)
						End If

					Case Else
						If viewData.ofnr > 0 Then
							Dim _ClsVak As New ClsOpenModul(New ClsSetting With {.SelectedOfferNr = viewData.ofnr,
																																	 .SelectedKDNr = viewData.kdnr,
																																	 .SelectedZHDNr = viewData.zhdnr})
							_ClsVak.OpenSelectedOffer()

						End If

				End Select

			End If

		End If

	End Sub

	Private Sub OngvMain_FocusedRowChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Dim selectedrow = SelectedRowViewData

			If Not selectedrow Is Nothing Then
				Me._OSetting.SelectedOfferNr = selectedrow.ofnr
				Me._OSetting.SelectedKDNr = selectedrow.kdnr
				Me._OSetting.SelectedZHDNr = selectedrow.zhdnr

				If selectedrow.ofnr = 0 Then Exit Sub

				FillPopupFields(selectedrow)
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			ShowErrDetail(ex.Message)

		End Try

	End Sub

	Private Sub FillPopupFields(ByVal selectedrow As FoundedOfferData)
		Dim result As New Dictionary(Of String, String)
		Dim strValue As String = String.Empty

		Dim listOfPropertyLData As New List(Of PropertyData)
		Dim listOfPropertyRData As New List(Of PropertyData)
		grdLProperty.DataSource = Nothing

		Dim strPFieldName As String = m_griddata.FieldsInHeaderToShow
		Dim strPFieldCaption As String = m_griddata.CaptionsInHeaderToShow
		Dim aPoupFields As String() = strPFieldName.Split(CChar(";"))
		Dim aPoupCaption As String() = strPFieldCaption.Split(CChar(";"))
		Dim iCountofFieldInHeaderInfo As Short = 3
		If m_griddata.CountOfFieldsInHeader.HasValue Then
			iCountofFieldInHeaderInfo = m_griddata.CountOfFieldsInHeader
		End If

		Try

			For j As Integer = 0 To aPoupFields.Length - 1
				Select Case aPoupFields(j).ToLower
					Case "advisor"
						strValue = selectedrow.advisor
					Case "employeename"
						strValue = selectedrow.employeename

					Case "customername"
						strValue = selectedrow.customername

					Case "bezeichung"
						strValue = selectedrow.bezeichnung

					Case "createdfrom"
						strValue = selectedrow.createdfrom

					Case "createdon"
						strValue = selectedrow.createdon

					Case "customeremail"
						strValue = selectedrow.customeremail

					Case "kdnr"
						strValue = selectedrow.kdnr


					Case "customertelefon"
						strValue = selectedrow.customertelefon
					Case "customertelefax"
						strValue = selectedrow.customertelefax
					Case "ztelefon"
						strValue = selectedrow.ztelefon

					Case "zmobile"
						strValue = selectedrow.zmobile
					Case "zemail"
						strValue = selectedrow.zemail

					Case "zhdnr"
						strValue = selectedrow.zhdnr
					Case "zname"
						strValue = selectedrow.zname

					Case "offerstate"
						strValue = selectedrow.offerstate
					Case "advisor"
						strValue = selectedrow.advisor

					Case "zfiliale"
						strValue = selectedrow.zfiliale


					Case Else
						strValue = "?"

				End Select

				result.Add(aPoupFields(j), strValue)
			Next

			Dim itemName As String
			Dim itemValue As String

			For i As Integer = 0 To result.Count - 1
				If Not String.IsNullOrEmpty(aPoupCaption(i)) And Not String.IsNullOrEmpty(result.Item(aPoupFields(i))) Then

					If i > iCountofFieldInHeaderInfo Then
						itemName = aPoupCaption(i)
						itemValue = result.Item(aPoupFields(i))

						Dim propertyItem As New PropertyData With {.ValueName = m_translate.GetSafeTranslationValue(itemName), .Value = itemValue}
						listOfPropertyRData.Add(propertyItem)

					Else
						itemName = aPoupCaption(i)
						itemValue = result.Item(aPoupFields(i))

						Dim propertyItem As New PropertyData With {.ValueName = m_translate.GetSafeTranslationValue(itemName), .Value = itemValue}
						listOfPropertyLData.Add(propertyItem)

					End If

				End If
			Next

			If Not (listOfPropertyLData Is Nothing) Then
				grdLProperty.DataSource = listOfPropertyLData
				grdLProperty.Visible = True

			Else
				grdLProperty.Visible = False

			End If


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			ShowErrDetail(ex.Message)
			result = Nothing

		End Try

	End Sub


#Region "DropDown Button für New und Print..."

	''' <summary>
	''' shows contextmenu for print-button
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub cmdPrint_Click(sender As System.Object, e As System.EventArgs) Handles cmdPrint.Click
		Me.cmdPrint.ShowDropDown()
	End Sub

	''' <summary>
	''' shows Contexmenu for New-button
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub cmdNew_Click(sender As System.Object, e As System.EventArgs) Handles cmdNew.Click
		Me.cmdNew.ShowDropDown()
	End Sub


#End Region

#Region "Mandantendaten auswählen..."

	''' <summary>
	''' Resets the Mandanten drop down.
	''' </summary>
	Private Sub ResetMandantenDropDown()

		cboMD.Properties.DisplayMember = "MDName"
		cboMD.Properties.ValueMember = "MDNr"

		Dim columns = cboMD.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo With {.FieldName = "MDName",
																					 .Width = 100,
																					 .Caption = "Mandant"})

		cboMD.Properties.ShowHeader = False
		cboMD.Properties.ShowFooter = False

		cboMD.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		cboMD.Properties.SearchMode = SearchMode.AutoComplete
		cboMD.Properties.AutoSearchColumnIndex = 0

		cboMD.Properties.NullText = String.Empty
		cboMD.EditValue = Nothing

	End Sub

	' Mandantendaten...
	Private Sub OnCboMD_EditValueChanged(sender As Object, e As System.EventArgs) Handles cboMD.EditValueChanged
		Dim SelectedData As MDData = TryCast(Me.cboMD.GetSelectedDataRow(), MDData)
		Dim OldMDNr = ModulConstants.MDData.MDNr

		If Not SelectedData Is Nothing Then

			ModulConstants.MDData.MDNr = cboMD.EditValue
			If OldMDNr <> SelectedData.MDNr Then
				ModulConstants.MDData = ModulConstants.SelectedMDData(cboMD.EditValue)
				ResetMainGrid()
				LoadFoundedOffersList()
			End If
		End If

		Me.grdMain.Enabled = Not (cboMD.EditValue Is Nothing)
		pccMandant.HidePopup()

	End Sub

	Private Sub OnpageVisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged

		pccMandant.HidePopup()
		If Me.Visible Then RefreshMainViewStateBar()
		If LoadedMDNr <> ModulConstants.MDData.MDNr Then
			LoadedMDNr = ModulConstants.MDData.MDNr
			ResetMandantenDropDown()
			Dim supportedData = New SuportedCodes
			ListOfMandantData = supportedData.LoadFoundedMDList()

			LoadFoundedMDList()
			cboMD.EditValue = LoadedMDNr

			RefreshData()

		End If

	End Sub

	Private Function LoadFoundedMDList() As Boolean
		cboMD.Properties.DataSource = ListOfMandantData

		Return Not ListOfMandantData Is Nothing
	End Function

	Private Sub grpFunction_CustomButtonClick(sender As Object, e As DevExpress.XtraBars.Docking2010.BaseButtonEventArgs) Handles grpFunction.CustomButtonClick

		If e.Button.Properties.GroupIndex = 0 Then

			pccMandant.SuspendLayout()
			Me.pccMandant.Manager = New DevExpress.XtraBars.BarManager
			pccMandant.ShowCloseButton = True
			pccMandant.ShowSizeGrip = True

			pccMandant.ShowPopup(Cursor.Position)
			pccMandant.ResumeLayout()

		Else
			RefreshData()

		End If

	End Sub

	Private Sub RefreshData()
		SplashScreenManager.CloseForm(False)
		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
		SplashScreenManager.Default.SetWaitFormCaption(m_translate.GetSafeTranslationValue("Ihre Abfrage wird ausgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		'LoadXMLDataForSelectedModule()
		ResetMainGrid()
		LoadFoundedOffersList()

		SplashScreenManager.CloseForm(False)

	End Sub


	Private Sub RefreshMainViewStateBar()

		m_communicationHub.Publish(New RefreshMainViewStatebar(Me, Me.gvMain.RowCount, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing))
		cmdNew.Enabled = ModulConstants.MDData.ClosedMD = 0

	End Sub

#End Region


#Region "Helpers"

	Private Sub BuildNewPrintContextMenu()

		Try
			' build contextmenu
			Dim _ClsNewPrint As New ClsOfferModuls(Me._OSetting)
			_ClsNewPrint.ShowContextMenu4Print(Me.cmdPrint)
			_ClsNewPrint.ShowContextMenu4New(cmdNew)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

	End Sub

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
			Case "gvmain".ToLower
				Try
					keepFilter = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingMainFilter), False)
					restoreLayout = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreMainSetting), True)

				Catch ex As Exception

				End Try

				If restoreLayout AndAlso File.Exists(m_GVMainSettingfilename) Then gvMain.RestoreLayoutFromXml(m_GVMainSettingfilename)
				If restoreLayout AndAlso Not keepFilter Then gvMain.ActiveFilterCriteria = Nothing


			Case Else

				Exit Sub


		End Select


	End Sub


	Private Sub OngvMainColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvMain.SaveLayoutToXml(m_GVMainSettingfilename)

	End Sub


#End Region

End Class
