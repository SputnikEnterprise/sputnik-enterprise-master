<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucEmployeeTables
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ucEmployeeTables))
		Dim GridLevelNode1 As DevExpress.XtraGrid.GridLevelNode = New DevExpress.XtraGrid.GridLevelNode()
		Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
		Me.btnDelete = New DevExpress.XtraEditors.SimpleButton()
		Me.btnSave = New DevExpress.XtraEditors.SimpleButton()
		Me.rlblHeader = New DevComponents.DotNetBar.Controls.ReflectionLabel()
		Me.grdTableContent = New DevExpress.XtraGrid.GridControl()
		Me.gvTableContent = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.scMain = New DevExpress.XtraEditors.SplitContainerControl()
		Me.GroupControl1 = New DevExpress.XtraEditors.GroupControl()
		CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.PanelControl1.SuspendLayout()
		CType(Me.grdTableContent, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvTableContent, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.scMain, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.scMain.SuspendLayout()
		CType(Me.GroupControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupControl1.SuspendLayout()
		Me.SuspendLayout()
		'
		'PanelControl1
		'
		Me.PanelControl1.Controls.Add(Me.btnDelete)
		Me.PanelControl1.Controls.Add(Me.btnSave)
		Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.PanelControl1.Location = New System.Drawing.Point(5, 564)
		Me.PanelControl1.Name = "PanelControl1"
		Me.PanelControl1.Size = New System.Drawing.Size(819, 26)
		Me.PanelControl1.TabIndex = 317
		'
		'btnDelete
		'
		Me.btnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnDelete.Image = CType(resources.GetObject("btnDelete.Image"), System.Drawing.Image)
		Me.btnDelete.Location = New System.Drawing.Point(727, 0)
		Me.btnDelete.Name = "btnDelete"
		Me.btnDelete.Size = New System.Drawing.Size(88, 25)
		Me.btnDelete.TabIndex = 315
		Me.btnDelete.Text = "Löschen"
		'
		'btnSave
		'
		Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnSave.Image = CType(resources.GetObject("btnSave.Image"), System.Drawing.Image)
		Me.btnSave.Location = New System.Drawing.Point(633, 0)
		Me.btnSave.Name = "btnSave"
		Me.btnSave.Size = New System.Drawing.Size(88, 25)
		Me.btnSave.TabIndex = 0
		Me.btnSave.Text = "Speichern"
		'
		'rlblHeader
		'
		Me.rlblHeader.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.rlblHeader.BackColor = System.Drawing.Color.Transparent
		'
		'
		'
		Me.rlblHeader.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
		Me.rlblHeader.Location = New System.Drawing.Point(27, 3)
		Me.rlblHeader.Name = "rlblHeader"
		Me.rlblHeader.Size = New System.Drawing.Size(936, 35)
		Me.rlblHeader.TabIndex = 316
		Me.rlblHeader.Text = "<b><font size=""+3""><i>Dev</i><font color=""#B02B2C"">Components</font></font></b>"
		'
		'grdTableContent
		'
		Me.grdTableContent.Cursor = System.Windows.Forms.Cursors.Default
		Me.grdTableContent.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdTableContent.EmbeddedNavigator.Buttons.First.Visible = False
		Me.grdTableContent.EmbeddedNavigator.Buttons.Last.Visible = False
		Me.grdTableContent.EmbeddedNavigator.Buttons.Next.Visible = False
		Me.grdTableContent.EmbeddedNavigator.Buttons.NextPage.Visible = False
		Me.grdTableContent.EmbeddedNavigator.Buttons.Prev.Visible = False
		Me.grdTableContent.EmbeddedNavigator.Buttons.PrevPage.Visible = False
		GridLevelNode1.RelationName = "Level1"
		Me.grdTableContent.LevelTree.Nodes.AddRange(New DevExpress.XtraGrid.GridLevelNode() {GridLevelNode1})
		Me.grdTableContent.Location = New System.Drawing.Point(2, 21)
		Me.grdTableContent.MainView = Me.gvTableContent
		Me.grdTableContent.Name = "grdTableContent"
		Me.grdTableContent.Size = New System.Drawing.Size(805, 479)
		Me.grdTableContent.TabIndex = 315
		Me.grdTableContent.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvTableContent})
		'
		'gvTableContent
		'
		Me.gvTableContent.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvTableContent.GridControl = Me.grdTableContent
		Me.gvTableContent.Name = "gvTableContent"
		Me.gvTableContent.OptionsBehavior.AutoSelectAllInEditor = False
		Me.gvTableContent.OptionsBehavior.Editable = False
		Me.gvTableContent.OptionsPrint.ExpandAllDetails = True
		Me.gvTableContent.OptionsPrint.PrintDetails = True
		Me.gvTableContent.OptionsPrint.PrintPreview = True
		Me.gvTableContent.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvTableContent.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top
		Me.gvTableContent.OptionsView.RowAutoHeight = True
		Me.gvTableContent.OptionsView.ShowGroupPanel = False
		'
		'scMain
		'
		Me.scMain.Dock = System.Windows.Forms.DockStyle.Fill
		Me.scMain.Horizontal = False
		Me.scMain.IsSplitterFixed = True
		Me.scMain.Location = New System.Drawing.Point(5, 5)
		Me.scMain.Name = "scMain"
		Me.scMain.Panel1.Controls.Add(Me.rlblHeader)
		Me.scMain.Panel1.Text = "Panel1"
		Me.scMain.Panel2.Controls.Add(Me.GroupControl1)
		Me.scMain.Panel2.Padding = New System.Windows.Forms.Padding(5)
		Me.scMain.Panel2.Text = "Panel2"
		Me.scMain.Size = New System.Drawing.Size(819, 559)
		Me.scMain.SplitterPosition = 42
		Me.scMain.TabIndex = 318
		Me.scMain.Text = "SplitContainerControl1"
		'
		'GroupControl1
		'
		Me.GroupControl1.Controls.Add(Me.grdTableContent)
		Me.GroupControl1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.GroupControl1.Location = New System.Drawing.Point(5, 5)
		Me.GroupControl1.Name = "GroupControl1"
		Me.GroupControl1.Size = New System.Drawing.Size(809, 502)
		Me.GroupControl1.TabIndex = 319
		Me.GroupControl1.Text = "Liste der erstellten Zahlungen"
		'
		'ucEmployeeTables
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.Controls.Add(Me.scMain)
		Me.Controls.Add(Me.PanelControl1)
		Me.Name = "ucEmployeeTables"
		Me.Padding = New System.Windows.Forms.Padding(5)
		Me.Size = New System.Drawing.Size(829, 595)
		CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.PanelControl1.ResumeLayout(False)
		CType(Me.grdTableContent, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvTableContent, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.scMain, System.ComponentModel.ISupportInitialize).EndInit()
		Me.scMain.ResumeLayout(False)
		CType(Me.GroupControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupControl1.ResumeLayout(False)
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
	Friend WithEvents btnDelete As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents btnSave As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents rlblHeader As DevComponents.DotNetBar.Controls.ReflectionLabel
	Friend WithEvents grdTableContent As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvTableContent As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents scMain As DevExpress.XtraEditors.SplitContainerControl
	Friend WithEvents GroupControl1 As DevExpress.XtraEditors.GroupControl

End Class
