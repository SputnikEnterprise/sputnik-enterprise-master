<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCloseMonth
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCloseMonth))
		Dim EditorButtonImageOptions1 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
		Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject4 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim EditorButtonImageOptions2 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
		Dim SerializableAppearanceObject5 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject6 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject7 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject8 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Me.gridBank = New DevExpress.XtraGrid.GridControl()
		Me.gvBank = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.btnClose = New DevExpress.XtraEditors.SimpleButton()
		Me.lblMonat = New DevExpress.XtraEditors.LabelControl()
		Me.lblJahr = New DevExpress.XtraEditors.LabelControl()
		Me.pcHeader = New DevExpress.XtraEditors.PanelControl()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.lblHeaderNormal = New System.Windows.Forms.Label()
		Me.lblHeaderFett = New System.Windows.Forms.Label()
		Me.SplitContainerControl1 = New DevExpress.XtraEditors.SplitContainerControl()
		Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
		Me.lueMonth = New DevExpress.XtraEditors.LookUpEdit()
		Me.lueYear = New DevExpress.XtraEditors.LookUpEdit()
		Me.btnCloseMonth = New DevExpress.XtraEditors.SimpleButton()
		Me.lueMandant = New DevExpress.XtraEditors.LookUpEdit()
		Me.lblMDName = New System.Windows.Forms.Label()
		Me.grpExistsrecs = New DevExpress.XtraEditors.GroupControl()
		Me.chkThisYear = New DevExpress.XtraEditors.CheckEdit()
		Me.errorProviderMonthCloseMng = New DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider()
		Me.Bar4 = New DevExpress.XtraBars.Bar()
		Me.bsiInfo = New DevExpress.XtraBars.BarStaticItem()
		Me.bbiOpenMonth = New DevExpress.XtraBars.BarButtonItem()
		Me.RepositoryItemMarqueeProgressBar3 = New DevExpress.XtraEditors.Repository.RepositoryItemMarqueeProgressBar()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager()
		Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
		Me.BarStaticItem1 = New DevExpress.XtraBars.BarStaticItem()
		Me.BarEditItem1 = New DevExpress.XtraBars.BarEditItem()
		Me.RepositoryItemMarqueeProgressBar4 = New DevExpress.XtraEditors.Repository.RepositoryItemMarqueeProgressBar()
		CType(Me.gridBank, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvBank, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.pcHeader, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.pcHeader.SuspendLayout()
		CType(Me.SplitContainerControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SplitContainerControl1.SuspendLayout()
		CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.PanelControl1.SuspendLayout()
		CType(Me.lueMonth.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lueYear.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.grpExistsrecs, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grpExistsrecs.SuspendLayout()
		CType(Me.chkThisYear.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.errorProviderMonthCloseMng, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.RepositoryItemMarqueeProgressBar3, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.RepositoryItemMarqueeProgressBar4, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'gridBank
		'
		Me.gridBank.Cursor = System.Windows.Forms.Cursors.Default
		Me.gridBank.Dock = System.Windows.Forms.DockStyle.Fill
		Me.gridBank.Location = New System.Drawing.Point(2, 20)
		Me.gridBank.MainView = Me.gvBank
		Me.gridBank.Name = "gridBank"
		Me.gridBank.Size = New System.Drawing.Size(704, 222)
		Me.gridBank.TabIndex = 214
		Me.gridBank.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvBank})
		'
		'gvBank
		'
		Me.gvBank.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvBank.GridControl = Me.gridBank
		Me.gvBank.Name = "gvBank"
		Me.gvBank.OptionsBehavior.Editable = False
		Me.gvBank.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvBank.OptionsView.ShowGroupPanel = False
		'
		'btnClose
		'
		Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnClose.Location = New System.Drawing.Point(587, 22)
		Me.btnClose.Name = "btnClose"
		Me.btnClose.Size = New System.Drawing.Size(105, 28)
		Me.btnClose.TabIndex = 221
		Me.btnClose.Text = "Schliessen"
		'
		'lblMonat
		'
		Me.lblMonat.Appearance.Options.UseTextOptions = True
		Me.lblMonat.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblMonat.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblMonat.Location = New System.Drawing.Point(46, 83)
		Me.lblMonat.Name = "lblMonat"
		Me.lblMonat.Size = New System.Drawing.Size(108, 13)
		Me.lblMonat.TabIndex = 224
		Me.lblMonat.Text = "Monat"
		'
		'lblJahr
		'
		Me.lblJahr.Appearance.Options.UseTextOptions = True
		Me.lblJahr.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblJahr.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblJahr.Location = New System.Drawing.Point(46, 57)
		Me.lblJahr.Name = "lblJahr"
		Me.lblJahr.Size = New System.Drawing.Size(108, 13)
		Me.lblJahr.TabIndex = 223
		Me.lblJahr.Text = "Jahr"
		'
		'pcHeader
		'
		Me.pcHeader.Controls.Add(Me.Label2)
		Me.pcHeader.Controls.Add(Me.lblHeaderNormal)
		Me.pcHeader.Controls.Add(Me.lblHeaderFett)
		Me.pcHeader.Controls.Add(Me.btnClose)
		Me.pcHeader.Dock = System.Windows.Forms.DockStyle.Top
		Me.pcHeader.Location = New System.Drawing.Point(0, 0)
		Me.pcHeader.Name = "pcHeader"
		Me.pcHeader.Size = New System.Drawing.Size(718, 74)
		Me.pcHeader.TabIndex = 217
		'
		'Label2
		'
		Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label2.ForeColor = System.Drawing.SystemColors.HotTrack
		Me.Label2.Image = CType(resources.GetObject("Label2.Image"), System.Drawing.Image)
		Me.Label2.Location = New System.Drawing.Point(12, 9)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(54, 49)
		Me.Label2.TabIndex = 1003
		'
		'lblHeaderNormal
		'
		Me.lblHeaderNormal.AutoSize = True
		Me.lblHeaderNormal.BackColor = System.Drawing.Color.Transparent
		Me.lblHeaderNormal.Location = New System.Drawing.Point(87, 37)
		Me.lblHeaderNormal.Name = "lblHeaderNormal"
		Me.lblHeaderNormal.Size = New System.Drawing.Size(234, 13)
		Me.lblHeaderNormal.TabIndex = 1002
		Me.lblHeaderNormal.Text = "Geben Sie bitte Ihre gewünschten Kriterien ein."
		'
		'lblHeaderFett
		'
		Me.lblHeaderFett.AutoSize = True
		Me.lblHeaderFett.BackColor = System.Drawing.Color.Transparent
		Me.lblHeaderFett.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblHeaderFett.Location = New System.Drawing.Point(72, 21)
		Me.lblHeaderFett.Name = "lblHeaderFett"
		Me.lblHeaderFett.Size = New System.Drawing.Size(169, 13)
		Me.lblHeaderFett.TabIndex = 1001
		Me.lblHeaderFett.Text = "Monat abschliessen / öffnen"
		'
		'SplitContainerControl1
		'
		Me.SplitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.SplitContainerControl1.Horizontal = False
		Me.SplitContainerControl1.IsSplitterFixed = True
		Me.SplitContainerControl1.Location = New System.Drawing.Point(0, 74)
		Me.SplitContainerControl1.Name = "SplitContainerControl1"
		Me.SplitContainerControl1.Panel1.Controls.Add(Me.PanelControl1)
		Me.SplitContainerControl1.Panel1.Padding = New System.Windows.Forms.Padding(5)
		Me.SplitContainerControl1.Panel1.Text = "Panel1"
		Me.SplitContainerControl1.Panel2.Controls.Add(Me.grpExistsrecs)
		Me.SplitContainerControl1.Panel2.Padding = New System.Windows.Forms.Padding(5)
		Me.SplitContainerControl1.Panel2.Text = "Panel2"
		Me.SplitContainerControl1.Size = New System.Drawing.Size(718, 388)
		Me.SplitContainerControl1.SplitterPosition = 129
		Me.SplitContainerControl1.TabIndex = 225
		Me.SplitContainerControl1.Text = "SplitContainerControl1"
		'
		'PanelControl1
		'
		Me.PanelControl1.Controls.Add(Me.lueMonth)
		Me.PanelControl1.Controls.Add(Me.lueYear)
		Me.PanelControl1.Controls.Add(Me.btnCloseMonth)
		Me.PanelControl1.Controls.Add(Me.lueMandant)
		Me.PanelControl1.Controls.Add(Me.lblMDName)
		Me.PanelControl1.Controls.Add(Me.lblJahr)
		Me.PanelControl1.Controls.Add(Me.lblMonat)
		Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.PanelControl1.Location = New System.Drawing.Point(5, 5)
		Me.PanelControl1.Name = "PanelControl1"
		Me.PanelControl1.Size = New System.Drawing.Size(708, 119)
		Me.PanelControl1.TabIndex = 0
		'
		'lueMonth
		'
		Me.lueMonth.Location = New System.Drawing.Point(160, 80)
		Me.lueMonth.Name = "lueMonth"
		SerializableAppearanceObject1.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject1.Options.UseForeColor = True
		SerializableAppearanceObject2.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject2.Options.UseForeColor = True
		SerializableAppearanceObject3.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject3.Options.UseForeColor = True
		SerializableAppearanceObject4.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject4.Options.UseForeColor = True
		Me.lueMonth.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing)})
		Me.lueMonth.Properties.ShowFooter = False
		Me.lueMonth.Size = New System.Drawing.Size(104, 20)
		Me.lueMonth.TabIndex = 333
		'
		'lueYear
		'
		Me.lueYear.Location = New System.Drawing.Point(160, 54)
		Me.lueYear.Name = "lueYear"
		SerializableAppearanceObject5.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject5.Options.UseForeColor = True
		SerializableAppearanceObject6.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject6.Options.UseForeColor = True
		SerializableAppearanceObject7.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject7.Options.UseForeColor = True
		SerializableAppearanceObject8.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject8.Options.UseForeColor = True
		Me.lueYear.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, EditorButtonImageOptions2, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject5, SerializableAppearanceObject6, SerializableAppearanceObject7, SerializableAppearanceObject8, "", Nothing, Nothing)})
		Me.lueYear.Properties.ShowFooter = False
		Me.lueYear.Size = New System.Drawing.Size(104, 20)
		Me.lueYear.TabIndex = 332
		'
		'btnCloseMonth
		'
		Me.btnCloseMonth.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnCloseMonth.Location = New System.Drawing.Point(582, 20)
		Me.btnCloseMonth.Name = "btnCloseMonth"
		Me.btnCloseMonth.Size = New System.Drawing.Size(105, 28)
		Me.btnCloseMonth.TabIndex = 331
		Me.btnCloseMonth.Text = "Abschliessen"
		'
		'lueMandant
		'
		Me.lueMandant.Location = New System.Drawing.Point(160, 20)
		Me.lueMandant.Name = "lueMandant"
		Me.lueMandant.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.lueMandant.Properties.NullText = ""
		Me.lueMandant.Size = New System.Drawing.Size(384, 20)
		Me.lueMandant.TabIndex = 329
		'
		'lblMDName
		'
		Me.lblMDName.BackColor = System.Drawing.Color.Transparent
		Me.lblMDName.Location = New System.Drawing.Point(15, 24)
		Me.lblMDName.Name = "lblMDName"
		Me.lblMDName.Size = New System.Drawing.Size(139, 13)
		Me.lblMDName.TabIndex = 330
		Me.lblMDName.Text = "Mandanten"
		Me.lblMDName.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'grpExistsrecs
		'
		Me.grpExistsrecs.AppearanceCaption.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.grpExistsrecs.AppearanceCaption.Options.UseFont = True
		Me.grpExistsrecs.Controls.Add(Me.chkThisYear)
		Me.grpExistsrecs.Controls.Add(Me.gridBank)
		Me.grpExistsrecs.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grpExistsrecs.Location = New System.Drawing.Point(5, 5)
		Me.grpExistsrecs.Name = "grpExistsrecs"
		Me.grpExistsrecs.Size = New System.Drawing.Size(708, 244)
		Me.grpExistsrecs.TabIndex = 0
		Me.grpExistsrecs.Text = "Bereits abgeschlossene Monate"
		'
		'chkThisYear
		'
		Me.chkThisYear.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.chkThisYear.Location = New System.Drawing.Point(462, 0)
		Me.chkThisYear.Name = "chkThisYear"
		Me.chkThisYear.Properties.AllowFocused = False
		Me.chkThisYear.Properties.Appearance.Options.UseTextOptions = True
		Me.chkThisYear.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.chkThisYear.Properties.Caption = "Nur aktuelles Jahr"
		Me.chkThisYear.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.chkThisYear.Size = New System.Drawing.Size(244, 19)
		Me.chkThisYear.TabIndex = 298
		'
		'errorProviderMonthCloseMng
		'
		Me.errorProviderMonthCloseMng.ContainerControl = Me
		'
		'Bar4
		'
		Me.Bar4.BarName = "Statusleiste"
		Me.Bar4.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
		Me.Bar4.DockCol = 0
		Me.Bar4.DockRow = 0
		Me.Bar4.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
		Me.Bar4.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiInfo), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiOpenMonth)})
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
		'bbiOpenMonth
		'
		Me.bbiOpenMonth.Caption = "Monat öffnen"
		Me.bbiOpenMonth.Id = 1
		Me.bbiOpenMonth.ImageOptions.Image = CType(resources.GetObject("bbiOpenMonth.ImageOptions.Image"), System.Drawing.Image)
		Me.bbiOpenMonth.ImageOptions.LargeImage = CType(resources.GetObject("bbiOpenMonth.ImageOptions.LargeImage"), System.Drawing.Image)
		Me.bbiOpenMonth.Name = "bbiOpenMonth"
		Me.bbiOpenMonth.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'RepositoryItemMarqueeProgressBar3
		'
		Me.RepositoryItemMarqueeProgressBar3.Name = "RepositoryItemMarqueeProgressBar3"
		'
		'BarManager1
		'
		Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar4})
		Me.BarManager1.DockControls.Add(Me.barDockControlTop)
		Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
		Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
		Me.BarManager1.DockControls.Add(Me.barDockControlRight)
		Me.BarManager1.Form = Me
		Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiInfo, Me.bbiOpenMonth, Me.BarStaticItem1, Me.BarEditItem1})
		Me.BarManager1.MaxItemId = 12
		Me.BarManager1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemMarqueeProgressBar4, Me.RepositoryItemMarqueeProgressBar3})
		Me.BarManager1.StatusBar = Me.Bar4
		'
		'barDockControlTop
		'
		Me.barDockControlTop.CausesValidation = False
		Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
		Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlTop.Manager = Me.BarManager1
		Me.barDockControlTop.Size = New System.Drawing.Size(718, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 462)
		Me.barDockControlBottom.Manager = Me.BarManager1
		Me.barDockControlBottom.Size = New System.Drawing.Size(718, 27)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Manager = Me.BarManager1
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 462)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(718, 0)
		Me.barDockControlRight.Manager = Me.BarManager1
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 462)
		'
		'BarStaticItem1
		'
		Me.BarStaticItem1.Caption = " "
		Me.BarStaticItem1.Id = 5
		Me.BarStaticItem1.Name = "BarStaticItem1"
		'
		'BarEditItem1
		'
		Me.BarEditItem1.Caption = "BarEditItem1"
		Me.BarEditItem1.Edit = Me.RepositoryItemMarqueeProgressBar4
		Me.BarEditItem1.Id = 10
		Me.BarEditItem1.Name = "BarEditItem1"
		'
		'RepositoryItemMarqueeProgressBar4
		'
		Me.RepositoryItemMarqueeProgressBar4.Name = "RepositoryItemMarqueeProgressBar4"
		'
		'frmCloseMonth
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(718, 489)
		Me.Controls.Add(Me.SplitContainerControl1)
		Me.Controls.Add(Me.pcHeader)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MinimumSize = New System.Drawing.Size(734, 527)
		Me.Name = "frmCloseMonth"
		Me.Text = "Monat abschliessen / öffnen"
		CType(Me.gridBank, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvBank, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.pcHeader, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pcHeader.ResumeLayout(False)
		Me.pcHeader.PerformLayout()
		CType(Me.SplitContainerControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.SplitContainerControl1.ResumeLayout(False)
		CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.PanelControl1.ResumeLayout(False)
		CType(Me.lueMonth.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lueYear.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.grpExistsrecs, System.ComponentModel.ISupportInitialize).EndInit()
		Me.grpExistsrecs.ResumeLayout(False)
		CType(Me.chkThisYear.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.errorProviderMonthCloseMng, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.RepositoryItemMarqueeProgressBar3, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.RepositoryItemMarqueeProgressBar4, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents gridBank As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvBank As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents btnClose As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents lblMonat As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblJahr As DevExpress.XtraEditors.LabelControl
	Friend WithEvents pcHeader As DevExpress.XtraEditors.PanelControl
	Friend WithEvents SplitContainerControl1 As DevExpress.XtraEditors.SplitContainerControl
	Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
	Friend WithEvents grpExistsrecs As DevExpress.XtraEditors.GroupControl
	Friend WithEvents lblHeaderNormal As System.Windows.Forms.Label
	Friend WithEvents lblHeaderFett As System.Windows.Forms.Label
	Friend WithEvents btnCloseMonth As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents lueMandant As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents lblMDName As System.Windows.Forms.Label
	Friend WithEvents errorProviderMonthCloseMng As DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider
	Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
	Friend WithEvents Bar4 As DevExpress.XtraBars.Bar
	Friend WithEvents RepositoryItemMarqueeProgressBar3 As DevExpress.XtraEditors.Repository.RepositoryItemMarqueeProgressBar
	Friend WithEvents bsiInfo As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents bbiOpenMonth As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
	Friend WithEvents BarStaticItem1 As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents BarEditItem1 As DevExpress.XtraBars.BarEditItem
	Friend WithEvents RepositoryItemMarqueeProgressBar4 As DevExpress.XtraEditors.Repository.RepositoryItemMarqueeProgressBar
	Friend WithEvents lueMonth As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents lueYear As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents chkThisYear As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents Label2 As System.Windows.Forms.Label
End Class
