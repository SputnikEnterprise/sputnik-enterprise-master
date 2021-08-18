<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBranches
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
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBranches))
		Me.LblSetting = New System.Windows.Forms.Label()
		Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
		Me.cmdClose = New DevExpress.XtraEditors.SimpleButton()
		Me.lblHeaderNormal = New System.Windows.Forms.Label()
		Me.lblHeaderFett = New System.Windows.Forms.Label()
		Me.grdBranches = New DevExpress.XtraGrid.GridControl()
		Me.gvBranches = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
		Me.Bar3 = New DevExpress.XtraBars.Bar()
		Me.bsiNameInfo = New DevExpress.XtraBars.BarStaticItem()
		Me.bsiCountInfo = New DevExpress.XtraBars.BarStaticItem()
		Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
		Me.cmdOK = New DevExpress.XtraEditors.SimpleButton()
		CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.PanelControl1.SuspendLayout()
		CType(Me.grdBranches, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvBranches, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'LblSetting
		'
		Me.LblSetting.BackColor = System.Drawing.Color.Transparent
		Me.LblSetting.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.LblSetting.ForeColor = System.Drawing.SystemColors.HotTrack
		Me.LblSetting.Image = CType(resources.GetObject("LblSetting.Image"), System.Drawing.Image)
		Me.LblSetting.Location = New System.Drawing.Point(14, 8)
		Me.LblSetting.Name = "LblSetting"
		Me.LblSetting.Size = New System.Drawing.Size(67, 71)
		Me.LblSetting.TabIndex = 126
		'
		'PanelControl1
		'
		Me.PanelControl1.Appearance.BackColor = System.Drawing.Color.White
		Me.PanelControl1.Appearance.Options.UseBackColor = True
		Me.PanelControl1.Controls.Add(Me.cmdClose)
		Me.PanelControl1.Controls.Add(Me.LblSetting)
		Me.PanelControl1.Controls.Add(Me.lblHeaderNormal)
		Me.PanelControl1.Controls.Add(Me.lblHeaderFett)
		Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Top
		Me.PanelControl1.Location = New System.Drawing.Point(0, 0)
		Me.PanelControl1.Name = "PanelControl1"
		Me.PanelControl1.Size = New System.Drawing.Size(648, 82)
		Me.PanelControl1.TabIndex = 173
		'
		'cmdClose
		'
		Me.cmdClose.Anchor = System.Windows.Forms.AnchorStyles.Right
		Me.cmdClose.Location = New System.Drawing.Point(529, 21)
		Me.cmdClose.Name = "cmdClose"
		Me.cmdClose.Size = New System.Drawing.Size(100, 27)
		Me.cmdClose.TabIndex = 179
		Me.cmdClose.Text = "Schliessen"
		'
		'lblHeaderNormal
		'
		Me.lblHeaderNormal.AutoSize = True
		Me.lblHeaderNormal.BackColor = System.Drawing.Color.Transparent
		Me.lblHeaderNormal.Location = New System.Drawing.Point(105, 45)
		Me.lblHeaderNormal.Name = "lblHeaderNormal"
		Me.lblHeaderNormal.Size = New System.Drawing.Size(225, 13)
		Me.lblHeaderNormal.TabIndex = 123
		Me.lblHeaderNormal.Text = "Wählen Sie Ihre gewünschten Kriterien aus..."
		'
		'lblHeaderFett
		'
		Me.lblHeaderFett.AutoSize = True
		Me.lblHeaderFett.BackColor = System.Drawing.Color.Transparent
		Me.lblHeaderFett.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblHeaderFett.Location = New System.Drawing.Point(87, 21)
		Me.lblHeaderFett.Name = "lblHeaderFett"
		Me.lblHeaderFett.Size = New System.Drawing.Size(114, 13)
		Me.lblHeaderFett.TabIndex = 122
		Me.lblHeaderFett.Text = "Liste der Branchen"
		'
		'grdBranches
		'
		Me.grdBranches.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.grdBranches.Location = New System.Drawing.Point(12, 132)
		Me.grdBranches.MainView = Me.gvBranches
		Me.grdBranches.Name = "grdBranches"
		Me.grdBranches.Size = New System.Drawing.Size(617, 501)
		Me.grdBranches.TabIndex = 169
		Me.grdBranches.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvBranches})
		'
		'gvBranches
		'
		Me.gvBranches.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvBranches.GridControl = Me.grdBranches
		Me.gvBranches.Name = "gvBranches"
		Me.gvBranches.OptionsBehavior.Editable = False
		Me.gvBranches.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvBranches.OptionsView.ShowGroupPanel = False
		'
		'BarManager1
		'
		Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar3})
		Me.BarManager1.DockControls.Add(Me.barDockControlTop)
		Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
		Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
		Me.BarManager1.DockControls.Add(Me.barDockControlRight)
		Me.BarManager1.Form = Me
		Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiNameInfo, Me.bsiCountInfo})
		Me.BarManager1.MaxItemId = 2
		Me.BarManager1.StatusBar = Me.Bar3
		'
		'Bar3
		'
		Me.Bar3.BarName = "Statusleiste"
		Me.Bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
		Me.Bar3.DockCol = 0
		Me.Bar3.DockRow = 0
		Me.Bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
		Me.Bar3.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiNameInfo), New DevExpress.XtraBars.LinkPersistInfo(Me.bsiCountInfo)})
		Me.Bar3.OptionsBar.AllowQuickCustomization = False
		Me.Bar3.OptionsBar.DrawDragBorder = False
		Me.Bar3.OptionsBar.UseWholeRow = True
		Me.Bar3.Text = "Statusleiste"
		'
		'bsiNameInfo
		'
		Me.bsiNameInfo.Caption = "BarStaticItem1"
		Me.bsiNameInfo.Id = 0
		Me.bsiNameInfo.Name = "bsiNameInfo"
		'
		'bsiCountInfo
		'
		Me.bsiCountInfo.Caption = "BarStaticItem1"
		Me.bsiCountInfo.Id = 1
		Me.bsiCountInfo.Name = "bsiCountInfo"
		'
		'barDockControlTop
		'
		Me.barDockControlTop.CausesValidation = False
		Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
		Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlTop.Manager = Me.BarManager1
		Me.barDockControlTop.Size = New System.Drawing.Size(648, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 653)
		Me.barDockControlBottom.Manager = Me.BarManager1
		Me.barDockControlBottom.Size = New System.Drawing.Size(648, 22)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Manager = Me.BarManager1
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 653)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(648, 0)
		Me.barDockControlRight.Manager = Me.BarManager1
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 653)
		'
		'cmdOK
		'
		Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.cmdOK.Location = New System.Drawing.Point(481, 96)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.Size = New System.Drawing.Size(148, 25)
		Me.cmdOK.TabIndex = 178
		Me.cmdOK.Text = "Auswahl übernehmen"
		'
		'frmBranches
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(648, 675)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.PanelControl1)
		Me.Controls.Add(Me.grdBranches)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.IconOptions.Icon = CType(resources.GetObject("frmBranches.IconOptions.Icon"), System.Drawing.Icon)
		Me.MinimumSize = New System.Drawing.Size(650, 707)
		Me.Name = "frmBranches"
		Me.Text = "Suche nach Branchen"
		CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.PanelControl1.ResumeLayout(False)
		Me.PanelControl1.PerformLayout()
		CType(Me.grdBranches, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvBranches, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents LblSetting As System.Windows.Forms.Label
  Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
  Friend WithEvents lblHeaderNormal As System.Windows.Forms.Label
  Friend WithEvents lblHeaderFett As System.Windows.Forms.Label
  Friend WithEvents grdBranches As DevExpress.XtraGrid.GridControl
  Friend WithEvents gvBranches As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
  Friend WithEvents Bar3 As DevExpress.XtraBars.Bar
  Friend WithEvents bsiNameInfo As DevExpress.XtraBars.BarStaticItem
  Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
  Friend WithEvents cmdClose As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents cmdOK As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents bsiCountInfo As DevExpress.XtraBars.BarStaticItem
End Class
