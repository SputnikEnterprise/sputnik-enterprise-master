<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEmployeeQuickSearch
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmEmployeeQuickSearch))
		Me.grdMain = New DevExpress.XtraGrid.GridControl()
		Me.gvMain = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
		Me.Bar1 = New DevExpress.XtraBars.Bar()
		Me.bsiLblRecCount = New DevExpress.XtraBars.BarStaticItem()
		Me.bsiRecCount = New DevExpress.XtraBars.BarStaticItem()
		Me.BarStaticItem3 = New DevExpress.XtraBars.BarStaticItem()
		Me.bbiPrint = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiSMS = New DevExpress.XtraBars.BarButtonItem()
		Me.BarDockControl5 = New DevExpress.XtraBars.BarDockControl()
		Me.BarDockControl6 = New DevExpress.XtraBars.BarDockControl()
		Me.BarDockControl7 = New DevExpress.XtraBars.BarDockControl()
		Me.BarDockControl8 = New DevExpress.XtraBars.BarDockControl()
		Me.BarSubItem3 = New DevExpress.XtraBars.BarSubItem()
		Me.RepositoryItemFontEdit4 = New DevExpress.XtraEditors.Repository.RepositoryItemFontEdit()
		Me.RepositoryItemComboBox7 = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
		Me.RepositoryItemComboBox8 = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
		Me.grpQuery = New DevExpress.XtraEditors.GroupControl()
		Me.grdQuery = New DevExpress.XtraGrid.GridControl()
		Me.gvQuery = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.sccHeaderInfo = New DevExpress.XtraEditors.SplitContainerControl()
		CType(Me.grdMain, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvMain, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.RepositoryItemFontEdit4, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.RepositoryItemComboBox7, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.RepositoryItemComboBox8, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.grpQuery, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grpQuery.SuspendLayout()
		CType(Me.grdQuery, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvQuery, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.sccHeaderInfo, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.sccHeaderInfo.SuspendLayout()
		Me.SuspendLayout()
		'
		'grdMain
		'
		Me.grdMain.Dock = DockStyle.Fill
		Me.grdMain.Location = New System.Drawing.Point(5, 5)
		Me.grdMain.MainView = Me.gvMain
		Me.grdMain.Name = "grdMain"
		Me.grdMain.Size = New System.Drawing.Size(919, 428)
		Me.grdMain.TabIndex = 154
		Me.grdMain.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvMain})
		'
		'gvMain
		'
		Me.gvMain.GridControl = Me.grdMain
		Me.gvMain.Name = "gvMain"
		Me.gvMain.OptionsBehavior.Editable = False
		Me.gvMain.OptionsCustomization.AllowFilter = False
		Me.gvMain.OptionsFind.AlwaysVisible = True
		Me.gvMain.OptionsMenu.ShowAutoFilterRowItem = False
		Me.gvMain.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvMain.OptionsView.ShowAutoFilterRow = True
		Me.gvMain.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways
		Me.gvMain.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		Me.gvMain.OptionsView.ShowGroupedColumns = True
		Me.gvMain.OptionsView.ShowGroupPanel = False
		Me.gvMain.OptionsView.WaitAnimationOptions = DevExpress.XtraEditors.WaitAnimationOptions.Panel
		'
		'BarManager1
		'
		Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar1})
		Me.BarManager1.DockControls.Add(Me.BarDockControl5)
		Me.BarManager1.DockControls.Add(Me.BarDockControl6)
		Me.BarManager1.DockControls.Add(Me.BarDockControl7)
		Me.BarManager1.DockControls.Add(Me.BarDockControl8)
		Me.BarManager1.Form = Me
		Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.BarSubItem3, Me.bsiRecCount, Me.BarStaticItem3, Me.bsiLblRecCount, Me.bbiSMS, Me.bbiPrint})
		Me.BarManager1.MaxItemId = 67
		Me.BarManager1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemFontEdit4, Me.RepositoryItemComboBox7, Me.RepositoryItemComboBox8})
		Me.BarManager1.StatusBar = Me.Bar1
		'
		'Bar1
		'
		Me.Bar1.BarName = "Benutzerdefiniert 2"
		Me.Bar1.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
		Me.Bar1.DockCol = 0
		Me.Bar1.DockRow = 0
		Me.Bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
		Me.Bar1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiLblRecCount, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bsiRecCount), New DevExpress.XtraBars.LinkPersistInfo(Me.BarStaticItem3), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiPrint, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiSMS)})
		Me.Bar1.OptionsBar.AllowQuickCustomization = False
		Me.Bar1.OptionsBar.DrawDragBorder = False
		Me.Bar1.OptionsBar.UseWholeRow = True
		Me.Bar1.Text = "Benutzerdefiniert 2"
		'
		'bsiLblRecCount
		'
		Me.bsiLblRecCount.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
		Me.bsiLblRecCount.Caption = "Anzahl Datensätze:"
		Me.bsiLblRecCount.Id = 63
		Me.bsiLblRecCount.ItemAppearance.Normal.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.bsiLblRecCount.ItemAppearance.Normal.Options.UseFont = True
		Me.bsiLblRecCount.Name = "bsiLblRecCount"
		'
		'bsiRecCount
		'
		Me.bsiRecCount.AutoSize = DevExpress.XtraBars.BarStaticItemSize.Spring
		Me.bsiRecCount.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
		Me.bsiRecCount.Caption = "{0}"
		Me.bsiRecCount.Id = 60
		Me.bsiRecCount.Name = "bsiRecCount"
		'
		'BarStaticItem3
		'
		Me.BarStaticItem3.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
		Me.BarStaticItem3.Id = 62
		Me.BarStaticItem3.Name = "BarStaticItem3"
		'
		'bbiPrint
		'
		Me.bbiPrint.Caption = "Drucken"
		Me.bbiPrint.Id = 66
		Me.bbiPrint.ImageOptions.Image = CType(resources.GetObject("bbiPrint.ImageOptions.Image"), System.Drawing.Image)
		Me.bbiPrint.Name = "bbiPrint"
		Me.bbiPrint.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiSMS
		'
		Me.bbiSMS.Caption = "SMS-Versand"
		Me.bbiSMS.Id = 65
		Me.bbiSMS.ImageOptions.Image = CType(resources.GetObject("bbiSMS.ImageOptions.Image"), System.Drawing.Image)
		Me.bbiSMS.Name = "bbiSMS"
		Me.bbiSMS.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'BarDockControl5
		'
		Me.BarDockControl5.CausesValidation = False
		Me.BarDockControl5.Dock = DockStyle.Top
		Me.BarDockControl5.Location = New System.Drawing.Point(5, 5)
		Me.BarDockControl5.Manager = Me.BarManager1
		Me.BarDockControl5.Size = New System.Drawing.Size(929, 0)
		'
		'BarDockControl6
		'
		Me.BarDockControl6.CausesValidation = False
		Me.BarDockControl6.Dock = DockStyle.Bottom
		Me.BarDockControl6.Location = New System.Drawing.Point(5, 621)
		Me.BarDockControl6.Manager = Me.BarManager1
		Me.BarDockControl6.Size = New System.Drawing.Size(929, 27)
		'
		'BarDockControl7
		'
		Me.BarDockControl7.CausesValidation = False
		Me.BarDockControl7.Dock = DockStyle.Left
		Me.BarDockControl7.Location = New System.Drawing.Point(5, 5)
		Me.BarDockControl7.Manager = Me.BarManager1
		Me.BarDockControl7.Size = New System.Drawing.Size(0, 616)
		'
		'BarDockControl8
		'
		Me.BarDockControl8.CausesValidation = False
		Me.BarDockControl8.Dock = DockStyle.Right
		Me.BarDockControl8.Location = New System.Drawing.Point(934, 5)
		Me.BarDockControl8.Manager = Me.BarManager1
		Me.BarDockControl8.Size = New System.Drawing.Size(0, 616)
		'
		'BarSubItem3
		'
		Me.BarSubItem3.Caption = "Vorlage laden"
		Me.BarSubItem3.Id = 59
		Me.BarSubItem3.Name = "BarSubItem3"
		Me.BarSubItem3.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		Me.BarSubItem3.ShowMenuCaption = True
		'
		'RepositoryItemFontEdit4
		'
		Me.RepositoryItemFontEdit4.AutoHeight = False
		Me.RepositoryItemFontEdit4.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.RepositoryItemFontEdit4.Name = "RepositoryItemFontEdit4"
		'
		'RepositoryItemComboBox7
		'
		Me.RepositoryItemComboBox7.AutoHeight = False
		Me.RepositoryItemComboBox7.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.RepositoryItemComboBox7.Name = "RepositoryItemComboBox7"
		'
		'RepositoryItemComboBox8
		'
		Me.RepositoryItemComboBox8.AutoHeight = False
		Me.RepositoryItemComboBox8.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.RepositoryItemComboBox8.Name = "RepositoryItemComboBox8"
		'
		'grpQuery
		'
		Me.grpQuery.Controls.Add(Me.grdQuery)
		Me.grpQuery.Dock = DockStyle.Fill
		Me.grpQuery.Location = New System.Drawing.Point(5, 5)
		Me.grpQuery.Name = "grpQuery"
		Me.grpQuery.Padding = New System.Windows.Forms.Padding(10)
		Me.grpQuery.Size = New System.Drawing.Size(919, 163)
		Me.grpQuery.TabIndex = 159
		Me.grpQuery.Text = "Ihre individuelle Abfragen"
		'
		'grdQuery
		'
		Me.grdQuery.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.grdQuery.Location = New System.Drawing.Point(4, 24)
		Me.grdQuery.MainView = Me.gvQuery
		Me.grdQuery.Name = "grdQuery"
		Me.grdQuery.Size = New System.Drawing.Size(512, 133)
		Me.grdQuery.TabIndex = 155
		Me.grdQuery.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvQuery})
		'
		'gvQuery
		'
		Me.gvQuery.GridControl = Me.grdQuery
		Me.gvQuery.Name = "gvQuery"
		Me.gvQuery.OptionsBehavior.Editable = False
		Me.gvQuery.OptionsCustomization.AllowFilter = False
		Me.gvQuery.OptionsMenu.ShowAutoFilterRowItem = False
		Me.gvQuery.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvQuery.OptionsView.ShowAutoFilterRow = True
		Me.gvQuery.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways
		Me.gvQuery.OptionsView.ShowColumnHeaders = False
		Me.gvQuery.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		Me.gvQuery.OptionsView.ShowGroupedColumns = True
		Me.gvQuery.OptionsView.ShowGroupPanel = False
		Me.gvQuery.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.[False]
		Me.gvQuery.OptionsView.ShowIndicator = False
		Me.gvQuery.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.[False]
		Me.gvQuery.OptionsView.WaitAnimationOptions = DevExpress.XtraEditors.WaitAnimationOptions.Panel
		'
		'sccHeaderInfo
		'
		Me.sccHeaderInfo.Dock = DockStyle.Fill
		Me.sccHeaderInfo.Horizontal = False
		Me.sccHeaderInfo.Location = New System.Drawing.Point(5, 5)
		Me.sccHeaderInfo.Name = "sccHeaderInfo"
		Me.sccHeaderInfo.Panel1.Controls.Add(Me.grpQuery)
		Me.sccHeaderInfo.Panel1.Padding = New System.Windows.Forms.Padding(5)
		Me.sccHeaderInfo.Panel1.Text = "Panel1"
		Me.sccHeaderInfo.Panel2.Controls.Add(Me.grdMain)
		Me.sccHeaderInfo.Panel2.Padding = New System.Windows.Forms.Padding(5)
		Me.sccHeaderInfo.Panel2.Text = "Panel2"
		Me.sccHeaderInfo.Size = New System.Drawing.Size(929, 616)
		Me.sccHeaderInfo.SplitterPosition = 173
		Me.sccHeaderInfo.TabIndex = 160
		Me.sccHeaderInfo.Text = "SplitContainerControl1"
		'
		'frmEmployeeQuickSearch
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(939, 653)
		Me.Controls.Add(Me.sccHeaderInfo)
		Me.Controls.Add(Me.BarDockControl7)
		Me.Controls.Add(Me.BarDockControl8)
		Me.Controls.Add(Me.BarDockControl6)
		Me.Controls.Add(Me.BarDockControl5)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.Name = "frmEmployeeQuickSearch"
		Me.Padding = New System.Windows.Forms.Padding(5)
		Me.Text = "Quick-Search"
		CType(Me.grdMain, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvMain, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.RepositoryItemFontEdit4, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.RepositoryItemComboBox7, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.RepositoryItemComboBox8, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.grpQuery, System.ComponentModel.ISupportInitialize).EndInit()
		Me.grpQuery.ResumeLayout(False)
		CType(Me.grdQuery, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvQuery, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.sccHeaderInfo, System.ComponentModel.ISupportInitialize).EndInit()
		Me.sccHeaderInfo.ResumeLayout(False)
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub

	Friend WithEvents grdMain As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvMain As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
	Friend WithEvents Bar1 As DevExpress.XtraBars.Bar
	Friend WithEvents bsiLblRecCount As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents bsiRecCount As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents BarStaticItem3 As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents BarDockControl5 As DevExpress.XtraBars.BarDockControl
	Friend WithEvents BarDockControl6 As DevExpress.XtraBars.BarDockControl
	Friend WithEvents BarDockControl7 As DevExpress.XtraBars.BarDockControl
	Friend WithEvents BarDockControl8 As DevExpress.XtraBars.BarDockControl
	Friend WithEvents BarSubItem3 As DevExpress.XtraBars.BarSubItem
	Friend WithEvents RepositoryItemFontEdit4 As DevExpress.XtraEditors.Repository.RepositoryItemFontEdit
	Friend WithEvents RepositoryItemComboBox7 As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
	Friend WithEvents RepositoryItemComboBox8 As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
	Friend WithEvents sccHeaderInfo As DevExpress.XtraEditors.SplitContainerControl
	Friend WithEvents grpQuery As DevExpress.XtraEditors.GroupControl
	Friend WithEvents grdQuery As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvQuery As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents bbiPrint As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbiSMS As DevExpress.XtraBars.BarButtonItem
End Class
