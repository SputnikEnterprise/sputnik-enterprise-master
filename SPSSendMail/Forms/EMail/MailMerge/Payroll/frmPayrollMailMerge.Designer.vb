
Namespace RichEditSendMail

	<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
	Partial Class frmPayrollEMailMerge

		Inherits DevExpress.XtraEditors.XtraForm

		'Form overrides dispose to clean up the component list.
		<System.Diagnostics.DebuggerNonUserCode()>
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
		<System.Diagnostics.DebuggerStepThrough()>
		Private Sub InitializeComponent()
			Me.components = New System.ComponentModel.Container()
			Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPayrollEMailMerge))
			Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
			Me.Bar4 = New DevExpress.XtraBars.Bar()
			Me.bsiInfo = New DevExpress.XtraBars.BarStaticItem()
			Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
			Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
			Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
			Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
			Me.BarStaticItem1 = New DevExpress.XtraBars.BarStaticItem()
			Me.StyleController1 = New DevExpress.XtraEditors.StyleController(Me.components)
			Me.pnlMain = New DevExpress.XtraEditors.PanelControl()
			Me.accordionControl2 = New DevExpress.XtraBars.Navigation.AccordionControl()
			Me.accordionControlElement23 = New DevExpress.XtraBars.Navigation.AccordionControlElement()
			Me.aceSender = New DevExpress.XtraBars.Navigation.AccordionControlElement()
			Me.aceStaging = New DevExpress.XtraBars.Navigation.AccordionControlElement()
			Me.aceSmtpServer = New DevExpress.XtraBars.Navigation.AccordionControlElement()
			Me.aceSep1 = New DevExpress.XtraBars.Navigation.AccordionControlSeparator()
			Me.aceSendResult = New DevExpress.XtraBars.Navigation.AccordionControlElement()
			Me.btnMergeSend = New DevExpress.XtraEditors.SimpleButton()
			Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
			Me.grpData = New DevExpress.XtraEditors.GroupControl()
			Me.grdMergeData = New DevExpress.XtraGrid.GridControl()
			Me.gvMergeData = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.btnSend = New DevExpress.XtraEditors.SimpleButton()
			Me.xtabHtml = New DevExpress.XtraTab.XtraTabPage()
			Me.WebBrowser1 = New System.Windows.Forms.WebBrowser()
			Me.ImageCollection1 = New DevExpress.Utils.ImageCollection(Me.components)
			Me.GroupBox1 = New DevExpress.XtraEditors.PanelControl()
			Me.lblFormImage = New DevExpress.XtraEditors.LabelControl()
			Me.btnClose = New DevExpress.XtraEditors.SimpleButton()
			CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.StyleController1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.pnlMain, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.pnlMain.SuspendLayout()
			CType(Me.accordionControl2, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.grpData, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.grpData.SuspendLayout()
			CType(Me.grdMergeData, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvMergeData, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.xtabHtml.SuspendLayout()
			CType(Me.ImageCollection1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.GroupBox1.SuspendLayout()
			Me.SuspendLayout()
			'
			'BarManager1
			'
			Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar4})
			Me.BarManager1.DockControls.Add(Me.barDockControlTop)
			Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
			Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
			Me.BarManager1.DockControls.Add(Me.barDockControlRight)
			Me.BarManager1.Form = Me
			Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiInfo, Me.BarStaticItem1})
			Me.BarManager1.MaxItemId = 14
			Me.BarManager1.StatusBar = Me.Bar4
			'
			'Bar4
			'
			Me.Bar4.BarName = "Statusleiste"
			Me.Bar4.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
			Me.Bar4.DockCol = 0
			Me.Bar4.DockRow = 0
			Me.Bar4.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
			Me.Bar4.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiInfo)})
			Me.Bar4.OptionsBar.AllowQuickCustomization = False
			Me.Bar4.OptionsBar.DrawDragBorder = False
			Me.Bar4.OptionsBar.UseWholeRow = True
			Me.Bar4.Text = "Statusleiste"
			'
			'bsiInfo
			'
			Me.bsiInfo.AutoSize = DevExpress.XtraBars.BarStaticItemSize.Spring
			Me.bsiInfo.Caption = "Bereit"
			Me.bsiInfo.Id = 0
			Me.bsiInfo.Name = "bsiInfo"
			Me.bsiInfo.Size = New System.Drawing.Size(32, 0)
			Me.bsiInfo.Width = 32
			'
			'barDockControlTop
			'
			Me.barDockControlTop.CausesValidation = False
			Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
			Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
			Me.barDockControlTop.Manager = Me.BarManager1
			Me.barDockControlTop.Size = New System.Drawing.Size(981, 0)
			'
			'barDockControlBottom
			'
			Me.barDockControlBottom.CausesValidation = False
			Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
			Me.barDockControlBottom.Location = New System.Drawing.Point(0, 679)
			Me.barDockControlBottom.Manager = Me.BarManager1
			Me.barDockControlBottom.Size = New System.Drawing.Size(981, 22)
			'
			'barDockControlLeft
			'
			Me.barDockControlLeft.CausesValidation = False
			Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
			Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
			Me.barDockControlLeft.Manager = Me.BarManager1
			Me.barDockControlLeft.Size = New System.Drawing.Size(0, 679)
			'
			'barDockControlRight
			'
			Me.barDockControlRight.CausesValidation = False
			Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
			Me.barDockControlRight.Location = New System.Drawing.Point(981, 0)
			Me.barDockControlRight.Manager = Me.BarManager1
			Me.barDockControlRight.Size = New System.Drawing.Size(0, 679)
			'
			'BarStaticItem1
			'
			Me.BarStaticItem1.Caption = " "
			Me.BarStaticItem1.Id = 5
			Me.BarStaticItem1.Name = "BarStaticItem1"
			'
			'pnlMain
			'
			Me.pnlMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.pnlMain.Controls.Add(Me.accordionControl2)
			Me.pnlMain.Controls.Add(Me.btnMergeSend)
			Me.pnlMain.Controls.Add(Me.grpData)
			Me.pnlMain.Controls.Add(Me.btnSend)
			Me.pnlMain.Location = New System.Drawing.Point(0, 83)
			Me.pnlMain.Name = "pnlMain"
			Me.pnlMain.Padding = New System.Windows.Forms.Padding(5)
			Me.pnlMain.Size = New System.Drawing.Size(975, 584)
			Me.pnlMain.TabIndex = 2
			'
			'accordionControl2
			'
			Me.accordionControl2.Dock = System.Windows.Forms.DockStyle.Right
			Me.accordionControl2.Elements.AddRange(New DevExpress.XtraBars.Navigation.AccordionControlElement() {Me.accordionControlElement23, Me.aceSendResult})
			Me.accordionControl2.ExpandGroupOnHeaderClick = False
			Me.accordionControl2.ExpandItemOnHeaderClick = False
			Me.accordionControl2.Location = New System.Drawing.Point(710, 7)
			Me.accordionControl2.Name = "accordionControl2"
			Me.accordionControl2.ShowGroupExpandButtons = False
			Me.accordionControl2.ShowItemExpandButtons = False
			Me.accordionControl2.Size = New System.Drawing.Size(258, 570)
			Me.accordionControl2.TabIndex = 12
			Me.accordionControl2.Text = "accordionControl2"
			'
			'accordionControlElement23
			'
			Me.accordionControlElement23.Elements.AddRange(New DevExpress.XtraBars.Navigation.AccordionControlElement() {Me.aceSender, Me.aceStaging, Me.aceSmtpServer, Me.aceSep1})
			Me.accordionControlElement23.Expanded = True
			Me.accordionControlElement23.Name = "accordionControlElement23"
			Me.accordionControlElement23.Text = "Info"
			'
			'aceSender
			'
			Me.aceSender.ImageOptions.Image = Global.SPSSendMail.My.Resources.Resources.user_16x16
			Me.aceSender.Name = "aceSender"
			Me.aceSender.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item
			Me.aceSender.Text = "Sender"
			'
			'aceStaging
			'
			Me.aceStaging.Name = "aceStaging"
			Me.aceStaging.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item
			Me.aceStaging.Text = "Testnachricht an"
			'
			'aceSmtpServer
			'
			Me.aceSmtpServer.Name = "aceSmtpServer"
			Me.aceSmtpServer.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item
			Me.aceSmtpServer.Text = "SMTPServer"
			'
			'aceSep1
			'
			Me.aceSep1.Name = "aceSep1"
			'
			'aceSendResult
			'
			Me.aceSendResult.Expanded = True
			Me.aceSendResult.Name = "aceSendResult"
			Me.aceSendResult.Text = "Versand-Result"
			'
			'btnMergeSend
			'
			Me.btnMergeSend.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.[True]
			Me.btnMergeSend.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnMergeSend.ImageOptions.ImageList = Me.ImageList1
			Me.btnMergeSend.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.TopCenter
			Me.btnMergeSend.ImageOptions.SvgImage = Global.SPSSendMail.My.Resources.Resources.actions_send
			Me.btnMergeSend.Location = New System.Drawing.Point(529, 90)
			Me.btnMergeSend.Name = "btnMergeSend"
			Me.btnMergeSend.Size = New System.Drawing.Size(106, 76)
			Me.btnMergeSend.TabIndex = 11
			Me.btnMergeSend.Text = "Versand starten"
			'
			'ImageList1
			'
			Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
			Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
			Me.ImageList1.Images.SetKeyName(0, "Disponent.ico")
			Me.ImageList1.Images.SetKeyName(1, "Mandant.png")
			Me.ImageList1.Images.SetKeyName(2, "Mail.ICO")
			Me.ImageList1.Images.SetKeyName(3, "Kunde.ico")
			Me.ImageList1.Images.SetKeyName(4, "ZHD.png")
			Me.ImageList1.Images.SetKeyName(5, "OPEN.GIF")
			Me.ImageList1.Images.SetKeyName(6, "Delete K.ico")
			'
			'grpData
			'
			Me.grpData.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.grpData.AppearanceCaption.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grpData.AppearanceCaption.Options.UseFont = True
			Me.grpData.CaptionImageOptions.SvgImage = Global.SPSSendMail.My.Resources.Resources.reviewers
			Me.grpData.CaptionLocation = DevExpress.Utils.Locations.Top
			Me.grpData.Controls.Add(Me.grdMergeData)
			Me.grpData.GroupStyle = DevExpress.Utils.GroupStyle.Card
			Me.grpData.Location = New System.Drawing.Point(7, 7)
			Me.grpData.Name = "grpData"
			Me.grpData.Padding = New System.Windows.Forms.Padding(5)
			Me.grpData.Size = New System.Drawing.Size(509, 570)
			Me.grpData.TabIndex = 10
			Me.grpData.Text = "Adress-Daten"
			'
			'grdMergeData
			'
			Me.grdMergeData.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.grdMergeData.Location = New System.Drawing.Point(3, 32)
			Me.grdMergeData.MainView = Me.gvMergeData
			Me.grdMergeData.Name = "grdMergeData"
			Me.grdMergeData.Size = New System.Drawing.Size(496, 528)
			Me.grdMergeData.TabIndex = 2
			Me.grdMergeData.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvMergeData})
			'
			'gvMergeData
			'
			Me.gvMergeData.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			Me.gvMergeData.GridControl = Me.grdMergeData
			Me.gvMergeData.Name = "gvMergeData"
			Me.gvMergeData.OptionsBehavior.Editable = False
			Me.gvMergeData.OptionsSelection.EnableAppearanceFocusedCell = False
			Me.gvMergeData.OptionsView.ShowGroupPanel = False
			'
			'btnSend
			'
			Me.btnSend.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.[True]
			Me.btnSend.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnSend.ImageOptions.Image = Global.SPSSendMail.My.Resources.Resources.user_32x32
			Me.btnSend.ImageOptions.ImageList = Me.ImageList1
			Me.btnSend.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.TopCenter
			Me.btnSend.Location = New System.Drawing.Point(529, 6)
			Me.btnSend.Name = "btnSend"
			Me.btnSend.Size = New System.Drawing.Size(106, 76)
			Me.btnSend.TabIndex = 1
			Me.btnSend.Text = "Test-Nachricht"
			'
			'xtabHtml
			'
			Me.xtabHtml.Controls.Add(Me.WebBrowser1)
			Me.xtabHtml.Name = "xtabHtml"
			Me.xtabHtml.Size = New System.Drawing.Size(1225, 330)
			'
			'WebBrowser1
			'
			Me.WebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.WebBrowser1.Location = New System.Drawing.Point(0, 0)
			Me.WebBrowser1.MinimumSize = New System.Drawing.Size(20, 20)
			Me.WebBrowser1.Name = "WebBrowser1"
			Me.WebBrowser1.Size = New System.Drawing.Size(1225, 330)
			Me.WebBrowser1.TabIndex = 0
			'
			'ImageCollection1
			'
			Me.ImageCollection1.ImageStream = CType(resources.GetObject("ImageCollection1.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
			Me.ImageCollection1.InsertGalleryImage("employee_16x16.png", "images/people/employee_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/people/employee_16x16.png"), 0)
			Me.ImageCollection1.Images.SetKeyName(0, "employee_16x16.png")
			Me.ImageCollection1.InsertGalleryImage("contact_16x16.png", "images/mail/contact_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/mail/contact_16x16.png"), 1)
			Me.ImageCollection1.Images.SetKeyName(1, "contact_16x16.png")
			Me.ImageCollection1.InsertGalleryImage("customer_16x16.png", "images/people/publicfix_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/people/publicfix_16x16.png"), 2)
			Me.ImageCollection1.Images.SetKeyName(2, "customer_16x16.png")
			Me.ImageCollection1.Images.SetKeyName(3, "UserBlue.png")
			Me.ImageCollection1.InsertGalleryImage("home_16x16.png", "images/navigation/home_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/navigation/home_16x16.png"), 4)
			Me.ImageCollection1.Images.SetKeyName(4, "home_16x16.png")
			Me.ImageCollection1.InsertImage(Global.SPSSendMail.My.Resources.Resources.open_16x16, "open_16x16", GetType(Global.SPSSendMail.My.Resources.Resources), 5)
			Me.ImageCollection1.Images.SetKeyName(5, "open_16x16")
			Me.ImageCollection1.InsertImage(Global.SPSSendMail.My.Resources.Resources.delete_16x16, "delete_16x16", GetType(Global.SPSSendMail.My.Resources.Resources), 6)
			Me.ImageCollection1.Images.SetKeyName(6, "delete_16x16")
			Me.ImageCollection1.Images.SetKeyName(7, "File-PDF-icon.png")
			Me.ImageCollection1.Images.SetKeyName(8, "compress-icon.png")
			'
			'GroupBox1
			'
			Me.GroupBox1.Controls.Add(Me.lblFormImage)
			Me.GroupBox1.Controls.Add(Me.btnClose)
			Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
			Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
			Me.GroupBox1.Name = "GroupBox1"
			Me.GroupBox1.Size = New System.Drawing.Size(981, 77)
			Me.GroupBox1.TabIndex = 208
			'
			'lblFormImage
			'
			Me.lblFormImage.AllowHtmlString = True
			Me.lblFormImage.Appearance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
			Me.lblFormImage.Appearance.Options.UseFont = True
			Me.lblFormImage.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblFormImage.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter
			Me.lblFormImage.ImageOptions.Alignment = System.Drawing.ContentAlignment.MiddleLeft
			Me.lblFormImage.ImageOptions.SvgImage = Global.SPSSendMail.My.Resources.Resources.mailmerge2
			Me.lblFormImage.Location = New System.Drawing.Point(12, 7)
			Me.lblFormImage.Name = "lblFormImage"
			Me.lblFormImage.Size = New System.Drawing.Size(341, 53)
			Me.lblFormImage.TabIndex = 1001
			Me.lblFormImage.Text = "  EMail-Versand von Lohnabrechnungen"
			'
			'btnClose
			'
			Me.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Right
			Me.btnClose.Location = New System.Drawing.Point(853, 21)
			Me.btnClose.Name = "btnClose"
			Me.btnClose.Size = New System.Drawing.Size(100, 25)
			Me.btnClose.TabIndex = 204
			Me.btnClose.Text = "Schliessen"
			'
			'frmPayrollEMailMerge
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(981, 701)
			Me.Controls.Add(Me.GroupBox1)
			Me.Controls.Add(Me.pnlMain)
			Me.Controls.Add(Me.barDockControlLeft)
			Me.Controls.Add(Me.barDockControlRight)
			Me.Controls.Add(Me.barDockControlBottom)
			Me.Controls.Add(Me.barDockControlTop)
			Me.IconOptions.Icon = CType(resources.GetObject("frmPayrollEMailMerge.IconOptions.Icon"), System.Drawing.Icon)
			Me.IconOptions.SvgImage = Global.SPSSendMail.My.Resources.Resources.mailmerge3
			Me.MinimumSize = New System.Drawing.Size(983, 733)
			Me.Name = "frmPayrollEMailMerge"
			Me.Text = "Versand von Nachrichten"
			CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.StyleController1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.pnlMain, System.ComponentModel.ISupportInitialize).EndInit()
			Me.pnlMain.ResumeLayout(False)
			CType(Me.accordionControl2, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.grpData, System.ComponentModel.ISupportInitialize).EndInit()
			Me.grpData.ResumeLayout(False)
			CType(Me.grdMergeData, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvMergeData, System.ComponentModel.ISupportInitialize).EndInit()
			Me.xtabHtml.ResumeLayout(False)
			CType(Me.ImageCollection1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.GroupBox1.ResumeLayout(False)
			Me.ResumeLayout(False)
			Me.PerformLayout()

		End Sub
		Friend WithEvents pnlMain As DevExpress.XtraEditors.PanelControl
		Friend WithEvents btnSend As DevExpress.XtraEditors.SimpleButton
		Friend WithEvents grpData As DevExpress.XtraEditors.GroupControl
		Friend WithEvents StyleController1 As DevExpress.XtraEditors.StyleController
		Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
		Friend WithEvents xtabHtml As DevExpress.XtraTab.XtraTabPage
		Friend WithEvents WebBrowser1 As System.Windows.Forms.WebBrowser
		Friend WithEvents ImageCollection1 As DevExpress.Utils.ImageCollection
		Friend WithEvents btnMergeSend As DevExpress.XtraEditors.SimpleButton
		Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
		Friend WithEvents Bar4 As DevExpress.XtraBars.Bar
		Friend WithEvents bsiInfo As DevExpress.XtraBars.BarStaticItem
		Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
		Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
		Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
		Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
		Friend WithEvents BarStaticItem1 As DevExpress.XtraBars.BarStaticItem
		Private WithEvents accordionControl2 As DevExpress.XtraBars.Navigation.AccordionControl
		Private WithEvents accordionControlElement23 As DevExpress.XtraBars.Navigation.AccordionControlElement
		Private WithEvents aceSmtpServer As DevExpress.XtraBars.Navigation.AccordionControlElement
		Private WithEvents aceSender As DevExpress.XtraBars.Navigation.AccordionControlElement
		Friend WithEvents GroupBox1 As DevExpress.XtraEditors.PanelControl
		Friend WithEvents btnClose As DevExpress.XtraEditors.SimpleButton
		Friend WithEvents lblFormImage As DevExpress.XtraEditors.LabelControl
		Friend WithEvents aceStaging As DevExpress.XtraBars.Navigation.AccordionControlElement
		Friend WithEvents grdMergeData As DevExpress.XtraGrid.GridControl
		Friend WithEvents gvMergeData As DevExpress.XtraGrid.Views.Grid.GridView
		Friend WithEvents aceSendResult As DevExpress.XtraBars.Navigation.AccordionControlElement
		Private WithEvents aceSep1 As DevExpress.XtraBars.Navigation.AccordionControlSeparator
	End Class

End Namespace
