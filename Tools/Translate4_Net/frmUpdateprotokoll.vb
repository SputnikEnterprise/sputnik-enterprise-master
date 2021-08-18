
Public Class frmUpdateprotokoll


  Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    If Not EventLog.Exists("DebSmplg") Then
      EventLog.CreateEventSource("DebSmp", "DebSmplg")
    End If

    SampleEventlog = New EventLog
    SampleEventlog.Log = "DebSmplg"
    SampleEventlog.Source = "DebSmp"

    EventLog1.Log = "DebSmplg"
    EventLog1.EnableRaisingEvents = True

  End Sub

  Private Sub EventLog1_EntryWritten(ByVal sender As Object, ByVal e As System.Diagnostics.EntryWrittenEventArgs) Handles EventLog1.EntryWritten
    LBLMessage.Text = e.Entry.Message
  End Sub

  Private Sub LBLMessage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LBLMessage.Click

  End Sub
End Class