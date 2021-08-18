Partial Public Class frmInvoicePrint
	''' <summary>
	''' Required designer variable.
	''' </summary>
	Private components As System.ComponentModel.IContainer = Nothing

	''' <summary>
	''' Clean up any resources being used.
	''' </summary>
	''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	Protected Overrides Sub Dispose(ByVal disposing As Boolean)
		If disposing AndAlso (components IsNot Nothing) Then
			components.Dispose()
		End If
		MyBase.Dispose(disposing)
	End Sub

#Region "Windows Form Designer generated code"

	''' <summary>
	''' Required method for Designer support - do not modify
	''' the contents of this method with the code editor.
	''' </summary>
	Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInvoicePrint))
		Dim SuperToolTip5 As DevExpress.Utils.SuperToolTip = New DevExpress.Utils.SuperToolTip()
		Dim ToolTipTitleItem7 As DevExpress.Utils.ToolTipTitleItem = New DevExpress.Utils.ToolTipTitleItem()
		Dim ToolTipItem5 As DevExpress.Utils.ToolTipItem = New DevExpress.Utils.ToolTipItem()
		Dim SuperToolTip1 As DevExpress.Utils.SuperToolTip = New DevExpress.Utils.SuperToolTip()
		Dim ToolTipTitleItem1 As DevExpress.Utils.ToolTipTitleItem = New DevExpress.Utils.ToolTipTitleItem()
		Dim ToolTipItem1 As DevExpress.Utils.ToolTipItem = New DevExpress.Utils.ToolTipItem()
		Dim ToolTipTitleItem8 As DevExpress.Utils.ToolTipTitleItem = New DevExpress.Utils.ToolTipTitleItem()
		Dim SuperToolTip2 As DevExpress.Utils.SuperToolTip = New DevExpress.Utils.SuperToolTip()
		Dim ToolTipTitleItem2 As DevExpress.Utils.ToolTipTitleItem = New DevExpress.Utils.ToolTipTitleItem()
		Dim ToolTipItem2 As DevExpress.Utils.ToolTipItem = New DevExpress.Utils.ToolTipItem()
		Dim ToolTipSeparatorItem2 As DevExpress.Utils.ToolTipSeparatorItem = New DevExpress.Utils.ToolTipSeparatorItem()
		Dim ToolTipTitleItem3 As DevExpress.Utils.ToolTipTitleItem = New DevExpress.Utils.ToolTipTitleItem()
		Dim SuperToolTip3 As DevExpress.Utils.SuperToolTip = New DevExpress.Utils.SuperToolTip()
		Dim ToolTipTitleItem4 As DevExpress.Utils.ToolTipTitleItem = New DevExpress.Utils.ToolTipTitleItem()
		Dim ToolTipItem3 As DevExpress.Utils.ToolTipItem = New DevExpress.Utils.ToolTipItem()
		Me.GroupBox1 = New DevExpress.XtraEditors.PanelControl()
		Me.sbClose = New DevExpress.XtraEditors.SimpleButton()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.lblHeaderNormal = New System.Windows.Forms.Label()
		Me.lblHeaderFett = New System.Windows.Forms.Label()
		Me.tgsSelection = New DevExpress.XtraEditors.ToggleSwitch()
		Me.grdPrint = New DevExpress.XtraGrid.GridControl()
		Me.gvPrint = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.gpSuchKriterien = New DevExpress.XtraEditors.GroupControl()
		Me.pnlSearchType = New DevExpress.XtraEditors.PanelControl()
		Me.cbEMail = New DevExpress.XtraEditors.CheckButton()
		Me.cbPrintWOS = New DevExpress.XtraEditors.CheckButton()
		Me.tpMainSetting = New DevExpress.XtraBars.Navigation.TabPane()
		Me.tnpWOSSetting = New DevExpress.XtraBars.Navigation.TabNavigationPage()
		Me.lblWarnByCopy = New DevExpress.XtraEditors.LabelControl()
		Me.lblNoEMailCustomers = New DevExpress.XtraEditors.LabelControl()
		Me.btnWOSProperty = New DevExpress.XtraEditors.DropDownButton()
		Me.lblWOSBez = New System.Windows.Forms.Label()
		Me.btnWhatToPrint = New DevExpress.XtraEditors.DropDownButton()
		Me.chk_AsCopy = New DevExpress.XtraEditors.CheckEdit()
		Me.lblWhatToPrint = New System.Windows.Forms.Label()
		Me.tgsNoEMailCustomers = New DevExpress.XtraEditors.ToggleSwitch()
		Me.tnpMailSetting = New DevExpress.XtraBars.Navigation.TabNavigationPage()
		Me.lblIndividualFiles = New DevExpress.XtraEditors.LabelControl()
		Me.tgsIndividalFiles = New DevExpress.XtraEditors.ToggleSwitch()
		Me.chkAnonymEmployee = New DevExpress.XtraEditors.CheckEdit()
		Me.cbo_KDNr = New DevExpress.XtraEditors.CheckedComboBoxEdit()
		Me.lblKDNr = New System.Windows.Forms.Label()
		Me.chkOpenAmount = New DevExpress.XtraEditors.CheckEdit()
		Me.lueMandant = New DevExpress.XtraEditors.LookUpEdit()
		Me.lblMDName = New System.Windows.Forms.Label()
		Me.lblSortierennach = New System.Windows.Forms.Label()
		Me.cbo_ReNr = New DevExpress.XtraEditors.CheckedComboBoxEdit()
		Me.lblReNr = New System.Windows.Forms.Label()
		Me.CboSort = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
		Me.Bar4 = New DevExpress.XtraBars.Bar()
		Me.bsiPrintinfo = New DevExpress.XtraBars.BarStaticItem()
		Me.bbiSearch = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiPrint = New DevExpress.XtraBars.BarLargeButtonItem()
		Me.bbiDelete = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiSendMail = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiExport = New DevExpress.XtraBars.BarButtonItem()
		Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
		Me.BarStaticItem1 = New DevExpress.XtraBars.BarStaticItem()
		Me.ImageCollection1 = New DevExpress.Utils.ImageCollection(Me.components)
		Me.AdornerUIManager1 = New DevExpress.Utils.VisualEffects.AdornerUIManager(Me.components)
		Me.BehaviorManager1 = New DevExpress.Utils.Behaviors.BehaviorManager(Me.components)
		Me.tpMainView = New DevExpress.XtraBars.Navigation.TabPane()
		Me.tnpPrint = New DevExpress.XtraBars.Navigation.TabNavigationPage()
		Me.tnpMail = New DevExpress.XtraBars.Navigation.TabNavigationPage()
		Me.grdInvoiceEMail = New DevExpress.XtraGrid.GridControl()
		Me.gvInvoiceEMail = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.GridView3 = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.tnpEMailSummery = New DevExpress.XtraBars.Navigation.TabNavigationPage()
		Me.grdEMailSummery = New DevExpress.XtraGrid.GridControl()
		Me.gvEMailSummery = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.GridView4 = New DevExpress.XtraGrid.Views.Grid.GridView()
		CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupBox1.SuspendLayout()
		CType(Me.tgsSelection.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.grdPrint, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvPrint, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gpSuchKriterien, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.gpSuchKriterien.SuspendLayout()
		CType(Me.pnlSearchType, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.pnlSearchType.SuspendLayout()
		CType(Me.tpMainSetting, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.tpMainSetting.SuspendLayout()
		Me.tnpWOSSetting.SuspendLayout()
		CType(Me.chk_AsCopy.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.tgsNoEMailCustomers.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.tnpMailSetting.SuspendLayout()
		CType(Me.tgsIndividalFiles.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkAnonymEmployee.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.cbo_KDNr.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkOpenAmount.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.cbo_ReNr.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.CboSort.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.ImageCollection1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.AdornerUIManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.BehaviorManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.tpMainView, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.tpMainView.SuspendLayout()
		Me.tnpPrint.SuspendLayout()
		Me.tnpMail.SuspendLayout()
		CType(Me.grdInvoiceEMail, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvInvoiceEMail, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.GridView3, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.tnpEMailSummery.SuspendLayout()
		CType(Me.grdEMailSummery, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvEMailSummery, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.GridView4, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'GroupBox1
		'
		Me.GroupBox1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003
		Me.GroupBox1.Controls.Add(Me.sbClose)
		Me.GroupBox1.Controls.Add(Me.Label2)
		Me.GroupBox1.Controls.Add(Me.lblHeaderNormal)
		Me.GroupBox1.Controls.Add(Me.lblHeaderFett)
		Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
		Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(1177, 77)
		Me.GroupBox1.TabIndex = 207
		'
		'sbClose
		'
		Me.sbClose.Anchor = System.Windows.Forms.AnchorStyles.Right
		Me.sbClose.Location = New System.Drawing.Point(1049, 21)
		Me.sbClose.Name = "sbClose"
		Me.sbClose.Size = New System.Drawing.Size(100, 25)
		Me.sbClose.TabIndex = 204
		Me.sbClose.Text = "Schliessen"
		'
		'Label2
		'
		Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label2.ForeColor = System.Drawing.SystemColors.HotTrack
		Me.Label2.Image = CType(resources.GetObject("Label2.Image"), System.Drawing.Image)
		Me.Label2.Location = New System.Drawing.Point(5, 8)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(83, 65)
		Me.Label2.TabIndex = 1000
		'
		'lblHeaderNormal
		'
		Me.lblHeaderNormal.BackColor = System.Drawing.Color.Transparent
		Me.lblHeaderNormal.Location = New System.Drawing.Point(112, 45)
		Me.lblHeaderNormal.Name = "lblHeaderNormal"
		Me.lblHeaderNormal.Size = New System.Drawing.Size(299, 16)
		Me.lblHeaderNormal.TabIndex = 1
		Me.lblHeaderNormal.Text = "Geben Sie bitte Ihre gewünschten Kriterien ein."
		'
		'lblHeaderFett
		'
		Me.lblHeaderFett.AutoSize = True
		Me.lblHeaderFett.BackColor = System.Drawing.Color.Transparent
		Me.lblHeaderFett.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblHeaderFett.Location = New System.Drawing.Point(94, 21)
		Me.lblHeaderFett.Name = "lblHeaderFett"
		Me.lblHeaderFett.Size = New System.Drawing.Size(269, 13)
		Me.lblHeaderFett.TabIndex = 0
		Me.lblHeaderFett.Text = "Verwaltung von Rechnungen und Gutschriften"
		'
		'tgsSelection
		'
		Me.tgsSelection.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.tgsSelection.EditValue = True
		Me.tgsSelection.Location = New System.Drawing.Point(930, 84)
		Me.tgsSelection.Name = "tgsSelection"
		Me.tgsSelection.Properties.AllowFocused = False
		Me.tgsSelection.Properties.Appearance.Options.UseTextOptions = True
		Me.tgsSelection.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.tgsSelection.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.tgsSelection.Properties.OffText = "Alles abgewählt"
		Me.tgsSelection.Properties.OnText = "Alles ausgewählt"
		Me.tgsSelection.Size = New System.Drawing.Size(214, 18)
		Me.tgsSelection.TabIndex = 320
		'
		'grdPrint
		'
		Me.grdPrint.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdPrint.Location = New System.Drawing.Point(5, 5)
		Me.grdPrint.MainView = Me.gvPrint
		Me.grdPrint.Name = "grdPrint"
		Me.grdPrint.Size = New System.Drawing.Size(558, 497)
		Me.grdPrint.TabIndex = 156
		Me.grdPrint.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvPrint, Me.GridView1})
		'
		'gvPrint
		'
		Me.gvPrint.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvPrint.GridControl = Me.grdPrint
		Me.gvPrint.Name = "gvPrint"
		Me.gvPrint.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvPrint.OptionsView.ShowAutoFilterRow = True
		Me.gvPrint.OptionsView.ShowGroupPanel = False
		'
		'GridView1
		'
		Me.GridView1.GridControl = Me.grdPrint
		Me.GridView1.Name = "GridView1"
		'
		'gpSuchKriterien
		'
		Me.gpSuchKriterien.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.gpSuchKriterien.CaptionImageOptions.SvgImage = Global.SP.Invoice.PrintUtility.My.Resources.Resources.find
		Me.gpSuchKriterien.Controls.Add(Me.pnlSearchType)
		Me.gpSuchKriterien.Controls.Add(Me.tpMainSetting)
		Me.gpSuchKriterien.Controls.Add(Me.chkAnonymEmployee)
		Me.gpSuchKriterien.Controls.Add(Me.cbo_KDNr)
		Me.gpSuchKriterien.Controls.Add(Me.lblKDNr)
		Me.gpSuchKriterien.Controls.Add(Me.chkOpenAmount)
		Me.gpSuchKriterien.Controls.Add(Me.lueMandant)
		Me.gpSuchKriterien.Controls.Add(Me.lblMDName)
		Me.gpSuchKriterien.Controls.Add(Me.lblSortierennach)
		Me.gpSuchKriterien.Controls.Add(Me.cbo_ReNr)
		Me.gpSuchKriterien.Controls.Add(Me.lblReNr)
		Me.gpSuchKriterien.Controls.Add(Me.CboSort)
		Me.gpSuchKriterien.GroupStyle = DevExpress.Utils.GroupStyle.Card
		Me.gpSuchKriterien.Location = New System.Drawing.Point(23, 110)
		Me.gpSuchKriterien.Name = "gpSuchKriterien"
		Me.gpSuchKriterien.Size = New System.Drawing.Size(533, 556)
		Me.gpSuchKriterien.TabIndex = 279
		Me.gpSuchKriterien.Text = "Suchkriterien"
		'
		'pnlSearchType
		'
		Me.pnlSearchType.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.pnlSearchType.Appearance.Options.UseBackColor = True
		Me.pnlSearchType.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
		Me.pnlSearchType.Controls.Add(Me.cbEMail)
		Me.pnlSearchType.Controls.Add(Me.cbPrintWOS)
		Me.pnlSearchType.Location = New System.Drawing.Point(17, 231)
		Me.pnlSearchType.Name = "pnlSearchType"
		Me.pnlSearchType.Size = New System.Drawing.Size(499, 63)
		Me.pnlSearchType.TabIndex = 332
		'
		'cbEMail
		'
		Me.cbEMail.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.[True]
		Me.cbEMail.AppearancePressed.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.cbEMail.AppearancePressed.Options.UseFont = True
		Me.cbEMail.GroupIndex = 1
		Me.cbEMail.ImageOptions.SvgImage = Global.SP.Invoice.PrintUtility.My.Resources.Resources.mailmerge2
		Me.cbEMail.Location = New System.Drawing.Point(250, 6)
		Me.cbEMail.Name = "cbEMail"
		Me.cbEMail.Size = New System.Drawing.Size(129, 52)
		Me.cbEMail.TabIndex = 331
		Me.cbEMail.TabStop = False
		Me.cbEMail.Text = "Email-Versand"
		'
		'cbPrintWOS
		'
		Me.cbPrintWOS.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.[True]
		Me.cbPrintWOS.AppearancePressed.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.cbPrintWOS.AppearancePressed.Options.UseFont = True
		Me.cbPrintWOS.GroupIndex = 1
		Me.cbPrintWOS.ImageOptions.SvgImage = Global.SP.Invoice.PrintUtility.My.Resources.Resources.print1
		Me.cbPrintWOS.Location = New System.Drawing.Point(115, 6)
		Me.cbPrintWOS.Name = "cbPrintWOS"
		Me.cbPrintWOS.Size = New System.Drawing.Size(129, 52)
		Me.cbPrintWOS.TabIndex = 330
		Me.cbPrintWOS.TabStop = False
		Me.cbPrintWOS.Text = "Drucken / WOS"
		'
		'tpMainSetting
		'
		Me.tpMainSetting.AllowHtmlDraw = True
		Me.tpMainSetting.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.tpMainSetting.Controls.Add(Me.tnpWOSSetting)
		Me.tpMainSetting.Controls.Add(Me.tnpMailSetting)
		Me.tpMainSetting.Location = New System.Drawing.Point(17, 308)
		Me.tpMainSetting.Name = "tpMainSetting"
		Me.tpMainSetting.Pages.AddRange(New DevExpress.XtraBars.Navigation.NavigationPageBase() {Me.tnpWOSSetting, Me.tnpMailSetting})
		Me.tpMainSetting.RegularSize = New System.Drawing.Size(499, 240)
		Me.tpMainSetting.SelectedPage = Me.tnpWOSSetting
		Me.tpMainSetting.Size = New System.Drawing.Size(499, 240)
		Me.tpMainSetting.TabIndex = 327
		Me.tpMainSetting.Text = "Allgemein"
		'
		'tnpWOSSetting
		'
		Me.tnpWOSSetting.Caption = "Drucken / WOS senden"
		Me.tnpWOSSetting.Controls.Add(Me.lblWarnByCopy)
		Me.tnpWOSSetting.Controls.Add(Me.lblNoEMailCustomers)
		Me.tnpWOSSetting.Controls.Add(Me.btnWOSProperty)
		Me.tnpWOSSetting.Controls.Add(Me.lblWOSBez)
		Me.tnpWOSSetting.Controls.Add(Me.btnWhatToPrint)
		Me.tnpWOSSetting.Controls.Add(Me.chk_AsCopy)
		Me.tnpWOSSetting.Controls.Add(Me.lblWhatToPrint)
		Me.tnpWOSSetting.Controls.Add(Me.tgsNoEMailCustomers)
		Me.tnpWOSSetting.ImageOptions.SvgImage = Global.SP.Invoice.PrintUtility.My.Resources.Resources.print
		Me.tnpWOSSetting.ItemShowMode = DevExpress.XtraBars.Navigation.ItemShowMode.ImageAndText
		Me.tnpWOSSetting.Name = "tnpWOSSetting"
		Me.tnpWOSSetting.Properties.ShowMode = DevExpress.XtraBars.Navigation.ItemShowMode.ImageAndText
		Me.tnpWOSSetting.Size = New System.Drawing.Size(499, 201)
		'
		'lblWarnByCopy
		'
		Me.lblWarnByCopy.AllowHtmlString = True
		Me.lblWarnByCopy.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.lblWarnByCopy.Appearance.Image = CType(resources.GetObject("lblWarnByCopy.Appearance.Image"), System.Drawing.Image)
		Me.lblWarnByCopy.Appearance.Options.UseBackColor = True
		Me.lblWarnByCopy.Appearance.Options.UseImage = True
		Me.lblWarnByCopy.Appearance.Options.UseTextOptions = True
		Me.lblWarnByCopy.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter
		Me.lblWarnByCopy.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
		Me.lblWarnByCopy.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
		Me.lblWarnByCopy.AutoEllipsis = True
		Me.lblWarnByCopy.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblWarnByCopy.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftTop
		Me.lblWarnByCopy.ImageOptions.Image = Global.SP.Invoice.PrintUtility.My.Resources.Resources.warning_16x161
		Me.lblWarnByCopy.ImageOptions.SvgImage = CType(resources.GetObject("lblWarnByCopy.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
		Me.lblWarnByCopy.LineColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
		Me.lblWarnByCopy.LineLocation = DevExpress.XtraEditors.LineLocation.Bottom
		Me.lblWarnByCopy.LineOrientation = DevExpress.XtraEditors.LabelLineOrientation.Vertical
		Me.lblWarnByCopy.LineVisible = True
		Me.lblWarnByCopy.Location = New System.Drawing.Point(17, 79)
		Me.lblWarnByCopy.Name = "lblWarnByCopy"
		Me.lblWarnByCopy.Size = New System.Drawing.Size(463, 41)
		Me.lblWarnByCopy.TabIndex = 334
		Me.lblWarnByCopy.Text = "Es werden <b>nur</b> die Rechnungsdetail mit Vermerk ""Kopie"" gedruckt.<br>Die Ein" &
	"zahlungsscheine werden <b>nicht</b> gedruckt."
		'
		'lblNoEMailCustomers
		'
		Me.lblNoEMailCustomers.AllowHtmlString = True
		Me.lblNoEMailCustomers.Appearance.Options.UseTextOptions = True
		Me.lblNoEMailCustomers.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblNoEMailCustomers.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblNoEMailCustomers.Location = New System.Drawing.Point(17, 165)
		Me.lblNoEMailCustomers.Name = "lblNoEMailCustomers"
		Me.lblNoEMailCustomers.Size = New System.Drawing.Size(166, 13)
		Me.lblNoEMailCustomers.TabIndex = 333
		Me.lblNoEMailCustomers.Text = "Rechnungen mit Email-Adresse"
		'
		'btnWOSProperty
		'
		Me.btnWOSProperty.Location = New System.Drawing.Point(189, 126)
		Me.btnWOSProperty.Name = "btnWOSProperty"
		Me.btnWOSProperty.Size = New System.Drawing.Size(280, 25)
		Me.btnWOSProperty.TabIndex = 4
		Me.btnWOSProperty.Text = "Drucken"
		'
		'lblWOSBez
		'
		Me.lblWOSBez.BackColor = System.Drawing.Color.Transparent
		Me.lblWOSBez.Location = New System.Drawing.Point(33, 132)
		Me.lblWOSBez.Name = "lblWOSBez"
		Me.lblWOSBez.Size = New System.Drawing.Size(150, 13)
		Me.lblWOSBez.TabIndex = 276
		Me.lblWOSBez.Text = "WOS-Kunden"
		Me.lblWOSBez.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'btnWhatToPrint
		'
		Me.btnWhatToPrint.Location = New System.Drawing.Point(189, 23)
		Me.btnWhatToPrint.Name = "btnWhatToPrint"
		Me.btnWhatToPrint.Size = New System.Drawing.Size(280, 25)
		Me.btnWhatToPrint.TabIndex = 292
		Me.btnWhatToPrint.Text = "Detail + Einzahlungsscheine"
		'
		'chk_AsCopy
		'
		Me.chk_AsCopy.AllowHtmlTextInToolTip = DevExpress.Utils.DefaultBoolean.[True]
		Me.chk_AsCopy.Location = New System.Drawing.Point(189, 54)
		Me.chk_AsCopy.Name = "chk_AsCopy"
		Me.chk_AsCopy.Properties.AllowFocused = False
		Me.chk_AsCopy.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.[True]
		Me.chk_AsCopy.Properties.Caption = "Als <b>KOPIE</b> ausdrucken"
		Me.chk_AsCopy.Size = New System.Drawing.Size(240, 20)
		SuperToolTip5.AllowHtmlText = DevExpress.Utils.DefaultBoolean.[True]
		ToolTipTitleItem7.Appearance.Image = CType(resources.GetObject("resource.Image"), System.Drawing.Image)
		ToolTipTitleItem7.Appearance.Options.UseImage = True
		ToolTipTitleItem7.ImageOptions.Image = CType(resources.GetObject("resource.Image1"), System.Drawing.Image)
		ToolTipTitleItem7.Text = "Kopie"
		ToolTipItem5.LeftIndent = 6
		ToolTipItem5.Text = "Kopie/Es werden <b>nur</b> die Rechnungsdetail mit Vermerk ""Kopie"" gedruckt. Die " &
	"Einzahlungsscheine werden <b>nicht</b> gedruckt." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
		SuperToolTip5.Items.Add(ToolTipTitleItem7)
		SuperToolTip5.Items.Add(ToolTipItem5)
		Me.chk_AsCopy.SuperTip = SuperToolTip5
		Me.chk_AsCopy.TabIndex = 281
		Me.chk_AsCopy.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information
		'
		'lblWhatToPrint
		'
		Me.lblWhatToPrint.BackColor = System.Drawing.Color.Transparent
		Me.lblWhatToPrint.Location = New System.Drawing.Point(33, 29)
		Me.lblWhatToPrint.Name = "lblWhatToPrint"
		Me.lblWhatToPrint.Size = New System.Drawing.Size(150, 13)
		Me.lblWhatToPrint.TabIndex = 293
		Me.lblWhatToPrint.Text = "Was möchten Sie drucken?"
		Me.lblWhatToPrint.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'tgsNoEMailCustomers
		'
		Me.tgsNoEMailCustomers.Location = New System.Drawing.Point(189, 162)
		Me.tgsNoEMailCustomers.Name = "tgsNoEMailCustomers"
		Me.tgsNoEMailCustomers.Properties.AllowFocused = False
		Me.tgsNoEMailCustomers.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.[True]
		Me.tgsNoEMailCustomers.Properties.Appearance.Options.UseTextOptions = True
		Me.tgsNoEMailCustomers.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
		Me.tgsNoEMailCustomers.Properties.OffText = "werden eingeschlossen"
		Me.tgsNoEMailCustomers.Properties.OnText = "werden ausgeschlossen"
		Me.tgsNoEMailCustomers.Size = New System.Drawing.Size(210, 18)
		Me.tgsNoEMailCustomers.TabIndex = 298
		Me.tgsNoEMailCustomers.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information
		'
		'tnpMailSetting
		'
		Me.tnpMailSetting.Caption = "Mail-Versand"
		Me.tnpMailSetting.Controls.Add(Me.lblIndividualFiles)
		Me.tnpMailSetting.Controls.Add(Me.tgsIndividalFiles)
		Me.tnpMailSetting.ImageOptions.SvgImage = Global.SP.Invoice.PrintUtility.My.Resources.Resources.mailmerge1
		Me.tnpMailSetting.ItemShowMode = DevExpress.XtraBars.Navigation.ItemShowMode.ImageAndText
		Me.tnpMailSetting.Name = "tnpMailSetting"
		Me.tnpMailSetting.PageText = "Mail-Versand"
		Me.tnpMailSetting.Properties.ShowMode = DevExpress.XtraBars.Navigation.ItemShowMode.ImageAndText
		Me.tnpMailSetting.Size = New System.Drawing.Size(499, 184)
		'
		'lblIndividualFiles
		'
		Me.lblIndividualFiles.AllowHtmlString = True
		Me.lblIndividualFiles.Appearance.Options.UseTextOptions = True
		Me.lblIndividualFiles.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblIndividualFiles.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblIndividualFiles.Location = New System.Drawing.Point(3, 32)
		Me.lblIndividualFiles.Name = "lblIndividualFiles"
		Me.lblIndividualFiles.Size = New System.Drawing.Size(180, 13)
		Me.lblIndividualFiles.TabIndex = 337
		Me.lblIndividualFiles.Text = "Anhänge werden als "
		'
		'tgsIndividalFiles
		'
		Me.tgsIndividalFiles.Location = New System.Drawing.Point(189, 29)
		Me.tgsIndividalFiles.Name = "tgsIndividalFiles"
		Me.tgsIndividalFiles.Properties.AllowFocused = False
		Me.tgsIndividalFiles.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.[True]
		Me.tgsIndividalFiles.Properties.OffText = "separate PDF gesendet"
		Me.tgsIndividalFiles.Properties.OnText = "eine ZIP Datei gesendet"
		Me.tgsIndividalFiles.Size = New System.Drawing.Size(194, 18)
		Me.tgsIndividalFiles.TabIndex = 336
		Me.tgsIndividalFiles.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information
		'
		'chkAnonymEmployee
		'
		Me.chkAnonymEmployee.AllowHtmlTextInToolTip = DevExpress.Utils.DefaultBoolean.[True]
		Me.chkAnonymEmployee.Location = New System.Drawing.Point(206, 197)
		Me.chkAnonymEmployee.Name = "chkAnonymEmployee"
		Me.chkAnonymEmployee.Properties.AllowFocused = False
		Me.chkAnonymEmployee.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.[True]
		Me.chkAnonymEmployee.Properties.Caption = "Kandidatennamen: <b>unsichtbar</b>"
		Me.chkAnonymEmployee.Size = New System.Drawing.Size(280, 20)
		SuperToolTip1.AllowHtmlText = DevExpress.Utils.DefaultBoolean.[True]
		ToolTipTitleItem1.ImageOptions.Image = CType(resources.GetObject("resource.Image2"), System.Drawing.Image)
		ToolTipTitleItem1.Text = "Anonymisieren"
		ToolTipItem1.LeftIndent = 6
		ToolTipItem1.Text = "Wenn aktiviert, dann blendet das System die Kandidatennamen aus."
		ToolTipTitleItem8.ImageOptions.Image = CType(resources.GetObject("resource.Image3"), System.Drawing.Image)
		ToolTipTitleItem8.LeftIndent = 6
		SuperToolTip1.Items.Add(ToolTipTitleItem1)
		SuperToolTip1.Items.Add(ToolTipItem1)
		SuperToolTip1.Items.Add(ToolTipTitleItem8)
		Me.chkAnonymEmployee.SuperTip = SuperToolTip1
		Me.chkAnonymEmployee.TabIndex = 297
		Me.chkAnonymEmployee.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information
		'
		'cbo_KDNr
		'
		Me.cbo_KDNr.Location = New System.Drawing.Point(206, 120)
		Me.cbo_KDNr.Name = "cbo_KDNr"
		Me.cbo_KDNr.Properties.DropDownRows = 20
		Me.cbo_KDNr.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
		Me.cbo_KDNr.Size = New System.Drawing.Size(280, 20)
		Me.cbo_KDNr.TabIndex = 295
		'
		'lblKDNr
		'
		Me.lblKDNr.BackColor = System.Drawing.Color.Transparent
		Me.lblKDNr.Location = New System.Drawing.Point(50, 124)
		Me.lblKDNr.Name = "lblKDNr"
		Me.lblKDNr.Size = New System.Drawing.Size(150, 13)
		Me.lblKDNr.TabIndex = 296
		Me.lblKDNr.Text = "Kunden-Nr."
		Me.lblKDNr.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'chkOpenAmount
		'
		Me.chkOpenAmount.AllowHtmlTextInToolTip = DevExpress.Utils.DefaultBoolean.[True]
		Me.chkOpenAmount.Location = New System.Drawing.Point(206, 172)
		Me.chkOpenAmount.Name = "chkOpenAmount"
		Me.chkOpenAmount.Properties.AllowFocused = False
		Me.chkOpenAmount.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.[True]
		Me.chkOpenAmount.Properties.Caption = "Offener Rechnungsbetrag drucken?"
		Me.chkOpenAmount.Size = New System.Drawing.Size(280, 20)
		SuperToolTip2.AllowHtmlText = DevExpress.Utils.DefaultBoolean.[True]
		ToolTipTitleItem2.Appearance.Image = CType(resources.GetObject("resource.Image4"), System.Drawing.Image)
		ToolTipTitleItem2.Appearance.Options.UseImage = True
		ToolTipTitleItem2.ImageOptions.Image = CType(resources.GetObject("resource.Image5"), System.Drawing.Image)
		ToolTipTitleItem2.Text = "Offener Betrag"
		ToolTipItem2.LeftIndent = 6
		ToolTipItem2.Text = "Druckt den offenen Rechnungsbetrag auf Einzahlungsschein."
		ToolTipTitleItem3.Appearance.Image = CType(resources.GetObject("resource.Image6"), System.Drawing.Image)
		ToolTipTitleItem3.Appearance.Options.UseImage = True
		ToolTipTitleItem3.ImageOptions.Image = CType(resources.GetObject("resource.Image7"), System.Drawing.Image)
		ToolTipTitleItem3.LeftIndent = 6
		ToolTipTitleItem3.Text = "Achtung: die Vorlage für Rechnungsdetail muss angepasst werden!"
		SuperToolTip2.Items.Add(ToolTipTitleItem2)
		SuperToolTip2.Items.Add(ToolTipItem2)
		SuperToolTip2.Items.Add(ToolTipSeparatorItem2)
		SuperToolTip2.Items.Add(ToolTipTitleItem3)
		Me.chkOpenAmount.SuperTip = SuperToolTip2
		Me.chkOpenAmount.TabIndex = 294
		Me.chkOpenAmount.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information
		'
		'lueMandant
		'
		Me.lueMandant.Location = New System.Drawing.Point(206, 50)
		Me.lueMandant.Name = "lueMandant"
		Me.lueMandant.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.lueMandant.Properties.NullText = ""
		Me.lueMandant.Size = New System.Drawing.Size(280, 20)
		Me.lueMandant.TabIndex = 280
		'
		'lblMDName
		'
		Me.lblMDName.Location = New System.Drawing.Point(50, 54)
		Me.lblMDName.Name = "lblMDName"
		Me.lblMDName.Size = New System.Drawing.Size(150, 13)
		Me.lblMDName.TabIndex = 279
		Me.lblMDName.Text = "Mandanten"
		Me.lblMDName.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblSortierennach
		'
		Me.lblSortierennach.Location = New System.Drawing.Point(50, 80)
		Me.lblSortierennach.Name = "lblSortierennach"
		Me.lblSortierennach.Size = New System.Drawing.Size(150, 13)
		Me.lblSortierennach.TabIndex = 277
		Me.lblSortierennach.Text = "Sortieren nach"
		Me.lblSortierennach.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'cbo_ReNr
		'
		Me.cbo_ReNr.Location = New System.Drawing.Point(206, 146)
		Me.cbo_ReNr.Name = "cbo_ReNr"
		Me.cbo_ReNr.Properties.DropDownRows = 20
		Me.cbo_ReNr.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
		Me.cbo_ReNr.Size = New System.Drawing.Size(280, 20)
		SuperToolTip3.AllowHtmlText = DevExpress.Utils.DefaultBoolean.[True]
		ToolTipTitleItem4.ImageOptions.Image = Global.SP.Invoice.PrintUtility.My.Resources.Resources.info_16x16
		ToolTipTitleItem4.Text = "<b>Eingabe der Nummerfelder</b>"
		ToolTipItem3.LeftIndent = 6
		SuperToolTip3.Items.Add(ToolTipTitleItem4)
		SuperToolTip3.Items.Add(ToolTipItem3)
		Me.cbo_ReNr.SuperTip = SuperToolTip3
		Me.cbo_ReNr.TabIndex = 0
		'
		'lblReNr
		'
		Me.lblReNr.BackColor = System.Drawing.Color.Transparent
		Me.lblReNr.Location = New System.Drawing.Point(50, 150)
		Me.lblReNr.Name = "lblReNr"
		Me.lblReNr.Size = New System.Drawing.Size(150, 13)
		Me.lblReNr.TabIndex = 63
		Me.lblReNr.Text = "Rechnung-Nr."
		Me.lblReNr.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'CboSort
		'
		Me.CboSort.Location = New System.Drawing.Point(206, 76)
		Me.CboSort.Name = "CboSort"
		Me.CboSort.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.CboSort.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
		Me.CboSort.Size = New System.Drawing.Size(280, 20)
		Me.CboSort.TabIndex = 278
		'
		'BarManager1
		'
		Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar4})
		Me.BarManager1.DockControls.Add(Me.barDockControlTop)
		Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
		Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
		Me.BarManager1.DockControls.Add(Me.barDockControlRight)
		Me.BarManager1.Form = Me
		Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiPrintinfo, Me.bbiSearch, Me.bbiPrint, Me.bbiDelete, Me.BarStaticItem1, Me.bbiExport, Me.bbiSendMail})
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
		Me.Bar4.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiPrintinfo), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiSearch), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiPrint, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiDelete, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiSendMail, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiExport)})
		Me.Bar4.OptionsBar.AllowQuickCustomization = False
		Me.Bar4.OptionsBar.DrawDragBorder = False
		Me.Bar4.OptionsBar.UseWholeRow = True
		Me.Bar4.Text = "Statusleiste"
		'
		'bsiPrintinfo
		'
		Me.bsiPrintinfo.AutoSize = DevExpress.XtraBars.BarStaticItemSize.Spring
		Me.bsiPrintinfo.Caption = "Bereit"
		Me.bsiPrintinfo.Id = 0
		Me.bsiPrintinfo.Name = "bsiPrintinfo"
		Me.bsiPrintinfo.Size = New System.Drawing.Size(32, 0)
		Me.bsiPrintinfo.Width = 32
		'
		'bbiSearch
		'
		Me.bbiSearch.Caption = "Suchen"
		Me.bbiSearch.Id = 1
		Me.bbiSearch.ImageOptions.Image = CType(resources.GetObject("bbiSearch.ImageOptions.Image"), System.Drawing.Image)
		Me.bbiSearch.ImageOptions.LargeImage = CType(resources.GetObject("bbiSearch.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.bbiSearch.Name = "bbiSearch"
		Me.bbiSearch.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiPrint
		'
		Me.bbiPrint.Caption = "Drucken"
		Me.bbiPrint.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Right
		Me.bbiPrint.Id = 6
		Me.bbiPrint.ImageOptions.Image = CType(resources.GetObject("bbiPrint.ImageOptions.Image"), System.Drawing.Image)
		Me.bbiPrint.Name = "bbiPrint"
		Me.bbiPrint.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiDelete
		'
		Me.bbiDelete.Caption = "Löschen"
		Me.bbiDelete.Id = 8
		Me.bbiDelete.ImageOptions.Image = CType(resources.GetObject("bbiDelete.ImageOptions.Image"), System.Drawing.Image)
		Me.bbiDelete.Name = "bbiDelete"
		Me.bbiDelete.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiSendMail
		'
		Me.bbiSendMail.Caption = "Email-Versand zusammenstellen"
		Me.bbiSendMail.Id = 13
		Me.bbiSendMail.ImageOptions.Image = Global.SP.Invoice.PrintUtility.My.Resources.Resources.mailmerge_16x16
		Me.bbiSendMail.ImageOptions.LargeImage = Global.SP.Invoice.PrintUtility.My.Resources.Resources.mailmerge_32x32
		Me.bbiSendMail.Name = "bbiSendMail"
		Me.bbiSendMail.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiExport
		'
		Me.bbiExport.Caption = "Exportieren"
		Me.bbiExport.Id = 4
		Me.bbiExport.ImageOptions.Image = CType(resources.GetObject("bbiExport.ImageOptions.Image"), System.Drawing.Image)
		Me.bbiExport.ImageOptions.LargeImage = CType(resources.GetObject("bbiExport.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.bbiExport.Name = "bbiExport"
		Me.bbiExport.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'barDockControlTop
		'
		Me.barDockControlTop.CausesValidation = False
		Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
		Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlTop.Manager = Me.BarManager1
		Me.barDockControlTop.Size = New System.Drawing.Size(1177, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 696)
		Me.barDockControlBottom.Manager = Me.BarManager1
		Me.barDockControlBottom.Size = New System.Drawing.Size(1177, 26)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Manager = Me.BarManager1
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 696)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(1177, 0)
		Me.barDockControlRight.Manager = Me.BarManager1
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 696)
		'
		'BarStaticItem1
		'
		Me.BarStaticItem1.Caption = " "
		Me.BarStaticItem1.Id = 5
		Me.BarStaticItem1.Name = "BarStaticItem1"
		'
		'ImageCollection1
		'
		Me.ImageCollection1.ImageStream = CType(resources.GetObject("ImageCollection1.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
		Me.ImageCollection1.InsertGalleryImage("info_16x16.png", "images/support/info_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/support/info_16x16.png"), 0)
		Me.ImageCollection1.Images.SetKeyName(0, "info_16x16.png")
		'
		'AdornerUIManager1
		'
		Me.AdornerUIManager1.Owner = Me
		'
		'tpMainView
		'
		Me.tpMainView.AllowHtmlDraw = True
		Me.tpMainView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.tpMainView.Controls.Add(Me.tnpPrint)
		Me.tpMainView.Controls.Add(Me.tnpMail)
		Me.tpMainView.Controls.Add(Me.tnpEMailSummery)
		Me.tpMainView.Location = New System.Drawing.Point(581, 110)
		Me.tpMainView.Name = "tpMainView"
		Me.tpMainView.Pages.AddRange(New DevExpress.XtraBars.Navigation.NavigationPageBase() {Me.tnpPrint, Me.tnpMail, Me.tnpEMailSummery})
		Me.tpMainView.RegularSize = New System.Drawing.Size(568, 556)
		Me.tpMainView.SelectedPage = Me.tnpMail
		Me.tpMainView.Size = New System.Drawing.Size(568, 556)
		Me.tpMainView.TabIndex = 332
		Me.tpMainView.Text = "TabPane1"
		'
		'tnpPrint
		'
		Me.tnpPrint.Caption = "Detail zum Drucken"
		Me.tnpPrint.Controls.Add(Me.grdPrint)
		Me.tnpPrint.Name = "tnpPrint"
		Me.tnpPrint.Padding = New System.Windows.Forms.Padding(5)
		Me.tnpPrint.Size = New System.Drawing.Size(568, 507)
		'
		'tnpMail
		'
		Me.tnpMail.Caption = "Detail zum EMail-Versand"
		Me.tnpMail.Controls.Add(Me.grdInvoiceEMail)
		Me.tnpMail.Name = "tnpMail"
		Me.tnpMail.Padding = New System.Windows.Forms.Padding(5)
		Me.tnpMail.Size = New System.Drawing.Size(568, 527)
		'
		'grdInvoiceEMail
		'
		Me.grdInvoiceEMail.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdInvoiceEMail.Location = New System.Drawing.Point(5, 5)
		Me.grdInvoiceEMail.MainView = Me.gvInvoiceEMail
		Me.grdInvoiceEMail.Name = "grdInvoiceEMail"
		Me.grdInvoiceEMail.Size = New System.Drawing.Size(558, 517)
		Me.grdInvoiceEMail.TabIndex = 157
		Me.grdInvoiceEMail.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvInvoiceEMail, Me.GridView3})
		'
		'gvInvoiceEMail
		'
		Me.gvInvoiceEMail.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvInvoiceEMail.GridControl = Me.grdInvoiceEMail
		Me.gvInvoiceEMail.Name = "gvInvoiceEMail"
		Me.gvInvoiceEMail.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvInvoiceEMail.OptionsView.ShowAutoFilterRow = True
		Me.gvInvoiceEMail.OptionsView.ShowGroupPanel = False
		'
		'GridView3
		'
		Me.GridView3.GridControl = Me.grdInvoiceEMail
		Me.GridView3.Name = "GridView3"
		'
		'tnpEMailSummery
		'
		Me.tnpEMailSummery.Caption = "Zusammenstellung der EMail-Versand"
		Me.tnpEMailSummery.Controls.Add(Me.grdEMailSummery)
		Me.tnpEMailSummery.Name = "tnpEMailSummery"
		Me.tnpEMailSummery.Padding = New System.Windows.Forms.Padding(5)
		Me.tnpEMailSummery.Size = New System.Drawing.Size(568, 507)
		'
		'grdEMailSummery
		'
		Me.grdEMailSummery.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdEMailSummery.Location = New System.Drawing.Point(5, 5)
		Me.grdEMailSummery.MainView = Me.gvEMailSummery
		Me.grdEMailSummery.Name = "grdEMailSummery"
		Me.grdEMailSummery.Size = New System.Drawing.Size(558, 497)
		Me.grdEMailSummery.TabIndex = 158
		Me.grdEMailSummery.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvEMailSummery, Me.GridView4})
		'
		'gvEMailSummery
		'
		Me.gvEMailSummery.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvEMailSummery.GridControl = Me.grdEMailSummery
		Me.gvEMailSummery.Name = "gvEMailSummery"
		Me.gvEMailSummery.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvEMailSummery.OptionsView.ShowAutoFilterRow = True
		Me.gvEMailSummery.OptionsView.ShowGroupPanel = False
		'
		'GridView4
		'
		Me.GridView4.GridControl = Me.grdEMailSummery
		Me.GridView4.Name = "GridView4"
		'
		'frmInvoicePrint
		'
		Me.ClientSize = New System.Drawing.Size(1177, 722)
		Me.Controls.Add(Me.tpMainView)
		Me.Controls.Add(Me.tgsSelection)
		Me.Controls.Add(Me.gpSuchKriterien)
		Me.Controls.Add(Me.GroupBox1)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.IconOptions.Icon = CType(resources.GetObject("frmInvoicePrint.IconOptions.Icon"), System.Drawing.Icon)
		Me.MinimumSize = New System.Drawing.Size(1179, 754)
		Me.Name = "frmInvoicePrint"
		Me.Text = "Verwaltung von Rechnungen, Gutschriften und Rückvergütungen"
		CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupBox1.ResumeLayout(False)
		Me.GroupBox1.PerformLayout()
		CType(Me.tgsSelection.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.grdPrint, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvPrint, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gpSuchKriterien, System.ComponentModel.ISupportInitialize).EndInit()
		Me.gpSuchKriterien.ResumeLayout(False)
		CType(Me.pnlSearchType, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pnlSearchType.ResumeLayout(False)
		CType(Me.tpMainSetting, System.ComponentModel.ISupportInitialize).EndInit()
		Me.tpMainSetting.ResumeLayout(False)
		Me.tnpWOSSetting.ResumeLayout(False)
		CType(Me.chk_AsCopy.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.tgsNoEMailCustomers.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.tnpMailSetting.ResumeLayout(False)
		CType(Me.tgsIndividalFiles.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkAnonymEmployee.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.cbo_KDNr.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkOpenAmount.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.cbo_ReNr.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.CboSort.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.ImageCollection1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.AdornerUIManager1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.BehaviorManager1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.tpMainView, System.ComponentModel.ISupportInitialize).EndInit()
		Me.tpMainView.ResumeLayout(False)
		Me.tnpPrint.ResumeLayout(False)
		Me.tnpMail.ResumeLayout(False)
		CType(Me.grdInvoiceEMail, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvInvoiceEMail, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.GridView3, System.ComponentModel.ISupportInitialize).EndInit()
		Me.tnpEMailSummery.ResumeLayout(False)
		CType(Me.grdEMailSummery, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvEMailSummery, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.GridView4, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents GroupBox1 As DevExpress.XtraEditors.PanelControl
	Friend WithEvents sbClose As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents Label2 As System.Windows.Forms.Label
	Friend WithEvents lblHeaderNormal As System.Windows.Forms.Label
	Friend WithEvents lblHeaderFett As System.Windows.Forms.Label
	Friend WithEvents tgsSelection As DevExpress.XtraEditors.ToggleSwitch
	Friend WithEvents grdPrint As DevExpress.XtraGrid.GridControl
	Private WithEvents gvPrint As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents gpSuchKriterien As DevExpress.XtraEditors.GroupControl
	Friend WithEvents lueMandant As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents lblMDName As System.Windows.Forms.Label
	Friend WithEvents lblSortierennach As System.Windows.Forms.Label
	Friend WithEvents lblWOSBez As System.Windows.Forms.Label
	Friend WithEvents btnWOSProperty As DevExpress.XtraEditors.DropDownButton
	Friend WithEvents cbo_ReNr As DevExpress.XtraEditors.CheckedComboBoxEdit
	Friend WithEvents lblReNr As System.Windows.Forms.Label
	Friend WithEvents CboSort As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
	Friend WithEvents Bar4 As DevExpress.XtraBars.Bar
	Friend WithEvents bsiPrintinfo As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents bbiSearch As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbiPrint As DevExpress.XtraBars.BarLargeButtonItem
	Friend WithEvents bbiDelete As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbiExport As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
	Friend WithEvents BarStaticItem1 As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents chk_AsCopy As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents lblWhatToPrint As System.Windows.Forms.Label
	Friend WithEvents btnWhatToPrint As DevExpress.XtraEditors.DropDownButton
	Friend WithEvents ImageCollection1 As DevExpress.Utils.ImageCollection
	Friend WithEvents chkOpenAmount As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents cbo_KDNr As DevExpress.XtraEditors.CheckedComboBoxEdit
	Friend WithEvents lblKDNr As Label
	Friend WithEvents chkAnonymEmployee As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents AdornerUIManager1 As DevExpress.Utils.VisualEffects.AdornerUIManager
	Friend WithEvents tpMainSetting As DevExpress.XtraBars.Navigation.TabPane
	Friend WithEvents tnpWOSSetting As DevExpress.XtraBars.Navigation.TabNavigationPage
	Friend WithEvents tnpMailSetting As DevExpress.XtraBars.Navigation.TabNavigationPage
	Friend WithEvents BehaviorManager1 As DevExpress.Utils.Behaviors.BehaviorManager
	Friend WithEvents bbiSendMail As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents tpMainView As DevExpress.XtraBars.Navigation.TabPane
	Friend WithEvents tnpPrint As DevExpress.XtraBars.Navigation.TabNavigationPage
	Friend WithEvents tnpMail As DevExpress.XtraBars.Navigation.TabNavigationPage
	Friend WithEvents grdInvoiceEMail As DevExpress.XtraGrid.GridControl
	Private WithEvents gvInvoiceEMail As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents GridView3 As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents tnpEMailSummery As DevExpress.XtraBars.Navigation.TabNavigationPage
	Friend WithEvents grdEMailSummery As DevExpress.XtraGrid.GridControl
	Private WithEvents gvEMailSummery As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents GridView4 As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents cbPrintWOS As DevExpress.XtraEditors.CheckButton
	Friend WithEvents cbEMail As DevExpress.XtraEditors.CheckButton
	Friend WithEvents pnlSearchType As DevExpress.XtraEditors.PanelControl
	Friend WithEvents lblNoEMailCustomers As DevExpress.XtraEditors.LabelControl
	Friend WithEvents tgsNoEMailCustomers As DevExpress.XtraEditors.ToggleSwitch
	Friend WithEvents lblWarnByCopy As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblIndividualFiles As DevExpress.XtraEditors.LabelControl
	Friend WithEvents tgsIndividalFiles As DevExpress.XtraEditors.ToggleSwitch

#End Region

End Class
