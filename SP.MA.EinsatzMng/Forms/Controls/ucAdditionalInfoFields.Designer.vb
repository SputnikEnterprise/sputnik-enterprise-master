Namespace UI


  <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
  Partial Class ucAdditionalInfoFields
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
			Me.lblESBeurteilung = New DevExpress.XtraEditors.LabelControl()
			Me.lblEinsatzvertrag = New DevExpress.XtraEditors.LabelControl()
			Me.lblZusatz1 = New DevExpress.XtraEditors.LabelControl()
			Me.lblZusatz2 = New DevExpress.XtraEditors.LabelControl()
			Me.lblZusatz3 = New DevExpress.XtraEditors.LabelControl()
			Me.lblZusatz4 = New DevExpress.XtraEditors.LabelControl()
			Me.lblZusatz5 = New DevExpress.XtraEditors.LabelControl()
			Me.lblZusatz6 = New DevExpress.XtraEditors.LabelControl()
			Me.txtAdditionalText4 = New DevExpress.XtraEditors.TextEdit()
			Me.txtAdditionalText3 = New DevExpress.XtraEditors.TextEdit()
			Me.txtAdditionalText2 = New DevExpress.XtraEditors.TextEdit()
			Me.txtAdditionalText1 = New DevExpress.XtraEditors.TextEdit()
			Me.txtAdditionalText5 = New DevExpress.XtraEditors.TextEdit()
			Me.txtAdditionalText6 = New DevExpress.XtraEditors.TextEdit()
			Me.txtEinsatzvertrag = New DevExpress.XtraEditors.TextEdit()
			Me.txtVerleihvertrag = New DevExpress.XtraEditors.TextEdit()
			Me.lblVerleihvertrag = New DevExpress.XtraEditors.LabelControl()
			Me.memoEsBeurteilung = New DevExpress.XtraEditors.MemoEdit()
			Me.xtabAdditionalInfoFields = New DevExpress.XtraTab.XtraTabControl()
			Me.xtabZusatzFelder = New DevExpress.XtraTab.XtraTabPage()
			Me.XtraScrollableControl1 = New DevExpress.XtraEditors.XtraScrollableControl()
			Me.xtabZVARG = New DevExpress.XtraTab.XtraTabPage()
			Me.PanelControl2 = New DevExpress.XtraEditors.PanelControl()
			Me.lueArt = New DevExpress.XtraEditors.LookUpEdit()
			Me.dateEditam = New DevExpress.XtraEditors.DateEdit()
			Me.dateEditauf = New DevExpress.XtraEditors.DateEdit()
			Me.lblErfolgtam = New DevExpress.XtraEditors.LabelControl()
			Me.lblAuf = New DevExpress.XtraEditors.LabelControl()
			Me.lblArtderKuendigung = New DevExpress.XtraEditors.LabelControl()
			Me.lblBegruendung = New DevExpress.XtraEditors.LabelControl()
			Me.lblKuendungdurch = New DevExpress.XtraEditors.LabelControl()
			Me.luedismissalwho = New DevExpress.XtraEditors.LookUpEdit()
			Me.txtdismissalreason = New DevExpress.XtraEditors.MemoExEdit()
			CType(Me.txtAdditionalText4.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.txtAdditionalText3.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.txtAdditionalText2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.txtAdditionalText1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.txtAdditionalText5.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.txtAdditionalText6.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.txtEinsatzvertrag.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.txtVerleihvertrag.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.memoEsBeurteilung.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.xtabAdditionalInfoFields, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.xtabAdditionalInfoFields.SuspendLayout()
			Me.xtabZusatzFelder.SuspendLayout()
			Me.XtraScrollableControl1.SuspendLayout()
			Me.xtabZVARG.SuspendLayout()
			CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.PanelControl2.SuspendLayout()
			CType(Me.lueArt.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.dateEditam.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.dateEditam.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.dateEditauf.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.dateEditauf.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.luedismissalwho.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.txtdismissalreason.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			'
			'lblESBeurteilung
			'
			Me.lblESBeurteilung.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblESBeurteilung.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblESBeurteilung.Location = New System.Drawing.Point(13, 19)
			Me.lblESBeurteilung.Name = "lblESBeurteilung"
			Me.lblESBeurteilung.Size = New System.Drawing.Size(111, 13)
			Me.lblESBeurteilung.TabIndex = 241
			Me.lblESBeurteilung.Text = "ES-Beurteilung"
			'
			'lblEinsatzvertrag
			'
			Me.lblEinsatzvertrag.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblEinsatzvertrag.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblEinsatzvertrag.Location = New System.Drawing.Point(13, 61)
			Me.lblEinsatzvertrag.Name = "lblEinsatzvertrag"
			Me.lblEinsatzvertrag.Size = New System.Drawing.Size(111, 13)
			Me.lblEinsatzvertrag.TabIndex = 243
			Me.lblEinsatzvertrag.Text = "Einsatzvertrag"
			'
			'lblZusatz1
			'
			Me.lblZusatz1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblZusatz1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblZusatz1.Location = New System.Drawing.Point(10, 23)
			Me.lblZusatz1.Name = "lblZusatz1"
			Me.lblZusatz1.Size = New System.Drawing.Size(100, 13)
			Me.lblZusatz1.TabIndex = 244
			Me.lblZusatz1.Text = "ES_1Zusatz"
			'
			'lblZusatz2
			'
			Me.lblZusatz2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblZusatz2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblZusatz2.Location = New System.Drawing.Point(10, 48)
			Me.lblZusatz2.Name = "lblZusatz2"
			Me.lblZusatz2.Size = New System.Drawing.Size(100, 13)
			Me.lblZusatz2.TabIndex = 245
			Me.lblZusatz2.Text = "ES_2Zusatz"
			'
			'lblZusatz3
			'
			Me.lblZusatz3.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblZusatz3.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblZusatz3.Location = New System.Drawing.Point(10, 74)
			Me.lblZusatz3.Name = "lblZusatz3"
			Me.lblZusatz3.Size = New System.Drawing.Size(100, 13)
			Me.lblZusatz3.TabIndex = 246
			Me.lblZusatz3.Text = "ES_3Zusatz"
			'
			'lblZusatz4
			'
			Me.lblZusatz4.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblZusatz4.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblZusatz4.Location = New System.Drawing.Point(10, 100)
			Me.lblZusatz4.Name = "lblZusatz4"
			Me.lblZusatz4.Size = New System.Drawing.Size(100, 13)
			Me.lblZusatz4.TabIndex = 247
			Me.lblZusatz4.Text = "ES_4Zusatz"
			'
			'lblZusatz5
			'
			Me.lblZusatz5.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblZusatz5.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblZusatz5.Location = New System.Drawing.Point(10, 126)
			Me.lblZusatz5.Name = "lblZusatz5"
			Me.lblZusatz5.Size = New System.Drawing.Size(100, 13)
			Me.lblZusatz5.TabIndex = 248
			Me.lblZusatz5.Text = "ES_5Zusatz"
			'
			'lblZusatz6
			'
			Me.lblZusatz6.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblZusatz6.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblZusatz6.Location = New System.Drawing.Point(10, 152)
			Me.lblZusatz6.Name = "lblZusatz6"
			Me.lblZusatz6.Size = New System.Drawing.Size(100, 13)
			Me.lblZusatz6.TabIndex = 249
			Me.lblZusatz6.Text = "ES_6Zusatz"
			'
			'txtAdditionalText4
			'
			Me.txtAdditionalText4.Location = New System.Drawing.Point(116, 97)
			Me.txtAdditionalText4.Name = "txtAdditionalText4"
			Me.txtAdditionalText4.Size = New System.Drawing.Size(558, 20)
			Me.txtAdditionalText4.TabIndex = 250
			'
			'txtAdditionalText3
			'
			Me.txtAdditionalText3.Location = New System.Drawing.Point(116, 71)
			Me.txtAdditionalText3.Name = "txtAdditionalText3"
			Me.txtAdditionalText3.Size = New System.Drawing.Size(558, 20)
			Me.txtAdditionalText3.TabIndex = 251
			'
			'txtAdditionalText2
			'
			Me.txtAdditionalText2.Location = New System.Drawing.Point(116, 45)
			Me.txtAdditionalText2.Name = "txtAdditionalText2"
			Me.txtAdditionalText2.Size = New System.Drawing.Size(558, 20)
			Me.txtAdditionalText2.TabIndex = 252
			'
			'txtAdditionalText1
			'
			Me.txtAdditionalText1.Location = New System.Drawing.Point(116, 19)
			Me.txtAdditionalText1.Name = "txtAdditionalText1"
			Me.txtAdditionalText1.Size = New System.Drawing.Size(558, 20)
			Me.txtAdditionalText1.TabIndex = 253
			'
			'txtAdditionalText5
			'
			Me.txtAdditionalText5.Location = New System.Drawing.Point(116, 123)
			Me.txtAdditionalText5.Name = "txtAdditionalText5"
			Me.txtAdditionalText5.Size = New System.Drawing.Size(558, 20)
			Me.txtAdditionalText5.TabIndex = 254
			'
			'txtAdditionalText6
			'
			Me.txtAdditionalText6.Location = New System.Drawing.Point(116, 149)
			Me.txtAdditionalText6.Name = "txtAdditionalText6"
			Me.txtAdditionalText6.Size = New System.Drawing.Size(558, 20)
			Me.txtAdditionalText6.TabIndex = 255
			'
			'txtEinsatzvertrag
			'
			Me.txtEinsatzvertrag.Location = New System.Drawing.Point(130, 57)
			Me.txtEinsatzvertrag.Name = "txtEinsatzvertrag"
			Me.txtEinsatzvertrag.Size = New System.Drawing.Size(558, 20)
			Me.txtEinsatzvertrag.TabIndex = 256
			'
			'txtVerleihvertrag
			'
			Me.txtVerleihvertrag.Location = New System.Drawing.Point(130, 83)
			Me.txtVerleihvertrag.Name = "txtVerleihvertrag"
			Me.txtVerleihvertrag.Size = New System.Drawing.Size(558, 20)
			Me.txtVerleihvertrag.TabIndex = 257
			'
			'lblVerleihvertrag
			'
			Me.lblVerleihvertrag.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblVerleihvertrag.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblVerleihvertrag.Location = New System.Drawing.Point(13, 87)
			Me.lblVerleihvertrag.Name = "lblVerleihvertrag"
			Me.lblVerleihvertrag.Size = New System.Drawing.Size(111, 13)
			Me.lblVerleihvertrag.TabIndex = 258
			Me.lblVerleihvertrag.Text = "Verleihvertrag"
			'
			'memoEsBeurteilung
			'
			Me.memoEsBeurteilung.Location = New System.Drawing.Point(130, 19)
			Me.memoEsBeurteilung.Name = "memoEsBeurteilung"
			Me.memoEsBeurteilung.Size = New System.Drawing.Size(558, 32)
			Me.memoEsBeurteilung.TabIndex = 260
			Me.memoEsBeurteilung.UseOptimizedRendering = True
			'
			'xtabAdditionalInfoFields
			'
			Me.xtabAdditionalInfoFields.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
							Or System.Windows.Forms.AnchorStyles.Left) _
							Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.xtabAdditionalInfoFields.Location = New System.Drawing.Point(13, 109)
			Me.xtabAdditionalInfoFields.Name = "xtabAdditionalInfoFields"
			Me.xtabAdditionalInfoFields.SelectedTabPage = Me.xtabZusatzFelder
			Me.xtabAdditionalInfoFields.Size = New System.Drawing.Size(712, 208)
			Me.xtabAdditionalInfoFields.TabIndex = 261
			Me.xtabAdditionalInfoFields.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabZusatzFelder, Me.xtabZVARG})
			'
			'xtabZusatzFelder
			'
			Me.xtabZusatzFelder.Controls.Add(Me.XtraScrollableControl1)
			Me.xtabZusatzFelder.Name = "xtabZusatzFelder"
			Me.xtabZusatzFelder.Size = New System.Drawing.Size(706, 180)
			Me.xtabZusatzFelder.Text = "Zusatzfelder"
			'
			'XtraScrollableControl1
			'
			Me.XtraScrollableControl1.Controls.Add(Me.txtAdditionalText1)
			Me.XtraScrollableControl1.Controls.Add(Me.lblZusatz1)
			Me.XtraScrollableControl1.Controls.Add(Me.txtAdditionalText3)
			Me.XtraScrollableControl1.Controls.Add(Me.lblZusatz2)
			Me.XtraScrollableControl1.Controls.Add(Me.txtAdditionalText2)
			Me.XtraScrollableControl1.Controls.Add(Me.lblZusatz3)
			Me.XtraScrollableControl1.Controls.Add(Me.txtAdditionalText4)
			Me.XtraScrollableControl1.Controls.Add(Me.lblZusatz4)
			Me.XtraScrollableControl1.Controls.Add(Me.lblZusatz6)
			Me.XtraScrollableControl1.Controls.Add(Me.txtAdditionalText6)
			Me.XtraScrollableControl1.Controls.Add(Me.txtAdditionalText5)
			Me.XtraScrollableControl1.Controls.Add(Me.lblZusatz5)
			Me.XtraScrollableControl1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.XtraScrollableControl1.Location = New System.Drawing.Point(0, 0)
			Me.XtraScrollableControl1.Name = "XtraScrollableControl1"
			Me.XtraScrollableControl1.Size = New System.Drawing.Size(706, 180)
			Me.XtraScrollableControl1.TabIndex = 256
			'
			'xtabZVARG
			'
			Me.xtabZVARG.Controls.Add(Me.PanelControl2)
			Me.xtabZVARG.Name = "xtabZVARG"
			Me.xtabZVARG.Size = New System.Drawing.Size(706, 180)
			Me.xtabZVARG.Text = "Kündigungsfelder für ZV und Arbeitgeberbescheinigung"
			'
			'PanelControl2
			'
			Me.PanelControl2.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(236, Byte), Integer), CType(CType(239, Byte), Integer))
			Me.PanelControl2.Appearance.Options.UseBackColor = True
			Me.PanelControl2.Controls.Add(Me.lueArt)
			Me.PanelControl2.Controls.Add(Me.dateEditam)
			Me.PanelControl2.Controls.Add(Me.dateEditauf)
			Me.PanelControl2.Controls.Add(Me.lblErfolgtam)
			Me.PanelControl2.Controls.Add(Me.lblAuf)
			Me.PanelControl2.Controls.Add(Me.lblArtderKuendigung)
			Me.PanelControl2.Controls.Add(Me.lblBegruendung)
			Me.PanelControl2.Controls.Add(Me.lblKuendungdurch)
			Me.PanelControl2.Controls.Add(Me.luedismissalwho)
			Me.PanelControl2.Controls.Add(Me.txtdismissalreason)
			Me.PanelControl2.Dock = System.Windows.Forms.DockStyle.Fill
			Me.PanelControl2.Location = New System.Drawing.Point(0, 0)
			Me.PanelControl2.Name = "PanelControl2"
			Me.PanelControl2.Padding = New System.Windows.Forms.Padding(5)
			Me.PanelControl2.Size = New System.Drawing.Size(706, 180)
			Me.PanelControl2.TabIndex = 1
			'
			'lueArt
			'
			Me.lueArt.Location = New System.Drawing.Point(116, 45)
			Me.lueArt.Name = "lueArt"
			SerializableAppearanceObject1.ForeColor = System.Drawing.Color.Red
			SerializableAppearanceObject1.Options.UseForeColor = True
			Me.lueArt.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, "", Nothing, Nothing, True)})
			Me.lueArt.Properties.ShowFooter = False
			Me.lueArt.Size = New System.Drawing.Size(298, 20)
			Me.lueArt.TabIndex = 277
			'
			'dateEditam
			'
			Me.dateEditam.EditValue = Nothing
			Me.dateEditam.Location = New System.Drawing.Point(116, 19)
			Me.dateEditam.Name = "dateEditam"
			SerializableAppearanceObject2.ForeColor = System.Drawing.Color.Red
			SerializableAppearanceObject2.Options.UseForeColor = True
			Me.dateEditam.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject2, "", Nothing, Nothing, True)})
			Me.dateEditam.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
			Me.dateEditam.Size = New System.Drawing.Size(104, 20)
			Me.dateEditam.TabIndex = 256
			'
			'dateEditauf
			'
			Me.dateEditauf.EditValue = Nothing
			Me.dateEditauf.Location = New System.Drawing.Point(310, 19)
			Me.dateEditauf.Name = "dateEditauf"
			SerializableAppearanceObject3.ForeColor = System.Drawing.Color.Red
			SerializableAppearanceObject3.Options.UseForeColor = True
			Me.dateEditauf.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject3, "", Nothing, Nothing, True)})
			Me.dateEditauf.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
			Me.dateEditauf.Size = New System.Drawing.Size(104, 20)
			Me.dateEditauf.TabIndex = 257
			'
			'lblErfolgtam
			'
			Me.lblErfolgtam.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblErfolgtam.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblErfolgtam.Location = New System.Drawing.Point(11, 23)
			Me.lblErfolgtam.Name = "lblErfolgtam"
			Me.lblErfolgtam.Size = New System.Drawing.Size(99, 13)
			Me.lblErfolgtam.TabIndex = 244
			Me.lblErfolgtam.Text = "Erfolgte am"
			'
			'lblAuf
			'
			Me.lblAuf.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblAuf.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblAuf.Location = New System.Drawing.Point(239, 23)
			Me.lblAuf.Name = "lblAuf"
			Me.lblAuf.Size = New System.Drawing.Size(65, 13)
			Me.lblAuf.TabIndex = 245
			Me.lblAuf.Text = "Auf"
			'
			'lblArtderKuendigung
			'
			Me.lblArtderKuendigung.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblArtderKuendigung.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblArtderKuendigung.Location = New System.Drawing.Point(11, 49)
			Me.lblArtderKuendigung.Name = "lblArtderKuendigung"
			Me.lblArtderKuendigung.Size = New System.Drawing.Size(99, 13)
			Me.lblArtderKuendigung.TabIndex = 246
			Me.lblArtderKuendigung.Text = "Art der Kündigung"
			'
			'lblBegruendung
			'
			Me.lblBegruendung.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblBegruendung.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblBegruendung.Location = New System.Drawing.Point(11, 75)
			Me.lblBegruendung.Name = "lblBegruendung"
			Me.lblBegruendung.Size = New System.Drawing.Size(99, 13)
			Me.lblBegruendung.TabIndex = 247
			Me.lblBegruendung.Text = "Begründung"
			'
			'lblKuendungdurch
			'
			Me.lblKuendungdurch.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblKuendungdurch.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblKuendungdurch.Location = New System.Drawing.Point(11, 101)
			Me.lblKuendungdurch.Name = "lblKuendungdurch"
			Me.lblKuendungdurch.Size = New System.Drawing.Size(99, 13)
			Me.lblKuendungdurch.TabIndex = 248
			Me.lblKuendungdurch.Text = "Kündigung durch"
			'
			'luedismissalwho
			'
			Me.luedismissalwho.Location = New System.Drawing.Point(116, 97)
			Me.luedismissalwho.Name = "luedismissalwho"
			SerializableAppearanceObject4.ForeColor = System.Drawing.Color.Red
			SerializableAppearanceObject4.Options.UseForeColor = True
			Me.luedismissalwho.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject4, "", Nothing, Nothing, True)})
			Me.luedismissalwho.Properties.NullText = ""
			Me.luedismissalwho.Size = New System.Drawing.Size(298, 20)
			Me.luedismissalwho.TabIndex = 252
			'
			'txtdismissalreason
			'
			Me.txtdismissalreason.EditValue = ""
			Me.txtdismissalreason.Location = New System.Drawing.Point(116, 71)
			Me.txtdismissalreason.Name = "txtdismissalreason"
			Me.txtdismissalreason.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.txtdismissalreason.Properties.HideSelection = False
			Me.txtdismissalreason.Properties.PopupResizeMode = DevExpress.XtraEditors.Controls.ResizeMode.FrameResize
			Me.txtdismissalreason.Size = New System.Drawing.Size(558, 20)
			Me.txtdismissalreason.TabIndex = 253
			'
			'ucAdditionalInfoFields
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.Controls.Add(Me.xtabAdditionalInfoFields)
			Me.Controls.Add(Me.memoEsBeurteilung)
			Me.Controls.Add(Me.lblVerleihvertrag)
			Me.Controls.Add(Me.txtVerleihvertrag)
			Me.Controls.Add(Me.txtEinsatzvertrag)
			Me.Controls.Add(Me.lblEinsatzvertrag)
			Me.Controls.Add(Me.lblESBeurteilung)
			Me.Name = "ucAdditionalInfoFields"
			Me.Size = New System.Drawing.Size(735, 324)
			CType(Me.txtAdditionalText4.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.txtAdditionalText3.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.txtAdditionalText2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.txtAdditionalText1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.txtAdditionalText5.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.txtAdditionalText6.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.txtEinsatzvertrag.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.txtVerleihvertrag.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.memoEsBeurteilung.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.xtabAdditionalInfoFields, System.ComponentModel.ISupportInitialize).EndInit()
			Me.xtabAdditionalInfoFields.ResumeLayout(False)
			Me.xtabZusatzFelder.ResumeLayout(False)
			Me.XtraScrollableControl1.ResumeLayout(False)
			Me.xtabZVARG.ResumeLayout(False)
			CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).EndInit()
			Me.PanelControl2.ResumeLayout(False)
			CType(Me.lueArt.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.dateEditam.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.dateEditam.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.dateEditauf.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.dateEditauf.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.luedismissalwho.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.txtdismissalreason.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub
    Friend WithEvents lblESBeurteilung As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblEinsatzvertrag As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblZusatz1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblZusatz2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblZusatz3 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblZusatz4 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblZusatz5 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblZusatz6 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtAdditionalText4 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtAdditionalText3 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtAdditionalText2 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtAdditionalText1 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtAdditionalText5 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtAdditionalText6 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtEinsatzvertrag As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtVerleihvertrag As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblVerleihvertrag As DevExpress.XtraEditors.LabelControl
    Friend WithEvents memoEsBeurteilung As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents xtabAdditionalInfoFields As DevExpress.XtraTab.XtraTabControl
    Friend WithEvents xtabZusatzFelder As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents xtabZVARG As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents PanelControl2 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblErfolgtam As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblAuf As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblArtderKuendigung As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblBegruendung As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblKuendungdurch As DevExpress.XtraEditors.LabelControl
    Friend WithEvents dateEditam As DevExpress.XtraEditors.DateEdit
    Friend WithEvents dateEditauf As DevExpress.XtraEditors.DateEdit
    Friend WithEvents lueArt As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents luedismissalwho As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents txtdismissalreason As DevExpress.XtraEditors.MemoExEdit
    Friend WithEvents XtraScrollableControl1 As DevExpress.XtraEditors.XtraScrollableControl

  End Class

End Namespace
