<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMDSelection
	Inherits DevExpress.XtraEditors.XtraForm

	'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
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

	'Wird vom Windows Form-Designer benötigt.
	Private components As System.ComponentModel.IContainer

	'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
	'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
	'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
	<System.Diagnostics.DebuggerStepThrough()> _
	Private Sub InitializeComponent()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMDSelection))
		Me.ucMDList = New SPS.MainView.MDList()
		Me.SuspendLayout()
		'
		'ucMDList
		'
		Me.ucMDList.Dock = DockStyle.Fill
		Me.ucMDList.Location = New System.Drawing.Point(0, 0)
		Me.ucMDList.Name = "ucMDList"
		Me.ucMDList.Padding = New System.Windows.Forms.Padding(20)
		Me.ucMDList.Size = New System.Drawing.Size(795, 401)
		Me.ucMDList.TabIndex = 0
		'
		'frmMDSelection
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(795, 401)
		Me.Controls.Add(Me.ucMDList)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.MinimumSize = New System.Drawing.Size(811, 440)
		Me.Name = "frmMDSelection"
		Me.Text = "Anmeldung"
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents ucMDList As SPS.MainView.MDList
	'  System.Windows.Forms.TextBox
	' System.Windows.Forms.TextBox
	' System.Windows.Forms.Button
	' System.Windows.Forms.Button
	' System.Windows.Forms.TextBox
	' System.Windows.Forms.TextBox
End Class
