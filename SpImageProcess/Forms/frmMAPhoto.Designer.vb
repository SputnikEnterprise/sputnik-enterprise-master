<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMAPhoto
  Inherits DevComponents.DotNetBar.Office2007RibbonForm

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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMAPhoto))
    Me.ribMain = New DevComponents.DotNetBar.RibbonControl()
    Me.RibbonPanel1 = New DevComponents.DotNetBar.RibbonPanel()
    Me.RibbonBar2 = New DevComponents.DotNetBar.RibbonBar()
    Me.ItemContainer8 = New DevComponents.DotNetBar.ItemContainer()
    Me.LabelItem2 = New DevComponents.DotNetBar.LabelItem()
    Me.LabelItem4 = New DevComponents.DotNetBar.LabelItem()
    Me.LabelItem1 = New DevComponents.DotNetBar.LabelItem()
    Me.ItemContainer7 = New DevComponents.DotNetBar.ItemContainer()
    Me.ribLblFileName = New DevComponents.DotNetBar.LabelItem()
    Me.ribLblOrgSize = New DevComponents.DotNetBar.LabelItem()
    Me.ribLblNewSize = New DevComponents.DotNetBar.LabelItem()
    Me.RibbonTabItem1 = New DevComponents.DotNetBar.RibbonTabItem()
    Me.ribBtnOpen = New DevComponents.DotNetBar.ButtonItem()
    Me.ribBtnSave = New DevComponents.DotNetBar.ButtonItem()
    Me.ribBtnDelete = New DevComponents.DotNetBar.ButtonItem()
    Me.StyleManager1 = New DevComponents.DotNetBar.StyleManager()
    Me.sImgResizer = New DevComponents.DotNetBar.Controls.Slider()
    Me.picMA = New System.Windows.Forms.PictureBox()
    Me.btnBack = New DevComponents.DotNetBar.ButtonX()
    Me.btnVor = New DevComponents.DotNetBar.ButtonX()
    Me.GroupPanel1 = New DevComponents.DotNetBar.Controls.GroupPanel()
    Me.picWait = New DevComponents.DotNetBar.Controls.CircularProgress()
    Me.ribMain.SuspendLayout()
    Me.RibbonPanel1.SuspendLayout()
    CType(Me.picMA, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.GroupPanel1.SuspendLayout()
    Me.SuspendLayout()
    '
    'ribMain
    '
    Me.ribMain.BackColor = System.Drawing.Color.White
    '
    '
    '
    Me.ribMain.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
    Me.ribMain.CanCustomize = False
    Me.ribMain.CaptionVisible = True
    Me.ribMain.Controls.Add(Me.RibbonPanel1)
    Me.ribMain.Dock = System.Windows.Forms.DockStyle.Top
    Me.ribMain.ForeColor = System.Drawing.Color.Black
    Me.ribMain.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.RibbonTabItem1})
    Me.ribMain.KeyTipsFont = New System.Drawing.Font("Tahoma", 7.0!)
    Me.ribMain.Location = New System.Drawing.Point(5, 1)
    Me.ribMain.Name = "ribMain"
    Me.ribMain.Padding = New System.Windows.Forms.Padding(0, 0, 0, 2)
    Me.ribMain.QuickToolbarItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.ribBtnOpen, Me.ribBtnSave, Me.ribBtnDelete})
    Me.ribMain.Size = New System.Drawing.Size(522, 116)
    Me.ribMain.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
    Me.ribMain.SystemText.MaximizeRibbonText = "&Maximize the Ribbon"
    Me.ribMain.SystemText.MinimizeRibbonText = "Mi&nimize the Ribbon"
    Me.ribMain.SystemText.QatAddItemText = "&Add to Quick Access Toolbar"
    Me.ribMain.SystemText.QatCustomizeMenuLabel = "<b>Customize Quick Access Toolbar</b>"
    Me.ribMain.SystemText.QatCustomizeText = "&Customize Quick Access Toolbar..."
    Me.ribMain.SystemText.QatDialogAddButton = "&Add >>"
    Me.ribMain.SystemText.QatDialogCancelButton = "Cancel"
    Me.ribMain.SystemText.QatDialogCaption = "Customize Quick Access Toolbar"
    Me.ribMain.SystemText.QatDialogCategoriesLabel = "&Choose commands from:"
    Me.ribMain.SystemText.QatDialogOkButton = "OK"
    Me.ribMain.SystemText.QatDialogPlacementCheckbox = "&Place Quick Access Toolbar below the Ribbon"
    Me.ribMain.SystemText.QatDialogRemoveButton = "&Remove"
    Me.ribMain.SystemText.QatPlaceAboveRibbonText = "&Place Quick Access Toolbar above the Ribbon"
    Me.ribMain.SystemText.QatPlaceBelowRibbonText = "&Place Quick Access Toolbar below the Ribbon"
    Me.ribMain.SystemText.QatRemoveItemText = "&Remove from Quick Access Toolbar"
    Me.ribMain.TabGroupHeight = 14
    Me.ribMain.TabIndex = 0
    '
    'RibbonPanel1
    '
    Me.RibbonPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
    Me.RibbonPanel1.Controls.Add(Me.RibbonBar2)
    Me.RibbonPanel1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.RibbonPanel1.Location = New System.Drawing.Point(0, 53)
    Me.RibbonPanel1.Name = "RibbonPanel1"
    Me.RibbonPanel1.Padding = New System.Windows.Forms.Padding(3, 0, 3, 3)
    Me.RibbonPanel1.Size = New System.Drawing.Size(522, 61)
    '
    '
    '
    Me.RibbonPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square
    '
    '
    '
    Me.RibbonPanel1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
    '
    '
    '
    Me.RibbonPanel1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
    Me.RibbonPanel1.TabIndex = 1
    '
    'RibbonBar2
    '
    Me.RibbonBar2.AutoOverflowEnabled = True
    '
    '
    '
    Me.RibbonBar2.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
    '
    '
    '
    Me.RibbonBar2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
    Me.RibbonBar2.ContainerControlProcessDialogKey = True
    Me.RibbonBar2.Dock = System.Windows.Forms.DockStyle.Left
    Me.RibbonBar2.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.ItemContainer8, Me.ItemContainer7})
    Me.RibbonBar2.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
    Me.RibbonBar2.Location = New System.Drawing.Point(3, 0)
    Me.RibbonBar2.Name = "RibbonBar2"
    Me.RibbonBar2.Size = New System.Drawing.Size(267, 58)
    Me.RibbonBar2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
    Me.RibbonBar2.TabIndex = 1
    Me.RibbonBar2.Text = "Bildinformationen"
    '
    '
    '
    Me.RibbonBar2.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
    '
    '
    '
    Me.RibbonBar2.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
    '
    'ItemContainer8
    '
    '
    '
    '
    Me.ItemContainer8.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
    Me.ItemContainer8.ItemSpacing = 0
    Me.ItemContainer8.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical
    Me.ItemContainer8.Name = "ItemContainer8"
    Me.ItemContainer8.SubItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.LabelItem2, Me.LabelItem4, Me.LabelItem1})
    '
    '
    '
    Me.ItemContainer8.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
    '
    'LabelItem2
    '
    Me.LabelItem2.Name = "LabelItem2"
    Me.LabelItem2.Text = "Datei"
    '
    'LabelItem4
    '
    Me.LabelItem4.Name = "LabelItem4"
    Me.LabelItem4.Text = "Original"
    '
    'LabelItem1
    '
    Me.LabelItem1.Name = "LabelItem1"
    Me.LabelItem1.Text = "Neu"
    '
    'ItemContainer7
    '
    '
    '
    '
    Me.ItemContainer7.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
    Me.ItemContainer7.ItemSpacing = 0
    Me.ItemContainer7.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical
    Me.ItemContainer7.MinimumSize = New System.Drawing.Size(200, 0)
    Me.ItemContainer7.Name = "ItemContainer7"
    Me.ItemContainer7.SubItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.ribLblFileName, Me.ribLblOrgSize, Me.ribLblNewSize})
    '
    '
    '
    Me.ItemContainer7.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
    '
    'ribLblFileName
    '
    Me.ribLblFileName.Name = "ribLblFileName"
    Me.ribLblFileName.Text = "ribLblFileName"
    '
    'ribLblOrgSize
    '
    Me.ribLblOrgSize.Name = "ribLblOrgSize"
    Me.ribLblOrgSize.Text = "ribLblSize"
    '
    'ribLblNewSize
    '
    Me.ribLblNewSize.Name = "ribLblNewSize"
    Me.ribLblNewSize.Text = "ribLblNewSize"
    '
    'RibbonTabItem1
    '
    Me.RibbonTabItem1.Checked = True
    Me.RibbonTabItem1.Name = "RibbonTabItem1"
    Me.RibbonTabItem1.Panel = Me.RibbonPanel1
    Me.RibbonTabItem1.Text = "Allgemein"
    '
    'ribBtnOpen
    '
    Me.ribBtnOpen.Icon = CType(resources.GetObject("ribBtnOpen.Icon"), System.Drawing.Icon)
    Me.ribBtnOpen.Name = "ribBtnOpen"
    Me.ribBtnOpen.Text = "Öffnen"
    '
    'ribBtnSave
    '
    Me.ribBtnSave.Icon = CType(resources.GetObject("ribBtnSave.Icon"), System.Drawing.Icon)
    Me.ribBtnSave.Name = "ribBtnSave"
    Me.ribBtnSave.Text = "Sichern"
    '
    'ribBtnDelete
    '
    Me.ribBtnDelete.Icon = CType(resources.GetObject("ribBtnDelete.Icon"), System.Drawing.Icon)
    Me.ribBtnDelete.Name = "ribBtnDelete"
    Me.ribBtnDelete.Text = "Löschen"
    '
    'StyleManager1
    '
    Me.StyleManager1.ManagerStyle = DevComponents.DotNetBar.eStyle.Office2010Blue
    Me.StyleManager1.MetroColorParameters = New DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(System.Drawing.Color.White, System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(163, Byte), Integer), CType(CType(26, Byte), Integer)))
    '
    'sImgResizer
    '
    Me.sImgResizer.BackColor = System.Drawing.Color.Transparent
    '
    '
    '
    Me.sImgResizer.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
    Me.sImgResizer.LabelPosition = DevComponents.DotNetBar.eSliderLabelPosition.Bottom
    Me.sImgResizer.Location = New System.Drawing.Point(138, 441)
    Me.sImgResizer.Name = "sImgResizer"
    Me.sImgResizer.Size = New System.Drawing.Size(244, 40)
    Me.sImgResizer.Step = 5
    Me.sImgResizer.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
    Me.sImgResizer.TabIndex = 1
    Me.sImgResizer.Text = "100 %"
    Me.sImgResizer.Value = 0
    '
    'picMA
    '
    Me.picMA.ErrorImage = Global.SpImageProcess.My.Resources.Resources.images_4_
    Me.picMA.InitialImage = Nothing
    Me.picMA.Location = New System.Drawing.Point(2, 2)
    Me.picMA.Name = "picMA"
    Me.picMA.Size = New System.Drawing.Size(225, 225)
    Me.picMA.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
    Me.picMA.TabIndex = 0
    Me.picMA.TabStop = False
    '
    'btnBack
    '
    Me.btnBack.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
    Me.btnBack.BackColor = System.Drawing.Color.Transparent
    Me.btnBack.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
    Me.btnBack.Image = CType(resources.GetObject("btnBack.Image"), System.Drawing.Image)
    Me.btnBack.Location = New System.Drawing.Point(400, 314)
    Me.btnBack.Name = "btnBack"
    Me.btnBack.Size = New System.Drawing.Size(53, 41)
    Me.btnBack.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
    Me.btnBack.TabIndex = 3
    '
    'btnVor
    '
    Me.btnVor.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
    Me.btnVor.BackColor = System.Drawing.Color.Transparent
    Me.btnVor.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
    Me.btnVor.Image = CType(resources.GetObject("btnVor.Image"), System.Drawing.Image)
    Me.btnVor.Location = New System.Drawing.Point(68, 314)
    Me.btnVor.Name = "btnVor"
    Me.btnVor.PressedImage = CType(resources.GetObject("btnVor.PressedImage"), System.Drawing.Image)
    Me.btnVor.Size = New System.Drawing.Size(53, 41)
    Me.btnVor.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
    Me.btnVor.TabIndex = 4
    '
    'GroupPanel1
    '
    Me.GroupPanel1.AutoScroll = True
    Me.GroupPanel1.CanvasColor = System.Drawing.SystemColors.Control
    Me.GroupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
    Me.GroupPanel1.Controls.Add(Me.picMA)
    Me.GroupPanel1.Location = New System.Drawing.Point(138, 152)
    Me.GroupPanel1.Name = "GroupPanel1"
    Me.GroupPanel1.Size = New System.Drawing.Size(244, 283)
    '
    '
    '
    Me.GroupPanel1.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
    Me.GroupPanel1.Style.BackColorGradientAngle = 90
    Me.GroupPanel1.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
    Me.GroupPanel1.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
    Me.GroupPanel1.Style.BorderBottomWidth = 1
    Me.GroupPanel1.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
    Me.GroupPanel1.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
    Me.GroupPanel1.Style.BorderLeftWidth = 1
    Me.GroupPanel1.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
    Me.GroupPanel1.Style.BorderRightWidth = 1
    Me.GroupPanel1.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
    Me.GroupPanel1.Style.BorderTopWidth = 1
    Me.GroupPanel1.Style.CornerDiameter = 4
    Me.GroupPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
    Me.GroupPanel1.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
    Me.GroupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
    Me.GroupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
    '
    '
    '
    Me.GroupPanel1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
    '
    '
    '
    Me.GroupPanel1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
    Me.GroupPanel1.TabIndex = 5
    '
    'picWait
    '
    Me.picWait.BackColor = System.Drawing.Color.Transparent
    '
    '
    '
    Me.picWait.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
    Me.picWait.Location = New System.Drawing.Point(400, 389)
    Me.picWait.Name = "picWait"
    Me.picWait.ProgressText = "Bitte warten Sie..."
    Me.picWait.Size = New System.Drawing.Size(46, 46)
    Me.picWait.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
    Me.picWait.TabIndex = 6
    Me.picWait.TabStop = False
    '
    'frmMAPhoto
    '
    Me.ClientSize = New System.Drawing.Size(532, 513)
    Me.Controls.Add(Me.picWait)
    Me.Controls.Add(Me.GroupPanel1)
    Me.Controls.Add(Me.btnVor)
    Me.Controls.Add(Me.btnBack)
    Me.Controls.Add(Me.sImgResizer)
    Me.Controls.Add(Me.ribMain)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.MaximumSize = New System.Drawing.Size(532, 513)
    Me.Name = "frmMAPhoto"
    Me.Text = "Bild von Kandidat bearbeiten"
    Me.ribMain.ResumeLayout(False)
    Me.ribMain.PerformLayout()
    Me.RibbonPanel1.ResumeLayout(False)
    CType(Me.picMA, System.ComponentModel.ISupportInitialize).EndInit()
    Me.GroupPanel1.ResumeLayout(False)
    Me.GroupPanel1.PerformLayout()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents ribMain As DevComponents.DotNetBar.RibbonControl
  Friend WithEvents RibbonPanel1 As DevComponents.DotNetBar.RibbonPanel
  Friend WithEvents RibbonTabItem1 As DevComponents.DotNetBar.RibbonTabItem
  Friend WithEvents ribBtnSave As DevComponents.DotNetBar.ButtonItem
  Friend WithEvents StyleManager1 As DevComponents.DotNetBar.StyleManager
  Friend WithEvents RibbonBar2 As DevComponents.DotNetBar.RibbonBar
  Private WithEvents ItemContainer8 As DevComponents.DotNetBar.ItemContainer
  Friend WithEvents LabelItem4 As DevComponents.DotNetBar.LabelItem
  Private WithEvents ItemContainer7 As DevComponents.DotNetBar.ItemContainer
  Friend WithEvents ribLblOrgSize As DevComponents.DotNetBar.LabelItem
  Friend WithEvents ribBtnDelete As DevComponents.DotNetBar.ButtonItem
  Friend WithEvents sImgResizer As DevComponents.DotNetBar.Controls.Slider
  Friend WithEvents picMA As System.Windows.Forms.PictureBox
  Friend WithEvents ribBtnOpen As DevComponents.DotNetBar.ButtonItem
  Friend WithEvents btnBack As DevComponents.DotNetBar.ButtonX
  Friend WithEvents btnVor As DevComponents.DotNetBar.ButtonX
  Friend WithEvents LabelItem1 As DevComponents.DotNetBar.LabelItem
  Friend WithEvents ribLblNewSize As DevComponents.DotNetBar.LabelItem
  Friend WithEvents GroupPanel1 As DevComponents.DotNetBar.Controls.GroupPanel
  Friend WithEvents picWait As DevComponents.DotNetBar.Controls.CircularProgress
  Friend WithEvents LabelItem2 As DevComponents.DotNetBar.LabelItem
  Friend WithEvents ribLblFileName As DevComponents.DotNetBar.LabelItem
End Class
