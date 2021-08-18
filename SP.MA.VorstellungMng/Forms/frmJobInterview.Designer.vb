<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmJobInterview
  Inherits SP.Infrastructure.Forms.frmBase

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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmJobInterview))
		Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject4 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject5 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject6 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Me.sccMain = New DevExpress.XtraEditors.SplitContainerControl()
		Me.gridInterview = New DevExpress.XtraGrid.GridControl()
		Me.gvInterview = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.btnCreateTODO = New DevExpress.XtraEditors.SimpleButton()
		Me.employeePicture = New DevExpress.XtraEditors.PictureEdit()
		Me.lueState1 = New DevExpress.XtraEditors.LookUpEdit()
		Me.lblstatus = New DevExpress.XtraEditors.LabelControl()
		Me.lblHomepage = New DevExpress.XtraEditors.LabelControl()
		Me.lblEMail = New DevExpress.XtraEditors.LabelControl()
		Me.lblErgebnis = New DevExpress.XtraEditors.LabelControl()
		Me.lblvorschlag = New DevExpress.XtraEditors.LabelControl()
		Me.lblvakanz = New DevExpress.XtraEditors.LabelControl()
		Me.txteMail = New DevExpress.XtraEditors.TextEdit()
		Me.txtHompage = New DevExpress.XtraEditors.TextEdit()
		Me.lblZHD = New DevExpress.XtraEditors.LabelControl()
		Me.timeStart = New DevExpress.XtraEditors.TimeEdit()
		Me.dateEditFrom = New DevExpress.XtraEditors.DateEdit()
		Me.lbldatum = New DevExpress.XtraEditors.LabelControl()
		Me.lblKunde = New DevExpress.XtraEditors.LabelControl()
		Me.lblAdresse = New DevExpress.XtraEditors.LabelControl()
		Me.txtTelefax = New DevExpress.XtraEditors.TextEdit()
		Me.txtTelefon = New DevExpress.XtraEditors.TextEdit()
		Me.txtAddress = New DevExpress.XtraEditors.TextEdit()
		Me.lblTelefon = New DevExpress.XtraEditors.LabelControl()
		Me.lblTelefax = New DevExpress.XtraEditors.LabelControl()
		Me.txtInterviewAs = New DevExpress.XtraEditors.TextEdit()
		Me.lblVorstellungals = New DevExpress.XtraEditors.LabelControl()
		Me.btnDeleteInterview = New DevExpress.XtraEditors.SimpleButton()
		Me.lblInterviewChanged = New DevExpress.XtraEditors.LabelControl()
		Me.lblInterviewCreated = New DevExpress.XtraEditors.LabelControl()
		Me.lblgaendert = New DevExpress.XtraEditors.LabelControl()
		Me.lblerstellt = New DevExpress.XtraEditors.LabelControl()
		Me.btnNewInterview = New DevExpress.XtraEditors.SimpleButton()
		Me.btnSave = New DevExpress.XtraEditors.SimpleButton()
		Me.txtResult = New DevExpress.XtraEditors.MemoEdit()
		Me.lueZHDName = New DevExpress.XtraEditors.GridLookUpEdit()
		Me.gvZHDName = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.lueCustomerName = New DevExpress.XtraEditors.GridLookUpEdit()
		Me.gvCustomer = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.lueVacancy = New DevExpress.XtraEditors.GridLookUpEdit()
		Me.gvVacancy = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.luePropose = New DevExpress.XtraEditors.GridLookUpEdit()
		Me.gvPropose = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.errorProviderJobInterviewMng = New System.Windows.Forms.ErrorProvider()
		CType(Me.sccMain, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.sccMain.SuspendLayout()
		CType(Me.gridInterview, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvInterview, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.employeePicture.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lueState1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txteMail.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtHompage.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.timeStart.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.dateEditFrom.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.dateEditFrom.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtTelefax.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtTelefon.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtAddress.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtInterviewAs.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtResult.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lueZHDName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvZHDName, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lueCustomerName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvCustomer, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lueVacancy.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvVacancy, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.luePropose.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvPropose, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.errorProviderJobInterviewMng, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'sccMain
		'
		Me.sccMain.Dock = System.Windows.Forms.DockStyle.Fill
		Me.sccMain.Location = New System.Drawing.Point(5, 5)
		Me.sccMain.Name = "sccMain"
		Me.sccMain.Panel1.Controls.Add(Me.gridInterview)
		Me.sccMain.Panel1.Text = "Panel1"
		Me.sccMain.Panel2.Controls.Add(Me.btnCreateTODO)
		Me.sccMain.Panel2.Controls.Add(Me.employeePicture)
		Me.sccMain.Panel2.Controls.Add(Me.lueState1)
		Me.sccMain.Panel2.Controls.Add(Me.lblstatus)
		Me.sccMain.Panel2.Controls.Add(Me.lblHomepage)
		Me.sccMain.Panel2.Controls.Add(Me.lblEMail)
		Me.sccMain.Panel2.Controls.Add(Me.lblErgebnis)
		Me.sccMain.Panel2.Controls.Add(Me.lblvorschlag)
		Me.sccMain.Panel2.Controls.Add(Me.lblvakanz)
		Me.sccMain.Panel2.Controls.Add(Me.txteMail)
		Me.sccMain.Panel2.Controls.Add(Me.txtHompage)
		Me.sccMain.Panel2.Controls.Add(Me.lblZHD)
		Me.sccMain.Panel2.Controls.Add(Me.timeStart)
		Me.sccMain.Panel2.Controls.Add(Me.dateEditFrom)
		Me.sccMain.Panel2.Controls.Add(Me.lbldatum)
		Me.sccMain.Panel2.Controls.Add(Me.lblKunde)
		Me.sccMain.Panel2.Controls.Add(Me.lblAdresse)
		Me.sccMain.Panel2.Controls.Add(Me.txtTelefax)
		Me.sccMain.Panel2.Controls.Add(Me.txtTelefon)
		Me.sccMain.Panel2.Controls.Add(Me.txtAddress)
		Me.sccMain.Panel2.Controls.Add(Me.lblTelefon)
		Me.sccMain.Panel2.Controls.Add(Me.lblTelefax)
		Me.sccMain.Panel2.Controls.Add(Me.txtInterviewAs)
		Me.sccMain.Panel2.Controls.Add(Me.lblVorstellungals)
		Me.sccMain.Panel2.Controls.Add(Me.btnDeleteInterview)
		Me.sccMain.Panel2.Controls.Add(Me.lblInterviewChanged)
		Me.sccMain.Panel2.Controls.Add(Me.lblInterviewCreated)
		Me.sccMain.Panel2.Controls.Add(Me.lblgaendert)
		Me.sccMain.Panel2.Controls.Add(Me.lblerstellt)
		Me.sccMain.Panel2.Controls.Add(Me.btnNewInterview)
		Me.sccMain.Panel2.Controls.Add(Me.btnSave)
		Me.sccMain.Panel2.Controls.Add(Me.txtResult)
		Me.sccMain.Panel2.Controls.Add(Me.lueZHDName)
		Me.sccMain.Panel2.Controls.Add(Me.lueCustomerName)
		Me.sccMain.Panel2.Controls.Add(Me.lueVacancy)
		Me.sccMain.Panel2.Controls.Add(Me.luePropose)
		Me.sccMain.Panel2.Text = "Panel2"
		Me.sccMain.Size = New System.Drawing.Size(1021, 511)
		Me.sccMain.SplitterPosition = 322
		Me.sccMain.TabIndex = 0
		Me.sccMain.Text = "SplitContainerControl1"
		'
		'gridInterview
		'
		Me.gridInterview.Dock = System.Windows.Forms.DockStyle.Fill
		Me.gridInterview.Location = New System.Drawing.Point(0, 0)
		Me.gridInterview.MainView = Me.gvInterview
		Me.gridInterview.Name = "gridInterview"
		Me.gridInterview.Size = New System.Drawing.Size(322, 511)
		Me.gridInterview.TabIndex = 3
		Me.gridInterview.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvInterview})
		'
		'gvInterview
		'
		Me.gvInterview.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvInterview.GridControl = Me.gridInterview
		Me.gvInterview.Name = "gvInterview"
		Me.gvInterview.OptionsBehavior.Editable = False
		Me.gvInterview.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvInterview.OptionsView.ShowGroupPanel = False
		'
		'btnCreateTODO
		'
		Me.btnCreateTODO.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnCreateTODO.Image = CType(resources.GetObject("btnCreateTODO.Image"), System.Drawing.Image)
		Me.btnCreateTODO.Location = New System.Drawing.Point(554, 369)
		Me.btnCreateTODO.Name = "btnCreateTODO"
		Me.btnCreateTODO.Size = New System.Drawing.Size(105, 28)
		Me.btnCreateTODO.TabIndex = 290
		Me.btnCreateTODO.Text = "to-do erstellen"
		'
		'employeePicture
		'
		Me.employeePicture.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.employeePicture.Location = New System.Drawing.Point(539, 28)
		Me.employeePicture.Name = "employeePicture"
		Me.employeePicture.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.employeePicture.Properties.Appearance.Options.UseBackColor = True
		Me.employeePicture.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
		Me.employeePicture.Size = New System.Drawing.Size(120, 120)
		Me.employeePicture.TabIndex = 289
		'
		'lueState1
		'
		Me.lueState1.Location = New System.Drawing.Point(163, 289)
		Me.lueState1.MaximumSize = New System.Drawing.Size(190, 20)
		Me.lueState1.MinimumSize = New System.Drawing.Size(150, 20)
		Me.lueState1.Name = "lueState1"
		SerializableAppearanceObject1.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject1.Options.UseForeColor = True
		Me.lueState1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, "", Nothing, Nothing, True)})
		Me.lueState1.Properties.ShowFooter = False
		Me.lueState1.Size = New System.Drawing.Size(174, 20)
		Me.lueState1.TabIndex = 10
		'
		'lblstatus
		'
		Me.lblstatus.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblstatus.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblstatus.Location = New System.Drawing.Point(81, 293)
		Me.lblstatus.Name = "lblstatus"
		Me.lblstatus.Size = New System.Drawing.Size(75, 13)
		Me.lblstatus.TabIndex = 288
		Me.lblstatus.Text = "Status"
		'
		'lblHomepage
		'
		Me.lblHomepage.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblHomepage.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblHomepage.Location = New System.Drawing.Point(38, 238)
		Me.lblHomepage.Name = "lblHomepage"
		Me.lblHomepage.Size = New System.Drawing.Size(119, 13)
		Me.lblHomepage.TabIndex = 286
		Me.lblHomepage.Text = "Homepage"
		'
		'lblEMail
		'
		Me.lblEMail.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblEMail.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblEMail.Location = New System.Drawing.Point(38, 264)
		Me.lblEMail.Name = "lblEMail"
		Me.lblEMail.Size = New System.Drawing.Size(119, 13)
		Me.lblEMail.TabIndex = 285
		Me.lblEMail.Text = "E-Mail"
		'
		'lblErgebnis
		'
		Me.lblErgebnis.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblErgebnis.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblErgebnis.Location = New System.Drawing.Point(38, 315)
		Me.lblErgebnis.Name = "lblErgebnis"
		Me.lblErgebnis.Size = New System.Drawing.Size(119, 13)
		Me.lblErgebnis.TabIndex = 284
		Me.lblErgebnis.Text = "Ergebnis"
		'
		'lblvorschlag
		'
		Me.lblvorschlag.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblvorschlag.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblvorschlag.Location = New System.Drawing.Point(38, 433)
		Me.lblvorschlag.Name = "lblvorschlag"
		Me.lblvorschlag.Size = New System.Drawing.Size(119, 13)
		Me.lblvorschlag.TabIndex = 283
		Me.lblvorschlag.Text = "Vorschlag"
		'
		'lblvakanz
		'
		Me.lblvakanz.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblvakanz.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblvakanz.Location = New System.Drawing.Point(38, 407)
		Me.lblvakanz.Name = "lblvakanz"
		Me.lblvakanz.Size = New System.Drawing.Size(119, 13)
		Me.lblvakanz.TabIndex = 282
		Me.lblvakanz.Text = "Vakanz"
		'
		'txteMail
		'
		Me.txteMail.Location = New System.Drawing.Point(163, 260)
		Me.txteMail.Name = "txteMail"
		Me.txteMail.Size = New System.Drawing.Size(367, 20)
		Me.txteMail.TabIndex = 9
		'
		'txtHompage
		'
		Me.txtHompage.Location = New System.Drawing.Point(163, 234)
		Me.txtHompage.Name = "txtHompage"
		Me.txtHompage.Size = New System.Drawing.Size(367, 20)
		Me.txtHompage.TabIndex = 8
		'
		'lblZHD
		'
		Me.lblZHD.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblZHD.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblZHD.Location = New System.Drawing.Point(38, 120)
		Me.lblZHD.Name = "lblZHD"
		Me.lblZHD.Size = New System.Drawing.Size(119, 13)
		Me.lblZHD.TabIndex = 276
		Me.lblZHD.Text = "Zuständige Person"
		'
		'timeStart
		'
		Me.timeStart.EditValue = New Date(CType(0, Long))
		Me.timeStart.Location = New System.Drawing.Point(283, 64)
		Me.timeStart.Name = "timeStart"
		Me.timeStart.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.timeStart.Properties.DisplayFormat.FormatString = "HH:mm"
		Me.timeStart.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom
		Me.timeStart.Properties.EditFormat.FormatString = "HH:mm"
		Me.timeStart.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom
		Me.timeStart.Properties.Mask.EditMask = "HH:mm"
		Me.timeStart.Size = New System.Drawing.Size(54, 20)
		Me.timeStart.TabIndex = 2
		'
		'dateEditFrom
		'
		Me.dateEditFrom.EditValue = Nothing
		Me.dateEditFrom.Location = New System.Drawing.Point(163, 64)
		Me.dateEditFrom.Name = "dateEditFrom"
		SerializableAppearanceObject2.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject2.Options.UseForeColor = True
		Me.dateEditFrom.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject2, "", Nothing, Nothing, True)})
		Me.dateEditFrom.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.dateEditFrom.Properties.Mask.EditMask = ""
		Me.dateEditFrom.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None
		Me.dateEditFrom.Size = New System.Drawing.Size(104, 20)
		Me.dateEditFrom.TabIndex = 1
		'
		'lbldatum
		'
		Me.lbldatum.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lbldatum.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lbldatum.Location = New System.Drawing.Point(38, 68)
		Me.lbldatum.Name = "lbldatum"
		Me.lbldatum.Size = New System.Drawing.Size(119, 13)
		Me.lbldatum.TabIndex = 274
		Me.lbldatum.Text = "Datum"
		'
		'lblKunde
		'
		Me.lblKunde.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblKunde.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblKunde.Location = New System.Drawing.Point(38, 94)
		Me.lblKunde.Name = "lblKunde"
		Me.lblKunde.Size = New System.Drawing.Size(119, 13)
		Me.lblKunde.TabIndex = 273
		Me.lblKunde.Text = "Kunde"
		'
		'lblAdresse
		'
		Me.lblAdresse.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblAdresse.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblAdresse.Location = New System.Drawing.Point(38, 160)
		Me.lblAdresse.Name = "lblAdresse"
		Me.lblAdresse.Size = New System.Drawing.Size(119, 13)
		Me.lblAdresse.TabIndex = 269
		Me.lblAdresse.Text = "Adresse"
		'
		'txtTelefax
		'
		Me.txtTelefax.Location = New System.Drawing.Point(163, 208)
		Me.txtTelefax.Name = "txtTelefax"
		Me.txtTelefax.Size = New System.Drawing.Size(367, 20)
		Me.txtTelefax.TabIndex = 7
		'
		'txtTelefon
		'
		Me.txtTelefon.Location = New System.Drawing.Point(163, 182)
		Me.txtTelefon.Name = "txtTelefon"
		Me.txtTelefon.Size = New System.Drawing.Size(367, 20)
		Me.txtTelefon.TabIndex = 6
		'
		'txtAddress
		'
		Me.txtAddress.Location = New System.Drawing.Point(163, 156)
		Me.txtAddress.Name = "txtAddress"
		Me.txtAddress.Size = New System.Drawing.Size(367, 20)
		Me.txtAddress.TabIndex = 5
		'
		'lblTelefon
		'
		Me.lblTelefon.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblTelefon.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblTelefon.Location = New System.Drawing.Point(38, 186)
		Me.lblTelefon.Name = "lblTelefon"
		Me.lblTelefon.Size = New System.Drawing.Size(119, 13)
		Me.lblTelefon.TabIndex = 265
		Me.lblTelefon.Text = "Telefon"
		'
		'lblTelefax
		'
		Me.lblTelefax.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblTelefax.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblTelefax.Location = New System.Drawing.Point(38, 212)
		Me.lblTelefax.Name = "lblTelefax"
		Me.lblTelefax.Size = New System.Drawing.Size(119, 13)
		Me.lblTelefax.TabIndex = 264
		Me.lblTelefax.Text = "Telefax"
		'
		'txtInterviewAs
		'
		Me.txtInterviewAs.Location = New System.Drawing.Point(163, 38)
		Me.txtInterviewAs.Name = "txtInterviewAs"
		Me.txtInterviewAs.Size = New System.Drawing.Size(367, 20)
		Me.txtInterviewAs.TabIndex = 0
		'
		'lblVorstellungals
		'
		Me.lblVorstellungals.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblVorstellungals.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblVorstellungals.Location = New System.Drawing.Point(38, 42)
		Me.lblVorstellungals.Name = "lblVorstellungals"
		Me.lblVorstellungals.Size = New System.Drawing.Size(119, 13)
		Me.lblVorstellungals.TabIndex = 262
		Me.lblVorstellungals.Text = "Vorstellung als"
		'
		'btnDeleteInterview
		'
		Me.btnDeleteInterview.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnDeleteInterview.Image = CType(resources.GetObject("btnDeleteInterview.Image"), System.Drawing.Image)
		Me.btnDeleteInterview.Location = New System.Drawing.Point(554, 262)
		Me.btnDeleteInterview.Name = "btnDeleteInterview"
		Me.btnDeleteInterview.Size = New System.Drawing.Size(105, 28)
		Me.btnDeleteInterview.TabIndex = 16
		Me.btnDeleteInterview.Text = "Löschen"
		'
		'lblInterviewChanged
		'
		Me.lblInterviewChanged.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.lblInterviewChanged.Location = New System.Drawing.Point(163, 481)
		Me.lblInterviewChanged.Name = "lblInterviewChanged"
		Me.lblInterviewChanged.Size = New System.Drawing.Size(49, 13)
		Me.lblInterviewChanged.TabIndex = 53
		Me.lblInterviewChanged.Text = "Geändert:"
		'
		'lblInterviewCreated
		'
		Me.lblInterviewCreated.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.lblInterviewCreated.Location = New System.Drawing.Point(163, 462)
		Me.lblInterviewCreated.Name = "lblInterviewCreated"
		Me.lblInterviewCreated.Size = New System.Drawing.Size(37, 13)
		Me.lblInterviewCreated.TabIndex = 52
		Me.lblInterviewCreated.Text = "Erstellt:"
		'
		'lblgaendert
		'
		Me.lblgaendert.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.lblgaendert.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblgaendert.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblgaendert.Location = New System.Drawing.Point(79, 481)
		Me.lblgaendert.Name = "lblgaendert"
		Me.lblgaendert.Size = New System.Drawing.Size(77, 13)
		Me.lblgaendert.TabIndex = 51
		Me.lblgaendert.Text = "Geändert:"
		'
		'lblerstellt
		'
		Me.lblerstellt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.lblerstellt.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblerstellt.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblerstellt.Location = New System.Drawing.Point(79, 462)
		Me.lblerstellt.Name = "lblerstellt"
		Me.lblerstellt.Size = New System.Drawing.Size(77, 13)
		Me.lblerstellt.TabIndex = 50
		Me.lblerstellt.Text = "Erstellt:"
		'
		'btnNewInterview
		'
		Me.btnNewInterview.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnNewInterview.Image = CType(resources.GetObject("btnNewInterview.Image"), System.Drawing.Image)
		Me.btnNewInterview.Location = New System.Drawing.Point(554, 225)
		Me.btnNewInterview.Name = "btnNewInterview"
		Me.btnNewInterview.Size = New System.Drawing.Size(105, 28)
		Me.btnNewInterview.TabIndex = 15
		Me.btnNewInterview.Text = "Neu"
		'
		'btnSave
		'
		Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnSave.Image = CType(resources.GetObject("btnSave.Image"), System.Drawing.Image)
		Me.btnSave.Location = New System.Drawing.Point(554, 188)
		Me.btnSave.Name = "btnSave"
		Me.btnSave.Size = New System.Drawing.Size(105, 28)
		Me.btnSave.TabIndex = 14
		Me.btnSave.Text = "Speichern"
		'
		'txtResult
		'
		Me.txtResult.Location = New System.Drawing.Point(163, 315)
		Me.txtResult.Name = "txtResult"
		Me.txtResult.Size = New System.Drawing.Size(367, 82)
		Me.txtResult.TabIndex = 11
		'
		'lueZHDName
		'
		Me.lueZHDName.Location = New System.Drawing.Point(163, 116)
		Me.lueZHDName.Name = "lueZHDName"
		SerializableAppearanceObject3.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject3.Options.UseForeColor = True
		Me.lueZHDName.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject3, "", Nothing, Nothing, True)})
		Me.lueZHDName.Properties.ShowFooter = False
		Me.lueZHDName.Properties.View = Me.gvZHDName
		Me.lueZHDName.Size = New System.Drawing.Size(367, 20)
		Me.lueZHDName.TabIndex = 4
		'
		'gvZHDName
		'
		Me.gvZHDName.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvZHDName.Name = "gvZHDName"
		Me.gvZHDName.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvZHDName.OptionsView.ShowGroupPanel = False
		'
		'lueCustomerName
		'
		Me.lueCustomerName.Location = New System.Drawing.Point(163, 90)
		Me.lueCustomerName.Name = "lueCustomerName"
		SerializableAppearanceObject4.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject4.Options.UseForeColor = True
		Me.lueCustomerName.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject4, "", Nothing, Nothing, True)})
		Me.lueCustomerName.Properties.ShowFooter = False
		Me.lueCustomerName.Properties.View = Me.gvCustomer
		Me.lueCustomerName.Size = New System.Drawing.Size(367, 20)
		Me.lueCustomerName.TabIndex = 3
		'
		'gvCustomer
		'
		Me.gvCustomer.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvCustomer.Name = "gvCustomer"
		Me.gvCustomer.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvCustomer.OptionsView.ShowGroupPanel = False
		'
		'lueVacancy
		'
		Me.lueVacancy.Location = New System.Drawing.Point(163, 403)
		Me.lueVacancy.Name = "lueVacancy"
		SerializableAppearanceObject5.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject5.Options.UseForeColor = True
		Me.lueVacancy.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject5, "", Nothing, Nothing, True)})
		Me.lueVacancy.Properties.ShowFooter = False
		Me.lueVacancy.Properties.View = Me.gvVacancy
		Me.lueVacancy.Size = New System.Drawing.Size(367, 20)
		Me.lueVacancy.TabIndex = 12
		'
		'gvVacancy
		'
		Me.gvVacancy.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvVacancy.Name = "gvVacancy"
		Me.gvVacancy.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvVacancy.OptionsView.ShowGroupPanel = False
		'
		'luePropose
		'
		Me.luePropose.Location = New System.Drawing.Point(163, 429)
		Me.luePropose.Name = "luePropose"
		SerializableAppearanceObject6.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject6.Options.UseForeColor = True
		Me.luePropose.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject6, "", Nothing, Nothing, True)})
		Me.luePropose.Properties.ShowFooter = False
		Me.luePropose.Properties.View = Me.gvPropose
		Me.luePropose.Size = New System.Drawing.Size(367, 20)
		Me.luePropose.TabIndex = 13
		'
		'gvPropose
		'
		Me.gvPropose.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvPropose.Name = "gvPropose"
		Me.gvPropose.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvPropose.OptionsView.ShowGroupPanel = False
		'
		'errorProviderJobInterviewMng
		'
		Me.errorProviderJobInterviewMng.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
		Me.errorProviderJobInterviewMng.ContainerControl = Me
		'
		'frmJobInterview
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(1031, 521)
		Me.Controls.Add(Me.sccMain)
		Me.MinimumSize = New System.Drawing.Size(1047, 559)
		Me.Name = "frmJobInterview"
		Me.Padding = New System.Windows.Forms.Padding(5)
		Me.Text = "Vorstellungsgespräche"
		CType(Me.sccMain, System.ComponentModel.ISupportInitialize).EndInit()
		Me.sccMain.ResumeLayout(False)
		CType(Me.gridInterview, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvInterview, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.employeePicture.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lueState1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txteMail.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtHompage.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.timeStart.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.dateEditFrom.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.dateEditFrom.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtTelefax.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtTelefon.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtAddress.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtInterviewAs.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtResult.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lueZHDName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvZHDName, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lueCustomerName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvCustomer, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lueVacancy.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvVacancy, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.luePropose.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvPropose, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.errorProviderJobInterviewMng, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents sccMain As DevExpress.XtraEditors.SplitContainerControl
	Friend WithEvents gridInterview As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvInterview As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents btnDeleteInterview As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents lblInterviewChanged As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblInterviewCreated As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblgaendert As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblerstellt As DevExpress.XtraEditors.LabelControl
	Friend WithEvents btnNewInterview As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents btnSave As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents lblAdresse As DevExpress.XtraEditors.LabelControl
	Friend WithEvents txtTelefax As DevExpress.XtraEditors.TextEdit
	Friend WithEvents txtTelefon As DevExpress.XtraEditors.TextEdit
	Friend WithEvents txtAddress As DevExpress.XtraEditors.TextEdit
	Friend WithEvents lblTelefon As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblTelefax As DevExpress.XtraEditors.LabelControl
	Friend WithEvents txtInterviewAs As DevExpress.XtraEditors.TextEdit
	Friend WithEvents lblVorstellungals As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblZHD As DevExpress.XtraEditors.LabelControl
	Private WithEvents timeStart As DevExpress.XtraEditors.TimeEdit
	Friend WithEvents dateEditFrom As DevExpress.XtraEditors.DateEdit
	Friend WithEvents lbldatum As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblKunde As DevExpress.XtraEditors.LabelControl
	Friend WithEvents txteMail As DevExpress.XtraEditors.TextEdit
	Friend WithEvents txtHompage As DevExpress.XtraEditors.TextEdit
	Friend WithEvents txtResult As DevExpress.XtraEditors.MemoEdit
	Friend WithEvents lblHomepage As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblEMail As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblErgebnis As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblvorschlag As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblvakanz As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lueState1 As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents lblstatus As DevExpress.XtraEditors.LabelControl
	Friend WithEvents employeePicture As DevExpress.XtraEditors.PictureEdit
	Friend WithEvents errorProviderJobInterviewMng As System.Windows.Forms.ErrorProvider
	Friend WithEvents btnCreateTODO As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents lueZHDName As DevExpress.XtraEditors.GridLookUpEdit
	Friend WithEvents gvZHDName As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents lueCustomerName As DevExpress.XtraEditors.GridLookUpEdit
	Friend WithEvents gvCustomer As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents lueVacancy As DevExpress.XtraEditors.GridLookUpEdit
	Friend WithEvents gvVacancy As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents luePropose As DevExpress.XtraEditors.GridLookUpEdit
	Friend WithEvents gvPropose As DevExpress.XtraGrid.Views.Grid.GridView
End Class
