<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMAGuthaben
	Inherits DevExpress.XtraEditors.XtraForm

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
		Dim SerializableAppearanceObject5 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject6 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject7 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject8 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject4 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMAGuthaben))
		Me.btnClose = New DevExpress.XtraEditors.SimpleButton()
		Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
		Me.XtraTabControl1 = New DevExpress.XtraTab.XtraTabControl()
		Me.XtraTabPage1 = New DevExpress.XtraTab.XtraTabPage()
		Me.grdSummery = New DevExpress.XtraGrid.GridControl()
		Me.gvSummery = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.XtraTabPage2 = New DevExpress.XtraTab.XtraTabPage()
		Me.grdDetail = New DevExpress.XtraGrid.GridControl()
		Me.gvDetail = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.xtabGFremdSystem = New DevExpress.XtraTab.XtraTabPage()
		Me.GroupControl1 = New DevExpress.XtraEditors.GroupControl()
		Me.lblMandant = New DevExpress.XtraEditors.LabelControl()
		Me.lueMandant = New DevExpress.XtraEditors.LookUpEdit()
		Me.lueLAData = New DevExpress.XtraEditors.LookUpEdit()
		Me.lblLohnart = New DevExpress.XtraEditors.LabelControl()
		Me.btnNewFremdGuthaben = New DevExpress.XtraEditors.SimpleButton()
		Me.grdFremdGuthaben = New DevExpress.XtraGrid.GridControl()
		Me.gvFremdGuthaben = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.btnSave = New DevExpress.XtraEditors.SimpleButton()
		Me.lblFerien = New DevExpress.XtraEditors.LabelControl()
		Me.txtFerienAmount = New DevExpress.XtraEditors.TextEdit()
		CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.PanelControl1.SuspendLayout()
		CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.XtraTabControl1.SuspendLayout()
		Me.XtraTabPage1.SuspendLayout()
		CType(Me.grdSummery, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvSummery, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.XtraTabPage2.SuspendLayout()
		CType(Me.grdDetail, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvDetail, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.xtabGFremdSystem.SuspendLayout()
		CType(Me.GroupControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupControl1.SuspendLayout()
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lueLAData.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.grdFremdGuthaben, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvFremdGuthaben, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtFerienAmount.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'btnClose
		'
		Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnClose.Location = New System.Drawing.Point(554, 72)
		Me.btnClose.Name = "btnClose"
		Me.btnClose.Size = New System.Drawing.Size(96, 27)
		Me.btnClose.TabIndex = 0
		Me.btnClose.Text = "Schliessen"
		'
		'PanelControl1
		'
		Me.PanelControl1.Controls.Add(Me.lblMandant)
		Me.PanelControl1.Controls.Add(Me.XtraTabControl1)
		Me.PanelControl1.Controls.Add(Me.lueMandant)
		Me.PanelControl1.Controls.Add(Me.btnClose)
		Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.PanelControl1.Location = New System.Drawing.Point(0, 0)
		Me.PanelControl1.Name = "PanelControl1"
		Me.PanelControl1.Size = New System.Drawing.Size(663, 460)
		Me.PanelControl1.TabIndex = 156
		'
		'XtraTabControl1
		'
		Me.XtraTabControl1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.XtraTabControl1.Location = New System.Drawing.Point(5, 49)
		Me.XtraTabControl1.Name = "XtraTabControl1"
		Me.XtraTabControl1.SelectedTabPage = Me.XtraTabPage1
		Me.XtraTabControl1.Size = New System.Drawing.Size(539, 406)
		Me.XtraTabControl1.TabIndex = 156
		Me.XtraTabControl1.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.XtraTabPage1, Me.XtraTabPage2, Me.xtabGFremdSystem})
		'
		'XtraTabPage1
		'
		Me.XtraTabPage1.Controls.Add(Me.grdSummery)
		Me.XtraTabPage1.Name = "XtraTabPage1"
		Me.XtraTabPage1.Padding = New System.Windows.Forms.Padding(5)
		Me.XtraTabPage1.Size = New System.Drawing.Size(528, 378)
		Me.XtraTabPage1.Text = "Zusammenfassung"
		'
		'grdSummery
		'
		Me.grdSummery.Cursor = System.Windows.Forms.Cursors.Default
		Me.grdSummery.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdSummery.Location = New System.Drawing.Point(5, 5)
		Me.grdSummery.MainView = Me.gvSummery
		Me.grdSummery.Name = "grdSummery"
		Me.grdSummery.Size = New System.Drawing.Size(518, 368)
		Me.grdSummery.TabIndex = 4
		Me.grdSummery.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvSummery})
		'
		'gvSummery
		'
		Me.gvSummery.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvSummery.GridControl = Me.grdSummery
		Me.gvSummery.Name = "gvSummery"
		Me.gvSummery.OptionsBehavior.Editable = False
		Me.gvSummery.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvSummery.OptionsView.ShowAutoFilterRow = True
		Me.gvSummery.OptionsView.ShowGroupPanel = False
		'
		'XtraTabPage2
		'
		Me.XtraTabPage2.Controls.Add(Me.grdDetail)
		Me.XtraTabPage2.Name = "XtraTabPage2"
		Me.XtraTabPage2.Padding = New System.Windows.Forms.Padding(5)
		Me.XtraTabPage2.Size = New System.Drawing.Size(528, 371)
		Me.XtraTabPage2.Text = "Details"
		'
		'grdDetail
		'
		Me.grdDetail.Cursor = System.Windows.Forms.Cursors.Default
		Me.grdDetail.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdDetail.Location = New System.Drawing.Point(5, 5)
		Me.grdDetail.MainView = Me.gvDetail
		Me.grdDetail.Name = "grdDetail"
		Me.grdDetail.Size = New System.Drawing.Size(518, 361)
		Me.grdDetail.TabIndex = 4
		Me.grdDetail.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvDetail})
		'
		'gvDetail
		'
		Me.gvDetail.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvDetail.GridControl = Me.grdDetail
		Me.gvDetail.Name = "gvDetail"
		Me.gvDetail.OptionsBehavior.Editable = False
		Me.gvDetail.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvDetail.OptionsView.ShowAutoFilterRow = True
		Me.gvDetail.OptionsView.ShowGroupPanel = False
		'
		'xtabGFremdSystem
		'
		Me.xtabGFremdSystem.Controls.Add(Me.GroupControl1)
		Me.xtabGFremdSystem.Name = "xtabGFremdSystem"
		Me.xtabGFremdSystem.Size = New System.Drawing.Size(533, 378)
		Me.xtabGFremdSystem.Text = "Lohnzeilen ändern!"
		'
		'GroupControl1
		'
		Me.GroupControl1.Controls.Add(Me.lueLAData)
		Me.GroupControl1.Controls.Add(Me.lblLohnart)
		Me.GroupControl1.Controls.Add(Me.btnNewFremdGuthaben)
		Me.GroupControl1.Controls.Add(Me.grdFremdGuthaben)
		Me.GroupControl1.Controls.Add(Me.btnSave)
		Me.GroupControl1.Controls.Add(Me.lblFerien)
		Me.GroupControl1.Controls.Add(Me.txtFerienAmount)
		Me.GroupControl1.Location = New System.Drawing.Point(19, 16)
		Me.GroupControl1.Name = "GroupControl1"
		Me.GroupControl1.Size = New System.Drawing.Size(484, 339)
		Me.GroupControl1.TabIndex = 0
		Me.GroupControl1.Text = "Guthaben aus Fremdsystemen"
		'
		'lblMandant
		'
		Me.lblMandant.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.lblMandant.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblMandant.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblMandant.Location = New System.Drawing.Point(25, 23)
		Me.lblMandant.Name = "lblMandant"
		Me.lblMandant.Size = New System.Drawing.Size(109, 13)
		Me.lblMandant.TabIndex = 251
		Me.lblMandant.Text = "Mandant"
		'
		'lueMandant
		'
		Me.lueMandant.Location = New System.Drawing.Point(140, 19)
		Me.lueMandant.MinimumSize = New System.Drawing.Size(150, 20)
		Me.lueMandant.Name = "lueMandant"
		SerializableAppearanceObject5.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject5.Options.UseForeColor = True
		SerializableAppearanceObject6.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject6.Options.UseForeColor = True
		SerializableAppearanceObject7.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject7.Options.UseForeColor = True
		SerializableAppearanceObject8.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject8.Options.UseForeColor = True
		Me.lueMandant.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject5, SerializableAppearanceObject6, SerializableAppearanceObject7, SerializableAppearanceObject8, "", Nothing, Nothing, True)})
		Me.lueMandant.Properties.ShowFooter = False
		Me.lueMandant.Size = New System.Drawing.Size(219, 20)
		Me.lueMandant.TabIndex = 250
		'
		'lueLAData
		'
		Me.lueLAData.Location = New System.Drawing.Point(115, 38)
		Me.lueLAData.Name = "lueLAData"
		SerializableAppearanceObject1.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject1.Options.UseForeColor = True
		SerializableAppearanceObject2.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject2.Options.UseForeColor = True
		SerializableAppearanceObject3.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject3.Options.UseForeColor = True
		SerializableAppearanceObject4.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject4.Options.UseForeColor = True
		Me.lueLAData.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, True)})
		Me.lueLAData.Properties.ShowFooter = False
		Me.lueLAData.Size = New System.Drawing.Size(219, 20)
		Me.lueLAData.TabIndex = 9
		'
		'lblLohnart
		'
		Me.lblLohnart.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblLohnart.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblLohnart.Location = New System.Drawing.Point(22, 42)
		Me.lblLohnart.Name = "lblLohnart"
		Me.lblLohnart.Size = New System.Drawing.Size(87, 13)
		Me.lblLohnart.TabIndex = 10
		Me.lblLohnart.Text = "Lohnart"
		'
		'btnNewFremdGuthaben
		'
		Me.btnNewFremdGuthaben.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnNewFremdGuthaben.Location = New System.Drawing.Point(371, 100)
		Me.btnNewFremdGuthaben.Name = "btnNewFremdGuthaben"
		Me.btnNewFremdGuthaben.Size = New System.Drawing.Size(96, 27)
		Me.btnNewFremdGuthaben.TabIndex = 8
		Me.btnNewFremdGuthaben.Text = "Neu"
		'
		'grdFremdGuthaben
		'
		Me.grdFremdGuthaben.Cursor = System.Windows.Forms.Cursors.Default
		Me.grdFremdGuthaben.Location = New System.Drawing.Point(5, 152)
		Me.grdFremdGuthaben.MainView = Me.gvFremdGuthaben
		Me.grdFremdGuthaben.Name = "grdFremdGuthaben"
		Me.grdFremdGuthaben.Size = New System.Drawing.Size(474, 182)
		Me.grdFremdGuthaben.TabIndex = 7
		Me.grdFremdGuthaben.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvFremdGuthaben})
		'
		'gvFremdGuthaben
		'
		Me.gvFremdGuthaben.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvFremdGuthaben.GridControl = Me.grdFremdGuthaben
		Me.gvFremdGuthaben.Name = "gvFremdGuthaben"
		Me.gvFremdGuthaben.OptionsBehavior.Editable = False
		Me.gvFremdGuthaben.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvFremdGuthaben.OptionsView.ShowAutoFilterRow = True
		Me.gvFremdGuthaben.OptionsView.ShowGroupPanel = False
		'
		'btnSave
		'
		Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnSave.Location = New System.Drawing.Point(371, 34)
		Me.btnSave.Name = "btnSave"
		Me.btnSave.Size = New System.Drawing.Size(96, 27)
		Me.btnSave.TabIndex = 6
		Me.btnSave.Text = "Speichern"
		'
		'lblFerien
		'
		Me.lblFerien.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblFerien.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblFerien.Location = New System.Drawing.Point(22, 67)
		Me.lblFerien.Name = "lblFerien"
		Me.lblFerien.Size = New System.Drawing.Size(87, 13)
		Me.lblFerien.TabIndex = 1
		Me.lblFerien.Text = "Ferien"
		'
		'txtFerienAmount
		'
		Me.txtFerienAmount.Location = New System.Drawing.Point(115, 64)
		Me.txtFerienAmount.Name = "txtFerienAmount"
		Me.txtFerienAmount.Size = New System.Drawing.Size(116, 20)
		Me.txtFerienAmount.TabIndex = 0
		'
		'frmMAGuthaben
		'
		Me.Appearance.Options.UseFont = True
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(663, 460)
		Me.Controls.Add(Me.PanelControl1)
		Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.Name = "frmMAGuthaben"
		Me.Text = "MetroForm"
		CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.PanelControl1.ResumeLayout(False)
		CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.XtraTabControl1.ResumeLayout(False)
		Me.XtraTabPage1.ResumeLayout(False)
		CType(Me.grdSummery, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvSummery, System.ComponentModel.ISupportInitialize).EndInit()
		Me.XtraTabPage2.ResumeLayout(False)
		CType(Me.grdDetail, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvDetail, System.ComponentModel.ISupportInitialize).EndInit()
		Me.xtabGFremdSystem.ResumeLayout(False)
		CType(Me.GroupControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupControl1.ResumeLayout(False)
		CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lueLAData.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.grdFremdGuthaben, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvFremdGuthaben, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtFerienAmount.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents btnClose As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
	Friend WithEvents XtraTabControl1 As DevExpress.XtraTab.XtraTabControl
	Friend WithEvents XtraTabPage1 As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents XtraTabPage2 As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents grdSummery As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvSummery As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents grdDetail As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvDetail As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents xtabGFremdSystem As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents GroupControl1 As DevExpress.XtraEditors.GroupControl
	Friend WithEvents grdFremdGuthaben As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvFremdGuthaben As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents btnSave As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents lblFerien As DevExpress.XtraEditors.LabelControl
	Friend WithEvents txtFerienAmount As DevExpress.XtraEditors.TextEdit
	Friend WithEvents btnNewFremdGuthaben As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents lueLAData As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents lblLohnart As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblMandant As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lueMandant As DevExpress.XtraEditors.LookUpEdit
End Class
