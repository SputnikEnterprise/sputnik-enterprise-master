Namespace UI


	<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
	Partial Class frmUpdateNotifications
		Inherits DevExpress.XtraEditors.XtraForm

		'Form overrides dispose to clean up the component list.
		<System.Diagnostics.DebuggerNonUserCode()>
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
		<System.Diagnostics.DebuggerStepThrough()>
		Private Sub InitializeComponent()
			Me.components = New System.ComponentModel.Container()
			Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUpdateNotifications))
			Dim GridLevelNode1 As DevExpress.XtraGrid.GridLevelNode = New DevExpress.XtraGrid.GridLevelNode()
			Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
			Me.reNotifyHeader = New DevExpress.XtraRichEdit.RichEditControl()
			Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
			Me.Bar1 = New DevExpress.XtraBars.Bar()
			Me.bhiCreatedonLabel = New DevExpress.XtraBars.BarHeaderItem()
			Me.bsiCreatedon = New DevExpress.XtraBars.BarStaticItem()
			Me.bhiCheckedonLabel = New DevExpress.XtraBars.BarHeaderItem()
			Me.bsiCheckedon = New DevExpress.XtraBars.BarStaticItem()
			Me.bsiReserve = New DevExpress.XtraBars.BarStaticItem()
			Me.bbiSave = New DevExpress.XtraBars.BarLargeButtonItem()
			Me.bbiNew = New DevExpress.XtraBars.BarButtonItem()
			Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
			Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
			Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
			Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
			Me.BarStaticItem1 = New DevExpress.XtraBars.BarStaticItem()
			Me.BarStaticItem3 = New DevExpress.XtraBars.BarStaticItem()
			Me.BarStaticItem4 = New DevExpress.XtraBars.BarStaticItem()
			Me.BarStaticItem6 = New DevExpress.XtraBars.BarStaticItem()
			Me.reNotifyComments = New DevExpress.XtraRichEdit.RichEditControl()
			Me.lblBeschreibung = New DevExpress.XtraEditors.LabelControl()
			Me.lblBetreff = New DevExpress.XtraEditors.LabelControl()
			Me.chkExcludeChecked = New DevExpress.XtraEditors.CheckEdit()
			Me.grdTableContent = New DevExpress.XtraGrid.GridControl()
			Me.gvTableContent = New DevExpress.XtraGrid.Views.Grid.GridView()
			CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.PanelControl1.SuspendLayout()
			CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.chkExcludeChecked.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.grdTableContent, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvTableContent, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			'
			'PanelControl1
			'
			Me.PanelControl1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.PanelControl1.Controls.Add(Me.reNotifyHeader)
			Me.PanelControl1.Controls.Add(Me.reNotifyComments)
			Me.PanelControl1.Controls.Add(Me.lblBeschreibung)
			Me.PanelControl1.Controls.Add(Me.lblBetreff)
			Me.PanelControl1.Location = New System.Drawing.Point(12, 302)
			Me.PanelControl1.Name = "PanelControl1"
			Me.PanelControl1.Size = New System.Drawing.Size(1081, 292)
			Me.PanelControl1.TabIndex = 327
			'
			'reNotifyHeader
			'
			Me.reNotifyHeader.ActiveViewType = DevExpress.XtraRichEdit.RichEditViewType.Simple
			Me.reNotifyHeader.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.reNotifyHeader.Appearance.Text.Options.UseFont = True
			Me.reNotifyHeader.Location = New System.Drawing.Point(152, 13)
			Me.reNotifyHeader.MenuManager = Me.BarManager1
			Me.reNotifyHeader.Name = "reNotifyHeader"
			Me.reNotifyHeader.ReadOnly = True
			Me.reNotifyHeader.Size = New System.Drawing.Size(911, 41)
			Me.reNotifyHeader.TabIndex = 336
			Me.reNotifyHeader.Unit = DevExpress.Office.DocumentUnit.Millimeter
			Me.reNotifyHeader.Views.SimpleView.AllowDisplayLineNumbers = True
			Me.reNotifyHeader.Views.SimpleView.Padding = New DevExpress.Portable.PortablePadding(5, 4, 4, 0)
			'
			'BarManager1
			'
			Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar1})
			Me.BarManager1.DockControls.Add(Me.barDockControlTop)
			Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
			Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
			Me.BarManager1.DockControls.Add(Me.barDockControlRight)
			Me.BarManager1.Form = Me
			Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bbiSave, Me.BarStaticItem1, Me.bhiCreatedonLabel, Me.bsiCreatedon, Me.bhiCheckedonLabel, Me.BarStaticItem3, Me.BarStaticItem4, Me.bsiCheckedon, Me.BarStaticItem6, Me.bsiReserve, Me.bbiNew})
			Me.BarManager1.MaxItemId = 23
			Me.BarManager1.StatusBar = Me.Bar1
			'
			'Bar1
			'
			Me.Bar1.BarName = "Statusleiste"
			Me.Bar1.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
			Me.Bar1.DockCol = 0
			Me.Bar1.DockRow = 0
			Me.Bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
			Me.Bar1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bhiCreatedonLabel), New DevExpress.XtraBars.LinkPersistInfo(Me.bsiCreatedon), New DevExpress.XtraBars.LinkPersistInfo(Me.bhiCheckedonLabel), New DevExpress.XtraBars.LinkPersistInfo(Me.bsiCheckedon), New DevExpress.XtraBars.LinkPersistInfo(Me.bsiReserve), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiSave, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiNew)})
			Me.Bar1.OptionsBar.AllowQuickCustomization = False
			Me.Bar1.OptionsBar.DrawDragBorder = False
			Me.Bar1.OptionsBar.UseWholeRow = True
			Me.Bar1.Text = "Statusleiste"
			'
			'bhiCreatedonLabel
			'
			Me.bhiCreatedonLabel.Caption = "Veröffentlicht"
			Me.bhiCreatedonLabel.Id = 13
			Me.bhiCreatedonLabel.Name = "bhiCreatedonLabel"
			'
			'bsiCreatedon
			'
			Me.bsiCreatedon.Caption = "createdon"
			Me.bsiCreatedon.Id = 14
			Me.bsiCreatedon.Name = "bsiCreatedon"
			'
			'bhiCheckedonLabel
			'
			Me.bhiCheckedonLabel.Caption = "Gelesen"
			Me.bhiCheckedonLabel.Id = 15
			Me.bhiCheckedonLabel.Name = "bhiCheckedonLabel"
			'
			'bsiCheckedon
			'
			Me.bsiCheckedon.Caption = "checkedon"
			Me.bsiCheckedon.Id = 18
			Me.bsiCheckedon.Name = "bsiCheckedon"
			'
			'bsiReserve
			'
			Me.bsiReserve.AutoSize = DevExpress.XtraBars.BarStaticItemSize.Spring
			Me.bsiReserve.Id = 20
			Me.bsiReserve.Name = "bsiReserve"
			'
			'bbiSave
			'
			Me.bbiSave.Caption = "Speichern"
			Me.bbiSave.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Right
			Me.bbiSave.Id = 6
			Me.bbiSave.ImageOptions.Image = CType(resources.GetObject("bbiSave.ImageOptions.Image"), System.Drawing.Image)
			Me.bbiSave.Name = "bbiSave"
			Me.bbiSave.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
			'
			'bbiNew
			'
			Me.bbiNew.Caption = "Neu"
			Me.bbiNew.Id = 22
			Me.bbiNew.ImageOptions.Image = CType(resources.GetObject("bbiNew.ImageOptions.Image"), System.Drawing.Image)
			Me.bbiNew.Name = "bbiNew"
			Me.bbiNew.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
			'
			'barDockControlTop
			'
			Me.barDockControlTop.CausesValidation = False
			Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
			Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
			Me.barDockControlTop.Manager = Me.BarManager1
			Me.barDockControlTop.Size = New System.Drawing.Size(1105, 0)
			'
			'barDockControlBottom
			'
			Me.barDockControlBottom.CausesValidation = False
			Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
			Me.barDockControlBottom.Location = New System.Drawing.Point(0, 613)
			Me.barDockControlBottom.Manager = Me.BarManager1
			Me.barDockControlBottom.Size = New System.Drawing.Size(1105, 27)
			'
			'barDockControlLeft
			'
			Me.barDockControlLeft.CausesValidation = False
			Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
			Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
			Me.barDockControlLeft.Manager = Me.BarManager1
			Me.barDockControlLeft.Size = New System.Drawing.Size(0, 613)
			'
			'barDockControlRight
			'
			Me.barDockControlRight.CausesValidation = False
			Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
			Me.barDockControlRight.Location = New System.Drawing.Point(1105, 0)
			Me.barDockControlRight.Manager = Me.BarManager1
			Me.barDockControlRight.Size = New System.Drawing.Size(0, 613)
			'
			'BarStaticItem1
			'
			Me.BarStaticItem1.Caption = " "
			Me.BarStaticItem1.Id = 5
			Me.BarStaticItem1.Name = "BarStaticItem1"
			'
			'BarStaticItem3
			'
			Me.BarStaticItem3.Caption = "checkedon"
			Me.BarStaticItem3.Id = 16
			Me.BarStaticItem3.Name = "BarStaticItem3"
			'
			'BarStaticItem4
			'
			Me.BarStaticItem4.Caption = "checkedon"
			Me.BarStaticItem4.Id = 17
			Me.BarStaticItem4.Name = "BarStaticItem4"
			'
			'BarStaticItem6
			'
			Me.BarStaticItem6.Id = 19
			Me.BarStaticItem6.Name = "BarStaticItem6"
			'
			'reNotifyComments
			'
			Me.reNotifyComments.ActiveViewType = DevExpress.XtraRichEdit.RichEditViewType.Simple
			Me.reNotifyComments.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.reNotifyComments.Appearance.Text.Options.UseFont = True
			Me.reNotifyComments.Location = New System.Drawing.Point(152, 60)
			Me.reNotifyComments.MenuManager = Me.BarManager1
			Me.reNotifyComments.Name = "reNotifyComments"
			Me.reNotifyComments.ReadOnly = True
			Me.reNotifyComments.Size = New System.Drawing.Size(911, 218)
			Me.reNotifyComments.TabIndex = 335
			Me.reNotifyComments.Unit = DevExpress.Office.DocumentUnit.Millimeter
			Me.reNotifyComments.Views.SimpleView.AllowDisplayLineNumbers = True
			Me.reNotifyComments.Views.SimpleView.Padding = New DevExpress.Portable.PortablePadding(5, 4, 4, 0)
			'
			'lblBeschreibung
			'
			Me.lblBeschreibung.Appearance.Options.UseTextOptions = True
			Me.lblBeschreibung.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblBeschreibung.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblBeschreibung.Location = New System.Drawing.Point(29, 60)
			Me.lblBeschreibung.Name = "lblBeschreibung"
			Me.lblBeschreibung.Size = New System.Drawing.Size(114, 13)
			Me.lblBeschreibung.TabIndex = 328
			Me.lblBeschreibung.Text = "Beschreibung"
			'
			'lblBetreff
			'
			Me.lblBetreff.Appearance.Options.UseTextOptions = True
			Me.lblBetreff.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblBetreff.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblBetreff.Location = New System.Drawing.Point(29, 13)
			Me.lblBetreff.Name = "lblBetreff"
			Me.lblBetreff.Size = New System.Drawing.Size(114, 13)
			Me.lblBetreff.TabIndex = 324
			Me.lblBetreff.Text = "Betreff"
			'
			'chkExcludeChecked
			'
			Me.chkExcludeChecked.EditValue = True
			Me.chkExcludeChecked.Location = New System.Drawing.Point(833, 3)
			Me.chkExcludeChecked.Name = "chkExcludeChecked"
			Me.chkExcludeChecked.Properties.AllowFocused = False
			Me.chkExcludeChecked.Properties.Appearance.Options.UseTextOptions = True
			Me.chkExcludeChecked.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.chkExcludeChecked.Properties.Caption = "Bereits eingesehene Informationen ausblenden"
			Me.chkExcludeChecked.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.chkExcludeChecked.Size = New System.Drawing.Size(260, 19)
			Me.chkExcludeChecked.TabIndex = 336
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
			GridLevelNode1.RelationName = "Level1"
			Me.grdTableContent.LevelTree.Nodes.AddRange(New DevExpress.XtraGrid.GridLevelNode() {GridLevelNode1})
			Me.grdTableContent.Location = New System.Drawing.Point(12, 28)
			Me.grdTableContent.MainView = Me.gvTableContent
			Me.grdTableContent.Name = "grdTableContent"
			Me.grdTableContent.Size = New System.Drawing.Size(1081, 268)
			Me.grdTableContent.TabIndex = 341
			Me.grdTableContent.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvTableContent})
			'
			'gvTableContent
			'
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
			'frmUpdateNotifications
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(1105, 640)
			Me.Controls.Add(Me.grdTableContent)
			Me.Controls.Add(Me.chkExcludeChecked)
			Me.Controls.Add(Me.PanelControl1)
			Me.Controls.Add(Me.barDockControlLeft)
			Me.Controls.Add(Me.barDockControlRight)
			Me.Controls.Add(Me.barDockControlBottom)
			Me.Controls.Add(Me.barDockControlTop)
			Me.Name = "frmUpdateNotifications"
			Me.Text = "frmUpdateNotifications"
			CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.PanelControl1.ResumeLayout(False)
			CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.chkExcludeChecked.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.grdTableContent, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvTableContent, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)
			Me.PerformLayout()

		End Sub
		Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
		Friend WithEvents reNotifyComments As DevExpress.XtraRichEdit.RichEditControl
		Friend WithEvents lblBeschreibung As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblBetreff As DevExpress.XtraEditors.LabelControl
		Friend WithEvents chkExcludeChecked As DevExpress.XtraEditors.CheckEdit
		Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
		Friend WithEvents Bar1 As DevExpress.XtraBars.Bar
		Friend WithEvents bhiCreatedonLabel As DevExpress.XtraBars.BarHeaderItem
		Friend WithEvents bsiCreatedon As DevExpress.XtraBars.BarStaticItem
		Friend WithEvents bhiCheckedonLabel As DevExpress.XtraBars.BarHeaderItem
		Friend WithEvents bbiSave As DevExpress.XtraBars.BarLargeButtonItem
		Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
		Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
		Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
		Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
		Friend WithEvents BarStaticItem1 As DevExpress.XtraBars.BarStaticItem
		Friend WithEvents BarStaticItem3 As DevExpress.XtraBars.BarStaticItem
		Friend WithEvents BarStaticItem4 As DevExpress.XtraBars.BarStaticItem
		Friend WithEvents bsiCheckedon As DevExpress.XtraBars.BarStaticItem
		Friend WithEvents bsiReserve As DevExpress.XtraBars.BarStaticItem
		Friend WithEvents BarStaticItem6 As DevExpress.XtraBars.BarStaticItem
		Friend WithEvents reNotifyHeader As DevExpress.XtraRichEdit.RichEditControl
		Friend WithEvents bbiNew As DevExpress.XtraBars.BarButtonItem
		Friend WithEvents grdTableContent As DevExpress.XtraGrid.GridControl
		Friend WithEvents gvTableContent As DevExpress.XtraGrid.Views.Grid.GridView
	End Class

End Namespace
