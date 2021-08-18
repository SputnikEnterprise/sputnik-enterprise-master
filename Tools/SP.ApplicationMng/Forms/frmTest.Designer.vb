<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmStartProgram
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
		Me.chkScan = New DevExpress.XtraEditors.CheckEdit()
		Me.chkEMailParser = New DevExpress.XtraEditors.CheckEdit()
		Me.chkNotifier = New DevExpress.XtraEditors.CheckEdit()
		Me.chkCVLizer = New DevExpress.XtraEditors.CheckEdit()
		Me.btnStartScan = New DevExpress.XtraEditors.SimpleButton()
		Me.btnStartEMailParsing = New DevExpress.XtraEditors.SimpleButton()
		Me.btnStartCVLizer = New DevExpress.XtraEditors.SimpleButton()
		Me.btnStartNotifier = New DevExpress.XtraEditors.SimpleButton()
		Me.btnReadXMLForCommunity = New DevExpress.XtraEditors.SimpleButton()
		Me.lblSettingfilename = New DevExpress.XtraEditors.LabelControl()
		Me.btnCountFiles = New DevExpress.XtraEditors.SimpleButton()
		Me.lblLocalIPAddress = New DevExpress.XtraEditors.LabelControl()
		CType(Me.chkScan.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkEMailParser.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkNotifier.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkCVLizer.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'chkScan
		'
		Me.chkScan.Location = New System.Drawing.Point(45, 41)
		Me.chkScan.Name = "chkScan"
		Me.chkScan.Properties.Appearance.Options.UseTextOptions = True
		Me.chkScan.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
		Me.chkScan.Properties.Caption = "Starte automatische Scannvorgang"
		Me.chkScan.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.chkScan.Size = New System.Drawing.Size(317, 20)
		Me.chkScan.TabIndex = 4
		'
		'chkEMailParser
		'
		Me.chkEMailParser.Location = New System.Drawing.Point(45, 93)
		Me.chkEMailParser.Name = "chkEMailParser"
		Me.chkEMailParser.Properties.Appearance.Options.UseTextOptions = True
		Me.chkEMailParser.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
		Me.chkEMailParser.Properties.Caption = "Starte automatische eMail-Watching"
		Me.chkEMailParser.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.chkEMailParser.Size = New System.Drawing.Size(317, 20)
		Me.chkEMailParser.TabIndex = 5
		'
		'chkNotifier
		'
		Me.chkNotifier.Location = New System.Drawing.Point(45, 148)
		Me.chkNotifier.Name = "chkNotifier"
		Me.chkNotifier.Properties.Appearance.Options.UseTextOptions = True
		Me.chkNotifier.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
		Me.chkNotifier.Properties.Caption = "Starte Notifier"
		Me.chkNotifier.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.chkNotifier.Size = New System.Drawing.Size(317, 20)
		Me.chkNotifier.TabIndex = 6
		'
		'chkCVLizer
		'
		Me.chkCVLizer.Location = New System.Drawing.Point(45, 199)
		Me.chkCVLizer.Name = "chkCVLizer"
		Me.chkCVLizer.Properties.Appearance.Options.UseTextOptions = True
		Me.chkCVLizer.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
		Me.chkCVLizer.Properties.Caption = "Starte CV-Parsing mit CVLizer"
		Me.chkCVLizer.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.chkCVLizer.Size = New System.Drawing.Size(317, 20)
		Me.chkCVLizer.TabIndex = 7
		'
		'btnStartScan
		'
		Me.btnStartScan.Location = New System.Drawing.Point(388, 33)
		Me.btnStartScan.Name = "btnStartScan"
		Me.btnStartScan.Size = New System.Drawing.Size(122, 34)
		Me.btnStartScan.TabIndex = 8
		Me.btnStartScan.Text = "Starte Scanvorgang"
		'
		'btnStartEMailParsing
		'
		Me.btnStartEMailParsing.Location = New System.Drawing.Point(388, 85)
		Me.btnStartEMailParsing.Name = "btnStartEMailParsing"
		Me.btnStartEMailParsing.Size = New System.Drawing.Size(122, 34)
		Me.btnStartEMailParsing.TabIndex = 9
		Me.btnStartEMailParsing.Text = "Starte eMail-Parsing"
		'
		'btnStartCVLizer
		'
		Me.btnStartCVLizer.Location = New System.Drawing.Point(388, 191)
		Me.btnStartCVLizer.Name = "btnStartCVLizer"
		Me.btnStartCVLizer.Size = New System.Drawing.Size(122, 34)
		Me.btnStartCVLizer.TabIndex = 11
		Me.btnStartCVLizer.Text = "Starte CVLizer"
		'
		'btnStartNotifier
		'
		Me.btnStartNotifier.Location = New System.Drawing.Point(388, 140)
		Me.btnStartNotifier.Name = "btnStartNotifier"
		Me.btnStartNotifier.Size = New System.Drawing.Size(122, 34)
		Me.btnStartNotifier.TabIndex = 10
		Me.btnStartNotifier.Text = "Starte Notifier"
		'
		'btnReadXMLForCommunity
		'
		Me.btnReadXMLForCommunity.Location = New System.Drawing.Point(572, 34)
		Me.btnReadXMLForCommunity.Name = "btnReadXMLForCommunity"
		Me.btnReadXMLForCommunity.Size = New System.Drawing.Size(166, 34)
		Me.btnReadXMLForCommunity.TabIndex = 12
		Me.btnReadXMLForCommunity.Text = "Import XML For Community"
		'
		'lblSettingfilename
		'
		Me.lblSettingfilename.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.lblSettingfilename.Cursor = System.Windows.Forms.Cursors.Hand
		Me.lblSettingfilename.Location = New System.Drawing.Point(47, 252)
		Me.lblSettingfilename.Name = "lblSettingfilename"
		Me.lblSettingfilename.Size = New System.Drawing.Size(75, 13)
		Me.lblSettingfilename.TabIndex = 352
		Me.lblSettingfilename.Text = "settingFilename"
		'
		'btnCountFiles
		'
		Me.btnCountFiles.Location = New System.Drawing.Point(572, 140)
		Me.btnCountFiles.Name = "btnCountFiles"
		Me.btnCountFiles.Size = New System.Drawing.Size(166, 34)
		Me.btnCountFiles.TabIndex = 353
		Me.btnCountFiles.Text = "Count Files"
		'
		'lblLocalIPAddress
		'
		Me.lblLocalIPAddress.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.lblLocalIPAddress.Cursor = System.Windows.Forms.Cursors.Hand
		Me.lblLocalIPAddress.Location = New System.Drawing.Point(47, 271)
		Me.lblLocalIPAddress.Name = "lblLocalIPAddress"
		Me.lblLocalIPAddress.Size = New System.Drawing.Size(31, 13)
		Me.lblLocalIPAddress.TabIndex = 354
		Me.lblLocalIPAddress.Text = "localIP"
		'
		'frmStartProgram
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(1003, 301)
		Me.Controls.Add(Me.lblLocalIPAddress)
		Me.Controls.Add(Me.btnCountFiles)
		Me.Controls.Add(Me.lblSettingfilename)
		Me.Controls.Add(Me.btnReadXMLForCommunity)
		Me.Controls.Add(Me.btnStartCVLizer)
		Me.Controls.Add(Me.btnStartNotifier)
		Me.Controls.Add(Me.btnStartEMailParsing)
		Me.Controls.Add(Me.btnStartScan)
		Me.Controls.Add(Me.chkCVLizer)
		Me.Controls.Add(Me.chkNotifier)
		Me.Controls.Add(Me.chkEMailParser)
		Me.Controls.Add(Me.chkScan)
		Me.Name = "frmStartProgram"
		Me.Text = "Start von diversen Dienten"
		CType(Me.chkScan.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkEMailParser.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkNotifier.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkCVLizer.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents chkScan As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chkEMailParser As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chkNotifier As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chkCVLizer As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents btnStartScan As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents btnStartEMailParsing As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents btnStartCVLizer As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents btnStartNotifier As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents btnReadXMLForCommunity As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents lblSettingfilename As DevExpress.XtraEditors.LabelControl
	Friend WithEvents btnCountFiles As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents lblLocalIPAddress As DevExpress.XtraEditors.LabelControl
End Class
