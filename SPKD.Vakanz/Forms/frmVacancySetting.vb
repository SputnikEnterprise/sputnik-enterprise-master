
Imports System.Text.RegularExpressions
Imports System.IO

Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Vacancy
Imports SP.DatabaseAccess.Vacancy.DataObjects
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SPKD.Vakanz.ClsDataDetail


Public Class frmVacancySetting


#Region "private fields"

	Private m_Logger As ILogger = New Logger()

	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private m_VacancyDatabaseAccess As IVacancyDatabaseAccess

	Protected m_UtilityUI As UtilityUI

	Private m_md As Mandant
	Private m_path As ClsProgPath
	Private m_common As CommonSetting

	Private m_connectionString As String
	Protected m_CommonDatabaseAccess As ICommonDatabaseAccess

	Private m_ShouldbeOnline As Boolean
	Private m_OstJobData As VacancyOstJobMasterData
	Private m_JobCHCustomerData As VacancyJobCHPlattformCustomerData
	Private m_OstjobCustomerData As VacancyOstJobPlattformCustomerData
	Private m_StmpSettingData As VacancyStmpSettingData

#End Region


#Region "public properties"

	Public Property VacancySettingData As ClsVakSetting

	Public Property CurrentVacancyData As VacancyMasterData
	Public Property CurrentVacancyJobCHData As VacancyJobCHMasterData
	Public Property ShouldbeOnline As Boolean

	Public Property VacancyDatabaseAccess As IVacancyDatabaseAccess

	Public ReadOnly Property IsDateForPublishiingOK As Boolean
		Get
			Return m_ShouldbeOnline
		End Get
	End Property

#End Region


#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		m_InitializationData = _setting

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
		m_md = New Mandant
		m_common = New CommonSetting
		m_path = New ClsProgPath

		m_UtilityUI = New UtilityUI


		m_connectionString = m_InitializationData.MDData.MDDbConn
		m_CommonDatabaseAccess = New CommonDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

		InitializeComponent()

		Dim strStyleName As String = m_md.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, 0, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If


		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.


		TranslateControls()
		Reset()

		AddHandler lueWorkExperience.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueJCHAngebot_Art.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler stmpEndDate.ButtonClick, AddressOf OnDropDown_ButtonClick


	End Sub

#End Region


#Region "public methodes"

	Public Function LoadData() As Boolean
		Dim success As Boolean = True

		xtabSettingJobCH.PageEnabled = VacancySettingData.IsAllowedJCH
		xtabSettingOstJob.PageEnabled = VacancySettingData.IsAllowedOstJob
		xtabSettingsuedwestjobs.PageEnabled = False

		m_VacancyDatabaseAccess = VacancyDatabaseAccess
		m_ShouldbeOnline = ShouldbeOnline

		success = success AndAlso LoadAngebotsartDownData()
		success = success AndAlso LoadLanguageDownData()
		success = success AndAlso LoadWorkExperienceDownData()

		success = success AndAlso LoadOstJobMasterData()

		success = success AndAlso LoadJobCHCustomerData()
		success = success AndAlso LoadOstjobCustomerData()
		success = success AndAlso LoadStmpCustomerData()

		success = success AndAlso DisplaySettingData()


		Return success

	End Function

#End Region

	Private Sub Reset()

		Me.urlUserContact.CreateEditorControl()
		Me.urlUserContact.HtmlContent = String.Empty

		Me.xtabJCHInfo.SelectedTabPage = Me.xtabSettingJCHPublikation

		deStart.EditValue = Nothing
		deEnd.EditValue = Nothing

		txtJobCHOrganisationID.EditValue = Nothing
		txtJobCHOrganisationSubID.EditValue = Nothing
		cboJCHLayout_ID.EditValue = Nothing
		cboJCHLogo_ID.EditValue = Nothing
		txtJobCHOrganisationID.Enabled = False
		txtJobCHOrganisationSubID.Enabled = False
		cboJCHLayout_ID.Enabled = False
		cboJCHLogo_ID.Enabled = False

		lueJCHAngebot_Art.EditValue = Nothing
		txtJCHOur_URL.EditValue = Nothing
		txtJCHDirekt_Link.EditValue = Nothing
		txtJCHDirekt_Link.Enabled = False

		txtJCH_Bewerben_URL.EditValue = Nothing
		txtJCH_Xing_Poster_URL.EditValue = Nothing
		txtJCH_Xing_Company_Profile_URL.EditValue = Nothing
		chkJCHXingIsPoc.Checked = False

		lblStellennummerEgov.Text = String.Empty
		lblJobroomID.Text = String.Empty

		urlUserContact.HtmlContent = Nothing
		txtUserEMail.EditValue = Nothing

		ResetAngebotArtDropDown()
		ResetLanguageDropDown()
		ResetWorkExperienceDropDown()

	End Sub

	Sub TranslateControls()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim Time_1 As Double = System.Environment.TickCount
		Try
			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

			lblHeader1.Text = m_Translate.GetSafeTranslationValue(lblHeader1.Text)
			lblHeader2.Text = m_Translate.GetSafeTranslationValue(lblHeader2.Text)

			xtabSettingJobCH.Text = m_Translate.GetSafeTranslationValue(xtabSettingJobCH.Text)
			xtabSettingJCHPublikation.Text = m_Translate.GetSafeTranslationValue(xtabSettingJCHPublikation.Text)
			lblJCHKundennummer.Text = m_Translate.GetSafeTranslationValue(lblJCHKundennummer.Text)
			lblJCHAngebotsart.Text = m_Translate.GetSafeTranslationValue(lblJCHAngebotsart.Text)
			lblJCHErscheinungsdatum.Text = m_Translate.GetSafeTranslationValue(lblJCHErscheinungsdatum.Text)
			lblJCHUnsereURL.Text = m_Translate.GetSafeTranslationValue(lblJCHUnsereURL.Text)
			lblJCHSprache.Text = m_Translate.GetSafeTranslationValue(lblJCHSprache.Text)
			lblJCHLayoutID.Text = m_Translate.GetSafeTranslationValue(lblJCHLayoutID.Text)
			lblJCHLogoID.Text = m_Translate.GetSafeTranslationValue(lblJCHLogoID.Text)

			xtabSettingJCHVakanz.Text = m_Translate.GetSafeTranslationValue(xtabSettingJCHVakanz.Text)
			lblJCHBewerbenURL.Text = m_Translate.GetSafeTranslationValue(lblJCHBewerbenURL.Text)
			lblJCHJobURL.Text = m_Translate.GetSafeTranslationValue(lblJCHJobURL.Text)
			lblJCHXing_Poster_URL.Text = m_Translate.GetSafeTranslationValue(lblJCHXing_Poster_URL.Text)
			lblJCHXing_Company_Profile_URL.Text = m_Translate.GetSafeTranslationValue(lblJCHXing_Company_Profile_URL.Text)
			chkJCHXingIsPoc.Text = m_Translate.GetSafeTranslationValue(chkJCHXingIsPoc.Text)

			xtabSettingOstJob.Text = m_Translate.GetSafeTranslationValue(xtabSettingOstJob.Text)
			lblOJobErscheinungsdatum.Text = m_Translate.GetSafeTranslationValue(lblOJobErscheinungsdatum.Text)
			lblOJobVorlage.Text = m_Translate.GetSafeTranslationValue(lblOJobVorlage.Text)
			chkojcompanyhomepage.Text = m_Translate.GetSafeTranslationValue(chkojcompanyhomepage.Text)
			chkojLehrstelle.Text = m_Translate.GetSafeTranslationValue(chkojLehrstelle.Text)
			grpostjobpublikation.Text = m_Translate.GetSafeTranslationValue(grpostjobpublikation.Text)
			chkojostjob.Text = m_Translate.GetSafeTranslationValue(chkojostjob.Text)
			chkojzentraljob.Text = m_Translate.GetSafeTranslationValue(chkojzentraljob.Text)
			chkojminisite.Text = m_Translate.GetSafeTranslationValue(chkojminisite.Text)
			chkojnicejob.Text = m_Translate.GetSafeTranslationValue(chkojnicejob.Text)
			chkojwestjob.Text = m_Translate.GetSafeTranslationValue(chkojwestjob.Text)

			xtabSettingsuedwestjobs.Text = m_Translate.GetSafeTranslationValue(xtabSettingsuedwestjobs.Text)

			xtabSettingSBN2000.Text = m_Translate.GetSafeTranslationValue(xtabSettingSBN2000.Text)
			lblJobID.Text = m_Translate.GetSafeTranslationValue(lblJobID.Text)
			lblSBNErscheinungsdatum.Text = m_Translate.GetSafeTranslationValue(lblSBNErscheinungsdatum.Text)
			lblSBNArbeitserfahrung.Text = m_Translate.GetSafeTranslationValue(lblSBNArbeitserfahrung.Text)
			chkSBNshortEmployment.Text = m_Translate.GetSafeTranslationValue(chkSBNshortEmployment.Text)
			chkSBNimmediately.Text = m_Translate.GetSafeTranslationValue(chkSBNimmediately.Text)
			chkSBNpermanent.Text = m_Translate.GetSafeTranslationValue(chkSBNpermanent.Text)
			chkSBNsurrogate.Text = m_Translate.GetSafeTranslationValue(chkSBNsurrogate.Text)
			chkSBNreportToAvam.Text = m_Translate.GetSafeTranslationValue(chkSBNreportToAvam.Text)
			chkSBNeuresDisplay.Text = m_Translate.GetSafeTranslationValue(chkSBNeuresDisplay.Text)
			chkSBNpublicDisplay.Text = m_Translate.GetSafeTranslationValue(chkSBNpublicDisplay.Text)

			grpWorkforms.Text = m_Translate.GetSafeTranslationValue(grpWorkforms.Text)
			chkSunday_And_Holidays.Text = m_Translate.GetSafeTranslationValue(chkSunday_And_Holidays.Text)
			chkSBNSHIFT_WORK.Text = m_Translate.GetSafeTranslationValue(chkSBNSHIFT_WORK.Text)
			chkSBNNIGHT_WORK.Text = m_Translate.GetSafeTranslationValue(chkSBNNIGHT_WORK.Text)
			chkSBNHOME_WORK.Text = m_Translate.GetSafeTranslationValue(chkSBNHOME_WORK.Text)

			xtabSettingAllgemein.Text = m_Translate.GetSafeTranslationValue(xtabSettingAllgemein.Text)
			lblEMail.Text = m_Translate.GetSafeTranslationValue(xtabSettingJobCH.Text)
			lblKontaktdaten.Text = m_Translate.GetSafeTranslationValue(xtabSettingJobCH.Text)


			bsiInfo.Caption = m_Translate.GetSafeTranslationValue(bsiInfo.Caption)
			bbiSave.Caption = m_Translate.GetSafeTranslationValue(bbiSave.Caption)


		Catch ex As Exception
			m_Logger.LogError(String.Format("1=>{0}.{1}", strMethodeName, ex.Message))
		End Try

	End Sub

	Private Sub ResetAngebotArtDropDown()

		lueJCHAngebot_Art.Properties.DisplayMember = "DisplayText"
		lueJCHAngebot_Art.Properties.ValueMember = "Value"

		Dim columns = lueJCHAngebot_Art.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("DisplayText", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

		lueJCHAngebot_Art.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueJCHAngebot_Art.Properties.SearchMode = SearchMode.AutoComplete
		lueJCHAngebot_Art.Properties.AutoSearchColumnIndex = 0

		lueJCHAngebot_Art.Properties.NullText = String.Empty
		lueJCHAngebot_Art.EditValue = Nothing

	End Sub

	Private Sub ResetLanguageDropDown()

		lueVakLanguage.Properties.DisplayMember = "DisplayText"
		lueVakLanguage.Properties.ValueMember = "Value"

		Dim columns = lueVakLanguage.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("DisplayText", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

		lueVakLanguage.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueVakLanguage.Properties.SearchMode = SearchMode.AutoComplete
		lueVakLanguage.Properties.AutoSearchColumnIndex = 0

		lueVakLanguage.Properties.NullText = String.Empty
		lueVakLanguage.EditValue = Nothing

	End Sub

	Private Sub ResetWorkExperienceDropDown()

		lueWorkExperience.Properties.DisplayMember = "DisplayText"
		lueWorkExperience.Properties.ValueMember = "Value"

		Dim columns = lueWorkExperience.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("DisplayText", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

		lueWorkExperience.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueWorkExperience.Properties.SearchMode = SearchMode.AutoComplete
		lueWorkExperience.Properties.AutoSearchColumnIndex = 0

		lueWorkExperience.Properties.NullText = String.Empty
		lueWorkExperience.EditValue = Nothing

	End Sub

	Private Function LoadOstJobMasterData() As Boolean
		m_OstJobData = m_VacancyDatabaseAccess.LoadOstJobMasterData(m_InitializationData.MDData.MDGuid, m_InitializationData.UserData.UserFullName, m_InitializationData.UserData.UserNr, CurrentVacancyData.VakNr)
		If m_OstJobData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Vakanz Daten für Ostjob.ch konnte nicht geladen werden."))

			Return Nothing
		End If

		Return Not m_OstJobData Is Nothing
	End Function

	Private Function LoadJobCHCustomerData() As Boolean
		m_JobCHCustomerData = m_VacancyDatabaseAccess.LoadJobCHCustomerData(m_InitializationData.MDData.MDGuid, m_InitializationData.UserData.UserNr, CurrentVacancyData.VakNr, ExternalPlattforms.JOBSCH)
		If m_JobCHCustomerData Is Nothing OrElse m_JobCHCustomerData.ID Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kundendaten für Jobs.ch konnte nicht geladen werden."))

			Return Nothing
		End If

		Return Not m_JobCHCustomerData Is Nothing
	End Function

	Private Function LoadOstjobCustomerData() As Boolean
		m_OstjobCustomerData = m_VacancyDatabaseAccess.LoadOstJobCustomerData(m_InitializationData.MDData.MDGuid, m_InitializationData.UserData.UserNr, CurrentVacancyData.VakNr, ExternalPlattforms.OSTJOB)
		If m_OstjobCustomerData Is Nothing OrElse m_OstjobCustomerData.ID Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kundendaten für Ostjob.ch konnte nicht geladen werden."))

			Return Nothing
		End If

		Return Not m_OstjobCustomerData Is Nothing
	End Function

	Private Function LoadStmpCustomerData() As Boolean
		m_StmpSettingData = m_VacancyDatabaseAccess.LoadStmpSettingData(m_InitializationData.MDData.MDGuid, m_InitializationData.UserData.UserFullName, m_InitializationData.UserData.UserNr, CurrentVacancyData.VakNr)
		If m_StmpSettingData Is Nothing OrElse m_StmpSettingData.VakNr Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kundendaten für STMP konnte nicht geladen werden."))

			Return Nothing
		End If

		Return Not m_StmpSettingData Is Nothing
	End Function

	Private Function LoadAngebotsartDownData() As Boolean

		Dim data = New List(Of AngebotArtViewData) From {
			New AngebotArtViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Clasic Konto"), .Value = 27},
			New AngebotArtViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Clasic Kontingent"), .Value = 28},
			New AngebotArtViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Plus Konto"), .Value = 29},
			New AngebotArtViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Plus Kontingent"), .Value = 30},
			New AngebotArtViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Premium Konto"), .Value = 31},
			New AngebotArtViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Premium Kontingent"), .Value = 32},
			New AngebotArtViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Starter Konto"), .Value = 35},
			New AngebotArtViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Starter Kontingent"), .Value = 36},
			New AngebotArtViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Standard Konto"), .Value = 37},
			New AngebotArtViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Standard Kontingent"), .Value = 38},
			New AngebotArtViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Professional Konto"), .Value = 39},
			New AngebotArtViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Professional Kontingent"), .Value = 40},
			New AngebotArtViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Expert Konto"), .Value = 41},
			New AngebotArtViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Expert Kontingent"), .Value = 42}
		}
		lueJCHAngebot_Art.Properties.DataSource = data
		lueJCHAngebot_Art.Properties.ForceInitialize()


		Return True
	End Function

	Private Function LoadLanguageDownData() As Boolean
		Dim data = New List(Of WorkExperienceViewData) From {
			New WorkExperienceViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Deutsch"), .Value = "de"},
			New WorkExperienceViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Französisch"), .Value = "fr"},
			New WorkExperienceViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Italienisch"), .Value = "it"},
			New WorkExperienceViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Englisch"), .Value = "en"}
		}
		lueVakLanguage.Properties.DataSource = data
		lueVakLanguage.Properties.ForceInitialize()


		Return True
	End Function

	Private Function LoadWorkExperienceDownData() As Boolean
		Dim data = New List(Of WorkExperienceViewData) From {
			New WorkExperienceViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Weniger als ein Jahr"), .Value = "LESS_THAN_1_YEAR"},
			New WorkExperienceViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Mehr als ein Jahr"), .Value = "MORE_THAN_1_YEAR"},
			New WorkExperienceViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Mehr als drei Jahr"), .Value = "MORE_THAN_3_YEARS"}
		}
		lueWorkExperience.Properties.DataSource = data
		lueWorkExperience.Properties.ForceInitialize()


		Return True
	End Function

	Private Sub CmdClose_Click(sender As Object, e As EventArgs) Handles CmdClose.Click
		Me.Close()
	End Sub

	Private Sub bbiSave_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSave.ItemClick
		Dim success As Boolean = True

		success = success AndAlso UpdateVacancySettingData()
		If Not success Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))

			Return
		Else
			Me.Close()
		End If

	End Sub

	Private Function LoadUserContactData() As Boolean
		Dim success As Boolean = True

		If urlUserContact.HtmlContent = String.Empty Then

			Dim strContact As String = TranslatUserInfo(CurrentVacancyJobCHData.VakNr)
			If String.IsNullOrWhiteSpace(strContact) Then
				strContact = m_InitialChangedData.UserData.UserFullName
				strContact &= String.Format("{0}{1}<br />", "<br />", m_InitialChangedData.UserData.UserMDName)
				strContact &= String.Format("{0}{1}<br />", "", m_InitialChangedData.UserData.UserMDStrasse)
				strContact &= String.Format("{0}{1} {2}<br />", "", m_InitialChangedData.UserData.UserMDPLZ, m_InitialChangedData.UserData.UserMDOrt)
				strContact &= String.Format("{0}{1}<br />", "", m_InitialChangedData.UserData.UserMDTelefon)
				strContact &= String.Format("{0}{1}<br />", "", m_InitialChangedData.UserData.UserMDTelefax)
				strContact &= String.Format("{0}{1}<br />", "", m_InitialChangedData.UserData.UserMDeMail)
				strContact &= String.Format("{0}{1}", "", m_InitialChangedData.UserData.UserMDHomepage)
			End If

			''txtUserContact.Text = strContact
			Me.urlUserContact.HtmlContent = strContact
		End If

		If String.IsNullOrWhiteSpace(txtUserEMail.EditValue) Then
			Dim strContact As String = m_InitialChangedData.UserData.UserMDeMail

			txtUserEMail.EditValue = strContact
		End If

		Return success
	End Function

	Private Function TranslatUserInfo(ByVal vacancyNumber As Integer) As String
		Dim result As String = String.Empty

		Try

			Dim fileExtension = "txt"
			Dim tplUserVakInfoFile = Path.Combine(m_md.GetSelectedMDTemplatePath(m_InitializationData.MDData.MDNr), String.Format("tplUserVacancy_{0}.{1}", m_InitialChangedData.UserData.UserNr, fileExtension))
			If Not File.Exists(tplUserVakInfoFile) Then tplUserVakInfoFile = Path.Combine(m_md.GetSelectedMDTemplatePath(m_InitializationData.MDData.MDNr),
				String.Format("tplUserVacancy.{0}", fileExtension))

			If File.Exists(tplUserVakInfoFile) Then

				Dim ParsedFile As String = String.Empty
				Dim line As String = String.Empty

				Dim lines = IO.File.ReadAllLines(tplUserVakInfoFile, System.Text.Encoding.Default)
				For Each line In lines
					If Not String.IsNullOrWhiteSpace(line) Then
						Dim translatedText As String = ParseTemplateFile(line)
						ParsedFile &= If(String.IsNullOrWhiteSpace(translatedText), String.Empty, translatedText & vbNewLine)
					End If
				Next

				'With OpenTextFileReader(tplUserVakInfoFile, System.Text.Encoding.Default)
				'		Do
				'			line = .ReadLine()
				'			'Trace.WriteLine(line)

				'			If Not String.IsNullOrWhiteSpace(line) Then
				'				Dim translatedText As String = ParseTemplateFile(line)
				'				ParsedFile &= If(String.IsNullOrWhiteSpace(translatedText), String.Empty, translatedText & vbNewLine)
				'			End If

				'		Loop Until line Is Nothing

				'		.Close()
				'End With

				result = ParsedFile.Replace(vbNewLine, "</br>")
			End If


		Catch ex As Exception
			result = Nothing
			m_Logger.LogError(ex.ToString())

		Finally

		End Try

		Return result

	End Function

	Private Function DisplaySettingData() As Boolean
		Dim success As Boolean = True

		Try
			success = success AndAlso (Not CurrentVacancyJobCHData Is Nothing)
			success = success AndAlso DisplayJobCHDetails()
			success = success AndAlso DisplayOstJobDetails()
			success = success AndAlso DisplayAVAMDetails()

			If Not success Then Return False

			urlUserContact.HtmlContent = CurrentVacancyJobCHData.UserKontakt
			txtUserEMail.EditValue = CurrentVacancyJobCHData.UserEMail
			success = success AndAlso LoadUserContactData()


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_UtilityUI.ShowErrorDialog(ex.ToString)

			success = False

		End Try


		Return success
	End Function

	Private Function DisplayJobCHDetails() As Boolean
		Dim success As Boolean = True

		' jobs.ch data
		Try

			If Not m_JobCHCustomerData.StartDate.HasValue Then
				deStart.EditValue = Now.Date
			Else
				deStart.EditValue = m_JobCHCustomerData.StartDate
			End If

			If Not m_JobCHCustomerData.EndDate.HasValue Then
				deEnd.EditValue = Now.AddDays(m_JobCHCustomerData.DaysToAdd.GetValueOrDefault(0))
			Else
				deEnd.EditValue = m_JobCHCustomerData.EndDate
			End If

			txtJobCHOrganisationID.EditValue = m_JobCHCustomerData.Organisation_ID
			txtJobCHOrganisationSubID.EditValue = m_JobCHCustomerData.Organisation_ID
			cboJCHLayout_ID.EditValue = m_JobCHCustomerData.Layout_ID
			cboJCHLogo_ID.EditValue = m_JobCHCustomerData.Logo_ID
			lueJCHAngebot_Art.EditValue = m_JobCHCustomerData.Jobplattform_Art
			txtJCHOur_URL.EditValue = m_JobCHCustomerData.Our_URL
			txtJCHDirekt_Link.EditValue = String.Format(m_JobCHCustomerData.Direkt_URL, CurrentVacancyJobCHData.VakNr)
			lueVakLanguage.EditValue = m_JobCHCustomerData.Vak_Sprache

			txtJCH_Bewerben_URL.EditValue = m_JobCHCustomerData.Bewerben_URL
			txtJCH_Xing_Poster_URL.EditValue = m_JobCHCustomerData.Xing_Poster_URL
			txtJCH_Xing_Company_Profile_URL.EditValue = m_JobCHCustomerData.Xing_Company_Profile_URL
			chkJCHXingIsPoc.Checked = m_JobCHCustomerData.Xing_Company_Is_Poc.GetValueOrDefault(False)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_UtilityUI.ShowErrorDialog(ex.ToString)
			Return False

		End Try

		Return success
	End Function

	Private Function DisplayOstJobDetails() As Boolean
		Dim success As Boolean = True

		' ostjob data
		Try
			If Not m_OstJobData.startdate.HasValue Then
				deojstart.EditValue = Now.Date
			Else
				deojstart.EditValue = m_OstJobData.startdate
			End If

			If Not m_OstJobData.enddate.HasValue Then
				deojEnd.EditValue = Now.AddDays(m_OstjobCustomerData.DaysToAdd.GetValueOrDefault(0))
			Else
				deojEnd.EditValue = m_OstJobData.enddate
			End If

			cboojTemplateID.EditValue = m_OstJobData.layoutid
			chkojcompanyhomepage.Checked = m_OstJobData.companyhomepage
			chkojLehrstelle.Checked = m_OstJobData.lehrstelle.GetValueOrDefault(False)
			chkojostjob.Checked = m_OstJobData.ostjob.GetValueOrDefault(False)
			chkojzentraljob.Checked = m_OstJobData.zentraljob.GetValueOrDefault(False)
			chkojminisite.Checked = m_OstJobData.minisite.GetValueOrDefault(False)
			chkojnicejob.Checked = m_OstJobData.nicejob.GetValueOrDefault(False)
			chkojwestjob.Checked = m_OstJobData.westjob.GetValueOrDefault(False)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_UtilityUI.ShowErrorDialog(ex.ToString)
			Return False

		End Try

		Return success
	End Function

	Private Function DisplayAVAMDetails() As Boolean
		Dim success As Boolean = True

		' AVAM data
		Try
			If Not m_StmpSettingData.PublicationStartDate.HasValue Then
				stmpStartDate.EditValue = Now.Date
			Else
				stmpStartDate.EditValue = m_StmpSettingData.PublicationStartDate
			End If

			stmpEndDate.EditValue = m_StmpSettingData.PublicationEndDate
			speNumberofJobs.EditValue = m_StmpSettingData.NumberOfJobs

			If m_StmpSettingData.Less_One_Year.GetValueOrDefault(False) Then lueWorkExperience.EditValue = "LESS_THAN_1_YEAR"
			If m_StmpSettingData.More_One_Year.GetValueOrDefault(False) Then lueWorkExperience.EditValue = "MORE_THAN_1_YEAR"
			If m_StmpSettingData.More_Three_Years.GetValueOrDefault(False) Then lueWorkExperience.EditValue = "MORE_THAN_3_YEARS"

			lblStellennummerEgov.Text = m_StmpSettingData.stellennummerEgov
			lblJobroomID.Text = m_StmpSettingData.JobroomID
			chkSunday_And_Holidays.Checked = m_StmpSettingData.Sunday_and_Holidays.GetValueOrDefault(False)
			chkSBNHOME_WORK.Checked = m_StmpSettingData.Home_Work.GetValueOrDefault(False)
			chkSBNSHIFT_WORK.Checked = m_StmpSettingData.Shift_Work.GetValueOrDefault(False)
			chkSBNNIGHT_WORK.Checked = m_StmpSettingData.Night_Work.GetValueOrDefault(False)

			chkSBNreportToAvam.Checked = m_StmpSettingData.ReportToAvam.GetValueOrDefault(False)
			chkSBNshortEmployment.Checked = m_StmpSettingData.ShortEmployment.GetValueOrDefault(False)
			chkSBNimmediately.Checked = m_StmpSettingData.Immediately.GetValueOrDefault(False)
			chkSBNsurrogate.Checked = m_StmpSettingData.Surrogate.GetValueOrDefault(False)
			chkSBNpermanent.Checked = m_StmpSettingData.Permanent.GetValueOrDefault(False)
			chkSBNeuresDisplay.Checked = m_StmpSettingData.EuresDisplay.GetValueOrDefault(False)
			chkSBNpublicDisplay.Checked = m_StmpSettingData.PublicDisplay.GetValueOrDefault(False)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_UtilityUI.ShowErrorDialog(ex.ToString)
			Return False

		End Try

		Return success
	End Function

	Private Function UpdateVacancySettingData() As Boolean
		Dim success As Boolean = True
		Dim msg As String = String.Empty

		Try

			CurrentVacancyJobCHData.Organisation_ID = CInt(Val(txtJobCHOrganisationID.EditValue))
			CurrentVacancyJobCHData.Organisation_SubID = CInt(Val(txtJobCHOrganisationSubID.EditValue))
			CurrentVacancyJobCHData.Layout_ID = CInt(Val(Me.cboJCHLayout_ID.EditValue))
			CurrentVacancyJobCHData.Logo_ID = CInt(Val(Me.cboJCHLogo_ID.EditValue))

			' Datumfelder überprüfen...
			If ShouldbeOnline Then

				If deStart.EditValue Is Nothing Then
					deStart.EditValue = Now.Date
				End If

				'If deEnd.EditValue < deStart.EditValue Then
				If deEnd.EditValue Is Nothing OrElse deEnd.EditValue < deStart.EditValue Then
					deEnd.EditValue = deStart.EditValue.AddDays(CurrentVacancyJobCHData.DaysToAdd.GetValueOrDefault(0))
				End If

				'If Me.deEnd.EditValue < Now.Date Then
				'	msg = m_Translate.GetSafeTranslationValue("Das Jobs.ch Enddatum der Vakanz liegt in der Vergangenheit.{0}")
				'	m_ShouldbeOnline = False
				'End If

				If (CurrentVacancyJobCHData.Organisation_ID = 0 OrElse CurrentVacancyJobCHData.Organisation_SubID = 0) Then
					'msg &= m_Translate.GetSafeTranslationValue("Die Organisation-ID ist in der Einstellungen leer. Bitte tragen Sie Ihre Organisation-ID welche Sie von Jobs.ch erhalten haben ein.{0}Ihre Daten werden nicht gespeichert!{0}")

					'Throw New Exception(msg)
				End If
			End If

			CurrentVacancyJobCHData.Our_URL = Me.txtJCHOur_URL.EditValue
			CurrentVacancyJobCHData.Direkt_URL = String.Format(String.Format("{0}", txtJCHDirekt_Link.EditValue), CurrentVacancyData.VakNr)
			CurrentVacancyJobCHData.Bewerben_URL = Me.txtJCH_Bewerben_URL.EditValue

			If lueJCHAngebot_Art.EditValue Is Nothing Then CurrentVacancyJobCHData.Angebot_Value = 0 Else CurrentVacancyJobCHData.Angebot_Value = lueJCHAngebot_Art.EditValue
			CurrentVacancyJobCHData.Vak_Sprache = lueVakLanguage.EditValue
			CurrentVacancyJobCHData.Xing_Poster_URL = txtJCH_Xing_Poster_URL.EditValue
			CurrentVacancyJobCHData.Xing_Company_Profile_URL = txtJCH_Xing_Company_Profile_URL.EditValue
			CurrentVacancyJobCHData.Xing_Company_Is_Poc = chkJCHXingIsPoc.CheckState

			CurrentVacancyJobCHData.StartDate = Me.deStart.EditValue
			CurrentVacancyJobCHData.EndDate = Me.deEnd.EditValue
			CurrentVacancyJobCHData.UserEMail = Me.txtUserEMail.EditValue
			CurrentVacancyJobCHData.UserKontakt = If(Me.urlUserContact.HtmlContent Is Nothing, String.Empty, Me.urlUserContact.HtmlContent)

			success = success AndAlso m_VacancyDatabaseAccess.UpdateJobCHCustomerData(CurrentVacancyJobCHData)
			success = success AndAlso UpdateOstjobCustomerData()
			success = success AndAlso UpdateStmpSettingData()

		Catch ex As Exception
			m_Logger.LogError(String.Format("vacancy setting could not be saved! {0}", ex.ToString))
			msg = "Ihre Einstellungsdaten konnten nicht gespeichert werden."
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(msg))

			Return False
		End Try


		Return success

	End Function

	Private Function UpdateOstjobCustomerData() As Boolean
		Dim success As Boolean = True

		Try

			If deojstart.EditValue Is Nothing Then
				deojstart.EditValue = Now.Date
			End If

			If deojEnd.EditValue Is Nothing OrElse deojEnd.EditValue < deojstart.EditValue Then
				deojEnd.EditValue = deojEnd.EditValue.AddDays(m_OstjobCustomerData.DaysToAdd.GetValueOrDefault(0))
			End If

			m_OstJobData.startdate = deojstart.EditValue
			m_OstJobData.enddate = deojEnd.EditValue

			m_OstJobData.layoutid = cboojTemplateID.EditValue
			m_OstJobData.companyhomepage = chkojcompanyhomepage.Checked
			m_OstJobData.lehrstelle = chkojLehrstelle.Checked
			m_OstJobData.ostjob = chkojostjob.Checked
			m_OstJobData.zentraljob = chkojzentraljob.Checked
			m_OstJobData.minisite = chkojminisite.Checked
			m_OstJobData.nicejob = chkojnicejob.Checked
			m_OstJobData.westjob = chkojwestjob.Checked
			m_OstJobData.ChangedUserNumber = m_InitialData.UserData.UserNr


			success = success AndAlso m_VacancyDatabaseAccess.UpdateVacancyOstJobMasterData(m_InitializationData.MDData.MDGuid, m_InitialChangedData.UserData.UserNr, m_OstJobData)

		Catch ex As Exception
			m_Logger.LogError("ostjob.ch setting data could not be saved.")

			Return False
		End Try


		Return success

	End Function

	Private Function UpdateStmpSettingData() As Boolean
		Dim success As Boolean = True

		Try

			If Not m_StmpSettingData.PublicationStartDate.HasValue Then
				m_StmpSettingData.PublicationStartDate = Now.Date
			Else
				m_StmpSettingData.PublicationStartDate = stmpStartDate.EditValue
			End If
			If Not stmpEndDate.EditValue Is Nothing Then
				If stmpEndDate.EditValue < stmpStartDate.EditValue Then stmpEndDate.EditValue = Nothing
			End If
			m_StmpSettingData.PublicationEndDate = stmpEndDate.EditValue
			m_StmpSettingData.NumberOfJobs = Val(speNumberofJobs.EditValue)

			Select Case lueWorkExperience.EditValue
				Case "LESS_THAN_1_YEAR"
					m_StmpSettingData.Less_One_Year = True
				Case "MORE_THAN_1_YEAR"
					m_StmpSettingData.More_One_Year = True
				Case "MORE_THAN_3_YEARS"
					m_StmpSettingData.More_Three_Years = True
				Case Else
					' nothing

			End Select

			m_StmpSettingData.Sunday_and_Holidays = chkSunday_And_Holidays.Checked
			m_StmpSettingData.Home_Work = chkSBNHOME_WORK.Checked
			m_StmpSettingData.Shift_Work = chkSBNSHIFT_WORK.Checked
			m_StmpSettingData.Night_Work = chkSBNNIGHT_WORK.Checked

			m_StmpSettingData.ReportToAvam = chkSBNreportToAvam.Checked
			m_StmpSettingData.ShortEmployment = chkSBNshortEmployment.Checked
			m_StmpSettingData.Immediately = chkSBNimmediately.Checked
			m_StmpSettingData.Surrogate = chkSBNsurrogate.Checked
			m_StmpSettingData.Permanent = chkSBNpermanent.Checked
			m_StmpSettingData.EuresDisplay = chkSBNeuresDisplay.Checked
			m_StmpSettingData.PublicDisplay = chkSBNpublicDisplay.Checked


			success = success AndAlso m_VacancyDatabaseAccess.UpdateVacancyStmpSettingData(m_InitializationData.MDData.MDGuid, m_InitialChangedData.UserData.UserNr, m_StmpSettingData)

		Catch ex As Exception
			m_Logger.LogError(String.Format("stmp setting data could not be saved. {0}", ex.ToString))

			Return False
		End Try

		Return success

	End Function


#Region "helpers"

	''' <summary>
	''' Handles drop down button clicks.
	''' </summary>
	Private Sub OnDropDown_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

		Const ID_OF_DELETE_BUTTON As Int32 = 1

		' If delete button has been clicked reset the drop down.
		If e.Button.Index = ID_OF_DELETE_BUTTON Then

			If TypeOf sender Is LookUpEdit Then
				Dim lookupEdit As LookUpEdit = CType(sender, LookUpEdit)
				lookupEdit.EditValue = Nothing
			ElseIf TypeOf sender Is DateEdit Then
				Dim dateEdit As DateEdit = CType(sender, DateEdit)
				dateEdit.EditValue = Nothing
			End If
		End If
	End Sub

	Private Function ParseTemplateFile(ByVal TextToTranslate As String) As String
		Dim ParsedFile As String = TextToTranslate
		Dim pattern As String = String.Empty

		Const REGEX_USMDName As String = "\{(?i)TMPL_VAR name=\'USMDName\'\}"
		Const REGEX_USMDName2 As String = "\{(?i)TMPL_VAR name=\'USMDName2\'\}"
		Const REGEX_USMDPostfach As String = "\{(?i)TMPL_VAR name=\'USMDPostfach\'\}"
		Const REGEX_USMDStrasse As String = "\{(?i)TMPL_VAR name=\'USMDStrasse\'\}"
		Const REGEX_USMDOrt As String = "\{(?i)TMPL_VAR name=\'USMDort\'\}"
		Const REGEX_USMDPlz As String = "\{(?i)TMPL_VAR name=\'USMDPlz\'\}"
		Const REGEX_USMDLand As String = "\{(?i)TMPL_VAR name=\'USMDLand\'\}"
		Const REGEX_USMDTelefon As String = "\{(?i)TMPL_VAR name=\'USMDTelefon\'\}"
		Const REGEX_USMDDTelefon As String = "\{(?i)TMPL_VAR name=\'USMDDTelefon\'\}"
		Const REGEX_USMDTelefax As String = "\{(?i)TMPL_VAR name=\'USMDTelefax\'\}"
		Const REGEX_USMDeMail As String = "\{(?i)TMPL_VAR name=\'USMDeMail\'\}"
		Const REGEX_USMDHomepage As String = "\{(?i)TMPL_VAR name=\'USMDHomepage\'\}"

		Const REGEX_USAnrede As String = "\{(?i)TMPL_VAR name=\'USAnrede\'\}"
		Const REGEX_USNachname As String = "\{(?i)TMPL_VAR name=\'USNachname\'\}"
		Const REGEX_USVorname As String = "\{(?i)TMPL_VAR name=\'USVorname\'\}"
		Const REGEX_USPostfach As String = "\{(?i)TMPL_VAR name=\'USPostfach\'\}"
		Const REGEX_USStrasse As String = "\{(?i)TMPL_VAR name=\'USStrasse\'\}"
		Const REGEX_USPLZ As String = "\{(?i)TMPL_VAR name=\'USPLZ\'\}"
		Const REGEX_USOrt As String = "\{(?i)TMPL_VAR name=\'USOrt\'\}"
		Const REGEX_USLand As String = "\{(?i)TMPL_VAR name=\'USLand\'\}"
		Const REGEX_USTelefon As String = "\{(?i)TMPL_VAR name=\'USTelefon\'\}"
		Const REGEX_USNatel As String = "\{(?i)TMPL_VAR name=\'USNatel\'\}"
		Const REGEX_USTelefax As String = "\{(?i)TMPL_VAR name=\'USTelefax\'\}"
		Const REGEX_USeMail As String = "\{(?i)TMPL_VAR name=\'USeMail\'\}"
		Const REGEX_USAbteilung As String = "\{(?i)TMPL_VAR name=\'USAbteilung\'\}"
		Const REGEX_USTitel_1 As String = "\{(?i)TMPL_VAR name=\'USTitel_1\'\}"
		Const REGEX_USTitel_2 As String = "\{(?i)TMPL_VAR name=\'USTitel_2\'\}"


		Try
			'// search templatevars
			'pattern = REGEX_USNachname
			Dim regex As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex(pattern)

			Dim userData = m_CommonDatabaseAccess.LoadAdvisorDataforGivenNumber(m_InitializationData.MDData.MDNr, m_InitialChangedData.UserData.UserNr)
			If userData Is Nothing Then
				m_Logger.LogError("user could not be founded!")

				Return String.Empty
			End If

			' Benutzerdaten ...............................................................................................
			ParsedFile = regex.Replace(ParsedFile, REGEX_USNachname, userData.Lastname)
			ParsedFile = regex.Replace(ParsedFile, REGEX_USVorname, userData.Firstname)
			ParsedFile = regex.Replace(ParsedFile, REGEX_USAnrede, userData.Salutation)
			ParsedFile = regex.Replace(ParsedFile, REGEX_USTelefon, userData.UserMDTelefon)
			ParsedFile = regex.Replace(ParsedFile, REGEX_USNatel, userData.UserMobile)
			ParsedFile = regex.Replace(ParsedFile, REGEX_USTelefax, userData.UserMDTelefax)
			ParsedFile = regex.Replace(ParsedFile, REGEX_USeMail, userData.UserMDeMail)
			ParsedFile = regex.Replace(ParsedFile, REGEX_USPostfach, userData.UserMDPostfach)
			ParsedFile = regex.Replace(ParsedFile, REGEX_USStrasse, userData.UserMDStrasse)
			ParsedFile = regex.Replace(ParsedFile, REGEX_USPLZ, userData.UserMDPLZ)
			ParsedFile = regex.Replace(ParsedFile, REGEX_USOrt, userData.UserMDOrt)
			ParsedFile = regex.Replace(ParsedFile, REGEX_USLand, userData.UserMDLand)
			ParsedFile = regex.Replace(ParsedFile, REGEX_USAbteilung, userData.UserFTitel)
			ParsedFile = regex.Replace(ParsedFile, REGEX_USMDName, userData.UserMDName)
			ParsedFile = regex.Replace(ParsedFile, REGEX_USMDName2, userData.UserMDName2)
			ParsedFile = regex.Replace(ParsedFile, REGEX_USMDPostfach, userData.UserMDPostfach)
			ParsedFile = regex.Replace(ParsedFile, REGEX_USMDStrasse, userData.UserMDStrasse)
			ParsedFile = regex.Replace(ParsedFile, REGEX_USMDOrt, userData.UserMDOrt)
			ParsedFile = regex.Replace(ParsedFile, REGEX_USMDPlz, userData.UserMDPLZ)
			ParsedFile = regex.Replace(ParsedFile, REGEX_USMDLand, userData.UserMDLand)
			ParsedFile = regex.Replace(ParsedFile, REGEX_USMDTelefon, userData.UserMDTelefon)
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USMDDTelefon, String.Format("{0}", userData.UserMDDTelefon))
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USMDTelefax, userData.UserMDTelefax)
			ParsedFile = regex.Replace(ParsedFile, REGEX_USMDeMail, userData.UserMDeMail)
			ParsedFile = regex.Replace(ParsedFile, REGEX_USMDHomepage, userData.UserMDHomepage)
			ParsedFile = regex.Replace(ParsedFile, REGEX_USTitel_1, userData.UserFTitel)
			ParsedFile = regex.Replace(ParsedFile, REGEX_USTitel_2, userData.UserSTitel)


			'ParsedFile = SetSyntax(ParsedFile)


		Catch ex As Exception
			'      ParsedFile = String.Empty
			'      MsgBox("Feher: " & ex.Message.Trim & vbCrLf & ParsedFile & vbCrLf & pattern & vbCrLf & line)

		End Try

		Return ParsedFile
	End Function

#End Region


#Region "Helper Class"

	Class AngebotArtViewData

		Public Property Value As Integer
		Public Property DisplayText As String

	End Class

	Class WorkExperienceViewData

		Public Property DisplayText As String
		Public Property Value As String

	End Class



#End Region

End Class