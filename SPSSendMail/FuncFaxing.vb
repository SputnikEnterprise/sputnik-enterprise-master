
'Imports System.IO
'Imports System.Data
'Imports System.Data.SqlClient
'Imports System.Drawing.Printing
'Imports System.Drawing
'Imports System.Collections.Generic

'Module FuncFaxing
'  Dim regex As New ClsDivFunc

'  Dim _ClsReg As New SPProgUtility.ClsDivReg
'  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
'  Dim _ClsFunc As New ClsDivFunc
'  Dim _ClsLog As New SPProgUtility.ClsEventLog
'  Dim _ClsApp As New ClsAssInfo

'  Dim strConnString As String = _ClsProgSetting.GetConnString()
'  Dim strMDProgFile As String = _ClsProgSetting.GetMDIniFile()
'  Dim strInitProgFile As String = _ClsProgSetting.GetInitIniFile()

'  Dim strMDPath As String = _ClsProgSetting.GetMDIniFile()
'  Dim strInitPath As String = ""
'  Dim MDNr As String = ""

'  Dim iLogedUSNr As Integer = 0
'  Dim iKDNr As Integer = ClsDataDetail.GetKDNr
'  Dim iKDZNr As Integer = ClsDataDetail.GetZHDNr

'  Dim FaxField_1 As String = "KDTelefax"
'  Dim FaxField_2 As String = "ZHDTelefax"
'  Dim strFileForAttachment As String = ""

'Sub GetDataForFaxing(ByVal iOfferNr As Integer, _
'                        ByVal iKDTempNr As Integer, _
'                        ByVal iKDZTempNr As Integer, _
'                        ByVal bSendAsTest As Boolean, _
'                        ByVal strFileToSend As String())
'  Dim Conn As SqlConnection
'  Dim strFullFilename As String() = strFileToSend
'  Dim strFaxServer As String = _ClsProgSetting.GetFaxServer()
'  Dim streMailValue As String = ""
'  Dim Of_Res7 As String = ""
'  Dim Of_Res8 As String = ""
'  Dim Of_Bezeichnung As String = ""
'  Dim strFaxFrom As String = GetFaxFrom()
'  Dim strFaxToValue As String = ""

'  Try
'    Conn = New SqlConnection(strConnString)
'    Conn.Open()
'    Try

'      Dim sOfferSql As String = "Select Of_Res7 From Offers Where OfNr = " & iOfferNr
'      Dim SQLOffCmd As SqlCommand = New SqlCommand(sOfferSql, Conn)
'      Dim rOffrec As SqlDataReader = SQLOffCmd.ExecuteReader          ' Offertendatenbank
'      rOffrec.Read()
'      If Not rOffrec.HasRows Then
'        MsgBox("Keine Daten wurden gefunden. Bitte kontrollieren Sie Ihre Auswahl und versuchen Sie es erneuert.", _
'               MsgBoxStyle.Critical, "Daten nicht gefunden.")

'        Conn.Dispose()
'        Exit Sub
'      Else
'        regex.OfferBez = rOffrec("Of_Res7").ToString

'      End If
'      rOffrec.Close()

'    Catch ex As Exception
'      MsgBox(ex.Message, MsgBoxStyle.Critical, "GetDataForFaxing_1")

'    End Try

'    'iOfferNumber = iOfferNr
'    iKDNr = iKDTempNr
'    iKDZNr = iKDZTempNr
'    strFileForAttachment = strFileToSend

'    Dim sSql As String = "Select KD.Telefax As KDTelefax, KD.eMail As KDeMail, KDZ.Nachname as KDZNachname, KDZ.Vorname as KDZVorname, "
'    sSql &= "KDZ.Anrede as KDZAnrede, KDZ.Anredeform as KDZAnredeForm, KDZ.eMail as ZHDeMail, KDZ.Telefax as ZHDTelefax, "
'    sSql &= "KD.KD_Telefax_Mailing, KDZ.ZHD_Telefax_Mailing "
'    sSql &= "From Kunden KD Left Join KD_Zustaendig KDZ On KD.KDNr = KDZ.KDNr Where KD.KDNr = " & iKDNr.ToString & " "
'    If iKDZNr > 0 Then sSql &= "And KDZ.RecNr = " & iKDZNr.ToString

'    Dim SQLCmd As SqlCommand = New SqlCommand(sSql, Conn)
'    Dim rTemprec As SqlDataReader = SQLCmd.ExecuteReader            ' Kundendatenbank
'    If Not rTemprec.HasRows Then
'      MsgBox("KDNr: " & iKDNr & vbCrLf & "ZHDNr: " & iKDZNr & vbCrLf & _
'             "Keine Kundendaten wurden gefunden. Bitte kontrollieren Sie Ihre Auswahl und versuchen Sie es erneuert.", _
'             MsgBoxStyle.Critical, "Kundendaten nicht gefunden")
'      Conn.Dispose()
'      Exit Sub
'    End If

'    '' wegen der Mailingfelder...
'    'GetRecipientFaxField()
'    Dim strFaxToNumber As String = GetRecipientFaxField()

'    ' Datenbank durchgehen...
'    While rTemprec.Read

'      If strFaxToNumber.ToUpper <> "Kunden und Zuständige Personen".ToUpper Then
'        Try
'          If strFaxToNumber.ToUpper.IndexOf("KDTelefax".ToUpper) >= 0 Then
'            If Not CBool(rTemprec("KD_Telefax_Mailing")) Then
'              If Not String.IsNullOrEmpty(rTemprec(strFaxToNumber).ToString) Then
'                strFaxToValue = rTemprec(strFaxToNumber).ToString
'              End If
'            End If

'          ElseIf strFaxToNumber.ToUpper.IndexOf("ZHDTelefax".ToUpper) >= 0 And iKDZNr > 0 Then
'            If Not CBool(rTemprec("ZHD_Telefax_Mailing")) Then
'              If Not String.IsNullOrEmpty(rTemprec(strFaxToNumber).ToString) Then
'                strFaxToValue = rTemprec(strFaxToNumber).ToString
'              End If
'            End If
'          End If

'          If Not String.IsNullOrEmpty(strFaxToValue) Then
'            regex.KdZEmail = strFaxToValue
'            regex.KdzVorname = rTemprec("KDZVorname").ToString
'            regex.KdzNachname = rTemprec("KDZNachname").ToString

'            If strFileForAttachment = String.Empty Then       ' Ohne Kandidaten
'              strFullFilename = StoreDataToFs(iOfferNr)
'            Else
'              strFullFilename = strFileForAttachment
'            End If

'            If strFullFilename <> String.Empty Then
'              GetUSData()

'              If Not bSendAsTest Then CreateLogToKDKontaktDb(iKDNr, iKDZNr)
'              CreateLogToMailKontaktDb(iKDNr, iKDZNr, 0, strFullFilename, False, regex.OfferBez, "", bSendAsTest)
'            End If

'          End If

'        Catch ex As Exception
'          MsgBox(ex.Message, MsgBoxStyle.Critical, "GetDataForFaxing_3")

'        End Try

'      Else
'        Dim bSendMessage As Boolean
'        Dim aMailFields As New List(Of String)
'        aMailFields.Add(CStr("KDTelefax"))
'        If iKDZNr > 0 Then aMailFields.Add(CStr("ZHDTelefax"))

'        Try
'          For Each strMailAddress As String In aMailFields

'            If strMailAddress.ToUpper.IndexOf("KDTelefax".ToUpper) >= 0 Then
'              bSendMessage = Not CBool(rTemprec("KD_Telefax_Mailing").ToString)

'            ElseIf strMailAddress.ToUpper.IndexOf("ZHDTelefax".ToUpper) >= 0 And iKDZNr > 0 Then
'              bSendMessage = Not CBool(rTemprec("ZHD_Telefax_Mailing").ToString)
'            Else
'              bSendMessage = False
'            End If

'            If Not _ClsProgSetting.IsMessageAlreadySent(rTemprec(strMailAddress).ToString, _
'                                                  regex.OfferBez, "", iKDNr, bSendAsTest) And bSendMessage Then
'              If Not String.IsNullOrEmpty(rTemprec(strMailAddress).ToString) Then
'                strFaxToValue = rTemprec(strMailAddress).ToString
'                regex.KdZEmail = strFaxToValue
'                regex.KdzVorname = rTemprec("KDZVorname").ToString
'                regex.KdzNachname = rTemprec("KDZNachname").ToString

'                If strFileForAttachment = String.Empty Then       ' Ohne Kandidaten
'                  strFullFilename = StoreDataToFs(iOfferNr)
'                Else
'                  strFullFilename = strFileForAttachment
'                End If

'                If strFullFilename <> String.Empty Then
'                  GetUSData()

'                  If Not bSendAsTest Then CreateLogToKDKontaktDb(iKDNr, iKDZNr)
'                  CreateLogToMailKontaktDb(iKDNr, iKDZNr, 0, strFullFilename, False, regex.OfferBez, "", bSendAsTest)
'                End If
'              End If
'            End If

'          Next

'        Catch ex As Exception
'          MsgBox(ex.Message, MsgBoxStyle.Critical, "OpenConnection_4")

'        End Try

'      End If

'    End While
'    rTemprec.Close()

'    Conn.Close()
'    Conn.Dispose()

'  Catch ex As Exception
'    MsgBox(ex.Message, MsgBoxStyle.Critical, "GetDataForFaxing_0")

'  End Try

'End Sub

'Sub SendDataToFaxSmtp(ByVal iOfferNr As Integer, _
'                        ByVal iKDTempNr As Integer, _
'                        ByVal iKDZTempNr As Integer, _
'                        ByVal bSendAsTest As Boolean, _
'                        ByVal strFileToSend As String())
'  Dim Conn As SqlConnection
'  Dim strFullFilename As String() = strFileToSend
'  Dim strFaxServer As String = _ClsProgSetting.GetFaxServer()
'  Dim streMailValue As String = ""
'  Dim Of_Res7 As String = ""
'  Dim Of_Res8 As String = ""
'  Dim Of_Bezeichnung As String = ""
'  Dim strFaxFrom As String = GetFaxFrom()
'  Dim strFaxToValue As String = ""
'  Dim strSmtp As String = _ClsProgSetting.GetSmtpServer()

'  Try
'    If strFaxServer = String.Empty Then
'      MsgBox("Sie haben keinen Fax-Server für Fax-Versand definiert." & vbLf & "Das Programm wird beendet.", _
'            MsgBoxStyle.Critical, "Fax-Versand")
'      Exit Sub
'    End If

'    If strFaxFrom = String.Empty Then
'      MsgBox("Sie haben keine Absender für Fax-Versand definiert." & vbLf & "Das Programm wird beendet.", _
'            MsgBoxStyle.Critical, "Fax-Versand")

'      Exit Sub
'    End If

'    Conn = New SqlConnection(strConnString)
'    Conn.Open()
'    Try

'      Dim sOfferSql As String = "Select Of_Res7 From Offers Where OfNr = " & iOfferNr
'      Dim SQLOffCmd As SqlCommand = New SqlCommand(sOfferSql, Conn)
'      Dim rOffrec As SqlDataReader = SQLOffCmd.ExecuteReader          ' Offertendatenbank
'      rOffrec.Read()
'      If Not rOffrec.HasRows Then
'        MsgBox("Keine Daten wurden gefunden. Bitte kontrollieren Sie Ihre Auswahl und versuchen Sie es erneuert.", _
'               MsgBoxStyle.Critical, "Daten nicht gefunden.")

'        Conn.Dispose()
'        Exit Sub
'      Else
'        regex.OfferBez = rOffrec("Of_Res7").ToString

'      End If
'      rOffrec.Close()

'    Catch ex As Exception
'      MsgBox(ex.Message, MsgBoxStyle.Critical, "GetDataForFaxing_1")

'    End Try

'    iKDNr = iKDTempNr
'    iKDZNr = iKDZTempNr
'    strFileForAttachment = strFileToSend

'    Dim sSql As String = "Select KD.Telefax As KDTelefax, KD.eMail As KDeMail, "
'    sSql &= "KDZ.Nachname as KDZNachname, KDZ.Vorname as KDZVorname, "
'    sSql &= "KDZ.Anrede as KDZAnrede, KDZ.Anredeform as KDZAnredeForm, KDZ.eMail as ZHDeMail, "
'    sSql &= "KDZ.Telefax as ZHDTelefax, "
'    sSql &= "KD.KD_Telefax_Mailing, KDZ.ZHD_Telefax_Mailing "
'    sSql &= "From Kunden KD Left Join KD_Zustaendig KDZ On KD.KDNr = KDZ.KDNr "
'    sSql &= "Where KD.KDNr = " & iKDNr.ToString & " "
'    If iKDZNr > 0 Then sSql &= "And KDZ.RecNr = " & iKDZNr.ToString

'    Dim SQLCmd As SqlCommand = New SqlCommand(sSql, Conn)
'    Dim rTemprec As SqlDataReader = SQLCmd.ExecuteReader            ' Kundendatenbank
'    If Not rTemprec.HasRows Then
'      MsgBox("KDNr: " & iKDNr & vbCrLf & "ZHDNr: " & iKDZNr & vbCrLf & _
'             "Keine Kundendaten wurden gefunden. " & _
'             "Bitte kontrollieren Sie Ihre Auswahl und versuchen Sie es erneuert.", _
'             MsgBoxStyle.Critical, "Kundendaten nicht gefunden")
'      Conn.Dispose()
'      Exit Sub
'    End If

'    '' wegen der Mailingfelder...
'    'GetRecipientFaxField()
'    Dim strFaxToNumber As String = GetRecipientFaxField()

'    ' Datenbank durchgehen...
'    While rTemprec.Read

'      If strFaxToNumber.ToUpper <> "Kunden und Zuständige Personen".ToUpper Then
'        Try
'          If strFaxToNumber.ToUpper.IndexOf("KDTelefax".ToUpper) >= 0 Then
'            If Not CBool(rTemprec("KD_Telefax_Mailing")) Then
'              If Not String.IsNullOrEmpty(rTemprec(strFaxToNumber).ToString) Then
'                strFaxToValue = rTemprec(strFaxToNumber).ToString
'              End If
'            End If

'          ElseIf strFaxToNumber.ToUpper.IndexOf("ZHDTelefax".ToUpper) >= 0 And iKDZNr > 0 Then
'            If Not CBool(rTemprec("ZHD_Telefax_Mailing")) Then
'              If Not String.IsNullOrEmpty(rTemprec(strFaxToNumber).ToString) Then
'                strFaxToValue = rTemprec(strFaxToNumber).ToString
'              End If
'            End If
'          End If

'          If Not String.IsNullOrEmpty(strFaxToValue) Then
'            regex.KdZEmail = strFaxToValue
'            regex.KdzVorname = rTemprec("KDZVorname").ToString
'            regex.KdzNachname = rTemprec("KDZNachname").ToString

'            If strFileForAttachment = String.Empty Then       ' Ohne Kandidaten
'              strFullFilename = StoreDataToFs(iOfferNr)
'            Else
'              strFullFilename = strFileForAttachment
'            End If

'            If strFullFilename <> String.Empty Then
'              GetUSData()
'              Dim streMail2FaxFrom As String = GeteMail2FaxFrom().Trim
'              Dim strFaxExtension As String = GetFaxExtension().Trim
'              Dim streMail2FaxSubject As String = GeteMail2FaxFrom().Trim 'String.Empty

'              'If streMail2FaxFrom.ToLower.Contains("subject:") Then
'              '  streMail2FaxSubject = streMail2FaxFrom.Replace("subject:", "")
'              'Else
'              '  streMail2FaxFrom = streMail2FaxFrom.Replace("from:", "")
'              'End If


'              If Not strFaxToValue.StartsWith("00") And Not strFaxToValue.StartsWith("+") Then
'                If strFaxToValue.StartsWith("0") Then strFaxToValue = strFaxToValue.Remove(0, 1)
'                strFaxToValue = "0041" & strFaxToValue
'              End If
'              strFaxToValue = strFaxToValue.Replace(" ", "").Replace("-", "").Replace("/", "")

'              If SendMailToKD(False, _
'                              streMail2FaxFrom, _
'                              strFaxToValue & strFaxExtension, _
'                              strFaxFrom, _
'                              String.Empty, _
'                              1, _
'                              strFullFilename, _
'                              strSmtp) Then

'                'If Not bSendAsTest Then CreateLogToKDKontaktDb(iKDNr, iKDZNr)
'                'CreateLogToMailKontaktDb(iKDNr, iKDZNr, 0, strFullFilename, _
'                '                         bSendasHtml, rTemprec(strMyeMailField).ToString, _
'                '                         streMailFrom, strOf_Res7, strOf_Res8, bSendAsTest)

'                If Not bSendAsTest Then CreateLogToKDKontaktDb(iKDNr, iKDZNr)
'                CreateLogToMailKontaktDb(iKDNr, iKDZNr, 0, strFullFilename, False, regex.OfferBez, "", bSendAsTest)
'              End If

'            End If

'          End If

'        Catch ex As Exception
'          MsgBox(ex.Message, MsgBoxStyle.Critical, "GetDataForFaxing_3")

'        End Try

'      Else
'        Dim bSendMessage As Boolean
'        Dim aMailFields As New List(Of String)
'        aMailFields.Add(CStr("KDTelefax"))
'        If iKDZNr > 0 Then aMailFields.Add(CStr("ZHDTelefax"))

'        Try
'          For Each strMailAddress As String In aMailFields

'            If strMailAddress.ToUpper.IndexOf("KDTelefax".ToUpper) >= 0 Then
'              bSendMessage = Not CBool(rTemprec("KD_Telefax_Mailing").ToString)

'            ElseIf strMailAddress.ToUpper.IndexOf("ZHDTelefax".ToUpper) >= 0 And iKDZNr > 0 Then
'              bSendMessage = Not CBool(rTemprec("ZHD_Telefax_Mailing").ToString)
'            Else
'              bSendMessage = False
'            End If

'            If Not _ClsProgSetting.IsMessageAlreadySent(rTemprec(strMailAddress).ToString, _
'                                                  regex.OfferBez, "", iKDNr, bSendAsTest) And bSendMessage Then
'              If Not String.IsNullOrEmpty(rTemprec(strMailAddress).ToString) Then
'                strFaxToValue = rTemprec(strMailAddress).ToString
'                regex.KdZEmail = strFaxToValue
'                regex.KdzVorname = rTemprec("KDZVorname").ToString
'                regex.KdzNachname = rTemprec("KDZNachname").ToString

'                If strFileForAttachment = String.Empty Then       ' Ohne Kandidaten
'                  strFullFilename = StoreDataToFs(iOfferNr)
'                Else
'                  strFullFilename = strFileForAttachment
'                End If

'                If strFullFilename <> String.Empty Then
'                  GetUSData()

'                  If Not bSendAsTest Then CreateLogToKDKontaktDb(iKDNr, iKDZNr)
'                  CreateLogToMailKontaktDb(iKDNr, iKDZNr, 0, strFullFilename, False, regex.OfferBez, "", bSendAsTest)
'                End If
'              End If
'            End If

'          Next

'        Catch ex As Exception
'          MsgBox(ex.Message, MsgBoxStyle.Critical, "OpenConnection_4")

'        End Try

'      End If

'    End While
'    rTemprec.Close()

'    Conn.Close()
'    Conn.Dispose()

'  Catch ex As Exception
'    MsgBox(ex.Message, MsgBoxStyle.Critical, "GetDataForFaxing_0")

'  End Try

'End Sub

'Function SendMessasgeWithFax(ByVal bSendAsTest As Boolean) As Boolean
'  Dim strConnString As String = ClsDataDetail.GetDbConnString()
'  Dim Conn As SqlConnection
'  Dim iOfferNumber As Integer = ClsDataDetail.GetOffNr
'  Dim iKDNr As Integer = ClsDataDetail.GetKDNr
'  Dim iKDZNr As Integer = ClsDataDetail.GetZHDNr
'  Dim streMailValue As String = String.Empty
'  Dim Of_Res7 As String = String.Empty
'  Dim Of_Res8 As String = String.Empty
'  Dim Of_Bezeichnung As String = String.Empty
'  Dim streMailFrom As String = GetFaxFrom()
'  Dim streMailToField As String = ClsDataDetail.GeteMailFieldToSend()

'  Dim sOfferSql As String = "Select Offers.OF_Res1, Offers.OF_Res2, Offers.OF_Res3, Offers.OF_Res4, Offers.OF_Res5,"
'  sOfferSql &= "Offers.OF_Res6, Offers.OF_Res7, Offers.OF_Res8, "
'  sOfferSql &= "Offers.Of_Slogan, Offers.OF_Gruppe, Offers.OF_Kontakt, Offers.OF_Bezeichnung, "
'  sOfferSql &= "KD.KDNr, KD.Firma1, KD.eMail As KDeMail, KD.KD_Mail_Mailing, "
'  sOfferSql &= "KD.Telefax As KDTelefax, "
'  sOfferSql &= "KDZ.Nachname as KDZNachname, "
'  sOfferSql &= "KDZ.Vorname as KDZVorname, KDZ.Anrede as KDZAnrede, KDZ.Anredeform as KDZAnredeForm, "
'  sOfferSql &= "KDZ.Telefax as ZHDTelefax, KDZ.eMail as ZHDeMail, KDZ.ZHD_Mail_Mailing, KDZ.RecNr As KDZNr "
'  sOfferSql &= "From Offers, "
'  sOfferSql &= "Kunden KD Left Join KD_Zustaendig KDZ On KD.KDNr = KDZ.KDNr "
'  sOfferSql &= String.Format("Where KD.KDNr = {0} And Offers.OfNr = {1} ", iKDNr, iOfferNumber)
'  If iKDZNr > 0 Then sOfferSql &= String.Format("And KDZ.RecNr = {0}", iKDZNr)

'  Conn = New SqlConnection(strConnString)

'  Try
'    Conn.Open()


'  Catch ex As Exception
'    _ClsLog.WriteTempLogFile(String.Format("***SendMessasgeWithFax_1: {0}", ex.Message))

'  End Try

'  ' wegen der Mailingfelder...
'  'GetOffMailingValue()
'  Try
'    Dim SQLOffCmd As SqlCommand = New SqlCommand(sOfferSql, Conn)
'    Dim rOffrec As SqlDataReader = SQLOffCmd.ExecuteReader          ' Offertendatenbank
'    rOffrec.Read()
'    If Not rOffrec.HasRows AndAlso String.IsNullOrEmpty(rOffrec(streMailToField).ToString) Then
'      _ClsLog.WriteTempLogFile(String.Format("***SendMessasgeWithFax_2: {0}", _
'                                             "Keine Daten wurden gefunden. " & _
'                                             "Bitte kontrollieren Sie Ihre Auswahl und versuchen Sie es erneuert."))
'      Conn.Dispose()
'      Exit Function

'    Else
'      Of_Res7 = rOffrec("Of_Res7").ToString
'      Of_Res8 = rOffrec("Of_Res8").ToString
'      Of_Bezeichnung = rOffrec("Of_Bezeichnung").ToString

'      With regex
'        .OfferSchluss = rOffrec("OF_Res6").ToString
'        .OfferNachricht = rOffrec("OF_Res8").ToString
'        .OfferRes1 = rOffrec("OF_Res1").ToString
'        .OfferRes2 = rOffrec("OF_Res2").ToString
'        .OfferRes3 = rOffrec("OF_Res3").ToString
'        .OfferRes4 = rOffrec("OF_Res4").ToString
'        .OfferRes5 = rOffrec("OF_Res5").ToString

'        .OfferWerbe = rOffrec("OF_Slogan").ToString
'        .OfferGruppe = rOffrec("OF_Gruppe").ToString
'        .OfferKontakt = rOffrec("OF_Kontakt").ToString
'        .OfferBez = rOffrec("OF_Bezeichnung").ToString

'        .Off_KDNr = ClsDataDetail.GetKDNr
'        .Off_KDZNr = ClsDataDetail.GetZHDNr

'      End With

'      Try
'        SendFaxOverSMTP(rOffrec, streMailToField, Of_Res7, Of_Res8, ClsDataDetail.SendAsHtml, bSendAsTest)

'      Catch ex As Exception
'        _ClsLog.WriteTempLogFile(String.Format("***SendMessasgeWithFax_3: {0}", ex.Message))

'      End Try

'    End If

'  Catch ex As Exception
'    _ClsLog.WriteTempLogFile(String.Format("***SendMessasgeWithFax_4: {0}", ex.Message))

'  End Try

'  Return True
'End Function

'Sub SendFaxOverSMTP(ByVal rTemprec As SqlDataReader, _
'                       ByVal strMyeMailField As String, _
'                       ByVal strOf_Res7 As String, _
'                       ByVal strOf_Res8 As String, _
'                       ByVal bSendasHtml As Boolean, _
'                       ByVal bSendAsTest As Boolean)
'  Dim strFullFilename As String() = ClsDataDetail.GetAttachmentFile()
'  Dim strSmtp As String = _ClsProgSetting.GetSmtpServer()
'  Dim streMailFrom As String = GetFaxFrom()
'  Dim iOfferNumber As Integer = ClsDataDetail.GetOffNr
'  Dim iKDNr As Integer = ClsDataDetail.GetKDNr
'  Dim iKDZNr As Integer = ClsDataDetail.GetZHDNr

'  Try
'    If Not String.IsNullOrEmpty(rTemprec(strMyeMailField).ToString) Then

'      Try
'        With regex
'          .MailBetreff = strOf_Res7.Trim
'          .BodyAsHtml = bSendasHtml
'          .KdEmail = rTemprec("KDeMail").ToString
'          ' Zum TESTEN...
'          .Off_KDNr = CInt(rTemprec("KDNr").ToString)
'          .Off_Firma1 = rTemprec("Firma1").ToString
'          .KdZEmail = rTemprec("ZHDeMail").ToString
'          If Not strMyeMailField.ToUpper.Contains("KDTelefax".ToUpper) Then
'            .Off_KDZNr = CInt(rTemprec("KDZNr").ToString)

'            .KdzAnredeform = rTemprec("KDZAnredeForm").ToString
'            .KdzNachname = rTemprec("KDZNachname").ToString
'            .KdzVorname = rTemprec("KDZVorname").ToString

'          Else
'            .KdzAnredeform = String.Empty
'            .KdzNachname = String.Empty
'            .KdzVorname = String.Empty

'          End If

'          If .KdzAnredeform & .KdzNachname & .KdzVorname = String.Empty Then
'            .KdzNachname = _ClsProgSetting.TranslateText("Sehr geehrte Damen und Herren")
'          End If

'        End With

'      Catch ex As Exception
'        _ClsLog.WriteTempLogFile(String.Format("***SendFaxOverSMTP_1: {0}", ex.Message))

'      End Try

'		GetUSData(iOfferNumber, regex)
'      strOf_Res7 = regex.ParseTemplateFile(strOf_Res7.Trim)     ' Betreff
'      strOf_Res8 = regex.ParseTemplateFile(String.Empty)        ' Body

'      Dim streMail2FaxFrom As String = GeteMail2FaxFrom().Trim
'      Dim strFaxExtension As String = GetFaxExtension().Trim
'      Dim streMail2FaxSubject As String = streMail2FaxFrom

'      If streMail2FaxFrom.ToLower.Contains("subject:") Then
'        streMail2FaxSubject = streMail2FaxFrom.Replace("subject:", "")
'      Else
'        streMail2FaxFrom = streMail2FaxFrom.Replace("from:", "")
'      End If
'      Dim strFaxToValue As String = rTemprec(strMyeMailField).ToString

'      If Not strFaxToValue.StartsWith("00") And Not strFaxToValue.StartsWith("+") Then
'        If strFaxToValue.StartsWith("0") Then strFaxToValue = strFaxToValue.Remove(0, 1)
'        strFaxToValue = "0041" & strFaxToValue
'      End If
'      strFaxToValue = strFaxToValue.Replace(" ", "").Replace("-", "").Replace("/", "")

'      ' Überprüfen ob die Nachricht bereits versendet wurde...
'      If Not bSendAsTest Then
'        If IsMyMessageAlreadySent(rTemprec(strMyeMailField).ToString, _
'                                          strOf_Res7, strOf_Res8, iKDNr, _
'                                          ClsDataDetail.GetMessageGuid, bSendAsTest) Then
'          _ClsLog.WriteTempLogFile(String.Format("***SendFaxOverSMTP_: {0} ==>> {1}", _
'                                                   "Achtung: Wurde bereits versendet!!!", rTemprec(strMyeMailField).ToString))
'          Exit Sub
'        End If

'      End If
'      If strFullFilename.Length > 0 Then
'        If strFullFilename(0) = String.Empty Then strFullFilename = StoreDataToFs(iOfferNumber)
'      End If

'      Dim strValue As String = "Erfolgreich"
'      strValue = SendMailToKD(False, _
'                                streMail2FaxFrom, _
'                                strFaxToValue & strFaxExtension, _
'                                String.Empty, _
'                                String.Empty, _
'                                1, _
'                                strFullFilename, _
'                                strSmtp, _
'                                regex.Exchange_USName, _
'                                regex.Exchange_USPW)
'		If strValue.ToLower.Contains("erfolgreich".ToLower) Then
'			Dim result As Boolean = True
'			If Not bSendAsTest Then result = result AndAlso CreateLogToKDKontaktDb(iKDNr, iKDZNr)
'			result = result AndAlso CreateLogToMailKontaktDb(iKDNr, iKDZNr, 0, strFullFilename, _
'														 bSendasHtml, rTemprec(strMyeMailField).ToString, _
'														 streMailFrom, strOf_Res7, strOf_Res8, bSendAsTest)
'		End If

'    End If

'  Catch ex As Exception
'    _ClsLog.WriteTempLogFile(String.Format("***SendFaxOverSMTP_3: {0}", ex.Message))

'  End Try

'End Sub

'Sub SendFaxOverSMTP_0()
'              Dim streMail2FaxFrom As String = GeteMail2FaxFrom().Trim
'              Dim strFaxExtension As String = GetFaxExtension().Trim
'              Dim streMail2FaxSubject As String = GeteMail2FaxFrom().Trim 'String.Empty

'              'If streMail2FaxFrom.ToLower.Contains("subject:") Then
'              '  streMail2FaxSubject = streMail2FaxFrom.Replace("subject:", "")
'              'Else
'              '  streMail2FaxFrom = streMail2FaxFrom.Replace("from:", "")
'              'End If


'              If Not strFaxToValue.StartsWith("00") And Not strFaxToValue.StartsWith("+") Then
'                If strFaxToValue.StartsWith("0") Then strFaxToValue = strFaxToValue.Remove(0, 1)
'                strFaxToValue = "0041" & strFaxToValue
'              End If
'              strFaxToValue = strFaxToValue.Replace(" ", "").Replace("-", "").Replace("/", "")

'              If SendMailToKD(False, _
'                              streMail2FaxFrom, _
'                              strFaxToValue & strFaxExtension, _
'                              strFaxFrom, _
'                              String.Empty, _
'                              1, _
'                              strFullFilename, _
'                              strSmtp) Then

'End Sub


'Function StoreDataToFs(ByVal lOFNr As Integer) As String()
'	Dim Conn As New SqlConnection(ClsDataDetail.GetDbConnString)
'	Dim strFullFilename As String() = New String() {""}
'	Dim strFiles As String = String.Empty
'	Dim BA As Byte()
'	Dim sOffDocSql As String = "Select DocScan, Bezeichnung From OFF_Doc Where "
'	sOffDocSql &= String.Format("OfNr = {0} And MANr = {1}", lOFNr, 0)

'	Dim i As Integer = 0

'	Conn.Open()
'	Dim SQLCmd As SqlCommand = New SqlCommand(sOffDocSql, Conn)
'	Dim SQLCmd_1 As SqlCommand = New SqlCommand(sOffDocSql, Conn)
'	Dim rOfDoc As SqlDataReader = SQLCmd.ExecuteReader

'	Try
'		While rOfDoc.Read
'			ClsDataDetail.IsAttachedFileInd = True
'			Dim strSelectedFile As String = _ClsProgSetting.GetPersonalFolder & System.IO.Path.GetFileName(rOfDoc("Bezeichnung").ToString)
'			strFiles &= If(strFiles <> String.Empty, ";", "") & _
'									_ClsProgSetting.GetPersonalFolder & System.IO.Path.GetFileName(rOfDoc("Bezeichnung").ToString)

'			Try
'				BA = CType(SQLCmd_1.ExecuteScalar, Byte())

'				Dim ArraySize As New Integer
'				ArraySize = BA.GetUpperBound(0)

'				If File.Exists(strSelectedFile) Then File.Delete(strSelectedFile)
'				Dim fs As New FileStream(strSelectedFile, FileMode.CreateNew)
'				fs.Write(BA, 0, ArraySize + 1)
'				fs.Close()
'				fs.Dispose()

'				i += 1

'			Catch ex As Exception
'				_ClsLog.WriteTempLogFile(String.Format("***StoreDataToFs_1: {0}", ex.Message))

'			End Try

'		End While
'		ReDim strFullFilename(i - 1)
'		strFullFilename = strFiles.Split(CChar(";"))

'		rOfDoc.Close()

'	Catch ex As Exception
'		_ClsLog.WriteTempLogFile(String.Format("***StoreDataToFs_2: {0}", ex.Message))

'	End Try

'	Return strFullFilename
'End Function

'#Region "Kontaktdatenbanken schreiben..."

'Private Sub CreateLogToKDKontaktDb_0(ByVal lKDNr As Integer, ByVal lZHDNr As Integer)
'	Dim Time_1 As Double = System.Environment.TickCount
'	Dim strUSName As String = _ClsProgSetting.GetUserName()
'	Dim Conn As New SqlConnection(strConnString)
'	Dim sKDZSql As String = "Insert Into KD_KontaktTotal (KDNr, KDZNr, RecNr, KontaktDate, Kontakte, KontaktType1, KontaktType2, "
'	sKDZSql &= "Kontaktwichtig, KontaktDauer, KontaktErledigt, MANr, CreatedOn, CreatedFrom) "
'	sKDZSql &= "Values (@KDNr, @ZHDNr, @RecNr, @KontaktDate, "
'	sKDZSql &= "'Wurde Offerte geschickt', 'Faxmailing', 2, 0, '', 0, 0, @KontaktDate, @USName)"
'	Dim lNewRecNr As Integer

'	Try
'		Conn.Open()
'		lNewRecNr = GetNewKontaktNr(lKDNr, lZHDNr)

'		Dim rKontaktrec As New SqlDataAdapter()

'		rKontaktrec.SelectCommand = New SqlCommand(sKDZSql, Conn)
'		rKontaktrec.SelectCommand.Parameters.AddWithValue("@KDNr", lKDNr)
'		rKontaktrec.SelectCommand.Parameters.AddWithValue("@ZHDNr", lZHDNr)
'		rKontaktrec.SelectCommand.Parameters.AddWithValue("@RecNr", lNewRecNr)

'		rKontaktrec.SelectCommand.Parameters.AddWithValue("@KontaktDate", Now.ToString("G"))
'		rKontaktrec.SelectCommand.Parameters.AddWithValue("@USName", strUSName)

'		Dim dt As DataTable = New DataTable()
'		rKontaktrec.Fill(dt)

'	Catch e As Exception

'		MsgBox(Err.Description, MsgBoxStyle.Critical, "Kundenkontakt hinzufügen")

'	End Try

'	Conn.Close()

'	Dim Time_2 As Double = System.Environment.TickCount
'	Console.WriteLine("Zeit für CreateLogToKDKontaktDb: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")

'End Sub

'	Private Sub CreateLogToMailKontaktDb_0(ByVal lKDNr As Integer, _
'															 ByVal lZHDNr As Integer, _
'															 ByVal lMANr As Integer, _
'															 ByVal strFilename As String, _
'															 ByVal bSendAsHtml As Boolean, _
'															 Optional ByVal MailSubject As String = "", _
'															 Optional ByVal MailBody As String = "", _
'															 Optional ByVal bSendAsTest As Boolean = False)
'		Dim Time_1 As Double = System.Environment.TickCount
'		Dim strUSName As String = _ClsProgSetting.GetUserName()
'		Dim Conn As New SqlConnection(strConnString)
'		Dim sMailSql As String = "Insert Into Mail_Kontakte (KDNr, KDZNr, RecNr, MANr, eMail_To, eMail_From, eMail_Subject, eMail_Body, "
'		sMailSql &= "eMail_smtp, AsHtml, AsTelefax, CreatedOn, CreatedFrom) Values (@KDNr, @ZHDNr, @RecNr, @MANr, @eMailTo, @eMailFrom, "
'		sMailSql &= "@eMailsubject, @eMailbody, @eMailSmtp, 0, 1, @KontaktDate, @USName)"
'		Dim lNewRecNr As Integer

'		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
'		Dim param As System.Data.SqlClient.SqlParameter

'		Try
'			Conn.Open()
'			lNewRecNr = GetNeweMailKontaktNr()

'			cmd.CommandType = CommandType.Text
'			cmd.CommandText = sMailSql

'			param = cmd.Parameters.AddWithValue("@KDNr", lKDNr)
'			param = cmd.Parameters.AddWithValue("@ZHDNr", lZHDNr)
'			param = cmd.Parameters.AddWithValue("@RecNr", lNewRecNr)

'			param = cmd.Parameters.AddWithValue("@MANr", lMANr)

'			param = cmd.Parameters.AddWithValue("@eMailTo", regex.KdZEmail)
'			param = cmd.Parameters.AddWithValue("@eMailFrom", regex.USMDTelefax)
'			param = cmd.Parameters.AddWithValue("@eMailsubject", MailSubject & CStr(IIf(bSendAsTest, " Als TEST (" & Now & ")", "")))
'			param = cmd.Parameters.AddWithValue("@eMailbody", strFilename)
'			param = cmd.Parameters.AddWithValue("@eMailSmtp", _ClsProgSetting.GetFaxServer())

'			param = cmd.Parameters.AddWithValue("@KontaktDate", Now.ToString("G"))
'			param = cmd.Parameters.AddWithValue("@USName", strUSName)

'			cmd.Connection = Conn
'			cmd.ExecuteNonQuery()

'		Catch e As Exception
'			MsgBox(Err.Description, MsgBoxStyle.Critical, "Mailkontakt hinzufügen")

'		Finally
'			cmd.Dispose()
'			Conn.Close()

'			' Binaryfile in die Datenbank
'			'If strFilename <> String.Empty Then InsertBinaryToMailDb(lNewRecNr, strFilename, regex.KdZEmail, _
'			'                                                          regex.USMDTelefax, MailSubject)

'		End Try

'		Dim Time_2 As Double = System.Environment.TickCount
'		Console.WriteLine("Zeit für CreateLogToMailKontaktDb: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")

'	End Sub

'	Private Sub InsertBinaryToMailDb_0(ByVal lRecNr As Integer, ByVal strFilename As String, _
'												Optional ByVal MailTo As String = "", Optional ByVal MailFrom As String = "", _
'												Optional ByVal MailSubject As String = "")
'		Dim Time_1 As Double = System.Environment.TickCount
'		Dim strUSName As String = _ClsProgSetting.GetUserName()
'		Dim Conn As New SqlConnection(strConnString)

'		Dim sMailSql As String = "Insert Into [{0}].dbo.Mail_FileScan (RecNr, eMail_To, eMail_From, eMail_Subject, ScanFile, Filename, "
'		sMailSql &= "Customer_ID, CreatedOn, CreatedFrom) Values (@RecNr, @eMailTo, @eMailFrom, @eMailsubject, @BinaryFile, @FileName, "
'		sMailSql &= "@Customer_ID, @KontaktDate, @USName)"
'		sMailSql = String.Format(sMailSql, ClsDataDetail.GetMailDbName)

'		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
'		Dim param As System.Data.SqlClient.SqlParameter

'		' Load file into a byte array
'		Dim fi As New System.IO.FileInfo(strFilename)
'		Dim fs As System.IO.FileStream = fi.OpenRead
'		Dim CheckFile As New FileInfo(strFilename)

'		Dim lBytes As Integer = CInt(fs.Length)
'		Dim myImage(lBytes) As Byte

'		fs.Read(myImage, 0, lBytes)
'		fs.Close()
'		fs.Dispose()

'		Try
'			Conn.Open()

'			cmd.CommandType = CommandType.Text
'			cmd.CommandText = sMailSql

'			param = cmd.Parameters.AddWithValue("@RecNr", lRecNr)

'			param = cmd.Parameters.AddWithValue("@eMailTo", MailTo)
'			param = cmd.Parameters.AddWithValue("@eMailFrom", MailFrom)
'			param = cmd.Parameters.AddWithValue("@eMailsubject", MailSubject)

'			param = cmd.Parameters.AddWithValue("@BinaryFile", myImage)
'			param = cmd.Parameters.AddWithValue("@FileName", CheckFile.Name)

'			param = cmd.Parameters.AddWithValue("@Customer_ID", _ClsProgSetting.GetMDGuid)
'			param = cmd.Parameters.AddWithValue("@KontaktDate", Now.ToString("G"))
'			param = cmd.Parameters.AddWithValue("@USName", strUSName)

'			cmd.Connection = Conn
'			cmd.ExecuteNonQuery()

'		Catch e As Exception
'			MsgBox(Err.Description, MsgBoxStyle.Critical, "Faxdokument hinzufügen")

'		Finally
'			cmd.Dispose()
'			Conn.Close()

'		End Try

'		Dim Time_2 As Double = System.Environment.TickCount
'		Console.WriteLine("Zeit für InsertBinaryToMailDb: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")

'	End Sub

'#End Region

'Function GetFaxFrom() As String
'	Dim Time_1 As Double = System.Environment.TickCount
'	Dim Conn As New SqlConnection(strConnString)
'	Dim strFaxFrom As String = ""

'	Dim sMDSql As String = "[Get MDData For Header] " & Year(Now) & ", " & _ClsProgSetting.GetLogedUSNr()

'	Conn.Open()
'	Dim SQLMDCmd As SqlCommand = New SqlCommand(sMDSql, Conn)
'	Dim rTemprec As SqlDataReader = SQLMDCmd.ExecuteReader

'	rTemprec.Read()
'	strFaxFrom = rTemprec("MDTelefax").ToString

'	Conn.Close()

'	Dim Time_2 As Double = System.Environment.TickCount
'	Console.WriteLine("Zeit für GeteMailFrom: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")

'	Return strFaxFrom
'End Function


'#Region "Testfunktionen..."

'Sub SendFax(ByVal strFullFilename As String, ByVal strSubject As String)
'  Dim strFaxServerName As String = ClsSystem.GetFaxServer()
'  Dim strDavidServerName As String = ClsSystem.GetDavidServer()
'  Dim lJobID As Int32

'  Try

'    If ClsSystem.GetLogedUSNr() = 1 Then MsgBox("Der Vorgang wird gestartet...")

'    Dim faxServ As FAXCOMLib.FaxServer
'    faxServ = New FAXCOMLib.FaxServer

'    Dim faxDoc As FAXCOMLib.FaxDoc
'    If ClsSystem.GetLogedUSNr() = 1 Then MsgBox("Beendet_1...")

'    Try
'      faxServ.Connect(strFaxServerName)
'    Catch ex As Exception
'      MsgBox("Ich kann leider keine Verbindung zum Faxserver " & strFaxServerName & " herstellen." & vbCrLf & _
'      "Ich versuche es noch einmal. Möglicherweise greift eine andere Software auf Faxserver zu." & vbCrLf & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Connect_2...")

'      faxServ.Connect("")

'      Try
'        faxServ.Connect(strFaxServerName)

'      Catch e As Exception
'        MsgBox("Leider kann ich immer noch keine Verbindung zum Faxserver " & strFaxServerName & " herstellen." & vbCrLf & _
'"Der Versuch wird unterbrochen. Bitte Beenden Sie alle Programme, die das MSFax benutzen." & vbCrLf & vbCrLf & e.Message, MsgBoxStyle.Critical, "Connect_3...")
'        Exit Sub
'      End Try

'    End Try


'    If ClsSystem.GetLogedUSNr() = 1 Then MsgBox("Connected_1..." & vbCrLf & strFaxServerName)

'    Try

'      faxDoc = CType(faxServ.CreateDocument(strFullFilename), FAXCOMLib.FaxDoc)

'      If File.Exists(strFullFilename) Then
'        faxDoc.FaxNumber = regex.KdZEmail
'        faxDoc.RecipientCompany = regex.KdzVorname & " " & regex.KdzNachname

'        If ClsSystem.GetLogedUSNr() = 1 Then MsgBox("Connected_4...")

'        faxDoc.SenderName = regex.USVorname & " " & regex.USNachname
'        faxDoc.SenderCompany = regex.USMDname
'        faxDoc.SenderFax = regex.USMDTelefax

'        faxDoc.DisplayName = regex.OfferBez
'        If ClsSystem.GetLogedUSNr() = 1 Then MsgBox("Connected_5...")

'        lJobID = faxDoc.Send()
'        If ClsSystem.GetLogedUSNr() = 1 Then MsgBox("Submitted...")

'      End If

'    Catch ex As Exception
'      MsgBox(ex.Message.ToString & vbCrLf & ex.GetBaseException.ToString, MsgBoxStyle.Critical, "SendFax_1")

'    Finally
'      faxDoc = Nothing
'      faxServ.Disconnect()
'      If ClsSystem.GetLogedUSNr() = 1 Then MsgBox("Disconnected_1..." & vbCrLf & strFaxServerName)

'    End Try

'  Catch ex As Exception
'    MsgBox(strFaxServerName & vbCrLf & ex.Message.ToString & vbCrLf & ex.GetBaseException.ToString, MsgBoxStyle.Critical, "SendFax_2")

'  Finally

'  End Try

'End Sub


'Sub SendFaxOverPrinter(ByVal strFullFilename As String, ByVal strSubject As String)
'  Dim strFaxServerName As String = ClsSystem.GetFaxServer()
'  Dim strDavidServerName As String = ClsSystem.GetDavidServer()
'  Dim Druckerform As PrintDialog = New PrintDialog

'  Dim sPrinterName As String = "Tobit FaxWare"

'  ChangePrinter(sPrinterName)
'  'For i = 0 To System.Drawing.Printing.PrinterSettings.InstalledPrinters.Count - 1
'  '  sPrinterName = System.Drawing.Printing.PrinterSettings.InstalledPrinters.Item(i)
'  '  If UCase(sPrinterName) = UCase("Tobit Faxware") Then


'  '    'System.Drawing.Printing.PrinterSettings.InstalledPrinters.Item(i)
'  '    System.Drawing.Printing.PrintDocument()
'  '  End If
'  '  '      ComboBox1.Items.Add(sPrinterName)
'  'Next


'  ' 1. Erfolg...
'  'Dim p As New System.Diagnostics.ProcessStartInfo()
'  'p.Verb = "print"
'  'p.WindowStyle = ProcessWindowStyle.Hidden
'  'p.FileName = strFullFilename
'  'p.UseShellExecute = True
'  'System.Diagnostics.Process.Start(p)
'  'p = Nothing


'  ' Geht hier das Printdialog auf...
'  'Dim pd As PrintDocument = New PrintDocument
'  'pd.DocumentName = strFullFilename
'  'pd.OriginAtMargins = False
'  'pd.PrinterSettings.PrinterName = sPrinterName
'  'pd.PrintController() = New System.Drawing.Printing.StandardPrintController()
'  'pd.Print()

'  'pd.Dispose()

'  MsgBox("Wurde Printing gestartet...")
'  Dim Process As New Process()
'  Dim pathToExecutable As String = "AcroRd32.exe"
'  Dim startInfo As New ProcessStartInfo(pathToExecutable)
'  startInfo.WindowStyle = ProcessWindowStyle.Minimized

'  Process.Start(startInfo)
'  startInfo.Arguments = "/t " + strFullFilename + " " + sPrinterName + ""
'  MsgBox("Argumente übergeben...")

'  'Dim startinfo As New ProcessStartInfo(pathToExecutable, "/t " + strFullFilename + " " + sPrinterName + "")
'  Process.Start(startInfo)
'  System.Threading.Thread.Sleep(30000)
'  MsgBox("gestartet...")

'  Process.CloseMainWindow()

'End Sub

'Public Function ChangePrinter(ByVal PrinterName As String) As Boolean
'	' Benötigte Variablen

'	Dim scope As String = "ROOT\CIMV2"
'	Dim query As String = "Select * from Win32_Printer"
'	Const DefaultPrinter As String = "SetDefaultPrinter"
'	Const ReturnValue As String = "ReturnValue"

'	' Fehlerüberwachung einschalten
'	Try
'		Dim Printers As New Management.ManagementObjectSearcher(scope, query)
'		For Each Printer As Management.ManagementObject In Printers.Get()
'			Dim PrinterDescription As String = _
'				DirectCast(Printer.GetPropertyValue("Name"), String)
'			' Vergleichsvariable deklarieren und initialisieren
'			Dim Compared As Integer = String.Compare( _
'				PrinterDescription, PrinterName, True)
'			' Übergebenen Drucker mit vorhandenen Druckern vergleichen.
'			' Stimmt der übergebene Drucker mit dem Vergleich überein 
'			' wird der übergebene Drucker...
'			If Compared = 0 Then
'				' ... als Standarddrucker systemweit gesetzt
'				Dim mbo As Management.ManagementBaseObject = _
'					Printer.InvokeMethod(DefaultPrinter, Nothing, Nothing)
'				' Ist das Rückgabeergebnis = 0 gibt die Funktion...
'				If CType(mbo.Properties(ReturnValue).Value, Int32) = 0 Then
'					' True zurueck
'					Return True
'				End If
'			End If
'		Next
'	Catch ex As Exception
'		' Eventuell auftretenden Fehler abfangen 
'		' Fehlermeldung ausgeben
'		MessageBox.Show(ex.Message.ToString(), "Info")
'	End Try
'	Return False
'End Function

'Function DefaultPrinterName() As String
'	Dim oPS As New System.Drawing.Printing.PrinterSettings

'	Try
'		DefaultPrinterName = oPS.PrinterName

'	Catch ex As System.Exception
'		DefaultPrinterName = ""
'	Finally
'		oPS = Nothing
'	End Try

'End Function

'Sub SendFax_1(ByVal strFullFilename As String, ByVal strSubject As String)
'Dim strFaxServerName As String = ClsSystem.GetFaxServer()
'Dim strDavidServerName As String = ClsSystem.GetDavidServer()
'Dim lJobID As Long

'Try

'  If ClsSystem.GetLogedUSNr() = 1 Then MsgBox("Der Vorgang wird gestartet...")

'  Dim faxDoc As FAXCOMEXLib.FaxDocument
'  Dim faxServ As FAXCOMEXLib.FaxServer

'  If ClsSystem.GetLogedUSNr() = 1 Then MsgBox("Beendet_1...")



'  faxDoc = New FAXCOMEXLib.FaxDocument
'  faxServ = New FAXCOMEXLib.FaxServer


'  'Dim faxDoc As

'  Try
'    strFullFilename = ClsSystem.GetPersonalFolder & "c4b.txt"

'    faxDoc.Body = strFullFilename
'    If ClsSystem.GetLogedUSNr() = 1 Then MsgBox(strFullFilename)

'    faxDoc.DocumentName = strFullFilename
'    faxDoc.Priority = FAX_PRIORITY_TYPE_ENUM.fptHIGH      ' Newsgroups
'    If ClsSystem.GetLogedUSNr() = 1 Then MsgBox("Connected_2...")

'    If File.Exists(strFullFilename) Then

'      faxDoc.Recipients.Add(regex.KdZEmail, regex.KdzVorname & " " & regex.KdzNachname)               ' Fax Number
'      If ClsSystem.GetLogedUSNr() = 1 Then MsgBox("Connected_3...")

'      faxDoc.ReceiptType() = FAX_RECEIPT_TYPE_ENUM.frtMAIL
'      If ClsSystem.GetLogedUSNr() = 1 Then MsgBox("Connected_4...")

'      'faxDoc.Sender.StreetAddress = regex.USMDStrasse
'      'faxDoc.Sender.Company = regex.USMDname
'      'faxDoc.Sender.Department = regex.USMDname2
'      'faxDoc.Sender.HomePhone = ""
'      'faxDoc.Sender.Name = regex.USVorname & " " & regex.USNachname
'      'faxDoc.Sender.OfficeLocation = regex.USMDOrt
'      'faxDoc.Sender.OfficePhone = regex.USMDTelefon
'      'faxDoc.Sender.Title = ""

'      faxDoc.Subject = regex.OfferBez
'      If ClsSystem.GetLogedUSNr() = 1 Then MsgBox("Connected_5...")

'      faxDoc.Sender.Name = regex.USMDname
'      If ClsSystem.GetLogedUSNr() = 1 Then MsgBox("Connected_6...")

'      faxDoc.ScheduleType = FAX_SCHEDULE_TYPE_ENUM.fstNOW
'      If ClsSystem.GetLogedUSNr() = 1 Then MsgBox("Connected_7...")


'      faxServ.Disconnect()
'      If ClsSystem.GetLogedUSNr() = 1 Then MsgBox("Disconnected_1..." & vbCrLf & strFaxServerName)
'      faxServ.Connect(strFaxServerName)
'      If ClsSystem.GetLogedUSNr() = 1 Then MsgBox("Connected_1..." & vbCrLf & strFaxServerName)

'      '          faxdoc.
'      lJobID = CLng(faxDoc.ConnectedSubmit(faxServ))
'      If ClsSystem.GetLogedUSNr() = 1 Then MsgBox("Submitted...")

'    End If

'  Catch ex As Exception
'    MsgBox(ex.Message.ToString & vbCrLf & ex.GetBaseException.ToString, MsgBoxStyle.Critical, "SendFax_1")

'  End Try
'  faxDoc = Nothing
'  faxServ.Disconnect()

'Catch ex As Exception
'  MsgBox(ex.Message.ToString & vbCrLf & ex.GetBaseException.ToString, MsgBoxStyle.Critical, "SendFax_2")

'Finally

'End Try

'End Sub

'	Function GetRecipientFaxField() As String
'		Dim strFaxField As String = "KDTelefax"

'		Try
'			strFaxField = _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms", "frmOFFMailing / KDOffFields_1")


'		Catch ex As Exception
'			MsgBox(ex.Message, MsgBoxStyle.Critical, "GetRecipientFaxField_0")

'		End Try

'		Return strFaxField
'	End Function

'#End Region


'#Region "Kontaktnummern..."

'	Function GetNewKontaktNr(ByVal lKDNr As Integer, ByVal lKDZNr As Integer) As Integer
'		Dim lRecNr As Integer = 1
'		Dim Conn As New SqlConnection(strConnString)
'		Conn.Open()

'		Dim sSql As String = "Select Top 1 ID From KD_KontaktTotal Order By ID Desc"
'		Dim SQLOffCmd As SqlCommand = New SqlCommand(sSql, Conn)
'		Dim rTemprec As SqlDataReader = SQLOffCmd.ExecuteReader					 ' Offertendatenbank

'		rTemprec.Read()
'		If rTemprec.HasRows Then
'			lRecNr = CInt(rTemprec("ID").ToString) + 1
'		Else
'			lRecNr = 1
'		End If

'		rTemprec.Close()
'		Conn.Close()
'		Return lRecNr
'	End Function

'	Function GetNeweMailKontaktNr() As Integer
'		Dim lRecNr As Integer = 1
'		Dim Conn As New SqlConnection(strConnString)

'		Try
'			Conn.Open()

'			Dim sSql As String = "Select Top 1 RecNr From [{0}].dbo.Mail_Kontakte Order By RecNr Desc"
'			sSql = String.Format(sSql, ClsDataDetail.GetMailDbName)
'			Dim SQLOffCmd As SqlCommand = New SqlCommand(sSql, Conn)
'			Dim rTemprec As SqlDataReader = SQLOffCmd.ExecuteReader

'			rTemprec.Read()
'			If rTemprec.HasRows Then
'				lRecNr = CInt(rTemprec("RecNr").ToString) + 1
'			Else
'				lRecNr = 1
'			End If

'			rTemprec.Close()
'			Conn.Close()

'		Catch ex As Exception
'			MsgBox(ex.Message, MsgBoxStyle.Critical, "GetNeweMailKontaktNr_0")

'		End Try

'		Return lRecNr
'	End Function

'#End Region

'Sub GetUSData_0()
'  '    Dim regex As New ClsDivFunc
'  Dim strUSKst As String = String.Empty
'  Dim strUSNachname As String = String.Empty
'  Dim iUSNr As Integer = 0
'  Dim Conn As New SqlConnection(strConnString)
'  Conn.Open()

'  Dim sSql As String = "Select Top 1 USNr, Anrede As USAnrede, Nachname As USNachname, Vorname As USVorname, "
'  sSql &= "eMail As USeMail, Telefon As USTelefon, Telefax As USTelefax From Benutzer "
'  sSql &= "Where KST = "
'  sSql &= String.Format("(Select Of_Berater From Offers Where OfNr = {0})", ClsDataDetail.GetOffNr)
'  Dim SQLOffCmd As SqlCommand = New SqlCommand(sSql, Conn)
'  Dim rTemprec As SqlDataReader = SQLOffCmd.ExecuteReader          ' Offertendatenbank

'  Try

'    rTemprec.Read()
'    With regex
'      .USAnrede = rTemprec("USAnrede").ToString
'      .USeMail = rTemprec("USeMail").ToString
'      .USNachname = rTemprec("USNachname").ToString
'      .USVorname = rTemprec("USVorname").ToString
'      .USTelefon = rTemprec("USTelefon").ToString
'      .USTelefax = rTemprec("USTelefax").ToString

'      iUSNr = CInt(rTemprec("USNr").ToString)
'    End With
'    rTemprec.Close()

'    Dim sMDSql As String = "[Get MDData For Header] " & Year(Now) & ", " & iUSNr
'    SQLOffCmd = New SqlCommand(sMDSql, Conn)
'    rTemprec = SQLOffCmd.ExecuteReader          ' Offertendatenbank
'    rTemprec.Read()
'    With regex
'      .USMDname = rTemprec("MDName").ToString
'      .USMDname2 = rTemprec("MDName2").ToString
'      .USMDPostfach = rTemprec("MDPostfach").ToString
'      .USMDStrasse = rTemprec("MDStrasse").ToString
'      .USMDPlz = rTemprec("MDPLZ").ToString
'      .USMDOrt = rTemprec("MDOrt").ToString
'      .USMDLand = rTemprec("MDLand").ToString

'      .USMDTelefon = rTemprec("MDTelefon").ToString
'      .USMDTelefax = rTemprec("MDTelefax").ToString
'      .USMDeMail = rTemprec("MDeMail").ToString
'      .USMDHomepage = rTemprec("MDHomepage").ToString

'    End With
'    rTemprec.Close()

'    Conn.Close()
'  Catch ex As Exception
'    MsgBox(ex.Message, MsgBoxStyle.Critical, "GetUSData_0")

'  End Try

'End Sub

'End Module
