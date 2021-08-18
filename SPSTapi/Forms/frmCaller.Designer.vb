Namespace UI

	<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
	Partial Class frmCaller
		Inherits DevExpress.XtraEditors.XtraForm

		'Form overrides dispose to clean up the component list.
		<System.Diagnostics.DebuggerNonUserCode()>
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
		<System.Diagnostics.DebuggerStepThrough()>
		Private Sub InitializeComponent()
			Me.components = New System.ComponentModel.Container()
			Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCaller))
			Dim ButtonImageOptions1 As DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions = New DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions()
			Me.grpNumber = New System.Windows.Forms.GroupBox()
			Me.GroupControl1 = New DevExpress.XtraEditors.GroupControl()
			Me.lueTapiLines = New DevExpress.XtraEditors.LookUpEdit()
			Me.btnDial = New DevExpress.XtraEditors.SimpleButton()
			Me.lblAvailableLines = New DevExpress.XtraEditors.LabelControl()
			Me.cbo_MyNumber = New DevExpress.XtraEditors.ComboBoxEdit()
			Me.btnRefreshData = New DevExpress.XtraEditors.SimpleButton()
			Me.grdAvailableData = New DevExpress.XtraGrid.GridControl()
			Me.gvAvailableData = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.grpContact = New DevExpress.XtraEditors.GroupControl()
			Me.btnAddNewContact = New DevExpress.XtraEditors.SimpleButton()
			Me.grdContact = New DevExpress.XtraGrid.GridControl()
			Me.gvContact = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.txtContactData = New DevExpress.XtraEditors.MemoEdit()
			Me.txtContactTitle = New DevExpress.XtraEditors.TextEdit()
			Me.listBoxLog = New DevExpress.XtraEditors.ListBoxControl()
			Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
			Me.RepositoryItemMarqueeProgressBar4 = New DevExpress.XtraEditors.Repository.RepositoryItemMarqueeProgressBar()
			Me.RepositoryItemMarqueeProgressBar3 = New DevExpress.XtraEditors.Repository.RepositoryItemMarqueeProgressBar()
			Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
			Me.Bar4 = New DevExpress.XtraBars.Bar()
			Me.bsiInfo = New DevExpress.XtraBars.BarStaticItem()
			Me.bbiSetting = New DevExpress.XtraBars.BarButtonItem()
			Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
			Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
			Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
			Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
			Me.BarStaticItem1 = New DevExpress.XtraBars.BarStaticItem()
			Me.BarEditItem1 = New DevExpress.XtraBars.BarEditItem()
			Me.RepositoryItemMarqueeProgressBar1 = New DevExpress.XtraEditors.Repository.RepositoryItemMarqueeProgressBar()
			Me.RepositoryItemMarqueeProgressBar2 = New DevExpress.XtraEditors.Repository.RepositoryItemMarqueeProgressBar()
			Me.ErrorProvider = New DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(Me.components)
			Me.grpNumber.SuspendLayout()
			CType(Me.GroupControl1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.GroupControl1.SuspendLayout()
			CType(Me.lueTapiLines.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.cbo_MyNumber.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.grdAvailableData, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvAvailableData, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.grpContact, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.grpContact.SuspendLayout()
			CType(Me.grdContact, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvContact, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.txtContactData.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.txtContactTitle.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.listBoxLog, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.RepositoryItemMarqueeProgressBar4, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.RepositoryItemMarqueeProgressBar3, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.RepositoryItemMarqueeProgressBar1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.RepositoryItemMarqueeProgressBar2, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			'
			'grpNumber
			'
			Me.grpNumber.Controls.Add(Me.GroupControl1)
			Me.grpNumber.Controls.Add(Me.grpContact)
			Me.grpNumber.Controls.Add(Me.listBoxLog)
			Me.grpNumber.Dock = System.Windows.Forms.DockStyle.Fill
			Me.grpNumber.Location = New System.Drawing.Point(5, 5)
			Me.grpNumber.Name = "grpNumber"
			Me.grpNumber.Size = New System.Drawing.Size(801, 530)
			Me.grpNumber.TabIndex = 213
			Me.grpNumber.TabStop = False
			'
			'GroupControl1
			'
			Me.GroupControl1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
			Me.GroupControl1.AppearanceCaption.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.GroupControl1.AppearanceCaption.Options.UseFont = True
			Me.GroupControl1.Controls.Add(Me.lueTapiLines)
			Me.GroupControl1.Controls.Add(Me.btnDial)
			Me.GroupControl1.Controls.Add(Me.lblAvailableLines)
			Me.GroupControl1.Controls.Add(Me.cbo_MyNumber)
			Me.GroupControl1.Controls.Add(Me.btnRefreshData)
			Me.GroupControl1.Controls.Add(Me.grdAvailableData)
			Me.GroupControl1.Location = New System.Drawing.Point(6, 11)
			Me.GroupControl1.Name = "GroupControl1"
			Me.GroupControl1.Size = New System.Drawing.Size(390, 441)
			Me.GroupControl1.TabIndex = 1058
			Me.GroupControl1.Text = "Telefonie"
			'
			'lueTapiLines
			'
			Me.lueTapiLines.Location = New System.Drawing.Point(5, 26)
			Me.lueTapiLines.Name = "lueTapiLines"
			Me.lueTapiLines.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.lueTapiLines.Properties.NullText = ""
			Me.lueTapiLines.Properties.PopupSizeable = False
			Me.lueTapiLines.Size = New System.Drawing.Size(336, 20)
			Me.lueTapiLines.TabIndex = 1053
			'
			'btnDial
			'
			Me.btnDial.ImageOptions.Image = Global.SPSTapi.My.Resources.Resources.phone3_16x16
			Me.btnDial.Location = New System.Drawing.Point(347, 49)
			Me.btnDial.Name = "btnDial"
			Me.btnDial.Size = New System.Drawing.Size(27, 20)
			Me.btnDial.TabIndex = 1057
			'
			'lblAvailableLines
			'
			Me.lblAvailableLines.Appearance.ForeColor = System.Drawing.Color.Black
			Me.lblAvailableLines.Appearance.Options.UseForeColor = True
			Me.lblAvailableLines.Appearance.Options.UseTextOptions = True
			Me.lblAvailableLines.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblAvailableLines.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblAvailableLines.Location = New System.Drawing.Point(-125, 27)
			Me.lblAvailableLines.Name = "lblAvailableLines"
			Me.lblAvailableLines.Size = New System.Drawing.Size(124, 13)
			Me.lblAvailableLines.TabIndex = 47
			Me.lblAvailableLines.Text = "Verfügbare Linien"
			'
			'cbo_MyNumber
			'
			Me.cbo_MyNumber.Location = New System.Drawing.Point(5, 52)
			Me.cbo_MyNumber.Name = "cbo_MyNumber"
			Me.cbo_MyNumber.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.cbo_MyNumber.Size = New System.Drawing.Size(336, 20)
			Me.cbo_MyNumber.TabIndex = 44
			'
			'btnRefreshData
			'
			Me.btnRefreshData.ImageOptions.Image = CType(resources.GetObject("btnRefreshData.ImageOptions.Image"), System.Drawing.Image)
			Me.btnRefreshData.Location = New System.Drawing.Point(347, 75)
			Me.btnRefreshData.Name = "btnRefreshData"
			Me.btnRefreshData.Size = New System.Drawing.Size(27, 20)
			Me.btnRefreshData.TabIndex = 1056
			'
			'grdAvailableData
			'
			Me.grdAvailableData.AllowDrop = True
			Me.grdAvailableData.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
			Me.grdAvailableData.Location = New System.Drawing.Point(5, 75)
			Me.grdAvailableData.MainView = Me.gvAvailableData
			Me.grdAvailableData.Name = "grdAvailableData"
			Me.grdAvailableData.Size = New System.Drawing.Size(336, 361)
			Me.grdAvailableData.TabIndex = 1055
			Me.grdAvailableData.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvAvailableData})
			'
			'gvAvailableData
			'
			Me.gvAvailableData.Appearance.FocusedRow.BackColor = System.Drawing.Color.Orange
			Me.gvAvailableData.Appearance.FocusedRow.BorderColor = System.Drawing.Color.Transparent
			Me.gvAvailableData.Appearance.FocusedRow.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.gvAvailableData.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black
			Me.gvAvailableData.Appearance.FocusedRow.Options.UseBackColor = True
			Me.gvAvailableData.Appearance.FocusedRow.Options.UseBorderColor = True
			Me.gvAvailableData.Appearance.FocusedRow.Options.UseFont = True
			Me.gvAvailableData.Appearance.FocusedRow.Options.UseForeColor = True
			Me.gvAvailableData.Appearance.SelectedRow.BackColor = System.Drawing.Color.Orange
			Me.gvAvailableData.Appearance.SelectedRow.BorderColor = System.Drawing.Color.Transparent
			Me.gvAvailableData.Appearance.SelectedRow.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.gvAvailableData.Appearance.SelectedRow.ForeColor = System.Drawing.Color.Black
			Me.gvAvailableData.Appearance.SelectedRow.Options.UseBackColor = True
			Me.gvAvailableData.Appearance.SelectedRow.Options.UseBorderColor = True
			Me.gvAvailableData.Appearance.SelectedRow.Options.UseFont = True
			Me.gvAvailableData.Appearance.SelectedRow.Options.UseForeColor = True
			Me.gvAvailableData.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus
			Me.gvAvailableData.GridControl = Me.grdAvailableData
			Me.gvAvailableData.Name = "gvAvailableData"
			Me.gvAvailableData.OptionsBehavior.Editable = False
			Me.gvAvailableData.OptionsSelection.EnableAppearanceFocusedCell = False
			Me.gvAvailableData.OptionsView.EnableAppearanceEvenRow = True
			Me.gvAvailableData.OptionsView.ShowGroupPanel = False
			'
			'grpContact
			'
			Me.grpContact.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.grpContact.AppearanceCaption.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grpContact.AppearanceCaption.Options.UseFont = True
			Me.grpContact.Controls.Add(Me.btnAddNewContact)
			Me.grpContact.Controls.Add(Me.grdContact)
			Me.grpContact.Controls.Add(Me.txtContactData)
			Me.grpContact.Controls.Add(Me.txtContactTitle)
			Me.grpContact.CustomHeaderButtons.AddRange(New DevExpress.XtraEditors.ButtonPanel.IBaseButton() {New DevExpress.XtraEditors.ButtonsPanelControl.GroupBoxButton("", True, ButtonImageOptions1, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", -1, True, Nothing, True, False, True, Nothing, 0)})
			Me.grpContact.CustomHeaderButtonsLocation = DevExpress.Utils.GroupElementLocation.AfterText
			Me.grpContact.Location = New System.Drawing.Point(408, 11)
			Me.grpContact.Name = "grpContact"
			Me.grpContact.Size = New System.Drawing.Size(390, 441)
			Me.grpContact.TabIndex = 217
			Me.grpContact.Text = "Kontakte"
			'
			'btnAddNewContact
			'
			Me.btnAddNewContact.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnAddNewContact.ImageOptions.Image = Global.SPSTapi.My.Resources.Resources.saveto_16x16
			Me.btnAddNewContact.Location = New System.Drawing.Point(356, 52)
			Me.btnAddNewContact.Name = "btnAddNewContact"
			Me.btnAddNewContact.Size = New System.Drawing.Size(27, 20)
			Me.btnAddNewContact.TabIndex = 273
			'
			'grdContact
			'
			Me.grdContact.AllowDrop = True
			Me.grdContact.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.grdContact.Location = New System.Drawing.Point(5, 94)
			Me.grdContact.MainView = Me.gvContact
			Me.grdContact.Name = "grdContact"
			Me.grdContact.Size = New System.Drawing.Size(378, 342)
			Me.grdContact.TabIndex = 1
			Me.grdContact.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvContact})
			'
			'gvContact
			'
			Me.gvContact.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			Me.gvContact.GridControl = Me.grdContact
			Me.gvContact.Name = "gvContact"
			Me.gvContact.OptionsBehavior.Editable = False
			Me.gvContact.OptionsSelection.EnableAppearanceFocusedCell = False
			Me.gvContact.OptionsView.ShowGroupPanel = False
			'
			'txtContactData
			'
			Me.txtContactData.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.txtContactData.EditValue = ""
			Me.txtContactData.Location = New System.Drawing.Point(5, 52)
			Me.txtContactData.Name = "txtContactData"
			Me.txtContactData.Properties.NullValuePrompt = "Kontak Beschreibung"
			Me.txtContactData.Size = New System.Drawing.Size(345, 36)
			Me.txtContactData.TabIndex = 45
			'
			'txtContactTitle
			'
			Me.txtContactTitle.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.txtContactTitle.EditValue = ""
			Me.txtContactTitle.Location = New System.Drawing.Point(5, 26)
			Me.txtContactTitle.Name = "txtContactTitle"
			Me.txtContactTitle.Properties.NullValuePrompt = "Kontakt Bezeichnung"
			Me.txtContactTitle.Size = New System.Drawing.Size(378, 20)
			Me.txtContactTitle.TabIndex = 274
			'
			'listBoxLog
			'
			Me.listBoxLog.Appearance.Options.UseTextOptions = True
			Me.listBoxLog.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
			Me.listBoxLog.AppearanceSelected.Options.UseTextOptions = True
			Me.listBoxLog.AppearanceSelected.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
			Me.listBoxLog.Cursor = System.Windows.Forms.Cursors.Default
			Me.listBoxLog.Dock = System.Windows.Forms.DockStyle.Bottom
			Me.listBoxLog.Location = New System.Drawing.Point(3, 457)
			Me.listBoxLog.Name = "listBoxLog"
			Me.listBoxLog.Size = New System.Drawing.Size(795, 70)
			Me.listBoxLog.TabIndex = 1054
			'
			'NotifyIcon1
			'
			Me.NotifyIcon1.Text = "NotifyIcon1"
			Me.NotifyIcon1.Visible = True
			'
			'RepositoryItemMarqueeProgressBar4
			'
			Me.RepositoryItemMarqueeProgressBar4.Name = "RepositoryItemMarqueeProgressBar4"
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
			Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiInfo, Me.BarStaticItem1, Me.BarEditItem1, Me.bbiSetting})
			Me.BarManager1.MaxItemId = 16
			Me.BarManager1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemMarqueeProgressBar1, Me.RepositoryItemMarqueeProgressBar2})
			Me.BarManager1.StatusBar = Me.Bar4
			'
			'Bar4
			'
			Me.Bar4.BarName = "Statusleiste"
			Me.Bar4.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
			Me.Bar4.DockCol = 0
			Me.Bar4.DockRow = 0
			Me.Bar4.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
			Me.Bar4.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiInfo), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiSetting)})
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
			'bbiSetting
			'
			Me.bbiSetting.Caption = "Einstellungen"
			Me.bbiSetting.Id = 15
			Me.bbiSetting.ImageOptions.Image = CType(resources.GetObject("bbiSetting.ImageOptions.Image"), System.Drawing.Image)
			Me.bbiSetting.Name = "bbiSetting"
			Me.bbiSetting.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
			'
			'barDockControlTop
			'
			Me.barDockControlTop.CausesValidation = False
			Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
			Me.barDockControlTop.Location = New System.Drawing.Point(5, 5)
			Me.barDockControlTop.Manager = Me.BarManager1
			Me.barDockControlTop.Size = New System.Drawing.Size(801, 0)
			'
			'barDockControlBottom
			'
			Me.barDockControlBottom.CausesValidation = False
			Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
			Me.barDockControlBottom.Location = New System.Drawing.Point(5, 535)
			Me.barDockControlBottom.Manager = Me.BarManager1
			Me.barDockControlBottom.Size = New System.Drawing.Size(801, 26)
			'
			'barDockControlLeft
			'
			Me.barDockControlLeft.CausesValidation = False
			Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
			Me.barDockControlLeft.Location = New System.Drawing.Point(5, 5)
			Me.barDockControlLeft.Manager = Me.BarManager1
			Me.barDockControlLeft.Size = New System.Drawing.Size(0, 530)
			'
			'barDockControlRight
			'
			Me.barDockControlRight.CausesValidation = False
			Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
			Me.barDockControlRight.Location = New System.Drawing.Point(806, 5)
			Me.barDockControlRight.Manager = Me.BarManager1
			Me.barDockControlRight.Size = New System.Drawing.Size(0, 530)
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
			Me.BarEditItem1.Edit = Me.RepositoryItemMarqueeProgressBar1
			Me.BarEditItem1.Id = 10
			Me.BarEditItem1.Name = "BarEditItem1"
			'
			'RepositoryItemMarqueeProgressBar1
			'
			Me.RepositoryItemMarqueeProgressBar1.Name = "RepositoryItemMarqueeProgressBar1"
			'
			'RepositoryItemMarqueeProgressBar2
			'
			Me.RepositoryItemMarqueeProgressBar2.Name = "RepositoryItemMarqueeProgressBar2"
			'
			'ErrorProvider
			'
			Me.ErrorProvider.ContainerControl = Me
			'
			'frmCaller
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(811, 566)
			Me.Controls.Add(Me.grpNumber)
			Me.Controls.Add(Me.barDockControlLeft)
			Me.Controls.Add(Me.barDockControlRight)
			Me.Controls.Add(Me.barDockControlBottom)
			Me.Controls.Add(Me.barDockControlTop)
			Me.IconOptions.Icon = CType(resources.GetObject("frmCaller.IconOptions.Icon"), System.Drawing.Icon)
			Me.MaximumSize = New System.Drawing.Size(813, 598)
			Me.MinimumSize = New System.Drawing.Size(813, 598)
			Me.Name = "frmCaller"
			Me.Padding = New System.Windows.Forms.Padding(5)
			Me.Text = "Sputnik Telefon-Manager"
			Me.grpNumber.ResumeLayout(False)
			CType(Me.GroupControl1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.GroupControl1.ResumeLayout(False)
			CType(Me.lueTapiLines.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.cbo_MyNumber.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.grdAvailableData, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvAvailableData, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.grpContact, System.ComponentModel.ISupportInitialize).EndInit()
			Me.grpContact.ResumeLayout(False)
			CType(Me.grdContact, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvContact, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.txtContactData.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.txtContactTitle.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.listBoxLog, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.RepositoryItemMarqueeProgressBar4, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.RepositoryItemMarqueeProgressBar3, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.RepositoryItemMarqueeProgressBar1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.RepositoryItemMarqueeProgressBar2, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)
			Me.PerformLayout()

		End Sub
		Friend WithEvents grpNumber As GroupBox
		Friend WithEvents lblAvailableLines As DevExpress.XtraEditors.LabelControl
		Friend WithEvents cbo_MyNumber As DevExpress.XtraEditors.ComboBoxEdit
		Friend WithEvents grpContact As DevExpress.XtraEditors.GroupControl
		Friend WithEvents grdContact As DevExpress.XtraGrid.GridControl
		Friend WithEvents gvContact As DevExpress.XtraGrid.Views.Grid.GridView
		Friend WithEvents NotifyIcon1 As NotifyIcon
		Friend WithEvents lueTapiLines As DevExpress.XtraEditors.LookUpEdit
		Friend WithEvents listBoxLog As DevExpress.XtraEditors.ListBoxControl
		Friend WithEvents txtContactData As DevExpress.XtraEditors.MemoEdit
		Friend WithEvents btnAddNewContact As DevExpress.XtraEditors.SimpleButton
		Friend WithEvents grdAvailableData As DevExpress.XtraGrid.GridControl
		Friend WithEvents gvAvailableData As DevExpress.XtraGrid.Views.Grid.GridView
		Friend WithEvents btnRefreshData As DevExpress.XtraEditors.SimpleButton
		Friend WithEvents RepositoryItemMarqueeProgressBar4 As DevExpress.XtraEditors.Repository.RepositoryItemMarqueeProgressBar
		Friend WithEvents RepositoryItemMarqueeProgressBar3 As DevExpress.XtraEditors.Repository.RepositoryItemMarqueeProgressBar
		Friend WithEvents btnDial As DevExpress.XtraEditors.SimpleButton
		Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
		Friend WithEvents Bar4 As DevExpress.XtraBars.Bar
		Friend WithEvents bsiInfo As DevExpress.XtraBars.BarStaticItem
		Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
		Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
		Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
		Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
		Friend WithEvents BarStaticItem1 As DevExpress.XtraBars.BarStaticItem
		Friend WithEvents BarEditItem1 As DevExpress.XtraBars.BarEditItem
		Friend WithEvents RepositoryItemMarqueeProgressBar1 As DevExpress.XtraEditors.Repository.RepositoryItemMarqueeProgressBar
		Friend WithEvents RepositoryItemMarqueeProgressBar2 As DevExpress.XtraEditors.Repository.RepositoryItemMarqueeProgressBar
		Friend WithEvents bbiSetting As DevExpress.XtraBars.BarButtonItem
		Friend WithEvents ErrorProvider As DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider
		Friend WithEvents GroupControl1 As DevExpress.XtraEditors.GroupControl
		Friend WithEvents txtContactTitle As DevExpress.XtraEditors.TextEdit
	End Class

End Namespace
