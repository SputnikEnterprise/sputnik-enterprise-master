<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmReportViewer
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
		Me.SnapControl1 = New DevExpress.Snap.SnapControl()
		Me.SnapDockManager1 = New DevExpress.Snap.Extensions.SnapDockManager()
		Me.FieldListDockPanel1 = New DevExpress.Snap.Extensions.UI.FieldListDockPanel()
		Me.FieldListDockPanel1_Container = New DevExpress.XtraBars.Docking.ControlContainer()
		Me.ReportExplorerDockPanel1 = New DevExpress.Snap.Extensions.UI.ReportExplorerDockPanel()
		Me.ReportExplorerDockPanel1_Container = New DevExpress.XtraBars.Docking.ControlContainer()
		Me.panelContainer1 = New DevExpress.XtraBars.Docking.DockPanel()
		Me.SnapDocumentManager1 = New DevExpress.Snap.Extensions.SnapDocumentManager()
		Me.NoDocumentsView1 = New DevExpress.XtraBars.Docking2010.Views.NoDocuments.NoDocumentsView()
		CType(Me.SnapDockManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.FieldListDockPanel1.SuspendLayout()
		Me.ReportExplorerDockPanel1.SuspendLayout()
		Me.panelContainer1.SuspendLayout()
		CType(Me.SnapDocumentManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.NoDocumentsView1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'SnapControl1
		'
		Me.SnapControl1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.SnapControl1.EnableToolTips = True
		Me.SnapControl1.Location = New System.Drawing.Point(0, 0)
		Me.SnapControl1.Name = "SnapControl1"
		Me.SnapControl1.Options.Comments.ShowAllAuthors = True
		Me.SnapControl1.Options.Comments.Visibility = DevExpress.XtraRichEdit.RichEditCommentVisibility.Auto
		Me.SnapControl1.Options.CopyPaste.MaintainDocumentSectionSettings = False
		Me.SnapControl1.Options.Fields.EnableEmptyFieldDataAlias = True
		Me.SnapControl1.Options.Fields.UseCurrentCultureDateTimeFormat = False
		Me.SnapControl1.Options.SnapMailMergeVisualOptions.DataSourceName = Nothing
		Me.SnapControl1.Size = New System.Drawing.Size(665, 636)
		Me.SnapControl1.TabIndex = 0
		Me.SnapControl1.Text = "SnapControl1"
		'
		'SnapDockManager1
		'
		Me.SnapDockManager1.Form = Me
		Me.SnapDockManager1.RootPanels.AddRange(New DevExpress.XtraBars.Docking.DockPanel() {Me.panelContainer1})
		Me.SnapDockManager1.SnapControl = Me.SnapControl1
		Me.SnapDockManager1.TopZIndexControls.AddRange(New String() {"DevExpress.XtraBars.BarDockControl", "DevExpress.XtraBars.StandaloneBarDockControl", "System.Windows.Forms.StatusBar", "System.Windows.Forms.MenuStrip", "System.Windows.Forms.StatusStrip", "DevExpress.XtraBars.Ribbon.RibbonStatusBar", "DevExpress.XtraBars.Ribbon.RibbonControl", "DevExpress.XtraBars.Navigation.OfficeNavigationBar", "DevExpress.XtraBars.Navigation.TileNavPane"})
		'
		'FieldListDockPanel1
		'
		Me.FieldListDockPanel1.Controls.Add(Me.FieldListDockPanel1_Container)
		Me.FieldListDockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill
		Me.FieldListDockPanel1.ID = New System.Guid("20eebb46-294a-4053-bf00-061221d25814")
		Me.FieldListDockPanel1.Location = New System.Drawing.Point(0, 0)
		Me.FieldListDockPanel1.Name = "FieldListDockPanel1"
		Me.FieldListDockPanel1.OriginalSize = New System.Drawing.Size(200, 200)
		Me.FieldListDockPanel1.Size = New System.Drawing.Size(200, 318)
		Me.FieldListDockPanel1.SnapControl = Me.SnapControl1
		'
		'FieldListDockPanel1_Container
		'
		Me.FieldListDockPanel1_Container.Location = New System.Drawing.Point(4, 23)
		Me.FieldListDockPanel1_Container.Name = "FieldListDockPanel1_Container"
		Me.FieldListDockPanel1_Container.Size = New System.Drawing.Size(192, 291)
		Me.FieldListDockPanel1_Container.TabIndex = 0
		'
		'ReportExplorerDockPanel1
		'
		Me.ReportExplorerDockPanel1.Controls.Add(Me.ReportExplorerDockPanel1_Container)
		Me.ReportExplorerDockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill
		Me.ReportExplorerDockPanel1.ID = New System.Guid("417f8721-5d99-4e8d-bf18-a80e6b471711")
		Me.ReportExplorerDockPanel1.Location = New System.Drawing.Point(0, 318)
		Me.ReportExplorerDockPanel1.Name = "ReportExplorerDockPanel1"
		Me.ReportExplorerDockPanel1.OriginalSize = New System.Drawing.Size(84, 200)
		Me.ReportExplorerDockPanel1.Size = New System.Drawing.Size(200, 318)
		Me.ReportExplorerDockPanel1.SnapControl = Me.SnapControl1
		'
		'ReportExplorerDockPanel1_Container
		'
		Me.ReportExplorerDockPanel1_Container.Location = New System.Drawing.Point(4, 23)
		Me.ReportExplorerDockPanel1_Container.Name = "ReportExplorerDockPanel1_Container"
		Me.ReportExplorerDockPanel1_Container.Size = New System.Drawing.Size(192, 291)
		Me.ReportExplorerDockPanel1_Container.TabIndex = 0
		'
		'panelContainer1
		'
		Me.panelContainer1.Controls.Add(Me.FieldListDockPanel1)
		Me.panelContainer1.Controls.Add(Me.ReportExplorerDockPanel1)
		Me.panelContainer1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right
		Me.panelContainer1.ID = New System.Guid("40049cd6-11e2-432d-8509-3e17efcba015")
		Me.panelContainer1.Location = New System.Drawing.Point(665, 0)
		Me.panelContainer1.Name = "panelContainer1"
		Me.panelContainer1.OriginalSize = New System.Drawing.Size(200, 200)
		Me.panelContainer1.Size = New System.Drawing.Size(200, 636)
		Me.panelContainer1.Text = "panelContainer1"
		'
		'SnapDocumentManager1
		'
		Me.SnapDocumentManager1.ClientControl = Me.SnapControl1
		Me.SnapDocumentManager1.ShowThumbnailsInTaskBar = DevExpress.Utils.DefaultBoolean.[False]
		Me.SnapDocumentManager1.View = Me.NoDocumentsView1
		Me.SnapDocumentManager1.ViewCollection.AddRange(New DevExpress.XtraBars.Docking2010.Views.BaseView() {Me.NoDocumentsView1})
		'
		'frmReportViewer
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(865, 636)
		Me.Controls.Add(Me.SnapControl1)
		Me.Controls.Add(Me.panelContainer1)
		Me.Name = "frmReportViewer"
		Me.Text = "frmReportViewer"
		CType(Me.SnapDockManager1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.FieldListDockPanel1.ResumeLayout(False)
		Me.ReportExplorerDockPanel1.ResumeLayout(False)
		Me.panelContainer1.ResumeLayout(False)
		CType(Me.SnapDocumentManager1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.NoDocumentsView1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents SnapControl1 As DevExpress.Snap.SnapControl
	Friend WithEvents SnapDockManager1 As DevExpress.Snap.Extensions.SnapDockManager
	Friend WithEvents panelContainer1 As DevExpress.XtraBars.Docking.DockPanel
	Friend WithEvents FieldListDockPanel1 As DevExpress.Snap.Extensions.UI.FieldListDockPanel
	Friend WithEvents FieldListDockPanel1_Container As DevExpress.XtraBars.Docking.ControlContainer
	Friend WithEvents ReportExplorerDockPanel1 As DevExpress.Snap.Extensions.UI.ReportExplorerDockPanel
	Friend WithEvents ReportExplorerDockPanel1_Container As DevExpress.XtraBars.Docking.ControlContainer
	Friend WithEvents SnapDocumentManager1 As DevExpress.Snap.Extensions.SnapDocumentManager
	Friend WithEvents NoDocumentsView1 As DevExpress.XtraBars.Docking2010.Views.NoDocuments.NoDocumentsView
End Class
