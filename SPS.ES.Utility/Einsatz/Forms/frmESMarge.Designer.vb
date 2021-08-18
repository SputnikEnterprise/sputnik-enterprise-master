'<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
'Partial Class frmESMarge
'	'Inherits DevComponents.DotNetBar.Metro.MetroForm
'	Inherits DevExpress.XtraEditors.XtraForm

'	'Form overrides dispose to clean up the component list.
'	<System.Diagnostics.DebuggerNonUserCode()> _
'  Protected Overrides Sub Dispose(ByVal disposing As Boolean)
'    Try
'      If disposing AndAlso components IsNot Nothing Then
'        components.Dispose()
'      End If
'    Finally
'      MyBase.Dispose(disposing)
'    End Try
'  End Sub

'  'Required by the Windows Form Designer
'  Private components As System.ComponentModel.IContainer

'  'NOTE: The following procedure is required by the Windows Form Designer
'  'It can be modified using the Windows Form Designer.  
'  'Do not modify it using the code editor.
'  <System.Diagnostics.DebuggerStepThrough()> _
'  Private Sub InitializeComponent()
'    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmESMarge))
'    Me.btnClose = New DevExpress.XtraEditors.SimpleButton()
'    Me.grdFoundedrec = New DevExpress.XtraGrid.GridControl()
'    Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView()
'    Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
'    Me.XtraTabControl1 = New DevExpress.XtraTab.XtraTabControl()
'    Me.XtraTabPage1 = New DevExpress.XtraTab.XtraTabPage()
'    Me.sccMain_0 = New DevExpress.XtraEditors.SplitContainerControl()
'    Me.sccMain_1 = New DevExpress.XtraEditors.SplitContainerControl()
'    Me.lvESDetail = New DevComponents.DotNetBar.Controls.ListViewEx()
'    Me.lvFinalData = New DevComponents.DotNetBar.Controls.ListViewEx()
'    Me.XtraTabPage2 = New DevExpress.XtraTab.XtraTabPage()
'    Me.grdFoundedDetailrec = New DevExpress.XtraGrid.GridControl()
'    Me.GridView2 = New DevExpress.XtraGrid.Views.Grid.GridView()
'    CType(Me.grdFoundedrec, System.ComponentModel.ISupportInitialize).BeginInit()
'    CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
'    CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
'    Me.PanelControl1.SuspendLayout()
'    CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
'    Me.XtraTabControl1.SuspendLayout()
'    Me.XtraTabPage1.SuspendLayout()
'    CType(Me.sccMain_0, System.ComponentModel.ISupportInitialize).BeginInit()
'    Me.sccMain_0.SuspendLayout()
'    CType(Me.sccMain_1, System.ComponentModel.ISupportInitialize).BeginInit()
'    Me.sccMain_1.SuspendLayout()
'    Me.XtraTabPage2.SuspendLayout()
'    CType(Me.grdFoundedDetailrec, System.ComponentModel.ISupportInitialize).BeginInit()
'    CType(Me.GridView2, System.ComponentModel.ISupportInitialize).BeginInit()
'    Me.SuspendLayout()
'    '
'    'btnClose
'    '
'    Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
'    Me.btnClose.Location = New System.Drawing.Point(545, 26)
'    Me.btnClose.Name = "btnClose"
'    Me.btnClose.Size = New System.Drawing.Size(96, 27)
'    Me.btnClose.TabIndex = 0
'    Me.btnClose.Text = "Schliessen"
'    '
'    'grdFoundedrec
'    '
'    Me.grdFoundedrec.Dock = System.Windows.Forms.DockStyle.Fill
'    Me.grdFoundedrec.Location = New System.Drawing.Point(0, 0)
'    Me.grdFoundedrec.MainView = Me.GridView1
'    Me.grdFoundedrec.Name = "grdFoundedrec"
'    Me.grdFoundedrec.Size = New System.Drawing.Size(528, 282)
'    Me.grdFoundedrec.TabIndex = 155
'    Me.grdFoundedrec.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
'    '
'    'GridView1
'    '
'    Me.GridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None
'    Me.GridView1.GridControl = Me.grdFoundedrec
'    Me.GridView1.Name = "GridView1"
'    Me.GridView1.OptionsBehavior.Editable = False
'    Me.GridView1.OptionsFind.AlwaysVisible = True
'    Me.GridView1.OptionsSelection.EnableAppearanceFocusedCell = False
'    Me.GridView1.OptionsView.ShowGroupedColumns = True
'    Me.GridView1.OptionsView.ShowGroupPanel = False
'    Me.GridView1.OptionsView.WaitAnimationOptions = DevExpress.XtraEditors.WaitAnimationOptions.Panel
'		Me.GridView1.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways
'    '
'    'PanelControl1
'    '
'    Me.PanelControl1.Controls.Add(Me.XtraTabControl1)
'    Me.PanelControl1.Controls.Add(Me.btnClose)
'    Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Fill
'    Me.PanelControl1.Location = New System.Drawing.Point(0, 0)
'    Me.PanelControl1.Name = "PanelControl1"
'    Me.PanelControl1.Size = New System.Drawing.Size(646, 527)
'    Me.PanelControl1.TabIndex = 156
'    '
'    'XtraTabControl1
'    '
'    Me.XtraTabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
'            Or System.Windows.Forms.AnchorStyles.Left) _
'            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
'    Me.XtraTabControl1.Location = New System.Drawing.Point(5, 5)
'    Me.XtraTabControl1.Name = "XtraTabControl1"
'    Me.XtraTabControl1.SelectedTabPage = Me.XtraTabPage1
'    Me.XtraTabControl1.Size = New System.Drawing.Size(534, 522)
'    Me.XtraTabControl1.TabIndex = 156
'    Me.XtraTabControl1.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.XtraTabPage1, Me.XtraTabPage2})
'    '
'    'XtraTabPage1
'    '
'    Me.XtraTabPage1.Controls.Add(Me.sccMain_0)
'    Me.XtraTabPage1.Name = "XtraTabPage1"
'    Me.XtraTabPage1.Size = New System.Drawing.Size(528, 494)
'    Me.XtraTabPage1.Text = "Zusammenfassung"
'    '
'    'sccMain_0
'    '
'    Me.sccMain_0.Dock = System.Windows.Forms.DockStyle.Fill
'    Me.sccMain_0.Horizontal = False
'    Me.sccMain_0.Location = New System.Drawing.Point(0, 0)
'    Me.sccMain_0.Name = "sccMain_0"
'    Me.sccMain_0.Panel1.Controls.Add(Me.sccMain_1)
'    Me.sccMain_0.Panel1.Text = "Panel1"
'    Me.sccMain_0.Panel2.Controls.Add(Me.lvFinalData)
'    Me.sccMain_0.Panel2.Text = "Panel2"
'    Me.sccMain_0.Size = New System.Drawing.Size(528, 494)
'    Me.sccMain_0.SplitterPosition = 368
'    Me.sccMain_0.TabIndex = 156
'    Me.sccMain_0.Text = "SplitContainerControl1"
'    '
'    'sccMain_1
'    '
'    Me.sccMain_1.Dock = System.Windows.Forms.DockStyle.Fill
'    Me.sccMain_1.Horizontal = False
'    Me.sccMain_1.Location = New System.Drawing.Point(0, 0)
'    Me.sccMain_1.Name = "sccMain_1"
'    Me.sccMain_1.Panel1.Controls.Add(Me.lvESDetail)
'    Me.sccMain_1.Panel1.Text = "Panel1"
'    Me.sccMain_1.Panel2.Controls.Add(Me.grdFoundedrec)
'    Me.sccMain_1.Panel2.Text = "Panel2"
'    Me.sccMain_1.Size = New System.Drawing.Size(528, 368)
'    Me.sccMain_1.SplitterPosition = 81
'    Me.sccMain_1.TabIndex = 156
'    Me.sccMain_1.Text = "SplitContainerControl2"
'    '
'    'lvESDetail
'    '
'    Me.lvESDetail.BackColor = System.Drawing.Color.White
'    '
'    '
'    '
'    Me.lvESDetail.Border.Class = "ListViewBorder"
'    Me.lvESDetail.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
'    Me.lvESDetail.Dock = System.Windows.Forms.DockStyle.Fill
'    Me.lvESDetail.ForeColor = System.Drawing.Color.Black
'    Me.lvESDetail.FullRowSelect = True
'    Me.lvESDetail.HideSelection = False
'    Me.lvESDetail.Location = New System.Drawing.Point(0, 0)
'    Me.lvESDetail.Name = "lvESDetail"
'    Me.lvESDetail.Size = New System.Drawing.Size(528, 81)
'    Me.lvESDetail.TabIndex = 2
'    Me.lvESDetail.UseCompatibleStateImageBehavior = False
'    Me.lvESDetail.View = System.Windows.Forms.View.Details
'    '
'    'lvFinalData
'    '
'    Me.lvFinalData.BackColor = System.Drawing.Color.White
'    '
'    '
'    '
'    Me.lvFinalData.Border.Class = "ListViewBorder"
'    Me.lvFinalData.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
'    Me.lvFinalData.Dock = System.Windows.Forms.DockStyle.Fill
'    Me.lvFinalData.ForeColor = System.Drawing.Color.Black
'    Me.lvFinalData.FullRowSelect = True
'    Me.lvFinalData.HideSelection = False
'    Me.lvFinalData.Location = New System.Drawing.Point(0, 0)
'    Me.lvFinalData.Name = "lvFinalData"
'    Me.lvFinalData.Size = New System.Drawing.Size(528, 121)
'    Me.lvFinalData.TabIndex = 1
'    Me.lvFinalData.UseCompatibleStateImageBehavior = False
'    Me.lvFinalData.View = System.Windows.Forms.View.Details
'    '
'    'XtraTabPage2
'    '
'    Me.XtraTabPage2.Controls.Add(Me.grdFoundedDetailrec)
'    Me.XtraTabPage2.Name = "XtraTabPage2"
'    Me.XtraTabPage2.Size = New System.Drawing.Size(528, 494)
'    Me.XtraTabPage2.Text = "Detail über Arbeitgeberanteil"
'    '
'    'grdFoundedDetailrec
'    '
'    Me.grdFoundedDetailrec.Dock = System.Windows.Forms.DockStyle.Fill
'    Me.grdFoundedDetailrec.Location = New System.Drawing.Point(0, 0)
'    Me.grdFoundedDetailrec.MainView = Me.GridView2
'    Me.grdFoundedDetailrec.Name = "grdFoundedDetailrec"
'    Me.grdFoundedDetailrec.Size = New System.Drawing.Size(528, 371)
'    Me.grdFoundedDetailrec.TabIndex = 156
'    Me.grdFoundedDetailrec.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView2})
'    '
'    'GridView2
'    '
'    Me.GridView2.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None
'    Me.GridView2.GridControl = Me.grdFoundedDetailrec
'    Me.GridView2.Name = "GridView2"
'    Me.GridView2.OptionsBehavior.Editable = False
'    Me.GridView2.OptionsFind.AlwaysVisible = True
'    Me.GridView2.OptionsSelection.EnableAppearanceFocusedCell = False
'    Me.GridView2.OptionsSelection.UseIndicatorForSelection = False
'    Me.GridView2.OptionsView.ShowGroupedColumns = True
'    Me.GridView2.OptionsView.ShowGroupPanel = False
'    Me.GridView2.OptionsView.WaitAnimationOptions = DevExpress.XtraEditors.WaitAnimationOptions.Panel
'		Me.GridView2.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways
'    '
'    'frmESMarge
'    '
'    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
'    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
'    Me.ClientSize = New System.Drawing.Size(646, 527)
'    Me.Controls.Add(Me.PanelControl1)
'    Me.DoubleBuffered = True
'    Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
'    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
'    Me.Name = "frmESMarge"
'    Me.Text = "Darstellung der Bruttomarge"
'    CType(Me.grdFoundedrec, System.ComponentModel.ISupportInitialize).EndInit()
'    CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
'    CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
'    Me.PanelControl1.ResumeLayout(False)
'    CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).EndInit()
'    Me.XtraTabControl1.ResumeLayout(False)
'    Me.XtraTabPage1.ResumeLayout(False)
'    CType(Me.sccMain_0, System.ComponentModel.ISupportInitialize).EndInit()
'    Me.sccMain_0.ResumeLayout(False)
'    CType(Me.sccMain_1, System.ComponentModel.ISupportInitialize).EndInit()
'    Me.sccMain_1.ResumeLayout(False)
'    Me.XtraTabPage2.ResumeLayout(False)
'    CType(Me.grdFoundedDetailrec, System.ComponentModel.ISupportInitialize).EndInit()
'    CType(Me.GridView2, System.ComponentModel.ISupportInitialize).EndInit()
'    Me.ResumeLayout(False)

'  End Sub
'  Friend WithEvents btnClose As DevExpress.XtraEditors.SimpleButton
'  Friend WithEvents grdFoundedrec As DevExpress.XtraGrid.GridControl
'  Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
'  Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
'  Friend WithEvents XtraTabControl1 As DevExpress.XtraTab.XtraTabControl
'  Friend WithEvents XtraTabPage1 As DevExpress.XtraTab.XtraTabPage
'  Friend WithEvents XtraTabPage2 As DevExpress.XtraTab.XtraTabPage
'  Friend WithEvents grdFoundedDetailrec As DevExpress.XtraGrid.GridControl
'  Friend WithEvents GridView2 As DevExpress.XtraGrid.Views.Grid.GridView
'  Friend WithEvents sccMain_0 As DevExpress.XtraEditors.SplitContainerControl
'  Friend WithEvents lvFinalData As DevComponents.DotNetBar.Controls.ListViewEx
'  Friend WithEvents sccMain_1 As DevExpress.XtraEditors.SplitContainerControl
'  Friend WithEvents lvESDetail As DevComponents.DotNetBar.Controls.ListViewEx
'End Class
