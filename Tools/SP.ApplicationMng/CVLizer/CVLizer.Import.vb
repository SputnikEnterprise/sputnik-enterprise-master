
Imports System

Imports TrxmlUtility.Xsd
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.Applicant
Imports SP.DatabaseAccess.CVLizer.DataObjects


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
Imports SP.DatabaseAccess.CVLizer
Imports SP.ApplicationMng.CVLizer.UI
Imports SP.ApplicationMng.CVLizer.DataObject


Namespace CVLizer.Import

	Public Class CVLizer


#Region "private consts"


		Private Const webServiceCVLUri As String = "http://cvlizer.joinvision.com:80/cvlizer/exservicesoap"
		Private Const CVL_WEB_REQUEST_URL As String = "https://cvlizer.joinvision.com/cvlizer/rest/v1/extract/xml/"
		Private Const JOBROOM_WEB_REQUEST_URL As String = "http://test-api.job-room.ch/jobAdvertisements/v1"

		Private Const CVL_USER_NAME As String = "username"
		Private Const CLV_PASSWORD As String = "password"

		Private Const CLV_DEMO_TOKEN As String = "demotoken"
		Private Const CVL_SPUTNIK_TOKEN As String = "cvltoken"

		Private Const JobRoom_USER_NAME As String = "username"
		Private Const JobRoom_PASSWORD As String = "password"

		Private Const JSON_AUTHENTICATION_TOKEN As String = "JSON_AUTHENTICATION_TOKEN"
		Private Const JSON_JOBROOM_AUTHENTICATION_TOKEN As String = "JSON_JOBROOM_AUTHENTICATION_TOKEN"

		Private Const TEMPORARY_FOLDER_ORIGINAL_PDF As String = "ORIGINAL_PDF"
		Private Const TEMPORARY_FOLDER_SPLITTED_PDFS As String = "SPLITTED_PDFS"
		Private Const RUNTIME_COMMON_CONFIG_FOLDER As String = "Config"
		Private Const PROGRAM_SETTING_FILE As String = "NotifyerSettings.xml"
		Private Const PROGRAM_XML_SETTING_PATH As String = "Settings/Path"
		Private Const PROGRAM_XML_SETTING_DBCONNECTIONS As String = "Settings/DBConnection"

		Private m_FinishValue As CVLFinishValue

#End Region



#Region "private fields"

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As SP.Infrastructure.Utility

		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_UtilityUI As SP.Infrastructure.UI.UtilityUI

		Private m_customerID As String

		Private m_CVLXMLFile As String
		Private m_CVLizerXMLData As CVLizerXMLData

		Private m_CVLXMLFolder As String
		Private m_CVFileData As CVFileData

		Private m_CVLUserName As String
		Private m_CVLToken As String
		Private Property m_DoneFile As List(Of CVFileData)

#End Region


#Region "public property"
		Public Property PayableUserData As CustomerPayableUserData
		Public Property Customer_ID As String
		Public Property ApplicationID As Integer?
		Public Property ApplicantID As Integer?

		Public Property m_SelectedFile As List(Of String)

		''' <summary>
		''' The common data accessobject.
		''' </summary>
		Public Property m_AppDatabaseAccess As IAppDatabaseAccess

		''' <summary>
		''' The cv data access object.
		''' </summary>
		Public Property m_CVLDatabaseAccess As ICVLizerDatabaseAccess

		''' <summary>
		''' connection string
		''' </summary>
		Public Property m_connStr_Application As String
		Public Property m_connStr_CVlizer As String
		Public Property m_connStr_Systeminfo As String
		Public Property m_connStr_Scanjobs As String
		Public Property m_connStr_Email As String
		Public Property m_SettingFile As ProgramSettings
		Public Property ErrorDescription As String


		Public Enum CVLFinishValue
			SUCCESS
			FAILED
			DUPLICATED
		End Enum

		Public ReadOnly Property ProcessFinishValue As CVLFinishValue
			Get
				Return m_FinishValue
			End Get
		End Property

#End Region


#Region "constructor"

		Public Sub New()

			m_Utility = New SP.Infrastructure.Utility
			m_UtilityUI = New SP.Infrastructure.UI.UtilityUI

#If DEBUG Then
			'm_customerID = "EFFE4D79-2AC6-4f9f-839F-1E4E9D6D9E2A"
#End If

			m_CVLUserName = CVL_USER_NAME
#If DEBUG Then
			m_CVLToken = CLV_DEMO_TOKEN
#End If


		End Sub

#End Region

		Public Function LoadCVLDomains() As String
			Dim result = String.Empty

			result = PerformCVDomainFileWithWebService()


			Return result
		End Function

		Public Function ParseCVFileWithCVLizer(ByVal cvFilename As List(Of String)) As Boolean
			Dim result As Boolean = True
			Dim existsOldParsing As Boolean = False

			m_CVFileData = New CVFileData
			m_customerID = Customer_ID

			If m_SettingFile.CVLParseAsDemo Then
				m_CVLToken = CLV_DEMO_TOKEN
			Else
				m_CVLToken = CVL_SPUTNIK_TOKEN
			End If


			Try
				If String.IsNullOrWhiteSpace(m_SettingFile.CVLXMLFolder) Then m_SettingFile.CVLXMLFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
				m_CVLXMLFolder = Path.Combine(m_SettingFile.CVLXMLFolder, m_customerID)
				If Not Directory.Exists(m_CVLXMLFolder) Then Directory.CreateDirectory(m_CVLXMLFolder)
				m_SelectedFile = cvFilename


				If Not m_SelectedFile(0).ToLower.Contains(".xml") Then
					Dim msg As String = String.Format("User: {0} | Token: {1} | Parsing as Demo: {2}", CVL_USER_NAME, m_CVLToken, m_SettingFile.CVLParseAsDemo)
					m_Logger.LogInfo(msg)

					If m_SettingFile.AskSendToCVLizer Then
						If Not m_UtilityUI.ShowYesNoDialog(String.Format("{0}?", msg)) Then
							Return False
						End If
					End If

					result = result AndAlso PerformCVMergeFileWithWebService(m_SelectedFile)

				Else
					' read xml file
					m_CVFileData.XMLFileName = m_SelectedFile(0)

				End If

				result = result AndAlso ParseAssignedXMLDetail(True)
				If result Then
					m_FinishValue = CVLFinishValue.SUCCESS
				Else
					m_FinishValue = CVLFinishValue.FAILED
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				Return False

			End Try


			Return result

		End Function

		Public Function GetCVLProfileData() As CVLizerXMLData
			m_customerID = Customer_ID

			If m_FinishValue = CVLFinishValue.DUPLICATED Then

			End If

			Return m_CVLizerXMLData
		End Function

		Private Function AddLogToCustomerpayableService() As Boolean
			Dim success As Boolean = True

			If PayableUserData Is Nothing Then Return False

			success = success AndAlso m_CVLDatabaseAccess.AddCustomerPayableServiceUsage(m_customerID, PayableUserData)
			If Not success Then
				m_Logger.LogWarning(String.Format("logging was failed!!! >>> {0}  | {1} | {2}", PayableUserData.CustomerID, PayableUserData.AdvisorID, PayableUserData.Advisorname))
			End If

			Return success

		End Function

		Private Function PerformCVMergeFileWithWebService(ByVal cvFilename As List(Of String)) As Boolean
			Dim success As Boolean = True

			If String.IsNullOrWhiteSpace(cvFilename(0)) Then Return False

			Try
				m_Logger.LogDebug(String.Format("uploading file for parsing: {0}", String.Join(" | ", cvFilename)))

				Dim client As SPCVLCategorizeService.SemanticExtractionService = New SPCVLCategorizeService.SemanticExtractionService()

				Dim newfiles As New List(Of SPCVLCategorizeService.inputDoc)
				For Each tmpFile In cvFilename
					Dim fileData = New SPCVLCategorizeService.inputDoc
					fileData.filename = tmpFile
					fileData.data = m_Utility.LoadFileBytes(tmpFile)

					newfiles.Add(fileData)

				Next
				If newfiles.Count = 0 Then
					m_Logger.LogWarning("cv would be not sent to CVLizer. file is empty.")
					Return False
				End If

				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
				Dim mergeResult = client.mergeToXML(m_CVLUserName, m_CVLToken, "DE", "cvlizer_3_0", newfiles.ToArray)
				Dim contingentResult = client.getContingent(m_CVLUserName, m_CVLToken)

				m_Logger.LogDebug(String.Format("parsing: contingent is now {0}", contingentResult))

				Dim tmpFilename = Path.GetTempFileName()
				Dim resultXMLFileName As String = String.Empty
				tmpFilename = Path.ChangeExtension(tmpFilename, "XML")
				Using sw As StreamWriter = New StreamWriter(tmpFilename, False, Encoding.UTF8)
					sw.Write(mergeResult)
					sw.Close()
				End Using
				m_Logger.LogDebug(String.Format("result file is created in: {0}", tmpFilename))
				success = success AndAlso File.Exists(tmpFilename) AndAlso AddLogToCustomerpayableService()

				Try
					resultXMLFileName = Path.Combine(m_CVLXMLFolder, IO.Path.GetFileName((Path.ChangeExtension(cvFilename(0), "xml"))))
					If IO.File.Exists(resultXMLFileName) Then
						m_Logger.LogDebug(String.Format("deleting file: {0}", resultXMLFileName))
						IO.File.Delete(resultXMLFileName)
					End If
					IO.File.Move(tmpFilename, resultXMLFileName)

					' set cvfile to result file
					m_CVFileData.XMLFileName = resultXMLFileName

				Catch ex As Exception
					m_Logger.LogError(String.Format("resultXMLFileName file could not be created! {0}", ex.ToString))
					If File.Exists(tmpFilename) Then
						m_CVFileData.XMLFileName = tmpFilename
					Else
						m_CVFileData.XMLFileName = resultXMLFileName
					End If

				End Try

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))
				ErrorDescription = ex.ToString
				success = False

			Finally

			End Try


			Return success

		End Function

		Private Function PerformCVDomainFileWithWebService() As String
			Dim tmpFilename = Path.GetTempFileName()

			Try
				Dim client As SPCVLCategorizeService.SemanticExtractionService = New SPCVLCategorizeService.SemanticExtractionService()

				Dim domainDataresult = client.getXMLDomainsFor("DE")

				m_Logger.LogDebug(String.Format("downloading domain data: domain data is in: {0}", domainDataresult))

				Dim resultXMLFileName As String = String.Empty
				tmpFilename = Path.ChangeExtension(tmpFilename, "XML")
				Using sw As StreamWriter = New StreamWriter(tmpFilename, False, Encoding.UTF8)
					sw.Write(domainDataresult)
					sw.Close()
				End Using
				m_Logger.LogDebug(String.Format("result file is created in: {0}", tmpFilename))


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))
				tmpFilename = String.Empty

			Finally

			End Try


			Return tmpFilename

		End Function

		Private Function ParseAssignedXMLDetail(ByVal SaveToDb As Boolean) As Boolean
			Dim success As Boolean = True
			Dim cvLizerUtility = New CVLizerLoader.CustomAreaType(m_customerID, m_CVFileData)

			If Not SaveToDb Then
				m_CVLizerXMLData = cvLizerUtility.ImportFromFile()
				success = success AndAlso Not (m_CVLizerXMLData Is Nothing)
			Else
				success = success AndAlso cvLizerUtility.PaseXMLFileAndAddToDatabase()
				If success Then m_CVLizerXMLData = cvLizerUtility.GetCVLProfileData()
			End If
			If Not m_CVLizerXMLData Is Nothing Then m_CVLizerXMLData.Customer_ID = m_customerID


			Return success

		End Function


#Region "helpers"

		Private Function AddFileData(ByVal myFilename As String, ByVal fileContent As Byte()) As Boolean
			Dim result As Boolean = True

			m_CVFileData = New CVFileData

			Dim myFile As New FileInfo(myFilename)
			Dim sizeInBytes As Long = myFile.Length
			If Path.GetExtension(myFilename).ToLower = ".xml" Then Return True

			Dim bytes() = fileContent
			If fileContent Is Nothing Then
				bytes = m_Utility.LoadFileBytes(myFilename)
			Else
				bytes = fileContent
			End If
			Dim s As New SHA256Managed
			Dim hash() As Byte = s.ComputeHash(bytes)
			Dim hashValue = Convert.ToBase64String(hash)

			Dim myFileExtension = GetMimeFromBytes(bytes)

			Dim extension = (From kp As KeyValuePair(Of String, String) In MIMETypesDictionary
											 Where kp.Value = myFileExtension
											 Select kp.Key).ToList()

			m_CVFileData.FileContent = bytes
			m_CVFileData.FileName = myFilename
			m_CVFileData.FileHash = hashValue
			m_CVFileData.FileExtension = Path.GetExtension(myFilename)
			m_CVFileData.FileDate = File.GetCreationTimeUtc(myFilename)
			m_CVFileData.Filesize = myFile.Length

			result = m_CVLDatabaseAccess.AddParsingFileHashData(m_customerID, myFilename, hashValue)


			Return result

		End Function

		Private Function IsFileAlreadyParsed(ByVal cvFilename As String) As Boolean
			Dim result As Boolean?

			m_CVFileData = New CVFileData

			Dim myFile As New FileInfo(cvFilename)
			Dim sizeInBytes As Long = myFile.Length
			If Path.GetExtension(cvFilename).ToLower = ".xml" Then Return False

			Dim bytes() = m_Utility.LoadFileBytes(cvFilename)
			Dim s As New SHA256Managed
			Dim hash() As Byte = s.ComputeHash(bytes)
			Dim hashValue = Convert.ToBase64String(hash)

			Dim myFileExtension = GetMimeFromBytes(bytes)

			Dim extension = (From kp As KeyValuePair(Of String, String) In MIMETypesDictionary
											 Where kp.Value = myFileExtension
											 Select kp.Key).ToList()

			m_CVFileData.FileContent = bytes
			m_CVFileData.FileName = cvFilename
			m_CVFileData.FileHash = hashValue
			m_CVFileData.FileExtension = Path.GetExtension(cvFilename)
			m_CVFileData.FileDate = File.GetCreationTimeUtc(cvFilename)
			m_CVFileData.Filesize = myFile.Length

			result = m_CVLDatabaseAccess.ExistsCVLFile(m_customerID, hashValue)

			If (result Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_Logger.LogError(("Dokument-Daten konnten nicht überprüft werden."))

				Return False
			End If


			Return result

		End Function

		Private Function FilesareAlreadyParsed(ByVal cvFilename As List(Of String)) As Boolean
			Dim result As Boolean = False
			Dim existsFileHashes As String = String.Empty
			Dim hashValues = New List(Of String)

			m_DoneFile = New List(Of CVFileData)

			For Each itm In cvFilename
				Dim myDoneFile = New CVFileData

				Dim myFile As New FileInfo(itm)
				Dim sizeInBytes As Long = myFile.Length
				If Path.GetExtension(itm).ToLower = ".xml" Then Return False

				Dim bytes() = m_Utility.LoadFileBytes(itm)
				Dim s As New SHA256Managed
				Dim hash() As Byte = s.ComputeHash(bytes)
				Dim hashValue = Convert.ToBase64String(hash)

				Dim myFileExtension = GetMimeFromBytes(bytes)

				Dim extension = (From kp As KeyValuePair(Of String, String) In MIMETypesDictionary
												 Where kp.Value = myFileExtension
												 Select kp.Key).ToList()

				myDoneFile.FileContent = bytes
				myDoneFile.FileName = itm
				myDoneFile.FileHash = hashValue
				myDoneFile.FileExtension = Path.GetExtension(itm)
				myDoneFile.FileDate = File.GetCreationTimeUtc(itm)
				myDoneFile.Filesize = myFile.Length

				hashValues.Add(hashValue)
				result = m_CVLDatabaseAccess.ExistsCVLFile(m_customerID, hashValue)
				If result Then
					m_DoneFile.Add(myDoneFile)

					existsFileHashes &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(existsFileHashes), "", ","), myDoneFile.FileHash)
				End If

			Next
			If Not String.IsNullOrWhiteSpace(existsFileHashes) Then
				Dim persData = m_CVLDatabaseAccess.LoadAssignedPersonalInformationDataWithFileHashValueData(m_customerID, existsFileHashes)
				If persData Is Nothing OrElse persData.PersonalID.GetValueOrDefault(0) = 0 Then Return False

				If persData.PersonalID.GetValueOrDefault(0) > 0 Then
					result = result AndAlso m_AppDatabaseAccess.UpdateNewApplicationWithOldApplicantData(m_customerID, persData.PersonalID, ApplicationID, ApplicantID)

				End If
			End If


			Return result

		End Function

#End Region


	End Class

End Namespace
