Namespace UI

  <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
  Partial Class ucNotes
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
      Me.SplitContainerControl2 = New DevExpress.XtraEditors.SplitContainerControl()
      Me.grpEmployee = New DevComponents.DotNetBar.Controls.GroupPanel()
      Me.txtCommentEmployee = New DevExpress.XtraEditors.MemoEdit()
      Me.grpCustomer = New DevComponents.DotNetBar.Controls.GroupPanel()
      Me.txtCommentCustomer = New DevExpress.XtraEditors.MemoEdit()
      CType(Me.SplitContainerControl2, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SplitContainerControl2.SuspendLayout()
      Me.grpEmployee.SuspendLayout()
      CType(Me.txtCommentEmployee.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.grpCustomer.SuspendLayout()
      CType(Me.txtCommentCustomer.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'SplitContainerControl2
      '
      Me.SplitContainerControl2.Dock = System.Windows.Forms.DockStyle.Fill
      Me.SplitContainerControl2.Location = New System.Drawing.Point(0, 0)
      Me.SplitContainerControl2.Name = "SplitContainerControl2"
      Me.SplitContainerControl2.Panel1.Controls.Add(Me.grpEmployee)
      Me.SplitContainerControl2.Panel1.Padding = New System.Windows.Forms.Padding(5)
      Me.SplitContainerControl2.Panel1.Text = "Panel1"
      Me.SplitContainerControl2.Panel2.Controls.Add(Me.grpCustomer)
      Me.SplitContainerControl2.Panel2.Padding = New System.Windows.Forms.Padding(5)
      Me.SplitContainerControl2.Panel2.Text = "Panel2"
      Me.SplitContainerControl2.Size = New System.Drawing.Size(847, 267)
      Me.SplitContainerControl2.SplitterPosition = 416
      Me.SplitContainerControl2.TabIndex = 1
      Me.SplitContainerControl2.Text = "SplitContainerControl2"
      '
      'grpEmployee
      '
      Me.grpEmployee.BackColor = System.Drawing.Color.Transparent
      Me.grpEmployee.CanvasColor = System.Drawing.SystemColors.Control
      Me.grpEmployee.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
      Me.grpEmployee.Controls.Add(Me.txtCommentEmployee)
      Me.grpEmployee.Dock = System.Windows.Forms.DockStyle.Fill
      Me.grpEmployee.Location = New System.Drawing.Point(5, 5)
      Me.grpEmployee.Name = "grpEmployee"
      Me.grpEmployee.Size = New System.Drawing.Size(406, 257)
      '
      '
      '
      Me.grpEmployee.Style.BackColorGradientAngle = 90
      Me.grpEmployee.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
      Me.grpEmployee.Style.BorderBottomWidth = 1
      Me.grpEmployee.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
      Me.grpEmployee.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
      Me.grpEmployee.Style.BorderLeftWidth = 1
      Me.grpEmployee.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
      Me.grpEmployee.Style.BorderRightWidth = 1
      Me.grpEmployee.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
      Me.grpEmployee.Style.BorderTopWidth = 1
      Me.grpEmployee.Style.CornerDiameter = 4
      Me.grpEmployee.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
      Me.grpEmployee.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
      Me.grpEmployee.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
      Me.grpEmployee.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
      Me.grpEmployee.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
      '
      '
      '
      Me.grpEmployee.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
      '
      '
      '
      Me.grpEmployee.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
      Me.grpEmployee.TabIndex = 213
      Me.grpEmployee.Text = "Kandidaten-Bemerkung"
      '
      'txtCommentEmployee
      '
      Me.txtCommentEmployee.Dock = System.Windows.Forms.DockStyle.Fill
      Me.txtCommentEmployee.Location = New System.Drawing.Point(0, 0)
      Me.txtCommentEmployee.Name = "txtCommentEmployee"
      Me.txtCommentEmployee.Size = New System.Drawing.Size(400, 235)
      Me.txtCommentEmployee.TabIndex = 2
      '
      'grpCustomer
      '
      Me.grpCustomer.BackColor = System.Drawing.Color.Transparent
      Me.grpCustomer.CanvasColor = System.Drawing.SystemColors.Control
      Me.grpCustomer.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
      Me.grpCustomer.Controls.Add(Me.txtCommentCustomer)
      Me.grpCustomer.Dock = System.Windows.Forms.DockStyle.Fill
      Me.grpCustomer.Location = New System.Drawing.Point(5, 5)
      Me.grpCustomer.Name = "grpCustomer"
      Me.grpCustomer.Size = New System.Drawing.Size(416, 257)
      '
      '
      '
      Me.grpCustomer.Style.BackColorGradientAngle = 90
      Me.grpCustomer.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
      Me.grpCustomer.Style.BorderBottomWidth = 1
      Me.grpCustomer.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
      Me.grpCustomer.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
      Me.grpCustomer.Style.BorderLeftWidth = 1
      Me.grpCustomer.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
      Me.grpCustomer.Style.BorderRightWidth = 1
      Me.grpCustomer.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
      Me.grpCustomer.Style.BorderTopWidth = 1
      Me.grpCustomer.Style.CornerDiameter = 4
      Me.grpCustomer.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
      Me.grpCustomer.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
      Me.grpCustomer.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
      Me.grpCustomer.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
      Me.grpCustomer.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
      '
      '
      '
      Me.grpCustomer.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
      '
      '
      '
      Me.grpCustomer.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
      Me.grpCustomer.TabIndex = 214
      Me.grpCustomer.Text = "Kunden-Bemerkung"
      '
      'txtCommentCustomer
      '
      Me.txtCommentCustomer.Dock = System.Windows.Forms.DockStyle.Fill
      Me.txtCommentCustomer.Location = New System.Drawing.Point(0, 0)
      Me.txtCommentCustomer.Name = "txtCommentCustomer"
      Me.txtCommentCustomer.Size = New System.Drawing.Size(410, 235)
      Me.txtCommentCustomer.TabIndex = 28
      '
      'ucNotes
      '
      Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
      Me.Controls.Add(Me.SplitContainerControl2)
      Me.Name = "ucNotes"
      Me.Size = New System.Drawing.Size(847, 267)
      CType(Me.SplitContainerControl2, System.ComponentModel.ISupportInitialize).EndInit()
      Me.SplitContainerControl2.ResumeLayout(False)
      Me.grpEmployee.ResumeLayout(False)
      CType(Me.txtCommentEmployee.Properties, System.ComponentModel.ISupportInitialize).EndInit()
      Me.grpCustomer.ResumeLayout(False)
      CType(Me.txtCommentCustomer.Properties, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainerControl2 As DevExpress.XtraEditors.SplitContainerControl
    Friend WithEvents grpEmployee As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents grpCustomer As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents txtCommentCustomer As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents txtCommentEmployee As DevExpress.XtraEditors.MemoEdit

  End Class

End Namespace
