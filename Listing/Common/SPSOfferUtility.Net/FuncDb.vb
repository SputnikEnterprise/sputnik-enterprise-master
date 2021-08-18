
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient

Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SP.Infrastructure.Logging

Imports SPSOfferUtility_Net.ClsOfDetails


Module FuncDb
	Private m_Logger As ILogger = New Logger()


	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	Private _ClsReg As New SPProgUtility.ClsDivReg

	Public iOfferNumber As Integer

	Public Function GetCustomerData(ByVal conn As String, ByVal sql As String) As CustomerData
		Dim m_Utility As New SPProgUtility.MainUtilities.Utilities
		Dim result As CustomerData = Nothing


		Dim reader As SqlClient.SqlDataReader = m_Utility.OpenReader(conn, sql, Nothing, CommandType.Text)

		Try

			If (Not reader Is Nothing AndAlso reader.Read()) Then
				result = New CustomerData

				result.CustomerNumber = m_Utility.SafeGetInteger(reader, "KDNr", 0)
				result.CResponsibleNumber = m_Utility.SafeGetInteger(reader, "ZHDRecNr", 0)

			End If

		Catch e As Exception
			result = Nothing
			m_Logger.LogError(e.ToString())

		End Try

		Return result

	End Function

	Public Function LoadOfferData(ByVal conn As String, ByVal OfferNumber As Integer, ByVal customerNumber As Integer?, ByVal responsibleNumber As Integer?) As IEnumerable(Of OfferData)
		Dim m_Utility As New SPProgUtility.MainUtilities.Utilities
		Dim result As List(Of OfferData) = Nothing

		Dim SQL As String = String.Empty

		SQL &= "Select * From OFF_KDSelection "
		SQL &= "Where OfNr = @OfNr "
		SQL &= "And (@KDNr = 0 Or KDNr = @KDNr) "
		SQL &= "And (@KDZNr = 0 Or KDzNr = @KDZNr) "

		Dim listOfParams As New List(Of SqlClient.SqlParameter)
		listOfParams.Add(New SqlClient.SqlParameter("OfNr", OfferNumber))
		listOfParams.Add(New SqlClient.SqlParameter("KDZNr", m_Utility.ReplaceMissing(responsibleNumber, 0)))
		listOfParams.Add(New SqlClient.SqlParameter("KDNr", m_Utility.ReplaceMissing(customerNumber, 0)))

		Dim reader As SqlClient.SqlDataReader = m_Utility.OpenReader(conn, SQL, listOfParams, CommandType.Text)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of OfferData)

				While reader.Read()
					Dim overviewData As New OfferData

					overviewData.OfferNumber = m_Utility.SafeGetInteger(reader, "OfNr", 0)
					overviewData.CustomerNumber = m_Utility.SafeGetInteger(reader, "KDNr", 0)
					overviewData.CResponsibleNumber = m_Utility.SafeGetInteger(reader, "KDzNr", 0)

					result.Add(overviewData)

				End While

			End If

		Catch e As Exception
			result = Nothing
			m_Logger.LogError(e.ToString())

		End Try

		Return result
	End Function


	Public Function GetCustomerForOfferData(ByVal conn As String, ByVal offerNumber As Integer) As CustomerData
		Dim m_Utility As New SPProgUtility.MainUtilities.Utilities
		Dim result As CustomerData = Nothing

		Dim sql As String
		sql = "Select Top 1 KDNr, KDZNr From OFF_KDSelection Where OfNr = @OfNr"

		Dim listOfParams As New List(Of SqlClient.SqlParameter)
		listOfParams.Add(New SqlClient.SqlParameter("OfNr", offerNumber))

		Dim reader As SqlClient.SqlDataReader = m_Utility.OpenReader(conn, sql, listOfParams, CommandType.Text)

		Try

			If (Not reader Is Nothing AndAlso reader.Read()) Then
				result = New CustomerData

				result.CustomerNumber = m_Utility.SafeGetInteger(reader, "KDNr", 0)
				result.CResponsibleNumber = m_Utility.SafeGetInteger(reader, "KDzNr", 0)

			End If

		Catch e As Exception
			result = Nothing
			m_Logger.LogError(e.ToString())

		End Try

		Return result

	End Function

	Public Sub PlaySound(ByVal sSound As Short)

		If sSound = 0 Then
			System.Media.SystemSounds.Asterisk.Play()

		ElseIf sSound = 2 Then
			System.Media.SystemSounds.Exclamation.Play()

		ElseIf sSound = 3 Then
			System.Media.SystemSounds.Hand.Play()

		ElseIf sSound = 4 Then
			System.Media.SystemSounds.Question.Play()

		Else
			System.Media.SystemSounds.Beep.Play()

		End If

	End Sub

#Region "Kontakteinträge..."

	Sub CreateLogToKDKontaktDb(ByVal lKDNr As Integer, ByVal lZHDNr As Integer, ByVal strBetreff As String)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Time_1 As Double = System.Environment.TickCount
		Dim strUSName As String = _ClsProgSetting.GetUserName()
		Dim Conn As New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim sKDZSql As String = "Insert Into KD_KontaktTotal (KDNr, KDZNr, RecNr, KontaktDate, Kontakte, "
		sKDZSql &= "KontaktType1, KontaktType2, Kontaktwichtig, KontaktDauer, KontaktErledigt, MANr, "
		sKDZSql &= "CreatedOn, CreatedFrom) Values (@KDNr, @ZHDNr, @RecNr, @KontaktDate, "
		sKDZSql &= "'Wurde Offerte geschickt', @KontaktType1, 2, 0, '', 0, 0, @KontaktDate, @USName)"
		Dim lNewRecNr As Integer

		Try
			Conn.Open()
			lNewRecNr = GetNewKontaktNr(lKDNr, lZHDNr)

			Dim rKontaktrec As New SqlDataAdapter()

			rKontaktrec.SelectCommand = New SqlCommand(sKDZSql, Conn)
			rKontaktrec.SelectCommand.Parameters.AddWithValue("@KDNr", lKDNr)
			rKontaktrec.SelectCommand.Parameters.AddWithValue("@ZHDNr", lZHDNr)
			rKontaktrec.SelectCommand.Parameters.AddWithValue("@RecNr", lNewRecNr)

			rKontaktrec.SelectCommand.Parameters.AddWithValue("@KontaktType1", strBetreff)
			rKontaktrec.SelectCommand.Parameters.AddWithValue("@KontaktDate", Now.ToString("G"))
			rKontaktrec.SelectCommand.Parameters.AddWithValue("@USName", strUSName)
			'_ClsProgsetting.
			Dim dt As DataTable = New DataTable()
			rKontaktrec.Fill(dt)

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try

		Conn.Close()

		Dim Time_2 As Double = System.Environment.TickCount
		Console.WriteLine("Zeit für CreateLogToKDKontaktDb: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")

	End Sub

	Sub CreateLogToMailKontaktDb(ByVal lKDNr As Integer, ByVal lZHDNr As Integer, _
																ByVal lMANr As Integer, ByVal strFilename As String(), _
																ByVal bSendAsHtml As Boolean, Optional ByVal MailTo As String = "", _
																Optional ByVal MailFrom As String = "", _
																Optional ByVal MailSubject As String = "", _
																Optional ByVal MailBody As String = "", _
															 Optional ByVal bSendAsTest As Boolean = False, _
															 Optional ByVal bAsTelefax As Boolean = False)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Time_1 As Double = System.Environment.TickCount
		Dim strUSName As String = _ClsProgSetting.GetUserName()
		Dim Conn As New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim sMailSql As String = "Insert Into [{0}].dbo.Mail_Kontakte (KDNr, KDZNr, RecNr, MANr, Message_ID, eMail_To, "
		sMailSql &= "eMail_From, eMail_Subject, eMail_Body, eMail_smtp, AsHtml, AsTelefax, Customer_ID, "
		sMailSql &= "CreatedOn, CreatedFrom) Values (@KDNr, @ZHDNr, @RecNr, @MANr, @Message_ID, @eMailTo, @eMailFrom, "
		sMailSql &= "@eMailsubject, @eMailbody, @eMailSmtp, @SendAsHtml, @AsTelefax, @Customer_ID, @KontaktDate, @USName)"
		sMailSql = String.Format(sMailSql, ClsOfDetails.GetMailDbName)
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

			param = cmd.Parameters.AddWithValue("@Message_ID", String.Empty) ' ClsDataDetail.GetMessageGuid)
			param = cmd.Parameters.AddWithValue("@eMailTo", MailTo)
			param = cmd.Parameters.AddWithValue("@eMailFrom", MailFrom)
			param = cmd.Parameters.AddWithValue("@eMailsubject", String.Format("{0}{1}", String.Format("{0}", If(bSendAsTest, "#Testnachricht#", "")), MailSubject))
			param = cmd.Parameters.AddWithValue("@eMailbody", MailBody)
			param = cmd.Parameters.AddWithValue("@eMailSmtp", _ClsProgSetting.GetSmtpServer())
			param = cmd.Parameters.AddWithValue("@SendAsHtml", bSendAsHtml)
			param = cmd.Parameters.AddWithValue("@AsTelefax", bAsTelefax)

			param = cmd.Parameters.AddWithValue("@Customer_ID", _ClsProgSetting.GetSelectedMDData(0))
			param = cmd.Parameters.AddWithValue("@KontaktDate", Now.ToString("G"))
			param = cmd.Parameters.AddWithValue("@USName", strUSName)

			cmd.Connection = Conn
			cmd.ExecuteNonQuery()

			' Binaryfile in die Datenbank
			If strFilename.Length <> 0 Then	'And Not ClsDataDetail.IsAttachedFileInd Then
				'        InsertBinaryToMailDb(lNewRecNr, strFilename, MailTo, MailFrom, MailSubject)
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		Finally
			cmd.Dispose()
			Conn.Close()

		End Try

		Dim Time_2 As Double = System.Environment.TickCount
		Console.WriteLine("Zeit für CreateLogToMailKontaktDb: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")

	End Sub

	Function GetNewKontaktNr(ByVal lKDNr As Integer, ByVal lKDZNr As Integer) As Integer
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim lRecNr As Integer = 1
		Dim Conn As New SqlConnection(m_InitialData.MDData.MDDbConn)
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
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		Finally
			Conn.Close()

		End Try

		Return lRecNr
	End Function

	Function GetNeweMailKontaktNr() As Integer
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim lRecNr As Integer = 1
		Dim Conn As New SqlConnection(m_InitialData.MDData.MDDbConn)
		Conn.Open()

		Try
			Dim sSql As String = "Select Top 1 RecNr From [{0}].dbo.Mail_Kontakte Order By RecNr Desc"
			sSql = String.Format(sSql, ClsOfDetails.GetMailDbName)
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
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		Finally
			Conn.Close()

		End Try

		Return lRecNr
	End Function


#End Region

	Function IsMyMessageAlreadySent(ByVal streMailTo As String, _
														ByVal strSubject As String, _
														ByVal strBody As String, _
														ByVal iKDNr As Integer, _
														ByVal strGuid As String, _
														ByVal bSendAsTest As Boolean) As Boolean
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Time_1 As Double = System.Environment.TickCount
		Dim strConnString As String = m_InitialData.MDData.MDDbConn
		Dim Conn As New SqlConnection(strConnString)
		Dim bResult As Boolean

		Dim sSQL As String = "Select Top 1 eMail_To, eMail_Subject From [{0}].dbo.Mail_Kontakte Where "
		sSQL &= "Customer_ID = @Customer_ID And eMail_To = @streMailTo And "
		sSQL &= "KDNr = @iKDNr And "
		sSQL &= "(eMail_Subject = @strSubject And Patindex('#Testnachricht#%', [{0}].dbo.Mail_Kontakte.eMail_Subject) = 0)"
		sSQL = String.Format(sSQL, ClsOfDetails.GetMailDbName)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSQL, Conn)
			Dim param As System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@Customer_ID", _ClsProgSetting.GetSelectedMDData(0))
			param = cmd.Parameters.AddWithValue("@streMailTo", streMailTo)
			param = cmd.Parameters.AddWithValue("@iKDNr", iKDNr)
			param = cmd.Parameters.AddWithValue("@strSubject", strSubject)
			If strBody <> String.Empty Then param = cmd.Parameters.AddWithValue("@strBody", strBody)

			Dim rKontaktrec As SqlDataReader = cmd.ExecuteReader

			rKontaktrec.Read()
			bResult = rKontaktrec.HasRows()


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			MsgBox(ex.Message.ToString & vbCrLf & ex.GetBaseException.ToString, MsgBoxStyle.Critical, "IsMessageAlreadySent")

		End Try

		Return bResult
	End Function

	Function GetOfferSubject(ByVal iOffNr As Integer) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strOfferBez As String = String.Empty
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim sOfferSql As String = "Select Of_Res7 From Offers Where OfNr = @OffNr"
			Dim SQLOffCmd As SqlCommand = New SqlCommand(sOfferSql, Conn)
			SQLOffCmd.CommandType = CommandType.Text
			Dim param As System.Data.SqlClient.SqlParameter
			param = SQLOffCmd.Parameters.AddWithValue("@OffNr", iOffNr)
			Dim rOffrec As SqlDataReader = SQLOffCmd.ExecuteReader					' Offertendatenbank
			rOffrec.Read()
			If Not rOffrec.HasRows Then
				MsgBox("Keine Daten wurden gefunden. Bitte kontrollieren Sie Ihre Auswahl und versuchen Sie es erneuert.", _
							 MsgBoxStyle.Critical, "Daten nicht gefunden.")

				Conn.Dispose()
				Return strOfferBez
			Else
				strOfferBez = rOffrec("Of_Res7").ToString

			End If
			rOffrec.Close()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			MsgBox(ex.Message, MsgBoxStyle.Critical, "GetOfferSubject_1")

		End Try

		Return strOfferBez
	End Function

	Function GetOfferBezeichnung(ByVal iOffNr As Integer) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strOfferBez As String = String.Empty
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim sOfferSql As String = "Select Of_Bezeichnung From Offers Where OfNr = @OffNr"
			Dim SQLOffCmd As SqlCommand = New SqlCommand(sOfferSql, Conn)
			SQLOffCmd.CommandType = CommandType.Text
			Dim param As System.Data.SqlClient.SqlParameter
			param = SQLOffCmd.Parameters.AddWithValue("@OffNr", iOffNr)

			Dim rOffrec As SqlDataReader = SQLOffCmd.ExecuteReader					' Offertendatenbank
			rOffrec.Read()
			If Not rOffrec.HasRows Then

				Conn.Dispose()
				Return strOfferBez
			Else
				strOfferBez = rOffrec("Of_Bezeichnung").ToString

			End If
			rOffrec.Close()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			strOfferBez = String.Format("Fehler: {0} / {1}" & ex.Message, "GetOfferBezeichnung_1")

		End Try

		Return strOfferBez
	End Function

	'Function GetDbInfo(ByVal LL As ListLabel, ByVal strJobNr As String, _
	'									 ByVal iOfferNr As Integer, ByVal iKDNr As Integer, _
	'									 ByVal iKDZNr As Integer) As SqlDataReader
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim sSql As String = String.Empty	' "Select DocDb.JobNr, DocDb.Bezeichnung From Dokprint DocDb Where DocDb.JobNr Like @JobNr"
	'	Dim SelectedOffNumber As Integer = iOfferNr
	'	Dim bWithoutMA As Boolean

	'	Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn )
	'	Dim i As Integer = 0
	'	Dim cmd As SqlCommand
	'	Dim rOffrec As SqlDataReader

	'	Try
	'		Conn.Open()

	'		'If strJobNr.Contains("15.") Then
	'		'  ' Benutzerdaten auflisten
	'		'  sSql = "Select _Of.Of_Berater, US.Telefon As USTelefon, US.Nachname As USNachname, "
	'		'  sSql += "US.Vorname As USVorname, US.Telefax As USTelefax From Offers _Of "
	'		'  sSql += "Left Join Benutzer US On _Of.Of_Berater = US.KST "
	'		'  sSql += "Where _Of.OfNr = " & iOfferNr & " "

	'		'  cmd = New System.Data.SqlClient.SqlCommand(sSql, Conn)
	'		'  Dim rUSrec As SqlDataReader = cmd.ExecuteReader          ' Benutzerdatenbank
	'		'  rUSrec.Read()
	'		'  If rUSrec.HasRows Then
	'		'    PubFunc.DefineData(LL, False, rUSrec)
	'		'  End If
	'		'  rUSrec.Close()

	'		'  sSql = "[Get OfferData For Print In Stammblatt] "
	'		'  sSql += iOfferNr & ", " & iKDNr & ", 0"

	'		'  cmd = New System.Data.SqlClient.SqlCommand(sSql, Conn)
	'		'  rOffrec = cmd.ExecuteReader          ' Offertendatenbank
	'		'  rOffrec.Read()
	'		'  If rOffrec.HasRows Then
	'		'    Return rOffrec

	'		'  Else
	'		'    Return Nothing

	'		'  End If

	'		'Else
	'		sSql = "Select MANr From OFF_MASelection Where OfNr = " & iOfferNr

	'		cmd = New System.Data.SqlClient.SqlCommand(sSql, Conn)
	'		rOffrec = cmd.ExecuteReader					 ' Offertendatenbank
	'		rOffrec.Read()
	'		If rOffrec.HasRows Then
	'			sSql = "[Get OfferData For Print In MailMerge] "
	'			bWithoutMA = False

	'		Else
	'			sSql = "[Get OfferData For Print In MailMerge Without MA] "
	'			bWithoutMA = True

	'		End If
	'		sSql += iOfferNr & ", " & iKDNr & ", " & iKDZNr
	'		rOffrec.Close()
	'		cmd = New System.Data.SqlClient.SqlCommand(sSql, Conn)
	'		rOffrec = cmd.ExecuteReader					 ' Offertendatenbank
	'		rOffrec.Read()
	'		If Not rOffrec.HasRows Then
	'			rOffrec.Close()
	'			Conn.Close()

	'			Return Nothing
	'		End If
	'		Return rOffrec

	'		'End If

	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
	'		MsgBox(String.Format("***Fehler_1 (GetDbInfo): Fehler bei der Suche nach Daten. JobNr: {0}{1}", _
	'												 strJobNr, vbNewLine & ex.Message), _
	'												 MsgBoxStyle.Critical, "GetDbInfo")

	'	Finally
	'		'    Conn.Close()

	'	End Try

	'	Return Nothing
	'End Function

	'Sub ListMailToFields(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal str4What As String, ByVal sql As String)
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	'Dim sSql As String = _ClsLLFunc.GetSearchQuery
	'	Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
	'	Dim i As Integer

	'	cbo.Properties.Items.Clear()

	'	If sql = String.Empty Then Return
	'	'Dim Time_1 As Double = System.Environment.TickCount

	'	Try
	'		Conn.Open()

	'		Dim SQLDocCmd As SqlCommand = New SqlCommand(sql, Conn)
	'		SQLDocCmd.CommandType = CommandType.Text
	'		SQLDocCmd.CommandText = sql

	'		SQLDocCmd.Connection = Conn
	'		Dim rMailingrec As SqlDataReader = SQLDocCmd.ExecuteReader					' Offertendatenbank

	'		rMailingrec.Read()
	'		For i = 0 To rMailingrec.FieldCount - 1
	'			If rMailingrec.GetName(i).ToString.ToUpper.Contains("eMail".ToUpper) And str4What.ToUpper.Contains("MAIL".ToUpper) Then
	'				cbo.Properties.Items.Add(New ComboValue(rMailingrec.GetName(i).ToString, rMailingrec.GetName(i)))

	'			ElseIf rMailingrec.GetName(i).ToString.ToUpper.Contains("DTeleFax".ToUpper) And str4What.ToUpper.Contains("FAX".ToUpper) Then
	'				cbo.Properties.Items.Add(New ComboValue(rMailingrec.GetName(i).ToString, rMailingrec.GetName(i)))
	'				'cbo.Properties.Items.Add(rMailingrec.GetName(i).ToString)
	'			End If

	'		Next i
	'		cbo.Properties.Items.Add(New ComboValue(TranslateText("Kunden und Zuständige Personen"), "kdzhd")) ' "KDTelefax|ZHDTelefax"))

	'		rMailingrec.Close()
	'		'Dim Time_2 As Double = System.Environment.TickCount
	'		'Console.WriteLine("Zeit für ListMailToFields: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")

	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
	'		MsgBox(ex.Message, MsgBoxStyle.Critical, "ListMailToFields_0")

	'	Finally
	'		Conn.Close()

	'	End Try

	'End Sub

	'Sub ListMailFromFields(ByVal cbo As ComboBox)
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim sSql As String = ClsOfDetails.GetOrgProgQuery
	'	Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
	'	Dim i As Integer

	'	Dim Time_1 As Double = System.Environment.TickCount

	'	If _ClsProgSetting.GetLogedUSNr() = 1 Then MsgBox(sSql, , "ListMailFromFields_1")
	'	Try
	'		Conn.Open()

	'		Dim SQLDocCmd As SqlCommand = New SqlCommand(sSql, Conn)
	'		SQLDocCmd.CommandType = CommandType.Text
	'		SQLDocCmd.CommandText = sSql

	'		SQLDocCmd.Connection = Conn
	'		Dim rMailingrec As SqlDataReader = SQLDocCmd.ExecuteReader					' Offertendatenbank

	'		cbo.Items.Clear()

	'		For i = 0 To rMailingrec.FieldCount - 1
	'			If InStr(UCase(rMailingrec.GetName(i).ToString), UCase("Anredeform")) > 0 Then
	'				cbo.Items.Add(rMailingrec.GetName(i).ToString + " " + rMailingrec.GetName(CInt("Nachname")).ToString)
	'			End If
	'		Next i

	'		rMailingrec.Close()

	'		Dim Time_2 As Double = System.Environment.TickCount
	'		Console.WriteLine("Zeit für ListMailFromFields: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")

	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
	'		MsgBox(ex.Message, MsgBoxStyle.Critical, "ListMailFromFields_0")

	'	Finally
	'		Conn.Close()

	'	End Try

	'End Sub

End Module
