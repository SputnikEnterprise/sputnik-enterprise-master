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
      Me.gridBank = New DevExpress.XtraGrid.GridControl()
      Me.gvBank = New DevExpress.XtraGrid.Views.Grid.GridView()
      Me.btnAddBank = New DevExpress.XtraEditors.SimpleButton()
      Me.grpBank = New DevExpress.XtraEditors.GroupControl()
      CType(Me.gridBank, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.gvBank, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.grpBank, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.grpBank.SuspendLayout()
      Me.SuspendLayout()
      '
      'gridBank
      '
      Me.gridBank.Dock = System.Windows.Forms.DockStyle.Fill
      Me.gridBank.Location = New System.Drawing.Point(2, 21)
      Me.gridBank.MainView = Me.gvBank
      Me.gridBank.Name = "gridBank"
      Me.gridBank.Size = New System.Drawing.Size(827, 398)
      Me.gridBank.TabIndex = 1
      Me.gridBank.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvBank})
      '
      'gvBank
      '
      Me.gvBank.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
      Me.gvBank.GridControl = Me.gridBank
      Me.gvBank.Name = "gvBank"
      Me.gvBank.OptionsBehavior.Editable = False
      Me.gvBank.OptionsSelection.EnableAppearanceFocusedCell = False
      Me.gvBank.OptionsView.ShowGroupPanel = False
      '
      'btnAddBank
      '
      Me.btnAddBank.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
      Me.btnAddBank.Image = CType(resources.GetObject("btnAddBank.Image"), System.Drawing.Image)
      Me.btnAddBank.Location = New System.Drawing.Point(799, 3)
      Me.btnAddBank.Name = "btnAddBank"
      Me.btnAddBank.Size = New System.Drawing.Size(27, 15)
      Me.btnAddBank.TabIndex = 214
      '
      'grpBank
      '
      Me.grpBank.AppearanceCaption.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
      Me.grpBank.AppearanceCaption.Options.UseFont = True
      Me.grpBank.Controls.Add(Me.gridBank)
      Me.grpBank.Controls.Add(Me.btnAddBank)
      Me.grpBank.Dock = System.Windows.Forms.DockStyle.Fill
      Me.grpBank.Location = New System.Drawing.Point(0, 0)
      Me.grpBank.Name = "grpBank"
      Me.grpBank.Size = New System.Drawing.Size(831, 421)
      Me.grpBank.TabIndex = 270
      Me.grpBank.Text = "Bankdaten"
      '
      'ucBankData
      '
      Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
      Me.Controls.Add(Me.grpBank)
      Me.Name = "ucBankData"
      Me.Size = New System.Drawing.Size(831, 421)
      CType(Me.gridBank, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.gvBank, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.grpBank, System.ComponentModel.ISupportInitialize).EndInit()
      Me.grpBank.ResumeLayout(False)
      Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gridBank As DevExpress.XtraGrid.GridControl
    Friend WithEvents gvBank As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents btnAddBank As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents grpBank As DevExpress.XtraEditors.GroupControl

  End Class

End Namespace