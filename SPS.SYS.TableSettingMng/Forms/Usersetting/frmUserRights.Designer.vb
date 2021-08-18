<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUserRights
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUserRights))
		Dim GridLevelNode1 As DevExpress.XtraGrid.GridLevelNode = New DevExpress.XtraGrid.GridLevelNode()
		Dim EditorButtonImageOptions1 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
		Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject4 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim EditorButtonImageOptions2 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
		Dim SerializableAppearanceObject5 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject6 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject7 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject8 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim EditorButtonImageOptions3 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
		Dim SerializableAppearanceObject9 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject10 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject11 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject12 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Me.pnlHeader = New DevExpress.XtraEditors.PanelControl()
		Me.lblUserFullname = New DevExpress.XtraEditors.LabelControl()
		Me.lblHeaderimg = New DevExpress.XtraEditors.LabelControl()
		Me.btnClose = New DevExpress.XtraEditors.SimpleButton()
		Me.lblHeaderDescription = New DevExpress.XtraEditors.LabelControl()
		Me.lblHeader = New DevExpress.XtraEditors.LabelControl()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
		Me.Bar4 = New DevExpress.XtraBars.Bar()
		Me.bsiInfo = New DevExpress.XtraBars.BarStaticItem()
		Me.bbiSaveforAll = New DevExpress.XtraBars.BarButtonItem()
		Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
		Me.BarStaticItem1 = New DevExpress.XtraBars.BarStaticItem()
		Me.grdUserRights = New DevExpress.XtraGrid.GridControl()
		Me.gvUserRights = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.lblTableCaption = New DevExpress.XtraEditors.LabelControl()
		Me.sccMain = New DevExpress.XtraEditors.SplitContainerControl()
		Me.grpFilter = New DevExpress.XtraEditors.GroupControl()
		Me.lueRights = New DevExpress.XtraEditors.LookUpEdit()
		Me.lueMandant = New DevExpress.XtraEditors.LookUpEdit()
		Me.navMain = New DevExpress.XtraNavBar.NavBarControl()
		Me.btnDelete = New DevExpress.XtraEditors.SimpleButton()
		CType(Me.pnlHeader, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.pnlHeader.SuspendLayout()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.grdUserRights, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvUserRights, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.sccMain, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.sccMain.SuspendLayout()
		CType(Me.grpFilter, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grpFilter.SuspendLayout()
		CType(Me.lueRights.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.navMain, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'pnlHeader
		'
		Me.pnlHeader.Controls.Add(Me.lblUserFullname)
		Me.pnlHeader.Controls.Add(Me.lblHeaderimg)
		Me.pnlHeader.Controls.Add(Me.btnClose)
		Me.pnlHeader.Controls.Add(Me.lblHeaderDescription)
		Me.pnlHeader.Controls.Add(Me.lblHeader)
		Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
		Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
		Me.pnlHeader.Name = "pnlHeader"
		Me.pnlHeader.Size = New System.Drawing.Size(792, 71)
		Me.pnlHeader.TabIndex = 303
		'
		'lblUserFullname
		'
		Me.lblUserFullname.Location = New System.Drawing.Point(80, 52)
		Me.lblUserFullname.Name = "lblUserFullname"
		Me.lblUserFullname.Size = New System.Drawing.Size(74, 13)
		Me.lblUserFullname.TabIndex = 1002
		Me.lblUserFullname.Text = "lblUserFullname"
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
		Me.lblHeaderimg.Location = New System.Drawing.Point(12, 8)
		Me.lblHeaderimg.Name = "lblHeaderimg"
		Me.lblHeaderimg.Size = New System.Drawing.Size(45, 38)
		Me.lblHeaderimg.TabIndex = 1001
		'
		'btnClose
		'
		Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnClose.Location = New System.Drawing.Point(693, 12)
		Me.btnClose.Name = "btnClose"
		Me.btnClose.Size = New System.Drawing.Size(87, 23)
		Me.btnClose.TabIndex = 2
		Me.btnClose.Text = "Schliessen"
		'
		'lblHeaderDescription
		'
		Me.lblHeaderDescription.Location = New System.Drawing.Point(80, 33)
		Me.lblHeaderDescription.Name = "lblHeaderDescription"
		Me.lblHeaderDescription.Size = New System.Drawing.Size(212, 13)
		Me.lblHeaderDescription.TabIndex = 1
		Me.lblHeaderDescription.Text = "Wählen Sie Ihre gewünschte Kriterien aus..."
		'
		'lblHeader
		'
		Me.lblHeader.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblHeader.Appearance.Options.UseFont = True
		Me.lblHeader.Location = New System.Drawing.Point(63, 12)
		Me.lblHeader.Name = "lblHeader"
		Me.lblHeader.Size = New System.Drawing.Size(148, 13)
		Me.lblHeader.TabIndex = 0
		Me.lblHeader.Text = "Benutzerrechte verwalten"
		'
		'BarManager1
		'
		Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar4})
		Me.BarManager1.DockControls.Add(Me.barDockControlTop)
		Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
		Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
		Me.BarManager1.DockControls.Add(Me.barDockControlRight)
		Me.BarManager1.Form = Me
		Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiInfo, Me.BarStaticItem1, Me.bbiSaveforAll})
		Me.BarManager1.MaxItemId = 19
		Me.BarManager1.StatusBar = Me.Bar4
		'
		'Bar4
		'
		Me.Bar4.BarName = "Statusleiste"
		Me.Bar4.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
		Me.Bar4.DockCol = 0
		Me.Bar4.DockRow = 0
		Me.Bar4.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
		Me.Bar4.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiInfo), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiSaveforAll, True)})
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
		'bbiSaveforAll
		'
		Me.bbiSaveforAll.Caption = "Benutzerrechte für alle übernehmen"
		Me.bbiSaveforAll.Id = 12
		Me.bbiSaveforAll.ImageOptions.Image = CType(resources.GetObject("bbiSaveforAll.ImageOptions.Image"), System.Drawing.Image)
		Me.bbiSaveforAll.ImageOptions.LargeImage = CType(resources.GetObject("bbiSaveforAll.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.bbiSaveforAll.Name = "bbiSaveforAll"
		Me.bbiSaveforAll.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'barDockControlTop
		'
		Me.barDockControlTop.CausesValidation = False
		Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
		Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlTop.Manager = Me.BarManager1
		Me.barDockControlTop.Size = New System.Drawing.Size(792, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 668)
		Me.barDockControlBottom.Manager = Me.BarManager1
		Me.barDockControlBottom.Size = New System.Drawing.Size(792, 28)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Manager = Me.BarManager1
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 668)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(792, 0)
		Me.barDockControlRight.Manager = Me.BarManager1
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 668)
		'
		'BarStaticItem1
		'
		Me.BarStaticItem1.Caption = " "
		Me.BarStaticItem1.Id = 5
		Me.BarStaticItem1.Name = "BarStaticItem1"
		'
		'grdUserRights
		'
		Me.grdUserRights.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.grdUserRights.Cursor = System.Windows.Forms.Cursors.Default
		Me.grdUserRights.EmbeddedNavigator.Buttons.First.Visible = False
		Me.grdUserRights.EmbeddedNavigator.Buttons.Last.Visible = False
		Me.grdUserRights.EmbeddedNavigator.Buttons.Next.Visible = False
		Me.grdUserRights.EmbeddedNavigator.Buttons.NextPage.Visible = False
		Me.grdUserRights.EmbeddedNavigator.Buttons.Prev.Visible = False
		Me.grdUserRights.EmbeddedNavigator.Buttons.PrevPage.Visible = False
		GridLevelNode1.RelationName = "Level1"
		Me.grdUserRights.LevelTree.Nodes.AddRange(New DevExpress.XtraGrid.GridLevelNode() {GridLevelNode1})
		Me.grdUserRights.Location = New System.Drawing.Point(204, 97)
		Me.grdUserRights.MainView = Me.gvUserRights
		Me.grdUserRights.Name = "grdUserRights"
		Me.grdUserRights.Size = New System.Drawing.Size(578, 525)
		Me.grdUserRights.TabIndex = 307
		Me.grdUserRights.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvUserRights})
		'
		'gvUserRights
		'
		Me.gvUserRights.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvUserRights.GridControl = Me.grdUserRights
		Me.gvUserRights.Name = "gvUserRights"
		Me.gvUserRights.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.[False]
		Me.gvUserRights.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.[False]
		Me.gvUserRights.OptionsBehavior.AutoSelectAllInEditor = False
		Me.gvUserRights.OptionsBehavior.EditingMode = DevExpress.XtraGrid.Views.Grid.GridEditingMode.Inplace
		Me.gvUserRights.OptionsClipboard.AllowHtmlFormat = DevExpress.Utils.DefaultBoolean.[True]
		Me.gvUserRights.OptionsPrint.ExpandAllDetails = True
		Me.gvUserRights.OptionsPrint.PrintDetails = True
		Me.gvUserRights.OptionsPrint.PrintPreview = True
		Me.gvUserRights.OptionsView.AllowHtmlDrawHeaders = True
		Me.gvUserRights.OptionsView.RowAutoHeight = True
		Me.gvUserRights.OptionsView.ShowGroupPanel = False
		'
		'lblTableCaption
		'
		Me.lblTableCaption.AllowHtmlString = True
		Me.lblTableCaption.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.lblTableCaption.Appearance.Options.UseFont = True
		Me.lblTableCaption.Location = New System.Drawing.Point(203, 77)
		Me.lblTableCaption.Name = "lblTableCaption"
		Me.lblTableCaption.Size = New System.Drawing.Size(161, 13)
		Me.lblTableCaption.TabIndex = 306
		Me.lblTableCaption.Text = "Kandidatenrechte verwalten"
		'
		'sccMain
		'
		Me.sccMain.Dock = System.Windows.Forms.DockStyle.Left
		Me.sccMain.Horizontal = False
		Me.sccMain.Location = New System.Drawing.Point(0, 71)
		Me.sccMain.Name = "sccMain"
		Me.sccMain.Panel1.Controls.Add(Me.grpFilter)
		Me.sccMain.Panel1.Controls.Add(Me.navMain)
		Me.sccMain.Panel1.Padding = New System.Windows.Forms.Padding(10)
		Me.sccMain.Panel1.Text = "Panel1"
		Me.sccMain.Panel2.Padding = New System.Windows.Forms.Padding(5)
		Me.sccMain.Panel2.Text = "Panel2"
		Me.sccMain.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel1
		Me.sccMain.Size = New System.Drawing.Size(197, 597)
		Me.sccMain.SplitterPosition = 520
		Me.sccMain.TabIndex = 308
		Me.sccMain.Text = "SplitContainerControl1"
		'
		'grpFilter
		'
		Me.grpFilter.Controls.Add(Me.lueRights)
		Me.grpFilter.Controls.Add(Me.lueMandant)
		Me.grpFilter.Dock = System.Windows.Forms.DockStyle.Top
		Me.grpFilter.Location = New System.Drawing.Point(10, 10)
		Me.grpFilter.Name = "grpFilter"
		Me.grpFilter.Size = New System.Drawing.Size(177, 95)
		Me.grpFilter.TabIndex = 22
		Me.grpFilter.Text = "Filter"
		'
		'lueRights
		'
		Me.lueRights.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.lueRights.Location = New System.Drawing.Point(8, 57)
		Me.lueRights.Name = "lueRights"
		SerializableAppearanceObject1.ForeColor = System.Drawing.Color.Green
		SerializableAppearanceObject1.Options.UseForeColor = True
		SerializableAppearanceObject2.ForeColor = System.Drawing.Color.Green
		SerializableAppearanceObject2.Options.UseForeColor = True
		SerializableAppearanceObject3.ForeColor = System.Drawing.Color.Green
		SerializableAppearanceObject3.Options.UseForeColor = True
		SerializableAppearanceObject4.ForeColor = System.Drawing.Color.Green
		SerializableAppearanceObject4.Options.UseForeColor = True
		Me.lueRights.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.OK, "", -1, True, True, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
		Me.lueRights.Properties.NullText = "Vorlage"
		Me.lueRights.Properties.ShowFooter = False
		Me.lueRights.Size = New System.Drawing.Size(161, 20)
		Me.lueRights.TabIndex = 363
		'
		'lueMandant
		'
		Me.lueMandant.EditValue = ""
		Me.lueMandant.Location = New System.Drawing.Point(8, 31)
		Me.lueMandant.Name = "lueMandant"
		Me.lueMandant.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, True, True, False, EditorButtonImageOptions2, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject5, SerializableAppearanceObject6, SerializableAppearanceObject7, SerializableAppearanceObject8, "", CType(0, Short), Nothing, DevExpress.Utils.ToolTipAnchor.[Default]), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Plus, "", -1, True, True, False, EditorButtonImageOptions3, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject9, SerializableAppearanceObject10, SerializableAppearanceObject11, SerializableAppearanceObject12, "", CType(1, Short), Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
		Me.lueMandant.Properties.DisplayFormat.FormatString = "n4"
		Me.lueMandant.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		Me.lueMandant.Properties.EditFormat.FormatString = "n4"
		Me.lueMandant.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		Me.lueMandant.Properties.NullText = "Mandant"
		Me.lueMandant.Properties.PopupSizeable = False
		Me.lueMandant.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
		Me.lueMandant.Size = New System.Drawing.Size(161, 20)
		Me.lueMandant.TabIndex = 267
		'
		'navMain
		'
		Me.navMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.navMain.Location = New System.Drawing.Point(10, 109)
		Me.navMain.Name = "navMain"
		Me.navMain.OptionsNavPane.ExpandedWidth = 177
		Me.navMain.Size = New System.Drawing.Size(177, 483)
		Me.navMain.TabIndex = 20
		Me.navMain.Text = "NavBarControl1"
		'
		'btnDelete
		'
		Me.btnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnDelete.ImageOptions.Image = CType(resources.GetObject("btnDelete.ImageOptions.Image"), System.Drawing.Image)
		Me.btnDelete.Location = New System.Drawing.Point(687, 631)
		Me.btnDelete.Name = "btnDelete"
		Me.btnDelete.Size = New System.Drawing.Size(88, 25)
		Me.btnDelete.TabIndex = 315
		Me.btnDelete.Text = "Löschen"
		'
		'frmUserRights
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(792, 696)
		Me.Controls.Add(Me.btnDelete)
		Me.Controls.Add(Me.sccMain)
		Me.Controls.Add(Me.grdUserRights)
		Me.Controls.Add(Me.lblTableCaption)
		Me.Controls.Add(Me.pnlHeader)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.IconOptions.Icon = CType(resources.GetObject("frmUserRights.IconOptions.Icon"), System.Drawing.Icon)
		Me.MinimumSize = New System.Drawing.Size(794, 728)
		Me.Name = "frmUserRights"
		Me.Text = "Benutzerrechte verwalten"
		CType(Me.pnlHeader, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pnlHeader.ResumeLayout(False)
		Me.pnlHeader.PerformLayout()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.grdUserRights, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvUserRights, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.sccMain, System.ComponentModel.ISupportInitialize).EndInit()
		Me.sccMain.ResumeLayout(False)
		CType(Me.grpFilter, System.ComponentModel.ISupportInitialize).EndInit()
		Me.grpFilter.ResumeLayout(False)
		CType(Me.lueRights.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.navMain, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents pnlHeader As DevExpress.XtraEditors.PanelControl
	Friend WithEvents lblHeaderimg As DevExpress.XtraEditors.LabelControl
	Friend WithEvents btnClose As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents lblHeaderDescription As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblHeader As DevExpress.XtraEditors.LabelControl
	Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
	Friend WithEvents Bar4 As DevExpress.XtraBars.Bar
	Friend WithEvents bsiInfo As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents bbiSaveforAll As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
	Friend WithEvents BarStaticItem1 As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents grdUserRights As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvUserRights As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents lblTableCaption As DevExpress.XtraEditors.LabelControl
	Friend WithEvents sccMain As DevExpress.XtraEditors.SplitContainerControl
	Friend WithEvents grpFilter As DevExpress.XtraEditors.GroupControl
	Friend WithEvents navMain As DevExpress.XtraNavBar.NavBarControl
	Friend WithEvents lueMandant As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents lueRights As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents lblUserFullname As DevExpress.XtraEditors.LabelControl
	Friend WithEvents btnDelete As DevExpress.XtraEditors.SimpleButton
End Class
