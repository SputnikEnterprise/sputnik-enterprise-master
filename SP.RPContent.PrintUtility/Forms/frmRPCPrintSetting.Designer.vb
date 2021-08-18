<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRPCPrintSetting
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRPCPrintSetting))
		Me.XtraTabControl1 = New DevExpress.XtraTab.XtraTabControl()
		Me.xtabSetting = New DevExpress.XtraTab.XtraTabPage()
		Me.btnSaveTextValues = New DevExpress.XtraEditors.SimpleButton()
		Me.grpEinstellung = New DevExpress.XtraEditors.GroupControl()
		Me.chkPrintCustomerData = New DevExpress.XtraEditors.CheckEdit()
		Me.lblExportDateipfad = New DevExpress.XtraEditors.LabelControl()
		Me.lblExportDateiname = New DevExpress.XtraEditors.LabelControl()
		Me.lblDateizusammen = New DevExpress.XtraEditors.LabelControl()
		Me.txt_ExportFile = New DevExpress.XtraEditors.TextEdit()
		Me.lblAnmerkung2 = New DevExpress.XtraEditors.LabelControl()
		Me.txt_ExportFinalFile = New DevExpress.XtraEditors.TextEdit()
		Me.cbo_ExportPfad = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.lblAnmerkung1 = New DevExpress.XtraEditors.LabelControl()
		CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.XtraTabControl1.SuspendLayout()
		Me.xtabSetting.SuspendLayout()
		CType(Me.grpEinstellung, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grpEinstellung.SuspendLayout()
		CType(Me.chkPrintCustomerData.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txt_ExportFile.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txt_ExportFinalFile.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.cbo_ExportPfad.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'XtraTabControl1
		'
		Me.XtraTabControl1.Appearance.BackColor = System.Drawing.Color.White
		Me.XtraTabControl1.Appearance.ForeColor = System.Drawing.Color.Black
		Me.XtraTabControl1.Appearance.Options.UseBackColor = True
		Me.XtraTabControl1.Appearance.Options.UseForeColor = True
		Me.XtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.XtraTabControl1.Location = New System.Drawing.Point(5, 5)
		Me.XtraTabControl1.Name = "XtraTabControl1"
		Me.XtraTabControl1.SelectedTabPage = Me.xtabSetting
		Me.XtraTabControl1.Size = New System.Drawing.Size(787, 318)
		Me.XtraTabControl1.TabIndex = 2
		Me.XtraTabControl1.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabSetting})
		'
		'xtabSetting
		'
		Me.xtabSetting.Appearance.PageClient.ForeColor = System.Drawing.Color.Black
		Me.xtabSetting.Appearance.PageClient.Options.UseForeColor = True
		Me.xtabSetting.Controls.Add(Me.btnSaveTextValues)
		Me.xtabSetting.Controls.Add(Me.grpEinstellung)
		Me.xtabSetting.Name = "xtabSetting"
		Me.xtabSetting.Size = New System.Drawing.Size(781, 290)
		Me.xtabSetting.Text = "Druck und Export der Rapportdaten"
		'
		'btnSaveTextValues
		'
		Me.btnSaveTextValues.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnSaveTextValues.Appearance.ForeColor = System.Drawing.Color.Black
		Me.btnSaveTextValues.Appearance.Options.UseForeColor = True
		Me.btnSaveTextValues.Location = New System.Drawing.Point(658, 50)
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
		Me.grpEinstellung.Controls.Add(Me.chkPrintCustomerData)
		Me.grpEinstellung.Controls.Add(Me.lblExportDateipfad)
		Me.grpEinstellung.Controls.Add(Me.lblExportDateiname)
		Me.grpEinstellung.Controls.Add(Me.lblDateizusammen)
		Me.grpEinstellung.Controls.Add(Me.txt_ExportFile)
		Me.grpEinstellung.Controls.Add(Me.lblAnmerkung2)
		Me.grpEinstellung.Controls.Add(Me.txt_ExportFinalFile)
		Me.grpEinstellung.Controls.Add(Me.cbo_ExportPfad)
		Me.grpEinstellung.Controls.Add(Me.lblAnmerkung1)
		Me.grpEinstellung.Location = New System.Drawing.Point(11, 15)
		Me.grpEinstellung.Name = "grpEinstellung"
		Me.grpEinstellung.Size = New System.Drawing.Size(629, 260)
		Me.grpEinstellung.TabIndex = 0
		Me.grpEinstellung.Text = "Einstellungen"
		'
		'chkPrintCustomerData
		'
		Me.chkPrintCustomerData.EditValue = True
		Me.chkPrintCustomerData.Location = New System.Drawing.Point(231, 42)
		Me.chkPrintCustomerData.Name = "chkPrintCustomerData"
		Me.chkPrintCustomerData.Properties.Caption = "Kundendaten ausdrucken"
		Me.chkPrintCustomerData.Size = New System.Drawing.Size(280, 19)
		Me.chkPrintCustomerData.TabIndex = 286
		'
		'lblExportDateipfad
		'
		Me.lblExportDateipfad.AllowHtmlString = True
		Me.lblExportDateipfad.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.lblExportDateipfad.Appearance.ForeColor = System.Drawing.Color.Black
		Me.lblExportDateipfad.Appearance.Options.UseBackColor = True
		Me.lblExportDateipfad.Appearance.Options.UseForeColor = True
		Me.lblExportDateipfad.Appearance.Options.UseTextOptions = True
		Me.lblExportDateipfad.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblExportDateipfad.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblExportDateipfad.Location = New System.Drawing.Point(25, 71)
		Me.lblExportDateipfad.Name = "lblExportDateipfad"
		Me.lblExportDateipfad.Size = New System.Drawing.Size(200, 13)
		Me.lblExportDateipfad.TabIndex = 5
		Me.lblExportDateipfad.Text = "Export-Dateipfad"
		'
		'lblExportDateiname
		'
		Me.lblExportDateiname.AllowHtmlString = True
		Me.lblExportDateiname.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.lblExportDateiname.Appearance.ForeColor = System.Drawing.Color.Black
		Me.lblExportDateiname.Appearance.Options.UseBackColor = True
		Me.lblExportDateiname.Appearance.Options.UseForeColor = True
		Me.lblExportDateiname.Appearance.Options.UseTextOptions = True
		Me.lblExportDateiname.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblExportDateiname.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblExportDateiname.Location = New System.Drawing.Point(25, 97)
		Me.lblExportDateiname.Name = "lblExportDateiname"
		Me.lblExportDateiname.Size = New System.Drawing.Size(200, 13)
		Me.lblExportDateiname.TabIndex = 3
		Me.lblExportDateiname.Text = "Export-Dateiname"
		'
		'lblDateizusammen
		'
		Me.lblDateizusammen.AllowHtmlString = True
		Me.lblDateizusammen.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.lblDateizusammen.Appearance.ForeColor = System.Drawing.Color.Black
		Me.lblDateizusammen.Appearance.Options.UseBackColor = True
		Me.lblDateizusammen.Appearance.Options.UseForeColor = True
		Me.lblDateizusammen.Appearance.Options.UseTextOptions = True
		Me.lblDateizusammen.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblDateizusammen.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblDateizusammen.Location = New System.Drawing.Point(25, 176)
		Me.lblDateizusammen.Name = "lblDateizusammen"
		Me.lblDateizusammen.Size = New System.Drawing.Size(200, 13)
		Me.lblDateizusammen.TabIndex = 7
		Me.lblDateizusammen.Text = "Dateien zusammenführen als"
		'
		'txt_ExportFile
		'
		Me.txt_ExportFile.Location = New System.Drawing.Point(231, 93)
		Me.txt_ExportFile.Name = "txt_ExportFile"
		Me.txt_ExportFile.Properties.Appearance.BackColor = System.Drawing.Color.White
		Me.txt_ExportFile.Properties.Appearance.ForeColor = System.Drawing.Color.Black
		Me.txt_ExportFile.Properties.Appearance.Options.UseBackColor = True
		Me.txt_ExportFile.Properties.Appearance.Options.UseForeColor = True
		Me.txt_ExportFile.Size = New System.Drawing.Size(164, 20)
		Me.txt_ExportFile.TabIndex = 1
		'
		'lblAnmerkung2
		'
		Me.lblAnmerkung2.AllowHtmlString = True
		Me.lblAnmerkung2.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.lblAnmerkung2.Appearance.ForeColor = System.Drawing.Color.Black
		Me.lblAnmerkung2.Appearance.Options.UseBackColor = True
		Me.lblAnmerkung2.Appearance.Options.UseForeColor = True
		Me.lblAnmerkung2.Appearance.Options.UseTextOptions = True
		Me.lblAnmerkung2.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
		Me.lblAnmerkung2.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
		Me.lblAnmerkung2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblAnmerkung2.LineColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
		Me.lblAnmerkung2.LineLocation = DevExpress.XtraEditors.LineLocation.Left
		Me.lblAnmerkung2.LineVisible = True
		Me.lblAnmerkung2.Location = New System.Drawing.Point(401, 172)
		Me.lblAnmerkung2.Name = "lblAnmerkung2"
		Me.lblAnmerkung2.Size = New System.Drawing.Size(222, 73)
		Me.lblAnmerkung2.TabIndex = 9
		Me.lblAnmerkung2.Text = "<b>Anmerkung:</b>" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "FinalDateiname_<b><i>{0}</i></b></b>.PDF" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "<b>{0}:</b> Datum+Uh" &
		"rzeit" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Beispiel: FinalRPC_<b>01012013105010</b>.PDF"
		'
		'txt_ExportFinalFile
		'
		Me.txt_ExportFinalFile.Location = New System.Drawing.Point(231, 172)
		Me.txt_ExportFinalFile.Name = "txt_ExportFinalFile"
		Me.txt_ExportFinalFile.Properties.Appearance.BackColor = System.Drawing.Color.White
		Me.txt_ExportFinalFile.Properties.Appearance.ForeColor = System.Drawing.Color.Black
		Me.txt_ExportFinalFile.Properties.Appearance.Options.UseBackColor = True
		Me.txt_ExportFinalFile.Properties.Appearance.Options.UseForeColor = True
		Me.txt_ExportFinalFile.Size = New System.Drawing.Size(164, 20)
		Me.txt_ExportFinalFile.TabIndex = 3
		'
		'cbo_ExportPfad
		'
		Me.cbo_ExportPfad.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.cbo_ExportPfad.Location = New System.Drawing.Point(231, 67)
		Me.cbo_ExportPfad.Name = "cbo_ExportPfad"
		Me.cbo_ExportPfad.Properties.Appearance.BackColor = System.Drawing.Color.White
		Me.cbo_ExportPfad.Properties.Appearance.ForeColor = System.Drawing.Color.Black
		Me.cbo_ExportPfad.Properties.Appearance.Options.UseBackColor = True
		Me.cbo_ExportPfad.Properties.Appearance.Options.UseForeColor = True
		Me.cbo_ExportPfad.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.cbo_ExportPfad.Size = New System.Drawing.Size(358, 20)
		Me.cbo_ExportPfad.TabIndex = 0
		'
		'lblAnmerkung1
		'
		Me.lblAnmerkung1.AllowHtmlString = True
		Me.lblAnmerkung1.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.lblAnmerkung1.Appearance.ForeColor = System.Drawing.Color.Black
		Me.lblAnmerkung1.Appearance.Options.UseBackColor = True
		Me.lblAnmerkung1.Appearance.Options.UseForeColor = True
		Me.lblAnmerkung1.Appearance.Options.UseTextOptions = True
		Me.lblAnmerkung1.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
		Me.lblAnmerkung1.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
		Me.lblAnmerkung1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblAnmerkung1.LineColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
		Me.lblAnmerkung1.LineLocation = DevExpress.XtraEditors.LineLocation.Left
		Me.lblAnmerkung1.LineVisible = True
		Me.lblAnmerkung1.Location = New System.Drawing.Point(401, 93)
		Me.lblAnmerkung1.Name = "lblAnmerkung1"
		Me.lblAnmerkung1.Size = New System.Drawing.Size(222, 73)
		Me.lblAnmerkung1.TabIndex = 8
		Me.lblAnmerkung1.Text = "<b>Anmerkung:</b>" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Dateiname_<b><i>{0}</i></b>.PDF" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "<b>{0}:</b> Rapportnummer" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Be" &
		"ispiel: RPC_<b>2252</b>.PDF"
		'
		'frmRPCPrintSetting
		'
		Me.Appearance.Options.UseFont = True
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(797, 328)
		Me.Controls.Add(Me.XtraTabControl1)
		Me.DoubleBuffered = True
		Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.Name = "frmRPCPrintSetting"
		Me.Padding = New System.Windows.Forms.Padding(5)
		Me.Text = "Einstellungen für Druck und Export der Rapportdaten"
		CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.XtraTabControl1.ResumeLayout(False)
		Me.xtabSetting.ResumeLayout(False)
		CType(Me.grpEinstellung, System.ComponentModel.ISupportInitialize).EndInit()
		Me.grpEinstellung.ResumeLayout(False)
		CType(Me.chkPrintCustomerData.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txt_ExportFile.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txt_ExportFinalFile.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.cbo_ExportPfad.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents XtraTabControl1 As DevExpress.XtraTab.XtraTabControl
  Friend WithEvents xtabSetting As DevExpress.XtraTab.XtraTabPage
  Friend WithEvents btnSaveTextValues As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents grpEinstellung As DevExpress.XtraEditors.GroupControl
  Friend WithEvents txt_ExportFile As DevExpress.XtraEditors.TextEdit
  Friend WithEvents lblExportDateiname As DevExpress.XtraEditors.LabelControl
  Friend WithEvents lblExportDateipfad As DevExpress.XtraEditors.LabelControl
  Friend WithEvents lblDateizusammen As DevExpress.XtraEditors.LabelControl
  Friend WithEvents txt_ExportFinalFile As DevExpress.XtraEditors.TextEdit
  Friend WithEvents lblAnmerkung2 As DevExpress.XtraEditors.LabelControl
  Friend WithEvents lblAnmerkung1 As DevExpress.XtraEditors.LabelControl
  Friend WithEvents cbo_ExportPfad As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents chkPrintCustomerData As DevExpress.XtraEditors.CheckEdit
End Class
