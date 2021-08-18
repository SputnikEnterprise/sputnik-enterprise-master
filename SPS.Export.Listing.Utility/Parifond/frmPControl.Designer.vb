<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPControl
    Inherits DevExpress.XtraEditors.XtraForm

    'Form overrides dispose to clean up the component list.
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
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPControl))
		Me.GroupBox1 = New DevExpress.XtraEditors.PanelControl()
		Me.cmdClose = New DevExpress.XtraEditors.SimpleButton()
		Me.lblHeader1 = New System.Windows.Forms.Label()
		Me.lblMAStamm = New DevExpress.XtraEditors.LabelControl()
		Me.lblKDStamm = New DevExpress.XtraEditors.LabelControl()
		Me.lblVerleihvertrag = New DevExpress.XtraEditors.LabelControl()
		Me.lblESVertrag = New DevExpress.XtraEditors.LabelControl()
		Me.lblErfassteRP = New DevExpress.XtraEditors.LabelControl()
		Me.lblLohnabrechnungen = New DevExpress.XtraEditors.LabelControl()
		Me.sbMAStamm = New DevComponents.DotNetBar.Controls.SwitchButton()
		Me.sbKDStamm = New DevComponents.DotNetBar.Controls.SwitchButton()
		Me.sbESVertrag = New DevComponents.DotNetBar.Controls.SwitchButton()
		Me.sbVerleih = New DevComponents.DotNetBar.Controls.SwitchButton()
		Me.sbRPContent = New DevComponents.DotNetBar.Controls.SwitchButton()
		Me.sbLO = New DevComponents.DotNetBar.Controls.SwitchButton()
		Me.cmdCreateData = New DevExpress.XtraEditors.SimpleButton()
		Me.lblLOCount = New DevExpress.XtraEditors.LabelControl()
		Me.lblRPCount = New DevExpress.XtraEditors.LabelControl()
		Me.lblVerleihCount = New DevExpress.XtraEditors.LabelControl()
		Me.lblESVertragCount = New DevExpress.XtraEditors.LabelControl()
		Me.lblKDCount = New DevExpress.XtraEditors.LabelControl()
		Me.lblMACount = New DevExpress.XtraEditors.LabelControl()
		Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
		Me.CircularProgress1 = New DevComponents.DotNetBar.Controls.CircularProgress()
		Me.XtraScrollableControl1 = New DevExpress.XtraEditors.XtraScrollableControl()
		Me.lblLohnkontiCount = New DevExpress.XtraEditors.LabelControl()
		Me.sbLohnkonti = New DevComponents.DotNetBar.Controls.SwitchButton()
		Me.lblLohnkonti = New DevExpress.XtraEditors.LabelControl()
		Me.lblESListeState = New DevExpress.XtraEditors.LabelControl()
		Me.sbESListe = New DevComponents.DotNetBar.Controls.SwitchButton()
		Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl()
		Me.SeparatorControl1 = New DevExpress.XtraEditors.SeparatorControl()
		Me.tgsMergePDFFile = New DevExpress.XtraEditors.ToggleSwitch()
		Me.lblOutputfiles = New DevExpress.XtraEditors.LabelControl()
		CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupBox1.SuspendLayout()
		Me.XtraScrollableControl1.SuspendLayout()
		CType(Me.SeparatorControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.tgsMergePDFFile.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'GroupBox1
		'
		Me.GroupBox1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003
		Me.GroupBox1.Controls.Add(Me.cmdClose)
		Me.GroupBox1.Controls.Add(Me.lblHeader1)
		Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
		Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(769, 59)
		Me.GroupBox1.TabIndex = 210
		'
		'cmdClose
		'
		Me.cmdClose.Anchor = System.Windows.Forms.AnchorStyles.Right
		Me.cmdClose.Location = New System.Drawing.Point(673, 16)
		Me.cmdClose.Name = "cmdClose"
		Me.cmdClose.Size = New System.Drawing.Size(80, 25)
		Me.cmdClose.TabIndex = 204
		Me.cmdClose.Text = "Schliessen"
		'
		'lblHeader1
		'
		Me.lblHeader1.BackColor = System.Drawing.Color.Transparent
		Me.lblHeader1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblHeader1.Image = CType(resources.GetObject("lblHeader1.Image"), System.Drawing.Image)
		Me.lblHeader1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.lblHeader1.Location = New System.Drawing.Point(12, 13)
		Me.lblHeader1.Name = "lblHeader1"
		Me.lblHeader1.Size = New System.Drawing.Size(491, 31)
		Me.lblHeader1.TabIndex = 37
		Me.lblHeader1.Text = "Suchen nach Datensätze für Kontrolle der Paritätische Kommissionen"
		Me.lblHeader1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'lblMAStamm
		'
		Me.lblMAStamm.AllowHtmlString = True
		Me.lblMAStamm.Appearance.Options.UseTextOptions = True
		Me.lblMAStamm.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblMAStamm.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblMAStamm.Location = New System.Drawing.Point(21, 73)
		Me.lblMAStamm.Name = "lblMAStamm"
		Me.lblMAStamm.Size = New System.Drawing.Size(244, 13)
		Me.lblMAStamm.TabIndex = 211
		Me.lblMAStamm.Text = "Kandidatenstammblätter zusammenstellen"
		'
		'lblKDStamm
		'
		Me.lblKDStamm.AllowHtmlString = True
		Me.lblKDStamm.Appearance.Options.UseTextOptions = True
		Me.lblKDStamm.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblKDStamm.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblKDStamm.Location = New System.Drawing.Point(21, 115)
		Me.lblKDStamm.Name = "lblKDStamm"
		Me.lblKDStamm.Size = New System.Drawing.Size(244, 13)
		Me.lblKDStamm.TabIndex = 212
		Me.lblKDStamm.Text = "Kundenstammblätter zusammenstellen"
		'
		'lblVerleihvertrag
		'
		Me.lblVerleihvertrag.AllowHtmlString = True
		Me.lblVerleihvertrag.Appearance.Options.UseTextOptions = True
		Me.lblVerleihvertrag.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblVerleihvertrag.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblVerleihvertrag.Location = New System.Drawing.Point(21, 199)
		Me.lblVerleihvertrag.Name = "lblVerleihvertrag"
		Me.lblVerleihvertrag.Size = New System.Drawing.Size(244, 13)
		Me.lblVerleihvertrag.TabIndex = 214
		Me.lblVerleihvertrag.Text = "Verleihverträge zusammenstellen"
		'
		'lblESVertrag
		'
		Me.lblESVertrag.AllowHtmlString = True
		Me.lblESVertrag.Appearance.Options.UseTextOptions = True
		Me.lblESVertrag.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblESVertrag.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblESVertrag.Location = New System.Drawing.Point(21, 157)
		Me.lblESVertrag.Name = "lblESVertrag"
		Me.lblESVertrag.Size = New System.Drawing.Size(244, 13)
		Me.lblESVertrag.TabIndex = 213
		Me.lblESVertrag.Text = "Einsatzverträge zusammenstellen"
		'
		'lblErfassteRP
		'
		Me.lblErfassteRP.AllowHtmlString = True
		Me.lblErfassteRP.Appearance.Options.UseTextOptions = True
		Me.lblErfassteRP.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblErfassteRP.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblErfassteRP.Location = New System.Drawing.Point(21, 241)
		Me.lblErfassteRP.Name = "lblErfassteRP"
		Me.lblErfassteRP.Size = New System.Drawing.Size(244, 13)
		Me.lblErfassteRP.TabIndex = 215
		Me.lblErfassteRP.Text = "Erfassten Rapportdaten zusammenstellen"
		'
		'lblLohnabrechnungen
		'
		Me.lblLohnabrechnungen.AllowHtmlString = True
		Me.lblLohnabrechnungen.Appearance.Options.UseTextOptions = True
		Me.lblLohnabrechnungen.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblLohnabrechnungen.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblLohnabrechnungen.Location = New System.Drawing.Point(21, 283)
		Me.lblLohnabrechnungen.Name = "lblLohnabrechnungen"
		Me.lblLohnabrechnungen.Size = New System.Drawing.Size(244, 13)
		Me.lblLohnabrechnungen.TabIndex = 216
		Me.lblLohnabrechnungen.Text = "Lohnabrechnungen zusammenstellen"
		'
		'sbMAStamm
		'
		Me.sbMAStamm.BackColor = System.Drawing.Color.White
		'
		'
		'
		Me.sbMAStamm.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
		Me.sbMAStamm.FocusCuesEnabled = False
		Me.sbMAStamm.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.sbMAStamm.Location = New System.Drawing.Point(276, 67)
		Me.sbMAStamm.Name = "sbMAStamm"
		Me.sbMAStamm.OffBackColor = System.Drawing.Color.White
		Me.sbMAStamm.OffText = "O"
		Me.sbMAStamm.OffTextColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(53, Byte), Integer))
		Me.sbMAStamm.OnBackColor = System.Drawing.Color.LightSteelBlue
		Me.sbMAStamm.OnText = "|"
		Me.sbMAStamm.OnTextColor = System.Drawing.Color.Black
		Me.sbMAStamm.Size = New System.Drawing.Size(164, 25)
		Me.sbMAStamm.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
		Me.sbMAStamm.SwitchBackColor = System.Drawing.SystemColors.ControlDark
		Me.sbMAStamm.SwitchBorderColor = System.Drawing.SystemColors.ControlDark
		Me.sbMAStamm.SwitchFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.sbMAStamm.SwitchWidth = 30
		Me.sbMAStamm.TabIndex = 255
		'
		'sbKDStamm
		'
		Me.sbKDStamm.BackColor = System.Drawing.Color.White
		'
		'
		'
		Me.sbKDStamm.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
		Me.sbKDStamm.FocusCuesEnabled = False
		Me.sbKDStamm.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.sbKDStamm.Location = New System.Drawing.Point(276, 109)
		Me.sbKDStamm.Name = "sbKDStamm"
		Me.sbKDStamm.OffBackColor = System.Drawing.Color.White
		Me.sbKDStamm.OffText = "O"
		Me.sbKDStamm.OffTextColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(53, Byte), Integer))
		Me.sbKDStamm.OnBackColor = System.Drawing.Color.LightSteelBlue
		Me.sbKDStamm.OnText = "|"
		Me.sbKDStamm.OnTextColor = System.Drawing.Color.Black
		Me.sbKDStamm.Size = New System.Drawing.Size(164, 25)
		Me.sbKDStamm.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
		Me.sbKDStamm.SwitchBackColor = System.Drawing.SystemColors.ControlDark
		Me.sbKDStamm.SwitchBorderColor = System.Drawing.SystemColors.ControlDark
		Me.sbKDStamm.SwitchFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.sbKDStamm.SwitchWidth = 30
		Me.sbKDStamm.TabIndex = 256
		'
		'sbESVertrag
		'
		Me.sbESVertrag.BackColor = System.Drawing.Color.White
		'
		'
		'
		Me.sbESVertrag.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
		Me.sbESVertrag.FocusCuesEnabled = False
		Me.sbESVertrag.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.sbESVertrag.Location = New System.Drawing.Point(276, 151)
		Me.sbESVertrag.Name = "sbESVertrag"
		Me.sbESVertrag.OffBackColor = System.Drawing.Color.White
		Me.sbESVertrag.OffText = "O"
		Me.sbESVertrag.OffTextColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(53, Byte), Integer))
		Me.sbESVertrag.OnBackColor = System.Drawing.Color.LightSteelBlue
		Me.sbESVertrag.OnText = "|"
		Me.sbESVertrag.OnTextColor = System.Drawing.Color.Black
		Me.sbESVertrag.Size = New System.Drawing.Size(164, 25)
		Me.sbESVertrag.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
		Me.sbESVertrag.SwitchBackColor = System.Drawing.SystemColors.ControlDark
		Me.sbESVertrag.SwitchBorderColor = System.Drawing.SystemColors.ControlDark
		Me.sbESVertrag.SwitchFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.sbESVertrag.SwitchWidth = 30
		Me.sbESVertrag.TabIndex = 257
		'
		'sbVerleih
		'
		Me.sbVerleih.BackColor = System.Drawing.Color.White
		'
		'
		'
		Me.sbVerleih.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
		Me.sbVerleih.FocusCuesEnabled = False
		Me.sbVerleih.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.sbVerleih.Location = New System.Drawing.Point(276, 193)
		Me.sbVerleih.Name = "sbVerleih"
		Me.sbVerleih.OffBackColor = System.Drawing.Color.White
		Me.sbVerleih.OffText = "O"
		Me.sbVerleih.OffTextColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(53, Byte), Integer))
		Me.sbVerleih.OnBackColor = System.Drawing.Color.LightSteelBlue
		Me.sbVerleih.OnText = "|"
		Me.sbVerleih.OnTextColor = System.Drawing.Color.Black
		Me.sbVerleih.Size = New System.Drawing.Size(164, 25)
		Me.sbVerleih.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
		Me.sbVerleih.SwitchBackColor = System.Drawing.SystemColors.ControlDark
		Me.sbVerleih.SwitchBorderColor = System.Drawing.SystemColors.ControlDark
		Me.sbVerleih.SwitchFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.sbVerleih.SwitchWidth = 30
		Me.sbVerleih.TabIndex = 258
		'
		'sbRPContent
		'
		Me.sbRPContent.BackColor = System.Drawing.Color.White
		'
		'
		'
		Me.sbRPContent.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
		Me.sbRPContent.FocusCuesEnabled = False
		Me.sbRPContent.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.sbRPContent.Location = New System.Drawing.Point(276, 235)
		Me.sbRPContent.Name = "sbRPContent"
		Me.sbRPContent.OffBackColor = System.Drawing.Color.White
		Me.sbRPContent.OffText = "O"
		Me.sbRPContent.OffTextColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(53, Byte), Integer))
		Me.sbRPContent.OnBackColor = System.Drawing.Color.LightSteelBlue
		Me.sbRPContent.OnText = "|"
		Me.sbRPContent.OnTextColor = System.Drawing.Color.Black
		Me.sbRPContent.Size = New System.Drawing.Size(164, 25)
		Me.sbRPContent.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
		Me.sbRPContent.SwitchBackColor = System.Drawing.SystemColors.ControlDark
		Me.sbRPContent.SwitchBorderColor = System.Drawing.SystemColors.ControlDark
		Me.sbRPContent.SwitchFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.sbRPContent.SwitchWidth = 30
		Me.sbRPContent.TabIndex = 259
		'
		'sbLO
		'
		Me.sbLO.BackColor = System.Drawing.Color.White
		'
		'
		'
		Me.sbLO.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
		Me.sbLO.FocusCuesEnabled = False
		Me.sbLO.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.sbLO.Location = New System.Drawing.Point(276, 277)
		Me.sbLO.Name = "sbLO"
		Me.sbLO.OffBackColor = System.Drawing.Color.White
		Me.sbLO.OffText = "O"
		Me.sbLO.OffTextColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(53, Byte), Integer))
		Me.sbLO.OnBackColor = System.Drawing.Color.LightSteelBlue
		Me.sbLO.OnText = "|"
		Me.sbLO.OnTextColor = System.Drawing.Color.Black
		Me.sbLO.Size = New System.Drawing.Size(164, 25)
		Me.sbLO.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
		Me.sbLO.SwitchBackColor = System.Drawing.SystemColors.ControlDark
		Me.sbLO.SwitchBorderColor = System.Drawing.SystemColors.ControlDark
		Me.sbLO.SwitchFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.sbLO.SwitchWidth = 30
		Me.sbLO.TabIndex = 260
		'
		'cmdCreateData
		'
		Me.cmdCreateData.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.cmdCreateData.Location = New System.Drawing.Point(605, 473)
		Me.cmdCreateData.Name = "cmdCreateData"
		Me.cmdCreateData.Size = New System.Drawing.Size(147, 25)
		Me.cmdCreateData.TabIndex = 261
		Me.cmdCreateData.Text = "Daten zusammenstellen"
		'
		'lblLOCount
		'
		Me.lblLOCount.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblLOCount.Location = New System.Drawing.Point(446, 277)
		Me.lblLOCount.Name = "lblLOCount"
		Me.lblLOCount.Size = New System.Drawing.Size(202, 25)
		Me.lblLOCount.TabIndex = 268
		Me.lblLOCount.Text = "{0} Datensätze gefunden"
		'
		'lblRPCount
		'
		Me.lblRPCount.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblRPCount.Location = New System.Drawing.Point(446, 235)
		Me.lblRPCount.Name = "lblRPCount"
		Me.lblRPCount.Size = New System.Drawing.Size(202, 25)
		Me.lblRPCount.TabIndex = 267
		Me.lblRPCount.Text = "{0} Datensätze gefunden"
		'
		'lblVerleihCount
		'
		Me.lblVerleihCount.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblVerleihCount.Location = New System.Drawing.Point(446, 193)
		Me.lblVerleihCount.Name = "lblVerleihCount"
		Me.lblVerleihCount.Size = New System.Drawing.Size(202, 25)
		Me.lblVerleihCount.TabIndex = 266
		Me.lblVerleihCount.Text = "{0} Datensätze gefunden"
		'
		'lblESVertragCount
		'
		Me.lblESVertragCount.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblESVertragCount.Location = New System.Drawing.Point(446, 151)
		Me.lblESVertragCount.Name = "lblESVertragCount"
		Me.lblESVertragCount.Size = New System.Drawing.Size(202, 25)
		Me.lblESVertragCount.TabIndex = 265
		Me.lblESVertragCount.Text = "{0} Datensätze gefunden"
		'
		'lblKDCount
		'
		Me.lblKDCount.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblKDCount.Location = New System.Drawing.Point(446, 109)
		Me.lblKDCount.Name = "lblKDCount"
		Me.lblKDCount.Size = New System.Drawing.Size(202, 25)
		Me.lblKDCount.TabIndex = 264
		Me.lblKDCount.Text = "{0} Datensätze gefunden"
		'
		'lblMACount
		'
		Me.lblMACount.Appearance.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
		Me.lblMACount.Appearance.ImageList = Me.ImageList1
		Me.lblMACount.Appearance.Options.UseImageAlign = True
		Me.lblMACount.Appearance.Options.UseImageList = True
		Me.lblMACount.Appearance.Options.UseTextOptions = True
		Me.lblMACount.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
		Me.lblMACount.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblMACount.Location = New System.Drawing.Point(446, 67)
		Me.lblMACount.Name = "lblMACount"
		Me.lblMACount.Size = New System.Drawing.Size(202, 25)
		Me.lblMACount.TabIndex = 263
		Me.lblMACount.Text = "{0} Datensätze gefunden"
		'
		'ImageList1
		'
		Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
		Me.ImageList1.Images.SetKeyName(0, "OK.ico")
		Me.ImageList1.Images.SetKeyName(1, "green_rot.gif")
		'
		'CircularProgress1
		'
		Me.CircularProgress1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		'
		'
		'
		Me.CircularProgress1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
		Me.CircularProgress1.Location = New System.Drawing.Point(711, 67)
		Me.CircularProgress1.Name = "CircularProgress1"
		Me.CircularProgress1.ProgressBarType = DevComponents.DotNetBar.eCircularProgressType.Spoke
		Me.CircularProgress1.Size = New System.Drawing.Size(25, 25)
		Me.CircularProgress1.Style = DevComponents.DotNetBar.eDotNetBarStyle.OfficeXP
		Me.CircularProgress1.TabIndex = 270
		'
		'XtraScrollableControl1
		'
		Me.XtraScrollableControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.XtraScrollableControl1.Controls.Add(Me.lblLohnkontiCount)
		Me.XtraScrollableControl1.Controls.Add(Me.sbLohnkonti)
		Me.XtraScrollableControl1.Controls.Add(Me.lblLohnkonti)
		Me.XtraScrollableControl1.Controls.Add(Me.lblESListeState)
		Me.XtraScrollableControl1.Controls.Add(Me.sbESListe)
		Me.XtraScrollableControl1.Controls.Add(Me.LabelControl2)
		Me.XtraScrollableControl1.Controls.Add(Me.CircularProgress1)
		Me.XtraScrollableControl1.Controls.Add(Me.lblLOCount)
		Me.XtraScrollableControl1.Controls.Add(Me.lblRPCount)
		Me.XtraScrollableControl1.Controls.Add(Me.lblVerleihCount)
		Me.XtraScrollableControl1.Controls.Add(Me.lblESVertragCount)
		Me.XtraScrollableControl1.Controls.Add(Me.lblKDCount)
		Me.XtraScrollableControl1.Controls.Add(Me.lblMACount)
		Me.XtraScrollableControl1.Controls.Add(Me.sbLO)
		Me.XtraScrollableControl1.Controls.Add(Me.sbRPContent)
		Me.XtraScrollableControl1.Controls.Add(Me.sbVerleih)
		Me.XtraScrollableControl1.Controls.Add(Me.sbESVertrag)
		Me.XtraScrollableControl1.Controls.Add(Me.sbKDStamm)
		Me.XtraScrollableControl1.Controls.Add(Me.sbMAStamm)
		Me.XtraScrollableControl1.Controls.Add(Me.lblLohnabrechnungen)
		Me.XtraScrollableControl1.Controls.Add(Me.lblErfassteRP)
		Me.XtraScrollableControl1.Controls.Add(Me.lblVerleihvertrag)
		Me.XtraScrollableControl1.Controls.Add(Me.lblESVertrag)
		Me.XtraScrollableControl1.Controls.Add(Me.lblKDStamm)
		Me.XtraScrollableControl1.Controls.Add(Me.lblMAStamm)
		Me.XtraScrollableControl1.Location = New System.Drawing.Point(15, 67)
		Me.XtraScrollableControl1.Name = "XtraScrollableControl1"
		Me.XtraScrollableControl1.Size = New System.Drawing.Size(747, 393)
		Me.XtraScrollableControl1.TabIndex = 272
		'
		'lblLohnkontiCount
		'
		Me.lblLohnkontiCount.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblLohnkontiCount.Location = New System.Drawing.Point(446, 318)
		Me.lblLohnkontiCount.Name = "lblLohnkontiCount"
		Me.lblLohnkontiCount.Size = New System.Drawing.Size(202, 25)
		Me.lblLohnkontiCount.TabIndex = 276
		Me.lblLohnkontiCount.Text = "{0} Datensätze gefunden"
		'
		'sbLohnkonti
		'
		Me.sbLohnkonti.BackColor = System.Drawing.Color.White
		'
		'
		'
		Me.sbLohnkonti.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
		Me.sbLohnkonti.FocusCuesEnabled = False
		Me.sbLohnkonti.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.sbLohnkonti.Location = New System.Drawing.Point(276, 318)
		Me.sbLohnkonti.Name = "sbLohnkonti"
		Me.sbLohnkonti.OffBackColor = System.Drawing.Color.White
		Me.sbLohnkonti.OffText = "O"
		Me.sbLohnkonti.OffTextColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(53, Byte), Integer))
		Me.sbLohnkonti.OnBackColor = System.Drawing.Color.LightSteelBlue
		Me.sbLohnkonti.OnText = "|"
		Me.sbLohnkonti.OnTextColor = System.Drawing.Color.Black
		Me.sbLohnkonti.Size = New System.Drawing.Size(164, 25)
		Me.sbLohnkonti.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
		Me.sbLohnkonti.SwitchBackColor = System.Drawing.SystemColors.ControlDark
		Me.sbLohnkonti.SwitchBorderColor = System.Drawing.SystemColors.ControlDark
		Me.sbLohnkonti.SwitchFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.sbLohnkonti.SwitchWidth = 30
		Me.sbLohnkonti.TabIndex = 275
		'
		'lblLohnkonti
		'
		Me.lblLohnkonti.AllowHtmlString = True
		Me.lblLohnkonti.Appearance.Options.UseTextOptions = True
		Me.lblLohnkonti.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblLohnkonti.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblLohnkonti.Location = New System.Drawing.Point(21, 324)
		Me.lblLohnkonti.Name = "lblLohnkonti"
		Me.lblLohnkonti.Size = New System.Drawing.Size(244, 13)
		Me.lblLohnkonti.TabIndex = 274
		Me.lblLohnkonti.Text = "Lohnkonti zusammenstellen"
		'
		'lblESListeState
		'
		Me.lblESListeState.Appearance.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
		Me.lblESListeState.Appearance.ImageList = Me.ImageList1
		Me.lblESListeState.Appearance.Options.UseImageAlign = True
		Me.lblESListeState.Appearance.Options.UseImageList = True
		Me.lblESListeState.Appearance.Options.UseTextOptions = True
		Me.lblESListeState.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
		Me.lblESListeState.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblESListeState.Location = New System.Drawing.Point(446, 24)
		Me.lblESListeState.Name = "lblESListeState"
		Me.lblESListeState.Size = New System.Drawing.Size(202, 25)
		Me.lblESListeState.TabIndex = 273
		Me.lblESListeState.Text = "Liste wird erstellt."
		'
		'sbESListe
		'
		Me.sbESListe.BackColor = System.Drawing.Color.White
		'
		'
		'
		Me.sbESListe.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
		Me.sbESListe.FocusCuesEnabled = False
		Me.sbESListe.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.sbESListe.Location = New System.Drawing.Point(276, 24)
		Me.sbESListe.Name = "sbESListe"
		Me.sbESListe.OffBackColor = System.Drawing.Color.White
		Me.sbESListe.OffText = "O"
		Me.sbESListe.OffTextColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(53, Byte), Integer))
		Me.sbESListe.OnBackColor = System.Drawing.Color.LightSteelBlue
		Me.sbESListe.OnText = "|"
		Me.sbESListe.OnTextColor = System.Drawing.Color.Black
		Me.sbESListe.Size = New System.Drawing.Size(164, 25)
		Me.sbESListe.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
		Me.sbESListe.SwitchBackColor = System.Drawing.SystemColors.ControlDark
		Me.sbESListe.SwitchBorderColor = System.Drawing.SystemColors.ControlDark
		Me.sbESListe.SwitchFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.sbESListe.SwitchWidth = 30
		Me.sbESListe.TabIndex = 272
		'
		'LabelControl2
		'
		Me.LabelControl2.AllowHtmlString = True
		Me.LabelControl2.Appearance.Options.UseTextOptions = True
		Me.LabelControl2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.LabelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.LabelControl2.Location = New System.Drawing.Point(21, 30)
		Me.LabelControl2.Name = "LabelControl2"
		Me.LabelControl2.Size = New System.Drawing.Size(244, 13)
		Me.LabelControl2.TabIndex = 271
		Me.LabelControl2.Text = "Einsatzliste"
		'
		'SeparatorControl1
		'
		Me.SeparatorControl1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.SeparatorControl1.Location = New System.Drawing.Point(15, 443)
		Me.SeparatorControl1.Name = "SeparatorControl1"
		Me.SeparatorControl1.Size = New System.Drawing.Size(737, 18)
		Me.SeparatorControl1.TabIndex = 273
		'
		'tgsMergePDFFile
		'
		Me.tgsMergePDFFile.EditValue = True
		Me.tgsMergePDFFile.Location = New System.Drawing.Point(291, 476)
		Me.tgsMergePDFFile.Name = "tgsMergePDFFile"
		Me.tgsMergePDFFile.Properties.AllowFocused = False
		Me.tgsMergePDFFile.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.[True]
		Me.tgsMergePDFFile.Properties.Appearance.Options.UseTextOptions = True
		Me.tgsMergePDFFile.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
		Me.tgsMergePDFFile.Properties.OffText = "werden in einzelne und ZIP-Format erstellt"
		Me.tgsMergePDFFile.Properties.OnText = "werden in einer PDF-Datei zusammengestellt"
		Me.tgsMergePDFFile.Size = New System.Drawing.Size(308, 18)
		Me.tgsMergePDFFile.TabIndex = 299
		Me.tgsMergePDFFile.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information
		'
		'lblOutputfiles
		'
		Me.lblOutputfiles.AllowHtmlString = True
		Me.lblOutputfiles.Appearance.Options.UseTextOptions = True
		Me.lblOutputfiles.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblOutputfiles.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblOutputfiles.Location = New System.Drawing.Point(88, 479)
		Me.lblOutputfiles.Name = "lblOutputfiles"
		Me.lblOutputfiles.Size = New System.Drawing.Size(192, 13)
		Me.lblOutputfiles.TabIndex = 300
		Me.lblOutputfiles.Text = "Ausgabe-Dateien"
		'
		'frmPControl
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(769, 545)
		Me.Controls.Add(Me.lblOutputfiles)
		Me.Controls.Add(Me.tgsMergePDFFile)
		Me.Controls.Add(Me.SeparatorControl1)
		Me.Controls.Add(Me.XtraScrollableControl1)
		Me.Controls.Add(Me.cmdCreateData)
		Me.Controls.Add(Me.GroupBox1)
		Me.IconOptions.Icon = CType(resources.GetObject("frmPControl.IconOptions.Icon"), System.Drawing.Icon)
		Me.MinimumSize = New System.Drawing.Size(771, 577)
		Me.Name = "frmPControl"
		Me.Text = "Parifond-Kontrolle"
		CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupBox1.ResumeLayout(False)
		Me.XtraScrollableControl1.ResumeLayout(False)
		CType(Me.SeparatorControl1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.tgsMergePDFFile.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents GroupBox1 As DevExpress.XtraEditors.PanelControl
	Friend WithEvents cmdClose As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents lblHeader1 As System.Windows.Forms.Label
	Friend WithEvents lblMAStamm As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblKDStamm As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblVerleihvertrag As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblESVertrag As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblErfassteRP As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblLohnabrechnungen As DevExpress.XtraEditors.LabelControl
	Friend WithEvents sbMAStamm As DevComponents.DotNetBar.Controls.SwitchButton
	Friend WithEvents sbKDStamm As DevComponents.DotNetBar.Controls.SwitchButton
	Friend WithEvents sbESVertrag As DevComponents.DotNetBar.Controls.SwitchButton
	Friend WithEvents sbVerleih As DevComponents.DotNetBar.Controls.SwitchButton
	Friend WithEvents sbRPContent As DevComponents.DotNetBar.Controls.SwitchButton
	Friend WithEvents sbLO As DevComponents.DotNetBar.Controls.SwitchButton
	Friend WithEvents cmdCreateData As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents lblLOCount As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblRPCount As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblVerleihCount As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblESVertragCount As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblKDCount As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblMACount As DevExpress.XtraEditors.LabelControl
	Friend WithEvents CircularProgress1 As DevComponents.DotNetBar.Controls.CircularProgress
	Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
	Friend WithEvents XtraScrollableControl1 As DevExpress.XtraEditors.XtraScrollableControl
	Friend WithEvents lblESListeState As DevExpress.XtraEditors.LabelControl
	Friend WithEvents sbESListe As DevComponents.DotNetBar.Controls.SwitchButton
	Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblLohnkontiCount As DevExpress.XtraEditors.LabelControl
	Friend WithEvents sbLohnkonti As DevComponents.DotNetBar.Controls.SwitchButton
	Friend WithEvents lblLohnkonti As DevExpress.XtraEditors.LabelControl
	Friend WithEvents SeparatorControl1 As DevExpress.XtraEditors.SeparatorControl
	Friend WithEvents tgsMergePDFFile As DevExpress.XtraEditors.ToggleSwitch
	Friend WithEvents lblOutputfiles As DevExpress.XtraEditors.LabelControl
End Class
