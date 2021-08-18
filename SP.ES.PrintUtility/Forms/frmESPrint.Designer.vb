<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmESPrint
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
    Me.components = New System.ComponentModel.Container()
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmESPrint))
    Me.StyleManager1 = New DevComponents.DotNetBar.StyleManager(Me.components)
    Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
    Me.Bar4 = New DevExpress.XtraBars.Bar()
    Me.bsiInfo = New DevExpress.XtraBars.BarStaticItem()
    Me.bbiSearch = New DevExpress.XtraBars.BarButtonItem()
    Me.bbiPrint = New DevExpress.XtraBars.BarLargeButtonItem()
    Me.bbiDelete = New DevExpress.XtraBars.BarButtonItem()
    Me.bbiExport = New DevExpress.XtraBars.BarButtonItem()
    Me.bbiSetting = New DevExpress.XtraBars.BarButtonItem()
    Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
    Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
    Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
    Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
    Me.BarStaticItem1 = New DevExpress.XtraBars.BarStaticItem()
    Me.lvDetails = New DevComponents.DotNetBar.Controls.ListViewEx()
    Me.grpSuchkriterien = New DevExpress.XtraEditors.GroupControl()
    Me.chkVerleihvertrag = New DevExpress.XtraEditors.CheckEdit()
    Me.chkESVertrag = New DevExpress.XtraEditors.CheckEdit()
    Me.lblKanton = New System.Windows.Forms.Label()
    Me.lblWOS = New System.Windows.Forms.Label()
    Me.btnWOSProperty = New DevExpress.XtraEditors.DropDownButton()
    Me.cbo_Month = New DevExpress.XtraEditors.ComboBoxEdit()
    Me.cbo_Year = New DevExpress.XtraEditors.ComboBoxEdit()
    Me.lblJahr = New System.Windows.Forms.Label()
    Me.lblMonat = New System.Windows.Forms.Label()
    Me.lblESNr = New System.Windows.Forms.Label()
    Me.cbo_ESNr = New DevExpress.XtraEditors.ComboBoxEdit()
    Me.lblPVLBeruf = New System.Windows.Forms.Label()
    Me.cbo_PVLKanton = New DevExpress.XtraEditors.ComboBoxEdit()
    Me.cbo_PVLBez = New DevExpress.XtraEditors.ComboBoxEdit()
    Me.GroupBox1 = New DevExpress.XtraEditors.PanelControl()
    Me.sbClose = New DevExpress.XtraEditors.SimpleButton()
    Me.Label2 = New System.Windows.Forms.Label()
    Me.LblTimeValue = New System.Windows.Forms.Label()
    Me.lblHeader2 = New System.Windows.Forms.Label()
    Me.lblHeader1 = New System.Windows.Forms.Label()
    Me.lblDetails = New System.Windows.Forms.Label()
    CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.grpSuchkriterien, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.grpSuchkriterien.SuspendLayout()
    CType(Me.chkVerleihvertrag.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.chkESVertrag.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.cbo_Month.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.cbo_Year.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.cbo_ESNr.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.cbo_PVLKanton.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.cbo_PVLBez.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.GroupBox1.SuspendLayout()
    Me.SuspendLayout()
    '
    'StyleManager1
    '
    Me.StyleManager1.ManagerStyle = DevComponents.DotNetBar.eStyle.Metro
    Me.StyleManager1.MetroColorParameters = New DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(System.Drawing.Color.White, System.Drawing.Color.FromArgb(CType(CType(43, Byte), Integer), CType(CType(87, Byte), Integer), CType(CType(154, Byte), Integer)))
    '
    'BarManager1
    '
    Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar4})
    Me.BarManager1.DockControls.Add(Me.barDockControlTop)
    Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
    Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
    Me.BarManager1.DockControls.Add(Me.barDockControlRight)
    Me.BarManager1.Form = Me
    Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiInfo, Me.bbiSearch, Me.bbiPrint, Me.bbiDelete, Me.BarStaticItem1, Me.bbiExport, Me.bbiSetting})
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
    Me.Bar4.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiInfo), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiSearch), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiPrint, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiDelete, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiExport, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiSetting, True)})
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
    Me.bsiInfo.TextAlignment = System.Drawing.StringAlignment.Near
    Me.bsiInfo.Width = 32
    '
    'bbiSearch
    '
    Me.bbiSearch.Caption = "Suchen"
    Me.bbiSearch.Glyph = CType(resources.GetObject("bbiSearch.Glyph"), System.Drawing.Image)
    Me.bbiSearch.Id = 1
    Me.bbiSearch.LargeGlyph = CType(resources.GetObject("bbiSearch.LargeGlyph"), System.Drawing.Image)
    Me.bbiSearch.Name = "bbiSearch"
    Me.bbiSearch.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
    '
    'bbiPrint
    '
    Me.bbiPrint.Caption = "Drucken"
    Me.bbiPrint.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Right
    Me.bbiPrint.Enabled = False
    Me.bbiPrint.Glyph = CType(resources.GetObject("bbiPrint.Glyph"), System.Drawing.Image)
    Me.bbiPrint.Id = 6
    Me.bbiPrint.Name = "bbiPrint"
    Me.bbiPrint.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
    '
    'bbiDelete
    '
    Me.bbiDelete.Caption = "Löschen"
    Me.bbiDelete.Enabled = False
    Me.bbiDelete.Glyph = CType(resources.GetObject("bbiDelete.Glyph"), System.Drawing.Image)
    Me.bbiDelete.Id = 8
    Me.bbiDelete.Name = "bbiDelete"
    Me.bbiDelete.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
    '
    'bbiExport
    '
    Me.bbiExport.Caption = "Exportieren"
    Me.bbiExport.Enabled = False
    Me.bbiExport.Glyph = CType(resources.GetObject("bbiExport.Glyph"), System.Drawing.Image)
    Me.bbiExport.Id = 4
    Me.bbiExport.Name = "bbiExport"
    Me.bbiExport.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
    '
    'bbiSetting
    '
    Me.bbiSetting.Caption = "Einstellungen"
    Me.bbiSetting.Glyph = CType(resources.GetObject("bbiSetting.Glyph"), System.Drawing.Image)
    Me.bbiSetting.Id = 7
    Me.bbiSetting.Name = "bbiSetting"
    Me.bbiSetting.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
    '
    'barDockControlTop
    '
    Me.barDockControlTop.CausesValidation = False
    Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
    Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
    Me.barDockControlTop.Size = New System.Drawing.Size(992, 0)
    '
    'barDockControlBottom
    '
    Me.barDockControlBottom.CausesValidation = False
    Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
    Me.barDockControlBottom.Location = New System.Drawing.Point(0, 624)
    Me.barDockControlBottom.Size = New System.Drawing.Size(992, 27)
    '
    'barDockControlLeft
    '
    Me.barDockControlLeft.CausesValidation = False
    Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
    Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
    Me.barDockControlLeft.Size = New System.Drawing.Size(0, 624)
    '
    'barDockControlRight
    '
    Me.barDockControlRight.CausesValidation = False
    Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
    Me.barDockControlRight.Location = New System.Drawing.Point(992, 0)
    Me.barDockControlRight.Size = New System.Drawing.Size(0, 624)
    '
    'BarStaticItem1
    '
    Me.BarStaticItem1.Caption = " "
    Me.BarStaticItem1.Id = 5
    Me.BarStaticItem1.Name = "BarStaticItem1"
    Me.BarStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near
    '
    'lvDetails
    '
    Me.lvDetails.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lvDetails.BackColor = System.Drawing.Color.White
    '
    '
    '
    Me.lvDetails.Border.Class = "ListViewBorder"
    Me.lvDetails.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
    Me.lvDetails.ForeColor = System.Drawing.Color.Black
    Me.lvDetails.Location = New System.Drawing.Point(20, 112)
    Me.lvDetails.Name = "lvDetails"
    Me.lvDetails.Size = New System.Drawing.Size(477, 469)
    Me.lvDetails.TabIndex = 280
    Me.lvDetails.UseCompatibleStateImageBehavior = False
    Me.lvDetails.View = System.Windows.Forms.View.Details
    '
    'grpSuchkriterien
    '
    Me.grpSuchkriterien.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.grpSuchkriterien.Controls.Add(Me.chkVerleihvertrag)
    Me.grpSuchkriterien.Controls.Add(Me.chkESVertrag)
    Me.grpSuchkriterien.Controls.Add(Me.lblKanton)
    Me.grpSuchkriterien.Controls.Add(Me.lblWOS)
    Me.grpSuchkriterien.Controls.Add(Me.btnWOSProperty)
    Me.grpSuchkriterien.Controls.Add(Me.cbo_Month)
    Me.grpSuchkriterien.Controls.Add(Me.cbo_Year)
    Me.grpSuchkriterien.Controls.Add(Me.lblJahr)
    Me.grpSuchkriterien.Controls.Add(Me.lblMonat)
    Me.grpSuchkriterien.Controls.Add(Me.lblESNr)
    Me.grpSuchkriterien.Controls.Add(Me.cbo_ESNr)
    Me.grpSuchkriterien.Controls.Add(Me.lblPVLBeruf)
    Me.grpSuchkriterien.Controls.Add(Me.cbo_PVLKanton)
    Me.grpSuchkriterien.Controls.Add(Me.cbo_PVLBez)
    Me.grpSuchkriterien.Location = New System.Drawing.Point(515, 112)
    Me.grpSuchkriterien.Name = "grpSuchkriterien"
    Me.grpSuchkriterien.Size = New System.Drawing.Size(449, 469)
    Me.grpSuchkriterien.TabIndex = 282
    Me.grpSuchkriterien.Text = "Suchkriterien"
    '
    'chkVerleihvertrag
    '
    Me.chkVerleihvertrag.EditValue = True
    Me.chkVerleihvertrag.Location = New System.Drawing.Point(124, 236)
    Me.chkVerleihvertrag.MenuManager = Me.BarManager1
    Me.chkVerleihvertrag.Name = "chkVerleihvertrag"
    Me.chkVerleihvertrag.Properties.Caption = "Verleihvertrag drucken"
    Me.chkVerleihvertrag.Size = New System.Drawing.Size(280, 19)
    Me.chkVerleihvertrag.TabIndex = 7
    '
    'chkESVertrag
    '
    Me.chkESVertrag.EditValue = True
    Me.chkESVertrag.Location = New System.Drawing.Point(124, 211)
    Me.chkESVertrag.MenuManager = Me.BarManager1
    Me.chkESVertrag.Name = "chkESVertrag"
    Me.chkESVertrag.Properties.Caption = "Einsatzvertrag drucken"
    Me.chkESVertrag.Size = New System.Drawing.Size(280, 19)
    Me.chkESVertrag.TabIndex = 6
    '
    'lblKanton
    '
    Me.lblKanton.BackColor = System.Drawing.Color.Transparent
    Me.lblKanton.Location = New System.Drawing.Point(16, 135)
    Me.lblKanton.Name = "lblKanton"
    Me.lblKanton.Size = New System.Drawing.Size(102, 13)
    Me.lblKanton.TabIndex = 278
    Me.lblKanton.Text = "Kanton"
    Me.lblKanton.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'lblWOS
    '
    Me.lblWOS.BackColor = System.Drawing.Color.Transparent
    Me.lblWOS.Location = New System.Drawing.Point(16, 267)
    Me.lblWOS.Name = "lblWOS"
    Me.lblWOS.Size = New System.Drawing.Size(102, 13)
    Me.lblWOS.TabIndex = 276
    Me.lblWOS.Text = "WOS-Kandidaten"
    Me.lblWOS.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'btnWOSProperty
    '
    Me.btnWOSProperty.Location = New System.Drawing.Point(124, 261)
    Me.btnWOSProperty.Name = "btnWOSProperty"
    Me.btnWOSProperty.Size = New System.Drawing.Size(280, 25)
    Me.btnWOSProperty.TabIndex = 5
    Me.btnWOSProperty.Text = "Drucken"
    '
    'cbo_Month
    '
    Me.cbo_Month.Location = New System.Drawing.Point(124, 65)
    Me.cbo_Month.Name = "cbo_Month"
    Me.cbo_Month.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.cbo_Month.Properties.DropDownRows = 12
    Me.cbo_Month.Size = New System.Drawing.Size(76, 20)
    Me.cbo_Month.TabIndex = 1
    '
    'cbo_Year
    '
    Me.cbo_Year.Location = New System.Drawing.Point(124, 39)
    Me.cbo_Year.Name = "cbo_Year"
    Me.cbo_Year.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.cbo_Year.Size = New System.Drawing.Size(76, 20)
    Me.cbo_Year.TabIndex = 0
    '
    'lblJahr
    '
    Me.lblJahr.BackColor = System.Drawing.Color.Transparent
    Me.lblJahr.Location = New System.Drawing.Point(16, 43)
    Me.lblJahr.Name = "lblJahr"
    Me.lblJahr.Size = New System.Drawing.Size(102, 13)
    Me.lblJahr.TabIndex = 19
    Me.lblJahr.Text = "Jahr"
    Me.lblJahr.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'lblMonat
    '
    Me.lblMonat.BackColor = System.Drawing.Color.Transparent
    Me.lblMonat.Location = New System.Drawing.Point(16, 69)
    Me.lblMonat.Name = "lblMonat"
    Me.lblMonat.Size = New System.Drawing.Size(102, 13)
    Me.lblMonat.TabIndex = 61
    Me.lblMonat.Text = "Monat"
    Me.lblMonat.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'lblESNr
    '
    Me.lblESNr.BackColor = System.Drawing.Color.Transparent
    Me.lblESNr.Location = New System.Drawing.Point(16, 187)
    Me.lblESNr.Name = "lblESNr"
    Me.lblESNr.Size = New System.Drawing.Size(102, 13)
    Me.lblESNr.TabIndex = 114
    Me.lblESNr.TabStop = True
    Me.lblESNr.Text = "Einsatz-Nr."
    Me.lblESNr.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'cbo_ESNr
    '
    Me.cbo_ESNr.Location = New System.Drawing.Point(124, 184)
    Me.cbo_ESNr.Name = "cbo_ESNr"
    Me.cbo_ESNr.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.cbo_ESNr.Size = New System.Drawing.Size(280, 20)
    Me.cbo_ESNr.TabIndex = 4
    '
    'lblPVLBeruf
    '
    Me.lblPVLBeruf.BackColor = System.Drawing.Color.Transparent
    Me.lblPVLBeruf.Location = New System.Drawing.Point(16, 161)
    Me.lblPVLBeruf.Name = "lblPVLBeruf"
    Me.lblPVLBeruf.Size = New System.Drawing.Size(102, 13)
    Me.lblPVLBeruf.TabIndex = 63
    Me.lblPVLBeruf.Text = "PVL-Beruf"
    Me.lblPVLBeruf.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'cbo_PVLKanton
    '
    Me.cbo_PVLKanton.Location = New System.Drawing.Point(124, 132)
    Me.cbo_PVLKanton.Name = "cbo_PVLKanton"
    Me.cbo_PVLKanton.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.cbo_PVLKanton.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
    Me.cbo_PVLKanton.Size = New System.Drawing.Size(76, 20)
    Me.cbo_PVLKanton.TabIndex = 2
    '
    'cbo_PVLBez
    '
    Me.cbo_PVLBez.Location = New System.Drawing.Point(124, 158)
    Me.cbo_PVLBez.Name = "cbo_PVLBez"
    Me.cbo_PVLBez.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.cbo_PVLBez.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
    Me.cbo_PVLBez.Size = New System.Drawing.Size(280, 20)
    Me.cbo_PVLBez.TabIndex = 3
    '
    'GroupBox1
    '
    Me.GroupBox1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003
    Me.GroupBox1.Controls.Add(Me.sbClose)
    Me.GroupBox1.Controls.Add(Me.Label2)
    Me.GroupBox1.Controls.Add(Me.LblTimeValue)
    Me.GroupBox1.Controls.Add(Me.lblHeader2)
    Me.GroupBox1.Controls.Add(Me.lblHeader1)
    Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
    Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
    Me.GroupBox1.Name = "GroupBox1"
    Me.GroupBox1.Size = New System.Drawing.Size(992, 77)
    Me.GroupBox1.TabIndex = 281
    '
    'sbClose
    '
    Me.sbClose.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.sbClose.Location = New System.Drawing.Point(819, 21)
    Me.sbClose.Name = "sbClose"
    Me.sbClose.Size = New System.Drawing.Size(100, 25)
    Me.sbClose.TabIndex = 204
    Me.sbClose.Text = "Schliessen"
    '
    'Label2
    '
    Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label2.ForeColor = System.Drawing.SystemColors.HotTrack
    Me.Label2.Image = CType(resources.GetObject("Label2.Image"), System.Drawing.Image)
    Me.Label2.Location = New System.Drawing.Point(5, 8)
    Me.Label2.Name = "Label2"
    Me.Label2.Size = New System.Drawing.Size(83, 65)
    Me.Label2.TabIndex = 1000
    '
    'LblTimeValue
    '
    Me.LblTimeValue.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.LblTimeValue.BackColor = System.Drawing.Color.Transparent
    Me.LblTimeValue.Location = New System.Drawing.Point(611, 49)
    Me.LblTimeValue.Name = "LblTimeValue"
    Me.LblTimeValue.Size = New System.Drawing.Size(308, 13)
    Me.LblTimeValue.TabIndex = 14
    Me.LblTimeValue.Text = "Zeitangaben..."
    Me.LblTimeValue.TextAlign = System.Drawing.ContentAlignment.TopRight
    Me.LblTimeValue.Visible = False
    '
    'lblHeader2
    '
    Me.lblHeader2.BackColor = System.Drawing.Color.Transparent
    Me.lblHeader2.Location = New System.Drawing.Point(112, 45)
    Me.lblHeader2.Name = "lblHeader2"
    Me.lblHeader2.Size = New System.Drawing.Size(299, 16)
    Me.lblHeader2.TabIndex = 1
    Me.lblHeader2.Text = "Geben Sie bitte Ihre gewünschten Kriterien ein."
    '
    'lblHeader1
    '
    Me.lblHeader1.AutoSize = True
    Me.lblHeader1.BackColor = System.Drawing.Color.Transparent
    Me.lblHeader1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblHeader1.Location = New System.Drawing.Point(94, 21)
    Me.lblHeader1.Name = "lblHeader1"
    Me.lblHeader1.Size = New System.Drawing.Size(257, 13)
    Me.lblHeader1.TabIndex = 0
    Me.lblHeader1.Text = "Diverse Verwaltungen für Einsatzverwaltung"
    '
    'lblDetails
    '
    Me.lblDetails.AutoSize = True
    Me.lblDetails.BackColor = System.Drawing.Color.Transparent
    Me.lblDetails.Location = New System.Drawing.Point(20, 94)
    Me.lblDetails.Name = "lblDetails"
    Me.lblDetails.Size = New System.Drawing.Size(39, 13)
    Me.lblDetails.TabIndex = 283
    Me.lblDetails.Text = "Details"
    '
    'frmESPrint
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(992, 651)
    Me.Controls.Add(Me.lblDetails)
    Me.Controls.Add(Me.lvDetails)
    Me.Controls.Add(Me.grpSuchkriterien)
    Me.Controls.Add(Me.GroupBox1)
    Me.Controls.Add(Me.barDockControlLeft)
    Me.Controls.Add(Me.barDockControlRight)
    Me.Controls.Add(Me.barDockControlBottom)
    Me.Controls.Add(Me.barDockControlTop)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.MinimumSize = New System.Drawing.Size(922, 608)
    Me.Name = "frmESPrint"
    Me.Text = "Diverse Verwaltungen für Einsatzverwaltung"
    CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.grpSuchkriterien, System.ComponentModel.ISupportInitialize).EndInit()
    Me.grpSuchkriterien.ResumeLayout(False)
    CType(Me.chkVerleihvertrag.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.chkESVertrag.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.cbo_Month.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.cbo_Year.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.cbo_ESNr.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.cbo_PVLKanton.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.cbo_PVLBez.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.GroupBox1.ResumeLayout(False)
    Me.GroupBox1.PerformLayout()
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents StyleManager1 As DevComponents.DotNetBar.StyleManager
  Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
  Friend WithEvents Bar4 As DevExpress.XtraBars.Bar
  Friend WithEvents bsiInfo As DevExpress.XtraBars.BarStaticItem
  Friend WithEvents bbiSearch As DevExpress.XtraBars.BarButtonItem
  Friend WithEvents bbiPrint As DevExpress.XtraBars.BarLargeButtonItem
  Friend WithEvents bbiDelete As DevExpress.XtraBars.BarButtonItem
  Friend WithEvents bbiExport As DevExpress.XtraBars.BarButtonItem
  Friend WithEvents bbiSetting As DevExpress.XtraBars.BarButtonItem
  Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
  Friend WithEvents BarStaticItem1 As DevExpress.XtraBars.BarStaticItem
  Friend WithEvents lvDetails As DevComponents.DotNetBar.Controls.ListViewEx
  Friend WithEvents grpSuchkriterien As DevExpress.XtraEditors.GroupControl
  Friend WithEvents lblWOS As System.Windows.Forms.Label
  Friend WithEvents btnWOSProperty As DevExpress.XtraEditors.DropDownButton
  Friend WithEvents cbo_Month As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents cbo_Year As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents lblJahr As System.Windows.Forms.Label
  Friend WithEvents lblMonat As System.Windows.Forms.Label
  Friend WithEvents lblESNr As System.Windows.Forms.Label
  Friend WithEvents cbo_ESNr As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents lblPVLBeruf As System.Windows.Forms.Label
  Friend WithEvents GroupBox1 As DevExpress.XtraEditors.PanelControl
  Friend WithEvents sbClose As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents Label2 As System.Windows.Forms.Label
  Friend WithEvents LblTimeValue As System.Windows.Forms.Label
  Friend WithEvents lblHeader2 As System.Windows.Forms.Label
  Friend WithEvents lblHeader1 As System.Windows.Forms.Label
  Friend WithEvents lblDetails As System.Windows.Forms.Label
  Friend WithEvents lblKanton As System.Windows.Forms.Label
  Friend WithEvents cbo_PVLKanton As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents cbo_PVLBez As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents chkVerleihvertrag As DevExpress.XtraEditors.CheckEdit
  Friend WithEvents chkESVertrag As DevExpress.XtraEditors.CheckEdit

End Class
