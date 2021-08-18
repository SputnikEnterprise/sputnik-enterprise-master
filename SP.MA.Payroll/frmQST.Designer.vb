<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmQST
    Inherits DevExpress.XtraEditors.XtraForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmQST))
		Me.Panel1 = New System.Windows.Forms.Panel()
		Me.chkDoNOTShowForm = New DevExpress.XtraEditors.CheckEdit()
		Me.stdPanel = New System.Windows.Forms.Panel()
		Me.lblMonatsbrutto = New System.Windows.Forms.Label()
		Me.lblMonatsStunden = New System.Windows.Forms.Label()
		Me.lblMonatsBruttoValue = New System.Windows.Forms.Label()
		Me.lblTag = New System.Windows.Forms.Label()
		Me.lblBerechnetFuer = New System.Windows.Forms.Label()
		Me.lblMonatsstudenVon = New System.Windows.Forms.Label()
		Me.lblMANameValue = New System.Windows.Forms.Label()
		Me.lblMAName = New System.Windows.Forms.Label()
		Me.lblMANrValue = New System.Windows.Forms.Label()
		Me.lblMANr = New System.Windows.Forms.Label()
		Me.lblAngabenQSTAbzug = New System.Windows.Forms.Label()
		Me.lblListeES = New System.Windows.Forms.Label()
		Me.grpLohndaten = New System.Windows.Forms.GroupBox()
		Me.grdTaxData = New DevExpress.XtraGrid.GridControl()
		Me.gvTaxData = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.btnAdopt = New DevExpress.XtraEditors.SimpleButton()
		Me.lblMindestAbzugValue = New DevExpress.XtraEditors.LabelControl()
		Me.lblMindestabzug = New DevExpress.XtraEditors.LabelControl()
		Me.lblAbzugWaehrung = New System.Windows.Forms.Label()
		Me.txtAbzug = New DevExpress.XtraEditors.TextEdit()
		Me.lblAbzug = New System.Windows.Forms.Label()
		Me.lblAnsatzProz = New System.Windows.Forms.Label()
		Me.lblAnsatz = New System.Windows.Forms.Label()
		Me.lblQSTBasisInfo = New System.Windows.Forms.Label()
		Me.lblQSTWaehrung = New System.Windows.Forms.Label()
		Me.txtQstBasis = New DevExpress.XtraEditors.TextEdit()
		Me.lblQSTBasis = New System.Windows.Forms.Label()
		Me.lblTarifInfo = New System.Windows.Forms.Label()
		Me.txtTarif = New DevExpress.XtraEditors.TextEdit()
		Me.lblTarif = New System.Windows.Forms.Label()
		Me.lblSteuerkanton = New System.Windows.Forms.Label()
		Me.txtSteuerKanton = New DevExpress.XtraEditors.TextEdit()
		Me.txtAnsatz = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.grpPersonalien = New System.Windows.Forms.GroupBox()
		Me.grdPersonalien = New DevExpress.XtraGrid.GridControl()
		Me.gvPersonalien = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.txtEntryAndExit = New DevExpress.XtraEditors.TextEdit()
		Me.grdListOfES = New DevExpress.XtraGrid.GridControl()
		Me.gvListOfES = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.lblMindestAbzugInfo = New DevExpress.XtraEditors.LabelControl()
		Me.Panel1.SuspendLayout()
		CType(Me.chkDoNOTShowForm.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.stdPanel.SuspendLayout()
		Me.grpLohndaten.SuspendLayout()
		CType(Me.grdTaxData, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvTaxData, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtAbzug.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtQstBasis.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtTarif.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtSteuerKanton.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtAnsatz.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grpPersonalien.SuspendLayout()
		CType(Me.grdPersonalien, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvPersonalien, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtEntryAndExit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.grdListOfES, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvListOfES, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'Panel1
		'
		Me.Panel1.Controls.Add(Me.chkDoNOTShowForm)
		Me.Panel1.Controls.Add(Me.stdPanel)
		Me.Panel1.Controls.Add(Me.lblMANameValue)
		Me.Panel1.Controls.Add(Me.lblMAName)
		Me.Panel1.Controls.Add(Me.lblMANrValue)
		Me.Panel1.Controls.Add(Me.lblMANr)
		Me.Panel1.Controls.Add(Me.lblAngabenQSTAbzug)
		Me.Panel1.Location = New System.Drawing.Point(1, 1)
		Me.Panel1.Name = "Panel1"
		Me.Panel1.Size = New System.Drawing.Size(664, 100)
		Me.Panel1.TabIndex = 12
		'
		'chkDoNOTShowForm
		'
		Me.chkDoNOTShowForm.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.chkDoNOTShowForm.Location = New System.Drawing.Point(12, 69)
		Me.chkDoNOTShowForm.Name = "chkDoNOTShowForm"
		Me.chkDoNOTShowForm.Properties.AllowFocused = False
		Me.chkDoNOTShowForm.Properties.Appearance.Options.UseTextOptions = True
		Me.chkDoNOTShowForm.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
		Me.chkDoNOTShowForm.Properties.Caption = "Dieses Fenster nicht mehr anzeigen."
		Me.chkDoNOTShowForm.Size = New System.Drawing.Size(384, 20)
		Me.chkDoNOTShowForm.TabIndex = 304
		'
		'stdPanel
		'
		Me.stdPanel.Controls.Add(Me.lblMonatsbrutto)
		Me.stdPanel.Controls.Add(Me.lblMonatsStunden)
		Me.stdPanel.Controls.Add(Me.lblMonatsBruttoValue)
		Me.stdPanel.Controls.Add(Me.lblTag)
		Me.stdPanel.Controls.Add(Me.lblBerechnetFuer)
		Me.stdPanel.Controls.Add(Me.lblMonatsstudenVon)
		Me.stdPanel.Location = New System.Drawing.Point(415, 7)
		Me.stdPanel.Name = "stdPanel"
		Me.stdPanel.Size = New System.Drawing.Size(243, 73)
		Me.stdPanel.TabIndex = 11
		'
		'lblMonatsbrutto
		'
		Me.lblMonatsbrutto.AutoSize = True
		Me.lblMonatsbrutto.Location = New System.Drawing.Point(7, 29)
		Me.lblMonatsbrutto.Name = "lblMonatsbrutto"
		Me.lblMonatsbrutto.Size = New System.Drawing.Size(72, 13)
		Me.lblMonatsbrutto.TabIndex = 5
		Me.lblMonatsbrutto.Text = "Monatsbrutto"
		'
		'lblMonatsStunden
		'
		Me.lblMonatsStunden.AutoSize = True
		Me.lblMonatsStunden.Location = New System.Drawing.Point(193, 46)
		Me.lblMonatsStunden.Name = "lblMonatsStunden"
		Me.lblMonatsStunden.Size = New System.Drawing.Size(25, 13)
		Me.lblMonatsStunden.TabIndex = 10
		Me.lblMonatsStunden.Text = "180"
		'
		'lblMonatsBruttoValue
		'
		Me.lblMonatsBruttoValue.AutoSize = True
		Me.lblMonatsBruttoValue.Location = New System.Drawing.Point(85, 29)
		Me.lblMonatsBruttoValue.Name = "lblMonatsBruttoValue"
		Me.lblMonatsBruttoValue.Size = New System.Drawing.Size(45, 13)
		Me.lblMonatsBruttoValue.TabIndex = 6
		Me.lblMonatsBruttoValue.Text = "Betrag1"
		'
		'lblTag
		'
		Me.lblTag.AutoSize = True
		Me.lblTag.Location = New System.Drawing.Point(190, 29)
		Me.lblTag.Name = "lblTag"
		Me.lblTag.Size = New System.Drawing.Size(31, 13)
		Me.lblTag.TabIndex = 9
		Me.lblTag.Text = "Tag1"
		'
		'lblBerechnetFuer
		'
		Me.lblBerechnetFuer.AutoSize = True
		Me.lblBerechnetFuer.Location = New System.Drawing.Point(7, 46)
		Me.lblBerechnetFuer.Name = "lblBerechnetFuer"
		Me.lblBerechnetFuer.Size = New System.Drawing.Size(67, 13)
		Me.lblBerechnetFuer.TabIndex = 7
		Me.lblBerechnetFuer.Text = "Berechnt für"
		'
		'lblMonatsstudenVon
		'
		Me.lblMonatsstudenVon.AutoSize = True
		Me.lblMonatsstudenVon.Location = New System.Drawing.Point(85, 46)
		Me.lblMonatsstudenVon.Name = "lblMonatsstudenVon"
		Me.lblMonatsstudenVon.Size = New System.Drawing.Size(102, 13)
		Me.lblMonatsstudenVon.TabIndex = 8
		Me.lblMonatsstudenVon.Text = "Monatsstunden von"
		'
		'lblMANameValue
		'
		Me.lblMANameValue.AutoSize = True
		Me.lblMANameValue.Location = New System.Drawing.Point(72, 53)
		Me.lblMANameValue.Name = "lblMANameValue"
		Me.lblMANameValue.Size = New System.Drawing.Size(106, 13)
		Me.lblMANameValue.TabIndex = 4
		Me.lblMANameValue.Text = "Nachname, Vorname"
		'
		'lblMAName
		'
		Me.lblMAName.AutoSize = True
		Me.lblMAName.Location = New System.Drawing.Point(9, 53)
		Me.lblMAName.Name = "lblMAName"
		Me.lblMAName.Size = New System.Drawing.Size(59, 13)
		Me.lblMAName.TabIndex = 3
		Me.lblMAName.Text = "Mitarbeiter"
		'
		'lblMANrValue
		'
		Me.lblMANrValue.AutoSize = True
		Me.lblMANrValue.Location = New System.Drawing.Point(72, 36)
		Me.lblMANrValue.Name = "lblMANrValue"
		Me.lblMANrValue.Size = New System.Drawing.Size(46, 13)
		Me.lblMANrValue.TabIndex = 2
		Me.lblMANrValue.Text = "Nummer"
		'
		'lblMANr
		'
		Me.lblMANr.AutoSize = True
		Me.lblMANr.Location = New System.Drawing.Point(9, 36)
		Me.lblMANr.Name = "lblMANr"
		Me.lblMANr.Size = New System.Drawing.Size(46, 13)
		Me.lblMANr.TabIndex = 1
		Me.lblMANr.Text = "Nummer"
		'
		'lblAngabenQSTAbzug
		'
		Me.lblAngabenQSTAbzug.AutoSize = True
		Me.lblAngabenQSTAbzug.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.lblAngabenQSTAbzug.Location = New System.Drawing.Point(9, 18)
		Me.lblAngabenQSTAbzug.Name = "lblAngabenQSTAbzug"
		Me.lblAngabenQSTAbzug.Size = New System.Drawing.Size(208, 13)
		Me.lblAngabenQSTAbzug.TabIndex = 0
		Me.lblAngabenQSTAbzug.Text = "Angaben über Quellensteuer-Abzug"
		'
		'lblListeES
		'
		Me.lblListeES.AutoSize = True
		Me.lblListeES.Location = New System.Drawing.Point(10, 460)
		Me.lblListeES.Name = "lblListeES"
		Me.lblListeES.Size = New System.Drawing.Size(91, 13)
		Me.lblListeES.TabIndex = 15
		Me.lblListeES.Text = "Liste der Einsätze"
		'
		'grpLohndaten
		'
		Me.grpLohndaten.Controls.Add(Me.lblMindestAbzugInfo)
		Me.grpLohndaten.Controls.Add(Me.grdTaxData)
		Me.grpLohndaten.Controls.Add(Me.btnAdopt)
		Me.grpLohndaten.Controls.Add(Me.lblMindestAbzugValue)
		Me.grpLohndaten.Controls.Add(Me.lblMindestabzug)
		Me.grpLohndaten.Controls.Add(Me.lblAbzugWaehrung)
		Me.grpLohndaten.Controls.Add(Me.txtAbzug)
		Me.grpLohndaten.Controls.Add(Me.lblAbzug)
		Me.grpLohndaten.Controls.Add(Me.lblAnsatzProz)
		Me.grpLohndaten.Controls.Add(Me.lblAnsatz)
		Me.grpLohndaten.Controls.Add(Me.lblQSTBasisInfo)
		Me.grpLohndaten.Controls.Add(Me.lblQSTWaehrung)
		Me.grpLohndaten.Controls.Add(Me.txtQstBasis)
		Me.grpLohndaten.Controls.Add(Me.lblQSTBasis)
		Me.grpLohndaten.Controls.Add(Me.lblTarifInfo)
		Me.grpLohndaten.Controls.Add(Me.txtTarif)
		Me.grpLohndaten.Controls.Add(Me.lblTarif)
		Me.grpLohndaten.Controls.Add(Me.lblSteuerkanton)
		Me.grpLohndaten.Controls.Add(Me.txtSteuerKanton)
		Me.grpLohndaten.Controls.Add(Me.txtAnsatz)
		Me.grpLohndaten.Location = New System.Drawing.Point(343, 107)
		Me.grpLohndaten.Name = "grpLohndaten"
		Me.grpLohndaten.Size = New System.Drawing.Size(314, 332)
		Me.grpLohndaten.TabIndex = 14
		Me.grpLohndaten.TabStop = False
		Me.grpLohndaten.Text = "Lohndaten"
		'
		'grdTaxData
		'
		Me.grdTaxData.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.grdTaxData.Location = New System.Drawing.Point(3, 200)
		Me.grdTaxData.MainView = Me.gvTaxData
		Me.grdTaxData.Name = "grdTaxData"
		Me.grdTaxData.Size = New System.Drawing.Size(308, 129)
		Me.grdTaxData.TabIndex = 24
		Me.grdTaxData.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvTaxData})
		'
		'gvTaxData
		'
		Me.gvTaxData.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvTaxData.GridControl = Me.grdTaxData
		Me.gvTaxData.Name = "gvTaxData"
		Me.gvTaxData.OptionsBehavior.Editable = False
		Me.gvTaxData.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvTaxData.OptionsView.ShowGroupPanel = False
		'
		'btnAdopt
		'
		Me.btnAdopt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnAdopt.Location = New System.Drawing.Point(213, 171)
		Me.btnAdopt.Name = "btnAdopt"
		Me.btnAdopt.Size = New System.Drawing.Size(75, 23)
		Me.btnAdopt.TabIndex = 23
		Me.btnAdopt.Text = "Übernehmen"
		'
		'lblMindestAbzugValue
		'
		Me.lblMindestAbzugValue.Location = New System.Drawing.Point(96, 152)
		Me.lblMindestAbzugValue.Name = "lblMindestAbzugValue"
		Me.lblMindestAbzugValue.Size = New System.Drawing.Size(22, 13)
		Me.lblMindestAbzugValue.TabIndex = 22
		Me.lblMindestAbzugValue.Text = "0.00"
		'
		'lblMindestabzug
		'
		Me.lblMindestabzug.Location = New System.Drawing.Point(17, 152)
		Me.lblMindestabzug.Name = "lblMindestabzug"
		Me.lblMindestabzug.Size = New System.Drawing.Size(66, 13)
		Me.lblMindestabzug.TabIndex = 21
		Me.lblMindestabzug.Text = "Mindestabzug"
		'
		'lblAbzugWaehrung
		'
		Me.lblAbzugWaehrung.AutoSize = True
		Me.lblAbzugWaehrung.Location = New System.Drawing.Point(187, 127)
		Me.lblAbzugWaehrung.Name = "lblAbzugWaehrung"
		Me.lblAbzugWaehrung.Size = New System.Drawing.Size(26, 13)
		Me.lblAbzugWaehrung.TabIndex = 20
		Me.lblAbzugWaehrung.Text = "sFr."
		'
		'txtAbzug
		'
		Me.txtAbzug.Location = New System.Drawing.Point(95, 124)
		Me.txtAbzug.Name = "txtAbzug"
		Me.txtAbzug.Properties.Appearance.Options.UseTextOptions = True
		Me.txtAbzug.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.txtAbzug.Properties.Mask.EditMask = "n2"
		Me.txtAbzug.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
		Me.txtAbzug.Properties.Mask.UseMaskAsDisplayFormat = True
		Me.txtAbzug.Size = New System.Drawing.Size(86, 20)
		Me.txtAbzug.TabIndex = 5
		'
		'lblAbzug
		'
		Me.lblAbzug.AutoSize = True
		Me.lblAbzug.Location = New System.Drawing.Point(17, 127)
		Me.lblAbzug.Name = "lblAbzug"
		Me.lblAbzug.Size = New System.Drawing.Size(37, 13)
		Me.lblAbzug.TabIndex = 18
		Me.lblAbzug.Text = "Abzug"
		'
		'lblAnsatzProz
		'
		Me.lblAnsatzProz.AutoSize = True
		Me.lblAnsatzProz.Location = New System.Drawing.Point(223, 101)
		Me.lblAnsatzProz.Name = "lblAnsatzProz"
		Me.lblAnsatzProz.Size = New System.Drawing.Size(18, 13)
		Me.lblAnsatzProz.TabIndex = 17
		Me.lblAnsatzProz.Text = "%"
		'
		'lblAnsatz
		'
		Me.lblAnsatz.AutoSize = True
		Me.lblAnsatz.Location = New System.Drawing.Point(17, 100)
		Me.lblAnsatz.Name = "lblAnsatz"
		Me.lblAnsatz.Size = New System.Drawing.Size(40, 13)
		Me.lblAnsatz.TabIndex = 16
		Me.lblAnsatz.Text = "Ansatz"
		'
		'lblQSTBasisInfo
		'
		Me.lblQSTBasisInfo.AutoSize = True
		Me.lblQSTBasisInfo.Location = New System.Drawing.Point(223, 76)
		Me.lblQSTBasisInfo.Name = "lblQSTBasisInfo"
		Me.lblQSTBasisInfo.Size = New System.Drawing.Size(15, 13)
		Me.lblQSTBasisInfo.TabIndex = 15
		Me.lblQSTBasisInfo.Text = "#"
		'
		'lblQSTWaehrung
		'
		Me.lblQSTWaehrung.AutoSize = True
		Me.lblQSTWaehrung.Location = New System.Drawing.Point(187, 75)
		Me.lblQSTWaehrung.Name = "lblQSTWaehrung"
		Me.lblQSTWaehrung.Size = New System.Drawing.Size(26, 13)
		Me.lblQSTWaehrung.TabIndex = 14
		Me.lblQSTWaehrung.Text = "sFr."
		'
		'txtQstBasis
		'
		Me.txtQstBasis.Location = New System.Drawing.Point(95, 72)
		Me.txtQstBasis.Name = "txtQstBasis"
		Me.txtQstBasis.Properties.Appearance.Options.UseTextOptions = True
		Me.txtQstBasis.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.txtQstBasis.Properties.Mask.EditMask = "n2"
		Me.txtQstBasis.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
		Me.txtQstBasis.Properties.Mask.UseMaskAsDisplayFormat = True
		Me.txtQstBasis.Size = New System.Drawing.Size(86, 20)
		Me.txtQstBasis.TabIndex = 3
		'
		'lblQSTBasis
		'
		Me.lblQSTBasis.AutoSize = True
		Me.lblQSTBasis.Location = New System.Drawing.Point(17, 75)
		Me.lblQSTBasis.Name = "lblQSTBasis"
		Me.lblQSTBasis.Size = New System.Drawing.Size(55, 13)
		Me.lblQSTBasis.TabIndex = 12
		Me.lblQSTBasis.Text = "QST-Basis"
		'
		'lblTarifInfo
		'
		Me.lblTarifInfo.AutoSize = True
		Me.lblTarifInfo.Location = New System.Drawing.Point(187, 49)
		Me.lblTarifInfo.Name = "lblTarifInfo"
		Me.lblTarifInfo.Size = New System.Drawing.Size(15, 13)
		Me.lblTarifInfo.TabIndex = 11
		Me.lblTarifInfo.Text = "#"
		'
		'txtTarif
		'
		Me.txtTarif.Location = New System.Drawing.Point(95, 46)
		Me.txtTarif.Name = "txtTarif"
		Me.txtTarif.Properties.Appearance.Options.UseTextOptions = True
		Me.txtTarif.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.txtTarif.Properties.Mask.UseMaskAsDisplayFormat = True
		Me.txtTarif.Size = New System.Drawing.Size(86, 20)
		Me.txtTarif.TabIndex = 2
		'
		'lblTarif
		'
		Me.lblTarif.AutoSize = True
		Me.lblTarif.Location = New System.Drawing.Point(17, 49)
		Me.lblTarif.Name = "lblTarif"
		Me.lblTarif.Size = New System.Drawing.Size(29, 13)
		Me.lblTarif.TabIndex = 9
		Me.lblTarif.Text = "Tarif"
		'
		'lblSteuerkanton
		'
		Me.lblSteuerkanton.AutoSize = True
		Me.lblSteuerkanton.Location = New System.Drawing.Point(17, 20)
		Me.lblSteuerkanton.Name = "lblSteuerkanton"
		Me.lblSteuerkanton.Size = New System.Drawing.Size(72, 13)
		Me.lblSteuerkanton.TabIndex = 8
		Me.lblSteuerkanton.Text = "Steuerkanton"
		'
		'txtSteuerKanton
		'
		Me.txtSteuerKanton.Location = New System.Drawing.Point(95, 20)
		Me.txtSteuerKanton.Name = "txtSteuerKanton"
		Me.txtSteuerKanton.Properties.Appearance.Options.UseTextOptions = True
		Me.txtSteuerKanton.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.txtSteuerKanton.Size = New System.Drawing.Size(62, 20)
		Me.txtSteuerKanton.TabIndex = 1
		'
		'txtAnsatz
		'
		Me.txtAnsatz.Location = New System.Drawing.Point(95, 97)
		Me.txtAnsatz.Name = "txtAnsatz"
		Me.txtAnsatz.Properties.Appearance.Options.UseTextOptions = True
		Me.txtAnsatz.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.txtAnsatz.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Redo)})
		Me.txtAnsatz.Size = New System.Drawing.Size(86, 20)
		Me.txtAnsatz.TabIndex = 4
		'
		'grpPersonalien
		'
		Me.grpPersonalien.Controls.Add(Me.grdPersonalien)
		Me.grpPersonalien.Location = New System.Drawing.Point(10, 107)
		Me.grpPersonalien.Name = "grpPersonalien"
		Me.grpPersonalien.Size = New System.Drawing.Size(301, 332)
		Me.grpPersonalien.TabIndex = 13
		Me.grpPersonalien.TabStop = False
		Me.grpPersonalien.Text = "Personalien"
		'
		'grdPersonalien
		'
		Me.grdPersonalien.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.grdPersonalien.Location = New System.Drawing.Point(6, 20)
		Me.grdPersonalien.MainView = Me.gvPersonalien
		Me.grdPersonalien.Name = "grdPersonalien"
		Me.grdPersonalien.Size = New System.Drawing.Size(289, 306)
		Me.grdPersonalien.TabIndex = 2
		Me.grdPersonalien.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvPersonalien})
		'
		'gvPersonalien
		'
		Me.gvPersonalien.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvPersonalien.GridControl = Me.grdPersonalien
		Me.gvPersonalien.Name = "gvPersonalien"
		Me.gvPersonalien.OptionsBehavior.Editable = False
		Me.gvPersonalien.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvPersonalien.OptionsView.ShowGroupPanel = False
		'
		'txtEntryAndExit
		'
		Me.txtEntryAndExit.Location = New System.Drawing.Point(455, 559)
		Me.txtEntryAndExit.Name = "txtEntryAndExit"
		Me.txtEntryAndExit.Properties.Appearance.Options.UseTextOptions = True
		Me.txtEntryAndExit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.txtEntryAndExit.Properties.Mask.EditMask = "n2"
		Me.txtEntryAndExit.Properties.Mask.UseMaskAsDisplayFormat = True
		Me.txtEntryAndExit.Size = New System.Drawing.Size(204, 20)
		Me.txtEntryAndExit.TabIndex = 26
		'
		'grdListOfES
		'
		Me.grdListOfES.Location = New System.Drawing.Point(136, 460)
		Me.grdListOfES.MainView = Me.gvListOfES
		Me.grdListOfES.Name = "grdListOfES"
		Me.grdListOfES.Size = New System.Drawing.Size(523, 93)
		Me.grdListOfES.TabIndex = 289
		Me.grdListOfES.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvListOfES})
		'
		'gvListOfES
		'
		Me.gvListOfES.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvListOfES.GridControl = Me.grdListOfES
		Me.gvListOfES.Name = "gvListOfES"
		Me.gvListOfES.OptionsBehavior.Editable = False
		Me.gvListOfES.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvListOfES.OptionsView.ShowGroupPanel = False
		'
		'lblMindestAbzugInfo
		'
		Me.lblMindestAbzugInfo.Appearance.Options.UseTextOptions = True
		Me.lblMindestAbzugInfo.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
		Me.lblMindestAbzugInfo.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
		Me.lblMindestAbzugInfo.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblMindestAbzugInfo.Location = New System.Drawing.Point(17, 171)
		Me.lblMindestAbzugInfo.Name = "lblMindestAbzugInfo"
		Me.lblMindestAbzugInfo.Size = New System.Drawing.Size(190, 23)
		Me.lblMindestAbzugInfo.TabIndex = 25
		Me.lblMindestAbzugInfo.Text = "Mindestabzug"
		'
		'frmQST
		'
		Me.AcceptButton = Me.btnAdopt
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(668, 591)
		Me.Controls.Add(Me.grdListOfES)
		Me.Controls.Add(Me.txtEntryAndExit)
		Me.Controls.Add(Me.Panel1)
		Me.Controls.Add(Me.lblListeES)
		Me.Controls.Add(Me.grpLohndaten)
		Me.Controls.Add(Me.grpPersonalien)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.IconOptions.Icon = CType(resources.GetObject("frmQST.IconOptions.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmQST"
		Me.Text = "Angaben über Quellensteuer"
		Me.Panel1.ResumeLayout(False)
		Me.Panel1.PerformLayout()
		CType(Me.chkDoNOTShowForm.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.stdPanel.ResumeLayout(False)
		Me.stdPanel.PerformLayout()
		Me.grpLohndaten.ResumeLayout(False)
		Me.grpLohndaten.PerformLayout()
		CType(Me.grdTaxData, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvTaxData, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtAbzug.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtQstBasis.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtTarif.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtSteuerKanton.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtAnsatz.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.grpPersonalien.ResumeLayout(False)
		CType(Me.grdPersonalien, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvPersonalien, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtEntryAndExit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.grdListOfES, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvListOfES, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents Panel1 As System.Windows.Forms.Panel
	Friend WithEvents lblMonatsStunden As System.Windows.Forms.Label
	Friend WithEvents lblTag As System.Windows.Forms.Label
	Friend WithEvents lblMonatsstudenVon As System.Windows.Forms.Label
	Friend WithEvents lblBerechnetFuer As System.Windows.Forms.Label
	Friend WithEvents lblMonatsBruttoValue As System.Windows.Forms.Label
	Friend WithEvents lblMonatsbrutto As System.Windows.Forms.Label
	Friend WithEvents lblMANameValue As System.Windows.Forms.Label
	Friend WithEvents lblMAName As System.Windows.Forms.Label
	Friend WithEvents lblMANrValue As System.Windows.Forms.Label
	Friend WithEvents lblMANr As System.Windows.Forms.Label
	Friend WithEvents lblAngabenQSTAbzug As System.Windows.Forms.Label
	Friend WithEvents lblListeES As System.Windows.Forms.Label
	Friend WithEvents grpLohndaten As System.Windows.Forms.GroupBox
	Friend WithEvents grpPersonalien As System.Windows.Forms.GroupBox
	Friend WithEvents grdPersonalien As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvPersonalien As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents lblMindestAbzugValue As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblMindestabzug As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblAbzugWaehrung As System.Windows.Forms.Label
	Friend WithEvents txtAbzug As DevExpress.XtraEditors.TextEdit
	Friend WithEvents lblAbzug As System.Windows.Forms.Label
	Friend WithEvents lblAnsatzProz As System.Windows.Forms.Label
	Friend WithEvents lblAnsatz As System.Windows.Forms.Label
	Friend WithEvents lblQSTBasisInfo As System.Windows.Forms.Label
	Friend WithEvents lblQSTWaehrung As System.Windows.Forms.Label
	Friend WithEvents txtQstBasis As DevExpress.XtraEditors.TextEdit
	Friend WithEvents lblQSTBasis As System.Windows.Forms.Label
	Friend WithEvents lblTarifInfo As System.Windows.Forms.Label
	Friend WithEvents txtTarif As DevExpress.XtraEditors.TextEdit
	Friend WithEvents lblTarif As System.Windows.Forms.Label
	Friend WithEvents lblSteuerkanton As System.Windows.Forms.Label
	Friend WithEvents txtSteuerKanton As DevExpress.XtraEditors.TextEdit
	Friend WithEvents btnAdopt As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents stdPanel As System.Windows.Forms.Panel
	Friend WithEvents txtEntryAndExit As DevExpress.XtraEditors.TextEdit
	Friend WithEvents grdListOfES As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvListOfES As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents txtAnsatz As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents chkDoNOTShowForm As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents grdTaxData As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvTaxData As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents lblMindestAbzugInfo As DevExpress.XtraEditors.LabelControl
End Class
