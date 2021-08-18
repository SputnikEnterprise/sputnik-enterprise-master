<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLOLRJSearch
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLOLRJSearch))
		Me.XtraTabControl1 = New DevExpress.XtraTab.XtraTabControl()
		Me.xtabAllgemein = New DevExpress.XtraTab.XtraTabPage()
		Me.Cbo_JahrVon = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.lblJahr = New System.Windows.Forms.Label()
		Me.lueMandant = New DevExpress.XtraEditors.LookUpEdit()
		Me.lblMDName = New System.Windows.Forms.Label()
		Me.xtabSQLQuery = New DevExpress.XtraTab.XtraTabPage()
		Me.txt_SQLQuery = New DevExpress.XtraEditors.MemoEdit()
		Me.lblSQLAbfrage = New System.Windows.Forms.Label()
		Me.lblPeriode = New System.Windows.Forms.Label()
		Me.Cbo_Periode = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
		Me.lblHeaderNormal = New System.Windows.Forms.Label()
		Me.CmdClose = New DevExpress.XtraEditors.SimpleButton()
		Me.Label5 = New System.Windows.Forms.Label()
		Me.lblHeaderFett = New System.Windows.Forms.Label()
		Me.Bar4 = New DevExpress.XtraBars.Bar()
		Me.beiWorking = New DevExpress.XtraBars.BarEditItem()
		Me.RepositoryItemMarqueeProgressBar2 = New DevExpress.XtraEditors.Repository.RepositoryItemMarqueeProgressBar()
		Me.bsiInfo = New DevExpress.XtraBars.BarStaticItem()
		Me.bbiSearch = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiClearFields = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiPrint = New DevExpress.XtraBars.BarLargeButtonItem()
		Me.bbiExport = New DevExpress.XtraBars.BarLargeButtonItem()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
		Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
		Me.BarStaticItem1 = New DevExpress.XtraBars.BarStaticItem()
		Me.BarEditItem1 = New DevExpress.XtraBars.BarEditItem()
		Me.RepositoryItemMarqueeProgressBar1 = New DevExpress.XtraEditors.Repository.RepositoryItemMarqueeProgressBar()
		Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
		CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.XtraTabControl1.SuspendLayout()
		Me.xtabAllgemein.SuspendLayout()
		CType(Me.Cbo_JahrVon.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.xtabSQLQuery.SuspendLayout()
		CType(Me.txt_SQLQuery.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.Cbo_Periode.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.PanelControl1.SuspendLayout()
		CType(Me.RepositoryItemMarqueeProgressBar2, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.RepositoryItemMarqueeProgressBar1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'XtraTabControl1
		'
		Me.XtraTabControl1.Location = New System.Drawing.Point(28, 110)
		Me.XtraTabControl1.Name = "XtraTabControl1"
		Me.XtraTabControl1.SelectedTabPage = Me.xtabAllgemein
		Me.XtraTabControl1.Size = New System.Drawing.Size(596, 260)
		Me.XtraTabControl1.TabIndex = 310
		Me.XtraTabControl1.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabAllgemein, Me.xtabSQLQuery})
		'
		'xtabAllgemein
		'
		Me.xtabAllgemein.Controls.Add(Me.Cbo_JahrVon)
		Me.xtabAllgemein.Controls.Add(Me.lblJahr)
		Me.xtabAllgemein.Controls.Add(Me.lueMandant)
		Me.xtabAllgemein.Controls.Add(Me.lblMDName)
		Me.xtabAllgemein.Name = "xtabAllgemein"
		Me.xtabAllgemein.Size = New System.Drawing.Size(590, 232)
		Me.xtabAllgemein.Text = "Allgemein"
		'
		'Cbo_JahrVon
		'
		Me.Cbo_JahrVon.Location = New System.Drawing.Point(158, 67)
		Me.Cbo_JahrVon.Name = "Cbo_JahrVon"
		Me.Cbo_JahrVon.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.DropDown)})
		Me.Cbo_JahrVon.Size = New System.Drawing.Size(96, 20)
		Me.Cbo_JahrVon.TabIndex = 304
		'
		'lblJahr
		'
		Me.lblJahr.Location = New System.Drawing.Point(22, 70)
		Me.lblJahr.Name = "lblJahr"
		Me.lblJahr.Size = New System.Drawing.Size(130, 13)
		Me.lblJahr.TabIndex = 307
		Me.lblJahr.Text = "Jahr"
		Me.lblJahr.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lueMandant
		'
		Me.lueMandant.Location = New System.Drawing.Point(158, 29)
		Me.lueMandant.Name = "lueMandant"
		Me.lueMandant.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.lueMandant.Properties.NullText = ""
		Me.lueMandant.Size = New System.Drawing.Size(384, 20)
		Me.lueMandant.TabIndex = 295
		'
		'lblMDName
		'
		Me.lblMDName.Location = New System.Drawing.Point(13, 33)
		Me.lblMDName.Name = "lblMDName"
		Me.lblMDName.Size = New System.Drawing.Size(139, 13)
		Me.lblMDName.TabIndex = 296
		Me.lblMDName.Text = "Mandanten"
		Me.lblMDName.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'xtabSQLQuery
		'
		Me.xtabSQLQuery.Controls.Add(Me.txt_SQLQuery)
		Me.xtabSQLQuery.Controls.Add(Me.lblSQLAbfrage)
		Me.xtabSQLQuery.Name = "xtabSQLQuery"
		Me.xtabSQLQuery.Size = New System.Drawing.Size(590, 232)
		Me.xtabSQLQuery.Text = "SQL-Abfrage"
		'
		'txt_SQLQuery
		'
		Me.txt_SQLQuery.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txt_SQLQuery.Location = New System.Drawing.Point(22, 38)
		Me.txt_SQLQuery.Name = "txt_SQLQuery"
		Me.txt_SQLQuery.Size = New System.Drawing.Size(550, 177)
		Me.txt_SQLQuery.TabIndex = 101
		Me.txt_SQLQuery.UseOptimizedRendering = True
		'
		'lblSQLAbfrage
		'
		Me.lblSQLAbfrage.AutoSize = True
		Me.lblSQLAbfrage.Location = New System.Drawing.Point(19, 22)
		Me.lblSQLAbfrage.Name = "lblSQLAbfrage"
		Me.lblSQLAbfrage.Size = New System.Drawing.Size(150, 13)
		Me.lblSQLAbfrage.TabIndex = 73
		Me.lblSQLAbfrage.Text = "Ihre derzeitige Abfrage lautet"
		'
		'lblPeriode
		'
		Me.lblPeriode.Location = New System.Drawing.Point(3, -26)
		Me.lblPeriode.Name = "lblPeriode"
		Me.lblPeriode.Size = New System.Drawing.Size(130, 13)
		Me.lblPeriode.TabIndex = 308
		Me.lblPeriode.Text = "Periode"
		Me.lblPeriode.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'Cbo_Periode
		'
		Me.Cbo_Periode.Location = New System.Drawing.Point(139, -29)
		Me.Cbo_Periode.Name = "Cbo_Periode"
		Me.Cbo_Periode.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.DropDown)})
		Me.Cbo_Periode.Size = New System.Drawing.Size(384, 20)
		Me.Cbo_Periode.TabIndex = 301
		'
		'PanelControl1
		'
		Me.PanelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003
		Me.PanelControl1.Controls.Add(Me.lblHeaderNormal)
		Me.PanelControl1.Controls.Add(Me.CmdClose)
		Me.PanelControl1.Controls.Add(Me.Label5)
		Me.PanelControl1.Controls.Add(Me.lblHeaderFett)
		Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Top
		Me.PanelControl1.Location = New System.Drawing.Point(0, 0)
		Me.PanelControl1.Name = "PanelControl1"
		Me.PanelControl1.Size = New System.Drawing.Size(654, 77)
		Me.PanelControl1.TabIndex = 309
		'
		'lblHeaderNormal
		'
		Me.lblHeaderNormal.AutoSize = True
		Me.lblHeaderNormal.Location = New System.Drawing.Point(110, 47)
		Me.lblHeaderNormal.Name = "lblHeaderNormal"
		Me.lblHeaderNormal.Size = New System.Drawing.Size(234, 13)
		Me.lblHeaderNormal.TabIndex = 1
		Me.lblHeaderNormal.Text = "Geben Sie bitte Ihre gewünschten Kriterien ein."
		'
		'CmdClose
		'
		Me.CmdClose.Location = New System.Drawing.Point(519, 21)
		Me.CmdClose.Name = "CmdClose"
		Me.CmdClose.Size = New System.Drawing.Size(100, 25)
		Me.CmdClose.TabIndex = 204
		Me.CmdClose.Text = "Schliessen"
		'
		'Label5
		'
		Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label5.ForeColor = System.Drawing.SystemColors.HotTrack
		Me.Label5.Image = CType(resources.GetObject("Label5.Image"), System.Drawing.Image)
		Me.Label5.Location = New System.Drawing.Point(5, 8)
		Me.Label5.Name = "Label5"
		Me.Label5.Size = New System.Drawing.Size(83, 65)
		Me.Label5.TabIndex = 1000
		'
		'lblHeaderFett
		'
		Me.lblHeaderFett.AutoSize = True
		Me.lblHeaderFett.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblHeaderFett.Location = New System.Drawing.Point(94, 21)
		Me.lblHeaderFett.Name = "lblHeaderFett"
		Me.lblHeaderFett.Size = New System.Drawing.Size(186, 13)
		Me.lblHeaderFett.TabIndex = 0
		Me.lblHeaderFett.Text = "Lohnartenrekapitulation jährlich"
		'
		'Bar4
		'
		Me.Bar4.BarName = "Statusleiste"
		Me.Bar4.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
		Me.Bar4.DockCol = 0
		Me.Bar4.DockRow = 0
		Me.Bar4.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
		Me.Bar4.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.beiWorking), New DevExpress.XtraBars.LinkPersistInfo(Me.bsiInfo), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiSearch), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiClearFields, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiPrint), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiExport, True)})
		Me.Bar4.OptionsBar.AllowQuickCustomization = False
		Me.Bar4.OptionsBar.DrawDragBorder = False
		Me.Bar4.OptionsBar.UseWholeRow = True
		Me.Bar4.Text = "Statusleiste"
		'
		'beiWorking
		'
		Me.beiWorking.Caption = "BarEditItem2"
		Me.beiWorking.Edit = Me.RepositoryItemMarqueeProgressBar2
		Me.beiWorking.Id = 11
		Me.beiWorking.Name = "beiWorking"
		'
		'RepositoryItemMarqueeProgressBar2
		'
		Me.RepositoryItemMarqueeProgressBar2.Name = "RepositoryItemMarqueeProgressBar2"
		'
		'bsiInfo
		'
		Me.bsiInfo.AutoSize = DevExpress.XtraBars.BarStaticItemSize.Spring
		Me.bsiInfo.Caption = "Bereit"
		Me.bsiInfo.Id = 0
		Me.bsiInfo.Name = "bsiInfo"
		Me.bsiInfo.TextAlignment = System.Drawing.StringAlignment.Near
		Me.bsiInfo.Width = 32
		'
		'bbiSearch
		'
		Me.bbiSearch.Caption = "Suchen"
		Me.bbiSearch.Glyph = CType(resources.GetObject("bbiSearch.Glyph"), System.Drawing.Image)
		Me.bbiSearch.Id = 1
		Me.bbiSearch.Name = "bbiSearch"
		Me.bbiSearch.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiClearFields
		'
		Me.bbiClearFields.Caption = "Felder leeren"
		Me.bbiClearFields.Glyph = CType(resources.GetObject("bbiClearFields.Glyph"), System.Drawing.Image)
		Me.bbiClearFields.Id = 8
		Me.bbiClearFields.Name = "bbiClearFields"
		Me.bbiClearFields.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiPrint
		'
		Me.bbiPrint.AllowAllUp = True
		Me.bbiPrint.Caption = "Drucken"
		Me.bbiPrint.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Right
		Me.bbiPrint.Glyph = CType(resources.GetObject("bbiPrint.Glyph"), System.Drawing.Image)
		Me.bbiPrint.Id = 6
		Me.bbiPrint.Name = "bbiPrint"
		Me.bbiPrint.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiExport
		'
		Me.bbiExport.Caption = "Exportieren"
		Me.bbiExport.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Right
		Me.bbiExport.Glyph = CType(resources.GetObject("bbiExport.Glyph"), System.Drawing.Image)
		Me.bbiExport.Id = 4
		Me.bbiExport.Name = "bbiExport"
		Me.bbiExport.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'BarManager1
		'
		Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar4})
		Me.BarManager1.DockControls.Add(Me.barDockControlTop)
		Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
		Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
		Me.BarManager1.DockControls.Add(Me.barDockControlRight)
		Me.BarManager1.Form = Me
		Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiInfo, Me.bbiSearch, Me.bbiPrint, Me.bbiClearFields, Me.BarStaticItem1, Me.bbiExport, Me.BarEditItem1, Me.beiWorking})
		Me.BarManager1.MaxItemId = 12
		Me.BarManager1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemMarqueeProgressBar1, Me.RepositoryItemMarqueeProgressBar2})
		Me.BarManager1.StatusBar = Me.Bar4
		'
		'barDockControlTop
		'
		Me.barDockControlTop.CausesValidation = False
		Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
		Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlTop.Size = New System.Drawing.Size(654, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 410)
		Me.barDockControlBottom.Size = New System.Drawing.Size(654, 27)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 410)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(654, 0)
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 410)
		'
		'BarStaticItem1
		'
		Me.BarStaticItem1.Caption = " "
		Me.BarStaticItem1.Id = 5
		Me.BarStaticItem1.Name = "BarStaticItem1"
		Me.BarStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near
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
		'BackgroundWorker1
		'
		'
		'frmLOLRJSearch
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(654, 437)
		Me.Controls.Add(Me.XtraTabControl1)
		Me.Controls.Add(Me.lblPeriode)
		Me.Controls.Add(Me.Cbo_Periode)
		Me.Controls.Add(Me.PanelControl1)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.MaximumSize = New System.Drawing.Size(670, 475)
		Me.MinimumSize = New System.Drawing.Size(670, 475)
		Me.Name = "frmLOLRJSearch"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Jährliche Lohnlisten: Lohnartenrekapitulation"
		CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.XtraTabControl1.ResumeLayout(False)
		Me.xtabAllgemein.ResumeLayout(False)
		CType(Me.Cbo_JahrVon.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.xtabSQLQuery.ResumeLayout(False)
		Me.xtabSQLQuery.PerformLayout()
		CType(Me.txt_SQLQuery.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.Cbo_Periode.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.PanelControl1.ResumeLayout(False)
		Me.PanelControl1.PerformLayout()
		CType(Me.RepositoryItemMarqueeProgressBar2, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.RepositoryItemMarqueeProgressBar1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents XtraTabControl1 As DevExpress.XtraTab.XtraTabControl
	Friend WithEvents xtabAllgemein As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents lueMandant As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents lblMDName As System.Windows.Forms.Label
	Friend WithEvents xtabSQLQuery As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents txt_SQLQuery As DevExpress.XtraEditors.MemoEdit
	Friend WithEvents lblSQLAbfrage As System.Windows.Forms.Label
	Friend WithEvents lblPeriode As System.Windows.Forms.Label
	Friend WithEvents Cbo_Periode As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents Cbo_JahrVon As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents lblJahr As System.Windows.Forms.Label
	Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
	Friend WithEvents lblHeaderNormal As System.Windows.Forms.Label
	Friend WithEvents CmdClose As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents Label5 As System.Windows.Forms.Label
	Friend WithEvents lblHeaderFett As System.Windows.Forms.Label
	Friend WithEvents Bar4 As DevExpress.XtraBars.Bar
	Friend WithEvents bsiInfo As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents bbiSearch As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbiClearFields As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbiPrint As DevExpress.XtraBars.BarLargeButtonItem
	Friend WithEvents bbiExport As DevExpress.XtraBars.BarLargeButtonItem
	Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
	Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
	Friend WithEvents BarStaticItem1 As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
	Friend WithEvents beiWorking As DevExpress.XtraBars.BarEditItem
	Friend WithEvents RepositoryItemMarqueeProgressBar2 As DevExpress.XtraEditors.Repository.RepositoryItemMarqueeProgressBar
	Friend WithEvents BarEditItem1 As DevExpress.XtraBars.BarEditItem
	Friend WithEvents RepositoryItemMarqueeProgressBar1 As DevExpress.XtraEditors.Repository.RepositoryItemMarqueeProgressBar

End Class
