Namespace UI


  <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
  Partial Class frmZVMA
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
			Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmZVMA))
			Me.pnlContent = New DevExpress.XtraEditors.PanelControl()
			Me.XtraTabControl1 = New DevExpress.XtraTab.XtraTabControl()
			Me.xtabProtocoll = New DevExpress.XtraTab.XtraTabPage()
			Me.rtfContent = New DevExpress.XtraRichEdit.RichEditControl()
			Me.btnPrintProtocol = New DevExpress.XtraEditors.SimpleButton()
			Me.btnSendProtocol = New DevExpress.XtraEditors.SimpleButton()
			Me.xtabZV = New DevExpress.XtraTab.XtraTabPage()
			Me.tgsSelectedEmployee = New DevExpress.XtraEditors.ToggleSwitch()
			Me.grdZVARGBEmployees = New DevExpress.XtraGrid.GridControl()
			Me.gvZVARGBEmployees = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.btnOpenZVForm = New DevExpress.XtraEditors.SimpleButton()
			Me.btnOpenAGForm = New DevExpress.XtraEditors.SimpleButton()
			Me.xtabCheckCash = New DevExpress.XtraTab.XtraTabPage()
			Me.grdCheckCash = New DevExpress.XtraGrid.GridControl()
			Me.gvCheckCash = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.xtabSuspectPayrolls = New DevExpress.XtraTab.XtraTabPage()
			Me.grdSuspectPayroll = New DevExpress.XtraGrid.GridControl()
			Me.gvSuspectPayroll = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.pnlHeader = New DevExpress.XtraEditors.PanelControl()
			Me.Label5 = New System.Windows.Forms.Label()
			Me.btnClose = New DevExpress.XtraEditors.SimpleButton()
			Me.lblHeadingSubText = New System.Windows.Forms.Label()
			Me.lblHeadingText = New System.Windows.Forms.Label()
			CType(Me.pnlContent, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.pnlContent.SuspendLayout()
			CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.XtraTabControl1.SuspendLayout()
			Me.xtabProtocoll.SuspendLayout()
			Me.xtabZV.SuspendLayout()
			CType(Me.tgsSelectedEmployee.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.grdZVARGBEmployees, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvZVARGBEmployees, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.xtabCheckCash.SuspendLayout()
			CType(Me.grdCheckCash, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvCheckCash, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.xtabSuspectPayrolls.SuspendLayout()
			CType(Me.grdSuspectPayroll, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvSuspectPayroll, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.pnlHeader, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.pnlHeader.SuspendLayout()
			Me.SuspendLayout()
			'
			'pnlContent
			'
			Me.pnlContent.Controls.Add(Me.XtraTabControl1)
			Me.pnlContent.Controls.Add(Me.pnlHeader)
			Me.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill
			Me.pnlContent.Location = New System.Drawing.Point(0, 0)
			Me.pnlContent.Name = "pnlContent"
			Me.pnlContent.Size = New System.Drawing.Size(1015, 564)
			Me.pnlContent.TabIndex = 0
			'
			'XtraTabControl1
			'
			Me.XtraTabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.XtraTabControl1.Location = New System.Drawing.Point(16, 111)
			Me.XtraTabControl1.Name = "XtraTabControl1"
			Me.XtraTabControl1.SelectedTabPage = Me.xtabProtocoll
			Me.XtraTabControl1.Size = New System.Drawing.Size(981, 441)
			Me.XtraTabControl1.TabIndex = 296
			Me.XtraTabControl1.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabProtocoll, Me.xtabZV, Me.xtabCheckCash, Me.xtabSuspectPayrolls})
			'
			'xtabProtocoll
			'
			Me.xtabProtocoll.Controls.Add(Me.rtfContent)
			Me.xtabProtocoll.Controls.Add(Me.btnPrintProtocol)
			Me.xtabProtocoll.Controls.Add(Me.btnSendProtocol)
			Me.xtabProtocoll.Name = "xtabProtocoll"
			Me.xtabProtocoll.Padding = New System.Windows.Forms.Padding(5)
			Me.xtabProtocoll.Size = New System.Drawing.Size(979, 416)
			Me.xtabProtocoll.Text = "Protokoll"
			'
			'rtfContent
			'
			Me.rtfContent.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.rtfContent.Appearance.Text.Options.UseTextOptions = True
			Me.rtfContent.Appearance.Text.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
			Me.rtfContent.Location = New System.Drawing.Point(8, 9)
			Me.rtfContent.Name = "rtfContent"
			Me.rtfContent.Options.VerticalRuler.Visibility = DevExpress.XtraRichEdit.RichEditRulerVisibility.Hidden
			Me.rtfContent.Size = New System.Drawing.Size(839, 396)
			Me.rtfContent.TabIndex = 294
			Me.rtfContent.Unit = DevExpress.Office.DocumentUnit.Millimeter
			Me.rtfContent.Views.DraftView.AllowDisplayLineNumbers = True
			Me.rtfContent.Views.SimpleView.AllowDisplayLineNumbers = True
			'
			'btnPrintProtocol
			'
			Me.btnPrintProtocol.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnPrintProtocol.Location = New System.Drawing.Point(853, 9)
			Me.btnPrintProtocol.Name = "btnPrintProtocol"
			Me.btnPrintProtocol.Size = New System.Drawing.Size(114, 31)
			Me.btnPrintProtocol.TabIndex = 293
			Me.btnPrintProtocol.Text = "Drucken"
			'
			'btnSendProtocol
			'
			Me.btnSendProtocol.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnSendProtocol.Location = New System.Drawing.Point(853, 375)
			Me.btnSendProtocol.Name = "btnSendProtocol"
			Me.btnSendProtocol.Size = New System.Drawing.Size(114, 30)
			Me.btnSendProtocol.TabIndex = 292
			Me.btnSendProtocol.Text = "An Support senden"
			'
			'xtabZV
			'
			Me.xtabZV.Controls.Add(Me.tgsSelectedEmployee)
			Me.xtabZV.Controls.Add(Me.grdZVARGBEmployees)
			Me.xtabZV.Controls.Add(Me.btnOpenZVForm)
			Me.xtabZV.Controls.Add(Me.btnOpenAGForm)
			Me.xtabZV.Name = "xtabZV"
			Me.xtabZV.Padding = New System.Windows.Forms.Padding(5)
			Me.xtabZV.Size = New System.Drawing.Size(979, 416)
			Me.xtabZV.Text = "Zwischenverdienst und Arbeitgeberbescheinigung"
			'
			'tgsSelectedEmployee
			'
			Me.tgsSelectedEmployee.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.tgsSelectedEmployee.EditValue = True
			Me.tgsSelectedEmployee.Location = New System.Drawing.Point(594, 14)
			Me.tgsSelectedEmployee.Name = "tgsSelectedEmployee"
			Me.tgsSelectedEmployee.Properties.AllowFocused = False
			Me.tgsSelectedEmployee.Properties.Appearance.Options.UseTextOptions = True
			Me.tgsSelectedEmployee.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.tgsSelectedEmployee.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.tgsSelectedEmployee.Properties.OffText = "Alle Kandidaten anzeigen"
			Me.tgsSelectedEmployee.Properties.OnText = "Erstellte Kandidaten anzeigen"
			Me.tgsSelectedEmployee.Size = New System.Drawing.Size(253, 18)
			Me.tgsSelectedEmployee.TabIndex = 322
			'
			'grdZVARGBEmployees
			'
			Me.grdZVARGBEmployees.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.grdZVARGBEmployees.Location = New System.Drawing.Point(8, 45)
			Me.grdZVARGBEmployees.MainView = Me.gvZVARGBEmployees
			Me.grdZVARGBEmployees.Name = "grdZVARGBEmployees"
			Me.grdZVARGBEmployees.Size = New System.Drawing.Size(839, 360)
			Me.grdZVARGBEmployees.TabIndex = 289
			Me.grdZVARGBEmployees.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvZVARGBEmployees})
			'
			'gvZVARGBEmployees
			'
			Me.gvZVARGBEmployees.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			Me.gvZVARGBEmployees.GridControl = Me.grdZVARGBEmployees
			Me.gvZVARGBEmployees.Name = "gvZVARGBEmployees"
			Me.gvZVARGBEmployees.OptionsBehavior.Editable = False
			Me.gvZVARGBEmployees.OptionsSelection.EnableAppearanceFocusedCell = False
			Me.gvZVARGBEmployees.OptionsView.ShowGroupPanel = False
			'
			'btnOpenZVForm
			'
			Me.btnOpenZVForm.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnOpenZVForm.Location = New System.Drawing.Point(853, 8)
			Me.btnOpenZVForm.Name = "btnOpenZVForm"
			Me.btnOpenZVForm.Size = New System.Drawing.Size(114, 31)
			Me.btnOpenZVForm.TabIndex = 291
			Me.btnOpenZVForm.Text = "ZV.-Formular öffnen"
			'
			'btnOpenAGForm
			'
			Me.btnOpenAGForm.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnOpenAGForm.Location = New System.Drawing.Point(853, 45)
			Me.btnOpenAGForm.Name = "btnOpenAGForm"
			Me.btnOpenAGForm.Size = New System.Drawing.Size(114, 30)
			Me.btnOpenAGForm.TabIndex = 290
			Me.btnOpenAGForm.Text = "AG.-Formular öffnen"
			'
			'xtabCheckCash
			'
			Me.xtabCheckCash.Controls.Add(Me.grdCheckCash)
			Me.xtabCheckCash.Name = "xtabCheckCash"
			Me.xtabCheckCash.Padding = New System.Windows.Forms.Padding(5)
			Me.xtabCheckCash.Size = New System.Drawing.Size(979, 416)
			Me.xtabCheckCash.Text = "Check und Bar-Auszahlungen"
			'
			'grdCheckCash
			'
			Me.grdCheckCash.Dock = System.Windows.Forms.DockStyle.Fill
			Me.grdCheckCash.Location = New System.Drawing.Point(5, 5)
			Me.grdCheckCash.MainView = Me.gvCheckCash
			Me.grdCheckCash.Name = "grdCheckCash"
			Me.grdCheckCash.Size = New System.Drawing.Size(969, 406)
			Me.grdCheckCash.TabIndex = 290
			Me.grdCheckCash.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvCheckCash})
			'
			'gvCheckCash
			'
			Me.gvCheckCash.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			Me.gvCheckCash.GridControl = Me.grdCheckCash
			Me.gvCheckCash.Name = "gvCheckCash"
			Me.gvCheckCash.OptionsBehavior.Editable = False
			Me.gvCheckCash.OptionsSelection.EnableAppearanceFocusedCell = False
			Me.gvCheckCash.OptionsView.ShowGroupPanel = False
			'
			'xtabSuspectPayrolls
			'
			Me.xtabSuspectPayrolls.Controls.Add(Me.grdSuspectPayroll)
			Me.xtabSuspectPayrolls.Name = "xtabSuspectPayrolls"
			Me.xtabSuspectPayrolls.Padding = New System.Windows.Forms.Padding(5)
			Me.xtabSuspectPayrolls.Size = New System.Drawing.Size(979, 416)
			Me.xtabSuspectPayrolls.Text = "Verdächtige Lohnabrechnungen"
			'
			'grdSuspectPayroll
			'
			Me.grdSuspectPayroll.Dock = System.Windows.Forms.DockStyle.Fill
			Me.grdSuspectPayroll.Location = New System.Drawing.Point(5, 5)
			Me.grdSuspectPayroll.MainView = Me.gvSuspectPayroll
			Me.grdSuspectPayroll.Name = "grdSuspectPayroll"
			Me.grdSuspectPayroll.Size = New System.Drawing.Size(969, 406)
			Me.grdSuspectPayroll.TabIndex = 290
			Me.grdSuspectPayroll.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvSuspectPayroll})
			'
			'gvSuspectPayroll
			'
			Me.gvSuspectPayroll.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			Me.gvSuspectPayroll.GridControl = Me.grdSuspectPayroll
			Me.gvSuspectPayroll.Name = "gvSuspectPayroll"
			Me.gvSuspectPayroll.OptionsBehavior.Editable = False
			Me.gvSuspectPayroll.OptionsSelection.EnableAppearanceFocusedCell = False
			Me.gvSuspectPayroll.OptionsView.ShowGroupPanel = False
			'
			'pnlHeader
			'
			Me.pnlHeader.Appearance.BackColor = System.Drawing.Color.White
			Me.pnlHeader.Appearance.BackColor2 = System.Drawing.Color.White
			Me.pnlHeader.Appearance.Options.UseBackColor = True
			Me.pnlHeader.Controls.Add(Me.Label5)
			Me.pnlHeader.Controls.Add(Me.btnClose)
			Me.pnlHeader.Controls.Add(Me.lblHeadingSubText)
			Me.pnlHeader.Controls.Add(Me.lblHeadingText)
			Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
			Me.pnlHeader.Location = New System.Drawing.Point(2, 2)
			Me.pnlHeader.Name = "pnlHeader"
			Me.pnlHeader.Size = New System.Drawing.Size(1011, 80)
			Me.pnlHeader.TabIndex = 292
			'
			'Label5
			'
			Me.Label5.BackColor = System.Drawing.Color.Transparent
			Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
			Me.Label5.ForeColor = System.Drawing.SystemColors.HotTrack
			Me.Label5.Image = CType(resources.GetObject("Label5.Image"), System.Drawing.Image)
			Me.Label5.Location = New System.Drawing.Point(10, 7)
			Me.Label5.Name = "Label5"
			Me.Label5.Size = New System.Drawing.Size(83, 65)
			Me.Label5.TabIndex = 1001
			'
			'btnClose
			'
			Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnClose.Location = New System.Drawing.Point(868, 21)
			Me.btnClose.Name = "btnClose"
			Me.btnClose.Size = New System.Drawing.Size(114, 31)
			Me.btnClose.TabIndex = 293
			Me.btnClose.Text = "Schliessen"
			'
			'lblHeadingSubText
			'
			Me.lblHeadingSubText.AutoSize = True
			Me.lblHeadingSubText.BackColor = System.Drawing.Color.Transparent
			Me.lblHeadingSubText.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
			Me.lblHeadingSubText.Location = New System.Drawing.Point(116, 45)
			Me.lblHeadingSubText.Name = "lblHeadingSubText"
			Me.lblHeadingSubText.Size = New System.Drawing.Size(242, 13)
			Me.lblHeadingSubText.TabIndex = 294
			Me.lblHeadingSubText.Text = "Wählen Sie bitte Ihre geünschten Kriterien aus..."
			'
			'lblHeadingText
			'
			Me.lblHeadingText.AutoSize = True
			Me.lblHeadingText.BackColor = System.Drawing.Color.Transparent
			Me.lblHeadingText.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
			Me.lblHeadingText.Location = New System.Drawing.Point(99, 21)
			Me.lblHeadingText.Name = "lblHeadingText"
			Me.lblHeadingText.Size = New System.Drawing.Size(238, 13)
			Me.lblHeadingText.TabIndex = 293
			Me.lblHeadingText.Text = "Detail über erstellten Lohnabrechnungen"
			'
			'frmZVMA
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(1015, 564)
			Me.Controls.Add(Me.pnlContent)
			Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
			Me.IconOptions.Icon = CType(resources.GetObject("frmZVMA.IconOptions.Icon"), System.Drawing.Icon)
			Me.MinimumSize = New System.Drawing.Size(591, 544)
			Me.Name = "frmZVMA"
			Me.Text = "Detail über erstellten Lohnabrechnungen"
			CType(Me.pnlContent, System.ComponentModel.ISupportInitialize).EndInit()
			Me.pnlContent.ResumeLayout(False)
			CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.XtraTabControl1.ResumeLayout(False)
			Me.xtabProtocoll.ResumeLayout(False)
			Me.xtabZV.ResumeLayout(False)
			CType(Me.tgsSelectedEmployee.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.grdZVARGBEmployees, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvZVARGBEmployees, System.ComponentModel.ISupportInitialize).EndInit()
			Me.xtabCheckCash.ResumeLayout(False)
			CType(Me.grdCheckCash, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvCheckCash, System.ComponentModel.ISupportInitialize).EndInit()
			Me.xtabSuspectPayrolls.ResumeLayout(False)
			CType(Me.grdSuspectPayroll, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvSuspectPayroll, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.pnlHeader, System.ComponentModel.ISupportInitialize).EndInit()
			Me.pnlHeader.ResumeLayout(False)
			Me.pnlHeader.PerformLayout()
			Me.ResumeLayout(False)

		End Sub
		Friend WithEvents pnlContent As DevExpress.XtraEditors.PanelControl
    Friend WithEvents grdZVARGBEmployees As DevExpress.XtraGrid.GridControl
    Friend WithEvents gvZVARGBEmployees As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents pnlHeader As DevExpress.XtraEditors.PanelControl
    Friend WithEvents btnOpenZVForm As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnOpenAGForm As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnClose As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents lblHeadingSubText As System.Windows.Forms.Label
    Friend WithEvents lblHeadingText As System.Windows.Forms.Label
		Friend WithEvents Label5 As System.Windows.Forms.Label
		Friend WithEvents XtraTabControl1 As DevExpress.XtraTab.XtraTabControl
		Friend WithEvents xtabProtocoll As DevExpress.XtraTab.XtraTabPage
		Friend WithEvents xtabZV As DevExpress.XtraTab.XtraTabPage
		Friend WithEvents xtabCheckCash As DevExpress.XtraTab.XtraTabPage
		Friend WithEvents grdCheckCash As DevExpress.XtraGrid.GridControl
		Friend WithEvents gvCheckCash As DevExpress.XtraGrid.Views.Grid.GridView
		Friend WithEvents xtabSuspectPayrolls As DevExpress.XtraTab.XtraTabPage
		Friend WithEvents grdSuspectPayroll As DevExpress.XtraGrid.GridControl
		Friend WithEvents gvSuspectPayroll As DevExpress.XtraGrid.Views.Grid.GridView
		Friend WithEvents btnPrintProtocol As DevExpress.XtraEditors.SimpleButton
		Friend WithEvents btnSendProtocol As DevExpress.XtraEditors.SimpleButton
		Friend WithEvents rtfContent As DevExpress.XtraRichEdit.RichEditControl
		Friend WithEvents tgsSelectedEmployee As DevExpress.XtraEditors.ToggleSwitch
	End Class

End Namespace
