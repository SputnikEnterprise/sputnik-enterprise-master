
Imports System.IO
Imports System.Data.SqlClient

Public Class MainForm

  Dim ClsReg As New SPProgUtility.ClsDivReg
  Dim ClsFunc As New ClsDivFunc

  Dim ClsSystem As New SPSSendMail.ClsMain_Net
  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

  Dim strConnString As String = _ClsProgSetting.GetConnString()
  Dim strMDProgFile As String = _ClsProgSetting.GetMDIniFile()
  Dim strInitProgFile As String = _ClsProgSetting.GetInitIniFile 'etInitIniFile()

  Dim Conn As SqlConnection

  Function StorFiletoFilesystem(ByVal iRecNr As Integer) As String
    Dim Conn As New SqlConnection(strConnString)
    Dim _ClsSystem As New ClsMain_Net

    Dim strFilename As String = ""
    Dim BA As Byte()
    Dim sScanSql As String = "Select ScanFile, FileName From [{0}].dbo.Mail_FileScan Where RecNr = @iRecNr"
    sScanSql = String.Format(sScanSql, ClsDataDetail.GetMailDbName)
    Conn.Open()
    Dim SQLCmd As SqlCommand = New SqlCommand(sScanSql, Conn)
    SQLCmd.CommandType = CommandType.Text
    Dim param As System.Data.SqlClient.SqlParameter
    param = SQLCmd.Parameters.AddWithValue("@iRecNr", iRecNr)

    Dim rScanDoc As SqlDataReader = SQLCmd.ExecuteReader

    rScanDoc.Read()
    If rScanDoc.HasRows Then
      strFilename = rScanDoc("FileName").ToString
      strFilename = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\" & strFilename
      rScanDoc.Close()
    Else
      Conn.Dispose()

    End If

    Try
      BA = CType(SQLCmd.ExecuteScalar, Byte())

      Dim ArraySize As New Integer
      ArraySize = BA.GetUpperBound(0)

      If File.Exists(strFilename) Then File.Delete(strFilename)
      Dim fs As New FileStream(strFilename, FileMode.CreateNew)
      fs.Write(BA, 0, ArraySize + 1)
      fs.Close()
      fs.Dispose()

    Catch ex As Exception
      MsgBox(Err.Description, MsgBoxStyle.Critical, Application.ProductName)

    Finally
      Conn.Close()

    End Try

    Return strFilename
  End Function

  Private Sub lstAttachments_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstAttachments.DoubleClick
    Dim strFullFilename As String = StorFiletoFilesystem(CInt(Me.LblChanged.Text))

    Try
      Process.Start(strFullFilename)

    Catch ex As Exception
      MsgBox(ex.Message)

    End Try

  End Sub

  Private Sub lstAttachments_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstAttachments.SelectedIndexChanged

  End Sub

  Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click

    Me.Close()
    Me.Dispose()

  End Sub
End Class