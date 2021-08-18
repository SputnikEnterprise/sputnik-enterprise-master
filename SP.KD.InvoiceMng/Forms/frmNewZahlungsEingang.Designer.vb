
Namespace UI

  <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
  Partial Class frmNewZahlungsEingang
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
			Me.components = New System.ComponentModel.Container()
			Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmNewInvoice))
			Me.object_b394e378_e210_4e67_a7af_192a6b8a5120 = New DevExpress.XtraBars.PopupControlContainer(Me.components)
			Me.wizardCtrl = New DevExpress.XtraWizard.WizardControl()
			Me.pageMandantPaymentData = New DevExpress.XtraWizard.WizardPage()
			Me.ucSelectPaymentMandant = New SP.KD.InvoiceMng.UI.ucPageSelectMandantPayment()
			'Me.pageInvoiceData = New DevExpress.XtraWizard.WizardPage()
			'Me.ucPageSelectInvoiceData = New SP.KD.InvoiceMng.UI.ucPageSelectInvoiceData()
			Me.pageCreatePayment = New DevExpress.XtraWizard.WizardPage()
			Me.ucCreatePayment = New SP.KD.InvoiceMng.UI.ucPageCreatePayment()
			CType(Me.object_b394e378_e210_4e67_a7af_192a6b8a5120, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.wizardCtrl, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.wizardCtrl.SuspendLayout()
			Me.pageMandantPaymentData.SuspendLayout()
			'Me.pageInvoiceData.SuspendLayout()
			Me.pageCreatePayment.SuspendLayout()
			Me.SuspendLayout()
			'
			'object_b394e378_e210_4e67_a7af_192a6b8a5120
			'
			Me.object_b394e378_e210_4e67_a7af_192a6b8a5120.AutoSize = True
			Me.object_b394e378_e210_4e67_a7af_192a6b8a5120.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
			Me.object_b394e378_e210_4e67_a7af_192a6b8a5120.Location = New System.Drawing.Point(171, 144)
			Me.object_b394e378_e210_4e67_a7af_192a6b8a5120.Name = "object_b394e378_e210_4e67_a7af_192a6b8a5120"
			Me.object_b394e378_e210_4e67_a7af_192a6b8a5120.Size = New System.Drawing.Size(332, 239)
			Me.object_b394e378_e210_4e67_a7af_192a6b8a5120.TabIndex = 290
			Me.object_b394e378_e210_4e67_a7af_192a6b8a5120.Visible = False
			'
			'wizardCtrl
			'
			Me.wizardCtrl.AllowTransitionAnimation = False
			Me.wizardCtrl.Controls.Add(Me.pageMandantPaymentData)
			'Me.wizardCtrl.Controls.Add(Me.pageInvoiceData)
			Me.wizardCtrl.Controls.Add(Me.pageCreatePayment)
			Me.wizardCtrl.Dock = System.Windows.Forms.DockStyle.Fill
			Me.wizardCtrl.Location = New System.Drawing.Point(0, 0)
			Me.wizardCtrl.Name = "wizardCtrl"
			Me.wizardCtrl.Pages.AddRange(New DevExpress.XtraWizard.BaseWizardPage() {Me.pageMandantPaymentData, Me.pageCreatePayment})
			Me.wizardCtrl.Size = New System.Drawing.Size(674, 527)
			Me.wizardCtrl.Text = ""
			'
			'pageMandantData
			'
			Me.pageMandantPaymentData.Controls.Add(Me.ucSelectPaymentMandant)
			Me.pageMandantPaymentData.Name = "pageMandantData"
			Me.pageMandantPaymentData.Size = New System.Drawing.Size(642, 382)
			'
			'ucPageSelectMandant
			'
			Me.ucSelectPaymentMandant.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucSelectPaymentMandant.Location = New System.Drawing.Point(0, 0)
			Me.ucSelectPaymentMandant.Name = "ucPageSelectMandant"
			Me.ucSelectPaymentMandant.PreselectionData = Nothing
			Me.ucSelectPaymentMandant.Size = New System.Drawing.Size(642, 382)
			Me.ucSelectPaymentMandant.TabIndex = 0
			Me.ucSelectPaymentMandant.UCMediator = Nothing
			'
			''pageInvoiceData
			''
			'Me.pageInvoiceData.Controls.Add(Me.ucPageSelectInvoiceData)
			'Me.pageInvoiceData.Name = "pageInvoiceData"
			'Me.pageInvoiceData.Size = New System.Drawing.Size(642, 382)
			''
			''ucPageSelectInvoiceData
			''
			'Me.ucPageSelectInvoiceData.Dock = System.Windows.Forms.DockStyle.Fill
			'Me.ucPageSelectInvoiceData.Location = New System.Drawing.Point(0, 0)
			'Me.ucPageSelectInvoiceData.Name = "ucPageSelectInvoiceData"
			'Me.ucPageSelectInvoiceData.PreselectionData = Nothing
			'Me.ucPageSelectInvoiceData.Size = New System.Drawing.Size(642, 382)
			'Me.ucPageSelectInvoiceData.TabIndex = 1
			'Me.ucPageSelectInvoiceData.UCMediator = Nothing
			'
			'pageCreateInvoice
			'
			Me.pageCreatePayment.Controls.Add(Me.ucCreatePayment)
			Me.pageCreatePayment.Name = "pageCreateInvoice"
			Me.pageCreatePayment.Size = New System.Drawing.Size(642, 382)
			'
			'ucPageCreateInvoice
			'
			Me.ucCreatePayment.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucCreatePayment.Location = New System.Drawing.Point(0, 0)
			Me.ucCreatePayment.Name = "ucPageCreateInvoice"
			Me.ucCreatePayment.PreselectionData = Nothing
			Me.ucCreatePayment.Size = New System.Drawing.Size(642, 382)
			Me.ucCreatePayment.TabIndex = 1
			Me.ucCreatePayment.UCMediator = Nothing
			'
			'frmNewInvoice
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(674, 527)
			Me.Controls.Add(Me.wizardCtrl)
			Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
			Me.MinimumSize = New System.Drawing.Size(690, 565)
			Me.Name = "frmNewInvoice"
			Me.Text = "Neue Rechnung erfassen"
			CType(Me.object_b394e378_e210_4e67_a7af_192a6b8a5120, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.wizardCtrl, System.ComponentModel.ISupportInitialize).EndInit()
			Me.wizardCtrl.ResumeLayout(False)
			Me.pageMandantPaymentData.ResumeLayout(False)
			'Me.pageInvoiceData.ResumeLayout(False)
			Me.pageCreatePayment.ResumeLayout(False)
			Me.ResumeLayout(False)

		End Sub
		Friend WithEvents object_b394e378_e210_4e67_a7af_192a6b8a5120 As DevExpress.XtraBars.PopupControlContainer
		Friend WithEvents wizardCtrl As DevExpress.XtraWizard.WizardControl
		Friend WithEvents pageMandantPaymentData As DevExpress.XtraWizard.WizardPage
		'Friend WithEvents pageInvoiceData As DevExpress.XtraWizard.WizardPage
		Friend WithEvents pageCreatePayment As DevExpress.XtraWizard.WizardPage
		Friend WithEvents ucSelectPaymentMandant As SP.KD.InvoiceMng.UI.ucPageSelectMandantPayment
		'Friend WithEvents ucPageSelectInvoiceData As SP.KD.InvoiceMng.UI.ucPageSelectInvoiceData
		Friend WithEvents ucCreatePayment As SP.KD.InvoiceMng.UI.ucPageCreatePayment
	End Class

End Namespace
