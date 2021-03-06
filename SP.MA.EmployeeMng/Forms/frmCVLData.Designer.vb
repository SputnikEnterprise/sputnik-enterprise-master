Namespace UI

	<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
	Partial Class frmCVLData
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
			Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCVLData))
			Me.RepositoryItemComboBox2 = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
			Me.RepositoryItemComboBox1 = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
			Me.btnShowTemplate = New DevExpress.XtraBars.BarSubItem()
			Me.xtabMoreinfo = New DevExpress.XtraTab.XtraTabControl()
			Me.xtabApplications = New DevExpress.XtraTab.XtraTabPage()
			Me.ucApplicationData = New SP.MA.EmployeeMng.UI.ucApplicationData()
			Me.xtabWork = New DevExpress.XtraTab.XtraTabPage()
			Me.ucCVLWorkData = New SP.MA.EmployeeMng.UI.ucCVLWork()
			Me.xtabEducation = New DevExpress.XtraTab.XtraTabPage()
			Me.ucCVLEducationData = New SP.MA.EmployeeMng.UI.ucCVLEducation()
			Me.xtabAddInfo = New DevExpress.XtraTab.XtraTabPage()
			Me.ucCVLAdditionalData = New SP.MA.EmployeeMng.UI.ucCVLAdditioinalInformation()
			Me.xtabPublication = New DevExpress.XtraTab.XtraTabPage()
			Me.ucCVLPublicationData = New SP.MA.EmployeeMng.UI.ucCVLPublication()
			Me.libVakAls = New DevExpress.XtraEditors.LabelControl()
			Me.pcNavMain = New DevExpress.XtraEditors.PanelControl()
			Me.SplitContainerControl1 = New DevExpress.XtraEditors.SplitContainerControl()
			Me.employeePicture = New DevExpress.XtraEditors.PictureEdit()
			Me.navMain = New DevExpress.XtraNavBar.NavBarControl()
			Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
			Me.RepositoryItemFontEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemFontEdit()
			Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
			Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
			Me.Bar1 = New DevExpress.XtraBars.Bar()
			Me.bsiLblErstellt = New DevExpress.XtraBars.BarStaticItem()
			Me.bsiCreated = New DevExpress.XtraBars.BarStaticItem()
			Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
			Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
			Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
			Me.RepositoryItemPictureEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit()
			Me.ImageCollection1 = New DevExpress.Utils.ImageCollection(Me.components)
			CType(Me.RepositoryItemComboBox2, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.RepositoryItemComboBox1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.xtabMoreinfo, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.xtabMoreinfo.SuspendLayout()
			Me.xtabApplications.SuspendLayout()
			Me.xtabWork.SuspendLayout()
			Me.xtabEducation.SuspendLayout()
			Me.xtabAddInfo.SuspendLayout()
			Me.xtabPublication.SuspendLayout()
			CType(Me.pcNavMain, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.pcNavMain.SuspendLayout()
			CType(Me.SplitContainerControl1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SplitContainerControl1.SuspendLayout()
			CType(Me.employeePicture.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.navMain, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.RepositoryItemFontEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.RepositoryItemPictureEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.ImageCollection1, System.ComponentModel.ISupportInitialize).BeginInit()
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
			'xtabMoreinfo
			'
			Me.xtabMoreinfo.Dock = System.Windows.Forms.DockStyle.Fill
			Me.xtabMoreinfo.Location = New System.Drawing.Point(201, 5)
			Me.xtabMoreinfo.Name = "xtabMoreinfo"
			Me.xtabMoreinfo.Padding = New System.Windows.Forms.Padding(20, 19, 20, 19)
			Me.xtabMoreinfo.SelectedTabPage = Me.xtabApplications
			Me.xtabMoreinfo.Size = New System.Drawing.Size(1116, 405)
			Me.xtabMoreinfo.TabIndex = 0
			Me.xtabMoreinfo.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabApplications, Me.xtabWork, Me.xtabEducation, Me.xtabAddInfo, Me.xtabPublication})
			'
			'xtabApplications
			'
			Me.xtabApplications.Controls.Add(Me.ucApplicationData)
			Me.xtabApplications.Margin = New System.Windows.Forms.Padding(2)
			Me.xtabApplications.Name = "xtabApplications"
			Me.xtabApplications.Padding = New System.Windows.Forms.Padding(2, 3, 2, 3)
			Me.xtabApplications.Size = New System.Drawing.Size(1114, 380)
			Me.xtabApplications.Text = "Bewerbungen"
			'
			'ucApplicationData
			'
			Me.ucApplicationData.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucApplicationData.IsIntialControlDataLoaded = False
			Me.ucApplicationData.Location = New System.Drawing.Point(2, 3)
			Me.ucApplicationData.Margin = New System.Windows.Forms.Padding(2)
			Me.ucApplicationData.Name = "ucApplicationData"
			Me.ucApplicationData.Size = New System.Drawing.Size(1110, 374)
			Me.ucApplicationData.TabIndex = 0
			Me.ucApplicationData.UCMediator = Nothing
			'
			'xtabWork
			'
			Me.xtabWork.Controls.Add(Me.ucCVLWorkData)
			Me.xtabWork.Name = "xtabWork"
			Me.xtabWork.Size = New System.Drawing.Size(1114, 380)
			Me.xtabWork.Text = "Berufserfahrungen"
			'
			'ucCVLWorkData
			'
			Me.ucCVLWorkData.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucCVLWorkData.IsIntialControlDataLoaded = False
			Me.ucCVLWorkData.Location = New System.Drawing.Point(0, 0)
			Me.ucCVLWorkData.Margin = New System.Windows.Forms.Padding(6)
			Me.ucCVLWorkData.Name = "ucCVLWorkData"
			Me.ucCVLWorkData.Padding = New System.Windows.Forms.Padding(20, 19, 20, 19)
			Me.ucCVLWorkData.Size = New System.Drawing.Size(1114, 380)
			Me.ucCVLWorkData.TabIndex = 0
			Me.ucCVLWorkData.UCMediator = Nothing
			'
			'xtabEducation
			'
			Me.xtabEducation.Controls.Add(Me.ucCVLEducationData)
			Me.xtabEducation.Name = "xtabEducation"
			Me.xtabEducation.Size = New System.Drawing.Size(1114, 377)
			Me.xtabEducation.Text = "Ausbildungen"
			'
			'ucCVLEducationData
			'
			Me.ucCVLEducationData.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucCVLEducationData.IsIntialControlDataLoaded = False
			Me.ucCVLEducationData.Location = New System.Drawing.Point(0, 0)
			Me.ucCVLEducationData.Margin = New System.Windows.Forms.Padding(6)
			Me.ucCVLEducationData.Name = "ucCVLEducationData"
			Me.ucCVLEducationData.Padding = New System.Windows.Forms.Padding(20, 19, 20, 19)
			Me.ucCVLEducationData.Size = New System.Drawing.Size(1114, 377)
			Me.ucCVLEducationData.TabIndex = 0
			Me.ucCVLEducationData.UCMediator = Nothing
			'
			'xtabAddInfo
			'
			Me.xtabAddInfo.Controls.Add(Me.ucCVLAdditionalData)
			Me.xtabAddInfo.Name = "xtabAddInfo"
			Me.xtabAddInfo.Padding = New System.Windows.Forms.Padding(5)
			Me.xtabAddInfo.Size = New System.Drawing.Size(1114, 377)
			Me.xtabAddInfo.Text = "Sonstige Informationen"
			'
			'ucCVLAdditionalData
			'
			Me.ucCVLAdditionalData.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucCVLAdditionalData.IsIntialControlDataLoaded = False
			Me.ucCVLAdditionalData.Location = New System.Drawing.Point(5, 5)
			Me.ucCVLAdditionalData.Margin = New System.Windows.Forms.Padding(6)
			Me.ucCVLAdditionalData.Name = "ucCVLAdditionalData"
			Me.ucCVLAdditionalData.Padding = New System.Windows.Forms.Padding(20, 19, 20, 19)
			Me.ucCVLAdditionalData.Size = New System.Drawing.Size(1104, 367)
			Me.ucCVLAdditionalData.TabIndex = 0
			Me.ucCVLAdditionalData.UCMediator = Nothing
			'
			'xtabPublication
			'
			Me.xtabPublication.Controls.Add(Me.ucCVLPublicationData)
			Me.xtabPublication.Name = "xtabPublication"
			Me.xtabPublication.Padding = New System.Windows.Forms.Padding(5)
			Me.xtabPublication.Size = New System.Drawing.Size(1114, 377)
			Me.xtabPublication.Text = "Publikationen"
			'
			'ucCVLPublicationData
			'
			Me.ucCVLPublicationData.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucCVLPublicationData.IsIntialControlDataLoaded = False
			Me.ucCVLPublicationData.Location = New System.Drawing.Point(5, 5)
			Me.ucCVLPublicationData.Margin = New System.Windows.Forms.Padding(6)
			Me.ucCVLPublicationData.Name = "ucCVLPublicationData"
			Me.ucCVLPublicationData.Padding = New System.Windows.Forms.Padding(20, 19, 20, 19)
			Me.ucCVLPublicationData.Size = New System.Drawing.Size(1104, 367)
			Me.ucCVLPublicationData.TabIndex = 0
			Me.ucCVLPublicationData.UCMediator = Nothing
			'
			'libVakAls
			'
			Me.libVakAls.Appearance.BackColor = System.Drawing.Color.Transparent
			Me.libVakAls.Appearance.Options.UseBackColor = True
			Me.libVakAls.Location = New System.Drawing.Point(423, 340)
			Me.libVakAls.Name = "libVakAls"
			Me.libVakAls.Size = New System.Drawing.Size(55, 13)
			Me.libVakAls.TabIndex = 166
			Me.libVakAls.TabStop = True
			Me.libVakAls.Text = "Kunden-Nr."
			'
			'pcNavMain
			'
			Me.pcNavMain.Controls.Add(Me.SplitContainerControl1)
			Me.pcNavMain.Dock = System.Windows.Forms.DockStyle.Left
			Me.pcNavMain.Location = New System.Drawing.Point(5, 5)
			Me.pcNavMain.Name = "pcNavMain"
			Me.pcNavMain.Padding = New System.Windows.Forms.Padding(10)
			Me.pcNavMain.Size = New System.Drawing.Size(196, 405)
			Me.pcNavMain.TabIndex = 165
			'
			'SplitContainerControl1
			'
			Me.SplitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.SplitContainerControl1.Horizontal = False
			Me.SplitContainerControl1.Location = New System.Drawing.Point(12, 12)
			Me.SplitContainerControl1.Name = "SplitContainerControl1"
			Me.SplitContainerControl1.Panel1.Controls.Add(Me.employeePicture)
			Me.SplitContainerControl1.Panel1.Padding = New System.Windows.Forms.Padding(5)
			Me.SplitContainerControl1.Panel1.Text = "Panel1"
			Me.SplitContainerControl1.Panel2.Controls.Add(Me.navMain)
			Me.SplitContainerControl1.Panel2.Padding = New System.Windows.Forms.Padding(5)
			Me.SplitContainerControl1.Panel2.Text = "Panel2"
			Me.SplitContainerControl1.Size = New System.Drawing.Size(172, 381)
			Me.SplitContainerControl1.SplitterPosition = 119
			Me.SplitContainerControl1.TabIndex = 1
			Me.SplitContainerControl1.Text = "SplitContainerControl1"
			'
			'employeePicture
			'
			Me.employeePicture.Cursor = System.Windows.Forms.Cursors.Default
			Me.employeePicture.Dock = System.Windows.Forms.DockStyle.Fill
			Me.employeePicture.Location = New System.Drawing.Point(5, 5)
			Me.employeePicture.Name = "employeePicture"
			Me.employeePicture.Properties.AllowFocused = False
			Me.employeePicture.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
			Me.employeePicture.Properties.Appearance.Options.UseBackColor = True
			Me.employeePicture.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
			Me.employeePicture.Properties.ErrorImageOptions.Image = CType(resources.GetObject("employeePicture.Properties.ErrorImageOptions.Image"), System.Drawing.Image)
			Me.employeePicture.Properties.InitialImageOptions.Image = CType(resources.GetObject("employeePicture.Properties.InitialImageOptions.Image"), System.Drawing.Image)
			Me.employeePicture.Properties.ShowMenu = False
			Me.employeePicture.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze
			Me.employeePicture.Size = New System.Drawing.Size(162, 109)
			Me.employeePicture.TabIndex = 291
			'
			'navMain
			'
			Me.navMain.Dock = System.Windows.Forms.DockStyle.Fill
			Me.navMain.Location = New System.Drawing.Point(5, 5)
			Me.navMain.Name = "navMain"
			Me.navMain.OptionsNavPane.ExpandedWidth = 162
			Me.navMain.Size = New System.Drawing.Size(162, 242)
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
			Me.barDockControlLeft.Size = New System.Drawing.Size(0, 405)
			'
			'BarManager1
			'
			Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar1})
			Me.BarManager1.DockControls.Add(Me.barDockControlTop)
			Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
			Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
			Me.BarManager1.DockControls.Add(Me.barDockControlRight)
			Me.BarManager1.Form = Me
			Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.btnShowTemplate, Me.bsiLblErstellt, Me.bsiCreated})
			Me.BarManager1.MaxItemId = 66
			Me.BarManager1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemFontEdit1, Me.RepositoryItemComboBox1, Me.RepositoryItemComboBox2, Me.RepositoryItemPictureEdit1})
			Me.BarManager1.StatusBar = Me.Bar1
			'
			'Bar1
			'
			Me.Bar1.BarName = "Benutzerdefiniert 2"
			Me.Bar1.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
			Me.Bar1.DockCol = 0
			Me.Bar1.DockRow = 0
			Me.Bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
			Me.Bar1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiLblErstellt, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bsiCreated)})
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
			Me.bsiCreated.Caption = "{0}"
			Me.bsiCreated.Id = 61
			Me.bsiCreated.Name = "bsiCreated"
			'
			'barDockControlTop
			'
			Me.barDockControlTop.CausesValidation = False
			Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
			Me.barDockControlTop.Location = New System.Drawing.Point(5, 5)
			Me.barDockControlTop.Manager = Me.BarManager1
			Me.barDockControlTop.Size = New System.Drawing.Size(1312, 0)
			'
			'barDockControlBottom
			'
			Me.barDockControlBottom.CausesValidation = False
			Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
			Me.barDockControlBottom.Location = New System.Drawing.Point(5, 410)
			Me.barDockControlBottom.Manager = Me.BarManager1
			Me.barDockControlBottom.Size = New System.Drawing.Size(1312, 22)
			'
			'barDockControlRight
			'
			Me.barDockControlRight.CausesValidation = False
			Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
			Me.barDockControlRight.Location = New System.Drawing.Point(1317, 5)
			Me.barDockControlRight.Manager = Me.BarManager1
			Me.barDockControlRight.Size = New System.Drawing.Size(0, 405)
			'
			'RepositoryItemPictureEdit1
			'
			Me.RepositoryItemPictureEdit1.Name = "RepositoryItemPictureEdit1"
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
			Me.ImageCollection1.InsertGalleryImage("barcode_16x16.png", "office2013/content/barcode_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("office2013/content/barcode_16x16.png"), 16)
			Me.ImageCollection1.Images.SetKeyName(16, "barcode_16x16.png")
			'
			'frmCVLData
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(1322, 437)
			Me.Controls.Add(Me.xtabMoreinfo)
			Me.Controls.Add(Me.libVakAls)
			Me.Controls.Add(Me.pcNavMain)
			Me.Controls.Add(Me.barDockControlLeft)
			Me.Controls.Add(Me.barDockControlRight)
			Me.Controls.Add(Me.barDockControlBottom)
			Me.Controls.Add(Me.barDockControlTop)
			Me.IconOptions.Icon = CType(resources.GetObject("frmCVLData.IconOptions.Icon"), System.Drawing.Icon)
			Me.Margin = New System.Windows.Forms.Padding(6)
			Me.Name = "frmCVLData"
			Me.Padding = New System.Windows.Forms.Padding(5)
			Me.Text = "Angaben über Bewerber"
			CType(Me.RepositoryItemComboBox2, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.RepositoryItemComboBox1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.xtabMoreinfo, System.ComponentModel.ISupportInitialize).EndInit()
			Me.xtabMoreinfo.ResumeLayout(False)
			Me.xtabApplications.ResumeLayout(False)
			Me.xtabWork.ResumeLayout(False)
			Me.xtabEducation.ResumeLayout(False)
			Me.xtabAddInfo.ResumeLayout(False)
			Me.xtabPublication.ResumeLayout(False)
			CType(Me.pcNavMain, System.ComponentModel.ISupportInitialize).EndInit()
			Me.pcNavMain.ResumeLayout(False)
			CType(Me.SplitContainerControl1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.SplitContainerControl1.ResumeLayout(False)
			CType(Me.employeePicture.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.navMain, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.RepositoryItemFontEdit1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.RepositoryItemPictureEdit1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.ImageCollection1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)
			Me.PerformLayout()

		End Sub
		Friend WithEvents RepositoryItemComboBox2 As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
		Friend WithEvents RepositoryItemComboBox1 As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
		Friend WithEvents btnShowTemplate As DevExpress.XtraBars.BarSubItem
		Friend WithEvents libVakAls As DevExpress.XtraEditors.LabelControl
		Friend WithEvents pcNavMain As DevExpress.XtraEditors.PanelControl
		Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
		Friend WithEvents RepositoryItemFontEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemFontEdit
		Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
		Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
		Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
		Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
		Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
		Friend WithEvents xtabMoreinfo As DevExpress.XtraTab.XtraTabControl
		Friend WithEvents xtabApplications As DevExpress.XtraTab.XtraTabPage
		Friend WithEvents xtabEducation As DevExpress.XtraTab.XtraTabPage
		Friend WithEvents xtabWork As DevExpress.XtraTab.XtraTabPage
		Friend WithEvents navMain As DevExpress.XtraNavBar.NavBarControl
		Friend WithEvents ImageCollection1 As DevExpress.Utils.ImageCollection
		Friend WithEvents xtabAddInfo As DevExpress.XtraTab.XtraTabPage
		Friend WithEvents Bar1 As DevExpress.XtraBars.Bar
		'Friend WithEvents bsiCreated As DevExpress.XtraBars.BarStaticItem
		Friend WithEvents bsiLblErstellt As DevExpress.XtraBars.BarStaticItem
		Friend WithEvents bsiCreated As DevExpress.XtraBars.BarStaticItem
		Friend WithEvents SplitContainerControl1 As DevExpress.XtraEditors.SplitContainerControl
		Friend WithEvents employeePicture As DevExpress.XtraEditors.PictureEdit
		Friend WithEvents RepositoryItemPictureEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit
		Friend WithEvents ucCVLWorkData As SP.MA.EmployeeMng.UI.ucCVLWork
		Friend WithEvents ucCVLEducationData As SP.MA.EmployeeMng.UI.ucCVLEducation
		Friend WithEvents ucCVLAdditionalData As SP.MA.EmployeeMng.UI.ucCVLAdditioinalInformation
		Friend WithEvents xtabPublication As DevExpress.XtraTab.XtraTabPage
		Friend WithEvents ucCVLPublicationData As SP.MA.EmployeeMng.UI.ucCVLPublication
		Friend WithEvents ucApplicationData As SP.MA.EmployeeMng.UI.ucApplicationData

	End Class
End Namespace
