

Imports DevExpress.XtraEditors
Imports DevExpress.Skins
Imports DevExpress.LookAndFeel
Imports DevExpress.UserSkins

Namespace UI

	<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
	Partial Class frmCVDropIn
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
			Me.components = New System.ComponentModel.Container()
			Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCVDropIn))
			Me.lblOpenFile = New DevExpress.XtraEditors.LabelControl()
			Me.lblInfo = New DevExpress.XtraEditors.LabelControl()
			Me.pnlMain = New DevExpress.XtraEditors.PanelControl()
			Me.AlertControl1 = New DevExpress.XtraBars.Alerter.AlertControl(Me.components)
			CType(Me.pnlMain, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.pnlMain.SuspendLayout()
			Me.SuspendLayout()
			'
			'lblOpenFile
			'
			Me.lblOpenFile.AllowHtmlString = True
			Me.lblOpenFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
							Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.lblOpenFile.Appearance.Font = New System.Drawing.Font("Tahoma", 10.0!)
			Me.lblOpenFile.Appearance.ForeColor = System.Drawing.Color.Blue
			Me.lblOpenFile.Appearance.Image = CType(resources.GetObject("lblOpenFile.Appearance.Image"), System.Drawing.Image)
			Me.lblOpenFile.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			Me.lblOpenFile.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblOpenFile.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter
			Me.lblOpenFile.IndentBetweenImageAndText = 10
			Me.lblOpenFile.Location = New System.Drawing.Point(12, 193)
			Me.lblOpenFile.Name = "lblOpenFile"
			Me.lblOpenFile.Size = New System.Drawing.Size(376, 19)
			Me.lblOpenFile.TabIndex = 1
			Me.lblOpenFile.Text = "Stattdessen nach CV-Dateien suchen"
			'
			'lblInfo
			'
			Me.lblInfo.AllowHtmlString = True
			Me.lblInfo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
							Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.lblInfo.Appearance.Font = New System.Drawing.Font("Tahoma", 14.0!)
			Me.lblInfo.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			Me.lblInfo.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblInfo.Location = New System.Drawing.Point(12, 149)
			Me.lblInfo.Name = "lblInfo"
			Me.lblInfo.Size = New System.Drawing.Size(376, 38)
			Me.lblInfo.TabIndex = 0
			Me.lblInfo.Text = "CV-Dateien hierhin ziehen..."
			'
			'pnlMain
			'
			Me.pnlMain.AllowDrop = True
			Me.pnlMain.Appearance.Image = CType(resources.GetObject("pnlMain.Appearance.Image"), System.Drawing.Image)
			Me.pnlMain.Appearance.Options.UseImage = True
			Me.pnlMain.ContentImage = CType(resources.GetObject("pnlMain.ContentImage"), System.Drawing.Image)
			Me.pnlMain.Controls.Add(Me.lblOpenFile)
			Me.pnlMain.Controls.Add(Me.lblInfo)
			Me.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill
			Me.pnlMain.Location = New System.Drawing.Point(0, 0)
			Me.pnlMain.Name = "pnlMain"
			Me.pnlMain.Size = New System.Drawing.Size(400, 236)
			Me.pnlMain.TabIndex = 1
			'
			'AlertControl1
			'
			Me.AlertControl1.AllowHtmlText = True
			'
			'frmCVDropIn
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(400, 236)
			Me.Controls.Add(Me.pnlMain)
			Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
			Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
			Me.Name = "frmCVDropIn"
			Me.Text = "CV Drop-In"
			CType(Me.pnlMain, System.ComponentModel.ISupportInitialize).EndInit()
			Me.pnlMain.ResumeLayout(False)
			Me.ResumeLayout(False)

		End Sub
		Friend WithEvents lblOpenFile As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblInfo As DevExpress.XtraEditors.LabelControl
		Friend WithEvents pnlMain As DevExpress.XtraEditors.PanelControl
		Friend WithEvents AlertControl1 As DevExpress.XtraBars.Alerter.AlertControl
	End Class

End Namespace
