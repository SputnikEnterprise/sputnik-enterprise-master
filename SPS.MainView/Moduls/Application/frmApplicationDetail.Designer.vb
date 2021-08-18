<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmApplicationDetail
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmApplicationDetail))
		Me.pnlDetail = New DevExpress.XtraEditors.PanelControl()
		Me.GroupBox1 = New DevExpress.XtraEditors.PanelControl()
		Me.btnClose = New DevExpress.XtraEditors.SimpleButton()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.lblHeaderNormal = New System.Windows.Forms.Label()
		Me.lblHeaderFett = New System.Windows.Forms.Label()
		CType(Me.pnlDetail, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupBox1.SuspendLayout()
		Me.SuspendLayout()
		'
		'pnlDetail
		'
		Me.pnlDetail.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.pnlDetail.Location = New System.Drawing.Point(0, 77)
		Me.pnlDetail.Name = "pnlDetail"
		Me.pnlDetail.Size = New System.Drawing.Size(791, 552)
		Me.pnlDetail.TabIndex = 0
		'
		'GroupBox1
		'
		Me.GroupBox1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003
		Me.GroupBox1.Controls.Add(Me.btnClose)
		Me.GroupBox1.Controls.Add(Me.Label2)
		Me.GroupBox1.Controls.Add(Me.lblHeaderNormal)
		Me.GroupBox1.Controls.Add(Me.lblHeaderFett)
		Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
		Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(791, 77)
		Me.GroupBox1.TabIndex = 208
		'
		'btnClose
		'
		Me.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Right
		Me.btnClose.Location = New System.Drawing.Point(663, 21)
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
		'lblHeaderNormal
		'
		Me.lblHeaderNormal.BackColor = System.Drawing.Color.Transparent
		Me.lblHeaderNormal.Location = New System.Drawing.Point(112, 45)
		Me.lblHeaderNormal.Name = "lblHeaderNormal"
		Me.lblHeaderNormal.Size = New System.Drawing.Size(299, 16)
		Me.lblHeaderNormal.TabIndex = 1
		Me.lblHeaderNormal.Text = "Geben Sie bitte Ihre gewünschten Kriterien ein."
		'
		'lblHeaderFett
		'
		Me.lblHeaderFett.AutoSize = True
		Me.lblHeaderFett.BackColor = System.Drawing.Color.Transparent
		Me.lblHeaderFett.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblHeaderFett.Location = New System.Drawing.Point(94, 21)
		Me.lblHeaderFett.Name = "lblHeaderFett"
		Me.lblHeaderFett.Size = New System.Drawing.Size(176, 13)
		Me.lblHeaderFett.TabIndex = 0
		Me.lblHeaderFett.Text = "Verwaltung von Bewerbungen"
		'
		'frmApplicationDetail
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(791, 629)
		Me.Controls.Add(Me.GroupBox1)
		Me.Controls.Add(Me.pnlDetail)
		Me.MinimumSize = New System.Drawing.Size(793, 661)
		Me.Name = "frmApplicationDetail"
		Me.Text = "Verwaltung von Bewerbungen"
		CType(Me.pnlDetail, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupBox1.ResumeLayout(False)
		Me.GroupBox1.PerformLayout()
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents pnlDetail As DevExpress.XtraEditors.PanelControl
	Friend WithEvents GroupBox1 As DevExpress.XtraEditors.PanelControl
	Friend WithEvents btnClose As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents Label2 As System.Windows.Forms.Label
	Friend WithEvents lblHeaderNormal As System.Windows.Forms.Label
	Friend WithEvents lblHeaderFett As System.Windows.Forms.Label
End Class
