<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLOGUSearch
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLOGUSearch))
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
		Me.ComboBox2 = New System.Windows.Forms.ComboBox()
		Me.TextBox24 = New DevExpress.XtraEditors.TextEdit()
		Me.lblSQLAbfrage = New System.Windows.Forms.Label()
		Me.cbo_JahrBis = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.cbo_MonatBis = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.Label4 = New System.Windows.Forms.Label()
		Me.Label3 = New System.Windows.Forms.Label()
		Me.Chk13LohnVJ = New DevExpress.XtraEditors.CheckEdit()
		Me.ChkFerienVJ = New DevExpress.XtraEditors.CheckEdit()
		Me.ChkFeiertagVJ = New DevExpress.XtraEditors.CheckEdit()
		Me.ChkGleitstunden = New DevExpress.XtraEditors.CheckEdit()
		Me.ChkDarlehen = New DevExpress.XtraEditors.CheckEdit()
		Me.Chk13Lohn = New DevExpress.XtraEditors.CheckEdit()
		Me.ChkFerien = New DevExpress.XtraEditors.CheckEdit()
		Me.ChkFeiertag = New DevExpress.XtraEditors.CheckEdit()
		Me.LibMANr = New DevExpress.XtraEditors.LabelControl()
		Me.xtabMain = New DevExpress.XtraTab.XtraTabControl()
		Me.xtabAllgemein = New DevExpress.XtraTab.XtraTabPage()
		Me.lblToleranzgrenze = New DevExpress.XtraEditors.LabelControl()
		Me.lueMandant = New DevExpress.XtraEditors.LookUpEdit()
		Me.lblMDName = New System.Windows.Forms.Label()
		Me.grpDarlehen = New DevComponents.DotNetBar.Controls.GroupPanel()
		Me.grpGuthabenvorjahr = New DevComponents.DotNetBar.Controls.GroupPanel()
		Me.grpGuthaben = New DevComponents.DotNetBar.Controls.GroupPanel()
		Me.txt_MANr = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.txtTolerancelimit = New DevExpress.XtraEditors.TextEdit()
		Me.xtabSQL = New DevExpress.XtraTab.XtraTabPage()
		Me.txt_SQLQuery = New DevExpress.XtraEditors.MemoEdit()
		Me.GroupBox1 = New DevExpress.XtraEditors.PanelControl()
		Me.LblformImage = New System.Windows.Forms.Label()
		Me.CmdClose = New DevExpress.XtraEditors.SimpleButton()
		Me.lblHeaderNormal = New System.Windows.Forms.Label()
		Me.lblHeaderFett = New System.Windows.Forms.Label()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
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
		Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
		CType(Me.TextBox24.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.cbo_JahrBis.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.cbo_MonatBis.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Chk13LohnVJ.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.ChkFerienVJ.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.ChkFeiertagVJ.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.ChkGleitstunden.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.ChkDarlehen.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Chk13Lohn.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.ChkFerien.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.ChkFeiertag.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.xtabMain, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.xtabMain.SuspendLayout()
		Me.xtabAllgemein.SuspendLayout()
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grpDarlehen.SuspendLayout()
		Me.grpGuthabenvorjahr.SuspendLayout()
		Me.grpGuthaben.SuspendLayout()
		CType(Me.txt_MANr.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtTolerancelimit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.xtabSQL.SuspendLayout()
		CType(Me.txt_SQLQuery.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupBox1.SuspendLayout()
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
		'lblSQLAbfrage
		'
		Me.lblSQLAbfrage.AutoSize = True
		Me.lblSQLAbfrage.Location = New System.Drawing.Point(30, 28)
		Me.lblSQLAbfrage.Name = "lblSQLAbfrage"
		Me.lblSQLAbfrage.Size = New System.Drawing.Size(154, 13)
		Me.lblSQLAbfrage.TabIndex = 73
		Me.lblSQLAbfrage.Text = "Ihre derzeitige Abfrage lautet:"
		'
		'cbo_JahrBis
		'
		Me.cbo_JahrBis.Location = New System.Drawing.Point(144, 60)
		Me.cbo_JahrBis.Name = "cbo_JahrBis"
		Me.cbo_JahrBis.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.DropDown)})
		Me.cbo_JahrBis.Size = New System.Drawing.Size(99, 20)
		Me.cbo_JahrBis.TabIndex = 0
		'
		'cbo_MonatBis
		'
		Me.cbo_MonatBis.Location = New System.Drawing.Point(144, 86)
		Me.cbo_MonatBis.Name = "cbo_MonatBis"
		Me.cbo_MonatBis.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.DropDown)})
		Me.cbo_MonatBis.Size = New System.Drawing.Size(99, 20)
		Me.cbo_MonatBis.TabIndex = 1
		'
		'Label4
		'
		Me.Label4.Location = New System.Drawing.Point(18, 64)
		Me.Label4.Name = "Label4"
		Me.Label4.Size = New System.Drawing.Size(120, 13)
		Me.Label4.TabIndex = 294
		Me.Label4.Text = "Jahr bis"
		Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'Label3
		'
		Me.Label3.Location = New System.Drawing.Point(18, 90)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(120, 13)
		Me.Label3.TabIndex = 293
		Me.Label3.Text = "Monat bis"
		Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'Chk13LohnVJ
		'
		Me.Chk13LohnVJ.Location = New System.Drawing.Point(17, 60)
		Me.Chk13LohnVJ.Name = "Chk13LohnVJ"
		Me.Chk13LohnVJ.Properties.Caption = "13. Monatslohn Vorjahr"
		Me.Chk13LohnVJ.Size = New System.Drawing.Size(135, 19)
		Me.Chk13LohnVJ.TabIndex = 8
		'
		'ChkFerienVJ
		'
		Me.ChkFerienVJ.Location = New System.Drawing.Point(17, 36)
		Me.ChkFerienVJ.Name = "ChkFerienVJ"
		Me.ChkFerienVJ.Properties.Caption = "Ferienentschädigung Vorjahr"
		Me.ChkFerienVJ.Size = New System.Drawing.Size(161, 19)
		Me.ChkFerienVJ.TabIndex = 7
		'
		'ChkFeiertagVJ
		'
		Me.ChkFeiertagVJ.Location = New System.Drawing.Point(17, 12)
		Me.ChkFeiertagVJ.Name = "ChkFeiertagVJ"
		Me.ChkFeiertagVJ.Properties.Caption = "Feiertagsentschädigung Vorjahr"
		Me.ChkFeiertagVJ.Size = New System.Drawing.Size(175, 19)
		Me.ChkFeiertagVJ.TabIndex = 6
		'
		'ChkGleitstunden
		'
		Me.ChkGleitstunden.Location = New System.Drawing.Point(320, 7)
		Me.ChkGleitstunden.Name = "ChkGleitstunden"
		Me.ChkGleitstunden.Properties.Caption = "Gleitstunden"
		Me.ChkGleitstunden.Size = New System.Drawing.Size(85, 19)
		Me.ChkGleitstunden.TabIndex = 10
		'
		'ChkDarlehen
		'
		Me.ChkDarlehen.Location = New System.Drawing.Point(13, 7)
		Me.ChkDarlehen.Name = "ChkDarlehen"
		Me.ChkDarlehen.Properties.Caption = "Darlehen"
		Me.ChkDarlehen.Size = New System.Drawing.Size(69, 19)
		Me.ChkDarlehen.TabIndex = 9
		'
		'Chk13Lohn
		'
		Me.Chk13Lohn.Location = New System.Drawing.Point(13, 58)
		Me.Chk13Lohn.Name = "Chk13Lohn"
		Me.Chk13Lohn.Properties.Caption = "13. Monatslohn"
		Me.Chk13Lohn.Size = New System.Drawing.Size(99, 19)
		Me.Chk13Lohn.TabIndex = 5
		'
		'ChkFerien
		'
		Me.ChkFerien.Location = New System.Drawing.Point(13, 34)
		Me.ChkFerien.Name = "ChkFerien"
		Me.ChkFerien.Properties.Caption = "Ferienentschädigung"
		Me.ChkFerien.Size = New System.Drawing.Size(125, 19)
		Me.ChkFerien.TabIndex = 4
		'
		'ChkFeiertag
		'
		Me.ChkFeiertag.Location = New System.Drawing.Point(13, 10)
		Me.ChkFeiertag.Name = "ChkFeiertag"
		Me.ChkFeiertag.Properties.Caption = "Feiertagsentschädigung"
		Me.ChkFeiertag.Size = New System.Drawing.Size(139, 19)
		Me.ChkFeiertag.TabIndex = 3
		'
		'LibMANr
		'
		Me.LibMANr.Appearance.Options.UseTextOptions = True
		Me.LibMANr.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.LibMANr.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.LibMANr.Location = New System.Drawing.Point(18, 136)
		Me.LibMANr.Name = "LibMANr"
		Me.LibMANr.Size = New System.Drawing.Size(120, 13)
		Me.LibMANr.TabIndex = 284
		Me.LibMANr.TabStop = True
		Me.LibMANr.Text = "Kandidaten-Nr."
		'
		'xtabMain
		'
		Me.xtabMain.Location = New System.Drawing.Point(43, 118)
		Me.xtabMain.Name = "xtabMain"
		Me.xtabMain.SelectedTabPage = Me.xtabAllgemein
		Me.xtabMain.Size = New System.Drawing.Size(646, 448)
		Me.xtabMain.TabIndex = 201
		Me.xtabMain.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabAllgemein, Me.xtabSQL})
		'
		'xtabAllgemein
		'
		Me.xtabAllgemein.Controls.Add(Me.lblToleranzgrenze)
		Me.xtabAllgemein.Controls.Add(Me.lueMandant)
		Me.xtabAllgemein.Controls.Add(Me.lblMDName)
		Me.xtabAllgemein.Controls.Add(Me.cbo_MonatBis)
		Me.xtabAllgemein.Controls.Add(Me.Label3)
		Me.xtabAllgemein.Controls.Add(Me.cbo_JahrBis)
		Me.xtabAllgemein.Controls.Add(Me.LibMANr)
		Me.xtabAllgemein.Controls.Add(Me.Label4)
		Me.xtabAllgemein.Controls.Add(Me.grpDarlehen)
		Me.xtabAllgemein.Controls.Add(Me.grpGuthabenvorjahr)
		Me.xtabAllgemein.Controls.Add(Me.grpGuthaben)
		Me.xtabAllgemein.Controls.Add(Me.txt_MANr)
		Me.xtabAllgemein.Controls.Add(Me.txtTolerancelimit)
		Me.xtabAllgemein.Name = "xtabAllgemein"
		Me.xtabAllgemein.Size = New System.Drawing.Size(640, 420)
		Me.xtabAllgemein.Text = "Allgemein"
		'
		'lblToleranzgrenze
		'
		Me.lblToleranzgrenze.Appearance.Options.UseTextOptions = True
		Me.lblToleranzgrenze.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblToleranzgrenze.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblToleranzgrenze.Location = New System.Drawing.Point(18, 379)
		Me.lblToleranzgrenze.Name = "lblToleranzgrenze"
		Me.lblToleranzgrenze.Size = New System.Drawing.Size(120, 13)
		Me.lblToleranzgrenze.TabIndex = 296
		Me.lblToleranzgrenze.TabStop = True
		Me.lblToleranzgrenze.Text = "Toleranzgrenze"
		'
		'lueMandant
		'
		Me.lueMandant.Location = New System.Drawing.Point(144, 34)
		Me.lueMandant.Name = "lueMandant"
		Me.lueMandant.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.lueMandant.Properties.NullText = ""
		Me.lueMandant.Size = New System.Drawing.Size(431, 20)
		Me.lueMandant.TabIndex = 241
		'
		'lblMDName
		'
		Me.lblMDName.Location = New System.Drawing.Point(18, 38)
		Me.lblMDName.Name = "lblMDName"
		Me.lblMDName.Size = New System.Drawing.Size(120, 13)
		Me.lblMDName.TabIndex = 242
		Me.lblMDName.Text = "Mandanten"
		Me.lblMDName.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'grpDarlehen
		'
		Me.grpDarlehen.BackColor = System.Drawing.Color.Transparent
		Me.grpDarlehen.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
		Me.grpDarlehen.Controls.Add(Me.ChkGleitstunden)
		Me.grpDarlehen.Controls.Add(Me.ChkDarlehen)
		Me.grpDarlehen.Font = New System.Drawing.Font("Tahoma", 8.25!)
		Me.grpDarlehen.Location = New System.Drawing.Point(30, 300)
		Me.grpDarlehen.Name = "grpDarlehen"
		Me.grpDarlehen.Size = New System.Drawing.Size(579, 60)
		'
		'
		'
		Me.grpDarlehen.Style.BackColorGradientAngle = 90
		Me.grpDarlehen.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.grpDarlehen.Style.BorderBottomWidth = 1
		Me.grpDarlehen.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
		Me.grpDarlehen.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.grpDarlehen.Style.BorderLeftWidth = 1
		Me.grpDarlehen.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.grpDarlehen.Style.BorderRightWidth = 1
		Me.grpDarlehen.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.grpDarlehen.Style.BorderTopWidth = 1
		Me.grpDarlehen.Style.CornerDiameter = 4
		Me.grpDarlehen.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
		Me.grpDarlehen.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.grpDarlehen.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
		Me.grpDarlehen.Style.TextColor = System.Drawing.Color.Black
		Me.grpDarlehen.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
		'
		'
		'
		Me.grpDarlehen.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
		'
		'
		'
		Me.grpDarlehen.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
		Me.grpDarlehen.TabIndex = 259
		Me.grpDarlehen.Text = "Darlehen und Gleitstunden"
		'
		'grpGuthabenvorjahr
		'
		Me.grpGuthabenvorjahr.BackColor = System.Drawing.Color.Transparent
		Me.grpGuthabenvorjahr.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
		Me.grpGuthabenvorjahr.Controls.Add(Me.Chk13LohnVJ)
		Me.grpGuthabenvorjahr.Controls.Add(Me.ChkFeiertagVJ)
		Me.grpGuthabenvorjahr.Controls.Add(Me.ChkFerienVJ)
		Me.grpGuthabenvorjahr.Font = New System.Drawing.Font("Tahoma", 8.25!)
		Me.grpGuthabenvorjahr.Location = New System.Drawing.Point(333, 172)
		Me.grpGuthabenvorjahr.Name = "grpGuthabenvorjahr"
		Me.grpGuthabenvorjahr.Size = New System.Drawing.Size(276, 113)
		'
		'
		'
		Me.grpGuthabenvorjahr.Style.BackColorGradientAngle = 90
		Me.grpGuthabenvorjahr.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.grpGuthabenvorjahr.Style.BorderBottomWidth = 1
		Me.grpGuthabenvorjahr.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
		Me.grpGuthabenvorjahr.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.grpGuthabenvorjahr.Style.BorderLeftWidth = 1
		Me.grpGuthabenvorjahr.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.grpGuthabenvorjahr.Style.BorderRightWidth = 1
		Me.grpGuthabenvorjahr.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.grpGuthabenvorjahr.Style.BorderTopWidth = 1
		Me.grpGuthabenvorjahr.Style.CornerDiameter = 4
		Me.grpGuthabenvorjahr.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
		Me.grpGuthabenvorjahr.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.grpGuthabenvorjahr.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
		Me.grpGuthabenvorjahr.Style.TextColor = System.Drawing.Color.Black
		Me.grpGuthabenvorjahr.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
		'
		'
		'
		Me.grpGuthabenvorjahr.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
		'
		'
		'
		Me.grpGuthabenvorjahr.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
		Me.grpGuthabenvorjahr.TabIndex = 258
		Me.grpGuthabenvorjahr.Text = "Guthaben Vorjahr"
		'
		'grpGuthaben
		'
		Me.grpGuthaben.BackColor = System.Drawing.Color.Transparent
		Me.grpGuthaben.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
		Me.grpGuthaben.Controls.Add(Me.Chk13Lohn)
		Me.grpGuthaben.Controls.Add(Me.ChkFeiertag)
		Me.grpGuthaben.Controls.Add(Me.ChkFerien)
		Me.grpGuthaben.Font = New System.Drawing.Font("Tahoma", 8.25!)
		Me.grpGuthaben.Location = New System.Drawing.Point(30, 172)
		Me.grpGuthaben.Name = "grpGuthaben"
		Me.grpGuthaben.Size = New System.Drawing.Size(276, 113)
		'
		'
		'
		Me.grpGuthaben.Style.BackColorGradientAngle = 90
		Me.grpGuthaben.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.grpGuthaben.Style.BorderBottomWidth = 1
		Me.grpGuthaben.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
		Me.grpGuthaben.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.grpGuthaben.Style.BorderLeftWidth = 1
		Me.grpGuthaben.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.grpGuthaben.Style.BorderRightWidth = 1
		Me.grpGuthaben.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.grpGuthaben.Style.BorderTopWidth = 1
		Me.grpGuthaben.Style.CornerDiameter = 4
		Me.grpGuthaben.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
		Me.grpGuthaben.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.grpGuthaben.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
		Me.grpGuthaben.Style.TextColor = System.Drawing.Color.Black
		Me.grpGuthaben.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
		'
		'
		'
		Me.grpGuthaben.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
		'
		'
		'
		Me.grpGuthaben.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
		Me.grpGuthaben.TabIndex = 257
		Me.grpGuthaben.Text = "Guthaben"
		'
		'txt_MANr
		'
		Me.txt_MANr.Location = New System.Drawing.Point(144, 133)
		Me.txt_MANr.Name = "txt_MANr"
		Me.txt_MANr.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.txt_MANr.Size = New System.Drawing.Size(431, 20)
		Me.txt_MANr.TabIndex = 2
		'
		'txtTolerancelimit
		'
		Me.txtTolerancelimit.Location = New System.Drawing.Point(144, 376)
		Me.txtTolerancelimit.Name = "txtTolerancelimit"
		Me.txtTolerancelimit.Properties.DisplayFormat.FormatString = "0.00"
		Me.txtTolerancelimit.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		Me.txtTolerancelimit.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		Me.txtTolerancelimit.Size = New System.Drawing.Size(99, 20)
		Me.txtTolerancelimit.TabIndex = 295
		'
		'xtabSQL
		'
		Me.xtabSQL.Controls.Add(Me.lblSQLAbfrage)
		Me.xtabSQL.Controls.Add(Me.txt_SQLQuery)
		Me.xtabSQL.Name = "xtabSQL"
		Me.xtabSQL.Size = New System.Drawing.Size(640, 420)
		Me.xtabSQL.Text = "SQL-Abfrage"
		'
		'txt_SQLQuery
		'
		Me.txt_SQLQuery.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txt_SQLQuery.Location = New System.Drawing.Point(30, 47)
		Me.txt_SQLQuery.Name = "txt_SQLQuery"
		Me.txt_SQLQuery.Size = New System.Drawing.Size(590, 348)
		Me.txt_SQLQuery.TabIndex = 1
		'
		'GroupBox1
		'
		Me.GroupBox1.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.GroupBox1.Appearance.Options.UseBackColor = True
		Me.GroupBox1.Controls.Add(Me.LblformImage)
		Me.GroupBox1.Controls.Add(Me.CmdClose)
		Me.GroupBox1.Controls.Add(Me.lblHeaderNormal)
		Me.GroupBox1.Controls.Add(Me.lblHeaderFett)
		Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
		Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(731, 93)
		Me.GroupBox1.TabIndex = 240
		'
		'LblformImage
		'
		Me.LblformImage.BackColor = System.Drawing.Color.Transparent
		Me.LblformImage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.LblformImage.ForeColor = System.Drawing.SystemColors.HotTrack
		Me.LblformImage.Image = CType(resources.GetObject("LblformImage.Image"), System.Drawing.Image)
		Me.LblformImage.Location = New System.Drawing.Point(13, 16)
		Me.LblformImage.Name = "LblformImage"
		Me.LblformImage.Size = New System.Drawing.Size(72, 66)
		Me.LblformImage.TabIndex = 1000
		'
		'CmdClose
		'
		Me.CmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.CmdClose.Location = New System.Drawing.Point(578, 29)
		Me.CmdClose.Name = "CmdClose"
		Me.CmdClose.Size = New System.Drawing.Size(86, 24)
		Me.CmdClose.TabIndex = 999
		Me.CmdClose.Text = "Schliessen"
		'
		'lblHeaderNormal
		'
		Me.lblHeaderNormal.AutoSize = True
		Me.lblHeaderNormal.BackColor = System.Drawing.Color.Transparent
		Me.lblHeaderNormal.Location = New System.Drawing.Point(109, 53)
		Me.lblHeaderNormal.Name = "lblHeaderNormal"
		Me.lblHeaderNormal.Size = New System.Drawing.Size(225, 13)
		Me.lblHeaderNormal.TabIndex = 1
		Me.lblHeaderNormal.Text = "Wählen Sie Ihre gewünschten Kriterien aus..."
		'
		'lblHeaderFett
		'
		Me.lblHeaderFett.AutoSize = True
		Me.lblHeaderFett.BackColor = System.Drawing.Color.Transparent
		Me.lblHeaderFett.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblHeaderFett.Location = New System.Drawing.Point(91, 29)
		Me.lblHeaderFett.Name = "lblHeaderFett"
		Me.lblHeaderFett.Size = New System.Drawing.Size(217, 13)
		Me.lblHeaderFett.TabIndex = 0
		Me.lblHeaderFett.Text = "Liste der Guthaben eines Kandidaten"
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
		Me.barDockControlTop.Size = New System.Drawing.Size(731, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 602)
		Me.barDockControlBottom.Manager = Me.BarManager1
		Me.barDockControlBottom.Size = New System.Drawing.Size(731, 27)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Manager = Me.BarManager1
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 602)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(731, 0)
		Me.barDockControlRight.Manager = Me.BarManager1
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 602)
		'
		'BackgroundWorker1
		'
		'
		'frmLOGUSearch
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(731, 629)
		Me.Controls.Add(Me.GroupBox1)
		Me.Controls.Add(Me.xtabMain)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.MaximumSize = New System.Drawing.Size(747, 668)
		Me.MinimumSize = New System.Drawing.Size(747, 668)
		Me.Name = "frmLOGUSearch"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Jährliche Lohnlisten: Guthaben eines Kandidaten"
		CType(Me.TextBox24.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.cbo_JahrBis.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.cbo_MonatBis.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Chk13LohnVJ.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.ChkFerienVJ.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.ChkFeiertagVJ.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.ChkGleitstunden.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.ChkDarlehen.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Chk13Lohn.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.ChkFerien.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.ChkFeiertag.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.xtabMain, System.ComponentModel.ISupportInitialize).EndInit()
		Me.xtabMain.ResumeLayout(False)
		Me.xtabAllgemein.ResumeLayout(False)
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.grpDarlehen.ResumeLayout(False)
		Me.grpGuthabenvorjahr.ResumeLayout(False)
		Me.grpGuthaben.ResumeLayout(False)
		CType(Me.txt_MANr.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtTolerancelimit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.xtabSQL.ResumeLayout(False)
		Me.xtabSQL.PerformLayout()
		CType(Me.txt_SQLQuery.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupBox1.ResumeLayout(False)
		Me.GroupBox1.PerformLayout()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

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
	Friend WithEvents TextBox13 As DevExpress.XtraEditors.TextEdit
	Friend WithEvents TextBox14 As DevExpress.XtraEditors.TextEdit
	Friend WithEvents TextBox15 As DevExpress.XtraEditors.TextEdit
	Friend WithEvents TextBox16 As DevExpress.XtraEditors.TextEdit
	Friend WithEvents TextBox17 As DevExpress.XtraEditors.TextEdit
	Friend WithEvents TextBox18 As DevExpress.XtraEditors.TextEdit
	Friend WithEvents TextBox19 As DevExpress.XtraEditors.TextEdit
	Friend WithEvents TextBox20 As DevExpress.XtraEditors.TextEdit
	Friend WithEvents TextBox21 As DevExpress.XtraEditors.TextEdit
	Friend WithEvents TextBox22 As DevExpress.XtraEditors.TextEdit
	Friend WithEvents TextBox23 As DevExpress.XtraEditors.TextEdit
	Friend WithEvents ComboBox2 As System.Windows.Forms.ComboBox
	Friend WithEvents TextBox24 As DevExpress.XtraEditors.TextEdit
	Friend WithEvents lblSQLAbfrage As System.Windows.Forms.Label
	Friend WithEvents LibMANr As DevExpress.XtraEditors.LabelControl
	Friend WithEvents Chk13Lohn As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents ChkFerien As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents ChkFeiertag As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents Chk13LohnVJ As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents ChkFerienVJ As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents ChkFeiertagVJ As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents ChkGleitstunden As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents ChkDarlehen As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents cbo_MonatBis As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents Label4 As System.Windows.Forms.Label
	Friend WithEvents Label3 As System.Windows.Forms.Label
	Friend WithEvents cbo_JahrBis As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents xtabMain As DevExpress.XtraTab.XtraTabControl
	Friend WithEvents xtabAllgemein As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents grpGuthaben As DevComponents.DotNetBar.Controls.GroupPanel
	Friend WithEvents grpDarlehen As DevComponents.DotNetBar.Controls.GroupPanel
	Friend WithEvents grpGuthabenvorjahr As DevComponents.DotNetBar.Controls.GroupPanel
	Friend WithEvents txt_MANr As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents xtabSQL As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents txt_SQLQuery As DevExpress.XtraEditors.MemoEdit
	Friend WithEvents lueMandant As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents lblMDName As System.Windows.Forms.Label
	Friend WithEvents GroupBox1 As DevExpress.XtraEditors.PanelControl
	Friend WithEvents LblformImage As System.Windows.Forms.Label
	Friend WithEvents CmdClose As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents lblHeaderNormal As System.Windows.Forms.Label
	Friend WithEvents lblHeaderFett As System.Windows.Forms.Label
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
	Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
	Friend WithEvents lblToleranzgrenze As DevExpress.XtraEditors.LabelControl
	Friend WithEvents txtTolerancelimit As DevExpress.XtraEditors.TextEdit
End Class
