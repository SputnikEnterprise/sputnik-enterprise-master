
Imports System.Data.SqlClient
Imports System.IO

Public Class ClsDbFunc
  Implements IClsDbRegister

  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
  Dim _ClsLog As New SPProgUtility.ClsEventLog
  Dim Conn As New SqlConnection(_ClsProgSetting.GetConnString)
  'Dim Conn As New SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=Sputnik ScanJobs;Data Source=VM_SP_2008_64_3;Current Language=German")

  Dim reportDBInformation As New DBInformation

  Function StoreSelectedMAPhoto2FS() As String Implements IClsDbRegister.StoreSelectedMAPhoto2FS
    If reportDBInformation.iCandidatNr = 0 Then Return String.Empty

    Dim strFullFilename As String = String.Empty
    Dim strFiles As String = String.Empty
    Dim BA As Byte() = Nothing
    Dim sMASql As String = "Select MABild, MANr From Mitarbeiter Where "
    sMASql &= String.Format("MANr = {0} And MABild Is Not Null", reportDBInformation.iCandidatNr)

    'Dim sMASql As String = "Select Scan_Komplett From [RP.A0BB18D4-84EB-42d7-B366-721ED0E296EC] Where "
    'sMASql &= String.Format("ID = {0} And Scan_Komplett Is Not Null", 312)


    Dim i As Integer = 0

    Conn.Open()
    Dim SQLCmd As SqlCommand = New SqlCommand(sMASql, Conn)
    Dim SQLCmd_1 As SqlCommand = New SqlCommand(sMASql, Conn)

    Try

      strFullFilename = String.Format("{0}Bild_{1}_{2}.JPG", _ClsProgSetting.GetSpSBildFiles2DeletePath, _
                                       reportDBInformation.iCandidatNr, System.Guid.NewGuid.ToString())

      Try
        Try
          BA = CType(SQLCmd_1.ExecuteScalar, Byte())
        Catch ex As Exception

        End Try
        If BA Is Nothing Then Return String.Empty

        Dim ArraySize As New Integer
        ArraySize = BA.GetUpperBound(0)

        If File.Exists(strFullFilename) Then File.Delete(strFullFilename)
        Dim fs As New FileStream(strFullFilename, FileMode.CreateNew)
        fs.Write(BA, 0, ArraySize + 1)
        fs.Close()
        fs.Dispose()

        i += 1

      Catch ex As Exception
        _ClsLog.WriteToEventLog(String.Format("***GetMAPicture: {0}", ex.Message))
        MsgBox(String.Format("Fehler: {0}", ex.Message), MsgBoxStyle.Critical, "GetMAPicture")
        strFullFilename = String.Empty

      End Try


    Catch ex As Exception
      _ClsLog.WriteToEventLog(String.Format("***GetMAPicture: {0}", ex.Message))
      strFullFilename = String.Empty

    End Try

    Return strFullFilename
  End Function

  Function SaveFileIntoDb(ByVal strFileToSave As String, _
                          ByVal bild As Image) As String Implements IClsDbRegister.SaveFileIntoDb
    Dim Time_1 As Double = System.Environment.TickCount
    Dim strUSName As String = _ClsProgSetting.GetUserName()
    Dim Conn As New SqlConnection(_ClsProgSetting.GetConnString)
    Dim strLogFileName As String = _ClsProgSetting.GetProzessLOGFile()
    Dim sSql As String = String.Empty
    Dim strResult As String = String.Empty

    sSql = "Update Mitarbeiter Set MABild = @BinaryFile "
    sSql &= "Where MANr = @MANr"

    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
    Dim param As System.Data.SqlClient.SqlParameter

    Try
      Conn.Open()
      cmd.Connection = Conn

      If strFileToSave <> String.Empty Then
        'Dim myFile() As Byte = GetFileToByte(strFileToSave)
        Dim myFile() As Byte = Image2ByteArray(bild, Imaging.ImageFormat.Bmp)
        Dim fi As New System.IO.FileInfo(strFileToSave)
        Dim strFileExtension As String = fi.Extension

        Try
          cmd.CommandType = CommandType.Text
          cmd.CommandText = sSql

          param = cmd.Parameters.AddWithValue("@BinaryFile", myFile)
          param = cmd.Parameters.AddWithValue("@MANr", reportDBInformation.iCandidatNr)

          cmd.Connection = Conn
          cmd.ExecuteNonQuery()

          cmd.Parameters.Clear()
          strResult = "Erfolgreich..."


        Catch ex As Exception
          strResult = String.Format("***Fehler (SaveFileIntoDb_1): {0}", ex.Message)
          _ClsLog.WriteTempLogFile(String.Format("***SaveFileIntoDbb_1: {0}", _
                                                 ex.Message), strLogFileName)

        End Try
      End If
      _ClsLog.WriteTempLogFile(String.Format("Erfolgreich: SaveFileIntoDb: " & _
                                                  "MANr: {0} / strFilename: {1}", _
                                                  reportDBInformation.iCandidatNr, strFileToSave), strLogFileName)

    Catch ex As Exception
      strResult = String.Format("***Fehler (SaveFileIntoDb_2): {0}", ex.Message)
      _ClsLog.WriteTempLogFile(String.Format("***SaveFileIntoDb_2: {0}", ex.Message), strLogFileName)

    Finally
      cmd.Dispose()
      Conn.Close()

    End Try

    Dim Time_2 As Double = System.Environment.TickCount
    Console.WriteLine("Zeit für SaveFileIntoDb: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")

    Return strResult
  End Function


  Function DeleteImageFromDB() As String Implements IClsDbRegister.DeleteImageFromDb
    Dim Time_1 As Double = System.Environment.TickCount
    Dim strUSName As String = _ClsProgSetting.GetUserName()
    Dim Conn As New SqlConnection(_ClsProgSetting.GetConnString)
    Dim strLogFileName As String = _ClsProgSetting.GetProzessLOGFile()
    Dim sSql As String = String.Empty
    Dim strResult As String = String.Empty

    sSql = "Update Mitarbeiter Set MABild = Null "
    sSql &= "Where MANr = @MANr"

    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
    Dim param As System.Data.SqlClient.SqlParameter

    Try
      Conn.Open()
      cmd.Connection = Conn

      Try
        cmd.CommandType = CommandType.Text
        cmd.CommandText = sSql

        param = cmd.Parameters.AddWithValue("@MANr", Me.reportDBInformation.iCandidatNr)

        cmd.Connection = Conn
        cmd.ExecuteNonQuery()

        cmd.Parameters.Clear()
        strResult = "Erfolgreich..."


      Catch ex As Exception
        strResult = String.Format("***Fehler (DeleteImageFromDB_1): {0}", ex.Message)
        _ClsLog.WriteTempLogFile(String.Format("***DeleteImageFromDB_1: {0}", _
                                               ex.Message), strLogFileName)

      End Try

    Catch ex As Exception
      strResult = String.Format("***Fehler (DeleteImageFromDB_2): {0}", ex.Message)
      _ClsLog.WriteTempLogFile(String.Format("***DeleteImageFromDB_2: {0}", ex.Message), strLogFileName)

    Finally
      cmd.Dispose()
      Conn.Close()

    End Try

    Dim Time_2 As Double = System.Environment.TickCount
    Console.WriteLine("Zeit für DeleteImageFromDB: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")

    Return strResult
  End Function


  Function GetFileToByte(ByVal filePath As String) As Byte() Implements IClsDbRegister.GetFileToByte
    Dim stream As FileStream = New FileStream(filePath, FileMode.Open, FileAccess.Read)
    Dim reader As BinaryReader = New BinaryReader(stream)

    Dim photo() As Byte = Nothing
    Try

      photo = reader.ReadBytes(CInt(stream.Length))
      reader.Close()
      stream.Close()

    Catch ex As Exception
      _ClsLog.WriteTempLogFile(String.Format("***GetFileToByte_1: {0}", ex.Message), filePath)

    End Try

    Return photo
  End Function

  Function Image2ByteArray(ByVal Bild As Image, _
                           ByVal Bildformat As System.Drawing.Imaging.ImageFormat) As Byte()
    Dim MS As New IO.MemoryStream
    Bild.Save(MS, Bildformat)
    MS.Flush()

    Return MS.ToArray
  End Function

  Public Sub New(ByVal iMANr As Integer)
    Me.reportDBInformation.iCandidatNr = iMANr
  End Sub


End Class
