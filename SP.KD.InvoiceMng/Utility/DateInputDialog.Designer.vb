<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DateInputDialog
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
		Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Me.dateEditControl = New DevExpress.XtraEditors.DateEdit()
		Me.lblMessage = New DevExpress.XtraEditors.LabelControl()
		Me.btnOk = New DevExpress.XtraEditors.SimpleButton()
		Me.btnCancel = New DevExpress.XtraEditors.SimpleButton()
		CType(Me.dateEditControl.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.dateEditControl.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'dateEditControl
		'
		Me.dateEditControl.EditValue = Nothing
		Me.dateEditControl.Location = New System.Drawing.Point(205, 12)
		Me.dateEditControl.Name = "dateEditControl"
		SerializableAppearanceObject1.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject1.Options.UseForeColor = True
		Me.dateEditControl.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, "", Nothing, Nothing, True)})
		Me.dateEditControl.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.dateEditControl.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None
		Me.dateEditControl.Properties.MinValue = New Date(1800, 1, 1, 0, 0, 0, 0)
		Me.dateEditControl.Size = New System.Drawing.Size(104, 20)
		Me.dateEditControl.TabIndex = 11
		'
		'lblMessage
		'
		Me.lblMessage.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblMessage.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
		Me.lblMessage.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
		Me.lblMessage.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblMessage.Location = New System.Drawing.Point(5, 12)
		Me.lblMessage.Name = "lblMessage"
		Me.lblMessage.Size = New System.Drawing.Size(194, 33)
		Me.lblMessage.TabIndex = 233
		Me.lblMessage.Text = "Bitte geben Sie das Datum ein"
		'
		'btnOk
		'
		Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnOk.Location = New System.Drawing.Point(149, 50)
		Me.btnOk.Name = "btnOk"
		Me.btnOk.Size = New System.Drawing.Size(75, 24)
		Me.btnOk.TabIndex = 234
		Me.btnOk.Text = "Ok"
		'
		'btnCancel
		'
		Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnCancel.Location = New System.Drawing.Point(230, 50)
		Me.btnCancel.Name = "btnCancel"
		Me.btnCancel.Size = New System.Drawing.Size(79, 24)
		Me.btnCancel.TabIndex = 235
		Me.btnCancel.Text = "Abbrechen"
		'
		'DateInputDialog
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(339, 104)
		Me.ControlBox = False
		Me.Controls.Add(Me.btnCancel)
		Me.Controls.Add(Me.btnOk)
		Me.Controls.Add(Me.lblMessage)
		Me.Controls.Add(Me.dateEditControl)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.Name = "DateInputDialog"
		Me.Text = "Datumseingabe"
		CType(Me.dateEditControl.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.dateEditControl.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub
    Friend WithEvents dateEditControl As DevExpress.XtraEditors.DateEdit
    Friend WithEvents lblMessage As DevExpress.XtraEditors.LabelControl
    Friend WithEvents btnOk As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnCancel As DevExpress.XtraEditors.SimpleButton
End Class
