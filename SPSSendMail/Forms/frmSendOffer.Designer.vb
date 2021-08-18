<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSendOffer
  Inherits DevExpress.XtraEditors.XtraForm

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSendOffer))
		Me.chkOffblatt = New DevExpress.XtraEditors.CheckEdit()
		Me.chkOffPBlatt = New DevExpress.XtraEditors.CheckEdit()
		Me.chkOffMABlatt = New DevExpress.XtraEditors.CheckEdit()
		Me.cboFromField = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.lblAbsender = New System.Windows.Forms.Label()
		Me.lblVorlage = New System.Windows.Forms.Label()
		Me.cboTemplate = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.cmdClose = New DevExpress.XtraEditors.SimpleButton()
		Me.cboToField = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.lblEmpfaenger = New System.Windows.Forms.Label()
		Me.Label4 = New System.Windows.Forms.Label()
		Me.Label5 = New System.Windows.Forms.Label()
		Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
		Me.AlertControl1 = New DevExpress.XtraBars.Alerter.AlertControl()
		Me.grpAnhaenge = New DevComponents.DotNetBar.Controls.GroupPanel()
		Me.grpOptionen = New DevComponents.DotNetBar.Controls.GroupPanel()
		Me.GroupBox1 = New DevExpress.XtraEditors.PanelControl()
		Me.lblHeaderNormal = New System.Windows.Forms.Label()
		Me.btnClose = New DevExpress.XtraEditors.SimpleButton()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.lblHeaderFett = New System.Windows.Forms.Label()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager()
		Me.Bar4 = New DevExpress.XtraBars.Bar()
		Me.bbiWait = New DevExpress.XtraBars.BarEditItem()
		Me.RepositoryItemMarqueeProgressBar1 = New DevExpress.XtraEditors.Repository.RepositoryItemMarqueeProgressBar()
		Me.bsiInfo = New DevExpress.XtraBars.BarStaticItem()
		Me.bbiSend = New DevExpress.XtraBars.BarButtonItem()
		Me.bbiSendAsTest = New DevExpress.XtraBars.BarLargeButtonItem()
		Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
		Me.BarStaticItem1 = New DevExpress.XtraBars.BarStaticItem()
		CType(Me.chkOffblatt.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkOffPBlatt.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkOffMABlatt.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.cboFromField.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.cboTemplate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.cboToField.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grpAnhaenge.SuspendLayout()
		Me.grpOptionen.SuspendLayout()
		CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupBox1.SuspendLayout()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.RepositoryItemMarqueeProgressBar1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'chkOffblatt
		'
		Me.chkOffblatt.Location = New System.Drawing.Point(20, 8)
		Me.chkOffblatt.Name = "chkOffblatt"
		Me.chkOffblatt.Properties.AllowFocused = False
		Me.chkOffblatt.Properties.Caption = "Mit Offertenblatt"
		Me.chkOffblatt.Size = New System.Drawing.Size(107, 19)
		Me.chkOffblatt.TabIndex = 0
		'
		'chkOffPBlatt
		'
		Me.chkOffPBlatt.Location = New System.Drawing.Point(20, 33)
		Me.chkOffPBlatt.Name = "chkOffPBlatt"
		Me.chkOffPBlatt.Properties.AllowFocused = False
		Me.chkOffPBlatt.Properties.Caption = "Mit Präsentationsblätter"
		Me.chkOffPBlatt.Size = New System.Drawing.Size(141, 19)
		Me.chkOffPBlatt.TabIndex = 1
		'
		'chkOffMABlatt
		'
		Me.chkOffMABlatt.Location = New System.Drawing.Point(20, 58)
		Me.chkOffMABlatt.Name = "chkOffMABlatt"
		Me.chkOffMABlatt.Properties.AllowFocused = False
		Me.chkOffMABlatt.Properties.Caption = "Mit Kandidaten-Unterlagen"
		Me.chkOffMABlatt.Size = New System.Drawing.Size(154, 19)
		Me.chkOffMABlatt.TabIndex = 2
		'
		'cboFromField
		'
		Me.cboFromField.Location = New System.Drawing.Point(98, 16)
		Me.cboFromField.Name = "cboFromField"
		Me.cboFromField.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.DropDown)})
		Me.cboFromField.Size = New System.Drawing.Size(294, 20)
		Me.cboFromField.TabIndex = 4
		'
		'lblAbsender
		'
		Me.lblAbsender.AutoSize = True
		Me.lblAbsender.Location = New System.Drawing.Point(17, 20)
		Me.lblAbsender.Name = "lblAbsender"
		Me.lblAbsender.Size = New System.Drawing.Size(53, 13)
		Me.lblAbsender.TabIndex = 5
		Me.lblAbsender.Text = "Absender"
		'
		'lblVorlage
		'
		Me.lblVorlage.AutoSize = True
		Me.lblVorlage.Location = New System.Drawing.Point(17, 46)
		Me.lblVorlage.Name = "lblVorlage"
		Me.lblVorlage.Size = New System.Drawing.Size(75, 13)
		Me.lblVorlage.TabIndex = 6
		Me.lblVorlage.Text = "E-Mail-Vorlage"
		'
		'cboTemplate
		'
		Me.cboTemplate.Location = New System.Drawing.Point(98, 42)
		Me.cboTemplate.Name = "cboTemplate"
		Me.cboTemplate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.DropDown)})
		Me.cboTemplate.Size = New System.Drawing.Size(294, 20)
		Me.cboTemplate.TabIndex = 7
		'
		'cmdClose
		'
		Me.cmdClose.Location = New System.Drawing.Point(471, 12)
		Me.cmdClose.Name = "cmdClose"
		Me.cmdClose.Size = New System.Drawing.Size(89, 24)
		Me.cmdClose.TabIndex = 31
		Me.cmdClose.Text = "Schliessen"
		'
		'cboToField
		'
		Me.cboToField.Location = New System.Drawing.Point(98, 87)
		Me.cboToField.Name = "cboToField"
		Me.cboToField.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.DropDown)})
		Me.cboToField.Size = New System.Drawing.Size(294, 20)
		Me.cboToField.TabIndex = 256
		'
		'lblEmpfaenger
		'
		Me.lblEmpfaenger.AutoSize = True
		Me.lblEmpfaenger.Location = New System.Drawing.Point(17, 91)
		Me.lblEmpfaenger.Name = "lblEmpfaenger"
		Me.lblEmpfaenger.Size = New System.Drawing.Size(59, 13)
		Me.lblEmpfaenger.TabIndex = 257
		Me.lblEmpfaenger.Text = "Empfänger"
		'
		'Label4
		'
		Me.Label4.AutoSize = True
		Me.Label4.BackColor = System.Drawing.Color.White
		Me.Label4.Location = New System.Drawing.Point(-230, 39)
		Me.Label4.Name = "Label4"
		Me.Label4.Size = New System.Drawing.Size(225, 13)
		Me.Label4.TabIndex = 261
		Me.Label4.Text = "Wählen Sie Ihre gewünschten Kriterien aus..."
		'
		'Label5
		'
		Me.Label5.AutoSize = True
		Me.Label5.BackColor = System.Drawing.Color.White
		Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label5.Location = New System.Drawing.Point(-252, 17)
		Me.Label5.Name = "Label5"
		Me.Label5.Size = New System.Drawing.Size(194, 13)
		Me.Label5.TabIndex = 260
		Me.Label5.Text = "Einstellungen für Massenversand"
		'
		'BackgroundWorker1
		'
		'
		'grpAnhaenge
		'
		Me.grpAnhaenge.BackColor = System.Drawing.Color.Transparent
		Me.grpAnhaenge.CanvasColor = System.Drawing.Color.Transparent
		Me.grpAnhaenge.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
		Me.grpAnhaenge.Controls.Add(Me.chkOffMABlatt)
		Me.grpAnhaenge.Controls.Add(Me.chkOffblatt)
		Me.grpAnhaenge.Controls.Add(Me.chkOffPBlatt)
		Me.grpAnhaenge.Location = New System.Drawing.Point(36, 110)
		Me.grpAnhaenge.Name = "grpAnhaenge"
		Me.grpAnhaenge.Size = New System.Drawing.Size(628, 129)
		'
		'
		'
		Me.grpAnhaenge.Style.BackColor = System.Drawing.Color.Transparent
		Me.grpAnhaenge.Style.BackColor2 = System.Drawing.Color.Transparent
		Me.grpAnhaenge.Style.BackColorGradientAngle = 90
		Me.grpAnhaenge.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.grpAnhaenge.Style.BorderBottomWidth = 1
		Me.grpAnhaenge.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
		Me.grpAnhaenge.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.grpAnhaenge.Style.BorderLeftWidth = 1
		Me.grpAnhaenge.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.grpAnhaenge.Style.BorderRightWidth = 1
		Me.grpAnhaenge.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.grpAnhaenge.Style.BorderTopWidth = 1
		Me.grpAnhaenge.Style.CornerDiameter = 4
		Me.grpAnhaenge.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
		Me.grpAnhaenge.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
		Me.grpAnhaenge.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
		Me.grpAnhaenge.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
		'
		'
		'
		Me.grpAnhaenge.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
		'
		'
		'
		Me.grpAnhaenge.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
		Me.grpAnhaenge.TabIndex = 266
		Me.grpAnhaenge.Text = "E-Mail Anhänge"
		'
		'grpOptionen
		'
		Me.grpOptionen.BackColor = System.Drawing.Color.Transparent
		Me.grpOptionen.CanvasColor = System.Drawing.Color.Transparent
		Me.grpOptionen.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
		Me.grpOptionen.Controls.Add(Me.cboFromField)
		Me.grpOptionen.Controls.Add(Me.lblAbsender)
		Me.grpOptionen.Controls.Add(Me.cboTemplate)
		Me.grpOptionen.Controls.Add(Me.lblVorlage)
		Me.grpOptionen.Controls.Add(Me.cboToField)
		Me.grpOptionen.Controls.Add(Me.lblEmpfaenger)
		Me.grpOptionen.Location = New System.Drawing.Point(36, 256)
		Me.grpOptionen.Name = "grpOptionen"
		Me.grpOptionen.Size = New System.Drawing.Size(628, 161)
		'
		'
		'
		Me.grpOptionen.Style.BackColor = System.Drawing.Color.Transparent
		Me.grpOptionen.Style.BackColor2 = System.Drawing.Color.Transparent
		Me.grpOptionen.Style.BackColorGradientAngle = 90
		Me.grpOptionen.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.grpOptionen.Style.BorderBottomWidth = 1
		Me.grpOptionen.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
		Me.grpOptionen.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.grpOptionen.Style.BorderLeftWidth = 1
		Me.grpOptionen.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.grpOptionen.Style.BorderRightWidth = 1
		Me.grpOptionen.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.grpOptionen.Style.BorderTopWidth = 1
		Me.grpOptionen.Style.CornerDiameter = 4
		Me.grpOptionen.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
		Me.grpOptionen.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
		Me.grpOptionen.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
		Me.grpOptionen.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
		'
		'
		'
		Me.grpOptionen.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
		'
		'
		'
		Me.grpOptionen.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
		Me.grpOptionen.TabIndex = 267
		Me.grpOptionen.Text = "Optionen"
		'
		'GroupBox1
		'
		Me.GroupBox1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003
		Me.GroupBox1.Controls.Add(Me.lblHeaderNormal)
		Me.GroupBox1.Controls.Add(Me.btnClose)
		Me.GroupBox1.Controls.Add(Me.Label2)
		Me.GroupBox1.Controls.Add(Me.lblHeaderFett)
		Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
		Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(706, 77)
		Me.GroupBox1.TabIndex = 300
		'
		'lblHeaderNormal
		'
		Me.lblHeaderNormal.Location = New System.Drawing.Point(110, 38)
		Me.lblHeaderNormal.Name = "lblHeaderNormal"
		Me.lblHeaderNormal.Size = New System.Drawing.Size(299, 16)
		Me.lblHeaderNormal.TabIndex = 1
		Me.lblHeaderNormal.Text = "Geben Sie bitte Ihre gewünschten Kriterien ein."
		'
		'btnClose
		'
		Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnClose.Location = New System.Drawing.Point(564, 21)
		Me.btnClose.Name = "btnClose"
		Me.btnClose.Size = New System.Drawing.Size(100, 25)
		Me.btnClose.TabIndex = 204
		Me.btnClose.Text = "Schliessen"
		'
		'Label2
		'
		Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label2.ForeColor = System.Drawing.SystemColors.HotTrack
		Me.Label2.Image = CType(resources.GetObject("Label2.Image"), System.Drawing.Image)
		Me.Label2.Location = New System.Drawing.Point(5, 8)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(83, 65)
		Me.Label2.TabIndex = 1000
		'
		'lblHeaderFett
		'
		Me.lblHeaderFett.AutoSize = True
		Me.lblHeaderFett.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblHeaderFett.Location = New System.Drawing.Point(94, 21)
		Me.lblHeaderFett.Name = "lblHeaderFett"
		Me.lblHeaderFett.Size = New System.Drawing.Size(174, 13)
		Me.lblHeaderFett.TabIndex = 0
		Me.lblHeaderFett.Text = "Einstellungen für Mailversand"
		'
		'BarManager1
		'
		Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar4})
		Me.BarManager1.DockControls.Add(Me.barDockControlTop)
		Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
		Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
		Me.BarManager1.DockControls.Add(Me.barDockControlRight)
		Me.BarManager1.Form = Me
		Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiInfo, Me.bbiSend, Me.BarStaticItem1, Me.bbiSendAsTest, Me.bbiWait})
		Me.BarManager1.MaxItemId = 13
		Me.BarManager1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemMarqueeProgressBar1})
		Me.BarManager1.StatusBar = Me.Bar4
		'
		'Bar4
		'
		Me.Bar4.BarName = "Statusleiste"
		Me.Bar4.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
		Me.Bar4.DockCol = 0
		Me.Bar4.DockRow = 0
		Me.Bar4.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
		Me.Bar4.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bbiWait), New DevExpress.XtraBars.LinkPersistInfo(Me.bsiInfo), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiSend), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiSendAsTest, True)})
		Me.Bar4.OptionsBar.AllowQuickCustomization = False
		Me.Bar4.OptionsBar.DrawDragBorder = False
		Me.Bar4.OptionsBar.UseWholeRow = True
		Me.Bar4.Text = "Statusleiste"
		'
		'bbiWait
		'
		Me.bbiWait.Caption = "BarEditItem1"
		Me.bbiWait.Edit = Me.RepositoryItemMarqueeProgressBar1
		Me.bbiWait.Id = 12
		Me.bbiWait.Name = "bbiWait"
		'
		'RepositoryItemMarqueeProgressBar1
		'
		Me.RepositoryItemMarqueeProgressBar1.Name = "RepositoryItemMarqueeProgressBar1"
		'
		'bsiInfo
		'
		Me.bsiInfo.AutoSize = DevExpress.XtraBars.BarStaticItemSize.Spring
		Me.bsiInfo.Caption = "Bereit"
		Me.bsiInfo.Id = 0
		Me.bsiInfo.Name = "bsiInfo"
		Me.bsiInfo.TextAlignment = System.Drawing.StringAlignment.Near
		Me.bsiInfo.Width = 32
		'
		'bbiSend
		'
		Me.bbiSend.Caption = "Nachricht senden"
		Me.bbiSend.Glyph = CType(resources.GetObject("bbiSend.Glyph"), System.Drawing.Image)
		Me.bbiSend.Id = 1
		Me.bbiSend.Name = "bbiSend"
		Me.bbiSend.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'bbiSendAsTest
		'
		Me.bbiSendAsTest.Caption = "Test-Nachricht senden"
		Me.bbiSendAsTest.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Right
		Me.bbiSendAsTest.Glyph = CType(resources.GetObject("bbiSendAsTest.Glyph"), System.Drawing.Image)
		Me.bbiSendAsTest.Id = 4
		Me.bbiSendAsTest.Name = "bbiSendAsTest"
		Me.bbiSendAsTest.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
		'
		'barDockControlTop
		'
		Me.barDockControlTop.CausesValidation = False
		Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
		Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlTop.Size = New System.Drawing.Size(706, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 471)
		Me.barDockControlBottom.Size = New System.Drawing.Size(706, 27)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 471)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(706, 0)
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 471)
		'
		'BarStaticItem1
		'
		Me.BarStaticItem1.Caption = " "
		Me.BarStaticItem1.Id = 5
		Me.BarStaticItem1.Name = "BarStaticItem1"
		Me.BarStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near
		'
		'frmSendOffer
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(706, 498)
		Me.Controls.Add(Me.GroupBox1)
		Me.Controls.Add(Me.grpOptionen)
		Me.Controls.Add(Me.grpAnhaenge)
		Me.Controls.Add(Me.Label4)
		Me.Controls.Add(Me.Label5)
		Me.Controls.Add(Me.cmdClose)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximumSize = New System.Drawing.Size(722, 536)
		Me.MinimumSize = New System.Drawing.Size(722, 536)
		Me.Name = "frmSendOffer"
		Me.Text = "Offerte per E-Mail senden"
		CType(Me.chkOffblatt.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkOffPBlatt.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkOffMABlatt.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.cboFromField.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.cboTemplate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.cboToField.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.grpAnhaenge.ResumeLayout(False)
		Me.grpOptionen.ResumeLayout(False)
		Me.grpOptionen.PerformLayout()
		CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupBox1.ResumeLayout(False)
		Me.GroupBox1.PerformLayout()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.RepositoryItemMarqueeProgressBar1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents chkOffblatt As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chkOffPBlatt As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chkOffMABlatt As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents cboFromField As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents lblAbsender As System.Windows.Forms.Label
	Friend WithEvents lblVorlage As System.Windows.Forms.Label
	Friend WithEvents cboTemplate As DevExpress.XtraEditors.ComboBoxEdit
	'  System.Windows.Forms.Button
	Friend WithEvents cmdClose As DevExpress.XtraEditors.SimpleButton	'  System.Windows.Forms.Button
	Friend WithEvents cboToField As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents lblEmpfaenger As System.Windows.Forms.Label
	Friend WithEvents Label4 As System.Windows.Forms.Label
	Friend WithEvents Label5 As System.Windows.Forms.Label
	Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
	'  System.Windows.Forms.Button
	Friend WithEvents AlertControl1 As DevExpress.XtraBars.Alerter.AlertControl
	Friend WithEvents grpAnhaenge As DevComponents.DotNetBar.Controls.GroupPanel
	Friend WithEvents grpOptionen As DevComponents.DotNetBar.Controls.GroupPanel
	Friend WithEvents GroupBox1 As DevExpress.XtraEditors.PanelControl
	Friend WithEvents lblHeaderNormal As System.Windows.Forms.Label
	Friend WithEvents btnClose As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents Label2 As System.Windows.Forms.Label
	Friend WithEvents lblHeaderFett As System.Windows.Forms.Label
	Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
	Friend WithEvents Bar4 As DevExpress.XtraBars.Bar
	Friend WithEvents bsiInfo As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents bbiSend As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbiSendAsTest As DevExpress.XtraBars.BarLargeButtonItem
	Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
	Friend WithEvents BarStaticItem1 As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents bbiWait As DevExpress.XtraBars.BarEditItem
	Friend WithEvents RepositoryItemMarqueeProgressBar1 As DevExpress.XtraEditors.Repository.RepositoryItemMarqueeProgressBar
End Class
