<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUpdateprotokoll
  Inherits System.Windows.Forms.Form

  'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
  <System.Diagnostics.DebuggerNonUserCode()> _
  Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    Try
      If disposing AndAlso components IsNot Nothing Then
        components.Dispose()
      End If
    Finally
      MyBase.Dispose(disposing)
    End Try
  End Sub

  'Wird vom Windows Form-Designer benötigt.
  Private components As System.ComponentModel.IContainer

  'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
  'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
  'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
  <System.Diagnostics.DebuggerStepThrough()> _
  Private Sub InitializeComponent()
    Me.EventLog1 = New System.Diagnostics.EventLog()
    Me.LBLMessage = New System.Windows.Forms.Label()
    Me.TextBox1 = New System.Windows.Forms.TextBox()
    CType(Me.EventLog1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'EventLog1
    '
    Me.EventLog1.SynchronizingObject = Me
    '
    'LBLMessage
    '
    Me.LBLMessage.Location = New System.Drawing.Point(28, 9)
    Me.LBLMessage.Name = "LBLMessage"
    Me.LBLMessage.Size = New System.Drawing.Size(208, 20)
    Me.LBLMessage.TabIndex = 0
    Me.LBLMessage.Text = "Label1"
    '
    'TextBox1
    '
    Me.TextBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.TextBox1.Location = New System.Drawing.Point(31, 32)
    Me.TextBox1.Multiline = True
    Me.TextBox1.Name = "TextBox1"
    Me.TextBox1.Size = New System.Drawing.Size(824, 210)
    Me.TextBox1.TabIndex = 1
    '
    'frmUpdateprotokoll
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(884, 264)
    Me.Controls.Add(Me.TextBox1)
    Me.Controls.Add(Me.LBLMessage)
    Me.Name = "frmUpdateprotokoll"
    Me.Text = "Updateprotokoll"
    CType(Me.EventLog1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents EventLog1 As System.Diagnostics.EventLog
  Friend WithEvents LBLMessage As System.Windows.Forms.Label
  Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
End Class
