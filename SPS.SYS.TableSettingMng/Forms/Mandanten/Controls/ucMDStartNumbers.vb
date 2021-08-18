
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls

Imports DevExpress.LookAndFeel

Imports SP.DatabaseAccess

Imports SP.DatabaseAccess.TableSetting
Imports SP.DatabaseAccess.TableSetting.DataObjects
Imports SP.DatabaseAccess.TableSetting.DataObjects.MandantData


Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Infrastructure

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages


Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.Mandanten
Imports DevExpress.XtraSplashScreen
Imports SPProgUtility.ProgPath
Imports SP.Infrastructure.Initialization


Public Class ucMDStartNumbers

#Region "Private Fields"


	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	Private m_TablesettingDatabaseAccess As ITablesDatabaseAccess
	''' <summary>
	''' The common data access object.
	''' </summary>
	Private m_MandantDatabaseAccess As MandantData

	Private m_SuppressUIEvents As Boolean

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

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	Private connectionString As String

	Private m_MandantXMLFile As String
	Private m_MandantFormXMLFileName As String
	Private m_MandantSetting As String
	Private m_SonstigesSetting As String
	Private m_StartnumberSetting As String
	Private m_SuvaSetting As String
	Private m_AHVSetting As String
	Private m_FAKSetting As String

	Private m_MandantSettingsXml As SPProgUtility.CommonXmlUtility.SettingsXml

	Private m_ProgPath As ClsProgPath

	Private m_Year As Integer

#End Region


#Region "private consts"

	Private Const MANDANT_XML_MAIN_KEY As String = "MD_{0}/"
	Private Const MANDANT_XML_SETTING_SPUTNIK_SONSTIGES_SETTING As String = "MD_{0}/Sonstiges"
	Private Const MANDANT_XML_SETTING_SPUTNIK_STARTNUMBER_SETTING As String = "MD_{0}/StartNr"
	Private Const MANDANT_XML_SETTING_SPUTNIK_AHV_SETTING As String = "MD_{0}/AHV-Daten"
	Private Const MANDANT_XML_SETTING_SPUTNIK_SUVA_SETTING As String = "MD_{0}/SUVA-Daten"
	Private Const MANDANT_XML_SETTING_SPUTNIK_FAK_SETTING As String = "MD_{0}/Fak-Daten"

#End Region


#Region "public property"

	Public Property IsDataValid As Boolean


#End Region


#Region "Constructor"


	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New()

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_SuppressUIEvents = True
		InitializeComponent()
		m_SuppressUIEvents = False

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_InitializationData = ClsDataDetail.m_InitialData ' _setting

		m_mandant = New Mandant

		m_Utility = New Utility
		m_UtilityUI = New UtilityUI
		m_ProgPath = New ClsProgPath

		If m_InitializationData Is Nothing Then Exit Sub

		m_Year = m_InitializationData.MDData.MDYear

		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

		m_MandantSetting = String.Format(MANDANT_XML_MAIN_KEY, m_InitializationData.MDData.MDNr)
		IsDataValid = True

		Try
			m_SonstigesSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_SONSTIGES_SETTING, m_InitializationData.MDData.MDNr)
			m_StartnumberSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_STARTNUMBER_SETTING, m_InitializationData.MDData.MDNr)
			m_SuvaSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_SUVA_SETTING, m_InitializationData.MDData.MDNr)
			m_AHVSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_AHV_SETTING, m_InitializationData.MDData.MDNr)
			m_FAKSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_FAK_SETTING, m_InitializationData.MDData.MDNr)

			m_MandantXMLFile = m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, m_Year)
			If Not System.IO.File.Exists(m_MandantXMLFile) Then
				m_UtilityUI.ShowErrorDialog(String.Format("Die Einstellung-Datei wurde nicht gefunden.{0}{1}", vbNewLine, m_MandantXMLFile))
				IsDataValid = False
			Else
				m_MandantSettingsXml = New SPProgUtility.CommonXmlUtility.SettingsXml(m_MandantXMLFile)
			End If

			connectionString = m_InitializationData.MDData.MDDbConn
			m_TablesettingDatabaseAccess = New TablesDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
			m_MandantDatabaseAccess = m_TablesettingDatabaseAccess.LoadMandantData(m_InitializationData.MDData.MDNr, m_Year)

			Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

		Catch ex As Exception
			IsDataValid = False

		End Try


		Reset()

		TranslateControls()

	End Sub



#End Region


	''' <summary>
	''' Inits the control with configuration information.
	''' </summary>
	'''<param name="initializationClass">The initialization class.</param>
	'''<param name="translationHelper">The translation helper.</param>
	Public Overridable Sub InitWithConfigurationData(ByVal initializationClass As InitializeClass, ByVal translationHelper As TranslateValuesHelper, _Year As Integer)

		m_InitializationData = initializationClass
		m_Translate = translationHelper
		m_Year = _Year
		IsDataValid = True

		Try
			connectionString = m_InitializationData.MDData.MDDbConn
			m_TablesettingDatabaseAccess = New TablesDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
			m_MandantDatabaseAccess = m_TablesettingDatabaseAccess.LoadMandantData(m_InitializationData.MDData.MDNr, m_Year)

			m_MandantSetting = String.Format(MANDANT_XML_MAIN_KEY, m_InitializationData.MDData.MDNr)
			m_MandantFormXMLFileName = m_mandant.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr)
			If Not System.IO.File.Exists(m_MandantFormXMLFileName) Then
				m_UtilityUI.ShowErrorDialog(String.Format("Die Einstellung-Datei wurde nicht gefunden.{0}{1}", vbNewLine, m_MandantFormXMLFileName))
				IsDataValid = False
			End If

			m_SonstigesSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_SONSTIGES_SETTING, m_InitializationData.MDData.MDNr)
			m_StartnumberSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_STARTNUMBER_SETTING, m_InitializationData.MDData.MDNr)
			m_SuvaSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_SUVA_SETTING, m_InitializationData.MDData.MDNr)
			m_AHVSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_AHV_SETTING, m_InitializationData.MDData.MDNr)
			m_FAKSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_FAK_SETTING, m_InitializationData.MDData.MDNr)

			m_MandantXMLFile = m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, m_Year)
			If Not System.IO.File.Exists(m_MandantXMLFile) Then
				m_UtilityUI.ShowErrorDialog(String.Format("Die Einstellung-Datei wurde nicht gefunden.{0}{1}", vbNewLine, m_MandantXMLFile))
				IsDataValid = False
			Else
				m_MandantSettingsXml = New SPProgUtility.CommonXmlUtility.SettingsXml(m_MandantXMLFile)
			End If

			Reset()

		Catch ex As Exception
			IsDataValid = False

		End Try

		TranslateControls()

	End Sub


	''' <summary>
	'''  Trannslate controls.
	''' </summary>
	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)


	End Sub

	Private Sub Reset()

		Dim suppressState = m_SuppressUIEvents
		m_SuppressUIEvents = True

		txt_MANr.EditValue = 0
		txt_KDNr.EditValue = 0

		txt_VakNr.EditValue = 0
		txt_proposeNr.EditValue = 0
		txt_OfferNr.EditValue = 0

		txt_ESNr.EditValue = 0
		txt_RPNr.EditValue = 0

		txt_ZGNr.EditValue = 0
		txt_LONr.EditValue = 0

		txt_RENr.EditValue = 0
		txt_ZENr.EditValue = 0
		txt_FopNr.EditValue = 0


		m_SuppressUIEvents = False

	End Sub


	Public Function LoadMandantenData() As Boolean
		Dim success As Boolean = True

		Try
			If (m_MandantDatabaseAccess Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mandantendaten konnten nicht geladen werden."))
				IsDataValid = False

				Return False
			End If
			Dim Mitarbeiter As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/Mitarbeiter", m_StartnumberSetting)), 0)
			Dim Kunden As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/Kunden", m_StartnumberSetting)), 0)
			Dim Vakanzennr As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/Vakanzennr", m_StartnumberSetting)), 0)
			Dim proposenumber As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/proposenumber", m_StartnumberSetting)), 0)
			Dim Offers As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/Offers", m_StartnumberSetting)), 0)

			Dim Einsatzverwaltung As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/Einsatzverwaltung", m_StartnumberSetting)), 0)
			Dim Rapporte As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/Rapporte", m_StartnumberSetting)), 0)

			Dim Vorschussverwaltung As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/Vorschussverwaltung", m_StartnumberSetting)), 0)
			Dim Lohnabrechnung As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/Lohnabrechnung", m_StartnumberSetting)), 0)

			Dim Fakturen As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/Fakturen", m_StartnumberSetting)), 0)
			Dim Zahlungseingaenge As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/Zahlungseingänge", m_StartnumberSetting)), 0)
			Dim FremdOP As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/FremdOP", m_StartnumberSetting)), 0)


			txt_MANr.EditValue = Mitarbeiter
			txt_KDNr.EditValue = Kunden
			txt_VakNr.EditValue = Vakanzennr
			txt_proposeNr.EditValue = proposenumber
			txt_OfferNr.EditValue = Offers


			txt_ESNr.EditValue = Einsatzverwaltung
			txt_RPNr.EditValue = Rapporte
			txt_ZGNr.EditValue = Vorschussverwaltung
			txt_LONr.EditValue = Lohnabrechnung

			txt_RENr.EditValue = Fakturen
			txt_ZENr.EditValue = Zahlungseingaenge
			txt_FopNr.EditValue = FremdOP


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			success = False

		End Try

		Return success

	End Function


	Public Function SaveMandantenData() As Boolean
		Dim success As Boolean = True
		If Not IsDataValid Then Return False

		Dim suppressUIEventState = m_SuppressUIEvents
		m_SuppressUIEvents = False

		Try
			If (m_MandantDatabaseAccess Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mandantendaten konnten nicht geladen werden."))
				IsDataValid = False

				Return False
			End If

			' Bewilligung
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/Mitarbeiter", m_StartnumberSetting), txt_MANr.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/Kunden", m_StartnumberSetting), txt_KDNr.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/Vakanzennr", m_StartnumberSetting), txt_VakNr.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/proposenumber", m_StartnumberSetting), txt_proposeNr.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/Offers", m_StartnumberSetting), txt_OfferNr.EditValue)

			' Betrieb und Versicherungen
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/Einsatzverwaltung", m_StartnumberSetting), txt_ESNr.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/Rapporte", m_StartnumberSetting), txt_RPNr.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/Vorschussverwaltung", m_StartnumberSetting), txt_ZGNr.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/Lohnabrechnung", m_StartnumberSetting), txt_LONr.EditValue)

			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/Fakturen", m_StartnumberSetting), txt_RENr.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/Zahlungseingänge", m_StartnumberSetting), txt_ZENr.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/FremdOP", m_StartnumberSetting), txt_FopNr.EditValue)


			m_SuppressUIEvents = suppressUIEventState


		Catch ex As Exception
      m_Logger.LogError(String.Format("ucMDStartNumbers: {0}", ex.ToString))
      success = False

    Finally

		End Try

		Return success

	End Function



#Region "Helpers"


	Private Function ParseToBoolean(ByVal stringvalue As String, ByVal value As Boolean?) As Boolean
		Dim result As Boolean
		If (Not Boolean.TryParse(stringvalue, result)) Then
			Return value
		End If
		Return result
	End Function

	Private Function ParseToInteger(ByVal stringvalue As String, ByVal value As Integer?) As Integer
		Dim result As Integer
		If (Not Integer.TryParse(stringvalue, result)) Then
			Return value
		End If
		Return result
	End Function

	Private Function ParseToDec(ByVal stringvalue As String, ByVal value As Decimal?) As Decimal
		Dim result As Decimal
		If (Not Decimal.TryParse(stringvalue, result)) Then
			Return value
		End If
		Return result
	End Function

	Private Function StrToBool(ByVal str As String) As Boolean

		Dim result As Boolean = False

		If String.IsNullOrWhiteSpace(str) Then
			Return False
		End If

		Boolean.TryParse(str, result)

		Return result
	End Function


#End Region


End Class



