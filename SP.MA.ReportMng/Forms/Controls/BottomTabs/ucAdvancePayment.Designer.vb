Namespace UI

  <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
  Partial Class ucAdvancePayment
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
      Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ucAdvancePayment))
      Me.chkThisYear = New DevExpress.XtraEditors.CheckEdit()
      Me.grdAdvancePayment = New DevExpress.XtraGrid.GridControl()
      Me.gvAdvancePayment = New DevExpress.XtraGrid.Views.Grid.GridView()
      Me.grpAdvancePayment = New DevExpress.XtraEditors.GroupControl()
      Me.btnAddAdvancePayment = New DevExpress.XtraEditors.SimpleButton()
      CType(Me.chkThisYear.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.grdAdvancePayment, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.gvAdvancePayment, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.grpAdvancePayment, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.grpAdvancePayment.SuspendLayout()
      Me.SuspendLayout()
      '
      'chkThisYear
      '
      Me.chkThisYear.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
      Me.chkThisYear.Location = New System.Drawing.Point(664, 0)
      Me.chkThisYear.Name = "chkThisYear"
      Me.chkThisYear.Properties.AllowFocused = False
      Me.chkThisYear.Properties.Appearance.Options.UseTextOptions = True
      Me.chkThisYear.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
      Me.chkThisYear.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.[Default]
      Me.chkThisYear.Properties.Caption = "Nur aktuelles Jahr anzeigen"
      Me.chkThisYear.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
      Me.chkThisYear.Size = New System.Drawing.Size(252, 19)
      Me.chkThisYear.TabIndex = 2
      '
      'grdAdvancePayment
      '
      Me.grdAdvancePayment.Dock = System.Windows.Forms.DockStyle.Fill
      Me.grdAdvancePayment.Location = New System.Drawing.Point(2, 21)
      Me.grdAdvancePayment.MainView = Me.gvAdvancePayment
      Me.grdAdvancePayment.Name = "grdAdvancePayment"
      Me.grdAdvancePayment.Size = New System.Drawing.Size(950, 414)
      Me.grdAdvancePayment.TabIndex = 1
      Me.grdAdvancePayment.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvAdvancePayment})
      '
      'gvAdvancePayment
      '
      Me.gvAdvancePayment.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
      Me.gvAdvancePayment.GridControl = Me.grdAdvancePayment
      Me.gvAdvancePayment.Name = "gvAdvancePayment"
      Me.gvAdvancePayment.OptionsBehavior.Editable = False
      Me.gvAdvancePayment.OptionsSelection.EnableAppearanceFocusedCell = False
      Me.gvAdvancePayment.OptionsView.ShowGroupPanel = False
      '
      'grpAdvancePayment
      '
      Me.grpAdvancePayment.AppearanceCaption.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
      Me.grpAdvancePayment.AppearanceCaption.Options.UseFont = True
      Me.grpAdvancePayment.Controls.Add(Me.chkThisYear)
      Me.grpAdvancePayment.Controls.Add(Me.grdAdvancePayment)
      Me.grpAdvancePayment.Controls.Add(Me.btnAddAdvancePayment)
      Me.grpAdvancePayment.Dock = System.Windows.Forms.DockStyle.Fill
      Me.grpAdvancePayment.Location = New System.Drawing.Point(0, 0)
      Me.grpAdvancePayment.Name = "grpAdvancePayment"
      Me.grpAdvancePayment.Size = New System.Drawing.Size(954, 437)
      Me.grpAdvancePayment.TabIndex = 271
      Me.grpAdvancePayment.Text = "Vorschüsse"
      '
      'btnAddAdvancePayment
      '
      Me.btnAddAdvancePayment.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
      Me.btnAddAdvancePayment.Image = CType(resources.GetObject("btnAddAdvancePayment.Image"), System.Drawing.Image)
      Me.btnAddAdvancePayment.Location = New System.Drawing.Point(922, 3)
      Me.btnAddAdvancePayment.Name = "btnAddAdvancePayment"
      Me.btnAddAdvancePayment.Size = New System.Drawing.Size(27, 15)
      Me.btnAddAdvancePayment.TabIndex = 3
      '
      'ucAdvancePayment
      '
      Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
      Me.Controls.Add(Me.grpAdvancePayment)
      Me.Name = "ucAdvancePayment"
      Me.Size = New System.Drawing.Size(954, 437)
      CType(Me.chkThisYear.Properties, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.grdAdvancePayment, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.gvAdvancePayment, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.grpAdvancePayment, System.ComponentModel.ISupportInitialize).EndInit()
      Me.grpAdvancePayment.ResumeLayout(False)
      Me.ResumeLayout(False)

    End Sub
    Friend WithEvents chkThisYear As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents grdAdvancePayment As DevExpress.XtraGrid.GridControl
    Friend WithEvents gvAdvancePayment As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents grpAdvancePayment As DevExpress.XtraEditors.GroupControl
    Friend WithEvents btnAddAdvancePayment As DevExpress.XtraEditors.SimpleButton

  End Class

End Namespace
