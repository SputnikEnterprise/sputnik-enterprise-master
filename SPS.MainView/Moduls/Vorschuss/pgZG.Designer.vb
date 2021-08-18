<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class pgVorschuss
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
		Dim ButtonImageOptions2 As DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions = New DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(pgVorschuss))
		Me.Bar3 = New DevExpress.XtraBars.Bar()
		Me.sccMain = New DevExpress.XtraEditors.SplitContainerControl()
		Me.sccHeaderInfo = New DevExpress.XtraEditors.SplitContainerControl()
		Me.grdLProperty = New DevExpress.XtraGrid.GridControl()
		Me.gvlProperty = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.scHeaderDetail1 = New DevExpress.XtraEditors.XtraScrollableControl()
		Me.grpFunction = New DevExpress.XtraEditors.GroupControl()
		Me.cmdNew = New DevExpress.XtraEditors.DropDownButton()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager()
		Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
		Me.bsiRecCount = New DevExpress.XtraBars.BarStaticItem()
		Me.bsiMDData = New DevExpress.XtraBars.BarStaticItem()
		Me.StyleController1 = New DevExpress.XtraEditors.StyleController()
		Me.cmdPrint = New DevExpress.XtraEditors.DropDownButton()
		Me.grdMain = New DevExpress.XtraGrid.GridControl()
		Me.gvMain = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.pccMandant = New DevExpress.XtraBars.PopupControlContainer()
		Me.Label4 = New System.Windows.Forms.Label()
		Me.cboMD = New DevExpress.XtraEditors.LookUpEdit()
		Me.ImageList1 = New System.Windows.Forms.ImageList()
		CType(Me.sccMain, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.sccMain.SuspendLayout()
		CType(Me.sccHeaderInfo, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.sccHeaderInfo.SuspendLayout()
		CType(Me.grdLProperty, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvlProperty, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.grpFunction, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grpFunction.SuspendLayout()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.StyleController1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.grdMain, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvMain, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.pccMandant, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.pccMandant.SuspendLayout()
		CType(Me.cboMD.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
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
		Me.sccMain.Panel1.Text = "Panel1"
		Me.sccMain.Panel2.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
		Me.sccMain.Panel2.Appearance.Options.UseBackColor = True
		Me.sccMain.Panel2.Controls.Add(Me.grdMain)
		Me.sccMain.Panel2.Controls.Add(Me.pccMandant)
		Me.sccMain.Panel2.Text = "Panel2"
		Me.sccMain.Size = New System.Drawing.Size(865, 690)
		Me.sccMain.SplitterPosition = 77
		Me.sccMain.TabIndex = 158
		'
		'sccHeaderInfo
		'
		Me.sccHeaderInfo.AppearanceCaption.Image = Global.SPS.MainView.My.Resources.Resources.more_arrow
		Me.sccHeaderInfo.AppearanceCaption.Options.UseImage = True
		Me.sccHeaderInfo.CaptionImageOptions.Image = Global.SPS.MainView.My.Resources.Resources.more_arrow
		Me.sccHeaderInfo.Dock = System.Windows.Forms.DockStyle.Fill
		Me.sccHeaderInfo.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.None
		Me.sccHeaderInfo.IsSplitterFixed = True
		Me.sccHeaderInfo.Location = New System.Drawing.Point(0, 0)
		Me.sccHeaderInfo.Name = "sccHeaderInfo"
		Me.sccHeaderInfo.Padding = New System.Windows.Forms.Padding(1)
		Me.sccHeaderInfo.Panel1.Controls.Add(Me.grdLProperty)
		Me.sccHeaderInfo.Panel1.Controls.Add(Me.scHeaderDetail1)
		Me.sccHeaderInfo.Panel1.Padding = New System.Windows.Forms.Padding(5)
		Me.sccHeaderInfo.Panel1.Text = "pMABild"
		Me.sccHeaderInfo.Panel2.Controls.Add(Me.grpFunction)
		Me.sccHeaderInfo.Panel2.MinSize = 100
		Me.sccHeaderInfo.Panel2.Padding = New System.Windows.Forms.Padding(5)
		Me.sccHeaderInfo.Panel2.Text = "pMADetail"
		Me.sccHeaderInfo.Size = New System.Drawing.Size(861, 77)
		Me.sccHeaderInfo.SplitterPosition = 586
		Me.sccHeaderInfo.TabIndex = 9
		Me.sccHeaderInfo.Text = "SplitContainerControl2"
		'
		'grdLProperty
		'
		Me.grdLProperty.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdLProperty.Location = New System.Drawing.Point(5, 5)
		Me.grdLProperty.MainView = Me.gvlProperty
		Me.grdLProperty.Name = "grdLProperty"
		Me.grdLProperty.Size = New System.Drawing.Size(576, 65)
		Me.grdLProperty.TabIndex = 6
		Me.grdLProperty.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvlProperty})
		'
		'gvlProperty
		'
		Me.gvlProperty.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvlProperty.GridControl = Me.grdLProperty
		Me.gvlProperty.Name = "gvlProperty"
		Me.gvlProperty.OptionsBehavior.Editable = False
		Me.gvlProperty.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvlProperty.OptionsView.ShowColumnHeaders = False
		Me.gvlProperty.OptionsView.ShowGroupPanel = False
		'
		'scHeaderDetail1
		'
		Me.scHeaderDetail1.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.scHeaderDetail1.Appearance.Options.UseBackColor = True
		Me.scHeaderDetail1.Location = New System.Drawing.Point(21, 2)
		Me.scHeaderDetail1.Name = "scHeaderDetail1"
		Me.scHeaderDetail1.Size = New System.Drawing.Size(286, 50)
		Me.scHeaderDetail1.TabIndex = 3
		'
		'grpFunction
		'
		Me.grpFunction.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.grpFunction.AppearanceCaption.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.grpFunction.AppearanceCaption.Options.UseFont = True
		Me.grpFunction.Controls.Add(Me.cmdNew)
		Me.grpFunction.Controls.Add(Me.cmdPrint)
		ButtonImageOptions1.Image = CType(resources.GetObject("ButtonImageOptions1.Image"), System.Drawing.Image)
		ButtonImageOptions2.Image = CType(resources.GetObject("ButtonImageOptions2.Image"), System.Drawing.Image)
		Me.grpFunction.CustomHeaderButtons.AddRange(New DevExpress.XtraEditors.ButtonPanel.IBaseButton() {New DevExpress.XtraEditors.ButtonsPanelControl.GroupBoxButton("", True, ButtonImageOptions1, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", -1, True, Nothing, True, False, True, Nothing, 0), New DevExpress.XtraEditors.ButtonsPanelControl.GroupBoxButton("", True, ButtonImageOptions2, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", -1, True, Nothing, True, False, True, Nothing, 1)})
		Me.grpFunction.CustomHeaderButtonsLocation = DevExpress.Utils.GroupElementLocation.AfterText
		Me.grpFunction.Location = New System.Drawing.Point(8, 5)
		Me.grpFunction.MaximumSize = New System.Drawing.Size(251, 59)
		Me.grpFunction.MinimumSize = New System.Drawing.Size(251, 59)
		Me.grpFunction.Name = "grpFunction"
		Me.grpFunction.Size = New System.Drawing.Size(251, 59)
		Me.grpFunction.TabIndex = 162
		Me.grpFunction.Text = "Funktionen"
		'
		'cmdNew
		'
		Me.cmdNew.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.cmdNew.ImageOptions.Image = CType(resources.GetObject("cmdNew.ImageOptions.Image"), System.Drawing.Image)
		Me.cmdNew.Location = New System.Drawing.Point(109, 32)
		Me.cmdNew.MenuManager = Me.BarManager1
		Me.cmdNew.Name = "cmdNew"
		Me.cmdNew.Size = New System.Drawing.Size(97, 22)
		Me.cmdNew.StyleController = Me.StyleController1
		Me.cmdNew.TabIndex = 3
		Me.cmdNew.Text = "Neu"
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
		Me.barDockControlTop.Size = New System.Drawing.Size(865, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 690)
		Me.barDockControlBottom.Manager = Me.BarManager1
		Me.barDockControlBottom.Size = New System.Drawing.Size(865, 0)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Manager = Me.BarManager1
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 690)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(865, 0)
		Me.barDockControlRight.Manager = Me.BarManager1
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 690)
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
		'cmdPrint
		'
		Me.cmdPrint.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.cmdPrint.ImageOptions.Image = CType(resources.GetObject("cmdPrint.ImageOptions.Image"), System.Drawing.Image)
		Me.cmdPrint.Location = New System.Drawing.Point(6, 32)
		Me.cmdPrint.MenuManager = Me.BarManager1
		Me.cmdPrint.Name = "cmdPrint"
		Me.cmdPrint.Size = New System.Drawing.Size(97, 22)
		Me.cmdPrint.StyleController = Me.StyleController1
		Me.cmdPrint.TabIndex = 1
		Me.cmdPrint.Text = "Drucken"
		'
		'grdMain
		'
		Me.grdMain.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdMain.Location = New System.Drawing.Point(0, 0)
		Me.grdMain.MainView = Me.gvMain
		Me.grdMain.Name = "grdMain"
		Me.grdMain.Size = New System.Drawing.Size(861, 604)
		Me.grdMain.TabIndex = 163
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
		'pccMandant
		'
		Me.pccMandant.Appearance.BackColor = System.Drawing.Color.White
		Me.pccMandant.Appearance.Options.UseBackColor = True
		Me.pccMandant.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
		Me.pccMandant.Controls.Add(Me.Label4)
		Me.pccMandant.Controls.Add(Me.cboMD)
		Me.pccMandant.Location = New System.Drawing.Point(129, 213)
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
		Me.cboMD.MenuManager = Me.BarManager1
		Me.cboMD.Name = "cboMD"
		Me.cboMD.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.cboMD.Properties.NullText = ""
		Me.cboMD.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
		Me.cboMD.Size = New System.Drawing.Size(211, 20)
		Me.cboMD.TabIndex = 21
		'
		'ImageList1
		'
		Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
		Me.ImageList1.Images.SetKeyName(0, "eye-icon.png")
		'
		'pgVorschuss
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.Controls.Add(Me.sccMain)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.Name = "pgVorschuss"
		Me.Size = New System.Drawing.Size(865, 690)
		CType(Me.sccMain, System.ComponentModel.ISupportInitialize).EndInit()
		Me.sccMain.ResumeLayout(False)
		CType(Me.sccHeaderInfo, System.ComponentModel.ISupportInitialize).EndInit()
		Me.sccHeaderInfo.ResumeLayout(False)
		CType(Me.grdLProperty, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvlProperty, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.grpFunction, System.ComponentModel.ISupportInitialize).EndInit()
		Me.grpFunction.ResumeLayout(False)
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.StyleController1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.grdMain, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvMain, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.pccMandant, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pccMandant.ResumeLayout(False)
		CType(Me.cboMD.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents Bar3 As DevExpress.XtraBars.Bar
	Friend WithEvents sccMain As DevExpress.XtraEditors.SplitContainerControl
	Friend WithEvents sccHeaderInfo As DevExpress.XtraEditors.SplitContainerControl
	Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
	Friend WithEvents bsiRecCount As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
	Friend WithEvents StyleController1 As DevExpress.XtraEditors.StyleController
	Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
	Friend WithEvents pccMandant As DevExpress.XtraBars.PopupControlContainer
	Friend WithEvents Label4 As System.Windows.Forms.Label
	Friend WithEvents scHeaderDetail1 As DevExpress.XtraEditors.XtraScrollableControl
	Friend WithEvents grpFunction As DevExpress.XtraEditors.GroupControl
	Friend WithEvents cmdNew As DevExpress.XtraEditors.DropDownButton
	Friend WithEvents cmdPrint As DevExpress.XtraEditors.DropDownButton
	Friend WithEvents grdLProperty As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvlProperty As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents cboMD As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents bsiMDData As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents grdMain As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvMain As DevExpress.XtraGrid.Views.Grid.GridView
End Class
