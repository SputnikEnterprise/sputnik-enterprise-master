Namespace UI


	<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
	Partial Class frmNewAutomatedInvoice
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
			Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmNewAutomatedInvoice))
			Me.GroupBox1 = New DevExpress.XtraEditors.PanelControl()
			Me.btnClose = New DevExpress.XtraEditors.SimpleButton()
			Me.Label2 = New System.Windows.Forms.Label()
			Me.lblHeaderNormal = New System.Windows.Forms.Label()
			Me.lblHeaderFett = New System.Windows.Forms.Label()
			Me.gpEigenschaften = New DevComponents.DotNetBar.Controls.GroupPanel()
			Me.lueBankdaten = New DevExpress.XtraEditors.LookUpEdit()
			Me.lblBankdaten = New DevExpress.XtraEditors.LabelControl()
			Me.lueDebitorenart = New DevExpress.XtraEditors.LookUpEdit()
			Me.lblDebitorenart = New DevExpress.XtraEditors.LabelControl()
			Me.lueMandant = New DevExpress.XtraEditors.LookUpEdit()
			Me.lblMandant = New DevExpress.XtraEditors.LabelControl()
			Me.grdInvoices = New DevExpress.XtraGrid.GridControl()
			Me.gvInvoices = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.ToolTipController1 = New DevExpress.Utils.ToolTipController(Me.components)
			Me.grdIndData = New DevExpress.XtraGrid.GridControl()
			Me.gvIndData = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.Bar4 = New DevExpress.XtraBars.Bar()
			Me.bsiPrintinfo = New DevExpress.XtraBars.BarStaticItem()
			Me.bbiCreate = New DevExpress.XtraBars.BarButtonItem()
			Me.bbiPrint = New DevExpress.XtraBars.BarLargeButtonItem()
			Me.bbiDelete = New DevExpress.XtraBars.BarButtonItem()
			Me.bbiExport = New DevExpress.XtraBars.BarButtonItem()
			Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
			Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
			Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
			Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
			Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
			Me.BarStaticItem1 = New DevExpress.XtraBars.BarStaticItem()
			Me.lblErstellteRechnungen = New DevExpress.XtraEditors.LabelControl()
			Me.tgsSelectionIndividuell = New DevExpress.XtraEditors.ToggleSwitch()
			Me.lblIndividuelleDaten = New DevExpress.XtraEditors.LabelControl()
			Me.sccMain = New DevExpress.XtraEditors.SplitContainerControl()
			Me.tgsSelectionInvoices = New DevExpress.XtraEditors.ToggleSwitch()
			CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.GroupBox1.SuspendLayout()
			Me.gpEigenschaften.SuspendLayout()
			CType(Me.lueBankdaten.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.lueDebitorenart.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.grdInvoices, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvInvoices, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.grdIndData, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvIndData, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.tgsSelectionIndividuell.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.sccMain, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.sccMain.SuspendLayout()
			CType(Me.tgsSelectionInvoices.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			'
			'GroupBox1
			'
			Me.GroupBox1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003
			Me.GroupBox1.Controls.Add(Me.btnClose)
			Me.GroupBox1.Controls.Add(Me.Label2)
			Me.GroupBox1.Controls.Add(Me.lblHeaderNormal)
			Me.GroupBox1.Controls.Add(Me.lblHeaderFett)
			Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
			Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
			Me.GroupBox1.Name = "GroupBox1"
			Me.GroupBox1.Size = New System.Drawing.Size(1262, 77)
			Me.GroupBox1.TabIndex = 208
			'
			'btnClose
			'
			Me.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Right
			Me.btnClose.Location = New System.Drawing.Point(1150, 21)
			Me.btnClose.Name = "btnClose"
			Me.btnClose.Size = New System.Drawing.Size(100, 25)
			Me.btnClose.TabIndex = 204
			Me.btnClose.Text = "Schliessen"
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
			Me.lblHeaderFett.Size = New System.Drawing.Size(171, 13)
			Me.lblHeaderFett.TabIndex = 0
			Me.lblHeaderFett.Text = "Verwaltung von Rechnungen"
			'
			'gpEigenschaften
			'
			Me.gpEigenschaften.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
							Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.gpEigenschaften.BackColor = System.Drawing.Color.Transparent
			Me.gpEigenschaften.CanvasColor = System.Drawing.SystemColors.Control
			Me.gpEigenschaften.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
			Me.gpEigenschaften.Controls.Add(Me.lueBankdaten)
			Me.gpEigenschaften.Controls.Add(Me.lblBankdaten)
			Me.gpEigenschaften.Controls.Add(Me.lueDebitorenart)
			Me.gpEigenschaften.Controls.Add(Me.lblDebitorenart)
			Me.gpEigenschaften.Controls.Add(Me.lueMandant)
			Me.gpEigenschaften.Controls.Add(Me.lblMandant)
			Me.gpEigenschaften.Location = New System.Drawing.Point(5, 23)
			Me.gpEigenschaften.Name = "gpEigenschaften"
			Me.gpEigenschaften.Size = New System.Drawing.Size(565, 155)
			'
			'
			'
			Me.gpEigenschaften.Style.BackColorGradientAngle = 90
			Me.gpEigenschaften.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.gpEigenschaften.Style.BorderBottomWidth = 1
			Me.gpEigenschaften.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
			Me.gpEigenschaften.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.gpEigenschaften.Style.BorderLeftWidth = 1
			Me.gpEigenschaften.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.gpEigenschaften.Style.BorderRightWidth = 1
			Me.gpEigenschaften.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.gpEigenschaften.Style.BorderTopWidth = 1
			Me.gpEigenschaften.Style.CornerDiameter = 4
			Me.gpEigenschaften.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
			Me.gpEigenschaften.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.gpEigenschaften.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
			Me.gpEigenschaften.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
			Me.gpEigenschaften.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
			'
			'
			'
			Me.gpEigenschaften.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
			'
			'
			'
			Me.gpEigenschaften.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
			Me.gpEigenschaften.TabIndex = 289
			Me.gpEigenschaften.Text = "Eigenschaften und Merkmale"
			'
			'lueBankdaten
			'
			Me.lueBankdaten.Location = New System.Drawing.Point(117, 36)
			Me.lueBankdaten.Name = "lueBankdaten"
			Me.lueBankdaten.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup
			Me.lueBankdaten.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.lueBankdaten.Properties.DropDownRows = 10
			Me.lueBankdaten.Properties.NullText = ""
			Me.lueBankdaten.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete
			Me.lueBankdaten.Properties.ShowFooter = False
			Me.lueBankdaten.Properties.ShowHeader = False
			Me.lueBankdaten.Size = New System.Drawing.Size(309, 20)
			Me.lueBankdaten.TabIndex = 260
			'
			'lblBankdaten
			'
			Me.lblBankdaten.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblBankdaten.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblBankdaten.Location = New System.Drawing.Point(33, 39)
			Me.lblBankdaten.Name = "lblBankdaten"
			Me.lblBankdaten.Size = New System.Drawing.Size(78, 13)
			Me.lblBankdaten.TabIndex = 259
			Me.lblBankdaten.Text = "Bankdaten"
			'
			'lueDebitorenart
			'
			Me.lueDebitorenart.Location = New System.Drawing.Point(117, 79)
			Me.lueDebitorenart.Name = "lueDebitorenart"
			Me.lueDebitorenart.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup
			Me.lueDebitorenart.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.lueDebitorenart.Properties.DropDownRows = 8
			Me.lueDebitorenart.Properties.NullText = ""
			Me.lueDebitorenart.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete
			Me.lueDebitorenart.Properties.ShowFooter = False
			Me.lueDebitorenart.Properties.ShowHeader = False
			Me.lueDebitorenart.Size = New System.Drawing.Size(309, 20)
			Me.lueDebitorenart.TabIndex = 258
			'
			'lblDebitorenart
			'
			Me.lblDebitorenart.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblDebitorenart.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblDebitorenart.Location = New System.Drawing.Point(33, 82)
			Me.lblDebitorenart.Name = "lblDebitorenart"
			Me.lblDebitorenart.Size = New System.Drawing.Size(78, 13)
			Me.lblDebitorenart.TabIndex = 257
			Me.lblDebitorenart.Text = "Debitorenart"
			'
			'lueMandant
			'
			Me.lueMandant.Location = New System.Drawing.Point(117, 10)
			Me.lueMandant.Name = "lueMandant"
			Me.lueMandant.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup
			Me.lueMandant.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.lueMandant.Properties.DropDownRows = 10
			Me.lueMandant.Properties.NullText = ""
			Me.lueMandant.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete
			Me.lueMandant.Properties.ShowFooter = False
			Me.lueMandant.Properties.ShowHeader = False
			Me.lueMandant.Size = New System.Drawing.Size(309, 20)
			Me.lueMandant.TabIndex = 256
			'
			'lblMandant
			'
			Me.lblMandant.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblMandant.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblMandant.Location = New System.Drawing.Point(3, 13)
			Me.lblMandant.Name = "lblMandant"
			Me.lblMandant.Size = New System.Drawing.Size(108, 13)
			Me.lblMandant.TabIndex = 244
			Me.lblMandant.Text = "Mandant"
			'
			'grdInvoices
			'
			Me.grdInvoices.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
							Or System.Windows.Forms.AnchorStyles.Left) _
							Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.grdInvoices.Cursor = System.Windows.Forms.Cursors.Default
			Me.grdInvoices.Location = New System.Drawing.Point(8, 45)
			Me.grdInvoices.MainView = Me.gvInvoices
			Me.grdInvoices.Name = "grdInvoices"
			Me.grdInvoices.Size = New System.Drawing.Size(666, 493)
			Me.grdInvoices.TabIndex = 214
			Me.grdInvoices.ToolTipController = Me.ToolTipController1
			Me.grdInvoices.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvInvoices})
			'
			'gvInvoices
			'
			Me.gvInvoices.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			Me.gvInvoices.GridControl = Me.grdInvoices
			Me.gvInvoices.Name = "gvInvoices"
			Me.gvInvoices.OptionsBehavior.Editable = False
			Me.gvInvoices.OptionsSelection.EnableAppearanceFocusedCell = False
			Me.gvInvoices.OptionsView.ShowGroupPanel = False
			'
			'ToolTipController1
			'
			Me.ToolTipController1.AllowHtmlText = True
			'
			'grdIndData
			'
			Me.grdIndData.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
							Or System.Windows.Forms.AnchorStyles.Left) _
							Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.grdIndData.Cursor = System.Windows.Forms.Cursors.Default
			Me.grdIndData.Location = New System.Drawing.Point(5, 221)
			Me.grdIndData.MainView = Me.gvIndData
			Me.grdIndData.Name = "grdIndData"
			Me.grdIndData.Size = New System.Drawing.Size(565, 317)
			Me.grdIndData.TabIndex = 215
			Me.grdIndData.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvIndData})
			'
			'gvIndData
			'
			Me.gvIndData.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			Me.gvIndData.GridControl = Me.grdIndData
			Me.gvIndData.Name = "gvIndData"
			Me.gvIndData.OptionsBehavior.Editable = False
			Me.gvIndData.OptionsSelection.EnableAppearanceFocusedCell = False
			Me.gvIndData.OptionsView.ShowGroupPanel = False
			'
			'Bar4
			'
			Me.Bar4.BarName = "Statusleiste"
			Me.Bar4.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
			Me.Bar4.DockCol = 0
			Me.Bar4.DockRow = 0
			Me.Bar4.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
			Me.Bar4.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiPrintinfo), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiCreate), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiPrint, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiDelete, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiExport, True)})
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
			'bbiCreate
			'
			Me.bbiCreate.Caption = "Erstellen"
			Me.bbiCreate.Glyph = CType(resources.GetObject("bbiCreate.Glyph"), System.Drawing.Image)
			Me.bbiCreate.Id = 1
			Me.bbiCreate.LargeGlyph = CType(resources.GetObject("bbiCreate.LargeGlyph"), System.Drawing.Image)
			Me.bbiCreate.Name = "bbiCreate"
			Me.bbiCreate.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
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
			Me.bbiDelete.Caption = "Rückgängig"
			Me.bbiDelete.Description = "Erstellte Rechnungen löschen"
			Me.bbiDelete.Glyph = CType(resources.GetObject("bbiDelete.Glyph"), System.Drawing.Image)
			Me.bbiDelete.Id = 8
			Me.bbiDelete.LargeGlyph = CType(resources.GetObject("bbiDelete.LargeGlyph"), System.Drawing.Image)
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
			'BarManager1
			'
			Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar4})
			Me.BarManager1.DockControls.Add(Me.barDockControlTop)
			Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
			Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
			Me.BarManager1.DockControls.Add(Me.barDockControlRight)
			Me.BarManager1.Form = Me
			Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiPrintinfo, Me.bbiCreate, Me.bbiPrint, Me.bbiDelete, Me.BarStaticItem1, Me.bbiExport})
			Me.BarManager1.MaxItemId = 13
			Me.BarManager1.StatusBar = Me.Bar4
			'
			'barDockControlTop
			'
			Me.barDockControlTop.CausesValidation = False
			Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
			Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
			Me.barDockControlTop.Size = New System.Drawing.Size(1262, 0)
			'
			'barDockControlBottom
			'
			Me.barDockControlBottom.CausesValidation = False
			Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
			Me.barDockControlBottom.Location = New System.Drawing.Point(0, 638)
			Me.barDockControlBottom.Size = New System.Drawing.Size(1262, 27)
			'
			'barDockControlLeft
			'
			Me.barDockControlLeft.CausesValidation = False
			Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
			Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
			Me.barDockControlLeft.Size = New System.Drawing.Size(0, 638)
			'
			'barDockControlRight
			'
			Me.barDockControlRight.CausesValidation = False
			Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
			Me.barDockControlRight.Location = New System.Drawing.Point(1262, 0)
			Me.barDockControlRight.Size = New System.Drawing.Size(0, 638)
			'
			'BarStaticItem1
			'
			Me.BarStaticItem1.Caption = " "
			Me.BarStaticItem1.Id = 5
			Me.BarStaticItem1.Name = "BarStaticItem1"
			Me.BarStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near
			'
			'lblErstellteRechnungen
			'
			Me.lblErstellteRechnungen.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblErstellteRechnungen.Location = New System.Drawing.Point(8, 26)
			Me.lblErstellteRechnungen.Name = "lblErstellteRechnungen"
			Me.lblErstellteRechnungen.Size = New System.Drawing.Size(231, 13)
			Me.lblErstellteRechnungen.TabIndex = 295
			Me.lblErstellteRechnungen.Text = "Erstellte Rechnungen"
			'
			'tgsSelectionIndividuell
			'
			Me.tgsSelectionIndividuell.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.tgsSelectionIndividuell.Location = New System.Drawing.Point(356, 191)
			Me.tgsSelectionIndividuell.Name = "tgsSelectionIndividuell"
			Me.tgsSelectionIndividuell.Properties.AllowFocused = False
			Me.tgsSelectionIndividuell.Properties.Appearance.Options.UseTextOptions = True
			Me.tgsSelectionIndividuell.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.tgsSelectionIndividuell.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.tgsSelectionIndividuell.Properties.OffText = "Alles abgewählt"
			Me.tgsSelectionIndividuell.Properties.OnText = "Alles ausgewählt"
			Me.tgsSelectionIndividuell.Size = New System.Drawing.Size(214, 24)
			Me.tgsSelectionIndividuell.TabIndex = 321
			'
			'lblIndividuelleDaten
			'
			Me.lblIndividuelleDaten.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblIndividuelleDaten.Location = New System.Drawing.Point(5, 202)
			Me.lblIndividuelleDaten.Name = "lblIndividuelleDaten"
			Me.lblIndividuelleDaten.Size = New System.Drawing.Size(231, 13)
			Me.lblIndividuelleDaten.TabIndex = 322
			Me.lblIndividuelleDaten.Text = "Individuelle Daten"
			'
			'sccMain
			'
			Me.sccMain.Dock = System.Windows.Forms.DockStyle.Fill
			Me.sccMain.Location = New System.Drawing.Point(0, 77)
			Me.sccMain.Name = "sccMain"
			Me.sccMain.Panel1.Controls.Add(Me.tgsSelectionInvoices)
			Me.sccMain.Panel1.Controls.Add(Me.lblErstellteRechnungen)
			Me.sccMain.Panel1.Controls.Add(Me.grdInvoices)
			Me.sccMain.Panel1.Padding = New System.Windows.Forms.Padding(5)
			Me.sccMain.Panel1.Text = "Panel1"
			Me.sccMain.Panel2.Controls.Add(Me.gpEigenschaften)
			Me.sccMain.Panel2.Controls.Add(Me.grdIndData)
			Me.sccMain.Panel2.Controls.Add(Me.lblIndividuelleDaten)
			Me.sccMain.Panel2.Controls.Add(Me.tgsSelectionIndividuell)
			Me.sccMain.Panel2.Padding = New System.Windows.Forms.Padding(10)
			Me.sccMain.Panel2.Text = "Panel2"
			Me.sccMain.Size = New System.Drawing.Size(1262, 561)
			Me.sccMain.SplitterPosition = 675
			Me.sccMain.TabIndex = 327
			Me.sccMain.Text = "SplitContainerControl1"
			'
			'tgsSelectionInvoices
			'
			Me.tgsSelectionInvoices.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.tgsSelectionInvoices.EditValue = True
			Me.tgsSelectionInvoices.Location = New System.Drawing.Point(460, 15)
			Me.tgsSelectionInvoices.Name = "tgsSelectionInvoices"
			Me.tgsSelectionInvoices.Properties.AllowFocused = False
			Me.tgsSelectionInvoices.Properties.Appearance.Options.UseTextOptions = True
			Me.tgsSelectionInvoices.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.tgsSelectionInvoices.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.tgsSelectionInvoices.Properties.OffText = "Alles abgewählt"
			Me.tgsSelectionInvoices.Properties.OnText = "Alles ausgewählt"
			Me.tgsSelectionInvoices.Size = New System.Drawing.Size(214, 24)
			Me.tgsSelectionInvoices.TabIndex = 322
			'
			'frmNewAutomatedInvoice
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(1262, 665)
			Me.Controls.Add(Me.sccMain)
			Me.Controls.Add(Me.GroupBox1)
			Me.Controls.Add(Me.barDockControlLeft)
			Me.Controls.Add(Me.barDockControlRight)
			Me.Controls.Add(Me.barDockControlBottom)
			Me.Controls.Add(Me.barDockControlTop)
			Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
			Me.MinimumSize = New System.Drawing.Size(1278, 703)
			Me.Name = "frmNewAutomatedInvoice"
			Me.Text = "Neue Rechnungen erstellen"
			CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.GroupBox1.ResumeLayout(False)
			Me.GroupBox1.PerformLayout()
			Me.gpEigenschaften.ResumeLayout(False)
			CType(Me.lueBankdaten.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.lueDebitorenart.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.grdInvoices, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvInvoices, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.grdIndData, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvIndData, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.tgsSelectionIndividuell.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.sccMain, System.ComponentModel.ISupportInitialize).EndInit()
			Me.sccMain.ResumeLayout(False)
			CType(Me.tgsSelectionInvoices.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub
		Friend WithEvents GroupBox1 As DevExpress.XtraEditors.PanelControl
		Friend WithEvents btnClose As DevExpress.XtraEditors.SimpleButton
		Friend WithEvents Label2 As System.Windows.Forms.Label
		Friend WithEvents lblHeaderNormal As System.Windows.Forms.Label
		Friend WithEvents lblHeaderFett As System.Windows.Forms.Label
		Friend WithEvents gpEigenschaften As DevComponents.DotNetBar.Controls.GroupPanel
		Friend WithEvents lueMandant As DevExpress.XtraEditors.LookUpEdit
		Friend WithEvents lblMandant As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lueDebitorenart As DevExpress.XtraEditors.LookUpEdit
		Friend WithEvents lblDebitorenart As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lueBankdaten As DevExpress.XtraEditors.LookUpEdit
		Friend WithEvents lblBankdaten As DevExpress.XtraEditors.LabelControl
		Friend WithEvents grdInvoices As DevExpress.XtraGrid.GridControl
		Friend WithEvents gvInvoices As DevExpress.XtraGrid.Views.Grid.GridView
		Friend WithEvents grdIndData As DevExpress.XtraGrid.GridControl
		Friend WithEvents gvIndData As DevExpress.XtraGrid.Views.Grid.GridView
		Friend WithEvents Bar4 As DevExpress.XtraBars.Bar
		Friend WithEvents bsiPrintinfo As DevExpress.XtraBars.BarStaticItem
		Friend WithEvents bbiCreate As DevExpress.XtraBars.BarButtonItem
		Friend WithEvents bbiPrint As DevExpress.XtraBars.BarLargeButtonItem
		Friend WithEvents bbiDelete As DevExpress.XtraBars.BarButtonItem
		Friend WithEvents bbiExport As DevExpress.XtraBars.BarButtonItem
		Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
		Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
		Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
		Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
		Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
		Friend WithEvents lblErstellteRechnungen As DevExpress.XtraEditors.LabelControl
		Friend WithEvents BarStaticItem1 As DevExpress.XtraBars.BarStaticItem
		Friend WithEvents lblIndividuelleDaten As DevExpress.XtraEditors.LabelControl
		Friend WithEvents tgsSelectionIndividuell As DevExpress.XtraEditors.ToggleSwitch
		Friend WithEvents sccMain As DevExpress.XtraEditors.SplitContainerControl
		Friend WithEvents tgsSelectionInvoices As DevExpress.XtraEditors.ToggleSwitch
		Friend WithEvents ToolTipController1 As DevExpress.Utils.ToolTipController
	End Class

End Namespace
