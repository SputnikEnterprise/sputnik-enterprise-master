
Public NotInheritable Class frmWait


  Private Sub frmWait_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    Me.Text = ""
    Me.ControlBox = False

    '    CenterNewForm(Me)

  End Sub

  Private Sub CenterNewForm(ByRef frm As Form)
    Dim rc As Rectangle = Screen.GetBounds(frm)
    With frm
      .Left = Convert.ToInt32((rc.Width - .Width) / 2)
      .Top = Convert.ToInt32((rc.Height - .Height) / 2)
    End With
  End Sub

  Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
    Dim _ClsSystem As New ClsMain_Net

    '    _ClsProgSetting.SendMADoceMailWithTemplate(2)
    '_ClsProgSetting.SendMADoceMailWithTemplate(2)

  End Sub

End Class
