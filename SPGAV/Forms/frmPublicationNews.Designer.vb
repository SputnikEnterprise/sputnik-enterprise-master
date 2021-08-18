
Namespace UI

	<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
	Partial Class frmPublicationNews
		Inherits DevExpress.XtraEditors.XtraForm

		'Form overrides dispose to clean up the component list.
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
			Me.tgsHideReadNews = New DevExpress.XtraEditors.ToggleSwitch()
			Me.grdPublicationNews = New DevExpress.XtraGrid.GridControl()
			Me.gvPublicationNews = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
			Me.lblCountOfNews = New DevExpress.XtraEditors.LabelControl()
			Me.PanelControl2 = New DevExpress.XtraEditors.PanelControl()
			CType(Me.tgsHideReadNews.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.grdPublicationNews, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvPublicationNews, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.PanelControl1.SuspendLayout()
			CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.PanelControl2.SuspendLayout()
			Me.SuspendLayout()
			'
			'tgsHideReadNews
			'
			Me.tgsHideReadNews.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.tgsHideReadNews.EditValue = True
			Me.tgsHideReadNews.Location = New System.Drawing.Point(554, 10)
			Me.tgsHideReadNews.Name = "tgsHideReadNews"
			Me.tgsHideReadNews.Properties.AllowFocused = False
			Me.tgsHideReadNews.Properties.Appearance.Options.UseTextOptions = True
			Me.tgsHideReadNews.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.tgsHideReadNews.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.tgsHideReadNews.Properties.OffText = "Alles einblenden"
			Me.tgsHideReadNews.Properties.OnText = "Gelesene Nachrichten ausblenden"
			Me.tgsHideReadNews.Size = New System.Drawing.Size(280, 18)
			Me.tgsHideReadNews.TabIndex = 1116
			'
			'grdPublicationNews
			'
			Me.grdPublicationNews.Dock = System.Windows.Forms.DockStyle.Fill
			Me.grdPublicationNews.Location = New System.Drawing.Point(7, 7)
			Me.grdPublicationNews.MainView = Me.gvPublicationNews
			Me.grdPublicationNews.Name = "grdPublicationNews"
			Me.grdPublicationNews.Size = New System.Drawing.Size(827, 659)
			Me.grdPublicationNews.TabIndex = 1115
			Me.grdPublicationNews.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvPublicationNews})
			'
			'gvPublicationNews
			'
			Me.gvPublicationNews.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			Me.gvPublicationNews.GridControl = Me.grdPublicationNews
			Me.gvPublicationNews.Name = "gvPublicationNews"
			Me.gvPublicationNews.OptionsBehavior.Editable = False
			Me.gvPublicationNews.OptionsSelection.EnableAppearanceFocusedCell = False
			Me.gvPublicationNews.OptionsView.ShowGroupPanel = False
			'
			'PanelControl1
			'
			Me.PanelControl1.Controls.Add(Me.lblCountOfNews)
			Me.PanelControl1.Controls.Add(Me.tgsHideReadNews)
			Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Top
			Me.PanelControl1.Location = New System.Drawing.Point(5, 5)
			Me.PanelControl1.Name = "PanelControl1"
			Me.PanelControl1.Size = New System.Drawing.Size(841, 65)
			Me.PanelControl1.TabIndex = 1117
			'
			'lblCountOfNews
			'
			Me.lblCountOfNews.AllowHtmlString = True
			Me.lblCountOfNews.Appearance.Options.UseTextOptions = True
			Me.lblCountOfNews.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
			Me.lblCountOfNews.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
			Me.lblCountOfNews.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblCountOfNews.Location = New System.Drawing.Point(11, 10)
			Me.lblCountOfNews.Name = "lblCountOfNews"
			Me.lblCountOfNews.Size = New System.Drawing.Size(167, 49)
			Me.lblCountOfNews.TabIndex = 1117
			Me.lblCountOfNews.Text = "Gelesen: {0}<br>Ungelesen: {1}"
			'
			'PanelControl2
			'
			Me.PanelControl2.Controls.Add(Me.grdPublicationNews)
			Me.PanelControl2.Dock = System.Windows.Forms.DockStyle.Fill
			Me.PanelControl2.Location = New System.Drawing.Point(5, 70)
			Me.PanelControl2.Name = "PanelControl2"
			Me.PanelControl2.Padding = New System.Windows.Forms.Padding(5)
			Me.PanelControl2.Size = New System.Drawing.Size(841, 673)
			Me.PanelControl2.TabIndex = 1118
			'
			'frmPublicationNews
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(851, 748)
			Me.Controls.Add(Me.PanelControl2)
			Me.Controls.Add(Me.PanelControl1)
			Me.IconOptions.LargeImage = Global.SPGAV.My.Resources.Resources.announcement_32x32
			Me.Name = "frmPublicationNews"
			Me.Padding = New System.Windows.Forms.Padding(5)
			Me.Text = "News über GAV Berufe"
			CType(Me.tgsHideReadNews.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.grdPublicationNews, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvPublicationNews, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.PanelControl1.ResumeLayout(False)
			CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).EndInit()
			Me.PanelControl2.ResumeLayout(False)
			Me.ResumeLayout(False)

		End Sub

		Friend WithEvents tgsHideReadNews As DevExpress.XtraEditors.ToggleSwitch
		Friend WithEvents grdPublicationNews As DevExpress.XtraGrid.GridControl
		Friend WithEvents gvPublicationNews As DevExpress.XtraGrid.Views.Grid.GridView
		Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
		Friend WithEvents PanelControl2 As DevExpress.XtraEditors.PanelControl
		Friend WithEvents lblCountOfNews As DevExpress.XtraEditors.LabelControl
	End Class

End Namespace