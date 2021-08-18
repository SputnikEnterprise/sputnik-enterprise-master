<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOPSearch
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOPSearch))
		Me.CmdClose = New DevExpress.XtraEditors.SimpleButton()
		Me.lblheader2 = New DevExpress.XtraEditors.LabelControl()
		Me.lblheader1 = New DevExpress.XtraEditors.LabelControl()
		Me.Label14 = New DevExpress.XtraEditors.LabelControl()
		Me.Label15 = New DevExpress.XtraEditors.LabelControl()
		Me.Label16 = New DevExpress.XtraEditors.LabelControl()
		Me.Label17 = New DevExpress.XtraEditors.LabelControl()
		Me.Label18 = New DevExpress.XtraEditors.LabelControl()
		Me.Label19 = New DevExpress.XtraEditors.LabelControl()
		Me.Label20 = New DevExpress.XtraEditors.LabelControl()
		Me.Label21 = New DevExpress.XtraEditors.LabelControl()
		Me.Label22 = New DevExpress.XtraEditors.LabelControl()
		Me.Label23 = New DevExpress.XtraEditors.LabelControl()
		Me.Label24 = New DevExpress.XtraEditors.LabelControl()
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
		Me.lblderzeitigeabfrage = New DevExpress.XtraEditors.LabelControl()
		Me.txt_SQLQuery = New DevExpress.XtraEditors.MemoEdit()
		Me.lblAbfrage = New DevExpress.XtraEditors.LabelControl()
		Me.txt_IndSQLQuery = New DevExpress.XtraEditors.MemoEdit()
		Me.deMahnDate = New DevExpress.XtraEditors.DateEdit()
		Me.lblmahndatum = New DevExpress.XtraEditors.LabelControl()
		Me.cbo_Mahnstufe = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.lblmahnstufe = New DevExpress.XtraEditors.LabelControl()
		Me.deCreated_2 = New DevExpress.XtraEditors.DateEdit()
		Me.deCreated_1 = New DevExpress.XtraEditors.DateEdit()
		Me.lblerstellungzwischen = New DevExpress.XtraEditors.LabelControl()
		Me.Cbo_ErstellPeriode = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.lblErstellperiode = New DevExpress.XtraEditors.LabelControl()
		Me.lblkundennr = New DevExpress.XtraEditors.LabelControl()
		Me.lblnummer = New DevExpress.XtraEditors.LabelControl()
		Me.CboSort = New DevExpress.XtraEditors.CheckedComboBoxEdit()
		Me.txt_OpenBetrag_2 = New DevExpress.XtraEditors.TextEdit()
		Me.lblOffenerbetrag = New DevExpress.XtraEditors.LabelControl()
		Me.txt_OpenBetrag_1 = New DevExpress.XtraEditors.TextEdit()
		Me.Cbo_KST1 = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.Cbo_KST2 = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.lbl1kst = New DevExpress.XtraEditors.LabelControl()
		Me.lbllistenart = New DevExpress.XtraEditors.LabelControl()
		Me.Cbo_ListingArt = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.Cbo_Berater = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.lblberater = New DevExpress.XtraEditors.LabelControl()
		Me.lblfakturazwischen = New DevExpress.XtraEditors.LabelControl()
		Me.deFakDate_2 = New DevExpress.XtraEditors.DateEdit()
		Me.Cbo_FakPeriode = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.deFakDate_1 = New DevExpress.XtraEditors.DateEdit()
		Me.lblfakturaperiode = New DevExpress.XtraEditors.LabelControl()
		Me.Cbo_Filiale = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.lblfiliale = New DevExpress.XtraEditors.LabelControl()
		Me.lbl2kst = New DevExpress.XtraEditors.LabelControl()
		Me.lblart = New DevExpress.XtraEditors.LabelControl()
		Me.lblmahncode = New DevExpress.XtraEditors.LabelControl()
		Me.lblverfalltage = New DevExpress.XtraEditors.LabelControl()
		Me.lblsortierung = New DevExpress.XtraEditors.LabelControl()
		Me.chkKDKreditlimiteUeberschritten = New DevExpress.XtraEditors.CheckEdit()
		Me.lbleinstufung = New DevExpress.XtraEditors.LabelControl()
		Me.cbo_ESBranche = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.lblBranche = New DevExpress.XtraEditors.LabelControl()
		Me.Cbo_ESEinstufung = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.cbo_ESRBank = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.lblESBank = New DevExpress.XtraEditors.LabelControl()
		Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
		Me.xtabOPSearch = New DevExpress.XtraTab.XtraTabControl()
		Me.xtabAllgemein = New DevExpress.XtraTab.XtraTabPage()
		Me.grpInvoiceDate = New DevExpress.XtraEditors.GroupControl()
		Me.lblCreateDate = New DevExpress.XtraEditors.LabelControl()
		Me.lblInvoiceDate = New DevExpress.XtraEditors.LabelControl()
		Me.lblStichtag = New DevExpress.XtraEditors.LabelControl()
		Me.deStichtag = New DevExpress.XtraEditors.DateEdit()
		Me.grpKostenteilung = New DevExpress.XtraEditors.GroupControl()
		Me.chkValutaFromCreatedOn = New DevExpress.XtraEditors.CheckEdit()
		Me.SwitchButton2 = New DevComponents.DotNetBar.Controls.SwitchButton()
		Me.SwitchButton1 = New DevComponents.DotNetBar.Controls.SwitchButton()
		Me.lueMandant = New DevExpress.XtraEditors.LookUpEdit()
		Me.lblMDName = New DevExpress.XtraEditors.LabelControl()
		Me.txtKDNr_1 = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.txtOPNr_2 = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.txtKDNr_2 = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.txtOPNr_1 = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.txtVGNr_1 = New DevExpress.XtraEditors.SpinEdit()
		Me.txtVGNr_2 = New DevExpress.XtraEditors.SpinEdit()
		Me.xtabSonstiges = New DevExpress.XtraTab.XtraTabPage()
		Me.grpDebitorenArt = New DevExpress.XtraEditors.GroupControl()
		Me.lueDebitorenart = New DevExpress.XtraEditors.LookUpEdit()
		Me.grpMahnArt = New DevExpress.XtraEditors.GroupControl()
		Me.luePaymentReminderCode = New DevExpress.XtraEditors.LookUpEdit()
		Me.grpEinstufung = New DevExpress.XtraEditors.GroupControl()
		Me.grpESRAngaben = New DevExpress.XtraEditors.GroupControl()
		Me.grpPrintSetting = New DevExpress.XtraEditors.GroupControl()
		Me.chkHideKreditInfoLine = New DevExpress.XtraEditors.CheckEdit()
		Me.chkHideOPInfoLine = New DevExpress.XtraEditors.CheckEdit()
		Me.chkHideRefNrLine = New DevExpress.XtraEditors.CheckEdit()
		Me.xtabErweitert = New DevExpress.XtraTab.XtraTabPage()
		Me.xtabSQLAbfrage = New DevExpress.XtraTab.XtraTabPage()
		Me.xtabAdmin = New DevExpress.XtraTab.XtraTabPage()
		Me.lbladmin = New DevExpress.XtraEditors.LabelControl()
		Me.txtAdminQuery = New DevExpress.XtraEditors.MemoEdit()
		Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
		Me.PictureEdit1 = New DevExpress.XtraEditors.PictureEdit()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
		Me.Bar3 = New DevExpress.XtraBars.Bar()
		Me.bsiInfo = New DevExpress.XtraBars.BarStaticItem()
		Me.bbiSearch = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiClear = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiPrint = New DevExpress.XtraBars.BarLargeButtonItem()
		Me.bbiExport = New DevExpress.XtraBars.BarLargeButtonItem()
		Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
		CType(Me.txt_SQLQuery.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txt_IndSQLQuery.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.deMahnDate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.deMahnDate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.cbo_Mahnstufe.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.deCreated_2.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.deCreated_2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.deCreated_1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.deCreated_1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_ErstellPeriode.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.CboSort.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txt_OpenBetrag_2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txt_OpenBetrag_1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_KST1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_KST2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_ListingArt.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_Berater.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.deFakDate_2.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.deFakDate_2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_FakPeriode.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.deFakDate_1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.deFakDate_1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_Filiale.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkKDKreditlimiteUeberschritten.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.cbo_ESBranche.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_ESEinstufung.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.cbo_ESRBank.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.xtabOPSearch, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.xtabOPSearch.SuspendLayout()
		Me.xtabAllgemein.SuspendLayout()
		CType(Me.grpInvoiceDate, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grpInvoiceDate.SuspendLayout()
		CType(Me.deStichtag.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.deStichtag.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.grpKostenteilung, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grpKostenteilung.SuspendLayout()
		CType(Me.chkValutaFromCreatedOn.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtKDNr_1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtOPNr_2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtKDNr_2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtOPNr_1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtVGNr_1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtVGNr_2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.xtabSonstiges.SuspendLayout()
		CType(Me.grpDebitorenArt, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grpDebitorenArt.SuspendLayout()
		CType(Me.lueDebitorenart.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.grpMahnArt, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grpMahnArt.SuspendLayout()
		CType(Me.luePaymentReminderCode.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.grpEinstufung, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grpEinstufung.SuspendLayout()
		CType(Me.grpESRAngaben, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grpESRAngaben.SuspendLayout()
		CType(Me.grpPrintSetting, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grpPrintSetting.SuspendLayout()
		CType(Me.chkHideKreditInfoLine.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkHideOPInfoLine.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkHideRefNrLine.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.xtabErweitert.SuspendLayout()
		Me.xtabSQLAbfrage.SuspendLayout()
		Me.xtabAdmin.SuspendLayout()
		CType(Me.txtAdminQuery.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.PanelControl1.SuspendLayout()
		CType(Me.PictureEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'CmdClose
		'
		Me.CmdClose.Location = New System.Drawing.Point(580, 14)
		Me.CmdClose.Name = "CmdClose"
		Me.CmdClose.Size = New System.Drawing.Size(86, 24)
		Me.CmdClose.TabIndex = 999
		Me.CmdClose.Text = "Schliessen"
		'
		'lblheader2
		'
		Me.lblheader2.Location = New System.Drawing.Point(110, 38)
		Me.lblheader2.Name = "lblheader2"
		Me.lblheader2.Size = New System.Drawing.Size(218, 13)
		Me.lblheader2.TabIndex = 1
		Me.lblheader2.Text = "Wählen Sie Ihre gewünschten Kriterien aus..."
		'
		'lblheader1
		'
		Me.lblheader1.Appearance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblheader1.Appearance.Options.UseFont = True
		Me.lblheader1.Location = New System.Drawing.Point(92, 14)
		Me.lblheader1.Name = "lblheader1"
		Me.lblheader1.Size = New System.Drawing.Size(188, 13)
		Me.lblheader1.TabIndex = 0
		Me.lblheader1.Text = "Erweiterte Suche nach Debitoren"
		'
		'Label14
		'
		Me.Label14.Location = New System.Drawing.Point(411, 307)
		Me.Label14.Name = "Label14"
		Me.Label14.Size = New System.Drawing.Size(49, 13)
		Me.Label14.TabIndex = 23
		Me.Label14.Text = "2. Status"
		'
		'Label15
		'
		Me.Label15.Location = New System.Drawing.Point(411, 256)
		Me.Label15.Name = "Label15"
		Me.Label15.Size = New System.Drawing.Size(44, 13)
		Me.Label15.TabIndex = 21
		Me.Label15.Text = "Kontakt"
		'
		'Label16
		'
		Me.Label16.Location = New System.Drawing.Point(409, 281)
		Me.Label16.Name = "Label16"
		Me.Label16.Size = New System.Drawing.Size(49, 13)
		Me.Label16.TabIndex = 22
		Me.Label16.Text = "1. Status"
		'
		'Label17
		'
		Me.Label17.Location = New System.Drawing.Point(36, 246)
		Me.Label17.Name = "Label17"
		Me.Label17.Size = New System.Drawing.Size(75, 13)
		Me.Label17.TabIndex = 20
		Me.Label17.Text = "1. Eigenschaft"
		'
		'Label18
		'
		Me.Label18.Location = New System.Drawing.Point(36, 220)
		Me.Label18.Name = "Label18"
		Me.Label18.Size = New System.Drawing.Size(50, 13)
		Me.Label18.TabIndex = 19
		Me.Label18.Text = "BeraterIn"
		'
		'Label19
		'
		Me.Label19.Location = New System.Drawing.Point(34, 194)
		Me.Label19.Name = "Label19"
		Me.Label19.Size = New System.Drawing.Size(31, 13)
		Me.Label19.TabIndex = 18
		Me.Label19.Text = "Land"
		'
		'Label20
		'
		Me.Label20.Location = New System.Drawing.Point(36, 169)
		Me.Label20.Name = "Label20"
		Me.Label20.Size = New System.Drawing.Size(21, 13)
		Me.Label20.TabIndex = 17
		Me.Label20.Text = "Ort"
		'
		'Label21
		'
		Me.Label21.Location = New System.Drawing.Point(34, 143)
		Me.Label21.Name = "Label21"
		Me.Label21.Size = New System.Drawing.Size(27, 13)
		Me.Label21.TabIndex = 16
		Me.Label21.Text = "PLZ"
		'
		'Label22
		'
		Me.Label22.Location = New System.Drawing.Point(36, 117)
		Me.Label22.Name = "Label22"
		Me.Label22.Size = New System.Drawing.Size(32, 13)
		Me.Label22.TabIndex = 15
		Me.Label22.Text = "Firma"
		'
		'Label23
		'
		Me.Label23.Location = New System.Drawing.Point(34, 91)
		Me.Label23.Name = "Label23"
		Me.Label23.Size = New System.Drawing.Size(46, 13)
		Me.Label23.TabIndex = 14
		Me.Label23.Text = "Nummer"
		'
		'Label24
		'
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
		'lblderzeitigeabfrage
		'
		Me.lblderzeitigeabfrage.Location = New System.Drawing.Point(29, 20)
		Me.lblderzeitigeabfrage.Name = "lblderzeitigeabfrage"
		Me.lblderzeitigeabfrage.Size = New System.Drawing.Size(147, 13)
		Me.lblderzeitigeabfrage.TabIndex = 73
		Me.lblderzeitigeabfrage.Text = "Ihre derzeitige Abfrage lautet:"
		'
		'txt_SQLQuery
		'
		Me.txt_SQLQuery.Location = New System.Drawing.Point(29, 39)
		Me.txt_SQLQuery.Name = "txt_SQLQuery"
		Me.txt_SQLQuery.Size = New System.Drawing.Size(587, 464)
		Me.txt_SQLQuery.TabIndex = 1
		'
		'lblAbfrage
		'
		Me.lblAbfrage.Location = New System.Drawing.Point(29, 20)
		Me.lblAbfrage.Name = "lblAbfrage"
		Me.lblAbfrage.Size = New System.Drawing.Size(62, 13)
		Me.lblAbfrage.TabIndex = 74
		Me.lblAbfrage.Text = "Ihre Abfrage"
		'
		'txt_IndSQLQuery
		'
		Me.txt_IndSQLQuery.Location = New System.Drawing.Point(29, 39)
		Me.txt_IndSQLQuery.Name = "txt_IndSQLQuery"
		Me.txt_IndSQLQuery.Size = New System.Drawing.Size(587, 464)
		Me.txt_IndSQLQuery.TabIndex = 2
		'
		'deMahnDate
		'
		Me.deMahnDate.EditValue = ""
		Me.deMahnDate.Location = New System.Drawing.Point(287, 52)
		Me.deMahnDate.Name = "deMahnDate"
		Me.deMahnDate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.deMahnDate.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.deMahnDate.Size = New System.Drawing.Size(96, 20)
		Me.deMahnDate.TabIndex = 192
		'
		'lblmahndatum
		'
		Me.lblmahndatum.Appearance.Options.UseTextOptions = True
		Me.lblmahndatum.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblmahndatum.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblmahndatum.Location = New System.Drawing.Point(191, 56)
		Me.lblmahndatum.Name = "lblmahndatum"
		Me.lblmahndatum.Size = New System.Drawing.Size(90, 13)
		Me.lblmahndatum.TabIndex = 191
		Me.lblmahndatum.Text = "Datum"
		'
		'cbo_Mahnstufe
		'
		Me.cbo_Mahnstufe.Location = New System.Drawing.Point(134, 53)
		Me.cbo_Mahnstufe.Name = "cbo_Mahnstufe"
		Me.cbo_Mahnstufe.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.cbo_Mahnstufe.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
		Me.cbo_Mahnstufe.Size = New System.Drawing.Size(104, 20)
		Me.cbo_Mahnstufe.TabIndex = 189
		'
		'lblmahnstufe
		'
		Me.lblmahnstufe.Appearance.Options.UseTextOptions = True
		Me.lblmahnstufe.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblmahnstufe.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblmahnstufe.Location = New System.Drawing.Point(5, 57)
		Me.lblmahnstufe.Name = "lblmahnstufe"
		Me.lblmahnstufe.Size = New System.Drawing.Size(123, 13)
		Me.lblmahnstufe.TabIndex = 188
		Me.lblmahnstufe.Text = "Gemahnte Stufe"
		'
		'deCreated_2
		'
		Me.deCreated_2.EditValue = Nothing
		Me.deCreated_2.Location = New System.Drawing.Point(557, 130)
		Me.deCreated_2.Name = "deCreated_2"
		Me.deCreated_2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.deCreated_2.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.deCreated_2.Size = New System.Drawing.Size(84, 20)
		Me.deCreated_2.TabIndex = 235
		'
		'deCreated_1
		'
		Me.deCreated_1.EditValue = Nothing
		Me.deCreated_1.Location = New System.Drawing.Point(467, 130)
		Me.deCreated_1.Name = "deCreated_1"
		Me.deCreated_1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.deCreated_1.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.deCreated_1.Size = New System.Drawing.Size(84, 20)
		Me.deCreated_1.TabIndex = 234
		'
		'lblerstellungzwischen
		'
		Me.lblerstellungzwischen.Appearance.Options.UseTextOptions = True
		Me.lblerstellungzwischen.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblerstellungzwischen.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblerstellungzwischen.Location = New System.Drawing.Point(375, 134)
		Me.lblerstellungzwischen.Name = "lblerstellungzwischen"
		Me.lblerstellungzwischen.Size = New System.Drawing.Size(86, 13)
		Me.lblerstellungzwischen.TabIndex = 17
		Me.lblerstellungzwischen.Text = "Zwischen"
		'
		'Cbo_ErstellPeriode
		'
		Me.Cbo_ErstellPeriode.Location = New System.Drawing.Point(134, 130)
		Me.Cbo_ErstellPeriode.Name = "Cbo_ErstellPeriode"
		Me.Cbo_ErstellPeriode.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_ErstellPeriode.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
		Me.Cbo_ErstellPeriode.Size = New System.Drawing.Size(114, 20)
		Me.Cbo_ErstellPeriode.TabIndex = 40
		'
		'lblErstellperiode
		'
		Me.lblErstellperiode.Appearance.Options.UseTextOptions = True
		Me.lblErstellperiode.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblErstellperiode.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblErstellperiode.Location = New System.Drawing.Point(33, 134)
		Me.lblErstellperiode.Name = "lblErstellperiode"
		Me.lblErstellperiode.Size = New System.Drawing.Size(95, 13)
		Me.lblErstellperiode.TabIndex = 20
		Me.lblErstellperiode.Text = "Periode"
		'
		'lblkundennr
		'
		Me.lblkundennr.Appearance.Options.UseTextOptions = True
		Me.lblkundennr.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblkundennr.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblkundennr.Location = New System.Drawing.Point(28, 161)
		Me.lblkundennr.Name = "lblkundennr"
		Me.lblkundennr.Size = New System.Drawing.Size(108, 13)
		Me.lblkundennr.TabIndex = 96
		Me.lblkundennr.TabStop = True
		Me.lblkundennr.Text = "Kunden-Nr."
		'
		'lblnummer
		'
		Me.lblnummer.Appearance.Options.UseTextOptions = True
		Me.lblnummer.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblnummer.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblnummer.Location = New System.Drawing.Point(28, 135)
		Me.lblnummer.Name = "lblnummer"
		Me.lblnummer.Size = New System.Drawing.Size(108, 13)
		Me.lblnummer.TabIndex = 95
		Me.lblnummer.TabStop = True
		Me.lblnummer.Text = "Nummer"
		'
		'CboSort
		'
		Me.CboSort.Location = New System.Drawing.Point(142, 60)
		Me.CboSort.Name = "CboSort"
		Me.CboSort.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.CboSort.Properties.SelectAllItemVisible = False
		Me.CboSort.Size = New System.Drawing.Size(469, 20)
		Me.CboSort.TabIndex = 10
		'
		'txt_OpenBetrag_2
		'
		Me.txt_OpenBetrag_2.EditValue = "0.00"
		Me.txt_OpenBetrag_2.Location = New System.Drawing.Point(542, 337)
		Me.txt_OpenBetrag_2.Name = "txt_OpenBetrag_2"
		Me.txt_OpenBetrag_2.Size = New System.Drawing.Size(67, 20)
		Me.txt_OpenBetrag_2.TabIndex = 180
		'
		'lblOffenerbetrag
		'
		Me.lblOffenerbetrag.Appearance.Options.UseTextOptions = True
		Me.lblOffenerbetrag.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblOffenerbetrag.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblOffenerbetrag.Location = New System.Drawing.Point(140, 341)
		Me.lblOffenerbetrag.Name = "lblOffenerbetrag"
		Me.lblOffenerbetrag.Size = New System.Drawing.Size(325, 13)
		Me.lblOffenerbetrag.TabIndex = 88
		Me.lblOffenerbetrag.Text = "Offener Betrag pro Kunde (per heute)"
		'
		'txt_OpenBetrag_1
		'
		Me.txt_OpenBetrag_1.EditValue = "0.00"
		Me.txt_OpenBetrag_1.Location = New System.Drawing.Point(471, 337)
		Me.txt_OpenBetrag_1.Name = "txt_OpenBetrag_1"
		Me.txt_OpenBetrag_1.Size = New System.Drawing.Size(67, 20)
		Me.txt_OpenBetrag_1.TabIndex = 170
		'
		'Cbo_KST1
		'
		Me.Cbo_KST1.Location = New System.Drawing.Point(134, 30)
		Me.Cbo_KST1.Name = "Cbo_KST1"
		Me.Cbo_KST1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_KST1.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
		Me.Cbo_KST1.Size = New System.Drawing.Size(138, 20)
		Me.Cbo_KST1.TabIndex = 90
		'
		'Cbo_KST2
		'
		Me.Cbo_KST2.Location = New System.Drawing.Point(134, 56)
		Me.Cbo_KST2.Name = "Cbo_KST2"
		Me.Cbo_KST2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_KST2.Size = New System.Drawing.Size(138, 20)
		Me.Cbo_KST2.TabIndex = 110
		'
		'lbl1kst
		'
		Me.lbl1kst.Appearance.Options.UseTextOptions = True
		Me.lbl1kst.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lbl1kst.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lbl1kst.Location = New System.Drawing.Point(20, 35)
		Me.lbl1kst.Name = "lbl1kst"
		Me.lbl1kst.Size = New System.Drawing.Size(108, 13)
		Me.lbl1kst.TabIndex = 81
		Me.lbl1kst.Text = "1. Kst."
		'
		'lbllistenart
		'
		Me.lbllistenart.Appearance.Options.UseTextOptions = True
		Me.lbllistenart.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lbllistenart.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lbllistenart.Location = New System.Drawing.Point(28, 90)
		Me.lbllistenart.Name = "lbllistenart"
		Me.lbllistenart.Size = New System.Drawing.Size(108, 13)
		Me.lbllistenart.TabIndex = 80
		Me.lbllistenart.Text = "Art der Liste"
		'
		'Cbo_ListingArt
		'
		Me.Cbo_ListingArt.Location = New System.Drawing.Point(142, 86)
		Me.Cbo_ListingArt.Name = "Cbo_ListingArt"
		Me.Cbo_ListingArt.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_ListingArt.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
		Me.Cbo_ListingArt.Size = New System.Drawing.Size(469, 20)
		Me.Cbo_ListingArt.TabIndex = 20
		'
		'Cbo_Berater
		'
		Me.Cbo_Berater.Location = New System.Drawing.Point(467, 28)
		Me.Cbo_Berater.Name = "Cbo_Berater"
		Me.Cbo_Berater.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_Berater.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
		Me.Cbo_Berater.Size = New System.Drawing.Size(138, 20)
		Me.Cbo_Berater.TabIndex = 130
		'
		'lblberater
		'
		Me.lblberater.Appearance.Options.UseTextOptions = True
		Me.lblberater.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblberater.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblberater.Location = New System.Drawing.Point(353, 32)
		Me.lblberater.Name = "lblberater"
		Me.lblberater.Size = New System.Drawing.Size(108, 13)
		Me.lblberater.TabIndex = 78
		Me.lblberater.Text = "BeraterIn"
		'
		'lblfakturazwischen
		'
		Me.lblfakturazwischen.Appearance.Options.UseTextOptions = True
		Me.lblfakturazwischen.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblfakturazwischen.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblfakturazwischen.Location = New System.Drawing.Point(375, 82)
		Me.lblfakturazwischen.Name = "lblfakturazwischen"
		Me.lblfakturazwischen.Size = New System.Drawing.Size(86, 13)
		Me.lblfakturazwischen.TabIndex = 17
		Me.lblfakturazwischen.Text = "Zwischen"
		'
		'deFakDate_2
		'
		Me.deFakDate_2.EditValue = Nothing
		Me.deFakDate_2.Location = New System.Drawing.Point(557, 78)
		Me.deFakDate_2.Name = "deFakDate_2"
		Me.deFakDate_2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.deFakDate_2.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.deFakDate_2.Size = New System.Drawing.Size(84, 20)
		Me.deFakDate_2.TabIndex = 233
		'
		'Cbo_FakPeriode
		'
		Me.Cbo_FakPeriode.Location = New System.Drawing.Point(134, 78)
		Me.Cbo_FakPeriode.Name = "Cbo_FakPeriode"
		Me.Cbo_FakPeriode.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_FakPeriode.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
		Me.Cbo_FakPeriode.Size = New System.Drawing.Size(114, 20)
		Me.Cbo_FakPeriode.TabIndex = 40
		'
		'deFakDate_1
		'
		Me.deFakDate_1.EditValue = ""
		Me.deFakDate_1.Location = New System.Drawing.Point(467, 78)
		Me.deFakDate_1.Name = "deFakDate_1"
		Me.deFakDate_1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.deFakDate_1.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.deFakDate_1.Size = New System.Drawing.Size(84, 20)
		Me.deFakDate_1.TabIndex = 232
		'
		'lblfakturaperiode
		'
		Me.lblfakturaperiode.Appearance.Options.UseTextOptions = True
		Me.lblfakturaperiode.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblfakturaperiode.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblfakturaperiode.Location = New System.Drawing.Point(33, 82)
		Me.lblfakturaperiode.Name = "lblfakturaperiode"
		Me.lblfakturaperiode.Size = New System.Drawing.Size(95, 13)
		Me.lblfakturaperiode.TabIndex = 20
		Me.lblfakturaperiode.Text = "Periode"
		'
		'Cbo_Filiale
		'
		Me.Cbo_Filiale.Location = New System.Drawing.Point(467, 54)
		Me.Cbo_Filiale.Name = "Cbo_Filiale"
		Me.Cbo_Filiale.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_Filiale.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
		Me.Cbo_Filiale.Size = New System.Drawing.Size(138, 20)
		Me.Cbo_Filiale.TabIndex = 150
		'
		'lblfiliale
		'
		Me.lblfiliale.Appearance.Options.UseTextOptions = True
		Me.lblfiliale.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblfiliale.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblfiliale.Location = New System.Drawing.Point(353, 58)
		Me.lblfiliale.Name = "lblfiliale"
		Me.lblfiliale.Size = New System.Drawing.Size(108, 13)
		Me.lblfiliale.TabIndex = 57
		Me.lblfiliale.Text = "Filiale"
		'
		'lbl2kst
		'
		Me.lbl2kst.Appearance.Options.UseTextOptions = True
		Me.lbl2kst.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lbl2kst.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lbl2kst.Location = New System.Drawing.Point(20, 60)
		Me.lbl2kst.Name = "lbl2kst"
		Me.lbl2kst.Size = New System.Drawing.Size(108, 13)
		Me.lbl2kst.TabIndex = 23
		Me.lbl2kst.Text = "2. Kst."
		'
		'lblart
		'
		Me.lblart.Appearance.Options.UseTextOptions = True
		Me.lblart.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblart.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblart.Location = New System.Drawing.Point(15, 31)
		Me.lblart.Name = "lblart"
		Me.lblart.Size = New System.Drawing.Size(37, 13)
		Me.lblart.TabIndex = 21
		Me.lblart.Text = "Art"
		'
		'lblmahncode
		'
		Me.lblmahncode.Appearance.Options.UseTextOptions = True
		Me.lblmahncode.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblmahncode.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblmahncode.Location = New System.Drawing.Point(5, 31)
		Me.lblmahncode.Name = "lblmahncode"
		Me.lblmahncode.Size = New System.Drawing.Size(123, 13)
		Me.lblmahncode.TabIndex = 19
		Me.lblmahncode.Text = "Kunden Mahncode"
		'
		'lblverfalltage
		'
		Me.lblverfalltage.Appearance.Options.UseTextOptions = True
		Me.lblverfalltage.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblverfalltage.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblverfalltage.Location = New System.Drawing.Point(28, 187)
		Me.lblverfalltage.Name = "lblverfalltage"
		Me.lblverfalltage.Size = New System.Drawing.Size(108, 13)
		Me.lblverfalltage.TabIndex = 16
		Me.lblverfalltage.Text = "Verfalltag"
		'
		'lblsortierung
		'
		Me.lblsortierung.Appearance.Options.UseTextOptions = True
		Me.lblsortierung.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblsortierung.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblsortierung.Location = New System.Drawing.Point(28, 64)
		Me.lblsortierung.Name = "lblsortierung"
		Me.lblsortierung.Size = New System.Drawing.Size(108, 13)
		Me.lblsortierung.TabIndex = 13
		Me.lblsortierung.Text = "Sortieren nach"
		'
		'chkKDKreditlimiteUeberschritten
		'
		Me.chkKDKreditlimiteUeberschritten.Location = New System.Drawing.Point(409, 359)
		Me.chkKDKreditlimiteUeberschritten.Name = "chkKDKreditlimiteUeberschritten"
		Me.chkKDKreditlimiteUeberschritten.Properties.AllowFocused = False
		Me.chkKDKreditlimiteUeberschritten.Properties.Appearance.Options.UseTextOptions = True
		Me.chkKDKreditlimiteUeberschritten.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.chkKDKreditlimiteUeberschritten.Properties.Caption = "Kreditlimite überschritten"
		Me.chkKDKreditlimiteUeberschritten.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.chkKDKreditlimiteUeberschritten.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkKDKreditlimiteUeberschritten.Size = New System.Drawing.Size(203, 20)
		Me.chkKDKreditlimiteUeberschritten.TabIndex = 0
		'
		'lbleinstufung
		'
		Me.lbleinstufung.Appearance.Options.UseTextOptions = True
		Me.lbleinstufung.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lbleinstufung.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lbleinstufung.Location = New System.Drawing.Point(5, 34)
		Me.lbleinstufung.Name = "lbleinstufung"
		Me.lbleinstufung.Size = New System.Drawing.Size(123, 13)
		Me.lbleinstufung.TabIndex = 85
		Me.lbleinstufung.Text = "Einstufung"
		'
		'cbo_ESBranche
		'
		Me.cbo_ESBranche.Location = New System.Drawing.Point(134, 56)
		Me.cbo_ESBranche.Name = "cbo_ESBranche"
		Me.cbo_ESBranche.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.cbo_ESBranche.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
		Me.cbo_ESBranche.Size = New System.Drawing.Size(469, 20)
		Me.cbo_ESBranche.TabIndex = 1
		'
		'lblBranche
		'
		Me.lblBranche.Appearance.Options.UseTextOptions = True
		Me.lblBranche.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblBranche.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblBranche.Location = New System.Drawing.Point(5, 60)
		Me.lblBranche.Name = "lblBranche"
		Me.lblBranche.Size = New System.Drawing.Size(123, 13)
		Me.lblBranche.TabIndex = 161
		Me.lblBranche.Text = "Branche"
		'
		'Cbo_ESEinstufung
		'
		Me.Cbo_ESEinstufung.Location = New System.Drawing.Point(134, 30)
		Me.Cbo_ESEinstufung.Name = "Cbo_ESEinstufung"
		Me.Cbo_ESEinstufung.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_ESEinstufung.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
		Me.Cbo_ESEinstufung.Size = New System.Drawing.Size(469, 20)
		Me.Cbo_ESEinstufung.TabIndex = 0
		'
		'cbo_ESRBank
		'
		Me.cbo_ESRBank.Location = New System.Drawing.Point(134, 30)
		Me.cbo_ESRBank.Name = "cbo_ESRBank"
		Me.cbo_ESRBank.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.cbo_ESRBank.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
		Me.cbo_ESRBank.Size = New System.Drawing.Size(469, 20)
		Me.cbo_ESRBank.TabIndex = 0
		'
		'lblESBank
		'
		Me.lblESBank.Appearance.Options.UseTextOptions = True
		Me.lblESBank.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblESBank.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblESBank.Location = New System.Drawing.Point(5, 33)
		Me.lblESBank.Name = "lblESBank"
		Me.lblESBank.Size = New System.Drawing.Size(123, 13)
		Me.lblESBank.TabIndex = 193
		Me.lblESBank.Text = "ESR-Bank"
		'
		'BackgroundWorker1
		'
		'
		'xtabOPSearch
		'
		Me.xtabOPSearch.Location = New System.Drawing.Point(23, 107)
		Me.xtabOPSearch.Name = "xtabOPSearch"
		Me.xtabOPSearch.SelectedTabPage = Me.xtabAllgemein
		Me.xtabOPSearch.Size = New System.Drawing.Size(666, 553)
		Me.xtabOPSearch.TabIndex = 231
		Me.xtabOPSearch.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabAllgemein, Me.xtabSonstiges, Me.xtabErweitert, Me.xtabSQLAbfrage, Me.xtabAdmin})
		'
		'xtabAllgemein
		'
		Me.xtabAllgemein.Controls.Add(Me.grpInvoiceDate)
		Me.xtabAllgemein.Controls.Add(Me.grpKostenteilung)
		Me.xtabAllgemein.Controls.Add(Me.chkValutaFromCreatedOn)
		Me.xtabAllgemein.Controls.Add(Me.SwitchButton2)
		Me.xtabAllgemein.Controls.Add(Me.SwitchButton1)
		Me.xtabAllgemein.Controls.Add(Me.lueMandant)
		Me.xtabAllgemein.Controls.Add(Me.lblMDName)
		Me.xtabAllgemein.Controls.Add(Me.lblsortierung)
		Me.xtabAllgemein.Controls.Add(Me.lblverfalltage)
		Me.xtabAllgemein.Controls.Add(Me.lblkundennr)
		Me.xtabAllgemein.Controls.Add(Me.lblnummer)
		Me.xtabAllgemein.Controls.Add(Me.CboSort)
		Me.xtabAllgemein.Controls.Add(Me.Cbo_ListingArt)
		Me.xtabAllgemein.Controls.Add(Me.lbllistenart)
		Me.xtabAllgemein.Controls.Add(Me.txtKDNr_1)
		Me.xtabAllgemein.Controls.Add(Me.txtOPNr_2)
		Me.xtabAllgemein.Controls.Add(Me.txtKDNr_2)
		Me.xtabAllgemein.Controls.Add(Me.txtOPNr_1)
		Me.xtabAllgemein.Controls.Add(Me.txtVGNr_1)
		Me.xtabAllgemein.Controls.Add(Me.txtVGNr_2)
		Me.xtabAllgemein.Name = "xtabAllgemein"
		Me.xtabAllgemein.Size = New System.Drawing.Size(664, 528)
		Me.xtabAllgemein.Text = "Allgemein"
		'
		'grpInvoiceDate
		'
		Me.grpInvoiceDate.AppearanceCaption.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.grpInvoiceDate.AppearanceCaption.Options.UseFont = True
		Me.grpInvoiceDate.Controls.Add(Me.lblCreateDate)
		Me.grpInvoiceDate.Controls.Add(Me.lblInvoiceDate)
		Me.grpInvoiceDate.Controls.Add(Me.lblStichtag)
		Me.grpInvoiceDate.Controls.Add(Me.deStichtag)
		Me.grpInvoiceDate.Controls.Add(Me.lblfakturazwischen)
		Me.grpInvoiceDate.Controls.Add(Me.deCreated_2)
		Me.grpInvoiceDate.Controls.Add(Me.deFakDate_2)
		Me.grpInvoiceDate.Controls.Add(Me.deCreated_1)
		Me.grpInvoiceDate.Controls.Add(Me.Cbo_FakPeriode)
		Me.grpInvoiceDate.Controls.Add(Me.lblErstellperiode)
		Me.grpInvoiceDate.Controls.Add(Me.deFakDate_1)
		Me.grpInvoiceDate.Controls.Add(Me.lblfakturaperiode)
		Me.grpInvoiceDate.Controls.Add(Me.lblerstellungzwischen)
		Me.grpInvoiceDate.Controls.Add(Me.Cbo_ErstellPeriode)
		Me.grpInvoiceDate.GroupStyle = DevExpress.Utils.GroupStyle.Light
		Me.grpInvoiceDate.Location = New System.Drawing.Point(6, 340)
		Me.grpInvoiceDate.Name = "grpInvoiceDate"
		Me.grpInvoiceDate.Size = New System.Drawing.Size(651, 171)
		Me.grpInvoiceDate.TabIndex = 238
		Me.grpInvoiceDate.Text = "Perioden / Stichtag"
		'
		'lblCreateDate
		'
		Me.lblCreateDate.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.lblCreateDate.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.lblCreateDate.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.lblCreateDate.Appearance.Options.UseBackColor = True
		Me.lblCreateDate.Appearance.Options.UseFont = True
		Me.lblCreateDate.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblCreateDate.LineVisible = True
		Me.lblCreateDate.Location = New System.Drawing.Point(19, 111)
		Me.lblCreateDate.Name = "lblCreateDate"
		Me.lblCreateDate.Size = New System.Drawing.Size(610, 13)
		Me.lblCreateDate.TabIndex = 342
		Me.lblCreateDate.Text = "Erstellungsdatum"
		'
		'lblInvoiceDate
		'
		Me.lblInvoiceDate.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.lblInvoiceDate.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.lblInvoiceDate.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.lblInvoiceDate.Appearance.Options.UseBackColor = True
		Me.lblInvoiceDate.Appearance.Options.UseFont = True
		Me.lblInvoiceDate.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblInvoiceDate.LineVisible = True
		Me.lblInvoiceDate.Location = New System.Drawing.Point(19, 58)
		Me.lblInvoiceDate.Name = "lblInvoiceDate"
		Me.lblInvoiceDate.Size = New System.Drawing.Size(610, 13)
		Me.lblInvoiceDate.TabIndex = 341
		Me.lblInvoiceDate.Text = "Rechnungsdatum"
		'
		'lblStichtag
		'
		Me.lblStichtag.Appearance.Options.UseTextOptions = True
		Me.lblStichtag.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblStichtag.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblStichtag.Location = New System.Drawing.Point(375, 34)
		Me.lblStichtag.Name = "lblStichtag"
		Me.lblStichtag.Size = New System.Drawing.Size(86, 13)
		Me.lblStichtag.TabIndex = 236
		Me.lblStichtag.Text = "Stichtag"
		'
		'deStichtag
		'
		Me.deStichtag.EditValue = ""
		Me.deStichtag.Location = New System.Drawing.Point(467, 30)
		Me.deStichtag.Name = "deStichtag"
		Me.deStichtag.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.deStichtag.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.deStichtag.Properties.CalendarTimeProperties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.[Default]
		Me.deStichtag.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.Buffered
		Me.deStichtag.Size = New System.Drawing.Size(84, 20)
		Me.deStichtag.TabIndex = 237
		'
		'grpKostenteilung
		'
		Me.grpKostenteilung.AppearanceCaption.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.grpKostenteilung.AppearanceCaption.Options.UseFont = True
		Me.grpKostenteilung.Controls.Add(Me.Cbo_KST1)
		Me.grpKostenteilung.Controls.Add(Me.lbl1kst)
		Me.grpKostenteilung.Controls.Add(Me.Cbo_KST2)
		Me.grpKostenteilung.Controls.Add(Me.Cbo_Berater)
		Me.grpKostenteilung.Controls.Add(Me.lblberater)
		Me.grpKostenteilung.Controls.Add(Me.Cbo_Filiale)
		Me.grpKostenteilung.Controls.Add(Me.lblfiliale)
		Me.grpKostenteilung.Controls.Add(Me.lbl2kst)
		Me.grpKostenteilung.GroupStyle = DevExpress.Utils.GroupStyle.Light
		Me.grpKostenteilung.Location = New System.Drawing.Point(6, 235)
		Me.grpKostenteilung.Name = "grpKostenteilung"
		Me.grpKostenteilung.Size = New System.Drawing.Size(651, 95)
		Me.grpKostenteilung.TabIndex = 238
		Me.grpKostenteilung.Text = "Kostenteilung"
		'
		'chkValutaFromCreatedOn
		'
		Me.chkValutaFromCreatedOn.Location = New System.Drawing.Point(142, 209)
		Me.chkValutaFromCreatedOn.Name = "chkValutaFromCreatedOn"
		Me.chkValutaFromCreatedOn.Properties.AllowFocused = False
		Me.chkValutaFromCreatedOn.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.[True]
		Me.chkValutaFromCreatedOn.Properties.AutoWidth = True
		Me.chkValutaFromCreatedOn.Properties.Caption = "Ab Erstellungsdatum berechnen"
		Me.chkValutaFromCreatedOn.Size = New System.Drawing.Size(175, 20)
		Me.chkValutaFromCreatedOn.TabIndex = 254
		Me.chkValutaFromCreatedOn.TabStop = False
		'
		'SwitchButton2
		'
		Me.SwitchButton2.BackColor = System.Drawing.Color.White
		'
		'
		'
		Me.SwitchButton2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
		Me.SwitchButton2.FocusCuesEnabled = False
		Me.SwitchButton2.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.SwitchButton2.Location = New System.Drawing.Point(429, 158)
		Me.SwitchButton2.Name = "SwitchButton2"
		Me.SwitchButton2.OffBackColor = System.Drawing.Color.White
		Me.SwitchButton2.OffText = "O"
		Me.SwitchButton2.OffTextColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(53, Byte), Integer))
		Me.SwitchButton2.OnBackColor = System.Drawing.Color.LightSteelBlue
		Me.SwitchButton2.OnText = "|"
		Me.SwitchButton2.OnTextColor = System.Drawing.Color.Black
		Me.SwitchButton2.Size = New System.Drawing.Size(38, 19)
		Me.SwitchButton2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
		Me.SwitchButton2.SwitchBackColor = System.Drawing.Color.DarkGray
		Me.SwitchButton2.SwitchBorderColor = System.Drawing.Color.DarkGray
		Me.SwitchButton2.SwitchFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.SwitchButton2.SwitchWidth = 6
		Me.SwitchButton2.TabIndex = 253
		'
		'SwitchButton1
		'
		Me.SwitchButton1.BackColor = System.Drawing.Color.White
		'
		'
		'
		Me.SwitchButton1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
		Me.SwitchButton1.FocusCuesEnabled = False
		Me.SwitchButton1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.SwitchButton1.Location = New System.Drawing.Point(429, 132)
		Me.SwitchButton1.Name = "SwitchButton1"
		Me.SwitchButton1.OffBackColor = System.Drawing.Color.White
		Me.SwitchButton1.OffText = "O"
		Me.SwitchButton1.OffTextColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(53, Byte), Integer))
		Me.SwitchButton1.OnBackColor = System.Drawing.Color.LightSteelBlue
		Me.SwitchButton1.OnText = "|"
		Me.SwitchButton1.OnTextColor = System.Drawing.Color.Black
		Me.SwitchButton1.Size = New System.Drawing.Size(38, 19)
		Me.SwitchButton1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
		Me.SwitchButton1.SwitchBackColor = System.Drawing.Color.DarkGray
		Me.SwitchButton1.SwitchBorderColor = System.Drawing.Color.DarkGray
		Me.SwitchButton1.SwitchFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.SwitchButton1.SwitchWidth = 6
		Me.SwitchButton1.TabIndex = 252
		'
		'lueMandant
		'
		Me.lueMandant.Location = New System.Drawing.Point(142, 34)
		Me.lueMandant.Name = "lueMandant"
		Me.lueMandant.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.lueMandant.Properties.NullText = ""
		Me.lueMandant.Size = New System.Drawing.Size(469, 20)
		Me.lueMandant.TabIndex = 199
		'
		'lblMDName
		'
		Me.lblMDName.Appearance.Options.UseTextOptions = True
		Me.lblMDName.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblMDName.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblMDName.Location = New System.Drawing.Point(28, 38)
		Me.lblMDName.Name = "lblMDName"
		Me.lblMDName.Size = New System.Drawing.Size(108, 13)
		Me.lblMDName.TabIndex = 200
		Me.lblMDName.Text = "Mandanten"
		'
		'txtKDNr_1
		'
		Me.txtKDNr_1.Location = New System.Drawing.Point(142, 157)
		Me.txtKDNr_1.Name = "txtKDNr_1"
		Me.txtKDNr_1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.txtKDNr_1.Size = New System.Drawing.Size(138, 20)
		Me.txtKDNr_1.TabIndex = 50
		'
		'txtOPNr_2
		'
		Me.txtOPNr_2.Location = New System.Drawing.Point(473, 131)
		Me.txtOPNr_2.Name = "txtOPNr_2"
		Me.txtOPNr_2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.txtOPNr_2.Size = New System.Drawing.Size(138, 20)
		Me.txtOPNr_2.TabIndex = 40
		Me.txtOPNr_2.Visible = False
		'
		'txtKDNr_2
		'
		Me.txtKDNr_2.Location = New System.Drawing.Point(473, 157)
		Me.txtKDNr_2.Name = "txtKDNr_2"
		Me.txtKDNr_2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.txtKDNr_2.Size = New System.Drawing.Size(138, 20)
		Me.txtKDNr_2.TabIndex = 60
		Me.txtKDNr_2.Visible = False
		'
		'txtOPNr_1
		'
		Me.txtOPNr_1.Location = New System.Drawing.Point(142, 131)
		Me.txtOPNr_1.Name = "txtOPNr_1"
		Me.txtOPNr_1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.txtOPNr_1.Size = New System.Drawing.Size(138, 20)
		Me.txtOPNr_1.TabIndex = 30
		'
		'txtVGNr_1
		'
		Me.txtVGNr_1.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
		Me.txtVGNr_1.Location = New System.Drawing.Point(142, 183)
		Me.txtVGNr_1.Name = "txtVGNr_1"
		Me.txtVGNr_1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.txtVGNr_1.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.[Default]
		Me.txtVGNr_1.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None
		Me.txtVGNr_1.Size = New System.Drawing.Size(62, 20)
		Me.txtVGNr_1.TabIndex = 70
		'
		'txtVGNr_2
		'
		Me.txtVGNr_2.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
		Me.txtVGNr_2.Location = New System.Drawing.Point(218, 183)
		Me.txtVGNr_2.Name = "txtVGNr_2"
		Me.txtVGNr_2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.txtVGNr_2.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.[Default]
		Me.txtVGNr_2.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None
		Me.txtVGNr_2.Size = New System.Drawing.Size(62, 20)
		Me.txtVGNr_2.TabIndex = 80
		'
		'xtabSonstiges
		'
		Me.xtabSonstiges.Controls.Add(Me.grpDebitorenArt)
		Me.xtabSonstiges.Controls.Add(Me.grpMahnArt)
		Me.xtabSonstiges.Controls.Add(Me.grpEinstufung)
		Me.xtabSonstiges.Controls.Add(Me.grpESRAngaben)
		Me.xtabSonstiges.Controls.Add(Me.grpPrintSetting)
		Me.xtabSonstiges.Controls.Add(Me.chkKDKreditlimiteUeberschritten)
		Me.xtabSonstiges.Controls.Add(Me.txt_OpenBetrag_2)
		Me.xtabSonstiges.Controls.Add(Me.lblOffenerbetrag)
		Me.xtabSonstiges.Controls.Add(Me.txt_OpenBetrag_1)
		Me.xtabSonstiges.Name = "xtabSonstiges"
		Me.xtabSonstiges.Size = New System.Drawing.Size(664, 528)
		Me.xtabSonstiges.Text = "Sonstiges"
		'
		'grpDebitorenArt
		'
		Me.grpDebitorenArt.AppearanceCaption.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.grpDebitorenArt.AppearanceCaption.Options.UseFont = True
		Me.grpDebitorenArt.Controls.Add(Me.lueDebitorenart)
		Me.grpDebitorenArt.Controls.Add(Me.lblart)
		Me.grpDebitorenArt.GroupStyle = DevExpress.Utils.GroupStyle.Light
		Me.grpDebitorenArt.Location = New System.Drawing.Point(413, 37)
		Me.grpDebitorenArt.Name = "grpDebitorenArt"
		Me.grpDebitorenArt.Size = New System.Drawing.Size(244, 95)
		Me.grpDebitorenArt.TabIndex = 241
		Me.grpDebitorenArt.Text = "Debitorenart"
		'
		'lueDebitorenart
		'
		Me.lueDebitorenart.Location = New System.Drawing.Point(59, 28)
		Me.lueDebitorenart.Name = "lueDebitorenart"
		Me.lueDebitorenart.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup
		SerializableAppearanceObject1.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject1.Options.UseForeColor = True
		SerializableAppearanceObject2.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject2.Options.UseForeColor = True
		SerializableAppearanceObject3.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject3.Options.UseForeColor = True
		SerializableAppearanceObject4.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject4.Options.UseForeColor = True
		Me.lueDebitorenart.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
		Me.lueDebitorenart.Properties.DropDownRows = 8
		Me.lueDebitorenart.Properties.NullText = ""
		Me.lueDebitorenart.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete
		Me.lueDebitorenart.Properties.ShowFooter = False
		Me.lueDebitorenart.Properties.ShowHeader = False
		Me.lueDebitorenart.Size = New System.Drawing.Size(137, 20)
		Me.lueDebitorenart.TabIndex = 245
		'
		'grpMahnArt
		'
		Me.grpMahnArt.AppearanceCaption.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.grpMahnArt.AppearanceCaption.Options.UseFont = True
		Me.grpMahnArt.Controls.Add(Me.luePaymentReminderCode)
		Me.grpMahnArt.Controls.Add(Me.cbo_Mahnstufe)
		Me.grpMahnArt.Controls.Add(Me.lblmahnstufe)
		Me.grpMahnArt.Controls.Add(Me.lblmahndatum)
		Me.grpMahnArt.Controls.Add(Me.deMahnDate)
		Me.grpMahnArt.Controls.Add(Me.lblmahncode)
		Me.grpMahnArt.GroupStyle = DevExpress.Utils.GroupStyle.Light
		Me.grpMahnArt.Location = New System.Drawing.Point(6, 37)
		Me.grpMahnArt.Name = "grpMahnArt"
		Me.grpMahnArt.Size = New System.Drawing.Size(401, 95)
		Me.grpMahnArt.TabIndex = 237
		Me.grpMahnArt.Text = "Mahnwesen"
		'
		'luePaymentReminderCode
		'
		Me.luePaymentReminderCode.Location = New System.Drawing.Point(134, 28)
		Me.luePaymentReminderCode.Name = "luePaymentReminderCode"
		SerializableAppearanceObject5.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject5.Options.UseForeColor = True
		SerializableAppearanceObject6.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject6.Options.UseForeColor = True
		SerializableAppearanceObject7.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject7.Options.UseForeColor = True
		SerializableAppearanceObject8.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject8.Options.UseForeColor = True
		Me.luePaymentReminderCode.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, EditorButtonImageOptions2, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject5, SerializableAppearanceObject6, SerializableAppearanceObject7, SerializableAppearanceObject8, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
		Me.luePaymentReminderCode.Properties.ShowFooter = False
		Me.luePaymentReminderCode.Size = New System.Drawing.Size(249, 20)
		Me.luePaymentReminderCode.TabIndex = 121
		'
		'grpEinstufung
		'
		Me.grpEinstufung.AppearanceCaption.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.grpEinstufung.AppearanceCaption.Options.UseFont = True
		Me.grpEinstufung.Controls.Add(Me.cbo_ESBranche)
		Me.grpEinstufung.Controls.Add(Me.Cbo_ESEinstufung)
		Me.grpEinstufung.Controls.Add(Me.lblBranche)
		Me.grpEinstufung.Controls.Add(Me.lbleinstufung)
		Me.grpEinstufung.GroupStyle = DevExpress.Utils.GroupStyle.Light
		Me.grpEinstufung.Location = New System.Drawing.Point(6, 138)
		Me.grpEinstufung.Name = "grpEinstufung"
		Me.grpEinstufung.Size = New System.Drawing.Size(651, 95)
		Me.grpEinstufung.TabIndex = 240
		Me.grpEinstufung.Text = "Einstufung und Branchen"
		'
		'grpESRAngaben
		'
		Me.grpESRAngaben.AppearanceCaption.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.grpESRAngaben.AppearanceCaption.Options.UseFont = True
		Me.grpESRAngaben.Controls.Add(Me.cbo_ESRBank)
		Me.grpESRAngaben.Controls.Add(Me.lblESBank)
		Me.grpESRAngaben.GroupStyle = DevExpress.Utils.GroupStyle.Light
		Me.grpESRAngaben.Location = New System.Drawing.Point(6, 239)
		Me.grpESRAngaben.Name = "grpESRAngaben"
		Me.grpESRAngaben.Size = New System.Drawing.Size(651, 80)
		Me.grpESRAngaben.TabIndex = 239
		Me.grpESRAngaben.Text = "ESR-Angaben"
		'
		'grpPrintSetting
		'
		Me.grpPrintSetting.AppearanceCaption.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.grpPrintSetting.AppearanceCaption.Options.UseFont = True
		Me.grpPrintSetting.Controls.Add(Me.chkHideKreditInfoLine)
		Me.grpPrintSetting.Controls.Add(Me.chkHideOPInfoLine)
		Me.grpPrintSetting.Controls.Add(Me.chkHideRefNrLine)
		Me.grpPrintSetting.GroupStyle = DevExpress.Utils.GroupStyle.Light
		Me.grpPrintSetting.Location = New System.Drawing.Point(6, 399)
		Me.grpPrintSetting.Name = "grpPrintSetting"
		Me.grpPrintSetting.Size = New System.Drawing.Size(651, 113)
		Me.grpPrintSetting.TabIndex = 238
		Me.grpPrintSetting.Text = "Einstellungen für Druck"
		'
		'chkHideKreditInfoLine
		'
		Me.chkHideKreditInfoLine.Location = New System.Drawing.Point(29, 80)
		Me.chkHideKreditInfoLine.Name = "chkHideKreditInfoLine"
		Me.chkHideKreditInfoLine.Properties.AllowFocused = False
		Me.chkHideKreditInfoLine.Properties.Caption = "Zeile für ""Kreditinfo"" ausblenden"
		Me.chkHideKreditInfoLine.Size = New System.Drawing.Size(251, 20)
		Me.chkHideKreditInfoLine.TabIndex = 257
		Me.chkHideKreditInfoLine.TabStop = False
		'
		'chkHideOPInfoLine
		'
		Me.chkHideOPInfoLine.Location = New System.Drawing.Point(29, 30)
		Me.chkHideOPInfoLine.Name = "chkHideOPInfoLine"
		Me.chkHideOPInfoLine.Properties.AllowFocused = False
		Me.chkHideOPInfoLine.Properties.Caption = "Zeile für ""OP-Info"" ausblenden"
		Me.chkHideOPInfoLine.Size = New System.Drawing.Size(251, 20)
		Me.chkHideOPInfoLine.TabIndex = 255
		Me.chkHideOPInfoLine.TabStop = False
		'
		'chkHideRefNrLine
		'
		Me.chkHideRefNrLine.Location = New System.Drawing.Point(29, 55)
		Me.chkHideRefNrLine.Name = "chkHideRefNrLine"
		Me.chkHideRefNrLine.Properties.AllowFocused = False
		Me.chkHideRefNrLine.Properties.Caption = "Zeile für ""Ref. Nr."" ausblenden"
		Me.chkHideRefNrLine.Size = New System.Drawing.Size(251, 20)
		Me.chkHideRefNrLine.TabIndex = 256
		Me.chkHideRefNrLine.TabStop = False
		'
		'xtabErweitert
		'
		Me.xtabErweitert.Controls.Add(Me.lblAbfrage)
		Me.xtabErweitert.Controls.Add(Me.txt_IndSQLQuery)
		Me.xtabErweitert.Name = "xtabErweitert"
		Me.xtabErweitert.Size = New System.Drawing.Size(664, 528)
		Me.xtabErweitert.Text = "Erweiterte Abfrage"
		'
		'xtabSQLAbfrage
		'
		Me.xtabSQLAbfrage.Controls.Add(Me.lblderzeitigeabfrage)
		Me.xtabSQLAbfrage.Controls.Add(Me.txt_SQLQuery)
		Me.xtabSQLAbfrage.Name = "xtabSQLAbfrage"
		Me.xtabSQLAbfrage.Size = New System.Drawing.Size(664, 528)
		Me.xtabSQLAbfrage.Text = "SQL-Abfrage"
		'
		'xtabAdmin
		'
		Me.xtabAdmin.Controls.Add(Me.lbladmin)
		Me.xtabAdmin.Controls.Add(Me.txtAdminQuery)
		Me.xtabAdmin.Name = "xtabAdmin"
		Me.xtabAdmin.Size = New System.Drawing.Size(664, 528)
		Me.xtabAdmin.Text = "Admin"
		'
		'lbladmin
		'
		Me.lbladmin.Location = New System.Drawing.Point(29, 20)
		Me.lbladmin.Name = "lbladmin"
		Me.lbladmin.Size = New System.Drawing.Size(29, 13)
		Me.lbladmin.TabIndex = 75
		Me.lbladmin.Text = "Admin"
		'
		'txtAdminQuery
		'
		Me.txtAdminQuery.Location = New System.Drawing.Point(29, 39)
		Me.txtAdminQuery.Name = "txtAdminQuery"
		Me.txtAdminQuery.Size = New System.Drawing.Size(587, 464)
		Me.txtAdminQuery.TabIndex = 74
		'
		'PanelControl1
		'
		Me.PanelControl1.Controls.Add(Me.PictureEdit1)
		Me.PanelControl1.Controls.Add(Me.lblheader1)
		Me.PanelControl1.Controls.Add(Me.lblheader2)
		Me.PanelControl1.Controls.Add(Me.CmdClose)
		Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Top
		Me.PanelControl1.Location = New System.Drawing.Point(0, 0)
		Me.PanelControl1.Name = "PanelControl1"
		Me.PanelControl1.Size = New System.Drawing.Size(720, 79)
		Me.PanelControl1.TabIndex = 232
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
		'BarManager1
		'
		Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar3})
		Me.BarManager1.DockControls.Add(Me.barDockControlTop)
		Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
		Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
		Me.BarManager1.DockControls.Add(Me.barDockControlRight)
		Me.BarManager1.Form = Me
		Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiInfo, Me.bbiSearch, Me.bbiClear, Me.bbiPrint, Me.bbiExport})
		Me.BarManager1.MaxItemId = 9
		Me.BarManager1.StatusBar = Me.Bar3
		'
		'Bar3
		'
		Me.Bar3.BarName = "Statusleiste"
		Me.Bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
		Me.Bar3.DockCol = 0
		Me.Bar3.DockRow = 0
		Me.Bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
		Me.Bar3.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiInfo), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiSearch), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiClear), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiPrint, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiExport, True)})
		Me.Bar3.OptionsBar.AllowQuickCustomization = False
		Me.Bar3.OptionsBar.DrawDragBorder = False
		Me.Bar3.OptionsBar.UseWholeRow = True
		Me.Bar3.Text = "Statusleiste"
		'
		'bsiInfo
		'
		Me.bsiInfo.AutoSize = DevExpress.XtraBars.BarStaticItemSize.None
		Me.bsiInfo.Caption = "Bereit"
		Me.bsiInfo.Id = 0
		Me.bsiInfo.Name = "bsiInfo"
		Me.bsiInfo.Size = New System.Drawing.Size(300, 0)
		Me.bsiInfo.Width = 300
		'
		'bbiSearch
		'
		Me.bbiSearch.Caption = "Suchen"
		Me.bbiSearch.Id = 1
		Me.bbiSearch.ImageOptions.Image = CType(resources.GetObject("bbiSearch.ImageOptions.Image"), System.Drawing.Image)
		Me.bbiSearch.ImageOptions.LargeImage = CType(resources.GetObject("bbiSearch.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.bbiSearch.Name = "bbiSearch"
		Me.bbiSearch.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		Me.bbiSearch.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText
		'
		'bbiClear
		'
		Me.bbiClear.Caption = "Felder leeren"
		Me.bbiClear.Id = 2
		Me.bbiClear.ImageOptions.Image = CType(resources.GetObject("bbiClear.ImageOptions.Image"), System.Drawing.Image)
		Me.bbiClear.ImageOptions.LargeImage = CType(resources.GetObject("bbiClear.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.bbiClear.Name = "bbiClear"
		Me.bbiClear.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiPrint
		'
		Me.bbiPrint.Caption = "Drucken"
		Me.bbiPrint.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Right
		Me.bbiPrint.Id = 7
		Me.bbiPrint.ImageOptions.Image = CType(resources.GetObject("bbiPrint.ImageOptions.Image"), System.Drawing.Image)
		Me.bbiPrint.Name = "bbiPrint"
		Me.bbiPrint.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiExport
		'
		Me.bbiExport.Caption = "Exportieren"
		Me.bbiExport.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Right
		Me.bbiExport.Id = 8
		Me.bbiExport.ImageOptions.Image = CType(resources.GetObject("bbiExport.ImageOptions.Image"), System.Drawing.Image)
		Me.bbiExport.Name = "bbiExport"
		Me.bbiExport.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'barDockControlTop
		'
		Me.barDockControlTop.CausesValidation = False
		Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
		Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlTop.Manager = Me.BarManager1
		Me.barDockControlTop.Size = New System.Drawing.Size(720, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 695)
		Me.barDockControlBottom.Manager = Me.BarManager1
		Me.barDockControlBottom.Size = New System.Drawing.Size(720, 26)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Manager = Me.BarManager1
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 695)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(720, 0)
		Me.barDockControlRight.Manager = Me.BarManager1
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 695)
		'
		'frmOPSearch
		'
		Me.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
		Me.Appearance.Options.UseBackColor = True
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(720, 721)
		Me.Controls.Add(Me.PanelControl1)
		Me.Controls.Add(Me.xtabOPSearch)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.IconOptions.Icon = CType(resources.GetObject("frmOPSearch.IconOptions.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.MaximumSize = New System.Drawing.Size(722, 753)
		Me.MinimumSize = New System.Drawing.Size(722, 753)
		Me.Name = "frmOPSearch"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Erweiterte Suche nach Debitoren"
		CType(Me.txt_SQLQuery.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txt_IndSQLQuery.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.deMahnDate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.deMahnDate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.cbo_Mahnstufe.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.deCreated_2.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.deCreated_2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.deCreated_1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.deCreated_1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_ErstellPeriode.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.CboSort.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txt_OpenBetrag_2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txt_OpenBetrag_1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_KST1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_KST2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_ListingArt.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_Berater.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.deFakDate_2.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.deFakDate_2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_FakPeriode.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.deFakDate_1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.deFakDate_1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_Filiale.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkKDKreditlimiteUeberschritten.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.cbo_ESBranche.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_ESEinstufung.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.cbo_ESRBank.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.xtabOPSearch, System.ComponentModel.ISupportInitialize).EndInit()
		Me.xtabOPSearch.ResumeLayout(False)
		Me.xtabAllgemein.ResumeLayout(False)
		Me.xtabAllgemein.PerformLayout()
		CType(Me.grpInvoiceDate, System.ComponentModel.ISupportInitialize).EndInit()
		Me.grpInvoiceDate.ResumeLayout(False)
		CType(Me.deStichtag.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.deStichtag.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.grpKostenteilung, System.ComponentModel.ISupportInitialize).EndInit()
		Me.grpKostenteilung.ResumeLayout(False)
		CType(Me.chkValutaFromCreatedOn.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtKDNr_1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtOPNr_2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtKDNr_2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtOPNr_1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtVGNr_1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtVGNr_2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.xtabSonstiges.ResumeLayout(False)
		CType(Me.grpDebitorenArt, System.ComponentModel.ISupportInitialize).EndInit()
		Me.grpDebitorenArt.ResumeLayout(False)
		CType(Me.lueDebitorenart.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.grpMahnArt, System.ComponentModel.ISupportInitialize).EndInit()
		Me.grpMahnArt.ResumeLayout(False)
		CType(Me.luePaymentReminderCode.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.grpEinstufung, System.ComponentModel.ISupportInitialize).EndInit()
		Me.grpEinstufung.ResumeLayout(False)
		CType(Me.grpESRAngaben, System.ComponentModel.ISupportInitialize).EndInit()
		Me.grpESRAngaben.ResumeLayout(False)
		CType(Me.grpPrintSetting, System.ComponentModel.ISupportInitialize).EndInit()
		Me.grpPrintSetting.ResumeLayout(False)
		CType(Me.chkHideKreditInfoLine.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkHideOPInfoLine.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkHideRefNrLine.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.xtabErweitert.ResumeLayout(False)
		Me.xtabErweitert.PerformLayout()
		Me.xtabSQLAbfrage.ResumeLayout(False)
		Me.xtabSQLAbfrage.PerformLayout()
		Me.xtabAdmin.ResumeLayout(False)
		Me.xtabAdmin.PerformLayout()
		CType(Me.txtAdminQuery.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.PanelControl1.ResumeLayout(False)
		Me.PanelControl1.PerformLayout()
		CType(Me.PictureEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents lblheader2 As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblheader1 As DevExpress.XtraEditors.LabelControl
	Friend WithEvents Label14 As DevExpress.XtraEditors.LabelControl
	Friend WithEvents Label15 As DevExpress.XtraEditors.LabelControl
	Friend WithEvents Label16 As DevExpress.XtraEditors.LabelControl
	Friend WithEvents Label17 As DevExpress.XtraEditors.LabelControl
	Friend WithEvents Label18 As DevExpress.XtraEditors.LabelControl
	Friend WithEvents Label19 As DevExpress.XtraEditors.LabelControl
	Friend WithEvents Label20 As DevExpress.XtraEditors.LabelControl
	Friend WithEvents Label21 As DevExpress.XtraEditors.LabelControl
	Friend WithEvents Label22 As DevExpress.XtraEditors.LabelControl
	Friend WithEvents Label23 As DevExpress.XtraEditors.LabelControl
	Friend WithEvents Label24 As DevExpress.XtraEditors.LabelControl
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
	Friend WithEvents CmdClose As DevExpress.XtraEditors.SimpleButton       ' System.Windows.Forms.Button
	Friend WithEvents lblderzeitigeabfrage As DevExpress.XtraEditors.LabelControl
	Friend WithEvents txt_SQLQuery As DevExpress.XtraEditors.MemoEdit	 ' System.Windows.Forms.TextBox
	Friend WithEvents lblAbfrage As DevExpress.XtraEditors.LabelControl
	Friend WithEvents txt_IndSQLQuery As DevExpress.XtraEditors.MemoEdit	' System.Windows.Forms.TextBox
	Friend WithEvents Cbo_Filiale As DevExpress.XtraEditors.ComboBoxEdit ' SPOPSearch.myCbo
	Friend WithEvents lblfiliale As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lbl2kst As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblart As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblfakturaperiode As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblmahncode As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblfakturazwischen As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblverfalltage As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblsortierung As DevExpress.XtraEditors.LabelControl
	Friend WithEvents Cbo_FakPeriode As DevExpress.XtraEditors.ComboBoxEdit ' SPOPSearch.myCbo
	Friend WithEvents Cbo_Berater As DevExpress.XtraEditors.ComboBoxEdit ' SPOPSearch.myCbo
	Friend WithEvents lblberater As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lbllistenart As DevExpress.XtraEditors.LabelControl
	Friend WithEvents Cbo_ListingArt As DevExpress.XtraEditors.ComboBoxEdit ' System.Windows.Forms.ComboBox
	Friend WithEvents Cbo_ESEinstufung As DevExpress.XtraEditors.ComboBoxEdit ' SPOPSearch.myCbo
	Friend WithEvents lbleinstufung As DevExpress.XtraEditors.LabelControl
	Friend WithEvents Cbo_KST1 As DevExpress.XtraEditors.ComboBoxEdit ' SPOPSearch.myCbo
	Friend WithEvents Cbo_KST2 As DevExpress.XtraEditors.ComboBoxEdit ' SPOPSearch.myCbo
	Friend WithEvents lbl1kst As DevExpress.XtraEditors.LabelControl
	Friend WithEvents txt_OpenBetrag_2 As DevExpress.XtraEditors.TextEdit  ' System.Windows.Forms.TextBox
	Friend WithEvents lblOffenerbetrag As DevExpress.XtraEditors.LabelControl
	Friend WithEvents txt_OpenBetrag_1 As DevExpress.XtraEditors.TextEdit  ' System.Windows.Forms.TextBox
	Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
	Friend WithEvents CboSort As DevExpress.XtraEditors.CheckedComboBoxEdit ' SPOPSearch.myCbo
	Friend WithEvents lblnummer As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblkundennr As DevExpress.XtraEditors.LabelControl
	Friend WithEvents chkKDKreditlimiteUeberschritten As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents lblerstellungzwischen As DevExpress.XtraEditors.LabelControl
	Friend WithEvents Cbo_ErstellPeriode As DevExpress.XtraEditors.ComboBoxEdit ' SPOPSearch.myCbo
	Friend WithEvents lblErstellperiode As DevExpress.XtraEditors.LabelControl
	Friend WithEvents cbo_Mahnstufe As DevExpress.XtraEditors.ComboBoxEdit ' SPOPSearch.myCbo
	Friend WithEvents lblmahnstufe As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblmahndatum As DevExpress.XtraEditors.LabelControl
	Friend WithEvents deMahnDate As DevExpress.XtraEditors.DateEdit
	Friend WithEvents cbo_ESBranche As DevExpress.XtraEditors.ComboBoxEdit ' SPOPSearch.myCbo
	Friend WithEvents lblBranche As DevExpress.XtraEditors.LabelControl
	Friend WithEvents cbo_ESRBank As DevExpress.XtraEditors.ComboBoxEdit ' SPOPSearch.myCbo
	Friend WithEvents lblESBank As DevExpress.XtraEditors.LabelControl
	Friend WithEvents xtabOPSearch As DevExpress.XtraTab.XtraTabControl
	Friend WithEvents xtabAllgemein As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents xtabSonstiges As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents xtabErweitert As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents xtabSQLAbfrage As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents xtabAdmin As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents lbladmin As DevExpress.XtraEditors.LabelControl
	Friend WithEvents txtAdminQuery As DevExpress.XtraEditors.MemoEdit  ' System.Windows.Forms.TextBox
	Friend WithEvents lueMandant As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents lblMDName As DevExpress.XtraEditors.LabelControl
	Friend WithEvents deCreated_2 As DevExpress.XtraEditors.DateEdit
	Friend WithEvents deCreated_1 As DevExpress.XtraEditors.DateEdit
	Friend WithEvents deFakDate_2 As DevExpress.XtraEditors.DateEdit
	Friend WithEvents deFakDate_1 As DevExpress.XtraEditors.DateEdit
	Friend WithEvents SwitchButton2 As DevComponents.DotNetBar.Controls.SwitchButton
	Friend WithEvents SwitchButton1 As DevComponents.DotNetBar.Controls.SwitchButton
	Friend WithEvents chkValutaFromCreatedOn As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents txtKDNr_1 As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents txtOPNr_2 As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents txtKDNr_2 As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents txtOPNr_1 As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
	Friend WithEvents PictureEdit1 As DevExpress.XtraEditors.PictureEdit
	Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
	Friend WithEvents Bar3 As DevExpress.XtraBars.Bar
	Friend WithEvents bsiInfo As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents bbiSearch As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbiClear As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
	Friend WithEvents bbiPrint As DevExpress.XtraBars.BarLargeButtonItem
	Friend WithEvents bbiExport As DevExpress.XtraBars.BarLargeButtonItem
	Friend WithEvents chkHideOPInfoLine As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chkHideRefNrLine As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chkHideKreditInfoLine As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents txtVGNr_1 As DevExpress.XtraEditors.SpinEdit
	Friend WithEvents txtVGNr_2 As DevExpress.XtraEditors.SpinEdit
	Friend WithEvents grpInvoiceDate As DevExpress.XtraEditors.GroupControl
	Friend WithEvents lblStichtag As DevExpress.XtraEditors.LabelControl
	Friend WithEvents deStichtag As DevExpress.XtraEditors.DateEdit
	Friend WithEvents grpKostenteilung As DevExpress.XtraEditors.GroupControl
	Friend WithEvents grpMahnArt As DevExpress.XtraEditors.GroupControl
	Friend WithEvents lblCreateDate As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblInvoiceDate As DevExpress.XtraEditors.LabelControl
	Friend WithEvents grpEinstufung As DevExpress.XtraEditors.GroupControl
	Friend WithEvents grpESRAngaben As DevExpress.XtraEditors.GroupControl
	Friend WithEvents grpPrintSetting As DevExpress.XtraEditors.GroupControl
	Friend WithEvents grpDebitorenArt As DevExpress.XtraEditors.GroupControl
	Friend WithEvents luePaymentReminderCode As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents lueDebitorenart As DevExpress.XtraEditors.LookUpEdit
End Class
