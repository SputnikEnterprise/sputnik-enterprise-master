<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SplashScreen2
    Inherits DevExpress.XtraSplashScreen.SplashScreen

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
    Me.labelControl2 = New DevExpress.XtraEditors.LabelControl()
    Me.labelControl1 = New DevExpress.XtraEditors.LabelControl()
    Me.marqueeProgressBarControl1 = New DevExpress.XtraEditors.MarqueeProgressBarControl()
    Me.pictureEdit2 = New DevExpress.XtraEditors.PictureEdit()
    CType(Me.marqueeProgressBarControl1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.pictureEdit2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'labelControl2
    '
    Me.labelControl2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.labelControl2.Appearance.BackColor = System.Drawing.Color.White
    Me.labelControl2.Location = New System.Drawing.Point(23, 438)
    Me.labelControl2.Name = "labelControl2"
    Me.labelControl2.Size = New System.Drawing.Size(317, 13)
    Me.labelControl2.TabIndex = 12
    Me.labelControl2.Text = "Die Module werden gestartet. Bitte warten Sie einen Augenblick..."
    '
    'labelControl1
    '
    Me.labelControl1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.labelControl1.Appearance.BackColor = System.Drawing.Color.White
    Me.labelControl1.Location = New System.Drawing.Point(23, 475)
    Me.labelControl1.Name = "labelControl1"
    Me.labelControl1.Size = New System.Drawing.Size(255, 13)
    Me.labelControl1.TabIndex = 11
    Me.labelControl1.Text = "Copyright © 2004-2013."
    '
    'marqueeProgressBarControl1
    '
    Me.marqueeProgressBarControl1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.marqueeProgressBarControl1.EditValue = 0
    Me.marqueeProgressBarControl1.Location = New System.Drawing.Point(23, 457)
    Me.marqueeProgressBarControl1.Name = "marqueeProgressBarControl1"
    Me.marqueeProgressBarControl1.Properties.EndColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
    Me.marqueeProgressBarControl1.Properties.ProgressAnimationMode = DevExpress.Utils.Drawing.ProgressAnimationMode.PingPong
    Me.marqueeProgressBarControl1.Properties.StartColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
    Me.marqueeProgressBarControl1.Size = New System.Drawing.Size(475, 12)
    Me.marqueeProgressBarControl1.TabIndex = 10
    '
    'pictureEdit2
    '
    Me.pictureEdit2.Cursor = System.Windows.Forms.Cursors.Default
    Me.pictureEdit2.Dock = DockStyle.Fill
    Me.pictureEdit2.EditValue = Global.SPS.MainView.My.Resources.Resources.Sputnik_SES_Logo_Symbol_OrangeReflection_5_4
    Me.pictureEdit2.Location = New System.Drawing.Point(0, 0)
    Me.pictureEdit2.Name = "pictureEdit2"
    Me.pictureEdit2.Properties.AllowFocused = False
    Me.pictureEdit2.Properties.Appearance.BackColor = System.Drawing.Color.White
    Me.pictureEdit2.Properties.Appearance.Options.UseBackColor = True
    Me.pictureEdit2.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
    Me.pictureEdit2.Properties.PictureAlignment = System.Drawing.ContentAlignment.TopCenter
    Me.pictureEdit2.Properties.ShowMenu = False
    Me.pictureEdit2.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze
    Me.pictureEdit2.Size = New System.Drawing.Size(524, 500)
    Me.pictureEdit2.TabIndex = 14
    '
    'SplashScreen2
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.BackColor = System.Drawing.Color.White
    Me.ClientSize = New System.Drawing.Size(524, 500)
    Me.Controls.Add(Me.labelControl2)
    Me.Controls.Add(Me.labelControl1)
    Me.Controls.Add(Me.marqueeProgressBarControl1)
    Me.Controls.Add(Me.pictureEdit2)
    Me.Name = "SplashScreen2"
    Me.Text = "Form1"
    CType(Me.marqueeProgressBarControl1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.pictureEdit2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
    Private WithEvents pictureEdit2 As DevExpress.XtraEditors.PictureEdit
  Private WithEvents labelControl2 As DevExpress.XtraEditors.LabelControl
    Private WithEvents labelControl1 As DevExpress.XtraEditors.LabelControl
    Private WithEvents marqueeProgressBarControl1 As DevExpress.XtraEditors.MarqueeProgressBarControl
End Class
