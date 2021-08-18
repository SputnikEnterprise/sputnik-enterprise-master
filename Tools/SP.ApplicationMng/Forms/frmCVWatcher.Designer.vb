Namespace UI

	<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
	Partial Class frmCVWatcher
		Inherits DevExpress.XtraEditors.XtraForm

		'Form overrides dispose to clean up the component list.
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
			Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCVWatcher))
			Me.pnlCVEMail = New DevExpress.XtraEditors.PanelControl()
			Me.btnClose = New DevExpress.XtraEditors.SimpleButton()
			CType(Me.pnlCVEMail, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			'
			'pnlCVEMail
			'
			Me.pnlCVEMail.Dock = System.Windows.Forms.DockStyle.Fill
			Me.pnlCVEMail.Location = New System.Drawing.Point(5, 5)
			Me.pnlCVEMail.Name = "pnlCVEMail"
			Me.pnlCVEMail.Size = New System.Drawing.Size(911, 565)
			Me.pnlCVEMail.TabIndex = 0
			'
			'btnClose
			'
			Me.btnClose.Dock = System.Windows.Forms.DockStyle.Bottom
			Me.btnClose.Location = New System.Drawing.Point(5, 570)
			Me.btnClose.Name = "btnClose"
			Me.btnClose.Size = New System.Drawing.Size(911, 32)
			Me.btnClose.TabIndex = 350
			Me.btnClose.Text = "Schliessen"
			'
			'frmCVWatcher
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(921, 607)
			Me.Controls.Add(Me.pnlCVEMail)
			Me.Controls.Add(Me.btnClose)
			Me.IconOptions.Icon = CType(resources.GetObject("frmCVWatcher.IconOptions.Icon"), System.Drawing.Icon)
			Me.Name = "frmCVWatcher"
			Me.Padding = New System.Windows.Forms.Padding(5)
			Me.Text = "E-Mail Watcher"
			CType(Me.pnlCVEMail, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub
		Friend WithEvents pnlCVEMail As DevExpress.XtraEditors.PanelControl
		Friend WithEvents btnClose As DevExpress.XtraEditors.SimpleButton
	End Class

End Namespace
