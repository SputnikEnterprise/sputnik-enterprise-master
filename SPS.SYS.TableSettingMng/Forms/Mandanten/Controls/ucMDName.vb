

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


Public Class ucMDName

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

	Private m_MandantSettingsXml As SPProgUtility.CommonXmlUtility.SettingsXml

	Private m_ProgPath As ClsProgPath

	Private m_Year As Integer

#End Region


#Region "private consts"

	Private Const ENCRYPTION_KEY As String = "your crypt key"
	Private Const EXTRA_STRING_KEY As String = "yourseckey"

	Private Const MANDANT_XML_MAIN_KEY As String = "MD_{0}/"
	Private Const MANDANT_XML_SETTING_SPUTNIK_SONSTIGES_SETTING As String = "MD_{0}/Sonstiges"

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
		txt_Color.Properties.ShowSystemColors = False
		txt_Color.Properties.ShowWebColors = False

		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

		m_MandantSetting = String.Format(MANDANT_XML_MAIN_KEY, m_InitializationData.MDData.MDNr)
		IsDataValid = True

		Try
			m_SonstigesSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_SONSTIGES_SETTING, m_InitializationData.MDData.MDNr)
			m_MandantXMLFile = m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, m_Year)
			If Not System.IO.File.Exists(m_MandantXMLFile) Then
				m_UtilityUI.ShowErrorDialog(String.Format("Die Einstellung-Datei wurde nicht gefunden.{0}{1}", vbNewLine, m_MandantXMLFile))
				IsDataValid = False
			Else
				m_Logger.LogDebug(String.Format("{0}-{1}: {2}", m_InitializationData.MDData.MDNr, m_Year, m_MandantXMLFile))
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

			m_SonstigesSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_SONSTIGES_SETTING, m_InitializationData.MDData.MDNr)

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

		' Allgemein
		txt_MDName1.EditValue = String.Empty
		txt_MDName2.EditValue = String.Empty
		txt_MDName3.EditValue = String.Empty
		txt_Postfach.EditValue = String.Empty
		txt_Strasse.EditValue = String.Empty
		txt_Land.EditValue = String.Empty
		txt_PLZ.EditValue = String.Empty
		txt_Ort.EditValue = String.Empty
		txt_Kanton.EditValue = String.Empty

		' Kommunikation
		txt_Telefon.EditValue = String.Empty
		txt_Telefax.EditValue = String.Empty
		txt_Homepage.EditValue = String.Empty
		txt_eMail.EditValue = String.Empty

		' Eigenschaften
		txt_Bezeichnung.EditValue = String.Empty
		txt_Customer_ID.EditValue = String.Empty
		txt_MDFullfilename.EditValue = String.Empty
		txt_Jahr.EditValue = String.Empty
		txt_Color.EditValue = String.Empty

		' Passwort
		txt_Passwort4.EditValue = String.Empty


		m_SuppressUIEvents = False

	End Sub


	Public Function LoadMandantenData() As Boolean

		Try
			If (m_MandantDatabaseAccess Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mandantendaten konnten nicht geladen werden."))
				Return False
			End If

			' allgemein
			txt_MDName1.EditValue = m_MandantDatabaseAccess.MD_Name1
			txt_MDName2.EditValue = m_MandantDatabaseAccess.MD_Name2
			txt_MDName3.EditValue = m_MandantDatabaseAccess.MD_Name3

			txt_Postfach.EditValue = m_MandantDatabaseAccess.Postfach

			txt_Strasse.EditValue = m_MandantDatabaseAccess.Strasse
			txt_Land.EditValue = m_MandantDatabaseAccess.Land
			txt_PLZ.EditValue = m_MandantDatabaseAccess.PLZ

			txt_Ort.EditValue = m_MandantDatabaseAccess.Ort
			txt_Kanton.EditValue = m_MandantDatabaseAccess.MD_Kanton

			' Kommunikation
			txt_Telefon.EditValue = m_MandantDatabaseAccess.Telefon
			txt_Telefax.EditValue = m_MandantDatabaseAccess.Telefax
			txt_Homepage.EditValue = m_MandantDatabaseAccess.Homepage
			txt_eMail.EditValue = m_MandantDatabaseAccess.eMail

			' Eigenschaften
			txt_Bezeichnung.EditValue = m_MandantDatabaseAccess.MDAuflistung
			txt_Customer_ID.EditValue = m_MandantDatabaseAccess.Customer_ID
			txt_MDFullfilename.EditValue = m_MandantDatabaseAccess.MDFullFileName
			txt_Jahr.EditValue = m_MandantDatabaseAccess.Jahr

			Dim mandantcolor As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/mandantcolor", m_SonstigesSetting)), 0D)
			Dim converted As Color = Color.FromArgb(mandantcolor)
			txt_Color.Color = converted


			' Passwort
			Dim encryptedPasswort = m_MandantDatabaseAccess.Passwort4
			Dim decryptedPasswort = DecryptWithClipper(encryptedPasswort, ENCRYPTION_KEY)

			txt_Passwort4.EditValue = decryptedPasswort.Replace(EXTRA_STRING_KEY, String.Empty)


			Return Not m_MandantDatabaseAccess Is Nothing

		Catch ex As Exception

		Finally

		End Try


	End Function

	Public Function SaveMandantenData() As Boolean
		Dim success As Boolean = True
		If Not IsDataValid Then Return False

		Dim suppressUIEventState = m_SuppressUIEvents
		m_SuppressUIEvents = False

		Try
			If (m_MandantDatabaseAccess Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mandantendaten konnten nicht geladen werden."))
				Return False
			End If

			' allgemein
			m_MandantDatabaseAccess.MD_Name1 = txt_MDName1.EditValue
			m_MandantDatabaseAccess.MD_Name2 = txt_MDName2.EditValue
			m_MandantDatabaseAccess.MD_Name3 = txt_MDName3.EditValue
			m_MandantDatabaseAccess.Postfach = txt_Postfach.EditValue

			m_MandantDatabaseAccess.Strasse = txt_Strasse.EditValue
			m_MandantDatabaseAccess.Land = txt_Land.EditValue
			m_MandantDatabaseAccess.PLZ = txt_PLZ.EditValue
			m_MandantDatabaseAccess.Ort = txt_Ort.EditValue
			m_MandantDatabaseAccess.MD_Kanton = txt_Kanton.EditValue

			' Kommunikation
			m_MandantDatabaseAccess.Telefon = txt_Telefon.EditValue
			m_MandantDatabaseAccess.Telefax = txt_Telefax.EditValue
			m_MandantDatabaseAccess.Homepage = txt_Homepage.EditValue
			m_MandantDatabaseAccess.eMail = txt_eMail.EditValue


			' Eigenschaften
			m_MandantDatabaseAccess.MDAuflistung = txt_Bezeichnung.EditValue
			m_MandantDatabaseAccess.Customer_ID = txt_Customer_ID.EditValue
			m_MandantDatabaseAccess.MDFullFileName = txt_MDFullfilename.EditValue

			m_MandantDatabaseAccess.Jahr = txt_Jahr.EditValue
			m_MandantDatabaseAccess.MDFullFileName = txt_MDFullfilename.EditValue


			' Passwort
			Dim decryptedPasswort = txt_Passwort4.EditValue
			Dim encryptedPasswort = String.Empty
			If Not String.IsNullOrWhiteSpace(decryptedPasswort) Then
				encryptedPasswort = EncryptMyString(decryptedPasswort & EXTRA_STRING_KEY, ENCRYPTION_KEY)
			End If

			m_MandantDatabaseAccess.Passwort4 = encryptedPasswort


			success = m_TablesettingDatabaseAccess.SaveMandantData(m_InitializationData.MDData.MDNr, m_Year, m_MandantDatabaseAccess)

			If success Then
				Dim colorvalue As System.Drawing.Color = CType(txt_Color.EditValue, Color)
				Dim colorConvert As Int32 = colorvalue.ToArgb()

				m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/mandantcolor", m_SonstigesSetting), colorConvert)

			End If


			m_SuppressUIEvents = suppressUIEventState


		Catch ex As Exception
			m_Logger.LogError(String.Format("ucMDName: {0}", ex.ToString))
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

	Function EncryptMyString(ByVal strData As String, ByVal strCryptKey As String) As String
		Dim value As String = ""

		' put hier your code for Decrypt / decrypting
		Return value
	End Function


#End Region


End Class



