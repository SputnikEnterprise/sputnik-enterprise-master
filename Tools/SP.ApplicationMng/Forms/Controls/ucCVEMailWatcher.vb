
Imports SP.ApplicationMng.ChilKatUtility
Imports SP.ApplicationMng.CVLizer.DataObject
Imports SP.DatabaseAccess.Applicant
Imports SP.DatabaseAccess.CVLizer
Imports SP.DatabaseAccess.CVLizer.DataObjects
Imports SP.DatabaseAccess.EMailJob
Imports SP.DatabaseAccess.EMailJob.DataObjects
Imports SP.DatabaseAccess.ScanJob
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports System.ComponentModel
Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Reflection
Imports System.Threading

Namespace UI

	Public Class ucCVEMailWatcher




#Region "private consts"

		Private Delegate Sub StartLogingData(msg As String)

		Private Const TIMER_INTERVALL As Integer = 60000

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
		''' The common data access object.
		''' </summary>
		Private m_EMailDatabaseAccess As IEMailJobDatabaseAccess
		Private m_ScanDatabaseAccess As IScanJobDatabaseAccess

		''' <summary>
		''' The common data access object.
		''' </summary>
		Private m_AppDatabaseAccess As IAppDatabaseAccess

		''' <summary>
		''' The cv data access object.
		''' </summary>
		Private m_CVLDatabaseAccess As ICVLizerDatabaseAccess

		Private m_customerID As String
		Private m_EMailID As Integer?

		''' <summary>
		''' connections
		''' </summary>
		Private m_connStr_Application As String
		Private m_connStr_CVlizer As String
		Private m_connStr_Systeminfo As String
		Private m_connStr_Scanjobs As String
		Private m_connStr_Email As String

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI

		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As SP.Infrastructure.Utility

		Private m_ExitApplication As Boolean
		Private m_Timer As System.Timers.Timer
		Private m_IsProcessing As Boolean

		Private m_EMailData As BindingList(Of EMailData)
		Private m_CurrentEMailViewData As EMailData

		Private m_EMailUtility As EMailUtility
		Private m_firstLoginCall As Boolean
		Private m_SettingFile As ProgramSettings

		Private m_CVLizerXMLData As CVLizerXMLData
		Private m_LogData As List(Of EntryLOGData)
		Private m_NotAllowedEMailID As List(Of Integer)

		Private m_CVLParsingResultDescription As String

#End Region



#Region "constructor"

		Public Sub New(ByVal settingFile As ProgramSettings)

			' Dieser Aufruf ist für den Designer erforderlich.
			DevExpress.UserSkins.BonusSkins.Register()
			DevExpress.Skins.SkinManager.EnableFormSkins()

			m_SettingFile = settingFile

			m_UtilityUI = New UtilityUI
			m_Utility = New SP.Infrastructure.Utility

			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
			m_connStr_Application = m_SettingFile.ConnstringApplication
			m_connStr_CVlizer = m_SettingFile.ConnstringCVLizer
			m_connStr_Systeminfo = m_SettingFile.ConnstringSysteminfo
			m_connStr_Scanjobs = m_SettingFile.ConnstringScanjobs
			m_connStr_Email = m_SettingFile.ConnstringEMail

			m_EMailDatabaseAccess = New EMailJobDatabaseAccess(m_connStr_Email, "DE")
			m_ScanDatabaseAccess = New ScanJobDatabaseAccess(m_connStr_Scanjobs, "DE")
			m_AppDatabaseAccess = New AppDatabaseAccess(m_connStr_Application, "DE")
			m_CVLDatabaseAccess = New CVLizerDatabaseAccess(m_connStr_CVlizer, "DE")


			m_ExitApplication = True
			m_IsProcessing = False
			m_EMailUtility = New EMailUtility(settingFile)
			m_EMailUtility.CurrentMailBox = settingFile.CVMailbox

			Reset()


			m_LogData = New List(Of EntryLOGData)
			m_LogData.Add(New EntryLOGData With {.LogDate = Now, .LogType = "Info", .Message = "LOG started..."})

			m_Timer = New System.Timers.Timer(TIMER_INTERVALL)
			AddHandler m_Timer.Elapsed, AddressOf RunTimer

			m_Timer.Enabled = True

		End Sub

#End Region


#Region "Public Methods"

		Public Function LoadCVEMailData() As Boolean
			Dim success As Boolean = True

			success = success AndAlso LoadEMailData()


			Return success

		End Function

		Public Sub CleanUp()
			Dim success As Boolean = True

			If Not m_Timer Is Nothing Then m_Timer.Stop()
			success = success AndAlso Not m_EMailUtility Is Nothing AndAlso m_EMailUtility.CleanUp()

		End Sub


#End Region


		Private Sub Reset()

			m_ExitApplication = False
			m_NotAllowedEMailID = New List(Of Integer)

			lblWatchPostbox.Text = m_EMailUtility.CurrentMailBox
			lblRecCount.Text = String.Empty

			ResetEMailDetail()
			ResetLOGGrid()

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


		Private Sub ResetEMailDetail()

		End Sub



#Region "private properties"



#End Region


		Private Sub OnbtnLoadEMailsClick(sender As Object, e As EventArgs) Handles btnLoadEMails.Click

			LoadEMailData()

		End Sub

		Private Sub RunTimer(sender As Object, e As EventArgs)
			Try

				If Not m_IsProcessing Then
					LoadEMailData()
				End If

			Catch ex As Exception
				m_IsProcessing = False
				m_Logger.LogError(ex.ToString)

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
			Try
				success = success AndAlso AutomatedUploadProcess()
			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			End Try


			e.Result = True
			If bw.CancellationPending Then e.Cancel = True

		End Sub

		Private Sub BackgroundWorker1_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
			Trace.WriteLine(String.Format("{0} >>> {1}", "BackgroundWorker1_ProgressChanged", e.ToString))
		End Sub

		Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted

			If (e.Error IsNot Nothing) Then
				m_Logger.LogError(e.Error.ToString)
				AddLogData(String.Format("{0} job was finieshed with error.", e.Error.ToString))
				btnLoadEMails.Enabled = True

			Else
				If e.Cancelled = True Then
					m_Logger.LogError("Der Vorgang wurde abgebrochen.")
					AddLogData(String.Format("job was cancelled!"))
					btnLoadEMails.Enabled = True

				Else
					BackgroundWorker1.CancelAsync()

					Try
						AddLogData(String.Format("email processing is finished"))
						btnLoadEMails.Enabled = True

						m_SuppressUIEvents = False

					Catch ex As Exception
						btnLoadEMails.Enabled = True
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

			''m_Logger.LogDebug("loading cv email data!")
			'Dim s As String = Assembly.GetExecutingAssembly().CodeBase
			'Trace.WriteLine("CodeBase: [" + s + "]")
			's = (New Uri(s)).AbsolutePath
			'Trace.WriteLine("AbsolutePath: [" + s + "]")
			's = Uri.UnescapeDataString(s)
			'Trace.WriteLine("Unescaped: [" + s + "]")
			's = Path.GetFullPath(s)
			'Trace.WriteLine("FullPath: [" + s + "]")


			success = success AndAlso m_EMailUtility.PrepareChilKatLogin()
			success = success AndAlso m_EMailUtility.PrepareChilKatCVLogin()
			If Not success Then
				AddLogData(String.Format("loging to childat component for cv WAS NOT successfull."))
				Return False
			End If

			'Dim mailboxs = m_EMailUtility.ListIMAPMailboxes()
			Dim listOfData = m_EMailUtility.LoadIMAPMails()
			If listOfData Is Nothing Then
				AddLogData(String.Format("no cv email data was founded!"))
				m_Logger.LogError("no cv mail data was founded!")

				m_firstLoginCall = False
				Return False
			End If

			Dim gridData = (From person In listOfData
							Select New EMailData With {.EMailUidl = person.EMailUidl,
																					 .ID = person.ID,
																					 .Customer_ID = person.Customer_ID,
																					 .CreatedFrom = person.CreatedFrom,
																					 .CreatedOn = person.CreatedOn,
																					 .EMailAttachment = person.EMailAttachment,
																					 .EMailBody = person.EMailBody,
																					 .EMailFrom = person.EMailFrom,
																					 .EMailSubject = person.EMailSubject,
																					 .EMailTo = person.EMailTo,
																					 .EMailDate = person.EMailDate,
																					 .EMailPlainTextBody = person.EMailPlainTextBody,
																					 .EMailMime = person.EMailMime,
																					 .HasHtmlBody = person.HasHtmlBody
																					}).ToList()

			'Dim MailIsAllowed As Boolean = True
			For Each p In gridData
				'For Each a In p.EMailAttachment
				'	'If a.AttachmentSize > 0 Then MailIsAllowed = True
				'Next
				listDataSource.Add(p)
				If Not m_EMailID.HasValue Then m_EMailID = p.EMailUidl
			Next

			m_EMailData = listDataSource

			m_Logger.LogDebug("loading cv-email data finishing!")


			Return Not (listDataSource Is Nothing)

		End Function

		Private Function ReloadEMailData() As Boolean
			Dim success As Boolean = True

			success = success AndAlso PerformLoadingEMailData()
			FocusEMail(m_EMailID)


			Return (Not m_EMailData Is Nothing)

		End Function

		''' <summary>
		''' Focuses an email.
		''' </summary>
		Private Sub FocusEMail(ByVal emailID As Integer?)

		End Sub

		Private Function AutomatedUploadProcess() As Boolean
			Dim success As Boolean = True
			Dim continueProcess As Boolean = True
			Dim emailFileNameforCVL As String = String.Empty
			Dim performMovingMail As Boolean = True

			If m_EMailData Is Nothing OrElse m_EMailData.Count = 0 Then Return False
			m_Logger.LogDebug(String.Format("automated loading cv parsing started! >>> founded emails: {0}", m_EMailData.Count))
			btnLoadEMails.Enabled = False

			For Each email In m_EMailData

				Try
					m_CurrentEMailViewData = email
					continueProcess = Not m_NotAllowedEMailID.Contains(email.EMailUidl)

					AddLogData(String.Format("EMailUidl: {0} is processing: {1} >>> {2}", email.EMailUidl, email.EMailFrom, email.EMailTo))
					m_Logger.LogDebug(String.Format("EMailUidl: {0} is processing: {1} >>> {2}", email.EMailUidl, email.EMailFrom, email.EMailTo))
					If Not continueProcess Then
						m_Logger.LogWarning(String.Format("EMailUidl: {0} will be ignored for processing because is content of m_NotAllowedEMailID: {1} >>> {2}", email.EMailUidl, email.EMailFrom, email.EMailTo))

						Throw New Exception(String.Format("jumping mail {0}: content was in black list!", email.EMailUidl))
					End If

					Dim userData = m_EMailDatabaseAccess.LoadEMailUserData(email)
					If userData Is Nothing Then
						AddLogData(String.Format(">>> userID was not founded: {0} ->>> {1}", email.EMailFrom, email.EMailTo))
						m_Logger.LogError(String.Format(">>> jumping because userID was not founded: {0} ->>> {1}", email.EMailFrom, email.EMailTo))

						Throw New Exception(String.Format("jumping mail {0}: userID was not founded", email.EMailUidl))
					End If
					m_Logger.LogInfo(String.Format("user was founded: {0} >>> {1}", userData.UserFullname, userData.User_ID))

					Dim settingData = m_EMailDatabaseAccess.LoadEMailSettingForParsingData(userData.Customer_ID, EMailSettingData.UploadEnum.CVUpload)
					If settingData Is Nothing OrElse String.IsNullOrWhiteSpace(settingData.Customer_ID) Then
						AddLogData(String.Format(">>> customerid was not founded: {0} >>> {1}", settingData.Customer_ID, email.EMailFrom))
						m_Logger.LogError(String.Format(">>> customerid was not founded: {0} >>> {1}", settingData.Customer_ID, email.EMailFrom))

						Throw New Exception(String.Format("jumping mail {0}: customerid was not founded!", email.EMailUidl))
					End If
					m_Logger.LogInfo(String.Format("settingdata is loaded: {0} >>> {1}", userData.UserFullname, userData.User_ID))

					m_customerID = settingData.Customer_ID
					email.Customer_ID = settingData.Customer_ID
#If DEBUG Then
					'settingData.Customer_ID = "EFFE4D79-2AC6-4f9f-839F-1E4E9D6D9E2A"
#End If

					If m_NotAllowedEMailID.Contains(email.EMailUidl) Then continueProcess = False
					Dim duplicatedMail As Boolean = False
					If continueProcess Then duplicatedMail = ExistsReceivedEMailData(True)
					continueProcess = continueProcess AndAlso (Not duplicatedMail)

					If Not continueProcess Then
						If Not m_NotAllowedEMailID.Contains(email.EMailUidl) Then m_NotAllowedEMailID.Add(Val(email.EMailUidl))
						AddLogData(String.Format("EMailUidl: {0} continueProcess was not successfull. customerid: {1} >>> {2}", email.EMailUidl, settingData.Customer_ID, email.EMailFrom))
						m_Logger.LogWarning(String.Format("EMailUidl: {0} continueProcess was not successfull. customerid: {1} >>> {2}", email.EMailUidl, settingData.Customer_ID, email.EMailFrom))

						Throw New Exception(String.Format("jumping mail {0}: continueProcess was not successfull!", email.EMailUidl))
					End If

					m_EMailUtility.CustomerID = m_customerID
					m_EMailUtility.AssignedUserID = userData.User_ID
					m_EMailUtility.m_PatternData = m_EMailDatabaseAccess.LoadEMailPatternParsingData(m_customerID)
					If m_EMailUtility.m_PatternData Is Nothing OrElse String.IsNullOrWhiteSpace(m_EMailUtility.m_PatternData.Customer_ID) Then
						m_Logger.LogWarning(String.Format("pattern data could not be loaded! {0}", m_customerID))

						' because some mails comes from user directly!
						'Continue For
					End If
					m_EMailUtility.CurrentEMailData = email
					m_EMailUtility.CurrentEMailAttachmentData = email.EMailAttachment
					settingData.UploadForWhat = EMailSettingData.UploadEnum.CVUpload

					AddLogData(String.Format("customerid: {0} >>> email {1} ->>> {2} will be parsed...", m_customerID, email.EMailFrom, email.EMailTo))

					Dim parseResult As ParsResult = Nothing
					If success Then
						emailFileNameforCVL = m_EMailUtility.SaveAssignedIMAPEMailToEML(email.EMailUidl)
						email.EMLFilename = emailFileNameforCVL
						parseResult = m_EMailUtility.ParsReceivedEMailBody(email)
					End If

					success = success AndAlso (Not parseResult Is Nothing)


					If Not parseResult Is Nothing AndAlso Not parseResult.ParseValue AndAlso parseResult.ParseMessage.ToLower = "not defined" Then
						' it was not from homepage!
						m_Logger.LogDebug(String.Format("customerid: {0} >>> email {1} ->>> {2} is NOT from homepage!", m_customerID, email.EMailFrom, email.EMailTo))
						AddLogData(String.Format(">>> email is NOT from homepage!"))
						settingData.PriorityModul = EMailSettingData.PriorityModulEnum.CVL

						If Not email.ExistsAttachment Then
							AddLogData(String.Format("email is without attachment and will be removed: {0} >>> {1}", email.EMailFrom, email.EMailTo))
							m_Logger.LogWarning(String.Format("email is without attachment and will be removed: {0} >>> {1}", email.EMailFrom, email.EMailTo))
							success = success AndAlso NotifyEMailsender(email, "Keine Anhänge", "Ihre Mailnachricht war ohne Anhang! Das Mail wurde automatisch gelöscht.", emailFileNameforCVL)

							Throw New Exception(String.Format("jumping mail {0}: email is without attachment!", email.EMailUidl))
						End If

					Else
						m_Logger.LogDebug(String.Format("customerid: {0} >>> email {1} ->>> {2} is from homepage!", m_customerID, email.EMailFrom, email.EMailTo))
						AddLogData(String.Format("customerid: {0} >>> email {1} ->>> {2} is from homepage", m_customerID, email.EMailFrom, email.EMailTo))

					End If


					If success AndAlso m_SettingFile.ParseEMailAttachment Then
						Dim cvFiles As New List(Of String)

						If Not String.IsNullOrWhiteSpace(emailFileNameforCVL) AndAlso File.Exists(emailFileNameforCVL) Then
							cvFiles.Add(emailFileNameforCVL)
							email.EMailContent = m_Utility.LoadFileBytes(emailFileNameforCVL)

							m_Logger.LogDebug(String.Format("email file will be parsed: {0}", emailFileNameforCVL))

						Else
							m_Logger.LogDebug(String.Format("email could not be saved. I go to parse each files!!!"))
							For Each attachment In email.EMailAttachment
								Dim workingPath As String = Path.Combine(m_SettingFile.TemporaryFolder, m_customerID)
								Dim attachmentFullfileName As String = attachment.AttachmentName
								If Not Directory.Exists(workingPath) Then Directory.CreateDirectory(workingPath)

								If Not attachmentFullfileName.ToString.Contains("\") Then
									attachmentFullfileName = Path.Combine(workingPath, attachmentFullfileName)
								End If

								Dim result = m_Utility.WriteFileBytes(attachmentFullfileName, attachment.AttachmentSize)
								If Not String.IsNullOrWhiteSpace(attachmentFullfileName) AndAlso File.Exists(attachmentFullfileName) Then
									cvFiles.Add(attachmentFullfileName)
								End If
							Next
						End If

						Dim parsingWithoutCVL As Boolean = Not parseResult Is Nothing AndAlso parseResult.ParseValue
						If cvFiles.Count > 0 AndAlso email.EMailAttachment.Count > 0 Then
							AddLogData(String.Format(">>> {0} cv files will be parsed...", cvFiles.Count))

							success = success AndAlso ParseCVFileWithCVLizer(cvFiles, userData, parseResult.ApplicantID, parseResult.ApplicationID)
							If success AndAlso Not m_CVLizerXMLData Is Nothing Then
								parsingWithoutCVL = False
								success = success AndAlso UpdateApplicantDataWithCVLData(parseResult.ApplicantID, parseResult.ApplicationID, settingData.PriorityModul)

							Else
								Dim sendNotifyer As Boolean = NotifyEMailsender(email, "CV could not be parsed!",
																				String.Format("Die Mailnachricht von {0} am {1:G} konnte nicht verarbeitet werden!<br>Das Mail wurde automatisch gelöscht.<br>Mitteilung: {2}",
																							  email.EMailFrom, email.EMailDate, m_CVLParsingResultDescription), emailFileNameforCVL)
								m_Logger.LogWarning(String.Format(">>> cv parsing was failed! applicantID: {0} | applicationID: {1} | Sending notification: {2}", parseResult.ApplicantID, parseResult.ApplicationID, sendNotifyer))

								Throw New Exception(String.Format("jumping mail {0}: cv parsing was failed!", email.EMailUidl))

							End If

						Else
							m_Logger.LogDebug(String.Format("EMailUidl: {0} no attachment was founded! m_customerID: {1} | success: {2} >>> EMailFrom: {3} > EMailTo: {4}", email.EMailUidl, m_customerID, success, email.EMailFrom, email.EMailTo))

						End If

						If parsingWithoutCVL AndAlso parseResult.ApplicantID.GetValueOrDefault(0) > 0 AndAlso parseResult.ApplicationID.GetValueOrDefault(0) > 0 Then
							' mail was from homepage but not with CVLizer
							success = success AndAlso AddProfieDataForNoNotValidatedCVLData(parseResult.ApplicantID, parseResult.ApplicationID)
						End If

					Else
						m_Logger.LogDebug(String.Format("EMailUidl: {0} no parsing! m_customerID: {1} | success: {2} >>> EMailFrom: {3} > EMailTo: {4}", email.EMailUidl, m_customerID, success, email.EMailFrom, email.EMailTo))

						If Not m_NotAllowedEMailID.Contains(email.EMailUidl) Then m_NotAllowedEMailID.Add(Val(email.EMailUidl))

					End If

					Try

						If Not duplicatedMail Then
							Dim performMail As Boolean = True


							performMail = performMail AndAlso SaveReceivedEMailData(parseResult.ApplicationID)
								AddLogData(String.Format(">>> EMailUidl: {0} email is saved: {1}", email.EMailUidl, performMail))
								m_Logger.LogDebug(String.Format("EMailUidl: {0} email data was saved. {1}", email.EMailUidl, performMail))

							If Not m_SettingFile.DeleteParsedEMails Then
									performMovingMail = m_EMailUtility.MoveAssignedIMAPEMail(email.EMailUidl)
									AddLogData(String.Format(">>> EMailUidl: {0} moving email data to imap another box was: {1}", email.EMailUidl, performMovingMail))
									m_Logger.LogDebug(String.Format("EMailUidl: {0} moving email data to imap another box was: {1}", email.EMailUidl, performMovingMail))
									If Not performMovingMail Then Throw New Exception(String.Format("EMailUidl: {0} moving not possible!", email.EMailUidl))

								Else
									performMail = m_EMailUtility.DeleteAssignedIMAPEMail(email.EMailUidl)
									AddLogData(String.Format(">>> EMailUidl: {0} deleting email data from imap was: {1}", email.EMailUidl, performMail))
									m_Logger.LogDebug(String.Format("EMailUidl: {0} deleting email data from imap was: {1}", email.EMailUidl, performMail))
									If Not performMail Then Throw New Exception(String.Format("EMailUidl: {0} deleting not possible!", email.EMailUidl))

								End If

							End If

					Catch ex As Exception
						m_Logger.LogError(ex.ToString)

						If Not m_NotAllowedEMailID.Contains(email.EMailUidl) Then m_NotAllowedEMailID.Add(Val(email.EMailUidl))

					End Try

					If Not success Then
						AddLogData(String.Format(">>> EMailUidl: {0} email is ignored!", email.EMailUidl))
						m_Logger.LogWarning(String.Format("EMailUidl: {0} email is ignored! {1} >>> {2}", email.EMailUidl, settingData.Customer_ID, email.EMailFrom))

						If Not m_NotAllowedEMailID.Contains(email.EMailUidl) Then m_NotAllowedEMailID.Add(Val(email.EMailUidl))

						success = True
					Else
						m_Logger.LogWarning(String.Format("EMailUidl: {0} proccess was {1}. {2} >>> {3}", email.EMailUidl, success, settingData.Customer_ID, email.EMailFrom))

					End If


				Catch ex As Exception
					m_Logger.LogError(ex.ToString)
					If Not m_NotAllowedEMailID.Contains(email.EMailUidl) Then m_NotAllowedEMailID.Add(Val(email.EMailUidl))

					Try
						m_Logger.LogWarning(String.Format("EMailUidl: {0} proccess was not successfull and mail will be moved!", email.EMailUidl))
						performMovingMail = m_EMailUtility.MoveAssignedIMAPEMail(email.EMailUidl)
						If performMovingMail Then m_NotAllowedEMailID.Remove(email.EMailUidl)

					Catch moveEx As Exception
						m_Logger.LogError(moveEx.ToString)
					End Try

					Continue For
				End Try

			Next
			If m_NotAllowedEMailID.Count > 0 Then m_Logger.LogWarning(String.Format("m_NotAllowedEMailID has {1} entries!{0}{2}", vbNewLine, m_NotAllowedEMailID.Count, String.Join(", ", m_NotAllowedEMailID)))
			'm_NotAllowedEMailID.Clear()

			Thread.Sleep(New TimeSpan(0, 0, 10))

			success = success AndAlso ReloadEMailData()
			m_Logger.LogDebug("automated loading cv parsing finished!")

			btnLoadEMails.Enabled = True


			Return success

		End Function


		Private Function SendMailToWithExchange(ByVal strFrom As String,
															ByVal strReplyTo As String,
															ByVal strTo As String,
															ByVal strSubject As String,
															ByVal strBody As String,
															ByVal iPriority As Integer,
															ByVal aAttachmentFile As List(Of String)) As Boolean
			Dim obj As SmtpClient = New SmtpClient
			Dim mailmsg As New System.Net.Mail.MailMessage
			Dim result As Boolean = True
			Dim strToAdresses As String() = strTo.Split(New Char() {";"c, ","c, "#"c})
			If String.IsNullOrWhiteSpace(strReplyTo) Then strReplyTo = strFrom

			Try
				With mailmsg
					.IsBodyHtml = True

					.To.Clear()
					.ReplyToList.Clear()

					.From = New MailAddress(strFrom)
					.To.Add(New MailAddress(strToAdresses(0).Trim))
					If strToAdresses.Length > 1 Then
						For i As Integer = 1 To strToAdresses.Length - 1
							If Not String.IsNullOrWhiteSpace(strToAdresses(i).Trim) Then .CC.Add(New MailAddress(strToAdresses(i).Trim))
						Next
					End If

					.Bcc.Add("notification@domain.com")

					.ReplyToList.Add(strReplyTo)
					.Subject = strSubject.Trim()
					.Body = strBody.Trim()

					Select Case iPriority
						Case 0
							.Priority = Net.Mail.MailPriority.Low
						Case 1
							.Priority = Net.Mail.MailPriority.Normal
						Case 2
							.Priority = Net.Mail.MailPriority.High
					End Select
					If Not aAttachmentFile Is Nothing AndAlso aAttachmentFile.Count > 0 Then
						For Each itm In aAttachmentFile ' i As Integer = 0 To aAttachmentFile.Length - 1
							If File.Exists(itm) Then
								.Attachments.Add(New System.Net.Mail.Attachment(itm))
							End If
						Next
					End If

				End With

				Dim strEx_UserName As String = m_SettingFile.SmtpNotificationUser
				Dim strEx_UserPW As String = m_SettingFile.SmtpNotificationPassword
				Dim smtpPort As Integer = Val(m_SettingFile.SmtpNotificationPort)
				Dim smtpUseTLS As Boolean = m_SettingFile.SmtpNotificationUseTLS

				Try
					If strEx_UserName <> String.Empty Then

						Dim mailClient As New System.Net.Mail.SmtpClient(m_SettingFile.SmtpNotificationServer, smtpPort)
						mailClient.Credentials = New NetworkCredential(strEx_UserName, strEx_UserPW)
						mailClient.DeliveryMethod = Net.Mail.SmtpDeliveryMethod.Network
						mailClient.EnableSsl = smtpUseTLS

						mailClient.Send(mailmsg)

					Else
						obj.Host = m_SettingFile.SmtpNotificationServer
						obj.Send(mailmsg)

					End If


				Catch ex As Exception
					Dim msg = String.Format("AN: {0} From: {1} Message: {2}", strTo, strFrom, ex.ToString)
					m_Logger.LogError(String.Format("{0}", msg))
					result = False

				Finally
					obj = Nothing
					mailmsg.Attachments.Dispose()
					mailmsg.Dispose()

				End Try
			Catch ex As Exception
				Dim msg = String.Format("AN: {0} From: {1} Message: {2}", strTo, strFrom, ex.ToString)
				m_Logger.LogError(String.Format("{0}", msg))

				result = False
			End Try

			Return result
		End Function

		'Private Function CopyEMailAttachmentInDirectory(ByVal settingData As EMailSettingData, ByVal attachment As EMailAttachment) As Boolean
		'	Dim success As Boolean = True

		'	If attachment Is Nothing Then
		'		m_Logger.LogError("EMail-Attachment konnte nicht geladen werden.")
		'		AddLogData(String.Format(">>> attachment could not be loaded!"))
		'		Return False
		'	End If
		'	m_CurrentAttachmentViewData = attachment

		'	Dim bytes() = attachment.AttachmentSize
		'	Dim fileName As String = Path.GetTempFileName
		'	fileName = Path.ChangeExtension(fileName, "PDF")
		'	Dim fs As New IO.FileStream(fileName, IO.FileMode.Create)
		'	fs.Write(bytes, 0, CInt(bytes.Length))

		'	fs.Close()

		'	success = success AndAlso File.Exists(fileName)

		'	success = success AndAlso m_EMailUtility.UploadFileToFTP(fileName, settingData)
		'	File.Delete(fileName)

		'	Return success

		'End Function

		Private Function SaveReceivedEMailData(ByVal applicationID As Integer?) As Boolean
			Dim success As Boolean = True

			If m_EMailData Is Nothing OrElse m_CurrentEMailViewData Is Nothing Then
				m_Logger.LogError("not mail was selected!")
				AddLogData(String.Format(">>> no email data could not be loaded for save!"))

				Return False
			End If

			Try
				If m_CurrentEMailViewData.EMailContent Is Nothing Then m_CurrentEMailViewData.EMailContent = m_Utility.LoadFileBytes(m_CurrentEMailViewData.EMLFilename)
				success = success AndAlso m_EMailDatabaseAccess.AddEMailJob(m_CurrentEMailViewData, applicationID)
				m_Logger.LogDebug(String.Format("email data stored into db: {0}", success))
				AddLogData(String.Format(">>> email data successfully saved: {0}", success))

				For Each attachment In m_CurrentEMailViewData.EMailAttachment
					success = success AndAlso m_CurrentEMailViewData.ID.HasValue AndAlso m_EMailDatabaseAccess.AddEMailAttachmentJob(m_CurrentEMailViewData.ID, attachment)
				Next
				m_Logger.LogDebug("email attachment data stored into db!")
				AddLogData(String.Format(">>> email attachments successfully saved!"))

			Catch ex As Exception
				m_Logger.LogDebug(String.Format("error during save email attachment data! {0}", ex.ToString))

				' set flag to true anyway!!!
				success = True
			End Try


			Return success

		End Function

		Private Function ExistsReceivedEMailData(ByVal deleteMailDataIfExists As Boolean) As Boolean
			Dim success As Boolean = True

			If m_EMailData Is Nothing OrElse m_CurrentEMailViewData Is Nothing Then
				m_Logger.LogError("not mail was selected!")
				AddLogData(String.Format(">>> no email data could not be loaded for save!"))

				Return False
			End If

			Dim existingEMailData = m_EMailDatabaseAccess.LoadExistingAssigendEMailData(m_CurrentEMailViewData)
			If existingEMailData Is Nothing Then
				m_Logger.LogError(String.Format("existingEMailData could not be veryfied!{0}", m_CurrentEMailViewData.EMailUidl))
				Return True

			ElseIf existingEMailData.EMailUidl.GetValueOrDefault(0) = 0 Then
				Return False
			End If

			If DateAndTime.DateDiff(DateInterval.Minute, m_CurrentEMailViewData.EMailDate.GetValueOrDefault(Now), existingEMailData.EMailDate.GetValueOrDefault(Now), FirstDayOfWeek.System, FirstWeekOfYear.System) <= 5 Then
				m_Logger.LogWarning(String.Format("email seems to be with error during last deleting or moving data! m_customerID: {0} | existingEMailData.ID: {1} | existingEMailData.EMailUidl: {2} >>> EMailDate: {3}",
												m_customerID, existingEMailData.ID, existingEMailData.EMailUidl.GetValueOrDefault(0), existingEMailData.EMailDate))

				Try
					If deleteMailDataIfExists Then m_EMailUtility.DeleteAssignedIMAPEMail(m_CurrentEMailViewData.EMailUidl.GetValueOrDefault(0))
				Catch ex As Exception
					m_Logger.LogDebug(String.Format("email data was allready parsed and but can not be deleted! m_customerID: {0} | success: {1} >>> EMailFrom: {2} > EMailTo: {3}", m_customerID, success, m_CurrentEMailViewData.EMailFrom, m_CurrentEMailViewData.EMailTo))
				End Try

				Return True
			End If

			m_Logger.LogDebug(String.Format("email data was allready parsed! m_customerID: {0} | success: {1} >>> EMailFrom: {2} > EMailTo: {3}", m_customerID, success, m_CurrentEMailViewData.EMailFrom, m_CurrentEMailViewData.EMailTo))
			Dim msgWarn As String = String.Format("email data was allready parsed!{0}m_customerID: {1}{0}EMailDate: ({2}) {3}{0}EMailSubject: {4}",
													  vbNewLine, m_customerID, m_CurrentEMailViewData.EMailUidl.GetValueOrDefault(0), m_CurrentEMailViewData.EMailDate, m_CurrentEMailViewData.EMailSubject)
			m_Logger.LogWarning(msgWarn)
			AddLogData(String.Format("email data was allready parsed! m_customerID: {0} | success: {1} >>> EMailFrom: {2} > EMailTo: {3}", m_customerID, success, m_CurrentEMailViewData.EMailFrom, m_CurrentEMailViewData.EMailTo))
			Dim emailFileNameforCVL = m_EMailUtility.SaveAssignedIMAPEMailToEML(m_CurrentEMailViewData.EMailUidl.GetValueOrDefault(0))

			Try
				If deleteMailDataIfExists Then m_EMailUtility.DeleteAssignedIMAPEMail(m_CurrentEMailViewData.EMailUidl.GetValueOrDefault(0))

			Catch ex As Exception
				m_Logger.LogDebug(String.Format("email data was allready parsed and but can not be deleted! m_customerID: {0} | success: {1} >>> EMailFrom: {2} > EMailTo: {3}", m_customerID, success, m_CurrentEMailViewData.EMailFrom, m_CurrentEMailViewData.EMailTo))

			End Try

			Dim sendNotifyer As Boolean = NotifyEMailsender(m_CurrentEMailViewData, "Ihr Mail wurde als duplikat erkannt.", String.Format("Ihr Mail wurde als duplikat erkannt! Das Mail wurde automatisch gelöscht.{0}{1}", vbNewLine, msgWarn), emailFileNameforCVL)
			Try
				If sendNotifyer Then File.Delete(emailFileNameforCVL)

			Catch ex As Exception
				m_Logger.LogError(String.Format("emailFileNameforCVL file could not be deleted!{0}", emailFileNameforCVL))
				Return True
			End Try


			Return success

		End Function

		'Private Function ExistsReceivedEMailData(ByVal deleteMailDataIfExists As Boolean) As Boolean
		'	Dim success As Boolean = True

		'	If m_EMailData Is Nothing OrElse m_CurrentEMailViewData Is Nothing Then
		'		m_Logger.LogError("not mail was selected!")
		'		AddLogData(String.Format(">>> no email data could not be loaded for save!"))
		'		Return False
		'	End If

		'	success = success AndAlso m_EMailDatabaseAccess.ExistsAssigendEMailData(m_CurrentEMailViewData)
		'	If success Then
		'		m_Logger.LogDebug(String.Format("email data was allready parsed! m_customerID: {0} | success: {1} >>> EMailFrom: {2} > EMailTo: {3}", m_customerID, success, m_CurrentEMailViewData.EMailFrom, m_CurrentEMailViewData.EMailTo))
		'		Dim msgWarn As String = String.Format("email data was allready parsed!{0}m_customerID: {1}{0}EMailSubject: {2}{0}EMailBody: {3}{0}EMailPlainTextBody: {4}",
		'											  vbNewLine, m_customerID, m_CurrentEMailViewData.EMailSubject, m_CurrentEMailViewData.EMailBody, m_CurrentEMailViewData.EMailPlainTextBody)
		'		m_Logger.LogWarning(msgWarn)
		'		AddLogData(String.Format("email data was allready parsed! m_customerID: {0} | success: {1} >>> EMailFrom: {2} > EMailTo: {3}", m_customerID, success, m_CurrentEMailViewData.EMailFrom, m_CurrentEMailViewData.EMailTo))
		'		Dim emailFileNameforCVL = m_EMailUtility.SaveAssignedIMAPEMailToEML(m_CurrentEMailViewData.EMailUidl)

		'		Try
		'			If deleteMailDataIfExists Then m_EMailUtility.DeleteAssignedIMAPEMail(m_CurrentEMailViewData.EMailUidl)

		'		Catch ex As Exception
		'			m_Logger.LogDebug(String.Format("email data was allready parsed and but can not be deleted! m_customerID: {0} | success: {1} >>> EMailFrom: {2} > EMailTo: {3}", m_customerID, success, m_CurrentEMailViewData.EMailFrom, m_CurrentEMailViewData.EMailTo))

		'		End Try

		'		Dim sendNotifyer As Boolean = NotifyEMailsender(m_CurrentEMailViewData, "Ihr Mail wurde als duplikat erkannt.", String.Format("Ihr Mail wurde als duplikat erkannt! Das Mail wurde automatisch gelöscht.{0}{1}", vbNewLine, msgWarn), emailFileNameforCVL)
		'		Try
		'			If sendNotifyer Then File.Delete(emailFileNameforCVL)

		'		Catch ex As Exception
		'			m_Logger.LogError(String.Format("emailFileNameforCVL file could not be deleted!{0}", emailFileNameforCVL))
		'			Return True
		'		End Try
		'	Else
		'		m_Logger.LogDebug(String.Format("email data is new. m_customerID: {0} | success: {1} >>> EMailFrom: {2} > EMailTo: {3}", m_customerID, success, m_CurrentEMailViewData.EMailFrom, m_CurrentEMailViewData.EMailTo))
		'		AddLogData(String.Format("email data is new. m_customerID: {0} | success: {1} >>> EMailFrom: {2} > EMailTo: {3}", m_customerID, success, m_CurrentEMailViewData.EMailFrom, m_CurrentEMailViewData.EMailTo))
		'	End If


		'	Return success

		'End Function

		Private Function NotifyEMailsender(ByVal emailData As EMailData, ByVal subject As String, ByVal message As String, ByVal emlFilename As String) As Boolean
			Dim result As Boolean = True

			message = String.Format("{1}{0}{2}", vbNewLine, message, emailData.EMailBody)

			Try
				m_Logger.LogDebug("NotifyEMailsender, Debuging: False")
#If Not DEBUG Then
				result = result AndAlso SendMailToWithExchange(m_SettingFile.NotifyEMailFrom, "", emailData.EMailFrom, subject, message, 2, New List(Of String) From {emlFilename})
#End If

#If DEBUG Then
				result = result AndAlso SendMailToWithExchange(m_SettingFile.NotifyEMailFrom, "", emailData.EMailFrom, subject, message, 2, Nothing)
				result = result AndAlso SendMailToWithExchange(m_SettingFile.NotifyEMailFrom, "", m_SettingFile.StagingEMailTo, subject, message, 2, Nothing)
#End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))

				result = False

			End Try

			Return result

		End Function

		Private Function UpdateApplicantDataWithCVLData(ByVal applicantID As Integer, ByVal applicationID As Integer, ByVal priorityModul As EMailSettingData.PriorityModulEnum) As Boolean
			Dim success As Boolean = True

			If m_CVLizerXMLData Is Nothing Then
				m_Logger.LogError("no cv content was founded!")
				AddLogData(String.Format(">>> no cv content was founded!"))

				Return False
			End If

			success = success AndAlso m_AppDatabaseAccess.UpdateApplicatantWithCVLData(m_CVLizerXMLData, applicantID, applicationID, priorityModul)
			m_Logger.LogDebug(String.Format("applicant is now saved with new data: applicantID: {0} | applicationID: {1} >>> ProfileID: {2} | PersonalID: {3} | priorityModul: {4}", applicantID, applicationID, m_CVLizerXMLData.ProfileID, m_CVLizerXMLData.PersonalInformation.PersonalID, priorityModul))
			AddLogData(String.Format("applicant is now saved with new cvl data: applicantID: {0} | applicationID: {1} >>> ProfileID: {2} | PersonalID: {3}", applicantID, applicationID, m_CVLizerXMLData.ProfileID, m_CVLizerXMLData.PersonalInformation.PersonalID))


			Return success

		End Function

		Private Function AddProfieDataForNoNotValidatedCVLData(ByVal applicantID As Integer, ByVal applicationID As Integer) As Boolean
			Dim success As Boolean = True

			success = success AndAlso m_AppDatabaseAccess.AddProfileDataForNotValidatedCVLData(applicantID, applicationID)
			m_Logger.LogDebug(String.Format("applicant is now saved with new cvl profile data: applicantID: {0} | applicationID: {1} >>> success: {2}", applicantID, applicationID, success))
			AddLogData(String.Format("applicant is now saved with new cvl profile data: applicantID: {0} | applicationID: {1}", applicantID, applicationID))


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

				existData.Add(New EntryLOGData With {.LogDate = Now, .LogType = "CVEMail", .Message = msg})
				grdLOG.DataSource = existData
				grdLOG.RefreshDataSource()

				lblRecCount.Text = String.Format("Anzahl Datensätze: {0}", gvLOG.RowCount)
			End If

		End Sub

		Private Function ParseCVFileWithCVLizer(ByVal cvFilename As List(Of String), ByVal userData As EmailUserData, ByVal applicantID As Integer?, ByVal applicationID As Integer?) As Boolean
			Dim result As Boolean = True

			m_CVLizerXMLData = Nothing
			m_CVLParsingResultDescription = String.Empty

			Try
				Dim cvlImport = New CVLizer.Import.CVLizer
				cvlImport.m_AppDatabaseAccess = m_AppDatabaseAccess
				cvlImport.m_CVLDatabaseAccess = m_CVLDatabaseAccess
				cvlImport.m_SettingFile = m_SettingFile
				cvlImport.Customer_ID = m_customerID
				cvlImport.ApplicantID = applicantID
				cvlImport.ApplicationID = applicationID


				Dim payableUser As New CustomerPayableUserData
				payableUser.CustomerID = m_customerID
				payableUser.AdvisorID = userData.User_ID
				payableUser.JobID = "CV-EMAIL"
				payableUser.ServiceName = "CVLIZER_SCAN"
				payableUser.Advisorname = userData.UserFullnameWithoutComma

				cvlImport.PayableUserData = payableUser

				result = result AndAlso cvlImport.ParseCVFileWithCVLizer(cvFilename)
				If result Then
					m_CVLizerXMLData = cvlImport.GetCVLProfileData
				Else
					m_CVLParsingResultDescription = cvlImport.ErrorDescription
				End If

			Catch ex As Exception
				AddLogData(String.Format(">>> cv parsing was failed! applicantID: {0} | applicationID: {1}", applicantID, applicationID))
				m_Logger.LogError(String.Format(">>> cv parsing was failed! applicantID: {0} | applicationID: {1} | {2}", applicantID, applicationID, ex.ToString))

				Return False

			End Try


			Return result

		End Function

#End Region


	End Class


End Namespace

