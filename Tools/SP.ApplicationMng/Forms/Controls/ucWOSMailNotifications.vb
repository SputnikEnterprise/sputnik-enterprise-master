
Imports SP.DatabaseAccess.ScanJob
Imports SP.DatabaseAccess.EMailJob
Imports SP.DatabaseAccess.EMailJob.DataObjects
Imports SP.DatabaseAccess.WOS
Imports SP.DatabaseAccess.WOS.DataObjects

Imports SP.Infrastructure.Logging
Imports System.ComponentModel
Imports SPProgUtility.Mandanten
Imports SP.Infrastructure.UI
Imports SPProgUtility.CommonXmlUtility
Imports System.IO
Imports SP.ApplicationMng.ChilKatUtility
Imports SP.ApplicationMng.CVLizer.DataObject

Namespace UI

	Public Class ucWOSMailNotifications


#Region "private consts"
		Private Delegate Sub StartLogingData(msg As String)

		Private Const FTP_BASE_ADDRESS As String = "ftp://scan.domain.com/"
		Private Const LUBAG_FTP_ADDRESS As String = FTP_BASE_ADDRESS + "path"
		Private Const LUBAG_FTP_FOLDER As String = "path"
		'ftp://scan.domain.com/path

		Private Const FTP_USER_NAME As String = "username"
		Private Const FTP_USER_PASSWORD As String = "password"

		Private Const TIMER_INTERVALL As Integer = 60000

		Private Const REPORT_SCAN_EMAIL_USER As String = "mailaddress@domain.com"
		Private Const REPORT_SCAN_EMAIL_PASSWORD As String = "password"

		Private Const CHILKAT_COMPONENT_CODE As String = "yourserialnumber"
		Private Const OUR_EXCHANGE_SERVER As String = "mail.domain.com"

#End Region


#Region "private fields"

		''' <summary>
		''' Boolean flag indicating if form is initializing.
		''' </summary>
		Protected m_SuppressUIEvents As Boolean = False

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		''' <summary>
		''' The translation value helper.
		''' </summary>
		Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper


		''' <summary>
		''' The common data access object.
		''' </summary>
		Private m_EMailDatabaseAccess As IEMailJobDatabaseAccess
		Private m_ScanDatabaseAccess As IScanJobDatabaseAccess
		Private m_WOSDatabaseAccess As IWOSDatabaseAccess

		Private m_customerID As String
		Private m_EMailID As Integer?

		''' <summary>
		''' Settings xml.
		''' </summary>
		Private m_MandantSettingsXml As SettingsXml

		''' <summary>
		''' List of user controls.
		''' </summary>
		Private m_connStr_Application As String
		Private m_connStr_Scanjobs As String
		Private m_connStr_Email As String
		Private m_connStr_Systeminfo As String
		Private m_connStr_WOS As String
		Private m_connStr_SPPublicData As String

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI

		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As SP.Infrastructure.Utility

		Private m_mandant As Mandant

		Private m_ExitApplication As Boolean
		Private m_Timer As System.Timers.Timer
		Private m_IsProcessing As Boolean

		Private m_EMailData As BindingList(Of EMailData)
		Private m_CurrentEMailViewData As EMailData
		Private m_EMailAttachmentData As List(Of EMailAttachment)
		Private m_CurrentAttachmentViewData As EMailAttachment

		Private m_EMailUtility As EMailUtility
		Private m_SettingFile As ProgramSettings
		Private m_LogData As List(Of EntryLOGData)
		Private m_NotAllowedEMailID As List(Of Integer)

		'Delegate Sub dlgTimerEnable(ByVal enable As Boolean)

		''the delegate should match the method signature
		'Private Sub TimerEnable(ByVal enable As Boolean)
		'	m_Timer.Enabled = enable
		'End Sub

#End Region



#Region "constructor"

		Public Sub New(ByVal settingFile As ProgramSettings)

			' Dieser Aufruf ist für den Designer erforderlich.
			DevExpress.UserSkins.BonusSkins.Register()
			DevExpress.Skins.SkinManager.EnableFormSkins()

			m_UtilityUI = New UtilityUI
			m_Utility = New SP.Infrastructure.Utility

			m_SettingFile = settingFile

			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
			m_connStr_Application = m_SettingFile.ConnstringApplication
			m_connStr_Systeminfo = m_SettingFile.ConnstringSysteminfo
			m_connStr_Scanjobs = m_SettingFile.ConnstringScanjobs
			m_connStr_Email = m_SettingFile.ConnstringEMail
			m_connStr_WOS = m_SettingFile.ConnstringWOS
			m_connStr_SPPublicData = m_SettingFile.ConnstringSPPublicData

			m_EMailDatabaseAccess = New EMailJobDatabaseAccess(m_connStr_Email, "DE")
			m_ScanDatabaseAccess = New ScanJobDatabaseAccess(m_connStr_Scanjobs, "DE")
			m_WOSDatabaseAccess = New EMailJobDatabaseAccess(m_connStr_WOS, "DE")


			m_ExitApplication = True
			m_IsProcessing = False
			m_EMailUtility = New EMailUtility(settingFile)
			m_EMailUtility.CurrentMailBox = settingFile.ReportMailbox

			Reset()

			m_LogData = New List(Of EntryLOGData)
			m_LogData.Add(New EntryLOGData With {.LogDate = Now, .LogType = "Info", .Message = "LOG started..."})

			m_Timer = New System.Timers.Timer(TIMER_INTERVALL)
			' TODO
			'If m_SettingFile.Notificationintervalperiodeforreport Then
			AddHandler m_Timer.Elapsed, AddressOf RunTimer
			'End If

			m_Timer.Enabled = True

		End Sub

#End Region


#Region "public methodes"

		Public Sub CleanUp()
			Dim success As Boolean = True

			If Not m_Timer Is Nothing Then m_Timer.Stop()
			success = success AndAlso Not m_EMailUtility Is Nothing AndAlso m_EMailUtility.CleanUp()

		End Sub

#End Region


		Private Sub Reset()

			m_ExitApplication = False
			m_NotAllowedEMailID = New List(Of Integer)
			ResetLOGGrid()

			If Not m_EMailUtility.PrepareChilKatLogin Then
				btnLoadEMails.Enabled = False
				m_Timer.Stop()
			End If

		End Sub


		''' <summary>
		''' Resets LOG grid.
		''' </summary>
		Private Sub ResetLOGGrid()

			gvLOG.OptionsView.ShowIndicator = False
			gvLOG.OptionsView.ShowAutoFilterRow = True
			gvLOG.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvLOG.OptionsView.ShowFooter = False
			gvLOG.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False

			gvLOG.Columns.Clear()

			Dim columnCustomer_ID As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomer_ID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCustomer_ID.OptionsColumn.AllowEdit = False
			columnCustomer_ID.Caption = ("Datum")
			columnCustomer_ID.Name = "LogDate"
			columnCustomer_ID.FieldName = "LogDate"
			columnCustomer_ID.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
			columnCustomer_ID.DisplayFormat.FormatString = "G"
			columnCustomer_ID.Width = 30
			columnCustomer_ID.Visible = True
			gvLOG.Columns.Add(columnCustomer_ID)

			Dim columnLogType As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLogType.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnLogType.OptionsColumn.AllowEdit = False
			columnLogType.Caption = ("Art")
			columnLogType.Name = "LogType"
			columnLogType.FieldName = "LogType"
			columnLogType.Visible = False
			columnLogType.Width = 50
			gvLOG.Columns.Add(columnLogType)

			Dim columnMessage As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMessage.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnMessage.OptionsColumn.AllowEdit = False
			columnMessage.Caption = ("Nachricht")
			columnMessage.Name = "Message"
			columnMessage.FieldName = "Message"
			columnMessage.Visible = True
			gvLOG.Columns.Add(columnMessage)


			grdLOG.DataSource = Nothing

		End Sub


#Region "private properties"

		' ''' <summary>
		' ''' Gets the selected email data.
		' ''' </summary>
		'Private ReadOnly Property SelectEMailViewData As EMailData
		'	Get
		'		Dim grdView = TryCast(grdEMail.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

		'		If Not (grdView Is Nothing) Then

		'			Dim selectedRows = grdView.GetSelectedRows()

		'			If (selectedRows.Count > 0) Then
		'				Dim viewData = CType(grdView.GetRow(selectedRows(0)), EMailData)
		'				Return viewData
		'			End If

		'		End If

		'		Return Nothing
		'	End Get

		'End Property

		'''' <summary>
		'''' Gets the selected email attachment data.
		'''' </summary>
		'Private ReadOnly Property SelectedEMailAttachmentViewData As EMailAttachment
		'	Get
		'		Dim lstView = TryCast(lstInfo.SelectedItem, EMailAttachment)


		'		Return lstView
		'	End Get

		'End Property


#End Region


#Region "Form events"

		'Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

		'	e.Cancel = Not m_ExitApplication

		'	If Not e.Cancel Then

		'	Else
		'		'Me.WindowState = FormWindowState.Minimized
		'	End If

		'End Sub

#End Region

		'Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
		'	m_ExitApplication = True
		'	Me.Close()
		'End Sub

		Private Sub OnbtnLoadEMailsClick(sender As Object, e As EventArgs) Handles btnLoadEMails.Click

			LoadEMailData()

		End Sub

		Private Sub RunTimer(sender As Object, e As EventArgs)
			Try

				If Not m_IsProcessing Then
					LoadEMailData()
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				AddLogData(String.Format("RunTimer was with error: {0}", ex.ToString))

			End Try
		End Sub

		Private Function LoadEMailData() As Boolean
			Dim success As Boolean = True

			Try
				If Not m_IsProcessing Then

					BackgroundWorker1.WorkerSupportsCancellation = True
					BackgroundWorker1.WorkerReportsProgress = True
					BackgroundWorker1.RunWorkerAsync()    ' Multithreading starten

				End If


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				AddLogData(String.Format("LoadEMailData was with error: {0}", ex.ToString))
				success = False

			Finally
				'm_IsProcessing = False

			End Try

			Return success

		End Function

		Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
			Dim bw As System.ComponentModel.BackgroundWorker = DirectCast(sender, System.ComponentModel.BackgroundWorker)

			CheckForIllegalCrossThreadCalls = False

			m_IsProcessing = True
			m_Timer.Stop()
			m_Timer.Enabled = False
			m_EMailData = Nothing

			Dim success = PerformLoadingEMailData()

			m_SuppressUIEvents = True

			success = success AndAlso AutomatedUploadProcess()

			e.Result = True
			If bw.CancellationPending Then e.Cancel = True

		End Sub

		Private Sub BackgroundWorker1_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
			Trace.WriteLine(e.ToString)
		End Sub

		Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted

			If (e.Error IsNot Nothing) Then
				m_Logger.LogError(e.Error.ToString)
				AddLogData(String.Format("job was finieshed with error: {0}", e.Error.ToString))
			Else
				If e.Cancelled = True Then
					m_Logger.LogError("Der Vorgang wurde abgebrochen.")
					AddLogData(String.Format("job was cancelled!"))

				Else
					BackgroundWorker1.CancelAsync()

					Try
						AddLogData(String.Format("email processing is finished"))

						'm_Timer.Enabled = True
						'm_Timer.Start()
						m_SuppressUIEvents = False

					Catch ex As Exception
						'm_Timer.Enabled = True
						'm_Timer.Start()
						m_Logger.LogError(ex.ToString)
						AddLogData(String.Format("job was finieshed with error: {0}", ex.ToString))

					End Try

				End If
			End If

			m_Timer.Enabled = True
			m_Timer.Start()
			m_IsProcessing = False

		End Sub

		''' <summary>
		'''  Performs loading email data.
		''' </summary>
		Private Function PerformLoadingEMailData() As Boolean
			Dim success As Boolean = True
			Dim listDataSource As BindingList(Of EMailData) = New BindingList(Of EMailData)

			m_Logger.LogDebug("loading report email data!")
			success = success AndAlso m_EMailUtility.PrepareChilKatLogin()
			success = success AndAlso m_EMailUtility.PrepareChilKatReportLogin()
			If Not success Then
				Dim msg = String.Format("loging to childat component for Report WAS NOT successfull.")
				m_Logger.LogError(msg)
				AddLogData(msg)
				Return False
			End If

			Dim listOfData = m_EMailUtility.LoadIMAPMails()
			If listOfData Is Nothing Then
				m_Logger.LogError("no report mail data was founded!")
				AddLogData(String.Format("no report email data was founded!"))

				Return False
			End If

			Dim gridData = (From person In listOfData
											Select New EMailData With {.EMailUidl = person.EMailUidl,
																							 .EMailAttachment = person.EMailAttachment,
																							 .EMailBody = person.EMailBody,
																							 .EMailFrom = person.EMailFrom,
																							 .EMailSubject = person.EMailSubject,
																							 .EMailTo = person.EMailTo,
																							 .EMailDate = person.EMailDate,
																							 .EMailPlainTextBody = person.EMailPlainTextBody,
																							 .HasHtmlBody = person.HasHtmlBody
																							}).ToList()

			For Each p In gridData
				listDataSource.Add(p)
				If Not m_EMailID.HasValue Then m_EMailID = p.EMailUidl
			Next

			m_EMailData = listDataSource

			m_Logger.LogDebug("loading email data finishing!")


			Return Not (listDataSource Is Nothing)

		End Function

		Private Function ReloadEMailData() As Boolean
			Dim success As Boolean = True

			success = success AndAlso PerformLoadingEMailData()
			'FocusEMail(m_EMailID)


			Return (Not m_EMailData Is Nothing)

		End Function

		Private Function AutomatedUploadProcess() As Boolean
			Dim success As Boolean = True
			Dim continueProcess As Boolean = True

			If m_EMailData Is Nothing OrElse m_EMailData.Count = 0 Then Return False

			m_Logger.LogDebug(String.Format("automated loading WOS parsing started! >>> founded emails: {0}", m_EMailData.Count))
			For Each email In m_EMailData
				continueProcess = True
				m_CurrentEMailViewData = email

				m_Logger.LogInfo(String.Format("{0}: email is processing: {1}: {2}", Now, email.EMailUidl, email.EMailFrom))

				If m_NotAllowedEMailID.Contains(email.EMailUidl) Then continueProcess = False
				If continueProcess Then
					AddLogData(String.Format("email is processing: {0} >>> {1}", email.EMailFrom, email.EMailTo))

					Dim userData = m_EMailDatabaseAccess.LoadEMailUserData(email)
					If userData Is Nothing Then
						AddLogData(String.Format(">>> user could was not founded: {0} ->>> {1}", email.EMailFrom, email.EMailTo))
						m_Logger.LogError("user-setting is not founded!")
						m_NotAllowedEMailID.Add(Val(email.EMailUidl))

						Continue For
					End If
					If Not String.IsNullOrWhiteSpace(email.EMailSubject) Then
						If email.EMailSubject.ToLower.Contains("Customer_ID".ToLower) Then
							Dim subject As String() = email.EMailSubject.Split(CChar(":"))
							If subject.Count > 0 Then userData.Customer_ID = subject(1).Trim
						End If
					End If

					Dim settingData = m_EMailDatabaseAccess.LoadEMailSettingForParsingData(userData.Customer_ID, EMailSettingData.UploadEnum.ReportUpload)
					If settingData Is Nothing OrElse String.IsNullOrWhiteSpace(settingData.Customer_ID) Then
						AddLogData(String.Format(">>> customerid could was not founded: {0} >>> {1}", userData.Customer_ID, email.EMailFrom))
						m_Logger.LogError("customer-setting is not founded!")
						m_NotAllowedEMailID.Add(Val(email.EMailUidl))

						Continue For
					End If
#If DEBUG Then
					'settingData.Customer_ID = "EFFE4D79-2AC6-4f9f-839F-1E4E9D6D9E2A"
#End If
					email.Customer_ID = settingData.Customer_ID
					settingData.UploadForWhat = EMailSettingData.UploadEnum.ReportUpload
					AddLogData(String.Format(">>> customerid: {0}", settingData.Customer_ID))


					m_EMailUtility.m_PatternData = m_EMailDatabaseAccess.LoadEMailPatternParsingData(userData.Customer_ID)
					m_EMailUtility.CustomerID = settingData.Customer_ID
					m_EMailUtility.AssignedUserID = userData.User_ID
					m_EMailUtility.CurrentEMailData = email
					m_EMailUtility.CurrentEMailAttachmentData = email.EMailAttachment
					email.Customer_ID = settingData.Customer_ID
					m_customerID = settingData.Customer_ID
					settingData.UploadForWhat = EMailSettingData.UploadEnum.ReportUpload

					AddLogData(String.Format("customerid: {0} >>> email {1} ->>> {2} will be parsed...", m_customerID, email.EMailFrom, email.EMailTo))

					Dim parseResult As ParsResult = m_EMailUtility.ParsReceivedEMailBody(email)
					If Not parseResult.ParseValue AndAlso parseResult.ParseMessage.ToLower = "not defined" Then
						' it was not from homepage!
						m_Logger.LogDebug(String.Format(">>> email is not from homepage!"))
						AddLogData(String.Format(">>> email is not from homepage!"))
					End If

					For Each attachment In email.EMailAttachment
						AddLogData(String.Format(">>> filename: {0}", attachment.AttachmentName))
						success = success AndAlso CopyEMailAttachmentInDirectory(settingData, attachment)
					Next

					Dim result = success AndAlso m_EMailUtility.DisconnectFTPHost
					' report mails should not be saved!
					'If parseResult.ParseMessage.ToLower <> "not defined" Then success = success AndAlso SaveReceivedEMailData()
					If success Then
						AddLogData(String.Format(">>> email is saved: {0}", success))

						success = success AndAlso m_EMailUtility.DeleteAssignedIMAPEMail(email.EMailUidl)
						AddLogData(String.Format(">>> email is from host server deleted: {0}", success))
					End If

				End If

				If Not success Then
					AddLogData(String.Format(">>> email is ignored!"))
					success = True
				End If

			Next
			success = success AndAlso ReloadEMailData()
			'AddLogData(String.Format(">>> emails are reloaded: {0}", success))

			m_Logger.LogDebug("automated loading finished!")


			Return success

		End Function

		Private Function CopyEMailAttachmentInDirectory(ByVal settingData As EMailSettingData, ByVal attachment As EMailAttachment) As Boolean
			Dim success As Boolean = True

			If attachment Is Nothing Then
				m_Logger.LogError("EMail-Attachment konnte nicht geladen werden.")
				AddLogData(String.Format(">>> attachment could not be loaded!"))
				Return False
			End If
			m_CurrentAttachmentViewData = attachment

			Dim bytes() = attachment.AttachmentSize
			Dim fileName As String = Path.GetTempFileName
			fileName = Path.ChangeExtension(fileName, "PDF")
			Dim fs As New IO.FileStream(fileName, IO.FileMode.Create)
			fs.Write(bytes, 0, CInt(bytes.Length))

			fs.Close()

			success = success AndAlso File.Exists(fileName)

			success = success AndAlso m_EMailUtility.UploadFileToFTP(fileName, settingData)
			File.Delete(fileName)

			Return success

		End Function

		Private Function SaveReceivedEMailData() As Boolean
			Dim success As Boolean = True

			If m_EMailData Is Nothing OrElse m_CurrentEMailViewData Is Nothing Then
				m_Logger.LogError("not mail was selected!")
				AddLogData(String.Format(">>> no email data could not be loaded for save!"))
				Return False
			End If

			success = success AndAlso m_EMailDatabaseAccess.AddEMailJob(m_CurrentEMailViewData, Nothing)
			m_Logger.LogDebug("email data stored into db!")

			For Each attachment In m_CurrentEMailViewData.EMailAttachment
				success = success AndAlso m_CurrentEMailViewData.ID.HasValue AndAlso m_EMailDatabaseAccess.AddEMailAttachmentJob(m_CurrentEMailViewData.ID, attachment)
			Next
			m_Logger.LogDebug("email attachment data stored into db!")
			AddLogData(String.Format(">>> email data successfully saved!"))


			Return success

		End Function


#Region "Helpers"

		Private Sub AddLogData(ByVal msg As String)
			If String.IsNullOrWhiteSpace(msg) Then Return

			If Me.InvokeRequired = True Then
				Dim d As New StartLogingData(AddressOf AddLogData)
				Me.Invoke(d, New Object() {msg})

				'Me.Invoke(New StartLogingData(AddressOf AddLogData))
			Else

				Dim existData As List(Of EntryLOGData) = CType(grdLOG.DataSource, List(Of EntryLOGData))
				If existData Is Nothing Then
					existData = New List(Of EntryLOGData)
					existData = m_LogData

				Else
					If existData.Count > 50 Then
						grdLOG.DataSource = Nothing
						m_LogData = New List(Of EntryLOGData)
						m_LogData.Add(New EntryLOGData With {.LogDate = Now, .LogType = "Info", .Message = "restarting log..."})
						existData = m_LogData

					End If
				End If

				existData.Add(New EntryLOGData With {.LogDate = Now, .LogType = "DIVEMail", .Message = msg})
				grdLOG.DataSource = existData
				grdLOG.RefreshDataSource()

				lblRecCount.Text = String.Format("Anzahl Datensätze: {0}", gvLOG.RowCount)
			End If

		End Sub

		'Private Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

		'	Dim m_md As New SPProgUtility.Mandanten.Mandant
		'	Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
		'	Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
		'	Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

		'	Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
		'	Dim translate = clsTransalation.GetTranslationInObject

		'	Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

		'End Function

#End Region


	End Class


End Namespace
