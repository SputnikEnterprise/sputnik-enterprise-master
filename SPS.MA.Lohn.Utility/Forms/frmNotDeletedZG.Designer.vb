<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmNotDeletedZG
	Inherits DevExpress.XtraEditors.XtraForm
	'Inherits DevComponents.DotNetBar.Metro.MetroForm

	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmNotDeletedZG))
		Me.gridExistingAdvancePayment = New DevExpress.XtraGrid.GridControl()
		Me.gvExistingAdvancePayment = New DevExpress.XtraGrid.Views.Grid.GridView()
		CType(Me.gridExistingAdvancePayment, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvExistingAdvancePayment, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'gridExistingAdvancePayment
		'
		Me.gridExistingAdvancePayment.Cursor = System.Windows.Forms.Cursors.Default
		Me.gridExistingAdvancePayment.Dock = System.Windows.Forms.DockStyle.Fill
		Me.gridExistingAdvancePayment.Location = New System.Drawing.Point(5, 5)
		Me.gridExistingAdvancePayment.MainView = Me.gvExistingAdvancePayment
		Me.gridExistingAdvancePayment.Name = "gridExistingAdvancePayment"
		Me.gridExistingAdvancePayment.Size = New System.Drawing.Size(545, 202)
		Me.gridExistingAdvancePayment.TabIndex = 282
		Me.gridExistingAdvancePayment.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvExistingAdvancePayment})
		'
		'gvExistingAdvancePayment
		'
		Me.gvExistingAdvancePayment.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvExistingAdvancePayment.GridControl = Me.gridExistingAdvancePayment
		Me.gvExistingAdvancePayment.Name = "gvExistingAdvancePayment"
		Me.gvExistingAdvancePayment.OptionsBehavior.Editable = False
		Me.gvExistingAdvancePayment.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvExistingAdvancePayment.OptionsView.ShowGroupPanel = False
		'
		'frmNotDeletedZG
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(555, 212)
		Me.Controls.Add(Me.gridExistingAdvancePayment)
		Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.Name = "frmNotDeletedZG"
		Me.Padding = New System.Windows.Forms.Padding(5)
		Me.Text = "Liste der nicht gelöschten Auszahlungen (Vorschüsse)"
		CType(Me.gridExistingAdvancePayment, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvExistingAdvancePayment, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents gridExistingAdvancePayment As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvExistingAdvancePayment As DevExpress.XtraGrid.Views.Grid.GridView
End Class
