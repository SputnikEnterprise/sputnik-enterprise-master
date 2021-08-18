Namespace UI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class ucPageCreatePayment
        Inherits ucWizardPageBaseControl

        ''UserControl overrides dispose to clean up the component list.
        '<System.Diagnostics.DebuggerNonUserCode()>
        'Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        '    If disposing AndAlso components IsNot Nothing Then
        '        components.Dispose()
        '    End If
        '    MyBase.Dispose(disposing)
        'End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
			Me.lblMandantValue = New DevExpress.XtraEditors.LabelControl()
			Me.lblInvoiceValue = New DevExpress.XtraEditors.LabelControl()
			Me.lblInvoiceAmountValue = New DevExpress.XtraEditors.LabelControl()
			Me.lblRechnungsbetrag = New DevExpress.XtraEditors.LabelControl()
			Me.lblMandant = New DevExpress.XtraEditors.LabelControl()
			Me.lblRechnung = New DevExpress.XtraEditors.LabelControl()
			Me.lblBuchungValue = New DevExpress.XtraEditors.LabelControl()
			Me.lblValutaValue = New DevExpress.XtraEditors.LabelControl()
			Me.lblPaymentAmountValue = New DevExpress.XtraEditors.LabelControl()
			Me.lblSKontoValue = New DevExpress.XtraEditors.LabelControl()
			Me.lblBuchung = New DevExpress.XtraEditors.LabelControl()
			Me.lblZEBetrag = New DevExpress.XtraEditors.LabelControl()
			Me.lblSKonto = New DevExpress.XtraEditors.LabelControl()
			Me.lblValuta = New DevExpress.XtraEditors.LabelControl()
			Me.SeparatorControl1 = New DevExpress.XtraEditors.SeparatorControl()
			Me.gpEigenschaftenZE = New DevExpress.XtraEditors.GroupControl()
			Me.gpRechnungsdaten = New DevExpress.XtraEditors.GroupControl()
			CType(Me.SeparatorControl1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gpEigenschaftenZE, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.gpEigenschaftenZE.SuspendLayout()
			CType(Me.gpRechnungsdaten, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.gpRechnungsdaten.SuspendLayout()
			Me.SuspendLayout()
			'
			'lblMandantValue
			'
			Me.lblMandantValue.Appearance.Options.UseTextOptions = True
			Me.lblMandantValue.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
			Me.lblMandantValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblMandantValue.Location = New System.Drawing.Point(170, 38)
			Me.lblMandantValue.Name = "lblMandantValue"
			Me.lblMandantValue.Size = New System.Drawing.Size(260, 13)
			Me.lblMandantValue.TabIndex = 258
			Me.lblMandantValue.Text = "Mandant Value"
			'
			'lblInvoiceValue
			'
			Me.lblInvoiceValue.Appearance.Options.UseTextOptions = True
			Me.lblInvoiceValue.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
			Me.lblInvoiceValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblInvoiceValue.Location = New System.Drawing.Point(170, 42)
			Me.lblInvoiceValue.Name = "lblInvoiceValue"
			Me.lblInvoiceValue.Size = New System.Drawing.Size(260, 13)
			Me.lblInvoiceValue.TabIndex = 257
			Me.lblInvoiceValue.Text = "Rechnung"
			'
			'lblInvoiceAmountValue
			'
			Me.lblInvoiceAmountValue.Appearance.Options.UseTextOptions = True
			Me.lblInvoiceAmountValue.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
			Me.lblInvoiceAmountValue.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
			Me.lblInvoiceAmountValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblInvoiceAmountValue.Location = New System.Drawing.Point(170, 61)
			Me.lblInvoiceAmountValue.Name = "lblInvoiceAmountValue"
			Me.lblInvoiceAmountValue.Size = New System.Drawing.Size(260, 30)
			Me.lblInvoiceAmountValue.TabIndex = 256
			Me.lblInvoiceAmountValue.Text = "BetragInk Value"
			'
			'lblRechnungsbetrag
			'
			Me.lblRechnungsbetrag.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.lblRechnungsbetrag.Appearance.Options.UseFont = True
			Me.lblRechnungsbetrag.Appearance.Options.UseTextOptions = True
			Me.lblRechnungsbetrag.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblRechnungsbetrag.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblRechnungsbetrag.Location = New System.Drawing.Point(23, 61)
			Me.lblRechnungsbetrag.Name = "lblRechnungsbetrag"
			Me.lblRechnungsbetrag.Size = New System.Drawing.Size(141, 13)
			Me.lblRechnungsbetrag.TabIndex = 243
			Me.lblRechnungsbetrag.Text = "Beträge"
			'
			'lblMandant
			'
			Me.lblMandant.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.lblMandant.Appearance.Options.UseFont = True
			Me.lblMandant.Appearance.Options.UseTextOptions = True
			Me.lblMandant.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblMandant.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblMandant.Location = New System.Drawing.Point(23, 38)
			Me.lblMandant.Name = "lblMandant"
			Me.lblMandant.Size = New System.Drawing.Size(141, 13)
			Me.lblMandant.TabIndex = 244
			Me.lblMandant.Text = "Mandant"
			'
			'lblRechnung
			'
			Me.lblRechnung.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.lblRechnung.Appearance.Options.UseFont = True
			Me.lblRechnung.Appearance.Options.UseTextOptions = True
			Me.lblRechnung.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblRechnung.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblRechnung.Location = New System.Drawing.Point(23, 42)
			Me.lblRechnung.Name = "lblRechnung"
			Me.lblRechnung.Size = New System.Drawing.Size(141, 13)
			Me.lblRechnung.TabIndex = 241
			Me.lblRechnung.Text = "Rechnung"
			'
			'lblBuchungValue
			'
			Me.lblBuchungValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblBuchungValue.Location = New System.Drawing.Point(170, 195)
			Me.lblBuchungValue.Name = "lblBuchungValue"
			Me.lblBuchungValue.Size = New System.Drawing.Size(260, 13)
			Me.lblBuchungValue.TabIndex = 303
			Me.lblBuchungValue.Text = "Buchungsdatum Value"
			'
			'lblValutaValue
			'
			Me.lblValutaValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblValutaValue.Location = New System.Drawing.Point(170, 176)
			Me.lblValutaValue.Name = "lblValutaValue"
			Me.lblValutaValue.Size = New System.Drawing.Size(260, 13)
			Me.lblValutaValue.TabIndex = 302
			Me.lblValutaValue.Text = "ValutaDatum Value"
			'
			'lblPaymentAmountValue
			'
			Me.lblPaymentAmountValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblPaymentAmountValue.Location = New System.Drawing.Point(170, 157)
			Me.lblPaymentAmountValue.Name = "lblPaymentAmountValue"
			Me.lblPaymentAmountValue.Size = New System.Drawing.Size(260, 13)
			Me.lblPaymentAmountValue.TabIndex = 301
			Me.lblPaymentAmountValue.Text = "Betrag Value"
			'
			'lblSKontoValue
			'
			Me.lblSKontoValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblSKontoValue.Location = New System.Drawing.Point(170, 138)
			Me.lblSKontoValue.Name = "lblSKontoValue"
			Me.lblSKontoValue.Size = New System.Drawing.Size(260, 13)
			Me.lblSKontoValue.TabIndex = 300
			Me.lblSKontoValue.Text = "SKonto Value"
			'
			'lblBuchung
			'
			Me.lblBuchung.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.lblBuchung.Appearance.Options.UseFont = True
			Me.lblBuchung.Appearance.Options.UseTextOptions = True
			Me.lblBuchung.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblBuchung.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblBuchung.Location = New System.Drawing.Point(23, 195)
			Me.lblBuchung.Name = "lblBuchung"
			Me.lblBuchung.Size = New System.Drawing.Size(141, 13)
			Me.lblBuchung.TabIndex = 296
			Me.lblBuchung.Text = "Buchung"
			'
			'lblZEBetrag
			'
			Me.lblZEBetrag.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.lblZEBetrag.Appearance.Options.UseFont = True
			Me.lblZEBetrag.Appearance.Options.UseTextOptions = True
			Me.lblZEBetrag.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblZEBetrag.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblZEBetrag.Location = New System.Drawing.Point(23, 157)
			Me.lblZEBetrag.Name = "lblZEBetrag"
			Me.lblZEBetrag.Size = New System.Drawing.Size(141, 13)
			Me.lblZEBetrag.TabIndex = 239
			Me.lblZEBetrag.Text = "Betrag"
			'
			'lblSKonto
			'
			Me.lblSKonto.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.lblSKonto.Appearance.Options.UseFont = True
			Me.lblSKonto.Appearance.Options.UseTextOptions = True
			Me.lblSKonto.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblSKonto.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblSKonto.Location = New System.Drawing.Point(23, 138)
			Me.lblSKonto.Name = "lblSKonto"
			Me.lblSKonto.Size = New System.Drawing.Size(141, 13)
			Me.lblSKonto.TabIndex = 232
			Me.lblSKonto.Text = "SKonto"
			'
			'lblValuta
			'
			Me.lblValuta.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.lblValuta.Appearance.Options.UseFont = True
			Me.lblValuta.Appearance.Options.UseTextOptions = True
			Me.lblValuta.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblValuta.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblValuta.Location = New System.Drawing.Point(23, 176)
			Me.lblValuta.Name = "lblValuta"
			Me.lblValuta.Size = New System.Drawing.Size(141, 13)
			Me.lblValuta.TabIndex = 234
			Me.lblValuta.Text = "Valuta"
			'
			'SeparatorControl1
			'
			Me.SeparatorControl1.Location = New System.Drawing.Point(23, 97)
			Me.SeparatorControl1.Name = "SeparatorControl1"
			Me.SeparatorControl1.Size = New System.Drawing.Size(407, 25)
			Me.SeparatorControl1.TabIndex = 304
			'
			'gpEigenschaftenZE
			'
			Me.gpEigenschaftenZE.AppearanceCaption.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.gpEigenschaftenZE.AppearanceCaption.Options.UseFont = True
			Me.gpEigenschaftenZE.AppearanceCaption.Options.UseTextOptions = True
			Me.gpEigenschaftenZE.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			Me.gpEigenschaftenZE.Controls.Add(Me.lblMandantValue)
			Me.gpEigenschaftenZE.Controls.Add(Me.lblMandant)
			Me.gpEigenschaftenZE.Location = New System.Drawing.Point(21, 13)
			Me.gpEigenschaftenZE.Name = "gpEigenschaftenZE"
			Me.gpEigenschaftenZE.Size = New System.Drawing.Size(451, 87)
			Me.gpEigenschaftenZE.TabIndex = 290
			Me.gpEigenschaftenZE.Text = "Eigenschaften und Merkmale"
			'
			'gpRechnungsdaten
			'
			Me.gpRechnungsdaten.AppearanceCaption.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.gpRechnungsdaten.AppearanceCaption.Options.UseFont = True
			Me.gpRechnungsdaten.AppearanceCaption.Options.UseTextOptions = True
			Me.gpRechnungsdaten.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			Me.gpRechnungsdaten.Controls.Add(Me.SeparatorControl1)
			Me.gpRechnungsdaten.Controls.Add(Me.lblRechnung)
			Me.gpRechnungsdaten.Controls.Add(Me.lblBuchungValue)
			Me.gpRechnungsdaten.Controls.Add(Me.lblValuta)
			Me.gpRechnungsdaten.Controls.Add(Me.lblInvoiceValue)
			Me.gpRechnungsdaten.Controls.Add(Me.lblSKonto)
			Me.gpRechnungsdaten.Controls.Add(Me.lblInvoiceAmountValue)
			Me.gpRechnungsdaten.Controls.Add(Me.lblZEBetrag)
			Me.gpRechnungsdaten.Controls.Add(Me.lblValutaValue)
			Me.gpRechnungsdaten.Controls.Add(Me.lblBuchung)
			Me.gpRechnungsdaten.Controls.Add(Me.lblRechnungsbetrag)
			Me.gpRechnungsdaten.Controls.Add(Me.lblSKontoValue)
			Me.gpRechnungsdaten.Controls.Add(Me.lblPaymentAmountValue)
			Me.gpRechnungsdaten.Location = New System.Drawing.Point(21, 109)
			Me.gpRechnungsdaten.Name = "gpRechnungsdaten"
			Me.gpRechnungsdaten.Size = New System.Drawing.Size(451, 235)
			Me.gpRechnungsdaten.TabIndex = 291
			Me.gpRechnungsdaten.Text = "Zahlungsdetail"
			'
			'ucPageCreatePayment
			'
			Me.Appearance.BackColor = System.Drawing.Color.White
			Me.Appearance.Options.UseBackColor = True
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.Controls.Add(Me.gpRechnungsdaten)
			Me.Controls.Add(Me.gpEigenschaftenZE)
			Me.Name = "ucPageCreatePayment"
			Me.Size = New System.Drawing.Size(492, 353)
			CType(Me.SeparatorControl1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gpEigenschaftenZE, System.ComponentModel.ISupportInitialize).EndInit()
			Me.gpEigenschaftenZE.ResumeLayout(False)
			CType(Me.gpRechnungsdaten, System.ComponentModel.ISupportInitialize).EndInit()
			Me.gpRechnungsdaten.ResumeLayout(False)
			Me.ResumeLayout(False)

		End Sub
		Friend WithEvents lblInvoiceAmountValue As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblRechnungsbetrag As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblMandant As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblRechnung As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblBuchung As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblZEBetrag As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblSKonto As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblValuta As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblMandantValue As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblInvoiceValue As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblBuchungValue As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblValutaValue As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblPaymentAmountValue As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblSKontoValue As DevExpress.XtraEditors.LabelControl
		Friend WithEvents SeparatorControl1 As DevExpress.XtraEditors.SeparatorControl
		Friend WithEvents gpEigenschaftenZE As DevExpress.XtraEditors.GroupControl
		Friend WithEvents gpRechnungsdaten As DevExpress.XtraEditors.GroupControl
	End Class

End Namespace
