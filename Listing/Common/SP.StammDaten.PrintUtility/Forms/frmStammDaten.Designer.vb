<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmStammDaten
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmStammDaten))
		Me.RadialMenu1 = New DevExpress.XtraBars.Ribbon.RadialMenu()
		Me.BarButtonItem6 = New DevExpress.XtraBars.BarButtonItem()
		Me.BarButtonItem7 = New DevExpress.XtraBars.BarButtonItem()
		Me.BarButtonItem8 = New DevExpress.XtraBars.BarButtonItem()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager()
		Me.Bar3 = New DevExpress.XtraBars.Bar()
		Me.BarStaticItem1 = New DevExpress.XtraBars.BarStaticItem()
		Me.bbiPrint = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiDesign = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiExport = New DevExpress.XtraBars.BarButtonItem()
		Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
		Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl()
		Me.txtNumber = New DevExpress.XtraEditors.TextEdit()
		Me.btnWhatPrint = New DevExpress.XtraEditors.SimpleButton()
		Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl()
		Me.RadialMenu2 = New DevComponents.DotNetBar.RadialMenu()
		Me.itm_kandidat = New DevComponents.DotNetBar.RadialMenuItem()
		Me.itm_kunde = New DevComponents.DotNetBar.RadialMenuItem()
		Me.itm_vakanz = New DevComponents.DotNetBar.RadialMenuItem()
		Me.itm_vorschlag = New DevComponents.DotNetBar.RadialMenuItem()
		Me.GroupBox1 = New DevExpress.XtraEditors.PanelControl()
		Me.CmdClose = New DevExpress.XtraEditors.SimpleButton()
		Me.Label3 = New System.Windows.Forms.Label()
		CType(Me.RadialMenu1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtNumber.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupBox1.SuspendLayout()
		Me.SuspendLayout()
		'
		'RadialMenu1
		'
		Me.RadialMenu1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem6), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem7), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem8)})
		Me.RadialMenu1.Manager = Me.BarManager1
		Me.RadialMenu1.Name = "RadialMenu1"
		'
		'BarButtonItem6
		'
		Me.BarButtonItem6.Caption = "Kunde"
		Me.BarButtonItem6.Id = 6
		Me.BarButtonItem6.Name = "BarButtonItem6"
		'
		'BarButtonItem7
		'
		Me.BarButtonItem7.Caption = "Vakanz"
		Me.BarButtonItem7.Id = 7
		Me.BarButtonItem7.Name = "BarButtonItem7"
		'
		'BarButtonItem8
		'
		Me.BarButtonItem8.Caption = "Vorschlag"
		Me.BarButtonItem8.Id = 8
		Me.BarButtonItem8.Name = "BarButtonItem8"
		'
		'BarManager1
		'
		Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar3})
		Me.BarManager1.DockControls.Add(Me.barDockControlTop)
		Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
		Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
		Me.BarManager1.DockControls.Add(Me.barDockControlRight)
		Me.BarManager1.Form = Me
		Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.BarButtonItem6, Me.BarButtonItem7, Me.BarButtonItem8, Me.BarStaticItem1, Me.bbiPrint, Me.bbiDesign, Me.bbiExport})
		Me.BarManager1.MaxItemId = 13
		Me.BarManager1.StatusBar = Me.Bar3
		'
		'Bar3
		'
		Me.Bar3.BarName = "Statusleiste"
		Me.Bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
		Me.Bar3.DockCol = 0
		Me.Bar3.DockRow = 0
		Me.Bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
		Me.Bar3.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarStaticItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiPrint), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiDesign, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiExport, True)})
		Me.Bar3.OptionsBar.AllowQuickCustomization = False
		Me.Bar3.OptionsBar.DrawDragBorder = False
		Me.Bar3.OptionsBar.UseWholeRow = True
		Me.Bar3.Text = "Statusleiste"
		'
		'BarStaticItem1
		'
		Me.BarStaticItem1.AutoSize = DevExpress.XtraBars.BarStaticItemSize.Spring
		Me.BarStaticItem1.Caption = "Bereit"
		Me.BarStaticItem1.Id = 9
		Me.BarStaticItem1.Name = "BarStaticItem1"
		Me.BarStaticItem1.Size = New System.Drawing.Size(150, 0)
		Me.BarStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near
		Me.BarStaticItem1.Width = 150
		'
		'bbiPrint
		'
		Me.bbiPrint.Caption = "Drucken"
		Me.bbiPrint.Glyph = CType(resources.GetObject("bbiPrint.Glyph"), System.Drawing.Image)
		Me.bbiPrint.Id = 10
		Me.bbiPrint.Name = "bbiPrint"
		Me.bbiPrint.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiDesign
		'
		Me.bbiDesign.Caption = "Entwurfansicht"
		Me.bbiDesign.Glyph = CType(resources.GetObject("bbiDesign.Glyph"), System.Drawing.Image)
		Me.bbiDesign.Id = 11
		Me.bbiDesign.Name = "bbiDesign"
		Me.bbiDesign.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiExport
		'
		Me.bbiExport.Caption = "Exportieren"
		Me.bbiExport.Glyph = CType(resources.GetObject("bbiExport.Glyph"), System.Drawing.Image)
		Me.bbiExport.Id = 12
		Me.bbiExport.Name = "bbiExport"
		Me.bbiExport.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'barDockControlTop
		'
		Me.barDockControlTop.CausesValidation = False
		Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
		Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlTop.Size = New System.Drawing.Size(650, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 180)
		Me.barDockControlBottom.Size = New System.Drawing.Size(650, 27)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 180)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(650, 0)
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 180)
		'
		'LabelControl1
		'
		Me.LabelControl1.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.LabelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.LabelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.LabelControl1.Location = New System.Drawing.Point(18, 91)
		Me.LabelControl1.Name = "LabelControl1"
		Me.LabelControl1.Size = New System.Drawing.Size(210, 13)
		Me.LabelControl1.TabIndex = 0
		Me.LabelControl1.Text = "Was möchten Sie drucken?"
		'
		'TextEdit1
		'
		Me.txtNumber.EditValue = "0"
		Me.txtNumber.Location = New System.Drawing.Point(234, 116)
		Me.txtNumber.Name = "TextEdit1"
		Me.txtNumber.Properties.Appearance.Options.UseTextOptions = True
		Me.txtNumber.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.txtNumber.Size = New System.Drawing.Size(84, 20)
		Me.txtNumber.TabIndex = 1
		Me.txtNumber.Visible = False
		'
		'btnWhatPrint
		'
		Me.btnWhatPrint.Image = CType(resources.GetObject("btnWhatPrint.Image"), System.Drawing.Image)
		Me.btnWhatPrint.Location = New System.Drawing.Point(234, 85)
		Me.btnWhatPrint.Name = "btnWhatPrint"
		Me.btnWhatPrint.Size = New System.Drawing.Size(166, 25)
		Me.btnWhatPrint.TabIndex = 6
		Me.btnWhatPrint.Text = "Stammblatt drucken für..."
		'
		'LabelControl2
		'
		Me.LabelControl2.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.LabelControl2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.LabelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.LabelControl2.Location = New System.Drawing.Point(100, 120)
		Me.LabelControl2.Name = "LabelControl2"
		Me.LabelControl2.Size = New System.Drawing.Size(128, 13)
		Me.LabelControl2.TabIndex = 7
		Me.LabelControl2.Text = "Nummer"
		Me.LabelControl2.Visible = False
		'
		'RadialMenu2
		'
		Me.RadialMenu2.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.itm_kandidat, Me.itm_kunde, Me.itm_vakanz, Me.itm_vorschlag})
		Me.RadialMenu2.Location = New System.Drawing.Point(406, 82)
		Me.RadialMenu2.Name = "RadialMenu2"
		Me.RadialMenu2.Size = New System.Drawing.Size(28, 28)
		Me.RadialMenu2.Symbol = ""
		Me.RadialMenu2.TabIndex = 12
		Me.RadialMenu2.Text = "RadialMenu2"
		'
		'itm_kandidat
		'
		Me.itm_kandidat.Name = "itm_kandidat"
		Me.itm_kandidat.Text = "Kandidat"
		'
		'itm_kunde
		'
		Me.itm_kunde.Name = "itm_kunde"
		Me.itm_kunde.Text = "Kunde"
		'
		'itm_vakanz
		'
		Me.itm_vakanz.Name = "itm_vakanz"
		Me.itm_vakanz.Text = "Vakanzen"
		'
		'itm_vorschlag
		'
		Me.itm_vorschlag.Name = "itm_vorschlag"
		Me.itm_vorschlag.Text = "Vorschlag"
		'
		'GroupBox1
		'
		Me.GroupBox1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003
		Me.GroupBox1.Controls.Add(Me.CmdClose)
		Me.GroupBox1.Controls.Add(Me.Label3)
		Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
		Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(650, 59)
		Me.GroupBox1.TabIndex = 210
		'
		'CmdClose
		'
		Me.CmdClose.Anchor = System.Windows.Forms.AnchorStyles.Right
		Me.CmdClose.Location = New System.Drawing.Point(554, 18)
		Me.CmdClose.Name = "CmdClose"
		Me.CmdClose.Size = New System.Drawing.Size(80, 25)
		Me.CmdClose.TabIndex = 204
		Me.CmdClose.Text = "Schliessen"
		'
		'Label3
		'
		Me.Label3.BackColor = System.Drawing.Color.Transparent
		Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label3.Image = CType(resources.GetObject("Label3.Image"), System.Drawing.Image)
		Me.Label3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.Label3.Location = New System.Drawing.Point(12, 12)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(225, 36)
		Me.Label3.TabIndex = 37
		Me.Label3.Text = "Suche nach Datensätze"
		Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'frmStammDaten
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(650, 207)
		Me.Controls.Add(Me.GroupBox1)
		Me.Controls.Add(Me.RadialMenu2)
		Me.Controls.Add(Me.LabelControl2)
		Me.Controls.Add(Me.btnWhatPrint)
		Me.Controls.Add(Me.txtNumber)
		Me.Controls.Add(Me.LabelControl1)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.Name = "frmStammDaten"
		Me.Text = "Druck von Stammblätter"
		CType(Me.RadialMenu1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtNumber.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupBox1.ResumeLayout(False)
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents RadialMenu1 As DevExpress.XtraBars.Ribbon.RadialMenu
	Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
	Friend WithEvents txtNumber As DevExpress.XtraEditors.TextEdit
  Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
  Friend WithEvents Bar3 As DevExpress.XtraBars.Bar
  Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
  Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
  Friend WithEvents btnWhatPrint As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents RadialMenu2 As DevComponents.DotNetBar.RadialMenu
  Friend WithEvents itm_kandidat As DevComponents.DotNetBar.RadialMenuItem
  Friend WithEvents itm_kunde As DevComponents.DotNetBar.RadialMenuItem
  Friend WithEvents itm_vakanz As DevComponents.DotNetBar.RadialMenuItem
  Friend WithEvents itm_vorschlag As DevComponents.DotNetBar.RadialMenuItem
  Friend WithEvents BarButtonItem6 As DevExpress.XtraBars.BarButtonItem
  Friend WithEvents BarButtonItem7 As DevExpress.XtraBars.BarButtonItem
  Friend WithEvents BarButtonItem8 As DevExpress.XtraBars.BarButtonItem
  Friend WithEvents BarStaticItem1 As DevExpress.XtraBars.BarStaticItem
  Friend WithEvents bbiPrint As DevExpress.XtraBars.BarButtonItem
  Friend WithEvents bbiDesign As DevExpress.XtraBars.BarButtonItem
  Friend WithEvents bbiExport As DevExpress.XtraBars.BarButtonItem
  Friend WithEvents GroupBox1 As DevExpress.XtraEditors.PanelControl
  Friend WithEvents CmdClose As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents Label3 As System.Windows.Forms.Label
End Class
