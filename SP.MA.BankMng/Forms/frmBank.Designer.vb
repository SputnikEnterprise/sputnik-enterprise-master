<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBank
  Inherits SP.Infrastructure.Forms.frmBase

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
		Dim EditorButtonImageOptions1 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBank))
		Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject4 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Me.sccMain = New DevExpress.XtraEditors.SplitContainerControl()
		Me.gridBank = New DevExpress.XtraGrid.GridControl()
		Me.gvBank = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.txtSwift = New DevExpress.XtraEditors.ButtonEdit()
		Me.btnCreateTODO = New DevExpress.XtraEditors.SimpleButton()
		Me.btnOpenBankSearchForm = New DevExpress.XtraEditors.SimpleButton()
		Me.employeePicture = New DevExpress.XtraEditors.PictureEdit()
		Me.lblSwift = New DevExpress.XtraEditors.LabelControl()
		Me.chkAsStandard = New DevExpress.XtraEditors.CheckEdit()
		Me.chkForZG = New DevExpress.XtraEditors.CheckEdit()
		Me.chkForSalary = New DevExpress.XtraEditors.CheckEdit()
		Me.chkAsforeignBank = New DevExpress.XtraEditors.CheckEdit()
		Me.grpreservefelder = New DevComponents.DotNetBar.Controls.GroupPanel()
		Me.XtraScrollableControl2 = New DevExpress.XtraEditors.XtraScrollableControl()
		Me.lbl3Adresse = New DevExpress.XtraEditors.LabelControl()
		Me.txt3Address = New DevExpress.XtraEditors.TextEdit()
		Me.txt2Address = New DevExpress.XtraEditors.TextEdit()
		Me.txt1Address = New DevExpress.XtraEditors.TextEdit()
		Me.txtOwner = New DevExpress.XtraEditors.TextEdit()
		Me.txtIBAN = New DevExpress.XtraEditors.TextEdit()
		Me.txtKontoNr = New DevExpress.XtraEditors.TextEdit()
		Me.lbl1Adresse = New DevExpress.XtraEditors.LabelControl()
		Me.lblInhaber = New DevExpress.XtraEditors.LabelControl()
		Me.lbl2Adresse = New DevExpress.XtraEditors.LabelControl()
		Me.lbliban = New DevExpress.XtraEditors.LabelControl()
		Me.lblKontonummer = New DevExpress.XtraEditors.LabelControl()
		Me.txBankAddress = New DevExpress.XtraEditors.TextEdit()
		Me.txtBankname = New DevExpress.XtraEditors.TextEdit()
		Me.txtBLZ = New DevExpress.XtraEditors.TextEdit()
		Me.btnDeleteBank = New DevExpress.XtraEditors.SimpleButton()
		Me.lblBankChanged = New DevExpress.XtraEditors.LabelControl()
		Me.lblBankCreated = New DevExpress.XtraEditors.LabelControl()
		Me.lblgaendert = New DevExpress.XtraEditors.LabelControl()
		Me.lblerstellt = New DevExpress.XtraEditors.LabelControl()
		Me.btnNewBank = New DevExpress.XtraEditors.SimpleButton()
		Me.btnSave = New DevExpress.XtraEditors.SimpleButton()
		Me.lblBLZ = New DevExpress.XtraEditors.LabelControl()
		Me.lblBankname = New DevExpress.XtraEditors.LabelControl()
		Me.lblBankOrt = New DevExpress.XtraEditors.LabelControl()
		Me.txtClearing = New DevExpress.XtraEditors.TextEdit()
		Me.lblClearing = New DevExpress.XtraEditors.LabelControl()
		Me.errorProviderBankMng = New System.Windows.Forms.ErrorProvider()
		CType(Me.sccMain, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.sccMain.SuspendLayout()
		CType(Me.gridBank, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvBank, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtSwift.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.employeePicture.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkAsStandard.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkForZG.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkForSalary.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkAsforeignBank.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grpreservefelder.SuspendLayout()
		Me.XtraScrollableControl2.SuspendLayout()
		CType(Me.txt3Address.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txt2Address.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txt1Address.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtOwner.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtIBAN.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtKontoNr.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txBankAddress.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtBankname.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtBLZ.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtClearing.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.errorProviderBankMng, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'sccMain
		'
		Me.sccMain.Dock = System.Windows.Forms.DockStyle.Fill
		Me.sccMain.Location = New System.Drawing.Point(5, 5)
		Me.sccMain.Name = "sccMain"
		Me.sccMain.Panel1.Controls.Add(Me.gridBank)
		Me.sccMain.Panel1.Text = "Panel1"
		Me.sccMain.Panel2.Controls.Add(Me.txtSwift)
		Me.sccMain.Panel2.Controls.Add(Me.btnCreateTODO)
		Me.sccMain.Panel2.Controls.Add(Me.btnOpenBankSearchForm)
		Me.sccMain.Panel2.Controls.Add(Me.employeePicture)
		Me.sccMain.Panel2.Controls.Add(Me.lblSwift)
		Me.sccMain.Panel2.Controls.Add(Me.chkAsStandard)
		Me.sccMain.Panel2.Controls.Add(Me.chkForZG)
		Me.sccMain.Panel2.Controls.Add(Me.chkForSalary)
		Me.sccMain.Panel2.Controls.Add(Me.chkAsforeignBank)
		Me.sccMain.Panel2.Controls.Add(Me.grpreservefelder)
		Me.sccMain.Panel2.Controls.Add(Me.txBankAddress)
		Me.sccMain.Panel2.Controls.Add(Me.txtBankname)
		Me.sccMain.Panel2.Controls.Add(Me.txtBLZ)
		Me.sccMain.Panel2.Controls.Add(Me.btnDeleteBank)
		Me.sccMain.Panel2.Controls.Add(Me.lblBankChanged)
		Me.sccMain.Panel2.Controls.Add(Me.lblBankCreated)
		Me.sccMain.Panel2.Controls.Add(Me.lblgaendert)
		Me.sccMain.Panel2.Controls.Add(Me.lblerstellt)
		Me.sccMain.Panel2.Controls.Add(Me.btnNewBank)
		Me.sccMain.Panel2.Controls.Add(Me.btnSave)
		Me.sccMain.Panel2.Controls.Add(Me.lblBLZ)
		Me.sccMain.Panel2.Controls.Add(Me.lblBankname)
		Me.sccMain.Panel2.Controls.Add(Me.lblBankOrt)
		Me.sccMain.Panel2.Controls.Add(Me.txtClearing)
		Me.sccMain.Panel2.Controls.Add(Me.lblClearing)
		Me.sccMain.Panel2.Padding = New System.Windows.Forms.Padding(5)
		Me.sccMain.Panel2.Text = "Panel2"
		Me.sccMain.Size = New System.Drawing.Size(1006, 578)
		Me.sccMain.SplitterPosition = 362
		Me.sccMain.TabIndex = 0
		Me.sccMain.Text = "SplitContainerControl1"
		'
		'gridBank
		'
		Me.gridBank.Dock = System.Windows.Forms.DockStyle.Fill
		Me.gridBank.Location = New System.Drawing.Point(0, 0)
		Me.gridBank.MainView = Me.gvBank
		Me.gridBank.Name = "gridBank"
		Me.gridBank.Size = New System.Drawing.Size(362, 578)
		Me.gridBank.TabIndex = 2
		Me.gridBank.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvBank})
		'
		'gvBank
		'
		Me.gvBank.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvBank.GridControl = Me.gridBank
		Me.gvBank.Name = "gvBank"
		Me.gvBank.OptionsBehavior.Editable = False
		Me.gvBank.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvBank.OptionsView.ShowGroupPanel = False
		'
		'txtSwift
		'
		Me.txtSwift.Location = New System.Drawing.Point(153, 174)
		Me.txtSwift.Name = "txtSwift"
		EditorButtonImageOptions1.Image = CType(resources.GetObject("EditorButtonImageOptions1.Image"), System.Drawing.Image)
		SerializableAppearanceObject1.Image = CType(resources.GetObject("SerializableAppearanceObject1.Image"), System.Drawing.Image)
		SerializableAppearanceObject1.Options.UseImage = True
		SerializableAppearanceObject2.Image = CType(resources.GetObject("SerializableAppearanceObject2.Image"), System.Drawing.Image)
		SerializableAppearanceObject2.Options.UseImage = True
		SerializableAppearanceObject3.Image = CType(resources.GetObject("SerializableAppearanceObject3.Image"), System.Drawing.Image)
		SerializableAppearanceObject3.Options.UseImage = True
		SerializableAppearanceObject4.Image = CType(resources.GetObject("SerializableAppearanceObject4.Image"), System.Drawing.Image)
		SerializableAppearanceObject4.Options.UseImage = True
		Me.txtSwift.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, True, True, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
		Me.txtSwift.Size = New System.Drawing.Size(298, 22)
		Me.txtSwift.TabIndex = 263
		'
		'btnCreateTODO
		'
		Me.btnCreateTODO.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnCreateTODO.ImageOptions.Image = CType(resources.GetObject("btnCreateTODO.ImageOptions.Image"), System.Drawing.Image)
		Me.btnCreateTODO.Location = New System.Drawing.Point(509, 415)
		Me.btnCreateTODO.Name = "btnCreateTODO"
		Me.btnCreateTODO.Size = New System.Drawing.Size(105, 28)
		Me.btnCreateTODO.TabIndex = 14
		Me.btnCreateTODO.Text = "To-do erstellen"
		Me.btnCreateTODO.Visible = False
		'
		'btnOpenBankSearchForm
		'
		Me.btnOpenBankSearchForm.AllowFocus = False
		Me.btnOpenBankSearchForm.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.btnOpenBankSearchForm.Appearance.Options.UseBackColor = True
		Me.btnOpenBankSearchForm.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat
		Me.btnOpenBankSearchForm.ImageOptions.Image = CType(resources.GetObject("btnOpenBankSearchForm.ImageOptions.Image"), System.Drawing.Image)
		Me.btnOpenBankSearchForm.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter
		Me.btnOpenBankSearchForm.Location = New System.Drawing.Point(252, 97)
		Me.btnOpenBankSearchForm.Margin = New System.Windows.Forms.Padding(1)
		Me.btnOpenBankSearchForm.Name = "btnOpenBankSearchForm"
		Me.btnOpenBankSearchForm.Size = New System.Drawing.Size(20, 20)
		Me.btnOpenBankSearchForm.TabIndex = 228
		'
		'employeePicture
		'
		Me.employeePicture.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.employeePicture.Cursor = System.Windows.Forms.Cursors.Default
		Me.employeePicture.Location = New System.Drawing.Point(494, 28)
		Me.employeePicture.Name = "employeePicture"
		Me.employeePicture.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.employeePicture.Properties.Appearance.Options.UseBackColor = True
		Me.employeePicture.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
		Me.employeePicture.Size = New System.Drawing.Size(120, 120)
		Me.employeePicture.TabIndex = 262
		'
		'lblSwift
		'
		Me.lblSwift.Appearance.Options.UseTextOptions = True
		Me.lblSwift.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblSwift.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblSwift.Location = New System.Drawing.Point(28, 178)
		Me.lblSwift.Name = "lblSwift"
		Me.lblSwift.Size = New System.Drawing.Size(119, 13)
		Me.lblSwift.TabIndex = 261
		Me.lblSwift.Text = "Swift"
		'
		'chkAsStandard
		'
		Me.chkAsStandard.Location = New System.Drawing.Point(151, 464)
		Me.chkAsStandard.Name = "chkAsStandard"
		Me.chkAsStandard.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.[Default]
		Me.chkAsStandard.Properties.Caption = "Als Standard-Bankverbindung definieren"
		Me.chkAsStandard.Size = New System.Drawing.Size(407, 19)
		Me.chkAsStandard.TabIndex = 9
		'
		'chkForZG
		'
		Me.chkForZG.Location = New System.Drawing.Point(151, 489)
		Me.chkForZG.Name = "chkForZG"
		Me.chkForZG.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.[Default]
		Me.chkForZG.Properties.Caption = "Diese Daten sind für Vorschusszahlungen"
		Me.chkForZG.Size = New System.Drawing.Size(407, 19)
		Me.chkForZG.TabIndex = 10
		'
		'chkForSalary
		'
		Me.chkForSalary.Location = New System.Drawing.Point(151, 26)
		Me.chkForSalary.Name = "chkForSalary"
		Me.chkForSalary.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.[Default]
		Me.chkForSalary.Properties.Caption = "Für Auszahlung von speziellen Lohndaten"
		Me.chkForSalary.Size = New System.Drawing.Size(324, 19)
		Me.chkForSalary.TabIndex = 1
		'
		'chkAsforeignBank
		'
		Me.chkAsforeignBank.Location = New System.Drawing.Point(151, 51)
		Me.chkAsforeignBank.Name = "chkAsforeignBank"
		Me.chkAsforeignBank.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.[Default]
		Me.chkAsforeignBank.Properties.Caption = "Als Ausland-Bank definieren"
		Me.chkAsforeignBank.Size = New System.Drawing.Size(324, 19)
		Me.chkAsforeignBank.TabIndex = 2
		'
		'grpreservefelder
		'
		Me.grpreservefelder.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.grpreservefelder.BackColor = System.Drawing.Color.Transparent
		Me.grpreservefelder.CanvasColor = System.Drawing.SystemColors.Control
		Me.grpreservefelder.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
		Me.grpreservefelder.Controls.Add(Me.XtraScrollableControl2)
		Me.grpreservefelder.Location = New System.Drawing.Point(21, 211)
		Me.grpreservefelder.Name = "grpreservefelder"
		Me.grpreservefelder.Size = New System.Drawing.Size(471, 235)
		'
		'
		'
		Me.grpreservefelder.Style.BackColorGradientAngle = 90
		Me.grpreservefelder.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.grpreservefelder.Style.BorderBottomWidth = 1
		Me.grpreservefelder.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
		Me.grpreservefelder.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.grpreservefelder.Style.BorderLeftWidth = 1
		Me.grpreservefelder.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.grpreservefelder.Style.BorderRightWidth = 1
		Me.grpreservefelder.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.grpreservefelder.Style.BorderTopWidth = 1
		Me.grpreservefelder.Style.CornerDiameter = 4
		Me.grpreservefelder.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
		Me.grpreservefelder.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.grpreservefelder.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
		Me.grpreservefelder.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
		Me.grpreservefelder.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
		'
		'
		'
		Me.grpreservefelder.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
		'
		'
		'
		Me.grpreservefelder.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
		Me.grpreservefelder.TabIndex = 8
		Me.grpreservefelder.Text = "Begünstigte"
		'
		'XtraScrollableControl2
		'
		Me.XtraScrollableControl2.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.XtraScrollableControl2.Appearance.Options.UseBackColor = True
		Me.XtraScrollableControl2.Controls.Add(Me.lbl3Adresse)
		Me.XtraScrollableControl2.Controls.Add(Me.txt3Address)
		Me.XtraScrollableControl2.Controls.Add(Me.txt2Address)
		Me.XtraScrollableControl2.Controls.Add(Me.txt1Address)
		Me.XtraScrollableControl2.Controls.Add(Me.txtOwner)
		Me.XtraScrollableControl2.Controls.Add(Me.txtIBAN)
		Me.XtraScrollableControl2.Controls.Add(Me.txtKontoNr)
		Me.XtraScrollableControl2.Controls.Add(Me.lbl1Adresse)
		Me.XtraScrollableControl2.Controls.Add(Me.lblInhaber)
		Me.XtraScrollableControl2.Controls.Add(Me.lbl2Adresse)
		Me.XtraScrollableControl2.Controls.Add(Me.lbliban)
		Me.XtraScrollableControl2.Controls.Add(Me.lblKontonummer)
		Me.XtraScrollableControl2.Dock = System.Windows.Forms.DockStyle.Fill
		Me.XtraScrollableControl2.Location = New System.Drawing.Point(0, 0)
		Me.XtraScrollableControl2.Name = "XtraScrollableControl2"
		Me.XtraScrollableControl2.Size = New System.Drawing.Size(465, 213)
		Me.XtraScrollableControl2.TabIndex = 0
		'
		'lbl3Adresse
		'
		Me.lbl3Adresse.Appearance.Options.UseTextOptions = True
		Me.lbl3Adresse.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lbl3Adresse.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lbl3Adresse.Location = New System.Drawing.Point(15, 171)
		Me.lbl3Adresse.Name = "lbl3Adresse"
		Me.lbl3Adresse.Size = New System.Drawing.Size(108, 13)
		Me.lbl3Adresse.TabIndex = 227
		Me.lbl3Adresse.Text = "3. Adresse"
		'
		'txt3Address
		'
		Me.txt3Address.Location = New System.Drawing.Point(129, 167)
		Me.txt3Address.Name = "txt3Address"
		Me.txt3Address.Size = New System.Drawing.Size(298, 20)
		Me.txt3Address.TabIndex = 6
		'
		'txt2Address
		'
		Me.txt2Address.Location = New System.Drawing.Point(129, 141)
		Me.txt2Address.Name = "txt2Address"
		Me.txt2Address.Size = New System.Drawing.Size(298, 20)
		Me.txt2Address.TabIndex = 5
		'
		'txt1Address
		'
		Me.txt1Address.Location = New System.Drawing.Point(129, 115)
		Me.txt1Address.Name = "txt1Address"
		Me.txt1Address.Size = New System.Drawing.Size(298, 20)
		Me.txt1Address.TabIndex = 4
		'
		'txtOwner
		'
		Me.txtOwner.Location = New System.Drawing.Point(129, 89)
		Me.txtOwner.Name = "txtOwner"
		Me.txtOwner.Size = New System.Drawing.Size(298, 20)
		Me.txtOwner.TabIndex = 3
		'
		'txtIBAN
		'
		Me.txtIBAN.Location = New System.Drawing.Point(129, 39)
		Me.txtIBAN.Name = "txtIBAN"
		Me.txtIBAN.Size = New System.Drawing.Size(298, 20)
		Me.txtIBAN.TabIndex = 2
		'
		'txtKontoNr
		'
		Me.txtKontoNr.Location = New System.Drawing.Point(129, 13)
		Me.txtKontoNr.Name = "txtKontoNr"
		Me.txtKontoNr.Size = New System.Drawing.Size(298, 20)
		Me.txtKontoNr.TabIndex = 1
		'
		'lbl1Adresse
		'
		Me.lbl1Adresse.Appearance.Options.UseTextOptions = True
		Me.lbl1Adresse.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lbl1Adresse.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lbl1Adresse.Location = New System.Drawing.Point(15, 119)
		Me.lbl1Adresse.Name = "lbl1Adresse"
		Me.lbl1Adresse.Size = New System.Drawing.Size(108, 13)
		Me.lbl1Adresse.TabIndex = 213
		Me.lbl1Adresse.Text = "1. Adresse"
		'
		'lblInhaber
		'
		Me.lblInhaber.Appearance.Options.UseTextOptions = True
		Me.lblInhaber.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblInhaber.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblInhaber.Location = New System.Drawing.Point(15, 93)
		Me.lblInhaber.Name = "lblInhaber"
		Me.lblInhaber.Size = New System.Drawing.Size(108, 13)
		Me.lblInhaber.TabIndex = 212
		Me.lblInhaber.Text = "Inhaber"
		'
		'lbl2Adresse
		'
		Me.lbl2Adresse.Appearance.Options.UseTextOptions = True
		Me.lbl2Adresse.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lbl2Adresse.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lbl2Adresse.Location = New System.Drawing.Point(15, 145)
		Me.lbl2Adresse.Name = "lbl2Adresse"
		Me.lbl2Adresse.Size = New System.Drawing.Size(108, 13)
		Me.lbl2Adresse.TabIndex = 209
		Me.lbl2Adresse.Text = "2. Adresse"
		'
		'lbliban
		'
		Me.lbliban.Appearance.Options.UseTextOptions = True
		Me.lbliban.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lbliban.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lbliban.Location = New System.Drawing.Point(15, 43)
		Me.lbliban.Name = "lbliban"
		Me.lbliban.Size = New System.Drawing.Size(108, 13)
		Me.lbliban.TabIndex = 208
		Me.lbliban.Text = "IBAN"
		'
		'lblKontonummer
		'
		Me.lblKontonummer.Appearance.Options.UseTextOptions = True
		Me.lblKontonummer.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblKontonummer.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblKontonummer.Location = New System.Drawing.Point(15, 17)
		Me.lblKontonummer.Name = "lblKontonummer"
		Me.lblKontonummer.Size = New System.Drawing.Size(108, 13)
		Me.lblKontonummer.TabIndex = 207
		Me.lblKontonummer.Text = "Kontonummer"
		'
		'txBankAddress
		'
		Me.txBankAddress.Location = New System.Drawing.Point(153, 148)
		Me.txBankAddress.Name = "txBankAddress"
		Me.txBankAddress.Size = New System.Drawing.Size(298, 20)
		Me.txBankAddress.TabIndex = 6
		'
		'txtBankname
		'
		Me.txtBankname.Location = New System.Drawing.Point(153, 122)
		Me.txtBankname.Name = "txtBankname"
		Me.txtBankname.Size = New System.Drawing.Size(298, 20)
		Me.txtBankname.TabIndex = 5
		'
		'txtBLZ
		'
		Me.txtBLZ.Location = New System.Drawing.Point(331, 97)
		Me.txtBLZ.Name = "txtBLZ"
		Me.txtBLZ.Size = New System.Drawing.Size(120, 20)
		Me.txtBLZ.TabIndex = 4
		'
		'btnDeleteBank
		'
		Me.btnDeleteBank.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnDeleteBank.ImageOptions.Image = CType(resources.GetObject("btnDeleteBank.ImageOptions.Image"), System.Drawing.Image)
		Me.btnDeleteBank.Location = New System.Drawing.Point(509, 290)
		Me.btnDeleteBank.Name = "btnDeleteBank"
		Me.btnDeleteBank.Size = New System.Drawing.Size(105, 28)
		Me.btnDeleteBank.TabIndex = 13
		Me.btnDeleteBank.Text = "Löschen"
		'
		'lblBankChanged
		'
		Me.lblBankChanged.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.lblBankChanged.Location = New System.Drawing.Point(153, 554)
		Me.lblBankChanged.Name = "lblBankChanged"
		Me.lblBankChanged.Size = New System.Drawing.Size(49, 13)
		Me.lblBankChanged.TabIndex = 45
		Me.lblBankChanged.Text = "Geändert:"
		'
		'lblBankCreated
		'
		Me.lblBankCreated.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.lblBankCreated.Location = New System.Drawing.Point(153, 534)
		Me.lblBankCreated.Name = "lblBankCreated"
		Me.lblBankCreated.Size = New System.Drawing.Size(37, 13)
		Me.lblBankCreated.TabIndex = 44
		Me.lblBankCreated.Text = "Erstellt:"
		'
		'lblgaendert
		'
		Me.lblgaendert.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.lblgaendert.Appearance.Options.UseTextOptions = True
		Me.lblgaendert.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblgaendert.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblgaendert.Location = New System.Drawing.Point(70, 554)
		Me.lblgaendert.Name = "lblgaendert"
		Me.lblgaendert.Size = New System.Drawing.Size(77, 13)
		Me.lblgaendert.TabIndex = 43
		Me.lblgaendert.Text = "Geändert"
		'
		'lblerstellt
		'
		Me.lblerstellt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.lblerstellt.Appearance.Options.UseTextOptions = True
		Me.lblerstellt.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblerstellt.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblerstellt.Location = New System.Drawing.Point(70, 534)
		Me.lblerstellt.Name = "lblerstellt"
		Me.lblerstellt.Size = New System.Drawing.Size(77, 13)
		Me.lblerstellt.TabIndex = 42
		Me.lblerstellt.Text = "Erstellt"
		'
		'btnNewBank
		'
		Me.btnNewBank.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnNewBank.ImageOptions.Image = CType(resources.GetObject("btnNewBank.ImageOptions.Image"), System.Drawing.Image)
		Me.btnNewBank.Location = New System.Drawing.Point(509, 256)
		Me.btnNewBank.Name = "btnNewBank"
		Me.btnNewBank.Size = New System.Drawing.Size(105, 28)
		Me.btnNewBank.TabIndex = 12
		Me.btnNewBank.Text = "Neu"
		'
		'btnSave
		'
		Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnSave.ImageOptions.Image = CType(resources.GetObject("btnSave.ImageOptions.Image"), System.Drawing.Image)
		Me.btnSave.Location = New System.Drawing.Point(509, 220)
		Me.btnSave.Name = "btnSave"
		Me.btnSave.Size = New System.Drawing.Size(105, 28)
		Me.btnSave.TabIndex = 11
		Me.btnSave.Text = "Speichern"
		'
		'lblBLZ
		'
		Me.lblBLZ.Appearance.Options.UseTextOptions = True
		Me.lblBLZ.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblBLZ.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblBLZ.Location = New System.Drawing.Point(260, 101)
		Me.lblBLZ.Name = "lblBLZ"
		Me.lblBLZ.Size = New System.Drawing.Size(65, 13)
		Me.lblBLZ.TabIndex = 41
		Me.lblBLZ.Text = "BLZ"
		'
		'lblBankname
		'
		Me.lblBankname.Appearance.Options.UseTextOptions = True
		Me.lblBankname.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblBankname.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblBankname.Location = New System.Drawing.Point(28, 126)
		Me.lblBankname.Name = "lblBankname"
		Me.lblBankname.Size = New System.Drawing.Size(119, 13)
		Me.lblBankname.TabIndex = 38
		Me.lblBankname.Text = "Bank"
		'
		'lblBankOrt
		'
		Me.lblBankOrt.Appearance.Options.UseTextOptions = True
		Me.lblBankOrt.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblBankOrt.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblBankOrt.Location = New System.Drawing.Point(28, 152)
		Me.lblBankOrt.Name = "lblBankOrt"
		Me.lblBankOrt.Size = New System.Drawing.Size(119, 13)
		Me.lblBankOrt.TabIndex = 35
		Me.lblBankOrt.Text = "Ort"
		'
		'txtClearing
		'
		Me.txtClearing.Location = New System.Drawing.Point(153, 97)
		Me.txtClearing.Name = "txtClearing"
		Me.txtClearing.Size = New System.Drawing.Size(94, 20)
		Me.txtClearing.TabIndex = 3
		'
		'lblClearing
		'
		Me.lblClearing.Appearance.Options.UseTextOptions = True
		Me.lblClearing.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblClearing.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblClearing.Location = New System.Drawing.Point(28, 101)
		Me.lblClearing.Name = "lblClearing"
		Me.lblClearing.Size = New System.Drawing.Size(119, 13)
		Me.lblClearing.TabIndex = 33
		Me.lblClearing.Text = "Clearingnummer"
		'
		'errorProviderBankMng
		'
		Me.errorProviderBankMng.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
		Me.errorProviderBankMng.ContainerControl = Me
		'
		'frmBank
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(1016, 588)
		Me.Controls.Add(Me.sccMain)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MinimumSize = New System.Drawing.Size(1024, 615)
		Me.Name = "frmBank"
		Me.Padding = New System.Windows.Forms.Padding(5)
		Me.Text = "Bankverbindung"
		CType(Me.sccMain, System.ComponentModel.ISupportInitialize).EndInit()
		Me.sccMain.ResumeLayout(False)
		CType(Me.gridBank, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvBank, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtSwift.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.employeePicture.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkAsStandard.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkForZG.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkForSalary.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkAsforeignBank.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.grpreservefelder.ResumeLayout(False)
		Me.XtraScrollableControl2.ResumeLayout(False)
		CType(Me.txt3Address.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txt2Address.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txt1Address.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtOwner.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtIBAN.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtKontoNr.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txBankAddress.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtBankname.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtBLZ.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtClearing.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.errorProviderBankMng, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents sccMain As DevExpress.XtraEditors.SplitContainerControl
  Friend WithEvents gridBank As DevExpress.XtraGrid.GridControl
  Friend WithEvents gvBank As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents txBankAddress As DevExpress.XtraEditors.TextEdit
	Friend WithEvents txtBankname As DevExpress.XtraEditors.TextEdit
	Friend WithEvents txtBLZ As DevExpress.XtraEditors.TextEdit
	Friend WithEvents btnDeleteBank As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents lblBankChanged As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblBankCreated As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblgaendert As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblerstellt As DevExpress.XtraEditors.LabelControl
	Friend WithEvents btnNewBank As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents btnSave As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents lblBLZ As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblBankname As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblBankOrt As DevExpress.XtraEditors.LabelControl
	Friend WithEvents txtClearing As DevExpress.XtraEditors.TextEdit
	Friend WithEvents lblClearing As DevExpress.XtraEditors.LabelControl
	Friend WithEvents grpreservefelder As DevComponents.DotNetBar.Controls.GroupPanel
	Friend WithEvents XtraScrollableControl2 As DevExpress.XtraEditors.XtraScrollableControl
	Friend WithEvents txt3Address As DevExpress.XtraEditors.TextEdit
	Friend WithEvents txt2Address As DevExpress.XtraEditors.TextEdit
	Friend WithEvents txt1Address As DevExpress.XtraEditors.TextEdit
	Friend WithEvents txtOwner As DevExpress.XtraEditors.TextEdit
	Friend WithEvents txtIBAN As DevExpress.XtraEditors.TextEdit
	Friend WithEvents txtKontoNr As DevExpress.XtraEditors.TextEdit
	Friend WithEvents lbl1Adresse As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblInhaber As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lbl2Adresse As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lbliban As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblKontonummer As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lbl3Adresse As DevExpress.XtraEditors.LabelControl
	Friend WithEvents chkAsStandard As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chkForZG As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chkForSalary As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chkAsforeignBank As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents lblSwift As DevExpress.XtraEditors.LabelControl
	Friend WithEvents errorProviderBankMng As System.Windows.Forms.ErrorProvider
	Friend WithEvents employeePicture As DevExpress.XtraEditors.PictureEdit
	Friend WithEvents btnOpenBankSearchForm As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents btnCreateTODO As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents txtSwift As DevExpress.XtraEditors.ButtonEdit
End Class
