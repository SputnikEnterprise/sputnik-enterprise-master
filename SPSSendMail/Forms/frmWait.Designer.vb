<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmWait
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmWait))
    Me.Label1 = New System.Windows.Forms.Label
    Me.PictureBox1 = New System.Windows.Forms.PictureBox
    Me.Label3 = New System.Windows.Forms.Label
    Me.Button1 = New System.Windows.Forms.Button
    CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'Label1
    '
    Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label1.Location = New System.Drawing.Point(29, 73)
    Me.Label1.Name = "Label1"
    Me.Label1.Size = New System.Drawing.Size(328, 22)
    Me.Label1.TabIndex = 0
    Me.Label1.Text = "Bitte warten Sie einen Moment..."
    '
    'PictureBox1
    '
    Me.PictureBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.PictureBox1.BackColor = System.Drawing.Color.White
    Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
    Me.PictureBox1.Location = New System.Drawing.Point(-2, -1)
    Me.PictureBox1.Name = "PictureBox1"
    Me.PictureBox1.Size = New System.Drawing.Size(302, 50)
    Me.PictureBox1.TabIndex = 11
    Me.PictureBox1.TabStop = False
    '
    'Label3
    '
    Me.Label3.AutoSize = True
    Me.Label3.BackColor = System.Drawing.Color.White
    Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label3.Location = New System.Drawing.Point(20, 19)
    Me.Label3.Name = "Label3"
    Me.Label3.Size = New System.Drawing.Size(87, 13)
    Me.Label3.TabIndex = 14
    Me.Label3.Text = "Versandstatus"
    '
    'Button1
    '
    Me.Button1.Location = New System.Drawing.Point(32, 51)
    Me.Button1.Name = "Button1"
    Me.Button1.Size = New System.Drawing.Size(76, 19)
    Me.Button1.TabIndex = 15
    Me.Button1.Text = "Button1"
    Me.Button1.UseVisualStyleBackColor = True
    '
    'frmWait
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(290, 108)
    Me.Controls.Add(Me.Button1)
    Me.Controls.Add(Me.Label3)
    Me.Controls.Add(Me.PictureBox1)
    Me.Controls.Add(Me.Label1)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Name = "frmWait"
    Me.Padding = New System.Windows.Forms.Padding(9)
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
    Me.Text = "Bitte warten Sie einen moment..."
    CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents Label1 As System.Windows.Forms.Label
  Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
  Friend WithEvents Label3 As System.Windows.Forms.Label
  Friend WithEvents Button1 As System.Windows.Forms.Button

End Class
