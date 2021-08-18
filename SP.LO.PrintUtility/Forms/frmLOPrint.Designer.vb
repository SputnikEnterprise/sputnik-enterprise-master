<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLOPrint
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
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLOPrint))
		Dim EditorButtonImageOptions1 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
		Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim EditorButtonImageOptions2 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
		Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim ButtonImageOptions1 As DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions = New DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions()
		Dim SuperToolTip1 As DevExpress.Utils.SuperToolTip = New DevExpress.Utils.SuperToolTip()
		Dim ToolTipItem1 As DevExpress.Utils.ToolTipItem = New DevExpress.Utils.ToolTipItem()
		Me.LblState_1 = New System.Windows.Forms.ToolStripStatusLabel()
		Me.lblHeaderFett = New System.Windows.Forms.Label()
		Me.Label14 = New System.Windows.Forms.Label()
		Me.Label15 = New System.Windows.Forms.Label()
		Me.Label16 = New System.Windows.Forms.Label()
		Me.Label17 = New System.Windows.Forms.Label()
		Me.Label18 = New System.Windows.Forms.Label()
		Me.Label19 = New System.Windows.Forms.Label()
		Me.Label20 = New System.Windows.Forms.Label()
		Me.Label21 = New System.Windows.Forms.Label()
		Me.Label22 = New System.Windows.Forms.Label()
		Me.Label23 = New System.Windows.Forms.Label()
		Me.Label24 = New System.Windows.Forms.Label()
		Me.TextBox13 = New System.Windows.Forms.TextBox()
		Me.TextBox14 = New System.Windows.Forms.TextBox()
		Me.TextBox15 = New System.Windows.Forms.TextBox()
		Me.TextBox16 = New System.Windows.Forms.TextBox()
		Me.TextBox17 = New System.Windows.Forms.TextBox()
		Me.TextBox18 = New System.Windows.Forms.TextBox()
		Me.TextBox19 = New System.Windows.Forms.TextBox()
		Me.TextBox20 = New System.Windows.Forms.TextBox()
		Me.TextBox21 = New System.Windows.Forms.TextBox()
		Me.TextBox22 = New System.Windows.Forms.TextBox()
		Me.TextBox23 = New System.Windows.Forms.TextBox()
		Me.ComboBox2 = New System.Windows.Forms.ComboBox()
		Me.TextBox24 = New System.Windows.Forms.TextBox()
		Me.lbljahr = New System.Windows.Forms.Label()
		Me.lblLohnNr = New System.Windows.Forms.Label()
		Me.lblMonat = New System.Windows.Forms.Label()
		Me.lblKandidatenNr = New System.Windows.Forms.Label()
		Me.StyleManager1 = New DevComponents.DotNetBar.StyleManager(Me.components)
		Me.cbo_MANr = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.cbo_LONr = New DevExpress.XtraEditors.CheckedComboBoxEdit()
		Me.GroupBox1 = New DevExpress.XtraEditors.PanelControl()
		Me.sbClose = New DevExpress.XtraEditors.SimpleButton()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.lblHeaderNormal = New System.Windows.Forms.Label()
		Me.gpSuchKriterien = New DevExpress.XtraEditors.GroupControl()
		Me.pnlSearchType = New DevExpress.XtraEditors.PanelControl()
		Me.cbEMail = New DevExpress.XtraEditors.CheckButton()
		Me.cbPrintWOS = New DevExpress.XtraEditors.CheckButton()
		Me.tpMainSetting = New DevExpress.XtraBars.Navigation.TabPane()
		Me.tnpWOSSetting = New DevExpress.XtraBars.Navigation.TabNavigationPage()
		Me.lblNoEMailEmployees = New DevExpress.XtraEditors.LabelControl()
		Me.tgsNoEMailEmployee = New DevExpress.XtraEditors.ToggleSwitch()
		Me.btnWOSProperty = New DevExpress.XtraEditors.DropDownButton()
		Me.lblWOSBez = New System.Windows.Forms.Label()
		Me.tnpMailSetting = New DevExpress.XtraBars.Navigation.TabNavigationPage()
		Me.lblIndividualFiles = New DevExpress.XtraEditors.LabelControl()
		Me.tgsIndividalFiles = New DevExpress.XtraEditors.ToggleSwitch()
		Me.lueMandant = New DevExpress.XtraEditors.LookUpEdit()
		Me.lblMDName = New System.Windows.Forms.Label()
		Me.lblSortierennach = New System.Windows.Forms.Label()
		Me.cbo_Month = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.cbo_Year = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.CboSort = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
		Me.Bar4 = New DevExpress.XtraBars.Bar()
		Me.bsiPrintinfo = New DevExpress.XtraBars.BarStaticItem()
		Me.bsiDetailInfo = New DevExpress.XtraBars.BarStaticItem()
		Me.bbiSearch = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiPrint = New DevExpress.XtraBars.BarLargeButtonItem()
		Me.bbiDelete = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiSendMail = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiExport = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiSetting = New DevExpress.XtraBars.BarButtonItem()
		Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
		Me.BarStaticItem1 = New DevExpress.XtraBars.BarStaticItem()
		Me.Bar3 = New DevExpress.XtraBars.Bar()
		Me.RibbonPage2 = New DevExpress.XtraBars.Ribbon.RibbonPage()
		Me.RibbonPageGroup1 = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
		Me.xtabLo = New DevExpress.XtraTab.XtraTabControl()
		Me.xtabLOPrint = New DevExpress.XtraTab.XtraTabPage()
		Me.tpMainView = New DevExpress.XtraBars.Navigation.TabPane()
		Me.tnpPrint = New DevExpress.XtraBars.Navigation.TabNavigationPage()
		Me.grdPrint = New DevExpress.XtraGrid.GridControl()
		Me.gvPrint = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.tnpMail = New DevExpress.XtraBars.Navigation.TabNavigationPage()
		Me.grdPayrollEMail = New DevExpress.XtraGrid.GridControl()
		Me.gvPayrollEMail = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.GridView5 = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.tnpEMailSummery = New DevExpress.XtraBars.Navigation.TabNavigationPage()
		Me.grdEMailSummery = New DevExpress.XtraGrid.GridControl()
		Me.gvEMailSummery = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.GridView6 = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.tgsSelection = New DevExpress.XtraEditors.ToggleSwitch()
		Me.xtabLODetail = New DevExpress.XtraTab.XtraTabPage()
		Me.grdPayrollDetail = New DevExpress.XtraGrid.GridControl()
		Me.gvPayrollDetail = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.GridView3 = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.flyoutPanel = New DevExpress.Utils.FlyoutPanel()
		Me.rtfContent = New DevExpress.XtraRichEdit.RichEditControl()
		Me.flyoutPanelHintLabel = New DevExpress.XtraEditors.LabelControl()
		Me.svgImageCollection1 = New DevExpress.Utils.SvgImageCollection(Me.components)
		CType(Me.cbo_MANr.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.cbo_LONr.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupBox1.SuspendLayout()
		CType(Me.gpSuchKriterien, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.gpSuchKriterien.SuspendLayout()
		CType(Me.pnlSearchType, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.pnlSearchType.SuspendLayout()
		CType(Me.tpMainSetting, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.tpMainSetting.SuspendLayout()
		Me.tnpWOSSetting.SuspendLayout()
		CType(Me.tgsNoEMailEmployee.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.tnpMailSetting.SuspendLayout()
		CType(Me.tgsIndividalFiles.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.cbo_Month.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.cbo_Year.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.CboSort.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.xtabLo, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.xtabLo.SuspendLayout()
		Me.xtabLOPrint.SuspendLayout()
		CType(Me.tpMainView, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.tpMainView.SuspendLayout()
		Me.tnpPrint.SuspendLayout()
		CType(Me.grdPrint, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvPrint, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.tnpMail.SuspendLayout()
		CType(Me.grdPayrollEMail, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvPayrollEMail, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.GridView5, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.tnpEMailSummery.SuspendLayout()
		CType(Me.grdEMailSummery, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvEMailSummery, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.GridView6, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.tgsSelection.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.xtabLODetail.SuspendLayout()
		CType(Me.grdPayrollDetail, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvPayrollDetail, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.GridView3, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.flyoutPanel, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.flyoutPanel.SuspendLayout()
		CType(Me.svgImageCollection1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'LblState_1
		'
		Me.LblState_1.AutoSize = False
		Me.LblState_1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.LblState_1.Name = "LblState_1"
		Me.LblState_1.Size = New System.Drawing.Size(200, 17)
		Me.LblState_1.Text = "Bereit"
		Me.LblState_1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.LblState_1.ToolTipText = "Modul-Status"
		'
		'lblHeaderFett
		'
		Me.lblHeaderFett.AutoSize = True
		Me.lblHeaderFett.BackColor = System.Drawing.Color.Transparent
		Me.lblHeaderFett.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblHeaderFett.Location = New System.Drawing.Point(94, 21)
		Me.lblHeaderFett.Name = "lblHeaderFett"
		Me.lblHeaderFett.Size = New System.Drawing.Size(208, 13)
		Me.lblHeaderFett.TabIndex = 0
		Me.lblHeaderFett.Text = "Verwaltung von Lohnabrechnungen"
		'
		'Label14
		'
		Me.Label14.AutoSize = True
		Me.Label14.Location = New System.Drawing.Point(411, 307)
		Me.Label14.Name = "Label14"
		Me.Label14.Size = New System.Drawing.Size(49, 13)
		Me.Label14.TabIndex = 23
		Me.Label14.Text = "2. Status"
		'
		'Label15
		'
		Me.Label15.AutoSize = True
		Me.Label15.Location = New System.Drawing.Point(411, 256)
		Me.Label15.Name = "Label15"
		Me.Label15.Size = New System.Drawing.Size(44, 13)
		Me.Label15.TabIndex = 21
		Me.Label15.Text = "Kontakt"
		'
		'Label16
		'
		Me.Label16.AutoSize = True
		Me.Label16.Location = New System.Drawing.Point(409, 281)
		Me.Label16.Name = "Label16"
		Me.Label16.Size = New System.Drawing.Size(49, 13)
		Me.Label16.TabIndex = 22
		Me.Label16.Text = "1. Status"
		'
		'Label17
		'
		Me.Label17.AutoSize = True
		Me.Label17.Location = New System.Drawing.Point(36, 246)
		Me.Label17.Name = "Label17"
		Me.Label17.Size = New System.Drawing.Size(75, 13)
		Me.Label17.TabIndex = 20
		Me.Label17.Text = "1. Eigenschaft"
		'
		'Label18
		'
		Me.Label18.AutoSize = True
		Me.Label18.Location = New System.Drawing.Point(36, 220)
		Me.Label18.Name = "Label18"
		Me.Label18.Size = New System.Drawing.Size(50, 13)
		Me.Label18.TabIndex = 19
		Me.Label18.Text = "BeraterIn"
		'
		'Label19
		'
		Me.Label19.AutoSize = True
		Me.Label19.Location = New System.Drawing.Point(34, 194)
		Me.Label19.Name = "Label19"
		Me.Label19.Size = New System.Drawing.Size(31, 13)
		Me.Label19.TabIndex = 18
		Me.Label19.Text = "Land"
		'
		'Label20
		'
		Me.Label20.AutoSize = True
		Me.Label20.Location = New System.Drawing.Point(36, 169)
		Me.Label20.Name = "Label20"
		Me.Label20.Size = New System.Drawing.Size(21, 13)
		Me.Label20.TabIndex = 17
		Me.Label20.Text = "Ort"
		'
		'Label21
		'
		Me.Label21.AutoSize = True
		Me.Label21.Location = New System.Drawing.Point(34, 143)
		Me.Label21.Name = "Label21"
		Me.Label21.Size = New System.Drawing.Size(27, 13)
		Me.Label21.TabIndex = 16
		Me.Label21.Text = "PLZ"
		'
		'Label22
		'
		Me.Label22.AutoSize = True
		Me.Label22.Location = New System.Drawing.Point(36, 117)
		Me.Label22.Name = "Label22"
		Me.Label22.Size = New System.Drawing.Size(32, 13)
		Me.Label22.TabIndex = 15
		Me.Label22.Text = "Firma"
		'
		'Label23
		'
		Me.Label23.AutoSize = True
		Me.Label23.Location = New System.Drawing.Point(34, 91)
		Me.Label23.Name = "Label23"
		Me.Label23.Size = New System.Drawing.Size(46, 13)
		Me.Label23.TabIndex = 14
		Me.Label23.Text = "Nummer"
		'
		'Label24
		'
		Me.Label24.AutoSize = True
		Me.Label24.Location = New System.Drawing.Point(34, 44)
		Me.Label24.Name = "Label24"
		Me.Label24.Size = New System.Drawing.Size(76, 13)
		Me.Label24.TabIndex = 13
		Me.Label24.Text = "Sortieren nach"
		'
		'TextBox13
		'
		Me.TextBox13.Location = New System.Drawing.Point(88, 269)
		Me.TextBox13.Name = "TextBox13"
		Me.TextBox13.Size = New System.Drawing.Size(172, 20)
		Me.TextBox13.TabIndex = 12
		'
		'TextBox14
		'
		Me.TextBox14.Location = New System.Drawing.Point(88, 243)
		Me.TextBox14.Name = "TextBox14"
		Me.TextBox14.Size = New System.Drawing.Size(172, 20)
		Me.TextBox14.TabIndex = 11
		'
		'TextBox15
		'
		Me.TextBox15.Location = New System.Drawing.Point(88, 217)
		Me.TextBox15.Name = "TextBox15"
		Me.TextBox15.Size = New System.Drawing.Size(172, 20)
		Me.TextBox15.TabIndex = 10
		'
		'TextBox16
		'
		Me.TextBox16.Location = New System.Drawing.Point(88, 191)
		Me.TextBox16.Name = "TextBox16"
		Me.TextBox16.Size = New System.Drawing.Size(172, 20)
		Me.TextBox16.TabIndex = 9
		'
		'TextBox17
		'
		Me.TextBox17.Location = New System.Drawing.Point(343, 166)
		Me.TextBox17.Name = "TextBox17"
		Me.TextBox17.Size = New System.Drawing.Size(172, 20)
		Me.TextBox17.TabIndex = 8
		'
		'TextBox18
		'
		Me.TextBox18.Location = New System.Drawing.Point(88, 166)
		Me.TextBox18.Name = "TextBox18"
		Me.TextBox18.Size = New System.Drawing.Size(172, 20)
		Me.TextBox18.TabIndex = 7
		'
		'TextBox19
		'
		Me.TextBox19.Location = New System.Drawing.Point(343, 140)
		Me.TextBox19.Name = "TextBox19"
		Me.TextBox19.Size = New System.Drawing.Size(172, 20)
		Me.TextBox19.TabIndex = 6
		'
		'TextBox20
		'
		Me.TextBox20.Location = New System.Drawing.Point(88, 140)
		Me.TextBox20.Name = "TextBox20"
		Me.TextBox20.Size = New System.Drawing.Size(172, 20)
		Me.TextBox20.TabIndex = 5
		'
		'TextBox21
		'
		Me.TextBox21.Location = New System.Drawing.Point(343, 114)
		Me.TextBox21.Name = "TextBox21"
		Me.TextBox21.Size = New System.Drawing.Size(172, 20)
		Me.TextBox21.TabIndex = 4
		'
		'TextBox22
		'
		Me.TextBox22.Location = New System.Drawing.Point(88, 114)
		Me.TextBox22.Name = "TextBox22"
		Me.TextBox22.Size = New System.Drawing.Size(172, 20)
		Me.TextBox22.TabIndex = 3
		'
		'TextBox23
		'
		Me.TextBox23.Location = New System.Drawing.Point(343, 88)
		Me.TextBox23.Name = "TextBox23"
		Me.TextBox23.Size = New System.Drawing.Size(172, 20)
		Me.TextBox23.TabIndex = 2
		'
		'ComboBox2
		'
		Me.ComboBox2.FormattingEnabled = True
		Me.ComboBox2.Items.AddRange(New Object() {"0 - Nach Kundennummer", "1 - Kundenname", "2 - Kundenort"})
		Me.ComboBox2.Location = New System.Drawing.Point(110, 41)
		Me.ComboBox2.Name = "ComboBox2"
		Me.ComboBox2.Size = New System.Drawing.Size(405, 21)
		Me.ComboBox2.TabIndex = 1
		'
		'TextBox24
		'
		Me.TextBox24.Location = New System.Drawing.Point(88, 88)
		Me.TextBox24.Name = "TextBox24"
		Me.TextBox24.Size = New System.Drawing.Size(172, 20)
		Me.TextBox24.TabIndex = 0
		'
		'lbljahr
		'
		Me.lbljahr.BackColor = System.Drawing.Color.Transparent
		Me.lbljahr.Location = New System.Drawing.Point(70, 127)
		Me.lbljahr.Name = "lbljahr"
		Me.lbljahr.Size = New System.Drawing.Size(102, 13)
		Me.lbljahr.TabIndex = 19
		Me.lbljahr.Text = "Jahr"
		Me.lbljahr.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblLohnNr
		'
		Me.lblLohnNr.BackColor = System.Drawing.Color.Transparent
		Me.lblLohnNr.Location = New System.Drawing.Point(70, 219)
		Me.lblLohnNr.Name = "lblLohnNr"
		Me.lblLohnNr.Size = New System.Drawing.Size(102, 13)
		Me.lblLohnNr.TabIndex = 63
		Me.lblLohnNr.Text = "Lohn-Nr."
		Me.lblLohnNr.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblMonat
		'
		Me.lblMonat.BackColor = System.Drawing.Color.Transparent
		Me.lblMonat.Location = New System.Drawing.Point(70, 152)
		Me.lblMonat.Name = "lblMonat"
		Me.lblMonat.Size = New System.Drawing.Size(102, 13)
		Me.lblMonat.TabIndex = 61
		Me.lblMonat.Text = "Monat"
		Me.lblMonat.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblKandidatenNr
		'
		Me.lblKandidatenNr.BackColor = System.Drawing.Color.Transparent
		Me.lblKandidatenNr.Location = New System.Drawing.Point(72, 193)
		Me.lblKandidatenNr.Name = "lblKandidatenNr"
		Me.lblKandidatenNr.Size = New System.Drawing.Size(102, 13)
		Me.lblKandidatenNr.TabIndex = 114
		Me.lblKandidatenNr.TabStop = True
		Me.lblKandidatenNr.Text = "Kandidaten-Nr."
		Me.lblKandidatenNr.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'StyleManager1
		'
		Me.StyleManager1.ManagerStyle = DevComponents.DotNetBar.eStyle.Metro
		Me.StyleManager1.MetroColorParameters = New DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(System.Drawing.Color.White, System.Drawing.Color.FromArgb(CType(CType(43, Byte), Integer), CType(CType(87, Byte), Integer), CType(CType(154, Byte), Integer)))
		'
		'cbo_MANr
		'
		Me.cbo_MANr.Location = New System.Drawing.Point(178, 190)
		Me.cbo_MANr.Name = "cbo_MANr"
		Me.cbo_MANr.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.cbo_MANr.Size = New System.Drawing.Size(280, 20)
		Me.cbo_MANr.TabIndex = 2
		'
		'cbo_LONr
		'
		Me.cbo_LONr.Location = New System.Drawing.Point(178, 216)
		Me.cbo_LONr.Name = "cbo_LONr"
		Me.cbo_LONr.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.cbo_LONr.Properties.DropDownRows = 20
		Me.cbo_LONr.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
		Me.cbo_LONr.Size = New System.Drawing.Size(280, 20)
		Me.cbo_LONr.TabIndex = 3
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
		Me.GroupBox1.Size = New System.Drawing.Size(1042, 77)
		Me.GroupBox1.TabIndex = 206
		'
		'sbClose
		'
		Me.sbClose.Anchor = System.Windows.Forms.AnchorStyles.Right
		Me.sbClose.Location = New System.Drawing.Point(914, 21)
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
		'gpSuchKriterien
		'
		Me.gpSuchKriterien.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.gpSuchKriterien.CaptionImageOptions.SvgImage = Global.SP.LO.PrintUtility.My.Resources.Resources.find
		Me.gpSuchKriterien.Controls.Add(Me.pnlSearchType)
		Me.gpSuchKriterien.Controls.Add(Me.tpMainSetting)
		Me.gpSuchKriterien.Controls.Add(Me.lueMandant)
		Me.gpSuchKriterien.Controls.Add(Me.lblMDName)
		Me.gpSuchKriterien.Controls.Add(Me.lblSortierennach)
		Me.gpSuchKriterien.Controls.Add(Me.cbo_Month)
		Me.gpSuchKriterien.Controls.Add(Me.cbo_Year)
		Me.gpSuchKriterien.Controls.Add(Me.lbljahr)
		Me.gpSuchKriterien.Controls.Add(Me.lblMonat)
		Me.gpSuchKriterien.Controls.Add(Me.cbo_LONr)
		Me.gpSuchKriterien.Controls.Add(Me.lblKandidatenNr)
		Me.gpSuchKriterien.Controls.Add(Me.cbo_MANr)
		Me.gpSuchKriterien.Controls.Add(Me.lblLohnNr)
		Me.gpSuchKriterien.Controls.Add(Me.CboSort)
		Me.gpSuchKriterien.Location = New System.Drawing.Point(22, 105)
		Me.gpSuchKriterien.Name = "gpSuchKriterien"
		Me.gpSuchKriterien.Size = New System.Drawing.Size(533, 537)
		Me.gpSuchKriterien.TabIndex = 272
		Me.gpSuchKriterien.Text = "Suchkriterien"
		'
		'pnlSearchType
		'
		Me.pnlSearchType.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.pnlSearchType.Appearance.Options.UseBackColor = True
		Me.pnlSearchType.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
		Me.pnlSearchType.Controls.Add(Me.cbEMail)
		Me.pnlSearchType.Controls.Add(Me.cbPrintWOS)
		Me.pnlSearchType.Location = New System.Drawing.Point(6, 280)
		Me.pnlSearchType.Name = "pnlSearchType"
		Me.pnlSearchType.Size = New System.Drawing.Size(521, 63)
		Me.pnlSearchType.TabIndex = 334
		'
		'cbEMail
		'
		Me.cbEMail.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.[True]
		Me.cbEMail.AppearancePressed.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.cbEMail.AppearancePressed.Options.UseFont = True
		Me.cbEMail.GroupIndex = 1
		Me.cbEMail.ImageOptions.SvgImage = Global.SP.LO.PrintUtility.My.Resources.Resources.mailmerge
		Me.cbEMail.Location = New System.Drawing.Point(263, 6)
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
		Me.cbPrintWOS.ImageOptions.SvgImage = Global.SP.LO.PrintUtility.My.Resources.Resources.print
		Me.cbPrintWOS.Location = New System.Drawing.Point(128, 6)
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
		Me.tpMainSetting.Location = New System.Drawing.Point(17, 356)
		Me.tpMainSetting.Name = "tpMainSetting"
		Me.tpMainSetting.Pages.AddRange(New DevExpress.XtraBars.Navigation.NavigationPageBase() {Me.tnpWOSSetting, Me.tnpMailSetting})
		Me.tpMainSetting.RegularSize = New System.Drawing.Size(499, 164)
		Me.tpMainSetting.SelectedPage = Me.tnpWOSSetting
		Me.tpMainSetting.Size = New System.Drawing.Size(499, 164)
		Me.tpMainSetting.TabIndex = 333
		Me.tpMainSetting.Text = "Allgemein"
		'
		'tnpWOSSetting
		'
		Me.tnpWOSSetting.Caption = "Drucken / WOS senden"
		Me.tnpWOSSetting.Controls.Add(Me.lblNoEMailEmployees)
		Me.tnpWOSSetting.Controls.Add(Me.tgsNoEMailEmployee)
		Me.tnpWOSSetting.Controls.Add(Me.btnWOSProperty)
		Me.tnpWOSSetting.Controls.Add(Me.lblWOSBez)
		Me.tnpWOSSetting.ItemShowMode = DevExpress.XtraBars.Navigation.ItemShowMode.ImageAndText
		Me.tnpWOSSetting.Name = "tnpWOSSetting"
		Me.tnpWOSSetting.Properties.ShowMode = DevExpress.XtraBars.Navigation.ItemShowMode.ImageAndText
		Me.tnpWOSSetting.Size = New System.Drawing.Size(499, 135)
		'
		'lblNoEMailEmployees
		'
		Me.lblNoEMailEmployees.AllowHtmlString = True
		Me.lblNoEMailEmployees.Location = New System.Drawing.Point(14, 75)
		Me.lblNoEMailEmployees.Name = "lblNoEMailEmployees"
		Me.lblNoEMailEmployees.Size = New System.Drawing.Size(141, 13)
		Me.lblNoEMailEmployees.TabIndex = 333
		Me.lblNoEMailEmployees.Text = "Kandidaten mit Email-Adresse"
		'
		'tgsNoEMailEmployee
		'
		Me.tgsNoEMailEmployee.Location = New System.Drawing.Point(161, 72)
		Me.tgsNoEMailEmployee.Name = "tgsNoEMailEmployee"
		Me.tgsNoEMailEmployee.Properties.AllowFocused = False
		Me.tgsNoEMailEmployee.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.[True]
		Me.tgsNoEMailEmployee.Properties.Appearance.Options.UseTextOptions = True
		Me.tgsNoEMailEmployee.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
		Me.tgsNoEMailEmployee.Properties.OffText = "werden eingeschlossen"
		Me.tgsNoEMailEmployee.Properties.OnText = "werden ausgeschlossen"
		Me.tgsNoEMailEmployee.Size = New System.Drawing.Size(241, 18)
		Me.tgsNoEMailEmployee.TabIndex = 298
		Me.tgsNoEMailEmployee.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information
		'
		'btnWOSProperty
		'
		Me.btnWOSProperty.Location = New System.Drawing.Point(161, 33)
		Me.btnWOSProperty.Name = "btnWOSProperty"
		Me.btnWOSProperty.Size = New System.Drawing.Size(280, 25)
		Me.btnWOSProperty.TabIndex = 4
		Me.btnWOSProperty.Text = "Drucken"
		'
		'lblWOSBez
		'
		Me.lblWOSBez.BackColor = System.Drawing.Color.Transparent
		Me.lblWOSBez.Location = New System.Drawing.Point(14, 39)
		Me.lblWOSBez.Name = "lblWOSBez"
		Me.lblWOSBez.Size = New System.Drawing.Size(143, 13)
		Me.lblWOSBez.TabIndex = 276
		Me.lblWOSBez.Text = "WOS-Kandidaten"
		Me.lblWOSBez.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'tnpMailSetting
		'
		Me.tnpMailSetting.Caption = "Mail-Versand"
		Me.tnpMailSetting.Controls.Add(Me.lblIndividualFiles)
		Me.tnpMailSetting.Controls.Add(Me.tgsIndividalFiles)
		Me.tnpMailSetting.ItemShowMode = DevExpress.XtraBars.Navigation.ItemShowMode.ImageAndText
		Me.tnpMailSetting.Name = "tnpMailSetting"
		Me.tnpMailSetting.PageText = "Mail-Versand"
		Me.tnpMailSetting.Properties.ShowMode = DevExpress.XtraBars.Navigation.ItemShowMode.ImageAndText
		Me.tnpMailSetting.Size = New System.Drawing.Size(499, 135)
		'
		'lblIndividualFiles
		'
		Me.lblIndividualFiles.AllowHtmlString = True
		Me.lblIndividualFiles.Appearance.Options.UseTextOptions = True
		Me.lblIndividualFiles.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblIndividualFiles.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblIndividualFiles.Location = New System.Drawing.Point(14, 39)
		Me.lblIndividualFiles.Name = "lblIndividualFiles"
		Me.lblIndividualFiles.Size = New System.Drawing.Size(140, 13)
		Me.lblIndividualFiles.TabIndex = 339
		Me.lblIndividualFiles.Text = "Anhänge werden als "
		'
		'tgsIndividalFiles
		'
		Me.tgsIndividalFiles.Location = New System.Drawing.Point(161, 36)
		Me.tgsIndividalFiles.Name = "tgsIndividalFiles"
		Me.tgsIndividalFiles.Properties.AllowFocused = False
		Me.tgsIndividalFiles.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.[True]
		Me.tgsIndividalFiles.Properties.OffText = "separate PDF gesendet"
		Me.tgsIndividalFiles.Properties.OnText = "eine ZIP Datei gesendet"
		Me.tgsIndividalFiles.Size = New System.Drawing.Size(194, 18)
		Me.tgsIndividalFiles.TabIndex = 338
		Me.tgsIndividalFiles.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information
		'
		'lueMandant
		'
		Me.lueMandant.Location = New System.Drawing.Point(178, 53)
		Me.lueMandant.Name = "lueMandant"
		Me.lueMandant.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.lueMandant.Properties.NullText = ""
		Me.lueMandant.Size = New System.Drawing.Size(280, 20)
		Me.lueMandant.TabIndex = 280
		'
		'lblMDName
		'
		Me.lblMDName.Location = New System.Drawing.Point(55, 58)
		Me.lblMDName.Name = "lblMDName"
		Me.lblMDName.Size = New System.Drawing.Size(117, 13)
		Me.lblMDName.TabIndex = 279
		Me.lblMDName.Text = "Mandanten"
		Me.lblMDName.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblSortierennach
		'
		Me.lblSortierennach.Location = New System.Drawing.Point(55, 82)
		Me.lblSortierennach.Name = "lblSortierennach"
		Me.lblSortierennach.Size = New System.Drawing.Size(117, 13)
		Me.lblSortierennach.TabIndex = 277
		Me.lblSortierennach.Text = "Sortieren nach"
		Me.lblSortierennach.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'cbo_Month
		'
		Me.cbo_Month.Location = New System.Drawing.Point(178, 149)
		Me.cbo_Month.Name = "cbo_Month"
		Me.cbo_Month.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.cbo_Month.Properties.DropDownRows = 12
		Me.cbo_Month.Size = New System.Drawing.Size(76, 20)
		Me.cbo_Month.TabIndex = 1
		'
		'cbo_Year
		'
		Me.cbo_Year.Location = New System.Drawing.Point(178, 123)
		Me.cbo_Year.Name = "cbo_Year"
		Me.cbo_Year.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.cbo_Year.Size = New System.Drawing.Size(76, 20)
		Me.cbo_Year.TabIndex = 0
		'
		'CboSort
		'
		Me.CboSort.Location = New System.Drawing.Point(178, 79)
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
		Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiPrintinfo, Me.bbiSearch, Me.bbiPrint, Me.bbiDelete, Me.BarStaticItem1, Me.bbiExport, Me.bbiSetting, Me.bsiDetailInfo, Me.bbiSendMail})
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
		Me.Bar4.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiPrintinfo), New DevExpress.XtraBars.LinkPersistInfo(Me.bsiDetailInfo), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiSearch), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiPrint, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiDelete, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiSendMail, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiExport), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiSetting, True)})
		Me.Bar4.OptionsBar.AllowQuickCustomization = False
		Me.Bar4.OptionsBar.DrawDragBorder = False
		Me.Bar4.OptionsBar.UseWholeRow = True
		Me.Bar4.Text = "Statusleiste"
		'
		'bsiPrintinfo
		'
		Me.bsiPrintinfo.AutoSize = DevExpress.XtraBars.BarStaticItemSize.None
		Me.bsiPrintinfo.Caption = "Bereit"
		Me.bsiPrintinfo.Id = 0
		Me.bsiPrintinfo.Name = "bsiPrintinfo"
		Me.bsiPrintinfo.Size = New System.Drawing.Size(250, 0)
		Me.bsiPrintinfo.Width = 250
		'
		'bsiDetailInfo
		'
		Me.bsiDetailInfo.AutoSize = DevExpress.XtraBars.BarStaticItemSize.Spring
		Me.bsiDetailInfo.Caption = "Bereit"
		Me.bsiDetailInfo.Id = 10
		Me.bsiDetailInfo.Name = "bsiDetailInfo"
		Me.bsiDetailInfo.Size = New System.Drawing.Size(250, 0)
		Me.bsiDetailInfo.Width = 250
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
		Me.bbiSendMail.ImageOptions.Image = Global.SP.LO.PrintUtility.My.Resources.Resources.mailmerge_16x16
		Me.bbiSendMail.ImageOptions.LargeImage = Global.SP.LO.PrintUtility.My.Resources.Resources.mailmerge_32x32
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
		'bbiSetting
		'
		Me.bbiSetting.Caption = "Einstellungen"
		Me.bbiSetting.Id = 7
		Me.bbiSetting.ImageOptions.Image = CType(resources.GetObject("bbiSetting.ImageOptions.Image"), System.Drawing.Image)
		Me.bbiSetting.ImageOptions.LargeImage = CType(resources.GetObject("bbiSetting.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.bbiSetting.Name = "bbiSetting"
		Me.bbiSetting.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		Me.bbiSetting.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
		'
		'barDockControlTop
		'
		Me.barDockControlTop.CausesValidation = False
		Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
		Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlTop.Manager = Me.BarManager1
		Me.barDockControlTop.Size = New System.Drawing.Size(1042, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 676)
		Me.barDockControlBottom.Manager = Me.BarManager1
		Me.barDockControlBottom.Size = New System.Drawing.Size(1042, 26)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Manager = Me.BarManager1
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 676)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(1042, 0)
		Me.barDockControlRight.Manager = Me.BarManager1
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 676)
		'
		'BarStaticItem1
		'
		Me.BarStaticItem1.Caption = " "
		Me.BarStaticItem1.Id = 5
		Me.BarStaticItem1.Name = "BarStaticItem1"
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
		'RibbonPage2
		'
		Me.RibbonPage2.Name = "RibbonPage2"
		Me.RibbonPage2.Text = "Drucken"
		'
		'RibbonPageGroup1
		'
		Me.RibbonPageGroup1.Name = "RibbonPageGroup1"
		Me.RibbonPageGroup1.Text = "RibbonPageGroup1"
		'
		'xtabLo
		'
		Me.xtabLo.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		EditorButtonImageOptions1.Image = Global.SP.LO.PrintUtility.My.Resources.Resources.exporttoxls_16x16
		EditorButtonImageOptions2.Image = Global.SP.LO.PrintUtility.My.Resources.Resources.bugreport_16x16
		Me.xtabLo.CustomHeaderButtons.AddRange(New DevExpress.XtraTab.Buttons.CustomHeaderButton() {New DevExpress.XtraTab.Buttons.CustomHeaderButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "Exportieren", -1, True, True, EditorButtonImageOptions1, SerializableAppearanceObject1, "", CType(1, Short), Nothing), New DevExpress.XtraTab.Buttons.CustomHeaderButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, True, True, EditorButtonImageOptions2, SerializableAppearanceObject2, "", "2", Nothing)})
		Me.xtabLo.Location = New System.Drawing.Point(572, 105)
		Me.xtabLo.Name = "xtabLo"
		Me.xtabLo.SelectedTabPage = Me.xtabLOPrint
		Me.xtabLo.Size = New System.Drawing.Size(442, 537)
		Me.xtabLo.TabIndex = 277
		Me.xtabLo.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabLOPrint, Me.xtabLODetail})
		'
		'xtabLOPrint
		'
		Me.xtabLOPrint.Controls.Add(Me.tpMainView)
		Me.xtabLOPrint.Controls.Add(Me.tgsSelection)
		Me.xtabLOPrint.Name = "xtabLOPrint"
		Me.xtabLOPrint.Padding = New System.Windows.Forms.Padding(10)
		Me.xtabLOPrint.Size = New System.Drawing.Size(440, 511)
		Me.xtabLOPrint.Text = "Lohnabrechnungen drucken / löschen"
		'
		'tpMainView
		'
		Me.tpMainView.AllowHtmlDraw = True
		Me.tpMainView.Controls.Add(Me.tnpPrint)
		Me.tpMainView.Controls.Add(Me.tnpMail)
		Me.tpMainView.Controls.Add(Me.tnpEMailSummery)
		Me.tpMainView.Dock = System.Windows.Forms.DockStyle.Fill
		Me.tpMainView.Location = New System.Drawing.Point(10, 28)
		Me.tpMainView.Name = "tpMainView"
		Me.tpMainView.Pages.AddRange(New DevExpress.XtraBars.Navigation.NavigationPageBase() {Me.tnpPrint, Me.tnpMail, Me.tnpEMailSummery})
		Me.tpMainView.RegularSize = New System.Drawing.Size(420, 473)
		Me.tpMainView.SelectedPage = Me.tnpMail
		Me.tpMainView.Size = New System.Drawing.Size(420, 473)
		Me.tpMainView.TabIndex = 333
		Me.tpMainView.Text = "TabPane1"
		'
		'tnpPrint
		'
		Me.tnpPrint.Caption = "Detail zum Drucken"
		Me.tnpPrint.Controls.Add(Me.grdPrint)
		Me.tnpPrint.Name = "tnpPrint"
		Me.tnpPrint.Padding = New System.Windows.Forms.Padding(5)
		Me.tnpPrint.Size = New System.Drawing.Size(529, 557)
		'
		'grdPrint
		'
		Me.grdPrint.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdPrint.Location = New System.Drawing.Point(5, 5)
		Me.grdPrint.MainView = Me.gvPrint
		Me.grdPrint.Name = "grdPrint"
		Me.grdPrint.Size = New System.Drawing.Size(519, 547)
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
		'tnpMail
		'
		Me.tnpMail.Caption = "Detail zum EMail-Versand"
		Me.tnpMail.Controls.Add(Me.grdPayrollEMail)
		Me.tnpMail.Name = "tnpMail"
		Me.tnpMail.Padding = New System.Windows.Forms.Padding(5)
		Me.tnpMail.Size = New System.Drawing.Size(420, 444)
		'
		'grdPayrollEMail
		'
		Me.grdPayrollEMail.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdPayrollEMail.Location = New System.Drawing.Point(5, 5)
		Me.grdPayrollEMail.MainView = Me.gvPayrollEMail
		Me.grdPayrollEMail.Name = "grdPayrollEMail"
		Me.grdPayrollEMail.Size = New System.Drawing.Size(410, 434)
		Me.grdPayrollEMail.TabIndex = 157
		Me.grdPayrollEMail.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvPayrollEMail, Me.GridView5})
		'
		'gvPayrollEMail
		'
		Me.gvPayrollEMail.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvPayrollEMail.GridControl = Me.grdPayrollEMail
		Me.gvPayrollEMail.Name = "gvPayrollEMail"
		Me.gvPayrollEMail.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvPayrollEMail.OptionsView.ShowAutoFilterRow = True
		Me.gvPayrollEMail.OptionsView.ShowGroupPanel = False
		'
		'GridView5
		'
		Me.GridView5.GridControl = Me.grdPayrollEMail
		Me.GridView5.Name = "GridView5"
		'
		'tnpEMailSummery
		'
		Me.tnpEMailSummery.Caption = "Zusammenstellung der EMail-Versand"
		Me.tnpEMailSummery.Controls.Add(Me.grdEMailSummery)
		Me.tnpEMailSummery.Name = "tnpEMailSummery"
		Me.tnpEMailSummery.Padding = New System.Windows.Forms.Padding(5)
		Me.tnpEMailSummery.Size = New System.Drawing.Size(529, 557)
		'
		'grdEMailSummery
		'
		Me.grdEMailSummery.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdEMailSummery.Location = New System.Drawing.Point(5, 5)
		Me.grdEMailSummery.MainView = Me.gvEMailSummery
		Me.grdEMailSummery.Name = "grdEMailSummery"
		Me.grdEMailSummery.Size = New System.Drawing.Size(519, 547)
		Me.grdEMailSummery.TabIndex = 158
		Me.grdEMailSummery.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvEMailSummery, Me.GridView6})
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
		'GridView6
		'
		Me.GridView6.GridControl = Me.grdEMailSummery
		Me.GridView6.Name = "GridView6"
		'
		'tgsSelection
		'
		Me.tgsSelection.Dock = System.Windows.Forms.DockStyle.Top
		Me.tgsSelection.EditValue = True
		Me.tgsSelection.Location = New System.Drawing.Point(10, 10)
		Me.tgsSelection.MenuManager = Me.BarManager1
		Me.tgsSelection.Name = "tgsSelection"
		Me.tgsSelection.Properties.AllowFocused = False
		Me.tgsSelection.Properties.Appearance.Options.UseTextOptions = True
		Me.tgsSelection.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.tgsSelection.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.tgsSelection.Properties.OffText = "Alles abgewählt"
		Me.tgsSelection.Properties.OnText = "Alles ausgewählt"
		Me.tgsSelection.Size = New System.Drawing.Size(420, 18)
		Me.tgsSelection.TabIndex = 320
		'
		'xtabLODetail
		'
		Me.xtabLODetail.Controls.Add(Me.grdPayrollDetail)
		Me.xtabLODetail.Name = "xtabLODetail"
		Me.xtabLODetail.Padding = New System.Windows.Forms.Padding(10)
		Me.xtabLODetail.Size = New System.Drawing.Size(549, 624)
		Me.xtabLODetail.Text = "Lohnabrechnungsdetail"
		'
		'grdPayrollDetail
		'
		Me.grdPayrollDetail.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdPayrollDetail.Location = New System.Drawing.Point(10, 10)
		Me.grdPayrollDetail.MainView = Me.gvPayrollDetail
		Me.grdPayrollDetail.Name = "grdPayrollDetail"
		Me.grdPayrollDetail.Size = New System.Drawing.Size(529, 604)
		Me.grdPayrollDetail.TabIndex = 157
		Me.grdPayrollDetail.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvPayrollDetail, Me.GridView3})
		'
		'gvPayrollDetail
		'
		Me.gvPayrollDetail.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvPayrollDetail.GridControl = Me.grdPayrollDetail
		Me.gvPayrollDetail.Name = "gvPayrollDetail"
		Me.gvPayrollDetail.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvPayrollDetail.OptionsView.ShowAutoFilterRow = True
		Me.gvPayrollDetail.OptionsView.ShowGroupPanel = False
		'
		'GridView3
		'
		Me.GridView3.GridControl = Me.grdPayrollDetail
		Me.GridView3.Name = "GridView3"
		'
		'flyoutPanel
		'
		Me.flyoutPanel.Controls.Add(Me.rtfContent)
		Me.flyoutPanel.Controls.Add(Me.flyoutPanelHintLabel)
		Me.flyoutPanel.Location = New System.Drawing.Point(1163, 389)
		Me.flyoutPanel.Name = "flyoutPanel"
		Me.flyoutPanel.OptionsButtonPanel.AllowGlyphSkinning = True
		Me.flyoutPanel.OptionsButtonPanel.ButtonPanelHeight = 36
		ButtonImageOptions1.SvgImage = CType(resources.GetObject("ButtonImageOptions1.SvgImage"), DevExpress.Utils.Svg.SvgImage)
		ButtonImageOptions1.SvgImageSize = New System.Drawing.Size(16, 16)
		ToolTipItem1.Text = "Exit"
		SuperToolTip1.Items.Add(ToolTipItem1)
		Me.flyoutPanel.OptionsButtonPanel.Buttons.AddRange(New DevExpress.XtraEditors.ButtonPanel.IBaseButton() {New DevExpress.Utils.PeekFormButton("Button", False, ButtonImageOptions1, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Exit", -1, True, SuperToolTip1, True, False, True, "Exit", -1, False)})
		Me.flyoutPanel.OptionsButtonPanel.ShowButtonPanel = True
		Me.flyoutPanel.Padding = New System.Windows.Forms.Padding(0, 36, 0, 0)
		Me.flyoutPanel.Size = New System.Drawing.Size(466, 674)
		Me.flyoutPanel.TabIndex = 282
		'
		'rtfContent
		'
		Me.rtfContent.Dock = System.Windows.Forms.DockStyle.Fill
		Me.rtfContent.Location = New System.Drawing.Point(0, 36)
		Me.rtfContent.MenuManager = Me.BarManager1
		Me.rtfContent.Name = "rtfContent"
		Me.rtfContent.Size = New System.Drawing.Size(466, 638)
		Me.rtfContent.TabIndex = 1
		'
		'flyoutPanelHintLabel
		'
		Me.flyoutPanelHintLabel.Anchor = System.Windows.Forms.AnchorStyles.None
		Me.flyoutPanelHintLabel.Location = New System.Drawing.Point(154, 335)
		Me.flyoutPanelHintLabel.Name = "flyoutPanelHintLabel"
		Me.flyoutPanelHintLabel.Size = New System.Drawing.Size(152, 13)
		Me.flyoutPanelHintLabel.TabIndex = 0
		Me.flyoutPanelHintLabel.Text = "You can place any content here"
		'
		'svgImageCollection1
		'
		Me.svgImageCollection1.Add("properties", "image://svgimages/setup/properties.svg")
		'
		'frmLOPrint
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(1042, 702)
		Me.Controls.Add(Me.flyoutPanel)
		Me.Controls.Add(Me.xtabLo)
		Me.Controls.Add(Me.gpSuchKriterien)
		Me.Controls.Add(Me.GroupBox1)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.IconOptions.Icon = CType(resources.GetObject("frmLOPrint.IconOptions.Icon"), System.Drawing.Icon)
		Me.MinimumSize = New System.Drawing.Size(1044, 734)
		Me.Name = "frmLOPrint"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Verwaltung der Lohnabrechnungen"
		CType(Me.cbo_MANr.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.cbo_LONr.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupBox1.ResumeLayout(False)
		Me.GroupBox1.PerformLayout()
		CType(Me.gpSuchKriterien, System.ComponentModel.ISupportInitialize).EndInit()
		Me.gpSuchKriterien.ResumeLayout(False)
		CType(Me.pnlSearchType, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pnlSearchType.ResumeLayout(False)
		CType(Me.tpMainSetting, System.ComponentModel.ISupportInitialize).EndInit()
		Me.tpMainSetting.ResumeLayout(False)
		Me.tnpWOSSetting.ResumeLayout(False)
		Me.tnpWOSSetting.PerformLayout()
		CType(Me.tgsNoEMailEmployee.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.tnpMailSetting.ResumeLayout(False)
		CType(Me.tgsIndividalFiles.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.cbo_Month.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.cbo_Year.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.CboSort.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.xtabLo, System.ComponentModel.ISupportInitialize).EndInit()
		Me.xtabLo.ResumeLayout(False)
		Me.xtabLOPrint.ResumeLayout(False)
		CType(Me.tpMainView, System.ComponentModel.ISupportInitialize).EndInit()
		Me.tpMainView.ResumeLayout(False)
		Me.tnpPrint.ResumeLayout(False)
		CType(Me.grdPrint, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvPrint, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.tnpMail.ResumeLayout(False)
		CType(Me.grdPayrollEMail, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvPayrollEMail, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.GridView5, System.ComponentModel.ISupportInitialize).EndInit()
		Me.tnpEMailSummery.ResumeLayout(False)
		CType(Me.grdEMailSummery, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvEMailSummery, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.GridView6, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.tgsSelection.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.xtabLODetail.ResumeLayout(False)
		CType(Me.grdPayrollDetail, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvPayrollDetail, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.GridView3, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.flyoutPanel, System.ComponentModel.ISupportInitialize).EndInit()
		Me.flyoutPanel.ResumeLayout(False)
		Me.flyoutPanel.PerformLayout()
		CType(Me.svgImageCollection1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents lblHeaderFett As System.Windows.Forms.Label
	Friend WithEvents Label14 As System.Windows.Forms.Label
	Friend WithEvents Label15 As System.Windows.Forms.Label
	Friend WithEvents Label16 As System.Windows.Forms.Label
	Friend WithEvents Label17 As System.Windows.Forms.Label
	Friend WithEvents Label18 As System.Windows.Forms.Label
	Friend WithEvents Label19 As System.Windows.Forms.Label
	Friend WithEvents Label20 As System.Windows.Forms.Label
	Friend WithEvents Label21 As System.Windows.Forms.Label
	Friend WithEvents Label22 As System.Windows.Forms.Label
	Friend WithEvents Label23 As System.Windows.Forms.Label
	Friend WithEvents Label24 As System.Windows.Forms.Label
	Friend WithEvents TextBox13 As System.Windows.Forms.TextBox
	Friend WithEvents TextBox14 As System.Windows.Forms.TextBox
	Friend WithEvents TextBox15 As System.Windows.Forms.TextBox
	Friend WithEvents TextBox16 As System.Windows.Forms.TextBox
	Friend WithEvents TextBox17 As System.Windows.Forms.TextBox
	Friend WithEvents TextBox18 As System.Windows.Forms.TextBox
	Friend WithEvents TextBox19 As System.Windows.Forms.TextBox
	Friend WithEvents TextBox20 As System.Windows.Forms.TextBox
	Friend WithEvents TextBox21 As System.Windows.Forms.TextBox
	Friend WithEvents TextBox22 As System.Windows.Forms.TextBox
	Friend WithEvents TextBox23 As System.Windows.Forms.TextBox
	Friend WithEvents ComboBox2 As System.Windows.Forms.ComboBox
	Friend WithEvents TextBox24 As System.Windows.Forms.TextBox
	Friend WithEvents lbljahr As System.Windows.Forms.Label
	Friend WithEvents LblState_1 As System.Windows.Forms.ToolStripStatusLabel
	Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
	Friend WithEvents lblLohnNr As System.Windows.Forms.Label
	Friend WithEvents lblMonat As System.Windows.Forms.Label
	Friend WithEvents ToolStripMenuItem6 As System.Windows.Forms.ToolStripMenuItem
	Friend WithEvents lblKandidatenNr As System.Windows.Forms.Label
	Friend WithEvents StyleManager1 As DevComponents.DotNetBar.StyleManager
	Friend WithEvents gpSuchKriterien As DevExpress.XtraEditors.GroupControl
	Friend WithEvents cbo_LONr As DevExpress.XtraEditors.CheckedComboBoxEdit
	Friend WithEvents cbo_MANr As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents GroupBox1 As DevExpress.XtraEditors.PanelControl
	Friend WithEvents sbClose As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents Label2 As System.Windows.Forms.Label
	Friend WithEvents lblHeaderNormal As System.Windows.Forms.Label
	Friend WithEvents cbo_Month As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents cbo_Year As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents Bar3 As DevExpress.XtraBars.Bar
	Friend WithEvents btnWOSProperty As DevExpress.XtraEditors.DropDownButton
	Friend WithEvents lblWOSBez As System.Windows.Forms.Label
	Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
	Friend WithEvents RibbonPage2 As DevExpress.XtraBars.Ribbon.RibbonPage
	Friend WithEvents RibbonPageGroup1 As DevExpress.XtraBars.Ribbon.RibbonPageGroup
	Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
	Friend WithEvents Bar4 As DevExpress.XtraBars.Bar
	Friend WithEvents bsiPrintinfo As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents bbiSearch As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbiExport As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents BarStaticItem1 As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents bbiPrint As DevExpress.XtraBars.BarLargeButtonItem
	Friend WithEvents bbiSetting As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbiDelete As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents xtabLo As DevExpress.XtraTab.XtraTabControl
	Friend WithEvents xtabLOPrint As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents xtabLODetail As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents lueMandant As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents lblMDName As System.Windows.Forms.Label
	Friend WithEvents lblSortierennach As System.Windows.Forms.Label
	Friend WithEvents CboSort As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents grdPrint As DevExpress.XtraGrid.GridControl
	Private WithEvents gvPrint As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents grdPayrollDetail As DevExpress.XtraGrid.GridControl
	Private WithEvents gvPayrollDetail As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents GridView3 As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents bsiDetailInfo As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents tgsSelection As DevExpress.XtraEditors.ToggleSwitch
	Private WithEvents flyoutPanel As DevExpress.Utils.FlyoutPanel
	Private WithEvents flyoutPanelHintLabel As DevExpress.XtraEditors.LabelControl
	Private WithEvents svgImageCollection1 As DevExpress.Utils.SvgImageCollection
	Friend WithEvents rtfContent As DevExpress.XtraRichEdit.RichEditControl
	Friend WithEvents pnlSearchType As DevExpress.XtraEditors.PanelControl
	Friend WithEvents cbEMail As DevExpress.XtraEditors.CheckButton
	Friend WithEvents cbPrintWOS As DevExpress.XtraEditors.CheckButton
	Friend WithEvents tpMainSetting As DevExpress.XtraBars.Navigation.TabPane
	Friend WithEvents tnpWOSSetting As DevExpress.XtraBars.Navigation.TabNavigationPage
	Friend WithEvents lblNoEMailEmployees As DevExpress.XtraEditors.LabelControl
	Friend WithEvents tgsNoEMailEmployee As DevExpress.XtraEditors.ToggleSwitch
	Friend WithEvents tnpMailSetting As DevExpress.XtraBars.Navigation.TabNavigationPage
	Friend WithEvents bbiSendMail As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents tpMainView As DevExpress.XtraBars.Navigation.TabPane
	Friend WithEvents tnpPrint As DevExpress.XtraBars.Navigation.TabNavigationPage
	Friend WithEvents tnpMail As DevExpress.XtraBars.Navigation.TabNavigationPage
	Friend WithEvents grdPayrollEMail As DevExpress.XtraGrid.GridControl
	Private WithEvents gvPayrollEMail As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents GridView5 As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents tnpEMailSummery As DevExpress.XtraBars.Navigation.TabNavigationPage
	Friend WithEvents grdEMailSummery As DevExpress.XtraGrid.GridControl
	Private WithEvents gvEMailSummery As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents GridView6 As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents lblIndividualFiles As DevExpress.XtraEditors.LabelControl
	Friend WithEvents tgsIndividalFiles As DevExpress.XtraEditors.ToggleSwitch
End Class
