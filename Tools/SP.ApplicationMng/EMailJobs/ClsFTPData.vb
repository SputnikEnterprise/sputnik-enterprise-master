
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.Applicant
Imports SP.DatabaseAccess.Applicant.DataObjects
Imports SP.DatabaseAccess.EMailJob
Imports SP.DatabaseAccess.EMailJob.DataObjects


Imports System.Text.RegularExpressions
Imports SP.Infrastructure.Logging
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors

Imports DevExpress.XtraBars
Imports System.ComponentModel
Imports System.Reflection

Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.ColorUtility.ClsColorUtility
Imports SPProgUtility.SPUserSec.ClsUserSec

Imports SPProgUtility.MainUtilities
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports DevExpress.LookAndFeel
Imports SP.Infrastructure.UI

Imports DevExpress.XtraSplashScreen
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Base
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports DevExpress.Pdf
Imports SPProgUtility.CommonXmlUtility

Imports System.Text
Imports DevExpress.Skins
Imports DevExpress.UserSkins
Imports SPProgUtility
Imports System.Security.Cryptography
Imports System.IO
Imports SP.Infrastructure
Imports System.Collections.Specialized
Imports System.Net
Imports TrxmlUtility
Imports System.Threading.Tasks
Imports System.Threading
Imports SP.Infrastructure.Initialization


Namespace ChilKatUtility

	Partial Class EMailUtility


		Public Function UploadFileToFTP(ByVal filename As String, ByVal settingData As EMailSettingData) As Boolean
			Dim success As Boolean = True
			Dim ftpUserName As String
			Dim ftpUserPassword As String
			Dim ftpRDPath As String

			If Not File.Exists(filename) Then
				m_Logger.LogWarning(String.Format("file {0} was not founded!", filename))
				Return False
			End If

			If settingData.UploadForWhat = EMailSettingData.UploadEnum.ReportUpload Then
				ftpUserName = settingData.Report_FTPUser
				ftpUserPassword = settingData.Report_FTPPW
				ftpRDPath = settingData.Report_FTPRD

			ElseIf settingData.UploadForWhat = EMailSettingData.UploadEnum.CVUpload Then
				ftpUserName = settingData.CV_FTPUser
				ftpUserPassword = settingData.CV_FTPPW
				ftpRDPath = settingData.CV_FTPRD

			Else

				Return False
			End If

			m_ftp2.Hostname = My.Settings.ftpSite_Scan
			m_ftp2.Username = ftpUserName	' FTP_LUBAG_USER_NAME
			m_ftp2.Password = ftpUserPassword	' FTP_USER_PASSWORD


			'  Connect and login to the FTP server.
			success = m_ftp2.Connect()
			If (success <> True) Then
				m_Logger.LogError(String.Format("can not connect to ftp! {0} | {1} | {2} | {3}",My.Settings.ftpSite_Scan, ftpUserName, ftpUserPassword, m_ftp2.LastErrorText))
				Console.WriteLine(m_ftp2.LastErrorText)
				Return False
			End If


			'  Change to the remote directory where the file will be uploaded.
			If Not String.IsNullOrWhiteSpace(ftpRDPath) Then
				success = m_ftp2.ChangeRemoteDir(ftpRDPath)
				If (success <> True) Then
					m_Logger.LogError(String.Format("can not connect to ftp folder! {0} | {1}", settingData.Report_FTPRD, m_ftp2.LastErrorText))
					Console.WriteLine(m_ftp2.LastErrorText)
					Return False
				End If
			End If

			'  Upload a file.
			Dim localFilename As String = filename
			Dim remoteFilename As String = Path.GetFileName(filename)

			success = m_ftp2.PutFile(localFilename, remoteFilename)
			If (success <> True) Then
				m_Logger.LogError(String.Format("can not put file on ftp folder! {0} | {1} | {2}", localFilename, remoteFilename, m_ftp2.LastErrorText))
				Return False
			End If


			Return success

		End Function



	End Class


End Namespace
