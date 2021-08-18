<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmNewESLohn
    Inherits DevExpress.XtraEditors.XtraForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
    Me.btnAddESSalaryData = New DevExpress.XtraEditors.SimpleButton()
    Me.ucESSalaryData = New SP.MA.EinsatzMng.UI.ucPageSelectSalaryData()
    Me.btnCancel = New DevExpress.XtraEditors.SimpleButton()
    Me.SuspendLayout()
    '
    'btnAddESSalaryData
    '
    Me.btnAddESSalaryData.Location = New System.Drawing.Point(521, 366)
    Me.btnAddESSalaryData.Name = "btnAddESSalaryData"
    Me.btnAddESSalaryData.Size = New System.Drawing.Size(75, 23)
    Me.btnAddESSalaryData.TabIndex = 1
    Me.btnAddESSalaryData.Text = "Anlegen"
    '
    'ucESSalaryData
    '
    Me.ucESSalaryData.Appearance.BackColor = System.Drawing.Color.White
    Me.ucESSalaryData.Appearance.Options.UseBackColor = True
    Me.ucESSalaryData.Location = New System.Drawing.Point(11, 11)
    Me.ucESSalaryData.Name = "ucESSalaryData"
    Me.ucESSalaryData.Size = New System.Drawing.Size(685, 349)
    Me.ucESSalaryData.TabIndex = 0
    Me.ucESSalaryData.UCMediator = Nothing
    '
    'btnCancel
    '
    Me.btnCancel.Location = New System.Drawing.Point(614, 366)
    Me.btnCancel.Name = "btnCancel"
    Me.btnCancel.Size = New System.Drawing.Size(75, 23)
    Me.btnCancel.TabIndex = 2
    Me.btnCancel.Text = "Abbrechen"
    '
    'frmNewESLohn
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(701, 396)
    Me.Controls.Add(Me.btnCancel)
    Me.Controls.Add(Me.btnAddESSalaryData)
    Me.Controls.Add(Me.ucESSalaryData)
    Me.Name = "frmNewESLohn"
    Me.Text = "Neue Lohndaten"
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents ucESSalaryData As SP.MA.EinsatzMng.UI.ucPageSelectSalaryData
  Friend WithEvents btnAddESSalaryData As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents btnCancel As DevExpress.XtraEditors.SimpleButton
End Class
