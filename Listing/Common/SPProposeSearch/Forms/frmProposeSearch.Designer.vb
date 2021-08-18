<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmProposeSearch
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmProposeSearch))
    Me.Label1 = New System.Windows.Forms.Label()
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
    Me.Label72 = New System.Windows.Forms.Label()
    Me.txt_SQLQuery = New DevExpress.XtraEditors.MemoEdit()
    Me.Label10 = New System.Windows.Forms.Label()
    Me.txt_IndSQLQuery = New DevExpress.XtraEditors.MemoEdit()
    Me.p_1 = New System.Windows.Forms.Panel()
    Me.SwitchButton4 = New DevComponents.DotNetBar.Controls.SwitchButton()
    Me.SwitchButton3 = New DevComponents.DotNetBar.Controls.SwitchButton()
    Me.SwitchButton2 = New DevComponents.DotNetBar.Controls.SwitchButton()
    Me.SwitchButton1 = New DevComponents.DotNetBar.Controls.SwitchButton()
    Me.Lib_PNr_1 = New System.Windows.Forms.Label()
    Me.Lib_MANr_1 = New System.Windows.Forms.Label()
    Me.Lib_KDNr_1 = New System.Windows.Forms.Label()
    Me.Lib_VakNr_1 = New System.Windows.Forms.Label()
    Me.txt_PNr_1 = New DevExpress.XtraEditors.ComboBoxEdit()
    Me.txt_MANr_1 = New DevExpress.XtraEditors.ComboBoxEdit()
    Me.txt_KDNr_1 = New DevExpress.XtraEditors.ComboBoxEdit()
    Me.txt_VakNr_1 = New DevExpress.XtraEditors.ComboBoxEdit()
    Me.txt_PNr_2 = New DevExpress.XtraEditors.ComboBoxEdit()
    Me.txt_MANr_2 = New DevExpress.XtraEditors.ComboBoxEdit()
    Me.txt_KDNr_2 = New DevExpress.XtraEditors.ComboBoxEdit()
    Me.txt_VakNr_2 = New DevExpress.XtraEditors.ComboBoxEdit()
    Me.LblHeader_2 = New System.Windows.Forms.Label()
    Me.Label27 = New System.Windows.Forms.Label()
    Me.deErfasst_2 = New DevExpress.XtraEditors.DateEdit()
    Me.deErfasst_1 = New DevExpress.XtraEditors.DateEdit()
    Me.Cbo_Tarif = New DevExpress.XtraEditors.ComboBoxEdit()
    Me.Label5 = New System.Windows.Forms.Label()
    Me.Cbo_Lohn = New DevExpress.XtraEditors.ComboBoxEdit()
    Me.Label13 = New System.Windows.Forms.Label()
    Me.Cbo_Arbbegin = New DevExpress.XtraEditors.ComboBoxEdit()
    Me.Label4 = New System.Windows.Forms.Label()
    Me.p_2 = New System.Windows.Forms.Panel()
    Me.Cbo_Bez = New DevExpress.XtraEditors.ComboBoxEdit()
    Me.Label9 = New System.Windows.Forms.Label()
    Me.Cbo_KST = New DevExpress.XtraEditors.ComboBoxEdit()
    Me.Label34 = New System.Windows.Forms.Label()
    Me.Cbo_Filiale = New DevExpress.XtraEditors.ComboBoxEdit()
    Me.Label35 = New System.Windows.Forms.Label()
    Me.Cbo_State = New DevExpress.XtraEditors.ComboBoxEdit()
    Me.Label7 = New System.Windows.Forms.Label()
    Me.Cbo_Anstellung = New DevExpress.XtraEditors.ComboBoxEdit()
    Me.Label32 = New System.Windows.Forms.Label()
    Me.Label8 = New System.Windows.Forms.Label()
    Me.Cbo_Art = New DevExpress.XtraEditors.ComboBoxEdit()
    Me.LblHeader_1 = New System.Windows.Forms.Label()
    Me.Label3 = New System.Windows.Forms.Label()
    Me.xtabProposeSearch = New DevExpress.XtraTab.XtraTabControl()
    Me.xtabAllgemein = New DevExpress.XtraTab.XtraTabPage()
    Me.XtraTabControl1 = New DevExpress.XtraTab.XtraTabControl()
    Me.xtabErfasst = New DevExpress.XtraTab.XtraTabPage()
    Me.xtabBegin = New DevExpress.XtraTab.XtraTabPage()
    Me.xtabStaus = New DevExpress.XtraTab.XtraTabPage()
    Me.lueMandant = New DevExpress.XtraEditors.LookUpEdit()
    Me.lblMDName = New System.Windows.Forms.Label()
    Me.CboSort = New DevExpress.XtraEditors.CheckedComboBoxEdit()
    Me.xtabErweitert = New DevExpress.XtraTab.XtraTabPage()
    Me.xtabSQLAbfrage = New DevExpress.XtraTab.XtraTabPage()
    Me.BarManager1 = New DevExpress.XtraBars.BarManager()
    Me.Bar3 = New DevExpress.XtraBars.Bar()
    Me.bsiInfo = New DevExpress.XtraBars.BarStaticItem()
    Me.bbiSearch = New DevExpress.XtraBars.BarButtonItem()
    Me.bbiClear = New DevExpress.XtraBars.BarButtonItem()
    Me.bbiPrint = New DevExpress.XtraBars.BarLargeButtonItem()
    Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
    Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
    Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
    Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
    Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
    Me.PictureEdit1 = New DevExpress.XtraEditors.PictureEdit()
    Me.LblTimeValue = New DevExpress.XtraEditors.LabelControl()
    Me.Label2 = New DevExpress.XtraEditors.LabelControl()
    Me.CmdClose = New DevExpress.XtraEditors.SimpleButton()
    CType(Me.txt_SQLQuery.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txt_IndSQLQuery.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.p_1.SuspendLayout()
    CType(Me.txt_PNr_1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txt_MANr_1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txt_KDNr_1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txt_VakNr_1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txt_PNr_2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txt_MANr_2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txt_KDNr_2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txt_VakNr_2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.deErfasst_2.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.deErfasst_2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.deErfasst_1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.deErfasst_1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.Cbo_Tarif.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.Cbo_Lohn.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.Cbo_Arbbegin.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.p_2.SuspendLayout()
    CType(Me.Cbo_Bez.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.Cbo_KST.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.Cbo_Filiale.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.Cbo_State.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.Cbo_Anstellung.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.Cbo_Art.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.xtabProposeSearch, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.xtabProposeSearch.SuspendLayout()
    Me.xtabAllgemein.SuspendLayout()
    CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.XtraTabControl1.SuspendLayout()
    Me.xtabErfasst.SuspendLayout()
    Me.xtabBegin.SuspendLayout()
    Me.xtabStaus.SuspendLayout()
    CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.CboSort.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.xtabErweitert.SuspendLayout()
    Me.xtabSQLAbfrage.SuspendLayout()
    CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.PanelControl1.SuspendLayout()
    CType(Me.PictureEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'Label1
    '
    Me.Label1.AutoSize = True
    Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label1.Location = New System.Drawing.Point(84, 22)
    Me.Label1.Name = "Label1"
    Me.Label1.Size = New System.Drawing.Size(203, 13)
    Me.Label1.TabIndex = 0
    Me.Label1.Text = "Erweiterte Suche nach Vorschläge"
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
    'Label72
    '
    Me.Label72.AutoSize = True
    Me.Label72.Location = New System.Drawing.Point(23, 18)
    Me.Label72.Name = "Label72"
    Me.Label72.Size = New System.Drawing.Size(154, 13)
    Me.Label72.TabIndex = 73
    Me.Label72.Text = "Ihre derzeitige Abfrage lautet:"
    '
    'txt_SQLQuery
    '
    Me.txt_SQLQuery.Location = New System.Drawing.Point(26, 37)
    Me.txt_SQLQuery.Name = "txt_SQLQuery"
    Me.txt_SQLQuery.Size = New System.Drawing.Size(603, 432)
    Me.txt_SQLQuery.TabIndex = 1
    '
    'Label10
    '
    Me.Label10.AutoSize = True
    Me.Label10.Location = New System.Drawing.Point(23, 18)
    Me.Label10.Name = "Label10"
    Me.Label10.Size = New System.Drawing.Size(69, 13)
    Me.Label10.TabIndex = 74
    Me.Label10.Text = "Ihre Abfrage"
    '
    'txt_IndSQLQuery
    '
    Me.txt_IndSQLQuery.Location = New System.Drawing.Point(26, 37)
    Me.txt_IndSQLQuery.Name = "txt_IndSQLQuery"
    Me.txt_IndSQLQuery.Size = New System.Drawing.Size(603, 432)
    Me.txt_IndSQLQuery.TabIndex = 2
    '
    'p_1
    '
    Me.p_1.Controls.Add(Me.SwitchButton4)
    Me.p_1.Controls.Add(Me.SwitchButton3)
    Me.p_1.Controls.Add(Me.SwitchButton2)
    Me.p_1.Controls.Add(Me.SwitchButton1)
    Me.p_1.Controls.Add(Me.Lib_PNr_1)
    Me.p_1.Controls.Add(Me.Lib_MANr_1)
    Me.p_1.Controls.Add(Me.Lib_KDNr_1)
    Me.p_1.Controls.Add(Me.Lib_VakNr_1)
    Me.p_1.Controls.Add(Me.txt_PNr_1)
    Me.p_1.Controls.Add(Me.txt_MANr_1)
    Me.p_1.Controls.Add(Me.txt_KDNr_1)
    Me.p_1.Controls.Add(Me.txt_VakNr_1)
    Me.p_1.Controls.Add(Me.txt_PNr_2)
    Me.p_1.Controls.Add(Me.txt_MANr_2)
    Me.p_1.Controls.Add(Me.txt_KDNr_2)
    Me.p_1.Controls.Add(Me.txt_VakNr_2)
    Me.p_1.Location = New System.Drawing.Point(14, 115)
    Me.p_1.Name = "p_1"
    Me.p_1.Size = New System.Drawing.Size(620, 112)
    Me.p_1.TabIndex = 252
    '
    'SwitchButton4
    '
    Me.SwitchButton4.BackColor = System.Drawing.Color.White
    '
    '
    '
    Me.SwitchButton4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
    Me.SwitchButton4.FocusCuesEnabled = False
    Me.SwitchButton4.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.SwitchButton4.Location = New System.Drawing.Point(380, 82)
    Me.SwitchButton4.Name = "SwitchButton4"
    Me.SwitchButton4.OffBackColor = System.Drawing.Color.White
    Me.SwitchButton4.OffText = "O"
    Me.SwitchButton4.OffTextColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(53, Byte), Integer))
    Me.SwitchButton4.OnBackColor = System.Drawing.Color.LightSteelBlue
    Me.SwitchButton4.OnText = "|"
    Me.SwitchButton4.OnTextColor = System.Drawing.Color.Black
    Me.SwitchButton4.Size = New System.Drawing.Size(38, 19)
    Me.SwitchButton4.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
    Me.SwitchButton4.SwitchBackColor = System.Drawing.Color.DarkGray
    Me.SwitchButton4.SwitchBorderColor = System.Drawing.Color.DarkGray
    Me.SwitchButton4.SwitchFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.SwitchButton4.SwitchWidth = 6
    Me.SwitchButton4.TabIndex = 253
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
    Me.SwitchButton3.Location = New System.Drawing.Point(380, 56)
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
    Me.SwitchButton3.TabIndex = 252
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
    Me.SwitchButton2.Location = New System.Drawing.Point(380, 30)
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
    Me.SwitchButton2.TabIndex = 251
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
    Me.SwitchButton1.Location = New System.Drawing.Point(380, 4)
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
    Me.SwitchButton1.TabIndex = 250
    '
    'Lib_PNr_1
    '
    Me.Lib_PNr_1.Location = New System.Drawing.Point(10, 7)
    Me.Lib_PNr_1.Name = "Lib_PNr_1"
    Me.Lib_PNr_1.Size = New System.Drawing.Size(112, 13)
    Me.Lib_PNr_1.TabIndex = 240
    Me.Lib_PNr_1.TabStop = True
    Me.Lib_PNr_1.Tag = ""
    Me.Lib_PNr_1.Text = "Vorschlag-Nr."
    Me.Lib_PNr_1.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'Lib_MANr_1
    '
    Me.Lib_MANr_1.Location = New System.Drawing.Point(10, 33)
    Me.Lib_MANr_1.Name = "Lib_MANr_1"
    Me.Lib_MANr_1.Size = New System.Drawing.Size(112, 13)
    Me.Lib_MANr_1.TabIndex = 234
    Me.Lib_MANr_1.TabStop = True
    Me.Lib_MANr_1.Tag = ""
    Me.Lib_MANr_1.Text = "Kandidaten-Nr."
    Me.Lib_MANr_1.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'Lib_KDNr_1
    '
    Me.Lib_KDNr_1.Location = New System.Drawing.Point(10, 59)
    Me.Lib_KDNr_1.Name = "Lib_KDNr_1"
    Me.Lib_KDNr_1.Size = New System.Drawing.Size(112, 13)
    Me.Lib_KDNr_1.TabIndex = 0
    Me.Lib_KDNr_1.TabStop = True
    Me.Lib_KDNr_1.Tag = ""
    Me.Lib_KDNr_1.Text = "Kunden-Nr."
    Me.Lib_KDNr_1.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'Lib_VakNr_1
    '
    Me.Lib_VakNr_1.Location = New System.Drawing.Point(10, 85)
    Me.Lib_VakNr_1.Name = "Lib_VakNr_1"
    Me.Lib_VakNr_1.Size = New System.Drawing.Size(112, 13)
    Me.Lib_VakNr_1.TabIndex = 0
    Me.Lib_VakNr_1.TabStop = True
    Me.Lib_VakNr_1.Tag = ""
    Me.Lib_VakNr_1.Text = "Vakanz-Nr."
    Me.Lib_VakNr_1.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'txt_PNr_1
    '
    Me.txt_PNr_1.Location = New System.Drawing.Point(128, 3)
    Me.txt_PNr_1.Name = "txt_PNr_1"
    Me.txt_PNr_1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.txt_PNr_1.Size = New System.Drawing.Size(138, 20)
    Me.txt_PNr_1.TabIndex = 0
    Me.txt_PNr_1.Tag = ""
    '
    'txt_MANr_1
    '
    Me.txt_MANr_1.Location = New System.Drawing.Point(128, 29)
    Me.txt_MANr_1.Name = "txt_MANr_1"
    Me.txt_MANr_1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.txt_MANr_1.Size = New System.Drawing.Size(138, 20)
    Me.txt_MANr_1.TabIndex = 1
    Me.txt_MANr_1.Tag = "LibVakKDNr_1"
    '
    'txt_KDNr_1
    '
    Me.txt_KDNr_1.Location = New System.Drawing.Point(128, 55)
    Me.txt_KDNr_1.Name = "txt_KDNr_1"
    Me.txt_KDNr_1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.txt_KDNr_1.Size = New System.Drawing.Size(138, 20)
    Me.txt_KDNr_1.TabIndex = 2
    Me.txt_KDNr_1.Tag = "LibVakKDNr_1"
    '
    'txt_VakNr_1
    '
    Me.txt_VakNr_1.Location = New System.Drawing.Point(128, 81)
    Me.txt_VakNr_1.Name = "txt_VakNr_1"
    Me.txt_VakNr_1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.txt_VakNr_1.Size = New System.Drawing.Size(138, 20)
    Me.txt_VakNr_1.TabIndex = 3
    Me.txt_VakNr_1.Tag = "LibVakNr_1"
    '
    'txt_PNr_2
    '
    Me.txt_PNr_2.Location = New System.Drawing.Point(424, 3)
    Me.txt_PNr_2.Name = "txt_PNr_2"
    Me.txt_PNr_2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.txt_PNr_2.Size = New System.Drawing.Size(138, 20)
    Me.txt_PNr_2.TabIndex = 1
    Me.txt_PNr_2.Tag = "LibVakKDNr_2"
    Me.txt_PNr_2.Visible = False
    '
    'txt_MANr_2
    '
    Me.txt_MANr_2.Location = New System.Drawing.Point(424, 29)
    Me.txt_MANr_2.Name = "txt_MANr_2"
    Me.txt_MANr_2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.txt_MANr_2.Size = New System.Drawing.Size(138, 20)
    Me.txt_MANr_2.TabIndex = 3
    Me.txt_MANr_2.Tag = "LibVakKDNr_2"
    Me.txt_MANr_2.Visible = False
    '
    'txt_KDNr_2
    '
    Me.txt_KDNr_2.Location = New System.Drawing.Point(424, 55)
    Me.txt_KDNr_2.Name = "txt_KDNr_2"
    Me.txt_KDNr_2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.txt_KDNr_2.Size = New System.Drawing.Size(138, 20)
    Me.txt_KDNr_2.TabIndex = 5
    Me.txt_KDNr_2.Tag = "LibVakKDNr_2"
    Me.txt_KDNr_2.Visible = False
    '
    'txt_VakNr_2
    '
    Me.txt_VakNr_2.Location = New System.Drawing.Point(424, 81)
    Me.txt_VakNr_2.Name = "txt_VakNr_2"
    Me.txt_VakNr_2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.txt_VakNr_2.Size = New System.Drawing.Size(138, 20)
    Me.txt_VakNr_2.TabIndex = 7
    Me.txt_VakNr_2.Tag = "LibVakNr_2"
    Me.txt_VakNr_2.Visible = False
    '
    'LblHeader_2
    '
    Me.LblHeader_2.BackColor = System.Drawing.Color.Transparent
    Me.LblHeader_2.Cursor = System.Windows.Forms.Cursors.Hand
    Me.LblHeader_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.LblHeader_2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
    Me.LblHeader_2.Image = CType(resources.GetObject("LblHeader_2.Image"), System.Drawing.Image)
    Me.LblHeader_2.ImageAlign = System.Drawing.ContentAlignment.TopRight
    Me.LblHeader_2.Location = New System.Drawing.Point(40, 249)
    Me.LblHeader_2.Name = "LblHeader_2"
    Me.LblHeader_2.Size = New System.Drawing.Size(565, 14)
    Me.LblHeader_2.TabIndex = 261
    Me.LblHeader_2.Text = "Bezeichnung und BeraterIn"
    '
    'Label27
    '
    Me.Label27.Location = New System.Drawing.Point(51, 28)
    Me.Label27.Name = "Label27"
    Me.Label27.Size = New System.Drawing.Size(109, 13)
    Me.Label27.TabIndex = 0
    Me.Label27.Text = "Erfasst"
    Me.Label27.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'deErfasst_2
    '
    Me.deErfasst_2.EditValue = Nothing
    Me.deErfasst_2.Location = New System.Drawing.Point(166, 50)
    Me.deErfasst_2.Name = "deErfasst_2"
    Me.deErfasst_2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.deErfasst_2.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.deErfasst_2.Properties.CalendarTimeProperties.CloseUpKey = New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.F4)
    Me.deErfasst_2.Properties.CalendarTimeProperties.PopupBorderStyle = DevExpress.XtraEditors.Controls.PopupBorderStyles.[Default]
    Me.deErfasst_2.Size = New System.Drawing.Size(98, 20)
    Me.deErfasst_2.TabIndex = 7
    '
    'deErfasst_1
    '
    Me.deErfasst_1.EditValue = Nothing
    Me.deErfasst_1.Location = New System.Drawing.Point(166, 24)
    Me.deErfasst_1.Name = "deErfasst_1"
    Me.deErfasst_1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.deErfasst_1.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.deErfasst_1.Properties.CalendarTimeProperties.CloseUpKey = New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.F4)
    Me.deErfasst_1.Properties.CalendarTimeProperties.PopupBorderStyle = DevExpress.XtraEditors.Controls.PopupBorderStyles.[Default]
    Me.deErfasst_1.Size = New System.Drawing.Size(98, 20)
    Me.deErfasst_1.TabIndex = 6
    '
    'Cbo_Tarif
    '
    Me.Cbo_Tarif.Location = New System.Drawing.Point(166, 80)
    Me.Cbo_Tarif.Name = "Cbo_Tarif"
    Me.Cbo_Tarif.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.Cbo_Tarif.Size = New System.Drawing.Size(123, 20)
    Me.Cbo_Tarif.TabIndex = 2
    '
    'Label5
    '
    Me.Label5.Location = New System.Drawing.Point(3, 85)
    Me.Label5.Name = "Label5"
    Me.Label5.Size = New System.Drawing.Size(157, 13)
    Me.Label5.TabIndex = 233
    Me.Label5.Text = "Tarif"
    Me.Label5.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'Cbo_Lohn
    '
    Me.Cbo_Lohn.Location = New System.Drawing.Point(166, 52)
    Me.Cbo_Lohn.Name = "Cbo_Lohn"
    Me.Cbo_Lohn.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.Cbo_Lohn.Size = New System.Drawing.Size(123, 20)
    Me.Cbo_Lohn.TabIndex = 1
    '
    'Label13
    '
    Me.Label13.Location = New System.Drawing.Point(3, 57)
    Me.Label13.Name = "Label13"
    Me.Label13.Size = New System.Drawing.Size(157, 13)
    Me.Label13.TabIndex = 155
    Me.Label13.Text = "Lohn"
    Me.Label13.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'Cbo_Arbbegin
    '
    Me.Cbo_Arbbegin.Location = New System.Drawing.Point(166, 24)
    Me.Cbo_Arbbegin.Name = "Cbo_Arbbegin"
    Me.Cbo_Arbbegin.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.Cbo_Arbbegin.Size = New System.Drawing.Size(123, 20)
    Me.Cbo_Arbbegin.TabIndex = 0
    '
    'Label4
    '
    Me.Label4.Location = New System.Drawing.Point(3, 29)
    Me.Label4.Name = "Label4"
    Me.Label4.Size = New System.Drawing.Size(157, 13)
    Me.Label4.TabIndex = 142
    Me.Label4.Text = "Arbeitsbeginn"
    Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'p_2
    '
    Me.p_2.Controls.Add(Me.Cbo_Bez)
    Me.p_2.Controls.Add(Me.Label9)
    Me.p_2.Controls.Add(Me.Cbo_KST)
    Me.p_2.Controls.Add(Me.Label34)
    Me.p_2.Controls.Add(Me.Cbo_Filiale)
    Me.p_2.Controls.Add(Me.Label35)
    Me.p_2.Location = New System.Drawing.Point(14, 266)
    Me.p_2.Name = "p_2"
    Me.p_2.Size = New System.Drawing.Size(620, 65)
    Me.p_2.TabIndex = 258
    '
    'Cbo_Bez
    '
    Me.Cbo_Bez.Location = New System.Drawing.Point(128, 5)
    Me.Cbo_Bez.Name = "Cbo_Bez"
    Me.Cbo_Bez.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.Cbo_Bez.Size = New System.Drawing.Size(432, 20)
    Me.Cbo_Bez.TabIndex = 0
    '
    'Label9
    '
    Me.Label9.Location = New System.Drawing.Point(6, 9)
    Me.Label9.Name = "Label9"
    Me.Label9.Size = New System.Drawing.Size(118, 13)
    Me.Label9.TabIndex = 245
    Me.Label9.Text = "Bezeichnung"
    Me.Label9.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'Cbo_KST
    '
    Me.Cbo_KST.Location = New System.Drawing.Point(422, 31)
    Me.Cbo_KST.Name = "Cbo_KST"
    Me.Cbo_KST.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.Cbo_KST.Size = New System.Drawing.Size(138, 20)
    Me.Cbo_KST.TabIndex = 2
    '
    'Label34
    '
    Me.Label34.Location = New System.Drawing.Point(272, 34)
    Me.Label34.Name = "Label34"
    Me.Label34.Size = New System.Drawing.Size(146, 13)
    Me.Label34.TabIndex = 173
    Me.Label34.Text = "BeraterIn"
    Me.Label34.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'Cbo_Filiale
    '
    Me.Cbo_Filiale.Location = New System.Drawing.Point(128, 31)
    Me.Cbo_Filiale.Name = "Cbo_Filiale"
    Me.Cbo_Filiale.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.Cbo_Filiale.Size = New System.Drawing.Size(138, 20)
    Me.Cbo_Filiale.TabIndex = 1
    '
    'Label35
    '
    Me.Label35.Location = New System.Drawing.Point(6, 34)
    Me.Label35.Name = "Label35"
    Me.Label35.Size = New System.Drawing.Size(118, 13)
    Me.Label35.TabIndex = 171
    Me.Label35.Text = "Geschäftsstelle"
    Me.Label35.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'Cbo_State
    '
    Me.Cbo_State.Location = New System.Drawing.Point(166, 24)
    Me.Cbo_State.Name = "Cbo_State"
    Me.Cbo_State.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.Cbo_State.Size = New System.Drawing.Size(138, 20)
    Me.Cbo_State.TabIndex = 10
    '
    'Label7
    '
    Me.Label7.Location = New System.Drawing.Point(3, 85)
    Me.Label7.Name = "Label7"
    Me.Label7.Size = New System.Drawing.Size(157, 13)
    Me.Label7.TabIndex = 146
    Me.Label7.Text = "Anstellungsart"
    Me.Label7.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'Cbo_Anstellung
    '
    Me.Cbo_Anstellung.Location = New System.Drawing.Point(166, 80)
    Me.Cbo_Anstellung.Name = "Cbo_Anstellung"
    Me.Cbo_Anstellung.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.Cbo_Anstellung.Size = New System.Drawing.Size(138, 20)
    Me.Cbo_Anstellung.TabIndex = 12
    '
    'Label32
    '
    Me.Label32.Location = New System.Drawing.Point(3, 29)
    Me.Label32.Name = "Label32"
    Me.Label32.Size = New System.Drawing.Size(157, 13)
    Me.Label32.TabIndex = 169
    Me.Label32.Text = "Status"
    Me.Label32.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'Label8
    '
    Me.Label8.Location = New System.Drawing.Point(3, 57)
    Me.Label8.Name = "Label8"
    Me.Label8.Size = New System.Drawing.Size(157, 13)
    Me.Label8.TabIndex = 239
    Me.Label8.Text = "Vorschlagsart"
    Me.Label8.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'Cbo_Art
    '
    Me.Cbo_Art.Location = New System.Drawing.Point(166, 52)
    Me.Cbo_Art.Name = "Cbo_Art"
    Me.Cbo_Art.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.Cbo_Art.Size = New System.Drawing.Size(138, 20)
    Me.Cbo_Art.TabIndex = 11
    '
    'LblHeader_1
    '
    Me.LblHeader_1.BackColor = System.Drawing.Color.Transparent
    Me.LblHeader_1.Cursor = System.Windows.Forms.Cursors.Hand
    Me.LblHeader_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.LblHeader_1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
    Me.LblHeader_1.Image = CType(resources.GetObject("LblHeader_1.Image"), System.Drawing.Image)
    Me.LblHeader_1.ImageAlign = System.Drawing.ContentAlignment.TopRight
    Me.LblHeader_1.Location = New System.Drawing.Point(40, 97)
    Me.LblHeader_1.Name = "LblHeader_1"
    Me.LblHeader_1.Size = New System.Drawing.Size(565, 14)
    Me.LblHeader_1.TabIndex = 254
    Me.LblHeader_1.Text = "Nummerfelder"
    '
    'Label3
    '
    Me.Label3.Location = New System.Drawing.Point(61, 65)
    Me.Label3.Name = "Label3"
    Me.Label3.Size = New System.Drawing.Size(77, 13)
    Me.Label3.TabIndex = 0
    Me.Label3.Text = "Sortieren nach"
    Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'xtabProposeSearch
    '
    Me.xtabProposeSearch.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.xtabProposeSearch.Location = New System.Drawing.Point(28, 105)
    Me.xtabProposeSearch.Name = "xtabProposeSearch"
    Me.xtabProposeSearch.SelectedTabPage = Me.xtabAllgemein
    Me.xtabProposeSearch.Size = New System.Drawing.Size(656, 546)
    Me.xtabProposeSearch.TabIndex = 4
    Me.xtabProposeSearch.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabAllgemein, Me.xtabErweitert, Me.xtabSQLAbfrage})
    '
    'xtabAllgemein
    '
    Me.xtabAllgemein.Controls.Add(Me.XtraTabControl1)
    Me.xtabAllgemein.Controls.Add(Me.lueMandant)
    Me.xtabAllgemein.Controls.Add(Me.lblMDName)
    Me.xtabAllgemein.Controls.Add(Me.p_1)
    Me.xtabAllgemein.Controls.Add(Me.Label3)
    Me.xtabAllgemein.Controls.Add(Me.LblHeader_1)
    Me.xtabAllgemein.Controls.Add(Me.LblHeader_2)
    Me.xtabAllgemein.Controls.Add(Me.p_2)
    Me.xtabAllgemein.Controls.Add(Me.CboSort)
    Me.xtabAllgemein.Name = "xtabAllgemein"
    Me.xtabAllgemein.Size = New System.Drawing.Size(650, 518)
    Me.xtabAllgemein.Text = "Allgemein"
    '
    'XtraTabControl1
    '
    Me.XtraTabControl1.Location = New System.Drawing.Point(14, 353)
    Me.XtraTabControl1.Name = "XtraTabControl1"
    Me.XtraTabControl1.SelectedTabPage = Me.xtabErfasst
    Me.XtraTabControl1.Size = New System.Drawing.Size(620, 146)
    Me.XtraTabControl1.TabIndex = 1
    Me.XtraTabControl1.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabBegin, Me.xtabStaus, Me.xtabErfasst})
    '
    'xtabErfasst
    '
    Me.xtabErfasst.Controls.Add(Me.Label27)
    Me.xtabErfasst.Controls.Add(Me.deErfasst_2)
    Me.xtabErfasst.Controls.Add(Me.deErfasst_1)
    Me.xtabErfasst.Name = "xtabErfasst"
    Me.xtabErfasst.Size = New System.Drawing.Size(614, 118)
    Me.xtabErfasst.Text = "Erfasst am"
    '
    'xtabBegin
    '
    Me.xtabBegin.Controls.Add(Me.Cbo_Tarif)
    Me.xtabBegin.Controls.Add(Me.Label5)
    Me.xtabBegin.Controls.Add(Me.Cbo_Arbbegin)
    Me.xtabBegin.Controls.Add(Me.Cbo_Lohn)
    Me.xtabBegin.Controls.Add(Me.Label4)
    Me.xtabBegin.Controls.Add(Me.Label13)
    Me.xtabBegin.Name = "xtabBegin"
    Me.xtabBegin.Size = New System.Drawing.Size(614, 118)
    Me.xtabBegin.Text = "Beginn und Lohn"
    '
    'xtabStaus
    '
    Me.xtabStaus.Controls.Add(Me.Cbo_State)
    Me.xtabStaus.Controls.Add(Me.Label7)
    Me.xtabStaus.Controls.Add(Me.Cbo_Anstellung)
    Me.xtabStaus.Controls.Add(Me.Cbo_Art)
    Me.xtabStaus.Controls.Add(Me.Label32)
    Me.xtabStaus.Controls.Add(Me.Label8)
    Me.xtabStaus.Name = "xtabStaus"
    Me.xtabStaus.Size = New System.Drawing.Size(614, 118)
    Me.xtabStaus.Text = "Status und Art"
    '
    'lueMandant
    '
    Me.lueMandant.Location = New System.Drawing.Point(142, 34)
    Me.lueMandant.Name = "lueMandant"
    Me.lueMandant.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.lueMandant.Properties.NullText = ""
    Me.lueMandant.Size = New System.Drawing.Size(432, 20)
    Me.lueMandant.TabIndex = 266
    '
    'lblMDName
    '
    Me.lblMDName.Location = New System.Drawing.Point(21, 38)
    Me.lblMDName.Name = "lblMDName"
    Me.lblMDName.Size = New System.Drawing.Size(117, 13)
    Me.lblMDName.TabIndex = 265
    Me.lblMDName.Text = "Mandanten"
    Me.lblMDName.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'CboSort
    '
    Me.CboSort.Location = New System.Drawing.Point(142, 60)
    Me.CboSort.Name = "CboSort"
    Me.CboSort.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.CboSort.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
    Me.CboSort.Size = New System.Drawing.Size(432, 20)
    Me.CboSort.TabIndex = 0
    '
    'xtabErweitert
    '
    Me.xtabErweitert.Controls.Add(Me.Label10)
    Me.xtabErweitert.Controls.Add(Me.txt_IndSQLQuery)
    Me.xtabErweitert.Name = "xtabErweitert"
    Me.xtabErweitert.Size = New System.Drawing.Size(650, 518)
    Me.xtabErweitert.Text = "Erweiterte Abfrage"
    '
    'xtabSQLAbfrage
    '
    Me.xtabSQLAbfrage.Controls.Add(Me.Label72)
    Me.xtabSQLAbfrage.Controls.Add(Me.txt_SQLQuery)
    Me.xtabSQLAbfrage.Name = "xtabSQLAbfrage"
    Me.xtabSQLAbfrage.Size = New System.Drawing.Size(650, 518)
    Me.xtabSQLAbfrage.Text = "SQL-Abfrage"
    '
    'BarManager1
    '
    Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar3})
    Me.BarManager1.DockControls.Add(Me.barDockControlTop)
    Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
    Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
    Me.BarManager1.DockControls.Add(Me.barDockControlRight)
    Me.BarManager1.Form = Me
    Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiInfo, Me.bbiSearch, Me.bbiClear, Me.bbiPrint})
    Me.BarManager1.MaxItemId = 9
    Me.BarManager1.StatusBar = Me.Bar3
    '
    'Bar3
    '
    Me.Bar3.BarName = "Statusleiste"
    Me.Bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
    Me.Bar3.DockCol = 0
    Me.Bar3.DockRow = 0
    Me.Bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
    Me.Bar3.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiInfo), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiSearch), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiClear), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiPrint, True)})
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
    Me.bbiSearch.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText
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
    'barDockControlTop
    '
    Me.barDockControlTop.CausesValidation = False
    Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
    Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
    Me.barDockControlTop.Size = New System.Drawing.Size(713, 0)
    '
    'barDockControlBottom
    '
    Me.barDockControlBottom.CausesValidation = False
    Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
    Me.barDockControlBottom.Location = New System.Drawing.Point(0, 688)
    Me.barDockControlBottom.Size = New System.Drawing.Size(713, 27)
    '
    'barDockControlLeft
    '
    Me.barDockControlLeft.CausesValidation = False
    Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
    Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
    Me.barDockControlLeft.Size = New System.Drawing.Size(0, 688)
    '
    'barDockControlRight
    '
    Me.barDockControlRight.CausesValidation = False
    Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
    Me.barDockControlRight.Location = New System.Drawing.Point(713, 0)
    Me.barDockControlRight.Size = New System.Drawing.Size(0, 688)
    '
    'PanelControl1
    '
    Me.PanelControl1.Controls.Add(Me.PictureEdit1)
    Me.PanelControl1.Controls.Add(Me.LblTimeValue)
    Me.PanelControl1.Controls.Add(Me.Label2)
    Me.PanelControl1.Controls.Add(Me.CmdClose)
    Me.PanelControl1.Controls.Add(Me.Label1)
    Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Top
    Me.PanelControl1.Location = New System.Drawing.Point(0, 0)
    Me.PanelControl1.Name = "PanelControl1"
    Me.PanelControl1.Size = New System.Drawing.Size(713, 79)
    Me.PanelControl1.TabIndex = 234
    '
    'PictureEdit1
    '
    Me.PictureEdit1.EditValue = CType(resources.GetObject("PictureEdit1.EditValue"), Object)
    Me.PictureEdit1.Location = New System.Drawing.Point(12, 12)
    Me.PictureEdit1.Name = "PictureEdit1"
    Me.PictureEdit1.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
    Me.PictureEdit1.Properties.Appearance.Options.UseBackColor = True
    Me.PictureEdit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
    Me.PictureEdit1.Properties.PictureAlignment = System.Drawing.ContentAlignment.TopLeft
    Me.PictureEdit1.Size = New System.Drawing.Size(66, 59)
    Me.PictureEdit1.TabIndex = 233
    '
    'LblTimeValue
    '
    Me.LblTimeValue.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.LblTimeValue.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    Me.LblTimeValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
    Me.LblTimeValue.Location = New System.Drawing.Point(472, 52)
    Me.LblTimeValue.Name = "LblTimeValue"
    Me.LblTimeValue.Size = New System.Drawing.Size(201, 13)
    Me.LblTimeValue.TabIndex = 14
    Me.LblTimeValue.Text = "Zeitangaben..."
    Me.LblTimeValue.Visible = False
    '
    'Label2
    '
    Me.Label2.Location = New System.Drawing.Point(110, 44)
    Me.Label2.Name = "Label2"
    Me.Label2.Size = New System.Drawing.Size(218, 13)
    Me.Label2.TabIndex = 1
    Me.Label2.Text = "Wählen Sie Ihre gewünschten Kriterien aus..."
    '
    'CmdClose
    '
    Me.CmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.CmdClose.Location = New System.Drawing.Point(587, 22)
    Me.CmdClose.Name = "CmdClose"
    Me.CmdClose.Size = New System.Drawing.Size(86, 24)
    Me.CmdClose.TabIndex = 999
    Me.CmdClose.Text = "Schliessen"
    '
    'frmProposeSearch
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(713, 715)
    Me.Controls.Add(Me.PanelControl1)
    Me.Controls.Add(Me.xtabProposeSearch)
    Me.Controls.Add(Me.barDockControlLeft)
    Me.Controls.Add(Me.barDockControlRight)
    Me.Controls.Add(Me.barDockControlBottom)
    Me.Controls.Add(Me.barDockControlTop)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.MaximizeBox = False
    Me.MinimumSize = New System.Drawing.Size(729, 753)
    Me.Name = "frmProposeSearch"
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
    Me.Text = "Erweiterte Suche nach Vorschläge"
    CType(Me.txt_SQLQuery.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txt_IndSQLQuery.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    Me.p_1.ResumeLayout(False)
    CType(Me.txt_PNr_1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txt_MANr_1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txt_KDNr_1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txt_VakNr_1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txt_PNr_2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txt_MANr_2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txt_KDNr_2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txt_VakNr_2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.deErfasst_2.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.deErfasst_2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.deErfasst_1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.deErfasst_1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.Cbo_Tarif.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.Cbo_Lohn.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.Cbo_Arbbegin.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    Me.p_2.ResumeLayout(False)
    CType(Me.Cbo_Bez.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.Cbo_KST.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.Cbo_Filiale.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.Cbo_State.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.Cbo_Anstellung.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.Cbo_Art.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.xtabProposeSearch, System.ComponentModel.ISupportInitialize).EndInit()
    Me.xtabProposeSearch.ResumeLayout(False)
    Me.xtabAllgemein.ResumeLayout(False)
    CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.XtraTabControl1.ResumeLayout(False)
    Me.xtabErfasst.ResumeLayout(False)
    Me.xtabBegin.ResumeLayout(False)
    Me.xtabStaus.ResumeLayout(False)
    CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.CboSort.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    Me.xtabErweitert.ResumeLayout(False)
    Me.xtabErweitert.PerformLayout()
    Me.xtabSQLAbfrage.ResumeLayout(False)
    Me.xtabSQLAbfrage.PerformLayout()
    CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.PanelControl1.ResumeLayout(False)
    Me.PanelControl1.PerformLayout()
    CType(Me.PictureEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
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
	Friend WithEvents Label72 As System.Windows.Forms.Label
	Friend WithEvents txt_SQLQuery As DevExpress.XtraEditors.MemoEdit
  Friend WithEvents Label10 As System.Windows.Forms.Label
  Friend WithEvents txt_IndSQLQuery As DevExpress.XtraEditors.MemoEdit
  Friend WithEvents Lib_KDNr_1 As System.Windows.Forms.Label
  Friend WithEvents Lib_VakNr_1 As System.Windows.Forms.Label
  Friend WithEvents Label27 As System.Windows.Forms.Label
  Friend WithEvents Label3 As System.Windows.Forms.Label
  Friend WithEvents Cbo_Arbbegin As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents Label4 As System.Windows.Forms.Label
  Friend WithEvents Cbo_Anstellung As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents Label7 As System.Windows.Forms.Label
  Friend WithEvents Label13 As System.Windows.Forms.Label
  Friend WithEvents Cbo_Lohn As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents Cbo_KST As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents Label34 As System.Windows.Forms.Label
  Friend WithEvents Cbo_Filiale As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents Label35 As System.Windows.Forms.Label
  Friend WithEvents Cbo_State As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents Label32 As System.Windows.Forms.Label
  Friend WithEvents Lib_MANr_1 As System.Windows.Forms.Label
  Friend WithEvents Cbo_Tarif As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents Label5 As System.Windows.Forms.Label
  Friend WithEvents Cbo_Art As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents Label8 As System.Windows.Forms.Label
  Friend WithEvents Lib_PNr_1 As System.Windows.Forms.Label
  Friend WithEvents Cbo_Bez As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents Label9 As System.Windows.Forms.Label
  Friend WithEvents p_1 As System.Windows.Forms.Panel
  Friend WithEvents LblHeader_1 As System.Windows.Forms.Label
  Friend WithEvents p_2 As System.Windows.Forms.Panel
  Friend WithEvents LblHeader_2 As System.Windows.Forms.Label
  Friend WithEvents xtabProposeSearch As DevExpress.XtraTab.XtraTabControl
  Friend WithEvents xtabAllgemein As DevExpress.XtraTab.XtraTabPage
  Friend WithEvents xtabErweitert As DevExpress.XtraTab.XtraTabPage
  Friend WithEvents xtabSQLAbfrage As DevExpress.XtraTab.XtraTabPage
  Friend WithEvents deErfasst_2 As DevExpress.XtraEditors.DateEdit
  Friend WithEvents deErfasst_1 As DevExpress.XtraEditors.DateEdit
  Friend WithEvents lueMandant As DevExpress.XtraEditors.LookUpEdit
  Friend WithEvents lblMDName As System.Windows.Forms.Label
  Friend WithEvents XtraTabControl1 As DevExpress.XtraTab.XtraTabControl
  Friend WithEvents xtabErfasst As DevExpress.XtraTab.XtraTabPage
  Friend WithEvents xtabBegin As DevExpress.XtraTab.XtraTabPage
  Friend WithEvents xtabStaus As DevExpress.XtraTab.XtraTabPage
  Friend WithEvents SwitchButton4 As DevComponents.DotNetBar.Controls.SwitchButton
  Friend WithEvents SwitchButton3 As DevComponents.DotNetBar.Controls.SwitchButton
  Friend WithEvents SwitchButton2 As DevComponents.DotNetBar.Controls.SwitchButton
  Friend WithEvents SwitchButton1 As DevComponents.DotNetBar.Controls.SwitchButton
  Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
  Friend WithEvents Bar3 As DevExpress.XtraBars.Bar
  Friend WithEvents bsiInfo As DevExpress.XtraBars.BarStaticItem
  Friend WithEvents bbiSearch As DevExpress.XtraBars.BarButtonItem
  Friend WithEvents bbiClear As DevExpress.XtraBars.BarButtonItem
  Friend WithEvents bbiPrint As DevExpress.XtraBars.BarLargeButtonItem
  Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
  Friend WithEvents txt_PNr_1 As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents txt_MANr_1 As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents txt_KDNr_1 As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents txt_VakNr_1 As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents txt_PNr_2 As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents txt_MANr_2 As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents txt_KDNr_2 As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents txt_VakNr_2 As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents CboSort As DevExpress.XtraEditors.CheckedComboBoxEdit
  Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
  Friend WithEvents PictureEdit1 As DevExpress.XtraEditors.PictureEdit
  Friend WithEvents LblTimeValue As DevExpress.XtraEditors.LabelControl
  Friend WithEvents Label2 As DevExpress.XtraEditors.LabelControl
  Friend WithEvents CmdClose As DevExpress.XtraEditors.SimpleButton

End Class
