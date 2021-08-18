<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmYFakListSearch
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmYFakListSearch))
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
		Me.LblKst3 = New System.Windows.Forms.Label()
		Me.LblFilial = New System.Windows.Forms.Label()
		Me.LblYear = New System.Windows.Forms.Label()
		Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
		Me.txt_SQLQuery = New DevExpress.XtraEditors.MemoEdit()
		Me.Cbo_Year = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.Cbo_Filiale = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.Cbo_Kanton = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.XtraTabControl1 = New DevExpress.XtraTab.XtraTabControl()
		Me.xtabAllgemein = New DevExpress.XtraTab.XtraTabPage()
		Me.lueMandant = New DevExpress.XtraEditors.LookUpEdit()
		Me.lblMDName = New System.Windows.Forms.Label()
		Me.xtabSQLAbfrage = New DevExpress.XtraTab.XtraTabPage()
		Me.lblSQLAbfrage = New System.Windows.Forms.Label()
		Me.GroupBox1 = New DevExpress.XtraEditors.PanelControl()
		Me.CmdClose = New DevExpress.XtraEditors.SimpleButton()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.LblTimeValue = New System.Windows.Forms.Label()
		Me.lblHeaderNormal = New System.Windows.Forms.Label()
		Me.lbLstlInfo = New DevExpress.XtraEditors.LabelControl()
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
		CType(Me.txt_SQLQuery.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_Year.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_Filiale.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_Kanton.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.XtraTabControl1.SuspendLayout()
		Me.xtabAllgemein.SuspendLayout()
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.xtabSQLAbfrage.SuspendLayout()
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
		Me.lblHeaderFett.Size = New System.Drawing.Size(241, 13)
		Me.lblHeaderFett.TabIndex = 0
		Me.lblHeaderFett.Text = "Liste der Kinder- und Ausbildungszulagen"
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
		'LblKst3
		'
		Me.LblKst3.Location = New System.Drawing.Point(15, 107)
		Me.LblKst3.Name = "LblKst3"
		Me.LblKst3.Size = New System.Drawing.Size(103, 13)
		Me.LblKst3.TabIndex = 84
		Me.LblKst3.Text = "Kanton"
		Me.LblKst3.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'LblFilial
		'
		Me.LblFilial.Location = New System.Drawing.Point(15, 134)
		Me.LblFilial.Name = "LblFilial"
		Me.LblFilial.Size = New System.Drawing.Size(103, 13)
		Me.LblFilial.TabIndex = 83
		Me.LblFilial.Text = "Filiale"
		Me.LblFilial.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'LblYear
		'
		Me.LblYear.Location = New System.Drawing.Point(15, 73)
		Me.LblYear.Name = "LblYear"
		Me.LblYear.Size = New System.Drawing.Size(103, 13)
		Me.LblYear.TabIndex = 82
		Me.LblYear.Text = "Jahr"
		Me.LblYear.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'BackgroundWorker1
		'
		'
		'txt_SQLQuery
		'
		Me.txt_SQLQuery.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txt_SQLQuery.Location = New System.Drawing.Point(14, 34)
		Me.txt_SQLQuery.Name = "txt_SQLQuery"
		Me.txt_SQLQuery.Size = New System.Drawing.Size(445, 141)
		Me.txt_SQLQuery.TabIndex = 100
		'
		'Cbo_Year
		'
		Me.Cbo_Year.Location = New System.Drawing.Point(124, 70)
		Me.Cbo_Year.Name = "Cbo_Year"
		Me.Cbo_Year.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_Year.Size = New System.Drawing.Size(114, 20)
		Me.Cbo_Year.TabIndex = 0
		'
		'Cbo_Filiale
		'
		Me.Cbo_Filiale.Location = New System.Drawing.Point(124, 131)
		Me.Cbo_Filiale.Name = "Cbo_Filiale"
		Me.Cbo_Filiale.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_Filiale.Size = New System.Drawing.Size(114, 20)
		Me.Cbo_Filiale.TabIndex = 2
		'
		'Cbo_Kanton
		'
		Me.Cbo_Kanton.Location = New System.Drawing.Point(124, 104)
		Me.Cbo_Kanton.Name = "Cbo_Kanton"
		Me.Cbo_Kanton.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_Kanton.Size = New System.Drawing.Size(114, 20)
		Me.Cbo_Kanton.TabIndex = 1
		'
		'XtraTabControl1
		'
		Me.XtraTabControl1.Location = New System.Drawing.Point(350, 97)
		Me.XtraTabControl1.Name = "XtraTabControl1"
		Me.XtraTabControl1.SelectedTabPage = Me.xtabAllgemein
		Me.XtraTabControl1.Size = New System.Drawing.Size(484, 218)
		Me.XtraTabControl1.TabIndex = 102
		Me.XtraTabControl1.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabAllgemein, Me.xtabSQLAbfrage})
		'
		'xtabAllgemein
		'
		Me.xtabAllgemein.Controls.Add(Me.lueMandant)
		Me.xtabAllgemein.Controls.Add(Me.lblMDName)
		Me.xtabAllgemein.Controls.Add(Me.Cbo_Year)
		Me.xtabAllgemein.Controls.Add(Me.LblKst3)
		Me.xtabAllgemein.Controls.Add(Me.Cbo_Kanton)
		Me.xtabAllgemein.Controls.Add(Me.LblYear)
		Me.xtabAllgemein.Controls.Add(Me.Cbo_Filiale)
		Me.xtabAllgemein.Controls.Add(Me.LblFilial)
		Me.xtabAllgemein.Name = "xtabAllgemein"
		Me.xtabAllgemein.Size = New System.Drawing.Size(478, 190)
		Me.xtabAllgemein.Text = "Allgemein"
		'
		'lueMandant
		'
		Me.lueMandant.Location = New System.Drawing.Point(124, 28)
		Me.lueMandant.Name = "lueMandant"
		Me.lueMandant.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.lueMandant.Properties.NullText = ""
		Me.lueMandant.Size = New System.Drawing.Size(302, 20)
		Me.lueMandant.TabIndex = 0
		'
		'lblMDName
		'
		Me.lblMDName.Location = New System.Drawing.Point(7, 32)
		Me.lblMDName.Name = "lblMDName"
		Me.lblMDName.Size = New System.Drawing.Size(111, 13)
		Me.lblMDName.TabIndex = 202
		Me.lblMDName.Text = "Mandanten"
		Me.lblMDName.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'xtabSQLAbfrage
		'
		Me.xtabSQLAbfrage.Controls.Add(Me.lblSQLAbfrage)
		Me.xtabSQLAbfrage.Controls.Add(Me.txt_SQLQuery)
		Me.xtabSQLAbfrage.Name = "xtabSQLAbfrage"
		Me.xtabSQLAbfrage.Size = New System.Drawing.Size(478, 190)
		Me.xtabSQLAbfrage.Text = "SQL-Abfrage"
		'
		'lblSQLAbfrage
		'
		Me.lblSQLAbfrage.AutoSize = True
		Me.lblSQLAbfrage.Location = New System.Drawing.Point(14, 18)
		Me.lblSQLAbfrage.Name = "lblSQLAbfrage"
		Me.lblSQLAbfrage.Size = New System.Drawing.Size(154, 13)
		Me.lblSQLAbfrage.TabIndex = 101
		Me.lblSQLAbfrage.Text = "Ihre derzeitige Abfrage lautet:"
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
		Me.GroupBox1.Size = New System.Drawing.Size(850, 77)
		Me.GroupBox1.TabIndex = 208
		'
		'CmdClose
		'
		Me.CmdClose.Location = New System.Drawing.Point(730, 21)
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
		Me.LblTimeValue.Location = New System.Drawing.Point(522, 60)
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
		'lbLstlInfo
		'
		Me.lbLstlInfo.AllowHtmlString = True
		Me.lbLstlInfo.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.lbLstlInfo.Appearance.Image = CType(resources.GetObject("lbLstlInfo.Appearance.Image"), System.Drawing.Image)
		Me.lbLstlInfo.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter
		Me.lbLstlInfo.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
		Me.lbLstlInfo.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
		Me.lbLstlInfo.AutoEllipsis = True
		Me.lbLstlInfo.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lbLstlInfo.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftTop
		Me.lbLstlInfo.LineColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
		Me.lbLstlInfo.LineLocation = DevExpress.XtraEditors.LineLocation.Bottom
		Me.lbLstlInfo.LineOrientation = DevExpress.XtraEditors.LabelLineOrientation.Vertical
		Me.lbLstlInfo.LineVisible = True
		Me.lbLstlInfo.Location = New System.Drawing.Point(15, 97)
		Me.lbLstlInfo.Name = "lbLstlInfo"
		Me.lbLstlInfo.Size = New System.Drawing.Size(306, 218)
		Me.lbLstlInfo.TabIndex = 289
		Me.lbLstlInfo.Text = "<b>Info</b>" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Es werden alle Kandidaten und Kandidatinnen ausgewählt, welche im Fr" & _
		"ankreich wohnen</br>und eine Ansässigkeitsbescheinigung abgegeben haben."
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
		Me.bsiInfo.TextAlignment = System.Drawing.StringAlignment.Near
		Me.bsiInfo.Width = 32
		'
		'bbiSearch
		'
		Me.bbiSearch.Caption = "Suchen"
		Me.bbiSearch.Glyph = CType(resources.GetObject("bbiSearch.Glyph"), System.Drawing.Image)
		Me.bbiSearch.Id = 1
		Me.bbiSearch.LargeGlyph = CType(resources.GetObject("bbiSearch.LargeGlyph"), System.Drawing.Image)
		Me.bbiSearch.Name = "bbiSearch"
		Me.bbiSearch.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiClear
		'
		Me.bbiClear.Caption = "Felder leeren"
		Me.bbiClear.Glyph = CType(resources.GetObject("bbiClear.Glyph"), System.Drawing.Image)
		Me.bbiClear.Id = 2
		Me.bbiClear.LargeGlyph = CType(resources.GetObject("bbiClear.LargeGlyph"), System.Drawing.Image)
		Me.bbiClear.Name = "bbiClear"
		Me.bbiClear.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiPrint
		'
		Me.bbiPrint.Caption = "Drucken"
		Me.bbiPrint.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Right
		Me.bbiPrint.Glyph = CType(resources.GetObject("bbiPrint.Glyph"), System.Drawing.Image)
		Me.bbiPrint.Id = 7
		Me.bbiPrint.Name = "bbiPrint"
		Me.bbiPrint.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiExport
		'
		Me.bbiExport.Caption = "Exportieren"
		Me.bbiExport.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Right
		Me.bbiExport.Glyph = CType(resources.GetObject("bbiExport.Glyph"), System.Drawing.Image)
		Me.bbiExport.Id = 8
		Me.bbiExport.Name = "bbiExport"
		Me.bbiExport.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'BarStaticItem1
		'
		Me.BarStaticItem1.Id = 9
		Me.BarStaticItem1.Name = "BarStaticItem1"
		Me.BarStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near
		'
		'barDockControlTop
		'
		Me.barDockControlTop.CausesValidation = False
		Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
		Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlTop.Size = New System.Drawing.Size(850, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 337)
		Me.barDockControlBottom.Size = New System.Drawing.Size(850, 27)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 337)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(850, 0)
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 337)
		'
		'frmYFakListSearch
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(850, 364)
		Me.Controls.Add(Me.lbLstlInfo)
		Me.Controls.Add(Me.GroupBox1)
		Me.Controls.Add(Me.XtraTabControl1)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.Name = "frmYFakListSearch"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Jährliche Lohnlisten: Kinder- und Ausbildungszulagen"
		CType(Me.txt_SQLQuery.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_Year.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_Filiale.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_Kanton.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.XtraTabControl1.ResumeLayout(False)
		Me.xtabAllgemein.ResumeLayout(False)
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.xtabSQLAbfrage.ResumeLayout(False)
		Me.xtabSQLAbfrage.PerformLayout()
		CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupBox1.ResumeLayout(False)
		Me.GroupBox1.PerformLayout()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

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
	Friend WithEvents Cbo_Kanton As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents LblKst3 As System.Windows.Forms.Label
	Friend WithEvents Cbo_Year As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents Cbo_Filiale As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents LblFilial As System.Windows.Forms.Label
	Friend WithEvents LblYear As System.Windows.Forms.Label
	Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
	Friend WithEvents txt_SQLQuery As DevExpress.XtraEditors.MemoEdit
	Friend WithEvents XtraTabControl1 As DevExpress.XtraTab.XtraTabControl
	Friend WithEvents xtabAllgemein As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents xtabSQLAbfrage As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents lblSQLAbfrage As System.Windows.Forms.Label
	Friend WithEvents lueMandant As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents lblMDName As System.Windows.Forms.Label
	Friend WithEvents GroupBox1 As DevExpress.XtraEditors.PanelControl
	Friend WithEvents CmdClose As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents Label2 As System.Windows.Forms.Label
	Friend WithEvents LblTimeValue As System.Windows.Forms.Label
	Friend WithEvents lblHeaderNormal As System.Windows.Forms.Label
	Friend WithEvents lbLstlInfo As DevExpress.XtraEditors.LabelControl
	Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
	Friend WithEvents Bar3 As DevExpress.XtraBars.Bar
	Friend WithEvents bsiInfo As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents bbiSearch As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbiClear As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbiPrint As DevExpress.XtraBars.BarLargeButtonItem
	Friend WithEvents bbiExport As DevExpress.XtraBars.BarLargeButtonItem
	Friend WithEvents BarStaticItem1 As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl

End Class
