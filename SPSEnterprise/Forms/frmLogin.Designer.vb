<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmLogin
	Inherits DevExpress.XtraEditors.XtraForm

	'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
	<System.Diagnostics.DebuggerNonUserCode()>
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
	<System.Diagnostics.DebuggerStepThrough()>
	Private Sub InitializeComponent()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLogin))
		Me.txtPassword = New DevExpress.XtraEditors.TextEdit()
		Me.Picture1 = New DevExpress.XtraEditors.PanelControl()
		Me.lblHeader1 = New DevExpress.XtraEditors.LabelControl()
		Me.lblHeader2 = New DevExpress.XtraEditors.LabelControl()
		Me.mandantName = New DevExpress.XtraEditors.LabelControl()
		Me.lblPassword = New DevExpress.XtraEditors.LabelControl()
		Me.txtUserName = New DevExpress.XtraEditors.TextEdit()
		Me.lblUser = New DevExpress.XtraEditors.LabelControl()
		Me.CmdXPOK = New DevExpress.XtraEditors.SimpleButton()
		Me.CmdXPCancel = New DevExpress.XtraEditors.SimpleButton()
		Me.lblMandant = New DevExpress.XtraEditors.LabelControl()
		CType(Me.txtPassword.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Picture1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.Picture1.SuspendLayout()
		CType(Me.txtUserName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'txtPassword
		'
		Me.txtPassword.Location = New System.Drawing.Point(129, 143)
		Me.txtPassword.Name = "txtPassword"
		Me.txtPassword.Properties.NullValuePrompt = "Ihr Kennwort"
		Me.txtPassword.Properties.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
		Me.txtPassword.Size = New System.Drawing.Size(157, 20)
		Me.txtPassword.TabIndex = 1
		'
		'Picture1
		'
		Me.Picture1.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.Picture1.Appearance.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Picture1.Appearance.Options.UseBackColor = True
		Me.Picture1.Appearance.Options.UseForeColor = True
		Me.Picture1.Controls.Add(Me.lblHeader1)
		Me.Picture1.Controls.Add(Me.lblHeader2)
		Me.Picture1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Picture1.Dock = System.Windows.Forms.DockStyle.Top
		Me.Picture1.Location = New System.Drawing.Point(0, 0)
		Me.Picture1.Name = "Picture1"
		Me.Picture1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Picture1.Size = New System.Drawing.Size(434, 60)
		Me.Picture1.TabIndex = 20
		'
		'lblHeader1
		'
		Me.lblHeader1.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.lblHeader1.Appearance.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblHeader1.Appearance.ForeColor = System.Drawing.Color.Black
		Me.lblHeader1.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblHeader1.Location = New System.Drawing.Point(18, 12)
		Me.lblHeader1.Name = "lblHeader1"
		Me.lblHeader1.Size = New System.Drawing.Size(163, 13)
		Me.lblHeader1.TabIndex = 8
		Me.lblHeader1.Text = "Kennwort ist erforderlich"
		'
		'lblHeader2
		'
		Me.lblHeader2.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.lblHeader2.Appearance.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblHeader2.Appearance.ForeColor = System.Drawing.Color.Black
		Me.lblHeader2.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblHeader2.Location = New System.Drawing.Point(32, 29)
		Me.lblHeader2.Name = "lblHeader2"
		Me.lblHeader2.Size = New System.Drawing.Size(237, 13)
		Me.lblHeader2.TabIndex = 3
		Me.lblHeader2.Text = "Bitte tragen Sie Ihre Benutzerdaten ein..."
		'
		'mandantName
		'
		Me.mandantName.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.mandantName.Appearance.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.mandantName.Appearance.ForeColor = System.Drawing.Color.Black
		Me.mandantName.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
		Me.mandantName.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
		Me.mandantName.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.mandantName.Cursor = System.Windows.Forms.Cursors.Default
		Me.mandantName.Location = New System.Drawing.Point(126, 77)
		Me.mandantName.Name = "mandantName"
		Me.mandantName.Size = New System.Drawing.Size(270, 34)
		Me.mandantName.TabIndex = 9
		Me.mandantName.Text = "MDName"
		'
		'lblPassword
		'
		Me.lblPassword.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.lblPassword.Appearance.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPassword.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblPassword.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblPassword.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPassword.Location = New System.Drawing.Point(18, 148)
		Me.lblPassword.Name = "lblPassword"
		Me.lblPassword.Size = New System.Drawing.Size(105, 13)
		Me.lblPassword.TabIndex = 19
		Me.lblPassword.Text = "Kennwort"
		'
		'txtUserName
		'
		Me.txtUserName.Location = New System.Drawing.Point(129, 117)
		Me.txtUserName.Name = "txtUserName"
		Me.txtUserName.Properties.NullValuePrompt = "Ihr Benutzername"
		Me.txtUserName.Size = New System.Drawing.Size(157, 20)
		Me.txtUserName.TabIndex = 0
		'
		'lblUser
		'
		Me.lblUser.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.lblUser.Appearance.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblUser.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblUser.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblUser.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblUser.Location = New System.Drawing.Point(21, 122)
		Me.lblUser.Name = "lblUser"
		Me.lblUser.Size = New System.Drawing.Size(102, 13)
		Me.lblUser.TabIndex = 24
		Me.lblUser.Text = "Benutzer"
		'
		'CmdXPOK
		'
		Me.CmdXPOK.Location = New System.Drawing.Point(321, 117)
		Me.CmdXPOK.Name = "CmdXPOK"
		Me.CmdXPOK.Size = New System.Drawing.Size(75, 23)
		Me.CmdXPOK.TabIndex = 2
		Me.CmdXPOK.Text = "Anmelden"
		'
		'CmdXPCancel
		'
		Me.CmdXPCancel.Location = New System.Drawing.Point(321, 146)
		Me.CmdXPCancel.Name = "CmdXPCancel"
		Me.CmdXPCancel.Size = New System.Drawing.Size(75, 23)
		Me.CmdXPCancel.TabIndex = 3
		Me.CmdXPCancel.Text = "Abbrechen"
		'
		'lblMandant
		'
		Me.lblMandant.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.lblMandant.Appearance.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblMandant.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblMandant.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblMandant.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMandant.Location = New System.Drawing.Point(22, 78)
		Me.lblMandant.Name = "lblMandant"
		Me.lblMandant.Size = New System.Drawing.Size(101, 13)
		Me.lblMandant.TabIndex = 25
		Me.lblMandant.Text = "Mandant"
		'
		'frmLogin
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(434, 216)
		Me.Controls.Add(Me.lblMandant)
		Me.Controls.Add(Me.mandantName)
		Me.Controls.Add(Me.CmdXPCancel)
		Me.Controls.Add(Me.CmdXPOK)
		Me.Controls.Add(Me.txtUserName)
		Me.Controls.Add(Me.lblUser)
		Me.Controls.Add(Me.txtPassword)
		Me.Controls.Add(Me.Picture1)
		Me.Controls.Add(Me.lblPassword)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmLogin"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Text = "Anmeldung"
		CType(Me.txtPassword.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Picture1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.Picture1.ResumeLayout(False)
		Me.Picture1.PerformLayout()
		CType(Me.txtUserName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub
	'  System.Windows.Forms.TextBox
	Public WithEvents Picture1 As DevExpress.XtraEditors.PanelControl
	Public WithEvents lblHeader1 As DevExpress.XtraEditors.LabelControl
	Public WithEvents lblHeader2 As DevExpress.XtraEditors.LabelControl
	Public WithEvents lblPassword As DevExpress.XtraEditors.LabelControl
	' System.Windows.Forms.TextBox
	Public WithEvents lblUser As DevExpress.XtraEditors.LabelControl
	Friend WithEvents CmdXPOK As DevExpress.XtraEditors.SimpleButton ' System.Windows.Forms.Button
	Friend WithEvents CmdXPCancel As DevExpress.XtraEditors.SimpleButton ' System.Windows.Forms.Button
	Public WithEvents mandantName As DevExpress.XtraEditors.LabelControl
	Friend WithEvents txtPassword As DevExpress.XtraEditors.TextEdit ' System.Windows.Forms.TextBox
	Friend WithEvents txtUserName As DevExpress.XtraEditors.TextEdit
	Public WithEvents lblMandant As DevExpress.XtraEditors.LabelControl ' System.Windows.Forms.TextBox
End Class
