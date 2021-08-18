
Imports System
Imports SP.Infrastructure.Logging
Imports System.Text.RegularExpressions
Imports System.Data.SqlClient

Imports SPProgUtility.ClsProgSettingPath


Module FuncLV

	Private m_Logger As ILogger = New Logger()

	Dim _ClsReg As New SPProgUtility.ClsDivReg
  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath


  Sub FillDataHeaderLv(ByVal Lv As ListView, ByRef strColumnList As String, ByRef strColumnInfo As String)
    Dim lstStuff As ListViewItem = New ListViewItem()
    Dim lvwColumn As ColumnHeader

    With Lv
      .Clear()

      ' Nr;Nummer;Name;Strasse;PLZ Ort
      If strColumnList.EndsWith(";") Then strColumnInfo = Mid(strColumnList, 1, strColumnList.Length - 1)
      If strColumnInfo.EndsWith(";") Then strColumnInfo = Mid(strColumnInfo, 1, strColumnInfo.Length - 1)

      Dim strCaption As String() = Regex.Split(strColumnList, ";")
      ' 0-1;0-1;2000-0;2000-0;2500-0
      Dim strFieldInfo As String() = Regex.Split(strColumnInfo, ";")
      Dim strFieldWidth As String
      Dim strFieldAlign As String = "0"
      Dim strFieldData As String()

      For i As Integer = 0 To strCaption.Length - 1
        lvwColumn = New ColumnHeader()
        lvwColumn.Text = strCaption(i).ToString
        strFieldData = Regex.Split(strFieldInfo(i).ToString, "-")

        If strFieldInfo(i).ToString.StartsWith("-") Then
          strFieldWidth = strFieldData(1)
          lvwColumn.Width = CInt(strFieldWidth) * -1
          If strFieldData.Length > 1 Then
            strFieldAlign = CStr(IIf(strFieldData(0) = String.Empty, strFieldData(2), strFieldData(1)))
          End If
        Else
          strFieldWidth = Regex.Split(strFieldInfo(i).ToString, "-")(0)
          lvwColumn.Width = CInt(strFieldWidth) '* Screen.PrimaryScreen.BitsPerPixel  ' TwipsPerPixelX
          If strFieldData.Length > 1 Then
            strFieldAlign = strFieldData(1)
          End If
        End If
        If strFieldAlign = "1" Then
          lvwColumn.TextAlign = HorizontalAlignment.Right
        ElseIf strFieldAlign = "2" Then
          lvwColumn.TextAlign = HorizontalAlignment.Center
        Else
          lvwColumn.TextAlign = HorizontalAlignment.Left

        End If
        lstStuff.BackColor = Color.Yellow
        .Columns.Add(lvwColumn)
      Next

      lvwColumn = Nothing
    End With

  End Sub

  Sub SetLvwHeader(ByVal Lv As ListView)
    Dim strColumnString As String = String.Empty
    Dim strColumnWidthInfo As String = String.Empty
    Dim strUSLang As String = _ClsProgSetting.GetUSLanguage()
    Dim strQuery As String = String.Empty
    Dim strBez As String = String.Empty
    Lv.BorderStyle = BorderStyle.None

    strColumnString = _ClsProgSetting.TranslateText("ID;BerufCode;Berufsgruppe;FachCode;Fachbereich")
    strColumnWidthInfo = "0-0;0-0;200-0;0-0;200-0"

    FillDataHeaderLv(Lv, strColumnString, strColumnWidthInfo)

  End Sub


  Function SaveBerufgruppe(ByVal iMANr As Integer, ByVal iBerufID As Integer, ByVal iFachID As Integer, _
                      ByVal strBerufBez As String, ByVal strFachBez As String) As String
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)
    Dim strQuery As String = String.Empty
    Dim strResult As String = "Erfolg"

    Try
      Conn.Open()
      strQuery = "Delete [MA.BerufGruppe] Where MANr = @MANr And BerufID = @BerufID And FachID = @FachID "
      strQuery &= "Insert Into [MA.BerufGruppe] (MANr, BerufID, FachID, BerufBez_DE, FachBez_DE) Values ("
      strQuery &= "@MANr, @BerufID, @FachID, @BerufBez_DE, @FachBez_DE)"
      Dim cmd As SqlCommand = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
      cmd.CommandType = CommandType.Text
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@MANr", iMANr)
      param = cmd.Parameters.AddWithValue("@BerufID", iBerufID)
      param = cmd.Parameters.AddWithValue("@FachID", iFachID)
      param = cmd.Parameters.AddWithValue("@BerufBez_DE", strBerufBez)
      param = cmd.Parameters.AddWithValue("@FachBez_DE", strFachBez)

      'param = cmd.Parameters.AddWithValue("@Lang", _ClsProgSetting.GetUSLanguage)

      cmd.ExecuteNonQuery()


    Catch ex As Exception
      strResult = String.Format("Fehler: {0}", ex.Message)
			m_Logger.LogError(String.Format("Fehler: {0}", ex.Message))

		End Try

    Return strResult
  End Function

  Function DeleteBerufgruppe(ByVal iID As Integer, ByVal iMANr As Integer) As String
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)
    Dim strQuery As String = String.Empty
    Dim strResult As String = "Erfolg"

    Try
      Conn.Open()
      strQuery = "Delete [MA.BerufGruppe] Where MANr = @MANr And ID = @ID"
      Dim cmd As SqlCommand = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
      cmd.CommandType = CommandType.Text
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@MANr", iMANr)
      param = cmd.Parameters.AddWithValue("@ID", iID)

      cmd.ExecuteNonQuery()


    Catch ex As Exception
      strResult = String.Format("Fehler: {0}", ex.Message)
			m_Logger.LogError(String.Format("Fehler: {0}", ex.Message))

		End Try

		Return strResult
	End Function

	Sub FillBerufGruppeLV(ByVal Lv As ListView, ByVal iMANr As Integer)
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)
		Dim i As Integer = 0
		Dim strQuery As String = String.Empty
		Dim iDaysAgo As Integer = 7

		Try
			Conn.Open()

			strQuery = "[Kandidat. Get All Berufgruppen und Fachbereiche]"
			Dim cmd As SqlCommand = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
			cmd.CommandType = CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@MANr", iMANr)
			param = cmd.Parameters.AddWithValue("@USLanguage", _ClsProgSetting.GetUSLanguage)

			Trace.WriteLine(strQuery)

			Dim rAdressrec As SqlDataReader = cmd.ExecuteReader
			Lv.Items.Clear()
			Lv.FullRowSelect = True
			Lv.MultiSelect = False
			If Lv.Columns.Count = 0 Then SetLvwHeader(Lv)

			Dim Time_1 As Double = System.Environment.TickCount

			Lv.BeginUpdate()
			While rAdressrec.Read
				With Lv
					.Items.Add(rAdressrec("ID").ToString)
					.Items(i).SubItems.Add(rAdressrec("BerufID").ToString)
					.Items(i).SubItems.Add(rAdressrec("BerufBez").ToString)
					.Items(i).SubItems.Add(rAdressrec("FachID").ToString)
					.Items(i).SubItems.Add(rAdressrec("FachBez"))

				End With

				i += 1
			End While
			Lv.EndUpdate()
			Console.WriteLine(String.Format("Zeit für FillBerufGruppeLV: {0} s",
																			((System.Environment.TickCount - Time_1) / 1000).ToString()))


		Catch e As Exception
			Lv.Items.Clear()
			MsgBox(e.Message, MsgBoxStyle.Critical, "FillBerufGruppeLV")
			m_Logger.LogError(String.Format("Fehler: {0}", e.Message))

		Finally
      Conn.Close()
      Conn.Dispose()

    End Try

  End Sub

End Module
