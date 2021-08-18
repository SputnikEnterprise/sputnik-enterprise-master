<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmListeSearch_LV
	Inherits DevExpress.XtraEditors.XtraForm

	'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
	<System.Diagnostics.DebuggerNonUserCode()>
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
	<System.Diagnostics.DebuggerStepThrough()>
	Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim GridLevelNode1 As DevExpress.XtraGrid.GridLevelNode = New DevExpress.XtraGrid.GridLevelNode()
		Me.gvLevel1 = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.grdPayroll_Staging = New DevExpress.XtraGrid.GridControl()
		Me.gvPayroll_Staging = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.pnlMain = New DevExpress.XtraEditors.PanelControl()
		Me.xtabDetailData = New DevExpress.XtraTab.XtraTabControl()
		Me.xtabAllgemein = New DevExpress.XtraTab.XtraTabPage()
		Me.grdRP = New DevExpress.XtraGrid.GridControl()
		Me.gvRP = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.xtabPayroll = New DevExpress.XtraTab.XtraTabPage()
		Me.xtabInvoices = New DevExpress.XtraTab.XtraTabPage()
		Me.grdInvoiceData = New DevExpress.XtraGrid.GridControl()
		Me.gvInvoiceData = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.xtabCustomerJournal = New DevExpress.XtraTab.XtraTabPage()
		Me.grdCustomerJournal = New DevExpress.XtraGrid.GridControl()
		Me.gvCustomerJournal = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
		Me.Bar3 = New DevExpress.XtraBars.Bar()
		Me.bsiInfo = New DevExpress.XtraBars.BarStaticItem()
		Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
		CType(Me.gvLevel1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.grdPayroll_Staging, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvPayroll_Staging, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.pnlMain, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.pnlMain.SuspendLayout()
		CType(Me.xtabDetailData, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.xtabDetailData.SuspendLayout()
		Me.xtabAllgemein.SuspendLayout()
		CType(Me.grdRP, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvRP, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.xtabPayroll.SuspendLayout()
		Me.xtabInvoices.SuspendLayout()
		CType(Me.grdInvoiceData, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvInvoiceData, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.xtabCustomerJournal.SuspendLayout()
		CType(Me.grdCustomerJournal, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvCustomerJournal, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'gvLevel1
		'
		Me.gvLevel1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
		Me.gvLevel1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvLevel1.GridControl = Me.grdPayroll_Staging
		Me.gvLevel1.Name = "gvLevel1"
		'
		'grdPayroll_Staging
		'
		Me.grdPayroll_Staging.Dock = System.Windows.Forms.DockStyle.Fill
		GridLevelNode1.LevelTemplate = Me.gvLevel1
		GridLevelNode1.RelationName = "DataDetails"
		Me.grdPayroll_Staging.LevelTree.Nodes.AddRange(New DevExpress.XtraGrid.GridLevelNode() {GridLevelNode1})
		Me.grdPayroll_Staging.Location = New System.Drawing.Point(5, 5)
		Me.grdPayroll_Staging.MainView = Me.gvPayroll_Staging
		Me.grdPayroll_Staging.Name = "grdPayroll_Staging"
		Me.grdPayroll_Staging.Size = New System.Drawing.Size(398, 575)
		Me.grdPayroll_Staging.TabIndex = 6
		Me.grdPayroll_Staging.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvPayroll_Staging, Me.gvLevel1})
		'
		'gvPayroll_Staging
		'
		Me.gvPayroll_Staging.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvPayroll_Staging.GridControl = Me.grdPayroll_Staging
		Me.gvPayroll_Staging.Name = "gvPayroll_Staging"
		Me.gvPayroll_Staging.OptionsBehavior.Editable = False
		Me.gvPayroll_Staging.OptionsDetail.ShowDetailTabs = False
		Me.gvPayroll_Staging.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvPayroll_Staging.OptionsView.ShowAutoFilterRow = True
		Me.gvPayroll_Staging.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		Me.gvPayroll_Staging.OptionsView.ShowGroupPanel = False
		Me.gvPayroll_Staging.OptionsView.ShowIndicator = False
		'
		'pnlMain
		'
		Me.pnlMain.Controls.Add(Me.xtabDetailData)
		Me.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill
		Me.pnlMain.Location = New System.Drawing.Point(0, 0)
		Me.pnlMain.Name = "pnlMain"
		Me.pnlMain.Size = New System.Drawing.Size(414, 614)
		Me.pnlMain.TabIndex = 221
		'
		'xtabDetailData
		'
		Me.xtabDetailData.Dock = System.Windows.Forms.DockStyle.Fill
		Me.xtabDetailData.Location = New System.Drawing.Point(2, 2)
		Me.xtabDetailData.Name = "xtabDetailData"
		Me.xtabDetailData.SelectedTabPage = Me.xtabAllgemein
		Me.xtabDetailData.Size = New System.Drawing.Size(410, 610)
		Me.xtabDetailData.TabIndex = 5
		Me.xtabDetailData.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabAllgemein, Me.xtabPayroll, Me.xtabInvoices, Me.xtabCustomerJournal})
		'
		'xtabAllgemein
		'
		Me.xtabAllgemein.Controls.Add(Me.grdRP)
		Me.xtabAllgemein.Name = "xtabAllgemein"
		Me.xtabAllgemein.Padding = New System.Windows.Forms.Padding(5)
		Me.xtabAllgemein.Size = New System.Drawing.Size(408, 585)
		Me.xtabAllgemein.Text = "Allgemein"
		'
		'grdRP
		'
		Me.grdRP.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdRP.Location = New System.Drawing.Point(5, 5)
		Me.grdRP.MainView = Me.gvRP
		Me.grdRP.Name = "grdRP"
		Me.grdRP.Size = New System.Drawing.Size(398, 575)
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
		'xtabPayroll
		'
		Me.xtabPayroll.Controls.Add(Me.grdPayroll_Staging)
		Me.xtabPayroll.Name = "xtabPayroll"
		Me.xtabPayroll.Padding = New System.Windows.Forms.Padding(5)
		Me.xtabPayroll.Size = New System.Drawing.Size(408, 585)
		Me.xtabPayroll.Text = "Lohnaufwände"
		'
		'xtabInvoices
		'
		Me.xtabInvoices.Controls.Add(Me.grdInvoiceData)
		Me.xtabInvoices.Name = "xtabInvoices"
		Me.xtabInvoices.Padding = New System.Windows.Forms.Padding(5)
		Me.xtabInvoices.Size = New System.Drawing.Size(408, 585)
		Me.xtabInvoices.Text = "Umsätze"
		'
		'grdInvoiceData
		'
		Me.grdInvoiceData.Cursor = System.Windows.Forms.Cursors.Default
		Me.grdInvoiceData.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdInvoiceData.Location = New System.Drawing.Point(5, 5)
		Me.grdInvoiceData.MainView = Me.gvInvoiceData
		Me.grdInvoiceData.Name = "grdInvoiceData"
		Me.grdInvoiceData.Size = New System.Drawing.Size(398, 575)
		Me.grdInvoiceData.TabIndex = 6
		Me.grdInvoiceData.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvInvoiceData})
		'
		'gvInvoiceData
		'
		Me.gvInvoiceData.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvInvoiceData.GridControl = Me.grdInvoiceData
		Me.gvInvoiceData.Name = "gvInvoiceData"
		Me.gvInvoiceData.OptionsBehavior.Editable = False
		Me.gvInvoiceData.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvInvoiceData.OptionsView.ShowAutoFilterRow = True
		Me.gvInvoiceData.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		Me.gvInvoiceData.OptionsView.ShowGroupPanel = False
		Me.gvInvoiceData.OptionsView.ShowIndicator = False
		'
		'xtabCustomerJournal
		'
		Me.xtabCustomerJournal.Controls.Add(Me.grdCustomerJournal)
		Me.xtabCustomerJournal.Name = "xtabCustomerJournal"
		Me.xtabCustomerJournal.Padding = New System.Windows.Forms.Padding(5)
		Me.xtabCustomerJournal.Size = New System.Drawing.Size(408, 585)
		Me.xtabCustomerJournal.Text = "Kunden-Journal"
		'
		'grdCustomerJournal
		'
		Me.grdCustomerJournal.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdCustomerJournal.Location = New System.Drawing.Point(5, 5)
		Me.grdCustomerJournal.MainView = Me.gvCustomerJournal
		Me.grdCustomerJournal.Name = "grdCustomerJournal"
		Me.grdCustomerJournal.Size = New System.Drawing.Size(398, 575)
		Me.grdCustomerJournal.TabIndex = 5
		Me.grdCustomerJournal.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvCustomerJournal})
		'
		'gvCustomerJournal
		'
		Me.gvCustomerJournal.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvCustomerJournal.GridControl = Me.grdCustomerJournal
		Me.gvCustomerJournal.Name = "gvCustomerJournal"
		Me.gvCustomerJournal.OptionsBehavior.Editable = False
		Me.gvCustomerJournal.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvCustomerJournal.OptionsView.ShowAutoFilterRow = True
		Me.gvCustomerJournal.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		Me.gvCustomerJournal.OptionsView.ShowGroupPanel = False
		Me.gvCustomerJournal.OptionsView.ShowIndicator = False
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Manager = Me.BarManager1
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 614)
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
		Me.BarManager1.MaxItemId = 11
		Me.BarManager1.StatusBar = Me.Bar3
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
		Me.bsiInfo.Size = New System.Drawing.Size(32, 0)
		Me.bsiInfo.Width = 32
		'
		'barDockControlTop
		'
		Me.barDockControlTop.CausesValidation = False
		Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
		Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlTop.Manager = Me.BarManager1
		Me.barDockControlTop.Size = New System.Drawing.Size(414, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 614)
		Me.barDockControlBottom.Manager = Me.BarManager1
		Me.barDockControlBottom.Size = New System.Drawing.Size(414, 22)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(414, 0)
		Me.barDockControlRight.Manager = Me.BarManager1
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 614)
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
		Me.IconOptions.ShowIcon = False
		Me.Name = "frmListeSearch_LV"
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Text = "Detail"
		CType(Me.gvLevel1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.grdPayroll_Staging, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvPayroll_Staging, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.pnlMain, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pnlMain.ResumeLayout(False)
		CType(Me.xtabDetailData, System.ComponentModel.ISupportInitialize).EndInit()
		Me.xtabDetailData.ResumeLayout(False)
		Me.xtabAllgemein.ResumeLayout(False)
		CType(Me.grdRP, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvRP, System.ComponentModel.ISupportInitialize).EndInit()
		Me.xtabPayroll.ResumeLayout(False)
		Me.xtabInvoices.ResumeLayout(False)
		CType(Me.grdInvoiceData, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvInvoiceData, System.ComponentModel.ISupportInitialize).EndInit()
		Me.xtabCustomerJournal.ResumeLayout(False)
		CType(Me.grdCustomerJournal, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvCustomerJournal, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

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
	Friend WithEvents xtabDetailData As DevExpress.XtraTab.XtraTabControl
	Friend WithEvents xtabAllgemein As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents xtabInvoices As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents grdInvoiceData As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvInvoiceData As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents xtabCustomerJournal As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents grdCustomerJournal As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvCustomerJournal As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents xtabPayroll As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents grdPayroll_Staging As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvPayroll_Staging As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents gvLevel1 As DevExpress.XtraGrid.Views.Grid.GridView
End Class
