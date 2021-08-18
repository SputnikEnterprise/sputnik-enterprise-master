Namespace UI

  <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
  Partial Class frmAdvancePayments
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
			Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAdvancePayments))
			Me.pcNavMain = New DevExpress.XtraEditors.PanelControl()
			Me.navMain = New DevExpress.XtraNavBar.NavBarControl()
			Me.ImageList1 = New System.Windows.Forms.ImageList()
			Me.ImageCollection1 = New DevExpress.Utils.ImageCollection()
			Me.pnlContents = New DevExpress.XtraEditors.PanelControl()
			Me.sccAdvancePaymentMain = New DevExpress.XtraEditors.SplitContainerControl()
			Me.XtraScrollableControl2 = New DevExpress.XtraEditors.XtraScrollableControl()
			Me.ucMainContent = New SP.MA.AdvancePaymentMng.UI.ucMainContent()
			Me.bottomTabs = New DevExpress.XtraTab.XtraTabControl()
			Me.xtabNegativeSalary = New DevExpress.XtraTab.XtraTabPage()
			Me.ucNegativeSalaryData = New SP.MA.AdvancePaymentMng.UI.ucNegativeSalaryData()
			Me.UcNegativeSalaryData1 = New SP.MA.AdvancePaymentMng.UI.ucNegativeSalaryData()
			Me.xtabBank = New DevExpress.XtraTab.XtraTabPage()
			Me.ucBankData = New SP.MA.AdvancePaymentMng.UI.ucBankData()
			Me.xtabAdvancePaymentList = New DevExpress.XtraTab.XtraTabPage()
			Me.ucAdvancePaymentslist = New SP.MA.AdvancePaymentMng.UI.ucAdvancePaymentslist()
			Me.BarManager1 = New DevExpress.XtraBars.BarManager()
			Me.Bar1 = New DevExpress.XtraBars.Bar()
			Me.bsiLblErstellt = New DevExpress.XtraBars.BarStaticItem()
			Me.bsiCreated = New DevExpress.XtraBars.BarStaticItem()
			Me.bsiLblGeaendert = New DevExpress.XtraBars.BarStaticItem()
			Me.bsiChanged = New DevExpress.XtraBars.BarStaticItem()
			Me.bsiLblGedruckt = New DevExpress.XtraBars.BarStaticItem()
			Me.bsiPrinted = New DevExpress.XtraBars.BarStaticItem()
			Me.BarDockControl5 = New DevExpress.XtraBars.BarDockControl()
			Me.BarDockControl6 = New DevExpress.XtraBars.BarDockControl()
			Me.BarDockControl7 = New DevExpress.XtraBars.BarDockControl()
			Me.BarDockControl8 = New DevExpress.XtraBars.BarDockControl()
			Me.CutItem1 = New DevExpress.XtraRichEdit.UI.CutItem()
			Me.CopyItem1 = New DevExpress.XtraRichEdit.UI.CopyItem()
			Me.PasteItem1 = New DevExpress.XtraRichEdit.UI.PasteItem()
			Me.PasteSpecialItem1 = New DevExpress.XtraRichEdit.UI.PasteSpecialItem()
			Me.ChangeFontNameItem1 = New DevExpress.XtraRichEdit.UI.ChangeFontNameItem()
			Me.RepositoryItemFontEdit4 = New DevExpress.XtraEditors.Repository.RepositoryItemFontEdit()
			Me.ChangeFontSizeItem1 = New DevExpress.XtraRichEdit.UI.ChangeFontSizeItem()
			Me.RepositoryItemRichEditFontSizeEdit1 = New DevExpress.XtraRichEdit.Design.RepositoryItemRichEditFontSizeEdit()
			Me.ChangeFontColorItem1 = New DevExpress.XtraRichEdit.UI.ChangeFontColorItem()
			Me.ChangeFontBackColorItem1 = New DevExpress.XtraRichEdit.UI.ChangeFontBackColorItem()
			Me.ToggleFontBoldItem1 = New DevExpress.XtraRichEdit.UI.ToggleFontBoldItem()
			Me.ToggleFontItalicItem1 = New DevExpress.XtraRichEdit.UI.ToggleFontItalicItem()
			Me.ToggleFontUnderlineItem1 = New DevExpress.XtraRichEdit.UI.ToggleFontUnderlineItem()
			Me.ToggleFontDoubleUnderlineItem1 = New DevExpress.XtraRichEdit.UI.ToggleFontDoubleUnderlineItem()
			Me.ToggleFontStrikeoutItem1 = New DevExpress.XtraRichEdit.UI.ToggleFontStrikeoutItem()
			Me.ToggleFontDoubleStrikeoutItem1 = New DevExpress.XtraRichEdit.UI.ToggleFontDoubleStrikeoutItem()
			Me.ToggleFontSuperscriptItem1 = New DevExpress.XtraRichEdit.UI.ToggleFontSuperscriptItem()
			Me.ToggleFontSubscriptItem1 = New DevExpress.XtraRichEdit.UI.ToggleFontSubscriptItem()
			Me.ChangeTextCaseItem1 = New DevExpress.XtraRichEdit.UI.ChangeTextCaseItem()
			Me.MakeTextUpperCaseItem1 = New DevExpress.XtraRichEdit.UI.MakeTextUpperCaseItem()
			Me.MakeTextLowerCaseItem1 = New DevExpress.XtraRichEdit.UI.MakeTextLowerCaseItem()
			Me.ToggleTextCaseItem1 = New DevExpress.XtraRichEdit.UI.ToggleTextCaseItem()
			Me.FontSizeIncreaseItem1 = New DevExpress.XtraRichEdit.UI.FontSizeIncreaseItem()
			Me.FontSizeDecreaseItem1 = New DevExpress.XtraRichEdit.UI.FontSizeDecreaseItem()
			Me.ClearFormattingItem1 = New DevExpress.XtraRichEdit.UI.ClearFormattingItem()
			Me.ShowFontFormItem1 = New DevExpress.XtraRichEdit.UI.ShowFontFormItem()
			Me.ToggleParagraphAlignmentLeftItem1 = New DevExpress.XtraRichEdit.UI.ToggleParagraphAlignmentLeftItem()
			Me.ToggleParagraphAlignmentCenterItem1 = New DevExpress.XtraRichEdit.UI.ToggleParagraphAlignmentCenterItem()
			Me.ToggleParagraphAlignmentRightItem1 = New DevExpress.XtraRichEdit.UI.ToggleParagraphAlignmentRightItem()
			Me.ToggleParagraphAlignmentJustifyItem1 = New DevExpress.XtraRichEdit.UI.ToggleParagraphAlignmentJustifyItem()
			Me.ChangeParagraphLineSpacingItem1 = New DevExpress.XtraRichEdit.UI.ChangeParagraphLineSpacingItem()
			Me.SetSingleParagraphSpacingItem1 = New DevExpress.XtraRichEdit.UI.SetSingleParagraphSpacingItem()
			Me.SetSesquialteralParagraphSpacingItem1 = New DevExpress.XtraRichEdit.UI.SetSesquialteralParagraphSpacingItem()
			Me.SetDoubleParagraphSpacingItem1 = New DevExpress.XtraRichEdit.UI.SetDoubleParagraphSpacingItem()
			Me.ShowLineSpacingFormItem1 = New DevExpress.XtraRichEdit.UI.ShowLineSpacingFormItem()
			Me.AddSpacingBeforeParagraphItem1 = New DevExpress.XtraRichEdit.UI.AddSpacingBeforeParagraphItem()
			Me.RemoveSpacingBeforeParagraphItem1 = New DevExpress.XtraRichEdit.UI.RemoveSpacingBeforeParagraphItem()
			Me.AddSpacingAfterParagraphItem1 = New DevExpress.XtraRichEdit.UI.AddSpacingAfterParagraphItem()
			Me.RemoveSpacingAfterParagraphItem1 = New DevExpress.XtraRichEdit.UI.RemoveSpacingAfterParagraphItem()
			Me.ToggleBulletedListItem1 = New DevExpress.XtraRichEdit.UI.ToggleBulletedListItem()
			Me.ToggleNumberingListItem1 = New DevExpress.XtraRichEdit.UI.ToggleNumberingListItem()
			Me.ToggleMultiLevelListItem1 = New DevExpress.XtraRichEdit.UI.ToggleMultiLevelListItem()
			Me.DecreaseIndentItem1 = New DevExpress.XtraRichEdit.UI.DecreaseIndentItem()
			Me.IncreaseIndentItem1 = New DevExpress.XtraRichEdit.UI.IncreaseIndentItem()
			Me.ToggleShowWhitespaceItem1 = New DevExpress.XtraRichEdit.UI.ToggleShowWhitespaceItem()
			Me.ShowParagraphFormItem1 = New DevExpress.XtraRichEdit.UI.ShowParagraphFormItem()
			Me.ChangeStyleItem1 = New DevExpress.XtraRichEdit.UI.ChangeStyleItem()
			Me.rep_cbo_Template = New DevExpress.XtraRichEdit.Design.RepositoryItemRichEditStyleEdit()
			Me.FindItem1 = New DevExpress.XtraRichEdit.UI.FindItem()
			Me.ReplaceItem1 = New DevExpress.XtraRichEdit.UI.ReplaceItem()
			Me.FileSaveItem1 = New DevExpress.XtraRichEdit.UI.FileSaveItem()
			Me.BarSubItem3 = New DevExpress.XtraBars.BarSubItem()
			Me.RepositoryItemComboBox7 = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
			Me.RepositoryItemComboBox8 = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
			Me.errorProvider = New System.Windows.Forms.ErrorProvider()
			CType(Me.pcNavMain, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.pcNavMain.SuspendLayout()
			CType(Me.navMain, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.ImageCollection1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.pnlContents, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.pnlContents.SuspendLayout()
			CType(Me.sccAdvancePaymentMain, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.sccAdvancePaymentMain.SuspendLayout()
			Me.XtraScrollableControl2.SuspendLayout()
			CType(Me.bottomTabs, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.bottomTabs.SuspendLayout()
			Me.xtabNegativeSalary.SuspendLayout()
			Me.xtabBank.SuspendLayout()
			Me.xtabAdvancePaymentList.SuspendLayout()
			CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.RepositoryItemFontEdit4, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.RepositoryItemRichEditFontSizeEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.rep_cbo_Template, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.RepositoryItemComboBox7, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.RepositoryItemComboBox8, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.errorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			'
			'pcNavMain
			'
			Me.pcNavMain.Controls.Add(Me.navMain)
			Me.pcNavMain.Dock = System.Windows.Forms.DockStyle.Left
			Me.pcNavMain.Location = New System.Drawing.Point(5, 5)
			Me.pcNavMain.Name = "pcNavMain"
			Me.pcNavMain.Padding = New System.Windows.Forms.Padding(10)
			Me.pcNavMain.Size = New System.Drawing.Size(196, 724)
			Me.pcNavMain.TabIndex = 165
			'
			'navMain
			'
			Me.navMain.ActiveGroup = Nothing
			Me.navMain.Dock = System.Windows.Forms.DockStyle.Fill
			Me.navMain.Location = New System.Drawing.Point(12, 12)
			Me.navMain.Name = "navMain"
			Me.navMain.OptionsNavPane.ExpandedWidth = 172
			Me.navMain.Size = New System.Drawing.Size(172, 700)
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
			Me.ImageCollection1.InsertGalleryImage("clearformatting_16x16.png", "images/actions/clearformatting_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/clearformatting_16x16.png"), 10)
			Me.ImageCollection1.Images.SetKeyName(10, "clearformatting_16x16.png")
			'
			'pnlContents
			'
			Me.pnlContents.AllowTouchScroll = True
			Me.pnlContents.Controls.Add(Me.sccAdvancePaymentMain)
			Me.pnlContents.Dock = System.Windows.Forms.DockStyle.Fill
			Me.pnlContents.Location = New System.Drawing.Point(201, 5)
			Me.pnlContents.Name = "pnlContents"
			Me.pnlContents.Padding = New System.Windows.Forms.Padding(5)
			Me.pnlContents.Size = New System.Drawing.Size(1100, 724)
			Me.pnlContents.TabIndex = 170
			'
			'sccAdvancePaymentMain
			'
			Me.sccAdvancePaymentMain.Dock = System.Windows.Forms.DockStyle.Fill
			Me.sccAdvancePaymentMain.Horizontal = False
			Me.sccAdvancePaymentMain.Location = New System.Drawing.Point(7, 7)
			Me.sccAdvancePaymentMain.Name = "sccAdvancePaymentMain"
			Me.sccAdvancePaymentMain.Panel1.Controls.Add(Me.XtraScrollableControl2)
			Me.sccAdvancePaymentMain.Panel1.Padding = New System.Windows.Forms.Padding(5)
			Me.sccAdvancePaymentMain.Panel1.Text = "Panel1"
			Me.sccAdvancePaymentMain.Panel2.Controls.Add(Me.bottomTabs)
			Me.sccAdvancePaymentMain.Panel2.Padding = New System.Windows.Forms.Padding(5)
			Me.sccAdvancePaymentMain.Panel2.Text = "Panel2"
			Me.sccAdvancePaymentMain.Size = New System.Drawing.Size(1086, 710)
			Me.sccAdvancePaymentMain.SplitterPosition = 499
			Me.sccAdvancePaymentMain.TabIndex = 289
			Me.sccAdvancePaymentMain.Text = "SplitContainerControl1"
			'
			'XtraScrollableControl2
			'
			Me.XtraScrollableControl2.Controls.Add(Me.ucMainContent)
			Me.XtraScrollableControl2.Dock = System.Windows.Forms.DockStyle.Fill
			Me.XtraScrollableControl2.Location = New System.Drawing.Point(5, 5)
			Me.XtraScrollableControl2.Name = "XtraScrollableControl2"
			Me.XtraScrollableControl2.Size = New System.Drawing.Size(1076, 489)
			Me.XtraScrollableControl2.TabIndex = 0
			'
			'ucMainContent
			'
			Me.ucMainContent.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucMainContent.IsIntialControlDataLoaded = False
			Me.ucMainContent.Location = New System.Drawing.Point(0, 0)
			Me.ucMainContent.Name = "ucMainContent"
			Me.ucMainContent.Padding = New System.Windows.Forms.Padding(10)
			Me.ucMainContent.Size = New System.Drawing.Size(1076, 489)
			Me.ucMainContent.TabIndex = 0
			Me.ucMainContent.UCMediator = Nothing
			'
			'bottomTabs
			'
			Me.bottomTabs.Dock = System.Windows.Forms.DockStyle.Fill
			Me.bottomTabs.Location = New System.Drawing.Point(5, 5)
			Me.bottomTabs.Name = "bottomTabs"
			Me.bottomTabs.SelectedTabPage = Me.xtabNegativeSalary
			Me.bottomTabs.Size = New System.Drawing.Size(1076, 196)
			Me.bottomTabs.TabIndex = 0
			Me.bottomTabs.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabNegativeSalary, Me.xtabBank, Me.xtabAdvancePaymentList})
			'
			'xtabNegativeSalary
			'
			Me.xtabNegativeSalary.Controls.Add(Me.ucNegativeSalaryData)
			Me.xtabNegativeSalary.Controls.Add(Me.UcNegativeSalaryData1)
			Me.xtabNegativeSalary.Name = "xtabNegativeSalary"
			Me.xtabNegativeSalary.Size = New System.Drawing.Size(1070, 168)
			Me.xtabNegativeSalary.Text = "Negative Lohndaten"
			'
			'ucNegativeSalaryData
			'
			Me.ucNegativeSalaryData.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucNegativeSalaryData.IsIntialControlDataLoaded = False
			Me.ucNegativeSalaryData.Location = New System.Drawing.Point(0, 0)
			Me.ucNegativeSalaryData.Name = "ucNegativeSalaryData"
			Me.ucNegativeSalaryData.Size = New System.Drawing.Size(1070, 168)
			Me.ucNegativeSalaryData.TabIndex = 1
			Me.ucNegativeSalaryData.UCMediator = Nothing
			'
			'UcNegativeSalaryData1
			'
			Me.UcNegativeSalaryData1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.UcNegativeSalaryData1.IsIntialControlDataLoaded = False
			Me.UcNegativeSalaryData1.Location = New System.Drawing.Point(0, 0)
			Me.UcNegativeSalaryData1.Name = "UcNegativeSalaryData1"
			Me.UcNegativeSalaryData1.Size = New System.Drawing.Size(1070, 168)
			Me.UcNegativeSalaryData1.TabIndex = 0
			Me.UcNegativeSalaryData1.UCMediator = Nothing
			'
			'xtabBank
			'
			Me.xtabBank.Controls.Add(Me.ucBankData)
			Me.xtabBank.Name = "xtabBank"
			Me.xtabBank.Size = New System.Drawing.Size(1070, 168)
			Me.xtabBank.Text = "Bankdaten"
			'
			'ucBankData
			'
			Me.ucBankData.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucBankData.IsIntialControlDataLoaded = False
			Me.ucBankData.Location = New System.Drawing.Point(0, 0)
			Me.ucBankData.Name = "ucBankData"
			Me.ucBankData.Size = New System.Drawing.Size(1070, 168)
			Me.ucBankData.TabIndex = 0
			Me.ucBankData.UCMediator = Nothing
			'
			'xtabAdvancePaymentList
			'
			Me.xtabAdvancePaymentList.Controls.Add(Me.ucAdvancePaymentslist)
			Me.xtabAdvancePaymentList.Name = "xtabAdvancePaymentList"
			Me.xtabAdvancePaymentList.Size = New System.Drawing.Size(1070, 168)
			Me.xtabAdvancePaymentList.Text = "Vorschussliste"
			'
			'ucAdvancePaymentslist
			'
			Me.ucAdvancePaymentslist.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucAdvancePaymentslist.IsIntialControlDataLoaded = False
			Me.ucAdvancePaymentslist.Location = New System.Drawing.Point(0, 0)
			Me.ucAdvancePaymentslist.Name = "ucAdvancePaymentslist"
			Me.ucAdvancePaymentslist.Size = New System.Drawing.Size(1070, 168)
			Me.ucAdvancePaymentslist.TabIndex = 0
			Me.ucAdvancePaymentslist.UCMediator = Nothing
			'
			'BarManager1
			'
			Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar1})
			Me.BarManager1.DockControls.Add(Me.BarDockControl5)
			Me.BarManager1.DockControls.Add(Me.BarDockControl6)
			Me.BarManager1.DockControls.Add(Me.BarDockControl7)
			Me.BarManager1.DockControls.Add(Me.BarDockControl8)
			Me.BarManager1.Form = Me
			Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.CutItem1, Me.CopyItem1, Me.PasteItem1, Me.PasteSpecialItem1, Me.ChangeFontNameItem1, Me.ChangeFontSizeItem1, Me.ChangeFontColorItem1, Me.ChangeFontBackColorItem1, Me.ToggleFontBoldItem1, Me.ToggleFontItalicItem1, Me.ToggleFontUnderlineItem1, Me.ToggleFontDoubleUnderlineItem1, Me.ToggleFontStrikeoutItem1, Me.ToggleFontDoubleStrikeoutItem1, Me.ToggleFontSuperscriptItem1, Me.ToggleFontSubscriptItem1, Me.ChangeTextCaseItem1, Me.MakeTextUpperCaseItem1, Me.MakeTextLowerCaseItem1, Me.ToggleTextCaseItem1, Me.FontSizeIncreaseItem1, Me.FontSizeDecreaseItem1, Me.ClearFormattingItem1, Me.ShowFontFormItem1, Me.ToggleParagraphAlignmentLeftItem1, Me.ToggleParagraphAlignmentCenterItem1, Me.ToggleParagraphAlignmentRightItem1, Me.ToggleParagraphAlignmentJustifyItem1, Me.ChangeParagraphLineSpacingItem1, Me.SetSingleParagraphSpacingItem1, Me.SetSesquialteralParagraphSpacingItem1, Me.SetDoubleParagraphSpacingItem1, Me.ShowLineSpacingFormItem1, Me.AddSpacingBeforeParagraphItem1, Me.RemoveSpacingBeforeParagraphItem1, Me.AddSpacingAfterParagraphItem1, Me.RemoveSpacingAfterParagraphItem1, Me.ToggleBulletedListItem1, Me.ToggleNumberingListItem1, Me.ToggleMultiLevelListItem1, Me.DecreaseIndentItem1, Me.IncreaseIndentItem1, Me.ToggleShowWhitespaceItem1, Me.ShowParagraphFormItem1, Me.ChangeStyleItem1, Me.FindItem1, Me.ReplaceItem1, Me.FileSaveItem1, Me.BarSubItem3, Me.bsiCreated, Me.bsiChanged, Me.bsiLblErstellt, Me.bsiLblGeaendert, Me.bsiLblGedruckt, Me.bsiPrinted})
			Me.BarManager1.MaxItemId = 68
			Me.BarManager1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemFontEdit4, Me.RepositoryItemRichEditFontSizeEdit1, Me.rep_cbo_Template, Me.RepositoryItemComboBox7, Me.RepositoryItemComboBox8})
			Me.BarManager1.StatusBar = Me.Bar1
			'
			'Bar1
			'
			Me.Bar1.BarName = "Benutzerdefiniert 2"
			Me.Bar1.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
			Me.Bar1.DockCol = 0
			Me.Bar1.DockRow = 0
			Me.Bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
			Me.Bar1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiLblErstellt, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bsiCreated), New DevExpress.XtraBars.LinkPersistInfo(Me.bsiLblGeaendert, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bsiChanged), New DevExpress.XtraBars.LinkPersistInfo(Me.bsiLblGedruckt, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bsiPrinted)})
			Me.Bar1.OptionsBar.AllowQuickCustomization = False
			Me.Bar1.OptionsBar.DrawDragBorder = False
			Me.Bar1.OptionsBar.UseWholeRow = True
			Me.Bar1.Text = "Benutzerdefiniert 2"
			'
			'bsiLblErstellt
			'
			Me.bsiLblErstellt.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
			Me.bsiLblErstellt.Caption = "Erstellt:"
			Me.bsiLblErstellt.Id = 63
			Me.bsiLblErstellt.ItemAppearance.Normal.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.bsiLblErstellt.ItemAppearance.Normal.Options.UseFont = True
			Me.bsiLblErstellt.Name = "bsiLblErstellt"
			'
			'bsiCreated
			'
			Me.bsiCreated.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
			Me.bsiCreated.Caption = "{0}"
			Me.bsiCreated.Id = 60
			Me.bsiCreated.Name = "bsiCreated"
			'
			'bsiLblGeaendert
			'
			Me.bsiLblGeaendert.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
			Me.bsiLblGeaendert.Caption = "Geändert:"
			Me.bsiLblGeaendert.Id = 65
			Me.bsiLblGeaendert.ItemAppearance.Normal.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
			Me.bsiLblGeaendert.ItemAppearance.Normal.Options.UseFont = True
			Me.bsiLblGeaendert.Name = "bsiLblGeaendert"
			'
			'bsiChanged
			'
			Me.bsiChanged.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
			Me.bsiChanged.Caption = "{0}"
			Me.bsiChanged.Id = 61
			Me.bsiChanged.Name = "bsiChanged"
			'
			'bsiLblGedruckt
			'
			Me.bsiLblGedruckt.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
			Me.bsiLblGedruckt.Caption = "Gedruckt:"
			Me.bsiLblGedruckt.Id = 66
			Me.bsiLblGedruckt.ItemAppearance.Normal.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
			Me.bsiLblGedruckt.ItemAppearance.Normal.Options.UseFont = True
			Me.bsiLblGedruckt.Name = "bsiLblGedruckt"
			Me.bsiLblGedruckt.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.Caption
			'
			'bsiPrinted
			'
			Me.bsiPrinted.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
			Me.bsiPrinted.Caption = "{0}"
			Me.bsiPrinted.Id = 67
			Me.bsiPrinted.Name = "bsiPrinted"
			'
			'BarDockControl5
			'
			Me.BarDockControl5.CausesValidation = False
			Me.BarDockControl5.Dock = System.Windows.Forms.DockStyle.Top
			Me.BarDockControl5.Location = New System.Drawing.Point(5, 5)
			Me.BarDockControl5.Manager = Me.BarManager1
			Me.BarDockControl5.Size = New System.Drawing.Size(1296, 0)
			'
			'BarDockControl6
			'
			Me.BarDockControl6.CausesValidation = False
			Me.BarDockControl6.Dock = System.Windows.Forms.DockStyle.Bottom
			Me.BarDockControl6.Location = New System.Drawing.Point(5, 729)
			Me.BarDockControl6.Manager = Me.BarManager1
			Me.BarDockControl6.Size = New System.Drawing.Size(1296, 25)
			'
			'BarDockControl7
			'
			Me.BarDockControl7.CausesValidation = False
			Me.BarDockControl7.Dock = System.Windows.Forms.DockStyle.Left
			Me.BarDockControl7.Location = New System.Drawing.Point(5, 5)
			Me.BarDockControl7.Manager = Me.BarManager1
			Me.BarDockControl7.Size = New System.Drawing.Size(0, 724)
			'
			'BarDockControl8
			'
			Me.BarDockControl8.CausesValidation = False
			Me.BarDockControl8.Dock = System.Windows.Forms.DockStyle.Right
			Me.BarDockControl8.Location = New System.Drawing.Point(1301, 5)
			Me.BarDockControl8.Manager = Me.BarManager1
			Me.BarDockControl8.Size = New System.Drawing.Size(0, 724)
			'
			'CutItem1
			'
			Me.CutItem1.Id = 0
			Me.CutItem1.ImageOptions.Image = CType(resources.GetObject("CutItem1.ImageOptions.Image"), System.Drawing.Image)
			Me.CutItem1.ImageOptions.LargeImage = CType(resources.GetObject("CutItem1.ImageOptions.LargeImage"), System.Drawing.Image)
			Me.CutItem1.Name = "CutItem1"
			'
			'CopyItem1
			'
			Me.CopyItem1.Id = 1
			Me.CopyItem1.ImageOptions.Image = CType(resources.GetObject("CopyItem1.ImageOptions.Image"), System.Drawing.Image)
			Me.CopyItem1.ImageOptions.LargeImage = CType(resources.GetObject("CopyItem1.ImageOptions.LargeImage"), System.Drawing.Image)
			Me.CopyItem1.Name = "CopyItem1"
			'
			'PasteItem1
			'
			Me.PasteItem1.Id = 2
			Me.PasteItem1.ImageOptions.Image = CType(resources.GetObject("PasteItem1.ImageOptions.Image"), System.Drawing.Image)
			Me.PasteItem1.ImageOptions.LargeImage = CType(resources.GetObject("PasteItem1.ImageOptions.LargeImage"), System.Drawing.Image)
			Me.PasteItem1.Name = "PasteItem1"
			'
			'PasteSpecialItem1
			'
			Me.PasteSpecialItem1.Id = 3
			Me.PasteSpecialItem1.ImageOptions.Image = CType(resources.GetObject("PasteSpecialItem1.ImageOptions.Image"), System.Drawing.Image)
			Me.PasteSpecialItem1.ImageOptions.LargeImage = CType(resources.GetObject("PasteSpecialItem1.ImageOptions.LargeImage"), System.Drawing.Image)
			Me.PasteSpecialItem1.Name = "PasteSpecialItem1"
			'
			'ChangeFontNameItem1
			'
			Me.ChangeFontNameItem1.Edit = Me.RepositoryItemFontEdit4
			Me.ChangeFontNameItem1.Id = 4
			Me.ChangeFontNameItem1.Name = "ChangeFontNameItem1"
			'
			'RepositoryItemFontEdit4
			'
			Me.RepositoryItemFontEdit4.AutoHeight = False
			Me.RepositoryItemFontEdit4.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.RepositoryItemFontEdit4.Name = "RepositoryItemFontEdit4"
			'
			'ChangeFontSizeItem1
			'
			Me.ChangeFontSizeItem1.Edit = Me.RepositoryItemRichEditFontSizeEdit1
			Me.ChangeFontSizeItem1.Id = 5
			Me.ChangeFontSizeItem1.Name = "ChangeFontSizeItem1"
			'
			'RepositoryItemRichEditFontSizeEdit1
			'
			Me.RepositoryItemRichEditFontSizeEdit1.AutoHeight = False
			Me.RepositoryItemRichEditFontSizeEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.RepositoryItemRichEditFontSizeEdit1.Control = Nothing
			Me.RepositoryItemRichEditFontSizeEdit1.Name = "RepositoryItemRichEditFontSizeEdit1"
			'
			'ChangeFontColorItem1
			'
			Me.ChangeFontColorItem1.Id = 6
			Me.ChangeFontColorItem1.ImageOptions.Image = CType(resources.GetObject("ChangeFontColorItem1.ImageOptions.Image"), System.Drawing.Image)
			Me.ChangeFontColorItem1.ImageOptions.LargeImage = CType(resources.GetObject("ChangeFontColorItem1.ImageOptions.LargeImage"), System.Drawing.Image)
			Me.ChangeFontColorItem1.Name = "ChangeFontColorItem1"
			'
			'ChangeFontBackColorItem1
			'
			Me.ChangeFontBackColorItem1.Id = 7
			Me.ChangeFontBackColorItem1.ImageOptions.Image = CType(resources.GetObject("ChangeFontBackColorItem1.ImageOptions.Image"), System.Drawing.Image)
			Me.ChangeFontBackColorItem1.ImageOptions.LargeImage = CType(resources.GetObject("ChangeFontBackColorItem1.ImageOptions.LargeImage"), System.Drawing.Image)
			Me.ChangeFontBackColorItem1.Name = "ChangeFontBackColorItem1"
			'
			'ToggleFontBoldItem1
			'
			Me.ToggleFontBoldItem1.Id = 8
			Me.ToggleFontBoldItem1.ImageOptions.Image = CType(resources.GetObject("ToggleFontBoldItem1.ImageOptions.Image"), System.Drawing.Image)
			Me.ToggleFontBoldItem1.ImageOptions.LargeImage = CType(resources.GetObject("ToggleFontBoldItem1.ImageOptions.LargeImage"), System.Drawing.Image)
			Me.ToggleFontBoldItem1.Name = "ToggleFontBoldItem1"
			'
			'ToggleFontItalicItem1
			'
			Me.ToggleFontItalicItem1.Id = 9
			Me.ToggleFontItalicItem1.ImageOptions.Image = CType(resources.GetObject("ToggleFontItalicItem1.ImageOptions.Image"), System.Drawing.Image)
			Me.ToggleFontItalicItem1.ImageOptions.LargeImage = CType(resources.GetObject("ToggleFontItalicItem1.ImageOptions.LargeImage"), System.Drawing.Image)
			Me.ToggleFontItalicItem1.Name = "ToggleFontItalicItem1"
			'
			'ToggleFontUnderlineItem1
			'
			Me.ToggleFontUnderlineItem1.Id = 10
			Me.ToggleFontUnderlineItem1.ImageOptions.Image = CType(resources.GetObject("ToggleFontUnderlineItem1.ImageOptions.Image"), System.Drawing.Image)
			Me.ToggleFontUnderlineItem1.ImageOptions.LargeImage = CType(resources.GetObject("ToggleFontUnderlineItem1.ImageOptions.LargeImage"), System.Drawing.Image)
			Me.ToggleFontUnderlineItem1.Name = "ToggleFontUnderlineItem1"
			'
			'ToggleFontDoubleUnderlineItem1
			'
			Me.ToggleFontDoubleUnderlineItem1.Id = 11
			Me.ToggleFontDoubleUnderlineItem1.ImageOptions.Image = CType(resources.GetObject("ToggleFontDoubleUnderlineItem1.ImageOptions.Image"), System.Drawing.Image)
			Me.ToggleFontDoubleUnderlineItem1.ImageOptions.LargeImage = CType(resources.GetObject("ToggleFontDoubleUnderlineItem1.ImageOptions.LargeImage"), System.Drawing.Image)
			Me.ToggleFontDoubleUnderlineItem1.Name = "ToggleFontDoubleUnderlineItem1"
			'
			'ToggleFontStrikeoutItem1
			'
			Me.ToggleFontStrikeoutItem1.Id = 12
			Me.ToggleFontStrikeoutItem1.ImageOptions.Image = CType(resources.GetObject("ToggleFontStrikeoutItem1.ImageOptions.Image"), System.Drawing.Image)
			Me.ToggleFontStrikeoutItem1.ImageOptions.LargeImage = CType(resources.GetObject("ToggleFontStrikeoutItem1.ImageOptions.LargeImage"), System.Drawing.Image)
			Me.ToggleFontStrikeoutItem1.Name = "ToggleFontStrikeoutItem1"
			'
			'ToggleFontDoubleStrikeoutItem1
			'
			Me.ToggleFontDoubleStrikeoutItem1.Id = 13
			Me.ToggleFontDoubleStrikeoutItem1.ImageOptions.Image = CType(resources.GetObject("ToggleFontDoubleStrikeoutItem1.ImageOptions.Image"), System.Drawing.Image)
			Me.ToggleFontDoubleStrikeoutItem1.ImageOptions.LargeImage = CType(resources.GetObject("ToggleFontDoubleStrikeoutItem1.ImageOptions.LargeImage"), System.Drawing.Image)
			Me.ToggleFontDoubleStrikeoutItem1.Name = "ToggleFontDoubleStrikeoutItem1"
			'
			'ToggleFontSuperscriptItem1
			'
			Me.ToggleFontSuperscriptItem1.Id = 14
			Me.ToggleFontSuperscriptItem1.ImageOptions.Image = CType(resources.GetObject("ToggleFontSuperscriptItem1.ImageOptions.Image"), System.Drawing.Image)
			Me.ToggleFontSuperscriptItem1.ImageOptions.LargeImage = CType(resources.GetObject("ToggleFontSuperscriptItem1.ImageOptions.LargeImage"), System.Drawing.Image)
			Me.ToggleFontSuperscriptItem1.Name = "ToggleFontSuperscriptItem1"
			'
			'ToggleFontSubscriptItem1
			'
			Me.ToggleFontSubscriptItem1.Id = 15
			Me.ToggleFontSubscriptItem1.ImageOptions.Image = CType(resources.GetObject("ToggleFontSubscriptItem1.ImageOptions.Image"), System.Drawing.Image)
			Me.ToggleFontSubscriptItem1.ImageOptions.LargeImage = CType(resources.GetObject("ToggleFontSubscriptItem1.ImageOptions.LargeImage"), System.Drawing.Image)
			Me.ToggleFontSubscriptItem1.Name = "ToggleFontSubscriptItem1"
			'
			'ChangeTextCaseItem1
			'
			Me.ChangeTextCaseItem1.Id = 16
			Me.ChangeTextCaseItem1.ImageOptions.Image = CType(resources.GetObject("ChangeTextCaseItem1.ImageOptions.Image"), System.Drawing.Image)
			Me.ChangeTextCaseItem1.ImageOptions.LargeImage = CType(resources.GetObject("ChangeTextCaseItem1.ImageOptions.LargeImage"), System.Drawing.Image)
			Me.ChangeTextCaseItem1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.MakeTextUpperCaseItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.MakeTextLowerCaseItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.ToggleTextCaseItem1)})
			Me.ChangeTextCaseItem1.Name = "ChangeTextCaseItem1"
			'
			'MakeTextUpperCaseItem1
			'
			Me.MakeTextUpperCaseItem1.Id = 17
			Me.MakeTextUpperCaseItem1.Name = "MakeTextUpperCaseItem1"
			'
			'MakeTextLowerCaseItem1
			'
			Me.MakeTextLowerCaseItem1.Id = 18
			Me.MakeTextLowerCaseItem1.Name = "MakeTextLowerCaseItem1"
			'
			'ToggleTextCaseItem1
			'
			Me.ToggleTextCaseItem1.Id = 19
			Me.ToggleTextCaseItem1.Name = "ToggleTextCaseItem1"
			'
			'FontSizeIncreaseItem1
			'
			Me.FontSizeIncreaseItem1.Id = 20
			Me.FontSizeIncreaseItem1.ImageOptions.Image = CType(resources.GetObject("FontSizeIncreaseItem1.ImageOptions.Image"), System.Drawing.Image)
			Me.FontSizeIncreaseItem1.ImageOptions.LargeImage = CType(resources.GetObject("FontSizeIncreaseItem1.ImageOptions.LargeImage"), System.Drawing.Image)
			Me.FontSizeIncreaseItem1.Name = "FontSizeIncreaseItem1"
			'
			'FontSizeDecreaseItem1
			'
			Me.FontSizeDecreaseItem1.Id = 21
			Me.FontSizeDecreaseItem1.ImageOptions.Image = CType(resources.GetObject("FontSizeDecreaseItem1.ImageOptions.Image"), System.Drawing.Image)
			Me.FontSizeDecreaseItem1.ImageOptions.LargeImage = CType(resources.GetObject("FontSizeDecreaseItem1.ImageOptions.LargeImage"), System.Drawing.Image)
			Me.FontSizeDecreaseItem1.Name = "FontSizeDecreaseItem1"
			'
			'ClearFormattingItem1
			'
			Me.ClearFormattingItem1.Id = 22
			Me.ClearFormattingItem1.ImageOptions.Image = CType(resources.GetObject("ClearFormattingItem1.ImageOptions.Image"), System.Drawing.Image)
			Me.ClearFormattingItem1.ImageOptions.LargeImage = CType(resources.GetObject("ClearFormattingItem1.ImageOptions.LargeImage"), System.Drawing.Image)
			Me.ClearFormattingItem1.Name = "ClearFormattingItem1"
			'
			'ShowFontFormItem1
			'
			Me.ShowFontFormItem1.Id = 23
			Me.ShowFontFormItem1.ImageOptions.Image = CType(resources.GetObject("ShowFontFormItem1.ImageOptions.Image"), System.Drawing.Image)
			Me.ShowFontFormItem1.ImageOptions.LargeImage = CType(resources.GetObject("ShowFontFormItem1.ImageOptions.LargeImage"), System.Drawing.Image)
			Me.ShowFontFormItem1.Name = "ShowFontFormItem1"
			'
			'ToggleParagraphAlignmentLeftItem1
			'
			Me.ToggleParagraphAlignmentLeftItem1.Id = 24
			Me.ToggleParagraphAlignmentLeftItem1.ImageOptions.Image = CType(resources.GetObject("ToggleParagraphAlignmentLeftItem1.ImageOptions.Image"), System.Drawing.Image)
			Me.ToggleParagraphAlignmentLeftItem1.ImageOptions.LargeImage = CType(resources.GetObject("ToggleParagraphAlignmentLeftItem1.ImageOptions.LargeImage"), System.Drawing.Image)
			Me.ToggleParagraphAlignmentLeftItem1.Name = "ToggleParagraphAlignmentLeftItem1"
			'
			'ToggleParagraphAlignmentCenterItem1
			'
			Me.ToggleParagraphAlignmentCenterItem1.Id = 25
			Me.ToggleParagraphAlignmentCenterItem1.ImageOptions.Image = CType(resources.GetObject("ToggleParagraphAlignmentCenterItem1.ImageOptions.Image"), System.Drawing.Image)
			Me.ToggleParagraphAlignmentCenterItem1.ImageOptions.LargeImage = CType(resources.GetObject("ToggleParagraphAlignmentCenterItem1.ImageOptions.LargeImage"), System.Drawing.Image)
			Me.ToggleParagraphAlignmentCenterItem1.Name = "ToggleParagraphAlignmentCenterItem1"
			'
			'ToggleParagraphAlignmentRightItem1
			'
			Me.ToggleParagraphAlignmentRightItem1.Id = 26
			Me.ToggleParagraphAlignmentRightItem1.ImageOptions.Image = CType(resources.GetObject("ToggleParagraphAlignmentRightItem1.ImageOptions.Image"), System.Drawing.Image)
			Me.ToggleParagraphAlignmentRightItem1.ImageOptions.LargeImage = CType(resources.GetObject("ToggleParagraphAlignmentRightItem1.ImageOptions.LargeImage"), System.Drawing.Image)
			Me.ToggleParagraphAlignmentRightItem1.Name = "ToggleParagraphAlignmentRightItem1"
			'
			'ToggleParagraphAlignmentJustifyItem1
			'
			Me.ToggleParagraphAlignmentJustifyItem1.Id = 27
			Me.ToggleParagraphAlignmentJustifyItem1.ImageOptions.Image = CType(resources.GetObject("ToggleParagraphAlignmentJustifyItem1.ImageOptions.Image"), System.Drawing.Image)
			Me.ToggleParagraphAlignmentJustifyItem1.ImageOptions.LargeImage = CType(resources.GetObject("ToggleParagraphAlignmentJustifyItem1.ImageOptions.LargeImage"), System.Drawing.Image)
			Me.ToggleParagraphAlignmentJustifyItem1.Name = "ToggleParagraphAlignmentJustifyItem1"
			'
			'ChangeParagraphLineSpacingItem1
			'
			Me.ChangeParagraphLineSpacingItem1.Id = 28
			Me.ChangeParagraphLineSpacingItem1.ImageOptions.Image = CType(resources.GetObject("ChangeParagraphLineSpacingItem1.ImageOptions.Image"), System.Drawing.Image)
			Me.ChangeParagraphLineSpacingItem1.ImageOptions.LargeImage = CType(resources.GetObject("ChangeParagraphLineSpacingItem1.ImageOptions.LargeImage"), System.Drawing.Image)
			Me.ChangeParagraphLineSpacingItem1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.SetSingleParagraphSpacingItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.SetSesquialteralParagraphSpacingItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.SetDoubleParagraphSpacingItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.ShowLineSpacingFormItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.AddSpacingBeforeParagraphItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.RemoveSpacingBeforeParagraphItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.AddSpacingAfterParagraphItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.RemoveSpacingAfterParagraphItem1)})
			Me.ChangeParagraphLineSpacingItem1.Name = "ChangeParagraphLineSpacingItem1"
			'
			'SetSingleParagraphSpacingItem1
			'
			Me.SetSingleParagraphSpacingItem1.Id = 29
			Me.SetSingleParagraphSpacingItem1.Name = "SetSingleParagraphSpacingItem1"
			'
			'SetSesquialteralParagraphSpacingItem1
			'
			Me.SetSesquialteralParagraphSpacingItem1.Id = 30
			Me.SetSesquialteralParagraphSpacingItem1.Name = "SetSesquialteralParagraphSpacingItem1"
			'
			'SetDoubleParagraphSpacingItem1
			'
			Me.SetDoubleParagraphSpacingItem1.Id = 31
			Me.SetDoubleParagraphSpacingItem1.Name = "SetDoubleParagraphSpacingItem1"
			'
			'ShowLineSpacingFormItem1
			'
			Me.ShowLineSpacingFormItem1.Id = 32
			Me.ShowLineSpacingFormItem1.Name = "ShowLineSpacingFormItem1"
			'
			'AddSpacingBeforeParagraphItem1
			'
			Me.AddSpacingBeforeParagraphItem1.Id = 33
			Me.AddSpacingBeforeParagraphItem1.Name = "AddSpacingBeforeParagraphItem1"
			'
			'RemoveSpacingBeforeParagraphItem1
			'
			Me.RemoveSpacingBeforeParagraphItem1.Id = 34
			Me.RemoveSpacingBeforeParagraphItem1.Name = "RemoveSpacingBeforeParagraphItem1"
			'
			'AddSpacingAfterParagraphItem1
			'
			Me.AddSpacingAfterParagraphItem1.Id = 35
			Me.AddSpacingAfterParagraphItem1.Name = "AddSpacingAfterParagraphItem1"
			'
			'RemoveSpacingAfterParagraphItem1
			'
			Me.RemoveSpacingAfterParagraphItem1.Id = 36
			Me.RemoveSpacingAfterParagraphItem1.Name = "RemoveSpacingAfterParagraphItem1"
			'
			'ToggleBulletedListItem1
			'
			Me.ToggleBulletedListItem1.Id = 37
			Me.ToggleBulletedListItem1.ImageOptions.Image = CType(resources.GetObject("ToggleBulletedListItem1.ImageOptions.Image"), System.Drawing.Image)
			Me.ToggleBulletedListItem1.ImageOptions.LargeImage = CType(resources.GetObject("ToggleBulletedListItem1.ImageOptions.LargeImage"), System.Drawing.Image)
			Me.ToggleBulletedListItem1.Name = "ToggleBulletedListItem1"
			'
			'ToggleNumberingListItem1
			'
			Me.ToggleNumberingListItem1.Id = 38
			Me.ToggleNumberingListItem1.ImageOptions.Image = CType(resources.GetObject("ToggleNumberingListItem1.ImageOptions.Image"), System.Drawing.Image)
			Me.ToggleNumberingListItem1.ImageOptions.LargeImage = CType(resources.GetObject("ToggleNumberingListItem1.ImageOptions.LargeImage"), System.Drawing.Image)
			Me.ToggleNumberingListItem1.Name = "ToggleNumberingListItem1"
			'
			'ToggleMultiLevelListItem1
			'
			Me.ToggleMultiLevelListItem1.Id = 39
			Me.ToggleMultiLevelListItem1.ImageOptions.Image = CType(resources.GetObject("ToggleMultiLevelListItem1.ImageOptions.Image"), System.Drawing.Image)
			Me.ToggleMultiLevelListItem1.ImageOptions.LargeImage = CType(resources.GetObject("ToggleMultiLevelListItem1.ImageOptions.LargeImage"), System.Drawing.Image)
			Me.ToggleMultiLevelListItem1.Name = "ToggleMultiLevelListItem1"
			'
			'DecreaseIndentItem1
			'
			Me.DecreaseIndentItem1.Id = 40
			Me.DecreaseIndentItem1.ImageOptions.Image = CType(resources.GetObject("DecreaseIndentItem1.ImageOptions.Image"), System.Drawing.Image)
			Me.DecreaseIndentItem1.ImageOptions.LargeImage = CType(resources.GetObject("DecreaseIndentItem1.ImageOptions.LargeImage"), System.Drawing.Image)
			Me.DecreaseIndentItem1.Name = "DecreaseIndentItem1"
			'
			'IncreaseIndentItem1
			'
			Me.IncreaseIndentItem1.Id = 41
			Me.IncreaseIndentItem1.ImageOptions.Image = CType(resources.GetObject("IncreaseIndentItem1.ImageOptions.Image"), System.Drawing.Image)
			Me.IncreaseIndentItem1.ImageOptions.LargeImage = CType(resources.GetObject("IncreaseIndentItem1.ImageOptions.LargeImage"), System.Drawing.Image)
			Me.IncreaseIndentItem1.Name = "IncreaseIndentItem1"
			'
			'ToggleShowWhitespaceItem1
			'
			Me.ToggleShowWhitespaceItem1.Id = 42
			Me.ToggleShowWhitespaceItem1.ImageOptions.Image = CType(resources.GetObject("ToggleShowWhitespaceItem1.ImageOptions.Image"), System.Drawing.Image)
			Me.ToggleShowWhitespaceItem1.ImageOptions.LargeImage = CType(resources.GetObject("ToggleShowWhitespaceItem1.ImageOptions.LargeImage"), System.Drawing.Image)
			Me.ToggleShowWhitespaceItem1.Name = "ToggleShowWhitespaceItem1"
			'
			'ShowParagraphFormItem1
			'
			Me.ShowParagraphFormItem1.Id = 43
			Me.ShowParagraphFormItem1.ImageOptions.Image = CType(resources.GetObject("ShowParagraphFormItem1.ImageOptions.Image"), System.Drawing.Image)
			Me.ShowParagraphFormItem1.ImageOptions.LargeImage = CType(resources.GetObject("ShowParagraphFormItem1.ImageOptions.LargeImage"), System.Drawing.Image)
			Me.ShowParagraphFormItem1.Name = "ShowParagraphFormItem1"
			'
			'ChangeStyleItem1
			'
			Me.ChangeStyleItem1.Edit = Me.rep_cbo_Template
			Me.ChangeStyleItem1.Id = 44
			Me.ChangeStyleItem1.Name = "ChangeStyleItem1"
			'
			'rep_cbo_Template
			'
			Me.rep_cbo_Template.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.rep_cbo_Template.Control = Nothing
			Me.rep_cbo_Template.Name = "rep_cbo_Template"
			'
			'FindItem1
			'
			Me.FindItem1.Id = 45
			Me.FindItem1.ImageOptions.Image = CType(resources.GetObject("FindItem1.ImageOptions.Image"), System.Drawing.Image)
			Me.FindItem1.ImageOptions.LargeImage = CType(resources.GetObject("FindItem1.ImageOptions.LargeImage"), System.Drawing.Image)
			Me.FindItem1.Name = "FindItem1"
			'
			'ReplaceItem1
			'
			Me.ReplaceItem1.Id = 46
			Me.ReplaceItem1.ImageOptions.Image = CType(resources.GetObject("ReplaceItem1.ImageOptions.Image"), System.Drawing.Image)
			Me.ReplaceItem1.ImageOptions.LargeImage = CType(resources.GetObject("ReplaceItem1.ImageOptions.LargeImage"), System.Drawing.Image)
			Me.ReplaceItem1.Name = "ReplaceItem1"
			'
			'FileSaveItem1
			'
			Me.FileSaveItem1.Id = 49
			Me.FileSaveItem1.ImageOptions.Image = CType(resources.GetObject("FileSaveItem1.ImageOptions.Image"), System.Drawing.Image)
			Me.FileSaveItem1.ImageOptions.LargeImage = CType(resources.GetObject("FileSaveItem1.ImageOptions.LargeImage"), System.Drawing.Image)
			Me.FileSaveItem1.Name = "FileSaveItem1"
			'
			'BarSubItem3
			'
			Me.BarSubItem3.Caption = "Vorlage laden"
			Me.BarSubItem3.Id = 59
			Me.BarSubItem3.Name = "BarSubItem3"
			Me.BarSubItem3.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
			Me.BarSubItem3.ShowMenuCaption = True
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
			'errorProvider
			'
			Me.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
			Me.errorProvider.ContainerControl = Me
			'
			'frmAdvancePayments
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(1306, 759)
			Me.Controls.Add(Me.pnlContents)
			Me.Controls.Add(Me.pcNavMain)
			Me.Controls.Add(Me.BarDockControl7)
			Me.Controls.Add(Me.BarDockControl8)
			Me.Controls.Add(Me.BarDockControl6)
			Me.Controls.Add(Me.BarDockControl5)
			Me.Name = "frmAdvancePayments"
			Me.Padding = New System.Windows.Forms.Padding(5)
			Me.Text = "Vorschussverwaltung"
			CType(Me.pcNavMain, System.ComponentModel.ISupportInitialize).EndInit()
			Me.pcNavMain.ResumeLayout(False)
			CType(Me.navMain, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.ImageCollection1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.pnlContents, System.ComponentModel.ISupportInitialize).EndInit()
			Me.pnlContents.ResumeLayout(False)
			CType(Me.sccAdvancePaymentMain, System.ComponentModel.ISupportInitialize).EndInit()
			Me.sccAdvancePaymentMain.ResumeLayout(False)
			Me.XtraScrollableControl2.ResumeLayout(False)
			CType(Me.bottomTabs, System.ComponentModel.ISupportInitialize).EndInit()
			Me.bottomTabs.ResumeLayout(False)
			Me.xtabNegativeSalary.ResumeLayout(False)
			Me.xtabBank.ResumeLayout(False)
			Me.xtabAdvancePaymentList.ResumeLayout(False)
			CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.RepositoryItemFontEdit4, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.RepositoryItemRichEditFontSizeEdit1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.rep_cbo_Template, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.RepositoryItemComboBox7, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.RepositoryItemComboBox8, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.errorProvider, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)
			Me.PerformLayout()

		End Sub
		Friend WithEvents pcNavMain As DevExpress.XtraEditors.PanelControl
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents navMain As DevExpress.XtraNavBar.NavBarControl
    Friend WithEvents ImageCollection1 As DevExpress.Utils.ImageCollection
    Friend WithEvents pnlContents As DevExpress.XtraEditors.PanelControl
    Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
    Friend WithEvents Bar1 As DevExpress.XtraBars.Bar
    Friend WithEvents bsiLblErstellt As DevExpress.XtraBars.BarStaticItem
    Friend WithEvents bsiCreated As DevExpress.XtraBars.BarStaticItem
		'Friend WithEvents bsiLblGeaendert_OLD As DevExpress.XtraBars.BarStaticItem
		Friend WithEvents bsiChanged As DevExpress.XtraBars.BarStaticItem
		Friend WithEvents BarDockControl5 As DevExpress.XtraBars.BarDockControl
    Friend WithEvents BarDockControl6 As DevExpress.XtraBars.BarDockControl
    Friend WithEvents BarDockControl7 As DevExpress.XtraBars.BarDockControl
    Friend WithEvents BarDockControl8 As DevExpress.XtraBars.BarDockControl
    Friend WithEvents CutItem1 As DevExpress.XtraRichEdit.UI.CutItem
    Friend WithEvents CopyItem1 As DevExpress.XtraRichEdit.UI.CopyItem
    Friend WithEvents PasteItem1 As DevExpress.XtraRichEdit.UI.PasteItem
    Friend WithEvents PasteSpecialItem1 As DevExpress.XtraRichEdit.UI.PasteSpecialItem
    Friend WithEvents ChangeFontNameItem1 As DevExpress.XtraRichEdit.UI.ChangeFontNameItem
    Friend WithEvents RepositoryItemFontEdit4 As DevExpress.XtraEditors.Repository.RepositoryItemFontEdit
    Friend WithEvents ChangeFontSizeItem1 As DevExpress.XtraRichEdit.UI.ChangeFontSizeItem
    Friend WithEvents RepositoryItemRichEditFontSizeEdit1 As DevExpress.XtraRichEdit.Design.RepositoryItemRichEditFontSizeEdit
    Friend WithEvents ChangeFontColorItem1 As DevExpress.XtraRichEdit.UI.ChangeFontColorItem
    Friend WithEvents ChangeFontBackColorItem1 As DevExpress.XtraRichEdit.UI.ChangeFontBackColorItem
    Friend WithEvents ToggleFontBoldItem1 As DevExpress.XtraRichEdit.UI.ToggleFontBoldItem
    Friend WithEvents ToggleFontItalicItem1 As DevExpress.XtraRichEdit.UI.ToggleFontItalicItem
    Friend WithEvents ToggleFontUnderlineItem1 As DevExpress.XtraRichEdit.UI.ToggleFontUnderlineItem
    Friend WithEvents ToggleFontDoubleUnderlineItem1 As DevExpress.XtraRichEdit.UI.ToggleFontDoubleUnderlineItem
    Friend WithEvents ToggleFontStrikeoutItem1 As DevExpress.XtraRichEdit.UI.ToggleFontStrikeoutItem
    Friend WithEvents ToggleFontDoubleStrikeoutItem1 As DevExpress.XtraRichEdit.UI.ToggleFontDoubleStrikeoutItem
    Friend WithEvents ToggleFontSuperscriptItem1 As DevExpress.XtraRichEdit.UI.ToggleFontSuperscriptItem
    Friend WithEvents ToggleFontSubscriptItem1 As DevExpress.XtraRichEdit.UI.ToggleFontSubscriptItem
    Friend WithEvents ChangeTextCaseItem1 As DevExpress.XtraRichEdit.UI.ChangeTextCaseItem
    Friend WithEvents MakeTextUpperCaseItem1 As DevExpress.XtraRichEdit.UI.MakeTextUpperCaseItem
    Friend WithEvents MakeTextLowerCaseItem1 As DevExpress.XtraRichEdit.UI.MakeTextLowerCaseItem
    Friend WithEvents ToggleTextCaseItem1 As DevExpress.XtraRichEdit.UI.ToggleTextCaseItem
    Friend WithEvents FontSizeIncreaseItem1 As DevExpress.XtraRichEdit.UI.FontSizeIncreaseItem
    Friend WithEvents FontSizeDecreaseItem1 As DevExpress.XtraRichEdit.UI.FontSizeDecreaseItem
    Friend WithEvents ClearFormattingItem1 As DevExpress.XtraRichEdit.UI.ClearFormattingItem
    Friend WithEvents ShowFontFormItem1 As DevExpress.XtraRichEdit.UI.ShowFontFormItem
    Friend WithEvents ToggleParagraphAlignmentLeftItem1 As DevExpress.XtraRichEdit.UI.ToggleParagraphAlignmentLeftItem
    Friend WithEvents ToggleParagraphAlignmentCenterItem1 As DevExpress.XtraRichEdit.UI.ToggleParagraphAlignmentCenterItem
    Friend WithEvents ToggleParagraphAlignmentRightItem1 As DevExpress.XtraRichEdit.UI.ToggleParagraphAlignmentRightItem
    Friend WithEvents ToggleParagraphAlignmentJustifyItem1 As DevExpress.XtraRichEdit.UI.ToggleParagraphAlignmentJustifyItem
    Friend WithEvents ChangeParagraphLineSpacingItem1 As DevExpress.XtraRichEdit.UI.ChangeParagraphLineSpacingItem
    Friend WithEvents SetSingleParagraphSpacingItem1 As DevExpress.XtraRichEdit.UI.SetSingleParagraphSpacingItem
    Friend WithEvents SetSesquialteralParagraphSpacingItem1 As DevExpress.XtraRichEdit.UI.SetSesquialteralParagraphSpacingItem
    Friend WithEvents SetDoubleParagraphSpacingItem1 As DevExpress.XtraRichEdit.UI.SetDoubleParagraphSpacingItem
    Friend WithEvents ShowLineSpacingFormItem1 As DevExpress.XtraRichEdit.UI.ShowLineSpacingFormItem
    Friend WithEvents AddSpacingBeforeParagraphItem1 As DevExpress.XtraRichEdit.UI.AddSpacingBeforeParagraphItem
    Friend WithEvents RemoveSpacingBeforeParagraphItem1 As DevExpress.XtraRichEdit.UI.RemoveSpacingBeforeParagraphItem
    Friend WithEvents AddSpacingAfterParagraphItem1 As DevExpress.XtraRichEdit.UI.AddSpacingAfterParagraphItem
    Friend WithEvents RemoveSpacingAfterParagraphItem1 As DevExpress.XtraRichEdit.UI.RemoveSpacingAfterParagraphItem
    Friend WithEvents ToggleBulletedListItem1 As DevExpress.XtraRichEdit.UI.ToggleBulletedListItem
    Friend WithEvents ToggleNumberingListItem1 As DevExpress.XtraRichEdit.UI.ToggleNumberingListItem
    Friend WithEvents ToggleMultiLevelListItem1 As DevExpress.XtraRichEdit.UI.ToggleMultiLevelListItem
    Friend WithEvents DecreaseIndentItem1 As DevExpress.XtraRichEdit.UI.DecreaseIndentItem
    Friend WithEvents IncreaseIndentItem1 As DevExpress.XtraRichEdit.UI.IncreaseIndentItem
    Friend WithEvents ToggleShowWhitespaceItem1 As DevExpress.XtraRichEdit.UI.ToggleShowWhitespaceItem
    Friend WithEvents ShowParagraphFormItem1 As DevExpress.XtraRichEdit.UI.ShowParagraphFormItem
    Friend WithEvents ChangeStyleItem1 As DevExpress.XtraRichEdit.UI.ChangeStyleItem
    Friend WithEvents rep_cbo_Template As DevExpress.XtraRichEdit.Design.RepositoryItemRichEditStyleEdit
    Friend WithEvents FindItem1 As DevExpress.XtraRichEdit.UI.FindItem
    Friend WithEvents ReplaceItem1 As DevExpress.XtraRichEdit.UI.ReplaceItem
    Friend WithEvents FileSaveItem1 As DevExpress.XtraRichEdit.UI.FileSaveItem
    Friend WithEvents BarSubItem3 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents RepositoryItemComboBox7 As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
    Friend WithEvents RepositoryItemComboBox8 As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
    Friend WithEvents errorProvider As System.Windows.Forms.ErrorProvider
    Friend WithEvents sccAdvancePaymentMain As DevExpress.XtraEditors.SplitContainerControl
    Friend WithEvents XtraScrollableControl2 As DevExpress.XtraEditors.XtraScrollableControl
    Friend WithEvents bottomTabs As DevExpress.XtraTab.XtraTabControl
    Friend WithEvents xtabNegativeSalary As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents xtabBank As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents xtabAdvancePaymentList As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents ucMainContent As SP.MA.AdvancePaymentMng.UI.ucMainContent
    Friend WithEvents UcNegativeSalaryData1 As SP.MA.AdvancePaymentMng.UI.ucNegativeSalaryData
    Friend WithEvents ucNegativeSalaryData As SP.MA.AdvancePaymentMng.UI.ucNegativeSalaryData
    Friend WithEvents ucBankData As SP.MA.AdvancePaymentMng.UI.ucBankData
		Friend WithEvents ucAdvancePaymentslist As SP.MA.AdvancePaymentMng.UI.ucAdvancePaymentslist
		Friend WithEvents bsiLblGeaendert As DevExpress.XtraBars.BarStaticItem
		Friend WithEvents bsiLblGedruckt As DevExpress.XtraBars.BarStaticItem
		Friend WithEvents bsiPrinted As DevExpress.XtraBars.BarStaticItem
  End Class
End Namespace
