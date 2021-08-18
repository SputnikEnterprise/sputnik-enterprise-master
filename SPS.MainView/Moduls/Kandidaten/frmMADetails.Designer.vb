<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMADetails
	Inherits DevExpress.XtraEditors.XtraForm

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
		Me.components = New System.ComponentModel.Container()
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMADetails))
		Me.grdDetailrec = New DevExpress.XtraGrid.GridControl()
		Me.gvDetail = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.sccMain = New DevExpress.XtraEditors.SplitContainerControl()
		Me.rlblDetailHeader = New DevExpress.XtraEditors.LabelControl()
		Me.FluentDesignFormContainer1 = New DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormContainer()
		Me.acMain = New DevExpress.XtraBars.Navigation.AccordionControl()
		Me.AccordionContentContainer2 = New DevExpress.XtraBars.Navigation.AccordionContentContainer()
		Me.pnlContactFilter = New DevExpress.XtraEditors.PanelControl()
		Me.lblBezeichnung = New DevExpress.XtraEditors.LabelControl()
		Me.SeparatorControl2 = New DevExpress.XtraEditors.SeparatorControl()
		Me.lblMonat = New DevExpress.XtraEditors.LabelControl()
		Me.lueMonth = New DevExpress.XtraEditors.LookUpEdit()
		Me.lueYear = New DevExpress.XtraEditors.LookUpEdit()
		Me.lblJahr = New DevExpress.XtraEditors.LabelControl()
		Me.txtPlainText = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.tgsAssignedEmployee = New DevExpress.XtraEditors.ToggleSwitch()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
		Me.Bar3 = New DevExpress.XtraBars.Bar()
		Me.bsiRecCount = New DevExpress.XtraBars.BarStaticItem()
		Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
		Me.AccordionControlElement1 = New DevExpress.XtraBars.Navigation.AccordionControlElement()
		Me.aceAssignedEmployee = New DevExpress.XtraBars.Navigation.AccordionControlElement()
		Me.AccordionControlSeparator1 = New DevExpress.XtraBars.Navigation.AccordionControlSeparator()
		Me.AccordionControlElement2 = New DevExpress.XtraBars.Navigation.AccordionControlElement()
		Me.FluentFormDefaultManager1 = New DevExpress.XtraBars.FluentDesignSystem.FluentFormDefaultManager(Me.components)
		Me.Bar1 = New DevExpress.XtraBars.Bar()
		CType(Me.grdDetailrec, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvDetail, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.sccMain, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.sccMain.SuspendLayout()
		Me.FluentDesignFormContainer1.SuspendLayout()
		CType(Me.acMain, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.acMain.SuspendLayout()
		Me.AccordionContentContainer2.SuspendLayout()
		CType(Me.pnlContactFilter, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.pnlContactFilter.SuspendLayout()
		CType(Me.SeparatorControl2, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lueMonth.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lueYear.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtPlainText.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.tgsAssignedEmployee.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.FluentFormDefaultManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'grdDetailrec
		'
		Me.grdDetailrec.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdDetailrec.Location = New System.Drawing.Point(328, 5)
		Me.grdDetailrec.MainView = Me.gvDetail
		Me.grdDetailrec.Name = "grdDetailrec"
		Me.grdDetailrec.Size = New System.Drawing.Size(717, 570)
		Me.grdDetailrec.TabIndex = 3
		Me.grdDetailrec.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvDetail})
		'
		'gvDetail
		'
		Me.gvDetail.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvDetail.GridControl = Me.grdDetailrec
		Me.gvDetail.Name = "gvDetail"
		Me.gvDetail.OptionsBehavior.Editable = False
		Me.gvDetail.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvDetail.OptionsView.ShowAutoFilterRow = True
		Me.gvDetail.OptionsView.ShowGroupPanel = False
		'
		'sccMain
		'
		Me.sccMain.Dock = System.Windows.Forms.DockStyle.Fill
		Me.sccMain.Horizontal = False
		Me.sccMain.IsSplitterFixed = True
		Me.sccMain.Location = New System.Drawing.Point(0, 0)
		Me.sccMain.Name = "sccMain"
		Me.sccMain.Panel1.Controls.Add(Me.rlblDetailHeader)
		Me.sccMain.Panel1.Text = "Panel1"
		Me.sccMain.Panel2.Controls.Add(Me.FluentDesignFormContainer1)
		Me.sccMain.Panel2.Padding = New System.Windows.Forms.Padding(5)
		Me.sccMain.Panel2.Text = "Panel2"
		Me.sccMain.Size = New System.Drawing.Size(1060, 638)
		Me.sccMain.SplitterPosition = 38
		Me.sccMain.TabIndex = 4
		Me.sccMain.Text = "SplitContainerControl1"
		'
		'rlblDetailHeader
		'
		Me.rlblDetailHeader.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.[True]
		Me.rlblDetailHeader.AllowHtmlString = True
		Me.rlblDetailHeader.Appearance.Font = New System.Drawing.Font("Tahoma", 15.0!, System.Drawing.FontStyle.Bold)
		Me.rlblDetailHeader.Appearance.Options.UseFont = True
		Me.rlblDetailHeader.Appearance.Options.UseTextOptions = True
		Me.rlblDetailHeader.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter
		Me.rlblDetailHeader.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
		Me.rlblDetailHeader.AutoEllipsis = True
		Me.rlblDetailHeader.Location = New System.Drawing.Point(12, 12)
		Me.rlblDetailHeader.Name = "rlblDetailHeader"
		Me.rlblDetailHeader.Size = New System.Drawing.Size(132, 24)
		Me.rlblDetailHeader.TabIndex = 2
		Me.rlblDetailHeader.Text = "DetailHeader"
		'
		'FluentDesignFormContainer1
		'
		Me.FluentDesignFormContainer1.Controls.Add(Me.grdDetailrec)
		Me.FluentDesignFormContainer1.Controls.Add(Me.acMain)
		Me.FluentDesignFormContainer1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.FluentDesignFormContainer1.Location = New System.Drawing.Point(5, 5)
		Me.FluentDesignFormContainer1.Margin = New System.Windows.Forms.Padding(2)
		Me.FluentDesignFormContainer1.Name = "FluentDesignFormContainer1"
		Me.FluentDesignFormContainer1.Padding = New System.Windows.Forms.Padding(5)
		Me.FluentDesignFormContainer1.Size = New System.Drawing.Size(1050, 580)
		Me.FluentDesignFormContainer1.TabIndex = 6
		'
		'acMain
		'
		Me.acMain.ContextButtonsOptions.AllowHtmlText = True
		Me.acMain.Controls.Add(Me.AccordionContentContainer2)
		Me.acMain.Controls.Add(Me.tgsAssignedEmployee)
		Me.acMain.Dock = System.Windows.Forms.DockStyle.Left
		Me.acMain.ElementPositionOnExpanding = DevExpress.XtraBars.Navigation.ElementPositionOnExpanding.ScrollUp
		Me.acMain.Elements.AddRange(New DevExpress.XtraBars.Navigation.AccordionControlElement() {Me.AccordionControlElement1})
		Me.acMain.ExpandElementMode = DevExpress.XtraBars.Navigation.ExpandElementMode.Multiple
		Me.acMain.Location = New System.Drawing.Point(5, 5)
		Me.acMain.Margin = New System.Windows.Forms.Padding(2)
		Me.acMain.Name = "acMain"
		Me.acMain.Size = New System.Drawing.Size(323, 570)
		Me.acMain.TabIndex = 4
		Me.acMain.ViewType = DevExpress.XtraBars.Navigation.AccordionControlViewType.HamburgerMenu
		'
		'AccordionContentContainer2
		'
		Me.AccordionContentContainer2.Controls.Add(Me.pnlContactFilter)
		Me.AccordionContentContainer2.Name = "AccordionContentContainer2"
		Me.AccordionContentContainer2.Size = New System.Drawing.Size(304, 206)
		Me.AccordionContentContainer2.TabIndex = 13
		'
		'pnlContactFileter
		'
		Me.pnlContactFilter.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.pnlContactFilter.Appearance.Options.UseBackColor = True
		Me.pnlContactFilter.Controls.Add(Me.lblBezeichnung)
		Me.pnlContactFilter.Controls.Add(Me.SeparatorControl2)
		Me.pnlContactFilter.Controls.Add(Me.lblMonat)
		Me.pnlContactFilter.Controls.Add(Me.lueMonth)
		Me.pnlContactFilter.Controls.Add(Me.lueYear)
		Me.pnlContactFilter.Controls.Add(Me.lblJahr)
		Me.pnlContactFilter.Controls.Add(Me.txtPlainText)
		Me.pnlContactFilter.Dock = System.Windows.Forms.DockStyle.Fill
		Me.pnlContactFilter.Location = New System.Drawing.Point(0, 0)
		Me.pnlContactFilter.Name = "pnlContactFileter"
		Me.pnlContactFilter.Size = New System.Drawing.Size(304, 206)
		Me.pnlContactFilter.TabIndex = 0
		'
		'lblBezeichnung
		'
		Me.lblBezeichnung.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblBezeichnung.Location = New System.Drawing.Point(14, 131)
		Me.lblBezeichnung.Name = "lblBezeichnung"
		Me.lblBezeichnung.Size = New System.Drawing.Size(67, 13)
		Me.lblBezeichnung.TabIndex = 324
		Me.lblBezeichnung.Text = "Text"
		'
		'SeparatorControl2
		'
		Me.SeparatorControl2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.SeparatorControl2.Location = New System.Drawing.Point(5, 100)
		Me.SeparatorControl2.Name = "SeparatorControl2"
		Me.SeparatorControl2.Size = New System.Drawing.Size(294, 18)
		Me.SeparatorControl2.TabIndex = 321
		'
		'lblMonat
		'
		Me.lblMonat.Location = New System.Drawing.Point(14, 61)
		Me.lblMonat.Name = "lblMonat"
		Me.lblMonat.Size = New System.Drawing.Size(30, 13)
		Me.lblMonat.TabIndex = 319
		Me.lblMonat.Text = "Monat"
		'
		'lueMonth
		'
		Me.lueMonth.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.lueMonth.Location = New System.Drawing.Point(171, 57)
		Me.lueMonth.Name = "lueMonth"
		SerializableAppearanceObject1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		SerializableAppearanceObject1.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject1.Options.UseFont = True
		SerializableAppearanceObject1.Options.UseForeColor = True
		Me.lueMonth.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
		Me.lueMonth.Properties.ShowFooter = False
		Me.lueMonth.Size = New System.Drawing.Size(104, 20)
		Me.lueMonth.TabIndex = 317
		'
		'lueYear
		'
		Me.lueYear.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.lueYear.Location = New System.Drawing.Point(171, 31)
		Me.lueYear.Name = "lueYear"
		SerializableAppearanceObject5.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		SerializableAppearanceObject5.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject5.Options.UseFont = True
		SerializableAppearanceObject5.Options.UseForeColor = True
		Me.lueYear.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, EditorButtonImageOptions2, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject5, SerializableAppearanceObject6, SerializableAppearanceObject7, SerializableAppearanceObject8, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
		Me.lueYear.Properties.ShowFooter = False
		Me.lueYear.Size = New System.Drawing.Size(104, 20)
		Me.lueYear.TabIndex = 316
		'
		'lblJahr
		'
		Me.lblJahr.Location = New System.Drawing.Point(14, 35)
		Me.lblJahr.Name = "lblJahr"
		Me.lblJahr.Size = New System.Drawing.Size(21, 13)
		Me.lblJahr.TabIndex = 318
		Me.lblJahr.Text = "Jahr"
		'
		'txtPlainText
		'
		Me.txtPlainText.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txtPlainText.Location = New System.Drawing.Point(104, 128)
		Me.txtPlainText.Name = "txtPlainText"
		Me.txtPlainText.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Search)})
		Me.txtPlainText.Size = New System.Drawing.Size(171, 20)
		Me.txtPlainText.TabIndex = 326
		'
		'tgsAssignedEmployee
		'
		Me.tgsAssignedEmployee.Location = New System.Drawing.Point(198, 91)
		Me.tgsAssignedEmployee.MenuManager = Me.BarManager1
		Me.tgsAssignedEmployee.Name = "tgsAssignedEmployee"
		Me.tgsAssignedEmployee.Properties.AllowFocused = False
		Me.tgsAssignedEmployee.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.[True]
		Me.tgsAssignedEmployee.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.tgsAssignedEmployee.Properties.OffText = ""
		Me.tgsAssignedEmployee.Properties.OnText = ""
		Me.tgsAssignedEmployee.Properties.ShowText = False
		Me.tgsAssignedEmployee.Size = New System.Drawing.Size(95, 18)
		Me.tgsAssignedEmployee.TabIndex = 19
		'
		'BarManager1
		'
		Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar3})
		Me.BarManager1.DockControls.Add(Me.barDockControlTop)
		Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
		Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
		Me.BarManager1.DockControls.Add(Me.barDockControlRight)
		Me.BarManager1.Form = Me
		Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiRecCount})
		Me.BarManager1.MaxItemId = 1
		Me.BarManager1.StatusBar = Me.Bar3
		'
		'Bar3
		'
		Me.Bar3.BarName = "Statusleiste"
		Me.Bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
		Me.Bar3.DockCol = 0
		Me.Bar3.DockRow = 0
		Me.Bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
		Me.Bar3.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiRecCount)})
		Me.Bar3.OptionsBar.AllowQuickCustomization = False
		Me.Bar3.OptionsBar.DrawDragBorder = False
		Me.Bar3.OptionsBar.UseWholeRow = True
		Me.Bar3.Text = "Statusleiste"
		'
		'bsiRecCount
		'
		Me.bsiRecCount.Caption = "Bereit"
		Me.bsiRecCount.Id = 0
		Me.bsiRecCount.Name = "bsiRecCount"
		'
		'barDockControlTop
		'
		Me.barDockControlTop.CausesValidation = False
		Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
		Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlTop.Manager = Me.BarManager1
		Me.barDockControlTop.Size = New System.Drawing.Size(1060, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 638)
		Me.barDockControlBottom.Manager = Me.BarManager1
		Me.barDockControlBottom.Size = New System.Drawing.Size(1060, 22)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Manager = Me.BarManager1
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 638)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(1060, 0)
		Me.barDockControlRight.Manager = Me.BarManager1
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 638)
		'
		'AccordionControlElement1
		'
		Me.AccordionControlElement1.Elements.AddRange(New DevExpress.XtraBars.Navigation.AccordionControlElement() {Me.aceAssignedEmployee, Me.AccordionControlSeparator1, Me.AccordionControlElement2})
		Me.AccordionControlElement1.Expanded = True
		Me.AccordionControlElement1.Name = "AccordionControlElement1"
		Me.AccordionControlElement1.Text = "FILTER"
		'
		'aceAssignedEmployee
		'
		Me.aceAssignedEmployee.HeaderControl = Me.tgsAssignedEmployee
		Me.aceAssignedEmployee.Name = "aceAssignedEmployee"
		Me.aceAssignedEmployee.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item
		Me.aceAssignedEmployee.Text = "NUR für Selektierten Kunde"
		'
		'AccordionControlSeparator1
		'
		Me.AccordionControlSeparator1.Name = "AccordionControlSeparator1"
		'
		'AccordionControlElement2
		'
		Me.AccordionControlElement2.ContentContainer = Me.AccordionContentContainer2
		Me.AccordionControlElement2.Expanded = True
		Me.AccordionControlElement2.HeaderTemplate.AddRange(New DevExpress.XtraBars.Navigation.HeaderElementInfo() {New DevExpress.XtraBars.Navigation.HeaderElementInfo(DevExpress.XtraBars.Navigation.HeaderElementType.ContextButtons, DevExpress.XtraBars.Navigation.HeaderElementAlignment.Left), New DevExpress.XtraBars.Navigation.HeaderElementInfo(DevExpress.XtraBars.Navigation.HeaderElementType.Image), New DevExpress.XtraBars.Navigation.HeaderElementInfo(DevExpress.XtraBars.Navigation.HeaderElementType.Text, DevExpress.XtraBars.Navigation.HeaderElementAlignment.Right), New DevExpress.XtraBars.Navigation.HeaderElementInfo(DevExpress.XtraBars.Navigation.HeaderElementType.HeaderControl)})
		Me.AccordionControlElement2.HeaderVisible = False
		Me.AccordionControlElement2.Name = "AccordionControlElement2"
		Me.AccordionControlElement2.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item
		Me.AccordionControlElement2.Text = "Detail"
		'
		'FluentFormDefaultManager1
		'
		Me.FluentFormDefaultManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar1})
		Me.FluentFormDefaultManager1.DockingEnabled = False
		Me.FluentFormDefaultManager1.Form = Me
		'
		'Bar1
		'
		Me.Bar1.BarName = "Meine Filterbedingung"
		Me.Bar1.DockCol = 0
		Me.Bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top
		Me.Bar1.Text = "Meine Filterbedingung"
		'
		'frmMADetails
		'
		Me.Appearance.Options.UseFont = True
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(1060, 660)
		Me.Controls.Add(Me.sccMain)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.DoubleBuffered = True
		Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.IconOptions.Icon = CType(resources.GetObject("frmMADetails.IconOptions.Icon"), System.Drawing.Icon)
		Me.Name = "frmMADetails"
		Me.Text = "MetroForm"
		CType(Me.grdDetailrec, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvDetail, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.sccMain, System.ComponentModel.ISupportInitialize).EndInit()
		Me.sccMain.ResumeLayout(False)
		Me.FluentDesignFormContainer1.ResumeLayout(False)
		CType(Me.acMain, System.ComponentModel.ISupportInitialize).EndInit()
		Me.acMain.ResumeLayout(False)
		Me.AccordionContentContainer2.ResumeLayout(False)
		CType(Me.pnlContactFilter, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pnlContactFilter.ResumeLayout(False)
		Me.pnlContactFilter.PerformLayout()
		CType(Me.SeparatorControl2, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lueMonth.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lueYear.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtPlainText.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.tgsAssignedEmployee.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.FluentFormDefaultManager1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents grdDetailrec As DevExpress.XtraGrid.GridControl
  Friend WithEvents gvDetail As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents sccMain As DevExpress.XtraEditors.SplitContainerControl
	Friend WithEvents Bar3 As DevExpress.XtraBars.Bar
	Friend WithEvents bsiRecCount As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
	Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
	Friend WithEvents rlblDetailHeader As DevExpress.XtraEditors.LabelControl
	Friend WithEvents FluentFormDefaultManager1 As DevExpress.XtraBars.FluentDesignSystem.FluentFormDefaultManager
	Friend WithEvents Bar1 As DevExpress.XtraBars.Bar
	Friend WithEvents FluentDesignFormContainer1 As DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormContainer
	Friend WithEvents acMain As DevExpress.XtraBars.Navigation.AccordionControl
	Friend WithEvents AccordionContentContainer2 As DevExpress.XtraBars.Navigation.AccordionContentContainer
	Friend WithEvents pnlContactFilter As DevExpress.XtraEditors.PanelControl
	Friend WithEvents lblBezeichnung As DevExpress.XtraEditors.LabelControl
	Friend WithEvents SeparatorControl2 As DevExpress.XtraEditors.SeparatorControl
	Friend WithEvents lblMonat As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lueMonth As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents lueYear As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents lblJahr As DevExpress.XtraEditors.LabelControl
	Friend WithEvents txtPlainText As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents tgsAssignedEmployee As DevExpress.XtraEditors.ToggleSwitch
	Friend WithEvents AccordionControlElement1 As DevExpress.XtraBars.Navigation.AccordionControlElement
	Friend WithEvents aceAssignedEmployee As DevExpress.XtraBars.Navigation.AccordionControlElement
	Private WithEvents AccordionControlSeparator1 As DevExpress.XtraBars.Navigation.AccordionControlSeparator
	Friend WithEvents AccordionControlElement2 As DevExpress.XtraBars.Navigation.AccordionControlElement
End Class
