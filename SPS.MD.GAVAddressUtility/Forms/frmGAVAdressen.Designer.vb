<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGAVAdressen
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmGAVAdressen))
		Me.lblHeader2 = New System.Windows.Forms.Label()
		Me.lblHeader1 = New System.Windows.Forms.Label()
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
		Me.Timer1 = New System.Windows.Forms.Timer()
		Me.lblbankname = New System.Windows.Forms.Label()
		Me.lblpckonto = New System.Windows.Forms.Label()
		Me.lblbetriebsnummer = New System.Windows.Forms.Label()
		Me.lblzhd = New System.Windows.Forms.Label()
		Me.txtGAV_Postfach = New DevExpress.XtraEditors.TextEdit()
		Me.txtGAV_ZHD = New DevExpress.XtraEditors.TextEdit()
		Me.lblpostfach = New System.Windows.Forms.Label()
		Me.txtGAV_Strasse = New DevExpress.XtraEditors.TextEdit()
		Me.lblstrasse = New System.Windows.Forms.Label()
		Me.txtGAV_PLZ = New DevExpress.XtraEditors.TextEdit()
		Me.lblplzort = New System.Windows.Forms.Label()
		Me.txtGAV_Ort = New DevExpress.XtraEditors.TextEdit()
		Me.txtGAV_Bank = New DevExpress.XtraEditors.TextEdit()
		Me.txtGAV_IBAN = New DevExpress.XtraEditors.TextEdit()
		Me.lbliban = New System.Windows.Forms.Label()
		Me.txtGAV_BankKonto = New DevExpress.XtraEditors.TextEdit()
		Me.txtRecNr = New DevExpress.XtraEditors.TextEdit()
		Me.txtGAV_BankPLZOrt = New DevExpress.XtraEditors.TextEdit()
		Me.lblNummer = New System.Windows.Forms.Label()
		Me.lblbankplzort = New System.Windows.Forms.Label()
		Me.txtGAV_AdressNr = New DevExpress.XtraEditors.TextEdit()
		Me.lblname = New System.Windows.Forms.Label()
		Me.txtGAV_Name = New DevExpress.XtraEditors.TextEdit()
		Me.lvAdressen = New DevComponents.DotNetBar.Controls.ListViewEx()
		Me.btnAdrIdInsert = New DevExpress.XtraEditors.SimpleButton()
		Me.btnAdrIdCopy = New DevExpress.XtraEditors.SimpleButton()
		Me.lblgavberuf = New System.Windows.Forms.Label()
		Me.lblDetail = New System.Windows.Forms.Label()
		Me.txt_SQLQuery = New DevExpress.XtraEditors.TextEdit()
		Me.Cbo_GAVBeruf = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.Cbo_Kanton = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.lblkanton = New System.Windows.Forms.Label()
		Me.Cbo_Organ = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.lblorgen = New System.Windows.Forms.Label()
		Me.lblJahr = New System.Windows.Forms.Label()
		Me.libLoadAddress = New System.Windows.Forms.LinkLabel()
		Me.GroupBox1 = New DevExpress.XtraEditors.PanelControl()
		Me.Label37 = New System.Windows.Forms.Label()
		Me.LblTimeValue = New DevExpress.XtraEditors.LabelControl()
		Me.CmdClose = New DevExpress.XtraEditors.SimpleButton()
		Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager()
		Me.Bar4 = New DevExpress.XtraBars.Bar()
		Me.bsiInfo = New DevExpress.XtraBars.BarStaticItem()
		Me.bbiSave = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiNew = New DevExpress.XtraBars.BarLargeButtonItem()
		Me.bbiDelete = New DevExpress.XtraBars.BarButtonItem()
		Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
		Me.BarStaticItem1 = New DevExpress.XtraBars.BarStaticItem()
		CType(Me.txtGAV_Postfach.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtGAV_ZHD.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtGAV_Strasse.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtGAV_PLZ.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtGAV_Ort.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtGAV_Bank.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtGAV_IBAN.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtGAV_BankKonto.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtRecNr.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtGAV_BankPLZOrt.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtGAV_AdressNr.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtGAV_Name.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txt_SQLQuery.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_GAVBeruf.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_Kanton.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_Organ.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupBox1.SuspendLayout()
		CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.PanelControl1.SuspendLayout()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'lblHeader2
		'
		Me.lblHeader2.AutoSize = True
		Me.lblHeader2.BackColor = System.Drawing.Color.Transparent
		Me.lblHeader2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblHeader2.Location = New System.Drawing.Point(120, 53)
		Me.lblHeader2.Name = "lblHeader2"
		Me.lblHeader2.Size = New System.Drawing.Size(244, 13)
		Me.lblHeader2.TabIndex = 1
		Me.lblHeader2.Text = "Wählen Sie bitte Ihre gewünschte Kriterien aus..."
		'
		'lblHeader1
		'
		Me.lblHeader1.AutoSize = True
		Me.lblHeader1.BackColor = System.Drawing.Color.Transparent
		Me.lblHeader1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblHeader1.Location = New System.Drawing.Point(102, 29)
		Me.lblHeader1.Name = "lblHeader1"
		Me.lblHeader1.Size = New System.Drawing.Size(161, 13)
		Me.lblHeader1.TabIndex = 0
		Me.lblHeader1.Text = "Auswahl der GAV-Adressen"
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
		'lblbankname
		'
		Me.lblbankname.Location = New System.Drawing.Point(376, 48)
		Me.lblbankname.Name = "lblbankname"
		Me.lblbankname.Size = New System.Drawing.Size(84, 13)
		Me.lblbankname.TabIndex = 0
		Me.lblbankname.Text = "Bankname"
		Me.lblbankname.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblpckonto
		'
		Me.lblpckonto.Location = New System.Drawing.Point(376, 100)
		Me.lblpckonto.Name = "lblpckonto"
		Me.lblpckonto.Size = New System.Drawing.Size(84, 13)
		Me.lblpckonto.TabIndex = 139
		Me.lblpckonto.Text = "PC-Konto"
		Me.lblpckonto.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblbetriebsnummer
		'
		Me.lblbetriebsnummer.Location = New System.Drawing.Point(376, 152)
		Me.lblbetriebsnummer.Name = "lblbetriebsnummer"
		Me.lblbetriebsnummer.Size = New System.Drawing.Size(84, 13)
		Me.lblbetriebsnummer.TabIndex = 0
		Me.lblbetriebsnummer.Text = "Betriebsnummer"
		Me.lblbetriebsnummer.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblbetriebsnummer.Visible = False
		'
		'lblzhd
		'
		Me.lblzhd.Location = New System.Drawing.Point(32, 74)
		Me.lblzhd.Name = "lblzhd"
		Me.lblzhd.Size = New System.Drawing.Size(50, 13)
		Me.lblzhd.TabIndex = 0
		Me.lblzhd.Text = "Z. Hd."
		Me.lblzhd.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'txtGAV_Postfach
		'
		Me.txtGAV_Postfach.Location = New System.Drawing.Point(88, 100)
		Me.txtGAV_Postfach.Name = "txtGAV_Postfach"
		Me.txtGAV_Postfach.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtGAV_Postfach.Size = New System.Drawing.Size(256, 20)
		Me.txtGAV_Postfach.TabIndex = 30
		'
		'txtGAV_ZHD
		'
		Me.txtGAV_ZHD.Location = New System.Drawing.Point(88, 74)
		Me.txtGAV_ZHD.Name = "txtGAV_ZHD"
		Me.txtGAV_ZHD.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
		Me.txtGAV_ZHD.Properties.Appearance.Options.UseBackColor = True
		Me.txtGAV_ZHD.Size = New System.Drawing.Size(256, 20)
		Me.txtGAV_ZHD.TabIndex = 20
		'
		'lblpostfach
		'
		Me.lblpostfach.Location = New System.Drawing.Point(32, 101)
		Me.lblpostfach.Name = "lblpostfach"
		Me.lblpostfach.Size = New System.Drawing.Size(50, 13)
		Me.lblpostfach.TabIndex = 0
		Me.lblpostfach.Text = "Postfach"
		Me.lblpostfach.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'txtGAV_Strasse
		'
		Me.txtGAV_Strasse.Location = New System.Drawing.Point(88, 126)
		Me.txtGAV_Strasse.Name = "txtGAV_Strasse"
		Me.txtGAV_Strasse.Size = New System.Drawing.Size(256, 20)
		Me.txtGAV_Strasse.TabIndex = 40
		'
		'lblstrasse
		'
		Me.lblstrasse.Location = New System.Drawing.Point(32, 126)
		Me.lblstrasse.Name = "lblstrasse"
		Me.lblstrasse.Size = New System.Drawing.Size(50, 13)
		Me.lblstrasse.TabIndex = 158
		Me.lblstrasse.Text = "Strasse"
		Me.lblstrasse.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'txtGAV_PLZ
		'
		Me.txtGAV_PLZ.Location = New System.Drawing.Point(88, 152)
		Me.txtGAV_PLZ.Name = "txtGAV_PLZ"
		Me.txtGAV_PLZ.Size = New System.Drawing.Size(60, 20)
		Me.txtGAV_PLZ.TabIndex = 50
		'
		'lblplzort
		'
		Me.lblplzort.Location = New System.Drawing.Point(32, 153)
		Me.lblplzort.Name = "lblplzort"
		Me.lblplzort.Size = New System.Drawing.Size(50, 13)
		Me.lblplzort.TabIndex = 157
		Me.lblplzort.Text = "PLZ / Ort"
		Me.lblplzort.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'txtGAV_Ort
		'
		Me.txtGAV_Ort.Location = New System.Drawing.Point(166, 152)
		Me.txtGAV_Ort.Name = "txtGAV_Ort"
		Me.txtGAV_Ort.Size = New System.Drawing.Size(178, 20)
		Me.txtGAV_Ort.TabIndex = 60
		'
		'txtGAV_Bank
		'
		Me.txtGAV_Bank.Location = New System.Drawing.Point(466, 48)
		Me.txtGAV_Bank.Name = "txtGAV_Bank"
		Me.txtGAV_Bank.Size = New System.Drawing.Size(256, 20)
		Me.txtGAV_Bank.TabIndex = 80
		'
		'txtGAV_IBAN
		'
		Me.txtGAV_IBAN.Location = New System.Drawing.Point(466, 126)
		Me.txtGAV_IBAN.Name = "txtGAV_IBAN"
		Me.txtGAV_IBAN.Size = New System.Drawing.Size(256, 20)
		Me.txtGAV_IBAN.TabIndex = 174
		'
		'lbliban
		'
		Me.lbliban.Location = New System.Drawing.Point(376, 126)
		Me.lbliban.Name = "lbliban"
		Me.lbliban.Size = New System.Drawing.Size(84, 13)
		Me.lbliban.TabIndex = 173
		Me.lbliban.Text = "IBAN"
		Me.lbliban.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'txtGAV_BankKonto
		'
		Me.txtGAV_BankKonto.Location = New System.Drawing.Point(466, 100)
		Me.txtGAV_BankKonto.Name = "txtGAV_BankKonto"
		Me.txtGAV_BankKonto.Size = New System.Drawing.Size(256, 20)
		Me.txtGAV_BankKonto.TabIndex = 172
		'
		'txtRecNr
		'
		Me.txtRecNr.EditValue = "0"
		Me.txtRecNr.Enabled = False
		Me.txtRecNr.Location = New System.Drawing.Point(88, 22)
		Me.txtRecNr.Name = "txtRecNr"
		Me.txtRecNr.Properties.Appearance.Options.UseTextOptions = True
		Me.txtRecNr.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.txtRecNr.Size = New System.Drawing.Size(62, 20)
		Me.txtRecNr.TabIndex = 177
		Me.txtRecNr.TabStop = False
		'
		'txtGAV_BankPLZOrt
		'
		Me.txtGAV_BankPLZOrt.Location = New System.Drawing.Point(466, 74)
		Me.txtGAV_BankPLZOrt.Name = "txtGAV_BankPLZOrt"
		Me.txtGAV_BankPLZOrt.Size = New System.Drawing.Size(256, 20)
		Me.txtGAV_BankPLZOrt.TabIndex = 170
		'
		'lblNummer
		'
		Me.lblNummer.Location = New System.Drawing.Point(32, 26)
		Me.lblNummer.Name = "lblNummer"
		Me.lblNummer.Size = New System.Drawing.Size(50, 13)
		Me.lblNummer.TabIndex = 178
		Me.lblNummer.Text = "Nummer"
		Me.lblNummer.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblbankplzort
		'
		Me.lblbankplzort.Location = New System.Drawing.Point(376, 74)
		Me.lblbankplzort.Name = "lblbankplzort"
		Me.lblbankplzort.Size = New System.Drawing.Size(84, 13)
		Me.lblbankplzort.TabIndex = 171
		Me.lblbankplzort.Text = "PLZ Ort"
		Me.lblbankplzort.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'txtGAV_AdressNr
		'
		Me.txtGAV_AdressNr.Location = New System.Drawing.Point(466, 152)
		Me.txtGAV_AdressNr.Name = "txtGAV_AdressNr"
		Me.txtGAV_AdressNr.Size = New System.Drawing.Size(256, 20)
		Me.txtGAV_AdressNr.TabIndex = 180
		Me.txtGAV_AdressNr.Visible = False
		'
		'lblname
		'
		Me.lblname.Location = New System.Drawing.Point(32, 48)
		Me.lblname.Name = "lblname"
		Me.lblname.Size = New System.Drawing.Size(50, 13)
		Me.lblname.TabIndex = 166
		Me.lblname.Text = "Name"
		Me.lblname.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'txtGAV_Name
		'
		Me.txtGAV_Name.Location = New System.Drawing.Point(88, 48)
		Me.txtGAV_Name.Name = "txtGAV_Name"
		Me.txtGAV_Name.Size = New System.Drawing.Size(256, 20)
		Me.txtGAV_Name.TabIndex = 10
		'
		'lvAdressen
		'
		Me.lvAdressen.BackgroundImageTiled = True
		'
		'
		'
		Me.lvAdressen.Border.Class = "ListViewBorder"
		Me.lvAdressen.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
		Me.lvAdressen.FullRowSelect = True
		Me.lvAdressen.HideSelection = False
		Me.lvAdressen.Location = New System.Drawing.Point(17, 129)
		Me.lvAdressen.MultiSelect = False
		Me.lvAdressen.Name = "lvAdressen"
		Me.lvAdressen.Size = New System.Drawing.Size(743, 241)
		Me.lvAdressen.TabIndex = 1
		Me.lvAdressen.UseCompatibleStateImageBehavior = False
		Me.lvAdressen.View = System.Windows.Forms.View.Details
		'
		'btnAdrIdInsert
		'
		Me.btnAdrIdInsert.Enabled = False
		Me.btnAdrIdInsert.Location = New System.Drawing.Point(685, 420)
		Me.btnAdrIdInsert.Name = "btnAdrIdInsert"
		Me.btnAdrIdInsert.Size = New System.Drawing.Size(75, 23)
		Me.btnAdrIdInsert.TabIndex = 7
		Me.btnAdrIdInsert.Text = "Einfügen"
		Me.btnAdrIdInsert.Visible = False
		'
		'btnAdrIdCopy
		'
		Me.btnAdrIdCopy.Enabled = False
		Me.btnAdrIdCopy.Location = New System.Drawing.Point(603, 420)
		Me.btnAdrIdCopy.Name = "btnAdrIdCopy"
		Me.btnAdrIdCopy.Size = New System.Drawing.Size(75, 23)
		Me.btnAdrIdCopy.TabIndex = 6
		Me.btnAdrIdCopy.Text = "Kopieren"
		Me.btnAdrIdCopy.Visible = False
		'
		'lblgavberuf
		'
		Me.lblgavberuf.Location = New System.Drawing.Point(17, 394)
		Me.lblgavberuf.Name = "lblgavberuf"
		Me.lblgavberuf.Size = New System.Drawing.Size(82, 13)
		Me.lblgavberuf.TabIndex = 9
		Me.lblgavberuf.Text = "GAV-Beruf"
		Me.lblgavberuf.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblDetail
		'
		Me.lblDetail.AutoSize = True
		Me.lblDetail.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDetail.Location = New System.Drawing.Point(17, 111)
		Me.lblDetail.Name = "lblDetail"
		Me.lblDetail.Size = New System.Drawing.Size(72, 13)
		Me.lblDetail.TabIndex = 10
		Me.lblDetail.Text = "Alle Adressen"
		'
		'txt_SQLQuery
		'
		Me.txt_SQLQuery.Location = New System.Drawing.Point(1039, 376)
		Me.txt_SQLQuery.Name = "txt_SQLQuery"
		Me.txt_SQLQuery.Size = New System.Drawing.Size(28, 20)
		Me.txt_SQLQuery.TabIndex = 81
		Me.txt_SQLQuery.Visible = False
		'
		'Cbo_GAVBeruf
		'
		Me.Cbo_GAVBeruf.Location = New System.Drawing.Point(105, 390)
		Me.Cbo_GAVBeruf.Name = "Cbo_GAVBeruf"
		Me.Cbo_GAVBeruf.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_GAVBeruf.Size = New System.Drawing.Size(256, 20)
		Me.Cbo_GAVBeruf.TabIndex = 2
		'
		'Cbo_Kanton
		'
		Me.Cbo_Kanton.Location = New System.Drawing.Point(105, 416)
		Me.Cbo_Kanton.Name = "Cbo_Kanton"
		Me.Cbo_Kanton.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_Kanton.Size = New System.Drawing.Size(64, 20)
		Me.Cbo_Kanton.TabIndex = 3
		'
		'lblkanton
		'
		Me.lblkanton.Location = New System.Drawing.Point(17, 420)
		Me.lblkanton.Name = "lblkanton"
		Me.lblkanton.Size = New System.Drawing.Size(82, 13)
		Me.lblkanton.TabIndex = 183
		Me.lblkanton.Text = "Kanton"
		Me.lblkanton.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'Cbo_Organ
		'
		Me.Cbo_Organ.Location = New System.Drawing.Point(234, 416)
		Me.Cbo_Organ.Name = "Cbo_Organ"
		Me.Cbo_Organ.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_Organ.Size = New System.Drawing.Size(127, 20)
		Me.Cbo_Organ.TabIndex = 4
		'
		'lblorgen
		'
		Me.lblorgen.Location = New System.Drawing.Point(191, 420)
		Me.lblorgen.Name = "lblorgen"
		Me.lblorgen.Size = New System.Drawing.Size(37, 13)
		Me.lblorgen.TabIndex = 185
		Me.lblorgen.Text = "Organ"
		Me.lblorgen.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblJahr
		'
		Me.lblJahr.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.lblJahr.Location = New System.Drawing.Point(755, 87)
		Me.lblJahr.Name = "lblJahr"
		Me.lblJahr.Size = New System.Drawing.Size(101, 13)
		Me.lblJahr.TabIndex = 6
		Me.lblJahr.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblJahr.Visible = False
		'
		'libLoadAddress
		'
		Me.libLoadAddress.AutoSize = True
		Me.libLoadAddress.Location = New System.Drawing.Point(367, 394)
		Me.libLoadAddress.Name = "libLoadAddress"
		Me.libLoadAddress.Size = New System.Drawing.Size(87, 13)
		Me.libLoadAddress.TabIndex = 187
		Me.libLoadAddress.TabStop = True
		Me.libLoadAddress.Text = "Adresse laden..."
		Me.libLoadAddress.Visible = False
		'
		'GroupBox1
		'
		Me.GroupBox1.Appearance.BackColor = System.Drawing.Color.White
		Me.GroupBox1.Appearance.Options.UseBackColor = True
		Me.GroupBox1.Controls.Add(Me.Label37)
		Me.GroupBox1.Controls.Add(Me.LblTimeValue)
		Me.GroupBox1.Controls.Add(Me.CmdClose)
		Me.GroupBox1.Controls.Add(Me.lblHeader1)
		Me.GroupBox1.Controls.Add(Me.lblHeader2)
		Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
		Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(783, 93)
		Me.GroupBox1.TabIndex = 188
		'
		'Label37
		'
		Me.Label37.BackColor = System.Drawing.Color.Transparent
		Me.Label37.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label37.ForeColor = System.Drawing.SystemColors.HotTrack
		Me.Label37.Image = CType(resources.GetObject("Label37.Image"), System.Drawing.Image)
		Me.Label37.Location = New System.Drawing.Point(13, 19)
		Me.Label37.Name = "Label37"
		Me.Label37.Size = New System.Drawing.Size(83, 65)
		Me.Label37.TabIndex = 1001
		'
		'LblTimeValue
		'
		Me.LblTimeValue.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.LblTimeValue.Appearance.Options.UseTextOptions = True
		Me.LblTimeValue.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.LblTimeValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.LblTimeValue.Location = New System.Drawing.Point(536, 53)
		Me.LblTimeValue.Name = "LblTimeValue"
		Me.LblTimeValue.Size = New System.Drawing.Size(201, 13)
		Me.LblTimeValue.TabIndex = 14
		Me.LblTimeValue.Text = "Zeitangaben..."
		Me.LblTimeValue.Visible = False
		'
		'CmdClose
		'
		Me.CmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.CmdClose.Location = New System.Drawing.Point(651, 29)
		Me.CmdClose.Name = "CmdClose"
		Me.CmdClose.Size = New System.Drawing.Size(86, 24)
		Me.CmdClose.TabIndex = 100
		Me.CmdClose.TabStop = False
		Me.CmdClose.Text = "Schliessen"
		'
		'PanelControl1
		'
		Me.PanelControl1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.PanelControl1.Controls.Add(Me.txtGAV_IBAN)
		Me.PanelControl1.Controls.Add(Me.lblNummer)
		Me.PanelControl1.Controls.Add(Me.lbliban)
		Me.PanelControl1.Controls.Add(Me.lblplzort)
		Me.PanelControl1.Controls.Add(Me.txtGAV_BankKonto)
		Me.PanelControl1.Controls.Add(Me.txtGAV_PLZ)
		Me.PanelControl1.Controls.Add(Me.txtRecNr)
		Me.PanelControl1.Controls.Add(Me.txtGAV_ZHD)
		Me.PanelControl1.Controls.Add(Me.txtGAV_BankPLZOrt)
		Me.PanelControl1.Controls.Add(Me.lblstrasse)
		Me.PanelControl1.Controls.Add(Me.txtGAV_Postfach)
		Me.PanelControl1.Controls.Add(Me.lblbankplzort)
		Me.PanelControl1.Controls.Add(Me.txtGAV_Strasse)
		Me.PanelControl1.Controls.Add(Me.txtGAV_AdressNr)
		Me.PanelControl1.Controls.Add(Me.lblpostfach)
		Me.PanelControl1.Controls.Add(Me.lblname)
		Me.PanelControl1.Controls.Add(Me.txtGAV_Ort)
		Me.PanelControl1.Controls.Add(Me.txtGAV_Name)
		Me.PanelControl1.Controls.Add(Me.lblbetriebsnummer)
		Me.PanelControl1.Controls.Add(Me.lblzhd)
		Me.PanelControl1.Controls.Add(Me.txtGAV_Bank)
		Me.PanelControl1.Controls.Add(Me.lblpckonto)
		Me.PanelControl1.Controls.Add(Me.lblbankname)
		Me.PanelControl1.Location = New System.Drawing.Point(17, 449)
		Me.PanelControl1.Name = "PanelControl1"
		Me.PanelControl1.Size = New System.Drawing.Size(743, 217)
		Me.PanelControl1.TabIndex = 189
		'
		'BarManager1
		'
		Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar4})
		Me.BarManager1.DockControls.Add(Me.barDockControlTop)
		Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
		Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
		Me.BarManager1.DockControls.Add(Me.barDockControlRight)
		Me.BarManager1.Form = Me
		Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiInfo, Me.bbiSave, Me.bbiNew, Me.bbiDelete, Me.BarStaticItem1})
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
		Me.Bar4.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiInfo), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiSave), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiNew, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiDelete, True)})
		Me.Bar4.OptionsBar.AllowQuickCustomization = False
		Me.Bar4.OptionsBar.DrawDragBorder = False
		Me.Bar4.OptionsBar.UseWholeRow = True
		Me.Bar4.Text = "Statusleiste"
		'
		'bsiInfo
		'
		Me.bsiInfo.AutoSize = DevExpress.XtraBars.BarStaticItemSize.None
		Me.bsiInfo.Caption = "Bereit"
		Me.bsiInfo.Id = 0
		Me.bsiInfo.Name = "bsiInfo"
		Me.bsiInfo.Size = New System.Drawing.Size(400, 0)
		Me.bsiInfo.Width = 400
		'
		'bbiSave
		'
		Me.bbiSave.Caption = "Daten sichern"
		Me.bbiSave.Id = 1
		Me.bbiSave.ImageOptions.Image = CType(resources.GetObject("bbiSave.ImageOptions.Image"), System.Drawing.Image)
		Me.bbiSave.Name = "bbiSave"
		Me.bbiSave.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiNew
		'
		Me.bbiNew.Caption = "Neu hinzufügen"
		Me.bbiNew.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Right
		Me.bbiNew.Id = 6
		Me.bbiNew.ImageOptions.Image = CType(resources.GetObject("bbiNew.ImageOptions.Image"), System.Drawing.Image)
		Me.bbiNew.ImageOptions.LargeImage = CType(resources.GetObject("bbiNew.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.bbiNew.Name = "bbiNew"
		Me.bbiNew.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiDelete
		'
		Me.bbiDelete.Caption = "Löschen"
		Me.bbiDelete.Id = 8
		Me.bbiDelete.ImageOptions.Image = CType(resources.GetObject("bbiDelete.ImageOptions.Image"), System.Drawing.Image)
		Me.bbiDelete.Name = "bbiDelete"
		Me.bbiDelete.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'barDockControlTop
		'
		Me.barDockControlTop.CausesValidation = False
		Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
		Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlTop.Manager = Me.BarManager1
		Me.barDockControlTop.Size = New System.Drawing.Size(783, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 711)
		Me.barDockControlBottom.Manager = Me.BarManager1
		Me.barDockControlBottom.Size = New System.Drawing.Size(783, 27)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Manager = Me.BarManager1
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 711)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(783, 0)
		Me.barDockControlRight.Manager = Me.BarManager1
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 711)
		'
		'BarStaticItem1
		'
		Me.BarStaticItem1.Caption = " "
		Me.BarStaticItem1.Id = 5
		Me.BarStaticItem1.Name = "BarStaticItem1"
		'
		'frmGAVAdressen
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(783, 738)
		Me.Controls.Add(Me.btnAdrIdInsert)
		Me.Controls.Add(Me.PanelControl1)
		Me.Controls.Add(Me.btnAdrIdCopy)
		Me.Controls.Add(Me.GroupBox1)
		Me.Controls.Add(Me.lblJahr)
		Me.Controls.Add(Me.libLoadAddress)
		Me.Controls.Add(Me.Cbo_Kanton)
		Me.Controls.Add(Me.Cbo_Organ)
		Me.Controls.Add(Me.lblkanton)
		Me.Controls.Add(Me.txt_SQLQuery)
		Me.Controls.Add(Me.lblorgen)
		Me.Controls.Add(Me.Cbo_GAVBeruf)
		Me.Controls.Add(Me.lblgavberuf)
		Me.Controls.Add(Me.lblDetail)
		Me.Controls.Add(Me.lvAdressen)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.MaximumSize = New System.Drawing.Size(799, 776)
		Me.MinimumSize = New System.Drawing.Size(793, 770)
		Me.Name = "frmGAVAdressen"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Adresse der GAV-Organisationen"
		CType(Me.txtGAV_Postfach.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtGAV_ZHD.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtGAV_Strasse.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtGAV_PLZ.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtGAV_Ort.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtGAV_Bank.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtGAV_IBAN.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtGAV_BankKonto.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtRecNr.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtGAV_BankPLZOrt.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtGAV_AdressNr.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtGAV_Name.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txt_SQLQuery.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_GAVBeruf.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_Kanton.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_Organ.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupBox1.ResumeLayout(False)
		Me.GroupBox1.PerformLayout()
		CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.PanelControl1.ResumeLayout(False)
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents lblHeader2 As System.Windows.Forms.Label
  Friend WithEvents lblHeader1 As System.Windows.Forms.Label
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
	Friend WithEvents Timer1 As System.Windows.Forms.Timer
	Friend WithEvents lblbankname As System.Windows.Forms.Label
	Friend WithEvents lblpckonto As System.Windows.Forms.Label
  Friend WithEvents lblbetriebsnummer As System.Windows.Forms.Label
  Friend WithEvents lblzhd As System.Windows.Forms.Label
  Friend WithEvents txtGAV_Postfach As DevExpress.XtraEditors.TextEdit
  Friend WithEvents txtGAV_ZHD As DevExpress.XtraEditors.TextEdit
  Friend WithEvents lblpostfach As System.Windows.Forms.Label
  Friend WithEvents txtGAV_Strasse As DevExpress.XtraEditors.TextEdit
  Friend WithEvents lblstrasse As System.Windows.Forms.Label
  Friend WithEvents txtGAV_PLZ As DevExpress.XtraEditors.TextEdit
  Friend WithEvents lblplzort As System.Windows.Forms.Label
  Friend WithEvents txtGAV_Ort As DevExpress.XtraEditors.TextEdit
  Friend WithEvents txtGAV_Bank As DevExpress.XtraEditors.TextEdit
  Friend WithEvents lblname As System.Windows.Forms.Label
  Friend WithEvents txtGAV_Name As DevExpress.XtraEditors.TextEdit
  Friend WithEvents txtGAV_AdressNr As DevExpress.XtraEditors.TextEdit
  Friend WithEvents lvAdressen As DevComponents.DotNetBar.Controls.ListViewEx
  Friend WithEvents txtGAV_BankPLZOrt As DevExpress.XtraEditors.TextEdit
  Friend WithEvents lblbankplzort As System.Windows.Forms.Label
  Friend WithEvents txtGAV_IBAN As DevExpress.XtraEditors.TextEdit
  Friend WithEvents lbliban As System.Windows.Forms.Label
  Friend WithEvents txtGAV_BankKonto As DevExpress.XtraEditors.TextEdit
  Friend WithEvents lblgavberuf As System.Windows.Forms.Label
  Friend WithEvents lblDetail As System.Windows.Forms.Label
  Friend WithEvents txt_SQLQuery As DevExpress.XtraEditors.TextEdit
  Friend WithEvents btnAdrIdInsert As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents btnAdrIdCopy As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents txtRecNr As DevExpress.XtraEditors.TextEdit
  Friend WithEvents lblNummer As System.Windows.Forms.Label
  Friend WithEvents Cbo_GAVBeruf As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents Cbo_Kanton As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents lblkanton As System.Windows.Forms.Label
  Friend WithEvents Cbo_Organ As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents lblorgen As System.Windows.Forms.Label
  Friend WithEvents lblJahr As System.Windows.Forms.Label
  Friend WithEvents libLoadAddress As System.Windows.Forms.LinkLabel
  Friend WithEvents GroupBox1 As DevExpress.XtraEditors.PanelControl
  Friend WithEvents Label37 As System.Windows.Forms.Label
  Friend WithEvents LblTimeValue As DevExpress.XtraEditors.LabelControl
  Friend WithEvents CmdClose As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
  Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
  Friend WithEvents Bar4 As DevExpress.XtraBars.Bar
  Friend WithEvents bsiInfo As DevExpress.XtraBars.BarStaticItem
  Friend WithEvents bbiSave As DevExpress.XtraBars.BarButtonItem
  Friend WithEvents bbiNew As DevExpress.XtraBars.BarLargeButtonItem
  Friend WithEvents bbiDelete As DevExpress.XtraBars.BarButtonItem
  Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
  Friend WithEvents BarStaticItem1 As DevExpress.XtraBars.BarStaticItem

End Class
