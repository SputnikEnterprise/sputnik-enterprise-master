Namespace UI

	<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
	Partial Class frmTelephonySetting
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
			Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTelephonySetting))
			Me.chkReplacePlusWithZero = New DevExpress.XtraEditors.CheckEdit()
			Me.lblamtkennziffer = New System.Windows.Forms.Label()
			Me.txtAmtsziffer = New DevExpress.XtraEditors.TextEdit()
			Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
			Me.Bar4 = New DevExpress.XtraBars.Bar()
			Me.bsiInfo = New DevExpress.XtraBars.BarStaticItem()
			Me.bbiSave = New DevExpress.XtraBars.BarButtonItem()
			Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
			Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
			Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
			Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
			Me.BarStaticItem1 = New DevExpress.XtraBars.BarStaticItem()
			Me.BarEditItem1 = New DevExpress.XtraBars.BarEditItem()
			Me.RepositoryItemMarqueeProgressBar1 = New DevExpress.XtraEditors.Repository.RepositoryItemMarqueeProgressBar()
			Me.RepositoryItemMarqueeProgressBar2 = New DevExpress.XtraEditors.Repository.RepositoryItemMarqueeProgressBar()
			Me.chkCreateAutoContact = New DevExpress.XtraEditors.CheckEdit()
			CType(Me.chkReplacePlusWithZero.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.txtAmtsziffer.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.RepositoryItemMarqueeProgressBar1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.RepositoryItemMarqueeProgressBar2, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.chkCreateAutoContact.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			'
			'chkReplacePlusWithZero
			'
			Me.chkReplacePlusWithZero.Location = New System.Drawing.Point(35, 61)
			Me.chkReplacePlusWithZero.Name = "chkReplacePlusWithZero"
			Me.chkReplacePlusWithZero.Properties.AllowFocused = False
			Me.chkReplacePlusWithZero.Properties.Caption = "(++ / +) durch „00"" ersetzen"
			Me.chkReplacePlusWithZero.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.chkReplacePlusWithZero.Size = New System.Drawing.Size(233, 19)
			Me.chkReplacePlusWithZero.TabIndex = 29
			'
			'lblamtkennziffer
			'
			Me.lblamtkennziffer.AutoSize = True
			Me.lblamtkennziffer.Location = New System.Drawing.Point(32, 38)
			Me.lblamtkennziffer.Name = "lblamtkennziffer"
			Me.lblamtkennziffer.Size = New System.Drawing.Size(170, 13)
			Me.lblamtkennziffer.TabIndex = 28
			Me.lblamtkennziffer.Text = "Amtskennziffer für Ortsgespräche"
			'
			'txtAmtsziffer
			'
			Me.txtAmtsziffer.Location = New System.Drawing.Point(217, 35)
			Me.txtAmtsziffer.Name = "txtAmtsziffer"
			Me.txtAmtsziffer.Size = New System.Drawing.Size(51, 20)
			Me.txtAmtsziffer.TabIndex = 27
			'
			'BarManager1
			'
			Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar4})
			Me.BarManager1.DockControls.Add(Me.barDockControlTop)
			Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
			Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
			Me.BarManager1.DockControls.Add(Me.barDockControlRight)
			Me.BarManager1.Form = Me
			Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiInfo, Me.BarStaticItem1, Me.BarEditItem1, Me.bbiSave})
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
			Me.Bar4.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiInfo), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiSave)})
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
			'bbiSave
			'
			Me.bbiSave.Caption = "Speichern"
			Me.bbiSave.Id = 15
			Me.bbiSave.ImageOptions.Image = CType(resources.GetObject("bbiSave.ImageOptions.Image"), System.Drawing.Image)
			Me.bbiSave.ImageOptions.LargeImage = CType(resources.GetObject("bbiSave.ImageOptions.LargeImage"), System.Drawing.Image)
			Me.bbiSave.Name = "bbiSave"
			Me.bbiSave.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
			'
			'barDockControlTop
			'
			Me.barDockControlTop.CausesValidation = False
			Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
			Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
			Me.barDockControlTop.Manager = Me.BarManager1
			Me.barDockControlTop.Size = New System.Drawing.Size(336, 0)
			'
			'barDockControlBottom
			'
			Me.barDockControlBottom.CausesValidation = False
			Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
			Me.barDockControlBottom.Location = New System.Drawing.Point(0, 141)
			Me.barDockControlBottom.Manager = Me.BarManager1
			Me.barDockControlBottom.Size = New System.Drawing.Size(336, 27)
			'
			'barDockControlLeft
			'
			Me.barDockControlLeft.CausesValidation = False
			Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
			Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
			Me.barDockControlLeft.Manager = Me.BarManager1
			Me.barDockControlLeft.Size = New System.Drawing.Size(0, 141)
			'
			'barDockControlRight
			'
			Me.barDockControlRight.CausesValidation = False
			Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
			Me.barDockControlRight.Location = New System.Drawing.Point(336, 0)
			Me.barDockControlRight.Manager = Me.BarManager1
			Me.barDockControlRight.Size = New System.Drawing.Size(0, 141)
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
			'chkCreateAutoContact
			'
			Me.chkCreateAutoContact.Location = New System.Drawing.Point(35, 86)
			Me.chkCreateAutoContact.Name = "chkCreateAutoContact"
			Me.chkCreateAutoContact.Properties.AllowFocused = False
			Me.chkCreateAutoContact.Properties.Caption = "Kontakteintrag automatisch eintragen"
			Me.chkCreateAutoContact.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.chkCreateAutoContact.Size = New System.Drawing.Size(233, 19)
			Me.chkCreateAutoContact.TabIndex = 34
			'
			'frmTelephonySetting
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(336, 168)
			Me.Controls.Add(Me.chkCreateAutoContact)
			Me.Controls.Add(Me.chkReplacePlusWithZero)
			Me.Controls.Add(Me.lblamtkennziffer)
			Me.Controls.Add(Me.txtAmtsziffer)
			Me.Controls.Add(Me.barDockControlLeft)
			Me.Controls.Add(Me.barDockControlRight)
			Me.Controls.Add(Me.barDockControlBottom)
			Me.Controls.Add(Me.barDockControlTop)
			Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
			Me.MaximizeBox = False
			Me.MinimizeBox = False
			Me.Name = "frmTelephonySetting"
			Me.Text = "Einstellungen über Telefonie"
			CType(Me.chkReplacePlusWithZero.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.txtAmtsziffer.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.RepositoryItemMarqueeProgressBar1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.RepositoryItemMarqueeProgressBar2, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.chkCreateAutoContact.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)
			Me.PerformLayout()

		End Sub

		Friend WithEvents chkReplacePlusWithZero As DevExpress.XtraEditors.CheckEdit
		Friend WithEvents lblamtkennziffer As Label
		Friend WithEvents txtAmtsziffer As DevExpress.XtraEditors.TextEdit
		Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
		Friend WithEvents Bar4 As DevExpress.XtraBars.Bar
		Friend WithEvents bsiInfo As DevExpress.XtraBars.BarStaticItem
		Friend WithEvents bbiSave As DevExpress.XtraBars.BarButtonItem
		Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
		Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
		Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
		Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
		Friend WithEvents BarStaticItem1 As DevExpress.XtraBars.BarStaticItem
		Friend WithEvents BarEditItem1 As DevExpress.XtraBars.BarEditItem
		Friend WithEvents RepositoryItemMarqueeProgressBar1 As DevExpress.XtraEditors.Repository.RepositoryItemMarqueeProgressBar
		Friend WithEvents RepositoryItemMarqueeProgressBar2 As DevExpress.XtraEditors.Repository.RepositoryItemMarqueeProgressBar
		Friend WithEvents chkCreateAutoContact As DevExpress.XtraEditors.CheckEdit
	End Class

End Namespace
