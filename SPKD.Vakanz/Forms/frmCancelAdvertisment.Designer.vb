<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCancelAdvertisment
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCancelAdvertisment))
		Me.grpCancellationReason = New DevExpress.XtraEditors.GroupControl()
		Me.opCancellationReason = New DevExpress.XtraEditors.RadioGroup()
		Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
		Me.PictureEdit1 = New DevExpress.XtraEditors.PictureEdit()
		Me.lblHeader2 = New DevExpress.XtraEditors.LabelControl()
		Me.CmdClose = New DevExpress.XtraEditors.SimpleButton()
		Me.lblHeader1 = New System.Windows.Forms.Label()
		Me.cmdTransfer = New DevExpress.XtraEditors.SimpleButton()
		CType(Me.grpCancellationReason, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grpCancellationReason.SuspendLayout()
		CType(Me.opCancellationReason.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.PanelControl1.SuspendLayout()
		CType(Me.PictureEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'grpCancellationReason
		'
		Me.grpCancellationReason.AllowHtmlText = True
		Me.grpCancellationReason.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.grpCancellationReason.AppearanceCaption.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.grpCancellationReason.AppearanceCaption.Options.UseFont = True
		Me.grpCancellationReason.Controls.Add(Me.opCancellationReason)
		Me.grpCancellationReason.Location = New System.Drawing.Point(31, 105)
		Me.grpCancellationReason.Name = "grpCancellationReason"
		Me.grpCancellationReason.Padding = New System.Windows.Forms.Padding(5)
		Me.grpCancellationReason.Size = New System.Drawing.Size(676, 203)
		Me.grpCancellationReason.TabIndex = 0
		Me.grpCancellationReason.Text = "Möglichen Gründe für Absage der übermittlten Daten"
		'
		'opCancellationReason
		'
		Me.opCancellationReason.Dock = System.Windows.Forms.DockStyle.Fill
		Me.opCancellationReason.Location = New System.Drawing.Point(7, 25)
		Me.opCancellationReason.Name = "opCancellationReason"
		Me.opCancellationReason.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.opCancellationReason.Properties.Appearance.Options.UseBackColor = True
		Me.opCancellationReason.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
		Me.opCancellationReason.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem(CType(0, Short), "Sie konnten die Stelle besetzen in Zusammenarbeit mit der regionalen Arbeitsvermi" &
										"ttlung (RAV): OCCUPIED_JOBCENTER"), New DevExpress.XtraEditors.Controls.RadioGroupItem(CType(1, Short), "Sie konnten die Stelle besetzen in Zusammenarbeit mit der privaten Arbeitsvermitt" &
										"lung (pAV):  OCCUPIED_AGENCY"), New DevExpress.XtraEditors.Controls.RadioGroupItem(CType(2, Short), "Sie konnten die Stelle selber besetzen mit einer Kandidatin/einem Kandidaten aus " &
										"Job-Room: OCCUPIED_JOBROOM"), New DevExpress.XtraEditors.Controls.RadioGroupItem(CType(3, Short), "Sie konnten die Stelle selber besetzen anderweitig: OCCUPIED_OTHER"), New DevExpress.XtraEditors.Controls.RadioGroupItem(CType(4, Short), "Sie konnten die Stelle nicht besetzen: NOT_OCCUPIED"), New DevExpress.XtraEditors.Controls.RadioGroupItem(CType(5, Short), "Sie möchten die Ausschreibung ändern oder neu erfassen: CHANGE_OR_REPOSE")})
		Me.opCancellationReason.Size = New System.Drawing.Size(662, 171)
		Me.opCancellationReason.TabIndex = 1001
		'
		'PanelControl1
		'
		Me.PanelControl1.Controls.Add(Me.PictureEdit1)
		Me.PanelControl1.Controls.Add(Me.lblHeader2)
		Me.PanelControl1.Controls.Add(Me.CmdClose)
		Me.PanelControl1.Controls.Add(Me.lblHeader1)
		Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Top
		Me.PanelControl1.Location = New System.Drawing.Point(0, 0)
		Me.PanelControl1.Name = "PanelControl1"
		Me.PanelControl1.Size = New System.Drawing.Size(863, 79)
		Me.PanelControl1.TabIndex = 234
		'
		'PictureEdit1
		'
		Me.PictureEdit1.EditValue = CType(resources.GetObject("PictureEdit1.EditValue"), Object)
		Me.PictureEdit1.Location = New System.Drawing.Point(12, 12)
		Me.PictureEdit1.Name = "PictureEdit1"
		Me.PictureEdit1.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.PictureEdit1.Properties.Appearance.Options.UseBackColor = True
		Me.PictureEdit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
		Me.PictureEdit1.Properties.PictureAlignment = System.Drawing.ContentAlignment.TopLeft
		Me.PictureEdit1.Size = New System.Drawing.Size(40, 45)
		Me.PictureEdit1.TabIndex = 233
		'
		'lblHeader2
		'
		Me.lblHeader2.Location = New System.Drawing.Point(110, 44)
		Me.lblHeader2.Name = "lblHeader2"
		Me.lblHeader2.Size = New System.Drawing.Size(218, 13)
		Me.lblHeader2.TabIndex = 1
		Me.lblHeader2.Text = "Wählen Sie Ihre gewünschten Kriterien aus..."
		'
		'CmdClose
		'
		Me.CmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.CmdClose.Location = New System.Drawing.Point(737, 22)
		Me.CmdClose.Name = "CmdClose"
		Me.CmdClose.Size = New System.Drawing.Size(86, 24)
		Me.CmdClose.TabIndex = 999
		Me.CmdClose.Text = "Schliessen"
		'
		'lblHeader1
		'
		Me.lblHeader1.AutoSize = True
		Me.lblHeader1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblHeader1.Location = New System.Drawing.Point(84, 22)
		Me.lblHeader1.Name = "lblHeader1"
		Me.lblHeader1.Size = New System.Drawing.Size(272, 13)
		Me.lblHeader1.TabIndex = 0
		Me.lblHeader1.Text = "Möglichen Gründe für Absage der Übermittlung"
		'
		'cmdTransfer
		'
		Me.cmdTransfer.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.cmdTransfer.Location = New System.Drawing.Point(737, 105)
		Me.cmdTransfer.Name = "cmdTransfer"
		Me.cmdTransfer.Size = New System.Drawing.Size(86, 24)
		Me.cmdTransfer.TabIndex = 1000
		Me.cmdTransfer.Text = "Übermitteln"
		'
		'frmCancelAdvertisment
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(863, 351)
		Me.Controls.Add(Me.cmdTransfer)
		Me.Controls.Add(Me.PanelControl1)
		Me.Controls.Add(Me.grpCancellationReason)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.Name = "frmCancelAdvertisment"
		Me.Text = "AVAM-Stelle abmelden"
		CType(Me.grpCancellationReason, System.ComponentModel.ISupportInitialize).EndInit()
		Me.grpCancellationReason.ResumeLayout(False)
		CType(Me.opCancellationReason.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.PanelControl1.ResumeLayout(False)
		Me.PanelControl1.PerformLayout()
		CType(Me.PictureEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub

	Friend WithEvents grpCancellationReason As DevExpress.XtraEditors.GroupControl
	Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
	Friend WithEvents PictureEdit1 As DevExpress.XtraEditors.PictureEdit
	Friend WithEvents lblHeader2 As DevExpress.XtraEditors.LabelControl
	Friend WithEvents CmdClose As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents lblHeader1 As Label
	Friend WithEvents cmdTransfer As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents opCancellationReason As DevExpress.XtraEditors.RadioGroup
End Class
