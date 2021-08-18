<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmCVLSearch
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCVLSearch))
		Me.GroupBox1 = New DevExpress.XtraEditors.PanelControl()
		Me.btnClose = New DevExpress.XtraEditors.SimpleButton()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.lblHeaderNormal = New System.Windows.Forms.Label()
		Me.lblHeaderFett = New System.Windows.Forms.Label()
		Me.pnlDetail = New DevExpress.XtraEditors.PanelControl()
		Me.Bar3 = New DevExpress.XtraBars.Bar()
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
		Me.GroupBox1.Controls.Add(Me.lblHeaderNormal)
		Me.GroupBox1.Controls.Add(Me.lblHeaderFett)
		Me.GroupBox1.Dock = DockStyle.Top
		Me.GroupBox1.Location = New System.Drawing.Point(5, 5)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(1074, 77)
		Me.GroupBox1.TabIndex = 209
		'
		'btnClose
		'
		Me.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Right
		Me.btnClose.Location = New System.Drawing.Point(922, 21)
		Me.btnClose.Name = "btnClose"
		Me.btnClose.Size = New System.Drawing.Size(119, 29)
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
		Me.lblHeaderFett.Size = New System.Drawing.Size(225, 13)
		Me.lblHeaderFett.TabIndex = 0
		Me.lblHeaderFett.Text = "Erweiterte Suche nach Bewerberdaten"
		'
		'pnlDetail
		'
		Me.pnlDetail.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.pnlDetail.Location = New System.Drawing.Point(5, 85)
		Me.pnlDetail.Name = "pnlDetail"
		Me.pnlDetail.Size = New System.Drawing.Size(1074, 519)
		Me.pnlDetail.TabIndex = 210
		'
		'Bar3
		'
		Me.Bar3.BarName = "Statusleiste"
		Me.Bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
		Me.Bar3.DockCol = 0
		Me.Bar3.DockRow = 0
		Me.Bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
		Me.Bar3.OptionsBar.AllowQuickCustomization = False
		Me.Bar3.OptionsBar.DrawDragBorder = False
		Me.Bar3.OptionsBar.UseWholeRow = True
		Me.Bar3.Text = "Statusleiste"
		'
		'frmCVLSearch
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(1084, 611)
		Me.Controls.Add(Me.pnlDetail)
		Me.Controls.Add(Me.GroupBox1)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MinimumSize = New System.Drawing.Size(1100, 650)
		Me.Name = "frmCVLSearch"
		Me.Padding = New System.Windows.Forms.Padding(5)
		Me.Text = "Erweiterte Suche nach Bewerberdaten"
		CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupBox1.ResumeLayout(False)
		Me.GroupBox1.PerformLayout()
		CType(Me.pnlDetail, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub

	Friend WithEvents GroupBox1 As DevExpress.XtraEditors.PanelControl
	Friend WithEvents btnClose As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents Label2 As Label
	Friend WithEvents lblHeaderNormal As Label
	Friend WithEvents lblHeaderFett As Label
	Friend WithEvents pnlDetail As DevExpress.XtraEditors.PanelControl
	Friend WithEvents Bar3 As DevExpress.XtraBars.Bar
End Class
