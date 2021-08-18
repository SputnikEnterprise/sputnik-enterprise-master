
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Reflection

Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data

Module FuncLv
  Dim _ClsFunc As New ClsDivFunc
  Dim _ClsReg As New SPProgUtility.ClsDivReg
  Dim _Clsprogsetting As New SPProgUtility.ClsProgSettingPath

  Dim strMDPath As String = ""
  Dim strInitPath As String = ""

  Dim iLogedUSNr As Integer = 0

#Region "Sonstige Funktions..."

  Function GetKundenData(ByVal iKDNr As Integer) As List(Of String)
    Dim liResult As New List(Of String)
    Dim conn As SqlConnection = New SqlConnection(_Clsprogsetting.GetConnString)

    Try
      conn.Open()
      Dim cmdText As String = "[List Data For TarifCalculator]"

      Dim cmd As SqlCommand = New SqlCommand(cmdText, conn)
      cmd.CommandType = CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@SortKey", 4)
      param = cmd.Parameters.AddWithValue("@parameter", iKDNr)

      Dim reader As SqlDataReader = cmd.ExecuteReader()
      reader.Read()
      If reader.HasRows Then
        liResult.Add(CStr(reader("Firma1")))
        liResult.Add(CStr(reader("Kanton")))
      End If

    Catch ex As Exception
      MsgBox(String.Format("{0}", ex.Message), MsgBoxStyle.Critical, "GetKundenData")

    Finally
      conn.Close()
    End Try

    Return liResult
  End Function

  Function GetKandidatData(ByVal iMANr As Integer) As List(Of String)
    Dim liResult As New List(Of String)
    Dim conn As SqlConnection = New SqlConnection(_Clsprogsetting.GetConnString)

    Try
      conn.Open()
      Dim cmdText As String = "[List Data For TarifCalculator]"

      Dim cmd As SqlCommand = New SqlCommand(cmdText, conn)
      cmd.CommandType = CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@SortKey", 3)
      param = cmd.Parameters.AddWithValue("@parameter", iMANr)

      Dim reader As SqlDataReader = cmd.ExecuteReader()
      reader.Read()
      If reader.HasRows Then
        liResult.Add(CStr(reader("Nachname")))
        liResult.Add(CStr(reader("Vorname")))
        liResult.Add(CStr(GetSUVA_MAAlter(CDate(reader("GebDat")))))
      End If

    Catch ex As Exception
      MsgBox(String.Format("{0}", ex.Message), MsgBoxStyle.Critical, "GetKandidatData")

    Finally
      conn.Close()
    End Try

    Return liResult
  End Function

  Sub GetGAVKantone(ByVal cbo As ComboBox)
    Dim conn As SqlConnection = New SqlConnection(_ClsProgSetting.GetConnString)
		Try
			conn.Open()
			Dim cmdText As String = "SELECT * "
			cmdText += "FROM Tab_Kanton ORDER BY GetFeld "
			Dim cmd As SqlCommand = New SqlCommand(cmdText, conn)
			cmd.CommandType = CommandType.Text
			Dim reader As SqlDataReader = cmd.ExecuteReader()
			cbo.Items.Clear()
			While reader.Read()
				cbo.Items.Add(reader("GetFeld"))
			End While

		Catch ex As Exception
			MsgBox(String.Format("{0}", ex.Message), MsgBoxStyle.Critical, "GetGAVKantone")

		Finally
			conn.Close()
    End Try
  End Sub

  Function GetKDGAVListe(ByVal iKDNr As Integer) As List(Of String)
    Dim strResult As New List(Of String)
    Dim conn As SqlConnection = New SqlConnection(_ClsProgSetting.GetConnString)

    Try
      conn.Open()
      Dim cmdText As String = "SELECT GAVNumber, Bezeichnung "
      cmdText &= "FROM KD_GAVGruppe K0 "
      cmdText += "WHERE K0.KDNR = @KDNr "
      Dim cmd As SqlCommand = New SqlCommand(cmdText, conn)
      cmd.CommandType = CommandType.Text
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@KDNr", iKDNr)

      Dim reader As SqlDataReader = cmd.ExecuteReader()

      While reader.Read
        strResult.Add(String.Format("{0}", reader("GAVNumber").ToString))
      End While

    Catch ex As Exception

    End Try


    Return strResult
  End Function

  Function GetSUVA_MAAlter(ByVal dGebDate As Date) As Integer

    Dim dGeburtstag As Date = CDate(dGebDate)
    Dim today As DateTime = DateTime.Now
    Dim birth As DateTime = New DateTime(dGeburtstag.Year, dGeburtstag.Month, dGeburtstag.Day)
    Dim alter As Integer = today.Year - birth.Year
    If (today.Month < birth.Month) Then
      alter = alter - 1
    ElseIf (today.Month = birth.Month And today.Day < birth.Day) Then
      alter = alter - 1
    End If

    Return alter
  End Function

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

      For i = 0 To strCaption.Length - 1
        lvwColumn = New ColumnHeader()
        lvwColumn.Text = strCaption(i).ToString
        strFieldData = Regex.Split(strFieldInfo(i).ToString, "-")

        If strFieldInfo(i).ToString.StartsWith("-") Then
          strFieldWidth = strFieldData(1)
          lvwColumn.Width = CInt(strFieldWidth) * -1
          If strFieldData.Count > 1 Then
            strFieldAlign = CStr(IIf(strFieldData(0) = String.Empty, strFieldData(2), strFieldData(1)))
          End If
        Else
          strFieldWidth = Regex.Split(strFieldInfo(i).ToString, "-")(0)
          lvwColumn.Width = CInt(strFieldWidth) '* Screen.PrimaryScreen.BitsPerPixel  ' TwipsPerPixelX
          If strFieldData.Count > 1 Then
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

  Sub FillFoundedData(ByVal Lv As ListView, ByVal frmSource As frmCalculator, ByVal strQuery As String)
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)
    Dim i As Integer = 0
    Dim _ClsDb As New ClsDbFunc

    Try
      Conn.Open()
      Dim cmd As System.Data.SqlClient.SqlCommand
      If strQuery = String.Empty Then
        strQuery = _ClsDb.GetLocalSQLString(frmSource, 0)
      End If
      cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)

      Dim rAdressrec As SqlDataReader = cmd.ExecuteReader          ' Quellensteuer
      Lv.Items.Clear()
      Lv.FullRowSelect = True

      Dim Time_1 As Double = System.Environment.TickCount

      Lv.BeginUpdate()
      While rAdressrec.Read
        With Lv
          .Items.Add(rAdressrec("ID").ToString)
          .Items(i).SubItems.Add(rAdressrec("RecNr").ToString)
          .Items(i).SubItems.Add(rAdressrec("Kanton").ToString)
          .Items(i).SubItems.Add(rAdressrec("Gemeinde").ToString)
          .Items(i).SubItems.Add(rAdressrec("Adresse1").ToString)

          .Items(i).SubItems.Add(rAdressrec("PLZ").ToString & " " & rAdressrec("Ort").ToString)

        End With

        i += 1
        Lv.EndUpdate()

      End While
      Console.WriteLine(String.Format("Zeit für FillFoundedData: {0} s", _
                                      ((System.Environment.TickCount - Time_1) / 1000).ToString()))


    Catch e As Exception
      Lv.Items.Clear()
      MsgBox(e.Message, MsgBoxStyle.Critical, "FillFoundedData")

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try

  End Sub

  Sub GetAGBeitragData(ByVal iMANr As Integer)
    Dim strFieldName As String = "USFullName"
    Dim strSqlQuery As String = "Get_MDData_For_Marge"
    Dim LiMDData As New List(Of Double)

    Dim i As Integer = 0
    Dim iWidth As Integer = 0
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@MDYear", Year(Now))

      Dim rMDrec As SqlDataReader = cmd.ExecuteReader


      While rMDrec.Read
        LiMDData.Add(CDbl(rMDrec("AHV_AG")))
        LiMDData.Add(CDbl(rMDrec("ALV_AG")))
        LiMDData.Add(CDbl(rMDrec("Suva_A")))
        LiMDData.Add(CDbl(rMDrec("Suva_Z")))

        LiMDData.Add(CDbl(rMDrec("Fak_Proz")))
        LiMDData.Add(CDbl(rMDrec("X_Marge")))

        LiMDData.Add(CDbl(rMDrec("KK_AG_WA")))
        LiMDData.Add(CDbl(rMDrec("KK_AG_WZ")))
        LiMDData.Add(CDbl(rMDrec("KK_AG_MA")))
        LiMDData.Add(CDbl(rMDrec("KK_AG_MZ")))

        LiMDData.Add(CDbl(rMDrec("Rentfrei_Jahr")))
        LiMDData.Add(CDbl(rMDrec("MindestAlter")))

        LiMDData.Add(CDbl(rMDrec("AG_Tar_Proz")))
        If iMANr > 0 Then
          LiMDData.Add(CDbl(GetBVGProz(iMANr)))
        Else
          LiMDData.Add(0)
        End If


      End While
      ClsDataDetail.GetLiAGData = LiMDData

    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try

  End Sub

  Function GetBVGProz(ByVal iMANr As Integer) As Double
    Dim strFieldName As String = "ProzentSatz"
    Dim strSqlQuery As String = "[Get BVGProzent For Kandidat]"
    Dim dProz As Double = 0
    If iMANr = 0 Then Return dProz

    Dim i As Integer = 0
    Dim iWidth As Integer = 0
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@lMANr", iMANr)
      param = cmd.Parameters.AddWithValue("@MDYear", Year(Now))

      Dim rMDrec As SqlDataReader = cmd.ExecuteReader

      While rMDrec.Read
        dProz = CDbl(rMDrec(strFieldName))
      End While

    Catch e As Exception
      MsgBox(e.Message)
      Return dProz

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try

    Return dProz
  End Function


#End Region




End Module
