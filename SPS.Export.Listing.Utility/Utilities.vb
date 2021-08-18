
Imports System.Data.SqlClient
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports SPProgUtility.SPTranslation.ClsTranslation

Module CommonUtil


	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	Private ReadOnly Property GetMailDbName() As String
		Get
			Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
			Dim strDatabaseName As String = "Sputnik_MailDb"
			Dim strQuery As String = String.Format("//Mailing/Mail-Database")
			strDatabaseName = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetMDData_XMLFile, _
																																							 strQuery, strDatabaseName)

			Return strDatabaseName

		End Get
	End Property



#Region "Kontakteinträge..."


	Sub CreateLogToKDKontaktDb(ByVal lKDNr As Integer, ByVal lZHDNr As Integer, ByVal strBetreff As String)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Time_1 As Double = System.Environment.TickCount
		Dim strUSName As String = _ClsProgSetting.GetUserName()
		Dim Conn As New SqlConnection(_ClsProgSetting.GetConnString())
		Dim sKDZSql As String = "Insert Into KD_KontaktTotal (KDNr, KDZNr, RecNr, KontaktDate, Kontakte, "
		sKDZSql &= "KontaktType1, KontaktType2, Kontaktwichtig, KontaktDauer, KontaktErledigt, MANr, "
		sKDZSql &= "CreatedOn, CreatedFrom) Values (@KDNr, @ZHDNr, @RecNr, @KontaktDate, "
		sKDZSql &= "'Wurde Fax-Nachricht geschickt', @KontaktType1, 2, 0, '', 0, 0, @KontaktDate, @USName)"
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
		Dim Conn As New SqlConnection(_ClsProgSetting.GetConnString())
		Dim sMailSql As String = "Insert Into [{0}].dbo.Mail_Kontakte (KDNr, KDZNr, RecNr, MANr, Message_ID, eMail_To, "
		sMailSql &= "eMail_From, eMail_Subject, eMail_Body, eMail_smtp, AsHtml, AsTelefax, Customer_ID, "
		sMailSql &= "CreatedOn, CreatedFrom) Values (@KDNr, @ZHDNr, @RecNr, @MANr, @Message_ID, @eMailTo, @eMailFrom, "
		sMailSql &= "@eMailsubject, @eMailbody, @eMailSmtp, @SendAsHtml, @AsTelefax, @Customer_ID, @KontaktDate, @USName)"
		sMailSql = String.Format(sMailSql, GetMailDbName)
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

			param = cmd.Parameters.AddWithValue("@Message_ID", String.Empty)
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
			If strFilename.Length <> 0 Then	'And Not clsmainsetting.IsAttachedFileInd Then
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
		Dim Conn As New SqlConnection(_ClsProgSetting.GetConnString())
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
		Dim Conn As New SqlConnection(_ClsProgSetting.GetConnString())
		Conn.Open()

		Try
			Dim sSql As String = "Select Top 1 RecNr From [{0}].dbo.Mail_Kontakte Order By RecNr Desc"
			sSql = String.Format(sSql, GetMailDbName)
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

	Function GetRandom(ByVal minimum As Integer, ByVal maximum As Integer) As Integer
		Try
			Dim nRandom As Integer

			Randomize()
			nRandom = CInt(minimum + (maximum - minimum + 1) * Rnd())

			While nRandom < minimum OrElse nRandom > maximum
				Randomize()
				nRandom = CInt(minimum + (maximum - minimum + 1) * Rnd())
			End While

			Return nRandom
		Catch ex As Exception
			'ToDo Fehlerbehandlung
			Return minimum
		End Try

	End Function




End Module
