<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmKDKstSearch
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmKDKstSearch))
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
		Me.Cbo_Periode = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.lblMANr = New DevExpress.XtraEditors.LabelControl()
		Me.cbo_KDNr = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.lblPerdiode = New System.Windows.Forms.Label()
		Me.lblmonat = New System.Windows.Forms.Label()
		Me.lblBeruf = New System.Windows.Forms.Label()
		Me.XtraTabControl1 = New DevExpress.XtraTab.XtraTabControl()
		Me.xtabAllgemein = New DevExpress.XtraTab.XtraTabPage()
		Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl()
		Me.cbo_MANr = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl()
		Me.cbo_ESNr = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.lblJahr = New System.Windows.Forms.Label()
		Me.lueMandant = New DevExpress.XtraEditors.LookUpEdit()
		Me.lblMDName = New System.Windows.Forms.Label()
		Me.SwitchButton1 = New DevComponents.DotNetBar.Controls.SwitchButton()
		Me.Cbo_BMonth_1 = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.Cbo_VMonth_1 = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.Cbo_JahrVon = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.Cbo_FARListeBeruf = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.xtabSQLQuery = New DevExpress.XtraTab.XtraTabPage()
		Me.txt_SQLQuery = New System.Windows.Forms.TextBox()
		Me.lblSQLAbfrage = New System.Windows.Forms.Label()
		Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
		Me.lblHeaderNormal = New System.Windows.Forms.Label()
		Me.CmdClose = New DevExpress.XtraEditors.SimpleButton()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.lblHeaderFett = New System.Windows.Forms.Label()
		Me.Bar4 = New DevExpress.XtraBars.Bar()
		Me.beiWorking = New DevExpress.XtraBars.BarEditItem()
		Me.RepositoryItemMarqueeProgressBar2 = New DevExpress.XtraEditors.Repository.RepositoryItemMarqueeProgressBar()
		Me.bsiInfo = New DevExpress.XtraBars.BarStaticItem()
		Me.bbiSearch = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiClearFields = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiPrint = New DevExpress.XtraBars.BarLargeButtonItem()
		Me.bbiExport = New DevExpress.XtraBars.BarLargeButtonItem()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
		Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
		Me.BarStaticItem1 = New DevExpress.XtraBars.BarStaticItem()
		Me.BarEditItem1 = New DevExpress.XtraBars.BarEditItem()
		Me.RepositoryItemMarqueeProgressBar1 = New DevExpress.XtraEditors.Repository.RepositoryItemMarqueeProgressBar()
		Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
		CType(Me.Cbo_Periode.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.cbo_KDNr.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.XtraTabControl1.SuspendLayout()
		Me.xtabAllgemein.SuspendLayout()
		CType(Me.cbo_MANr.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.cbo_ESNr.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_BMonth_1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_VMonth_1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_JahrVon.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_FARListeBeruf.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.xtabSQLQuery.SuspendLayout()
		CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.PanelControl1.SuspendLayout()
		CType(Me.RepositoryItemMarqueeProgressBar2, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.RepositoryItemMarqueeProgressBar1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
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
		'Cbo_Periode
		'
		Me.Cbo_Periode.Location = New System.Drawing.Point(158, 164)
		Me.Cbo_Periode.Name = "Cbo_Periode"
		Me.Cbo_Periode.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.DropDown)})
		Me.Cbo_Periode.Size = New System.Drawing.Size(384, 20)
		Me.Cbo_Periode.TabIndex = 30
		'
		'lblMANr
		'
		Me.lblMANr.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblMANr.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblMANr.Location = New System.Drawing.Point(13, 71)
		Me.lblMANr.Name = "lblMANr"
		Me.lblMANr.Size = New System.Drawing.Size(139, 13)
		Me.lblMANr.TabIndex = 276
		Me.lblMANr.TabStop = True
		Me.lblMANr.Tag = ""
		Me.lblMANr.Text = "Kunden-Nr."
		'
		'cbo_KDNr
		'
		Me.cbo_KDNr.Location = New System.Drawing.Point(158, 67)
		Me.cbo_KDNr.Name = "cbo_KDNr"
		Me.cbo_KDNr.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.cbo_KDNr.Size = New System.Drawing.Size(384, 20)
		Me.cbo_KDNr.TabIndex = 10
		Me.cbo_KDNr.Tag = ""
		'
		'lblPerdiode
		'
		Me.lblPerdiode.Location = New System.Drawing.Point(13, 168)
		Me.lblPerdiode.Name = "lblPerdiode"
		Me.lblPerdiode.Size = New System.Drawing.Size(139, 13)
		Me.lblPerdiode.TabIndex = 274
		Me.lblPerdiode.Text = "Periode"
		Me.lblPerdiode.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblmonat
		'
		Me.lblmonat.Location = New System.Drawing.Point(13, 220)
		Me.lblmonat.Name = "lblmonat"
		Me.lblmonat.Size = New System.Drawing.Size(139, 13)
		Me.lblmonat.TabIndex = 272
		Me.lblmonat.Text = "Monat"
		Me.lblmonat.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblBeruf
		'
		Me.lblBeruf.Location = New System.Drawing.Point(13, 261)
		Me.lblBeruf.Name = "lblBeruf"
		Me.lblBeruf.Size = New System.Drawing.Size(139, 13)
		Me.lblBeruf.TabIndex = 264
		Me.lblBeruf.Text = "PVL-Beruf"
		Me.lblBeruf.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'XtraTabControl1
		'
		Me.XtraTabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.XtraTabControl1.Location = New System.Drawing.Point(20, 104)
		Me.XtraTabControl1.Name = "XtraTabControl1"
		Me.XtraTabControl1.SelectedTabPage = Me.xtabAllgemein
		Me.XtraTabControl1.Size = New System.Drawing.Size(593, 332)
		Me.XtraTabControl1.TabIndex = 202
		Me.XtraTabControl1.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabAllgemein, Me.xtabSQLQuery})
		'
		'xtabAllgemein
		'
		Me.xtabAllgemein.Controls.Add(Me.LabelControl2)
		Me.xtabAllgemein.Controls.Add(Me.cbo_MANr)
		Me.xtabAllgemein.Controls.Add(Me.LabelControl1)
		Me.xtabAllgemein.Controls.Add(Me.cbo_ESNr)
		Me.xtabAllgemein.Controls.Add(Me.lblJahr)
		Me.xtabAllgemein.Controls.Add(Me.lueMandant)
		Me.xtabAllgemein.Controls.Add(Me.lblMDName)
		Me.xtabAllgemein.Controls.Add(Me.SwitchButton1)
		Me.xtabAllgemein.Controls.Add(Me.Cbo_BMonth_1)
		Me.xtabAllgemein.Controls.Add(Me.Cbo_VMonth_1)
		Me.xtabAllgemein.Controls.Add(Me.Cbo_JahrVon)
		Me.xtabAllgemein.Controls.Add(Me.Cbo_FARListeBeruf)
		Me.xtabAllgemein.Controls.Add(Me.lblMANr)
		Me.xtabAllgemein.Controls.Add(Me.Cbo_Periode)
		Me.xtabAllgemein.Controls.Add(Me.cbo_KDNr)
		Me.xtabAllgemein.Controls.Add(Me.lblBeruf)
		Me.xtabAllgemein.Controls.Add(Me.lblPerdiode)
		Me.xtabAllgemein.Controls.Add(Me.lblmonat)
		Me.xtabAllgemein.Name = "xtabAllgemein"
		Me.xtabAllgemein.Size = New System.Drawing.Size(587, 304)
		Me.xtabAllgemein.Text = "Allgemein"
		'
		'LabelControl2
		'
		Me.LabelControl2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.LabelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.LabelControl2.Location = New System.Drawing.Point(13, 97)
		Me.LabelControl2.Name = "LabelControl2"
		Me.LabelControl2.Size = New System.Drawing.Size(139, 13)
		Me.LabelControl2.TabIndex = 314
		Me.LabelControl2.TabStop = True
		Me.LabelControl2.Tag = ""
		Me.LabelControl2.Text = "Kandidaten-Nr"
		'
		'cbo_MANr
		'
		Me.cbo_MANr.Location = New System.Drawing.Point(158, 93)
		Me.cbo_MANr.Name = "cbo_MANr"
		Me.cbo_MANr.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.cbo_MANr.Size = New System.Drawing.Size(384, 20)
		Me.cbo_MANr.TabIndex = 313
		Me.cbo_MANr.Tag = ""
		'
		'LabelControl1
		'
		Me.LabelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.LabelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.LabelControl1.Location = New System.Drawing.Point(13, 123)
		Me.LabelControl1.Name = "LabelControl1"
		Me.LabelControl1.Size = New System.Drawing.Size(139, 13)
		Me.LabelControl1.TabIndex = 312
		Me.LabelControl1.TabStop = True
		Me.LabelControl1.Tag = ""
		Me.LabelControl1.Text = "Einsatz-Nr."
		'
		'cbo_ESNr
		'
		Me.cbo_ESNr.Location = New System.Drawing.Point(158, 119)
		Me.cbo_ESNr.Name = "cbo_ESNr"
		Me.cbo_ESNr.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.cbo_ESNr.Size = New System.Drawing.Size(384, 20)
		Me.cbo_ESNr.TabIndex = 311
		Me.cbo_ESNr.Tag = ""
		'
		'lblJahr
		'
		Me.lblJahr.Location = New System.Drawing.Point(13, 193)
		Me.lblJahr.Name = "lblJahr"
		Me.lblJahr.Size = New System.Drawing.Size(139, 13)
		Me.lblJahr.TabIndex = 310
		Me.lblJahr.Text = "Jahr"
		Me.lblJahr.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lueMandant
		'
		Me.lueMandant.Location = New System.Drawing.Point(158, 29)
		Me.lueMandant.Name = "lueMandant"
		Me.lueMandant.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.lueMandant.Properties.NullText = ""
		Me.lueMandant.Size = New System.Drawing.Size(384, 20)
		Me.lueMandant.TabIndex = 308
		'
		'lblMDName
		'
		Me.lblMDName.Location = New System.Drawing.Point(13, 34)
		Me.lblMDName.Name = "lblMDName"
		Me.lblMDName.Size = New System.Drawing.Size(139, 13)
		Me.lblMDName.TabIndex = 309
		Me.lblMDName.Text = "Mandanten"
		Me.lblMDName.TextAlign = System.Drawing.ContentAlignment.TopRight
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
		Me.SwitchButton1.Location = New System.Drawing.Point(422, 217)
		Me.SwitchButton1.Name = "SwitchButton1"
		Me.SwitchButton1.OffBackColor = System.Drawing.Color.White
		Me.SwitchButton1.OffText = "O"
		Me.SwitchButton1.OffTextColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(53, Byte), Integer))
		Me.SwitchButton1.OnBackColor = System.Drawing.Color.LightSteelBlue
		Me.SwitchButton1.OnText = "|"
		Me.SwitchButton1.OnTextColor = System.Drawing.Color.Black
		Me.SwitchButton1.Size = New System.Drawing.Size(38, 19)
		Me.SwitchButton1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
		Me.SwitchButton1.SwitchBackColor = System.Drawing.SystemColors.ControlDark
		Me.SwitchButton1.SwitchBorderColor = System.Drawing.SystemColors.ControlDark
		Me.SwitchButton1.SwitchFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.SwitchButton1.SwitchWidth = 6
		Me.SwitchButton1.TabIndex = 303
		'
		'Cbo_BMonth_1
		'
		Me.Cbo_BMonth_1.Location = New System.Drawing.Point(466, 216)
		Me.Cbo_BMonth_1.Name = "Cbo_BMonth_1"
		Me.Cbo_BMonth_1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_BMonth_1.Size = New System.Drawing.Size(76, 20)
		Me.Cbo_BMonth_1.TabIndex = 278
		Me.Cbo_BMonth_1.Visible = False
		'
		'Cbo_VMonth_1
		'
		Me.Cbo_VMonth_1.Location = New System.Drawing.Point(158, 216)
		Me.Cbo_VMonth_1.Name = "Cbo_VMonth_1"
		Me.Cbo_VMonth_1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_VMonth_1.Size = New System.Drawing.Size(76, 20)
		Me.Cbo_VMonth_1.TabIndex = 280
		'
		'Cbo_JahrVon
		'
		Me.Cbo_JahrVon.Location = New System.Drawing.Point(158, 190)
		Me.Cbo_JahrVon.Name = "Cbo_JahrVon"
		Me.Cbo_JahrVon.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_JahrVon.Size = New System.Drawing.Size(76, 20)
		Me.Cbo_JahrVon.TabIndex = 279
		'
		'Cbo_FARListeBeruf
		'
		Me.Cbo_FARListeBeruf.Location = New System.Drawing.Point(158, 258)
		Me.Cbo_FARListeBeruf.Name = "Cbo_FARListeBeruf"
		Me.Cbo_FARListeBeruf.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_FARListeBeruf.Size = New System.Drawing.Size(384, 20)
		Me.Cbo_FARListeBeruf.TabIndex = 277
		'
		'xtabSQLQuery
		'
		Me.xtabSQLQuery.Controls.Add(Me.txt_SQLQuery)
		Me.xtabSQLQuery.Controls.Add(Me.lblSQLAbfrage)
		Me.xtabSQLQuery.Name = "xtabSQLQuery"
		Me.xtabSQLQuery.Size = New System.Drawing.Size(587, 304)
		Me.xtabSQLQuery.Text = "SQL- Abfrage"
		'
		'txt_SQLQuery
		'
		Me.txt_SQLQuery.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txt_SQLQuery.Location = New System.Drawing.Point(21, 34)
		Me.txt_SQLQuery.Multiline = True
		Me.txt_SQLQuery.Name = "txt_SQLQuery"
		Me.txt_SQLQuery.Size = New System.Drawing.Size(544, 249)
		Me.txt_SQLQuery.TabIndex = 101
		'
		'lblSQLAbfrage
		'
		Me.lblSQLAbfrage.AutoSize = True
		Me.lblSQLAbfrage.Location = New System.Drawing.Point(18, 18)
		Me.lblSQLAbfrage.Name = "lblSQLAbfrage"
		Me.lblSQLAbfrage.Size = New System.Drawing.Size(154, 13)
		Me.lblSQLAbfrage.TabIndex = 105
		Me.lblSQLAbfrage.Text = "Ihre derzeitige Abfrage lautet:"
		'
		'PanelControl1
		'
		Me.PanelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003
		Me.PanelControl1.Controls.Add(Me.lblHeaderNormal)
		Me.PanelControl1.Controls.Add(Me.CmdClose)
		Me.PanelControl1.Controls.Add(Me.Label2)
		Me.PanelControl1.Controls.Add(Me.lblHeaderFett)
		Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Top
		Me.PanelControl1.Location = New System.Drawing.Point(0, 0)
		Me.PanelControl1.Name = "PanelControl1"
		Me.PanelControl1.Size = New System.Drawing.Size(638, 77)
		Me.PanelControl1.TabIndex = 312
		'
		'lblHeaderNormal
		'
		Me.lblHeaderNormal.AutoSize = True
		Me.lblHeaderNormal.Location = New System.Drawing.Point(110, 47)
		Me.lblHeaderNormal.Name = "lblHeaderNormal"
		Me.lblHeaderNormal.Size = New System.Drawing.Size(234, 13)
		Me.lblHeaderNormal.TabIndex = 1
		Me.lblHeaderNormal.Text = "Geben Sie bitte Ihre gewünschten Kriterien ein."
		'
		'CmdClose
		'
		Me.CmdClose.Location = New System.Drawing.Point(519, 21)
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
		'lblHeaderFett
		'
		Me.lblHeaderFett.AutoSize = True
		Me.lblHeaderFett.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblHeaderFett.Location = New System.Drawing.Point(94, 21)
		Me.lblHeaderFett.Name = "lblHeaderFett"
		Me.lblHeaderFett.Size = New System.Drawing.Size(201, 13)
		Me.lblHeaderFett.TabIndex = 0
		Me.lblHeaderFett.Text = "Liste für die Kunden-Kostenstellen"
		'
		'Bar4
		'
		Me.Bar4.BarName = "Statusleiste"
		Me.Bar4.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
		Me.Bar4.DockCol = 0
		Me.Bar4.DockRow = 0
		Me.Bar4.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
		Me.Bar4.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.beiWorking), New DevExpress.XtraBars.LinkPersistInfo(Me.bsiInfo), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiSearch), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiClearFields, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiPrint), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiExport, True)})
		Me.Bar4.OptionsBar.AllowQuickCustomization = False
		Me.Bar4.OptionsBar.DrawDragBorder = False
		Me.Bar4.OptionsBar.UseWholeRow = True
		Me.Bar4.Text = "Statusleiste"
		'
		'beiWorking
		'
		Me.beiWorking.Caption = "BarEditItem2"
		Me.beiWorking.Edit = Me.RepositoryItemMarqueeProgressBar2
		Me.beiWorking.Id = 11
		Me.beiWorking.Name = "beiWorking"
		'
		'RepositoryItemMarqueeProgressBar2
		'
		Me.RepositoryItemMarqueeProgressBar2.Name = "RepositoryItemMarqueeProgressBar2"
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
		'BarManager1
		'
		Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar4})
		Me.BarManager1.DockControls.Add(Me.barDockControlTop)
		Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
		Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
		Me.BarManager1.DockControls.Add(Me.barDockControlRight)
		Me.BarManager1.Form = Me
		Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiInfo, Me.bbiSearch, Me.bbiPrint, Me.bbiClearFields, Me.BarStaticItem1, Me.bbiExport, Me.BarEditItem1, Me.beiWorking})
		Me.BarManager1.MaxItemId = 12
		Me.BarManager1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemMarqueeProgressBar1, Me.RepositoryItemMarqueeProgressBar2})
		Me.BarManager1.StatusBar = Me.Bar4
		'
		'barDockControlTop
		'
		Me.barDockControlTop.CausesValidation = False
		Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
		Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlTop.Size = New System.Drawing.Size(638, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 471)
		Me.barDockControlBottom.Size = New System.Drawing.Size(638, 27)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 471)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(638, 0)
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 471)
		'
		'BarStaticItem1
		'
		Me.BarStaticItem1.Caption = " "
		Me.BarStaticItem1.Id = 5
		Me.BarStaticItem1.Name = "BarStaticItem1"
		Me.BarStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near
		'
		'BarEditItem1
		'
		Me.BarEditItem1.Caption = "BarEditItem1"
		Me.BarEditItem1.Edit = Me.RepositoryItemMarqueeProgressBar1
		Me.BarEditItem1.Id = 10
		Me.BarEditItem1.Name = "BarEditItem1"
		'
		'RepositoryItemMarqueeProgressBar1
		'
		Me.RepositoryItemMarqueeProgressBar1.Name = "RepositoryItemMarqueeProgressBar1"
		'
		'BackgroundWorker1
		'
		'
		'frmKDKstSearch
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(638, 498)
		Me.Controls.Add(Me.PanelControl1)
		Me.Controls.Add(Me.XtraTabControl1)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.MaximumSize = New System.Drawing.Size(654, 536)
		Me.MinimumSize = New System.Drawing.Size(654, 536)
		Me.Name = "frmKDKstSearch"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Erweiterte-Listen: Kunden-Kostenstellen"
		CType(Me.Cbo_Periode.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.cbo_KDNr.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.XtraTabControl1.ResumeLayout(False)
		Me.xtabAllgemein.ResumeLayout(False)
		CType(Me.cbo_MANr.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.cbo_ESNr.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_BMonth_1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_VMonth_1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_JahrVon.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_FARListeBeruf.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.xtabSQLQuery.ResumeLayout(False)
		Me.xtabSQLQuery.PerformLayout()
		CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.PanelControl1.ResumeLayout(False)
		Me.PanelControl1.PerformLayout()
		CType(Me.RepositoryItemMarqueeProgressBar2, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.RepositoryItemMarqueeProgressBar1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub
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
	Friend WithEvents lblBeruf As System.Windows.Forms.Label
	Friend WithEvents Cbo_Periode As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents lblMANr As DevExpress.XtraEditors.LabelControl
	Friend WithEvents cbo_KDNr As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents lblPerdiode As System.Windows.Forms.Label
	Friend WithEvents lblmonat As System.Windows.Forms.Label
	Friend WithEvents XtraTabControl1 As DevExpress.XtraTab.XtraTabControl
	Friend WithEvents xtabAllgemein As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents xtabSQLQuery As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents txt_SQLQuery As System.Windows.Forms.TextBox
	Friend WithEvents lblSQLAbfrage As System.Windows.Forms.Label
	Friend WithEvents Cbo_FARListeBeruf As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents Cbo_BMonth_1 As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents Cbo_VMonth_1 As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents Cbo_JahrVon As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents SwitchButton1 As DevComponents.DotNetBar.Controls.SwitchButton
	Friend WithEvents lblJahr As System.Windows.Forms.Label
	Friend WithEvents lueMandant As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents lblMDName As System.Windows.Forms.Label
	Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
	Friend WithEvents lblHeaderNormal As System.Windows.Forms.Label
	Friend WithEvents CmdClose As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents Label2 As System.Windows.Forms.Label
	Friend WithEvents lblHeaderFett As System.Windows.Forms.Label
	Friend WithEvents Bar4 As DevExpress.XtraBars.Bar
	Friend WithEvents beiWorking As DevExpress.XtraBars.BarEditItem
	Friend WithEvents RepositoryItemMarqueeProgressBar2 As DevExpress.XtraEditors.Repository.RepositoryItemMarqueeProgressBar
	Friend WithEvents bsiInfo As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents bbiSearch As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbiClearFields As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbiPrint As DevExpress.XtraBars.BarLargeButtonItem
	Friend WithEvents bbiExport As DevExpress.XtraBars.BarLargeButtonItem
	Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
	Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
	Friend WithEvents BarStaticItem1 As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents BarEditItem1 As DevExpress.XtraBars.BarEditItem
	Friend WithEvents RepositoryItemMarqueeProgressBar1 As DevExpress.XtraEditors.Repository.RepositoryItemMarqueeProgressBar
	Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
	Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
	Friend WithEvents cbo_MANr As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
	Friend WithEvents cbo_ESNr As DevExpress.XtraEditors.ComboBoxEdit

End Class
