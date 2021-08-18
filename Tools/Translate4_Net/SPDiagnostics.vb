
Imports System.Windows.Forms

Public Class SPDiagnostics

  Dim _ClsProgsetting As New ClsProgSettingPath
  Dim _diagnosticsStart As Long = Diagnostics.Stopwatch.GetTimestamp()
  Dim _diagnosticsMsgs As ArrayList = New ArrayList()

  Sub ResetDiagnosticsStopwatch()
    _diagnosticsStart = Diagnostics.Stopwatch.GetTimestamp()
    _diagnosticsMsgs.Clear()
  End Sub

  Sub AddDiagnosticsMsg(ByVal msg As String)

    _diagnosticsMsgs.Add(String.Format("{0}{3}: {1}{2}", _
                                       msg.PadRight(30), _
                                       (Diagnostics.Stopwatch.GetTimestamp() - _diagnosticsStart) / Diagnostics.Stopwatch.Frequency, _
                                       vbLf, _
                                       vbTab _
                                       ))

  End Sub

  Sub ShowDiagnosticsMsg()
    Dim _meldung As String = String.Empty

    For Each meldung As String In _diagnosticsMsgs
      _meldung += meldung
    Next

    If _ClsProgsetting.GetLogedUSNr = 1 Then
      MessageBox.Show(_meldung, "Diagnostic", MessageBoxButtons.OK)
    End If

  End Sub

End Class
