<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSrvFTPDownload
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSrvFTPDownload))
		Me.grdUpdates = New DevExpress.XtraGrid.GridControl()
		Me.gvUpdates = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.xTab = New DevExpress.XtraTab.XtraTabControl()
		Me.xtabUpdates = New DevExpress.XtraTab.XtraTabPage()
		Me.xtabNew = New DevExpress.XtraTab.XtraTabPage()
		Me.SeparatorControl1 = New DevExpress.XtraEditors.SeparatorControl()
		Me.lblInstalledUpdates = New DevExpress.XtraEditors.LabelControl()
		Me.grdInstalled = New DevExpress.XtraGrid.GridControl()
		Me.gvInstalled = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.grdNew = New DevExpress.XtraGrid.GridControl()
		Me.gvNew = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.xtabProgModules = New DevExpress.XtraTab.XtraTabPage()
		Me.SeparatorControl2 = New DevExpress.XtraEditors.SeparatorControl()
		Me.lblClientUpdates = New DevExpress.XtraEditors.LabelControl()
		Me.grdClientUpdateFiles = New DevExpress.XtraGrid.GridControl()
		Me.gvClientUpdateFiles = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.grdFTPUpdateFiles = New DevExpress.XtraGrid.GridControl()
		Me.gvFTPUpdateFiles = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.btnLoadData = New DevExpress.XtraEditors.SimpleButton()
		Me.btnDownloadData = New DevExpress.XtraEditors.SimpleButton()
		Me.chkSimulateUpload = New DevExpress.XtraEditors.CheckEdit()
		Me.hlNewProgramVersion = New DevExpress.XtraEditors.HyperlinkLabelControl()
		Me.lblDestPath = New DevExpress.XtraEditors.LabelControl()
		Me.lueDestPath = New DevExpress.XtraEditors.LookUpEdit()
		CType(Me.grdUpdates, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvUpdates, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.xTab, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.xTab.SuspendLayout()
		Me.xtabUpdates.SuspendLayout()
		Me.xtabNew.SuspendLayout()
		CType(Me.SeparatorControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.grdInstalled, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvInstalled, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.grdNew, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvNew, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.xtabProgModules.SuspendLayout()
		CType(Me.SeparatorControl2, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.grdClientUpdateFiles, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvClientUpdateFiles, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.grdFTPUpdateFiles, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvFTPUpdateFiles, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkSimulateUpload.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lueDestPath.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'grdUpdates
		'
		Me.grdUpdates.AllowDrop = True
		Me.grdUpdates.Cursor = System.Windows.Forms.Cursors.Default
		Me.grdUpdates.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdUpdates.Location = New System.Drawing.Point(5, 5)
		Me.grdUpdates.MainView = Me.gvUpdates
		Me.grdUpdates.Name = "grdUpdates"
		Me.grdUpdates.Size = New System.Drawing.Size(933, 587)
		Me.grdUpdates.TabIndex = 215
		Me.grdUpdates.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvUpdates})
		'
		'gvUpdates
		'
		Me.gvUpdates.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvUpdates.GridControl = Me.grdUpdates
		Me.gvUpdates.Name = "gvUpdates"
		Me.gvUpdates.OptionsBehavior.Editable = False
		Me.gvUpdates.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvUpdates.OptionsView.ShowGroupPanel = False
		'
		'xTab
		'
		Me.xTab.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.xTab.Location = New System.Drawing.Point(10, 139)
		Me.xTab.Name = "xTab"
		Me.xTab.SelectedTabPage = Me.xtabUpdates
		Me.xTab.Size = New System.Drawing.Size(945, 622)
		Me.xTab.TabIndex = 216
		Me.xTab.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabUpdates, Me.xtabNew, Me.xtabProgModules})
		'
		'xtabUpdates
		'
		Me.xtabUpdates.Controls.Add(Me.grdUpdates)
		Me.xtabUpdates.Name = "xtabUpdates"
		Me.xtabUpdates.Padding = New System.Windows.Forms.Padding(5)
		Me.xtabUpdates.Size = New System.Drawing.Size(943, 597)
		Me.xtabUpdates.Text = "Vorhandene Updates"
		'
		'xtabNew
		'
		Me.xtabNew.Controls.Add(Me.SeparatorControl1)
		Me.xtabNew.Controls.Add(Me.lblInstalledUpdates)
		Me.xtabNew.Controls.Add(Me.grdInstalled)
		Me.xtabNew.Controls.Add(Me.grdNew)
		Me.xtabNew.Name = "xtabNew"
		Me.xtabNew.Padding = New System.Windows.Forms.Padding(5)
		Me.xtabNew.Size = New System.Drawing.Size(943, 597)
		Me.xtabNew.Text = "Benötige Updates"
		'
		'SeparatorControl1
		'
		Me.SeparatorControl1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.SeparatorControl1.Location = New System.Drawing.Point(135, 297)
		Me.SeparatorControl1.Name = "SeparatorControl1"
		Me.SeparatorControl1.Size = New System.Drawing.Size(796, 27)
		Me.SeparatorControl1.TabIndex = 219
		'
		'lblInstalledUpdates
		'
		Me.lblInstalledUpdates.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.lblInstalledUpdates.Location = New System.Drawing.Point(5, 302)
		Me.lblInstalledUpdates.Name = "lblInstalledUpdates"
		Me.lblInstalledUpdates.Size = New System.Drawing.Size(124, 13)
		Me.lblInstalledUpdates.TabIndex = 218
		Me.lblInstalledUpdates.Text = "Bereits insallierte Updates"
		'
		'grdInstalled
		'
		Me.grdInstalled.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.grdInstalled.Cursor = System.Windows.Forms.Cursors.Default
		Me.grdInstalled.Location = New System.Drawing.Point(5, 328)
		Me.grdInstalled.MainView = Me.gvInstalled
		Me.grdInstalled.Name = "grdInstalled"
		Me.grdInstalled.Size = New System.Drawing.Size(929, 258)
		Me.grdInstalled.TabIndex = 217
		Me.grdInstalled.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvInstalled})
		'
		'gvInstalled
		'
		Me.gvInstalled.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvInstalled.GridControl = Me.grdInstalled
		Me.gvInstalled.Name = "gvInstalled"
		Me.gvInstalled.OptionsBehavior.Editable = False
		Me.gvInstalled.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvInstalled.OptionsView.ShowGroupPanel = False
		'
		'grdNew
		'
		Me.grdNew.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.grdNew.Cursor = System.Windows.Forms.Cursors.Default
		Me.grdNew.Location = New System.Drawing.Point(5, 5)
		Me.grdNew.MainView = Me.gvNew
		Me.grdNew.Name = "grdNew"
		Me.grdNew.Size = New System.Drawing.Size(929, 284)
		Me.grdNew.TabIndex = 216
		Me.grdNew.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvNew})
		'
		'gvNew
		'
		Me.gvNew.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvNew.GridControl = Me.grdNew
		Me.gvNew.Name = "gvNew"
		Me.gvNew.OptionsBehavior.Editable = False
		Me.gvNew.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvNew.OptionsView.ShowGroupPanel = False
		'
		'xtabProgModules
		'
		Me.xtabProgModules.Controls.Add(Me.SeparatorControl2)
		Me.xtabProgModules.Controls.Add(Me.lblClientUpdates)
		Me.xtabProgModules.Controls.Add(Me.grdClientUpdateFiles)
		Me.xtabProgModules.Controls.Add(Me.grdFTPUpdateFiles)
		Me.xtabProgModules.Name = "xtabProgModules"
		Me.xtabProgModules.Padding = New System.Windows.Forms.Padding(5)
		Me.xtabProgModules.Size = New System.Drawing.Size(943, 597)
		Me.xtabProgModules.Text = "Programm-Module"
		'
		'SeparatorControl2
		'
		Me.SeparatorControl2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.SeparatorControl2.Location = New System.Drawing.Point(96, 300)
		Me.SeparatorControl2.Name = "SeparatorControl2"
		Me.SeparatorControl2.Size = New System.Drawing.Size(835, 27)
		Me.SeparatorControl2.TabIndex = 221
		'
		'lblClientUpdates
		'
		Me.lblClientUpdates.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.lblClientUpdates.Location = New System.Drawing.Point(6, 305)
		Me.lblClientUpdates.Name = "lblClientUpdates"
		Me.lblClientUpdates.Size = New System.Drawing.Size(84, 13)
		Me.lblClientUpdates.TabIndex = 220
		Me.lblClientUpdates.Text = "Updates für Lokal"
		'
		'grdClientUpdateFiles
		'
		Me.grdClientUpdateFiles.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.grdClientUpdateFiles.Cursor = System.Windows.Forms.Cursors.Default
		Me.grdClientUpdateFiles.Location = New System.Drawing.Point(5, 333)
		Me.grdClientUpdateFiles.MainView = Me.gvClientUpdateFiles
		Me.grdClientUpdateFiles.Name = "grdClientUpdateFiles"
		Me.grdClientUpdateFiles.Size = New System.Drawing.Size(926, 253)
		Me.grdClientUpdateFiles.TabIndex = 218
		Me.grdClientUpdateFiles.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvClientUpdateFiles})
		'
		'gvClientUpdateFiles
		'
		Me.gvClientUpdateFiles.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvClientUpdateFiles.GridControl = Me.grdClientUpdateFiles
		Me.gvClientUpdateFiles.Name = "gvClientUpdateFiles"
		Me.gvClientUpdateFiles.OptionsBehavior.Editable = False
		Me.gvClientUpdateFiles.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvClientUpdateFiles.OptionsView.ShowGroupPanel = False
		'
		'grdFTPUpdateFiles
		'
		Me.grdFTPUpdateFiles.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.grdFTPUpdateFiles.Cursor = System.Windows.Forms.Cursors.Default
		Me.grdFTPUpdateFiles.Location = New System.Drawing.Point(5, 5)
		Me.grdFTPUpdateFiles.MainView = Me.gvFTPUpdateFiles
		Me.grdFTPUpdateFiles.Name = "grdFTPUpdateFiles"
		Me.grdFTPUpdateFiles.Size = New System.Drawing.Size(926, 289)
		Me.grdFTPUpdateFiles.TabIndex = 216
		Me.grdFTPUpdateFiles.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvFTPUpdateFiles})
		'
		'gvFTPUpdateFiles
		'
		Me.gvFTPUpdateFiles.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvFTPUpdateFiles.GridControl = Me.grdFTPUpdateFiles
		Me.gvFTPUpdateFiles.Name = "gvFTPUpdateFiles"
		Me.gvFTPUpdateFiles.OptionsBehavior.Editable = False
		Me.gvFTPUpdateFiles.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvFTPUpdateFiles.OptionsView.ShowGroupPanel = False
		'
		'btnLoadData
		'
		Me.btnLoadData.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnLoadData.Location = New System.Drawing.Point(868, 16)
		Me.btnLoadData.Name = "btnLoadData"
		Me.btnLoadData.Size = New System.Drawing.Size(87, 33)
		Me.btnLoadData.TabIndex = 217
		Me.btnLoadData.Text = "Load Data"
		'
		'btnDownloadData
		'
		Me.btnDownloadData.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnDownloadData.Location = New System.Drawing.Point(868, 55)
		Me.btnDownloadData.Name = "btnDownloadData"
		Me.btnDownloadData.Size = New System.Drawing.Size(87, 33)
		Me.btnDownloadData.TabIndex = 218
		Me.btnDownloadData.Text = "Download Data"
		'
		'chkSimulateUpload
		'
		Me.chkSimulateUpload.Location = New System.Drawing.Point(10, 16)
		Me.chkSimulateUpload.Name = "chkSimulateUpload"
		Me.chkSimulateUpload.Properties.AllowFocused = False
		Me.chkSimulateUpload.Properties.Caption = "Upload SIMULIEREN!"
		Me.chkSimulateUpload.Size = New System.Drawing.Size(132, 20)
		Me.chkSimulateUpload.TabIndex = 219
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
		Me.hlNewProgramVersion.Location = New System.Drawing.Point(10, 43)
		Me.hlNewProgramVersion.Name = "hlNewProgramVersion"
		Me.hlNewProgramVersion.Size = New System.Drawing.Size(704, 90)
		Me.hlNewProgramVersion.TabIndex = 221
		Me.hlNewProgramVersion.Text = "Es existiert eine neue Programmversion. Bitte kontaktieren Sie Ihre Softwareherst" &
	"eller." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Sie können auch die neuen Dateien aus {0} ins {1} umkopieren."
		'
		'lblDestPath
		'
		Me.lblDestPath.Location = New System.Drawing.Point(753, 108)
		Me.lblDestPath.Name = "lblDestPath"
		Me.lblDestPath.Size = New System.Drawing.Size(42, 13)
		Me.lblDestPath.TabIndex = 223
		Me.lblDestPath.Text = "Ziel-Path"
		'
		'lueDestPath
		'
		Me.lueDestPath.Location = New System.Drawing.Point(801, 105)
		Me.lueDestPath.Name = "lueDestPath"
		Me.lueDestPath.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.lueDestPath.Properties.NullText = ""
		Me.lueDestPath.Properties.PopupSizeable = False
		Me.lueDestPath.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
		Me.lueDestPath.Size = New System.Drawing.Size(151, 20)
		Me.lueDestPath.TabIndex = 222
		'
		'frmSrvFTPDownload
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(972, 780)
		Me.Controls.Add(Me.lblDestPath)
		Me.Controls.Add(Me.hlNewProgramVersion)
		Me.Controls.Add(Me.chkSimulateUpload)
		Me.Controls.Add(Me.btnLoadData)
		Me.Controls.Add(Me.btnDownloadData)
		Me.Controls.Add(Me.xTab)
		Me.Controls.Add(Me.lueDestPath)
		Me.IconOptions.Icon = CType(resources.GetObject("frmSrvFTPDownload.IconOptions.Icon"), System.Drawing.Icon)
		Me.MinimumSize = New System.Drawing.Size(934, 659)
		Me.Name = "frmSrvFTPDownload"
		Me.Padding = New System.Windows.Forms.Padding(10)
		Me.Text = "FTP-Update Server"
		CType(Me.grdUpdates, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvUpdates, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.xTab, System.ComponentModel.ISupportInitialize).EndInit()
		Me.xTab.ResumeLayout(False)
		Me.xtabUpdates.ResumeLayout(False)
		Me.xtabNew.ResumeLayout(False)
		Me.xtabNew.PerformLayout()
		CType(Me.SeparatorControl1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.grdInstalled, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvInstalled, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.grdNew, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvNew, System.ComponentModel.ISupportInitialize).EndInit()
		Me.xtabProgModules.ResumeLayout(False)
		Me.xtabProgModules.PerformLayout()
		CType(Me.SeparatorControl2, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.grdClientUpdateFiles, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvClientUpdateFiles, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.grdFTPUpdateFiles, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvFTPUpdateFiles, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkSimulateUpload.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lueDestPath.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents grdUpdates As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvUpdates As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents xTab As DevExpress.XtraTab.XtraTabControl
	Friend WithEvents xtabUpdates As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents xtabNew As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents grdNew As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvNew As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents grdInstalled As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvInstalled As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents btnLoadData As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents btnDownloadData As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents chkSimulateUpload As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents xtabProgModules As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents grdFTPUpdateFiles As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvFTPUpdateFiles As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents grdClientUpdateFiles As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvClientUpdateFiles As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents hlNewProgramVersion As DevExpress.XtraEditors.HyperlinkLabelControl
	Friend WithEvents SeparatorControl1 As DevExpress.XtraEditors.SeparatorControl
	Friend WithEvents lblInstalledUpdates As DevExpress.XtraEditors.LabelControl
	Friend WithEvents SeparatorControl2 As DevExpress.XtraEditors.SeparatorControl
	Friend WithEvents lblClientUpdates As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblDestPath As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lueDestPath As DevExpress.XtraEditors.LookUpEdit
End Class
