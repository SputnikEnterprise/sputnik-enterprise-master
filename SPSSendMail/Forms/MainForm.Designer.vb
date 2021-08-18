<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
    Inherits System.Windows.Forms.Form

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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
    Me.cmdClose = New DevExpress.XtraEditors.SimpleButton()
    Me.Label3 = New System.Windows.Forms.Label()
    Me.PictureBox1 = New System.Windows.Forms.PictureBox()
    Me.Label7 = New System.Windows.Forms.Label()
    Me.txtTo = New DevExpress.XtraEditors.TextEdit()
    Me.Label6 = New System.Windows.Forms.Label()
    Me.txtFrom = New DevExpress.XtraEditors.TextEdit()
    Me.Label5 = New System.Windows.Forms.Label()
    Me.txtSubject = New DevExpress.XtraEditors.TextEdit()
    Me.Label1 = New System.Windows.Forms.Label()
    Me.Label2 = New System.Windows.Forms.Label()
    Me.lstAttachments = New DevExpress.XtraEditors.ListBoxControl()
    Me.Label8 = New System.Windows.Forms.Label()
    Me.LblChanged = New System.Windows.Forms.Label()
    Me.rtxtText = New System.Windows.Forms.RichTextBox()
    Me.wbHtml = New System.Windows.Forms.WebBrowser()
    Me.Label4 = New System.Windows.Forms.Label()
    Me.LblChanged_0 = New System.Windows.Forms.Label()
    Me.lblKDMAData = New DevExpress.XtraEditors.LabelControl()
    CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txtTo.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txtFrom.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txtSubject.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.lstAttachments, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'cmdClose
    '
    Me.cmdClose.Location = New System.Drawing.Point(908, 19)
    Me.cmdClose.Name = "cmdClose"
    Me.cmdClose.Size = New System.Drawing.Size(75, 23)
    Me.cmdClose.TabIndex = 17
    Me.cmdClose.Text = "Schliessen"
    '
    'Label3
    '
    Me.Label3.AutoSize = True
    Me.Label3.BackColor = System.Drawing.Color.White
    Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label3.Location = New System.Drawing.Point(26, 19)
    Me.Label3.Name = "Label3"
    Me.Label3.Size = New System.Drawing.Size(160, 13)
    Me.Label3.TabIndex = 15
    Me.Label3.Text = "Details über Ihre Nachricht"
    '
    'PictureBox1
    '
    Me.PictureBox1.BackColor = System.Drawing.Color.White
    Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
    Me.PictureBox1.Location = New System.Drawing.Point(-2, -5)
    Me.PictureBox1.Name = "PictureBox1"
    Me.PictureBox1.Size = New System.Drawing.Size(1092, 74)
    Me.PictureBox1.TabIndex = 14
    Me.PictureBox1.TabStop = False
    '
    'Label7
    '
    Me.Label7.AutoSize = True
    Me.Label7.Location = New System.Drawing.Point(-44, 131)
    Me.Label7.Name = "Label7"
    Me.Label7.Size = New System.Drawing.Size(43, 13)
    Me.Label7.TabIndex = 22
    Me.Label7.Text = "Vorlage"
    '
    'txtTo
    '
    Me.txtTo.Location = New System.Drawing.Point(104, 114)
    Me.txtTo.Name = "txtTo"
    Me.txtTo.Properties.Appearance.BackColor = System.Drawing.SystemColors.Control
    Me.txtTo.Properties.Appearance.Options.UseBackColor = True
    Me.txtTo.Size = New System.Drawing.Size(396, 20)
    Me.txtTo.TabIndex = 21
    '
    'Label6
    '
    Me.Label6.Location = New System.Drawing.Point(12, 117)
    Me.Label6.Name = "Label6"
    Me.Label6.Size = New System.Drawing.Size(82, 13)
    Me.Label6.TabIndex = 20
    Me.Label6.Text = "An"
    Me.Label6.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'txtFrom
    '
    Me.txtFrom.Location = New System.Drawing.Point(104, 88)
    Me.txtFrom.Name = "txtFrom"
    Me.txtFrom.Properties.Appearance.BackColor = System.Drawing.SystemColors.Control
    Me.txtFrom.Properties.Appearance.Options.UseBackColor = True
    Me.txtFrom.Size = New System.Drawing.Size(396, 20)
    Me.txtFrom.TabIndex = 19
    '
    'Label5
    '
    Me.Label5.Location = New System.Drawing.Point(12, 91)
    Me.Label5.Name = "Label5"
    Me.Label5.Size = New System.Drawing.Size(82, 13)
    Me.Label5.TabIndex = 18
    Me.Label5.Text = "Von"
    Me.Label5.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'txtSubject
    '
    Me.txtSubject.Location = New System.Drawing.Point(104, 140)
    Me.txtSubject.Name = "txtSubject"
    Me.txtSubject.Properties.Appearance.BackColor = System.Drawing.SystemColors.Control
    Me.txtSubject.Properties.Appearance.Options.UseBackColor = True
    Me.txtSubject.Size = New System.Drawing.Size(396, 20)
    Me.txtSubject.TabIndex = 25
    '
    'Label1
    '
    Me.Label1.Location = New System.Drawing.Point(12, 143)
    Me.Label1.Name = "Label1"
    Me.Label1.Size = New System.Drawing.Size(82, 13)
    Me.Label1.TabIndex = 24
    Me.Label1.Text = "Betreff"
    Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'Label2
    '
    Me.Label2.Location = New System.Drawing.Point(12, 201)
    Me.Label2.Name = "Label2"
    Me.Label2.Size = New System.Drawing.Size(82, 13)
    Me.Label2.TabIndex = 26
    Me.Label2.Text = "Nachricht"
    Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'lstAttachments
    '
    Me.lstAttachments.Appearance.BackColor = System.Drawing.SystemColors.Control
    Me.lstAttachments.Appearance.Options.UseBackColor = True
    Me.lstAttachments.Location = New System.Drawing.Point(104, 166)
    Me.lstAttachments.Name = "lstAttachments"
    Me.lstAttachments.Size = New System.Drawing.Size(396, 26)
    Me.lstAttachments.TabIndex = 27
    '
    'Label8
    '
    Me.Label8.Location = New System.Drawing.Point(12, 166)
    Me.Label8.Name = "Label8"
    Me.Label8.Size = New System.Drawing.Size(82, 13)
    Me.Label8.TabIndex = 28
    Me.Label8.Text = "Dateianhang"
    Me.Label8.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'LblChanged
    '
    Me.LblChanged.AutoSize = True
    Me.LblChanged.Location = New System.Drawing.Point(970, 88)
    Me.LblChanged.Name = "LblChanged"
    Me.LblChanged.Size = New System.Drawing.Size(13, 13)
    Me.LblChanged.TabIndex = 29
    Me.LblChanged.Text = "0"
    Me.LblChanged.Visible = False
    '
    'rtxtText
    '
    Me.rtxtText.AcceptsTab = True
    Me.rtxtText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
    Me.rtxtText.Location = New System.Drawing.Point(104, 201)
    Me.rtxtText.Name = "rtxtText"
    Me.rtxtText.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth
    Me.rtxtText.Size = New System.Drawing.Size(879, 506)
    Me.rtxtText.TabIndex = 30
    Me.rtxtText.Text = ""
    Me.rtxtText.WordWrap = False
    '
    'wbHtml
    '
    Me.wbHtml.Location = New System.Drawing.Point(104, 203)
    Me.wbHtml.MinimumSize = New System.Drawing.Size(20, 20)
    Me.wbHtml.Name = "wbHtml"
    Me.wbHtml.ScriptErrorsSuppressed = True
    Me.wbHtml.Size = New System.Drawing.Size(879, 506)
    Me.wbHtml.TabIndex = 31
    Me.wbHtml.TabStop = False
    '
    'Label4
    '
    Me.Label4.AutoSize = True
    Me.Label4.BackColor = System.Drawing.Color.White
    Me.Label4.Location = New System.Drawing.Point(53, 44)
    Me.Label4.Name = "Label4"
    Me.Label4.Size = New System.Drawing.Size(53, 13)
    Me.Label4.TabIndex = 32
    Me.Label4.Text = "Gesendet"
    '
    'LblChanged_0
    '
    Me.LblChanged_0.AutoSize = True
    Me.LblChanged_0.BackColor = System.Drawing.Color.White
    Me.LblChanged_0.Location = New System.Drawing.Point(112, 44)
    Me.LblChanged_0.Name = "LblChanged_0"
    Me.LblChanged_0.Size = New System.Drawing.Size(53, 13)
    Me.LblChanged_0.TabIndex = 33
    Me.LblChanged_0.Text = "Gesendet"
    '
    'lblKDMAData
    '
    Me.lblKDMAData.AllowHtmlString = True
    Me.lblKDMAData.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.lblKDMAData.LineVisible = True
    Me.lblKDMAData.Location = New System.Drawing.Point(506, 117)
    Me.lblKDMAData.Name = "lblKDMAData"
    Me.lblKDMAData.Size = New System.Drawing.Size(18, 13)
    Me.lblKDMAData.TabIndex = 34
    Me.lblKDMAData.Text = "Von"
    '
    'MainForm
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(1012, 721)
    Me.Controls.Add(Me.lblKDMAData)
    Me.Controls.Add(Me.LblChanged_0)
    Me.Controls.Add(Me.Label4)
    Me.Controls.Add(Me.wbHtml)
    Me.Controls.Add(Me.rtxtText)
    Me.Controls.Add(Me.LblChanged)
    Me.Controls.Add(Me.Label8)
    Me.Controls.Add(Me.lstAttachments)
    Me.Controls.Add(Me.Label2)
    Me.Controls.Add(Me.txtSubject)
    Me.Controls.Add(Me.Label1)
    Me.Controls.Add(Me.Label7)
    Me.Controls.Add(Me.txtTo)
    Me.Controls.Add(Me.Label6)
    Me.Controls.Add(Me.txtFrom)
    Me.Controls.Add(Me.Label5)
    Me.Controls.Add(Me.cmdClose)
    Me.Controls.Add(Me.Label3)
    Me.Controls.Add(Me.PictureBox1)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Name = "MainForm"
    Me.Text = "Ihre Nachricht"
    CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txtTo.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txtFrom.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txtSubject.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.lstAttachments, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents cmdClose As DevExpress.XtraEditors.SimpleButton ' System.Windows.Forms.Button
  Friend WithEvents Label3 As System.Windows.Forms.Label
  Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
  Friend WithEvents Label7 As System.Windows.Forms.Label
  Friend WithEvents txtTo As DevExpress.XtraEditors.TextEdit ' System.Windows.Forms.TextBox
  Friend WithEvents Label6 As System.Windows.Forms.Label
  Friend WithEvents txtFrom As DevExpress.XtraEditors.TextEdit ' System.Windows.Forms.TextBox
  Friend WithEvents Label5 As System.Windows.Forms.Label
  Friend WithEvents txtSubject As DevExpress.XtraEditors.TextEdit ' System.Windows.Forms.TextBox
  Friend WithEvents Label1 As System.Windows.Forms.Label
  Friend WithEvents Label2 As System.Windows.Forms.Label
  Friend WithEvents lstAttachments As DevExpress.XtraEditors.ListBoxControl ' System.Windows.Forms.ListBox
  Friend WithEvents Label8 As System.Windows.Forms.Label
  Friend WithEvents LblChanged As System.Windows.Forms.Label
  Friend WithEvents rtxtText As System.Windows.Forms.RichTextBox
  Friend WithEvents wbHtml As System.Windows.Forms.WebBrowser
  Friend WithEvents Label4 As System.Windows.Forms.Label
  Friend WithEvents LblChanged_0 As System.Windows.Forms.Label
  Friend WithEvents lblKDMAData As DevExpress.XtraEditors.LabelControl ' System.Windows.Forms.Label
End Class
