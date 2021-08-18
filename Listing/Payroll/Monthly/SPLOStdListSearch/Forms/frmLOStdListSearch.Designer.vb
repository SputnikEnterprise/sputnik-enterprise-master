<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLOStdListSearch
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLOStdListSearch))
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
		Me.Cbo_Filiale = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.Cbo_Kanton = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.Cbo_Beruf = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.Cbo_MonatVon = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.txt_MANr = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.Cbo_MonatBis = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.Cbo_Periode = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.Cbo_JahrBis = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.Cbo_JahrVon = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
		Me.XtraTabControl1 = New DevExpress.XtraTab.XtraTabControl()
		Me.xtabAllgemein = New DevExpress.XtraTab.XtraTabPage()
		Me.grpInfo = New DevComponents.DotNetBar.Controls.GroupPanel()
		Me.lblPeriode = New System.Windows.Forms.Label()
		Me.SwitchButton2 = New DevComponents.DotNetBar.Controls.SwitchButton()
		Me.SwitchButton1 = New DevComponents.DotNetBar.Controls.SwitchButton()
		Me.lblJahr = New System.Windows.Forms.Label()
		Me.lblMonat = New System.Windows.Forms.Label()
		Me.lblMANr = New DevExpress.XtraEditors.LabelControl()
		Me.lblFiliale = New System.Windows.Forms.Label()
		Me.lueMandant = New DevExpress.XtraEditors.LookUpEdit()
		Me.lblMDName = New System.Windows.Forms.Label()
		Me.lblKanton = New System.Windows.Forms.Label()
		Me.lblBeruf = New System.Windows.Forms.Label()
		Me.xtabSQLQuery = New DevExpress.XtraTab.XtraTabPage()
		Me.txt_SQLQuery = New DevExpress.XtraEditors.MemoEdit()
		Me.lblSQLAbfrage = New System.Windows.Forms.Label()
		Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
		Me.lblHeaderNormal = New System.Windows.Forms.Label()
		Me.CmdClose = New DevExpress.XtraEditors.SimpleButton()
		Me.Label5 = New System.Windows.Forms.Label()
		Me.lblHeaderFett = New System.Windows.Forms.Label()
		Me.Bar4 = New DevExpress.XtraBars.Bar()
		Me.bsiInfo = New DevExpress.XtraBars.BarStaticItem()
		Me.bbiSearch = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiClearFields = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiPrint = New DevExpress.XtraBars.BarLargeButtonItem()
		Me.bbiExport = New DevExpress.XtraBars.BarLargeButtonItem()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager()
		Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
		Me.BarStaticItem1 = New DevExpress.XtraBars.BarStaticItem()
		CType(Me.Cbo_Filiale.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_Kanton.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_Beruf.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_MonatVon.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txt_MANr.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_MonatBis.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_Periode.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_JahrBis.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_JahrVon.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.XtraTabControl1.SuspendLayout()
		Me.xtabAllgemein.SuspendLayout()
		Me.grpInfo.SuspendLayout()
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.xtabSQLQuery.SuspendLayout()
		CType(Me.txt_SQLQuery.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.PanelControl1.SuspendLayout()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
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
		'Cbo_Filiale
		'
		Me.Cbo_Filiale.Location = New System.Drawing.Point(158, 266)
		Me.Cbo_Filiale.Name = "Cbo_Filiale"
		Me.Cbo_Filiale.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.DropDown)})
		Me.Cbo_Filiale.Size = New System.Drawing.Size(96, 20)
		Me.Cbo_Filiale.TabIndex = 294
		'
		'Cbo_Kanton
		'
		Me.Cbo_Kanton.Location = New System.Drawing.Point(158, 292)
		Me.Cbo_Kanton.Name = "Cbo_Kanton"
		Me.Cbo_Kanton.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.DropDown)})
		Me.Cbo_Kanton.Size = New System.Drawing.Size(96, 20)
		Me.Cbo_Kanton.TabIndex = 10
		'
		'Cbo_Beruf
		'
		Me.Cbo_Beruf.Location = New System.Drawing.Point(158, 319)
		Me.Cbo_Beruf.Name = "Cbo_Beruf"
		Me.Cbo_Beruf.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.DropDown)})
		Me.Cbo_Beruf.Size = New System.Drawing.Size(384, 20)
		Me.Cbo_Beruf.TabIndex = 20
		'
		'Cbo_MonatVon
		'
		Me.Cbo_MonatVon.Location = New System.Drawing.Point(139, 65)
		Me.Cbo_MonatVon.Name = "Cbo_MonatVon"
		Me.Cbo_MonatVon.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.DropDown)})
		Me.Cbo_MonatVon.Size = New System.Drawing.Size(96, 20)
		Me.Cbo_MonatVon.TabIndex = 40
		'
		'txt_MANr
		'
		Me.txt_MANr.Location = New System.Drawing.Point(158, 71)
		Me.txt_MANr.Name = "txt_MANr"
		Me.txt_MANr.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.txt_MANr.Size = New System.Drawing.Size(384, 20)
		Me.txt_MANr.TabIndex = 20
		'
		'Cbo_MonatBis
		'
		Me.Cbo_MonatBis.Location = New System.Drawing.Point(427, 65)
		Me.Cbo_MonatBis.Name = "Cbo_MonatBis"
		Me.Cbo_MonatBis.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.DropDown)})
		Me.Cbo_MonatBis.Size = New System.Drawing.Size(96, 20)
		Me.Cbo_MonatBis.TabIndex = 50
		Me.Cbo_MonatBis.Visible = False
		'
		'Cbo_Periode
		'
		Me.Cbo_Periode.Location = New System.Drawing.Point(139, 13)
		Me.Cbo_Periode.Name = "Cbo_Periode"
		Me.Cbo_Periode.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.DropDown)})
		Me.Cbo_Periode.Size = New System.Drawing.Size(384, 20)
		Me.Cbo_Periode.TabIndex = 10
		'
		'Cbo_JahrBis
		'
		Me.Cbo_JahrBis.Location = New System.Drawing.Point(427, 39)
		Me.Cbo_JahrBis.Name = "Cbo_JahrBis"
		Me.Cbo_JahrBis.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.DropDown)})
		Me.Cbo_JahrBis.Size = New System.Drawing.Size(96, 20)
		Me.Cbo_JahrBis.TabIndex = 70
		Me.Cbo_JahrBis.Visible = False
		'
		'Cbo_JahrVon
		'
		Me.Cbo_JahrVon.Location = New System.Drawing.Point(139, 39)
		Me.Cbo_JahrVon.Name = "Cbo_JahrVon"
		Me.Cbo_JahrVon.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.DropDown)})
		Me.Cbo_JahrVon.Size = New System.Drawing.Size(96, 20)
		Me.Cbo_JahrVon.TabIndex = 60
		'
		'BackgroundWorker1
		'
		'
		'XtraTabControl1
		'
		Me.XtraTabControl1.Location = New System.Drawing.Point(23, 100)
		Me.XtraTabControl1.Name = "XtraTabControl1"
		Me.XtraTabControl1.SelectedTabPage = Me.xtabAllgemein
		Me.XtraTabControl1.Size = New System.Drawing.Size(596, 407)
		Me.XtraTabControl1.TabIndex = 300
		Me.XtraTabControl1.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabAllgemein, Me.xtabSQLQuery})
		'
		'xtabAllgemein
		'
		Me.xtabAllgemein.Controls.Add(Me.grpInfo)
		Me.xtabAllgemein.Controls.Add(Me.lblMANr)
		Me.xtabAllgemein.Controls.Add(Me.txt_MANr)
		Me.xtabAllgemein.Controls.Add(Me.lblFiliale)
		Me.xtabAllgemein.Controls.Add(Me.Cbo_Kanton)
		Me.xtabAllgemein.Controls.Add(Me.lueMandant)
		Me.xtabAllgemein.Controls.Add(Me.Cbo_Beruf)
		Me.xtabAllgemein.Controls.Add(Me.Cbo_Filiale)
		Me.xtabAllgemein.Controls.Add(Me.lblMDName)
		Me.xtabAllgemein.Controls.Add(Me.lblKanton)
		Me.xtabAllgemein.Controls.Add(Me.lblBeruf)
		Me.xtabAllgemein.Name = "xtabAllgemein"
		Me.xtabAllgemein.Size = New System.Drawing.Size(590, 379)
		Me.xtabAllgemein.Text = "Allgemein"
		'
		'grpInfo
		'
		Me.grpInfo.BackColor = System.Drawing.Color.Transparent
		Me.grpInfo.CanvasColor = System.Drawing.SystemColors.Control
		Me.grpInfo.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
		Me.grpInfo.Controls.Add(Me.lblPeriode)
		Me.grpInfo.Controls.Add(Me.SwitchButton2)
		Me.grpInfo.Controls.Add(Me.SwitchButton1)
		Me.grpInfo.Controls.Add(Me.Cbo_Periode)
		Me.grpInfo.Controls.Add(Me.Cbo_JahrVon)
		Me.grpInfo.Controls.Add(Me.Cbo_JahrBis)
		Me.grpInfo.Controls.Add(Me.lblJahr)
		Me.grpInfo.Controls.Add(Me.Cbo_MonatVon)
		Me.grpInfo.Controls.Add(Me.Cbo_MonatBis)
		Me.grpInfo.Controls.Add(Me.lblMonat)
		Me.grpInfo.Location = New System.Drawing.Point(16, 118)
		Me.grpInfo.Name = "grpInfo"
		Me.grpInfo.Size = New System.Drawing.Size(559, 120)
		'
		'
		'
		Me.grpInfo.Style.BackColorGradientAngle = 90
		Me.grpInfo.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.grpInfo.Style.BorderBottomWidth = 1
		Me.grpInfo.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
		Me.grpInfo.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.grpInfo.Style.BorderLeftWidth = 1
		Me.grpInfo.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.grpInfo.Style.BorderRightWidth = 1
		Me.grpInfo.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.grpInfo.Style.BorderTopWidth = 1
		Me.grpInfo.Style.CornerDiameter = 4
		Me.grpInfo.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
		Me.grpInfo.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.grpInfo.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
		Me.grpInfo.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
		Me.grpInfo.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
		'
		'
		'
		Me.grpInfo.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
		'
		'
		'
		Me.grpInfo.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
		Me.grpInfo.TabIndex = 307
		Me.grpInfo.Text = "Periode"
		'
		'lblPeriode
		'
		Me.lblPeriode.Location = New System.Drawing.Point(3, 16)
		Me.lblPeriode.Name = "lblPeriode"
		Me.lblPeriode.Size = New System.Drawing.Size(130, 13)
		Me.lblPeriode.TabIndex = 298
		Me.lblPeriode.Text = "Periode"
		Me.lblPeriode.TextAlign = System.Drawing.ContentAlignment.TopRight
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
		Me.SwitchButton2.Location = New System.Drawing.Point(383, 40)
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
		Me.SwitchButton2.TabIndex = 306
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
		Me.SwitchButton1.Location = New System.Drawing.Point(383, 66)
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
		Me.SwitchButton1.TabIndex = 305
		'
		'lblJahr
		'
		Me.lblJahr.Location = New System.Drawing.Point(3, 42)
		Me.lblJahr.Name = "lblJahr"
		Me.lblJahr.Size = New System.Drawing.Size(130, 13)
		Me.lblJahr.TabIndex = 258
		Me.lblJahr.Text = "Jahr von"
		Me.lblJahr.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblMonat
		'
		Me.lblMonat.Location = New System.Drawing.Point(3, 68)
		Me.lblMonat.Name = "lblMonat"
		Me.lblMonat.Size = New System.Drawing.Size(130, 13)
		Me.lblMonat.TabIndex = 254
		Me.lblMonat.Text = "Monat von"
		Me.lblMonat.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblMANr
		'
		Me.lblMANr.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblMANr.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblMANr.Location = New System.Drawing.Point(13, 74)
		Me.lblMANr.Name = "lblMANr"
		Me.lblMANr.Size = New System.Drawing.Size(139, 13)
		Me.lblMANr.TabIndex = 284
		Me.lblMANr.TabStop = True
		Me.lblMANr.Text = "Kandidaten-Nr."
		'
		'lblFiliale
		'
		Me.lblFiliale.Location = New System.Drawing.Point(13, 269)
		Me.lblFiliale.Name = "lblFiliale"
		Me.lblFiliale.Size = New System.Drawing.Size(139, 20)
		Me.lblFiliale.TabIndex = 297
		Me.lblFiliale.Text = "Filiale"
		Me.lblFiliale.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lueMandant
		'
		Me.lueMandant.Location = New System.Drawing.Point(158, 29)
		Me.lueMandant.Name = "lueMandant"
		Me.lueMandant.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.lueMandant.Properties.NullText = ""
		Me.lueMandant.Size = New System.Drawing.Size(384, 20)
		Me.lueMandant.TabIndex = 295
		'
		'lblMDName
		'
		Me.lblMDName.Location = New System.Drawing.Point(13, 33)
		Me.lblMDName.Name = "lblMDName"
		Me.lblMDName.Size = New System.Drawing.Size(139, 13)
		Me.lblMDName.TabIndex = 296
		Me.lblMDName.Text = "Mandanten"
		Me.lblMDName.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblKanton
		'
		Me.lblKanton.Location = New System.Drawing.Point(55, 295)
		Me.lblKanton.Name = "lblKanton"
		Me.lblKanton.Size = New System.Drawing.Size(97, 20)
		Me.lblKanton.TabIndex = 275
		Me.lblKanton.Text = "Kanton"
		Me.lblKanton.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblBeruf
		'
		Me.lblBeruf.Location = New System.Drawing.Point(13, 322)
		Me.lblBeruf.Name = "lblBeruf"
		Me.lblBeruf.Size = New System.Drawing.Size(139, 20)
		Me.lblBeruf.TabIndex = 273
		Me.lblBeruf.Text = "Beruf"
		Me.lblBeruf.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'xtabSQLQuery
		'
		Me.xtabSQLQuery.Controls.Add(Me.txt_SQLQuery)
		Me.xtabSQLQuery.Controls.Add(Me.lblSQLAbfrage)
		Me.xtabSQLQuery.Name = "xtabSQLQuery"
		Me.xtabSQLQuery.Size = New System.Drawing.Size(590, 379)
		Me.xtabSQLQuery.Text = "SQL-Abfrage"
		'
		'txt_SQLQuery
		'
		Me.txt_SQLQuery.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txt_SQLQuery.Location = New System.Drawing.Point(22, 38)
		Me.txt_SQLQuery.Name = "txt_SQLQuery"
		Me.txt_SQLQuery.Size = New System.Drawing.Size(550, 324)
		Me.txt_SQLQuery.TabIndex = 101
		Me.txt_SQLQuery.UseOptimizedRendering = True
		'
		'lblSQLAbfrage
		'
		Me.lblSQLAbfrage.AutoSize = True
		Me.lblSQLAbfrage.Location = New System.Drawing.Point(19, 22)
		Me.lblSQLAbfrage.Name = "lblSQLAbfrage"
		Me.lblSQLAbfrage.Size = New System.Drawing.Size(150, 13)
		Me.lblSQLAbfrage.TabIndex = 73
		Me.lblSQLAbfrage.Text = "Ihre derzeitige Abfrage lautet"
		'
		'PanelControl1
		'
		Me.PanelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003
		Me.PanelControl1.Controls.Add(Me.lblHeaderNormal)
		Me.PanelControl1.Controls.Add(Me.CmdClose)
		Me.PanelControl1.Controls.Add(Me.Label5)
		Me.PanelControl1.Controls.Add(Me.lblHeaderFett)
		Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Top
		Me.PanelControl1.Location = New System.Drawing.Point(0, 0)
		Me.PanelControl1.Name = "PanelControl1"
		Me.PanelControl1.Size = New System.Drawing.Size(647, 77)
		Me.PanelControl1.TabIndex = 299
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
		Me.CmdClose.Location = New System.Drawing.Point(514, 21)
		Me.CmdClose.Name = "CmdClose"
		Me.CmdClose.Size = New System.Drawing.Size(100, 25)
		Me.CmdClose.TabIndex = 204
		Me.CmdClose.Text = "Schliessen"
		'
		'Label5
		'
		Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label5.ForeColor = System.Drawing.SystemColors.HotTrack
		Me.Label5.Image = CType(resources.GetObject("Label5.Image"), System.Drawing.Image)
		Me.Label5.Location = New System.Drawing.Point(5, 8)
		Me.Label5.Name = "Label5"
		Me.Label5.Size = New System.Drawing.Size(83, 65)
		Me.Label5.TabIndex = 1000
		'
		'lblHeaderFett
		'
		Me.lblHeaderFett.AutoSize = True
		Me.lblHeaderFett.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblHeaderFett.Location = New System.Drawing.Point(94, 21)
		Me.lblHeaderFett.Name = "lblHeaderFett"
		Me.lblHeaderFett.Size = New System.Drawing.Size(144, 13)
		Me.lblHeaderFett.TabIndex = 0
		Me.lblHeaderFett.Text = "Liste der Arbeitsstunden"
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
		Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiInfo, Me.bbiSearch, Me.bbiPrint, Me.bbiClearFields, Me.BarStaticItem1, Me.bbiExport})
		Me.BarManager1.MaxItemId = 10
		Me.BarManager1.StatusBar = Me.Bar4
		'
		'barDockControlTop
		'
		Me.barDockControlTop.CausesValidation = False
		Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
		Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlTop.Size = New System.Drawing.Size(647, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 554)
		Me.barDockControlBottom.Size = New System.Drawing.Size(647, 27)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 554)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(647, 0)
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 554)
		'
		'BarStaticItem1
		'
		Me.BarStaticItem1.Caption = " "
		Me.BarStaticItem1.Id = 5
		Me.BarStaticItem1.Name = "BarStaticItem1"
		Me.BarStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near
		'
		'frmLOStdListSearch
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(647, 581)
		Me.Controls.Add(Me.XtraTabControl1)
		Me.Controls.Add(Me.PanelControl1)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.MaximumSize = New System.Drawing.Size(663, 619)
		Me.MinimumSize = New System.Drawing.Size(663, 619)
		Me.Name = "frmLOStdListSearch"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Monatliche Lohnlisten"
		CType(Me.Cbo_Filiale.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_Kanton.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_Beruf.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_MonatVon.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txt_MANr.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_MonatBis.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_Periode.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_JahrBis.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_JahrVon.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.XtraTabControl1.ResumeLayout(False)
		Me.xtabAllgemein.ResumeLayout(False)
		Me.grpInfo.ResumeLayout(False)
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.xtabSQLQuery.ResumeLayout(False)
		Me.xtabSQLQuery.PerformLayout()
		CType(Me.txt_SQLQuery.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.PanelControl1.ResumeLayout(False)
		Me.PanelControl1.PerformLayout()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
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
	Friend WithEvents Cbo_Beruf As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents Cbo_JahrVon As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents Cbo_MonatBis As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents Cbo_MonatVon As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents Cbo_JahrBis As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents txt_MANr As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents Cbo_Periode As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents Cbo_Kanton As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
	Friend WithEvents Cbo_Filiale As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents XtraTabControl1 As DevExpress.XtraTab.XtraTabControl
	Friend WithEvents xtabAllgemein As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents lblMANr As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lueMandant As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents lblMDName As System.Windows.Forms.Label
	Friend WithEvents lblMonat As System.Windows.Forms.Label
	Friend WithEvents lblJahr As System.Windows.Forms.Label
	Friend WithEvents lblKanton As System.Windows.Forms.Label
	Friend WithEvents lblBeruf As System.Windows.Forms.Label
	Friend WithEvents xtabSQLQuery As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents lblSQLAbfrage As System.Windows.Forms.Label
	Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
	Friend WithEvents lblHeaderNormal As System.Windows.Forms.Label
	Friend WithEvents CmdClose As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents Label5 As System.Windows.Forms.Label
	Friend WithEvents lblHeaderFett As System.Windows.Forms.Label
	Friend WithEvents Bar4 As DevExpress.XtraBars.Bar
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
	Friend WithEvents lblFiliale As System.Windows.Forms.Label
	Friend WithEvents lblPeriode As System.Windows.Forms.Label
	Friend WithEvents SwitchButton2 As DevComponents.DotNetBar.Controls.SwitchButton
	Friend WithEvents SwitchButton1 As DevComponents.DotNetBar.Controls.SwitchButton
	Friend WithEvents grpInfo As DevComponents.DotNetBar.Controls.GroupPanel
	Friend WithEvents txt_SQLQuery As DevExpress.XtraEditors.MemoEdit

End Class
