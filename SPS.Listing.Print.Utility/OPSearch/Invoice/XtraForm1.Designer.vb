<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class XtraForm1
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
		Me.PdfViewer1 = New DevExpress.XtraPdfViewer.PdfViewer()
		Me.SuspendLayout()
		'
		'PdfViewer1
		'
		Me.PdfViewer1.Location = New System.Drawing.Point(39, 43)
		Me.PdfViewer1.Name = "PdfViewer1"
		Me.PdfViewer1.Size = New System.Drawing.Size(235, 191)
		Me.PdfViewer1.TabIndex = 0
		'
		'XtraForm1
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(284, 262)
		Me.Controls.Add(Me.PdfViewer1)
		Me.Name = "XtraForm1"
		Me.Text = "XtraForm1"
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents PdfViewer1 As DevExpress.XtraPdfViewer.PdfViewer
End Class
