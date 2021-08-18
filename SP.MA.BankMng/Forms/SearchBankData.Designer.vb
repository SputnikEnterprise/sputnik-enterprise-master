<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SearchBankData
  Inherits SP.Infrastructure.Forms.frmBase

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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SearchBankData))
    Me.gridBank = New DevExpress.XtraGrid.GridControl()
    Me.gvBank = New DevExpress.XtraGrid.Views.Grid.GridView()
    Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
    Me.lblSearchInfo = New System.Windows.Forms.Label()
    Me.btnSearch = New DevExpress.XtraEditors.SimpleButton()
    Me.btnClose = New DevExpress.XtraEditors.SimpleButton()
    Me.lblFoundItems = New DevExpress.XtraEditors.LabelControl()
    Me.lblSwift = New DevExpress.XtraEditors.LabelControl()
    Me.lblBankOrt = New DevExpress.XtraEditors.LabelControl()
    Me.lblBLZ = New DevExpress.XtraEditors.LabelControl()
    Me.lblBankname = New DevExpress.XtraEditors.LabelControl()
    Me.lblClearing = New DevExpress.XtraEditors.LabelControl()
    Me.txtPLZ = New DevExpress.XtraEditors.TextEdit()
    Me.txtLocation = New DevExpress.XtraEditors.TextEdit()
    Me.txtSwift = New DevExpress.XtraEditors.TextEdit()
    Me.txtBankname = New DevExpress.XtraEditors.TextEdit()
    Me.txtClearingNumber = New DevExpress.XtraEditors.TextEdit()
    Me.groupDataSource = New DevExpress.XtraEditors.RadioGroup()
    CType(Me.gridBank, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.gvBank, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.PanelControl1.SuspendLayout()
    CType(Me.txtPLZ.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txtLocation.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txtSwift.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txtBankname.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txtClearingNumber.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.groupDataSource.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'gridBank
    '
    Me.gridBank.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.gridBank.Location = New System.Drawing.Point(5, 252)
    Me.gridBank.MainView = Me.gvBank
    Me.gridBank.Name = "gridBank"
    Me.gridBank.Size = New System.Drawing.Size(699, 232)
    Me.gridBank.TabIndex = 214
    Me.gridBank.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvBank})
    '
    'gvBank
    '
    Me.gvBank.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
    Me.gvBank.GridControl = Me.gridBank
    Me.gvBank.Name = "gvBank"
    Me.gvBank.OptionsBehavior.Editable = False
    Me.gvBank.OptionsSelection.EnableAppearanceFocusedCell = False
    Me.gvBank.OptionsView.ShowGroupPanel = False
    '
    'PanelControl1
    '
    Me.PanelControl1.Controls.Add(Me.gridBank)
    Me.PanelControl1.Controls.Add(Me.lblSearchInfo)
    Me.PanelControl1.Controls.Add(Me.btnSearch)
    Me.PanelControl1.Controls.Add(Me.btnClose)
    Me.PanelControl1.Controls.Add(Me.lblFoundItems)
    Me.PanelControl1.Controls.Add(Me.lblSwift)
    Me.PanelControl1.Controls.Add(Me.lblBankOrt)
    Me.PanelControl1.Controls.Add(Me.lblBLZ)
    Me.PanelControl1.Controls.Add(Me.lblBankname)
    Me.PanelControl1.Controls.Add(Me.lblClearing)
    Me.PanelControl1.Controls.Add(Me.txtPLZ)
    Me.PanelControl1.Controls.Add(Me.txtLocation)
    Me.PanelControl1.Controls.Add(Me.txtSwift)
    Me.PanelControl1.Controls.Add(Me.txtBankname)
    Me.PanelControl1.Controls.Add(Me.txtClearingNumber)
    Me.PanelControl1.Controls.Add(Me.groupDataSource)
    Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.PanelControl1.Location = New System.Drawing.Point(5, 5)
    Me.PanelControl1.Name = "PanelControl1"
    Me.PanelControl1.Size = New System.Drawing.Size(709, 489)
    Me.PanelControl1.TabIndex = 215
    '
    'lblSearchInfo
    '
    Me.lblSearchInfo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblSearchInfo.Location = New System.Drawing.Point(503, 119)
    Me.lblSearchInfo.Name = "lblSearchInfo"
    Me.lblSearchInfo.Size = New System.Drawing.Size(175, 44)
    Me.lblSearchInfo.TabIndex = 228
    Me.lblSearchInfo.Text = "Daten werden geladen..."
    Me.lblSearchInfo.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'btnSearch
    '
    Me.btnSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnSearch.Image = CType(resources.GetObject("btnSearch.Image"), System.Drawing.Image)
    Me.btnSearch.Location = New System.Drawing.Point(573, 82)
    Me.btnSearch.Name = "btnSearch"
    Me.btnSearch.Size = New System.Drawing.Size(105, 28)
    Me.btnSearch.TabIndex = 220
    Me.btnSearch.Text = "Suchen"
    '
    'btnClose
    '
    Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnClose.Location = New System.Drawing.Point(573, 15)
    Me.btnClose.Name = "btnClose"
    Me.btnClose.Size = New System.Drawing.Size(105, 28)
    Me.btnClose.TabIndex = 221
    Me.btnClose.Text = "Schliessen"
    '
    'lblFoundItems
    '
    Me.lblFoundItems.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
    Me.lblFoundItems.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
    Me.lblFoundItems.Location = New System.Drawing.Point(5, 233)
    Me.lblFoundItems.Name = "lblFoundItems"
    Me.lblFoundItems.Size = New System.Drawing.Size(108, 13)
    Me.lblFoundItems.TabIndex = 227
    Me.lblFoundItems.Text = "Gefundene Einträge"
    '
    'lblSwift
    '
    Me.lblSwift.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    Me.lblSwift.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
    Me.lblSwift.Location = New System.Drawing.Point(22, 189)
    Me.lblSwift.Name = "lblSwift"
    Me.lblSwift.Size = New System.Drawing.Size(108, 13)
    Me.lblSwift.TabIndex = 226
    Me.lblSwift.Text = "Swift"
    '
    'lblBankOrt
    '
    Me.lblBankOrt.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    Me.lblBankOrt.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
    Me.lblBankOrt.Location = New System.Drawing.Point(22, 163)
    Me.lblBankOrt.Name = "lblBankOrt"
    Me.lblBankOrt.Size = New System.Drawing.Size(108, 13)
    Me.lblBankOrt.TabIndex = 225
    Me.lblBankOrt.Text = "Ort"
    '
    'lblBLZ
    '
    Me.lblBLZ.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    Me.lblBLZ.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
    Me.lblBLZ.Location = New System.Drawing.Point(22, 137)
    Me.lblBLZ.Name = "lblBLZ"
    Me.lblBLZ.Size = New System.Drawing.Size(108, 13)
    Me.lblBLZ.TabIndex = 224
    Me.lblBLZ.Text = "PLZ"
    '
    'lblBankname
    '
    Me.lblBankname.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    Me.lblBankname.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
    Me.lblBankname.Location = New System.Drawing.Point(22, 111)
    Me.lblBankname.Name = "lblBankname"
    Me.lblBankname.Size = New System.Drawing.Size(108, 13)
    Me.lblBankname.TabIndex = 223
    Me.lblBankname.Text = "Bankname"
    '
    'lblClearing
    '
    Me.lblClearing.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    Me.lblClearing.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
    Me.lblClearing.Location = New System.Drawing.Point(22, 85)
    Me.lblClearing.Name = "lblClearing"
    Me.lblClearing.Size = New System.Drawing.Size(108, 13)
    Me.lblClearing.TabIndex = 222
    Me.lblClearing.Text = "Clearingnummer"
    '
    'txtPLZ
    '
    Me.txtPLZ.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtPLZ.Location = New System.Drawing.Point(136, 134)
    Me.txtPLZ.Name = "txtPLZ"
    Me.txtPLZ.Size = New System.Drawing.Size(361, 20)
    Me.txtPLZ.TabIndex = 217
    '
    'txtLocation
    '
    Me.txtLocation.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtLocation.Location = New System.Drawing.Point(136, 160)
    Me.txtLocation.Name = "txtLocation"
    Me.txtLocation.Size = New System.Drawing.Size(361, 20)
    Me.txtLocation.TabIndex = 218
    '
    'txtSwift
    '
    Me.txtSwift.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtSwift.Location = New System.Drawing.Point(136, 186)
    Me.txtSwift.Name = "txtSwift"
    Me.txtSwift.Size = New System.Drawing.Size(361, 20)
    Me.txtSwift.TabIndex = 219
    '
    'txtBankname
    '
    Me.txtBankname.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtBankname.Location = New System.Drawing.Point(136, 108)
    Me.txtBankname.Name = "txtBankname"
    Me.txtBankname.Size = New System.Drawing.Size(361, 20)
    Me.txtBankname.TabIndex = 216
    '
    'txtClearingNumber
    '
    Me.txtClearingNumber.Location = New System.Drawing.Point(136, 82)
    Me.txtClearingNumber.Name = "txtClearingNumber"
    Me.txtClearingNumber.Size = New System.Drawing.Size(44, 20)
    Me.txtClearingNumber.TabIndex = 215
    '
    'groupDataSource
    '
    Me.groupDataSource.Location = New System.Drawing.Point(15, 8)
    Me.groupDataSource.Name = "groupDataSource"
    Me.groupDataSource.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
    Me.groupDataSource.Properties.Appearance.Options.UseBackColor = True
    Me.groupDataSource.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
    Me.groupDataSource.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "Datenbank vom lokalen Server laden (Intern)..."), New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "Datenbank vom  Sputnik Server laden (SIC)...")})
    Me.groupDataSource.Size = New System.Drawing.Size(373, 56)
    Me.groupDataSource.TabIndex = 214
    '
    'SearchBankData
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(719, 499)
    Me.Controls.Add(Me.PanelControl1)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.MinimumSize = New System.Drawing.Size(562, 476)
    Me.Name = "SearchBankData"
    Me.Padding = New System.Windows.Forms.Padding(5)
    Me.Text = "Nach Bankdaten suchen"
    CType(Me.gridBank, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.gvBank, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.PanelControl1.ResumeLayout(False)
    CType(Me.txtPLZ.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txtLocation.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txtSwift.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txtBankname.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txtClearingNumber.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.groupDataSource.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents gridBank As DevExpress.XtraGrid.GridControl
  Friend WithEvents gvBank As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
  Friend WithEvents btnSearch As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents btnClose As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents lblFoundItems As DevExpress.XtraEditors.LabelControl
  Friend WithEvents lblSwift As DevExpress.XtraEditors.LabelControl
  Friend WithEvents lblBankOrt As DevExpress.XtraEditors.LabelControl
  Friend WithEvents lblBLZ As DevExpress.XtraEditors.LabelControl
  Friend WithEvents lblBankname As DevExpress.XtraEditors.LabelControl
  Friend WithEvents lblClearing As DevExpress.XtraEditors.LabelControl
  Friend WithEvents txtPLZ As DevExpress.XtraEditors.TextEdit
  Friend WithEvents txtLocation As DevExpress.XtraEditors.TextEdit
  Friend WithEvents txtSwift As DevExpress.XtraEditors.TextEdit
  Friend WithEvents txtBankname As DevExpress.XtraEditors.TextEdit
  Friend WithEvents txtClearingNumber As DevExpress.XtraEditors.TextEdit
  Friend WithEvents groupDataSource As DevExpress.XtraEditors.RadioGroup
  Friend WithEvents lblSearchInfo As System.Windows.Forms.Label
End Class
