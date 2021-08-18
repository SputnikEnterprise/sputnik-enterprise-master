<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDoc
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
		Dim ButtonImageOptions1 As DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions = New DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions()
		Dim ButtonImageOptions2 As DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions = New DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDoc))
		Dim EditorButtonImageOptions1 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
		Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject4 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim EditorButtonImageOptions2 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
		Dim SerializableAppearanceObject5 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject6 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject7 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject8 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Me.sccMain = New DevExpress.XtraEditors.SplitContainerControl()
		Me.SplitContainerControl1 = New DevExpress.XtraEditors.SplitContainerControl()
		Me.lstCategorie = New DevExpress.XtraEditors.ListBoxControl()
		Me.grpAnhaenge = New DevExpress.XtraEditors.GroupControl()
		Me.gridDocuments = New DevExpress.XtraGrid.GridControl()
		Me.gvDocuments = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.grpdetails = New DevExpress.XtraEditors.GroupControl()
		Me.pcc_1Filename = New DevExpress.XtraBars.PopupControlContainer(Me.components)
		Me.grdSelectedFile = New DevExpress.XtraGrid.GridControl()
		Me.gvSelectedFile = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.grdFileToSelect = New DevExpress.XtraGrid.GridControl()
		Me.gvFileToSelect = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.btnLoadFilesToSelect = New DevExpress.XtraEditors.SimpleButton()
		Me.btnSend2SortedFiles = New DevExpress.XtraEditors.SimpleButton()
		Me.sbtnCreateOnePDF = New DevExpress.XtraEditors.SimpleButton()
		Me.lblSortierte = New System.Windows.Forms.Label()
		Me.lblUnsortierte = New System.Windows.Forms.Label()
		Me.beFilename2Zip = New DevExpress.XtraEditors.ButtonEdit()
		Me.lblDateiname = New System.Windows.Forms.Label()
		Me.btnDeleteDocument = New DevExpress.XtraEditors.SimpleButton()
		Me.lblDocumentChanged = New DevExpress.XtraEditors.LabelControl()
		Me.lblDocumentCreated = New DevExpress.XtraEditors.LabelControl()
		Me.lblgaendert = New DevExpress.XtraEditors.LabelControl()
		Me.lblerstellt = New DevExpress.XtraEditors.LabelControl()
		Me.btnNewDocument = New DevExpress.XtraEditors.SimpleButton()
		Me.btnSave = New DevExpress.XtraEditors.SimpleButton()
		Me.lueCategory = New DevExpress.XtraEditors.LookUpEdit()
		Me.lblkategorie = New DevExpress.XtraEditors.LabelControl()
		Me.lblBeschreibung = New DevExpress.XtraEditors.LabelControl()
		Me.txtDescription = New DevExpress.XtraEditors.MemoEdit()
		Me.lblDatei = New DevExpress.XtraEditors.LabelControl()
		Me.txtTitle = New DevExpress.XtraEditors.TextEdit()
		Me.lblBezeichnung = New DevExpress.XtraEditors.LabelControl()
		Me.txtFilePath = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
		Me.errorProviderDocumentMangement = New System.Windows.Forms.ErrorProvider(Me.components)
		CType(Me.sccMain, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.sccMain.SuspendLayout()
		CType(Me.SplitContainerControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SplitContainerControl1.SuspendLayout()
		CType(Me.lstCategorie, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.grpAnhaenge, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grpAnhaenge.SuspendLayout()
		CType(Me.gridDocuments, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvDocuments, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.grpdetails, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grpdetails.SuspendLayout()
		CType(Me.pcc_1Filename, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.pcc_1Filename.SuspendLayout()
		CType(Me.grdSelectedFile, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvSelectedFile, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.grdFileToSelect, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvFileToSelect, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.beFilename2Zip.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lueCategory.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtDescription.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtTitle.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtFilePath.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.errorProviderDocumentMangement, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'sccMain
		'
		Me.sccMain.AllowDrop = True
		Me.sccMain.Dock = System.Windows.Forms.DockStyle.Fill
		Me.sccMain.Horizontal = False
		Me.sccMain.Location = New System.Drawing.Point(5, 5)
		Me.sccMain.Name = "sccMain"
		Me.sccMain.Panel1.Controls.Add(Me.SplitContainerControl1)
		Me.sccMain.Panel1.Text = "Panel1"
		Me.sccMain.Panel2.Controls.Add(Me.grpdetails)
		Me.sccMain.Panel2.MinSize = 300
		Me.sccMain.Panel2.Padding = New System.Windows.Forms.Padding(5)
		Me.sccMain.Panel2.Text = "Panel2"
		Me.sccMain.Size = New System.Drawing.Size(852, 647)
		Me.sccMain.SplitterPosition = 264
		Me.sccMain.TabIndex = 0
		Me.sccMain.Text = "SplitContainerControl1"
		'
		'SplitContainerControl1
		'
		Me.SplitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.SplitContainerControl1.Location = New System.Drawing.Point(0, 0)
		Me.SplitContainerControl1.Name = "SplitContainerControl1"
		Me.SplitContainerControl1.Padding = New System.Windows.Forms.Padding(5)
		Me.SplitContainerControl1.Panel1.Controls.Add(Me.lstCategorie)
		Me.SplitContainerControl1.Panel1.Text = "Panel1"
		Me.SplitContainerControl1.Panel2.Controls.Add(Me.grpAnhaenge)
		Me.SplitContainerControl1.Panel2.Text = "Panel2"
		Me.SplitContainerControl1.Size = New System.Drawing.Size(852, 264)
		Me.SplitContainerControl1.SplitterPosition = 209
		Me.SplitContainerControl1.TabIndex = 0
		Me.SplitContainerControl1.Text = "SplitContainerControl1"
		'
		'lstCategorie
		'
		Me.lstCategorie.Dock = System.Windows.Forms.DockStyle.Fill
		Me.lstCategorie.Location = New System.Drawing.Point(0, 0)
		Me.lstCategorie.Name = "lstCategorie"
		Me.lstCategorie.Size = New System.Drawing.Size(209, 254)
		Me.lstCategorie.TabIndex = 0
		'
		'grpAnhaenge
		'
		Me.grpAnhaenge.AppearanceCaption.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.grpAnhaenge.AppearanceCaption.Options.UseFont = True
		Me.grpAnhaenge.AppearanceCaption.Options.UseTextOptions = True
		Me.grpAnhaenge.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
		Me.grpAnhaenge.Controls.Add(Me.gridDocuments)
		ButtonImageOptions1.SvgImage = Global.SP.MA.DocumentMng.My.Resources.Resources.sendpdf
		ButtonImageOptions2.SvgImage = Global.SP.MA.DocumentMng.My.Resources.Resources.finishmerge
		Me.grpAnhaenge.CustomHeaderButtons.AddRange(New DevExpress.XtraEditors.ButtonPanel.IBaseButton() {New DevExpress.XtraEditors.ButtonsPanelControl.GroupBoxButton("", True, ButtonImageOptions1, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Dokumente per Mail senden", -1, True, Nothing, True, False, True, Nothing, 0), New DevExpress.XtraEditors.ButtonsPanelControl.GroupBoxButton("", True, ButtonImageOptions2, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Dateien zusammenführen", -1, True, Nothing, True, False, True, Nothing, 1)})
		Me.grpAnhaenge.CustomHeaderButtonsLocation = DevExpress.Utils.GroupElementLocation.AfterText
		Me.grpAnhaenge.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grpAnhaenge.Location = New System.Drawing.Point(0, 0)
		Me.grpAnhaenge.Name = "grpAnhaenge"
		Me.grpAnhaenge.Padding = New System.Windows.Forms.Padding(5)
		Me.grpAnhaenge.Size = New System.Drawing.Size(623, 254)
		Me.grpAnhaenge.TabIndex = 11
		'
		'gridDocuments
		'
		Me.gridDocuments.Dock = System.Windows.Forms.DockStyle.Fill
		Me.gridDocuments.Location = New System.Drawing.Point(7, 42)
		Me.gridDocuments.MainView = Me.gvDocuments
		Me.gridDocuments.Name = "gridDocuments"
		Me.gridDocuments.Size = New System.Drawing.Size(609, 205)
		Me.gridDocuments.TabIndex = 1
		Me.gridDocuments.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvDocuments})
		'
		'gvDocuments
		'
		Me.gvDocuments.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvDocuments.GridControl = Me.gridDocuments
		Me.gvDocuments.Name = "gvDocuments"
		Me.gvDocuments.OptionsBehavior.Editable = False
		Me.gvDocuments.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvDocuments.OptionsView.ShowGroupPanel = False
		'
		'grpdetails
		'
		Me.grpdetails.AllowDrop = True
		Me.grpdetails.Controls.Add(Me.pcc_1Filename)
		Me.grpdetails.Controls.Add(Me.btnDeleteDocument)
		Me.grpdetails.Controls.Add(Me.lblDocumentChanged)
		Me.grpdetails.Controls.Add(Me.lblDocumentCreated)
		Me.grpdetails.Controls.Add(Me.lblgaendert)
		Me.grpdetails.Controls.Add(Me.lblerstellt)
		Me.grpdetails.Controls.Add(Me.btnNewDocument)
		Me.grpdetails.Controls.Add(Me.btnSave)
		Me.grpdetails.Controls.Add(Me.lueCategory)
		Me.grpdetails.Controls.Add(Me.lblkategorie)
		Me.grpdetails.Controls.Add(Me.lblBeschreibung)
		Me.grpdetails.Controls.Add(Me.txtDescription)
		Me.grpdetails.Controls.Add(Me.lblDatei)
		Me.grpdetails.Controls.Add(Me.txtTitle)
		Me.grpdetails.Controls.Add(Me.lblBezeichnung)
		Me.grpdetails.Controls.Add(Me.txtFilePath)
		Me.grpdetails.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grpdetails.Location = New System.Drawing.Point(5, 5)
		Me.grpdetails.Name = "grpdetails"
		Me.grpdetails.Size = New System.Drawing.Size(842, 363)
		Me.grpdetails.TabIndex = 2
		Me.grpdetails.Text = "Details"
		'
		'pcc_1Filename
		'
		Me.pcc_1Filename.AutoSize = True
		Me.pcc_1Filename.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
		Me.pcc_1Filename.Controls.Add(Me.grdSelectedFile)
		Me.pcc_1Filename.Controls.Add(Me.grdFileToSelect)
		Me.pcc_1Filename.Controls.Add(Me.btnLoadFilesToSelect)
		Me.pcc_1Filename.Controls.Add(Me.btnSend2SortedFiles)
		Me.pcc_1Filename.Controls.Add(Me.sbtnCreateOnePDF)
		Me.pcc_1Filename.Controls.Add(Me.lblSortierte)
		Me.pcc_1Filename.Controls.Add(Me.lblUnsortierte)
		Me.pcc_1Filename.Controls.Add(Me.beFilename2Zip)
		Me.pcc_1Filename.Controls.Add(Me.lblDateiname)
		Me.pcc_1Filename.Location = New System.Drawing.Point(310, 23)
		Me.pcc_1Filename.Name = "pcc_1Filename"
		Me.pcc_1Filename.ShowCloseButton = True
		Me.pcc_1Filename.Size = New System.Drawing.Size(349, 324)
		Me.pcc_1Filename.TabIndex = 150
		Me.pcc_1Filename.Visible = False
		'
		'grdSelectedFile
		'
		Me.grdSelectedFile.Location = New System.Drawing.Point(190, 32)
		Me.grdSelectedFile.MainView = Me.gvSelectedFile
		Me.grdSelectedFile.Name = "grdSelectedFile"
		Me.grdSelectedFile.Size = New System.Drawing.Size(149, 222)
		Me.grdSelectedFile.TabIndex = 160
		Me.grdSelectedFile.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvSelectedFile})
		'
		'gvSelectedFile
		'
		Me.gvSelectedFile.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvSelectedFile.GridControl = Me.grdSelectedFile
		Me.gvSelectedFile.Name = "gvSelectedFile"
		Me.gvSelectedFile.OptionsBehavior.Editable = False
		Me.gvSelectedFile.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvSelectedFile.OptionsView.ShowGroupPanel = False
		'
		'grdFileToSelect
		'
		Me.grdFileToSelect.Location = New System.Drawing.Point(9, 32)
		Me.grdFileToSelect.MainView = Me.gvFileToSelect
		Me.grdFileToSelect.Name = "grdFileToSelect"
		Me.grdFileToSelect.Size = New System.Drawing.Size(149, 222)
		Me.grdFileToSelect.TabIndex = 159
		Me.grdFileToSelect.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvFileToSelect})
		'
		'gvFileToSelect
		'
		Me.gvFileToSelect.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvFileToSelect.GridControl = Me.grdFileToSelect
		Me.gvFileToSelect.Name = "gvFileToSelect"
		Me.gvFileToSelect.OptionsBehavior.Editable = False
		Me.gvFileToSelect.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvFileToSelect.OptionsView.ShowGroupPanel = False
		'
		'btnLoadFilesToSelect
		'
		Me.btnLoadFilesToSelect.Location = New System.Drawing.Point(164, 228)
		Me.btnLoadFilesToSelect.Name = "btnLoadFilesToSelect"
		Me.btnLoadFilesToSelect.Size = New System.Drawing.Size(20, 26)
		Me.btnLoadFilesToSelect.TabIndex = 156
		Me.btnLoadFilesToSelect.Text = "<<"
		'
		'btnSend2SortedFiles
		'
		Me.btnSend2SortedFiles.Location = New System.Drawing.Point(164, 32)
		Me.btnSend2SortedFiles.Name = "btnSend2SortedFiles"
		Me.btnSend2SortedFiles.Size = New System.Drawing.Size(20, 26)
		Me.btnSend2SortedFiles.TabIndex = 155
		Me.btnSend2SortedFiles.Text = ">"
		'
		'sbtnCreateOnePDF
		'
		Me.sbtnCreateOnePDF.AutoWidthInLayoutControl = True
		Me.sbtnCreateOnePDF.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.sbtnCreateOnePDF.Location = New System.Drawing.Point(0, 290)
		Me.sbtnCreateOnePDF.Name = "sbtnCreateOnePDF"
		Me.sbtnCreateOnePDF.Size = New System.Drawing.Size(349, 34)
		Me.sbtnCreateOnePDF.TabIndex = 154
		Me.sbtnCreateOnePDF.Text = "Ausgewählte Daten in einer PDF-Datei erstellen"
		'
		'lblSortierte
		'
		Me.lblSortierte.AutoSize = True
		Me.lblSortierte.Location = New System.Drawing.Point(187, 14)
		Me.lblSortierte.Name = "lblSortierte"
		Me.lblSortierte.Size = New System.Drawing.Size(89, 13)
		Me.lblSortierte.TabIndex = 153
		Me.lblSortierte.Text = "Sortierte Dateien"
		'
		'lblUnsortierte
		'
		Me.lblUnsortierte.AutoSize = True
		Me.lblUnsortierte.Location = New System.Drawing.Point(6, 14)
		Me.lblUnsortierte.Name = "lblUnsortierte"
		Me.lblUnsortierte.Size = New System.Drawing.Size(101, 13)
		Me.lblUnsortierte.TabIndex = 152
		Me.lblUnsortierte.Text = "Unsortierte Dateien"
		'
		'beFilename2Zip
		'
		Me.beFilename2Zip.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.beFilename2Zip.EditValue = ""
		Me.beFilename2Zip.Location = New System.Drawing.Point(64, 260)
		Me.beFilename2Zip.Name = "beFilename2Zip"
		Me.beFilename2Zip.Properties.NullText = "Dateiname zum Komprimieren..."
		Me.beFilename2Zip.Size = New System.Drawing.Size(274, 20)
		Me.beFilename2Zip.TabIndex = 148
		'
		'lblDateiname
		'
		Me.lblDateiname.AutoSize = True
		Me.lblDateiname.Location = New System.Drawing.Point(6, 263)
		Me.lblDateiname.Name = "lblDateiname"
		Me.lblDateiname.Size = New System.Drawing.Size(58, 13)
		Me.lblDateiname.TabIndex = 149
		Me.lblDateiname.Text = "Dateiname"
		'
		'btnDeleteDocument
		'
		Me.btnDeleteDocument.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnDeleteDocument.ImageOptions.Image = CType(resources.GetObject("btnDeleteDocument.ImageOptions.Image"), System.Drawing.Image)
		Me.btnDeleteDocument.Location = New System.Drawing.Point(713, 142)
		Me.btnDeleteDocument.Name = "btnDeleteDocument"
		Me.btnDeleteDocument.Size = New System.Drawing.Size(105, 28)
		Me.btnDeleteDocument.TabIndex = 8
		Me.btnDeleteDocument.Text = "Löschen"
		'
		'lblDocumentChanged
		'
		Me.lblDocumentChanged.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.lblDocumentChanged.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblDocumentChanged.Location = New System.Drawing.Point(141, 334)
		Me.lblDocumentChanged.Name = "lblDocumentChanged"
		Me.lblDocumentChanged.Size = New System.Drawing.Size(682, 13)
		Me.lblDocumentChanged.TabIndex = 32
		Me.lblDocumentChanged.Text = "Geändert:"
		'
		'lblDocumentCreated
		'
		Me.lblDocumentCreated.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.lblDocumentCreated.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblDocumentCreated.Location = New System.Drawing.Point(141, 314)
		Me.lblDocumentCreated.Name = "lblDocumentCreated"
		Me.lblDocumentCreated.Size = New System.Drawing.Size(682, 13)
		Me.lblDocumentCreated.TabIndex = 31
		Me.lblDocumentCreated.Text = "Erstellt:"
		'
		'lblgaendert
		'
		Me.lblgaendert.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.lblgaendert.Appearance.Options.UseTextOptions = True
		Me.lblgaendert.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblgaendert.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblgaendert.Location = New System.Drawing.Point(58, 334)
		Me.lblgaendert.Name = "lblgaendert"
		Me.lblgaendert.Size = New System.Drawing.Size(77, 13)
		Me.lblgaendert.TabIndex = 30
		Me.lblgaendert.Text = "Geändert"
		'
		'lblerstellt
		'
		Me.lblerstellt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.lblerstellt.Appearance.Options.UseTextOptions = True
		Me.lblerstellt.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblerstellt.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblerstellt.Location = New System.Drawing.Point(58, 314)
		Me.lblerstellt.Name = "lblerstellt"
		Me.lblerstellt.Size = New System.Drawing.Size(77, 13)
		Me.lblerstellt.TabIndex = 29
		Me.lblerstellt.Text = "Erstellt"
		'
		'btnNewDocument
		'
		Me.btnNewDocument.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnNewDocument.ImageOptions.Image = CType(resources.GetObject("btnNewDocument.ImageOptions.Image"), System.Drawing.Image)
		Me.btnNewDocument.Location = New System.Drawing.Point(713, 104)
		Me.btnNewDocument.Name = "btnNewDocument"
		Me.btnNewDocument.Size = New System.Drawing.Size(105, 28)
		Me.btnNewDocument.TabIndex = 7
		Me.btnNewDocument.Text = "Neu"
		'
		'btnSave
		'
		Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnSave.ImageOptions.Image = CType(resources.GetObject("btnSave.ImageOptions.Image"), System.Drawing.Image)
		Me.btnSave.Location = New System.Drawing.Point(713, 66)
		Me.btnSave.Name = "btnSave"
		Me.btnSave.Size = New System.Drawing.Size(105, 28)
		Me.btnSave.TabIndex = 6
		Me.btnSave.Text = "Speichern"
		'
		'lueCategory
		'
		Me.lueCategory.Location = New System.Drawing.Point(141, 40)
		Me.lueCategory.Name = "lueCategory"
		SerializableAppearanceObject1.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject1.Options.UseForeColor = True
		SerializableAppearanceObject2.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject2.Options.UseForeColor = True
		SerializableAppearanceObject3.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject3.Options.UseForeColor = True
		SerializableAppearanceObject4.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject4.Options.UseForeColor = True
		Me.lueCategory.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
		Me.lueCategory.Properties.ShowFooter = False
		Me.lueCategory.Size = New System.Drawing.Size(182, 20)
		Me.lueCategory.TabIndex = 2
		'
		'lblkategorie
		'
		Me.lblkategorie.Appearance.Options.UseTextOptions = True
		Me.lblkategorie.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblkategorie.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblkategorie.Location = New System.Drawing.Point(16, 44)
		Me.lblkategorie.Name = "lblkategorie"
		Me.lblkategorie.Size = New System.Drawing.Size(119, 13)
		Me.lblkategorie.TabIndex = 9
		Me.lblkategorie.Text = "Kategorie"
		'
		'lblBeschreibung
		'
		Me.lblBeschreibung.Appearance.Options.UseTextOptions = True
		Me.lblBeschreibung.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblBeschreibung.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblBeschreibung.Location = New System.Drawing.Point(16, 92)
		Me.lblBeschreibung.Name = "lblBeschreibung"
		Me.lblBeschreibung.Size = New System.Drawing.Size(119, 13)
		Me.lblBeschreibung.TabIndex = 7
		Me.lblBeschreibung.Text = "Beschreibung"
		'
		'txtDescription
		'
		Me.txtDescription.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txtDescription.Location = New System.Drawing.Point(141, 92)
		Me.txtDescription.Name = "txtDescription"
		Me.txtDescription.Size = New System.Drawing.Size(550, 98)
		Me.txtDescription.TabIndex = 4
		'
		'lblDatei
		'
		Me.lblDatei.Appearance.Options.UseTextOptions = True
		Me.lblDatei.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblDatei.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblDatei.Location = New System.Drawing.Point(16, 198)
		Me.lblDatei.Name = "lblDatei"
		Me.lblDatei.Size = New System.Drawing.Size(119, 13)
		Me.lblDatei.TabIndex = 5
		Me.lblDatei.Text = "Datei"
		'
		'txtTitle
		'
		Me.txtTitle.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txtTitle.Location = New System.Drawing.Point(141, 66)
		Me.txtTitle.Name = "txtTitle"
		Me.txtTitle.Size = New System.Drawing.Size(550, 20)
		Me.txtTitle.TabIndex = 3
		'
		'lblBezeichnung
		'
		Me.lblBezeichnung.Appearance.Options.UseTextOptions = True
		Me.lblBezeichnung.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblBezeichnung.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblBezeichnung.Location = New System.Drawing.Point(16, 70)
		Me.lblBezeichnung.Name = "lblBezeichnung"
		Me.lblBezeichnung.Size = New System.Drawing.Size(119, 13)
		Me.lblBezeichnung.TabIndex = 0
		Me.lblBezeichnung.Text = "Bezeichnung"
		'
		'txtFilePath
		'
		Me.txtFilePath.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txtFilePath.Location = New System.Drawing.Point(141, 196)
		Me.txtFilePath.Name = "txtFilePath"
		EditorButtonImageOptions2.Image = Global.SP.MA.DocumentMng.My.Resources.Resources.pdf
		Me.txtFilePath.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, True, True, False, EditorButtonImageOptions2, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject5, SerializableAppearanceObject6, SerializableAppearanceObject7, SerializableAppearanceObject8, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
		Me.txtFilePath.Size = New System.Drawing.Size(550, 23)
		Me.txtFilePath.TabIndex = 5
		'
		'OpenFileDialog1
		'
		Me.OpenFileDialog1.FileName = "OpenFileDialog1"
		'
		'errorProviderDocumentMangement
		'
		Me.errorProviderDocumentMangement.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
		Me.errorProviderDocumentMangement.ContainerControl = Me
		'
		'frmDoc
		'
		Me.AllowDrop = True
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(862, 657)
		Me.Controls.Add(Me.sccMain)
		Me.IconOptions.Icon = CType(resources.GetObject("frmDoc.IconOptions.Icon"), System.Drawing.Icon)
		Me.MinimumSize = New System.Drawing.Size(682, 598)
		Me.Name = "frmDoc"
		Me.Padding = New System.Windows.Forms.Padding(5)
		Me.Text = "Dokumentverwaltung"
		CType(Me.sccMain, System.ComponentModel.ISupportInitialize).EndInit()
		Me.sccMain.ResumeLayout(False)
		CType(Me.SplitContainerControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.SplitContainerControl1.ResumeLayout(False)
		CType(Me.lstCategorie, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.grpAnhaenge, System.ComponentModel.ISupportInitialize).EndInit()
		Me.grpAnhaenge.ResumeLayout(False)
		CType(Me.gridDocuments, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvDocuments, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.grpdetails, System.ComponentModel.ISupportInitialize).EndInit()
		Me.grpdetails.ResumeLayout(False)
		Me.grpdetails.PerformLayout()
		CType(Me.pcc_1Filename, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pcc_1Filename.ResumeLayout(False)
		Me.pcc_1Filename.PerformLayout()
		CType(Me.grdSelectedFile, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvSelectedFile, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.grdFileToSelect, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvFileToSelect, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.beFilename2Zip.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lueCategory.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtDescription.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtTitle.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtFilePath.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.errorProviderDocumentMangement, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents sccMain As DevExpress.XtraEditors.SplitContainerControl
  Friend WithEvents SplitContainerControl1 As DevExpress.XtraEditors.SplitContainerControl
  Friend WithEvents lstCategorie As DevExpress.XtraEditors.ListBoxControl
  Friend WithEvents gridDocuments As DevExpress.XtraGrid.GridControl
  Friend WithEvents gvDocuments As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents grpdetails As DevExpress.XtraEditors.GroupControl
  Friend WithEvents lblBeschreibung As DevExpress.XtraEditors.LabelControl
  Friend WithEvents txtDescription As DevExpress.XtraEditors.MemoEdit
  Friend WithEvents lblDatei As DevExpress.XtraEditors.LabelControl
  Friend WithEvents txtTitle As DevExpress.XtraEditors.TextEdit
  Friend WithEvents lblBezeichnung As DevExpress.XtraEditors.LabelControl
  Friend WithEvents lueCategory As DevExpress.XtraEditors.LookUpEdit
  Friend WithEvents lblkategorie As DevExpress.XtraEditors.LabelControl
  Friend WithEvents lblDocumentChanged As DevExpress.XtraEditors.LabelControl
  Friend WithEvents lblDocumentCreated As DevExpress.XtraEditors.LabelControl
  Friend WithEvents lblgaendert As DevExpress.XtraEditors.LabelControl
  Friend WithEvents lblerstellt As DevExpress.XtraEditors.LabelControl
  Friend WithEvents btnNewDocument As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnSave As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
  Friend WithEvents errorProviderDocumentMangement As System.Windows.Forms.ErrorProvider
  Friend WithEvents btnDeleteDocument As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents txtFilePath As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents grpAnhaenge As DevExpress.XtraEditors.GroupControl
	Friend WithEvents pcc_1Filename As DevExpress.XtraBars.PopupControlContainer
	Friend WithEvents grdSelectedFile As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvSelectedFile As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents grdFileToSelect As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvFileToSelect As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents btnLoadFilesToSelect As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents btnSend2SortedFiles As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents sbtnCreateOnePDF As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents lblSortierte As Label
	Friend WithEvents lblUnsortierte As Label
	Friend WithEvents beFilename2Zip As DevExpress.XtraEditors.ButtonEdit
	Friend WithEvents lblDateiname As Label
End Class
