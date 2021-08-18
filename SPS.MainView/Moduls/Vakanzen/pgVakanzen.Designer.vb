<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class pgVakanzen
    Inherits System.Windows.Forms.UserControl

    'UserControl überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()>
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
		Dim ButtonImageOptions1 As DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions = New DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions()
		Dim ButtonImageOptions2 As DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions = New DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(pgVakanzen))
		Dim ButtonImageOptions3 As DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions = New DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions()
		Dim ButtonImageOptions4 As DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions = New DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions()
		Me.pcHeader = New DevExpress.XtraEditors.PanelControl()
		Me.sccHeaderInfo = New DevExpress.XtraEditors.SplitContainerControl()
		Me.scHeaderDetail1 = New DevExpress.XtraEditors.XtraScrollableControl()
		Me.grdLProperty = New DevExpress.XtraGrid.GridControl()
		Me.gvLProperty = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.scHeaderDetail2 = New DevExpress.XtraEditors.XtraScrollableControl()
		Me.grdJobplattforms = New DevExpress.XtraGrid.GridControl()
		Me.gvJobplattforms = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.sccMain = New DevExpress.XtraEditors.SplitContainerControl()
		Me.pccMandant = New DevExpress.XtraBars.PopupControlContainer()
		Me.Label4 = New System.Windows.Forms.Label()
		Me.cboMD = New DevExpress.XtraEditors.LookUpEdit()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager()
		Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
		Me.bsiRecCount = New DevExpress.XtraBars.BarStaticItem()
		Me.bsiMDData = New DevExpress.XtraBars.BarStaticItem()
		Me.grdMain = New DevExpress.XtraGrid.GridControl()
		Me.gvMain = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.DockManager1 = New DevExpress.XtraBars.Docking.DockManager()
		Me.dpProperties = New DevExpress.XtraBars.Docking.DockPanel()
		Me.DockPanel1_Container = New DevExpress.XtraBars.Docking.ControlContainer()
		Me.sccMainNav_1 = New DevExpress.XtraEditors.SplitContainerControl()
		Me.grpFunction = New DevExpress.XtraEditors.GroupControl()
		Me.cmdNew = New DevExpress.XtraEditors.DropDownButton()
		Me.StyleController1 = New DevExpress.XtraEditors.StyleController()
		Me.cmdPrint = New DevExpress.XtraEditors.DropDownButton()
		Me.sccMainProp_1 = New DevExpress.XtraEditors.SplitContainerControl()
		Me.grpPropose = New DevExpress.XtraEditors.GroupControl()
		Me.grdPropose = New DevExpress.XtraGrid.GridControl()
		Me.gvPropose = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.grpES = New DevExpress.XtraEditors.GroupControl()
		Me.grdES = New DevExpress.XtraGrid.GridControl()
		Me.gvES = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.ImageCollection1 = New DevExpress.Utils.ImageCollection()
		CType(Me.pcHeader, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.sccHeaderInfo, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.sccHeaderInfo.SuspendLayout()
		Me.scHeaderDetail1.SuspendLayout()
		CType(Me.grdLProperty, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvLProperty, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.scHeaderDetail2.SuspendLayout()
		CType(Me.grdJobplattforms, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvJobplattforms, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.sccMain, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.sccMain.SuspendLayout()
		CType(Me.pccMandant, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.pccMandant.SuspendLayout()
		CType(Me.cboMD.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.grdMain, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvMain, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.DockManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.dpProperties.SuspendLayout()
		Me.DockPanel1_Container.SuspendLayout()
		CType(Me.sccMainNav_1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.sccMainNav_1.SuspendLayout()
		CType(Me.grpFunction, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grpFunction.SuspendLayout()
		CType(Me.StyleController1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.sccMainProp_1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.sccMainProp_1.SuspendLayout()
		CType(Me.grpPropose, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grpPropose.SuspendLayout()
		CType(Me.grdPropose, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvPropose, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.grpES, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grpES.SuspendLayout()
		CType(Me.grdES, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvES, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.ImageCollection1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'pcHeader
		'
		Me.pcHeader.Location = New System.Drawing.Point(253, 6)
		Me.pcHeader.Name = "pcHeader"
		Me.pcHeader.Padding = New System.Windows.Forms.Padding(5, 0, 0, 0)
		Me.pcHeader.Size = New System.Drawing.Size(60, 40)
		Me.pcHeader.TabIndex = 0
		'
		'sccHeaderInfo
		'
		Me.sccHeaderInfo.AppearanceCaption.Image = Global.SPS.MainView.My.Resources.Resources.more_arrow
		Me.sccHeaderInfo.AppearanceCaption.Options.UseImage = True
		Me.sccHeaderInfo.CaptionImageOptions.Image = Global.SPS.MainView.My.Resources.Resources.more_arrow
		Me.sccHeaderInfo.Dock = System.Windows.Forms.DockStyle.Fill
		Me.sccHeaderInfo.Location = New System.Drawing.Point(0, 0)
		Me.sccHeaderInfo.Name = "sccHeaderInfo"
		Me.sccHeaderInfo.Padding = New System.Windows.Forms.Padding(1)
		Me.sccHeaderInfo.Panel1.Controls.Add(Me.scHeaderDetail1)
		Me.sccHeaderInfo.Panel1.Padding = New System.Windows.Forms.Padding(2)
		Me.sccHeaderInfo.Panel1.Text = "pMABild"
		Me.sccHeaderInfo.Panel2.Controls.Add(Me.scHeaderDetail2)
		Me.sccHeaderInfo.Panel2.Padding = New System.Windows.Forms.Padding(2)
		Me.sccHeaderInfo.Panel2.Text = "pMADetail"
		Me.sccHeaderInfo.Size = New System.Drawing.Size(800, 176)
		Me.sccHeaderInfo.SplitterPosition = 393
		Me.sccHeaderInfo.TabIndex = 163
		Me.sccHeaderInfo.Text = "SplitContainerControl2"
		'
		'scHeaderDetail1
		'
		Me.scHeaderDetail1.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.scHeaderDetail1.Appearance.Options.UseBackColor = True
		Me.scHeaderDetail1.Controls.Add(Me.grdLProperty)
		Me.scHeaderDetail1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.scHeaderDetail1.Location = New System.Drawing.Point(2, 2)
		Me.scHeaderDetail1.Name = "scHeaderDetail1"
		Me.scHeaderDetail1.Size = New System.Drawing.Size(389, 170)
		Me.scHeaderDetail1.TabIndex = 2
		'
		'grdLProperty
		'
		Me.grdLProperty.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdLProperty.Location = New System.Drawing.Point(0, 0)
		Me.grdLProperty.MainView = Me.gvLProperty
		Me.grdLProperty.Name = "grdLProperty"
		Me.grdLProperty.Size = New System.Drawing.Size(389, 170)
		Me.grdLProperty.TabIndex = 8
		Me.grdLProperty.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvLProperty})
		'
		'gvLProperty
		'
		Me.gvLProperty.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvLProperty.GridControl = Me.grdLProperty
		Me.gvLProperty.Name = "gvLProperty"
		Me.gvLProperty.OptionsBehavior.Editable = False
		Me.gvLProperty.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvLProperty.OptionsView.ShowColumnHeaders = False
		Me.gvLProperty.OptionsView.ShowGroupPanel = False
		'
		'scHeaderDetail2
		'
		Me.scHeaderDetail2.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.scHeaderDetail2.Appearance.Options.UseBackColor = True
		Me.scHeaderDetail2.Controls.Add(Me.grdJobplattforms)
		Me.scHeaderDetail2.Dock = System.Windows.Forms.DockStyle.Fill
		Me.scHeaderDetail2.Location = New System.Drawing.Point(2, 2)
		Me.scHeaderDetail2.Name = "scHeaderDetail2"
		Me.scHeaderDetail2.Size = New System.Drawing.Size(396, 170)
		Me.scHeaderDetail2.TabIndex = 3
		'
		'grdJobplattforms
		'
		Me.grdJobplattforms.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdJobplattforms.Location = New System.Drawing.Point(0, 0)
		Me.grdJobplattforms.MainView = Me.gvJobplattforms
		Me.grdJobplattforms.Name = "grdJobplattforms"
		Me.grdJobplattforms.Size = New System.Drawing.Size(396, 170)
		Me.grdJobplattforms.TabIndex = 7
		Me.grdJobplattforms.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvJobplattforms})
		'
		'gvJobplattforms
		'
		Me.gvJobplattforms.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvJobplattforms.GridControl = Me.grdJobplattforms
		Me.gvJobplattforms.Name = "gvJobplattforms"
		Me.gvJobplattforms.OptionsBehavior.Editable = False
		Me.gvJobplattforms.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvJobplattforms.OptionsView.ShowColumnHeaders = False
		Me.gvJobplattforms.OptionsView.ShowGroupPanel = False
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
		Me.sccMain.Panel1.Controls.Add(Me.sccHeaderInfo)
		Me.sccMain.Panel1.Controls.Add(Me.pcHeader)
		Me.sccMain.Panel1.Text = "Panel1"
		Me.sccMain.Panel2.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
		Me.sccMain.Panel2.Appearance.Options.UseBackColor = True
		Me.sccMain.Panel2.Controls.Add(Me.pccMandant)
		Me.sccMain.Panel2.Controls.Add(Me.grdMain)
		Me.sccMain.Panel2.Text = "Panel2"
		Me.sccMain.Size = New System.Drawing.Size(804, 669)
		Me.sccMain.SplitterPosition = 176
		Me.sccMain.TabIndex = 157
		'
		'pccMandant
		'
		Me.pccMandant.Appearance.BackColor = System.Drawing.Color.White
		Me.pccMandant.Appearance.Options.UseBackColor = True
		Me.pccMandant.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
		Me.pccMandant.Controls.Add(Me.Label4)
		Me.pccMandant.Controls.Add(Me.cboMD)
		Me.pccMandant.Location = New System.Drawing.Point(121, 173)
		Me.pccMandant.Manager = Me.BarManager1
		Me.pccMandant.Name = "pccMandant"
		Me.pccMandant.Size = New System.Drawing.Size(361, 103)
		Me.pccMandant.TabIndex = 162
		Me.pccMandant.Visible = False
		'
		'Label4
		'
		Me.Label4.BackColor = System.Drawing.Color.Transparent
		Me.Label4.Location = New System.Drawing.Point(13, 25)
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
		Me.cboMD.MenuManager = Me.BarManager1
		Me.cboMD.Name = "cboMD"
		Me.cboMD.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.cboMD.Properties.NullText = ""
		Me.cboMD.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
		Me.cboMD.Size = New System.Drawing.Size(211, 20)
		Me.cboMD.TabIndex = 21
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
		Me.barDockControlTop.Size = New System.Drawing.Size(1059, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 669)
		Me.barDockControlBottom.Manager = Me.BarManager1
		Me.barDockControlBottom.Size = New System.Drawing.Size(1059, 0)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Manager = Me.BarManager1
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 669)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(1059, 0)
		Me.barDockControlRight.Manager = Me.BarManager1
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 669)
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
		'grdMain
		'
		Me.grdMain.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdMain.Location = New System.Drawing.Point(0, 0)
		Me.grdMain.MainView = Me.gvMain
		Me.grdMain.Name = "grdMain"
		Me.grdMain.Size = New System.Drawing.Size(800, 484)
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
		Me.gvMain.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways
		Me.gvMain.OptionsView.ShowGroupedColumns = True
		Me.gvMain.OptionsView.ShowGroupPanel = False
		Me.gvMain.OptionsView.WaitAnimationOptions = DevExpress.XtraEditors.WaitAnimationOptions.Panel
		'
		'DockManager1
		'
		Me.DockManager1.Form = Me
		Me.DockManager1.MenuManager = Me.BarManager1
		Me.DockManager1.RootPanels.AddRange(New DevExpress.XtraBars.Docking.DockPanel() {Me.dpProperties})
		Me.DockManager1.TopZIndexControls.AddRange(New String() {"DevExpress.XtraBars.BarDockControl", "DevExpress.XtraBars.StandaloneBarDockControl", "System.Windows.Forms.StatusBar", "System.Windows.Forms.MenuStrip", "System.Windows.Forms.StatusStrip", "DevExpress.XtraBars.Ribbon.RibbonStatusBar", "DevExpress.XtraBars.Ribbon.RibbonControl"})
		'
		'dpProperties
		'
		Me.dpProperties.Controls.Add(Me.DockPanel1_Container)
		Me.dpProperties.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right
		Me.dpProperties.ID = New System.Guid("650ca3f6-5fe5-4c19-83db-566087d793ad")
		Me.dpProperties.Location = New System.Drawing.Point(804, 0)
		Me.dpProperties.Name = "dpProperties"
		Me.dpProperties.OriginalSize = New System.Drawing.Size(255, 200)
		Me.dpProperties.SavedSizeFactor = 0R
		Me.dpProperties.Size = New System.Drawing.Size(255, 669)
		Me.dpProperties.Text = "Abhängigkeiten"
		'
		'DockPanel1_Container
		'
		Me.DockPanel1_Container.Controls.Add(Me.sccMainNav_1)
		Me.DockPanel1_Container.Location = New System.Drawing.Point(5, 23)
		Me.DockPanel1_Container.Name = "DockPanel1_Container"
		Me.DockPanel1_Container.Size = New System.Drawing.Size(246, 642)
		Me.DockPanel1_Container.TabIndex = 0
		'
		'sccMainNav_1
		'
		Me.sccMainNav_1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.sccMainNav_1.Horizontal = False
		Me.sccMainNav_1.IsSplitterFixed = True
		Me.sccMainNav_1.Location = New System.Drawing.Point(0, 0)
		Me.sccMainNav_1.Name = "sccMainNav_1"
		Me.sccMainNav_1.Panel1.Controls.Add(Me.grpFunction)
		Me.sccMainNav_1.Panel1.Padding = New System.Windows.Forms.Padding(5)
		Me.sccMainNav_1.Panel1.Text = "Panel1"
		Me.sccMainNav_1.Panel2.Controls.Add(Me.sccMainProp_1)
		Me.sccMainNav_1.Panel2.Text = "Panel2"
		Me.sccMainNav_1.Size = New System.Drawing.Size(246, 642)
		Me.sccMainNav_1.SplitterPosition = 74
		Me.sccMainNav_1.TabIndex = 167
		Me.sccMainNav_1.Text = "SplitContainerControl1"
		'
		'grpFunction
		'
		Me.grpFunction.AppearanceCaption.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.grpFunction.AppearanceCaption.Options.UseFont = True
		Me.grpFunction.Controls.Add(Me.cmdNew)
		Me.grpFunction.Controls.Add(Me.cmdPrint)
		ButtonImageOptions1.Image = CType(resources.GetObject("ButtonImageOptions1.Image"), System.Drawing.Image)
		ButtonImageOptions2.Image = CType(resources.GetObject("ButtonImageOptions2.Image"), System.Drawing.Image)
		Me.grpFunction.CustomHeaderButtons.AddRange(New DevExpress.XtraEditors.ButtonPanel.IBaseButton() {New DevExpress.XtraEditors.ButtonsPanelControl.GroupBoxButton("", True, ButtonImageOptions1, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", -1, True, Nothing, True, False, True, Nothing, 0), New DevExpress.XtraEditors.ButtonsPanelControl.GroupBoxButton("", True, ButtonImageOptions2, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", -1, True, Nothing, True, False, True, Nothing, 1)})
		Me.grpFunction.CustomHeaderButtonsLocation = DevExpress.Utils.GroupElementLocation.AfterText
		Me.grpFunction.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grpFunction.Location = New System.Drawing.Point(5, 5)
		Me.grpFunction.Name = "grpFunction"
		Me.grpFunction.Size = New System.Drawing.Size(236, 64)
		Me.grpFunction.TabIndex = 164
		Me.grpFunction.Text = "Funktionen"
		'
		'cmdNew
		'
		Me.cmdNew.ImageOptions.Image = CType(resources.GetObject("cmdNew.ImageOptions.Image"), System.Drawing.Image)
		Me.cmdNew.Location = New System.Drawing.Point(113, 33)
		Me.cmdNew.MenuManager = Me.BarManager1
		Me.cmdNew.Name = "cmdNew"
		Me.cmdNew.Size = New System.Drawing.Size(97, 22)
		Me.cmdNew.StyleController = Me.StyleController1
		Me.cmdNew.TabIndex = 3
		Me.cmdNew.Text = "Neu"
		'
		'cmdPrint
		'
		Me.cmdPrint.ImageOptions.Image = CType(resources.GetObject("cmdPrint.ImageOptions.Image"), System.Drawing.Image)
		Me.cmdPrint.Location = New System.Drawing.Point(10, 33)
		Me.cmdPrint.MenuManager = Me.BarManager1
		Me.cmdPrint.Name = "cmdPrint"
		Me.cmdPrint.Size = New System.Drawing.Size(97, 22)
		Me.cmdPrint.StyleController = Me.StyleController1
		Me.cmdPrint.TabIndex = 1
		Me.cmdPrint.Text = "Drucken"
		'
		'sccMainProp_1
		'
		Me.sccMainProp_1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.sccMainProp_1.Horizontal = False
		Me.sccMainProp_1.Location = New System.Drawing.Point(0, 0)
		Me.sccMainProp_1.Name = "sccMainProp_1"
		Me.sccMainProp_1.Panel1.Controls.Add(Me.grpPropose)
		Me.sccMainProp_1.Panel1.Padding = New System.Windows.Forms.Padding(5)
		Me.sccMainProp_1.Panel1.Text = "Panel1"
		Me.sccMainProp_1.Panel2.Controls.Add(Me.grpES)
		Me.sccMainProp_1.Panel2.Padding = New System.Windows.Forms.Padding(5)
		Me.sccMainProp_1.Panel2.Text = "Panel2"
		Me.sccMainProp_1.Size = New System.Drawing.Size(246, 563)
		Me.sccMainProp_1.SplitterPosition = 207
		Me.sccMainProp_1.TabIndex = 168
		Me.sccMainProp_1.Text = "SplitContainerControl2"
		'
		'grpPropose
		'
		Me.grpPropose.AppearanceCaption.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.grpPropose.AppearanceCaption.Options.UseFont = True
		Me.grpPropose.Controls.Add(Me.grdPropose)
		ButtonImageOptions3.Image = CType(resources.GetObject("ButtonImageOptions3.Image"), System.Drawing.Image)
		Me.grpPropose.CustomHeaderButtons.AddRange(New DevExpress.XtraEditors.ButtonPanel.IBaseButton() {New DevExpress.XtraEditors.ButtonsPanelControl.GroupBoxButton("", True, ButtonImageOptions3, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", -1, True, Nothing, True, False, True, Nothing, 0)})
		Me.grpPropose.CustomHeaderButtonsLocation = DevExpress.Utils.GroupElementLocation.AfterText
		Me.grpPropose.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grpPropose.Location = New System.Drawing.Point(5, 5)
		Me.grpPropose.Name = "grpPropose"
		Me.grpPropose.Padding = New System.Windows.Forms.Padding(5)
		Me.grpPropose.Size = New System.Drawing.Size(236, 197)
		Me.grpPropose.TabIndex = 165
		Me.grpPropose.Text = "Vorschläge"
		'
		'grdPropose
		'
		Me.grdPropose.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdPropose.Location = New System.Drawing.Point(7, 34)
		Me.grdPropose.MainView = Me.gvPropose
		Me.grdPropose.MenuManager = Me.BarManager1
		Me.grdPropose.Name = "grdPropose"
		Me.grdPropose.Size = New System.Drawing.Size(222, 156)
		Me.grdPropose.TabIndex = 1
		Me.grdPropose.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvPropose})
		'
		'gvPropose
		'
		Me.gvPropose.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvPropose.GridControl = Me.grdPropose
		Me.gvPropose.Name = "gvPropose"
		Me.gvPropose.OptionsBehavior.Editable = False
		Me.gvPropose.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvPropose.OptionsView.ShowGroupPanel = False
		'
		'grpES
		'
		Me.grpES.AppearanceCaption.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.grpES.AppearanceCaption.Options.UseFont = True
		Me.grpES.Controls.Add(Me.grdES)
		ButtonImageOptions4.Image = CType(resources.GetObject("ButtonImageOptions4.Image"), System.Drawing.Image)
		Me.grpES.CustomHeaderButtons.AddRange(New DevExpress.XtraEditors.ButtonPanel.IBaseButton() {New DevExpress.XtraEditors.ButtonsPanelControl.GroupBoxButton("", True, ButtonImageOptions4, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", -1, True, Nothing, True, False, True, Nothing, 0)})
		Me.grpES.CustomHeaderButtonsLocation = DevExpress.Utils.GroupElementLocation.AfterText
		Me.grpES.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grpES.Location = New System.Drawing.Point(5, 5)
		Me.grpES.Name = "grpES"
		Me.grpES.Padding = New System.Windows.Forms.Padding(5)
		Me.grpES.Size = New System.Drawing.Size(236, 341)
		Me.grpES.TabIndex = 166
		Me.grpES.Text = "Aktive Einsätze"
		'
		'grdES
		'
		Me.grdES.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdES.Location = New System.Drawing.Point(7, 34)
		Me.grdES.MainView = Me.gvES
		Me.grdES.MenuManager = Me.BarManager1
		Me.grdES.Name = "grdES"
		Me.grdES.Size = New System.Drawing.Size(222, 300)
		Me.grdES.TabIndex = 2
		Me.grdES.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvES})
		'
		'gvES
		'
		Me.gvES.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvES.GridControl = Me.grdES
		Me.gvES.Name = "gvES"
		Me.gvES.OptionsBehavior.Editable = False
		Me.gvES.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvES.OptionsView.ShowGroupPanel = False
		'
		'ImageCollection1
		'
		Me.ImageCollection1.ImageStream = CType(resources.GetObject("ImageCollection1.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
		Me.ImageCollection1.Images.SetKeyName(0, "warning_16x16.png")
		Me.ImageCollection1.Images.SetKeyName(1, "question_16x16.png")
		Me.ImageCollection1.Images.SetKeyName(2, "protectsheet_16x16.png")
		Me.ImageCollection1.Images.SetKeyName(3, "show_16x16.png")
		Me.ImageCollection1.InsertImage(Global.SPS.MainView.My.Resources.Resources.bullet_ball_green, "bullet_ball_green", GetType(Global.SPS.MainView.My.Resources.Resources), 4)
		Me.ImageCollection1.Images.SetKeyName(4, "bullet_ball_green")
		Me.ImageCollection1.InsertImage(Global.SPS.MainView.My.Resources.Resources.bullet_ball_red, "bullet_ball_red", GetType(Global.SPS.MainView.My.Resources.Resources), 5)
		Me.ImageCollection1.Images.SetKeyName(5, "bullet_ball_red")
		Me.ImageCollection1.InsertImage(Global.SPS.MainView.My.Resources.Resources.bullet_ball_yellow, "bullet_ball_yellow", GetType(Global.SPS.MainView.My.Resources.Resources), 6)
		Me.ImageCollection1.Images.SetKeyName(6, "bullet_ball_yellow")
		'
		'pgVakanzen
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.Controls.Add(Me.sccMain)
		Me.Controls.Add(Me.dpProperties)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.Name = "pgVakanzen"
		Me.Size = New System.Drawing.Size(1059, 669)
		CType(Me.pcHeader, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.sccHeaderInfo, System.ComponentModel.ISupportInitialize).EndInit()
		Me.sccHeaderInfo.ResumeLayout(False)
		Me.scHeaderDetail1.ResumeLayout(False)
		CType(Me.grdLProperty, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvLProperty, System.ComponentModel.ISupportInitialize).EndInit()
		Me.scHeaderDetail2.ResumeLayout(False)
		CType(Me.grdJobplattforms, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvJobplattforms, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.sccMain, System.ComponentModel.ISupportInitialize).EndInit()
		Me.sccMain.ResumeLayout(False)
		CType(Me.pccMandant, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pccMandant.ResumeLayout(False)
		CType(Me.cboMD.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.grdMain, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvMain, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.DockManager1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.dpProperties.ResumeLayout(False)
		Me.DockPanel1_Container.ResumeLayout(False)
		CType(Me.sccMainNav_1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.sccMainNav_1.ResumeLayout(False)
		CType(Me.grpFunction, System.ComponentModel.ISupportInitialize).EndInit()
		Me.grpFunction.ResumeLayout(False)
		CType(Me.StyleController1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.sccMainProp_1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.sccMainProp_1.ResumeLayout(False)
		CType(Me.grpPropose, System.ComponentModel.ISupportInitialize).EndInit()
		Me.grpPropose.ResumeLayout(False)
		CType(Me.grdPropose, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvPropose, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.grpES, System.ComponentModel.ISupportInitialize).EndInit()
		Me.grpES.ResumeLayout(False)
		CType(Me.grdES, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvES, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.ImageCollection1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents pcHeader As DevExpress.XtraEditors.PanelControl
	Friend WithEvents sccMain As DevExpress.XtraEditors.SplitContainerControl
	Friend WithEvents grdMain As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvMain As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
	Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
	Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
	Friend WithEvents bsiRecCount As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents DockManager1 As DevExpress.XtraBars.Docking.DockManager
	Friend WithEvents StyleController1 As DevExpress.XtraEditors.StyleController
	Friend WithEvents dpProperties As DevExpress.XtraBars.Docking.DockPanel
	Friend WithEvents DockPanel1_Container As DevExpress.XtraBars.Docking.ControlContainer
	Friend WithEvents sccMainNav_1 As DevExpress.XtraEditors.SplitContainerControl
	Friend WithEvents grpFunction As DevExpress.XtraEditors.GroupControl
	Friend WithEvents cmdNew As DevExpress.XtraEditors.DropDownButton
	Friend WithEvents cmdPrint As DevExpress.XtraEditors.DropDownButton
	Friend WithEvents grpPropose As DevExpress.XtraEditors.GroupControl
	Friend WithEvents grdPropose As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvPropose As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents grpES As DevExpress.XtraEditors.GroupControl
	Friend WithEvents grdES As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvES As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents sccMainProp_1 As DevExpress.XtraEditors.SplitContainerControl
	Friend WithEvents pccMandant As DevExpress.XtraBars.PopupControlContainer
  Friend WithEvents Label4 As System.Windows.Forms.Label
	Friend WithEvents sccHeaderInfo As DevExpress.XtraEditors.SplitContainerControl
  Friend WithEvents scHeaderDetail1 As DevExpress.XtraEditors.XtraScrollableControl
  Friend WithEvents grdLProperty As DevExpress.XtraGrid.GridControl
  Friend WithEvents gvLProperty As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents scHeaderDetail2 As DevExpress.XtraEditors.XtraScrollableControl
  Friend WithEvents grdJobplattforms As DevExpress.XtraGrid.GridControl
  Friend WithEvents gvJobplattforms As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents bsiMDData As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents cboMD As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents ImageCollection1 As DevExpress.Utils.ImageCollection
End Class
