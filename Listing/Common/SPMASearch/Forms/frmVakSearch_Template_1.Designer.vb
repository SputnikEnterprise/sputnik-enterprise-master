<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmVakSearch_Template_1
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmVakSearch_Template_1))
    Me.LibVakNr_1 = New System.Windows.Forms.LinkLabel()
    Me.Chk_HBerufe = New DevExpress.XtraEditors.CheckEdit()
    Me.Chk_SBerufe = New DevExpress.XtraEditors.CheckEdit()
    Me.Chk_Branche = New DevExpress.XtraEditors.CheckEdit()
    Me.Chk_Geschlecht = New DevExpress.XtraEditors.CheckEdit()
    Me.txt_IndSQLQuery = New System.Windows.Forms.TextBox()
    Me.Label3 = New System.Windows.Forms.Label()
    Me.Label50 = New System.Windows.Forms.Label()
    Me.Label45 = New System.Windows.Forms.Label()
    Me.Chk_InES = New DevExpress.XtraEditors.CheckEdit()
    Me.CboSort = New DevExpress.XtraEditors.CheckedComboBoxEdit()
    Me.Cbo_MAKontakt = New DevExpress.XtraEditors.CheckedComboBoxEdit()
    Me.Cbo_MAStatus1 = New DevExpress.XtraEditors.CheckedComboBoxEdit()
    Me.Bar3 = New DevExpress.XtraBars.Bar()
    Me.bsiInfo = New DevExpress.XtraBars.BarStaticItem()
    Me.bbiSearch = New DevExpress.XtraBars.BarButtonItem()
    Me.bbiClear = New DevExpress.XtraBars.BarButtonItem()
    Me.bbiPrint = New DevExpress.XtraBars.BarLargeButtonItem()
    Me.bbiExport = New DevExpress.XtraBars.BarLargeButtonItem()
    Me.BarManager1 = New DevExpress.XtraBars.BarManager()
    Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
    Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
    Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
    Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
    Me.GroupBox1 = New DevExpress.XtraEditors.PanelControl()
    Me.LblTimeValue = New System.Windows.Forms.Label()
    Me.CmdClose = New DevExpress.XtraEditors.SimpleButton()
    Me.LblSetting = New System.Windows.Forms.Label()
    Me.Label2 = New System.Windows.Forms.Label()
    Me.Label4 = New System.Windows.Forms.Label()
    CType(Me.Chk_HBerufe.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.Chk_SBerufe.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.Chk_Branche.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.Chk_Geschlecht.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.Chk_InES.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.CboSort.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.Cbo_MAKontakt.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.Cbo_MAStatus1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.GroupBox1.SuspendLayout()
    Me.SuspendLayout()
    '
    'LibVakNr_1
    '
    Me.LibVakNr_1.AutoSize = True
    Me.LibVakNr_1.Location = New System.Drawing.Point(37, 327)
    Me.LibVakNr_1.Name = "LibVakNr_1"
    Me.LibVakNr_1.Size = New System.Drawing.Size(217, 13)
    Me.LibVakNr_1.TabIndex = 5
    Me.LibVakNr_1.TabStop = True
    Me.LibVakNr_1.Tag = ""
    Me.LibVakNr_1.Text = "Ich möchte noch detailierter suchen --->>>"
    '
    'Chk_HBerufe
    '
    Me.Chk_HBerufe.Location = New System.Drawing.Point(38, 143)
    Me.Chk_HBerufe.Name = "Chk_HBerufe"
    Me.Chk_HBerufe.Properties.Caption = "Ich möchte Kandidaten anhand Qualifikation suchen"
    Me.Chk_HBerufe.Size = New System.Drawing.Size(274, 19)
    Me.Chk_HBerufe.TabIndex = 1
    '
    'Chk_SBerufe
    '
    Me.Chk_SBerufe.Location = New System.Drawing.Point(38, 166)
    Me.Chk_SBerufe.Name = "Chk_SBerufe"
    Me.Chk_SBerufe.Properties.Caption = "Ich möchte Kandidaten anhand Sonstige-Qualifikationen suchen"
    Me.Chk_SBerufe.Size = New System.Drawing.Size(331, 19)
    Me.Chk_SBerufe.TabIndex = 2
    '
    'Chk_Branche
    '
    Me.Chk_Branche.Location = New System.Drawing.Point(38, 189)
    Me.Chk_Branche.Name = "Chk_Branche"
    Me.Chk_Branche.Properties.Caption = "Ich möchte Kandidaten anhand Branchen suchen"
    Me.Chk_Branche.Size = New System.Drawing.Size(260, 19)
    Me.Chk_Branche.TabIndex = 3
    '
    'Chk_Geschlecht
    '
    Me.Chk_Geschlecht.Location = New System.Drawing.Point(38, 212)
    Me.Chk_Geschlecht.Name = "Chk_Geschlecht"
    Me.Chk_Geschlecht.Properties.Caption = "Ich möchte Kandidaten anhand Geschlecht suchen"
    Me.Chk_Geschlecht.Size = New System.Drawing.Size(267, 19)
    Me.Chk_Geschlecht.TabIndex = 4
    '
    'txt_IndSQLQuery
    '
    Me.txt_IndSQLQuery.Location = New System.Drawing.Point(412, 143)
    Me.txt_IndSQLQuery.Multiline = True
    Me.txt_IndSQLQuery.Name = "txt_IndSQLQuery"
    Me.txt_IndSQLQuery.Size = New System.Drawing.Size(436, 56)
    Me.txt_IndSQLQuery.TabIndex = 21
    '
    'Label3
    '
    Me.Label3.AutoSize = True
    Me.Label3.Location = New System.Drawing.Point(35, 103)
    Me.Label3.Name = "Label3"
    Me.Label3.Size = New System.Drawing.Size(77, 13)
    Me.Label3.TabIndex = 22
    Me.Label3.Text = "Sortieren nach"
    '
    'Label50
    '
    Me.Label50.AutoSize = True
    Me.Label50.Location = New System.Drawing.Point(35, 273)
    Me.Label50.Name = "Label50"
    Me.Label50.Size = New System.Drawing.Size(59, 13)
    Me.Label50.TabIndex = 42
    Me.Label50.Text = "MAKontakt"
    '
    'Label45
    '
    Me.Label45.AutoSize = True
    Me.Label45.Location = New System.Drawing.Point(35, 298)
    Me.Label45.Name = "Label45"
    Me.Label45.Size = New System.Drawing.Size(59, 13)
    Me.Label45.TabIndex = 41
    Me.Label45.Text = "MA1Status"
    '
    'Chk_InES
    '
    Me.Chk_InES.EditValue = True
    Me.Chk_InES.Location = New System.Drawing.Point(38, 235)
    Me.Chk_InES.Name = "Chk_InES"
    Me.Chk_InES.Properties.Caption = "Welche heute nicht im Einsatz sind "
    Me.Chk_InES.Size = New System.Drawing.Size(193, 19)
    Me.Chk_InES.TabIndex = 45
    '
    'CboSort
    '
    Me.CboSort.Location = New System.Drawing.Point(123, 100)
    Me.CboSort.Name = "CboSort"
    Me.CboSort.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.CboSort.Size = New System.Drawing.Size(384, 20)
    Me.CboSort.TabIndex = 0
    '
    'Cbo_MAKontakt
    '
    Me.Cbo_MAKontakt.Location = New System.Drawing.Point(123, 269)
    Me.Cbo_MAKontakt.Name = "Cbo_MAKontakt"
    Me.Cbo_MAKontakt.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.Cbo_MAKontakt.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
    Me.Cbo_MAKontakt.Size = New System.Drawing.Size(140, 20)
    Me.Cbo_MAKontakt.TabIndex = 43
    '
    'Cbo_MAStatus1
    '
    Me.Cbo_MAStatus1.Location = New System.Drawing.Point(123, 294)
    Me.Cbo_MAStatus1.Name = "Cbo_MAStatus1"
    Me.Cbo_MAStatus1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.Cbo_MAStatus1.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
    Me.Cbo_MAStatus1.Size = New System.Drawing.Size(140, 20)
    Me.Cbo_MAStatus1.TabIndex = 44
    '
    'Bar3
    '
    Me.Bar3.BarName = "Statusleiste"
    Me.Bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
    Me.Bar3.DockCol = 0
    Me.Bar3.DockRow = 0
    Me.Bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
    Me.Bar3.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiInfo), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiSearch), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiClear), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiPrint, True), New DevExpress.XtraBars.LinkPersistInfo(Me.bbiExport, True)})
    Me.Bar3.OptionsBar.AllowQuickCustomization = False
    Me.Bar3.OptionsBar.DrawDragBorder = False
    Me.Bar3.OptionsBar.UseWholeRow = True
    Me.Bar3.Text = "Statusleiste"
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
    'bbiSearch
    '
    Me.bbiSearch.Caption = "Suchen"
    Me.bbiSearch.Glyph = CType(resources.GetObject("bbiSearch.Glyph"), System.Drawing.Image)
    Me.bbiSearch.Id = 1
    Me.bbiSearch.LargeGlyph = CType(resources.GetObject("bbiSearch.LargeGlyph"), System.Drawing.Image)
    Me.bbiSearch.Name = "bbiSearch"
    Me.bbiSearch.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
    '
    'bbiClear
    '
    Me.bbiClear.Caption = "Felder leeren"
    Me.bbiClear.Glyph = CType(resources.GetObject("bbiClear.Glyph"), System.Drawing.Image)
    Me.bbiClear.Id = 2
    Me.bbiClear.LargeGlyph = CType(resources.GetObject("bbiClear.LargeGlyph"), System.Drawing.Image)
    Me.bbiClear.Name = "bbiClear"
    Me.bbiClear.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
    '
    'bbiPrint
    '
    Me.bbiPrint.Caption = "Drucken"
    Me.bbiPrint.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Right
    Me.bbiPrint.Glyph = CType(resources.GetObject("bbiPrint.Glyph"), System.Drawing.Image)
    Me.bbiPrint.Id = 7
    Me.bbiPrint.Name = "bbiPrint"
    Me.bbiPrint.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
    '
    'bbiExport
    '
    Me.bbiExport.Caption = "Exportieren"
    Me.bbiExport.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Right
    Me.bbiExport.Glyph = CType(resources.GetObject("bbiExport.Glyph"), System.Drawing.Image)
    Me.bbiExport.Id = 8
    Me.bbiExport.Name = "bbiExport"
    Me.bbiExport.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
    '
    'BarManager1
    '
    Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar3})
    Me.BarManager1.DockControls.Add(Me.barDockControlTop)
    Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
    Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
    Me.BarManager1.DockControls.Add(Me.barDockControlRight)
    Me.BarManager1.Form = Me
    Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiInfo, Me.bbiSearch, Me.bbiClear, Me.bbiPrint, Me.bbiExport})
    Me.BarManager1.MaxItemId = 9
    Me.BarManager1.StatusBar = Me.Bar3
    '
    'barDockControlTop
    '
    Me.barDockControlTop.CausesValidation = False
    Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
    Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
    Me.barDockControlTop.Size = New System.Drawing.Size(591, 0)
    '
    'barDockControlBottom
    '
    Me.barDockControlBottom.CausesValidation = False
    Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
    Me.barDockControlBottom.Location = New System.Drawing.Point(0, 364)
    Me.barDockControlBottom.Size = New System.Drawing.Size(591, 27)
    '
    'barDockControlLeft
    '
    Me.barDockControlLeft.CausesValidation = False
    Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
    Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
    Me.barDockControlLeft.Size = New System.Drawing.Size(0, 364)
    '
    'barDockControlRight
    '
    Me.barDockControlRight.CausesValidation = False
    Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
    Me.barDockControlRight.Location = New System.Drawing.Point(591, 0)
    Me.barDockControlRight.Size = New System.Drawing.Size(0, 364)
    '
    'GroupBox1
    '
    Me.GroupBox1.Appearance.BackColor = System.Drawing.Color.White
    Me.GroupBox1.Appearance.Options.UseBackColor = True
    Me.GroupBox1.Controls.Add(Me.LblTimeValue)
    Me.GroupBox1.Controls.Add(Me.CmdClose)
    Me.GroupBox1.Controls.Add(Me.LblSetting)
    Me.GroupBox1.Controls.Add(Me.Label2)
    Me.GroupBox1.Controls.Add(Me.Label4)
    Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
    Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
    Me.GroupBox1.Name = "GroupBox1"
    Me.GroupBox1.Size = New System.Drawing.Size(591, 77)
    Me.GroupBox1.TabIndex = 50
    '
    'LblTimeValue
    '
    Me.LblTimeValue.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.LblTimeValue.BackColor = System.Drawing.Color.Transparent
    Me.LblTimeValue.Location = New System.Drawing.Point(335, 56)
    Me.LblTimeValue.Name = "LblTimeValue"
    Me.LblTimeValue.Size = New System.Drawing.Size(229, 13)
    Me.LblTimeValue.TabIndex = 0
    Me.LblTimeValue.Text = "Zeitangaben..."
    Me.LblTimeValue.TextAlign = System.Drawing.ContentAlignment.TopRight
    Me.LblTimeValue.Visible = False
    '
    'CmdClose
    '
    Me.CmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.CmdClose.Location = New System.Drawing.Point(478, 29)
    Me.CmdClose.Name = "CmdClose"
    Me.CmdClose.Size = New System.Drawing.Size(86, 24)
    Me.CmdClose.TabIndex = 5
    Me.CmdClose.Text = "Schliessen"
    '
    'LblSetting
    '
    Me.LblSetting.BackColor = System.Drawing.Color.Transparent
    Me.LblSetting.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.LblSetting.ForeColor = System.Drawing.SystemColors.HotTrack
    Me.LblSetting.Image = CType(resources.GetObject("LblSetting.Image"), System.Drawing.Image)
    Me.LblSetting.Location = New System.Drawing.Point(12, 9)
    Me.LblSetting.Name = "LblSetting"
    Me.LblSetting.Size = New System.Drawing.Size(71, 63)
    Me.LblSetting.TabIndex = 0
    '
    'Label2
    '
    Me.Label2.AutoSize = True
    Me.Label2.BackColor = System.Drawing.Color.Transparent
    Me.Label2.Location = New System.Drawing.Point(109, 53)
    Me.Label2.Name = "Label2"
    Me.Label2.Size = New System.Drawing.Size(225, 13)
    Me.Label2.TabIndex = 1
    Me.Label2.Text = "Wählen Sie Ihre gewünschten Kriterien aus..."
    '
    'Label4
    '
    Me.Label4.AutoSize = True
    Me.Label4.BackColor = System.Drawing.Color.Transparent
    Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label4.Location = New System.Drawing.Point(91, 29)
    Me.Label4.Name = "Label4"
    Me.Label4.Size = New System.Drawing.Size(333, 13)
    Me.Label4.TabIndex = 0
    Me.Label4.Text = "Vorlage für Suche nach Kandidaten anhand einer Vakanz"
    '
    'frmVakSearch_Template_1
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(591, 391)
    Me.Controls.Add(Me.GroupBox1)
    Me.Controls.Add(Me.Chk_InES)
    Me.Controls.Add(Me.Label50)
    Me.Controls.Add(Me.Label45)
    Me.Controls.Add(Me.Label3)
    Me.Controls.Add(Me.CboSort)
    Me.Controls.Add(Me.txt_IndSQLQuery)
    Me.Controls.Add(Me.Chk_Geschlecht)
    Me.Controls.Add(Me.Chk_Branche)
    Me.Controls.Add(Me.Chk_SBerufe)
    Me.Controls.Add(Me.Chk_HBerufe)
    Me.Controls.Add(Me.LibVakNr_1)
    Me.Controls.Add(Me.Cbo_MAKontakt)
    Me.Controls.Add(Me.Cbo_MAStatus1)
    Me.Controls.Add(Me.barDockControlLeft)
    Me.Controls.Add(Me.barDockControlRight)
    Me.Controls.Add(Me.barDockControlBottom)
    Me.Controls.Add(Me.barDockControlTop)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.MinimumSize = New System.Drawing.Size(607, 429)
    Me.Name = "frmVakSearch_Template_1"
    Me.Text = "Vorlage für Suche nach Kandidaten"
    CType(Me.Chk_HBerufe.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.Chk_SBerufe.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.Chk_Branche.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.Chk_Geschlecht.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.Chk_InES.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.CboSort.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.Cbo_MAKontakt.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.Cbo_MAStatus1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.GroupBox1.ResumeLayout(False)
    Me.GroupBox1.PerformLayout()
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents LibVakNr_1 As System.Windows.Forms.LinkLabel
  Friend WithEvents Chk_HBerufe As DevExpress.XtraEditors.CheckEdit
  Friend WithEvents Chk_SBerufe As DevExpress.XtraEditors.CheckEdit
  Friend WithEvents Chk_Branche As DevExpress.XtraEditors.CheckEdit
  Friend WithEvents Chk_Geschlecht As DevExpress.XtraEditors.CheckEdit
  Friend WithEvents txt_IndSQLQuery As System.Windows.Forms.TextBox
  Friend WithEvents Label3 As System.Windows.Forms.Label
  Friend WithEvents CboSort As DevExpress.XtraEditors.CheckedComboBoxEdit
	Friend WithEvents Label50 As System.Windows.Forms.Label
	Friend WithEvents Label45 As System.Windows.Forms.Label
  Friend WithEvents Chk_InES As DevExpress.XtraEditors.CheckEdit
  Friend WithEvents Cbo_MAKontakt As DevExpress.XtraEditors.CheckedComboBoxEdit
  Friend WithEvents Cbo_MAStatus1 As DevExpress.XtraEditors.CheckedComboBoxEdit
  Friend WithEvents Bar3 As DevExpress.XtraBars.Bar
  Friend WithEvents bsiInfo As DevExpress.XtraBars.BarStaticItem
  Friend WithEvents bbiSearch As DevExpress.XtraBars.BarButtonItem
  Friend WithEvents bbiClear As DevExpress.XtraBars.BarButtonItem
  Friend WithEvents bbiPrint As DevExpress.XtraBars.BarLargeButtonItem
  Friend WithEvents bbiExport As DevExpress.XtraBars.BarLargeButtonItem
  Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
  Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
  Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
  Friend WithEvents GroupBox1 As DevExpress.XtraEditors.PanelControl
  Friend WithEvents LblTimeValue As System.Windows.Forms.Label
  Friend WithEvents CmdClose As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents LblSetting As System.Windows.Forms.Label
  Friend WithEvents Label2 As System.Windows.Forms.Label
  Friend WithEvents Label4 As System.Windows.Forms.Label
End Class
