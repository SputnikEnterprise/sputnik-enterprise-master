<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCSV
  Inherits DevExpress.XtraEditors.XtraForm

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
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

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCSV))
		Me.lblGEDateiname = New DevExpress.XtraEditors.LabelControl()
		Me.txt_GEFilename = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.Bar3 = New DevExpress.XtraBars.Bar()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager()
		Me.Bar1 = New DevExpress.XtraBars.Bar()
		Me.bsiInfo = New DevExpress.XtraBars.BarStaticItem()
		Me.bbiExport = New DevExpress.XtraBars.BarLargeButtonItem()
		Me.bbiPossibleValues = New DevExpress.XtraBars.BarLargeButtonItem()
		Me.bbiPrintlist = New DevExpress.XtraBars.BarButtonItem()
		Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
		Me.GroupBox1 = New DevExpress.XtraEditors.PanelControl()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.lblHeader2 = New DevExpress.XtraEditors.LabelControl()
		Me.LblSetting = New DevExpress.XtraEditors.LabelControl()
		Me.lblHeader1 = New DevExpress.XtraEditors.LabelControl()
		Me.btnClose = New DevExpress.XtraEditors.SimpleButton()
		Me.lbl_ListTypeGe = New DevExpress.XtraEditors.LabelControl()
		Me.xtabMain = New DevExpress.XtraTab.XtraTabControl()
		Me.xtabExportGenf = New DevExpress.XtraTab.XtraTabPage()
		Me.lbLstlInfo = New DevExpress.XtraEditors.LabelControl()
		Me.lueXMLArt = New DevExpress.XtraEditors.LookUpEdit()
		Me.xtabExporteQuest = New DevExpress.XtraTab.XtraTabPage()
		Me.tpeQuestData = New DevExpress.XtraBars.Navigation.TabPane()
		Me.tnpValidData = New DevExpress.XtraBars.Navigation.TabNavigationPage()
		Me.pnleQuest = New DevExpress.XtraEditors.PanelControl()
		Me.grdeQuestValidData = New DevExpress.XtraGrid.GridControl()
		Me.gveQuestValidData = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.tnpInvalidData = New DevExpress.XtraBars.Navigation.TabNavigationPage()
		Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
		Me.grdeQuestInvalidData = New DevExpress.XtraGrid.GridControl()
		Me.gveQuestInvalidData = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.lbleQuestPfad = New DevExpress.XtraEditors.LabelControl()
		Me.txt_eQuestFilename = New DevExpress.XtraEditors.ComboBoxEdit()
		CType(Me.txt_GEFilename.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupBox1.SuspendLayout()
		CType(Me.xtabMain, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.xtabMain.SuspendLayout()
		Me.xtabExportGenf.SuspendLayout()
		CType(Me.lueXMLArt.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.xtabExporteQuest.SuspendLayout()
		CType(Me.tpeQuestData, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.tpeQuestData.SuspendLayout()
		Me.tnpValidData.SuspendLayout()
		CType(Me.pnleQuest, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.pnleQuest.SuspendLayout()
		CType(Me.grdeQuestValidData, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gveQuestValidData, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.tnpInvalidData.SuspendLayout()
		CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.PanelControl1.SuspendLayout()
		CType(Me.grdeQuestInvalidData, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gveQuestInvalidData, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txt_eQuestFilename.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'lblGEDateiname
		'
		Me.lblGEDateiname.Appearance.Options.UseTextOptions = True
		Me.lblGEDateiname.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblGEDateiname.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblGEDateiname.Location = New System.Drawing.Point(38, 47)
		Me.lblGEDateiname.Name = "lblGEDateiname"
		Me.lblGEDateiname.Size = New System.Drawing.Size(101, 13)
		Me.lblGEDateiname.TabIndex = 143
		Me.lblGEDateiname.Text = "Dateiname"
		'
		'txt_GEFilename
		'
		Me.txt_GEFilename.Location = New System.Drawing.Point(145, 44)
		Me.txt_GEFilename.Name = "txt_GEFilename"
		Me.txt_GEFilename.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.txt_GEFilename.Size = New System.Drawing.Size(392, 20)
		Me.txt_GEFilename.TabIndex = 0
		'
		'Bar3
		'
		Me.Bar3.DockCol = 0
		'
		'BarManager1
		'
		Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar1})
		Me.BarManager1.DockControls.Add(Me.barDockControlTop)
		Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
		Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
		Me.BarManager1.DockControls.Add(Me.barDockControlRight)
		Me.BarManager1.Form = Me
		Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiInfo, Me.bbiExport, Me.bbiPossibleValues, Me.bbiPrintlist})
		Me.BarManager1.MaxItemId = 13
		Me.BarManager1.StatusBar = Me.Bar1
		'
		'Bar1
		'
		Me.Bar1.BarName = "Statusleiste"
		Me.Bar1.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
		Me.Bar1.DockCol = 0
		Me.Bar1.DockRow = 0
		Me.Bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
		Me.Bar1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiInfo), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiExport, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiPossibleValues, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiPrintlist)})
		Me.Bar1.OptionsBar.AllowQuickCustomization = False
		Me.Bar1.OptionsBar.DrawDragBorder = False
		Me.Bar1.OptionsBar.UseWholeRow = True
		Me.Bar1.Text = "Statusleiste"
		'
		'bsiInfo
		'
		Me.bsiInfo.AutoSize = DevExpress.XtraBars.BarStaticItemSize.Spring
		Me.bsiInfo.Caption = "Bereit"
		Me.bsiInfo.Id = 0
		Me.bsiInfo.Name = "bsiInfo"
		Me.bsiInfo.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiExport
		'
		Me.bbiExport.Caption = "Exportieren"
		Me.bbiExport.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Right
		Me.bbiExport.Id = 10
		Me.bbiExport.ImageOptions.Image = CType(resources.GetObject("bbiExport.ImageOptions.Image"), System.Drawing.Image)
		Me.bbiExport.Name = "bbiExport"
		Me.bbiExport.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiPossibleValues
		'
		Me.bbiPossibleValues.Caption = "Mögliche Eingaben"
		Me.bbiPossibleValues.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Right
		Me.bbiPossibleValues.Id = 11
		Me.bbiPossibleValues.ImageOptions.Image = CType(resources.GetObject("bbiPossibleValues.ImageOptions.Image"), System.Drawing.Image)
		Me.bbiPossibleValues.Name = "bbiPossibleValues"
		Me.bbiPossibleValues.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiPrintlist
		'
		Me.bbiPrintlist.Caption = "Auflistung drucken"
		Me.bbiPrintlist.Id = 12
		Me.bbiPrintlist.ImageOptions.Image = Global.SPQSTListeSearch.My.Resources.Resources.printarea_16x161
		Me.bbiPrintlist.ImageOptions.LargeImage = Global.SPQSTListeSearch.My.Resources.Resources.printarea_32x321
		Me.bbiPrintlist.Name = "bbiPrintlist"
		Me.bbiPrintlist.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'barDockControlTop
		'
		Me.barDockControlTop.CausesValidation = False
		Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
		Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlTop.Manager = Me.BarManager1
		Me.barDockControlTop.Size = New System.Drawing.Size(690, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 341)
		Me.barDockControlBottom.Manager = Me.BarManager1
		Me.barDockControlBottom.Size = New System.Drawing.Size(690, 27)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Manager = Me.BarManager1
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 341)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(690, 0)
		Me.barDockControlRight.Manager = Me.BarManager1
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 341)
		'
		'GroupBox1
		'
		Me.GroupBox1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003
		Me.GroupBox1.Controls.Add(Me.Label1)
		Me.GroupBox1.Controls.Add(Me.lblHeader2)
		Me.GroupBox1.Controls.Add(Me.LblSetting)
		Me.GroupBox1.Controls.Add(Me.lblHeader1)
		Me.GroupBox1.Controls.Add(Me.btnClose)
		Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
		Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(690, 59)
		Me.GroupBox1.TabIndex = 229
		'
		'Label1
		'
		Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label1.ForeColor = System.Drawing.SystemColors.HotTrack
		Me.Label1.Image = CType(resources.GetObject("Label1.Image"), System.Drawing.Image)
		Me.Label1.Location = New System.Drawing.Point(12, 10)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(51, 38)
		Me.Label1.TabIndex = 215
		'
		'lblHeader2
		'
		Me.lblHeader2.AllowHtmlString = True
		Me.lblHeader2.Location = New System.Drawing.Point(78, 31)
		Me.lblHeader2.Name = "lblHeader2"
		Me.lblHeader2.Size = New System.Drawing.Size(218, 13)
		Me.lblHeader2.TabIndex = 214
		Me.lblHeader2.Text = "Wählen Sie Ihre gewünschten Kriterien aus..."
		'
		'LblSetting
		'
		Me.LblSetting.Appearance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.LblSetting.Appearance.ForeColor = System.Drawing.SystemColors.HotTrack
		Me.LblSetting.Appearance.Options.UseFont = True
		Me.LblSetting.Appearance.Options.UseForeColor = True
		Me.LblSetting.Location = New System.Drawing.Point(12, 12)
		Me.LblSetting.Name = "LblSetting"
		Me.LblSetting.Size = New System.Drawing.Size(0, 13)
		Me.LblSetting.TabIndex = 213
		'
		'lblHeader1
		'
		Me.lblHeader1.AllowHtmlString = True
		Me.lblHeader1.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.lblHeader1.Appearance.Options.UseFont = True
		Me.lblHeader1.Location = New System.Drawing.Point(69, 12)
		Me.lblHeader1.Name = "lblHeader1"
		Me.lblHeader1.Size = New System.Drawing.Size(197, 13)
		Me.lblHeader1.TabIndex = 213
		Me.lblHeader1.Text = "Export der Daten für Quellensteuer"
		'
		'btnClose
		'
		Me.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Right
		Me.btnClose.Location = New System.Drawing.Point(563, 12)
		Me.btnClose.Name = "btnClose"
		Me.btnClose.Size = New System.Drawing.Size(94, 25)
		Me.btnClose.TabIndex = 204
		Me.btnClose.Text = "Schliessen"
		'
		'lbl_ListTypeGe
		'
		Me.lbl_ListTypeGe.Appearance.Options.UseTextOptions = True
		Me.lbl_ListTypeGe.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lbl_ListTypeGe.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lbl_ListTypeGe.Location = New System.Drawing.Point(7, 74)
		Me.lbl_ListTypeGe.Name = "lbl_ListTypeGe"
		Me.lbl_ListTypeGe.Size = New System.Drawing.Size(132, 13)
		Me.lbl_ListTypeGe.TabIndex = 144
		Me.lbl_ListTypeGe.Text = "Listentyp GE"
		'
		'xtabMain
		'
		Me.xtabMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.xtabMain.Location = New System.Drawing.Point(29, 74)
		Me.xtabMain.Name = "xtabMain"
		Me.xtabMain.SelectedTabPage = Me.xtabExportGenf
		Me.xtabMain.Size = New System.Drawing.Size(628, 242)
		Me.xtabMain.TabIndex = 234
		Me.xtabMain.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabExportGenf, Me.xtabExporteQuest})
		'
		'xtabExportGenf
		'
		Me.xtabExportGenf.Controls.Add(Me.lbLstlInfo)
		Me.xtabExportGenf.Controls.Add(Me.lblGEDateiname)
		Me.xtabExportGenf.Controls.Add(Me.txt_GEFilename)
		Me.xtabExportGenf.Controls.Add(Me.lbl_ListTypeGe)
		Me.xtabExportGenf.Controls.Add(Me.lueXMLArt)
		Me.xtabExportGenf.Name = "xtabExportGenf"
		Me.xtabExportGenf.Padding = New System.Windows.Forms.Padding(10)
		Me.xtabExportGenf.Size = New System.Drawing.Size(622, 214)
		Me.xtabExportGenf.Text = "Genf / Waadt"
		'
		'lbLstlInfo
		'
		Me.lbLstlInfo.AllowHtmlString = True
		Me.lbLstlInfo.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.lbLstlInfo.Appearance.Image = CType(resources.GetObject("lbLstlInfo.Appearance.Image"), System.Drawing.Image)
		Me.lbLstlInfo.Appearance.Options.UseBackColor = True
		Me.lbLstlInfo.Appearance.Options.UseImage = True
		Me.lbLstlInfo.Appearance.Options.UseTextOptions = True
		Me.lbLstlInfo.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter
		Me.lbLstlInfo.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
		Me.lbLstlInfo.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
		Me.lbLstlInfo.AutoEllipsis = True
		Me.lbLstlInfo.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lbLstlInfo.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftTop
		Me.lbLstlInfo.LineColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
		Me.lbLstlInfo.LineLocation = DevExpress.XtraEditors.LineLocation.Left
		Me.lbLstlInfo.LineOrientation = DevExpress.XtraEditors.LabelLineOrientation.Vertical
		Me.lbLstlInfo.LineVisible = True
		Me.lbLstlInfo.Location = New System.Drawing.Point(247, 70)
		Me.lbLstlInfo.Name = "lbLstlInfo"
		Me.lbLstlInfo.Size = New System.Drawing.Size(311, 111)
		Me.lbLstlInfo.TabIndex = 290
		Me.lbLstlInfo.Text = resources.GetString("lbLstlInfo.Text")
		'
		'lueXMLArt
		'
		Me.lueXMLArt.EditValue = ""
		Me.lueXMLArt.Location = New System.Drawing.Point(145, 70)
		Me.lueXMLArt.Name = "lueXMLArt"
		Me.lueXMLArt.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.DropDown)})
		Me.lueXMLArt.Properties.NullText = ""
		Me.lueXMLArt.Properties.PopupSizeable = False
		Me.lueXMLArt.Size = New System.Drawing.Size(96, 20)
		Me.lueXMLArt.TabIndex = 2
		'
		'xtabExporteQuest
		'
		Me.xtabExporteQuest.Controls.Add(Me.tpeQuestData)
		Me.xtabExporteQuest.Controls.Add(Me.lbleQuestPfad)
		Me.xtabExporteQuest.Controls.Add(Me.txt_eQuestFilename)
		Me.xtabExporteQuest.Name = "xtabExporteQuest"
		Me.xtabExporteQuest.Padding = New System.Windows.Forms.Padding(10)
		Me.xtabExporteQuest.Size = New System.Drawing.Size(622, 214)
		Me.xtabExporteQuest.Text = "eQuest"
		'
		'tpeQuestData
		'
		Me.tpeQuestData.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.tpeQuestData.Controls.Add(Me.tnpValidData)
		Me.tpeQuestData.Controls.Add(Me.tnpInvalidData)
		Me.tpeQuestData.Location = New System.Drawing.Point(3, 84)
		Me.tpeQuestData.Name = "tpeQuestData"
		Me.tpeQuestData.Pages.AddRange(New DevExpress.XtraBars.Navigation.NavigationPageBase() {Me.tnpValidData, Me.tnpInvalidData})
		Me.tpeQuestData.RegularSize = New System.Drawing.Size(616, 127)
		Me.tpeQuestData.SelectedPage = Me.tnpInvalidData
		Me.tpeQuestData.Size = New System.Drawing.Size(616, 127)
		Me.tpeQuestData.TabIndex = 152
		Me.tpeQuestData.Text = "TabPane1"
		'
		'tnpValidData
		'
		Me.tnpValidData.Caption = "Gültige Daten (Daten werden übermittelt)"
		Me.tnpValidData.Controls.Add(Me.pnleQuest)
		Me.tnpValidData.Name = "tnpValidData"
		Me.tnpValidData.Padding = New System.Windows.Forms.Padding(5)
		Me.tnpValidData.Size = New System.Drawing.Size(616, 100)
		'
		'pnleQuest
		'
		Me.pnleQuest.Controls.Add(Me.grdeQuestValidData)
		Me.pnleQuest.Dock = System.Windows.Forms.DockStyle.Fill
		Me.pnleQuest.Location = New System.Drawing.Point(5, 5)
		Me.pnleQuest.Name = "pnleQuest"
		Me.pnleQuest.Padding = New System.Windows.Forms.Padding(5)
		Me.pnleQuest.Size = New System.Drawing.Size(606, 90)
		Me.pnleQuest.TabIndex = 152
		'
		'grdeQuestValidData
		'
		Me.grdeQuestValidData.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdeQuestValidData.Location = New System.Drawing.Point(7, 7)
		Me.grdeQuestValidData.MainView = Me.gveQuestValidData
		Me.grdeQuestValidData.Name = "grdeQuestValidData"
		Me.grdeQuestValidData.Size = New System.Drawing.Size(592, 76)
		Me.grdeQuestValidData.TabIndex = 4
		Me.grdeQuestValidData.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gveQuestValidData})
		'
		'gveQuestValidData
		'
		Me.gveQuestValidData.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gveQuestValidData.GridControl = Me.grdeQuestValidData
		Me.gveQuestValidData.Name = "gveQuestValidData"
		Me.gveQuestValidData.OptionsBehavior.Editable = False
		Me.gveQuestValidData.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gveQuestValidData.OptionsView.ShowAutoFilterRow = True
		Me.gveQuestValidData.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		Me.gveQuestValidData.OptionsView.ShowGroupPanel = False
		Me.gveQuestValidData.OptionsView.ShowIndicator = False
		'
		'tnpInvalidData
		'
		Me.tnpInvalidData.Appearance.Options.UseForeColor = True
		Me.tnpInvalidData.Caption = "Ungültige Daten (Daten werden NICHT übermittelt)"
		Me.tnpInvalidData.Controls.Add(Me.PanelControl1)
		Me.tnpInvalidData.ItemShowMode = DevExpress.XtraBars.Navigation.ItemShowMode.ImageAndText
		Me.tnpInvalidData.Name = "tnpInvalidData"
		Me.tnpInvalidData.Padding = New System.Windows.Forms.Padding(5)
		Me.tnpInvalidData.Properties.AppearanceCaption.ForeColor = System.Drawing.Color.Red
		Me.tnpInvalidData.Properties.AppearanceCaption.Image = Global.SPQSTListeSearch.My.Resources.Resources.warning_16x16
		Me.tnpInvalidData.Properties.AppearanceCaption.Options.UseForeColor = True
		Me.tnpInvalidData.Properties.AppearanceCaption.Options.UseImage = True
		Me.tnpInvalidData.Properties.ShowMode = DevExpress.XtraBars.Navigation.ItemShowMode.ImageAndText
		Me.tnpInvalidData.Size = New System.Drawing.Size(616, 100)
		'
		'PanelControl1
		'
		Me.PanelControl1.Controls.Add(Me.grdeQuestInvalidData)
		Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.PanelControl1.Location = New System.Drawing.Point(5, 5)
		Me.PanelControl1.Name = "PanelControl1"
		Me.PanelControl1.Padding = New System.Windows.Forms.Padding(5)
		Me.PanelControl1.Size = New System.Drawing.Size(606, 90)
		Me.PanelControl1.TabIndex = 152
		'
		'grdeQuestInvalidData
		'
		Me.grdeQuestInvalidData.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdeQuestInvalidData.Location = New System.Drawing.Point(7, 7)
		Me.grdeQuestInvalidData.MainView = Me.gveQuestInvalidData
		Me.grdeQuestInvalidData.Name = "grdeQuestInvalidData"
		Me.grdeQuestInvalidData.Size = New System.Drawing.Size(592, 76)
		Me.grdeQuestInvalidData.TabIndex = 4
		Me.grdeQuestInvalidData.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gveQuestInvalidData})
		'
		'gveQuestInvalidData
		'
		Me.gveQuestInvalidData.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gveQuestInvalidData.GridControl = Me.grdeQuestInvalidData
		Me.gveQuestInvalidData.Name = "gveQuestInvalidData"
		Me.gveQuestInvalidData.OptionsBehavior.Editable = False
		Me.gveQuestInvalidData.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gveQuestInvalidData.OptionsView.ShowAutoFilterRow = True
		Me.gveQuestInvalidData.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		Me.gveQuestInvalidData.OptionsView.ShowGroupPanel = False
		Me.gveQuestInvalidData.OptionsView.ShowIndicator = False
		'
		'lbleQuestPfad
		'
		Me.lbleQuestPfad.Appearance.Options.UseTextOptions = True
		Me.lbleQuestPfad.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lbleQuestPfad.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lbleQuestPfad.Location = New System.Drawing.Point(38, 47)
		Me.lbleQuestPfad.Name = "lbleQuestPfad"
		Me.lbleQuestPfad.Size = New System.Drawing.Size(101, 13)
		Me.lbleQuestPfad.TabIndex = 150
		Me.lbleQuestPfad.Text = "Dateipfad"
		'
		'txt_eQuestFilename
		'
		Me.txt_eQuestFilename.Location = New System.Drawing.Point(145, 44)
		Me.txt_eQuestFilename.Name = "txt_eQuestFilename"
		Me.txt_eQuestFilename.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.txt_eQuestFilename.Size = New System.Drawing.Size(392, 20)
		Me.txt_eQuestFilename.TabIndex = 146
		'
		'frmCSV
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(690, 368)
		Me.Controls.Add(Me.xtabMain)
		Me.Controls.Add(Me.GroupBox1)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.Name = "frmCSV"
		Me.Text = "Daten in einer Datei exportieren"
		CType(Me.txt_GEFilename.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupBox1.ResumeLayout(False)
		Me.GroupBox1.PerformLayout()
		CType(Me.xtabMain, System.ComponentModel.ISupportInitialize).EndInit()
		Me.xtabMain.ResumeLayout(False)
		Me.xtabExportGenf.ResumeLayout(False)
		CType(Me.lueXMLArt.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.xtabExporteQuest.ResumeLayout(False)
		CType(Me.tpeQuestData, System.ComponentModel.ISupportInitialize).EndInit()
		Me.tpeQuestData.ResumeLayout(False)
		Me.tnpValidData.ResumeLayout(False)
		CType(Me.pnleQuest, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pnleQuest.ResumeLayout(False)
		CType(Me.grdeQuestValidData, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gveQuestValidData, System.ComponentModel.ISupportInitialize).EndInit()
		Me.tnpInvalidData.ResumeLayout(False)
		CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.PanelControl1.ResumeLayout(False)
		CType(Me.grdeQuestInvalidData, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gveQuestInvalidData, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txt_eQuestFilename.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents lblGEDateiname As DevExpress.XtraEditors.LabelControl
	Friend WithEvents txt_GEFilename As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents Bar3 As DevExpress.XtraBars.Bar
	Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
	Friend WithEvents Bar1 As DevExpress.XtraBars.Bar
	Friend WithEvents bsiInfo As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents bbiExport As DevExpress.XtraBars.BarLargeButtonItem
	Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
	Friend WithEvents GroupBox1 As DevExpress.XtraEditors.PanelControl
	Friend WithEvents lblHeader2 As DevExpress.XtraEditors.LabelControl
	Friend WithEvents LblSetting As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblHeader1 As DevExpress.XtraEditors.LabelControl
	Friend WithEvents btnClose As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents Label1 As System.Windows.Forms.Label
	Friend WithEvents lbl_ListTypeGe As DevExpress.XtraEditors.LabelControl
	Friend WithEvents bbiPossibleValues As DevExpress.XtraBars.BarLargeButtonItem
	Friend WithEvents xtabMain As DevExpress.XtraTab.XtraTabControl
	Friend WithEvents xtabExportGenf As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents xtabExporteQuest As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents lbleQuestPfad As DevExpress.XtraEditors.LabelControl
	Friend WithEvents txt_eQuestFilename As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents lueXMLArt As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents lbLstlInfo As DevExpress.XtraEditors.LabelControl
	Friend WithEvents tpeQuestData As DevExpress.XtraBars.Navigation.TabPane
	Friend WithEvents tnpValidData As DevExpress.XtraBars.Navigation.TabNavigationPage
	Friend WithEvents pnleQuest As DevExpress.XtraEditors.PanelControl
	Friend WithEvents grdeQuestValidData As DevExpress.XtraGrid.GridControl
	Friend WithEvents gveQuestValidData As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents tnpInvalidData As DevExpress.XtraBars.Navigation.TabNavigationPage
	Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
	Friend WithEvents grdeQuestInvalidData As DevExpress.XtraGrid.GridControl
	Friend WithEvents gveQuestInvalidData As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents bbiPrintlist As DevExpress.XtraBars.BarButtonItem
End Class
