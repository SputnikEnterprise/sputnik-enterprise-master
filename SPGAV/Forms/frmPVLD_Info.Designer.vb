<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPVLD_Info
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPVLD_Info))
    Me.grpD_1 = New DevExpress.XtraEditors.GroupControl()
    Me.btnCopyGAVD_Info = New DevExpress.XtraEditors.SimpleButton()
    Me.scD_Info = New DevExpress.XtraEditors.XtraScrollableControl()
    Me.scD_2 = New DevExpress.XtraEditors.XtraScrollableControl()
    Me.scD_1 = New DevExpress.XtraEditors.XtraScrollableControl()
		Me.CmdClose = New DevExpress.XtraEditors.SimpleButton()
		Me.GroupBox1 = New System.Windows.Forms.GroupBox()
    Me.LblTimeValue = New System.Windows.Forms.Label()
    Me.LblSetting = New System.Windows.Forms.Label()
    Me.lblPath = New System.Windows.Forms.Label()
    Me.Label1 = New System.Windows.Forms.Label()
    CType(Me.grpD_1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.grpD_1.SuspendLayout()
    Me.GroupBox1.SuspendLayout()
    Me.SuspendLayout()
    '
    'grpD_1
    '
    Me.grpD_1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.grpD_1.AppearanceCaption.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
    Me.grpD_1.AppearanceCaption.Options.UseFont = True
    Me.grpD_1.Controls.Add(Me.btnCopyGAVD_Info)
    Me.grpD_1.Controls.Add(Me.scD_Info)
    Me.grpD_1.Controls.Add(Me.scD_2)
    Me.grpD_1.Controls.Add(Me.scD_1)
    Me.grpD_1.Location = New System.Drawing.Point(12, 91)
    Me.grpD_1.Name = "grpD_1"
    Me.grpD_1.Size = New System.Drawing.Size(940, 580)
    Me.grpD_1.TabIndex = 1080
    Me.grpD_1.Text = "GAV-Übersicht"
    Me.grpD_1.Visible = False
    '
    'btnCopyGAVD_Info
    '
    Me.btnCopyGAVD_Info.Image = CType(resources.GetObject("btnCopyGAVD_Info.Image"), System.Drawing.Image)
    Me.btnCopyGAVD_Info.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight
    Me.btnCopyGAVD_Info.Location = New System.Drawing.Point(912, 362)
    Me.btnCopyGAVD_Info.Name = "btnCopyGAVD_Info"
    Me.btnCopyGAVD_Info.Size = New System.Drawing.Size(23, 21)
    Me.btnCopyGAVD_Info.TabIndex = 1083
    '
    'scD_Info
    '
    Me.scD_Info.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.scD_Info.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
    Me.scD_Info.Appearance.Options.UseBackColor = True
    Me.scD_Info.Location = New System.Drawing.Point(2, 376)
    Me.scD_Info.Name = "scD_Info"
    Me.scD_Info.Size = New System.Drawing.Size(935, 199)
    Me.scD_Info.TabIndex = 2
    '
    'scD_2
    '
    Me.scD_2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.scD_2.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(128, Byte), Integer))
    Me.scD_2.Appearance.Options.UseBackColor = True
    Me.scD_2.Location = New System.Drawing.Point(473, 23)
    Me.scD_2.Name = "scD_2"
    Me.scD_2.Size = New System.Drawing.Size(464, 347)
    Me.scD_2.TabIndex = 1
    '
    'scD_1
    '
    Me.scD_1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.scD_1.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(128, Byte), Integer))
    Me.scD_1.Appearance.Options.UseBackColor = True
    Me.scD_1.Location = New System.Drawing.Point(2, 23)
    Me.scD_1.Name = "scD_1"
    Me.scD_1.Size = New System.Drawing.Size(464, 347)
    Me.scD_1.TabIndex = 0
    '
    'CmdClose
    '
    Me.CmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.CmdClose.Location = New System.Drawing.Point(867, 22)
    Me.CmdClose.Name = "CmdClose"
    Me.CmdClose.Size = New System.Drawing.Size(86, 24)
    Me.CmdClose.TabIndex = 999
    Me.CmdClose.Text = "Schliessen"
		'
		'GroupBox1
		'
		Me.GroupBox1.BackColor = System.Drawing.Color.White
    Me.GroupBox1.Controls.Add(Me.LblTimeValue)
    Me.GroupBox1.Controls.Add(Me.CmdClose)
    Me.GroupBox1.Controls.Add(Me.LblSetting)
    Me.GroupBox1.Controls.Add(Me.lblPath)
    Me.GroupBox1.Controls.Add(Me.Label1)
    Me.GroupBox1.Location = New System.Drawing.Point(-1, -8)
    Me.GroupBox1.Name = "GroupBox1"
    Me.GroupBox1.Size = New System.Drawing.Size(1028, 93)
    Me.GroupBox1.TabIndex = 1081
    Me.GroupBox1.TabStop = False
    '
    'LblTimeValue
    '
    Me.LblTimeValue.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.LblTimeValue.Location = New System.Drawing.Point(708, 53)
    Me.LblTimeValue.Name = "LblTimeValue"
    Me.LblTimeValue.Size = New System.Drawing.Size(245, 13)
    Me.LblTimeValue.TabIndex = 0
    Me.LblTimeValue.Text = "Zeitangaben..."
    Me.LblTimeValue.TextAlign = System.Drawing.ContentAlignment.TopRight
    Me.LblTimeValue.Visible = False
    '
    'LblSetting
    '
    Me.LblSetting.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.LblSetting.ForeColor = System.Drawing.SystemColors.HotTrack
    Me.LblSetting.Image = CType(resources.GetObject("LblSetting.Image"), System.Drawing.Image)
    Me.LblSetting.Location = New System.Drawing.Point(13, 17)
    Me.LblSetting.Name = "LblSetting"
    Me.LblSetting.Size = New System.Drawing.Size(44, 49)
    Me.LblSetting.TabIndex = 0
    '
    'lblPath
    '
    Me.lblPath.AutoSize = True
    Me.lblPath.ForeColor = System.Drawing.SystemColors.ControlText
    Me.lblPath.Location = New System.Drawing.Point(81, 53)
    Me.lblPath.Name = "lblPath"
    Me.lblPath.Size = New System.Drawing.Size(214, 13)
    Me.lblPath.TabIndex = 1
    Me.lblPath.Text = "Wählen Sie Ihre gewünschten Daten aus..."
    '
    'Label1
    '
    Me.Label1.AutoSize = True
    Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label1.Location = New System.Drawing.Point(63, 29)
    Me.Label1.Name = "Label1"
    Me.Label1.Size = New System.Drawing.Size(157, 13)
    Me.Label1.TabIndex = 0
    Me.Label1.Text = "Auswahl der Informationen"
    '
    'frmPVLD_Info
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(966, 686)
    Me.Controls.Add(Me.GroupBox1)
    Me.Controls.Add(Me.grpD_1)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Name = "frmPVLD_Info"
    Me.Text = "GAV-Informationen"
    CType(Me.grpD_1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.grpD_1.ResumeLayout(False)
    Me.GroupBox1.ResumeLayout(False)
    Me.GroupBox1.PerformLayout()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents grpD_1 As DevExpress.XtraEditors.GroupControl
  Friend WithEvents btnCopyGAVD_Info As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents scD_Info As DevExpress.XtraEditors.XtraScrollableControl
  Friend WithEvents scD_2 As DevExpress.XtraEditors.XtraScrollableControl
  Friend WithEvents scD_1 As DevExpress.XtraEditors.XtraScrollableControl
	Friend WithEvents CmdClose As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
  Friend WithEvents LblTimeValue As System.Windows.Forms.Label
  Friend WithEvents LblSetting As System.Windows.Forms.Label
  Friend WithEvents lblPath As System.Windows.Forms.Label
  Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
