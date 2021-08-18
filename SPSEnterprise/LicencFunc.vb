
Imports System.IO
Imports System.Data.SqlClient
Imports NLog


Module LicencFunc

	Private logger As Logger = LogManager.GetCurrentClassLogger()

	Private _ClsReg As New ClsDivReg
	Private _ClsSystem As New ClsMain_Net

	Private strConnString As String = _ClsSystem.GetConnString()
	Private strDbSelectConn As String = _ClsSystem.GetDbSelectConnString()
  
	Private strUsName As String = Environment.UserName
	Private strMashineName As String = Environment.MachineName

  Function ISLicencOK() As Boolean
    Dim bResult As Boolean = True
    Dim StIPName As String = String.Empty
    Dim StNetName As String = String.Empty
    Dim sSql As String = ""
		logger.Debug("entring into licenccheck")

    Try
      Dim aIP As Array = GetIP().ToArray
      Dim strMyIP As String = String.Empty
      For i As Integer = 0 To aIP.Length - 1
        strMyIP = IIf(aIP(i).ToString.Length < 16, aIP(i).ToString, "")
      Next
      StIPName = strMyIP

		Catch ex As Exception
			logger.Error(ex.ToString)
			MsgBox(ex.ToString, MsgBoxStyle.Critical, "IsLicencOK_0")

		End Try

		Try
			StNetName = GetIPHostName()

		Catch ex As Exception
			logger.Error(ex.ToString)
			MsgBox(ex.ToString, MsgBoxStyle.Critical, "IsLicencOK_1")

		End Try

    Try
      Dim ConnDbSelect As System.Data.SqlClient.SqlConnection = Nothing
			Try
				logger.Debug(String.Format("strDbSelectConn: {0}", strDbSelectConn))

				ConnDbSelect = New System.Data.SqlClient.SqlConnection(strDbSelectConn)

			Catch ex As Exception
				logger.Error(ex.ToString)
				MsgBox(Err.Description & vbNewLine & "strDbSelectConn: " & strDbSelectConn & vbNewLine & ex.StackTrace, _
							 MsgBoxStyle.Critical, "IsLicencOK_1")

			End Try

      sSql = "Select * From UserInfo Where MACAdress = @MacAddress"
      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, ConnDbSelect)
      Dim param As System.Data.SqlClient.SqlParameter

      Dim sLizCount As Short = CShort(_ClsReg.GetINIString(SrvSettingFullFileName, "Customer", _
                                                           My.Resources.str423).ToString.Trim)
      Dim strMacAdress As String = GetMACAddress()
      Dim LizenText As String = ""

      ConnDbSelect.Open()
      param = cmd.Parameters.AddWithValue("@MacAddress", strMacAdress)
			Dim rLicrec As SqlDataReader = cmd.ExecuteReader
      rLicrec.Read()
      If Not rLicrec.HasRows Then
        InsertToUserInfo()

      Else
        cmd.Dispose()
        rLicrec.Close()

        sSql = "Select * From UserInfo Where MACAdress = @strMacAdress"
        cmd = New System.Data.SqlClient.SqlCommand(sSql, ConnDbSelect)
				param = cmd.Parameters.AddWithValue("@strMacAdress", strMacAdress)

				rLicrec = cmd.ExecuteReader
        rLicrec.Read()

        If Not rLicrec.HasRows Then
          cmd.Dispose()
          rLicrec.Close()

          sSql = "Select Count(*) As LizCount From UserInfo Where "
          sSql += "(Loged_USer_ID Is Null Or Loged_USer_ID = '')"
          cmd = New SqlClient.SqlCommand(sSql, ConnDbSelect)
          rLicrec = cmd.ExecuteReader

          If rLicrec("LizCount") >= sLizCount Then
						logger.Warn("Licenc is overloaded!!!")
						MsgBox(String.Format(GetSafeTranslationValue("Die maximale Benutzeranzahl wurde erreicht.{0}" & _
									 "Wenn Sie noch weitere Lizenzen benötigen, " & _
									 "kontaktieren Sie Ihren Softwarelieferanten.{1}"), vbNewLine, LizenText), _
									 MsgBoxStyle.Information, GetSafeTranslationValue("Benutzerlizenzen"))
            bResult = False

          Else
            ' hinzufügen...
            sSql = "Insert Into UserInfo (StationIP, StationName, MACAdress, FirstTimeLoged, "
            sSql += "Loged_USer_ID) Values (@strIP, @strNetName, @strMacAddress, @dDate, @strWStationInfo)"

            param = New System.Data.SqlClient.SqlParameter
            cmd = New System.Data.SqlClient.SqlCommand(sSql, ConnDbSelect)

            param = cmd.Parameters.AddWithValue("@strIP", StIPName)
            param = cmd.Parameters.AddWithValue("@strNetName", StNetName)
						param = cmd.Parameters.AddWithValue("@strMacAddress", strMacAdress)
						param = cmd.Parameters.AddWithValue("@dDate", CType(Date.Now, Date).ToShortDateString() & " - " & CType(TimeOfDay, Date).ToShortDateString())
            param = cmd.Parameters.AddWithValue("@strWStationInfo", strMashineName & " " & strUsName)

            cmd.ExecuteNonQuery()

          End If

        End If

      End If
      ConnDbSelect.Dispose()

    Catch ex As Exception
			logger.Error(ex.ToString)
			MsgBox(Err.Description & vbNewLine & "SSQL: " & sSql, MsgBoxStyle.Critical, "IsLicencOK_0")
      ' eupro Arbon...
      bResult = True ' False

    Finally

    End Try

    Return bResult
  End Function

  Sub InsertToUserInfo()
    Dim ConnDbSelect As New SqlConnection(strDbSelectConn)
    Dim iAnzahl As Integer = 0
    Dim sSql As String = "Select Count(Loged_User_ID) As UserAnz From UserInfo Where Loged_User_ID = @WStationInfo"

    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, ConnDbSelect)
    Dim param As System.Data.SqlClient.SqlParameter

    Try
      ConnDbSelect.Open()
      param = cmd.Parameters.AddWithValue("@WStationInfo", strMashineName & " " & strUsName)
      Dim rLicrec As SqlDataReader = cmd.ExecuteReader          ' Offertendatenbank
      rLicrec.Read()
      iAnzahl = rLicrec("UserAnz")
      rLicrec.Close()

      ' hinzufügen...
      sSql = "Insert Into UserInfo (StationIP, StationName, MACAdress, FirstTimeLoged, "
      sSql += "Loged_USer_ID) Values (@strIP, @strNetName, @strMacAddress, @dDate, @strWStationInfo)"

      param = New System.Data.SqlClient.SqlParameter
      cmd = New System.Data.SqlClient.SqlCommand(sSql, ConnDbSelect)

      Dim aIP As Array = GetIP().ToArray
      Dim strMyIP As String = String.Empty
      For i As Integer = 0 To aIP.Length - 1
        strMyIP = IIf(aIP(i).ToString.Length < 16, aIP(i).ToString, "")
      Next

      param = cmd.Parameters.AddWithValue("@strIP", strMyIP)
      param = cmd.Parameters.AddWithValue("@strNetName", GetIPHostName())
      param = cmd.Parameters.AddWithValue("@strMacAddress", GetMACAddress())
      param = cmd.Parameters.AddWithValue("@dDate", CType(Date.Now, Date).ToShortDateString() & " - " & CType(TimeOfDay, Date).ToShortDateString())
      param = cmd.Parameters.AddWithValue("@strWStationInfo", strMashineName & " " & strUsName)

      cmd.ExecuteNonQuery()

    Catch ex As Exception
      MsgBox(Err.Description, MsgBoxStyle.Critical, "InsertToUserInfo")

    Finally
      ConnDbSelect.Close()
    End Try

  End Sub

End Module
