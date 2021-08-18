<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDoc2eCall
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDoc2eCall))
    Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
    Me.GroupBox1 = New DevExpress.XtraEditors.PanelControl()
    Me.lblHeader2 = New DevExpress.XtraEditors.LabelControl()
    Me.LblSetting = New System.Windows.Forms.Label()
    Me.lblHeader1 = New DevExpress.XtraEditors.LabelControl()
    Me.CmdClose = New DevExpress.XtraEditors.SimpleButton()
    Me.lblDatei = New DevExpress.XtraEditors.LabelControl()
    Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
    Me.Bar4 = New DevExpress.XtraBars.Bar()
    Me.bsiInfo = New DevExpress.XtraBars.BarStaticItem()
    Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
    Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
    Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
    Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
    Me.BarStaticItem1 = New DevExpress.XtraBars.BarStaticItem()
    Me.cmdOK = New DevExpress.XtraEditors.SimpleButton()
    Me.txtFilename = New DevExpress.XtraEditors.ComboBoxEdit()
    Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
    Me.XtraTabControl1 = New DevExpress.XtraTab.XtraTabControl()
    Me.xtabVersanddata = New DevExpress.XtraTab.XtraTabPage()
    Me.SplitContainerControl1 = New DevExpress.XtraEditors.SplitContainerControl()
    Me.PanelControl2 = New DevExpress.XtraEditors.PanelControl()
    Me.chkSendZhd = New DevExpress.XtraEditors.CheckEdit()
    Me.chkSendKD = New DevExpress.XtraEditors.CheckEdit()
    Me.grdContent4ImportIntoSPDb = New DevExpress.XtraGrid.GridControl()
    Me.gvContent4InsertIntoSPDb = New DevExpress.XtraGrid.Views.Grid.GridView()
    Me.xtabResultData = New DevExpress.XtraTab.XtraTabPage()
    Me.PanelControl3 = New DevExpress.XtraEditors.PanelControl()
    Me.lstVersandResult = New DevExpress.XtraEditors.ListBoxControl()
    Me.chkSendOne = New DevExpress.XtraEditors.CheckEdit()
    Me.cmdLoad = New DevExpress.XtraEditors.SimpleButton()
    CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.GroupBox1.SuspendLayout()
    CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txtFilename.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.PanelControl1.SuspendLayout()
    CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.XtraTabControl1.SuspendLayout()
    Me.xtabVersanddata.SuspendLayout()
    CType(Me.SplitContainerControl1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SplitContainerControl1.SuspendLayout()
    CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.PanelControl2.SuspendLayout()
    CType(Me.chkSendZhd.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.chkSendKD.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.grdContent4ImportIntoSPDb, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.gvContent4InsertIntoSPDb, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.xtabResultData.SuspendLayout()
    CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.PanelControl3.SuspendLayout()
    CType(Me.lstVersandResult, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.chkSendOne.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'GroupBox1
    '
    Me.GroupBox1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003
    Me.GroupBox1.Controls.Add(Me.lblHeader2)
    Me.GroupBox1.Controls.Add(Me.LblSetting)
    Me.GroupBox1.Controls.Add(Me.lblHeader1)
    Me.GroupBox1.Controls.Add(Me.CmdClose)
    Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
    Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
    Me.GroupBox1.Name = "GroupBox1"
    Me.GroupBox1.Size = New System.Drawing.Size(721, 59)
    Me.GroupBox1.TabIndex = 228
    '
    'lblHeader2
    '
    Me.lblHeader2.AllowHtmlString = True
    Me.lblHeader2.Location = New System.Drawing.Point(78, 31)
    Me.lblHeader2.Name = "lblHeader2"
    Me.lblHeader2.Size = New System.Drawing.Size(218, 13)
    Me.lblHeader2.TabIndex = 214
    Me.lblHeader2.Text = "Wählen Sie Ihre gewünschten Kriterien aus..."
    '
    'LblSetting
    '
    Me.LblSetting.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.LblSetting.ForeColor = System.Drawing.SystemColors.HotTrack
    Me.LblSetting.Image = CType(resources.GetObject("LblSetting.Image"), System.Drawing.Image)
    Me.LblSetting.Location = New System.Drawing.Point(12, 9)
    Me.LblSetting.Name = "LblSetting"
    Me.LblSetting.Size = New System.Drawing.Size(51, 38)
    Me.LblSetting.TabIndex = 213
    '
    'lblHeader1
    '
    Me.lblHeader1.AllowHtmlString = True
    Me.lblHeader1.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
    Me.lblHeader1.Location = New System.Drawing.Point(69, 12)
    Me.lblHeader1.Name = "lblHeader1"
    Me.lblHeader1.Size = New System.Drawing.Size(184, 13)
    Me.lblHeader1.TabIndex = 213
    Me.lblHeader1.Text = "Export der Daten für FAX-Mailing"
    '
    'CmdClose
    '
    Me.CmdClose.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.CmdClose.Location = New System.Drawing.Point(569, 12)
    Me.CmdClose.Name = "CmdClose"
    Me.CmdClose.Size = New System.Drawing.Size(94, 25)
    Me.CmdClose.TabIndex = 204
    Me.CmdClose.Text = "Schliessen"
    '
    'lblDatei
    '
    Me.lblDatei.AllowHtmlString = True
    Me.lblDatei.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    Me.lblDatei.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
    Me.lblDatei.Location = New System.Drawing.Point(35, 104)
    Me.lblDatei.Name = "lblDatei"
    Me.lblDatei.Size = New System.Drawing.Size(111, 13)
    Me.lblDatei.TabIndex = 234
    Me.lblDatei.Text = "Dateiname"
    '
    'BarManager1
    '
    Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar4})
    Me.BarManager1.DockControls.Add(Me.barDockControlTop)
    Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
    Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
    Me.BarManager1.DockControls.Add(Me.barDockControlRight)
    Me.BarManager1.Form = Me
    Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiInfo, Me.BarStaticItem1})
    Me.BarManager1.MaxItemId = 10
    Me.BarManager1.StatusBar = Me.Bar4
    '
    'Bar4
    '
    Me.Bar4.BarName = "Statusleiste"
    Me.Bar4.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
    Me.Bar4.DockCol = 0
    Me.Bar4.DockRow = 0
    Me.Bar4.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
    Me.Bar4.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiInfo)})
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
    Me.bsiInfo.TextAlignment = System.Drawing.StringAlignment.Near
    Me.bsiInfo.Width = 32
    '
    'barDockControlTop
    '
    Me.barDockControlTop.CausesValidation = False
    Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
    Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
    Me.barDockControlTop.Size = New System.Drawing.Size(721, 0)
    '
    'barDockControlBottom
    '
    Me.barDockControlBottom.CausesValidation = False
    Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
    Me.barDockControlBottom.Location = New System.Drawing.Point(0, 578)
    Me.barDockControlBottom.Size = New System.Drawing.Size(721, 25)
    '
    'barDockControlLeft
    '
    Me.barDockControlLeft.CausesValidation = False
    Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
    Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
    Me.barDockControlLeft.Size = New System.Drawing.Size(0, 578)
    '
    'barDockControlRight
    '
    Me.barDockControlRight.CausesValidation = False
    Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
    Me.barDockControlRight.Location = New System.Drawing.Point(721, 0)
    Me.barDockControlRight.Size = New System.Drawing.Size(0, 578)
    '
    'BarStaticItem1
    '
    Me.BarStaticItem1.Caption = " "
    Me.BarStaticItem1.Id = 5
    Me.BarStaticItem1.Name = "BarStaticItem1"
    Me.BarStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near
    '
    'cmdOK
    '
    Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmdOK.Location = New System.Drawing.Point(491, 13)
    Me.cmdOK.Name = "cmdOK"
    Me.cmdOK.Size = New System.Drawing.Size(131, 25)
    Me.cmdOK.TabIndex = 235
    Me.cmdOK.Text = "Fax-Nachricht senden"
    '
    'txtFilename
    '
    Me.txtFilename.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtFilename.Location = New System.Drawing.Point(152, 100)
    Me.txtFilename.Name = "txtFilename"
    Me.txtFilename.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, CType(resources.GetObject("txtFilename.Properties.Buttons"), System.Drawing.Image), New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, "", Nothing, Nothing, True)})
    Me.txtFilename.Size = New System.Drawing.Size(386, 22)
    Me.txtFilename.TabIndex = 233
    '
    'PanelControl1
    '
    Me.PanelControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.PanelControl1.Controls.Add(Me.XtraTabControl1)
    Me.PanelControl1.Location = New System.Drawing.Point(23, 146)
    Me.PanelControl1.Name = "PanelControl1"
    Me.PanelControl1.Padding = New System.Windows.Forms.Padding(10)
    Me.PanelControl1.Size = New System.Drawing.Size(676, 416)
    Me.PanelControl1.TabIndex = 236
    '
    'XtraTabControl1
    '
    Me.XtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.XtraTabControl1.Location = New System.Drawing.Point(12, 12)
    Me.XtraTabControl1.Name = "XtraTabControl1"
    Me.XtraTabControl1.SelectedTabPage = Me.xtabVersanddata
    Me.XtraTabControl1.Size = New System.Drawing.Size(652, 392)
    Me.XtraTabControl1.TabIndex = 157
    Me.XtraTabControl1.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabVersanddata, Me.xtabResultData})
    '
    'xtabVersanddata
    '
    Me.xtabVersanddata.Controls.Add(Me.SplitContainerControl1)
    Me.xtabVersanddata.Name = "xtabVersanddata"
    Me.xtabVersanddata.Size = New System.Drawing.Size(646, 364)
    Me.xtabVersanddata.Text = "Versand-Daten"
    '
    'SplitContainerControl1
    '
    Me.SplitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.SplitContainerControl1.Horizontal = False
    Me.SplitContainerControl1.Location = New System.Drawing.Point(0, 0)
    Me.SplitContainerControl1.Name = "SplitContainerControl1"
    Me.SplitContainerControl1.Panel1.Controls.Add(Me.PanelControl2)
    Me.SplitContainerControl1.Panel1.Padding = New System.Windows.Forms.Padding(5)
    Me.SplitContainerControl1.Panel1.Text = "Panel1"
    Me.SplitContainerControl1.Panel2.Controls.Add(Me.grdContent4ImportIntoSPDb)
    Me.SplitContainerControl1.Panel2.Padding = New System.Windows.Forms.Padding(5)
    Me.SplitContainerControl1.Panel2.Text = "Panel2"
    Me.SplitContainerControl1.Size = New System.Drawing.Size(646, 364)
    Me.SplitContainerControl1.TabIndex = 156
    Me.SplitContainerControl1.Text = "SplitContainerControl1"
    '
    'PanelControl2
    '
    Me.PanelControl2.Controls.Add(Me.chkSendZhd)
    Me.PanelControl2.Controls.Add(Me.chkSendKD)
    Me.PanelControl2.Controls.Add(Me.cmdOK)
    Me.PanelControl2.Dock = System.Windows.Forms.DockStyle.Fill
    Me.PanelControl2.Location = New System.Drawing.Point(5, 5)
    Me.PanelControl2.Name = "PanelControl2"
    Me.PanelControl2.Size = New System.Drawing.Size(636, 90)
    Me.PanelControl2.TabIndex = 0
    '
    'chkSendZhd
    '
    Me.chkSendZhd.Location = New System.Drawing.Point(13, 36)
    Me.chkSendZhd.MenuManager = Me.BarManager1
    Me.chkSendZhd.Name = "chkSendZhd"
    Me.chkSendZhd.Properties.Caption = "An Zhd.-Telefaxnummer senden"
    Me.chkSendZhd.Size = New System.Drawing.Size(367, 19)
    Me.chkSendZhd.TabIndex = 237
    '
    'chkSendKD
    '
    Me.chkSendKD.Location = New System.Drawing.Point(13, 11)
    Me.chkSendKD.MenuManager = Me.BarManager1
    Me.chkSendKD.Name = "chkSendKD"
    Me.chkSendKD.Properties.Caption = "An Kunden-Telefaxnummer senden"
    Me.chkSendKD.Size = New System.Drawing.Size(367, 19)
    Me.chkSendKD.TabIndex = 0
    '
    'grdContent4ImportIntoSPDb
    '
    Me.grdContent4ImportIntoSPDb.Dock = System.Windows.Forms.DockStyle.Fill
    Me.grdContent4ImportIntoSPDb.Location = New System.Drawing.Point(5, 5)
    Me.grdContent4ImportIntoSPDb.MainView = Me.gvContent4InsertIntoSPDb
    Me.grdContent4ImportIntoSPDb.Name = "grdContent4ImportIntoSPDb"
    Me.grdContent4ImportIntoSPDb.Size = New System.Drawing.Size(636, 249)
    Me.grdContent4ImportIntoSPDb.TabIndex = 155
    Me.grdContent4ImportIntoSPDb.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvContent4InsertIntoSPDb})
    '
    'gvContent4InsertIntoSPDb
    '
    Me.gvContent4InsertIntoSPDb.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
    Me.gvContent4InsertIntoSPDb.GridControl = Me.grdContent4ImportIntoSPDb
    Me.gvContent4InsertIntoSPDb.Name = "gvContent4InsertIntoSPDb"
    Me.gvContent4InsertIntoSPDb.OptionsSelection.EnableAppearanceFocusedCell = False
    Me.gvContent4InsertIntoSPDb.OptionsView.ShowAutoFilterRow = True
    Me.gvContent4InsertIntoSPDb.OptionsView.ShowGroupPanel = False
    '
    'xtabResultData
    '
    Me.xtabResultData.Controls.Add(Me.PanelControl3)
    Me.xtabResultData.Name = "xtabResultData"
    Me.xtabResultData.Padding = New System.Windows.Forms.Padding(5)
    Me.xtabResultData.Size = New System.Drawing.Size(646, 364)
    Me.xtabResultData.Text = "Versand-Ergebnis"
    '
    'PanelControl3
    '
    Me.PanelControl3.Controls.Add(Me.lstVersandResult)
    Me.PanelControl3.Dock = System.Windows.Forms.DockStyle.Fill
    Me.PanelControl3.Location = New System.Drawing.Point(5, 5)
    Me.PanelControl3.Name = "PanelControl3"
    Me.PanelControl3.Padding = New System.Windows.Forms.Padding(5)
    Me.PanelControl3.Size = New System.Drawing.Size(636, 354)
    Me.PanelControl3.TabIndex = 0
    '
    'lstVersandResult
    '
    Me.lstVersandResult.Dock = System.Windows.Forms.DockStyle.Fill
    Me.lstVersandResult.Location = New System.Drawing.Point(7, 7)
    Me.lstVersandResult.Name = "lstVersandResult"
    Me.lstVersandResult.Size = New System.Drawing.Size(622, 340)
    Me.lstVersandResult.TabIndex = 0
    '
    'chkSendOne
    '
    Me.chkSendOne.EditValue = True
    Me.chkSendOne.Location = New System.Drawing.Point(53, 65)
    Me.chkSendOne.MenuManager = Me.BarManager1
    Me.chkSendOne.Name = "chkSendOne"
    Me.chkSendOne.Properties.Caption = "Wenn die Nummer von Kunde und Zhd. gleich ist, nur einmal senden."
    Me.chkSendOne.Size = New System.Drawing.Size(485, 19)
    Me.chkSendOne.TabIndex = 238
    Me.chkSendOne.Visible = False
    '
    'cmdLoad
    '
    Me.cmdLoad.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmdLoad.Location = New System.Drawing.Point(583, 65)
    Me.cmdLoad.Name = "cmdLoad"
    Me.cmdLoad.Size = New System.Drawing.Size(94, 25)
    Me.cmdLoad.TabIndex = 236
    Me.cmdLoad.Text = "Daten laden"
    Me.cmdLoad.Visible = False
    '
    'frmDoc2eCall
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(721, 603)
    Me.Controls.Add(Me.chkSendOne)
    Me.Controls.Add(Me.PanelControl1)
    Me.Controls.Add(Me.lblDatei)
    Me.Controls.Add(Me.cmdLoad)
    Me.Controls.Add(Me.txtFilename)
    Me.Controls.Add(Me.GroupBox1)
    Me.Controls.Add(Me.barDockControlLeft)
    Me.Controls.Add(Me.barDockControlRight)
    Me.Controls.Add(Me.barDockControlBottom)
    Me.Controls.Add(Me.barDockControlTop)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Name = "frmDoc2eCall"
    Me.Text = "Versand der Daten für FAX-Mailing"
    CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.GroupBox1.ResumeLayout(False)
    Me.GroupBox1.PerformLayout()
    CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txtFilename.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.PanelControl1.ResumeLayout(False)
    CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.XtraTabControl1.ResumeLayout(False)
    Me.xtabVersanddata.ResumeLayout(False)
    CType(Me.SplitContainerControl1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.SplitContainerControl1.ResumeLayout(False)
    CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).EndInit()
    Me.PanelControl2.ResumeLayout(False)
    CType(Me.chkSendZhd.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.chkSendKD.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.grdContent4ImportIntoSPDb, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.gvContent4InsertIntoSPDb, System.ComponentModel.ISupportInitialize).EndInit()
    Me.xtabResultData.ResumeLayout(False)
    CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).EndInit()
    Me.PanelControl3.ResumeLayout(False)
    CType(Me.lstVersandResult, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.chkSendOne.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents GroupBox1 As DevExpress.XtraEditors.PanelControl
  Friend WithEvents lblHeader2 As DevExpress.XtraEditors.LabelControl
  Friend WithEvents LblSetting As System.Windows.Forms.Label
  Friend WithEvents lblHeader1 As DevExpress.XtraEditors.LabelControl
  Friend WithEvents CmdClose As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents lblDatei As DevExpress.XtraEditors.LabelControl
  Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
  Friend WithEvents Bar4 As DevExpress.XtraBars.Bar
  Friend WithEvents bsiInfo As DevExpress.XtraBars.BarStaticItem
  Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
  Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
  Friend WithEvents cmdOK As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents txtFilename As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents BarStaticItem1 As DevExpress.XtraBars.BarStaticItem
  Friend WithEvents grdContent4ImportIntoSPDb As DevExpress.XtraGrid.GridControl
  Friend WithEvents gvContent4InsertIntoSPDb As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents SplitContainerControl1 As DevExpress.XtraEditors.SplitContainerControl
  Friend WithEvents PanelControl2 As DevExpress.XtraEditors.PanelControl
  Friend WithEvents chkSendOne As DevExpress.XtraEditors.CheckEdit
  Friend WithEvents chkSendZhd As DevExpress.XtraEditors.CheckEdit
  Friend WithEvents cmdLoad As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents chkSendKD As DevExpress.XtraEditors.CheckEdit
  Friend WithEvents XtraTabControl1 As DevExpress.XtraTab.XtraTabControl
  Friend WithEvents xtabVersanddata As DevExpress.XtraTab.XtraTabPage
  Friend WithEvents xtabResultData As DevExpress.XtraTab.XtraTabPage
  Friend WithEvents PanelControl3 As DevExpress.XtraEditors.PanelControl
  Friend WithEvents lstVersandResult As DevExpress.XtraEditors.ListBoxControl
End Class
