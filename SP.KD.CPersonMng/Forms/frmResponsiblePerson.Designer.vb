
Namespace UI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmResponsiblePerson
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
			Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmResponsiblePerson))
			Me.grpZustaendigeperson = New DevExpress.XtraEditors.GroupControl()
			Me.lbLstlInfo = New DevExpress.XtraEditors.LabelControl()
			Me.gridResponsiblePersons = New DevExpress.XtraGrid.GridControl()
			Me.gvResponsiblePersons = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.sccMain = New DevExpress.XtraEditors.SplitContainerControl()
			Me.XtraTabControl1 = New DevExpress.XtraTab.XtraTabControl()
			Me.xtabCommonData = New DevExpress.XtraTab.XtraTabPage()
			Me.ucCommonData = New SP.KD.CPersonMng.UI.ucCommonData()
			Me.xtabDispositionData = New DevExpress.XtraTab.XtraTabPage()
			Me.ucDisposal = New SP.KD.CPersonMng.UI.ucDisposal()
			Me.xtabProfessionsAndSectorsData = New DevExpress.XtraTab.XtraTabPage()
			Me.ucProfessionsAndSectors = New SP.KD.CPersonMng.UI.ucProfessionsAndSectors()
			Me.xtabContacts = New DevExpress.XtraTab.XtraTabPage()
			Me.ucContactData = New SP.KD.CPersonMng.UI.ucContactData()
			Me.xtabDocuments = New DevExpress.XtraTab.XtraTabPage()
			Me.ucDocumentManagement = New SP.KD.CPersonMng.UI.ucDocumentManagement()
			Me.Bar1 = New DevExpress.XtraBars.Bar()
			Me.bsiLblErstellt = New DevExpress.XtraBars.BarStaticItem()
			Me.bsiCreated = New DevExpress.XtraBars.BarStaticItem()
			Me.bsiLblGeaendert = New DevExpress.XtraBars.BarStaticItem()
			Me.bsiChanged = New DevExpress.XtraBars.BarStaticItem()
			Me.bbiSave = New DevExpress.XtraBars.BarButtonItem()
			Me.bbiDelete = New DevExpress.XtraBars.BarButtonItem()
			Me.bbiNew = New DevExpress.XtraBars.BarButtonItem()
			Me.bbiPrint = New DevExpress.XtraBars.BarButtonItem()
			Me.bbiOutlookContact = New DevExpress.XtraBars.BarButtonItem()
			Me.bbiTODO = New DevExpress.XtraBars.BarButtonItem()
			Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
			Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
			Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
			Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
			Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
			Me.btnShowTemplate = New DevExpress.XtraBars.BarSubItem()
			Me.RepositoryItemFontEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemFontEdit()
			Me.RepositoryItemComboBox1 = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
			Me.RepositoryItemComboBox2 = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
			Me.RepositoryItemPictureEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit()
			CType(Me.grpZustaendigeperson, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.grpZustaendigeperson.SuspendLayout()
			CType(Me.gridResponsiblePersons, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvResponsiblePersons, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.sccMain, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.sccMain.SuspendLayout()
			CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.XtraTabControl1.SuspendLayout()
			Me.xtabCommonData.SuspendLayout()
			Me.xtabDispositionData.SuspendLayout()
			Me.xtabProfessionsAndSectorsData.SuspendLayout()
			Me.xtabContacts.SuspendLayout()
			Me.xtabDocuments.SuspendLayout()
			CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.RepositoryItemFontEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.RepositoryItemComboBox1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.RepositoryItemComboBox2, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.RepositoryItemPictureEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			'
			'grpZustaendigeperson
			'
			Me.grpZustaendigeperson.Appearance.Options.UseTextOptions = True
			Me.grpZustaendigeperson.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
			Me.grpZustaendigeperson.AppearanceCaption.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grpZustaendigeperson.AppearanceCaption.Options.UseFont = True
			Me.grpZustaendigeperson.AppearanceCaption.Options.UseTextOptions = True
			Me.grpZustaendigeperson.AppearanceCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
			Me.grpZustaendigeperson.Controls.Add(Me.lbLstlInfo)
			Me.grpZustaendigeperson.Controls.Add(Me.gridResponsiblePersons)
			Me.grpZustaendigeperson.CustomHeaderButtonsLocation = DevExpress.Utils.GroupElementLocation.AfterText
			Me.grpZustaendigeperson.Dock = System.Windows.Forms.DockStyle.Fill
			Me.grpZustaendigeperson.Location = New System.Drawing.Point(5, 5)
			Me.grpZustaendigeperson.Name = "grpZustaendigeperson"
			Me.grpZustaendigeperson.Size = New System.Drawing.Size(259, 655)
			Me.grpZustaendigeperson.TabIndex = 165
			Me.grpZustaendigeperson.Text = "Zuständige Personen"
			'
			'lbLstlInfo
			'
			Me.lbLstlInfo.AllowHtmlString = True
			Me.lbLstlInfo.AllowHtmlTextInToolTip = DevExpress.Utils.DefaultBoolean.[True]
			Me.lbLstlInfo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
			Me.lbLstlInfo.Appearance.BackColor = System.Drawing.Color.Transparent
			Me.lbLstlInfo.Appearance.Image = CType(resources.GetObject("lbLstlInfo.Appearance.Image"), System.Drawing.Image)
			Me.lbLstlInfo.Appearance.Options.UseBackColor = True
			Me.lbLstlInfo.Appearance.Options.UseImage = True
			Me.lbLstlInfo.Appearance.Options.UseTextOptions = True
			Me.lbLstlInfo.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter
			Me.lbLstlInfo.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Bottom
			Me.lbLstlInfo.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
			Me.lbLstlInfo.AutoEllipsis = True
			Me.lbLstlInfo.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lbLstlInfo.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftTop
			Me.lbLstlInfo.LineColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
			Me.lbLstlInfo.LineLocation = DevExpress.XtraEditors.LineLocation.Top
			Me.lbLstlInfo.LineOrientation = DevExpress.XtraEditors.LabelLineOrientation.Vertical
			Me.lbLstlInfo.LineVisible = True
			Me.lbLstlInfo.Location = New System.Drawing.Point(7, 589)
			Me.lbLstlInfo.Name = "lbLstlInfo"
			Me.lbLstlInfo.Size = New System.Drawing.Size(248, 59)
			Me.lbLstlInfo.TabIndex = 292
			Me.lbLstlInfo.Text = "<b>Mehrfachauswahl</b>" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Mehrfachauswahl ist nur für Listen-Druck gültig!"
			'
			'gridResponsiblePersons
			'
			Me.gridResponsiblePersons.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.gridResponsiblePersons.Location = New System.Drawing.Point(2, 29)
			Me.gridResponsiblePersons.MainView = Me.gvResponsiblePersons
			Me.gridResponsiblePersons.Name = "gridResponsiblePersons"
			Me.gridResponsiblePersons.Size = New System.Drawing.Size(255, 556)
			Me.gridResponsiblePersons.TabIndex = 2
			Me.gridResponsiblePersons.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvResponsiblePersons})
			'
			'gvResponsiblePersons
			'
			Me.gvResponsiblePersons.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			Me.gvResponsiblePersons.GridControl = Me.gridResponsiblePersons
			Me.gvResponsiblePersons.Name = "gvResponsiblePersons"
			Me.gvResponsiblePersons.OptionsBehavior.Editable = False
			Me.gvResponsiblePersons.OptionsSelection.EnableAppearanceFocusedCell = False
			Me.gvResponsiblePersons.OptionsView.ShowGroupPanel = False
			'
			'sccMain
			'
			Me.sccMain.Dock = System.Windows.Forms.DockStyle.Fill
			Me.sccMain.Location = New System.Drawing.Point(0, 0)
			Me.sccMain.Name = "sccMain"
			Me.sccMain.Panel1.Controls.Add(Me.grpZustaendigeperson)
			Me.sccMain.Panel1.MinSize = 200
			Me.sccMain.Panel1.Padding = New System.Windows.Forms.Padding(5)
			Me.sccMain.Panel1.Text = "Panel1"
			Me.sccMain.Panel2.Controls.Add(Me.XtraTabControl1)
			Me.sccMain.Panel2.Padding = New System.Windows.Forms.Padding(5)
			Me.sccMain.Panel2.Text = "Panel2"
			Me.sccMain.Size = New System.Drawing.Size(1123, 665)
			Me.sccMain.SplitterPosition = 269
			Me.sccMain.TabIndex = 166
			Me.sccMain.Text = "SplitContainerControl1"
			'
			'XtraTabControl1
			'
			Me.XtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.XtraTabControl1.Location = New System.Drawing.Point(5, 5)
			Me.XtraTabControl1.Name = "XtraTabControl1"
			Me.XtraTabControl1.SelectedTabPage = Me.xtabCommonData
			Me.XtraTabControl1.Size = New System.Drawing.Size(839, 655)
			Me.XtraTabControl1.TabIndex = 0
			Me.XtraTabControl1.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabCommonData, Me.xtabDispositionData, Me.xtabProfessionsAndSectorsData, Me.xtabContacts, Me.xtabDocuments})
			'
			'xtabCommonData
			'
			Me.xtabCommonData.Controls.Add(Me.ucCommonData)
			Me.xtabCommonData.Name = "xtabCommonData"
			Me.xtabCommonData.Size = New System.Drawing.Size(833, 627)
			Me.xtabCommonData.Text = "Allgemein"
			'
			'ucCommonData
			'
			Me.ucCommonData.Appearance.BackColor = System.Drawing.Color.Transparent
			Me.ucCommonData.Appearance.Options.UseBackColor = True
			Me.ucCommonData.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucCommonData.IsIntialControlDataLoaded = False
			Me.ucCommonData.Location = New System.Drawing.Point(0, 0)
			Me.ucCommonData.Name = "ucCommonData"
			Me.ucCommonData.Size = New System.Drawing.Size(833, 627)
			Me.ucCommonData.TabIndex = 0
			'
			'xtabDispositionData
			'
			Me.xtabDispositionData.Controls.Add(Me.ucDisposal)
			Me.xtabDispositionData.Name = "xtabDispositionData"
			Me.xtabDispositionData.Size = New System.Drawing.Size(833, 627)
			Me.xtabDispositionData.Text = "Disposition"
			'
			'ucDisposal
			'
			Me.ucDisposal.Appearance.BackColor = System.Drawing.Color.Transparent
			Me.ucDisposal.Appearance.Options.UseBackColor = True
			Me.ucDisposal.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucDisposal.IsIntialControlDataLoaded = False
			Me.ucDisposal.Location = New System.Drawing.Point(0, 0)
			Me.ucDisposal.Name = "ucDisposal"
			Me.ucDisposal.Size = New System.Drawing.Size(833, 627)
			Me.ucDisposal.TabIndex = 0
			'
			'xtabProfessionsAndSectorsData
			'
			Me.xtabProfessionsAndSectorsData.Controls.Add(Me.ucProfessionsAndSectors)
			Me.xtabProfessionsAndSectorsData.Name = "xtabProfessionsAndSectorsData"
			Me.xtabProfessionsAndSectorsData.Size = New System.Drawing.Size(833, 627)
			Me.xtabProfessionsAndSectorsData.Text = "Berufe und Branchen"
			'
			'ucProfessionsAndSectors
			'
			Me.ucProfessionsAndSectors.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucProfessionsAndSectors.IsIntialControlDataLoaded = False
			Me.ucProfessionsAndSectors.Location = New System.Drawing.Point(0, 0)
			Me.ucProfessionsAndSectors.Name = "ucProfessionsAndSectors"
			Me.ucProfessionsAndSectors.Size = New System.Drawing.Size(833, 627)
			Me.ucProfessionsAndSectors.TabIndex = 0
			'
			'xtabContacts
			'
			Me.xtabContacts.Controls.Add(Me.ucContactData)
			Me.xtabContacts.Name = "xtabContacts"
			Me.xtabContacts.Size = New System.Drawing.Size(833, 627)
			Me.xtabContacts.Text = "Kontaktverwaltung"
			'
			'ucContactData
			'
			Me.ucContactData.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucContactData.IsIntialControlDataLoaded = False
			Me.ucContactData.Location = New System.Drawing.Point(0, 0)
			Me.ucContactData.Name = "ucContactData"
			Me.ucContactData.Padding = New System.Windows.Forms.Padding(5)
			Me.ucContactData.Size = New System.Drawing.Size(833, 627)
			Me.ucContactData.TabIndex = 0
			'
			'xtabDocuments
			'
			Me.xtabDocuments.Controls.Add(Me.ucDocumentManagement)
			Me.xtabDocuments.Name = "xtabDocuments"
			Me.xtabDocuments.Size = New System.Drawing.Size(833, 627)
			Me.xtabDocuments.Text = "Dokumentenverwaltung"
			'
			'ucDocumentManagement
			'
			Me.ucDocumentManagement.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucDocumentManagement.IsIntialControlDataLoaded = False
			Me.ucDocumentManagement.Location = New System.Drawing.Point(0, 0)
			Me.ucDocumentManagement.Name = "ucDocumentManagement"
			Me.ucDocumentManagement.Padding = New System.Windows.Forms.Padding(5)
			Me.ucDocumentManagement.Size = New System.Drawing.Size(833, 627)
			Me.ucDocumentManagement.TabIndex = 0
			'
			'Bar1
			'
			Me.Bar1.BarName = "Benutzerdefiniert 2"
			Me.Bar1.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
			Me.Bar1.DockCol = 0
			Me.Bar1.DockRow = 0
			Me.Bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
			Me.Bar1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiLblErstellt, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bsiCreated), New DevExpress.XtraBars.LinkPersistInfo(Me.bsiLblGeaendert), New DevExpress.XtraBars.LinkPersistInfo(Me.bsiChanged), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiSave, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiDelete, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiNew, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiPrint, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiOutlookContact, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiTODO)})
			Me.Bar1.OptionsBar.AllowQuickCustomization = False
			Me.Bar1.OptionsBar.DrawDragBorder = False
			Me.Bar1.OptionsBar.UseWholeRow = True
			Me.Bar1.Text = "Benutzerdefiniert 2"
			'
			'bsiLblErstellt
			'
			Me.bsiLblErstellt.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
			Me.bsiLblErstellt.Caption = "Erstellt:"
			Me.bsiLblErstellt.Id = 60
			Me.bsiLblErstellt.ItemAppearance.Normal.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.bsiLblErstellt.ItemAppearance.Normal.Options.UseFont = True
			Me.bsiLblErstellt.Name = "bsiLblErstellt"
			'
			'bsiCreated
			'
			Me.bsiCreated.AutoSize = DevExpress.XtraBars.BarStaticItemSize.Spring
			Me.bsiCreated.Caption = "{0}"
			Me.bsiCreated.Id = 61
			Me.bsiCreated.Name = "bsiCreated"
			'
			'bsiLblGeaendert
			'
			Me.bsiLblGeaendert.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
			Me.bsiLblGeaendert.Caption = "Geändert:"
			Me.bsiLblGeaendert.Id = 62
			Me.bsiLblGeaendert.ItemAppearance.Normal.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.bsiLblGeaendert.ItemAppearance.Normal.Options.UseFont = True
			Me.bsiLblGeaendert.Name = "bsiLblGeaendert"
			'
			'bsiChanged
			'
			Me.bsiChanged.AutoSize = DevExpress.XtraBars.BarStaticItemSize.Spring
			Me.bsiChanged.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
			Me.bsiChanged.Caption = "{0}"
			Me.bsiChanged.Id = 63
			Me.bsiChanged.Name = "bsiChanged"
			'
			'bbiSave
			'
			Me.bbiSave.Caption = "Daten sichern"
			Me.bbiSave.Id = 65
			Me.bbiSave.ImageOptions.Image = CType(resources.GetObject("bbiSave.ImageOptions.Image"), System.Drawing.Image)
			Me.bbiSave.Name = "bbiSave"
			Me.bbiSave.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
			'
			'bbiDelete
			'
			Me.bbiDelete.Caption = "Löschen"
			Me.bbiDelete.Id = 69
			Me.bbiDelete.ImageOptions.Image = CType(resources.GetObject("bbiDelete.ImageOptions.Image"), System.Drawing.Image)
			Me.bbiDelete.ImageOptions.LargeImage = CType(resources.GetObject("bbiDelete.ImageOptions.LargeImage"), System.Drawing.Image)
			Me.bbiDelete.Name = "bbiDelete"
			Me.bbiDelete.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
			'
			'bbiNew
			'
			Me.bbiNew.Caption = "Neu"
			Me.bbiNew.Id = 66
			Me.bbiNew.ImageOptions.Image = Global.SP.KD.CPersonMng.My.Resources.Resources.Plus
			Me.bbiNew.Name = "bbiNew"
			Me.bbiNew.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
			'
			'bbiPrint
			'
			Me.bbiPrint.Caption = "Drucken"
			Me.bbiPrint.Id = 68
			Me.bbiPrint.ImageOptions.Image = CType(resources.GetObject("bbiPrint.ImageOptions.Image"), System.Drawing.Image)
			Me.bbiPrint.Name = "bbiPrint"
			Me.bbiPrint.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
			'
			'bbiOutlookContact
			'
			Me.bbiOutlookContact.Caption = "Adresse ins Outlook exportieren"
			Me.bbiOutlookContact.Id = 70
			Me.bbiOutlookContact.ImageOptions.Image = CType(resources.GetObject("bbiOutlookContact.ImageOptions.Image"), System.Drawing.Image)
			Me.bbiOutlookContact.Name = "bbiOutlookContact"
			Me.bbiOutlookContact.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
			'
			'bbiTODO
			'
			Me.bbiTODO.Caption = "To-do erstellen"
			Me.bbiTODO.Id = 67
			Me.bbiTODO.ImageOptions.Image = CType(resources.GetObject("bbiTODO.ImageOptions.Image"), System.Drawing.Image)
			Me.bbiTODO.Name = "bbiTODO"
			Me.bbiTODO.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
			'
			'BarManager1
			'
			Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar1})
			Me.BarManager1.DockControls.Add(Me.barDockControlTop)
			Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
			Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
			Me.BarManager1.DockControls.Add(Me.barDockControlRight)
			Me.BarManager1.Form = Me
			Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.btnShowTemplate, Me.bsiLblErstellt, Me.bsiCreated, Me.bsiLblGeaendert, Me.bsiChanged, Me.bbiSave, Me.bbiNew, Me.bbiTODO, Me.bbiPrint, Me.bbiDelete, Me.bbiOutlookContact})
			Me.BarManager1.MaxItemId = 71
			Me.BarManager1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemFontEdit1, Me.RepositoryItemComboBox1, Me.RepositoryItemComboBox2, Me.RepositoryItemPictureEdit1})
			Me.BarManager1.StatusBar = Me.Bar1
			'
			'barDockControlTop
			'
			Me.barDockControlTop.CausesValidation = False
			Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
			Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
			Me.barDockControlTop.Manager = Me.BarManager1
			Me.barDockControlTop.Size = New System.Drawing.Size(1123, 0)
			'
			'barDockControlBottom
			'
			Me.barDockControlBottom.CausesValidation = False
			Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
			Me.barDockControlBottom.Location = New System.Drawing.Point(0, 665)
			Me.barDockControlBottom.Manager = Me.BarManager1
			Me.barDockControlBottom.Size = New System.Drawing.Size(1123, 27)
			'
			'barDockControlLeft
			'
			Me.barDockControlLeft.CausesValidation = False
			Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
			Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
			Me.barDockControlLeft.Manager = Me.BarManager1
			Me.barDockControlLeft.Size = New System.Drawing.Size(0, 665)
			'
			'barDockControlRight
			'
			Me.barDockControlRight.CausesValidation = False
			Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
			Me.barDockControlRight.Location = New System.Drawing.Point(1123, 0)
			Me.barDockControlRight.Manager = Me.BarManager1
			Me.barDockControlRight.Size = New System.Drawing.Size(0, 665)
			'
			'btnShowTemplate
			'
			Me.btnShowTemplate.Caption = "Vorlage laden"
			Me.btnShowTemplate.Id = 59
			Me.btnShowTemplate.Name = "btnShowTemplate"
			Me.btnShowTemplate.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
			Me.btnShowTemplate.ShowMenuCaption = True
			'
			'RepositoryItemFontEdit1
			'
			Me.RepositoryItemFontEdit1.AutoHeight = False
			Me.RepositoryItemFontEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.RepositoryItemFontEdit1.Name = "RepositoryItemFontEdit1"
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
			'RepositoryItemPictureEdit1
			'
			Me.RepositoryItemPictureEdit1.Name = "RepositoryItemPictureEdit1"
			'
			'frmResponsiblePerson
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(1123, 692)
			Me.Controls.Add(Me.sccMain)
			Me.Controls.Add(Me.barDockControlLeft)
			Me.Controls.Add(Me.barDockControlRight)
			Me.Controls.Add(Me.barDockControlBottom)
			Me.Controls.Add(Me.barDockControlTop)
			Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
			Me.MinimumSize = New System.Drawing.Size(1133, 724)
			Me.Name = "frmResponsiblePerson"
			Me.Text = "Zuständige Personen"
			CType(Me.grpZustaendigeperson, System.ComponentModel.ISupportInitialize).EndInit()
			Me.grpZustaendigeperson.ResumeLayout(False)
			CType(Me.gridResponsiblePersons, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvResponsiblePersons, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.sccMain, System.ComponentModel.ISupportInitialize).EndInit()
			Me.sccMain.ResumeLayout(False)
			CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.XtraTabControl1.ResumeLayout(False)
			Me.xtabCommonData.ResumeLayout(False)
			Me.xtabDispositionData.ResumeLayout(False)
			Me.xtabProfessionsAndSectorsData.ResumeLayout(False)
			Me.xtabContacts.ResumeLayout(False)
			Me.xtabDocuments.ResumeLayout(False)
			CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.RepositoryItemFontEdit1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.RepositoryItemComboBox1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.RepositoryItemComboBox2, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.RepositoryItemPictureEdit1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)
			Me.PerformLayout()

		End Sub
		Friend WithEvents grpZustaendigeperson As DevExpress.XtraEditors.GroupControl
		Friend WithEvents gridResponsiblePersons As DevExpress.XtraGrid.GridControl
		Friend WithEvents gvResponsiblePersons As DevExpress.XtraGrid.Views.Grid.GridView
		Friend WithEvents sccMain As DevExpress.XtraEditors.SplitContainerControl
		Friend WithEvents XtraTabControl1 As DevExpress.XtraTab.XtraTabControl
		Friend WithEvents xtabCommonData As DevExpress.XtraTab.XtraTabPage
		Friend WithEvents xtabDispositionData As DevExpress.XtraTab.XtraTabPage
		Friend WithEvents xtabProfessionsAndSectorsData As DevExpress.XtraTab.XtraTabPage
		Friend WithEvents xtabContacts As DevExpress.XtraTab.XtraTabPage
		Friend WithEvents xtabDocuments As DevExpress.XtraTab.XtraTabPage
		Friend WithEvents ucCommonData As SP.KD.CPersonMng.UI.ucCommonData
		Friend WithEvents ucDisposal As SP.KD.CPersonMng.UI.ucDisposal
		Friend WithEvents ucProfessionsAndSectors As SP.KD.CPersonMng.UI.ucProfessionsAndSectors
		Friend WithEvents ucDocumentManagement As SP.KD.CPersonMng.UI.ucDocumentManagement
		Friend WithEvents ucContactData As SP.KD.CPersonMng.UI.ucContactData
		Friend WithEvents lbLstlInfo As DevExpress.XtraEditors.LabelControl
		Friend WithEvents Bar1 As DevExpress.XtraBars.Bar
		Friend WithEvents bsiLblErstellt As DevExpress.XtraBars.BarStaticItem
		Friend WithEvents bsiCreated As DevExpress.XtraBars.BarStaticItem
		Friend WithEvents bsiLblGeaendert As DevExpress.XtraBars.BarStaticItem
		Friend WithEvents bsiChanged As DevExpress.XtraBars.BarStaticItem
		Friend WithEvents bbiSave As DevExpress.XtraBars.BarButtonItem
		Friend WithEvents bbiNew As DevExpress.XtraBars.BarButtonItem
		Friend WithEvents bbiTODO As DevExpress.XtraBars.BarButtonItem
		Friend WithEvents bbiPrint As DevExpress.XtraBars.BarButtonItem
		Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
		Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
		Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
		Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
		Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
		Friend WithEvents btnShowTemplate As DevExpress.XtraBars.BarSubItem
		Friend WithEvents RepositoryItemFontEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemFontEdit
		Friend WithEvents RepositoryItemComboBox1 As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
		Friend WithEvents RepositoryItemComboBox2 As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
		Friend WithEvents RepositoryItemPictureEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit
		Friend WithEvents bbiDelete As DevExpress.XtraBars.BarButtonItem
		Friend WithEvents bbiOutlookContact As DevExpress.XtraBars.BarButtonItem
	End Class

End Namespace