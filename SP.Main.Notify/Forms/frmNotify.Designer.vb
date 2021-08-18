Imports DevExpress.XtraEditors
Imports DevExpress.Skins
Imports DevExpress.LookAndFeel
Imports DevExpress.UserSkins

Namespace UI


	<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
	Partial Class frmNotify
		Inherits XtraForm

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
			Dim ButtonImageOptions1 As DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions = New DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions()
			Dim ButtonImageOptions2 As DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions = New DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions()
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
			Dim GridLevelNode1 As DevExpress.XtraGrid.GridLevelNode = New DevExpress.XtraGrid.GridLevelNode()
			Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmNotify))
			Me.splitContainerControl = New DevExpress.XtraEditors.SplitContainerControl()
			Me.grpSearch = New DevExpress.XtraEditors.GroupControl()
			Me.LabelControl8 = New DevExpress.XtraEditors.LabelControl()
			Me.btnCheckVacancies = New DevExpress.XtraEditors.SimpleButton()
			Me.btnUpdateGeoData = New DevExpress.XtraEditors.SimpleButton()
			Me.btnUpdateCountryData = New DevExpress.XtraEditors.SimpleButton()
			Me.LabelControl5 = New DevExpress.XtraEditors.LabelControl()
			Me.txtFileServer = New DevExpress.XtraEditors.TextEdit()
			Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
			Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
			Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
			Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
			Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
			Me.chkExcludeChecked = New DevExpress.XtraEditors.CheckEdit()
			Me.lueMandant = New DevExpress.XtraEditors.LookUpEdit()
			Me.btnClose = New DevExpress.XtraEditors.SimpleButton()
			Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl()
			Me.lueAdvisor = New DevExpress.XtraEditors.LookUpEdit()
			Me.lblBeraterIn = New DevExpress.XtraEditors.LabelControl()
			Me.txt_MDGuidForVacancyCheck = New DevExpress.XtraEditors.ComboBoxEdit()
			Me.XtraTabControl1 = New DevExpress.XtraTab.XtraTabControl()
			Me.xtabNotifying = New DevExpress.XtraTab.XtraTabPage()
			Me.grdNotification = New DevExpress.XtraGrid.GridControl()
			Me.gvNotification = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
			Me.reNotifyComments = New DevExpress.XtraRichEdit.RichEditControl()
			Me.lblNotifyHeader = New DevExpress.XtraEditors.LabelControl()
			Me.lblCheckedOn = New DevExpress.XtraEditors.LabelControl()
			Me.LabelControl7 = New DevExpress.XtraEditors.LabelControl()
			Me.lblCreatedOn = New DevExpress.XtraEditors.LabelControl()
			Me.LabelControl6 = New DevExpress.XtraEditors.LabelControl()
			Me.lblNotifyArt = New DevExpress.XtraEditors.LabelControl()
			Me.LabelControl4 = New DevExpress.XtraEditors.LabelControl()
			Me.LabelControl3 = New DevExpress.XtraEditors.LabelControl()
			Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl()
			Me.xtabMasterMandant = New DevExpress.XtraTab.XtraTabPage()
			Me.grdMasterMandant = New DevExpress.XtraGrid.GridControl()
			Me.gvMasterMandant = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.GridView2 = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.xtabDocScan = New DevExpress.XtraTab.XtraTabPage()
			Me.grdScanJobs = New DevExpress.XtraGrid.GridControl()
			Me.gvScanJobs = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.GridView3 = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.xtabApplicant = New DevExpress.XtraTab.XtraTabPage()
			Me.grdApplicant = New DevExpress.XtraGrid.GridControl()
			Me.gvApplicant = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.GridView4 = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.navbarImageCollectionLarge = New DevExpress.Utils.ImageCollection(Me.components)
			Me.navbarImageCollection = New DevExpress.Utils.ImageCollection(Me.components)
			Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
			Me.AlertControl1 = New DevExpress.XtraBars.Alerter.AlertControl(Me.components)
			CType(Me.splitContainerControl, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.splitContainerControl.SuspendLayout()
			CType(Me.grpSearch, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.grpSearch.SuspendLayout()
			CType(Me.txtFileServer.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.chkExcludeChecked.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.lueAdvisor.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.txt_MDGuidForVacancyCheck.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.XtraTabControl1.SuspendLayout()
			Me.xtabNotifying.SuspendLayout()
			CType(Me.grdNotification, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvNotification, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.PanelControl1.SuspendLayout()
			Me.xtabMasterMandant.SuspendLayout()
			CType(Me.grdMasterMandant, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvMasterMandant, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.GridView2, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.xtabDocScan.SuspendLayout()
			CType(Me.grdScanJobs, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvScanJobs, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.GridView3, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.xtabApplicant.SuspendLayout()
			CType(Me.grdApplicant, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvApplicant, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.GridView4, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.navbarImageCollectionLarge, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.navbarImageCollection, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			'
			'splitContainerControl
			'
			Me.splitContainerControl.Dock = System.Windows.Forms.DockStyle.Fill
			Me.splitContainerControl.Horizontal = False
			Me.splitContainerControl.Location = New System.Drawing.Point(0, 0)
			Me.splitContainerControl.Name = "splitContainerControl"
			Me.splitContainerControl.Padding = New System.Windows.Forms.Padding(6)
			Me.splitContainerControl.Panel1.Controls.Add(Me.grpSearch)
			Me.splitContainerControl.Panel1.Padding = New System.Windows.Forms.Padding(5)
			Me.splitContainerControl.Panel1.Text = "Panel1"
			Me.splitContainerControl.Panel2.Controls.Add(Me.XtraTabControl1)
			Me.splitContainerControl.Panel2.Padding = New System.Windows.Forms.Padding(5)
			Me.splitContainerControl.Panel2.Text = "Panel2"
			Me.splitContainerControl.Size = New System.Drawing.Size(903, 652)
			Me.splitContainerControl.SplitterPosition = 198
			Me.splitContainerControl.TabIndex = 1
			Me.splitContainerControl.Text = "splitContainerControl1"
			'
			'grpSearch
			'
			Me.grpSearch.Controls.Add(Me.LabelControl8)
			Me.grpSearch.Controls.Add(Me.btnCheckVacancies)
			Me.grpSearch.Controls.Add(Me.btnUpdateGeoData)
			Me.grpSearch.Controls.Add(Me.btnUpdateCountryData)
			Me.grpSearch.Controls.Add(Me.LabelControl5)
			Me.grpSearch.Controls.Add(Me.txtFileServer)
			Me.grpSearch.Controls.Add(Me.chkExcludeChecked)
			Me.grpSearch.Controls.Add(Me.lueMandant)
			Me.grpSearch.Controls.Add(Me.btnClose)
			Me.grpSearch.Controls.Add(Me.LabelControl1)
			Me.grpSearch.Controls.Add(Me.lueAdvisor)
			Me.grpSearch.Controls.Add(Me.lblBeraterIn)
			Me.grpSearch.Controls.Add(Me.txt_MDGuidForVacancyCheck)
			ButtonImageOptions1.Image = CType(resources.GetObject("ButtonImageOptions1.Image"), System.Drawing.Image)
			ButtonImageOptions2.Image = CType(resources.GetObject("ButtonImageOptions2.Image"), System.Drawing.Image)
			Me.grpSearch.CustomHeaderButtons.AddRange(New DevExpress.XtraEditors.ButtonPanel.IBaseButton() {New DevExpress.XtraEditors.ButtonsPanelControl.GroupBoxButton("", True, ButtonImageOptions1, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", -1, True, Nothing, True, False, True, Nothing, 0), New DevExpress.XtraEditors.ButtonsPanelControl.GroupBoxButton("", True, ButtonImageOptions2, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", -1, True, Nothing, True, False, True, Nothing, 1)})
			Me.grpSearch.CustomHeaderButtonsLocation = DevExpress.Utils.GroupElementLocation.AfterText
			Me.grpSearch.Dock = System.Windows.Forms.DockStyle.Fill
			Me.grpSearch.Location = New System.Drawing.Point(5, 5)
			Me.grpSearch.Name = "grpSearch"
			Me.grpSearch.Size = New System.Drawing.Size(881, 188)
			Me.grpSearch.TabIndex = 3
			Me.grpSearch.Text = "Suchen nach Daten"
			'
			'LabelControl8
			'
			Me.LabelControl8.Appearance.Options.UseTextOptions = True
			Me.LabelControl8.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.LabelControl8.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.LabelControl8.Location = New System.Drawing.Point(18, 140)
			Me.LabelControl8.Name = "LabelControl8"
			Me.LabelControl8.Size = New System.Drawing.Size(114, 13)
			Me.LabelControl8.TabIndex = 335
			Me.LabelControl8.Text = "MD-Guid"
			'
			'btnCheckVacancies
			'
			Me.btnCheckVacancies.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnCheckVacancies.Location = New System.Drawing.Point(355, 137)
			Me.btnCheckVacancies.Name = "btnCheckVacancies"
			Me.btnCheckVacancies.Size = New System.Drawing.Size(103, 31)
			Me.btnCheckVacancies.TabIndex = 334
			Me.btnCheckVacancies.Text = "Update Vacancies"
			'
			'btnUpdateGeoData
			'
			Me.btnUpdateGeoData.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnUpdateGeoData.Location = New System.Drawing.Point(640, 19)
			Me.btnUpdateGeoData.Name = "btnUpdateGeoData"
			Me.btnUpdateGeoData.Size = New System.Drawing.Size(103, 31)
			Me.btnUpdateGeoData.TabIndex = 332
			Me.btnUpdateGeoData.Text = "Update geo data"
			'
			'btnUpdateCountryData
			'
			Me.btnUpdateCountryData.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnUpdateCountryData.Location = New System.Drawing.Point(768, 105)
			Me.btnUpdateCountryData.Name = "btnUpdateCountryData"
			Me.btnUpdateCountryData.Size = New System.Drawing.Size(103, 31)
			Me.btnUpdateCountryData.TabIndex = 331
			Me.btnUpdateCountryData.Text = "Update data"
			'
			'LabelControl5
			'
			Me.LabelControl5.Appearance.Options.UseTextOptions = True
			Me.LabelControl5.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.LabelControl5.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.LabelControl5.Location = New System.Drawing.Point(18, 114)
			Me.LabelControl5.Name = "LabelControl5"
			Me.LabelControl5.Size = New System.Drawing.Size(114, 13)
			Me.LabelControl5.TabIndex = 330
			Me.LabelControl5.Text = "FileServerPath"
			'
			'txtFileServer
			'
			Me.txtFileServer.Location = New System.Drawing.Point(138, 111)
			Me.txtFileServer.MenuManager = Me.BarManager1
			Me.txtFileServer.Name = "txtFileServer"
			Me.txtFileServer.Size = New System.Drawing.Size(320, 20)
			Me.txtFileServer.TabIndex = 328
			'
			'BarManager1
			'
			Me.BarManager1.DockControls.Add(Me.barDockControlTop)
			Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
			Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
			Me.BarManager1.DockControls.Add(Me.barDockControlRight)
			Me.BarManager1.Form = Me
			'
			'barDockControlTop
			'
			Me.barDockControlTop.CausesValidation = False
			Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
			Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
			Me.barDockControlTop.Manager = Me.BarManager1
			Me.barDockControlTop.Size = New System.Drawing.Size(903, 0)
			'
			'barDockControlBottom
			'
			Me.barDockControlBottom.CausesValidation = False
			Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
			Me.barDockControlBottom.Location = New System.Drawing.Point(0, 652)
			Me.barDockControlBottom.Manager = Me.BarManager1
			Me.barDockControlBottom.Size = New System.Drawing.Size(903, 0)
			'
			'barDockControlLeft
			'
			Me.barDockControlLeft.CausesValidation = False
			Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
			Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
			Me.barDockControlLeft.Manager = Me.BarManager1
			Me.barDockControlLeft.Size = New System.Drawing.Size(0, 652)
			'
			'barDockControlRight
			'
			Me.barDockControlRight.CausesValidation = False
			Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
			Me.barDockControlRight.Location = New System.Drawing.Point(903, 0)
			Me.barDockControlRight.Manager = Me.BarManager1
			Me.barDockControlRight.Size = New System.Drawing.Size(0, 652)
			'
			'chkExcludeChecked
			'
			Me.chkExcludeChecked.EditValue = True
			Me.chkExcludeChecked.Location = New System.Drawing.Point(464, 86)
			Me.chkExcludeChecked.Name = "chkExcludeChecked"
			Me.chkExcludeChecked.Properties.AllowFocused = False
			Me.chkExcludeChecked.Properties.Appearance.Options.UseTextOptions = True
			Me.chkExcludeChecked.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.chkExcludeChecked.Properties.Caption = "Bereits eingesehene Informationen ausblenden"
			Me.chkExcludeChecked.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.chkExcludeChecked.Size = New System.Drawing.Size(260, 19)
			Me.chkExcludeChecked.TabIndex = 326
			'
			'lueMandant
			'
			Me.lueMandant.Location = New System.Drawing.Point(138, 59)
			Me.lueMandant.Name = "lueMandant"
			Me.lueMandant.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.lueMandant.Properties.NullText = ""
			Me.lueMandant.Size = New System.Drawing.Size(320, 20)
			Me.lueMandant.TabIndex = 325
			'
			'btnClose
			'
			Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnClose.Location = New System.Drawing.Point(765, 62)
			Me.btnClose.Name = "btnClose"
			Me.btnClose.Size = New System.Drawing.Size(103, 31)
			Me.btnClose.TabIndex = 2
			Me.btnClose.Text = "Schliessen"
			'
			'LabelControl1
			'
			Me.LabelControl1.Appearance.Options.UseTextOptions = True
			Me.LabelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.LabelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.LabelControl1.Location = New System.Drawing.Point(18, 63)
			Me.LabelControl1.Name = "LabelControl1"
			Me.LabelControl1.Size = New System.Drawing.Size(114, 13)
			Me.LabelControl1.TabIndex = 319
			Me.LabelControl1.Text = "Mandant"
			'
			'lueAdvisor
			'
			Me.lueAdvisor.Location = New System.Drawing.Point(138, 85)
			Me.lueAdvisor.MinimumSize = New System.Drawing.Size(150, 20)
			Me.lueAdvisor.Name = "lueAdvisor"
			SerializableAppearanceObject1.ForeColor = System.Drawing.Color.Red
			SerializableAppearanceObject1.Options.UseForeColor = True
			SerializableAppearanceObject2.ForeColor = System.Drawing.Color.Red
			SerializableAppearanceObject2.Options.UseForeColor = True
			SerializableAppearanceObject3.ForeColor = System.Drawing.Color.Red
			SerializableAppearanceObject3.Options.UseForeColor = True
			SerializableAppearanceObject4.ForeColor = System.Drawing.Color.Red
			SerializableAppearanceObject4.Options.UseForeColor = True
			Me.lueAdvisor.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
			Me.lueAdvisor.Properties.ShowFooter = False
			Me.lueAdvisor.Size = New System.Drawing.Size(320, 20)
			Me.lueAdvisor.TabIndex = 318
			'
			'lblBeraterIn
			'
			Me.lblBeraterIn.Appearance.Options.UseTextOptions = True
			Me.lblBeraterIn.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblBeraterIn.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblBeraterIn.Location = New System.Drawing.Point(18, 88)
			Me.lblBeraterIn.Name = "lblBeraterIn"
			Me.lblBeraterIn.Size = New System.Drawing.Size(114, 13)
			Me.lblBeraterIn.TabIndex = 313
			Me.lblBeraterIn.Text = "BeraterIn"
			'
			'txt_MDGuidForVacancyCheck
			'
			Me.txt_MDGuidForVacancyCheck.Location = New System.Drawing.Point(138, 137)
			Me.txt_MDGuidForVacancyCheck.MenuManager = Me.BarManager1
			Me.txt_MDGuidForVacancyCheck.Name = "txt_MDGuidForVacancyCheck"
			SerializableAppearanceObject5.ForeColor = System.Drawing.Color.Blue
			SerializableAppearanceObject5.Options.UseForeColor = True
			Me.txt_MDGuidForVacancyCheck.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.OK, "", -1, True, True, False, EditorButtonImageOptions2, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject5, SerializableAppearanceObject6, SerializableAppearanceObject7, SerializableAppearanceObject8, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
			Me.txt_MDGuidForVacancyCheck.Size = New System.Drawing.Size(215, 20)
			Me.txt_MDGuidForVacancyCheck.TabIndex = 333
			'
			'XtraTabControl1
			'
			Me.XtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.XtraTabControl1.Location = New System.Drawing.Point(5, 5)
			Me.XtraTabControl1.Name = "XtraTabControl1"
			Me.XtraTabControl1.SelectedTabPage = Me.xtabNotifying
			Me.XtraTabControl1.Size = New System.Drawing.Size(881, 420)
			Me.XtraTabControl1.TabIndex = 326
			Me.XtraTabControl1.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabMasterMandant, Me.xtabNotifying, Me.xtabDocScan, Me.xtabApplicant})
			'
			'xtabNotifying
			'
			Me.xtabNotifying.Controls.Add(Me.grdNotification)
			Me.xtabNotifying.Controls.Add(Me.PanelControl1)
			Me.xtabNotifying.Name = "xtabNotifying"
			Me.xtabNotifying.Padding = New System.Windows.Forms.Padding(5)
			Me.xtabNotifying.Size = New System.Drawing.Size(879, 395)
			Me.xtabNotifying.Text = "Mitteilungen"
			'
			'grdNotification
			'
			Me.grdNotification.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.grdNotification.Cursor = System.Windows.Forms.Cursors.Default
			Me.grdNotification.EmbeddedNavigator.Buttons.First.Visible = False
			Me.grdNotification.EmbeddedNavigator.Buttons.Last.Visible = False
			Me.grdNotification.EmbeddedNavigator.Buttons.Next.Visible = False
			Me.grdNotification.EmbeddedNavigator.Buttons.NextPage.Visible = False
			Me.grdNotification.EmbeddedNavigator.Buttons.Prev.Visible = False
			Me.grdNotification.EmbeddedNavigator.Buttons.PrevPage.Visible = False
			GridLevelNode1.RelationName = "Level1"
			Me.grdNotification.LevelTree.Nodes.AddRange(New DevExpress.XtraGrid.GridLevelNode() {GridLevelNode1})
			Me.grdNotification.Location = New System.Drawing.Point(6, 8)
			Me.grdNotification.MainView = Me.gvNotification
			Me.grdNotification.Name = "grdNotification"
			Me.grdNotification.Size = New System.Drawing.Size(868, 170)
			Me.grdNotification.TabIndex = 326
			Me.grdNotification.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvNotification})
			'
			'gvNotification
			'
			Me.gvNotification.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			Me.gvNotification.GridControl = Me.grdNotification
			Me.gvNotification.Name = "gvNotification"
			Me.gvNotification.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.[False]
			Me.gvNotification.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.[False]
			Me.gvNotification.OptionsBehavior.AutoSelectAllInEditor = False
			Me.gvNotification.OptionsBehavior.EditingMode = DevExpress.XtraGrid.Views.Grid.GridEditingMode.Inplace
			Me.gvNotification.OptionsPrint.ExpandAllDetails = True
			Me.gvNotification.OptionsPrint.PrintDetails = True
			Me.gvNotification.OptionsPrint.PrintPreview = True
			Me.gvNotification.OptionsView.RowAutoHeight = True
			Me.gvNotification.OptionsView.ShowGroupPanel = False
			'
			'PanelControl1
			'
			Me.PanelControl1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.PanelControl1.Controls.Add(Me.reNotifyComments)
			Me.PanelControl1.Controls.Add(Me.lblNotifyHeader)
			Me.PanelControl1.Controls.Add(Me.lblCheckedOn)
			Me.PanelControl1.Controls.Add(Me.LabelControl7)
			Me.PanelControl1.Controls.Add(Me.lblCreatedOn)
			Me.PanelControl1.Controls.Add(Me.LabelControl6)
			Me.PanelControl1.Controls.Add(Me.lblNotifyArt)
			Me.PanelControl1.Controls.Add(Me.LabelControl4)
			Me.PanelControl1.Controls.Add(Me.LabelControl3)
			Me.PanelControl1.Controls.Add(Me.LabelControl2)
			Me.PanelControl1.Location = New System.Drawing.Point(6, 186)
			Me.PanelControl1.Name = "PanelControl1"
			Me.PanelControl1.Size = New System.Drawing.Size(868, 203)
			Me.PanelControl1.TabIndex = 325
			'
			'reNotifyComments
			'
			Me.reNotifyComments.ActiveViewType = DevExpress.XtraRichEdit.RichEditViewType.Simple
			Me.reNotifyComments.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.reNotifyComments.Appearance.Text.Options.UseFont = True
			Me.reNotifyComments.LayoutUnit = DevExpress.XtraRichEdit.DocumentLayoutUnit.Pixel
			Me.reNotifyComments.Location = New System.Drawing.Point(152, 41)
			Me.reNotifyComments.MenuManager = Me.BarManager1
			Me.reNotifyComments.Name = "reNotifyComments"
			Me.reNotifyComments.ReadOnly = True
			Me.reNotifyComments.Size = New System.Drawing.Size(698, 92)
			Me.reNotifyComments.TabIndex = 335
			Me.reNotifyComments.Unit = DevExpress.Office.DocumentUnit.Millimeter
			Me.reNotifyComments.Views.SimpleView.AllowDisplayLineNumbers = True
			Me.reNotifyComments.Views.SimpleView.Padding = New DevExpress.Portable.PortablePadding(5, 4, 4, 0) ' System.Windows.Forms.Padding(5, 4, 4, 0)
			'
			'lblNotifyHeader
			'
			Me.lblNotifyHeader.AllowHtmlString = True
			Me.lblNotifyHeader.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
			Me.lblNotifyHeader.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblNotifyHeader.Location = New System.Drawing.Point(152, 22)
			Me.lblNotifyHeader.Name = "lblNotifyHeader"
			Me.lblNotifyHeader.Size = New System.Drawing.Size(249, 13)
			Me.lblNotifyHeader.TabIndex = 334
			Me.lblNotifyHeader.Text = "lblNotifyHeader"
			'
			'lblCheckedOn
			'
			Me.lblCheckedOn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
			Me.lblCheckedOn.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblCheckedOn.Location = New System.Drawing.Point(152, 158)
			Me.lblCheckedOn.Name = "lblCheckedOn"
			Me.lblCheckedOn.Size = New System.Drawing.Size(249, 13)
			Me.lblCheckedOn.TabIndex = 333
			Me.lblCheckedOn.Text = "lblCheckedOn"
			'
			'LabelControl7
			'
			Me.LabelControl7.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
			Me.LabelControl7.Appearance.Options.UseTextOptions = True
			Me.LabelControl7.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.LabelControl7.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.LabelControl7.Location = New System.Drawing.Point(29, 158)
			Me.LabelControl7.Name = "LabelControl7"
			Me.LabelControl7.Size = New System.Drawing.Size(114, 13)
			Me.LabelControl7.TabIndex = 332
			Me.LabelControl7.Text = "Gelesen"
			'
			'lblCreatedOn
			'
			Me.lblCreatedOn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
			Me.lblCreatedOn.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblCreatedOn.Location = New System.Drawing.Point(152, 139)
			Me.lblCreatedOn.Name = "lblCreatedOn"
			Me.lblCreatedOn.Size = New System.Drawing.Size(249, 13)
			Me.lblCreatedOn.TabIndex = 331
			Me.lblCreatedOn.Text = "lblCreatedOn"
			'
			'LabelControl6
			'
			Me.LabelControl6.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
			Me.LabelControl6.Appearance.Options.UseTextOptions = True
			Me.LabelControl6.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.LabelControl6.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.LabelControl6.Location = New System.Drawing.Point(29, 139)
			Me.LabelControl6.Name = "LabelControl6"
			Me.LabelControl6.Size = New System.Drawing.Size(114, 13)
			Me.LabelControl6.TabIndex = 330
			Me.LabelControl6.Text = "Veröffentlicht"
			'
			'lblNotifyArt
			'
			Me.lblNotifyArt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
			Me.lblNotifyArt.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblNotifyArt.Location = New System.Drawing.Point(685, 22)
			Me.lblNotifyArt.Name = "lblNotifyArt"
			Me.lblNotifyArt.Size = New System.Drawing.Size(201, 13)
			Me.lblNotifyArt.TabIndex = 329
			Me.lblNotifyArt.Text = "lblNotifyArt"
			'
			'LabelControl4
			'
			Me.LabelControl4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
			Me.LabelControl4.Appearance.Options.UseTextOptions = True
			Me.LabelControl4.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.LabelControl4.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.LabelControl4.Location = New System.Drawing.Point(29, 41)
			Me.LabelControl4.Name = "LabelControl4"
			Me.LabelControl4.Size = New System.Drawing.Size(114, 13)
			Me.LabelControl4.TabIndex = 328
			Me.LabelControl4.Text = "Beschreibung"
			'
			'LabelControl3
			'
			Me.LabelControl3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
			Me.LabelControl3.Appearance.Options.UseTextOptions = True
			Me.LabelControl3.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.LabelControl3.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.LabelControl3.Location = New System.Drawing.Point(565, 22)
			Me.LabelControl3.Name = "LabelControl3"
			Me.LabelControl3.Size = New System.Drawing.Size(114, 13)
			Me.LabelControl3.TabIndex = 326
			Me.LabelControl3.Text = "Art"
			'
			'LabelControl2
			'
			Me.LabelControl2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
			Me.LabelControl2.Appearance.Options.UseTextOptions = True
			Me.LabelControl2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.LabelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.LabelControl2.Location = New System.Drawing.Point(29, 22)
			Me.LabelControl2.Name = "LabelControl2"
			Me.LabelControl2.Size = New System.Drawing.Size(114, 13)
			Me.LabelControl2.TabIndex = 324
			Me.LabelControl2.Text = "Betreff"
			'
			'xtabMasterMandant
			'
			Me.xtabMasterMandant.Controls.Add(Me.grdMasterMandant)
			Me.xtabMasterMandant.Name = "xtabMasterMandant"
			Me.xtabMasterMandant.Padding = New System.Windows.Forms.Padding(5)
			Me.xtabMasterMandant.Size = New System.Drawing.Size(875, 399)
			Me.xtabMasterMandant.Text = "Mandanten"
			'
			'grdMasterMandant
			'
			Me.grdMasterMandant.Dock = System.Windows.Forms.DockStyle.Fill
			Me.grdMasterMandant.Location = New System.Drawing.Point(5, 5)
			Me.grdMasterMandant.MainView = Me.gvMasterMandant
			Me.grdMasterMandant.Name = "grdMasterMandant"
			Me.grdMasterMandant.Size = New System.Drawing.Size(865, 389)
			Me.grdMasterMandant.TabIndex = 327
			Me.grdMasterMandant.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvMasterMandant, Me.GridView2})
			'
			'gvMasterMandant
			'
			Me.gvMasterMandant.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			Me.gvMasterMandant.GridControl = Me.grdMasterMandant
			Me.gvMasterMandant.Name = "gvMasterMandant"
			Me.gvMasterMandant.OptionsSelection.EnableAppearanceFocusedCell = False
			Me.gvMasterMandant.OptionsView.ShowAutoFilterRow = True
			Me.gvMasterMandant.OptionsView.ShowGroupPanel = False
			'
			'GridView2
			'
			Me.GridView2.GridControl = Me.grdMasterMandant
			Me.GridView2.Name = "GridView2"
			'
			'xtabDocScan
			'
			Me.xtabDocScan.Controls.Add(Me.grdScanJobs)
			Me.xtabDocScan.Name = "xtabDocScan"
			Me.xtabDocScan.Padding = New System.Windows.Forms.Padding(5)
			Me.xtabDocScan.Size = New System.Drawing.Size(875, 399)
			Me.xtabDocScan.Text = "Gescannte Dokumentene"
			'
			'grdScanJobs
			'
			Me.grdScanJobs.Dock = System.Windows.Forms.DockStyle.Fill
			Me.grdScanJobs.Location = New System.Drawing.Point(5, 5)
			Me.grdScanJobs.MainView = Me.gvScanJobs
			Me.grdScanJobs.Name = "grdScanJobs"
			Me.grdScanJobs.Size = New System.Drawing.Size(865, 389)
			Me.grdScanJobs.TabIndex = 325
			Me.grdScanJobs.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvScanJobs, Me.GridView3})
			'
			'gvScanJobs
			'
			Me.gvScanJobs.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			Me.gvScanJobs.GridControl = Me.grdScanJobs
			Me.gvScanJobs.Name = "gvScanJobs"
			Me.gvScanJobs.OptionsSelection.EnableAppearanceFocusedCell = False
			Me.gvScanJobs.OptionsView.ShowAutoFilterRow = True
			Me.gvScanJobs.OptionsView.ShowGroupPanel = False
			'
			'GridView3
			'
			Me.GridView3.GridControl = Me.grdScanJobs
			Me.GridView3.Name = "GridView3"
			'
			'xtabApplicant
			'
			Me.xtabApplicant.Controls.Add(Me.grdApplicant)
			Me.xtabApplicant.Name = "xtabApplicant"
			Me.xtabApplicant.Padding = New System.Windows.Forms.Padding(5)
			Me.xtabApplicant.Size = New System.Drawing.Size(875, 399)
			Me.xtabApplicant.Text = "Applicant"
			'
			'grdApplicant
			'
			Me.grdApplicant.Dock = System.Windows.Forms.DockStyle.Fill
			Me.grdApplicant.Location = New System.Drawing.Point(5, 5)
			Me.grdApplicant.MainView = Me.gvApplicant
			Me.grdApplicant.Name = "grdApplicant"
			Me.grdApplicant.Size = New System.Drawing.Size(865, 389)
			Me.grdApplicant.TabIndex = 326
			Me.grdApplicant.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvApplicant, Me.GridView4})
			'
			'gvApplicant
			'
			Me.gvApplicant.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			Me.gvApplicant.GridControl = Me.grdApplicant
			Me.gvApplicant.Name = "gvApplicant"
			Me.gvApplicant.OptionsSelection.EnableAppearanceFocusedCell = False
			Me.gvApplicant.OptionsView.ShowAutoFilterRow = True
			Me.gvApplicant.OptionsView.ShowGroupPanel = False
			'
			'GridView4
			'
			Me.GridView4.GridControl = Me.grdApplicant
			Me.GridView4.Name = "GridView4"
			'
			'navbarImageCollectionLarge
			'
			Me.navbarImageCollectionLarge.ImageStream = CType(resources.GetObject("navbarImageCollectionLarge.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
			Me.navbarImageCollectionLarge.TransparentColor = System.Drawing.Color.Transparent
			Me.navbarImageCollectionLarge.Images.SetKeyName(0, "Mail_16x16.png")
			Me.navbarImageCollectionLarge.Images.SetKeyName(1, "Organizer_16x16.png")
			'
			'navbarImageCollection
			'
			Me.navbarImageCollection.ImageStream = CType(resources.GetObject("navbarImageCollection.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
			Me.navbarImageCollection.TransparentColor = System.Drawing.Color.Transparent
			Me.navbarImageCollection.Images.SetKeyName(0, "Inbox_16x16.png")
			Me.navbarImageCollection.Images.SetKeyName(1, "Outbox_16x16.png")
			Me.navbarImageCollection.Images.SetKeyName(2, "Drafts_16x16.png")
			Me.navbarImageCollection.Images.SetKeyName(3, "Trash_16x16.png")
			Me.navbarImageCollection.Images.SetKeyName(4, "Calendar_16x16.png")
			Me.navbarImageCollection.Images.SetKeyName(5, "Tasks_16x16.png")
			Me.navbarImageCollection.InsertGalleryImage("information_32x32.png", "images/function%20library/information_32x32.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/function%20library/information_32x32.png"), 6)
			Me.navbarImageCollection.Images.SetKeyName(6, "information_32x32.png")
			'
			'NotifyIcon1
			'
			Me.NotifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info
			Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
			Me.NotifyIcon1.Text = "Sputnik Enterprise Suite: [Notification]"
			Me.NotifyIcon1.Visible = True
			'
			'frmNotify
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(903, 652)
			Me.Controls.Add(Me.splitContainerControl)
			Me.Controls.Add(Me.barDockControlLeft)
			Me.Controls.Add(Me.barDockControlRight)
			Me.Controls.Add(Me.barDockControlBottom)
			Me.Controls.Add(Me.barDockControlTop)
			Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
			Me.MinimumSize = New System.Drawing.Size(919, 691)
			Me.Name = "frmNotify"
			Me.Text = "Sputnik Enterprise Suite: [Notification]"
			CType(Me.splitContainerControl, System.ComponentModel.ISupportInitialize).EndInit()
			Me.splitContainerControl.ResumeLayout(False)
			CType(Me.grpSearch, System.ComponentModel.ISupportInitialize).EndInit()
			Me.grpSearch.ResumeLayout(False)
			CType(Me.txtFileServer.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.chkExcludeChecked.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.lueAdvisor.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.txt_MDGuidForVacancyCheck.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.XtraTabControl1.ResumeLayout(False)
			Me.xtabNotifying.ResumeLayout(False)
			CType(Me.grdNotification, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvNotification, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.PanelControl1.ResumeLayout(False)
			Me.xtabMasterMandant.ResumeLayout(False)
			CType(Me.grdMasterMandant, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvMasterMandant, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.GridView2, System.ComponentModel.ISupportInitialize).EndInit()
			Me.xtabDocScan.ResumeLayout(False)
			CType(Me.grdScanJobs, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvScanJobs, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.GridView3, System.ComponentModel.ISupportInitialize).EndInit()
			Me.xtabApplicant.ResumeLayout(False)
			CType(Me.grdApplicant, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvApplicant, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.GridView4, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.navbarImageCollectionLarge, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.navbarImageCollection, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)
			Me.PerformLayout()

		End Sub
		Private WithEvents splitContainerControl As DevExpress.XtraEditors.SplitContainerControl
		Private WithEvents navbarImageCollection As DevExpress.Utils.ImageCollection
		Private WithEvents navbarImageCollectionLarge As DevExpress.Utils.ImageCollection
		Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
		Friend WithEvents btnClose As DevExpress.XtraEditors.SimpleButton
		Friend WithEvents grpSearch As DevExpress.XtraEditors.GroupControl
		Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lueAdvisor As DevExpress.XtraEditors.LookUpEdit
		Friend WithEvents lblBeraterIn As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lueMandant As DevExpress.XtraEditors.LookUpEdit
		Friend WithEvents chkExcludeChecked As DevExpress.XtraEditors.CheckEdit
		Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
		Friend WithEvents lblCheckedOn As DevExpress.XtraEditors.LabelControl
		Friend WithEvents LabelControl7 As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblCreatedOn As DevExpress.XtraEditors.LabelControl
		Friend WithEvents LabelControl6 As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblNotifyArt As DevExpress.XtraEditors.LabelControl
		Friend WithEvents LabelControl4 As DevExpress.XtraEditors.LabelControl
		Friend WithEvents LabelControl3 As DevExpress.XtraEditors.LabelControl
		Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblNotifyHeader As DevExpress.XtraEditors.LabelControl
		Friend WithEvents reNotifyComments As DevExpress.XtraRichEdit.RichEditControl
		Friend WithEvents XtraTabControl1 As DevExpress.XtraTab.XtraTabControl
		Friend WithEvents xtabNotifying As DevExpress.XtraTab.XtraTabPage
		Friend WithEvents xtabDocScan As DevExpress.XtraTab.XtraTabPage
		Friend WithEvents grdScanJobs As DevExpress.XtraGrid.GridControl
		Private WithEvents gvScanJobs As DevExpress.XtraGrid.Views.Grid.GridView
		Friend WithEvents GridView3 As DevExpress.XtraGrid.Views.Grid.GridView
		Friend WithEvents grdNotification As DevExpress.XtraGrid.GridControl
		Friend WithEvents gvNotification As DevExpress.XtraGrid.Views.Grid.GridView
		Friend WithEvents AlertControl1 As DevExpress.XtraBars.Alerter.AlertControl
		Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
		Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
		Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
		Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
		Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
		Friend WithEvents grdMasterMandant As DevExpress.XtraGrid.GridControl
		Private WithEvents gvMasterMandant As DevExpress.XtraGrid.Views.Grid.GridView
		Friend WithEvents GridView2 As DevExpress.XtraGrid.Views.Grid.GridView
		Friend WithEvents xtabMasterMandant As DevExpress.XtraTab.XtraTabPage
		Friend WithEvents xtabApplicant As DevExpress.XtraTab.XtraTabPage
		Friend WithEvents grdApplicant As DevExpress.XtraGrid.GridControl
		Private WithEvents gvApplicant As DevExpress.XtraGrid.Views.Grid.GridView
		Friend WithEvents GridView4 As DevExpress.XtraGrid.Views.Grid.GridView
		Friend WithEvents txtFileServer As TextEdit
		Friend WithEvents LabelControl5 As LabelControl
		Friend WithEvents btnUpdateCountryData As SimpleButton
		Friend WithEvents btnUpdateGeoData As SimpleButton
		Friend WithEvents LabelControl8 As LabelControl
		Friend WithEvents btnCheckVacancies As SimpleButton
		Friend WithEvents txt_MDGuidForVacancyCheck As ComboBoxEdit
	End Class

End Namespace
