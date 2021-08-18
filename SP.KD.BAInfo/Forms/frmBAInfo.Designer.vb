<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBAInfo
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBAInfo))
		Me.sccMain = New DevExpress.XtraEditors.SplitContainerControl()
		Me.gridBAInfo = New DevExpress.XtraGrid.GridControl()
		Me.gvBAInfo = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.grpDetails = New DevExpress.XtraEditors.GroupControl()
		Me.btnCreateTODO = New DevExpress.XtraEditors.SimpleButton()
		Me.lblTypeOfQuery = New DevExpress.XtraEditors.LabelControl()
		Me.lblArt = New DevExpress.XtraEditors.LabelControl()
		Me.lblArchiveNr = New DevExpress.XtraEditors.LabelControl()
		Me.chkActive = New DevExpress.XtraEditors.CheckEdit()
		Me.btnDeleteCreditInfo = New DevExpress.XtraEditors.SimpleButton()
		Me.btnSolvencyDecision = New DevExpress.XtraEditors.SimpleButton()
		Me.btnOpenReport = New DevExpress.XtraEditors.SimpleButton()
		Me.lblCreditInfoChanged = New DevExpress.XtraEditors.LabelControl()
		Me.lblCreditInfoCreated = New DevExpress.XtraEditors.LabelControl()
		Me.lblGeaendert = New DevExpress.XtraEditors.LabelControl()
		Me.lblErstellt = New DevExpress.XtraEditors.LabelControl()
		Me.btnNewCreditInfo = New DevExpress.XtraEditors.SimpleButton()
		Me.btnSave = New DevExpress.XtraEditors.SimpleButton()
		Me.lblBeschreibung = New DevExpress.XtraEditors.LabelControl()
		Me.txtDescription = New DevExpress.XtraEditors.MemoEdit()
		Me.lblDatei = New DevExpress.XtraEditors.LabelControl()
		Me.txtFilePath = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.errorProviderCreditInfo = New System.Windows.Forms.ErrorProvider()
		Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
		CType(Me.sccMain, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.sccMain.SuspendLayout()
		CType(Me.gridBAInfo, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvBAInfo, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.grpDetails, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grpDetails.SuspendLayout()
		CType(Me.chkActive.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtDescription.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtFilePath.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.errorProviderCreditInfo, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'sccMain
		'
		Me.sccMain.Dock = System.Windows.Forms.DockStyle.Fill
		Me.sccMain.Horizontal = False
		Me.sccMain.Location = New System.Drawing.Point(0, 0)
		Me.sccMain.Name = "sccMain"
		Me.sccMain.Padding = New System.Windows.Forms.Padding(5)
		Me.sccMain.Panel1.Controls.Add(Me.gridBAInfo)
		Me.sccMain.Panel1.Padding = New System.Windows.Forms.Padding(5)
		Me.sccMain.Panel1.Text = "Panel1"
		Me.sccMain.Panel2.Controls.Add(Me.grpDetails)
		Me.sccMain.Panel2.MinSize = 300
		Me.sccMain.Panel2.Padding = New System.Windows.Forms.Padding(5)
		Me.sccMain.Panel2.Text = "Panel2"
		Me.sccMain.Size = New System.Drawing.Size(663, 558)
		Me.sccMain.SplitterPosition = 318
		Me.sccMain.TabIndex = 0
		Me.sccMain.Text = "SplitContainerControl1"
		'
		'gridBAInfo
		'
		Me.gridBAInfo.Dock = System.Windows.Forms.DockStyle.Fill
		Me.gridBAInfo.Location = New System.Drawing.Point(5, 5)
		Me.gridBAInfo.MainView = Me.gvBAInfo
		Me.gridBAInfo.Name = "gridBAInfo"
		Me.gridBAInfo.Size = New System.Drawing.Size(643, 233)
		Me.gridBAInfo.TabIndex = 3
		Me.gridBAInfo.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvBAInfo})
		'
		'gvBAInfo
		'
		Me.gvBAInfo.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvBAInfo.GridControl = Me.gridBAInfo
		Me.gvBAInfo.Name = "gvBAInfo"
		Me.gvBAInfo.OptionsBehavior.Editable = False
		Me.gvBAInfo.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvBAInfo.OptionsView.ShowGroupPanel = False
		'
		'grpDetails
		'
		Me.grpDetails.Controls.Add(Me.btnCreateTODO)
		Me.grpDetails.Controls.Add(Me.lblTypeOfQuery)
		Me.grpDetails.Controls.Add(Me.lblArt)
		Me.grpDetails.Controls.Add(Me.lblArchiveNr)
		Me.grpDetails.Controls.Add(Me.chkActive)
		Me.grpDetails.Controls.Add(Me.btnDeleteCreditInfo)
		Me.grpDetails.Controls.Add(Me.btnSolvencyDecision)
		Me.grpDetails.Controls.Add(Me.btnOpenReport)
		Me.grpDetails.Controls.Add(Me.lblCreditInfoChanged)
		Me.grpDetails.Controls.Add(Me.lblCreditInfoCreated)
		Me.grpDetails.Controls.Add(Me.lblGeaendert)
		Me.grpDetails.Controls.Add(Me.lblErstellt)
		Me.grpDetails.Controls.Add(Me.btnNewCreditInfo)
		Me.grpDetails.Controls.Add(Me.btnSave)
		Me.grpDetails.Controls.Add(Me.lblBeschreibung)
		Me.grpDetails.Controls.Add(Me.txtDescription)
		Me.grpDetails.Controls.Add(Me.lblDatei)
		Me.grpDetails.Controls.Add(Me.txtFilePath)
		Me.grpDetails.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grpDetails.Location = New System.Drawing.Point(5, 5)
		Me.grpDetails.Name = "grpDetails"
		Me.grpDetails.Size = New System.Drawing.Size(643, 290)
		Me.grpDetails.TabIndex = 0
		Me.grpDetails.Text = "Details"
		'
		'btnCreateTODO
		'
		Me.btnCreateTODO.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnCreateTODO.ImageOptions.Image = CType(resources.GetObject("btnCreateTODO.ImageOptions.Image"), System.Drawing.Image)
		Me.btnCreateTODO.Location = New System.Drawing.Point(524, 229)
		Me.btnCreateTODO.Name = "btnCreateTODO"
		Me.btnCreateTODO.Size = New System.Drawing.Size(105, 28)
		Me.btnCreateTODO.TabIndex = 287
		Me.btnCreateTODO.Text = "to-do erstellen"
		'
		'lblTypeOfQuery
		'
		Me.lblTypeOfQuery.Location = New System.Drawing.Point(143, 99)
		Me.lblTypeOfQuery.Name = "lblTypeOfQuery"
		Me.lblTypeOfQuery.Size = New System.Drawing.Size(15, 13)
		Me.lblTypeOfQuery.TabIndex = 220
		Me.lblTypeOfQuery.Text = "Art"
		'
		'lblArt
		'
		Me.lblArt.Location = New System.Drawing.Point(55, 98)
		Me.lblArt.Name = "lblArt"
		Me.lblArt.Size = New System.Drawing.Size(76, 13)
		Me.lblArt.TabIndex = 217
		Me.lblArt.Text = "Art der Abfrage"
		'
		'lblArchiveNr
		'
		Me.lblArchiveNr.Appearance.Options.UseTextOptions = True
		Me.lblArchiveNr.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblArchiveNr.Location = New System.Drawing.Point(306, 73)
		Me.lblArchiveNr.Name = "lblArchiveNr"
		Me.lblArchiveNr.Size = New System.Drawing.Size(49, 13)
		Me.lblArchiveNr.TabIndex = 214
		Me.lblArchiveNr.Text = "Archiv-Nr."
		'
		'chkActive
		'
		Me.chkActive.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.chkActive.Location = New System.Drawing.Point(141, 202)
		Me.chkActive.Name = "chkActive"
		Me.chkActive.Properties.Caption = "Diese Daten müssen auf der Debitorenliste erscheinen"
		Me.chkActive.Size = New System.Drawing.Size(367, 19)
		Me.chkActive.TabIndex = 213
		'
		'btnDeleteCreditInfo
		'
		Me.btnDeleteCreditInfo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnDeleteCreditInfo.ImageOptions.Image = CType(resources.GetObject("btnDeleteCreditInfo.ImageOptions.Image"), System.Drawing.Image)
		Me.btnDeleteCreditInfo.Location = New System.Drawing.Point(524, 195)
		Me.btnDeleteCreditInfo.Name = "btnDeleteCreditInfo"
		Me.btnDeleteCreditInfo.Size = New System.Drawing.Size(105, 28)
		Me.btnDeleteCreditInfo.TabIndex = 212
		Me.btnDeleteCreditInfo.Text = "Löschen"
		'
		'btnSolvencyDecision
		'
		Me.btnSolvencyDecision.AllowFocus = False
		Me.btnSolvencyDecision.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnSolvencyDecision.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.btnSolvencyDecision.Appearance.Image = Global.SP.KD.BAInfo.My.Resources.Resources.OpenFile
		Me.btnSolvencyDecision.Appearance.Options.UseBackColor = True
		Me.btnSolvencyDecision.Appearance.Options.UseImage = True
		Me.btnSolvencyDecision.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat
		Me.btnSolvencyDecision.ImageOptions.Image = CType(resources.GetObject("btnSolvencyDecision.ImageOptions.Image"), System.Drawing.Image)
		Me.btnSolvencyDecision.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter
		Me.btnSolvencyDecision.Location = New System.Drawing.Point(552, 34)
		Me.btnSolvencyDecision.Margin = New System.Windows.Forms.Padding(1)
		Me.btnSolvencyDecision.Name = "btnSolvencyDecision"
		Me.btnSolvencyDecision.Size = New System.Drawing.Size(48, 48)
		Me.btnSolvencyDecision.TabIndex = 211
		'
		'btnOpenReport
		'
		Me.btnOpenReport.AllowFocus = False
		Me.btnOpenReport.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.btnOpenReport.Appearance.Image = Global.SP.KD.BAInfo.My.Resources.Resources.OpenFile
		Me.btnOpenReport.Appearance.Options.UseBackColor = True
		Me.btnOpenReport.Appearance.Options.UseImage = True
		Me.btnOpenReport.ImageOptions.Image = CType(resources.GetObject("btnOpenReport.ImageOptions.Image"), System.Drawing.Image)
		Me.btnOpenReport.Location = New System.Drawing.Point(143, 69)
		Me.btnOpenReport.Margin = New System.Windows.Forms.Padding(1)
		Me.btnOpenReport.Name = "btnOpenReport"
		Me.btnOpenReport.Size = New System.Drawing.Size(159, 22)
		Me.btnOpenReport.TabIndex = 210
		Me.btnOpenReport.Text = "Datei öffnen"
		'
		'lblCreditInfoChanged
		'
		Me.lblCreditInfoChanged.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.lblCreditInfoChanged.Location = New System.Drawing.Point(141, 264)
		Me.lblCreditInfoChanged.Name = "lblCreditInfoChanged"
		Me.lblCreditInfoChanged.Size = New System.Drawing.Size(49, 13)
		Me.lblCreditInfoChanged.TabIndex = 32
		Me.lblCreditInfoChanged.Text = "Geändert:"
		'
		'lblCreditInfoCreated
		'
		Me.lblCreditInfoCreated.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.lblCreditInfoCreated.Location = New System.Drawing.Point(141, 245)
		Me.lblCreditInfoCreated.Name = "lblCreditInfoCreated"
		Me.lblCreditInfoCreated.Size = New System.Drawing.Size(37, 13)
		Me.lblCreditInfoCreated.TabIndex = 31
		Me.lblCreditInfoCreated.Text = "Erstellt:"
		'
		'lblGeaendert
		'
		Me.lblGeaendert.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.lblGeaendert.Appearance.Options.UseTextOptions = True
		Me.lblGeaendert.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblGeaendert.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblGeaendert.Location = New System.Drawing.Point(18, 264)
		Me.lblGeaendert.Name = "lblGeaendert"
		Me.lblGeaendert.Size = New System.Drawing.Size(117, 13)
		Me.lblGeaendert.TabIndex = 30
		Me.lblGeaendert.Text = "Geändert"
		'
		'lblErstellt
		'
		Me.lblErstellt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.lblErstellt.Appearance.Options.UseTextOptions = True
		Me.lblErstellt.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblErstellt.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblErstellt.Location = New System.Drawing.Point(18, 245)
		Me.lblErstellt.Name = "lblErstellt"
		Me.lblErstellt.Size = New System.Drawing.Size(117, 13)
		Me.lblErstellt.TabIndex = 29
		Me.lblErstellt.Text = "Erstellt"
		'
		'btnNewCreditInfo
		'
		Me.btnNewCreditInfo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnNewCreditInfo.ImageOptions.Image = CType(resources.GetObject("btnNewCreditInfo.ImageOptions.Image"), System.Drawing.Image)
		Me.btnNewCreditInfo.Location = New System.Drawing.Point(524, 161)
		Me.btnNewCreditInfo.Name = "btnNewCreditInfo"
		Me.btnNewCreditInfo.Size = New System.Drawing.Size(105, 28)
		Me.btnNewCreditInfo.TabIndex = 27
		Me.btnNewCreditInfo.Text = "Neu"
		'
		'btnSave
		'
		Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnSave.ImageOptions.Image = CType(resources.GetObject("btnSave.ImageOptions.Image"), System.Drawing.Image)
		Me.btnSave.Location = New System.Drawing.Point(524, 127)
		Me.btnSave.Name = "btnSave"
		Me.btnSave.Size = New System.Drawing.Size(105, 28)
		Me.btnSave.TabIndex = 26
		Me.btnSave.Text = "Speichern"
		'
		'lblBeschreibung
		'
		Me.lblBeschreibung.Appearance.Options.UseTextOptions = True
		Me.lblBeschreibung.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblBeschreibung.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblBeschreibung.Location = New System.Drawing.Point(18, 127)
		Me.lblBeschreibung.Name = "lblBeschreibung"
		Me.lblBeschreibung.Size = New System.Drawing.Size(117, 13)
		Me.lblBeschreibung.TabIndex = 7
		Me.lblBeschreibung.Text = "Beschreibung"
		'
		'txtDescription
		'
		Me.txtDescription.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txtDescription.Location = New System.Drawing.Point(141, 127)
		Me.txtDescription.Name = "txtDescription"
		Me.txtDescription.Size = New System.Drawing.Size(367, 71)
		Me.txtDescription.TabIndex = 6
		'
		'lblDatei
		'
		Me.lblDatei.Appearance.Options.UseTextOptions = True
		Me.lblDatei.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblDatei.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblDatei.Location = New System.Drawing.Point(18, 45)
		Me.lblDatei.Name = "lblDatei"
		Me.lblDatei.Size = New System.Drawing.Size(117, 13)
		Me.lblDatei.TabIndex = 5
		Me.lblDatei.Text = "Datei"
		'
		'txtFilePath
		'
		Me.txtFilePath.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txtFilePath.Location = New System.Drawing.Point(141, 43)
		Me.txtFilePath.Name = "txtFilePath"
		Me.txtFilePath.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.txtFilePath.Size = New System.Drawing.Size(367, 20)
		Me.txtFilePath.TabIndex = 34
		'
		'errorProviderCreditInfo
		'
		Me.errorProviderCreditInfo.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
		Me.errorProviderCreditInfo.ContainerControl = Me
		'
		'OpenFileDialog1
		'
		Me.OpenFileDialog1.FileName = "OpenFileDialog1"
		'
		'frmBAInfo
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(663, 558)
		Me.Controls.Add(Me.sccMain)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MinimumSize = New System.Drawing.Size(679, 596)
		Me.Name = "frmBAInfo"
		Me.Text = "Kreditinformationen"
		CType(Me.sccMain, System.ComponentModel.ISupportInitialize).EndInit()
		Me.sccMain.ResumeLayout(False)
		CType(Me.gridBAInfo, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvBAInfo, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.grpDetails, System.ComponentModel.ISupportInitialize).EndInit()
		Me.grpDetails.ResumeLayout(False)
		Me.grpDetails.PerformLayout()
		CType(Me.chkActive.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtDescription.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtFilePath.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.errorProviderCreditInfo, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents sccMain As DevExpress.XtraEditors.SplitContainerControl
    Friend WithEvents gridBAInfo As DevExpress.XtraGrid.GridControl
    Friend WithEvents gvBAInfo As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents grpDetails As DevExpress.XtraEditors.GroupControl
    Friend WithEvents lblCreditInfoChanged As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblCreditInfoCreated As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblGeaendert As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblErstellt As DevExpress.XtraEditors.LabelControl
  Friend WithEvents btnNewCreditInfo As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnSave As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents lblBeschreibung As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtDescription As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents lblDatei As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtFilePath As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents btnOpenReport As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnSolvencyDecision As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents errorProviderCreditInfo As System.Windows.Forms.ErrorProvider
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents btnDeleteCreditInfo As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents chkActive As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents lblArchiveNr As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblArt As DevExpress.XtraEditors.LabelControl
  Friend WithEvents lblTypeOfQuery As DevExpress.XtraEditors.LabelControl
  Friend WithEvents btnCreateTODO As DevExpress.XtraEditors.SimpleButton
End Class
