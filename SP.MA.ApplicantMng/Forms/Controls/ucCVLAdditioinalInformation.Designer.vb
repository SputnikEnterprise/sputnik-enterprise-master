Namespace UI

	<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
	Partial Class ucCVLAdditioinalInformation
    Inherits ucBaseControl

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
			Me.grdAddtional = New DevExpress.XtraGrid.GridControl()
			Me.lvAddtional = New DevExpress.XtraGrid.Views.Layout.LayoutView()
			Me.LayoutViewCard1 = New DevExpress.XtraGrid.Views.Layout.LayoutViewCard()
			Me.GridView3 = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.LayoutView1 = New DevExpress.XtraGrid.Views.Layout.LayoutView()
			Me.LayoutViewCard2 = New DevExpress.XtraGrid.Views.Layout.LayoutViewCard()
			Me.XtraScrollableControl1 = New DevExpress.XtraEditors.XtraScrollableControl()
			Me.grdLanguage = New DevExpress.XtraGrid.GridControl()
			Me.lvLanguage = New DevExpress.XtraGrid.Views.Layout.LayoutView()
			Me.LayoutViewCard3 = New DevExpress.XtraGrid.Views.Layout.LayoutViewCard()
			Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.LayoutView3 = New DevExpress.XtraGrid.Views.Layout.LayoutView()
			Me.LayoutViewCard4 = New DevExpress.XtraGrid.Views.Layout.LayoutViewCard()
			CType(Me.grdAddtional, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.lvAddtional, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.LayoutViewCard1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.GridView3, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.LayoutView1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.LayoutViewCard2, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.XtraScrollableControl1.SuspendLayout()
			CType(Me.grdLanguage, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.lvLanguage, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.LayoutViewCard3, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.LayoutView3, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.LayoutViewCard4, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			'
			'grdAddtional
			'
			Me.grdAddtional.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.grdAddtional.Location = New System.Drawing.Point(320, 3)
			Me.grdAddtional.MainView = Me.lvAddtional
			Me.grdAddtional.Name = "grdAddtional"
			Me.grdAddtional.Size = New System.Drawing.Size(568, 354)
			Me.grdAddtional.TabIndex = 331
			Me.grdAddtional.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.lvAddtional, Me.GridView3, Me.LayoutView1})
			'
			'lvAddtional
			'
			Me.lvAddtional.ActiveFilterEnabled = False
			Me.lvAddtional.GridControl = Me.grdAddtional
			Me.lvAddtional.Name = "lvAddtional"
			Me.lvAddtional.OptionsBehavior.Editable = False
			Me.lvAddtional.TemplateCard = Me.LayoutViewCard1
			'
			'LayoutViewCard1
			'
			Me.LayoutViewCard1.HeaderButtonsLocation = DevExpress.Utils.GroupElementLocation.AfterText
			Me.LayoutViewCard1.Name = "LayoutViewCard1"
			'
			'GridView3
			'
			Me.GridView3.GridControl = Me.grdAddtional
			Me.GridView3.Name = "GridView3"
			'
			'LayoutView1
			'
			Me.LayoutView1.ActiveFilterEnabled = False
			Me.LayoutView1.GridControl = Me.grdAddtional
			Me.LayoutView1.Name = "LayoutView1"
			Me.LayoutView1.OptionsBehavior.Editable = False
			Me.LayoutView1.TemplateCard = Me.LayoutViewCard2
			'
			'LayoutViewCard2
			'
			Me.LayoutViewCard2.HeaderButtonsLocation = DevExpress.Utils.GroupElementLocation.AfterText
			Me.LayoutViewCard2.Name = "LayoutViewCard2"
			'
			'XtraScrollableControl1
			'
			Me.XtraScrollableControl1.Appearance.BackColor = System.Drawing.Color.White
			Me.XtraScrollableControl1.Appearance.Options.UseBackColor = True
			Me.XtraScrollableControl1.Controls.Add(Me.grdLanguage)
			Me.XtraScrollableControl1.Controls.Add(Me.grdAddtional)
			Me.XtraScrollableControl1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.XtraScrollableControl1.Location = New System.Drawing.Point(5, 5)
			Me.XtraScrollableControl1.Name = "XtraScrollableControl1"
			Me.XtraScrollableControl1.Size = New System.Drawing.Size(889, 362)
			Me.XtraScrollableControl1.TabIndex = 333
			'
			'grdLanguage
			'
			Me.grdLanguage.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
			Me.grdLanguage.Location = New System.Drawing.Point(3, 3)
			Me.grdLanguage.MainView = Me.lvLanguage
			Me.grdLanguage.Name = "grdLanguage"
			Me.grdLanguage.Size = New System.Drawing.Size(311, 354)
			Me.grdLanguage.TabIndex = 332
			Me.grdLanguage.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.lvLanguage, Me.GridView1, Me.LayoutView3})
			'
			'lvLanguage
			'
			Me.lvLanguage.ActiveFilterEnabled = False
			Me.lvLanguage.GridControl = Me.grdLanguage
			Me.lvLanguage.Name = "lvLanguage"
			Me.lvLanguage.OptionsBehavior.Editable = False
			Me.lvLanguage.TemplateCard = Me.LayoutViewCard3
			'
			'LayoutViewCard3
			'
			Me.LayoutViewCard3.HeaderButtonsLocation = DevExpress.Utils.GroupElementLocation.AfterText
			Me.LayoutViewCard3.Name = "LayoutViewCard1"
			'
			'GridView1
			'
			Me.GridView1.GridControl = Me.grdLanguage
			Me.GridView1.Name = "GridView1"
			'
			'LayoutView3
			'
			Me.LayoutView3.ActiveFilterEnabled = False
			Me.LayoutView3.GridControl = Me.grdLanguage
			Me.LayoutView3.Name = "LayoutView3"
			Me.LayoutView3.OptionsBehavior.Editable = False
			Me.LayoutView3.TemplateCard = Me.LayoutViewCard4
			'
			'LayoutViewCard4
			'
			Me.LayoutViewCard4.HeaderButtonsLocation = DevExpress.Utils.GroupElementLocation.AfterText
			Me.LayoutViewCard4.Name = "LayoutViewCard2"
			'
			'ucCVLAdditioinalInformation
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.Controls.Add(Me.XtraScrollableControl1)
			Me.Name = "ucCVLAdditioinalInformation"
			Me.Padding = New System.Windows.Forms.Padding(5)
			Me.Size = New System.Drawing.Size(899, 372)
			CType(Me.grdAddtional, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.lvAddtional, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.LayoutViewCard1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.GridView3, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.LayoutView1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.LayoutViewCard2, System.ComponentModel.ISupportInitialize).EndInit()
			Me.XtraScrollableControl1.ResumeLayout(False)
			CType(Me.grdLanguage, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.lvLanguage, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.LayoutViewCard3, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.LayoutView3, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.LayoutViewCard4, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub
		Friend WithEvents grdAddtional As DevExpress.XtraGrid.GridControl
		Friend WithEvents GridView3 As DevExpress.XtraGrid.Views.Grid.GridView
		Friend WithEvents XtraScrollableControl1 As DevExpress.XtraEditors.XtraScrollableControl
		Friend WithEvents LayoutView1 As DevExpress.XtraGrid.Views.Layout.LayoutView
		Friend WithEvents LayoutViewCard2 As DevExpress.XtraGrid.Views.Layout.LayoutViewCard
		Friend WithEvents lvAddtional As DevExpress.XtraGrid.Views.Layout.LayoutView
		Friend WithEvents LayoutViewCard1 As DevExpress.XtraGrid.Views.Layout.LayoutViewCard
		Friend WithEvents grdLanguage As DevExpress.XtraGrid.GridControl
		Friend WithEvents lvLanguage As DevExpress.XtraGrid.Views.Layout.LayoutView
		Friend WithEvents LayoutViewCard3 As DevExpress.XtraGrid.Views.Layout.LayoutViewCard
		Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
		Friend WithEvents LayoutView3 As DevExpress.XtraGrid.Views.Layout.LayoutView
		Friend WithEvents LayoutViewCard4 As DevExpress.XtraGrid.Views.Layout.LayoutViewCard
	End Class

End Namespace
