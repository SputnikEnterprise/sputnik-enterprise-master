Namespace UI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class ucDisposal
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
			Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ucDisposal))
			Me.btnAddContactType = New DevExpress.XtraEditors.SimpleButton()
			Me.grpVersandart = New DevComponents.DotNetBar.Controls.GroupPanel()
			Me.ucContactTypeDataPopup = New SP.Infrastructure.ucListSelectPopup()
			Me.lstContactType = New DevExpress.XtraEditors.ListBoxControl()
			Me.grpKommunikation = New DevComponents.DotNetBar.Controls.GroupPanel()
			Me.ucComDataPopup = New SP.Infrastructure.ucListSelectPopup()
			Me.lstCommData = New DevExpress.XtraEditors.ListBoxControl()
			Me.btnAddCommunicationData = New DevExpress.XtraEditors.SimpleButton()
			Me.grp4Reserve = New DevComponents.DotNetBar.Controls.GroupPanel()
			Me.ucReserve4Popup = New SP.Infrastructure.ucListSelectPopup()
			Me.btnAddReserve4 = New DevExpress.XtraEditors.SimpleButton()
			Me.lstReserve4 = New DevExpress.XtraEditors.ListBoxControl()
			Me.grp3Reserve = New DevComponents.DotNetBar.Controls.GroupPanel()
			Me.ucReserve3Popup = New SP.Infrastructure.ucListSelectPopup()
			Me.lstReserve3 = New DevExpress.XtraEditors.ListBoxControl()
			Me.btnAddReserve3 = New DevExpress.XtraEditors.SimpleButton()
			Me.grp2Reserve = New DevComponents.DotNetBar.Controls.GroupPanel()
			Me.ucReserve2Popup = New SP.Infrastructure.ucListSelectPopup()
			Me.btnAddReserve2 = New DevExpress.XtraEditors.SimpleButton()
			Me.lstReserve2 = New DevExpress.XtraEditors.ListBoxControl()
			Me.grp1Reserve = New DevComponents.DotNetBar.Controls.GroupPanel()
			Me.ucReserve1Popup = New SP.Infrastructure.ucListSelectPopup()
			Me.lstReserve1 = New DevExpress.XtraEditors.ListBoxControl()
			Me.btnAddReserve1 = New DevExpress.XtraEditors.SimpleButton()
			Me.grpVersandart.SuspendLayout()
			CType(Me.lstContactType, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.grpKommunikation.SuspendLayout()
			CType(Me.lstCommData, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.grp4Reserve.SuspendLayout()
			CType(Me.lstReserve4, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.grp3Reserve.SuspendLayout()
			CType(Me.lstReserve3, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.grp2Reserve.SuspendLayout()
			CType(Me.lstReserve2, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.grp1Reserve.SuspendLayout()
			CType(Me.lstReserve1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			'
			'btnAddContactType
			'
			Me.btnAddContactType.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnAddContactType.Image = CType(resources.GetObject("btnAddContactType.Image"), System.Drawing.Image)
			Me.btnAddContactType.Location = New System.Drawing.Point(286, 3)
			Me.btnAddContactType.Name = "btnAddContactType"
			Me.btnAddContactType.Size = New System.Drawing.Size(25, 22)
			Me.btnAddContactType.TabIndex = 214
			Me.btnAddContactType.Text = "..."
			'
			'grpVersandart
			'
			Me.grpVersandart.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
							Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.grpVersandart.BackColor = System.Drawing.Color.Transparent
			Me.grpVersandart.CanvasColor = System.Drawing.SystemColors.Control
			Me.grpVersandart.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
			Me.grpVersandart.Controls.Add(Me.ucContactTypeDataPopup)
			Me.grpVersandart.Controls.Add(Me.btnAddContactType)
			Me.grpVersandart.Controls.Add(Me.lstContactType)
			Me.grpVersandart.Location = New System.Drawing.Point(438, 12)
			Me.grpVersandart.Name = "grpVersandart"
			Me.grpVersandart.Size = New System.Drawing.Size(327, 198)
			'
			'
			'
			Me.grpVersandart.Style.BackColorGradientAngle = 90
			Me.grpVersandart.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpVersandart.Style.BorderBottomWidth = 1
			Me.grpVersandart.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
			Me.grpVersandart.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpVersandart.Style.BorderLeftWidth = 1
			Me.grpVersandart.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpVersandart.Style.BorderRightWidth = 1
			Me.grpVersandart.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpVersandart.Style.BorderTopWidth = 1
			Me.grpVersandart.Style.CornerDiameter = 4
			Me.grpVersandart.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
			Me.grpVersandart.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grpVersandart.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
			Me.grpVersandart.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
			Me.grpVersandart.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
			'
			'
			'
			Me.grpVersandart.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
			'
			'
			'
			Me.grpVersandart.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
			Me.grpVersandart.TabIndex = 228
			Me.grpVersandart.Text = "ZHD_Versand"
			'
			'ucContactTypeDataPopup
			'
			Me.ucContactTypeDataPopup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.ucContactTypeDataPopup.Location = New System.Drawing.Point(286, 32)
			Me.ucContactTypeDataPopup.Name = "ucContactTypeDataPopup"
			Me.ucContactTypeDataPopup.Size = New System.Drawing.Size(11, 10)
			Me.ucContactTypeDataPopup.TabIndex = 215
			Me.ucContactTypeDataPopup.Visible = False
			'
			'lstContactType
			'
			Me.lstContactType.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
							Or System.Windows.Forms.AnchorStyles.Left) _
							Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.lstContactType.Location = New System.Drawing.Point(9, 3)
			Me.lstContactType.Name = "lstContactType"
			Me.lstContactType.Size = New System.Drawing.Size(271, 155)
			Me.lstContactType.TabIndex = 202
			'
			'grpKommunikation
			'
			Me.grpKommunikation.BackColor = System.Drawing.Color.Transparent
			Me.grpKommunikation.CanvasColor = System.Drawing.SystemColors.Control
			Me.grpKommunikation.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
			Me.grpKommunikation.Controls.Add(Me.ucComDataPopup)
			Me.grpKommunikation.Controls.Add(Me.lstCommData)
			Me.grpKommunikation.Controls.Add(Me.btnAddCommunicationData)
			Me.grpKommunikation.Location = New System.Drawing.Point(13, 12)
			Me.grpKommunikation.Name = "grpKommunikation"
			Me.grpKommunikation.Size = New System.Drawing.Size(384, 198)
			'
			'
			'
			Me.grpKommunikation.Style.BackColorGradientAngle = 90
			Me.grpKommunikation.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpKommunikation.Style.BorderBottomWidth = 1
			Me.grpKommunikation.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
			Me.grpKommunikation.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpKommunikation.Style.BorderLeftWidth = 1
			Me.grpKommunikation.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpKommunikation.Style.BorderRightWidth = 1
			Me.grpKommunikation.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpKommunikation.Style.BorderTopWidth = 1
			Me.grpKommunikation.Style.CornerDiameter = 4
			Me.grpKommunikation.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
			Me.grpKommunikation.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grpKommunikation.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
			Me.grpKommunikation.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
			Me.grpKommunikation.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
			'
			'
			'
			Me.grpKommunikation.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
			'
			'
			'
			Me.grpKommunikation.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
			Me.grpKommunikation.TabIndex = 227
			Me.grpKommunikation.Text = "Kommunikation"
			'
			'ucComDataPopup
			'
			Me.ucComDataPopup.Appearance.BackColor = System.Drawing.Color.White
			Me.ucComDataPopup.Appearance.Options.UseBackColor = True
			Me.ucComDataPopup.Location = New System.Drawing.Point(344, 32)
			Me.ucComDataPopup.Name = "ucComDataPopup"
			Me.ucComDataPopup.Size = New System.Drawing.Size(11, 10)
			Me.ucComDataPopup.TabIndex = 213
			Me.ucComDataPopup.Visible = False
			'
			'lstCommData
			'
			Me.lstCommData.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
							Or System.Windows.Forms.AnchorStyles.Left) _
							Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.lstCommData.Location = New System.Drawing.Point(9, 3)
			Me.lstCommData.Name = "lstCommData"
			Me.lstCommData.Size = New System.Drawing.Size(329, 155)
			Me.lstCommData.TabIndex = 202
			'
			'btnAddCommunicationData
			'
			Me.btnAddCommunicationData.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnAddCommunicationData.Image = CType(resources.GetObject("btnAddCommunicationData.Image"), System.Drawing.Image)
			Me.btnAddCommunicationData.Location = New System.Drawing.Point(345, 3)
			Me.btnAddCommunicationData.Name = "btnAddCommunicationData"
			Me.btnAddCommunicationData.Size = New System.Drawing.Size(25, 22)
			Me.btnAddCommunicationData.TabIndex = 212
			Me.btnAddCommunicationData.Text = "..."
			'
			'grp4Reserve
			'
			Me.grp4Reserve.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
							Or System.Windows.Forms.AnchorStyles.Left) _
							Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.grp4Reserve.BackColor = System.Drawing.Color.Transparent
			Me.grp4Reserve.CanvasColor = System.Drawing.SystemColors.Control
			Me.grp4Reserve.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
			Me.grp4Reserve.Controls.Add(Me.ucReserve4Popup)
			Me.grp4Reserve.Controls.Add(Me.btnAddReserve4)
			Me.grp4Reserve.Controls.Add(Me.lstReserve4)
			Me.grp4Reserve.Location = New System.Drawing.Point(438, 424)
			Me.grp4Reserve.Name = "grp4Reserve"
			Me.grp4Reserve.Size = New System.Drawing.Size(327, 129)
			'
			'
			'
			Me.grp4Reserve.Style.BackColorGradientAngle = 90
			Me.grp4Reserve.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grp4Reserve.Style.BorderBottomWidth = 1
			Me.grp4Reserve.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
			Me.grp4Reserve.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grp4Reserve.Style.BorderLeftWidth = 1
			Me.grp4Reserve.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grp4Reserve.Style.BorderRightWidth = 1
			Me.grp4Reserve.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grp4Reserve.Style.BorderTopWidth = 1
			Me.grp4Reserve.Style.CornerDiameter = 4
			Me.grp4Reserve.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
			Me.grp4Reserve.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grp4Reserve.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
			Me.grp4Reserve.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
			Me.grp4Reserve.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
			'
			'
			'
			Me.grp4Reserve.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
			'
			'
			'
			Me.grp4Reserve.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
			Me.grp4Reserve.TabIndex = 226
			Me.grp4Reserve.Text = "ZHD_Res4"
			'
			'ucReserve4Popup
			'
			Me.ucReserve4Popup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.ucReserve4Popup.Location = New System.Drawing.Point(284, 32)
			Me.ucReserve4Popup.Name = "ucReserve4Popup"
			Me.ucReserve4Popup.Size = New System.Drawing.Size(11, 10)
			Me.ucReserve4Popup.TabIndex = 215
			Me.ucReserve4Popup.Visible = False
			'
			'btnAddReserve4
			'
			Me.btnAddReserve4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnAddReserve4.Image = CType(resources.GetObject("btnAddReserve4.Image"), System.Drawing.Image)
			Me.btnAddReserve4.Location = New System.Drawing.Point(286, 3)
			Me.btnAddReserve4.Name = "btnAddReserve4"
			Me.btnAddReserve4.Size = New System.Drawing.Size(25, 22)
			Me.btnAddReserve4.TabIndex = 214
			Me.btnAddReserve4.Text = "..."
			'
			'lstReserve4
			'
			Me.lstReserve4.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
							Or System.Windows.Forms.AnchorStyles.Left) _
							Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.lstReserve4.Location = New System.Drawing.Point(9, 3)
			Me.lstReserve4.Name = "lstReserve4"
			Me.lstReserve4.Size = New System.Drawing.Size(271, 86)
			Me.lstReserve4.TabIndex = 202
			'
			'grp3Reserve
			'
			Me.grp3Reserve.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
							Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
			Me.grp3Reserve.BackColor = System.Drawing.Color.Transparent
			Me.grp3Reserve.CanvasColor = System.Drawing.SystemColors.Control
			Me.grp3Reserve.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
			Me.grp3Reserve.Controls.Add(Me.ucReserve3Popup)
			Me.grp3Reserve.Controls.Add(Me.lstReserve3)
			Me.grp3Reserve.Controls.Add(Me.btnAddReserve3)
			Me.grp3Reserve.Location = New System.Drawing.Point(13, 424)
			Me.grp3Reserve.Name = "grp3Reserve"
			Me.grp3Reserve.Size = New System.Drawing.Size(384, 129)
			'
			'
			'
			Me.grp3Reserve.Style.BackColorGradientAngle = 90
			Me.grp3Reserve.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grp3Reserve.Style.BorderBottomWidth = 1
			Me.grp3Reserve.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
			Me.grp3Reserve.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grp3Reserve.Style.BorderLeftWidth = 1
			Me.grp3Reserve.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grp3Reserve.Style.BorderRightWidth = 1
			Me.grp3Reserve.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grp3Reserve.Style.BorderTopWidth = 1
			Me.grp3Reserve.Style.CornerDiameter = 4
			Me.grp3Reserve.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
			Me.grp3Reserve.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grp3Reserve.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
			Me.grp3Reserve.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
			Me.grp3Reserve.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
			'
			'
			'
			Me.grp3Reserve.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
			'
			'
			'
			Me.grp3Reserve.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
			Me.grp3Reserve.TabIndex = 225
			Me.grp3Reserve.Text = "ZHD_Res3"
			'
			'ucReserve3Popup
			'
			Me.ucReserve3Popup.Location = New System.Drawing.Point(344, 29)
			Me.ucReserve3Popup.Name = "ucReserve3Popup"
			Me.ucReserve3Popup.Size = New System.Drawing.Size(11, 10)
			Me.ucReserve3Popup.TabIndex = 213
			Me.ucReserve3Popup.Visible = False
			'
			'lstReserve3
			'
			Me.lstReserve3.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
							Or System.Windows.Forms.AnchorStyles.Left) _
							Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.lstReserve3.Location = New System.Drawing.Point(9, 3)
			Me.lstReserve3.Name = "lstReserve3"
			Me.lstReserve3.Size = New System.Drawing.Size(329, 86)
			Me.lstReserve3.TabIndex = 202
			'
			'btnAddReserve3
			'
			Me.btnAddReserve3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnAddReserve3.Image = CType(resources.GetObject("btnAddReserve3.Image"), System.Drawing.Image)
			Me.btnAddReserve3.Location = New System.Drawing.Point(345, 3)
			Me.btnAddReserve3.Name = "btnAddReserve3"
			Me.btnAddReserve3.Size = New System.Drawing.Size(25, 22)
			Me.btnAddReserve3.TabIndex = 212
			Me.btnAddReserve3.Text = "..."
			'
			'grp2Reserve
			'
			Me.grp2Reserve.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
							Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.grp2Reserve.BackColor = System.Drawing.Color.Transparent
			Me.grp2Reserve.CanvasColor = System.Drawing.SystemColors.Control
			Me.grp2Reserve.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
			Me.grp2Reserve.Controls.Add(Me.ucReserve2Popup)
			Me.grp2Reserve.Controls.Add(Me.btnAddReserve2)
			Me.grp2Reserve.Controls.Add(Me.lstReserve2)
			Me.grp2Reserve.Location = New System.Drawing.Point(438, 218)
			Me.grp2Reserve.Name = "grp2Reserve"
			Me.grp2Reserve.Size = New System.Drawing.Size(327, 198)
			'
			'
			'
			Me.grp2Reserve.Style.BackColorGradientAngle = 90
			Me.grp2Reserve.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grp2Reserve.Style.BorderBottomWidth = 1
			Me.grp2Reserve.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
			Me.grp2Reserve.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grp2Reserve.Style.BorderLeftWidth = 1
			Me.grp2Reserve.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grp2Reserve.Style.BorderRightWidth = 1
			Me.grp2Reserve.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grp2Reserve.Style.BorderTopWidth = 1
			Me.grp2Reserve.Style.CornerDiameter = 4
			Me.grp2Reserve.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
			Me.grp2Reserve.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grp2Reserve.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
			Me.grp2Reserve.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
			Me.grp2Reserve.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
			'
			'
			'
			Me.grp2Reserve.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
			'
			'
			'
			Me.grp2Reserve.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
			Me.grp2Reserve.TabIndex = 224
			Me.grp2Reserve.Text = "ZHD_Res2"
			'
			'ucReserve2Popup
			'
			Me.ucReserve2Popup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.ucReserve2Popup.Location = New System.Drawing.Point(286, 32)
			Me.ucReserve2Popup.Name = "ucReserve2Popup"
			Me.ucReserve2Popup.Size = New System.Drawing.Size(11, 10)
			Me.ucReserve2Popup.TabIndex = 215
			Me.ucReserve2Popup.Visible = False
			'
			'btnAddReserve2
			'
			Me.btnAddReserve2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnAddReserve2.Image = CType(resources.GetObject("btnAddReserve2.Image"), System.Drawing.Image)
			Me.btnAddReserve2.Location = New System.Drawing.Point(286, 3)
			Me.btnAddReserve2.Name = "btnAddReserve2"
			Me.btnAddReserve2.Size = New System.Drawing.Size(25, 22)
			Me.btnAddReserve2.TabIndex = 214
			Me.btnAddReserve2.Text = "..."
			'
			'lstReserve2
			'
			Me.lstReserve2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
							Or System.Windows.Forms.AnchorStyles.Left) _
							Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.lstReserve2.Location = New System.Drawing.Point(9, 3)
			Me.lstReserve2.Name = "lstReserve2"
			Me.lstReserve2.Size = New System.Drawing.Size(271, 155)
			Me.lstReserve2.TabIndex = 202
			'
			'grp1Reserve
			'
			Me.grp1Reserve.BackColor = System.Drawing.Color.Transparent
			Me.grp1Reserve.CanvasColor = System.Drawing.SystemColors.Control
			Me.grp1Reserve.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
			Me.grp1Reserve.Controls.Add(Me.ucReserve1Popup)
			Me.grp1Reserve.Controls.Add(Me.lstReserve1)
			Me.grp1Reserve.Controls.Add(Me.btnAddReserve1)
			Me.grp1Reserve.Location = New System.Drawing.Point(13, 218)
			Me.grp1Reserve.Name = "grp1Reserve"
			Me.grp1Reserve.Size = New System.Drawing.Size(384, 198)
			'
			'
			'
			Me.grp1Reserve.Style.BackColorGradientAngle = 90
			Me.grp1Reserve.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grp1Reserve.Style.BorderBottomWidth = 1
			Me.grp1Reserve.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
			Me.grp1Reserve.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grp1Reserve.Style.BorderLeftWidth = 1
			Me.grp1Reserve.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grp1Reserve.Style.BorderRightWidth = 1
			Me.grp1Reserve.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grp1Reserve.Style.BorderTopWidth = 1
			Me.grp1Reserve.Style.CornerDiameter = 4
			Me.grp1Reserve.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
			Me.grp1Reserve.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grp1Reserve.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
			Me.grp1Reserve.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
			Me.grp1Reserve.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
			'
			'
			'
			Me.grp1Reserve.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
			'
			'
			'
			Me.grp1Reserve.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
			Me.grp1Reserve.TabIndex = 223
			Me.grp1Reserve.Text = "ZHD_Res1"
			'
			'ucReserve1Popup
			'
			Me.ucReserve1Popup.Location = New System.Drawing.Point(344, 29)
			Me.ucReserve1Popup.Name = "ucReserve1Popup"
			Me.ucReserve1Popup.Size = New System.Drawing.Size(11, 10)
			Me.ucReserve1Popup.TabIndex = 213
			Me.ucReserve1Popup.Visible = False
			'
			'lstReserve1
			'
			Me.lstReserve1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
							Or System.Windows.Forms.AnchorStyles.Left) _
							Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.lstReserve1.Location = New System.Drawing.Point(9, 3)
			Me.lstReserve1.Name = "lstReserve1"
			Me.lstReserve1.Size = New System.Drawing.Size(329, 155)
			Me.lstReserve1.TabIndex = 202
			'
			'btnAddReserve1
			'
			Me.btnAddReserve1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnAddReserve1.Image = CType(resources.GetObject("btnAddReserve1.Image"), System.Drawing.Image)
			Me.btnAddReserve1.Location = New System.Drawing.Point(345, 3)
			Me.btnAddReserve1.Name = "btnAddReserve1"
			Me.btnAddReserve1.Size = New System.Drawing.Size(25, 22)
			Me.btnAddReserve1.TabIndex = 212
			Me.btnAddReserve1.Text = "..."
			'
			'ucDisposal
			'
			Me.Appearance.BackColor = System.Drawing.Color.Transparent
			Me.Appearance.Options.UseBackColor = True
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.Controls.Add(Me.grpVersandart)
			Me.Controls.Add(Me.grpKommunikation)
			Me.Controls.Add(Me.grp4Reserve)
			Me.Controls.Add(Me.grp3Reserve)
			Me.Controls.Add(Me.grp2Reserve)
			Me.Controls.Add(Me.grp1Reserve)
			Me.Name = "ucDisposal"
			Me.Size = New System.Drawing.Size(782, 576)
			Me.grpVersandart.ResumeLayout(False)
			CType(Me.lstContactType, System.ComponentModel.ISupportInitialize).EndInit()
			Me.grpKommunikation.ResumeLayout(False)
			CType(Me.lstCommData, System.ComponentModel.ISupportInitialize).EndInit()
			Me.grp4Reserve.ResumeLayout(False)
			CType(Me.lstReserve4, System.ComponentModel.ISupportInitialize).EndInit()
			Me.grp3Reserve.ResumeLayout(False)
			CType(Me.lstReserve3, System.ComponentModel.ISupportInitialize).EndInit()
			Me.grp2Reserve.ResumeLayout(False)
			CType(Me.lstReserve2, System.ComponentModel.ISupportInitialize).EndInit()
			Me.grp1Reserve.ResumeLayout(False)
			CType(Me.lstReserve1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub
    Friend WithEvents grpVersandart As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents lstContactType As DevExpress.XtraEditors.ListBoxControl
    Friend WithEvents grpKommunikation As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents lstCommData As DevExpress.XtraEditors.ListBoxControl
    Friend WithEvents btnAddCommunicationData As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents grp4Reserve As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents btnAddReserve4 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents lstReserve4 As DevExpress.XtraEditors.ListBoxControl
    Friend WithEvents grp3Reserve As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents lstReserve3 As DevExpress.XtraEditors.ListBoxControl
    Friend WithEvents btnAddReserve3 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents grp2Reserve As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents btnAddReserve2 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents lstReserve2 As DevExpress.XtraEditors.ListBoxControl
    Friend WithEvents grp1Reserve As DevComponents.DotNetBar.Controls.GroupPanel
        Friend WithEvents lstReserve1 As DevExpress.XtraEditors.ListBoxControl
        Friend WithEvents btnAddReserve1 As DevExpress.XtraEditors.SimpleButton
        Friend WithEvents ucComDataPopup As SP.Infrastructure.ucListSelectPopup
        Friend WithEvents ucContactTypeDataPopup As SP.Infrastructure.ucListSelectPopup
        Friend WithEvents btnAddContactType As DevExpress.XtraEditors.SimpleButton
        Friend WithEvents ucReserve4Popup As SP.Infrastructure.ucListSelectPopup
        Friend WithEvents ucReserve3Popup As SP.Infrastructure.ucListSelectPopup
        Friend WithEvents ucReserve2Popup As SP.Infrastructure.ucListSelectPopup
        Friend WithEvents ucReserve1Popup As SP.Infrastructure.ucListSelectPopup

    End Class
End Namespace
