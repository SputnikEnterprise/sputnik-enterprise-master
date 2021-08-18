
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


Public Class ucMDPublicAuthorities

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
		xtabAuthorities.SelectedTabPage = xtabAusgleichskasse

		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

		m_MandantSetting = String.Format(MANDANT_XML_MAIN_KEY, m_InitializationData.MDData.MDNr)
		IsDataValid = True

		Try
			m_SonstigesSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_SONSTIGES_SETTING, m_InitializationData.MDData.MDNr)
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
			xtabAuthorities.SelectedTabPage = xtabAusgleichskasse

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

		' Bewilligung
		txt_BewName.EditValue = String.Empty
		txt_BewPostfach.EditValue = String.Empty
		txt_BewStrasse.EditValue = String.Empty
		txt_BewPLZOrt.EditValue = String.Empty
		txt_BewSeco.EditValue = String.Empty

		' Betrieb und Versicherungen
		txt_BURNr.EditValue = String.Empty
		txt_UIDNr.EditValue = String.Empty
		txt_BVGName.EditValue = String.Empty
		txt_BVGAGNr.EditValue = String.Empty
		txt_FARNr.EditValue = String.Empty

		' Ausgleichskasse
		txt_AZusatz.EditValue = String.Empty
		txt_AZHD.EditValue = String.Empty
		txt_APostfach.EditValue = String.Empty
		txt_AStrasse.EditValue = String.Empty
		txt_APLZOrt.EditValue = String.Empty

		txt_ANr.EditValue = String.Empty
		txt_AKassennr.EditValue = String.Empty
		txt_ASubNr1.EditValue = String.Empty

		' FAK
		txt_FZusatz.EditValue = String.Empty
		txt_FZHD.EditValue = String.Empty
		txt_FPostfach.EditValue = String.Empty
		txt_FStrasse.EditValue = String.Empty
		txt_FPLZOrt.EditValue = String.Empty

		txt_FNr.EditValue = String.Empty
		txt_FKassennr.EditValue = String.Empty
		txt_FSubNr1.EditValue = String.Empty

		' Unfallversicherung
		txt_UZusatz.EditValue = String.Empty
		txt_UZHD.EditValue = String.Empty
		txt_UPostfach.EditValue = String.Empty
		txt_UStrasse.EditValue = String.Empty
		txt_UPLZOrt.EditValue = String.Empty

		txt_UNr.EditValue = String.Empty
		txt_USubNr1.EditValue = String.Empty


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
			Dim BewName As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/BewName", m_SonstigesSetting))
			Dim BewPostfach As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/BewPostfach", m_SonstigesSetting))
			Dim BewStrasse As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/BewStrasse", m_SonstigesSetting))
			Dim BewPLZOrt As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/BewPLZOrt", m_SonstigesSetting))
			Dim BewSeco As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/BewSeco", m_SonstigesSetting))

			Dim BURNumber As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/BURNumber", m_SonstigesSetting))
			Dim UIDNumber As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/UIDNumber", m_SonstigesSetting))
			Dim bvgname As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/bvgname", m_SonstigesSetting))
			Dim bvgagnr As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/bvgagnr", m_SonstigesSetting))
			Dim farNr As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/FAR.-MitgliedNr", m_SonstigesSetting))

			' Bewilligung
			txt_BewName.EditValue = BewName
			txt_BewPostfach.EditValue = BewPostfach
			txt_BewStrasse.EditValue = BewStrasse
			txt_BewPLZOrt.EditValue = BewPLZOrt
			txt_BewSeco.EditValue = BewSeco


			' Betrieb und Versicherungen
			txt_BURNr.EditValue = BURNumber
			txt_UIDNr.EditValue = UIDNumber
			txt_BVGName.EditValue = bvgname
			txt_BVGAGNr.EditValue = bvgagnr
			txt_FARNr.EditValue = farNr

			success = success AndAlso LoadMandantenAusgleichskasseData()
			success = success AndAlso LoadMandantenFAKData()
			success = success AndAlso LoadMandantenUnfallData()

			Dim progSetting As New SPProgUtility.ClsDivReg
			Dim iniFullName As String = IO.Path.Combine(m_InitializationData.MDData.MDMainPath, m_Year, "Programm.Dat")
			progSetting.SetINIString(iniFullName, "Sonstiges", "BVGVersicherung", txt_BVGName.EditValue)
			progSetting.SetINIString(iniFullName, "Sonstiges", "BURNumber", txt_BURNr.EditValue)
			progSetting.SetINIString(iniFullName, "Sonstiges", "AHV-Ausgleichskasse", txt_AZusatz.EditValue)
			progSetting.SetINIString(iniFullName, "Sonstiges", "AusgNummer", txt_AKassennr.EditValue)


		Catch ex As Exception
			success = False

		End Try

		Return success

	End Function

	Private Function LoadMandantenAusgleichskasseData() As Boolean
		Dim success As Boolean = True

		Try
			Dim AHVAddressZusatz As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/AHVAddressZusatz", m_AHVSetting))
			Dim AHVAddressZHD As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/AHVAddressZHD", m_AHVSetting))
			Dim AHVAddressPostfach As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/AHVAddressPostfach", m_AHVSetting))
			Dim AHVAddressStrasse As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/AHVAddressStrasse", m_AHVSetting))
			Dim AHVAddressPLZOrt As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/AHVAddressPLZOrt", m_AHVSetting))

			Dim AHVMitgliedNr As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/AHVMitgliedNr", m_AHVSetting))
			Dim AusgNummer As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/AusgNummer", m_AHVSetting))
			Dim AHVSub1 As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/AHVSub1", m_AHVSetting))

			' Ausgleichskasse
			txt_AZusatz.EditValue = AHVAddressZusatz
			txt_AZHD.EditValue = AHVAddressZHD
			txt_APostfach.EditValue = AHVAddressPostfach
			txt_AStrasse.EditValue = AHVAddressStrasse
			txt_APLZOrt.EditValue = AHVAddressPLZOrt

			txt_ANr.EditValue = AHVMitgliedNr
			txt_AKassennr.EditValue = AusgNummer
			txt_ASubNr1.EditValue = AHVSub1


		Catch ex As Exception
			success = False

		Finally

		End Try

		Return success

	End Function

	Private Function LoadMandantenFAKData() As Boolean
		Dim success As Boolean = True

		Try
			Dim FAKAddressZusatz As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/FAKAddressZusatz", m_FAKSetting))
			Dim FAKAddressZHD As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/FAKAddressZHD", m_FAKSetting))
			Dim FAKAddressPostfach As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/FAKAddressPostfach", m_FAKSetting))
			Dim FAKAddressStrasse As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/FAKAddressStrasse", m_FAKSetting))
			Dim FAKAddressPLZOrt As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/FAKAddressPLZOrt", m_FAKSetting))

			Dim FAKAddressMitgliednr As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/FAKAddressMitgliednr", m_FAKSetting))
			Dim FAKAddressNr As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/FAKAddressNr", m_FAKSetting))
			Dim FAKAddressSub1 As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/FAKAddressSub1", m_FAKSetting))

			' FAK
			txt_FZusatz.EditValue = FAKAddressZusatz
			txt_FZHD.EditValue = FAKAddressZHD
			txt_FPostfach.EditValue = FAKAddressPostfach
			txt_FStrasse.EditValue = FAKAddressStrasse
			txt_FPLZOrt.EditValue = FAKAddressPLZOrt

			txt_FNr.EditValue = FAKAddressMitgliednr
			txt_FKassennr.EditValue = FAKAddressNr
			txt_FSubNr1.EditValue = FAKAddressSub1


		Catch ex As Exception
			success = False

		Finally

		End Try

		Return success

	End Function

	Private Function LoadMandantenUnfallData() As Boolean
		Dim success As Boolean = True

		Try
			Dim SUVAAddressZusatz As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/SUVAAddressZusatz", m_SuvaSetting))
			Dim SUVAAddressZHD As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/SUVAAddressZHD", m_SuvaSetting))
			Dim SUVAAddressPostfach As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/SUVAAddressPostfach", m_SuvaSetting))
			Dim SUVAAddressStrasse As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/SUVAAddressStrasse", m_SuvaSetting))
			Dim SUVAAddressPLZOrt As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/SUVAAddressPLZOrt", m_SuvaSetting))

			Dim Abrechnungsnummer As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/Abrechnungsnummer", m_SuvaSetting))
			Dim SUVAAddressSub1 As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/SUVAAddressSub1", m_SuvaSetting))

			' Unfallversicherung
			txt_UZusatz.EditValue = SUVAAddressZusatz
			txt_UZHD.EditValue = SUVAAddressZHD
			txt_UPostfach.EditValue = SUVAAddressPostfach
			txt_UStrasse.EditValue = SUVAAddressStrasse
			txt_UPLZOrt.EditValue = SUVAAddressPLZOrt

			txt_UNr.EditValue = Abrechnungsnummer
			txt_USubNr1.EditValue = SUVAAddressSub1


		Catch ex As Exception
			success = False

		Finally

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
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/BewName", m_SonstigesSetting), txt_BewName.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/BewPostfach", m_SonstigesSetting), txt_BewPostfach.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/BewStrasse", m_SonstigesSetting), txt_BewStrasse.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/BewPLZOrt", m_SonstigesSetting), txt_BewPLZOrt.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/BewSeco", m_SonstigesSetting), txt_BewSeco.EditValue)

			' Betrieb und Versicherungen
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/BURNumber", m_SonstigesSetting), txt_BURNr.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/UIDNumber", m_SonstigesSetting), txt_UIDNr.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/bvgname", m_SonstigesSetting), txt_BVGName.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/bvgagnr", m_SonstigesSetting), txt_BVGAGNr.EditValue)

			Dim progSetting As New SPProgUtility.ClsDivReg
			Dim iniFullName As String = IO.Path.Combine(m_InitializationData.MDData.MDMainPath, m_Year, "Programm.Dat")
			progSetting.SetINIString(iniFullName, "Sonstiges", "BVGVersicherung", txt_BVGName.EditValue)
			progSetting.SetINIString(iniFullName, "Sonstiges", "BURNumber", txt_BURNr.EditValue)
			progSetting.SetINIString(iniFullName, "Sonstiges", "AHV-Ausgleichskasse", txt_AZusatz.EditValue)
			progSetting.SetINIString(iniFullName, "Sonstiges", "AusgNummer", txt_AKassennr.EditValue)

			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/FAR.-MitgliedNr", m_SonstigesSetting), txt_FARNr.EditValue)

			success = success AndAlso saveAusgleichskasseData
			success = success AndAlso saveFAKData
			success = success AndAlso saveUnfallData


			m_SuppressUIEvents = suppressUIEventState


		Catch ex As Exception
      m_Logger.LogError(String.Format("ucMDPublicAuthorities: {0}", ex.ToString))
      success = False

    Finally

		End Try

		Return success

	End Function

	Private Function SaveAusgleichskasseData() As Boolean
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

			' Ausgleichskasse
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/AHVAddressZusatz", m_AHVSetting), txt_AZusatz.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/AHVAddressZHD", m_AHVSetting), txt_AZHD.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/AHVAddressPostfach", m_AHVSetting), txt_APostfach.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/AHVAddressStrasse", m_AHVSetting), txt_AStrasse.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/AHVAddressPLZOrt", m_AHVSetting), txt_APLZOrt.EditValue)

			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/AHVMitgliedNr", m_AHVSetting), txt_ANr.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/AusgNummer", m_AHVSetting), txt_AKassennr.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/AHVSub1", m_AHVSetting), txt_ASubNr1.EditValue)


			m_SuppressUIEvents = suppressUIEventState


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			success = False

		Finally

		End Try

		Return success

	End Function

	Private Function SaveFAKData() As Boolean
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

			' Ausgleichskasse
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/FAKAddressZusatz", m_FAKSetting), txt_FZusatz.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/FAKAddressZHD", m_FAKSetting), txt_FZHD.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/FAKAddressPostfach", m_FAKSetting), txt_FPostfach.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/FAKAddressStrasse", m_FAKSetting), txt_FStrasse.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/FAKAddressPLZOrt", m_FAKSetting), txt_FPLZOrt.EditValue)

			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/FAKAddressMitgliednr", m_FAKSetting), txt_FNr.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/FAKAddressNr", m_FAKSetting), txt_FKassennr.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/FAKAddressSub1", m_FAKSetting), txt_FSubNr1.EditValue)


			m_SuppressUIEvents = suppressUIEventState


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			success = False

		Finally

		End Try

		Return success

	End Function

	Private Function SaveUnfallData() As Boolean
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

			' Ausgleichskasse
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/SUVAAddressZusatz", m_SuvaSetting), txt_UZusatz.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/SUVAAddressZHD", m_SuvaSetting), txt_UZHD.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/SUVAAddressPostfach", m_SuvaSetting), txt_UPostfach.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/SUVAAddressStrasse", m_SuvaSetting), txt_UStrasse.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/SUVAAddressPLZOrt", m_SuvaSetting), txt_UPLZOrt.EditValue)

			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/Abrechnungsnummer", m_SuvaSetting), txt_UNr.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/SUVAAddressSub1", m_SuvaSetting), txt_USubNr1.EditValue)


			m_SuppressUIEvents = suppressUIEventState


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
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



