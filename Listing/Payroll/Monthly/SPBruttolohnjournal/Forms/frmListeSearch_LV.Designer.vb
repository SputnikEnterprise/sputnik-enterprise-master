<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmListeSearch_LV
	Inherits DevExpress.XtraEditors.XtraForm

	'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
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

	'Wird vom Windows Form-Designer benötigt.
	Private components As System.ComponentModel.IContainer

	'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
	'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
	'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
	<System.Diagnostics.DebuggerStepThrough()> _
	Private Sub InitializeComponent()
		Me.pnlMain = New DevExpress.XtraEditors.PanelControl()
		Me.XtraTabControl1 = New DevExpress.XtraTab.XtraTabControl()
		Me.xtabDetails = New DevExpress.XtraTab.XtraTabPage()
		Me.grdRP = New DevExpress.XtraGrid.GridControl()
		Me.gvRP = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.xtabSummery = New DevExpress.XtraTab.XtraTabPage()
		Me.grdSummery = New DevExpress.XtraGrid.GridControl()
		Me.gvSummery = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
		Me.Bar3 = New DevExpress.XtraBars.Bar()
		Me.bsiInfo = New DevExpress.XtraBars.BarStaticItem()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager()
		Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
		CType(Me.pnlMain, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.pnlMain.SuspendLayout()
		CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.XtraTabControl1.SuspendLayout()
		Me.xtabDetails.SuspendLayout()
		CType(Me.grdRP, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvRP, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.xtabSummery.SuspendLayout()
		CType(Me.grdSummery, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvSummery, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'pnlMain
		'
		Me.pnlMain.Controls.Add(Me.XtraTabControl1)
		Me.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill
		Me.pnlMain.Location = New System.Drawing.Point(0, 0)
		Me.pnlMain.Name = "pnlMain"
		Me.pnlMain.Size = New System.Drawing.Size(414, 611)
		Me.pnlMain.TabIndex = 221
		'
		'XtraTabControl1
		'
		Me.XtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.XtraTabControl1.Location = New System.Drawing.Point(2, 2)
		Me.XtraTabControl1.Name = "XtraTabControl1"
		Me.XtraTabControl1.SelectedTabPage = Me.xtabDetails
		Me.XtraTabControl1.Size = New System.Drawing.Size(410, 607)
		Me.XtraTabControl1.TabIndex = 5
		Me.XtraTabControl1.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabDetails, Me.xtabSummery})
		'
		'xtabDetails
		'
		Me.xtabDetails.Controls.Add(Me.grdRP)
		Me.xtabDetails.Name = "xtabDetails"
		Me.xtabDetails.Size = New System.Drawing.Size(404, 579)
		Me.xtabDetails.Text = "Detail"
		'
		'grdRP
		'
		Me.grdRP.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdRP.Location = New System.Drawing.Point(0, 0)
		Me.grdRP.MainView = Me.gvRP
		Me.grdRP.Name = "grdRP"
		Me.grdRP.Size = New System.Drawing.Size(404, 579)
		Me.grdRP.TabIndex = 4
		Me.grdRP.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvRP})
		'
		'gvRP
		'
		Me.gvRP.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvRP.GridControl = Me.grdRP
		Me.gvRP.Name = "gvRP"
		Me.gvRP.OptionsBehavior.Editable = False
		Me.gvRP.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvRP.OptionsView.ShowAutoFilterRow = True
		Me.gvRP.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		Me.gvRP.OptionsView.ShowGroupPanel = False
		Me.gvRP.OptionsView.ShowIndicator = False
		'
		'xtabSummery
		'
		Me.xtabSummery.Controls.Add(Me.grdSummery)
		Me.xtabSummery.Name = "xtabSummery"
		Me.xtabSummery.Size = New System.Drawing.Size(404, 579)
		Me.xtabSummery.Text = "Zusammenfassung"
		'
		'grdSummery
		'
		Me.grdSummery.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdSummery.Location = New System.Drawing.Point(0, 0)
		Me.grdSummery.MainView = Me.gvSummery
		Me.grdSummery.Name = "grdSummery"
		Me.grdSummery.Size = New System.Drawing.Size(404, 579)
		Me.grdSummery.TabIndex = 5
		Me.grdSummery.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvSummery})
		'
		'gvSummery
		'
		Me.gvSummery.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvSummery.GridControl = Me.grdSummery
		Me.gvSummery.Name = "gvSummery"
		Me.gvSummery.OptionsBehavior.Editable = False
		Me.gvSummery.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvSummery.OptionsView.ShowAutoFilterRow = True
		Me.gvSummery.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		Me.gvSummery.OptionsView.ShowGroupPanel = False
		Me.gvSummery.OptionsView.ShowIndicator = False
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 611)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 611)
		Me.barDockControlBottom.Size = New System.Drawing.Size(414, 25)
		'
		'barDockControlTop
		'
		Me.barDockControlTop.CausesValidation = False
		Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
		Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlTop.Size = New System.Drawing.Size(414, 0)
		'
		'Bar3
		'
		Me.Bar3.BarName = "Statusleiste"
		Me.Bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
		Me.Bar3.DockCol = 0
		Me.Bar3.DockRow = 0
		Me.Bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
		Me.Bar3.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiInfo)})
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
		'BarManager1
		'
		Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar3})
		Me.BarManager1.DockControls.Add(Me.barDockControlTop)
		Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
		Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
		Me.BarManager1.DockControls.Add(Me.barDockControlRight)
		Me.BarManager1.Form = Me
		Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiInfo})
		Me.BarManager1.MaxItemId = 9
		Me.BarManager1.StatusBar = Me.Bar3
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(414, 0)
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 611)
		'
		'frmListeSearch_LV
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(414, 636)
		Me.Controls.Add(Me.pnlMain)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.Name = "frmListeSearch_LV"
		Me.ShowIcon = False
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Text = "Detail"
		CType(Me.pnlMain, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pnlMain.ResumeLayout(False)
		CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.XtraTabControl1.ResumeLayout(False)
		Me.xtabDetails.ResumeLayout(False)
		CType(Me.grdRP, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvRP, System.ComponentModel.ISupportInitialize).EndInit()
		Me.xtabSummery.ResumeLayout(False)
		CType(Me.grdSummery, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvSummery, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents pnlMain As DevExpress.XtraEditors.PanelControl
	Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
	Friend WithEvents Bar3 As DevExpress.XtraBars.Bar
	Friend WithEvents bsiInfo As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
	Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
	Friend WithEvents grdRP As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvRP As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents XtraTabControl1 As DevExpress.XtraTab.XtraTabControl
	Friend WithEvents xtabDetails As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents xtabSummery As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents grdSummery As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvSummery As DevExpress.XtraGrid.Views.Grid.GridView
End Class
