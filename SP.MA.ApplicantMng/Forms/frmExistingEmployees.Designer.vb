
Namespace UI


	<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
	Partial Class frmExistingEmployees
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
			Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmExistingEmployees))
			Me.GroupBox1 = New DevExpress.XtraEditors.PanelControl()
			Me.btnClose = New DevExpress.XtraEditors.SimpleButton()
			Me.Label2 = New System.Windows.Forms.Label()
			Me.lblHeaderFett = New System.Windows.Forms.Label()
			Me.pnlDetail = New DevExpress.XtraEditors.PanelControl()
			Me.btnSaveApplicationsToAssignedEmployee = New DevExpress.XtraEditors.SimpleButton()
			CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.GroupBox1.SuspendLayout()
			CType(Me.pnlDetail, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			'
			'GroupBox1
			'
			Me.GroupBox1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003
			Me.GroupBox1.Controls.Add(Me.btnClose)
			Me.GroupBox1.Controls.Add(Me.Label2)
			Me.GroupBox1.Controls.Add(Me.lblHeaderFett)
			Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
			Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
			Me.GroupBox1.Name = "GroupBox1"
			Me.GroupBox1.Size = New System.Drawing.Size(832, 77)
			Me.GroupBox1.TabIndex = 209
			'
			'btnClose
			'
			Me.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Right
			Me.btnClose.Location = New System.Drawing.Point(704, 21)
			Me.btnClose.Name = "btnClose"
			Me.btnClose.Size = New System.Drawing.Size(100, 25)
			Me.btnClose.TabIndex = 204
			Me.btnClose.Text = "Schliessen"
			'
			'Label2
			'
			Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
			Me.Label2.ForeColor = System.Drawing.SystemColors.HotTrack
			Me.Label2.Image = CType(resources.GetObject("Label2.Image"), System.Drawing.Image)
			Me.Label2.Location = New System.Drawing.Point(5, 8)
			Me.Label2.Name = "Label2"
			Me.Label2.Size = New System.Drawing.Size(83, 65)
			Me.Label2.TabIndex = 1000
			'
			'lblHeaderFett
			'
			Me.lblHeaderFett.AutoSize = True
			Me.lblHeaderFett.BackColor = System.Drawing.Color.Transparent
			Me.lblHeaderFett.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
			Me.lblHeaderFett.Location = New System.Drawing.Point(94, 21)
			Me.lblHeaderFett.Name = "lblHeaderFett"
			Me.lblHeaderFett.Size = New System.Drawing.Size(143, 13)
			Me.lblHeaderFett.TabIndex = 0
			Me.lblHeaderFett.Text = "Vorhandene Datensätze"
			'
			'pnlDetail
			'
			Me.pnlDetail.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
			Me.pnlDetail.Location = New System.Drawing.Point(21, 103)
			Me.pnlDetail.Name = "pnlDetail"
			Me.pnlDetail.Size = New System.Drawing.Size(653, 397)
			Me.pnlDetail.TabIndex = 210
			'
			'btnSaveApplicationsToAssignedEmployee
			'
			Me.btnSaveApplicationsToAssignedEmployee.Anchor = System.Windows.Forms.AnchorStyles.Right
			Me.btnSaveApplicationsToAssignedEmployee.Appearance.Options.UseTextOptions = True
			Me.btnSaveApplicationsToAssignedEmployee.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
			Me.btnSaveApplicationsToAssignedEmployee.Location = New System.Drawing.Point(704, 103)
			Me.btnSaveApplicationsToAssignedEmployee.Name = "btnSaveApplicationsToAssignedEmployee"
			Me.btnSaveApplicationsToAssignedEmployee.Size = New System.Drawing.Size(100, 40)
			Me.btnSaveApplicationsToAssignedEmployee.TabIndex = 211
			Me.btnSaveApplicationsToAssignedEmployee.Text = "Bewerbungen zuweisen"
			Me.btnSaveApplicationsToAssignedEmployee.Visible = False
			'
			'frmExistingEmployees
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(832, 523)
			Me.Controls.Add(Me.btnSaveApplicationsToAssignedEmployee)
			Me.Controls.Add(Me.pnlDetail)
			Me.Controls.Add(Me.GroupBox1)
			Me.MinimumSize = New System.Drawing.Size(842, 555)
			Me.Name = "frmExistingEmployees"
			Me.Text = "Vorhandene Datensätze"
			CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.GroupBox1.ResumeLayout(False)
			Me.GroupBox1.PerformLayout()
			CType(Me.pnlDetail, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		Friend WithEvents GroupBox1 As DevExpress.XtraEditors.PanelControl
		Friend WithEvents btnClose As DevExpress.XtraEditors.SimpleButton
		Friend WithEvents Label2 As Label
		Friend WithEvents lblHeaderFett As Label
		Friend WithEvents pnlDetail As DevExpress.XtraEditors.PanelControl
		Friend WithEvents btnSaveApplicationsToAssignedEmployee As DevExpress.XtraEditors.SimpleButton
	End Class

End Namespace