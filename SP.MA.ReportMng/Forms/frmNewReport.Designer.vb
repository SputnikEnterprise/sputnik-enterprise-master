Namespace UI


	<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
	Partial Class frmNewReport
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
			Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmNewReport))
			Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
			Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
			Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
			Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
			Me.PictureEdit1 = New DevExpress.XtraEditors.PictureEdit()
			Me.lblHeaderNormal = New DevExpress.XtraEditors.LabelControl()
			Me.btnClose = New DevExpress.XtraEditors.SimpleButton()
			Me.lblHeaderFett = New System.Windows.Forms.Label()
			Me.Bar3 = New DevExpress.XtraBars.Bar()
			Me.Bar1 = New DevExpress.XtraBars.Bar()
			Me.bsiLblRPCount = New DevExpress.XtraBars.BarStaticItem()
			Me.bsiChanged = New DevExpress.XtraBars.BarStaticItem()
			Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
			Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
			Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
			Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
			Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
			Me.btnShowTemplate = New DevExpress.XtraBars.BarSubItem()
			Me.RepositoryItemFontEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemFontEdit()
			Me.RepositoryItemComboBox1 = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
			Me.RepositoryItemComboBox2 = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
			Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
			Me.ImageCollection1 = New DevExpress.Utils.ImageCollection(Me.components)
			Me.lueMandant = New DevExpress.XtraEditors.LookUpEdit()
			Me.btnSearch = New DevExpress.XtraEditors.SimpleButton()
			Me.lblMonat = New DevExpress.XtraEditors.LabelControl()
			Me.lblMandant = New DevExpress.XtraEditors.LabelControl()
			Me.lueMonth = New DevExpress.XtraEditors.LookUpEdit()
			Me.lueYear = New DevExpress.XtraEditors.LookUpEdit()
			Me.lblJahr = New DevExpress.XtraEditors.LabelControl()
			Me.lblDetails = New DevExpress.XtraEditors.LabelControl()
			Me.grdReportOverview = New DevExpress.XtraGrid.GridControl()
			Me.gvReportOverview = New DevExpress.XtraGrid.Views.Grid.GridView()
			CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.PanelControl1.SuspendLayout()
			CType(Me.PictureEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.RepositoryItemFontEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.RepositoryItemComboBox1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.RepositoryItemComboBox2, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.ImageCollection1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.lueMonth.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.lueYear.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.grdReportOverview, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvReportOverview, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			'
			'PanelControl1
			'
			Me.PanelControl1.Controls.Add(Me.PictureEdit1)
			Me.PanelControl1.Controls.Add(Me.lblHeaderNormal)
			Me.PanelControl1.Controls.Add(Me.btnClose)
			Me.PanelControl1.Controls.Add(Me.lblHeaderFett)
			Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Top
			Me.PanelControl1.Location = New System.Drawing.Point(0, 0)
			Me.PanelControl1.Name = "PanelControl1"
			Me.PanelControl1.Size = New System.Drawing.Size(814, 79)
			Me.PanelControl1.TabIndex = 234
			'
			'PictureEdit1
			'
			Me.PictureEdit1.EditValue = CType(resources.GetObject("PictureEdit1.EditValue"), Object)
			Me.PictureEdit1.Location = New System.Drawing.Point(12, 12)
			Me.PictureEdit1.Name = "PictureEdit1"
			Me.PictureEdit1.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
			Me.PictureEdit1.Properties.Appearance.Options.UseBackColor = True
			Me.PictureEdit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
			Me.PictureEdit1.Properties.PictureAlignment = System.Drawing.ContentAlignment.TopLeft
			Me.PictureEdit1.Size = New System.Drawing.Size(66, 59)
			Me.PictureEdit1.TabIndex = 233
			'
			'lblHeaderNormal
			'
			Me.lblHeaderNormal.Location = New System.Drawing.Point(110, 44)
			Me.lblHeaderNormal.Name = "lblHeaderNormal"
			Me.lblHeaderNormal.Size = New System.Drawing.Size(218, 13)
			Me.lblHeaderNormal.TabIndex = 1
			Me.lblHeaderNormal.Text = "Wählen Sie Ihre gewünschten Kriterien aus..."
			'
			'btnClose
			'
			Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnClose.Location = New System.Drawing.Point(698, 22)
			Me.btnClose.Name = "btnClose"
			Me.btnClose.Size = New System.Drawing.Size(86, 24)
			Me.btnClose.TabIndex = 999
			Me.btnClose.Text = "Schliessen"
			'
			'lblHeaderFett
			'
			Me.lblHeaderFett.AutoSize = True
			Me.lblHeaderFett.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
			Me.lblHeaderFett.Location = New System.Drawing.Point(84, 22)
			Me.lblHeaderFett.Name = "lblHeaderFett"
			Me.lblHeaderFett.Size = New System.Drawing.Size(111, 13)
			Me.lblHeaderFett.TabIndex = 0
			Me.lblHeaderFett.Text = "Rapporte erstellen"
			'
			'Bar3
			'
			Me.Bar3.BarName = "Statusleiste"
			Me.Bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
			Me.Bar3.DockCol = 0
			Me.Bar3.DockRow = 0
			Me.Bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
			Me.Bar3.OptionsBar.AllowQuickCustomization = False
			Me.Bar3.OptionsBar.DrawDragBorder = False
			Me.Bar3.OptionsBar.UseWholeRow = True
			Me.Bar3.Text = "Statusleiste"
			'
			'Bar1
			'
			Me.Bar1.BarName = "Benutzerdefiniert 2"
			Me.Bar1.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
			Me.Bar1.DockCol = 0
			Me.Bar1.DockRow = 0
			Me.Bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
			Me.Bar1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiLblRPCount, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bsiChanged)})
			Me.Bar1.OptionsBar.AllowQuickCustomization = False
			Me.Bar1.OptionsBar.DrawDragBorder = False
			Me.Bar1.OptionsBar.UseWholeRow = True
			Me.Bar1.Text = "Benutzerdefiniert 2"
			'
			'bsiLblRPCount
			'
			Me.bsiLblRPCount.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
			Me.bsiLblRPCount.Caption = "Anzahl Datensätze:"
			Me.bsiLblRPCount.Id = 60
			Me.bsiLblRPCount.ItemAppearance.Normal.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.bsiLblRPCount.ItemAppearance.Normal.Options.UseFont = True
			Me.bsiLblRPCount.Name = "bsiLblRPCount"
			Me.bsiLblRPCount.TextAlignment = System.Drawing.StringAlignment.Near
			'
			'bsiChanged
			'
			Me.bsiChanged.Caption = "{0}"
			Me.bsiChanged.Id = 61
			Me.bsiChanged.Name = "bsiChanged"
			Me.bsiChanged.TextAlignment = System.Drawing.StringAlignment.Near
			'
			'BarManager1
			'
			Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar1})
			Me.BarManager1.DockControls.Add(Me.barDockControlTop)
			Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
			Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
			Me.BarManager1.DockControls.Add(Me.barDockControlRight)
			Me.BarManager1.Form = Me
			Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.btnShowTemplate, Me.bsiLblRPCount, Me.bsiChanged})
			Me.BarManager1.MaxItemId = 65
			Me.BarManager1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemFontEdit1, Me.RepositoryItemComboBox1, Me.RepositoryItemComboBox2})
			Me.BarManager1.StatusBar = Me.Bar1
			'
			'barDockControlTop
			'
			Me.barDockControlTop.CausesValidation = False
			Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
			Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
			Me.barDockControlTop.Size = New System.Drawing.Size(814, 0)
			'
			'barDockControlBottom
			'
			Me.barDockControlBottom.CausesValidation = False
			Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
			Me.barDockControlBottom.Location = New System.Drawing.Point(0, 656)
			Me.barDockControlBottom.Size = New System.Drawing.Size(814, 25)
			'
			'barDockControlLeft
			'
			Me.barDockControlLeft.CausesValidation = False
			Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
			Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
			Me.barDockControlLeft.Size = New System.Drawing.Size(0, 656)
			'
			'barDockControlRight
			'
			Me.barDockControlRight.CausesValidation = False
			Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
			Me.barDockControlRight.Location = New System.Drawing.Point(814, 0)
			Me.barDockControlRight.Size = New System.Drawing.Size(0, 656)
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
			'lueMandant
			'
			Me.lueMandant.Location = New System.Drawing.Point(118, 118)
			Me.lueMandant.Name = "lueMandant"
			Me.lueMandant.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup
			SerializableAppearanceObject1.ForeColor = System.Drawing.Color.Red
			SerializableAppearanceObject1.Options.UseForeColor = True
			Me.lueMandant.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, "", Nothing, Nothing, True)})
			Me.lueMandant.Properties.DropDownRows = 10
			Me.lueMandant.Properties.NullText = ""
			Me.lueMandant.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete
			Me.lueMandant.Properties.ShowFooter = False
			Me.lueMandant.Properties.ShowHeader = False
			Me.lueMandant.Size = New System.Drawing.Size(290, 20)
			Me.lueMandant.TabIndex = 299
			'
			'btnSearch
			'
			Me.btnSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnSearch.Location = New System.Drawing.Point(699, 117)
			Me.btnSearch.Name = "btnSearch"
			Me.btnSearch.Size = New System.Drawing.Size(85, 23)
			Me.btnSearch.TabIndex = 305
			Me.btnSearch.Text = "Erstellen"
			'
			'lblMonat
			'
			Me.lblMonat.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblMonat.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblMonat.Location = New System.Drawing.Point(228, 148)
			Me.lblMonat.Name = "lblMonat"
			Me.lblMonat.Size = New System.Drawing.Size(70, 13)
			Me.lblMonat.TabIndex = 304
			Me.lblMonat.Text = "Monat"
			'
			'lblMandant
			'
			Me.lblMandant.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblMandant.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblMandant.Location = New System.Drawing.Point(12, 122)
			Me.lblMandant.Name = "lblMandant"
			Me.lblMandant.Size = New System.Drawing.Size(100, 13)
			Me.lblMandant.TabIndex = 300
			Me.lblMandant.Text = "Mandant"
			'
			'lueMonth
			'
			Me.lueMonth.Location = New System.Drawing.Point(304, 144)
			Me.lueMonth.Name = "lueMonth"
			SerializableAppearanceObject2.ForeColor = System.Drawing.Color.Red
			SerializableAppearanceObject2.Options.UseForeColor = True
			Me.lueMonth.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject2, "", Nothing, Nothing, True)})
			Me.lueMonth.Properties.ShowFooter = False
			Me.lueMonth.Size = New System.Drawing.Size(104, 20)
			Me.lueMonth.TabIndex = 302
			'
			'lueYear
			'
			Me.lueYear.Location = New System.Drawing.Point(118, 144)
			Me.lueYear.Name = "lueYear"
			SerializableAppearanceObject3.ForeColor = System.Drawing.Color.Red
			SerializableAppearanceObject3.Options.UseForeColor = True
			Me.lueYear.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject3, "", Nothing, Nothing, True)})
			Me.lueYear.Properties.ShowFooter = False
			Me.lueYear.Size = New System.Drawing.Size(104, 20)
			Me.lueYear.TabIndex = 301
			'
			'lblJahr
			'
			Me.lblJahr.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblJahr.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblJahr.Location = New System.Drawing.Point(12, 148)
			Me.lblJahr.Name = "lblJahr"
			Me.lblJahr.Size = New System.Drawing.Size(100, 13)
			Me.lblJahr.TabIndex = 303
			Me.lblJahr.Text = "Jahr"
			'
			'lblDetails
			'
			Me.lblDetails.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblDetails.Location = New System.Drawing.Point(12, 184)
			Me.lblDetails.Name = "lblDetails"
			Me.lblDetails.Size = New System.Drawing.Size(100, 13)
			Me.lblDetails.TabIndex = 310
			Me.lblDetails.Text = "Details"
			'
			'grdReportOverview
			'
			Me.grdReportOverview.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
							Or System.Windows.Forms.AnchorStyles.Left) _
							Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.grdReportOverview.Location = New System.Drawing.Point(12, 203)
			Me.grdReportOverview.MainView = Me.gvReportOverview
			Me.grdReportOverview.Name = "grdReportOverview"
			Me.grdReportOverview.Size = New System.Drawing.Size(787, 436)
			Me.grdReportOverview.TabIndex = 315
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
			'frmNewReport
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(814, 681)
			Me.Controls.Add(Me.grdReportOverview)
			Me.Controls.Add(Me.lblDetails)
			Me.Controls.Add(Me.lueMandant)
			Me.Controls.Add(Me.btnSearch)
			Me.Controls.Add(Me.lblMonat)
			Me.Controls.Add(Me.lblMandant)
			Me.Controls.Add(Me.lueMonth)
			Me.Controls.Add(Me.lueYear)
			Me.Controls.Add(Me.lblJahr)
			Me.Controls.Add(Me.PanelControl1)
			Me.Controls.Add(Me.barDockControlLeft)
			Me.Controls.Add(Me.barDockControlRight)
			Me.Controls.Add(Me.barDockControlBottom)
			Me.Controls.Add(Me.barDockControlTop)
			Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
			Me.MinimumSize = New System.Drawing.Size(830, 719)
			Me.Name = "frmNewReport"
			Me.Text = "Neue Rapporte erstellen"
			CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.PanelControl1.ResumeLayout(False)
			Me.PanelControl1.PerformLayout()
			CType(Me.PictureEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.RepositoryItemFontEdit1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.RepositoryItemComboBox1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.RepositoryItemComboBox2, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.ImageCollection1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.lueMonth.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.lueYear.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.grdReportOverview, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvReportOverview, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub
		Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
		Friend WithEvents PictureEdit1 As DevExpress.XtraEditors.PictureEdit
		Friend WithEvents lblHeaderNormal As DevExpress.XtraEditors.LabelControl
		Friend WithEvents btnClose As DevExpress.XtraEditors.SimpleButton
		Friend WithEvents lblHeaderFett As System.Windows.Forms.Label
		Friend WithEvents Bar3 As DevExpress.XtraBars.Bar
		Friend WithEvents Bar1 As DevExpress.XtraBars.Bar
		Friend WithEvents bsiLblRPCount As DevExpress.XtraBars.BarStaticItem
		Friend WithEvents bsiChanged As DevExpress.XtraBars.BarStaticItem
		Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
		Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
		Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
		Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
		Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
		Friend WithEvents btnShowTemplate As DevExpress.XtraBars.BarSubItem
		Friend WithEvents RepositoryItemFontEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemFontEdit
		Friend WithEvents RepositoryItemComboBox1 As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
		Friend WithEvents RepositoryItemComboBox2 As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
		Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
		Friend WithEvents ImageCollection1 As DevExpress.Utils.ImageCollection
		Friend WithEvents lueMandant As DevExpress.XtraEditors.LookUpEdit
		Friend WithEvents btnSearch As DevExpress.XtraEditors.SimpleButton
		Friend WithEvents lblMonat As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblMandant As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lueMonth As DevExpress.XtraEditors.LookUpEdit
		Friend WithEvents lueYear As DevExpress.XtraEditors.LookUpEdit
		Friend WithEvents lblJahr As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblDetails As DevExpress.XtraEditors.LabelControl
		Friend WithEvents grdReportOverview As DevExpress.XtraGrid.GridControl
		Friend WithEvents gvReportOverview As DevExpress.XtraGrid.Views.Grid.GridView
	End Class

End Namespace
