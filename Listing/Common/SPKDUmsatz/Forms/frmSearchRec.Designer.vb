<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmSearchRec
	Inherits DevExpress.XtraEditors.XtraForm

	'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
	<System.Diagnostics.DebuggerNonUserCode()>
	Protected Overrides Sub Dispose(ByVal disposing As Boolean)
		Try
			If disposing AndAlso components IsNot Nothing Then
				components.Dispose()
			End If
		Finally
			MyBase.Dispose(disposing)
		End Try
	End Sub

	'Wird vom Windows Form-Designer benötigt.
	Private components As System.ComponentModel.IContainer

	'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
	'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
	'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
	<System.Diagnostics.DebuggerStepThrough()>
	Private Sub InitializeComponent()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSearchRec))
		Me.cmdOK = New DevExpress.XtraEditors.SimpleButton()
		Me.GroupBox1 = New DevExpress.XtraEditors.PanelControl()
		Me.CmdClose = New DevExpress.XtraEditors.SimpleButton()
		Me.lblHeaderfett = New System.Windows.Forms.Label()
		Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
		CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupBox1.SuspendLayout()
		CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'cmdOK
		'
		Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.cmdOK.Location = New System.Drawing.Point(438, 82)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.Size = New System.Drawing.Size(80, 25)
		Me.cmdOK.TabIndex = 219
		Me.cmdOK.Text = "Auswählen"
		'
		'GroupBox1
		'
		Me.GroupBox1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003
		Me.GroupBox1.Controls.Add(Me.CmdClose)
		Me.GroupBox1.Controls.Add(Me.lblHeaderfett)
		Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
		Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(530, 59)
		Me.GroupBox1.TabIndex = 218
		'
		'CmdClose
		'
		Me.CmdClose.Anchor = System.Windows.Forms.AnchorStyles.Right
		Me.CmdClose.Location = New System.Drawing.Point(438, 12)
		Me.CmdClose.Name = "CmdClose"
		Me.CmdClose.Size = New System.Drawing.Size(80, 25)
		Me.CmdClose.TabIndex = 204
		Me.CmdClose.Text = "Schliessen"
		'
		'lblHeaderfett
		'
		Me.lblHeaderfett.BackColor = System.Drawing.Color.Transparent
		Me.lblHeaderfett.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblHeaderfett.Image = CType(resources.GetObject("lblHeaderfett.Image"), System.Drawing.Image)
		Me.lblHeaderfett.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.lblHeaderfett.Location = New System.Drawing.Point(12, 13)
		Me.lblHeaderfett.Name = "lblHeaderfett"
		Me.lblHeaderfett.Size = New System.Drawing.Size(225, 31)
		Me.lblHeaderfett.TabIndex = 37
		Me.lblHeaderfett.Text = "Suche nach Datensätze"
		Me.lblHeaderfett.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'PanelControl1
		'
		Me.PanelControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.PanelControl1.Location = New System.Drawing.Point(15, 82)
		Me.PanelControl1.Name = "PanelControl1"
		Me.PanelControl1.Size = New System.Drawing.Size(417, 543)
		Me.PanelControl1.TabIndex = 220
		'
		'frmSearchRec
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(530, 627)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.GroupBox1)
		Me.Controls.Add(Me.PanelControl1)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.Name = "frmSearchRec"
		Me.Text = "Suche nach Datensätze"
		CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupBox1.ResumeLayout(False)
		CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents cmdOK As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents GroupBox1 As DevExpress.XtraEditors.PanelControl
	Friend WithEvents CmdClose As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents lblHeaderfett As System.Windows.Forms.Label
	Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
End Class
