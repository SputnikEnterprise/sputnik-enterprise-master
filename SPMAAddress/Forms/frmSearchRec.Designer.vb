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
    Me.Label4 = New System.Windows.Forms.Label()
    Me.Label3 = New System.Windows.Forms.Label()
    Me.PictureBox1 = New System.Windows.Forms.PictureBox()
    Me.LblChanged = New System.Windows.Forms.Label()
    Me.cmdOK = New System.Windows.Forms.Button()
    Me.Label1 = New System.Windows.Forms.Label()
    Me.txtSearchValue = New System.Windows.Forms.TextBox()
    Me.LvData = New System.Windows.Forms.ListView()
    Me.Label2 = New System.Windows.Forms.Label()
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
    'Label4
    '
    Me.Label4.AutoSize = True
    Me.Label4.BackColor = System.Drawing.Color.White
    Me.Label4.Location = New System.Drawing.Point(46, 45)
    Me.Label4.Name = "Label4"
    Me.Label4.Size = New System.Drawing.Size(220, 13)
    Me.Label4.TabIndex = 38
    Me.Label4.Text = "Wählen Sie Ihre gewünschten Kriterien aus..."
    '
    'Label3
    '
    Me.Label3.AutoSize = True
    Me.Label3.BackColor = System.Drawing.Color.White
    Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label3.Location = New System.Drawing.Point(24, 23)
    Me.Label3.Name = "Label3"
    Me.Label3.Size = New System.Drawing.Size(143, 13)
    Me.Label3.TabIndex = 37
    Me.Label3.Text = "Suche nach Datensätze"
    '
    'PictureBox1
    '
    Me.PictureBox1.BackColor = System.Drawing.Color.White
    Me.PictureBox1.Dock = System.Windows.Forms.DockStyle.Top
    Me.PictureBox1.Location = New System.Drawing.Point(0, 0)
    Me.PictureBox1.Name = "PictureBox1"
    Me.PictureBox1.Size = New System.Drawing.Size(530, 74)
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
    'Label1
    '
    Me.Label1.AutoSize = True
    Me.Label1.Location = New System.Drawing.Point(12, 105)
    Me.Label1.Name = "Label1"
    Me.Label1.Size = New System.Drawing.Size(52, 13)
    Me.Label1.TabIndex = 32
    Me.Label1.Text = "Datailliste"
    '
    'txtSearchValue
    '
    Me.txtSearchValue.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtSearchValue.Location = New System.Drawing.Point(283, 98)
    Me.txtSearchValue.Name = "txtSearchValue"
    Me.txtSearchValue.Size = New System.Drawing.Size(149, 20)
    Me.txtSearchValue.TabIndex = 29
    '
    'LvData
    '
    Me.LvData.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.LvData.Location = New System.Drawing.Point(12, 124)
    Me.LvData.MultiSelect = False
    Me.LvData.Name = "LvData"
    Me.LvData.Size = New System.Drawing.Size(420, 491)
    Me.LvData.TabIndex = 28
    Me.LvData.UseCompatibleStateImageBehavior = False
    Me.LvData.View = System.Windows.Forms.View.Details
    '
    'Label2
    '
    Me.Label2.Location = New System.Drawing.Point(153, 101)
    Me.Label2.Name = "Label2"
    Me.Label2.Size = New System.Drawing.Size(124, 13)
    Me.Label2.TabIndex = 39
    Me.Label2.Text = "Suchfeld"
    Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'frmSearchRec
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(530, 627)
    Me.Controls.Add(Me.Label2)
    Me.Controls.Add(Me.cmdClose)
    Me.Controls.Add(Me.Label4)
    Me.Controls.Add(Me.Label3)
    Me.Controls.Add(Me.PictureBox1)
    Me.Controls.Add(Me.LblChanged)
    Me.Controls.Add(Me.cmdOK)
    Me.Controls.Add(Me.Label1)
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
  Friend WithEvents Label4 As System.Windows.Forms.Label
  Friend WithEvents Label3 As System.Windows.Forms.Label
  Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
  Friend WithEvents LblChanged As System.Windows.Forms.Label
  Friend WithEvents cmdOK As System.Windows.Forms.Button
  Friend WithEvents Label1 As System.Windows.Forms.Label
  Friend WithEvents txtSearchValue As System.Windows.Forms.TextBox
  Friend WithEvents LvData As System.Windows.Forms.ListView
  Friend WithEvents Label2 As System.Windows.Forms.Label
End Class
