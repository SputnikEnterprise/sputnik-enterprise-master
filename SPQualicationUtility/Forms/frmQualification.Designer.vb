<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmQualification
	Inherits DevExpress.XtraEditors.XtraForm

	'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
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
	<System.Diagnostics.DebuggerStepThrough()>
	Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmQualification))
		Me.LinkLabel4 = New System.Windows.Forms.LinkLabel()
		Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
		Me.grdQualifikation = New DevExpress.XtraGrid.GridControl()
		Me.gvQualifikation = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
		Me.cmdClose = New DevExpress.XtraEditors.SimpleButton()
		Me.LblSetting = New System.Windows.Forms.Label()
		Me.lblHeaderNormal = New System.Windows.Forms.Label()
		Me.lblHeaderFett = New System.Windows.Forms.Label()
		Me.Bar3 = New DevExpress.XtraBars.Bar()
		Me.Bar1 = New DevExpress.XtraBars.Bar()
		Me.Bar2 = New DevExpress.XtraBars.Bar()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
		Me.Bar6 = New DevExpress.XtraBars.Bar()
		Me.bsiNameInfo = New DevExpress.XtraBars.BarStaticItem()
		Me.bsiCountInfo = New DevExpress.XtraBars.BarStaticItem()
		Me.BarAndDockingController1 = New DevExpress.XtraBars.BarAndDockingController(Me.components)
		Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
		Me.cmdOK = New DevExpress.XtraEditors.SimpleButton()
		Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton()
		Me.btnOK = New DevExpress.XtraEditors.SimpleButton()
		CType(Me.grdQualifikation, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvQualifikation, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.PanelControl1.SuspendLayout()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.BarAndDockingController1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'LinkLabel4
		'
		Me.LinkLabel4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.LinkLabel4.Location = New System.Drawing.Point(12, 110)
		Me.LinkLabel4.Name = "LinkLabel4"
		Me.LinkLabel4.Size = New System.Drawing.Size(167, 13)
		Me.LinkLabel4.TabIndex = 156
		Me.LinkLabel4.TabStop = True
		Me.LinkLabel4.Text = "Seco Berufsdatenbanken..."
		'
		'LinkLabel1
		'
		Me.LinkLabel1.AutoSize = True
		Me.LinkLabel1.Location = New System.Drawing.Point(12, 96)
		Me.LinkLabel1.Name = "LinkLabel1"
		Me.LinkLabel1.Size = New System.Drawing.Size(122, 13)
		Me.LinkLabel1.TabIndex = 153
		Me.LinkLabel1.TabStop = True
		Me.LinkLabel1.Text = "Lokale Berufsdatenbank"
		'
		'grdQualifikation
		'
		Me.grdQualifikation.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.grdQualifikation.Location = New System.Drawing.Point(12, 133)
		Me.grdQualifikation.MainView = Me.gvQualifikation
		Me.grdQualifikation.Name = "grdQualifikation"
		Me.grdQualifikation.Size = New System.Drawing.Size(617, 521)
		Me.grdQualifikation.TabIndex = 152
		Me.grdQualifikation.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvQualifikation})
		'
		'gvQualifikation
		'
		Me.gvQualifikation.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvQualifikation.GridControl = Me.grdQualifikation
		Me.gvQualifikation.Name = "gvQualifikation"
		Me.gvQualifikation.OptionsBehavior.Editable = False
		Me.gvQualifikation.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvQualifikation.OptionsView.ShowGroupPanel = False
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
		Me.PanelControl1.Size = New System.Drawing.Size(647, 82)
		Me.PanelControl1.TabIndex = 168
		'
		'cmdClose
		'
		Me.cmdClose.Anchor = System.Windows.Forms.AnchorStyles.Right
		Me.cmdClose.Location = New System.Drawing.Point(532, 20)
		Me.cmdClose.Name = "cmdClose"
		Me.cmdClose.Size = New System.Drawing.Size(97, 25)
		Me.cmdClose.TabIndex = 127
		Me.cmdClose.Text = "Schliessen"
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
		'lblHeaderNormal
		'
		Me.lblHeaderNormal.AutoSize = True
		Me.lblHeaderNormal.BackColor = System.Drawing.Color.Transparent
		Me.lblHeaderNormal.Location = New System.Drawing.Point(107, 47)
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
		Me.lblHeaderFett.Location = New System.Drawing.Point(89, 23)
		Me.lblHeaderFett.Name = "lblHeaderFett"
		Me.lblHeaderFett.Size = New System.Drawing.Size(260, 13)
		Me.lblHeaderFett.TabIndex = 122
		Me.lblHeaderFett.Text = "Liste der Qualifikationen (Remote und Lokal)"
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
		'BarManager1
		'
		Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar6})
		Me.BarManager1.Controller = Me.BarAndDockingController1
		Me.BarManager1.DockControls.Add(Me.barDockControlTop)
		Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
		Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
		Me.BarManager1.DockControls.Add(Me.barDockControlRight)
		Me.BarManager1.Form = Me
		Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiNameInfo, Me.bsiCountInfo})
		Me.BarManager1.MaxItemId = 2
		Me.BarManager1.StatusBar = Me.Bar6
		'
		'Bar6
		'
		Me.Bar6.BarName = "Statusleiste"
		Me.Bar6.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
		Me.Bar6.DockCol = 0
		Me.Bar6.DockRow = 0
		Me.Bar6.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
		Me.Bar6.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiNameInfo), New DevExpress.XtraBars.LinkPersistInfo(Me.bsiCountInfo)})
		Me.Bar6.OptionsBar.AllowQuickCustomization = False
		Me.Bar6.OptionsBar.DrawDragBorder = False
		Me.Bar6.OptionsBar.UseWholeRow = True
		Me.Bar6.Text = "Statusleiste"
		'
		'bsiNameInfo
		'
		Me.bsiNameInfo.Caption = "BarStaticItem1"
		Me.bsiNameInfo.Id = 0
		Me.bsiNameInfo.Name = "bsiNameInfo"
		'
		'bsiCountInfo
		'
		Me.bsiCountInfo.Caption = "BarStaticItem2"
		Me.bsiCountInfo.Id = 1
		Me.bsiCountInfo.Name = "bsiCountInfo"
		'
		'BarAndDockingController1
		'
		Me.BarAndDockingController1.PropertiesBar.AllowLinkLighting = False
		'
		'barDockControlTop
		'
		Me.barDockControlTop.CausesValidation = False
		Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
		Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlTop.Manager = Me.BarManager1
		Me.barDockControlTop.Size = New System.Drawing.Size(647, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 678)
		Me.barDockControlBottom.Manager = Me.BarManager1
		Me.barDockControlBottom.Size = New System.Drawing.Size(647, 22)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Manager = Me.BarManager1
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 678)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(647, 0)
		Me.barDockControlRight.Manager = Me.BarManager1
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 678)
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
		'SimpleButton1
		'
		Me.SimpleButton1.Anchor = System.Windows.Forms.AnchorStyles.Right
		Me.SimpleButton1.Location = New System.Drawing.Point(519, 20)
		Me.SimpleButton1.Name = "SimpleButton1"
		Me.SimpleButton1.Size = New System.Drawing.Size(97, 25)
		Me.SimpleButton1.TabIndex = 127
		Me.SimpleButton1.Text = "Schliessen"
		'
		'btnOK
		'
		Me.btnOK.Location = New System.Drawing.Point(468, 91)
		Me.btnOK.Name = "btnOK"
		Me.btnOK.Size = New System.Drawing.Size(148, 25)
		Me.btnOK.TabIndex = 178
		Me.btnOK.Text = "Auswahl übernehmen"
		'
		'frmQualification
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(647, 700)
		Me.Controls.Add(Me.grdQualifikation)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.LinkLabel4)
		Me.Controls.Add(Me.PanelControl1)
		Me.Controls.Add(Me.LinkLabel1)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.IconOptions.Icon = CType(resources.GetObject("frmQualification.IconOptions.Icon"), System.Drawing.Icon)
		Me.MinimumSize = New System.Drawing.Size(649, 732)
		Me.Name = "frmQualification"
		Me.Text = "Suche nach Qualifikationen"
		CType(Me.grdQualifikation, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvQualifikation, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.PanelControl1.ResumeLayout(False)
		Me.PanelControl1.PerformLayout()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.BarAndDockingController1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents LinkLabel4 As System.Windows.Forms.LinkLabel
	Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
	Friend WithEvents grdQualifikation As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvQualifikation As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
	Friend WithEvents LblSetting As System.Windows.Forms.Label
	Friend WithEvents lblHeaderNormal As System.Windows.Forms.Label
	Friend WithEvents lblHeaderFett As System.Windows.Forms.Label
	Friend WithEvents Bar3 As DevExpress.XtraBars.Bar
	Friend WithEvents Bar1 As DevExpress.XtraBars.Bar
	Friend WithEvents Bar2 As DevExpress.XtraBars.Bar
	Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
	Friend WithEvents Bar6 As DevExpress.XtraBars.Bar
	Friend WithEvents bsiNameInfo As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
	Friend WithEvents bsiCountInfo As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents cmdClose As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents cmdOK As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents btnOK As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents BarAndDockingController1 As DevExpress.XtraBars.BarAndDockingController
End Class
