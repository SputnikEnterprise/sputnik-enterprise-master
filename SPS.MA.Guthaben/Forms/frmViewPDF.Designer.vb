<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmViewPDF
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmViewPDF))
		Me.PdfViewer = New DevExpress.XtraPdfViewer.PdfViewer()
		Me.PdfBarController1 = New DevExpress.XtraPdfViewer.Bars.PdfBarController()
		Me.PdfFileOpenBarItem1 = New DevExpress.XtraPdfViewer.Bars.PdfFileOpenBarItem()
		Me.PdfFilePrintBarItem1 = New DevExpress.XtraPdfViewer.Bars.PdfFilePrintBarItem()
		Me.PdfPreviousPageBarItem1 = New DevExpress.XtraPdfViewer.Bars.PdfPreviousPageBarItem()
		Me.PdfNextPageBarItem1 = New DevExpress.XtraPdfViewer.Bars.PdfNextPageBarItem()
		Me.PdfFindTextBarItem1 = New DevExpress.XtraPdfViewer.Bars.PdfFindTextBarItem()
		Me.PdfZoomOutBarItem1 = New DevExpress.XtraPdfViewer.Bars.PdfZoomOutBarItem()
		Me.PdfZoomInBarItem1 = New DevExpress.XtraPdfViewer.Bars.PdfZoomInBarItem()
		Me.PdfExactZoomListBarSubItem1 = New DevExpress.XtraPdfViewer.Bars.PdfExactZoomListBarSubItem()
		Me.PdfZoom10CheckItem1 = New DevExpress.XtraPdfViewer.Bars.PdfZoom10CheckItem()
		Me.PdfZoom25CheckItem1 = New DevExpress.XtraPdfViewer.Bars.PdfZoom25CheckItem()
		Me.PdfZoom50CheckItem1 = New DevExpress.XtraPdfViewer.Bars.PdfZoom50CheckItem()
		Me.PdfZoom75CheckItem1 = New DevExpress.XtraPdfViewer.Bars.PdfZoom75CheckItem()
		Me.PdfZoom100CheckItem1 = New DevExpress.XtraPdfViewer.Bars.PdfZoom100CheckItem()
		Me.PdfZoom125CheckItem1 = New DevExpress.XtraPdfViewer.Bars.PdfZoom125CheckItem()
		Me.PdfZoom150CheckItem1 = New DevExpress.XtraPdfViewer.Bars.PdfZoom150CheckItem()
		Me.PdfZoom200CheckItem1 = New DevExpress.XtraPdfViewer.Bars.PdfZoom200CheckItem()
		Me.PdfZoom400CheckItem1 = New DevExpress.XtraPdfViewer.Bars.PdfZoom400CheckItem()
		Me.PdfZoom500CheckItem1 = New DevExpress.XtraPdfViewer.Bars.PdfZoom500CheckItem()
		Me.PdfSetActualSizeZoomModeCheckItem1 = New DevExpress.XtraPdfViewer.Bars.PdfSetActualSizeZoomModeCheckItem()
		Me.PdfSetPageLevelZoomModeCheckItem1 = New DevExpress.XtraPdfViewer.Bars.PdfSetPageLevelZoomModeCheckItem()
		Me.PdfSetFitWidthZoomModeCheckItem1 = New DevExpress.XtraPdfViewer.Bars.PdfSetFitWidthZoomModeCheckItem()
		Me.PdfSetFitVisibleZoomModeCheckItem1 = New DevExpress.XtraPdfViewer.Bars.PdfSetFitVisibleZoomModeCheckItem()
		Me.RibbonControl1 = New DevExpress.XtraBars.Ribbon.RibbonControl()
		Me.PdfRibbonPage1 = New DevExpress.XtraPdfViewer.Bars.PdfRibbonPage()
		Me.PdfFileRibbonPageGroup1 = New DevExpress.XtraPdfViewer.Bars.PdfFileRibbonPageGroup()
		Me.PdfNavigationRibbonPageGroup1 = New DevExpress.XtraPdfViewer.Bars.PdfNavigationRibbonPageGroup()
		Me.PdfZoomRibbonPageGroup1 = New DevExpress.XtraPdfViewer.Bars.PdfZoomRibbonPageGroup()
		CType(Me.PdfBarController1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.RibbonControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'PdfViewer
		'
		Me.PdfViewer.Dock = DockStyle.Fill
		Me.PdfViewer.Location = New System.Drawing.Point(0, 96)
		Me.PdfViewer.Name = "PdfViewer"
		Me.PdfViewer.Size = New System.Drawing.Size(1022, 528)
		Me.PdfViewer.TabIndex = 3
		'
		'PdfBarController1
		'
		Me.PdfBarController1.BarItems.Add(Me.PdfFileOpenBarItem1)
		Me.PdfBarController1.BarItems.Add(Me.PdfFilePrintBarItem1)
		Me.PdfBarController1.BarItems.Add(Me.PdfPreviousPageBarItem1)
		Me.PdfBarController1.BarItems.Add(Me.PdfNextPageBarItem1)
		Me.PdfBarController1.BarItems.Add(Me.PdfFindTextBarItem1)
		Me.PdfBarController1.BarItems.Add(Me.PdfZoomOutBarItem1)
		Me.PdfBarController1.BarItems.Add(Me.PdfZoomInBarItem1)
		Me.PdfBarController1.BarItems.Add(Me.PdfExactZoomListBarSubItem1)
		Me.PdfBarController1.BarItems.Add(Me.PdfZoom10CheckItem1)
		Me.PdfBarController1.BarItems.Add(Me.PdfZoom25CheckItem1)
		Me.PdfBarController1.BarItems.Add(Me.PdfZoom50CheckItem1)
		Me.PdfBarController1.BarItems.Add(Me.PdfZoom75CheckItem1)
		Me.PdfBarController1.BarItems.Add(Me.PdfZoom100CheckItem1)
		Me.PdfBarController1.BarItems.Add(Me.PdfZoom125CheckItem1)
		Me.PdfBarController1.BarItems.Add(Me.PdfZoom150CheckItem1)
		Me.PdfBarController1.BarItems.Add(Me.PdfZoom200CheckItem1)
		Me.PdfBarController1.BarItems.Add(Me.PdfZoom400CheckItem1)
		Me.PdfBarController1.BarItems.Add(Me.PdfZoom500CheckItem1)
		Me.PdfBarController1.BarItems.Add(Me.PdfSetActualSizeZoomModeCheckItem1)
		Me.PdfBarController1.BarItems.Add(Me.PdfSetPageLevelZoomModeCheckItem1)
		Me.PdfBarController1.BarItems.Add(Me.PdfSetFitWidthZoomModeCheckItem1)
		Me.PdfBarController1.BarItems.Add(Me.PdfSetFitVisibleZoomModeCheckItem1)
		Me.PdfBarController1.Control = Me.PdfViewer
		'
		'PdfFileOpenBarItem1
		'
		Me.PdfFileOpenBarItem1.Id = 1
		Me.PdfFileOpenBarItem1.ItemShortcut = New DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O))
		Me.PdfFileOpenBarItem1.Name = "PdfFileOpenBarItem1"
		'
		'PdfFilePrintBarItem1
		'
		Me.PdfFilePrintBarItem1.Id = 2
		Me.PdfFilePrintBarItem1.ItemShortcut = New DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.P))
		Me.PdfFilePrintBarItem1.Name = "PdfFilePrintBarItem1"
		'
		'PdfPreviousPageBarItem1
		'
		Me.PdfPreviousPageBarItem1.Id = 3
		Me.PdfPreviousPageBarItem1.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.PageUp)
		Me.PdfPreviousPageBarItem1.Name = "PdfPreviousPageBarItem1"
		'
		'PdfNextPageBarItem1
		'
		Me.PdfNextPageBarItem1.Id = 4
		Me.PdfNextPageBarItem1.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.PageDown)
		Me.PdfNextPageBarItem1.Name = "PdfNextPageBarItem1"
		'
		'PdfFindTextBarItem1
		'
		Me.PdfFindTextBarItem1.Id = 5
		Me.PdfFindTextBarItem1.ItemShortcut = New DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F))
		Me.PdfFindTextBarItem1.Name = "PdfFindTextBarItem1"
		'
		'PdfZoomOutBarItem1
		'
		Me.PdfZoomOutBarItem1.Id = 6
		Me.PdfZoomOutBarItem1.Name = "PdfZoomOutBarItem1"
		'
		'PdfZoomInBarItem1
		'
		Me.PdfZoomInBarItem1.Id = 7
		Me.PdfZoomInBarItem1.Name = "PdfZoomInBarItem1"
		'
		'PdfExactZoomListBarSubItem1
		'
		Me.PdfExactZoomListBarSubItem1.Id = 8
		Me.PdfExactZoomListBarSubItem1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.PdfZoom10CheckItem1, True), New DevExpress.XtraBars.LinkPersistInfo(Me.PdfZoom25CheckItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.PdfZoom50CheckItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.PdfZoom75CheckItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.PdfZoom100CheckItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.PdfZoom125CheckItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.PdfZoom150CheckItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.PdfZoom200CheckItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.PdfZoom400CheckItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.PdfZoom500CheckItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.PdfSetActualSizeZoomModeCheckItem1, True), New DevExpress.XtraBars.LinkPersistInfo(Me.PdfSetPageLevelZoomModeCheckItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.PdfSetFitWidthZoomModeCheckItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.PdfSetFitVisibleZoomModeCheckItem1)})
		Me.PdfExactZoomListBarSubItem1.Name = "PdfExactZoomListBarSubItem1"
		Me.PdfExactZoomListBarSubItem1.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionInMenu
		'
		'PdfZoom10CheckItem1
		'
		Me.PdfZoom10CheckItem1.Id = 9
		Me.PdfZoom10CheckItem1.Name = "PdfZoom10CheckItem1"
		'
		'PdfZoom25CheckItem1
		'
		Me.PdfZoom25CheckItem1.Id = 10
		Me.PdfZoom25CheckItem1.Name = "PdfZoom25CheckItem1"
		'
		'PdfZoom50CheckItem1
		'
		Me.PdfZoom50CheckItem1.Id = 11
		Me.PdfZoom50CheckItem1.Name = "PdfZoom50CheckItem1"
		'
		'PdfZoom75CheckItem1
		'
		Me.PdfZoom75CheckItem1.Id = 12
		Me.PdfZoom75CheckItem1.Name = "PdfZoom75CheckItem1"
		'
		'PdfZoom100CheckItem1
		'
		Me.PdfZoom100CheckItem1.Id = 13
		Me.PdfZoom100CheckItem1.Name = "PdfZoom100CheckItem1"
		'
		'PdfZoom125CheckItem1
		'
		Me.PdfZoom125CheckItem1.Id = 14
		Me.PdfZoom125CheckItem1.Name = "PdfZoom125CheckItem1"
		'
		'PdfZoom150CheckItem1
		'
		Me.PdfZoom150CheckItem1.Id = 15
		Me.PdfZoom150CheckItem1.Name = "PdfZoom150CheckItem1"
		'
		'PdfZoom200CheckItem1
		'
		Me.PdfZoom200CheckItem1.Id = 16
		Me.PdfZoom200CheckItem1.Name = "PdfZoom200CheckItem1"
		'
		'PdfZoom400CheckItem1
		'
		Me.PdfZoom400CheckItem1.Id = 17
		Me.PdfZoom400CheckItem1.Name = "PdfZoom400CheckItem1"
		'
		'PdfZoom500CheckItem1
		'
		Me.PdfZoom500CheckItem1.Id = 18
		Me.PdfZoom500CheckItem1.Name = "PdfZoom500CheckItem1"
		'
		'PdfSetActualSizeZoomModeCheckItem1
		'
		Me.PdfSetActualSizeZoomModeCheckItem1.Id = 19
		Me.PdfSetActualSizeZoomModeCheckItem1.Name = "PdfSetActualSizeZoomModeCheckItem1"
		'
		'PdfSetPageLevelZoomModeCheckItem1
		'
		Me.PdfSetPageLevelZoomModeCheckItem1.Id = 20
		Me.PdfSetPageLevelZoomModeCheckItem1.Name = "PdfSetPageLevelZoomModeCheckItem1"
		'
		'PdfSetFitWidthZoomModeCheckItem1
		'
		Me.PdfSetFitWidthZoomModeCheckItem1.Id = 21
		Me.PdfSetFitWidthZoomModeCheckItem1.Name = "PdfSetFitWidthZoomModeCheckItem1"
		'
		'PdfSetFitVisibleZoomModeCheckItem1
		'
		Me.PdfSetFitVisibleZoomModeCheckItem1.Id = 22
		Me.PdfSetFitVisibleZoomModeCheckItem1.Name = "PdfSetFitVisibleZoomModeCheckItem1"
		'
		'RibbonControl1
		'
		Me.RibbonControl1.AllowMinimizeRibbon = False
		Me.RibbonControl1.ExpandCollapseItem.Id = 0
		Me.RibbonControl1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.RibbonControl1.ExpandCollapseItem, Me.PdfFileOpenBarItem1, Me.PdfFilePrintBarItem1, Me.PdfPreviousPageBarItem1, Me.PdfNextPageBarItem1, Me.PdfFindTextBarItem1, Me.PdfZoomOutBarItem1, Me.PdfZoomInBarItem1, Me.PdfExactZoomListBarSubItem1, Me.PdfZoom10CheckItem1, Me.PdfZoom25CheckItem1, Me.PdfZoom50CheckItem1, Me.PdfZoom75CheckItem1, Me.PdfZoom100CheckItem1, Me.PdfZoom125CheckItem1, Me.PdfZoom150CheckItem1, Me.PdfZoom200CheckItem1, Me.PdfZoom400CheckItem1, Me.PdfZoom500CheckItem1, Me.PdfSetActualSizeZoomModeCheckItem1, Me.PdfSetPageLevelZoomModeCheckItem1, Me.PdfSetFitWidthZoomModeCheckItem1, Me.PdfSetFitVisibleZoomModeCheckItem1})
		Me.RibbonControl1.Location = New System.Drawing.Point(0, 0)
		Me.RibbonControl1.MaxItemId = 23
		Me.RibbonControl1.Name = "RibbonControl1"
		Me.RibbonControl1.Pages.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPage() {Me.PdfRibbonPage1})
		Me.RibbonControl1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2013
		Me.RibbonControl1.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.[False]
		Me.RibbonControl1.ShowCategoryInCaption = False
		Me.RibbonControl1.ShowExpandCollapseButton = DevExpress.Utils.DefaultBoolean.[False]
		Me.RibbonControl1.ShowFullScreenButton = DevExpress.Utils.DefaultBoolean.[False]
		Me.RibbonControl1.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.ShowOnMultiplePages
		Me.RibbonControl1.ShowToolbarCustomizeItem = False
		Me.RibbonControl1.Size = New System.Drawing.Size(1022, 96)
		Me.RibbonControl1.Toolbar.ShowCustomizeItem = False
		Me.RibbonControl1.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden
		'
		'PdfRibbonPage1
		'
		Me.PdfRibbonPage1.Groups.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageGroup() {Me.PdfFileRibbonPageGroup1, Me.PdfNavigationRibbonPageGroup1, Me.PdfZoomRibbonPageGroup1})
		Me.PdfRibbonPage1.Name = "PdfRibbonPage1"
		'
		'PdfFileRibbonPageGroup1
		'
		Me.PdfFileRibbonPageGroup1.ItemLinks.Add(Me.PdfFileOpenBarItem1)
		Me.PdfFileRibbonPageGroup1.ItemLinks.Add(Me.PdfFilePrintBarItem1)
		Me.PdfFileRibbonPageGroup1.Name = "PdfFileRibbonPageGroup1"
		Me.PdfFileRibbonPageGroup1.Text = ""
		'
		'PdfNavigationRibbonPageGroup1
		'
		Me.PdfNavigationRibbonPageGroup1.ItemLinks.Add(Me.PdfPreviousPageBarItem1)
		Me.PdfNavigationRibbonPageGroup1.ItemLinks.Add(Me.PdfNextPageBarItem1)
		Me.PdfNavigationRibbonPageGroup1.ItemLinks.Add(Me.PdfFindTextBarItem1)
		Me.PdfNavigationRibbonPageGroup1.Name = "PdfNavigationRibbonPageGroup1"
		Me.PdfNavigationRibbonPageGroup1.Text = ""
		'
		'PdfZoomRibbonPageGroup1
		'
		Me.PdfZoomRibbonPageGroup1.ItemLinks.Add(Me.PdfZoomOutBarItem1)
		Me.PdfZoomRibbonPageGroup1.ItemLinks.Add(Me.PdfZoomInBarItem1)
		Me.PdfZoomRibbonPageGroup1.ItemLinks.Add(Me.PdfExactZoomListBarSubItem1)
		Me.PdfZoomRibbonPageGroup1.Name = "PdfZoomRibbonPageGroup1"
		Me.PdfZoomRibbonPageGroup1.Text = ""
		'
		'frmViewPDF
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(1022, 624)
		Me.Controls.Add(Me.PdfViewer)
		Me.Controls.Add(Me.RibbonControl1)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.Name = "frmViewPDF"
		Me.Text = "PDF-Anzeige"
		CType(Me.PdfBarController1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.RibbonControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents PdfViewer As DevExpress.XtraPdfViewer.PdfViewer
	Friend WithEvents PdfBarController1 As DevExpress.XtraPdfViewer.Bars.PdfBarController
	Friend WithEvents PdfFileOpenBarItem1 As DevExpress.XtraPdfViewer.Bars.PdfFileOpenBarItem
	Friend WithEvents PdfFilePrintBarItem1 As DevExpress.XtraPdfViewer.Bars.PdfFilePrintBarItem
	Friend WithEvents PdfPreviousPageBarItem1 As DevExpress.XtraPdfViewer.Bars.PdfPreviousPageBarItem
	Friend WithEvents PdfNextPageBarItem1 As DevExpress.XtraPdfViewer.Bars.PdfNextPageBarItem
	Friend WithEvents PdfFindTextBarItem1 As DevExpress.XtraPdfViewer.Bars.PdfFindTextBarItem
	Friend WithEvents PdfZoomOutBarItem1 As DevExpress.XtraPdfViewer.Bars.PdfZoomOutBarItem
	Friend WithEvents PdfZoomInBarItem1 As DevExpress.XtraPdfViewer.Bars.PdfZoomInBarItem
	Friend WithEvents PdfExactZoomListBarSubItem1 As DevExpress.XtraPdfViewer.Bars.PdfExactZoomListBarSubItem
	Friend WithEvents PdfZoom10CheckItem1 As DevExpress.XtraPdfViewer.Bars.PdfZoom10CheckItem
	Friend WithEvents PdfZoom25CheckItem1 As DevExpress.XtraPdfViewer.Bars.PdfZoom25CheckItem
	Friend WithEvents PdfZoom50CheckItem1 As DevExpress.XtraPdfViewer.Bars.PdfZoom50CheckItem
	Friend WithEvents PdfZoom75CheckItem1 As DevExpress.XtraPdfViewer.Bars.PdfZoom75CheckItem
	Friend WithEvents PdfZoom100CheckItem1 As DevExpress.XtraPdfViewer.Bars.PdfZoom100CheckItem
	Friend WithEvents PdfZoom125CheckItem1 As DevExpress.XtraPdfViewer.Bars.PdfZoom125CheckItem
	Friend WithEvents PdfZoom150CheckItem1 As DevExpress.XtraPdfViewer.Bars.PdfZoom150CheckItem
	Friend WithEvents PdfZoom200CheckItem1 As DevExpress.XtraPdfViewer.Bars.PdfZoom200CheckItem
	Friend WithEvents PdfZoom400CheckItem1 As DevExpress.XtraPdfViewer.Bars.PdfZoom400CheckItem
	Friend WithEvents PdfZoom500CheckItem1 As DevExpress.XtraPdfViewer.Bars.PdfZoom500CheckItem
	Friend WithEvents PdfSetActualSizeZoomModeCheckItem1 As DevExpress.XtraPdfViewer.Bars.PdfSetActualSizeZoomModeCheckItem
	Friend WithEvents PdfSetPageLevelZoomModeCheckItem1 As DevExpress.XtraPdfViewer.Bars.PdfSetPageLevelZoomModeCheckItem
	Friend WithEvents PdfSetFitWidthZoomModeCheckItem1 As DevExpress.XtraPdfViewer.Bars.PdfSetFitWidthZoomModeCheckItem
	Friend WithEvents PdfSetFitVisibleZoomModeCheckItem1 As DevExpress.XtraPdfViewer.Bars.PdfSetFitVisibleZoomModeCheckItem
	Friend WithEvents RibbonControl1 As DevExpress.XtraBars.Ribbon.RibbonControl
	Friend WithEvents PdfRibbonPage1 As DevExpress.XtraPdfViewer.Bars.PdfRibbonPage
	Friend WithEvents PdfFileRibbonPageGroup1 As DevExpress.XtraPdfViewer.Bars.PdfFileRibbonPageGroup
	Friend WithEvents PdfNavigationRibbonPageGroup1 As DevExpress.XtraPdfViewer.Bars.PdfNavigationRibbonPageGroup
	Friend WithEvents PdfZoomRibbonPageGroup1 As DevExpress.XtraPdfViewer.Bars.PdfZoomRibbonPageGroup
End Class
