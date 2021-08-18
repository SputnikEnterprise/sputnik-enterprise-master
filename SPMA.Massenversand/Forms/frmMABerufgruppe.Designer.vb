<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMABerufgruppe
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
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMABerufgruppe))
		Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Me.lvMABerufe = New System.Windows.Forms.ListView()
		Me.cmdDelete = New DevExpress.XtraEditors.SimpleButton()
		Me.Panel1 = New System.Windows.Forms.Panel()
		Me.cmdClose = New DevExpress.XtraEditors.SimpleButton()
		Me.LblSetting = New System.Windows.Forms.Label()
		Me.lblHeaderFett = New System.Windows.Forms.Label()
		Me.lblJBerufErfahrung_0 = New System.Windows.Forms.Label()
		Me.lblJBerufErfahrungFach_0 = New System.Windows.Forms.Label()
		Me.LookUpEdit15 = New DevExpress.XtraEditors.LookUpEdit()
		Me.LookUpEdit16 = New DevExpress.XtraEditors.LookUpEdit()
		Me.cmdSave = New DevExpress.XtraEditors.SimpleButton()
		Me.lblBerufgruppe = New System.Windows.Forms.Label()
		Me.lblFachbereiche = New System.Windows.Forms.Label()
		Me.ToolTipController1 = New DevExpress.Utils.ToolTipController(Me.components)
		Me.Panel1.SuspendLayout()
		CType(Me.LookUpEdit15.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.LookUpEdit16.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'lvMABerufe
		'
		Me.lvMABerufe.Location = New System.Drawing.Point(22, 128)
		Me.lvMABerufe.Name = "lvMABerufe"
		Me.lvMABerufe.Size = New System.Drawing.Size(499, 188)
		Me.lvMABerufe.TabIndex = 0
		Me.lvMABerufe.UseCompatibleStateImageBehavior = False
		Me.lvMABerufe.View = System.Windows.Forms.View.Details
		'
		'cmdDelete
		'
		Me.cmdDelete.Image = CType(resources.GetObject("cmdDelete.Image"), System.Drawing.Image)
		Me.cmdDelete.Location = New System.Drawing.Point(535, 134)
		Me.cmdDelete.Name = "cmdDelete"
		Me.cmdDelete.Size = New System.Drawing.Size(90, 26)
		Me.cmdDelete.TabIndex = 3
		Me.cmdDelete.Text = "Löschen"
		'
		'Panel1
		'
		Me.Panel1.BackColor = System.Drawing.Color.White
		Me.Panel1.Controls.Add(Me.cmdClose)
		Me.Panel1.Controls.Add(Me.LblSetting)
		Me.Panel1.Controls.Add(Me.lblHeaderFett)
		Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
		Me.Panel1.Location = New System.Drawing.Point(0, 0)
		Me.Panel1.Name = "Panel1"
		Me.Panel1.Size = New System.Drawing.Size(666, 64)
		Me.Panel1.TabIndex = 4
		'
		'cmdClose
		'
		Me.cmdClose.Location = New System.Drawing.Point(535, 19)
		Me.cmdClose.Name = "cmdClose"
		Me.cmdClose.Size = New System.Drawing.Size(90, 26)
		Me.cmdClose.TabIndex = 192
		Me.cmdClose.Text = "Schliessen"
		'
		'LblSetting
		'
		Me.LblSetting.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.LblSetting.ForeColor = System.Drawing.SystemColors.HotTrack
		Me.LblSetting.Image = CType(resources.GetObject("LblSetting.Image"), System.Drawing.Image)
		Me.LblSetting.Location = New System.Drawing.Point(12, 9)
		Me.LblSetting.Name = "LblSetting"
		Me.LblSetting.Size = New System.Drawing.Size(46, 44)
		Me.LblSetting.TabIndex = 192
		'
		'lblHeaderFett
		'
		Me.lblHeaderFett.AutoSize = True
		Me.lblHeaderFett.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblHeaderFett.Location = New System.Drawing.Point(64, 19)
		Me.lblHeaderFett.Name = "lblHeaderFett"
		Me.lblHeaderFett.Size = New System.Drawing.Size(369, 13)
		Me.lblHeaderFett.TabIndex = 190
		Me.lblHeaderFett.Text = "Details über Berufsgruppen und Fachbereiche eines Kandidaten"
		'
		'lblJBerufErfahrung_0
		'
		Me.lblJBerufErfahrung_0.AutoSize = True
		Me.lblJBerufErfahrung_0.Location = New System.Drawing.Point(523, 174)
		Me.lblJBerufErfahrung_0.Name = "lblJBerufErfahrung_0"
		Me.lblJBerufErfahrung_0.Size = New System.Drawing.Size(108, 13)
		Me.lblJBerufErfahrung_0.TabIndex = 187
		Me.lblJBerufErfahrung_0.Text = "lblJBerufErfahrung_0"
		Me.lblJBerufErfahrung_0.Visible = False
		'
		'lblJBerufErfahrungFach_0
		'
		Me.lblJBerufErfahrungFach_0.AutoSize = True
		Me.lblJBerufErfahrungFach_0.Location = New System.Drawing.Point(523, 198)
		Me.lblJBerufErfahrungFach_0.Name = "lblJBerufErfahrungFach_0"
		Me.lblJBerufErfahrungFach_0.Size = New System.Drawing.Size(131, 13)
		Me.lblJBerufErfahrungFach_0.TabIndex = 188
		Me.lblJBerufErfahrungFach_0.Text = "lblJBerufErfahrungFach_0"
		Me.lblJBerufErfahrungFach_0.Visible = False
		'
		'LookUpEdit15
		'
		Me.LookUpEdit15.Location = New System.Drawing.Point(22, 102)
		Me.LookUpEdit15.Name = "LookUpEdit15"
		SerializableAppearanceObject1.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject1.Options.UseForeColor = True
		Me.LookUpEdit15.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, "", Nothing, Nothing, True)})
		Me.LookUpEdit15.Properties.NullText = ""
		Me.LookUpEdit15.Size = New System.Drawing.Size(245, 20)
		Me.LookUpEdit15.TabIndex = 0
		'
		'LookUpEdit16
		'
		Me.LookUpEdit16.Location = New System.Drawing.Point(276, 102)
		Me.LookUpEdit16.Name = "LookUpEdit16"
		SerializableAppearanceObject2.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject2.Options.UseForeColor = True
		Me.LookUpEdit16.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject2, "", Nothing, Nothing, True)})
		Me.LookUpEdit16.Properties.NullText = ""
		Me.LookUpEdit16.Size = New System.Drawing.Size(245, 20)
		Me.LookUpEdit16.TabIndex = 1
		'
		'cmdSave
		'
		Me.cmdSave.Image = CType(resources.GetObject("cmdSave.Image"), System.Drawing.Image)
		Me.cmdSave.Location = New System.Drawing.Point(535, 100)
		Me.cmdSave.Name = "cmdSave"
		Me.cmdSave.Size = New System.Drawing.Size(90, 26)
		Me.cmdSave.TabIndex = 2
		Me.cmdSave.Text = "Speichern"
		'
		'lblBerufgruppe
		'
		Me.lblBerufgruppe.AutoSize = True
		Me.lblBerufgruppe.Location = New System.Drawing.Point(22, 83)
		Me.lblBerufgruppe.Name = "lblBerufgruppe"
		Me.lblBerufgruppe.Size = New System.Drawing.Size(72, 13)
		Me.lblBerufgruppe.TabIndex = 190
		Me.lblBerufgruppe.Text = "Berufsgruppe"
		'
		'lblFachbereiche
		'
		Me.lblFachbereiche.AutoSize = True
		Me.lblFachbereiche.Location = New System.Drawing.Point(273, 83)
		Me.lblFachbereiche.Name = "lblFachbereiche"
		Me.lblFachbereiche.Size = New System.Drawing.Size(65, 13)
		Me.lblFachbereiche.TabIndex = 191
		Me.lblFachbereiche.Text = "Fachbereich"
		'
		'frmMABerufgruppe
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(666, 343)
		Me.Controls.Add(Me.lblBerufgruppe)
		Me.Controls.Add(Me.lblFachbereiche)
		Me.Controls.Add(Me.cmdSave)
		Me.Controls.Add(Me.lblJBerufErfahrung_0)
		Me.Controls.Add(Me.Panel1)
		Me.Controls.Add(Me.lblJBerufErfahrungFach_0)
		Me.Controls.Add(Me.cmdDelete)
		Me.Controls.Add(Me.LookUpEdit15)
		Me.Controls.Add(Me.LookUpEdit16)
		Me.Controls.Add(Me.lvMABerufe)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximumSize = New System.Drawing.Size(682, 381)
		Me.Name = "frmMABerufgruppe"
		Me.Text = "Berufsgruppen und Fachbereiche"
		Me.Panel1.ResumeLayout(False)
		Me.Panel1.PerformLayout()
		CType(Me.LookUpEdit15.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.LookUpEdit16.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents lvMABerufe As System.Windows.Forms.ListView
	Friend WithEvents cmdDelete As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents Panel1 As System.Windows.Forms.Panel
	Friend WithEvents lblJBerufErfahrung_0 As System.Windows.Forms.Label
	Friend WithEvents lblJBerufErfahrungFach_0 As System.Windows.Forms.Label
	Friend WithEvents LookUpEdit15 As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents LookUpEdit16 As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents cmdSave As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents LblSetting As System.Windows.Forms.Label
	Friend WithEvents lblHeaderFett As System.Windows.Forms.Label
	Friend WithEvents lblBerufgruppe As System.Windows.Forms.Label
	Friend WithEvents lblFachbereiche As System.Windows.Forms.Label
  Friend WithEvents cmdClose As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents ToolTipController1 As DevExpress.Utils.ToolTipController
End Class
