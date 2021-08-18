Namespace UI

  <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
  Partial Class ucCredit
    Inherits ucBaseControlBottomTab

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
			Me.grpguthaben = New DevComponents.DotNetBar.Controls.GroupPanel()
			Me.XtraScrollableControl6 = New DevExpress.XtraEditors.XtraScrollableControl()
			Me.grdGuthaben = New DevExpress.XtraGrid.GridControl()
			Me.gvGuthaben = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.grpRuekstellungen = New DevComponents.DotNetBar.Controls.GroupPanel()
			Me.chkEnableGleitzeit = New DevExpress.XtraEditors.CheckEdit()
			Me.chkFerienBack = New DevExpress.XtraEditors.CheckEdit()
			Me.chkLohn13Back = New DevExpress.XtraEditors.CheckEdit()
			Me.chkFeiertagBack = New DevExpress.XtraEditors.CheckEdit()
			Me.grpEinstellungen = New DevComponents.DotNetBar.Controls.GroupPanel()
			Me.chkWeeklyPayment = New DevExpress.XtraEditors.CheckEdit()
			Me.grpguthaben.SuspendLayout()
			Me.XtraScrollableControl6.SuspendLayout()
			CType(Me.grdGuthaben, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvGuthaben, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.grpRuekstellungen.SuspendLayout()
			CType(Me.chkEnableGleitzeit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.chkFerienBack.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.chkLohn13Back.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.chkFeiertagBack.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.grpEinstellungen.SuspendLayout()
			CType(Me.chkWeeklyPayment.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			'
			'grpguthaben
			'
			Me.grpguthaben.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.grpguthaben.BackColor = System.Drawing.Color.Transparent
			Me.grpguthaben.CanvasColor = System.Drawing.SystemColors.Control
			Me.grpguthaben.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
			Me.grpguthaben.Controls.Add(Me.XtraScrollableControl6)
			Me.grpguthaben.Location = New System.Drawing.Point(12, 8)
			Me.grpguthaben.Name = "grpguthaben"
			Me.grpguthaben.Size = New System.Drawing.Size(448, 221)
			'
			'
			'
			Me.grpguthaben.Style.BackColorGradientAngle = 90
			Me.grpguthaben.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpguthaben.Style.BorderBottomWidth = 1
			Me.grpguthaben.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
			Me.grpguthaben.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpguthaben.Style.BorderLeftWidth = 1
			Me.grpguthaben.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpguthaben.Style.BorderRightWidth = 1
			Me.grpguthaben.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpguthaben.Style.BorderTopWidth = 1
			Me.grpguthaben.Style.CornerDiameter = 4
			Me.grpguthaben.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
			Me.grpguthaben.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grpguthaben.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
			Me.grpguthaben.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
			Me.grpguthaben.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
			'
			'
			'
			Me.grpguthaben.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
			'
			'
			'
			Me.grpguthaben.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
			Me.grpguthaben.TabIndex = 1
			Me.grpguthaben.Text = "Guthaben"
			'
			'XtraScrollableControl6
			'
			Me.XtraScrollableControl6.Appearance.BackColor = System.Drawing.Color.Transparent
			Me.XtraScrollableControl6.Appearance.Options.UseBackColor = True
			Me.XtraScrollableControl6.Controls.Add(Me.grdGuthaben)
			Me.XtraScrollableControl6.Dock = System.Windows.Forms.DockStyle.Fill
			Me.XtraScrollableControl6.Location = New System.Drawing.Point(0, 0)
			Me.XtraScrollableControl6.Name = "XtraScrollableControl6"
			Me.XtraScrollableControl6.Size = New System.Drawing.Size(442, 199)
			Me.XtraScrollableControl6.TabIndex = 0
			'
			'grdGuthaben
			'
			Me.grdGuthaben.Dock = System.Windows.Forms.DockStyle.Fill
			Me.grdGuthaben.Location = New System.Drawing.Point(0, 0)
			Me.grdGuthaben.MainView = Me.gvGuthaben
			Me.grdGuthaben.Name = "grdGuthaben"
			Me.grdGuthaben.Size = New System.Drawing.Size(442, 199)
			Me.grdGuthaben.TabIndex = 1
			Me.grdGuthaben.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvGuthaben})
			'
			'gvGuthaben
			'
			Me.gvGuthaben.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			Me.gvGuthaben.GridControl = Me.grdGuthaben
			Me.gvGuthaben.Name = "gvGuthaben"
			Me.gvGuthaben.OptionsBehavior.Editable = False
			Me.gvGuthaben.OptionsSelection.EnableAppearanceFocusedCell = False
			Me.gvGuthaben.OptionsView.ShowGroupPanel = False
			'
			'grpRuekstellungen
			'
			Me.grpRuekstellungen.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.grpRuekstellungen.BackColor = System.Drawing.Color.Transparent
			Me.grpRuekstellungen.CanvasColor = System.Drawing.SystemColors.Control
			Me.grpRuekstellungen.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
			Me.grpRuekstellungen.Controls.Add(Me.chkEnableGleitzeit)
			Me.grpRuekstellungen.Controls.Add(Me.chkFerienBack)
			Me.grpRuekstellungen.Controls.Add(Me.chkLohn13Back)
			Me.grpRuekstellungen.Controls.Add(Me.chkFeiertagBack)
			Me.grpRuekstellungen.Location = New System.Drawing.Point(475, 77)
			Me.grpRuekstellungen.Name = "grpRuekstellungen"
			Me.grpRuekstellungen.Size = New System.Drawing.Size(448, 152)
			'
			'
			'
			Me.grpRuekstellungen.Style.BackColorGradientAngle = 90
			Me.grpRuekstellungen.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpRuekstellungen.Style.BorderBottomWidth = 1
			Me.grpRuekstellungen.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
			Me.grpRuekstellungen.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpRuekstellungen.Style.BorderLeftWidth = 1
			Me.grpRuekstellungen.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpRuekstellungen.Style.BorderRightWidth = 1
			Me.grpRuekstellungen.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpRuekstellungen.Style.BorderTopWidth = 1
			Me.grpRuekstellungen.Style.CornerDiameter = 4
			Me.grpRuekstellungen.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
			Me.grpRuekstellungen.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grpRuekstellungen.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
			Me.grpRuekstellungen.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
			Me.grpRuekstellungen.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
			'
			'
			'
			Me.grpRuekstellungen.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
			'
			'
			'
			Me.grpRuekstellungen.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
			Me.grpRuekstellungen.TabIndex = 2
			Me.grpRuekstellungen.Text = "Rückstellungen"
			'
			'chkEnableGleitzeit
			'
			Me.chkEnableGleitzeit.Location = New System.Drawing.Point(22, 80)
			Me.chkEnableGleitzeit.Name = "chkEnableGleitzeit"
			Me.chkEnableGleitzeit.Properties.Caption = "Gleitzeiten einschalten"
			Me.chkEnableGleitzeit.Size = New System.Drawing.Size(301, 19)
			Me.chkEnableGleitzeit.TabIndex = 4
			'
			'chkFerienBack
			'
			Me.chkFerienBack.Location = New System.Drawing.Point(22, 5)
			Me.chkFerienBack.Name = "chkFerienBack"
			Me.chkFerienBack.Properties.Caption = "Ferien zurückstellen"
			Me.chkFerienBack.Size = New System.Drawing.Size(301, 19)
			Me.chkFerienBack.TabIndex = 1
			'
			'chkLohn13Back
			'
			Me.chkLohn13Back.Location = New System.Drawing.Point(22, 55)
			Me.chkLohn13Back.Name = "chkLohn13Back"
			Me.chkLohn13Back.Properties.Caption = "13. Lohn zurückstellen"
			Me.chkLohn13Back.Size = New System.Drawing.Size(301, 19)
			Me.chkLohn13Back.TabIndex = 3
			'
			'chkFeiertagBack
			'
			Me.chkFeiertagBack.Location = New System.Drawing.Point(22, 30)
			Me.chkFeiertagBack.Name = "chkFeiertagBack"
			Me.chkFeiertagBack.Properties.Caption = "Feiertage zurückstellen"
			Me.chkFeiertagBack.Size = New System.Drawing.Size(301, 19)
			Me.chkFeiertagBack.TabIndex = 2
			'
			'grpEinstellungen
			'
			Me.grpEinstellungen.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.grpEinstellungen.BackColor = System.Drawing.Color.Transparent
			Me.grpEinstellungen.CanvasColor = System.Drawing.SystemColors.Control
			Me.grpEinstellungen.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
			Me.grpEinstellungen.Controls.Add(Me.chkWeeklyPayment)
			Me.grpEinstellungen.Location = New System.Drawing.Point(475, 8)
			Me.grpEinstellungen.Name = "grpEinstellungen"
			Me.grpEinstellungen.Size = New System.Drawing.Size(448, 63)
			'
			'
			'
			Me.grpEinstellungen.Style.BackColorGradientAngle = 90
			Me.grpEinstellungen.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpEinstellungen.Style.BorderBottomWidth = 1
			Me.grpEinstellungen.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
			Me.grpEinstellungen.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpEinstellungen.Style.BorderLeftWidth = 1
			Me.grpEinstellungen.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpEinstellungen.Style.BorderRightWidth = 1
			Me.grpEinstellungen.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpEinstellungen.Style.BorderTopWidth = 1
			Me.grpEinstellungen.Style.CornerDiameter = 4
			Me.grpEinstellungen.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
			Me.grpEinstellungen.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grpEinstellungen.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
			Me.grpEinstellungen.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
			Me.grpEinstellungen.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
			'
			'
			'
			Me.grpEinstellungen.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
			'
			'
			'
			Me.grpEinstellungen.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
			Me.grpEinstellungen.TabIndex = 3
			Me.grpEinstellungen.Text = "Einstellungen"
			'
			'chkWeeklyPayment
			'
			Me.chkWeeklyPayment.Location = New System.Drawing.Point(22, 5)
			Me.chkWeeklyPayment.Name = "chkWeeklyPayment"
			Me.chkWeeklyPayment.Properties.Caption = "Wöchentliche Vorschüsse"
			Me.chkWeeklyPayment.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.[Default]
			Me.chkWeeklyPayment.Size = New System.Drawing.Size(283, 19)
			Me.chkWeeklyPayment.TabIndex = 267
			'
			'ucCredit
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.Controls.Add(Me.grpEinstellungen)
			Me.Controls.Add(Me.grpguthaben)
			Me.Controls.Add(Me.grpRuekstellungen)
			Me.Name = "ucCredit"
			Me.Padding = New System.Windows.Forms.Padding(5)
			Me.Size = New System.Drawing.Size(936, 237)
			Me.grpguthaben.ResumeLayout(False)
			Me.XtraScrollableControl6.ResumeLayout(False)
			CType(Me.grdGuthaben, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvGuthaben, System.ComponentModel.ISupportInitialize).EndInit()
			Me.grpRuekstellungen.ResumeLayout(False)
			CType(Me.chkEnableGleitzeit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.chkFerienBack.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.chkLohn13Back.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.chkFeiertagBack.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			Me.grpEinstellungen.ResumeLayout(False)
			CType(Me.chkWeeklyPayment.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub
		Friend WithEvents grpguthaben As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents XtraScrollableControl6 As DevExpress.XtraEditors.XtraScrollableControl
    Friend WithEvents grdGuthaben As DevExpress.XtraGrid.GridControl
    Friend WithEvents gvGuthaben As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents grpRuekstellungen As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents chkEnableGleitzeit As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents chkFerienBack As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents chkLohn13Back As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents chkFeiertagBack As DevExpress.XtraEditors.CheckEdit
		Friend WithEvents grpEinstellungen As DevComponents.DotNetBar.Controls.GroupPanel
		Friend WithEvents chkWeeklyPayment As DevExpress.XtraEditors.CheckEdit
	End Class

End Namespace
