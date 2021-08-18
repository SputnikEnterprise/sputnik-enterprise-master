Namespace UI

  <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
  Partial Class frmNewES
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
			Me.pageCandidateAndCustomer = New DevExpress.XtraWizard.WizardPage()
			Me.ucCandidateAndCustomer = New SP.MA.EinsatzMng.UI.ucPageSelectCandidateAndCustomer()
			Me.pageESData = New DevExpress.XtraWizard.WizardPage()
			Me.ucESData = New SP.MA.EinsatzMng.UI.ucPageSelectESData()
			Me.pageSalaryData = New DevExpress.XtraWizard.WizardPage()
			Me.ucSalaryData = New SP.MA.EinsatzMng.UI.ucPageSelectSalaryData()
			Me.pageCreateES = New DevExpress.XtraWizard.WizardPage()
			Me.ucCreateES = New SP.MA.EinsatzMng.UI.ucPageCreateES()
			CType(Me.wizardCtrl, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.wizardCtrl.SuspendLayout()
			Me.pageCandidateAndCustomer.SuspendLayout()
			Me.pageESData.SuspendLayout()
			Me.pageSalaryData.SuspendLayout()
			Me.pageCreateES.SuspendLayout()
			Me.SuspendLayout()
			'
			'wizardCtrl
			'
			Me.wizardCtrl.AllowTransitionAnimation = False
			Me.wizardCtrl.Controls.Add(Me.pageCandidateAndCustomer)
			Me.wizardCtrl.Controls.Add(Me.pageESData)
			Me.wizardCtrl.Controls.Add(Me.pageSalaryData)
			Me.wizardCtrl.Controls.Add(Me.pageCreateES)
			Me.wizardCtrl.Dock = System.Windows.Forms.DockStyle.Fill
			Me.wizardCtrl.Location = New System.Drawing.Point(0, 0)
			Me.wizardCtrl.Name = "wizardCtrl"
			Me.wizardCtrl.Pages.AddRange(New DevExpress.XtraWizard.BaseWizardPage() {Me.pageCandidateAndCustomer, Me.pageESData, Me.pageSalaryData, Me.pageCreateES})
			Me.wizardCtrl.Size = New System.Drawing.Size(700, 634)
			Me.wizardCtrl.Text = ""
			'
			'pageCandidateAndCustomer
			'
			Me.pageCandidateAndCustomer.Controls.Add(Me.ucCandidateAndCustomer)
			Me.pageCandidateAndCustomer.Name = "pageCandidateAndCustomer"
			Me.pageCandidateAndCustomer.Size = New System.Drawing.Size(668, 489)
			'
			'ucCandidateAndCustomer
			'
			Me.ucCandidateAndCustomer.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.ucCandidateAndCustomer.Appearance.BackColor = System.Drawing.Color.White
			Me.ucCandidateAndCustomer.Appearance.Options.UseBackColor = True
			Me.ucCandidateAndCustomer.Location = New System.Drawing.Point(0, 0)
			Me.ucCandidateAndCustomer.Name = "ucCandidateAndCustomer"
			Me.ucCandidateAndCustomer.PreselectionData = Nothing
			Me.ucCandidateAndCustomer.Size = New System.Drawing.Size(668, 489)
			Me.ucCandidateAndCustomer.TabIndex = 0
			Me.ucCandidateAndCustomer.UCMediator = Nothing
			'
			'pageESData
			'
			Me.pageESData.Controls.Add(Me.ucESData)
			Me.pageESData.Name = "pageESData"
			Me.pageESData.Size = New System.Drawing.Size(668, 489)
			'
			'ucESData
			'
			Me.ucESData.Appearance.BackColor = System.Drawing.Color.White
			Me.ucESData.Appearance.Options.UseBackColor = True
			Me.ucESData.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucESData.Location = New System.Drawing.Point(0, 0)
			Me.ucESData.Name = "ucESData"
			Me.ucESData.PreselectionData = Nothing
			Me.ucESData.Size = New System.Drawing.Size(668, 489)
			Me.ucESData.TabIndex = 0
			Me.ucESData.UCMediator = Nothing
			'
			'pageSalaryData
			'
			Me.pageSalaryData.Controls.Add(Me.ucSalaryData)
			Me.pageSalaryData.Name = "pageSalaryData"
			Me.pageSalaryData.Size = New System.Drawing.Size(668, 489)
			'
			'ucSalaryData
			'
			Me.ucSalaryData.Appearance.BackColor = System.Drawing.Color.White
			Me.ucSalaryData.Appearance.Options.UseBackColor = True
			Me.ucSalaryData.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucSalaryData.ESNr = Nothing
			Me.ucSalaryData.Location = New System.Drawing.Point(0, 0)
			Me.ucSalaryData.Name = "ucSalaryData"
			Me.ucSalaryData.PreselectionData = Nothing
			Me.ucSalaryData.Size = New System.Drawing.Size(668, 489)
			Me.ucSalaryData.TabIndex = 0
			Me.ucSalaryData.UCMediator = Nothing
			'
			'pageCreateES
			'
			Me.pageCreateES.Controls.Add(Me.ucCreateES)
			Me.pageCreateES.Name = "pageCreateES"
			Me.pageCreateES.Size = New System.Drawing.Size(668, 489)
			'
			'ucCreateES
			'
			Me.ucCreateES.Appearance.BackColor = System.Drawing.Color.White
			Me.ucCreateES.Appearance.Options.UseBackColor = True
			Me.ucCreateES.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucCreateES.Location = New System.Drawing.Point(0, 0)
			Me.ucCreateES.Name = "ucCreateES"
			Me.ucCreateES.PreselectionData = Nothing
			Me.ucCreateES.Size = New System.Drawing.Size(668, 489)
			Me.ucCreateES.TabIndex = 0
			Me.ucCreateES.UCMediator = Nothing
			'
			'frmNewES
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(700, 634)
			Me.Controls.Add(Me.wizardCtrl)
			Me.MinimumSize = New System.Drawing.Size(710, 666)
			Me.Name = "frmNewES"
			Me.Text = "Neuer Einsatz erfassen"
			CType(Me.wizardCtrl, System.ComponentModel.ISupportInitialize).EndInit()
			Me.wizardCtrl.ResumeLayout(False)
			Me.pageCandidateAndCustomer.ResumeLayout(False)
			Me.pageESData.ResumeLayout(False)
			Me.pageSalaryData.ResumeLayout(False)
			Me.pageCreateES.ResumeLayout(False)
			Me.ResumeLayout(False)

		End Sub
		Friend WithEvents wizardCtrl As DevExpress.XtraWizard.WizardControl
    Friend WithEvents pageCandidateAndCustomer As DevExpress.XtraWizard.WizardPage
    Friend WithEvents ucCandidateAndCustomer As SP.MA.EinsatzMng.UI.ucPageSelectCandidateAndCustomer
    Friend WithEvents pageESData As DevExpress.XtraWizard.WizardPage
    Friend WithEvents ucESData As SP.MA.EinsatzMng.UI.ucPageSelectESData
    Friend WithEvents pageSalaryData As DevExpress.XtraWizard.WizardPage
    Friend WithEvents ucSalaryData As SP.MA.EinsatzMng.UI.ucPageSelectSalaryData
    Friend WithEvents pageCreateES As DevExpress.XtraWizard.WizardPage
    Friend WithEvents ucCreateES As SP.MA.EinsatzMng.UI.ucPageCreateES
  End Class

End Namespace