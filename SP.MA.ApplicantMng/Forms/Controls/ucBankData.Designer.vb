Namespace UI

  <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
  Partial Class ucBankData
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
			Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ucBankData))
			Me.gridApplication = New DevExpress.XtraGrid.GridControl()
			Me.gvApplication = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.btnAddBank = New DevExpress.XtraEditors.SimpleButton()
			Me.grpApplication = New DevExpress.XtraEditors.GroupControl()
			CType(Me.gridApplication, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvApplication, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.grpApplication, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.grpApplication.SuspendLayout()
			Me.SuspendLayout()
			'
			'gridBank
			'
			Me.gridApplication.Dock = System.Windows.Forms.DockStyle.Fill
			Me.gridApplication.Location = New System.Drawing.Point(2, 20)
			Me.gridApplication.MainView = Me.gvApplication
			Me.gridApplication.Name = "gridBank"
			Me.gridApplication.Size = New System.Drawing.Size(732, 299)
			Me.gridApplication.TabIndex = 1
			Me.gridApplication.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvApplication})
			'
			'gvBank
			'
			Me.gvApplication.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			Me.gvApplication.GridControl = Me.gridApplication
			Me.gvApplication.Name = "gvBank"
			Me.gvApplication.OptionsBehavior.Editable = False
			Me.gvApplication.OptionsSelection.EnableAppearanceFocusedCell = False
			Me.gvApplication.OptionsView.ShowGroupPanel = False
			'
			'btnAddBank
			'
			Me.btnAddBank.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnAddBank.Image = CType(resources.GetObject("btnAddBank.Image"), System.Drawing.Image)
			Me.btnAddBank.Location = New System.Drawing.Point(704, 3)
			Me.btnAddBank.Name = "btnAddBank"
			Me.btnAddBank.Size = New System.Drawing.Size(27, 15)
			Me.btnAddBank.TabIndex = 214
			'
			'grpBank
			'
			Me.grpApplication.AppearanceCaption.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grpApplication.AppearanceCaption.Options.UseFont = True
			Me.grpApplication.Controls.Add(Me.gridApplication)
			Me.grpApplication.Controls.Add(Me.btnAddBank)
			Me.grpApplication.Dock = System.Windows.Forms.DockStyle.Fill
			Me.grpApplication.Location = New System.Drawing.Point(0, 0)
			Me.grpApplication.Name = "grpBank"
			Me.grpApplication.Size = New System.Drawing.Size(736, 321)
			Me.grpApplication.TabIndex = 270
			Me.grpApplication.Text = "Bewerbungen"
			'
			'ucBankData
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.Controls.Add(Me.grpApplication)
			Me.Name = "ucBankData"
			Me.Size = New System.Drawing.Size(736, 321)
			CType(Me.gridApplication, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvApplication, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.grpApplication, System.ComponentModel.ISupportInitialize).EndInit()
			Me.grpApplication.ResumeLayout(False)
			Me.ResumeLayout(False)

		End Sub
		Friend WithEvents gridApplication As DevExpress.XtraGrid.GridControl
		Friend WithEvents gvApplication As DevExpress.XtraGrid.Views.Grid.GridView
		Friend WithEvents btnAddBank As DevExpress.XtraEditors.SimpleButton
		Friend WithEvents grpApplication As DevExpress.XtraEditors.GroupControl

  End Class

End Namespace