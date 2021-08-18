<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFibuSearch
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmFibuSearch))
		Me.GroupBox1 = New System.Windows.Forms.GroupBox()
		Me.LblSetting = New System.Windows.Forms.Label()
		Me.LblTimeValue = New System.Windows.Forms.Label()
		Me.CmdClose = New DevExpress.XtraEditors.SimpleButton()
		Me.lblHeaderNormal = New System.Windows.Forms.Label()
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
		Me.LblSort = New System.Windows.Forms.Label()
		Me.LblMonth = New System.Windows.Forms.Label()
		Me.LblFilial = New System.Windows.Forms.Label()
		Me.LblYear = New System.Windows.Forms.Label()
		Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
		Me.txt_SQL_1 = New System.Windows.Forms.TextBox()
		Me.txt_SQL_2 = New System.Windows.Forms.TextBox()
		Me.ChkNot_0 = New DevExpress.XtraEditors.CheckEdit()
		Me.ChkUJListe = New DevExpress.XtraEditors.CheckEdit()
		Me.ChkNot_Kum = New DevExpress.XtraEditors.CheckEdit()
		Me.grpZusatzinfo = New System.Windows.Forms.GroupBox()
		Me.Label6 = New System.Windows.Forms.Label()
		Me.Label5 = New System.Windows.Forms.Label()
		Me.ChkNewVersion = New DevExpress.XtraEditors.CheckEdit()
		Me.Cbo_Filiale = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.CboSort = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.Cbo_Year = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.Cbo_Month = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.lueMandant = New DevExpress.XtraEditors.LookUpEdit()
		Me.lblMDName = New System.Windows.Forms.Label()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager()
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
		Me.XtraTabControl1 = New DevExpress.XtraTab.XtraTabControl()
		Me.xtabAllgemein = New DevExpress.XtraTab.XtraTabPage()
		Me.xtabSQLQuery = New DevExpress.XtraTab.XtraTabPage()
		Me.GroupBox1.SuspendLayout()
		CType(Me.ChkNot_0.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.ChkUJListe.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.ChkNot_Kum.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grpZusatzinfo.SuspendLayout()
		CType(Me.ChkNewVersion.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_Filiale.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.CboSort.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_Year.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_Month.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.XtraTabControl1.SuspendLayout()
		Me.xtabAllgemein.SuspendLayout()
		Me.xtabSQLQuery.SuspendLayout()
		Me.SuspendLayout()
		'
		'GroupBox1
		'
		Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.GroupBox1.BackColor = System.Drawing.Color.White
		Me.GroupBox1.Controls.Add(Me.LblSetting)
		Me.GroupBox1.Controls.Add(Me.LblTimeValue)
		Me.GroupBox1.Controls.Add(Me.CmdClose)
		Me.GroupBox1.Controls.Add(Me.lblHeaderNormal)
		Me.GroupBox1.Controls.Add(Me.lblHeaderFett)
		Me.GroupBox1.Location = New System.Drawing.Point(-1, -10)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(757, 93)
		Me.GroupBox1.TabIndex = 3
		Me.GroupBox1.TabStop = False
		'
		'LblSetting
		'
		Me.LblSetting.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.LblSetting.ForeColor = System.Drawing.SystemColors.HotTrack
		Me.LblSetting.Image = CType(resources.GetObject("LblSetting.Image"), System.Drawing.Image)
		Me.LblSetting.Location = New System.Drawing.Point(6, 20)
		Me.LblSetting.Name = "LblSetting"
		Me.LblSetting.Size = New System.Drawing.Size(83, 63)
		Me.LblSetting.TabIndex = 169
		'
		'LblTimeValue
		'
		Me.LblTimeValue.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.LblTimeValue.Location = New System.Drawing.Point(387, 56)
		Me.LblTimeValue.Name = "LblTimeValue"
		Me.LblTimeValue.Size = New System.Drawing.Size(308, 13)
		Me.LblTimeValue.TabIndex = 14
		Me.LblTimeValue.Text = "Zeitangaben..."
		Me.LblTimeValue.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.LblTimeValue.Visible = False
		'
		'CmdClose
		'
		Me.CmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.CmdClose.Location = New System.Drawing.Point(609, 29)
		Me.CmdClose.Name = "CmdClose"
		Me.CmdClose.Size = New System.Drawing.Size(86, 24)
		Me.CmdClose.TabIndex = 5
		Me.CmdClose.Text = "Schliessen"
		'
		'lblHeaderNormal
		'
		Me.lblHeaderNormal.Location = New System.Drawing.Point(113, 53)
		Me.lblHeaderNormal.Name = "lblHeaderNormal"
		Me.lblHeaderNormal.Size = New System.Drawing.Size(225, 13)
		Me.lblHeaderNormal.TabIndex = 1
		Me.lblHeaderNormal.Text = "Geben Sie bitte Ihre gewünschten Kriterien ein."
		'
		'lblHeaderFett
		'
		Me.lblHeaderFett.AutoSize = True
		Me.lblHeaderFett.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblHeaderFett.Location = New System.Drawing.Point(95, 29)
		Me.lblHeaderFett.Name = "lblHeaderFett"
		Me.lblHeaderFett.Size = New System.Drawing.Size(205, 13)
		Me.lblHeaderFett.TabIndex = 0
		Me.lblHeaderFett.Text = "Liste der Buchungsbelege für FIBU"
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
		'LblSort
		'
		Me.LblSort.AutoSize = True
		Me.LblSort.Location = New System.Drawing.Point(803, 148)
		Me.LblSort.Name = "LblSort"
		Me.LblSort.Size = New System.Drawing.Size(77, 13)
		Me.LblSort.TabIndex = 15
		Me.LblSort.Text = "Sortieren nach"
		Me.LblSort.Visible = False
		'
		'LblMonth
		'
		Me.LblMonth.Location = New System.Drawing.Point(21, 108)
		Me.LblMonth.Name = "LblMonth"
		Me.LblMonth.Size = New System.Drawing.Size(117, 13)
		Me.LblMonth.TabIndex = 21
		Me.LblMonth.Text = "Monat"
		Me.LblMonth.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'LblFilial
		'
		Me.LblFilial.Location = New System.Drawing.Point(21, 145)
		Me.LblFilial.Name = "LblFilial"
		Me.LblFilial.Size = New System.Drawing.Size(117, 13)
		Me.LblFilial.TabIndex = 83
		Me.LblFilial.Text = "Filiale"
		Me.LblFilial.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'LblYear
		'
		Me.LblYear.Location = New System.Drawing.Point(21, 81)
		Me.LblYear.Name = "LblYear"
		Me.LblYear.Size = New System.Drawing.Size(117, 13)
		Me.LblYear.TabIndex = 82
		Me.LblYear.Text = "Jahr"
		Me.LblYear.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'BackgroundWorker1
		'
		'
		'txt_SQL_1
		'
		Me.txt_SQL_1.Location = New System.Drawing.Point(37, 30)
		Me.txt_SQL_1.Multiline = True
		Me.txt_SQL_1.Name = "txt_SQL_1"
		Me.txt_SQL_1.Size = New System.Drawing.Size(597, 118)
		Me.txt_SQL_1.TabIndex = 100
		'
		'txt_SQL_2
		'
		Me.txt_SQL_2.Location = New System.Drawing.Point(37, 164)
		Me.txt_SQL_2.Multiline = True
		Me.txt_SQL_2.Name = "txt_SQL_2"
		Me.txt_SQL_2.Size = New System.Drawing.Size(597, 118)
		Me.txt_SQL_2.TabIndex = 101
		'
		'ChkNot_0
		'
		Me.ChkNot_0.EditValue = True
		Me.ChkNot_0.Location = New System.Drawing.Point(537, 105)
		Me.ChkNot_0.Name = "ChkNot_0"
		Me.ChkNot_0.Properties.Caption = "0-Beträge ausblenden"
		Me.ChkNot_0.Size = New System.Drawing.Size(132, 19)
		Me.ChkNot_0.TabIndex = 0
		Me.ChkNot_0.Visible = False
		'
		'ChkUJListe
		'
		Me.ChkUJListe.EditValue = True
		Me.ChkUJListe.Location = New System.Drawing.Point(322, 27)
		Me.ChkUJListe.Name = "ChkUJListe"
		Me.ChkUJListe.Properties.Caption = "Daten aus DB1-Liste berücksichtigen"
		Me.ChkUJListe.Size = New System.Drawing.Size(200, 19)
		Me.ChkUJListe.TabIndex = 1
		'
		'ChkNot_Kum
		'
		Me.ChkNot_Kum.EditValue = True
		Me.ChkNot_Kum.Location = New System.Drawing.Point(537, 130)
		Me.ChkNot_Kum.Name = "ChkNot_Kum"
		Me.ChkNot_Kum.Properties.Caption = "Ohne Kumulativzeilen"
		Me.ChkNot_Kum.Size = New System.Drawing.Size(128, 19)
		Me.ChkNot_Kum.TabIndex = 104
		Me.ChkNot_Kum.Visible = False
		'
		'grpZusatzinfo
		'
		Me.grpZusatzinfo.Controls.Add(Me.Label6)
		Me.grpZusatzinfo.Controls.Add(Me.Label5)
		Me.grpZusatzinfo.Controls.Add(Me.ChkNewVersion)
		Me.grpZusatzinfo.Controls.Add(Me.ChkUJListe)
		Me.grpZusatzinfo.Location = New System.Drawing.Point(37, 175)
		Me.grpZusatzinfo.Name = "grpZusatzinfo"
		Me.grpZusatzinfo.Size = New System.Drawing.Size(597, 116)
		Me.grpZusatzinfo.TabIndex = 105
		Me.grpZusatzinfo.TabStop = False
		Me.grpZusatzinfo.Text = "Zusatzinfo"
		'
		'Label6
		'
		Me.Label6.Location = New System.Drawing.Point(-39, 30)
		Me.Label6.Name = "Label6"
		Me.Label6.Size = New System.Drawing.Size(36, 42)
		Me.Label6.TabIndex = 107
		'
		'Label5
		'
		Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label5.ForeColor = System.Drawing.Color.Blue
		Me.Label5.Location = New System.Drawing.Point(15, 53)
		Me.Label5.Name = "Label5"
		Me.Label5.Size = New System.Drawing.Size(576, 45)
		Me.Label5.TabIndex = 106
		Me.Label5.Text = resources.GetString("Label5.Text")
		'
		'ChkNewVersion
		'
		Me.ChkNewVersion.EditValue = True
		Me.ChkNewVersion.Location = New System.Drawing.Point(17, 27)
		Me.ChkNewVersion.Name = "ChkNewVersion"
		Me.ChkNewVersion.Properties.Caption = "Teilung unterhalb der Filiale"
		Me.ChkNewVersion.Size = New System.Drawing.Size(157, 19)
		Me.ChkNewVersion.TabIndex = 105
		'
		'Cbo_Filiale
		'
		Me.Cbo_Filiale.EditValue = "Alle"
		Me.Cbo_Filiale.Location = New System.Drawing.Point(144, 141)
		Me.Cbo_Filiale.Name = "Cbo_Filiale"
		Me.Cbo_Filiale.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_Filiale.Size = New System.Drawing.Size(114, 20)
		Me.Cbo_Filiale.TabIndex = 81
		'
		'CboSort
		'
		Me.CboSort.Location = New System.Drawing.Point(806, 164)
		Me.CboSort.Name = "CboSort"
		Me.CboSort.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.CboSort.Size = New System.Drawing.Size(114, 20)
		Me.CboSort.TabIndex = 0
		Me.CboSort.Visible = False
		'
		'Cbo_Year
		'
		Me.Cbo_Year.Location = New System.Drawing.Point(144, 78)
		Me.Cbo_Year.Name = "Cbo_Year"
		Me.Cbo_Year.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_Year.Size = New System.Drawing.Size(114, 20)
		Me.Cbo_Year.TabIndex = 1
		'
		'Cbo_Month
		'
		Me.Cbo_Month.Location = New System.Drawing.Point(144, 104)
		Me.Cbo_Month.Name = "Cbo_Month"
		Me.Cbo_Month.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_Month.Size = New System.Drawing.Size(114, 20)
		Me.Cbo_Month.TabIndex = 2
		'
		'lueMandant
		'
		Me.lueMandant.Location = New System.Drawing.Point(144, 35)
		Me.lueMandant.Name = "lueMandant"
		Me.lueMandant.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.lueMandant.Properties.NullText = ""
		Me.lueMandant.Size = New System.Drawing.Size(421, 20)
		Me.lueMandant.TabIndex = 168
		'
		'lblMDName
		'
		Me.lblMDName.Location = New System.Drawing.Point(21, 39)
		Me.lblMDName.Name = "lblMDName"
		Me.lblMDName.Size = New System.Drawing.Size(117, 13)
		Me.lblMDName.TabIndex = 167
		Me.lblMDName.Text = "Mandanten"
		Me.lblMDName.TextAlign = System.Drawing.ContentAlignment.TopRight
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
		Me.bsiInfo.Width = 32
		'
		'bbiSearch
		'
		Me.bbiSearch.Caption = "Suchen"
		Me.bbiSearch.Id = 1
		Me.bbiSearch.ImageOptions.Image = CType(resources.GetObject("bbiSearch.ImageOptions.Image"), System.Drawing.Image)
		Me.bbiSearch.Name = "bbiSearch"
		Me.bbiSearch.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiClearFields
		'
		Me.bbiClearFields.Caption = "Felder leeren"
		Me.bbiClearFields.Id = 8
		Me.bbiClearFields.ImageOptions.Image = CType(resources.GetObject("bbiClearFields.ImageOptions.Image"), System.Drawing.Image)
		Me.bbiClearFields.Name = "bbiClearFields"
		Me.bbiClearFields.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiPrint
		'
		Me.bbiPrint.AllowAllUp = True
		Me.bbiPrint.Caption = "Drucken"
		Me.bbiPrint.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Right
		Me.bbiPrint.Id = 6
		Me.bbiPrint.ImageOptions.Image = CType(resources.GetObject("bbiPrint.ImageOptions.Image"), System.Drawing.Image)
		Me.bbiPrint.Name = "bbiPrint"
		Me.bbiPrint.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiExport
		'
		Me.bbiExport.Caption = "Exportieren"
		Me.bbiExport.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Right
		Me.bbiExport.Id = 4
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
		Me.barDockControlTop.Size = New System.Drawing.Size(728, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 482)
		Me.barDockControlBottom.Manager = Me.BarManager1
		Me.barDockControlBottom.Size = New System.Drawing.Size(728, 27)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Manager = Me.BarManager1
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 482)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(728, 0)
		Me.barDockControlRight.Manager = Me.BarManager1
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 482)
		'
		'BarStaticItem1
		'
		Me.BarStaticItem1.Caption = " "
		Me.BarStaticItem1.Id = 5
		Me.BarStaticItem1.Name = "BarStaticItem1"
		'
		'XtraTabControl1
		'
		Me.XtraTabControl1.Location = New System.Drawing.Point(27, 109)
		Me.XtraTabControl1.Name = "XtraTabControl1"
		Me.XtraTabControl1.SelectedTabPage = Me.xtabAllgemein
		Me.XtraTabControl1.Size = New System.Drawing.Size(675, 341)
		Me.XtraTabControl1.TabIndex = 169
		Me.XtraTabControl1.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabAllgemein, Me.xtabSQLQuery})
		'
		'xtabAllgemein
		'
		Me.xtabAllgemein.Controls.Add(Me.lueMandant)
		Me.xtabAllgemein.Controls.Add(Me.LblMonth)
		Me.xtabAllgemein.Controls.Add(Me.grpZusatzinfo)
		Me.xtabAllgemein.Controls.Add(Me.ChkNot_Kum)
		Me.xtabAllgemein.Controls.Add(Me.lblMDName)
		Me.xtabAllgemein.Controls.Add(Me.Cbo_Month)
		Me.xtabAllgemein.Controls.Add(Me.ChkNot_0)
		Me.xtabAllgemein.Controls.Add(Me.LblYear)
		Me.xtabAllgemein.Controls.Add(Me.Cbo_Filiale)
		Me.xtabAllgemein.Controls.Add(Me.Cbo_Year)
		Me.xtabAllgemein.Controls.Add(Me.LblFilial)
		Me.xtabAllgemein.Name = "xtabAllgemein"
		Me.xtabAllgemein.Size = New System.Drawing.Size(669, 313)
		Me.xtabAllgemein.Text = "Allgemein"
		'
		'xtabSQLQuery
		'
		Me.xtabSQLQuery.Controls.Add(Me.txt_SQL_1)
		Me.xtabSQLQuery.Controls.Add(Me.txt_SQL_2)
		Me.xtabSQLQuery.Name = "xtabSQLQuery"
		Me.xtabSQLQuery.Size = New System.Drawing.Size(669, 313)
		Me.xtabSQLQuery.Text = "SQL-Abfrage"
		'
		'frmFibuSearch
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(728, 509)
		Me.Controls.Add(Me.XtraTabControl1)
		Me.Controls.Add(Me.CboSort)
		Me.Controls.Add(Me.GroupBox1)
		Me.Controls.Add(Me.LblSort)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.Name = "frmFibuSearch"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Monatliche Lohnlisten: Buchungsbelege für FIBU"
		Me.GroupBox1.ResumeLayout(False)
		Me.GroupBox1.PerformLayout()
		CType(Me.ChkNot_0.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.ChkUJListe.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.ChkNot_Kum.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.grpZusatzinfo.ResumeLayout(False)
		CType(Me.ChkNewVersion.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_Filiale.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.CboSort.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_Year.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_Month.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.XtraTabControl1.ResumeLayout(False)
		Me.xtabAllgemein.ResumeLayout(False)
		Me.xtabSQLQuery.ResumeLayout(False)
		Me.xtabSQLQuery.PerformLayout()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
	Friend WithEvents lblHeaderNormal As System.Windows.Forms.Label
	Friend WithEvents lblHeaderFett As System.Windows.Forms.Label
	Friend WithEvents Label1 As System.Windows.Forms.Label
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
	Friend WithEvents CmdClose As DevExpress.XtraEditors.SimpleButton	' System.Windows.Forms.Button
	Friend WithEvents LblTimeValue As System.Windows.Forms.Label
	Friend WithEvents LblSort As System.Windows.Forms.Label
	Friend WithEvents Cbo_Month As DevExpress.XtraEditors.ComboBoxEdit ' SPFibuSearch.myCbo
	Friend WithEvents LblMonth As System.Windows.Forms.Label
	Friend WithEvents Cbo_Year As DevExpress.XtraEditors.ComboBoxEdit	' SPFibuSearch.myCbo
	Friend WithEvents Cbo_Filiale As DevExpress.XtraEditors.ComboBoxEdit ' SPFibuSearch.myCbo
	Friend WithEvents LblFilial As System.Windows.Forms.Label
	Friend WithEvents LblYear As System.Windows.Forms.Label
	Friend WithEvents CboSort As DevExpress.XtraEditors.ComboBoxEdit ' SPFibuSearch.myCbo
	Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
	Friend WithEvents txt_SQL_1 As System.Windows.Forms.TextBox
	Friend WithEvents txt_SQL_2 As System.Windows.Forms.TextBox
	Friend WithEvents ChkNot_0 As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents ChkUJListe As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents ChkNot_Kum As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents grpZusatzinfo As System.Windows.Forms.GroupBox
	Friend WithEvents ChkNewVersion As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents Label6 As System.Windows.Forms.Label
	Friend WithEvents Label5 As System.Windows.Forms.Label
	Friend WithEvents lueMandant As DevExpress.XtraEditors.LookUpEdit
  Friend WithEvents lblMDName As System.Windows.Forms.Label
	Friend WithEvents LblSetting As System.Windows.Forms.Label
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
	Friend WithEvents XtraTabControl1 As DevExpress.XtraTab.XtraTabControl
	Friend WithEvents xtabAllgemein As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents xtabSQLQuery As DevExpress.XtraTab.XtraTabPage

End Class
