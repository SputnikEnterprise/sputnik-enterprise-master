Imports DevExpress.Skins
Imports DevExpress.LookAndFeel
Imports DevExpress.UserSkins
Imports DevExpress.XtraBars.Helpers


<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMAMassenversand
  Inherits DevExpress.XtraEditors.XtraForm

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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMAMassenversand))
		Dim OptionsSpelling1 As DevExpress.XtraSpellChecker.OptionsSpelling = New DevExpress.XtraSpellChecker.OptionsSpelling()
		Me.splitContainerControl = New DevExpress.XtraEditors.SplitContainerControl()
		Me.navBarControl = New DevExpress.XtraNavBar.NavBarControl()
		Me.inboxItem = New DevExpress.XtraNavBar.NavBarItem()
		Me.outboxItem = New DevExpress.XtraNavBar.NavBarItem()
		Me.draftsItem = New DevExpress.XtraNavBar.NavBarItem()
		Me.trashItem = New DevExpress.XtraNavBar.NavBarItem()
		Me.calendarItem = New DevExpress.XtraNavBar.NavBarItem()
		Me.tasksItem = New DevExpress.XtraNavBar.NavBarItem()
		Me.navbarImageListLarge = New System.Windows.Forms.ImageList(Me.components)
		Me.navbarImageList = New System.Windows.Forms.ImageList(Me.components)
		Me.rtfContent = New DevExpress.XtraRichEdit.RichEditControl()
		Me.ribbonControl = New DevExpress.XtraBars.Ribbon.RibbonControl()
		Me.ribbonImageCollection = New DevExpress.Utils.ImageCollection(Me.components)
		Me.iNew = New DevExpress.XtraBars.BarButtonItem()
		Me.iOpen = New DevExpress.XtraBars.BarButtonItem()
		Me.iClose = New DevExpress.XtraBars.BarButtonItem()
		Me.iFind = New DevExpress.XtraBars.BarButtonItem()
		Me.iSaveAs = New DevExpress.XtraBars.BarButtonItem()
		Me.iExit = New DevExpress.XtraBars.BarButtonItem()
		Me.iHelp = New DevExpress.XtraBars.BarButtonItem()
		Me.iAbout = New DevExpress.XtraBars.BarButtonItem()
		Me.siStatus = New DevExpress.XtraBars.BarStaticItem()
		Me.siInfo = New DevExpress.XtraBars.BarStaticItem()
		Me.alignButtonGroup = New DevExpress.XtraBars.BarButtonGroup()
		Me.iBoldFontStyle = New DevExpress.XtraBars.BarButtonItem()
		Me.iItalicFontStyle = New DevExpress.XtraBars.BarButtonItem()
		Me.iUnderlinedFontStyle = New DevExpress.XtraBars.BarButtonItem()
		Me.fontStyleButtonGroup = New DevExpress.XtraBars.BarButtonGroup()
		Me.iLeftTextAlign = New DevExpress.XtraBars.BarButtonItem()
		Me.iCenterTextAlign = New DevExpress.XtraBars.BarButtonItem()
		Me.iRightTextAlign = New DevExpress.XtraBars.BarButtonItem()
		Me.rgbiSkins = New DevExpress.XtraBars.RibbonGalleryBarItem()
		Me.FileNewItem1 = New DevExpress.XtraRichEdit.UI.FileNewItem()
		Me.FileOpenItem1 = New DevExpress.XtraRichEdit.UI.FileOpenItem()
		Me.FileSaveItem1 = New DevExpress.XtraRichEdit.UI.FileSaveItem()
		Me.FileSaveAsItem1 = New DevExpress.XtraRichEdit.UI.FileSaveAsItem()
		Me.QuickPrintItem1 = New DevExpress.XtraRichEdit.UI.QuickPrintItem()
		Me.PrintItem1 = New DevExpress.XtraRichEdit.UI.PrintItem()
		Me.PrintPreviewItem1 = New DevExpress.XtraRichEdit.UI.PrintPreviewItem()
		Me.UndoItem1 = New DevExpress.XtraRichEdit.UI.UndoItem()
		Me.RedoItem1 = New DevExpress.XtraRichEdit.UI.RedoItem()
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
		Me.RepositoryItemRichEditStyleEdit1 = New DevExpress.XtraRichEdit.Design.RepositoryItemRichEditStyleEdit()
		Me.FindItem1 = New DevExpress.XtraRichEdit.UI.FindItem()
		Me.ReplaceItem1 = New DevExpress.XtraRichEdit.UI.ReplaceItem()
		Me.BarButtonGroup1 = New DevExpress.XtraBars.BarButtonGroup()
		Me.BarButtonGroup2 = New DevExpress.XtraBars.BarButtonGroup()
		Me.BarButtonGroup3 = New DevExpress.XtraBars.BarButtonGroup()
		Me.BarButtonGroup4 = New DevExpress.XtraBars.BarButtonGroup()
		Me.BarButtonGroup5 = New DevExpress.XtraBars.BarButtonGroup()
		Me.BarButtonGroup6 = New DevExpress.XtraBars.BarButtonGroup()
		Me.BarButtonGroup7 = New DevExpress.XtraBars.BarButtonGroup()
		Me.BarButtonGroup8 = New DevExpress.XtraBars.BarButtonGroup()
		Me.BarButtonGroup9 = New DevExpress.XtraBars.BarButtonGroup()
		Me.bbgSpeichern = New DevExpress.XtraBars.BarButtonGroup()
		Me.RibbonGalleryBarItem1 = New DevExpress.XtraBars.RibbonGalleryBarItem()
		Me.bei_Template = New DevExpress.XtraBars.BarEditItem()
		Me.rep_lueTemplate = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit()
		Me.ribbonImageCollectionLarge = New DevExpress.Utils.ImageCollection(Me.components)
		Me.HomeRibbonPage1 = New DevExpress.XtraRichEdit.UI.HomeRibbonPage()
		Me.frpZwischenablage = New DevExpress.XtraRichEdit.UI.ClipboardRibbonPageGroup()
		Me.frpSchriftart = New DevExpress.XtraRichEdit.UI.FontRibbonPageGroup()
		Me.frpAbsatz = New DevExpress.XtraRichEdit.UI.ParagraphRibbonPageGroup()
		Me.frpBearbeiten = New DevExpress.XtraRichEdit.UI.EditingRibbonPageGroup()
		Me.ribbonStatusBar = New DevExpress.XtraBars.Ribbon.RibbonStatusBar()
		Me.spellChecker = New DevExpress.XtraSpellChecker.SpellChecker(Me.components)
		Me.popupControlContainer2 = New DevExpress.XtraBars.PopupControlContainer(Me.components)
		Me.buttonEdit = New DevExpress.XtraEditors.ButtonEdit()
		Me.popupControlContainer1 = New DevExpress.XtraBars.PopupControlContainer(Me.components)
		Me.someLabelControl2 = New DevExpress.XtraEditors.LabelControl()
		Me.someLabelControl1 = New DevExpress.XtraEditors.LabelControl()
		Me.RichEditBarController1 = New DevExpress.XtraRichEdit.UI.RichEditBarController(Me.components)
		CType(Me.splitContainerControl, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.splitContainerControl.SuspendLayout()
		CType(Me.navBarControl, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.ribbonControl, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.ribbonImageCollection, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.RepositoryItemFontEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.RepositoryItemRichEditFontSizeEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.RepositoryItemRichEditStyleEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.rep_lueTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.ribbonImageCollectionLarge, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.popupControlContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.popupControlContainer2.SuspendLayout()
		CType(Me.buttonEdit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.popupControlContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.popupControlContainer1.SuspendLayout()
		CType(Me.RichEditBarController1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'splitContainerControl
		'
		Me.splitContainerControl.Dock = System.Windows.Forms.DockStyle.Fill
		Me.splitContainerControl.Location = New System.Drawing.Point(0, 120)
		Me.splitContainerControl.Name = "splitContainerControl"
		Me.splitContainerControl.Padding = New System.Windows.Forms.Padding(6)
		Me.splitContainerControl.Panel1.Controls.Add(Me.navBarControl)
		Me.splitContainerControl.Panel1.Text = "Panel1"
		Me.splitContainerControl.Panel2.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.splitContainerControl.Panel2.Appearance.Options.UseBackColor = True
		Me.splitContainerControl.Panel2.Controls.Add(Me.rtfContent)
		Me.splitContainerControl.Panel2.Text = "Panel2"
		Me.splitContainerControl.Size = New System.Drawing.Size(858, 443)
		Me.splitContainerControl.SplitterPosition = 165
		Me.splitContainerControl.TabIndex = 1
		Me.splitContainerControl.Text = "splitContainerControl1"
		'
		'navBarControl
		'
		Me.navBarControl.Appearance.ItemActive.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.navBarControl.Appearance.ItemActive.ForeColor = System.Drawing.Color.Red
		Me.navBarControl.Appearance.ItemActive.Options.UseFont = True
		Me.navBarControl.Appearance.ItemActive.Options.UseForeColor = True
		Me.navBarControl.Appearance.ItemPressed.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.navBarControl.Appearance.ItemPressed.Options.UseFont = True
		Me.navBarControl.Appearance.LinkDropTarget.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.navBarControl.Appearance.LinkDropTarget.Options.UseFont = True
		Me.navBarControl.Dock = System.Windows.Forms.DockStyle.Fill
		Me.navBarControl.Items.AddRange(New DevExpress.XtraNavBar.NavBarItem() {Me.inboxItem, Me.outboxItem, Me.draftsItem, Me.trashItem, Me.calendarItem, Me.tasksItem})
		Me.navBarControl.LargeImages = Me.navbarImageListLarge
		Me.navBarControl.Location = New System.Drawing.Point(0, 0)
		Me.navBarControl.Name = "navBarControl"
		Me.navBarControl.OptionsNavPane.ExpandedWidth = 165
		Me.navBarControl.PaintStyleKind = DevExpress.XtraNavBar.NavBarViewKind.ExplorerBar
		Me.navBarControl.Size = New System.Drawing.Size(165, 431)
		Me.navBarControl.SmallImages = Me.navbarImageList
		Me.navBarControl.TabIndex = 1
		Me.navBarControl.Text = "navBarControl1"
		Me.navBarControl.View = New DevExpress.XtraNavBar.ViewInfo.StandardSkinNavigationPaneViewInfoRegistrator("DevExpress Style")
		'
		'inboxItem
		'
		Me.inboxItem.Caption = "Inbox"
		Me.inboxItem.ImageOptions.SmallImageIndex = 0
		Me.inboxItem.Name = "inboxItem"
		'
		'outboxItem
		'
		Me.outboxItem.Caption = "Outbox"
		Me.outboxItem.ImageOptions.SmallImageIndex = 1
		Me.outboxItem.Name = "outboxItem"
		'
		'draftsItem
		'
		Me.draftsItem.Caption = "Drafts"
		Me.draftsItem.ImageOptions.SmallImageIndex = 2
		Me.draftsItem.Name = "draftsItem"
		'
		'trashItem
		'
		Me.trashItem.Caption = "Trash"
		Me.trashItem.ImageOptions.SmallImageIndex = 3
		Me.trashItem.Name = "trashItem"
		'
		'calendarItem
		'
		Me.calendarItem.Caption = "Calendar"
		Me.calendarItem.ImageOptions.SmallImageIndex = 4
		Me.calendarItem.Name = "calendarItem"
		'
		'tasksItem
		'
		Me.tasksItem.Caption = "Tasks"
		Me.tasksItem.ImageOptions.SmallImageIndex = 5
		Me.tasksItem.Name = "tasksItem"
		'
		'navbarImageListLarge
		'
		Me.navbarImageListLarge.ImageStream = CType(resources.GetObject("navbarImageListLarge.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.navbarImageListLarge.TransparentColor = System.Drawing.Color.Transparent
		Me.navbarImageListLarge.Images.SetKeyName(0, "icon_contactlist.gif")
		Me.navbarImageListLarge.Images.SetKeyName(1, "Organizer_16x16.png")
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
		Me.navbarImageList.Images.SetKeyName(6, "icon_contactlist.gif")
		'
		'rtfContent
		'
		Me.rtfContent.Location = New System.Drawing.Point(0, 95)
		Me.rtfContent.MenuManager = Me.ribbonControl
		Me.rtfContent.Name = "rtfContent"
		Me.rtfContent.Size = New System.Drawing.Size(556, 340)
		Me.rtfContent.SpellChecker = Me.spellChecker
		Me.spellChecker.SetSpellCheckerOptions(Me.rtfContent, OptionsSpelling1)
		Me.rtfContent.TabIndex = 2
		'
		'ribbonControl
		'
		Me.ribbonControl.ApplicationButtonText = Nothing
		Me.ribbonControl.AutoSizeItems = True
		Me.ribbonControl.ExpandCollapseItem.Id = 0
		Me.ribbonControl.Images = Me.ribbonImageCollection
		Me.ribbonControl.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.ribbonControl.ExpandCollapseItem, Me.iNew, Me.iOpen, Me.iClose, Me.iFind, Me.iSaveAs, Me.iExit, Me.iHelp, Me.iAbout, Me.siStatus, Me.siInfo, Me.alignButtonGroup, Me.iBoldFontStyle, Me.iItalicFontStyle, Me.iUnderlinedFontStyle, Me.fontStyleButtonGroup, Me.iLeftTextAlign, Me.iCenterTextAlign, Me.iRightTextAlign, Me.rgbiSkins, Me.FileNewItem1, Me.FileOpenItem1, Me.FileSaveItem1, Me.FileSaveAsItem1, Me.QuickPrintItem1, Me.PrintItem1, Me.PrintPreviewItem1, Me.UndoItem1, Me.RedoItem1, Me.CutItem1, Me.CopyItem1, Me.PasteItem1, Me.PasteSpecialItem1, Me.ChangeFontNameItem1, Me.ChangeFontSizeItem1, Me.ChangeFontColorItem1, Me.ChangeFontBackColorItem1, Me.ToggleFontBoldItem1, Me.ToggleFontItalicItem1, Me.ToggleFontUnderlineItem1, Me.ToggleFontDoubleUnderlineItem1, Me.ToggleFontStrikeoutItem1, Me.ToggleFontDoubleStrikeoutItem1, Me.ToggleFontSuperscriptItem1, Me.ToggleFontSubscriptItem1, Me.ChangeTextCaseItem1, Me.MakeTextUpperCaseItem1, Me.MakeTextLowerCaseItem1, Me.ToggleTextCaseItem1, Me.FontSizeIncreaseItem1, Me.FontSizeDecreaseItem1, Me.ClearFormattingItem1, Me.ShowFontFormItem1, Me.ToggleParagraphAlignmentLeftItem1, Me.ToggleParagraphAlignmentCenterItem1, Me.ToggleParagraphAlignmentRightItem1, Me.ToggleParagraphAlignmentJustifyItem1, Me.ChangeParagraphLineSpacingItem1, Me.SetSingleParagraphSpacingItem1, Me.SetSesquialteralParagraphSpacingItem1, Me.SetDoubleParagraphSpacingItem1, Me.ShowLineSpacingFormItem1, Me.AddSpacingBeforeParagraphItem1, Me.RemoveSpacingBeforeParagraphItem1, Me.AddSpacingAfterParagraphItem1, Me.RemoveSpacingAfterParagraphItem1, Me.ToggleBulletedListItem1, Me.ToggleNumberingListItem1, Me.ToggleMultiLevelListItem1, Me.DecreaseIndentItem1, Me.IncreaseIndentItem1, Me.ToggleShowWhitespaceItem1, Me.ShowParagraphFormItem1, Me.ChangeStyleItem1, Me.FindItem1, Me.ReplaceItem1, Me.BarButtonGroup1, Me.BarButtonGroup2, Me.BarButtonGroup3, Me.BarButtonGroup4, Me.BarButtonGroup5, Me.BarButtonGroup6, Me.BarButtonGroup7, Me.BarButtonGroup8, Me.BarButtonGroup9, Me.bbgSpeichern, Me.RibbonGalleryBarItem1, Me.bei_Template})
		Me.ribbonControl.LargeImages = Me.ribbonImageCollectionLarge
		Me.ribbonControl.Location = New System.Drawing.Point(0, 0)
		Me.ribbonControl.MaxItemId = 134
		Me.ribbonControl.Name = "ribbonControl"
		Me.ribbonControl.Pages.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPage() {Me.HomeRibbonPage1})
		Me.ribbonControl.QuickToolbarItemLinks.Add(Me.bbgSpeichern)
		Me.ribbonControl.QuickToolbarItemLinks.Add(Me.bei_Template)
		Me.ribbonControl.QuickToolbarItemLinks.Add(Me.ribbonControl.ExpandCollapseItem, True)
		Me.ribbonControl.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemFontEdit1, Me.RepositoryItemRichEditFontSizeEdit1, Me.RepositoryItemRichEditStyleEdit1, Me.rep_lueTemplate})
		Me.ribbonControl.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2010
		Me.ribbonControl.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.[False]
		Me.ribbonControl.ShowCategoryInCaption = False
		Me.ribbonControl.ShowExpandCollapseButton = DevExpress.Utils.DefaultBoolean.[False]
		Me.ribbonControl.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.ShowOnMultiplePages
		Me.ribbonControl.ShowToolbarCustomizeItem = False
		Me.ribbonControl.Size = New System.Drawing.Size(858, 120)
		Me.ribbonControl.StatusBar = Me.ribbonStatusBar
		Me.ribbonControl.Toolbar.ShowCustomizeItem = False
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
		'iNew
		'
		Me.iNew.Caption = "New"
		Me.iNew.Description = "Creates a new, blank file."
		Me.iNew.Hint = "Creates a new, blank file"
		Me.iNew.Id = 1
		Me.iNew.ImageOptions.ImageIndex = 0
		Me.iNew.ImageOptions.LargeImageIndex = 0
		Me.iNew.Name = "iNew"
		'
		'iOpen
		'
		Me.iOpen.Caption = "Vorlage laden"
		Me.iOpen.Description = "Opens a file."
		Me.iOpen.Hint = "Opens a file"
		Me.iOpen.Id = 2
		Me.iOpen.ImageOptions.ImageIndex = 1
		Me.iOpen.ImageOptions.LargeImageIndex = 1
		Me.iOpen.Name = "iOpen"
		Me.iOpen.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText
		'
		'iClose
		'
		Me.iClose.Caption = "&Close"
		Me.iClose.Description = "Closes the active document."
		Me.iClose.Hint = "Closes the active document"
		Me.iClose.Id = 3
		Me.iClose.ImageOptions.ImageIndex = 2
		Me.iClose.ImageOptions.LargeImageIndex = 2
		Me.iClose.Name = "iClose"
		Me.iClose.RibbonStyle = CType((DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText Or DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText), DevExpress.XtraBars.Ribbon.RibbonItemStyles)
		'
		'iFind
		'
		Me.iFind.Caption = "Find"
		Me.iFind.Description = "Searches for the specified info."
		Me.iFind.Hint = "Searches for the specified info"
		Me.iFind.Id = 15
		Me.iFind.ImageOptions.ImageIndex = 3
		Me.iFind.ImageOptions.LargeImageIndex = 3
		Me.iFind.Name = "iFind"
		Me.iFind.RibbonStyle = CType((DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText Or DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText), DevExpress.XtraBars.Ribbon.RibbonItemStyles)
		'
		'iSaveAs
		'
		Me.iSaveAs.Caption = "Save As"
		Me.iSaveAs.Description = "Saves the active document in a different location."
		Me.iSaveAs.Hint = "Saves the active document in a different location"
		Me.iSaveAs.Id = 17
		Me.iSaveAs.ImageOptions.ImageIndex = 5
		Me.iSaveAs.ImageOptions.LargeImageIndex = 5
		Me.iSaveAs.Name = "iSaveAs"
		'
		'iExit
		'
		Me.iExit.Caption = "Exit"
		Me.iExit.Description = "Closes this program after prompting you to save unsaved data."
		Me.iExit.Hint = "Closes this program after prompting you to save unsaved data"
		Me.iExit.Id = 20
		Me.iExit.ImageOptions.ImageIndex = 6
		Me.iExit.ImageOptions.LargeImageIndex = 6
		Me.iExit.Name = "iExit"
		'
		'iHelp
		'
		Me.iHelp.Caption = "Help"
		Me.iHelp.Description = "Start the program help system."
		Me.iHelp.Hint = "Start the program help system"
		Me.iHelp.Id = 22
		Me.iHelp.ImageOptions.ImageIndex = 7
		Me.iHelp.ImageOptions.LargeImageIndex = 7
		Me.iHelp.Name = "iHelp"
		'
		'iAbout
		'
		Me.iAbout.Caption = "About"
		Me.iAbout.Description = "Displays general program information."
		Me.iAbout.Hint = "Displays general program information"
		Me.iAbout.Id = 24
		Me.iAbout.ImageOptions.ImageIndex = 8
		Me.iAbout.ImageOptions.LargeImageIndex = 8
		Me.iAbout.Name = "iAbout"
		'
		'siStatus
		'
		Me.siStatus.Caption = "Some Status Info"
		Me.siStatus.Id = 31
		Me.siStatus.Name = "siStatus"
		'
		'siInfo
		'
		Me.siInfo.Id = 32
		Me.siInfo.Name = "siInfo"
		'
		'alignButtonGroup
		'
		Me.alignButtonGroup.Caption = "Align Commands"
		Me.alignButtonGroup.Id = 52
		Me.alignButtonGroup.ItemLinks.Add(Me.iBoldFontStyle)
		Me.alignButtonGroup.ItemLinks.Add(Me.iItalicFontStyle)
		Me.alignButtonGroup.ItemLinks.Add(Me.iUnderlinedFontStyle)
		Me.alignButtonGroup.Name = "alignButtonGroup"
		'
		'iBoldFontStyle
		'
		Me.iBoldFontStyle.Caption = "Bold"
		Me.iBoldFontStyle.Id = 53
		Me.iBoldFontStyle.ImageOptions.ImageIndex = 9
		Me.iBoldFontStyle.Name = "iBoldFontStyle"
		'
		'iItalicFontStyle
		'
		Me.iItalicFontStyle.Caption = "Italic"
		Me.iItalicFontStyle.Id = 54
		Me.iItalicFontStyle.ImageOptions.ImageIndex = 10
		Me.iItalicFontStyle.Name = "iItalicFontStyle"
		'
		'iUnderlinedFontStyle
		'
		Me.iUnderlinedFontStyle.Caption = "Underlined"
		Me.iUnderlinedFontStyle.Id = 55
		Me.iUnderlinedFontStyle.ImageOptions.ImageIndex = 11
		Me.iUnderlinedFontStyle.Name = "iUnderlinedFontStyle"
		'
		'fontStyleButtonGroup
		'
		Me.fontStyleButtonGroup.Caption = "Font Style"
		Me.fontStyleButtonGroup.Id = 56
		Me.fontStyleButtonGroup.ItemLinks.Add(Me.iLeftTextAlign)
		Me.fontStyleButtonGroup.ItemLinks.Add(Me.iCenterTextAlign)
		Me.fontStyleButtonGroup.ItemLinks.Add(Me.iRightTextAlign)
		Me.fontStyleButtonGroup.Name = "fontStyleButtonGroup"
		'
		'iLeftTextAlign
		'
		Me.iLeftTextAlign.Caption = "Left"
		Me.iLeftTextAlign.Id = 57
		Me.iLeftTextAlign.ImageOptions.ImageIndex = 12
		Me.iLeftTextAlign.Name = "iLeftTextAlign"
		'
		'iCenterTextAlign
		'
		Me.iCenterTextAlign.Caption = "Center"
		Me.iCenterTextAlign.Id = 58
		Me.iCenterTextAlign.ImageOptions.ImageIndex = 13
		Me.iCenterTextAlign.Name = "iCenterTextAlign"
		'
		'iRightTextAlign
		'
		Me.iRightTextAlign.Caption = "Right"
		Me.iRightTextAlign.Id = 59
		Me.iRightTextAlign.ImageOptions.ImageIndex = 14
		Me.iRightTextAlign.Name = "iRightTextAlign"
		'
		'rgbiSkins
		'
		Me.rgbiSkins.Caption = "Skins"
		'
		'
		'
		Me.rgbiSkins.Gallery.AllowHoverImages = True
		Me.rgbiSkins.Gallery.Appearance.ItemCaptionAppearance.Normal.Options.UseFont = True
		Me.rgbiSkins.Gallery.Appearance.ItemCaptionAppearance.Normal.Options.UseTextOptions = True
		Me.rgbiSkins.Gallery.Appearance.ItemCaptionAppearance.Normal.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
		Me.rgbiSkins.Gallery.ColumnCount = 4
		Me.rgbiSkins.Gallery.FixedHoverImageSize = False
		Me.rgbiSkins.Gallery.ImageSize = New System.Drawing.Size(32, 17)
		Me.rgbiSkins.Gallery.ItemImageLocation = DevExpress.Utils.Locations.Top
		Me.rgbiSkins.Gallery.RowCount = 4
		Me.rgbiSkins.Id = 60
		Me.rgbiSkins.Name = "rgbiSkins"
		'
		'FileNewItem1
		'
		Me.FileNewItem1.Id = 62
		Me.FileNewItem1.ImageOptions.Image = CType(resources.GetObject("FileNewItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.FileNewItem1.ImageOptions.LargeImage = CType(resources.GetObject("FileNewItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.FileNewItem1.Name = "FileNewItem1"
		'
		'FileOpenItem1
		'
		Me.FileOpenItem1.Id = 63
		Me.FileOpenItem1.ImageOptions.Image = CType(resources.GetObject("FileOpenItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.FileOpenItem1.ImageOptions.LargeImage = CType(resources.GetObject("FileOpenItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.FileOpenItem1.Name = "FileOpenItem1"
		'
		'FileSaveItem1
		'
		Me.FileSaveItem1.Caption = "Daten sichern"
		Me.FileSaveItem1.Id = 64
		Me.FileSaveItem1.ImageOptions.Image = CType(resources.GetObject("FileSaveItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.FileSaveItem1.ImageOptions.LargeImage = CType(resources.GetObject("FileSaveItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.FileSaveItem1.Name = "FileSaveItem1"
		Me.FileSaveItem1.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'FileSaveAsItem1
		'
		Me.FileSaveAsItem1.Id = 65
		Me.FileSaveAsItem1.ImageOptions.Image = CType(resources.GetObject("FileSaveAsItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.FileSaveAsItem1.ImageOptions.LargeImage = CType(resources.GetObject("FileSaveAsItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.FileSaveAsItem1.Name = "FileSaveAsItem1"
		'
		'QuickPrintItem1
		'
		Me.QuickPrintItem1.Id = 66
		Me.QuickPrintItem1.ImageOptions.Image = CType(resources.GetObject("QuickPrintItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.QuickPrintItem1.ImageOptions.LargeImage = CType(resources.GetObject("QuickPrintItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.QuickPrintItem1.Name = "QuickPrintItem1"
		'
		'PrintItem1
		'
		Me.PrintItem1.Id = 67
		Me.PrintItem1.ImageOptions.Image = CType(resources.GetObject("PrintItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.PrintItem1.ImageOptions.LargeImage = CType(resources.GetObject("PrintItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.PrintItem1.Name = "PrintItem1"
		'
		'PrintPreviewItem1
		'
		Me.PrintPreviewItem1.Id = 68
		Me.PrintPreviewItem1.ImageOptions.Image = CType(resources.GetObject("PrintPreviewItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.PrintPreviewItem1.ImageOptions.LargeImage = CType(resources.GetObject("PrintPreviewItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.PrintPreviewItem1.Name = "PrintPreviewItem1"
		'
		'UndoItem1
		'
		Me.UndoItem1.Id = 69
		Me.UndoItem1.ImageOptions.Image = CType(resources.GetObject("UndoItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.UndoItem1.ImageOptions.LargeImage = CType(resources.GetObject("UndoItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.UndoItem1.Name = "UndoItem1"
		'
		'RedoItem1
		'
		Me.RedoItem1.Id = 70
		Me.RedoItem1.ImageOptions.Image = CType(resources.GetObject("RedoItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.RedoItem1.ImageOptions.LargeImage = CType(resources.GetObject("RedoItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.RedoItem1.Name = "RedoItem1"
		'
		'CutItem1
		'
		Me.CutItem1.Id = 71
		Me.CutItem1.ImageOptions.Image = CType(resources.GetObject("CutItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.CutItem1.ImageOptions.LargeImage = CType(resources.GetObject("CutItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.CutItem1.Name = "CutItem1"
		Me.CutItem1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText
		'
		'CopyItem1
		'
		Me.CopyItem1.Id = 72
		Me.CopyItem1.ImageOptions.Image = CType(resources.GetObject("CopyItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.CopyItem1.ImageOptions.LargeImage = CType(resources.GetObject("CopyItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.CopyItem1.Name = "CopyItem1"
		Me.CopyItem1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText
		'
		'PasteItem1
		'
		Me.PasteItem1.Id = 73
		Me.PasteItem1.ImageOptions.Image = CType(resources.GetObject("PasteItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.PasteItem1.ImageOptions.LargeImage = CType(resources.GetObject("PasteItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.PasteItem1.Name = "PasteItem1"
		'
		'PasteSpecialItem1
		'
		Me.PasteSpecialItem1.Id = 74
		Me.PasteSpecialItem1.ImageOptions.Image = CType(resources.GetObject("PasteSpecialItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.PasteSpecialItem1.ImageOptions.LargeImage = CType(resources.GetObject("PasteSpecialItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.PasteSpecialItem1.Name = "PasteSpecialItem1"
		Me.PasteSpecialItem1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText
		'
		'ChangeFontNameItem1
		'
		Me.ChangeFontNameItem1.Edit = Me.RepositoryItemFontEdit1
		Me.ChangeFontNameItem1.Id = 75
		Me.ChangeFontNameItem1.Name = "ChangeFontNameItem1"
		Me.ChangeFontNameItem1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText
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
		Me.ChangeFontSizeItem1.Id = 76
		Me.ChangeFontSizeItem1.Name = "ChangeFontSizeItem1"
		Me.ChangeFontSizeItem1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText
		'
		'RepositoryItemRichEditFontSizeEdit1
		'
		Me.RepositoryItemRichEditFontSizeEdit1.AutoHeight = False
		Me.RepositoryItemRichEditFontSizeEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.RepositoryItemRichEditFontSizeEdit1.Control = Me.rtfContent
		Me.RepositoryItemRichEditFontSizeEdit1.Name = "RepositoryItemRichEditFontSizeEdit1"
		'
		'ChangeFontColorItem1
		'
		Me.ChangeFontColorItem1.Id = 77
		Me.ChangeFontColorItem1.ImageOptions.Image = CType(resources.GetObject("ChangeFontColorItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.ChangeFontColorItem1.ImageOptions.LargeImage = CType(resources.GetObject("ChangeFontColorItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.ChangeFontColorItem1.Name = "ChangeFontColorItem1"
		Me.ChangeFontColorItem1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.[Default]
		'
		'ChangeFontBackColorItem1
		'
		Me.ChangeFontBackColorItem1.Id = 78
		Me.ChangeFontBackColorItem1.ImageOptions.Image = CType(resources.GetObject("ChangeFontBackColorItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.ChangeFontBackColorItem1.ImageOptions.LargeImage = CType(resources.GetObject("ChangeFontBackColorItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.ChangeFontBackColorItem1.Name = "ChangeFontBackColorItem1"
		Me.ChangeFontBackColorItem1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.[Default]
		'
		'ToggleFontBoldItem1
		'
		Me.ToggleFontBoldItem1.Id = 79
		Me.ToggleFontBoldItem1.ImageOptions.Image = CType(resources.GetObject("ToggleFontBoldItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.ToggleFontBoldItem1.ImageOptions.LargeImage = CType(resources.GetObject("ToggleFontBoldItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.ToggleFontBoldItem1.Name = "ToggleFontBoldItem1"
		Me.ToggleFontBoldItem1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.[Default]
		'
		'ToggleFontItalicItem1
		'
		Me.ToggleFontItalicItem1.Id = 80
		Me.ToggleFontItalicItem1.ImageOptions.Image = CType(resources.GetObject("ToggleFontItalicItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.ToggleFontItalicItem1.ImageOptions.LargeImage = CType(resources.GetObject("ToggleFontItalicItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.ToggleFontItalicItem1.Name = "ToggleFontItalicItem1"
		Me.ToggleFontItalicItem1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.[Default]
		'
		'ToggleFontUnderlineItem1
		'
		Me.ToggleFontUnderlineItem1.Id = 81
		Me.ToggleFontUnderlineItem1.ImageOptions.Image = CType(resources.GetObject("ToggleFontUnderlineItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.ToggleFontUnderlineItem1.ImageOptions.LargeImage = CType(resources.GetObject("ToggleFontUnderlineItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.ToggleFontUnderlineItem1.Name = "ToggleFontUnderlineItem1"
		Me.ToggleFontUnderlineItem1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.[Default]
		'
		'ToggleFontDoubleUnderlineItem1
		'
		Me.ToggleFontDoubleUnderlineItem1.Id = 82
		Me.ToggleFontDoubleUnderlineItem1.ImageOptions.Image = CType(resources.GetObject("ToggleFontDoubleUnderlineItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.ToggleFontDoubleUnderlineItem1.ImageOptions.LargeImage = CType(resources.GetObject("ToggleFontDoubleUnderlineItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.ToggleFontDoubleUnderlineItem1.Name = "ToggleFontDoubleUnderlineItem1"
		Me.ToggleFontDoubleUnderlineItem1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.[Default]
		'
		'ToggleFontStrikeoutItem1
		'
		Me.ToggleFontStrikeoutItem1.Id = 83
		Me.ToggleFontStrikeoutItem1.ImageOptions.Image = CType(resources.GetObject("ToggleFontStrikeoutItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.ToggleFontStrikeoutItem1.ImageOptions.LargeImage = CType(resources.GetObject("ToggleFontStrikeoutItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.ToggleFontStrikeoutItem1.Name = "ToggleFontStrikeoutItem1"
		Me.ToggleFontStrikeoutItem1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.[Default]
		'
		'ToggleFontDoubleStrikeoutItem1
		'
		Me.ToggleFontDoubleStrikeoutItem1.Id = 84
		Me.ToggleFontDoubleStrikeoutItem1.ImageOptions.Image = CType(resources.GetObject("ToggleFontDoubleStrikeoutItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.ToggleFontDoubleStrikeoutItem1.ImageOptions.LargeImage = CType(resources.GetObject("ToggleFontDoubleStrikeoutItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.ToggleFontDoubleStrikeoutItem1.Name = "ToggleFontDoubleStrikeoutItem1"
		Me.ToggleFontDoubleStrikeoutItem1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.[Default]
		'
		'ToggleFontSuperscriptItem1
		'
		Me.ToggleFontSuperscriptItem1.Id = 85
		Me.ToggleFontSuperscriptItem1.ImageOptions.Image = CType(resources.GetObject("ToggleFontSuperscriptItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.ToggleFontSuperscriptItem1.ImageOptions.LargeImage = CType(resources.GetObject("ToggleFontSuperscriptItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.ToggleFontSuperscriptItem1.Name = "ToggleFontSuperscriptItem1"
		Me.ToggleFontSuperscriptItem1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.[Default]
		'
		'ToggleFontSubscriptItem1
		'
		Me.ToggleFontSubscriptItem1.Id = 86
		Me.ToggleFontSubscriptItem1.ImageOptions.Image = CType(resources.GetObject("ToggleFontSubscriptItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.ToggleFontSubscriptItem1.ImageOptions.LargeImage = CType(resources.GetObject("ToggleFontSubscriptItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.ToggleFontSubscriptItem1.Name = "ToggleFontSubscriptItem1"
		Me.ToggleFontSubscriptItem1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.[Default]
		'
		'ChangeTextCaseItem1
		'
		Me.ChangeTextCaseItem1.Id = 87
		Me.ChangeTextCaseItem1.ImageOptions.Image = CType(resources.GetObject("ChangeTextCaseItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.ChangeTextCaseItem1.ImageOptions.LargeImage = CType(resources.GetObject("ChangeTextCaseItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.ChangeTextCaseItem1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.MakeTextUpperCaseItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.MakeTextLowerCaseItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.ToggleTextCaseItem1)})
		Me.ChangeTextCaseItem1.Name = "ChangeTextCaseItem1"
		Me.ChangeTextCaseItem1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.[Default]
		'
		'MakeTextUpperCaseItem1
		'
		Me.MakeTextUpperCaseItem1.Id = 88
		Me.MakeTextUpperCaseItem1.Name = "MakeTextUpperCaseItem1"
		'
		'MakeTextLowerCaseItem1
		'
		Me.MakeTextLowerCaseItem1.Id = 89
		Me.MakeTextLowerCaseItem1.Name = "MakeTextLowerCaseItem1"
		'
		'ToggleTextCaseItem1
		'
		Me.ToggleTextCaseItem1.Id = 90
		Me.ToggleTextCaseItem1.Name = "ToggleTextCaseItem1"
		'
		'FontSizeIncreaseItem1
		'
		Me.FontSizeIncreaseItem1.Id = 91
		Me.FontSizeIncreaseItem1.ImageOptions.Image = CType(resources.GetObject("FontSizeIncreaseItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.FontSizeIncreaseItem1.ImageOptions.LargeImage = CType(resources.GetObject("FontSizeIncreaseItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.FontSizeIncreaseItem1.Name = "FontSizeIncreaseItem1"
		Me.FontSizeIncreaseItem1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.[Default]
		'
		'FontSizeDecreaseItem1
		'
		Me.FontSizeDecreaseItem1.Id = 92
		Me.FontSizeDecreaseItem1.ImageOptions.Image = CType(resources.GetObject("FontSizeDecreaseItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.FontSizeDecreaseItem1.ImageOptions.LargeImage = CType(resources.GetObject("FontSizeDecreaseItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.FontSizeDecreaseItem1.Name = "FontSizeDecreaseItem1"
		Me.FontSizeDecreaseItem1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.[Default]
		'
		'ClearFormattingItem1
		'
		Me.ClearFormattingItem1.Id = 93
		Me.ClearFormattingItem1.ImageOptions.Image = CType(resources.GetObject("ClearFormattingItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.ClearFormattingItem1.ImageOptions.LargeImage = CType(resources.GetObject("ClearFormattingItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.ClearFormattingItem1.Name = "ClearFormattingItem1"
		Me.ClearFormattingItem1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.[Default]
		'
		'ShowFontFormItem1
		'
		Me.ShowFontFormItem1.Id = 94
		Me.ShowFontFormItem1.ImageOptions.Image = CType(resources.GetObject("ShowFontFormItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.ShowFontFormItem1.ImageOptions.LargeImage = CType(resources.GetObject("ShowFontFormItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.ShowFontFormItem1.Name = "ShowFontFormItem1"
		'
		'ToggleParagraphAlignmentLeftItem1
		'
		Me.ToggleParagraphAlignmentLeftItem1.Id = 95
		Me.ToggleParagraphAlignmentLeftItem1.ImageOptions.Image = CType(resources.GetObject("ToggleParagraphAlignmentLeftItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.ToggleParagraphAlignmentLeftItem1.ImageOptions.LargeImage = CType(resources.GetObject("ToggleParagraphAlignmentLeftItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.ToggleParagraphAlignmentLeftItem1.Name = "ToggleParagraphAlignmentLeftItem1"
		'
		'ToggleParagraphAlignmentCenterItem1
		'
		Me.ToggleParagraphAlignmentCenterItem1.Id = 96
		Me.ToggleParagraphAlignmentCenterItem1.ImageOptions.Image = CType(resources.GetObject("ToggleParagraphAlignmentCenterItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.ToggleParagraphAlignmentCenterItem1.ImageOptions.LargeImage = CType(resources.GetObject("ToggleParagraphAlignmentCenterItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.ToggleParagraphAlignmentCenterItem1.Name = "ToggleParagraphAlignmentCenterItem1"
		'
		'ToggleParagraphAlignmentRightItem1
		'
		Me.ToggleParagraphAlignmentRightItem1.Id = 97
		Me.ToggleParagraphAlignmentRightItem1.ImageOptions.Image = CType(resources.GetObject("ToggleParagraphAlignmentRightItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.ToggleParagraphAlignmentRightItem1.ImageOptions.LargeImage = CType(resources.GetObject("ToggleParagraphAlignmentRightItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.ToggleParagraphAlignmentRightItem1.Name = "ToggleParagraphAlignmentRightItem1"
		'
		'ToggleParagraphAlignmentJustifyItem1
		'
		Me.ToggleParagraphAlignmentJustifyItem1.Id = 98
		Me.ToggleParagraphAlignmentJustifyItem1.ImageOptions.Image = CType(resources.GetObject("ToggleParagraphAlignmentJustifyItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.ToggleParagraphAlignmentJustifyItem1.ImageOptions.LargeImage = CType(resources.GetObject("ToggleParagraphAlignmentJustifyItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.ToggleParagraphAlignmentJustifyItem1.Name = "ToggleParagraphAlignmentJustifyItem1"
		'
		'ChangeParagraphLineSpacingItem1
		'
		Me.ChangeParagraphLineSpacingItem1.Id = 99
		Me.ChangeParagraphLineSpacingItem1.ImageOptions.Image = CType(resources.GetObject("ChangeParagraphLineSpacingItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.ChangeParagraphLineSpacingItem1.ImageOptions.LargeImage = CType(resources.GetObject("ChangeParagraphLineSpacingItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.ChangeParagraphLineSpacingItem1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.SetSingleParagraphSpacingItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.SetSesquialteralParagraphSpacingItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.SetDoubleParagraphSpacingItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.ShowLineSpacingFormItem1), New DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.KeyTip, Me.AddSpacingBeforeParagraphItem1, "", False, True, True, 0, Nothing, DevExpress.XtraBars.BarItemPaintStyle.Standard, "B", ""), New DevExpress.XtraBars.LinkPersistInfo(Me.RemoveSpacingBeforeParagraphItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.AddSpacingAfterParagraphItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.RemoveSpacingAfterParagraphItem1)})
		Me.ChangeParagraphLineSpacingItem1.Name = "ChangeParagraphLineSpacingItem1"
		'
		'SetSingleParagraphSpacingItem1
		'
		Me.SetSingleParagraphSpacingItem1.Id = 100
		Me.SetSingleParagraphSpacingItem1.Name = "SetSingleParagraphSpacingItem1"
		'
		'SetSesquialteralParagraphSpacingItem1
		'
		Me.SetSesquialteralParagraphSpacingItem1.Id = 101
		Me.SetSesquialteralParagraphSpacingItem1.Name = "SetSesquialteralParagraphSpacingItem1"
		'
		'SetDoubleParagraphSpacingItem1
		'
		Me.SetDoubleParagraphSpacingItem1.Id = 102
		Me.SetDoubleParagraphSpacingItem1.Name = "SetDoubleParagraphSpacingItem1"
		'
		'ShowLineSpacingFormItem1
		'
		Me.ShowLineSpacingFormItem1.Id = 103
		Me.ShowLineSpacingFormItem1.Name = "ShowLineSpacingFormItem1"
		'
		'AddSpacingBeforeParagraphItem1
		'
		Me.AddSpacingBeforeParagraphItem1.Id = 104
		Me.AddSpacingBeforeParagraphItem1.Name = "AddSpacingBeforeParagraphItem1"
		'
		'RemoveSpacingBeforeParagraphItem1
		'
		Me.RemoveSpacingBeforeParagraphItem1.Id = 105
		Me.RemoveSpacingBeforeParagraphItem1.Name = "RemoveSpacingBeforeParagraphItem1"
		'
		'AddSpacingAfterParagraphItem1
		'
		Me.AddSpacingAfterParagraphItem1.Id = 106
		Me.AddSpacingAfterParagraphItem1.Name = "AddSpacingAfterParagraphItem1"
		'
		'RemoveSpacingAfterParagraphItem1
		'
		Me.RemoveSpacingAfterParagraphItem1.Id = 107
		Me.RemoveSpacingAfterParagraphItem1.Name = "RemoveSpacingAfterParagraphItem1"
		'
		'ToggleBulletedListItem1
		'
		Me.ToggleBulletedListItem1.Id = 108
		Me.ToggleBulletedListItem1.ImageOptions.Image = CType(resources.GetObject("ToggleBulletedListItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.ToggleBulletedListItem1.ImageOptions.LargeImage = CType(resources.GetObject("ToggleBulletedListItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.ToggleBulletedListItem1.Name = "ToggleBulletedListItem1"
		'
		'ToggleNumberingListItem1
		'
		Me.ToggleNumberingListItem1.Id = 109
		Me.ToggleNumberingListItem1.ImageOptions.Image = CType(resources.GetObject("ToggleNumberingListItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.ToggleNumberingListItem1.ImageOptions.LargeImage = CType(resources.GetObject("ToggleNumberingListItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.ToggleNumberingListItem1.Name = "ToggleNumberingListItem1"
		'
		'ToggleMultiLevelListItem1
		'
		Me.ToggleMultiLevelListItem1.Id = 110
		Me.ToggleMultiLevelListItem1.ImageOptions.Image = CType(resources.GetObject("ToggleMultiLevelListItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.ToggleMultiLevelListItem1.ImageOptions.LargeImage = CType(resources.GetObject("ToggleMultiLevelListItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.ToggleMultiLevelListItem1.Name = "ToggleMultiLevelListItem1"
		'
		'DecreaseIndentItem1
		'
		Me.DecreaseIndentItem1.Id = 111
		Me.DecreaseIndentItem1.ImageOptions.Image = CType(resources.GetObject("DecreaseIndentItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.DecreaseIndentItem1.ImageOptions.LargeImage = CType(resources.GetObject("DecreaseIndentItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.DecreaseIndentItem1.Name = "DecreaseIndentItem1"
		'
		'IncreaseIndentItem1
		'
		Me.IncreaseIndentItem1.Id = 112
		Me.IncreaseIndentItem1.ImageOptions.Image = CType(resources.GetObject("IncreaseIndentItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.IncreaseIndentItem1.ImageOptions.LargeImage = CType(resources.GetObject("IncreaseIndentItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.IncreaseIndentItem1.Name = "IncreaseIndentItem1"
		'
		'ToggleShowWhitespaceItem1
		'
		Me.ToggleShowWhitespaceItem1.Id = 113
		Me.ToggleShowWhitespaceItem1.ImageOptions.Image = CType(resources.GetObject("ToggleShowWhitespaceItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.ToggleShowWhitespaceItem1.ImageOptions.LargeImage = CType(resources.GetObject("ToggleShowWhitespaceItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.ToggleShowWhitespaceItem1.Name = "ToggleShowWhitespaceItem1"
		'
		'ShowParagraphFormItem1
		'
		Me.ShowParagraphFormItem1.Id = 114
		Me.ShowParagraphFormItem1.ImageOptions.Image = CType(resources.GetObject("ShowParagraphFormItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.ShowParagraphFormItem1.ImageOptions.LargeImage = CType(resources.GetObject("ShowParagraphFormItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.ShowParagraphFormItem1.Name = "ShowParagraphFormItem1"
		'
		'ChangeStyleItem1
		'
		Me.ChangeStyleItem1.Edit = Me.RepositoryItemRichEditStyleEdit1
		Me.ChangeStyleItem1.Id = 115
		Me.ChangeStyleItem1.Name = "ChangeStyleItem1"
		'
		'RepositoryItemRichEditStyleEdit1
		'
		Me.RepositoryItemRichEditStyleEdit1.AutoHeight = False
		Me.RepositoryItemRichEditStyleEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.RepositoryItemRichEditStyleEdit1.Control = Me.rtfContent
		Me.RepositoryItemRichEditStyleEdit1.Name = "RepositoryItemRichEditStyleEdit1"
		'
		'FindItem1
		'
		Me.FindItem1.Id = 116
		Me.FindItem1.ImageOptions.Image = CType(resources.GetObject("FindItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.FindItem1.ImageOptions.LargeImage = CType(resources.GetObject("FindItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.FindItem1.Name = "FindItem1"
		'
		'ReplaceItem1
		'
		Me.ReplaceItem1.Id = 117
		Me.ReplaceItem1.ImageOptions.Image = CType(resources.GetObject("ReplaceItem1.ImageOptions.Image"), System.Drawing.Image)
		Me.ReplaceItem1.ImageOptions.LargeImage = CType(resources.GetObject("ReplaceItem1.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.ReplaceItem1.Name = "ReplaceItem1"
		'
		'BarButtonGroup1
		'
		Me.BarButtonGroup1.Caption = "BarButtonGroup1"
		Me.BarButtonGroup1.Id = 118
		Me.BarButtonGroup1.ItemLinks.Add(Me.ChangeFontColorItem1, "FC")
		Me.BarButtonGroup1.ItemLinks.Add(Me.ChangeFontBackColorItem1, "I")
		Me.BarButtonGroup1.Name = "BarButtonGroup1"
		Me.BarButtonGroup1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText
		'
		'BarButtonGroup2
		'
		Me.BarButtonGroup2.Caption = "BarButtonGroup2"
		Me.BarButtonGroup2.Id = 119
		Me.BarButtonGroup2.ItemLinks.Add(Me.ToggleFontBoldItem1)
		Me.BarButtonGroup2.ItemLinks.Add(Me.ToggleFontItalicItem1)
		Me.BarButtonGroup2.ItemLinks.Add(Me.ToggleFontUnderlineItem1)
		Me.BarButtonGroup2.ItemLinks.Add(Me.ToggleFontDoubleUnderlineItem1)
		Me.BarButtonGroup2.ItemLinks.Add(Me.ToggleFontStrikeoutItem1)
		Me.BarButtonGroup2.ItemLinks.Add(Me.ToggleFontDoubleStrikeoutItem1)
		Me.BarButtonGroup2.ItemLinks.Add(Me.ToggleFontSubscriptItem1)
		Me.BarButtonGroup2.ItemLinks.Add(Me.ToggleFontSuperscriptItem1)
		Me.BarButtonGroup2.Name = "BarButtonGroup2"
		Me.BarButtonGroup2.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText
		'
		'BarButtonGroup3
		'
		Me.BarButtonGroup3.Caption = "BarButtonGroup3"
		Me.BarButtonGroup3.Id = 120
		Me.BarButtonGroup3.ItemLinks.Add(Me.FontSizeIncreaseItem1, "FG")
		Me.BarButtonGroup3.ItemLinks.Add(Me.FontSizeDecreaseItem1, "FK")
		Me.BarButtonGroup3.ItemLinks.Add(Me.ChangeTextCaseItem1)
		Me.BarButtonGroup3.ItemLinks.Add(Me.ClearFormattingItem1, "E")
		Me.BarButtonGroup3.Name = "BarButtonGroup3"
		Me.BarButtonGroup3.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText
		'
		'BarButtonGroup4
		'
		Me.BarButtonGroup4.Caption = "BarButtonGroup4"
		Me.BarButtonGroup4.Id = 121
		Me.BarButtonGroup4.ItemLinks.Add(Me.ToggleBulletedListItem1, "U")
		Me.BarButtonGroup4.ItemLinks.Add(Me.ToggleNumberingListItem1, "N")
		Me.BarButtonGroup4.ItemLinks.Add(Me.ToggleMultiLevelListItem1, "M")
		Me.BarButtonGroup4.ItemLinks.Add(Me.ChangeParagraphLineSpacingItem1, "K")
		Me.BarButtonGroup4.ItemLinks.Add(Me.IncreaseIndentItem1, "AI")
		Me.BarButtonGroup4.ItemLinks.Add(Me.DecreaseIndentItem1, "AO")
		Me.BarButtonGroup4.Name = "BarButtonGroup4"
		'
		'BarButtonGroup5
		'
		Me.BarButtonGroup5.Caption = "BarButtonGroup5"
		Me.BarButtonGroup5.Id = 122
		Me.BarButtonGroup5.ItemLinks.Add(Me.ToggleParagraphAlignmentLeftItem1, "AL")
		Me.BarButtonGroup5.ItemLinks.Add(Me.ToggleParagraphAlignmentCenterItem1, "AC")
		Me.BarButtonGroup5.ItemLinks.Add(Me.ToggleParagraphAlignmentRightItem1, "AR")
		Me.BarButtonGroup5.ItemLinks.Add(Me.ToggleParagraphAlignmentJustifyItem1, "AJ")
		Me.BarButtonGroup5.Name = "BarButtonGroup5"
		Me.BarButtonGroup5.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText
		'
		'BarButtonGroup6
		'
		Me.BarButtonGroup6.Caption = "BarButtonGroup6"
		Me.BarButtonGroup6.Id = 123
		Me.BarButtonGroup6.ItemLinks.Add(Me.ToggleShowWhitespaceItem1)
		Me.BarButtonGroup6.Name = "BarButtonGroup6"
		Me.BarButtonGroup6.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText
		'
		'BarButtonGroup7
		'
		Me.BarButtonGroup7.Caption = "BarButtonGroup7"
		Me.BarButtonGroup7.Id = 124
		Me.BarButtonGroup7.ItemLinks.Add(Me.ShowFontFormItem1)
		Me.BarButtonGroup7.Name = "BarButtonGroup7"
		'
		'BarButtonGroup8
		'
		Me.BarButtonGroup8.Caption = "BarButtonGroup8"
		Me.BarButtonGroup8.Id = 125
		Me.BarButtonGroup8.ItemLinks.Add(Me.ShowParagraphFormItem1)
		Me.BarButtonGroup8.Name = "BarButtonGroup8"
		'
		'BarButtonGroup9
		'
		Me.BarButtonGroup9.Caption = "BarButtonGroup9"
		Me.BarButtonGroup9.Id = 127
		Me.BarButtonGroup9.Name = "BarButtonGroup9"
		'
		'bbgSpeichern
		'
		Me.bbgSpeichern.Caption = "BarButtonGroup10"
		Me.bbgSpeichern.Id = 130
		Me.bbgSpeichern.ItemLinks.Add(Me.FileSaveItem1, "S")
		Me.bbgSpeichern.Name = "bbgSpeichern"
		'
		'RibbonGalleryBarItem1
		'
		Me.RibbonGalleryBarItem1.Caption = "RibbonGalleryBarItem1"
		Me.RibbonGalleryBarItem1.Id = 131
		Me.RibbonGalleryBarItem1.Name = "RibbonGalleryBarItem1"
		'
		'bei_Template
		'
		Me.bei_Template.Caption = "Vorlage"
		Me.bei_Template.Edit = Me.rep_lueTemplate
		Me.bei_Template.EditWidth = 200
		Me.bei_Template.Id = 133
		Me.bei_Template.Name = "bei_Template"
		'
		'rep_lueTemplate
		'
		Me.rep_lueTemplate.AutoHeight = False
		Me.rep_lueTemplate.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.rep_lueTemplate.Name = "rep_lueTemplate"
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
		'HomeRibbonPage1
		'
		Me.HomeRibbonPage1.Groups.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageGroup() {Me.frpZwischenablage, Me.frpSchriftart, Me.frpAbsatz, Me.frpBearbeiten})
		Me.HomeRibbonPage1.Name = "HomeRibbonPage1"
		'
		'frpZwischenablage
		'
		Me.frpZwischenablage.ItemLinks.Add(Me.PasteItem1, "V")
		Me.frpZwischenablage.ItemLinks.Add(Me.CutItem1, "X")
		Me.frpZwischenablage.ItemLinks.Add(Me.CopyItem1, "C")
		Me.frpZwischenablage.ItemLinks.Add(Me.PasteSpecialItem1)
		Me.frpZwischenablage.Name = "frpZwischenablage"
		'
		'frpSchriftart
		'
		Me.frpSchriftart.ItemLinks.Add(Me.ChangeFontNameItem1, True, "FF", "", True)
		Me.frpSchriftart.ItemLinks.Add(Me.ChangeFontSizeItem1, False, "", "", True)
		Me.frpSchriftart.ItemLinks.Add(Me.BarButtonGroup3)
		Me.frpSchriftart.ItemLinks.Add(Me.BarButtonGroup2)
		Me.frpSchriftart.ItemLinks.Add(Me.BarButtonGroup1)
		Me.frpSchriftart.ItemLinks.Add(Me.BarButtonGroup7)
		Me.frpSchriftart.Name = "frpSchriftart"
		'
		'frpAbsatz
		'
		Me.frpAbsatz.ItemLinks.Add(Me.BarButtonGroup4)
		Me.frpAbsatz.ItemLinks.Add(Me.BarButtonGroup5)
		Me.frpAbsatz.ItemLinks.Add(Me.BarButtonGroup8)
		Me.frpAbsatz.ItemLinks.Add(Me.BarButtonGroup6)
		Me.frpAbsatz.Name = "frpAbsatz"
		'
		'frpBearbeiten
		'
		Me.frpBearbeiten.ItemLinks.Add(Me.FindItem1, "FD")
		Me.frpBearbeiten.ItemLinks.Add(Me.ReplaceItem1, "R")
		Me.frpBearbeiten.Name = "frpBearbeiten"
		'
		'ribbonStatusBar
		'
		Me.ribbonStatusBar.ItemLinks.Add(Me.siStatus)
		Me.ribbonStatusBar.ItemLinks.Add(Me.siInfo)
		Me.ribbonStatusBar.Location = New System.Drawing.Point(0, 563)
		Me.ribbonStatusBar.Name = "ribbonStatusBar"
		Me.ribbonStatusBar.Ribbon = Me.ribbonControl
		Me.ribbonStatusBar.Size = New System.Drawing.Size(858, 27)
		'
		'spellChecker
		'
		Me.spellChecker.Culture = New System.Globalization.CultureInfo("ru-RU")
		Me.spellChecker.ParentContainer = Nothing
		'
		'popupControlContainer2
		'
		Me.popupControlContainer2.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.popupControlContainer2.Appearance.Options.UseBackColor = True
		Me.popupControlContainer2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
		Me.popupControlContainer2.Controls.Add(Me.buttonEdit)
		Me.popupControlContainer2.Location = New System.Drawing.Point(238, 289)
		Me.popupControlContainer2.Name = "popupControlContainer2"
		Me.popupControlContainer2.Ribbon = Me.ribbonControl
		Me.popupControlContainer2.Size = New System.Drawing.Size(118, 28)
		Me.popupControlContainer2.TabIndex = 7
		Me.popupControlContainer2.Visible = False
		'
		'buttonEdit
		'
		Me.buttonEdit.EditValue = "Some Text"
		Me.buttonEdit.Location = New System.Drawing.Point(3, 5)
		Me.buttonEdit.MenuManager = Me.ribbonControl
		Me.buttonEdit.Name = "buttonEdit"
		Me.buttonEdit.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.buttonEdit.Size = New System.Drawing.Size(100, 20)
		Me.buttonEdit.TabIndex = 0
		'
		'popupControlContainer1
		'
		Me.popupControlContainer1.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.popupControlContainer1.Appearance.Options.UseBackColor = True
		Me.popupControlContainer1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
		Me.popupControlContainer1.Controls.Add(Me.someLabelControl2)
		Me.popupControlContainer1.Controls.Add(Me.someLabelControl1)
		Me.popupControlContainer1.Location = New System.Drawing.Point(111, 197)
		Me.popupControlContainer1.Name = "popupControlContainer1"
		Me.popupControlContainer1.Ribbon = Me.ribbonControl
		Me.popupControlContainer1.Size = New System.Drawing.Size(76, 70)
		Me.popupControlContainer1.TabIndex = 6
		Me.popupControlContainer1.Visible = False
		'
		'someLabelControl2
		'
		Me.someLabelControl2.Location = New System.Drawing.Point(3, 57)
		Me.someLabelControl2.Name = "someLabelControl2"
		Me.someLabelControl2.Size = New System.Drawing.Size(49, 13)
		Me.someLabelControl2.TabIndex = 0
		Me.someLabelControl2.Text = "Some Info"
		'
		'someLabelControl1
		'
		Me.someLabelControl1.Location = New System.Drawing.Point(3, 3)
		Me.someLabelControl1.Name = "someLabelControl1"
		Me.someLabelControl1.Size = New System.Drawing.Size(49, 13)
		Me.someLabelControl1.TabIndex = 0
		Me.someLabelControl1.Text = "Some Info"
		'
		'RichEditBarController1
		'
		Me.RichEditBarController1.BarItems.Add(Me.FileNewItem1)
		Me.RichEditBarController1.BarItems.Add(Me.FileOpenItem1)
		Me.RichEditBarController1.BarItems.Add(Me.FileSaveItem1)
		Me.RichEditBarController1.BarItems.Add(Me.FileSaveAsItem1)
		Me.RichEditBarController1.BarItems.Add(Me.QuickPrintItem1)
		Me.RichEditBarController1.BarItems.Add(Me.PrintItem1)
		Me.RichEditBarController1.BarItems.Add(Me.PrintPreviewItem1)
		Me.RichEditBarController1.BarItems.Add(Me.UndoItem1)
		Me.RichEditBarController1.BarItems.Add(Me.RedoItem1)
		Me.RichEditBarController1.BarItems.Add(Me.CutItem1)
		Me.RichEditBarController1.BarItems.Add(Me.CopyItem1)
		Me.RichEditBarController1.BarItems.Add(Me.PasteItem1)
		Me.RichEditBarController1.BarItems.Add(Me.PasteSpecialItem1)
		Me.RichEditBarController1.BarItems.Add(Me.ChangeFontNameItem1)
		Me.RichEditBarController1.BarItems.Add(Me.ChangeFontSizeItem1)
		Me.RichEditBarController1.BarItems.Add(Me.ChangeFontColorItem1)
		Me.RichEditBarController1.BarItems.Add(Me.ChangeFontBackColorItem1)
		Me.RichEditBarController1.BarItems.Add(Me.ToggleFontBoldItem1)
		Me.RichEditBarController1.BarItems.Add(Me.ToggleFontItalicItem1)
		Me.RichEditBarController1.BarItems.Add(Me.ToggleFontUnderlineItem1)
		Me.RichEditBarController1.BarItems.Add(Me.ToggleFontDoubleUnderlineItem1)
		Me.RichEditBarController1.BarItems.Add(Me.ToggleFontStrikeoutItem1)
		Me.RichEditBarController1.BarItems.Add(Me.ToggleFontDoubleStrikeoutItem1)
		Me.RichEditBarController1.BarItems.Add(Me.ToggleFontSuperscriptItem1)
		Me.RichEditBarController1.BarItems.Add(Me.ToggleFontSubscriptItem1)
		Me.RichEditBarController1.BarItems.Add(Me.ChangeTextCaseItem1)
		Me.RichEditBarController1.BarItems.Add(Me.MakeTextUpperCaseItem1)
		Me.RichEditBarController1.BarItems.Add(Me.MakeTextLowerCaseItem1)
		Me.RichEditBarController1.BarItems.Add(Me.ToggleTextCaseItem1)
		Me.RichEditBarController1.BarItems.Add(Me.FontSizeIncreaseItem1)
		Me.RichEditBarController1.BarItems.Add(Me.FontSizeDecreaseItem1)
		Me.RichEditBarController1.BarItems.Add(Me.ClearFormattingItem1)
		Me.RichEditBarController1.BarItems.Add(Me.ShowFontFormItem1)
		Me.RichEditBarController1.BarItems.Add(Me.ToggleParagraphAlignmentLeftItem1)
		Me.RichEditBarController1.BarItems.Add(Me.ToggleParagraphAlignmentCenterItem1)
		Me.RichEditBarController1.BarItems.Add(Me.ToggleParagraphAlignmentRightItem1)
		Me.RichEditBarController1.BarItems.Add(Me.ToggleParagraphAlignmentJustifyItem1)
		Me.RichEditBarController1.BarItems.Add(Me.ChangeParagraphLineSpacingItem1)
		Me.RichEditBarController1.BarItems.Add(Me.SetSingleParagraphSpacingItem1)
		Me.RichEditBarController1.BarItems.Add(Me.SetSesquialteralParagraphSpacingItem1)
		Me.RichEditBarController1.BarItems.Add(Me.SetDoubleParagraphSpacingItem1)
		Me.RichEditBarController1.BarItems.Add(Me.ShowLineSpacingFormItem1)
		Me.RichEditBarController1.BarItems.Add(Me.AddSpacingBeforeParagraphItem1)
		Me.RichEditBarController1.BarItems.Add(Me.RemoveSpacingBeforeParagraphItem1)
		Me.RichEditBarController1.BarItems.Add(Me.AddSpacingAfterParagraphItem1)
		Me.RichEditBarController1.BarItems.Add(Me.RemoveSpacingAfterParagraphItem1)
		Me.RichEditBarController1.BarItems.Add(Me.ToggleBulletedListItem1)
		Me.RichEditBarController1.BarItems.Add(Me.ToggleNumberingListItem1)
		Me.RichEditBarController1.BarItems.Add(Me.ToggleMultiLevelListItem1)
		Me.RichEditBarController1.BarItems.Add(Me.DecreaseIndentItem1)
		Me.RichEditBarController1.BarItems.Add(Me.IncreaseIndentItem1)
		Me.RichEditBarController1.BarItems.Add(Me.ToggleShowWhitespaceItem1)
		Me.RichEditBarController1.BarItems.Add(Me.ShowParagraphFormItem1)
		Me.RichEditBarController1.BarItems.Add(Me.ChangeStyleItem1)
		Me.RichEditBarController1.BarItems.Add(Me.FindItem1)
		Me.RichEditBarController1.BarItems.Add(Me.ReplaceItem1)
		Me.RichEditBarController1.Control = Me.rtfContent
		'
		'frmMAMassenversand
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(858, 590)
		Me.Controls.Add(Me.splitContainerControl)
		Me.Controls.Add(Me.ribbonControl)
		Me.Controls.Add(Me.popupControlContainer1)
		Me.Controls.Add(Me.popupControlContainer2)
		Me.Controls.Add(Me.ribbonStatusBar)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.Name = "frmMAMassenversand"
		Me.Text = "Form1"
		CType(Me.splitContainerControl, System.ComponentModel.ISupportInitialize).EndInit()
		Me.splitContainerControl.ResumeLayout(False)
		CType(Me.navBarControl, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.ribbonControl, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.ribbonImageCollection, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.RepositoryItemFontEdit1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.RepositoryItemRichEditFontSizeEdit1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.RepositoryItemRichEditStyleEdit1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.rep_lueTemplate, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.ribbonImageCollectionLarge, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.popupControlContainer2, System.ComponentModel.ISupportInitialize).EndInit()
		Me.popupControlContainer2.ResumeLayout(False)
		CType(Me.buttonEdit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.popupControlContainer1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.popupControlContainer1.ResumeLayout(False)
		Me.popupControlContainer1.PerformLayout()
		CType(Me.RichEditBarController1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Private WithEvents splitContainerControl As DevExpress.XtraEditors.SplitContainerControl
	Private WithEvents ribbonControl As DevExpress.XtraBars.Ribbon.RibbonControl
	Private WithEvents siStatus As DevExpress.XtraBars.BarStaticItem
	Private WithEvents siInfo As DevExpress.XtraBars.BarStaticItem
	Private WithEvents iNew As DevExpress.XtraBars.BarButtonItem
	Private WithEvents iOpen As DevExpress.XtraBars.BarButtonItem
	Private WithEvents iClose As DevExpress.XtraBars.BarButtonItem
	Private WithEvents iFind As DevExpress.XtraBars.BarButtonItem
	Private WithEvents iSaveAs As DevExpress.XtraBars.BarButtonItem
	Private WithEvents alignButtonGroup As DevExpress.XtraBars.BarButtonGroup
	Private WithEvents iBoldFontStyle As DevExpress.XtraBars.BarButtonItem
	Private WithEvents iItalicFontStyle As DevExpress.XtraBars.BarButtonItem
	Private WithEvents iUnderlinedFontStyle As DevExpress.XtraBars.BarButtonItem
	Private WithEvents fontStyleButtonGroup As DevExpress.XtraBars.BarButtonGroup
	Private WithEvents iLeftTextAlign As DevExpress.XtraBars.BarButtonItem
	Private WithEvents iCenterTextAlign As DevExpress.XtraBars.BarButtonItem
	Private WithEvents iRightTextAlign As DevExpress.XtraBars.BarButtonItem
	Private WithEvents rgbiSkins As DevExpress.XtraBars.RibbonGalleryBarItem
	Private WithEvents iExit As DevExpress.XtraBars.BarButtonItem
	Private WithEvents iHelp As DevExpress.XtraBars.BarButtonItem
	Private WithEvents iAbout As DevExpress.XtraBars.BarButtonItem
	Private WithEvents popupControlContainer1 As DevExpress.XtraBars.PopupControlContainer
	Private WithEvents someLabelControl2 As DevExpress.XtraEditors.LabelControl
	Private WithEvents someLabelControl1 As DevExpress.XtraEditors.LabelControl
	Private WithEvents popupControlContainer2 As DevExpress.XtraBars.PopupControlContainer
	Private WithEvents buttonEdit As DevExpress.XtraEditors.ButtonEdit
	Private WithEvents ribbonStatusBar As DevExpress.XtraBars.Ribbon.RibbonStatusBar
	Private WithEvents ribbonImageCollection As DevExpress.Utils.ImageCollection
	Private WithEvents ribbonImageCollectionLarge As DevExpress.Utils.ImageCollection
	Private WithEvents navBarControl As DevExpress.XtraNavBar.NavBarControl
	Private WithEvents inboxItem As DevExpress.XtraNavBar.NavBarItem
	Private WithEvents outboxItem As DevExpress.XtraNavBar.NavBarItem
	Private WithEvents draftsItem As DevExpress.XtraNavBar.NavBarItem
	Private WithEvents trashItem As DevExpress.XtraNavBar.NavBarItem
	Private WithEvents calendarItem As DevExpress.XtraNavBar.NavBarItem
	Private WithEvents tasksItem As DevExpress.XtraNavBar.NavBarItem
	Private WithEvents navbarImageList As System.Windows.Forms.ImageList
	Private WithEvents navbarImageListLarge As System.Windows.Forms.ImageList
	Private WithEvents rtfContent As DevExpress.XtraRichEdit.RichEditControl
	Private WithEvents spellChecker As DevExpress.XtraSpellChecker.SpellChecker
	Friend WithEvents FileNewItem1 As DevExpress.XtraRichEdit.UI.FileNewItem
	Friend WithEvents FileOpenItem1 As DevExpress.XtraRichEdit.UI.FileOpenItem
	Friend WithEvents FileSaveItem1 As DevExpress.XtraRichEdit.UI.FileSaveItem
	Friend WithEvents FileSaveAsItem1 As DevExpress.XtraRichEdit.UI.FileSaveAsItem
	Friend WithEvents QuickPrintItem1 As DevExpress.XtraRichEdit.UI.QuickPrintItem
	Friend WithEvents PrintItem1 As DevExpress.XtraRichEdit.UI.PrintItem
	Friend WithEvents PrintPreviewItem1 As DevExpress.XtraRichEdit.UI.PrintPreviewItem
	Friend WithEvents UndoItem1 As DevExpress.XtraRichEdit.UI.UndoItem
	Friend WithEvents RedoItem1 As DevExpress.XtraRichEdit.UI.RedoItem
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
	Friend WithEvents RepositoryItemRichEditStyleEdit1 As DevExpress.XtraRichEdit.Design.RepositoryItemRichEditStyleEdit
	Friend WithEvents FindItem1 As DevExpress.XtraRichEdit.UI.FindItem
	Friend WithEvents ReplaceItem1 As DevExpress.XtraRichEdit.UI.ReplaceItem
	Friend WithEvents RichEditBarController1 As DevExpress.XtraRichEdit.UI.RichEditBarController
	Friend WithEvents BarButtonGroup1 As DevExpress.XtraBars.BarButtonGroup
	Friend WithEvents HomeRibbonPage1 As DevExpress.XtraRichEdit.UI.HomeRibbonPage
	Friend WithEvents frpZwischenablage As DevExpress.XtraRichEdit.UI.ClipboardRibbonPageGroup
	Friend WithEvents frpSchriftart As DevExpress.XtraRichEdit.UI.FontRibbonPageGroup
	Friend WithEvents frpAbsatz As DevExpress.XtraRichEdit.UI.ParagraphRibbonPageGroup
	Friend WithEvents frpBearbeiten As DevExpress.XtraRichEdit.UI.EditingRibbonPageGroup
	Friend WithEvents BarButtonGroup2 As DevExpress.XtraBars.BarButtonGroup
	Friend WithEvents BarButtonGroup3 As DevExpress.XtraBars.BarButtonGroup
	Friend WithEvents BarButtonGroup4 As DevExpress.XtraBars.BarButtonGroup
	Friend WithEvents BarButtonGroup5 As DevExpress.XtraBars.BarButtonGroup
	Friend WithEvents BarButtonGroup6 As DevExpress.XtraBars.BarButtonGroup
	Friend WithEvents BarButtonGroup7 As DevExpress.XtraBars.BarButtonGroup
	Friend WithEvents BarButtonGroup8 As DevExpress.XtraBars.BarButtonGroup
	Friend WithEvents BarButtonGroup9 As DevExpress.XtraBars.BarButtonGroup
	Friend WithEvents bbgSpeichern As DevExpress.XtraBars.BarButtonGroup
	Friend WithEvents RibbonGalleryBarItem1 As DevExpress.XtraBars.RibbonGalleryBarItem
	Friend WithEvents bei_Template As DevExpress.XtraBars.BarEditItem
	Friend WithEvents rep_lueTemplate As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
End Class
