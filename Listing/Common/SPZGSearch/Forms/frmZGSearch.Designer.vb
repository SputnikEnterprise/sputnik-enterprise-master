<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmZGSearch
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmZGSearch))
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
    Me.lblderzeitigeabfrage = New System.Windows.Forms.Label()
    Me.txt_SQLQuery = New DevExpress.XtraEditors.MemoEdit()
    Me.lblAbfrage = New System.Windows.Forms.Label()
    Me.txt_IndSQLQuery = New DevExpress.XtraEditors.MemoEdit()
    Me.lblkandidat = New System.Windows.Forms.Label()
    Me.lblNummer = New System.Windows.Forms.Label()
    Me.CboSort = New DevExpress.XtraEditors.CheckedComboBoxEdit()
    Me.Cbo_Berater = New DevExpress.XtraEditors.ComboBoxEdit()
    Me.lblberater = New System.Windows.Forms.Label()
    Me.Cbo_Year = New DevExpress.XtraEditors.ComboBoxEdit()
    Me.GroupBox2 = New System.Windows.Forms.GroupBox()
    Me.lblzahlungam = New System.Windows.Forms.Label()
    Me.lbldruckam = New System.Windows.Forms.Label()
    Me.dePrintedOn_2 = New DevExpress.XtraEditors.DateEdit()
    Me.deOuton_1 = New DevExpress.XtraEditors.DateEdit()
    Me.deOuton_2 = New DevExpress.XtraEditors.DateEdit()
    Me.dePrintedOn_1 = New DevExpress.XtraEditors.DateEdit()
    Me.Cbo_Filiale = New DevExpress.XtraEditors.ComboBoxEdit()
    Me.lblfiliale = New System.Windows.Forms.Label()
    Me.Cbo_LANr = New DevExpress.XtraEditors.ComboBoxEdit()
    Me.Cbo_Currency = New DevExpress.XtraEditors.ComboBoxEdit()
    Me.Cbo_Month = New DevExpress.XtraEditors.ComboBoxEdit()
    Me.Cbo_Paryed = New DevExpress.XtraEditors.ComboBoxEdit()
    Me.Cbo_LO = New DevExpress.XtraEditors.ComboBoxEdit()
    Me.lbllohnabrechnung = New System.Windows.Forms.Label()
    Me.lblueberwiesen = New System.Windows.Forms.Label()
    Me.lblwaehrung = New System.Windows.Forms.Label()
    Me.lblzahlart = New System.Windows.Forms.Label()
    Me.lbljahr = New System.Windows.Forms.Label()
    Me.lblmonat = New System.Windows.Forms.Label()
    Me.lblverguetung = New System.Windows.Forms.Label()
    Me.lblSortierung = New System.Windows.Forms.Label()
    Me.txtVGNr_1 = New DevExpress.XtraEditors.TextEdit()
    Me.xtabZGSearch = New DevExpress.XtraTab.XtraTabControl()
    Me.xtabAllgemein = New DevExpress.XtraTab.XtraTabPage()
    Me.XtraScrollableControl1 = New DevExpress.XtraEditors.XtraScrollableControl()
    Me.SwitchButton2 = New DevComponents.DotNetBar.Controls.SwitchButton()
    Me.SwitchButton1 = New DevComponents.DotNetBar.Controls.SwitchButton()
    Me.lueMandant = New DevExpress.XtraEditors.LookUpEdit()
    Me.lblMDName = New System.Windows.Forms.Label()
    Me.txtZGNr_2 = New DevExpress.XtraEditors.ComboBoxEdit()
    Me.txtMANr_2 = New DevExpress.XtraEditors.ComboBoxEdit()
    Me.txtZGNr_1 = New DevExpress.XtraEditors.ComboBoxEdit()
    Me.txtMANr_1 = New DevExpress.XtraEditors.ComboBoxEdit()
    Me.xtabErweitert = New DevExpress.XtraTab.XtraTabPage()
    Me.xtabSQLAbfrage = New DevExpress.XtraTab.XtraTabPage()
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
    Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
    Me.LblTimeValue = New System.Windows.Forms.Label()
    Me.lblheader2 = New System.Windows.Forms.Label()
    Me.CmdClose = New DevExpress.XtraEditors.SimpleButton()
    Me.lblHeader1 = New System.Windows.Forms.Label()
    Me.LblSetting = New System.Windows.Forms.Label()
    CType(Me.txt_SQLQuery.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txt_IndSQLQuery.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.CboSort.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.Cbo_Berater.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.Cbo_Year.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.GroupBox2.SuspendLayout()
    CType(Me.dePrintedOn_2.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.dePrintedOn_2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.deOuton_1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.deOuton_1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.deOuton_2.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.deOuton_2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.dePrintedOn_1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.dePrintedOn_1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.Cbo_Filiale.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.Cbo_LANr.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.Cbo_Currency.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.Cbo_Month.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.Cbo_Paryed.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.Cbo_LO.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txtVGNr_1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.xtabZGSearch, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.xtabZGSearch.SuspendLayout()
    Me.xtabAllgemein.SuspendLayout()
    Me.XtraScrollableControl1.SuspendLayout()
    CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txtZGNr_2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txtMANr_2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txtZGNr_1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txtMANr_1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.xtabErweitert.SuspendLayout()
    Me.xtabSQLAbfrage.SuspendLayout()
    CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.PanelControl1.SuspendLayout()
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
    'lblderzeitigeabfrage
    '
    Me.lblderzeitigeabfrage.AutoSize = True
    Me.lblderzeitigeabfrage.Location = New System.Drawing.Point(28, 25)
    Me.lblderzeitigeabfrage.Name = "lblderzeitigeabfrage"
    Me.lblderzeitigeabfrage.Size = New System.Drawing.Size(154, 13)
    Me.lblderzeitigeabfrage.TabIndex = 73
    Me.lblderzeitigeabfrage.Text = "Ihre derzeitige Abfrage lautet:"
    '
    'txt_SQLQuery
    '
    Me.txt_SQLQuery.Location = New System.Drawing.Point(28, 44)
    Me.txt_SQLQuery.Name = "txt_SQLQuery"
    Me.txt_SQLQuery.Size = New System.Drawing.Size(590, 421)
    Me.txt_SQLQuery.TabIndex = 1
    '
    'lblAbfrage
    '
    Me.lblAbfrage.AutoSize = True
    Me.lblAbfrage.Location = New System.Drawing.Point(28, 25)
    Me.lblAbfrage.Name = "lblAbfrage"
    Me.lblAbfrage.Size = New System.Drawing.Size(69, 13)
    Me.lblAbfrage.TabIndex = 74
    Me.lblAbfrage.Text = "Ihre Abfrage"
    '
    'txt_IndSQLQuery
    '
    Me.txt_IndSQLQuery.Location = New System.Drawing.Point(28, 44)
    Me.txt_IndSQLQuery.Name = "txt_IndSQLQuery"
    Me.txt_IndSQLQuery.Size = New System.Drawing.Size(590, 421)
    Me.txt_IndSQLQuery.TabIndex = 2
    '
    'lblkandidat
    '
    Me.lblkandidat.Location = New System.Drawing.Point(14, 130)
    Me.lblkandidat.Name = "lblkandidat"
    Me.lblkandidat.Size = New System.Drawing.Size(136, 13)
    Me.lblkandidat.TabIndex = 81
    Me.lblkandidat.TabStop = True
    Me.lblkandidat.Tag = "txtESMANr_1"
    Me.lblkandidat.Text = "Kandidaten-Nr."
    Me.lblkandidat.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'lblNummer
    '
    Me.lblNummer.Location = New System.Drawing.Point(14, 104)
    Me.lblNummer.Name = "lblNummer"
    Me.lblNummer.Size = New System.Drawing.Size(136, 13)
    Me.lblNummer.TabIndex = 80
    Me.lblNummer.TabStop = True
    Me.lblNummer.Tag = "txtESESNr_1"
    Me.lblNummer.Text = "Nummer"
    Me.lblNummer.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'CboSort
    '
    Me.CboSort.Location = New System.Drawing.Point(156, 55)
    Me.CboSort.Name = "CboSort"
    Me.CboSort.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.CboSort.Size = New System.Drawing.Size(447, 20)
    Me.CboSort.TabIndex = 79
    '
    'Cbo_Berater
    '
    Me.Cbo_Berater.Location = New System.Drawing.Point(489, 248)
    Me.Cbo_Berater.Name = "Cbo_Berater"
    Me.Cbo_Berater.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.Cbo_Berater.Size = New System.Drawing.Size(114, 20)
    Me.Cbo_Berater.TabIndex = 10
    '
    'lblberater
    '
    Me.lblberater.Location = New System.Drawing.Point(341, 250)
    Me.lblberater.Name = "lblberater"
    Me.lblberater.Size = New System.Drawing.Size(142, 13)
    Me.lblberater.TabIndex = 78
    Me.lblberater.Text = "BeraterIn"
    Me.lblberater.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'Cbo_Year
    '
    Me.Cbo_Year.Location = New System.Drawing.Point(489, 195)
    Me.Cbo_Year.Name = "Cbo_Year"
    Me.Cbo_Year.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.Cbo_Year.Size = New System.Drawing.Size(114, 20)
    Me.Cbo_Year.TabIndex = 7
    '
    'GroupBox2
    '
    Me.GroupBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.GroupBox2.Controls.Add(Me.lblzahlungam)
    Me.GroupBox2.Controls.Add(Me.lbldruckam)
    Me.GroupBox2.Controls.Add(Me.dePrintedOn_2)
    Me.GroupBox2.Controls.Add(Me.deOuton_1)
    Me.GroupBox2.Controls.Add(Me.deOuton_2)
    Me.GroupBox2.Controls.Add(Me.dePrintedOn_1)
    Me.GroupBox2.Location = New System.Drawing.Point(17, 375)
    Me.GroupBox2.Name = "GroupBox2"
    Me.GroupBox2.Size = New System.Drawing.Size(618, 81)
    Me.GroupBox2.TabIndex = 75
    Me.GroupBox2.TabStop = False
    '
    'lblzahlungam
    '
    Me.lblzahlungam.Location = New System.Drawing.Point(6, 24)
    Me.lblzahlungam.Name = "lblzahlungam"
    Me.lblzahlungam.Size = New System.Drawing.Size(127, 13)
    Me.lblzahlungam.TabIndex = 17
    Me.lblzahlungam.Text = "Zahlung am"
    Me.lblzahlungam.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'lbldruckam
    '
    Me.lbldruckam.Location = New System.Drawing.Point(321, 24)
    Me.lbldruckam.Name = "lbldruckam"
    Me.lbldruckam.Size = New System.Drawing.Size(145, 13)
    Me.lbldruckam.TabIndex = 18
    Me.lbldruckam.Text = "Druck am"
    Me.lbldruckam.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'dePrintedOn_2
    '
    Me.dePrintedOn_2.EditValue = Nothing
    Me.dePrintedOn_2.Location = New System.Drawing.Point(472, 47)
    Me.dePrintedOn_2.Name = "dePrintedOn_2"
    Me.dePrintedOn_2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.dePrintedOn_2.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.dePrintedOn_2.Properties.CalendarTimeProperties.CloseUpKey = New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.F4)
    Me.dePrintedOn_2.Properties.CalendarTimeProperties.PopupBorderStyle = DevExpress.XtraEditors.Controls.PopupBorderStyles.[Default]
    Me.dePrintedOn_2.Size = New System.Drawing.Size(98, 20)
    Me.dePrintedOn_2.TabIndex = 148
    '
    'deOuton_1
    '
    Me.deOuton_1.EditValue = Nothing
    Me.deOuton_1.Location = New System.Drawing.Point(139, 21)
    Me.deOuton_1.Name = "deOuton_1"
    Me.deOuton_1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.deOuton_1.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.deOuton_1.Properties.CalendarTimeProperties.CloseUpKey = New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.F4)
    Me.deOuton_1.Properties.CalendarTimeProperties.PopupBorderStyle = DevExpress.XtraEditors.Controls.PopupBorderStyles.[Default]
    Me.deOuton_1.Size = New System.Drawing.Size(98, 20)
    Me.deOuton_1.TabIndex = 145
    '
    'deOuton_2
    '
    Me.deOuton_2.EditValue = Nothing
    Me.deOuton_2.Location = New System.Drawing.Point(139, 47)
    Me.deOuton_2.Name = "deOuton_2"
    Me.deOuton_2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.deOuton_2.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.deOuton_2.Properties.CalendarTimeProperties.CloseUpKey = New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.F4)
    Me.deOuton_2.Properties.CalendarTimeProperties.PopupBorderStyle = DevExpress.XtraEditors.Controls.PopupBorderStyles.[Default]
    Me.deOuton_2.Size = New System.Drawing.Size(98, 20)
    Me.deOuton_2.TabIndex = 146
    '
    'dePrintedOn_1
    '
    Me.dePrintedOn_1.EditValue = Nothing
    Me.dePrintedOn_1.Location = New System.Drawing.Point(472, 21)
    Me.dePrintedOn_1.Name = "dePrintedOn_1"
    Me.dePrintedOn_1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.dePrintedOn_1.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.dePrintedOn_1.Properties.CalendarTimeProperties.CloseUpKey = New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.F4)
    Me.dePrintedOn_1.Properties.CalendarTimeProperties.PopupBorderStyle = DevExpress.XtraEditors.Controls.PopupBorderStyles.[Default]
    Me.dePrintedOn_1.Size = New System.Drawing.Size(98, 20)
    Me.dePrintedOn_1.TabIndex = 147
    '
    'Cbo_Filiale
    '
    Me.Cbo_Filiale.Location = New System.Drawing.Point(489, 275)
    Me.Cbo_Filiale.Name = "Cbo_Filiale"
    Me.Cbo_Filiale.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.Cbo_Filiale.Size = New System.Drawing.Size(114, 20)
    Me.Cbo_Filiale.TabIndex = 11
    '
    'lblfiliale
    '
    Me.lblfiliale.Location = New System.Drawing.Point(341, 277)
    Me.lblfiliale.Name = "lblfiliale"
    Me.lblfiliale.Size = New System.Drawing.Size(142, 13)
    Me.lblfiliale.TabIndex = 57
    Me.lblfiliale.Text = "Filiale"
    Me.lblfiliale.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'Cbo_LANr
    '
    Me.Cbo_LANr.Location = New System.Drawing.Point(156, 247)
    Me.Cbo_LANr.Name = "Cbo_LANr"
    Me.Cbo_LANr.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.Cbo_LANr.Size = New System.Drawing.Size(114, 20)
    Me.Cbo_LANr.TabIndex = 8
    '
    'Cbo_Currency
    '
    Me.Cbo_Currency.Location = New System.Drawing.Point(156, 274)
    Me.Cbo_Currency.Name = "Cbo_Currency"
    Me.Cbo_Currency.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.Cbo_Currency.Size = New System.Drawing.Size(114, 20)
    Me.Cbo_Currency.TabIndex = 9
    '
    'Cbo_Month
    '
    Me.Cbo_Month.Location = New System.Drawing.Point(156, 194)
    Me.Cbo_Month.Name = "Cbo_Month"
    Me.Cbo_Month.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.Cbo_Month.Size = New System.Drawing.Size(114, 20)
    Me.Cbo_Month.TabIndex = 6
    '
    'Cbo_Paryed
    '
    Me.Cbo_Paryed.Location = New System.Drawing.Point(156, 311)
    Me.Cbo_Paryed.Name = "Cbo_Paryed"
    Me.Cbo_Paryed.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.Cbo_Paryed.Size = New System.Drawing.Size(114, 20)
    Me.Cbo_Paryed.TabIndex = 12
    '
    'Cbo_LO
    '
    Me.Cbo_LO.Location = New System.Drawing.Point(156, 338)
    Me.Cbo_LO.Name = "Cbo_LO"
    Me.Cbo_LO.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.Cbo_LO.Size = New System.Drawing.Size(114, 20)
    Me.Cbo_LO.TabIndex = 13
    '
    'lbllohnabrechnung
    '
    Me.lbllohnabrechnung.Location = New System.Drawing.Point(14, 342)
    Me.lbllohnabrechnung.Name = "lbllohnabrechnung"
    Me.lbllohnabrechnung.Size = New System.Drawing.Size(136, 13)
    Me.lbllohnabrechnung.TabIndex = 24
    Me.lbllohnabrechnung.Text = "Lohnabrechnung"
    Me.lbllohnabrechnung.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'lblueberwiesen
    '
    Me.lblueberwiesen.Location = New System.Drawing.Point(14, 314)
    Me.lblueberwiesen.Name = "lblueberwiesen"
    Me.lblueberwiesen.Size = New System.Drawing.Size(136, 13)
    Me.lblueberwiesen.TabIndex = 23
    Me.lblueberwiesen.Text = "Überweisung"
    Me.lblueberwiesen.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'lblwaehrung
    '
    Me.lblwaehrung.Location = New System.Drawing.Point(14, 278)
    Me.lblwaehrung.Name = "lblwaehrung"
    Me.lblwaehrung.Size = New System.Drawing.Size(136, 13)
    Me.lblwaehrung.TabIndex = 22
    Me.lblwaehrung.Text = "Währung"
    Me.lblwaehrung.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'lblzahlart
    '
    Me.lblzahlart.Location = New System.Drawing.Point(14, 251)
    Me.lblzahlart.Name = "lblzahlart"
    Me.lblzahlart.Size = New System.Drawing.Size(136, 13)
    Me.lblzahlart.TabIndex = 21
    Me.lblzahlart.Text = "Zahlart"
    Me.lblzahlart.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'lbljahr
    '
    Me.lbljahr.Location = New System.Drawing.Point(341, 197)
    Me.lbljahr.Name = "lbljahr"
    Me.lbljahr.Size = New System.Drawing.Size(142, 13)
    Me.lbljahr.TabIndex = 20
    Me.lbljahr.Text = "Jahr"
    Me.lbljahr.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'lblmonat
    '
    Me.lblmonat.Location = New System.Drawing.Point(14, 198)
    Me.lblmonat.Name = "lblmonat"
    Me.lblmonat.Size = New System.Drawing.Size(136, 13)
    Me.lblmonat.TabIndex = 19
    Me.lblmonat.Text = "Monat"
    Me.lblmonat.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'lblverguetung
    '
    Me.lblverguetung.Location = New System.Drawing.Point(14, 156)
    Me.lblverguetung.Name = "lblverguetung"
    Me.lblverguetung.Size = New System.Drawing.Size(136, 13)
    Me.lblverguetung.TabIndex = 16
    Me.lblverguetung.Text = "Vergütung-Nr."
    Me.lblverguetung.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'lblSortierung
    '
    Me.lblSortierung.Location = New System.Drawing.Point(14, 58)
    Me.lblSortierung.Name = "lblSortierung"
    Me.lblSortierung.Size = New System.Drawing.Size(136, 13)
    Me.lblSortierung.TabIndex = 13
    Me.lblSortierung.Text = "Sortieren nach"
    Me.lblSortierung.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'txtVGNr_1
    '
    Me.txtVGNr_1.Location = New System.Drawing.Point(156, 152)
    Me.txtVGNr_1.Name = "txtVGNr_1"
    Me.txtVGNr_1.Size = New System.Drawing.Size(158, 20)
    Me.txtVGNr_1.TabIndex = 5
    '
    'xtabZGSearch
    '
    Me.xtabZGSearch.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.xtabZGSearch.Location = New System.Drawing.Point(28, 105)
    Me.xtabZGSearch.Name = "xtabZGSearch"
    Me.xtabZGSearch.SelectedTabPage = Me.xtabAllgemein
    Me.xtabZGSearch.Size = New System.Drawing.Size(661, 515)
    Me.xtabZGSearch.TabIndex = 4
    Me.xtabZGSearch.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabAllgemein, Me.xtabErweitert, Me.xtabSQLAbfrage})
    '
    'xtabAllgemein
    '
    Me.xtabAllgemein.Controls.Add(Me.XtraScrollableControl1)
    Me.xtabAllgemein.Name = "xtabAllgemein"
    Me.xtabAllgemein.Padding = New System.Windows.Forms.Padding(5)
    Me.xtabAllgemein.Size = New System.Drawing.Size(655, 487)
    Me.xtabAllgemein.Text = "Allgemein"
    '
    'XtraScrollableControl1
    '
    Me.XtraScrollableControl1.Controls.Add(Me.SwitchButton2)
    Me.XtraScrollableControl1.Controls.Add(Me.SwitchButton1)
    Me.XtraScrollableControl1.Controls.Add(Me.lueMandant)
    Me.XtraScrollableControl1.Controls.Add(Me.lblMDName)
    Me.XtraScrollableControl1.Controls.Add(Me.lblSortierung)
    Me.XtraScrollableControl1.Controls.Add(Me.lblkandidat)
    Me.XtraScrollableControl1.Controls.Add(Me.lblNummer)
    Me.XtraScrollableControl1.Controls.Add(Me.CboSort)
    Me.XtraScrollableControl1.Controls.Add(Me.Cbo_Berater)
    Me.XtraScrollableControl1.Controls.Add(Me.txtVGNr_1)
    Me.XtraScrollableControl1.Controls.Add(Me.lblberater)
    Me.XtraScrollableControl1.Controls.Add(Me.lblverguetung)
    Me.XtraScrollableControl1.Controls.Add(Me.Cbo_Year)
    Me.XtraScrollableControl1.Controls.Add(Me.lblmonat)
    Me.XtraScrollableControl1.Controls.Add(Me.GroupBox2)
    Me.XtraScrollableControl1.Controls.Add(Me.lbljahr)
    Me.XtraScrollableControl1.Controls.Add(Me.Cbo_Filiale)
    Me.XtraScrollableControl1.Controls.Add(Me.lblzahlart)
    Me.XtraScrollableControl1.Controls.Add(Me.lblfiliale)
    Me.XtraScrollableControl1.Controls.Add(Me.lblwaehrung)
    Me.XtraScrollableControl1.Controls.Add(Me.Cbo_LANr)
    Me.XtraScrollableControl1.Controls.Add(Me.lblueberwiesen)
    Me.XtraScrollableControl1.Controls.Add(Me.Cbo_Currency)
    Me.XtraScrollableControl1.Controls.Add(Me.lbllohnabrechnung)
    Me.XtraScrollableControl1.Controls.Add(Me.Cbo_Month)
    Me.XtraScrollableControl1.Controls.Add(Me.Cbo_LO)
    Me.XtraScrollableControl1.Controls.Add(Me.Cbo_Paryed)
    Me.XtraScrollableControl1.Controls.Add(Me.txtZGNr_2)
    Me.XtraScrollableControl1.Controls.Add(Me.txtMANr_2)
    Me.XtraScrollableControl1.Controls.Add(Me.txtZGNr_1)
    Me.XtraScrollableControl1.Controls.Add(Me.txtMANr_1)
    Me.XtraScrollableControl1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.XtraScrollableControl1.Location = New System.Drawing.Point(5, 5)
    Me.XtraScrollableControl1.Name = "XtraScrollableControl1"
    Me.XtraScrollableControl1.Size = New System.Drawing.Size(645, 477)
    Me.XtraScrollableControl1.TabIndex = 256
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
    Me.SwitchButton2.Location = New System.Drawing.Point(401, 127)
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
    Me.SwitchButton2.TabIndex = 255
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
    Me.SwitchButton1.Location = New System.Drawing.Point(401, 101)
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
    Me.SwitchButton1.TabIndex = 254
    '
    'lueMandant
    '
    Me.lueMandant.Location = New System.Drawing.Point(156, 29)
    Me.lueMandant.Name = "lueMandant"
    Me.lueMandant.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.lueMandant.Properties.NullText = ""
    Me.lueMandant.Size = New System.Drawing.Size(447, 20)
    Me.lueMandant.TabIndex = 201
    '
    'lblMDName
    '
    Me.lblMDName.Location = New System.Drawing.Point(32, 33)
    Me.lblMDName.Name = "lblMDName"
    Me.lblMDName.Size = New System.Drawing.Size(118, 13)
    Me.lblMDName.TabIndex = 202
    Me.lblMDName.Text = "Mandanten"
    Me.lblMDName.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'txtZGNr_2
    '
    Me.txtZGNr_2.Location = New System.Drawing.Point(445, 100)
    Me.txtZGNr_2.Name = "txtZGNr_2"
    Me.txtZGNr_2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.txtZGNr_2.Size = New System.Drawing.Size(158, 20)
    Me.txtZGNr_2.TabIndex = 2
    Me.txtZGNr_2.Visible = False
    '
    'txtMANr_2
    '
    Me.txtMANr_2.Location = New System.Drawing.Point(445, 126)
    Me.txtMANr_2.Name = "txtMANr_2"
    Me.txtMANr_2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.txtMANr_2.Size = New System.Drawing.Size(158, 20)
    Me.txtMANr_2.TabIndex = 4
    Me.txtMANr_2.Visible = False
    '
    'txtZGNr_1
    '
    Me.txtZGNr_1.Location = New System.Drawing.Point(156, 100)
    Me.txtZGNr_1.Name = "txtZGNr_1"
    Me.txtZGNr_1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.txtZGNr_1.Size = New System.Drawing.Size(158, 20)
    Me.txtZGNr_1.TabIndex = 1
    '
    'txtMANr_1
    '
    Me.txtMANr_1.Location = New System.Drawing.Point(156, 126)
    Me.txtMANr_1.Name = "txtMANr_1"
    Me.txtMANr_1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.txtMANr_1.Size = New System.Drawing.Size(158, 20)
    Me.txtMANr_1.TabIndex = 3
    '
    'xtabErweitert
    '
    Me.xtabErweitert.Controls.Add(Me.lblAbfrage)
    Me.xtabErweitert.Controls.Add(Me.txt_IndSQLQuery)
    Me.xtabErweitert.Name = "xtabErweitert"
    Me.xtabErweitert.Size = New System.Drawing.Size(655, 487)
    Me.xtabErweitert.Text = "Erweiterte Abfrage"
    '
    'xtabSQLAbfrage
    '
    Me.xtabSQLAbfrage.Controls.Add(Me.lblderzeitigeabfrage)
    Me.xtabSQLAbfrage.Controls.Add(Me.txt_SQLQuery)
    Me.xtabSQLAbfrage.Name = "xtabSQLAbfrage"
    Me.xtabSQLAbfrage.Size = New System.Drawing.Size(655, 487)
    Me.xtabSQLAbfrage.Text = "SQL-Abfrage"
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
    Me.bsiInfo.Width = 300
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
    'barDockControlTop
    '
    Me.barDockControlTop.CausesValidation = False
    Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
    Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
    Me.barDockControlTop.Size = New System.Drawing.Size(722, 0)
    '
    'barDockControlBottom
    '
    Me.barDockControlBottom.CausesValidation = False
    Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
    Me.barDockControlBottom.Location = New System.Drawing.Point(0, 659)
    Me.barDockControlBottom.Size = New System.Drawing.Size(722, 27)
    '
    'barDockControlLeft
    '
    Me.barDockControlLeft.CausesValidation = False
    Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
    Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
    Me.barDockControlLeft.Size = New System.Drawing.Size(0, 659)
    '
    'barDockControlRight
    '
    Me.barDockControlRight.CausesValidation = False
    Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
    Me.barDockControlRight.Location = New System.Drawing.Point(722, 0)
    Me.barDockControlRight.Size = New System.Drawing.Size(0, 659)
    '
    'PanelControl1
    '
    Me.PanelControl1.Appearance.BackColor = System.Drawing.Color.White
    Me.PanelControl1.Appearance.Options.UseBackColor = True
    Me.PanelControl1.Controls.Add(Me.LblTimeValue)
    Me.PanelControl1.Controls.Add(Me.lblheader2)
    Me.PanelControl1.Controls.Add(Me.CmdClose)
    Me.PanelControl1.Controls.Add(Me.lblHeader1)
    Me.PanelControl1.Controls.Add(Me.LblSetting)
    Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Top
    Me.PanelControl1.Location = New System.Drawing.Point(0, 0)
    Me.PanelControl1.Name = "PanelControl1"
    Me.PanelControl1.Size = New System.Drawing.Size(722, 77)
    Me.PanelControl1.TabIndex = 9
    '
    'LblTimeValue
    '
    Me.LblTimeValue.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.LblTimeValue.BackColor = System.Drawing.Color.Transparent
    Me.LblTimeValue.Location = New System.Drawing.Point(388, 50)
    Me.LblTimeValue.Name = "LblTimeValue"
    Me.LblTimeValue.Size = New System.Drawing.Size(287, 13)
    Me.LblTimeValue.TabIndex = 14
    Me.LblTimeValue.Text = "Zeitangaben..."
    Me.LblTimeValue.TextAlign = System.Drawing.ContentAlignment.TopRight
    Me.LblTimeValue.Visible = False
    '
    'lblheader2
    '
    Me.lblheader2.AutoSize = True
    Me.lblheader2.BackColor = System.Drawing.Color.Transparent
    Me.lblheader2.Location = New System.Drawing.Point(97, 47)
    Me.lblheader2.Name = "lblheader2"
    Me.lblheader2.Size = New System.Drawing.Size(225, 13)
    Me.lblheader2.TabIndex = 1
    Me.lblheader2.Text = "Wählen Sie Ihre gewünschten Kriterien aus..."
    '
    'CmdClose
    '
    Me.CmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.CmdClose.Location = New System.Drawing.Point(595, 23)
    Me.CmdClose.Name = "CmdClose"
    Me.CmdClose.Size = New System.Drawing.Size(80, 25)
    Me.CmdClose.TabIndex = 5
    Me.CmdClose.Text = "Schliessen"
    '
    'lblHeader1
    '
    Me.lblHeader1.AutoSize = True
    Me.lblHeader1.BackColor = System.Drawing.Color.Transparent
    Me.lblHeader1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblHeader1.Location = New System.Drawing.Point(89, 23)
    Me.lblHeader1.Name = "lblHeader1"
    Me.lblHeader1.Size = New System.Drawing.Size(219, 13)
    Me.lblHeader1.TabIndex = 0
    Me.lblHeader1.Text = "Erweiterte Suche nach Auszahlungen"
    '
    'LblSetting
    '
    Me.LblSetting.BackColor = System.Drawing.Color.Transparent
    Me.LblSetting.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.LblSetting.ForeColor = System.Drawing.SystemColors.HotTrack
    Me.LblSetting.Image = CType(resources.GetObject("LblSetting.Image"), System.Drawing.Image)
    Me.LblSetting.Location = New System.Drawing.Point(12, 9)
    Me.LblSetting.Name = "LblSetting"
    Me.LblSetting.Size = New System.Drawing.Size(71, 63)
    Me.LblSetting.TabIndex = 4
    '
    'frmZGSearch
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(722, 686)
    Me.Controls.Add(Me.PanelControl1)
    Me.Controls.Add(Me.xtabZGSearch)
    Me.Controls.Add(Me.barDockControlLeft)
    Me.Controls.Add(Me.barDockControlRight)
    Me.Controls.Add(Me.barDockControlBottom)
    Me.Controls.Add(Me.barDockControlTop)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.MaximizeBox = False
    Me.Name = "frmZGSearch"
    Me.Text = "Erweiterte Suche nach Auszahlungen"
    CType(Me.txt_SQLQuery.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txt_IndSQLQuery.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.CboSort.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.Cbo_Berater.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.Cbo_Year.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    Me.GroupBox2.ResumeLayout(False)
    CType(Me.dePrintedOn_2.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.dePrintedOn_2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.deOuton_1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.deOuton_1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.deOuton_2.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.deOuton_2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.dePrintedOn_1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.dePrintedOn_1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.Cbo_Filiale.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.Cbo_LANr.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.Cbo_Currency.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.Cbo_Month.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.Cbo_Paryed.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.Cbo_LO.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txtVGNr_1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.xtabZGSearch, System.ComponentModel.ISupportInitialize).EndInit()
    Me.xtabZGSearch.ResumeLayout(False)
    Me.xtabAllgemein.ResumeLayout(False)
    Me.XtraScrollableControl1.ResumeLayout(False)
    CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txtZGNr_2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txtMANr_2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txtZGNr_1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txtMANr_1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    Me.xtabErweitert.ResumeLayout(False)
    Me.xtabErweitert.PerformLayout()
    Me.xtabSQLAbfrage.ResumeLayout(False)
    Me.xtabSQLAbfrage.PerformLayout()
    CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.PanelControl1.ResumeLayout(False)
    Me.PanelControl1.PerformLayout()
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
	Friend WithEvents lblderzeitigeabfrage As System.Windows.Forms.Label
	Friend WithEvents txt_SQLQuery As DevExpress.XtraEditors.MemoEdit
  Friend WithEvents lblAbfrage As System.Windows.Forms.Label
  Friend WithEvents txt_IndSQLQuery As DevExpress.XtraEditors.MemoEdit
  Friend WithEvents Cbo_Filiale As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents lblfiliale As System.Windows.Forms.Label
  Friend WithEvents Cbo_LANr As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents Cbo_Currency As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents Cbo_Month As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents Cbo_Paryed As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents Cbo_LO As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents lbllohnabrechnung As System.Windows.Forms.Label
  Friend WithEvents lblueberwiesen As System.Windows.Forms.Label
  Friend WithEvents lblwaehrung As System.Windows.Forms.Label
  Friend WithEvents lblzahlart As System.Windows.Forms.Label
  Friend WithEvents lbljahr As System.Windows.Forms.Label
  Friend WithEvents lblmonat As System.Windows.Forms.Label
  Friend WithEvents lbldruckam As System.Windows.Forms.Label
  Friend WithEvents lblzahlungam As System.Windows.Forms.Label
  Friend WithEvents lblverguetung As System.Windows.Forms.Label
  Friend WithEvents lblSortierung As System.Windows.Forms.Label
  Friend WithEvents txtVGNr_1 As DevExpress.XtraEditors.TextEdit
  Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
  Friend WithEvents Cbo_Year As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents Cbo_Berater As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents lblberater As System.Windows.Forms.Label
  Friend WithEvents CboSort As DevExpress.XtraEditors.CheckedComboBoxEdit
  Friend WithEvents lblkandidat As System.Windows.Forms.Label
  Friend WithEvents lblNummer As System.Windows.Forms.Label
  Friend WithEvents xtabZGSearch As DevExpress.XtraTab.XtraTabControl
  Friend WithEvents xtabAllgemein As DevExpress.XtraTab.XtraTabPage
  Friend WithEvents xtabErweitert As DevExpress.XtraTab.XtraTabPage
  Friend WithEvents xtabSQLAbfrage As DevExpress.XtraTab.XtraTabPage
  Friend WithEvents deOuton_2 As DevExpress.XtraEditors.DateEdit
  Friend WithEvents deOuton_1 As DevExpress.XtraEditors.DateEdit
  Friend WithEvents dePrintedOn_2 As DevExpress.XtraEditors.DateEdit
  Friend WithEvents dePrintedOn_1 As DevExpress.XtraEditors.DateEdit
  Friend WithEvents lueMandant As DevExpress.XtraEditors.LookUpEdit
  Friend WithEvents lblMDName As System.Windows.Forms.Label
  Friend WithEvents SwitchButton2 As DevComponents.DotNetBar.Controls.SwitchButton
  Friend WithEvents SwitchButton1 As DevComponents.DotNetBar.Controls.SwitchButton
  Friend WithEvents Bar3 As DevExpress.XtraBars.Bar
  Friend WithEvents bsiInfo As DevExpress.XtraBars.BarStaticItem
  Friend WithEvents bbiSearch As DevExpress.XtraBars.BarButtonItem
  Friend WithEvents bbiClear As DevExpress.XtraBars.BarButtonItem
  Friend WithEvents bbiPrint As DevExpress.XtraBars.BarLargeButtonItem
  Friend WithEvents bbiExport As DevExpress.XtraBars.BarLargeButtonItem
  Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
  Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
  Friend WithEvents txtZGNr_2 As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents txtMANr_2 As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents txtZGNr_1 As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents txtMANr_1 As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents XtraScrollableControl1 As DevExpress.XtraEditors.XtraScrollableControl
  Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
  Friend WithEvents LblTimeValue As System.Windows.Forms.Label
  Friend WithEvents lblheader2 As System.Windows.Forms.Label
  Friend WithEvents CmdClose As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents lblHeader1 As System.Windows.Forms.Label
  Friend WithEvents LblSetting As System.Windows.Forms.Label
  Friend WithEvents BarStaticItem1 As DevExpress.XtraBars.BarStaticItem

End Class
