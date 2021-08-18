
Namespace UI


	<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
	Partial Class frmLibraryInfo
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
			Me.grdRP = New DevExpress.XtraGrid.GridControl()
			Me.gvRP = New DevExpress.XtraGrid.Views.Grid.GridView()
			CType(Me.grdRP, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvRP, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			'
			'grdRP
			'
			Me.grdRP.Cursor = System.Windows.Forms.Cursors.Default
			Me.grdRP.Dock = System.Windows.Forms.DockStyle.Fill
			Me.grdRP.Location = New System.Drawing.Point(0, 0)
			Me.grdRP.MainView = Me.gvRP
			Me.grdRP.Name = "grdRP"
			Me.grdRP.Size = New System.Drawing.Size(756, 310)
			Me.grdRP.TabIndex = 4
			Me.grdRP.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvRP})
			'
			'gvRP
			'
			Me.gvRP.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			Me.gvRP.GridControl = Me.grdRP
			Me.gvRP.Name = "gvRP"
			Me.gvRP.OptionsBehavior.Editable = False
			Me.gvRP.OptionsSelection.EnableAppearanceFocusedCell = False
			Me.gvRP.OptionsView.ShowAutoFilterRow = True
			Me.gvRP.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			Me.gvRP.OptionsView.ShowGroupPanel = False
			Me.gvRP.OptionsView.ShowIndicator = False
			'
			'frmLibraryInfo
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(756, 310)
			Me.Controls.Add(Me.grdRP)
			Me.Name = "frmLibraryInfo"
			Me.Text = "Informationen über Bibliothen"
			Me.TopMost = True
			CType(Me.grdRP, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvRP, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub
		Friend WithEvents grdRP As DevExpress.XtraGrid.GridControl
		Friend WithEvents gvRP As DevExpress.XtraGrid.Views.Grid.GridView
	End Class


End Namespace
