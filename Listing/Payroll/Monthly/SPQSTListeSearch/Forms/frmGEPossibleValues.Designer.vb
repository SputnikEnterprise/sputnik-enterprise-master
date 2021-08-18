<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGEPossibleValues
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
		Me.GroupControl1 = New DevExpress.XtraEditors.GroupControl()
		Me.grdCommunity = New DevExpress.XtraGrid.GridControl()
		Me.gvCommunity = New DevExpress.XtraGrid.Views.Grid.GridView()
		CType(Me.GroupControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupControl1.SuspendLayout()
		CType(Me.grdCommunity, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvCommunity, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'GroupControl1
		'
		Me.GroupControl1.Controls.Add(Me.grdCommunity)
		Me.GroupControl1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.GroupControl1.Location = New System.Drawing.Point(10, 10)
		Me.GroupControl1.Name = "GroupControl1"
		Me.GroupControl1.Size = New System.Drawing.Size(150, 610)
		Me.GroupControl1.TabIndex = 5
		Me.GroupControl1.Text = "Gemeinde"
		'
		'grdCommunity
		'
		Me.grdCommunity.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdCommunity.Location = New System.Drawing.Point(2, 21)
		Me.grdCommunity.MainView = Me.gvCommunity
		Me.grdCommunity.Name = "grdCommunity"
		Me.grdCommunity.Size = New System.Drawing.Size(146, 587)
		Me.grdCommunity.TabIndex = 289
		Me.grdCommunity.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvCommunity})
		'
		'gvCommunity
		'
		Me.gvCommunity.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvCommunity.GridControl = Me.grdCommunity
		Me.gvCommunity.Name = "gvCommunity"
		Me.gvCommunity.OptionsBehavior.Editable = False
		Me.gvCommunity.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvCommunity.OptionsView.ShowGroupPanel = False
		'
		'frmGEPossibleValues
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(170, 630)
		Me.Controls.Add(Me.GroupControl1)
		Me.Name = "frmGEPossibleValues"
		Me.Padding = New System.Windows.Forms.Padding(10)
		Me.Text = "Mögliche Eingaben für XML-Export"
		CType(Me.GroupControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupControl1.ResumeLayout(False)
		CType(Me.grdCommunity, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvCommunity, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents GroupControl1 As DevExpress.XtraEditors.GroupControl
	Friend WithEvents grdCommunity As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvCommunity As DevExpress.XtraGrid.Views.Grid.GridView
End Class
