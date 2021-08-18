<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmESPrintSetting
  Inherits DevComponents.DotNetBar.Metro.MetroForm

  'Form overrides dispose to clean up the component list.
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

  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer

  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.  
  'Do not modify it using the code editor.
  <System.Diagnostics.DebuggerStepThrough()> _
  Private Sub InitializeComponent()
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmESPrintSetting))
    Me.xtabSetting = New DevExpress.XtraTab.XtraTabControl()
    Me.xtabPrintSetting = New DevExpress.XtraTab.XtraTabPage()
    Me.btnSaveTextValues = New DevExpress.XtraEditors.SimpleButton()
    Me.grpEinstellung = New DevExpress.XtraEditors.GroupControl()
    Me.grpUnterzeichner = New DevExpress.XtraEditors.GroupControl()
    Me.lblAktiviert = New DevExpress.XtraEditors.LabelControl()
    Me.chkUnterzeichner = New DevExpress.XtraEditors.CheckEdit()
    Me.grpExport = New DevExpress.XtraEditors.GroupControl()
    Me.lblDateizusammenfuegen = New DevExpress.XtraEditors.LabelControl()
    Me.txt_ExportFinalFileVerleih = New DevExpress.XtraEditors.TextEdit()
    Me.lblVerleihvertragDatei = New DevExpress.XtraEditors.LabelControl()
    Me.txt_ExportFileVerleihVertrag = New DevExpress.XtraEditors.TextEdit()
    Me.lblExportVerleih = New DevExpress.XtraEditors.LabelControl()
    Me.lblAnmerkung2 = New DevExpress.XtraEditors.LabelControl()
    Me.cbo_ExportPfad = New DevExpress.XtraEditors.ComboBoxEdit()
    Me.lblAnmerkung1 = New DevExpress.XtraEditors.LabelControl()
    Me.txt_ExportFinalFileESVertrag = New DevExpress.XtraEditors.TextEdit()
    Me.txt_ExportFileESVertrag = New DevExpress.XtraEditors.TextEdit()
    Me.lblESVertragDatei = New DevExpress.XtraEditors.LabelControl()
    Me.lblExportEinsatzvertrag = New DevExpress.XtraEditors.LabelControl()
    Me.lblDateipfad = New DevExpress.XtraEditors.LabelControl()
    Me.grpAnzahl = New DevExpress.XtraEditors.GroupControl()
    Me.lblVerleihvertrag = New DevExpress.XtraEditors.LabelControl()
    Me.txt_AnzKopienVerleih = New DevExpress.XtraEditors.SpinEdit()
    Me.lblEinsatzvertrag = New DevExpress.XtraEditors.LabelControl()
    Me.txt_AnzKopienESVertrag = New DevExpress.XtraEditors.SpinEdit()
    CType(Me.xtabSetting, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.xtabSetting.SuspendLayout()
    Me.xtabPrintSetting.SuspendLayout()
    CType(Me.grpEinstellung, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.grpEinstellung.SuspendLayout()
    CType(Me.grpUnterzeichner, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.grpUnterzeichner.SuspendLayout()
    CType(Me.chkUnterzeichner.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.grpExport, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.grpExport.SuspendLayout()
    CType(Me.txt_ExportFinalFileVerleih.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txt_ExportFileVerleihVertrag.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.cbo_ExportPfad.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txt_ExportFinalFileESVertrag.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txt_ExportFileESVertrag.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.grpAnzahl, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.grpAnzahl.SuspendLayout()
    CType(Me.txt_AnzKopienVerleih.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txt_AnzKopienESVertrag.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'xtabSetting
    '
    Me.xtabSetting.Appearance.BackColor = System.Drawing.Color.White
    Me.xtabSetting.Appearance.ForeColor = System.Drawing.Color.Black
    Me.xtabSetting.Appearance.Options.UseBackColor = True
    Me.xtabSetting.Appearance.Options.UseForeColor = True
    Me.xtabSetting.Dock = System.Windows.Forms.DockStyle.Fill
    Me.xtabSetting.Location = New System.Drawing.Point(0, 0)
    Me.xtabSetting.Name = "xtabSetting"
    Me.xtabSetting.SelectedTabPage = Me.xtabPrintSetting
    Me.xtabSetting.Size = New System.Drawing.Size(1121, 460)
    Me.xtabSetting.TabIndex = 2
    Me.xtabSetting.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabPrintSetting})
    '
    'xtabPrintSetting
    '
    Me.xtabPrintSetting.Appearance.PageClient.ForeColor = System.Drawing.Color.Black
    Me.xtabPrintSetting.Appearance.PageClient.Options.UseForeColor = True
    Me.xtabPrintSetting.Controls.Add(Me.btnSaveTextValues)
    Me.xtabPrintSetting.Controls.Add(Me.grpEinstellung)
    Me.xtabPrintSetting.Name = "xtabPrintSetting"
    Me.xtabPrintSetting.Size = New System.Drawing.Size(1115, 432)
    Me.xtabPrintSetting.Text = "Druck und Export der Einsatzverträge"
    '
    'btnSaveTextValues
    '
    Me.btnSaveTextValues.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnSaveTextValues.Appearance.ForeColor = System.Drawing.Color.Black
    Me.btnSaveTextValues.Appearance.Options.UseForeColor = True
    Me.btnSaveTextValues.Location = New System.Drawing.Point(992, 50)
    Me.btnSaveTextValues.Name = "btnSaveTextValues"
    Me.btnSaveTextValues.Size = New System.Drawing.Size(104, 26)
    Me.btnSaveTextValues.TabIndex = 0
    Me.btnSaveTextValues.Text = "Speichern"
    '
    'grpEinstellung
    '
    Me.grpEinstellung.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.grpEinstellung.Appearance.BackColor = System.Drawing.Color.White
    Me.grpEinstellung.Appearance.ForeColor = System.Drawing.Color.Black
    Me.grpEinstellung.Appearance.Options.UseBackColor = True
    Me.grpEinstellung.Appearance.Options.UseForeColor = True
    Me.grpEinstellung.Controls.Add(Me.grpUnterzeichner)
    Me.grpEinstellung.Controls.Add(Me.grpExport)
    Me.grpEinstellung.Controls.Add(Me.grpAnzahl)
    Me.grpEinstellung.Location = New System.Drawing.Point(11, 15)
    Me.grpEinstellung.Name = "grpEinstellung"
    Me.grpEinstellung.Size = New System.Drawing.Size(963, 407)
    Me.grpEinstellung.TabIndex = 0
    Me.grpEinstellung.Text = "Einstellungen"
    '
    'grpUnterzeichner
    '
    Me.grpUnterzeichner.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.grpUnterzeichner.Appearance.BackColor = System.Drawing.Color.White
    Me.grpUnterzeichner.Appearance.ForeColor = System.Drawing.Color.Black
    Me.grpUnterzeichner.Appearance.Options.UseBackColor = True
    Me.grpUnterzeichner.Appearance.Options.UseForeColor = True
    Me.grpUnterzeichner.Controls.Add(Me.lblAktiviert)
    Me.grpUnterzeichner.Controls.Add(Me.chkUnterzeichner)
    Me.grpUnterzeichner.Location = New System.Drawing.Point(338, 35)
    Me.grpUnterzeichner.Name = "grpUnterzeichner"
    Me.grpUnterzeichner.Size = New System.Drawing.Size(604, 115)
    Me.grpUnterzeichner.TabIndex = 25
    Me.grpUnterzeichner.Text = "Wie soll der Unterzeichner gedruckt werden?"
    '
    'lblAktiviert
    '
    Me.lblAktiviert.AllowHtmlString = True
    Me.lblAktiviert.Appearance.BackColor = System.Drawing.Color.Transparent
    Me.lblAktiviert.Appearance.ForeColor = System.Drawing.Color.Black
    Me.lblAktiviert.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
    Me.lblAktiviert.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.lblAktiviert.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
    Me.lblAktiviert.LineColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
    Me.lblAktiviert.LineLocation = DevExpress.XtraEditors.LineLocation.Left
    Me.lblAktiviert.LineVisible = True
    Me.lblAktiviert.Location = New System.Drawing.Point(16, 55)
    Me.lblAktiviert.Name = "lblAktiviert"
    Me.lblAktiviert.Size = New System.Drawing.Size(570, 51)
    Me.lblAktiviert.TabIndex = 9
    Me.lblAktiviert.Text = resources.GetString("lblAktiviert.Text")
    '
    'chkUnterzeichner
    '
    Me.chkUnterzeichner.Location = New System.Drawing.Point(16, 31)
    Me.chkUnterzeichner.Name = "chkUnterzeichner"
    Me.chkUnterzeichner.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.[True]
    Me.chkUnterzeichner.Properties.Appearance.Options.UseTextOptions = True
    Me.chkUnterzeichner.Properties.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.chkUnterzeichner.Properties.Caption = "Benutzerdaten = Unterzeichner"
    Me.chkUnterzeichner.Size = New System.Drawing.Size(248, 19)
    Me.chkUnterzeichner.TabIndex = 0
    '
    'grpExport
    '
    Me.grpExport.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.grpExport.Appearance.BackColor = System.Drawing.Color.White
    Me.grpExport.Appearance.ForeColor = System.Drawing.Color.Black
    Me.grpExport.Appearance.Options.UseBackColor = True
    Me.grpExport.Appearance.Options.UseForeColor = True
    Me.grpExport.Controls.Add(Me.lblDateizusammenfuegen)
    Me.grpExport.Controls.Add(Me.txt_ExportFinalFileVerleih)
    Me.grpExport.Controls.Add(Me.lblVerleihvertragDatei)
    Me.grpExport.Controls.Add(Me.txt_ExportFileVerleihVertrag)
    Me.grpExport.Controls.Add(Me.lblExportVerleih)
    Me.grpExport.Controls.Add(Me.lblAnmerkung2)
    Me.grpExport.Controls.Add(Me.cbo_ExportPfad)
    Me.grpExport.Controls.Add(Me.lblAnmerkung1)
    Me.grpExport.Controls.Add(Me.txt_ExportFinalFileESVertrag)
    Me.grpExport.Controls.Add(Me.txt_ExportFileESVertrag)
    Me.grpExport.Controls.Add(Me.lblESVertragDatei)
    Me.grpExport.Controls.Add(Me.lblExportEinsatzvertrag)
    Me.grpExport.Controls.Add(Me.lblDateipfad)
    Me.grpExport.Location = New System.Drawing.Point(16, 156)
    Me.grpExport.Name = "grpExport"
    Me.grpExport.Size = New System.Drawing.Size(926, 234)
    Me.grpExport.TabIndex = 24
    Me.grpExport.Text = "Export der Verträge"
    '
    'lblDateizusammenfuegen
    '
    Me.lblDateizusammenfuegen.AllowHtmlString = True
    Me.lblDateizusammenfuegen.Appearance.BackColor = System.Drawing.Color.Transparent
    Me.lblDateizusammenfuegen.Appearance.ForeColor = System.Drawing.Color.Black
    Me.lblDateizusammenfuegen.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    Me.lblDateizusammenfuegen.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
    Me.lblDateizusammenfuegen.Location = New System.Drawing.Point(12, 125)
    Me.lblDateizusammenfuegen.Name = "lblDateizusammenfuegen"
    Me.lblDateizusammenfuegen.Size = New System.Drawing.Size(200, 13)
    Me.lblDateizusammenfuegen.TabIndex = 14
    Me.lblDateizusammenfuegen.Text = "<b>Dateien zusammenfügen</b>"
    '
    'txt_ExportFinalFileVerleih
    '
    Me.txt_ExportFinalFileVerleih.Location = New System.Drawing.Point(218, 170)
    Me.txt_ExportFinalFileVerleih.Name = "txt_ExportFinalFileVerleih"
    Me.txt_ExportFinalFileVerleih.Properties.Appearance.BackColor = System.Drawing.Color.White
    Me.txt_ExportFinalFileVerleih.Properties.Appearance.ForeColor = System.Drawing.Color.Black
    Me.txt_ExportFinalFileVerleih.Properties.Appearance.Options.UseBackColor = True
    Me.txt_ExportFinalFileVerleih.Properties.Appearance.Options.UseForeColor = True
    Me.txt_ExportFinalFileVerleih.Size = New System.Drawing.Size(164, 20)
    Me.txt_ExportFinalFileVerleih.TabIndex = 4
    '
    'lblVerleihvertragDatei
    '
    Me.lblVerleihvertragDatei.AllowHtmlString = True
    Me.lblVerleihvertragDatei.Appearance.BackColor = System.Drawing.Color.Transparent
    Me.lblVerleihvertragDatei.Appearance.ForeColor = System.Drawing.Color.Black
    Me.lblVerleihvertragDatei.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    Me.lblVerleihvertragDatei.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
    Me.lblVerleihvertragDatei.Location = New System.Drawing.Point(12, 174)
    Me.lblVerleihvertragDatei.Name = "lblVerleihvertragDatei"
    Me.lblVerleihvertragDatei.Size = New System.Drawing.Size(200, 13)
    Me.lblVerleihvertragDatei.TabIndex = 13
    Me.lblVerleihvertragDatei.Text = "Verleihvertrag"
    '
    'txt_ExportFileVerleihVertrag
    '
    Me.txt_ExportFileVerleihVertrag.Location = New System.Drawing.Point(218, 91)
    Me.txt_ExportFileVerleihVertrag.Name = "txt_ExportFileVerleihVertrag"
    Me.txt_ExportFileVerleihVertrag.Properties.Appearance.BackColor = System.Drawing.Color.White
    Me.txt_ExportFileVerleihVertrag.Properties.Appearance.ForeColor = System.Drawing.Color.Black
    Me.txt_ExportFileVerleihVertrag.Properties.Appearance.Options.UseBackColor = True
    Me.txt_ExportFileVerleihVertrag.Properties.Appearance.Options.UseForeColor = True
    Me.txt_ExportFileVerleihVertrag.Size = New System.Drawing.Size(164, 20)
    Me.txt_ExportFileVerleihVertrag.TabIndex = 2
    '
    'lblExportVerleih
    '
    Me.lblExportVerleih.AllowHtmlString = True
    Me.lblExportVerleih.Appearance.BackColor = System.Drawing.Color.Transparent
    Me.lblExportVerleih.Appearance.ForeColor = System.Drawing.Color.Black
    Me.lblExportVerleih.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    Me.lblExportVerleih.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
    Me.lblExportVerleih.Location = New System.Drawing.Point(12, 95)
    Me.lblExportVerleih.Name = "lblExportVerleih"
    Me.lblExportVerleih.Size = New System.Drawing.Size(200, 13)
    Me.lblExportVerleih.TabIndex = 11
    Me.lblExportVerleih.Text = "Export-Datei für Verleihvertrag"
    '
    'lblAnmerkung2
    '
    Me.lblAnmerkung2.AllowHtmlString = True
    Me.lblAnmerkung2.Appearance.BackColor = System.Drawing.Color.Transparent
    Me.lblAnmerkung2.Appearance.ForeColor = System.Drawing.Color.Black
    Me.lblAnmerkung2.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
    Me.lblAnmerkung2.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.lblAnmerkung2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
    Me.lblAnmerkung2.LineColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
    Me.lblAnmerkung2.LineLocation = DevExpress.XtraEditors.LineLocation.Left
    Me.lblAnmerkung2.LineVisible = True
    Me.lblAnmerkung2.Location = New System.Drawing.Point(388, 144)
    Me.lblAnmerkung2.Name = "lblAnmerkung2"
    Me.lblAnmerkung2.Size = New System.Drawing.Size(212, 73)
    Me.lblAnmerkung2.TabIndex = 9
    Me.lblAnmerkung2.Text = "<b>Anmerkung:</b>" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "FinalDateiname_<b><i>{0}</i></b>_<b><i>{1}</i></b>.PDF" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "<b>{0}" & _
    ":</b> Monat" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "<b>{1}:</b> Jahr" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Beispiel: FinalES_<b>01</b>_<b>2013</b>.PDF"
    '
    'cbo_ExportPfad
    '
    Me.cbo_ExportPfad.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cbo_ExportPfad.Location = New System.Drawing.Point(218, 39)
    Me.cbo_ExportPfad.Name = "cbo_ExportPfad"
    Me.cbo_ExportPfad.Properties.Appearance.BackColor = System.Drawing.Color.White
    Me.cbo_ExportPfad.Properties.Appearance.ForeColor = System.Drawing.Color.Black
    Me.cbo_ExportPfad.Properties.Appearance.Options.UseBackColor = True
    Me.cbo_ExportPfad.Properties.Appearance.Options.UseForeColor = True
    Me.cbo_ExportPfad.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.cbo_ExportPfad.Size = New System.Drawing.Size(692, 20)
    Me.cbo_ExportPfad.TabIndex = 0
    '
    'lblAnmerkung1
    '
    Me.lblAnmerkung1.AllowHtmlString = True
    Me.lblAnmerkung1.Appearance.BackColor = System.Drawing.Color.Transparent
    Me.lblAnmerkung1.Appearance.ForeColor = System.Drawing.Color.Black
    Me.lblAnmerkung1.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
    Me.lblAnmerkung1.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.lblAnmerkung1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
    Me.lblAnmerkung1.LineColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
    Me.lblAnmerkung1.LineLocation = DevExpress.XtraEditors.LineLocation.Left
    Me.lblAnmerkung1.LineVisible = True
    Me.lblAnmerkung1.Location = New System.Drawing.Point(388, 65)
    Me.lblAnmerkung1.Name = "lblAnmerkung1"
    Me.lblAnmerkung1.Size = New System.Drawing.Size(212, 73)
    Me.lblAnmerkung1.TabIndex = 8
    Me.lblAnmerkung1.Text = "<b>Anmerkung:</b>" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Dateiname_<b><i>{0}</i></b>_<b><i>{1}</i></b>.PDF" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "<b>{0}:</b>" & _
    " Einsatznummer" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "<b>{1}:</b> Kandidatennummer" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Beispiel: ES_<b>2252</b>_<b>21</b>" & _
    ".PDF"
    '
    'txt_ExportFinalFileESVertrag
    '
    Me.txt_ExportFinalFileESVertrag.Location = New System.Drawing.Point(218, 144)
    Me.txt_ExportFinalFileESVertrag.Name = "txt_ExportFinalFileESVertrag"
    Me.txt_ExportFinalFileESVertrag.Properties.Appearance.BackColor = System.Drawing.Color.White
    Me.txt_ExportFinalFileESVertrag.Properties.Appearance.ForeColor = System.Drawing.Color.Black
    Me.txt_ExportFinalFileESVertrag.Properties.Appearance.Options.UseBackColor = True
    Me.txt_ExportFinalFileESVertrag.Properties.Appearance.Options.UseForeColor = True
    Me.txt_ExportFinalFileESVertrag.Size = New System.Drawing.Size(164, 20)
    Me.txt_ExportFinalFileESVertrag.TabIndex = 3
    '
    'txt_ExportFileESVertrag
    '
    Me.txt_ExportFileESVertrag.Location = New System.Drawing.Point(218, 65)
    Me.txt_ExportFileESVertrag.Name = "txt_ExportFileESVertrag"
    Me.txt_ExportFileESVertrag.Properties.Appearance.BackColor = System.Drawing.Color.White
    Me.txt_ExportFileESVertrag.Properties.Appearance.ForeColor = System.Drawing.Color.Black
    Me.txt_ExportFileESVertrag.Properties.Appearance.Options.UseBackColor = True
    Me.txt_ExportFileESVertrag.Properties.Appearance.Options.UseForeColor = True
    Me.txt_ExportFileESVertrag.Size = New System.Drawing.Size(164, 20)
    Me.txt_ExportFileESVertrag.TabIndex = 1
    '
    'lblESVertragDatei
    '
    Me.lblESVertragDatei.AllowHtmlString = True
    Me.lblESVertragDatei.Appearance.BackColor = System.Drawing.Color.Transparent
    Me.lblESVertragDatei.Appearance.ForeColor = System.Drawing.Color.Black
    Me.lblESVertragDatei.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    Me.lblESVertragDatei.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
    Me.lblESVertragDatei.Location = New System.Drawing.Point(12, 148)
    Me.lblESVertragDatei.Name = "lblESVertragDatei"
    Me.lblESVertragDatei.Size = New System.Drawing.Size(200, 13)
    Me.lblESVertragDatei.TabIndex = 7
    Me.lblESVertragDatei.Text = "Einsatzvertrag"
    '
    'lblExportEinsatzvertrag
    '
    Me.lblExportEinsatzvertrag.AllowHtmlString = True
    Me.lblExportEinsatzvertrag.Appearance.BackColor = System.Drawing.Color.Transparent
    Me.lblExportEinsatzvertrag.Appearance.ForeColor = System.Drawing.Color.Black
    Me.lblExportEinsatzvertrag.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    Me.lblExportEinsatzvertrag.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
    Me.lblExportEinsatzvertrag.Location = New System.Drawing.Point(12, 69)
    Me.lblExportEinsatzvertrag.Name = "lblExportEinsatzvertrag"
    Me.lblExportEinsatzvertrag.Size = New System.Drawing.Size(200, 13)
    Me.lblExportEinsatzvertrag.TabIndex = 3
    Me.lblExportEinsatzvertrag.Text = "Export-Datei für Einsatzvertrag"
    '
    'lblDateipfad
    '
    Me.lblDateipfad.AllowHtmlString = True
    Me.lblDateipfad.Appearance.BackColor = System.Drawing.Color.Transparent
    Me.lblDateipfad.Appearance.ForeColor = System.Drawing.Color.Black
    Me.lblDateipfad.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    Me.lblDateipfad.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
    Me.lblDateipfad.Location = New System.Drawing.Point(12, 43)
    Me.lblDateipfad.Name = "lblDateipfad"
    Me.lblDateipfad.Size = New System.Drawing.Size(200, 13)
    Me.lblDateipfad.TabIndex = 5
    Me.lblDateipfad.Text = "Export-Dateipfad"
    '
    'grpAnzahl
    '
    Me.grpAnzahl.Appearance.BackColor = System.Drawing.Color.White
    Me.grpAnzahl.Appearance.ForeColor = System.Drawing.Color.Black
    Me.grpAnzahl.Appearance.Options.UseBackColor = True
    Me.grpAnzahl.Appearance.Options.UseForeColor = True
    Me.grpAnzahl.Controls.Add(Me.lblVerleihvertrag)
    Me.grpAnzahl.Controls.Add(Me.txt_AnzKopienVerleih)
    Me.grpAnzahl.Controls.Add(Me.lblEinsatzvertrag)
    Me.grpAnzahl.Controls.Add(Me.txt_AnzKopienESVertrag)
    Me.grpAnzahl.Location = New System.Drawing.Point(16, 34)
    Me.grpAnzahl.Name = "grpAnzahl"
    Me.grpAnzahl.Size = New System.Drawing.Size(316, 115)
    Me.grpAnzahl.TabIndex = 23
    Me.grpAnzahl.Text = "Anzahl Kopien für alle Sprachen"
    '
    'lblVerleihvertrag
    '
    Me.lblVerleihvertrag.AllowHtmlString = True
    Me.lblVerleihvertrag.Appearance.BackColor = System.Drawing.Color.Transparent
    Me.lblVerleihvertrag.Appearance.ForeColor = System.Drawing.Color.Black
    Me.lblVerleihvertrag.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    Me.lblVerleihvertrag.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
    Me.lblVerleihvertrag.Location = New System.Drawing.Point(12, 60)
    Me.lblVerleihvertrag.Name = "lblVerleihvertrag"
    Me.lblVerleihvertrag.Size = New System.Drawing.Size(200, 13)
    Me.lblVerleihvertrag.TabIndex = 4
    Me.lblVerleihvertrag.Text = "Verleihvertrag"
    '
    'txt_AnzKopienVerleih
    '
    Me.txt_AnzKopienVerleih.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
    Me.txt_AnzKopienVerleih.Location = New System.Drawing.Point(218, 56)
    Me.txt_AnzKopienVerleih.Name = "txt_AnzKopienVerleih"
    Me.txt_AnzKopienVerleih.Properties.Appearance.BackColor = System.Drawing.Color.White
    Me.txt_AnzKopienVerleih.Properties.Appearance.ForeColor = System.Drawing.Color.Black
    Me.txt_AnzKopienVerleih.Properties.Appearance.Options.UseBackColor = True
    Me.txt_AnzKopienVerleih.Properties.Appearance.Options.UseForeColor = True
    Me.txt_AnzKopienVerleih.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.txt_AnzKopienVerleih.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.[Default]
    Me.txt_AnzKopienVerleih.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None
    Me.txt_AnzKopienVerleih.Size = New System.Drawing.Size(75, 20)
    Me.txt_AnzKopienVerleih.TabIndex = 1
    '
    'lblEinsatzvertrag
    '
    Me.lblEinsatzvertrag.AllowHtmlString = True
    Me.lblEinsatzvertrag.Appearance.BackColor = System.Drawing.Color.Transparent
    Me.lblEinsatzvertrag.Appearance.ForeColor = System.Drawing.Color.Black
    Me.lblEinsatzvertrag.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    Me.lblEinsatzvertrag.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
    Me.lblEinsatzvertrag.Location = New System.Drawing.Point(12, 35)
    Me.lblEinsatzvertrag.Name = "lblEinsatzvertrag"
    Me.lblEinsatzvertrag.Size = New System.Drawing.Size(200, 13)
    Me.lblEinsatzvertrag.TabIndex = 1
    Me.lblEinsatzvertrag.Text = "Einsatzvertrag"
    '
    'txt_AnzKopienESVertrag
    '
    Me.txt_AnzKopienESVertrag.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
    Me.txt_AnzKopienESVertrag.Location = New System.Drawing.Point(218, 31)
    Me.txt_AnzKopienESVertrag.Name = "txt_AnzKopienESVertrag"
    Me.txt_AnzKopienESVertrag.Properties.Appearance.BackColor = System.Drawing.Color.White
    Me.txt_AnzKopienESVertrag.Properties.Appearance.ForeColor = System.Drawing.Color.Black
    Me.txt_AnzKopienESVertrag.Properties.Appearance.Options.UseBackColor = True
    Me.txt_AnzKopienESVertrag.Properties.Appearance.Options.UseForeColor = True
    Me.txt_AnzKopienESVertrag.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.txt_AnzKopienESVertrag.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.[Default]
    Me.txt_AnzKopienESVertrag.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None
    Me.txt_AnzKopienESVertrag.Size = New System.Drawing.Size(75, 20)
    Me.txt_AnzKopienESVertrag.TabIndex = 0
    '
    'frmESPrintSetting
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(1121, 460)
    Me.Controls.Add(Me.xtabSetting)
    Me.DoubleBuffered = True
    Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Name = "frmESPrintSetting"
    Me.Text = "Einstellungen für Druck und Export der Einsatzverträge"
    CType(Me.xtabSetting, System.ComponentModel.ISupportInitialize).EndInit()
    Me.xtabSetting.ResumeLayout(False)
    Me.xtabPrintSetting.ResumeLayout(False)
    CType(Me.grpEinstellung, System.ComponentModel.ISupportInitialize).EndInit()
    Me.grpEinstellung.ResumeLayout(False)
    CType(Me.grpUnterzeichner, System.ComponentModel.ISupportInitialize).EndInit()
    Me.grpUnterzeichner.ResumeLayout(False)
    CType(Me.chkUnterzeichner.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.grpExport, System.ComponentModel.ISupportInitialize).EndInit()
    Me.grpExport.ResumeLayout(False)
    CType(Me.txt_ExportFinalFileVerleih.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txt_ExportFileVerleihVertrag.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.cbo_ExportPfad.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txt_ExportFinalFileESVertrag.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txt_ExportFileESVertrag.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.grpAnzahl, System.ComponentModel.ISupportInitialize).EndInit()
    Me.grpAnzahl.ResumeLayout(False)
    CType(Me.txt_AnzKopienVerleih.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txt_AnzKopienESVertrag.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents xtabSetting As DevExpress.XtraTab.XtraTabControl
  Friend WithEvents xtabPrintSetting As DevExpress.XtraTab.XtraTabPage
  Friend WithEvents btnSaveTextValues As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents grpEinstellung As DevExpress.XtraEditors.GroupControl
  Friend WithEvents grpAnzahl As DevExpress.XtraEditors.GroupControl
  Friend WithEvents lblEinsatzvertrag As DevExpress.XtraEditors.LabelControl
  Friend WithEvents txt_ExportFileESVertrag As DevExpress.XtraEditors.TextEdit
  Friend WithEvents lblExportEinsatzvertrag As DevExpress.XtraEditors.LabelControl
  Friend WithEvents lblDateipfad As DevExpress.XtraEditors.LabelControl
  Friend WithEvents lblESVertragDatei As DevExpress.XtraEditors.LabelControl
  Friend WithEvents txt_ExportFinalFileESVertrag As DevExpress.XtraEditors.TextEdit
  Friend WithEvents lblAnmerkung2 As DevExpress.XtraEditors.LabelControl
  Friend WithEvents lblAnmerkung1 As DevExpress.XtraEditors.LabelControl
  Friend WithEvents cbo_ExportPfad As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents grpExport As DevExpress.XtraEditors.GroupControl
  Friend WithEvents txt_AnzKopienESVertrag As DevExpress.XtraEditors.SpinEdit
  Friend WithEvents txt_ExportFileVerleihVertrag As DevExpress.XtraEditors.TextEdit
  Friend WithEvents lblExportVerleih As DevExpress.XtraEditors.LabelControl
  Friend WithEvents lblDateizusammenfuegen As DevExpress.XtraEditors.LabelControl
  Friend WithEvents txt_ExportFinalFileVerleih As DevExpress.XtraEditors.TextEdit
  Friend WithEvents lblVerleihvertragDatei As DevExpress.XtraEditors.LabelControl
  Friend WithEvents lblVerleihvertrag As DevExpress.XtraEditors.LabelControl
  Friend WithEvents txt_AnzKopienVerleih As DevExpress.XtraEditors.SpinEdit
  Friend WithEvents grpUnterzeichner As DevExpress.XtraEditors.GroupControl
  Friend WithEvents chkUnterzeichner As DevExpress.XtraEditors.CheckEdit
  Friend WithEvents lblAktiviert As DevExpress.XtraEditors.LabelControl
End Class
