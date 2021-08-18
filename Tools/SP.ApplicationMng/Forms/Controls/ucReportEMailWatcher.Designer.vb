Namespace UI


	<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
	Partial Class ucReportEMailWatcher
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
            Me.btnLoadEMails = New DevExpress.XtraEditors.SimpleButton()
            Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
            Me.grdLOG = New DevExpress.XtraGrid.GridControl()
            Me.gvLOG = New DevExpress.XtraGrid.Views.Grid.GridView()
            Me.GridView3 = New DevExpress.XtraGrid.Views.Grid.GridView()
            Me.lblRecCount = New DevExpress.XtraEditors.LabelControl()
            CType(Me.grdLOG, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.gvLOG, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.GridView3, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'btnLoadEMails
            '
            Me.btnLoadEMails.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnLoadEMails.Location = New System.Drawing.Point(618, 322)
            Me.btnLoadEMails.Name = "btnLoadEMails"
            Me.btnLoadEMails.Size = New System.Drawing.Size(107, 31)
            Me.btnLoadEMails.TabIndex = 352
            Me.btnLoadEMails.Text = "Load EMail"
            '
            'BackgroundWorker1
            '
            '
            'grdLOG
            '
            Me.grdLOG.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.grdLOG.Location = New System.Drawing.Point(5, 8)
            Me.grdLOG.MainView = Me.gvLOG
            Me.grdLOG.Name = "grdLOG"
            Me.grdLOG.Size = New System.Drawing.Size(722, 308)
            Me.grdLOG.TabIndex = 354
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
            'lblRecCount
            '
            Me.lblRecCount.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.lblRecCount.Location = New System.Drawing.Point(5, 340)
            Me.lblRecCount.Name = "lblRecCount"
            Me.lblRecCount.Size = New System.Drawing.Size(113, 13)
            Me.lblRecCount.TabIndex = 353
            Me.lblRecCount.Text = "Anzahl Datensätze: {0}"
            '
            'ucReportEMailWatcher
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.grdLOG)
            Me.Controls.Add(Me.lblRecCount)
            Me.Controls.Add(Me.btnLoadEMails)
            Me.Name = "ucReportEMailWatcher"
            Me.Padding = New System.Windows.Forms.Padding(5)
            Me.Size = New System.Drawing.Size(733, 371)
            CType(Me.grdLOG, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.gvLOG, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.GridView3, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents btnLoadEMails As DevExpress.XtraEditors.SimpleButton
		Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
		Friend WithEvents grdLOG As DevExpress.XtraGrid.GridControl
		Private WithEvents gvLOG As DevExpress.XtraGrid.Views.Grid.GridView
		Friend WithEvents GridView3 As DevExpress.XtraGrid.Views.Grid.GridView
		Friend WithEvents lblRecCount As DevExpress.XtraEditors.LabelControl
	End Class

End Namespace
