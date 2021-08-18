Namespace UI
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmApplicationDetail
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmApplicationDetail))
			Dim EditorButtonImageOptions1 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
			Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
			Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
			Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
			Dim SerializableAppearanceObject4 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
			Me.GroupBox1 = New DevExpress.XtraEditors.PanelControl()
			Me.btnClose = New DevExpress.XtraEditors.SimpleButton()
			Me.Label2 = New System.Windows.Forms.Label()
			Me.lblHeaderNormal = New System.Windows.Forms.Label()
			Me.lblHeaderFett = New System.Windows.Forms.Label()
			Me.grpWeitereSchritte = New DevExpress.XtraEditors.GroupControl()
			Me.btnEmployee = New DevExpress.XtraEditors.SimpleButton()
			Me.btnPropose = New DevExpress.XtraEditors.SimpleButton()
			Me.grpZuweisen = New DevExpress.XtraEditors.GroupControl()
			Me.lblBeraterIn = New DevExpress.XtraEditors.LabelControl()
			Me.lblFiliale = New DevExpress.XtraEditors.LabelControl()
			Me.btnAssign = New DevExpress.XtraEditors.SimpleButton()
			Me.lueAdvisor = New DevExpress.XtraEditors.LookUpEdit()
			Me.lueBusinessbranch = New DevExpress.XtraEditors.LookUpEdit()
			Me.grpBewerbung = New DevExpress.XtraEditors.GroupControl()
			Me.lblBewerbungals = New DevExpress.XtraEditors.LabelControl()
			Me.btnInviteApplication = New DevExpress.XtraEditors.SimpleButton()
			Me.lblChangedOn = New DevExpress.XtraEditors.LabelControl()
			Me.deChangedOn = New DevExpress.XtraEditors.DateEdit()
			Me.lblStatus = New DevExpress.XtraEditors.LabelControl()
			Me.btnRejectWithoutMail = New DevExpress.XtraEditors.SimpleButton()
			Me.lblDurch = New DevExpress.XtraEditors.LabelControl()
			Me.lblDatum = New DevExpress.XtraEditors.LabelControl()
			Me.lblBemerkung = New DevExpress.XtraEditors.LabelControl()
			Me.txtComment = New DevExpress.XtraEditors.MemoEdit()
			Me.deCreatedOn = New DevExpress.XtraEditors.DateEdit()
			Me.LifecycleInfo = New DevExpress.XtraEditors.LabelControl()
			Me.lueCheckedFrom = New DevExpress.XtraEditors.LookUpEdit()
			Me.txtApplicationLabel = New DevExpress.XtraEditors.ComboBoxEdit()
			Me.SeparatorControl1 = New DevExpress.XtraEditors.SeparatorControl()
			CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.GroupBox1.SuspendLayout()
			CType(Me.grpWeitereSchritte, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.grpWeitereSchritte.SuspendLayout()
			CType(Me.grpZuweisen, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.grpZuweisen.SuspendLayout()
			CType(Me.lueAdvisor.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.lueBusinessbranch.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.grpBewerbung, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.grpBewerbung.SuspendLayout()
			CType(Me.deChangedOn.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.deChangedOn.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.txtComment.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.deCreatedOn.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.deCreatedOn.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.lueCheckedFrom.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.txtApplicationLabel.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.SeparatorControl1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			'
			'GroupBox1
			'
			Me.GroupBox1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003
			Me.GroupBox1.Controls.Add(Me.btnClose)
			Me.GroupBox1.Controls.Add(Me.Label2)
			Me.GroupBox1.Controls.Add(Me.lblHeaderNormal)
			Me.GroupBox1.Controls.Add(Me.lblHeaderFett)
			Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
			Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
			Me.GroupBox1.Name = "GroupBox1"
			Me.GroupBox1.Size = New System.Drawing.Size(689, 77)
			Me.GroupBox1.TabIndex = 208
			'
			'btnClose
			'
			Me.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Right
			Me.btnClose.Location = New System.Drawing.Point(561, 21)
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
			'lblHeaderNormal
			'
			Me.lblHeaderNormal.BackColor = System.Drawing.Color.Transparent
			Me.lblHeaderNormal.Location = New System.Drawing.Point(112, 45)
			Me.lblHeaderNormal.Name = "lblHeaderNormal"
			Me.lblHeaderNormal.Size = New System.Drawing.Size(299, 16)
			Me.lblHeaderNormal.TabIndex = 1
			Me.lblHeaderNormal.Text = "Geben Sie bitte Ihre gewünschten Kriterien ein."
			'
			'lblHeaderFett
			'
			Me.lblHeaderFett.AutoSize = True
			Me.lblHeaderFett.BackColor = System.Drawing.Color.Transparent
			Me.lblHeaderFett.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
			Me.lblHeaderFett.Location = New System.Drawing.Point(94, 21)
			Me.lblHeaderFett.Name = "lblHeaderFett"
			Me.lblHeaderFett.Size = New System.Drawing.Size(176, 13)
			Me.lblHeaderFett.TabIndex = 0
			Me.lblHeaderFett.Text = "Verwaltung von Bewerbungen"
			'
			'grpWeitereSchritte
			'
			Me.grpWeitereSchritte.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.grpWeitereSchritte.Controls.Add(Me.btnEmployee)
			Me.grpWeitereSchritte.Controls.Add(Me.btnPropose)
			Me.grpWeitereSchritte.Location = New System.Drawing.Point(12, 592)
			Me.grpWeitereSchritte.Name = "grpWeitereSchritte"
			Me.grpWeitereSchritte.Size = New System.Drawing.Size(665, 82)
			Me.grpWeitereSchritte.TabIndex = 9
			Me.grpWeitereSchritte.Text = "Weitere Schritte"
			'
			'btnEmployee
			'
			Me.btnEmployee.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.btnEmployee.Location = New System.Drawing.Point(110, 34)
			Me.btnEmployee.Name = "btnEmployee"
			Me.btnEmployee.Size = New System.Drawing.Size(189, 29)
			Me.btnEmployee.TabIndex = 9
			Me.btnEmployee.Text = "Kandidat erstellen"
			'
			'btnPropose
			'
			Me.btnPropose.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.btnPropose.Location = New System.Drawing.Point(357, 34)
			Me.btnPropose.Name = "btnPropose"
			Me.btnPropose.Size = New System.Drawing.Size(189, 29)
			Me.btnPropose.TabIndex = 6
			Me.btnPropose.Text = "Vorschlag erstellen"
			'
			'grpZuweisen
			'
			Me.grpZuweisen.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.grpZuweisen.Controls.Add(Me.lblBeraterIn)
			Me.grpZuweisen.Controls.Add(Me.lblFiliale)
			Me.grpZuweisen.Controls.Add(Me.btnAssign)
			Me.grpZuweisen.Controls.Add(Me.lueAdvisor)
			Me.grpZuweisen.Controls.Add(Me.lueBusinessbranch)
			Me.grpZuweisen.Location = New System.Drawing.Point(12, 485)
			Me.grpZuweisen.Name = "grpZuweisen"
			Me.grpZuweisen.Size = New System.Drawing.Size(665, 101)
			Me.grpZuweisen.TabIndex = 8
			Me.grpZuweisen.Text = "Neu zuweisen"
			'
			'lblBeraterIn
			'
			Me.lblBeraterIn.Appearance.Options.UseTextOptions = True
			Me.lblBeraterIn.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblBeraterIn.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblBeraterIn.Location = New System.Drawing.Point(24, 40)
			Me.lblBeraterIn.Name = "lblBeraterIn"
			Me.lblBeraterIn.Size = New System.Drawing.Size(107, 13)
			Me.lblBeraterIn.TabIndex = 8
			Me.lblBeraterIn.Text = "BeraterIn"
			'
			'lblFiliale
			'
			Me.lblFiliale.Appearance.Options.UseTextOptions = True
			Me.lblFiliale.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblFiliale.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblFiliale.Location = New System.Drawing.Point(23, 66)
			Me.lblFiliale.Name = "lblFiliale"
			Me.lblFiliale.Size = New System.Drawing.Size(107, 13)
			Me.lblFiliale.TabIndex = 7
			Me.lblFiliale.Text = "Filiale"
			'
			'btnAssign
			'
			Me.btnAssign.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnAssign.Location = New System.Drawing.Point(530, 37)
			Me.btnAssign.Name = "btnAssign"
			Me.btnAssign.Size = New System.Drawing.Size(119, 29)
			Me.btnAssign.TabIndex = 5
			Me.btnAssign.Text = "Zuweisen"
			'
			'lueAdvisor
			'
			Me.lueAdvisor.Location = New System.Drawing.Point(137, 37)
			Me.lueAdvisor.Name = "lueAdvisor"
			Me.lueAdvisor.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.lueAdvisor.Properties.NullText = ""
			Me.lueAdvisor.Properties.PopupSizeable = False
			Me.lueAdvisor.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
			Me.lueAdvisor.Size = New System.Drawing.Size(294, 20)
			Me.lueAdvisor.TabIndex = 3
			'
			'lueBusinessbranch
			'
			Me.lueBusinessbranch.Location = New System.Drawing.Point(136, 63)
			Me.lueBusinessbranch.Name = "lueBusinessbranch"
			Me.lueBusinessbranch.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.lueBusinessbranch.Properties.NullText = ""
			Me.lueBusinessbranch.Properties.PopupSizeable = False
			Me.lueBusinessbranch.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
			Me.lueBusinessbranch.Size = New System.Drawing.Size(294, 20)
			Me.lueBusinessbranch.TabIndex = 4
			'
			'grpBewerbung
			'
			Me.grpBewerbung.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.grpBewerbung.Controls.Add(Me.SeparatorControl1)
			Me.grpBewerbung.Controls.Add(Me.lblBewerbungals)
			Me.grpBewerbung.Controls.Add(Me.btnInviteApplication)
			Me.grpBewerbung.Controls.Add(Me.lblChangedOn)
			Me.grpBewerbung.Controls.Add(Me.deChangedOn)
			Me.grpBewerbung.Controls.Add(Me.lblStatus)
			Me.grpBewerbung.Controls.Add(Me.btnRejectWithoutMail)
			Me.grpBewerbung.Controls.Add(Me.lblDurch)
			Me.grpBewerbung.Controls.Add(Me.lblDatum)
			Me.grpBewerbung.Controls.Add(Me.lblBemerkung)
			Me.grpBewerbung.Controls.Add(Me.txtComment)
			Me.grpBewerbung.Controls.Add(Me.deCreatedOn)
			Me.grpBewerbung.Controls.Add(Me.LifecycleInfo)
			Me.grpBewerbung.Controls.Add(Me.lueCheckedFrom)
			Me.grpBewerbung.Controls.Add(Me.txtApplicationLabel)
			Me.grpBewerbung.Location = New System.Drawing.Point(12, 88)
			Me.grpBewerbung.Name = "grpBewerbung"
			Me.grpBewerbung.Size = New System.Drawing.Size(665, 391)
			Me.grpBewerbung.TabIndex = 7
			Me.grpBewerbung.Text = "Bewerbung"
			'
			'lblBewerbungals
			'
			Me.lblBewerbungals.Appearance.Options.UseTextOptions = True
			Me.lblBewerbungals.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblBewerbungals.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblBewerbungals.Location = New System.Drawing.Point(23, 32)
			Me.lblBewerbungals.Name = "lblBewerbungals"
			Me.lblBewerbungals.Size = New System.Drawing.Size(107, 13)
			Me.lblBewerbungals.TabIndex = 12
			Me.lblBewerbungals.Text = "Bewerbung als"
			'
			'btnInviteApplication
			'
			Me.btnInviteApplication.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnInviteApplication.Location = New System.Drawing.Point(530, 284)
			Me.btnInviteApplication.Name = "btnInviteApplication"
			Me.btnInviteApplication.Size = New System.Drawing.Size(119, 29)
			Me.btnInviteApplication.TabIndex = 10
			Me.btnInviteApplication.Text = "Bewerbung zusagen"
			'
			'lblChangedOn
			'
			Me.lblChangedOn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
			Me.lblChangedOn.Appearance.Options.UseTextOptions = True
			Me.lblChangedOn.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblChangedOn.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblChangedOn.Location = New System.Drawing.Point(24, 319)
			Me.lblChangedOn.Name = "lblChangedOn"
			Me.lblChangedOn.Size = New System.Drawing.Size(107, 13)
			Me.lblChangedOn.TabIndex = 9
			Me.lblChangedOn.Text = "Geändert am"
			'
			'deChangedOn
			'
			Me.deChangedOn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
			Me.deChangedOn.EditValue = Nothing
			Me.deChangedOn.Location = New System.Drawing.Point(137, 315)
			Me.deChangedOn.Name = "deChangedOn"
			Me.deChangedOn.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.deChangedOn.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.deChangedOn.Size = New System.Drawing.Size(163, 20)
			Me.deChangedOn.TabIndex = 8
			'
			'lblStatus
			'
			Me.lblStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
			Me.lblStatus.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.lblStatus.Appearance.Options.UseFont = True
			Me.lblStatus.Appearance.Options.UseTextOptions = True
			Me.lblStatus.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblStatus.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblStatus.Location = New System.Drawing.Point(24, 367)
			Me.lblStatus.Name = "lblStatus"
			Me.lblStatus.Size = New System.Drawing.Size(107, 13)
			Me.lblStatus.TabIndex = 7
			Me.lblStatus.Text = "Status"
			'
			'btnRejectWithoutMail
			'
			Me.btnRejectWithoutMail.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnRejectWithoutMail.Location = New System.Drawing.Point(530, 319)
			Me.btnRejectWithoutMail.Name = "btnRejectWithoutMail"
			Me.btnRejectWithoutMail.Size = New System.Drawing.Size(119, 29)
			Me.btnRejectWithoutMail.TabIndex = 7
			Me.btnRejectWithoutMail.Text = "Bewerbung absagen"
			'
			'lblDurch
			'
			Me.lblDurch.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
			Me.lblDurch.Appearance.Options.UseTextOptions = True
			Me.lblDurch.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblDurch.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblDurch.Location = New System.Drawing.Point(23, 345)
			Me.lblDurch.Name = "lblDurch"
			Me.lblDurch.Size = New System.Drawing.Size(107, 13)
			Me.lblDurch.TabIndex = 6
			Me.lblDurch.Text = "Durch"
			'
			'lblDatum
			'
			Me.lblDatum.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
			Me.lblDatum.Appearance.Options.UseTextOptions = True
			Me.lblDatum.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblDatum.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblDatum.Location = New System.Drawing.Point(24, 288)
			Me.lblDatum.Name = "lblDatum"
			Me.lblDatum.Size = New System.Drawing.Size(107, 13)
			Me.lblDatum.TabIndex = 5
			Me.lblDatum.Text = "Erstellt am"
			'
			'lblBemerkung
			'
			Me.lblBemerkung.Appearance.Options.UseTextOptions = True
			Me.lblBemerkung.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblBemerkung.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblBemerkung.Location = New System.Drawing.Point(24, 60)
			Me.lblBemerkung.Name = "lblBemerkung"
			Me.lblBemerkung.Size = New System.Drawing.Size(107, 13)
			Me.lblBemerkung.TabIndex = 4
			Me.lblBemerkung.Text = "Bemerkung"
			'
			'txtComment
			'
			Me.txtComment.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.txtComment.Location = New System.Drawing.Point(137, 58)
			Me.txtComment.Name = "txtComment"
			Me.txtComment.Size = New System.Drawing.Size(512, 188)
			Me.txtComment.TabIndex = 0
			'
			'deCreatedOn
			'
			Me.deCreatedOn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
			Me.deCreatedOn.EditValue = Nothing
			Me.deCreatedOn.Location = New System.Drawing.Point(137, 284)
			Me.deCreatedOn.Name = "deCreatedOn"
			Me.deCreatedOn.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.deCreatedOn.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.deCreatedOn.Size = New System.Drawing.Size(163, 20)
			Me.deCreatedOn.TabIndex = 1
			'
			'LifecycleInfo
			'
			Me.LifecycleInfo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
			Me.LifecycleInfo.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.LifecycleInfo.Appearance.Options.UseFont = True
			Me.LifecycleInfo.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.LifecycleInfo.Location = New System.Drawing.Point(137, 367)
			Me.LifecycleInfo.Name = "LifecycleInfo"
			Me.LifecycleInfo.Size = New System.Drawing.Size(294, 13)
			Me.LifecycleInfo.TabIndex = 3
			Me.LifecycleInfo.Text = "LifecycleInfo"
			'
			'lueCheckedFrom
			'
			Me.lueCheckedFrom.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
			Me.lueCheckedFrom.Location = New System.Drawing.Point(136, 341)
			Me.lueCheckedFrom.Name = "lueCheckedFrom"
			Me.lueCheckedFrom.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.lueCheckedFrom.Properties.NullText = ""
			Me.lueCheckedFrom.Properties.PopupSizeable = False
			Me.lueCheckedFrom.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
			Me.lueCheckedFrom.Size = New System.Drawing.Size(163, 20)
			Me.lueCheckedFrom.TabIndex = 2
			'
			'txtApplicationLabel
			'
			Me.txtApplicationLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.txtApplicationLabel.Location = New System.Drawing.Point(136, 30)
			Me.txtApplicationLabel.Name = "txtApplicationLabel"
			EditorButtonImageOptions1.Image = CType(resources.GetObject("EditorButtonImageOptions1.Image"), System.Drawing.Image)
			Me.txtApplicationLabel.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, True, True, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
			Me.txtApplicationLabel.Size = New System.Drawing.Size(513, 24)
			Me.txtApplicationLabel.TabIndex = 11
			'
			'SeparatorControl1
			'
			Me.SeparatorControl1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.SeparatorControl1.Location = New System.Drawing.Point(5, 252)
			Me.SeparatorControl1.Name = "SeparatorControl1"
			Me.SeparatorControl1.Size = New System.Drawing.Size(655, 26)
			Me.SeparatorControl1.TabIndex = 295
			'
			'frmApplicationDetail
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(689, 686)
			Me.Controls.Add(Me.grpWeitereSchritte)
			Me.Controls.Add(Me.GroupBox1)
			Me.Controls.Add(Me.grpZuweisen)
			Me.Controls.Add(Me.grpBewerbung)
			Me.MinimumSize = New System.Drawing.Size(691, 718)
			Me.Name = "frmApplicationDetail"
			Me.Text = "Verwaltung von Bewerbungen"
			CType(Me.GroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.GroupBox1.ResumeLayout(False)
			Me.GroupBox1.PerformLayout()
			CType(Me.grpWeitereSchritte, System.ComponentModel.ISupportInitialize).EndInit()
			Me.grpWeitereSchritte.ResumeLayout(False)
			CType(Me.grpZuweisen, System.ComponentModel.ISupportInitialize).EndInit()
			Me.grpZuweisen.ResumeLayout(False)
			CType(Me.lueAdvisor.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.lueBusinessbranch.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.grpBewerbung, System.ComponentModel.ISupportInitialize).EndInit()
			Me.grpBewerbung.ResumeLayout(False)
			CType(Me.deChangedOn.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.deChangedOn.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.txtComment.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.deCreatedOn.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.deCreatedOn.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.lueCheckedFrom.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.txtApplicationLabel.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.SeparatorControl1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub
		Friend WithEvents GroupBox1 As DevExpress.XtraEditors.PanelControl
		Friend WithEvents btnClose As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents Label2 As System.Windows.Forms.Label
	Friend WithEvents lblHeaderNormal As System.Windows.Forms.Label
	Friend WithEvents lblHeaderFett As System.Windows.Forms.Label
		Friend WithEvents grpWeitereSchritte As DevExpress.XtraEditors.GroupControl
		Friend WithEvents btnEmployee As DevExpress.XtraEditors.SimpleButton
		Friend WithEvents btnPropose As DevExpress.XtraEditors.SimpleButton
		Friend WithEvents grpZuweisen As DevExpress.XtraEditors.GroupControl
		Friend WithEvents lblBeraterIn As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblFiliale As DevExpress.XtraEditors.LabelControl
		Friend WithEvents btnAssign As DevExpress.XtraEditors.SimpleButton
		Friend WithEvents lueAdvisor As DevExpress.XtraEditors.LookUpEdit
		Friend WithEvents lueBusinessbranch As DevExpress.XtraEditors.LookUpEdit
		Friend WithEvents grpBewerbung As DevExpress.XtraEditors.GroupControl
		Friend WithEvents lblBewerbungals As DevExpress.XtraEditors.LabelControl
		Friend WithEvents btnInviteApplication As DevExpress.XtraEditors.SimpleButton
		Friend WithEvents lblChangedOn As DevExpress.XtraEditors.LabelControl
		Friend WithEvents deChangedOn As DevExpress.XtraEditors.DateEdit
		Friend WithEvents lblStatus As DevExpress.XtraEditors.LabelControl
		Friend WithEvents btnRejectWithoutMail As DevExpress.XtraEditors.SimpleButton
		Friend WithEvents lblDurch As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblDatum As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblBemerkung As DevExpress.XtraEditors.LabelControl
		Friend WithEvents txtComment As DevExpress.XtraEditors.MemoEdit
		Friend WithEvents deCreatedOn As DevExpress.XtraEditors.DateEdit
		Friend WithEvents LifecycleInfo As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lueCheckedFrom As DevExpress.XtraEditors.LookUpEdit
		Friend WithEvents txtApplicationLabel As DevExpress.XtraEditors.ComboBoxEdit
		Friend WithEvents SeparatorControl1 As DevExpress.XtraEditors.SeparatorControl
	End Class
End Namespace
