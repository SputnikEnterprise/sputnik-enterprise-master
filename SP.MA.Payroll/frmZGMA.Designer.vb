Namespace UI


	<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
	Partial Class frmZGMA
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
			Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmZGMA))
			Me.pnlContent = New DevExpress.XtraEditors.PanelControl()
			Me.lblGridCaption = New System.Windows.Forms.Label()
			Me.pnlHeader = New DevExpress.XtraEditors.PanelControl()
			Me.Label5 = New System.Windows.Forms.Label()
			Me.btnClose = New DevExpress.XtraEditors.SimpleButton()
			Me.lblHeadingSubText = New System.Windows.Forms.Label()
			Me.lblHeadingText = New System.Windows.Forms.Label()
			Me.btnOpenZVForm = New DevExpress.XtraEditors.SimpleButton()
			Me.btnOpenAGForm = New DevExpress.XtraEditors.SimpleButton()
			Me.grdEmployees = New DevExpress.XtraGrid.GridControl()
			Me.gvEmployees = New DevExpress.XtraGrid.Views.Grid.GridView()
			CType(Me.pnlContent, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.pnlContent.SuspendLayout()
			CType(Me.pnlHeader, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.pnlHeader.SuspendLayout()
			CType(Me.grdEmployees, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvEmployees, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			'
			'pnlContent
			'
			Me.pnlContent.Controls.Add(Me.lblGridCaption)
			Me.pnlContent.Controls.Add(Me.pnlHeader)
			Me.pnlContent.Controls.Add(Me.btnOpenZVForm)
			Me.pnlContent.Controls.Add(Me.btnOpenAGForm)
			Me.pnlContent.Controls.Add(Me.grdEmployees)
			Me.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill
			Me.pnlContent.Location = New System.Drawing.Point(0, 0)
			Me.pnlContent.Name = "pnlContent"
			Me.pnlContent.Size = New System.Drawing.Size(558, 532)
			Me.pnlContent.TabIndex = 1
			'
			'lblGridCaption
			'
			Me.lblGridCaption.AutoSize = True
			Me.lblGridCaption.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
			Me.lblGridCaption.Location = New System.Drawing.Point(12, 95)
			Me.lblGridCaption.Name = "lblGridCaption"
			Me.lblGridCaption.Size = New System.Drawing.Size(61, 13)
			Me.lblGridCaption.TabIndex = 295
			Me.lblGridCaption.Text = "Kandidaten"
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
			Me.pnlHeader.Size = New System.Drawing.Size(554, 80)
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
			Me.btnClose.Location = New System.Drawing.Point(430, 21)
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
			Me.lblHeadingText.Size = New System.Drawing.Size(249, 13)
			Me.lblHeadingText.TabIndex = 293
			Me.lblHeadingText.Text = "Liste der Kandidaten im Zwischenverdienst"
			'
			'btnOpenZVForm
			'
			Me.btnOpenZVForm.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnOpenZVForm.Location = New System.Drawing.Point(432, 111)
			Me.btnOpenZVForm.Name = "btnOpenZVForm"
			Me.btnOpenZVForm.Size = New System.Drawing.Size(114, 31)
			Me.btnOpenZVForm.TabIndex = 291
			Me.btnOpenZVForm.Text = "ZV.-Formular öffnen"
			'
			'btnOpenAGForm
			'
			Me.btnOpenAGForm.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnOpenAGForm.Location = New System.Drawing.Point(432, 148)
			Me.btnOpenAGForm.Name = "btnOpenAGForm"
			Me.btnOpenAGForm.Size = New System.Drawing.Size(114, 30)
			Me.btnOpenAGForm.TabIndex = 290
			Me.btnOpenAGForm.Text = "AG.-Formular öffnen"
			'
			'grdEmployees
			'
			Me.grdEmployees.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
							Or System.Windows.Forms.AnchorStyles.Left) _
							Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.grdEmployees.Location = New System.Drawing.Point(12, 111)
			Me.grdEmployees.MainView = Me.gvEmployees
			Me.grdEmployees.Name = "grdEmployees"
			Me.grdEmployees.Size = New System.Drawing.Size(403, 409)
			Me.grdEmployees.TabIndex = 289
			Me.grdEmployees.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvEmployees})
			'
			'gvEmployees
			'
			Me.gvEmployees.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			Me.gvEmployees.GridControl = Me.grdEmployees
			Me.gvEmployees.Name = "gvEmployees"
			Me.gvEmployees.OptionsBehavior.Editable = False
			Me.gvEmployees.OptionsSelection.EnableAppearanceFocusedCell = False
			Me.gvEmployees.OptionsView.ShowGroupPanel = False
			'
			'frmZGMA
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(558, 532)
			Me.Controls.Add(Me.pnlContent)
			Me.Name = "frmZGMA"
			Me.Text = "Liste der Vorschüsse für Checks und Quittungen"
			CType(Me.pnlContent, System.ComponentModel.ISupportInitialize).EndInit()
			Me.pnlContent.ResumeLayout(False)
			Me.pnlContent.PerformLayout()
			CType(Me.pnlHeader, System.ComponentModel.ISupportInitialize).EndInit()
			Me.pnlHeader.ResumeLayout(False)
			Me.pnlHeader.PerformLayout()
			CType(Me.grdEmployees, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvEmployees, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub
		Friend WithEvents pnlContent As DevExpress.XtraEditors.PanelControl
		Friend WithEvents lblGridCaption As System.Windows.Forms.Label
		Friend WithEvents pnlHeader As DevExpress.XtraEditors.PanelControl
		Friend WithEvents Label5 As System.Windows.Forms.Label
		Friend WithEvents btnClose As DevExpress.XtraEditors.SimpleButton
		Friend WithEvents lblHeadingSubText As System.Windows.Forms.Label
		Friend WithEvents lblHeadingText As System.Windows.Forms.Label
		Friend WithEvents btnOpenZVForm As DevExpress.XtraEditors.SimpleButton
		Friend WithEvents btnOpenAGForm As DevExpress.XtraEditors.SimpleButton
		Friend WithEvents grdEmployees As DevExpress.XtraGrid.GridControl
		Friend WithEvents gvEmployees As DevExpress.XtraGrid.Views.Grid.GridView
	End Class

End Namespace
