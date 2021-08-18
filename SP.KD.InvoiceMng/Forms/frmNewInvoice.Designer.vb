
Namespace UI

  <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
  Partial Class frmNewInvoice
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
			Me.pageMandantData = New DevExpress.XtraWizard.WizardPage()
			Me.ucPageSelectMandant = New SP.KD.InvoiceMng.UI.ucPageSelectMandant()
			Me.pageInvoiceData = New DevExpress.XtraWizard.WizardPage()
			Me.ucPageSelectInvoiceData = New SP.KD.InvoiceMng.UI.ucPageSelectInvoiceData()
			Me.pageCreateInvoice = New DevExpress.XtraWizard.WizardPage()
			Me.ucPageCreateInvoice = New SP.KD.InvoiceMng.UI.ucPageCreateInvoice()
			CType(Me.object_b394e378_e210_4e67_a7af_192a6b8a5120, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.wizardCtrl, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.wizardCtrl.SuspendLayout()
			Me.pageMandantData.SuspendLayout()
			Me.pageInvoiceData.SuspendLayout()
			Me.pageCreateInvoice.SuspendLayout()
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
			Me.wizardCtrl.Controls.Add(Me.pageMandantData)
			Me.wizardCtrl.Controls.Add(Me.pageInvoiceData)
			Me.wizardCtrl.Controls.Add(Me.pageCreateInvoice)
			Me.wizardCtrl.Dock = System.Windows.Forms.DockStyle.Fill
			Me.wizardCtrl.Name = "wizardCtrl"
			Me.wizardCtrl.Pages.AddRange(New DevExpress.XtraWizard.BaseWizardPage() {Me.pageMandantData, Me.pageInvoiceData, Me.pageCreateInvoice})
			Me.wizardCtrl.Size = New System.Drawing.Size(674, 527)
			Me.wizardCtrl.Text = ""
			'
			'pageMandantData
			'
			Me.pageMandantData.Controls.Add(Me.ucPageSelectMandant)
			Me.pageMandantData.Name = "pageMandantData"
			Me.pageMandantData.Size = New System.Drawing.Size(642, 384)
			'
			'ucPageSelectMandant
			'
			Me.ucPageSelectMandant.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucPageSelectMandant.Location = New System.Drawing.Point(0, 0)
			Me.ucPageSelectMandant.Name = "ucPageSelectMandant"
			Me.ucPageSelectMandant.PreselectionData = Nothing
			Me.ucPageSelectMandant.PreselectionPaymentData = Nothing
			Me.ucPageSelectMandant.Size = New System.Drawing.Size(642, 384)
			Me.ucPageSelectMandant.TabIndex = 0
			Me.ucPageSelectMandant.UCMediator = Nothing
			Me.ucPageSelectMandant.UCPaymentMediator = Nothing
			'
			'pageInvoiceData
			'
			Me.pageInvoiceData.Controls.Add(Me.ucPageSelectInvoiceData)
			Me.pageInvoiceData.Name = "pageInvoiceData"
			Me.pageInvoiceData.Size = New System.Drawing.Size(642, 384)
			'
			'ucPageSelectInvoiceData
			'
			Me.ucPageSelectInvoiceData.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucPageSelectInvoiceData.Location = New System.Drawing.Point(0, 0)
			Me.ucPageSelectInvoiceData.Name = "ucPageSelectInvoiceData"
			Me.ucPageSelectInvoiceData.PreselectionData = Nothing
			Me.ucPageSelectInvoiceData.PreselectionPaymentData = Nothing
			Me.ucPageSelectInvoiceData.Size = New System.Drawing.Size(642, 384)
			Me.ucPageSelectInvoiceData.TabIndex = 1
			Me.ucPageSelectInvoiceData.UCMediator = Nothing
			Me.ucPageSelectInvoiceData.UCPaymentMediator = Nothing
			'
			'pageCreateInvoice
			'
			Me.pageCreateInvoice.Controls.Add(Me.ucPageCreateInvoice)
			Me.pageCreateInvoice.Name = "pageCreateInvoice"
			Me.pageCreateInvoice.Size = New System.Drawing.Size(642, 384)
			'
			'ucPageCreateInvoice
			'
			Me.ucPageCreateInvoice.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucPageCreateInvoice.Location = New System.Drawing.Point(0, 0)
			Me.ucPageCreateInvoice.Name = "ucPageCreateInvoice"
			Me.ucPageCreateInvoice.PreselectionData = Nothing
			Me.ucPageCreateInvoice.PreselectionPaymentData = Nothing
			Me.ucPageCreateInvoice.Size = New System.Drawing.Size(642, 384)
			Me.ucPageCreateInvoice.TabIndex = 1
			Me.ucPageCreateInvoice.UCMediator = Nothing
			Me.ucPageCreateInvoice.UCPaymentMediator = Nothing
			'
			'frmNewInvoice
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(674, 527)
			Me.Controls.Add(Me.wizardCtrl)
			Me.IconOptions.Icon = CType(resources.GetObject("frmNewInvoice.IconOptions.Icon"), System.Drawing.Icon)
			Me.MinimumSize = New System.Drawing.Size(676, 559)
			Me.Name = "frmNewInvoice"
			Me.Text = "Neue Rechnung erfassen"
			CType(Me.object_b394e378_e210_4e67_a7af_192a6b8a5120, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.wizardCtrl, System.ComponentModel.ISupportInitialize).EndInit()
			Me.wizardCtrl.ResumeLayout(False)
			Me.pageMandantData.ResumeLayout(False)
			Me.pageInvoiceData.ResumeLayout(False)
			Me.pageCreateInvoice.ResumeLayout(False)
			Me.ResumeLayout(False)

		End Sub
		Friend WithEvents object_b394e378_e210_4e67_a7af_192a6b8a5120 As DevExpress.XtraBars.PopupControlContainer
    Friend WithEvents wizardCtrl As DevExpress.XtraWizard.WizardControl
    Friend WithEvents pageMandantData As DevExpress.XtraWizard.WizardPage
    Friend WithEvents pageInvoiceData As DevExpress.XtraWizard.WizardPage
    Friend WithEvents pageCreateInvoice As DevExpress.XtraWizard.WizardPage
    Friend WithEvents ucPageSelectMandant As SP.KD.InvoiceMng.UI.ucPageSelectMandant
    Friend WithEvents ucPageSelectInvoiceData As SP.KD.InvoiceMng.UI.ucPageSelectInvoiceData
    Friend WithEvents ucPageCreateInvoice As SP.KD.InvoiceMng.UI.ucPageCreateInvoice
  End Class

End Namespace
