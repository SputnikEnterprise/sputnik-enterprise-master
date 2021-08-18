Namespace UI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class ucMediationAndRentalData
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
			Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
			Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ucMediationAndRentalData))
			Me.grpanstellungsart = New DevComponents.DotNetBar.Controls.GroupPanel()
			Me.lstEmploymentType = New DevExpress.XtraEditors.ListBoxControl()
			Me.lueEmploymentType = New DevExpress.XtraEditors.LookUpEdit()
			Me.grpstichwort = New DevComponents.DotNetBar.Controls.GroupPanel()
			Me.ucKeywordsPopup = New SP.Infrastructure.ucListSelectPopup()
			Me.btnAddKeywords = New DevExpress.XtraEditors.SimpleButton()
			Me.lstKeywords = New DevExpress.XtraEditors.ListBoxControl()
			Me.grpgav = New DevComponents.DotNetBar.Controls.GroupPanel()
			Me.ucGAVPopup = New SP.Infrastructure.ucListSelectPopup()
			Me.btnAddGAV = New DevExpress.XtraEditors.SimpleButton()
			Me.lstGAV = New DevExpress.XtraEditors.ListBoxControl()
			Me.grpbranchen = New DevComponents.DotNetBar.Controls.GroupPanel()
			Me.btnAddSector = New DevExpress.XtraEditors.SimpleButton()
			Me.lstSector = New DevExpress.XtraEditors.ListBoxControl()
			Me.grpberufe = New DevComponents.DotNetBar.Controls.GroupPanel()
			Me.lstProfessions = New DevExpress.XtraEditors.ListBoxControl()
			Me.btnAddJobs = New DevExpress.XtraEditors.SimpleButton()
			Me.XtraScrollableControl1 = New DevExpress.XtraEditors.XtraScrollableControl()
			Me.grpanstellungsart.SuspendLayout()
			CType(Me.lstEmploymentType, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.lueEmploymentType.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.grpstichwort.SuspendLayout()
			CType(Me.lstKeywords, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.grpgav.SuspendLayout()
			CType(Me.lstGAV, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.grpbranchen.SuspendLayout()
			CType(Me.lstSector, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.grpberufe.SuspendLayout()
			CType(Me.lstProfessions, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.XtraScrollableControl1.SuspendLayout()
			Me.SuspendLayout()
			'
			'grpanstellungsart
			'
			Me.grpanstellungsart.BackColor = System.Drawing.Color.Transparent
			Me.grpanstellungsart.CanvasColor = System.Drawing.SystemColors.Control
			Me.grpanstellungsart.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
			Me.grpanstellungsart.Controls.Add(Me.lstEmploymentType)
			Me.grpanstellungsart.Controls.Add(Me.lueEmploymentType)
			Me.grpanstellungsart.Location = New System.Drawing.Point(364, 237)
			Me.grpanstellungsart.Name = "grpanstellungsart"
			Me.grpanstellungsart.Size = New System.Drawing.Size(339, 209)
			'
			'
			'
			Me.grpanstellungsart.Style.BackColorGradientAngle = 90
			Me.grpanstellungsart.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpanstellungsart.Style.BorderBottomWidth = 1
			Me.grpanstellungsart.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
			Me.grpanstellungsart.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpanstellungsart.Style.BorderLeftWidth = 1
			Me.grpanstellungsart.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpanstellungsart.Style.BorderRightWidth = 1
			Me.grpanstellungsart.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpanstellungsart.Style.BorderTopWidth = 1
			Me.grpanstellungsart.Style.CornerDiameter = 4
			Me.grpanstellungsart.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
			Me.grpanstellungsart.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grpanstellungsart.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
			Me.grpanstellungsart.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
			Me.grpanstellungsart.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
			'
			'
			'
			Me.grpanstellungsart.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
			'
			'
			'
			Me.grpanstellungsart.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
			Me.grpanstellungsart.TabIndex = 217
			Me.grpanstellungsart.Text = "KD_Anstellungsarten"
			'
			'lstEmploymentType
			'
			Me.lstEmploymentType.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
							Or System.Windows.Forms.AnchorStyles.Left) _
							Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.lstEmploymentType.Location = New System.Drawing.Point(9, 31)
			Me.lstEmploymentType.Name = "lstEmploymentType"
			Me.lstEmploymentType.Size = New System.Drawing.Size(313, 145)
			Me.lstEmploymentType.TabIndex = 204
			'
			'lueEmploymentType
			'
			Me.lueEmploymentType.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
							Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.lueEmploymentType.Location = New System.Drawing.Point(9, 3)
			Me.lueEmploymentType.Name = "lueEmploymentType"
			SerializableAppearanceObject1.ForeColor = System.Drawing.Color.Red
			SerializableAppearanceObject1.Options.UseForeColor = True
			Me.lueEmploymentType.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, "", Nothing, Nothing, True)})
			Me.lueEmploymentType.Properties.ShowFooter = False
			Me.lueEmploymentType.Size = New System.Drawing.Size(313, 20)
			Me.lueEmploymentType.TabIndex = 23
			'
			'grpstichwort
			'
			Me.grpstichwort.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
							Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.grpstichwort.BackColor = System.Drawing.Color.Transparent
			Me.grpstichwort.CanvasColor = System.Drawing.SystemColors.Control
			Me.grpstichwort.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
			Me.grpstichwort.Controls.Add(Me.ucKeywordsPopup)
			Me.grpstichwort.Controls.Add(Me.btnAddKeywords)
			Me.grpstichwort.Controls.Add(Me.lstKeywords)
			Me.grpstichwort.Location = New System.Drawing.Point(715, 237)
			Me.grpstichwort.Name = "grpstichwort"
			Me.grpstichwort.Size = New System.Drawing.Size(333, 209)
			'
			'
			'
			Me.grpstichwort.Style.BackColorGradientAngle = 90
			Me.grpstichwort.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpstichwort.Style.BorderBottomWidth = 1
			Me.grpstichwort.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
			Me.grpstichwort.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpstichwort.Style.BorderLeftWidth = 1
			Me.grpstichwort.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpstichwort.Style.BorderRightWidth = 1
			Me.grpstichwort.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpstichwort.Style.BorderTopWidth = 1
			Me.grpstichwort.Style.CornerDiameter = 4
			Me.grpstichwort.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
			Me.grpstichwort.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grpstichwort.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
			Me.grpstichwort.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
			Me.grpstichwort.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
			'
			'
			'
			Me.grpstichwort.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
			'
			'
			'
			Me.grpstichwort.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
			Me.grpstichwort.TabIndex = 216
			Me.grpstichwort.Text = "Stichwörter"
			'
			'ucKeywordsPopup
			'
			Me.ucKeywordsPopup.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.ucKeywordsPopup.Location = New System.Drawing.Point(294, 31)
			Me.ucKeywordsPopup.Name = "ucKeywordsPopup"
			Me.ucKeywordsPopup.Size = New System.Drawing.Size(11, 10)
			Me.ucKeywordsPopup.TabIndex = 216
			'
			'btnAddKeywords
			'
			Me.btnAddKeywords.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnAddKeywords.Image = CType(resources.GetObject("btnAddKeywords.Image"), System.Drawing.Image)
			Me.btnAddKeywords.Location = New System.Drawing.Point(292, 3)
			Me.btnAddKeywords.Name = "btnAddKeywords"
			Me.btnAddKeywords.Size = New System.Drawing.Size(25, 22)
			Me.btnAddKeywords.TabIndex = 215
			'
			'lstKeywords
			'
			Me.lstKeywords.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
							Or System.Windows.Forms.AnchorStyles.Left) _
							Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.lstKeywords.Location = New System.Drawing.Point(9, 3)
			Me.lstKeywords.Name = "lstKeywords"
			Me.lstKeywords.Size = New System.Drawing.Size(273, 178)
			Me.lstKeywords.TabIndex = 203
			'
			'grpgav
			'
			Me.grpgav.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
							Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.grpgav.BackColor = System.Drawing.Color.Transparent
			Me.grpgav.CanvasColor = System.Drawing.SystemColors.Control
			Me.grpgav.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
			Me.grpgav.Controls.Add(Me.ucGAVPopup)
			Me.grpgav.Controls.Add(Me.btnAddGAV)
			Me.grpgav.Controls.Add(Me.lstGAV)
			Me.grpgav.Location = New System.Drawing.Point(715, 7)
			Me.grpgav.Name = "grpgav"
			Me.grpgav.Size = New System.Drawing.Size(333, 209)
			'
			'
			'
			Me.grpgav.Style.BackColorGradientAngle = 90
			Me.grpgav.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpgav.Style.BorderBottomWidth = 1
			Me.grpgav.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
			Me.grpgav.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpgav.Style.BorderLeftWidth = 1
			Me.grpgav.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpgav.Style.BorderRightWidth = 1
			Me.grpgav.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpgav.Style.BorderTopWidth = 1
			Me.grpgav.Style.CornerDiameter = 4
			Me.grpgav.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
			Me.grpgav.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grpgav.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
			Me.grpgav.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
			Me.grpgav.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
			'
			'
			'
			Me.grpgav.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
			'
			'
			'
			Me.grpgav.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
			Me.grpgav.TabIndex = 215
			Me.grpgav.Text = "GAV-Unterstellung"
			'
			'ucGAVPopup
			'
			Me.ucGAVPopup.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.ucGAVPopup.Location = New System.Drawing.Point(292, 32)
			Me.ucGAVPopup.Name = "ucGAVPopup"
			Me.ucGAVPopup.Size = New System.Drawing.Size(11, 10)
			Me.ucGAVPopup.TabIndex = 214
			'
			'btnAddGAV
			'
			Me.btnAddGAV.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnAddGAV.Image = CType(resources.GetObject("btnAddGAV.Image"), System.Drawing.Image)
			Me.btnAddGAV.Location = New System.Drawing.Point(289, 3)
			Me.btnAddGAV.Name = "btnAddGAV"
			Me.btnAddGAV.Size = New System.Drawing.Size(25, 22)
			Me.btnAddGAV.TabIndex = 213
			'
			'lstGAV
			'
			Me.lstGAV.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
							Or System.Windows.Forms.AnchorStyles.Left) _
							Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.lstGAV.Location = New System.Drawing.Point(9, 3)
			Me.lstGAV.Name = "lstGAV"
			Me.lstGAV.Size = New System.Drawing.Size(273, 179)
			Me.lstGAV.TabIndex = 203
			'
			'grpbranchen
			'
			Me.grpbranchen.BackColor = System.Drawing.Color.Transparent
			Me.grpbranchen.CanvasColor = System.Drawing.SystemColors.Control
			Me.grpbranchen.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
			Me.grpbranchen.Controls.Add(Me.btnAddSector)
			Me.grpbranchen.Controls.Add(Me.lstSector)
			Me.grpbranchen.Location = New System.Drawing.Point(364, 7)
			Me.grpbranchen.Name = "grpbranchen"
			Me.grpbranchen.Size = New System.Drawing.Size(339, 209)
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
			Me.grpbranchen.TabIndex = 214
			Me.grpbranchen.Text = "Branchen / Gewerbe"
			'
			'btnAddSector
			'
			Me.btnAddSector.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnAddSector.Image = CType(resources.GetObject("btnAddSector.Image"), System.Drawing.Image)
			Me.btnAddSector.Location = New System.Drawing.Point(297, 3)
			Me.btnAddSector.Name = "btnAddSector"
			Me.btnAddSector.Size = New System.Drawing.Size(25, 22)
			Me.btnAddSector.TabIndex = 214
			'
			'lstSector
			'
			Me.lstSector.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
							Or System.Windows.Forms.AnchorStyles.Left) _
							Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.lstSector.Location = New System.Drawing.Point(9, 3)
			Me.lstSector.Name = "lstSector"
			Me.lstSector.Size = New System.Drawing.Size(279, 176)
			Me.lstSector.TabIndex = 202
			'
			'grpberufe
			'
			Me.grpberufe.BackColor = System.Drawing.Color.Transparent
			Me.grpberufe.CanvasColor = System.Drawing.SystemColors.Control
			Me.grpberufe.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
			Me.grpberufe.Controls.Add(Me.lstProfessions)
			Me.grpberufe.Controls.Add(Me.btnAddJobs)
			Me.grpberufe.Location = New System.Drawing.Point(13, 7)
			Me.grpberufe.Name = "grpberufe"
			Me.grpberufe.Size = New System.Drawing.Size(339, 439)
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
			Me.grpberufe.TabIndex = 212
			Me.grpberufe.Text = "Berufe"
			'
			'lstProfessions
			'
			Me.lstProfessions.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
							Or System.Windows.Forms.AnchorStyles.Left) _
							Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.lstProfessions.Location = New System.Drawing.Point(9, 3)
			Me.lstProfessions.Name = "lstProfessions"
			Me.lstProfessions.Size = New System.Drawing.Size(279, 407)
			Me.lstProfessions.TabIndex = 202
			'
			'btnAddJobs
			'
			Me.btnAddJobs.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnAddJobs.Image = CType(resources.GetObject("btnAddJobs.Image"), System.Drawing.Image)
			Me.btnAddJobs.Location = New System.Drawing.Point(298, 3)
			Me.btnAddJobs.Name = "btnAddJobs"
			Me.btnAddJobs.Size = New System.Drawing.Size(25, 22)
			Me.btnAddJobs.TabIndex = 212
			'
			'XtraScrollableControl1
			'
			Me.XtraScrollableControl1.Controls.Add(Me.grpanstellungsart)
			Me.XtraScrollableControl1.Controls.Add(Me.grpstichwort)
			Me.XtraScrollableControl1.Controls.Add(Me.grpgav)
			Me.XtraScrollableControl1.Controls.Add(Me.grpbranchen)
			Me.XtraScrollableControl1.Controls.Add(Me.grpberufe)
			Me.XtraScrollableControl1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.XtraScrollableControl1.Location = New System.Drawing.Point(5, 5)
			Me.XtraScrollableControl1.Name = "XtraScrollableControl1"
			Me.XtraScrollableControl1.Size = New System.Drawing.Size(1063, 463)
			Me.XtraScrollableControl1.TabIndex = 218
			'
			'ucMediationAndRentalData
			'
			Me.Appearance.BackColor = System.Drawing.Color.Transparent
			Me.Appearance.Options.UseBackColor = True
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.Controls.Add(Me.XtraScrollableControl1)
			Me.Name = "ucMediationAndRentalData"
			Me.Padding = New System.Windows.Forms.Padding(5)
			Me.Size = New System.Drawing.Size(1073, 473)
			Me.grpanstellungsart.ResumeLayout(False)
			CType(Me.lstEmploymentType, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.lueEmploymentType.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			Me.grpstichwort.ResumeLayout(False)
			CType(Me.lstKeywords, System.ComponentModel.ISupportInitialize).EndInit()
			Me.grpgav.ResumeLayout(False)
			CType(Me.lstGAV, System.ComponentModel.ISupportInitialize).EndInit()
			Me.grpbranchen.ResumeLayout(False)
			CType(Me.lstSector, System.ComponentModel.ISupportInitialize).EndInit()
			Me.grpberufe.ResumeLayout(False)
			CType(Me.lstProfessions, System.ComponentModel.ISupportInitialize).EndInit()
			Me.XtraScrollableControl1.ResumeLayout(False)
			Me.ResumeLayout(False)

		End Sub
    Friend WithEvents grpanstellungsart As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents lstEmploymentType As DevExpress.XtraEditors.ListBoxControl
    Friend WithEvents lueEmploymentType As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents grpstichwort As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents btnAddKeywords As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents lstKeywords As DevExpress.XtraEditors.ListBoxControl
    Friend WithEvents grpgav As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents btnAddGAV As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents lstGAV As DevExpress.XtraEditors.ListBoxControl
    Friend WithEvents grpbranchen As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents btnAddSector As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents lstSector As DevExpress.XtraEditors.ListBoxControl
    Friend WithEvents grpberufe As DevComponents.DotNetBar.Controls.GroupPanel
        Friend WithEvents lstProfessions As DevExpress.XtraEditors.ListBoxControl
        Friend WithEvents btnAddJobs As DevExpress.XtraEditors.SimpleButton
        Friend WithEvents ucKeywordsPopup As SP.Infrastructure.ucListSelectPopup
    Friend WithEvents ucGAVPopup As SP.Infrastructure.ucListSelectPopup
    Friend WithEvents XtraScrollableControl1 As DevExpress.XtraEditors.XtraScrollableControl

    End Class

End Namespace