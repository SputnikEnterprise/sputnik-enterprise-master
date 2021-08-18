Namespace UI

  <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
  Partial Class frmInvalidRecordNumbers
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
			Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInvalidRecordNumbers))
			Me.grdInvalidRecordNumbers = New DevExpress.XtraGrid.GridControl()
			Me.gvInvalidRecordNumbers = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.lblInvalidRecords = New DevExpress.XtraEditors.LabelControl()
			CType(Me.grdInvalidRecordNumbers, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvInvalidRecordNumbers, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			'
			'grdInvalidRecordNumbers
			'
			Me.grdInvalidRecordNumbers.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.grdInvalidRecordNumbers.Location = New System.Drawing.Point(12, 111)
			Me.grdInvalidRecordNumbers.MainView = Me.gvInvalidRecordNumbers
			Me.grdInvalidRecordNumbers.Name = "grdInvalidRecordNumbers"
			Me.grdInvalidRecordNumbers.Size = New System.Drawing.Size(319, 309)
			Me.grdInvalidRecordNumbers.TabIndex = 288
			Me.grdInvalidRecordNumbers.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvInvalidRecordNumbers})
			'
			'gvInvalidRecordNumbers
			'
			Me.gvInvalidRecordNumbers.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			Me.gvInvalidRecordNumbers.GridControl = Me.grdInvalidRecordNumbers
			Me.gvInvalidRecordNumbers.Name = "gvInvalidRecordNumbers"
			Me.gvInvalidRecordNumbers.OptionsBehavior.Editable = False
			Me.gvInvalidRecordNumbers.OptionsSelection.EnableAppearanceFocusedCell = False
			Me.gvInvalidRecordNumbers.OptionsView.ShowGroupPanel = False
			'
			'lblInvalidRecords
			'
			Me.lblInvalidRecords.AllowHtmlString = True
			Me.lblInvalidRecords.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.lblInvalidRecords.Appearance.Options.UseTextOptions = True
			Me.lblInvalidRecords.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
			Me.lblInvalidRecords.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
			Me.lblInvalidRecords.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblInvalidRecords.Location = New System.Drawing.Point(12, 12)
			Me.lblInvalidRecords.Name = "lblInvalidRecords"
			Me.lblInvalidRecords.Size = New System.Drawing.Size(319, 93)
			Me.lblInvalidRecords.TabIndex = 296
			Me.lblInvalidRecords.Text = resources.GetString("lblInvalidRecords.Text")
			'
			'frmInvalidRecordNumbers
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(343, 432)
			Me.Controls.Add(Me.lblInvalidRecords)
			Me.Controls.Add(Me.grdInvalidRecordNumbers)
			Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
			Me.Name = "frmInvalidRecordNumbers"
			Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
			Me.Text = "Fehlende Daten"
			CType(Me.grdInvalidRecordNumbers, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvInvalidRecordNumbers, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub
		Friend WithEvents grdInvalidRecordNumbers As DevExpress.XtraGrid.GridControl
    Friend WithEvents gvInvalidRecordNumbers As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents lblInvalidRecords As DevExpress.XtraEditors.LabelControl
  End Class

End Namespace
