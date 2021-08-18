Namespace UI

  <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
  Partial Class ucPageEmployeeBasicData
		Inherits DevExpress.XtraEditors.XtraUserControl

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
			Me.errorProvider = New System.Windows.Forms.ErrorProvider()
			Me.grdExistingEmployees = New DevExpress.XtraGrid.GridControl()
			Me.gvExistingEmployees = New DevExpress.XtraGrid.Views.Grid.GridView()
			CType(Me.errorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.grdExistingEmployees, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvExistingEmployees, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			'
			'errorProvider
			'
			Me.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
			Me.errorProvider.ContainerControl = Me
			'
			'grdExistingEmployees
			'
			Me.grdExistingEmployees.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.grdExistingEmployees.Location = New System.Drawing.Point(13, 15)
			Me.grdExistingEmployees.MainView = Me.gvExistingEmployees
			Me.grdExistingEmployees.Name = "grdExistingEmployees"
			Me.grdExistingEmployees.Size = New System.Drawing.Size(839, 381)
			Me.grdExistingEmployees.TabIndex = 273
			Me.grdExistingEmployees.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvExistingEmployees})
			'
			'gvExistingEmployees
			'
			Me.gvExistingEmployees.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			Me.gvExistingEmployees.GridControl = Me.grdExistingEmployees
			Me.gvExistingEmployees.Name = "gvExistingEmployees"
			Me.gvExistingEmployees.OptionsBehavior.Editable = False
			Me.gvExistingEmployees.OptionsSelection.EnableAppearanceFocusedCell = False
			Me.gvExistingEmployees.OptionsView.ShowGroupPanel = False
			'
			'ucPageEmployeeBasicData
			'
			Me.Appearance.BackColor = System.Drawing.Color.White
			Me.Appearance.Options.UseBackColor = True
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.Controls.Add(Me.grdExistingEmployees)
			Me.Name = "ucPageEmployeeBasicData"
			Me.Padding = New System.Windows.Forms.Padding(10)
			Me.Size = New System.Drawing.Size(865, 412)
			CType(Me.errorProvider, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.grdExistingEmployees, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvExistingEmployees, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub
		Friend WithEvents errorProvider As System.Windows.Forms.ErrorProvider
		Friend WithEvents gvExistingEmployees As DevExpress.XtraGrid.Views.Grid.GridView
		Friend WithEvents grdExistingEmployees As DevExpress.XtraGrid.GridControl
	End Class

End Namespace
