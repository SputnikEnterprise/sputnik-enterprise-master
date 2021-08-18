
Namespace UI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class ucDocumentManagement
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
      Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ucDocumentManagement))
      Me.grpDokumente = New DevExpress.XtraEditors.GroupControl()
      Me.btnAddDocument = New DevExpress.XtraEditors.SimpleButton()
      Me.gridDocuments = New DevExpress.XtraGrid.GridControl()
      Me.gvDocuments = New DevExpress.XtraGrid.Views.Grid.GridView()
      CType(Me.grpDokumente, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.grpDokumente.SuspendLayout()
      CType(Me.gridDocuments, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.gvDocuments, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'grpDokumente
      '
      Me.grpDokumente.AppearanceCaption.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
      Me.grpDokumente.AppearanceCaption.Options.UseFont = True
      Me.grpDokumente.Controls.Add(Me.btnAddDocument)
      Me.grpDokumente.Controls.Add(Me.gridDocuments)
      Me.grpDokumente.Dock = System.Windows.Forms.DockStyle.Fill
      Me.grpDokumente.Location = New System.Drawing.Point(5, 5)
      Me.grpDokumente.Name = "grpDokumente"
      Me.grpDokumente.Size = New System.Drawing.Size(829, 630)
      Me.grpDokumente.TabIndex = 267
      Me.grpDokumente.Text = "Dokumente"
      '
      'btnAddDocument
      '
      Me.btnAddDocument.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
      Me.btnAddDocument.Image = CType(resources.GetObject("btnAddDocument.Image"), System.Drawing.Image)
      Me.btnAddDocument.Location = New System.Drawing.Point(797, 3)
      Me.btnAddDocument.Name = "btnAddDocument"
      Me.btnAddDocument.Size = New System.Drawing.Size(27, 15)
      Me.btnAddDocument.TabIndex = 215
      Me.btnAddDocument.Text = "..."
      '
      'gridDocuments
      '
      Me.gridDocuments.Dock = System.Windows.Forms.DockStyle.Fill
      Me.gridDocuments.Location = New System.Drawing.Point(2, 21)
      Me.gridDocuments.MainView = Me.gvDocuments
      Me.gridDocuments.Name = "gridDocuments"
      Me.gridDocuments.Size = New System.Drawing.Size(825, 607)
      Me.gridDocuments.TabIndex = 2
      Me.gridDocuments.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvDocuments})
      '
      'gvDocuments
      '
      Me.gvDocuments.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
      Me.gvDocuments.GridControl = Me.gridDocuments
      Me.gvDocuments.Name = "gvDocuments"
      Me.gvDocuments.OptionsBehavior.Editable = False
      Me.gvDocuments.OptionsSelection.EnableAppearanceFocusedCell = False
      Me.gvDocuments.OptionsView.ShowGroupPanel = False
      '
      'ucDocumentManagement
      '
      Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
      Me.Controls.Add(Me.grpDokumente)
      Me.Name = "ucDocumentManagement"
      Me.Padding = New System.Windows.Forms.Padding(5)
      Me.Size = New System.Drawing.Size(839, 640)
      CType(Me.grpDokumente, System.ComponentModel.ISupportInitialize).EndInit()
      Me.grpDokumente.ResumeLayout(False)
      CType(Me.gridDocuments, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.gvDocuments, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpDokumente As DevExpress.XtraEditors.GroupControl
        Friend WithEvents gridDocuments As DevExpress.XtraGrid.GridControl
        Friend WithEvents gvDocuments As DevExpress.XtraGrid.Views.Grid.GridView
        Friend WithEvents btnAddDocument As DevExpress.XtraEditors.SimpleButton

    End Class

End Namespace