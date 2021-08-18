<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucApplication
	Inherits DevExpress.XtraEditors.XtraUserControl

	'UserControl overrides dispose to clean up the component list.
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
		Dim ButtonImageOptions1 As DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions = New DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ucApplication))
		Dim ButtonImageOptions2 As DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions = New DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions()
		Dim ButtonImageOptions3 As DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions = New DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions()
		Dim ButtonImageOptions4 As DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions = New DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions()
		Me.sccMain = New DevExpress.XtraEditors.SplitContainerControl()
		Me.pcHeader = New DevExpress.XtraEditors.PanelControl()
		Me.sccHeaderInfo = New DevExpress.XtraEditors.SplitContainerControl()
		Me.grpFunction = New DevExpress.XtraEditors.GroupControl()
		Me.rgApplicationLifeCycel = New DevExpress.XtraEditors.RadioGroup()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager()
		Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
		Me.bsiRecCount = New DevExpress.XtraBars.BarStaticItem()
		Me.bsiMDData = New DevExpress.XtraBars.BarStaticItem()
		Me.rgApplicationAdvisor = New DevExpress.XtraEditors.RadioGroup()
		Me.pccMandant = New DevExpress.XtraBars.PopupControlContainer()
		Me.Label4 = New System.Windows.Forms.Label()
		Me.cboMD = New DevExpress.XtraEditors.LookUpEdit()
		Me.grdMain = New DevExpress.XtraGrid.GridControl()
		Me.gvMain = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.Bar1 = New DevExpress.XtraBars.Bar()
		Me.Bar2 = New DevExpress.XtraBars.Bar()
		Me.Bar3 = New DevExpress.XtraBars.Bar()
		CType(Me.sccMain, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.sccMain.SuspendLayout()
		CType(Me.pcHeader, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.pcHeader.SuspendLayout()
		CType(Me.sccHeaderInfo, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.sccHeaderInfo.SuspendLayout()
		CType(Me.grpFunction, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grpFunction.SuspendLayout()
		CType(Me.rgApplicationLifeCycel.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.rgApplicationAdvisor.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.pccMandant, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.pccMandant.SuspendLayout()
		CType(Me.cboMD.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.grdMain, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvMain, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'sccMain
		'
		Me.sccMain.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.[Default]
		Me.sccMain.CollapsePanel = DevExpress.XtraEditors.SplitCollapsePanel.Panel1
		Me.sccMain.Dock = System.Windows.Forms.DockStyle.Fill
		Me.sccMain.Horizontal = False
		Me.sccMain.Location = New System.Drawing.Point(0, 0)
		Me.sccMain.Name = "sccMain"
		Me.sccMain.Panel1.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.sccMain.Panel1.Appearance.Options.UseBackColor = True
		Me.sccMain.Panel1.Controls.Add(Me.pcHeader)
		Me.sccMain.Panel1.Text = "Panel1"
		Me.sccMain.Panel2.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.sccMain.Panel2.Appearance.Options.UseBackColor = True
		Me.sccMain.Panel2.Controls.Add(Me.pccMandant)
		Me.sccMain.Panel2.Controls.Add(Me.grdMain)
		Me.sccMain.Panel2.Text = "Panel2"
		Me.sccMain.Size = New System.Drawing.Size(1008, 702)
		Me.sccMain.SplitterPosition = 138
		Me.sccMain.TabIndex = 159
		'
		'pcHeader
		'
		Me.pcHeader.Controls.Add(Me.sccHeaderInfo)
		Me.pcHeader.Dock = System.Windows.Forms.DockStyle.Fill
		Me.pcHeader.Location = New System.Drawing.Point(0, 0)
		Me.pcHeader.Name = "pcHeader"
		Me.pcHeader.Padding = New System.Windows.Forms.Padding(5, 0, 0, 0)
		Me.pcHeader.Size = New System.Drawing.Size(1004, 138)
		Me.pcHeader.TabIndex = 0
		'
		'sccHeaderInfo
		'
		Me.sccHeaderInfo.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.sccHeaderInfo.Appearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
		Me.sccHeaderInfo.Appearance.Options.UseBackColor = True
		Me.sccHeaderInfo.Appearance.Options.UseBorderColor = True
		Me.sccHeaderInfo.AppearanceCaption.Image = Global.SPS.MainView.My.Resources.Resources.more_arrow
		Me.sccHeaderInfo.AppearanceCaption.Options.UseImage = True
		Me.sccHeaderInfo.CaptionImageOptions.Image = Global.SPS.MainView.My.Resources.Resources.more_arrow
		Me.sccHeaderInfo.Dock = System.Windows.Forms.DockStyle.Fill
		Me.sccHeaderInfo.IsSplitterFixed = True
		Me.sccHeaderInfo.Location = New System.Drawing.Point(7, 2)
		Me.sccHeaderInfo.Name = "sccHeaderInfo"
		Me.sccHeaderInfo.Padding = New System.Windows.Forms.Padding(1)
		Me.sccHeaderInfo.Panel1.Controls.Add(Me.grpFunction)
		Me.sccHeaderInfo.Panel1.Padding = New System.Windows.Forms.Padding(5)
		Me.sccHeaderInfo.Panel1.Text = "pMABild"
		Me.sccHeaderInfo.Panel2.MinSize = 150
		Me.sccHeaderInfo.Panel2.Padding = New System.Windows.Forms.Padding(5)
		Me.sccHeaderInfo.Panel2.Text = "pMADetail"
		Me.sccHeaderInfo.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel1
		Me.sccHeaderInfo.Size = New System.Drawing.Size(995, 134)
		Me.sccHeaderInfo.SplitterPosition = 823
		Me.sccHeaderInfo.TabIndex = 9
		Me.sccHeaderInfo.Text = "SplitContainerControl2"
		'
		'grpFunction
		'
		Me.grpFunction.Controls.Add(Me.rgApplicationLifeCycel)
		Me.grpFunction.Controls.Add(Me.rgApplicationAdvisor)
		ButtonImageOptions1.Image = CType(resources.GetObject("ButtonImageOptions1.Image"), System.Drawing.Image)
		ButtonImageOptions2.Image = CType(resources.GetObject("ButtonImageOptions2.Image"), System.Drawing.Image)
		ButtonImageOptions3.Image = CType(resources.GetObject("ButtonImageOptions3.Image"), System.Drawing.Image)
		ButtonImageOptions3.Location = DevExpress.XtraEditors.ButtonPanel.ImageLocation.AfterText
		ButtonImageOptions4.Image = CType(resources.GetObject("ButtonImageOptions4.Image"), System.Drawing.Image)
		ButtonImageOptions4.Location = DevExpress.XtraEditors.ButtonPanel.ImageLocation.AfterText
		Me.grpFunction.CustomHeaderButtons.AddRange(New DevExpress.XtraEditors.ButtonPanel.IBaseButton() {New DevExpress.XtraEditors.ButtonsPanelControl.GroupBoxButton("Quick Search", True, ButtonImageOptions1, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", -1, True, Nothing, True, False, True, Nothing, 3), New DevExpress.XtraEditors.ButtonsPanelControl.GroupBoxButton("Erweiterte Suche", True, ButtonImageOptions2, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", -1, True, Nothing, True, False, True, Nothing, 2), New DevExpress.XtraEditors.ButtonsPanelControl.GroupBoxButton("", True, ButtonImageOptions3, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", -1, True, Nothing, True, False, True, Nothing, 1), New DevExpress.XtraEditors.ButtonsPanelControl.GroupBoxButton("", True, ButtonImageOptions4, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", -1, True, Nothing, True, False, True, Nothing, 0)})
		Me.grpFunction.CustomHeaderButtonsLocation = DevExpress.Utils.GroupElementLocation.AfterText
		Me.grpFunction.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grpFunction.Location = New System.Drawing.Point(5, 5)
		Me.grpFunction.Name = "grpFunction"
		Me.grpFunction.Size = New System.Drawing.Size(979, 118)
		Me.grpFunction.TabIndex = 0
		Me.grpFunction.Text = "Filterbedingungen"
		'
		'rgApplicationLifeCycel
		'
		Me.rgApplicationLifeCycel.Location = New System.Drawing.Point(19, 47)
		Me.rgApplicationLifeCycel.MenuManager = Me.BarManager1
		Me.rgApplicationLifeCycel.Name = "rgApplicationLifeCycel"
		Me.rgApplicationLifeCycel.Properties.AllowFocused = False
		Me.rgApplicationLifeCycel.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "Alle"), New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "Neu"), New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "In Bearbeitung"), New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "Abgeschlossen")})
		Me.rgApplicationLifeCycel.Size = New System.Drawing.Size(292, 49)
		Me.rgApplicationLifeCycel.TabIndex = 1
		'
		'BarManager1
		'
		Me.BarManager1.DockControls.Add(Me.barDockControlTop)
		Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
		Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
		Me.BarManager1.DockControls.Add(Me.barDockControlRight)
		Me.BarManager1.Form = Me
		Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiRecCount, Me.bsiMDData})
		Me.BarManager1.MaxItemId = 2
		'
		'barDockControlTop
		'
		Me.barDockControlTop.CausesValidation = False
		Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
		Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlTop.Manager = Me.BarManager1
		Me.barDockControlTop.Size = New System.Drawing.Size(1008, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 702)
		Me.barDockControlBottom.Manager = Me.BarManager1
		Me.barDockControlBottom.Size = New System.Drawing.Size(1008, 0)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Manager = Me.BarManager1
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 702)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(1008, 0)
		Me.barDockControlRight.Manager = Me.BarManager1
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 702)
		'
		'bsiRecCount
		'
		Me.bsiRecCount.Caption = "Bereit"
		Me.bsiRecCount.Id = 0
		Me.bsiRecCount.Name = "bsiRecCount"
		'
		'bsiMDData
		'
		Me.bsiMDData.Id = 1
		Me.bsiMDData.Name = "bsiMDData"
		'
		'rgApplicationAdvisor
		'
		Me.rgApplicationAdvisor.Location = New System.Drawing.Point(317, 47)
		Me.rgApplicationAdvisor.MenuManager = Me.BarManager1
		Me.rgApplicationAdvisor.Name = "rgApplicationAdvisor"
		Me.rgApplicationAdvisor.Properties.AllowFocused = False
		Me.rgApplicationAdvisor.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "Alle Bewerbungen"), New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "Meine Bewerbungen"), New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "Spontan Bewerbungen")})
		Me.rgApplicationAdvisor.Size = New System.Drawing.Size(363, 49)
		Me.rgApplicationAdvisor.TabIndex = 0
		'
		'pccMandant
		'
		Me.pccMandant.Appearance.BackColor = System.Drawing.Color.White
		Me.pccMandant.Appearance.Options.UseBackColor = True
		Me.pccMandant.AutoSize = True
		Me.pccMandant.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
		Me.pccMandant.Controls.Add(Me.Label4)
		Me.pccMandant.Controls.Add(Me.cboMD)
		Me.pccMandant.Location = New System.Drawing.Point(443, 233)
		Me.pccMandant.Manager = Me.BarManager1
		Me.pccMandant.Name = "pccMandant"
		Me.pccMandant.Size = New System.Drawing.Size(361, 103)
		Me.pccMandant.TabIndex = 162
		Me.pccMandant.Visible = False
		'
		'Label4
		'
		Me.Label4.Location = New System.Drawing.Point(13, 24)
		Me.Label4.Name = "Label4"
		Me.Label4.Size = New System.Drawing.Size(107, 13)
		Me.Label4.TabIndex = 20
		Me.Label4.Text = "Mandanten"
		Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'cboMD
		'
		Me.cboMD.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.cboMD.Location = New System.Drawing.Point(126, 21)
		Me.cboMD.Name = "cboMD"
		Me.cboMD.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.cboMD.Properties.NullText = ""
		Me.cboMD.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
		Me.cboMD.Size = New System.Drawing.Size(211, 20)
		Me.cboMD.TabIndex = 21
		'
		'grdMain
		'
		Me.grdMain.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdMain.Location = New System.Drawing.Point(0, 0)
		Me.grdMain.MainView = Me.gvMain
		Me.grdMain.Name = "grdMain"
		Me.grdMain.Size = New System.Drawing.Size(1004, 555)
		Me.grdMain.TabIndex = 153
		Me.grdMain.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvMain})
		'
		'gvMain
		'
		Me.gvMain.GridControl = Me.grdMain
		Me.gvMain.Name = "gvMain"
		Me.gvMain.OptionsBehavior.Editable = False
		Me.gvMain.OptionsFind.AlwaysVisible = True
		Me.gvMain.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvMain.OptionsView.ShowAutoFilterRow = True
		Me.gvMain.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways
		Me.gvMain.OptionsView.ShowGroupedColumns = True
		Me.gvMain.OptionsView.ShowGroupPanel = False
		Me.gvMain.OptionsView.WaitAnimationOptions = DevExpress.XtraEditors.WaitAnimationOptions.Panel
		'
		'Bar1
		'
		Me.Bar1.BarName = "Statusleiste"
		Me.Bar1.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
		Me.Bar1.DockCol = 0
		Me.Bar1.DockRow = 0
		Me.Bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
		Me.Bar1.OptionsBar.AllowQuickCustomization = False
		Me.Bar1.OptionsBar.DrawDragBorder = False
		Me.Bar1.OptionsBar.UseWholeRow = True
		Me.Bar1.Text = "Statusleiste"
		'
		'Bar2
		'
		Me.Bar2.BarName = "Statusleiste"
		Me.Bar2.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
		Me.Bar2.DockCol = 0
		Me.Bar2.DockRow = 0
		Me.Bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
		Me.Bar2.OptionsBar.AllowQuickCustomization = False
		Me.Bar2.OptionsBar.DrawDragBorder = False
		Me.Bar2.OptionsBar.UseWholeRow = True
		Me.Bar2.Text = "Statusleiste"
		'
		'Bar3
		'
		Me.Bar3.BarName = "Statusleiste"
		Me.Bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
		Me.Bar3.DockCol = 0
		Me.Bar3.DockRow = 0
		Me.Bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
		Me.Bar3.OptionsBar.AllowQuickCustomization = False
		Me.Bar3.OptionsBar.DrawDragBorder = False
		Me.Bar3.OptionsBar.UseWholeRow = True
		Me.Bar3.Text = "Statusleiste"
		'
		'ucApplication
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.Controls.Add(Me.sccMain)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.Name = "ucApplication"
		Me.Size = New System.Drawing.Size(1008, 702)
		CType(Me.sccMain, System.ComponentModel.ISupportInitialize).EndInit()
		Me.sccMain.ResumeLayout(False)
		CType(Me.pcHeader, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pcHeader.ResumeLayout(False)
		CType(Me.sccHeaderInfo, System.ComponentModel.ISupportInitialize).EndInit()
		Me.sccHeaderInfo.ResumeLayout(False)
		CType(Me.grpFunction, System.ComponentModel.ISupportInitialize).EndInit()
		Me.grpFunction.ResumeLayout(False)
		CType(Me.rgApplicationLifeCycel.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.rgApplicationAdvisor.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.pccMandant, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pccMandant.ResumeLayout(False)
		CType(Me.cboMD.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.grdMain, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvMain, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents sccMain As DevExpress.XtraEditors.SplitContainerControl
	Friend WithEvents pcHeader As DevExpress.XtraEditors.PanelControl
	Friend WithEvents sccHeaderInfo As DevExpress.XtraEditors.SplitContainerControl
	Friend WithEvents pccMandant As DevExpress.XtraBars.PopupControlContainer
	Friend WithEvents Label4 As System.Windows.Forms.Label
	Friend WithEvents grdMain As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvMain As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents Bar1 As DevExpress.XtraBars.Bar
	Friend WithEvents Bar2 As DevExpress.XtraBars.Bar
	Friend WithEvents Bar3 As DevExpress.XtraBars.Bar
	Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
	Friend WithEvents bsiRecCount As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
	Friend WithEvents cboMD As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents bsiMDData As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents grpFunction As DevExpress.XtraEditors.GroupControl
	Friend WithEvents rgApplicationLifeCycel As DevExpress.XtraEditors.RadioGroup
	Friend WithEvents rgApplicationAdvisor As DevExpress.XtraEditors.RadioGroup

End Class
