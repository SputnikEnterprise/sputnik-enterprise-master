Namespace UI

  <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
  Partial Class ucNegativeSalaryData
    Inherits ucBaseControlBottomTab

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
			Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ucNegativeSalaryData))
			Me.grpMonthlySalary = New DevExpress.XtraEditors.GroupControl()
			Me.btnAddMonthlySalary = New DevExpress.XtraEditors.SimpleButton()
			Me.grdMSalary = New DevExpress.XtraGrid.GridControl()
			Me.gvMSalary = New DevExpress.XtraGrid.Views.Grid.GridView()
			CType(Me.grpMonthlySalary, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.grpMonthlySalary.SuspendLayout()
			CType(Me.grdMSalary, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvMSalary, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			'
			'grpMonthlySalary
			'
			Me.grpMonthlySalary.AppearanceCaption.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grpMonthlySalary.AppearanceCaption.Options.UseFont = True
			Me.grpMonthlySalary.Controls.Add(Me.btnAddMonthlySalary)
			Me.grpMonthlySalary.Controls.Add(Me.grdMSalary)
			Me.grpMonthlySalary.Dock = System.Windows.Forms.DockStyle.Fill
			Me.grpMonthlySalary.Location = New System.Drawing.Point(0, 0)
			Me.grpMonthlySalary.Name = "grpMonthlySalary"
			Me.grpMonthlySalary.Size = New System.Drawing.Size(756, 331)
			Me.grpMonthlySalary.TabIndex = 271
			Me.grpMonthlySalary.Text = "Negative Lohnlisten"
			'
			'btnAddMonthlySalary
			'
			Me.btnAddMonthlySalary.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnAddMonthlySalary.Image = CType(resources.GetObject("btnAddMonthlySalary.Image"), System.Drawing.Image)
			Me.btnAddMonthlySalary.Location = New System.Drawing.Point(724, 3)
			Me.btnAddMonthlySalary.Name = "btnAddMonthlySalary"
			Me.btnAddMonthlySalary.Size = New System.Drawing.Size(27, 15)
			Me.btnAddMonthlySalary.TabIndex = 4
			'
			'grdMSalary
			'
			Me.grdMSalary.Dock = System.Windows.Forms.DockStyle.Fill
			Me.grdMSalary.Location = New System.Drawing.Point(2, 21)
			Me.grdMSalary.MainView = Me.gvMSalary
			Me.grdMSalary.Name = "grdMSalary"
			Me.grdMSalary.Size = New System.Drawing.Size(752, 308)
			Me.grdMSalary.TabIndex = 1
			Me.grdMSalary.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvMSalary})
			'
			'gvMSalary
			'
			Me.gvMSalary.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			Me.gvMSalary.GridControl = Me.grdMSalary
			Me.gvMSalary.Name = "gvMSalary"
			Me.gvMSalary.OptionsBehavior.Editable = False
			Me.gvMSalary.OptionsSelection.EnableAppearanceFocusedCell = False
			Me.gvMSalary.OptionsView.ShowGroupPanel = False
			'
			'ucNegativeSalaryData
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.Controls.Add(Me.grpMonthlySalary)
			Me.Name = "ucNegativeSalaryData"
			Me.Size = New System.Drawing.Size(756, 331)
			CType(Me.grpMonthlySalary, System.ComponentModel.ISupportInitialize).EndInit()
			Me.grpMonthlySalary.ResumeLayout(False)
			CType(Me.grdMSalary, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvMSalary, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub
    Friend WithEvents grpMonthlySalary As DevExpress.XtraEditors.GroupControl
    Friend WithEvents grdMSalary As DevExpress.XtraGrid.GridControl
    Friend WithEvents gvMSalary As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents btnAddMonthlySalary As DevExpress.XtraEditors.SimpleButton

  End Class

End Namespace
