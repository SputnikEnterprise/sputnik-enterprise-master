<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
	Inherits DevExpress.XtraEditors.XtraForm

	'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
	<System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
		Me.XtraTabControl1 = New DevExpress.XtraTab.XtraTabControl()
		Me.xtabJob = New DevExpress.XtraTab.XtraTabPage()
		Me.grdUpdateSetting = New DevExpress.XtraGrid.GridControl()
		Me.gvUpdateSetting = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.xtabProtocol = New DevExpress.XtraTab.XtraTabPage()
		Me.grdLOG = New DevExpress.XtraGrid.GridControl()
		Me.gvLOG = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.chkSimulateUpload = New DevExpress.XtraEditors.CheckEdit()
		Me.btnLoadData = New DevExpress.XtraEditors.SimpleButton()
		Me.btnDownloadData = New DevExpress.XtraEditors.SimpleButton()
		Me.hlNewProgramVersion = New DevExpress.XtraEditors.HyperlinkLabelControl()
		CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.XtraTabControl1.SuspendLayout()
		Me.xtabJob.SuspendLayout()
		CType(Me.grdUpdateSetting, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvUpdateSetting, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.xtabProtocol.SuspendLayout()
		CType(Me.grdLOG, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvLOG, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkSimulateUpload.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'XtraTabControl1
		'
		Me.XtraTabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.XtraTabControl1.Location = New System.Drawing.Point(12, 147)
		Me.XtraTabControl1.Name = "XtraTabControl1"
		Me.XtraTabControl1.SelectedTabPage = Me.xtabJob
		Me.XtraTabControl1.Size = New System.Drawing.Size(780, 691)
		Me.XtraTabControl1.TabIndex = 3
		Me.XtraTabControl1.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabJob, Me.xtabProtocol})
		'
		'xtabJob
		'
		Me.xtabJob.Controls.Add(Me.grdUpdateSetting)
		Me.xtabJob.Name = "xtabJob"
		Me.xtabJob.Padding = New System.Windows.Forms.Padding(5)
		Me.xtabJob.Size = New System.Drawing.Size(774, 663)
		Me.xtabJob.Text = "Update-Job"
		'
		'grdUpdateSetting
		'
		Me.grdUpdateSetting.Cursor = System.Windows.Forms.Cursors.Default
		Me.grdUpdateSetting.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdUpdateSetting.Location = New System.Drawing.Point(5, 5)
		Me.grdUpdateSetting.MainView = Me.gvUpdateSetting
		Me.grdUpdateSetting.Name = "grdUpdateSetting"
		Me.grdUpdateSetting.Size = New System.Drawing.Size(764, 653)
		Me.grdUpdateSetting.TabIndex = 216
		Me.grdUpdateSetting.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvUpdateSetting})
		'
		'gvUpdateSetting
		'
		Me.gvUpdateSetting.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvUpdateSetting.GridControl = Me.grdUpdateSetting
		Me.gvUpdateSetting.Name = "gvUpdateSetting"
		Me.gvUpdateSetting.OptionsBehavior.Editable = False
		Me.gvUpdateSetting.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvUpdateSetting.OptionsView.ShowGroupPanel = False
		'
		'xtabProtocol
		'
		Me.xtabProtocol.Controls.Add(Me.grdLOG)
		Me.xtabProtocol.Name = "xtabProtocol"
		Me.xtabProtocol.Padding = New System.Windows.Forms.Padding(5)
		Me.xtabProtocol.Size = New System.Drawing.Size(774, 663)
		Me.xtabProtocol.Text = "Protokoll"
		'
		'grdLOG
		'
		Me.grdLOG.Cursor = System.Windows.Forms.Cursors.Default
		Me.grdLOG.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdLOG.Location = New System.Drawing.Point(5, 5)
		Me.grdLOG.MainView = Me.gvLOG
		Me.grdLOG.Name = "grdLOG"
		Me.grdLOG.Size = New System.Drawing.Size(764, 653)
		Me.grdLOG.TabIndex = 218
		Me.grdLOG.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvLOG})
		'
		'gvLOG
		'
		Me.gvLOG.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvLOG.GridControl = Me.grdLOG
		Me.gvLOG.Name = "gvLOG"
		Me.gvLOG.OptionsBehavior.Editable = False
		Me.gvLOG.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvLOG.OptionsView.ShowGroupPanel = False
		'
		'chkSimulateUpload
		'
		Me.chkSimulateUpload.Location = New System.Drawing.Point(12, 12)
		Me.chkSimulateUpload.Name = "chkSimulateUpload"
		Me.chkSimulateUpload.Properties.AllowFocused = False
		Me.chkSimulateUpload.Properties.Caption = "Upload SIMULIEREN!"
		Me.chkSimulateUpload.Size = New System.Drawing.Size(248, 19)
		Me.chkSimulateUpload.TabIndex = 222
		'
		'btnLoadData
		'
		Me.btnLoadData.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnLoadData.Location = New System.Drawing.Point(705, 12)
		Me.btnLoadData.Name = "btnLoadData"
		Me.btnLoadData.Size = New System.Drawing.Size(87, 33)
		Me.btnLoadData.TabIndex = 220
		Me.btnLoadData.Text = "Load Data"
		'
		'btnDownloadData
		'
		Me.btnDownloadData.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnDownloadData.Location = New System.Drawing.Point(705, 51)
		Me.btnDownloadData.Name = "btnDownloadData"
		Me.btnDownloadData.Size = New System.Drawing.Size(87, 33)
		Me.btnDownloadData.TabIndex = 221
		Me.btnDownloadData.Text = "Download Data"
		'
		'hlNewProgramVersion
		'
		Me.hlNewProgramVersion.Appearance.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
		Me.hlNewProgramVersion.Appearance.Options.UseFont = True
		Me.hlNewProgramVersion.Appearance.Options.UseTextOptions = True
		Me.hlNewProgramVersion.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
		Me.hlNewProgramVersion.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
		Me.hlNewProgramVersion.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.hlNewProgramVersion.Cursor = System.Windows.Forms.Cursors.Hand
		Me.hlNewProgramVersion.Location = New System.Drawing.Point(13, 40)
		Me.hlNewProgramVersion.Name = "hlNewProgramVersion"
		Me.hlNewProgramVersion.Size = New System.Drawing.Size(653, 90)
		Me.hlNewProgramVersion.TabIndex = 223
		Me.hlNewProgramVersion.Text = "Es existiert eine neue Programmversion. Bitte kontaktieren Sie Ihre Softwareherst" &
		"eller." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Sie können auch die neuen Dateien aus {0} ins {1} umkopieren."
		'
		'frmMain
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(804, 850)
		Me.Controls.Add(Me.hlNewProgramVersion)
		Me.Controls.Add(Me.chkSimulateUpload)
		Me.Controls.Add(Me.btnLoadData)
		Me.Controls.Add(Me.btnDownloadData)
		Me.Controls.Add(Me.XtraTabControl1)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MinimumSize = New System.Drawing.Size(500, 300)
		Me.Name = "frmMain"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Clientupdater für Sputnik Enterprise Suite"
		CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.XtraTabControl1.ResumeLayout(False)
		Me.xtabJob.ResumeLayout(False)
		CType(Me.grdUpdateSetting, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvUpdateSetting, System.ComponentModel.ISupportInitialize).EndInit()
		Me.xtabProtocol.ResumeLayout(False)
		CType(Me.grdLOG, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvLOG, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkSimulateUpload.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents XtraTabControl1 As DevExpress.XtraTab.XtraTabControl
	Friend WithEvents xtabJob As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents xtabProtocol As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents grdUpdateSetting As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvUpdateSetting As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents grdLOG As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvLOG As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents chkSimulateUpload As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents btnLoadData As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents btnDownloadData As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents hlNewProgramVersion As DevExpress.XtraEditors.HyperlinkLabelControl
End Class
