<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUpdate
	'Inherits DevComponents.DotNetBar.Metro.MetroForm
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUpdate))
		Me.grdDetailrec = New DevExpress.XtraGrid.GridControl()
		Me.gvDetail = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.sccMain = New DevExpress.XtraEditors.SplitContainerControl()
		Me.Bar3 = New DevExpress.XtraBars.Bar()
		Me.bsiRecCount = New DevExpress.XtraBars.BarStaticItem()
		Me.bbiOpenUpdatePDF = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiRunUpdate = New DevExpress.XtraBars.BarButtonItem()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
		Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
		CType(Me.grdDetailrec, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvDetail, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.sccMain, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.sccMain.SuspendLayout()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'grdDetailrec
		'
		Me.grdDetailrec.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdDetailrec.Location = New System.Drawing.Point(5, 5)
		Me.grdDetailrec.MainView = Me.gvDetail
		Me.grdDetailrec.Name = "grdDetailrec"
		Me.grdDetailrec.Size = New System.Drawing.Size(643, 310)
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
		Me.sccMain.Horizontal = False
		Me.sccMain.Location = New System.Drawing.Point(0, 0)
		Me.sccMain.Name = "sccMain"
		Me.sccMain.Panel1.Text = "Panel1"
		Me.sccMain.Panel2.Controls.Add(Me.grdDetailrec)
		Me.sccMain.Panel2.Padding = New System.Windows.Forms.Padding(5)
		Me.sccMain.Panel2.Text = "Panel2"
		Me.sccMain.Size = New System.Drawing.Size(653, 363)
		Me.sccMain.SplitterPosition = 38
		Me.sccMain.TabIndex = 4
		Me.sccMain.Text = "SplitContainerControl1"
		'
		'Bar3
		'
		Me.Bar3.BarName = "Statusleiste"
		Me.Bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
		Me.Bar3.DockCol = 0
		Me.Bar3.DockRow = 0
		Me.Bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
		Me.Bar3.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiRecCount), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiOpenUpdatePDF), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiRunUpdate)})
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
		'bbiOpenUpdatePDF
		'
		Me.bbiOpenUpdatePDF.Caption = "Updateprotokoll anzeigen"
		Me.bbiOpenUpdatePDF.Id = 1
		Me.bbiOpenUpdatePDF.ImageOptions.Image = CType(resources.GetObject("bbiOpenUpdatePDF.ImageOptions.Image"), System.Drawing.Image)
		Me.bbiOpenUpdatePDF.Name = "bbiOpenUpdatePDF"
		Me.bbiOpenUpdatePDF.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiRunUpdate
		'
		Me.bbiRunUpdate.Caption = "Update ausführen"
		Me.bbiRunUpdate.Id = 2
		Me.bbiRunUpdate.ImageOptions.Image = CType(resources.GetObject("bbiRunUpdate.ImageOptions.Image"), System.Drawing.Image)
		Me.bbiRunUpdate.Name = "bbiRunUpdate"
		Me.bbiRunUpdate.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'BarManager1
		'
		Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar3})
		Me.BarManager1.DockControls.Add(Me.barDockControlTop)
		Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
		Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
		Me.BarManager1.DockControls.Add(Me.barDockControlRight)
		Me.BarManager1.Form = Me
		Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiRecCount, Me.bbiOpenUpdatePDF, Me.bbiRunUpdate})
		Me.BarManager1.MaxItemId = 3
		Me.BarManager1.StatusBar = Me.Bar3
		'
		'barDockControlTop
		'
		Me.barDockControlTop.CausesValidation = False
		Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
		Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlTop.Manager = Me.BarManager1
		Me.barDockControlTop.Size = New System.Drawing.Size(645, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 409)
		Me.barDockControlBottom.Manager = Me.BarManager1
		Me.barDockControlBottom.Size = New System.Drawing.Size(645, 27)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Manager = Me.BarManager1
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 409)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(645, 0)
		Me.barDockControlRight.Manager = Me.BarManager1
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 409)
		'
		'frmUpdate
		'
		Me.Appearance.Options.UseFont = True
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(645, 436)
		Me.Controls.Add(Me.sccMain)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.IconOptions.Icon = CType(resources.GetObject("frmUpdate.IconOptions.Icon"), System.Drawing.Icon)
		Me.Name = "frmUpdate"
		Me.Text = "Liste der Updates"
		CType(Me.grdDetailrec, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvDetail, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.sccMain, System.ComponentModel.ISupportInitialize).EndInit()
		Me.sccMain.ResumeLayout(False)
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents grdDetailrec As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvDetail As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents sccMain As DevExpress.XtraEditors.SplitContainerControl
	Friend WithEvents Bar3 As DevExpress.XtraBars.Bar
	Friend WithEvents bsiRecCount As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
	Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
	Friend WithEvents bbiOpenUpdatePDF As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbiRunUpdate As DevExpress.XtraBars.BarButtonItem
End Class
