
Imports System.IO

Imports System.Reflection.Assembly

Imports SP.DatabaseAccess.Customer

Imports SPS.MainView.DataBaseAccess
Imports SPS.MainView.ModulConstants

Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging

Imports SP.KD.CPersonMng.UI
Imports SP.KD.CustomerMng.UI
Imports SP.MA.EmployeeMng.UI
Imports SP.MA.EinsatzMng.UI
Imports SP.MA.ReportMng.UI
Imports SP.KD.InvoiceMng.UI
Imports SP.MA.AdvancePaymentMng.UI
Imports SP.MA.PayrollMng.UI

Imports SPProgUtility.MainUtilities
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.SPTranslation.ClsTranslation

Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraEditors
Imports DevExpress.Metro.Navigation
Imports DevExpress.Utils
Imports DevExpress.DXperience.Demos.TutorialControlBase

Imports DevExpress.LookAndFeel
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports System.Xml
Imports System.Threading
Imports DevExpress.XtraSplashScreen

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SPProgUtility.CommonXmlUtility
Imports System.Threading.Tasks
Imports SPProgUtility.SPTranslation
Imports SP.Internal.Automations
Imports System.Reflection
Imports DevExpress.XtraBars
Imports DevExpress.XtraTab
Imports Traysoft.AddTapi
Imports AutoUpdaterDotNET
Imports DevExpress.XtraGauges.Core.Model
Imports DevExpress.XtraGauges.Win.Gauges.Circular

Public Class frmMainView
	Inherits DevExpress.XtraEditors.XtraForm

#Region "public consts"

	Public Const DEFAULT_SPUTNIK_UPDATE_PROTOKOLL_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPUpdateUtilities.asmx" ' "http://asmx.domain.com/wsSPS_services/SPUpdateUtilities.asmx"
	Public Const DATE_COLUMN_NAME As String = "magetdat;createdon;date;es_ab;es_ende;datum;fakdate;birthdate;aus_dat;paidon;krediton;printdate;reminder_1date;reminder_2date;reminder_3date;buchungdate"
	Public Const INTEGER_COLUMN_NAME As String = "vacancynumber;ofnr;vaknr;fopnr;manr;kdnr;zhdnr;esnr;rpnr;zgnr;lonr;renr;id;vgnr;monat;jahr"
	Public Const DECIMAL_COLUMN_NAME As String = "betragex;bezahlt;betragink;betragmwst;betragtotal;zebetrag;lobetrag;betrag;aus_dat"

#End Region


#Region "Private Consts"

	Private Const ProgHeaderName As String = "Sputnik Enterprise Suite - [{0}] | {1}"
	Private Const TileLayoutFile As String = "{0}{1}_{2}.xml"

	Private Const MANDANT_XML_SETTING_SPUTNIK_UTILITIES_WEBSERVICE_WEBSERVICE_URI As String = "MD_{0}/Interfaces/webservices/webserviceupdateinfoservices"
	Private Const NOTIFYER_UTILITIY_ASSEMBLY_NAME As String = "SP.Main.Notify.dll"
	Private Const MANDANT_XML_SETTING_SPUTNIK_ALLOWEAUTOFILTERCONDITIONCHANGE As String = "MD_{0}/Sonstiges/allowautofilterconditionchange"
	Private Const MANDANT_XML_SETTING_SPUTNIK_COLUMN_AUTOFILTERMODE As String = "MD_{0}/Sonstiges/columnautofiltermode"

#End Region


#Region "private fields"

	Private m_SuppressZGUIEvents As Boolean

	Protected m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	Private m_UtilityUI As UtilityUI
	Private m_Utility As Utility
	Private Shared m_Logger As ILogger

	Private load_msg As WaitDialogForm

	Private m_DatabaseAccess As DataBaseAccess.MainGrid
	Private m_Translate As TranslateValues
	Private m_MandantData As Mandant

	Private bIsEmployeeloaded As Boolean
	Private bIsCustomerloaded As Boolean
	Private bIsVacancyloaded As Boolean
	Private bIsProposeloaded As Boolean
	Private bIsMassenloaded As Boolean

	Private bIsESloaded As Boolean
	Private bIsRPloaded As Boolean
	Private bIsZGloaded As Boolean
	Private bIsLOloaded As Boolean
	Private bIsREloaded As Boolean

	Private bIsGUloaded As Boolean
	Private bIsZEloaded As Boolean
	Private bIsMahnloaded As Boolean
	Private bIsFOPloaded As Boolean

	Private m_MainTileLayoutFile As String
	Private m_ListingTileLayoutFile As String
	Private m_ExtraTileLayoutFile As String
	Private m_ManagementTileLayoutFile As String

	Private m_CustomerDatabaseAccess As ICustomerDatabaseAccess

	Private m_MandantSettingsXml As SettingsXml
	Private m_SPUpdateUtilitiesServiceUrl As String

	Private m_NotifyerFileName As String
	Private m_ExistsNotifyer As Boolean
	Private m_NotifyerUtility As SP.Main.Notify.UI.frmNotify
	Private m_ExistsGAVNews As Boolean
	Private m_ExistsToDoNews As Boolean

	Private m_AllowAutoFilterConditionChange As Boolean
	Private m_ColumnAutoFilterMode As ColumnAutoFilterMode

	Private scaleMinutes, scaleSeconds As ArcScaleComponent
	Private lockTimerCounter As Integer = 0
	Private m_MDNr As Integer?

	Private m_MainViewGridData As SuportedCodes

	Private m_AllowedTempDataPVL As Boolean

#End Region


#Region "private properties"

	Private Property ExistsNewupdate As Boolean
	Private Property UpdateProtokollID As Integer?

	Private ReadOnly Property GetColumnAutoFilterMode() As ColumnAutoFilterMode
		Get
			Dim value As String = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_COLUMN_AUTOFILTERMODE, ModulConstants.MDData.MDNr))
			Dim result As ColumnAutoFilterMode
			If String.IsNullOrWhiteSpace(value) Then
				result = ColumnAutoFilterMode.Default
			Else
				result = value
			End If

			Return result

		End Get
	End Property

	Private ReadOnly Property AlloweAutoFilterConditionChange() As Boolean
		Get
			Dim value As String = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_ALLOWEAUTOFILTERCONDITIONCHANGE, ModulConstants.MDData.MDNr))
			Dim result As Boolean?
			If String.IsNullOrWhiteSpace(value) Then
				result = Nothing
			Else
				result = CBool(value)
			End If

			Return result.HasValue AndAlso result

		End Get
	End Property

	Private ReadOnly Property DeleteUserTempFilesOnLogin() As Boolean
		Get
			Dim FORM_XML_MAIN_KEY As String = "UserProfile/programsetting"
			Dim m_path As New SPProgUtility.ProgPath.ClsProgPath

			Dim UserXMLFileName = m_MandantData.GetSelectedMDUserProfileXMLFilename(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)
			Dim value As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(UserXMLFileName, String.Format("{0}/" & "DeleteUserTempFilesOnLogin".ToLower, FORM_XML_MAIN_KEY)), False)

			Return value
		End Get

	End Property


#End Region


#Region "Contructor"

	Public Sub New()

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_Logger = New Logger
		m_DatabaseAccess = New DataBaseAccess.MainGrid
		m_MandantData = New Mandant
		m_Utility = New Utility
		m_UtilityUI = New UtilityUI
		m_AllowedTempDataPVL = True

		m_Translate = New TranslateValues
		m_InitializationData = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		m_MDNr = ModulConstants.MDData.MDNr
		m_ExistsGAVNews = False
		m_ExistsToDoNews = False

		InitializeComponent()

		Reset()

		Try
			LoadData()

			'InitializeMainModules()
			Dim domainName = ModulConstants.MDData.WebserviceDomain
			m_SPUpdateUtilitiesServiceUrl = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_UPDATE_PROTOKOLL_UTIL_WEBSERVICE_URI)

			m_CustomerDatabaseAccess = New CustomerDatabaseAccess(ModulConstants.MDData.MDDbConn, ModulConstants.UserData.UserLanguage)

			TrasnlateControls()


		Catch ex As Exception
			m_Logger.LogError(String.Format("Start-Moduls: {0}", ex.Message))

		End Try


	End Sub

	Private Sub Reset()

		m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(ModulConstants.MDData.MDNr, Now.Year))

		bbiUpdatAnnouncement.ItemAppearance.Normal.BackColor = Color.Red

		bbiGAVPublication.ItemAppearance.Normal.BackColor = Color.Red
		bbiGAVPublication.Visibility = BarItemVisibility.Never

		bsiInfo.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True
		bsiMDData.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True
		bsiMDPath.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True
		bsiDbSrvName.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True

		scaleMinutes = circularGauge1.AddScale()
		scaleSeconds = circularGauge1.AddScale()

		scaleMinutes.Assign(scaleHours)
		scaleSeconds.Assign(scaleHours)

		arcScaleNeedleComponent2.ArcScale = scaleMinutes
		arcScaleNeedleComponent3.ArcScale = scaleSeconds
		timer.Start()
		OnTimerTick(Nothing, Nothing)


		Try
			TapiApp.SerialNumber = "serialnumber"
			TapiApp.Initialize("Sputnik Enterprise Suite")

		Catch ex As TapiException
			m_Logger.LogError(String.Format("TapiException: error during Initialize: {0}", ex.ToString))
		Catch ex As Exception
			m_Logger.LogError(String.Format("Exception: error during Initialize: {0}", ex.ToString))
		End Try

		Try

			m_ColumnAutoFilterMode = GetColumnAutoFilterMode
			m_AllowAutoFilterConditionChange = AlloweAutoFilterConditionChange
			WindowsFormsSettings.ColumnAutoFilterMode = m_ColumnAutoFilterMode
			WindowsFormsSettings.AllowAutoFilterConditionChange = If(Not m_AllowAutoFilterConditionChange, DefaultBoolean.False, DefaultBoolean.Default)

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		End Try

	End Sub

	Private Sub LoadData()

		VerifyUserTempFiles()

		'#If Not DEBUG Then
		DownloadCommonSettingFilesViaWebService()
		'#End If

		LoadTilImages()
		CheckSettingForFilesAndDirectories()

	End Sub

	Private Sub LoadTilImages()

		' Mainview
		tiKandidat.Image = My.Resources.Sputnik_ICON_Kandidaten
		tiKunde.Image = My.Resources.Sputnik_ICON_Kunden
		tiApplicationMng.Image = My.Resources.Sputnik_Icon_Bewerbermanagement
		tiVakanz.Image = My.Resources.Sputnik_Icon_Vakanzen
		tiPropose.Image = My.Resources.Sputnik_ICON_Vorschläge
		tiMassenversand.Image = My.Resources.Sputnik_ICON_Massenversand

		tiEinsatz.Image = My.Resources.Sputnik_Icon_Einsätze

		tiReport.Image = My.Resources.Sputnik_ICON_Rapporte
		tiVorschuss.Image = My.Resources.Sputnik_ICON_Vorschüsse
		tiLohn.Image = My.Resources.Sputnik_ICON_Löhne

		tiFaktura.Image = My.Resources.Sputnik_ICON_Fakturen
		tiGutschriften.Image = My.Resources.Sputnik_ICON_Gutschriften
		tiMahn.Image = My.Resources.Sputnik_ICON_Mahnungen
		tiZahlung.Image = My.Resources.Sputnik_ICON_Zahlungseingänge
		tiFremdrechnung.Image = My.Resources.Sputnik_ICON_Fremdrechnungen

		' Listing
		tiMAListing.Image = My.Resources.Sputnik_Icon_LI_Kandidaten_72
		tiKDListing.Image = My.Resources.Sputnik_Icon_LI_Kunden_72
		tiVakListing.Image = My.Resources.Sputnik_Icon_LI_Vakanzen_72
		tiProposeListing.Image = My.Resources.Sputnik_Icon_LI_Vorschlag_72
		tiTelefonListing.Image = My.Resources.Sputnik_Icon_LI_Telefon_72
		tiNachrichtenListing.Image = My.Resources.Sputnik_Icon_LI_Nachrichten_72

		tiESListing.Image = My.Resources.Sputnik_Icon_LI_Einsätze_72
		tiRPDatenListing.Image = My.Resources.Sputnik_Icon_LI_Rapporte_72
		tiZGListing.Image = My.Resources.Sputnik_Icon_LI_Auszahlungen_72
		tiLOListing.Image = My.Resources.Sputnik_Icon_LI_Lohn_72
		tiDebitorenListing.Image = My.Resources.Sputnik_Icon_LI_Debitoren_72
		tiZEListing.Image = My.Resources.Sputnik_Icon_LI_Zahlungen_72
		tiFOPListing.Image = My.Resources.Sputnik_Icon_LI_Fremdrechnung_72

		tiDB1Listing.Image = My.Resources.Sputnik_Icon_LI_DB1
		tiKDUmsatzListing.Image = My.Resources.Sputnik_Icon_LI_Umsatz

		' Extras
		tiUpdateprotokoll.Image = My.Resources.Sputnik_Icon_eXtra_Updateprotokoll_GzD
		tiGAVNews.Image = My.Resources.Sputnik_Icon_eXtra_GAV_News_GzD
		tiFernwartung.Image = My.Resources.Sputnik_Icon_eXtra_Fernwartung_R_Ring_GzD
		tiBenutzerhandbuch.Image = My.Resources.Sputnik_Icon_eXtra_Benutzerhandbuch_GzD

		tiMandantListing.Image = My.Resources.Sputnik_Icon_eXtra_Datenliste_72pixel
		tiDeleteprokoll.Image = My.Resources.Sputnik_Icon_eXtra_Löschprotokoll

		' Verwaltung
		tiMandanten.Image = My.Resources.Sputnik_Icon_Verw__Mandantenverw__GzD
		tiBenutzer.Image = My.Resources.Sputnik_Icon_Verw__Benutzer_GzD
		tiLohnartenstamm.Image = My.Resources.Sputnik_Icon_Verw__Lohnartenstamm_GzD
		tiEinstellungen.Image = My.Resources.Sputnik_Icon_Verw__Einstellungen_GzD
		tiDokument.Image = My.Resources.Sputnik_Icon_Verw__Dokumentenverwaltung_GzD
		tiTabellen.Image = My.Resources.Sputnik_Icon_Verw__Tabellenverwaltung_GzD

		bbiScan2Sputnik.Enabled = (m_MandantData.ModulLicenseKeys(ModulConstants.MDData.MDNr, Now.Year, "").ScanDropIN AndAlso UserSecValue(677))
		bbiCV2Sputnik.Enabled = (m_MandantData.ModulLicenseKeys(ModulConstants.MDData.MDNr, Now.Year, "").CVDropIN AndAlso UserSecValue(678))
		bbiPMSputnik.Enabled = (m_MandantData.ModulLicenseKeys(ModulConstants.MDData.MDNr, Now.Year, "").PMSearch AndAlso UserSecValue(679))

	End Sub

	Private Sub StartNotifying()

		If m_ExistsNotifyer Then
			Try
				'Dim m_init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)
				If m_NotifyerUtility Is Nothing Then m_NotifyerUtility = New SP.Main.Notify.UI.frmNotify(m_InitializationData)

				Dim success = m_NotifyerUtility.LoadNotify
				m_NotifyerUtility.Show()

				'Process.Start(notifyApp)

			Catch ex As Exception
				m_Logger.LogError(String.Format("starting process: {0} | {1}", m_NotifyerFileName, ex.ToString))

			End Try
		Else
			m_Logger.LogDebug(String.Format("Process was already gestartet or not available: {0}", m_NotifyerFileName))
		End If

	End Sub

	Private Sub VerifyUserTempFiles()
		If Not DeleteUserTempFilesOnLogin Then Return

		Try
			'm_Utility.ClearAssignedFolder(Directory.GetParent(ModulConstants.UserData.SPTempPath).FullName)
			m_Utility.ClearAssignedFolder(ModulConstants.UserData.SPTempPath)
			m_Utility.ClearAssignedFolder(ModulConstants.UserData.spAllowedPath)

		Catch ex As Exception

		End Try

	End Sub

	Private Sub CheckSettingForFilesAndDirectories()

		SetMainFormName(True)

		Dim configfile As String = m_MandantData.GetAllUserGridSettingXMLFilename(m_MandantData.GetDefaultMDNr)
		Try
			UserGridSettingsXml = New SettingsXml(m_MandantData.GetAllUserGridSettingXMLFilename(m_MandantData.GetDefaultMDNr))

		Catch ex As Exception
			m_Logger.LogError(String.Format("Fehler in der Konfigurationsdatei: {0}", configfile))
			Throw New Exception(String.Format("Fehler in der Konfigurationsdatei: {0}{1}{2}", vbNewLine, configfile, ex.Message))
		End Try

		Try
			Dim newDirectoryPath As String = String.Empty

			ModulConstants.GridSettingPath = String.Format("{0}MainView\", m_MandantData.GetGridSettingPath(ModulConstants.MDData.MDNr))
			If Not Directory.Exists(ModulConstants.GridSettingPath) Then Directory.CreateDirectory(ModulConstants.GridSettingPath)

			m_MainTileLayoutFile = String.Format(TileLayoutFile, ModulConstants.GridSettingPath, ticMainModules.Name, ModulConstants.UserData.UserNr)
			m_ListingTileLayoutFile = String.Format(TileLayoutFile, ModulConstants.GridSettingPath, ticListingModules.Name, ModulConstants.UserData.UserNr)
			m_ExtraTileLayoutFile = String.Format(TileLayoutFile, ModulConstants.GridSettingPath, ticExtraModules.Name, ModulConstants.UserData.UserNr)
			m_ManagementTileLayoutFile = String.Format(TileLayoutFile, ModulConstants.GridSettingPath, ticManagementModules.Name, ModulConstants.UserData.UserNr)

			newDirectoryPath = ModulConstants.GridSettingPath & "Employee\"
			If Not Directory.Exists(newDirectoryPath) Then Directory.CreateDirectory(newDirectoryPath)
			newDirectoryPath = newDirectoryPath & "Details\"
			If Not Directory.Exists(newDirectoryPath) Then Directory.CreateDirectory(newDirectoryPath)

			newDirectoryPath = ModulConstants.GridSettingPath & "Customer\"
			If Not Directory.Exists(newDirectoryPath) Then Directory.CreateDirectory(newDirectoryPath)
			newDirectoryPath = newDirectoryPath & "Details\"
			If Not Directory.Exists(newDirectoryPath) Then Directory.CreateDirectory(newDirectoryPath)

			newDirectoryPath = ModulConstants.GridSettingPath & "Propose\"
			If Not Directory.Exists(newDirectoryPath) Then Directory.CreateDirectory(newDirectoryPath)
			newDirectoryPath = newDirectoryPath & "Details\"
			If Not Directory.Exists(newDirectoryPath) Then Directory.CreateDirectory(newDirectoryPath)

			newDirectoryPath = ModulConstants.GridSettingPath & "Vacancy\"
			If Not Directory.Exists(newDirectoryPath) Then Directory.CreateDirectory(newDirectoryPath)
			newDirectoryPath = newDirectoryPath & "Details\"
			If Not Directory.Exists(newDirectoryPath) Then Directory.CreateDirectory(newDirectoryPath)

			newDirectoryPath = ModulConstants.GridSettingPath & "APPLICATION\"
			If Not Directory.Exists(newDirectoryPath) Then Directory.CreateDirectory(newDirectoryPath)

			newDirectoryPath = ModulConstants.GridSettingPath & "Employment\"
			If Not Directory.Exists(newDirectoryPath) Then Directory.CreateDirectory(newDirectoryPath)
			newDirectoryPath = newDirectoryPath & "Details\"
			If Not Directory.Exists(newDirectoryPath) Then Directory.CreateDirectory(newDirectoryPath)

			newDirectoryPath = ModulConstants.GridSettingPath & "Report\"
			If Not Directory.Exists(newDirectoryPath) Then Directory.CreateDirectory(newDirectoryPath)
			newDirectoryPath = newDirectoryPath & "Details\"
			If Not Directory.Exists(newDirectoryPath) Then Directory.CreateDirectory(newDirectoryPath)

			newDirectoryPath = ModulConstants.GridSettingPath & "ZG\"
			If Not Directory.Exists(newDirectoryPath) Then Directory.CreateDirectory(newDirectoryPath)
			newDirectoryPath = newDirectoryPath & "Details\"
			If Not Directory.Exists(newDirectoryPath) Then Directory.CreateDirectory(newDirectoryPath)

			newDirectoryPath = ModulConstants.GridSettingPath & "Salary\"
			If Not Directory.Exists(newDirectoryPath) Then Directory.CreateDirectory(newDirectoryPath)
			newDirectoryPath = newDirectoryPath & "Details\"
			If Not Directory.Exists(newDirectoryPath) Then Directory.CreateDirectory(newDirectoryPath)

			newDirectoryPath = ModulConstants.GridSettingPath & "Invoice\"
			If Not Directory.Exists(newDirectoryPath) Then Directory.CreateDirectory(newDirectoryPath)
			newDirectoryPath = newDirectoryPath & "Details\"
			If Not Directory.Exists(newDirectoryPath) Then Directory.CreateDirectory(newDirectoryPath)

			newDirectoryPath = ModulConstants.GridSettingPath & "ZE\"
			If Not Directory.Exists(newDirectoryPath) Then Directory.CreateDirectory(newDirectoryPath)
			newDirectoryPath = newDirectoryPath & "Details\"
			If Not Directory.Exists(newDirectoryPath) Then Directory.CreateDirectory(newDirectoryPath)

			newDirectoryPath = ModulConstants.GridSettingPath & "GU\"
			If Not Directory.Exists(newDirectoryPath) Then Directory.CreateDirectory(newDirectoryPath)
			newDirectoryPath = newDirectoryPath & "Details\"
			If Not Directory.Exists(newDirectoryPath) Then Directory.CreateDirectory(newDirectoryPath)

			newDirectoryPath = ModulConstants.GridSettingPath & "Mahnung\"
			If Not Directory.Exists(newDirectoryPath) Then Directory.CreateDirectory(newDirectoryPath)

			newDirectoryPath = ModulConstants.GridSettingPath & "FOP\"
			If Not Directory.Exists(newDirectoryPath) Then Directory.CreateDirectory(newDirectoryPath)
			newDirectoryPath = newDirectoryPath & "Details\"
			If Not Directory.Exists(newDirectoryPath) Then Directory.CreateDirectory(newDirectoryPath)

			newDirectoryPath = ModulConstants.GridSettingPath & "TODO\"
			If Not Directory.Exists(newDirectoryPath) Then Directory.CreateDirectory(newDirectoryPath)


		Catch ex As Exception
			m_Logger.LogError(String.Format("Verzeichnis konnte nicht erstellt werden: {0}", ModulConstants.GridSettingPath))
			Throw New Exception(String.Format("Verzeichnis konnte nicht erstellt werden: {0}", ModulConstants.GridSettingPath))
		End Try


		Try
			If m_MDNr.GetValueOrDefault(0) <> ModulConstants.MDData.MDNr Then
				DownloadCommonSettingFilesViaWebService()
				m_MDNr = ModulConstants.MDData.MDNr
			End If

			LoadUserImage()

		Catch ex As Exception
			m_Logger.LogError(String.Format("Userimage: {0}", ex.Message))

			advisorPicture.Image = Nothing
			advisorPicture.Properties.NullText = m_Translate.GetSafeTranslationValue("Kein Bild vorhanden!")

		End Try

		' set users allowed moduls
		Try
			SetUserAllowedAction()

		Catch ex As Exception
			m_Logger.LogError(String.Format("Authorized-Moduls: {0}", ex.Message))

		End Try

		Try
			Me.PageTODO1.LoadXMLDataForSelectedModule()
			Me.PageTODO1.grpFunction.Text = String.Format(m_Translate.GetSafeTranslationValue("TODO-Liste für {0}"), ModulConstants.UserData.UserFullName)

		Catch ex As Exception
			m_Logger.LogError(String.Format("TODO-Liste: {0}", ex.Message))

		End Try

	End Sub

#End Region


#Region "ModulCaching"

	''' <summary>
	''' create hup for starting moduls
	''' </summary>
	''' <param name="msg"></param>
	''' <remarks></remarks>
	Private Sub HandleOpenEmployeeMngFormMsg(ByVal msg As OpenEmployeeMngRequest)

		Dim _ClsMA As New ClsOpenModul(New ClsSetting With {.SelectedMANr = msg.EmployeeNumber})
		_ClsMA.OpenSelectedEmployee(msg.MDNr, msg.USNr)

	End Sub

	Private Sub HandleOpenCustomerMngFormMsg(ByVal msg As OpenCustomerMngRequest)

		Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedKDNr = msg.CustomerNumber})
		_ClsKD.OpenSelectedCustomer(msg.MDNr, msg.USNr)

	End Sub

	Private Sub HandleOpenResponsiblePersonMngFormMsg(ByVal msg As OpenResponsiblePersonMngRequest)
		'Dim m_init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)
		Dim responsiblePersonsFrom = New frmResponsiblePerson(m_InitializationData)

		If (responsiblePersonsFrom.LoadResponsiblePersonData(msg.KDNr, msg.ZHDNumber)) Then
			responsiblePersonsFrom.Show()
			responsiblePersonsFrom.BringToFront()
		End If

	End Sub

	Private Sub HandleOpenVacancyMngFormMsg(ByVal msg As OpenVacancyMngRequest)
		Dim clsObj As New ClsOpenModul(New ClsSetting With {.SelectedVakNr = msg.VacancyNumber})
		clsObj.OpenSelectedVacancyTiny(msg.MDNr, msg.USNr)

	End Sub

	Private Sub HandleOpenProposeMngFormMsg(ByVal msg As OpenProposeMngRequest)
		Dim clsObj As New ClsOpenModul(New ClsSetting With {.SelectedProposeNr = msg.ProposeNumber})
		clsObj.OpenSelectedProposeTiny(msg.MDNr, msg.USNr)

	End Sub

	Private Sub HandleOpenEinsatzMngFormMsg(ByVal msg As OpenEinsatzMngRequest)

		Dim _ClsES As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = msg.MDNr, .SelectedESNr = msg.ESNr})
		_ClsES.OpenSelectedES(msg.MDNr, msg.USNr)

	End Sub

	Private Sub HandleOpenReportsMngFormMsg(ByVal msg As OpenReportsMngRequest)

		Dim _ClsES As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = msg.MDNr, .SelectedRPNr = msg.ReportNumber})

		_ClsES.OpenSelectedReport(msg.MDNr, msg.USNr)

	End Sub

	Private Sub HandleOpenInvoicesMngFormMsg(ByVal msg As OpenInvoiceMngRequest)

		Dim _ClsES As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = msg.MDNr, .SelectedRENr = msg.InvoiceNumber})

		_ClsES.OpenSelectedInvoice(msg.MDNr, msg.USNr)

	End Sub


	Private Sub HandleOpenEmployeeContactMngFormMsg(ByVal msg As OpenMAKontaktMngRequest)

		Dim _ClsES As New ClsOpenModul(New ClsSetting With {.SelectedMANr = msg.EmployeeNumber, .ContactRecordNumber = msg.ContactRecordNumber})

		_ClsES.OpenSelectedEmployeeContact()

	End Sub

	Private Sub HandleOpenCustomerContactMngFormMsg(ByVal msg As OpenKDKontaktMngRequest)

		Dim _ClsES As New ClsOpenModul(New ClsSetting With {.SelectedKDNr = msg.CustomerNumber, .ContactRecordNumber = msg.ContactRecordNumber})

		_ClsES.OpenSelectedCustomerContact()

	End Sub

	Private Sub HandleOpenAdvancePaymentMngFormMsg(ByVal msg As OpenAdvancePaymentMngRequest)

		Dim _ClsES As New ClsOpenModul(New ClsSetting With {.SelectedZGNr = msg.ZGNumber, .SelectedMDNr = msg.MDNr})

		_ClsES.OpenSelectedAdvancePayment(msg.MDNr, msg.USNr)

	End Sub

	Private Sub HandlePayrollProcessingStateHasChanged(ByVal msg As PayrollProcessingStateHasChanged)

		IsPayrollInProcessing = msg.IsPayrollProcessing

	End Sub

	Private Sub HandleRefreshMainViewStatebarFormMsg(ByVal msg As RefreshMainViewStatebar)

		If msg.Recordcount.HasValue Then
			bsiInfo.Caption = String.Format("Anzahl Datensätze: {0}", msg.Recordcount)
		End If
		SetMainFormName(Not msg.Recordcount.HasValue)

	End Sub

	Private Sub xtabMain_SelectedPageChanged(sender As Object, e As TabPageChangedEventArgs) Handles xtabMain.SelectedPageChanged
		SetUserAllowedAction()
	End Sub



#End Region



	''' <summary>
	''' initial all .net module for faster start
	''' </summary>
	''' <remarks></remarks>
	Private Sub InitializeMainModules()

		Try
			Dim frm As frmEmployees = CType(ModulConstants.GetModuleCach.GetModuleForm(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr, SP.ModuleCaching.ModuleName.EmployeeMng), frmEmployees)

		Catch ex As Exception
			m_Logger.LogError(String.Format("frmEmployees: {0}", ex.ToString()))
		End Try

		Try
			Dim frm As frmCustomers = CType(ModulConstants.GetModuleCach.GetModuleForm(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr, SP.ModuleCaching.ModuleName.CustomerMng), frmCustomers)

		Catch ex As Exception
			m_Logger.LogError(String.Format("frmCustomers: {0}", ex.ToString()))
			ShowErrDetail(ex.ToString)

		End Try

		Try
			Dim frmEinsatz As SP.MA.EinsatzMng.UI.frmES = CType(ModulConstants.GetModuleCach.GetModuleForm(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr, SP.ModuleCaching.ModuleName.ESMng), frmES)

		Catch ex As Exception
			m_Logger.LogError(String.Format("frmES: {0}", ex.ToString()))
			ShowErrDetail(ex.ToString)

		End Try

		Try
			Dim frmRP As SP.MA.ReportMng.UI.frmReportMng = CType(ModulConstants.GetModuleCach.GetModuleForm(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr, SP.ModuleCaching.ModuleName.ReportMng), frmReportMng)

		Catch ex As Exception
			m_Logger.LogError(String.Format("frmReportMng: {0}", ex.ToString()))
			ShowErrDetail(ex.ToString)

		End Try

		Try
			Dim frmInvoice As SP.KD.InvoiceMng.UI.frmInvoices = CType(ModulConstants.GetModuleCach.GetModuleForm(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr, SP.ModuleCaching.ModuleName.InvoiceMng), frmInvoices)

		Catch ex As Exception
			m_Logger.LogError(String.Format("frmInvoices: {0}", ex.ToString()))
			ShowErrDetail(ex.ToString)

		End Try

		Try
			Dim frmadvancepayment As SP.MA.AdvancePaymentMng.UI.frmAdvancePayments = CType(ModulConstants.GetModuleCach.GetModuleForm(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr, SP.ModuleCaching.ModuleName.AdvancePaymentMng), frmAdvancePayments)

		Catch ex As Exception
			m_Logger.LogError(String.Format("frmAdvancePayments: {0}", ex.ToString()))
			ShowErrDetail(ex.ToString)

		End Try

		Try
			Dim frm As frmPayroll = CType(ModulConstants.GetModuleCach.GetModuleForm(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr, SP.ModuleCaching.ModuleName.PayrollMng), frmPayroll)

		Catch ex As Exception
			m_Logger.LogError(String.Format("frmPayroll: {0}", ex.ToString()))
			ShowErrDetail(ex.ToString)

		End Try

		Try
			'm_Logger.LogDebug(String.Format("SPSModulsView.ClsMain: starting..."))
			vb6Object = CreateObject("SPSModulsView.ClsMain")
			'm_Logger.LogDebug(String.Format("SPSModulsView.ClsMain: Finishing..."))

		Catch ex As Exception
			'ShowErrDetail(ex.ToString)
			m_Logger.LogError(String.Format("SPSModulsView.ClsMain: {0}", ex.ToString()))
			'ShowErrDetail("COM.Components could not be loaded...")

		End Try


	End Sub


	Private Sub VerifyUserGAVPublicationNews()
		Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		m_ExistsGAVNews = False

		Try
			m_Logger.LogInfo(String.Format("VerifyUserGAVPublicationNews: searching for gav publications!!!"))

			bbiGAVPublication.Visibility = BarItemVisibility.Never
			Dim newsData = New SPGAV.UI.frmPublicationNews(init)
			Dim tempDataMergedNews = newsData.LoadMergedNewsData(True)
			If tempDataMergedNews Is Nothing Then Return

			bbiGAVPublication.Visibility = BarItemVisibility.Always
			bbiGAVPublication.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl GAV-Nachrichten: {0}"), tempDataMergedNews.Count)
			If tempDataMergedNews.Count = 0 Then bbiGAVPublication.ItemAppearance.Normal.BackColor = Color.Transparent

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		m_ExistsGAVNews = True

	End Sub

	Private Sub LoadUserGAVPublicationNews()
		Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		Dim newsData = New SPGAV.UI.frmPublicationNews(init)
		If Not newsData.LoadData(True) Then Return

		newsData.Show()
		newsData.BringToFront()

	End Sub

	Sub SetUserAllowedAction()


		Try
			Dim Time_1 As Double = System.Environment.TickCount
			m_Logger.LogDebug("Starting: SetUserAllowedAction")

			Dim bRight101 = UserSecValue(101)
			Dim bRight201 = UserSecValue(201)
			Dim bRight701 = UserSecValue(701)
			Dim bRight801 = UserSecValue(801)
			Dim bRight250 = UserSecValue(250)
			Dim bRight300 = UserSecValue(300)
			Dim bRight349 = UserSecValue(349)
			Dim bRight16 = UserSecValue(16)
			Dim bRight14 = UserSecValue(14)
			Dim bRight15 = UserSecValue(15)

			Dim bRight100100 = UserSecValue(100100)
			Dim bRight601 = UserSecValue(601)
			Dim bRight621 = UserSecValue(621)
			Dim bRight643 = UserSecValue(643)
			Dim bRight603 = UserSecValue(603)
			Dim bRight556 = UserSecValue(556)

			Dim bRight557 = UserSecValue(557)
			Dim bRight605 = UserSecValue(605)
			Dim bRight606 = UserSecValue(606)
			Dim bRight100105 = UserSecValue(100105)
			Dim bRight607 = UserSecValue(607)
			Dim bRight608 = UserSecValue(608)
			Dim bRight610 = UserSecValue(610)
			Dim bRight650 = UserSecValue(650)
			Dim bRight552 = UserSecValue(552)

			Dim bRight666 = UserSecValue(666)
			Dim bRight659 = UserSecValue(659)
			Dim bRight660 = UserSecValue(660)
			Dim bRight654 = UserSecValue(654)

			Dim bRight602 = UserSecValue(602)
			Dim bRight673 = UserSecValue(673)
			Dim bRight678 = (m_MandantData.ModulLicenseKeys(ModulConstants.MDData.MDNr, Now.Year, "").CVDropIN AndAlso UserSecValue(678))

			tiKandidat.Visible = bRight101
			tiKunde.Visible = bRight201

			tiVakanz.Visible = bRight701
			tiPropose.Visible = bRight801
			tiApplicationMng.Visible = True
			tiEinsatz.Visible = bRight250
			tiReport.Visible = bRight300
			tiVorschuss.Visible = bRight349
			tiLohn.Visible = bRight16

			tiFaktura.Visible = bRight14
			tiZahlung.Visible = bRight15
			tiMahn.Visible = bRight14
			tiGutschriften.Visible = bRight14
			tiFremdrechnung.Visible = False ' bRight100100

			tiMAListing.Visible = bRight601

			tiKDListing.Visible = bRight602
			tiTelefonListing.Visible = bRight602
			tiNachrichtenListing.Visible = bRight602

			tiVakListing.Visible = bRight621
			tiProposeListing.Visible = bRight643
			tiESListing.Visible = bRight603
			tiZGListing.Visible = bRight605
			tiLOListing.Visible = bRight556 And bRight557

			tiDebitorenListing.Visible = bRight606
			tiFOPListing.Visible = False ' bRight100105
			tiZEListing.Visible = bRight607
			tiDB1Listing.Visible = bRight608
			tiKDUmsatzListing.Visible = bRight610

			tiMandanten.Visible = bRight650
			tiDeleteprokoll.Visible = bRight673

			tiLohnartenstamm.Visible = bRight552
			tiEinstellungen.Visible = bRight666

			tiDokument.Visible = bRight659
			tiTabellen.Visible = bRight660
			tiBenutzer.Visible = bRight654

			If Not bRight650 AndAlso Not bRight552 AndAlso Not bRight666 AndAlso Not bRight659 AndAlso Not bRight654 AndAlso Not bRight659 Then
				Me.xtabEVVerwaltung.PageEnabled() = False
			End If
			m_Logger.LogDebug("Ending: SetUserAllowedAction")


		Catch ex As Exception
			m_Logger.LogError(String.Format("Userrights: {0}", ex.Message))
			m_UtilityUI.ShowErrorDialog(String.Format("Userrights: {0}", ex.Message))

		End Try


	End Sub

	Private Sub frmMainView_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

		If ModulConstants.MDData Is Nothing Then Return
		Dim FORM_XML_MAIN_KEY As String = "UserProfile/programsetting"
		Dim m_path As New SPProgUtility.ProgPath.ClsProgPath

		Dim askonexit As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_MandantData.GetSelectedMDUserProfileXMLFilename(ModulConstants.MDData.MDNr,
																																	  ModulConstants.UserData.UserNr),
																					String.Format("{0}/askonexit", FORM_XML_MAIN_KEY)), False)

		Try
			If askonexit.HasValue AndAlso askonexit Then
				Dim msg As String = m_Translate.GetSafeTranslationValue("Sie beenden das Programm. Sind Sie sicher?")
				If m_UtilityUI.ShowYesNoDialog(msg, "Programm verlassen", MessageBoxDefaultButton.Button1) = False Then
					e.Cancel = True
				End If
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try


	End Sub

	Private Sub OnFormLoad(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Me.KeyPreview = True
			Dim strStyleName As String = m_MandantData.GetSelectedUILayoutName(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr, String.Empty)
			Me.cboLFormStyle.EditValue = strStyleName
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Formstyle. {1}", strMethodeName, ex.Message))
		End Try

		Try
			If My.Settings.SETTING_MAIN_HEIGHT > 0 Then Me.Height = Math.Max(Me.Height, My.Settings.SETTING_MAIN_HEIGHT)
			If My.Settings.SETTING_MAIN_WIDTH > 0 Then Me.Width = Math.Max(Me.Width, My.Settings.SETTING_MAIN_WIDTH)
			If My.Settings.SETTING_MAIN_LOCATION <> String.Empty Then
				Dim aLoc As String() = My.Settings.SETTING_MAIN_LOCATION.Split(CChar(";"))
				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Val(aLoc(0)), Val(aLoc(1)))
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Formsizing. {1}", strMethodeName, ex.Message))
		End Try

		'Try
		'	advisorPicture.Properties.ShowMenu = False
		'	advisorPicture.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze

		'	TrasnlateControls()

		'	' load data for external docs
		'	Dim _ClsNewPrint As New ClsOpenModul(Nothing)
		'	_ClsNewPrint.ShowContextMenu4ExternDocuments(cmdExternalDocuments)


		'Catch ex As Exception
		'	' m_Logger.LogError(ex.ToString)
		'End Try

		RestoreTicLayoutFromXML()

	End Sub

	Private Sub OnTimerTick(ByVal sender As Object, ByVal e As System.EventArgs) Handles timer.Tick
		If lockTimerCounter = 0 Then
			lockTimerCounter += 1
			UpdateClock(DateTime.Now, scaleHours, scaleMinutes, scaleSeconds)
			lockTimerCounter -= 1
		End If
	End Sub

	Private Sub UpdateClock(ByVal dt As DateTime, ByVal h As IArcScale, ByVal m As IArcScale, ByVal s As IArcScale)
		Dim hour As Integer = If(dt.Hour <= 12, dt.Hour, dt.Hour - 12)
		Dim min As Integer = dt.Minute
		Dim sec As Integer = dt.Second

		h.Value = CSng(hour) + CSng(min) / 60.0F
		m.Value = (CSng(min) + CSng(sec) / 60.0F) / 5.0F
		s.Value = sec / 5.0F

	End Sub

	Private Sub OnFormDisposed(sender As Object, e As System.EventArgs) Handles Me.Disposed

		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.SETTING_MAIN_LOCATION = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.SETTING_MAIN_WIDTH = Me.Width
				My.Settings.SETTING_MAIN_HEIGHT = Me.Height

				My.Settings.Save()
			End If

			lockTimerCounter += 1
			If timer IsNot Nothing Then
				timer.Stop()
				timer.Dispose()
				timer = Nothing
			End If

			bgwUpdate.CancelAsync()

		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub

	Private Sub OnfrmKeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
		Dim m_path As New SPProgUtility.ClsProgSettingPath

		If e.KeyCode = Keys.F12 And ModulConstants.UserData.UserNr = 1 Then
			Dim strRAssembly As String = ""
			Dim strMsg As String = "Information über Modul: {0}{4}Bibliothek: {1}{0}Ort: {2}{0}{0}ReferencedAssamblies:{0}{3}{0}"

			Dim strProgFileSetting As String = "Mandantenverzeichnis: {1}{0}Benutzerverzeichnis: {2}{0}Temporärverzeichnis: {3}{0}{0}Server: {4}{0}{0}"
			strProgFileSetting = String.Format(strProgFileSetting, vbNewLine, m_MandantData.GetSelectedMDDataXMLFilename(ModulConstants.MDData.MDNr, Now.Year),
											   m_MandantData.GetSelectedMDUserProfileXMLFilename(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr),
											   m_path.GetSpSTempFolder,
											   m_path.GetSrvRootPath)

			For Each a In AppDomain.CurrentDomain.GetAssemblies()
				strRAssembly &= String.Format("-->> {2}{0}{1}({3}){0}", vbNewLine, vbTab, a, a.CodeBase) ' GetExecutingAssembly.GetReferencedAssemblies(i).FullName)
			Next
			strMsg = String.Format(strMsg, vbNewLine, GetExecutingAssembly().FullName, GetExecutingAssembly().Location, strRAssembly, strProgFileSetting)
			DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, "Modul-Information", MessageBoxButtons.OK, MessageBoxIcon.Information)

		ElseIf (e.KeyCode And Not Keys.Modifiers) = Keys.T AndAlso e.Modifiers = Keys.Control Then
			Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMANr = Nothing})

			obj.ShowTarifCalculator()

		ElseIf (e.KeyCode And Not Keys.Modifiers) = Keys.M AndAlso e.Modifiers = Keys.Control Then
			ShowMandantSelection()

		End If

	End Sub

	Private Sub LoadUserImage()
		Dim usNr = ModulConstants.UserData.UserNr

		If ModulConstants.UserData.UserNr > 0 Then
			Dim userImage = m_DatabaseAccess.LoadUserImageData(usNr)
			If ((Not userImage Is Nothing) AndAlso (Not userImage.UserImage Is Nothing) AndAlso (userImage.UserImage.Count > 0)) Then
				Dim memoryStream As New System.IO.MemoryStream(userImage.UserImage)
				advisorPicture.Image = Image.FromStream(memoryStream)
				Return
			End If
		End If

		advisorPicture.Image = Nothing
		advisorPicture.Properties.NullText = m_Translate.GetSafeTranslationValue("Kein Bild vorhanden!")

	End Sub


	Private Sub TrasnlateControls()

		Me.xtabAllgemein.Text = m_Translate.GetSafeTranslationValue(Me.xtabAllgemein.Text)
		Me.xtabListing.Text = m_Translate.GetSafeTranslationValue(Me.xtabListing.Text)
		Me.xtabVerwaltung.Text = m_Translate.GetSafeTranslationValue(Me.xtabVerwaltung.Text)
		Me.xtabEVExtras.Text = m_Translate.GetSafeTranslationValue(Me.xtabEVExtras.Text)
		Me.xtabEVVerwaltung.Text = m_Translate.GetSafeTranslationValue(Me.xtabEVVerwaltung.Text)

		Me.tiKandidat.Text = m_Translate.GetSafeTranslationValue(tiKandidat.Text)
		Me.tiKunde.Text = m_Translate.GetSafeTranslationValue(tiKunde.Text)
		Me.tiVakanz.Text = m_Translate.GetSafeTranslationValue(tiVakanz.Text)
		Me.tiPropose.Text = m_Translate.GetSafeTranslationValue(tiPropose.Text)
		Me.tiMassenversand.Text = m_Translate.GetSafeTranslationValue(tiMassenversand.Text)
		Me.tiApplicationMng.Text = m_Translate.GetSafeTranslationValue(tiApplicationMng.Text)

		Me.tiEinsatz.Text = m_Translate.GetSafeTranslationValue(tiEinsatz.Text)
		Me.tiReport.Text = m_Translate.GetSafeTranslationValue(tiReport.Text)
		Me.tiVorschuss.Text = m_Translate.GetSafeTranslationValue(tiVorschuss.Text)
		Me.tiLohn.Text = m_Translate.GetSafeTranslationValue(tiLohn.Text)

		Me.tiFaktura.Text = m_Translate.GetSafeTranslationValue(tiFaktura.Text)
		Me.tiZahlung.Text = m_Translate.GetSafeTranslationValue(tiZahlung.Text)
		Me.tiMahn.Text = m_Translate.GetSafeTranslationValue(tiMahn.Text)
		Me.tiGutschriften.Text = m_Translate.GetSafeTranslationValue(tiGutschriften.Text)
		Me.tiFremdrechnung.Text = m_Translate.GetSafeTranslationValue(tiFremdrechnung.Text)

		Me.tiMAListing.Text = m_Translate.GetSafeTranslationValue(tiMAListing.Text)
		Me.tiKDListing.Text = m_Translate.GetSafeTranslationValue(tiKDListing.Text)
		Me.tiVakListing.Text = m_Translate.GetSafeTranslationValue(tiVakListing.Text)
		Me.tiProposeListing.Text = m_Translate.GetSafeTranslationValue(tiProposeListing.Text)
		Me.tiTelefonListing.Text = m_Translate.GetSafeTranslationValue(tiTelefonListing.Text)
		Me.tiNachrichtenListing.Text = m_Translate.GetSafeTranslationValue(tiNachrichtenListing.Text)
		Me.tiESListing.Text = m_Translate.GetSafeTranslationValue(tiESListing.Text)

		Me.tiRPDatenListing.Text = m_Translate.GetSafeTranslationValue(tiRPDatenListing.Text)
		Me.tiZGListing.Text = m_Translate.GetSafeTranslationValue(tiZGListing.Text)
		Me.tiLOListing.Text = m_Translate.GetSafeTranslationValue(tiLOListing.Text)
		Me.tiDebitorenListing.Text = m_Translate.GetSafeTranslationValue(tiDebitorenListing.Text)
		Me.tiZEListing.Text = m_Translate.GetSafeTranslationValue(tiZEListing.Text)
		Me.tiFOPListing.Text = m_Translate.GetSafeTranslationValue(tiFOPListing.Text)
		Me.tiDB1Listing.Text = m_Translate.GetSafeTranslationValue(tiDB1Listing.Text)
		Me.tiKDUmsatzListing.Text = m_Translate.GetSafeTranslationValue(tiKDUmsatzListing.Text)

		Me.tiUpdateprotokoll.Text = m_Translate.GetSafeTranslationValue(tiUpdateprotokoll.Text)
		Me.tiFernwartung.Text = m_Translate.GetSafeTranslationValue(tiFernwartung.Text)
		Me.tiBenutzerhandbuch.Text = m_Translate.GetSafeTranslationValue(tiBenutzerhandbuch.Text)
		Me.tiGAVNews.Text = m_Translate.GetSafeTranslationValue(tiGAVNews.Text)

		Me.tiMandanten.Text = m_Translate.GetSafeTranslationValue(tiMandanten.Text)
		Me.tiLohnartenstamm.Text = m_Translate.GetSafeTranslationValue(tiLohnartenstamm.Text)
		Me.tiEinstellungen.Text = m_Translate.GetSafeTranslationValue(tiEinstellungen.Text)
		Me.tiTabellen.Text = m_Translate.GetSafeTranslationValue(tiTabellen.Text)
		Me.tiBenutzer.Text = m_Translate.GetSafeTranslationValue(tiBenutzer.Text)
		Me.tiDokument.Text = m_Translate.GetSafeTranslationValue(tiDokument.Text)


	End Sub



#Region "Itemclick MainModuls..."

	Private Sub tiKandidat_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiKandidat.ItemClick

		Me.[GoTo](Of pageKandidaten)()

	End Sub

	Private Sub tiKunde_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiKunde.ItemClick

		Me.[GoTo](Of pageKunden)()

	End Sub

	Private Sub tiVakanz_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiVakanz.ItemClick

		Me.[GoTo](Of pgVakanzen)()

	End Sub

	Private Sub tiPropose_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiPropose.ItemClick

		Me.[GoTo](Of pgPropose)()

	End Sub

	Private Sub tiMassenversand_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiMassenversand.ItemClick

		Me.[GoTo](Of pgOffers)()

	End Sub

	Private Sub OntiApplicationMng_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiApplicationMng.ItemClick
		Dim bRight678 = (m_MandantData.ModulLicenseKeys(ModulConstants.MDData.MDNr, Now.Year, "").CVDropIN AndAlso UserSecValue(678))

		If bRight678 Then
			Me.[GoTo](Of ucApplication)()
		Else
			m_Logger.LogWarning(String.Format("{0}: Module is not licenced!!!", "tiApplicationMng"))
			Process.Start("http://downloads.domain.com/sps_downloads/PDF/infos/alle_bewerbungen_auf_einen_blick.pdf")
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Für dieses Modul sind Sie nicht lizenziert! Bitte kontaktieren Sie Ihren Softwarelieferanten."), m_Translate.GetSafeTranslationValue("Nicht lizenziertes Modul"), MessageBoxIcon.Warning)
		End If

	End Sub


	Private Sub tiEinsatz_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiEinsatz.ItemClick

		Me.[GoTo](Of pgES)()

	End Sub

	Private Sub tiReport_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiReport.ItemClick
		m_RPSuppressUIEvents = True
		Me.[GoTo](Of pgReport)()
		m_RPSuppressUIEvents = False
	End Sub

	Private Sub tiVorschuss_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiVorschuss.ItemClick

		Me.[GoTo](Of pgVorschuss)()

	End Sub

	Private Sub tiLohn_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiLohn.ItemClick

		Me.[GoTo](Of pageSalary)()

	End Sub


	Private Sub tiFaktura_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiFaktura.ItemClick

		Me.[GoTo](Of pageDebitoren)()

	End Sub

	Private Sub tiZahlung_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiZahlung.ItemClick

		Me.[GoTo](Of pageZE)()

	End Sub

	Private Sub tiGutschriften_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiGutschriften.ItemClick

		Me.[GoTo](Of pageGutschrift)()

	End Sub

	Private Sub OntiMahn_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiMahn.ItemClick

		Me.[GoTo](Of pageMahnung)()

	End Sub

	Private Sub OntiFremdrechnung_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiFremdrechnung.ItemClick

		Me.[GoTo](Of pageFOP)()

	End Sub


#End Region


#Region "Listing und Auswertungen..."

	Private Sub tiMAListing_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiMAListing.ItemClick
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim obj As New SPMASearch.ClsMain_Net(New SPMASearch.ClsSetting With {.SelectedMDNr = ModulConstants.MDData.MDNr, .LogedUSNr = ModulConstants.UserData.UserNr})
		obj.ShowfrmMASearch()

	End Sub

	Private Sub tiKDListing_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiKDListing.ItemClick
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim obj As New SPKDSearch.ClsMain_Net(New SPKDSearch.ClsSetting With {.SelectedMDNr = ModulConstants.MDData.MDNr, .LogedUSNr = ModulConstants.UserData.UserNr})
		obj.ShowfrmKDSearch()

	End Sub

	Private Sub tiVakListing_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiVakListing.ItemClick
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim obj As New SPVakSearch.ClsMain_Net(New SPVakSearch.ClsSetting With {.SelectedMDNr = ModulConstants.MDData.MDNr, .LogedUSNr = ModulConstants.UserData.UserNr})
		obj.ShowfrmVakSearch()

	End Sub

	Private Sub tiProposeListing_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiProposeListing.ItemClick
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim obj As New SPProposeSearch.ClsMain_Net(New SPProposeSearch.ClsSetting With {.SelectedMDNr = ModulConstants.MDData.MDNr, .LogedUSNr = ModulConstants.UserData.UserNr})
		obj.ShowfrmProposeSearch()

	End Sub

	Private Sub titelefonListing_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiTelefonListing.ItemClick
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim obj As New SPCallHistory.ClsMain_Net(New SPCallHistory.ClsSetting With {.SelectedMDNr = ModulConstants.MDData.MDNr, .LogedUSNr = ModulConstants.UserData.UserNr})
		obj.ShowfrmCallHistory()

	End Sub


	Private Sub tiNachrichtenListing_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiNachrichtenListing.ItemClick
		Dim showNewModul As Boolean = True ' My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown

		'Dim m_init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)
		If My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown Then

			Dim o2Open As New frmMain(m_InitializationData)
			o2Open.Show()
			o2Open.BringToFront()

		Else
			Dim o2Open As New frmeCalllogs(m_InitializationData)
			o2Open.LoadData()

			o2Open.Show()
			o2Open.BringToFront()

		End If
		'Dim obj As New SPSSendMail.ClsMain_Net '(New SPProposeSearch.ClsSetting With {.SelectedMDNr = ModulConstants.MDData.MDNr, .LogedUSNr = ModulConstants.UserData.UserNr})
		'obj.OpenMailingList(1, String.Empty)

	End Sub

	Private Sub tiRPDatenListing_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiRPDatenListing.ItemClick
		'Dim m_init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		Dim obj As New SP.RPContent.PrintUtility.ClsMain_Net(New SP.RPContent.PrintUtility.ClsRPCSetting With {.SelectedMDNr = ModulConstants.MDData.MDNr, .LogedUSNr = ModulConstants.UserData.UserNr})
		obj.ShowfrmRPContent4Print()

	End Sub

	Private Sub tiESListing_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiESListing.ItemClick
		'Dim m_init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		Dim obj As New SPESSearch.ClsMain_Net(New SPESSearch.ClsSetting With {.SelectedMDNr = ModulConstants.MDData.MDNr, .LogedUSNr = ModulConstants.UserData.UserNr})
		obj.ShowfrmESSearch()

	End Sub

	Private Sub tiZG_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiZGListing.ItemClick
		'Dim m_init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		Dim frmZGSearch As New SPZGSearch.frmZGSearch(m_InitializationData)

		frmZGSearch.Show()
		frmZGSearch.BringToFront()

	End Sub

	Private Sub tiLOListing_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiLOListing.ItemClick
		'Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Me.[GoTo](Of pgLOListing)()

	End Sub


	Private Sub tiDebitorenListing_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiDebitorenListing.ItemClick
		'Dim m_init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		Dim obj As New SPOPSearch.ClsMain_Net(New SPOPSearch.ClsSetting With {.SelectedMDNr = ModulConstants.MDData.MDNr, .LogedUSNr = ModulConstants.UserData.UserNr})
		obj.ShowfrmOPSearch()

	End Sub

	Private Sub tiZEListing_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiZEListing.ItemClick
		'Dim m_init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		Dim frmZEListing = New SPZESearch.frmZESearch(m_InitializationData)

		frmZEListing.Show()
		frmZEListing.BringToFront()

	End Sub

	Private Sub tiFOPListing_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiFOPListing.ItemClick
		'Dim m_init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		Dim obj As New SPFOPSearch.frmFOPSearch(m_InitializationData)
		obj.Show()
		obj.BringToFront()

	End Sub

	Private Sub tiDB1Listing_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiDB1Listing.ItemClick
		'Dim m_init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		Dim obj As New SPRPUmsatzTotal.ClsMain_Net(m_InitializationData)
		obj.ShowFrmUmsDb1()

	End Sub

	Private Sub tiKDUmsatzListing_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiKDUmsatzListing.ItemClick
		'Dim m_init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		Dim obj As New SPKDUmsatz.ClsMain_Net(m_InitializationData)   'New SPKDUmsatz.ClsSetting( With {.SelectedMDNr = ModulConstants.MDData.MDNr, .LogedUSNr = ModulConstants.UserData.UserNr})
		obj.ShowListForm()

	End Sub


#End Region


#Region "Extras und Verwaltung"

	Private Sub tiUpdateProtokoll_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiUpdateprotokoll.ItemClick
		Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMANr = Nothing})

		obj.ShowUpdateNews()

		'Dim frm As New frmUpdate(False)

		'frm.LoadFilesListForUpdate()
		'If frm.m_UpdateFileresult.Count > 0 Then
		'	frm.LoadUpdateresultList()
		'End If
		'frm.Show()
		'frm.BringToFront()

	End Sub

	Private Sub tiFernwartung_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiFernwartung.ItemClick
		Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMANr = Nothing})

		obj.ShowExtrasFernwartung()

	End Sub

	Private Sub tiBenutzerhandbuch_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiBenutzerhandbuch.ItemClick
		Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMANr = Nothing})

		obj.ShowExtrasBenutzerhandbuch()

	End Sub

	Private Sub tiGAVNews_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiGAVNews.ItemClick
		Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMANr = Nothing})

		obj.ShowExtrasGAVNews()

	End Sub

	Private Sub OntiDeleteProtokoll_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiDeleteprokoll.ItemClick
		Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = ModulConstants.MDData.MDNr})

		obj.ShowExtrasDeleteProtokoll()

	End Sub

	Private Sub OntiMandantListing_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiMandantListing.ItemClick

		ShowMandantSelection()

	End Sub


#End Region


#Region "Verwaltung"

	Private Sub tiMandanten_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiMandanten.ItemClick
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMANr = Nothing})

		obj.ShowVMandanten()
		' m_Logger.LogInfo(String.Format("{0} wurde gestartet...", strMethodeName))

	End Sub

	Private Sub tiLohnartenstamm_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiLohnartenstamm.ItemClick
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMANr = Nothing})

		obj.ShowVLohnartenstamm()
		' m_Logger.LogInfo(String.Format("{0} wurde gestartet...", strMethodeName))

	End Sub

	Private Sub tiEinstellungen_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiEinstellungen.ItemClick
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMANr = Nothing})

		obj.ShowVEinstellung()
		' m_Logger.LogInfo(String.Format("{0} wurde gestartet...", strMethodeName))

	End Sub

	Private Sub tiTabellen_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiTabellen.ItemClick
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMANr = Nothing})

		obj.ShowVTabellen()
		' m_Logger.LogInfo(String.Format("{0} wurde gestartet...", strMethodeName))

	End Sub

	Private Sub tiBenutzer_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiBenutzer.ItemClick
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMANr = Nothing})

		obj.ShowVBenutzer()
		' m_Logger.LogInfo(String.Format("{0} wurde gestartet...", strMethodeName))

	End Sub

	Private Sub tiDokument_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiDokument.ItemClick
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMANr = Nothing})

		obj.ShowVDokument()
		' m_Logger.LogInfo(String.Format("{0} wurde gestartet...", strMethodeName))

	End Sub


#End Region



#Region "FormStyle"

	Private Sub OncmdUserLayoutClick(sender As System.Object, e As System.EventArgs) Handles cmdUserLayout.Click
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		PopupControlContainer1.SuspendLayout()
		Me.PopupControlContainer1.Manager = New DevExpress.XtraBars.BarManager
		PopupControlContainer1.ShowCloseButton = True
		PopupControlContainer1.ShowSizeGrip = True

		PopupControlContainer1.ShowPopup(Cursor.Position)
		PopupControlContainer1.ResumeLayout()

	End Sub

	Private Sub cboLFormStyle_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cboLFormStyle.QueryPopUp

		Me.cboLFormStyle.Properties.Items.Clear()
		For Each cnt As DevExpress.Skins.SkinContainer In DevExpress.Skins.SkinManager.Default.Skins
			Me.cboLFormStyle.Properties.Items.Add(cnt.SkinName)
		Next cnt

	End Sub

	Private Sub cboLFormStyle_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboLFormStyle.SelectedIndexChanged
		Dim comboBox As ComboBoxEdit = sender
		Dim skinName As String = comboBox.Text

		DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName = skinName
		SaveSelectedLayout()

	End Sub

	Private Sub SaveSelectedLayout()

		Try
			Dim strUserprofile As String = m_MandantData.GetSelectedMDUserProfileXMLFilename(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)
			Dim xEle As XElement = XElement.Load(strUserprofile)
			Dim MyNode = xEle.Elements("Layouts").Elements("Form_DevEx").Elements("FormStyle").ToList()
			If MyNode.Count > 0 Then
				For Each cEle As XElement In MyNode
					cEle.ReplaceNodes(String.Format("{0}", Me.cboLFormStyle.EditValue))
				Next cEle

			Else
				xEle.AddFirst(New XElement("Layouts", New XElement("Form_DevEx", New XElement("FormStyle", Me.cboLFormStyle.EditValue))))
			End If

			xEle.Save(strUserprofile)

		Catch ex As Exception

		End Try

	End Sub

	Private Function InsertTextNode(ByVal xDoc As XmlDocument, ByVal xNode As XmlNode, ByVal strTag As String, ByVal strText As String) As XmlElement
		Dim xNodeTemp As XmlNode

		xNodeTemp = xDoc.CreateElement(strTag)
		xNodeTemp.AppendChild(xDoc.CreateTextNode(strText))
		xNode.AppendChild(xNodeTemp)

		Return CType(xNodeTemp, XmlElement)
	End Function


#End Region



	Private Sub frmMainView_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
		SplashScreenManager.CloseForm(False)

		SendLoginDataViaWebService()
		LoadProgramModuls()

		m_MainViewGridData = New SuportedCodes
		ListOfMandantData = m_MainViewGridData.LoadFoundedMDList()

	End Sub

	Private Sub OnTicMainModules_VisibleChanged(sender As Object, e As System.EventArgs) Handles ticMainModules.VisibleChanged

		If ticMainModules.Visible Then
			Trace.WriteLine("TicMainModules: is Visible!")
			Me.bsiInfo.Caption = "Bereit"
		End If

	End Sub

	Private Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

		Dim m_md As New SPProgUtility.Mandanten.Mandant
		Dim clsMandant = ModulConstants.MDData
		Dim logedUserData = ModulConstants.UserData
		Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

		Dim clsTransalation As New ClsTranslation
		Dim translate = clsTransalation.GetTranslationInObject

		Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

	End Function

	Sub ShowMandantSelection()

		Dim oldMDNr As Integer = ModulConstants.MDData.MDNr

		Dim test As New frmMDSelection()
		test.BringToFront()
		test.ShowDialog()

		If ModulConstants.MDData Is Nothing Then
			Me.Close()

			Return
		End If
		If oldMDNr <> ModulConstants.MDData.MDNr Then

			CheckSettingForFilesAndDirectories()
			xtabMain.SelectedTabPage = xtabAllgemein
			SendLoginDataViaWebService()

		End If

	End Sub

	Sub SetMainFormName(ByVal reWritebsiInfobar As Boolean)
		Dim m_DataAccess As New MainGrid

		If m_MDNr.GetValueOrDefault(0) <> ModulConstants.MDData.MDNr Then
			m_InitializationData = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

			DownloadCommonSettingFilesViaWebService()
			m_MDNr = ModulConstants.MDData.MDNr

			If m_AllowedTempDataPVL Then VerifyUserGAVPublicationNews()
		End If

		Me.Text = String.Format(ProgHeaderName, ModulConstants.MDData.MDName, ModulConstants.MDData.MDDbName)
		Me.SplitContainerControl2.Text = String.Format(ProgHeaderName, ModulConstants.MDData.MDName, ModulConstants.MDData.MDDbName)
		m_ExistsToDoNews = False

		Dim listOfCustomerBill = m_DataAccess.GetDbTODOData4Show("[Mainview.Show TODO Data In MainView]", ModulConstants.UserData.UserNr)
		If Not listOfCustomerBill Is Nothing AndAlso listOfCustomerBill.Count > 0 Then
			m_ExistsToDoNews = True

			If m_ExistsGAVNews Then
				bbiUpdatAnnouncement.Caption = String.Format(m_Translate.GetSafeTranslationValue("Neue Nachrichten sind vorhanden!"))
			Else
				bbiUpdatAnnouncement.Caption = String.Format(m_Translate.GetSafeTranslationValue("Neue Nachrichten: {0}"), listOfCustomerBill.Count)
			End If

			bbiUpdatAnnouncement.Visibility = BarItemVisibility.Always
			If Not reWritebsiInfobar Then PageTODO1.LoadData()
		Else
			If Not m_ExistsGAVNews Then bbiUpdatAnnouncement.Visibility = BarItemVisibility.Never
		End If


		If reWritebsiInfobar Then bsiInfo.Caption = String.Format("{0}", m_Translate.GetSafeTranslationValue("Bereit"))
		bsiMDData.Caption = String.Format("{0}: {1}, {2}", ModulConstants.MDData.MDNr, ModulConstants.MDData.MDName, ModulConstants.MDData.MDCity)
		bsiMDPath.Caption = String.Format("{0}", ModulConstants.MDData.MDMainPath)
		bsiDbSrvName.Caption = String.Format("{0}.{1}", ModulConstants.MDData.MDDbServer, ModulConstants.MDData.MDDbName)
		bsiUserData.Caption = String.Format("{0}: {1}@{2}", ModulConstants.UserData.UserNr, ModulConstants.UserData.UserLoginname, ModulConstants.UserData.UserFullNameWithComma)


	End Sub


#Region "saving and restoring layout of tilecontrol"

	Sub RestoreTicLayoutFromXML()
		' bis auf weiteres...
		Return

		'Try
		'	If ModulConstants.UserData.UserNr <> 1 Then Return

		'	If File.Exists(m_MainTileLayoutFile) Then ticMainModules.RestoreLayoutFromXml(m_MainTileLayoutFile)
		'	If File.Exists(m_ListingTileLayoutFile) Then ticListingModules.RestoreLayoutFromXml(m_ListingTileLayoutFile)
		'	If File.Exists(m_ExtraTileLayoutFile) Then ticExtraModules.RestoreLayoutFromXml(m_ExtraTileLayoutFile)
		'	If File.Exists(m_ManagementTileLayoutFile) Then ticManagementModules.RestoreLayoutFromXml(m_ManagementTileLayoutFile)

		'Catch ex As Exception
		'	m_Logger.LogError(String.Format("SaveLayoutofTiles: {0}", ex.Message))
		'End Try

	End Sub

	'Private Sub ticMainModules_EndItemDragging(sender As Object, e As TileItemDragEventArgs) Handles ticMainModules.EndItemDragging
	'	ticMainModules.SaveLayoutToXml(m_MainTileLayoutFile)
	'End Sub

	'Private Sub ticListingModules_EndItemDragging(sender As Object, e As TileItemDragEventArgs) Handles ticListingModules.EndItemDragging
	'	ticListingModules.SaveLayoutToXml(m_ListingTileLayoutFile)
	'End Sub

	'Private Sub ticExtraModules_EndItemDragging(sender As Object, e As TileItemDragEventArgs) Handles ticExtraModules.EndItemDragging
	'	ticExtraModules.SaveLayoutToXml(m_ExtraTileLayoutFile)
	'End Sub

	'Private Sub ticManagementModules_EndItemDragging(sender As Object, e As TileItemDragEventArgs) Handles ticManagementModules.EndItemDragging
	'	ticManagementModules.SaveLayoutToXml(m_ManagementTileLayoutFile)
	'End Sub

#End Region


#Region "sendig login data to webservice"

	''' <summary>
	''' send login data over web service.
	''' </summary>
	Private Sub SendLoginDataViaWebService()

		Dim uiSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext()


		Task(Of Boolean).Factory.StartNew(Function() PerformSendingLoginDataWebserviceCallAsync(),
										  CancellationToken.None,
										  TaskCreationOptions.None,
										  TaskScheduler.Default).ContinueWith(Sub(t) FinishSendingLoginDataWebserviceCallTask(t), CancellationToken.None,
																			  TaskContinuationOptions.None, uiSynchronizationContext)

	End Sub

	''' <summary>
	'''  Performs sending login data asynchronous.
	''' </summary>
	''' <returns>The report response.</returns>
	Private Function PerformSendingLoginDataWebserviceCallAsync() As Boolean
		Dim success As Boolean = True
		Dim _setting = New SP.Infrastructure.Initialization.InitializeClass(ModulConstants.TranslationData, ModulConstants.ProsonalizedData, ModulConstants.MDData, ModulConstants.UserData)

		Dim webservice As New SP.Internal.Automations.SendSputnikLoginInfomations(_setting)

		Try
			' Read data over webservice
			success = success AndAlso webservice.AddSputnikLoginDataWithWebservice()
			success = success AndAlso webservice.SendSputnikUserDataWithWebservice(ModulConstants.UserData.UserNr)
			'success = success AndAlso webservice.SendSputnikUserDataWithWebservice(ModulConstants.UserData.UserNr)

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

	Private Sub DownloadCommonSettingFilesViaWebService()

		Dim uiSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext()

		Task(Of Boolean).Factory.StartNew(Function() PerformDownloadingCommonSettingFilesWebserviceCallAsync(),
										  CancellationToken.None,
										  TaskCreationOptions.None,
										  TaskScheduler.Default).ContinueWith(Sub(t) FinishDownloadingCommonSettingFilesWebserviceCallTask(t), CancellationToken.None,
																			  TaskContinuationOptions.None, uiSynchronizationContext)

	End Sub

	''' <summary>
	'''  Performs sending login data asynchronous.
	''' </summary>
	''' <returns>The report response.</returns>
	Private Function PerformDownloadingCommonSettingFilesWebserviceCallAsync() As Boolean
		Dim success As Boolean = True
		'Dim _setting = New SP.Infrastructure.Initialization.InitializeClass(m_InitializationData) 
		'ModulConstants.TranslationData, ModulConstants.ProsonalizedData, ModulConstants.MDData, ModulConstants.UserData)

		Dim webservice As New SP.Internal.Automations.BaseTable.SPSBaseTables(m_InitializationData)

		Try
			' Read data over webservice
			success = success AndAlso webservice.PerformDownloadingCommonSettingFilesOverWebService()

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			success = False
		End Try


		Return success

	End Function

	''' <summary>
	''' Finish sending login data web service call.
	''' </summary>
	Private Sub FinishDownloadingCommonSettingFilesWebserviceCallTask(ByVal t As Task(Of Boolean))

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

#End Region


#Region "Backgroundworker..."


	''' <summary>
	''' loading program moduls as thread
	''' </summary>
	Private Sub LoadProgramModuls()

		Dim uiSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext()

		m_Logger.LogInfo("starting with loading moduls")
		Task(Of Boolean).Factory.StartNew(Function() PerformLoadingModulsCallAsync(), CancellationToken.None, TaskCreationOptions.None,
										  TaskScheduler.Default).ContinueWith(Sub(t) FinishLoadingModulsCallTask(t), CancellationToken.None,
																			  TaskContinuationOptions.None, uiSynchronizationContext)

	End Sub

	Private Function PerformLoadingModulsCallAsync() As Boolean
		Dim success As Boolean = True


		Try

			Try
				advisorPicture.Properties.ShowMenu = False
				advisorPicture.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze

				' load data for external docs
				Dim _ClsNewPrint As New ClsOpenModul(Nothing)
				_ClsNewPrint.ShowContextMenu4ExternDocuments(cmdExternalDocuments)

			Catch ex As Exception
				' m_Logger.LogError(ex.ToString)
			End Try

			Dim communicationHub = MessageService.Instance.Hub

			communicationHub.Subscribe(Of OpenEmployeeMngRequest)(AddressOf HandleOpenEmployeeMngFormMsg)
			communicationHub.Subscribe(Of OpenCustomerMngRequest)(AddressOf HandleOpenCustomerMngFormMsg)


			communicationHub.Subscribe(Of OpenEinsatzMngRequest)(AddressOf HandleOpenEinsatzMngFormMsg)
			communicationHub.Subscribe(Of OpenReportsMngRequest)(AddressOf HandleOpenReportsMngFormMsg)
			communicationHub.Subscribe(Of OpenInvoiceMngRequest)(AddressOf HandleOpenInvoicesMngFormMsg)

			communicationHub.Subscribe(Of OpenMAKontaktMngRequest)(AddressOf HandleOpenEmployeeContactMngFormMsg)
			communicationHub.Subscribe(Of OpenKDKontaktMngRequest)(AddressOf HandleOpenCustomerContactMngFormMsg)

			communicationHub.Subscribe(Of OpenAdvancePaymentMngRequest)(AddressOf HandleOpenAdvancePaymentMngFormMsg)

			communicationHub.Subscribe(Of PayrollProcessingStateHasChanged)(AddressOf HandlePayrollProcessingStateHasChanged)

			communicationHub.Subscribe(Of RefreshMainViewStatebar)(AddressOf HandleRefreshMainViewStatebarFormMsg)

			communicationHub.Subscribe(Of OpenResponsiblePersonMngRequest)(AddressOf HandleOpenResponsiblePersonMngFormMsg)
			communicationHub.Subscribe(Of OpenVacancyMngRequest)(AddressOf HandleOpenVacancyMngFormMsg)
			communicationHub.Subscribe(Of OpenProposeMngRequest)(AddressOf HandleOpenProposeMngFormMsg)

			InitializeMainModules()

			If m_AllowedTempDataPVL Then VerifyUserGAVPublicationNews()


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			success = False
		End Try


		Return success

	End Function

	''' <summary>
	''' Finish sending login data web service call.
	''' </summary>
	Private Sub FinishLoadingModulsCallTask(ByVal t As Task(Of Boolean))

		Try

			Select Case t.Status
				Case TaskStatus.RanToCompletion
					' Webservice call was successful.

					m_Logger.LogInfo("finish with loading moduls")

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








	Private Sub OnbgwUpdate_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bgwUpdate.DoWork
#If DEBUG Then
		Return
#End If
		Try
			ExistsNewupdate = False
			If ModulConstants.UserData.UserNr = 1 AndAlso Environment.UserName <> "username" Then Return
			Dim UpdateData = m_DatabaseAccess.GetLastRegisteredUpdate(ModulConstants.UserData.UserLoginname)


			Dim ws = New SPUpdateUtilitiesService.SPUpdateUtilitiesSoapClient
			ws.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_SPUpdateUtilitiesServiceUrl)


			' Read data over webservice
			Dim searchResult As SPUpdateUtilitiesService.UpdateUtilitiesDTO = ws.GetUpdateNotification()

			If searchResult Is Nothing Then Exit Sub
			UpdateProtokollID = searchResult.UpdateID

			If UpdateProtokollID.HasValue AndAlso UpdateProtokollID > 0 Then
				If Not UpdateData Is Nothing Then
					If Not UpdateData.RecID.HasValue OrElse UpdateData.RecID < UpdateProtokollID Then
						ExistsNewupdate = True
					End If
				Else
					ExistsNewupdate = True
				End If
			End If

			If ModulConstants.UserData.UserNr = 1 AndAlso Environment.UserName <> "username" Then
				Dim frm As New frmUpdate(False)
				frm.LoadFilesListForUpdate()
				If frm.m_UpdateFileresult.Count > 0 Then
					frm.LoadUpdateresultList()
					frm.Show()
					frm.BringToFront()
					ExistsNewupdate = True
				Else
					ExistsNewupdate = False
				End If
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		If bgwUpdate.CancellationPending Then e.Cancel = True

	End Sub

	Private Sub OnbgwUpdate_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgwUpdate.RunWorkerCompleted

		If (e.Error IsNot Nothing) Then
			MessageBox.Show(e.Error.Message)
		Else
			If e.Cancelled = True Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Aktion abgebrochen!"))
				bbiUpdatAnnouncement.Visibility = False
			Else
				bgwUpdate.CancelAsync()

				bbiUpdatAnnouncement.Visibility = If(ExistsNewupdate, DevExpress.XtraBars.BarItemVisibility.Always, DevExpress.XtraBars.BarItemVisibility.Never)
				If ModulConstants.UserData.UserNr = 1 Then
					If ExistsNewupdate Then
						Dim msg = "Es sind neue Updates vorhanden, welche nicht automatisch installiert wurden! Bitte installieren Sie umgehend die Updates!"
						m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue(msg))
						OnbbiUpdatAnnouncement_ItemClick(Nothing, Nothing)
					End If
				End If
				If bbiUpdatAnnouncement.Visibility <> BarItemVisibility.Always Then bbiScan2Sputnik.Links(0).BeginGroup = False
			End If
		End If

	End Sub

#End Region

	Private Sub OnbbiUpdatAnnouncement_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiUpdatAnnouncement.ItemClick
		Dim m_DataAccess As New MainGrid

		Dim listOfCustomerBill = m_DataAccess.GetDbTODOData4Show("[Mainview.Show TODO Data In MainView]", ModulConstants.UserData.UserNr)

		If Not listOfCustomerBill Is Nothing AndAlso listOfCustomerBill.Count > 0 Then
			Dim _Clsre As New ClsOpenModul(New ClsSetting With {.SelectedTODONr = listOfCustomerBill(0).id})
			_Clsre.OpenTODOList()

		End If

	End Sub

	Private Sub OnbbiGAVPublication_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiGAVPublication.ItemClick

		LoadUserGAVPublicationNews()

	End Sub

	Private Sub OnbsiUserData_ItemDoubleClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bsiUserData.ItemDoubleClick

		If My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown AndAlso UserSecValue(654) Then
			Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMANr = Nothing})
			obj.ShowExtrasBenutzerhandbuch()

		Else
			Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile))

		End If

	End Sub

	Private Sub OnbsiMDPath_ItemDoubleClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bsiMDPath.ItemDoubleClick
		Process.Start(ModulConstants.MDData.MDMainPath)
	End Sub

	Private Sub OnbbiScan2Sputnik_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiScan2Sputnik.ItemClick

		If Not UserSecValue(677) OrElse Not m_MandantData.ModulLicenseKeys(ModulConstants.MDData.MDNr, Now.Year, "").ScanDropIN Then Return
		Try
			Dim frm = New SP.Main.Notify.UI.frmReportDropIn(m_InitializationData)

			frm.Show()
			frm.BringToFront()


		Catch ex As Exception
			m_Logger.LogError(String.Format("starting process: {0} | {1}", m_NotifyerFileName, ex.ToString))

		End Try


	End Sub

	Private Sub OnbbiCV2Sputnik_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiCV2Sputnik.ItemClick

		Try

			If Not UserSecValue(678) OrElse Not m_MandantData.ModulLicenseKeys(ModulConstants.MDData.MDNr, Now.Year, "").CVDropIN Then Return

			If My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown Then
				Dim frm = New SP.Main.Notify.UI.frmNotify(m_InitializationData)
				frm.Show()
				frm.BringToFront()

			Else
				Dim frm = New SP.Main.Notify.UI.frmCVDropIn(m_InitializationData)
				frm.Show()
				frm.BringToFront()
			End If



		Catch ex As Exception
			m_Logger.LogError(String.Format("starting process: {0} | {1}", m_NotifyerFileName, ex.ToString))

		End Try


	End Sub

	Private Sub OnbbiPMSputnik_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPMSputnik.ItemClick

		If Not UserSecValue(679) OrElse Not m_MandantData.ModulLicenseKeys(ModulConstants.MDData.MDNr, Now.Year, "").PMSearch Then Return
		Dim _setting = New SP.Infrastructure.Initialization.InitializeClass(ModulConstants.TranslationData, ModulConstants.ProsonalizedData, ModulConstants.MDData, ModulConstants.UserData)

		Try
			' Read data over webservice
			Dim frm = New SP.Internal.Automations.SPProfilMatcher.frmProfilmatcher(_setting)
			frm.PreselectionData = New SP.Internal.Automations.SPProfilMatcher.PreselectionData With {.MDNr = _setting.MDData.MDNr, .CustomerNumber = 24820}

			frm.PreselectData()
			frm.Show()
			frm.BringToFront()

		Catch ex As Exception
			m_Logger.LogError(String.Format("starting process: {0} | {1}", m_NotifyerFileName, ex.ToString))

		End Try

	End Sub


#Region "Helpers"

	Public Function DownloadFile(ByVal _url As String, ByVal _filename As String) As Boolean
		Try
			Dim _webClient As New System.Net.WebClient
			_webClient.DownloadFile(_url, _filename)

			Return IO.File.Exists(_filename)

		Catch ex As Exception
			m_Logger.LogError(String.Format("File {0} from {1} could not be downloaded! {2}", _filename, _url, ex.ToString))

			Return False
		End Try

	End Function



#End Region


End Class