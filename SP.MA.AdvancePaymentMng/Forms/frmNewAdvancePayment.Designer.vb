Namespace UI

  <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
  Partial Class frmNewAdvancePayment
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
			Me.wizardCtrl = New DevExpress.XtraWizard.WizardControl()
			Me.pageMandantData = New DevExpress.XtraWizard.WizardPage()
			Me.ucPageSelectMandant = New SP.MA.AdvancePaymentMng.UI.ucPageSelectMandant()
			Me.pageAmountOfPayment = New DevExpress.XtraWizard.WizardPage()
			Me.ucPageSelectAmountOfPayment = New SP.MA.AdvancePaymentMng.UI.ucPageSelectAmountOfPayment()
			Me.pageCreateAdvancePayment = New DevExpress.XtraWizard.WizardPage()
			Me.ucPageCreateZG = New SP.MA.AdvancePaymentMng.UI.ucPageCreateZG()
			CType(Me.wizardCtrl, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.wizardCtrl.SuspendLayout()
			Me.pageMandantData.SuspendLayout()
			Me.pageAmountOfPayment.SuspendLayout()
			Me.pageCreateAdvancePayment.SuspendLayout()
			Me.SuspendLayout()
			'
			'wizardCtrl
			'
			Me.wizardCtrl.AllowTransitionAnimation = False
			Me.wizardCtrl.Controls.Add(Me.pageMandantData)
			Me.wizardCtrl.Controls.Add(Me.pageAmountOfPayment)
			Me.wizardCtrl.Controls.Add(Me.pageCreateAdvancePayment)
			Me.wizardCtrl.Dock = System.Windows.Forms.DockStyle.Fill
			Me.wizardCtrl.Location = New System.Drawing.Point(0, 0)
			Me.wizardCtrl.Name = "wizardCtrl"
			Me.wizardCtrl.Pages.AddRange(New DevExpress.XtraWizard.BaseWizardPage() {Me.pageMandantData, Me.pageAmountOfPayment, Me.pageCreateAdvancePayment})
			Me.wizardCtrl.Size = New System.Drawing.Size(674, 660)
			Me.wizardCtrl.Text = ""
			'
			'pageMandantData
			'
			Me.pageMandantData.Controls.Add(Me.ucPageSelectMandant)
			Me.pageMandantData.Name = "pageMandantData"
			Me.pageMandantData.Size = New System.Drawing.Size(642, 515)
			'
			'ucPageSelectMandant
			'
			Me.ucPageSelectMandant.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.ucPageSelectMandant.Appearance.BackColor = System.Drawing.Color.White
			Me.ucPageSelectMandant.Appearance.Options.UseBackColor = True
			Me.ucPageSelectMandant.Location = New System.Drawing.Point(0, 0)
			Me.ucPageSelectMandant.Name = "ucPageSelectMandant"
			Me.ucPageSelectMandant.PreselectionData = Nothing
			Me.ucPageSelectMandant.Size = New System.Drawing.Size(642, 515)
			Me.ucPageSelectMandant.TabIndex = 1
			Me.ucPageSelectMandant.UCMediator = Nothing
			'
			'pageAmountOfPayment
			'
			Me.pageAmountOfPayment.Controls.Add(Me.ucPageSelectAmountOfPayment)
			Me.pageAmountOfPayment.Name = "pageAmountOfPayment"
			Me.pageAmountOfPayment.Size = New System.Drawing.Size(642, 515)
			'
			'ucPageSelectAmountOfPayment
			'
			Me.ucPageSelectAmountOfPayment.Appearance.BackColor = System.Drawing.Color.White
			Me.ucPageSelectAmountOfPayment.Appearance.Options.UseBackColor = True
			Me.ucPageSelectAmountOfPayment.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucPageSelectAmountOfPayment.Location = New System.Drawing.Point(0, 0)
			Me.ucPageSelectAmountOfPayment.Name = "ucPageSelectAmountOfPayment"
			Me.ucPageSelectAmountOfPayment.PreselectionData = Nothing
			Me.ucPageSelectAmountOfPayment.Size = New System.Drawing.Size(642, 515)
			Me.ucPageSelectAmountOfPayment.TabIndex = 3
			Me.ucPageSelectAmountOfPayment.UCMediator = Nothing
			'
			'pageCreateAdvancePayment
			'
			Me.pageCreateAdvancePayment.Controls.Add(Me.ucPageCreateZG)
			Me.pageCreateAdvancePayment.Name = "pageCreateAdvancePayment"
			Me.pageCreateAdvancePayment.Size = New System.Drawing.Size(642, 515)
			'
			'ucPageCreateZG
			'
			Me.ucPageCreateZG.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucPageCreateZG.Location = New System.Drawing.Point(0, 0)
			Me.ucPageCreateZG.Name = "ucPageCreateZG"
			Me.ucPageCreateZG.PreselectionData = Nothing
			Me.ucPageCreateZG.Size = New System.Drawing.Size(642, 515)
			Me.ucPageCreateZG.TabIndex = 2
			Me.ucPageCreateZG.UCMediator = Nothing
			'
			'frmNewAdvancePayment
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.AutoScroll = True
			Me.ClientSize = New System.Drawing.Size(674, 660)
			Me.Controls.Add(Me.wizardCtrl)
			Me.MinimumSize = New System.Drawing.Size(684, 692)
			Me.Name = "frmNewAdvancePayment"
			Me.Text = "Neuen Vorschuss erfassen"
			CType(Me.wizardCtrl, System.ComponentModel.ISupportInitialize).EndInit()
			Me.wizardCtrl.ResumeLayout(False)
			Me.pageMandantData.ResumeLayout(False)
			Me.pageAmountOfPayment.ResumeLayout(False)
			Me.pageCreateAdvancePayment.ResumeLayout(False)
			Me.ResumeLayout(False)

		End Sub
		Friend WithEvents wizardCtrl As DevExpress.XtraWizard.WizardControl
    Friend WithEvents pageMandantData As DevExpress.XtraWizard.WizardPage
		Friend WithEvents ucPageSelectMandant As SP.MA.AdvancePaymentMng.UI.ucPageSelectMandant
    Friend WithEvents pageAmountOfPayment As DevExpress.XtraWizard.WizardPage
    Friend WithEvents pageCreateAdvancePayment As DevExpress.XtraWizard.WizardPage
		Friend WithEvents ucPageSelectAmountOfPayment As SP.MA.AdvancePaymentMng.UI.ucPageSelectAmountOfPayment
    Friend WithEvents ucPageCreateZG As SP.MA.AdvancePaymentMng.UI.ucPageCreateZG
  End Class


End Namespace
