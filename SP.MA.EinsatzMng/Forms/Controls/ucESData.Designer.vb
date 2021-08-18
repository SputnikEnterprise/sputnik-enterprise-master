Namespace UI


  <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
  Partial Class ucESData
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
			Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
			Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
			Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
			Dim SerializableAppearanceObject4 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
			Dim SerializableAppearanceObject5 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
			Me.grpESData = New DevComponents.DotNetBar.Controls.GroupPanel()
			Me.XtraScrollableControl2 = New DevExpress.XtraEditors.XtraScrollableControl()
			Me.lblEinsatzAls = New DevExpress.XtraEditors.LabelControl()
			Me.txtRemarks = New DevExpress.XtraEditors.MemoExEdit()
			Me.txtEsAls = New DevExpress.XtraEditors.ComboBoxEdit()
			Me.txtGoesLonger = New DevExpress.XtraEditors.MemoExEdit()
			Me.lblMeldenBei = New DevExpress.XtraEditors.LabelControl()
			Me.dateEditStartDate = New DevExpress.XtraEditors.DateEdit()
			Me.lblArbeitsort = New DevExpress.XtraEditors.LabelControl()
			Me.lblBescheinigungGueltigBis = New DevExpress.XtraEditors.LabelControl()
			Me.lblUhrzeit = New DevExpress.XtraEditors.LabelControl()
			Me.txtReportTo = New DevExpress.XtraEditors.TextEdit()
			Me.txtTime = New DevExpress.XtraEditors.TextEdit()
			Me.txtWorktime = New DevExpress.XtraEditors.TextEdit()
			Me.lblEndetAm = New DevExpress.XtraEditors.LabelControl()
			Me.txtWorkplace = New DevExpress.XtraEditors.TextEdit()
			Me.dateEditEndDate = New DevExpress.XtraEditors.DateEdit()
			Me.lblArbeitszeit = New DevExpress.XtraEditors.LabelControl()
			Me.lblDays = New DevExpress.XtraEditors.LabelControl()
			Me.lblBemerkung = New DevExpress.XtraEditors.LabelControl()
			Me.lblVerlaengert = New DevExpress.XtraEditors.LabelControl()
			Me.lueSuva = New DevExpress.XtraEditors.LookUpEdit()
			Me.lueCurrency = New DevExpress.XtraEditors.LookUpEdit()
			Me.lblSuva = New DevExpress.XtraEditors.LabelControl()
			Me.lblWaehrung = New DevExpress.XtraEditors.LabelControl()
			Me.ErrorProvider = New System.Windows.Forms.ErrorProvider()
			Me.lueESEinstufung = New DevExpress.XtraEditors.LookUpEdit()
			Me.lblBranche = New DevExpress.XtraEditors.LabelControl()
			Me.lblEinsatzEinstufung = New DevExpress.XtraEditors.LabelControl()
			Me.grpSettings = New DevComponents.DotNetBar.Controls.GroupPanel()
			Me.XtraScrollableControl1 = New DevExpress.XtraEditors.XtraScrollableControl()
			Me.chkNoPrintReports = New DevExpress.XtraEditors.CheckEdit()
			Me.chkVerleihvertragBacked = New DevExpress.XtraEditors.CheckEdit()
			Me.chkVeleihVertragPrinted = New DevExpress.XtraEditors.CheckEdit()
			Me.chkDoNotAddInESList = New DevExpress.XtraEditors.CheckEdit()
			Me.chkESBacked = New DevExpress.XtraEditors.CheckEdit()
			Me.chkESPrinted = New DevExpress.XtraEditors.CheckEdit()
			Me.lueBranch = New DevExpress.XtraEditors.GridLookUpEdit()
			Me.gvLueBranch = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.grpESData.SuspendLayout()
			Me.XtraScrollableControl2.SuspendLayout()
			CType(Me.txtRemarks.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.txtEsAls.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.txtGoesLonger.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.dateEditStartDate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.dateEditStartDate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.txtReportTo.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.txtTime.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.txtWorktime.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.txtWorkplace.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.dateEditEndDate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.dateEditEndDate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.lueSuva.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.lueCurrency.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.lueESEinstufung.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.grpSettings.SuspendLayout()
			Me.XtraScrollableControl1.SuspendLayout()
			CType(Me.chkNoPrintReports.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.chkVerleihvertragBacked.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.chkVeleihVertragPrinted.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.chkDoNotAddInESList.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.chkESBacked.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.chkESPrinted.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.lueBranch.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvLueBranch, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			'
			'grpESData
			'
			Me.grpESData.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
							Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.grpESData.BackColor = System.Drawing.Color.White
			Me.grpESData.CanvasColor = System.Drawing.SystemColors.Control
			Me.grpESData.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
			Me.grpESData.Controls.Add(Me.XtraScrollableControl2)
			Me.grpESData.Location = New System.Drawing.Point(7, 1)
			Me.grpESData.Name = "grpESData"
			Me.grpESData.Size = New System.Drawing.Size(670, 143)
			'
			'
			'
			Me.grpESData.Style.BackColorGradientAngle = 90
			Me.grpESData.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpESData.Style.BorderBottomWidth = 1
			Me.grpESData.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
			Me.grpESData.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpESData.Style.BorderLeftWidth = 1
			Me.grpESData.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpESData.Style.BorderRightWidth = 1
			Me.grpESData.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpESData.Style.BorderTopWidth = 1
			Me.grpESData.Style.CornerDiameter = 4
			Me.grpESData.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
			Me.grpESData.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grpESData.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
			Me.grpESData.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
			Me.grpESData.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
			'
			'
			'
			Me.grpESData.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
			'
			'
			'
			Me.grpESData.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
			Me.grpESData.TabIndex = 267
			Me.grpESData.Text = "Einsatzdaten"
			'
			'XtraScrollableControl2
			'
			Me.XtraScrollableControl2.Controls.Add(Me.lblEinsatzAls)
			Me.XtraScrollableControl2.Controls.Add(Me.txtRemarks)
			Me.XtraScrollableControl2.Controls.Add(Me.txtEsAls)
			Me.XtraScrollableControl2.Controls.Add(Me.txtGoesLonger)
			Me.XtraScrollableControl2.Controls.Add(Me.lblMeldenBei)
			Me.XtraScrollableControl2.Controls.Add(Me.dateEditStartDate)
			Me.XtraScrollableControl2.Controls.Add(Me.lblArbeitsort)
			Me.XtraScrollableControl2.Controls.Add(Me.lblBescheinigungGueltigBis)
			Me.XtraScrollableControl2.Controls.Add(Me.lblUhrzeit)
			Me.XtraScrollableControl2.Controls.Add(Me.txtReportTo)
			Me.XtraScrollableControl2.Controls.Add(Me.txtTime)
			Me.XtraScrollableControl2.Controls.Add(Me.txtWorktime)
			Me.XtraScrollableControl2.Controls.Add(Me.lblEndetAm)
			Me.XtraScrollableControl2.Controls.Add(Me.txtWorkplace)
			Me.XtraScrollableControl2.Controls.Add(Me.dateEditEndDate)
			Me.XtraScrollableControl2.Controls.Add(Me.lblArbeitszeit)
			Me.XtraScrollableControl2.Controls.Add(Me.lblDays)
			Me.XtraScrollableControl2.Controls.Add(Me.lblBemerkung)
			Me.XtraScrollableControl2.Controls.Add(Me.lblVerlaengert)
			Me.XtraScrollableControl2.Dock = System.Windows.Forms.DockStyle.Fill
			Me.XtraScrollableControl2.Location = New System.Drawing.Point(0, 0)
			Me.XtraScrollableControl2.Name = "XtraScrollableControl2"
			Me.XtraScrollableControl2.Size = New System.Drawing.Size(664, 121)
			Me.XtraScrollableControl2.TabIndex = 272
			'
			'lblEinsatzAls
			'
			Me.lblEinsatzAls.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblEinsatzAls.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblEinsatzAls.Location = New System.Drawing.Point(17, 14)
			Me.lblEinsatzAls.Name = "lblEinsatzAls"
			Me.lblEinsatzAls.Size = New System.Drawing.Size(96, 13)
			Me.lblEinsatzAls.TabIndex = 244
			Me.lblEinsatzAls.Text = "Einsatz als"
			'
			'txtRemarks
			'
			Me.txtRemarks.Location = New System.Drawing.Point(399, 88)
			Me.txtRemarks.MinimumSize = New System.Drawing.Size(102, 20)
			Me.txtRemarks.Name = "txtRemarks"
			Me.txtRemarks.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.txtRemarks.Properties.ShowIcon = False
			Me.txtRemarks.Size = New System.Drawing.Size(255, 20)
			Me.txtRemarks.TabIndex = 271
			'
			'txtEsAls
			'
			Me.txtEsAls.Location = New System.Drawing.Point(119, 10)
			Me.txtEsAls.Name = "txtEsAls"
			Me.txtEsAls.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
			Me.txtEsAls.Size = New System.Drawing.Size(160, 20)
			Me.txtEsAls.TabIndex = 245
			'
			'txtGoesLonger
			'
			Me.txtGoesLonger.Location = New System.Drawing.Point(399, 62)
			Me.txtGoesLonger.MinimumSize = New System.Drawing.Size(102, 20)
			Me.txtGoesLonger.Name = "txtGoesLonger"
			Me.txtGoesLonger.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.txtGoesLonger.Properties.ShowIcon = False
			Me.txtGoesLonger.Size = New System.Drawing.Size(255, 20)
			Me.txtGoesLonger.TabIndex = 270
			'
			'lblMeldenBei
			'
			Me.lblMeldenBei.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblMeldenBei.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblMeldenBei.Location = New System.Drawing.Point(17, 92)
			Me.lblMeldenBei.Name = "lblMeldenBei"
			Me.lblMeldenBei.Size = New System.Drawing.Size(96, 13)
			Me.lblMeldenBei.TabIndex = 243
			Me.lblMeldenBei.Text = "Melden bei"
			'
			'dateEditStartDate
			'
			Me.dateEditStartDate.EditValue = Nothing
			Me.dateEditStartDate.Location = New System.Drawing.Point(399, 10)
			Me.dateEditStartDate.Name = "dateEditStartDate"
			SerializableAppearanceObject1.ForeColor = System.Drawing.Color.Red
			SerializableAppearanceObject1.Options.UseForeColor = True
			Me.dateEditStartDate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, "", Nothing, Nothing, True)})
			Me.dateEditStartDate.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
			Me.dateEditStartDate.Size = New System.Drawing.Size(104, 20)
			Me.dateEditStartDate.TabIndex = 250
			'
			'lblArbeitsort
			'
			Me.lblArbeitsort.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblArbeitsort.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblArbeitsort.Location = New System.Drawing.Point(17, 66)
			Me.lblArbeitsort.Name = "lblArbeitsort"
			Me.lblArbeitsort.Size = New System.Drawing.Size(96, 13)
			Me.lblArbeitsort.TabIndex = 267
			Me.lblArbeitsort.Text = "Arbeitsort"
			'
			'lblBescheinigungGueltigBis
			'
			Me.lblBescheinigungGueltigBis.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblBescheinigungGueltigBis.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblBescheinigungGueltigBis.Location = New System.Drawing.Point(285, 14)
			Me.lblBescheinigungGueltigBis.Name = "lblBescheinigungGueltigBis"
			Me.lblBescheinigungGueltigBis.Size = New System.Drawing.Size(109, 13)
			Me.lblBescheinigungGueltigBis.TabIndex = 251
			Me.lblBescheinigungGueltigBis.Text = "Beginnt am"
			'
			'lblUhrzeit
			'
			Me.lblUhrzeit.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblUhrzeit.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblUhrzeit.Location = New System.Drawing.Point(531, 14)
			Me.lblUhrzeit.Name = "lblUhrzeit"
			Me.lblUhrzeit.Size = New System.Drawing.Size(59, 13)
			Me.lblUhrzeit.TabIndex = 252
			Me.lblUhrzeit.Text = "Uhrzeit"
			'
			'txtReportTo
			'
			Me.txtReportTo.Location = New System.Drawing.Point(119, 88)
			Me.txtReportTo.Name = "txtReportTo"
			Me.txtReportTo.Size = New System.Drawing.Size(160, 20)
			Me.txtReportTo.TabIndex = 242
			'
			'txtTime
			'
			Me.txtTime.Location = New System.Drawing.Point(596, 10)
			Me.txtTime.Name = "txtTime"
			Me.txtTime.Size = New System.Drawing.Size(58, 20)
			Me.txtTime.TabIndex = 253
			'
			'txtWorktime
			'
			Me.txtWorktime.Location = New System.Drawing.Point(119, 36)
			Me.txtWorktime.Name = "txtWorktime"
			Me.txtWorktime.Size = New System.Drawing.Size(160, 20)
			Me.txtWorktime.TabIndex = 238
			'
			'lblEndetAm
			'
			Me.lblEndetAm.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblEndetAm.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblEndetAm.Location = New System.Drawing.Point(285, 40)
			Me.lblEndetAm.Name = "lblEndetAm"
			Me.lblEndetAm.Size = New System.Drawing.Size(109, 13)
			Me.lblEndetAm.TabIndex = 254
			Me.lblEndetAm.Text = "Endet am"
			'
			'txtWorkplace
			'
			Me.txtWorkplace.Location = New System.Drawing.Point(119, 62)
			Me.txtWorkplace.Name = "txtWorkplace"
			Me.txtWorkplace.Size = New System.Drawing.Size(160, 20)
			Me.txtWorkplace.TabIndex = 240
			'
			'dateEditEndDate
			'
			Me.dateEditEndDate.EditValue = Nothing
			Me.dateEditEndDate.Location = New System.Drawing.Point(399, 36)
			Me.dateEditEndDate.Name = "dateEditEndDate"
			SerializableAppearanceObject2.ForeColor = System.Drawing.Color.Red
			SerializableAppearanceObject2.Options.UseForeColor = True
			Me.dateEditEndDate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject2, "", Nothing, Nothing, True)})
			Me.dateEditEndDate.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
			Me.dateEditEndDate.Size = New System.Drawing.Size(104, 20)
			Me.dateEditEndDate.TabIndex = 255
			'
			'lblArbeitszeit
			'
			Me.lblArbeitszeit.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblArbeitszeit.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblArbeitszeit.Location = New System.Drawing.Point(17, 40)
			Me.lblArbeitszeit.Name = "lblArbeitszeit"
			Me.lblArbeitszeit.Size = New System.Drawing.Size(96, 13)
			Me.lblArbeitszeit.TabIndex = 239
			Me.lblArbeitszeit.Text = "Arbeitszeit"
			'
			'lblDays
			'
			Me.lblDays.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
			Me.lblDays.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblDays.Location = New System.Drawing.Point(509, 40)
			Me.lblDays.Name = "lblDays"
			Me.lblDays.Size = New System.Drawing.Size(136, 13)
			Me.lblDays.TabIndex = 256
			Me.lblDays.Text = "[Tage]"
			'
			'lblBemerkung
			'
			Me.lblBemerkung.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblBemerkung.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblBemerkung.Location = New System.Drawing.Point(284, 92)
			Me.lblBemerkung.Name = "lblBemerkung"
			Me.lblBemerkung.Size = New System.Drawing.Size(109, 13)
			Me.lblBemerkung.TabIndex = 262
			Me.lblBemerkung.Text = "Bemerkung"
			'
			'lblVerlaengert
			'
			Me.lblVerlaengert.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblVerlaengert.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblVerlaengert.Location = New System.Drawing.Point(285, 66)
			Me.lblVerlaengert.Name = "lblVerlaengert"
			Me.lblVerlaengert.Size = New System.Drawing.Size(109, 13)
			Me.lblVerlaengert.TabIndex = 257
			Me.lblVerlaengert.Text = "Verlängert"
			'
			'lueSuva
			'
			Me.lueSuva.Location = New System.Drawing.Point(119, 36)
			Me.lueSuva.Name = "lueSuva"
			SerializableAppearanceObject3.ForeColor = System.Drawing.Color.Red
			SerializableAppearanceObject3.Options.UseForeColor = True
			Me.lueSuva.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject3, "", Nothing, Nothing, True)})
			Me.lueSuva.Properties.ShowFooter = False
			Me.lueSuva.Size = New System.Drawing.Size(160, 20)
			Me.lueSuva.TabIndex = 273
			'
			'lueCurrency
			'
			Me.lueCurrency.Location = New System.Drawing.Point(119, 10)
			Me.lueCurrency.Name = "lueCurrency"
			SerializableAppearanceObject4.ForeColor = System.Drawing.Color.Red
			SerializableAppearanceObject4.Options.UseForeColor = True
			Me.lueCurrency.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject4, "", Nothing, Nothing, True)})
			Me.lueCurrency.Properties.ShowFooter = False
			Me.lueCurrency.Size = New System.Drawing.Size(160, 20)
			Me.lueCurrency.TabIndex = 272
			'
			'lblSuva
			'
			Me.lblSuva.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblSuva.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblSuva.Location = New System.Drawing.Point(17, 40)
			Me.lblSuva.Name = "lblSuva"
			Me.lblSuva.Size = New System.Drawing.Size(96, 13)
			Me.lblSuva.TabIndex = 269
			Me.lblSuva.Text = "Suva"
			'
			'lblWaehrung
			'
			Me.lblWaehrung.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblWaehrung.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblWaehrung.Location = New System.Drawing.Point(17, 14)
			Me.lblWaehrung.Name = "lblWaehrung"
			Me.lblWaehrung.Size = New System.Drawing.Size(96, 13)
			Me.lblWaehrung.TabIndex = 263
			Me.lblWaehrung.Text = "Währung"
			'
			'ErrorProvider
			'
			Me.ErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
			Me.ErrorProvider.ContainerControl = Me
			'
			'lueESEinstufung
			'
			Me.lueESEinstufung.Location = New System.Drawing.Point(119, 62)
			Me.lueESEinstufung.Name = "lueESEinstufung"
			SerializableAppearanceObject5.ForeColor = System.Drawing.Color.Red
			SerializableAppearanceObject5.Options.UseForeColor = True
			Me.lueESEinstufung.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject5, "", Nothing, Nothing, True)})
			Me.lueESEinstufung.Properties.ShowFooter = False
			Me.lueESEinstufung.Size = New System.Drawing.Size(160, 20)
			Me.lueESEinstufung.TabIndex = 276
			'
			'lblBranche
			'
			Me.lblBranche.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblBranche.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
			Me.lblBranche.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblBranche.Location = New System.Drawing.Point(17, 91)
			Me.lblBranche.Name = "lblBranche"
			Me.lblBranche.Size = New System.Drawing.Size(96, 27)
			Me.lblBranche.TabIndex = 275
			Me.lblBranche.Text = "es_branche"
			'
			'lblEinsatzEinstufung
			'
			Me.lblEinsatzEinstufung.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblEinsatzEinstufung.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblEinsatzEinstufung.Location = New System.Drawing.Point(17, 66)
			Me.lblEinsatzEinstufung.Name = "lblEinsatzEinstufung"
			Me.lblEinsatzEinstufung.Size = New System.Drawing.Size(96, 13)
			Me.lblEinsatzEinstufung.TabIndex = 274
			Me.lblEinsatzEinstufung.Text = "es_einstufung"
			'
			'grpSettings
			'
			Me.grpSettings.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
							Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.grpSettings.BackColor = System.Drawing.Color.White
			Me.grpSettings.CanvasColor = System.Drawing.SystemColors.Control
			Me.grpSettings.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
			Me.grpSettings.Controls.Add(Me.XtraScrollableControl1)
			Me.grpSettings.Location = New System.Drawing.Point(7, 154)
			Me.grpSettings.Name = "grpSettings"
			Me.grpSettings.Size = New System.Drawing.Size(670, 146)
			'
			'
			'
			Me.grpSettings.Style.BackColorGradientAngle = 90
			Me.grpSettings.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpSettings.Style.BorderBottomWidth = 1
			Me.grpSettings.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
			Me.grpSettings.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpSettings.Style.BorderLeftWidth = 1
			Me.grpSettings.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpSettings.Style.BorderRightWidth = 1
			Me.grpSettings.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpSettings.Style.BorderTopWidth = 1
			Me.grpSettings.Style.CornerDiameter = 4
			Me.grpSettings.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
			Me.grpSettings.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grpSettings.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
			Me.grpSettings.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
			Me.grpSettings.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
			'
			'
			'
			Me.grpSettings.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
			'
			'
			'
			Me.grpSettings.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
			Me.grpSettings.TabIndex = 268
			Me.grpSettings.Text = "Einstellungen"
			'
			'XtraScrollableControl1
			'
			Me.XtraScrollableControl1.Controls.Add(Me.chkNoPrintReports)
			Me.XtraScrollableControl1.Controls.Add(Me.lblWaehrung)
			Me.XtraScrollableControl1.Controls.Add(Me.chkVerleihvertragBacked)
			Me.XtraScrollableControl1.Controls.Add(Me.chkVeleihVertragPrinted)
			Me.XtraScrollableControl1.Controls.Add(Me.lblBranche)
			Me.XtraScrollableControl1.Controls.Add(Me.chkDoNotAddInESList)
			Me.XtraScrollableControl1.Controls.Add(Me.lblEinsatzEinstufung)
			Me.XtraScrollableControl1.Controls.Add(Me.chkESBacked)
			Me.XtraScrollableControl1.Controls.Add(Me.lueESEinstufung)
			Me.XtraScrollableControl1.Controls.Add(Me.chkESPrinted)
			Me.XtraScrollableControl1.Controls.Add(Me.lblSuva)
			Me.XtraScrollableControl1.Controls.Add(Me.lueCurrency)
			Me.XtraScrollableControl1.Controls.Add(Me.lueBranch)
			Me.XtraScrollableControl1.Controls.Add(Me.lueSuva)
			Me.XtraScrollableControl1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.XtraScrollableControl1.Location = New System.Drawing.Point(0, 0)
			Me.XtraScrollableControl1.Name = "XtraScrollableControl1"
			Me.XtraScrollableControl1.Size = New System.Drawing.Size(664, 124)
			Me.XtraScrollableControl1.TabIndex = 283
			'
			'chkNoPrintReports
			'
			Me.chkNoPrintReports.Location = New System.Drawing.Point(399, 74)
			Me.chkNoPrintReports.Name = "chkNoPrintReports"
			Me.chkNoPrintReports.Properties.AllowFocused = False
			Me.chkNoPrintReports.Properties.Appearance.Options.UseTextOptions = True
			Me.chkNoPrintReports.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.chkNoPrintReports.Properties.Caption = "Rapporte nicht drucken"
			Me.chkNoPrintReports.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.chkNoPrintReports.Size = New System.Drawing.Size(255, 19)
			Me.chkNoPrintReports.TabIndex = 283
			'
			'chkVerleihvertragBacked
			'
			Me.chkVerleihvertragBacked.Location = New System.Drawing.Point(306, 11)
			Me.chkVerleihvertragBacked.Name = "chkVerleihvertragBacked"
			Me.chkVerleihvertragBacked.Properties.AllowFocused = False
			Me.chkVerleihvertragBacked.Properties.Appearance.Options.UseTextOptions = True
			Me.chkVerleihvertragBacked.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.chkVerleihvertragBacked.Properties.Caption = "Verleihvertrag zurückgenommen"
			Me.chkVerleihvertragBacked.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.chkVerleihvertragBacked.Size = New System.Drawing.Size(181, 19)
			Me.chkVerleihvertragBacked.TabIndex = 282
			'
			'chkVeleihVertragPrinted
			'
			Me.chkVeleihVertragPrinted.Location = New System.Drawing.Point(493, 11)
			Me.chkVeleihVertragPrinted.Name = "chkVeleihVertragPrinted"
			Me.chkVeleihVertragPrinted.Properties.AllowFocused = False
			Me.chkVeleihVertragPrinted.Properties.Appearance.Options.UseTextOptions = True
			Me.chkVeleihVertragPrinted.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.chkVeleihVertragPrinted.Properties.Caption = "Verleihvertrag gedruckt"
			Me.chkVeleihVertragPrinted.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.chkVeleihVertragPrinted.Size = New System.Drawing.Size(161, 19)
			Me.chkVeleihVertragPrinted.TabIndex = 281
			'
			'chkDoNotAddInESList
			'
			Me.chkDoNotAddInESList.Location = New System.Drawing.Point(399, 99)
			Me.chkDoNotAddInESList.Name = "chkDoNotAddInESList"
			Me.chkDoNotAddInESList.Properties.AllowFocused = False
			Me.chkDoNotAddInESList.Properties.Appearance.Options.UseTextOptions = True
			Me.chkDoNotAddInESList.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.chkDoNotAddInESList.Properties.Caption = "Nicht in der Einsatzliste auflisten"
			Me.chkDoNotAddInESList.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.chkDoNotAddInESList.Size = New System.Drawing.Size(255, 19)
			Me.chkDoNotAddInESList.TabIndex = 280
			'
			'chkESBacked
			'
			Me.chkESBacked.Location = New System.Drawing.Point(306, 34)
			Me.chkESBacked.Name = "chkESBacked"
			Me.chkESBacked.Properties.AllowFocused = False
			Me.chkESBacked.Properties.Appearance.Options.UseTextOptions = True
			Me.chkESBacked.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.chkESBacked.Properties.Caption = "Einsatzvertrag zurückgenommen"
			Me.chkESBacked.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.chkESBacked.Size = New System.Drawing.Size(181, 19)
			Me.chkESBacked.TabIndex = 279
			'
			'chkESPrinted
			'
			Me.chkESPrinted.Location = New System.Drawing.Point(493, 34)
			Me.chkESPrinted.Name = "chkESPrinted"
			Me.chkESPrinted.Properties.AllowFocused = False
			Me.chkESPrinted.Properties.Appearance.Options.UseTextOptions = True
			Me.chkESPrinted.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.chkESPrinted.Properties.Caption = "Einsatzvertrag gedruckt"
			Me.chkESPrinted.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.chkESPrinted.Size = New System.Drawing.Size(161, 19)
			Me.chkESPrinted.TabIndex = 278
			'
			'lueBranch
			'
			Me.lueBranch.Location = New System.Drawing.Point(119, 88)
			Me.lueBranch.Name = "lueBranch"
			Me.lueBranch.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.lueBranch.Properties.View = Me.gvLueBranch
			Me.lueBranch.Size = New System.Drawing.Size(160, 20)
			Me.lueBranch.TabIndex = 277
			'
			'gvLueBranch
			'
			Me.gvLueBranch.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			Me.gvLueBranch.Name = "gvLueBranch"
			Me.gvLueBranch.OptionsSelection.EnableAppearanceFocusedCell = False
			Me.gvLueBranch.OptionsView.ShowGroupPanel = False
			'
			'ucESData
			'
			Me.Appearance.BackColor = System.Drawing.Color.Transparent
			Me.Appearance.Options.UseBackColor = True
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.Controls.Add(Me.grpSettings)
			Me.Controls.Add(Me.grpESData)
			Me.Name = "ucESData"
			Me.Size = New System.Drawing.Size(682, 305)
			Me.grpESData.ResumeLayout(False)
			Me.XtraScrollableControl2.ResumeLayout(False)
			CType(Me.txtRemarks.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.txtEsAls.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.txtGoesLonger.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.dateEditStartDate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.dateEditStartDate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.txtReportTo.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.txtTime.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.txtWorktime.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.txtWorkplace.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.dateEditEndDate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.dateEditEndDate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.lueSuva.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.lueCurrency.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.lueESEinstufung.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			Me.grpSettings.ResumeLayout(False)
			Me.XtraScrollableControl1.ResumeLayout(False)
			CType(Me.chkNoPrintReports.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.chkVerleihvertragBacked.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.chkVeleihVertragPrinted.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.chkDoNotAddInESList.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.chkESBacked.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.chkESPrinted.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.lueBranch.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvLueBranch, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub
    Friend WithEvents lblArbeitszeit As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtWorktime As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtWorkplace As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtReportTo As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblMeldenBei As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblEinsatzAls As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtEsAls As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents lblBescheinigungGueltigBis As DevExpress.XtraEditors.LabelControl
    Friend WithEvents dateEditStartDate As DevExpress.XtraEditors.DateEdit
    Friend WithEvents lblUhrzeit As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtTime As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblEndetAm As DevExpress.XtraEditors.LabelControl
    Friend WithEvents dateEditEndDate As DevExpress.XtraEditors.DateEdit
    Friend WithEvents lblDays As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblVerlaengert As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblBemerkung As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblWaehrung As DevExpress.XtraEditors.LabelControl
    Friend WithEvents grpESData As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents lblArbeitsort As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblSuva As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtGoesLonger As DevExpress.XtraEditors.MemoExEdit
    Friend WithEvents txtRemarks As DevExpress.XtraEditors.MemoExEdit
    Friend WithEvents lueCurrency As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents lueSuva As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents ErrorProvider As System.Windows.Forms.ErrorProvider
    Friend WithEvents lueESEinstufung As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents lblBranche As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblEinsatzEinstufung As DevExpress.XtraEditors.LabelControl
    Friend WithEvents grpSettings As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents lueBranch As DevExpress.XtraEditors.GridLookUpEdit
    Friend WithEvents gvLueBranch As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents chkVerleihvertragBacked As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents chkVeleihVertragPrinted As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents chkDoNotAddInESList As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents chkESBacked As DevExpress.XtraEditors.CheckEdit
		Friend WithEvents chkESPrinted As DevExpress.XtraEditors.CheckEdit
		Friend WithEvents XtraScrollableControl1 As DevExpress.XtraEditors.XtraScrollableControl
    Friend WithEvents XtraScrollableControl2 As DevExpress.XtraEditors.XtraScrollableControl
    Friend WithEvents chkNoPrintReports As DevExpress.XtraEditors.CheckEdit

  End Class

End Namespace
