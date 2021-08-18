<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLATranslation
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
		Dim GridLevelNode1 As DevExpress.XtraGrid.GridLevelNode = New DevExpress.XtraGrid.GridLevelNode()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLATranslation))
		Me.Bar3 = New DevExpress.XtraBars.Bar()
		Me.bsiLblRecCount = New DevExpress.XtraBars.BarStaticItem()
		Me.bsiRecCount = New DevExpress.XtraBars.BarStaticItem()
		Me.BarStaticItem3 = New DevExpress.XtraBars.BarStaticItem()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
		Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
		Me.BarSubItem3 = New DevExpress.XtraBars.BarSubItem()
		Me.RepositoryItemFontEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemFontEdit()
		Me.RepositoryItemComboBox1 = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
		Me.RepositoryItemComboBox2 = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
		Me.pnlHeader = New DevExpress.XtraEditors.PanelControl()
		Me.btnClose = New DevExpress.XtraEditors.SimpleButton()
		Me.lblHeaderDescription = New DevExpress.XtraEditors.LabelControl()
		Me.lblHeader = New DevExpress.XtraEditors.LabelControl()
		Me.grdLATranslated = New DevExpress.XtraGrid.GridControl()
		Me.gvLATranslated = New DevExpress.XtraGrid.Views.Grid.GridView()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.RepositoryItemFontEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.RepositoryItemComboBox1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.RepositoryItemComboBox2, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.pnlHeader, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.pnlHeader.SuspendLayout()
		CType(Me.grdLATranslated, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvLATranslated, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
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
		Me.bsiLblRecCount.TextAlignment = System.Drawing.StringAlignment.Near
		'
		'bsiRecCount
		'
		Me.bsiRecCount.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
		Me.bsiRecCount.Caption = "{0}"
		Me.bsiRecCount.Id = 60
		Me.bsiRecCount.Name = "bsiRecCount"
		Me.bsiRecCount.TextAlignment = System.Drawing.StringAlignment.Near
		'
		'BarStaticItem3
		'
		Me.BarStaticItem3.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
		Me.BarStaticItem3.Id = 62
		Me.BarStaticItem3.Name = "BarStaticItem3"
		Me.BarStaticItem3.TextAlignment = System.Drawing.StringAlignment.Near
		'
		'BarManager1
		'
		Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar3})
		Me.BarManager1.DockControls.Add(Me.barDockControlTop)
		Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
		Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
		Me.BarManager1.DockControls.Add(Me.barDockControlRight)
		Me.BarManager1.Form = Me
		Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.BarSubItem3, Me.bsiRecCount, Me.BarStaticItem3, Me.bsiLblRecCount})
		Me.BarManager1.MaxItemId = 66
		Me.BarManager1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemFontEdit1, Me.RepositoryItemComboBox1, Me.RepositoryItemComboBox2})
		Me.BarManager1.StatusBar = Me.Bar3
		'
		'barDockControlTop
		'
		Me.barDockControlTop.CausesValidation = False
		Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
		Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlTop.Size = New System.Drawing.Size(1159, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 597)
		Me.barDockControlBottom.Size = New System.Drawing.Size(1159, 25)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 597)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(1159, 0)
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 597)
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
		'pnlHeader
		'
		Me.pnlHeader.Controls.Add(Me.btnClose)
		Me.pnlHeader.Controls.Add(Me.lblHeaderDescription)
		Me.pnlHeader.Controls.Add(Me.lblHeader)
		Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
		Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
		Me.pnlHeader.Name = "pnlHeader"
		Me.pnlHeader.Size = New System.Drawing.Size(1159, 71)
		Me.pnlHeader.TabIndex = 333
		'
		'btnClose
		'
		Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnClose.Location = New System.Drawing.Point(1060, 12)
		Me.btnClose.Name = "btnClose"
		Me.btnClose.Size = New System.Drawing.Size(87, 23)
		Me.btnClose.TabIndex = 0
		Me.btnClose.Text = "Schliessen"
		'
		'lblHeaderDescription
		'
		Me.lblHeaderDescription.Location = New System.Drawing.Point(29, 33)
		Me.lblHeaderDescription.Name = "lblHeaderDescription"
		Me.lblHeaderDescription.Size = New System.Drawing.Size(202, 13)
		Me.lblHeaderDescription.TabIndex = 1
		Me.lblHeaderDescription.Text = "Tragen Sie Ihre gewünschten Daten ein..."
		'
		'lblHeader
		'
		Me.lblHeader.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblHeader.Location = New System.Drawing.Point(12, 12)
		Me.lblHeader.Name = "lblHeader"
		Me.lblHeader.Size = New System.Drawing.Size(280, 13)
		Me.lblHeader.TabIndex = 0
		Me.lblHeader.Text = "Verwaltung von Lohnartenstamm-Übersetzungen"
		'
		'grdLATranslated
		'
		Me.grdLATranslated.Cursor = System.Windows.Forms.Cursors.Default
		Me.grdLATranslated.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdLATranslated.EmbeddedNavigator.Buttons.First.Visible = False
		Me.grdLATranslated.EmbeddedNavigator.Buttons.Last.Visible = False
		Me.grdLATranslated.EmbeddedNavigator.Buttons.Next.Visible = False
		Me.grdLATranslated.EmbeddedNavigator.Buttons.NextPage.Visible = False
		Me.grdLATranslated.EmbeddedNavigator.Buttons.Prev.Visible = False
		Me.grdLATranslated.EmbeddedNavigator.Buttons.PrevPage.Visible = False
		GridLevelNode1.RelationName = "Level1"
		Me.grdLATranslated.LevelTree.Nodes.AddRange(New DevExpress.XtraGrid.GridLevelNode() {GridLevelNode1})
		Me.grdLATranslated.Location = New System.Drawing.Point(0, 71)
		Me.grdLATranslated.MainView = Me.gvLATranslated
		Me.grdLATranslated.Name = "grdLATranslated"
		Me.grdLATranslated.Size = New System.Drawing.Size(1159, 526)
		Me.grdLATranslated.TabIndex = 334
		Me.grdLATranslated.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvLATranslated})
		'
		'gvLATranslated
		'
		Me.gvLATranslated.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvLATranslated.GridControl = Me.grdLATranslated
		Me.gvLATranslated.Name = "gvLATranslated"
		Me.gvLATranslated.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.[True]
		Me.gvLATranslated.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.[True]
		Me.gvLATranslated.OptionsBehavior.AutoSelectAllInEditor = False
		Me.gvLATranslated.OptionsBehavior.EditingMode = DevExpress.XtraGrid.Views.Grid.GridEditingMode.EditFormInplace
		Me.gvLATranslated.OptionsPrint.ExpandAllDetails = True
		Me.gvLATranslated.OptionsPrint.PrintDetails = True
		Me.gvLATranslated.OptionsPrint.PrintPreview = True
		Me.gvLATranslated.OptionsView.RowAutoHeight = True
		Me.gvLATranslated.OptionsView.ShowGroupPanel = False
		'
		'frmLATranslation
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(1159, 622)
		Me.Controls.Add(Me.grdLATranslated)
		Me.Controls.Add(Me.pnlHeader)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.Name = "frmLATranslation"
		Me.Text = "Übersetzung der Lohnartenstamm"
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.RepositoryItemFontEdit1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.RepositoryItemComboBox1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.RepositoryItemComboBox2, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.pnlHeader, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pnlHeader.ResumeLayout(False)
		Me.pnlHeader.PerformLayout()
		CType(Me.grdLATranslated, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvLATranslated, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents Bar3 As DevExpress.XtraBars.Bar
	Friend WithEvents bsiLblRecCount As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents bsiRecCount As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents BarStaticItem3 As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
	Friend WithEvents BarSubItem3 As DevExpress.XtraBars.BarSubItem
	Friend WithEvents RepositoryItemFontEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemFontEdit
	Friend WithEvents RepositoryItemComboBox1 As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
	Friend WithEvents RepositoryItemComboBox2 As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
	Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
	Friend WithEvents pnlHeader As DevExpress.XtraEditors.PanelControl
	Friend WithEvents btnClose As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents lblHeaderDescription As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblHeader As DevExpress.XtraEditors.LabelControl
	Friend WithEvents grdLATranslated As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvLATranslated As DevExpress.XtraGrid.Views.Grid.GridView
End Class
