
Namespace UI

  <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
  Partial Class ucContactData
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
			Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ucContactData))
			Me.grpkontaktdaten = New DevExpress.XtraEditors.GroupControl()
			Me.btnAddContact = New DevExpress.XtraEditors.SimpleButton()
			Me.grpInfo = New DevComponents.DotNetBar.Controls.GroupPanel()
			Me.XtraScrollableControl2 = New DevExpress.XtraEditors.XtraScrollableControl()
			Me.lblanzahl = New System.Windows.Forms.Label()
			Me.lblLastInfoValue = New System.Windows.Forms.Label()
			Me.lblNumberOfEntriesValue = New System.Windows.Forms.Label()
			Me.lblFirstInfoValue = New System.Windows.Forms.Label()
			Me.lblinfoneu = New System.Windows.Forms.Label()
			Me.lblinfoalt = New System.Windows.Forms.Label()
			Me.grpjahre = New DevComponents.DotNetBar.Controls.GroupPanel()
			Me.XtraScrollableControl3 = New DevExpress.XtraEditors.XtraScrollableControl()
			Me.lstYears = New DevExpress.XtraEditors.CheckedListBoxControl()
			Me.grpeintrag = New DevComponents.DotNetBar.Controls.GroupPanel()
			Me.XtraScrollableControl4 = New DevExpress.XtraEditors.XtraScrollableControl()
			Me.chkSMS = New DevExpress.XtraEditors.CheckEdit()
			Me.chkMailed = New DevExpress.XtraEditors.CheckEdit()
			Me.chkOffered = New DevExpress.XtraEditors.CheckEdit()
			Me.chkTelephone = New DevExpress.XtraEditors.CheckEdit()
			Me.gridContactData = New DevExpress.XtraGrid.GridControl()
			Me.gridViewContactData = New DevExpress.XtraGrid.Views.Grid.GridView()
			CType(Me.grpkontaktdaten, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.grpkontaktdaten.SuspendLayout()
			Me.grpInfo.SuspendLayout()
			Me.XtraScrollableControl2.SuspendLayout()
			Me.grpjahre.SuspendLayout()
			Me.XtraScrollableControl3.SuspendLayout()
			CType(Me.lstYears, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.grpeintrag.SuspendLayout()
			Me.XtraScrollableControl4.SuspendLayout()
			CType(Me.chkSMS.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.chkMailed.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.chkOffered.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.chkTelephone.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gridContactData, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gridViewContactData, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			'
			'grpkontaktdaten
			'
			Me.grpkontaktdaten.Appearance.BackColor = System.Drawing.Color.Transparent
			Me.grpkontaktdaten.Appearance.Options.UseBackColor = True
			Me.grpkontaktdaten.AppearanceCaption.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grpkontaktdaten.AppearanceCaption.Options.UseFont = True
			Me.grpkontaktdaten.Controls.Add(Me.btnAddContact)
			Me.grpkontaktdaten.Controls.Add(Me.grpInfo)
			Me.grpkontaktdaten.Controls.Add(Me.grpjahre)
			Me.grpkontaktdaten.Controls.Add(Me.grpeintrag)
			Me.grpkontaktdaten.Controls.Add(Me.gridContactData)
			Me.grpkontaktdaten.Dock = System.Windows.Forms.DockStyle.Fill
			Me.grpkontaktdaten.Location = New System.Drawing.Point(0, 0)
			Me.grpkontaktdaten.Name = "grpkontaktdaten"
			Me.grpkontaktdaten.Size = New System.Drawing.Size(736, 321)
			Me.grpkontaktdaten.TabIndex = 173
			Me.grpkontaktdaten.Text = "Kontaktdaten"
			'
			'btnAddContact
			'
			Me.btnAddContact.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnAddContact.ImageOptions.Image = CType(resources.GetObject("btnAddContact.ImageOptions.Image"), System.Drawing.Image)
			Me.btnAddContact.Location = New System.Drawing.Point(704, 3)
			Me.btnAddContact.Name = "btnAddContact"
			Me.btnAddContact.Size = New System.Drawing.Size(27, 15)
			Me.btnAddContact.TabIndex = 271
			'
			'grpInfo
			'
			Me.grpInfo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
			Me.grpInfo.BackColor = System.Drawing.Color.Transparent
			Me.grpInfo.CanvasColor = System.Drawing.SystemColors.Control
			Me.grpInfo.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
			Me.grpInfo.Controls.Add(Me.XtraScrollableControl2)
			Me.grpInfo.Location = New System.Drawing.Point(14, 174)
			Me.grpInfo.Name = "grpInfo"
			Me.grpInfo.Size = New System.Drawing.Size(251, 137)
			'
			'
			'
			Me.grpInfo.Style.BackColorGradientAngle = 90
			Me.grpInfo.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpInfo.Style.BorderBottomWidth = 1
			Me.grpInfo.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
			Me.grpInfo.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpInfo.Style.BorderLeftWidth = 1
			Me.grpInfo.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpInfo.Style.BorderRightWidth = 1
			Me.grpInfo.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpInfo.Style.BorderTopWidth = 1
			Me.grpInfo.Style.CornerDiameter = 4
			Me.grpInfo.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
			Me.grpInfo.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grpInfo.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
			Me.grpInfo.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
			Me.grpInfo.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
			'
			'
			'
			Me.grpInfo.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
			'
			'
			'
			Me.grpInfo.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
			Me.grpInfo.TabIndex = 206
			Me.grpInfo.Text = "Info"
			'
			'XtraScrollableControl2
			'
			Me.XtraScrollableControl2.Appearance.BackColor = System.Drawing.Color.Transparent
			Me.XtraScrollableControl2.Appearance.Options.UseBackColor = True
			Me.XtraScrollableControl2.Controls.Add(Me.lblanzahl)
			Me.XtraScrollableControl2.Controls.Add(Me.lblLastInfoValue)
			Me.XtraScrollableControl2.Controls.Add(Me.lblNumberOfEntriesValue)
			Me.XtraScrollableControl2.Controls.Add(Me.lblFirstInfoValue)
			Me.XtraScrollableControl2.Controls.Add(Me.lblinfoneu)
			Me.XtraScrollableControl2.Controls.Add(Me.lblinfoalt)
			Me.XtraScrollableControl2.Dock = System.Windows.Forms.DockStyle.Fill
			Me.XtraScrollableControl2.Location = New System.Drawing.Point(0, 0)
			Me.XtraScrollableControl2.Name = "XtraScrollableControl2"
			Me.XtraScrollableControl2.Size = New System.Drawing.Size(245, 115)
			Me.XtraScrollableControl2.TabIndex = 0
			'
			'lblanzahl
			'
			Me.lblanzahl.AutoSize = True
			Me.lblanzahl.Location = New System.Drawing.Point(3, 13)
			Me.lblanzahl.Name = "lblanzahl"
			Me.lblanzahl.Size = New System.Drawing.Size(82, 13)
			Me.lblanzahl.TabIndex = 207
			Me.lblanzahl.Text = "Anzahl Einträge"
			'
			'lblLastInfoValue
			'
			Me.lblLastInfoValue.AutoSize = True
			Me.lblLastInfoValue.ForeColor = System.Drawing.Color.Blue
			Me.lblLastInfoValue.Location = New System.Drawing.Point(167, 34)
			Me.lblLastInfoValue.Name = "lblLastInfoValue"
			Me.lblLastInfoValue.Size = New System.Drawing.Size(15, 13)
			Me.lblLastInfoValue.TabIndex = 212
			Me.lblLastInfoValue.Text = "[]"
			'
			'lblNumberOfEntriesValue
			'
			Me.lblNumberOfEntriesValue.AutoSize = True
			Me.lblNumberOfEntriesValue.Location = New System.Drawing.Point(167, 13)
			Me.lblNumberOfEntriesValue.Name = "lblNumberOfEntriesValue"
			Me.lblNumberOfEntriesValue.Size = New System.Drawing.Size(15, 13)
			Me.lblNumberOfEntriesValue.TabIndex = 208
			Me.lblNumberOfEntriesValue.Text = "[]"
			'
			'lblFirstInfoValue
			'
			Me.lblFirstInfoValue.AutoSize = True
			Me.lblFirstInfoValue.ForeColor = System.Drawing.Color.Blue
			Me.lblFirstInfoValue.Location = New System.Drawing.Point(167, 55)
			Me.lblFirstInfoValue.Name = "lblFirstInfoValue"
			Me.lblFirstInfoValue.Size = New System.Drawing.Size(15, 13)
			Me.lblFirstInfoValue.TabIndex = 211
			Me.lblFirstInfoValue.Text = "[]"
			'
			'lblinfoneu
			'
			Me.lblinfoneu.AutoSize = True
			Me.lblinfoneu.Location = New System.Drawing.Point(3, 34)
			Me.lblinfoneu.Name = "lblinfoneu"
			Me.lblinfoneu.Size = New System.Drawing.Size(77, 13)
			Me.lblinfoneu.TabIndex = 209
			Me.lblinfoneu.Text = "Aktuellste Info"
			'
			'lblinfoalt
			'
			Me.lblinfoalt.AutoSize = True
			Me.lblinfoalt.Location = New System.Drawing.Point(3, 55)
			Me.lblinfoalt.Name = "lblinfoalt"
			Me.lblinfoalt.Size = New System.Drawing.Size(64, 13)
			Me.lblinfoalt.TabIndex = 210
			Me.lblinfoalt.Text = "Älteste Info"
			'
			'grpjahre
			'
			Me.grpjahre.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.grpjahre.BackColor = System.Drawing.Color.Transparent
			Me.grpjahre.CanvasColor = System.Drawing.SystemColors.Control
			Me.grpjahre.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
			Me.grpjahre.Controls.Add(Me.XtraScrollableControl3)
			Me.grpjahre.Location = New System.Drawing.Point(549, 174)
			Me.grpjahre.Name = "grpjahre"
			Me.grpjahre.Size = New System.Drawing.Size(173, 137)
			'
			'
			'
			Me.grpjahre.Style.BackColorGradientAngle = 90
			Me.grpjahre.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpjahre.Style.BorderBottomWidth = 1
			Me.grpjahre.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
			Me.grpjahre.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpjahre.Style.BorderLeftWidth = 1
			Me.grpjahre.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpjahre.Style.BorderRightWidth = 1
			Me.grpjahre.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpjahre.Style.BorderTopWidth = 1
			Me.grpjahre.Style.CornerDiameter = 4
			Me.grpjahre.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
			Me.grpjahre.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grpjahre.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
			Me.grpjahre.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
			Me.grpjahre.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
			'
			'
			'
			Me.grpjahre.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
			'
			'
			'
			Me.grpjahre.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
			Me.grpjahre.TabIndex = 3
			Me.grpjahre.Text = "Gesuchte Jahre"
			'
			'XtraScrollableControl3
			'
			Me.XtraScrollableControl3.Controls.Add(Me.lstYears)
			Me.XtraScrollableControl3.Dock = System.Windows.Forms.DockStyle.Fill
			Me.XtraScrollableControl3.Location = New System.Drawing.Point(0, 0)
			Me.XtraScrollableControl3.Name = "XtraScrollableControl3"
			Me.XtraScrollableControl3.Size = New System.Drawing.Size(167, 115)
			Me.XtraScrollableControl3.TabIndex = 0
			'
			'lstYears
			'
			Me.lstYears.CheckOnClick = True
			Me.lstYears.Cursor = System.Windows.Forms.Cursors.Default
			Me.lstYears.Dock = System.Windows.Forms.DockStyle.Fill
			Me.lstYears.Location = New System.Drawing.Point(0, 0)
			Me.lstYears.Name = "lstYears"
			Me.lstYears.Size = New System.Drawing.Size(167, 115)
			Me.lstYears.TabIndex = 211
			'
			'grpeintrag
			'
			Me.grpeintrag.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.grpeintrag.BackColor = System.Drawing.Color.Transparent
			Me.grpeintrag.CanvasColor = System.Drawing.SystemColors.Control
			Me.grpeintrag.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
			Me.grpeintrag.Controls.Add(Me.XtraScrollableControl4)
			Me.grpeintrag.Location = New System.Drawing.Point(282, 174)
			Me.grpeintrag.Name = "grpeintrag"
			Me.grpeintrag.Size = New System.Drawing.Size(251, 137)
			'
			'
			'
			Me.grpeintrag.Style.BackColorGradientAngle = 90
			Me.grpeintrag.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpeintrag.Style.BorderBottomWidth = 1
			Me.grpeintrag.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
			Me.grpeintrag.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpeintrag.Style.BorderLeftWidth = 1
			Me.grpeintrag.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpeintrag.Style.BorderRightWidth = 1
			Me.grpeintrag.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpeintrag.Style.BorderTopWidth = 1
			Me.grpeintrag.Style.CornerDiameter = 4
			Me.grpeintrag.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
			Me.grpeintrag.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grpeintrag.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
			Me.grpeintrag.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
			Me.grpeintrag.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
			'
			'
			'
			Me.grpeintrag.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
			'
			'
			'
			Me.grpeintrag.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
			Me.grpeintrag.TabIndex = 2
			Me.grpeintrag.Text = "Einträge ausblenden für"
			'
			'XtraScrollableControl4
			'
			Me.XtraScrollableControl4.Appearance.BackColor = System.Drawing.Color.Transparent
			Me.XtraScrollableControl4.Appearance.Options.UseBackColor = True
			Me.XtraScrollableControl4.Controls.Add(Me.chkSMS)
			Me.XtraScrollableControl4.Controls.Add(Me.chkMailed)
			Me.XtraScrollableControl4.Controls.Add(Me.chkOffered)
			Me.XtraScrollableControl4.Controls.Add(Me.chkTelephone)
			Me.XtraScrollableControl4.Dock = System.Windows.Forms.DockStyle.Fill
			Me.XtraScrollableControl4.Location = New System.Drawing.Point(0, 0)
			Me.XtraScrollableControl4.Name = "XtraScrollableControl4"
			Me.XtraScrollableControl4.Size = New System.Drawing.Size(245, 115)
			Me.XtraScrollableControl4.TabIndex = 0
			'
			'chkSMS
			'
			Me.chkSMS.Location = New System.Drawing.Point(3, 78)
			Me.chkSMS.Name = "chkSMS"
			Me.chkSMS.Properties.Caption = "SMS-Nachrichten"
			Me.chkSMS.Size = New System.Drawing.Size(231, 19)
			Me.chkSMS.TabIndex = 210
			'
			'chkMailed
			'
			Me.chkMailed.Location = New System.Drawing.Point(3, 53)
			Me.chkMailed.Name = "chkMailed"
			Me.chkMailed.Properties.Caption = "Gemailt"
			Me.chkMailed.Size = New System.Drawing.Size(231, 19)
			Me.chkMailed.TabIndex = 209
			'
			'chkOffered
			'
			Me.chkOffered.Location = New System.Drawing.Point(3, 32)
			Me.chkOffered.Name = "chkOffered"
			Me.chkOffered.Properties.Caption = "Offeriert"
			Me.chkOffered.Size = New System.Drawing.Size(231, 19)
			Me.chkOffered.TabIndex = 208
			'
			'chkTelephone
			'
			Me.chkTelephone.Location = New System.Drawing.Point(3, 7)
			Me.chkTelephone.Name = "chkTelephone"
			Me.chkTelephone.Properties.Caption = "Telefoniert"
			Me.chkTelephone.Size = New System.Drawing.Size(231, 19)
			Me.chkTelephone.TabIndex = 207
			'
			'gridContactData
			'
			Me.gridContactData.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.gridContactData.Location = New System.Drawing.Point(2, 21)
			Me.gridContactData.MainView = Me.gridViewContactData
			Me.gridContactData.Name = "gridContactData"
			Me.gridContactData.Size = New System.Drawing.Size(732, 136)
			Me.gridContactData.TabIndex = 1
			Me.gridContactData.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gridViewContactData})
			'
			'gridViewContactData
			'
			Me.gridViewContactData.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			Me.gridViewContactData.GridControl = Me.gridContactData
			Me.gridViewContactData.Name = "gridViewContactData"
			Me.gridViewContactData.OptionsBehavior.Editable = False
			Me.gridViewContactData.OptionsSelection.EnableAppearanceFocusedCell = False
			Me.gridViewContactData.OptionsView.ShowGroupPanel = False
			Me.gridViewContactData.OptionsView.ShowIndicator = False
			'
			'ucContactData
			'
			Me.Appearance.BackColor = System.Drawing.Color.Transparent
			Me.Appearance.Options.UseBackColor = True
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.Controls.Add(Me.grpkontaktdaten)
			Me.Name = "ucContactData"
			Me.Size = New System.Drawing.Size(736, 321)
			CType(Me.grpkontaktdaten, System.ComponentModel.ISupportInitialize).EndInit()
			Me.grpkontaktdaten.ResumeLayout(False)
			Me.grpInfo.ResumeLayout(False)
			Me.XtraScrollableControl2.ResumeLayout(False)
			Me.XtraScrollableControl2.PerformLayout()
			Me.grpjahre.ResumeLayout(False)
			Me.XtraScrollableControl3.ResumeLayout(False)
			CType(Me.lstYears, System.ComponentModel.ISupportInitialize).EndInit()
			Me.grpeintrag.ResumeLayout(False)
			Me.XtraScrollableControl4.ResumeLayout(False)
			CType(Me.chkSMS.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.chkMailed.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.chkOffered.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.chkTelephone.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gridContactData, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gridViewContactData, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub
		Friend WithEvents grpkontaktdaten As DevExpress.XtraEditors.GroupControl
    Friend WithEvents btnAddContact As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents grpInfo As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents XtraScrollableControl2 As DevExpress.XtraEditors.XtraScrollableControl
    Friend WithEvents lblanzahl As System.Windows.Forms.Label
    Friend WithEvents lblLastInfoValue As System.Windows.Forms.Label
    Friend WithEvents lblNumberOfEntriesValue As System.Windows.Forms.Label
    Friend WithEvents lblFirstInfoValue As System.Windows.Forms.Label
    Friend WithEvents lblinfoneu As System.Windows.Forms.Label
    Friend WithEvents lblinfoalt As System.Windows.Forms.Label
    Friend WithEvents grpjahre As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents XtraScrollableControl3 As DevExpress.XtraEditors.XtraScrollableControl
    Friend WithEvents lstYears As DevExpress.XtraEditors.CheckedListBoxControl
    Friend WithEvents grpeintrag As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents XtraScrollableControl4 As DevExpress.XtraEditors.XtraScrollableControl
    Friend WithEvents chkSMS As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents chkMailed As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents chkOffered As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents chkTelephone As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents gridContactData As DevExpress.XtraGrid.GridControl
    Friend WithEvents gridViewContactData As DevExpress.XtraGrid.Views.Grid.GridView

  End Class

End Namespace
