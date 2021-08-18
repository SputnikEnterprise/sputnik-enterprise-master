<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SingleLineTextInputDialog
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
		Me.btnCancel = New DevExpress.XtraEditors.SimpleButton()
		Me.btnOk = New DevExpress.XtraEditors.SimpleButton()
		Me.lblMessage = New DevExpress.XtraEditors.LabelControl()
		Me.txtInput = New DevExpress.XtraEditors.TextEdit()
		CType(Me.txtInput.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'btnCancel
		'
		Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnCancel.Location = New System.Drawing.Point(205, 74)
		Me.btnCancel.Name = "btnCancel"
		Me.btnCancel.Size = New System.Drawing.Size(79, 24)
		Me.btnCancel.TabIndex = 239
		Me.btnCancel.Text = "Abbrechen"
		'
		'btnOk
		'
		Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnOk.Location = New System.Drawing.Point(124, 74)
		Me.btnOk.Name = "btnOk"
		Me.btnOk.Size = New System.Drawing.Size(75, 24)
		Me.btnOk.TabIndex = 238
		Me.btnOk.Text = "Ok"
		'
		'lblMessage
		'
		Me.lblMessage.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
		Me.lblMessage.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblMessage.Location = New System.Drawing.Point(10, 12)
		Me.lblMessage.Name = "lblMessage"
		Me.lblMessage.Size = New System.Drawing.Size(249, 13)
		Me.lblMessage.TabIndex = 237
		Me.lblMessage.Text = "Bitte geben Sie den Text ein"
		'
		'txtInput
		'
		Me.txtInput.Location = New System.Drawing.Point(7, 38)
		Me.txtInput.Name = "txtInput"
		Me.txtInput.Size = New System.Drawing.Size(277, 20)
		Me.txtInput.TabIndex = 259
		'
		'SingleLineTextInputDialog
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(291, 108)
		Me.ControlBox = False
		Me.Controls.Add(Me.txtInput)
		Me.Controls.Add(Me.btnCancel)
		Me.Controls.Add(Me.btnOk)
		Me.Controls.Add(Me.lblMessage)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.Name = "SingleLineTextInputDialog"
		Me.Text = "Texteingabe"
		CType(Me.txtInput.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub
    Friend WithEvents btnCancel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnOk As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents lblMessage As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtInput As DevExpress.XtraEditors.TextEdit
End Class
