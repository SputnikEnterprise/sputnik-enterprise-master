<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFibuSearch_LV
  Inherits DevExpress.XtraEditors.XtraForm

  'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
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

  'Wird vom Windows Form-Designer benötigt.
  Private components As System.ComponentModel.IContainer

  'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
  'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
  'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
  <System.Diagnostics.DebuggerStepThrough()> _
  Private Sub InitializeComponent()
    Me.components = New System.ComponentModel.Container()
    Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
    Me.LblState_1 = New System.Windows.Forms.ToolStripStatusLabel()
    Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
    Me.TestToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
    Me.Test1ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
    Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
    Me.StatusStrip1.SuspendLayout()
    Me.ContextMenuStrip1.SuspendLayout()
    CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'StatusStrip1
    '
    Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LblState_1})
    Me.StatusStrip1.Location = New System.Drawing.Point(0, 614)
    Me.StatusStrip1.Name = "StatusStrip1"
    Me.StatusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode
    Me.StatusStrip1.Size = New System.Drawing.Size(414, 22)
    Me.StatusStrip1.TabIndex = 7
    Me.StatusStrip1.Text = "StatusStrip1"
    '
    'LblState_1
    '
    Me.LblState_1.AutoSize = False
    Me.LblState_1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.LblState_1.Name = "LblState_1"
    Me.LblState_1.Size = New System.Drawing.Size(220, 17)
    Me.LblState_1.Text = "Bereit"
    Me.LblState_1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.LblState_1.ToolTipText = "Modul-Status"
    '
    'ContextMenuStrip1
    '
    Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TestToolStripMenuItem, Me.Test1ToolStripMenuItem})
    Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
    Me.ContextMenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
    Me.ContextMenuStrip1.Size = New System.Drawing.Size(100, 48)
    '
    'TestToolStripMenuItem
    '
    Me.TestToolStripMenuItem.Name = "TestToolStripMenuItem"
    Me.TestToolStripMenuItem.Size = New System.Drawing.Size(99, 22)
    Me.TestToolStripMenuItem.Text = "test"
    '
    'Test1ToolStripMenuItem
    '
    Me.Test1ToolStripMenuItem.Name = "Test1ToolStripMenuItem"
    Me.Test1ToolStripMenuItem.Size = New System.Drawing.Size(99, 22)
    Me.Test1ToolStripMenuItem.Text = "test1"
    '
    'PanelControl1
    '
    Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.PanelControl1.Location = New System.Drawing.Point(0, 0)
    Me.PanelControl1.Name = "PanelControl1"
    Me.PanelControl1.Size = New System.Drawing.Size(414, 614)
    Me.PanelControl1.TabIndex = 215
    '
    'frmFibuSearch_LV
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(414, 636)
    Me.Controls.Add(Me.PanelControl1)
    Me.Controls.Add(Me.StatusStrip1)
    Me.Name = "frmFibuSearch_LV"
    Me.ShowIcon = False
    Me.ShowInTaskbar = False
    Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
    Me.Text = "Details"
    Me.StatusStrip1.ResumeLayout(False)
    Me.StatusStrip1.PerformLayout()
    Me.ContextMenuStrip1.ResumeLayout(False)
    CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
  Friend WithEvents LblState_1 As System.Windows.Forms.ToolStripStatusLabel
  Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
  Friend WithEvents TestToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
  Friend WithEvents Test1ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
  Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
End Class
