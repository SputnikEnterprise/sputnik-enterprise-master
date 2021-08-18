
Namespace UI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class ucProfessionsAndSectors
        Inherits ucBaseControl

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
      Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ucProfessionsAndSectors))
      Me.grpBranchen = New DevComponents.DotNetBar.Controls.GroupPanel()
      Me.btnAddSector = New DevExpress.XtraEditors.SimpleButton()
      Me.lstSector = New DevExpress.XtraEditors.ListBoxControl()
      Me.grpBerufe = New DevComponents.DotNetBar.Controls.GroupPanel()
      Me.lstProfessions = New DevExpress.XtraEditors.ListBoxControl()
      Me.btnAddJobs = New DevExpress.XtraEditors.SimpleButton()
      Me.grpBranchen.SuspendLayout()
      CType(Me.lstSector, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.grpBerufe.SuspendLayout()
      CType(Me.lstProfessions, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'grpBranchen
      '
      Me.grpBranchen.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
              Or System.Windows.Forms.AnchorStyles.Left) _
              Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
      Me.grpBranchen.BackColor = System.Drawing.Color.Transparent
      Me.grpBranchen.CanvasColor = System.Drawing.SystemColors.Control
      Me.grpBranchen.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
      Me.grpBranchen.Controls.Add(Me.btnAddSector)
      Me.grpBranchen.Controls.Add(Me.lstSector)
      Me.grpBranchen.Location = New System.Drawing.Point(418, 12)
      Me.grpBranchen.Name = "grpBranchen"
      Me.grpBranchen.Size = New System.Drawing.Size(324, 504)
      '
      '
      '
      Me.grpBranchen.Style.BackColorGradientAngle = 90
      Me.grpBranchen.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
      Me.grpBranchen.Style.BorderBottomWidth = 1
      Me.grpBranchen.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
      Me.grpBranchen.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
      Me.grpBranchen.Style.BorderLeftWidth = 1
      Me.grpBranchen.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
      Me.grpBranchen.Style.BorderRightWidth = 1
      Me.grpBranchen.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
      Me.grpBranchen.Style.BorderTopWidth = 1
      Me.grpBranchen.Style.CornerDiameter = 4
      Me.grpBranchen.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
      Me.grpBranchen.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
      Me.grpBranchen.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
      Me.grpBranchen.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
      Me.grpBranchen.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
      '
      '
      '
      Me.grpBranchen.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
      '
      '
      '
      Me.grpBranchen.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
      Me.grpBranchen.TabIndex = 220
      Me.grpBranchen.Text = "Branchen / Gewerbe"
      '
      'btnAddSector
      '
      Me.btnAddSector.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
      Me.btnAddSector.Image = CType(resources.GetObject("btnAddSector.Image"), System.Drawing.Image)
      Me.btnAddSector.Location = New System.Drawing.Point(285, 3)
      Me.btnAddSector.Name = "btnAddSector"
      Me.btnAddSector.Size = New System.Drawing.Size(25, 22)
      Me.btnAddSector.TabIndex = 214
      Me.btnAddSector.Text = "..."
      '
      'lstSector
      '
      Me.lstSector.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
              Or System.Windows.Forms.AnchorStyles.Left) _
              Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
      Me.lstSector.Location = New System.Drawing.Point(9, 3)
      Me.lstSector.Name = "lstSector"
      Me.lstSector.Size = New System.Drawing.Size(270, 466)
      Me.lstSector.TabIndex = 202
      '
      'grpBerufe
      '
      Me.grpBerufe.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
              Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
      Me.grpBerufe.BackColor = System.Drawing.Color.Transparent
      Me.grpBerufe.CanvasColor = System.Drawing.SystemColors.Control
      Me.grpBerufe.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
      Me.grpBerufe.Controls.Add(Me.lstProfessions)
      Me.grpBerufe.Controls.Add(Me.btnAddJobs)
      Me.grpBerufe.Location = New System.Drawing.Point(13, 12)
      Me.grpBerufe.Name = "grpBerufe"
      Me.grpBerufe.Size = New System.Drawing.Size(364, 504)
      '
      '
      '
      Me.grpBerufe.Style.BackColorGradientAngle = 90
      Me.grpBerufe.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
      Me.grpBerufe.Style.BorderBottomWidth = 1
      Me.grpBerufe.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
      Me.grpBerufe.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
      Me.grpBerufe.Style.BorderLeftWidth = 1
      Me.grpBerufe.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
      Me.grpBerufe.Style.BorderRightWidth = 1
      Me.grpBerufe.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
      Me.grpBerufe.Style.BorderTopWidth = 1
      Me.grpBerufe.Style.CornerDiameter = 4
      Me.grpBerufe.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
      Me.grpBerufe.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
      Me.grpBerufe.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
      Me.grpBerufe.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
      Me.grpBerufe.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
      '
      '
      '
      Me.grpBerufe.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
      '
      '
      '
      Me.grpBerufe.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
      Me.grpBerufe.TabIndex = 219
      Me.grpBerufe.Text = "Berufe"
      '
      'lstProfessions
      '
      Me.lstProfessions.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
              Or System.Windows.Forms.AnchorStyles.Left) _
              Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
      Me.lstProfessions.Location = New System.Drawing.Point(9, 3)
      Me.lstProfessions.Name = "lstProfessions"
      Me.lstProfessions.Size = New System.Drawing.Size(311, 466)
      Me.lstProfessions.TabIndex = 202
      '
      'btnAddJobs
      '
      Me.btnAddJobs.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
      Me.btnAddJobs.Image = CType(resources.GetObject("btnAddJobs.Image"), System.Drawing.Image)
      Me.btnAddJobs.Location = New System.Drawing.Point(326, 0)
      Me.btnAddJobs.Name = "btnAddJobs"
      Me.btnAddJobs.Size = New System.Drawing.Size(25, 22)
      Me.btnAddJobs.TabIndex = 212
      Me.btnAddJobs.Text = "..."
      '
      'ucProfessionsAndSectors
      '
      Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
      Me.Controls.Add(Me.grpBranchen)
      Me.Controls.Add(Me.grpBerufe)
      Me.Name = "ucProfessionsAndSectors"
      Me.Size = New System.Drawing.Size(758, 530)
      Me.grpBranchen.ResumeLayout(False)
      CType(Me.lstSector, System.ComponentModel.ISupportInitialize).EndInit()
      Me.grpBerufe.ResumeLayout(False)
      CType(Me.lstProfessions, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpBranchen As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents btnAddSector As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents lstSector As DevExpress.XtraEditors.ListBoxControl
    Friend WithEvents grpBerufe As DevComponents.DotNetBar.Controls.GroupPanel
        Friend WithEvents lstProfessions As DevExpress.XtraEditors.ListBoxControl
        Friend WithEvents btnAddJobs As DevExpress.XtraEditors.SimpleButton

    End Class

End Namespace