<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmNewMDYear
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmNewMDYear))
		Me.GroupBox1 = New System.Windows.Forms.GroupBox()
		Me.CmdClose = New DevExpress.XtraEditors.SimpleButton()
		Me.Label3 = New System.Windows.Forms.Label()
		Me.LblTimeValue = New System.Windows.Forms.Label()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.LabelControl3 = New DevExpress.XtraEditors.LabelControl()
		Me.ListBoxControl1 = New DevExpress.XtraEditors.ListBoxControl()
		Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl()
		Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl()
		Me.cboToYear = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.cboFromYear = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.CmdOpen = New DevExpress.XtraEditors.SimpleButton()
		Me.GroupBox1.SuspendLayout()
		CType(Me.ListBoxControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.cboToYear.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.cboFromYear.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'GroupBox1
		'
		Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.GroupBox1.BackColor = System.Drawing.Color.White
		Me.GroupBox1.Controls.Add(Me.CmdClose)
		Me.GroupBox1.Controls.Add(Me.Label3)
		Me.GroupBox1.Controls.Add(Me.LblTimeValue)
		Me.GroupBox1.Controls.Add(Me.Label2)
		Me.GroupBox1.Controls.Add(Me.Label1)
		Me.GroupBox1.Location = New System.Drawing.Point(-1, -7)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(773, 87)
		Me.GroupBox1.TabIndex = 1007
		Me.GroupBox1.TabStop = False
		'
		'CmdClose
		'
		Me.CmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.CmdClose.Location = New System.Drawing.Point(511, 21)
		Me.CmdClose.Name = "CmdClose"
		Me.CmdClose.Size = New System.Drawing.Size(86, 29)
		Me.CmdClose.TabIndex = 1009
		Me.CmdClose.Text = "Schliessen"
		'
		'Label3
		'
		Me.Label3.BackColor = System.Drawing.Color.White
		Me.Label3.Image = CType(resources.GetObject("Label3.Image"), System.Drawing.Image)
		Me.Label3.Location = New System.Drawing.Point(13, 17)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(47, 47)
		Me.Label3.TabIndex = 1009
		Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'LblTimeValue
		'
		Me.LblTimeValue.Anchor = System.Windows.Forms.AnchorStyles.Right
		Me.LblTimeValue.Location = New System.Drawing.Point(459, 56)
		Me.LblTimeValue.Name = "LblTimeValue"
		Me.LblTimeValue.Size = New System.Drawing.Size(136, 13)
		Me.LblTimeValue.TabIndex = 14
		Me.LblTimeValue.Text = "Zeitangaben..."
		Me.LblTimeValue.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.LblTimeValue.Visible = False
		'
		'Label2
		'
		Me.Label2.Location = New System.Drawing.Point(84, 48)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(299, 16)
		Me.Label2.TabIndex = 1
		Me.Label2.Text = "Geben Sie bitte Ihre gewünschten Kriterien ein."
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label1.Location = New System.Drawing.Point(66, 29)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(122, 13)
		Me.Label1.TabIndex = 0
		Me.Label1.Text = "Neues Jahr eröffnen"
		'
		'LabelControl3
		'
		Me.LabelControl3.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.LabelControl3.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.TopRight
		Me.LabelControl3.LineOrientation = DevExpress.XtraEditors.LabelLineOrientation.Horizontal
		Me.LabelControl3.Location = New System.Drawing.Point(15, 157)
		Me.LabelControl3.Name = "LabelControl3"
		Me.LabelControl3.Size = New System.Drawing.Size(132, 13)
		Me.LabelControl3.TabIndex = 1006
		Me.LabelControl3.Text = "Detail"
		'
		'ListBoxControl1
		'
		Me.ListBoxControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.ListBoxControl1.Location = New System.Drawing.Point(15, 176)
		Me.ListBoxControl1.Name = "ListBoxControl1"
		Me.ListBoxControl1.Size = New System.Drawing.Size(581, 268)
		Me.ListBoxControl1.TabIndex = 3
		'
		'LabelControl2
		'
		Me.LabelControl2.Appearance.Options.UseTextOptions = True
		Me.LabelControl2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.LabelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.LabelControl2.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.TopRight
		Me.LabelControl2.LineOrientation = DevExpress.XtraEditors.LabelLineOrientation.Horizontal
		Me.LabelControl2.Location = New System.Drawing.Point(15, 138)
		Me.LabelControl2.Name = "LabelControl2"
		Me.LabelControl2.Size = New System.Drawing.Size(161, 13)
		Me.LabelControl2.TabIndex = 1004
		Me.LabelControl2.Text = "Ziel-Jahr"
		'
		'LabelControl1
		'
		Me.LabelControl1.Appearance.Options.UseTextOptions = True
		Me.LabelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.LabelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.LabelControl1.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.TopRight
		Me.LabelControl1.Location = New System.Drawing.Point(15, 112)
		Me.LabelControl1.Name = "LabelControl1"
		Me.LabelControl1.Size = New System.Drawing.Size(161, 13)
		Me.LabelControl1.TabIndex = 1003
		Me.LabelControl1.Text = "Aus welchem Jahr"
		'
		'cboToYear
		'
		Me.cboToYear.Location = New System.Drawing.Point(182, 134)
		Me.cboToYear.Name = "cboToYear"
		Me.cboToYear.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.cboToYear.Size = New System.Drawing.Size(115, 20)
		Me.cboToYear.TabIndex = 1
		'
		'cboFromYear
		'
		Me.cboFromYear.Location = New System.Drawing.Point(182, 108)
		Me.cboFromYear.Name = "cboFromYear"
		Me.cboFromYear.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.cboFromYear.Size = New System.Drawing.Size(115, 20)
		Me.cboFromYear.TabIndex = 0
		'
		'CmdOpen
		'
		Me.CmdOpen.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.CmdOpen.Location = New System.Drawing.Point(508, 108)
		Me.CmdOpen.Name = "CmdOpen"
		Me.CmdOpen.Size = New System.Drawing.Size(86, 29)
		Me.CmdOpen.TabIndex = 1008
		Me.CmdOpen.Text = "Eröffnen"
		'
		'frmNewMDYear
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(616, 465)
		Me.Controls.Add(Me.CmdOpen)
		Me.Controls.Add(Me.GroupBox1)
		Me.Controls.Add(Me.LabelControl3)
		Me.Controls.Add(Me.ListBoxControl1)
		Me.Controls.Add(Me.LabelControl2)
		Me.Controls.Add(Me.LabelControl1)
		Me.Controls.Add(Me.cboToYear)
		Me.Controls.Add(Me.cboFromYear)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MinimumSize = New System.Drawing.Size(626, 497)
		Me.Name = "frmNewMDYear"
		Me.Text = "Mandantendaten für neues Jahr eröffnen"
		Me.GroupBox1.ResumeLayout(False)
		Me.GroupBox1.PerformLayout()
		CType(Me.ListBoxControl1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.cboToYear.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.cboFromYear.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
  Friend WithEvents LblTimeValue As System.Windows.Forms.Label
  Friend WithEvents Label2 As System.Windows.Forms.Label
  Friend WithEvents Label1 As System.Windows.Forms.Label
  Friend WithEvents LabelControl3 As DevExpress.XtraEditors.LabelControl
  Friend WithEvents ListBoxControl1 As DevExpress.XtraEditors.ListBoxControl
  Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
  Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
  Friend WithEvents cboToYear As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents cboFromYear As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents Label3 As System.Windows.Forms.Label
  Friend WithEvents CmdOpen As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents CmdClose As DevExpress.XtraEditors.SimpleButton
End Class
