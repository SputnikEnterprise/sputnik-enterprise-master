Imports DevExpress.Skins
Imports DevExpress.LookAndFeel
Imports DevExpress.UserSkins
Imports DevExpress.XtraEditors
Imports DevExpress.XtraBars.Helpers
Imports DevExpress.XtraBars

Imports DevExpress.XtraTab
'Imports DevExpress.XtraEditors.Controls


<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRPDocScan
  Inherits DevExpress.XtraBars.Ribbon.RibbonForm

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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRPDocScan))
		Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject4 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject5 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject6 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject7 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject8 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject9 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject10 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject11 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject12 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject13 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject14 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject15 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject16 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject17 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject18 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject19 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject20 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Me.BarAndDockingController11 = New DevExpress.XtraBars.BarAndDockingController(Me.components)
		Me.BarAndDockingController12 = New DevExpress.XtraBars.BarAndDockingController(Me.components)
		Me.BarAndDockingController10 = New DevExpress.XtraBars.BarAndDockingController(Me.components)
		Me.BarAndDockingController9 = New DevExpress.XtraBars.BarAndDockingController(Me.components)
		Me.BarAndDockingController8 = New DevExpress.XtraBars.BarAndDockingController(Me.components)
		Me.BarAndDockingController5 = New DevExpress.XtraBars.BarAndDockingController(Me.components)
		Me.BarAndDockingController7 = New DevExpress.XtraBars.BarAndDockingController(Me.components)
		Me.BarAndDockingController6 = New DevExpress.XtraBars.BarAndDockingController(Me.components)
		Me.BarAndDockingController4 = New DevExpress.XtraBars.BarAndDockingController(Me.components)
		Me.BarAndDockingController3 = New DevExpress.XtraBars.BarAndDockingController(Me.components)
		Me.BarAndDockingController2 = New DevExpress.XtraBars.BarAndDockingController(Me.components)
		Me.BarAndDockingController1 = New DevExpress.XtraBars.BarAndDockingController(Me.components)
		Me.xtabControl = New DevExpress.XtraTab.XtraTabControl()
		Me.navbarImageList = New System.Windows.Forms.ImageList(Me.components)
		Me.xtabSummery = New DevExpress.XtraTab.XtraTabPage()
		Me.sccRPStd = New DevExpress.XtraEditors.SplitContainerControl()
		Me.pcSPStdInfoHeader = New DevExpress.XtraEditors.PanelControl()
		Me.btnSaveCheckedContentInfoLocalDb = New DevExpress.XtraEditors.SimpleButton()
		Me.grdImportScanContent = New DevExpress.XtraGrid.GridControl()
		Me.gvImportScanContent = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.xtabScanedDoc = New DevExpress.XtraTab.XtraTabPage()
		Me.sccScanRPMain = New DevExpress.XtraEditors.SplitContainerControl()
		Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
		Me.grdScanContent = New DevExpress.XtraGrid.GridControl()
		Me.gvScanContent = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.lueScaned = New DevExpress.XtraEditors.GridLookUpEdit()
		Me.gvScanedDoc = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.sccControl = New DevExpress.XtraEditors.SplitContainerControl()
		Me.pcRapportDaten = New DevExpress.XtraEditors.PanelControl()
		Me.pnlScanData = New DevExpress.XtraEditors.PanelControl()
		Me.lblKategorie = New DevExpress.XtraEditors.LabelControl()
		Me.lueRecordNumber = New DevExpress.XtraEditors.GridLookUpEdit()
		Me.gvRecordNumber = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.lblRecordNumber = New DevExpress.XtraEditors.LabelControl()
		Me.lueCategory = New DevExpress.XtraEditors.LookUpEdit()
		Me.pnlReportData = New DevExpress.XtraEditors.PanelControl()
		Me.lblRapperNr = New DevExpress.XtraEditors.LabelControl()
		Me.txt_Beginn = New DevExpress.XtraEditors.TextEdit()
		Me.txt_Ende = New DevExpress.XtraEditors.TextEdit()
		Me.RadialMenu1 = New DevComponents.DotNetBar.RadialMenu()
		Me.rmRPOpen = New DevComponents.DotNetBar.RadialMenuItem()
		Me.rmRPDelete = New DevComponents.DotNetBar.RadialMenuItem()
		Me.rmRPPrint = New DevComponents.DotNetBar.RadialMenuItem()
		Me.txtKW = New DevExpress.XtraEditors.TextEdit()
		Me.lblErfasstzeile = New DevExpress.XtraEditors.LabelControl()
		Me.lueReportNumber = New DevExpress.XtraEditors.GridLookUpEdit()
		Me.gvReportNumber = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.lueReportLineID = New DevExpress.XtraEditors.GridLookUpEdit()
		Me.gvReportLineID = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.btnSaveContent = New DevExpress.XtraEditors.SimpleButton()
		Me.pnlPDFViewer = New DevExpress.XtraEditors.PanelControl()
		Me.PdfViewer1 = New DevExpress.XtraPdfViewer.PdfViewer()
		Me.RibbonControl1 = New DevExpress.XtraBars.Ribbon.RibbonControl()
		Me.beiRPNumber = New DevExpress.XtraBars.BarEditItem()
		Me.RepositoryItemTextEdit2 = New DevExpress.XtraEditors.Repository.RepositoryItemTextEdit()
		Me.beiKW = New DevExpress.XtraBars.BarEditItem()
		Me.RepositoryItemTextEdit3 = New DevExpress.XtraEditors.Repository.RepositoryItemTextEdit()
		Me.bbiOpenOneRP = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiSaveIndividualReportIntoDb = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiDeleteIndividualReport = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiRotate_ = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiRotate = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiPrintIndividualReport = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiZoomOneIn = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiZoomOneOut = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiActualSizeOne = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiSaveIndividualReportIntoFile = New DevExpress.XtraBars.BarButtonItem()
		Me.bsiLblRapportnummer = New DevExpress.XtraBars.BarStaticItem()
		Me.lblExistsRPNr = New DevExpress.XtraBars.BarStaticItem()
		Me.bsiLblRapportzeile = New DevExpress.XtraBars.BarStaticItem()
		Me.lblExistsRPLData = New DevExpress.XtraBars.BarStaticItem()
		Me.bChkMAWOS = New DevExpress.XtraBars.BarEditItem()
		Me.RepositoryItemCheckEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit()
		Me.bChkKDWOS = New DevExpress.XtraBars.BarEditItem()
		Me.RepositoryItemCheckEdit2 = New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit()
		Me.bsiState = New DevExpress.XtraBars.BarStaticItem()
		Me.bsiInfo = New DevExpress.XtraBars.BarStaticItem()
		Me.RibbonPage1 = New DevExpress.XtraBars.Ribbon.RibbonPage()
		Me.rpgEinzelnrapport = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
		Me.rpgDokumentseite = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
		Me.rpgRapportinfo = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
		Me.rpgWOS = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
		Me.RepositoryItemTextEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemTextEdit()
		Me.RepositoryItemButtonEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit()
		Me.RepositoryItemButtonEdit2 = New DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit()
		Me.RepositoryItemSpinEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit()
		Me.RepositoryItemComboBox1 = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
		Me.RibbonStatusBar1 = New DevExpress.XtraBars.Ribbon.RibbonStatusBar()
		Me.navbarImageListLarge = New System.Windows.Forms.ImageList(Me.components)
		Me.appMenu = New DevExpress.XtraBars.Ribbon.ApplicationMenu(Me.components)
		Me.ribbonImageCollection = New DevExpress.Utils.ImageCollection(Me.components)
		Me.ribbonImageCollectionLarge = New DevExpress.Utils.ImageCollection(Me.components)
		Me.PanelControl2 = New DevExpress.XtraEditors.PanelControl()
		CType(Me.BarAndDockingController11, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.BarAndDockingController12, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.BarAndDockingController10, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.BarAndDockingController9, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.BarAndDockingController8, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.BarAndDockingController5, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.BarAndDockingController7, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.BarAndDockingController6, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.BarAndDockingController4, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.BarAndDockingController3, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.BarAndDockingController2, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.BarAndDockingController1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.xtabControl, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.xtabControl.SuspendLayout()
		Me.xtabSummery.SuspendLayout()
		CType(Me.sccRPStd, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.sccRPStd.SuspendLayout()
		CType(Me.pcSPStdInfoHeader, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.pcSPStdInfoHeader.SuspendLayout()
		CType(Me.grdImportScanContent, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvImportScanContent, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.xtabScanedDoc.SuspendLayout()
		CType(Me.sccScanRPMain, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.sccScanRPMain.SuspendLayout()
		CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.PanelControl1.SuspendLayout()
		CType(Me.grdScanContent, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvScanContent, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lueScaned.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvScanedDoc, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.sccControl, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.sccControl.SuspendLayout()
		CType(Me.pcRapportDaten, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.pcRapportDaten.SuspendLayout()
		CType(Me.pnlScanData, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.pnlScanData.SuspendLayout()
		CType(Me.lueRecordNumber.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvRecordNumber, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lueCategory.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.pnlReportData, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.pnlReportData.SuspendLayout()
		CType(Me.txt_Beginn.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txt_Ende.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtKW.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lueReportNumber.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvReportNumber, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lueReportLineID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvReportLineID, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.pnlPDFViewer, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.pnlPDFViewer.SuspendLayout()
		CType(Me.RibbonControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.RepositoryItemTextEdit2, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.RepositoryItemTextEdit3, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.RepositoryItemCheckEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.RepositoryItemCheckEdit2, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.RepositoryItemTextEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.RepositoryItemButtonEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.RepositoryItemButtonEdit2, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.RepositoryItemSpinEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.RepositoryItemComboBox1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.appMenu, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.ribbonImageCollection, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.ribbonImageCollectionLarge, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.PanelControl2.SuspendLayout()
		Me.SuspendLayout()
		'
		'BarAndDockingController11
		'
		Me.BarAndDockingController11.PropertiesBar.DefaultGlyphSize = New System.Drawing.Size(16, 16)
		Me.BarAndDockingController11.PropertiesBar.DefaultLargeGlyphSize = New System.Drawing.Size(32, 32)
		'
		'BarAndDockingController12
		'
		Me.BarAndDockingController12.PropertiesBar.DefaultGlyphSize = New System.Drawing.Size(16, 16)
		Me.BarAndDockingController12.PropertiesBar.DefaultLargeGlyphSize = New System.Drawing.Size(32, 32)
		'
		'BarAndDockingController10
		'
		Me.BarAndDockingController10.PropertiesBar.DefaultGlyphSize = New System.Drawing.Size(16, 16)
		Me.BarAndDockingController10.PropertiesBar.DefaultLargeGlyphSize = New System.Drawing.Size(32, 32)
		'
		'BarAndDockingController9
		'
		Me.BarAndDockingController9.PropertiesBar.DefaultGlyphSize = New System.Drawing.Size(16, 16)
		Me.BarAndDockingController9.PropertiesBar.DefaultLargeGlyphSize = New System.Drawing.Size(32, 32)
		'
		'BarAndDockingController8
		'
		Me.BarAndDockingController8.PropertiesBar.DefaultGlyphSize = New System.Drawing.Size(16, 16)
		Me.BarAndDockingController8.PropertiesBar.DefaultLargeGlyphSize = New System.Drawing.Size(32, 32)
		'
		'BarAndDockingController5
		'
		Me.BarAndDockingController5.PropertiesBar.DefaultGlyphSize = New System.Drawing.Size(16, 16)
		Me.BarAndDockingController5.PropertiesBar.DefaultLargeGlyphSize = New System.Drawing.Size(32, 32)
		'
		'BarAndDockingController7
		'
		Me.BarAndDockingController7.PropertiesBar.DefaultGlyphSize = New System.Drawing.Size(16, 16)
		Me.BarAndDockingController7.PropertiesBar.DefaultLargeGlyphSize = New System.Drawing.Size(32, 32)
		'
		'BarAndDockingController6
		'
		Me.BarAndDockingController6.PropertiesBar.DefaultGlyphSize = New System.Drawing.Size(16, 16)
		Me.BarAndDockingController6.PropertiesBar.DefaultLargeGlyphSize = New System.Drawing.Size(32, 32)
		'
		'BarAndDockingController4
		'
		Me.BarAndDockingController4.PropertiesBar.DefaultGlyphSize = New System.Drawing.Size(16, 16)
		Me.BarAndDockingController4.PropertiesBar.DefaultLargeGlyphSize = New System.Drawing.Size(32, 32)
		'
		'BarAndDockingController3
		'
		Me.BarAndDockingController3.PropertiesBar.DefaultGlyphSize = New System.Drawing.Size(16, 16)
		Me.BarAndDockingController3.PropertiesBar.DefaultLargeGlyphSize = New System.Drawing.Size(32, 32)
		'
		'BarAndDockingController2
		'
		Me.BarAndDockingController2.PropertiesBar.DefaultGlyphSize = New System.Drawing.Size(16, 16)
		Me.BarAndDockingController2.PropertiesBar.DefaultLargeGlyphSize = New System.Drawing.Size(32, 32)
		'
		'BarAndDockingController1
		'
		Me.BarAndDockingController1.PropertiesBar.DefaultGlyphSize = New System.Drawing.Size(16, 16)
		Me.BarAndDockingController1.PropertiesBar.DefaultLargeGlyphSize = New System.Drawing.Size(32, 32)
		'
		'xtabControl
		'
		Me.xtabControl.Dock = System.Windows.Forms.DockStyle.Fill
		Me.xtabControl.Images = Me.navbarImageList
		Me.xtabControl.Location = New System.Drawing.Point(2, 2)
		Me.xtabControl.Name = "xtabControl"
		Me.xtabControl.SelectedTabPage = Me.xtabSummery
		Me.xtabControl.Size = New System.Drawing.Size(1268, 533)
		Me.xtabControl.TabIndex = 11
		Me.xtabControl.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabScanedDoc, Me.xtabSummery})
		'
		'navbarImageList
		'
		Me.navbarImageList.ImageStream = CType(resources.GetObject("navbarImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.navbarImageList.TransparentColor = System.Drawing.Color.Transparent
		Me.navbarImageList.Images.SetKeyName(0, "Inbox_16x16.png")
		Me.navbarImageList.Images.SetKeyName(1, "Outbox_16x16.png")
		Me.navbarImageList.Images.SetKeyName(2, "Drafts_16x16.png")
		Me.navbarImageList.Images.SetKeyName(3, "Trash_16x16.png")
		Me.navbarImageList.Images.SetKeyName(4, "Calendar_16x16.png")
		Me.navbarImageList.Images.SetKeyName(5, "Tasks_16x16.png")
		Me.navbarImageList.Images.SetKeyName(6, "NoteNew.png")
		Me.navbarImageList.Images.SetKeyName(7, "FolderOpen.png")
		'
		'xtabSummery
		'
		Me.xtabSummery.Controls.Add(Me.sccRPStd)
		Me.xtabSummery.ImageIndex = 5
		Me.xtabSummery.Name = "xtabSummery"
		Me.xtabSummery.Padding = New System.Windows.Forms.Padding(5)
		Me.xtabSummery.Size = New System.Drawing.Size(1262, 502)
		Me.xtabSummery.Text = "Zusammenfassen und in Rapportdatenbanken speichern"
		'
		'sccRPStd
		'
		Me.sccRPStd.Dock = System.Windows.Forms.DockStyle.Fill
		Me.sccRPStd.Horizontal = False
		Me.sccRPStd.Location = New System.Drawing.Point(5, 5)
		Me.sccRPStd.Name = "sccRPStd"
		Me.sccRPStd.Panel1.Controls.Add(Me.pcSPStdInfoHeader)
		Me.sccRPStd.Panel1.MinSize = 60
		Me.sccRPStd.Panel1.Padding = New System.Windows.Forms.Padding(5)
		Me.sccRPStd.Panel1.Text = "Panel1"
		Me.sccRPStd.Panel2.Controls.Add(Me.grdImportScanContent)
		Me.sccRPStd.Panel2.MinSize = 400
		Me.sccRPStd.Panel2.Text = "Panel2"
		Me.sccRPStd.Size = New System.Drawing.Size(1252, 492)
		Me.sccRPStd.SplitterPosition = 65
		Me.sccRPStd.TabIndex = 155
		Me.sccRPStd.Text = "SplitContainerControl3"
		'
		'pcSPStdInfoHeader
		'
		Me.pcSPStdInfoHeader.Controls.Add(Me.btnSaveCheckedContentInfoLocalDb)
		Me.pcSPStdInfoHeader.Location = New System.Drawing.Point(30, 9)
		Me.pcSPStdInfoHeader.Name = "pcSPStdInfoHeader"
		Me.pcSPStdInfoHeader.Size = New System.Drawing.Size(867, 51)
		Me.pcSPStdInfoHeader.TabIndex = 0
		'
		'btnSaveCheckedContentInfoLocalDb
		'
		Me.btnSaveCheckedContentInfoLocalDb.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnSaveCheckedContentInfoLocalDb.Image = CType(resources.GetObject("btnSaveCheckedContentInfoLocalDb.Image"), System.Drawing.Image)
		Me.btnSaveCheckedContentInfoLocalDb.Location = New System.Drawing.Point(626, 9)
		Me.btnSaveCheckedContentInfoLocalDb.Name = "btnSaveCheckedContentInfoLocalDb"
		Me.btnSaveCheckedContentInfoLocalDb.Size = New System.Drawing.Size(227, 36)
		Me.btnSaveCheckedContentInfoLocalDb.TabIndex = 7
		Me.btnSaveCheckedContentInfoLocalDb.Text = "Rapportdaten in Sputnik übernehmen"
		'
		'grdImportScanContent
		'
		Me.grdImportScanContent.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.grdImportScanContent.Location = New System.Drawing.Point(15, 10)
		Me.grdImportScanContent.MainView = Me.gvImportScanContent
		Me.grdImportScanContent.Name = "grdImportScanContent"
		Me.grdImportScanContent.Size = New System.Drawing.Size(1222, 327)
		Me.grdImportScanContent.TabIndex = 154
		Me.grdImportScanContent.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvImportScanContent})
		'
		'gvImportScanContent
		'
		Me.gvImportScanContent.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvImportScanContent.GridControl = Me.grdImportScanContent
		Me.gvImportScanContent.Name = "gvImportScanContent"
		Me.gvImportScanContent.OptionsBehavior.AutoSelectAllInEditor = False
		Me.gvImportScanContent.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvImportScanContent.OptionsView.ShowGroupPanel = False
		'
		'xtabScanedDoc
		'
		Me.xtabScanedDoc.Appearance.Header.Image = CType(resources.GetObject("xtabScanedDoc.Appearance.Header.Image"), System.Drawing.Image)
		Me.xtabScanedDoc.Appearance.Header.Options.UseImage = True
		Me.xtabScanedDoc.Controls.Add(Me.sccScanRPMain)
		Me.xtabScanedDoc.ImageIndex = 6
		Me.xtabScanedDoc.Name = "xtabScanedDoc"
		Me.xtabScanedDoc.Size = New System.Drawing.Size(1262, 502)
		Me.xtabScanedDoc.Text = "Gescannte Rapporte"
		'
		'sccScanRPMain
		'
		Me.sccScanRPMain.Dock = System.Windows.Forms.DockStyle.Fill
		Me.sccScanRPMain.Location = New System.Drawing.Point(0, 0)
		Me.sccScanRPMain.Name = "sccScanRPMain"
		Me.sccScanRPMain.Panel1.Controls.Add(Me.PanelControl1)
		Me.sccScanRPMain.Panel1.Text = "Panel1"
		Me.sccScanRPMain.Panel2.Controls.Add(Me.sccControl)
		Me.sccScanRPMain.Panel2.Text = "Panel2"
		Me.sccScanRPMain.Size = New System.Drawing.Size(1262, 502)
		Me.sccScanRPMain.SplitterPosition = 199
		Me.sccScanRPMain.TabIndex = 3
		Me.sccScanRPMain.Text = "SplitContainerControl1"
		'
		'PanelControl1
		'
		Me.PanelControl1.Controls.Add(Me.grdScanContent)
		Me.PanelControl1.Controls.Add(Me.lueScaned)
		Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.PanelControl1.Location = New System.Drawing.Point(0, 0)
		Me.PanelControl1.Name = "PanelControl1"
		Me.PanelControl1.Size = New System.Drawing.Size(199, 502)
		Me.PanelControl1.TabIndex = 1
		'
		'grdScanContent
		'
		Me.grdScanContent.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.grdScanContent.Location = New System.Drawing.Point(5, 28)
		Me.grdScanContent.MainView = Me.gvScanContent
		Me.grdScanContent.Name = "grdScanContent"
		Me.grdScanContent.Size = New System.Drawing.Size(191, 469)
		Me.grdScanContent.TabIndex = 153
		Me.grdScanContent.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvScanContent})
		'
		'gvScanContent
		'
		Me.gvScanContent.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvScanContent.GridControl = Me.grdScanContent
		Me.gvScanContent.Name = "gvScanContent"
		Me.gvScanContent.OptionsBehavior.Editable = False
		Me.gvScanContent.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvScanContent.OptionsView.ShowGroupPanel = False
		'
		'lueScaned
		'
		Me.lueScaned.Dock = System.Windows.Forms.DockStyle.Top
		Me.lueScaned.Location = New System.Drawing.Point(2, 2)
		Me.lueScaned.Name = "lueScaned"
		SerializableAppearanceObject1.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject1.Options.UseForeColor = True
		SerializableAppearanceObject2.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject2.Options.UseForeColor = True
		SerializableAppearanceObject3.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject3.Options.UseForeColor = True
		SerializableAppearanceObject4.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject4.Options.UseForeColor = True
		Me.lueScaned.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, True)})
		Me.lueScaned.Properties.NullText = ""
		Me.lueScaned.Properties.View = Me.gvScanedDoc
		Me.lueScaned.Size = New System.Drawing.Size(195, 20)
		Me.lueScaned.TabIndex = 154
		'
		'gvScanedDoc
		'
		Me.gvScanedDoc.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvScanedDoc.Name = "gvScanedDoc"
		Me.gvScanedDoc.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvScanedDoc.OptionsView.ShowGroupPanel = False
		'
		'sccControl
		'
		Me.sccControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple
		Me.sccControl.CaptionLocation = DevExpress.Utils.Locations.Top
		Me.sccControl.Collapsed = True
		Me.sccControl.Dock = System.Windows.Forms.DockStyle.Fill
		Me.sccControl.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.None
		Me.sccControl.Horizontal = False
		Me.sccControl.Location = New System.Drawing.Point(0, 0)
		Me.sccControl.Name = "sccControl"
		Me.sccControl.Panel1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple
		Me.sccControl.Panel1.CaptionLocation = DevExpress.Utils.Locations.Left
		Me.sccControl.Panel1.Controls.Add(Me.pcRapportDaten)
		Me.sccControl.Panel1.MinSize = 60
		Me.sccControl.Panel1.Padding = New System.Windows.Forms.Padding(5)
		Me.sccControl.Panel2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple
		Me.sccControl.Panel2.Controls.Add(Me.pnlPDFViewer)
		Me.sccControl.Panel2.MinSize = 400
		Me.sccControl.Panel2.Padding = New System.Windows.Forms.Padding(5)
		Me.sccControl.Size = New System.Drawing.Size(1058, 502)
		Me.sccControl.SplitterPosition = 93
		Me.sccControl.TabIndex = 5
		Me.sccControl.Text = "Details"
		'
		'pcRapportDaten
		'
		Me.pcRapportDaten.Controls.Add(Me.pnlScanData)
		Me.pcRapportDaten.Controls.Add(Me.pnlReportData)
		Me.pcRapportDaten.Controls.Add(Me.btnSaveContent)
		Me.pcRapportDaten.Location = New System.Drawing.Point(0, 1)
		Me.pcRapportDaten.Name = "pcRapportDaten"
		Me.pcRapportDaten.Size = New System.Drawing.Size(967, 84)
		Me.pcRapportDaten.TabIndex = 4
		'
		'pnlScanData
		'
		Me.pnlScanData.Controls.Add(Me.lblKategorie)
		Me.pnlScanData.Controls.Add(Me.lueRecordNumber)
		Me.pnlScanData.Controls.Add(Me.lblRecordNumber)
		Me.pnlScanData.Controls.Add(Me.lueCategory)
		Me.pnlScanData.Location = New System.Drawing.Point(33, 10)
		Me.pnlScanData.Name = "pnlScanData"
		Me.pnlScanData.Size = New System.Drawing.Size(748, 29)
		Me.pnlScanData.TabIndex = 18
		Me.pnlScanData.Visible = False
		'
		'lblKategorie
		'
		Me.lblKategorie.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblKategorie.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblKategorie.Location = New System.Drawing.Point(365, 8)
		Me.lblKategorie.Name = "lblKategorie"
		Me.lblKategorie.Size = New System.Drawing.Size(121, 13)
		Me.lblKategorie.TabIndex = 156
		Me.lblKategorie.Text = "Kategorie"
		Me.lblKategorie.Visible = False
		'
		'lueRecordNumber
		'
		Me.lueRecordNumber.Location = New System.Drawing.Point(145, 5)
		Me.lueRecordNumber.Name = "lueRecordNumber"
		SerializableAppearanceObject5.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject5.Options.UseForeColor = True
		SerializableAppearanceObject6.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject6.Options.UseForeColor = True
		SerializableAppearanceObject7.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject7.Options.UseForeColor = True
		SerializableAppearanceObject8.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject8.Options.UseForeColor = True
		Me.lueRecordNumber.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject5, SerializableAppearanceObject6, SerializableAppearanceObject7, SerializableAppearanceObject8, "", Nothing, Nothing, True), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, CType(resources.GetObject("lueRecordNumber.Properties.Buttons"), System.Drawing.Image), New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject9, SerializableAppearanceObject10, SerializableAppearanceObject11, SerializableAppearanceObject12, "", Nothing, Nothing, True)})
		Me.lueRecordNumber.Properties.NullText = ""
		Me.lueRecordNumber.Properties.View = Me.gvRecordNumber
		Me.lueRecordNumber.Size = New System.Drawing.Size(195, 22)
		Me.lueRecordNumber.TabIndex = 155
		'
		'gvRecordNumber
		'
		Me.gvRecordNumber.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvRecordNumber.Name = "gvRecordNumber"
		Me.gvRecordNumber.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvRecordNumber.OptionsView.ShowGroupPanel = False
		'
		'lblRecordNumber
		'
		Me.lblRecordNumber.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblRecordNumber.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblRecordNumber.Location = New System.Drawing.Point(18, 8)
		Me.lblRecordNumber.Name = "lblRecordNumber"
		Me.lblRecordNumber.Size = New System.Drawing.Size(121, 13)
		Me.lblRecordNumber.TabIndex = 11
		Me.lblRecordNumber.Text = "Kandidat"
		'
		'lueCategory
		'
		Me.lueCategory.Location = New System.Drawing.Point(492, 5)
		Me.lueCategory.Name = "lueCategory"
		SerializableAppearanceObject13.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject13.Options.UseForeColor = True
		SerializableAppearanceObject14.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject14.Options.UseForeColor = True
		SerializableAppearanceObject15.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject15.Options.UseForeColor = True
		SerializableAppearanceObject16.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject16.Options.UseForeColor = True
		Me.lueCategory.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject13, SerializableAppearanceObject14, SerializableAppearanceObject15, SerializableAppearanceObject16, "", Nothing, Nothing, True)})
		Me.lueCategory.Properties.NullText = ""
		Me.lueCategory.Size = New System.Drawing.Size(234, 20)
		Me.lueCategory.TabIndex = 157
		'
		'pnlReportData
		'
		Me.pnlReportData.Controls.Add(Me.lblRapperNr)
		Me.pnlReportData.Controls.Add(Me.txt_Beginn)
		Me.pnlReportData.Controls.Add(Me.txt_Ende)
		Me.pnlReportData.Controls.Add(Me.RadialMenu1)
		Me.pnlReportData.Controls.Add(Me.txtKW)
		Me.pnlReportData.Controls.Add(Me.lblErfasstzeile)
		Me.pnlReportData.Controls.Add(Me.lueReportNumber)
		Me.pnlReportData.Controls.Add(Me.lueReportLineID)
		Me.pnlReportData.Location = New System.Drawing.Point(33, 44)
		Me.pnlReportData.Name = "pnlReportData"
		Me.pnlReportData.Size = New System.Drawing.Size(748, 37)
		Me.pnlReportData.TabIndex = 17
		Me.pnlReportData.Visible = False
		'
		'lblRapperNr
		'
		Me.lblRapperNr.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.lblRapperNr.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblRapperNr.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblRapperNr.Location = New System.Drawing.Point(18, 9)
		Me.lblRapperNr.Name = "lblRapperNr"
		Me.lblRapperNr.Size = New System.Drawing.Size(121, 13)
		Me.lblRapperNr.TabIndex = 11
		Me.lblRapperNr.TabStop = True
		Me.lblRapperNr.Text = "Rapport-Nr."
		'
		'txt_Beginn
		'
		Me.txt_Beginn.Location = New System.Drawing.Point(574, 6)
		Me.txt_Beginn.Name = "txt_Beginn"
		Me.txt_Beginn.Size = New System.Drawing.Size(73, 20)
		Me.txt_Beginn.TabIndex = 13
		'
		'txt_Ende
		'
		Me.txt_Ende.Location = New System.Drawing.Point(653, 6)
		Me.txt_Ende.Name = "txt_Ende"
		Me.txt_Ende.Size = New System.Drawing.Size(73, 20)
		Me.txt_Ende.TabIndex = 14
		'
		'RadialMenu1
		'
		Me.RadialMenu1.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.rmRPOpen, Me.rmRPDelete, Me.rmRPPrint})
		Me.RadialMenu1.Location = New System.Drawing.Point(346, 4)
		Me.RadialMenu1.Name = "RadialMenu1"
		Me.RadialMenu1.Size = New System.Drawing.Size(28, 28)
		Me.RadialMenu1.Symbol = ""
		Me.RadialMenu1.TabIndex = 16
		Me.RadialMenu1.Text = "RadialMenu1"
		'
		'rmRPOpen
		'
		Me.rmRPOpen.Name = "rmRPOpen"
		Me.rmRPOpen.Symbol = ""
		Me.rmRPOpen.Text = "Öffnen"
		'
		'rmRPDelete
		'
		Me.rmRPDelete.Name = "rmRPDelete"
		Me.rmRPDelete.Symbol = ""
		Me.rmRPDelete.Text = "Löschen"
		'
		'rmRPPrint
		'
		Me.rmRPPrint.Name = "rmRPPrint"
		Me.rmRPPrint.Symbol = ""
		Me.rmRPPrint.Text = "Drucken"
		'
		'txtKW
		'
		Me.txtKW.Location = New System.Drawing.Point(302, 5)
		Me.txtKW.Name = "txtKW"
		Me.txtKW.Size = New System.Drawing.Size(38, 20)
		Me.txtKW.TabIndex = 15
		'
		'lblErfasstzeile
		'
		Me.lblErfasstzeile.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.lblErfasstzeile.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblErfasstzeile.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblErfasstzeile.Location = New System.Drawing.Point(387, 8)
		Me.lblErfasstzeile.Name = "lblErfasstzeile"
		Me.lblErfasstzeile.Size = New System.Drawing.Size(99, 13)
		Me.lblErfasstzeile.TabIndex = 12
		Me.lblErfasstzeile.TabStop = True
		Me.lblErfasstzeile.Text = "Erfasste Zeile"
		'
		'lueReportNumber
		'
		Me.lueReportNumber.Location = New System.Drawing.Point(145, 5)
		Me.lueReportNumber.Name = "lueReportNumber"
		Me.lueReportNumber.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.lueReportNumber.Properties.NullText = ""
		Me.lueReportNumber.Properties.View = Me.gvReportNumber
		Me.lueReportNumber.Size = New System.Drawing.Size(151, 20)
		Me.lueReportNumber.TabIndex = 0
		'
		'gvReportNumber
		'
		Me.gvReportNumber.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvReportNumber.Name = "gvReportNumber"
		Me.gvReportNumber.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvReportNumber.OptionsView.ShowGroupPanel = False
		'
		'lueReportLineID
		'
		Me.lueReportLineID.Location = New System.Drawing.Point(492, 5)
		Me.lueReportLineID.Name = "lueReportLineID"
		Me.lueReportLineID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.lueReportLineID.Properties.NullText = ""
		Me.lueReportLineID.Properties.View = Me.gvReportLineID
		Me.lueReportLineID.Size = New System.Drawing.Size(76, 20)
		Me.lueReportLineID.TabIndex = 5
		'
		'gvReportLineID
		'
		Me.gvReportLineID.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvReportLineID.Name = "gvReportLineID"
		Me.gvReportLineID.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvReportLineID.OptionsView.ShowGroupPanel = False
		'
		'btnSaveContent
		'
		Me.btnSaveContent.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnSaveContent.Image = CType(resources.GetObject("btnSaveContent.Image"), System.Drawing.Image)
		Me.btnSaveContent.Location = New System.Drawing.Point(797, 14)
		Me.btnSaveContent.Name = "btnSaveContent"
		Me.btnSaveContent.Size = New System.Drawing.Size(159, 33)
		Me.btnSaveContent.TabIndex = 6
		Me.btnSaveContent.Text = "Änderungen speichern"
		'
		'pnlPDFViewer
		'
		Me.pnlPDFViewer.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.pnlPDFViewer.Appearance.BackColor = System.Drawing.Color.White
		Me.pnlPDFViewer.Appearance.Options.UseBackColor = True
		Me.pnlPDFViewer.Controls.Add(Me.PdfViewer1)
		Me.pnlPDFViewer.Location = New System.Drawing.Point(1, 43)
		Me.pnlPDFViewer.Name = "pnlPDFViewer"
		Me.pnlPDFViewer.Size = New System.Drawing.Size(1044, 349)
		Me.pnlPDFViewer.TabIndex = 2
		'
		'PdfViewer1
		'
		Me.PdfViewer1.Location = New System.Drawing.Point(25, 54)
		Me.PdfViewer1.MenuManager = Me.RibbonControl1
		Me.PdfViewer1.Name = "PdfViewer1"
		Me.PdfViewer1.Size = New System.Drawing.Size(648, 222)
		Me.PdfViewer1.TabIndex = 1
		'
		'RibbonControl1
		'
		Me.RibbonControl1.ApplicationIcon = CType(resources.GetObject("RibbonControl1.ApplicationIcon"), System.Drawing.Bitmap)
		Me.RibbonControl1.Controller = Me.BarAndDockingController11
		Me.RibbonControl1.ExpandCollapseItem.Id = 0
		Me.RibbonControl1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.RibbonControl1.ExpandCollapseItem, Me.beiRPNumber, Me.beiKW, Me.bbiOpenOneRP, Me.bbiSaveIndividualReportIntoDb, Me.bbiDeleteIndividualReport, Me.bbiRotate_, Me.bbiRotate, Me.bbiPrintIndividualReport, Me.bbiZoomOneIn, Me.bbiZoomOneOut, Me.bbiActualSizeOne, Me.bbiSaveIndividualReportIntoFile, Me.bsiLblRapportnummer, Me.lblExistsRPNr, Me.bsiLblRapportzeile, Me.lblExistsRPLData, Me.bChkMAWOS, Me.bChkKDWOS, Me.bsiState, Me.bsiInfo})
		Me.RibbonControl1.Location = New System.Drawing.Point(0, 0)
		Me.RibbonControl1.MaxItemId = 30
		Me.RibbonControl1.Name = "RibbonControl1"
		Me.RibbonControl1.Pages.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPage() {Me.RibbonPage1})
		Me.RibbonControl1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemTextEdit1, Me.RepositoryItemTextEdit2, Me.RepositoryItemTextEdit3, Me.RepositoryItemButtonEdit1, Me.RepositoryItemButtonEdit2, Me.RepositoryItemSpinEdit1, Me.RepositoryItemComboBox1, Me.RepositoryItemCheckEdit1, Me.RepositoryItemCheckEdit2})
		Me.RibbonControl1.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.Hide
		Me.RibbonControl1.ShowToolbarCustomizeItem = False
		Me.RibbonControl1.Size = New System.Drawing.Size(1272, 143)
		Me.RibbonControl1.StatusBar = Me.RibbonStatusBar1
		Me.RibbonControl1.Toolbar.ShowCustomizeItem = False
		Me.RibbonControl1.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden
		'
		'beiRPNumber
		'
		Me.beiRPNumber.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right
		Me.beiRPNumber.Caption = "Rapport-Nr."
		Me.beiRPNumber.Edit = Me.RepositoryItemTextEdit2
		Me.beiRPNumber.EditWidth = 100
		Me.beiRPNumber.Enabled = False
		Me.beiRPNumber.Id = 3
		Me.beiRPNumber.Name = "beiRPNumber"
		'
		'RepositoryItemTextEdit2
		'
		Me.RepositoryItemTextEdit2.AutoHeight = False
		Me.RepositoryItemTextEdit2.Name = "RepositoryItemTextEdit2"
		'
		'beiKW
		'
		Me.beiKW.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right
		Me.beiKW.Caption = "Kalenderwoche"
		Me.beiKW.Edit = Me.RepositoryItemTextEdit3
		Me.beiKW.Enabled = False
		Me.beiKW.Id = 4
		Me.beiKW.Name = "beiKW"
		'
		'RepositoryItemTextEdit3
		'
		Me.RepositoryItemTextEdit3.AutoHeight = False
		Me.RepositoryItemTextEdit3.Name = "RepositoryItemTextEdit3"
		'
		'bbiOpenOneRP
		'
		Me.bbiOpenOneRP.Caption = "PDF-Datei öffnen"
		Me.bbiOpenOneRP.Glyph = CType(resources.GetObject("bbiOpenOneRP.Glyph"), System.Drawing.Image)
		Me.bbiOpenOneRP.Id = 8
		Me.bbiOpenOneRP.Name = "bbiOpenOneRP"
		'
		'bbiSaveIndividualReportIntoDb
		'
		Me.bbiSaveIndividualReportIntoDb.Caption = "PDF-Datei in Datenbank speichern"
		Me.bbiSaveIndividualReportIntoDb.Glyph = CType(resources.GetObject("bbiSaveIndividualReportIntoDb.Glyph"), System.Drawing.Image)
		Me.bbiSaveIndividualReportIntoDb.Id = 9
		Me.bbiSaveIndividualReportIntoDb.Name = "bbiSaveIndividualReportIntoDb"
		'
		'bbiDeleteIndividualReport
		'
		Me.bbiDeleteIndividualReport.Caption = "Dokument löschen"
		Me.bbiDeleteIndividualReport.Glyph = CType(resources.GetObject("bbiDeleteIndividualReport.Glyph"), System.Drawing.Image)
		Me.bbiDeleteIndividualReport.Id = 10
		Me.bbiDeleteIndividualReport.Name = "bbiDeleteIndividualReport"
		'
		'bbiRotate_
		'
		Me.bbiRotate_.Caption = "Nach rechts drehen"
		Me.bbiRotate_.Glyph = CType(resources.GetObject("bbiRotate_.Glyph"), System.Drawing.Image)
		Me.bbiRotate_.Id = 11
		Me.bbiRotate_.Name = "bbiRotate_"
		'
		'bbiRotate
		'
		Me.bbiRotate.Caption = "Nach links drehen"
		Me.bbiRotate.Glyph = CType(resources.GetObject("bbiRotate.Glyph"), System.Drawing.Image)
		Me.bbiRotate.Id = 12
		Me.bbiRotate.Name = "bbiRotate"
		'
		'bbiPrintIndividualReport
		'
		Me.bbiPrintIndividualReport.Caption = "PDF-Datei drucken"
		Me.bbiPrintIndividualReport.Glyph = CType(resources.GetObject("bbiPrintIndividualReport.Glyph"), System.Drawing.Image)
		Me.bbiPrintIndividualReport.Id = 13
		Me.bbiPrintIndividualReport.Name = "bbiPrintIndividualReport"
		'
		'bbiZoomOneIn
		'
		Me.bbiZoomOneIn.Caption = "Vergrössern"
		Me.bbiZoomOneIn.Glyph = CType(resources.GetObject("bbiZoomOneIn.Glyph"), System.Drawing.Image)
		Me.bbiZoomOneIn.Id = 14
		Me.bbiZoomOneIn.Name = "bbiZoomOneIn"
		'
		'bbiZoomOneOut
		'
		Me.bbiZoomOneOut.Caption = "Verkleinern"
		Me.bbiZoomOneOut.Glyph = CType(resources.GetObject("bbiZoomOneOut.Glyph"), System.Drawing.Image)
		Me.bbiZoomOneOut.Id = 15
		Me.bbiZoomOneOut.Name = "bbiZoomOneOut"
		'
		'bbiActualSizeOne
		'
		Me.bbiActualSizeOne.Caption = "Aktuelle Grösse"
		Me.bbiActualSizeOne.Glyph = CType(resources.GetObject("bbiActualSizeOne.Glyph"), System.Drawing.Image)
		Me.bbiActualSizeOne.Id = 16
		Me.bbiActualSizeOne.Name = "bbiActualSizeOne"
		'
		'bbiSaveIndividualReportIntoFile
		'
		Me.bbiSaveIndividualReportIntoFile.Caption = "Als Datei speichern"
		Me.bbiSaveIndividualReportIntoFile.Glyph = CType(resources.GetObject("bbiSaveIndividualReportIntoFile.Glyph"), System.Drawing.Image)
		Me.bbiSaveIndividualReportIntoFile.Id = 17
		Me.bbiSaveIndividualReportIntoFile.Name = "bbiSaveIndividualReportIntoFile"
		'
		'bsiLblRapportnummer
		'
		Me.bsiLblRapportnummer.Caption = "Rapportnummer"
		Me.bsiLblRapportnummer.Id = 20
		Me.bsiLblRapportnummer.Name = "bsiLblRapportnummer"
		Me.bsiLblRapportnummer.TextAlignment = System.Drawing.StringAlignment.Near
		'
		'lblExistsRPNr
		'
		Me.lblExistsRPNr.Caption = "0"
		Me.lblExistsRPNr.Description = "Rapportnummer"
		Me.lblExistsRPNr.Id = 21
		Me.lblExistsRPNr.Name = "lblExistsRPNr"
		Me.lblExistsRPNr.TextAlignment = System.Drawing.StringAlignment.Near
		'
		'bsiLblRapportzeile
		'
		Me.bsiLblRapportzeile.Caption = "Rapportzeilen"
		Me.bsiLblRapportzeile.Id = 22
		Me.bsiLblRapportzeile.Name = "bsiLblRapportzeile"
		Me.bsiLblRapportzeile.TextAlignment = System.Drawing.StringAlignment.Near
		'
		'lblExistsRPLData
		'
		Me.lblExistsRPLData.Caption = "0-0"
		Me.lblExistsRPLData.Description = "Rapportzeilen Nummer | Kalenderwoche"
		Me.lblExistsRPLData.Id = 23
		Me.lblExistsRPLData.Name = "lblExistsRPLData"
		Me.lblExistsRPLData.TextAlignment = System.Drawing.StringAlignment.Near
		'
		'bChkMAWOS
		'
		Me.bChkMAWOS.AllowHtmlText = DevExpress.Utils.DefaultBoolean.[True]
		Me.bChkMAWOS.Caption = "Rapporte an WOS-Kandidaten senden"
		Me.bChkMAWOS.CaptionAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.bChkMAWOS.Edit = Me.RepositoryItemCheckEdit1
		Me.bChkMAWOS.Id = 26
		Me.bChkMAWOS.Name = "bChkMAWOS"
		'
		'RepositoryItemCheckEdit1
		'
		Me.RepositoryItemCheckEdit1.AutoHeight = False
		Me.RepositoryItemCheckEdit1.Caption = "Check"
		Me.RepositoryItemCheckEdit1.Name = "RepositoryItemCheckEdit1"
		'
		'bChkKDWOS
		'
		Me.bChkKDWOS.AllowHtmlText = DevExpress.Utils.DefaultBoolean.[True]
		Me.bChkKDWOS.Caption = "Rapporte an WOS-Kunden senden"
		Me.bChkKDWOS.CaptionAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.bChkKDWOS.Edit = Me.RepositoryItemCheckEdit2
		Me.bChkKDWOS.Id = 27
		Me.bChkKDWOS.Name = "bChkKDWOS"
		'
		'RepositoryItemCheckEdit2
		'
		Me.RepositoryItemCheckEdit2.AutoHeight = False
		Me.RepositoryItemCheckEdit2.Caption = "Check"
		Me.RepositoryItemCheckEdit2.Name = "RepositoryItemCheckEdit2"
		'
		'bsiState
		'
		Me.bsiState.Caption = "Bereit"
		Me.bsiState.CategoryGuid = New System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537")
		Me.bsiState.Id = 28
		Me.bsiState.Name = "bsiState"
		Me.bsiState.TextAlignment = System.Drawing.StringAlignment.Near
		Me.bsiState.Width = 400
		'
		'bsiInfo
		'
		Me.bsiInfo.Caption = "Info"
		Me.bsiInfo.CategoryGuid = New System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537")
		Me.bsiInfo.Id = 29
		Me.bsiInfo.Name = "bsiInfo"
		Me.bsiInfo.TextAlignment = System.Drawing.StringAlignment.Near
		'
		'RibbonPage1
		'
		Me.RibbonPage1.Groups.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageGroup() {Me.rpgEinzelnrapport, Me.rpgDokumentseite, Me.rpgRapportinfo, Me.rpgWOS})
		Me.RibbonPage1.Name = "RibbonPage1"
		Me.RibbonPage1.Text = "Einzelne Rapportdaten"
		'
		'rpgEinzelnrapport
		'
		Me.rpgEinzelnrapport.ItemLinks.Add(Me.bbiOpenOneRP, True)
		Me.rpgEinzelnrapport.ItemLinks.Add(Me.bbiSaveIndividualReportIntoDb)
		Me.rpgEinzelnrapport.ItemLinks.Add(Me.bbiPrintIndividualReport)
		Me.rpgEinzelnrapport.ItemLinks.Add(Me.bbiSaveIndividualReportIntoFile, True)
		Me.rpgEinzelnrapport.ItemLinks.Add(Me.bbiDeleteIndividualReport)
		Me.rpgEinzelnrapport.Name = "rpgEinzelnrapport"
		Me.rpgEinzelnrapport.Text = "Einzelne Rapportdaten"
		'
		'rpgDokumentseite
		'
		Me.rpgDokumentseite.ItemLinks.Add(Me.bbiRotate)
		Me.rpgDokumentseite.ItemLinks.Add(Me.bbiRotate_)
		Me.rpgDokumentseite.ItemLinks.Add(Me.bbiZoomOneIn, True)
		Me.rpgDokumentseite.ItemLinks.Add(Me.bbiZoomOneOut)
		Me.rpgDokumentseite.ItemLinks.Add(Me.bbiActualSizeOne)
		Me.rpgDokumentseite.Name = "rpgDokumentseite"
		Me.rpgDokumentseite.Text = "Dokumentenseiten"
		'
		'rpgRapportinfo
		'
		Me.rpgRapportinfo.ItemLinks.Add(Me.bsiLblRapportnummer)
		Me.rpgRapportinfo.ItemLinks.Add(Me.bsiLblRapportzeile)
		Me.rpgRapportinfo.ItemLinks.Add(Me.lblExistsRPNr, False, "", "", True)
		Me.rpgRapportinfo.ItemLinks.Add(Me.lblExistsRPLData, False, "", "", True)
		Me.rpgRapportinfo.Name = "rpgRapportinfo"
		Me.rpgRapportinfo.Text = "Rapportinformationen"
		'
		'rpgWOS
		'
		Me.rpgWOS.ItemLinks.Add(Me.bChkMAWOS)
		Me.rpgWOS.ItemLinks.Add(Me.bChkKDWOS)
		Me.rpgWOS.Name = "rpgWOS"
		Me.rpgWOS.Text = "WOS-Einstellungen"
		'
		'RepositoryItemTextEdit1
		'
		Me.RepositoryItemTextEdit1.AutoHeight = False
		Me.RepositoryItemTextEdit1.Name = "RepositoryItemTextEdit1"
		'
		'RepositoryItemButtonEdit1
		'
		Me.RepositoryItemButtonEdit1.AutoHeight = False
		Me.RepositoryItemButtonEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.RepositoryItemButtonEdit1.Name = "RepositoryItemButtonEdit1"
		'
		'RepositoryItemButtonEdit2
		'
		Me.RepositoryItemButtonEdit2.AutoHeight = False
		Me.RepositoryItemButtonEdit2.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.RepositoryItemButtonEdit2.Name = "RepositoryItemButtonEdit2"
		'
		'RepositoryItemSpinEdit1
		'
		Me.RepositoryItemSpinEdit1.AllowNullInput = DevExpress.Utils.DefaultBoolean.[True]
		Me.RepositoryItemSpinEdit1.AutoHeight = False
		Me.RepositoryItemSpinEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.RepositoryItemSpinEdit1.Name = "RepositoryItemSpinEdit1"
		'
		'RepositoryItemComboBox1
		'
		Me.RepositoryItemComboBox1.AutoHeight = False
		Me.RepositoryItemComboBox1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.DropDown, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleLeft, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject17, SerializableAppearanceObject18, SerializableAppearanceObject19, SerializableAppearanceObject20, "", Nothing, Nothing, True)})
		Me.RepositoryItemComboBox1.Name = "RepositoryItemComboBox1"
		'
		'RibbonStatusBar1
		'
		Me.RibbonStatusBar1.ItemLinks.Add(Me.bsiState)
		Me.RibbonStatusBar1.ItemLinks.Add(Me.bsiInfo, True)
		Me.RibbonStatusBar1.Location = New System.Drawing.Point(0, 669)
		Me.RibbonStatusBar1.Name = "RibbonStatusBar1"
		Me.RibbonStatusBar1.Ribbon = Me.RibbonControl1
		Me.RibbonStatusBar1.Size = New System.Drawing.Size(1272, 31)
		'
		'navbarImageListLarge
		'
		Me.navbarImageListLarge.ImageStream = CType(resources.GetObject("navbarImageListLarge.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.navbarImageListLarge.TransparentColor = System.Drawing.Color.Transparent
		Me.navbarImageListLarge.Images.SetKeyName(0, "Mail_16x16.png")
		Me.navbarImageListLarge.Images.SetKeyName(1, "Organizer_16x16.png")
		'
		'appMenu
		'
		Me.appMenu.Name = "appMenu"
		Me.appMenu.ShowRightPane = True
		'
		'ribbonImageCollection
		'
		Me.ribbonImageCollection.ImageStream = CType(resources.GetObject("ribbonImageCollection.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
		Me.ribbonImageCollection.Images.SetKeyName(0, "Ribbon_New_16x16.png")
		Me.ribbonImageCollection.Images.SetKeyName(1, "Ribbon_Open_16x16.png")
		Me.ribbonImageCollection.Images.SetKeyName(2, "Ribbon_Close_16x16.png")
		Me.ribbonImageCollection.Images.SetKeyName(3, "Ribbon_Find_16x16.png")
		Me.ribbonImageCollection.Images.SetKeyName(4, "Ribbon_Save_16x16.png")
		Me.ribbonImageCollection.Images.SetKeyName(5, "Ribbon_SaveAs_16x16.png")
		Me.ribbonImageCollection.Images.SetKeyName(6, "Ribbon_Exit_16x16.png")
		Me.ribbonImageCollection.Images.SetKeyName(7, "Ribbon_Content_16x16.png")
		Me.ribbonImageCollection.Images.SetKeyName(8, "Ribbon_Info_16x16.png")
		Me.ribbonImageCollection.Images.SetKeyName(9, "Ribbon_Bold_16x16.png")
		Me.ribbonImageCollection.Images.SetKeyName(10, "Ribbon_Italic_16x16.png")
		Me.ribbonImageCollection.Images.SetKeyName(11, "Ribbon_Underline_16x16.png")
		Me.ribbonImageCollection.Images.SetKeyName(12, "Ribbon_AlignLeft_16x16.png")
		Me.ribbonImageCollection.Images.SetKeyName(13, "Ribbon_AlignCenter_16x16.png")
		Me.ribbonImageCollection.Images.SetKeyName(14, "Ribbon_AlignRight_16x16.png")
		'
		'ribbonImageCollectionLarge
		'
		Me.ribbonImageCollectionLarge.ImageSize = New System.Drawing.Size(32, 32)
		Me.ribbonImageCollectionLarge.ImageStream = CType(resources.GetObject("ribbonImageCollectionLarge.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
		Me.ribbonImageCollectionLarge.Images.SetKeyName(0, "Ribbon_New_32x32.png")
		Me.ribbonImageCollectionLarge.Images.SetKeyName(1, "Ribbon_Open_32x32.png")
		Me.ribbonImageCollectionLarge.Images.SetKeyName(2, "Ribbon_Close_32x32.png")
		Me.ribbonImageCollectionLarge.Images.SetKeyName(3, "Ribbon_Find_32x32.png")
		Me.ribbonImageCollectionLarge.Images.SetKeyName(4, "Ribbon_Save_32x32.png")
		Me.ribbonImageCollectionLarge.Images.SetKeyName(5, "Ribbon_SaveAs_32x32.png")
		Me.ribbonImageCollectionLarge.Images.SetKeyName(6, "Ribbon_Exit_32x32.png")
		Me.ribbonImageCollectionLarge.Images.SetKeyName(7, "Ribbon_Content_32x32.png")
		Me.ribbonImageCollectionLarge.Images.SetKeyName(8, "Ribbon_Info_32x32.png")
		'
		'PanelControl2
		'
		Me.PanelControl2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.PanelControl2.Controls.Add(Me.xtabControl)
		Me.PanelControl2.Location = New System.Drawing.Point(0, 141)
		Me.PanelControl2.Name = "PanelControl2"
		Me.PanelControl2.Size = New System.Drawing.Size(1272, 537)
		Me.PanelControl2.TabIndex = 13
		'
		'frmRPDocScan
		'
		Me.AllowFormGlass = DevExpress.Utils.DefaultBoolean.[True]
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(1272, 700)
		Me.Controls.Add(Me.RibbonStatusBar1)
		Me.Controls.Add(Me.PanelControl2)
		Me.Controls.Add(Me.RibbonControl1)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.Name = "frmRPDocScan"
		Me.Ribbon = Me.RibbonControl1
		Me.StatusBar = Me.RibbonStatusBar1
		Me.Text = "Eingescannte Rapporte"
		CType(Me.BarAndDockingController11, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.BarAndDockingController12, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.BarAndDockingController10, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.BarAndDockingController9, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.BarAndDockingController8, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.BarAndDockingController5, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.BarAndDockingController7, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.BarAndDockingController6, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.BarAndDockingController4, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.BarAndDockingController3, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.BarAndDockingController2, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.BarAndDockingController1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.xtabControl, System.ComponentModel.ISupportInitialize).EndInit()
		Me.xtabControl.ResumeLayout(False)
		Me.xtabSummery.ResumeLayout(False)
		CType(Me.sccRPStd, System.ComponentModel.ISupportInitialize).EndInit()
		Me.sccRPStd.ResumeLayout(False)
		CType(Me.pcSPStdInfoHeader, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pcSPStdInfoHeader.ResumeLayout(False)
		CType(Me.grdImportScanContent, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvImportScanContent, System.ComponentModel.ISupportInitialize).EndInit()
		Me.xtabScanedDoc.ResumeLayout(False)
		CType(Me.sccScanRPMain, System.ComponentModel.ISupportInitialize).EndInit()
		Me.sccScanRPMain.ResumeLayout(False)
		CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.PanelControl1.ResumeLayout(False)
		CType(Me.grdScanContent, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvScanContent, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lueScaned.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvScanedDoc, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.sccControl, System.ComponentModel.ISupportInitialize).EndInit()
		Me.sccControl.ResumeLayout(False)
		CType(Me.pcRapportDaten, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pcRapportDaten.ResumeLayout(False)
		CType(Me.pnlScanData, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pnlScanData.ResumeLayout(False)
		CType(Me.lueRecordNumber.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvRecordNumber, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lueCategory.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.pnlReportData, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pnlReportData.ResumeLayout(False)
		CType(Me.txt_Beginn.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txt_Ende.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtKW.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lueReportNumber.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvReportNumber, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lueReportLineID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvReportLineID, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.pnlPDFViewer, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pnlPDFViewer.ResumeLayout(False)
		CType(Me.RibbonControl1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.RepositoryItemTextEdit2, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.RepositoryItemTextEdit3, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.RepositoryItemCheckEdit1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.RepositoryItemCheckEdit2, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.RepositoryItemTextEdit1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.RepositoryItemButtonEdit1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.RepositoryItemButtonEdit2, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.RepositoryItemSpinEdit1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.RepositoryItemComboBox1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.appMenu, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.ribbonImageCollection, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.ribbonImageCollectionLarge, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).EndInit()
		Me.PanelControl2.ResumeLayout(False)
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Private WithEvents appMenu As DevExpress.XtraBars.Ribbon.ApplicationMenu
	Private WithEvents ribbonImageCollection As DevExpress.Utils.ImageCollection
	Private WithEvents ribbonImageCollectionLarge As DevExpress.Utils.ImageCollection
	Private WithEvents navbarImageList As System.Windows.Forms.ImageList
	Private WithEvents navbarImageListLarge As System.Windows.Forms.ImageList
	Friend WithEvents pnlPDFViewer As DevExpress.XtraEditors.PanelControl
	Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
	Friend WithEvents grdScanContent As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvScanContent As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents xtabControl As DevExpress.XtraTab.XtraTabControl
	Friend WithEvents xtabSummery As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents xtabScanedDoc As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents sccScanRPMain As DevExpress.XtraEditors.SplitContainerControl
	Friend WithEvents btnSaveContent As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents sccControl As DevExpress.XtraEditors.SplitContainerControl
	Friend WithEvents pcRapportDaten As DevExpress.XtraEditors.PanelControl
	Friend WithEvents grdImportScanContent As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvImportScanContent As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents sccRPStd As DevExpress.XtraEditors.SplitContainerControl
	Friend WithEvents pcSPStdInfoHeader As DevExpress.XtraEditors.PanelControl
	Friend WithEvents btnSaveCheckedContentInfoLocalDb As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents PanelControl2 As DevExpress.XtraEditors.PanelControl
	Friend WithEvents RibbonControl1 As DevExpress.XtraBars.Ribbon.RibbonControl
	Friend WithEvents beiRPNumber As DevExpress.XtraBars.BarEditItem
	Friend WithEvents RepositoryItemTextEdit2 As DevExpress.XtraEditors.Repository.RepositoryItemTextEdit
	Friend WithEvents beiKW As DevExpress.XtraBars.BarEditItem
	Friend WithEvents RepositoryItemTextEdit3 As DevExpress.XtraEditors.Repository.RepositoryItemTextEdit
	Friend WithEvents bbiOpenOneRP As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents RibbonPage1 As DevExpress.XtraBars.Ribbon.RibbonPage
	Friend WithEvents rpgEinzelnrapport As DevExpress.XtraBars.Ribbon.RibbonPageGroup
	Friend WithEvents RepositoryItemTextEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemTextEdit
	Friend WithEvents RepositoryItemButtonEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
	Friend WithEvents RepositoryItemButtonEdit2 As DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
	Friend WithEvents bbiSaveIndividualReportIntoDb As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbiDeleteIndividualReport As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbiRotate_ As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbiRotate As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents rpgDokumentseite As DevExpress.XtraBars.Ribbon.RibbonPageGroup
	Friend WithEvents bbiPrintIndividualReport As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbiZoomOneIn As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbiZoomOneOut As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbiActualSizeOne As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbiSaveIndividualReportIntoFile As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents RepositoryItemSpinEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit
	Friend WithEvents RepositoryItemComboBox1 As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
	Friend WithEvents lblRapperNr As DevExpress.XtraEditors.LabelControl
	Friend WithEvents txt_Ende As DevExpress.XtraEditors.TextEdit
	Friend WithEvents txt_Beginn As DevExpress.XtraEditors.TextEdit
	Friend WithEvents lblErfasstzeile As DevExpress.XtraEditors.LabelControl
	Friend WithEvents txtKW As DevExpress.XtraEditors.TextEdit
	Friend WithEvents bsiLblRapportnummer As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents lblExistsRPNr As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents bsiLblRapportzeile As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents lblExistsRPLData As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents rpgRapportinfo As DevExpress.XtraBars.Ribbon.RibbonPageGroup
	Friend WithEvents rpgWOS As DevExpress.XtraBars.Ribbon.RibbonPageGroup
	Friend WithEvents bChkMAWOS As DevExpress.XtraBars.BarEditItem
	Friend WithEvents RepositoryItemCheckEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
	Friend WithEvents bChkKDWOS As DevExpress.XtraBars.BarEditItem
	Friend WithEvents RepositoryItemCheckEdit2 As DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
	Friend WithEvents RadialMenu1 As DevComponents.DotNetBar.RadialMenu
	Friend WithEvents rmRPDelete As DevComponents.DotNetBar.RadialMenuItem
	Friend WithEvents rmRPPrint As DevComponents.DotNetBar.RadialMenuItem
	Friend WithEvents rmRPOpen As DevComponents.DotNetBar.RadialMenuItem
	Friend WithEvents bsiState As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents bsiInfo As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents RibbonStatusBar1 As DevExpress.XtraBars.Ribbon.RibbonStatusBar
	Friend WithEvents PdfViewer1 As DevExpress.XtraPdfViewer.PdfViewer
	Friend WithEvents lueScaned As DevExpress.XtraEditors.GridLookUpEdit
	Friend WithEvents gvScanedDoc As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents pnlScanData As DevExpress.XtraEditors.PanelControl
	Friend WithEvents lblKategorie As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lueRecordNumber As DevExpress.XtraEditors.GridLookUpEdit
	Friend WithEvents gvRecordNumber As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents lblRecordNumber As DevExpress.XtraEditors.LabelControl
	Friend WithEvents pnlReportData As DevExpress.XtraEditors.PanelControl
	Friend WithEvents lueCategory As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents lueReportNumber As DevExpress.XtraEditors.GridLookUpEdit
	Friend WithEvents gvReportNumber As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents BarAndDockingController1 As DevExpress.XtraBars.BarAndDockingController
	Friend WithEvents BarAndDockingController2 As DevExpress.XtraBars.BarAndDockingController
	Friend WithEvents BarAndDockingController3 As DevExpress.XtraBars.BarAndDockingController
	Friend WithEvents lueReportLineID As DevExpress.XtraEditors.GridLookUpEdit
	Friend WithEvents gvReportLineID As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents BarAndDockingController4 As DevExpress.XtraBars.BarAndDockingController
	Friend WithEvents BarAndDockingController6 As DevExpress.XtraBars.BarAndDockingController
	Friend WithEvents BarAndDockingController7 As DevExpress.XtraBars.BarAndDockingController
	Friend WithEvents BarAndDockingController5 As DevExpress.XtraBars.BarAndDockingController
	Friend WithEvents BarAndDockingController8 As DevExpress.XtraBars.BarAndDockingController
	Friend WithEvents BarAndDockingController9 As DevExpress.XtraBars.BarAndDockingController
	Friend WithEvents BarAndDockingController10 As DevExpress.XtraBars.BarAndDockingController
	Friend WithEvents BarAndDockingController12 As DevExpress.XtraBars.BarAndDockingController
	Friend WithEvents BarAndDockingController11 As BarAndDockingController
End Class
