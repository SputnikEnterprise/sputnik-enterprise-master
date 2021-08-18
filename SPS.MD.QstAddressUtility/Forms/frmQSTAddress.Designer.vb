<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmQSTAddress
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmQSTAddress))
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
		Me.lblbemerkung = New System.Windows.Forms.Label()
		Me.txt_Bemerkung = New DevExpress.XtraEditors.MemoEdit()
		Me.txt_PLZ = New DevExpress.XtraEditors.TextEdit()
		Me.txt_Land = New DevExpress.XtraEditors.TextEdit()
		Me.txt_Provision = New DevExpress.XtraEditors.TextEdit()
		Me.LblChange_11 = New System.Windows.Forms.Label()
		Me.lblprovision = New System.Windows.Forms.Label()
		Me.lblstartnummer = New System.Windows.Forms.Label()
		Me.lblort = New System.Windows.Forms.Label()
		Me.lblgemeinde = New System.Windows.Forms.Label()
		Me.lblstrasse = New System.Windows.Forms.Label()
		Me.lblpostfach = New System.Windows.Forms.Label()
		Me.lblzhd = New System.Windows.Forms.Label()
		Me.txt_Strasse = New DevExpress.XtraEditors.TextEdit()
		Me.txt_Postfach = New DevExpress.XtraEditors.TextEdit()
		Me.txt_ZHD = New DevExpress.XtraEditors.TextEdit()
		Me.txt_StammNr = New DevExpress.XtraEditors.TextEdit()
		Me.txt_Ort = New DevExpress.XtraEditors.TextEdit()
		Me.Timer1 = New System.Windows.Forms.Timer()
		Me.LvFoundedrecs = New DevComponents.DotNetBar.Controls.ListViewEx()
		Me.lblzusatz = New System.Windows.Forms.Label()
		Me.lbladresse = New System.Windows.Forms.Label()
		Me.lblkanton = New System.Windows.Forms.Label()
		Me.txt_Zusatz = New DevExpress.XtraEditors.TextEdit()
		Me.txt_Adresse = New DevExpress.XtraEditors.TextEdit()
		Me.LblRecNr = New System.Windows.Forms.Label()
		Me.LblRecInfo = New System.Windows.Forms.Label()
		Me.txt_Gemeinde = New DevExpress.XtraEditors.TextEdit()
		Me.lbldetail = New System.Windows.Forms.Label()
		Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider()
		Me.Cbo_Kanton = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.lblland = New System.Windows.Forms.Label()
		Me.lblplz = New System.Windows.Forms.Label()
		Me.GroupBox1 = New DevExpress.XtraEditors.PanelControl()
		Me.Label37 = New System.Windows.Forms.Label()
		Me.LblTimeValue = New DevExpress.XtraEditors.LabelControl()
		Me.CmdClose = New DevExpress.XtraEditors.SimpleButton()
		Me.lblHeader2 = New System.Windows.Forms.Label()
		Me.Bar4 = New DevExpress.XtraBars.Bar()
		Me.bsiInfo = New DevExpress.XtraBars.BarStaticItem()
		Me.bbiSave = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiNew = New DevExpress.XtraBars.BarLargeButtonItem()
		Me.bbiDelete = New DevExpress.XtraBars.BarButtonItem()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager()
		Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
		Me.BarStaticItem1 = New DevExpress.XtraBars.BarStaticItem()
		CType(Me.txt_Bemerkung.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txt_PLZ.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txt_Land.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txt_Provision.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txt_Strasse.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txt_Postfach.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txt_ZHD.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txt_StammNr.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txt_Ort.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txt_Zusatz.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txt_Adresse.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txt_Gemeinde.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_Kanton.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupBox1.SuspendLayout()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'lblHeader1
		'
		Me.lblHeader1.AutoSize = True
		Me.lblHeader1.BackColor = System.Drawing.Color.Transparent
		Me.lblHeader1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblHeader1.Location = New System.Drawing.Point(102, 29)
		Me.lblHeader1.Name = "lblHeader1"
		Me.lblHeader1.Size = New System.Drawing.Size(258, 13)
		Me.lblHeader1.TabIndex = 0
		Me.lblHeader1.Text = "Adresse der kantonalen Steuerverwaltungen"
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
		'lblbemerkung
		'
		Me.lblbemerkung.Location = New System.Drawing.Point(30, 404)
		Me.lblbemerkung.Name = "lblbemerkung"
		Me.lblbemerkung.Size = New System.Drawing.Size(112, 13)
		Me.lblbemerkung.TabIndex = 74
		Me.lblbemerkung.Text = "Bemerkung"
		Me.lblbemerkung.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'txt_Bemerkung
		'
		Me.txt_Bemerkung.Location = New System.Drawing.Point(147, 404)
		Me.txt_Bemerkung.Name = "txt_Bemerkung"
		Me.txt_Bemerkung.Size = New System.Drawing.Size(384, 45)
		Me.txt_Bemerkung.TabIndex = 13
		'
		'txt_PLZ
		'
		Me.txt_PLZ.Location = New System.Drawing.Point(443, 307)
		Me.txt_PLZ.Name = "txt_PLZ"
		Me.txt_PLZ.Size = New System.Drawing.Size(88, 20)
		Me.txt_PLZ.TabIndex = 9
		'
		'txt_Land
		'
		Me.txt_Land.EditValue = "CH"
		Me.txt_Land.Location = New System.Drawing.Point(148, 307)
		Me.txt_Land.Name = "txt_Land"
		Me.txt_Land.Size = New System.Drawing.Size(54, 20)
		Me.txt_Land.TabIndex = 8
		'
		'txt_Provision
		'
		Me.txt_Provision.EditValue = "0.00"
		Me.txt_Provision.Location = New System.Drawing.Point(443, 378)
		Me.txt_Provision.Name = "txt_Provision"
		Me.txt_Provision.Properties.Appearance.Options.UseTextOptions = True
		Me.txt_Provision.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.txt_Provision.Size = New System.Drawing.Size(88, 20)
		Me.txt_Provision.TabIndex = 12
		'
		'LblChange_11
		'
		Me.LblChange_11.AutoSize = True
		Me.LblChange_11.Location = New System.Drawing.Point(534, 382)
		Me.LblChange_11.Name = "LblChange_11"
		Me.LblChange_11.Size = New System.Drawing.Size(18, 13)
		Me.LblChange_11.TabIndex = 71
		Me.LblChange_11.Text = "%"
		'
		'lblprovision
		'
		Me.lblprovision.Location = New System.Drawing.Point(346, 382)
		Me.lblprovision.Name = "lblprovision"
		Me.lblprovision.Size = New System.Drawing.Size(91, 13)
		Me.lblprovision.TabIndex = 70
		Me.lblprovision.Text = "Provision"
		Me.lblprovision.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblstartnummer
		'
		Me.lblstartnummer.Location = New System.Drawing.Point(30, 382)
		Me.lblstartnummer.Name = "lblstartnummer"
		Me.lblstartnummer.Size = New System.Drawing.Size(112, 13)
		Me.lblstartnummer.TabIndex = 69
		Me.lblstartnummer.Text = "Stammnummer"
		Me.lblstartnummer.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblort
		'
		Me.lblort.Location = New System.Drawing.Point(35, 337)
		Me.lblort.Name = "lblort"
		Me.lblort.Size = New System.Drawing.Size(107, 13)
		Me.lblort.TabIndex = 63
		Me.lblort.Text = "Ort"
		Me.lblort.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblgemeinde
		'
		Me.lblgemeinde.Location = New System.Drawing.Point(31, 156)
		Me.lblgemeinde.Name = "lblgemeinde"
		Me.lblgemeinde.Size = New System.Drawing.Size(112, 13)
		Me.lblgemeinde.TabIndex = 19
		Me.lblgemeinde.Text = "Gemeinde"
		Me.lblgemeinde.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblstrasse
		'
		Me.lblstrasse.Location = New System.Drawing.Point(31, 285)
		Me.lblstrasse.Name = "lblstrasse"
		Me.lblstrasse.Size = New System.Drawing.Size(112, 13)
		Me.lblstrasse.TabIndex = 18
		Me.lblstrasse.Text = "Strasse"
		Me.lblstrasse.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblpostfach
		'
		Me.lblpostfach.Location = New System.Drawing.Point(31, 260)
		Me.lblpostfach.Name = "lblpostfach"
		Me.lblpostfach.Size = New System.Drawing.Size(112, 13)
		Me.lblpostfach.TabIndex = 17
		Me.lblpostfach.Text = "Postfach"
		Me.lblpostfach.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblzhd
		'
		Me.lblzhd.Location = New System.Drawing.Point(31, 234)
		Me.lblzhd.Name = "lblzhd"
		Me.lblzhd.Size = New System.Drawing.Size(112, 13)
		Me.lblzhd.TabIndex = 16
		Me.lblzhd.Text = "Z. Hd."
		Me.lblzhd.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'txt_Strasse
		'
		Me.txt_Strasse.Location = New System.Drawing.Point(149, 281)
		Me.txt_Strasse.Name = "txt_Strasse"
		Me.txt_Strasse.Size = New System.Drawing.Size(384, 20)
		Me.txt_Strasse.TabIndex = 7
		'
		'txt_Postfach
		'
		Me.txt_Postfach.Location = New System.Drawing.Point(149, 256)
		Me.txt_Postfach.Name = "txt_Postfach"
		Me.txt_Postfach.Size = New System.Drawing.Size(384, 20)
		Me.txt_Postfach.TabIndex = 6
		'
		'txt_ZHD
		'
		Me.txt_ZHD.Location = New System.Drawing.Point(149, 230)
		Me.txt_ZHD.Name = "txt_ZHD"
		Me.txt_ZHD.Size = New System.Drawing.Size(384, 20)
		Me.txt_ZHD.TabIndex = 5
		'
		'txt_StammNr
		'
		Me.txt_StammNr.Location = New System.Drawing.Point(148, 378)
		Me.txt_StammNr.Name = "txt_StammNr"
		Me.txt_StammNr.Size = New System.Drawing.Size(192, 20)
		Me.txt_StammNr.TabIndex = 11
		'
		'txt_Ort
		'
		Me.txt_Ort.Location = New System.Drawing.Point(148, 333)
		Me.txt_Ort.Name = "txt_Ort"
		Me.txt_Ort.Size = New System.Drawing.Size(384, 20)
		Me.txt_Ort.TabIndex = 10
		'
		'LvFoundedrecs
		'
		Me.LvFoundedrecs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.LvFoundedrecs.BackColor = System.Drawing.Color.White
		Me.LvFoundedrecs.BackgroundImageTiled = True
		'
		'
		'
		Me.LvFoundedrecs.Border.Class = "ListViewBorder"
		Me.LvFoundedrecs.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
		Me.LvFoundedrecs.ForeColor = System.Drawing.Color.Black
		Me.LvFoundedrecs.FullRowSelect = True
		Me.LvFoundedrecs.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
		Me.LvFoundedrecs.Location = New System.Drawing.Point(30, 491)
		Me.LvFoundedrecs.MultiSelect = False
		Me.LvFoundedrecs.Name = "LvFoundedrecs"
		Me.LvFoundedrecs.ShowItemToolTips = True
		Me.LvFoundedrecs.Size = New System.Drawing.Size(528, 144)
		Me.LvFoundedrecs.TabIndex = 0
		Me.LvFoundedrecs.UseCompatibleStateImageBehavior = False
		Me.LvFoundedrecs.View = System.Windows.Forms.View.Details
		'
		'lblzusatz
		'
		Me.lblzusatz.Location = New System.Drawing.Point(31, 208)
		Me.lblzusatz.Name = "lblzusatz"
		Me.lblzusatz.Size = New System.Drawing.Size(112, 13)
		Me.lblzusatz.TabIndex = 63
		Me.lblzusatz.Text = "Zusatz"
		Me.lblzusatz.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lbladresse
		'
		Me.lbladresse.Location = New System.Drawing.Point(31, 182)
		Me.lbladresse.Name = "lbladresse"
		Me.lbladresse.Size = New System.Drawing.Size(112, 13)
		Me.lbladresse.TabIndex = 62
		Me.lbladresse.Text = "Adresse"
		Me.lbladresse.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblkanton
		'
		Me.lblkanton.Location = New System.Drawing.Point(31, 129)
		Me.lblkanton.Name = "lblkanton"
		Me.lblkanton.Size = New System.Drawing.Size(112, 13)
		Me.lblkanton.TabIndex = 61
		Me.lblkanton.Text = "Kanton"
		Me.lblkanton.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'txt_Zusatz
		'
		Me.txt_Zusatz.Location = New System.Drawing.Point(149, 204)
		Me.txt_Zusatz.Name = "txt_Zusatz"
		Me.txt_Zusatz.Size = New System.Drawing.Size(384, 20)
		Me.txt_Zusatz.TabIndex = 4
		'
		'txt_Adresse
		'
		Me.txt_Adresse.Location = New System.Drawing.Point(149, 178)
		Me.txt_Adresse.Name = "txt_Adresse"
		Me.txt_Adresse.Size = New System.Drawing.Size(384, 20)
		Me.txt_Adresse.TabIndex = 3
		'
		'LblRecNr
		'
		Me.LblRecNr.AutoSize = True
		Me.LblRecNr.Location = New System.Drawing.Point(570, 205)
		Me.LblRecNr.Name = "LblRecNr"
		Me.LblRecNr.Size = New System.Drawing.Size(13, 13)
		Me.LblRecNr.TabIndex = 110
		Me.LblRecNr.Text = "0"
		Me.LblRecNr.Visible = False
		'
		'LblRecInfo
		'
		Me.LblRecInfo.AutoSize = True
		Me.LblRecInfo.Location = New System.Drawing.Point(560, 143)
		Me.LblRecInfo.Name = "LblRecInfo"
		Me.LblRecInfo.Size = New System.Drawing.Size(161, 13)
		Me.LblRecInfo.TabIndex = 111
		Me.LblRecInfo.Text = "Keine Datensätze ausgewählt..."
		Me.LblRecInfo.Visible = False
		'
		'txt_Gemeinde
		'
		Me.txt_Gemeinde.Location = New System.Drawing.Point(149, 152)
		Me.txt_Gemeinde.Name = "txt_Gemeinde"
		Me.txt_Gemeinde.Properties.AccessibleDescription = ""
		Me.txt_Gemeinde.Properties.AccessibleName = ""
		Me.txt_Gemeinde.Size = New System.Drawing.Size(383, 20)
		Me.txt_Gemeinde.TabIndex = 2
		Me.txt_Gemeinde.Tag = ""
		'
		'lbldetail
		'
		Me.lbldetail.AutoSize = True
		Me.lbldetail.Location = New System.Drawing.Point(30, 473)
		Me.lbldetail.Name = "lbldetail"
		Me.lbldetail.Size = New System.Drawing.Size(141, 13)
		Me.lbldetail.TabIndex = 116
		Me.lbldetail.Text = "Bereits erfasste Datensätze"
		'
		'ErrorProvider1
		'
		Me.ErrorProvider1.ContainerControl = Me
		'
		'Cbo_Kanton
		'
		Me.Cbo_Kanton.Location = New System.Drawing.Point(149, 125)
		Me.Cbo_Kanton.Name = "Cbo_Kanton"
		Me.Cbo_Kanton.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.Cbo_Kanton.Size = New System.Drawing.Size(53, 20)
		Me.Cbo_Kanton.TabIndex = 1
		'
		'lblland
		'
		Me.lblland.Location = New System.Drawing.Point(31, 311)
		Me.lblland.Name = "lblland"
		Me.lblland.Size = New System.Drawing.Size(112, 13)
		Me.lblland.TabIndex = 117
		Me.lblland.Text = "Land"
		Me.lblland.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblplz
		'
		Me.lblplz.Location = New System.Drawing.Point(371, 310)
		Me.lblplz.Name = "lblplz"
		Me.lblplz.Size = New System.Drawing.Size(66, 13)
		Me.lblplz.TabIndex = 118
		Me.lblplz.Text = "PLZ"
		Me.lblplz.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'GroupBox1
		'
		Me.GroupBox1.Appearance.BackColor = System.Drawing.Color.White
		Me.GroupBox1.Appearance.Options.UseBackColor = True
		Me.GroupBox1.Controls.Add(Me.Label37)
		Me.GroupBox1.Controls.Add(Me.LblTimeValue)
		Me.GroupBox1.Controls.Add(Me.CmdClose)
		Me.GroupBox1.Controls.Add(Me.lblHeader2)
		Me.GroupBox1.Controls.Add(Me.lblHeader1)
		Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
		Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(593, 93)
		Me.GroupBox1.TabIndex = 189
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
		Me.LblTimeValue.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.LblTimeValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.LblTimeValue.Location = New System.Drawing.Point(346, 53)
		Me.LblTimeValue.Name = "LblTimeValue"
		Me.LblTimeValue.Size = New System.Drawing.Size(201, 13)
		Me.LblTimeValue.TabIndex = 14
		Me.LblTimeValue.Text = "Zeitangaben..."
		Me.LblTimeValue.Visible = False
		'
		'CmdClose
		'
		Me.CmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.CmdClose.Location = New System.Drawing.Point(461, 29)
		Me.CmdClose.Name = "CmdClose"
		Me.CmdClose.Size = New System.Drawing.Size(86, 24)
		Me.CmdClose.TabIndex = 100
		Me.CmdClose.TabStop = False
		Me.CmdClose.Text = "Schliessen"
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
		Me.bsiInfo.AutoSize = DevExpress.XtraBars.BarStaticItemSize.Spring
		Me.bsiInfo.Caption = "Bereit"
		Me.bsiInfo.Id = 0
		Me.bsiInfo.Name = "bsiInfo"
		Me.bsiInfo.TextAlignment = System.Drawing.StringAlignment.Near
		Me.bsiInfo.Width = 32
		'
		'bbiSave
		'
		Me.bbiSave.Caption = "Daten sichern"
		Me.bbiSave.Glyph = CType(resources.GetObject("bbiSave.Glyph"), System.Drawing.Image)
		Me.bbiSave.Id = 1
		Me.bbiSave.Name = "bbiSave"
		Me.bbiSave.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiNew
		'
		Me.bbiNew.Caption = "Neu hinzufügen"
		Me.bbiNew.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Right
		Me.bbiNew.Glyph = CType(resources.GetObject("bbiNew.Glyph"), System.Drawing.Image)
		Me.bbiNew.Id = 6
		Me.bbiNew.Name = "bbiNew"
		Me.bbiNew.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiDelete
		'
		Me.bbiDelete.Caption = "Löschen"
		Me.bbiDelete.Glyph = CType(resources.GetObject("bbiDelete.Glyph"), System.Drawing.Image)
		Me.bbiDelete.Id = 8
		Me.bbiDelete.Name = "bbiDelete"
		Me.bbiDelete.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
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
		'barDockControlTop
		'
		Me.barDockControlTop.CausesValidation = False
		Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
		Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlTop.Size = New System.Drawing.Size(593, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 675)
		Me.barDockControlBottom.Size = New System.Drawing.Size(593, 27)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 675)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(593, 0)
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 675)
		'
		'BarStaticItem1
		'
		Me.BarStaticItem1.Caption = " "
		Me.BarStaticItem1.Id = 5
		Me.BarStaticItem1.Name = "BarStaticItem1"
		Me.BarStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near
		'
		'frmQSTAddress
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(593, 702)
		Me.Controls.Add(Me.GroupBox1)
		Me.Controls.Add(Me.lblplz)
		Me.Controls.Add(Me.lblland)
		Me.Controls.Add(Me.lbldetail)
		Me.Controls.Add(Me.txt_Gemeinde)
		Me.Controls.Add(Me.LblRecNr)
		Me.Controls.Add(Me.lblkanton)
		Me.Controls.Add(Me.Cbo_Kanton)
		Me.Controls.Add(Me.LblRecInfo)
		Me.Controls.Add(Me.lblzusatz)
		Me.Controls.Add(Me.LvFoundedrecs)
		Me.Controls.Add(Me.lblgemeinde)
		Me.Controls.Add(Me.lblbemerkung)
		Me.Controls.Add(Me.txt_Bemerkung)
		Me.Controls.Add(Me.txt_Adresse)
		Me.Controls.Add(Me.lbladresse)
		Me.Controls.Add(Me.txt_Provision)
		Me.Controls.Add(Me.txt_Strasse)
		Me.Controls.Add(Me.lblprovision)
		Me.Controls.Add(Me.txt_Land)
		Me.Controls.Add(Me.txt_PLZ)
		Me.Controls.Add(Me.txt_Zusatz)
		Me.Controls.Add(Me.lblpostfach)
		Me.Controls.Add(Me.lblzhd)
		Me.Controls.Add(Me.txt_Ort)
		Me.Controls.Add(Me.txt_ZHD)
		Me.Controls.Add(Me.lblort)
		Me.Controls.Add(Me.LblChange_11)
		Me.Controls.Add(Me.lblstrasse)
		Me.Controls.Add(Me.txt_Postfach)
		Me.Controls.Add(Me.lblstartnummer)
		Me.Controls.Add(Me.txt_StammNr)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximumSize = New System.Drawing.Size(609, 740)
		Me.MinimumSize = New System.Drawing.Size(609, 740)
		Me.Name = "frmQSTAddress"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Adresse für Quellensteuer Organisationen"
		CType(Me.txt_Bemerkung.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txt_PLZ.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txt_Land.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txt_Provision.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txt_Strasse.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txt_Postfach.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txt_ZHD.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txt_StammNr.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txt_Ort.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txt_Zusatz.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txt_Adresse.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txt_Gemeinde.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_Kanton.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupBox1.ResumeLayout(False)
		Me.GroupBox1.PerformLayout()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
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
  Friend WithEvents lblgemeinde As System.Windows.Forms.Label
  Friend WithEvents lblstrasse As System.Windows.Forms.Label
  Friend WithEvents lblpostfach As System.Windows.Forms.Label
  Friend WithEvents lblzhd As System.Windows.Forms.Label
  Friend WithEvents txt_Strasse As DevExpress.XtraEditors.TextEdit
  Friend WithEvents txt_Postfach As DevExpress.XtraEditors.TextEdit
  Friend WithEvents txt_ZHD As DevExpress.XtraEditors.TextEdit
  Friend WithEvents txt_StammNr As DevExpress.XtraEditors.TextEdit
  Friend WithEvents txt_Ort As DevExpress.XtraEditors.TextEdit
  Friend WithEvents LblChange_11 As System.Windows.Forms.Label
  Friend WithEvents lblprovision As System.Windows.Forms.Label
  Friend WithEvents lblstartnummer As System.Windows.Forms.Label
  Friend WithEvents lblort As System.Windows.Forms.Label
  Friend WithEvents txt_Bemerkung As DevExpress.XtraEditors.MemoEdit
  Friend WithEvents txt_PLZ As DevExpress.XtraEditors.TextEdit
  Friend WithEvents txt_Land As DevExpress.XtraEditors.TextEdit
  Friend WithEvents txt_Provision As DevExpress.XtraEditors.TextEdit
  Friend WithEvents Timer1 As System.Windows.Forms.Timer
  Friend WithEvents lblbemerkung As System.Windows.Forms.Label
  Friend WithEvents LvFoundedrecs As DevComponents.DotNetBar.Controls.ListViewEx
  Friend WithEvents Cbo_Kanton As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents lblzusatz As System.Windows.Forms.Label
  Friend WithEvents lbladresse As System.Windows.Forms.Label
  Friend WithEvents lblkanton As System.Windows.Forms.Label
  Friend WithEvents txt_Zusatz As DevExpress.XtraEditors.TextEdit
  Friend WithEvents txt_Adresse As DevExpress.XtraEditors.TextEdit
  Friend WithEvents LblRecNr As System.Windows.Forms.Label
  Friend WithEvents LblRecInfo As System.Windows.Forms.Label
  Friend WithEvents txt_Gemeinde As DevExpress.XtraEditors.TextEdit
  Friend WithEvents lbldetail As System.Windows.Forms.Label
  Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
  Friend WithEvents lblplz As System.Windows.Forms.Label
  Friend WithEvents lblland As System.Windows.Forms.Label
  Friend WithEvents GroupBox1 As DevExpress.XtraEditors.PanelControl
  Friend WithEvents Label37 As System.Windows.Forms.Label
  Friend WithEvents LblTimeValue As DevExpress.XtraEditors.LabelControl
  Friend WithEvents CmdClose As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents lblHeader2 As System.Windows.Forms.Label
  Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
  Friend WithEvents Bar4 As DevExpress.XtraBars.Bar
  Friend WithEvents bsiInfo As DevExpress.XtraBars.BarStaticItem
  Friend WithEvents bbiSave As DevExpress.XtraBars.BarButtonItem
  Friend WithEvents bbiNew As DevExpress.XtraBars.BarLargeButtonItem
  Friend WithEvents bbiDelete As DevExpress.XtraBars.BarButtonItem
  Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
  Friend WithEvents BarStaticItem1 As DevExpress.XtraBars.BarStaticItem

End Class
