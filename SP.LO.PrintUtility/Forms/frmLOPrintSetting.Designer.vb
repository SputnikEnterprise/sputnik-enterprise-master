<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLOPrintSetting
	Inherits DevExpress.XtraEditors.XtraForm

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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLOPrintSetting))
		Me.XtraTabControl1 = New DevExpress.XtraTab.XtraTabControl()
		Me.xtabMain = New DevExpress.XtraTab.XtraTabPage()
		Me.btnSaveTextValues = New DevExpress.XtraEditors.SimpleButton()
		Me.grpEinstellungen = New DevExpress.XtraEditors.GroupControl()
		Me.grpExport = New DevExpress.XtraEditors.GroupControl()
		Me.lblAnmerkung2 = New DevExpress.XtraEditors.LabelControl()
		Me.cbo_ExportPfad = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.lblAnmerkung1 = New DevExpress.XtraEditors.LabelControl()
		Me.txt_ExportFinalFile = New DevExpress.XtraEditors.TextEdit()
		Me.txt_ExportFile = New DevExpress.XtraEditors.TextEdit()
		Me.lblDateienzusammenfassung = New DevExpress.XtraEditors.LabelControl()
		Me.lblExportDateiname = New DevExpress.XtraEditors.LabelControl()
		Me.lblExportpfad = New DevExpress.XtraEditors.LabelControl()
		Me.grpDruckLohnabrechnung = New DevExpress.XtraEditors.GroupControl()
		Me.lblfuerallevorlagen = New DevExpress.XtraEditors.LabelControl()
		Me.lblAnzahlkopien = New DevExpress.XtraEditors.LabelControl()
		Me.txt_AnzKopien = New DevExpress.XtraEditors.SpinEdit()
		CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.XtraTabControl1.SuspendLayout()
		Me.xtabMain.SuspendLayout()
		CType(Me.grpEinstellungen, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grpEinstellungen.SuspendLayout()
		CType(Me.grpExport, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grpExport.SuspendLayout()
		CType(Me.cbo_ExportPfad.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txt_ExportFinalFile.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txt_ExportFile.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.grpDruckLohnabrechnung, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grpDruckLohnabrechnung.SuspendLayout()
		CType(Me.txt_AnzKopien.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'XtraTabControl1
		'
		Me.XtraTabControl1.Appearance.BackColor = System.Drawing.Color.White
		Me.XtraTabControl1.Appearance.ForeColor = System.Drawing.Color.Black
		Me.XtraTabControl1.Appearance.Options.UseBackColor = True
		Me.XtraTabControl1.Appearance.Options.UseForeColor = True
		Me.XtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.XtraTabControl1.Location = New System.Drawing.Point(0, 0)
		Me.XtraTabControl1.Name = "XtraTabControl1"
		Me.XtraTabControl1.SelectedTabPage = Me.xtabMain
		Me.XtraTabControl1.Size = New System.Drawing.Size(797, 411)
		Me.XtraTabControl1.TabIndex = 2
		Me.XtraTabControl1.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabMain})
		'
		'xtabMain
		'
		Me.xtabMain.Appearance.PageClient.ForeColor = System.Drawing.Color.Black
		Me.xtabMain.Appearance.PageClient.Options.UseForeColor = True
		Me.xtabMain.Controls.Add(Me.btnSaveTextValues)
		Me.xtabMain.Controls.Add(Me.grpEinstellungen)
		Me.xtabMain.Name = "xtabMain"
		Me.xtabMain.Size = New System.Drawing.Size(791, 383)
		Me.xtabMain.Text = "Druck und Export der Lohnabrechnungen"
		'
		'btnSaveTextValues
		'
		Me.btnSaveTextValues.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnSaveTextValues.Appearance.ForeColor = System.Drawing.Color.Black
		Me.btnSaveTextValues.Appearance.Options.UseForeColor = True
		Me.btnSaveTextValues.Location = New System.Drawing.Point(668, 50)
		Me.btnSaveTextValues.Name = "btnSaveTextValues"
		Me.btnSaveTextValues.Size = New System.Drawing.Size(104, 26)
		Me.btnSaveTextValues.TabIndex = 0
		Me.btnSaveTextValues.Text = "Speichern"
		'
		'grpEinstellungen
		'
		Me.grpEinstellungen.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.grpEinstellungen.Appearance.BackColor = System.Drawing.Color.White
		Me.grpEinstellungen.Appearance.ForeColor = System.Drawing.Color.Black
		Me.grpEinstellungen.Appearance.Options.UseBackColor = True
		Me.grpEinstellungen.Appearance.Options.UseForeColor = True
		Me.grpEinstellungen.Controls.Add(Me.grpExport)
		Me.grpEinstellungen.Controls.Add(Me.grpDruckLohnabrechnung)
		Me.grpEinstellungen.Location = New System.Drawing.Point(11, 15)
		Me.grpEinstellungen.Name = "grpEinstellungen"
		Me.grpEinstellungen.Size = New System.Drawing.Size(639, 358)
		Me.grpEinstellungen.TabIndex = 0
		Me.grpEinstellungen.Text = "Einstellungen"
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
		Me.grpExport.Controls.Add(Me.lblAnmerkung2)
		Me.grpExport.Controls.Add(Me.cbo_ExportPfad)
		Me.grpExport.Controls.Add(Me.lblAnmerkung1)
		Me.grpExport.Controls.Add(Me.txt_ExportFinalFile)
		Me.grpExport.Controls.Add(Me.txt_ExportFile)
		Me.grpExport.Controls.Add(Me.lblDateienzusammenfassung)
		Me.grpExport.Controls.Add(Me.lblExportDateiname)
		Me.grpExport.Controls.Add(Me.lblExportpfad)
		Me.grpExport.Location = New System.Drawing.Point(16, 118)
		Me.grpExport.Name = "grpExport"
		Me.grpExport.Size = New System.Drawing.Size(602, 226)
		Me.grpExport.TabIndex = 24
		Me.grpExport.Text = "Export der Lohnabrechnungen"
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
		":</b> Monat" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "<b>{1}:</b> Jahr" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Beispiel: FinalLo_<b>01</b>_<b>2013</b>.PDF"
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
		Me.cbo_ExportPfad.Size = New System.Drawing.Size(368, 20)
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
		" Lohnabrechnungsnummer" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "<b>{1}:</b> Kandidatennummer" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Beispiel: Lo_<b>2252</b>_<" & _
		"b>21</b>.PDF"
		'
		'txt_ExportFinalFile
		'
		Me.txt_ExportFinalFile.Location = New System.Drawing.Point(218, 144)
		Me.txt_ExportFinalFile.Name = "txt_ExportFinalFile"
		Me.txt_ExportFinalFile.Properties.Appearance.BackColor = System.Drawing.Color.White
		Me.txt_ExportFinalFile.Properties.Appearance.ForeColor = System.Drawing.Color.Black
		Me.txt_ExportFinalFile.Properties.Appearance.Options.UseBackColor = True
		Me.txt_ExportFinalFile.Properties.Appearance.Options.UseForeColor = True
		Me.txt_ExportFinalFile.Size = New System.Drawing.Size(164, 20)
		Me.txt_ExportFinalFile.TabIndex = 2
		'
		'txt_ExportFile
		'
		Me.txt_ExportFile.Location = New System.Drawing.Point(218, 65)
		Me.txt_ExportFile.Name = "txt_ExportFile"
		Me.txt_ExportFile.Properties.Appearance.BackColor = System.Drawing.Color.White
		Me.txt_ExportFile.Properties.Appearance.ForeColor = System.Drawing.Color.Black
		Me.txt_ExportFile.Properties.Appearance.Options.UseBackColor = True
		Me.txt_ExportFile.Properties.Appearance.Options.UseForeColor = True
		Me.txt_ExportFile.Size = New System.Drawing.Size(164, 20)
		Me.txt_ExportFile.TabIndex = 1
		'
		'lblDateienzusammenfassung
		'
		Me.lblDateienzusammenfassung.AllowHtmlString = True
		Me.lblDateienzusammenfassung.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.lblDateienzusammenfassung.Appearance.ForeColor = System.Drawing.Color.Black
		Me.lblDateienzusammenfassung.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblDateienzusammenfassung.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblDateienzusammenfassung.Location = New System.Drawing.Point(12, 148)
		Me.lblDateienzusammenfassung.Name = "lblDateienzusammenfassung"
		Me.lblDateienzusammenfassung.Size = New System.Drawing.Size(200, 13)
		Me.lblDateienzusammenfassung.TabIndex = 7
		Me.lblDateienzusammenfassung.Text = "Dateien zusammenführen als"
		'
		'lblExportDateiname
		'
		Me.lblExportDateiname.AllowHtmlString = True
		Me.lblExportDateiname.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.lblExportDateiname.Appearance.ForeColor = System.Drawing.Color.Black
		Me.lblExportDateiname.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblExportDateiname.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblExportDateiname.Location = New System.Drawing.Point(12, 69)
		Me.lblExportDateiname.Name = "lblExportDateiname"
		Me.lblExportDateiname.Size = New System.Drawing.Size(200, 13)
		Me.lblExportDateiname.TabIndex = 3
		Me.lblExportDateiname.Text = "Export-Dateiname"
		'
		'lblExportpfad
		'
		Me.lblExportpfad.AllowHtmlString = True
		Me.lblExportpfad.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.lblExportpfad.Appearance.ForeColor = System.Drawing.Color.Black
		Me.lblExportpfad.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblExportpfad.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblExportpfad.Location = New System.Drawing.Point(12, 43)
		Me.lblExportpfad.Name = "lblExportpfad"
		Me.lblExportpfad.Size = New System.Drawing.Size(200, 13)
		Me.lblExportpfad.TabIndex = 5
		Me.lblExportpfad.Text = "Export-Dateipfad"
		'
		'grpDruckLohnabrechnung
		'
		Me.grpDruckLohnabrechnung.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.grpDruckLohnabrechnung.Appearance.BackColor = System.Drawing.Color.White
		Me.grpDruckLohnabrechnung.Appearance.ForeColor = System.Drawing.Color.Black
		Me.grpDruckLohnabrechnung.Appearance.Options.UseBackColor = True
		Me.grpDruckLohnabrechnung.Appearance.Options.UseForeColor = True
		Me.grpDruckLohnabrechnung.Controls.Add(Me.lblfuerallevorlagen)
		Me.grpDruckLohnabrechnung.Controls.Add(Me.lblAnzahlkopien)
		Me.grpDruckLohnabrechnung.Controls.Add(Me.txt_AnzKopien)
		Me.grpDruckLohnabrechnung.Location = New System.Drawing.Point(16, 34)
		Me.grpDruckLohnabrechnung.Name = "grpDruckLohnabrechnung"
		Me.grpDruckLohnabrechnung.Size = New System.Drawing.Size(602, 63)
		Me.grpDruckLohnabrechnung.TabIndex = 23
		Me.grpDruckLohnabrechnung.Text = "Druck der Lohnabrechnungen"
		'
		'lblfuerallevorlagen
		'
		Me.lblfuerallevorlagen.AllowHtmlString = True
		Me.lblfuerallevorlagen.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.lblfuerallevorlagen.Appearance.ForeColor = System.Drawing.Color.Black
		Me.lblfuerallevorlagen.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblfuerallevorlagen.Location = New System.Drawing.Point(299, 35)
		Me.lblfuerallevorlagen.Name = "lblfuerallevorlagen"
		Me.lblfuerallevorlagen.Size = New System.Drawing.Size(294, 13)
		Me.lblfuerallevorlagen.TabIndex = 2
		Me.lblfuerallevorlagen.Text = "(Für alle Vorlagen!)"
		'
		'lblAnzahlkopien
		'
		Me.lblAnzahlkopien.AllowHtmlString = True
		Me.lblAnzahlkopien.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.lblAnzahlkopien.Appearance.ForeColor = System.Drawing.Color.Black
		Me.lblAnzahlkopien.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblAnzahlkopien.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblAnzahlkopien.Location = New System.Drawing.Point(12, 35)
		Me.lblAnzahlkopien.Name = "lblAnzahlkopien"
		Me.lblAnzahlkopien.Size = New System.Drawing.Size(200, 13)
		Me.lblAnzahlkopien.TabIndex = 1
		Me.lblAnzahlkopien.Text = "Anzahl Kopien"
		'
		'txt_AnzKopien
		'
		Me.txt_AnzKopien.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
		Me.txt_AnzKopien.Location = New System.Drawing.Point(218, 31)
		Me.txt_AnzKopien.Name = "txt_AnzKopien"
		Me.txt_AnzKopien.Properties.Appearance.BackColor = System.Drawing.Color.White
		Me.txt_AnzKopien.Properties.Appearance.ForeColor = System.Drawing.Color.Black
		Me.txt_AnzKopien.Properties.Appearance.Options.UseBackColor = True
		Me.txt_AnzKopien.Properties.Appearance.Options.UseForeColor = True
		Me.txt_AnzKopien.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.txt_AnzKopien.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.[Default]
		Me.txt_AnzKopien.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None
		Me.txt_AnzKopien.Size = New System.Drawing.Size(75, 20)
		Me.txt_AnzKopien.TabIndex = 0
		'
		'frmLOPrintSetting
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(797, 411)
		Me.Controls.Add(Me.XtraTabControl1)
		Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.Name = "frmLOPrintSetting"
		Me.Text = "Einstellungen für Druck und Export der Lohnabrechnungen"
		CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.XtraTabControl1.ResumeLayout(False)
		Me.xtabMain.ResumeLayout(False)
		CType(Me.grpEinstellungen, System.ComponentModel.ISupportInitialize).EndInit()
		Me.grpEinstellungen.ResumeLayout(False)
		CType(Me.grpExport, System.ComponentModel.ISupportInitialize).EndInit()
		Me.grpExport.ResumeLayout(False)
		CType(Me.cbo_ExportPfad.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txt_ExportFinalFile.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txt_ExportFile.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.grpDruckLohnabrechnung, System.ComponentModel.ISupportInitialize).EndInit()
		Me.grpDruckLohnabrechnung.ResumeLayout(False)
		CType(Me.txt_AnzKopien.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents XtraTabControl1 As DevExpress.XtraTab.XtraTabControl
	Friend WithEvents xtabMain As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents btnSaveTextValues As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents grpEinstellungen As DevExpress.XtraEditors.GroupControl
	Friend WithEvents grpDruckLohnabrechnung As DevExpress.XtraEditors.GroupControl
	Friend WithEvents lblAnzahlkopien As DevExpress.XtraEditors.LabelControl
	Friend WithEvents txt_ExportFile As DevExpress.XtraEditors.TextEdit
	Friend WithEvents lblExportDateiname As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblExportpfad As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblDateienzusammenfassung As DevExpress.XtraEditors.LabelControl
	Friend WithEvents txt_ExportFinalFile As DevExpress.XtraEditors.TextEdit
	Friend WithEvents lblAnmerkung2 As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblAnmerkung1 As DevExpress.XtraEditors.LabelControl
	Friend WithEvents cbo_ExportPfad As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents grpExport As DevExpress.XtraEditors.GroupControl
	Friend WithEvents txt_AnzKopien As DevExpress.XtraEditors.SpinEdit
	Friend WithEvents lblfuerallevorlagen As DevExpress.XtraEditors.LabelControl
End Class
