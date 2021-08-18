Namespace UI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmCustomers
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
			Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCustomers))
			Me.XtraTabControl1 = New DevExpress.XtraTab.XtraTabControl()
			Me.xtabAllgemein = New DevExpress.XtraTab.XtraTabPage()
			Me.ucCommonData = New SP.KD.CustomerMng.UI.ucCommonData()
			Me.xtabVermittlung = New DevExpress.XtraTab.XtraTabPage()
			Me.ucMediationAndRental = New SP.KD.CustomerMng.UI.ucMediationAndRentalData()
			Me.xtabContact = New DevExpress.XtraTab.XtraTabPage()
			Me.ucContactData = New SP.KD.CustomerMng.UI.ucContactData()
			Me.xtabBilling = New DevExpress.XtraTab.XtraTabPage()
			Me.ucAccountAndSales = New SP.KD.CustomerMng.UI.ucAccountAndSales()
			Me.xtabOthers = New DevExpress.XtraTab.XtraTabPage()
			Me.ucAdditionalInfo = New SP.KD.CustomerMng.UI.ucAdditionalInfo()
			Me.xtabDocMng = New DevExpress.XtraTab.XtraTabPage()
			Me.ucDocumentManagement = New SP.KD.CustomerMng.UI.ucDocumentManagement()
			Me.pcNavMain = New DevExpress.XtraEditors.PanelControl()
			Me.navMain = New DevExpress.XtraNavBar.NavBarControl()
			Me.libVakAls = New DevExpress.XtraEditors.LabelControl()
			Me.sccMain = New DevExpress.XtraEditors.SplitContainerControl()
			Me.grpzustaendigepersonen = New DevExpress.XtraEditors.GroupControl()
			Me.btnAddResponsiblePerson = New DevExpress.XtraEditors.SimpleButton()
			Me.grdZHD = New DevExpress.XtraGrid.GridControl()
			Me.gvResponsiblePersons = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.ImageList1 = New System.Windows.Forms.ImageList()
			Me.BarManager1 = New DevExpress.XtraBars.BarManager()
			Me.Bar1 = New DevExpress.XtraBars.Bar()
			Me.bsiLblErstellt = New DevExpress.XtraBars.BarStaticItem()
			Me.bsiCreated = New DevExpress.XtraBars.BarStaticItem()
			Me.bsiLblGeaendert = New DevExpress.XtraBars.BarStaticItem()
			Me.bsiChanged = New DevExpress.XtraBars.BarStaticItem()
			Me.bbiDatamatrix = New DevExpress.XtraBars.BarButtonItem()
			Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
			Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
			Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
			Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
			Me.CutItem1 = New DevExpress.XtraRichEdit.UI.CutItem()
			Me.CopyItem1 = New DevExpress.XtraRichEdit.UI.CopyItem()
			Me.PasteItem1 = New DevExpress.XtraRichEdit.UI.PasteItem()
			Me.PasteSpecialItem1 = New DevExpress.XtraRichEdit.UI.PasteSpecialItem()
			Me.ChangeFontNameItem1 = New DevExpress.XtraRichEdit.UI.ChangeFontNameItem()
			Me.RepositoryItemFontEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemFontEdit()
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
			Me.btnShowTemplate = New DevExpress.XtraBars.BarSubItem()
			Me.RepositoryItemComboBox1 = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
			Me.RepositoryItemComboBox2 = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
			Me.ImageCollection1 = New DevExpress.Utils.ImageCollection()
			CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.XtraTabControl1.SuspendLayout()
			Me.xtabAllgemein.SuspendLayout()
			Me.xtabVermittlung.SuspendLayout()
			Me.xtabContact.SuspendLayout()
			Me.xtabBilling.SuspendLayout()
			Me.xtabOthers.SuspendLayout()
			Me.xtabDocMng.SuspendLayout()
			CType(Me.pcNavMain, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.pcNavMain.SuspendLayout()
			CType(Me.navMain, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.sccMain, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.sccMain.SuspendLayout()
			CType(Me.grpzustaendigepersonen, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.grpzustaendigepersonen.SuspendLayout()
			CType(Me.grdZHD, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvResponsiblePersons, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.RepositoryItemFontEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.RepositoryItemRichEditFontSizeEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.rep_cbo_Template, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.RepositoryItemComboBox1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.RepositoryItemComboBox2, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.ImageCollection1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			'
			'XtraTabControl1
			'
			Me.XtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.XtraTabControl1.Location = New System.Drawing.Point(5, 5)
			Me.XtraTabControl1.Name = "XtraTabControl1"
			Me.XtraTabControl1.SelectedTabPage = Me.xtabAllgemein
			Me.XtraTabControl1.Size = New System.Drawing.Size(1099, 505)
			Me.XtraTabControl1.TabIndex = 2
			Me.XtraTabControl1.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabAllgemein, Me.xtabVermittlung, Me.xtabContact, Me.xtabBilling, Me.xtabOthers, Me.xtabDocMng})
			'
			'xtabAllgemein
			'
			Me.xtabAllgemein.Controls.Add(Me.ucCommonData)
			Me.xtabAllgemein.Name = "xtabAllgemein"
			Me.xtabAllgemein.Size = New System.Drawing.Size(1093, 477)
			Me.xtabAllgemein.Text = "Allgemein"
			'
			'ucCommonData
			'
			Me.ucCommonData.Appearance.BackColor = System.Drawing.Color.Transparent
			Me.ucCommonData.Appearance.Options.UseBackColor = True
			Me.ucCommonData.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucCommonData.IsIntialControlDataLoaded = False
			Me.ucCommonData.Location = New System.Drawing.Point(0, 0)
			Me.ucCommonData.MinimumSize = New System.Drawing.Size(1092, 467)
			Me.ucCommonData.Name = "ucCommonData"
			Me.ucCommonData.Padding = New System.Windows.Forms.Padding(5)
			Me.ucCommonData.Size = New System.Drawing.Size(1093, 477)
			Me.ucCommonData.TabIndex = 0
			'
			'xtabVermittlung
			'
			Me.xtabVermittlung.Controls.Add(Me.ucMediationAndRental)
			Me.xtabVermittlung.Name = "xtabVermittlung"
			Me.xtabVermittlung.Size = New System.Drawing.Size(1093, 477)
			Me.xtabVermittlung.Text = "Vermittlung und Verleih"
			'
			'ucMediationAndRental
			'
			Me.ucMediationAndRental.Appearance.BackColor = System.Drawing.Color.Transparent
			Me.ucMediationAndRental.Appearance.Options.UseBackColor = True
			Me.ucMediationAndRental.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucMediationAndRental.IsIntialControlDataLoaded = False
			Me.ucMediationAndRental.Location = New System.Drawing.Point(0, 0)
			Me.ucMediationAndRental.Name = "ucMediationAndRental"
			Me.ucMediationAndRental.Padding = New System.Windows.Forms.Padding(5)
			Me.ucMediationAndRental.Size = New System.Drawing.Size(1093, 477)
			Me.ucMediationAndRental.TabIndex = 0
			'
			'xtabContact
			'
			Me.xtabContact.Controls.Add(Me.ucContactData)
			Me.xtabContact.Name = "xtabContact"
			Me.xtabContact.Size = New System.Drawing.Size(1093, 477)
			Me.xtabContact.Text = "Kontakte"
			'
			'ucContactData
			'
			Me.ucContactData.Appearance.BackColor = System.Drawing.Color.Transparent
			Me.ucContactData.Appearance.Options.UseBackColor = True
			Me.ucContactData.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucContactData.IsIntialControlDataLoaded = False
			Me.ucContactData.Location = New System.Drawing.Point(0, 0)
			Me.ucContactData.Name = "ucContactData"
			Me.ucContactData.Padding = New System.Windows.Forms.Padding(5)
			Me.ucContactData.Size = New System.Drawing.Size(1093, 477)
			Me.ucContactData.TabIndex = 0
			'
			'xtabBilling
			'
			Me.xtabBilling.Controls.Add(Me.ucAccountAndSales)
			Me.xtabBilling.Name = "xtabBilling"
			Me.xtabBilling.Size = New System.Drawing.Size(1093, 477)
			Me.xtabBilling.Text = "Fakturaadressen und Umsatzzahlen"
			'
			'ucAccountAndSales
			'
			Me.ucAccountAndSales.Appearance.BackColor = System.Drawing.Color.Transparent
			Me.ucAccountAndSales.Appearance.Options.UseBackColor = True
			Me.ucAccountAndSales.IsIntialControlDataLoaded = False
			Me.ucAccountAndSales.Location = New System.Drawing.Point(3, 3)
			Me.ucAccountAndSales.Name = "ucAccountAndSales"
			Me.ucAccountAndSales.Padding = New System.Windows.Forms.Padding(5)
			Me.ucAccountAndSales.Size = New System.Drawing.Size(1237, 505)
			Me.ucAccountAndSales.TabIndex = 0
			'
			'xtabOthers
			'
			Me.xtabOthers.Controls.Add(Me.ucAdditionalInfo)
			Me.xtabOthers.Name = "xtabOthers"
			Me.xtabOthers.Size = New System.Drawing.Size(1093, 477)
			Me.xtabOthers.Text = "Sonstige Auskünfte"
			'
			'ucAdditionalInfo
			'
			Me.ucAdditionalInfo.Appearance.BackColor = System.Drawing.Color.Transparent
			Me.ucAdditionalInfo.Appearance.Options.UseBackColor = True
			Me.ucAdditionalInfo.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucAdditionalInfo.IsIntialControlDataLoaded = False
			Me.ucAdditionalInfo.Location = New System.Drawing.Point(0, 0)
			Me.ucAdditionalInfo.Name = "ucAdditionalInfo"
			Me.ucAdditionalInfo.Padding = New System.Windows.Forms.Padding(5)
			Me.ucAdditionalInfo.Size = New System.Drawing.Size(1093, 477)
			Me.ucAdditionalInfo.TabIndex = 0
			'
			'xtabDocMng
			'
			Me.xtabDocMng.Controls.Add(Me.ucDocumentManagement)
			Me.xtabDocMng.Name = "xtabDocMng"
			Me.xtabDocMng.Padding = New System.Windows.Forms.Padding(5)
			Me.xtabDocMng.Size = New System.Drawing.Size(1093, 477)
			Me.xtabDocMng.Text = "Dokumentenverwaltung"
			'
			'ucDocumentManagement
			'
			Me.ucDocumentManagement.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucDocumentManagement.IsIntialControlDataLoaded = False
			Me.ucDocumentManagement.Location = New System.Drawing.Point(5, 5)
			Me.ucDocumentManagement.Name = "ucDocumentManagement"
			Me.ucDocumentManagement.Padding = New System.Windows.Forms.Padding(2)
			Me.ucDocumentManagement.Size = New System.Drawing.Size(1083, 467)
			Me.ucDocumentManagement.TabIndex = 0
			'
			'pcNavMain
			'
			Me.pcNavMain.Controls.Add(Me.navMain)
			Me.pcNavMain.Dock = System.Windows.Forms.DockStyle.Left
			Me.pcNavMain.Location = New System.Drawing.Point(0, 0)
			Me.pcNavMain.Name = "pcNavMain"
			Me.pcNavMain.Padding = New System.Windows.Forms.Padding(10)
			Me.pcNavMain.Size = New System.Drawing.Size(196, 739)
			Me.pcNavMain.TabIndex = 162
			'
			'navMain
			'
			Me.navMain.ActiveGroup = Nothing
			Me.navMain.Dock = System.Windows.Forms.DockStyle.Fill
			Me.navMain.Location = New System.Drawing.Point(12, 12)
			Me.navMain.Name = "navMain"
			Me.navMain.OptionsNavPane.ExpandedWidth = 172
			Me.navMain.Size = New System.Drawing.Size(172, 715)
			Me.navMain.TabIndex = 19
			Me.navMain.Text = "NavBarControl1"
			'
			'libVakAls
			'
			Me.libVakAls.Appearance.BackColor = System.Drawing.Color.Transparent
			Me.libVakAls.Appearance.Options.UseBackColor = True
			Me.libVakAls.Location = New System.Drawing.Point(423, 340)
			Me.libVakAls.Name = "libVakAls"
			Me.libVakAls.Size = New System.Drawing.Size(55, 13)
			Me.libVakAls.TabIndex = 163
			Me.libVakAls.TabStop = True
			Me.libVakAls.Text = "Kunden-Nr."
			'
			'sccMain
			'
			Me.sccMain.Dock = System.Windows.Forms.DockStyle.Fill
			Me.sccMain.Horizontal = False
			Me.sccMain.Location = New System.Drawing.Point(196, 0)
			Me.sccMain.Name = "sccMain"
			Me.sccMain.Panel1.Controls.Add(Me.XtraTabControl1)
			Me.sccMain.Panel1.Padding = New System.Windows.Forms.Padding(5)
			Me.sccMain.Panel1.Text = "Panel1"
			Me.sccMain.Panel2.Controls.Add(Me.grpzustaendigepersonen)
			Me.sccMain.Panel2.Padding = New System.Windows.Forms.Padding(5)
			Me.sccMain.Panel2.Text = "Panel2"
			Me.sccMain.Size = New System.Drawing.Size(1109, 739)
			Me.sccMain.SplitterPosition = 515
			Me.sccMain.TabIndex = 164
			Me.sccMain.Text = "SplitContainerControl1"
			'
			'grpzustaendigepersonen
			'
			Me.grpzustaendigepersonen.AppearanceCaption.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grpzustaendigepersonen.AppearanceCaption.Options.UseFont = True
			Me.grpzustaendigepersonen.Controls.Add(Me.btnAddResponsiblePerson)
			Me.grpzustaendigepersonen.Controls.Add(Me.grdZHD)
			Me.grpzustaendigepersonen.Dock = System.Windows.Forms.DockStyle.Fill
			Me.grpzustaendigepersonen.Location = New System.Drawing.Point(5, 5)
			Me.grpzustaendigepersonen.Name = "grpzustaendigepersonen"
			Me.grpzustaendigepersonen.Size = New System.Drawing.Size(1099, 209)
			Me.grpzustaendigepersonen.TabIndex = 164
			Me.grpzustaendigepersonen.Text = "Zuständige Personen"
			'
			'btnAddResponsiblePerson
			'
			Me.btnAddResponsiblePerson.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnAddResponsiblePerson.ImageOptions.Image = CType(resources.GetObject("btnAddResponsiblePerson.ImageOptions.Image"), System.Drawing.Image)
			Me.btnAddResponsiblePerson.Location = New System.Drawing.Point(1061, 2)
			Me.btnAddResponsiblePerson.Name = "btnAddResponsiblePerson"
			Me.btnAddResponsiblePerson.Size = New System.Drawing.Size(27, 15)
			Me.btnAddResponsiblePerson.TabIndex = 213
			'
			'grdZHD
			'
			Me.grdZHD.Dock = System.Windows.Forms.DockStyle.Fill
			Me.grdZHD.Location = New System.Drawing.Point(2, 20)
			Me.grdZHD.MainView = Me.gvResponsiblePersons
			Me.grdZHD.Name = "grdZHD"
			Me.grdZHD.Size = New System.Drawing.Size(1095, 187)
			Me.grdZHD.TabIndex = 2
			Me.grdZHD.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvResponsiblePersons})
			'
			'gvResponsiblePersons
			'
			Me.gvResponsiblePersons.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			Me.gvResponsiblePersons.GridControl = Me.grdZHD
			Me.gvResponsiblePersons.Name = "gvResponsiblePersons"
			Me.gvResponsiblePersons.OptionsBehavior.Editable = False
			Me.gvResponsiblePersons.OptionsSelection.EnableAppearanceFocusedCell = False
			Me.gvResponsiblePersons.OptionsView.ShowGroupPanel = False
			Me.gvResponsiblePersons.OptionsView.ShowIndicator = False
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
			'BarManager1
			'
			Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar1})
			Me.BarManager1.DockControls.Add(Me.barDockControlTop)
			Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
			Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
			Me.BarManager1.DockControls.Add(Me.barDockControlRight)
			Me.BarManager1.Form = Me
			Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.CutItem1, Me.CopyItem1, Me.PasteItem1, Me.PasteSpecialItem1, Me.ChangeFontNameItem1, Me.ChangeFontSizeItem1, Me.ChangeFontColorItem1, Me.ChangeFontBackColorItem1, Me.ToggleFontBoldItem1, Me.ToggleFontItalicItem1, Me.ToggleFontUnderlineItem1, Me.ToggleFontDoubleUnderlineItem1, Me.ToggleFontStrikeoutItem1, Me.ToggleFontDoubleStrikeoutItem1, Me.ToggleFontSuperscriptItem1, Me.ToggleFontSubscriptItem1, Me.ChangeTextCaseItem1, Me.MakeTextUpperCaseItem1, Me.MakeTextLowerCaseItem1, Me.ToggleTextCaseItem1, Me.FontSizeIncreaseItem1, Me.FontSizeDecreaseItem1, Me.ClearFormattingItem1, Me.ShowFontFormItem1, Me.ToggleParagraphAlignmentLeftItem1, Me.ToggleParagraphAlignmentCenterItem1, Me.ToggleParagraphAlignmentRightItem1, Me.ToggleParagraphAlignmentJustifyItem1, Me.ChangeParagraphLineSpacingItem1, Me.SetSingleParagraphSpacingItem1, Me.SetSesquialteralParagraphSpacingItem1, Me.SetDoubleParagraphSpacingItem1, Me.ShowLineSpacingFormItem1, Me.AddSpacingBeforeParagraphItem1, Me.RemoveSpacingBeforeParagraphItem1, Me.AddSpacingAfterParagraphItem1, Me.RemoveSpacingAfterParagraphItem1, Me.ToggleBulletedListItem1, Me.ToggleNumberingListItem1, Me.ToggleMultiLevelListItem1, Me.DecreaseIndentItem1, Me.IncreaseIndentItem1, Me.ToggleShowWhitespaceItem1, Me.ShowParagraphFormItem1, Me.ChangeStyleItem1, Me.FindItem1, Me.ReplaceItem1, Me.FileSaveItem1, Me.btnShowTemplate, Me.bsiCreated, Me.bsiChanged, Me.bsiLblErstellt, Me.bsiLblGeaendert, Me.bbiDatamatrix})
			Me.BarManager1.MaxItemId = 66
			Me.BarManager1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemFontEdit1, Me.RepositoryItemRichEditFontSizeEdit1, Me.rep_cbo_Template, Me.RepositoryItemComboBox1, Me.RepositoryItemComboBox2})
			Me.BarManager1.StatusBar = Me.Bar1
			'
			'Bar1
			'
			Me.Bar1.BarName = "Benutzerdefiniert 2"
			Me.Bar1.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
			Me.Bar1.DockCol = 0
			Me.Bar1.DockRow = 0
			Me.Bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
			Me.Bar1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiLblErstellt, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bsiCreated), New DevExpress.XtraBars.LinkPersistInfo(Me.bsiLblGeaendert), New DevExpress.XtraBars.LinkPersistInfo(Me.bsiChanged), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiDatamatrix, True)})
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
			Me.bsiCreated.Caption = "{0}"
			Me.bsiCreated.Id = 60
			Me.bsiCreated.Name = "bsiCreated"
			'
			'bsiLblGeaendert
			'
			Me.bsiLblGeaendert.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
			Me.bsiLblGeaendert.Caption = "Geändert:"
			Me.bsiLblGeaendert.Id = 64
			Me.bsiLblGeaendert.ItemAppearance.Normal.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
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
			'bbiDatamatrix
			'
			Me.bbiDatamatrix.Id = 65
			Me.bbiDatamatrix.ImageOptions.Image = CType(resources.GetObject("bbiDatamatrix.ImageOptions.Image"), System.Drawing.Image)
			Me.bbiDatamatrix.Name = "bbiDatamatrix"
			Me.bbiDatamatrix.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
			'
			'barDockControlTop
			'
			Me.barDockControlTop.CausesValidation = False
			Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
			Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
			Me.barDockControlTop.Manager = Me.BarManager1
			Me.barDockControlTop.Size = New System.Drawing.Size(1305, 0)
			'
			'barDockControlBottom
			'
			Me.barDockControlBottom.CausesValidation = False
			Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
			Me.barDockControlBottom.Location = New System.Drawing.Point(0, 739)
			Me.barDockControlBottom.Manager = Me.BarManager1
			Me.barDockControlBottom.Size = New System.Drawing.Size(1305, 27)
			'
			'barDockControlLeft
			'
			Me.barDockControlLeft.CausesValidation = False
			Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
			Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
			Me.barDockControlLeft.Manager = Me.BarManager1
			Me.barDockControlLeft.Size = New System.Drawing.Size(0, 739)
			'
			'barDockControlRight
			'
			Me.barDockControlRight.CausesValidation = False
			Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
			Me.barDockControlRight.Location = New System.Drawing.Point(1305, 0)
			Me.barDockControlRight.Manager = Me.BarManager1
			Me.barDockControlRight.Size = New System.Drawing.Size(0, 739)
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
			Me.ChangeFontNameItem1.Edit = Me.RepositoryItemFontEdit1
			Me.ChangeFontNameItem1.Id = 4
			Me.ChangeFontNameItem1.Name = "ChangeFontNameItem1"
			'
			'RepositoryItemFontEdit1
			'
			Me.RepositoryItemFontEdit1.AutoHeight = False
			Me.RepositoryItemFontEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.RepositoryItemFontEdit1.Name = "RepositoryItemFontEdit1"
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
			'btnShowTemplate
			'
			Me.btnShowTemplate.Caption = "Vorlage laden"
			Me.btnShowTemplate.Id = 59
			Me.btnShowTemplate.Name = "btnShowTemplate"
			Me.btnShowTemplate.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
			Me.btnShowTemplate.ShowMenuCaption = True
			'
			'RepositoryItemComboBox1
			'
			Me.RepositoryItemComboBox1.AutoHeight = False
			Me.RepositoryItemComboBox1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.RepositoryItemComboBox1.Name = "RepositoryItemComboBox1"
			'
			'RepositoryItemComboBox2
			'
			Me.RepositoryItemComboBox2.AutoHeight = False
			Me.RepositoryItemComboBox2.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.RepositoryItemComboBox2.Name = "RepositoryItemComboBox2"
			'
			'ImageCollection1
			'
			Me.ImageCollection1.ImageStream = CType(resources.GetObject("ImageCollection1.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
			Me.ImageCollection1.Images.SetKeyName(0, "Plus.png")
			Me.ImageCollection1.Images.SetKeyName(1, "Save.png")
			Me.ImageCollection1.InsertGalleryImage("print_16x16.png", "images/print/print_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/print/print_16x16.png"), 2)
			Me.ImageCollection1.Images.SetKeyName(2, "print_16x16.png")
			Me.ImageCollection1.Images.SetKeyName(3, "close.ico")
			Me.ImageCollection1.Images.SetKeyName(4, "Delete.png")
			Me.ImageCollection1.Images.SetKeyName(5, "Hyperlink.png")
			Me.ImageCollection1.InsertGalleryImage("contact_16x16.png", "images/mail/contact_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/mail/contact_16x16.png"), 6)
			Me.ImageCollection1.Images.SetKeyName(6, "contact_16x16.png")
			Me.ImageCollection1.Images.SetKeyName(7, "Funds.png")
			Me.ImageCollection1.InsertGalleryImage("newtask_16x16.png", "images/tasks/newtask_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/tasks/newtask_16x16.png"), 8)
			Me.ImageCollection1.Images.SetKeyName(8, "newtask_16x16.png")
			Me.ImageCollection1.InsertGalleryImage("properties_16x16.png", "images/setup/properties_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/setup/properties_16x16.png"), 9)
			Me.ImageCollection1.Images.SetKeyName(9, "properties_16x16.png")
			Me.ImageCollection1.InsertGalleryImage("barcode_16x16.png", "office2013/content/barcode_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("office2013/content/barcode_16x16.png"), 10)
			Me.ImageCollection1.Images.SetKeyName(10, "barcode_16x16.png")
			Me.ImageCollection1.InsertGalleryImage("sales_16x16.png", "devav/view/sales_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("devav/view/sales_16x16.png"), 11)
			Me.ImageCollection1.Images.SetKeyName(11, "sales_16x16.png")
			'
			'frmCustomers
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(1305, 766)
			Me.Controls.Add(Me.sccMain)
			Me.Controls.Add(Me.libVakAls)
			Me.Controls.Add(Me.pcNavMain)
			Me.Controls.Add(Me.barDockControlLeft)
			Me.Controls.Add(Me.barDockControlRight)
			Me.Controls.Add(Me.barDockControlBottom)
			Me.Controls.Add(Me.barDockControlTop)
			Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
			Me.MinimumSize = New System.Drawing.Size(1313, 793)
			Me.Name = "frmCustomers"
			Me.Text = "Verwaltung von Kundendaten"
			CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.XtraTabControl1.ResumeLayout(False)
			Me.xtabAllgemein.ResumeLayout(False)
			Me.xtabVermittlung.ResumeLayout(False)
			Me.xtabContact.ResumeLayout(False)
			Me.xtabBilling.ResumeLayout(False)
			Me.xtabOthers.ResumeLayout(False)
			Me.xtabDocMng.ResumeLayout(False)
			CType(Me.pcNavMain, System.ComponentModel.ISupportInitialize).EndInit()
			Me.pcNavMain.ResumeLayout(False)
			CType(Me.navMain, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.sccMain, System.ComponentModel.ISupportInitialize).EndInit()
			Me.sccMain.ResumeLayout(False)
			CType(Me.grpzustaendigepersonen, System.ComponentModel.ISupportInitialize).EndInit()
			Me.grpzustaendigepersonen.ResumeLayout(False)
			CType(Me.grdZHD, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvResponsiblePersons, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.RepositoryItemFontEdit1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.RepositoryItemRichEditFontSizeEdit1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.rep_cbo_Template, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.RepositoryItemComboBox1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.RepositoryItemComboBox2, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.ImageCollection1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)
			Me.PerformLayout()

		End Sub
		Friend WithEvents XtraTabControl1 As DevExpress.XtraTab.XtraTabControl
		Friend WithEvents xtabAllgemein As DevExpress.XtraTab.XtraTabPage
		Friend WithEvents xtabVermittlung As DevExpress.XtraTab.XtraTabPage
		Friend WithEvents pcNavMain As DevExpress.XtraEditors.PanelControl
		Friend WithEvents libVakAls As DevExpress.XtraEditors.LabelControl
		Friend WithEvents sccMain As DevExpress.XtraEditors.SplitContainerControl
		Friend WithEvents grpzustaendigepersonen As DevExpress.XtraEditors.GroupControl
		Friend WithEvents grdZHD As DevExpress.XtraGrid.GridControl
		Friend WithEvents gvResponsiblePersons As DevExpress.XtraGrid.Views.Grid.GridView
		Friend WithEvents navMain As DevExpress.XtraNavBar.NavBarControl
		Friend WithEvents xtabContact As DevExpress.XtraTab.XtraTabPage
		Friend WithEvents xtabBilling As DevExpress.XtraTab.XtraTabPage
		Friend WithEvents xtabOthers As DevExpress.XtraTab.XtraTabPage
		Friend WithEvents xtabDocMng As DevExpress.XtraTab.XtraTabPage
		Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
		Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
		Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
		Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
		Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
		Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
		Friend WithEvents CutItem1 As DevExpress.XtraRichEdit.UI.CutItem
		Friend WithEvents CopyItem1 As DevExpress.XtraRichEdit.UI.CopyItem
		Friend WithEvents PasteItem1 As DevExpress.XtraRichEdit.UI.PasteItem
		Friend WithEvents PasteSpecialItem1 As DevExpress.XtraRichEdit.UI.PasteSpecialItem
		Friend WithEvents ChangeFontNameItem1 As DevExpress.XtraRichEdit.UI.ChangeFontNameItem
		Friend WithEvents RepositoryItemFontEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemFontEdit
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
		Friend WithEvents btnShowTemplate As DevExpress.XtraBars.BarSubItem
		Friend WithEvents RepositoryItemComboBox1 As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
		Friend WithEvents RepositoryItemComboBox2 As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
		Friend WithEvents ucMediationAndRental As ucMediationAndRentalData
		Friend WithEvents ucCommonData As SP.KD.CustomerMng.UI.ucCommonData
		Friend WithEvents ucContactData As SP.KD.CustomerMng.UI.ucContactData
		Friend WithEvents ucAccountAndSales As SP.KD.CustomerMng.UI.ucAccountAndSales
		Friend WithEvents ucAdditionalInfo As SP.KD.CustomerMng.UI.ucAdditionalInfo
		Friend WithEvents ucDocumentManagement As SP.KD.CustomerMng.UI.ucDocumentManagement
		Friend WithEvents btnAddResponsiblePerson As DevExpress.XtraEditors.SimpleButton
		Friend WithEvents ImageCollection1 As DevExpress.Utils.ImageCollection
		Friend WithEvents Bar1 As DevExpress.XtraBars.Bar
		Friend WithEvents bsiCreated As DevExpress.XtraBars.BarStaticItem
		Friend WithEvents bsiChanged As DevExpress.XtraBars.BarStaticItem
		Friend WithEvents bsiLblErstellt As DevExpress.XtraBars.BarStaticItem
		Friend WithEvents bsiLblGeaendert As DevExpress.XtraBars.BarStaticItem
		Friend WithEvents bbiDatamatrix As DevExpress.XtraBars.BarButtonItem

    End Class
End Namespace