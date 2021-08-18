<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class pageTODO
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
		Me.components = New System.ComponentModel.Container()
		Dim ButtonImageOptions1 As DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions = New DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(pageTODO))
		Me.grpFunction = New DevExpress.XtraEditors.GroupControl()
		Me.grdMainTodo = New DevExpress.XtraGrid.GridControl()
		Me.gvMainTodo = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.Bar1 = New DevExpress.XtraBars.Bar()
		Me.Bar2 = New DevExpress.XtraBars.Bar()
		Me.Bar3 = New DevExpress.XtraBars.Bar()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
		Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
		Me.bsiRecCount = New DevExpress.XtraBars.BarStaticItem()
		CType(Me.grpFunction, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grpFunction.SuspendLayout()
		CType(Me.grdMainTodo, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvMainTodo, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'grpFunction
		'
		Me.grpFunction.AppearanceCaption.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.grpFunction.AppearanceCaption.Options.UseFont = True
		Me.grpFunction.Controls.Add(Me.grdMainTodo)
		ButtonImageOptions1.Image = CType(resources.GetObject("ButtonImageOptions1.Image"), System.Drawing.Image)
		Me.grpFunction.CustomHeaderButtons.AddRange(New DevExpress.XtraEditors.ButtonPanel.IBaseButton() {New DevExpress.XtraEditors.ButtonsPanelControl.GroupBoxButton("", True, ButtonImageOptions1, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", -1, True, Nothing, True, False, True, Nothing, 0)})
		Me.grpFunction.CustomHeaderButtonsLocation = DevExpress.Utils.GroupElementLocation.AfterText
		Me.grpFunction.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grpFunction.Location = New System.Drawing.Point(0, 0)
		Me.grpFunction.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
		Me.grpFunction.Name = "grpFunction"
		Me.grpFunction.Size = New System.Drawing.Size(1633, 324)
		Me.grpFunction.TabIndex = 165
		Me.grpFunction.Text = "TODO für {0}"
		'
		'grdMainTodo
		'
		Me.grdMainTodo.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdMainTodo.EmbeddedNavigator.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
		Me.grdMainTodo.Location = New System.Drawing.Point(3, 37)
		Me.grdMainTodo.MainView = Me.gvMainTodo
		Me.grdMainTodo.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
		Me.grdMainTodo.Name = "grdMainTodo"
		Me.grdMainTodo.Size = New System.Drawing.Size(1627, 284)
		Me.grdMainTodo.TabIndex = 154
		Me.grdMainTodo.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvMainTodo})
		'
		'gvMainTodo
		'
		Me.gvMainTodo.DetailHeight = 619
		Me.gvMainTodo.FixedLineWidth = 4
		Me.gvMainTodo.GridControl = Me.grdMainTodo
		Me.gvMainTodo.Name = "gvMainTodo"
		Me.gvMainTodo.OptionsBehavior.Editable = False
		Me.gvMainTodo.OptionsClipboard.AllowHtmlFormat = DevExpress.Utils.DefaultBoolean.[True]
		Me.gvMainTodo.OptionsCustomization.AllowFilter = False
		Me.gvMainTodo.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvMainTodo.OptionsView.AllowHtmlDrawHeaders = True
		Me.gvMainTodo.OptionsView.ShowAutoFilterRow = True
		Me.gvMainTodo.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways
		Me.gvMainTodo.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		Me.gvMainTodo.OptionsView.ShowGroupedColumns = True
		Me.gvMainTodo.OptionsView.ShowGroupPanel = False
		Me.gvMainTodo.OptionsView.WaitAnimationOptions = DevExpress.XtraEditors.WaitAnimationOptions.Panel
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
		'BarManager1
		'
		Me.BarManager1.DockControls.Add(Me.barDockControlTop)
		Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
		Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
		Me.BarManager1.DockControls.Add(Me.barDockControlRight)
		Me.BarManager1.Form = Me
		Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiRecCount})
		Me.BarManager1.MaxItemId = 1
		'
		'barDockControlTop
		'
		Me.barDockControlTop.CausesValidation = False
		Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
		Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlTop.Manager = Me.BarManager1
		Me.barDockControlTop.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
		Me.barDockControlTop.Size = New System.Drawing.Size(1633, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 324)
		Me.barDockControlBottom.Manager = Me.BarManager1
		Me.barDockControlBottom.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
		Me.barDockControlBottom.Size = New System.Drawing.Size(1633, 0)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Manager = Me.BarManager1
		Me.barDockControlLeft.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 324)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(1633, 0)
		Me.barDockControlRight.Manager = Me.BarManager1
		Me.barDockControlRight.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 324)
		'
		'bsiRecCount
		'
		Me.bsiRecCount.Caption = "Bereit"
		Me.bsiRecCount.Id = 0
		Me.bsiRecCount.Name = "bsiRecCount"
		'
		'pageTODO
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(10.0!, 23.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.Controls.Add(Me.grpFunction)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
		Me.Name = "pageTODO"
		Me.Size = New System.Drawing.Size(1633, 324)
		CType(Me.grpFunction, System.ComponentModel.ISupportInitialize).EndInit()
		Me.grpFunction.ResumeLayout(False)
		CType(Me.grdMainTodo, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvMainTodo, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents grpFunction As DevExpress.XtraEditors.GroupControl
  Friend WithEvents Bar1 As DevExpress.XtraBars.Bar
  Friend WithEvents Bar2 As DevExpress.XtraBars.Bar
  Friend WithEvents Bar3 As DevExpress.XtraBars.Bar
  Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
  Friend WithEvents bsiRecCount As DevExpress.XtraBars.BarStaticItem
  Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
	Friend WithEvents grdMainTodo As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvMainTodo As DevExpress.XtraGrid.Views.Grid.GridView
End Class
