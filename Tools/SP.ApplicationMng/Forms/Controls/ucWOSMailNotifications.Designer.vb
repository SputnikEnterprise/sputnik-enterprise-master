Namespace UI

	<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
	Partial Class ucWOSMailNotifications
		Inherits DevExpress.XtraEditors.XtraUserControl

		'UserControl overrides dispose to clean up the component list.
		<System.Diagnostics.DebuggerNonUserCode()>
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
		<System.Diagnostics.DebuggerStepThrough()>
		Private Sub InitializeComponent()
            Me.grdLOG = New DevExpress.XtraGrid.GridControl()
            Me.gvLOG = New DevExpress.XtraGrid.Views.Grid.GridView()
            Me.GridView3 = New DevExpress.XtraGrid.Views.Grid.GridView()
            Me.btnLoadEMails = New DevExpress.XtraEditors.SimpleButton()
            Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
            Me.lblRecCount = New DevExpress.XtraEditors.LabelControl()
            CType(Me.grdLOG, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.gvLOG, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.GridView3, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'grdLOG
            '
            Me.grdLOG.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.grdLOG.Location = New System.Drawing.Point(3, 3)
            Me.grdLOG.MainView = Me.gvLOG
            Me.grdLOG.Name = "grdLOG"
            Me.grdLOG.Size = New System.Drawing.Size(704, 393)
            Me.grdLOG.TabIndex = 357
            Me.grdLOG.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvLOG, Me.GridView3})
            '
            'gvLOG
            '
            Me.gvLOG.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
            Me.gvLOG.GridControl = Me.grdLOG
            Me.gvLOG.Name = "gvLOG"
            Me.gvLOG.OptionsBehavior.Editable = False
            Me.gvLOG.OptionsSelection.EnableAppearanceFocusedCell = False
            Me.gvLOG.OptionsView.ShowAutoFilterRow = True
            Me.gvLOG.OptionsView.ShowGroupPanel = False
            '
            'GridView3
            '
            Me.GridView3.GridControl = Me.grdLOG
            Me.GridView3.Name = "GridView3"
            '
            'btnLoadEMails
            '
            Me.btnLoadEMails.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnLoadEMails.Location = New System.Drawing.Point(600, 402)
            Me.btnLoadEMails.Name = "btnLoadEMails"
            Me.btnLoadEMails.Size = New System.Drawing.Size(107, 31)
            Me.btnLoadEMails.TabIndex = 355
            Me.btnLoadEMails.Text = "Load EMail"
            '
            'BackgroundWorker1
            '
            '
            'lblRecCount
            '
            Me.lblRecCount.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.lblRecCount.Location = New System.Drawing.Point(3, 411)
            Me.lblRecCount.Name = "lblRecCount"
            Me.lblRecCount.Size = New System.Drawing.Size(113, 13)
            Me.lblRecCount.TabIndex = 356
            Me.lblRecCount.Text = "Anzahl Datensätze: {0}"
            '
            'ucWOSMailNotifications
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.grdLOG)
            Me.Controls.Add(Me.btnLoadEMails)
            Me.Controls.Add(Me.lblRecCount)
            Me.Name = "ucWOSMailNotifications"
            Me.Size = New System.Drawing.Size(710, 445)
            CType(Me.grdLOG, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.gvLOG, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.GridView3, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

        Friend WithEvents grdLOG As DevExpress.XtraGrid.GridControl
		Private WithEvents gvLOG As DevExpress.XtraGrid.Views.Grid.GridView
		Friend WithEvents GridView3 As DevExpress.XtraGrid.Views.Grid.GridView
		Friend WithEvents btnLoadEMails As DevExpress.XtraEditors.SimpleButton
		Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
		Friend WithEvents lblRecCount As DevExpress.XtraEditors.LabelControl
	End Class

End Namespace
