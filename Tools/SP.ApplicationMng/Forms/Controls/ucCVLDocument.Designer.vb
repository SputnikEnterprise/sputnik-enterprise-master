Namespace CVLizer.UI


	<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
	Partial Class ucCVLDocument
		Inherits ucBaseControl

		'UserControl overrides dispose to clean up the component list.
		<System.Diagnostics.DebuggerNonUserCode()>
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
		<System.Diagnostics.DebuggerStepThrough()>
		Private Sub InitializeComponent()
			Me.grdDocument = New DevExpress.XtraGrid.GridControl()
			Me.gvDocument = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.GridView3 = New DevExpress.XtraGrid.Views.Grid.GridView()
			CType(Me.grdDocument, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvDocument, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.GridView3, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			'
			'grdDocument
			'
			Me.grdDocument.Dock = System.Windows.Forms.DockStyle.Fill
			Me.grdDocument.Location = New System.Drawing.Point(0, 0)
			Me.grdDocument.MainView = Me.gvDocument
			Me.grdDocument.Name = "grdDocument"
			Me.grdDocument.Size = New System.Drawing.Size(1153, 359)
			Me.grdDocument.TabIndex = 332
			Me.grdDocument.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvDocument, Me.GridView3})
			'
			'gvDocument
			'
			Me.gvDocument.ActiveFilterEnabled = False
			Me.gvDocument.GridControl = Me.grdDocument
			Me.gvDocument.Name = "gvDocument"
			Me.gvDocument.OptionsBehavior.Editable = False
			Me.gvDocument.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never
			'
			'GridView3
			'
			Me.GridView3.GridControl = Me.grdDocument
			Me.GridView3.Name = "GridView3"
			'
			'ucCVLDocument
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.Controls.Add(Me.grdDocument)
			Me.Name = "ucCVLDocument"
			Me.Size = New System.Drawing.Size(1153, 359)
			CType(Me.grdDocument, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvDocument, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.GridView3, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		Friend WithEvents grdDocument As DevExpress.XtraGrid.GridControl
		Friend WithEvents gvDocument As DevExpress.XtraGrid.Views.Grid.GridView
		Friend WithEvents GridView3 As DevExpress.XtraGrid.Views.Grid.GridView
	End Class
End Namespace
