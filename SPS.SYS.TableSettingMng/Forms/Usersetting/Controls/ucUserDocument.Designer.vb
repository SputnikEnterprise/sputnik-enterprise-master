<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucUserDocument
    Inherits DevExpress.XtraEditors.XtraUserControl

    'UserControl overrides dispose to clean up the component list.
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ucUserDocument))
		Me.grdDoc = New DevExpress.XtraGrid.GridControl()
		Me.gvDoc = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
		Me.Bar3 = New DevExpress.XtraBars.Bar()
		Me.bsiInfo = New DevExpress.XtraBars.BarStaticItem()
		Me.bbiCopyForAllUsers = New DevExpress.XtraBars.BarButtonItem()
		Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
		CType(Me.grdDoc, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvDoc, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'grdDoc
		'
		Me.grdDoc.Cursor = System.Windows.Forms.Cursors.Default
		Me.grdDoc.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdDoc.EmbeddedNavigator.Buttons.First.Visible = False
		Me.grdDoc.EmbeddedNavigator.Buttons.Last.Visible = False
		Me.grdDoc.EmbeddedNavigator.Buttons.Next.Visible = False
		Me.grdDoc.EmbeddedNavigator.Buttons.NextPage.Visible = False
		Me.grdDoc.EmbeddedNavigator.Buttons.Prev.Visible = False
		Me.grdDoc.EmbeddedNavigator.Buttons.PrevPage.Visible = False
		GridLevelNode1.RelationName = "Level1"
		Me.grdDoc.LevelTree.Nodes.AddRange(New DevExpress.XtraGrid.GridLevelNode() {GridLevelNode1})
		Me.grdDoc.Location = New System.Drawing.Point(0, 0)
		Me.grdDoc.MainView = Me.gvDoc
		Me.grdDoc.Name = "grdDoc"
		Me.grdDoc.Size = New System.Drawing.Size(641, 371)
		Me.grdDoc.TabIndex = 335
		Me.grdDoc.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvDoc})
		'
		'gvDoc
		'
		Me.gvDoc.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvDoc.GridControl = Me.grdDoc
		Me.gvDoc.Name = "gvDoc"
		Me.gvDoc.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.[False]
		Me.gvDoc.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.[False]
		Me.gvDoc.OptionsBehavior.AutoSelectAllInEditor = False
		Me.gvDoc.OptionsBehavior.EditingMode = DevExpress.XtraGrid.Views.Grid.GridEditingMode.Inplace
		Me.gvDoc.OptionsPrint.ExpandAllDetails = True
		Me.gvDoc.OptionsPrint.PrintDetails = True
		Me.gvDoc.OptionsPrint.PrintPreview = True
		Me.gvDoc.OptionsView.RowAutoHeight = True
		Me.gvDoc.OptionsView.ShowGroupPanel = False
		'
		'BarManager1
		'
		Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar3})
		Me.BarManager1.DockControls.Add(Me.barDockControlTop)
		Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
		Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
		Me.BarManager1.DockControls.Add(Me.barDockControlRight)
		Me.BarManager1.Form = Me
		Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiInfo, Me.bbiCopyForAllUsers})
		Me.BarManager1.MaxItemId = 2
		Me.BarManager1.StatusBar = Me.Bar3
		'
		'Bar3
		'
		Me.Bar3.BarName = "Statusleiste"
		Me.Bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
		Me.Bar3.DockCol = 0
		Me.Bar3.DockRow = 0
		Me.Bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
		Me.Bar3.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiInfo), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiCopyForAllUsers)})
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
		Me.bsiInfo.TextAlignment = System.Drawing.StringAlignment.Near
		Me.bsiInfo.Width = 32
		'
		'bbiCopyForAllUsers
		'
		Me.bbiCopyForAllUsers.Caption = "Daten für alle Benutzer kopieren"
		Me.bbiCopyForAllUsers.Glyph = CType(resources.GetObject("bbiCopyForAllUsers.Glyph"), System.Drawing.Image)
		Me.bbiCopyForAllUsers.Id = 1
		Me.bbiCopyForAllUsers.LargeGlyph = CType(resources.GetObject("bbiCopyForAllUsers.LargeGlyph"), System.Drawing.Image)
		Me.bbiCopyForAllUsers.Name = "bbiCopyForAllUsers"
		Me.bbiCopyForAllUsers.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'barDockControlTop
		'
		Me.barDockControlTop.CausesValidation = False
		Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
		Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlTop.Size = New System.Drawing.Size(641, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 371)
		Me.barDockControlBottom.Size = New System.Drawing.Size(641, 27)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 371)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(641, 0)
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 371)
		'
		'ucUserDocument
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.Controls.Add(Me.grdDoc)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.Name = "ucUserDocument"
		Me.Size = New System.Drawing.Size(641, 398)
		CType(Me.grdDoc, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvDoc, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents grdDoc As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvDoc As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
	Friend WithEvents Bar3 As DevExpress.XtraBars.Bar
	Friend WithEvents bsiInfo As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents bbiCopyForAllUsers As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl

End Class
