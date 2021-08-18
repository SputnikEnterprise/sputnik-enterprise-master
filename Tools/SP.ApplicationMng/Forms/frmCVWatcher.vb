
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.ColorUtility.ClsColorUtility
Imports SPProgUtility.SPUserSec.ClsUserSec

Imports SPProgUtility.MainUtilities
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports DevExpress.LookAndFeel
Imports SP.Infrastructure.UI
Imports SPProgUtility
Imports SP.Infrastructure
Imports TrxmlUtility
Imports SP.Infrastructure.Logging
Imports System.Threading.Tasks
Imports System.Threading
Imports System.IO
Imports SPProgUtility.CommonXmlUtility

Namespace UI


	Public Class frmCVWatcher


#Region "private consts"

		Private Const webServiceCVLUri As String = "http://cvlizer.joinvision.com:80/cvlizer/exservicesoap"
		Private Const CVL_WEB_REQUEST_URL As String = "https://cvlizer.joinvision.com/cvlizer/rest/v1/extract/xml/"

		Private Const JSON_AUTHENTICATION_TOKEN As String = "JSON_AUTHENTICATION_TOKEN"

		Private Const TEMPORARY_FOLDER_ORIGINAL_PDF As String = "ORIGINAL_PDF"
		Private Const TEMPORARY_FOLDER_SPLITTED_PDFS As String = "SPLITTED_PDFS"
		Private Const RUNTIME_COMMON_CONFIG_FOLDER As String = "Config"
		Private Const PROGRAM_SETTING_FILE As String = "NotifyerSettings.xml"
		Private Const PROGRAM_XML_SETTING_PATH As String = "Settings/Path"
		Private Const PROGRAM_XML_SETTING_DBCONNECTIONS As String = "Settings/DBConnection"

#End Region


#Region "private fields"

		''' <summary>
		''' The Initialization data.
		''' </summary>
		Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		''' <summary>
		''' The translation value helper.
		''' </summary>
		Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI

		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As SP.Infrastructure.Utility

		Private m_mandant As Mandant
		Private m_path As SPProgUtility.ProgPath.ClsProgPath

		Private ucCVWatcher As ucCVEMailWatcher
		'Private ucReportWatcher As ucReportEMailWatcher

		''' <summary>
		''' Settings xml.
		''' </summary>
		Private m_MandantSettingsXml As SettingsXml
		Private m_ProgSettingsXml As SettingsXml

		''' <summary>
		''' connection string
		''' </summary>
		Private m_connStr_Application As String
		Private m_connStr_CVlizer As String
		Private m_connStr_Systeminfo As String
		Private m_connStr_Scanjobs As String
		Private m_connStr_Email As String

		Private m_CommonConfigFolder As String
		Private m_SettingFile As ProgramSettings


#End Region


#Region "Constructor"

		Public Sub New(ByVal settingFile As ProgramSettings)

			' Dieser Aufruf ist für den Designer erforderlich.
			DevExpress.UserSkins.BonusSkins.Register()
			DevExpress.Skins.SkinManager.EnableFormSkins()

			m_CommonConfigFolder = Path.Combine(Environment.CurrentDirectory, RUNTIME_COMMON_CONFIG_FOLDER)
			m_SettingFile = settingFile

			m_connStr_Application = m_SettingFile.ConnstringApplication
			m_connStr_CVlizer = m_SettingFile.ConnstringCVLizer
			m_connStr_Systeminfo = m_SettingFile.ConnstringSysteminfo
			m_connStr_Scanjobs = m_SettingFile.ConnstringScanjobs
			m_connStr_Email = m_SettingFile.ConnstringEMail


			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
			ucCVWatcher = New ucCVEMailWatcher(m_SettingFile)
			pnlCVEMail.Controls.Add(ucCVWatcher)
			ucCVWatcher.Dock = DockStyle.Fill

		End Sub

#End Region



#Region "Helpers"


		Private Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

			Dim m_md As New SPProgUtility.Mandanten.Mandant
			Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
			Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
			Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

			Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
			Dim translate = clsTransalation.GetTranslationInObject

			Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

		End Function

		Private Function ParseToDouble(ByVal stringvalue As String, ByVal value As Double?) As Double
			Dim result As Double
			If (Not Double.TryParse(stringvalue, result)) Then
				Return value
			End If
			Return result
		End Function

		Private Function ParseToBoolean(ByVal stringvalue As String, ByVal value As Boolean?) As Boolean
			Dim result As Boolean
			If (Not Boolean.TryParse(stringvalue, result)) Then
				Return value
			End If
			Return result
		End Function


#End Region


		Private Sub OnbtnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
			If Not ucCVWatcher Is Nothing Then ucCVWatcher.CleanUp()

			Me.Close()

		End Sub
	End Class


End Namespace