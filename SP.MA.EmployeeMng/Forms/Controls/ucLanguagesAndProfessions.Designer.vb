
Namespace UI

  <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
  Partial Class ucLanguagesAndProfessions
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
			Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ucLanguagesAndProfessions))
			Me.grpWrittenLanguages = New DevComponents.DotNetBar.Controls.GroupPanel()
			Me.ucWrittenLanguagePopup = New SP.Infrastructure.ucListSelectPopup()
			Me.btnAddWrittenLanguage = New DevExpress.XtraEditors.SimpleButton()
			Me.lstWrittenLanguages = New DevExpress.XtraEditors.ListBoxControl()
			Me.grpVerbalLanguages = New DevComponents.DotNetBar.Controls.GroupPanel()
			Me.ucVerbalLanguagePopup = New SP.Infrastructure.ucListSelectPopup()
			Me.btnAddVerbalLanguage = New DevExpress.XtraEditors.SimpleButton()
			Me.lstVerbalLanguages = New DevExpress.XtraEditors.ListBoxControl()
			Me.grpbranchen = New DevComponents.DotNetBar.Controls.GroupPanel()
			Me.btnAddSector = New DevExpress.XtraEditors.SimpleButton()
			Me.lstSector = New DevExpress.XtraEditors.ListBoxControl()
			Me.grpberufe = New DevComponents.DotNetBar.Controls.GroupPanel()
			Me.lstProfessions = New DevExpress.XtraEditors.ListBoxControl()
			Me.btnAddJobs = New DevExpress.XtraEditors.SimpleButton()
			Me.grpWrittenLanguages.SuspendLayout()
			CType(Me.lstWrittenLanguages, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.grpVerbalLanguages.SuspendLayout()
			CType(Me.lstVerbalLanguages, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.grpbranchen.SuspendLayout()
			CType(Me.lstSector, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.grpberufe.SuspendLayout()
			CType(Me.lstProfessions, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			'
			'grpWrittenLanguages
			'
			Me.grpWrittenLanguages.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
			Me.grpWrittenLanguages.BackColor = System.Drawing.Color.Transparent
			Me.grpWrittenLanguages.CanvasColor = System.Drawing.SystemColors.Control
			Me.grpWrittenLanguages.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
			Me.grpWrittenLanguages.Controls.Add(Me.ucWrittenLanguagePopup)
			Me.grpWrittenLanguages.Controls.Add(Me.btnAddWrittenLanguage)
			Me.grpWrittenLanguages.Controls.Add(Me.lstWrittenLanguages)
			Me.grpWrittenLanguages.Location = New System.Drawing.Point(796, 246)
			Me.grpWrittenLanguages.Name = "grpWrittenLanguages"
			Me.grpWrittenLanguages.Size = New System.Drawing.Size(334, 224)
			'
			'
			'
			Me.grpWrittenLanguages.Style.BackColorGradientAngle = 90
			Me.grpWrittenLanguages.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpWrittenLanguages.Style.BorderBottomWidth = 1
			Me.grpWrittenLanguages.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
			Me.grpWrittenLanguages.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpWrittenLanguages.Style.BorderLeftWidth = 1
			Me.grpWrittenLanguages.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpWrittenLanguages.Style.BorderRightWidth = 1
			Me.grpWrittenLanguages.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpWrittenLanguages.Style.BorderTopWidth = 1
			Me.grpWrittenLanguages.Style.CornerDiameter = 4
			Me.grpWrittenLanguages.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
			Me.grpWrittenLanguages.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grpWrittenLanguages.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
			Me.grpWrittenLanguages.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
			Me.grpWrittenLanguages.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
			'
			'
			'
			Me.grpWrittenLanguages.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
			'
			'
			'
			Me.grpWrittenLanguages.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
			Me.grpWrittenLanguages.TabIndex = 4
			Me.grpWrittenLanguages.Text = "Schriftliche Sprachen"
			'
			'ucWrittenLanguagePopup
			'
			Me.ucWrittenLanguagePopup.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.ucWrittenLanguagePopup.Location = New System.Drawing.Point(300, 28)
			Me.ucWrittenLanguagePopup.Name = "ucWrittenLanguagePopup"
			Me.ucWrittenLanguagePopup.Size = New System.Drawing.Size(11, 10)
			Me.ucWrittenLanguagePopup.TabIndex = 216
			'
			'btnAddWrittenLanguage
			'
			Me.btnAddWrittenLanguage.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnAddWrittenLanguage.ImageOptions.Image = CType(resources.GetObject("btnAddWrittenLanguage.ImageOptions.Image"), System.Drawing.Image)
			Me.btnAddWrittenLanguage.Location = New System.Drawing.Point(297, 0)
			Me.btnAddWrittenLanguage.Name = "btnAddWrittenLanguage"
			Me.btnAddWrittenLanguage.Size = New System.Drawing.Size(25, 22)
			Me.btnAddWrittenLanguage.TabIndex = 2
			'
			'lstWrittenLanguages
			'
			Me.lstWrittenLanguages.Dock = System.Windows.Forms.DockStyle.Left
			Me.lstWrittenLanguages.Location = New System.Drawing.Point(0, 0)
			Me.lstWrittenLanguages.Name = "lstWrittenLanguages"
			Me.lstWrittenLanguages.Size = New System.Drawing.Size(293, 202)
			Me.lstWrittenLanguages.TabIndex = 1
			'
			'grpVerbalLanguages
			'
			Me.grpVerbalLanguages.BackColor = System.Drawing.Color.Transparent
			Me.grpVerbalLanguages.CanvasColor = System.Drawing.SystemColors.Control
			Me.grpVerbalLanguages.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
			Me.grpVerbalLanguages.Controls.Add(Me.ucVerbalLanguagePopup)
			Me.grpVerbalLanguages.Controls.Add(Me.btnAddVerbalLanguage)
			Me.grpVerbalLanguages.Controls.Add(Me.lstVerbalLanguages)
			Me.grpVerbalLanguages.Location = New System.Drawing.Point(796, 5)
			Me.grpVerbalLanguages.Name = "grpVerbalLanguages"
			Me.grpVerbalLanguages.Size = New System.Drawing.Size(337, 222)
			'
			'
			'
			Me.grpVerbalLanguages.Style.BackColorGradientAngle = 90
			Me.grpVerbalLanguages.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpVerbalLanguages.Style.BorderBottomWidth = 1
			Me.grpVerbalLanguages.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
			Me.grpVerbalLanguages.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpVerbalLanguages.Style.BorderLeftWidth = 1
			Me.grpVerbalLanguages.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpVerbalLanguages.Style.BorderRightWidth = 1
			Me.grpVerbalLanguages.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpVerbalLanguages.Style.BorderTopWidth = 1
			Me.grpVerbalLanguages.Style.CornerDiameter = 4
			Me.grpVerbalLanguages.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
			Me.grpVerbalLanguages.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grpVerbalLanguages.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
			Me.grpVerbalLanguages.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
			Me.grpVerbalLanguages.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
			'
			'
			'
			Me.grpVerbalLanguages.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
			'
			'
			'
			Me.grpVerbalLanguages.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
			Me.grpVerbalLanguages.TabIndex = 3
			Me.grpVerbalLanguages.Text = "Mündliche Sprachen"
			'
			'ucVerbalLanguagePopup
			'
			Me.ucVerbalLanguagePopup.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.ucVerbalLanguagePopup.Location = New System.Drawing.Point(303, 29)
			Me.ucVerbalLanguagePopup.Name = "ucVerbalLanguagePopup"
			Me.ucVerbalLanguagePopup.Size = New System.Drawing.Size(11, 10)
			Me.ucVerbalLanguagePopup.TabIndex = 214
			'
			'btnAddVerbalLanguage
			'
			Me.btnAddVerbalLanguage.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnAddVerbalLanguage.ImageOptions.Image = CType(resources.GetObject("btnAddVerbalLanguage.ImageOptions.Image"), System.Drawing.Image)
			Me.btnAddVerbalLanguage.Location = New System.Drawing.Point(298, 1)
			Me.btnAddVerbalLanguage.Name = "btnAddVerbalLanguage"
			Me.btnAddVerbalLanguage.Size = New System.Drawing.Size(25, 22)
			Me.btnAddVerbalLanguage.TabIndex = 2
			'
			'lstVerbalLanguages
			'
			Me.lstVerbalLanguages.Dock = System.Windows.Forms.DockStyle.Left
			Me.lstVerbalLanguages.Location = New System.Drawing.Point(0, 0)
			Me.lstVerbalLanguages.Name = "lstVerbalLanguages"
			Me.lstVerbalLanguages.Size = New System.Drawing.Size(293, 200)
			Me.lstVerbalLanguages.TabIndex = 203
			'
			'grpbranchen
			'
			Me.grpbranchen.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
			Me.grpbranchen.BackColor = System.Drawing.Color.Transparent
			Me.grpbranchen.CanvasColor = System.Drawing.SystemColors.Control
			Me.grpbranchen.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
			Me.grpbranchen.Controls.Add(Me.btnAddSector)
			Me.grpbranchen.Controls.Add(Me.lstSector)
			Me.grpbranchen.Location = New System.Drawing.Point(404, 5)
			Me.grpbranchen.Name = "grpbranchen"
			Me.grpbranchen.Size = New System.Drawing.Size(373, 465)
			'
			'
			'
			Me.grpbranchen.Style.BackColorGradientAngle = 90
			Me.grpbranchen.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpbranchen.Style.BorderBottomWidth = 1
			Me.grpbranchen.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
			Me.grpbranchen.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpbranchen.Style.BorderLeftWidth = 1
			Me.grpbranchen.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpbranchen.Style.BorderRightWidth = 1
			Me.grpbranchen.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpbranchen.Style.BorderTopWidth = 1
			Me.grpbranchen.Style.CornerDiameter = 4
			Me.grpbranchen.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
			Me.grpbranchen.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grpbranchen.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
			Me.grpbranchen.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
			Me.grpbranchen.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
			'
			'
			'
			Me.grpbranchen.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
			'
			'
			'
			Me.grpbranchen.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
			Me.grpbranchen.TabIndex = 2
			Me.grpbranchen.Text = "Branchen / Gewerbe"
			'
			'btnAddSector
			'
			Me.btnAddSector.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnAddSector.ImageOptions.Image = CType(resources.GetObject("btnAddSector.ImageOptions.Image"), System.Drawing.Image)
			Me.btnAddSector.Location = New System.Drawing.Point(338, 0)
			Me.btnAddSector.Name = "btnAddSector"
			Me.btnAddSector.Size = New System.Drawing.Size(25, 22)
			Me.btnAddSector.TabIndex = 2
			'
			'lstSector
			'
			Me.lstSector.Dock = System.Windows.Forms.DockStyle.Left
			Me.lstSector.Location = New System.Drawing.Point(0, 0)
			Me.lstSector.Name = "lstSector"
			Me.lstSector.Size = New System.Drawing.Size(332, 443)
			Me.lstSector.TabIndex = 1
			'
			'grpberufe
			'
			Me.grpberufe.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
			Me.grpberufe.BackColor = System.Drawing.Color.Transparent
			Me.grpberufe.CanvasColor = System.Drawing.SystemColors.Control
			Me.grpberufe.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
			Me.grpberufe.Controls.Add(Me.lstProfessions)
			Me.grpberufe.Controls.Add(Me.btnAddJobs)
			Me.grpberufe.Location = New System.Drawing.Point(12, 5)
			Me.grpberufe.Name = "grpberufe"
			Me.grpberufe.Size = New System.Drawing.Size(373, 465)
			'
			'
			'
			Me.grpberufe.Style.BackColorGradientAngle = 90
			Me.grpberufe.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpberufe.Style.BorderBottomWidth = 1
			Me.grpberufe.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
			Me.grpberufe.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpberufe.Style.BorderLeftWidth = 1
			Me.grpberufe.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpberufe.Style.BorderRightWidth = 1
			Me.grpberufe.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpberufe.Style.BorderTopWidth = 1
			Me.grpberufe.Style.CornerDiameter = 4
			Me.grpberufe.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
			Me.grpberufe.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grpberufe.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
			Me.grpberufe.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
			Me.grpberufe.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
			'
			'
			'
			Me.grpberufe.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
			'
			'
			'
			Me.grpberufe.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
			Me.grpberufe.TabIndex = 1
			Me.grpberufe.Text = "Sonstige Qualifikation"
			'
			'lstProfessions
			'
			Me.lstProfessions.Dock = System.Windows.Forms.DockStyle.Left
			Me.lstProfessions.Location = New System.Drawing.Point(0, 0)
			Me.lstProfessions.Name = "lstProfessions"
			Me.lstProfessions.Size = New System.Drawing.Size(332, 443)
			Me.lstProfessions.TabIndex = 1
			'
			'btnAddJobs
			'
			Me.btnAddJobs.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnAddJobs.ImageOptions.Image = CType(resources.GetObject("btnAddJobs.ImageOptions.Image"), System.Drawing.Image)
			Me.btnAddJobs.Location = New System.Drawing.Point(338, 0)
			Me.btnAddJobs.Name = "btnAddJobs"
			Me.btnAddJobs.Size = New System.Drawing.Size(25, 22)
			Me.btnAddJobs.TabIndex = 2
			'
			'ucLanguagesAndProfessions
			'
			Me.Appearance.BackColor = System.Drawing.Color.Transparent
			Me.Appearance.Options.UseBackColor = True
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.Controls.Add(Me.grpWrittenLanguages)
			Me.Controls.Add(Me.grpVerbalLanguages)
			Me.Controls.Add(Me.grpberufe)
			Me.Controls.Add(Me.grpbranchen)
			Me.Name = "ucLanguagesAndProfessions"
			Me.Size = New System.Drawing.Size(1147, 483)
			Me.grpWrittenLanguages.ResumeLayout(False)
			CType(Me.lstWrittenLanguages, System.ComponentModel.ISupportInitialize).EndInit()
			Me.grpVerbalLanguages.ResumeLayout(False)
			CType(Me.lstVerbalLanguages, System.ComponentModel.ISupportInitialize).EndInit()
			Me.grpbranchen.ResumeLayout(False)
			CType(Me.lstSector, System.ComponentModel.ISupportInitialize).EndInit()
			Me.grpberufe.ResumeLayout(False)
			CType(Me.lstProfessions, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub
		Friend WithEvents grpWrittenLanguages As DevComponents.DotNetBar.Controls.GroupPanel
		Friend WithEvents ucWrittenLanguagePopup As SP.Infrastructure.ucListSelectPopup
    Friend WithEvents btnAddWrittenLanguage As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents lstWrittenLanguages As DevExpress.XtraEditors.ListBoxControl
    Friend WithEvents grpVerbalLanguages As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents ucVerbalLanguagePopup As SP.Infrastructure.ucListSelectPopup
    Friend WithEvents btnAddVerbalLanguage As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents lstVerbalLanguages As DevExpress.XtraEditors.ListBoxControl
    Friend WithEvents grpbranchen As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents btnAddSector As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents lstSector As DevExpress.XtraEditors.ListBoxControl
    Friend WithEvents grpberufe As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents lstProfessions As DevExpress.XtraEditors.ListBoxControl
    Friend WithEvents btnAddJobs As DevExpress.XtraEditors.SimpleButton

  End Class

End Namespace
