<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmImportTwixData
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
		Me.components = New System.ComponentModel.Container()
		Dim TileItemElement7 As DevExpress.XtraEditors.TileItemElement = New DevExpress.XtraEditors.TileItemElement()
		Dim TileItemElement8 As DevExpress.XtraEditors.TileItemElement = New DevExpress.XtraEditors.TileItemElement()
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
		Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
		Me.Bar4 = New DevExpress.XtraBars.Bar()
		Me.bsiInfo = New DevExpress.XtraBars.BarStaticItem()
		Me.BarStaticItem2 = New DevExpress.XtraBars.BarStaticItem()
		Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
		Me.BarStaticItem1 = New DevExpress.XtraBars.BarStaticItem()
		Me.tileBar = New DevExpress.XtraBars.Navigation.TileBar()
		Me.tileBarGroupTables = New DevExpress.XtraBars.Navigation.TileBarGroup()
		Me.employeesTileBarItem = New DevExpress.XtraBars.Navigation.TileBarItem()
		Me.customersTileBarItem = New DevExpress.XtraBars.Navigation.TileBarItem()
		Me.pnlMandant = New DevExpress.XtraEditors.PanelControl()
		Me.pnlUpdateAddButton = New DevExpress.XtraEditors.PanelControl()
		Me.btnNewData = New DevExpress.XtraEditors.SimpleButton()
		Me.btnUpdateData = New DevExpress.XtraEditors.SimpleButton()
		Me.tgsSourceSelection = New DevExpress.XtraEditors.ToggleSwitch()
		Me.btnImportData = New DevExpress.XtraEditors.SimpleButton()
		Me.lueSourceMD = New DevExpress.XtraEditors.LookUpEdit()
		Me.lblSourceMandant = New DevExpress.XtraEditors.LabelControl()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.pnlMandant, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.pnlMandant.SuspendLayout()
		CType(Me.pnlUpdateAddButton, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.pnlUpdateAddButton.SuspendLayout()
		CType(Me.tgsSourceSelection.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lueSourceMD.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'BarManager1
		'
		Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar4})
		Me.BarManager1.DockControls.Add(Me.barDockControlTop)
		Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
		Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
		Me.BarManager1.DockControls.Add(Me.barDockControlRight)
		Me.BarManager1.Form = Me
		Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiInfo, Me.BarStaticItem1, Me.BarStaticItem2})
		Me.BarManager1.MaxItemId = 11
		Me.BarManager1.StatusBar = Me.Bar4
		'
		'Bar4
		'
		Me.Bar4.BarName = "Statusleiste"
		Me.Bar4.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
		Me.Bar4.DockCol = 0
		Me.Bar4.DockRow = 0
		Me.Bar4.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
		Me.Bar4.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiInfo), New DevExpress.XtraBars.LinkPersistInfo(Me.BarStaticItem2)})
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
		'BarStaticItem2
		'
		Me.BarStaticItem2.Id = 10
		Me.BarStaticItem2.Name = "BarStaticItem2"
		'
		'barDockControlTop
		'
		Me.barDockControlTop.CausesValidation = False
		Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
		Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlTop.Manager = Me.BarManager1
		Me.barDockControlTop.Size = New System.Drawing.Size(1334, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 571)
		Me.barDockControlBottom.Manager = Me.BarManager1
		Me.barDockControlBottom.Size = New System.Drawing.Size(1334, 22)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Manager = Me.BarManager1
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 571)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(1334, 0)
		Me.barDockControlRight.Manager = Me.BarManager1
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 571)
		'
		'BarStaticItem1
		'
		Me.BarStaticItem1.Caption = " "
		Me.BarStaticItem1.Id = 5
		Me.BarStaticItem1.Name = "BarStaticItem1"
		'
		'tileBar
		'
		Me.tileBar.AllowGlyphSkinning = True
		Me.tileBar.AllowHtmlText = DevExpress.Utils.DefaultBoolean.[True]
		Me.tileBar.AllowSelectedItem = True
		Me.tileBar.AppearanceGroupText.ForeColor = System.Drawing.Color.FromArgb(CType(CType(140, Byte), Integer), CType(CType(140, Byte), Integer), CType(CType(140, Byte), Integer))
		Me.tileBar.AppearanceGroupText.Options.UseForeColor = True
		Me.tileBar.BackColor = System.Drawing.Color.Transparent
		Me.tileBar.Dock = System.Windows.Forms.DockStyle.Top
		Me.tileBar.DropDownButtonWidth = 30
		Me.tileBar.DropDownOptions.BeakColor = System.Drawing.Color.Empty
		Me.tileBar.EnableItemDoubleClickEvent = False
		Me.tileBar.Groups.Add(Me.tileBarGroupTables)
		Me.tileBar.IndentBetweenGroups = 10
		Me.tileBar.IndentBetweenItems = 10
		Me.tileBar.ItemPadding = New System.Windows.Forms.Padding(8, 6, 12, 6)
		Me.tileBar.Location = New System.Drawing.Point(0, 0)
		Me.tileBar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
		Me.tileBar.MaxId = 3
		Me.tileBar.MaximumSize = New System.Drawing.Size(0, 110)
		Me.tileBar.MinimumSize = New System.Drawing.Size(100, 110)
		Me.tileBar.Name = "tileBar"
		Me.tileBar.Padding = New System.Windows.Forms.Padding(29, 11, 29, 11)
		Me.tileBar.ScrollMode = DevExpress.XtraEditors.TileControlScrollMode.None
		Me.tileBar.SelectedItem = Me.employeesTileBarItem
		Me.tileBar.SelectionBorderWidth = 2
		Me.tileBar.SelectionColorMode = DevExpress.XtraBars.Navigation.SelectionColorMode.UseItemBackColor
		Me.tileBar.ShowGroupText = False
		Me.tileBar.ShowItemShadow = True
		Me.tileBar.Size = New System.Drawing.Size(1334, 110)
		Me.tileBar.TabIndex = 4
		Me.tileBar.Text = "tileBar"
		Me.tileBar.WideTileWidth = 150
		'
		'tileBarGroupTables
		'
		Me.tileBarGroupTables.Items.Add(Me.employeesTileBarItem)
		Me.tileBarGroupTables.Items.Add(Me.customersTileBarItem)
		Me.tileBarGroupTables.Name = "tileBarGroupTables"
		Me.tileBarGroupTables.Text = "TABLES"
		'
		'employeesTileBarItem
		'
		Me.employeesTileBarItem.AppearanceItem.Normal.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(115, Byte), Integer), CType(CType(196, Byte), Integer))
		Me.employeesTileBarItem.AppearanceItem.Normal.Options.UseBackColor = True
		Me.employeesTileBarItem.DropDownOptions.BeakColor = System.Drawing.Color.Empty
		TileItemElement7.ImageOptions.Image = Global.SPS.Export.Listing.Utility.My.Resources.Resources.employee_16x16
		TileItemElement7.ImageOptions.ImageUri.Uri = "Cube;Size32x32;GrayScaled"
		TileItemElement7.Text = "Kandidaten"
		Me.employeesTileBarItem.Elements.Add(TileItemElement7)
		Me.employeesTileBarItem.Id = 1
		Me.employeesTileBarItem.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide
		Me.employeesTileBarItem.Name = "employeesTileBarItem"
		'
		'customersTileBarItem
		'
		Me.customersTileBarItem.AllowHtmlText = DevExpress.Utils.DefaultBoolean.[True]
		Me.customersTileBarItem.DropDownOptions.BeakColor = System.Drawing.Color.Empty
		TileItemElement8.ImageOptions.Image = Global.SPS.Export.Listing.Utility.My.Resources.Resources.customer_16x16
		TileItemElement8.ImageOptions.ImageUri.Uri = "Cube;Size32x32;GrayScaled"
		TileItemElement8.Text = "Kunden"
		Me.customersTileBarItem.Elements.Add(TileItemElement8)
		Me.customersTileBarItem.Id = 2
		Me.customersTileBarItem.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide
		Me.customersTileBarItem.Name = "customersTileBarItem"
		'
		'pnlMandant
		'
		Me.pnlMandant.Controls.Add(Me.pnlUpdateAddButton)
		Me.pnlMandant.Controls.Add(Me.tgsSourceSelection)
		Me.pnlMandant.Controls.Add(Me.btnImportData)
		Me.pnlMandant.Controls.Add(Me.lueSourceMD)
		Me.pnlMandant.Controls.Add(Me.lblSourceMandant)
		Me.pnlMandant.Dock = System.Windows.Forms.DockStyle.Top
		Me.pnlMandant.Location = New System.Drawing.Point(0, 110)
		Me.pnlMandant.Name = "pnlMandant"
		Me.pnlMandant.Size = New System.Drawing.Size(1334, 58)
		Me.pnlMandant.TabIndex = 6
		'
		'pnlUpdateAddButton
		'
		Me.pnlUpdateAddButton.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
		Me.pnlUpdateAddButton.Controls.Add(Me.btnNewData)
		Me.pnlUpdateAddButton.Controls.Add(Me.btnUpdateData)
		Me.pnlUpdateAddButton.Location = New System.Drawing.Point(689, 12)
		Me.pnlUpdateAddButton.Name = "pnlUpdateAddButton"
		Me.pnlUpdateAddButton.Size = New System.Drawing.Size(203, 35)
		Me.pnlUpdateAddButton.TabIndex = 331
		'
		'btnNewData
		'
		Me.btnNewData.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnNewData.ImageOptions.Image = Global.SPS.Export.Listing.Utility.My.Resources.Resources.addnewdatasource_16x16
		Me.btnNewData.Location = New System.Drawing.Point(105, 0)
		Me.btnNewData.Name = "btnNewData"
		Me.btnNewData.Size = New System.Drawing.Size(98, 33)
		Me.btnNewData.TabIndex = 330
		Me.btnNewData.Text = "Hinzufügen"
		'
		'btnUpdateData
		'
		Me.btnUpdateData.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnUpdateData.ImageOptions.Image = Global.SPS.Export.Listing.Utility.My.Resources.Resources.refreshpivottable_16x16
		Me.btnUpdateData.Location = New System.Drawing.Point(0, 0)
		Me.btnUpdateData.Name = "btnUpdateData"
		Me.btnUpdateData.Size = New System.Drawing.Size(98, 33)
		Me.btnUpdateData.TabIndex = 329
		Me.btnUpdateData.Text = "Überschreiben"
		'
		'tgsSourceSelection
		'
		Me.tgsSourceSelection.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.tgsSourceSelection.EditValue = True
		Me.tgsSourceSelection.Location = New System.Drawing.Point(884, 17)
		Me.tgsSourceSelection.Name = "tgsSourceSelection"
		Me.tgsSourceSelection.Properties.AllowFocused = False
		Me.tgsSourceSelection.Properties.Appearance.Options.UseTextOptions = True
		Me.tgsSourceSelection.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.tgsSourceSelection.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.tgsSourceSelection.Properties.OffText = "Alles abgewählt"
		Me.tgsSourceSelection.Properties.OnText = "Alles ausgewählt"
		Me.tgsSourceSelection.Size = New System.Drawing.Size(214, 18)
		Me.tgsSourceSelection.TabIndex = 321
		'
		'btnImportData
		'
		Me.btnImportData.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnImportData.ImageOptions.Image = Global.SPS.Export.Listing.Utility.My.Resources.Resources.download_16x16
		Me.btnImportData.Location = New System.Drawing.Point(1209, 12)
		Me.btnImportData.Name = "btnImportData"
		Me.btnImportData.Size = New System.Drawing.Size(98, 33)
		Me.btnImportData.TabIndex = 328
		Me.btnImportData.Text = "Starten"
		'
		'lueSourceMD
		'
		Me.lueSourceMD.Location = New System.Drawing.Point(126, 18)
		Me.lueSourceMD.Name = "lueSourceMD"
		EditorButtonImageOptions2.Image = Global.SPS.Export.Listing.Utility.My.Resources.Resources.database_16x16
		Me.lueSourceMD.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, True, True, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", "0", Nothing, DevExpress.Utils.ToolTipAnchor.[Default]), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, True, True, False, EditorButtonImageOptions2, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject5, SerializableAppearanceObject6, SerializableAppearanceObject7, SerializableAppearanceObject8, "", "1", Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
		Me.lueSourceMD.Properties.NullText = ""
		Me.lueSourceMD.Size = New System.Drawing.Size(190, 24)
		Me.lueSourceMD.TabIndex = 326
		'
		'lblSourceMandant
		'
		Me.lblSourceMandant.Appearance.Options.UseTextOptions = True
		Me.lblSourceMandant.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblSourceMandant.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblSourceMandant.Location = New System.Drawing.Point(13, 21)
		Me.lblSourceMandant.Name = "lblSourceMandant"
		Me.lblSourceMandant.Size = New System.Drawing.Size(107, 13)
		Me.lblSourceMandant.TabIndex = 0
		Me.lblSourceMandant.Text = "Quell-Mandant"
		'
		'frmImportTwixData
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(1334, 593)
		Me.Controls.Add(Me.pnlMandant)
		Me.Controls.Add(Me.tileBar)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.Name = "frmImportTwixData"
		Me.Text = "frmImportTwixData"
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.pnlMandant, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pnlMandant.ResumeLayout(False)
		CType(Me.pnlUpdateAddButton, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pnlUpdateAddButton.ResumeLayout(False)
		CType(Me.tgsSourceSelection.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lueSourceMD.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub

	Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
	Friend WithEvents Bar4 As DevExpress.XtraBars.Bar
	Friend WithEvents bsiInfo As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents BarStaticItem2 As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
	Friend WithEvents BarStaticItem1 As DevExpress.XtraBars.BarStaticItem
	Private WithEvents tileBar As DevExpress.XtraBars.Navigation.TileBar
	Private WithEvents tileBarGroupTables As DevExpress.XtraBars.Navigation.TileBarGroup
	Private WithEvents employeesTileBarItem As DevExpress.XtraBars.Navigation.TileBarItem
	Private WithEvents customersTileBarItem As DevExpress.XtraBars.Navigation.TileBarItem
	Friend WithEvents pnlMandant As DevExpress.XtraEditors.PanelControl
	Friend WithEvents pnlUpdateAddButton As DevExpress.XtraEditors.PanelControl
	Friend WithEvents btnNewData As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents btnUpdateData As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents tgsSourceSelection As DevExpress.XtraEditors.ToggleSwitch
	Friend WithEvents btnImportData As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents lueSourceMD As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents lblSourceMandant As DevExpress.XtraEditors.LabelControl
End Class
