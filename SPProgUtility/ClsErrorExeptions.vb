
Imports System.IO
Imports NLog

Namespace SPExceptionsManager

  Public Class ClsErrorExceptions

    Private Shared logger As Logger = LogManager.GetCurrentClassLogger()

    Public Shared Function MessageBoxShowError(ByVal Caption As String, _
                                        ByVal ExceptionObject As Exception) As String
      Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
      Dim _ClsProgsetting As New ClsProgSettingPath
      Dim bAdminUser As Boolean = _ClsProgsetting.GetLogedUSNr

      Try
        Dim message As String = String.Format("{0}{1}", vbNewLine, ExceptionObject.Message)

        If bAdminUser Then
          message += String.Format("{0}{1}", vbNewLine, ExceptionObject.StackTrace)
        End If
        MsgBox(message, Caption, Caption)
        logger.Error(String.Format("{0}.{1}", strMethodeName, ExceptionObject.Message))

      Catch ex As Exception
        logger.Error(String.Format("{0}.Fehler bei der Exception:{1}", strMethodeName, ex.Message))

      End Try

      Return String.Empty
    End Function

  End Class


End Namespace
