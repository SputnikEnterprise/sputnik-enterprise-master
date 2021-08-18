Namespace UI

  <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
  Partial Class ucMonthlySalaryData
    Inherits ucBaseControlBottomTab

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
      Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ucMonthlySalaryData))
      Me.grpMonthlySalary = New DevExpress.XtraEditors.GroupControl()
      Me.chkThisYear = New DevExpress.XtraEditors.CheckEdit()
      Me.grdExistsMSalary = New DevExpress.XtraGrid.GridControl()
      Me.gvExistsMSalary = New DevExpress.XtraGrid.Views.Grid.GridView()
      Me.btnAddMonthlySalary = New DevExpress.XtraEditors.SimpleButton()
      CType(Me.grpMonthlySalary, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.grpMonthlySalary.SuspendLayout()
      CType(Me.chkThisYear.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.grdExistsMSalary, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.gvExistsMSalary, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'grpMonthlySalary
      '
      Me.grpMonthlySalary.AppearanceCaption.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
      Me.grpMonthlySalary.AppearanceCaption.Options.UseFont = True
      Me.grpMonthlySalary.Controls.Add(Me.chkThisYear)
      Me.grpMonthlySalary.Controls.Add(Me.grdExistsMSalary)
      Me.grpMonthlySalary.Controls.Add(Me.btnAddMonthlySalary)
      Me.grpMonthlySalary.Dock = System.Windows.Forms.DockStyle.Fill
      Me.grpMonthlySalary.Location = New System.Drawing.Point(0, 0)
      Me.grpMonthlySalary.Name = "grpMonthlySalary"
      Me.grpMonthlySalary.Size = New System.Drawing.Size(954, 437)
      Me.grpMonthlySalary.TabIndex = 270
      Me.grpMonthlySalary.Text = "Monatliche Lohnangaben"
      '
      'chkThisYear
      '
      Me.chkThisYear.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
      Me.chkThisYear.Location = New System.Drawing.Point(658, 0)
      Me.chkThisYear.Name = "chkThisYear"
      Me.chkThisYear.Properties.AllowFocused = False
      Me.chkThisYear.Properties.Appearance.Options.UseTextOptions = True
      Me.chkThisYear.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
      Me.chkThisYear.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.[Default]
      Me.chkThisYear.Properties.Caption = "Nur aktuelles Jahr anzeigen"
      Me.chkThisYear.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
      Me.chkThisYear.Size = New System.Drawing.Size(258, 19)
      Me.chkThisYear.TabIndex = 2
      '
      'grdExistsMSalary
      '
      Me.grdExistsMSalary.Dock = System.Windows.Forms.DockStyle.Fill
      Me.grdExistsMSalary.Location = New System.Drawing.Point(2, 21)
      Me.grdExistsMSalary.MainView = Me.gvExistsMSalary
      Me.grdExistsMSalary.Name = "grdExistsMSalary"
      Me.grdExistsMSalary.Size = New System.Drawing.Size(950, 414)
      Me.grdExistsMSalary.TabIndex = 1
      Me.grdExistsMSalary.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvExistsMSalary})
      '
      'gvExistsMSalary
      '
      Me.gvExistsMSalary.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
      Me.gvExistsMSalary.GridControl = Me.grdExistsMSalary
      Me.gvExistsMSalary.Name = "gvExistsMSalary"
      Me.gvExistsMSalary.OptionsBehavior.Editable = False
      Me.gvExistsMSalary.OptionsSelection.EnableAppearanceFocusedCell = False
      Me.gvExistsMSalary.OptionsView.ShowGroupPanel = False
      '
      'btnAddMonthlySalary
      '
      Me.btnAddMonthlySalary.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
      Me.btnAddMonthlySalary.Image = CType(resources.GetObject("btnAddMonthlySalary.Image"), System.Drawing.Image)
      Me.btnAddMonthlySalary.Location = New System.Drawing.Point(922, 3)
      Me.btnAddMonthlySalary.Name = "btnAddMonthlySalary"
      Me.btnAddMonthlySalary.Size = New System.Drawing.Size(27, 15)
      Me.btnAddMonthlySalary.TabIndex = 3
      '
      'ucMonthlySalaryData
      '
      Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
      Me.Controls.Add(Me.grpMonthlySalary)
      Me.Name = "ucMonthlySalaryData"
      Me.Size = New System.Drawing.Size(954, 437)
      CType(Me.grpMonthlySalary, System.ComponentModel.ISupportInitialize).EndInit()
      Me.grpMonthlySalary.ResumeLayout(False)
      CType(Me.chkThisYear.Properties, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.grdExistsMSalary, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.gvExistsMSalary, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpMonthlySalary As DevExpress.XtraEditors.GroupControl
    Friend WithEvents chkThisYear As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents grdExistsMSalary As DevExpress.XtraGrid.GridControl
    Friend WithEvents gvExistsMSalary As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents btnAddMonthlySalary As DevExpress.XtraEditors.SimpleButton

  End Class

End Namespace
