<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSolvencyResult
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSolvencyResult))
    Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
    Me.lblentscheidung = New DevExpress.XtraEditors.LabelControl()
    Me.lblgefundeneadresse = New DevExpress.XtraEditors.LabelControl()
    Me.lblUser = New DevExpress.XtraEditors.LabelControl()
    Me.btnShowSolvencyPDFReport = New DevExpress.XtraEditors.SimpleButton()
    Me.lblTypeOfQuery = New DevExpress.XtraEditors.LabelControl()
    Me.lblDateValue = New DevExpress.XtraEditors.LabelControl()
    Me.lblbenutzer = New DevExpress.XtraEditors.LabelControl()
    Me.lblartAbfrage = New DevExpress.XtraEditors.LabelControl()
    Me.lbldatum = New DevExpress.XtraEditors.LabelControl()
    Me.memoFoundedAddress = New DevExpress.XtraEditors.MemoEdit()
    Me.btnSolvencyDecision = New DevExpress.XtraEditors.SimpleButton()
    Me.txtDecisionText = New DevExpress.XtraEditors.MemoEdit()
    Me.grdDebts = New DevExpress.XtraGrid.GridControl()
    Me.gvDebts = New DevExpress.XtraGrid.Views.Grid.GridView()
    Me.SplitContainerControl1 = New DevExpress.XtraEditors.SplitContainerControl()
    Me.grpzahlungserfahrung = New DevExpress.XtraEditors.GroupControl()
    CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.PanelControl1.SuspendLayout()
    CType(Me.memoFoundedAddress.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txtDecisionText.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.grdDebts, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.gvDebts, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.SplitContainerControl1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SplitContainerControl1.SuspendLayout()
    CType(Me.grpzahlungserfahrung, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.grpzahlungserfahrung.SuspendLayout()
    Me.SuspendLayout()
    '
    'PanelControl1
    '
    Me.PanelControl1.Controls.Add(Me.lblentscheidung)
    Me.PanelControl1.Controls.Add(Me.lblgefundeneadresse)
    Me.PanelControl1.Controls.Add(Me.lblUser)
    Me.PanelControl1.Controls.Add(Me.btnShowSolvencyPDFReport)
    Me.PanelControl1.Controls.Add(Me.lblTypeOfQuery)
    Me.PanelControl1.Controls.Add(Me.lblDateValue)
    Me.PanelControl1.Controls.Add(Me.lblbenutzer)
    Me.PanelControl1.Controls.Add(Me.lblartAbfrage)
    Me.PanelControl1.Controls.Add(Me.lbldatum)
    Me.PanelControl1.Controls.Add(Me.memoFoundedAddress)
    Me.PanelControl1.Controls.Add(Me.btnSolvencyDecision)
    Me.PanelControl1.Controls.Add(Me.txtDecisionText)
    Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.PanelControl1.Location = New System.Drawing.Point(5, 5)
    Me.PanelControl1.Name = "PanelControl1"
    Me.PanelControl1.Size = New System.Drawing.Size(814, 158)
    Me.PanelControl1.TabIndex = 0
    '
    'lblentscheidung
    '
    Me.lblentscheidung.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    Me.lblentscheidung.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
    Me.lblentscheidung.Location = New System.Drawing.Point(308, 85)
    Me.lblentscheidung.Name = "lblentscheidung"
    Me.lblentscheidung.Size = New System.Drawing.Size(118, 13)
    Me.lblentscheidung.TabIndex = 222
    Me.lblentscheidung.Text = "Entscheidung"
    '
    'lblgefundeneadresse
    '
    Me.lblgefundeneadresse.Location = New System.Drawing.Point(18, 9)
    Me.lblgefundeneadresse.Name = "lblgefundeneadresse"
    Me.lblgefundeneadresse.Size = New System.Drawing.Size(95, 13)
    Me.lblgefundeneadresse.TabIndex = 221
    Me.lblgefundeneadresse.Text = "Gefundene Adresse"
    '
    'lblUser
    '
    Me.lblUser.Location = New System.Drawing.Point(432, 66)
    Me.lblUser.Name = "lblUser"
    Me.lblUser.Size = New System.Drawing.Size(43, 13)
    Me.lblUser.TabIndex = 220
    Me.lblUser.Text = "Benutzer"
    '
    'btnShowSolvencyPDFReport
    '
    Me.btnShowSolvencyPDFReport.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnShowSolvencyPDFReport.Location = New System.Drawing.Point(683, 85)
    Me.btnShowSolvencyPDFReport.Name = "btnShowSolvencyPDFReport"
    Me.btnShowSolvencyPDFReport.Size = New System.Drawing.Size(117, 33)
    Me.btnShowSolvencyPDFReport.TabIndex = 1
    Me.btnShowSolvencyPDFReport.Text = "Bericht anzeigen"
    '
    'lblTypeOfQuery
    '
    Me.lblTypeOfQuery.Location = New System.Drawing.Point(432, 47)
    Me.lblTypeOfQuery.Name = "lblTypeOfQuery"
    Me.lblTypeOfQuery.Size = New System.Drawing.Size(15, 13)
    Me.lblTypeOfQuery.TabIndex = 219
    Me.lblTypeOfQuery.Text = "Art"
    '
    'lblDateValue
    '
    Me.lblDateValue.Location = New System.Drawing.Point(432, 28)
    Me.lblDateValue.Name = "lblDateValue"
    Me.lblDateValue.Size = New System.Drawing.Size(31, 13)
    Me.lblDateValue.TabIndex = 218
    Me.lblDateValue.Text = "Datum"
    '
    'lblbenutzer
    '
    Me.lblbenutzer.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    Me.lblbenutzer.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
    Me.lblbenutzer.Location = New System.Drawing.Point(308, 66)
    Me.lblbenutzer.Name = "lblbenutzer"
    Me.lblbenutzer.Size = New System.Drawing.Size(118, 13)
    Me.lblbenutzer.TabIndex = 217
    Me.lblbenutzer.Text = "Benutzer"
    '
    'lblartAbfrage
    '
    Me.lblartAbfrage.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    Me.lblartAbfrage.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
    Me.lblartAbfrage.Location = New System.Drawing.Point(308, 47)
    Me.lblartAbfrage.Name = "lblartAbfrage"
    Me.lblartAbfrage.Size = New System.Drawing.Size(118, 13)
    Me.lblartAbfrage.TabIndex = 216
    Me.lblartAbfrage.Text = "Art der Abfrage"
    '
    'lbldatum
    '
    Me.lbldatum.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    Me.lbldatum.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
    Me.lbldatum.Location = New System.Drawing.Point(308, 28)
    Me.lbldatum.Name = "lbldatum"
    Me.lbldatum.Size = New System.Drawing.Size(118, 13)
    Me.lbldatum.TabIndex = 215
    Me.lbldatum.Text = "Datum"
    '
    'memoFoundedAddress
    '
    Me.memoFoundedAddress.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.memoFoundedAddress.Location = New System.Drawing.Point(18, 28)
    Me.memoFoundedAddress.Name = "memoFoundedAddress"
    Me.memoFoundedAddress.Size = New System.Drawing.Size(280, 118)
    Me.memoFoundedAddress.TabIndex = 214
    '
    'btnSolvencyDecision
    '
    Me.btnSolvencyDecision.AllowFocus = False
    Me.btnSolvencyDecision.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnSolvencyDecision.Appearance.BackColor = System.Drawing.Color.Transparent
    Me.btnSolvencyDecision.Appearance.Options.UseBackColor = True
    Me.btnSolvencyDecision.Appearance.Options.UseImage = True
    Me.btnSolvencyDecision.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat
    Me.btnSolvencyDecision.Image = CType(resources.GetObject("btnSolvencyDecision.Image"), System.Drawing.Image)
    Me.btnSolvencyDecision.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter
    Me.btnSolvencyDecision.Location = New System.Drawing.Point(732, 30)
    Me.btnSolvencyDecision.Margin = New System.Windows.Forms.Padding(1)
    Me.btnSolvencyDecision.Name = "btnSolvencyDecision"
    Me.btnSolvencyDecision.Size = New System.Drawing.Size(37, 44)
    Me.btnSolvencyDecision.TabIndex = 213
    '
    'txtDecisionText
    '
    Me.txtDecisionText.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtDecisionText.Location = New System.Drawing.Point(432, 85)
    Me.txtDecisionText.Name = "txtDecisionText"
    Me.txtDecisionText.Size = New System.Drawing.Size(233, 61)
    Me.txtDecisionText.TabIndex = 0
    '
    'grdDebts
    '
    Me.grdDebts.Dock = System.Windows.Forms.DockStyle.Fill
    Me.grdDebts.Location = New System.Drawing.Point(2, 21)
    Me.grdDebts.MainView = Me.gvDebts
    Me.grdDebts.Name = "grdDebts"
    Me.grdDebts.Size = New System.Drawing.Size(810, 152)
    Me.grdDebts.TabIndex = 220
    Me.grdDebts.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvDebts})
    '
    'gvDebts
    '
    Me.gvDebts.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
    Me.gvDebts.GridControl = Me.grdDebts
    Me.gvDebts.Name = "gvDebts"
    Me.gvDebts.OptionsBehavior.Editable = False
    Me.gvDebts.OptionsSelection.EnableAppearanceFocusedCell = False
    Me.gvDebts.OptionsView.ShowGroupPanel = False
    Me.gvDebts.OptionsView.ShowIndicator = False
    '
    'SplitContainerControl1
    '
    Me.SplitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.SplitContainerControl1.Horizontal = False
    Me.SplitContainerControl1.Location = New System.Drawing.Point(0, 0)
    Me.SplitContainerControl1.Name = "SplitContainerControl1"
    Me.SplitContainerControl1.Panel1.Controls.Add(Me.PanelControl1)
    Me.SplitContainerControl1.Panel1.Padding = New System.Windows.Forms.Padding(5)
    Me.SplitContainerControl1.Panel1.Text = "Panel1"
    Me.SplitContainerControl1.Panel2.Controls.Add(Me.grpzahlungserfahrung)
    Me.SplitContainerControl1.Panel2.Padding = New System.Windows.Forms.Padding(5)
    Me.SplitContainerControl1.Panel2.Text = "Panel2"
    Me.SplitContainerControl1.Size = New System.Drawing.Size(824, 358)
    Me.SplitContainerControl1.SplitterPosition = 168
    Me.SplitContainerControl1.TabIndex = 222
    Me.SplitContainerControl1.Text = "SplitContainerControl1"
    '
    'grpzahlungserfahrung
    '
    Me.grpzahlungserfahrung.Controls.Add(Me.grdDebts)
    Me.grpzahlungserfahrung.Dock = System.Windows.Forms.DockStyle.Fill
    Me.grpzahlungserfahrung.Location = New System.Drawing.Point(5, 5)
    Me.grpzahlungserfahrung.Name = "grpzahlungserfahrung"
    Me.grpzahlungserfahrung.Size = New System.Drawing.Size(814, 175)
    Me.grpzahlungserfahrung.TabIndex = 0
    Me.grpzahlungserfahrung.Text = "Zahlungserfahrungen"
    '
    'frmSolvencyResult
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(824, 358)
    Me.Controls.Add(Me.SplitContainerControl1)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.MinimumSize = New System.Drawing.Size(830, 386)
    Me.Name = "frmSolvencyResult"
    Me.Text = "Bonitätsabfrage"
    CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.PanelControl1.ResumeLayout(False)
    Me.PanelControl1.PerformLayout()
    CType(Me.memoFoundedAddress.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txtDecisionText.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.grdDebts, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.gvDebts, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.SplitContainerControl1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.SplitContainerControl1.ResumeLayout(False)
    CType(Me.grpzahlungserfahrung, System.ComponentModel.ISupportInitialize).EndInit()
    Me.grpzahlungserfahrung.ResumeLayout(False)
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
  Friend WithEvents txtDecisionText As DevExpress.XtraEditors.MemoEdit
  Friend WithEvents btnShowSolvencyPDFReport As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents btnSolvencyDecision As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents lblbenutzer As DevExpress.XtraEditors.LabelControl
  Friend WithEvents lblartAbfrage As DevExpress.XtraEditors.LabelControl
  Friend WithEvents lbldatum As DevExpress.XtraEditors.LabelControl
  Friend WithEvents memoFoundedAddress As DevExpress.XtraEditors.MemoEdit
  Friend WithEvents lblDateValue As DevExpress.XtraEditors.LabelControl
  Friend WithEvents lblUser As DevExpress.XtraEditors.LabelControl
  Friend WithEvents lblTypeOfQuery As DevExpress.XtraEditors.LabelControl
  Friend WithEvents lblgefundeneadresse As DevExpress.XtraEditors.LabelControl
  Friend WithEvents grdDebts As DevExpress.XtraGrid.GridControl
  Friend WithEvents gvDebts As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents lblentscheidung As DevExpress.XtraEditors.LabelControl
  Friend WithEvents SplitContainerControl1 As DevExpress.XtraEditors.SplitContainerControl
  Friend WithEvents grpzahlungserfahrung As DevExpress.XtraEditors.GroupControl
End Class
