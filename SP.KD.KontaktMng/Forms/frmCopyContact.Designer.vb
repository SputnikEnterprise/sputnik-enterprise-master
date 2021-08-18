<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCopyContact
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCopyContact))
    Me.SplitContainerControl1 = New DevExpress.XtraEditors.SplitContainerControl()
    Me.gridAllEmployees = New DevExpress.XtraGrid.GridControl()
    Me.gvAllEmployees = New DevExpress.XtraGrid.Views.Grid.GridView()
    Me.gridSelectedEmployees = New DevExpress.XtraGrid.GridControl()
    Me.gvSelectedEmployees = New DevExpress.XtraGrid.Views.Grid.GridView()
    Me.btnCopyContact = New DevExpress.XtraEditors.SimpleButton()
    Me.lblInfo = New DevExpress.XtraEditors.LabelControl()
    Me.btnCancel = New DevExpress.XtraEditors.SimpleButton()
    CType(Me.SplitContainerControl1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SplitContainerControl1.SuspendLayout()
    CType(Me.gridAllEmployees, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.gvAllEmployees, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.gridSelectedEmployees, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.gvSelectedEmployees, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'SplitContainerControl1
    '
    Me.SplitContainerControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.SplitContainerControl1.Location = New System.Drawing.Point(4, 37)
    Me.SplitContainerControl1.Name = "SplitContainerControl1"
    Me.SplitContainerControl1.Panel1.Controls.Add(Me.gridAllEmployees)
    Me.SplitContainerControl1.Panel1.Text = "Panel1"
    Me.SplitContainerControl1.Panel2.Controls.Add(Me.gridSelectedEmployees)
    Me.SplitContainerControl1.Panel2.Text = "Panel2"
    Me.SplitContainerControl1.Size = New System.Drawing.Size(698, 476)
    Me.SplitContainerControl1.SplitterPosition = 323
    Me.SplitContainerControl1.TabIndex = 0
    Me.SplitContainerControl1.Text = "SplitContainerControl1"
    '
    'gridAllEmployees
    '
    Me.gridAllEmployees.Dock = System.Windows.Forms.DockStyle.Fill
    Me.gridAllEmployees.Location = New System.Drawing.Point(0, 0)
    Me.gridAllEmployees.MainView = Me.gvAllEmployees
    Me.gridAllEmployees.Name = "gridAllEmployees"
    Me.gridAllEmployees.Size = New System.Drawing.Size(323, 476)
    Me.gridAllEmployees.TabIndex = 2
    Me.gridAllEmployees.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvAllEmployees})
    '
    'gvAllEmployees
    '
    Me.gvAllEmployees.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
    Me.gvAllEmployees.GridControl = Me.gridAllEmployees
    Me.gvAllEmployees.Name = "gvAllEmployees"
    Me.gvAllEmployees.OptionsBehavior.Editable = False
    Me.gvAllEmployees.OptionsSelection.EnableAppearanceFocusedCell = False
    Me.gvAllEmployees.OptionsView.ShowGroupPanel = False
    '
    'gridSelectedEmployees
    '
    Me.gridSelectedEmployees.Dock = System.Windows.Forms.DockStyle.Fill
    Me.gridSelectedEmployees.Location = New System.Drawing.Point(0, 0)
    Me.gridSelectedEmployees.MainView = Me.gvSelectedEmployees
    Me.gridSelectedEmployees.Name = "gridSelectedEmployees"
    Me.gridSelectedEmployees.Size = New System.Drawing.Size(370, 476)
    Me.gridSelectedEmployees.TabIndex = 2
    Me.gridSelectedEmployees.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvSelectedEmployees})
    '
    'gvSelectedEmployees
    '
    Me.gvSelectedEmployees.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
    Me.gvSelectedEmployees.GridControl = Me.gridSelectedEmployees
    Me.gvSelectedEmployees.Name = "gvSelectedEmployees"
    Me.gvSelectedEmployees.OptionsBehavior.Editable = False
    Me.gvSelectedEmployees.OptionsSelection.EnableAppearanceFocusedCell = False
    Me.gvSelectedEmployees.OptionsView.ShowGroupPanel = False
    '
    'btnCopyContact
    '
    Me.btnCopyContact.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnCopyContact.Location = New System.Drawing.Point(497, 524)
    Me.btnCopyContact.Name = "btnCopyContact"
    Me.btnCopyContact.Size = New System.Drawing.Size(99, 23)
    Me.btnCopyContact.TabIndex = 1
    Me.btnCopyContact.Text = "Kontakt kopieren"
    '
    'lblInfo
    '
    Me.lblInfo.Location = New System.Drawing.Point(4, 12)
    Me.lblInfo.Name = "lblInfo"
    Me.lblInfo.Size = New System.Drawing.Size(359, 13)
    Me.lblInfo.TabIndex = 2
    Me.lblInfo.Text = "Bitte wählen Sie die Kandidaten für welche der Kontakt kopiert werden soll."
    '
    'btnCancel
    '
    Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnCancel.Location = New System.Drawing.Point(603, 523)
    Me.btnCancel.Name = "btnCancel"
    Me.btnCancel.Size = New System.Drawing.Size(99, 23)
    Me.btnCancel.TabIndex = 3
    Me.btnCancel.Text = "Schliessen"
    '
    'frmCopyContact
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(707, 558)
    Me.Controls.Add(Me.btnCancel)
    Me.Controls.Add(Me.lblInfo)
    Me.Controls.Add(Me.btnCopyContact)
    Me.Controls.Add(Me.SplitContainerControl1)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Name = "frmCopyContact"
    Me.Text = "Kontakt kopieren"
    CType(Me.SplitContainerControl1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.SplitContainerControl1.ResumeLayout(False)
    CType(Me.gridAllEmployees, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.gvAllEmployees, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.gridSelectedEmployees, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.gvSelectedEmployees, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents SplitContainerControl1 As DevExpress.XtraEditors.SplitContainerControl
  Friend WithEvents btnCopyContact As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents gridAllEmployees As DevExpress.XtraGrid.GridControl
  Friend WithEvents gvAllEmployees As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents gridSelectedEmployees As DevExpress.XtraGrid.GridControl
  Friend WithEvents gvSelectedEmployees As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents lblInfo As DevExpress.XtraEditors.LabelControl
  Friend WithEvents btnCancel As DevExpress.XtraEditors.SimpleButton
End Class
