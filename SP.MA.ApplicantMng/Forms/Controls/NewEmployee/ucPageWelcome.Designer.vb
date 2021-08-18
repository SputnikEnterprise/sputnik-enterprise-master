Namespace UI

  <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
  Partial Class ucPageWelcome
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
      Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
      Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
      Me.grpMandant = New DevComponents.DotNetBar.Controls.GroupPanel()
      Me.lblMandant = New DevExpress.XtraEditors.LabelControl()
      Me.lueAdvisor = New DevExpress.XtraEditors.LookUpEdit()
      Me.lueMandant = New DevExpress.XtraEditors.LookUpEdit()
      Me.lblBerater = New DevExpress.XtraEditors.LabelControl()
      Me.errorProvider = New System.Windows.Forms.ErrorProvider()
      Me.grpMandant.SuspendLayout()
      CType(Me.lueAdvisor.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.errorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'grpMandant
      '
      Me.grpMandant.BackColor = System.Drawing.Color.Transparent
      Me.grpMandant.CanvasColor = System.Drawing.SystemColors.Control
      Me.grpMandant.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
      Me.grpMandant.Controls.Add(Me.lblMandant)
      Me.grpMandant.Controls.Add(Me.lueAdvisor)
      Me.grpMandant.Controls.Add(Me.lueMandant)
      Me.grpMandant.Controls.Add(Me.lblBerater)
      Me.grpMandant.Location = New System.Drawing.Point(15, 14)
      Me.grpMandant.Name = "grpMandant"
      Me.grpMandant.Size = New System.Drawing.Size(452, 98)
      '
      '
      '
      Me.grpMandant.Style.BackColorGradientAngle = 90
      Me.grpMandant.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
      Me.grpMandant.Style.BorderBottomWidth = 1
      Me.grpMandant.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
      Me.grpMandant.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
      Me.grpMandant.Style.BorderLeftWidth = 1
      Me.grpMandant.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
      Me.grpMandant.Style.BorderRightWidth = 1
      Me.grpMandant.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
      Me.grpMandant.Style.BorderTopWidth = 1
      Me.grpMandant.Style.CornerDiameter = 4
      Me.grpMandant.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
      Me.grpMandant.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
      Me.grpMandant.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
      Me.grpMandant.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
      Me.grpMandant.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
      '
      '
      '
      Me.grpMandant.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
      '
      '
      '
      Me.grpMandant.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
      Me.grpMandant.TabIndex = 248
      Me.grpMandant.Text = "Mandant und Berater"
      '
      'lblMandant
      '
      Me.lblMandant.Appearance.BackColor = System.Drawing.Color.Transparent
      Me.lblMandant.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
      Me.lblMandant.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
      Me.lblMandant.Location = New System.Drawing.Point(23, 14)
      Me.lblMandant.Name = "lblMandant"
      Me.lblMandant.Size = New System.Drawing.Size(77, 13)
      Me.lblMandant.TabIndex = 247
      Me.lblMandant.Text = "Mandant"
      '
      'lueAdvisor
      '
      Me.lueAdvisor.Location = New System.Drawing.Point(106, 36)
      Me.lueAdvisor.MinimumSize = New System.Drawing.Size(150, 20)
      Me.lueAdvisor.Name = "lueAdvisor"
      SerializableAppearanceObject1.ForeColor = System.Drawing.Color.Red
      SerializableAppearanceObject1.Options.UseForeColor = True
      Me.lueAdvisor.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, "", Nothing, Nothing, True)})
      Me.lueAdvisor.Properties.ShowFooter = False
      Me.lueAdvisor.Size = New System.Drawing.Size(320, 20)
      Me.lueAdvisor.TabIndex = 245
      '
      'lueMandant
      '
      Me.lueMandant.Location = New System.Drawing.Point(106, 10)
      Me.lueMandant.MinimumSize = New System.Drawing.Size(150, 20)
      Me.lueMandant.Name = "lueMandant"
      SerializableAppearanceObject2.ForeColor = System.Drawing.Color.Red
      SerializableAppearanceObject2.Options.UseForeColor = True
      Me.lueMandant.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject2, "", Nothing, Nothing, True)})
      Me.lueMandant.Properties.ShowFooter = False
      Me.lueMandant.Size = New System.Drawing.Size(320, 20)
      Me.lueMandant.TabIndex = 244
      '
      'lblBerater
      '
      Me.lblBerater.Appearance.BackColor = System.Drawing.Color.Transparent
      Me.lblBerater.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
      Me.lblBerater.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
      Me.lblBerater.Location = New System.Drawing.Point(23, 40)
      Me.lblBerater.Name = "lblBerater"
      Me.lblBerater.Size = New System.Drawing.Size(77, 13)
      Me.lblBerater.TabIndex = 246
      Me.lblBerater.Text = "BeraterIn"
      '
      'errorProvider
      '
      Me.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
      Me.errorProvider.ContainerControl = Me
      '
      'ucPageWelcome
      '
      Me.Appearance.BackColor = System.Drawing.Color.White
      Me.Appearance.Options.UseBackColor = True
      Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
      Me.Controls.Add(Me.grpMandant)
      Me.Name = "ucPageWelcome"
      Me.Size = New System.Drawing.Size(844, 174)
      Me.grpMandant.ResumeLayout(False)
      CType(Me.lueAdvisor.Properties, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.errorProvider, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblMandant As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lueMandant As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents lblBerater As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lueAdvisor As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents errorProvider As System.Windows.Forms.ErrorProvider
    Friend WithEvents grpMandant As DevComponents.DotNetBar.Controls.GroupPanel

  End Class

End Namespace
