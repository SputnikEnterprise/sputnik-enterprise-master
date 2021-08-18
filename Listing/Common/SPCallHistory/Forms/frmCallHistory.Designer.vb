<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCallHistory
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCallHistory))
		Me.GroupBox1 = New System.Windows.Forms.GroupBox()
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
		Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
		Me.lblderzeitigeabfrage = New System.Windows.Forms.Label()
		Me.txt_SQLQuery = New DevExpress.XtraEditors.MemoEdit()
		Me.lblAbfrage = New System.Windows.Forms.Label()
		Me.txt_IndSQLQuery = New DevExpress.XtraEditors.MemoEdit()
		Me.Cbo_MA_Kontakt = New DevExpress.XtraEditors.TextEdit()
		Me.lblbezeichnung = New System.Windows.Forms.Label()
		Me.lblkontaktart = New System.Windows.Forms.Label()
		Me.lblperiode2 = New System.Windows.Forms.Label()
		Me.lblberater2 = New System.Windows.Forms.Label()
		Me.lbldatumzwischen2 = New System.Windows.Forms.Label()
		Me.Cbo_MA_Art = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.Cbo_MA_DateTpl = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.Cbo_MA_KST = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.Cbo_ESAktivImSelektion = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.lblperiode = New System.Windows.Forms.Label()
		Me.Cbo_Berater = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.lblBerater = New System.Windows.Forms.Label()
		Me.lblDatumzwischen = New System.Windows.Forms.Label()
		Me.Cbo_LstArt = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.lbllistart = New System.Windows.Forms.Label()
		Me.CboSort = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.lblSort1 = New System.Windows.Forms.Label()
		Me.XtraTabControl1 = New DevExpress.XtraTab.XtraTabControl()
		Me.xtabAllgemein = New DevExpress.XtraTab.XtraTabPage()
		Me.deCallAt_2 = New DevExpress.XtraEditors.DateEdit()
		Me.deCallAt_1 = New DevExpress.XtraEditors.DateEdit()
		Me.xtabKontakt = New DevExpress.XtraTab.XtraTabPage()
		Me.lblSort2 = New System.Windows.Forms.Label()
		Me.cboContactSort = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.sbKontaktDb = New DevComponents.DotNetBar.Controls.SwitchButton()
		Me.deMAKontakt_2 = New DevExpress.XtraEditors.DateEdit()
		Me.deMAKontakt_1 = New DevExpress.XtraEditors.DateEdit()
		Me.xtabTODO = New DevExpress.XtraTab.XtraTabPage()
		Me.DateEdit1 = New DevExpress.XtraEditors.DateEdit()
		Me.DateEdit2 = New DevExpress.XtraEditors.DateEdit()
		Me.ComboBoxEdit1 = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.ComboBoxEdit2 = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.ComboBoxEdit3 = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.Label3 = New System.Windows.Forms.Label()
		Me.Label4 = New System.Windows.Forms.Label()
		Me.ComboBoxEdit4 = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.Label5 = New System.Windows.Forms.Label()
		Me.xtabErweitert = New DevExpress.XtraTab.XtraTabPage()
		Me.xtabSQLQuery = New DevExpress.XtraTab.XtraTabPage()
		Me.lblMDName = New System.Windows.Forms.Label()
		Me.Bar3 = New DevExpress.XtraBars.Bar()
		Me.bsiInfo = New DevExpress.XtraBars.BarStaticItem()
		Me.bbiSearch = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiClear = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiPrint = New DevExpress.XtraBars.BarLargeButtonItem()
		Me.bbiExport = New DevExpress.XtraBars.BarLargeButtonItem()
		Me.BarStaticItem1 = New DevExpress.XtraBars.BarStaticItem()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
		Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
		Me.lueMandant = New DevExpress.XtraEditors.CheckedComboBoxEdit()
		Me.GroupBox1.SuspendLayout()
		CType(Me.txt_SQLQuery.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txt_IndSQLQuery.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_MA_Kontakt.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_MA_Art.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_MA_DateTpl.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_MA_KST.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_ESAktivImSelektion.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_Berater.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_LstArt.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.CboSort.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.XtraTabControl1.SuspendLayout()
		Me.xtabAllgemein.SuspendLayout()
		CType(Me.deCallAt_2.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.deCallAt_2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.deCallAt_1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.deCallAt_1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.xtabKontakt.SuspendLayout()
		CType(Me.cboContactSort.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.deMAKontakt_2.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.deMAKontakt_2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.deMAKontakt_1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.deMAKontakt_1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.xtabTODO.SuspendLayout()
		CType(Me.DateEdit1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.DateEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.DateEdit2.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.DateEdit2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.ComboBoxEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.ComboBoxEdit2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.ComboBoxEdit3.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.ComboBoxEdit4.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.xtabErweitert.SuspendLayout()
		Me.xtabSQLQuery.SuspendLayout()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
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
		Me.GroupBox1.Controls.Add(Me.lblHeader2)
		Me.GroupBox1.Controls.Add(Me.lblHeader1)
		Me.GroupBox1.Location = New System.Drawing.Point(-1, -10)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(775, 93)
		Me.GroupBox1.TabIndex = 3
		Me.GroupBox1.TabStop = False
		'
		'LblSetting
		'
		Me.LblSetting.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.LblSetting.ForeColor = System.Drawing.SystemColors.HotTrack
		Me.LblSetting.Image = CType(resources.GetObject("LblSetting.Image"), System.Drawing.Image)
		Me.LblSetting.Location = New System.Drawing.Point(13, 17)
		Me.LblSetting.Name = "LblSetting"
		Me.LblSetting.Size = New System.Drawing.Size(83, 65)
		Me.LblSetting.TabIndex = 15
		'
		'LblTimeValue
		'
		Me.LblTimeValue.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.LblTimeValue.Location = New System.Drawing.Point(500, 56)
		Me.LblTimeValue.Name = "LblTimeValue"
		Me.LblTimeValue.Size = New System.Drawing.Size(214, 13)
		Me.LblTimeValue.TabIndex = 14
		Me.LblTimeValue.Text = "Zeitangaben..."
		Me.LblTimeValue.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.LblTimeValue.Visible = False
		'
		'CmdClose
		'
		Me.CmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.CmdClose.Location = New System.Drawing.Point(628, 29)
		Me.CmdClose.Name = "CmdClose"
		Me.CmdClose.Size = New System.Drawing.Size(86, 24)
		Me.CmdClose.TabIndex = 5
		Me.CmdClose.Text = "Schliessen"
		'
		'lblHeader2
		'
		Me.lblHeader2.AutoSize = True
		Me.lblHeader2.Location = New System.Drawing.Point(120, 53)
		Me.lblHeader2.Name = "lblHeader2"
		Me.lblHeader2.Size = New System.Drawing.Size(225, 13)
		Me.lblHeader2.TabIndex = 1
		Me.lblHeader2.Text = "Wählen Sie Ihre gewünschten Kriterien aus..."
		'
		'lblHeader1
		'
		Me.lblHeader1.AutoSize = True
		Me.lblHeader1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblHeader1.Location = New System.Drawing.Point(102, 29)
		Me.lblHeader1.Name = "lblHeader1"
		Me.lblHeader1.Size = New System.Drawing.Size(292, 13)
		Me.lblHeader1.TabIndex = 0
		Me.lblHeader1.Text = "Erweiterte Suche nach Telefon- und Kontaktdaten"
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
		'lblderzeitigeabfrage
		'
		Me.lblderzeitigeabfrage.AutoSize = True
		Me.lblderzeitigeabfrage.Location = New System.Drawing.Point(31, 21)
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
		Me.txt_SQLQuery.Location = New System.Drawing.Point(31, 40)
		Me.txt_SQLQuery.Name = "txt_SQLQuery"
		Me.txt_SQLQuery.Size = New System.Drawing.Size(621, 231)
		Me.txt_SQLQuery.TabIndex = 1
		'
		'lblAbfrage
		'
		Me.lblAbfrage.AutoSize = True
		Me.lblAbfrage.Location = New System.Drawing.Point(31, 21)
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
		Me.txt_IndSQLQuery.Location = New System.Drawing.Point(31, 40)
		Me.txt_IndSQLQuery.Name = "txt_IndSQLQuery"
		Me.txt_IndSQLQuery.Size = New System.Drawing.Size(621, 231)
		Me.txt_IndSQLQuery.TabIndex = 2
		'
		'Cbo_MA_Kontakt
		'
		Me.Cbo_MA_Kontakt.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.Cbo_MA_Kontakt.Location = New System.Drawing.Point(144, 238)
		Me.Cbo_MA_Kontakt.Name = "Cbo_MA_Kontakt"
		Me.Cbo_MA_Kontakt.Size = New System.Drawing.Size(359, 20)
		Me.Cbo_MA_Kontakt.TabIndex = 189
		'
		'lblbezeichnung
		'
		Me.lblbezeichnung.Location = New System.Drawing.Point(14, 241)
		Me.lblbezeichnung.Name = "lblbezeichnung"
		Me.lblbezeichnung.Size = New System.Drawing.Size(124, 13)
		Me.lblbezeichnung.TabIndex = 188
		Me.lblbezeichnung.Text = "Beschreibung"
		Me.lblbezeichnung.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblkontaktart
		'
		Me.lblkontaktart.Location = New System.Drawing.Point(14, 96)
		Me.lblkontaktart.Name = "lblkontaktart"
		Me.lblkontaktart.Size = New System.Drawing.Size(124, 13)
		Me.lblkontaktart.TabIndex = 186
		Me.lblkontaktart.Text = "Kontaktart"
		Me.lblkontaktart.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblperiode2
		'
		Me.lblperiode2.Location = New System.Drawing.Point(14, 165)
		Me.lblperiode2.Name = "lblperiode2"
		Me.lblperiode2.Size = New System.Drawing.Size(124, 13)
		Me.lblperiode2.TabIndex = 178
		Me.lblperiode2.Text = "Periode"
		Me.lblperiode2.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblberater2
		'
		Me.lblberater2.Location = New System.Drawing.Point(14, 121)
		Me.lblberater2.Name = "lblberater2"
		Me.lblberater2.Size = New System.Drawing.Size(124, 13)
		Me.lblberater2.TabIndex = 185
		Me.lblberater2.Text = "BeraterIn"
		Me.lblberater2.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lbldatumzwischen2
		'
		Me.lbldatumzwischen2.Location = New System.Drawing.Point(14, 190)
		Me.lbldatumzwischen2.Name = "lbldatumzwischen2"
		Me.lbldatumzwischen2.Size = New System.Drawing.Size(124, 13)
		Me.lbldatumzwischen2.TabIndex = 175
		Me.lbldatumzwischen2.Text = "Datum zwischen"
		Me.lbldatumzwischen2.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'Cbo_MA_Art
		'
		Me.Cbo_MA_Art.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.Cbo_MA_Art.Location = New System.Drawing.Point(144, 92)
		Me.Cbo_MA_Art.Name = "Cbo_MA_Art"
		Me.Cbo_MA_Art.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_MA_Art.Size = New System.Drawing.Size(359, 20)
		Me.Cbo_MA_Art.TabIndex = 187
		'
		'Cbo_MA_DateTpl
		'
		Me.Cbo_MA_DateTpl.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.Cbo_MA_DateTpl.Location = New System.Drawing.Point(144, 160)
		Me.Cbo_MA_DateTpl.Name = "Cbo_MA_DateTpl"
		Me.Cbo_MA_DateTpl.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_MA_DateTpl.Size = New System.Drawing.Size(159, 20)
		Me.Cbo_MA_DateTpl.TabIndex = 180
		'
		'Cbo_MA_KST
		'
		Me.Cbo_MA_KST.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.Cbo_MA_KST.Location = New System.Drawing.Point(144, 118)
		Me.Cbo_MA_KST.Name = "Cbo_MA_KST"
		Me.Cbo_MA_KST.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_MA_KST.Size = New System.Drawing.Size(159, 20)
		Me.Cbo_MA_KST.TabIndex = 179
		'
		'Cbo_ESAktivImSelektion
		'
		Me.Cbo_ESAktivImSelektion.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.Cbo_ESAktivImSelektion.Location = New System.Drawing.Point(144, 136)
		Me.Cbo_ESAktivImSelektion.Name = "Cbo_ESAktivImSelektion"
		Me.Cbo_ESAktivImSelektion.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_ESAktivImSelektion.Size = New System.Drawing.Size(231, 20)
		Me.Cbo_ESAktivImSelektion.TabIndex = 2
		'
		'lblperiode
		'
		Me.lblperiode.Location = New System.Drawing.Point(24, 140)
		Me.lblperiode.Name = "lblperiode"
		Me.lblperiode.Size = New System.Drawing.Size(114, 13)
		Me.lblperiode.TabIndex = 0
		Me.lblperiode.Text = "Periode"
		Me.lblperiode.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'Cbo_Berater
		'
		Me.Cbo_Berater.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.Cbo_Berater.Location = New System.Drawing.Point(144, 94)
		Me.Cbo_Berater.Name = "Cbo_Berater"
		Me.Cbo_Berater.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_Berater.Size = New System.Drawing.Size(231, 20)
		Me.Cbo_Berater.TabIndex = 1
		'
		'lblBerater
		'
		Me.lblBerater.Location = New System.Drawing.Point(24, 97)
		Me.lblBerater.Name = "lblBerater"
		Me.lblBerater.Size = New System.Drawing.Size(114, 13)
		Me.lblBerater.TabIndex = 159
		Me.lblBerater.Text = "BeraterIn"
		Me.lblBerater.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblDatumzwischen
		'
		Me.lblDatumzwischen.Location = New System.Drawing.Point(24, 166)
		Me.lblDatumzwischen.Name = "lblDatumzwischen"
		Me.lblDatumzwischen.Size = New System.Drawing.Size(114, 13)
		Me.lblDatumzwischen.TabIndex = 0
		Me.lblDatumzwischen.Text = "Datum zwischen"
		Me.lblDatumzwischen.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'Cbo_LstArt
		'
		Me.Cbo_LstArt.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.Cbo_LstArt.Location = New System.Drawing.Point(144, 68)
		Me.Cbo_LstArt.Name = "Cbo_LstArt"
		Me.Cbo_LstArt.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_LstArt.Size = New System.Drawing.Size(431, 20)
		Me.Cbo_LstArt.TabIndex = 0
		'
		'lbllistart
		'
		Me.lbllistart.Location = New System.Drawing.Point(24, 71)
		Me.lbllistart.Name = "lbllistart"
		Me.lbllistart.Size = New System.Drawing.Size(114, 13)
		Me.lbllistart.TabIndex = 0
		Me.lbllistart.Text = "Art der Liste"
		Me.lbllistart.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'CboSort
		'
		Me.CboSort.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.CboSort.Location = New System.Drawing.Point(144, 42)
		Me.CboSort.Name = "CboSort"
		Me.CboSort.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.CboSort.Size = New System.Drawing.Size(431, 20)
		Me.CboSort.TabIndex = 1
		'
		'lblSort1
		'
		Me.lblSort1.Location = New System.Drawing.Point(24, 46)
		Me.lblSort1.Name = "lblSort1"
		Me.lblSort1.Size = New System.Drawing.Size(114, 13)
		Me.lblSort1.TabIndex = 0
		Me.lblSort1.Text = "Sortieren nach"
		Me.lblSort1.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'XtraTabControl1
		'
		Me.XtraTabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.XtraTabControl1.Location = New System.Drawing.Point(29, 145)
		Me.XtraTabControl1.Name = "XtraTabControl1"
		Me.XtraTabControl1.SelectedTabPage = Me.xtabAllgemein
		Me.XtraTabControl1.Size = New System.Drawing.Size(684, 322)
		Me.XtraTabControl1.TabIndex = 4
		Me.XtraTabControl1.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabAllgemein, Me.xtabKontakt, Me.xtabTODO, Me.xtabErweitert, Me.xtabSQLQuery})
		'
		'xtabAllgemein
		'
		Me.xtabAllgemein.Controls.Add(Me.deCallAt_2)
		Me.xtabAllgemein.Controls.Add(Me.deCallAt_1)
		Me.xtabAllgemein.Controls.Add(Me.Cbo_ESAktivImSelektion)
		Me.xtabAllgemein.Controls.Add(Me.lblperiode)
		Me.xtabAllgemein.Controls.Add(Me.lblSort1)
		Me.xtabAllgemein.Controls.Add(Me.Cbo_Berater)
		Me.xtabAllgemein.Controls.Add(Me.CboSort)
		Me.xtabAllgemein.Controls.Add(Me.lblBerater)
		Me.xtabAllgemein.Controls.Add(Me.lbllistart)
		Me.xtabAllgemein.Controls.Add(Me.Cbo_LstArt)
		Me.xtabAllgemein.Controls.Add(Me.lblDatumzwischen)
		Me.xtabAllgemein.Name = "xtabAllgemein"
		Me.xtabAllgemein.Size = New System.Drawing.Size(682, 297)
		Me.xtabAllgemein.Text = "Allgemein"
		'
		'deCallAt_2
		'
		Me.deCallAt_2.EditValue = Nothing
		Me.deCallAt_2.Location = New System.Drawing.Point(144, 188)
		Me.deCallAt_2.Name = "deCallAt_2"
		Me.deCallAt_2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.deCallAt_2.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.deCallAt_2.Size = New System.Drawing.Size(98, 20)
		Me.deCallAt_2.TabIndex = 4
		'
		'deCallAt_1
		'
		Me.deCallAt_1.EditValue = Nothing
		Me.deCallAt_1.Location = New System.Drawing.Point(144, 162)
		Me.deCallAt_1.Name = "deCallAt_1"
		Me.deCallAt_1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.deCallAt_1.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.deCallAt_1.Size = New System.Drawing.Size(98, 20)
		Me.deCallAt_1.TabIndex = 3
		'
		'xtabKontakt
		'
		Me.xtabKontakt.Controls.Add(Me.lblSort2)
		Me.xtabKontakt.Controls.Add(Me.cboContactSort)
		Me.xtabKontakt.Controls.Add(Me.sbKontaktDb)
		Me.xtabKontakt.Controls.Add(Me.deMAKontakt_2)
		Me.xtabKontakt.Controls.Add(Me.deMAKontakt_1)
		Me.xtabKontakt.Controls.Add(Me.Cbo_MA_Kontakt)
		Me.xtabKontakt.Controls.Add(Me.lblbezeichnung)
		Me.xtabKontakt.Controls.Add(Me.Cbo_MA_KST)
		Me.xtabKontakt.Controls.Add(Me.lblkontaktart)
		Me.xtabKontakt.Controls.Add(Me.Cbo_MA_DateTpl)
		Me.xtabKontakt.Controls.Add(Me.lblperiode2)
		Me.xtabKontakt.Controls.Add(Me.Cbo_MA_Art)
		Me.xtabKontakt.Controls.Add(Me.lblberater2)
		Me.xtabKontakt.Controls.Add(Me.lbldatumzwischen2)
		Me.xtabKontakt.Name = "xtabKontakt"
		Me.xtabKontakt.Size = New System.Drawing.Size(682, 297)
		Me.xtabKontakt.Text = "Kontaktsuche zu den Kunden und Kandidaten"
		'
		'lblSort2
		'
		Me.lblSort2.Location = New System.Drawing.Point(14, 70)
		Me.lblSort2.Name = "lblSort2"
		Me.lblSort2.Size = New System.Drawing.Size(124, 13)
		Me.lblSort2.TabIndex = 256
		Me.lblSort2.Text = "Sortieren nach"
		Me.lblSort2.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'cboContactSort
		'
		Me.cboContactSort.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.cboContactSort.Location = New System.Drawing.Point(144, 66)
		Me.cboContactSort.Name = "cboContactSort"
		Me.cboContactSort.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.cboContactSort.Size = New System.Drawing.Size(359, 20)
		Me.cboContactSort.TabIndex = 257
		'
		'sbKontaktDb
		'
		Me.sbKontaktDb.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.sbKontaktDb.BackColor = System.Drawing.Color.White
		'
		'
		'
		Me.sbKontaktDb.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
		Me.sbKontaktDb.FocusCuesEnabled = False
		Me.sbKontaktDb.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.sbKontaktDb.Location = New System.Drawing.Point(144, 25)
		Me.sbKontaktDb.Name = "sbKontaktDb"
		Me.sbKontaktDb.OffBackColor = System.Drawing.Color.White
		Me.sbKontaktDb.OffText = "Kandidaten-Kontakte"
		Me.sbKontaktDb.OffTextColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(53, Byte), Integer))
		Me.sbKontaktDb.OnBackColor = System.Drawing.Color.LightSteelBlue
		Me.sbKontaktDb.OnText = "Kunden-Kontakte"
		Me.sbKontaktDb.OnTextColor = System.Drawing.Color.Black
		Me.sbKontaktDb.Size = New System.Drawing.Size(359, 26)
		Me.sbKontaktDb.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
		Me.sbKontaktDb.SwitchBackColor = System.Drawing.SystemColors.ControlDark
		Me.sbKontaktDb.SwitchBorderColor = System.Drawing.SystemColors.ControlDark
		Me.sbKontaktDb.SwitchFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.sbKontaktDb.SwitchWidth = 20
		Me.sbKontaktDb.TabIndex = 255
		Me.sbKontaktDb.Value = True
		Me.sbKontaktDb.ValueObject = "Y"
		'
		'deMAKontakt_2
		'
		Me.deMAKontakt_2.EditValue = Nothing
		Me.deMAKontakt_2.Location = New System.Drawing.Point(144, 212)
		Me.deMAKontakt_2.Name = "deMAKontakt_2"
		Me.deMAKontakt_2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.deMAKontakt_2.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.deMAKontakt_2.Size = New System.Drawing.Size(98, 20)
		Me.deMAKontakt_2.TabIndex = 191
		'
		'deMAKontakt_1
		'
		Me.deMAKontakt_1.EditValue = Nothing
		Me.deMAKontakt_1.Location = New System.Drawing.Point(144, 186)
		Me.deMAKontakt_1.Name = "deMAKontakt_1"
		Me.deMAKontakt_1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.deMAKontakt_1.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.deMAKontakt_1.Size = New System.Drawing.Size(98, 20)
		Me.deMAKontakt_1.TabIndex = 190
		'
		'xtabTODO
		'
		Me.xtabTODO.Controls.Add(Me.DateEdit1)
		Me.xtabTODO.Controls.Add(Me.DateEdit2)
		Me.xtabTODO.Controls.Add(Me.ComboBoxEdit1)
		Me.xtabTODO.Controls.Add(Me.Label1)
		Me.xtabTODO.Controls.Add(Me.Label2)
		Me.xtabTODO.Controls.Add(Me.ComboBoxEdit2)
		Me.xtabTODO.Controls.Add(Me.ComboBoxEdit3)
		Me.xtabTODO.Controls.Add(Me.Label3)
		Me.xtabTODO.Controls.Add(Me.Label4)
		Me.xtabTODO.Controls.Add(Me.ComboBoxEdit4)
		Me.xtabTODO.Controls.Add(Me.Label5)
		Me.xtabTODO.Name = "xtabTODO"
		Me.xtabTODO.Size = New System.Drawing.Size(682, 297)
		Me.xtabTODO.Text = "to-do-Liste"
		'
		'DateEdit1
		'
		Me.DateEdit1.EditValue = Nothing
		Me.DateEdit1.Location = New System.Drawing.Point(144, 188)
		Me.DateEdit1.Name = "DateEdit1"
		Me.DateEdit1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.DateEdit1.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.DateEdit1.Size = New System.Drawing.Size(98, 20)
		Me.DateEdit1.TabIndex = 169
		'
		'DateEdit2
		'
		Me.DateEdit2.EditValue = Nothing
		Me.DateEdit2.Location = New System.Drawing.Point(144, 162)
		Me.DateEdit2.Name = "DateEdit2"
		Me.DateEdit2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.DateEdit2.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.DateEdit2.Size = New System.Drawing.Size(98, 20)
		Me.DateEdit2.TabIndex = 168
		'
		'ComboBoxEdit1
		'
		Me.ComboBoxEdit1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.ComboBoxEdit1.Location = New System.Drawing.Point(144, 136)
		Me.ComboBoxEdit1.Name = "ComboBoxEdit1"
		Me.ComboBoxEdit1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.ComboBoxEdit1.Size = New System.Drawing.Size(231, 20)
		Me.ComboBoxEdit1.TabIndex = 167
		'
		'Label1
		'
		Me.Label1.Location = New System.Drawing.Point(24, 140)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(114, 13)
		Me.Label1.TabIndex = 160
		Me.Label1.Text = "Periode"
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'Label2
		'
		Me.Label2.Location = New System.Drawing.Point(24, 45)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(114, 13)
		Me.Label2.TabIndex = 161
		Me.Label2.Text = "Sortieren nach"
		Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'ComboBoxEdit2
		'
		Me.ComboBoxEdit2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.ComboBoxEdit2.Location = New System.Drawing.Point(144, 94)
		Me.ComboBoxEdit2.Name = "ComboBoxEdit2"
		Me.ComboBoxEdit2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.ComboBoxEdit2.Size = New System.Drawing.Size(231, 20)
		Me.ComboBoxEdit2.TabIndex = 166
		'
		'ComboBoxEdit3
		'
		Me.ComboBoxEdit3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.ComboBoxEdit3.Location = New System.Drawing.Point(144, 42)
		Me.ComboBoxEdit3.Name = "ComboBoxEdit3"
		Me.ComboBoxEdit3.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.ComboBoxEdit3.Size = New System.Drawing.Size(431, 20)
		Me.ComboBoxEdit3.TabIndex = 165
		'
		'Label3
		'
		Me.Label3.Location = New System.Drawing.Point(24, 97)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(114, 13)
		Me.Label3.TabIndex = 170
		Me.Label3.Text = "BeraterIn"
		Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'Label4
		'
		Me.Label4.Location = New System.Drawing.Point(24, 71)
		Me.Label4.Name = "Label4"
		Me.Label4.Size = New System.Drawing.Size(114, 13)
		Me.Label4.TabIndex = 162
		Me.Label4.Text = "Art der Liste"
		Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'ComboBoxEdit4
		'
		Me.ComboBoxEdit4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.ComboBoxEdit4.Location = New System.Drawing.Point(144, 68)
		Me.ComboBoxEdit4.Name = "ComboBoxEdit4"
		Me.ComboBoxEdit4.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.ComboBoxEdit4.Size = New System.Drawing.Size(431, 20)
		Me.ComboBoxEdit4.TabIndex = 164
		'
		'Label5
		'
		Me.Label5.Location = New System.Drawing.Point(24, 166)
		Me.Label5.Name = "Label5"
		Me.Label5.Size = New System.Drawing.Size(114, 13)
		Me.Label5.TabIndex = 163
		Me.Label5.Text = "Datum zwischen"
		Me.Label5.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'xtabErweitert
		'
		Me.xtabErweitert.Controls.Add(Me.lblAbfrage)
		Me.xtabErweitert.Controls.Add(Me.txt_IndSQLQuery)
		Me.xtabErweitert.Name = "xtabErweitert"
		Me.xtabErweitert.Size = New System.Drawing.Size(682, 297)
		Me.xtabErweitert.Text = "Erweiterte Abfrage"
		'
		'xtabSQLQuery
		'
		Me.xtabSQLQuery.Controls.Add(Me.lblderzeitigeabfrage)
		Me.xtabSQLQuery.Controls.Add(Me.txt_SQLQuery)
		Me.xtabSQLQuery.Name = "xtabSQLQuery"
		Me.xtabSQLQuery.Size = New System.Drawing.Size(682, 297)
		Me.xtabSQLQuery.Text = "SQL-Abfrage"
		'
		'lblMDName
		'
		Me.lblMDName.Location = New System.Drawing.Point(51, 109)
		Me.lblMDName.Name = "lblMDName"
		Me.lblMDName.Size = New System.Drawing.Size(117, 13)
		Me.lblMDName.TabIndex = 167
		Me.lblMDName.Text = "Mandanten"
		Me.lblMDName.TextAlign = System.Drawing.ContentAlignment.TopRight
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
		Me.bbiExport.Id = 10
		Me.bbiExport.ImageOptions.Image = CType(resources.GetObject("bbiExport.ImageOptions.Image"), System.Drawing.Image)
		Me.bbiExport.Name = "bbiExport"
		Me.bbiExport.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'BarStaticItem1
		'
		Me.BarStaticItem1.Id = 11
		Me.BarStaticItem1.Name = "BarStaticItem1"
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
		Me.BarManager1.MaxItemId = 12
		Me.BarManager1.StatusBar = Me.Bar3
		'
		'barDockControlTop
		'
		Me.barDockControlTop.CausesValidation = False
		Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
		Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlTop.Manager = Me.BarManager1
		Me.barDockControlTop.Size = New System.Drawing.Size(746, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 514)
		Me.barDockControlBottom.Manager = Me.BarManager1
		Me.barDockControlBottom.Size = New System.Drawing.Size(746, 26)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Manager = Me.BarManager1
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 514)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(746, 0)
		Me.barDockControlRight.Manager = Me.BarManager1
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 514)
		'
		'lueMandant
		'
		Me.lueMandant.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.lueMandant.Location = New System.Drawing.Point(174, 104)
		Me.lueMandant.Name = "lueMandant"
		Me.lueMandant.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.lueMandant.Size = New System.Drawing.Size(431, 20)
		Me.lueMandant.TabIndex = 0
		'
		'frmCallHistory
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(746, 540)
		Me.Controls.Add(Me.lblMDName)
		Me.Controls.Add(Me.XtraTabControl1)
		Me.Controls.Add(Me.GroupBox1)
		Me.Controls.Add(Me.lueMandant)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.IconOptions.Icon = CType(resources.GetObject("frmCallHistory.IconOptions.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.MinimumSize = New System.Drawing.Size(748, 572)
		Me.Name = "frmCallHistory"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Erweiterte Suche nach Telefon- und Kontaktdaten"
		Me.GroupBox1.ResumeLayout(False)
		Me.GroupBox1.PerformLayout()
		CType(Me.txt_SQLQuery.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txt_IndSQLQuery.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_MA_Kontakt.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_MA_Art.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_MA_DateTpl.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_MA_KST.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_ESAktivImSelektion.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_Berater.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_LstArt.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.CboSort.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.XtraTabControl1.ResumeLayout(False)
		Me.xtabAllgemein.ResumeLayout(False)
		CType(Me.deCallAt_2.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.deCallAt_2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.deCallAt_1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.deCallAt_1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.xtabKontakt.ResumeLayout(False)
		CType(Me.cboContactSort.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.deMAKontakt_2.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.deMAKontakt_2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.deMAKontakt_1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.deMAKontakt_1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.xtabTODO.ResumeLayout(False)
		CType(Me.DateEdit1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.DateEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.DateEdit2.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.DateEdit2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.ComboBoxEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.ComboBoxEdit2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.ComboBoxEdit3.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.ComboBoxEdit4.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.xtabErweitert.ResumeLayout(False)
		Me.xtabErweitert.PerformLayout()
		Me.xtabSQLQuery.ResumeLayout(False)
		Me.xtabSQLQuery.PerformLayout()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
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
  Friend WithEvents CmdClose As DevExpress.XtraEditors.SimpleButton ' System.Windows.Forms.Button
  Friend WithEvents Timer1 As System.Windows.Forms.Timer
  Friend WithEvents LblTimeValue As System.Windows.Forms.Label
  Friend WithEvents lblderzeitigeabfrage As System.Windows.Forms.Label
  Friend WithEvents txt_SQLQuery As DevExpress.XtraEditors.MemoEdit ' System.Windows.Forms.TextBox
  Friend WithEvents lblAbfrage As System.Windows.Forms.Label
  Friend WithEvents txt_IndSQLQuery As DevExpress.XtraEditors.MemoEdit ' System.Windows.Forms.TextBox
  Friend WithEvents CboSort As DevExpress.XtraEditors.ComboBoxEdit ' SPCallHistory.myCbo
  Friend WithEvents lblSort1 As System.Windows.Forms.Label
  Friend WithEvents lblDatumzwischen As System.Windows.Forms.Label
  Friend WithEvents Cbo_Berater As DevExpress.XtraEditors.ComboBoxEdit ' SPCallHistory.myCbo
  Friend WithEvents lblBerater As System.Windows.Forms.Label
  Friend WithEvents Cbo_ESAktivImSelektion As DevExpress.XtraEditors.ComboBoxEdit ' SPCallHistory.myCbo
  Friend WithEvents lblperiode As System.Windows.Forms.Label
  Friend WithEvents Cbo_LstArt As DevExpress.XtraEditors.ComboBoxEdit ' SPCallHistory.myCbo
  Friend WithEvents lbllistart As System.Windows.Forms.Label
  Friend WithEvents Cbo_MA_Kontakt As DevExpress.XtraEditors.TextEdit ' System.Windows.Forms.TextBox
  Friend WithEvents lblbezeichnung As System.Windows.Forms.Label
  Friend WithEvents Cbo_MA_Art As DevExpress.XtraEditors.ComboBoxEdit ' SPCallHistory.myCbo
  Friend WithEvents lblkontaktart As System.Windows.Forms.Label
  Friend WithEvents Cbo_MA_DateTpl As DevExpress.XtraEditors.ComboBoxEdit ' SPCallHistory.myCbo
  Friend WithEvents lblperiode2 As System.Windows.Forms.Label
  Friend WithEvents Cbo_MA_KST As DevExpress.XtraEditors.ComboBoxEdit ' SPCallHistory.myCbo
  Friend WithEvents lblberater2 As System.Windows.Forms.Label
  Friend WithEvents lbldatumzwischen2 As System.Windows.Forms.Label
  ' SPCallHistory.myCbo
  Friend WithEvents XtraTabControl1 As DevExpress.XtraTab.XtraTabControl
  Friend WithEvents xtabAllgemein As DevExpress.XtraTab.XtraTabPage
  Friend WithEvents xtabKontakt As DevExpress.XtraTab.XtraTabPage
  Friend WithEvents xtabErweitert As DevExpress.XtraTab.XtraTabPage
  Friend WithEvents xtabSQLQuery As DevExpress.XtraTab.XtraTabPage
  Friend WithEvents deCallAt_2 As DevExpress.XtraEditors.DateEdit
  Friend WithEvents deCallAt_1 As DevExpress.XtraEditors.DateEdit
  Friend WithEvents deMAKontakt_2 As DevExpress.XtraEditors.DateEdit
  Friend WithEvents deMAKontakt_1 As DevExpress.XtraEditors.DateEdit
  Friend WithEvents LblSetting As System.Windows.Forms.Label
	Friend WithEvents lblMDName As System.Windows.Forms.Label
	Friend WithEvents sbKontaktDb As DevComponents.DotNetBar.Controls.SwitchButton
	Friend WithEvents Bar3 As DevExpress.XtraBars.Bar
	Friend WithEvents bsiInfo As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents bbiSearch As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbiClear As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbiPrint As DevExpress.XtraBars.BarLargeButtonItem
	Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
	Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
	Friend WithEvents bbiExport As DevExpress.XtraBars.BarLargeButtonItem
	Friend WithEvents lblSort2 As System.Windows.Forms.Label
	Friend WithEvents cboContactSort As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents BarStaticItem1 As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents xtabTODO As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents DateEdit1 As DevExpress.XtraEditors.DateEdit
	Friend WithEvents DateEdit2 As DevExpress.XtraEditors.DateEdit
	Friend WithEvents ComboBoxEdit1 As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents Label1 As System.Windows.Forms.Label
	Friend WithEvents Label2 As System.Windows.Forms.Label
	Friend WithEvents ComboBoxEdit2 As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents ComboBoxEdit3 As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents Label3 As System.Windows.Forms.Label
	Friend WithEvents Label4 As System.Windows.Forms.Label
	Friend WithEvents ComboBoxEdit4 As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents Label5 As System.Windows.Forms.Label
	Friend WithEvents lueMandant As DevExpress.XtraEditors.CheckedComboBoxEdit
End Class
