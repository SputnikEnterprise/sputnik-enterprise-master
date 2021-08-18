<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMargenInfo
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
		Me.grdMargenInfo = New DevExpress.XtraGrid.GridControl()
		Me.gvMargenInfo = New DevExpress.XtraGrid.Views.Grid.GridView()
		CType(Me.grdMargenInfo, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvMargenInfo, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'grdReport
		'
		Me.grdMargenInfo.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdMargenInfo.Location = New System.Drawing.Point(5, 5)
		Me.grdMargenInfo.MainView = Me.gvMargenInfo
		Me.grdMargenInfo.Name = "grdReport"
		Me.grdMargenInfo.Size = New System.Drawing.Size(663, 664)
		Me.grdMargenInfo.TabIndex = 283
		Me.grdMargenInfo.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvMargenInfo})
		'
		'gvReport
		'
		Me.gvMargenInfo.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvMargenInfo.GridControl = Me.grdMargenInfo
		Me.gvMargenInfo.Name = "gvReport"
		Me.gvMargenInfo.OptionsBehavior.Editable = False
		Me.gvMargenInfo.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvMargenInfo.OptionsView.ShowGroupPanel = False
		'
		'frmMargenInfo
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(673, 674)
		Me.Controls.Add(Me.grdMargenInfo)
		Me.Name = "frmMargenInfo"
		Me.Padding = New System.Windows.Forms.Padding(5)
		Me.Text = "Informationen über die Bruttomarge"
		CType(Me.grdMargenInfo, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvMargenInfo, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents grdMargenInfo As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvMargenInfo As DevExpress.XtraGrid.Views.Grid.GridView
End Class
