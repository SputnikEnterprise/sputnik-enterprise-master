
Namespace UI
  <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
  Partial Class ucReportDetailData2
    Inherits ucBaseControl

    'UserControl overrides dispose to clean up the component list.
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
      Me.gridGavDetailData = New DevExpress.XtraGrid.GridControl()
      Me.gvGavDetail = New DevExpress.XtraGrid.Views.Grid.GridView()
      CType(Me.gridGavDetailData, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.gvGavDetail, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'gridGavDetailData
      '
      Me.gridGavDetailData.Dock = System.Windows.Forms.DockStyle.Fill
      Me.gridGavDetailData.Location = New System.Drawing.Point(0, 0)
      Me.gridGavDetailData.MainView = Me.gvGavDetail
      Me.gridGavDetailData.Name = "gridGavDetailData"
      Me.gridGavDetailData.Size = New System.Drawing.Size(323, 374)
      Me.gridGavDetailData.TabIndex = 2
      Me.gridGavDetailData.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvGavDetail})
      '
      'gvGavDetail
      '
      Me.gvGavDetail.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
      Me.gvGavDetail.GridControl = Me.gridGavDetailData
      Me.gvGavDetail.Name = "gvGavDetail"
      Me.gvGavDetail.OptionsBehavior.Editable = False
      Me.gvGavDetail.OptionsSelection.EnableAppearanceFocusedCell = False
      Me.gvGavDetail.OptionsView.ShowGroupPanel = False
      '
      'ucReportDetailData2
      '
      Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
      Me.Controls.Add(Me.gridGavDetailData)
      Me.Name = "ucReportDetailData2"
      Me.Size = New System.Drawing.Size(323, 374)
      CType(Me.gridGavDetailData, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.gvGavDetail, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gridGavDetailData As DevExpress.XtraGrid.GridControl
    Friend WithEvents gvGavDetail As DevExpress.XtraGrid.Views.Grid.GridView

  End Class

End Namespace

