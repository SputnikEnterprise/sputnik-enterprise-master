<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPrintESRP
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPrintESRP))
		Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Me.lblAktuellQery = New System.Windows.Forms.Label()
		Me.txt_SQL_1 = New DevExpress.XtraEditors.MemoEdit()
		Me.txt_SQL_2 = New DevExpress.XtraEditors.MemoEdit()
		Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
		Me.bsiInfo = New DevExpress.XtraBars.BarStaticItem()
		Me.bbiSearch = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiClear = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiPrint = New DevExpress.XtraBars.BarLargeButtonItem()
		Me.BarStaticItem1 = New DevExpress.XtraBars.BarStaticItem()
		Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
		Me.xtabSQLAbfrage = New DevExpress.XtraTab.XtraTabPage()
		Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
		Me.PictureEdit1 = New DevExpress.XtraEditors.PictureEdit()
		Me.LblTimeValue = New DevExpress.XtraEditors.LabelControl()
		Me.lblHeader2 = New DevExpress.XtraEditors.LabelControl()
		Me.CmdClose = New DevExpress.XtraEditors.SimpleButton()
		Me.lblHeader1 = New System.Windows.Forms.Label()
		Me.xtabRPSearch = New DevExpress.XtraTab.XtraTabControl()
		Me.xtabAllgemein = New DevExpress.XtraTab.XtraTabPage()
		Me.LookUpEdit2 = New DevExpress.XtraEditors.LookUpEdit()
		Me.lueWeek = New DevExpress.XtraEditors.LookUpEdit()
		Me.lblWoche = New DevExpress.XtraEditors.LabelControl()
		Me.lueMandant = New DevExpress.XtraEditors.LookUpEdit()
		Me.lblMDName = New System.Windows.Forms.Label()
		Me.Cbo_Year = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.lblBerater = New System.Windows.Forms.Label()
		Me.lblMonat = New System.Windows.Forms.Label()
		Me.lbl1KST = New System.Windows.Forms.Label()
		Me.lblJahr = New System.Windows.Forms.Label()
		Me.Cbo_Berater = New DevExpress.XtraEditors.CheckedComboBoxEdit()
		Me.Cbo_Month = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.lueES = New DevExpress.XtraEditors.LookUpEdit()
		Me.XtraTabPage1 = New DevExpress.XtraTab.XtraTabPage()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.MemoEdit1 = New DevExpress.XtraEditors.MemoEdit()
		Me.MemoEdit2 = New DevExpress.XtraEditors.MemoEdit()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
		Me.Bar3 = New DevExpress.XtraBars.Bar()
		Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
		Me.BarDockControl1 = New DevExpress.XtraBars.BarDockControl()
		CType(Me.txt_SQL_1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txt_SQL_2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.xtabSQLAbfrage.SuspendLayout()
		CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.PanelControl1.SuspendLayout()
		CType(Me.PictureEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.xtabRPSearch, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.xtabRPSearch.SuspendLayout()
		Me.xtabAllgemein.SuspendLayout()
		CType(Me.LookUpEdit2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lueWeek.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_Year.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_Berater.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_Month.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lueES.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.XtraTabPage1.SuspendLayout()
		CType(Me.MemoEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.MemoEdit2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'lblAktuellQery
		'
		Me.lblAktuellQery.AutoSize = True
		Me.lblAktuellQery.Location = New System.Drawing.Point(32, 20)
		Me.lblAktuellQery.Name = "lblAktuellQery"
		Me.lblAktuellQery.Size = New System.Drawing.Size(145, 13)
		Me.lblAktuellQery.TabIndex = 102
		Me.lblAktuellQery.Text = "Ihre derzeitige Abfrage lautet:"
		'
		'txt_SQL_1
		'
		Me.txt_SQL_1.Location = New System.Drawing.Point(35, 36)
		Me.txt_SQL_1.Name = "txt_SQL_1"
		Me.txt_SQL_1.Size = New System.Drawing.Size(599, 120)
		Me.txt_SQL_1.TabIndex = 100
		Me.txt_SQL_1.UseOptimizedRendering = True
		'
		'txt_SQL_2
		'
		Me.txt_SQL_2.Location = New System.Drawing.Point(35, 162)
		Me.txt_SQL_2.Name = "txt_SQL_2"
		Me.txt_SQL_2.Size = New System.Drawing.Size(599, 120)
		Me.txt_SQL_2.TabIndex = 101
		Me.txt_SQL_2.UseOptimizedRendering = True
		'
		'BackgroundWorker1
		'
		'
		'bsiInfo
		'
		Me.bsiInfo.AutoSize = DevExpress.XtraBars.BarStaticItemSize.Spring
		Me.bsiInfo.Caption = "Bereit"
		Me.bsiInfo.Id = 0
		Me.bsiInfo.Name = "bsiInfo"
		Me.bsiInfo.TextAlignment = System.Drawing.StringAlignment.Near
		Me.bsiInfo.Width = 300
		'
		'bbiSearch
		'
		Me.bbiSearch.Caption = "Suchen"
		Me.bbiSearch.Glyph = CType(resources.GetObject("bbiSearch.Glyph"), System.Drawing.Image)
		Me.bbiSearch.Id = 1
		Me.bbiSearch.LargeGlyph = CType(resources.GetObject("bbiSearch.LargeGlyph"), System.Drawing.Image)
		Me.bbiSearch.Name = "bbiSearch"
		Me.bbiSearch.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		Me.bbiSearch.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText
		'
		'bbiClear
		'
		Me.bbiClear.Caption = "Felder leeren"
		Me.bbiClear.Glyph = CType(resources.GetObject("bbiClear.Glyph"), System.Drawing.Image)
		Me.bbiClear.Id = 2
		Me.bbiClear.LargeGlyph = CType(resources.GetObject("bbiClear.LargeGlyph"), System.Drawing.Image)
		Me.bbiClear.Name = "bbiClear"
		Me.bbiClear.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiPrint
		'
		Me.bbiPrint.Caption = "Drucken"
		Me.bbiPrint.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Right
		Me.bbiPrint.Glyph = CType(resources.GetObject("bbiPrint.Glyph"), System.Drawing.Image)
		Me.bbiPrint.Id = 7
		Me.bbiPrint.Name = "bbiPrint"
		Me.bbiPrint.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'BarStaticItem1
		'
		Me.BarStaticItem1.Id = 9
		Me.BarStaticItem1.Name = "BarStaticItem1"
		Me.BarStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(820, 0)
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 483)
		'
		'xtabSQLAbfrage
		'
		Me.xtabSQLAbfrage.Controls.Add(Me.lblAktuellQery)
		Me.xtabSQLAbfrage.Controls.Add(Me.txt_SQL_1)
		Me.xtabSQLAbfrage.Controls.Add(Me.txt_SQL_2)
		Me.xtabSQLAbfrage.Name = "xtabSQLAbfrage"
		Me.xtabSQLAbfrage.Size = New System.Drawing.Size(662, 293)
		Me.xtabSQLAbfrage.Text = "SQL-Abfrage"
		'
		'PanelControl1
		'
		Me.PanelControl1.Controls.Add(Me.PictureEdit1)
		Me.PanelControl1.Controls.Add(Me.LblTimeValue)
		Me.PanelControl1.Controls.Add(Me.lblHeader2)
		Me.PanelControl1.Controls.Add(Me.CmdClose)
		Me.PanelControl1.Controls.Add(Me.lblHeader1)
		Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Top
		Me.PanelControl1.Location = New System.Drawing.Point(0, 0)
		Me.PanelControl1.Name = "PanelControl1"
		Me.PanelControl1.Size = New System.Drawing.Size(820, 79)
		Me.PanelControl1.TabIndex = 234
		'
		'PictureEdit1
		'
		Me.PictureEdit1.EditValue = CType(resources.GetObject("PictureEdit1.EditValue"), Object)
		Me.PictureEdit1.Location = New System.Drawing.Point(12, 12)
		Me.PictureEdit1.Name = "PictureEdit1"
		Me.PictureEdit1.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.PictureEdit1.Properties.Appearance.Options.UseBackColor = True
		Me.PictureEdit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
		Me.PictureEdit1.Properties.PictureAlignment = System.Drawing.ContentAlignment.TopLeft
		Me.PictureEdit1.Size = New System.Drawing.Size(66, 59)
		Me.PictureEdit1.TabIndex = 233
		'
		'LblTimeValue
		'
		Me.LblTimeValue.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.LblTimeValue.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.LblTimeValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.LblTimeValue.Location = New System.Drawing.Point(579, 52)
		Me.LblTimeValue.Name = "LblTimeValue"
		Me.LblTimeValue.Size = New System.Drawing.Size(201, 13)
		Me.LblTimeValue.TabIndex = 14
		Me.LblTimeValue.Text = "Zeitangaben..."
		Me.LblTimeValue.Visible = False
		'
		'lblHeader2
		'
		Me.lblHeader2.Location = New System.Drawing.Point(110, 44)
		Me.lblHeader2.Name = "lblHeader2"
		Me.lblHeader2.Size = New System.Drawing.Size(218, 13)
		Me.lblHeader2.TabIndex = 1
		Me.lblHeader2.Text = "Wählen Sie Ihre gewünschten Kriterien aus..."
		'
		'CmdClose
		'
		Me.CmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.CmdClose.Location = New System.Drawing.Point(694, 22)
		Me.CmdClose.Name = "CmdClose"
		Me.CmdClose.Size = New System.Drawing.Size(86, 24)
		Me.CmdClose.TabIndex = 999
		Me.CmdClose.Text = "Schliessen"
		'
		'lblHeader1
		'
		Me.lblHeader1.AutoSize = True
		Me.lblHeader1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblHeader1.Location = New System.Drawing.Point(84, 22)
		Me.lblHeader1.Name = "lblHeader1"
		Me.lblHeader1.Size = New System.Drawing.Size(109, 13)
		Me.lblHeader1.TabIndex = 0
		Me.lblHeader1.Text = "Rapporte drucken"
		'
		'xtabRPSearch
		'
		Me.xtabRPSearch.Location = New System.Drawing.Point(28, 98)
		Me.xtabRPSearch.Name = "xtabRPSearch"
		Me.xtabRPSearch.SelectedTabPage = Me.xtabAllgemein
		Me.xtabRPSearch.Size = New System.Drawing.Size(668, 321)
		Me.xtabRPSearch.TabIndex = 239
		Me.xtabRPSearch.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabAllgemein, Me.XtraTabPage1})
		'
		'xtabAllgemein
		'
		Me.xtabAllgemein.Controls.Add(Me.LookUpEdit2)
		Me.xtabAllgemein.Controls.Add(Me.lueWeek)
		Me.xtabAllgemein.Controls.Add(Me.lblWoche)
		Me.xtabAllgemein.Controls.Add(Me.lueMandant)
		Me.xtabAllgemein.Controls.Add(Me.lblMDName)
		Me.xtabAllgemein.Controls.Add(Me.Cbo_Year)
		Me.xtabAllgemein.Controls.Add(Me.lblBerater)
		Me.xtabAllgemein.Controls.Add(Me.lblMonat)
		Me.xtabAllgemein.Controls.Add(Me.lbl1KST)
		Me.xtabAllgemein.Controls.Add(Me.lblJahr)
		Me.xtabAllgemein.Controls.Add(Me.Cbo_Berater)
		Me.xtabAllgemein.Controls.Add(Me.Cbo_Month)
		Me.xtabAllgemein.Controls.Add(Me.lueES)
		Me.xtabAllgemein.Name = "xtabAllgemein"
		Me.xtabAllgemein.Size = New System.Drawing.Size(662, 293)
		Me.xtabAllgemein.Text = "Allgemein"
		'
		'LookUpEdit2
		'
		Me.LookUpEdit2.Location = New System.Drawing.Point(142, 175)
		Me.LookUpEdit2.Name = "LookUpEdit2"
		SerializableAppearanceObject1.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject1.Options.UseForeColor = True
		Me.LookUpEdit2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, "", Nothing, Nothing, True)})
		Me.LookUpEdit2.Properties.ShowFooter = False
		Me.LookUpEdit2.Size = New System.Drawing.Size(103, 20)
		Me.LookUpEdit2.TabIndex = 171
		'
		'lueWeek
		'
		Me.lueWeek.Location = New System.Drawing.Point(142, 149)
		Me.lueWeek.Name = "lueWeek"
		SerializableAppearanceObject2.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject2.Options.UseForeColor = True
		Me.lueWeek.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject2, "", Nothing, Nothing, True)})
		Me.lueWeek.Properties.ShowFooter = False
		Me.lueWeek.Size = New System.Drawing.Size(103, 20)
		Me.lueWeek.TabIndex = 169
		'
		'lblWoche
		'
		Me.lblWoche.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblWoche.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblWoche.Location = New System.Drawing.Point(63, 153)
		Me.lblWoche.Name = "lblWoche"
		Me.lblWoche.Size = New System.Drawing.Size(73, 13)
		Me.lblWoche.TabIndex = 170
		Me.lblWoche.Text = "Woche"
		'
		'lueMandant
		'
		Me.lueMandant.Location = New System.Drawing.Point(142, 34)
		Me.lueMandant.Name = "lueMandant"
		Me.lueMandant.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.lueMandant.Properties.NullText = ""
		Me.lueMandant.Size = New System.Drawing.Size(481, 20)
		Me.lueMandant.TabIndex = 168
		'
		'lblMDName
		'
		Me.lblMDName.Location = New System.Drawing.Point(19, 38)
		Me.lblMDName.Name = "lblMDName"
		Me.lblMDName.Size = New System.Drawing.Size(117, 13)
		Me.lblMDName.TabIndex = 167
		Me.lblMDName.Text = "Mandanten"
		Me.lblMDName.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'Cbo_Year
		'
		Me.Cbo_Year.Location = New System.Drawing.Point(142, 123)
		Me.Cbo_Year.Name = "Cbo_Year"
		Me.Cbo_Year.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_Year.Size = New System.Drawing.Size(103, 20)
		Me.Cbo_Year.TabIndex = 1
		'
		'lblBerater
		'
		Me.lblBerater.Location = New System.Drawing.Point(6, 205)
		Me.lblBerater.Name = "lblBerater"
		Me.lblBerater.Size = New System.Drawing.Size(130, 13)
		Me.lblBerater.TabIndex = 84
		Me.lblBerater.Text = "BeraterIn"
		Me.lblBerater.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblMonat
		'
		Me.lblMonat.Location = New System.Drawing.Point(313, 127)
		Me.lblMonat.Name = "lblMonat"
		Me.lblMonat.Size = New System.Drawing.Size(130, 13)
		Me.lblMonat.TabIndex = 21
		Me.lblMonat.Text = "Monat"
		Me.lblMonat.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lbl1KST
		'
		Me.lbl1KST.Location = New System.Drawing.Point(6, 101)
		Me.lbl1KST.Name = "lbl1KST"
		Me.lbl1KST.Size = New System.Drawing.Size(130, 13)
		Me.lbl1KST.TabIndex = 94
		Me.lbl1KST.Text = "Einsatznummer"
		Me.lbl1KST.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblJahr
		'
		Me.lblJahr.Location = New System.Drawing.Point(6, 127)
		Me.lblJahr.Name = "lblJahr"
		Me.lblJahr.Size = New System.Drawing.Size(130, 13)
		Me.lblJahr.TabIndex = 82
		Me.lblJahr.Text = "Jahr"
		Me.lblJahr.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'Cbo_Berater
		'
		Me.Cbo_Berater.EditValue = ""
		Me.Cbo_Berater.Location = New System.Drawing.Point(142, 201)
		Me.Cbo_Berater.Name = "Cbo_Berater"
		Me.Cbo_Berater.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_Berater.Properties.SeparatorChar = Global.Microsoft.VisualBasic.ChrW(35)
		Me.Cbo_Berater.Size = New System.Drawing.Size(481, 20)
		Me.Cbo_Berater.TabIndex = 5
		'
		'Cbo_Month
		'
		Me.Cbo_Month.Location = New System.Drawing.Point(449, 123)
		Me.Cbo_Month.Name = "Cbo_Month"
		Me.Cbo_Month.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_Month.Size = New System.Drawing.Size(103, 20)
		Me.Cbo_Month.TabIndex = 2
		'
		'lueES
		'
		Me.lueES.Location = New System.Drawing.Point(142, 97)
		Me.lueES.Name = "lueES"
		Me.lueES.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.lueES.Properties.NullText = ""
		Me.lueES.Properties.PopupSizeable = False
		Me.lueES.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
		Me.lueES.Size = New System.Drawing.Size(410, 20)
		Me.lueES.TabIndex = 3
		'
		'XtraTabPage1
		'
		Me.XtraTabPage1.Controls.Add(Me.Label1)
		Me.XtraTabPage1.Controls.Add(Me.MemoEdit1)
		Me.XtraTabPage1.Controls.Add(Me.MemoEdit2)
		Me.XtraTabPage1.Name = "XtraTabPage1"
		Me.XtraTabPage1.Size = New System.Drawing.Size(662, 293)
		Me.XtraTabPage1.Text = "SQL-Abfrage"
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Location = New System.Drawing.Point(32, 20)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(154, 13)
		Me.Label1.TabIndex = 102
		Me.Label1.Text = "Ihre derzeitige Abfrage lautet:"
		'
		'MemoEdit1
		'
		Me.MemoEdit1.Location = New System.Drawing.Point(35, 36)
		Me.MemoEdit1.Name = "MemoEdit1"
		Me.MemoEdit1.Size = New System.Drawing.Size(599, 120)
		Me.MemoEdit1.TabIndex = 100
		Me.MemoEdit1.UseOptimizedRendering = True
		'
		'MemoEdit2
		'
		Me.MemoEdit2.Location = New System.Drawing.Point(35, 162)
		Me.MemoEdit2.Name = "MemoEdit2"
		Me.MemoEdit2.Size = New System.Drawing.Size(599, 120)
		Me.MemoEdit2.TabIndex = 101
		Me.MemoEdit2.UseOptimizedRendering = True
		'
		'BarManager1
		'
		Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar3})
		Me.BarManager1.DockControls.Add(Me.barDockControlTop)
		Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
		Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
		Me.BarManager1.DockControls.Add(Me.BarDockControl1)
		Me.BarManager1.Form = Me
		Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiInfo, Me.bbiSearch, Me.bbiClear, Me.bbiPrint, Me.BarStaticItem1})
		Me.BarManager1.MaxItemId = 10
		Me.BarManager1.StatusBar = Me.Bar3
		'
		'Bar3
		'
		Me.Bar3.BarName = "Statusleiste"
		Me.Bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
		Me.Bar3.DockCol = 0
		Me.Bar3.DockRow = 0
		Me.Bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
		Me.Bar3.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiInfo), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiSearch), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiClear), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiPrint, True), New DevExpress.XtraBars.LinkPersistInfo(Me.BarStaticItem1)})
		Me.Bar3.OptionsBar.AllowQuickCustomization = False
		Me.Bar3.OptionsBar.DrawDragBorder = False
		Me.Bar3.OptionsBar.UseWholeRow = True
		Me.Bar3.Text = "Statusleiste"
		'
		'barDockControlTop
		'
		Me.barDockControlTop.CausesValidation = False
		Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
		Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlTop.Size = New System.Drawing.Size(820, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 483)
		Me.barDockControlBottom.Size = New System.Drawing.Size(820, 27)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 483)
		'
		'BarDockControl1
		'
		Me.BarDockControl1.CausesValidation = False
		Me.BarDockControl1.Dock = System.Windows.Forms.DockStyle.Right
		Me.BarDockControl1.Location = New System.Drawing.Point(820, 0)
		Me.BarDockControl1.Size = New System.Drawing.Size(0, 483)
		'
		'frmPrintESRP
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(820, 510)
		Me.Controls.Add(Me.xtabRPSearch)
		Me.Controls.Add(Me.PanelControl1)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.BarDockControl1)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.Name = "frmPrintESRP"
		Me.Text = "Rapporte drucken"
		CType(Me.txt_SQL_1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txt_SQL_2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.xtabSQLAbfrage.ResumeLayout(False)
		Me.xtabSQLAbfrage.PerformLayout()
		CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.PanelControl1.ResumeLayout(False)
		Me.PanelControl1.PerformLayout()
		CType(Me.PictureEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.xtabRPSearch, System.ComponentModel.ISupportInitialize).EndInit()
		Me.xtabRPSearch.ResumeLayout(False)
		Me.xtabAllgemein.ResumeLayout(False)
		CType(Me.LookUpEdit2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lueWeek.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_Year.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_Berater.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_Month.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lueES.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.XtraTabPage1.ResumeLayout(False)
		Me.XtraTabPage1.PerformLayout()
		CType(Me.MemoEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.MemoEdit2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents lblAktuellQery As System.Windows.Forms.Label
	Friend WithEvents txt_SQL_1 As DevExpress.XtraEditors.MemoEdit
	Friend WithEvents txt_SQL_2 As DevExpress.XtraEditors.MemoEdit
	Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
	Friend WithEvents bsiInfo As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents bbiSearch As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbiClear As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbiPrint As DevExpress.XtraBars.BarLargeButtonItem
	Friend WithEvents BarStaticItem1 As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
	Friend WithEvents xtabSQLAbfrage As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
	Friend WithEvents PictureEdit1 As DevExpress.XtraEditors.PictureEdit
	Friend WithEvents LblTimeValue As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblHeader2 As DevExpress.XtraEditors.LabelControl
	Friend WithEvents CmdClose As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents lblHeader1 As System.Windows.Forms.Label
	Friend WithEvents xtabRPSearch As DevExpress.XtraTab.XtraTabControl
	Friend WithEvents xtabAllgemein As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents lueMandant As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents lblMDName As System.Windows.Forms.Label
	Friend WithEvents Cbo_Year As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents lblBerater As System.Windows.Forms.Label
	Friend WithEvents lblMonat As System.Windows.Forms.Label
	Friend WithEvents lbl1KST As System.Windows.Forms.Label
	Friend WithEvents lblJahr As System.Windows.Forms.Label
	Friend WithEvents Cbo_Berater As DevExpress.XtraEditors.CheckedComboBoxEdit
	Friend WithEvents Cbo_Month As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents XtraTabPage1 As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents Label1 As System.Windows.Forms.Label
	Friend WithEvents MemoEdit1 As DevExpress.XtraEditors.MemoEdit
	Friend WithEvents MemoEdit2 As DevExpress.XtraEditors.MemoEdit
	Friend WithEvents LookUpEdit2 As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents lueWeek As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents lblWoche As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lueES As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
	Friend WithEvents Bar3 As DevExpress.XtraBars.Bar
	Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
	Friend WithEvents BarDockControl1 As DevExpress.XtraBars.BarDockControl
End Class
