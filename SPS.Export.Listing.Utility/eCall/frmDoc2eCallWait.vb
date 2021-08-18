Imports SPProgUtility.Mandanten
Imports System.Threading.Tasks
Imports System.Threading
Imports SPSSendMail

Public Class frmDoc2eCallWait

#Region "Private Consts"

  '6000 Unbekannter Fehler
  '6002 Fehler beim Konvertieren der Dokumente
  '6004 Abgebrochen, Keine Antwort
  '6005 Fehler: Nichts zum Senden in diesem Auftrag. Kann keine JobID erstellen.
  '6006 Besetzt
  '6007 Zurückgewiesen
  '6009 Unbekannte Nummer
  '6010 Ungültige Nummer
  '6011 Nummer geändert
  '6013 Gegenstelle ist kein Faxgerät
  '6014 Verbindung vom Sender abgebrochen
  '6015 Verbindung vom Empfänger abgebrochen
  '6016 Dateiformat nicht unterstützt
  '6017 Keinen Dateizugriff
  Public Const EXIT_ON_ERRORSTATUS As String = "6000 # 6002 # 6005 # 6014 # 6016 # 6017"

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
  Private m_faxCollextion As frmDoc2eCall.FaxCollection

#End Region

#Region "Constructor"

  Sub New(ByVal faxCollextion As frmDoc2eCall.FaxCollection)
    InitializeComponent()
    Me.progressPanel1.AutoHeight = True

    m_UITaskScheduler = TaskScheduler.FromCurrentSynchronizationContext()

    m_translate = New TranslateValues
    m_MandantData = New Mandant

    SetCaption(m_translate.GetSafeTranslationValue("Fax Versand"))
    SetDescription(m_translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

    m_faxCollextion = faxCollextion

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

  Public Event FaxSent(ByVal faxReceiver As FaxReceiver)
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
    Dim faxService As New SPSSendMail.ClsFaxStart2(New SPSSendMail.InitializeClass With {
      .MDData = ModulConstants.MDData,
      .ProsonalizedData = ModulConstants.PersonalizedData,
      .TranslationData = ModulConstants.TranslationData,
    .UserData = ModulConstants.UserData
    })

    faxService.AddAttachment(m_faxCollextion.Filename2Send)

    Dim isFirstMessage As Boolean = True
    Dim counter As Integer = 0
    Dim result As String = String.Format("Es wurden {0} Faxe gesendet.", counter)

    For Each faxMessage In m_faxCollextion.Receivers

      Dim status = faxService.SendFax(faxMessage)

      If isFirstMessage Then
        ' Wait for SendState 41, 42
        '101 JobGruppe erfolgreich an Gateway übergeben
        '201 Begonnen mit dem Konvertieren
        '202 Einzelnes File konvertiert
        '301 Begonnen mit dem Zusammenfügen der Dokumente
        '401 Begonnen mit dem Senden
        '402 Einzelner Job abgeschlossen
        '501 Gesamte JobGroup abgeschlossen
        ' 41 Fax Meldung erfolgreich übermittelt.
        ' 42 Versand mit Fehler beendet.
				While faxMessage.SendState <> 41 And faxMessage.SendState <> 42
					If faxMessage.ResponseCode = 11913 Then Exit While

					' exit on error while sending first fax
					Dim ResponseCodeOk = New Long() {-1, 0, 11912}
					If Not ResponseCodeOk.Contains(faxMessage.ResponseCode) Then
						' Break all Messages and LogPaymentService
						result = String.Format("Fehler: [{0}] {1}", faxMessage.ResponseCode.ToString(), faxMessage.ResponseText)
						faxService.LogPaymentService(faxMessage.JobId, faxMessage.PointsUsed)
						Exit For
					End If

					Thread.Sleep(2000)
					status = faxService.GetState(faxMessage)
					faxMessage.UpdateStatus(status)
				End While

				faxService.LogPaymentService(faxMessage.JobId, faxMessage.PointsUsed)

				If faxMessage.SendState = 42 Then
					If EXIT_ON_ERRORSTATUS.IndexOf(faxMessage.ErrorState) > -1 Then
						' Break all Messages
						result = String.Format("Fehler: [{0}] ", faxMessage.ErrorState)
						Exit For
					End If
				End If
				isFirstMessage = False
			Else
				faxService.LogPaymentService(faxMessage.JobId, faxMessage.PointsUsed)
			End If

			'clone message
			Dim cMessage As FaxReceiver = faxMessage.Clone()
			Task.Factory.StartNew(Sub() UpdateMessage(cMessage), CancellationToken.None, TaskCreationOptions.None, m_UITaskScheduler)

			If CONTINUE_ON_RESPONSECODE.IndexOf(faxMessage.ResponseCode) > -1 Then
				counter += 1
				result = String.Format("Es wurden {0} Fax{1} gesendet.", counter, If(counter > 1, "e", ""))
			Else
				result += String.Format("Fehler: [{0}]{1} ", faxMessage.ResponseCode, faxMessage.ResponseText)
				Exit For
			End If

		Next

    Task.Factory.StartNew(Sub() Finished(result), CancellationToken.None, TaskCreationOptions.None, m_UITaskScheduler)
  End Sub

  Private Sub UpdateMessage(ByVal faxReceiver As FaxReceiver)
    RaiseEvent FaxSent(faxReceiver)
  End Sub

  Private Sub Finished(ByVal result As String)
    Me.Close()
    RaiseEvent SendFinished(result)
  End Sub

#End Region

End Class
