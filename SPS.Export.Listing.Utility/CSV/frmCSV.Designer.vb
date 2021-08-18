<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCSV
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCSV))
		Me.lblDatei = New System.Windows.Forms.Label()
		Me.pccSetting = New DevExpress.XtraBars.PopupControlContainer()
		Me.grpSetting = New DevExpress.XtraEditors.GroupControl()
		Me.lblAusgewaehltefelder = New System.Windows.Forms.Label()
		Me.cbo_Trennzeichen = New System.Windows.Forms.ComboBox()
		Me.lblfeldertrennen = New System.Windows.Forms.Label()
		Me.lblFelder = New System.Windows.Forms.Label()
		Me.cbo_Darstellungszeichen = New System.Windows.Forms.ComboBox()
		Me.lstSelektierteFelder = New DevExpress.XtraEditors.ListBoxControl()
		Me.lstVorhandeneFelder = New DevExpress.XtraEditors.ListBoxControl()
		Me.lblfelderdarstellenin = New System.Windows.Forms.Label()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager()
		Me.Bar3 = New DevExpress.XtraBars.Bar()
		Me.bsiInfo = New DevExpress.XtraBars.BarStaticItem()
		Me.bbiExport = New DevExpress.XtraBars.BarLargeButtonItem()
		Me.bbiSetting = New DevExpress.XtraBars.BarLargeButtonItem()
		Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
		Me.GroupBox1 = New DevExpress.XtraEditors.PanelControl()
		Me.lblHeader2 = New DevExpress.XtraEditors.LabelControl()
		Me.LblSetting = New System.Windows.Forms.Label()
		Me.lblHeader1 = New DevExpress.XtraEditors.LabelControl()
		Me.CmdClose = New DevExpress.XtraEditors.SimpleButton()
		Me.txt_Filename = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.ChkIgnorEmptyRecords = New DevExpress.XtraEditors.CheckEdit()
		Me.chkRecordsAsHeader = New DevExpress.XtraEditors.CheckEdit()
		CType(Me.pccSetting, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.pccSetting.SuspendLayout()
		CType(Me.grpSetting, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grpSetting.SuspendLayout()
		CType(Me.lstSelektierteFelder, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lstVorhandeneFelder, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupBox1.SuspendLayout()
		CType(Me.txt_Filename.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.ChkIgnorEmptyRecords.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkRecordsAsHeader.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'lblDatei
		'
		Me.lblDatei.Location = New System.Drawing.Point(15, 109)
		Me.lblDatei.Name = "lblDatei"
		Me.lblDatei.Size = New System.Drawing.Size(123, 13)
		Me.lblDatei.TabIndex = 143
		Me.lblDatei.Text = "Dateiname"
		Me.lblDatei.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'pccSetting
		'
		Me.pccSetting.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
		Me.pccSetting.Controls.Add(Me.grpSetting)
		Me.pccSetting.Location = New System.Drawing.Point(563, 98)
		Me.pccSetting.Manager = Me.BarManager1
		Me.pccSetting.Name = "pccSetting"
		Me.pccSetting.ShowCloseButton = True
		Me.pccSetting.Size = New System.Drawing.Size(351, 280)
		Me.pccSetting.TabIndex = 148
		Me.pccSetting.Visible = False
		'
		'grpSetting
		'
		Me.grpSetting.Controls.Add(Me.lblAusgewaehltefelder)
		Me.grpSetting.Controls.Add(Me.cbo_Trennzeichen)
		Me.grpSetting.Controls.Add(Me.lblfeldertrennen)
		Me.grpSetting.Controls.Add(Me.lblFelder)
		Me.grpSetting.Controls.Add(Me.cbo_Darstellungszeichen)
		Me.grpSetting.Controls.Add(Me.lstSelektierteFelder)
		Me.grpSetting.Controls.Add(Me.lstVorhandeneFelder)
		Me.grpSetting.Controls.Add(Me.lblfelderdarstellenin)
		Me.grpSetting.Dock = System.Windows.Forms.DockStyle.Top
		Me.grpSetting.Location = New System.Drawing.Point(0, 0)
		Me.grpSetting.Name = "grpSetting"
		Me.grpSetting.Size = New System.Drawing.Size(351, 264)
		Me.grpSetting.TabIndex = 0
		Me.grpSetting.Text = "Auswahl der Felder für Export"
		'
		'lblAusgewaehltefelder
		'
		Me.lblAusgewaehltefelder.AutoSize = True
		Me.lblAusgewaehltefelder.Location = New System.Drawing.Point(182, 30)
		Me.lblAusgewaehltefelder.Name = "lblAusgewaehltefelder"
		Me.lblAusgewaehltefelder.Size = New System.Drawing.Size(102, 13)
		Me.lblAusgewaehltefelder.TabIndex = 147
		Me.lblAusgewaehltefelder.Text = "Ausgewählte Felder"
		'
		'cbo_Trennzeichen
		'
		Me.cbo_Trennzeichen.FormattingEnabled = True
		Me.cbo_Trennzeichen.Items.AddRange(New Object() {",", ";", "#", "{Tab}"})
		Me.cbo_Trennzeichen.Location = New System.Drawing.Point(272, 209)
		Me.cbo_Trennzeichen.Name = "cbo_Trennzeichen"
		Me.cbo_Trennzeichen.Size = New System.Drawing.Size(64, 21)
		Me.cbo_Trennzeichen.TabIndex = 2
		Me.cbo_Trennzeichen.Text = ";"
		'
		'lblfeldertrennen
		'
		Me.lblfeldertrennen.AutoSize = True
		Me.lblfeldertrennen.Location = New System.Drawing.Point(10, 213)
		Me.lblfeldertrennen.Name = "lblfeldertrennen"
		Me.lblfeldertrennen.Size = New System.Drawing.Size(108, 13)
		Me.lblfeldertrennen.TabIndex = 144
		Me.lblfeldertrennen.Text = "Felder trennen durch"
		'
		'lblFelder
		'
		Me.lblFelder.AutoSize = True
		Me.lblFelder.Location = New System.Drawing.Point(15, 30)
		Me.lblFelder.Name = "lblFelder"
		Me.lblFelder.Size = New System.Drawing.Size(98, 13)
		Me.lblFelder.TabIndex = 146
		Me.lblFelder.Text = "Felder zur Auswahl"
		'
		'cbo_Darstellungszeichen
		'
		Me.cbo_Darstellungszeichen.FormattingEnabled = True
		Me.cbo_Darstellungszeichen.Items.AddRange(New Object() {"'", """"})
		Me.cbo_Darstellungszeichen.Location = New System.Drawing.Point(272, 236)
		Me.cbo_Darstellungszeichen.Name = "cbo_Darstellungszeichen"
		Me.cbo_Darstellungszeichen.Size = New System.Drawing.Size(64, 21)
		Me.cbo_Darstellungszeichen.TabIndex = 3
		'
		'lstSelektierteFelder
		'
		Me.lstSelektierteFelder.Location = New System.Drawing.Point(182, 48)
		Me.lstSelektierteFelder.Name = "lstSelektierteFelder"
		Me.lstSelektierteFelder.Size = New System.Drawing.Size(154, 155)
		Me.lstSelektierteFelder.TabIndex = 1
		'
		'lstVorhandeneFelder
		'
		Me.lstVorhandeneFelder.Location = New System.Drawing.Point(15, 48)
		Me.lstVorhandeneFelder.Name = "lstVorhandeneFelder"
		Me.lstVorhandeneFelder.Size = New System.Drawing.Size(154, 155)
		Me.lstVorhandeneFelder.TabIndex = 0
		'
		'lblfelderdarstellenin
		'
		Me.lblfelderdarstellenin.AutoSize = True
		Me.lblfelderdarstellenin.Location = New System.Drawing.Point(10, 239)
		Me.lblfelderdarstellenin.Name = "lblfelderdarstellenin"
		Me.lblfelderdarstellenin.Size = New System.Drawing.Size(101, 13)
		Me.lblfelderdarstellenin.TabIndex = 145
		Me.lblfelderdarstellenin.Text = "Felder darstellen in "
		'
		'BarManager1
		'
		Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar3})
		Me.BarManager1.DockControls.Add(Me.barDockControlTop)
		Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
		Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
		Me.BarManager1.DockControls.Add(Me.barDockControlRight)
		Me.BarManager1.Form = Me
		Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiInfo, Me.bbiSetting, Me.bbiExport})
		Me.BarManager1.MaxItemId = 11
		Me.BarManager1.StatusBar = Me.Bar3
		'
		'Bar3
		'
		Me.Bar3.BarName = "Statusleiste"
		Me.Bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
		Me.Bar3.DockCol = 0
		Me.Bar3.DockRow = 0
		Me.Bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
		Me.Bar3.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiInfo), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiExport), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiSetting)})
		Me.Bar3.OptionsBar.AllowQuickCustomization = False
		Me.Bar3.OptionsBar.DrawDragBorder = False
		Me.Bar3.OptionsBar.UseWholeRow = True
		Me.Bar3.Text = "Statusleiste"
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
		'bbiExport
		'
		Me.bbiExport.Caption = "Exportieren"
		Me.bbiExport.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Right
		Me.bbiExport.Id = 10
		Me.bbiExport.ImageOptions.Image = CType(resources.GetObject("bbiExport.ImageOptions.Image"), System.Drawing.Image)
		Me.bbiExport.Name = "bbiExport"
		Me.bbiExport.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiSetting
		'
		Me.bbiSetting.Caption = "Einstellungen"
		Me.bbiSetting.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Right
		Me.bbiSetting.Id = 7
		Me.bbiSetting.ImageOptions.Image = CType(resources.GetObject("bbiSetting.ImageOptions.Image"), System.Drawing.Image)
		Me.bbiSetting.Name = "bbiSetting"
		Me.bbiSetting.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'barDockControlTop
		'
		Me.barDockControlTop.CausesValidation = False
		Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
		Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlTop.Manager = Me.BarManager1
		Me.barDockControlTop.Size = New System.Drawing.Size(636, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 215)
		Me.barDockControlBottom.Manager = Me.BarManager1
		Me.barDockControlBottom.Size = New System.Drawing.Size(636, 27)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Manager = Me.BarManager1
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 215)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(636, 0)
		Me.barDockControlRight.Manager = Me.BarManager1
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 215)
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
		Me.GroupBox1.Size = New System.Drawing.Size(636, 59)
		Me.GroupBox1.TabIndex = 231
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
		Me.lblHeader1.Size = New System.Drawing.Size(174, 13)
		Me.lblHeader1.TabIndex = 213
		Me.lblHeader1.Text = "Export der Daten in einer Datei"
		'
		'CmdClose
		'
		Me.CmdClose.Anchor = System.Windows.Forms.AnchorStyles.Right
		Me.CmdClose.Location = New System.Drawing.Point(509, 12)
		Me.CmdClose.Name = "CmdClose"
		Me.CmdClose.Size = New System.Drawing.Size(94, 25)
		Me.CmdClose.TabIndex = 204
		Me.CmdClose.Text = "Schliessen"
		'
		'txt_Filename
		'
		Me.txt_Filename.Location = New System.Drawing.Point(144, 106)
		Me.txt_Filename.Name = "txt_Filename"
		Me.txt_Filename.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.txt_Filename.Size = New System.Drawing.Size(392, 20)
		Me.txt_Filename.TabIndex = 0
		'
		'ChkIgnorEmptyRecords
		'
		Me.ChkIgnorEmptyRecords.Location = New System.Drawing.Point(144, 132)
		Me.ChkIgnorEmptyRecords.Name = "ChkIgnorEmptyRecords"
		Me.ChkIgnorEmptyRecords.Properties.AllowFocused = False
		Me.ChkIgnorEmptyRecords.Properties.Caption = "Leere Datensätze nicht erstellen"
		Me.ChkIgnorEmptyRecords.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.[Default]
		Me.ChkIgnorEmptyRecords.RightToLeft = System.Windows.Forms.RightToLeft.Yes
		Me.ChkIgnorEmptyRecords.Size = New System.Drawing.Size(392, 19)
		Me.ChkIgnorEmptyRecords.TabIndex = 236
		'
		'chkRecordsAsHeader
		'
		Me.chkRecordsAsHeader.Location = New System.Drawing.Point(144, 157)
		Me.chkRecordsAsHeader.Name = "chkRecordsAsHeader"
		Me.chkRecordsAsHeader.Properties.AllowFocused = False
		Me.chkRecordsAsHeader.Properties.Caption = "Lohnkonti (Datenzeile als Felder)"
		Me.chkRecordsAsHeader.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.[Default]
		Me.chkRecordsAsHeader.RightToLeft = System.Windows.Forms.RightToLeft.Yes
		Me.chkRecordsAsHeader.Size = New System.Drawing.Size(392, 19)
		Me.chkRecordsAsHeader.TabIndex = 241
		'
		'frmCSV
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(636, 242)
		Me.Controls.Add(Me.chkRecordsAsHeader)
		Me.Controls.Add(Me.ChkIgnorEmptyRecords)
		Me.Controls.Add(Me.GroupBox1)
		Me.Controls.Add(Me.pccSetting)
		Me.Controls.Add(Me.lblDatei)
		Me.Controls.Add(Me.txt_Filename)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MinimumSize = New System.Drawing.Size(652, 280)
		Me.Name = "frmCSV"
		Me.Text = "Daten in einer Datei exportieren"
		CType(Me.pccSetting, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pccSetting.ResumeLayout(False)
		CType(Me.grpSetting, System.ComponentModel.ISupportInitialize).EndInit()
		Me.grpSetting.ResumeLayout(False)
		Me.grpSetting.PerformLayout()
		CType(Me.lstSelektierteFelder, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lstVorhandeneFelder, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupBox1.ResumeLayout(False)
		Me.GroupBox1.PerformLayout()
		CType(Me.txt_Filename.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.ChkIgnorEmptyRecords.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkRecordsAsHeader.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents lblDatei As System.Windows.Forms.Label
  Friend WithEvents grpSetting As DevExpress.XtraEditors.GroupControl
  Friend WithEvents lblAusgewaehltefelder As System.Windows.Forms.Label
  Friend WithEvents cbo_Trennzeichen As System.Windows.Forms.ComboBox
  Friend WithEvents lblfeldertrennen As System.Windows.Forms.Label
  Friend WithEvents lblFelder As System.Windows.Forms.Label
  Friend WithEvents cbo_Darstellungszeichen As System.Windows.Forms.ComboBox
  Friend WithEvents lstSelektierteFelder As DevExpress.XtraEditors.ListBoxControl
  Friend WithEvents lstVorhandeneFelder As DevExpress.XtraEditors.ListBoxControl
  Friend WithEvents lblfelderdarstellenin As System.Windows.Forms.Label
  Friend WithEvents pccSetting As DevExpress.XtraBars.PopupControlContainer
  Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
  Friend WithEvents Bar3 As DevExpress.XtraBars.Bar
  Friend WithEvents bsiInfo As DevExpress.XtraBars.BarStaticItem
  Friend WithEvents bbiExport As DevExpress.XtraBars.BarLargeButtonItem
  Friend WithEvents bbiSetting As DevExpress.XtraBars.BarLargeButtonItem
  Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
  Friend WithEvents GroupBox1 As DevExpress.XtraEditors.PanelControl
  Friend WithEvents lblHeader2 As DevExpress.XtraEditors.LabelControl
  Friend WithEvents LblSetting As System.Windows.Forms.Label
  Friend WithEvents lblHeader1 As DevExpress.XtraEditors.LabelControl
  Friend WithEvents CmdClose As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents txt_Filename As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents ChkIgnorEmptyRecords As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chkRecordsAsHeader As DevExpress.XtraEditors.CheckEdit
End Class
