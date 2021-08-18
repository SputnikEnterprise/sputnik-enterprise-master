<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucSourceBox
	Inherits DevExpress.XtraEditors.XtraUserControl

    'UserControl überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
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
    Me.btnSaveCv = New DevExpress.XtraEditors.SimpleButton()
    Me.btnLogout = New DevExpress.XtraEditors.SimpleButton()
    Me.btnLoadCv = New DevExpress.XtraEditors.SimpleButton()
    Me.pnlCommands = New DevExpress.XtraEditors.PanelControl()
		Me.lblJSessionId = New DevExpress.XtraEditors.LabelControl()
    Me.ctrWebBrowser = New System.Windows.Forms.WebBrowser()
    Me.btnViewStructure = New DevExpress.XtraEditors.SimpleButton()
    CType(Me.pnlCommands, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlCommands.SuspendLayout()
    Me.SuspendLayout()
    '
    'btnSaveCv
    '
    Me.btnSaveCv.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnSaveCv.Location = New System.Drawing.Point(457, 5)
    Me.btnSaveCv.Name = "btnSaveCv"
    Me.btnSaveCv.Size = New System.Drawing.Size(107, 32)
    Me.btnSaveCv.TabIndex = 145
    Me.btnSaveCv.Text = "Save CV"
    '
    'btnLogout
    '
    Me.btnLogout.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnLogout.Location = New System.Drawing.Point(570, 5)
    Me.btnLogout.Name = "btnLogout"
    Me.btnLogout.Size = New System.Drawing.Size(107, 32)
    Me.btnLogout.TabIndex = 144
    Me.btnLogout.Text = "Logout"
    '
    'btnLoadCv
    '
    Me.btnLoadCv.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnLoadCv.Location = New System.Drawing.Point(231, 5)
    Me.btnLoadCv.Name = "btnLoadCv"
    Me.btnLoadCv.Size = New System.Drawing.Size(107, 32)
    Me.btnLoadCv.TabIndex = 143
    Me.btnLoadCv.Text = "Load CV"
    '
    'pnlCommands
    '
    Me.pnlCommands.Controls.Add(Me.lblJSessionId)
    Me.pnlCommands.Controls.Add(Me.btnSaveCv)
    Me.pnlCommands.Controls.Add(Me.btnViewStructure)
    Me.pnlCommands.Controls.Add(Me.btnLoadCv)
    Me.pnlCommands.Controls.Add(Me.btnLogout)
    Me.pnlCommands.Dock = System.Windows.Forms.DockStyle.Bottom
    Me.pnlCommands.Location = New System.Drawing.Point(0, 347)
    Me.pnlCommands.Name = "pnlCommands"
    Me.pnlCommands.Size = New System.Drawing.Size(682, 44)
    Me.pnlCommands.TabIndex = 2
    '
    'lblJSessionId
    '
    Me.lblJSessionId.AutoSize = True
    Me.lblJSessionId.Location = New System.Drawing.Point(5, 14)
    Me.lblJSessionId.Name = "lblJSessionId"
    Me.lblJSessionId.Size = New System.Drawing.Size(68, 13)
    Me.lblJSessionId.TabIndex = 146
    Me.lblJSessionId.Text = "lblJSessionId"
    '
    'ctrWebBrowser
    '
    Me.ctrWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill
    Me.ctrWebBrowser.Location = New System.Drawing.Point(0, 0)
    Me.ctrWebBrowser.MinimumSize = New System.Drawing.Size(20, 20)
    Me.ctrWebBrowser.Name = "ctrWebBrowser"
    Me.ctrWebBrowser.Size = New System.Drawing.Size(682, 347)
    Me.ctrWebBrowser.TabIndex = 3
    '
    'btnViewStructure
    '
    Me.btnViewStructure.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnViewStructure.Location = New System.Drawing.Point(344, 5)
    Me.btnViewStructure.Name = "btnViewStructure"
    Me.btnViewStructure.Size = New System.Drawing.Size(107, 32)
    Me.btnViewStructure.TabIndex = 143
    Me.btnViewStructure.Text = "View Structure"
    '
    'ucSourceBox
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.Controls.Add(Me.ctrWebBrowser)
    Me.Controls.Add(Me.pnlCommands)
    Me.Name = "ucSourceBox"
    Me.Size = New System.Drawing.Size(682, 391)
    CType(Me.pnlCommands, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlCommands.ResumeLayout(False)
    Me.pnlCommands.PerformLayout()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents btnSaveCv As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents btnLogout As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents btnLoadCv As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents pnlCommands As DevExpress.XtraEditors.PanelControl
  Friend WithEvents ctrWebBrowser As System.Windows.Forms.WebBrowser
	Friend WithEvents lblJSessionId As DevExpress.XtraEditors.LabelControl
  Friend WithEvents btnViewStructure As DevExpress.XtraEditors.SimpleButton

End Class
