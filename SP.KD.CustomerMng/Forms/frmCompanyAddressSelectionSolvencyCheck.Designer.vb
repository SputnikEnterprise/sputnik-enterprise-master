<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCompanyAddressSelectionSolvencyCheck
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCompanyAddressSelectionSolvencyCheck))
    Me.grdAddresses = New DevExpress.XtraGrid.GridControl()
    Me.gvAddresses = New DevExpress.XtraGrid.Views.Grid.GridView()
    Me.btnOk = New DevExpress.XtraEditors.SimpleButton()
    Me.lblwaehlen = New DevExpress.XtraEditors.LabelControl()
    Me.btnCancel = New DevExpress.XtraEditors.SimpleButton()
    CType(Me.grdAddresses, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.gvAddresses, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'grdAddresses
    '
    Me.grdAddresses.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.grdAddresses.Location = New System.Drawing.Point(0, 37)
    Me.grdAddresses.MainView = Me.gvAddresses
    Me.grdAddresses.Name = "grdAddresses"
    Me.grdAddresses.Size = New System.Drawing.Size(471, 296)
    Me.grdAddresses.TabIndex = 219
    Me.grdAddresses.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvAddresses})
    '
    'gvAddresses
    '
    Me.gvAddresses.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
    Me.gvAddresses.GridControl = Me.grdAddresses
    Me.gvAddresses.Name = "gvAddresses"
    Me.gvAddresses.OptionsBehavior.Editable = False
    Me.gvAddresses.OptionsSelection.EnableAppearanceFocusedCell = False
    Me.gvAddresses.OptionsView.ShowGroupPanel = False
    '
    'btnOk
    '
    Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnOk.Location = New System.Drawing.Point(303, 350)
    Me.btnOk.Name = "btnOk"
    Me.btnOk.Size = New System.Drawing.Size(75, 23)
    Me.btnOk.TabIndex = 220
    Me.btnOk.Text = "Ok"
    '
    'lblwaehlen
    '
    Me.lblwaehlen.Location = New System.Drawing.Point(12, 18)
    Me.lblwaehlen.Name = "lblwaehlen"
    Me.lblwaehlen.Size = New System.Drawing.Size(351, 13)
    Me.lblwaehlen.TabIndex = 221
    Me.lblwaehlen.Text = "Bitte wählen Sie die Adresse welche Sie für die Prüfung benutzen wollen. "
    '
    'btnCancel
    '
    Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnCancel.Location = New System.Drawing.Point(384, 350)
    Me.btnCancel.Name = "btnCancel"
    Me.btnCancel.Size = New System.Drawing.Size(75, 23)
    Me.btnCancel.TabIndex = 222
    Me.btnCancel.Text = "Abbrechen"
    '
    'frmCompanyAddressSelectionSolvencyCheck
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(471, 385)
    Me.Controls.Add(Me.btnCancel)
    Me.Controls.Add(Me.lblwaehlen)
    Me.Controls.Add(Me.btnOk)
    Me.Controls.Add(Me.grdAddresses)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "frmCompanyAddressSelectionSolvencyCheck"
    Me.Text = "Adresswahl"
    CType(Me.grdAddresses, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.gvAddresses, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents grdAddresses As DevExpress.XtraGrid.GridControl
  Friend WithEvents gvAddresses As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents btnOk As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents lblwaehlen As DevExpress.XtraEditors.LabelControl
  Friend WithEvents btnCancel As DevExpress.XtraEditors.SimpleButton
End Class
