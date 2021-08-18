
Namespace UI

  <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
  Partial Class frmNewEmployee
    Inherits SP.Infrastructure.Forms.frmBase

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
			Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmNewEmployee))
			Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider()
			Me.wizardCtrl = New DevExpress.XtraWizard.WizardControl()
			Me.pageWelcomePage = New DevExpress.XtraWizard.WizardPage()
			Me.ucWelcomePage = New SP.MA.EmployeeMng.UI.ucPageWelcome()
			Me.pageBasicDataPage = New DevExpress.XtraWizard.WizardPage()
			Me.ucEmployeeBasicDataPage = New SP.MA.EmployeeMng.UI.ucPageEmployeeBasicData()
			Me.pageAdditionalData1 = New DevExpress.XtraWizard.WizardPage()
			Me.ucEmployeeAdditionalData1Page = New SP.MA.EmployeeMng.UI.ucPageEmployeeAdditionalData1()
			Me.pageAdditionalData2 = New DevExpress.XtraWizard.WizardPage()
			Me.ucEmployeeAdditionalData2Page = New SP.MA.EmployeeMng.UI.ucPageEmployeeAdditionalData2()
			Me.pageCreateEmployee = New DevExpress.XtraWizard.WizardPage()
			Me.ucCreateEmployeePage = New SP.MA.EmployeeMng.UI.ucPageCreateEmployee()
			CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.wizardCtrl, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.wizardCtrl.SuspendLayout()
			Me.pageWelcomePage.SuspendLayout()
			Me.pageBasicDataPage.SuspendLayout()
			Me.pageAdditionalData1.SuspendLayout()
			Me.pageAdditionalData2.SuspendLayout()
			Me.pageCreateEmployee.SuspendLayout()
			Me.SuspendLayout()
			'
			'ErrorProvider1
			'
			Me.ErrorProvider1.BlinkRate = 0
			Me.ErrorProvider1.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
			Me.ErrorProvider1.ContainerControl = Me
			'
			'wizardCtrl
			'
			Me.wizardCtrl.AllowTransitionAnimation = False
			Me.wizardCtrl.Controls.Add(Me.pageWelcomePage)
			Me.wizardCtrl.Controls.Add(Me.pageBasicDataPage)
			Me.wizardCtrl.Controls.Add(Me.pageAdditionalData1)
			Me.wizardCtrl.Controls.Add(Me.pageAdditionalData2)
			Me.wizardCtrl.Controls.Add(Me.pageCreateEmployee)
			Me.wizardCtrl.Dock = System.Windows.Forms.DockStyle.Fill
			Me.wizardCtrl.Location = New System.Drawing.Point(0, 0)
			Me.wizardCtrl.Name = "wizardCtrl"
			Me.wizardCtrl.Pages.AddRange(New DevExpress.XtraWizard.BaseWizardPage() {Me.pageWelcomePage, Me.pageBasicDataPage, Me.pageAdditionalData1, Me.pageAdditionalData2, Me.pageCreateEmployee})
			Me.wizardCtrl.Size = New System.Drawing.Size(1090, 592)
			'
			'pageWelcomePage
			'
			Me.pageWelcomePage.Controls.Add(Me.ucWelcomePage)
			Me.pageWelcomePage.Name = "pageWelcomePage"
			Me.pageWelcomePage.Size = New System.Drawing.Size(1058, 447)
			'
			'ucWelcomePage
			'
			Me.ucWelcomePage.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.ucWelcomePage.Location = New System.Drawing.Point(0, 0)
			Me.ucWelcomePage.Name = "ucWelcomePage"
			Me.ucWelcomePage.PreselectionData = Nothing
			Me.ucWelcomePage.Size = New System.Drawing.Size(1058, 447)
			Me.ucWelcomePage.TabIndex = 0
			Me.ucWelcomePage.UCMediator = Nothing
			'
			'pageBasicDataPage
			'
			Me.pageBasicDataPage.Controls.Add(Me.ucEmployeeBasicDataPage)
			Me.pageBasicDataPage.Name = "pageBasicDataPage"
			Me.pageBasicDataPage.Size = New System.Drawing.Size(1058, 447)
			'
			'ucEmployeeBasicDataPage
			'
			Me.ucEmployeeBasicDataPage.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucEmployeeBasicDataPage.Location = New System.Drawing.Point(0, 0)
			Me.ucEmployeeBasicDataPage.Name = "ucEmployeeBasicDataPage"
			Me.ucEmployeeBasicDataPage.PreselectionData = Nothing
			Me.ucEmployeeBasicDataPage.Size = New System.Drawing.Size(1058, 447)
			Me.ucEmployeeBasicDataPage.TabIndex = 0
			Me.ucEmployeeBasicDataPage.UCMediator = Nothing
			'
			'pageAdditionalData1
			'
			Me.pageAdditionalData1.Controls.Add(Me.ucEmployeeAdditionalData1Page)
			Me.pageAdditionalData1.Name = "pageAdditionalData1"
			Me.pageAdditionalData1.Size = New System.Drawing.Size(1058, 447)
			'
			'ucEmployeeAdditionalData1Page
			'
			Me.ucEmployeeAdditionalData1Page.Appearance.BackColor = System.Drawing.Color.Transparent
			Me.ucEmployeeAdditionalData1Page.Appearance.Options.UseBackColor = True
			Me.ucEmployeeAdditionalData1Page.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucEmployeeAdditionalData1Page.Location = New System.Drawing.Point(0, 0)
			Me.ucEmployeeAdditionalData1Page.Name = "ucEmployeeAdditionalData1Page"
			Me.ucEmployeeAdditionalData1Page.PreselectionData = Nothing
			Me.ucEmployeeAdditionalData1Page.Size = New System.Drawing.Size(1058, 447)
			Me.ucEmployeeAdditionalData1Page.TabIndex = 0
			Me.ucEmployeeAdditionalData1Page.UCMediator = Nothing
			'
			'pageAdditionalData2
			'
			Me.pageAdditionalData2.Controls.Add(Me.ucEmployeeAdditionalData2Page)
			Me.pageAdditionalData2.Name = "pageAdditionalData2"
			Me.pageAdditionalData2.Size = New System.Drawing.Size(1058, 447)
			'
			'ucEmployeeAdditionalData2Page
			'
			Me.ucEmployeeAdditionalData2Page.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucEmployeeAdditionalData2Page.Location = New System.Drawing.Point(0, 0)
			Me.ucEmployeeAdditionalData2Page.Name = "ucEmployeeAdditionalData2Page"
			Me.ucEmployeeAdditionalData2Page.PreselectionData = Nothing
			Me.ucEmployeeAdditionalData2Page.Size = New System.Drawing.Size(1058, 447)
			Me.ucEmployeeAdditionalData2Page.TabIndex = 0
			Me.ucEmployeeAdditionalData2Page.UCMediator = Nothing
			'
			'pageCreateEmployee
			'
			Me.pageCreateEmployee.Controls.Add(Me.ucCreateEmployeePage)
			Me.pageCreateEmployee.Name = "pageCreateEmployee"
			Me.pageCreateEmployee.Size = New System.Drawing.Size(1058, 447)
			'
			'ucCreateEmployeePage
			'
			Me.ucCreateEmployeePage.Dock = System.Windows.Forms.DockStyle.Fill
			Me.ucCreateEmployeePage.Location = New System.Drawing.Point(0, 0)
			Me.ucCreateEmployeePage.Name = "ucCreateEmployeePage"
			Me.ucCreateEmployeePage.PreselectionData = Nothing
			Me.ucCreateEmployeePage.Size = New System.Drawing.Size(1058, 447)
			Me.ucCreateEmployeePage.TabIndex = 0
			Me.ucCreateEmployeePage.UCMediator = Nothing
			'
			'frmNewEmployee
			'
			Me.Appearance.BackColor = System.Drawing.Color.White
			Me.Appearance.Options.UseBackColor = True
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(1090, 592)
			Me.Controls.Add(Me.wizardCtrl)
			Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
			Me.MinimumSize = New System.Drawing.Size(1100, 624)
			Me.Name = "frmNewEmployee"
			Me.Text = "Neuer Kandidat"
			CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.wizardCtrl, System.ComponentModel.ISupportInitialize).EndInit()
			Me.wizardCtrl.ResumeLayout(False)
			Me.pageWelcomePage.ResumeLayout(False)
			Me.pageBasicDataPage.ResumeLayout(False)
			Me.pageAdditionalData1.ResumeLayout(False)
			Me.pageAdditionalData2.ResumeLayout(False)
			Me.pageCreateEmployee.ResumeLayout(False)
			Me.ResumeLayout(False)

		End Sub
		Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents wizardCtrl As DevExpress.XtraWizard.WizardControl
    Friend WithEvents pageWelcomePage As DevExpress.XtraWizard.WizardPage
    Friend WithEvents ucWelcomePage As SP.MA.EmployeeMng.UI.ucPageWelcome
    Friend WithEvents pageBasicDataPage As DevExpress.XtraWizard.WizardPage
    Friend WithEvents ucEmployeeBasicDataPage As SP.MA.EmployeeMng.UI.ucPageEmployeeBasicData
    Friend WithEvents pageAdditionalData1 As DevExpress.XtraWizard.WizardPage
    Friend WithEvents pageAdditionalData2 As DevExpress.XtraWizard.WizardPage
    Friend WithEvents ucCreateEmployeePage As SP.MA.EmployeeMng.UI.ucPageCreateEmployee
    Friend WithEvents ucEmployeeAdditionalData2Page As SP.MA.EmployeeMng.UI.ucPageEmployeeAdditionalData2
    Friend WithEvents pageCreateEmployee As DevExpress.XtraWizard.WizardPage
    Friend WithEvents ucEmployeeAdditionalData1Page As SP.MA.EmployeeMng.UI.ucPageEmployeeAdditionalData1
  End Class

End Namespace