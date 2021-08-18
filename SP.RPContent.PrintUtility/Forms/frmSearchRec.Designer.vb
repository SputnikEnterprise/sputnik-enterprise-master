<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSearchRec
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSearchRec))
    Me.cmdOK = New DevExpress.XtraEditors.SimpleButton()
    Me.CmdClose = New DevExpress.XtraEditors.SimpleButton()
    Me.Label3 = New System.Windows.Forms.Label()
    Me.GroupBox1 = New DevExpress.XtraEditors.PanelControl()
    Me.pnlMain = New DevExpress.XtraEditors.PanelControl()
    Me.grdRP = New DevExpress.XtraGrid.GridControl()
    Me.gvRP = New DevExpress.XtraGrid.Views.Grid.GridView()
    CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.GroupBox1.SuspendLayout()
    CType(Me.pnlMain, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlMain.SuspendLayout()
    CType(Me.grdRP, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.gvRP, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'cmdOK
    '
    Me.cmdOK.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.cmdOK.Location = New System.Drawing.Point(434, 77)
    Me.cmdOK.Name = "cmdOK"
    Me.cmdOK.Size = New System.Drawing.Size(80, 25)
    Me.cmdOK.TabIndex = 210
    Me.cmdOK.Text = "Auswählen"
    '
    'CmdClose
    '
    Me.CmdClose.Anchor = System.Windows.Forms.AnchorStyles.Right
		Me.CmdClose.DialogResult = DialogResult.Cancel
		Me.CmdClose.Location = New System.Drawing.Point(434, 12)
    Me.CmdClose.Name = "CmdClose"
    Me.CmdClose.Size = New System.Drawing.Size(80, 25)
    Me.CmdClose.TabIndex = 204
    Me.CmdClose.Text = "Schliessen"
    '
    'Label3
    '
    Me.Label3.BackColor = System.Drawing.Color.Transparent
    Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label3.Image = CType(resources.GetObject("Label3.Image"), System.Drawing.Image)
    Me.Label3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.Label3.Location = New System.Drawing.Point(12, 13)
    Me.Label3.Name = "Label3"
    Me.Label3.Size = New System.Drawing.Size(225, 31)
    Me.Label3.TabIndex = 37
    Me.Label3.Text = "Suche nach Datensätze"
    Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
    '
    'GroupBox1
    '
    Me.GroupBox1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003
    Me.GroupBox1.Controls.Add(Me.CmdClose)
    Me.GroupBox1.Controls.Add(Me.Label3)
    Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
    Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
    Me.GroupBox1.Name = "GroupBox1"
    Me.GroupBox1.Size = New System.Drawing.Size(530, 59)
    Me.GroupBox1.TabIndex = 209
    '
    'pnlMain
    '
    Me.pnlMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlMain.Controls.Add(Me.grdRP)
    Me.pnlMain.Location = New System.Drawing.Point(0, 77)
    Me.pnlMain.Name = "pnlMain"
    Me.pnlMain.Size = New System.Drawing.Size(417, 549)
    Me.pnlMain.TabIndex = 217
    '
    'grdRP
    '
    Me.grdRP.Dock = System.Windows.Forms.DockStyle.Fill
    Me.grdRP.Location = New System.Drawing.Point(2, 2)
    Me.grdRP.MainView = Me.gvRP
    Me.grdRP.Name = "grdRP"
    Me.grdRP.Size = New System.Drawing.Size(413, 545)
    Me.grdRP.TabIndex = 3
    Me.grdRP.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvRP})
    '
    'gvRP
    '
    Me.gvRP.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
    Me.gvRP.GridControl = Me.grdRP
    Me.gvRP.Name = "gvRP"
    Me.gvRP.OptionsBehavior.Editable = False
    Me.gvRP.OptionsSelection.EnableAppearanceFocusedCell = False
    Me.gvRP.OptionsView.ShowAutoFilterRow = True
    Me.gvRP.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
    Me.gvRP.OptionsView.ShowGroupPanel = False
    Me.gvRP.OptionsView.ShowIndicator = False
    '
    'frmSearchRec
    '
    Me.AcceptButton = Me.cmdOK
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.CancelButton = Me.CmdClose
    Me.ClientSize = New System.Drawing.Size(530, 627)
    Me.Controls.Add(Me.pnlMain)
    Me.Controls.Add(Me.cmdOK)
    Me.Controls.Add(Me.GroupBox1)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.MaximizeBox = False
    Me.Name = "frmSearchRec"
    Me.Text = "Suche nach Datensätze"
    CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.GroupBox1.ResumeLayout(False)
    CType(Me.pnlMain, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlMain.ResumeLayout(False)
    CType(Me.grdRP, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.gvRP, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents cmdOK As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents CmdClose As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents Label3 As System.Windows.Forms.Label
  Friend WithEvents GroupBox1 As DevExpress.XtraEditors.PanelControl
  Friend WithEvents pnlMain As DevExpress.XtraEditors.PanelControl
  Friend WithEvents grdRP As DevExpress.XtraGrid.GridControl
  Friend WithEvents gvRP As DevExpress.XtraGrid.Views.Grid.GridView
End Class
