<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
    Me.Button1 = New System.Windows.Forms.Button
    Me.Button2 = New System.Windows.Forms.Button
    Me.Button3 = New System.Windows.Forms.Button
    Me.Button4 = New System.Windows.Forms.Button
    Me.TextBox1 = New System.Windows.Forms.TextBox
    Me.Button5 = New System.Windows.Forms.Button
    Me.SuspendLayout()
    '
    'Button1
    '
    Me.Button1.Location = New System.Drawing.Point(359, 298)
    Me.Button1.Name = "Button1"
    Me.Button1.Size = New System.Drawing.Size(95, 33)
    Me.Button1.TabIndex = 0
    Me.Button1.Text = "Send Fax"
    Me.Button1.UseVisualStyleBackColor = True
    '
    'Button2
    '
    Me.Button2.Location = New System.Drawing.Point(359, 337)
    Me.Button2.Name = "Button2"
    Me.Button2.Size = New System.Drawing.Size(95, 33)
    Me.Button2.TabIndex = 1
    Me.Button2.Text = "Send Mail"
    Me.Button2.UseVisualStyleBackColor = True
    '
    'Button3
    '
    Me.Button3.Location = New System.Drawing.Point(480, 378)
    Me.Button3.Name = "Button3"
    Me.Button3.Size = New System.Drawing.Size(95, 33)
    Me.Button3.TabIndex = 2
    Me.Button3.Text = "Telefonieren"
    Me.Button3.UseVisualStyleBackColor = True
    '
    'Button4
    '
    Me.Button4.Location = New System.Drawing.Point(480, 417)
    Me.Button4.Name = "Button4"
    Me.Button4.Size = New System.Drawing.Size(95, 33)
    Me.Button4.TabIndex = 3
    Me.Button4.Text = "Kundensearch"
    Me.Button4.UseVisualStyleBackColor = True
    '
    'TextBox1
    '
    Me.TextBox1.Location = New System.Drawing.Point(15, 47)
    Me.TextBox1.Multiline = True
    Me.TextBox1.Name = "TextBox1"
    Me.TextBox1.Size = New System.Drawing.Size(560, 245)
    Me.TextBox1.TabIndex = 4
    '
    'Button5
    '
    Me.Button5.Location = New System.Drawing.Point(480, 298)
    Me.Button5.Name = "Button5"
    Me.Button5.Size = New System.Drawing.Size(95, 33)
    Me.Button5.TabIndex = 5
    Me.Button5.Text = "Offertenversand"
    Me.Button5.UseVisualStyleBackColor = True
    '
    'Form1
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(587, 499)
    Me.Controls.Add(Me.Button5)
    Me.Controls.Add(Me.TextBox1)
    Me.Controls.Add(Me.Button4)
    Me.Controls.Add(Me.Button3)
    Me.Controls.Add(Me.Button2)
    Me.Controls.Add(Me.Button1)
    Me.Name = "Form1"
    Me.Text = "Form1"
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents Button1 As System.Windows.Forms.Button
  Friend WithEvents Button2 As System.Windows.Forms.Button
  Friend WithEvents Button3 As System.Windows.Forms.Button
  Friend WithEvents Button4 As System.Windows.Forms.Button
  Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
  Friend WithEvents Button5 As System.Windows.Forms.Button

End Class
