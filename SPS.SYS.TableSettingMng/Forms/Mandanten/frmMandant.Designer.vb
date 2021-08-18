<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMandant
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMandant))
		Me.navMain = New DevExpress.XtraNavBar.NavBarControl()
		Me.pnlHeader = New DevExpress.XtraEditors.PanelControl()
		Me.btnClose = New DevExpress.XtraEditors.SimpleButton()
		Me.lblHeaderDescription = New DevExpress.XtraEditors.LabelControl()
		Me.lblHeader = New DevExpress.XtraEditors.LabelControl()
		Me.Bar4 = New DevExpress.XtraBars.Bar()
		Me.bsiInfo = New DevExpress.XtraBars.BarStaticItem()
		Me.bbiSave = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiPrint = New DevExpress.XtraBars.BarLargeButtonItem()
		Me.bbiExport = New DevExpress.XtraBars.BarLargeButtonItem()
		Me.BarStaticItem2 = New DevExpress.XtraBars.BarStaticItem()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager()
		Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
		Me.BarStaticItem1 = New DevExpress.XtraBars.BarStaticItem()
		Me.sccMain = New DevExpress.XtraEditors.SplitContainerControl()
		Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
		Me.lueYear = New DevExpress.XtraEditors.LookUpEdit()
		Me.lblJahr = New System.Windows.Forms.Label()
		Me.lueMandant = New DevExpress.XtraEditors.LookUpEdit()
		Me.lblMDName = New System.Windows.Forms.Label()
		Me.pnlNavbar = New DevExpress.XtraEditors.PanelControl()
		CType(Me.navMain, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.pnlHeader, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.pnlHeader.SuspendLayout()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.sccMain, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.sccMain.SuspendLayout()
		CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.PanelControl1.SuspendLayout()
		CType(Me.lueYear.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.pnlNavbar, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.pnlNavbar.SuspendLayout()
		Me.SuspendLayout()
		'
		'navMain
		'
		Me.navMain.ActiveGroup = Nothing
		Me.navMain.Dock = System.Windows.Forms.DockStyle.Fill
		Me.navMain.Location = New System.Drawing.Point(12, 12)
		Me.navMain.Name = "navMain"
		Me.navMain.OptionsNavPane.ExpandedWidth = 173
		Me.navMain.Size = New System.Drawing.Size(173, 423)
		Me.navMain.TabIndex = 20
		Me.navMain.Text = "NavBarControl1"
		'
		'pnlHeader
		'
		Me.pnlHeader.Controls.Add(Me.btnClose)
		Me.pnlHeader.Controls.Add(Me.lblHeaderDescription)
		Me.pnlHeader.Controls.Add(Me.lblHeader)
		Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
		Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
		Me.pnlHeader.Name = "pnlHeader"
		Me.pnlHeader.Size = New System.Drawing.Size(923, 71)
		Me.pnlHeader.TabIndex = 307
		'
		'btnClose
		'
		Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnClose.Location = New System.Drawing.Point(804, 12)
		Me.btnClose.Name = "btnClose"
		Me.btnClose.Size = New System.Drawing.Size(87, 23)
		Me.btnClose.TabIndex = 2
		Me.btnClose.Text = "Schliessen"
		'
		'lblHeaderDescription
		'
		Me.lblHeaderDescription.Location = New System.Drawing.Point(29, 33)
		Me.lblHeaderDescription.Name = "lblHeaderDescription"
		Me.lblHeaderDescription.Size = New System.Drawing.Size(212, 13)
		Me.lblHeaderDescription.TabIndex = 1
		Me.lblHeaderDescription.Text = "Wählen Sie Ihre gewünschte Kriterien aus..."
		'
		'lblHeader
		'
		Me.lblHeader.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblHeader.Appearance.Options.UseFont = True
		Me.lblHeader.Location = New System.Drawing.Point(12, 12)
		Me.lblHeader.Name = "lblHeader"
		Me.lblHeader.Size = New System.Drawing.Size(134, 13)
		Me.lblHeader.TabIndex = 0
		Me.lblHeader.Text = "Verwaltung von System"
		'
		'Bar4
		'
		Me.Bar4.BarName = "Statusleiste"
		Me.Bar4.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
		Me.Bar4.DockCol = 0
		Me.Bar4.DockRow = 0
		Me.Bar4.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
		Me.Bar4.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiInfo), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiSave), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiPrint), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiExport, True), New DevExpress.XtraBars.LinkPersistInfo(Me.BarStaticItem2)})
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
		Me.bsiInfo.Size = New System.Drawing.Size(32, 0)
		Me.bsiInfo.Width = 32
		'
		'bbiSave
		'
		Me.bbiSave.Caption = "Daten sichern"
		Me.bbiSave.Id = 1
		Me.bbiSave.ImageOptions.Image = CType(resources.GetObject("bbiSave.ImageOptions.Image"), System.Drawing.Image)
		Me.bbiSave.ImageOptions.LargeImage = CType(resources.GetObject("bbiSave.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.bbiSave.Name = "bbiSave"
		Me.bbiSave.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiPrint
		'
		Me.bbiPrint.AllowAllUp = True
		Me.bbiPrint.Caption = "Drucken"
		Me.bbiPrint.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Right
		Me.bbiPrint.Id = 6
		Me.bbiPrint.ImageOptions.Image = CType(resources.GetObject("bbiPrint.ImageOptions.Image"), System.Drawing.Image)
		Me.bbiPrint.Name = "bbiPrint"
		Me.bbiPrint.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiExport
		'
		Me.bbiExport.Caption = "Exportieren"
		Me.bbiExport.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Right
		Me.bbiExport.Id = 4
		Me.bbiExport.ImageOptions.Image = CType(resources.GetObject("bbiExport.ImageOptions.Image"), System.Drawing.Image)
		Me.bbiExport.Name = "bbiExport"
		Me.bbiExport.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'BarStaticItem2
		'
		Me.BarStaticItem2.Caption = " "
		Me.BarStaticItem2.Id = 10
		Me.BarStaticItem2.Name = "BarStaticItem2"
		'
		'BarManager1
		'
		Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar4})
		Me.BarManager1.DockControls.Add(Me.barDockControlTop)
		Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
		Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
		Me.BarManager1.DockControls.Add(Me.barDockControlRight)
		Me.BarManager1.Form = Me
		Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiInfo, Me.bbiSave, Me.bbiPrint, Me.BarStaticItem1, Me.bbiExport, Me.BarStaticItem2})
		Me.BarManager1.MaxItemId = 11
		Me.BarManager1.StatusBar = Me.Bar4
		'
		'barDockControlTop
		'
		Me.barDockControlTop.CausesValidation = False
		Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
		Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlTop.Manager = Me.BarManager1
		Me.barDockControlTop.Size = New System.Drawing.Size(923, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 518)
		Me.barDockControlBottom.Manager = Me.BarManager1
		Me.barDockControlBottom.Size = New System.Drawing.Size(923, 27)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Manager = Me.BarManager1
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 518)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(923, 0)
		Me.barDockControlRight.Manager = Me.BarManager1
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 518)
		'
		'BarStaticItem1
		'
		Me.BarStaticItem1.Caption = " "
		Me.BarStaticItem1.Id = 5
		Me.BarStaticItem1.Name = "BarStaticItem1"
		'
		'sccMain
		'
		Me.sccMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.sccMain.Horizontal = False
		Me.sccMain.IsSplitterFixed = True
		Me.sccMain.Location = New System.Drawing.Point(198, 71)
		Me.sccMain.Name = "sccMain"
		Me.sccMain.Panel1.Controls.Add(Me.PanelControl1)
		Me.sccMain.Panel1.Padding = New System.Windows.Forms.Padding(5)
		Me.sccMain.Panel1.Text = "Panel1"
		Me.sccMain.Panel2.Text = "Panel2"
		Me.sccMain.Size = New System.Drawing.Size(721, 445)
		Me.sccMain.SplitterPosition = 86
		Me.sccMain.TabIndex = 331
		Me.sccMain.Text = "SplitContainerControl2"
		'
		'PanelControl1
		'
		Me.PanelControl1.Controls.Add(Me.lueYear)
		Me.PanelControl1.Controls.Add(Me.lblJahr)
		Me.PanelControl1.Controls.Add(Me.lueMandant)
		Me.PanelControl1.Controls.Add(Me.lblMDName)
		Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.PanelControl1.Location = New System.Drawing.Point(5, 5)
		Me.PanelControl1.Name = "PanelControl1"
		Me.PanelControl1.Size = New System.Drawing.Size(711, 76)
		Me.PanelControl1.TabIndex = 0
		'
		'lueYear
		'
		Me.lueYear.Location = New System.Drawing.Point(134, 43)
		Me.lueYear.Name = "lueYear"
		Me.lueYear.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.lueYear.Properties.NullText = ""
		Me.lueYear.Size = New System.Drawing.Size(80, 20)
		Me.lueYear.TabIndex = 321
		'
		'lblJahr
		'
		Me.lblJahr.Location = New System.Drawing.Point(10, 47)
		Me.lblJahr.Name = "lblJahr"
		Me.lblJahr.Size = New System.Drawing.Size(118, 13)
		Me.lblJahr.TabIndex = 305
		Me.lblJahr.Text = "Jahr"
		Me.lblJahr.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lueMandant
		'
		Me.lueMandant.Location = New System.Drawing.Point(134, 17)
		Me.lueMandant.Name = "lueMandant"
		Me.lueMandant.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.lueMandant.Properties.NullText = ""
		Me.lueMandant.Size = New System.Drawing.Size(384, 20)
		Me.lueMandant.TabIndex = 302
		'
		'lblMDName
		'
		Me.lblMDName.Location = New System.Drawing.Point(10, 20)
		Me.lblMDName.Name = "lblMDName"
		Me.lblMDName.Size = New System.Drawing.Size(118, 13)
		Me.lblMDName.TabIndex = 303
		Me.lblMDName.Text = "Mandanten"
		Me.lblMDName.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'pnlNavbar
		'
		Me.pnlNavbar.Controls.Add(Me.navMain)
		Me.pnlNavbar.Dock = System.Windows.Forms.DockStyle.Left
		Me.pnlNavbar.Location = New System.Drawing.Point(0, 71)
		Me.pnlNavbar.Name = "pnlNavbar"
		Me.pnlNavbar.Padding = New System.Windows.Forms.Padding(10)
		Me.pnlNavbar.Size = New System.Drawing.Size(197, 447)
		Me.pnlNavbar.TabIndex = 347
		'
		'frmMandant
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(923, 545)
		Me.Controls.Add(Me.pnlNavbar)
		Me.Controls.Add(Me.sccMain)
		Me.Controls.Add(Me.pnlHeader)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.Name = "frmMandant"
		Me.Text = "Mandantenverwaltung"
		CType(Me.navMain, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.pnlHeader, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pnlHeader.ResumeLayout(False)
		Me.pnlHeader.PerformLayout()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.sccMain, System.ComponentModel.ISupportInitialize).EndInit()
		Me.sccMain.ResumeLayout(False)
		CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.PanelControl1.ResumeLayout(False)
		CType(Me.lueYear.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.pnlNavbar, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pnlNavbar.ResumeLayout(False)
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents navMain As DevExpress.XtraNavBar.NavBarControl
	Friend WithEvents pnlHeader As DevExpress.XtraEditors.PanelControl
	Friend WithEvents btnClose As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents lblHeaderDescription As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblHeader As DevExpress.XtraEditors.LabelControl
	Friend WithEvents Bar4 As DevExpress.XtraBars.Bar
	Friend WithEvents bsiInfo As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents bbiSave As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbiPrint As DevExpress.XtraBars.BarLargeButtonItem
	Friend WithEvents bbiExport As DevExpress.XtraBars.BarLargeButtonItem
	Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
	Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
	Friend WithEvents BarStaticItem1 As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents BarStaticItem2 As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents sccMain As DevExpress.XtraEditors.SplitContainerControl
	Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
	Friend WithEvents lueMandant As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents lblMDName As System.Windows.Forms.Label
	Friend WithEvents lblJahr As System.Windows.Forms.Label
	Friend WithEvents lueYear As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents pnlNavbar As DevExpress.XtraEditors.PanelControl
End Class
