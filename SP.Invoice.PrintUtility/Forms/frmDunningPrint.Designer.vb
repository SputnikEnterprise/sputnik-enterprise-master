<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDunningPrint
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDunningPrint))
		Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
		Me.Bar4 = New DevExpress.XtraBars.Bar()
		Me.bsiPrintinfo = New DevExpress.XtraBars.BarStaticItem()
		Me.bbiSearch = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiPrint = New DevExpress.XtraBars.BarLargeButtonItem()
		Me.bbiDelete = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiExport = New DevExpress.XtraBars.BarButtonItem()
		Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
		Me.BarStaticItem1 = New DevExpress.XtraBars.BarStaticItem()
		Me.lueMandant = New DevExpress.XtraEditors.LookUpEdit()
		Me.lblMDName = New System.Windows.Forms.Label()
		Me.lblDetail = New System.Windows.Forms.Label()
		Me.gpSuchKriterien = New DevExpress.XtraEditors.GroupControl()
		Me.chk_PrintInvoices = New DevExpress.XtraEditors.CheckEdit()
		Me.lueDunningLevel = New DevExpress.XtraEditors.LookUpEdit()
		Me.lblMahnstufe = New DevExpress.XtraEditors.LabelControl()
		Me.lblMahnDatum = New DevExpress.XtraEditors.LabelControl()
		Me.lueDunningDate = New DevExpress.XtraEditors.LookUpEdit()
		Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.grdPrint = New DevExpress.XtraGrid.GridControl()
		Me.gvPrint = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.tgsSelection = New DevExpress.XtraEditors.ToggleSwitch()
		Me.sbClose = New DevExpress.XtraEditors.SimpleButton()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.lblHeaderNormal = New System.Windows.Forms.Label()
		Me.lblHeaderFett = New System.Windows.Forms.Label()
		Me.GroupBox1 = New DevExpress.XtraEditors.PanelControl()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gpSuchKriterien, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.gpSuchKriterien.SuspendLayout()
		CType(Me.chk_PrintInvoices.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lueDunningLevel.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lueDunningDate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.grdPrint, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvPrint, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.tgsSelection.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupBox1.SuspendLayout()
		Me.SuspendLayout()
		'
		'BarManager1
		'
		Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar4})
		Me.BarManager1.DockControls.Add(Me.barDockControlTop)
		Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
		Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
		Me.BarManager1.DockControls.Add(Me.barDockControlRight)
		Me.BarManager1.Form = Me
		Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiPrintinfo, Me.bbiSearch, Me.bbiPrint, Me.bbiDelete, Me.BarStaticItem1, Me.bbiExport})
		Me.BarManager1.MaxItemId = 13
		Me.BarManager1.StatusBar = Me.Bar4
		'
		'Bar4
		'
		Me.Bar4.BarName = "Statusleiste"
		Me.Bar4.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
		Me.Bar4.DockCol = 0
		Me.Bar4.DockRow = 0
		Me.Bar4.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
		Me.Bar4.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiPrintinfo), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiSearch), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiPrint, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiDelete, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiExport, True)})
		Me.Bar4.OptionsBar.AllowQuickCustomization = False
		Me.Bar4.OptionsBar.DrawDragBorder = False
		Me.Bar4.OptionsBar.UseWholeRow = True
		Me.Bar4.Text = "Statusleiste"
		'
		'bsiPrintinfo
		'
		Me.bsiPrintinfo.AutoSize = DevExpress.XtraBars.BarStaticItemSize.Spring
		Me.bsiPrintinfo.Caption = "Bereit"
		Me.bsiPrintinfo.Id = 0
		Me.bsiPrintinfo.Name = "bsiPrintinfo"
		Me.bsiPrintinfo.TextAlignment = System.Drawing.StringAlignment.Near
		Me.bsiPrintinfo.Width = 32
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
		Me.bbiPrint.Glyph = CType(resources.GetObject("bbiPrint.Glyph"), System.Drawing.Image)
		Me.bbiPrint.Id = 6
		Me.bbiPrint.Name = "bbiPrint"
		Me.bbiPrint.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiDelete
		'
		Me.bbiDelete.Caption = "Löschen"
		Me.bbiDelete.Glyph = CType(resources.GetObject("bbiDelete.Glyph"), System.Drawing.Image)
		Me.bbiDelete.Id = 8
		Me.bbiDelete.Name = "bbiDelete"
		Me.bbiDelete.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiExport
		'
		Me.bbiExport.Caption = "Exportieren"
		Me.bbiExport.Glyph = CType(resources.GetObject("bbiExport.Glyph"), System.Drawing.Image)
		Me.bbiExport.Id = 4
		Me.bbiExport.LargeGlyph = CType(resources.GetObject("bbiExport.LargeGlyph"), System.Drawing.Image)
		Me.bbiExport.Name = "bbiExport"
		Me.bbiExport.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'barDockControlTop
		'
		Me.barDockControlTop.CausesValidation = False
		Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
		Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlTop.Size = New System.Drawing.Size(946, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 594)
		Me.barDockControlBottom.Size = New System.Drawing.Size(946, 27)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 594)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(946, 0)
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 594)
		'
		'BarStaticItem1
		'
		Me.BarStaticItem1.Caption = " "
		Me.BarStaticItem1.Id = 5
		Me.BarStaticItem1.Name = "BarStaticItem1"
		Me.BarStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near
		'
		'lueMandant
		'
		Me.lueMandant.Location = New System.Drawing.Point(124, 40)
		Me.lueMandant.Name = "lueMandant"
		Me.lueMandant.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.lueMandant.Properties.NullText = ""
		Me.lueMandant.Size = New System.Drawing.Size(280, 20)
		Me.lueMandant.TabIndex = 280
		'
		'lblMDName
		'
		Me.lblMDName.Location = New System.Drawing.Point(1, 45)
		Me.lblMDName.Name = "lblMDName"
		Me.lblMDName.Size = New System.Drawing.Size(117, 13)
		Me.lblMDName.TabIndex = 279
		Me.lblMDName.Text = "Mandanten"
		Me.lblMDName.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblDetail
		'
		Me.lblDetail.AutoSize = True
		Me.lblDetail.BackColor = System.Drawing.Color.Transparent
		Me.lblDetail.Location = New System.Drawing.Point(13, 106)
		Me.lblDetail.Name = "lblDetail"
		Me.lblDetail.Size = New System.Drawing.Size(34, 13)
		Me.lblDetail.TabIndex = 326
		Me.lblDetail.Text = "Detail"
		Me.lblDetail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'gpSuchKriterien
		'
		Me.gpSuchKriterien.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.gpSuchKriterien.Controls.Add(Me.chk_PrintInvoices)
		Me.gpSuchKriterien.Controls.Add(Me.lueDunningLevel)
		Me.gpSuchKriterien.Controls.Add(Me.lblMahnstufe)
		Me.gpSuchKriterien.Controls.Add(Me.lblMahnDatum)
		Me.gpSuchKriterien.Controls.Add(Me.lueDunningDate)
		Me.gpSuchKriterien.Controls.Add(Me.lueMandant)
		Me.gpSuchKriterien.Controls.Add(Me.lblMDName)
		Me.gpSuchKriterien.Location = New System.Drawing.Point(482, 125)
		Me.gpSuchKriterien.Name = "gpSuchKriterien"
		Me.gpSuchKriterien.Size = New System.Drawing.Size(437, 442)
		Me.gpSuchKriterien.TabIndex = 324
		Me.gpSuchKriterien.Text = "Suchkriterien"
		'
		'chk_PrintInvoices
		'
		Me.chk_PrintInvoices.Location = New System.Drawing.Point(124, 160)
		Me.chk_PrintInvoices.Name = "chk_PrintInvoices"
		Me.chk_PrintInvoices.Properties.AllowFocused = False
		Me.chk_PrintInvoices.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.[True]
		Me.chk_PrintInvoices.Properties.Caption = "Zugehörige Rechnungen mitdrucken."
		Me.chk_PrintInvoices.Size = New System.Drawing.Size(240, 19)
		Me.chk_PrintInvoices.TabIndex = 296
		'
		'lueDunningLevel
		'
		Me.lueDunningLevel.Location = New System.Drawing.Point(124, 108)
		Me.lueDunningLevel.Name = "lueDunningLevel"
		Me.lueDunningLevel.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup
		Me.lueDunningLevel.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.lueDunningLevel.Properties.DropDownRows = 8
		Me.lueDunningLevel.Properties.NullText = ""
		Me.lueDunningLevel.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete
		Me.lueDunningLevel.Properties.ShowFooter = False
		Me.lueDunningLevel.Properties.ShowHeader = False
		Me.lueDunningLevel.Size = New System.Drawing.Size(280, 20)
		Me.lueDunningLevel.TabIndex = 293
		'
		'lblMahnstufe
		'
		Me.lblMahnstufe.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblMahnstufe.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblMahnstufe.Location = New System.Drawing.Point(9, 111)
		Me.lblMahnstufe.Name = "lblMahnstufe"
		Me.lblMahnstufe.Size = New System.Drawing.Size(109, 13)
		Me.lblMahnstufe.TabIndex = 292
		Me.lblMahnstufe.Text = "Mahnstufe"
		'
		'lblMahnDatum
		'
		Me.lblMahnDatum.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblMahnDatum.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblMahnDatum.Location = New System.Drawing.Point(9, 137)
		Me.lblMahnDatum.Name = "lblMahnDatum"
		Me.lblMahnDatum.Size = New System.Drawing.Size(109, 13)
		Me.lblMahnDatum.TabIndex = 294
		Me.lblMahnDatum.Text = "Mahndatum"
		'
		'lueDunningDate
		'
		Me.lueDunningDate.Location = New System.Drawing.Point(124, 134)
		Me.lueDunningDate.Name = "lueDunningDate"
		Me.lueDunningDate.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup
		SerializableAppearanceObject1.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject1.Options.UseForeColor = True
		Me.lueDunningDate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, "", Nothing, Nothing, True)})
		Me.lueDunningDate.Properties.DisplayFormat.FormatString = "d"
		Me.lueDunningDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		Me.lueDunningDate.Properties.DropDownRows = 8
		Me.lueDunningDate.Properties.EditFormat.FormatString = "d"
		Me.lueDunningDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		Me.lueDunningDate.Properties.NullText = ""
		Me.lueDunningDate.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete
		Me.lueDunningDate.Properties.ShowFooter = False
		Me.lueDunningDate.Properties.ShowHeader = False
		Me.lueDunningDate.Size = New System.Drawing.Size(104, 20)
		Me.lueDunningDate.TabIndex = 295
		'
		'GridView1
		'
		Me.GridView1.GridControl = Me.grdPrint
		Me.GridView1.Name = "GridView1"
		'
		'grdPrint
		'
		Me.grdPrint.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.grdPrint.Location = New System.Drawing.Point(13, 125)
		Me.grdPrint.MainView = Me.gvPrint
		Me.grdPrint.Name = "grdPrint"
		Me.grdPrint.Size = New System.Drawing.Size(451, 442)
		Me.grdPrint.TabIndex = 322
		Me.grdPrint.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvPrint, Me.GridView1})
		'
		'gvPrint
		'
		Me.gvPrint.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvPrint.GridControl = Me.grdPrint
		Me.gvPrint.Name = "gvPrint"
		Me.gvPrint.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvPrint.OptionsView.ShowAutoFilterRow = True
		Me.gvPrint.OptionsView.ShowGroupPanel = False
		'
		'tgsSelection
		'
		Me.tgsSelection.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.tgsSelection.EditValue = True
		Me.tgsSelection.Location = New System.Drawing.Point(250, 95)
		Me.tgsSelection.Name = "tgsSelection"
		Me.tgsSelection.Properties.AllowFocused = False
		Me.tgsSelection.Properties.Appearance.Options.UseTextOptions = True
		Me.tgsSelection.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.tgsSelection.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.tgsSelection.Properties.OffText = "Alles abgewählt"
		Me.tgsSelection.Properties.OnText = "Alles ausgewählt"
		Me.tgsSelection.Size = New System.Drawing.Size(214, 24)
		Me.tgsSelection.TabIndex = 325
		'
		'sbClose
		'
		Me.sbClose.Anchor = System.Windows.Forms.AnchorStyles.Right
		Me.sbClose.Location = New System.Drawing.Point(818, 21)
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
		'lblHeaderNormal
		'
		Me.lblHeaderNormal.BackColor = System.Drawing.Color.Transparent
		Me.lblHeaderNormal.Location = New System.Drawing.Point(112, 45)
		Me.lblHeaderNormal.Name = "lblHeaderNormal"
		Me.lblHeaderNormal.Size = New System.Drawing.Size(299, 16)
		Me.lblHeaderNormal.TabIndex = 1
		Me.lblHeaderNormal.Text = "Geben Sie bitte Ihre gewünschten Kriterien ein."
		'
		'lblHeaderFett
		'
		Me.lblHeaderFett.AutoSize = True
		Me.lblHeaderFett.BackColor = System.Drawing.Color.Transparent
		Me.lblHeaderFett.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblHeaderFett.Location = New System.Drawing.Point(94, 21)
		Me.lblHeaderFett.Name = "lblHeaderFett"
		Me.lblHeaderFett.Size = New System.Drawing.Size(165, 13)
		Me.lblHeaderFett.TabIndex = 0
		Me.lblHeaderFett.Text = "Verwaltung von Mahnungen"
		'
		'GroupBox1
		'
		Me.GroupBox1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003
		Me.GroupBox1.Controls.Add(Me.sbClose)
		Me.GroupBox1.Controls.Add(Me.Label2)
		Me.GroupBox1.Controls.Add(Me.lblHeaderNormal)
		Me.GroupBox1.Controls.Add(Me.lblHeaderFett)
		Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
		Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(946, 77)
		Me.GroupBox1.TabIndex = 323
		'
		'frmDunningPrint
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(946, 621)
		Me.Controls.Add(Me.lblDetail)
		Me.Controls.Add(Me.gpSuchKriterien)
		Me.Controls.Add(Me.tgsSelection)
		Me.Controls.Add(Me.grdPrint)
		Me.Controls.Add(Me.GroupBox1)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MinimumSize = New System.Drawing.Size(962, 659)
		Me.Name = "frmDunningPrint"
		Me.Text = "Drucken von Mahnungen"
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gpSuchKriterien, System.ComponentModel.ISupportInitialize).EndInit()
		Me.gpSuchKriterien.ResumeLayout(False)
		CType(Me.chk_PrintInvoices.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lueDunningLevel.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lueDunningDate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.grdPrint, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvPrint, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.tgsSelection.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupBox1.ResumeLayout(False)
		Me.GroupBox1.PerformLayout()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
	Friend WithEvents Bar4 As DevExpress.XtraBars.Bar
	Friend WithEvents bsiPrintinfo As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents bbiSearch As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbiPrint As DevExpress.XtraBars.BarLargeButtonItem
	Friend WithEvents bbiDelete As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbiExport As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
	Friend WithEvents lblDetail As System.Windows.Forms.Label
	Friend WithEvents gpSuchKriterien As DevExpress.XtraEditors.GroupControl
	Friend WithEvents lueMandant As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents lblMDName As System.Windows.Forms.Label
	Friend WithEvents tgsSelection As DevExpress.XtraEditors.ToggleSwitch
	Friend WithEvents grdPrint As DevExpress.XtraGrid.GridControl
	Private WithEvents gvPrint As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents GroupBox1 As DevExpress.XtraEditors.PanelControl
	Friend WithEvents sbClose As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents Label2 As System.Windows.Forms.Label
	Friend WithEvents lblHeaderNormal As System.Windows.Forms.Label
	Friend WithEvents lblHeaderFett As System.Windows.Forms.Label
	Friend WithEvents BarStaticItem1 As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents lueDunningLevel As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents lblMahnstufe As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblMahnDatum As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lueDunningDate As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents chk_PrintInvoices As DevExpress.XtraEditors.CheckEdit
End Class
