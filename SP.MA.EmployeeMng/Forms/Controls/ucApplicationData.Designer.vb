Namespace UI


  <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
  Partial Class ucApplicationData
    Inherits ucBaseControl

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
			Me.chkAsActive = New DevExpress.XtraEditors.CheckEdit()
			Me.grdApplication = New DevExpress.XtraGrid.GridControl()
			Me.gvApplication = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.grpMonthlySalary = New DevExpress.XtraEditors.GroupControl()
			CType(Me.chkAsActive.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.grdApplication, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvApplication, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.grpMonthlySalary, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.grpMonthlySalary.SuspendLayout()
			Me.SuspendLayout()
			'
			'chkAsActive
			'
			Me.chkAsActive.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.chkAsActive.Location = New System.Drawing.Point(486, -1)
			Me.chkAsActive.Name = "chkAsActive"
			Me.chkAsActive.Properties.AllowFocused = False
			Me.chkAsActive.Properties.Appearance.Options.UseTextOptions = True
			Me.chkAsActive.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.chkAsActive.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.[Default]
			Me.chkAsActive.Properties.Caption = "Nur offene Bewerbungen anzeigen"
			Me.chkAsActive.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.chkAsActive.Size = New System.Drawing.Size(246, 19)
			Me.chkAsActive.TabIndex = 2
			'
			'grdApplication
			'
			Me.grdApplication.Dock = System.Windows.Forms.DockStyle.Fill
			Me.grdApplication.Location = New System.Drawing.Point(2, 20)
			Me.grdApplication.MainView = Me.gvApplication
			Me.grdApplication.Name = "grdApplication"
			Me.grdApplication.Size = New System.Drawing.Size(732, 299)
			Me.grdApplication.TabIndex = 1
			Me.grdApplication.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvApplication})
			'
			'gvApplication
			'
			Me.gvApplication.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			Me.gvApplication.GridControl = Me.grdApplication
			Me.gvApplication.Name = "gvApplication"
			Me.gvApplication.OptionsBehavior.Editable = False
			Me.gvApplication.OptionsSelection.EnableAppearanceFocusedCell = False
			Me.gvApplication.OptionsView.ShowGroupPanel = False
			'
			'grpMonthlySalary
			'
			Me.grpMonthlySalary.AppearanceCaption.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grpMonthlySalary.AppearanceCaption.Options.UseFont = True
			Me.grpMonthlySalary.Controls.Add(Me.chkAsActive)
			Me.grpMonthlySalary.Controls.Add(Me.grdApplication)
			Me.grpMonthlySalary.Dock = System.Windows.Forms.DockStyle.Fill
			Me.grpMonthlySalary.Location = New System.Drawing.Point(0, 0)
			Me.grpMonthlySalary.Name = "grpMonthlySalary"
			Me.grpMonthlySalary.Size = New System.Drawing.Size(736, 321)
			Me.grpMonthlySalary.TabIndex = 269
			Me.grpMonthlySalary.Text = "Bewerbungen"
			'
			'ucApplicationData
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.Controls.Add(Me.grpMonthlySalary)
			Me.Name = "ucApplicationData"
			Me.Size = New System.Drawing.Size(736, 321)
			CType(Me.chkAsActive.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.grdApplication, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvApplication, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.grpMonthlySalary, System.ComponentModel.ISupportInitialize).EndInit()
			Me.grpMonthlySalary.ResumeLayout(False)
			Me.ResumeLayout(False)

		End Sub
		Friend WithEvents chkAsActive As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents grdApplication As DevExpress.XtraGrid.GridControl
    Friend WithEvents gvApplication As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents grpMonthlySalary As DevExpress.XtraEditors.GroupControl

  End Class

End Namespace
