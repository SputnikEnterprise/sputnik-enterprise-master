
Partial Class frmMDLogin

	Private components As System.ComponentModel.IContainer = Nothing

	'<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> 

	'	Inherits XtraForm


	''' <summary>
	''' Clean up any resources being used.
	''' </summary>
	''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	Protected Overrides Sub Dispose(ByVal disposing As Boolean)
		If disposing AndAlso (components IsNot Nothing) Then
			components.Dispose()
		End If
		MyBase.Dispose(disposing)
	End Sub


#Region "Vom Windows Form-Designer generierter Code "
	'<System.Diagnostics.DebuggerNonUserCode()> Public Sub New(ByVal _setting As ClsMDData)

	'	' Dieser Aufruf ist für den Designer erforderlich.
	'	DevExpress.UserSkins.BonusSkins.Register()
	'	DevExpress.Skins.SkinManager.EnableFormSkins()

	'	'MyBase.New()
	'	'Dieser Aufruf ist für den Windows Form-Designer erforderlich.
	'	InitializeComponent()

	'	Me._ClsSetting = _setting

	'End Sub

	''Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
	'<System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
	'	If Disposing Then
	'		If Not components Is Nothing Then
	'			components.Dispose()
	'		End If
	'	End If
	'	MyBase.Dispose(Disposing)
	'End Sub


	'Wird vom Windows Form-Designer benötigt.
	'Private components As System.ComponentModel.IContainer
	Public WithEvents lblHeader1 As System.Windows.Forms.Label
	Public WithEvents lblHeader2 As System.Windows.Forms.Label
	Public WithEvents Picture1 As DevExpress.XtraEditors.PanelControl
	Public WithEvents lblPassword As System.Windows.Forms.Label
	'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
	'Das Verändern mit dem Windows Form-Designer ist nicht möglich.
	'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
	Private Sub InitializeComponent()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMDLogin))
		Me.Picture1 = New DevExpress.XtraEditors.PanelControl()
		Me.lblHeader1 = New System.Windows.Forms.Label()
		Me.lblHeader2 = New System.Windows.Forms.Label()
		Me.lblPassword = New System.Windows.Forms.Label()
		Me.txtPassword = New DevExpress.XtraEditors.TextEdit()
		Me.CmdXPCancel = New DevExpress.XtraEditors.SimpleButton()
		Me.CmdXPOK = New DevExpress.XtraEditors.SimpleButton()
		Me.Picture1.SuspendLayout()
		CType(Me.txtPassword.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'Picture1
		'
		Me.Picture1.BackColor = Color.Transparent
		Me.Picture1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default
		Me.Picture1.Controls.Add(Me.lblHeader1)
		Me.Picture1.Controls.Add(Me.lblHeader2)
		Me.Picture1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Picture1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Picture1.Location = New System.Drawing.Point(-6, -2)
		Me.Picture1.Name = "Picture1"
		Me.Picture1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Picture1.Size = New System.Drawing.Size(496, 63)
		Me.Picture1.TabIndex = 2
		'
		'lblHeader1
		'
		Me.lblHeader1.AutoSize = True
		Me.lblHeader1.BackColor = System.Drawing.Color.Transparent
		Me.lblHeader1.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblHeader1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblHeader1.ForeColor = System.Drawing.Color.Black
		Me.lblHeader1.Location = New System.Drawing.Point(18, 12)
		Me.lblHeader1.Name = "lblHeader1"
		Me.lblHeader1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblHeader1.Size = New System.Drawing.Size(170, 13)
		Me.lblHeader1.TabIndex = 8
		Me.lblHeader1.Text = "Kennwort ist erforderlich"
		'
		'lblHeader2
		'
		Me.lblHeader2.AutoSize = True
		Me.lblHeader2.BackColor = System.Drawing.Color.Transparent
		Me.lblHeader2.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblHeader2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblHeader2.ForeColor = System.Drawing.Color.Black
		Me.lblHeader2.Location = New System.Drawing.Point(31, 29)
		Me.lblHeader2.Name = "lblHeader2"
		Me.lblHeader2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblHeader2.Size = New System.Drawing.Size(340, 13)
		Me.lblHeader2.TabIndex = 3
		Me.lblHeader2.Text = "Bitte tragen Sie im untenstehenden Feld Ihr Kennwort ein."
		'
		'lblPassword
		'
		Me.lblPassword.BackColor = System.Drawing.Color.Transparent
		Me.lblPassword.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPassword.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPassword.Location = New System.Drawing.Point(12, 91)
		Me.lblPassword.Name = "lblPassword"
		Me.lblPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPassword.Size = New System.Drawing.Size(70, 13)
		Me.lblPassword.TabIndex = 1
		Me.lblPassword.Text = "Kennwort"
		Me.lblPassword.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'txtPassword
		'
		Me.txtPassword.Location = New System.Drawing.Point(88, 88)
		Me.txtPassword.Name = "txtPassword"
		Me.txtPassword.Size = New System.Drawing.Size(157, 20)
		Me.txtPassword.TabIndex = 11
		'
		'CmdXPCancel
		'
		Me.CmdXPCancel.Location = New System.Drawing.Point(292, 117)
		Me.CmdXPCancel.Name = "CmdXPCancel"
		Me.CmdXPCancel.Size = New System.Drawing.Size(75, 23)
		Me.CmdXPCancel.TabIndex = 29
		Me.CmdXPCancel.Text = "Schliessen"
		'
		'CmdXPOK
		'
		Me.CmdXPOK.Location = New System.Drawing.Point(292, 88)
		Me.CmdXPOK.Name = "CmdXPOK"
		Me.CmdXPOK.Size = New System.Drawing.Size(75, 23)
		Me.CmdXPOK.TabIndex = 28
		Me.CmdXPOK.Text = "Anmelden"
		'
		'frmMDLogin
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(412, 169)
		Me.Controls.Add(Me.CmdXPCancel)
		Me.Controls.Add(Me.CmdXPOK)
		Me.Controls.Add(Me.txtPassword)
		Me.Controls.Add(Me.Picture1)
		Me.Controls.Add(Me.lblPassword)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.KeyPreview = True
		Me.Location = New System.Drawing.Point(288, 122)
		Me.MaximizeBox = False
		Me.MaximumSize = New System.Drawing.Size(418, 197)
		Me.MinimizeBox = False
		Me.Name = "frmMDLogin"
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Tag = "1"
		Me.Text = "Authentifizierung"
		Me.Picture1.ResumeLayout(False)
		Me.Picture1.PerformLayout()
		CType(Me.txtPassword.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub

	Friend WithEvents txtPassword As DevExpress.XtraEditors.TextEdit ' System.Windows.Forms.TextBox
	Friend WithEvents CmdXPCancel As DevExpress.XtraEditors.SimpleButton ' System.Windows.Forms.Button
	Friend WithEvents CmdXPOK As DevExpress.XtraEditors.SimpleButton ' System.Windows.Forms.Button
#End Region

End Class