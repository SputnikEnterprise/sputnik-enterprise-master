<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSearchRec
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSearchRec))
		Me.cmdClose = New System.Windows.Forms.Button()
		Me.lblHeaderNormal = New System.Windows.Forms.Label()
		Me.lblHeaderFett = New System.Windows.Forms.Label()
		Me.PictureBox1 = New System.Windows.Forms.PictureBox()
		Me.LblChanged = New System.Windows.Forms.Label()
		Me.cmdOK = New System.Windows.Forms.Button()
		Me.lblDetails = New System.Windows.Forms.Label()
		Me.cboDbField = New System.Windows.Forms.ComboBox()
		Me.txtSearchValue = New System.Windows.Forms.TextBox()
		Me.LvData = New System.Windows.Forms.ListView()
		Me.lblSearchField = New System.Windows.Forms.Label()
		CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'cmdClose
		'
		Me.cmdClose.FlatStyle = System.Windows.Forms.FlatStyle.System
		Me.cmdClose.Location = New System.Drawing.Point(438, 23)
		Me.cmdClose.Name = "cmdClose"
		Me.cmdClose.Size = New System.Drawing.Size(75, 23)
		Me.cmdClose.TabIndex = 35
		Me.cmdClose.Text = "Schliessen"
		Me.cmdClose.UseVisualStyleBackColor = True
		'
		'lblHeaderNormal
		'
		Me.lblHeaderNormal.AutoSize = True
		Me.lblHeaderNormal.BackColor = System.Drawing.Color.White
		Me.lblHeaderNormal.Location = New System.Drawing.Point(46, 45)
		Me.lblHeaderNormal.Name = "lblHeaderNormal"
		Me.lblHeaderNormal.Size = New System.Drawing.Size(225, 13)
		Me.lblHeaderNormal.TabIndex = 38
		Me.lblHeaderNormal.Text = "Wählen Sie Ihre gewünschten Kriterien aus..."
		'
		'lblHeaderFett
		'
		Me.lblHeaderFett.AutoSize = True
		Me.lblHeaderFett.BackColor = System.Drawing.Color.White
		Me.lblHeaderFett.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblHeaderFett.Location = New System.Drawing.Point(24, 23)
		Me.lblHeaderFett.Name = "lblHeaderFett"
		Me.lblHeaderFett.Size = New System.Drawing.Size(143, 13)
		Me.lblHeaderFett.TabIndex = 37
		Me.lblHeaderFett.Text = "Suche nach Datensätze"
		'
		'PictureBox1
		'
		Me.PictureBox1.BackColor = System.Drawing.Color.White
		Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.PictureBox1.Location = New System.Drawing.Point(-4, -1)
		Me.PictureBox1.Name = "PictureBox1"
		Me.PictureBox1.Size = New System.Drawing.Size(667, 74)
		Me.PictureBox1.TabIndex = 36
		Me.PictureBox1.TabStop = False
		'
		'LblChanged
		'
		Me.LblChanged.AutoSize = True
		Me.LblChanged.Location = New System.Drawing.Point(461, 158)
		Me.LblChanged.Name = "LblChanged"
		Me.LblChanged.Size = New System.Drawing.Size(13, 13)
		Me.LblChanged.TabIndex = 34
		Me.LblChanged.Text = "0"
		Me.LblChanged.Visible = False
		'
		'cmdOK
		'
		Me.cmdOK.FlatStyle = System.Windows.Forms.FlatStyle.System
		Me.cmdOK.Location = New System.Drawing.Point(438, 98)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.Size = New System.Drawing.Size(75, 23)
		Me.cmdOK.TabIndex = 33
		Me.cmdOK.Text = "Auswählen"
		Me.cmdOK.UseVisualStyleBackColor = True
		'
		'lblDetails
		'
		Me.lblDetails.AutoSize = True
		Me.lblDetails.Location = New System.Drawing.Point(12, 105)
		Me.lblDetails.Name = "lblDetails"
		Me.lblDetails.Size = New System.Drawing.Size(53, 13)
		Me.lblDetails.TabIndex = 32
		Me.lblDetails.Text = "Datailliste"
		'
		'cboDbField
		'
		Me.cboDbField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboDbField.FormattingEnabled = True
		Me.cboDbField.Location = New System.Drawing.Point(178, 97)
		Me.cboDbField.Name = "cboDbField"
		Me.cboDbField.Size = New System.Drawing.Size(99, 21)
		Me.cboDbField.TabIndex = 30
		'
		'txtSearchValue
		'
		Me.txtSearchValue.Location = New System.Drawing.Point(283, 98)
		Me.txtSearchValue.Name = "txtSearchValue"
		Me.txtSearchValue.Size = New System.Drawing.Size(149, 21)
		Me.txtSearchValue.TabIndex = 29
		'
		'LvData
		'
		Me.LvData.Location = New System.Drawing.Point(12, 124)
		Me.LvData.Name = "LvData"
		Me.LvData.Size = New System.Drawing.Size(420, 474)
		Me.LvData.TabIndex = 28
		Me.LvData.UseCompatibleStateImageBehavior = False
		Me.LvData.View = System.Windows.Forms.View.Details
		'
		'lblSearchField
		'
		Me.lblSearchField.AutoSize = True
		Me.lblSearchField.Location = New System.Drawing.Point(178, 79)
		Me.lblSearchField.Name = "lblSearchField"
		Me.lblSearchField.Size = New System.Drawing.Size(48, 13)
		Me.lblSearchField.TabIndex = 39
		Me.lblSearchField.Text = "Suchfeld"
		'
		'frmSearchRec
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(530, 627)
		Me.Controls.Add(Me.lblSearchField)
		Me.Controls.Add(Me.cmdClose)
		Me.Controls.Add(Me.lblHeaderNormal)
		Me.Controls.Add(Me.lblHeaderFett)
		Me.Controls.Add(Me.PictureBox1)
		Me.Controls.Add(Me.LblChanged)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.lblDetails)
		Me.Controls.Add(Me.cboDbField)
		Me.Controls.Add(Me.txtSearchValue)
		Me.Controls.Add(Me.LvData)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.Name = "frmSearchRec"
		Me.Text = "Suche nach Datensätze"
		CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents cmdClose As System.Windows.Forms.Button
  Friend WithEvents lblHeaderNormal As System.Windows.Forms.Label
  Friend WithEvents lblHeaderFett As System.Windows.Forms.Label
  Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
  Friend WithEvents LblChanged As System.Windows.Forms.Label
  Friend WithEvents cmdOK As System.Windows.Forms.Button
  Friend WithEvents lblDetails As System.Windows.Forms.Label
  Friend WithEvents cboDbField As System.Windows.Forms.ComboBox
  Friend WithEvents txtSearchValue As System.Windows.Forms.TextBox
  Friend WithEvents LvData As System.Windows.Forms.ListView
  Friend WithEvents lblSearchField As System.Windows.Forms.Label
End Class
