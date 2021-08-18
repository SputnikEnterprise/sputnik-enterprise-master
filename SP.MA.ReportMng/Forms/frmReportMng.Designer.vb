Namespace UI

  <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
  Partial Class frmReportMng
    Inherits SP.Infrastructure.Forms.frmBase

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
			Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmReportMng))
			Me.RepositoryItemComboBox2 = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
			Me.RepositoryItemComboBox1 = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
			Me.btnShowTemplate = New DevExpress.XtraBars.BarSubItem()
			Me.xtabReportOverviewData = New DevExpress.XtraTab.XtraTabControl()
			Me.xtabEmployeeList = New DevExpress.XtraTab.XtraTabPage()
			Me.grdReportOverview = New DevExpress.XtraGrid.GridControl()
			Me.gvReportOverview = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.xtabEmployeeDetail = New DevExpress.XtraTab.XtraTabPage()
			Me.SplitContainerControl1 = New DevExpress.XtraEditors.SplitContainerControl()
			Me.ucReportDetailData = New SP.MA.ReportMng.UI.ucReportDetailData()
			Me.ucReportDetailData2 = New SP.MA.ReportMng.UI.ucReportDetailData2()
			Me.chkBtnReportInfo = New DevExpress.XtraEditors.CheckButton()
			Me.chkBtnReportDetails = New DevExpress.XtraEditors.CheckButton()
			Me.navMain = New DevExpress.XtraNavBar.NavBarControl()
			Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
			Me.RepositoryItemFontEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemFontEdit()
			Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
			Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
			Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
			Me.Bar1 = New DevExpress.XtraBars.Bar()
			Me.bsiLblGeandert = New DevExpress.XtraBars.BarStaticItem()
			Me.bsiChanged = New DevExpress.XtraBars.BarStaticItem()
			Me.bsiBtnGavInfo = New DevExpress.XtraBars.BarButtonItem()
			Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
			Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
			Me.ImageCollection1 = New DevExpress.Utils.ImageCollection(Me.components)
			Me.SplitContainerControl4 = New DevExpress.XtraEditors.SplitContainerControl()
			Me.ucMainContent = New SP.MA.ReportMng.UI.ucMainContent()
			Me.XtraTabControl1 = New DevExpress.XtraTab.XtraTabControl()
			Me.xtabBemerkung = New DevExpress.XtraTab.XtraTabPage()
			Me.ucNotes = New SP.MA.ReportMng.UI.ucNotes()
			Me.xtabGuthaben = New DevExpress.XtraTab.XtraTabPage()
			Me.ucCredit = New SP.MA.ReportMng.UI.ucCredit()
			Me.xtabLM = New DevExpress.XtraTab.XtraTabPage()
			Me.ucMonthlySalaryData = New SP.MA.ReportMng.UI.ucMonthlySalaryData()
			Me.xtabZG = New DevExpress.XtraTab.XtraTabPage()
			Me.ucAdvancePayment = New SP.MA.ReportMng.UI.ucAdvancePayment()
			Me.ucTimetableAndInfoData = New SP.MA.ReportMng.UI.ucTimetableAndInfoData()
			Me.pcNavMain = New DevExpress.XtraEditors.PanelControl()
			Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
			CType(Me.RepositoryItemComboBox2, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.RepositoryItemComboBox1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.xtabReportOverviewData, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.xtabReportOverviewData.SuspendLayout()
			Me.xtabEmployeeList.SuspendLayout()
			CType(Me.grdReportOverview, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvReportOverview, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.xtabEmployeeDetail.SuspendLayout()
			CType(Me.SplitContainerControl1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SplitContainerControl1.SuspendLayout()
			CType(Me.navMain, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.RepositoryItemFontEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.ImageCollection1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.SplitContainerControl4, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SplitContainerControl4.SuspendLayout()
			CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.XtraTabControl1.SuspendLayout()
			Me.xtabBemerkung.SuspendLayout()
			Me.xtabGuthaben.SuspendLayout()
			Me.xtabLM.SuspendLayout()
			Me.xtabZG.SuspendLayout()
			CType(Me.pcNavMain, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.pcNavMain.SuspendLayout()
			CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.PanelControl1.SuspendLayout()
			Me.SuspendLayout()
			'
			'RepositoryItemComboBox2
			'
			Me.RepositoryItemComboBox2.AutoHeight = False
			Me.RepositoryItemComboBox2.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.RepositoryItemComboBox2.Name = "RepositoryItemComboBox2"
			'
			'RepositoryItemComboBox1
			'
			Me.RepositoryItemComboBox1.AutoHeight = False
			Me.RepositoryItemComboBox1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.RepositoryItemComboBox1.Name = "RepositoryItemComboBox1"
			'
			'btnShowTemplate
			'
			Me.btnShowTemplate.Caption = "Vorlage laden"
			Me.btnShowTemplate.Id = 59
			Me.btnShowTemplate.Name = "btnShowTemplate"
			Me.btnShowTemplate.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
			Me.btnShowTemplate.ShowMenuCaption = True
			'
			'xtabReportOverviewData
			'
			Me.xtabReportOverviewData.Dock = System.Windows.Forms.DockStyle.Fill
			Me.xtabReportOverviewData.Location = New System.Drawing.Point(7, 7)
			Me.xtabReportOverviewData.Name = "xtabReportOverviewData"
			Me.xtabReportOverviewData.SelectedTabPage = Me.xtabEmployeeList
			Me.xtabReportOverviewData.Size = New System.Drawing.Size(347, 791)
			Me.xtabReportOverviewData.TabIndex = 175
			Me.xtabReportOverviewData.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabEmployeeList, Me.xtabEmployeeDetail})
			'
			'xtabEmployeeList
			'
			Me.xtabEmployeeList.Controls.Add(Me.grdReportOverview)
			Me.xtabEmployeeList.Name = "xtabEmployeeList"
			Me.xtabEmployeeList.Size = New System.Drawing.Size(345, 766)
			Me.xtabEmployeeList.Text = "Rapporte"
			'
			'grdReportOverview
			'
			Me.grdReportOverview.Dock = System.Windows.Forms.DockStyle.Fill
			Me.grdReportOverview.Location = New System.Drawing.Point(0, 0)
			Me.grdReportOverview.MainView = Me.gvReportOverview
			Me.grdReportOverview.Name = "grdReportOverview"
			Me.grdReportOverview.Size = New System.Drawing.Size(345, 766)
			Me.grdReportOverview.TabIndex = 1
			Me.grdReportOverview.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvReportOverview})
			'
			'gvReportOverview
			'
			Me.gvReportOverview.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			Me.gvReportOverview.GridControl = Me.grdReportOverview
			Me.gvReportOverview.Name = "gvReportOverview"
			Me.gvReportOverview.OptionsBehavior.Editable = False
			Me.gvReportOverview.OptionsSelection.EnableAppearanceFocusedCell = False
			Me.gvReportOverview.OptionsView.ShowGroupPanel = False
			'
			'xtabEmployeeDetail
			'
			Me.xtabEmployeeDetail.Controls.Add(Me.SplitContainerControl1)
			Me.xtabEmployeeDetail.Name = "xtabEmployeeDetail"
			Me.xtabEmployeeDetail.Size = New System.Drawing.Size(345, 765)
			Me.xtabEmployeeDetail.Text = "Details"
			'
			'SplitContainerControl1
			'
			Me.SplitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.SplitContainerControl1.Horizontal = False
			Me.SplitContainerControl1.Location = New System.Drawing.Point(0, 0)
			Me.SplitContainerControl1.Name = "SplitContainerControl1"
			Me.SplitContainerControl1.Panel1.Controls.Add(Me.ucReportDetailData)
			Me.SplitContainerControl1.Panel1.Controls.Add(Me.ucReportDetailData2)
			Me.SplitContainerControl1.Panel1.Controls.Add(Me.chkBtnReportInfo)
			Me.SplitContainerControl1.Panel1.Controls.Add(Me.chkBtnReportDetails)
			Me.SplitContainerControl1.Panel1.Padding = New System.Windows.Forms.Padding(5)
			Me.SplitContainerControl1.Panel1.Text = "Panel1"
			Me.SplitContainerControl1.Panel2.Controls.Add(Me.navMain)
			Me.SplitContainerControl1.Panel2.Padding = New System.Windows.Forms.Padding(5)
			Me.SplitContainerControl1.Panel2.Text = "Panel2"
			Me.SplitContainerControl1.Size = New System.Drawing.Size(345, 765)
			Me.SplitContainerControl1.SplitterPosition = 431
			Me.SplitContainerControl1.TabIndex = 212
			Me.SplitContainerControl1.Text = "SplitContainerControl1"
			'
			'ucReportDetailData
			'
			Me.ucReportDetailData.Dock = System.Windows.Forms.DockStyle.Bottom
			Me.ucReportDetailData.IsIntialControlDataLoaded = False
			Me.ucReportDetailData.Location = New System.Drawing.Point(5, 43)
			Me.ucReportDetailData.Name = "ucReportDetailData"
			Me.ucReportDetailData.Size = New System.Drawing.Size(335, 383)
			Me.ucReportDetailData.TabIndex = 0
			Me.ucReportDetailData.UCMediator = Nothing
			'
			'ucReportDetailData2
			'
			Me.ucReportDetailData2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.ucReportDetailData2.IsIntialControlDataLoaded = False
			Me.ucReportDetailData2.Location = New System.Drawing.Point(3, 43)
			Me.ucReportDetailData2.Name = "ucReportDetailData2"
			Me.ucReportDetailData2.Size = New System.Drawing.Size(337, 382)
			Me.ucReportDetailData2.TabIndex = 1
			Me.ucReportDetailData2.UCMediator = Nothing
			Me.ucReportDetailData2.Visible = False
			'
			'chkBtnReportInfo
			'
			Me.chkBtnReportInfo.Appearance.BackColor = System.Drawing.Color.White
			Me.chkBtnReportInfo.Appearance.BorderColor = System.Drawing.Color.Black
			Me.chkBtnReportInfo.Appearance.Options.UseBackColor = True
			Me.chkBtnReportInfo.Appearance.Options.UseBorderColor = True
			Me.chkBtnReportInfo.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple
			Me.chkBtnReportInfo.GroupIndex = 1
			Me.chkBtnReportInfo.Location = New System.Drawing.Point(61, 9)
			Me.chkBtnReportInfo.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat
			Me.chkBtnReportInfo.LookAndFeel.UseDefaultLookAndFeel = False
			Me.chkBtnReportInfo.Name = "chkBtnReportInfo"
			Me.chkBtnReportInfo.Size = New System.Drawing.Size(56, 23)
			Me.chkBtnReportInfo.TabIndex = 1
			Me.chkBtnReportInfo.TabStop = False
			Me.chkBtnReportInfo.Text = "Info"
			'
			'chkBtnReportDetails
			'
			Me.chkBtnReportDetails.Appearance.BackColor = System.Drawing.Color.White
			Me.chkBtnReportDetails.Appearance.BorderColor = System.Drawing.Color.Black
			Me.chkBtnReportDetails.Appearance.Options.UseBackColor = True
			Me.chkBtnReportDetails.Appearance.Options.UseBorderColor = True
			Me.chkBtnReportDetails.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple
			Me.chkBtnReportDetails.GroupIndex = 1
			Me.chkBtnReportDetails.Location = New System.Drawing.Point(5, 9)
			Me.chkBtnReportDetails.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat
			Me.chkBtnReportDetails.LookAndFeel.UseDefaultLookAndFeel = False
			Me.chkBtnReportDetails.Name = "chkBtnReportDetails"
			Me.chkBtnReportDetails.Size = New System.Drawing.Size(53, 23)
			Me.chkBtnReportDetails.TabIndex = 0
			Me.chkBtnReportDetails.TabStop = False
			Me.chkBtnReportDetails.Text = "Details"
			'
			'navMain
			'
			Me.navMain.Dock = System.Windows.Forms.DockStyle.Fill
			Me.navMain.Location = New System.Drawing.Point(5, 5)
			Me.navMain.Name = "navMain"
			Me.navMain.OptionsNavPane.ExpandedWidth = 335
			Me.navMain.Size = New System.Drawing.Size(335, 314)
			Me.navMain.TabIndex = 20
			Me.navMain.Text = "NavBarControl1"
			'
			'ImageList1
			'
			Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
			Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
			Me.ImageList1.Images.SetKeyName(0, "NewContentPageHS.png")
			Me.ImageList1.Images.SetKeyName(1, "Delete.ico")
			Me.ImageList1.Images.SetKeyName(2, "close.ico")
			Me.ImageList1.Images.SetKeyName(3, "Printer.ico")
			Me.ImageList1.Images.SetKeyName(4, "SAVE.GIF")
			Me.ImageList1.Images.SetKeyName(5, "searchqry.gif")
			Me.ImageList1.Images.SetKeyName(6, "Mail2New.ico")
			Me.ImageList1.Images.SetKeyName(7, "organizationtitle.png")
			Me.ImageList1.Images.SetKeyName(8, "Einsatz.png")
			Me.ImageList1.Images.SetKeyName(9, "")
			Me.ImageList1.Images.SetKeyName(10, "")
			Me.ImageList1.Images.SetKeyName(11, "crif.png")
			'
			'RepositoryItemFontEdit1
			'
			Me.RepositoryItemFontEdit1.AutoHeight = False
			Me.RepositoryItemFontEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.RepositoryItemFontEdit1.Name = "RepositoryItemFontEdit1"
			'
			'barDockControlLeft
			'
			Me.barDockControlLeft.CausesValidation = False
			Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
			Me.barDockControlLeft.Location = New System.Drawing.Point(5, 5)
			Me.barDockControlLeft.Manager = Me.BarManager1
			Me.barDockControlLeft.Size = New System.Drawing.Size(0, 805)
			'
			'barDockControlRight
			'
			Me.barDockControlRight.CausesValidation = False
			Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
			Me.barDockControlRight.Location = New System.Drawing.Point(1500, 5)
			Me.barDockControlRight.Manager = Me.BarManager1
			Me.barDockControlRight.Size = New System.Drawing.Size(0, 805)
			'
			'BarManager1
			'
			Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar1})
			Me.BarManager1.DockControls.Add(Me.barDockControlTop)
			Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
			Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
			Me.BarManager1.DockControls.Add(Me.barDockControlRight)
			Me.BarManager1.Form = Me
			Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.btnShowTemplate, Me.bsiLblGeandert, Me.bsiChanged, Me.bsiBtnGavInfo})
			Me.BarManager1.MaxItemId = 65
			Me.BarManager1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemFontEdit1, Me.RepositoryItemComboBox1, Me.RepositoryItemComboBox2})
			Me.BarManager1.StatusBar = Me.Bar1
			'
			'Bar1
			'
			Me.Bar1.BarName = "Benutzerdefiniert 2"
			Me.Bar1.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
			Me.Bar1.DockCol = 0
			Me.Bar1.DockRow = 0
			Me.Bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
			Me.Bar1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiLblGeandert, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bsiChanged), New DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, Me.bsiBtnGavInfo, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)})
			Me.Bar1.OptionsBar.AllowQuickCustomization = False
			Me.Bar1.OptionsBar.DrawDragBorder = False
			Me.Bar1.OptionsBar.UseWholeRow = True
			Me.Bar1.Text = "Benutzerdefiniert 2"
			'
			'bsiLblGeandert
			'
			Me.bsiLblGeandert.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
			Me.bsiLblGeandert.Caption = "Geändert:"
			Me.bsiLblGeandert.Id = 60
			Me.bsiLblGeandert.ItemAppearance.Normal.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.bsiLblGeandert.ItemAppearance.Normal.Options.UseFont = True
			Me.bsiLblGeandert.Name = "bsiLblGeandert"
			'
			'bsiChanged
			'
			Me.bsiChanged.Caption = "{0}"
			Me.bsiChanged.Id = 61
			Me.bsiChanged.Name = "bsiChanged"
			'
			'bsiBtnGavInfo
			'
			Me.bsiBtnGavInfo.Caption = "GAV Info"
			Me.bsiBtnGavInfo.Id = 64
			Me.bsiBtnGavInfo.ImageOptions.Image = Global.SP.MA.ReportMng.My.Resources.Resources.apply_16x16
			Me.bsiBtnGavInfo.ImageOptions.LargeImage = Global.SP.MA.ReportMng.My.Resources.Resources.apply_32x32
			Me.bsiBtnGavInfo.Name = "bsiBtnGavInfo"
			'
			'barDockControlTop
			'
			Me.barDockControlTop.CausesValidation = False
			Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
			Me.barDockControlTop.Location = New System.Drawing.Point(5, 5)
			Me.barDockControlTop.Manager = Me.BarManager1
			Me.barDockControlTop.Size = New System.Drawing.Size(1495, 0)
			'
			'barDockControlBottom
			'
			Me.barDockControlBottom.CausesValidation = False
			Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
			Me.barDockControlBottom.Location = New System.Drawing.Point(5, 810)
			Me.barDockControlBottom.Manager = Me.BarManager1
			Me.barDockControlBottom.Size = New System.Drawing.Size(1495, 26)
			'
			'ImageCollection1
			'
			Me.ImageCollection1.ImageStream = CType(resources.GetObject("ImageCollection1.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
			Me.ImageCollection1.Images.SetKeyName(0, "Plus.png")
			Me.ImageCollection1.Images.SetKeyName(1, "Save.png")
			Me.ImageCollection1.InsertGalleryImage("print_16x16.png", "images/print/print_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/print/print_16x16.png"), 2)
			Me.ImageCollection1.Images.SetKeyName(2, "print_16x16.png")
			Me.ImageCollection1.Images.SetKeyName(3, "close.ico")
			Me.ImageCollection1.Images.SetKeyName(4, "delete.png")
			Me.ImageCollection1.Images.SetKeyName(5, "technology_32x32.png")
			Me.ImageCollection1.Images.SetKeyName(6, "Hyperlink.png")
			Me.ImageCollection1.Images.SetKeyName(7, "Contact.png")
			Me.ImageCollection1.Images.SetKeyName(8, "Funds.png")
			Me.ImageCollection1.InsertGalleryImage("newtask_16x16.png", "images/tasks/newtask_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/tasks/newtask_16x16.png"), 9)
			Me.ImageCollection1.Images.SetKeyName(9, "newtask_16x16.png")
			Me.ImageCollection1.InsertGalleryImage("newcontact_16x16.png", "images/mail/newcontact_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/mail/newcontact_16x16.png"), 10)
			Me.ImageCollection1.Images.SetKeyName(10, "newcontact_16x16.png")
			Me.ImageCollection1.Images.SetKeyName(11, "AppointmentNew.png")
			Me.ImageCollection1.Images.SetKeyName(12, "Calc.png")
			Me.ImageCollection1.Images.SetKeyName(13, "AddressBook.png")
			Me.ImageCollection1.InsertGalleryImage("image_16x16.png", "images/content/image_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/content/image_16x16.png"), 14)
			Me.ImageCollection1.Images.SetKeyName(14, "image_16x16.png")
			Me.ImageCollection1.InsertGalleryImage("properties_16x16.png", "images/setup/properties_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/setup/properties_16x16.png"), 15)
			Me.ImageCollection1.Images.SetKeyName(15, "properties_16x16.png")
			'
			'SplitContainerControl4
			'
			Me.SplitContainerControl4.Dock = System.Windows.Forms.DockStyle.Fill
			Me.SplitContainerControl4.Horizontal = False
			Me.SplitContainerControl4.Location = New System.Drawing.Point(366, 5)
			Me.SplitContainerControl4.Name = "SplitContainerControl4"
			Me.SplitContainerControl4.Panel1.Controls.Add(Me.ucMainContent)
			Me.SplitContainerControl4.Panel1.Padding = New System.Windows.Forms.Padding(0, 5, 0, 0)
			Me.SplitContainerControl4.Panel1.Text = "Panel1"
			Me.SplitContainerControl4.Panel2.Controls.Add(Me.XtraTabControl1)
			Me.SplitContainerControl4.Panel2.Padding = New System.Windows.Forms.Padding(0, 5, 0, 0)
			Me.SplitContainerControl4.Panel2.Text = "Panel2"
			Me.SplitContainerControl4.Size = New System.Drawing.Size(900, 805)
			Me.SplitContainerControl4.SplitterPosition = 526
			Me.SplitContainerControl4.TabIndex = 214
			Me.SplitContainerControl4.Text = "SplitContainerControl4"
			'
			'ucMainContent
			'
			Me.ucMainContent.AnsatzValueInUI = New Decimal(New Integer() {0, 0, 0, 0})
			Me.ucMainContent.AnzahlValueInUI = New Decimal(New Integer() {0, 0, 0, 0})
			Me.ucMainContent.BasisValueInUI = New Decimal(New Integer() {0, 0, 0, 0})
			Me.ucMainContent.BetragValueInUI = New Decimal(New Integer() {0, 0, 0, 0})
			Me.ucMainContent.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucMainContent.IsIntialControlDataLoaded = False
			Me.ucMainContent.Location = New System.Drawing.Point(0, 5)
			Me.ucMainContent.MwStInUI = New Decimal(New Integer() {0, 0, 0, 0})
			Me.ucMainContent.Name = "ucMainContent"
			Me.ucMainContent.Padding = New System.Windows.Forms.Padding(5)
			Me.ucMainContent.Size = New System.Drawing.Size(900, 521)
			Me.ucMainContent.TabIndex = 1
			Me.ucMainContent.UCMediator = Nothing
			'
			'XtraTabControl1
			'
			Me.XtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.XtraTabControl1.Location = New System.Drawing.Point(0, 5)
			Me.XtraTabControl1.Name = "XtraTabControl1"
			Me.XtraTabControl1.SelectedTabPage = Me.xtabBemerkung
			Me.XtraTabControl1.Size = New System.Drawing.Size(900, 264)
			Me.XtraTabControl1.TabIndex = 213
			Me.XtraTabControl1.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabBemerkung, Me.xtabGuthaben, Me.xtabLM, Me.xtabZG})
			'
			'xtabBemerkung
			'
			Me.xtabBemerkung.Controls.Add(Me.ucNotes)
			Me.xtabBemerkung.Name = "xtabBemerkung"
			Me.xtabBemerkung.Padding = New System.Windows.Forms.Padding(5)
			Me.xtabBemerkung.Size = New System.Drawing.Size(898, 239)
			Me.xtabBemerkung.Text = "Bemerkungen"
			'
			'ucNotes
			'
			Me.ucNotes.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucNotes.IsIntialControlDataLoaded = False
			Me.ucNotes.Location = New System.Drawing.Point(5, 5)
			Me.ucNotes.Name = "ucNotes"
			Me.ucNotes.Size = New System.Drawing.Size(888, 229)
			Me.ucNotes.TabIndex = 0
			Me.ucNotes.UCMediator = Nothing
			'
			'xtabGuthaben
			'
			Me.xtabGuthaben.Controls.Add(Me.ucCredit)
			Me.xtabGuthaben.Name = "xtabGuthaben"
			Me.xtabGuthaben.Padding = New System.Windows.Forms.Padding(5)
			Me.xtabGuthaben.Size = New System.Drawing.Size(898, 238)
			Me.xtabGuthaben.Text = "Guthaben und Rückstellungen"
			'
			'ucCredit
			'
			Me.ucCredit.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucCredit.IsIntialControlDataLoaded = False
			Me.ucCredit.Location = New System.Drawing.Point(5, 5)
			Me.ucCredit.Name = "ucCredit"
			Me.ucCredit.Padding = New System.Windows.Forms.Padding(5)
			Me.ucCredit.Size = New System.Drawing.Size(888, 228)
			Me.ucCredit.TabIndex = 0
			Me.ucCredit.UCMediator = Nothing
			'
			'xtabLM
			'
			Me.xtabLM.Controls.Add(Me.ucMonthlySalaryData)
			Me.xtabLM.Name = "xtabLM"
			Me.xtabLM.Padding = New System.Windows.Forms.Padding(5)
			Me.xtabLM.Size = New System.Drawing.Size(898, 238)
			Me.xtabLM.Text = "Monatliche Lohnangaben"
			'
			'ucMonthlySalaryData
			'
			Me.ucMonthlySalaryData.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucMonthlySalaryData.IsIntialControlDataLoaded = False
			Me.ucMonthlySalaryData.Location = New System.Drawing.Point(5, 5)
			Me.ucMonthlySalaryData.Name = "ucMonthlySalaryData"
			Me.ucMonthlySalaryData.Size = New System.Drawing.Size(888, 228)
			Me.ucMonthlySalaryData.TabIndex = 0
			Me.ucMonthlySalaryData.UCMediator = Nothing
			'
			'xtabZG
			'
			Me.xtabZG.Controls.Add(Me.ucAdvancePayment)
			Me.xtabZG.Name = "xtabZG"
			Me.xtabZG.Padding = New System.Windows.Forms.Padding(5)
			Me.xtabZG.Size = New System.Drawing.Size(898, 238)
			Me.xtabZG.Text = "Vorschüsse"
			'
			'ucAdvancePayment
			'
			Me.ucAdvancePayment.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucAdvancePayment.IsIntialControlDataLoaded = False
			Me.ucAdvancePayment.Location = New System.Drawing.Point(5, 5)
			Me.ucAdvancePayment.Name = "ucAdvancePayment"
			Me.ucAdvancePayment.Size = New System.Drawing.Size(888, 228)
			Me.ucAdvancePayment.TabIndex = 0
			Me.ucAdvancePayment.UCMediator = Nothing
			'
			'ucTimetableAndInfoData
			'
			Me.ucTimetableAndInfoData.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucTimetableAndInfoData.IsIntialControlDataLoaded = False
			Me.ucTimetableAndInfoData.Location = New System.Drawing.Point(2, 2)
			Me.ucTimetableAndInfoData.Name = "ucTimetableAndInfoData"
			Me.ucTimetableAndInfoData.Padding = New System.Windows.Forms.Padding(5)
			Me.ucTimetableAndInfoData.Size = New System.Drawing.Size(230, 801)
			Me.ucTimetableAndInfoData.TabIndex = 2
			Me.ucTimetableAndInfoData.UCMediator = Nothing
			'
			'pcNavMain
			'
			Me.pcNavMain.Controls.Add(Me.xtabReportOverviewData)
			Me.pcNavMain.Dock = System.Windows.Forms.DockStyle.Left
			Me.pcNavMain.Location = New System.Drawing.Point(5, 5)
			Me.pcNavMain.Name = "pcNavMain"
			Me.pcNavMain.Padding = New System.Windows.Forms.Padding(5)
			Me.pcNavMain.Size = New System.Drawing.Size(361, 805)
			Me.pcNavMain.TabIndex = 176
			'
			'PanelControl1
			'
			Me.PanelControl1.Controls.Add(Me.ucTimetableAndInfoData)
			Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Right
			Me.PanelControl1.Location = New System.Drawing.Point(1266, 5)
			Me.PanelControl1.Name = "PanelControl1"
			Me.PanelControl1.Size = New System.Drawing.Size(234, 805)
			Me.PanelControl1.TabIndex = 177
			'
			'frmReportMng
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(1505, 841)
			Me.Controls.Add(Me.SplitContainerControl4)
			Me.Controls.Add(Me.PanelControl1)
			Me.Controls.Add(Me.pcNavMain)
			Me.Controls.Add(Me.barDockControlLeft)
			Me.Controls.Add(Me.barDockControlRight)
			Me.Controls.Add(Me.barDockControlBottom)
			Me.Controls.Add(Me.barDockControlTop)
			Me.IconOptions.Icon = CType(resources.GetObject("frmReportMng.IconOptions.Icon"), System.Drawing.Icon)
			Me.MaximumSize = New System.Drawing.Size(1521, 879)
			Me.Name = "frmReportMng"
			Me.Padding = New System.Windows.Forms.Padding(5)
			Me.Text = "Rapportverwaltung"
			CType(Me.RepositoryItemComboBox2, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.RepositoryItemComboBox1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.xtabReportOverviewData, System.ComponentModel.ISupportInitialize).EndInit()
			Me.xtabReportOverviewData.ResumeLayout(False)
			Me.xtabEmployeeList.ResumeLayout(False)
			CType(Me.grdReportOverview, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvReportOverview, System.ComponentModel.ISupportInitialize).EndInit()
			Me.xtabEmployeeDetail.ResumeLayout(False)
			CType(Me.SplitContainerControl1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.SplitContainerControl1.ResumeLayout(False)
			CType(Me.navMain, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.RepositoryItemFontEdit1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.ImageCollection1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.SplitContainerControl4, System.ComponentModel.ISupportInitialize).EndInit()
			Me.SplitContainerControl4.ResumeLayout(False)
			CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.XtraTabControl1.ResumeLayout(False)
			Me.xtabBemerkung.ResumeLayout(False)
			Me.xtabGuthaben.ResumeLayout(False)
			Me.xtabLM.ResumeLayout(False)
			Me.xtabZG.ResumeLayout(False)
			CType(Me.pcNavMain, System.ComponentModel.ISupportInitialize).EndInit()
			Me.pcNavMain.ResumeLayout(False)
			CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.PanelControl1.ResumeLayout(False)
			Me.ResumeLayout(False)
			Me.PerformLayout()

		End Sub
		Friend WithEvents RepositoryItemComboBox2 As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
    Friend WithEvents RepositoryItemComboBox1 As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
    Friend WithEvents btnShowTemplate As DevExpress.XtraBars.BarSubItem
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents RepositoryItemFontEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemFontEdit
    Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
    Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
    Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
    Friend WithEvents navMain As DevExpress.XtraNavBar.NavBarControl
    Friend WithEvents ImageCollection1 As DevExpress.Utils.ImageCollection
    Friend WithEvents Bar1 As DevExpress.XtraBars.Bar
    'Friend WithEvents bsiCreated As DevExpress.XtraBars.BarStaticItem
    Friend WithEvents bsiLblGeandert As DevExpress.XtraBars.BarStaticItem
    Friend WithEvents bsiChanged As DevExpress.XtraBars.BarStaticItem
    Friend WithEvents ucTimetableAndInfoData As SP.MA.ReportMng.UI.ucTimetableAndInfoData
    Friend WithEvents ucMainContent As SP.MA.ReportMng.UI.ucMainContent
    Friend WithEvents xtabReportOverviewData As DevExpress.XtraTab.XtraTabControl
    Friend WithEvents xtabEmployeeList As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents xtabEmployeeDetail As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents SplitContainerControl1 As DevExpress.XtraEditors.SplitContainerControl
    Friend WithEvents XtraTabControl1 As DevExpress.XtraTab.XtraTabControl
    Friend WithEvents xtabBemerkung As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents xtabGuthaben As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents xtabZG As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents xtabLM As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents pcNavMain As DevExpress.XtraEditors.PanelControl
    Friend WithEvents SplitContainerControl4 As DevExpress.XtraEditors.SplitContainerControl
    Friend WithEvents grdReportOverview As DevExpress.XtraGrid.GridControl
    Friend WithEvents gvReportOverview As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents chkBtnReportInfo As DevExpress.XtraEditors.CheckButton
    Friend WithEvents chkBtnReportDetails As DevExpress.XtraEditors.CheckButton
    Friend WithEvents ucReportDetailData As SP.MA.ReportMng.UI.ucReportDetailData
    Friend WithEvents ucReportDetailData2 As SP.MA.ReportMng.UI.ucReportDetailData2
    Friend WithEvents ucNotes As SP.MA.ReportMng.UI.ucNotes
    Friend WithEvents ucCredit As SP.MA.ReportMng.UI.ucCredit
    Friend WithEvents ucMonthlySalaryData As SP.MA.ReportMng.UI.ucMonthlySalaryData
    Friend WithEvents ucAdvancePayment As SP.MA.ReportMng.UI.ucAdvancePayment
    Friend WithEvents bsiBtnGavInfo As DevExpress.XtraBars.BarButtonItem
    'Friend WithEvents bsiLblErstellt As DevExpress.XtraBars.BarStaticItem

  End Class
End Namespace
