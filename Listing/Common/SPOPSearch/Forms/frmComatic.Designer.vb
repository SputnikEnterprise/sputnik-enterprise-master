<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmComatic
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmComatic))
    Me.lblDatei = New DevExpress.XtraEditors.LabelControl()
    Me.chkArtikel = New DevExpress.XtraEditors.CheckEdit()
    Me.txt_Filename = New DevExpress.XtraEditors.ComboBoxEdit()
    Me.BarManager1 = New DevExpress.XtraBars.BarManager()
    Me.Bar4 = New DevExpress.XtraBars.Bar()
    Me.bsiInfo = New DevExpress.XtraBars.BarStaticItem()
    Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
    Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
    Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
    Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
    Me.BarStaticItem1 = New DevExpress.XtraBars.BarStaticItem()
    Me.cmdOK = New DevExpress.XtraEditors.SimpleButton()
    Me.GroupBox1 = New DevExpress.XtraEditors.PanelControl()
    Me.lblHeader2 = New DevExpress.XtraEditors.LabelControl()
    Me.LblSetting = New System.Windows.Forms.Label()
    Me.lblHeader1 = New DevExpress.XtraEditors.LabelControl()
    Me.CmdClose = New DevExpress.XtraEditors.SimpleButton()
    CType(Me.chkArtikel.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txt_Filename.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.GroupBox1.SuspendLayout()
    Me.SuspendLayout()
    '
    'lblDatei
    '
    Me.lblDatei.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    Me.lblDatei.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
    Me.lblDatei.Location = New System.Drawing.Point(15, 105)
    Me.lblDatei.Name = "lblDatei"
    Me.lblDatei.Size = New System.Drawing.Size(81, 13)
    Me.lblDatei.TabIndex = 125
    Me.lblDatei.Text = "Dateiname"
    '
    'chkArtikel
    '
    Me.chkArtikel.Location = New System.Drawing.Point(100, 129)
    Me.chkArtikel.Name = "chkArtikel"
    Me.chkArtikel.Properties.Caption = "Inkl. Artikel bzw. Bezeichnung"
    Me.chkArtikel.Size = New System.Drawing.Size(247, 19)
    Me.chkArtikel.TabIndex = 126
    '
    'txt_Filename
    '
    Me.txt_Filename.Location = New System.Drawing.Point(102, 102)
    Me.txt_Filename.Name = "txt_Filename"
    Me.txt_Filename.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.txt_Filename.Size = New System.Drawing.Size(366, 20)
    Me.txt_Filename.TabIndex = 122
    '
    'BarManager1
    '
    Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar4})
    Me.BarManager1.DockControls.Add(Me.barDockControlTop)
    Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
    Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
    Me.BarManager1.DockControls.Add(Me.barDockControlRight)
    Me.BarManager1.Form = Me
    Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiInfo, Me.BarStaticItem1})
    Me.BarManager1.MaxItemId = 10
    Me.BarManager1.StatusBar = Me.Bar4
    '
    'Bar4
    '
    Me.Bar4.BarName = "Statusleiste"
    Me.Bar4.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
    Me.Bar4.DockCol = 0
    Me.Bar4.DockRow = 0
    Me.Bar4.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
    Me.Bar4.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiInfo)})
    Me.Bar4.OptionsBar.AllowQuickCustomization = False
    Me.Bar4.OptionsBar.DrawDragBorder = False
    Me.Bar4.OptionsBar.UseWholeRow = True
    Me.Bar4.Text = "Statusleiste"
    '
    'bsiInfo
    '
    Me.bsiInfo.AutoSize = DevExpress.XtraBars.BarStaticItemSize.Spring
    Me.bsiInfo.Caption = "Bereit"
    Me.bsiInfo.Id = 0
    Me.bsiInfo.Name = "bsiInfo"
    Me.bsiInfo.TextAlignment = System.Drawing.StringAlignment.Near
    Me.bsiInfo.Width = 32
    '
    'barDockControlTop
    '
    Me.barDockControlTop.CausesValidation = False
    Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
    Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
    Me.barDockControlTop.Size = New System.Drawing.Size(636, 0)
    '
    'barDockControlBottom
    '
    Me.barDockControlBottom.CausesValidation = False
    Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
    Me.barDockControlBottom.Location = New System.Drawing.Point(0, 217)
    Me.barDockControlBottom.Size = New System.Drawing.Size(636, 25)
    '
    'barDockControlLeft
    '
    Me.barDockControlLeft.CausesValidation = False
    Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
    Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
    Me.barDockControlLeft.Size = New System.Drawing.Size(0, 217)
    '
    'barDockControlRight
    '
    Me.barDockControlRight.CausesValidation = False
    Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
    Me.barDockControlRight.Location = New System.Drawing.Point(636, 0)
    Me.barDockControlRight.Size = New System.Drawing.Size(0, 217)
    '
    'BarStaticItem1
    '
    Me.BarStaticItem1.Caption = " "
    Me.BarStaticItem1.Id = 5
    Me.BarStaticItem1.Name = "BarStaticItem1"
    Me.BarStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near
    '
    'cmdOK
    '
    Me.cmdOK.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.cmdOK.Location = New System.Drawing.Point(499, 102)
    Me.cmdOK.Name = "cmdOK"
    Me.cmdOK.Size = New System.Drawing.Size(94, 25)
    Me.cmdOK.TabIndex = 222
    Me.cmdOK.Text = "Ausführen"
    '
    'GroupBox1
    '
    Me.GroupBox1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003
    Me.GroupBox1.Controls.Add(Me.lblHeader2)
    Me.GroupBox1.Controls.Add(Me.LblSetting)
    Me.GroupBox1.Controls.Add(Me.lblHeader1)
    Me.GroupBox1.Controls.Add(Me.CmdClose)
    Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
    Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
    Me.GroupBox1.Name = "GroupBox1"
    Me.GroupBox1.Size = New System.Drawing.Size(636, 59)
    Me.GroupBox1.TabIndex = 227
    '
    'lblHeader2
    '
    Me.lblHeader2.AllowHtmlString = True
    Me.lblHeader2.Location = New System.Drawing.Point(78, 31)
    Me.lblHeader2.Name = "lblHeader2"
    Me.lblHeader2.Size = New System.Drawing.Size(218, 13)
    Me.lblHeader2.TabIndex = 214
    Me.lblHeader2.Text = "Wählen Sie Ihre gewünschten Kriterien aus..."
    '
    'LblSetting
    '
    Me.LblSetting.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.LblSetting.ForeColor = System.Drawing.SystemColors.HotTrack
    Me.LblSetting.Image = CType(resources.GetObject("LblSetting.Image"), System.Drawing.Image)
    Me.LblSetting.Location = New System.Drawing.Point(12, 12)
    Me.LblSetting.Name = "LblSetting"
    Me.LblSetting.Size = New System.Drawing.Size(51, 38)
    Me.LblSetting.TabIndex = 213
    '
    'lblHeader1
    '
    Me.lblHeader1.AllowHtmlString = True
    Me.lblHeader1.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
    Me.lblHeader1.Location = New System.Drawing.Point(69, 12)
    Me.lblHeader1.Name = "lblHeader1"
    Me.lblHeader1.Size = New System.Drawing.Size(164, 13)
    Me.lblHeader1.TabIndex = 213
    Me.lblHeader1.Text = "Export der Daten für Comatic"
    '
    'CmdClose
    '
    Me.CmdClose.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.CmdClose.Location = New System.Drawing.Point(499, 12)
    Me.CmdClose.Name = "CmdClose"
    Me.CmdClose.Size = New System.Drawing.Size(94, 25)
    Me.CmdClose.TabIndex = 204
    Me.CmdClose.Text = "Schliessen"
    '
    'frmComatic
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(636, 242)
    Me.Controls.Add(Me.GroupBox1)
    Me.Controls.Add(Me.cmdOK)
    Me.Controls.Add(Me.chkArtikel)
    Me.Controls.Add(Me.lblDatei)
    Me.Controls.Add(Me.txt_Filename)
    Me.Controls.Add(Me.barDockControlLeft)
    Me.Controls.Add(Me.barDockControlRight)
    Me.Controls.Add(Me.barDockControlBottom)
    Me.Controls.Add(Me.barDockControlTop)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.MaximumSize = New System.Drawing.Size(652, 280)
    Me.Name = "frmComatic"
    Me.Text = "Export für Comatic"
    CType(Me.chkArtikel.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txt_Filename.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.GroupBox1.ResumeLayout(False)
    Me.GroupBox1.PerformLayout()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents lblDatei As DevExpress.XtraEditors.LabelControl
  Friend WithEvents chkArtikel As DevExpress.XtraEditors.CheckEdit
  Friend WithEvents txt_Filename As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
  Friend WithEvents Bar4 As DevExpress.XtraBars.Bar
  Friend WithEvents bsiInfo As DevExpress.XtraBars.BarStaticItem
  Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
  Friend WithEvents BarStaticItem1 As DevExpress.XtraBars.BarStaticItem
  Friend WithEvents cmdOK As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents GroupBox1 As DevExpress.XtraEditors.PanelControl
  Friend WithEvents lblHeader2 As DevExpress.XtraEditors.LabelControl
  Friend WithEvents LblSetting As System.Windows.Forms.Label
  Friend WithEvents lblHeader1 As DevExpress.XtraEditors.LabelControl
  Friend WithEvents CmdClose As DevExpress.XtraEditors.SimpleButton
End Class
