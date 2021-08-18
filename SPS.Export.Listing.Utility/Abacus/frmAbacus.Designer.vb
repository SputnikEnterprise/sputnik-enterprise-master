<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAbacus
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAbacus))
		Me.GroupBox1 = New DevExpress.XtraEditors.PanelControl()
		Me.lblHeader2 = New DevExpress.XtraEditors.LabelControl()
		Me.LblSetting = New System.Windows.Forms.Label()
		Me.lblHeader1 = New DevExpress.XtraEditors.LabelControl()
		Me.CmdClose = New DevExpress.XtraEditors.SimpleButton()
		Me.txtFilename = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.lblDatei = New DevExpress.XtraEditors.LabelControl()
		Me.chkRef = New DevExpress.XtraEditors.CheckEdit()
		Me.cbo_Trennzeichen = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.cbo_Darstellungszeichen = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.lblFeldertrennen = New DevExpress.XtraEditors.LabelControl()
		Me.lblFelderdarstellenin = New DevExpress.XtraEditors.LabelControl()
		Me.cmdOK = New DevExpress.XtraEditors.SimpleButton()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager()
		Me.Bar4 = New DevExpress.XtraBars.Bar()
		Me.bsiInfo = New DevExpress.XtraBars.BarStaticItem()
		Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
		Me.BarStaticItem1 = New DevExpress.XtraBars.BarStaticItem()
		Me.chkGegenkostenart = New DevExpress.XtraEditors.CheckEdit()
		Me.lblMwstCode = New DevExpress.XtraEditors.LabelControl()
		Me.txtMwStCode = New DevExpress.XtraEditors.TextEdit()
		CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupBox1.SuspendLayout()
		CType(Me.txtFilename.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkRef.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.cbo_Trennzeichen.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.cbo_Darstellungszeichen.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkGegenkostenart.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtMwStCode.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'GroupBox1
		'
		Me.GroupBox1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003
		Me.GroupBox1.Controls.Add(Me.lblHeader2)
		Me.GroupBox1.Controls.Add(Me.LblSetting)
		Me.GroupBox1.Controls.Add(Me.lblHeader1)
		Me.GroupBox1.Controls.Add(Me.CmdClose)
		Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
		Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(672, 59)
		Me.GroupBox1.TabIndex = 212
		'
		'lblHeader2
		'
		Me.lblHeader2.AllowHtmlString = True
		Me.lblHeader2.Location = New System.Drawing.Point(78, 31)
		Me.lblHeader2.Name = "lblHeader2"
		Me.lblHeader2.Size = New System.Drawing.Size(218, 13)
		Me.lblHeader2.TabIndex = 214
		Me.lblHeader2.Text = "Wählen Sie Ihre gewünschten Kriterien aus..."
		'
		'LblSetting
		'
		Me.LblSetting.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.LblSetting.ForeColor = System.Drawing.SystemColors.HotTrack
		Me.LblSetting.Image = CType(resources.GetObject("LblSetting.Image"), System.Drawing.Image)
		Me.LblSetting.Location = New System.Drawing.Point(12, 12)
		Me.LblSetting.Name = "LblSetting"
		Me.LblSetting.Size = New System.Drawing.Size(51, 38)
		Me.LblSetting.TabIndex = 213
		'
		'lblHeader1
		'
		Me.lblHeader1.AllowHtmlString = True
		Me.lblHeader1.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.lblHeader1.Appearance.Options.UseFont = True
		Me.lblHeader1.Location = New System.Drawing.Point(69, 12)
		Me.lblHeader1.Name = "lblHeader1"
		Me.lblHeader1.Size = New System.Drawing.Size(209, 13)
		Me.lblHeader1.TabIndex = 213
		Me.lblHeader1.Text = "Export der Daten für ABACUS-Format"
		'
		'CmdClose
		'
		Me.CmdClose.Anchor = System.Windows.Forms.AnchorStyles.Right
		Me.CmdClose.Location = New System.Drawing.Point(546, 12)
		Me.CmdClose.Name = "CmdClose"
		Me.CmdClose.Size = New System.Drawing.Size(94, 25)
		Me.CmdClose.TabIndex = 204
		Me.CmdClose.Text = "Schliessen"
		'
		'txtFilename
		'
		Me.txtFilename.Location = New System.Drawing.Point(179, 95)
		Me.txtFilename.Name = "txtFilename"
		Me.txtFilename.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.txtFilename.Size = New System.Drawing.Size(347, 20)
		Me.txtFilename.TabIndex = 213
		'
		'lblDatei
		'
		Me.lblDatei.AllowHtmlString = True
		Me.lblDatei.Appearance.Options.UseTextOptions = True
		Me.lblDatei.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblDatei.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblDatei.Location = New System.Drawing.Point(15, 99)
		Me.lblDatei.Name = "lblDatei"
		Me.lblDatei.Size = New System.Drawing.Size(158, 13)
		Me.lblDatei.TabIndex = 215
		Me.lblDatei.Text = "Dateiname"
		'
		'chkRef
		'
		Me.chkRef.Location = New System.Drawing.Point(177, 199)
		Me.chkRef.Name = "chkRef"
		Me.chkRef.Properties.Caption = "Ref-Nr. anstelle FKSoll auswählen"
		Me.chkRef.Size = New System.Drawing.Size(349, 19)
		Me.chkRef.TabIndex = 216
		'
		'cbo_Trennzeichen
		'
		Me.cbo_Trennzeichen.Location = New System.Drawing.Point(179, 121)
		Me.cbo_Trennzeichen.Name = "cbo_Trennzeichen"
		Me.cbo_Trennzeichen.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.cbo_Trennzeichen.Size = New System.Drawing.Size(99, 20)
		Me.cbo_Trennzeichen.TabIndex = 217
		'
		'cbo_Darstellungszeichen
		'
		Me.cbo_Darstellungszeichen.Location = New System.Drawing.Point(179, 147)
		Me.cbo_Darstellungszeichen.Name = "cbo_Darstellungszeichen"
		Me.cbo_Darstellungszeichen.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.cbo_Darstellungszeichen.Size = New System.Drawing.Size(99, 20)
		Me.cbo_Darstellungszeichen.TabIndex = 218
		'
		'lblFeldertrennen
		'
		Me.lblFeldertrennen.AllowHtmlString = True
		Me.lblFeldertrennen.Appearance.Options.UseTextOptions = True
		Me.lblFeldertrennen.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblFeldertrennen.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblFeldertrennen.Location = New System.Drawing.Point(15, 125)
		Me.lblFeldertrennen.Name = "lblFeldertrennen"
		Me.lblFeldertrennen.Size = New System.Drawing.Size(158, 13)
		Me.lblFeldertrennen.TabIndex = 219
		Me.lblFeldertrennen.Text = "Felder trennen durch"
		'
		'lblFelderdarstellenin
		'
		Me.lblFelderdarstellenin.AllowHtmlString = True
		Me.lblFelderdarstellenin.Appearance.Options.UseTextOptions = True
		Me.lblFelderdarstellenin.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblFelderdarstellenin.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblFelderdarstellenin.Location = New System.Drawing.Point(15, 151)
		Me.lblFelderdarstellenin.Name = "lblFelderdarstellenin"
		Me.lblFelderdarstellenin.Size = New System.Drawing.Size(158, 13)
		Me.lblFelderdarstellenin.TabIndex = 220
		Me.lblFelderdarstellenin.Text = "Felder darstellen in"
		'
		'cmdOK
		'
		Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.cmdOK.Location = New System.Drawing.Point(546, 95)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.Size = New System.Drawing.Size(94, 25)
		Me.cmdOK.TabIndex = 221
		Me.cmdOK.Text = "Ausführen"
		'
		'BarManager1
		'
		Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar4})
		Me.BarManager1.DockControls.Add(Me.barDockControlTop)
		Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
		Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
		Me.BarManager1.DockControls.Add(Me.barDockControlRight)
		Me.BarManager1.Form = Me
		Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiInfo, Me.BarStaticItem1})
		Me.BarManager1.MaxItemId = 10
		Me.BarManager1.StatusBar = Me.Bar4
		'
		'Bar4
		'
		Me.Bar4.BarName = "Statusleiste"
		Me.Bar4.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
		Me.Bar4.DockCol = 0
		Me.Bar4.DockRow = 0
		Me.Bar4.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
		Me.Bar4.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiInfo)})
		Me.Bar4.OptionsBar.AllowQuickCustomization = False
		Me.Bar4.OptionsBar.DrawDragBorder = False
		Me.Bar4.OptionsBar.UseWholeRow = True
		Me.Bar4.Text = "Statusleiste"
		'
		'bsiInfo
		'
		Me.bsiInfo.AutoSize = DevExpress.XtraBars.BarStaticItemSize.Spring
		Me.bsiInfo.Caption = "Bereit"
		Me.bsiInfo.Id = 0
		Me.bsiInfo.Name = "bsiInfo"
		Me.bsiInfo.Size = New System.Drawing.Size(32, 0)
		Me.bsiInfo.Width = 32
		'
		'barDockControlTop
		'
		Me.barDockControlTop.CausesValidation = False
		Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
		Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlTop.Manager = Me.BarManager1
		Me.barDockControlTop.Size = New System.Drawing.Size(672, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 265)
		Me.barDockControlBottom.Manager = Me.BarManager1
		Me.barDockControlBottom.Size = New System.Drawing.Size(672, 25)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Manager = Me.BarManager1
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 265)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(672, 0)
		Me.barDockControlRight.Manager = Me.BarManager1
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 265)
		'
		'BarStaticItem1
		'
		Me.BarStaticItem1.Caption = " "
		Me.BarStaticItem1.Id = 5
		Me.BarStaticItem1.Name = "BarStaticItem1"
		'
		'chkGegenkostenart
		'
		Me.chkGegenkostenart.Location = New System.Drawing.Point(177, 224)
		Me.chkGegenkostenart.Name = "chkGegenkostenart"
		Me.chkGegenkostenart.Properties.Caption = """Gegenkostenart"" verwenden"
		Me.chkGegenkostenart.Size = New System.Drawing.Size(349, 19)
		Me.chkGegenkostenart.TabIndex = 226
		'
		'lblMwstCode
		'
		Me.lblMwstCode.AllowHtmlString = True
		Me.lblMwstCode.Appearance.Options.UseTextOptions = True
		Me.lblMwstCode.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblMwstCode.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblMwstCode.Location = New System.Drawing.Point(15, 177)
		Me.lblMwstCode.Name = "lblMwstCode"
		Me.lblMwstCode.Size = New System.Drawing.Size(158, 13)
		Me.lblMwstCode.TabIndex = 232
		Me.lblMwstCode.Text = "MwSt.-Code"
		'
		'txtMwStCode
		'
		Me.txtMwStCode.Location = New System.Drawing.Point(179, 173)
		Me.txtMwStCode.Name = "txtMwStCode"
		Me.txtMwStCode.Properties.MaxLength = 3
		Me.txtMwStCode.Size = New System.Drawing.Size(99, 20)
		Me.txtMwStCode.TabIndex = 231
		'
		'frmAbacus
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(672, 290)
		Me.Controls.Add(Me.lblMwstCode)
		Me.Controls.Add(Me.chkGegenkostenart)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.lblFelderdarstellenin)
		Me.Controls.Add(Me.lblFeldertrennen)
		Me.Controls.Add(Me.cbo_Darstellungszeichen)
		Me.Controls.Add(Me.cbo_Trennzeichen)
		Me.Controls.Add(Me.chkRef)
		Me.Controls.Add(Me.lblDatei)
		Me.Controls.Add(Me.GroupBox1)
		Me.Controls.Add(Me.txtFilename)
		Me.Controls.Add(Me.txtMwStCode)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MinimumSize = New System.Drawing.Size(682, 322)
		Me.Name = "frmAbacus"
		Me.Text = "Datenexport für ABACUS"
		CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupBox1.ResumeLayout(False)
		Me.GroupBox1.PerformLayout()
		CType(Me.txtFilename.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkRef.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.cbo_Trennzeichen.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.cbo_Darstellungszeichen.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkGegenkostenart.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtMwStCode.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents GroupBox1 As DevExpress.XtraEditors.PanelControl
  Friend WithEvents lblHeader1 As DevExpress.XtraEditors.LabelControl
  Friend WithEvents CmdClose As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents LblSetting As System.Windows.Forms.Label
  Friend WithEvents lblHeader2 As DevExpress.XtraEditors.LabelControl
  Friend WithEvents txtFilename As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents lblDatei As DevExpress.XtraEditors.LabelControl
  Friend WithEvents chkRef As DevExpress.XtraEditors.CheckEdit
  Friend WithEvents cbo_Trennzeichen As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents cbo_Darstellungszeichen As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents lblFeldertrennen As DevExpress.XtraEditors.LabelControl
  Friend WithEvents lblFelderdarstellenin As DevExpress.XtraEditors.LabelControl
  Friend WithEvents cmdOK As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
  Friend WithEvents Bar4 As DevExpress.XtraBars.Bar
  Friend WithEvents bsiInfo As DevExpress.XtraBars.BarStaticItem
  Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
  Friend WithEvents BarStaticItem1 As DevExpress.XtraBars.BarStaticItem
  Friend WithEvents chkGegenkostenart As DevExpress.XtraEditors.CheckEdit
  Friend WithEvents lblMwstCode As DevExpress.XtraEditors.LabelControl
  Friend WithEvents txtMwStCode As DevExpress.XtraEditors.TextEdit
End Class
