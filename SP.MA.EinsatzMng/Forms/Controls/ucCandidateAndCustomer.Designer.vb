Namespace UI

  <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
  Partial Class ucCandidateAndCustomer
    Inherits ucBaseControl

    'UserControl overrides dispose to clean up the component list.
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
			Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ucCandidateAndCustomer))
			Dim EditorButtonImageOptions1 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
			Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
			Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
			Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
			Dim SerializableAppearanceObject4 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
			Dim EditorButtonImageOptions2 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
			Dim SerializableAppearanceObject5 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
			Dim SerializableAppearanceObject6 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
			Dim SerializableAppearanceObject7 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
			Dim SerializableAppearanceObject8 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
			Me.grpCandidateData = New DevComponents.DotNetBar.Controls.GroupPanel()
			Me.txtNotice_Employment = New DevExpress.XtraEditors.MemoExEdit()
			Me.btnNotice_Employment = New DevExpress.XtraEditors.SimpleButton()
			Me.btnSendSMS = New DevExpress.XtraEditors.SimpleButton()
			Me.lblMAStateValue = New DevExpress.XtraEditors.LabelControl()
			Me.lblMAStatus = New DevExpress.XtraEditors.LabelControl()
			Me.lblQualificationValue = New DevExpress.XtraEditors.LabelControl()
			Me.lblQualifikation = New DevExpress.XtraEditors.LabelControl()
			Me.hlnkEmployee = New DevExpress.XtraEditors.HyperLinkEdit()
			Me.lblEmployeeAddressValue = New DevExpress.XtraEditors.LabelControl()
			Me.lblBirthdateValue = New DevExpress.XtraEditors.LabelControl()
			Me.lblAdresseKandidat = New DevExpress.XtraEditors.LabelControl()
			Me.lblGebDatum = New DevExpress.XtraEditors.LabelControl()
			Me.lblMitarbeiter = New DevExpress.XtraEditors.LabelControl()
			Me.grpCustomerData = New DevComponents.DotNetBar.Controls.GroupPanel()
			Me.txtCustomerNotice_Employment = New DevExpress.XtraEditors.MemoExEdit()
			Me.btnCustomerNotice_Employment = New DevExpress.XtraEditors.SimpleButton()
			Me.lblCustomerpostcodeValue = New DevExpress.XtraEditors.LabelControl()
			Me.lblCustomerStreetValue = New DevExpress.XtraEditors.LabelControl()
			Me.lblAdresse = New DevExpress.XtraEditors.LabelControl()
			Me.hlnkCustomer = New DevExpress.XtraEditors.HyperLinkEdit()
			Me.lblZHD = New DevExpress.XtraEditors.LabelControl()
			Me.lblKunde = New DevExpress.XtraEditors.LabelControl()
			Me.lueZHD = New DevExpress.XtraEditors.GridLookUpEdit()
			Me.gvZHD = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.grpCandidateData.SuspendLayout()
			CType(Me.txtNotice_Employment.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.hlnkEmployee.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.grpCustomerData.SuspendLayout()
			CType(Me.txtCustomerNotice_Employment.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.hlnkCustomer.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.lueZHD.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvZHD, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			'
			'grpCandidateData
			'
			Me.grpCandidateData.BackColor = System.Drawing.Color.Transparent
			Me.grpCandidateData.CanvasColor = System.Drawing.SystemColors.Control
			Me.grpCandidateData.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
			Me.grpCandidateData.Controls.Add(Me.txtNotice_Employment)
			Me.grpCandidateData.Controls.Add(Me.btnNotice_Employment)
			Me.grpCandidateData.Controls.Add(Me.btnSendSMS)
			Me.grpCandidateData.Controls.Add(Me.lblMAStateValue)
			Me.grpCandidateData.Controls.Add(Me.lblMAStatus)
			Me.grpCandidateData.Controls.Add(Me.lblQualificationValue)
			Me.grpCandidateData.Controls.Add(Me.lblQualifikation)
			Me.grpCandidateData.Controls.Add(Me.hlnkEmployee)
			Me.grpCandidateData.Controls.Add(Me.lblEmployeeAddressValue)
			Me.grpCandidateData.Controls.Add(Me.lblBirthdateValue)
			Me.grpCandidateData.Controls.Add(Me.lblAdresseKandidat)
			Me.grpCandidateData.Controls.Add(Me.lblGebDatum)
			Me.grpCandidateData.Controls.Add(Me.lblMitarbeiter)
			Me.grpCandidateData.Location = New System.Drawing.Point(7, 1)
			Me.grpCandidateData.Name = "grpCandidateData"
			Me.grpCandidateData.Size = New System.Drawing.Size(399, 143)
			'
			'
			'
			Me.grpCandidateData.Style.BackColorGradientAngle = 90
			Me.grpCandidateData.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpCandidateData.Style.BorderBottomWidth = 1
			Me.grpCandidateData.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
			Me.grpCandidateData.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpCandidateData.Style.BorderLeftWidth = 1
			Me.grpCandidateData.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpCandidateData.Style.BorderRightWidth = 1
			Me.grpCandidateData.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpCandidateData.Style.BorderTopWidth = 1
			Me.grpCandidateData.Style.CornerDiameter = 4
			Me.grpCandidateData.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
			Me.grpCandidateData.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grpCandidateData.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
			Me.grpCandidateData.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
			Me.grpCandidateData.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
			'
			'
			'
			Me.grpCandidateData.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
			'
			'
			'
			Me.grpCandidateData.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
			Me.grpCandidateData.TabIndex = 3
			Me.grpCandidateData.Text = "Kandidatdaten: {0}"
			'
			'txtNotice_Employment
			'
			Me.txtNotice_Employment.Location = New System.Drawing.Point(390, 89)
			Me.txtNotice_Employment.Name = "txtNotice_Employment"
			Me.txtNotice_Employment.Properties.ShowIcon = False
			Me.txtNotice_Employment.Size = New System.Drawing.Size(30, 20)
			Me.txtNotice_Employment.TabIndex = 155
			Me.txtNotice_Employment.Visible = False
			'
			'btnNotice_Employment
			'
			Me.btnNotice_Employment.ImageOptions.Image = CType(resources.GetObject("btnNotice_Employment.ImageOptions.Image"), System.Drawing.Image)
			Me.btnNotice_Employment.Location = New System.Drawing.Point(366, 90)
			Me.btnNotice_Employment.Name = "btnNotice_Employment"
			Me.btnNotice_Employment.Size = New System.Drawing.Size(22, 19)
			Me.btnNotice_Employment.TabIndex = 244
			Me.btnNotice_Employment.ToolTip = "Kandidaten Bemerkung"
			'
			'btnSendSMS
			'
			Me.btnSendSMS.ImageOptions.Image = CType(resources.GetObject("btnSendSMS.ImageOptions.Image"), System.Drawing.Image)
			Me.btnSendSMS.Location = New System.Drawing.Point(368, 12)
			Me.btnSendSMS.Name = "btnSendSMS"
			Me.btnSendSMS.Size = New System.Drawing.Size(22, 19)
			Me.btnSendSMS.TabIndex = 243
			Me.btnSendSMS.ToolTip = "Eine SMS-Nachricht senden"
			'
			'lblMAStateValue
			'
			Me.lblMAStateValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblMAStateValue.Location = New System.Drawing.Point(153, 93)
			Me.lblMAStateValue.Name = "lblMAStateValue"
			Me.lblMAStateValue.Size = New System.Drawing.Size(207, 13)
			Me.lblMAStateValue.TabIndex = 242
			Me.lblMAStateValue.Text = "MA-Status"
			'
			'lblMAStatus
			'
			Me.lblMAStatus.Appearance.Options.UseTextOptions = True
			Me.lblMAStatus.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblMAStatus.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblMAStatus.Location = New System.Drawing.Point(16, 93)
			Me.lblMAStatus.Name = "lblMAStatus"
			Me.lblMAStatus.Size = New System.Drawing.Size(133, 13)
			Me.lblMAStatus.TabIndex = 241
			Me.lblMAStatus.Text = "MA-Status"
			'
			'lblQualificationValue
			'
			Me.lblQualificationValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblQualificationValue.Location = New System.Drawing.Point(153, 74)
			Me.lblQualificationValue.Name = "lblQualificationValue"
			Me.lblQualificationValue.Size = New System.Drawing.Size(207, 13)
			Me.lblQualificationValue.TabIndex = 240
			Me.lblQualificationValue.Text = "Qualifikation"
			'
			'lblQualifikation
			'
			Me.lblQualifikation.Appearance.Options.UseTextOptions = True
			Me.lblQualifikation.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblQualifikation.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblQualifikation.Location = New System.Drawing.Point(16, 74)
			Me.lblQualifikation.Name = "lblQualifikation"
			Me.lblQualifikation.Size = New System.Drawing.Size(133, 13)
			Me.lblQualifikation.TabIndex = 239
			Me.lblQualifikation.Text = "Qualifikation"
			'
			'hlnkEmployee
			'
			Me.hlnkEmployee.EditValue = "Employee"
			Me.hlnkEmployee.Location = New System.Drawing.Point(154, 12)
			Me.hlnkEmployee.Name = "hlnkEmployee"
			Me.hlnkEmployee.Properties.AllowFocused = False
			Me.hlnkEmployee.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
			Me.hlnkEmployee.Size = New System.Drawing.Size(208, 18)
			Me.hlnkEmployee.TabIndex = 238
			'
			'lblEmployeeAddressValue
			'
			Me.lblEmployeeAddressValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblEmployeeAddressValue.Location = New System.Drawing.Point(154, 55)
			Me.lblEmployeeAddressValue.Name = "lblEmployeeAddressValue"
			Me.lblEmployeeAddressValue.Size = New System.Drawing.Size(207, 13)
			Me.lblEmployeeAddressValue.TabIndex = 237
			Me.lblEmployeeAddressValue.Text = "Adresse"
			'
			'lblBirthdateValue
			'
			Me.lblBirthdateValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblBirthdateValue.Location = New System.Drawing.Point(154, 36)
			Me.lblBirthdateValue.Name = "lblBirthdateValue"
			Me.lblBirthdateValue.Size = New System.Drawing.Size(188, 13)
			Me.lblBirthdateValue.TabIndex = 236
			Me.lblBirthdateValue.Text = "Geburtsdatum"
			'
			'lblAdresseKandidat
			'
			Me.lblAdresseKandidat.Appearance.Options.UseTextOptions = True
			Me.lblAdresseKandidat.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblAdresseKandidat.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblAdresseKandidat.Location = New System.Drawing.Point(16, 54)
			Me.lblAdresseKandidat.Name = "lblAdresseKandidat"
			Me.lblAdresseKandidat.Size = New System.Drawing.Size(133, 13)
			Me.lblAdresseKandidat.TabIndex = 235
			Me.lblAdresseKandidat.Text = "Adresse"
			'
			'lblGebDatum
			'
			Me.lblGebDatum.Appearance.Options.UseTextOptions = True
			Me.lblGebDatum.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblGebDatum.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblGebDatum.Location = New System.Drawing.Point(16, 36)
			Me.lblGebDatum.Name = "lblGebDatum"
			Me.lblGebDatum.Size = New System.Drawing.Size(133, 13)
			Me.lblGebDatum.TabIndex = 234
			Me.lblGebDatum.Text = "Geburtsdatum"
			'
			'lblMitarbeiter
			'
			Me.lblMitarbeiter.Appearance.Options.UseTextOptions = True
			Me.lblMitarbeiter.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblMitarbeiter.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblMitarbeiter.Location = New System.Drawing.Point(16, 15)
			Me.lblMitarbeiter.Name = "lblMitarbeiter"
			Me.lblMitarbeiter.Size = New System.Drawing.Size(133, 13)
			Me.lblMitarbeiter.TabIndex = 232
			Me.lblMitarbeiter.Text = "Mitarbeiter"
			'
			'grpCustomerData
			'
			Me.grpCustomerData.BackColor = System.Drawing.Color.Transparent
			Me.grpCustomerData.CanvasColor = System.Drawing.SystemColors.Control
			Me.grpCustomerData.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
			Me.grpCustomerData.Controls.Add(Me.txtCustomerNotice_Employment)
			Me.grpCustomerData.Controls.Add(Me.btnCustomerNotice_Employment)
			Me.grpCustomerData.Controls.Add(Me.lblCustomerpostcodeValue)
			Me.grpCustomerData.Controls.Add(Me.lblCustomerStreetValue)
			Me.grpCustomerData.Controls.Add(Me.lblAdresse)
			Me.grpCustomerData.Controls.Add(Me.hlnkCustomer)
			Me.grpCustomerData.Controls.Add(Me.lblZHD)
			Me.grpCustomerData.Controls.Add(Me.lblKunde)
			Me.grpCustomerData.Controls.Add(Me.lueZHD)
			Me.grpCustomerData.Location = New System.Drawing.Point(7, 154)
			Me.grpCustomerData.Name = "grpCustomerData"
			Me.grpCustomerData.Size = New System.Drawing.Size(399, 146)
			'
			'
			'
			Me.grpCustomerData.Style.BackColorGradientAngle = 90
			Me.grpCustomerData.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpCustomerData.Style.BorderBottomWidth = 1
			Me.grpCustomerData.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
			Me.grpCustomerData.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpCustomerData.Style.BorderLeftWidth = 1
			Me.grpCustomerData.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpCustomerData.Style.BorderRightWidth = 1
			Me.grpCustomerData.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpCustomerData.Style.BorderTopWidth = 1
			Me.grpCustomerData.Style.CornerDiameter = 4
			Me.grpCustomerData.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
			Me.grpCustomerData.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grpCustomerData.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
			Me.grpCustomerData.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
			Me.grpCustomerData.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
			'
			'
			'
			Me.grpCustomerData.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
			'
			'
			'
			Me.grpCustomerData.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
			Me.grpCustomerData.TabIndex = 4
			Me.grpCustomerData.Text = "Kundendaten: {0}"
			'
			'txtCustomerNotice_Employment
			'
			Me.txtCustomerNotice_Employment.Location = New System.Drawing.Point(390, 9)
			Me.txtCustomerNotice_Employment.Name = "txtCustomerNotice_Employment"
			Me.txtCustomerNotice_Employment.Properties.ShowIcon = False
			Me.txtCustomerNotice_Employment.Size = New System.Drawing.Size(30, 20)
			Me.txtCustomerNotice_Employment.TabIndex = 246
			Me.txtCustomerNotice_Employment.Visible = False
			'
			'btnCustomerNotice_Employment
			'
			Me.btnCustomerNotice_Employment.ImageOptions.Image = CType(resources.GetObject("btnCustomerNotice_Employment.ImageOptions.Image"), System.Drawing.Image)
			Me.btnCustomerNotice_Employment.Location = New System.Drawing.Point(366, 9)
			Me.btnCustomerNotice_Employment.Name = "btnCustomerNotice_Employment"
			Me.btnCustomerNotice_Employment.Size = New System.Drawing.Size(22, 19)
			Me.btnCustomerNotice_Employment.TabIndex = 245
			Me.btnCustomerNotice_Employment.ToolTip = "Kandidaten Bemerkung"
			'
			'lblCustomerpostcodeValue
			'
			Me.lblCustomerpostcodeValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblCustomerpostcodeValue.Location = New System.Drawing.Point(154, 55)
			Me.lblCustomerpostcodeValue.Name = "lblCustomerpostcodeValue"
			Me.lblCustomerpostcodeValue.Size = New System.Drawing.Size(207, 13)
			Me.lblCustomerpostcodeValue.TabIndex = 243
			Me.lblCustomerpostcodeValue.Text = "PLZ Ort"
			'
			'lblCustomerStreetValue
			'
			Me.lblCustomerStreetValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblCustomerStreetValue.Location = New System.Drawing.Point(154, 36)
			Me.lblCustomerStreetValue.Name = "lblCustomerStreetValue"
			Me.lblCustomerStreetValue.Size = New System.Drawing.Size(188, 13)
			Me.lblCustomerStreetValue.TabIndex = 242
			Me.lblCustomerStreetValue.Text = "Strasse"
			'
			'lblAdresse
			'
			Me.lblAdresse.Appearance.Options.UseTextOptions = True
			Me.lblAdresse.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblAdresse.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblAdresse.Location = New System.Drawing.Point(16, 36)
			Me.lblAdresse.Name = "lblAdresse"
			Me.lblAdresse.Size = New System.Drawing.Size(133, 13)
			Me.lblAdresse.TabIndex = 240
			Me.lblAdresse.Text = "Adresse"
			'
			'hlnkCustomer
			'
			Me.hlnkCustomer.EditValue = "Customer"
			Me.hlnkCustomer.Location = New System.Drawing.Point(154, 12)
			Me.hlnkCustomer.Name = "hlnkCustomer"
			Me.hlnkCustomer.Properties.AllowFocused = False
			Me.hlnkCustomer.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
			Me.hlnkCustomer.Size = New System.Drawing.Size(208, 18)
			Me.hlnkCustomer.TabIndex = 239
			'
			'lblZHD
			'
			Me.lblZHD.Appearance.Options.UseTextOptions = True
			Me.lblZHD.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblZHD.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblZHD.Location = New System.Drawing.Point(16, 78)
			Me.lblZHD.Name = "lblZHD"
			Me.lblZHD.Size = New System.Drawing.Size(133, 13)
			Me.lblZHD.TabIndex = 238
			Me.lblZHD.Text = "Zuständige Person"
			'
			'lblKunde
			'
			Me.lblKunde.Appearance.Options.UseTextOptions = True
			Me.lblKunde.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblKunde.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblKunde.Location = New System.Drawing.Point(16, 15)
			Me.lblKunde.Name = "lblKunde"
			Me.lblKunde.Size = New System.Drawing.Size(133, 13)
			Me.lblKunde.TabIndex = 234
			Me.lblKunde.Text = "Firma"
			'
			'lueZHD
			'
			Me.lueZHD.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.lueZHD.Location = New System.Drawing.Point(155, 74)
			Me.lueZHD.Name = "lueZHD"
			SerializableAppearanceObject1.ForeColor = System.Drawing.Color.Red
			SerializableAppearanceObject1.Options.UseForeColor = True
			SerializableAppearanceObject2.ForeColor = System.Drawing.Color.Red
			SerializableAppearanceObject2.Options.UseForeColor = True
			SerializableAppearanceObject3.ForeColor = System.Drawing.Color.Red
			SerializableAppearanceObject3.Options.UseForeColor = True
			SerializableAppearanceObject4.ForeColor = System.Drawing.Color.Red
			SerializableAppearanceObject4.Options.UseForeColor = True
			EditorButtonImageOptions2.Image = CType(resources.GetObject("EditorButtonImageOptions2.Image"), System.Drawing.Image)
			Me.lueZHD.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default]), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, True, True, False, EditorButtonImageOptions2, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject5, SerializableAppearanceObject6, SerializableAppearanceObject7, SerializableAppearanceObject8, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
			Me.lueZHD.Properties.PopupView = Me.gvZHD
			Me.lueZHD.Properties.ShowFooter = False
			Me.lueZHD.Size = New System.Drawing.Size(208, 24)
			Me.lueZHD.TabIndex = 239
			'
			'gvZHD
			'
			Me.gvZHD.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			Me.gvZHD.Name = "gvZHD"
			Me.gvZHD.OptionsSelection.EnableAppearanceFocusedCell = False
			Me.gvZHD.OptionsView.ShowGroupPanel = False
			'
			'ucCandidateAndCustomer
			'
			Me.Appearance.BackColor = System.Drawing.Color.White
			Me.Appearance.Options.UseBackColor = True
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.Controls.Add(Me.grpCustomerData)
			Me.Controls.Add(Me.grpCandidateData)
			Me.Name = "ucCandidateAndCustomer"
			Me.Size = New System.Drawing.Size(416, 305)
			Me.grpCandidateData.ResumeLayout(False)
			CType(Me.txtNotice_Employment.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.hlnkEmployee.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			Me.grpCustomerData.ResumeLayout(False)
			CType(Me.txtCustomerNotice_Employment.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.hlnkCustomer.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.lueZHD.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvZHD, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub
		Friend WithEvents grpCandidateData As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents grpCustomerData As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents lblMitarbeiter As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblKunde As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblZHD As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblEmployeeAddressValue As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblBirthdateValue As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblAdresseKandidat As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblGebDatum As DevExpress.XtraEditors.LabelControl
    Friend WithEvents hlnkEmployee As DevExpress.XtraEditors.HyperLinkEdit
    Friend WithEvents hlnkCustomer As DevExpress.XtraEditors.HyperLinkEdit
    Friend WithEvents lblQualificationValue As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblQualifikation As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblMAStateValue As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblMAStatus As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblCustomerpostcodeValue As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblCustomerStreetValue As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblAdresse As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lueZHD As DevExpress.XtraEditors.GridLookUpEdit
		Friend WithEvents gvZHD As DevExpress.XtraGrid.Views.Grid.GridView
		Friend WithEvents btnSendSMS As DevExpress.XtraEditors.SimpleButton
		Friend WithEvents btnNotice_Employment As DevExpress.XtraEditors.SimpleButton
		Friend WithEvents txtNotice_Employment As DevExpress.XtraEditors.MemoExEdit
		Friend WithEvents txtCustomerNotice_Employment As DevExpress.XtraEditors.MemoExEdit
		Friend WithEvents btnCustomerNotice_Employment As DevExpress.XtraEditors.SimpleButton
	End Class

End Namespace
