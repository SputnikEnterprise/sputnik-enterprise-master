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
			Me.chkGotQualificationCertificate = New DevExpress.XtraEditors.CheckEdit()
			Me.btnAddDocument = New DevExpress.XtraEditors.SimpleButton()
			Me.gridDocuments = New DevExpress.XtraGrid.GridControl()
			Me.gvDocuments = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.SplitContainerControl1 = New DevExpress.XtraEditors.SplitContainerControl()
			Me.grpEmployeeCV = New DevExpress.XtraEditors.GroupControl()
			Me.btnAddCV = New DevExpress.XtraEditors.SimpleButton()
			Me.grdCV = New DevExpress.XtraGrid.GridControl()
			Me.gvCV = New DevExpress.XtraGrid.Views.Grid.GridView()
			CType(Me.grpDokumente, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.grpDokumente.SuspendLayout()
			CType(Me.chkGotQualificationCertificate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gridDocuments, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvDocuments, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.SplitContainerControl1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SplitContainerControl1.SuspendLayout()
			CType(Me.grpEmployeeCV, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.grpEmployeeCV.SuspendLayout()
			CType(Me.grdCV, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvCV, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			'
			'grpDokumente
			'
			Me.grpDokumente.AppearanceCaption.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grpDokumente.AppearanceCaption.Options.UseFont = True
			Me.grpDokumente.Controls.Add(Me.chkGotQualificationCertificate)
			Me.grpDokumente.Controls.Add(Me.btnAddDocument)
			Me.grpDokumente.Controls.Add(Me.gridDocuments)
			Me.grpDokumente.Dock = System.Windows.Forms.DockStyle.Fill
			Me.grpDokumente.Location = New System.Drawing.Point(5, 5)
			Me.grpDokumente.Name = "grpDokumente"
			Me.grpDokumente.Size = New System.Drawing.Size(726, 206)
			Me.grpDokumente.TabIndex = 268
			Me.grpDokumente.Text = "Dokumente"
			'
			'chkGotQualificationCertificate
			'
			Me.chkGotQualificationCertificate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.chkGotQualificationCertificate.Location = New System.Drawing.Point(346, 0)
			Me.chkGotQualificationCertificate.Name = "chkGotQualificationCertificate"
			Me.chkGotQualificationCertificate.Properties.AllowFocused = False
			Me.chkGotQualificationCertificate.Properties.Appearance.Options.UseTextOptions = True
			Me.chkGotQualificationCertificate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.chkGotQualificationCertificate.Properties.Caption = "Qualifikationsnachweis erhalten"
			Me.chkGotQualificationCertificate.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.chkGotQualificationCertificate.Size = New System.Drawing.Size(330, 19)
			Me.chkGotQualificationCertificate.TabIndex = 2
			'
			'btnAddDocument
			'
			Me.btnAddDocument.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnAddDocument.Image = CType(resources.GetObject("btnAddDocument.Image"), System.Drawing.Image)
			Me.btnAddDocument.Location = New System.Drawing.Point(694, 3)
			Me.btnAddDocument.Name = "btnAddDocument"
			Me.btnAddDocument.Size = New System.Drawing.Size(27, 15)
			Me.btnAddDocument.TabIndex = 3
			'
			'gridDocuments
			'
			Me.gridDocuments.Dock = System.Windows.Forms.DockStyle.Fill
			Me.gridDocuments.Location = New System.Drawing.Point(2, 20)
			Me.gridDocuments.MainView = Me.gvDocuments
			Me.gridDocuments.Name = "gridDocuments"
			Me.gridDocuments.Size = New System.Drawing.Size(722, 184)
			Me.gridDocuments.TabIndex = 1
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
			Me.gvDocuments.OptionsView.ShowIndicator = False
			'
			'SplitContainerControl1
			'
			Me.SplitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.SplitContainerControl1.Horizontal = False
			Me.SplitContainerControl1.Location = New System.Drawing.Point(0, 0)
			Me.SplitContainerControl1.Name = "SplitContainerControl1"
			Me.SplitContainerControl1.Panel1.Controls.Add(Me.grpDokumente)
			Me.SplitContainerControl1.Panel1.Padding = New System.Windows.Forms.Padding(5)
			Me.SplitContainerControl1.Panel1.Text = "Panel1"
			Me.SplitContainerControl1.Panel2.Controls.Add(Me.grpEmployeeCV)
			Me.SplitContainerControl1.Panel2.MinSize = 100
			Me.SplitContainerControl1.Panel2.Padding = New System.Windows.Forms.Padding(5)
			Me.SplitContainerControl1.Panel2.Text = "Panel2"
			Me.SplitContainerControl1.Size = New System.Drawing.Size(736, 321)
			Me.SplitContainerControl1.SplitterPosition = 452
			Me.SplitContainerControl1.TabIndex = 5
			Me.SplitContainerControl1.Text = "SplitContainerControl1"
			'
			'grpEmployeeCV
			'
			Me.grpEmployeeCV.AppearanceCaption.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grpEmployeeCV.AppearanceCaption.Options.UseFont = True
			Me.grpEmployeeCV.Controls.Add(Me.btnAddCV)
			Me.grpEmployeeCV.Controls.Add(Me.grdCV)
			Me.grpEmployeeCV.Dock = System.Windows.Forms.DockStyle.Fill
			Me.grpEmployeeCV.Location = New System.Drawing.Point(5, 5)
			Me.grpEmployeeCV.Name = "grpEmployeeCV"
			Me.grpEmployeeCV.Size = New System.Drawing.Size(726, 90)
			Me.grpEmployeeCV.TabIndex = 269
			Me.grpEmployeeCV.Text = "Lebenslauf"
			'
			'btnAddCV
			'
			Me.btnAddCV.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnAddCV.Image = CType(resources.GetObject("btnAddCV.Image"), System.Drawing.Image)
			Me.btnAddCV.Location = New System.Drawing.Point(694, 3)
			Me.btnAddCV.Name = "btnAddCV"
			Me.btnAddCV.Size = New System.Drawing.Size(27, 15)
			Me.btnAddCV.TabIndex = 3
			'
			'grdCV
			'
			Me.grdCV.Dock = System.Windows.Forms.DockStyle.Fill
			Me.grdCV.Location = New System.Drawing.Point(2, 20)
			Me.grdCV.MainView = Me.gvCV
			Me.grdCV.Name = "grdCV"
			Me.grdCV.Size = New System.Drawing.Size(722, 68)
			Me.grdCV.TabIndex = 1
			Me.grdCV.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvCV})
			'
			'gvCV
			'
			Me.gvCV.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			Me.gvCV.GridControl = Me.grdCV
			Me.gvCV.Name = "gvCV"
			Me.gvCV.OptionsBehavior.Editable = False
			Me.gvCV.OptionsSelection.EnableAppearanceFocusedCell = False
			Me.gvCV.OptionsView.ShowGroupPanel = False
			Me.gvCV.OptionsView.ShowIndicator = False
			'
			'ucDocumentManagement
			'
			Me.Appearance.BackColor = System.Drawing.Color.Transparent
			Me.Appearance.Options.UseBackColor = True
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.Controls.Add(Me.SplitContainerControl1)
			Me.Name = "ucDocumentManagement"
			Me.Size = New System.Drawing.Size(736, 321)
			CType(Me.grpDokumente, System.ComponentModel.ISupportInitialize).EndInit()
			Me.grpDokumente.ResumeLayout(False)
			CType(Me.chkGotQualificationCertificate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gridDocuments, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvDocuments, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.SplitContainerControl1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.SplitContainerControl1.ResumeLayout(False)
			CType(Me.grpEmployeeCV, System.ComponentModel.ISupportInitialize).EndInit()
			Me.grpEmployeeCV.ResumeLayout(False)
			CType(Me.grdCV, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvCV, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub
		Friend WithEvents grpDokumente As DevExpress.XtraEditors.GroupControl
		Friend WithEvents chkGotQualificationCertificate As DevExpress.XtraEditors.CheckEdit
		Friend WithEvents btnAddDocument As DevExpress.XtraEditors.SimpleButton
		Friend WithEvents gridDocuments As DevExpress.XtraGrid.GridControl
		Friend WithEvents gvDocuments As DevExpress.XtraGrid.Views.Grid.GridView
		Friend WithEvents SplitContainerControl1 As DevExpress.XtraEditors.SplitContainerControl
		Friend WithEvents grpEmployeeCV As DevExpress.XtraEditors.GroupControl
		Friend WithEvents btnAddCV As DevExpress.XtraEditors.SimpleButton
		Friend WithEvents grdCV As DevExpress.XtraGrid.GridControl
		Friend WithEvents gvCV As DevExpress.XtraGrid.Views.Grid.GridView

  End Class

End Namespace
