<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmConflictedMonthClose
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
    Me.grdMonthCloseRecords = New DevExpress.XtraGrid.GridControl()
    Me.gvMonthCloseRecords = New DevExpress.XtraGrid.Views.Grid.GridView()
    Me.btnOK = New DevExpress.XtraEditors.SimpleButton()
    Me.lblInfo = New DevExpress.XtraEditors.LabelControl()
    CType(Me.grdMonthCloseRecords, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.gvMonthCloseRecords, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'grdMonthCloseRecords
    '
    Me.grdMonthCloseRecords.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.grdMonthCloseRecords.Location = New System.Drawing.Point(8, 52)
    Me.grdMonthCloseRecords.MainView = Me.gvMonthCloseRecords
    Me.grdMonthCloseRecords.Name = "grdMonthCloseRecords"
    Me.grdMonthCloseRecords.Size = New System.Drawing.Size(402, 213)
    Me.grdMonthCloseRecords.TabIndex = 3
    Me.grdMonthCloseRecords.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvMonthCloseRecords})
    '
    'gvMonthCloseRecords
    '
    Me.gvMonthCloseRecords.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
    Me.gvMonthCloseRecords.GridControl = Me.grdMonthCloseRecords
    Me.gvMonthCloseRecords.Name = "gvMonthCloseRecords"
    Me.gvMonthCloseRecords.OptionsBehavior.Editable = False
    Me.gvMonthCloseRecords.OptionsSelection.EnableAppearanceFocusedCell = False
    Me.gvMonthCloseRecords.OptionsView.ShowGroupPanel = False
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
    Me.lblInfo.Text = "Der Datensatz kann nicht geändert/erfasst werden, da bereits verbuchte Monate vor" & _
    "handen sind."
    '
    'frmConflictedMonthClose
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(418, 302)
    Me.ControlBox = False
    Me.Controls.Add(Me.lblInfo)
    Me.Controls.Add(Me.btnOK)
    Me.Controls.Add(Me.grdMonthCloseRecords)
    Me.Name = "frmConflictedMonthClose"
    Me.Padding = New System.Windows.Forms.Padding(5)
    Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
    Me.Text = "Bestehende Abgeschlossene Monate"
    CType(Me.grdMonthCloseRecords, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.gvMonthCloseRecords, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents btnOK As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents grdMonthCloseRecords As DevExpress.XtraGrid.GridControl
  Friend WithEvents gvMonthCloseRecords As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents lblInfo As DevExpress.XtraEditors.LabelControl
End Class
