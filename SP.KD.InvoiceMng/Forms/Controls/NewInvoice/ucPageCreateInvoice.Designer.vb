Namespace UI

  <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
  Partial Class ucPageCreateInvoice
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
            Me.gpEigenschaften = New DevComponents.DotNetBar.Controls.GroupPanel()
            Me.lblMandantValue = New DevExpress.XtraEditors.LabelControl()
            Me.lblKostenstellenValue = New DevExpress.XtraEditors.LabelControl()
            Me.lblBeraterValue = New DevExpress.XtraEditors.LabelControl()
            Me.lblAdvisor = New DevExpress.XtraEditors.LabelControl()
            Me.lblMandant = New DevExpress.XtraEditors.LabelControl()
            Me.lblKst1 = New DevExpress.XtraEditors.LabelControl()
            Me.gpRechnungsdaten = New DevComponents.DotNetBar.Controls.GroupPanel()
            Me.lblFaelligValue = New DevExpress.XtraEditors.LabelControl()
            Me.lblAdresseValue = New DevExpress.XtraEditors.LabelControl()
            Me.lblFirmaValue = New DevExpress.XtraEditors.LabelControl()
            Me.lblDatumValue = New DevExpress.XtraEditors.LabelControl()
            Me.lblBankdatenValue = New DevExpress.XtraEditors.LabelControl()
            Me.lblDebitorenartValue = New DevExpress.XtraEditors.LabelControl()
            Me.lblAddresse = New DevExpress.XtraEditors.LabelControl()
            Me.ucCustomerPopup = New SP.Infrastructure.ucListSelectPopup()
            Me.lblFirma = New DevExpress.XtraEditors.LabelControl()
            Me.lblBankdaten = New DevExpress.XtraEditors.LabelControl()
            Me.lblDebitorenart = New DevExpress.XtraEditors.LabelControl()
            Me.lblDatum = New DevExpress.XtraEditors.LabelControl()
            Me.lblDueDate = New DevExpress.XtraEditors.LabelControl()
            Me.gpEigenschaften.SuspendLayout()
            Me.gpRechnungsdaten.SuspendLayout()
            Me.SuspendLayout()
            '
            'gpEigenschaften
            '
            Me.gpEigenschaften.BackColor = System.Drawing.Color.Transparent
            Me.gpEigenschaften.CanvasColor = System.Drawing.SystemColors.Control
            Me.gpEigenschaften.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
            Me.gpEigenschaften.Controls.Add(Me.lblMandantValue)
            Me.gpEigenschaften.Controls.Add(Me.lblKostenstellenValue)
            Me.gpEigenschaften.Controls.Add(Me.lblBeraterValue)
            Me.gpEigenschaften.Controls.Add(Me.lblAdvisor)
            Me.gpEigenschaften.Controls.Add(Me.lblMandant)
            Me.gpEigenschaften.Controls.Add(Me.lblKst1)
            Me.gpEigenschaften.Location = New System.Drawing.Point(21, 13)
            Me.gpEigenschaften.Name = "gpEigenschaften"
            Me.gpEigenschaften.Size = New System.Drawing.Size(451, 120)
            '
            '
            '
            Me.gpEigenschaften.Style.BackColorGradientAngle = 90
            Me.gpEigenschaften.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.gpEigenschaften.Style.BorderBottomWidth = 1
            Me.gpEigenschaften.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
            Me.gpEigenschaften.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.gpEigenschaften.Style.BorderLeftWidth = 1
            Me.gpEigenschaften.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.gpEigenschaften.Style.BorderRightWidth = 1
            Me.gpEigenschaften.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.gpEigenschaften.Style.BorderTopWidth = 1
            Me.gpEigenschaften.Style.CornerDiameter = 4
            Me.gpEigenschaften.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
            Me.gpEigenschaften.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
            Me.gpEigenschaften.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
            Me.gpEigenschaften.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
            Me.gpEigenschaften.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
            '
            '
            '
            Me.gpEigenschaften.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
            '
            '
            '
            Me.gpEigenschaften.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.gpEigenschaften.TabIndex = 288
            Me.gpEigenschaften.Text = "Eigenschaften und Merkmale"
            '
            'lblMandantValue
            '
            Me.lblMandantValue.Appearance.Options.UseTextOptions = True
            Me.lblMandantValue.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
            Me.lblMandantValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
            Me.lblMandantValue.Location = New System.Drawing.Point(172, 13)
            Me.lblMandantValue.Name = "lblMandantValue"
            Me.lblMandantValue.Size = New System.Drawing.Size(230, 13)
            Me.lblMandantValue.TabIndex = 258
            Me.lblMandantValue.Text = "Mandant Value"
            '
            'lblKostenstellenValue
            '
            Me.lblKostenstellenValue.Appearance.Options.UseTextOptions = True
            Me.lblKostenstellenValue.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
            Me.lblKostenstellenValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
            Me.lblKostenstellenValue.Location = New System.Drawing.Point(172, 39)
            Me.lblKostenstellenValue.Name = "lblKostenstellenValue"
            Me.lblKostenstellenValue.Size = New System.Drawing.Size(195, 13)
            Me.lblKostenstellenValue.TabIndex = 257
            Me.lblKostenstellenValue.Text = "Kostenstellen Value"
            '
            'lblBeraterValue
            '
            Me.lblBeraterValue.Appearance.Options.UseTextOptions = True
            Me.lblBeraterValue.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
            Me.lblBeraterValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
            Me.lblBeraterValue.Location = New System.Drawing.Point(172, 66)
            Me.lblBeraterValue.Name = "lblBeraterValue"
            Me.lblBeraterValue.Size = New System.Drawing.Size(195, 13)
            Me.lblBeraterValue.TabIndex = 256
            Me.lblBeraterValue.Text = "BeraterIn Value"
            '
            'lblAdvisor
            '
            Me.lblAdvisor.Appearance.Options.UseTextOptions = True
            Me.lblAdvisor.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
            Me.lblAdvisor.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
            Me.lblAdvisor.Location = New System.Drawing.Point(33, 66)
            Me.lblAdvisor.Name = "lblAdvisor"
            Me.lblAdvisor.Size = New System.Drawing.Size(78, 13)
            Me.lblAdvisor.TabIndex = 243
            Me.lblAdvisor.Text = "BeraterIn"
            '
            'lblMandant
            '
            Me.lblMandant.Appearance.Options.UseTextOptions = True
            Me.lblMandant.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
            Me.lblMandant.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
            Me.lblMandant.Location = New System.Drawing.Point(33, 13)
            Me.lblMandant.Name = "lblMandant"
            Me.lblMandant.Size = New System.Drawing.Size(78, 13)
            Me.lblMandant.TabIndex = 244
            Me.lblMandant.Text = "Mandant"
            '
            'lblKst1
            '
            Me.lblKst1.Appearance.Options.UseTextOptions = True
            Me.lblKst1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
            Me.lblKst1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
            Me.lblKst1.Location = New System.Drawing.Point(33, 39)
            Me.lblKst1.Name = "lblKst1"
            Me.lblKst1.Size = New System.Drawing.Size(78, 13)
            Me.lblKst1.TabIndex = 241
            Me.lblKst1.Text = "Kostenstellen"
            '
            'gpRechnungsdaten
            '
            Me.gpRechnungsdaten.BackColor = System.Drawing.Color.Transparent
            Me.gpRechnungsdaten.CanvasColor = System.Drawing.SystemColors.Control
            Me.gpRechnungsdaten.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
            Me.gpRechnungsdaten.Controls.Add(Me.lblFaelligValue)
            Me.gpRechnungsdaten.Controls.Add(Me.lblAdresseValue)
            Me.gpRechnungsdaten.Controls.Add(Me.lblFirmaValue)
            Me.gpRechnungsdaten.Controls.Add(Me.lblDatumValue)
            Me.gpRechnungsdaten.Controls.Add(Me.lblBankdatenValue)
            Me.gpRechnungsdaten.Controls.Add(Me.lblDebitorenartValue)
            Me.gpRechnungsdaten.Controls.Add(Me.lblAddresse)
            Me.gpRechnungsdaten.Controls.Add(Me.ucCustomerPopup)
            Me.gpRechnungsdaten.Controls.Add(Me.lblFirma)
            Me.gpRechnungsdaten.Controls.Add(Me.lblBankdaten)
            Me.gpRechnungsdaten.Controls.Add(Me.lblDebitorenart)
            Me.gpRechnungsdaten.Controls.Add(Me.lblDatum)
            Me.gpRechnungsdaten.Controls.Add(Me.lblDueDate)
            Me.gpRechnungsdaten.Location = New System.Drawing.Point(21, 139)
            Me.gpRechnungsdaten.Name = "gpRechnungsdaten"
            Me.gpRechnungsdaten.Size = New System.Drawing.Size(451, 189)
            '
            '
            '
            Me.gpRechnungsdaten.Style.BackColorGradientAngle = 90
            Me.gpRechnungsdaten.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.gpRechnungsdaten.Style.BorderBottomWidth = 1
            Me.gpRechnungsdaten.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
            Me.gpRechnungsdaten.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.gpRechnungsdaten.Style.BorderLeftWidth = 1
            Me.gpRechnungsdaten.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.gpRechnungsdaten.Style.BorderRightWidth = 1
            Me.gpRechnungsdaten.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.gpRechnungsdaten.Style.BorderTopWidth = 1
            Me.gpRechnungsdaten.Style.CornerDiameter = 4
            Me.gpRechnungsdaten.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
            Me.gpRechnungsdaten.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
            Me.gpRechnungsdaten.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
            Me.gpRechnungsdaten.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
            Me.gpRechnungsdaten.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
            '
            '
            '
            Me.gpRechnungsdaten.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
            '
            '
            '
            Me.gpRechnungsdaten.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.gpRechnungsdaten.TabIndex = 289
            Me.gpRechnungsdaten.Text = "Rechnungsdaten"
            '
            'lblFaelligValue
            '
            Me.lblFaelligValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
            Me.lblFaelligValue.Location = New System.Drawing.Point(168, 145)
            Me.lblFaelligValue.Name = "lblFaelligValue"
            Me.lblFaelligValue.Size = New System.Drawing.Size(207, 13)
            Me.lblFaelligValue.TabIndex = 305
            Me.lblFaelligValue.Text = "Fällig Value"
            '
            'lblAdresseValue
            '
            Me.lblAdresseValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
            Me.lblAdresseValue.Location = New System.Drawing.Point(167, 120)
            Me.lblAdresseValue.Name = "lblAdresseValue"
            Me.lblAdresseValue.Size = New System.Drawing.Size(207, 13)
            Me.lblAdresseValue.TabIndex = 304
            Me.lblAdresseValue.Text = "Adresse Value"
            '
            'lblFirmaValue
            '
            Me.lblFirmaValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
            Me.lblFirmaValue.Location = New System.Drawing.Point(168, 92)
            Me.lblFirmaValue.Name = "lblFirmaValue"
            Me.lblFirmaValue.Size = New System.Drawing.Size(207, 13)
            Me.lblFirmaValue.TabIndex = 303
            Me.lblFirmaValue.Text = "Firma Value"
            '
            'lblDatumValue
            '
            Me.lblDatumValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
            Me.lblDatumValue.Location = New System.Drawing.Point(168, 68)
            Me.lblDatumValue.Name = "lblDatumValue"
            Me.lblDatumValue.Size = New System.Drawing.Size(207, 13)
            Me.lblDatumValue.TabIndex = 302
            Me.lblDatumValue.Text = "Datum Value"
            '
            'lblBankdatenValue
            '
            Me.lblBankdatenValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
            Me.lblBankdatenValue.Location = New System.Drawing.Point(168, 42)
            Me.lblBankdatenValue.Name = "lblBankdatenValue"
            Me.lblBankdatenValue.Size = New System.Drawing.Size(207, 13)
            Me.lblBankdatenValue.TabIndex = 301
            Me.lblBankdatenValue.Text = "Bankdaten Value"
            '
            'lblDebitorenartValue
            '
            Me.lblDebitorenartValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
            Me.lblDebitorenartValue.Location = New System.Drawing.Point(168, 16)
            Me.lblDebitorenartValue.Name = "lblDebitorenartValue"
            Me.lblDebitorenartValue.Size = New System.Drawing.Size(207, 13)
            Me.lblDebitorenartValue.TabIndex = 300
            Me.lblDebitorenartValue.Text = "Debitorenart Value"
            '
            'lblAddresse
            '
            Me.lblAddresse.Appearance.Options.UseTextOptions = True
            Me.lblAddresse.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
            Me.lblAddresse.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
            Me.lblAddresse.Location = New System.Drawing.Point(21, 120)
            Me.lblAddresse.Name = "lblAddresse"
            Me.lblAddresse.Size = New System.Drawing.Size(93, 10)
            Me.lblAddresse.TabIndex = 299
            Me.lblAddresse.Text = "Adresse"
            '
            'ucCustomerPopup
            '
            Me.ucCustomerPopup.Location = New System.Drawing.Point(408, 71)
            Me.ucCustomerPopup.Name = "ucCustomerPopup"
            Me.ucCustomerPopup.Size = New System.Drawing.Size(11, 10)
            Me.ucCustomerPopup.TabIndex = 298
            '
            'lblFirma
            '
            Me.lblFirma.Appearance.Options.UseTextOptions = True
            Me.lblFirma.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
            Me.lblFirma.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
            Me.lblFirma.Location = New System.Drawing.Point(21, 95)
            Me.lblFirma.Name = "lblFirma"
            Me.lblFirma.Size = New System.Drawing.Size(93, 10)
            Me.lblFirma.TabIndex = 296
            Me.lblFirma.Text = "Firma"
            '
            'lblBankdaten
            '
            Me.lblBankdaten.Appearance.Options.UseTextOptions = True
            Me.lblBankdaten.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
            Me.lblBankdaten.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
            Me.lblBankdaten.Location = New System.Drawing.Point(36, 42)
            Me.lblBankdaten.Name = "lblBankdaten"
            Me.lblBankdaten.Size = New System.Drawing.Size(78, 13)
            Me.lblBankdaten.TabIndex = 239
            Me.lblBankdaten.Text = "Bankdaten"
            '
            'lblDebitorenart
            '
            Me.lblDebitorenart.Appearance.Options.UseTextOptions = True
            Me.lblDebitorenart.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
            Me.lblDebitorenart.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
            Me.lblDebitorenart.Location = New System.Drawing.Point(36, 16)
            Me.lblDebitorenart.Name = "lblDebitorenart"
            Me.lblDebitorenart.Size = New System.Drawing.Size(78, 13)
            Me.lblDebitorenart.TabIndex = 232
            Me.lblDebitorenart.Text = "Debitorenart"
            '
            'lblDatum
            '
            Me.lblDatum.Appearance.Options.UseTextOptions = True
            Me.lblDatum.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
            Me.lblDatum.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
            Me.lblDatum.Location = New System.Drawing.Point(36, 68)
            Me.lblDatum.Name = "lblDatum"
            Me.lblDatum.Size = New System.Drawing.Size(78, 13)
            Me.lblDatum.TabIndex = 234
            Me.lblDatum.Text = "Datum"
            '
            'lblDueDate
            '
            Me.lblDueDate.Appearance.Options.UseTextOptions = True
            Me.lblDueDate.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
            Me.lblDueDate.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
            Me.lblDueDate.Location = New System.Drawing.Point(36, 146)
            Me.lblDueDate.Name = "lblDueDate"
            Me.lblDueDate.Size = New System.Drawing.Size(78, 13)
            Me.lblDueDate.TabIndex = 235
            Me.lblDueDate.Text = "Fällig"
            '
            'ucPageCreateInvoice
            '
            Me.Appearance.BackColor = System.Drawing.Color.White
            Me.Appearance.Options.UseBackColor = True
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.gpRechnungsdaten)
            Me.Controls.Add(Me.gpEigenschaften)
            Me.Name = "ucPageCreateInvoice"
            Me.Size = New System.Drawing.Size(492, 353)
            Me.gpEigenschaften.ResumeLayout(False)
            Me.gpRechnungsdaten.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents gpEigenschaften As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents lblBeraterValue As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblAdvisor As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblMandant As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblKst1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents gpRechnungsdaten As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents lblAddresse As DevExpress.XtraEditors.LabelControl
    Friend WithEvents ucCustomerPopup As SP.Infrastructure.ucListSelectPopup
    Friend WithEvents lblFirma As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblBankdaten As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblDebitorenart As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblDatum As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblDueDate As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblMandantValue As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblKostenstellenValue As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblFaelligValue As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblAdresseValue As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblFirmaValue As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblDatumValue As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblBankdatenValue As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblDebitorenartValue As DevExpress.XtraEditors.LabelControl

  End Class

End Namespace
