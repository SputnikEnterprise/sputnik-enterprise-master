<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmREDetails
	Inherits DevExpress.XtraEditors.XtraForm

	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> _
  Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    Try
      If disposing AndAlso components IsNot Nothing Then
        components.Dispose()
      End If
    Finally
      MyBase.Dispose(disposing)
    End Try
  End Sub

  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer

  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.  
  'Do not modify it using the code editor.
  <System.Diagnostics.DebuggerStepThrough()> _
  Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmREDetails))
		Me.grdDetailrec = New DevExpress.XtraGrid.GridControl()
		Me.gvDetail = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.sccMain = New DevExpress.XtraEditors.SplitContainerControl()
		Me.chkSelMA = New DevExpress.XtraEditors.CheckEdit()
		Me.Bar3 = New DevExpress.XtraBars.Bar()
		Me.bsiRecCount = New DevExpress.XtraBars.BarStaticItem()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
		Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
		Me.rlblDetailHeader = New DevExpress.XtraEditors.LabelControl()
		CType(Me.grdDetailrec, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvDetail, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.sccMain, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.sccMain.SuspendLayout()
		CType(Me.chkSelMA.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'grdDetailrec
		'
		Me.grdDetailrec.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdDetailrec.Location = New System.Drawing.Point(5, 5)
		Me.grdDetailrec.MainView = Me.gvDetail
		Me.grdDetailrec.Name = "grdDetailrec"
		Me.grdDetailrec.Size = New System.Drawing.Size(643, 356)
		Me.grdDetailrec.TabIndex = 3
		Me.grdDetailrec.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvDetail})
		'
		'gvDetail
		'
		Me.gvDetail.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvDetail.GridControl = Me.grdDetailrec
		Me.gvDetail.Name = "gvDetail"
		Me.gvDetail.OptionsBehavior.Editable = False
		Me.gvDetail.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvDetail.OptionsView.ShowAutoFilterRow = True
		Me.gvDetail.OptionsView.ShowGroupPanel = False
		'
		'sccMain
		'
		Me.sccMain.Dock = System.Windows.Forms.DockStyle.Fill
		Me.sccMain.Horizontal = False
		Me.sccMain.IsSplitterFixed = True
		Me.sccMain.Location = New System.Drawing.Point(0, 0)
		Me.sccMain.Name = "sccMain"
		Me.sccMain.Panel1.Controls.Add(Me.rlblDetailHeader)
		Me.sccMain.Panel1.Controls.Add(Me.chkSelMA)
		Me.sccMain.Panel1.Text = "Panel1"
		Me.sccMain.Panel2.Controls.Add(Me.grdDetailrec)
		Me.sccMain.Panel2.Padding = New System.Windows.Forms.Padding(5)
		Me.sccMain.Panel2.Text = "Panel2"
		Me.sccMain.Size = New System.Drawing.Size(653, 414)
		Me.sccMain.SplitterPosition = 38
		Me.sccMain.TabIndex = 4
		Me.sccMain.Text = "SplitContainerControl1"
		'
		'chkSelMA
		'
		Me.chkSelMA.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.chkSelMA.Location = New System.Drawing.Point(383, 12)
		Me.chkSelMA.Name = "chkSelMA"
		Me.chkSelMA.Properties.AllowFocused = False
		Me.chkSelMA.Properties.Appearance.Options.UseTextOptions = True
		Me.chkSelMA.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.chkSelMA.Properties.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
		Me.chkSelMA.Properties.Caption = "Daten für selektierte Rechnung anzeigen"
		Me.chkSelMA.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.chkSelMA.Size = New System.Drawing.Size(265, 20)
		Me.chkSelMA.TabIndex = 0
		'
		'Bar3
		'
		Me.Bar3.BarName = "Statusleiste"
		Me.Bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
		Me.Bar3.DockCol = 0
		Me.Bar3.DockRow = 0
		Me.Bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
		Me.Bar3.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiRecCount)})
		Me.Bar3.OptionsBar.AllowQuickCustomization = False
		Me.Bar3.OptionsBar.DrawDragBorder = False
		Me.Bar3.OptionsBar.UseWholeRow = True
		Me.Bar3.Text = "Statusleiste"
		'
		'bsiRecCount
		'
		Me.bsiRecCount.Caption = "Bereit"
		Me.bsiRecCount.Id = 0
		Me.bsiRecCount.Name = "bsiRecCount"
		'
		'BarManager1
		'
		Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar3})
		Me.BarManager1.DockControls.Add(Me.barDockControlTop)
		Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
		Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
		Me.BarManager1.DockControls.Add(Me.barDockControlRight)
		Me.BarManager1.Form = Me
		Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiRecCount})
		Me.BarManager1.MaxItemId = 1
		Me.BarManager1.StatusBar = Me.Bar3
		'
		'barDockControlTop
		'
		Me.barDockControlTop.CausesValidation = False
		Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
		Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlTop.Manager = Me.BarManager1
		Me.barDockControlTop.Size = New System.Drawing.Size(653, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 414)
		Me.barDockControlBottom.Manager = Me.BarManager1
		Me.barDockControlBottom.Size = New System.Drawing.Size(653, 22)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Manager = Me.BarManager1
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 414)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(653, 0)
		Me.barDockControlRight.Manager = Me.BarManager1
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 414)
		'
		'rlblDetailHeader
		'
		Me.rlblDetailHeader.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.[True]
		Me.rlblDetailHeader.AllowHtmlString = True
		Me.rlblDetailHeader.Appearance.Font = New System.Drawing.Font("Tahoma", 15.0!, System.Drawing.FontStyle.Bold)
		Me.rlblDetailHeader.Appearance.Options.UseFont = True
		Me.rlblDetailHeader.Appearance.Options.UseTextOptions = True
		Me.rlblDetailHeader.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter
		Me.rlblDetailHeader.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
		Me.rlblDetailHeader.AutoEllipsis = True
		Me.rlblDetailHeader.Location = New System.Drawing.Point(12, 12)
		Me.rlblDetailHeader.Name = "rlblDetailHeader"
		Me.rlblDetailHeader.Size = New System.Drawing.Size(132, 24)
		Me.rlblDetailHeader.TabIndex = 3
		Me.rlblDetailHeader.Text = "DetailHeader"
		'
		'frmREDetails
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(653, 436)
		Me.Controls.Add(Me.sccMain)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.DoubleBuffered = True
		Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.Name = "frmREDetails"
		Me.Text = "MetroForm"
		CType(Me.grdDetailrec, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvDetail, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.sccMain, System.ComponentModel.ISupportInitialize).EndInit()
		Me.sccMain.ResumeLayout(False)
		CType(Me.chkSelMA.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents grdDetailrec As DevExpress.XtraGrid.GridControl
  Friend WithEvents gvDetail As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents sccMain As DevExpress.XtraEditors.SplitContainerControl
  Friend WithEvents chkSelMA As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents Bar3 As DevExpress.XtraBars.Bar
	Friend WithEvents bsiRecCount As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
	Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
	Friend WithEvents rlblDetailHeader As DevExpress.XtraEditors.LabelControl
End Class
