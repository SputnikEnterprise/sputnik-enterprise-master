<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmZESearch
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmZESearch))
		Me.GroupBox1 = New DevExpress.XtraEditors.PanelControl()
		Me.LblSetting = New System.Windows.Forms.Label()
		Me.LblTimeValue = New System.Windows.Forms.Label()
		Me.CmdClose = New DevExpress.XtraEditors.SimpleButton()
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
		Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip()
		Me.TestToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
		Me.Test1ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
		Me.lblderzeitigeabfrage = New System.Windows.Forms.Label()
		Me.txt_SQLQuery = New DevExpress.XtraEditors.MemoEdit()
		Me.lblAbfrage = New System.Windows.Forms.Label()
		Me.txt_IndSQLQuery = New DevExpress.XtraEditors.MemoEdit()
		Me.Cbo_MwSt = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.Cbo_BuKonto = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.lblMwSt = New System.Windows.Forms.Label()
		Me.lblBuchungskonto = New System.Windows.Forms.Label()
		Me.lblValutadatum = New System.Windows.Forms.Label()
		Me.lblNummer = New System.Windows.Forms.Label()
		Me.lblBuchungsdatum = New System.Windows.Forms.Label()
		Me.lblkundennr = New System.Windows.Forms.Label()
		Me.lblRENr = New System.Windows.Forms.Label()
		Me.CboSort = New DevExpress.XtraEditors.CheckedComboBoxEdit()
		Me.ChkBeraterExakt = New System.Windows.Forms.CheckBox()
		Me.ChkKst2Exakt = New System.Windows.Forms.CheckBox()
		Me.ChkKst1Exakt = New System.Windows.Forms.CheckBox()
		Me.txtBUBetrag2 = New DevExpress.XtraEditors.TextEdit()
		Me.Cbo_KST1 = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.Cbo_KST2 = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.Cbo_Berater = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.lblBerater = New System.Windows.Forms.Label()
		Me.Cbo_Filiale = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.lblfiliale = New System.Windows.Forms.Label()
		Me.Cbo_REArt = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.lblRechnungsart = New System.Windows.Forms.Label()
		Me.lblbuchungsbetrag = New System.Windows.Forms.Label()
		Me.lblsortierung = New System.Windows.Forms.Label()
		Me.txtBUBetrag1 = New DevExpress.XtraEditors.TextEdit()
		Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
		Me.xtabZESearch = New DevExpress.XtraTab.XtraTabControl()
		Me.xtabAllgemein = New DevExpress.XtraTab.XtraTabPage()
		Me.chkVerfalltageFromCreatedOn = New DevExpress.XtraEditors.CheckEdit()
		Me.SwitchButton3 = New DevComponents.DotNetBar.Controls.SwitchButton()
		Me.SwitchButton2 = New DevComponents.DotNetBar.Controls.SwitchButton()
		Me.SwitchButton1 = New DevComponents.DotNetBar.Controls.SwitchButton()
		Me.lueMandant = New DevExpress.XtraEditors.LookUpEdit()
		Me.lblMDName = New System.Windows.Forms.Label()
		Me.deValutaDate_2 = New DevExpress.XtraEditors.DateEdit()
		Me.deValutaDate_1 = New DevExpress.XtraEditors.DateEdit()
		Me.deBuchungDate_2 = New DevExpress.XtraEditors.DateEdit()
		Me.deBuchungDate_1 = New DevExpress.XtraEditors.DateEdit()
		Me.txtZENr1 = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.txtRENr1 = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.txtKDNr1 = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.txtZENr2 = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.txtRENr2 = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.txtKDNr2 = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.xtabErweitert = New DevExpress.XtraTab.XtraTabPage()
		Me.xtabSQLAbfrage = New DevExpress.XtraTab.XtraTabPage()
		Me.Label26 = New System.Windows.Forms.Label()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager()
		Me.Bar3 = New DevExpress.XtraBars.Bar()
		Me.bsiInfo = New DevExpress.XtraBars.BarStaticItem()
		Me.bbiSearch = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiClear = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiPrint = New DevExpress.XtraBars.BarLargeButtonItem()
		Me.bbiExport = New DevExpress.XtraBars.BarLargeButtonItem()
		Me.BarStaticItem1 = New DevExpress.XtraBars.BarStaticItem()
		Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
		CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupBox1.SuspendLayout()
		Me.ContextMenuStrip1.SuspendLayout()
		CType(Me.txt_SQLQuery.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txt_IndSQLQuery.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_MwSt.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_BuKonto.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.CboSort.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtBUBetrag2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_KST1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_KST2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_Berater.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_Filiale.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_REArt.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtBUBetrag1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.xtabZESearch, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.xtabZESearch.SuspendLayout()
		Me.xtabAllgemein.SuspendLayout()
		CType(Me.chkVerfalltageFromCreatedOn.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.deValutaDate_2.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.deValutaDate_2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.deValutaDate_1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.deValutaDate_1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.deBuchungDate_2.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.deBuchungDate_2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.deBuchungDate_1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.deBuchungDate_1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtZENr1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtRENr1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtKDNr1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtZENr2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtRENr2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtKDNr2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.xtabErweitert.SuspendLayout()
		Me.xtabSQLAbfrage.SuspendLayout()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'GroupBox1
		'
		Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.GroupBox1.Appearance.BackColor = System.Drawing.Color.White
		Me.GroupBox1.Appearance.Options.UseBackColor = True
		Me.GroupBox1.Controls.Add(Me.LblSetting)
		Me.GroupBox1.Controls.Add(Me.LblTimeValue)
		Me.GroupBox1.Controls.Add(Me.CmdClose)
		Me.GroupBox1.Controls.Add(Me.lblHeader2)
		Me.GroupBox1.Controls.Add(Me.lblHeader1)
		Me.GroupBox1.Location = New System.Drawing.Point(-1, -10)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(751, 93)
		Me.GroupBox1.TabIndex = 3
		'
		'LblSetting
		'
		Me.LblSetting.BackColor = System.Drawing.Color.Transparent
		Me.LblSetting.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.LblSetting.ForeColor = System.Drawing.SystemColors.HotTrack
		Me.LblSetting.Image = CType(resources.GetObject("LblSetting.Image"), System.Drawing.Image)
		Me.LblSetting.Location = New System.Drawing.Point(13, 16)
		Me.LblSetting.Name = "LblSetting"
		Me.LblSetting.Size = New System.Drawing.Size(72, 66)
		Me.LblSetting.TabIndex = 1000
		'
		'LblTimeValue
		'
		Me.LblTimeValue.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.LblTimeValue.BackColor = System.Drawing.Color.Transparent
		Me.LblTimeValue.Location = New System.Drawing.Point(439, 53)
		Me.LblTimeValue.Name = "LblTimeValue"
		Me.LblTimeValue.Size = New System.Drawing.Size(245, 13)
		Me.LblTimeValue.TabIndex = 14
		Me.LblTimeValue.Text = "Zeitangaben..."
		Me.LblTimeValue.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.LblTimeValue.Visible = False
		'
		'CmdClose
		'
		Me.CmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.CmdClose.Location = New System.Drawing.Point(598, 29)
		Me.CmdClose.Name = "CmdClose"
		Me.CmdClose.Size = New System.Drawing.Size(86, 24)
		Me.CmdClose.TabIndex = 999
		Me.CmdClose.Text = "Schliessen"
		'
		'lblHeader2
		'
		Me.lblHeader2.AutoSize = True
		Me.lblHeader2.BackColor = System.Drawing.Color.Transparent
		Me.lblHeader2.Location = New System.Drawing.Point(109, 53)
		Me.lblHeader2.Name = "lblHeader2"
		Me.lblHeader2.Size = New System.Drawing.Size(225, 13)
		Me.lblHeader2.TabIndex = 1
		Me.lblHeader2.Text = "Wählen Sie Ihre gewünschten Kriterien aus..."
		'
		'lblHeader1
		'
		Me.lblHeader1.AutoSize = True
		Me.lblHeader1.BackColor = System.Drawing.Color.Transparent
		Me.lblHeader1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblHeader1.Location = New System.Drawing.Point(91, 29)
		Me.lblHeader1.Name = "lblHeader1"
		Me.lblHeader1.Size = New System.Drawing.Size(244, 13)
		Me.lblHeader1.TabIndex = 0
		Me.lblHeader1.Text = "Erweiterte Suche nach Zahlungseingänge"
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
		'ContextMenuStrip1
		'
		Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TestToolStripMenuItem, Me.Test1ToolStripMenuItem})
		Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
		Me.ContextMenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
		Me.ContextMenuStrip1.Size = New System.Drawing.Size(100, 48)
		'
		'TestToolStripMenuItem
		'
		Me.TestToolStripMenuItem.Name = "TestToolStripMenuItem"
		Me.TestToolStripMenuItem.Size = New System.Drawing.Size(99, 22)
		Me.TestToolStripMenuItem.Text = "test"
		'
		'Test1ToolStripMenuItem
		'
		Me.Test1ToolStripMenuItem.Name = "Test1ToolStripMenuItem"
		Me.Test1ToolStripMenuItem.Size = New System.Drawing.Size(99, 22)
		Me.Test1ToolStripMenuItem.Text = "test1"
		'
		'lblderzeitigeabfrage
		'
		Me.lblderzeitigeabfrage.AutoSize = True
		Me.lblderzeitigeabfrage.Location = New System.Drawing.Point(29, 20)
		Me.lblderzeitigeabfrage.Name = "lblderzeitigeabfrage"
		Me.lblderzeitigeabfrage.Size = New System.Drawing.Size(154, 13)
		Me.lblderzeitigeabfrage.TabIndex = 73
		Me.lblderzeitigeabfrage.Text = "Ihre derzeitige Abfrage lautet:"
		'
		'txt_SQLQuery
		'
		Me.txt_SQLQuery.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txt_SQLQuery.Location = New System.Drawing.Point(29, 39)
		Me.txt_SQLQuery.Name = "txt_SQLQuery"
		Me.txt_SQLQuery.Size = New System.Drawing.Size(592, 366)
		Me.txt_SQLQuery.TabIndex = 1
		'
		'lblAbfrage
		'
		Me.lblAbfrage.AutoSize = True
		Me.lblAbfrage.Location = New System.Drawing.Point(29, 20)
		Me.lblAbfrage.Name = "lblAbfrage"
		Me.lblAbfrage.Size = New System.Drawing.Size(69, 13)
		Me.lblAbfrage.TabIndex = 74
		Me.lblAbfrage.Text = "Ihre Abfrage"
		'
		'txt_IndSQLQuery
		'
		Me.txt_IndSQLQuery.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txt_IndSQLQuery.Location = New System.Drawing.Point(29, 39)
		Me.txt_IndSQLQuery.Name = "txt_IndSQLQuery"
		Me.txt_IndSQLQuery.Size = New System.Drawing.Size(592, 366)
		Me.txt_IndSQLQuery.TabIndex = 2
		'
		'Cbo_MwSt
		'
		Me.Cbo_MwSt.Location = New System.Drawing.Point(148, 343)
		Me.Cbo_MwSt.Name = "Cbo_MwSt"
		Me.Cbo_MwSt.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_MwSt.Size = New System.Drawing.Size(138, 20)
		Me.Cbo_MwSt.TabIndex = 206
		'
		'Cbo_BuKonto
		'
		Me.Cbo_BuKonto.Location = New System.Drawing.Point(148, 316)
		Me.Cbo_BuKonto.Name = "Cbo_BuKonto"
		Me.Cbo_BuKonto.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_BuKonto.Size = New System.Drawing.Size(431, 20)
		Me.Cbo_BuKonto.TabIndex = 205
		'
		'lblMwSt
		'
		Me.lblMwSt.Location = New System.Drawing.Point(14, 347)
		Me.lblMwSt.Name = "lblMwSt"
		Me.lblMwSt.Size = New System.Drawing.Size(128, 13)
		Me.lblMwSt.TabIndex = 203
		Me.lblMwSt.Text = "MwSt."
		Me.lblMwSt.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblBuchungskonto
		'
		Me.lblBuchungskonto.Location = New System.Drawing.Point(14, 320)
		Me.lblBuchungskonto.Name = "lblBuchungskonto"
		Me.lblBuchungskonto.Size = New System.Drawing.Size(128, 13)
		Me.lblBuchungskonto.TabIndex = 201
		Me.lblBuchungskonto.Text = "Buchungskonto"
		Me.lblBuchungskonto.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblValutadatum
		'
		Me.lblValutadatum.Location = New System.Drawing.Point(14, 208)
		Me.lblValutadatum.Name = "lblValutadatum"
		Me.lblValutadatum.Size = New System.Drawing.Size(128, 13)
		Me.lblValutadatum.TabIndex = 192
		Me.lblValutadatum.Text = "Valutadatum"
		Me.lblValutadatum.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblNummer
		'
		Me.lblNummer.Location = New System.Drawing.Point(14, 123)
		Me.lblNummer.Name = "lblNummer"
		Me.lblNummer.Size = New System.Drawing.Size(128, 13)
		Me.lblNummer.TabIndex = 189
		Me.lblNummer.TabStop = True
		Me.lblNummer.Text = "Nummer"
		Me.lblNummer.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblBuchungsdatum
		'
		Me.lblBuchungsdatum.Location = New System.Drawing.Point(291, 208)
		Me.lblBuchungsdatum.Name = "lblBuchungsdatum"
		Me.lblBuchungsdatum.Size = New System.Drawing.Size(144, 13)
		Me.lblBuchungsdatum.TabIndex = 17
		Me.lblBuchungsdatum.Text = "Buchungsdatum"
		Me.lblBuchungsdatum.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblkundennr
		'
		Me.lblkundennr.Location = New System.Drawing.Point(14, 175)
		Me.lblkundennr.Name = "lblkundennr"
		Me.lblkundennr.Size = New System.Drawing.Size(128, 13)
		Me.lblkundennr.TabIndex = 96
		Me.lblkundennr.TabStop = True
		Me.lblkundennr.Text = "Kunden-Nr."
		Me.lblkundennr.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblRENr
		'
		Me.lblRENr.Location = New System.Drawing.Point(14, 149)
		Me.lblRENr.Name = "lblRENr"
		Me.lblRENr.Size = New System.Drawing.Size(128, 13)
		Me.lblRENr.TabIndex = 95
		Me.lblRENr.TabStop = True
		Me.lblRENr.Text = "Rechnung-Nr."
		Me.lblRENr.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'CboSort
		'
		Me.CboSort.Location = New System.Drawing.Point(148, 70)
		Me.CboSort.Name = "CboSort"
		Me.CboSort.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.CboSort.Size = New System.Drawing.Size(431, 20)
		Me.CboSort.TabIndex = 10
		'
		'ChkBeraterExakt
		'
		Me.ChkBeraterExakt.AutoSize = True
		Me.ChkBeraterExakt.Checked = True
		Me.ChkBeraterExakt.CheckState = System.Windows.Forms.CheckState.Checked
		Me.ChkBeraterExakt.Location = New System.Drawing.Point(924, 398)
		Me.ChkBeraterExakt.Name = "ChkBeraterExakt"
		Me.ChkBeraterExakt.Size = New System.Drawing.Size(15, 14)
		Me.ChkBeraterExakt.TabIndex = 93
		Me.ChkBeraterExakt.TabStop = False
		Me.ChkBeraterExakt.UseVisualStyleBackColor = True
		Me.ChkBeraterExakt.Visible = False
		'
		'ChkKst2Exakt
		'
		Me.ChkKst2Exakt.AutoSize = True
		Me.ChkKst2Exakt.Checked = True
		Me.ChkKst2Exakt.CheckState = System.Windows.Forms.CheckState.Checked
		Me.ChkKst2Exakt.Location = New System.Drawing.Point(923, 369)
		Me.ChkKst2Exakt.Name = "ChkKst2Exakt"
		Me.ChkKst2Exakt.Size = New System.Drawing.Size(15, 14)
		Me.ChkKst2Exakt.TabIndex = 92
		Me.ChkKst2Exakt.TabStop = False
		Me.ChkKst2Exakt.UseVisualStyleBackColor = True
		Me.ChkKst2Exakt.Visible = False
		'
		'ChkKst1Exakt
		'
		Me.ChkKst1Exakt.AutoSize = True
		Me.ChkKst1Exakt.Checked = True
		Me.ChkKst1Exakt.CheckState = System.Windows.Forms.CheckState.Checked
		Me.ChkKst1Exakt.Location = New System.Drawing.Point(923, 340)
		Me.ChkKst1Exakt.Name = "ChkKst1Exakt"
		Me.ChkKst1Exakt.Size = New System.Drawing.Size(15, 14)
		Me.ChkKst1Exakt.TabIndex = 0
		Me.ChkKst1Exakt.TabStop = False
		Me.ChkKst1Exakt.UseVisualStyleBackColor = True
		Me.ChkKst1Exakt.Visible = False
		'
		'txtBUBetrag2
		'
		Me.txtBUBetrag2.Location = New System.Drawing.Point(441, 289)
		Me.txtBUBetrag2.Name = "txtBUBetrag2"
		Me.txtBUBetrag2.Size = New System.Drawing.Size(138, 20)
		Me.txtBUBetrag2.TabIndex = 140
		'
		'Cbo_KST1
		'
		Me.Cbo_KST1.Location = New System.Drawing.Point(805, 340)
		Me.Cbo_KST1.Name = "Cbo_KST1"
		Me.Cbo_KST1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_KST1.Size = New System.Drawing.Size(114, 20)
		Me.Cbo_KST1.TabIndex = 150
		Me.Cbo_KST1.Visible = False
		'
		'Cbo_KST2
		'
		Me.Cbo_KST2.Location = New System.Drawing.Point(805, 367)
		Me.Cbo_KST2.Name = "Cbo_KST2"
		Me.Cbo_KST2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_KST2.Size = New System.Drawing.Size(114, 20)
		Me.Cbo_KST2.TabIndex = 160
		Me.Cbo_KST2.Visible = False
		'
		'Cbo_Berater
		'
		Me.Cbo_Berater.Location = New System.Drawing.Point(149, 385)
		Me.Cbo_Berater.Name = "Cbo_Berater"
		Me.Cbo_Berater.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_Berater.Size = New System.Drawing.Size(138, 20)
		Me.Cbo_Berater.TabIndex = 170
		'
		'lblBerater
		'
		Me.lblBerater.Location = New System.Drawing.Point(15, 389)
		Me.lblBerater.Name = "lblBerater"
		Me.lblBerater.Size = New System.Drawing.Size(128, 13)
		Me.lblBerater.TabIndex = 78
		Me.lblBerater.Text = "BeraterIn"
		Me.lblBerater.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'Cbo_Filiale
		'
		Me.Cbo_Filiale.Location = New System.Drawing.Point(441, 385)
		Me.Cbo_Filiale.Name = "Cbo_Filiale"
		Me.Cbo_Filiale.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_Filiale.Size = New System.Drawing.Size(138, 20)
		Me.Cbo_Filiale.TabIndex = 180
		'
		'lblfiliale
		'
		Me.lblfiliale.Location = New System.Drawing.Point(321, 388)
		Me.lblfiliale.Name = "lblfiliale"
		Me.lblfiliale.Size = New System.Drawing.Size(114, 13)
		Me.lblfiliale.TabIndex = 57
		Me.lblfiliale.Text = "Filiale"
		Me.lblfiliale.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'Cbo_REArt
		'
		Me.Cbo_REArt.Location = New System.Drawing.Point(441, 343)
		Me.Cbo_REArt.Name = "Cbo_REArt"
		Me.Cbo_REArt.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_REArt.Size = New System.Drawing.Size(138, 20)
		Me.Cbo_REArt.TabIndex = 190
		'
		'lblRechnungsart
		'
		Me.lblRechnungsart.Location = New System.Drawing.Point(321, 347)
		Me.lblRechnungsart.Name = "lblRechnungsart"
		Me.lblRechnungsart.Size = New System.Drawing.Size(114, 13)
		Me.lblRechnungsart.TabIndex = 21
		Me.lblRechnungsart.Text = "Rechnungsart"
		Me.lblRechnungsart.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblbuchungsbetrag
		'
		Me.lblbuchungsbetrag.Location = New System.Drawing.Point(14, 293)
		Me.lblbuchungsbetrag.Name = "lblbuchungsbetrag"
		Me.lblbuchungsbetrag.Size = New System.Drawing.Size(128, 13)
		Me.lblbuchungsbetrag.TabIndex = 16
		Me.lblbuchungsbetrag.Text = "Buchungsbetrag"
		Me.lblbuchungsbetrag.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblsortierung
		'
		Me.lblsortierung.Location = New System.Drawing.Point(14, 74)
		Me.lblsortierung.Name = "lblsortierung"
		Me.lblsortierung.Size = New System.Drawing.Size(128, 13)
		Me.lblsortierung.TabIndex = 13
		Me.lblsortierung.Text = "Sortieren nach"
		Me.lblsortierung.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'txtBUBetrag1
		'
		Me.txtBUBetrag1.Location = New System.Drawing.Point(148, 289)
		Me.txtBUBetrag1.Name = "txtBUBetrag1"
		Me.txtBUBetrag1.Size = New System.Drawing.Size(138, 20)
		Me.txtBUBetrag1.TabIndex = 130
		'
		'BackgroundWorker1
		'
		'
		'xtabZESearch
		'
		Me.xtabZESearch.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.xtabZESearch.Location = New System.Drawing.Point(29, 109)
		Me.xtabZESearch.Name = "xtabZESearch"
		Me.xtabZESearch.SelectedTabPage = Me.xtabAllgemein
		Me.xtabZESearch.Size = New System.Drawing.Size(659, 470)
		Me.xtabZESearch.TabIndex = 231
		Me.xtabZESearch.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabAllgemein, Me.xtabErweitert, Me.xtabSQLAbfrage})
		'
		'xtabAllgemein
		'
		Me.xtabAllgemein.Controls.Add(Me.chkVerfalltageFromCreatedOn)
		Me.xtabAllgemein.Controls.Add(Me.SwitchButton3)
		Me.xtabAllgemein.Controls.Add(Me.SwitchButton2)
		Me.xtabAllgemein.Controls.Add(Me.SwitchButton1)
		Me.xtabAllgemein.Controls.Add(Me.lueMandant)
		Me.xtabAllgemein.Controls.Add(Me.lblMDName)
		Me.xtabAllgemein.Controls.Add(Me.deValutaDate_2)
		Me.xtabAllgemein.Controls.Add(Me.deValutaDate_1)
		Me.xtabAllgemein.Controls.Add(Me.deBuchungDate_2)
		Me.xtabAllgemein.Controls.Add(Me.Cbo_MwSt)
		Me.xtabAllgemein.Controls.Add(Me.deBuchungDate_1)
		Me.xtabAllgemein.Controls.Add(Me.lblsortierung)
		Me.xtabAllgemein.Controls.Add(Me.Cbo_BuKonto)
		Me.xtabAllgemein.Controls.Add(Me.lblMwSt)
		Me.xtabAllgemein.Controls.Add(Me.lblBuchungskonto)
		Me.xtabAllgemein.Controls.Add(Me.Cbo_Filiale)
		Me.xtabAllgemein.Controls.Add(Me.lblfiliale)
		Me.xtabAllgemein.Controls.Add(Me.txtBUBetrag1)
		Me.xtabAllgemein.Controls.Add(Me.lblbuchungsbetrag)
		Me.xtabAllgemein.Controls.Add(Me.lblValutadatum)
		Me.xtabAllgemein.Controls.Add(Me.lblRechnungsart)
		Me.xtabAllgemein.Controls.Add(Me.Cbo_REArt)
		Me.xtabAllgemein.Controls.Add(Me.lblNummer)
		Me.xtabAllgemein.Controls.Add(Me.lblBerater)
		Me.xtabAllgemein.Controls.Add(Me.lblBuchungsdatum)
		Me.xtabAllgemein.Controls.Add(Me.Cbo_Berater)
		Me.xtabAllgemein.Controls.Add(Me.txtBUBetrag2)
		Me.xtabAllgemein.Controls.Add(Me.lblkundennr)
		Me.xtabAllgemein.Controls.Add(Me.lblRENr)
		Me.xtabAllgemein.Controls.Add(Me.CboSort)
		Me.xtabAllgemein.Controls.Add(Me.txtZENr1)
		Me.xtabAllgemein.Controls.Add(Me.txtRENr1)
		Me.xtabAllgemein.Controls.Add(Me.txtKDNr1)
		Me.xtabAllgemein.Controls.Add(Me.txtZENr2)
		Me.xtabAllgemein.Controls.Add(Me.txtRENr2)
		Me.xtabAllgemein.Controls.Add(Me.txtKDNr2)
		Me.xtabAllgemein.Name = "xtabAllgemein"
		Me.xtabAllgemein.Size = New System.Drawing.Size(653, 442)
		Me.xtabAllgemein.Text = "Allgemein"
		'
		'chkVerfalltageFromCreatedOn
		'
		Me.chkVerfalltageFromCreatedOn.Location = New System.Drawing.Point(149, 255)
		Me.chkVerfalltageFromCreatedOn.Name = "chkVerfalltageFromCreatedOn"
		Me.chkVerfalltageFromCreatedOn.Properties.AllowFocused = False
		Me.chkVerfalltageFromCreatedOn.Properties.Caption = "Verfalltage ab Erstellungsdatum berechnen"
		Me.chkVerfalltageFromCreatedOn.Size = New System.Drawing.Size(286, 19)
		Me.chkVerfalltageFromCreatedOn.TabIndex = 256
		Me.chkVerfalltageFromCreatedOn.TabStop = False
		'
		'SwitchButton3
		'
		Me.SwitchButton3.BackColor = System.Drawing.Color.White
		'
		'
		'
		Me.SwitchButton3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
		Me.SwitchButton3.FocusCuesEnabled = False
		Me.SwitchButton3.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.SwitchButton3.Location = New System.Drawing.Point(397, 172)
		Me.SwitchButton3.Name = "SwitchButton3"
		Me.SwitchButton3.OffBackColor = System.Drawing.Color.White
		Me.SwitchButton3.OffText = "O"
		Me.SwitchButton3.OffTextColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(53, Byte), Integer))
		Me.SwitchButton3.OnBackColor = System.Drawing.Color.LightSteelBlue
		Me.SwitchButton3.OnText = "|"
		Me.SwitchButton3.OnTextColor = System.Drawing.Color.Black
		Me.SwitchButton3.Size = New System.Drawing.Size(38, 19)
		Me.SwitchButton3.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
		Me.SwitchButton3.SwitchBackColor = System.Drawing.Color.DarkGray
		Me.SwitchButton3.SwitchBorderColor = System.Drawing.Color.DarkGray
		Me.SwitchButton3.SwitchFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.SwitchButton3.SwitchWidth = 6
		Me.SwitchButton3.TabIndex = 255
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
		Me.SwitchButton2.Location = New System.Drawing.Point(397, 146)
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
		Me.SwitchButton2.TabIndex = 254
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
		Me.SwitchButton1.Location = New System.Drawing.Point(397, 120)
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
		Me.SwitchButton1.TabIndex = 253
		'
		'lueMandant
		'
		Me.lueMandant.Location = New System.Drawing.Point(148, 44)
		Me.lueMandant.Name = "lueMandant"
		Me.lueMandant.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.lueMandant.Properties.NullText = ""
		Me.lueMandant.Size = New System.Drawing.Size(431, 20)
		Me.lueMandant.TabIndex = 238
		'
		'lblMDName
		'
		Me.lblMDName.Location = New System.Drawing.Point(31, 48)
		Me.lblMDName.Name = "lblMDName"
		Me.lblMDName.Size = New System.Drawing.Size(111, 13)
		Me.lblMDName.TabIndex = 239
		Me.lblMDName.Text = "Mandanten"
		Me.lblMDName.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'deValutaDate_2
		'
		Me.deValutaDate_2.EditValue = Nothing
		Me.deValutaDate_2.Location = New System.Drawing.Point(148, 229)
		Me.deValutaDate_2.Name = "deValutaDate_2"
		Me.deValutaDate_2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.deValutaDate_2.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.deValutaDate_2.Size = New System.Drawing.Size(84, 20)
		Me.deValutaDate_2.TabIndex = 235
		'
		'deValutaDate_1
		'
		Me.deValutaDate_1.EditValue = ""
		Me.deValutaDate_1.Location = New System.Drawing.Point(148, 205)
		Me.deValutaDate_1.Name = "deValutaDate_1"
		Me.deValutaDate_1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.deValutaDate_1.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.deValutaDate_1.Size = New System.Drawing.Size(84, 20)
		Me.deValutaDate_1.TabIndex = 234
		'
		'deBuchungDate_2
		'
		Me.deBuchungDate_2.EditValue = Nothing
		Me.deBuchungDate_2.Location = New System.Drawing.Point(441, 229)
		Me.deBuchungDate_2.Name = "deBuchungDate_2"
		Me.deBuchungDate_2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.deBuchungDate_2.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.deBuchungDate_2.Size = New System.Drawing.Size(84, 20)
		Me.deBuchungDate_2.TabIndex = 237
		'
		'deBuchungDate_1
		'
		Me.deBuchungDate_1.EditValue = ""
		Me.deBuchungDate_1.Location = New System.Drawing.Point(441, 205)
		Me.deBuchungDate_1.Name = "deBuchungDate_1"
		Me.deBuchungDate_1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.deBuchungDate_1.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.deBuchungDate_1.Size = New System.Drawing.Size(84, 20)
		Me.deBuchungDate_1.TabIndex = 236
		'
		'txtZENr1
		'
		Me.txtZENr1.Location = New System.Drawing.Point(148, 119)
		Me.txtZENr1.Name = "txtZENr1"
		Me.txtZENr1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.txtZENr1.Size = New System.Drawing.Size(138, 20)
		Me.txtZENr1.TabIndex = 20
		'
		'txtRENr1
		'
		Me.txtRENr1.Location = New System.Drawing.Point(148, 145)
		Me.txtRENr1.Name = "txtRENr1"
		Me.txtRENr1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.txtRENr1.Size = New System.Drawing.Size(138, 20)
		Me.txtRENr1.TabIndex = 40
		'
		'txtKDNr1
		'
		Me.txtKDNr1.Location = New System.Drawing.Point(148, 171)
		Me.txtKDNr1.Name = "txtKDNr1"
		Me.txtKDNr1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.txtKDNr1.Size = New System.Drawing.Size(138, 20)
		Me.txtKDNr1.TabIndex = 60
		'
		'txtZENr2
		'
		Me.txtZENr2.Location = New System.Drawing.Point(441, 119)
		Me.txtZENr2.Name = "txtZENr2"
		Me.txtZENr2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.txtZENr2.Size = New System.Drawing.Size(138, 20)
		Me.txtZENr2.TabIndex = 30
		Me.txtZENr2.Visible = False
		'
		'txtRENr2
		'
		Me.txtRENr2.Location = New System.Drawing.Point(441, 145)
		Me.txtRENr2.Name = "txtRENr2"
		Me.txtRENr2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.txtRENr2.Size = New System.Drawing.Size(138, 20)
		Me.txtRENr2.TabIndex = 50
		Me.txtRENr2.Visible = False
		'
		'txtKDNr2
		'
		Me.txtKDNr2.Location = New System.Drawing.Point(441, 171)
		Me.txtKDNr2.Name = "txtKDNr2"
		Me.txtKDNr2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.txtKDNr2.Size = New System.Drawing.Size(138, 20)
		Me.txtKDNr2.TabIndex = 70
		Me.txtKDNr2.Visible = False
		'
		'xtabErweitert
		'
		Me.xtabErweitert.Controls.Add(Me.lblAbfrage)
		Me.xtabErweitert.Controls.Add(Me.txt_IndSQLQuery)
		Me.xtabErweitert.Name = "xtabErweitert"
		Me.xtabErweitert.Size = New System.Drawing.Size(653, 442)
		Me.xtabErweitert.Text = "Erweiterte Abfrage"
		'
		'xtabSQLAbfrage
		'
		Me.xtabSQLAbfrage.Controls.Add(Me.lblderzeitigeabfrage)
		Me.xtabSQLAbfrage.Controls.Add(Me.txt_SQLQuery)
		Me.xtabSQLAbfrage.Name = "xtabSQLAbfrage"
		Me.xtabSQLAbfrage.Size = New System.Drawing.Size(653, 442)
		Me.xtabSQLAbfrage.Text = "SQL-Abfrage"
		'
		'Label26
		'
		Me.Label26.Location = New System.Drawing.Point(883, 322)
		Me.Label26.Name = "Label26"
		Me.Label26.Size = New System.Drawing.Size(93, 13)
		Me.Label26.TabIndex = 240
		Me.Label26.Text = "Exakt"
		Me.Label26.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me.Label26.Visible = False
		'
		'BarManager1
		'
		Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar3})
		Me.BarManager1.DockControls.Add(Me.barDockControlTop)
		Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
		Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
		Me.BarManager1.DockControls.Add(Me.barDockControlRight)
		Me.BarManager1.Form = Me
		Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiInfo, Me.bbiSearch, Me.bbiClear, Me.bbiPrint, Me.bbiExport, Me.BarStaticItem1})
		Me.BarManager1.MaxItemId = 10
		Me.BarManager1.StatusBar = Me.Bar3
		'
		'Bar3
		'
		Me.Bar3.BarName = "Statusleiste"
		Me.Bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
		Me.Bar3.DockCol = 0
		Me.Bar3.DockRow = 0
		Me.Bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
		Me.Bar3.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiInfo), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiSearch), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiClear), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiPrint, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiExport, True), New DevExpress.XtraBars.LinkPersistInfo(Me.BarStaticItem1)})
		Me.Bar3.OptionsBar.AllowQuickCustomization = False
		Me.Bar3.OptionsBar.DrawDragBorder = False
		Me.Bar3.OptionsBar.UseWholeRow = True
		Me.Bar3.Text = "Statusleiste"
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
		Me.bbiSearch.ImageOptions.LargeImage = CType(resources.GetObject("bbiSearch.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.bbiSearch.Name = "bbiSearch"
		Me.bbiSearch.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
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
		'BarStaticItem1
		'
		Me.BarStaticItem1.Id = 9
		Me.BarStaticItem1.Name = "BarStaticItem1"
		'
		'barDockControlTop
		'
		Me.barDockControlTop.CausesValidation = False
		Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
		Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlTop.Manager = Me.BarManager1
		Me.barDockControlTop.Size = New System.Drawing.Size(722, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 617)
		Me.barDockControlBottom.Manager = Me.BarManager1
		Me.barDockControlBottom.Size = New System.Drawing.Size(722, 27)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Manager = Me.BarManager1
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 617)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(722, 0)
		Me.barDockControlRight.Manager = Me.BarManager1
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 617)
		'
		'frmZESearch
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(722, 644)
		Me.Controls.Add(Me.xtabZESearch)
		Me.Controls.Add(Me.GroupBox1)
		Me.Controls.Add(Me.Label26)
		Me.Controls.Add(Me.ChkBeraterExakt)
		Me.Controls.Add(Me.ChkKst2Exakt)
		Me.Controls.Add(Me.ChkKst1Exakt)
		Me.Controls.Add(Me.Cbo_KST1)
		Me.Controls.Add(Me.Cbo_KST2)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.MinimumSize = New System.Drawing.Size(732, 664)
		Me.Name = "frmZESearch"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Erweiterte Suche nach Zahlungseingänge"
		CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupBox1.ResumeLayout(False)
		Me.GroupBox1.PerformLayout()
		Me.ContextMenuStrip1.ResumeLayout(False)
		CType(Me.txt_SQLQuery.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txt_IndSQLQuery.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_MwSt.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_BuKonto.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.CboSort.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtBUBetrag2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_KST1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_KST2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_Berater.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_Filiale.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_REArt.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtBUBetrag1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.xtabZESearch, System.ComponentModel.ISupportInitialize).EndInit()
		Me.xtabZESearch.ResumeLayout(False)
		Me.xtabAllgemein.ResumeLayout(False)
		CType(Me.chkVerfalltageFromCreatedOn.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.deValutaDate_2.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.deValutaDate_2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.deValutaDate_1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.deValutaDate_1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.deBuchungDate_2.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.deBuchungDate_2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.deBuchungDate_1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.deBuchungDate_1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtZENr1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtRENr1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtKDNr1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtZENr2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtRENr2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtKDNr2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.xtabErweitert.ResumeLayout(False)
		Me.xtabErweitert.PerformLayout()
		Me.xtabSQLAbfrage.ResumeLayout(False)
		Me.xtabSQLAbfrage.PerformLayout()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents GroupBox1 As DevExpress.XtraEditors.PanelControl
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
  Friend WithEvents CmdClose As DevExpress.XtraEditors.SimpleButton  ' System.Windows.Forms.Button
  Friend WithEvents LblTimeValue As System.Windows.Forms.Label
  Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
  Friend WithEvents TestToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
  Friend WithEvents Test1ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
  Friend WithEvents lblderzeitigeabfrage As System.Windows.Forms.Label
  Friend WithEvents txt_SQLQuery As DevExpress.XtraEditors.MemoEdit  ' System.Windows.Forms.TextBox
  Friend WithEvents lblAbfrage As System.Windows.Forms.Label
  Friend WithEvents txt_IndSQLQuery As DevExpress.XtraEditors.MemoEdit ' System.Windows.Forms.TextBox
  Friend WithEvents Cbo_Filiale As DevExpress.XtraEditors.ComboBoxEdit ' SPZESearch.myCbo
  Friend WithEvents lblfiliale As System.Windows.Forms.Label
  Friend WithEvents Cbo_REArt As DevExpress.XtraEditors.ComboBoxEdit ' SPZESearch.myCbo
  Friend WithEvents lblRechnungsart As System.Windows.Forms.Label
  Friend WithEvents lblBuchungsdatum As System.Windows.Forms.Label
  Friend WithEvents lblbuchungsbetrag As System.Windows.Forms.Label
  Friend WithEvents lblsortierung As System.Windows.Forms.Label
  Friend WithEvents txtBUBetrag1 As DevExpress.XtraEditors.TextEdit ' System.Windows.Forms.TextBox
  ' System.Windows.Forms.TextBox
  ' System.Windows.Forms.TextBox
  ' System.Windows.Forms.TextBox
  ' System.Windows.Forms.TextBox
  Friend WithEvents Cbo_Berater As DevExpress.XtraEditors.ComboBoxEdit ' SPZESearch.myCbo
  Friend WithEvents lblBerater As System.Windows.Forms.Label
  Friend WithEvents txtBUBetrag2 As DevExpress.XtraEditors.TextEdit ' System.Windows.Forms.TextBox
  Friend WithEvents Cbo_KST1 As DevExpress.XtraEditors.ComboBoxEdit ' SPZESearch.myCbo
  Friend WithEvents Cbo_KST2 As DevExpress.XtraEditors.ComboBoxEdit ' SPZESearch.myCbo
  Friend WithEvents ChkKst1Exakt As System.Windows.Forms.CheckBox
  Friend WithEvents ChkBeraterExakt As System.Windows.Forms.CheckBox
  Friend WithEvents ChkKst2Exakt As System.Windows.Forms.CheckBox
  Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
  Friend WithEvents CboSort As DevExpress.XtraEditors.CheckedComboBoxEdit
  Friend WithEvents lblRENr As System.Windows.Forms.Label
  Friend WithEvents lblkundennr As System.Windows.Forms.Label
  Friend WithEvents lblNummer As System.Windows.Forms.Label
  ' System.Windows.Forms.TextBox
  ' System.Windows.Forms.TextBox
  Friend WithEvents lblValutadatum As System.Windows.Forms.Label
  Friend WithEvents lblMwSt As System.Windows.Forms.Label
  Friend WithEvents lblBuchungskonto As System.Windows.Forms.Label
  Friend WithEvents Cbo_BuKonto As DevExpress.XtraEditors.ComboBoxEdit ' SPZESearch.myCbo
  Friend WithEvents Cbo_MwSt As DevExpress.XtraEditors.ComboBoxEdit ' SPZESearch.myCbo
  Friend WithEvents LblSetting As System.Windows.Forms.Label
  Friend WithEvents xtabZESearch As DevExpress.XtraTab.XtraTabControl
  Friend WithEvents xtabAllgemein As DevExpress.XtraTab.XtraTabPage
  Friend WithEvents xtabErweitert As DevExpress.XtraTab.XtraTabPage
  Friend WithEvents xtabSQLAbfrage As DevExpress.XtraTab.XtraTabPage
  Friend WithEvents deValutaDate_2 As DevExpress.XtraEditors.DateEdit
  Friend WithEvents deValutaDate_1 As DevExpress.XtraEditors.DateEdit
  Friend WithEvents deBuchungDate_2 As DevExpress.XtraEditors.DateEdit
  Friend WithEvents deBuchungDate_1 As DevExpress.XtraEditors.DateEdit
  Friend WithEvents lueMandant As DevExpress.XtraEditors.LookUpEdit
  Friend WithEvents lblMDName As System.Windows.Forms.Label
  Friend WithEvents Label26 As System.Windows.Forms.Label
  Friend WithEvents SwitchButton1 As DevComponents.DotNetBar.Controls.SwitchButton
  Friend WithEvents SwitchButton3 As DevComponents.DotNetBar.Controls.SwitchButton
  Friend WithEvents SwitchButton2 As DevComponents.DotNetBar.Controls.SwitchButton
  Friend WithEvents txtZENr1 As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents txtRENr1 As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents txtKDNr1 As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents txtZENr2 As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents txtRENr2 As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents txtKDNr2 As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
  Friend WithEvents Bar3 As DevExpress.XtraBars.Bar
  Friend WithEvents bsiInfo As DevExpress.XtraBars.BarStaticItem
  Friend WithEvents bbiSearch As DevExpress.XtraBars.BarButtonItem
  Friend WithEvents bbiClear As DevExpress.XtraBars.BarButtonItem
  Friend WithEvents bbiPrint As DevExpress.XtraBars.BarLargeButtonItem
  Friend WithEvents bbiExport As DevExpress.XtraBars.BarLargeButtonItem
  Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
	Friend WithEvents BarStaticItem1 As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents chkVerfalltageFromCreatedOn As DevExpress.XtraEditors.CheckEdit

End Class
