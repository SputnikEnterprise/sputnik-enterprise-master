<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmConflictedRPL
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
    Me.grdRPL = New DevExpress.XtraGrid.GridControl()
    Me.gvRPL = New DevExpress.XtraGrid.Views.Grid.GridView()
    Me.btnOK = New DevExpress.XtraEditors.SimpleButton()
    Me.lblInfo = New DevExpress.XtraEditors.LabelControl()
    CType(Me.grdRPL, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.gvRPL, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'grdRPL
    '
    Me.grdRPL.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.grdRPL.Location = New System.Drawing.Point(8, 52)
    Me.grdRPL.MainView = Me.gvRPL
    Me.grdRPL.Name = "grdRPL"
    Me.grdRPL.Size = New System.Drawing.Size(402, 213)
    Me.grdRPL.TabIndex = 3
    Me.grdRPL.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvRPL})
    '
    'gvRPL
    '
    Me.gvRPL.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
    Me.gvRPL.GridControl = Me.grdRPL
    Me.gvRPL.Name = "gvRPL"
    Me.gvRPL.OptionsBehavior.Editable = False
    Me.gvRPL.OptionsSelection.EnableAppearanceFocusedCell = False
    Me.gvRPL.OptionsView.ShowGroupPanel = False
    '
    'btnOK
    '
    Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnOK.Location = New System.Drawing.Point(335, 271)
    Me.btnOK.Name = "btnOK"
    Me.btnOK.Size = New System.Drawing.Size(75, 23)
    Me.btnOK.TabIndex = 4
    Me.btnOK.Text = "OK"
    '
    'lblInfo
    '
    Me.lblInfo.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical
    Me.lblInfo.Location = New System.Drawing.Point(8, 9)
    Me.lblInfo.Name = "lblInfo"
    Me.lblInfo.Size = New System.Drawing.Size(402, 26)
    Me.lblInfo.TabIndex = 5
    Me.lblInfo.Text = "Der Datensatz kann nicht geändert/erfasst werden, da bereits Rapporte vorhanden s" & _
    "ind."
    '
    'frmConflictedRPL
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(418, 302)
    Me.ControlBox = False
    Me.Controls.Add(Me.lblInfo)
    Me.Controls.Add(Me.btnOK)
    Me.Controls.Add(Me.grdRPL)
    Me.Name = "frmConflictedRPL"
    Me.Padding = New System.Windows.Forms.Padding(5)
    Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
    Me.Text = "Bestehende Rapporte"
    CType(Me.grdRPL, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.gvRPL, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents btnOK As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents grdRPL As DevExpress.XtraGrid.GridControl
  Friend WithEvents gvRPL As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents lblInfo As DevExpress.XtraEditors.LabelControl
End Class
