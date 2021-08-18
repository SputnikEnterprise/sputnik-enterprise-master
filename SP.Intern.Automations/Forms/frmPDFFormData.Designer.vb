<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPDFFormData
	Inherits DevExpress.XtraEditors.XtraForm

	'Form overrides dispose to clean up the component list.
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
		Me.components = New System.ComponentModel.Container()
		Dim EditorButtonImageOptions2 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
		Dim SerializableAppearanceObject5 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject6 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject7 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject8 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Me.xtabMain = New DevExpress.XtraTab.XtraTabControl()
		Me.xtabPDFViewer = New DevExpress.XtraTab.XtraTabPage()
		Me.PdfViewer1 = New DevExpress.XtraPdfViewer.PdfViewer()
		Me.txtPDFFormResult = New DevExpress.XtraEditors.MemoEdit()
		Me.lbldatei = New DevExpress.XtraEditors.LabelControl()
		Me.txtPDFFields = New DevExpress.XtraEditors.ComboBoxEdit()
		CType(Me.xtabMain, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.xtabMain.SuspendLayout()
		Me.xtabPDFViewer.SuspendLayout()
		CType(Me.txtPDFFormResult.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtPDFFields.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'xtabMain
		'
		Me.xtabMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.xtabMain.Location = New System.Drawing.Point(12, 91)
		Me.xtabMain.Name = "xtabMain"
		Me.xtabMain.SelectedTabPage = Me.xtabPDFViewer
		Me.xtabMain.Size = New System.Drawing.Size(963, 521)
		Me.xtabMain.TabIndex = 0
		Me.xtabMain.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabPDFViewer})
		'
		'xtabPDFViewer
		'
		Me.xtabPDFViewer.Controls.Add(Me.txtPDFFormResult)
		Me.xtabPDFViewer.Controls.Add(Me.PdfViewer1)
		Me.xtabPDFViewer.Name = "xtabPDFViewer"
		Me.xtabPDFViewer.Padding = New System.Windows.Forms.Padding(5)
		Me.xtabPDFViewer.Size = New System.Drawing.Size(961, 496)
		Me.xtabPDFViewer.Text = "PDF-Viewer"
		'
		'PdfViewer1
		'
		Me.PdfViewer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.PdfViewer1.Location = New System.Drawing.Point(5, 5)
		Me.PdfViewer1.Name = "PdfViewer1"
		Me.PdfViewer1.Size = New System.Drawing.Size(655, 486)
		Me.PdfViewer1.TabIndex = 0
		'
		'txtPDFFormResult
		'
		Me.txtPDFFormResult.Dock = System.Windows.Forms.DockStyle.Right
		Me.txtPDFFormResult.Location = New System.Drawing.Point(666, 5)
		Me.txtPDFFormResult.Name = "txtPDFFormResult"
		Me.txtPDFFormResult.Size = New System.Drawing.Size(290, 486)
		Me.txtPDFFormResult.TabIndex = 0
		'
		'lbldatei
		'
		Me.lbldatei.Appearance.Options.UseTextOptions = True
		Me.lbldatei.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lbldatei.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lbldatei.Location = New System.Drawing.Point(17, 45)
		Me.lbldatei.Name = "lbldatei"
		Me.lbldatei.Size = New System.Drawing.Size(119, 13)
		Me.lbldatei.TabIndex = 6
		Me.lbldatei.Text = "PDF-Datei"
		'
		'txtPDFFields
		'
		Me.txtPDFFields.AllowDrop = True
		Me.txtPDFFields.Location = New System.Drawing.Point(142, 41)
		Me.txtPDFFields.Name = "txtPDFFields"
		EditorButtonImageOptions2.Image = Global.SP.Internal.Automations.My.Resources.Resources.showallfieldresults_16x161
		Me.txtPDFFields.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, True, True, False, EditorButtonImageOptions2, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject5, SerializableAppearanceObject6, SerializableAppearanceObject7, SerializableAppearanceObject8, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
		Me.txtPDFFields.Size = New System.Drawing.Size(816, 24)
		Me.txtPDFFields.TabIndex = 7
		'
		'frmPDFFormData
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(987, 624)
		Me.Controls.Add(Me.lbldatei)
		Me.Controls.Add(Me.txtPDFFields)
		Me.Controls.Add(Me.xtabMain)
		Me.MinimumSize = New System.Drawing.Size(989, 656)
		Me.Name = "frmPDFFormData"
		Me.Text = "frmPDFFormData"
		CType(Me.xtabMain, System.ComponentModel.ISupportInitialize).EndInit()
		Me.xtabMain.ResumeLayout(False)
		Me.xtabPDFViewer.ResumeLayout(False)
		CType(Me.txtPDFFormResult.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtPDFFields.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub

	Friend WithEvents xtabMain As DevExpress.XtraTab.XtraTabControl
	Friend WithEvents xtabPDFViewer As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents PdfViewer1 As DevExpress.XtraPdfViewer.PdfViewer
	Friend WithEvents txtPDFFormResult As DevExpress.XtraEditors.MemoEdit
	Friend WithEvents lbldatei As DevExpress.XtraEditors.LabelControl
	Friend WithEvents txtPDFFields As DevExpress.XtraEditors.ComboBoxEdit
End Class
