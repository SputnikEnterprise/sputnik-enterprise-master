Imports SPProgUtility.Mandanten
Imports System.Threading.Tasks
Imports System.Threading
Imports SPSSendMail

Public Class frmSMS2eCallWait

#Region "Private Consts"

  '  71 Keine „Auftrag akzeptiert“‐Meldung von Gateway
  '1000 Verbindung abgebrochen
  '1001 Timeout: Die Page konnte nicht erfolgreich versandt werden
  '5111 Der Text ist zu lang
  '5112 Ungültige Rufnummer
  '5120 Ziel‐Rufnummer ist eine Tonrufnummer: Meldung unzulässig
  '5121 Ziel‐Rufnummer ist eine Numeriknummer: Unzulässiges Zeichen
  '5155 Zu starker Verkehr
  '5164 Alle Rufzonen gestört oder überlastet
  '5502 Meldung vom Provider: Syntaxfehler
  '5504 Meldung vom Provider: Operation not allowed ‐ Maximum messages for the address exceeded
  '5506 Meldung vom Provider: ADC invalid
  '5509 Ungültige Rufnummer
  Public Const EXIT_ON_ERRORSTATUS As String = "1000 # 1001 # 5155 # 5164 # 5502 # 5506"

  '    0 OK Die Meldung wurde verschickt
  '11000 SyntaxError SyntaxError
  '11001 PermissionDenied Zugriff verweigert
  '11100 AdrAdCInvalid Ungültige oder falsche Empfänger Adresse
  '11101 AdrNAdInvalid Ungültige oder falsche Bestätigungsadresse
  '11102 AdrAdCMissing Keine Empfänger Adresse vorhanden
  '11103 AdrNAdMissing Keine Notifikation Adresse vorhanden
  '11104 AdrAdCTooMany Zu viele Empfänger Adressen übermittelt
  '11105 AdrAdCNotAllowed Empfänger Adresse liegt ausserhalb des definierten Bereiches
  '11200 MsgNoCharacters Keine Meldung vorhanden
  '11201 MsgInvalidCharacters Ungültige Zeichen in der Meldung
  '11202 MsgSendTimeInvalid Ungültige Sendezeit angegeben
  '11203 MsgSendTimeNotAllowed Sendezeit liegt ausserhalb des Zeitfensters
  '11204 MsgTooLong Nur SMS/Pager: Meldung ist zu lang. In diesem Fall muss die Einstellung „Maximale Anzahl Seiten bei langen Meldungen“ in eCall überprüft werden
  '11300 AccNoUser Unbekannter User
  '11301 AccNoUnits Zuwenig Punkte vorhanden
  '11303 AccNoFreeUnits Zuwenig Gratis-Punkte vorhanden
  '11400 CallSystemInvalid Ungültiges Rufsystem
  '11401 CallSystemInvalidForUser Ungültiges Rufsystem für diesen User
  '11402 CallSystemNotSupported Rufsystem wird nicht unterstützt
  '11403 CallSystemConfused Rufsystem ist vorübergehend gestört
  '11500 IDMissing Keine ID vorhanden
  '11501 IDInvalid ID konnte nicht gefunden werden
  '11502 IDDoesNotExist ID existiert im System nicht
  '11600 StateAlreadyTransmitted Meldung wurde bereits gesendet
  '11700 DataReadError Dateninhalt konnte nicht gelesen werden
  '11800 CBMsgError Callback enthält eine verbotene Nummer oder Text
  '11905 Too many attachments Zu viele Attachments vorhanden (max. 10 erlaubt)
  '11906 Attachment(s) too large Mindestens ein Attachment ist zu gross. Maximale Grösse 5MB
  '11907 File type not supported Nicht unterstützter Dateityp in Attachments. Folgende Dateitypen werden durch das System unterstützt: bmp, pdf, doc, docx, rtf, ppt, pptx, dok, snp, gif, tif, tiff, html, txt, jpg, wir, jpeg, xls, xlsx, zip
  '11908 Error while saving the attachments Beim Abspeichern der Attachments ist ein unerwarteter Fehler aufgetreten.
  '11910 Job not found in Log Job konnte im Log nicht gefunden werden
  '11911 JobID required Um eine Statusabfrage absetzen zu können muss eine JobID angegeben werden.
  '11912 Job is scheduled Job ist terminiert
	Public Const CONTINUE_ON_RESPONSECODE As String = "0 # 11912 # 11913"

#End Region

#Region "Private Fields"

  ''' <summary>
  ''' The mandant.
  ''' </summary>
  Private m_MandantData As Mandant

  ''' <summary>
  ''' translate values
  ''' </summary>
  ''' <remarks></remarks>
  Private m_translate As TranslateValues

  ''' <summary>
  ''' The ui task scheduler.
  ''' </summary>
  Private m_UITaskScheduler As TaskScheduler

  ''' <summary>
  ''' The message list
  ''' </summary>
  ''' <remarks></remarks>
  Private m_messages As List(Of ShortMessage)

#End Region

#Region "Constructor"

  Sub New(ByVal messageList As List(Of ShortMessage))
    InitializeComponent()
    Me.progressPanel1.AutoHeight = True

    m_UITaskScheduler = TaskScheduler.FromCurrentSynchronizationContext()

    m_translate = New TranslateValues
    m_MandantData = New Mandant

    SetCaption(m_translate.GetSafeTranslationValue("SMS Versand"))
    SetDescription(m_translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

    m_messages = messageList

  End Sub

#End Region

#Region "Public Methods"

  Public Overrides Sub SetCaption(ByVal caption As String)
    MyBase.SetCaption(caption)
    Me.progressPanel1.Caption = caption
  End Sub

  Public Overrides Sub SetDescription(ByVal description As String)
    MyBase.SetDescription(description)
    Me.progressPanel1.Description = description
  End Sub

  Public Overrides Sub ProcessCommand(ByVal cmd As System.Enum, ByVal arg As Object)
    MyBase.ProcessCommand(cmd, arg)
  End Sub

  Public Enum WaitFormCommand
    SomeCommandId
  End Enum

#End Region

#Region "Events"

  Public Event MessageSent(ByVal message As ShortMessage)
  Public Event SendFinished(ByVal result As String)

  Private Sub OnFormLoad(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
    SendMessages()
  End Sub

#End Region

#Region "Private Methods"

  Private Sub SendMessages()
    Task.Factory.StartNew(Sub() SendMessagesAsync(), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default)
  End Sub


  Private Sub SendMessagesAsync()
    'Webservice
    Dim smsService As New SPSSendMail.ClsSMSStart(New SPSSendMail.InitializeClass With {
      .MDData = ModulConstants.MDData,
      .ProsonalizedData = ModulConstants.PersonalizedData,
      .TranslationData = ModulConstants.TranslationData,
      .UserData = ModulConstants.UserData
    })

    Dim smsCallback = String.Empty ' ModulConstants.UserData.UserMobile
    Dim AnswerMailAddress = String.Empty
    Dim isFirstMessage As Boolean = True
    Dim counter As Integer = 0
		Dim result As String = String.Format(m_translate.GetSafeTranslationValue("Es wurden {0} SMS gesendet."), counter)

		For Each smsMessage In m_messages
      If Not String.IsNullOrWhiteSpace(smsMessage.AnswerAddress) Then
        smsCallback = If(smsMessage.AnswerAddress.Contains("@"), String.Empty, smsMessage.AnswerAddress)
        AnswerMailAddress = If(smsMessage.AnswerAddress.Contains("@"), smsMessage.AnswerAddress, String.Empty)
      End If

      Dim status = smsService.SendSMS(smsMessage, smsCallback, AnswerMailAddress)
      smsMessage.UpdateStatus(status)

      If isFirstMessage Then
				' Wait for SendState 1,8,9,10
				'0 Transmitting
				'1 Transmission OK
				'2 Error -> ErrorState
				'3 Scheduled transmission time
				'8 Transmission OK (reception confirmed)
				'9 Transmission OK (reception not yet confirmed, waiting to delivermessage)
				'10 Transmission OK (reception not confirmed,message delivered)

				While smsMessage.SendState = 0 OrElse smsMessage.SendState = 3

					' exit on error while sending first sms
					Dim ResponseCodeOk = New Long() {-1, 0, 11912}
          If Not ResponseCodeOk.Contains(smsMessage.ResponseCode) Then
            ' Break all Messages and LogPaymentService
            result = String.Format("Fehler: [{0}] {1}", smsMessage.ResponseCode.ToString(), smsMessage.ResponseText)
            smsService.LogPaymentService(smsMessage.JobId, smsMessage.PointsUsed)
            Exit For
          End If

          Thread.Sleep(1000)
          status = smsService.GetState(smsMessage)
          smsMessage.UpdateStatus(status)
        End While

        smsService.LogPaymentService(smsMessage.JobId, smsMessage.PointsUsed)

        If smsMessage.SendState = 2 Then
          If EXIT_ON_ERRORSTATUS.IndexOf(smsMessage.ErrorState) > -1 Then
            ' Break all Messages
            result = String.Format("Fehler: [{0}] ", smsMessage.ErrorState)
            Exit For
          End If
        End If
        isFirstMessage = False
      Else
        smsService.LogPaymentService(smsMessage.JobId, smsMessage.PointsUsed)
      End If

      'clone message
      Dim cMessage As ShortMessage = smsMessage.Clone()
      Task.Factory.StartNew(Sub() UpdateMessage(cMessage), CancellationToken.None, TaskCreationOptions.None, m_UITaskScheduler)

      If CONTINUE_ON_RESPONSECODE.IndexOf(smsMessage.ResponseCode) > -1 Then
        counter += 1
        result = String.Format(m_translate.GetSafeTranslationValue("Es wurden {0} SMS gesendet."), counter)
      Else
        result += String.Format("Fehler: [{0}]{1} ", smsMessage.ResponseCode, smsMessage.ResponseText)
        Exit For
      End If

    Next

    Task.Factory.StartNew(Sub() Finished(result), CancellationToken.None, TaskCreationOptions.None, m_UITaskScheduler)
  End Sub

  Private Sub UpdateMessage(ByVal message As ShortMessage)
    RaiseEvent MessageSent(message)
  End Sub

  Private Sub Finished(ByVal result As String)
    Me.Close()
    RaiseEvent SendFinished(result)
  End Sub

#End Region
End Class
