
Namespace UI

  <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
  Partial Class ucTimetableAndInfoData
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
      Me.btnDone = New DevExpress.XtraEditors.SimpleButton()
      Me.grdTotalHours = New DevExpress.XtraGrid.GridControl()
      Me.gvTotalHours = New DevExpress.XtraGrid.Views.Grid.GridView()
      CType(Me.grdTotalHours, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.gvTotalHours, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'btnDone
      '
      Me.btnDone.Dock = System.Windows.Forms.DockStyle.Top
      Me.btnDone.Location = New System.Drawing.Point(0, 0)
      Me.btnDone.Name = "btnDone"
      Me.btnDone.Size = New System.Drawing.Size(299, 28)
      Me.btnDone.TabIndex = 9
      Me.btnDone.Text = "Rapport ist vollständig"
      '
      'grdTotalHours
      '
      Me.grdTotalHours.Dock = System.Windows.Forms.DockStyle.Fill
      Me.grdTotalHours.Location = New System.Drawing.Point(0, 28)
      Me.grdTotalHours.MainView = Me.gvTotalHours
      Me.grdTotalHours.Name = "grdTotalHours"
      Me.grdTotalHours.Size = New System.Drawing.Size(299, 471)
      Me.grdTotalHours.TabIndex = 10
      Me.grdTotalHours.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvTotalHours})
      '
      'gvTotalHours
      '
      Me.gvTotalHours.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
      Me.gvTotalHours.GridControl = Me.grdTotalHours
      Me.gvTotalHours.Name = "gvTotalHours"
      Me.gvTotalHours.OptionsBehavior.Editable = False
      Me.gvTotalHours.OptionsSelection.EnableAppearanceFocusedCell = False
      Me.gvTotalHours.OptionsView.ShowGroupPanel = False
      '
      'ucTimetableAndInfoData
      '
      Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
      Me.Controls.Add(Me.grdTotalHours)
      Me.Controls.Add(Me.btnDone)
      Me.Name = "ucTimetableAndInfoData"
      Me.Size = New System.Drawing.Size(299, 499)
      CType(Me.grdTotalHours, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.gvTotalHours, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnDone As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents grdTotalHours As DevExpress.XtraGrid.GridControl
    Friend WithEvents gvTotalHours As DevExpress.XtraGrid.Views.Grid.GridView

  End Class

End Namespace
