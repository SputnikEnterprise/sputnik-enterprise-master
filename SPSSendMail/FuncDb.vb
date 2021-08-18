
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text.StringBuilder
Imports System.Collections.Generic
Imports System.Net
Imports System.Net.Mail

Imports SP.Infrastructure.Logging
Imports SPProgUtility.Mandanten
Imports SPSSendMail.ClsDataDetail
Imports SP.Infrastructure.UI
Imports System.Linq

Module FuncDb
	Private m_Logger As ILogger = New Logger()

	Private regex As New ClsDivFunc

	Private _ClsProgsetting As New SPProgUtility.ClsProgSettingPath
	Private strMDGuid As String = _ClsProgsetting.GetMDGuid
	Private _ClsReg As New SPProgUtility.ClsDivReg
	Private _ClsLog As New SPProgUtility.ClsEventLog

	Private eMailField As String = "ZHDeMail"
	Private m_TemporaryLogingFile As String

	Private m_Utility As New SPProgUtility.MainUtilities.Utilities
	Private m_UtilityUI As New UtilityUI

	Private m_SendFileType As SendFiletype
	Enum SendFiletype
		PresentationDoc
		EmployeeDoc
		BothDoc
	End Enum

	Function OpenConnection(ByVal bSendAsTest As Boolean) As Boolean
		Dim strConnString As String = ClsDataDetail.GetDbConnString()
		Dim Conn As SqlConnection
		Dim iOfferNumber As Integer = ClsDataDetail.GetOffNr
		Dim iKDNr As Integer = ClsDataDetail.GetKDNr
		Dim iKDZNr As Integer = ClsDataDetail.GetZHDNr
		Dim streMailValue As String = String.Empty
		Dim Of_Res7 As String = String.Empty
		Dim Of_Res8 As String = String.Empty
		Dim Of_Bezeichnung As String = String.Empty
		Dim sql As String
		Dim streMailToField As String = ClsDataDetail.GeteMailFieldToSend()
		Dim bResult As Boolean = False

		m_TemporaryLogingFile = ClsDataDetail.GetTempLogFile

		Try
			sql = "Select Offers.OfNr, Offers.OF_Res1, Offers.OF_Res2, Offers.OF_Res3, Offers.OF_Res4, Offers.OF_Res5,"
			sql &= "Offers.OF_Res6, Offers.OF_Res7, Offers.OF_Res8, "
			sql &= "Offers.Of_Slogan, Offers.OF_Gruppe, Offers.OF_Kontakt, Offers.OF_Bezeichnung, "
			sql &= "KD.KDNr, KD.Firma1, KD.eMail As KDeMail, KD.KD_Mail_Mailing, IsNull(KD.Sprache, 'deutsch') As KDSprache,  "
			sql &= "KDZ.Nachname as KDZNachname, "
			sql &= "KDZ.Vorname as KDZVorname, KDZ.Anrede as KDZAnrede, KDZ.Anredeform as KDZAnredeForm, "
			sql &= "KDZ.eMail as ZHDeMail, KDZ.ZHD_Mail_Mailing, KDZ.RecNr As KDZNr "
			sql &= "From Offers, "
			sql &= "Kunden KD Left Join KD_Zustaendig KDZ On KD.KDNr = KDZ.KDNr "
			sql &= String.Format("Where KD.KDNr = {0} And Offers.OfNr = {1} ", iKDNr, iOfferNumber)
			If iKDZNr > 0 Then sql &= String.Format("And KDZ.RecNr = {0}", iKDZNr)
			m_Logger.LogInfo(String.Format("search SQL (OpenConnection): {0}", sql))

			Conn = New SqlConnection(strConnString)
			Conn.Open()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.", ex.ToString))
			_ClsLog.WriteTempLogFile(String.Format("***Fehler (Datenbanken öffnen): {0}", ex.ToString), m_TemporaryLogingFile)

			Return False
		End Try

		Try
			Dim SQLOffCmd As SqlCommand = New SqlCommand(sql, Conn)
			Dim rOffrec As SqlDataReader = SQLOffCmd.ExecuteReader
			rOffrec.Read()
			If Not rOffrec.HasRows Then
				If Not streMailToField.Contains("@") And Not streMailToField.Contains("#UsereMail") Then
					If String.IsNullOrEmpty(rOffrec(streMailToField).ToString) Then
						m_Logger.LogError(String.Format("Leere streMailToField: {0}", streMailToField))

						Dim msg As String = "Keine Daten wurden gefunden.{0}Bitte kontrollieren Sie Ihre Auswahl und versuchen Sie es erneuert."
						msg = String.Format(_ClsProgsetting.TranslateText(msg), vbNewLine)
						_ClsLog.WriteTempLogFile(String.Format("***Fehler: {0}", msg, m_TemporaryLogingFile))

						Conn.Dispose()
						Return False
					End If
				End If

			Else

				Try
					Dim tplFolder = _ClsProgsetting.GetMDTemplatePath
					Dim tplFilename = FileIO.FileSystem.GetName(ClsDataDetail.GetMailTemplateFilename)
					Dim customerLanguage = m_Utility.SafeGetString(rOffrec, "KDSprache").ToUpper
					If String.IsNullOrWhiteSpace(customerLanguage) Then customerLanguage = "D"
					Select Case Mid(customerLanguage, 1, 1)
						Case "I"
							tplFolder = Path.Combine(tplFolder, "I")
						Case "F"
							tplFolder = Path.Combine(tplFolder, "F")

					End Select
					If Not Directory.Exists(tplFolder) Then
						tplFolder = ClsDataDetail.GetMailTemplateFilename
					End If
					tplFilename = Path.Combine(tplFolder, tplFilename)
					If File.Exists(tplFilename) Then
						ClsDataDetail.GetMailTemplateFilename = tplFilename
					End If
					m_Logger.LogInfo(String.Format("tplFilename: {0}", tplFilename))


				Catch ex As Exception
					_ClsLog.WriteTempLogFile(String.Format("***Fehler (Anrede und Voralgen): {0}", ex.ToString), m_TemporaryLogingFile)
					Return False
				End Try

				Of_Res7 = m_Utility.SafeGetString(rOffrec, "Of_Res7")
				Of_Res8 = m_Utility.SafeGetString(rOffrec, "Of_Res8")
				Of_Bezeichnung = m_Utility.SafeGetString(rOffrec, "Of_Bezeichnung")
				If Of_Res7 = String.Empty Then Of_Res7 = Of_Bezeichnung

				With regex
					.Off_Nr = m_Utility.SafeGetInteger(rOffrec, "OfNr", 0)
					.OfferSchluss = m_Utility.SafeGetString(rOffrec, "OF_Res6")
					.OfferNachricht = m_Utility.SafeGetString(rOffrec, "OF_Res8")
					.OfferRes1 = m_Utility.SafeGetString(rOffrec, "OF_Res1")
					.OfferRes2 = m_Utility.SafeGetString(rOffrec, "OF_Res2")
					.OfferRes3 = m_Utility.SafeGetString(rOffrec, "OF_Res3")
					.OfferRes4 = m_Utility.SafeGetString(rOffrec, "OF_Res4")
					.OfferRes5 = m_Utility.SafeGetString(rOffrec, "OF_Res5")

					.OfferWerbe = m_Utility.SafeGetString(rOffrec, "OF_Slogan")
					.OfferGruppe = m_Utility.SafeGetString(rOffrec, "OF_Gruppe")
					.OfferKontakt = m_Utility.SafeGetString(rOffrec, "OF_Kontakt")
					.OfferBez = m_Utility.SafeGetString(rOffrec, "OF_Bezeichnung")

					.Off_KDNr = ClsDataDetail.GetKDNr
					.Off_KDZNr = ClsDataDetail.GetZHDNr

				End With

				Try
					m_Logger.LogDebug(String.Format("SendMailOut is started..."))

					bResult = SendMailOut(rOffrec, streMailToField, Of_Res7, Of_Res8, ClsDataDetail.SendAsHtml, bSendAsTest)
					m_Logger.LogDebug(String.Format("SendMailOut is finished: {0}", bResult))

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.", ex.ToString))
					_ClsLog.WriteTempLogFile(String.Format("***Fehler (OpenConnection_3): {0}", ex.ToString), m_TemporaryLogingFile)
					Return False

				End Try

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.", ex.ToString))
			_ClsLog.WriteTempLogFile(String.Format("***Fehler (OpenConnection_4): {0}", ex.ToString), m_TemporaryLogingFile)

			Return False
		End Try

		Return bResult
	End Function

	Private Function SendMailOut(ByVal rTemprec As SqlDataReader,
									ByVal strMyeMailField As String,
									ByVal strOf_Res7 As String,
									ByVal strOf_Res8 As String,
									ByVal bSendasHtml As Boolean,
									ByVal bSendAsTest As Boolean) As Boolean
		Dim result As Boolean = True
		Dim strFullFilename As String() = ClsDataDetail.GetAttachmentFile()
		Dim assigendUsereMail As String = GeteMailFrom()
		Dim iOfferNumber As Integer = ClsDataDetail.GetOffNr
		Dim iKDNr As Integer = ClsDataDetail.GetKDNr
		Dim iKDZNr As Integer = ClsDataDetail.GetZHDNr
		Dim logEntry As String = String.Empty

		Dim m_md As New Mandant
		Dim strSmtp As String = m_md.GetMailingData(m_md.GetDefaultMDNr, Now.Year).SMTPServer
		If String.IsNullOrWhiteSpace(assigendUsereMail) Then _ClsLog.WriteTempLogFile(String.Format("***Fehler: keine Absender-Adresse gefunden."), m_TemporaryLogingFile)
		If String.IsNullOrWhiteSpace(strSmtp) Then _ClsLog.WriteTempLogFile(String.Format("***Fehler: keine SMTP-Server gefunden."), m_TemporaryLogingFile)
		If String.IsNullOrWhiteSpace(assigendUsereMail) OrElse String.IsNullOrWhiteSpace(strSmtp) Then Return False

		Try
			If Not strMyeMailField.Contains("@") AndAlso Not strMyeMailField.Contains("#UsereMail") Then
				If strMyeMailField = "KD" Then
					strMyeMailField = "KDEMail"
				ElseIf strMyeMailField = "ZHD" Then
					strMyeMailField = "ZHDEMail"
				End If

				If String.IsNullOrEmpty(m_Utility.SafeGetString(rTemprec, strMyeMailField)) AndAlso Not bSendAsTest Then
					m_Logger.LogDebug(String.Format("Leere strMyeMailField: {0}", strMyeMailField))
					'bContinue = False
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Keine EMail-Adresse als Empfänger wurde ausgewählt!"))
					result = False

					Return False

				End If
			End If

			Try
				With regex
					.MailBetreff = strOf_Res7.Trim
					.BodyAsHtml = bSendasHtml
					.KdEmail = m_Utility.SafeGetString(rTemprec, "KDeMail")
					' Zum TESTEN...
					.Off_Nr = m_Utility.SafeGetInteger(rTemprec, "OfNr", 0)
					.Off_KDNr = m_Utility.SafeGetInteger(rTemprec, "KDNr", 0)
					.Off_Firma1 = m_Utility.SafeGetString(rTemprec, "Firma1")
					If .Off_KDZNr > 0 Then .KdZEmail = m_Utility.SafeGetString(rTemprec, "ZHDeMail")
					If Not strMyeMailField.ToUpper.Contains("KDeMail".ToUpper) AndAlso Not strMyeMailField.ToUpper.Contains("#UsereMail".ToUpper) Then
						.Off_KDZNr = m_Utility.SafeGetInteger(rTemprec, "KDZNr", 0)
						If .Off_KDZNr > 0 Then
							.KdzAnredeform = m_Translate.GetSafeTranslationValue(m_Utility.SafeGetString(rTemprec, "KDZAnredeForm"))
							.KdzNachname = m_Utility.SafeGetString(rTemprec, "KDZNachname")
							.KdzVorname = m_Utility.SafeGetString(rTemprec, "KDZVorname")
						End If

					Else
						.KdzAnredeform = String.Empty
						.KdzNachname = String.Empty
						.KdzVorname = String.Empty

					End If

					If .KdzAnredeform & .KdzNachname & .KdzVorname = String.Empty Then
						.KdzNachname = m_Translate.GetSafeTranslationValue("Sehr geehrte Damen und Herren")
					End If

				End With

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))
				_ClsLog.WriteTempLogFile(String.Format("***Fehler (SendMailOut: regex): {0}", ex.ToString), m_TemporaryLogingFile)

				Return False
			End Try

			Try
				result = result AndAlso GetUSData(regex)

				If ((strMyeMailField.Contains("@") OrElse strMyeMailField.Contains("#UsereMail"))) AndAlso bSendAsTest Then
					strMyeMailField = If(String.IsNullOrWhiteSpace(m_InitialData.UserData.UserMDeMail), m_InitialData.UserData.UsereMail, m_InitialData.UserData.UserMDeMail)
				End If

				If bSendAsTest Then
					strOf_Res7 = String.Format("{0} {1}", m_Translate.GetSafeTranslationValue("#Testnachricht#"), strOf_Res7)

				Else
					If String.IsNullOrWhiteSpace(strOf_Res7) Then
						strOf_Res7 = String.Empty
					Else
						strOf_Res7 = regex.ParseTemplateFile(strOf_Res7.Trim)
					End If
				End If
				m_Logger.LogDebug(String.Format("parsed strOf_Res7(subject): {0}", strOf_Res7))

				strOf_Res8 = regex.ParseTemplateFile(String.Empty)        ' Body
				m_Logger.LogInfo(String.Format("parsed strOf_Res8(body)"))
				If String.IsNullOrWhiteSpace(strOf_Res8) Then
					_ClsLog.WriteTempLogFile("***Fehler: leere Nachrichten-Text.", m_TemporaryLogingFile)

					Return False
				End If

				' Überprüfen ob die Nachricht bereits versendet wurde...
				If Not bSendAsTest Then
					If IsMyMessageAlreadySent(rTemprec(strMyeMailField).ToString, strOf_Res7, strOf_Res8, iKDNr, ClsDataDetail.GetTempLogFile, bSendAsTest) Then
						m_Logger.LogWarning(String.Format("Duplikate: iKDNr: {0} | rTemprec(strMyeMailField): {1} strOf_Res7: {2}", iKDNr, rTemprec(strMyeMailField).ToString, strOf_Res7))

						'Dim strMsg As String = "***Duplikat: KDNr: {0} | EMail-Adresse: {1} | Betreff: {2}"
						'strMsg = String.Format(_ClsProgsetting.TranslateText(strMsg), iKDNr, m_Utility.SafeGetString(rTemprec, strMyeMailField), strOf_Res7)
						'_ClsLog.WriteTempLogFile(strMsg, m_TemporaryLogingFile)

						Return False
					End If
				End If

				If ClsDataDetail.SendWithMADoks And ClsDataDetail.SendWithPDoks Then
					m_SendFileType = SendFiletype.BothDoc
				ElseIf ClsDataDetail.SendWithMADoks Then
					m_SendFileType = SendFiletype.EmployeeDoc
				ElseIf ClsDataDetail.SendWithPDoks Then
					m_SendFileType = SendFiletype.PresentationDoc
				End If


				If ClsDataDetail.SendWithPDoks Or ClsDataDetail.SendWithMADoks Then strFullFilename = StoreDataToFs(iOfferNumber, m_SendFileType)
				m_Logger.LogDebug(String.Format("strFullFilename: {0}", strFullFilename))


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.", ex.ToString))
				_ClsLog.WriteTempLogFile(ex.ToString, m_TemporaryLogingFile)

				Return False
			End Try

			Dim strValue As String = String.Empty
			Dim strToAddress As String = ""
			If Not strMyeMailField.Contains("@") Then
				strToAddress = rTemprec(strMyeMailField).ToString
			Else
				strToAddress = strMyeMailField
			End If
			If bSendAsTest Then strToAddress = assigendUsereMail

			m_Logger.LogInfo(String.Format("SendMailToKD wird gestartet... strSmtp: {0} | assigendUsereMail: {1} | strToAddress: {2}", strSmtp, assigendUsereMail, strToAddress))
			'#If Not DEBUG Then
			result = result AndAlso SendMailToWithExchange(ClsDataDetail.SendAsHtml,
											assigendUsereMail,
											strToAddress,
											strOf_Res7,
											strOf_Res8,
											1,
											strFullFilename,
											strSmtp,
											regex.Exchange_USName, regex.Exchange_USPW)
			'#End If
			'_ClsLog.WriteTempLogFile(String.Format("Mail-Versand: {0} >>> strToAddress: {1}", If(result, "OK", "Fehlerhaft"), strToAddress), m_TemporaryLogingFile)
			logEntry = String.Format("Mail-Versand: {0} >>> An: {1}", If(result, "OK", "Fehlerhaft"), strToAddress)

			If Not result Then
				_ClsLog.WriteTempLogFile(logEntry, m_TemporaryLogingFile)
				Return result
			End If

			Dim bodyAdditional As String = String.Format("Offertennummer: {1}{0}{2}", vbNewLine, iOfferNumber, strOf_Res8)
			If Not bSendAsTest Then
				m_Logger.LogDebug(String.Format("CreateLogToKDKontaktDb wird gestartet... iKDNr: {0} | iKDZNr: {1}", iKDNr, iKDZNr, strToAddress))
				result = result AndAlso CreateLogToKDKontaktDb(iKDNr, iKDZNr, bodyAdditional)
				'_ClsLog.WriteTempLogFile(String.Format("Kontakt-Eintrag: {0} >>> KDNr: {1} | iKDZNr: {2}", If(result, "OK", "Fehlerhaft"), iKDNr, iKDZNr), m_TemporaryLogingFile)
				logEntry &= If(String.IsNullOrWhiteSpace(logEntry), "", vbNewLine) & String.Format("Kontakt-Eintrag: {0} >>> KDNr: {1} | KDZNr: {2}", If(result, "OK", "Fehlerhaft"), iKDNr, iKDZNr)

			End If

			If result Then m_Logger.LogDebug(String.Format("CreateLogToMailKontaktDb wird gestartet... iKDNr: {0} | iKDZNr: {1}", iKDNr, iKDZNr, strToAddress))
			result = result AndAlso CreateLogToMailKontaktDb(iKDNr, iKDZNr, 0, strFullFilename, bSendasHtml, strToAddress, assigendUsereMail, strOf_Res7, bodyAdditional, bSendAsTest)
			'_ClsLog.WriteTempLogFile(String.Format("Mail-Eintrag: {0} >>> KDNr: {1} | iKDZNr: {2}", If(result, "OK", "Fehlerhaft"), iKDNr, iKDZNr), m_TemporaryLogingFile)
			logEntry &= If(String.IsNullOrWhiteSpace(logEntry), "", vbNewLine) & String.Format("Mail-Eintrag: {0} >>> KDNr: {1} | KDZNr: {2}", If(result, "OK", "Fehlerhaft"), iKDNr, iKDZNr)

			_ClsLog.WriteTempLogFile(logEntry, m_TemporaryLogingFile)

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.", ex.ToString))
			_ClsLog.WriteTempLogFile(String.Format("{0}", ex.ToString), m_TemporaryLogingFile)

			Return False
		End Try


		Return result

	End Function

	Function StoreDataToFs(ByVal lOFNr As Integer, ByVal justPFiles As SendFiletype) As String()
		Dim Conn As New SqlConnection(ClsDataDetail.GetDbConnString)
		Dim strFullFilename As String() = New String() {""}
		Dim strFiles As String = String.Empty
		Dim bCreateFile As Boolean = False
		Dim sql As String = "Select DocScan, MANr, Bezeichnung, ScanExtension From OFF_Doc Where "

		' Achtung: nicht Kandidatendokumente werden ausgewählt. Es sind allgemeine Dokumente, welches sich auf Offerte beziehen!
		sql &= String.Format("OfNr = {0} ", lOFNr)

		If justPFiles = SendFiletype.EmployeeDoc Then
			sql &= "And (MANr Is Not Null And MANr <> 0)"

		ElseIf justPFiles = SendFiletype.PresentationDoc Then
			sql &= "And (MANr Is Null Or MANr = 0)"

		End If


		Dim i As Integer = 0

		Conn.Open()
		Dim SQLCmd As SqlCommand = New SqlCommand(sql, Conn)
		Dim rOfDoc As SqlDataReader = SQLCmd.ExecuteReader

		Try
			While rOfDoc.Read
				ClsDataDetail.IsAttachedFileInd = True

				bCreateFile = True
				If bCreateFile Then
					Dim filename = (System.IO.Path.GetFileName(rOfDoc("Bezeichnung").ToString) & If(rOfDoc("Bezeichnung").ToString.Contains("\"), "", "." & rOfDoc("ScanExtension").ToString))
					Dim strSelectedFile As String = Path.Combine(_ClsProgsetting.GetSpSFiles2DeletePath, ClsDataDetail.GetMessageGuid, filename)

					strFiles &= If(strFiles <> String.Empty, ";", "") & strSelectedFile

					If Not File.Exists(strSelectedFile) Then
						Try
							Dim BA As Byte() = Nothing
							Dim BA_1 As Byte() = Nothing
							BA = CType(rOfDoc("DocScan"), Byte())

							Try
								If Not Directory.Exists(_ClsProgsetting.GetSpSFiles2DeletePath & ClsDataDetail.GetMessageGuid) Then
									Directory.CreateDirectory(_ClsProgsetting.GetSpSFiles2DeletePath & ClsDataDetail.GetMessageGuid)
								End If

							Catch ex As Exception
								m_Logger.LogError(String.Format("{0}.", ex.ToString))

							End Try

							Dim ArraySize As New Integer
							ArraySize = BA.GetUpperBound(0)

							If File.Exists(strSelectedFile) Then File.Delete(strSelectedFile)
							Dim fs As New FileStream(strSelectedFile, FileMode.CreateNew)
							fs.Write(BA, 0, ArraySize + 1)
							fs.Close()
							fs.Dispose()
							_ClsLog.WriteTempLogFile(String.Format("***Datei wurde neu angelegt (StoreDataToFs_2): {0}", strSelectedFile))
							i += 1


						Catch ex As Exception
							m_Logger.LogError(String.Format("{0}.", ex.ToString))
							_ClsLog.WriteTempLogFile(String.Format("{0}", ex.ToString), m_TemporaryLogingFile)
						End Try

					End If

				End If

			End While

			ReDim strFullFilename(i - 1)
			If ClsDataDetail.GetAttachmentFile.Length >= 1 Then
				If ClsDataDetail.GetAttachmentFile(0) <> String.Empty Then
					strFiles &= ";" & ClsDataDetail.GetAttachmentFile(0)
				End If
			End If
			strFullFilename = strFiles.Split(CChar(";"))


			rOfDoc.Close()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.", ex.ToString))
			_ClsLog.WriteTempLogFile(String.Format("{0}", ex.ToString), m_TemporaryLogingFile)

		End Try

		Return strFullFilename
	End Function


	Function CreateLogToKDKontaktDb(ByVal lKDNr As Integer,
														 ByVal lZHDNr As Integer,
														 Optional ByVal MailBody As String = ""
														 ) As Boolean
		Dim result As Boolean = False
		Dim Time_1 As Double = System.Environment.TickCount


		Dim strUSName As String = _ClsProgsetting.GetUserName()
		Dim Conn As New SqlConnection(ClsDataDetail.GetDbConnString)
		Dim sKDZSql As String = "Insert Into KD_KontaktTotal (KDNr, KDZNr, RecNr, KontaktDate, Kontakte, "
		sKDZSql &= "KontaktType1, KontaktType2, Kontaktwichtig, KontaktDauer, KontaktErledigt, MANr, "
		sKDZSql &= "CreatedOn, CreatedFrom) Values (@KDNr, @ZHDNr, @RecNr, @KontaktDate, "
		sKDZSql &= "@body, 'Serienmail', 2, 0, @KontaktDauer, 0, 0, @KontaktDate, @USName)"
		Dim lNewRecNr As Integer

		Try
			Conn.Open()
			lNewRecNr = GetNewKontaktNr(lKDNr, lZHDNr)

			Dim rKontaktrec As New SqlDataAdapter()

			rKontaktrec.SelectCommand = New SqlCommand(sKDZSql, Conn)
			rKontaktrec.SelectCommand.Parameters.AddWithValue("@KDNr", lKDNr)
			rKontaktrec.SelectCommand.Parameters.AddWithValue("@ZHDNr", lZHDNr)
			rKontaktrec.SelectCommand.Parameters.AddWithValue("@RecNr", lNewRecNr)


			If String.IsNullOrWhiteSpace(MailBody) Then MailBody = m_Translate.GetSafeTranslationValue("Einde Offerte wurde geschickt")
			rKontaktrec.SelectCommand.Parameters.AddWithValue("@body", MailBody)

			Dim subject As String = m_Translate.GetSafeTranslationValue("Eine Offerte wurde geschickt")
			rKontaktrec.SelectCommand.Parameters.AddWithValue("@KontaktDauer", subject)

			rKontaktrec.SelectCommand.Parameters.AddWithValue("@KontaktDate", Now.ToString("G"))
			rKontaktrec.SelectCommand.Parameters.AddWithValue("@USName", strUSName)

			'_ClsProgsetting.
			Dim dt As DataTable = New DataTable()
			rKontaktrec.Fill(dt)

			result = True

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		Finally
			Conn.Close()

		End Try

		Return result

	End Function

	Function CreateLogToMailKontaktDb(ByVal lKDNr As Integer, ByVal lZHDNr As Integer,
																ByVal lMANr As Integer, ByVal strFilename As String(),
																ByVal bSendAsHtml As Boolean, Optional ByVal MailTo As String = "",
																Optional ByVal MailFrom As String = "",
																Optional ByVal MailSubject As String = "",
																Optional ByVal MailBody As String = "",
															 Optional ByVal bSendAsTest As Boolean = False,
															 Optional ByVal bAsTelefax As Boolean = False) As Boolean
		Dim result As Boolean = False
		Dim Time_1 As Double = System.Environment.TickCount
		Dim strUSName As String = _ClsProgsetting.GetUserName()
		Dim Conn As New SqlConnection(ClsDataDetail.GetDbConnString)
		Dim sMailSql As String = "Insert Into [{0}].dbo.Mail_Kontakte (KDNr, KDZNr, RecNr, MANr, Message_ID, eMail_To, "
		sMailSql &= "eMail_From, eMail_Subject, eMail_Body, eMail_smtp, AsHtml, AsTelefax, Customer_ID, "
		sMailSql &= "CreatedOn, CreatedFrom) Values (@KDNr, @ZHDNr, @RecNr, @MANr, @Message_ID, @eMailTo, @eMailFrom, "
		sMailSql &= "@eMailsubject, @eMailbody, @eMailSmtp, @SendAsHtml, @AsTelefax, @Customer_ID, @KontaktDate, @USName)"
		sMailSql = String.Format(sMailSql, ClsDataDetail.GetMailDbName)
		Dim lNewRecNr As Integer

		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
		Dim param As System.Data.SqlClient.SqlParameter

		Try
			Conn.Open()
			lNewRecNr = GetNeweMailKontaktNr()

			cmd.CommandType = CommandType.Text
			cmd.CommandText = sMailSql

			param = cmd.Parameters.AddWithValue("@KDNr", lKDNr)
			param = cmd.Parameters.AddWithValue("@ZHDNr", lZHDNr)
			param = cmd.Parameters.AddWithValue("@RecNr", lNewRecNr)

			param = cmd.Parameters.AddWithValue("@MANr", lMANr)

			param = cmd.Parameters.AddWithValue("@Message_ID", ClsDataDetail.GetMessageGuid)
			param = cmd.Parameters.AddWithValue("@eMailTo", MailTo)
			param = cmd.Parameters.AddWithValue("@eMailFrom", MailFrom)
			param = cmd.Parameters.AddWithValue("@eMailsubject", MailSubject)
			param = cmd.Parameters.AddWithValue("@eMailbody", MailBody)
			param = cmd.Parameters.AddWithValue("@eMailSmtp", _ClsProgsetting.GetSmtpServer())
			param = cmd.Parameters.AddWithValue("@SendAsHtml", bSendAsHtml)
			param = cmd.Parameters.AddWithValue("@AsTelefax", bAsTelefax)

			param = cmd.Parameters.AddWithValue("@Customer_ID", strMDGuid)
			param = cmd.Parameters.AddWithValue("@KontaktDate", Now.ToString("G"))
			param = cmd.Parameters.AddWithValue("@USName", strUSName)

			cmd.Connection = Conn
			cmd.ExecuteNonQuery()
			result = True

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		Finally
			cmd.Dispose()
			Conn.Close()

		End Try

		Return result

	End Function

	Sub InsertBinaryToMailDb(ByVal lRecNr As Integer, ByVal strFilename As String(),
												Optional ByVal MailTo As String = "", Optional ByVal MailFrom As String = "",
												Optional ByVal MailSubject As String = "")
		Dim Time_1 As Double = System.Environment.TickCount
		Dim strUSName As String = _ClsProgsetting.GetUserName()
		Dim Conn As New SqlConnection(ClsDataDetail.GetDbConnString)

		Dim sMailSql As String = "Insert Into [{0}].dbo.Mail_FileScan (RecNr, Message_ID, eMail_To, "
		sMailSql &= "eMail_From, eMail_Subject, "
		sMailSql &= "ScanFile, Filename, Customer_ID, CreatedOn, CreatedFrom) Values "
		sMailSql &= "(@RecNr, @Message_ID, @eMailTo, @eMailFrom, "
		sMailSql &= "@eMailsubject, @BinaryFile, @FileName, @Customer_ID, @KontaktDate, @USName)"
		sMailSql = String.Format(sMailSql, ClsDataDetail.GetMailDbName)

		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
		Dim param As System.Data.SqlClient.SqlParameter
		Dim bAllreadyOpen As Boolean = False

		Try
			Conn.Open()
			For i As Integer = 0 To strFilename.Length - 1
				If strFilename(i) <> String.Empty Then
					Dim fi As New System.IO.FileInfo(strFilename(i))
					Dim fs As System.IO.FileStream = fi.OpenRead
					Dim CheckFile As New FileInfo(strFilename(i))

					Dim lBytes As Integer = CInt(fs.Length)
					Dim myImage(lBytes) As Byte

					fs.Read(myImage, 0, lBytes)
					fs.Close()
					fs.Dispose()

					Try
						cmd.CommandType = CommandType.Text
						cmd.CommandText = sMailSql

						param = cmd.Parameters.AddWithValue("@RecNr", lRecNr)
						param = cmd.Parameters.AddWithValue("@Message_ID", ClsDataDetail.GetMessageGuid)
						param = cmd.Parameters.AddWithValue("@eMailTo", MailTo)
						param = cmd.Parameters.AddWithValue("@eMailFrom", MailFrom)
						param = cmd.Parameters.AddWithValue("@eMailsubject", MailSubject)
						param = cmd.Parameters.AddWithValue("@BinaryFile", myImage)
						param = cmd.Parameters.AddWithValue("@FileName", CheckFile.Name)
						param = cmd.Parameters.AddWithValue("@Customer_ID", strMDGuid)
						param = cmd.Parameters.AddWithValue("@KontaktDate", Now.ToString("G"))
						param = cmd.Parameters.AddWithValue("@USName", strUSName)

						'            If i = 0 Then cmd.Connection = Conn
						If Not bAllreadyOpen Then
							cmd.Connection = Conn
							bAllreadyOpen = True
						End If

						cmd.ExecuteNonQuery()

						cmd.Parameters.Clear()

					Catch ex As Exception
						m_Logger.LogError(String.Format("{0}.", ex.ToString))
						_ClsLog.WriteTempLogFile(String.Format("***Fehler (InsertBinaryToMailDb_1): {0}{1}{2}{1}I = {3}",
																									 ex.ToString,
																									 vbNewLine, ex.StackTrace, i))

					End Try
				End If
			Next

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.", ex.ToString))
			_ClsLog.WriteTempLogFile(String.Format("***Fehler (InsertBinaryToMailDb_2): {0}", ex.ToString))

		Finally
			cmd.Dispose()
			Conn.Close()

		End Try

		Dim Time_2 As Double = System.Environment.TickCount
		Console.WriteLine("Zeit für InsertBinaryToMailDb: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")

	End Sub

	Function GeteMailFrom() As String
		Dim result = m_InitialData.UserData.UserMDeMail
		If String.IsNullOrWhiteSpace(result) Then result = m_InitialData.UserData.UsereMail

		If String.IsNullOrWhiteSpace(result) Then
			m_Logger.LogError(String.Format("GeteMailFrom: email address is empty."))
			m_UtilityUI.ShowErrorDialog("Der Absender wurde nicht gefunden! Bitte definieren Sie eine EMail-Adresse in der Benutzerverwaltung.")
		End If

		Return result

	End Function

	Function GeteMail2FaxFrom() As String
		Dim strMyValue As String = String.Empty

		Try
			strMyValue = _ClsReg.GetINIString(_ClsProgsetting.GetMDIniFile(), "Mailing", "Fax-Forwarder", "").ToString      ' @fax.ip-plus.net

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.", ex.ToString))
			_ClsLog.WriteTempLogFile(String.Format("***Fehler (GeteMail2FaxFrom): {0}", ex.ToString))

		End Try

		Return strMyValue
	End Function

	Sub GetOffMailingValue()

		Try
			eMailField = _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms", "frmOFFMailing / KDOffFields_1")

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.", ex.ToString))
			_ClsLog.WriteTempLogFile(String.Format("***Fehler (GetOffMailingValue): {0}", ex.ToString))

		End Try

	End Sub

	Function GetFaxExtension() As String
		Dim strMyValue As String = String.Empty

		Try
			strMyValue = _ClsReg.GetINIString(_ClsProgsetting.GetMDIniFile(), "Mailing", "Fax-Extension", "").ToString      ' @fax.ip-plus.net

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.", ex.ToString))
			_ClsLog.WriteTempLogFile(String.Format("***Fehler (GetFaxExtension): {0}", ex.ToString))

		End Try

		Return strMyValue
	End Function

	Function SendMailToWithExchange(ByVal bIsHtml As Boolean,
																	ByVal strFrom As String,
																	ByVal strTo As String,
																	ByVal strSubject As String,
																	ByVal strBody As String,
																	ByVal iPriority As Integer,
																	ByVal aAttachmentFile As String(),
																	ByVal strSmtp As String,
																	ByVal strUserName As String,
																	ByVal strUserPW As String) As Boolean
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim obj As System.Net.Mail.SmtpClient = New System.Net.Mail.SmtpClient
		Dim mailmsg As New System.Net.Mail.MailMessage
		Dim result As Boolean = True
		Dim strToAdresses = strTo.Split(CChar(";")).ToList()
		Dim m_md As New Mandant
		Dim m_EnableSSL As Boolean = m_md.GetMailingData(m_md.GetDefaultMDNr, Now.Year).EnableSSL
		Dim m_SmtpPort As Integer = m_md.GetMailingData(m_md.GetDefaultMDNr, Now.Year).SMTPPort

		bIsHtml = True

		Try
			Dim strEx_UserName As String = String.Empty
			Dim strEx_UserPW As String = String.Empty
			strEx_UserName = strUserName
			strEx_UserPW = strUserPW

#If DEBUG Then
			strSmtp = "smtpserver"
			strEx_UserName = "username"
			strEx_UserPW = "password"
			m_SmtpPort = 587
			m_EnableSSL = True

			strFrom = "info@domain.com"
			strToAdresses = New List(Of String) From {"user@domain.com"}
#End If

			With mailmsg
				.IsBodyHtml = bIsHtml
				.To.Clear()
				.From = New MailAddress(strFrom)
				'.To.Add(New MailAddress(strToAdresses(0).Trim))
				For Each toItem In strToAdresses
					If Not String.IsNullOrWhiteSpace(toItem) Then .To.Add(New MailAddress(toItem.Trim))
				Next
				.ReplyToList.Clear()
				.ReplyToList.Add(.From)

				.Subject = strSubject.Trim()
				.Body = strBody.Trim()
				Try
					' die Imagevariablen umsetzen...
					Dim strImgVar1 As String = _ClsProgsetting.GetMDProfilValue("Templates", "eMailImageVar1", "")
					Dim strImgValue1 As String = _ClsProgsetting.GetMDProfilValue("Templates", "eMailImageValue1", "")
					Dim strImgVar2 As String = _ClsProgsetting.GetMDProfilValue("Templates", "eMailImageVar2", "")
					Dim strImgValue2 As String = _ClsProgsetting.GetMDProfilValue("Templates", "eMailImageValue2", "")
					Dim strImgVar3 As String = _ClsProgsetting.GetMDProfilValue("Templates", "eMailImageVar3", "")
					Dim strImgValue3 As String = _ClsProgsetting.GetMDProfilValue("Templates", "eMailImageValue3", "")

					If strImgVar1 <> String.Empty Then .Body = .Body.Replace(strImgVar1, String.Format("<{0}>", strImgValue1))
					If strImgVar2 <> String.Empty Then .Body = .Body.Replace(strImgVar2, String.Format("<{0}>", strImgValue2))
					If strImgVar3 <> String.Empty Then .Body = .Body.Replace(strImgVar3, String.Format("<{0}>", strImgValue3))


				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.", ex.ToString))

				End Try

				Select Case iPriority
					Case 0
						.Priority = Net.Mail.MailPriority.Low
					Case 1
						.Priority = Net.Mail.MailPriority.Normal
					Case 2
						.Priority = Net.Mail.MailPriority.High
				End Select
				If aAttachmentFile.Length <> 0 Then
					For i As Integer = 0 To aAttachmentFile.Length - 1
						If File.Exists(aAttachmentFile(i)) Then
							.Attachments.Add(New System.Net.Mail.Attachment(aAttachmentFile(i)))
						End If
					Next
				End If

			End With


			Try
				If strEx_UserName <> String.Empty Then
					strEx_UserName = _ClsProgsetting.DecryptBase64String(strEx_UserName)
					strEx_UserPW = _ClsProgsetting.DecryptBase64String(strEx_UserPW)

					Dim mailClient As New System.Net.Mail.SmtpClient(strSmtp, m_SmtpPort)
					mailClient.Credentials = New NetworkCredential(strEx_UserName, strEx_UserPW)
					mailClient.DeliveryMethod = Net.Mail.SmtpDeliveryMethod.Network
					mailClient.EnableSsl = m_EnableSSL

					mailClient.Send(mailmsg)
					mailClient.Dispose()

				Else
					obj.Host = strSmtp
					obj.Send(mailmsg)

				End If

				result = True
				m_Logger.LogDebug(String.Format("SMTPServer: {0} >>> SMTP-Port: {1} >>> SSL: {2} | From: {3} >>  AN: {4}  | Message: {5}", strSmtp, m_SmtpPort, m_EnableSSL, strTo, strFrom, result))


			Catch ex As Exception
				m_Logger.LogError(String.Format("SMTPServer: {0} >>> SMTP-Port: {1} >>> SSL: {2} | From: {3} >>  AN: {4}  | Message: {5}", strSmtp, m_SmtpPort, m_EnableSSL, strTo, strFrom, ex.ToString()))
				result = False

			Finally
				obj.Dispose()
				mailmsg.Attachments.Dispose()
				mailmsg.Dispose()

			End Try
		Catch ex As Exception
			m_Logger.LogError(String.Format("SMTPServer: {0} >>> SMTP-Port: {1} >>> SSL: {2} | From: {3} >>  AN: {4}  | Message: {5}", strSmtp, m_SmtpPort, m_EnableSSL, strTo, strFrom, ex.ToString()))
			result = False

		End Try

		Return result
	End Function

	Function GetNewKontaktNr(ByVal lKDNr As Integer, ByVal lKDZNr As Integer) As Integer
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim lRecNr As Integer = 1
		Dim Conn As New SqlConnection(ClsDataDetail.GetDbConnString)
		Conn.Open()

		Dim sSql As String = "Select Top 1 ID From KD_KontaktTotal Order By ID Desc"
		Try
			Dim SQLOffCmd As SqlCommand = New SqlCommand(sSql, Conn)
			Dim rTemprec As SqlDataReader = SQLOffCmd.ExecuteReader

			rTemprec.Read()
			If rTemprec.HasRows Then
				lRecNr = CInt(rTemprec("ID").ToString) + 1
			Else
				lRecNr = 1
			End If
			rTemprec.Close()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.", ex.ToString))

		Finally
			Conn.Close()

		End Try

		Return lRecNr
	End Function

	Function GetNeweMailKontaktNr() As Integer
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim lRecNr As Integer = 1
		Dim Conn As New SqlConnection(ClsDataDetail.GetDbConnString)
		Conn.Open()

		Try
			Dim sSql As String = "Select Top 1 RecNr From [{0}].dbo.Mail_Kontakte Order By RecNr Desc"
			sSql = String.Format(sSql, ClsDataDetail.GetMailDbName)
			Dim SQLOffCmd As SqlCommand = New SqlCommand(sSql, Conn)
			Dim rTemprec As SqlDataReader = SQLOffCmd.ExecuteReader

			rTemprec.Read()
			If rTemprec.HasRows Then
				lRecNr = CInt(rTemprec("RecNr").ToString) + 1
			Else
				lRecNr = 1
			End If
			rTemprec.Close()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.", ex.ToString))

		Finally
			Conn.Close()

		End Try

		Return lRecNr
	End Function




	Function LoadUserNumberFromOfferData(ByVal offerNumber As Integer) As Integer?
		Dim m_Utility As New SPProgUtility.MainUtilities.Utilities
		Dim result As Integer?

		Dim SQL As String = String.Empty

		SQL &= "Select Top 1 USNr From Benutzer "
		SQL &= "Where KST = (Select Of_Berater From Offers Where OfNr = @OffNr) "

		Dim listOfParams As New List(Of SqlClient.SqlParameter)
		listOfParams.Add(New SqlClient.SqlParameter("OffNr", offerNumber))


		Dim reader As SqlClient.SqlDataReader = m_Utility.OpenReader(m_InitialData.MDData.MDDbConn, SQL, listOfParams, CommandType.Text)

		Try

			If (Not reader Is Nothing) Then

				While reader.Read()

					result = m_Utility.SafeGetInteger(reader, "USNr", Nothing)

				End While

			End If

		Catch e As Exception
			result = Nothing
			m_Logger.LogError(e.ToString())

		End Try

		Return result

	End Function

	Function LoadOfferUSData(ByVal userNumber As Integer?) As UserData
		Dim result As UserData = Nothing
		Dim m_Utility As New SPProgUtility.MainUtilities.Utilities

		Dim SQL As String = String.Empty
		SQL = "[Get USData 4 Templates With USNumber]"

		Dim listOfParams As New List(Of SqlClient.SqlParameter)
		listOfParams.Add(New SqlClient.SqlParameter("USNr", m_Utility.ReplaceMissing(userNumber, m_InitialData.UserData.UserNr)))

		Dim reader As SqlClient.SqlDataReader = m_Utility.OpenReader(m_InitialData.MDData.MDDbConn, SQL, listOfParams, CommandType.StoredProcedure)

		Try

			If (Not reader Is Nothing) Then

				result = New UserData

				While reader.Read()

					result.USNr = m_Utility.SafeGetInteger(reader, "USNr", 0)
					result.MDName = m_Utility.SafeGetString(reader, "MDName")
					result.MDName2 = m_Utility.SafeGetString(reader, "MDName2")
					result.MDName3 = m_Utility.SafeGetString(reader, "MDName3")
					result.MDPostfach = m_Utility.SafeGetString(reader, "MDPostfach")
					result.MDStrasse = m_Utility.SafeGetString(reader, "MDStrasse")
					result.MDPLZ = m_Utility.SafeGetString(reader, "MDPLZ")
					result.MDOrt = m_Utility.SafeGetString(reader, "MDOrt")
					result.MDLand = m_Utility.SafeGetString(reader, "MDLand")
					result.MD_Kanton = m_Utility.SafeGetString(reader, "MD_Kanton")
					result.MDTelefon = m_Utility.SafeGetString(reader, "MDTelefon")
					result.MDTelefax = m_Utility.SafeGetString(reader, "MDTelefax")
					result.MDeMail = m_Utility.SafeGetString(reader, "MDeMail")
					result.MDHomepage = m_Utility.SafeGetString(reader, "MDHomepage")
					result.USAnrede = m_Utility.SafeGetString(reader, "USAnrede")
					result.USVorname = m_Utility.SafeGetString(reader, "USVorname")
					result.USNachname = m_Utility.SafeGetString(reader, "USNachname")
					result.USTelefon = m_Utility.SafeGetString(reader, "USTelefon")
					result.USAbteilung = m_Utility.SafeGetString(reader, "USAbteilung")
					result.USPostfach = m_Utility.SafeGetString(reader, "USPostfach")
					result.USStrasse = m_Utility.SafeGetString(reader, "USStrasse")
					result.USPLZ = m_Utility.SafeGetString(reader, "USPLZ")
					result.USOrt = m_Utility.SafeGetString(reader, "USOrt")
					result.USLand = m_Utility.SafeGetString(reader, "USLand")
					result.USTelefax = m_Utility.SafeGetString(reader, "USTelefax")
					result.EMail_UserName = m_Utility.SafeGetString(reader, "EMail_UserName")
					result.EMail_UserPW = m_Utility.SafeGetString(reader, "EMail_UserPW")
					result.KST = m_Utility.SafeGetString(reader, "KST")
					result.USKST1 = m_Utility.SafeGetString(reader, "USKST1")
					result.USKst2 = m_Utility.SafeGetString(reader, "USKST2")

					result.USeMail = m_Utility.SafeGetString(reader, "USeMail")
					result.USNatel = m_Utility.SafeGetString(reader, "USNatel")
					result.USTitel_1 = m_Utility.SafeGetString(reader, "USTitel_1")
					result.USTitel_2 = m_Utility.SafeGetString(reader, "USTitel_2")
					result.LogedUser_Guid = m_Utility.SafeGetString(reader, "LogedUser_Guid")


				End While

			End If

		Catch e As Exception
			result = Nothing
			m_Logger.LogError(e.ToString())

		End Try

		Return result

	End Function




	Function GetUSData(ByVal myRegx As ClsDivFunc) As Boolean
		Dim success As Boolean = True
		Dim data = New UserData
		data = LoadOfferUSData(m_InitialData.UserData.UserNr)
		If data Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Das Programm kann nicht fortgesetzt werden.")
			Return False
		End If

		Try

			myRegx.Off_USNr = data.USNr
			myRegx.USAnrede = data.USAnrede
			myRegx.USeMail = data.USeMail
			myRegx.USNachname = data.USNachname
			myRegx.USVorname = data.USVorname
			myRegx.USTelefon = data.USTelefon
			myRegx.USTelefax = data.USTelefax
			myRegx.USNatel = data.USNatel
			myRegx.USTitel_1 = data.USTitel_1
			myRegx.USTitel_2 = data.USTitel_2
			myRegx.USAbteil = data.USAbteilung
			myRegx.USPostfach = data.USPostfach
			myRegx.USStrasse = data.USStrasse
			myRegx.USPLZ = data.USPLZ
			myRegx.USLand = data.USLand
			myRegx.USOrt = data.USOrt
			myRegx.Exchange_USName = data.EMail_UserName
			myRegx.Exchange_USPW = data.EMail_UserPW
			myRegx.USMDname = data.MDName
			myRegx.USMDname2 = data.MDName2
			myRegx.USMDname3 = data.MDName3
			myRegx.USMDPostfach = data.MDPostfach
			myRegx.USMDStrasse = data.MDStrasse
			myRegx.USMDPlz = data.MDPLZ
			myRegx.USMDOrt = data.MDOrt
			myRegx.USMDLand = data.MDLand

			myRegx.USMDTelefon = data.MDTelefon
			myRegx.USMDTelefax = data.MDTelefax
			myRegx.USMDeMail = data.MDeMail
			myRegx.USMDHomepage = data.MDHomepage


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

			success = False

		End Try

		Return success

	End Function

	Sub LoadMailTemplateData(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, Optional ByVal bTemplateAsFax As Boolean = False)
		Dim Conn As SqlConnection
		Dim sSql As String = "Select DocDb.Bezeichnung, DocDb.JobNr, DocDb.DocName From Dokprint DocDb Where DocDb.JobNr Like @JobNr And DocDb.DocName Not Like '%.doc%'"

		Conn = New SqlConnection(ClsDataDetail.GetDbConnString)

		Try
			Conn.Open()
			Dim SQLDocCmd As SqlCommand = New SqlCommand(sSql, Conn)
			SQLDocCmd.CommandType = CommandType.Text
			SQLDocCmd.CommandText = sSql
			Dim param As SqlParameter

			If bTemplateAsFax Then
				param = SQLDocCmd.Parameters.AddWithValue("@JobNr", "Fax%")
			Else
				param = SQLDocCmd.Parameters.AddWithValue("@JobNr", "Mail%")
			End If
			SQLDocCmd.Connection = Conn
			Dim rDocrec As SqlDataReader = SQLDocCmd.ExecuteReader

			While rDocrec.Read
				cbo.Properties.Items.Add(New ComboBoxItem(String.Format("{0}: {1}", rDocrec("Bezeichnung").ToString, rDocrec("DocName").ToString), rDocrec("JobNr").ToString))
			End While

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.", ex.ToString))

		Finally
			Conn.Close()

		End Try

	End Sub

	''' <summary>
	''' schreibt die Ereignisse vom WOS in die WOS_LogDB
	''' </summary>
	''' <param name="strCustomerID"></param>
	''' <param name="strUSID"></param>
	''' <param name="strMAGuid"></param>
	''' <param name="strKDGuid"></param>
	''' <param name="strZHDGuid"></param>
	''' <param name="strDocGuid"></param>
	''' <param name="strDocArt"></param>
	''' <param name="strDocInfo"></param>
	''' <param name="strTransferedOn"></param>
	''' <param name="strTransferedFrom"></param>
	''' <param name="strResultValue"></param>
	''' <param name="strFuncName"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function WriteToLogDb(ByVal strCustomerID As String,
								 ByVal strUSID As String,
								 ByVal strMAGuid As String,
								 ByVal strKDGuid As String,
								 ByVal strZHDGuid As String,
								 ByVal strDocGuid As String,
								 ByVal strDocArt As String,
								 ByVal strDocInfo As String,
								 ByVal strTransferedOn As Date,
								 ByVal strTransferedFrom As String,
								 ByVal strResultValue As String,
								 ByVal strFuncName As String) As String
		Dim Conn As SqlConnection = New SqlConnection(_ClsProgsetting.GetDbSelectConnString)
		Dim strValue As String = String.Empty
		Dim strQuery As String = "Insert Into WOS_LogDb (Customer_ID, LogedUser_ID, MA_Guid, KD_Guid, ZHD_Guid, "
		strQuery &= "Doc_Guid, Doc_Art, Doc_Info, "
		strQuery &= "TransferedOn, TransferedFrom, WOSValue, FuncName) Values ("
		strQuery &= "@Customer_ID, @LogedUser_ID, @MA_Guid, @KD_Guid, @ZHD_Guid, @Doc_Guid, @Doc_Art, @Doc_Info, "
		strQuery &= "@TransferedOn, @TransferedFrom, @WOSValue, @FuncName)"

		Try
			Conn.Open()
			Dim cmd As System.Data.SqlClient.SqlCommand
			cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
			Dim param As System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@Customer_ID", strCustomerID)
			param = cmd.Parameters.AddWithValue("@LogedUser_ID", strUSID)
			param = cmd.Parameters.AddWithValue("@MA_Guid", strMAGuid)
			param = cmd.Parameters.AddWithValue("@KD_Guid", strKDGuid)
			param = cmd.Parameters.AddWithValue("@ZHD_Guid", strZHDGuid)
			param = cmd.Parameters.AddWithValue("@Doc_Guid", strDocGuid)
			param = cmd.Parameters.AddWithValue("@Doc_Art", strDocArt)
			param = cmd.Parameters.AddWithValue("@Doc_Info", strDocInfo)

			param = cmd.Parameters.AddWithValue("@TransferedOn", Format(Now, "G"))
			param = cmd.Parameters.AddWithValue("@TransferedFrom",
																					String.Format("{0} {1}", _ClsProgsetting.GetUserFName,
																												_ClsProgsetting.GetUserLName))
			param = cmd.Parameters.AddWithValue("@WOSValue", strResultValue)
			param = cmd.Parameters.AddWithValue("@FuncName", strFuncName)

			cmd.ExecuteNonQuery()
			cmd.Parameters.Clear()


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.", ex.ToString))
			strValue = String.Format("{0}", ex.ToString)
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

		Return strValue
	End Function

	Function IsMyMessageAlreadySent(ByVal streMailTo As String,
															ByVal strSubject As String,
															ByVal strBody As String,
															ByVal iKDNr As Integer,
															ByVal strGuid As String,
															ByVal bSendAsTest As Boolean) As Boolean
		If Not ClsDataDetail.CheckForMailSent Then Return False

		Dim strConnString As String = _ClsProgsetting.GetConnString()
		Dim Conn As New SqlConnection(strConnString)
		Dim bResult As Boolean

		Dim sql As String = "Select Top 1 eMail_To, eMail_Subject, Createdon, Createdfrom From [{0}].dbo.Mail_Kontakte Where "
		sql &= "ltrim(rtrim(Customer_ID)) = @Customer_ID And eMail_To = @streMailTo And "
		sql &= "KDNr = @iKDNr And "
		sql &= "(replace(ltrim(rtrim(eMail_Subject)), char(13) + char(10), '') = @strSubject And Patindex('#Testnachricht#%', [{0}].dbo.Mail_Kontakte.eMail_Subject) = 0) "
		sql &= "Order By CreatedOn Desc"
		sql = String.Format(sql, ClsDataDetail.GetMailDbName)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sql, Conn)
			Dim param As System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@Customer_ID", Trim(strMDGuid))
			param = cmd.Parameters.AddWithValue("@streMailTo", Trim(streMailTo))
			param = cmd.Parameters.AddWithValue("@iKDNr", iKDNr)
			param = cmd.Parameters.AddWithValue("@strSubject", Trim(strSubject.Replace(vbNewLine, String.Empty)))
			If Not String.IsNullOrWhiteSpace(strBody) Then param = cmd.Parameters.AddWithValue("@strBody", Trim(strBody))

			Dim rKontaktrec As SqlDataReader = cmd.ExecuteReader

			rKontaktrec.Read()
			bResult = rKontaktrec.HasRows()
			If bResult Then
				_ClsLog.WriteTempLogFile(String.Format("***Duplikat KDNr: {0}: sent on {1}, {2} EMail From: {3} TO: {4}", iKDNr,
																							 m_Utility.SafeGetDateTime(rKontaktrec, "CreatedOn", Nothing),
																							 m_Utility.SafeGetString(rKontaktrec, "CreatedFrom"),
																							 streMailTo, Trim(strSubject.Replace(vbNewLine, String.Empty))),
																						 If(Not String.IsNullOrWhiteSpace(ClsDataDetail.GetMessageGuid), ClsDataDetail.GetTempLogFile, String.Empty))
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.", ex.ToString))
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

		Return bResult
	End Function


	Function LoadCustomerData(ByVal CustomerNumber As Integer, ByVal cResponsibleNumber As Integer?) As CustomerData
		Dim m_Utility As New SPProgUtility.MainUtilities.Utilities
		Dim result As CustomerData = Nothing

		Dim SQL As String = String.Empty

		If cResponsibleNumber.GetValueOrDefault(0) > 0 Then
			SQL &= "Select z.KDNr, z.RecNr As zRecNr, z.Firma1, z.eMail As zEMail, z.Telefax As zTelefax, kd.EMail, kd.Telefax "
			SQL &= "From KD_Zustaendig Z Left Join Kunden KD On Z.KDNr = KD.KDNr "
			SQL &= "Where z.KDNr = @CustomerNumber "
			SQL &= "And z.RecNr = @cResponsibleNumber "

		Else
			SQL &= "Select KDNr, Firma1, EMail, Telefax, 0 As zRecNr, '' As zEMail, '' As zTelefax From Kunden "
			SQL &= "Where KDNr = @CustomerNumber "

		End If

		Dim listOfParams As New List(Of SqlClient.SqlParameter)
		listOfParams.Add(New SqlClient.SqlParameter("CustomerNumber", CustomerNumber))

		If cResponsibleNumber.GetValueOrDefault(0) > 0 Then
			listOfParams.Add(New SqlClient.SqlParameter("cResponsibleNumber", cResponsibleNumber))
		End If

		Dim reader As SqlClient.SqlDataReader = m_Utility.OpenReader(m_InitialData.MDData.MDDbConn, SQL, listOfParams, CommandType.Text)

		Try

			If (Not reader Is Nothing) Then

				result = New CustomerData

				While reader.Read()

					result.CustomerNumber = m_Utility.SafeGetInteger(reader, "KDNr", 0)
					result.cResponsibleNumber = m_Utility.SafeGetInteger(reader, "zRecNr", 0)

					result.CustomerName = m_Utility.SafeGetString(reader, "Firma1")
					result.CustomerEMail = m_Utility.SafeGetString(reader, "EMail")
					result.CustomerTelefax = m_Utility.SafeGetString(reader, "Telefax")

					result.cResponsibleEMail = m_Utility.SafeGetString(reader, "zEMail")
					result.cResponsibleTelefax = m_Utility.SafeGetString(reader, "zTelefax")

				End While

			End If

		Catch e As Exception
			result = Nothing
			m_Logger.LogError(e.ToString())

		End Try

		Return result

	End Function

End Module


Public Class CustomerData

	Public Property CustomerNumber As Integer
	Public Property CustomerName As String
	Public Property CustomerEMail As String
	Public Property CustomerTelefax As String

	Public Property cResponsibleNumber As Integer
	Public Property cResponsibleEMail As String
	Public Property cResponsibleTelefax As String

End Class


Public Class UserData

	Public Property USNr As Integer
	Public Property MDName As String
	Public Property MDName2 As String
	Public Property MDName3 As String
	Public Property MDPostfach As String
	Public Property MDStrasse As String
	Public Property MDPLZ As String
	Public Property MDOrt As String
	Public Property MDLand As String
	Public Property MD_Kanton As String
	Public Property MDTelefon As String
	Public Property MDTelefax As String
	Public Property MDeMail As String
	Public Property MDHomepage As String
	Public Property USAnrede As String
	Public Property USVorname As String
	Public Property USNachname As String
	Public Property USTelefon As String
	Public Property USAbteilung As String
	Public Property USPostfach As String
	Public Property USStrasse As String
	Public Property USPLZ As String
	Public Property USOrt As String
	Public Property USLand As String
	Public Property USTelefax As String
	Public Property EMail_UserName As String
	Public Property EMail_UserPW As String
	Public Property KST As String
	Public Property USKST1 As String
	Public Property USKst2 As String

	Public Property USeMail As String
	Public Property USNatel As String
	Public Property USTitel_1 As String
	Public Property USTitel_2 As String
	Public Property LogedUser_Guid As String



End Class