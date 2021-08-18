<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLOAGSearch
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLOAGSearch))
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
		Me.pnl_Berufe = New System.Windows.Forms.Panel()
		Me.Cbo_1Kategorie = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.lbl_BerufeHeader = New System.Windows.Forms.Label()
		Me.cbo_Beruf = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.lblBeruf = New System.Windows.Forms.Label()
		Me.lbl1Kategorie = New System.Windows.Forms.Label()
		Me.pnl_Nummerfelder = New System.Windows.Forms.Panel()
		Me.lblKandidatenname = New DevExpress.XtraEditors.LabelControl()
		Me.lbl_NummerfelderHeader = New System.Windows.Forms.Label()
		Me.txt_MANr = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.lblMonatVon = New System.Windows.Forms.Label()
		Me.ChkNullBetrag = New System.Windows.Forms.CheckBox()
		Me.lblArtderListe = New System.Windows.Forms.Label()
		Me.lblJahrVon = New System.Windows.Forms.Label()
		Me.lblLohnarten = New System.Windows.Forms.Label()
		Me.lblPeriode = New System.Windows.Forms.Label()
		Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
		Me.XtraTabControl1 = New DevExpress.XtraTab.XtraTabControl()
		Me.XtraTabPage3 = New DevExpress.XtraTab.XtraTabPage()
		Me.Cbo_JahrBis = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.Cbo_Lohnart = New DevExpress.XtraEditors.CheckedComboBoxEdit()
		Me.Cbo_MonatBis = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.Cbo_JahrVon = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.Cbo_Periode = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.Cbo_MonatVon = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.SwitchButton2 = New DevComponents.DotNetBar.Controls.SwitchButton()
		Me.SwitchButton1 = New DevComponents.DotNetBar.Controls.SwitchButton()
		Me.XtraTabPage2 = New DevExpress.XtraTab.XtraTabPage()
		Me.txt_SQLQuery = New System.Windows.Forms.TextBox()
		Me.Label25 = New System.Windows.Forms.Label()
		Me.Cbo_ArtderListe = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.lueMandant = New DevExpress.XtraEditors.LookUpEdit()
		Me.lblMDName = New System.Windows.Forms.Label()
		Me.GroupBox1 = New DevExpress.XtraEditors.PanelControl()
		Me.CmdClose = New DevExpress.XtraEditors.SimpleButton()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.LblTimeValue = New System.Windows.Forms.Label()
		Me.lblHeaderNormal = New System.Windows.Forms.Label()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
		Me.Bar4 = New DevExpress.XtraBars.Bar()
		Me.bsiInfo = New DevExpress.XtraBars.BarStaticItem()
		Me.bbiSearch = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiClearFields = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiPrint = New DevExpress.XtraBars.BarLargeButtonItem()
		Me.bbiExport = New DevExpress.XtraBars.BarLargeButtonItem()
		Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
		Me.BarStaticItem1 = New DevExpress.XtraBars.BarStaticItem()
		Me.pnl_Berufe.SuspendLayout()
		CType(Me.Cbo_1Kategorie.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.cbo_Beruf.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.pnl_Nummerfelder.SuspendLayout()
		CType(Me.txt_MANr.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.XtraTabControl1.SuspendLayout()
		Me.XtraTabPage3.SuspendLayout()
		CType(Me.Cbo_JahrBis.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_Lohnart.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_MonatBis.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_JahrVon.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_Periode.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_MonatVon.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.XtraTabPage2.SuspendLayout()
		CType(Me.Cbo_ArtderListe.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupBox1.SuspendLayout()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'lblHeaderFett
		'
		Me.lblHeaderFett.AutoSize = True
		Me.lblHeaderFett.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblHeaderFett.Location = New System.Drawing.Point(94, 21)
		Me.lblHeaderFett.Name = "lblHeaderFett"
		Me.lblHeaderFett.Size = New System.Drawing.Size(289, 13)
		Me.lblHeaderFett.TabIndex = 0
		Me.lblHeaderFett.Text = "Liste der Arbeitgeberlohnarten und Rekapitulation"
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
		'pnl_Berufe
		'
		Me.pnl_Berufe.Controls.Add(Me.Cbo_1Kategorie)
		Me.pnl_Berufe.Controls.Add(Me.lbl_BerufeHeader)
		Me.pnl_Berufe.Controls.Add(Me.cbo_Beruf)
		Me.pnl_Berufe.Controls.Add(Me.lblBeruf)
		Me.pnl_Berufe.Controls.Add(Me.lbl1Kategorie)
		Me.pnl_Berufe.Location = New System.Drawing.Point(15, 245)
		Me.pnl_Berufe.Name = "pnl_Berufe"
		Me.pnl_Berufe.Size = New System.Drawing.Size(550, 82)
		Me.pnl_Berufe.TabIndex = 292
		'
		'Cbo_1Kategorie
		'
		Me.Cbo_1Kategorie.Location = New System.Drawing.Point(124, 53)
		Me.Cbo_1Kategorie.Name = "Cbo_1Kategorie"
		Me.Cbo_1Kategorie.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_1Kategorie.Properties.DropDownRows = 10
		Me.Cbo_1Kategorie.Properties.PopupResizeMode = DevExpress.XtraEditors.Controls.ResizeMode.LiveResize
		Me.Cbo_1Kategorie.Properties.PopupSizeable = True
		Me.Cbo_1Kategorie.Size = New System.Drawing.Size(384, 20)
		Me.Cbo_1Kategorie.TabIndex = 298
		'
		'lbl_BerufeHeader
		'
		Me.lbl_BerufeHeader.BackColor = System.Drawing.Color.Transparent
		Me.lbl_BerufeHeader.Cursor = System.Windows.Forms.Cursors.Hand
		Me.lbl_BerufeHeader.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lbl_BerufeHeader.ForeColor = System.Drawing.Color.Gray
		Me.lbl_BerufeHeader.Image = CType(resources.GetObject("lbl_BerufeHeader.Image"), System.Drawing.Image)
		Me.lbl_BerufeHeader.ImageAlign = System.Drawing.ContentAlignment.TopRight
		Me.lbl_BerufeHeader.Location = New System.Drawing.Point(0, 0)
		Me.lbl_BerufeHeader.Name = "lbl_BerufeHeader"
		Me.lbl_BerufeHeader.Size = New System.Drawing.Size(550, 14)
		Me.lbl_BerufeHeader.TabIndex = 293
		Me.lbl_BerufeHeader.Text = "Berufe                                                                           " &
		"                                                                                " &
		"               "
		'
		'cbo_Beruf
		'
		Me.cbo_Beruf.Location = New System.Drawing.Point(124, 27)
		Me.cbo_Beruf.Name = "cbo_Beruf"
		Me.cbo_Beruf.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.cbo_Beruf.Properties.DropDownRows = 10
		Me.cbo_Beruf.Properties.PopupResizeMode = DevExpress.XtraEditors.Controls.ResizeMode.LiveResize
		Me.cbo_Beruf.Properties.PopupSizeable = True
		Me.cbo_Beruf.Size = New System.Drawing.Size(384, 20)
		Me.cbo_Beruf.TabIndex = 297
		'
		'lblBeruf
		'
		Me.lblBeruf.Location = New System.Drawing.Point(3, 30)
		Me.lblBeruf.Name = "lblBeruf"
		Me.lblBeruf.Size = New System.Drawing.Size(115, 13)
		Me.lblBeruf.TabIndex = 273
		Me.lblBeruf.Text = "Beruf"
		Me.lblBeruf.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lbl1Kategorie
		'
		Me.lbl1Kategorie.Location = New System.Drawing.Point(3, 57)
		Me.lbl1Kategorie.Name = "lbl1Kategorie"
		Me.lbl1Kategorie.Size = New System.Drawing.Size(115, 13)
		Me.lbl1Kategorie.TabIndex = 275
		Me.lbl1Kategorie.Text = "1. Kategorie"
		Me.lbl1Kategorie.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'pnl_Nummerfelder
		'
		Me.pnl_Nummerfelder.Controls.Add(Me.lblKandidatenname)
		Me.pnl_Nummerfelder.Controls.Add(Me.lbl_NummerfelderHeader)
		Me.pnl_Nummerfelder.Controls.Add(Me.txt_MANr)
		Me.pnl_Nummerfelder.Location = New System.Drawing.Point(15, 167)
		Me.pnl_Nummerfelder.Name = "pnl_Nummerfelder"
		Me.pnl_Nummerfelder.Size = New System.Drawing.Size(550, 61)
		Me.pnl_Nummerfelder.TabIndex = 289
		'
		'lblKandidatenname
		'
		Me.lblKandidatenname.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblKandidatenname.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblKandidatenname.Location = New System.Drawing.Point(6, 29)
		Me.lblKandidatenname.Name = "lblKandidatenname"
		Me.lblKandidatenname.Size = New System.Drawing.Size(112, 13)
		Me.lblKandidatenname.TabIndex = 290
		Me.lblKandidatenname.TabStop = True
		Me.lblKandidatenname.Text = "Kandidaten-Nr."
		'
		'lbl_NummerfelderHeader
		'
		Me.lbl_NummerfelderHeader.BackColor = System.Drawing.Color.Transparent
		Me.lbl_NummerfelderHeader.Cursor = System.Windows.Forms.Cursors.Hand
		Me.lbl_NummerfelderHeader.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lbl_NummerfelderHeader.ForeColor = System.Drawing.Color.Gray
		Me.lbl_NummerfelderHeader.Image = CType(resources.GetObject("lbl_NummerfelderHeader.Image"), System.Drawing.Image)
		Me.lbl_NummerfelderHeader.ImageAlign = System.Drawing.ContentAlignment.TopRight
		Me.lbl_NummerfelderHeader.Location = New System.Drawing.Point(0, 0)
		Me.lbl_NummerfelderHeader.Name = "lbl_NummerfelderHeader"
		Me.lbl_NummerfelderHeader.Size = New System.Drawing.Size(550, 14)
		Me.lbl_NummerfelderHeader.TabIndex = 288
		Me.lbl_NummerfelderHeader.Text = "Kandidatensuche                                                                  " &
		"                                                                                " &
		"                        "
		'
		'txt_MANr
		'
		Me.txt_MANr.Location = New System.Drawing.Point(124, 26)
		Me.txt_MANr.Name = "txt_MANr"
		Me.txt_MANr.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.txt_MANr.Size = New System.Drawing.Size(384, 20)
		Me.txt_MANr.TabIndex = 289
		'
		'lblMonatVon
		'
		Me.lblMonatVon.Location = New System.Drawing.Point(15, 52)
		Me.lblMonatVon.Name = "lblMonatVon"
		Me.lblMonatVon.Size = New System.Drawing.Size(118, 13)
		Me.lblMonatVon.TabIndex = 254
		Me.lblMonatVon.Text = "Monat von"
		Me.lblMonatVon.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'ChkNullBetrag
		'
		Me.ChkNullBetrag.AutoSize = True
		Me.ChkNullBetrag.Checked = True
		Me.ChkNullBetrag.CheckState = System.Windows.Forms.CheckState.Checked
		Me.ChkNullBetrag.Location = New System.Drawing.Point(139, 142)
		Me.ChkNullBetrag.Name = "ChkNullBetrag"
		Me.ChkNullBetrag.Size = New System.Drawing.Size(195, 17)
		Me.ChkNullBetrag.TabIndex = 130
		Me.ChkNullBetrag.Text = "Lohnarten mit 0-Betrag ausblenden"
		Me.ChkNullBetrag.UseVisualStyleBackColor = True
		'
		'lblArtderListe
		'
		Me.lblArtderListe.Location = New System.Drawing.Point(16, 126)
		Me.lblArtderListe.Name = "lblArtderListe"
		Me.lblArtderListe.Size = New System.Drawing.Size(133, 13)
		Me.lblArtderListe.TabIndex = 277
		Me.lblArtderListe.Text = "Art der Liste"
		Me.lblArtderListe.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblJahrVon
		'
		Me.lblJahrVon.Location = New System.Drawing.Point(15, 79)
		Me.lblJahrVon.Name = "lblJahrVon"
		Me.lblJahrVon.Size = New System.Drawing.Size(118, 13)
		Me.lblJahrVon.TabIndex = 258
		Me.lblJahrVon.Text = "Jahr von"
		Me.lblJahrVon.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblLohnarten
		'
		Me.lblLohnarten.Location = New System.Drawing.Point(15, 119)
		Me.lblLohnarten.Name = "lblLohnarten"
		Me.lblLohnarten.Size = New System.Drawing.Size(118, 13)
		Me.lblLohnarten.TabIndex = 261
		Me.lblLohnarten.Text = "Lohnart"
		Me.lblLohnarten.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblPeriode
		'
		Me.lblPeriode.Location = New System.Drawing.Point(15, 25)
		Me.lblPeriode.Name = "lblPeriode"
		Me.lblPeriode.Size = New System.Drawing.Size(118, 13)
		Me.lblPeriode.TabIndex = 286
		Me.lblPeriode.Text = "Periode"
		Me.lblPeriode.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'BackgroundWorker1
		'
		'
		'XtraTabControl1
		'
		Me.XtraTabControl1.Location = New System.Drawing.Point(15, 155)
		Me.XtraTabControl1.Name = "XtraTabControl1"
		Me.XtraTabControl1.SelectedTabPage = Me.XtraTabPage3
		Me.XtraTabControl1.Size = New System.Drawing.Size(590, 374)
		Me.XtraTabControl1.TabIndex = 202
		Me.XtraTabControl1.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.XtraTabPage3, Me.XtraTabPage2})
		'
		'XtraTabPage3
		'
		Me.XtraTabPage3.Controls.Add(Me.Cbo_JahrBis)
		Me.XtraTabPage3.Controls.Add(Me.Cbo_Lohnart)
		Me.XtraTabPage3.Controls.Add(Me.Cbo_MonatBis)
		Me.XtraTabPage3.Controls.Add(Me.Cbo_JahrVon)
		Me.XtraTabPage3.Controls.Add(Me.Cbo_Periode)
		Me.XtraTabPage3.Controls.Add(Me.Cbo_MonatVon)
		Me.XtraTabPage3.Controls.Add(Me.pnl_Berufe)
		Me.XtraTabPage3.Controls.Add(Me.SwitchButton2)
		Me.XtraTabPage3.Controls.Add(Me.lblPeriode)
		Me.XtraTabPage3.Controls.Add(Me.SwitchButton1)
		Me.XtraTabPage3.Controls.Add(Me.lblLohnarten)
		Me.XtraTabPage3.Controls.Add(Me.pnl_Nummerfelder)
		Me.XtraTabPage3.Controls.Add(Me.lblMonatVon)
		Me.XtraTabPage3.Controls.Add(Me.ChkNullBetrag)
		Me.XtraTabPage3.Controls.Add(Me.lblJahrVon)
		Me.XtraTabPage3.Name = "XtraTabPage3"
		Me.XtraTabPage3.Size = New System.Drawing.Size(584, 346)
		Me.XtraTabPage3.Text = "Allgemein"
		'
		'Cbo_JahrBis
		'
		Me.Cbo_JahrBis.Location = New System.Drawing.Point(427, 75)
		Me.Cbo_JahrBis.Name = "Cbo_JahrBis"
		Me.Cbo_JahrBis.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_JahrBis.Properties.DropDownRows = 10
		Me.Cbo_JahrBis.Properties.PopupResizeMode = DevExpress.XtraEditors.Controls.ResizeMode.LiveResize
		Me.Cbo_JahrBis.Properties.PopupSizeable = True
		Me.Cbo_JahrBis.Size = New System.Drawing.Size(96, 20)
		Me.Cbo_JahrBis.TabIndex = 300
		Me.Cbo_JahrBis.Visible = False
		'
		'Cbo_Lohnart
		'
		Me.Cbo_Lohnart.Location = New System.Drawing.Point(139, 116)
		Me.Cbo_Lohnart.Name = "Cbo_Lohnart"
		Me.Cbo_Lohnart.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_Lohnart.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
		Me.Cbo_Lohnart.Size = New System.Drawing.Size(384, 20)
		Me.Cbo_Lohnart.TabIndex = 297
		'
		'Cbo_MonatBis
		'
		Me.Cbo_MonatBis.Location = New System.Drawing.Point(427, 49)
		Me.Cbo_MonatBis.Name = "Cbo_MonatBis"
		Me.Cbo_MonatBis.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_MonatBis.Properties.DropDownRows = 12
		Me.Cbo_MonatBis.Properties.PopupResizeMode = DevExpress.XtraEditors.Controls.ResizeMode.LiveResize
		Me.Cbo_MonatBis.Properties.PopupSizeable = True
		Me.Cbo_MonatBis.Size = New System.Drawing.Size(96, 20)
		Me.Cbo_MonatBis.TabIndex = 298
		Me.Cbo_MonatBis.Visible = False
		'
		'Cbo_JahrVon
		'
		Me.Cbo_JahrVon.Location = New System.Drawing.Point(139, 75)
		Me.Cbo_JahrVon.Name = "Cbo_JahrVon"
		Me.Cbo_JahrVon.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_JahrVon.Properties.DropDownRows = 10
		Me.Cbo_JahrVon.Properties.PopupResizeMode = DevExpress.XtraEditors.Controls.ResizeMode.LiveResize
		Me.Cbo_JahrVon.Properties.PopupSizeable = True
		Me.Cbo_JahrVon.Size = New System.Drawing.Size(96, 20)
		Me.Cbo_JahrVon.TabIndex = 299
		'
		'Cbo_Periode
		'
		Me.Cbo_Periode.Location = New System.Drawing.Point(139, 22)
		Me.Cbo_Periode.Name = "Cbo_Periode"
		Me.Cbo_Periode.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_Periode.Properties.DropDownRows = 10
		Me.Cbo_Periode.Properties.PopupResizeMode = DevExpress.XtraEditors.Controls.ResizeMode.LiveResize
		Me.Cbo_Periode.Properties.PopupSizeable = True
		Me.Cbo_Periode.Size = New System.Drawing.Size(384, 20)
		Me.Cbo_Periode.TabIndex = 295
		'
		'Cbo_MonatVon
		'
		Me.Cbo_MonatVon.Location = New System.Drawing.Point(139, 49)
		Me.Cbo_MonatVon.Name = "Cbo_MonatVon"
		Me.Cbo_MonatVon.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_MonatVon.Properties.DropDownRows = 12
		Me.Cbo_MonatVon.Properties.PopupResizeMode = DevExpress.XtraEditors.Controls.ResizeMode.LiveResize
		Me.Cbo_MonatVon.Properties.PopupSizeable = True
		Me.Cbo_MonatVon.Size = New System.Drawing.Size(96, 20)
		Me.Cbo_MonatVon.TabIndex = 297
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
		Me.SwitchButton2.Location = New System.Drawing.Point(383, 77)
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
		Me.SwitchButton2.TabIndex = 297
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
		Me.SwitchButton1.Location = New System.Drawing.Point(383, 50)
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
		Me.SwitchButton1.TabIndex = 296
		'
		'XtraTabPage2
		'
		Me.XtraTabPage2.Controls.Add(Me.txt_SQLQuery)
		Me.XtraTabPage2.Controls.Add(Me.Label25)
		Me.XtraTabPage2.Name = "XtraTabPage2"
		Me.XtraTabPage2.Size = New System.Drawing.Size(584, 346)
		Me.XtraTabPage2.Text = "SQL- Abfrage"
		'
		'txt_SQLQuery
		'
		Me.txt_SQLQuery.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txt_SQLQuery.Location = New System.Drawing.Point(21, 34)
		Me.txt_SQLQuery.Multiline = True
		Me.txt_SQLQuery.Name = "txt_SQLQuery"
		Me.txt_SQLQuery.Size = New System.Drawing.Size(543, 353)
		Me.txt_SQLQuery.TabIndex = 101
		'
		'Label25
		'
		Me.Label25.AutoSize = True
		Me.Label25.Location = New System.Drawing.Point(18, 18)
		Me.Label25.Name = "Label25"
		Me.Label25.Size = New System.Drawing.Size(154, 13)
		Me.Label25.TabIndex = 105
		Me.Label25.Text = "Ihre derzeitige Abfrage lautet:"
		'
		'Cbo_ArtderListe
		'
		Me.Cbo_ArtderListe.Location = New System.Drawing.Point(155, 122)
		Me.Cbo_ArtderListe.Name = "Cbo_ArtderListe"
		Me.Cbo_ArtderListe.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_ArtderListe.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
		Me.Cbo_ArtderListe.Size = New System.Drawing.Size(384, 20)
		Me.Cbo_ArtderListe.TabIndex = 296
		'
		'lueMandant
		'
		Me.lueMandant.Location = New System.Drawing.Point(155, 96)
		Me.lueMandant.Name = "lueMandant"
		Me.lueMandant.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.lueMandant.Properties.NullText = ""
		Me.lueMandant.Size = New System.Drawing.Size(384, 20)
		Me.lueMandant.TabIndex = 203
		'
		'lblMDName
		'
		Me.lblMDName.Location = New System.Drawing.Point(16, 100)
		Me.lblMDName.Name = "lblMDName"
		Me.lblMDName.Size = New System.Drawing.Size(133, 13)
		Me.lblMDName.TabIndex = 204
		Me.lblMDName.Text = "Mandanten"
		Me.lblMDName.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'GroupBox1
		'
		Me.GroupBox1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003
		Me.GroupBox1.Controls.Add(Me.CmdClose)
		Me.GroupBox1.Controls.Add(Me.Label2)
		Me.GroupBox1.Controls.Add(Me.LblTimeValue)
		Me.GroupBox1.Controls.Add(Me.lblHeaderNormal)
		Me.GroupBox1.Controls.Add(Me.lblHeaderFett)
		Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
		Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(635, 77)
		Me.GroupBox1.TabIndex = 297
		'
		'CmdClose
		'
		Me.CmdClose.Location = New System.Drawing.Point(500, 21)
		Me.CmdClose.Name = "CmdClose"
		Me.CmdClose.Size = New System.Drawing.Size(100, 25)
		Me.CmdClose.TabIndex = 204
		Me.CmdClose.Text = "Schliessen"
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
		'LblTimeValue
		'
		Me.LblTimeValue.Location = New System.Drawing.Point(292, 61)
		Me.LblTimeValue.Name = "LblTimeValue"
		Me.LblTimeValue.Size = New System.Drawing.Size(308, 13)
		Me.LblTimeValue.TabIndex = 14
		Me.LblTimeValue.Text = "Zeitangaben..."
		Me.LblTimeValue.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.LblTimeValue.Visible = False
		'
		'lblHeaderNormal
		'
		Me.lblHeaderNormal.Location = New System.Drawing.Point(112, 45)
		Me.lblHeaderNormal.Name = "lblHeaderNormal"
		Me.lblHeaderNormal.Size = New System.Drawing.Size(299, 16)
		Me.lblHeaderNormal.TabIndex = 1
		Me.lblHeaderNormal.Text = "Geben Sie bitte Ihre gewünschten Kriterien ein."
		'
		'BarManager1
		'
		Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar4})
		Me.BarManager1.DockControls.Add(Me.barDockControlTop)
		Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
		Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
		Me.BarManager1.DockControls.Add(Me.barDockControlRight)
		Me.BarManager1.Form = Me
		Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiInfo, Me.bbiSearch, Me.bbiPrint, Me.bbiClearFields, Me.BarStaticItem1, Me.bbiExport})
		Me.BarManager1.MaxItemId = 10
		Me.BarManager1.StatusBar = Me.Bar4
		'
		'Bar4
		'
		Me.Bar4.BarName = "Statusleiste"
		Me.Bar4.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
		Me.Bar4.DockCol = 0
		Me.Bar4.DockRow = 0
		Me.Bar4.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
		Me.Bar4.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiInfo), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiSearch), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiClearFields, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiPrint), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiExport, True)})
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
		Me.bsiInfo.TextAlignment = System.Drawing.StringAlignment.Near
		Me.bsiInfo.Width = 32
		'
		'bbiSearch
		'
		Me.bbiSearch.Caption = "Suchen"
		Me.bbiSearch.Glyph = CType(resources.GetObject("bbiSearch.Glyph"), System.Drawing.Image)
		Me.bbiSearch.Id = 1
		Me.bbiSearch.Name = "bbiSearch"
		Me.bbiSearch.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiClearFields
		'
		Me.bbiClearFields.Caption = "Felder leeren"
		Me.bbiClearFields.Glyph = CType(resources.GetObject("bbiClearFields.Glyph"), System.Drawing.Image)
		Me.bbiClearFields.Id = 8
		Me.bbiClearFields.Name = "bbiClearFields"
		Me.bbiClearFields.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiPrint
		'
		Me.bbiPrint.AllowAllUp = True
		Me.bbiPrint.Caption = "Drucken"
		Me.bbiPrint.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Right
		Me.bbiPrint.Glyph = CType(resources.GetObject("bbiPrint.Glyph"), System.Drawing.Image)
		Me.bbiPrint.Id = 6
		Me.bbiPrint.Name = "bbiPrint"
		Me.bbiPrint.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiExport
		'
		Me.bbiExport.Caption = "Exportieren"
		Me.bbiExport.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Right
		Me.bbiExport.Glyph = CType(resources.GetObject("bbiExport.Glyph"), System.Drawing.Image)
		Me.bbiExport.Id = 4
		Me.bbiExport.Name = "bbiExport"
		Me.bbiExport.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'barDockControlTop
		'
		Me.barDockControlTop.CausesValidation = False
		Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
		Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlTop.Size = New System.Drawing.Size(635, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 562)
		Me.barDockControlBottom.Size = New System.Drawing.Size(635, 27)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 562)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(635, 0)
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 562)
		'
		'BarStaticItem1
		'
		Me.BarStaticItem1.Caption = " "
		Me.BarStaticItem1.Id = 5
		Me.BarStaticItem1.Name = "BarStaticItem1"
		Me.BarStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near
		'
		'frmLOAGSearch
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(635, 589)
		Me.Controls.Add(Me.GroupBox1)
		Me.Controls.Add(Me.Cbo_ArtderListe)
		Me.Controls.Add(Me.lueMandant)
		Me.Controls.Add(Me.XtraTabControl1)
		Me.Controls.Add(Me.lblMDName)
		Me.Controls.Add(Me.lblArtderListe)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.Name = "frmLOAGSearch"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Monatliche Lohnlisten"
		Me.pnl_Berufe.ResumeLayout(False)
		CType(Me.Cbo_1Kategorie.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.cbo_Beruf.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pnl_Nummerfelder.ResumeLayout(False)
		CType(Me.txt_MANr.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.XtraTabControl1.ResumeLayout(False)
		Me.XtraTabPage3.ResumeLayout(False)
		Me.XtraTabPage3.PerformLayout()
		CType(Me.Cbo_JahrBis.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_Lohnart.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_MonatBis.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_JahrVon.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_Periode.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_MonatVon.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.XtraTabPage2.ResumeLayout(False)
		Me.XtraTabPage2.PerformLayout()
		CType(Me.Cbo_ArtderListe.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupBox1.ResumeLayout(False)
		Me.GroupBox1.PerformLayout()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
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
	Friend WithEvents lblArtderListe As System.Windows.Forms.Label
	Friend WithEvents lbl1Kategorie As System.Windows.Forms.Label
	Friend WithEvents lblBeruf As System.Windows.Forms.Label
	Friend WithEvents lblLohnarten As System.Windows.Forms.Label
	Friend WithEvents lblJahrVon As System.Windows.Forms.Label
	Friend WithEvents lblMonatVon As System.Windows.Forms.Label
	Friend WithEvents ChkNullBetrag As System.Windows.Forms.CheckBox
	Friend WithEvents lblPeriode As System.Windows.Forms.Label
	Friend WithEvents pnl_Nummerfelder As System.Windows.Forms.Panel
	Friend WithEvents lbl_NummerfelderHeader As System.Windows.Forms.Label
	Friend WithEvents lbl_BerufeHeader As System.Windows.Forms.Label
	Friend WithEvents pnl_Berufe As System.Windows.Forms.Panel
	Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
	Friend WithEvents XtraTabControl1 As DevExpress.XtraTab.XtraTabControl
	Friend WithEvents XtraTabPage3 As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents XtraTabPage2 As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents txt_SQLQuery As System.Windows.Forms.TextBox
	Friend WithEvents Label25 As System.Windows.Forms.Label
	Friend WithEvents lueMandant As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents lblMDName As System.Windows.Forms.Label
	Friend WithEvents SwitchButton2 As DevComponents.DotNetBar.Controls.SwitchButton
	Friend WithEvents SwitchButton1 As DevComponents.DotNetBar.Controls.SwitchButton
	Friend WithEvents Cbo_ArtderListe As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents Cbo_Periode As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents Cbo_Lohnart As DevExpress.XtraEditors.CheckedComboBoxEdit
	Friend WithEvents Cbo_1Kategorie As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents cbo_Beruf As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents Cbo_JahrBis As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents Cbo_MonatBis As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents Cbo_JahrVon As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents Cbo_MonatVon As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents GroupBox1 As DevExpress.XtraEditors.PanelControl
	Friend WithEvents CmdClose As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents Label2 As System.Windows.Forms.Label
	Friend WithEvents LblTimeValue As System.Windows.Forms.Label
	Friend WithEvents lblHeaderNormal As System.Windows.Forms.Label
	Friend WithEvents txt_MANr As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents lblKandidatenname As DevExpress.XtraEditors.LabelControl
	Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
	Friend WithEvents Bar4 As DevExpress.XtraBars.Bar
	Friend WithEvents bsiInfo As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents bbiSearch As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbiClearFields As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbiPrint As DevExpress.XtraBars.BarLargeButtonItem
	Friend WithEvents bbiExport As DevExpress.XtraBars.BarLargeButtonItem
	Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
	Friend WithEvents BarStaticItem1 As DevExpress.XtraBars.BarStaticItem

End Class
