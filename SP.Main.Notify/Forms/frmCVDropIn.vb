
Imports System.ComponentModel
Imports System.Text
Imports DevExpress.XtraEditors
Imports DevExpress.Skins
Imports DevExpress.LookAndFeel
Imports DevExpress.UserSkins
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Common
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SPProgUtility.CommonXmlUtility
Imports SPProgUtility.Mandanten
Imports SP.Infrastructure.Initialization
Imports System.Threading.Tasks
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Common.DataObjects
Imports System.Threading
'Imports SP.Main.Notify.SPNotificationWebService
Imports DevExpress.XtraEditors.Repository
Imports SP.Main.Notify.ScanJobs
Imports DevExpress.XtraGrid.Views.Base
Imports System.IO
Imports SP.Infrastructure
Imports DevExpress.XtraBars.Alerter
Imports SP.DatabaseAccess.ScanJob.DataObjects.ScanJobData
Imports SP.Internal.Automations.SPNotificationWebService
Imports SP.Internal.Automations

Namespace UI


	Public Class frmCVDropIn



#Region "Private Consts"

		Private Const DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPNotification.asmx"
		Private Const DEFAULT_SPUTNIK_SCANJOB_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPScanJobUtility.asmx"
		Private Const MANDANT_XML_SETTING_SPUTNIK_SONSTIGES_SETTING As String = "MD_{0}/Sonstiges"

#End Region


#Region "Private fields"

		''' <summary>
		''' The Initialization data.
		''' </summary>
		Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

		''' <summary>
		''' The translation value helper.
		''' </summary>
		Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

		''' <summary>
		''' The employee data access object.
		''' </summary>
		Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

		''' <summary>
		''' The Listing data access object.
		''' </summary>
		Private m_ListingDatabaseAccess As IListingDatabaseAccess

		''' <summary>
		''' The SPProgUtility object.
		''' </summary>
		Private m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI

		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As Utility

		Private m_alarm As Threading.Timer

		''' <summary>
		''' The common database access.
		''' </summary>
		Protected m_CommonDatabaseAccess As ICommonDatabaseAccess

		''' <summary>
		''' Settings xml.
		''' </summary>
		Private m_MandantSettingsXml As SettingsXml

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		''' <summary>
		''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
		''' </summary>
		Private m_SuppressUIEvents As Boolean = False

		''' <summary>
		''' The mandant.
		''' </summary>
		Private m_MandantData As Mandant
		Private m_connectionString As String
		Private m_SonstigesSetting As String

		''' <summary>
		''' Service Uri of Sputnik notification util webservice.
		''' </summary>
		Private m_NotificationUtilWebServiceUri As String

		''' <summary>
		''' Service Uri of Sputnik scan job util webservice.
		''' </summary>
		Private m_ScanjobUtilWebServiceUri As String

		Private m_firstNotifyID As Integer?
		Private m_PopupMenu As DevExpress.XtraBars.PopupMenu

		Private gridDataList As New BindingList(Of NotifyData)
		Private m_ExitApplication As Boolean
		Private m_Timer As System.Timers.Timer

		Private m_OriginalCustomerID As String
		Private m_scanDropInData As New SP.Main.Notify.SPScanJobWebService.ScanDropInDTO


#End Region


#Region "Contructor"

		Sub New(ByVal _setting As InitializeClass)

			DevExpress.UserSkins.BonusSkins.Register()
			DevExpress.Skins.SkinManager.EnableFormSkins()

			m_InitializationData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

			m_MandantData = New Mandant
			m_UtilityUI = New UtilityUI
			m_Utility = New Utility

			m_connectionString = m_InitializationData.MDData.MDDbConn
			m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_ListingDatabaseAccess = New ListingDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

			m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))
			m_SonstigesSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_SONSTIGES_SETTING, m_InitializationData.MDData.MDNr)

			m_SuppressUIEvents = True

			InitializeComponent()

			m_ExitApplication = False

			Dim domainName = m_InitializationData.MDData.WebserviceDomain
			m_NotificationUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI)
			m_ScanjobUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_SCANJOB_UTIL_WEBSERVICE_URI)


			m_PopupMenu = New DevExpress.XtraBars.PopupMenu

			Me.AllowDrop = True
			Me.pnlMain.AllowDrop = True
			Me.TopMost = True

			Reset()
			TranslateControls()

		End Sub

#End Region


		Private Sub Reset()

			AlertControl1.AllowHtmlText = True
			AlertControl1.ShowCloseButton = True
			AlertControl1.AllowHotTrack = False
			AlertControl1.AppearanceText.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
			AlertControl1.FormLocation = AlertFormLocation.BottomRight

		End Sub

		''' <summary>
		'''  Translate controls
		''' </summary>
		Private Sub TranslateControls()

			Text = m_Translate.GetSafeTranslationValue(Text)
			lblInfo.Text = m_Translate.GetSafeTranslationValue(lblInfo.Text)
			lblOpenFile.Text = m_Translate.GetSafeTranslationValue(lblOpenFile.Text)

		End Sub


		''' <summary>
		''' Search for Paidlist over web service.
		''' </summary>
		Private Sub SearchUploadDropInlistViaWebService()

			Dim uiSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext()

			Task(Of Boolean).Factory.StartNew(Function() PerformUploadDropInWebserviceCallAsync(),
																							CancellationToken.None,
																							TaskCreationOptions.None,
																							TaskScheduler.Default).ContinueWith(Sub(t) FinishUploadDropInWebserviceCallTask(t), CancellationToken.None,
																																									TaskContinuationOptions.None, uiSynchronizationContext)

		End Sub

		''' <summary>
		'''  Performs Paidlist check asynchronous.
		''' </summary>
		''' <returns>The report response.</returns>
		Private Function PerformUploadDropInWebserviceCallAsync() As Boolean

			CheckMandantChanges()

			Dim Customer_ID As String = m_InitializationData.MDData.MDGuid
			Dim userName As String = String.Empty
			If m_scanDropInData Is Nothing Then Return False

			Dim webservice As New SPScanJobWebService.SPScanJobUtilitySoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ScanjobUtilWebServiceUri)

			Dim wsUserData As SPScanJobWebService.SystemUserData = New SPScanJobWebService.SystemUserData With {.UsereMail = m_InitializationData.UserData.UsereMail,
				.UserFiliale = m_InitializationData.UserData.UserFiliale,
				.UserBranchOffice = m_InitializationData.UserData.UserBusinessBranch,
				.UserFName = m_InitializationData.UserData.UserFName,
				.UserFTitel = m_InitializationData.UserData.UserFTitel,
				.UserGuid = m_InitializationData.UserData.UserGuid,
				.UserKST = m_InitializationData.UserData.UserKST,
				.UserLanguage = m_InitializationData.UserData.UserLanguage,
				.UserLName = m_InitializationData.UserData.UserLName,
				.UserMDCanton = m_InitializationData.UserData.UserMDCanton,
				.UserMDDTelefon = m_InitializationData.UserData.UserMDDTelefon,
				.UserMDeMail = m_InitializationData.UserData.UserMDeMail,
				.UserMDGuid = m_InitializationData.UserData.UserMDGuid,
				.UserMDHomepage = m_InitializationData.UserData.UserMDHomepage,
				.UserMDLand = m_InitializationData.UserData.UserMDLand,
				.UserMDName = m_InitializationData.UserData.UserMDName,
				.UserMDName2 = m_InitializationData.UserData.UserMDName2,
				.UserMDName3 = m_InitializationData.UserData.UserMDName3,
				.UserMDOrt = m_InitializationData.UserData.UserMDOrt,
				.UserMDPLZ = m_InitializationData.UserData.UserMDPLZ,
				.UserMDPostfach = m_InitializationData.UserData.UserMDPostfach,
				.UserMDStrasse = m_InitializationData.UserData.UserMDStrasse,
				.UserMDTelefax = m_InitializationData.UserData.UserMDTelefax,
				.UserMDTelefon = m_InitializationData.UserData.UserMDTelefon,
				.UserMobile = m_InitializationData.UserData.UserMobile,
				.UserNr = m_InitializationData.UserData.UserNr,
				.UserSTitel = m_InitializationData.UserData.UserSTitel,
				.UserSalutation = m_InitializationData.UserData.UserSalutation,
				.UserTelefax = m_InitializationData.UserData.UserTelefax,
				.UserTelefon = m_InitializationData.UserData.UserTelefon,
				.UserFullName = m_InitializationData.UserData.UserFullName,
				.UserFullNameWithComma = m_InitializationData.UserData.UserFullNameWithComma,
				.UserKST_1 = m_InitializationData.UserData.UserKST_1,
				.UserKST_2 = m_InitializationData.UserData.UserKST_2,
				.UserLoginname = m_InitializationData.UserData.UserLoginname,
				.UserLoginPassword = m_InitializationData.UserData.UserLoginPassword
				}

			' Read data over webservice
			Dim success = webservice.GetCVLDropInJob(Customer_ID, wsUserData, m_scanDropInData)

			Return success

		End Function

		''' <summary>
		''' Finish uploadlist web service call.
		''' </summary>
		Private Sub FinishUploadDropInWebserviceCallTask(ByVal t As Task(Of Boolean))

			Try

				Select Case t.Status
					Case TaskStatus.RanToCompletion
						' Webservice call was successful.
						m_SuppressUIEvents = True

						m_SuppressUIEvents = False

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

		''' <summary>
		'''  Performs Paidlist check asynchronous.
		''' </summary>
		''' <returns>The report response.</returns>
		Private Function PerformUpdatePayableServiceWebservice() As Boolean
			Dim success As Boolean = True
			Dim Customer_ID As String = m_InitializationData.MDData.MDGuid
			Dim userName As String = String.Empty
			If m_scanDropInData Is Nothing Then Return False


#If DEBUG Then
			Customer_ID = m_InitializationData.MDData.MDGuid
#End If

			Trace.WriteLine(String.Format("MDNr: {0} | Customer_ID: {1} contacting...", m_InitializationData.MDData.MDNr, m_InitializationData.MDData.MDGuid))

			Dim webservice As New SPNotificationWebService.SPNotificationSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_NotificationUtilWebServiceUri)



			Dim wsUserData As SPNotificationWebService.SystemUserData = New SPNotificationWebService.SystemUserData With {.UsereMail = m_InitializationData.UserData.UsereMail,
				.UserFiliale = m_InitializationData.UserData.UserFiliale,
				.UserBranchOffice = m_InitializationData.UserData.UserBusinessBranch,
				.UserFName = m_InitializationData.UserData.UserFName,
				.UserFTitel = m_InitializationData.UserData.UserFTitel,
				.UserGuid = m_InitializationData.UserData.UserGuid,
				.UserKST = m_InitializationData.UserData.UserKST,
				.UserLanguage = m_InitializationData.UserData.UserLanguage,
				.UserLName = m_InitializationData.UserData.UserLName,
				.UserMDCanton = m_InitializationData.UserData.UserMDCanton,
				.UserMDDTelefon = m_InitializationData.UserData.UserMDDTelefon,
				.UserMDeMail = m_InitializationData.UserData.UserMDeMail,
				.UserMDGuid = m_InitializationData.UserData.UserMDGuid,
				.UserMDHomepage = m_InitializationData.UserData.UserMDHomepage,
				.UserMDLand = m_InitializationData.UserData.UserMDLand,
				.UserMDName = m_InitializationData.UserData.UserMDName,
				.UserMDName2 = m_InitializationData.UserData.UserMDName2,
				.UserMDName3 = m_InitializationData.UserData.UserMDName3,
				.UserMDOrt = m_InitializationData.UserData.UserMDOrt,
				.UserMDPLZ = m_InitializationData.UserData.UserMDPLZ,
				.UserMDPostfach = m_InitializationData.UserData.UserMDPostfach,
				.UserMDStrasse = m_InitializationData.UserData.UserMDStrasse,
				.UserMDTelefax = m_InitializationData.UserData.UserMDTelefax,
				.UserMDTelefon = m_InitializationData.UserData.UserMDTelefon,
				.UserMobile = m_InitializationData.UserData.UserMobile,
				.UserNr = m_InitializationData.UserData.UserNr,
				.UserSTitel = m_InitializationData.UserData.UserSTitel,
				.UserSalutation = m_InitializationData.UserData.UserSalutation,
				.UserTelefax = m_InitializationData.UserData.UserTelefax,
				.UserTelefon = m_InitializationData.UserData.UserTelefon,
				.UserFullName = m_InitializationData.UserData.UserFullName,
				.UserFullNameWithComma = m_InitializationData.UserData.UserFullNameWithComma,
				.UserKST_1 = m_InitializationData.UserData.UserKST_1,
				.UserKST_2 = m_InitializationData.UserData.UserKST_2,
				.UserLoginname = m_InitializationData.UserData.UserLoginname,
				.UserLoginPassword = m_InitializationData.UserData.UserLoginPassword
			}

			' Read data over webservice
			success = success AndAlso webservice.UpdateAssignedCustomerPayableService(Customer_ID, wsUserData, "CVLIZER_SCAN", "CVDropIN")


			Return success

		End Function

		Private Sub CheckMandantChanges()

			If m_MandantData.GetDefaultMDNr <> m_InitializationData.MDData.MDNr Then

				Dim MandantData = ChangeMandantData(m_MandantData.GetDefaultMDNr, m_MandantData.GetDefaultUSNr)
				m_InitializationData = MandantData

				Dim supressUIEventState = m_SuppressUIEvents
				m_SuppressUIEvents = True
				InitWithConfigurationData(m_InitializationData)
				m_SuppressUIEvents = supressUIEventState

			End If

		End Sub

		''' <summary>
		''' Loads file bytes with file dialog.
		''' </summary>
		Private Sub LoadFileBytesWithFileDialg()
			Dim fileExtension As String = String.Empty
			Dim fileName As String = String.Empty
			Dim dlg As New OpenFileDialog

			With dlg
				.Filter = "MS Word-Dokumente (*.doc;*.docx)|*.doc*|PDF-Dokumente (*.pdf)|*.pdf|Alle Dateien (*.*)|*.*"
				.FilterIndex = 0
				.InitialDirectory = My.Settings.cvScanFolders
				.Title = "Lebenslauf öffnen"
				.FileName = String.Empty

				If .ShowDialog() = DialogResult.OK Then

					fileName = String.Empty
					Dim bytes() = m_Utility.LoadFileBytes(.FileName)

					If bytes Is Nothing Then
						fileExtension = String.Empty

						m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Datei konnte nicht geöffnet werden."))
					Else
						fileExtension = System.IO.Path.GetExtension(.FileName)

						If Not fileExtension Is Nothing Then
							fileExtension = fileExtension.Replace(".", "")
						End If

						fileName = .FileName
						LoadFileScanInfo(New FileInfo(fileName))

						My.Settings.cvScanFolders = New FileInfo(fileName).DirectoryName
						My.Settings.Save()
					End If

				End If
			End With

		End Sub

		Private Function LoadFileScanInfo(ByVal fileData As FileInfo) As Boolean
			Dim success As Boolean = True

			m_scanDropInData.BusinessBranch = m_InitializationData.UserData.UserBusinessBranch
			m_scanDropInData.CreatedFrom = m_InitializationData.UserData.UserFullName
			m_scanDropInData.Customer_ID = m_InitializationData.UserData.UserMDGuid
			m_scanDropInData.DocumentCategoryNumber = 201
			m_scanDropInData.FileExtension = fileData.Extension.Replace(".", "")
			m_scanDropInData.ModulNumber = DatabaseAccess.ScanJob.DataObjects.ScanJobData.ScannModulEnum.Employee
			m_scanDropInData.ScanContent = m_Utility.LoadFileBytes(fileData.FullName)

			success = success AndAlso PerformUploadDropInWebserviceCallAsync()


			Return success

		End Function


#Region "forms"

		Private Sub frmCVDropIn_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
			If e.Modifiers = Keys.Control AndAlso e.KeyCode = Keys.V Then

				Dim data_object As IDataObject = Clipboard.GetDataObject()
				If data_object.GetDataPresent(DataFormats.FileDrop) Then

				End If

				e.Handled = True
			End If

		End Sub

		Private Sub OnfrmReportDropIn_Load(sender As Object, e As EventArgs) Handles MyBase.Load
			pnlMain.AllowDrop = True
		End Sub

		Private Sub OnpnlMain_DragDrop(sender As Object, e As DragEventArgs) Handles pnlMain.DragDrop
			Dim files() As String = e.Data.GetData(DataFormats.FileDrop)
			Dim fileName As String = String.Empty
			Dim fileExtension As String = String.Empty

			'If e.Data.GetFormats(False)(1) = "RenPrivateFileAttachments" Then
			'End If

			If e.Data.GetDataPresent("FileGroupDescriptor") Then
				'supports a drop of a Outlook message 
				fileName = GetAssignedOutlookAttachment(e)
				If Not fileName Is Nothing Then
					files = New String() {fileName}
				Else
					Dim m_Path As New SPProgUtility.ClsProgSettingPath
					Dim objOL As Object = Nothing
					objOL = CreateObject("Outlook.Application")
					Dim myobj As Object
					For i As Integer = 1 To objOL.ActiveExplorer.Selection.Count
						myobj = objOL.ActiveExplorer.Selection.Item(i)

						'hardcode a destination path for testing
						Dim strFilename As String = myobj.Subject
						Try
							fileName = strFilename

						Catch ex As Exception

						End Try

						fileName = System.Text.RegularExpressions.Regex.Replace(myobj.Subject, "[\\/:*?""<>|\r\n]", "", System.Text.RegularExpressions.RegexOptions.Singleline)
						fileName &= ".msg"
						Dim strFile As String = IO.Path.Combine(m_Path.GetSpS2DeleteHomeFolder, fileName)

						myobj.SaveAs(strFile)
						files = New String() {strFile}
					Next
				End If

			End If
			Dim success As Boolean = True
			If Not files Is Nothing AndAlso files.Count > 0 Then
				Dim fileInfo As New FileInfo(files(0))

				success = success AndAlso LoadFileScanInfo(fileInfo)

				Dim header As String = "<b>" & m_Translate.GetSafeTranslationValue(String.Format("{0} Übermittlung der Dateien", If(success, "Erfolgreiche", "Fehlerhafte"))) & "</b>"
				Dim successbody As String = String.Format(m_Translate.GetSafeTranslationValue("Ihre Datei {0}({1}){0}wurde erfolgreich übermittelt."),
																									vbNewLine, fileInfo.FullName)
				Dim errorbody As String = String.Format(m_Translate.GetSafeTranslationValue("Ihre Datei {0}({1}){0}<b>konnte nicht</b> erfolgreich übermittelt werden!"), vbNewLine, fileInfo.FullName)
				For Each aForm In AlertControl1.AlertFormList
					aForm.Close()
				Next
				If success Then
					AlertControl1.Show(DirectCast(Me, XtraForm), New AlertInfo(header, successbody))
					m_Logger.LogInfo(successbody)
				Else
					AlertControl1.Show(DirectCast(Me, XtraForm), New AlertInfo(header, errorbody))
					m_Logger.LogInfo(errorbody)
				End If

			End If

		End Sub

		Private Sub OnpnlMain_DragEnter(sender As Object, e As DragEventArgs) Handles pnlMain.DragEnter
			e.Effect = DragDropEffects.Copy
		End Sub

		Private Sub OnlblOpenFile_Click(sender As Object, e As EventArgs) Handles lblOpenFile.Click
			LoadFileBytesWithFileDialg()
		End Sub

		Public Function GetAssignedOutlookAttachment(ByVal e As System.Windows.Forms.DragEventArgs) As String
			Dim theStream As IO.Stream = DirectCast(e.Data.GetData("FileGroupDescriptor"), IO.Stream)
			Dim fileGroupDescriptor As Byte() = New Byte(511) {}
			theStream.Read(fileGroupDescriptor, 0, 512)
			'Dim TempFiles As List(Of String) = Nothing

			' used to build the filename from the FileGroupDescriptor block
			Dim fileName As New System.Text.StringBuilder("")

			' this trick gets the filename of the passed attached file
			Dim i As Integer = 76
			While fileGroupDescriptor(i) <> 0
				fileName.Append(Convert.ToChar(fileGroupDescriptor(i)))
				i += 1
			End While
			theStream.Close()
			Dim path As String = IO.Path.GetTempPath()

			' put the zip file into the temp directory
			Dim attachmentFileName As String = System.Text.RegularExpressions.Regex.Replace(fileName.ToString, "[\\/:*?""<>|\r\n]", "", System.Text.RegularExpressions.RegexOptions.Singleline)
			Dim theFile As String = path + attachmentFileName ' fileName.ToString()
			'TempFiles.Add(theFile)

			Dim ms As IO.MemoryStream = DirectCast(e.Data.GetData("FileContents", True), IO.MemoryStream)
			If ms Is Nothing Then
				Return Nothing
			End If
			' allocate enough bytes to hold the raw data
			'Dim fileBytes As Byte() = New Byte(ms.Length - 1) {}
			Dim fileBytes As Byte() = New Byte(CInt(ms.Length - 1)) {}

			' set starting position at first byte and read in the raw data
			ms.Position = 0
			ms.Read(fileBytes, 0, CInt(ms.Length))

			' create a file and save the raw zip file to it
			Dim fs As New IO.FileStream(theFile, IO.FileMode.Create)
			fs.Write(fileBytes, 0, CInt(fileBytes.Length))

			fs.Close()
			' close the file

			Return theFile
		End Function

#End Region




#Region "Helpers"


		Private Function ChangeMandantData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

			Dim m_md As New SPProgUtility.Mandanten.Mandant
			Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
			Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
			Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

			Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
			Dim translate = clsTransalation.GetTranslationInObject

			Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

		End Function

		Private Sub InitWithConfigurationData(ByVal initializationClass As InitializeClass)

			m_InitializationData = initializationClass
			'm_Translate = translationHelper
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

			Try
				m_connectionString = m_InitializationData.MDData.MDDbConn
				m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
				m_ListingDatabaseAccess = New ListingDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

				m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))
				m_SonstigesSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_SONSTIGES_SETTING, m_InitializationData.MDData.MDNr)

				Reset()

				m_OriginalCustomerID = m_InitializationData.UserData.UserMDGuid


			Catch ex As Exception


			End Try

			TranslateControls()

		End Sub


		''' <summary>
		''' scan Drop-In view data (tbl_ScanDropIn).
		''' </summary>
		Private Class ScanDropInViewData

			Public Property ID As Integer
			Public Property Customer_ID As String
			Public Property BusinessBranch As String
			Public Property ModulNumber As Integer?
			Public Property DocumentCategoryNumber As Integer?
			Public Property ScanContent As Byte()
			Public Property FileExtension As String
			Public Property CreatedOn As DateTime?
			Public Property CreatedFrom As String
			Public Property CheckedOn As DateTime?
			Public Property CheckedFrom As String

			Public Enum ScannModulEnum
				Employee
				Customer
				Employment
				Report
				Invoice
				Payroll
				NotDefined
			End Enum



		End Class


#End Region







	End Class



End Namespace
