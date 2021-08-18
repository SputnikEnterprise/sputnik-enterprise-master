<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMFakListSearch_LV
  Inherits System.Windows.Forms.Form

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
    Me.components = New System.ComponentModel.Container
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMFakListSearch_LV))
    Me.StatusStrip1 = New System.Windows.Forms.StatusStrip
    Me.LblState_1 = New System.Windows.Forms.ToolStripStatusLabel
    Me.btnTotalbetrag = New System.Windows.Forms.ToolStripDropDownButton
    Me.LvFoundedrecs = New System.Windows.Forms.ListView
    Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
    Me.TestToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
    Me.Test1ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
    Me.StatusStrip1.SuspendLayout()
    Me.ContextMenuStrip1.SuspendLayout()
    Me.SuspendLayout()
    '
    'StatusStrip1
    '
    Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LblState_1, Me.btnTotalbetrag})
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
    'btnTotalbetrag
    '
    Me.btnTotalbetrag.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
    Me.btnTotalbetrag.Image = CType(resources.GetObject("btnTotalbetrag.Image"), System.Drawing.Image)
    Me.btnTotalbetrag.ImageTransparentColor = System.Drawing.Color.Magenta
    Me.btnTotalbetrag.Name = "btnTotalbetrag"
    Me.btnTotalbetrag.Size = New System.Drawing.Size(87, 20)
    Me.btnTotalbetrag.Text = "Totalbeträge"
    '
    'LvFoundedrecs
    '
    Me.LvFoundedrecs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.LvFoundedrecs.FullRowSelect = True
    Me.LvFoundedrecs.Location = New System.Drawing.Point(0, 0)
    Me.LvFoundedrecs.MultiSelect = False
    Me.LvFoundedrecs.Name = "LvFoundedrecs"
    Me.LvFoundedrecs.ShowItemToolTips = True
    Me.LvFoundedrecs.Size = New System.Drawing.Size(414, 611)
    Me.LvFoundedrecs.TabIndex = 6
    Me.LvFoundedrecs.UseCompatibleStateImageBehavior = False
    Me.LvFoundedrecs.View = System.Windows.Forms.View.Details
    Me.LvFoundedrecs.VirtualMode = True
    '
    'ContextMenuStrip1
    '
    Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TestToolStripMenuItem, Me.Test1ToolStripMenuItem})
    Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
    Me.ContextMenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
    Me.ContextMenuStrip1.Size = New System.Drawing.Size(238, 48)
    '
    'TestToolStripMenuItem
    '
    Me.TestToolStripMenuItem.Name = "TestToolStripMenuItem"
    Me.TestToolStripMenuItem.Size = New System.Drawing.Size(237, 22)
    Me.TestToolStripMenuItem.Text = "test                                                "
    '
    'Test1ToolStripMenuItem
    '
    Me.Test1ToolStripMenuItem.Name = "Test1ToolStripMenuItem"
    Me.Test1ToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
    Me.Test1ToolStripMenuItem.Text = "test1"
    '
    'frmMFakListSearch_LV
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(414, 636)
    Me.Controls.Add(Me.StatusStrip1)
    Me.Controls.Add(Me.LvFoundedrecs)
    Me.Name = "frmMFakListSearch_LV"
    Me.ShowIcon = False
    Me.ShowInTaskbar = False
    Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
    Me.Text = "Details"
    Me.StatusStrip1.ResumeLayout(False)
    Me.StatusStrip1.PerformLayout()
    Me.ContextMenuStrip1.ResumeLayout(False)
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
  Friend WithEvents LblState_1 As System.Windows.Forms.ToolStripStatusLabel
  Friend WithEvents LvFoundedrecs As System.Windows.Forms.ListView
  Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
  Friend WithEvents TestToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
  Friend WithEvents Test1ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
  Friend WithEvents btnTotalbetrag As System.Windows.Forms.ToolStripDropDownButton
End Class
