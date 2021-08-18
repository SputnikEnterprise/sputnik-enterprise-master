
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


Public Class ucMDFibuKonten

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
	Private m_FibuSetting As String

	Private m_MandantSettingsXml As SPProgUtility.CommonXmlUtility.SettingsXml

	Private m_ProgPath As ClsProgPath

	Private m_Year As Integer

#End Region


#Region "private consts"

	Private Const MANDANT_XML_MAIN_KEY As String = "MD_{0}/"
	Private Const MANDANT_XML_SETTING_SPUTNIK_FIBU_SETTING As String = "MD_{0}/BuchungsKonten"

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

		IsDataValid = True
		m_mandant = New Mandant
		m_Utility = New Utility
		m_UtilityUI = New UtilityUI
		m_ProgPath = New ClsProgPath

		If m_InitializationData Is Nothing Then Exit Sub

		m_Year = m_InitializationData.MDData.MDYear

		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

		m_MandantSetting = String.Format(MANDANT_XML_MAIN_KEY, m_InitializationData.MDData.MDNr)

		Try
			m_FibuSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_FIBU_SETTING, m_InitializationData.MDData.MDNr)

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

			m_FibuSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_FIBU_SETTING, m_InitializationData.MDData.MDNr)

			m_MandantXMLFile = m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, m_Year)
			If Not System.IO.File.Exists(m_MandantXMLFile) Then
				m_UtilityUI.ShowErrorDialog(String.Format("Die Einstellung-Datei wurde nicht gefunden.{0}{1}", vbNewLine, m_MandantXMLFile))
				IsDataValid = False
			Else
				m_MandantSettingsXml = New SPProgUtility.CommonXmlUtility.SettingsXml(m_MandantXMLFile)
			End If

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

		' Automatische Debitoren
		txt_ADebitoren.EditValue = 0
		txt_Aerloes.EditValue = 0
		txt_A0erloes.EditValue = 0
		txt_ASKonto.EditValue = 0
		txt_A0SKonto.EditValue = 0
		txt_AVerlust.EditValue = 0
		txt_A0Verlust.EditValue = 0
		txt_ARueck.EditValue = 0
		txt_A0Rueck.EditValue = 0
		txt_AGutschrift.EditValue = 0
		txt_A0Gutschrift.EditValue = 0

		' Individuelle Rechnungen
		txt_IDebitoren.EditValue = 0
		txt_Ierloes.EditValue = 0
		txt_I0erloes.EditValue = 0
		txt_ISKonto.EditValue = 0
		txt_I0SKonto.EditValue = 0
		txt_IVerlust.EditValue = 0
		txt_I0Verlust.EditValue = 0
		txt_IRueck.EditValue = 0
		txt_I0Rueck.EditValue = 0
		txt_IGutschrift.EditValue = 0
		txt_I0Gutschrift.EditValue = 0

		' Festanstellungen
		txt_FDebitoren.EditValue = 0
		txt_Ferloes.EditValue = 0
		txt_F0erloes.EditValue = 0
		txt_FSKonto.EditValue = 0
		txt_F0SKonto.EditValue = 0
		txt_FVerlust.EditValue = 0
		txt_F0Verlust.EditValue = 0
		txt_FRueck.EditValue = 0
		txt_F0Rueck.EditValue = 0
		txt_FGutschrift.EditValue = 0
		txt_F0Gutschrift.EditValue = 0


		' Allgemeines
		txt_BankESR.EditValue = 0
		txt_MwSt.EditValue = 0
		txt_Mahnspesen.EditValue = 0
		txt_Verzug.EditValue = 0

		txt_DB1Value.EditValue = String.Empty


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

			Dim _1 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_1", m_FibuSetting)), 0)
			Dim _2 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_2", m_FibuSetting)), 0)
			Dim _4 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_4", m_FibuSetting)), 0)
			Dim _10 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_10", m_FibuSetting)), 0)
			Dim _12 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_12", m_FibuSetting)), 0)
			Dim _21 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_21", m_FibuSetting)), 0)
			Dim _22 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_22", m_FibuSetting)), 0)
			Dim _23 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_23", m_FibuSetting)), 0)
			Dim _24 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_24", m_FibuSetting)), 0)
			Dim _33 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_33", m_FibuSetting)), 0)
			Dim _34 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_34", m_FibuSetting)), 0)

			' Automatische Debitoren
			txt_ADebitoren.EditValue = _1
			txt_Aerloes.EditValue = _2
			txt_A0erloes.EditValue = _4
			txt_ASKonto.EditValue = _10
			txt_A0SKonto.EditValue = _12
			txt_AVerlust.EditValue = _21
			txt_A0Verlust.EditValue = _22
			txt_ARueck.EditValue = _23
			txt_A0Rueck.EditValue = _24
			txt_AGutschrift.EditValue = _33
			txt_A0Gutschrift.EditValue = _34

			success = success AndAlso LoadFibuIndividuellData()
			success = success AndAlso LoadFibuFestanstellungData()
			success = success AndAlso LoadFibuAllgemeinData()


		Catch ex As Exception
			success = False

		End Try

		Return success

	End Function

	Private Function LoadFibuIndividuellData() As Boolean
		Dim success As Boolean = True

		Try
			Dim _15 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_15", m_FibuSetting)), 0)
			Dim _3 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_3", m_FibuSetting)), 0)
			Dim _5 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_5", m_FibuSetting)), 0)
			Dim _11 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_11", m_FibuSetting)), 0)
			Dim _13 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_13", m_FibuSetting)), 0)
			Dim _25 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_25", m_FibuSetting)), 0)
			Dim _26 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_26", m_FibuSetting)), 0)
			Dim _27 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_27", m_FibuSetting)), 0)
			Dim _28 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_28", m_FibuSetting)), 0)
			Dim _35 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_35", m_FibuSetting)), 0)
			Dim _36 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_36", m_FibuSetting)), 0)

			' Individuelle Rechnungen
			txt_IDebitoren.EditValue = _15
			txt_Ierloes.EditValue = _3
			txt_I0erloes.EditValue = _5
			txt_ISKonto.EditValue = _11
			txt_I0SKonto.EditValue = _13
			txt_IVerlust.EditValue = _25
			txt_I0Verlust.EditValue = _26
			txt_IRueck.EditValue = _27
			txt_I0Rueck.EditValue = _28
			txt_IGutschrift.EditValue = _35
			txt_I0Gutschrift.EditValue = _36


		Catch ex As Exception
			success = False

		Finally

		End Try

		Return success

	End Function

	Private Function LoadFibuFestanstellungData() As Boolean
		Dim success As Boolean = True

		Try
			Dim _16 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_16", m_FibuSetting)), 0)
			Dim _17 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_17", m_FibuSetting)), 0)
			Dim _18 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_18", m_FibuSetting)), 0)
			Dim _19 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_19", m_FibuSetting)), 0)
			Dim _20 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_20", m_FibuSetting)), 0)
			Dim _29 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_29", m_FibuSetting)), 0)
			Dim _30 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_30", m_FibuSetting)), 0)
			Dim _31 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_31", m_FibuSetting)), 0)
			Dim _32 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_32", m_FibuSetting)), 0)
			Dim _37 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_37", m_FibuSetting)), 0)
			Dim _38 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_38", m_FibuSetting)), 0)

			' Festanstellungen
			txt_FDebitoren.EditValue = _16
			txt_Ferloes.EditValue = _17
			txt_F0erloes.EditValue = _18
			txt_FSKonto.EditValue = _19
			txt_F0SKonto.EditValue = _20
			txt_FVerlust.EditValue = _29
			txt_F0Verlust.EditValue = _30
			txt_FRueck.EditValue = _31
			txt_F0Rueck.EditValue = _32
			txt_FGutschrift.EditValue = _37
			txt_F0Gutschrift.EditValue = _38


		Catch ex As Exception
			success = False

		Finally

		End Try

		Return success

	End Function

	Private Function LoadFibuAllgemeinData() As Boolean
		Dim success As Boolean = True

		Try
			Dim _7 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_7", m_FibuSetting)), 0)
			Dim _6 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_6", m_FibuSetting)), 0)
			Dim _8 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_8", m_FibuSetting)), 0)
			Dim _9 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_9", m_FibuSetting)), 0)
			Dim _14 As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_14", m_FibuSetting))

			' Allgemeines
			txt_BankESR.EditValue = _7
			txt_MwSt.EditValue = _6
			txt_Mahnspesen.EditValue = _8
			txt_Verzug.EditValue = _9
			txt_DB1Value.EditValue = _14


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

			' Automatische Debitoren
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/_1", m_FibuSetting), txt_ADebitoren.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/_2", m_FibuSetting), txt_Aerloes.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/_4", m_FibuSetting), txt_A0erloes.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/_10", m_FibuSetting), txt_ASKonto.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/_12", m_FibuSetting), txt_A0SKonto.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/_21", m_FibuSetting), txt_AVerlust.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/_22", m_FibuSetting), txt_A0Verlust.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/_23", m_FibuSetting), txt_ARueck.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/_24", m_FibuSetting), txt_A0Rueck.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/_33", m_FibuSetting), txt_AGutschrift.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/_34", m_FibuSetting), txt_A0Gutschrift.EditValue)

			success = success AndAlso SaveIndividuellData()
			success = success AndAlso SaveFestanstellungData()
			success = success AndAlso SaveAllgemeinData()


			m_SuppressUIEvents = suppressUIEventState


		Catch ex As Exception
      m_Logger.LogError(String.Format("ucMDFibuKonten: {0}", ex.ToString))
      success = False

    Finally

		End Try

		Return success

	End Function

	Private Function SaveIndividuellData() As Boolean
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

			' Individuelle Debitoren
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/_15", m_FibuSetting), txt_IDebitoren.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/_3", m_FibuSetting), txt_Ierloes.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/_5", m_FibuSetting), txt_I0erloes.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/_11", m_FibuSetting), txt_ISKonto.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/_13", m_FibuSetting), txt_I0SKonto.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/_25", m_FibuSetting), txt_IVerlust.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/_26", m_FibuSetting), txt_I0Verlust.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/_27", m_FibuSetting), txt_IRueck.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/_28", m_FibuSetting), txt_I0Rueck.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/_35", m_FibuSetting), txt_IGutschrift.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/_36", m_FibuSetting), txt_I0Gutschrift.EditValue)


			m_SuppressUIEvents = suppressUIEventState


		Catch ex As Exception
      m_Logger.LogError(String.Format("ucMDFibuKonten: {0}", ex.ToString))
      success = False

    Finally

		End Try

		Return success

	End Function

	Private Function SaveFestanstellungData() As Boolean
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

			' Festanstellungen 
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/_16", m_FibuSetting), txt_FDebitoren.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/_17", m_FibuSetting), txt_Ferloes.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/_18", m_FibuSetting), txt_F0erloes.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/_19", m_FibuSetting), txt_FSKonto.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/_20", m_FibuSetting), txt_F0SKonto.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/_29", m_FibuSetting), txt_FVerlust.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/_30", m_FibuSetting), txt_F0Verlust.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/_31", m_FibuSetting), txt_FRueck.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/_32", m_FibuSetting), txt_F0Rueck.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/_37", m_FibuSetting), txt_FGutschrift.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/_38", m_FibuSetting), txt_F0Gutschrift.EditValue)


			m_SuppressUIEvents = suppressUIEventState


		Catch ex As Exception
      m_Logger.LogError(String.Format("ucMDFibuKonten: {0}", ex.ToString))
      success = False

    Finally

		End Try

		Return success

	End Function

	Private Function SaveAllgemeinData() As Boolean
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

			' Allgemeines
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/_7", m_FibuSetting), txt_BankESR.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/_6", m_FibuSetting), txt_MwSt.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/_8", m_FibuSetting), txt_Mahnspesen.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/_9", m_FibuSetting), txt_Verzug.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/_14", m_FibuSetting), txt_DB1Value.EditValue)


			m_SuppressUIEvents = suppressUIEventState


		Catch ex As Exception
      m_Logger.LogError(String.Format("ucMDFibuKonten: {0}", ex.ToString))
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




