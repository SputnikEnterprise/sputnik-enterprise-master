<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMFakListSearch
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMFakListSearch))
		Me.GroupBox1 = New DevExpress.XtraEditors.PanelControl()
		Me.Label6 = New System.Windows.Forms.Label()
		Me.CmdClose = New DevExpress.XtraEditors.SimpleButton()
		Me.lblHeaderNormal = New System.Windows.Forms.Label()
		Me.lblHeaderFett = New System.Windows.Forms.Label()
		Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
		Me.txt_SQLQuery = New System.Windows.Forms.TextBox()
		Me.lblSQLAbfrage = New System.Windows.Forms.Label()
		Me.Cbo_Nationality = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.Cbo_Month_2 = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.Cbo_Month_1 = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.XtraTabControl1 = New DevExpress.XtraTab.XtraTabControl()
		Me.xtabAllgemein = New DevExpress.XtraTab.XtraTabPage()
		Me.lblNationalitaet = New System.Windows.Forms.Label()
		Me.lblMonat = New System.Windows.Forms.Label()
		Me.lueMandant = New DevExpress.XtraEditors.LookUpEdit()
		Me.lblMDName = New System.Windows.Forms.Label()
		Me.Cbo_Year = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.LblKst3 = New System.Windows.Forms.Label()
		Me.Cbo_Kanton = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.LblYear = New System.Windows.Forms.Label()
		Me.Cbo_Filiale = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.LblFilial = New System.Windows.Forms.Label()
		Me.xtabSQLAbfrage = New DevExpress.XtraTab.XtraTabPage()
		Me.Bar4 = New DevExpress.XtraBars.Bar()
		Me.beiWorking = New DevExpress.XtraBars.BarEditItem()
		Me.RepositoryItemMarqueeProgressBar2 = New DevExpress.XtraEditors.Repository.RepositoryItemMarqueeProgressBar()
		Me.bsiInfo = New DevExpress.XtraBars.BarStaticItem()
		Me.bbiSearch = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiClear = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiPrint = New DevExpress.XtraBars.BarLargeButtonItem()
		Me.bbiExport = New DevExpress.XtraBars.BarLargeButtonItem()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
		Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
		Me.BarStaticItem1 = New DevExpress.XtraBars.BarStaticItem()
		Me.BarEditItem1 = New DevExpress.XtraBars.BarEditItem()
		Me.RepositoryItemMarqueeProgressBar1 = New DevExpress.XtraEditors.Repository.RepositoryItemMarqueeProgressBar()
		Me.lbLstlInfo = New DevExpress.XtraEditors.LabelControl()
		Me.SwitchButton1 = New DevComponents.DotNetBar.Controls.SwitchButton()
		CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupBox1.SuspendLayout()
		CType(Me.Cbo_Nationality.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_Month_2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_Month_1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.XtraTabControl1.SuspendLayout()
		Me.xtabAllgemein.SuspendLayout()
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_Year.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_Kanton.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_Filiale.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.xtabSQLAbfrage.SuspendLayout()
		CType(Me.RepositoryItemMarqueeProgressBar2, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.RepositoryItemMarqueeProgressBar1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'GroupBox1
		'
		Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.GroupBox1.Appearance.BackColor = System.Drawing.Color.White
		Me.GroupBox1.Appearance.Options.UseBackColor = True
		Me.GroupBox1.Controls.Add(Me.Label6)
		Me.GroupBox1.Controls.Add(Me.CmdClose)
		Me.GroupBox1.Controls.Add(Me.lblHeaderNormal)
		Me.GroupBox1.Controls.Add(Me.lblHeaderFett)
		Me.GroupBox1.Location = New System.Drawing.Point(-1, -10)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(969, 93)
		Me.GroupBox1.TabIndex = 3
		'
		'Label6
		'
		Me.Label6.BackColor = System.Drawing.Color.Transparent
		Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label6.ForeColor = System.Drawing.SystemColors.HotTrack
		Me.Label6.Image = CType(resources.GetObject("Label6.Image"), System.Drawing.Image)
		Me.Label6.Location = New System.Drawing.Point(6, 16)
		Me.Label6.Name = "Label6"
		Me.Label6.Size = New System.Drawing.Size(83, 65)
		Me.Label6.TabIndex = 108
		'
		'CmdClose
		'
		Me.CmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.CmdClose.Location = New System.Drawing.Point(804, 26)
		Me.CmdClose.Name = "CmdClose"
		Me.CmdClose.Size = New System.Drawing.Size(86, 24)
		Me.CmdClose.TabIndex = 5
		Me.CmdClose.Text = "Schliessen"
		'
		'lblHeaderNormal
		'
		Me.lblHeaderNormal.AutoSize = True
		Me.lblHeaderNormal.BackColor = System.Drawing.Color.Transparent
		Me.lblHeaderNormal.Location = New System.Drawing.Point(113, 53)
		Me.lblHeaderNormal.Name = "lblHeaderNormal"
		Me.lblHeaderNormal.Size = New System.Drawing.Size(225, 13)
		Me.lblHeaderNormal.TabIndex = 1
		Me.lblHeaderNormal.Text = "Wählen Sie Ihre gewünschten Kriterien aus..."
		'
		'lblHeaderFett
		'
		Me.lblHeaderFett.AutoSize = True
		Me.lblHeaderFett.BackColor = System.Drawing.Color.Transparent
		Me.lblHeaderFett.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblHeaderFett.Location = New System.Drawing.Point(95, 29)
		Me.lblHeaderFett.Name = "lblHeaderFett"
		Me.lblHeaderFett.Size = New System.Drawing.Size(241, 13)
		Me.lblHeaderFett.TabIndex = 0
		Me.lblHeaderFett.Text = "Liste der Kinder- und Ausbildungszulagen"
		'
		'BackgroundWorker1
		'
		'
		'txt_SQLQuery
		'
		Me.txt_SQLQuery.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txt_SQLQuery.Location = New System.Drawing.Point(21, 34)
		Me.txt_SQLQuery.Multiline = True
		Me.txt_SQLQuery.Name = "txt_SQLQuery"
		Me.txt_SQLQuery.Size = New System.Drawing.Size(490, 217)
		Me.txt_SQLQuery.TabIndex = 100
		'
		'lblSQLAbfrage
		'
		Me.lblSQLAbfrage.AutoSize = True
		Me.lblSQLAbfrage.Location = New System.Drawing.Point(18, 18)
		Me.lblSQLAbfrage.Name = "lblSQLAbfrage"
		Me.lblSQLAbfrage.Size = New System.Drawing.Size(154, 13)
		Me.lblSQLAbfrage.TabIndex = 105
		Me.lblSQLAbfrage.Text = "Ihre derzeitige Abfrage lautet:"
		Me.lblSQLAbfrage.Visible = False
		'
		'Cbo_Nationality
		'
		Me.Cbo_Nationality.Location = New System.Drawing.Point(156, 193)
		Me.Cbo_Nationality.Name = "Cbo_Nationality"
		Me.Cbo_Nationality.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.DropDown)})
		Me.Cbo_Nationality.Size = New System.Drawing.Size(114, 20)
		Me.Cbo_Nationality.TabIndex = 5
		'
		'Cbo_Month_2
		'
		Me.Cbo_Month_2.Location = New System.Drawing.Point(404, 100)
		Me.Cbo_Month_2.Name = "Cbo_Month_2"
		Me.Cbo_Month_2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.DropDown)})
		Me.Cbo_Month_2.Size = New System.Drawing.Size(54, 20)
		Me.Cbo_Month_2.TabIndex = 1
		Me.Cbo_Month_2.Visible = False
		'
		'Cbo_Month_1
		'
		Me.Cbo_Month_1.Location = New System.Drawing.Point(156, 100)
		Me.Cbo_Month_1.Name = "Cbo_Month_1"
		Me.Cbo_Month_1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.DropDown)})
		Me.Cbo_Month_1.Size = New System.Drawing.Size(54, 20)
		Me.Cbo_Month_1.TabIndex = 0
		'
		'XtraTabControl1
		'
		Me.XtraTabControl1.Location = New System.Drawing.Point(350, 97)
		Me.XtraTabControl1.Name = "XtraTabControl1"
		Me.XtraTabControl1.SelectedTabPage = Me.xtabAllgemein
		Me.XtraTabControl1.Size = New System.Drawing.Size(539, 277)
		Me.XtraTabControl1.TabIndex = 108
		Me.XtraTabControl1.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabAllgemein, Me.xtabSQLAbfrage})
		'
		'xtabAllgemein
		'
		Me.xtabAllgemein.Controls.Add(Me.SwitchButton1)
		Me.xtabAllgemein.Controls.Add(Me.lblNationalitaet)
		Me.xtabAllgemein.Controls.Add(Me.lblMonat)
		Me.xtabAllgemein.Controls.Add(Me.lueMandant)
		Me.xtabAllgemein.Controls.Add(Me.lblMDName)
		Me.xtabAllgemein.Controls.Add(Me.Cbo_Year)
		Me.xtabAllgemein.Controls.Add(Me.LblKst3)
		Me.xtabAllgemein.Controls.Add(Me.Cbo_Kanton)
		Me.xtabAllgemein.Controls.Add(Me.LblYear)
		Me.xtabAllgemein.Controls.Add(Me.Cbo_Filiale)
		Me.xtabAllgemein.Controls.Add(Me.LblFilial)
		Me.xtabAllgemein.Controls.Add(Me.Cbo_Nationality)
		Me.xtabAllgemein.Controls.Add(Me.Cbo_Month_1)
		Me.xtabAllgemein.Controls.Add(Me.Cbo_Month_2)
		Me.xtabAllgemein.Name = "xtabAllgemein"
		Me.xtabAllgemein.Size = New System.Drawing.Size(533, 249)
		Me.xtabAllgemein.Text = "Allgemein"
		'
		'lblNationalitaet
		'
		Me.lblNationalitaet.Location = New System.Drawing.Point(47, 196)
		Me.lblNationalitaet.Name = "lblNationalitaet"
		Me.lblNationalitaet.Size = New System.Drawing.Size(103, 13)
		Me.lblNationalitaet.TabIndex = 212
		Me.lblNationalitaet.Text = "Nationalität"
		Me.lblNationalitaet.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblMonat
		'
		Me.lblMonat.Location = New System.Drawing.Point(47, 103)
		Me.lblMonat.Name = "lblMonat"
		Me.lblMonat.Size = New System.Drawing.Size(103, 13)
		Me.lblMonat.TabIndex = 211
		Me.lblMonat.Text = "Monat"
		Me.lblMonat.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lueMandant
		'
		Me.lueMandant.Location = New System.Drawing.Point(156, 32)
		Me.lueMandant.Name = "lueMandant"
		Me.lueMandant.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.lueMandant.Properties.NullText = ""
		Me.lueMandant.Size = New System.Drawing.Size(302, 20)
		Me.lueMandant.TabIndex = 203
		'
		'lblMDName
		'
		Me.lblMDName.Location = New System.Drawing.Point(39, 36)
		Me.lblMDName.Name = "lblMDName"
		Me.lblMDName.Size = New System.Drawing.Size(111, 13)
		Me.lblMDName.TabIndex = 210
		Me.lblMDName.Text = "Mandanten"
		Me.lblMDName.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'Cbo_Year
		'
		Me.Cbo_Year.Location = New System.Drawing.Point(156, 74)
		Me.Cbo_Year.Name = "Cbo_Year"
		Me.Cbo_Year.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_Year.Size = New System.Drawing.Size(114, 20)
		Me.Cbo_Year.TabIndex = 204
		'
		'LblKst3
		'
		Me.LblKst3.Location = New System.Drawing.Point(47, 143)
		Me.LblKst3.Name = "LblKst3"
		Me.LblKst3.Size = New System.Drawing.Size(103, 13)
		Me.LblKst3.TabIndex = 209
		Me.LblKst3.Text = "Kanton"
		Me.LblKst3.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'Cbo_Kanton
		'
		Me.Cbo_Kanton.Location = New System.Drawing.Point(156, 140)
		Me.Cbo_Kanton.Name = "Cbo_Kanton"
		Me.Cbo_Kanton.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_Kanton.Size = New System.Drawing.Size(114, 20)
		Me.Cbo_Kanton.TabIndex = 205
		'
		'LblYear
		'
		Me.LblYear.Location = New System.Drawing.Point(47, 77)
		Me.LblYear.Name = "LblYear"
		Me.LblYear.Size = New System.Drawing.Size(103, 13)
		Me.LblYear.TabIndex = 207
		Me.LblYear.Text = "Jahr"
		Me.LblYear.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'Cbo_Filiale
		'
		Me.Cbo_Filiale.Location = New System.Drawing.Point(156, 167)
		Me.Cbo_Filiale.Name = "Cbo_Filiale"
		Me.Cbo_Filiale.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_Filiale.Size = New System.Drawing.Size(114, 20)
		Me.Cbo_Filiale.TabIndex = 206
		'
		'LblFilial
		'
		Me.LblFilial.Location = New System.Drawing.Point(47, 170)
		Me.LblFilial.Name = "LblFilial"
		Me.LblFilial.Size = New System.Drawing.Size(103, 13)
		Me.LblFilial.TabIndex = 208
		Me.LblFilial.Text = "Filiale"
		Me.LblFilial.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'xtabSQLAbfrage
		'
		Me.xtabSQLAbfrage.Controls.Add(Me.lblSQLAbfrage)
		Me.xtabSQLAbfrage.Controls.Add(Me.txt_SQLQuery)
		Me.xtabSQLAbfrage.Name = "xtabSQLAbfrage"
		Me.xtabSQLAbfrage.Size = New System.Drawing.Size(533, 249)
		Me.xtabSQLAbfrage.Text = "SQL- Abfrage"
		'
		'Bar4
		'
		Me.Bar4.BarName = "Statusleiste"
		Me.Bar4.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
		Me.Bar4.DockCol = 0
		Me.Bar4.DockRow = 0
		Me.Bar4.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
		Me.Bar4.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.beiWorking), New DevExpress.XtraBars.LinkPersistInfo(Me.bsiInfo), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiSearch), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiClear, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiPrint), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiExport, True)})
		Me.Bar4.OptionsBar.AllowQuickCustomization = False
		Me.Bar4.OptionsBar.DrawDragBorder = False
		Me.Bar4.OptionsBar.UseWholeRow = True
		Me.Bar4.Text = "Statusleiste"
		'
		'beiWorking
		'
		Me.beiWorking.Caption = "BarEditItem2"
		Me.beiWorking.Edit = Me.RepositoryItemMarqueeProgressBar2
		Me.beiWorking.Id = 11
		Me.beiWorking.Name = "beiWorking"
		'
		'RepositoryItemMarqueeProgressBar2
		'
		Me.RepositoryItemMarqueeProgressBar2.Name = "RepositoryItemMarqueeProgressBar2"
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
		Me.bbiSearch.Name = "bbiSearch"
		Me.bbiSearch.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiClear
		'
		Me.bbiClear.Caption = "Felder leeren"
		Me.bbiClear.Glyph = CType(resources.GetObject("bbiClear.Glyph"), System.Drawing.Image)
		Me.bbiClear.Id = 8
		Me.bbiClear.Name = "bbiClear"
		Me.bbiClear.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiPrint
		'
		Me.bbiPrint.AllowAllUp = True
		Me.bbiPrint.Caption = "Drucken"
		Me.bbiPrint.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Right
		Me.bbiPrint.Glyph = CType(resources.GetObject("bbiPrint.Glyph"), System.Drawing.Image)
		Me.bbiPrint.Id = 6
		Me.bbiPrint.Name = "bbiPrint"
		Me.bbiPrint.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiExport
		'
		Me.bbiExport.Caption = "Exportieren"
		Me.bbiExport.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Right
		Me.bbiExport.Glyph = CType(resources.GetObject("bbiExport.Glyph"), System.Drawing.Image)
		Me.bbiExport.Id = 4
		Me.bbiExport.Name = "bbiExport"
		Me.bbiExport.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'BarManager1
		'
		Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar4})
		Me.BarManager1.DockControls.Add(Me.barDockControlTop)
		Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
		Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
		Me.BarManager1.DockControls.Add(Me.barDockControlRight)
		Me.BarManager1.Form = Me
		Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiInfo, Me.bbiSearch, Me.bbiPrint, Me.bbiClear, Me.BarStaticItem1, Me.bbiExport, Me.BarEditItem1, Me.beiWorking})
		Me.BarManager1.MaxItemId = 12
		Me.BarManager1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemMarqueeProgressBar1, Me.RepositoryItemMarqueeProgressBar2})
		Me.BarManager1.StatusBar = Me.Bar4
		'
		'barDockControlTop
		'
		Me.barDockControlTop.CausesValidation = False
		Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
		Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlTop.Size = New System.Drawing.Size(940, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 414)
		Me.barDockControlBottom.Size = New System.Drawing.Size(940, 27)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 414)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(940, 0)
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 414)
		'
		'BarStaticItem1
		'
		Me.BarStaticItem1.Caption = " "
		Me.BarStaticItem1.Id = 5
		Me.BarStaticItem1.Name = "BarStaticItem1"
		Me.BarStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near
		'
		'BarEditItem1
		'
		Me.BarEditItem1.Caption = "BarEditItem1"
		Me.BarEditItem1.Edit = Me.RepositoryItemMarqueeProgressBar1
		Me.BarEditItem1.Id = 10
		Me.BarEditItem1.Name = "BarEditItem1"
		'
		'RepositoryItemMarqueeProgressBar1
		'
		Me.RepositoryItemMarqueeProgressBar1.Name = "RepositoryItemMarqueeProgressBar1"
		'
		'lbLstlInfo
		'
		Me.lbLstlInfo.AllowHtmlString = True
		Me.lbLstlInfo.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.lbLstlInfo.Appearance.Image = CType(resources.GetObject("lbLstlInfo.Appearance.Image"), System.Drawing.Image)
		Me.lbLstlInfo.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter
		Me.lbLstlInfo.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
		Me.lbLstlInfo.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
		Me.lbLstlInfo.AutoEllipsis = True
		Me.lbLstlInfo.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lbLstlInfo.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftTop
		Me.lbLstlInfo.LineColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
		Me.lbLstlInfo.LineLocation = DevExpress.XtraEditors.LineLocation.Bottom
		Me.lbLstlInfo.LineOrientation = DevExpress.XtraEditors.LabelLineOrientation.Vertical
		Me.lbLstlInfo.LineVisible = True
		Me.lbLstlInfo.Location = New System.Drawing.Point(15, 97)
		Me.lbLstlInfo.Name = "lbLstlInfo"
		Me.lbLstlInfo.Size = New System.Drawing.Size(306, 218)
		Me.lbLstlInfo.TabIndex = 290
		Me.lbLstlInfo.Text = "<b>Info</b>" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "bla bla."
		'
		'SwitchButton1
		'
		Me.SwitchButton1.BackColor = System.Drawing.Color.White
		'
		'
		'
		Me.SwitchButton1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
		Me.SwitchButton1.FocusCuesEnabled = False
		Me.SwitchButton1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.SwitchButton1.Location = New System.Drawing.Point(360, 101)
		Me.SwitchButton1.Name = "SwitchButton1"
		Me.SwitchButton1.OffBackColor = System.Drawing.Color.White
		Me.SwitchButton1.OffText = "O"
		Me.SwitchButton1.OffTextColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(53, Byte), Integer))
		Me.SwitchButton1.OnBackColor = System.Drawing.Color.LightSteelBlue
		Me.SwitchButton1.OnText = "|"
		Me.SwitchButton1.OnTextColor = System.Drawing.Color.Black
		Me.SwitchButton1.Size = New System.Drawing.Size(38, 19)
		Me.SwitchButton1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
		Me.SwitchButton1.SwitchBackColor = System.Drawing.Color.DarkGray
		Me.SwitchButton1.SwitchBorderColor = System.Drawing.Color.DarkGray
		Me.SwitchButton1.SwitchFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.SwitchButton1.SwitchWidth = 6
		Me.SwitchButton1.TabIndex = 298
		'
		'frmMFakListSearch
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(940, 441)
		Me.Controls.Add(Me.lbLstlInfo)
		Me.Controls.Add(Me.XtraTabControl1)
		Me.Controls.Add(Me.GroupBox1)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.MaximumSize = New System.Drawing.Size(956, 479)
		Me.MinimumSize = New System.Drawing.Size(956, 479)
		Me.Name = "frmMFakListSearch"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Suche nach Kinder- und Ausbildungszulagen"
		CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupBox1.ResumeLayout(False)
		Me.GroupBox1.PerformLayout()
		CType(Me.Cbo_Nationality.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_Month_2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_Month_1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.XtraTabControl1.ResumeLayout(False)
		Me.xtabAllgemein.ResumeLayout(False)
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_Year.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_Kanton.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_Filiale.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.xtabSQLAbfrage.ResumeLayout(False)
		Me.xtabSQLAbfrage.PerformLayout()
		CType(Me.RepositoryItemMarqueeProgressBar2, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.RepositoryItemMarqueeProgressBar1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents GroupBox1 As DevExpress.XtraEditors.PanelControl
	Friend WithEvents lblHeaderNormal As System.Windows.Forms.Label
	Friend WithEvents lblHeaderFett As System.Windows.Forms.Label
	Friend WithEvents CmdClose As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
	Friend WithEvents txt_SQLQuery As System.Windows.Forms.TextBox
	Friend WithEvents Cbo_Month_2 As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents Cbo_Month_1 As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents lblSQLAbfrage As System.Windows.Forms.Label
	Friend WithEvents Cbo_Nationality As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents Label6 As System.Windows.Forms.Label
	Friend WithEvents XtraTabControl1 As DevExpress.XtraTab.XtraTabControl
	Friend WithEvents xtabAllgemein As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents xtabSQLAbfrage As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents lblNationalitaet As System.Windows.Forms.Label
	Friend WithEvents lblMonat As System.Windows.Forms.Label
	Friend WithEvents lueMandant As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents lblMDName As System.Windows.Forms.Label
	Friend WithEvents Cbo_Year As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents LblKst3 As System.Windows.Forms.Label
	Friend WithEvents Cbo_Kanton As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents LblYear As System.Windows.Forms.Label
	Friend WithEvents Cbo_Filiale As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents LblFilial As System.Windows.Forms.Label
	Friend WithEvents Bar4 As DevExpress.XtraBars.Bar
	Friend WithEvents beiWorking As DevExpress.XtraBars.BarEditItem
	Friend WithEvents RepositoryItemMarqueeProgressBar2 As DevExpress.XtraEditors.Repository.RepositoryItemMarqueeProgressBar
	Friend WithEvents bsiInfo As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents bbiSearch As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbiClear As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbiPrint As DevExpress.XtraBars.BarLargeButtonItem
	Friend WithEvents bbiExport As DevExpress.XtraBars.BarLargeButtonItem
	Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
	Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
	Friend WithEvents BarStaticItem1 As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents BarEditItem1 As DevExpress.XtraBars.BarEditItem
	Friend WithEvents RepositoryItemMarqueeProgressBar1 As DevExpress.XtraEditors.Repository.RepositoryItemMarqueeProgressBar
	Friend WithEvents lbLstlInfo As DevExpress.XtraEditors.LabelControl
	Friend WithEvents SwitchButton1 As DevComponents.DotNetBar.Controls.SwitchButton

End Class
