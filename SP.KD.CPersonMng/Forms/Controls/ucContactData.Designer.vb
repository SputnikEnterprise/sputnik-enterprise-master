
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
      Me.grpausblenden = New DevComponents.DotNetBar.Controls.GroupPanel()
      Me.XtraScrollableControl3 = New DevExpress.XtraEditors.XtraScrollableControl()
      Me.chkSMS = New DevExpress.XtraEditors.CheckEdit()
      Me.chkMailed = New DevExpress.XtraEditors.CheckEdit()
      Me.chkOffered = New DevExpress.XtraEditors.CheckEdit()
      Me.chkTelephone = New DevExpress.XtraEditors.CheckEdit()
      Me.lstYears = New DevExpress.XtraEditors.CheckedListBoxControl()
      Me.grpKontakt = New DevExpress.XtraEditors.GroupControl()
      Me.btnAddContact = New DevExpress.XtraEditors.SimpleButton()
      Me.grpInfo = New DevComponents.DotNetBar.Controls.GroupPanel()
      Me.XtraScrollableControl2 = New DevExpress.XtraEditors.XtraScrollableControl()
      Me.lblAnzahl = New System.Windows.Forms.Label()
      Me.lblLastInfoValue = New System.Windows.Forms.Label()
      Me.lblNumberOfEntriesValue = New System.Windows.Forms.Label()
      Me.lblFirstInfoValue = New System.Windows.Forms.Label()
      Me.lblinfoneu = New System.Windows.Forms.Label()
      Me.lblinfoalt = New System.Windows.Forms.Label()
      Me.grpjahre = New DevComponents.DotNetBar.Controls.GroupPanel()
      Me.XtraScrollableControl1 = New DevExpress.XtraEditors.XtraScrollableControl()
      Me.gridContactData = New DevExpress.XtraGrid.GridControl()
      Me.gridViewContactData = New DevExpress.XtraGrid.Views.Grid.GridView()
      Me.grpausblenden.SuspendLayout()
      Me.XtraScrollableControl3.SuspendLayout()
      CType(Me.chkSMS.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.chkMailed.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.chkOffered.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.chkTelephone.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.lstYears, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.grpKontakt, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.grpKontakt.SuspendLayout()
      Me.grpInfo.SuspendLayout()
      Me.XtraScrollableControl2.SuspendLayout()
      Me.grpjahre.SuspendLayout()
      Me.XtraScrollableControl1.SuspendLayout()
      CType(Me.gridContactData, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.gridViewContactData, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'grpausblenden
      '
      Me.grpausblenden.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
      Me.grpausblenden.BackColor = System.Drawing.Color.Transparent
      Me.grpausblenden.CanvasColor = System.Drawing.SystemColors.Control
      Me.grpausblenden.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
      Me.grpausblenden.Controls.Add(Me.XtraScrollableControl3)
      Me.grpausblenden.Location = New System.Drawing.Point(384, 482)
      Me.grpausblenden.Name = "grpausblenden"
      Me.grpausblenden.Size = New System.Drawing.Size(251, 137)
      '
      '
      '
      Me.grpausblenden.Style.BackColorGradientAngle = 90
      Me.grpausblenden.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
      Me.grpausblenden.Style.BorderBottomWidth = 1
      Me.grpausblenden.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
      Me.grpausblenden.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
      Me.grpausblenden.Style.BorderLeftWidth = 1
      Me.grpausblenden.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
      Me.grpausblenden.Style.BorderRightWidth = 1
      Me.grpausblenden.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
      Me.grpausblenden.Style.BorderTopWidth = 1
      Me.grpausblenden.Style.CornerDiameter = 4
      Me.grpausblenden.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
      Me.grpausblenden.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
      Me.grpausblenden.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
      Me.grpausblenden.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
      Me.grpausblenden.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
      '
      '
      '
      Me.grpausblenden.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
      '
      '
      '
      Me.grpausblenden.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
      Me.grpausblenden.TabIndex = 205
      Me.grpausblenden.Text = "Einträge ausblenden für"
      '
      'XtraScrollableControl3
      '
      Me.XtraScrollableControl3.Anchor = System.Windows.Forms.AnchorStyles.Top
      Me.XtraScrollableControl3.Controls.Add(Me.chkSMS)
      Me.XtraScrollableControl3.Controls.Add(Me.chkMailed)
      Me.XtraScrollableControl3.Controls.Add(Me.chkOffered)
      Me.XtraScrollableControl3.Controls.Add(Me.chkTelephone)
      Me.XtraScrollableControl3.Location = New System.Drawing.Point(0, 0)
      Me.XtraScrollableControl3.Name = "XtraScrollableControl3"
      Me.XtraScrollableControl3.Size = New System.Drawing.Size(245, 115)
      Me.XtraScrollableControl3.TabIndex = 0
      '
      'chkSMS
      '
      Me.chkSMS.Location = New System.Drawing.Point(3, 82)
      Me.chkSMS.Name = "chkSMS"
      Me.chkSMS.Properties.Caption = "SMS-Nachrichten"
      Me.chkSMS.Size = New System.Drawing.Size(144, 19)
      Me.chkSMS.TabIndex = 210
      '
      'chkMailed
      '
      Me.chkMailed.Location = New System.Drawing.Point(3, 57)
      Me.chkMailed.Name = "chkMailed"
      Me.chkMailed.Properties.Caption = "Gemailt"
      Me.chkMailed.Size = New System.Drawing.Size(144, 19)
      Me.chkMailed.TabIndex = 209
      '
      'chkOffered
      '
      Me.chkOffered.Location = New System.Drawing.Point(3, 32)
      Me.chkOffered.Name = "chkOffered"
      Me.chkOffered.Properties.Caption = "Offeriert"
      Me.chkOffered.Size = New System.Drawing.Size(144, 19)
      Me.chkOffered.TabIndex = 208
      '
      'chkTelephone
      '
      Me.chkTelephone.Location = New System.Drawing.Point(3, 7)
      Me.chkTelephone.Name = "chkTelephone"
      Me.chkTelephone.Properties.Caption = "Telefoniert"
      Me.chkTelephone.Size = New System.Drawing.Size(144, 19)
      Me.chkTelephone.TabIndex = 207
      '
      'lstYears
      '
      Me.lstYears.CheckOnClick = True
      Me.lstYears.Location = New System.Drawing.Point(3, 5)
      Me.lstYears.Name = "lstYears"
      Me.lstYears.Size = New System.Drawing.Size(160, 107)
      Me.lstYears.TabIndex = 211
      '
      'grpKontakt
      '
      Me.grpKontakt.Appearance.BackColor = System.Drawing.Color.Transparent
      Me.grpKontakt.Appearance.Options.UseBackColor = True
      Me.grpKontakt.AppearanceCaption.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
      Me.grpKontakt.AppearanceCaption.Options.UseFont = True
      Me.grpKontakt.Controls.Add(Me.btnAddContact)
      Me.grpKontakt.Controls.Add(Me.grpInfo)
      Me.grpKontakt.Controls.Add(Me.grpjahre)
      Me.grpKontakt.Controls.Add(Me.grpausblenden)
      Me.grpKontakt.Controls.Add(Me.gridContactData)
      Me.grpKontakt.Dock = System.Windows.Forms.DockStyle.Fill
      Me.grpKontakt.Location = New System.Drawing.Point(5, 5)
      Me.grpKontakt.Name = "grpKontakt"
      Me.grpKontakt.Size = New System.Drawing.Size(829, 630)
      Me.grpKontakt.TabIndex = 166
      Me.grpKontakt.Text = "Kontaktdaten"
      '
      'btnAddContact
      '
      Me.btnAddContact.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
      Me.btnAddContact.Image = CType(resources.GetObject("btnAddContact.Image"), System.Drawing.Image)
      Me.btnAddContact.Location = New System.Drawing.Point(797, 3)
      Me.btnAddContact.Name = "btnAddContact"
      Me.btnAddContact.Size = New System.Drawing.Size(27, 15)
      Me.btnAddContact.TabIndex = 272
      '
      'grpInfo
      '
      Me.grpInfo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
      Me.grpInfo.BackColor = System.Drawing.Color.Transparent
      Me.grpInfo.CanvasColor = System.Drawing.SystemColors.Control
      Me.grpInfo.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
      Me.grpInfo.Controls.Add(Me.XtraScrollableControl2)
      Me.grpInfo.Location = New System.Drawing.Point(5, 482)
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
      Me.XtraScrollableControl2.Controls.Add(Me.lblAnzahl)
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
      'lblAnzahl
      '
      Me.lblAnzahl.AutoSize = True
      Me.lblAnzahl.Location = New System.Drawing.Point(3, 9)
      Me.lblAnzahl.Name = "lblAnzahl"
      Me.lblAnzahl.Size = New System.Drawing.Size(82, 13)
      Me.lblAnzahl.TabIndex = 207
      Me.lblAnzahl.Text = "Anzahl Einträge"
      '
      'lblLastInfoValue
      '
      Me.lblLastInfoValue.AutoSize = True
      Me.lblLastInfoValue.ForeColor = System.Drawing.Color.Blue
      Me.lblLastInfoValue.Location = New System.Drawing.Point(167, 55)
      Me.lblLastInfoValue.Name = "lblLastInfoValue"
      Me.lblLastInfoValue.Size = New System.Drawing.Size(15, 13)
      Me.lblLastInfoValue.TabIndex = 212
      Me.lblLastInfoValue.Text = "[]"
      '
      'lblNumberOfEntriesValue
      '
      Me.lblNumberOfEntriesValue.AutoSize = True
      Me.lblNumberOfEntriesValue.Location = New System.Drawing.Point(167, 9)
      Me.lblNumberOfEntriesValue.Name = "lblNumberOfEntriesValue"
      Me.lblNumberOfEntriesValue.Size = New System.Drawing.Size(15, 13)
      Me.lblNumberOfEntriesValue.TabIndex = 208
      Me.lblNumberOfEntriesValue.Text = "[]"
      '
      'lblFirstInfoValue
      '
      Me.lblFirstInfoValue.AutoSize = True
      Me.lblFirstInfoValue.ForeColor = System.Drawing.Color.Blue
      Me.lblFirstInfoValue.Location = New System.Drawing.Point(167, 34)
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
      Me.grpjahre.Controls.Add(Me.XtraScrollableControl1)
      Me.grpjahre.Location = New System.Drawing.Point(651, 482)
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
      Me.grpjahre.TabIndex = 206
      Me.grpjahre.Text = "Gesuchte Jahre"
      '
      'XtraScrollableControl1
      '
      Me.XtraScrollableControl1.Controls.Add(Me.lstYears)
      Me.XtraScrollableControl1.Dock = System.Windows.Forms.DockStyle.Fill
      Me.XtraScrollableControl1.Location = New System.Drawing.Point(0, 0)
      Me.XtraScrollableControl1.Name = "XtraScrollableControl1"
      Me.XtraScrollableControl1.Size = New System.Drawing.Size(167, 115)
      Me.XtraScrollableControl1.TabIndex = 0
      '
      'gridContactData
      '
      Me.gridContactData.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
              Or System.Windows.Forms.AnchorStyles.Left) _
              Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
      Me.gridContactData.Location = New System.Drawing.Point(2, 21)
      Me.gridContactData.MainView = Me.gridViewContactData
      Me.gridContactData.Name = "gridContactData"
      Me.gridContactData.Size = New System.Drawing.Size(825, 444)
      Me.gridContactData.TabIndex = 2
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
      '
      'ucContactData
      '
      Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
      Me.Controls.Add(Me.grpKontakt)
      Me.Name = "ucContactData"
      Me.Padding = New System.Windows.Forms.Padding(5)
      Me.Size = New System.Drawing.Size(839, 640)
      Me.grpausblenden.ResumeLayout(False)
      Me.XtraScrollableControl3.ResumeLayout(False)
      CType(Me.chkSMS.Properties, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.chkMailed.Properties, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.chkOffered.Properties, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.chkTelephone.Properties, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.lstYears, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.grpKontakt, System.ComponentModel.ISupportInitialize).EndInit()
      Me.grpKontakt.ResumeLayout(False)
      Me.grpInfo.ResumeLayout(False)
      Me.XtraScrollableControl2.ResumeLayout(False)
      Me.XtraScrollableControl2.PerformLayout()
      Me.grpjahre.ResumeLayout(False)
      Me.XtraScrollableControl1.ResumeLayout(False)
      CType(Me.gridContactData, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.gridViewContactData, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpausblenden As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents XtraScrollableControl3 As DevExpress.XtraEditors.XtraScrollableControl
    Friend WithEvents chkSMS As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents chkMailed As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents chkOffered As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents chkTelephone As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents grpKontakt As DevExpress.XtraEditors.GroupControl
    Friend WithEvents gridContactData As DevExpress.XtraGrid.GridControl
    Friend WithEvents gridViewContactData As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents lstYears As DevExpress.XtraEditors.CheckedListBoxControl
    Friend WithEvents grpjahre As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents XtraScrollableControl1 As DevExpress.XtraEditors.XtraScrollableControl
    Friend WithEvents lblLastInfoValue As System.Windows.Forms.Label
    Friend WithEvents lblFirstInfoValue As System.Windows.Forms.Label
    Friend WithEvents lblinfoalt As System.Windows.Forms.Label
    Friend WithEvents lblinfoneu As System.Windows.Forms.Label
    Friend WithEvents lblNumberOfEntriesValue As System.Windows.Forms.Label
    Friend WithEvents lblAnzahl As System.Windows.Forms.Label
    Friend WithEvents grpInfo As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents XtraScrollableControl2 As DevExpress.XtraEditors.XtraScrollableControl
    Friend WithEvents btnAddContact As DevExpress.XtraEditors.SimpleButton

    End Class
End Namespace