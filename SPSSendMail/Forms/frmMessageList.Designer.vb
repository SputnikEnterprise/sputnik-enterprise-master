<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMessageList
  Inherits DevExpress.XtraEditors.XtraForm

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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMessageList))
		Me.Label1 = New System.Windows.Forms.Label()
		Me.cmdOK = New DevExpress.XtraEditors.SimpleButton()
		Me.txtSearch = New DevExpress.XtraEditors.TextEdit()
		Me.cboDbField = New System.Windows.Forms.ComboBox()
		Me.cboOperator = New System.Windows.Forms.ComboBox()
		Me.LblChanged = New System.Windows.Forms.Label()
		Me.LblChanged_1 = New System.Windows.Forms.Label()
		Me.LblChanged_2 = New System.Windows.Forms.Label()
		Me.cboMessageArt = New System.Windows.Forms.ComboBox()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.Label5 = New System.Windows.Forms.Label()
		Me.Label6 = New System.Windows.Forms.Label()
		Me.txtAllMessage = New DevExpress.XtraEditors.MemoEdit()
		Me.lvMail = New DevComponents.DotNetBar.Controls.ListViewEx()
		Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
		Me.LblWaitPic = New System.Windows.Forms.ToolStripStatusLabel()
		Me.LblWait = New System.Windows.Forms.ToolStripStatusLabel()
		Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
		Me.StyleManager1 = New DevComponents.DotNetBar.StyleManager()
		Me.GroupBox1 = New System.Windows.Forms.GroupBox()
		Me.CmdClose = New DevExpress.XtraEditors.SimpleButton()
		Me.LblSetting = New System.Windows.Forms.Label()
		Me.Label7 = New System.Windows.Forms.Label()
		Me.Label8 = New System.Windows.Forms.Label()
		Me.XtraTabControl1 = New DevExpress.XtraTab.XtraTabControl()
		Me.xtabZusammen = New DevExpress.XtraTab.XtraTabPage()
		Me.xtabDetail = New DevExpress.XtraTab.XtraTabPage()
		CType(Me.txtSearch.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtAllMessage.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.StatusStrip1.SuspendLayout()
		Me.GroupBox1.SuspendLayout()
		CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.XtraTabControl1.SuspendLayout()
		Me.xtabZusammen.SuspendLayout()
		Me.xtabDetail.SuspendLayout()
		Me.SuspendLayout()
		'
		'Label1
		'
		Me.Label1.Location = New System.Drawing.Point(14, 58)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(61, 13)
		Me.Label1.TabIndex = 0
		Me.Label1.Text = "Details"
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'cmdOK
		'
		Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.cmdOK.Location = New System.Drawing.Point(890, 117)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.Size = New System.Drawing.Size(82, 23)
		Me.cmdOK.TabIndex = 14
		Me.cmdOK.Text = "Aktualisieren"
		Me.cmdOK.Visible = False
		'
		'txtSearch
		'
		Me.txtSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txtSearch.Location = New System.Drawing.Point(620, 32)
		Me.txtSearch.Name = "txtSearch"
		Me.txtSearch.Size = New System.Drawing.Size(196, 20)
		Me.txtSearch.TabIndex = 15
		'
		'cboDbField
		'
		Me.cboDbField.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.cboDbField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboDbField.FormattingEnabled = True
		Me.cboDbField.Items.AddRange(New Object() {"CreatedOn", "CreatedFrom", "eMail_Subject", "eMail_To", "eMail_From"})
		Me.cboDbField.Location = New System.Drawing.Point(422, 31)
		Me.cboDbField.Name = "cboDbField"
		Me.cboDbField.Size = New System.Drawing.Size(129, 21)
		Me.cboDbField.TabIndex = 16
		'
		'cboOperator
		'
		Me.cboOperator.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.cboOperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboOperator.FormattingEnabled = True
		Me.cboOperator.Items.AddRange(New Object() {"=", ">", ">=", "<", "<=", "<>", "Like"})
		Me.cboOperator.Location = New System.Drawing.Point(557, 31)
		Me.cboOperator.Name = "cboOperator"
		Me.cboOperator.Size = New System.Drawing.Size(57, 21)
		Me.cboOperator.TabIndex = 17
		'
		'LblChanged
		'
		Me.LblChanged.AutoSize = True
		Me.LblChanged.Location = New System.Drawing.Point(869, 180)
		Me.LblChanged.Name = "LblChanged"
		Me.LblChanged.Size = New System.Drawing.Size(13, 13)
		Me.LblChanged.TabIndex = 18
		Me.LblChanged.Text = "0"
		Me.LblChanged.Visible = False
		'
		'LblChanged_1
		'
		Me.LblChanged_1.AutoSize = True
		Me.LblChanged_1.Location = New System.Drawing.Point(869, 199)
		Me.LblChanged_1.Name = "LblChanged_1"
		Me.LblChanged_1.Size = New System.Drawing.Size(13, 13)
		Me.LblChanged_1.TabIndex = 19
		Me.LblChanged_1.Text = "0"
		Me.LblChanged_1.Visible = False
		'
		'LblChanged_2
		'
		Me.LblChanged_2.AutoSize = True
		Me.LblChanged_2.Location = New System.Drawing.Point(869, 212)
		Me.LblChanged_2.Name = "LblChanged_2"
		Me.LblChanged_2.Size = New System.Drawing.Size(13, 13)
		Me.LblChanged_2.TabIndex = 20
		Me.LblChanged_2.Text = "0"
		Me.LblChanged_2.Visible = False
		'
		'cboMessageArt
		'
		Me.cboMessageArt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboMessageArt.FormattingEnabled = True
		Me.cboMessageArt.Items.AddRange(New Object() {"Alle", "EMail", "Telefax"})
		Me.cboMessageArt.Location = New System.Drawing.Point(81, 31)
		Me.cboMessageArt.Name = "cboMessageArt"
		Me.cboMessageArt.Size = New System.Drawing.Size(129, 21)
		Me.cboMessageArt.TabIndex = 21
		'
		'Label2
		'
		Me.Label2.Location = New System.Drawing.Point(14, 35)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(61, 13)
		Me.Label2.TabIndex = 22
		Me.Label2.Text = "Art"
		Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'Label5
		'
		Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.Label5.Location = New System.Drawing.Point(300, 35)
		Me.Label5.Name = "Label5"
		Me.Label5.Size = New System.Drawing.Size(116, 13)
		Me.Label5.TabIndex = 23
		Me.Label5.Text = "Suchen nach"
		Me.Label5.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'Label6
		'
		Me.Label6.AutoSize = True
		Me.Label6.Location = New System.Drawing.Point(24, 11)
		Me.Label6.Name = "Label6"
		Me.Label6.Size = New System.Drawing.Size(157, 13)
		Me.Label6.TabIndex = 1
		Me.Label6.Text = "Zusammenfassung der Versand"
		'
		'txtAllMessage
		'
		Me.txtAllMessage.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txtAllMessage.Location = New System.Drawing.Point(27, 29)
		Me.txtAllMessage.Name = "txtAllMessage"
		Me.txtAllMessage.Size = New System.Drawing.Size(790, 456)
		Me.txtAllMessage.TabIndex = 0
		'
		'lvMail
		'
		Me.lvMail.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.lvMail.BackColor = System.Drawing.Color.White
		'
		'
		'
		Me.lvMail.Border.Class = "ListViewBorder"
		Me.lvMail.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
		Me.lvMail.ForeColor = System.Drawing.Color.Black
		Me.lvMail.FullRowSelect = True
		Me.lvMail.HideSelection = False
		Me.lvMail.Location = New System.Drawing.Point(81, 58)
		Me.lvMail.Name = "lvMail"
		Me.lvMail.Size = New System.Drawing.Size(734, 429)
		Me.lvMail.TabIndex = 24
		Me.lvMail.UseCompatibleStateImageBehavior = False
		Me.lvMail.View = System.Windows.Forms.View.Details
		'
		'StatusStrip1
		'
		Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LblWaitPic, Me.LblWait})
		Me.StatusStrip1.Location = New System.Drawing.Point(0, 668)
		Me.StatusStrip1.Name = "StatusStrip1"
		Me.StatusStrip1.Size = New System.Drawing.Size(984, 22)
		Me.StatusStrip1.TabIndex = 25
		Me.StatusStrip1.Text = "StatusStrip1"
		'
		'LblWaitPic
		'
		Me.LblWaitPic.Image = Global.SPSSendMail.My.Resources.Resources.remembermilk_orange
		Me.LblWaitPic.Name = "LblWaitPic"
		Me.LblWaitPic.Size = New System.Drawing.Size(113, 17)
		Me.LblWaitPic.Text = "Bitte warten Sie..."
		'
		'LblWait
		'
		Me.LblWait.Name = "LblWait"
		Me.LblWait.Size = New System.Drawing.Size(223, 17)
		Me.LblWait.Text = "Bereit, {0} Datens{1} wurde{2} gefunden..."
		'
		'BackgroundWorker1
		'
		'
		'StyleManager1
		'
		Me.StyleManager1.ManagerStyle = DevComponents.DotNetBar.eStyle.Metro
		Me.StyleManager1.MetroColorParameters = New DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(System.Drawing.Color.White, System.Drawing.Color.FromArgb(CType(CType(43, Byte), Integer), CType(CType(87, Byte), Integer), CType(CType(154, Byte), Integer)))
		'
		'GroupBox1
		'
		Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.GroupBox1.BackColor = System.Drawing.Color.White
		Me.GroupBox1.Controls.Add(Me.CmdClose)
		Me.GroupBox1.Controls.Add(Me.LblSetting)
		Me.GroupBox1.Controls.Add(Me.Label7)
		Me.GroupBox1.Controls.Add(Me.Label8)
		Me.GroupBox1.Location = New System.Drawing.Point(0, -7)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(1013, 93)
		Me.GroupBox1.TabIndex = 26
		Me.GroupBox1.TabStop = False
		'
		'CmdClose
		'
		Me.CmdClose.Anchor = System.Windows.Forms.AnchorStyles.Right
		Me.CmdClose.Location = New System.Drawing.Point(890, 29)
		Me.CmdClose.Name = "CmdClose"
		Me.CmdClose.Size = New System.Drawing.Size(80, 25)
		Me.CmdClose.TabIndex = 205
		Me.CmdClose.Text = "Schliessen"
		'
		'LblSetting
		'
		Me.LblSetting.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.LblSetting.ForeColor = System.Drawing.SystemColors.HotTrack
		Me.LblSetting.Image = CType(resources.GetObject("LblSetting.Image"), System.Drawing.Image)
		Me.LblSetting.Location = New System.Drawing.Point(7, 19)
		Me.LblSetting.Name = "LblSetting"
		Me.LblSetting.Size = New System.Drawing.Size(83, 65)
		Me.LblSetting.TabIndex = 4
		'
		'Label7
		'
		Me.Label7.AutoSize = True
		Me.Label7.Location = New System.Drawing.Point(114, 53)
		Me.Label7.Name = "Label7"
		Me.Label7.Size = New System.Drawing.Size(225, 13)
		Me.Label7.TabIndex = 1
		Me.Label7.Text = "Wählen Sie Ihre gewünschten Kriterien aus..."
		'
		'Label8
		'
		Me.Label8.AutoSize = True
		Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label8.Location = New System.Drawing.Point(96, 29)
		Me.Label8.Name = "Label8"
		Me.Label8.Size = New System.Drawing.Size(224, 13)
		Me.Label8.TabIndex = 0
		Me.Label8.Text = "Liste der versandten E-Mails / Telefax"
		'
		'XtraTabControl1
		'
		Me.XtraTabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.XtraTabControl1.Location = New System.Drawing.Point(29, 117)
		Me.XtraTabControl1.Name = "XtraTabControl1"
		Me.XtraTabControl1.SelectedTabPage = Me.xtabZusammen
		Me.XtraTabControl1.Size = New System.Drawing.Size(850, 533)
		Me.XtraTabControl1.TabIndex = 27
		Me.XtraTabControl1.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabZusammen, Me.xtabDetail})
		'
		'xtabZusammen
		'
		Me.xtabZusammen.Controls.Add(Me.Label6)
		Me.xtabZusammen.Controls.Add(Me.txtAllMessage)
		Me.xtabZusammen.Name = "xtabZusammen"
		Me.xtabZusammen.Size = New System.Drawing.Size(844, 505)
		Me.xtabZusammen.Text = "Zusammenfassung"
		'
		'xtabDetail
		'
		Me.xtabDetail.Controls.Add(Me.lvMail)
		Me.xtabDetail.Controls.Add(Me.Label2)
		Me.xtabDetail.Controls.Add(Me.cboDbField)
		Me.xtabDetail.Controls.Add(Me.Label1)
		Me.xtabDetail.Controls.Add(Me.cboOperator)
		Me.xtabDetail.Controls.Add(Me.Label5)
		Me.xtabDetail.Controls.Add(Me.txtSearch)
		Me.xtabDetail.Controls.Add(Me.cboMessageArt)
		Me.xtabDetail.Name = "xtabDetail"
		Me.xtabDetail.PageVisible = False
		Me.xtabDetail.Size = New System.Drawing.Size(844, 505)
		Me.xtabDetail.Text = "Versanddetail"
		'
		'frmMessageList
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(984, 690)
		Me.Controls.Add(Me.XtraTabControl1)
		Me.Controls.Add(Me.GroupBox1)
		Me.Controls.Add(Me.StatusStrip1)
		Me.Controls.Add(Me.LblChanged_2)
		Me.Controls.Add(Me.LblChanged_1)
		Me.Controls.Add(Me.LblChanged)
		Me.Controls.Add(Me.cmdOK)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.Name = "frmMessageList"
		Me.Text = "Liste der Nachrichten"
		CType(Me.txtSearch.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtAllMessage.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.StatusStrip1.ResumeLayout(False)
		Me.StatusStrip1.PerformLayout()
		Me.GroupBox1.ResumeLayout(False)
		Me.GroupBox1.PerformLayout()
		CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.XtraTabControl1.ResumeLayout(False)
		Me.xtabZusammen.ResumeLayout(False)
		Me.xtabZusammen.PerformLayout()
		Me.xtabDetail.ResumeLayout(False)
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents Label1 As System.Windows.Forms.Label
  '  System.Windows.Forms.Button
  Friend WithEvents cmdOK As DevExpress.XtraEditors.SimpleButton '  System.Windows.Forms.Button
  Friend WithEvents txtSearch As DevExpress.XtraEditors.TextEdit  '  System.Windows.Forms.TextBox
  Friend WithEvents cboDbField As System.Windows.Forms.ComboBox
  Friend WithEvents cboOperator As System.Windows.Forms.ComboBox
  Friend WithEvents LblChanged As System.Windows.Forms.Label
  Friend WithEvents LblChanged_1 As System.Windows.Forms.Label
  Friend WithEvents LblChanged_2 As System.Windows.Forms.Label
  Friend WithEvents cboMessageArt As System.Windows.Forms.ComboBox
  Friend WithEvents Label2 As System.Windows.Forms.Label
  Friend WithEvents Label5 As System.Windows.Forms.Label
  Friend WithEvents Label6 As System.Windows.Forms.Label
  Friend WithEvents txtAllMessage As DevExpress.XtraEditors.MemoEdit ' System.Windows.Forms.TextBox
  Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
  Friend WithEvents LblWait As System.Windows.Forms.ToolStripStatusLabel
  Friend WithEvents LblWaitPic As System.Windows.Forms.ToolStripStatusLabel
  Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
  Friend WithEvents lvMail As DevComponents.DotNetBar.Controls.ListViewEx
  Friend WithEvents StyleManager1 As DevComponents.DotNetBar.StyleManager
  Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
  Friend WithEvents CmdClose As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents LblSetting As System.Windows.Forms.Label
  Friend WithEvents Label7 As System.Windows.Forms.Label
  Friend WithEvents Label8 As System.Windows.Forms.Label
  Friend WithEvents XtraTabControl1 As DevExpress.XtraTab.XtraTabControl
  Friend WithEvents xtabZusammen As DevExpress.XtraTab.XtraTabPage
  Friend WithEvents xtabDetail As DevExpress.XtraTab.XtraTabPage
End Class
