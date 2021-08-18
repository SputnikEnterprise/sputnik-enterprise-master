<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MDList
    Inherits DevExpress.XtraEditors.XtraUserControl

    'UserControl overrides dispose to clean up the component list.
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
		Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
		Me.grdMDList = New DevExpress.XtraGrid.GridControl()
		Me.gvMDList = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.grpMDLogin = New DevExpress.XtraEditors.GroupControl()
		Me.cmdMDLoginCancel = New DevExpress.XtraEditors.SimpleButton()
		Me.cmdMDLogin = New DevExpress.XtraEditors.SimpleButton()
		Me.txtMDLogin = New DevExpress.XtraEditors.TextEdit()
		Me.PanelControl2 = New DevExpress.XtraEditors.PanelControl()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.lblMDHeader1 = New System.Windows.Forms.Label()
		Me.lblmdpasswort = New System.Windows.Forms.Label()
		Me.grpUserLoging = New DevExpress.XtraEditors.GroupControl()
		Me.CmdLoginCancel = New DevExpress.XtraEditors.SimpleButton()
		Me.CmdLogin = New DevExpress.XtraEditors.SimpleButton()
		Me.txtUserName = New DevExpress.XtraEditors.TextEdit()
		Me.lblUser = New System.Windows.Forms.Label()
		Me.txtPassword = New DevExpress.XtraEditors.TextEdit()
		Me.Picture1 = New DevExpress.XtraEditors.PanelControl()
		Me.lblMDName = New System.Windows.Forms.Label()
		Me.lblHeaderNormal = New System.Windows.Forms.Label()
		Me.lblPassword = New System.Windows.Forms.Label()
		Me.LblChanged_2 = New System.Windows.Forms.Label()
		Me.lblMandanten = New System.Windows.Forms.Label()
		CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.PanelControl1.SuspendLayout()
		CType(Me.grdMDList, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvMDList, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.grpMDLogin, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grpMDLogin.SuspendLayout()
		CType(Me.txtMDLogin.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.PanelControl2.SuspendLayout()
		CType(Me.grpUserLoging, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grpUserLoging.SuspendLayout()
		CType(Me.txtUserName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtPassword.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Picture1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.Picture1.SuspendLayout()
		Me.SuspendLayout()
		'
		'PanelControl1
		'
		Me.PanelControl1.Controls.Add(Me.grdMDList)
		Me.PanelControl1.Controls.Add(Me.grpMDLogin)
		Me.PanelControl1.Controls.Add(Me.grpUserLoging)
		Me.PanelControl1.Controls.Add(Me.LblChanged_2)
		Me.PanelControl1.Controls.Add(Me.lblMandanten)
		Me.PanelControl1.Dock = DockStyle.Fill
		Me.PanelControl1.Location = New System.Drawing.Point(20, 20)
		Me.PanelControl1.Name = "PanelControl1"
		Me.PanelControl1.Padding = New System.Windows.Forms.Padding(10)
		Me.PanelControl1.Size = New System.Drawing.Size(815, 517)
		Me.PanelControl1.TabIndex = 0
		'
		'grdMDList
		'
		Me.grdMDList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.grdMDList.Location = New System.Drawing.Point(5, 30)
		Me.grdMDList.MainView = Me.gvMDList
		Me.grdMDList.Name = "grdMDList"
		Me.grdMDList.Size = New System.Drawing.Size(413, 472)
		Me.grdMDList.TabIndex = 0
		Me.grdMDList.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvMDList})
		'
		'gvMDList
		'
		Me.gvMDList.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvMDList.GridControl = Me.grdMDList
		Me.gvMDList.Name = "gvMDList"
		Me.gvMDList.OptionsBehavior.Editable = False
		Me.gvMDList.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvMDList.OptionsView.ShowAutoFilterRow = True
		Me.gvMDList.OptionsView.ShowGroupPanel = False
		'
		'grpMDLogin
		'
		Me.grpMDLogin.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.grpMDLogin.Controls.Add(Me.cmdMDLoginCancel)
		Me.grpMDLogin.Controls.Add(Me.cmdMDLogin)
		Me.grpMDLogin.Controls.Add(Me.txtMDLogin)
		Me.grpMDLogin.Controls.Add(Me.PanelControl2)
		Me.grpMDLogin.Controls.Add(Me.lblmdpasswort)
		Me.grpMDLogin.Location = New System.Drawing.Point(424, 222)
		Me.grpMDLogin.Name = "grpMDLogin"
		Me.grpMDLogin.Size = New System.Drawing.Size(380, 186)
		Me.grpMDLogin.TabIndex = 18
		Me.grpMDLogin.Text = "Kennwort für Mandantenzugriff ist erforderlich"
		'
		'cmdMDLoginCancel
		'
		Me.cmdMDLoginCancel.Location = New System.Drawing.Point(282, 131)
		Me.cmdMDLoginCancel.Name = "cmdMDLoginCancel"
		Me.cmdMDLoginCancel.Size = New System.Drawing.Size(75, 23)
		Me.cmdMDLoginCancel.TabIndex = 28
		Me.cmdMDLoginCancel.Text = "Abbrechen"
		'
		'cmdMDLogin
		'
		Me.cmdMDLogin.Location = New System.Drawing.Point(282, 102)
		Me.cmdMDLogin.Name = "cmdMDLogin"
		Me.cmdMDLogin.Size = New System.Drawing.Size(75, 23)
		Me.cmdMDLogin.TabIndex = 27
		Me.cmdMDLogin.Text = "Anmelden"
		'
		'txtMDLogin
		'
		Me.txtMDLogin.Location = New System.Drawing.Point(95, 105)
		Me.txtMDLogin.Name = "txtMDLogin"
		Me.txtMDLogin.Properties.NullValuePrompt = "Ihr Kennwort"
		Me.txtMDLogin.Properties.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
		Me.txtMDLogin.Size = New System.Drawing.Size(157, 20)
		Me.txtMDLogin.TabIndex = 26
		'
		'PanelControl2
		'
		Me.PanelControl2.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.PanelControl2.Appearance.ForeColor = System.Drawing.SystemColors.ControlText
		Me.PanelControl2.Appearance.Options.UseBackColor = True
		Me.PanelControl2.Appearance.Options.UseForeColor = True
		Me.PanelControl2.Controls.Add(Me.Label2)
		Me.PanelControl2.Controls.Add(Me.lblMDHeader1)
		Me.PanelControl2.Cursor = System.Windows.Forms.Cursors.Default
		Me.PanelControl2.Dock = DockStyle.Top
		Me.PanelControl2.Location = New System.Drawing.Point(2, 20)
		Me.PanelControl2.Name = "PanelControl2"
		Me.PanelControl2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.PanelControl2.Size = New System.Drawing.Size(376, 63)
		Me.PanelControl2.TabIndex = 30
		'
		'Label2
		'
		Me.Label2.AutoSize = True
		Me.Label2.BackColor = System.Drawing.Color.Transparent
		Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label2.ForeColor = System.Drawing.Color.Black
		Me.Label2.Location = New System.Drawing.Point(32, 29)
		Me.Label2.Name = "Label2"
		Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label2.Size = New System.Drawing.Size(63, 13)
		Me.Label2.TabIndex = 9
		Me.Label2.Text = "MDName"
		'
		'lblMDHeader1
		'
		Me.lblMDHeader1.AutoSize = True
		Me.lblMDHeader1.BackColor = System.Drawing.Color.Transparent
		Me.lblMDHeader1.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMDHeader1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblMDHeader1.ForeColor = System.Drawing.Color.Black
		Me.lblMDHeader1.Location = New System.Drawing.Point(32, 11)
		Me.lblMDHeader1.Name = "lblMDHeader1"
		Me.lblMDHeader1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblMDHeader1.Size = New System.Drawing.Size(205, 13)
		Me.lblMDHeader1.TabIndex = 3
		Me.lblMDHeader1.Text = "Bitte tragen Sie Ihr Passwort ein..."
		'
		'lblmdpasswort
		'
		Me.lblmdpasswort.BackColor = System.Drawing.Color.Transparent
		Me.lblmdpasswort.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblmdpasswort.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblmdpasswort.Location = New System.Drawing.Point(20, 109)
		Me.lblmdpasswort.Name = "lblmdpasswort"
		Me.lblmdpasswort.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblmdpasswort.Size = New System.Drawing.Size(69, 13)
		Me.lblmdpasswort.TabIndex = 29
		Me.lblmdpasswort.Text = "Kennwort"
		Me.lblmdpasswort.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'grpUserLoging
		'
		Me.grpUserLoging.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.grpUserLoging.Controls.Add(Me.CmdLoginCancel)
		Me.grpUserLoging.Controls.Add(Me.CmdLogin)
		Me.grpUserLoging.Controls.Add(Me.txtUserName)
		Me.grpUserLoging.Controls.Add(Me.lblUser)
		Me.grpUserLoging.Controls.Add(Me.txtPassword)
		Me.grpUserLoging.Controls.Add(Me.Picture1)
		Me.grpUserLoging.Controls.Add(Me.lblPassword)
		Me.grpUserLoging.Location = New System.Drawing.Point(424, 30)
		Me.grpUserLoging.Name = "grpUserLoging"
		Me.grpUserLoging.Size = New System.Drawing.Size(380, 186)
		Me.grpUserLoging.TabIndex = 17
		Me.grpUserLoging.Text = "Kennwort ist erforderlich"
		'
		'CmdLoginCancel
		'
		Me.CmdLoginCancel.Location = New System.Drawing.Point(282, 130)
		Me.CmdLoginCancel.Name = "CmdLoginCancel"
		Me.CmdLoginCancel.Size = New System.Drawing.Size(75, 23)
		Me.CmdLoginCancel.TabIndex = 28
		Me.CmdLoginCancel.Text = "Abbrechen"
		'
		'CmdLogin
		'
		Me.CmdLogin.Location = New System.Drawing.Point(282, 101)
		Me.CmdLogin.Name = "CmdLogin"
		Me.CmdLogin.Size = New System.Drawing.Size(75, 23)
		Me.CmdLogin.TabIndex = 27
		Me.CmdLogin.Text = "Anmelden"
		'
		'txtUserName
		'
		Me.txtUserName.Location = New System.Drawing.Point(95, 101)
		Me.txtUserName.Name = "txtUserName"
		Me.txtUserName.Properties.NullValuePrompt = "Ihr Benutzername"
		Me.txtUserName.Size = New System.Drawing.Size(157, 20)
		Me.txtUserName.TabIndex = 25
		'
		'lblUser
		'
		Me.lblUser.BackColor = System.Drawing.Color.Transparent
		Me.lblUser.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblUser.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblUser.Location = New System.Drawing.Point(20, 105)
		Me.lblUser.Name = "lblUser"
		Me.lblUser.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblUser.Size = New System.Drawing.Size(69, 13)
		Me.lblUser.TabIndex = 31
		Me.lblUser.Text = "Benutzer"
		Me.lblUser.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'txtPassword
		'
		Me.txtPassword.Location = New System.Drawing.Point(95, 127)
		Me.txtPassword.Name = "txtPassword"
		Me.txtPassword.Properties.NullValuePrompt = "Ihr Kennwort"
		Me.txtPassword.Properties.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
		Me.txtPassword.Size = New System.Drawing.Size(157, 20)
		Me.txtPassword.TabIndex = 26
		'
		'Picture1
		'
		Me.Picture1.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.Picture1.Appearance.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Picture1.Appearance.Options.UseBackColor = True
		Me.Picture1.Appearance.Options.UseForeColor = True
		Me.Picture1.Controls.Add(Me.lblMDName)
		Me.Picture1.Controls.Add(Me.lblHeaderNormal)
		Me.Picture1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Picture1.Dock = DockStyle.Top
		Me.Picture1.Location = New System.Drawing.Point(2, 20)
		Me.Picture1.Name = "Picture1"
		Me.Picture1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Picture1.Size = New System.Drawing.Size(376, 63)
		Me.Picture1.TabIndex = 30
		'
		'lblMDName
		'
		Me.lblMDName.AutoSize = True
		Me.lblMDName.BackColor = System.Drawing.Color.Transparent
		Me.lblMDName.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMDName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblMDName.ForeColor = System.Drawing.Color.Black
		Me.lblMDName.Location = New System.Drawing.Point(32, 29)
		Me.lblMDName.Name = "lblMDName"
		Me.lblMDName.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblMDName.Size = New System.Drawing.Size(63, 13)
		Me.lblMDName.TabIndex = 9
		Me.lblMDName.Text = "MDName"
		'
		'lblHeaderNormal
		'
		Me.lblHeaderNormal.AutoSize = True
		Me.lblHeaderNormal.BackColor = System.Drawing.Color.Transparent
		Me.lblHeaderNormal.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblHeaderNormal.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblHeaderNormal.ForeColor = System.Drawing.Color.Black
		Me.lblHeaderNormal.Location = New System.Drawing.Point(32, 11)
		Me.lblHeaderNormal.Name = "lblHeaderNormal"
		Me.lblHeaderNormal.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblHeaderNormal.Size = New System.Drawing.Size(244, 13)
		Me.lblHeaderNormal.TabIndex = 3
		Me.lblHeaderNormal.Text = "Bitte tragen Sie Ihre Benutzerdaten ein..."
		'
		'lblPassword
		'
		Me.lblPassword.BackColor = System.Drawing.Color.Transparent
		Me.lblPassword.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPassword.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPassword.Location = New System.Drawing.Point(20, 131)
		Me.lblPassword.Name = "lblPassword"
		Me.lblPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPassword.Size = New System.Drawing.Size(69, 13)
		Me.lblPassword.TabIndex = 29
		Me.lblPassword.Text = "Kennwort"
		Me.lblPassword.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'LblChanged_2
		'
		Me.LblChanged_2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.LblChanged_2.BackColor = System.Drawing.Color.Transparent
		Me.LblChanged_2.Cursor = System.Windows.Forms.Cursors.Default
		Me.LblChanged_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.LblChanged_2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
		Me.LblChanged_2.Location = New System.Drawing.Point(216, 12)
		Me.LblChanged_2.Name = "LblChanged_2"
		Me.LblChanged_2.Size = New System.Drawing.Size(202, 14)
		Me.LblChanged_2.TabIndex = 16
		Me.LblChanged_2.Text = "Keine Daten vorhanden..."
		Me.LblChanged_2.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblMandanten
		'
		Me.lblMandanten.AutoSize = True
		Me.lblMandanten.BackColor = System.Drawing.Color.Transparent
		Me.lblMandanten.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMandanten.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblMandanten.Location = New System.Drawing.Point(5, 12)
		Me.lblMandanten.Name = "lblMandanten"
		Me.lblMandanten.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblMandanten.Size = New System.Drawing.Size(61, 13)
		Me.lblMandanten.TabIndex = 15
		Me.lblMandanten.Text = "Mandanten"
		'
		'MDList
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.Controls.Add(Me.PanelControl1)
		Me.Name = "MDList"
		Me.Padding = New System.Windows.Forms.Padding(20)
		Me.Size = New System.Drawing.Size(855, 557)
		CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.PanelControl1.ResumeLayout(False)
		Me.PanelControl1.PerformLayout()
		CType(Me.grdMDList, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvMDList, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.grpMDLogin, System.ComponentModel.ISupportInitialize).EndInit()
		Me.grpMDLogin.ResumeLayout(False)
		CType(Me.txtMDLogin.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).EndInit()
		Me.PanelControl2.ResumeLayout(False)
		Me.PanelControl2.PerformLayout()
		CType(Me.grpUserLoging, System.ComponentModel.ISupportInitialize).EndInit()
		Me.grpUserLoging.ResumeLayout(False)
		CType(Me.txtUserName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtPassword.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Picture1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.Picture1.ResumeLayout(False)
		Me.Picture1.PerformLayout()
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
	Public WithEvents LblChanged_2 As System.Windows.Forms.Label
	Public WithEvents lblMandanten As System.Windows.Forms.Label
	Friend WithEvents grpUserLoging As DevExpress.XtraEditors.GroupControl
	Friend WithEvents CmdLoginCancel As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents CmdLogin As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents txtUserName As DevExpress.XtraEditors.TextEdit
	Public WithEvents lblUser As System.Windows.Forms.Label
	Friend WithEvents txtPassword As DevExpress.XtraEditors.TextEdit
	Public WithEvents Picture1 As DevExpress.XtraEditors.PanelControl
	Public WithEvents lblMDName As System.Windows.Forms.Label
	Public WithEvents lblHeaderNormal As System.Windows.Forms.Label
	Public WithEvents lblPassword As System.Windows.Forms.Label
	Friend WithEvents grpMDLogin As DevExpress.XtraEditors.GroupControl
	Friend WithEvents cmdMDLoginCancel As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents cmdMDLogin As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents txtMDLogin As DevExpress.XtraEditors.TextEdit
	Public WithEvents PanelControl2 As DevExpress.XtraEditors.PanelControl
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents lblMDHeader1 As System.Windows.Forms.Label
	Public WithEvents lblmdpasswort As System.Windows.Forms.Label
	Friend WithEvents grdMDList As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvMDList As DevExpress.XtraGrid.Views.Grid.GridView

End Class
