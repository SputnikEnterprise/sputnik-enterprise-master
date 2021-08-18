Namespace UI

  <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
  Partial Class ucPageCreateZG
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
			Me.ErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
			Me.lblMandantValue = New DevExpress.XtraEditors.LabelControl()
			Me.lblBeraterValue = New DevExpress.XtraEditors.LabelControl()
			Me.lblBerater = New DevExpress.XtraEditors.LabelControl()
			Me.lblMandant = New DevExpress.XtraEditors.LabelControl()
			Me.lblMitarbeiterValue = New DevExpress.XtraEditors.LabelControl()
			Me.grpAuszahlung = New DevComponents.DotNetBar.Controls.GroupPanel()
			Me.lblWarning = New DevExpress.XtraEditors.LabelControl()
			Me.lblWarningValue = New DevExpress.XtraEditors.LabelControl()
			Me.lblZahlungsgrundValue = New DevExpress.XtraEditors.LabelControl()
			Me.lblBetragValue = New DevExpress.XtraEditors.LabelControl()
			Me.lblZahlartValue = New DevExpress.XtraEditors.LabelControl()
			Me.lblZahlumAmValue = New DevExpress.XtraEditors.LabelControl()
			Me.lblMonatValue = New DevExpress.XtraEditors.LabelControl()
			Me.lblJahrValue = New DevExpress.XtraEditors.LabelControl()
			Me.chkGebAbzug = New DevExpress.XtraEditors.CheckEdit()
			Me.lblZahlungsgrund = New DevExpress.XtraEditors.LabelControl()
			Me.lblZahlungAm = New DevExpress.XtraEditors.LabelControl()
			Me.lblZahlArt = New DevExpress.XtraEditors.LabelControl()
			Me.lblBetrag = New DevExpress.XtraEditors.LabelControl()
			Me.lblMonat = New DevExpress.XtraEditors.LabelControl()
			Me.lblJahr = New DevExpress.XtraEditors.LabelControl()
			Me.chkOpenMainForm = New DevExpress.XtraEditors.CheckEdit()
			Me.chkPrintDocumentAfterCreate = New DevExpress.XtraEditors.CheckEdit()
			Me.gpEigenschaften = New DevComponents.DotNetBar.Controls.GroupPanel()
			Me.lblMitarbeiter = New DevExpress.XtraEditors.LabelControl()
			Me.grpAbschluss = New DevComponents.DotNetBar.Controls.GroupPanel()
			CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.grpAuszahlung.SuspendLayout()
			CType(Me.chkGebAbzug.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.chkOpenMainForm.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.chkPrintDocumentAfterCreate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.gpEigenschaften.SuspendLayout()
			Me.grpAbschluss.SuspendLayout()
			Me.SuspendLayout()
			'
			'ErrorProvider
			'
			Me.ErrorProvider.ContainerControl = Me
			'
			'lblMandantValue
			'
			Me.lblMandantValue.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
			Me.lblMandantValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblMandantValue.Location = New System.Drawing.Point(120, 12)
			Me.lblMandantValue.Name = "lblMandantValue"
			Me.lblMandantValue.Size = New System.Drawing.Size(230, 13)
			Me.lblMandantValue.TabIndex = 262
			Me.lblMandantValue.Text = "Mandant Value"
			'
			'lblBeraterValue
			'
			Me.lblBeraterValue.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
			Me.lblBeraterValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblBeraterValue.Location = New System.Drawing.Point(120, 55)
			Me.lblBeraterValue.Name = "lblBeraterValue"
			Me.lblBeraterValue.Size = New System.Drawing.Size(195, 13)
			Me.lblBeraterValue.TabIndex = 261
			Me.lblBeraterValue.Text = "BeraterIn Value"
			Me.lblBeraterValue.Visible = False
			'
			'lblBerater
			'
			Me.lblBerater.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblBerater.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblBerater.Location = New System.Drawing.Point(3, 54)
			Me.lblBerater.Name = "lblBerater"
			Me.lblBerater.Size = New System.Drawing.Size(111, 13)
			Me.lblBerater.TabIndex = 259
			Me.lblBerater.Text = "BeraterIn"
			Me.lblBerater.Visible = False
			'
			'lblMandant
			'
			Me.lblMandant.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblMandant.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblMandant.Location = New System.Drawing.Point(3, 12)
			Me.lblMandant.Name = "lblMandant"
			Me.lblMandant.Size = New System.Drawing.Size(111, 13)
			Me.lblMandant.TabIndex = 260
			Me.lblMandant.Text = "Mandant"
			'
			'lblMitarbeiterValue
			'
			Me.lblMitarbeiterValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblMitarbeiterValue.Location = New System.Drawing.Point(120, 33)
			Me.lblMitarbeiterValue.Name = "lblMitarbeiterValue"
			Me.lblMitarbeiterValue.Size = New System.Drawing.Size(207, 13)
			Me.lblMitarbeiterValue.TabIndex = 264
			Me.lblMitarbeiterValue.Text = "Mitarbeiter Value"
			'
			'grpAuszahlung
			'
			Me.grpAuszahlung.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
							Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.grpAuszahlung.BackColor = System.Drawing.Color.Transparent
			Me.grpAuszahlung.CanvasColor = System.Drawing.SystemColors.Control
			Me.grpAuszahlung.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
			Me.grpAuszahlung.Controls.Add(Me.lblWarning)
			Me.grpAuszahlung.Controls.Add(Me.lblWarningValue)
			Me.grpAuszahlung.Controls.Add(Me.lblZahlungsgrundValue)
			Me.grpAuszahlung.Controls.Add(Me.lblBetragValue)
			Me.grpAuszahlung.Controls.Add(Me.lblZahlartValue)
			Me.grpAuszahlung.Controls.Add(Me.lblZahlumAmValue)
			Me.grpAuszahlung.Controls.Add(Me.lblMonatValue)
			Me.grpAuszahlung.Controls.Add(Me.lblJahrValue)
			Me.grpAuszahlung.Controls.Add(Me.chkGebAbzug)
			Me.grpAuszahlung.Controls.Add(Me.lblZahlungsgrund)
			Me.grpAuszahlung.Controls.Add(Me.lblZahlungAm)
			Me.grpAuszahlung.Controls.Add(Me.lblZahlArt)
			Me.grpAuszahlung.Controls.Add(Me.lblBetrag)
			Me.grpAuszahlung.Controls.Add(Me.lblMonat)
			Me.grpAuszahlung.Controls.Add(Me.lblJahr)
			Me.grpAuszahlung.Location = New System.Drawing.Point(6, 115)
			Me.grpAuszahlung.Name = "grpAuszahlung"
			Me.grpAuszahlung.Size = New System.Drawing.Size(510, 205)
			'
			'
			'
			Me.grpAuszahlung.Style.BackColorGradientAngle = 90
			Me.grpAuszahlung.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpAuszahlung.Style.BorderBottomWidth = 1
			Me.grpAuszahlung.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
			Me.grpAuszahlung.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpAuszahlung.Style.BorderLeftWidth = 1
			Me.grpAuszahlung.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpAuszahlung.Style.BorderRightWidth = 1
			Me.grpAuszahlung.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpAuszahlung.Style.BorderTopWidth = 1
			Me.grpAuszahlung.Style.CornerDiameter = 4
			Me.grpAuszahlung.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
			Me.grpAuszahlung.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grpAuszahlung.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
			Me.grpAuszahlung.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
			Me.grpAuszahlung.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
			'
			'
			'
			Me.grpAuszahlung.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
			'
			'
			'
			Me.grpAuszahlung.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
			Me.grpAuszahlung.TabIndex = 287
			Me.grpAuszahlung.Text = "Auszahlung"
			'
			'lblWarning
			'
			Me.lblWarning.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
			Me.lblWarning.Appearance.ForeColor = System.Drawing.Color.Red
			Me.lblWarning.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblWarning.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblWarning.Location = New System.Drawing.Point(3, 158)
			Me.lblWarning.Name = "lblWarning"
			Me.lblWarning.Size = New System.Drawing.Size(111, 13)
			Me.lblWarning.TabIndex = 297
			Me.lblWarning.Text = "Warnung"
			'
			'lblWarningValue
			'
			Me.lblWarningValue.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
			Me.lblWarningValue.Appearance.ForeColor = System.Drawing.Color.Red
			Me.lblWarningValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical
			Me.lblWarningValue.Location = New System.Drawing.Point(120, 158)
			Me.lblWarningValue.Name = "lblWarningValue"
			Me.lblWarningValue.Size = New System.Drawing.Size(365, 13)
			Me.lblWarningValue.TabIndex = 296
			Me.lblWarningValue.Text = "Der maximale Auszahlungsbetrag wurde überschritten!"
			'
			'lblZahlungsgrundValue
			'
			Me.lblZahlungsgrundValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical
			Me.lblZahlungsgrundValue.Location = New System.Drawing.Point(120, 131)
			Me.lblZahlungsgrundValue.Name = "lblZahlungsgrundValue"
			Me.lblZahlungsgrundValue.Size = New System.Drawing.Size(365, 13)
			Me.lblZahlungsgrundValue.TabIndex = 295
			Me.lblZahlungsgrundValue.Text = "lblZahlungsgrundValue"
			'
			'lblBetragValue
			'
			Me.lblBetragValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblBetragValue.Location = New System.Drawing.Point(120, 87)
			Me.lblBetragValue.Name = "lblBetragValue"
			Me.lblBetragValue.Size = New System.Drawing.Size(112, 13)
			Me.lblBetragValue.TabIndex = 294
			Me.lblBetragValue.Text = "Betrag Value"
			'
			'lblZahlartValue
			'
			Me.lblZahlartValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblZahlartValue.Location = New System.Drawing.Point(120, 68)
			Me.lblZahlartValue.Name = "lblZahlartValue"
			Me.lblZahlartValue.Size = New System.Drawing.Size(112, 13)
			Me.lblZahlartValue.TabIndex = 293
			Me.lblZahlartValue.Text = "Zahlart Value"
			'
			'lblZahlumAmValue
			'
			Me.lblZahlumAmValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblZahlumAmValue.Location = New System.Drawing.Point(120, 49)
			Me.lblZahlumAmValue.Name = "lblZahlumAmValue"
			Me.lblZahlumAmValue.Size = New System.Drawing.Size(112, 13)
			Me.lblZahlumAmValue.TabIndex = 292
			Me.lblZahlumAmValue.Text = "ZahlungAm Value"
			'
			'lblMonatValue
			'
			Me.lblMonatValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblMonatValue.Location = New System.Drawing.Point(120, 30)
			Me.lblMonatValue.Name = "lblMonatValue"
			Me.lblMonatValue.Size = New System.Drawing.Size(112, 13)
			Me.lblMonatValue.TabIndex = 291
			Me.lblMonatValue.Text = "Monat Value"
			'
			'lblJahrValue
			'
			Me.lblJahrValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblJahrValue.Location = New System.Drawing.Point(120, 11)
			Me.lblJahrValue.Name = "lblJahrValue"
			Me.lblJahrValue.Size = New System.Drawing.Size(87, 13)
			Me.lblJahrValue.TabIndex = 266
			Me.lblJahrValue.Text = "Jahr Value"
			'
			'chkGebAbzug
			'
			Me.chkGebAbzug.Location = New System.Drawing.Point(118, 106)
			Me.chkGebAbzug.Name = "chkGebAbzug"
			Me.chkGebAbzug.Properties.AllowFocused = False
			Me.chkGebAbzug.Properties.Caption = "Mit Gebührenabzug"
			Me.chkGebAbzug.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.[Default]
			Me.chkGebAbzug.Size = New System.Drawing.Size(231, 19)
			Me.chkGebAbzug.TabIndex = 290
			'
			'lblZahlungsgrund
			'
			Me.lblZahlungsgrund.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblZahlungsgrund.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblZahlungsgrund.Location = New System.Drawing.Point(3, 131)
			Me.lblZahlungsgrund.Name = "lblZahlungsgrund"
			Me.lblZahlungsgrund.Size = New System.Drawing.Size(111, 13)
			Me.lblZahlungsgrund.TabIndex = 288
			Me.lblZahlungsgrund.Text = "Zahlungsgrund"
			'
			'lblZahlungAm
			'
			Me.lblZahlungAm.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblZahlungAm.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblZahlungAm.Location = New System.Drawing.Point(3, 49)
			Me.lblZahlungAm.Name = "lblZahlungAm"
			Me.lblZahlungAm.Size = New System.Drawing.Size(111, 13)
			Me.lblZahlungAm.TabIndex = 287
			Me.lblZahlungAm.Text = "Zahlung am"
			'
			'lblZahlArt
			'
			Me.lblZahlArt.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblZahlArt.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblZahlArt.Location = New System.Drawing.Point(3, 68)
			Me.lblZahlArt.Name = "lblZahlArt"
			Me.lblZahlArt.Size = New System.Drawing.Size(111, 13)
			Me.lblZahlArt.TabIndex = 284
			Me.lblZahlArt.Text = "Zahlart"
			'
			'lblBetrag
			'
			Me.lblBetrag.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblBetrag.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblBetrag.Location = New System.Drawing.Point(3, 87)
			Me.lblBetrag.Name = "lblBetrag"
			Me.lblBetrag.Size = New System.Drawing.Size(111, 13)
			Me.lblBetrag.TabIndex = 278
			Me.lblBetrag.Text = "Betrag"
			'
			'lblMonat
			'
			Me.lblMonat.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblMonat.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblMonat.Location = New System.Drawing.Point(3, 30)
			Me.lblMonat.Name = "lblMonat"
			Me.lblMonat.Size = New System.Drawing.Size(111, 13)
			Me.lblMonat.TabIndex = 282
			Me.lblMonat.Text = "Monat"
			'
			'lblJahr
			'
			Me.lblJahr.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblJahr.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblJahr.Location = New System.Drawing.Point(3, 11)
			Me.lblJahr.Name = "lblJahr"
			Me.lblJahr.Size = New System.Drawing.Size(111, 13)
			Me.lblJahr.TabIndex = 277
			Me.lblJahr.Text = "Jahr"
			'
			'chkOpenMainForm
			'
			Me.chkOpenMainForm.Location = New System.Drawing.Point(118, 36)
			Me.chkOpenMainForm.Name = "chkOpenMainForm"
			Me.chkOpenMainForm.Properties.AllowFocused = False
			Me.chkOpenMainForm.Properties.Caption = "Bei Abschluss Maske öffnen"
			Me.chkOpenMainForm.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.[Default]
			Me.chkOpenMainForm.Size = New System.Drawing.Size(367, 19)
			Me.chkOpenMainForm.TabIndex = 299
			'
			'chkPrintDocumentAfterCreate
			'
			Me.chkPrintDocumentAfterCreate.EditValue = True
			Me.chkPrintDocumentAfterCreate.Location = New System.Drawing.Point(118, 11)
			Me.chkPrintDocumentAfterCreate.Name = "chkPrintDocumentAfterCreate"
			Me.chkPrintDocumentAfterCreate.Properties.AllowFocused = False
			Me.chkPrintDocumentAfterCreate.Properties.Caption = "Bei Abschluss Vorschuss drucken"
			Me.chkPrintDocumentAfterCreate.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.[Default]
			Me.chkPrintDocumentAfterCreate.Size = New System.Drawing.Size(367, 19)
			Me.chkPrintDocumentAfterCreate.TabIndex = 298
			'
			'gpEigenschaften
			'
			Me.gpEigenschaften.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
							Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.gpEigenschaften.BackColor = System.Drawing.Color.Transparent
			Me.gpEigenschaften.CanvasColor = System.Drawing.SystemColors.Control
			Me.gpEigenschaften.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
			Me.gpEigenschaften.Controls.Add(Me.lblMitarbeiter)
			Me.gpEigenschaften.Controls.Add(Me.lblMandant)
			Me.gpEigenschaften.Controls.Add(Me.lblBerater)
			Me.gpEigenschaften.Controls.Add(Me.lblMitarbeiterValue)
			Me.gpEigenschaften.Controls.Add(Me.lblBeraterValue)
			Me.gpEigenschaften.Controls.Add(Me.lblMandantValue)
			Me.gpEigenschaften.Location = New System.Drawing.Point(6, 3)
			Me.gpEigenschaften.Name = "gpEigenschaften"
			Me.gpEigenschaften.Size = New System.Drawing.Size(510, 102)
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
			Me.gpEigenschaften.TabIndex = 290
			Me.gpEigenschaften.Text = "Eigenschaften und Merkmale"
			'
			'lblMitarbeiter
			'
			Me.lblMitarbeiter.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblMitarbeiter.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblMitarbeiter.Location = New System.Drawing.Point(3, 33)
			Me.lblMitarbeiter.Name = "lblMitarbeiter"
			Me.lblMitarbeiter.Size = New System.Drawing.Size(111, 13)
			Me.lblMitarbeiter.TabIndex = 265
			Me.lblMitarbeiter.Text = "Mitarbeiter"
			'
			'grpAbschluss
			'
			Me.grpAbschluss.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
							Or System.Windows.Forms.AnchorStyles.Left) _
							Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.grpAbschluss.BackColor = System.Drawing.Color.Transparent
			Me.grpAbschluss.CanvasColor = System.Drawing.SystemColors.Control
			Me.grpAbschluss.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
			Me.grpAbschluss.Controls.Add(Me.chkOpenMainForm)
			Me.grpAbschluss.Controls.Add(Me.chkPrintDocumentAfterCreate)
			Me.grpAbschluss.Location = New System.Drawing.Point(6, 330)
			Me.grpAbschluss.Name = "grpAbschluss"
			Me.grpAbschluss.Size = New System.Drawing.Size(510, 113)
			'
			'
			'
			Me.grpAbschluss.Style.BackColorGradientAngle = 90
			Me.grpAbschluss.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpAbschluss.Style.BorderBottomWidth = 1
			Me.grpAbschluss.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
			Me.grpAbschluss.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpAbschluss.Style.BorderLeftWidth = 1
			Me.grpAbschluss.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpAbschluss.Style.BorderRightWidth = 1
			Me.grpAbschluss.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpAbschluss.Style.BorderTopWidth = 1
			Me.grpAbschluss.Style.CornerDiameter = 4
			Me.grpAbschluss.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
			Me.grpAbschluss.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grpAbschluss.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
			Me.grpAbschluss.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
			Me.grpAbschluss.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
			'
			'
			'
			Me.grpAbschluss.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
			'
			'
			'
			Me.grpAbschluss.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
			Me.grpAbschluss.TabIndex = 291
			Me.grpAbschluss.Text = "Abschluss"
			'
			'ucPageCreateZG
			'
			Me.Appearance.BackColor = System.Drawing.Color.White
			Me.Appearance.Options.UseBackColor = True
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.Controls.Add(Me.grpAbschluss)
			Me.Controls.Add(Me.gpEigenschaften)
			Me.Controls.Add(Me.grpAuszahlung)
			Me.Name = "ucPageCreateZG"
			Me.Size = New System.Drawing.Size(526, 460)
			CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
			Me.grpAuszahlung.ResumeLayout(False)
			CType(Me.chkGebAbzug.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.chkOpenMainForm.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.chkPrintDocumentAfterCreate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			Me.gpEigenschaften.ResumeLayout(False)
			Me.grpAbschluss.ResumeLayout(False)
			Me.ResumeLayout(False)

		End Sub
		Friend WithEvents ErrorProvider As System.Windows.Forms.ErrorProvider
		Friend WithEvents lblMandantValue As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblBeraterValue As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblBerater As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblMandant As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblMitarbeiterValue As DevExpress.XtraEditors.LabelControl
		Friend WithEvents chkGebAbzug As DevExpress.XtraEditors.CheckEdit
		Friend WithEvents lblZahlungsgrund As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblZahlungAm As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblZahlArt As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblBetrag As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblMonat As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblJahr As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblMitarbeiter As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblZahlungsgrundValue As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblBetragValue As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblZahlartValue As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblZahlumAmValue As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblMonatValue As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblJahrValue As DevExpress.XtraEditors.LabelControl
		Private WithEvents grpAuszahlung As DevComponents.DotNetBar.Controls.GroupPanel
		Private WithEvents gpEigenschaften As DevComponents.DotNetBar.Controls.GroupPanel
		Friend WithEvents lblWarningValue As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblWarning As DevExpress.XtraEditors.LabelControl
		Friend WithEvents chkPrintDocumentAfterCreate As DevExpress.XtraEditors.CheckEdit
		Friend WithEvents chkOpenMainForm As DevExpress.XtraEditors.CheckEdit
		Private WithEvents grpAbschluss As DevComponents.DotNetBar.Controls.GroupPanel

  End Class

End Namespace

