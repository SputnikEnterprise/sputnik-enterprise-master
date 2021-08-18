<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucHtmlEditor
  Inherits System.Windows.Forms.UserControl

  'UserControl overrides dispose to clean up the component list.
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ucHtmlEditor))
		Dim DictionaryFileInfo1 As SpiceLogic.HtmlEditorControl.Domain.DesignTime.DictionaryFileInfo = New SpiceLogic.HtmlEditorControl.Domain.DesignTime.DictionaryFileInfo()
		Me.ctrl1 = New SpiceLogic.WinHTMLEditor.WinForm.WinFormHtmlEditor()
		Me.CustomFooter = New System.Windows.Forms.ToolStrip()
		Me.CustomHeader = New System.Windows.Forms.ToolStrip()
		Me.ctrl1.Toolbar1.SuspendLayout()
		Me.ctrl1.Toolbar2.SuspendLayout()
		Me.ctrl1.SuspendLayout()
		Me.SuspendLayout()
		'
		'ctrl1
		'
		Me.ctrl1.AutoScrollMargin = New System.Drawing.Size(0, 0)
		Me.ctrl1.AutoScrollMinSize = New System.Drawing.Size(0, 0)
		Me.ctrl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		'
		'ctrl1.BtnAlignCenter
		'
		Me.ctrl1.BtnAlignCenter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ctrl1.BtnAlignCenter.Image = CType(resources.GetObject("ctrl1.BtnAlignCenter.Image"), System.Drawing.Image)
		Me.ctrl1.BtnAlignCenter.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me.ctrl1.BtnAlignCenter.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ctrl1.BtnAlignCenter.Name = "_factoryBtnAlignCenter"
		Me.ctrl1.BtnAlignCenter.Size = New System.Drawing.Size(26, 26)
		Me.ctrl1.BtnAlignCenter.Text = "Align Centre"
		'
		'ctrl1.BtnAlignLeft
		'
		Me.ctrl1.BtnAlignLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ctrl1.BtnAlignLeft.Image = CType(resources.GetObject("ctrl1.BtnAlignLeft.Image"), System.Drawing.Image)
		Me.ctrl1.BtnAlignLeft.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me.ctrl1.BtnAlignLeft.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ctrl1.BtnAlignLeft.Name = "_factoryBtnAlignLeft"
		Me.ctrl1.BtnAlignLeft.Size = New System.Drawing.Size(26, 26)
		Me.ctrl1.BtnAlignLeft.Text = "Align Left"
		'
		'ctrl1.BtnAlignRight
		'
		Me.ctrl1.BtnAlignRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ctrl1.BtnAlignRight.Image = CType(resources.GetObject("ctrl1.BtnAlignRight.Image"), System.Drawing.Image)
		Me.ctrl1.BtnAlignRight.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me.ctrl1.BtnAlignRight.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ctrl1.BtnAlignRight.Name = "_factoryBtnAlignRight"
		Me.ctrl1.BtnAlignRight.Size = New System.Drawing.Size(26, 26)
		Me.ctrl1.BtnAlignRight.Text = "Align Right"
		'
		'ctrl1.BtnBodyStyle
		'
		Me.ctrl1.BtnBodyStyle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ctrl1.BtnBodyStyle.Image = CType(resources.GetObject("ctrl1.BtnBodyStyle.Image"), System.Drawing.Image)
		Me.ctrl1.BtnBodyStyle.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me.ctrl1.BtnBodyStyle.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ctrl1.BtnBodyStyle.Name = "_factoryBtnBodyStyle"
		Me.ctrl1.BtnBodyStyle.Size = New System.Drawing.Size(27, 15)
		Me.ctrl1.BtnBodyStyle.Text = "Document Style "
		'
		'ctrl1.BtnBold
		'
		Me.ctrl1.BtnBold.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ctrl1.BtnBold.Image = CType(resources.GetObject("ctrl1.BtnBold.Image"), System.Drawing.Image)
		Me.ctrl1.BtnBold.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me.ctrl1.BtnBold.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ctrl1.BtnBold.Name = "_factoryBtnBold"
		Me.ctrl1.BtnBold.Size = New System.Drawing.Size(23, 20)
		Me.ctrl1.BtnBold.Text = "Bold"
		'
		'ctrl1.BtnCopy
		'
		Me.ctrl1.BtnCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ctrl1.BtnCopy.Image = CType(resources.GetObject("ctrl1.BtnCopy.Image"), System.Drawing.Image)
		Me.ctrl1.BtnCopy.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me.ctrl1.BtnCopy.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ctrl1.BtnCopy.Name = "_factoryBtnCopy"
		Me.ctrl1.BtnCopy.Size = New System.Drawing.Size(23, 26)
		Me.ctrl1.BtnCopy.Text = "Copy"
		'
		'ctrl1.BtnCut
		'
		Me.ctrl1.BtnCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ctrl1.BtnCut.Image = CType(resources.GetObject("ctrl1.BtnCut.Image"), System.Drawing.Image)
		Me.ctrl1.BtnCut.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me.ctrl1.BtnCut.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ctrl1.BtnCut.Name = "_factoryBtnCut"
		Me.ctrl1.BtnCut.Size = New System.Drawing.Size(23, 26)
		Me.ctrl1.BtnCut.Text = "Cut"
		'
		'ctrl1.BtnFontColor
		'
		Me.ctrl1.BtnFontColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ctrl1.BtnFontColor.Image = CType(resources.GetObject("ctrl1.BtnFontColor.Image"), System.Drawing.Image)
		Me.ctrl1.BtnFontColor.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me.ctrl1.BtnFontColor.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ctrl1.BtnFontColor.Name = "_factoryBtnFontColor"
		Me.ctrl1.BtnFontColor.Size = New System.Drawing.Size(23, 26)
		Me.ctrl1.BtnFontColor.Text = "Apply Font Color"
		'
		'ctrl1.BtnFormatRedo
		'
		Me.ctrl1.BtnFormatRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ctrl1.BtnFormatRedo.Image = CType(resources.GetObject("ctrl1.BtnFormatRedo.Image"), System.Drawing.Image)
		Me.ctrl1.BtnFormatRedo.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ctrl1.BtnFormatRedo.Name = "_factoryBtnRedo"
		Me.ctrl1.BtnFormatRedo.Size = New System.Drawing.Size(23, 20)
		Me.ctrl1.BtnFormatRedo.Text = "Redo"
		'
		'ctrl1.BtnFormatReset
		'
		Me.ctrl1.BtnFormatReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ctrl1.BtnFormatReset.Image = CType(resources.GetObject("ctrl1.BtnFormatReset.Image"), System.Drawing.Image)
		Me.ctrl1.BtnFormatReset.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me.ctrl1.BtnFormatReset.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ctrl1.BtnFormatReset.Name = "_factoryBtnFormatReset"
		Me.ctrl1.BtnFormatReset.Size = New System.Drawing.Size(34, 20)
		Me.ctrl1.BtnFormatReset.Text = "Remove Format"
		'
		'ctrl1.BtnFormatUndo
		'
		Me.ctrl1.BtnFormatUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ctrl1.BtnFormatUndo.Image = CType(resources.GetObject("ctrl1.BtnFormatUndo.Image"), System.Drawing.Image)
		Me.ctrl1.BtnFormatUndo.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ctrl1.BtnFormatUndo.Name = "_factoryBtnUndo"
		Me.ctrl1.BtnFormatUndo.Size = New System.Drawing.Size(23, 20)
		Me.ctrl1.BtnFormatUndo.Text = "Undo"
		'
		'ctrl1.BtnHighlightColor
		'
		Me.ctrl1.BtnHighlightColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ctrl1.BtnHighlightColor.Image = CType(resources.GetObject("ctrl1.BtnHighlightColor.Image"), System.Drawing.Image)
		Me.ctrl1.BtnHighlightColor.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me.ctrl1.BtnHighlightColor.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ctrl1.BtnHighlightColor.Name = "_factoryBtnHighlightColor"
		Me.ctrl1.BtnHighlightColor.Size = New System.Drawing.Size(27, 26)
		Me.ctrl1.BtnHighlightColor.Text = "Apply Highlight Color"
		'
		'ctrl1.BtnHorizontalRule
		'
		Me.ctrl1.BtnHorizontalRule.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ctrl1.BtnHorizontalRule.Image = CType(resources.GetObject("ctrl1.BtnHorizontalRule.Image"), System.Drawing.Image)
		Me.ctrl1.BtnHorizontalRule.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me.ctrl1.BtnHorizontalRule.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ctrl1.BtnHorizontalRule.Name = "_factoryBtnHorizontalRule"
		Me.ctrl1.BtnHorizontalRule.Size = New System.Drawing.Size(24, 26)
		Me.ctrl1.BtnHorizontalRule.Text = "Insert Horizontal Rule"
		'
		'ctrl1.BtnHyperlink
		'
		Me.ctrl1.BtnHyperlink.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ctrl1.BtnHyperlink.Image = CType(resources.GetObject("ctrl1.BtnHyperlink.Image"), System.Drawing.Image)
		Me.ctrl1.BtnHyperlink.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me.ctrl1.BtnHyperlink.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ctrl1.BtnHyperlink.Name = "_factoryBtnHyperlink"
		Me.ctrl1.BtnHyperlink.Size = New System.Drawing.Size(23, 26)
		Me.ctrl1.BtnHyperlink.Text = "Hyperlink"
		'
		'ctrl1.BtnImage
		'
		Me.ctrl1.BtnImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ctrl1.BtnImage.Image = CType(resources.GetObject("ctrl1.BtnImage.Image"), System.Drawing.Image)
		Me.ctrl1.BtnImage.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me.ctrl1.BtnImage.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ctrl1.BtnImage.Name = "_factoryBtnImage"
		Me.ctrl1.BtnImage.Size = New System.Drawing.Size(23, 26)
		Me.ctrl1.BtnImage.Text = "Image"
		'
		'ctrl1.BtnIndent
		'
		Me.ctrl1.BtnIndent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ctrl1.BtnIndent.Image = CType(resources.GetObject("ctrl1.BtnIndent.Image"), System.Drawing.Image)
		Me.ctrl1.BtnIndent.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me.ctrl1.BtnIndent.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ctrl1.BtnIndent.Name = "_factoryBtnIndent"
		Me.ctrl1.BtnIndent.Size = New System.Drawing.Size(27, 26)
		Me.ctrl1.BtnIndent.Text = "Indent"
		'
		'ctrl1.BtnInsertYouTubeVideo
		'
		Me.ctrl1.BtnInsertYouTubeVideo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ctrl1.BtnInsertYouTubeVideo.Image = CType(resources.GetObject("ctrl1.BtnInsertYouTubeVideo.Image"), System.Drawing.Image)
		Me.ctrl1.BtnInsertYouTubeVideo.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me.ctrl1.BtnInsertYouTubeVideo.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ctrl1.BtnInsertYouTubeVideo.Name = "_factoryBtnInsertYouTubeVideo"
		Me.ctrl1.BtnInsertYouTubeVideo.Size = New System.Drawing.Size(23, 26)
		Me.ctrl1.BtnInsertYouTubeVideo.Text = "Insert YouTube Video"
		'
		'ctrl1.BtnItalic
		'
		Me.ctrl1.BtnItalic.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ctrl1.BtnItalic.Image = CType(resources.GetObject("ctrl1.BtnItalic.Image"), System.Drawing.Image)
		Me.ctrl1.BtnItalic.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me.ctrl1.BtnItalic.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ctrl1.BtnItalic.Name = "_factoryBtnItalic"
		Me.ctrl1.BtnItalic.Size = New System.Drawing.Size(23, 20)
		Me.ctrl1.BtnItalic.Text = "Italic"
		'
		'ctrl1.BtnNew
		'
		Me.ctrl1.BtnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ctrl1.BtnNew.Image = CType(resources.GetObject("ctrl1.BtnNew.Image"), System.Drawing.Image)
		Me.ctrl1.BtnNew.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me.ctrl1.BtnNew.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ctrl1.BtnNew.Name = "_factoryBtnNew"
		Me.ctrl1.BtnNew.Size = New System.Drawing.Size(23, 26)
		Me.ctrl1.BtnNew.Text = "New"
		'
		'ctrl1.BtnOpen
		'
		Me.ctrl1.BtnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ctrl1.BtnOpen.Image = CType(resources.GetObject("ctrl1.BtnOpen.Image"), System.Drawing.Image)
		Me.ctrl1.BtnOpen.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me.ctrl1.BtnOpen.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ctrl1.BtnOpen.Name = "_factoryBtnOpen"
		Me.ctrl1.BtnOpen.Size = New System.Drawing.Size(23, 26)
		Me.ctrl1.BtnOpen.Text = "Open"
		'
		'ctrl1.BtnOrderedList
		'
		Me.ctrl1.BtnOrderedList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ctrl1.BtnOrderedList.Image = CType(resources.GetObject("ctrl1.BtnOrderedList.Image"), System.Drawing.Image)
		Me.ctrl1.BtnOrderedList.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me.ctrl1.BtnOrderedList.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ctrl1.BtnOrderedList.Name = "_factoryBtnOrderedList"
		Me.ctrl1.BtnOrderedList.Size = New System.Drawing.Size(24, 26)
		Me.ctrl1.BtnOrderedList.Text = "Numbered List"
		'
		'ctrl1.BtnOutdent
		'
		Me.ctrl1.BtnOutdent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ctrl1.BtnOutdent.Image = CType(resources.GetObject("ctrl1.BtnOutdent.Image"), System.Drawing.Image)
		Me.ctrl1.BtnOutdent.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me.ctrl1.BtnOutdent.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ctrl1.BtnOutdent.Name = "_factoryBtnOutdent"
		Me.ctrl1.BtnOutdent.Size = New System.Drawing.Size(27, 26)
		Me.ctrl1.BtnOutdent.Text = "Outdent"
		'
		'ctrl1.BtnPaste
		'
		Me.ctrl1.BtnPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ctrl1.BtnPaste.Image = CType(resources.GetObject("ctrl1.BtnPaste.Image"), System.Drawing.Image)
		Me.ctrl1.BtnPaste.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me.ctrl1.BtnPaste.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ctrl1.BtnPaste.Name = "_factoryBtnPaste"
		Me.ctrl1.BtnPaste.Size = New System.Drawing.Size(23, 26)
		Me.ctrl1.BtnPaste.Text = "Paste"
		'
		'ctrl1.BtnPasteFromMSWord
		'
		Me.ctrl1.BtnPasteFromMSWord.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ctrl1.BtnPasteFromMSWord.Image = CType(resources.GetObject("ctrl1.BtnPasteFromMSWord.Image"), System.Drawing.Image)
		Me.ctrl1.BtnPasteFromMSWord.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me.ctrl1.BtnPasteFromMSWord.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ctrl1.BtnPasteFromMSWord.Name = "_factoryBtnPasteFromMSWord"
		Me.ctrl1.BtnPasteFromMSWord.Size = New System.Drawing.Size(23, 20)
		Me.ctrl1.BtnPasteFromMSWord.Text = "Paste the Content that you Copied from MS Word"
		'
		'ctrl1.BtnPrint
		'
		Me.ctrl1.BtnPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ctrl1.BtnPrint.Image = CType(resources.GetObject("ctrl1.BtnPrint.Image"), System.Drawing.Image)
		Me.ctrl1.BtnPrint.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me.ctrl1.BtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ctrl1.BtnPrint.Name = "_factoryBtnPrint"
		Me.ctrl1.BtnPrint.Size = New System.Drawing.Size(23, 20)
		Me.ctrl1.BtnPrint.Text = "Print"
		'
		'ctrl1.BtnSave
		'
		Me.ctrl1.BtnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ctrl1.BtnSave.Image = CType(resources.GetObject("ctrl1.BtnSave.Image"), System.Drawing.Image)
		Me.ctrl1.BtnSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me.ctrl1.BtnSave.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ctrl1.BtnSave.Name = "_factoryBtnSave"
		Me.ctrl1.BtnSave.Size = New System.Drawing.Size(23, 26)
		Me.ctrl1.BtnSave.Text = "Save"
		'
		'ctrl1.BtnSearch
		'
		Me.ctrl1.BtnSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ctrl1.BtnSearch.Image = CType(resources.GetObject("ctrl1.BtnSearch.Image"), System.Drawing.Image)
		Me.ctrl1.BtnSearch.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me.ctrl1.BtnSearch.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ctrl1.BtnSearch.Name = "_factoryBtnSearch"
		Me.ctrl1.BtnSearch.Size = New System.Drawing.Size(24, 24)
		Me.ctrl1.BtnSearch.Text = "Search"
		'
		'ctrl1.BtnSpellCheck
		'
		Me.ctrl1.BtnSpellCheck.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ctrl1.BtnSpellCheck.Image = CType(resources.GetObject("ctrl1.BtnSpellCheck.Image"), System.Drawing.Image)
		Me.ctrl1.BtnSpellCheck.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me.ctrl1.BtnSpellCheck.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ctrl1.BtnSpellCheck.Name = "_factoryBtnSpellCheck"
		Me.ctrl1.BtnSpellCheck.Size = New System.Drawing.Size(26, 26)
		Me.ctrl1.BtnSpellCheck.Text = "Check Spelling"
		'
		'ctrl1.BtnStrikeThrough
		'
		Me.ctrl1.BtnStrikeThrough.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ctrl1.BtnStrikeThrough.Image = CType(resources.GetObject("ctrl1.BtnStrikeThrough.Image"), System.Drawing.Image)
		Me.ctrl1.BtnStrikeThrough.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me.ctrl1.BtnStrikeThrough.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ctrl1.BtnStrikeThrough.Name = "_factoryBtnStrikeThrough"
		Me.ctrl1.BtnStrikeThrough.Size = New System.Drawing.Size(24, 24)
		Me.ctrl1.BtnStrikeThrough.Text = "Strike Thru"
		'
		'ctrl1.BtnSubscript
		'
		Me.ctrl1.BtnSubscript.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ctrl1.BtnSubscript.Image = CType(resources.GetObject("ctrl1.BtnSubscript.Image"), System.Drawing.Image)
		Me.ctrl1.BtnSubscript.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me.ctrl1.BtnSubscript.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ctrl1.BtnSubscript.Name = "_factoryBtnSubscript"
		Me.ctrl1.BtnSubscript.Size = New System.Drawing.Size(27, 26)
		Me.ctrl1.BtnSubscript.Text = "Subscript"
		'
		'ctrl1.BtnSuperScript
		'
		Me.ctrl1.BtnSuperScript.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ctrl1.BtnSuperScript.Image = CType(resources.GetObject("ctrl1.BtnSuperScript.Image"), System.Drawing.Image)
		Me.ctrl1.BtnSuperScript.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me.ctrl1.BtnSuperScript.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ctrl1.BtnSuperScript.Name = "_factoryBtnSuperScript"
		Me.ctrl1.BtnSuperScript.Size = New System.Drawing.Size(27, 26)
		Me.ctrl1.BtnSuperScript.Text = "Superscript"
		'
		'ctrl1.BtnSymbol
		'
		Me.ctrl1.BtnSymbol.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ctrl1.BtnSymbol.Image = CType(resources.GetObject("ctrl1.BtnSymbol.Image"), System.Drawing.Image)
		Me.ctrl1.BtnSymbol.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me.ctrl1.BtnSymbol.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ctrl1.BtnSymbol.Name = "_factoryBtnSymbol"
		Me.ctrl1.BtnSymbol.Size = New System.Drawing.Size(23, 26)
		Me.ctrl1.BtnSymbol.Text = "Insert Symbols"
		'
		'ctrl1.BtnTable
		'
		Me.ctrl1.BtnTable.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ctrl1.BtnTable.Image = CType(resources.GetObject("ctrl1.BtnTable.Image"), System.Drawing.Image)
		Me.ctrl1.BtnTable.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me.ctrl1.BtnTable.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ctrl1.BtnTable.Name = "_factoryBtnTable"
		Me.ctrl1.BtnTable.Size = New System.Drawing.Size(24, 26)
		Me.ctrl1.BtnTable.Text = "Table"
		'
		'ctrl1.BtnUnderline
		'
		Me.ctrl1.BtnUnderline.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ctrl1.BtnUnderline.Image = CType(resources.GetObject("ctrl1.BtnUnderline.Image"), System.Drawing.Image)
		Me.ctrl1.BtnUnderline.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me.ctrl1.BtnUnderline.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ctrl1.BtnUnderline.Name = "_factoryBtnUnderline"
		Me.ctrl1.BtnUnderline.Size = New System.Drawing.Size(23, 20)
		Me.ctrl1.BtnUnderline.Text = "Underline"
		'
		'ctrl1.BtnUnOrderedList
		'
		Me.ctrl1.BtnUnOrderedList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ctrl1.BtnUnOrderedList.Image = CType(resources.GetObject("ctrl1.BtnUnOrderedList.Image"), System.Drawing.Image)
		Me.ctrl1.BtnUnOrderedList.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me.ctrl1.BtnUnOrderedList.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ctrl1.BtnUnOrderedList.Name = "_factoryBtnUnOrderedList"
		Me.ctrl1.BtnUnOrderedList.Size = New System.Drawing.Size(24, 26)
		Me.ctrl1.BtnUnOrderedList.Text = "Bullet List"
		'
		'ctrl1.CmbFontName
		'
		Me.ctrl1.CmbFontName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
		Me.ctrl1.CmbFontName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
		Me.ctrl1.CmbFontName.MaxDropDownItems = 17
		Me.ctrl1.CmbFontName.Name = "_factoryCmbFontName"
		Me.ctrl1.CmbFontName.Size = New System.Drawing.Size(125, 29)
		Me.ctrl1.CmbFontName.Text = "Times New Roman"
		'
		'ctrl1.CmbFontSize
		'
		Me.ctrl1.CmbFontSize.Name = "_factoryCmbFontSize"
		Me.ctrl1.CmbFontSize.Size = New System.Drawing.Size(75, 29)
		Me.ctrl1.CmbFontSize.Text = "12pt"
		'
		'ctrl1.CmbTitleInsert
		'
		Me.ctrl1.CmbTitleInsert.Name = "_factoryCmbTitleInsert"
		Me.ctrl1.CmbTitleInsert.Size = New System.Drawing.Size(100, 29)
		Me.ctrl1.Controls.Add(Me.CustomFooter)
		Me.ctrl1.Controls.Add(Me.CustomHeader)
		Me.ctrl1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.ctrl1.DocumentHtml = resources.GetString("ctrl1.DocumentHtml")
		Me.ctrl1.EditorContextMenuStrip = Nothing
		Me.ctrl1.HeaderStyleContentElementID = "page_style"
		Me.ctrl1.HorizontalScroll = Nothing
		Me.ctrl1.Location = New System.Drawing.Point(0, 0)
		Me.ctrl1.Name = "ctrl1"
		Me.ctrl1.Options.ConvertFileUrlsToLocalPaths = True
		Me.ctrl1.Options.CustomDOCTYPE = Nothing
		Me.ctrl1.Options.DOCTYPEOverrideOptions = SpiceLogic.HtmlEditorControl.Domain.BOs.DocTypes.DoNotOverride
		Me.ctrl1.Options.EnterKeyResponse = SpiceLogic.HtmlEditorControl.Domain.BOs.EnteryKeyResponses.Paragraph
		Me.ctrl1.Options.FooterTagNavigatorFont = Nothing
		Me.ctrl1.Options.FooterTagNavigatorTextColor = System.Drawing.Color.Teal
		Me.ctrl1.Options.FTPSettingsForRemoteResources.ConnectionMode = SpiceLogic.HtmlEditorControl.Domain.BOs.UserOptions.FTPSettings.ConnectionModes.Active
		Me.ctrl1.Options.FTPSettingsForRemoteResources.Host = Nothing
		Me.ctrl1.Options.FTPSettingsForRemoteResources.Password = Nothing
		Me.ctrl1.Options.FTPSettingsForRemoteResources.Port = Nothing
		Me.ctrl1.Options.FTPSettingsForRemoteResources.RemoteFolderPath = Nothing
		Me.ctrl1.Options.FTPSettingsForRemoteResources.Timeout = 4000
		Me.ctrl1.Options.FTPSettingsForRemoteResources.UrlOfTheRemoteFolderPath = Nothing
		Me.ctrl1.Options.FTPSettingsForRemoteResources.UserName = Nothing
		Me.ctrl1.Size = New System.Drawing.Size(387, 343)
		Me.ctrl1.SpellCheckOptions.CurlyUnderlineImageFilePath = Nothing
		DictionaryFileInfo1.AffixFilePath = Nothing
		DictionaryFileInfo1.DictionaryFilePath = Nothing
		DictionaryFileInfo1.EnableUserDictionary = True
		DictionaryFileInfo1.UserDictionaryFilePath = Nothing
		Me.ctrl1.SpellCheckOptions.DictionaryFile = DictionaryFileInfo1
		Me.ctrl1.SpellCheckOptions.WaitAlertMessage = "Searching next misspelled word..... (please wait)"
		Me.ctrl1.TabIndex = 1
		'
		'ctrl1.WinFormHtmlEditor_Toolbar1
		'
		'
		'ctrl1.ToolStripSeparator1
		'
		Me.ctrl1.ToolStripSeparator1.Name = "_toolStripSeparator1"
		Me.ctrl1.ToolStripSeparator1.Size = New System.Drawing.Size(6, 29)
		'
		'ctrl1.ToolStripSeparator2
		'
		Me.ctrl1.ToolStripSeparator2.Name = "_toolStripSeparator2"
		Me.ctrl1.ToolStripSeparator2.Size = New System.Drawing.Size(6, 29)
		'
		'ctrl1.ToolStripSeparator3
		'
		Me.ctrl1.ToolStripSeparator3.Name = "_toolStripSeparator3"
		Me.ctrl1.ToolStripSeparator3.Size = New System.Drawing.Size(6, 29)
		'
		'ctrl1.ToolStripSeparator4
		'
		Me.ctrl1.ToolStripSeparator4.Name = "_toolStripSeparator4"
		Me.ctrl1.ToolStripSeparator4.Size = New System.Drawing.Size(6, 29)
		Me.ctrl1.Toolbar1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ctrl1.BtnNew, Me.ctrl1.BtnOpen, Me.ctrl1.BtnSave, Me.ctrl1.ToolStripSeparator1, Me.ctrl1.CmbFontName, Me.ctrl1.CmbFontSize, Me.ctrl1.ToolStripSeparator2, Me.ctrl1.BtnCut, Me.ctrl1.BtnCopy, Me.ctrl1.BtnPaste, Me.ctrl1.BtnPasteFromMSWord, Me.ctrl1.ToolStripSeparator3, Me.ctrl1.BtnBold, Me.ctrl1.BtnItalic, Me.ctrl1.BtnUnderline, Me.ctrl1.ToolStripSeparator4, Me.ctrl1.BtnFormatReset, Me.ctrl1.BtnFormatUndo, Me.ctrl1.BtnFormatRedo, Me.ctrl1.BtnPrint, Me.ctrl1.BtnSpellCheck, Me.ctrl1.BtnSearch})
		Me.ctrl1.Toolbar1.Location = New System.Drawing.Point(0, 25)
		Me.ctrl1.Toolbar1.Name = "WinFormHtmlEditor_Toolbar1"
		Me.ctrl1.Toolbar1.Size = New System.Drawing.Size(387, 29)
		Me.ctrl1.Toolbar1.TabIndex = 0
		Me.ctrl1.Toolbar1.Visible = False
		'
		'ctrl1.WinFormHtmlEditor_Toolbar2
		'
		'
		'ctrl1.ToolStripSeparator5
		'
		Me.ctrl1.ToolStripSeparator5.Name = "_toolStripSeparator5"
		Me.ctrl1.ToolStripSeparator5.Size = New System.Drawing.Size(6, 29)
		'
		'ctrl1.ToolStripSeparator6
		'
		Me.ctrl1.ToolStripSeparator6.Name = "_toolStripSeparator6"
		Me.ctrl1.ToolStripSeparator6.Size = New System.Drawing.Size(6, 29)
		'
		'ctrl1.ToolStripSeparator7
		'
		Me.ctrl1.ToolStripSeparator7.Name = "_toolStripSeparator7"
		Me.ctrl1.ToolStripSeparator7.Size = New System.Drawing.Size(6, 29)
		'
		'ctrl1.ToolStripSeparator8
		'
		Me.ctrl1.ToolStripSeparator8.Name = "_toolStripSeparator8"
		Me.ctrl1.ToolStripSeparator8.Size = New System.Drawing.Size(6, 29)
		'
		'ctrl1.ToolStripSeparator9
		'
		Me.ctrl1.ToolStripSeparator9.Name = "_toolStripSeparator9"
		Me.ctrl1.ToolStripSeparator9.Size = New System.Drawing.Size(6, 29)
		Me.ctrl1.Toolbar2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ctrl1.CmbTitleInsert, Me.ctrl1.BtnHighlightColor, Me.ctrl1.BtnFontColor, Me.ctrl1.ToolStripSeparator5, Me.ctrl1.BtnHyperlink, Me.ctrl1.BtnImage, Me.ctrl1.BtnInsertYouTubeVideo, Me.ctrl1.BtnTable, Me.ctrl1.BtnSymbol, Me.ctrl1.BtnHorizontalRule, Me.ctrl1.ToolStripSeparator6, Me.ctrl1.BtnOrderedList, Me.ctrl1.BtnUnOrderedList, Me.ctrl1.ToolStripSeparator7, Me.ctrl1.BtnAlignLeft, Me.ctrl1.BtnAlignCenter, Me.ctrl1.BtnAlignRight, Me.ctrl1.ToolStripSeparator8, Me.ctrl1.BtnOutdent, Me.ctrl1.BtnIndent, Me.ctrl1.ToolStripSeparator9, Me.ctrl1.BtnStrikeThrough, Me.ctrl1.BtnSuperScript, Me.ctrl1.BtnSubscript, Me.ctrl1.BtnBodyStyle})
		Me.ctrl1.Toolbar2.Location = New System.Drawing.Point(0, 25)
		Me.ctrl1.Toolbar2.Name = "WinFormHtmlEditor_Toolbar2"
		Me.ctrl1.Toolbar2.Size = New System.Drawing.Size(387, 29)
		Me.ctrl1.Toolbar2.TabIndex = 0
		Me.ctrl1.Toolbar2.Visible = False
		Me.ctrl1.ToolbarContextMenuStrip = Nothing
		'
		'ctrl1.WinFormHtmlEditor_ToolbarFooter
		'
		Me.ctrl1.ToolbarFooter.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.ctrl1.ToolbarFooter.Location = New System.Drawing.Point(0, 293)
		Me.ctrl1.ToolbarFooter.Name = "WinFormHtmlEditor_ToolbarFooter"
		Me.ctrl1.ToolbarFooter.Size = New System.Drawing.Size(387, 25)
		Me.ctrl1.ToolbarFooter.TabIndex = 7
		Me.ctrl1.ToolbarFooter.Visible = False
		Me.ctrl1.VerticalScroll = Nothing
		Me.ctrl1.z__ignore = True
		'
		'CustomFooter
		'
		Me.CustomFooter.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.CustomFooter.Location = New System.Drawing.Point(0, 318)
		Me.CustomFooter.Name = "CustomFooter"
		Me.CustomFooter.Size = New System.Drawing.Size(387, 25)
		Me.CustomFooter.TabIndex = 10
		Me.CustomFooter.Text = "ToolStrip2"
		'
		'CustomHeader
		'
		Me.CustomHeader.Location = New System.Drawing.Point(0, 0)
		Me.CustomHeader.Name = "CustomHeader"
		Me.CustomHeader.Size = New System.Drawing.Size(387, 25)
		Me.CustomHeader.TabIndex = 9
		Me.CustomHeader.Text = "ToolStrip1"
		'
		'ucHtmlEditor
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.Controls.Add(Me.ctrl1)
		Me.Name = "ucHtmlEditor"
		Me.Size = New System.Drawing.Size(387, 343)
		Me.ctrl1.Toolbar1.ResumeLayout(False)
		Me.ctrl1.Toolbar1.PerformLayout()
		Me.ctrl1.Toolbar2.ResumeLayout(False)
		Me.ctrl1.Toolbar2.PerformLayout()
		Me.ctrl1.ResumeLayout(False)
		Me.ctrl1.PerformLayout()
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents ctrl1 As SpiceLogic.WinHTMLEditor.WinForm.WinFormHtmlEditor
  Friend WithEvents CustomHeader As System.Windows.Forms.ToolStrip
  Friend WithEvents CustomFooter As System.Windows.Forms.ToolStrip

End Class
