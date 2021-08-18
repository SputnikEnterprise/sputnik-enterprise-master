Namespace UI

  <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
  Partial Class ucCVLEducation
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
			Me.XtraScrollableControl1 = New DevExpress.XtraEditors.XtraScrollableControl()
			Me.grdNavigation = New DevExpress.XtraGrid.GridControl()
			Me.gvNavigation = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.GridView2 = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.grdEducationPhase = New DevExpress.XtraGrid.GridControl()
			Me.lvEducationPhase = New DevExpress.XtraGrid.Views.Layout.LayoutView()
			Me.LayoutViewCard1 = New DevExpress.XtraGrid.Views.Layout.LayoutViewCard()
			Me.GridView3 = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.LayoutView1 = New DevExpress.XtraGrid.Views.Layout.LayoutView()
			Me.LayoutViewCard2 = New DevExpress.XtraGrid.Views.Layout.LayoutViewCard()
			Me.XtraScrollableControl1.SuspendLayout()
			CType(Me.grdNavigation, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvNavigation, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.GridView2, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.grdEducationPhase, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.lvEducationPhase, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.LayoutViewCard1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.GridView3, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.LayoutView1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.LayoutViewCard2, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			'
			'XtraScrollableControl1
			'
			Me.XtraScrollableControl1.Appearance.BackColor = System.Drawing.Color.Transparent
			Me.XtraScrollableControl1.Appearance.Options.UseBackColor = True
			Me.XtraScrollableControl1.Controls.Add(Me.grdNavigation)
			Me.XtraScrollableControl1.Controls.Add(Me.grdEducationPhase)
			Me.XtraScrollableControl1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.XtraScrollableControl1.Location = New System.Drawing.Point(5, 5)
			Me.XtraScrollableControl1.Name = "XtraScrollableControl1"
			Me.XtraScrollableControl1.Padding = New System.Windows.Forms.Padding(5)
			Me.XtraScrollableControl1.Size = New System.Drawing.Size(889, 281)
			Me.XtraScrollableControl1.TabIndex = 332
			'
			'grdNavigation
			'
			Me.grdNavigation.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
			Me.grdNavigation.Location = New System.Drawing.Point(3, 3)
			Me.grdNavigation.MainView = Me.gvNavigation
			Me.grdNavigation.Name = "grdNavigation"
			Me.grdNavigation.Size = New System.Drawing.Size(311, 273)
			Me.grdNavigation.TabIndex = 336
			Me.grdNavigation.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvNavigation, Me.GridView2})
			'
			'gvNavigation
			'
			Me.gvNavigation.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			Me.gvNavigation.GridControl = Me.grdNavigation
			Me.gvNavigation.Name = "gvNavigation"
			Me.gvNavigation.OptionsBehavior.Editable = False
			Me.gvNavigation.OptionsSelection.EnableAppearanceFocusedCell = False
			Me.gvNavigation.OptionsView.ShowAutoFilterRow = True
			Me.gvNavigation.OptionsView.ShowGroupPanel = False
			'
			'GridView2
			'
			Me.GridView2.GridControl = Me.grdNavigation
			Me.GridView2.Name = "GridView2"
			'
			'grdEducationPhase
			'
			Me.grdEducationPhase.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.grdEducationPhase.Location = New System.Drawing.Point(320, 3)
			Me.grdEducationPhase.MainView = Me.lvEducationPhase
			Me.grdEducationPhase.Name = "grdEducationPhase"
			Me.grdEducationPhase.Size = New System.Drawing.Size(568, 273)
			Me.grdEducationPhase.TabIndex = 332
			Me.grdEducationPhase.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.lvEducationPhase, Me.GridView3, Me.LayoutView1})
			'
			'lvEducationPhase
			'
			Me.lvEducationPhase.ActiveFilterEnabled = False
			Me.lvEducationPhase.GridControl = Me.grdEducationPhase
			Me.lvEducationPhase.Name = "lvEducationPhase"
			Me.lvEducationPhase.OptionsBehavior.Editable = False
			Me.lvEducationPhase.TemplateCard = Me.LayoutViewCard1
			'
			'LayoutViewCard1
			'
			Me.LayoutViewCard1.HeaderButtonsLocation = DevExpress.Utils.GroupElementLocation.AfterText
			Me.LayoutViewCard1.Name = "LayoutViewCard1"
			'
			'GridView3
			'
			Me.GridView3.GridControl = Me.grdEducationPhase
			Me.GridView3.Name = "GridView3"
			'
			'LayoutView1
			'
			Me.LayoutView1.ActiveFilterEnabled = False
			Me.LayoutView1.GridControl = Me.grdEducationPhase
			Me.LayoutView1.Name = "LayoutView1"
			Me.LayoutView1.OptionsBehavior.Editable = False
			Me.LayoutView1.TemplateCard = Me.LayoutViewCard2
			'
			'LayoutViewCard2
			'
			Me.LayoutViewCard2.HeaderButtonsLocation = DevExpress.Utils.GroupElementLocation.AfterText
			Me.LayoutViewCard2.Name = "LayoutViewCard2"
			'
			'ucCVLEducation
			'
			Me.Appearance.BackColor = System.Drawing.Color.Transparent
			Me.Appearance.Options.UseBackColor = True
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.Controls.Add(Me.XtraScrollableControl1)
			Me.Name = "ucCVLEducation"
			Me.Padding = New System.Windows.Forms.Padding(5)
			Me.Size = New System.Drawing.Size(899, 291)
			Me.XtraScrollableControl1.ResumeLayout(False)
			CType(Me.grdNavigation, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvNavigation, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.GridView2, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.grdEducationPhase, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.lvEducationPhase, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.LayoutViewCard1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.GridView3, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.LayoutView1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.LayoutViewCard2, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub
		Friend WithEvents XtraScrollableControl1 As DevExpress.XtraEditors.XtraScrollableControl
    Friend WithEvents grdEducationPhase As DevExpress.XtraGrid.GridControl
    Friend WithEvents lvEducationPhase As DevExpress.XtraGrid.Views.Layout.LayoutView
    Friend WithEvents LayoutViewCard1 As DevExpress.XtraGrid.Views.Layout.LayoutViewCard
    Friend WithEvents GridView3 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutView1 As DevExpress.XtraGrid.Views.Layout.LayoutView
    Friend WithEvents LayoutViewCard2 As DevExpress.XtraGrid.Views.Layout.LayoutViewCard
    Friend WithEvents grdNavigation As DevExpress.XtraGrid.GridControl
    Private WithEvents gvNavigation As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents GridView2 As DevExpress.XtraGrid.Views.Grid.GridView
  End Class

End Namespace