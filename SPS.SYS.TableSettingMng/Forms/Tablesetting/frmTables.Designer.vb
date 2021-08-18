<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTables
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
		Dim GridLevelNode1 As DevExpress.XtraGrid.GridLevelNode = New DevExpress.XtraGrid.GridLevelNode()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTables))
		Dim EditorButtonImageOptions1 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
		Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject4 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Me.grdTableContent = New DevExpress.XtraGrid.GridControl()
		Me.gvTableContent = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.lblTableCaption = New DevExpress.XtraEditors.LabelControl()
		Me.pnlHeader = New DevExpress.XtraEditors.PanelControl()
		Me.lblHeaderimg = New DevExpress.XtraEditors.LabelControl()
		Me.btnClose = New DevExpress.XtraEditors.SimpleButton()
		Me.lblHeaderDescription = New DevExpress.XtraEditors.LabelControl()
		Me.lblHeader = New DevExpress.XtraEditors.LabelControl()
		Me.sccMain = New DevExpress.XtraEditors.SplitContainerControl()
		Me.grpFilter = New DevExpress.XtraEditors.GroupControl()
		Me.lueMandant = New DevExpress.XtraEditors.LookUpEdit()
		Me.lblMDName = New System.Windows.Forms.Label()
		Me.lblJahr = New System.Windows.Forms.Label()
		Me.lueYear = New DevExpress.XtraEditors.LookUpEdit()
		Me.navMain = New DevExpress.XtraNavBar.NavBarControl()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager()
		Me.Bar3 = New DevExpress.XtraBars.Bar()
		Me.bsiLblRecCount = New DevExpress.XtraBars.BarStaticItem()
		Me.bsiRecCount = New DevExpress.XtraBars.BarStaticItem()
		Me.BarStaticItem3 = New DevExpress.XtraBars.BarStaticItem()
		Me.BarDockControl5 = New DevExpress.XtraBars.BarDockControl()
		Me.BarDockControl6 = New DevExpress.XtraBars.BarDockControl()
		Me.BarDockControl7 = New DevExpress.XtraBars.BarDockControl()
		Me.BarDockControl8 = New DevExpress.XtraBars.BarDockControl()
		Me.BarSubItem3 = New DevExpress.XtraBars.BarSubItem()
		Me.RepositoryItemFontEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemFontEdit()
		Me.RepositoryItemComboBox1 = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
		Me.RepositoryItemComboBox2 = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
		Me.Bar1 = New DevExpress.XtraBars.Bar()
		Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
		Me.btnDelete = New DevExpress.XtraEditors.SimpleButton()
		CType(Me.grdTableContent, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvTableContent, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.pnlHeader, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.pnlHeader.SuspendLayout()
		CType(Me.sccMain, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.sccMain.SuspendLayout()
		CType(Me.grpFilter, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grpFilter.SuspendLayout()
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lueYear.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.navMain, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.RepositoryItemFontEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.RepositoryItemComboBox1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.RepositoryItemComboBox2, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.PanelControl1.SuspendLayout()
		Me.SuspendLayout()
		'
		'grdTableContent
		'
		Me.grdTableContent.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.grdTableContent.Cursor = System.Windows.Forms.Cursors.Default
		Me.grdTableContent.EmbeddedNavigator.Buttons.First.Visible = False
		Me.grdTableContent.EmbeddedNavigator.Buttons.Last.Visible = False
		Me.grdTableContent.EmbeddedNavigator.Buttons.Next.Visible = False
		Me.grdTableContent.EmbeddedNavigator.Buttons.NextPage.Visible = False
		Me.grdTableContent.EmbeddedNavigator.Buttons.Prev.Visible = False
		Me.grdTableContent.EmbeddedNavigator.Buttons.PrevPage.Visible = False
		Me.grdTableContent.EmbeddedNavigator.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
		GridLevelNode1.RelationName = "Level1"
		Me.grdTableContent.LevelTree.Nodes.AddRange(New DevExpress.XtraGrid.GridLevelNode() {GridLevelNode1})
		Me.grdTableContent.Location = New System.Drawing.Point(413, 170)
		Me.grdTableContent.MainView = Me.gvTableContent
		Me.grdTableContent.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
		Me.grdTableContent.Name = "grdTableContent"
		Me.grdTableContent.Size = New System.Drawing.Size(1192, 846)
		Me.grdTableContent.TabIndex = 303
		Me.grdTableContent.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvTableContent})
		'
		'gvTableContent
		'
		Me.gvTableContent.DetailHeight = 619
		Me.gvTableContent.FixedLineWidth = 4
		Me.gvTableContent.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvTableContent.GridControl = Me.grdTableContent
		Me.gvTableContent.Name = "gvTableContent"
		Me.gvTableContent.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.[True]
		Me.gvTableContent.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.[True]
		Me.gvTableContent.OptionsBehavior.AutoSelectAllInEditor = False
		Me.gvTableContent.OptionsBehavior.EditingMode = DevExpress.XtraGrid.Views.Grid.GridEditingMode.EditFormInplace
		Me.gvTableContent.OptionsPrint.ExpandAllDetails = True
		Me.gvTableContent.OptionsPrint.PrintDetails = True
		Me.gvTableContent.OptionsPrint.PrintPreview = True
		Me.gvTableContent.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top
		Me.gvTableContent.OptionsView.RowAutoHeight = True
		Me.gvTableContent.OptionsView.ShowGroupPanel = False
		'
		'lblTableCaption
		'
		Me.lblTableCaption.AllowHtmlString = True
		Me.lblTableCaption.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.lblTableCaption.Appearance.Options.UseFont = True
		Me.lblTableCaption.Location = New System.Drawing.Point(417, 136)
		Me.lblTableCaption.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
		Me.lblTableCaption.Name = "lblTableCaption"
		Me.lblTableCaption.Size = New System.Drawing.Size(316, 23)
		Me.lblTableCaption.TabIndex = 302
		Me.lblTableCaption.Text = "Bitte wählen Sie eine Tabelle aus"
		'
		'pnlHeader
		'
		Me.pnlHeader.Controls.Add(Me.lblHeaderimg)
		Me.pnlHeader.Controls.Add(Me.btnClose)
		Me.pnlHeader.Controls.Add(Me.lblHeaderDescription)
		Me.pnlHeader.Controls.Add(Me.lblHeader)
		Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
		Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
		Me.pnlHeader.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
		Me.pnlHeader.Name = "pnlHeader"
		Me.pnlHeader.Size = New System.Drawing.Size(1612, 126)
		Me.pnlHeader.TabIndex = 301
		'
		'lblHeaderimg
		'
		Me.lblHeaderimg.Appearance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblHeaderimg.Appearance.ForeColor = System.Drawing.SystemColors.HotTrack
		Me.lblHeaderimg.Appearance.Image = CType(resources.GetObject("lblHeaderimg.Appearance.Image"), System.Drawing.Image)
		Me.lblHeaderimg.Appearance.Options.UseFont = True
		Me.lblHeaderimg.Appearance.Options.UseForeColor = True
		Me.lblHeaderimg.Appearance.Options.UseImage = True
		Me.lblHeaderimg.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblHeaderimg.Location = New System.Drawing.Point(20, 14)
		Me.lblHeaderimg.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
		Me.lblHeaderimg.Name = "lblHeaderimg"
		Me.lblHeaderimg.Size = New System.Drawing.Size(75, 67)
		Me.lblHeaderimg.TabIndex = 1001
		'
		'btnClose
		'
		Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnClose.Location = New System.Drawing.Point(1447, 21)
		Me.btnClose.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
		Me.btnClose.Name = "btnClose"
		Me.btnClose.Size = New System.Drawing.Size(145, 41)
		Me.btnClose.TabIndex = 2
		Me.btnClose.Text = "Schliessen"
		'
		'lblHeaderDescription
		'
		Me.lblHeaderDescription.Location = New System.Drawing.Point(133, 58)
		Me.lblHeaderDescription.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
		Me.lblHeaderDescription.Name = "lblHeaderDescription"
		Me.lblHeaderDescription.Size = New System.Drawing.Size(371, 23)
		Me.lblHeaderDescription.TabIndex = 1
		Me.lblHeaderDescription.Text = "Wählen Sie Ihre gewünschte Kriterien aus..."
		'
		'lblHeader
		'
		Me.lblHeader.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblHeader.Appearance.Options.UseFont = True
		Me.lblHeader.Location = New System.Drawing.Point(105, 21)
		Me.lblHeader.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
		Me.lblHeader.Name = "lblHeader"
		Me.lblHeader.Size = New System.Drawing.Size(292, 23)
		Me.lblHeader.TabIndex = 0
		Me.lblHeader.Text = "Angaben für Auswahl-Tabellen"
		'
		'sccMain
		'
		Me.sccMain.Dock = System.Windows.Forms.DockStyle.Left
		Me.sccMain.Horizontal = False
		Me.sccMain.Location = New System.Drawing.Point(0, 126)
		Me.sccMain.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
		Me.sccMain.Name = "sccMain"
		Me.sccMain.Panel1.Controls.Add(Me.grpFilter)
		Me.sccMain.Panel1.Controls.Add(Me.navMain)
		Me.sccMain.Panel1.Padding = New System.Windows.Forms.Padding(8, 9, 8, 9)
		Me.sccMain.Panel1.Text = "Panel1"
		Me.sccMain.Panel2.Padding = New System.Windows.Forms.Padding(8, 9, 8, 9)
		Me.sccMain.Panel2.Text = "Panel2"
		Me.sccMain.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel1
		Me.sccMain.Size = New System.Drawing.Size(412, 938)
		Me.sccMain.SplitterPosition = 920
		Me.sccMain.TabIndex = 306
		Me.sccMain.Text = "SplitContainerControl1"
		'
		'grpFilter
		'
		Me.grpFilter.Controls.Add(Me.lueMandant)
		Me.grpFilter.Controls.Add(Me.lblMDName)
		Me.grpFilter.Controls.Add(Me.lblJahr)
		Me.grpFilter.Controls.Add(Me.lueYear)
		Me.grpFilter.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.grpFilter.Location = New System.Drawing.Point(8, 745)
		Me.grpFilter.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
		Me.grpFilter.Name = "grpFilter"
		Me.grpFilter.Size = New System.Drawing.Size(396, 184)
		Me.grpFilter.TabIndex = 22
		Me.grpFilter.Text = "Filter"
		'
		'lueMandant
		'
		Me.lueMandant.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.lueMandant.Location = New System.Drawing.Point(125, 60)
		Me.lueMandant.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
		Me.lueMandant.Name = "lueMandant"
		Me.lueMandant.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.lueMandant.Properties.NullText = ""
		Me.lueMandant.Size = New System.Drawing.Size(246, 36)
		Me.lueMandant.TabIndex = 324
		'
		'lblMDName
		'
		Me.lblMDName.Location = New System.Drawing.Point(8, 65)
		Me.lblMDName.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
		Me.lblMDName.Name = "lblMDName"
		Me.lblMDName.Size = New System.Drawing.Size(107, 23)
		Me.lblMDName.TabIndex = 325
		Me.lblMDName.Text = "Mandanten"
		Me.lblMDName.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblJahr
		'
		Me.lblJahr.Location = New System.Drawing.Point(25, 111)
		Me.lblJahr.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
		Me.lblJahr.Name = "lblJahr"
		Me.lblJahr.Size = New System.Drawing.Size(90, 23)
		Me.lblJahr.TabIndex = 322
		Me.lblJahr.Text = "Jahr"
		Me.lblJahr.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lueYear
		'
		Me.lueYear.Location = New System.Drawing.Point(125, 106)
		Me.lueYear.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
		Me.lueYear.Name = "lueYear"
		SerializableAppearanceObject1.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject1.Options.UseForeColor = True
		SerializableAppearanceObject2.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject2.Options.UseForeColor = True
		SerializableAppearanceObject3.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject3.Options.UseForeColor = True
		SerializableAppearanceObject4.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject4.Options.UseForeColor = True
		Me.lueYear.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
		Me.lueYear.Properties.NullText = ""
		Me.lueYear.Size = New System.Drawing.Size(127, 36)
		Me.lueYear.TabIndex = 323
		'
		'navMain
		'
		Me.navMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.navMain.Location = New System.Drawing.Point(8, 9)
		Me.navMain.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
		Me.navMain.Name = "navMain"
		Me.navMain.OptionsNavPane.ExpandedWidth = 398
		Me.navMain.Size = New System.Drawing.Size(398, 726)
		Me.navMain.TabIndex = 20
		Me.navMain.Text = "NavBarControl1"
		'
		'BarManager1
		'
		Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar3})
		Me.BarManager1.DockControls.Add(Me.BarDockControl5)
		Me.BarManager1.DockControls.Add(Me.BarDockControl6)
		Me.BarManager1.DockControls.Add(Me.BarDockControl7)
		Me.BarManager1.DockControls.Add(Me.BarDockControl8)
		Me.BarManager1.Form = Me
		Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.BarSubItem3, Me.bsiRecCount, Me.BarStaticItem3, Me.bsiLblRecCount})
		Me.BarManager1.MaxItemId = 65
		Me.BarManager1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemFontEdit1, Me.RepositoryItemComboBox1, Me.RepositoryItemComboBox2})
		Me.BarManager1.StatusBar = Me.Bar3
		'
		'Bar3
		'
		Me.Bar3.BarName = "Benutzerdefiniert 2"
		Me.Bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
		Me.Bar3.DockCol = 0
		Me.Bar3.DockRow = 0
		Me.Bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
		Me.Bar3.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiLblRecCount, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bsiRecCount), New DevExpress.XtraBars.LinkPersistInfo(Me.BarStaticItem3)})
		Me.Bar3.OptionsBar.AllowQuickCustomization = False
		Me.Bar3.OptionsBar.DrawDragBorder = False
		Me.Bar3.OptionsBar.UseWholeRow = True
		Me.Bar3.Text = "Benutzerdefiniert 2"
		'
		'bsiLblRecCount
		'
		Me.bsiLblRecCount.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
		Me.bsiLblRecCount.Caption = "Anzahl Datensätze:"
		Me.bsiLblRecCount.Id = 63
		Me.bsiLblRecCount.ItemAppearance.Normal.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.bsiLblRecCount.ItemAppearance.Normal.Options.UseFont = True
		Me.bsiLblRecCount.Name = "bsiLblRecCount"
		'
		'bsiRecCount
		'
		Me.bsiRecCount.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
		Me.bsiRecCount.Caption = "{0}"
		Me.bsiRecCount.Id = 60
		Me.bsiRecCount.Name = "bsiRecCount"
		'
		'BarStaticItem3
		'
		Me.BarStaticItem3.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
		Me.BarStaticItem3.Id = 62
		Me.BarStaticItem3.Name = "BarStaticItem3"
		'
		'BarDockControl5
		'
		Me.BarDockControl5.CausesValidation = False
		Me.BarDockControl5.Dock = System.Windows.Forms.DockStyle.Top
		Me.BarDockControl5.Location = New System.Drawing.Point(0, 0)
		Me.BarDockControl5.Manager = Me.BarManager1
		Me.BarDockControl5.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
		Me.BarDockControl5.Size = New System.Drawing.Size(1612, 0)
		'
		'BarDockControl6
		'
		Me.BarDockControl6.CausesValidation = False
		Me.BarDockControl6.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.BarDockControl6.Location = New System.Drawing.Point(0, 1064)
		Me.BarDockControl6.Manager = Me.BarManager1
		Me.BarDockControl6.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
		Me.BarDockControl6.Size = New System.Drawing.Size(1612, 47)
		'
		'BarDockControl7
		'
		Me.BarDockControl7.CausesValidation = False
		Me.BarDockControl7.Dock = System.Windows.Forms.DockStyle.Left
		Me.BarDockControl7.Location = New System.Drawing.Point(0, 0)
		Me.BarDockControl7.Manager = Me.BarManager1
		Me.BarDockControl7.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
		Me.BarDockControl7.Size = New System.Drawing.Size(0, 1064)
		'
		'BarDockControl8
		'
		Me.BarDockControl8.CausesValidation = False
		Me.BarDockControl8.Dock = System.Windows.Forms.DockStyle.Right
		Me.BarDockControl8.Location = New System.Drawing.Point(1612, 0)
		Me.BarDockControl8.Manager = Me.BarManager1
		Me.BarDockControl8.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
		Me.BarDockControl8.Size = New System.Drawing.Size(0, 1064)
		'
		'BarSubItem3
		'
		Me.BarSubItem3.Caption = "Vorlage laden"
		Me.BarSubItem3.Id = 59
		Me.BarSubItem3.Name = "BarSubItem3"
		Me.BarSubItem3.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		Me.BarSubItem3.ShowMenuCaption = True
		'
		'RepositoryItemFontEdit1
		'
		Me.RepositoryItemFontEdit1.AutoHeight = False
		Me.RepositoryItemFontEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.RepositoryItemFontEdit1.Name = "RepositoryItemFontEdit1"
		'
		'RepositoryItemComboBox1
		'
		Me.RepositoryItemComboBox1.AutoHeight = False
		Me.RepositoryItemComboBox1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.RepositoryItemComboBox1.Name = "RepositoryItemComboBox1"
		'
		'RepositoryItemComboBox2
		'
		Me.RepositoryItemComboBox2.AutoHeight = False
		Me.RepositoryItemComboBox2.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.RepositoryItemComboBox2.Name = "RepositoryItemComboBox2"
		'
		'Bar1
		'
		Me.Bar1.BarName = "Befehle"
		Me.Bar1.DockCol = 0
		Me.Bar1.DockRow = 0
		Me.Bar1.FloatLocation = New System.Drawing.Point(313, 297)
		Me.Bar1.Text = "Befehle"
		'
		'PanelControl1
		'
		Me.PanelControl1.Controls.Add(Me.btnDelete)
		Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.PanelControl1.Location = New System.Drawing.Point(412, 1018)
		Me.PanelControl1.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
		Me.PanelControl1.Name = "PanelControl1"
		Me.PanelControl1.Size = New System.Drawing.Size(1200, 46)
		Me.PanelControl1.TabIndex = 313
		'
		'btnDelete
		'
		Me.btnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnDelete.ImageOptions.Image = CType(resources.GetObject("btnDelete.ImageOptions.Image"), System.Drawing.Image)
		Me.btnDelete.Location = New System.Drawing.Point(1047, 0)
		Me.btnDelete.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
		Me.btnDelete.Name = "btnDelete"
		Me.btnDelete.Size = New System.Drawing.Size(147, 44)
		Me.btnDelete.TabIndex = 315
		Me.btnDelete.Text = "Löschen"
		'
		'frmTables
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(10.0!, 23.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(1612, 1111)
		Me.Controls.Add(Me.PanelControl1)
		Me.Controls.Add(Me.sccMain)
		Me.Controls.Add(Me.grdTableContent)
		Me.Controls.Add(Me.lblTableCaption)
		Me.Controls.Add(Me.pnlHeader)
		Me.Controls.Add(Me.BarDockControl7)
		Me.Controls.Add(Me.BarDockControl8)
		Me.Controls.Add(Me.BarDockControl6)
		Me.Controls.Add(Me.BarDockControl5)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
		Me.Name = "frmTables"
		Me.Text = "Tabellenverwaltung"
		CType(Me.grdTableContent, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvTableContent, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.pnlHeader, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pnlHeader.ResumeLayout(False)
		Me.pnlHeader.PerformLayout()
		CType(Me.sccMain, System.ComponentModel.ISupportInitialize).EndInit()
		Me.sccMain.ResumeLayout(False)
		CType(Me.grpFilter, System.ComponentModel.ISupportInitialize).EndInit()
		Me.grpFilter.ResumeLayout(False)
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lueYear.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.navMain, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.RepositoryItemFontEdit1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.RepositoryItemComboBox1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.RepositoryItemComboBox2, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.PanelControl1.ResumeLayout(False)
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents grdTableContent As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvTableContent As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents lblTableCaption As DevExpress.XtraEditors.LabelControl
	Friend WithEvents pnlHeader As DevExpress.XtraEditors.PanelControl
	Friend WithEvents btnClose As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents lblHeaderDescription As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblHeader As DevExpress.XtraEditors.LabelControl
	Friend WithEvents sccMain As DevExpress.XtraEditors.SplitContainerControl
	Friend WithEvents navMain As DevExpress.XtraNavBar.NavBarControl
	Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
	Friend WithEvents Bar3 As DevExpress.XtraBars.Bar
	Friend WithEvents bsiLblRecCount As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents bsiRecCount As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents BarStaticItem3 As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents BarDockControl5 As DevExpress.XtraBars.BarDockControl
	Friend WithEvents BarDockControl6 As DevExpress.XtraBars.BarDockControl
	Friend WithEvents BarDockControl7 As DevExpress.XtraBars.BarDockControl
	Friend WithEvents BarDockControl8 As DevExpress.XtraBars.BarDockControl
	Friend WithEvents BarSubItem3 As DevExpress.XtraBars.BarSubItem
	Friend WithEvents RepositoryItemFontEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemFontEdit
	Friend WithEvents RepositoryItemComboBox1 As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
	Friend WithEvents RepositoryItemComboBox2 As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
	Friend WithEvents Bar1 As DevExpress.XtraBars.Bar
	Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
	Friend WithEvents btnDelete As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents lueYear As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents lblJahr As System.Windows.Forms.Label
	Friend WithEvents grpFilter As DevExpress.XtraEditors.GroupControl
	Friend WithEvents lblHeaderimg As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lueMandant As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents lblMDName As System.Windows.Forms.Label
End Class
