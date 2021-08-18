<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMessageDetails
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMessageDetails))
		Me.grpFilter = New DevComponents.DotNetBar.Controls.GroupPanel()
		Me.lblAmValue = New DevExpress.XtraEditors.LabelControl()
		Me.lblAm = New DevExpress.XtraEditors.LabelControl()
		Me.lblDurchValue = New DevExpress.XtraEditors.LabelControl()
		Me.lblBenutzer = New DevExpress.XtraEditors.LabelControl()
		Me.lstAttachments = New DevExpress.XtraEditors.ListBoxControl()
		Me.eMail_Subject = New DevExpress.XtraEditors.LabelControl()
		Me.eMail_To = New DevExpress.XtraEditors.LabelControl()
		Me.eMail_From = New DevExpress.XtraEditors.LabelControl()
		Me.lblDateianhang = New DevExpress.XtraEditors.LabelControl()
		Me.lblVon = New DevExpress.XtraEditors.LabelControl()
		Me.lblAn = New DevExpress.XtraEditors.LabelControl()
		Me.lblBetreff = New DevExpress.XtraEditors.LabelControl()
		Me.lblNachricht = New DevExpress.XtraEditors.LabelControl()
		Me.wbHtml = New System.Windows.Forms.WebBrowser()
		Me.grpFilter.SuspendLayout()
		CType(Me.lstAttachments, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'grpFilter
		'
		Me.grpFilter.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.grpFilter.BackColor = System.Drawing.Color.Transparent
		Me.grpFilter.CanvasColor = System.Drawing.SystemColors.Control
		Me.grpFilter.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
		Me.grpFilter.Controls.Add(Me.lblAmValue)
		Me.grpFilter.Controls.Add(Me.lblAm)
		Me.grpFilter.Controls.Add(Me.lblDurchValue)
		Me.grpFilter.Controls.Add(Me.lblBenutzer)
		Me.grpFilter.Controls.Add(Me.lstAttachments)
		Me.grpFilter.Controls.Add(Me.eMail_Subject)
		Me.grpFilter.Controls.Add(Me.eMail_To)
		Me.grpFilter.Controls.Add(Me.eMail_From)
		Me.grpFilter.Controls.Add(Me.lblDateianhang)
		Me.grpFilter.Controls.Add(Me.lblVon)
		Me.grpFilter.Controls.Add(Me.lblAn)
		Me.grpFilter.Controls.Add(Me.lblBetreff)
		Me.grpFilter.Location = New System.Drawing.Point(12, 12)
		Me.grpFilter.Name = "grpFilter"
		Me.grpFilter.Size = New System.Drawing.Size(761, 156)
		'
		'
		'
		Me.grpFilter.Style.BackColorGradientAngle = 90
		Me.grpFilter.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.grpFilter.Style.BorderBottomWidth = 1
		Me.grpFilter.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
		Me.grpFilter.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.grpFilter.Style.BorderLeftWidth = 1
		Me.grpFilter.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.grpFilter.Style.BorderRightWidth = 1
		Me.grpFilter.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.grpFilter.Style.BorderTopWidth = 1
		Me.grpFilter.Style.CornerDiameter = 4
		Me.grpFilter.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
		Me.grpFilter.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.grpFilter.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
		Me.grpFilter.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
		Me.grpFilter.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
		'
		'
		'
		Me.grpFilter.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
		'
		'
		'
		Me.grpFilter.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
		Me.grpFilter.TabIndex = 218
		Me.grpFilter.Text = "Detail"
		'
		'lblAmValue
		'
		Me.lblAmValue.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblAmValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblAmValue.Location = New System.Drawing.Point(613, 35)
		Me.lblAmValue.Name = "lblAmValue"
		Me.lblAmValue.Size = New System.Drawing.Size(125, 13)
		Me.lblAmValue.TabIndex = 317
		Me.lblAmValue.Text = "Von"
		'
		'lblAm
		'
		Me.lblAm.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.lblAm.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblAm.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblAm.Location = New System.Drawing.Point(486, 35)
		Me.lblAm.Name = "lblAm"
		Me.lblAm.Size = New System.Drawing.Size(121, 13)
		Me.lblAm.TabIndex = 316
		Me.lblAm.Text = "Am"
		'
		'lblDurchValue
		'
		Me.lblDurchValue.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblDurchValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblDurchValue.Location = New System.Drawing.Point(613, 54)
		Me.lblDurchValue.Name = "lblDurchValue"
		Me.lblDurchValue.Size = New System.Drawing.Size(125, 13)
		Me.lblDurchValue.TabIndex = 315
		Me.lblDurchValue.Text = "Von"
		'
		'lblBenutzer
		'
		Me.lblBenutzer.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.lblBenutzer.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblBenutzer.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblBenutzer.Location = New System.Drawing.Point(486, 54)
		Me.lblBenutzer.Name = "lblBenutzer"
		Me.lblBenutzer.Size = New System.Drawing.Size(121, 13)
		Me.lblBenutzer.TabIndex = 314
		Me.lblBenutzer.Text = "Benutzer"
		'
		'lstAttachments
		'
		Me.lstAttachments.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.lstAttachments.Location = New System.Drawing.Point(97, 82)
		Me.lstAttachments.MultiColumn = True
		Me.lstAttachments.Name = "lstAttachments"
		Me.lstAttachments.Size = New System.Drawing.Size(641, 37)
		Me.lstAttachments.TabIndex = 313
		'
		'eMail_Subject
		'
		Me.eMail_Subject.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.eMail_Subject.Location = New System.Drawing.Point(97, 54)
		Me.eMail_Subject.Name = "eMail_Subject"
		Me.eMail_Subject.Size = New System.Drawing.Size(383, 13)
		Me.eMail_Subject.TabIndex = 312
		Me.eMail_Subject.Text = "Von"
		'
		'eMail_To
		'
		Me.eMail_To.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.eMail_To.Location = New System.Drawing.Point(97, 35)
		Me.eMail_To.Name = "eMail_To"
		Me.eMail_To.Size = New System.Drawing.Size(383, 13)
		Me.eMail_To.TabIndex = 311
		Me.eMail_To.Text = "Von"
		'
		'eMail_From
		'
		Me.eMail_From.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.eMail_From.Location = New System.Drawing.Point(97, 13)
		Me.eMail_From.Name = "eMail_From"
		Me.eMail_From.Size = New System.Drawing.Size(383, 13)
		Me.eMail_From.TabIndex = 310
		Me.eMail_From.Text = "Von"
		'
		'lblDateianhang
		'
		Me.lblDateianhang.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.lblDateianhang.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.lblDateianhang.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblDateianhang.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblDateianhang.Location = New System.Drawing.Point(3, 84)
		Me.lblDateianhang.Name = "lblDateianhang"
		Me.lblDateianhang.Size = New System.Drawing.Size(88, 13)
		Me.lblDateianhang.TabIndex = 308
		Me.lblDateianhang.Text = "Anhang"
		'
		'lblVon
		'
		Me.lblVon.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.lblVon.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblVon.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblVon.Location = New System.Drawing.Point(3, 13)
		Me.lblVon.Name = "lblVon"
		Me.lblVon.Size = New System.Drawing.Size(88, 13)
		Me.lblVon.TabIndex = 279
		Me.lblVon.Text = "Von"
		'
		'lblAn
		'
		Me.lblAn.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.lblAn.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblAn.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblAn.Location = New System.Drawing.Point(3, 35)
		Me.lblAn.Name = "lblAn"
		Me.lblAn.Size = New System.Drawing.Size(88, 12)
		Me.lblAn.TabIndex = 222
		Me.lblAn.Text = "An"
		'
		'lblBetreff
		'
		Me.lblBetreff.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
		Me.lblBetreff.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblBetreff.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblBetreff.Location = New System.Drawing.Point(3, 54)
		Me.lblBetreff.Name = "lblBetreff"
		Me.lblBetreff.Size = New System.Drawing.Size(88, 12)
		Me.lblBetreff.TabIndex = 223
		Me.lblBetreff.Text = "Betreff"
		'
		'lblNachricht
		'
		Me.lblNachricht.Location = New System.Drawing.Point(12, 176)
		Me.lblNachricht.Name = "lblNachricht"
		Me.lblNachricht.Size = New System.Drawing.Size(45, 13)
		Me.lblNachricht.TabIndex = 224
		Me.lblNachricht.Text = "Nachricht"
		'
		'wbHtml
		'
		Me.wbHtml.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.wbHtml.Location = New System.Drawing.Point(12, 198)
		Me.wbHtml.MinimumSize = New System.Drawing.Size(20, 20)
		Me.wbHtml.Name = "wbHtml"
		Me.wbHtml.ScriptErrorsSuppressed = True
		Me.wbHtml.Size = New System.Drawing.Size(761, 321)
		Me.wbHtml.TabIndex = 227
		Me.wbHtml.TabStop = False
		'
		'frmMessageDetails
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(787, 533)
		Me.Controls.Add(Me.wbHtml)
		Me.Controls.Add(Me.grpFilter)
		Me.Controls.Add(Me.lblNachricht)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MinimumSize = New System.Drawing.Size(803, 571)
		Me.Name = "frmMessageDetails"
		Me.Text = "Ihre Nachricht"
		Me.grpFilter.ResumeLayout(False)
		CType(Me.lstAttachments, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents grpFilter As DevComponents.DotNetBar.Controls.GroupPanel
	Friend WithEvents eMail_Subject As DevExpress.XtraEditors.LabelControl
	Friend WithEvents eMail_To As DevExpress.XtraEditors.LabelControl
	Friend WithEvents eMail_From As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblDateianhang As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblVon As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblAn As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblBetreff As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblNachricht As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lstAttachments As DevExpress.XtraEditors.ListBoxControl
	Friend WithEvents wbHtml As System.Windows.Forms.WebBrowser
	Friend WithEvents lblAmValue As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblAm As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblDurchValue As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblBenutzer As DevExpress.XtraEditors.LabelControl
End Class
