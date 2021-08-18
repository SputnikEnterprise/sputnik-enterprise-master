
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Common.DataObjects

Imports SP.DatabaseAccess.TableSetting
Imports SP.DatabaseAccess.TableSetting.DataObjects
Imports SP.DatabaseAccess.TableSetting.DataObjects.MandantData

Imports System.ComponentModel
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraNavBar
Imports DevExpress.XtraEditors
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraSplashScreen
Imports DevExpress.XtraEditors.Repository

Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Infrastructure

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages

Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath
Imports SPS.SYS.TableSettingMng.SPPVLGAVUtilWebService
Imports System.Threading.Tasks
Imports System.Threading
Imports System.IO
Imports System.Xml.Serialization
Imports DevExpress.XtraBars
Imports System.Reflection
Imports DevExpress.Utils.Menu
Imports System.Xml.XPath
Imports System.Xml

Public Class frmUsers


#Region "Private Fields"

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The common data access object.
	''' </summary>
	Private m_CommonDatabaseAccess As ICommonDatabaseAccess

	Private m_TablesettingDatabaseAccess As ITablesDatabaseAccess
	''' <summary>
	''' The common data access object.
	''' </summary>
	Private m_MandantDatabaseAccess As ChildEducationData

	''' <summary>
	''' The cost centers.
	''' </summary>
	Private m_CostCenters As SP.DatabaseAccess.Common.DataObjects.CostCenters

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' The settings manager.
	''' </summary>
	Private m_SettingsManager As ISettingsManager

	Private m_mandant As Mandant

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As Utility

	Private m_ProgPath As ClsProgPath


	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	Private connectionString As String

	''' <summary>
	''' Record number of selected row.
	''' </summary>
	Private m_CurrentRecordNumber As Integer?
	Private m_UserNumbertoCopy As Integer?
	Private m_CurrentAddressRecordNumber As Integer?
	Private m_selectedUserData As UserData

	Private m_MandantXMLFile As String
	Private m_MandantFormXMLFileName As String
	Private m_MandantSetting As String
	Private m_PayrollSetting As String

	Private m_MandantSettingsXml As SPProgUtility.CommonXmlUtility.SettingsXml

	''' <summary>
	''' Boolean flag indicating if form is initializing.
	''' </summary>
	Protected m_SuppressUIEvents As Boolean = False

	Private errorProviderMangement As DXErrorProvider.DXErrorProvider

	Private ucUserDocumentRights As ucUserDocument

	Private m_UserRight654 As Boolean
	Private m_UserRight655 As Boolean
	Private m_UserRight656 As Boolean
	Private m_UserRight657 As Boolean
	Private m_UserRight658 As Boolean


#End Region


#Region "private constants"

	Private Const strEncryptionKey As String = "your crypt key"
	Private Const strExtraPass As String = "yourseckey"

#End Region

#Region "private consts"

	Private Const MANDANT_XML_MAIN_KEY As String = "MD_{0}/"
	Private Const MANDANT_XML_SETTING_SPUTNIK_PAYROLL_SETTING As String = "MD_{0}/Lohnbuchhaltung"
	Public Const DEFAULT_SPUTNIK_PVLGAV_UTIL_WEBSERVICE_URI As String = "http://asmx.domain.com/wsSPS_services/SPPVLGAVUtil.asmx"

#End Region


#Region "Constructor"

	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_SuppressUIEvents = True
		m_InitializationData = _setting
		ClsDataDetail.m_InitialData = m_InitializationData

		m_mandant = New Mandant

		m_Utility = New Utility
		m_UtilityUI = New UtilityUI
		m_ProgPath = New ClsProgPath


		errorProviderMangement = New DXErrorProvider.DXErrorProvider
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		Me.KeyPreview = True
		Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		InitializeComponent()


		' Translate controls.
		TranslateControls()

		connectionString = m_InitializationData.MDData.MDDbConn
		m_CommonDatabaseAccess = New CommonDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
		m_TablesettingDatabaseAccess = New TablesDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)


		m_MandantSetting = String.Format(MANDANT_XML_MAIN_KEY, m_InitializationData.MDData.MDNr)

		m_PayrollSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_PAYROLL_SETTING, m_InitializationData.MDData.MDNr)
		m_MandantXMLFile = m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, m_InitializationData.MDData.MDYear)
		If Not System.IO.File.Exists(m_MandantXMLFile) Then
			m_UtilityUI.ShowErrorDialog(String.Format("Die Einstellung-Datei wurde nicht gefunden.{0}{1}", vbNewLine, m_MandantXMLFile))
		Else
			m_MandantSettingsXml = New SPProgUtility.CommonXmlUtility.SettingsXml(m_MandantXMLFile)
		End If

		Reset()

		LoadSalutationAndSalutationFormDropDownData()
		LoadLanguageDropDownData()
		LoadKst1DropDownData()
		LoadMandantDropDownData()
		LoadBusinessBranchesDropDown()
		LoadRightsDropDownData()
		LoadCountryDropDownData()
		LoadPrivatCountryDropDownData()
		LoadPrivatPostcodeDropDownData()
		LoadPostcodeDropDownData()

		ucUserDocumentRights = New ucUserDocument
		xtabUserDocrights.Controls.Add(ucUserDocumentRights)
		ucUserDocumentRights.InitWithConfigurationData(m_InitializationData, m_Translate, m_InitializationData.MDData.MDYear)
		ucUserDocumentRights.Dock = DockStyle.Fill
		ucUserDocumentRights.Visible = False

		AddHandler gvUser.RowCellClick, AddressOf Ongv_RowCellClick
		AddHandler gvUser.FocusedRowChanged, AddressOf Ongv_FocusedRowChanged
		AddHandler lueBusinessBranches.ButtonClick, AddressOf OnDropDown_ButtonClick

		m_SuppressUIEvents = False

	End Sub


#End Region


#Region "private properties"

	Private ReadOnly Property SignAsEmploymentSigner() As Boolean
		Get
			Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
			Dim selectedData = SelectedExitingRecord
			Dim mdNumber As Integer = 0
			Dim userNumber As Integer = 0
			If Not selectedData Is Nothing Then
				mdNumber = selectedData.MDNr
				userNumber = selectedData.USNr
			Else
				mdNumber = m_InitializationData.MDData.MDNr
				userNumber = m_InitializationData.UserData.UserNr
			End If

			Dim sValue As Boolean = True

			Dim strQuery As String = String.Format("//ExportSetting[@Name={0}SP.ES.PrintUtility{0}]/esunterzeichner_esvertrag", Chr(34))
			Dim strBez As String = _ClsProgSetting.GetXMLNodeValue(m_mandant.GetSelectedMDUserProfileXMLFilename(mdNumber, userNumber), strQuery)
			sValue = StrToBool(strBez)

			Return sValue
		End Get

	End Property

	Private ReadOnly Property ShowCustomerRecordsInColor() As Boolean
		Get
			Dim FORM_XML_MAIN_KEY As String = "UserProfile/programsetting"
			Dim m_path As New SPProgUtility.ProgPath.ClsProgPath
			Dim selectedData = SelectedExitingRecord
			Dim mdNumber As Integer = 0
			Dim userNumber As Integer = 0
			If Not selectedData Is Nothing Then
				mdNumber = selectedData.MDNr
				userNumber = selectedData.USNr
			Else
				mdNumber = m_InitializationData.MDData.MDNr
				userNumber = m_InitializationData.UserData.UserNr
			End If

			Dim UserXMLFileName = m_mandant.GetSelectedMDUserProfileXMLFilename(mdNumber, userNumber)
			Dim value As Boolean? = StrToBool(m_path.GetXMLNodeValue(UserXMLFileName, String.Format("{0}/showcustomerrecordsincolor", FORM_XML_MAIN_KEY)))


			Return value
		End Get

	End Property

	Private ReadOnly Property LoadLabelPrinter() As String
		Get
			Dim FORM_XML_MAIN_KEY As String = "UserProfile/Layouts/Report"
			Dim m_path As New SPProgUtility.ProgPath.ClsProgPath
			Dim selectedData = SelectedExitingRecord
			Dim mdNumber As Integer = 0
			Dim userNumber As Integer = 0
			If Not selectedData Is Nothing Then
				mdNumber = selectedData.MDNr
				userNumber = selectedData.USNr
			Else
				mdNumber = m_InitializationData.MDData.MDNr
				userNumber = m_InitializationData.UserData.UserNr
			End If

			Dim UserXMLFileName = m_mandant.GetSelectedMDUserProfileXMLFilename(mdNumber, userNumber)
			'm_MandantSettingsXml = New SPProgUtility.CommonXmlUtility.SettingsXml(UserXMLFileName)

			Dim value As String = m_path.GetXMLNodeValue(UserXMLFileName, String.Format("{0}/matrixprintername", FORM_XML_MAIN_KEY))


			Return value
		End Get

	End Property

	Private ReadOnly Property DeleteUserTempFilesOnLogin() As Boolean
		Get
			Dim FORM_XML_MAIN_KEY As String = "UserProfile/programsetting"
			Dim m_path As New SPProgUtility.ProgPath.ClsProgPath
			Dim selectedData = SelectedExitingRecord
			Dim mdNumber As Integer = 0
			Dim userNumber As Integer = 0
			If Not selectedData Is Nothing Then
				mdNumber = selectedData.MDNr
				userNumber = selectedData.USNr
			Else
				mdNumber = m_InitializationData.MDData.MDNr
				userNumber = m_InitializationData.UserData.UserNr
			End If

			Dim UserXMLFileName = m_mandant.GetSelectedMDUserProfileXMLFilename(mdNumber, userNumber)
			Dim value As Boolean? = StrToBool(m_path.GetXMLNodeValue(UserXMLFileName, String.Format("{0}/" & "DeleteUserTempFilesOnLogin".ToLower, FORM_XML_MAIN_KEY)))


			Return value
		End Get

	End Property

	Private ReadOnly Property PrintCusotmerDataOnReportsContentTemplate() As Boolean
		Get
			Dim FORM_XML_MAIN_KEY As String = "UserProfile/programsetting"
			Dim m_path As New SPProgUtility.ProgPath.ClsProgPath
			'Dim selectedData = SelectedExitingRecord
			'Dim mdNumber As Integer = 0
			'Dim userNumber As Integer = 0
			'If Not selectedData Is Nothing Then
			'	mdNumber = selectedData.MDNr
			'	userNumber = selectedData.USNr
			'Else
			'	mdNumber = m_InitializationData.MDData.MDNr
			'	userNumber = m_InitializationData.UserData.UserNr
			'End If

			Dim UserXMLFileName = m_mandant.GetSelectedMDUserProfileXMLFilename(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr)
			Dim value As String = m_path.GetXMLNodeValue(UserXMLFileName, String.Format("{0}/printcustomerdataonreporttemplate", FORM_XML_MAIN_KEY))
			If String.IsNullOrWhiteSpace(value) Then Return True Else Return StrToBool(value)

		End Get

	End Property

#End Region

#Region "public property"

	''' <summary>
	''' Gets the selected record.
	''' </summary>
	''' <returns>The selected user or nothing if none is selected.</returns>
	Public ReadOnly Property SelectedExitingRecord As UserData
		Get
			Dim gvRP = TryCast(grdUser.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvRP Is Nothing) Then

				Dim selectedRows = gvRP.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim employee = CType(gvRP.GetRow(selectedRows(0)), UserData)
					Return employee
				End If

			End If

			Return Nothing
		End Get

	End Property


#End Region



#Region "public Methods"

	Public Function LoadUserData() As Boolean

		Dim success = LoadUserList()
		If success Then FocusUserData(m_InitializationData.UserData.UserNr)

		Return success
	End Function


#End Region


#Region "Private Methods"



	Private Sub Reset()

		Dim suppressUIEventsState As Boolean = m_SuppressUIEvents
		m_SuppressUIEvents = True

		m_CurrentRecordNumber = Nothing
		m_UserNumbertoCopy = Nothing
		m_CurrentAddressRecordNumber = Nothing
		m_CostCenters = Nothing

		lueSalutation.EditValue = Nothing
		txt_Nachname.EditValue = String.Empty
		txt_Vorname.EditValue = String.Empty
		txt_Anmeldename.EditValue = String.Empty
		txt_Passwort.EditValue = String.Empty
		txt_Passwort.Properties.PasswordChar = "*"

		pePicture.Image = Nothing
		peSign.Image = Nothing

		deUntil.EditValue = Nothing
		chk_Deaktivieren.Checked = False
		lueKst1.EditValue = Nothing
		lueKst2.EditValue = Nothing
		txt_Kst.EditValue = String.Empty
		lueMandant.EditValue = Nothing
		lueBusinessBranches.EditValue = Nothing
		lueRights.EditValue = Nothing

		txt_p_Abteilung.EditValue = String.Empty
		txt_p_EMail.EditValue = String.Empty
		txt_p_homepage.EditValue = String.Empty
		txt_p_Mobile.EditValue = String.Empty
		txt_p_Ort.EditValue = String.Empty
		txt_p_Postfach.EditValue = String.Empty
		txt_p_Strasse.EditValue = String.Empty
		txt_p_Telefax.EditValue = String.Empty
		txt_p_Telefon.EditValue = String.Empty
		txt_p_Titel1.EditValue = String.Empty
		txt_p_Titel2.EditValue = String.Empty
		luePCountry.EditValue = Nothing
		luePPostcode.EditValue = Nothing
		de_p_GebDate.EditValue = Nothing


		txt_MDName.EditValue = String.Empty
		txt_MDName2.EditValue = String.Empty
		txt_MDName3.EditValue = String.Empty
		txt_Email.EditValue = String.Empty
		txt_Homepage.EditValue = String.Empty
		txt_Ort.EditValue = String.Empty
		txt_Strasse.EditValue = String.Empty
		txt_Telefax.EditValue = String.Empty
		txt_Telefon.EditValue = String.Empty
		txt_dTelefon.EditValue = String.Empty
		txt_Bewilligung.EditValue = String.Empty
		lueCountry.EditValue = Nothing
		luePostcode.EditValue = Nothing


		txt_Ex_Benutzer.EditValue = String.Empty
		txt_Ex_Passwort.EditValue = String.Empty
		txt_Ex_Passwort.Properties.PasswordChar = "*"

		txt_Jobs_KDNr.EditValue = 0
		txt_Jobs_LayoutID.EditValue = 0
		lue_Jobs_LogoID.EditValue = 0
		lue_OstJob_KDNr.EditValue = String.Empty
		txt_OstJob_Anzahl.EditValue = 0

		chkUserAsCostcenter.Checked = False
		chkLogOnMorePlaces.Checked = False
		chkUserAsCostcenter.Enabled = m_InitializationData.UserData.UserNr = 1
		chkLogOnMorePlaces.Enabled = m_InitializationData.UserData.UserNr = 1

		bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Bereit")
		bsilblCreated.Caption = m_Translate.GetSafeTranslationValue(bsilblCreated.Caption)
		bsilblChanged.Caption = m_Translate.GetSafeTranslationValue(bsilblChanged.Caption)

		ResetUserGrid()
		ResetKstDropDown()
		ResetSalutationDropDown()
		ResetLanguageDropDown()
		ResetCountryDropDown()
		ResetPrivatCountryDropDown()
		ResetPrivatPostcodeDropDown()
		ResetPostcodeDropDown()
		ResetMandantDropDown()
		ResetBusinessBranchesDropDown()
		ResetRightsDropDown()

		bbiDelete.Visibility = If(m_InitializationData.UserData.UserNr = 1, BarItemVisibility.Always, BarItemVisibility.Never)
		m_UserRight654 = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 654, m_InitializationData.MDData.MDNr)
		m_UserRight655 = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 655, m_InitializationData.MDData.MDNr)
		m_UserRight656 = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 656, m_InitializationData.MDData.MDNr)
		m_UserRight657 = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 657, m_InitializationData.MDData.MDNr)
		m_UserRight658 = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 658, m_InitializationData.MDData.MDNr)

		txt_Passwort.Enabled = m_UserRight655
		txt_Kst.Enabled = m_UserRight657
		bbiSave.Visibility = If(m_UserRight654, BarItemVisibility.Always, BarItemVisibility.Never)
		bbiCopy.Visibility = If(m_UserRight654, BarItemVisibility.Always, BarItemVisibility.Never)
		bbiUserRights.Visibility = If(m_UserRight656, BarItemVisibility.Always, BarItemVisibility.Never)

		If Year(Now) >= 2018 Then
			bbiCopy.Enabled = m_InitializationData.UserData.UserNr = 1
		End If

		m_SuppressUIEvents = suppressUIEventsState

		errorProviderMangement.ClearErrors()

	End Sub

	Private Sub PrepareForNew()

		Dim suppressUIEventsState As Boolean = m_SuppressUIEvents
		m_SuppressUIEvents = True

		grpAnmeldung.Text = m_Translate.GetSafeTranslationValue("Neuer Benutzer")
		m_CurrentRecordNumber = Nothing
		m_UserNumbertoCopy = Nothing
		m_CurrentAddressRecordNumber = Nothing
		m_CostCenters = Nothing

		lueSalutation.EditValue = Nothing
		txt_Nachname.EditValue = String.Empty
		txt_Vorname.EditValue = String.Empty
		txt_Anmeldename.EditValue = String.Empty
		txt_Passwort.EditValue = String.Empty

		pePicture.Image = Nothing
		peSign.Image = Nothing

		deUntil.EditValue = Nothing
		chk_Deaktivieren.Checked = False
		lueKst1.EditValue = Nothing
		lueKst2.EditValue = Nothing
		txt_Kst.EditValue = String.Empty
		lueRights.EditValue = Nothing

		txt_p_EMail.EditValue = String.Empty
		txt_p_homepage.EditValue = String.Empty
		txt_p_Ort.EditValue = String.Empty
		txt_p_Postfach.EditValue = String.Empty
		txt_p_Strasse.EditValue = String.Empty
		txt_p_Telefax.EditValue = String.Empty
		txt_p_Telefon.EditValue = String.Empty
		txt_p_Titel1.EditValue = String.Empty
		txt_p_Titel2.EditValue = String.Empty
		de_p_GebDate.EditValue = Nothing

		txt_Jobs_KDNr.EditValue = 0
		txt_Jobs_LayoutID.EditValue = 0
		lue_Jobs_LogoID.EditValue = 0
		lue_OstJob_KDNr.EditValue = String.Empty
		txt_OstJob_Anzahl.EditValue = 0

		xtabUserDocrights.PageEnabled = False
		bbiDelete.Enabled = False
		bbiCopy.Enabled = False
		bbiUserRights.Enabled = False

		bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Benutzer anlegen")
		bsiCreated.Caption = String.Empty
		bsiChanged.Caption = String.Empty

		chkUserAsCostcenter.Checked = False
		chkLogOnMorePlaces.Checked = False

		ResetKstDropDown()
		LoadKst1DropDownData()

		txt_Passwort.Enabled = m_UserRight655
		txt_Kst.Enabled = m_UserRight657
		bbiSave.Enabled = m_UserRight654
		bbiUserRights.Enabled = m_UserRight656

		m_SuppressUIEvents = suppressUIEventsState

		errorProviderMangement.ClearErrors()

	End Sub

	Private Sub ResetUserGrid()

		gvUser.OptionsView.ShowIndicator = False
		gvUser.OptionsView.ShowAutoFilterRow = True
		gvUser.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvUser.OptionsView.ShowFooter = False

		gvUser.Columns.Clear()


		Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
		columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
		columnrecid.Name = "recid"
		columnrecid.FieldName = "recid"
		columnrecid.Visible = False
		gvUser.Columns.Add(columnrecid)

		Dim columnUSNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnUSNr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnUSNr.Caption = m_Translate.GetSafeTranslationValue("Nummer")
		columnUSNr.Name = "USNr"
		columnUSNr.FieldName = "USNr"
		columnUSNr.Width = 30
		columnUSNr.Visible = True
		columnUSNr.SummaryItem.DisplayFormat = "{0:f0}"
		columnUSNr.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count
		columnUSNr.SummaryItem.Tag = "Count_User"
		gvUser.Columns.Add(columnUSNr)


		Dim columnUserFullname As New DevExpress.XtraGrid.Columns.GridColumn()
		columnUserFullname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnUserFullname.Caption = m_Translate.GetSafeTranslationValue("Benutzer")
		columnUserFullname.Name = "UserFullname"
		columnUserFullname.FieldName = "UserFullname"
		columnUserFullname.Visible = True
		columnUserFullname.BestFit()
		gvUser.Columns.Add(columnUserFullname)


		grdUser.DataSource = Nothing

	End Sub


	''' <summary>
	''' Resets the mandant drop down.
	''' </summary>
	Private Sub ResetMandantDropDown()

		lueMandant.Properties.ShowHeader = False
		lueMandant.Properties.ShowFooter = False
		lueMandant.Properties.DropDownRows = 10

		lueMandant.Properties.DisplayMember = "MandantName1"
		lueMandant.Properties.ValueMember = "MandantNumber"

		Dim columns = lueMandant.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo With {.FieldName = "MandantName1",
																					 .Width = 100,
																					 .Caption = m_Translate.GetSafeTranslationValue("Mandant")})

		lueMandant.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueMandant.Properties.SearchMode = SearchMode.AutoComplete
		lueMandant.Properties.AutoSearchColumnIndex = 0

		lueMandant.Properties.NullText = String.Empty
		lueMandant.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets the Kst1 and Kst2 drop down.
	''' </summary>
	Private Sub ResetKstDropDown()
		'Kst1
		lueKst1.Properties.DisplayMember = "KSTBezeichnung"
		lueKst1.Properties.ValueMember = "KSTName"

		lueKst1.Properties.Columns.Clear()
		lueKst1.Properties.Columns.Add(New LookUpColumnInfo("KSTName", 0))
		lueKst1.Properties.Columns.Add(New LookUpColumnInfo("KSTBezeichnung", 0))

		lueKst1.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueKst1.Properties.SearchMode = SearchMode.AutoComplete
		lueKst1.Properties.AutoSearchColumnIndex = 1
		lueKst1.Properties.NullText = String.Empty
		lueKst1.EditValue = Nothing

		'Kst2
		lueKst2.Properties.DisplayMember = "KSTBezeichnung"
		lueKst2.Properties.ValueMember = "KSTName"

		lueKst2.Properties.Columns.Clear()
		lueKst2.Properties.Columns.Add(New LookUpColumnInfo("KSTName", 0))
		lueKst2.Properties.Columns.Add(New LookUpColumnInfo("KSTBezeichnung", 0))

		lueKst2.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueKst2.Properties.SearchMode = SearchMode.AutoComplete
		lueKst2.Properties.AutoSearchColumnIndex = 1
		lueKst2.Properties.NullText = String.Empty
		lueKst2.EditValue = Nothing
	End Sub

	''' <summary>
	''' Resets the business branches drop down.
	''' </summary>
	Private Sub ResetBusinessBranchesDropDown()

		lueBusinessBranches.Properties.DisplayMember = "Name"
		lueBusinessBranches.Properties.ValueMember = "Name"

		Dim columns = lueBusinessBranches.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("Name", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

		lueBusinessBranches.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueBusinessBranches.Properties.SearchMode = SearchMode.AutoComplete
		lueBusinessBranches.Properties.AutoSearchColumnIndex = 0

		lueBusinessBranches.Properties.NullText = String.Empty
		lueBusinessBranches.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets the language drop down.
	''' </summary>
	Private Sub ResetLanguageDropDown()

		lueLanguage.Properties.ShowHeader = False
		lueLanguage.Properties.ShowFooter = False
		lueLanguage.Properties.DropDownRows = 10

		lueLanguage.Properties.DisplayMember = "TranslatedDescription"
		lueLanguage.Properties.ValueMember = "Description"

		Dim columns = lueLanguage.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("TranslatedDescription", 0, m_Translate.GetSafeTranslationValue("Sprache")))

		lueLanguage.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueLanguage.Properties.SearchMode = SearchMode.AutoComplete
		lueLanguage.Properties.AutoSearchColumnIndex = 0

		lueLanguage.Properties.NullText = String.Empty
		lueLanguage.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets the salutation drop down.
	''' </summary>
	Private Sub ResetSalutationDropDown()

		lueSalutation.Properties.DisplayMember = "TranslatedSalutation"
		lueSalutation.Properties.ValueMember = "Salutation"

		Dim columns = lueSalutation.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("TranslatedSalutation", 0, ""))

		lueSalutation.Properties.DropDownRows = 10
		lueSalutation.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueSalutation.Properties.SearchMode = SearchMode.AutoComplete
		lueSalutation.Properties.AutoSearchColumnIndex = 0
		lueSalutation.Properties.NullText = String.Empty
		lueSalutation.EditValue = Nothing
	End Sub

	''' <summary>
	''' Resets the country drop down.
	''' </summary>
	Private Sub ResetPrivatCountryDropDown()

		luePCountry.Properties.ShowHeader = False
		luePCountry.Properties.ShowFooter = False
		luePCountry.Properties.DropDownRows = 20
		luePCountry.Properties.DisplayMember = "Name"
		luePCountry.Properties.ValueMember = "Code"

		Dim columns = luePCountry.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("Name", 0, m_Translate.GetSafeTranslationValue("Land")))

		luePCountry.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		luePCountry.Properties.SearchMode = SearchMode.AutoComplete
		luePCountry.Properties.AutoSearchColumnIndex = 0

		luePCountry.Properties.NullText = String.Empty
		luePCountry.EditValue = Nothing

	End Sub

	Private Sub ResetCountryDropDown()

		lueCountry.Properties.ShowHeader = False
		lueCountry.Properties.ShowFooter = False
		lueCountry.Properties.DropDownRows = 20
		lueCountry.Properties.DisplayMember = "Name"
		lueCountry.Properties.ValueMember = "Code"

		Dim columns = lueCountry.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("Name", 0, m_Translate.GetSafeTranslationValue("Land")))

		lueCountry.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueCountry.Properties.SearchMode = SearchMode.AutoComplete
		lueCountry.Properties.AutoSearchColumnIndex = 0

		lueCountry.Properties.NullText = String.Empty
		lueCountry.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets the postcode drop down.
	''' </summary>
	Private Sub ResetPrivatPostcodeDropDown()

		luePPostcode.Properties.SearchMode = SearchMode.OnlyInPopup
		luePPostcode.Properties.TextEditStyle = TextEditStyles.Standard

		luePPostcode.Properties.DisplayMember = "Postcode"
		luePPostcode.Properties.ValueMember = "Postcode"

		Dim columns = luePPostcode.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("Postcode", 0, m_Translate.GetSafeTranslationValue("PLZ")))
		columns.Add(New LookUpColumnInfo("Location", 0, m_Translate.GetSafeTranslationValue("Ort")))

		luePPostcode.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		luePPostcode.Properties.SearchMode = SearchMode.AutoComplete
		luePPostcode.Properties.AutoSearchColumnIndex = 1
		luePPostcode.Properties.NullText = String.Empty
		luePPostcode.EditValue = Nothing

	End Sub


	''' <summary>
	''' Resets the postcode drop down.
	''' </summary>
	Private Sub ResetPostcodeDropDown()

		luePostcode.Properties.SearchMode = SearchMode.OnlyInPopup
		luePostcode.Properties.TextEditStyle = TextEditStyles.Standard

		luePostcode.Properties.DisplayMember = "Postcode"
		luePostcode.Properties.ValueMember = "Postcode"

		Dim columns = luePostcode.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("Postcode", 0, m_Translate.GetSafeTranslationValue("PLZ")))
		columns.Add(New LookUpColumnInfo("Location", 0, m_Translate.GetSafeTranslationValue("Ort")))

		luePostcode.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		luePostcode.Properties.SearchMode = SearchMode.AutoComplete
		luePostcode.Properties.AutoSearchColumnIndex = 1
		luePostcode.Properties.NullText = String.Empty
		luePostcode.EditValue = Nothing
	End Sub


	''' <summary>
	''' Resets the rights drop down data.
	''' </summary>
	Private Sub ResetRightsDropDown()

		lueRights.Properties.DisplayMember = "Bezeichnung"
		lueRights.Properties.ValueMember = "RightProc"

		Dim columns = lueRights.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("Bezeichnung", 0))

		lueRights.Properties.ShowHeader = False
		lueRights.Properties.ShowFooter = False
		lueRights.Properties.DropDownRows = 10
		lueRights.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueRights.Properties.SearchMode = SearchMode.AutoComplete
		lueRights.Properties.AutoSearchColumnIndex = 0
		lueRights.Properties.NullText = String.Empty

		lueRights.EditValue = Nothing

	End Sub



	''' <summary>
	'''  Trannslate controls.
	''' </summary>
	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
		lblHeader.Text = m_Translate.GetSafeTranslationValue(lblHeader.Text)
		lblHeaderDescription.Text = m_Translate.GetSafeTranslationValue(lblHeaderDescription.Text)
		Me.btnClose.Text = m_Translate.GetSafeTranslationValue(Me.btnClose.Text)

		grpAnmeldung.Text = m_Translate.GetSafeTranslationValue(grpAnmeldung.Text)
		lblAnrede.Text = m_Translate.GetSafeTranslationValue(lblAnrede.Text)
		lblNachname.Text = m_Translate.GetSafeTranslationValue(lblNachname.Text)
		lblVorname.Text = m_Translate.GetSafeTranslationValue(lblVorname.Text)
		lblAnmeldename.Text = m_Translate.GetSafeTranslationValue(lblAnmeldename.Text)
		lblPasswort.Text = m_Translate.GetSafeTranslationValue(lblPasswort.Text)

		grpEigenschaften.Text = m_Translate.GetSafeTranslationValue(grpEigenschaften.Text)
		lblSprache.Text = m_Translate.GetSafeTranslationValue(lblSprache.Text)
		lblAblaufdatum.Text = m_Translate.GetSafeTranslationValue(lblAblaufdatum.Text)
		chk_Deaktivieren.Text = m_Translate.GetSafeTranslationValue(chk_Deaktivieren.Text)

		grpKostenteilung.Text = m_Translate.GetSafeTranslationValue(grpKostenteilung.Text)
		lblKostenstelle1.Text = m_Translate.GetSafeTranslationValue(lblKostenstelle1.Text)
		lblKostenstelle2.Text = m_Translate.GetSafeTranslationValue(lblKostenstelle2.Text)
		lblBerater.Text = m_Translate.GetSafeTranslationValue(lblBerater.Text)

		xtabPrivateAddress.Text = m_Translate.GetSafeTranslationValue(xtabPrivateAddress.Text)
		grpPrivateAddress.Text = m_Translate.GetSafeTranslationValue(grpPrivateAddress.Text)
		lblPPostfach.Text = m_Translate.GetSafeTranslationValue(lblPPostfach.Text)
		lblPStrasse.Text = m_Translate.GetSafeTranslationValue(lblPStrasse.Text)
		lblPland.Text = m_Translate.GetSafeTranslationValue(lblPland.Text)
		lblPplz.Text = m_Translate.GetSafeTranslationValue(lblPplz.Text)
		lblPOrt.Text = m_Translate.GetSafeTranslationValue(lblPOrt.Text)
		lblPTitel1.Text = m_Translate.GetSafeTranslationValue(lblPTitel1.Text)
		lblPTitel2.Text = m_Translate.GetSafeTranslationValue(lblPTitel2.Text)

		grpPrivateKommunikation.Text = m_Translate.GetSafeTranslationValue(grpPrivateKommunikation.Text)
		lblPTelefon.Text = m_Translate.GetSafeTranslationValue(lblPTelefon.Text)
		lblPTelefax.Text = m_Translate.GetSafeTranslationValue(lblPTelefax.Text)
		lblPMobile.Text = m_Translate.GetSafeTranslationValue(lblPMobile.Text)
		lblPEMail.Text = m_Translate.GetSafeTranslationValue(lblPEMail.Text)
		lblPHomepage.Text = m_Translate.GetSafeTranslationValue(lblPHomepage.Text)
		lblGeburtsdatum.Text = m_Translate.GetSafeTranslationValue(lblGeburtsdatum.Text)
		lblPAbteilung.Text = m_Translate.GetSafeTranslationValue(lblPAbteilung.Text)

		xtabBusinessAddress.Text = m_Translate.GetSafeTranslationValue(xtabBusinessAddress.Text)
		grpBusinessAddress.Text = m_Translate.GetSafeTranslationValue(grpBusinessAddress.Text)
		lblFirmenname.Text = m_Translate.GetSafeTranslationValue(lblFirmenname.Text)
		lblZusatz1.Text = m_Translate.GetSafeTranslationValue(lblZusatz1.Text)
		lblZusatz2.Text = m_Translate.GetSafeTranslationValue(lblZusatz2.Text)
		lblMDStrasse.Text = m_Translate.GetSafeTranslationValue(lblMDStrasse.Text)
		lblMDLand.Text = m_Translate.GetSafeTranslationValue(lblMDLand.Text)
		lblMDPLZ.Text = m_Translate.GetSafeTranslationValue(lblMDPLZ.Text)
		lblMDOrt.Text = m_Translate.GetSafeTranslationValue(lblMDOrt.Text)

		grpBusinessKommunikation.Text = m_Translate.GetSafeTranslationValue(grpBusinessKommunikation.Text)
		lblMDTelefon.Text = m_Translate.GetSafeTranslationValue(lblMDTelefon.Text)
		lblMDTelefondirekt.Text = m_Translate.GetSafeTranslationValue(lblMDTelefondirekt.Text)
		lblMDTelefax.Text = m_Translate.GetSafeTranslationValue(lblMDTelefax.Text)
		lblMDEMail.Text = m_Translate.GetSafeTranslationValue(lblMDEMail.Text)
		lblMDHomepage.Text = m_Translate.GetSafeTranslationValue(lblMDHomepage.Text)
		lblMDBewilligung.Text = m_Translate.GetSafeTranslationValue(lblMDBewilligung.Text)

		xtabAccount.Text = m_Translate.GetSafeTranslationValue(xtabAccount.Text)
		grpEigenschaften.Text = m_Translate.GetSafeTranslationValue(grpEigenschaften.Text)
		lblExBenutzer.Text = m_Translate.GetSafeTranslationValue(lblExBenutzer.Text)
		lblExPasswort.Text = m_Translate.GetSafeTranslationValue(lblExPasswort.Text)

		grpJobsch.Text = m_Translate.GetSafeTranslationValue(grpJobsch.Text)
		lblJobsKundennummer.Text = m_Translate.GetSafeTranslationValue(lblJobsKundennummer.Text)
		lblJobsLayoutID.Text = m_Translate.GetSafeTranslationValue(lblJobsLayoutID.Text)
		lblJobsLogoID.Text = m_Translate.GetSafeTranslationValue(lblJobsLogoID.Text)

		lblOstJobKundennummer.Text = m_Translate.GetSafeTranslationValue(lblOstJobKundennummer.Text)
		lblOstJobAnzahl.Text = m_Translate.GetSafeTranslationValue(lblOstJobAnzahl.Text)

		btnDeleteGridSetting.Text = m_Translate.GetSafeTranslationValue(btnDeleteGridSetting.Text)

		grpEtikette.Text = m_Translate.GetSafeTranslationValue(grpEtikette.Text)
		lblEtikketendrucker.Text = m_Translate.GetSafeTranslationValue(lblEtikketendrucker.Text)
		chkesunterzeichner_esvertrag.Text = m_Translate.GetSafeTranslationValue(chkesunterzeichner_esvertrag.Text)
		chkshowcustomerrecordsincolor.Text = m_Translate.GetSafeTranslationValue(chkshowcustomerrecordsincolor.Text)
		chkDeleteTempFilesOnLogin.Text = m_Translate.GetSafeTranslationValue(chkDeleteTempFilesOnLogin.Text)

		chkUserAsCostcenter.Text = m_Translate.GetSafeTranslationValue(chkUserAsCostcenter.Text)
		chkLogOnMorePlaces.Text = m_Translate.GetSafeTranslationValue(chkLogOnMorePlaces.Text)

		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)
		Me.bbiSave.Caption = m_Translate.GetSafeTranslationValue(Me.bbiSave.Caption)
		Me.bbiDelete.Caption = m_Translate.GetSafeTranslationValue(Me.bbiDelete.Caption)
		Me.bbiCopy.Caption = m_Translate.GetSafeTranslationValue(Me.bbiCopy.Caption)

	End Sub



	Private Function LoadUserList() As Boolean

		Dim listOfData = m_TablesettingDatabaseAccess.LoadUserData(If(m_UserRight658, Nothing, m_InitializationData.UserData.UserNr))

		If (listOfData Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Benutzerdaten konnten nicht geladen werden."))
			Return False
		End If

		Dim gridData = (From person In listOfData
										Select New UserData With
			 {.Abteilung = person.Abteilung,
				.AktivUntil = person.AktivUntil,
				.Anrede = person.Anrede,
				.ChangedFrom = person.ChangedFrom,
				.ChangedOn = person.ChangedOn,
				.CreatedFrom = person.CreatedFrom,
				.CreatedOn = person.CreatedOn,
				.Deaktiviert = person.Deaktiviert,
				.eMail = person.eMail,
				.EMail_UserName = person.EMail_UserName,
				.EMail_UserPW = person.EMail_UserPW,
				.GebDat = person.GebDat,
				.Homepage = person.Homepage,
				.jch_layoutID = person.jch_layoutID,
				.jch_logoID = person.jch_logoID,
				.JCH_SubID = person.JCH_SubID,
				.KST = person.KST,
				.Land = person.Land,
				.Logged = person.Logged,
				.MDNr = person.MDNr,
				.Nachname = person.Nachname,
				.Natel = person.Natel,
				.Ort = person.Ort,
				.OstJob_ID = person.OstJob_ID,
				.ostjob_Kontingent = person.ostjob_Kontingent,
				.PlanerDb = person.PlanerDb,
				.PLZ = person.PLZ,
				.Postfach = person.Postfach,
				.PW = person.PW,
				.recid = person.recid,
				.Result = person.Result,
				.SecLevel = person.SecLevel,
				.Sprache = person.Sprache,
				.Strasse = person.Strasse,
				.Telefax = person.Telefax,
				.Telefon = person.Telefon,
				.Transfered_Guid = person.Transfered_Guid,
				.US_Name = person.US_Name,
				.USBild = person.USBild,
				.USFiliale = person.USFiliale,
				.USKst1 = person.USKst1,
				.USKst2 = person.USKst2,
				.USLanguage = person.USLanguage,
				.USNr = person.USNr,
				.USRightsTemplate = person.USRightsTemplate,
				.USSign = person.USSign,
				.USTitel_1 = person.USTitel_1,
				.USTitel_2 = person.USTitel_2,
				.Vorname = person.Vorname,
				.AsCostCenter = person.AsCostCenter,
				.LogonMorePlaces = person.LogonMorePlaces
			 }).ToList()

		Dim listDataSource As BindingList(Of UserData) = New BindingList(Of UserData)
		If Not bciShowRecords.Checked AndAlso Not gridData Is Nothing Then
			gridData = gridData.Where(Function(data) data.Deaktiviert = bciShowRecords.Checked).ToList()
		End If

		For Each p In gridData
			listDataSource.Add(p)
		Next
		grdUser.DataSource = listDataSource
		Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), gvUser.RowCount)


		Return Not listOfData Is Nothing

	End Function


	''' <summary>
	''' Loads the Kst1 data.
	''' </summary>
	Private Function LoadKst1DropDownData() As Boolean

		Dim suppressUIEventsState As Boolean = m_SuppressUIEvents
		m_SuppressUIEvents = True

		' Load data
		m_CostCenters = m_CommonDatabaseAccess.LoadCostCenters()

		If m_CostCenters Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kostenstellen konnten nicht geladen werden."))
			Return False
		End If

		' Kst1
		lueKst1.EditValue = Nothing
		lueKst1.Properties.DataSource = m_CostCenters.CostCenter1
		lueKst1.Properties.ForceInitialize()

		' Kst2
		lueKst2.EditValue = Nothing
		lueKst2.Properties.DataSource = Nothing
		lueKst2.Properties.ForceInitialize()


		m_SuppressUIEvents = suppressUIEventsState

		Return True

	End Function

	''' <summary>
	''' Loads the Kst2 drop down data.
	''' </summary>
	Private Sub LoadKst2DropDown()

		If (lueKst1.EditValue Is Nothing OrElse m_CostCenters Is Nothing) Then
			Return
		End If

		Dim kst1Name = lueKst1.EditValue
		Dim kst2Data = m_CostCenters.GetCostCenter2ForCostCenter1(kst1Name)

		' Kst2
		lueKst2.EditValue = Nothing
		lueKst2.Properties.DataSource = kst2Data
		lueKst2.Properties.ForceInitialize()

	End Sub


	''' <summary>
	''' Loads the country drop down data.
	''' </summary>
	'''<returns>Boolean flag indicating success.</returns>
	Private Function LoadCountryDropDownData() As Boolean
		Dim countryData = m_CommonDatabaseAccess.LoadCountryData()

		If (countryData Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Länderdaten konnten nicht geladen werden."))
		End If

		lueCountry.Properties.DataSource = countryData
		lueCountry.Properties.ForceInitialize()

		Return Not countryData Is Nothing
	End Function

	''' <summary>
	''' Loads the country drop down data.
	''' </summary>
	'''<returns>Boolean flag indicating success.</returns>
	Private Function LoadPrivatCountryDropDownData() As Boolean
		Dim countryData = m_CommonDatabaseAccess.LoadCountryData()

		If (countryData Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Länderdaten konnten nicht geladen werden."))
		End If

		luePCountry.Properties.DataSource = countryData
		luePCountry.Properties.ForceInitialize()

		Return Not countryData Is Nothing
	End Function

	''' <summary>
	''' Loads the postcode drop downdata.
	''' </summary>
	'''<returns>Boolean flag indicating success.</returns>
	Private Function LoadPrivatPostcodeDropDownData() As Boolean
		Dim postcodeData = m_CommonDatabaseAccess.LoadPostcodeData()

		If (postcodeData Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Postleizahldaten konnten nicht geladen werden."))
		End If

		luePPostcode.Properties.DataSource = postcodeData
		luePPostcode.Properties.ForceInitialize()

		Return Not postcodeData Is Nothing
	End Function

	''' <summary>
	''' Loads the postcode drop downdata.
	''' </summary>
	'''<returns>Boolean flag indicating success.</returns>
	Private Function LoadPostcodeDropDownData() As Boolean
		Dim postcodeData = m_CommonDatabaseAccess.LoadPostcodeData()

		If (postcodeData Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Postleizahldaten konnten nicht geladen werden."))
		End If

		luePostcode.Properties.DataSource = postcodeData
		luePostcode.Properties.ForceInitialize()

		Return Not postcodeData Is Nothing
	End Function

	''' <summary>
	''' Loads the mandant down data.
	''' </summary>
	'''<returns>Boolean flag indicating success.</returns>
	Private Function LoadMandantDropDownData() As Boolean
		Dim mandantData = m_CommonDatabaseAccess.LoadCompaniesListData()

		If (mandantData Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mandantendaten konnten nicht geladen werden."))
		End If

		lueMandant.Properties.DataSource = mandantData
		lueMandant.Properties.ForceInitialize()

		Return Not mandantData Is Nothing
	End Function

	''' <summary>
	''' Loads the business branches drop down data.
	''' </summary>
	'''<returns>Boolean flag indicating success.</returns>
	Private Function LoadBusinessBranchesDropDown() As Boolean

		Dim availableBusinessBranches = m_CommonDatabaseAccess.LoadBusinessBranchsData()

		If (availableBusinessBranches Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Filialen konnten nicht geladen werden."))
		End If

		lueBusinessBranches.Properties.DataSource = availableBusinessBranches
		lueBusinessBranches.Properties.ForceInitialize()

		Return Not availableBusinessBranches Is Nothing
	End Function

	''' <summary>
	''' Loads language drop down data.
	''' </summary>
	'''<returns>Boolean flag indicating success.</returns>
	Private Function LoadLanguageDropDownData() As Boolean
		Dim languageData = m_CommonDatabaseAccess.LoadLanguageData()

		If (languageData Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Sprachen konnten nicht geladen werden."))
		End If

		lueLanguage.Properties.DataSource = languageData
		lueLanguage.Properties.ForceInitialize()

		Return Not languageData Is Nothing
	End Function

	''' <summary>
	''' Loads the salutation and salutation form drop downdata.
	''' </summary>
	''' <returns>Boolean value indicating success.</returns>
	Private Function LoadSalutationAndSalutationFormDropDownData() As Boolean
		Dim m_SalutationData = m_CommonDatabaseAccess.LoadSalutationData()

		If (m_SalutationData Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(m_Translate.GetSafeTranslationValue("Anredeformen konnten nicht geladen werden.")))
		End If

		lueSalutation.Properties.DataSource = m_SalutationData
		lueSalutation.Properties.ForceInitialize()

		Return Not m_SalutationData Is Nothing
	End Function


	''' <summary>
	''' Loads the rights group drop down data.
	''' </summary>
	Private Function LoadRightsDropDownData() As Boolean
		Dim data = m_TablesettingDatabaseAccess.LoadRightsData()

		If (data Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Vorlage für Benutzer-Rechte konnten nicht geladen werden."))
		End If

		lueRights.Properties.DataSource = data
		lueRights.Properties.ForceInitialize()

		Return Not data Is Nothing
	End Function


	Private Sub LoadUserImage()
		Dim data = SelectedExitingRecord

		If data Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Bild-Daten konnten nicht geladen werden.")

			Return
		End If

		Dim img = data.USBild
		If Not img Is Nothing AndAlso data.USBild.Length > 0 Then
			Try
				Dim memoryStream As New System.IO.MemoryStream(img)
				pePicture.Image = Image.FromStream(memoryStream)
				pePicture.Image = Image.FromStream(memoryStream)

				Return

			Catch ex As Exception

			End Try
		End If

		pePicture.Image = Nothing
		pePicture.Properties.NullText = m_Translate.GetSafeTranslationValue("Kein Foto vorhanden!")
		pePicture.Image = Nothing
		pePicture.Properties.NullText = m_Translate.GetSafeTranslationValue("Keine Unterschrift vorhanden!")

	End Sub

	Private Sub LoadpeSign()
		Dim data = SelectedExitingRecord

		If data Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Unterschrift-Daten konnten nicht geladen werden.")

			Return
		End If

		Dim img = data.USSign
		If Not img Is Nothing AndAlso data.USSign.Length > 0 Then
			Try
				Dim memoryStream As New System.IO.MemoryStream(img)
				peSign.Image = Image.FromStream(memoryStream)

				Return

			Catch ex As Exception

			End Try
		End If

		peSign.Image = Nothing
		peSign.Properties.NullText = m_Translate.GetSafeTranslationValue("Keine Unterschrift vorhanden!")

	End Sub


	Private Function LoadUserDocumentList() As Boolean

		Dim success As Boolean = True

		m_selectedUserData = SelectedExitingRecord

		If Not m_selectedUserData Is Nothing Then

			ucUserDocumentRights.MandantenNumber = m_selectedUserData.MDNr
			ucUserDocumentRights.UserNumber = m_selectedUserData.USNr
			Dim userdatalist As BindingList(Of UserData) = New BindingList(Of UserData)

			userdatalist = CType(grdUser.DataSource, BindingList(Of UserData))
			If userdatalist Is Nothing Then Return False

			ucUserDocumentRights.UserDataList = userdatalist.Where(Function(data) data.MDNr = m_selectedUserData.MDNr And data.USNr <> m_selectedUserData.USNr).ToList()

			ucUserDocumentRights.InitWithConfigurationData(m_InitializationData, m_Translate, m_InitializationData.MDData.MDYear)
			ucUserDocumentRights.Dock = DockStyle.Fill
			ucUserDocumentRights.Visible = False

			success = success AndAlso ucUserDocumentRights.LoadDocData()
			ucUserDocumentRights.Visible = success
			If success Then ucUserDocumentRights.Dock = DockStyle.Fill

		Else
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Dokumenten-Daten konnten nicht geladen werden."))
			success = False

		End If

		Return success

	End Function



	''' <summary>
	''' Handles change of KST1.
	''' </summary>
	Private Sub OnLueKst1_EditValueChanged(sender As Object, e As EventArgs) Handles lueKst1.EditValueChanged

		'If m_SuppressUIEvents Then
		'	Return
		'End If

		LoadKst2DropDown()
	End Sub

	''' <summary>
	''' Handles change of private postfach.
	''' </summary>
	Private Sub OnluePPostcode_EditValueChanged(sender As Object, e As EventArgs) Handles luePPostcode.EditValueChanged

		If m_SuppressUIEvents Then
			Return
		End If

		If Not luePostcode.EditValue Is Nothing Then
			Dim postCodeData As PostCodeData = TryCast(luePPostcode.GetSelectedDataRow(), PostCodeData)

			If Not postCodeData Is Nothing Then
				txt_p_Ort.EditValue = postCodeData.Location
			End If

		End If

	End Sub

	''' <summary>
	''' Handles change of postfach.
	''' </summary>
	Private Sub OnluePostcode_EditValueChanged(sender As Object, e As EventArgs) Handles luePostcode.EditValueChanged

		If m_SuppressUIEvents Then
			Return
		End If

		If Not luePostcode.EditValue Is Nothing Then
			Dim postCodeData As PostCodeData = TryCast(luePostcode.GetSelectedDataRow(), PostCodeData)

			If Not postCodeData Is Nothing Then
				txt_Ort.EditValue = postCodeData.Location
			End If

		End If

	End Sub

	''' <summary>
	''' Handles change of user rights templates.
	''' </summary>
	Private Sub OnlueRights_EditValueChanged(sender As Object, e As EventArgs) Handles lueRights.EditValueChanged

		If m_SuppressUIEvents Then
			Return
		End If

		If Not lueRights.EditValue Is Nothing Then
			Dim data As RightsData = TryCast(lueRights.GetSelectedDataRow(), RightsData)

			If Not data Is Nothing Then
				SaveUserRightsWithTemplateData()
			End If

		End If

	End Sub


	''' <summary>
	''' Handles focus click of row.
	''' </summary>
	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		'm_selectedLohnartData = SelectedExitingRecord

		'If Not m_selectedLohnartData Is Nothing Then
		'	Dim success = LoadSelectedDetailData(m_selectedLohnartData.recid)

		'	If Not success Then
		'		m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht geladen werden."))
		'	End If

		'End If

	End Sub

	''' <summary>
	''' Handles focus change of row.
	''' </summary>
	Sub Ongv_FocusedRowChanged(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs)


		m_selectedUserData = SelectedExitingRecord

		If Not m_selectedUserData Is Nothing Then
			Dim success = LoadSelectedDetailData(m_selectedUserData.recid)

			If Not success Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht geladen werden."))
			End If

		End If

	End Sub

	''' <summary>
	''' Handles drop down button clicks.
	''' </summary>
	Private Sub OnDropDown_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

		Const ID_OF_DELETE_BUTTON As Int32 = 1

		' If delete button has been clicked reset the drop down.
		If e.Button.Index = ID_OF_DELETE_BUTTON Then

			If TypeOf sender Is LookUpEdit Then
				Dim lookupEdit As LookUpEdit = CType(sender, LookUpEdit)
				lookupEdit.EditValue = Nothing
			End If
		End If
	End Sub

	''' <summary>
	''' Loads founded detail data.
	''' </summary>
	Private Function LoadSelectedDetailData(ByVal recid As Integer?) As Boolean
		Dim success As Boolean = True
		Dim sp_Utility As New SPProgUtility.MainUtilities.Utilities

		Dim suppressUIEventsState As Boolean = m_SuppressUIEvents
		m_SuppressUIEvents = True
		errorProviderMangement.ClearErrors()

		Dim data = m_TablesettingDatabaseAccess.LoadAssignedUserData(recid)

		If data Is Nothing Then
			m_SuppressUIEvents = suppressUIEventsState

			Return False
		End If

		Dim printers = System.Drawing.Printing.PrinterSettings.InstalledPrinters
		For Each p In printers
			cboUserMatrixPrinter.Properties.Items.Add(p.ToString)
		Next
		m_MandantSettingsXml = New SPProgUtility.CommonXmlUtility.SettingsXml(m_MandantXMLFile)

		cboUserMatrixPrinter.EditValue = LoadLabelPrinter

		grpAnmeldung.Text = String.Format(m_Translate.GetSafeTranslationValue("Stammdaten: {0}"), data.USNr)

		lueSalutation.EditValue = data.Anrede
		txt_Nachname.EditValue = data.Nachname
		txt_Vorname.EditValue = data.Vorname

		Dim strEnteredName = DecryptWithClipper(UCase(data.US_Name), strEncryptionKey)
		Dim strEnteredPass = DecryptWithClipper(data.PW & strExtraPass, strEncryptionKey)

		txt_Anmeldename.EditValue = strEnteredName
		txt_Passwort.EditValue = strEnteredPass.Split(strExtraPass)(0)

		lueLanguage.EditValue = data.USLanguage
		deUntil.EditValue = data.AktivUntil
		chk_Deaktivieren.Checked = data.Deaktiviert

		lueKst1.EditValue = data.USKst1
		lueKst2.EditValue = data.USKst2
		txt_Kst.EditValue = data.KST

		lueMandant.EditValue = data.MDNr
		lueBusinessBranches.EditValue = data.USFiliale
		lueRights.EditValue = data.USRightsTemplate

		txt_p_Postfach.EditValue = data.Postfach
		txt_p_Strasse.EditValue = data.Strasse
		luePCountry.EditValue = data.Land
		luePPostcode.EditValue = data.PLZ
		txt_p_Ort.EditValue = data.Ort
		txt_p_Titel1.EditValue = data.USTitel_1
		txt_p_Titel2.EditValue = data.USTitel_2
		de_p_GebDate.EditValue = data.GebDat

		txt_p_Telefax.EditValue = data.Telefax
		txt_p_Telefon.EditValue = data.Telefon
		txt_p_EMail.EditValue = data.eMail
		txt_p_homepage.EditValue = data.Homepage
		txt_p_Mobile.EditValue = data.Natel
		txt_p_Abteilung.EditValue = data.Abteilung

		Dim exchangeUserName = data.EMail_UserName
		Dim exchangePassword = data.EMail_UserPW
		If Not String.IsNullOrWhiteSpace(exchangeUserName) Then exchangeUserName = Decrypt(exchangeUserName)
		If Not String.IsNullOrWhiteSpace(exchangePassword) Then exchangePassword = Decrypt(exchangePassword)

		txt_Ex_Benutzer.EditValue = exchangeUserName
		txt_Ex_Passwort.EditValue = exchangePassword

		txt_Jobs_KDNr.EditValue = data.JCH_SubID
		txt_Jobs_LayoutID.EditValue = data.jch_layoutID
		lue_Jobs_LogoID.EditValue = data.jch_logoID

		lue_OstJob_KDNr.EditValue = data.OstJob_ID
		txt_OstJob_Anzahl.EditValue = data.ostjob_Kontingent
		chkesunterzeichner_esvertrag.Checked = SignAsEmploymentSigner
		chkshowcustomerrecordsincolor.Checked = ShowCustomerRecordsInColor
		chkDeleteTempFilesOnLogin.Checked = DeleteUserTempFilesOnLogin
		chkPrintCustomerData.Checked = PrintCusotmerDataOnReportsContentTemplate

		chkUserAsCostcenter.Checked = data.AsCostCenter
		chkLogOnMorePlaces.Checked = data.LogonMorePlaces


		bsiCreated.Caption = String.Format("{0:G}, {1}", data.CreatedOn, data.CreatedFrom)
		bsiChanged.Caption = String.Format("{0:G}, {1}", data.ChangedOn, data.ChangedFrom)

		LoadUserImage()
		LoadpeSign()

		success = success AndAlso LoadUserMDAddress(data.USNr)
		success = success AndAlso LoadUserDocumentList()

		m_CurrentRecordNumber = data.recid
		m_UserNumbertoCopy = Nothing

		pePicture.Enabled = Not m_CurrentRecordNumber Is Nothing
		peSign.Enabled = Not m_CurrentRecordNumber Is Nothing

		xtabUserDocrights.PageEnabled = Not m_CurrentRecordNumber Is Nothing
		bbiDelete.Enabled = Not m_CurrentRecordNumber Is Nothing
		bbiCopy.Enabled = (m_InitializationData.UserData.UserNr = 1) AndAlso (Not m_CurrentRecordNumber Is Nothing)
		bbiUserRights.Enabled = m_UserRight656 AndAlso Not m_CurrentRecordNumber Is Nothing

		If m_InitializationData.UserData.UserNr <> 1 Then
			bbiSave.Enabled = (data.USNr <> 1)
			bbiDelete.Enabled = (data.USNr <> 1)
		End If

		Dim strXMLFile As String = m_mandant.GetSelectedMDUserProfileXMLFilename(data.MDNr, data.USNr)
		If Not File.Exists(strXMLFile) Then
			Dim utility As New SPProgUtility.MainUtilities.Utilities
			If Directory.Exists(m_mandant.GetSelectedMDTemplatePath(lueMandant.EditValue)) Then
				utility.CreateUserProfileXMLFile(m_InitializationData.MDData.MDDbConn, data.USNr, lueMandant.EditValue)

			Else
				m_Logger.LogError(String.Format("MDUserProfilesPath: {0} was not founded!", m_InitializationData.MDData.MDUserProfilesPath))
			End If
		End If


		m_SuppressUIEvents = suppressUIEventsState


		Return success

	End Function

	Private Function LoadUserMDAddress(ByVal userNumber As Integer) As Boolean
		Dim success As Boolean = True

		Dim data = m_TablesettingDatabaseAccess.LoadAssignedUserAddressData(userNumber)

		If data Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Adress-Daten konnten nicht geladen werden."))

			Return False
		End If

		txt_MDName.EditValue = data.MD_Name1
		txt_MDName2.EditValue = data.MD_Name2
		txt_MDName3.EditValue = data.MD_Name3
		txt_Postfach.EditValue = data.MD_Postfach
		txt_Strasse.EditValue = data.MD_Strasse
		lueCountry.EditValue = data.MD_Land
		luePostcode.EditValue = data.MD_PLZ
		txt_Ort.EditValue = data.MD_Ort

		txt_Telefon.EditValue = data.MD_Telefon
		txt_dTelefon.EditValue = data.MD_DTelefon
		txt_Telefax.EditValue = data.MD_Telefax
		txt_Email.EditValue = data.MD_eMail
		txt_Homepage.EditValue = data.MD_Homepage
		txt_Bewilligung.EditValue = data.MD_Bewilligung

		m_CurrentAddressRecordNumber = data.recid


		Return True

	End Function

	Private Function SaveUserData() As Boolean
		Dim success As Boolean = True
		Dim successUSRights As Boolean = True

		success = success AndAlso ValidateInputData()
		If Not success Then Return success
		Try

			Dim data = SelectedExitingRecord
			Dim addressdata As UserAddressData = Nothing
			'data = New UserData
			addressdata = New UserAddressData

			data.recid = m_CurrentRecordNumber.GetValueOrDefault(0)
			If data Is Nothing Then
				data.CreatedFrom = m_InitializationData.UserData.UserFullName
				data.MDNr = m_InitializationData.MDData.MDNr
				data.USNr = m_InitializationData.UserData.UserNr
			End If
			data.ChangedFrom = m_InitializationData.UserData.UserFullName

			data.Anrede = lueSalutation.EditValue
			data.Nachname = txt_Nachname.EditValue.ToString.Trim
			data.Vorname = txt_Vorname.EditValue.ToString.Trim

			Dim strEnteredName = EncryptMyString(UCase(Me.txt_Anmeldename.Text.Trim), strEncryptionKey)
			Dim strEnteredPass = EncryptMyString(Me.txt_Passwort.Text.Trim & strExtraPass, strEncryptionKey)
			data.US_Name = strEnteredName
			data.PW = strEnteredPass

			Dim converter As New ImageConverter
			data.USBild = converter.ConvertTo(pePicture.Image, GetType(Byte()))
			data.USSign = converter.ConvertTo(peSign.Image, GetType(Byte()))

			data.Sprache = lueLanguage.EditValue
			data.USLanguage = lueLanguage.EditValue
			data.AktivUntil = deUntil.EditValue
			data.Deaktiviert = chk_Deaktivieren.Checked

			data.USKst1 = lueKst1.EditValue
			data.USKst2 = lueKst2.EditValue
			data.KST = txt_Kst.EditValue.ToString.Trim

			data.MDNr = lueMandant.EditValue
			data.USFiliale = lueBusinessBranches.EditValue
			data.USRightsTemplate = lueRights.EditValue

			data.Postfach = txt_p_Postfach.EditValue
			data.Strasse = txt_p_Strasse.EditValue
			data.Land = luePCountry.EditValue
			data.PLZ = luePPostcode.EditValue
			data.Ort = txt_p_Ort.EditValue
			data.USTitel_1 = txt_p_Titel1.EditValue
			data.USTitel_2 = txt_p_Titel2.EditValue
			data.GebDat = de_p_GebDate.EditValue

			data.Telefax = txt_p_Telefax.EditValue
			data.Telefon = txt_p_Telefon.EditValue
			data.eMail = txt_p_EMail.EditValue
			data.Homepage = txt_p_homepage.EditValue
			data.Natel = txt_p_Mobile.EditValue
			data.Abteilung = txt_p_Abteilung.EditValue

			data.AsCostCenter = chkUserAsCostcenter.Checked
			data.LogonMorePlaces = chkLogOnMorePlaces.Checked

			Dim exchangeUserName = txt_Ex_Benutzer.EditValue
			Dim exchangePassword = txt_Ex_Passwort.EditValue
			If Not String.IsNullOrWhiteSpace(exchangeUserName) Then exchangeUserName = Encrypt(exchangeUserName)
			If Not String.IsNullOrWhiteSpace(exchangePassword) Then exchangePassword = Encrypt(exchangePassword)

			data.EMail_UserName = exchangeUserName
			data.EMail_UserPW = exchangePassword

			data.JCH_SubID = txt_Jobs_KDNr.EditValue
			data.jch_layoutID = txt_Jobs_LayoutID.EditValue
			data.jch_logoID = lue_Jobs_LogoID.EditValue


			data.OstJob_ID = lue_OstJob_KDNr.EditValue
			data.ostjob_Kontingent = If(String.IsNullOrWhiteSpace(data.OstJob_ID), 0, txt_OstJob_Anzahl.Value)

			data.ChangedFrom = m_InitializationData.UserData.UserFullName

			addressdata.recid = m_CurrentAddressRecordNumber.GetValueOrDefault(0)
			addressdata.USNr = data.USNr
			addressdata.MD_Name1 = txt_MDName.EditValue
			addressdata.MD_Name2 = txt_MDName2.EditValue
			addressdata.MD_Name3 = txt_MDName3.EditValue
			addressdata.MD_Postfach = txt_Postfach.EditValue
			addressdata.MD_Strasse = txt_Strasse.EditValue
			addressdata.MD_Land = lueCountry.EditValue
			addressdata.MD_PLZ = luePostcode.EditValue
			addressdata.MD_Ort = txt_Ort.EditValue
			addressdata.MD_Telefax = txt_Telefax.EditValue
			addressdata.MD_Telefon = txt_Telefon.EditValue
			addressdata.MD_DTelefon = txt_dTelefon.EditValue
			addressdata.MD_eMail = txt_Email.EditValue
			addressdata.MD_Homepage = txt_Homepage.EditValue
			addressdata.MD_Bewilligung = txt_Bewilligung.EditValue
			addressdata.ChangedFrom = m_InitializationData.UserData.UserFullName

			Dim rightTemplatedata = TryCast(lueRights.GetSelectedDataRow(), RightsData)

			If m_CurrentRecordNumber.GetValueOrDefault(0) = 0 Then
				' new record
				success = m_TablesettingDatabaseAccess.AddUserData(data)
				Dim newRecid = data.recid
				success = success AndAlso newRecid > 0 AndAlso m_TablesettingDatabaseAccess.AddUserAddressData(newRecid, addressdata)

				If Not rightTemplatedata Is Nothing Then
					successUSRights = success AndAlso newRecid > 0 AndAlso m_TablesettingDatabaseAccess.SaveUSRightsWithSelectedTemplates(lueMandant.EditValue, data.USNr, rightTemplatedata.RightProc)
				Else
					If success AndAlso newRecid > 0 Then
						Dim userRightsDataforCopy = m_TablesettingDatabaseAccess.LoadAssignedUserRightsData(m_UserNumbertoCopy.GetValueOrDefault(0), lueMandant.EditValue, String.Empty)
						For Each rights In userRightsDataforCopy
							rights.ChangedFrom = m_InitializationData.UserData.UserFullName
							successUSRights = successUSRights AndAlso m_TablesettingDatabaseAccess.CopyUserRightsFromAnotherUser(data.USNr, rights)
						Next

					End If
				End If

			Else
				success = m_TablesettingDatabaseAccess.UpdateAssignedUserPictureAndSignData(data)
				success = success AndAlso m_TablesettingDatabaseAccess.UpdateAssignedUserData(data)
				success = success AndAlso m_TablesettingDatabaseAccess.UpdateAssignedUserAddressData(addressdata)

			End If

			If success Then
				Dim strXMLFile As String = m_mandant.GetSelectedMDUserProfileXMLFilename(data.MDNr, data.USNr)
				If Not File.Exists(strXMLFile) Then
					Dim utility As New SPProgUtility.MainUtilities.Utilities
					utility.CreateUserProfileXMLFile(m_InitializationData.MDData.MDDbConn, data.USNr, lueMandant.EditValue)
				End If
				Dim msg = m_Translate.GetSafeTranslationValue("Die Daten wurden gespeichert.")
				If m_CurrentRecordNumber.GetValueOrDefault(0) = 0 Then
					If Not successUSRights Then
						msg &= vbNewLine & m_Translate.GetSafeTranslationValue("Achtung: Der neu erfasste Benutzer besitzt keine Rechte. Bitte erstellen Sie Benutzerrechte ein!")
					ElseIf Not rightTemplatedata Is Nothing Then
						msg &= vbNewLine & m_Translate.GetSafeTranslationValue("Achtung: Die Benutzerrechte wurden gemäss ausgewählte Vorlage gespeichert. Bitte erstellen Sie Benutzerrechte ein!")
					End If

				End If
				m_UtilityUI.ShowInfoDialog(msg)
				If m_CurrentRecordNumber.GetValueOrDefault(0) = 0 AndAlso LoadUserData() Then FocusUserData(data.USNr)
				m_UserNumbertoCopy = Nothing

				xtabUserDocrights.PageEnabled = Not m_CurrentRecordNumber Is Nothing
				bbiDelete.Enabled = Not m_CurrentRecordNumber Is Nothing
				bbiCopy.Enabled = (m_InitializationData.UserData.UserNr = 1) AndAlso (Not m_CurrentRecordNumber Is Nothing)
				bbiUserRights.Enabled = m_UserRight656 AndAlso Not m_CurrentRecordNumber Is Nothing

				' user sign for employment 
				SaveUSSignSetting(data)

			Else
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))

			End If
			pePicture.Enabled = True
			peSign.Enabled = True


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

			success = False
		End Try


		Return success

	End Function

	Private Function SaveUserRightsWithTemplateData() As Boolean
		Dim success As Boolean = True
		If m_CurrentRecordNumber.GetValueOrDefault(0) = 0 Then Return False
		Dim data = TryCast(lueRights.GetSelectedDataRow(), RightsData)
		Dim userData = SelectedExitingRecord

		If Not data Is Nothing AndAlso Not userData Is Nothing Then

			Dim result = m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Möchten Sie wirklich die Benutzerrechte komplett gemäss Vorlage überschreiben?"),
																				 m_Translate.GetSafeTranslationValue("Benutzerrechte übernehmen?"))
			If result Then
				success = m_TablesettingDatabaseAccess.SaveUSRightsWithSelectedTemplates(lueMandant.EditValue, userData.USNr, data.RightProc)
				success = success AndAlso SaveUserData()
			Else
				success = False
			End If

			If success Then
				m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Die Benutzerrechte wurden erfolgreich gespeichert."))

			Else
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Benutzerrechte konnten nicht gespeichert werden."))
				If Not userData Is Nothing Then
					lueRights.EditValue = userData.USRightsTemplate
				End If

			End If

		End If


		Return success

	End Function

	Private Function DeleteUserData() As Boolean
		Dim result As DeleteUserResult
		Dim success As Boolean = True

		Dim selectedData = SelectedExitingRecord

		If Not selectedData Is Nothing Then
			If selectedData.USNr = 1 Then Return False

			If (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Möchten Sie die ausgewählen Daten wirklich löschen?"),
																m_Translate.GetSafeTranslationValue("Daten endgültig löschen?"))) Then

				result = m_TablesettingDatabaseAccess.DeleteUserData(selectedData.recid, m_InitializationData.UserData.UserNr)
			Else
				Return False

			End If

		End If
		success = success AndAlso (result = DeleteUserResult.ResultDeleteOk)

		If Not success Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Daten konnte nicht gelöscht werden."))
		End If

		If success AndAlso LoadUserData() Then
			FocusUserData(selectedData.USNr)
		End If

		Return success

	End Function

	Private Sub OnbciShowRecords_ItemClick(sender As Object, e As ItemClickEventArgs) Handles bciShowRecords.ItemClick
		Dim success = LoadUserList()
	End Sub

	Private Sub OnbbiSave_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSave.ItemClick

		Dim success = SaveUserData()
		If success Then SendLoginDataViaWebService()

	End Sub

	Private Sub OnbbiDelete_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiDelete.ItemClick

		' first user must be deactivated on web database
		Dim success = PerformDeletingLoginDataToWebService()
		success = success AndAlso DeleteUserData()

	End Sub

	Private Sub OnbbiCopy_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiCopy.ItemClick
		Dim oldIDNumber = m_CurrentRecordNumber

		PrepareForNew()
		Dim data = SelectedExitingRecord
		If Not data Is Nothing Then
			m_UserNumbertoCopy = SelectedExitingRecord.USNr
		End If

		pePicture.Enabled = Not m_CurrentRecordNumber Is Nothing
		peSign.Enabled = Not m_CurrentRecordNumber Is Nothing

	End Sub

	Private Sub btnDeleteGridSetting_Click(sender As Object, e As EventArgs) Handles btnDeleteGridSetting.Click
		Dim folderToSearch As String = m_mandant.GetGridSettingPath(m_InitializationData.MDData.MDNr)

		Dim msg As String = m_Translate.GetSafeTranslationValue("Hiermit werden alle Einstellungsdateien gelöscht! Sind Sie sicher?")
		If Not m_UtilityUI.ShowYesNoDialog(msg, "Einstellungen zurücksetzen?") Then Return

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim success = SearchInFolder(folderToSearch)
			If success Then

				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowInfoDialog("Die Einstellungsdaten wurden erfolgreich gelöscht. Bitte starten Sie das Programm neu.")

			End If

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Der Vorgang konnte nicht erfolgreich abgeschlossen werden."))
			m_Logger.LogError(ex.ToString)

		Finally
			SplashScreenManager.CloseForm(False)
		End Try


	End Sub


	Private Sub OnpePicture_ContextButtonClick(sender As Object, e As DevExpress.Utils.ContextItemClickEventArgs) Handles pePicture.Properties.ContextButtonClick
		Const ID_OF_ADD_BUTTON As String = "itemDownload"
		Const ID_OF_DELETE_BUTTON As String = "itemRemove"

		If e.Item.Name.ToUpper = ID_OF_ADD_BUTTON.ToUpper Then
			Dim m_path As New SPProgUtility.ProgPath.ClsProgPath

			Dim filename As String = String.Empty
			Dim openFileDialog1 As New OpenFileDialog()
			openFileDialog1.Filter = "JPEG-Dateien (*.jpeg)|*.jpg;*.jepeg"
			openFileDialog1.InitialDirectory = m_path.GetSpS2DeleteHomeFolder

			Dim result = openFileDialog1.ShowDialog()
			If result = DialogResult.Cancel Then
				Return
			Else
				filename = openFileDialog1.FileName
			End If

			Try
				If Not m_selectedUserData Is Nothing Then
					Dim strFullFilename As String = Path.Combine(m_path.GetSpS2DeleteHomeFolder, String.Format("USPicture_{0}_{1}.JPG", m_selectedUserData.MDNr, m_selectedUserData.Transfered_Guid))
					File.Delete(strFullFilename)
				End If

				pePicture.Image = Image.FromFile(filename)
				Return

			Catch ex As Exception
				pePicture.Image = Nothing
			End Try

		ElseIf e.Item.Name.ToUpper = ID_OF_DELETE_BUTTON.ToUpper Then
			pePicture.Image = Nothing

		End If

	End Sub

	Private Sub peSign_ContextButtonClick(sender As Object, e As DevExpress.Utils.ContextItemClickEventArgs) Handles peSign.Properties.ContextButtonClick
		' If delete button has been clicked reset the drop down.
		Const ID_OF_ADD_BUTTON As String = "itemDownload"
		Const ID_OF_DELETE_BUTTON As String = "itemRemove"

		If e.Item.Name.ToUpper = ID_OF_ADD_BUTTON.ToUpper Then
			Dim m_path As New SPProgUtility.ProgPath.ClsProgPath

			Dim filename As String = String.Empty
			Dim openFileDialog1 As New OpenFileDialog()
			openFileDialog1.Filter = "JPEG-Dateien (*.jpeg)|*.jpg;*.jepeg"
			openFileDialog1.InitialDirectory = m_path.GetSpS2DeleteHomeFolder

			Dim result = openFileDialog1.ShowDialog()
			If result = DialogResult.Cancel Then
				Return
			Else
				filename = openFileDialog1.FileName
			End If

			Try
				If Not m_selectedUserData Is Nothing Then
					Dim strFullFilename As String = Path.Combine(m_path.GetSpS2DeleteHomeFolder, String.Format("Bild_{0}_{1}.JPG", m_selectedUserData.MDNr, m_selectedUserData.Transfered_Guid))
					File.Delete(strFullFilename)
				End If

				peSign.Image = Image.FromFile(filename)
				Return

			Catch ex As Exception
				peSign.Image = Nothing

			End Try

		ElseIf e.Item.Name.ToUpper = ID_OF_DELETE_BUTTON.ToUpper Then
			peSign.Image = Nothing

		End If

	End Sub

	Private Sub OnbbiUserRights_ItemClick(sender As Object, e As ItemClickEventArgs) Handles bbiUserRights.ItemClick
		Dim frm = New frmUserRights(m_InitializationData)
		Dim usdata = SelectedExitingRecord

		If Not usdata Is Nothing Then
			frm.m_CurrentUserID = usdata.recid
			frm.m_CurrentMandantNumber = lueMandant.EditValue

			Dim userdatalist As BindingList(Of UserData) = New BindingList(Of UserData)

			userdatalist = CType(grdUser.DataSource, BindingList(Of UserData))
			If userdatalist Is Nothing Then Return
			frm.UserDataList = userdatalist.Where(Function(data) data.MDNr = m_selectedUserData.MDNr And data.USNr <> m_selectedUserData.USNr And data.USNr <> 1).ToList()

			frm.LoadUserRightData()
			frm.Show()
			frm.BringToFront()

		End If

	End Sub

	Private Sub lueRights_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles lueRights.ButtonClick

		If e.Button.Index = 1 Then
			SaveUserRightsWithTemplateData()
		End If

	End Sub

	Private Sub FocusUserData(ByVal userNumber As Integer)

		If Not grdUser.DataSource Is Nothing Then

			Dim esSalaryData = CType(gvUser.DataSource, BindingList(Of UserData))

			Dim index = esSalaryData.ToList().FindIndex(Function(data) data.USNr = userNumber)

			Dim rowHandle = gvUser.GetRowHandle(index)
			gvUser.FocusedRowHandle = rowHandle

		End If

	End Sub

	Private Sub gvUser_MouseDown(sender As Object, e As MouseEventArgs) Handles gvUser.MouseDown

		If e.Clicks = 2 AndAlso m_InitializationData.UserData.UserNr = 1 AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown) Then
			Dim data = SelectedExitingRecord
			If data Is Nothing Then Return

			Dim strEnteredName = DecryptWithClipper(UCase(data.US_Name), strEncryptionKey)
			Dim strEnteredPassword = DecryptWithClipper(data.PW & strExtraPass, strEncryptionKey)
			Dim exchangeUsername As String = Decrypt(data.EMail_UserName)
			Dim exchangePassword As String = Decrypt(data.EMail_UserPW)

			Dim msg = "Benutzername: {1}{0}Passwort: {2}{0}{0}Exchange-Benutzer: {3}{0}Exchange-Passwort: {4}"
			msg = String.Format(m_Translate.GetSafeTranslationValue(msg), vbNewLine, strEnteredName, strEnteredPassword, exchangeUsername, exchangePassword)

			m_UtilityUI.ShowInfoDialog(msg)
		End If

	End Sub

	Private Sub OngvUser_RowCellStyle(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs) Handles gvUser.RowCellStyle

		If (e.RowHandle >= 0) Then
			Dim view As GridView = CType(sender, GridView)
			Dim data = CType(view.GetRow(e.RowHandle), UserData)

			If data.Deaktiviert Then e.Appearance.BackColor = Color.PaleVioletRed

		End If

	End Sub



	''' <summary>
	''' Validates input data.
	''' </summary>
	Private Function ValidateInputData() As Boolean

		errorProviderMangement.ClearErrors()

		Dim errorText As String = "Bitte geben Sie einen Wert ein."
		Dim errorKSTDuplicate As String = "Die Daten sind bereits vorhanden."

		Dim isValid As Boolean = True

		Try

			isValid = isValid And SetErrorIfInvalid(lueSalutation, errorProviderMangement, lueSalutation.EditValue Is Nothing, errorText)
			isValid = isValid And SetErrorIfInvalid(txt_Nachname, errorProviderMangement, Len(txt_Nachname.EditValue) <= 0, errorText)
			isValid = isValid And SetErrorIfInvalid(txt_Vorname, errorProviderMangement, Len(txt_Vorname.EditValue) <= 0, errorText)
			isValid = isValid And SetErrorIfInvalid(txt_Anmeldename, errorProviderMangement, Len(txt_Anmeldename.EditValue) <= 0, errorText)
			isValid = isValid And SetErrorIfInvalid(txt_Passwort, errorProviderMangement, Len(txt_Passwort.EditValue) <= 0, errorText)

			isValid = isValid And SetErrorIfInvalid(lueLanguage, errorProviderMangement, Len(lueLanguage.EditValue) <= 0, errorText)
			isValid = isValid And SetErrorIfInvalid(txt_Kst, errorProviderMangement, Len(txt_Kst.EditValue) <= 0, errorText)

			If isValid Then

				Dim allUserData = m_TablesettingDatabaseAccess.LoadUserData(Nothing)
				'allUserData = CType(grdUser.DataSource, BindingList(Of UserData))
				isValid = isValid AndAlso Not allUserData Is Nothing
				Dim myuserdatalist As New List(Of UserData)

				If m_CurrentRecordNumber.GetValueOrDefault(0) = 0 Then
					isValid = isValid And SetErrorIfInvalid(lueRights, errorProviderMangement, m_UserNumbertoCopy Is Nothing AndAlso lueRights.EditValue = Nothing, errorText)

					myuserdatalist = allUserData.Where(Function(data) data.MDNr <> 0 And UCase(data.KST) = UCase(txt_Kst.EditValue)).ToList()
					If Not myuserdatalist Is Nothing Then
						isValid = isValid And SetErrorIfInvalid(txt_Kst, errorProviderMangement, (myuserdatalist.Count > 0), errorKSTDuplicate)
					End If

					Dim strEnteredName = EncryptMyString(UCase(Me.txt_Anmeldename.Text), strEncryptionKey)
					myuserdatalist = allUserData.Where(Function(data) data.MDNr <> 0 And data.US_Name = strEnteredName).ToList()
					If Not myuserdatalist Is Nothing Then
						isValid = isValid And SetErrorIfInvalid(txt_Anmeldename, errorProviderMangement, (myuserdatalist.Count > 0), errorKSTDuplicate)
					End If

				Else
					Dim currentUserData = SelectedExitingRecord
					If Not currentUserData Is Nothing Then
						myuserdatalist = allUserData.Where(Function(data) data.MDNr <> 0 And data.USNr <> currentUserData.USNr And UCase(data.KST) = UCase(txt_Kst.EditValue)).ToList()
						If Not myuserdatalist Is Nothing Then
							isValid = isValid And SetErrorIfInvalid(txt_Kst, errorProviderMangement, (myuserdatalist.Count > 0), errorKSTDuplicate)
						End If
						myuserdatalist = allUserData.Where(Function(data) data.MDNr <> 0 And data.USNr <> currentUserData.USNr And UCase(data.US_Name) = UCase(txt_Anmeldename.EditValue)).ToList()
						If Not myuserdatalist Is Nothing Then
							isValid = isValid And SetErrorIfInvalid(txt_Anmeldename, errorProviderMangement, (myuserdatalist.Count > 0), errorKSTDuplicate)
						End If

					End If

				End If
			End If

			isValid = isValid And SetErrorIfInvalid(lueMandant, errorProviderMangement, lueMandant.EditValue = Nothing, errorText)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

			isValid = False

		End Try

		Return isValid

	End Function

	''' <summary>
	''' Validates a control.
	''' </summary>
	''' <param name="control">The control to validate.</param>
	''' <param name="errorProvider">The error providor.</param>
	''' <param name="invalid">Boolean flag if data is invalid.</param>
	''' <param name="errorText">The error text.</param>
	''' <returns>Valid flag</returns>
	Private Function SetErrorIfInvalid(ByVal control As Control, ByVal errorProvider As DXErrorProvider.DXErrorProvider, ByVal invalid As Boolean, ByVal errorText As String) As Boolean

		If (invalid) Then
			errorProvider.SetError(control, errorText)
		Else
			errorProvider.SetError(control, String.Empty)
		End If

		Return Not invalid

	End Function


	''' <summary>
	''' send login data over web service.
	''' </summary>
	Private Sub SendLoginDataViaWebService()

		Dim uiSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext()

		Task(Of Boolean).Factory.StartNew(Function() PerformSendingLoginDataWebserviceCallAsync(), CancellationToken.None, TaskCreationOptions.None,
										  TaskScheduler.Default).ContinueWith(Sub(t) FinishSendingLoginDataWebserviceCallTask(t), CancellationToken.None,
																			  TaskContinuationOptions.None, uiSynchronizationContext)

	End Sub

	''' <summary>
	'''  Performs sending login data asynchronous.
	''' </summary>
	''' <returns>The report response.</returns>
	Private Function PerformSendingLoginDataWebserviceCallAsync() As Boolean
		Dim success As Boolean = True
		Dim _setting = New SP.Infrastructure.Initialization.InitializeClass(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData, m_InitializationData.MDData, m_InitializationData.UserData)

		Dim webservice As New SP.Internal.Automations.SendSputnikLoginInfomations(_setting)

		Try
			' Read data over webservice
			Dim data = SelectedExitingRecord
			If data Is Nothing Then Return False
			success = success AndAlso webservice.SendSputnikUserDataWithWebservice(data.USNr)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			success = False
		End Try


		Return success

	End Function

	''' <summary>
	''' Finish sending login data web service call.
	''' </summary>
	Private Sub FinishSendingLoginDataWebserviceCallTask(ByVal t As Task(Of Boolean))

		Try

			Select Case t.Status
				Case TaskStatus.RanToCompletion
					' Webservice call was successful.

				Case TaskStatus.Faulted
					' Something went wrong -> log error.
					m_Logger.LogError(t.Exception.ToString())


				Case Else
					' Do nothing
			End Select

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try


	End Sub

	Private Function PerformDeletingLoginDataToWebService() As Boolean
		Dim success As Boolean = True
		Dim _setting = New SP.Infrastructure.Initialization.InitializeClass(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData, m_InitializationData.MDData, m_InitializationData.UserData)

		Dim webservice As New SP.Internal.Automations.SendSputnikLoginInfomations(_setting)

		Try
			' Read data over webservice
			Dim data = SelectedExitingRecord
			If data Is Nothing Then Return False
			success = success AndAlso webservice.UpdateActivationFlagForUserDataWithWebservice(data.USNr)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			success = False
		End Try


		Return success

	End Function

#End Region



#Region "Forms"

	Private Sub Onfrm_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
		SaveFromSettings()
	End Sub

	''' <summary>
	''' Loads form settings if form gets visible.
	''' </summary>
	Private Sub OnFrm_VisibleChanged(sender As System.Object, e As System.EventArgs) Handles MyBase.VisibleChanged

		If Visible Then
			LoadFormSettings()
		End If

	End Sub

	''' <summary>
	''' Loads form settings.
	''' </summary>
	Private Sub LoadFormSettings()

		Try
			Dim setting_form_height = My.Settings.ifrmHeightUser
			Dim setting_form_width = My.Settings.ifrmWidthUser
			Dim setting_form_location = My.Settings.frmLocationUser

			If setting_form_height > 0 Then Me.Height = Math.Max(Me.Height, setting_form_height)
			If setting_form_width > 0 Then Me.Width = Math.Max(Me.Width, setting_form_width)

			If Not String.IsNullOrEmpty(setting_form_location) Then
				Dim aLoc As String() = setting_form_location.Split(CChar(";"))
				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Val(aLoc(0)), Val(aLoc(1)))
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())

		End Try

	End Sub

	''' <summary>
	''' Saves the form settings.
	''' </summary>
	Private Sub SaveFromSettings()

		' Save form location, width and height in setttings
		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.frmLocationUser = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.ifrmWidthUser = Me.Width
				My.Settings.ifrmHeightUser = Me.Height

				My.Settings.Save()

			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())

		End Try

	End Sub

	Private Sub OnbtnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
		Me.Close()
	End Sub


#End Region



#Region "Helpers"

	''' <summary>
	''' creates new \\server\mdxx\profiles\UserProfilexx.xml file 
	''' </summary>
	''' <param name="connstring"></param>
	''' <param name="userNumber"></param>
	''' <param name="mandantNumber"></param>
	''' <remarks></remarks>
	Sub CreateUserProfileXMLFile(ByVal connstring As String, ByVal userNumber As Integer, ByVal mandantNumber As Integer)
		Dim enc As New System.Text.UnicodeEncoding
		Dim strStartElementName As String = "UserProfile"
		Dim strAttribute As String = "UserNr"
		Dim strField_2 As String = "Document"
		Dim strField_3 As String = ""
		If userNumber = 0 Then Return
		If mandantNumber = 0 Then Return
		Dim m_md As New Mandant
		'If m_reg Is Nothing Then m_reg = New ClsDivReg
		Dim xmlUserProfile = m_md.GetSelectedMDUserProfileXMLFilename(mandantNumber, userNumber)

		Dim strOldUsINIFile As String = Path.Combine(System.IO.Directory.GetParent(xmlUserProfile).FullName, String.Format("UserPro{0}", userNumber))
		Dim strOldFilename As String = Path.Combine(m_ProgPath.GetSpS2DeleteHomeFolder, "OldFile.xml")
		Try
			File.Copy(xmlUserProfile, strOldFilename, True)
		Catch ex As Exception

		End Try

		' XmlTextWriter-Objekt für unsere Ausgabedatei erzeugen: 
		Dim XMLobj As Xml.XmlTextWriter = New Xml.XmlTextWriter(xmlUserProfile, enc)
		Dim strValue As String = String.Empty
		Dim bResult As Boolean = True
		Dim _clsreg As New SPProgUtility.ClsDivReg

		Try

			With XMLobj
				' Formatierung: 4er-Einzüge verwenden 
				.Formatting = Xml.Formatting.Indented
				.Indentation = 4

				.WriteStartDocument()
				.WriteStartElement(strStartElementName)


				' Die Masken-Vorlage für Kundensuche...
				' Layouts
				.WriteStartElement("Layouts")
				.WriteStartElement("Form_DevEx")

				Dim strQuery As String = "//Layouts/Form_DevEx/FormStyle"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement("FormStyle")
				.WriteString(strValue)
				.WriteEndElement()

				strQuery = "//Layouts/Form_DevEx/NavbarStyle"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement("NavbarStyle")
				.WriteString(strValue)
				.WriteEndElement()

				.WriteEndElement()

				.WriteStartElement("Report")
				strQuery = "//Layouts/Report/matrixprintername"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement("matrixprintername")
				.WriteString(strValue)
				.WriteEndElement()

				.WriteEndElement()

				.WriteEndElement()



				.WriteStartElement(String.Format("User_{0}", userNumber))
				.WriteStartElement("Documents")


				'Try
				'	Dim sSql As String = "Select JobNr, DocName From DokPrint Where (JobNr <> '' Or JobNr Is Not Null) And "
				'	sSql &= "(DocName <> '' or DocName is not null) Order By JobNr"
				'	Dim Conn As SqlConnection = New SqlConnection(connstring)

				'	Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				'	Conn.Open()
				'	Dim rDocrec As SqlDataReader = cmd.ExecuteReader

				'	While rDocrec.Read

				'		strValue = m_reg.GetINIString(strOldUsINIFile, "Export", rDocrec("DocName"))
				'		If strValue = String.Empty Then strValue = "0"

				'		.WriteStartElement("DocName")
				'		.WriteAttributeString("ID", rDocrec("JobNr"))
				'		.WriteStartElement("Export")
				'		.WriteString(strValue)
				'		.WriteEndElement()
				'		.WriteEndElement()

				'	End While

				'Catch ex As Exception
				'	MsgBox(String.Format("DokPrint Error: {0}", ex.Message))

				'End Try

				.WriteEndElement()


				' Die Masken-Vorlage für Kundensuche...
				.WriteStartElement("FormControls")
				.WriteStartElement("FormName")
				.WriteAttributeString("ID", "4c2db8b0-0521-4862-a640-d895e02100f9")
				.WriteStartElement("TemplateFile")
				.WriteString("")
				.WriteEndElement()
				.WriteEndElement()
				' Fertig

				' Die Sonstigen Einstellungen...
				.WriteStartElement("USSetting")
				.WriteStartElement("SettingName")
				.WriteAttributeString("ID", "Cockpit.DayAgo4GebDat")
				.WriteStartElement("USValue")
				.WriteString("")
				.WriteEndElement()
				.WriteEndElement()
				' Fertig
				.WriteEndElement()
				.WriteEndElement()
				.WriteEndElement()


				' CSV-Setting... ----------------------------------------------------------------------
				' ----------------------------------------------------------------------
				strQuery = ""

				.WriteComment("Export Einstellungen")
				strAttribute = "Name"

				.WriteStartElement("CSV-Setting")
				.WriteAttributeString(strAttribute, "MASearch")

				' SelectedFields
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields", Chr(34), "MASearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement("SelectedFields")
				.WriteString(strValue)
				.WriteEndElement()

				' FieldIn
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn", Chr(34), "MASearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement("FieldIn")
				.WriteString(strValue)
				.WriteEndElement()

				' FieldSep
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep", Chr(34), "MASearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, ";")
				.WriteStartElement("FieldSep")
				.WriteString(strValue)
				.WriteEndElement()

				' ExportFileName
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName", Chr(34), "MASearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement("ExportFileName")
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()


				.WriteStartElement("CSV-Setting")
				.WriteAttributeString(strAttribute, "KDSearch")

				' SelectedFields ---------------------------------------------------------------------------
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields", Chr(34), "KDSearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement("SelectedFields")
				.WriteString(strValue)
				.WriteEndElement()

				' FieldIn
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn", Chr(34), "KDSearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement("FieldIn")
				.WriteString(strValue)
				.WriteEndElement()

				' FieldSep
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep", Chr(34), "KDSearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, ";")
				.WriteStartElement("FieldSep")
				.WriteString(strValue)
				.WriteEndElement()

				' ExportFileName
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName", Chr(34), "KDSearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement("ExportFileName")
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()


				.WriteStartElement("CSV-Setting")
				.WriteAttributeString(strAttribute, "ESSearch")

				' SelectedFields -------------------------------------------------------------------------------------------
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields", Chr(34), "ESSearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement("SelectedFields")
				.WriteString(strValue)
				.WriteEndElement()

				' FieldIn
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn", Chr(34), "ESSearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement("FieldIn")
				.WriteString(strValue)
				.WriteEndElement()

				' FieldSep
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep", Chr(34), "ESSearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, ";")
				.WriteStartElement("FieldSep")
				.WriteString(strValue)
				.WriteEndElement()

				' ExportFileName
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName", Chr(34), "ESSearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement("ExportFileName")
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()


				.WriteStartElement("CSV-Setting")
				.WriteAttributeString(strAttribute, "ESKDSearch")

				' SelectedFields -------------------------------------------------------------------------------------------
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields", Chr(34), "ESKDSearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement("SelectedFields")
				.WriteString(strValue)
				.WriteEndElement()

				' FieldIn
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn", Chr(34), "ESKDSearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement("FieldIn")
				.WriteString(strValue)
				.WriteEndElement()

				' FieldSep
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep", Chr(34), "ESKDSearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, ";")
				.WriteStartElement("FieldSep")
				.WriteString(strValue)
				.WriteEndElement()

				' ExportFileName
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName", Chr(34), "ESKDSearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement("ExportFileName")
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()


				.WriteStartElement("CSV-Setting")
				.WriteAttributeString(strAttribute, "ESMASearch")

				' SelectedFields -------------------------------------------------------------------------------------------
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields", Chr(34), "ESMASearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement("SelectedFields")
				.WriteString(strValue)
				.WriteEndElement()

				' FieldIn
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn", Chr(34), "ESMASearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement("FieldIn")
				.WriteString(strValue)
				.WriteEndElement()

				' FieldSep
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep", Chr(34), "ESMASearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, ";")
				.WriteStartElement("FieldSep")
				.WriteString(strValue)
				.WriteEndElement()

				' ExportFileName
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName", Chr(34), "ESMASearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement("ExportFileName")
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()


				' ExportSetting LO -------------------------------------------------------------------------------------------
				Dim sValue As String = String.Empty
				Dim strGuid As String = "SP.LO.PrintUtility"
				Dim strMainKey As String = "//ExportSetting[@Name={0}{1}{0}]/{2}"
				.WriteComment("Exporteinstellung für den Druck und Export Lohnabrechnungen...")
				.WriteStartElement("ExportSetting")
				.WriteAttributeString(strAttribute, strGuid)

				Dim strKeyName As String = "ExportPfad".ToLower
				strQuery = String.Format(strMainKey, Chr(34), strGuid, strKeyName)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
				.WriteStartElement(strKeyName)
				.WriteString(strValue)
				.WriteEndElement()

				strKeyName = "ExportFinalFileFilename".ToLower
				strQuery = String.Format(strMainKey, Chr(34), strGuid, strKeyName)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
				.WriteStartElement(strKeyName)
				.WriteString(strValue)
				.WriteEndElement()

				strKeyName = "ExportFilename".ToLower
				strQuery = String.Format(strMainKey, Chr(34), strGuid, strKeyName)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
				.WriteStartElement(strKeyName)
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()


				' ExportSetting ES -------------------------------------------------------------------------------------------
				sValue = String.Empty
				.WriteComment("Exporteinstellung für den Druck und Export Einsatzverträge...")
				strGuid = "SP.ES.PrintUtility"
				strMainKey = "//ExportSetting[@Name={0}{1}{0}]/{2}"
				.WriteStartElement("ExportSetting")
				.WriteAttributeString(strAttribute, strGuid)

				strKeyName = "ESUnterzeichner_ESVertrag".ToLower
				strQuery = String.Format(strMainKey, Chr(34), strGuid, strKeyName)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, 0)
				.WriteStartElement(strKeyName)
				.WriteString(strValue)
				.WriteEndElement()

				strKeyName = "ExportPfad".ToLower
				strQuery = String.Format(strMainKey, Chr(34), strGuid, strKeyName)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
				.WriteStartElement(strKeyName)
				.WriteString(strValue)
				.WriteEndElement()

				strKeyName = "ExportFilename_ESVertrag".ToLower
				strQuery = String.Format(strMainKey, Chr(34), strGuid, strKeyName)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
				.WriteStartElement(strKeyName)
				.WriteString(strValue)
				.WriteEndElement()

				strKeyName = "ExportFilename_Verleih".ToLower
				strQuery = String.Format(strMainKey, Chr(34), strGuid, strKeyName)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
				.WriteStartElement(strKeyName)
				.WriteString(strValue)
				.WriteEndElement()

				strKeyName = "ExportFinalFileFilename_ESVertrag".ToLower
				strQuery = String.Format(strMainKey, Chr(34), strGuid, strKeyName)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
				.WriteStartElement(strKeyName)
				.WriteString(strValue)
				.WriteEndElement()

				strKeyName = "ExportFinalFileFilename_Verleih".ToLower
				strQuery = String.Format(strMainKey, Chr(34), strGuid, strKeyName)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
				.WriteStartElement(strKeyName)
				.WriteString(strValue)
				.WriteEndElement()

				.WriteEndElement()


				' AdvancePayment Quitttung & Check ----------------------------------------------------------------------------------------
				sValue = String.Empty
				.WriteComment("AdvancePayment Quitttung and Check...")
				strKeyName = "advancepayment"
				.WriteStartElement(strKeyName)

				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "maxvalue8900"
				strValue = _clsreg.GetINIString(strOldUsINIFile, "ZG", "ZGMaxBetragBar", "0")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "maxvalue8930"
				strValue = _clsreg.GetINIString(strOldUsINIFile, "ZG", "ZGMaxBetragCheck", "0")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				.WriteEndElement()



				' askonexit ----------------------------------------------------------------------------------------
				sValue = String.Empty
				strKeyName = "programsetting"
				.WriteStartElement(strKeyName)

				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "askonexit"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "false")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()


				.WriteEndElement()





				.WriteEndElement()
				.Close()


			End With

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		End Try

	End Sub

	Private Function SaveUSSignSetting(ByVal data As UserData) As Boolean
		Dim success As Boolean = True
		Dim strXMLFile As String = m_mandant.GetSelectedMDUserProfileXMLFilename(data.MDNr, data.USNr)
		Dim xDoc As XmlDocument = New XmlDocument()
		Dim xNode As XmlNode
		Dim xElmntFamily As XmlElement = Nothing


		Try
			xDoc.Load(strXMLFile)

			xNode = xDoc.SelectSingleNode("*//programsetting")
			If xNode Is Nothing Then
				xNode = xDoc.CreateNode(XmlNodeType.Element, "programsetting", "")
				xDoc.DocumentElement.AppendChild(xNode)
			End If
			If xNode IsNot Nothing Then
				If TypeOf xNode Is XmlElement Then
					xElmntFamily = CType(xNode, XmlElement)
				End If
				' showcustomerrecordsincolor
				If xElmntFamily.SelectSingleNode("showcustomerrecordsincolor") IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode("showcustomerrecordsincolor"))
				InsertTextNode(xDoc, xElmntFamily, "showcustomerrecordsincolor", If(chkshowcustomerrecordsincolor.Checked, "true", "false"))
			End If
			If xNode IsNot Nothing Then
				If TypeOf xNode Is XmlElement Then xElmntFamily = CType(xNode, XmlElement)
				' DeleteUserTempFilesOnLogin
				If xElmntFamily.SelectSingleNode("DeleteUserTempFilesOnLogin".ToLower) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode("DeleteUserTempFilesOnLogin".ToLower))
				InsertTextNode(xDoc, xElmntFamily, "DeleteUserTempFilesOnLogin".ToLower, If(chkDeleteTempFilesOnLogin.Checked, "true", "false"))
			End If
			If xNode IsNot Nothing Then
				If TypeOf xNode Is XmlElement Then xElmntFamily = CType(xNode, XmlElement)
				' PrintCusotmerDataOnReportsContentTemplate
				If xElmntFamily.SelectSingleNode("printcustomerdataonreporttemplate".ToLower) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode("printcustomerdataonreporttemplate".ToLower))
				InsertTextNode(xDoc, xElmntFamily, "printcustomerdataonreporttemplate".ToLower, If(chkPrintCustomerData.Checked, "true", "false"))
			End If


			xNode = xDoc.SelectSingleNode("*//Report")
			If xNode Is Nothing Then
				xNode = xDoc.CreateNode(XmlNodeType.Element, "Report", "")
				xDoc.DocumentElement.AppendChild(xNode)
			End If
			If xNode IsNot Nothing Then
				If TypeOf xNode Is XmlElement Then
					xElmntFamily = CType(xNode, XmlElement)
				End If
				' showcustomerrecordsincolor
				If xElmntFamily.SelectSingleNode("matrixprintername") IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode("matrixprintername"))
				InsertTextNode(xDoc, xElmntFamily, "matrixprintername", cboUserMatrixPrinter.EditValue)
			End If



			xNode = xDoc.SelectSingleNode("//ExportSetting[@Name='SP.ES.PrintUtility']")
			If xNode Is Nothing Then
				Dim newNode As Xml.XmlElement = xDoc.CreateElement("ExportSetting")

				newNode.SetAttribute("Name", "SP.ES.PrintUtility")
				xDoc.DocumentElement.AppendChild(newNode)
				xNode = xDoc.SelectSingleNode("//CSV-Setting[@Name='SP.ES.PrintUtility']")
			End If
			If xNode IsNot Nothing Then
				If TypeOf xNode Is XmlElement Then
					xElmntFamily = CType(xNode, XmlElement)
				End If
				' Felder
				If xElmntFamily.SelectSingleNode("esunterzeichner_esvertrag") IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode("esunterzeichner_esvertrag"))
				InsertTextNode(xDoc, xElmntFamily, "esunterzeichner_esvertrag", If(chkesunterzeichner_esvertrag.Checked, "true", "false")) ' Me.ExportSetting.SQLFields)

			End If


			xDoc.Save(strXMLFile)


		Catch ex As Exception
			success = False
			m_Logger.LogError(ex.ToString)
		End Try


		Return success

	End Function

	Private Function InsertTextNode(ByVal xDoc As XmlDocument, ByVal xNode As XmlNode, ByVal strTag As String, ByVal strText As String) As XmlElement
		Dim xNodeTemp As XmlNode

		xNodeTemp = xDoc.CreateElement(strTag)
		xNodeTemp.AppendChild(xDoc.CreateTextNode(strText))
		xNode.AppendChild(xNodeTemp)

		Return CType(xNodeTemp, XmlElement)
	End Function

	Private Function GetXMLValueByQuery(ByVal strFilename As String, ByVal strQuery As String, ByVal strValuebyNull As String) As String
		Dim bResult As String = String.Empty
		Dim strBez As String = GetXMLNodeValue(strFilename, strQuery)

		If strBez = String.Empty Then strBez = strValuebyNull

		Return strBez
	End Function

	''' <summary>
	''' Gibt die XML-Value einer Datei und Query aus...
	''' </summary>
	''' <param name="strFilename"></param>
	''' <param name="strQuery"></param>
	''' <param name="strValuebyNull"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Private Function GetXMLValueByQueryWithFilename(ByVal strFilename As String, _
															ByVal strQuery As String, _
															ByVal strValuebyNull As String) As String
		Dim bResult As String = String.Empty
		Dim strBez As String = GetXMLNodeValue(strFilename, strQuery)

		If strBez = String.Empty Then strBez = strValuebyNull

		Return strBez
	End Function

	Private Function GetXMLNodeValue(ByVal strFileName As String, ByVal strQuery As String) As String
		Dim strValue As String = String.Empty
		Dim xmlDoc As New Xml.XmlDocument()
		Dim xpNav As XPathNavigator
		Dim xni As XPathNodeIterator

		Try
			If File.Exists(strFileName) Then
				xmlDoc.Load(strFileName)
				xpNav = xmlDoc.CreateNavigator()

				xni = xpNav.Select(strQuery)
				Do While xni.MoveNext()
					strValue = xni.Current.Value

				Loop
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		Return strValue
	End Function

	Private Function SearchInFolder(ByVal SourcePath As String) As Boolean
		Dim success As Boolean = True
		Dim SourceDir As DirectoryInfo = New DirectoryInfo(SourcePath)
		Dim pathIndex As Integer
		pathIndex = SourcePath.LastIndexOf("\")
		' the source directory must exist, otherwise throw an exception

		If SourceDir.Exists Then
			Dim SubDir As DirectoryInfo
			For Each SubDir In SourceDir.GetDirectories()
				'Trace.WriteLine(SubDir.Name)
				SearchInFolder(SubDir.FullName)
			Next

			For Each childFile As FileInfo In SourceDir.GetFiles("*", SearchOption.AllDirectories).Where(Function(file) file.Extension.ToLower = ".xml")
				If childFile.Name.ToLower.EndsWith(String.Format("{0}.xml", m_selectedUserData.USNr)) Then
					If childFile.Name.ToLower.Contains(String.Format("n{0}.xml", m_selectedUserData.USNr)) OrElse
						childFile.Name.ToLower.Contains(String.Format("p{0}.xml", m_selectedUserData.USNr)) OrElse
						childFile.Name.ToLower.Contains(String.Format("s{0}.xml", m_selectedUserData.USNr)) OrElse
						childFile.Name.ToLower.Contains(String.Format("r{0}.xml", m_selectedUserData.USNr)) OrElse
						childFile.Name.ToLower.Contains(String.Format("t{0}.xml", m_selectedUserData.USNr)) OrElse
						childFile.Name.ToLower.Contains(String.Format("y{0}.xml", m_selectedUserData.USNr)) OrElse
						childFile.Name.ToLower.Contains(String.Format("l{0}.xml", m_selectedUserData.USNr)) OrElse
						childFile.Name.ToLower.Contains(String.Format("e{0}.xml", m_selectedUserData.USNr)) OrElse
						childFile.Name.ToLower.Contains(String.Format("a{0}.xml", m_selectedUserData.USNr)) OrElse
						childFile.Name.ToLower.Contains(String.Format("w{0}.xml", m_selectedUserData.USNr)) OrElse
						childFile.Name.ToLower.Contains(String.Format("g{0}.xml", m_selectedUserData.USNr)) OrElse
						childFile.Name.ToLower.Contains(String.Format("o{0}.xml", m_selectedUserData.USNr)) OrElse
						childFile.Name.ToLower.Contains(String.Format("_{0}.xml", m_selectedUserData.USNr)) Then

						childFile.Delete()
						Trace.WriteLine(childFile.Name)
					Else
						Trace.WriteLine("***" & childFile.Name)

					End If
				End If

			Next
		Else
			Throw New DirectoryNotFoundException("Source directory does not exist: " + SourceDir.FullName)
		End If

		Return success
	End Function

	Private Function StrToBool(ByVal str As String) As Boolean

		Dim result As Boolean = False

		If String.IsNullOrWhiteSpace(str) Then
			Return False
		ElseIf str = "1" Then
			Return True

		End If

		Boolean.TryParse(str, result)

		Return result
	End Function

	Function EncryptMyString(ByVal strData As String, ByVal strCryptKey As String) As String
		Dim value As String = ""

		' put hier your code for Decrypt / decrypting
		Return value
	End Function

	Public Function Encrypt(ByVal sInputVal As String) As String
		Dim value As String = ""

		' put hier your code for Decrypt / decrypting
		Return value
	End Function

	Public Function Decrypt(ByVal sQueryString As String) As String
		Dim value As String = ""

		' put hier your code for Decrypt / decrypting
		Return value
	End Function

#End Region



End Class
