Namespace UI

  <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
  Partial Class ucPageEmployeeAdditionalData1
    Inherits ucWizardPageBaseControl

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
			Me.components = New System.ComponentModel.Container()
			Dim SerializableAppearanceObject5 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
			Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
			Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
			Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
			Me.chkDStellen = New DevExpress.XtraEditors.CheckEdit()
			Me.chkESLock = New DevExpress.XtraEditors.CheckEdit()
			Me.lueState2 = New DevExpress.XtraEditors.LookUpEdit()
			Me.lueState1 = New DevExpress.XtraEditors.LookUpEdit()
			Me.lueContactInfo = New DevExpress.XtraEditors.LookUpEdit()
			Me.lblkontakt = New DevExpress.XtraEditors.LabelControl()
			Me.lbl2status = New DevExpress.XtraEditors.LabelControl()
			Me.lbl1status = New DevExpress.XtraEditors.LabelControl()
			Me.grpQualifikation = New DevComponents.DotNetBar.Controls.GroupPanel()
			Me.lblHerkunftslandQualifikation = New DevExpress.XtraEditors.LabelControl()
			Me.lueCountryQualification = New DevExpress.XtraEditors.LookUpEdit()
			Me.lblQualifikation = New DevExpress.XtraEditors.LabelControl()
			Me.txtQualification = New DevExpress.XtraEditors.ComboBoxEdit()
			Me.grpMerkmale = New DevComponents.DotNetBar.Controls.GroupPanel()
			Me.ErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
			CType(Me.chkDStellen.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.chkESLock.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.lueState2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.lueState1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.lueContactInfo.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.grpQualifikation.SuspendLayout()
			CType(Me.lueCountryQualification.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.txtQualification.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.grpMerkmale.SuspendLayout()
			CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			'
			'chkDStellen
			'
			Me.chkDStellen.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.chkDStellen.Location = New System.Drawing.Point(198, 105)
			Me.chkDStellen.Name = "chkDStellen"
			Me.chkDStellen.Properties.Appearance.Options.UseTextOptions = True
			Me.chkDStellen.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.chkDStellen.Properties.Caption = "Dauerstellen"
			Me.chkDStellen.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.chkDStellen.Size = New System.Drawing.Size(142, 19)
			Me.chkDStellen.TabIndex = 6
			'
			'chkESLock
			'
			Me.chkESLock.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.chkESLock.Location = New System.Drawing.Point(198, 130)
			Me.chkESLock.Name = "chkESLock"
			Me.chkESLock.Properties.Appearance.Options.UseTextOptions = True
			Me.chkESLock.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.chkESLock.Properties.Caption = "Einsatz sperren"
			Me.chkESLock.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.chkESLock.Size = New System.Drawing.Size(142, 19)
			Me.chkESLock.TabIndex = 7
			'
			'lueState2
			'
			Me.lueState2.Location = New System.Drawing.Point(125, 47)
			Me.lueState2.Name = "lueState2"
			SerializableAppearanceObject5.ForeColor = System.Drawing.Color.Red
			SerializableAppearanceObject5.Options.UseForeColor = True
			Me.lueState2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject5, "", Nothing, Nothing, True)})
			Me.lueState2.Properties.ShowFooter = False
			Me.lueState2.Size = New System.Drawing.Size(215, 20)
			Me.lueState2.TabIndex = 9
			'
			'lueState1
			'
			Me.lueState1.Location = New System.Drawing.Point(125, 20)
			Me.lueState1.Name = "lueState1"
			SerializableAppearanceObject1.ForeColor = System.Drawing.Color.Red
			SerializableAppearanceObject1.Options.UseForeColor = True
			Me.lueState1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, "", Nothing, Nothing, True)})
			Me.lueState1.Properties.ShowFooter = False
			Me.lueState1.Size = New System.Drawing.Size(215, 20)
			Me.lueState1.TabIndex = 8
			'
			'lueContactInfo
			'
			Me.lueContactInfo.Location = New System.Drawing.Point(125, 73)
			Me.lueContactInfo.Name = "lueContactInfo"
			SerializableAppearanceObject2.ForeColor = System.Drawing.Color.Red
			SerializableAppearanceObject2.Options.UseForeColor = True
			Me.lueContactInfo.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject2, "", Nothing, Nothing, True)})
			Me.lueContactInfo.Properties.ShowFooter = False
			Me.lueContactInfo.Size = New System.Drawing.Size(215, 20)
			Me.lueContactInfo.TabIndex = 10
			'
			'lblkontakt
			'
			Me.lblkontakt.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblkontakt.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
			Me.lblkontakt.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblkontakt.Location = New System.Drawing.Point(13, 76)
			Me.lblkontakt.Name = "lblkontakt"
			Me.lblkontakt.Size = New System.Drawing.Size(106, 13)
			Me.lblkontakt.TabIndex = 240
			Me.lblkontakt.Text = "MAKontakt"
			'
			'lbl2status
			'
			Me.lbl2status.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lbl2status.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lbl2status.Location = New System.Drawing.Point(13, 51)
			Me.lbl2status.Name = "lbl2status"
			Me.lbl2status.Size = New System.Drawing.Size(106, 13)
			Me.lbl2status.TabIndex = 242
			Me.lbl2status.Text = "MA2Status"
			'
			'lbl1status
			'
			Me.lbl1status.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lbl1status.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lbl1status.Location = New System.Drawing.Point(13, 24)
			Me.lbl1status.Name = "lbl1status"
			Me.lbl1status.Size = New System.Drawing.Size(106, 13)
			Me.lbl1status.TabIndex = 241
			Me.lbl1status.Text = "MA1Status"
			'
			'grpQualifikation
			'
			Me.grpQualifikation.BackColor = System.Drawing.Color.White
			Me.grpQualifikation.CanvasColor = System.Drawing.SystemColors.Control
			Me.grpQualifikation.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
			Me.grpQualifikation.Controls.Add(Me.lblHerkunftslandQualifikation)
			Me.grpQualifikation.Controls.Add(Me.lueCountryQualification)
			Me.grpQualifikation.Controls.Add(Me.lblQualifikation)
			Me.grpQualifikation.Controls.Add(Me.txtQualification)
			Me.grpQualifikation.Location = New System.Drawing.Point(407, 10)
			Me.grpQualifikation.Name = "grpQualifikation"
			Me.grpQualifikation.Size = New System.Drawing.Size(368, 190)
			'
			'
			'
			Me.grpQualifikation.Style.BackColorGradientAngle = 90
			Me.grpQualifikation.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpQualifikation.Style.BorderBottomWidth = 1
			Me.grpQualifikation.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
			Me.grpQualifikation.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpQualifikation.Style.BorderLeftWidth = 1
			Me.grpQualifikation.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpQualifikation.Style.BorderRightWidth = 1
			Me.grpQualifikation.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpQualifikation.Style.BorderTopWidth = 1
			Me.grpQualifikation.Style.CornerDiameter = 4
			Me.grpQualifikation.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
			Me.grpQualifikation.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grpQualifikation.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
			Me.grpQualifikation.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
			Me.grpQualifikation.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
			'
			'
			'
			Me.grpQualifikation.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
			'
			'
			'
			Me.grpQualifikation.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
			Me.grpQualifikation.TabIndex = 243
			Me.grpQualifikation.Text = "Qualifikation"
			'
			'lblHerkunftslandQualifikation
			'
			Me.lblHerkunftslandQualifikation.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblHerkunftslandQualifikation.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical
			Me.lblHerkunftslandQualifikation.Location = New System.Drawing.Point(10, 51)
			Me.lblHerkunftslandQualifikation.Name = "lblHerkunftslandQualifikation"
			Me.lblHerkunftslandQualifikation.Size = New System.Drawing.Size(106, 13)
			Me.lblHerkunftslandQualifikation.TabIndex = 246
			Me.lblHerkunftslandQualifikation.Text = "Land"
			'
			'lueCountryQualification
			'
			Me.lueCountryQualification.Location = New System.Drawing.Point(122, 47)
			Me.lueCountryQualification.Name = "lueCountryQualification"
			SerializableAppearanceObject3.ForeColor = System.Drawing.Color.Red
			SerializableAppearanceObject3.Options.UseForeColor = True
			Me.lueCountryQualification.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject3, "", Nothing, Nothing, True)})
			Me.lueCountryQualification.Properties.ShowFooter = False
			Me.lueCountryQualification.Size = New System.Drawing.Size(215, 20)
			Me.lueCountryQualification.TabIndex = 2
			'
			'lblQualifikation
			'
			Me.lblQualifikation.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblQualifikation.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblQualifikation.Location = New System.Drawing.Point(10, 24)
			Me.lblQualifikation.Name = "lblQualifikation"
			Me.lblQualifikation.Size = New System.Drawing.Size(106, 13)
			Me.lblQualifikation.TabIndex = 241
			Me.lblQualifikation.Text = "Qualifikation"
			'
			'txtQualification
			'
			Me.txtQualification.Location = New System.Drawing.Point(122, 20)
			Me.txtQualification.Name = "txtQualification"
			Me.txtQualification.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
			Me.txtQualification.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
			Me.txtQualification.Size = New System.Drawing.Size(215, 20)
			Me.txtQualification.TabIndex = 1
			'
			'grpMerkmale
			'
			Me.grpMerkmale.BackColor = System.Drawing.Color.White
			Me.grpMerkmale.CanvasColor = System.Drawing.SystemColors.Control
			Me.grpMerkmale.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
			Me.grpMerkmale.Controls.Add(Me.lueState1)
			Me.grpMerkmale.Controls.Add(Me.lueContactInfo)
			Me.grpMerkmale.Controls.Add(Me.lblkontakt)
			Me.grpMerkmale.Controls.Add(Me.lueState2)
			Me.grpMerkmale.Controls.Add(Me.lbl2status)
			Me.grpMerkmale.Controls.Add(Me.chkESLock)
			Me.grpMerkmale.Controls.Add(Me.lbl1status)
			Me.grpMerkmale.Controls.Add(Me.chkDStellen)
			Me.grpMerkmale.Location = New System.Drawing.Point(15, 10)
			Me.grpMerkmale.Name = "grpMerkmale"
			Me.grpMerkmale.Size = New System.Drawing.Size(368, 190)
			'
			'
			'
			Me.grpMerkmale.Style.BackColorGradientAngle = 90
			Me.grpMerkmale.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpMerkmale.Style.BorderBottomWidth = 1
			Me.grpMerkmale.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
			Me.grpMerkmale.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpMerkmale.Style.BorderLeftWidth = 1
			Me.grpMerkmale.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpMerkmale.Style.BorderRightWidth = 1
			Me.grpMerkmale.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpMerkmale.Style.BorderTopWidth = 1
			Me.grpMerkmale.Style.CornerDiameter = 4
			Me.grpMerkmale.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
			Me.grpMerkmale.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grpMerkmale.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
			Me.grpMerkmale.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
			Me.grpMerkmale.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
			'
			'
			'
			Me.grpMerkmale.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
			'
			'
			'
			Me.grpMerkmale.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
			Me.grpMerkmale.TabIndex = 244
			Me.grpMerkmale.Text = "Eigenschaften und Merkmale"
			'
			'ErrorProvider
			'
			Me.ErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
			Me.ErrorProvider.ContainerControl = Me
			'
			'ucPageEmployeeAdditionalData1
			'
			Me.Appearance.BackColor = System.Drawing.Color.White
			Me.Appearance.Options.UseBackColor = True
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.Controls.Add(Me.grpMerkmale)
			Me.Controls.Add(Me.grpQualifikation)
			Me.Name = "ucPageEmployeeAdditionalData1"
			Me.Size = New System.Drawing.Size(865, 223)
			CType(Me.chkDStellen.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.chkESLock.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.lueState2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.lueState1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.lueContactInfo.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			Me.grpQualifikation.ResumeLayout(False)
			CType(Me.lueCountryQualification.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.txtQualification.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			Me.grpMerkmale.ResumeLayout(False)
			CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub
    Friend WithEvents chkDStellen As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents chkESLock As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents lueState2 As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents lueState1 As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents lueContactInfo As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents lblkontakt As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lbl2status As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lbl1status As DevExpress.XtraEditors.LabelControl
    Friend WithEvents grpQualifikation As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents lblHerkunftslandQualifikation As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lueCountryQualification As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents lblQualifikation As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtQualification As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents grpMerkmale As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents ErrorProvider As System.Windows.Forms.ErrorProvider

  End Class

End Namespace
