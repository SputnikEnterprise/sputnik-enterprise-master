Namespace CVLizer.UI

	<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
	Partial Class ucCVLPersonal
		Inherits ucBaseControl

		'UserControl overrides dispose to clean up the component list.
		<System.Diagnostics.DebuggerNonUserCode()>
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
		<System.Diagnostics.DebuggerStepThrough()>
		Private Sub InitializeComponent()
			Me.XtraTabControl3 = New DevExpress.XtraTab.XtraTabControl()
			Me.xtabAllgemein = New DevExpress.XtraTab.XtraTabPage()
			Me.lblCivilState = New DevExpress.XtraEditors.LabelControl()
			Me.txtApplicantCreatedOn = New DevExpress.XtraEditors.DateEdit()
			Me.lblApplicantCreatedOn = New DevExpress.XtraEditors.LabelControl()
			Me.txtApplicantCivilstate = New DevExpress.XtraEditors.TextEdit()
			Me.LabelControl3 = New DevExpress.XtraEditors.LabelControl()
			Me.lstTelefax = New DevExpress.XtraEditors.ListBoxControl()
			Me.lstHomepages = New DevExpress.XtraEditors.ListBoxControl()
			Me.lblEMail = New DevExpress.XtraEditors.LabelControl()
			Me.lblTelephone = New DevExpress.XtraEditors.LabelControl()
			Me.lstTelefon = New DevExpress.XtraEditors.ListBoxControl()
			Me.lstEMails = New DevExpress.XtraEditors.ListBoxControl()
			Me.lstTiltles = New DevExpress.XtraEditors.ListBoxControl()
			Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl()
			Me.lblMobilePhone = New DevExpress.XtraEditors.LabelControl()
			Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl()
			Me.peProfilePicture = New DevExpress.XtraEditors.PictureEdit()
			Me.lblGender = New DevExpress.XtraEditors.LabelControl()
			Me.txtApplicantBirthDate = New DevExpress.XtraEditors.DateEdit()
			Me.txtApplicantLastName = New DevExpress.XtraEditors.TextEdit()
			Me.lblLastname = New DevExpress.XtraEditors.LabelControl()
			Me.txtApplicantFirstname = New DevExpress.XtraEditors.TextEdit()
			Me.lblFirstname = New DevExpress.XtraEditors.LabelControl()
			Me.txtApplicantStreet = New DevExpress.XtraEditors.TextEdit()
			Me.lblStreet = New DevExpress.XtraEditors.LabelControl()
			Me.txtApplicantPostofficeBox = New DevExpress.XtraEditors.TextEdit()
			Me.lblPostOfficeBox = New DevExpress.XtraEditors.LabelControl()
			Me.txtApplicantPostcode = New DevExpress.XtraEditors.TextEdit()
			Me.lblPostcode = New DevExpress.XtraEditors.LabelControl()
			Me.txtApplicantLocation = New DevExpress.XtraEditors.TextEdit()
			Me.lblLocation = New DevExpress.XtraEditors.LabelControl()
			Me.txtApplicantCountry = New DevExpress.XtraEditors.TextEdit()
			Me.lblCountry = New DevExpress.XtraEditors.LabelControl()
			Me.txtApplicantNationality = New DevExpress.XtraEditors.TextEdit()
			Me.lblNationality = New DevExpress.XtraEditors.LabelControl()
			Me.lblBirthdate = New DevExpress.XtraEditors.LabelControl()
			Me.TextEdit1 = New DevExpress.XtraEditors.ComboBoxEdit()
			Me.cboGender = New DevExpress.XtraEditors.ComboBoxEdit()
			Me.lblAge = New DevExpress.XtraEditors.LabelControl()
			CType(Me.XtraTabControl3, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.XtraTabControl3.SuspendLayout()
			Me.xtabAllgemein.SuspendLayout()
			CType(Me.txtApplicantCreatedOn.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.txtApplicantCreatedOn.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.txtApplicantCivilstate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.lstTelefax, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.lstHomepages, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.lstTelefon, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.lstEMails, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.lstTiltles, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.peProfilePicture.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.txtApplicantBirthDate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.txtApplicantBirthDate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.txtApplicantLastName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.txtApplicantFirstname.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.txtApplicantStreet.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.txtApplicantPostofficeBox.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.txtApplicantPostcode.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.txtApplicantLocation.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.txtApplicantCountry.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.txtApplicantNationality.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.TextEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.cboGender.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			'
			'XtraTabControl3
			'
			Me.XtraTabControl3.Dock = System.Windows.Forms.DockStyle.Fill
			Me.XtraTabControl3.Location = New System.Drawing.Point(0, 0)
			Me.XtraTabControl3.Name = "XtraTabControl3"
			Me.XtraTabControl3.SelectedTabPage = Me.xtabAllgemein
			Me.XtraTabControl3.Size = New System.Drawing.Size(1029, 380)
			Me.XtraTabControl3.TabIndex = 60
			Me.XtraTabControl3.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabAllgemein})
			'
			'xtabAllgemein
			'
			Me.xtabAllgemein.Controls.Add(Me.lblAge)
			Me.xtabAllgemein.Controls.Add(Me.lblCivilState)
			Me.xtabAllgemein.Controls.Add(Me.txtApplicantCreatedOn)
			Me.xtabAllgemein.Controls.Add(Me.lblApplicantCreatedOn)
			Me.xtabAllgemein.Controls.Add(Me.txtApplicantCivilstate)
			Me.xtabAllgemein.Controls.Add(Me.LabelControl3)
			Me.xtabAllgemein.Controls.Add(Me.lstTelefax)
			Me.xtabAllgemein.Controls.Add(Me.lstHomepages)
			Me.xtabAllgemein.Controls.Add(Me.lblEMail)
			Me.xtabAllgemein.Controls.Add(Me.lblTelephone)
			Me.xtabAllgemein.Controls.Add(Me.lstTelefon)
			Me.xtabAllgemein.Controls.Add(Me.lstEMails)
			Me.xtabAllgemein.Controls.Add(Me.lstTiltles)
			Me.xtabAllgemein.Controls.Add(Me.LabelControl2)
			Me.xtabAllgemein.Controls.Add(Me.lblMobilePhone)
			Me.xtabAllgemein.Controls.Add(Me.LabelControl1)
			Me.xtabAllgemein.Controls.Add(Me.peProfilePicture)
			Me.xtabAllgemein.Controls.Add(Me.lblGender)
			Me.xtabAllgemein.Controls.Add(Me.txtApplicantBirthDate)
			Me.xtabAllgemein.Controls.Add(Me.txtApplicantLastName)
			Me.xtabAllgemein.Controls.Add(Me.lblLastname)
			Me.xtabAllgemein.Controls.Add(Me.txtApplicantFirstname)
			Me.xtabAllgemein.Controls.Add(Me.lblFirstname)
			Me.xtabAllgemein.Controls.Add(Me.txtApplicantStreet)
			Me.xtabAllgemein.Controls.Add(Me.lblStreet)
			Me.xtabAllgemein.Controls.Add(Me.txtApplicantPostofficeBox)
			Me.xtabAllgemein.Controls.Add(Me.lblPostOfficeBox)
			Me.xtabAllgemein.Controls.Add(Me.txtApplicantPostcode)
			Me.xtabAllgemein.Controls.Add(Me.lblPostcode)
			Me.xtabAllgemein.Controls.Add(Me.txtApplicantLocation)
			Me.xtabAllgemein.Controls.Add(Me.lblLocation)
			Me.xtabAllgemein.Controls.Add(Me.txtApplicantCountry)
			Me.xtabAllgemein.Controls.Add(Me.lblCountry)
			Me.xtabAllgemein.Controls.Add(Me.txtApplicantNationality)
			Me.xtabAllgemein.Controls.Add(Me.lblNationality)
			Me.xtabAllgemein.Controls.Add(Me.lblBirthdate)
			Me.xtabAllgemein.Controls.Add(Me.TextEdit1)
			Me.xtabAllgemein.Controls.Add(Me.cboGender)
			Me.xtabAllgemein.Name = "xtabAllgemein"
			Me.xtabAllgemein.Padding = New System.Windows.Forms.Padding(5)
			Me.xtabAllgemein.Size = New System.Drawing.Size(1023, 352)
			Me.xtabAllgemein.Text = "Allgemein"
			'
			'lblCivilState
			'
			Me.lblCivilState.Appearance.Options.UseTextOptions = True
			Me.lblCivilState.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblCivilState.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblCivilState.Location = New System.Drawing.Point(389, 205)
			Me.lblCivilState.Name = "lblCivilState"
			Me.lblCivilState.Size = New System.Drawing.Size(99, 13)
			Me.lblCivilState.TabIndex = 336
			Me.lblCivilState.Text = "Zivilstand"
			'
			'txtApplicantCreatedOn
			'
			Me.txtApplicantCreatedOn.EditValue = Nothing
			Me.txtApplicantCreatedOn.Location = New System.Drawing.Point(494, 306)
			Me.txtApplicantCreatedOn.Name = "txtApplicantCreatedOn"
			Me.txtApplicantCreatedOn.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.txtApplicantCreatedOn.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.txtApplicantCreatedOn.Properties.DisplayFormat.FormatString = ""
			Me.txtApplicantCreatedOn.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
			Me.txtApplicantCreatedOn.Properties.EditFormat.FormatString = ""
			Me.txtApplicantCreatedOn.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime
			Me.txtApplicantCreatedOn.Properties.Mask.EditMask = ""
			Me.txtApplicantCreatedOn.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None
			Me.txtApplicantCreatedOn.Size = New System.Drawing.Size(147, 20)
			Me.txtApplicantCreatedOn.TabIndex = 44
			'
			'lblApplicantCreatedOn
			'
			Me.lblApplicantCreatedOn.Appearance.Options.UseTextOptions = True
			Me.lblApplicantCreatedOn.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblApplicantCreatedOn.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblApplicantCreatedOn.Location = New System.Drawing.Point(389, 308)
			Me.lblApplicantCreatedOn.Name = "lblApplicantCreatedOn"
			Me.lblApplicantCreatedOn.Size = New System.Drawing.Size(99, 13)
			Me.lblApplicantCreatedOn.TabIndex = 45
			Me.lblApplicantCreatedOn.Text = "Erstellt"
			'
			'txtApplicantCivilstate
			'
			Me.txtApplicantCivilstate.Location = New System.Drawing.Point(494, 202)
			Me.txtApplicantCivilstate.Name = "txtApplicantCivilstate"
			Me.txtApplicantCivilstate.Size = New System.Drawing.Size(187, 20)
			Me.txtApplicantCivilstate.TabIndex = 335
			'
			'LabelControl3
			'
			Me.LabelControl3.Appearance.Options.UseTextOptions = True
			Me.LabelControl3.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.LabelControl3.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.LabelControl3.Location = New System.Drawing.Point(20, 305)
			Me.LabelControl3.Name = "LabelControl3"
			Me.LabelControl3.Size = New System.Drawing.Size(99, 13)
			Me.LabelControl3.TabIndex = 333
			Me.LabelControl3.Text = "Telefax"
			'
			'lstTelefax
			'
			Me.lstTelefax.Location = New System.Drawing.Point(125, 305)
			Me.lstTelefax.Name = "lstTelefax"
			Me.lstTelefax.Size = New System.Drawing.Size(187, 36)
			Me.lstTelefax.TabIndex = 334
			'
			'lstHomepages
			'
			Me.lstHomepages.Cursor = System.Windows.Forms.Cursors.Default
			Me.lstHomepages.Location = New System.Drawing.Point(792, 240)
			Me.lstHomepages.Name = "lstHomepages"
			Me.lstHomepages.Size = New System.Drawing.Size(187, 36)
			Me.lstHomepages.TabIndex = 332
			'
			'lblEMail
			'
			Me.lblEMail.Appearance.Options.UseTextOptions = True
			Me.lblEMail.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblEMail.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblEMail.Location = New System.Drawing.Point(389, 242)
			Me.lblEMail.Name = "lblEMail"
			Me.lblEMail.Size = New System.Drawing.Size(99, 13)
			Me.lblEMail.TabIndex = 19
			Me.lblEMail.Text = "EMail"
			'
			'lblTelephone
			'
			Me.lblTelephone.Appearance.Options.UseTextOptions = True
			Me.lblTelephone.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblTelephone.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblTelephone.Location = New System.Drawing.Point(20, 242)
			Me.lblTelephone.Name = "lblTelephone"
			Me.lblTelephone.Size = New System.Drawing.Size(99, 13)
			Me.lblTelephone.TabIndex = 21
			Me.lblTelephone.Text = "Telefon"
			'
			'lstTelefon
			'
			Me.lstTelefon.Cursor = System.Windows.Forms.Cursors.Default
			Me.lstTelefon.Location = New System.Drawing.Point(125, 242)
			Me.lstTelefon.Name = "lstTelefon"
			Me.lstTelefon.Size = New System.Drawing.Size(187, 55)
			Me.lstTelefon.TabIndex = 331
			'
			'lstEMails
			'
			Me.lstEMails.Location = New System.Drawing.Point(494, 242)
			Me.lstEMails.Name = "lstEMails"
			Me.lstEMails.Size = New System.Drawing.Size(187, 55)
			Me.lstEMails.TabIndex = 330
			'
			'lstTiltles
			'
			Me.lstTiltles.Cursor = System.Windows.Forms.Cursors.Default
			Me.lstTiltles.Location = New System.Drawing.Point(494, 46)
			Me.lstTiltles.Name = "lstTiltles"
			Me.lstTiltles.Size = New System.Drawing.Size(187, 55)
			Me.lstTiltles.TabIndex = 329
			'
			'LabelControl2
			'
			Me.LabelControl2.Appearance.Options.UseTextOptions = True
			Me.LabelControl2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.LabelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.LabelControl2.Location = New System.Drawing.Point(389, 48)
			Me.LabelControl2.Name = "LabelControl2"
			Me.LabelControl2.Size = New System.Drawing.Size(99, 13)
			Me.LabelControl2.TabIndex = 60
			Me.LabelControl2.Text = "Titles"
			'
			'lblMobilePhone
			'
			Me.lblMobilePhone.Appearance.Options.UseTextOptions = True
			Me.lblMobilePhone.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblMobilePhone.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblMobilePhone.Location = New System.Drawing.Point(687, 242)
			Me.lblMobilePhone.Name = "lblMobilePhone"
			Me.lblMobilePhone.Size = New System.Drawing.Size(99, 13)
			Me.lblMobilePhone.TabIndex = 23
			Me.lblMobilePhone.Text = "Homepage"
			'
			'LabelControl1
			'
			Me.LabelControl1.Appearance.Options.UseTextOptions = True
			Me.LabelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.LabelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.LabelControl1.Location = New System.Drawing.Point(389, 23)
			Me.LabelControl1.Name = "LabelControl1"
			Me.LabelControl1.Size = New System.Drawing.Size(99, 13)
			Me.LabelControl1.TabIndex = 58
			Me.LabelControl1.Text = "Status"
			'
			'peProfilePicture
			'
			Me.peProfilePicture.Cursor = System.Windows.Forms.Cursors.Default
			Me.peProfilePicture.Location = New System.Drawing.Point(792, 9)
			Me.peProfilePicture.Name = "peProfilePicture"
			Me.peProfilePicture.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.[Auto]
			Me.peProfilePicture.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze
			Me.peProfilePicture.Size = New System.Drawing.Size(153, 162)
			Me.peProfilePicture.TabIndex = 53
			'
			'lblGender
			'
			Me.lblGender.Appearance.Options.UseTextOptions = True
			Me.lblGender.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblGender.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblGender.Location = New System.Drawing.Point(20, 23)
			Me.lblGender.Name = "lblGender"
			Me.lblGender.Size = New System.Drawing.Size(99, 13)
			Me.lblGender.TabIndex = 5
			Me.lblGender.Text = "Geschlecht"
			'
			'txtApplicantBirthDate
			'
			Me.txtApplicantBirthDate.EditValue = ""
			Me.txtApplicantBirthDate.Location = New System.Drawing.Point(125, 203)
			Me.txtApplicantBirthDate.Name = "txtApplicantBirthDate"
			Me.txtApplicantBirthDate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.txtApplicantBirthDate.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.txtApplicantBirthDate.Properties.DisplayFormat.FormatString = ""
			Me.txtApplicantBirthDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
			Me.txtApplicantBirthDate.Properties.EditFormat.FormatString = ""
			Me.txtApplicantBirthDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime
			Me.txtApplicantBirthDate.Properties.Mask.EditMask = ""
			Me.txtApplicantBirthDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None
			Me.txtApplicantBirthDate.Size = New System.Drawing.Size(87, 20)
			Me.txtApplicantBirthDate.TabIndex = 24
			'
			'txtApplicantLastName
			'
			Me.txtApplicantLastName.Location = New System.Drawing.Point(125, 47)
			Me.txtApplicantLastName.Name = "txtApplicantLastName"
			Me.txtApplicantLastName.Size = New System.Drawing.Size(187, 20)
			Me.txtApplicantLastName.TabIndex = 0
			'
			'lblLastname
			'
			Me.lblLastname.Appearance.Options.UseTextOptions = True
			Me.lblLastname.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblLastname.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblLastname.Location = New System.Drawing.Point(20, 49)
			Me.lblLastname.Name = "lblLastname"
			Me.lblLastname.Size = New System.Drawing.Size(99, 13)
			Me.lblLastname.TabIndex = 1
			Me.lblLastname.Text = "Nachname"
			'
			'txtApplicantFirstname
			'
			Me.txtApplicantFirstname.Location = New System.Drawing.Point(125, 73)
			Me.txtApplicantFirstname.Name = "txtApplicantFirstname"
			Me.txtApplicantFirstname.Size = New System.Drawing.Size(187, 20)
			Me.txtApplicantFirstname.TabIndex = 2
			'
			'lblFirstname
			'
			Me.lblFirstname.Appearance.Options.UseTextOptions = True
			Me.lblFirstname.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblFirstname.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblFirstname.Location = New System.Drawing.Point(20, 75)
			Me.lblFirstname.Name = "lblFirstname"
			Me.lblFirstname.Size = New System.Drawing.Size(99, 13)
			Me.lblFirstname.TabIndex = 3
			Me.lblFirstname.Text = "Vorname"
			'
			'txtApplicantStreet
			'
			Me.txtApplicantStreet.Location = New System.Drawing.Point(125, 125)
			Me.txtApplicantStreet.Name = "txtApplicantStreet"
			Me.txtApplicantStreet.Size = New System.Drawing.Size(187, 20)
			Me.txtApplicantStreet.TabIndex = 6
			'
			'lblStreet
			'
			Me.lblStreet.Appearance.Options.UseTextOptions = True
			Me.lblStreet.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblStreet.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblStreet.Location = New System.Drawing.Point(20, 127)
			Me.lblStreet.Name = "lblStreet"
			Me.lblStreet.Size = New System.Drawing.Size(99, 13)
			Me.lblStreet.TabIndex = 7
			Me.lblStreet.Text = "Strasse"
			'
			'txtApplicantPostofficeBox
			'
			Me.txtApplicantPostofficeBox.Location = New System.Drawing.Point(125, 99)
			Me.txtApplicantPostofficeBox.Name = "txtApplicantPostofficeBox"
			Me.txtApplicantPostofficeBox.Size = New System.Drawing.Size(187, 20)
			Me.txtApplicantPostofficeBox.TabIndex = 8
			'
			'lblPostOfficeBox
			'
			Me.lblPostOfficeBox.Appearance.Options.UseTextOptions = True
			Me.lblPostOfficeBox.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblPostOfficeBox.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblPostOfficeBox.Location = New System.Drawing.Point(20, 101)
			Me.lblPostOfficeBox.Name = "lblPostOfficeBox"
			Me.lblPostOfficeBox.Size = New System.Drawing.Size(99, 13)
			Me.lblPostOfficeBox.TabIndex = 9
			Me.lblPostOfficeBox.Text = "Postfach"
			'
			'txtApplicantPostcode
			'
			Me.txtApplicantPostcode.Location = New System.Drawing.Point(494, 151)
			Me.txtApplicantPostcode.Name = "txtApplicantPostcode"
			Me.txtApplicantPostcode.Size = New System.Drawing.Size(87, 20)
			Me.txtApplicantPostcode.TabIndex = 10
			'
			'lblPostcode
			'
			Me.lblPostcode.Appearance.Options.UseTextOptions = True
			Me.lblPostcode.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblPostcode.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblPostcode.Location = New System.Drawing.Point(389, 153)
			Me.lblPostcode.Name = "lblPostcode"
			Me.lblPostcode.Size = New System.Drawing.Size(99, 13)
			Me.lblPostcode.TabIndex = 11
			Me.lblPostcode.Text = "PLZ"
			'
			'txtApplicantLocation
			'
			Me.txtApplicantLocation.Location = New System.Drawing.Point(125, 177)
			Me.txtApplicantLocation.Name = "txtApplicantLocation"
			Me.txtApplicantLocation.Size = New System.Drawing.Size(187, 20)
			Me.txtApplicantLocation.TabIndex = 12
			'
			'lblLocation
			'
			Me.lblLocation.Appearance.Options.UseTextOptions = True
			Me.lblLocation.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblLocation.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblLocation.Location = New System.Drawing.Point(20, 179)
			Me.lblLocation.Name = "lblLocation"
			Me.lblLocation.Size = New System.Drawing.Size(99, 13)
			Me.lblLocation.TabIndex = 13
			Me.lblLocation.Text = "Ort"
			'
			'txtApplicantCountry
			'
			Me.txtApplicantCountry.Location = New System.Drawing.Point(125, 151)
			Me.txtApplicantCountry.Name = "txtApplicantCountry"
			Me.txtApplicantCountry.Size = New System.Drawing.Size(87, 20)
			Me.txtApplicantCountry.TabIndex = 14
			'
			'lblCountry
			'
			Me.lblCountry.Appearance.Options.UseTextOptions = True
			Me.lblCountry.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblCountry.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblCountry.Location = New System.Drawing.Point(20, 153)
			Me.lblCountry.Name = "lblCountry"
			Me.lblCountry.Size = New System.Drawing.Size(99, 13)
			Me.lblCountry.TabIndex = 15
			Me.lblCountry.Text = "Land"
			'
			'txtApplicantNationality
			'
			Me.txtApplicantNationality.Location = New System.Drawing.Point(494, 176)
			Me.txtApplicantNationality.Name = "txtApplicantNationality"
			Me.txtApplicantNationality.Size = New System.Drawing.Size(187, 20)
			Me.txtApplicantNationality.TabIndex = 16
			'
			'lblNationality
			'
			Me.lblNationality.Appearance.Options.UseTextOptions = True
			Me.lblNationality.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblNationality.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblNationality.Location = New System.Drawing.Point(389, 178)
			Me.lblNationality.Name = "lblNationality"
			Me.lblNationality.Size = New System.Drawing.Size(99, 13)
			Me.lblNationality.TabIndex = 17
			Me.lblNationality.Text = "Nationalität"
			'
			'lblBirthdate
			'
			Me.lblBirthdate.Appearance.Options.UseTextOptions = True
			Me.lblBirthdate.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblBirthdate.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblBirthdate.Location = New System.Drawing.Point(20, 207)
			Me.lblBirthdate.Name = "lblBirthdate"
			Me.lblBirthdate.Size = New System.Drawing.Size(99, 13)
			Me.lblBirthdate.TabIndex = 25
			Me.lblBirthdate.Text = "Geburtsdatum"
			'
			'TextEdit1
			'
			Me.TextEdit1.Location = New System.Drawing.Point(494, 21)
			Me.TextEdit1.Name = "TextEdit1"
			Me.TextEdit1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.TextEdit1.Size = New System.Drawing.Size(187, 20)
			Me.TextEdit1.TabIndex = 57
			'
			'cboGender
			'
			Me.cboGender.EditValue = ""
			Me.cboGender.Location = New System.Drawing.Point(125, 21)
			Me.cboGender.Name = "cboGender"
			Me.cboGender.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.cboGender.Size = New System.Drawing.Size(187, 20)
			Me.cboGender.TabIndex = 4
			'
			'lblAge
			'
			Me.lblAge.Location = New System.Drawing.Point(218, 207)
			Me.lblAge.Name = "lblAge"
			Me.lblAge.Size = New System.Drawing.Size(29, 13)
			Me.lblAge.TabIndex = 337
			Me.lblAge.Text = "lblAge"
			'
			'ucCVLPersonal
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.Controls.Add(Me.XtraTabControl3)
			Me.Name = "ucCVLPersonal"
			Me.Size = New System.Drawing.Size(1029, 380)
			CType(Me.XtraTabControl3, System.ComponentModel.ISupportInitialize).EndInit()
			Me.XtraTabControl3.ResumeLayout(False)
			Me.xtabAllgemein.ResumeLayout(False)
			Me.xtabAllgemein.PerformLayout()
			CType(Me.txtApplicantCreatedOn.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.txtApplicantCreatedOn.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.txtApplicantCivilstate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.lstTelefax, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.lstHomepages, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.lstTelefon, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.lstEMails, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.lstTiltles, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.peProfilePicture.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.txtApplicantBirthDate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.txtApplicantBirthDate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.txtApplicantLastName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.txtApplicantFirstname.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.txtApplicantStreet.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.txtApplicantPostofficeBox.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.txtApplicantPostcode.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.txtApplicantLocation.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.txtApplicantCountry.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.txtApplicantNationality.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.TextEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.cboGender.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		Friend WithEvents XtraTabControl3 As DevExpress.XtraTab.XtraTabControl
		Friend WithEvents xtabAllgemein As DevExpress.XtraTab.XtraTabPage
		Friend WithEvents lblCivilState As DevExpress.XtraEditors.LabelControl
		Friend WithEvents txtApplicantCivilstate As DevExpress.XtraEditors.TextEdit
		Friend WithEvents LabelControl3 As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lstTelefax As DevExpress.XtraEditors.ListBoxControl
		Friend WithEvents lstHomepages As DevExpress.XtraEditors.ListBoxControl
		Friend WithEvents lblEMail As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblTelephone As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lstTelefon As DevExpress.XtraEditors.ListBoxControl
		Friend WithEvents lstEMails As DevExpress.XtraEditors.ListBoxControl
		Friend WithEvents lstTiltles As DevExpress.XtraEditors.ListBoxControl
		Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblMobilePhone As DevExpress.XtraEditors.LabelControl
		Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
		Friend WithEvents peProfilePicture As DevExpress.XtraEditors.PictureEdit
		Friend WithEvents lblGender As DevExpress.XtraEditors.LabelControl
		Friend WithEvents txtApplicantBirthDate As DevExpress.XtraEditors.DateEdit
		Friend WithEvents txtApplicantLastName As DevExpress.XtraEditors.TextEdit
		Friend WithEvents lblLastname As DevExpress.XtraEditors.LabelControl
		Friend WithEvents txtApplicantFirstname As DevExpress.XtraEditors.TextEdit
		Friend WithEvents lblFirstname As DevExpress.XtraEditors.LabelControl
		Friend WithEvents txtApplicantStreet As DevExpress.XtraEditors.TextEdit
		Friend WithEvents lblStreet As DevExpress.XtraEditors.LabelControl
		Friend WithEvents txtApplicantPostofficeBox As DevExpress.XtraEditors.TextEdit
		Friend WithEvents lblPostOfficeBox As DevExpress.XtraEditors.LabelControl
		Friend WithEvents txtApplicantPostcode As DevExpress.XtraEditors.TextEdit
		Friend WithEvents lblPostcode As DevExpress.XtraEditors.LabelControl
		Friend WithEvents txtApplicantLocation As DevExpress.XtraEditors.TextEdit
		Friend WithEvents lblLocation As DevExpress.XtraEditors.LabelControl
		Friend WithEvents txtApplicantCountry As DevExpress.XtraEditors.TextEdit
		Friend WithEvents lblCountry As DevExpress.XtraEditors.LabelControl
		Friend WithEvents txtApplicantNationality As DevExpress.XtraEditors.TextEdit
		Friend WithEvents lblNationality As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblBirthdate As DevExpress.XtraEditors.LabelControl
		Friend WithEvents TextEdit1 As DevExpress.XtraEditors.ComboBoxEdit
		Friend WithEvents cboGender As DevExpress.XtraEditors.ComboBoxEdit
		Friend WithEvents lblApplicantCreatedOn As DevExpress.XtraEditors.LabelControl
		Friend WithEvents txtApplicantCreatedOn As DevExpress.XtraEditors.DateEdit
		Friend WithEvents lblAge As DevExpress.XtraEditors.LabelControl
	End Class


End Namespace
