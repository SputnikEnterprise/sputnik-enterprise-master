<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmQuellensteuer
	Inherits DevExpress.XtraEditors.XtraForm

	'Form overrides dispose to clean up the component list.
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

	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer

	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.  
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Me.FormAssistant1 = New DevExpress.XtraBars.FormAssistant()
		Me.DefaultLookAndFeel1 = New DevExpress.LookAndFeel.DefaultLookAndFeel(Me.components)
		Me.txtSQL = New DevExpress.XtraEditors.MemoEdit()
		Me.btnReadWrite = New DevExpress.XtraEditors.SimpleButton()
		Me.txt_Filepattern = New DevExpress.XtraEditors.TextEdit()
		Me.txt_Path = New DevExpress.XtraEditors.TextEdit()
		Me.txt_Year = New DevExpress.XtraEditors.TextEdit()
		Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl()
		Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl()
		Me.LabelControl3 = New DevExpress.XtraEditors.LabelControl()
		Me.lblInfo = New DevExpress.XtraEditors.LabelControl()
		Me.chkFinalfile = New DevExpress.XtraEditors.CheckEdit()
		Me.chkTruncateTables = New DevExpress.XtraEditors.CheckEdit()
		CType(Me.txtSQL.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txt_Filepattern.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txt_Path.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txt_Year.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkFinalfile.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkTruncateTables.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'txtSQL
		'
		Me.txtSQL.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txtSQL.Location = New System.Drawing.Point(12, 116)
		Me.txtSQL.Name = "txtSQL"
		Me.txtSQL.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.[True]
		Me.txtSQL.Size = New System.Drawing.Size(629, 480)
		Me.txtSQL.TabIndex = 45
		'
		'btnReadWrite
		'
		Me.btnReadWrite.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnReadWrite.Location = New System.Drawing.Point(517, 28)
		Me.btnReadWrite.Name = "btnReadWrite"
		Me.btnReadWrite.Size = New System.Drawing.Size(122, 36)
		Me.btnReadWrite.TabIndex = 44
		Me.btnReadWrite.Text = "Read && Write Text"
		'
		'txt_Filepattern
		'
		Me.txt_Filepattern.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txt_Filepattern.Location = New System.Drawing.Point(152, 54)
		Me.txt_Filepattern.Name = "txt_Filepattern"
		Me.txt_Filepattern.Size = New System.Drawing.Size(359, 20)
		Me.txt_Filepattern.TabIndex = 46
		'
		'txt_Path
		'
		Me.txt_Path.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txt_Path.Location = New System.Drawing.Point(152, 28)
		Me.txt_Path.Name = "txt_Path"
		Me.txt_Path.Size = New System.Drawing.Size(359, 20)
		Me.txt_Path.TabIndex = 47
		'
		'txt_Year
		'
		Me.txt_Year.Location = New System.Drawing.Point(152, 80)
		Me.txt_Year.Name = "txt_Year"
		Me.txt_Year.Size = New System.Drawing.Size(51, 20)
		Me.txt_Year.TabIndex = 48
		'
		'LabelControl1
		'
		Me.LabelControl1.Location = New System.Drawing.Point(70, 31)
		Me.LabelControl1.Name = "LabelControl1"
		Me.LabelControl1.Size = New System.Drawing.Size(22, 13)
		Me.LabelControl1.TabIndex = 49
		Me.LabelControl1.Text = "Pfad"
		'
		'LabelControl2
		'
		Me.LabelControl2.Location = New System.Drawing.Point(70, 57)
		Me.LabelControl2.Name = "LabelControl2"
		Me.LabelControl2.Size = New System.Drawing.Size(37, 13)
		Me.LabelControl2.TabIndex = 50
		Me.LabelControl2.Text = "Dateien"
		'
		'LabelControl3
		'
		Me.LabelControl3.Location = New System.Drawing.Point(70, 83)
		Me.LabelControl3.Name = "LabelControl3"
		Me.LabelControl3.Size = New System.Drawing.Size(21, 13)
		Me.LabelControl3.TabIndex = 51
		Me.LabelControl3.Text = "Jahr"
		'
		'lblInfo
		'
		Me.lblInfo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.lblInfo.Appearance.Options.UseTextOptions = True
		Me.lblInfo.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblInfo.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
		Me.lblInfo.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblInfo.Location = New System.Drawing.Point(227, 83)
		Me.lblInfo.Name = "lblInfo"
		Me.lblInfo.Size = New System.Drawing.Size(284, 13)
		Me.lblInfo.TabIndex = 52
		Me.lblInfo.Text = "Import wurde abgeschlossen"
		'
		'chkFinalfile
		'
		Me.chkFinalfile.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.chkFinalfile.Location = New System.Drawing.Point(519, 70)
		Me.chkFinalfile.Name = "chkFinalfile"
		Me.chkFinalfile.Properties.AllowFocused = False
		Me.chkFinalfile.Properties.Caption = "Final-File"
		Me.chkFinalfile.Size = New System.Drawing.Size(102, 20)
		Me.chkFinalfile.TabIndex = 53
		'
		'chkTruncateTables
		'
		Me.chkTruncateTables.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.chkTruncateTables.EditValue = True
		Me.chkTruncateTables.Location = New System.Drawing.Point(519, 91)
		Me.chkTruncateTables.Name = "chkTruncateTables"
		Me.chkTruncateTables.Properties.AllowFocused = False
		Me.chkTruncateTables.Properties.Caption = "Truncate Tables"
		Me.chkTruncateTables.Size = New System.Drawing.Size(102, 20)
		Me.chkTruncateTables.TabIndex = 54
		'
		'frmQuellensteuer
		'
		Me.Appearance.Options.UseFont = True
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(653, 608)
		Me.Controls.Add(Me.chkTruncateTables)
		Me.Controls.Add(Me.chkFinalfile)
		Me.Controls.Add(Me.lblInfo)
		Me.Controls.Add(Me.LabelControl3)
		Me.Controls.Add(Me.LabelControl2)
		Me.Controls.Add(Me.LabelControl1)
		Me.Controls.Add(Me.txt_Year)
		Me.Controls.Add(Me.txt_Path)
		Me.Controls.Add(Me.txt_Filepattern)
		Me.Controls.Add(Me.txtSQL)
		Me.Controls.Add(Me.btnReadWrite)
		Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.MinimumSize = New System.Drawing.Size(655, 640)
		Me.Name = "frmQuellensteuer"
		Me.Text = "Import von Quellensteuer Daten"
		CType(Me.txtSQL.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txt_Filepattern.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txt_Path.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txt_Year.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkFinalfile.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkTruncateTables.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents FormAssistant1 As DevExpress.XtraBars.FormAssistant
	Friend WithEvents DefaultLookAndFeel1 As DevExpress.LookAndFeel.DefaultLookAndFeel
	Friend WithEvents txtSQL As DevExpress.XtraEditors.MemoEdit
	Friend WithEvents btnReadWrite As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents txt_Filepattern As DevExpress.XtraEditors.TextEdit
	Friend WithEvents txt_Path As DevExpress.XtraEditors.TextEdit
	Friend WithEvents txt_Year As DevExpress.XtraEditors.TextEdit
	Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
	Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
	Friend WithEvents LabelControl3 As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblInfo As DevExpress.XtraEditors.LabelControl
	Friend WithEvents chkFinalfile As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chkTruncateTables As DevExpress.XtraEditors.CheckEdit
End Class
