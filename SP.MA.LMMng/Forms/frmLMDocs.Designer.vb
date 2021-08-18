<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLMDocs
  Inherits DevExpress.XtraEditors.XtraForm

  'UserControl überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLMDocs))
		Me.SplitContainerControl2 = New DevExpress.XtraEditors.SplitContainerControl()
		Me.btnSaveLMDocument = New DevExpress.XtraEditors.SimpleButton()
		Me.lblBezeichnung = New DevExpress.XtraEditors.LabelControl()
		Me.grdLMDocument = New DevExpress.XtraGrid.GridControl()
		Me.gvLMDocument = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.txtLMFile = New DevExpress.XtraEditors.ComboBoxEdit()
		CType(Me.SplitContainerControl2, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SplitContainerControl2.SuspendLayout()
		CType(Me.grdLMDocument, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvLMDocument, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtLMFile.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'SplitContainerControl2
		'
		Me.SplitContainerControl2.Dock = System.Windows.Forms.DockStyle.Fill
		Me.SplitContainerControl2.Horizontal = False
		Me.SplitContainerControl2.Location = New System.Drawing.Point(0, 0)
		Me.SplitContainerControl2.Name = "SplitContainerControl2"
		Me.SplitContainerControl2.Panel1.Controls.Add(Me.btnSaveLMDocument)
		Me.SplitContainerControl2.Panel1.Controls.Add(Me.lblBezeichnung)
		Me.SplitContainerControl2.Panel1.Controls.Add(Me.txtLMFile)
		Me.SplitContainerControl2.Panel1.Padding = New System.Windows.Forms.Padding(5)
		Me.SplitContainerControl2.Panel1.Text = "Panel1"
		Me.SplitContainerControl2.Panel2.Controls.Add(Me.grdLMDocument)
		Me.SplitContainerControl2.Panel2.Padding = New System.Windows.Forms.Padding(5)
		Me.SplitContainerControl2.Panel2.Text = "Panel2"
		Me.SplitContainerControl2.Size = New System.Drawing.Size(563, 321)
		Me.SplitContainerControl2.SplitterPosition = 49
		Me.SplitContainerControl2.TabIndex = 39
		Me.SplitContainerControl2.Text = "SplitContainerControl2"
		'
		'btnSaveLMDocument
		'
		Me.btnSaveLMDocument.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnSaveLMDocument.Image = CType(resources.GetObject("btnSaveLMDocument.Image"), System.Drawing.Image)
		Me.btnSaveLMDocument.Location = New System.Drawing.Point(446, 17)
		Me.btnSaveLMDocument.Name = "btnSaveLMDocument"
		Me.btnSaveLMDocument.Size = New System.Drawing.Size(105, 28)
		Me.btnSaveLMDocument.TabIndex = 38
		Me.btnSaveLMDocument.Text = "Speichern"
		'
		'lblBezeichnung
		'
		Me.lblBezeichnung.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblBezeichnung.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblBezeichnung.Location = New System.Drawing.Point(12, 20)
		Me.lblBezeichnung.Name = "lblBezeichnung"
		Me.lblBezeichnung.Size = New System.Drawing.Size(111, 13)
		Me.lblBezeichnung.TabIndex = 37
		Me.lblBezeichnung.Text = "Bezeichnung"
		'
		'grdLMDocument
		'
		Me.grdLMDocument.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdLMDocument.Location = New System.Drawing.Point(5, 5)
		Me.grdLMDocument.MainView = Me.gvLMDocument
		Me.grdLMDocument.Name = "grdLMDocument"
		Me.grdLMDocument.Size = New System.Drawing.Size(553, 257)
		Me.grdLMDocument.TabIndex = 5
		Me.grdLMDocument.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvLMDocument})
		'
		'gvLMDocument
		'
		Me.gvLMDocument.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvLMDocument.GridControl = Me.grdLMDocument
		Me.gvLMDocument.Name = "gvLMDocument"
		Me.gvLMDocument.OptionsBehavior.Editable = False
		Me.gvLMDocument.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvLMDocument.OptionsView.ShowGroupPanel = False
		'
		'txtLMFile
		'
		Me.txtLMFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txtLMFile.Location = New System.Drawing.Point(129, 17)
		Me.txtLMFile.Name = "txtLMFile"
		Me.txtLMFile.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.txtLMFile.Size = New System.Drawing.Size(274, 20)
		Me.txtLMFile.TabIndex = 4
		'
		'frmLMDocs
		'
		Me.AllowDrop = True
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(563, 321)
		Me.Controls.Add(Me.SplitContainerControl2)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmLMDocs"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Text = "Monatliche Lohnangaben / Dokumente  [{0}]"
		CType(Me.SplitContainerControl2, System.ComponentModel.ISupportInitialize).EndInit()
		Me.SplitContainerControl2.ResumeLayout(False)
		CType(Me.grdLMDocument, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvLMDocument, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtLMFile.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub
  Friend WithEvents SplitContainerControl2 As DevExpress.XtraEditors.SplitContainerControl
  Friend WithEvents btnSaveLMDocument As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents lblBezeichnung As DevExpress.XtraEditors.LabelControl
  Friend WithEvents grdLMDocument As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvLMDocument As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents txtLMFile As DevExpress.XtraEditors.ComboBoxEdit

End Class
