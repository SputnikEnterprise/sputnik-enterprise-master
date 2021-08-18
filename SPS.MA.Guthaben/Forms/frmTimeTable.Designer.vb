Namespace UI
	<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
	Partial Class frmTimeTable
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
			Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTimeTable))
			Me.grdTotalHours = New DevExpress.XtraGrid.GridControl()
			Me.gvTotalHours = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.XtraTabControl1 = New DevExpress.XtraTab.XtraTabControl()
			Me.xtabTotalRapportStd = New DevExpress.XtraTab.XtraTabPage()
			Me.xtabGroupedByKSTNr = New DevExpress.XtraTab.XtraTabPage()
			Me.grdTotalHoursGrouped = New DevExpress.XtraGrid.GridControl()
			Me.gvTotalHoursGrouped = New DevExpress.XtraGrid.Views.Grid.GridView()
			CType(Me.grdTotalHours, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvTotalHours, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.XtraTabControl1.SuspendLayout()
			Me.xtabTotalRapportStd.SuspendLayout()
			Me.xtabGroupedByKSTNr.SuspendLayout()
			CType(Me.grdTotalHoursGrouped, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvTotalHoursGrouped, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			'
			'grdTotalHours
			'
			Me.grdTotalHours.Dock = System.Windows.Forms.DockStyle.Fill
			Me.grdTotalHours.Location = New System.Drawing.Point(5, 5)
			Me.grdTotalHours.MainView = Me.gvTotalHours
			Me.grdTotalHours.Name = "grdTotalHours"
			Me.grdTotalHours.Size = New System.Drawing.Size(940, 833)
			Me.grdTotalHours.TabIndex = 11
			Me.grdTotalHours.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvTotalHours})
			'
			'gvTotalHours
			'
			Me.gvTotalHours.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			Me.gvTotalHours.GridControl = Me.grdTotalHours
			Me.gvTotalHours.Name = "gvTotalHours"
			Me.gvTotalHours.OptionsBehavior.Editable = False
			Me.gvTotalHours.OptionsSelection.EnableAppearanceFocusedCell = False
			Me.gvTotalHours.OptionsView.ShowGroupPanel = False
			'
			'XtraTabControl1
			'
			Me.XtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.XtraTabControl1.Location = New System.Drawing.Point(5, 5)
			Me.XtraTabControl1.Name = "XtraTabControl1"
			Me.XtraTabControl1.SelectedTabPage = Me.xtabTotalRapportStd
			Me.XtraTabControl1.Size = New System.Drawing.Size(956, 871)
			Me.XtraTabControl1.TabIndex = 12
			Me.XtraTabControl1.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabTotalRapportStd, Me.xtabGroupedByKSTNr})
			'
			'xtabTotalRapportStd
			'
			Me.xtabTotalRapportStd.Controls.Add(Me.grdTotalHours)
			Me.xtabTotalRapportStd.Name = "xtabTotalRapportStd"
			Me.xtabTotalRapportStd.Padding = New System.Windows.Forms.Padding(5)
			Me.xtabTotalRapportStd.Size = New System.Drawing.Size(950, 843)
			Me.xtabTotalRapportStd.Text = "Total Rapportstunden"
			'
			'xtabGroupedByKSTNr
			'
			Me.xtabGroupedByKSTNr.Controls.Add(Me.grdTotalHoursGrouped)
			Me.xtabGroupedByKSTNr.Name = "xtabGroupedByKSTNr"
			Me.xtabGroupedByKSTNr.Padding = New System.Windows.Forms.Padding(5)
			Me.xtabGroupedByKSTNr.Size = New System.Drawing.Size(582, 714)
			Me.xtabGroupedByKSTNr.Text = "Gruppiert nach Kostenstelle"
			'
			'grdTotalHoursGrouped
			'
			Me.grdTotalHoursGrouped.Dock = System.Windows.Forms.DockStyle.Fill
			Me.grdTotalHoursGrouped.Location = New System.Drawing.Point(5, 5)
			Me.grdTotalHoursGrouped.MainView = Me.gvTotalHoursGrouped
			Me.grdTotalHoursGrouped.Name = "grdTotalHoursGrouped"
			Me.grdTotalHoursGrouped.Size = New System.Drawing.Size(572, 704)
			Me.grdTotalHoursGrouped.TabIndex = 12
			Me.grdTotalHoursGrouped.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvTotalHoursGrouped})
			'
			'gvTotalHoursGrouped
			'
			Me.gvTotalHoursGrouped.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			Me.gvTotalHoursGrouped.GridControl = Me.grdTotalHoursGrouped
			Me.gvTotalHoursGrouped.Name = "gvTotalHoursGrouped"
			Me.gvTotalHoursGrouped.OptionsBehavior.Editable = False
			Me.gvTotalHoursGrouped.OptionsSelection.EnableAppearanceFocusedCell = False
			Me.gvTotalHoursGrouped.OptionsView.ShowGroupPanel = False
			'
			'frmTimeTable
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(966, 881)
			Me.Controls.Add(Me.XtraTabControl1)
			Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
			Me.Name = "frmTimeTable"
			Me.Padding = New System.Windows.Forms.Padding(5)
			Me.Text = "Stundenübersicht"
			CType(Me.grdTotalHours, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvTotalHours, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.XtraTabControl1.ResumeLayout(False)
			Me.xtabTotalRapportStd.ResumeLayout(False)
			Me.xtabGroupedByKSTNr.ResumeLayout(False)
			CType(Me.grdTotalHoursGrouped, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvTotalHoursGrouped, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		Friend WithEvents grdTotalHours As DevExpress.XtraGrid.GridControl
		Friend WithEvents gvTotalHours As DevExpress.XtraGrid.Views.Grid.GridView
		Friend WithEvents XtraTabControl1 As DevExpress.XtraTab.XtraTabControl
		Friend WithEvents xtabTotalRapportStd As DevExpress.XtraTab.XtraTabPage
		Friend WithEvents xtabGroupedByKSTNr As DevExpress.XtraTab.XtraTabPage
		Friend WithEvents grdTotalHoursGrouped As DevExpress.XtraGrid.GridControl
		Friend WithEvents gvTotalHoursGrouped As DevExpress.XtraGrid.Views.Grid.GridView
	End Class


End Namespace
