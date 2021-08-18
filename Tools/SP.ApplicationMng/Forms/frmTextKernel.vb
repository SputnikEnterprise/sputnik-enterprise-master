


Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.Applicant
Imports SP.DatabaseAccess.Applicant.DataObjects


Imports System.Text.RegularExpressions
Imports SP.Infrastructure.Logging
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors

Imports DevExpress.XtraBars
Imports System.ComponentModel
Imports System.Reflection

Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.ColorUtility.ClsColorUtility
Imports SPProgUtility.SPUserSec.ClsUserSec

Imports SPProgUtility.MainUtilities
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports DevExpress.LookAndFeel
Imports SP.Infrastructure.UI

Imports DevExpress.XtraSplashScreen
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Base
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports DevExpress.Pdf
Imports SPProgUtility.CommonXmlUtility

Imports System.Text
Imports DevExpress.Skins
Imports DevExpress.UserSkins
Imports SPProgUtility
Imports System.Security.Cryptography
Imports System.IO
Imports SP.Infrastructure
Imports System.Collections.Specialized
Imports System.Net
Imports TrxmlUtility




Public Class frmTextKernel

	'Account: tkdemo_de
	'User: sputnik
	'PW: eyfi6876

#Region "private consts"

  Private Const TEXTKERNEL_BASEURI = "https://staging.textkernel.nl/match/"
  ' Private Const TEXTKERNEL_BASEURI = "https://home.textkernel.nl/sourcebox/"
  ' Private Const TEXTKERNEL_BASEURI = "https://staging.textkernel.nl/sourcebox/"
  ' Private Const TEXTKERNEL_BASEURI = "https://staging.textkernel.nl:443/sourcebox/"

  Private Const webServiceTKProcessUri As String = TEXTKERNEL_BASEURI + "soap/processDocument?wsdl"
  Private Const webServiceTKDocumentMngUri As String = TEXTKERNEL_BASEURI + "soap/manageDocument?wsdl"

  Private Const TEXTKERNEL_LOGINURI = TEXTKERNEL_BASEURI + "loginUser.do"

  Private Const accountUrl As String = TEXTKERNEL_BASEURI + "loginUser.do?account={0}&username={1}&password={2}"
  Private Const changeContentUrl As String = TEXTKERNEL_BASEURI + "loadTrxmlIntoSession.do?jsessionid={0}&trxmlid={1}"

#End Region

#Region "private fields"

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' Boolean flag indicating if form is initializing.
	''' </summary>
	Protected m_SuppressUIEvents As Boolean = False

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper


	''' <summary>
	''' The common data access object.
	''' </summary>
	Private m_CommonDatabaseAccess As ICommonDatabaseAccess
	Private m_AppDatabaseAccess As IAppDatabaseAccess

	''' <summary>
	''' The cv data access object.
	''' </summary>
	Private m_AppCVDatabaseAccess As IAppCvDatabaseAccess


	Private Const Account As String = "username"	' "tkdemo_de"
	Private Const User As String = "username"	' "username"
	Private Const PW As String = "vcge6123"	' "eyfi6876"
	Private m_applicantDb As String
	Private m_customerID As String

	Private m_SelectedFile As String
	Private m_currentTrXMLID As Integer?
	Private m_JSessionID As String
	Private m_TrxmlContent As String
	Private m_Trxmldata As TrXMLData
	Private m_CurrentFileExtension As String


	''' <summary>
	''' Settings xml.
	''' </summary>
	Private m_MandantSettingsXml As SettingsXml

	''' <summary>
	''' List of user controls.
	''' </summary>
	Private m_connectionString As String

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As SP.Infrastructure.Utility

	Private m_mandant As Mandant
	Private m_path As SPProgUtility.ProgPath.ClsProgPath


#End Region


#Region "constructor"

	Public Sub New()

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_InitializationData = CreateInitialData(0, 0)
		m_mandant = New Mandant
		m_UtilityUI = New UtilityUI
		m_Utility = New SP.Infrastructure.Utility
		m_path = New SPProgUtility.ProgPath.ClsProgPath
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

		' Dieser Aufruf ist für den Designer erforderlich.
		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_connectionString = My.Settings.ConnString_Application

		Me.KeyPreview = True
		Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_AppDatabaseAccess = New AppDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_AppCVDatabaseAccess = New CvlDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

		m_MandantSettingsXml = New SettingsXml(m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))

		Dim m_MandantSettingsXml_ As SettingsXml
		Try
			m_MandantSettingsXml_ = New SettingsXml("<your path>\your file.xml")

		Catch ex As Exception

		End Try


		TranslateControls()
		Reset()

		'LoadFilesIntoLst()

		'AddHandler txtSearchURL.ButtonClick, AddressOf OnDropDownButtonClick


	End Sub

#End Region


#Region "Private Properties"

	''' <summary>
	''' Gets the selected file directory data.
	''' </summary>
	Private ReadOnly Property SelectedFileViewData As FileDirectoryData
		Get
			Dim grdView = TryCast(grdFiles.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim viewData = CType(grdView.GetRow(selectedRows(0)), FileDirectoryData)
					Return viewData
				End If

			End If

			Return Nothing
		End Get

	End Property

	''' <summary>
	''' Gets the selected application data.
	''' </summary>
	Private ReadOnly Property SelectedApplicationViewData As ApplicationData
		Get
			Dim grdView = TryCast(grdApplication.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim viewData = CType(grdView.GetRow(selectedRows(0)), ApplicationData)
					Return viewData
				End If

			End If

			Return Nothing
		End Get

	End Property

	''' <summary>
	''' Gets the selected applicant data.
	''' </summary>
	Private ReadOnly Property SelectedApplicantViewData As ApplicantData
		Get
			Dim grdView = TryCast(grdApplicant.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim viewData = CType(grdView.GetRow(selectedRows(0)), ApplicantData)
					Return viewData
				End If

			End If

			Return Nothing
		End Get

	End Property

	''' <summary>
	''' Gets the selected document data.
	''' </summary>
	Private ReadOnly Property SelectedApplicantDocumentViewData As ApplicantDocumentData
		Get
			Dim grdView = TryCast(grdDocument.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim viewData = CType(grdView.GetRow(selectedRows(0)), ApplicantDocumentData)
					Return viewData
				End If

			End If

			Return Nothing
		End Get

	End Property

	''' <summary>
	''' Gets the selected cv profile data.
	''' </summary>
	Private ReadOnly Property SelectedCvProfileViewData As ApplicantCvProfileData
		Get
			Dim grdView = TryCast(grdCVProfile.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim viewData = CType(grdView.GetRow(selectedRows(0)), ApplicantCvProfileData)
					Return viewData
				End If

			End If

			Return Nothing
		End Get

	End Property


#End Region


#Region "private methodes"

	Private Sub TranslateControls()


	End Sub

	Private Sub Reset()

		WebBrowser1.Navigate("about:blank")
		m_customerID = String.Empty

		ResetDetailFields()
		ResetFilesGrid()
		ResetGrids()

	End Sub

#End Region

	Private Sub ResetDetailFields()

		ResetApplicationFields()
		ResetApplicantFields()
		ResetDocumentFields()

	End Sub

	Private Sub ResetApplicationFields()
		txtApplicationApplicationID.EditValue = Nothing
		txtApplicationCustomer_ID.EditValue = Nothing
		txtApplicationApplicationNumber.EditValue = Nothing
		txtApplicationApplicantNumber.EditValue = Nothing
		txtApplicationVacancyNumber.EditValue = Nothing
		txtApplicationBusinessBranch.EditValue = Nothing
		txtApplicationDismissalperiod.EditValue = Nothing
		txtApplicationAvailability.EditValue = Nothing
		txtApplicationComment.EditValue = Nothing
		txtApplicationCreatedOn.EditValue = Nothing
		txtApplicationCreatedFrom.EditValue = Nothing

	End Sub

	Private Sub ResetApplicantFields()

		txtApplicantLastName.EditValue = Nothing
		txtApplicantFirstname.EditValue = Nothing
		txtApplicantGender.EditValue = Nothing
		txtApplicantStreet.EditValue = Nothing
		txtApplicantPostofficeBox.EditValue = Nothing
		txtApplicantPostcode.EditValue = Nothing
		txtApplicantLocation.EditValue = Nothing
		txtApplicantCountry.EditValue = Nothing
		txtApplicantNationality.EditValue = Nothing

		txtApplicantEMail.EditValue = Nothing
		txtApplicantTelephone.EditValue = Nothing
		txtApplicantMobilePhone.EditValue = Nothing

		txtApplicantBirthDate.EditValue = Nothing
		txtApplicantPermission.EditValue = Nothing
		txtApplicantProfession.EditValue = Nothing

		txtApplicantDrivingLicence1.EditValue = Nothing
		txtApplicantDrivingLicence2.EditValue = Nothing
		txtApplicantDrivingLicence3.EditValue = Nothing
		txtApplicantCivilstate.EditValue = Nothing
		lstLanguage.Items.Clear()

		txtApplicantCreatedOn.EditValue = Nothing
		txtApplicantCreatedFrom.EditValue = Nothing
		txtApplicantChangedOn.EditValue = Nothing
		txtApplicantChangedFrom.EditValue = Nothing

		chkApplicationAuto.Checked = False
		chkApplicationMotorcycle.Checked = False
		chkApplicationBicycle.Checked = False

	End Sub

	Private Sub ResetDocumentFields()

		peProfilePicture.Image = Nothing

		txtTrxmlID.EditValue = Nothing
		txtDocumentDocumentID.EditValue = Nothing
		txtDocumentCustomer_ID.EditValue = Nothing
		txtDocumentApplicationNumber.EditValue = Nothing
		txtDocumentApplicantNumber.EditValue = Nothing
		txtDocumentType.EditValue = Nothing
		txtDocumentFlag.EditValue = Nothing
		txtDocumentTitle.EditValue = Nothing
		txtDocumentTrXMLCreatedOn.EditValue = Nothing

		txtDocumentCreatedOn.EditValue = Nothing
		txtDocumentCreatedFrom.EditValue = Nothing

	End Sub

	Private Sub ResetcvProfileFields()

		txtCvID.EditValue = Nothing
		txtCvCustomer_ID.EditValue = Nothing
		txtCvBusinessBranch.EditValue = Nothing
		txtCvTrxmlID.EditValue = Nothing
		txtCvFK_CvPersonal.EditValue = Nothing
		txtCvFK_CvDocumentText.EditValue = Nothing
		txtCvFK_CvDocumentHtml.EditValue = Nothing
		txtCvCreatedOn.EditValue = Nothing
		txtCvCreatedFrom.EditValue = Nothing

		ResetApplicantFields()
		btnDeleteCvProfile.Enabled = False

	End Sub

	Private Sub ResetGrids()

		ResetApplicationGrid()
		ResetApplicantGrid()
		ResetDocumentGrid()
		ResetCvProfileGrid()

	End Sub

	''' <summary>
	''' Resets files grid.
	''' </summary>
	Private Sub ResetFilesGrid()

		gvFiles.OptionsView.ShowIndicator = False
		gvFiles.OptionsView.ShowAutoFilterRow = True
		gvFiles.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvFiles.OptionsView.ShowFooter = False
		gvFiles.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False

		gvFiles.Columns.Clear()


		Dim columnID As New DevExpress.XtraGrid.Columns.GridColumn()
		columnID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnID.OptionsColumn.AllowEdit = False
		columnID.Caption = m_Translate.GetSafeTranslationValue("Datei")
		columnID.Name = "Filename"
		columnID.FieldName = "Filename"
		columnID.Visible = True
		columnID.Width = 50
		gvFiles.Columns.Add(columnID)

		Dim columnCustomer_ID As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomer_ID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCustomer_ID.OptionsColumn.AllowEdit = False
		columnCustomer_ID.Caption = m_Translate.GetSafeTranslationValue("Erstellt am")
		columnCustomer_ID.Name = "FileDate"
		columnCustomer_ID.FieldName = "FileDate"
		columnCustomer_ID.Width = 10
		columnCustomer_ID.Visible = True
		gvFiles.Columns.Add(columnCustomer_ID)


		grdFiles.DataSource = Nothing

	End Sub

	''' <summary>
	''' Resets application grid.
	''' </summary>
	Private Sub ResetApplicationGrid()

		gvApplication.OptionsView.ShowIndicator = False
		gvApplication.OptionsView.ShowAutoFilterRow = True
		gvApplication.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvApplication.OptionsView.ShowFooter = False
		gvApplication.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False

		gvApplication.Columns.Clear()


		Dim columnID As New DevExpress.XtraGrid.Columns.GridColumn()
		columnID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnID.OptionsColumn.AllowEdit = True
		columnID.Caption = m_Translate.GetSafeTranslationValue("ID")
		columnID.Name = "ID"
		columnID.FieldName = "ID"
		columnID.Visible = False
		columnID.Width = 50
		gvApplication.Columns.Add(columnID)

		Dim columnCustomer_ID As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomer_ID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCustomer_ID.OptionsColumn.AllowEdit = False
		columnCustomer_ID.Caption = m_Translate.GetSafeTranslationValue("Customer_ID")
		columnCustomer_ID.Name = "Customer_ID"
		columnCustomer_ID.FieldName = "Customer_ID"
		columnCustomer_ID.Width = 60
		columnCustomer_ID.Visible = False
		gvApplication.Columns.Add(columnCustomer_ID)

		Dim columnApplicationNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnApplicationNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnApplicationNumber.OptionsColumn.AllowEdit = False
		columnApplicationNumber.Caption = m_Translate.GetSafeTranslationValue("ApplicationNumber")
		columnApplicationNumber.Name = "ApplicationNumber"
		columnApplicationNumber.FieldName = "ApplicationNumber"
		columnApplicationNumber.Visible = True
		columnApplicationNumber.Width = 80
		gvApplication.Columns.Add(columnApplicationNumber)

		Dim columnApplicantNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnApplicantNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnApplicantNumber.OptionsColumn.AllowEdit = False
		columnApplicantNumber.Caption = m_Translate.GetSafeTranslationValue("ApplicantNumber")
		columnApplicantNumber.Name = "ApplicantNumber"
		columnApplicantNumber.FieldName = "ApplicantNumber"
		columnApplicantNumber.Visible = True
		columnApplicantNumber.Width = 80
		gvApplication.Columns.Add(columnApplicantNumber)

		Dim columnVacancyNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnVacancyNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnVacancyNumber.OptionsColumn.AllowEdit = False
		columnVacancyNumber.Caption = m_Translate.GetSafeTranslationValue("VacancyNumber")
		columnVacancyNumber.Name = "VacancyNumber"
		columnVacancyNumber.FieldName = "VacancyNumber"
		columnVacancyNumber.Visible = False
		columnVacancyNumber.Width = 50
		gvApplication.Columns.Add(columnVacancyNumber)

		Dim columnBusinessBranch As New DevExpress.XtraGrid.Columns.GridColumn()
		columnBusinessBranch.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnBusinessBranch.OptionsColumn.AllowEdit = False
		columnBusinessBranch.Caption = m_Translate.GetSafeTranslationValue("Filiale")
		columnBusinessBranch.Name = "BusinessBranch"
		columnBusinessBranch.FieldName = "BusinessBranch"
		columnBusinessBranch.Visible = True
		columnBusinessBranch.Width = 60
		gvApplication.Columns.Add(columnBusinessBranch)

		Dim columnDismissalperiod As New DevExpress.XtraGrid.Columns.GridColumn()
		columnDismissalperiod.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnDismissalperiod.OptionsColumn.AllowEdit = False
		columnDismissalperiod.Caption = m_Translate.GetSafeTranslationValue("Kuendigungsfrist")
		columnDismissalperiod.Name = "Dismissalperiod"
		columnDismissalperiod.FieldName = "Dismissalperiod"
		columnDismissalperiod.Visible = True
		columnDismissalperiod.Width = 60
		gvApplication.Columns.Add(columnDismissalperiod)

		Dim columnAvailability As New DevExpress.XtraGrid.Columns.GridColumn()
		columnAvailability.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnAvailability.OptionsColumn.AllowEdit = False
		columnAvailability.Caption = m_Translate.GetSafeTranslationValue("Verfuegbarkeit")
		columnAvailability.Name = "Availability"
		columnAvailability.FieldName = "Availability"
		columnAvailability.Visible = True
		columnAvailability.Width = 60
		gvApplication.Columns.Add(columnAvailability)

		Dim columnComment As New DevExpress.XtraGrid.Columns.GridColumn()
		columnComment.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnComment.OptionsColumn.AllowEdit = False
		columnComment.Caption = m_Translate.GetSafeTranslationValue("Bemerkung")
		columnComment.Name = "Comment"
		columnComment.FieldName = "Comment"
		columnComment.Visible = False
		columnComment.Width = 60
		gvApplication.Columns.Add(columnComment)

		Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCreatedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCreatedOn.OptionsColumn.AllowEdit = False
		columnCreatedOn.Caption = m_Translate.GetSafeTranslationValue("CreatedOn")
		columnCreatedOn.Name = "CreatedOn"
		columnCreatedOn.FieldName = "CreatedOn"
		columnCreatedOn.Visible = True
		columnCreatedOn.Width = 60
		gvApplication.Columns.Add(columnCreatedOn)

		Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCreatedFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCreatedFrom.OptionsColumn.AllowEdit = False
		columnCreatedFrom.Caption = m_Translate.GetSafeTranslationValue("CreatedFrom")
		columnCreatedFrom.Name = "CreatedFrom"
		columnCreatedFrom.FieldName = "CreatedFrom"
		columnCreatedFrom.Visible = False
		columnCreatedFrom.Width = 60
		gvApplication.Columns.Add(columnCreatedFrom)


		grdApplication.DataSource = Nothing

	End Sub

	''' <summary>
	''' Resets applicant grid.
	''' </summary>
	Private Sub ResetApplicantGrid()

		gvApplicant.OptionsView.ShowIndicator = False
		gvApplicant.OptionsView.ShowAutoFilterRow = True
		gvApplicant.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvApplicant.OptionsView.ShowFooter = False
		gvApplicant.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False

		gvApplicant.Columns.Clear()


		Dim columnID As New DevExpress.XtraGrid.Columns.GridColumn()
		columnID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnID.OptionsColumn.AllowEdit = True
		columnID.Caption = m_Translate.GetSafeTranslationValue("ID")
		columnID.Name = "ID"
		columnID.FieldName = "ID"
		columnID.Visible = False
		columnID.Width = 50
		gvApplicant.Columns.Add(columnID)

		Dim columnCustomer_ID As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomer_ID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCustomer_ID.OptionsColumn.AllowEdit = False
		columnCustomer_ID.Caption = m_Translate.GetSafeTranslationValue("Customer_ID")
		columnCustomer_ID.Name = "Customer_ID"
		columnCustomer_ID.FieldName = "Customer_ID"
		columnCustomer_ID.Width = 60
		columnCustomer_ID.Visible = False
		gvApplicant.Columns.Add(columnCustomer_ID)

		Dim columnApplicationNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnApplicationNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnApplicationNumber.OptionsColumn.AllowEdit = False
		columnApplicationNumber.Caption = m_Translate.GetSafeTranslationValue("ApplicationNumber")
		columnApplicationNumber.Name = "ApplicationNumber"
		columnApplicationNumber.FieldName = "ApplicationNumber"
		columnApplicationNumber.Visible = True
		columnApplicationNumber.Width = 80
		gvApplicant.Columns.Add(columnApplicationNumber)

		Dim columnApplicantFullname As New DevExpress.XtraGrid.Columns.GridColumn()
		columnApplicantFullname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnApplicantFullname.OptionsColumn.AllowEdit = False
		columnApplicantFullname.Caption = m_Translate.GetSafeTranslationValue("Name")
		columnApplicantFullname.Name = "ApplicantFullname"
		columnApplicantFullname.FieldName = "ApplicantFullname"
		columnApplicantFullname.Visible = True
		columnApplicantFullname.Width = 60
		gvApplicant.Columns.Add(columnApplicantFullname)

		Dim columnGender As New DevExpress.XtraGrid.Columns.GridColumn()
		columnGender.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnGender.OptionsColumn.AllowEdit = False
		columnGender.Caption = m_Translate.GetSafeTranslationValue("Geschlecht")
		columnGender.Name = "Gender"
		columnGender.FieldName = "Gender"
		columnGender.Visible = True
		columnGender.Width = 60
		gvApplicant.Columns.Add(columnGender)

		Dim columnAddress As New DevExpress.XtraGrid.Columns.GridColumn()
		columnAddress.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnAddress.OptionsColumn.AllowEdit = False
		columnAddress.Caption = m_Translate.GetSafeTranslationValue("Adresse")
		columnAddress.Name = "Address"
		columnAddress.FieldName = "Address"
		columnAddress.Visible = True
		columnAddress.Width = 60
		gvApplicant.Columns.Add(columnAddress)

		Dim columnPostOfficeBox As New DevExpress.XtraGrid.Columns.GridColumn()
		columnPostOfficeBox.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnPostOfficeBox.OptionsColumn.AllowEdit = False
		columnPostOfficeBox.Caption = m_Translate.GetSafeTranslationValue("PostOfficeBox")
		columnPostOfficeBox.Name = "PostOfficeBox"
		columnPostOfficeBox.FieldName = "PostOfficeBox"
		columnPostOfficeBox.Visible = True
		columnPostOfficeBox.Width = 60
		gvApplicant.Columns.Add(columnPostOfficeBox)


		Dim columnCountry As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCountry.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCountry.OptionsColumn.AllowEdit = False
		columnCountry.Caption = m_Translate.GetSafeTranslationValue("Country")
		columnCountry.Name = "Country"
		columnCountry.FieldName = "Country"
		columnCountry.Visible = True
		columnCountry.Width = 60
		gvApplicant.Columns.Add(columnCountry)

		Dim columnNationality As New DevExpress.XtraGrid.Columns.GridColumn()
		columnNationality.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnNationality.OptionsColumn.AllowEdit = False
		columnNationality.Caption = m_Translate.GetSafeTranslationValue("Nationality")
		columnNationality.Name = "Nationality"
		columnNationality.FieldName = "Nationality"
		columnNationality.Visible = True
		columnNationality.Width = 60
		gvApplicant.Columns.Add(columnNationality)

		Dim columnEMail As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEMail.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnEMail.OptionsColumn.AllowEdit = False
		columnEMail.Caption = m_Translate.GetSafeTranslationValue("EMail")
		columnEMail.Name = "EMail"
		columnEMail.FieldName = "EMail"
		columnEMail.Visible = True
		columnEMail.Width = 60
		gvApplicant.Columns.Add(columnEMail)

		Dim columnTelephone As New DevExpress.XtraGrid.Columns.GridColumn()
		columnTelephone.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnTelephone.OptionsColumn.AllowEdit = False
		columnTelephone.Caption = m_Translate.GetSafeTranslationValue("Telephone")
		columnTelephone.Name = "Telephone"
		columnTelephone.FieldName = "Telephone"
		columnTelephone.Visible = True
		columnTelephone.Width = 60
		gvApplicant.Columns.Add(columnTelephone)

		Dim columnMobilePhone As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMobilePhone.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnMobilePhone.OptionsColumn.AllowEdit = False
		columnMobilePhone.Caption = m_Translate.GetSafeTranslationValue("MobilePhone")
		columnMobilePhone.Name = "MobilePhone"
		columnMobilePhone.FieldName = "MobilePhone"
		columnMobilePhone.Visible = True
		columnMobilePhone.Width = 60
		gvApplicant.Columns.Add(columnMobilePhone)

		Dim columnBirthdate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnBirthdate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnBirthdate.OptionsColumn.AllowEdit = False
		columnBirthdate.Caption = m_Translate.GetSafeTranslationValue("Birthdate")
		columnBirthdate.Name = "Birthdate"
		columnBirthdate.FieldName = "Birthdate"
		columnBirthdate.Visible = True
		columnBirthdate.Width = 60
		gvApplicant.Columns.Add(columnBirthdate)

		Dim columnPermission As New DevExpress.XtraGrid.Columns.GridColumn()
		columnPermission.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnPermission.OptionsColumn.AllowEdit = False
		columnPermission.Caption = m_Translate.GetSafeTranslationValue("Permission")
		columnPermission.Name = "Permission"
		columnPermission.FieldName = "Permission"
		columnPermission.Visible = False
		columnPermission.Width = 60
		gvApplicant.Columns.Add(columnPermission)

		Dim columncolumnPermission As New DevExpress.XtraGrid.Columns.GridColumn()
		columncolumnPermission.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncolumnPermission.OptionsColumn.AllowEdit = False
		columncolumnPermission.Caption = m_Translate.GetSafeTranslationValue("columnPermission")
		columncolumnPermission.Name = "columnPermission"
		columncolumnPermission.FieldName = "columnPermission"
		columncolumnPermission.Visible = False
		columncolumnPermission.Width = 60
		gvApplicant.Columns.Add(columncolumnPermission)

		Dim columnAuto As New DevExpress.XtraGrid.Columns.GridColumn()
		columnAuto.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnAuto.OptionsColumn.AllowEdit = False
		columnAuto.Caption = m_Translate.GetSafeTranslationValue("Auto")
		columnAuto.Name = "Auto"
		columnAuto.FieldName = "Auto"
		columnAuto.Visible = False
		columnAuto.Width = 60
		gvApplicant.Columns.Add(columnAuto)

		Dim columnMotorcycle As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMotorcycle.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnMotorcycle.OptionsColumn.AllowEdit = False
		columnMotorcycle.Caption = m_Translate.GetSafeTranslationValue("Motorcycle")
		columnMotorcycle.Name = "Motorcycle"
		columnMotorcycle.FieldName = "Motorcycle"
		columnMotorcycle.Visible = False
		columnMotorcycle.Width = 60
		gvApplicant.Columns.Add(columnMotorcycle)

		Dim columnBicycle As New DevExpress.XtraGrid.Columns.GridColumn()
		columnBicycle.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnBicycle.OptionsColumn.AllowEdit = False
		columnBicycle.Caption = m_Translate.GetSafeTranslationValue("Bicycle")
		columnBicycle.Name = "Bicycle"
		columnBicycle.FieldName = "Bicycle"
		columnBicycle.Visible = False
		columnBicycle.Width = 60
		gvApplicant.Columns.Add(columnBicycle)

		Dim columnDrivingLicence1 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnDrivingLicence1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnDrivingLicence1.OptionsColumn.AllowEdit = False
		columnDrivingLicence1.Caption = m_Translate.GetSafeTranslationValue("DrivingLicence1")
		columnDrivingLicence1.Name = "DrivingLicence1"
		columnDrivingLicence1.FieldName = "DrivingLicence1"
		columnDrivingLicence1.Visible = False
		columnDrivingLicence1.Width = 60
		gvApplicant.Columns.Add(columnDrivingLicence1)

		Dim columnDrivingLicence2 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnDrivingLicence2.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnDrivingLicence2.OptionsColumn.AllowEdit = False
		columnDrivingLicence2.Caption = m_Translate.GetSafeTranslationValue("DrivingLicence1")
		columnDrivingLicence2.Name = "DrivingLicence2"
		columnDrivingLicence2.FieldName = "DrivingLicence2"
		columnDrivingLicence2.Visible = False
		columnDrivingLicence2.Width = 60
		gvApplicant.Columns.Add(columnDrivingLicence2)

		Dim columnDrivingLicence3 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnDrivingLicence3.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnDrivingLicence3.OptionsColumn.AllowEdit = False
		columnDrivingLicence3.Caption = m_Translate.GetSafeTranslationValue("DrivingLicence3")
		columnDrivingLicence3.Name = "DrivingLicence3"
		columnDrivingLicence3.FieldName = "DrivingLicence3"
		columnDrivingLicence3.Visible = False
		columnDrivingLicence3.Width = 60
		gvApplicant.Columns.Add(columnDrivingLicence3)


		Dim columnCivilState As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCivilState.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCivilState.OptionsColumn.AllowEdit = False
		columnCivilState.Caption = m_Translate.GetSafeTranslationValue("CivilState")
		columnCivilState.Name = "CivilState"
		columnCivilState.FieldName = "CivilState"
		columnCivilState.Visible = True
		columnCivilState.Width = 60
		gvApplicant.Columns.Add(columnCivilState)

		Dim columnLanguage As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLanguage.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnLanguage.OptionsColumn.AllowEdit = False
		columnLanguage.Caption = m_Translate.GetSafeTranslationValue("Language")
		columnLanguage.Name = "Language"
		columnLanguage.FieldName = "Language"
		columnLanguage.Visible = True
		columnLanguage.Width = 60
		gvApplicant.Columns.Add(columnLanguage)

		Dim columnLanguageLevel As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLanguageLevel.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnLanguageLevel.OptionsColumn.AllowEdit = False
		columnLanguageLevel.Caption = m_Translate.GetSafeTranslationValue("LanguageLevel")
		columnLanguageLevel.Name = "LanguageLevel"
		columnLanguageLevel.FieldName = "LanguageLevel"
		columnLanguageLevel.Visible = False
		columnLanguageLevel.Width = 60
		gvApplicant.Columns.Add(columnLanguageLevel)

		Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCreatedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCreatedOn.OptionsColumn.AllowEdit = False
		columnCreatedOn.Caption = m_Translate.GetSafeTranslationValue("CreatedOn")
		columnCreatedOn.Name = "CreatedOn"
		columnCreatedOn.FieldName = "CreatedOn"
		columnCreatedOn.Visible = True
		columnCreatedOn.Width = 60
		gvApplicant.Columns.Add(columnCreatedOn)

		Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCreatedFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCreatedFrom.OptionsColumn.AllowEdit = False
		columnCreatedFrom.Caption = m_Translate.GetSafeTranslationValue("CreatedFrom")
		columnCreatedFrom.Name = "CreatedFrom"
		columnCreatedFrom.FieldName = "CreatedFrom"
		columnCreatedFrom.Visible = True
		columnCreatedFrom.Width = 60
		gvApplicant.Columns.Add(columnCreatedFrom)

		Dim columnChangedOn As New DevExpress.XtraGrid.Columns.GridColumn()
		columnChangedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnChangedOn.OptionsColumn.AllowEdit = False
		columnChangedOn.Caption = m_Translate.GetSafeTranslationValue("ChangedOn")
		columnChangedOn.Name = "ChangedOn"
		columnChangedOn.FieldName = "ChangedOn"
		columnChangedOn.Visible = False
		columnChangedOn.Width = 60
		gvApplicant.Columns.Add(columnChangedOn)

		Dim columnChangedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
		columnChangedFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnChangedFrom.OptionsColumn.AllowEdit = False
		columnChangedFrom.Caption = m_Translate.GetSafeTranslationValue("ChangedFrom")
		columnChangedFrom.Name = "ChangedFrom"
		columnChangedFrom.FieldName = "ChangedFrom"
		columnChangedFrom.Visible = False
		columnChangedFrom.Width = 60
		gvApplicant.Columns.Add(columnChangedFrom)


		grdApplicant.DataSource = Nothing

	End Sub

	''' <summary>
	''' Resets document grid.
	''' </summary>
	Private Sub ResetDocumentGrid()

		gvDocument.OptionsView.ShowIndicator = False
		gvDocument.OptionsView.ShowAutoFilterRow = True
		gvDocument.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvDocument.OptionsView.ShowFooter = False
		gvDocument.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False

		gvDocument.Columns.Clear()


		Dim columnID As New DevExpress.XtraGrid.Columns.GridColumn()
		columnID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnID.OptionsColumn.AllowEdit = True
		columnID.Caption = m_Translate.GetSafeTranslationValue("ID")
		columnID.Name = "ID"
		columnID.FieldName = "ID"
		columnID.Visible = False
		columnID.Width = 50
		gvDocument.Columns.Add(columnID)

		Dim columnCustomer_ID As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomer_ID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCustomer_ID.OptionsColumn.AllowEdit = False
		columnCustomer_ID.Caption = m_Translate.GetSafeTranslationValue("Customer_ID")
		columnCustomer_ID.Name = "Customer_ID"
		columnCustomer_ID.FieldName = "Customer_ID"
		columnCustomer_ID.Width = 60
		columnCustomer_ID.Visible = False
		gvDocument.Columns.Add(columnCustomer_ID)

		Dim columnApplicationNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnApplicationNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnApplicationNumber.OptionsColumn.AllowEdit = False
		columnApplicationNumber.Caption = m_Translate.GetSafeTranslationValue("ApplicationNumber")
		columnApplicationNumber.Name = "ApplicationNumber"
		columnApplicationNumber.FieldName = "ApplicationNumber"
		columnApplicationNumber.Visible = True
		columnApplicationNumber.Width = 80
		gvDocument.Columns.Add(columnApplicationNumber)

		Dim columnType As New DevExpress.XtraGrid.Columns.GridColumn()
		columnType.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnType.OptionsColumn.AllowEdit = False
		columnType.Caption = m_Translate.GetSafeTranslationValue("Type")
		columnType.Name = "Type"
		columnType.FieldName = "Type"
		columnType.Visible = False
		columnType.Width = 80
		gvDocument.Columns.Add(columnType)

		Dim columnFlag As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFlag.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnFlag.OptionsColumn.AllowEdit = False
		columnFlag.Caption = m_Translate.GetSafeTranslationValue("Flag")
		columnFlag.Name = "FlagFlag"
		columnFlag.FieldName = "FlagFlag"
		columnFlag.Visible = False
		columnFlag.Width = 80
		gvDocument.Columns.Add(columnFlag)

		Dim columnTitle As New DevExpress.XtraGrid.Columns.GridColumn()
		columnTitle.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnTitle.OptionsColumn.AllowEdit = False
		columnTitle.Caption = m_Translate.GetSafeTranslationValue("Title")
		columnTitle.Name = "Title"
		columnTitle.FieldName = "Title"
		columnTitle.Visible = True
		columnTitle.Width = 80
		gvDocument.Columns.Add(columnTitle)

		Dim columnTrXMLID As New DevExpress.XtraGrid.Columns.GridColumn()
		columnTrXMLID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnTrXMLID.OptionsColumn.AllowEdit = False
		columnTrXMLID.Caption = m_Translate.GetSafeTranslationValue("TrXMLID")
		columnTrXMLID.Name = "TrXMLID"
		columnTrXMLID.FieldName = "TrXMLID"
		columnTrXMLID.Visible = True
		columnTrXMLID.Width = 80
		gvDocument.Columns.Add(columnTrXMLID)

		Dim columnTrXMLCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
		columnTrXMLCreatedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnTrXMLCreatedOn.OptionsColumn.AllowEdit = False
		columnTrXMLCreatedOn.Caption = m_Translate.GetSafeTranslationValue("TrXMLCreatedOn")
		columnTrXMLCreatedOn.Name = "TrXMLCreatedOn"
		columnTrXMLCreatedOn.FieldName = "TrXMLCreatedOn"
		columnTrXMLCreatedOn.Visible = False
		columnTrXMLCreatedOn.Width = 80
		gvDocument.Columns.Add(columnTrXMLCreatedOn)

		Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCreatedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCreatedOn.OptionsColumn.AllowEdit = False
		columnCreatedOn.Caption = m_Translate.GetSafeTranslationValue("CreatedOn")
		columnCreatedOn.Name = "CreatedOn"
		columnCreatedOn.FieldName = "CreatedOn"
		columnCreatedOn.Visible = True
		columnCreatedOn.Width = 60
		gvDocument.Columns.Add(columnCreatedOn)

		Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCreatedFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCreatedFrom.OptionsColumn.AllowEdit = False
		columnCreatedFrom.Caption = m_Translate.GetSafeTranslationValue("CreatedFrom")
		columnCreatedFrom.Name = "CreatedFrom"
		columnCreatedFrom.FieldName = "CreatedFrom"
		columnCreatedFrom.Visible = False
		columnCreatedFrom.Width = 60
		gvDocument.Columns.Add(columnCreatedFrom)


		grdDocument.DataSource = Nothing

	End Sub

	''' <summary>
	''' Resets cv Proflie grid.
	''' </summary>
	Private Sub ResetCvProfileGrid()

		gvCVProfile.OptionsView.ShowIndicator = False
		gvCVProfile.OptionsView.ShowAutoFilterRow = True
		gvCVProfile.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvCVProfile.OptionsView.ShowFooter = False
		gvCVProfile.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False

		gvCVProfile.Columns.Clear()


		Dim columnID As New DevExpress.XtraGrid.Columns.GridColumn()
		columnID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnID.OptionsColumn.AllowEdit = True
		columnID.Caption = m_Translate.GetSafeTranslationValue("ID")
		columnID.Name = "ID"
		columnID.FieldName = "ID"
		columnID.Visible = False
		columnID.Width = 50
		gvCVProfile.Columns.Add(columnID)

		Dim columnCustomer_ID As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomer_ID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCustomer_ID.OptionsColumn.AllowEdit = False
		columnCustomer_ID.Caption = m_Translate.GetSafeTranslationValue("Customer_ID")
		columnCustomer_ID.Name = "Customer_ID"
		columnCustomer_ID.FieldName = "Customer_ID"
		columnCustomer_ID.Width = 60
		columnCustomer_ID.Visible = False
		gvCVProfile.Columns.Add(columnCustomer_ID)

		Dim columnApplicationNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnApplicationNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnApplicationNumber.OptionsColumn.AllowEdit = False
		columnApplicationNumber.Caption = m_Translate.GetSafeTranslationValue("BusinessBranch")
		columnApplicationNumber.Name = "BusinessBranch"
		columnApplicationNumber.FieldName = "BusinessBranch"
		columnApplicationNumber.Visible = True
		columnApplicationNumber.Width = 80
		gvCVProfile.Columns.Add(columnApplicationNumber)

		Dim columnApplicantNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnApplicantNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnApplicantNumber.OptionsColumn.AllowEdit = False
		columnApplicantNumber.Caption = m_Translate.GetSafeTranslationValue("TrxmlID")
		columnApplicantNumber.Name = "TrxmlID"
		columnApplicantNumber.FieldName = "TrxmlID"
		columnApplicantNumber.Visible = True
		columnApplicantNumber.Width = 80
		gvCVProfile.Columns.Add(columnApplicantNumber)

		Dim columnVacancyNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnVacancyNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnVacancyNumber.OptionsColumn.AllowEdit = False
		columnVacancyNumber.Caption = m_Translate.GetSafeTranslationValue("FK_CvPersonal")
		columnVacancyNumber.Name = "FK_CvPersonal"
		columnVacancyNumber.FieldName = "FK_CvPersonal"
		columnVacancyNumber.Visible = False
		columnVacancyNumber.Width = 50
		gvCVProfile.Columns.Add(columnVacancyNumber)

		Dim columnBusinessBranch As New DevExpress.XtraGrid.Columns.GridColumn()
		columnBusinessBranch.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnBusinessBranch.OptionsColumn.AllowEdit = False
		columnBusinessBranch.Caption = m_Translate.GetSafeTranslationValue("Filiale")
		columnBusinessBranch.Name = "BusinessBranch"
		columnBusinessBranch.FieldName = "BusinessBranch"
		columnBusinessBranch.Visible = True
		columnBusinessBranch.Width = 60
		gvCVProfile.Columns.Add(columnBusinessBranch)

		Dim columnDismissalperiod As New DevExpress.XtraGrid.Columns.GridColumn()
		columnDismissalperiod.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnDismissalperiod.OptionsColumn.AllowEdit = False
		columnDismissalperiod.Caption = m_Translate.GetSafeTranslationValue("Kuendigungsfrist")
		columnDismissalperiod.Name = "Dismissalperiod"
		columnDismissalperiod.FieldName = "Dismissalperiod"
		columnDismissalperiod.Visible = True
		columnDismissalperiod.Width = 60
		gvCVProfile.Columns.Add(columnDismissalperiod)

		Dim columnAvailability As New DevExpress.XtraGrid.Columns.GridColumn()
		columnAvailability.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnAvailability.OptionsColumn.AllowEdit = False
		columnAvailability.Caption = m_Translate.GetSafeTranslationValue("FK_CvDocumentText")
		columnAvailability.Name = "FK_CvDocumentText"
		columnAvailability.FieldName = "FK_CvDocumentText"
		columnAvailability.Visible = True
		columnAvailability.Width = 60
		gvCVProfile.Columns.Add(columnAvailability)

		Dim columnComment As New DevExpress.XtraGrid.Columns.GridColumn()
		columnComment.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnComment.OptionsColumn.AllowEdit = False
		columnComment.Caption = m_Translate.GetSafeTranslationValue("FK_CvDocumentHtml")
		columnComment.Name = "FK_CvDocumentHtml"
		columnComment.FieldName = "FK_CvDocumentHtml"
		columnComment.Visible = False
		columnComment.Width = 60
		gvCVProfile.Columns.Add(columnComment)

		Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCreatedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCreatedOn.OptionsColumn.AllowEdit = False
		columnCreatedOn.Caption = m_Translate.GetSafeTranslationValue("CreatedOn")
		columnCreatedOn.Name = "CreatedOn"
		columnCreatedOn.FieldName = "CreatedOn"
		columnCreatedOn.Visible = True
		columnCreatedOn.Width = 60
		gvCVProfile.Columns.Add(columnCreatedOn)

		Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCreatedFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCreatedFrom.OptionsColumn.AllowEdit = False
		columnCreatedFrom.Caption = m_Translate.GetSafeTranslationValue("CreatedFrom")
		columnCreatedFrom.Name = "CreatedFrom"
		columnCreatedFrom.FieldName = "CreatedFrom"
		columnCreatedFrom.Visible = False
		columnCreatedFrom.Width = 60
		gvCVProfile.Columns.Add(columnCreatedFrom)


		grdCVProfile.DataSource = Nothing

	End Sub



#Region "loading directore files"

	Private Sub txtPath_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles txtPath.ButtonClick
		Dim dialog As New FolderBrowserDialog()

		dialog.Description = m_Translate.GetSafeTranslationValue("Bitte wählen Sie ein Verzeichnis für Export der Datei aus:")
		dialog.ShowNewFolderButton = True
		dialog.SelectedPath = txtPath.EditValue

		If dialog.ShowDialog() = DialogResult.OK Then
			txtPath.EditValue = dialog.SelectedPath
			LoadFilesIntoLst()
		End If

	End Sub

	Private Sub txtPath_KeyUp(sender As Object, e As KeyEventArgs) Handles txtPath.KeyUp
		If e.KeyCode = Keys.Enter Then
			LoadFilesIntoLst()
		End If
	End Sub

	Private Sub LoadFilesIntoLst()

		grdFiles.DataSource = Nothing
		Dim AllowedExtension As String = ".pdf"
		Dim gridDataList As New BindingList(Of FileDirectoryData)
		If Not Directory.Exists(txtPath.EditValue) Then Return

		For Each file As String In IO.Directory.GetFiles(txtPath.EditValue, ".", SearchOption.TopDirectoryOnly)
			gridDataList.Add(New FileDirectoryData With {.Filename = Path.GetFileName(file), .FileDate = FileDateTime(file)})
		Next
		grdFiles.DataSource = gridDataList

	End Sub


#End Region


	Private Function LoadData() As Boolean
		Dim success As Boolean = True

		ResetGrids()

		success = success AndAlso LoadApplicationData()
		success = success AndAlso LoadApplicantData()
		success = success AndAlso LoadApplicationDocumentData()

		success = success AndAlso LoadCvProfileData()

		Return success

	End Function

	Private Function LoadAssignedFile(ByVal recID As Integer) As Boolean
		Dim success As Boolean = True

		Dim documentData = m_AppDatabaseAccess.LoadAssignedDocumentData(recID)
		Try

			If (documentData Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Dokumentdaten konnte nicht geladen werden."))
				Return False
			End If
			m_CurrentFileExtension = String.Empty

			If Not documentData.Content Is Nothing Then
				Dim bytes() = documentData.Content
				Dim tempFileName = System.IO.Path.GetTempFileName()
				m_CurrentFileExtension = System.IO.Path.GetExtension(documentData.FileExtension)
				m_CurrentFileExtension = m_CurrentFileExtension.Replace(".", "")

				Dim tempFileFinal = System.IO.Path.ChangeExtension(tempFileName, m_CurrentFileExtension)

				If (Not bytes Is Nothing AndAlso m_Utility.WriteFileBytes(tempFileFinal, bytes)) Then
					m_Utility.OpenFileWithDefaultProgram(tempFileFinal)
				End If

			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_UtilityUI.ShowErrorDialog(ex.ToString)
			success = False
		End Try


		Return success

	End Function

	Private Function LoadApplicationData() As Boolean
		Dim success As Boolean = True

		Dim listOfData = m_AppDatabaseAccess.LoadApplicationData(m_customerID, String.Empty)

		If (listOfData Is Nothing) Then
			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Bewerbungen konnten nicht geladen werden."))
			Return False
		End If

		Dim gridData = (From person In listOfData
										Select New ApplicationData With {
																								 .ID = person.ID,
																								 .Customer_ID = person.Customer_ID,
																								 .FK_ApplicantID = person.FK_ApplicantID,
																								 .VacancyNumber = person.VacancyNumber,
																								 .BusinessBranch = person.BusinessBranch,
																								 .Dismissalperiod = person.Dismissalperiod,
																								 .Availability = person.Availability,
																								 .Comment = person.Comment,
																								 .CreatedOn = person.CreatedOn,
																								 .CreatedFrom = person.CreatedFrom
																								}).ToList()

		Dim listDataSource As BindingList(Of ApplicationData) = New BindingList(Of ApplicationData)

		For Each p In gridData
			listDataSource.Add(p)
		Next

		grdApplication.DataSource = listDataSource


		Return Not listOfData Is Nothing

	End Function

	Private Function LoadApplicantData() As Boolean
		Dim success As Boolean = True

		Dim listOfData = m_AppDatabaseAccess.LoadApplicantData(m_customerID, String.Empty)

		If (listOfData Is Nothing) Then
			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Bewerberdaten konnten nicht geladen werden."))
			Return False
		End If

		Dim gridData = (From person In listOfData
										Select New ApplicantData With {
																								 .ID = person.ID,
																								 .Customer_ID = person.Customer_ID,
																								 .ApplicantNumber = person.ApplicantNumber,
																								 .Lastname = person.Lastname,
																								 .Firstname = person.Firstname,
																								 .Gender = person.Gender,
																								 .PostOfficeBox = person.PostOfficeBox,
																								 .Street = person.Street,
																								 .Postcode = person.Postcode,
																								 .Location = person.Location,
																								 .Country = person.Country,
																								 .Nationality = person.Nationality,
																								 .EMail = person.EMail,
																								 .Telephone = person.Telephone,
																								 .MobilePhone = person.MobilePhone,
																								 .Birthdate = person.Birthdate,
																								 .Permission = person.Permission,
																								 .Profession = person.Profession,
																								 .Auto = person.Auto,
																								 .Motorcycle = person.Motorcycle,
																								 .Bicycle = person.Bicycle,
																								 .DrivingLicence1 = person.DrivingLicence1,
																								 .DrivingLicence2 = person.DrivingLicence2,
																								 .DrivingLicence3 = person.DrivingLicence3,
																								 .CivilState = person.CivilState,
																								 .Language = person.Language,
																								 .LanguageLevel = person.LanguageLevel,
																								 .CreatedOn = person.CreatedOn,
																								 .CreatedFrom = person.CreatedFrom,
																								 .ChangedOn = person.ChangedOn,
																								 .ChangedFrom = person.ChangedFrom
																								}).ToList()

		Dim listDataSource As BindingList(Of ApplicantData) = New BindingList(Of ApplicantData)

		For Each p In gridData
			listDataSource.Add(p)
		Next

		grdApplicant.DataSource = listDataSource


		Return Not listOfData Is Nothing

	End Function

	Private Function LoadApplicationDocumentData() As Boolean
		Dim success As Boolean = True

		Dim listOfData = m_AppDatabaseAccess.LoadDocumentData(m_customerID, String.Empty)

		If (listOfData Is Nothing) Then
			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Bewerbungen konnten nicht geladen werden."))
			Return False
		End If

		Dim gridData = (From person In listOfData
										Select New ApplicantDocumentData With {
																								 .ID = person.ID,
																								 .FK_ApplicantID = person.FK_ApplicantID,
																								 .Type = person.Type,
																								 .Flag = person.Flag,
																								 .Title = person.Title,
																								 .FileExtension = person.FileExtension,
																								 .CreatedOn = person.CreatedOn,
																								 .CreatedFrom = person.CreatedFrom
																								}).ToList()

		Dim listDataSource As BindingList(Of ApplicantDocumentData) = New BindingList(Of ApplicantDocumentData)

		For Each p In gridData
			listDataSource.Add(p)
		Next

		grdDocument.DataSource = listDataSource


		Return Not listOfData Is Nothing

	End Function

	Private Function LoadCvProfileData() As Boolean
		Dim success As Boolean = True

		Dim listOfData = m_AppCVDatabaseAccess.LoadAllCvProfileData()

		If (listOfData Is Nothing) Then
			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Lebensläufe konnten nicht geladen werden."))
			Return False
		End If

		Dim gridData = (From person In listOfData
										Select New ApplicantCvProfileData With {
																								 .ID = person.ID,
																								 .Customer_ID = person.Customer_ID,
																								 .BusinessBranch = person.BusinessBranch,
																								 .TrxmlID = person.TrxmlID,
																								 .FK_CvPersonal = person.FK_CvPersonal,
																								 .FK_CvDocumentText = person.FK_CvDocumentText,
																								 .FK_CvDocumentHtml = person.FK_CvDocumentHtml,
																								 .CreatedOn = person.CreatedOn,
																								 .CreatedFrom = person.CreatedFrom
																								}).ToList()

		Dim listDataSource As BindingList(Of ApplicantCvProfileData) = New BindingList(Of ApplicantCvProfileData)

		For Each p In gridData
			listDataSource.Add(p)
		Next

		grdCVProfile.DataSource = listDataSource


		Return Not listOfData Is Nothing

	End Function


	Public Shared Function GenerateStreamFromText(ByRef text As String) As Stream
		Dim stream = New MemoryStream()
		Dim writer = New StreamWriter(stream)
		writer.Write(text)
		writer.Flush()
		stream.Position = 0
		Return stream
	End Function

	Private Sub DisplaySelectedDirectoryFileDetail()

		Dim fileextension = Path.GetExtension(m_SelectedFile)

		Try
			If fileextension.ToLower = ".pdf" Then
				xtabFileViewer.SelectedTabPage = xtabPDF
				pvApplicantData.LoadDocument(m_SelectedFile)

			ElseIf fileextension.ToLower = ".doc" OrElse fileextension.ToLower = ".docx" Then
				xtabFileViewer.SelectedTabPage = xtabDoc
				rtfApplicantData.LoadDocument(m_SelectedFile) ', DevExpress.XtraRichEdit.DocumentFormat.Doc)

				'Dim bData As Byte()
				'Dim br As BinaryReader = New BinaryReader(System.IO.File.OpenRead(m_SelectedFile))
				'bData = br.ReadBytes(br.BaseStream.Length)
				'Dim ms As MemoryStream = New MemoryStream(bData, 0, bData.Length)
				'rtfApplicantData.LoadDocumentTemplate(ms, DevExpress.XtraRichEdit.DocumentFormat.WordML)

			ElseIf fileextension.ToLower = ".rtf" Then
				xtabFileViewer.SelectedTabPage = xtabDoc
				rtfApplicantData.LoadDocument(m_SelectedFile, DevExpress.XtraRichEdit.DocumentFormat.Rtf)

			ElseIf fileextension.ToLower = ".txt" Then
				xtabFileViewer.SelectedTabPage = xtabDoc
				rtfApplicantData.LoadDocument(m_SelectedFile, DevExpress.XtraRichEdit.DocumentFormat.PlainText)

			ElseIf fileextension.ToLower = ".xml" Then
				xtabFileViewer.SelectedTabPage = xtabDoc
				rtfApplicantData.LoadDocument(m_SelectedFile, DevExpress.XtraRichEdit.DocumentFormat.PlainText)

			ElseIf fileextension.ToLower = ".bmp" OrElse fileextension.ToLower = ".jpg" OrElse fileextension.ToLower = ".png" Then
				xtabFileViewer.SelectedTabPage = xtabImage
				peApplicantData.Image = Image.FromFile(m_SelectedFile)

			Else
				xtabFileViewer.SelectedTabPage = xtabPDF

				pvApplicantData.CloseDocument()
				peApplicantData.Image = Nothing

			End If

		Catch ex As Exception
			pvApplicantData.CloseDocument()

		End Try


	End Sub

	Private Function DisplayAssignedApplicationData() As Boolean
		Dim success As Boolean = True

		ResetApplicationFields()
		Dim data = SelectedApplicationViewData
		If data Is Nothing Then
			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Bewerbungsdaten konnte nicht geladen werden."))
			Return False
		End If

		txtApplicationApplicationID.EditValue = data.ID
		txtApplicationCustomer_ID.EditValue = data.Customer_ID
		txtApplicationApplicationNumber.EditValue = data.ID
		txtApplicationApplicantNumber.EditValue = data.FK_ApplicantID
		txtApplicationVacancyNumber.EditValue = data.VacancyNumber
		txtApplicationBusinessBranch.EditValue = data.BusinessBranch
		txtApplicationDismissalperiod.EditValue = data.Dismissalperiod
		txtApplicationAvailability.EditValue = data.Availability
		txtApplicationComment.EditValue = data.Comment
		txtApplicationCreatedOn.EditValue = data.CreatedOn
		txtApplicationCreatedFrom.EditValue = data.CreatedFrom


		Return success

	End Function

	Private Function DisplayAssignedApplicantData() As Boolean
		Dim success As Boolean = True

		ResetApplicantFields()
		Dim data = SelectedApplicantViewData
		If data Is Nothing Then
			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Bewerberdaten konnte nicht geladen werden."))
			Return False
		End If

		txtApplicantLastName.EditValue = data.Lastname
		txtApplicantFirstname.EditValue = data.Firstname
		txtApplicantGender.EditValue = data.Gender
		txtApplicantStreet.EditValue = data.Street
		txtApplicantPostofficeBox.EditValue = data.PostOfficeBox
		txtApplicantPostcode.EditValue = data.Postcode
		txtApplicantLocation.EditValue = data.Location
		txtApplicantCountry.EditValue = data.Country
		txtApplicantNationality.EditValue = data.Nationality
		txtApplicantEMail.EditValue = data.EMail
		txtApplicantTelephone.EditValue = data.Telephone
		txtApplicantMobilePhone.EditValue = data.MobilePhone
		txtApplicantBirthDate.EditValue = data.Birthdate
		txtApplicantPermission.EditValue = data.Permission
		txtApplicantProfession.EditValue = data.Profession
		txtApplicantDrivingLicence1.EditValue = data.DrivingLicence1
		txtApplicantDrivingLicence2.EditValue = data.DrivingLicence2
		txtApplicantDrivingLicence3.EditValue = data.DrivingLicence3
		txtApplicantCivilstate.EditValue = data.CivilState
		lstLanguage.Items.Add(data.Language)

		chkApplicationAuto.Checked = data.Auto
		chkApplicationMotorcycle.Checked = data.Motorcycle
		chkApplicationBicycle.Checked = data.Bicycle

		txtApplicantCreatedOn.EditValue = data.CreatedOn
		txtApplicantCreatedFrom.EditValue = data.CreatedFrom
		txtApplicantChangedOn.EditValue = data.ChangedOn
		txtApplicantChangedFrom.EditValue = data.ChangedFrom


		Return success

	End Function

	Private Function DisplayAssignedDocumentData(ByVal recID As Integer) As Boolean
		Dim success As Boolean = True

		ResetDocumentFields()
		Dim documentData = m_AppDatabaseAccess.LoadAssignedDocumentData(recID)

		If (documentData Is Nothing) Then
			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Dokumentdaten konnte nicht geladen werden."))
			Return False
		End If
		m_CurrentFileExtension = String.Empty

		If Not documentData.Content Is Nothing Then
			Dim bytes() = documentData.Content
			Dim tempFileName = System.IO.Path.GetTempFileName()
			m_CurrentFileExtension = System.IO.Path.GetExtension(documentData.FileExtension)
			m_CurrentFileExtension = m_CurrentFileExtension.Replace(".", "")

			Dim tempFileFinal = System.IO.Path.ChangeExtension(tempFileName, m_CurrentFileExtension)

			If (Not bytes Is Nothing AndAlso m_Utility.WriteFileBytes(tempFileFinal, bytes)) Then

				If m_CurrentFileExtension.ToUpper.Contains("PDF") Then
					pvApplicantData.LoadDocument(tempFileFinal)

				ElseIf m_CurrentFileExtension.ToUpper.Contains("DOC") Then
					rtfApplicantData.Document.LoadDocument(tempFileFinal, DevExpress.XtraRichEdit.DocumentFormat.Doc)
				ElseIf m_CurrentFileExtension.ToUpper.Contains("RTF") Then
					rtfApplicantData.Document.LoadDocument(tempFileFinal, DevExpress.XtraRichEdit.DocumentFormat.Rtf)
				ElseIf m_CurrentFileExtension.ToUpper.Contains("XML") Then
					rtfApplicantData.Document.LoadDocument(tempFileFinal, DevExpress.XtraRichEdit.DocumentFormat.OpenXml)

				ElseIf m_CurrentFileExtension.ToUpper.Contains("BMP") OrElse m_CurrentFileExtension.ToUpper.Contains("PNG") OrElse m_CurrentFileExtension.ToUpper.Contains("JPG") Then
					peApplicantData.Image = Image.FromFile(tempFileFinal)

				End If
			End If



			'bytes = documentData.ProfilePicture
			'tempFileName = System.IO.Path.GetTempFileName()
			'Dim FileExtension = "JPG"

			'tempFileFinal = System.IO.Path.ChangeExtension(tempFileName, FileExtension)

			'If (Not bytes Is Nothing AndAlso m_Utility.WriteFileBytes(tempFileFinal, bytes)) Then
			'	peProfilePicture.Image = Image.FromFile(tempFileFinal)
			'End If

		End If

		txtDocumentDocumentID.EditValue = documentData.ID
		'txtDocumentCustomer_ID.EditValue = documentData.Customer_ID
		txtDocumentApplicationNumber.EditValue = documentData.ID
		txtDocumentApplicantNumber.EditValue = documentData.FK_ApplicantID
		txtDocumentType.EditValue = documentData.Type
		txtDocumentFlag.EditValue = documentData.Flag
		txtDocumentTitle.EditValue = documentData.Title

		txtApplicantCreatedOn.EditValue = documentData.CreatedOn
		txtApplicantCreatedFrom.EditValue = documentData.CreatedFrom


		Return success

	End Function

	Private Function DisplayAssignedCvProfileData() As Boolean
		Dim success As Boolean = True

		ResetcvProfileFields()
		Dim data = SelectedCvProfileViewData
		If data Is Nothing Then
			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Bewerberdaten konnte nicht geladen werden."))
			Return False
		End If
		m_currentTrXMLID = data.TrxmlID

		Dim profileData = m_AppCVDatabaseAccess.LoadCvProfileData(m_currentTrXMLID)
		Dim personalData = m_AppCVDatabaseAccess.LoadCvPersonalData(m_currentTrXMLID)
		Dim addressData = m_AppCVDatabaseAccess.LoadCvAddressData(m_currentTrXMLID)
		Dim phoneData = m_AppCVDatabaseAccess.LoadCvPhoneNumberData(m_currentTrXMLID)
		Dim emailData = m_AppCVDatabaseAccess.LoadCvEMailData(m_currentTrXMLID)
		Dim languageData = m_AppCVDatabaseAccess.LoadCvSkillData(m_currentTrXMLID)


		txtTrxmlID.EditValue = m_currentTrXMLID

		txtCvID.EditValue = profileData.ID
		txtCvCustomer_ID.EditValue = profileData.Customer_ID
		txtCvBusinessBranch.EditValue = profileData.BusinessBranch
		txtCvTrxmlID.EditValue = profileData.TrxmlID
		txtCvFK_CvPersonal.EditValue = profileData.FK_CvPersonal
		txtCvFK_CvDocumentText.EditValue = profileData.FK_CvDocumentText
		txtCvFK_CvDocumentHtml.EditValue = profileData.FK_CvDocumentHtml
		txtCvCreatedOn.EditValue = profileData.CreatedOn
		txtCvCreatedFrom.EditValue = profileData.CreatedFrom

		txtApplicantLastName.EditValue = personalData.LastName
		txtApplicantFirstname.EditValue = personalData.FirstName
		If Not personalData.FK_CvGender Is Nothing Then txtApplicantGender.EditValue = m_AppCVDatabaseAccess.GetCvGender(personalData.FK_CvGender).Description

		txtApplicantStreet.EditValue = addressData.StreetName
		txtApplicantPostofficeBox.EditValue = String.Empty
		txtApplicantPostcode.EditValue = addressData.PostalCode
		txtApplicantLocation.EditValue = addressData.City
		If Not addressData.FK_CvCountry Is Nothing Then txtApplicantCountry.EditValue = m_AppCVDatabaseAccess.GetCvCountry(addressData.FK_CvCountry)
		If Not personalData.FK_CvNationality Is Nothing Then txtApplicantNationality.EditValue = m_AppCVDatabaseAccess.GetCvNationality(personalData.FK_CvNationality)

		For Each number In phoneData
			If number.FK_CvPhoneNumberType <> 0 Then
				Dim phoneType = m_AppCVDatabaseAccess.GetCvPhoneNumberType(number.FK_CvPhoneNumberType)
				If number.FK_CvPhoneNumberType = 1 Then
					txtApplicantMobilePhone.EditValue = number.PhoneNumber
				ElseIf number.FK_CvPhoneNumberType = 2 OrElse number.FK_CvPhoneNumberType = 3 Then
					txtApplicantTelephone.EditValue = number.PhoneNumber
				End If
			End If
		Next

		For Each email In emailData
			txtApplicantEMail.EditValue &= If(String.IsNullOrWhiteSpace(txtApplicantEMail.EditValue), String.Empty, " | ") & email.Email
		Next

		If Not personalData.DateOfBirth Is Nothing AndAlso Not String.IsNullOrWhiteSpace(personalData.DateOfBirth) Then
			txtApplicantBirthDate.EditValue = String.Format("{0:d}", Date.Parse(personalData.DateOfBirth))
		End If

		txtApplicantPermission.EditValue = String.Empty
		txtApplicantProfession.EditValue = String.Empty

		If Not personalData.FK_CvDriversLicence Is Nothing Then txtApplicantDrivingLicence1.EditValue = m_AppCVDatabaseAccess.GetCvDriversLicence(personalData.FK_CvDriversLicence)
		txtApplicantDrivingLicence2.EditValue = String.Empty
		txtApplicantDrivingLicence3.EditValue = String.Empty
		If Not personalData.FK_CvMaritalStatus Is Nothing Then txtApplicantCivilstate.EditValue = m_AppCVDatabaseAccess.GetCvMaritalStatus(personalData.FK_CvMaritalStatus).Description

		For Each lang In languageData
			If Not lang.FK_CvLanguageSkillType Is Nothing Then
				Dim itm = m_AppCVDatabaseAccess.GetCvLanguageSkillTypeByID(lang.FK_CvLanguageSkillType)
				If Not itm Is Nothing Then
					Dim langBez = itm(0).Description

					If Not lang.FK_CvLanguageProficiency Is Nothing Then
						Dim proficiency = m_AppCVDatabaseAccess.GetCvLanguageProficiency(lang.FK_CvLanguageProficiency)
						If Not proficiency Is Nothing Then
							langBez &= If(Not String.IsNullOrWhiteSpace(langBez), " | ", String.Empty) & proficiency.Description
						End If
					End If
					lstLanguage.Items.Add(langBez)
				End If
			End If
		Next

		txtApplicantCreatedOn.EditValue = personalData.CreatedOn
		txtApplicantCreatedFrom.EditValue = personalData.CreatedFrom

		chkApplicationAuto.Checked = False
		chkApplicationMotorcycle.Checked = False
		chkApplicationBicycle.Checked = False

		Try
			LoadCVPictureData()
		Catch ex As Exception

		End Try

		btnDeleteCvProfile.Enabled = True


		Return success

	End Function

	Private Sub LoadCVPictureData()

		Dim pictureData = m_AppCVDatabaseAccess.LoadCvPictureData(m_currentTrXMLID)
		If pictureData Is Nothing OrElse pictureData.Content Is Nothing Then Return
		Dim bytes() As Byte = pictureData.Content
		Dim mem As MemoryStream = New MemoryStream(bytes)
		Dim bmp2 As Bitmap = New Bitmap(mem)

		peProfilePicture.Image = bmp2

	End Sub

	Private Sub OnbtnReloadCvProfile_Click(sender As Object, e As EventArgs) Handles btnReloadCvProfile.Click

		Dim success = LoadCvProfileData()

	End Sub

	Private Sub OnbtnDeleteCvProfile_Click(sender As Object, e As EventArgs) Handles btnDeleteCvProfile.Click
		Dim data = SelectedCvProfileViewData

		If data Is Nothing Then
			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Lebenslauf konnte nicht geladen werden."))
			Return
		End If

		Dim success = m_AppCVDatabaseAccess.DeleteCvProfileData(trxmlID:=data.TrxmlID.Value, deleteRelated:=True)
		If Not success Then
			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ihre Daten konnten nicht gelöscht werden."))
		Else
			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Ihre Daten wurden erfolgreich gelöscht."))

			ResetcvProfileFields()
		End If

	End Sub

	Private Sub FocusCvProfileData(ByVal trXMLID As Integer)

		If Not grdCVProfile.DataSource Is Nothing Then

			Dim cvData = CType(gvCVProfile.DataSource, BindingList(Of ApplicantCvProfileData))

			Dim index = cvData.ToList().FindIndex(Function(data) data.TrxmlID = trXMLID)

			Dim rowHandle = gvCVProfile.GetRowHandle(index)
			gvCVProfile.FocusedRowHandle = rowHandle

		End If

	End Sub


#Region "webservice calls"

	Private Function PerformSendingCVFileWithWebService(ByVal cvFileName As String) As Boolean
		Dim success = True

		'txtDocumentFileExtension.EditValue = New FileInfo(m_SelectedFile).Extension

		'Dim filecontent As Byte() = m_Utility.LoadFileBytes(cvFileName)
		''Dim tmfFileContent As Byte()

		'm_TrxmlContent = String.Empty
		'txtTrxmlID.Text = 0
		'lblTKTime.Text = "Gestartet..."

		'Dim Stopwatch As Stopwatch = New Stopwatch()

		'Stopwatch.Reset()
		'Stopwatch.Start()

		'Try

		'	SplashScreenManager.CloseForm(False)
		'	SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
		'	SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihr Dokument wird analysiert.") & Space(20))
		'	SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		'	ResetDetailFields()

		'	Dim webservice As New SPTextKernelService.ProcessDocumentInterfaceClient
		'	webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(webServiceTKProcessUri)

		'	m_TrxmlContent = webservice.processDocument(Account, User, PW, cvFileName, filecontent, Nothing, Nothing)

		'	rtfTrXML.Document.Text = m_TrxmlContent

		'	lblTKTime.Text = String.Format("Time: {0} Sekunden", Stopwatch.ElapsedMilliseconds() / 1000)

		'	success = success AndAlso SaveTrXMLDataToDatabase()
		'	'success = success AndAlso ParseTRXMLFile(m_TrxmlContent)

		'	If Not success Then
		'		m_UtilityUI.ShowErrorDialog("Die Trxml-Daten konnten nicht geladen werden.")

		'		Return False
		'	End If

		'	'success = success AndAlso DisplayTrXMLDetail()
		'	success = success AndAlso LoadCvProfileData()

		'	If success And m_currentTrXMLID.GetValueOrDefault(0) > 0 Then
		'		FocusCvProfileData(m_currentTrXMLID.GetValueOrDefault(0))

		'		'success = success AndAlso SaveApplicationDataIntoDatabase()

		'		'success = success AndAlso LoadData()
		'	End If


		'Catch ex As Exception
		'	SplashScreenManager.CloseForm(False)
		'	m_UtilityUI.ShowErrorDialog(ex.ToString)


		'Finally
		'	SplashScreenManager.CloseForm(False)

		'End Try

	End Function

	Private Function PerformLoadTrXMLWithWebService(ByVal trXMLID As Integer) As Boolean

	End Function

	Private Function PerformDeletingCVFileWithWebService(ByVal trXMLID As Integer) As Boolean

	End Function

	Private Sub btnSaveToDb_Click(sender As Object, e As EventArgs) Handles btnSaveToDb.Click
		Dim success As Boolean = True

		success = success AndAlso SaveTrXMLDataToDatabase()

	End Sub

	Private Function SaveTrXMLDataToDatabase() As Boolean
		Dim success As Boolean = True

		Dim trxmlImporter As New TrxmlImporter(m_InitializationData)
		success = success AndAlso trxmlImporter.ImportFromText(m_TrxmlContent)
		Dim profileData = trxmlImporter.ProfileData

		Dim trxmlContentData = profileData.GetDbaData ' trxmlImporter.GetTrXMLFileContent(Xsd.XmlReaderWriter.GenerateStreamFromText(m_TrxmlContent))
		m_currentTrXMLID = profileData.CustomArea.ExtraInfo.TrxmlId

		success = m_currentTrXMLID.GetValueOrDefault(0) > 0 AndAlso trxmlImporter.ImportFromText(m_TrxmlContent)


		Return success

	End Function

#End Region


#Region "parsing trXML file"

	Private Function ParseTRXMLFile(ByVal trxmlContent As String) As Boolean
		Dim match As Match = Nothing
		Dim success As Boolean = True

		m_Trxmldata = New TrXMLData

		Const DATAMATRIX_VALUE_PATTERN_TRXMLID As String = "<TrxmlID>(?<TrxmlID>.*?)</TrxmlID>"
		Const DATAMATRIX_VALUE_PATTERN_TOTALTIME As String = "<TotalTime>(?<TotalTime>.*?)</TotalTime>"
		Const DATAMATRIX_VALUE_PATTERN_TEMPLATINGTIME As String = "<TemplatingTime>(?<TemplatingTime>.*?)</TemplatingTime>"
		Const DATAMATRIX_VALUE_PATTERN_NORMALIZATIONTIME As String = "<NormalizationTime>(?<NormalizationTime>.*?)</NormalizationTime>"
		Const DATAMATRIX_VALUE_PATTERN_PREOCCINGTIME As String = "<ProcessingTime>(?<ProcessingTime>.*?)</ProcessingTime>"
		Const DATAMATRIX_VALUE_PATTERN_PREPOCESSINGTIME As String = "<PreProcessingTime>(?<PreProcessingTime>.*?)</PreProcessingTime>"

		Const DATAMATRIX_VALUE_PATTERN_FIRSTNAME As String = "<FirstName>(?<FirstName>.*?)</FirstName>"
		Const DATAMATRIX_VALUE_PATTERN_LASTNAME As String = "<LastName>(?<LastName>.*?)</LastName>"
		Const DATAMATRIX_VALUE_PATTERN_DATEOFBIRTH As String = "<DateOfBirth>(?<DateOfBirth>.*?)</DateOfBirth>"
		Const DATAMATRIX_VALUE_PATTERN_GENDERCODE As String = "<GenderCode>(?<GenderCode>.*?)</GenderCode>"
		Const DATAMATRIX_VALUE_PATTERN_NATIONALITYCODE As String = "<NationalityCode>(?<NationalityCode>.*?)</NationalityCode>"

		Const DATAMATRIX_VALUE_PATTERN_STREETNAME As String = "<StreetName>(?<StreetName>.*?)</StreetName>"
		Const DATAMATRIX_VALUE_PATTERN_STREETNUMBERBASE As String = "<StreetNumberBase>(?<StreetNumberBase>.*?)</StreetNumberBase>"
		Const DATAMATRIX_VALUE_PATTERN_POSTALCODE As String = "<PostalCode>(?<PostalCode>.*?)</PostalCode>"
		Const DATAMATRIX_VALUE_PATTERN_CITY As String = "<City>(?<City>.*?)</City>"
		Const DATAMATRIX_VALUE_PATTERN_COUNTRYCODE As String = "<CountryCode>(?<CountryCode>.*?)</CountryCode>"
		Const DATAMATRIX_VALUE_PATTERN_EMAIL As String = "<EMail>(?<EMail>.*?)</EMail>"
		Const DATAMATRIX_VALUE_PATTERN_MOBILEPHONE As String = "<MobilePhone>(?<MobilePhone>.*?)</MobilePhone>"
		Const DATAMATRIX_VALUE_PATTERN_HOMEPHONE As String = "<HomePhone>(?<HomePhone>.*?)</HomePhone>"

		Const DATAMATRIX_VALUE_PATTERN_MARITALSTATUSCODE As String = "<MaritalStatusCode>(?<MaritalStatusCode>.*?)</MaritalStatusCode>"
		Const DATAMATRIX_VALUE_PATTERN_DRIVERSLICENCE As String = "<DriversLicence>(?<DriversLicence>.*?)</DriversLicence>"
		Const DATAMATRIX_VALUE_PATTERN_BASE64CONTENT As String = "<Base64Content>(?<Base64Content>.*?)</Base64Content>"

		match = Regex.Match(m_TrxmlContent, DATAMATRIX_VALUE_PATTERN_TRXMLID, RegexOptions.IgnoreCase)
		m_Trxmldata.TrxmlID = Convert.ToInt32(match.Groups(1).Value)
		m_currentTrXMLID = m_Trxmldata.TrxmlID

		match = Regex.Match(m_TrxmlContent, DATAMATRIX_VALUE_PATTERN_TOTALTIME, RegexOptions.IgnoreCase)
		m_Trxmldata.Totaltime = match.Groups(1).Value
		match = Regex.Match(m_TrxmlContent, DATAMATRIX_VALUE_PATTERN_TEMPLATINGTIME, RegexOptions.IgnoreCase)
		m_Trxmldata.TemplatingTime = match.Groups(1).Value
		match = Regex.Match(m_TrxmlContent, DATAMATRIX_VALUE_PATTERN_NORMALIZATIONTIME, RegexOptions.IgnoreCase)
		m_Trxmldata.NormalizationTime = match.Groups(1).Value
		match = Regex.Match(m_TrxmlContent, DATAMATRIX_VALUE_PATTERN_PREOCCINGTIME, RegexOptions.IgnoreCase)
		m_Trxmldata.ProcessingTime = match.Groups(1).Value
		match = Regex.Match(m_TrxmlContent, DATAMATRIX_VALUE_PATTERN_PREPOCESSINGTIME, RegexOptions.IgnoreCase)
		m_Trxmldata.PreProcessingTime = match.Groups(1).Value

		match = Regex.Match(m_TrxmlContent, DATAMATRIX_VALUE_PATTERN_FIRSTNAME, RegexOptions.IgnoreCase)
		m_Trxmldata.FirstName = match.Groups(1).Value
		match = Regex.Match(m_TrxmlContent, DATAMATRIX_VALUE_PATTERN_LASTNAME, RegexOptions.IgnoreCase)
		m_Trxmldata.LastName = match.Groups(1).Value
		match = Regex.Match(m_TrxmlContent, DATAMATRIX_VALUE_PATTERN_DATEOFBIRTH, RegexOptions.IgnoreCase)
		m_Trxmldata.DateOfBirth = match.Groups(1).Value
		match = Regex.Match(m_TrxmlContent, DATAMATRIX_VALUE_PATTERN_GENDERCODE, RegexOptions.IgnoreCase)
		m_Trxmldata.Gender = match.Groups(1).Value

		match = Regex.Match(m_TrxmlContent, DATAMATRIX_VALUE_PATTERN_NATIONALITYCODE, RegexOptions.IgnoreCase)
		m_Trxmldata.Nationality = match.Groups(1).Value
		match = Regex.Match(m_TrxmlContent, DATAMATRIX_VALUE_PATTERN_STREETNAME, RegexOptions.IgnoreCase)
		m_Trxmldata.StreetName = match.Groups(1).Value
		match = Regex.Match(m_TrxmlContent, DATAMATRIX_VALUE_PATTERN_STREETNUMBERBASE, RegexOptions.IgnoreCase)
		m_Trxmldata.StreetNumberBase = match.Groups(1).Value
		match = Regex.Match(m_TrxmlContent, DATAMATRIX_VALUE_PATTERN_POSTALCODE, RegexOptions.IgnoreCase)
		m_Trxmldata.PostalCode = match.Groups(1).Value
		match = Regex.Match(m_TrxmlContent, DATAMATRIX_VALUE_PATTERN_CITY, RegexOptions.IgnoreCase)
		m_Trxmldata.City = match.Groups(1).Value
		match = Regex.Match(m_TrxmlContent, DATAMATRIX_VALUE_PATTERN_COUNTRYCODE, RegexOptions.IgnoreCase)
		m_Trxmldata.CountryCode = match.Groups(1).Value

		match = Regex.Match(m_TrxmlContent, DATAMATRIX_VALUE_PATTERN_EMAIL, RegexOptions.IgnoreCase)
		m_Trxmldata.Email = match.Groups(1).Value
		match = Regex.Match(m_TrxmlContent, DATAMATRIX_VALUE_PATTERN_MOBILEPHONE, RegexOptions.IgnoreCase)
		m_Trxmldata.MobilePhone = match.Groups(1).Value
		match = Regex.Match(m_TrxmlContent, DATAMATRIX_VALUE_PATTERN_HOMEPHONE, RegexOptions.IgnoreCase)
		m_Trxmldata.HomePhone = match.Groups(1).Value

		match = Regex.Match(m_TrxmlContent, DATAMATRIX_VALUE_PATTERN_MARITALSTATUSCODE, RegexOptions.IgnoreCase)
		m_Trxmldata.MaritalStatusCode = match.Groups(1).Value

		match = Regex.Match(m_TrxmlContent, DATAMATRIX_VALUE_PATTERN_DRIVERSLICENCE, RegexOptions.IgnoreCase)
		m_Trxmldata.DrivingLicence = match.Groups(1).Value

		match = Regex.Match(m_TrxmlContent, DATAMATRIX_VALUE_PATTERN_BASE64CONTENT, RegexOptions.IgnoreCase)
		m_Trxmldata.ProfilePicture = match.Groups(1).Value


		Return Not (m_Trxmldata Is Nothing)

	End Function

	Private Function DisplayTrXMLDetail() As Boolean
		Dim success As Boolean = True

		success = success AndAlso ParseTRXMLFile(m_TrxmlContent)
		txtTrxmlID.EditValue = m_currentTrXMLID

		txtApplicantLastName.EditValue = m_Trxmldata.LastName
		txtApplicantFirstname.EditValue = m_Trxmldata.FirstName
		txtApplicantGender.EditValue = m_Trxmldata.GenderLabel
		txtApplicantStreet.EditValue = m_Trxmldata.StreetName
		txtApplicantPostofficeBox.EditValue = String.Empty
		txtApplicantPostcode.EditValue = m_Trxmldata.PostalCode
		txtApplicantLocation.EditValue = m_Trxmldata.City
		txtApplicantCountry.EditValue = m_Trxmldata.CountryCode
		txtApplicantNationality.EditValue = m_Trxmldata.Nationality

		txtApplicantEMail.EditValue = m_Trxmldata.Email
		txtApplicantTelephone.EditValue = m_Trxmldata.HomePhone
		txtApplicantMobilePhone.EditValue = m_Trxmldata.MobilePhone
		If Not m_Trxmldata.DateOfBirth Is Nothing AndAlso Not String.IsNullOrWhiteSpace(m_Trxmldata.DateOfBirth) Then
			txtApplicantBirthDate.EditValue = String.Format("{0:d}", Date.Parse(m_Trxmldata.DateOfBirth))
		End If
		txtApplicantPermission.EditValue = String.Empty
		txtApplicantProfession.EditValue = String.Empty

		txtApplicantDrivingLicence1.EditValue = m_Trxmldata.DrivingLicence
		txtApplicantDrivingLicence2.EditValue = String.Empty
		txtApplicantDrivingLicence3.EditValue = String.Empty
		txtApplicantCivilstate.EditValue = m_Trxmldata.MaritalStatusLable
		lstLanguage.Items.Add(String.Empty)

		txtApplicantCreatedOn.EditValue = String.Empty
		txtApplicantCreatedFrom.EditValue = String.Empty
		txtApplicantChangedOn.EditValue = String.Empty
		txtApplicantChangedFrom.EditValue = String.Empty

		chkApplicationAuto.Checked = False
		chkApplicationMotorcycle.Checked = False
		chkApplicationBicycle.Checked = False

		Try
			If Not String.IsNullOrWhiteSpace(m_Trxmldata.ProfilePicture) Then
				LoadAplicantImageFromTrXML()
			End If

		Catch ex As Exception

		End Try

		Return success

	End Function

	Private Sub LoadAplicantImageFromTrXML()

		Dim val As String = m_Trxmldata.ProfilePicture
		If String.IsNullOrWhiteSpace(val) Then Return
		Dim bytes() As Byte = Convert.FromBase64String(val)
		Dim mem As MemoryStream = New MemoryStream(bytes)
		Dim bmp2 As Bitmap = New Bitmap(mem)

		peProfilePicture.Image = bmp2

	End Sub


	'Private Function SaveApplicationDataIntoDatabase() As Boolean
	'	Dim success As Boolean = True

	'	Dim applicationData As New ApplicationData

	'	applicationData.Customer_ID = m_customerID
	'	applicationData.VacancyNumber = txtApplicationVacancyNumber.EditValue
	'	applicationData.BusinessBranch = txtApplicationBusinessBranch.EditValue
	'	applicationData.Advisor = txtApplicationAdvisor.EditValue
	'	applicationData.Dismissalperiod = txtApplicationDismissalperiod.EditValue
	'	applicationData.Availability = txtApplicationAvailability.EditValue
	'	applicationData.Comment = txtApplicationComment.EditValue
	'	applicationData.CreatedFrom = txtApplicationCreatedFrom.EditValue




	'	Dim applicantData As New ApplicantData
	'	Dim documentData As New ApplicantDocumentData

	'	Dim listOfData '= m_AppDatabaseAccess.LoadCVApplicantData(m_customerID, m_currentTrXMLID)

	'	If (listOfData Is Nothing) Then
	'		SplashScreenManager.CloseForm(False)
	'		m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Bewerbungen konnten nicht geladen werden."))
	'		Return False
	'	End If

	'	applicantData.Customer_ID = m_customerID
	'	applicantData.Lastname = txtApplicantLastName.EditValue
	'	applicantData.Firstname = txtApplicantFirstname.EditValue
	'	Dim gender As String = txtApplicantGender.EditValue
	'	If String.IsNullOrWhiteSpace(gender) Or gender = "unbekannt" Then
	'		applicantData.Gender = 0
	'	ElseIf gender = "mann" Then
	'		applicantData.Gender = 1
	'	Else
	'		applicantData.Gender = 2
	'	End If
	'	applicantData.Street = txtApplicantStreet.EditValue
	'	applicantData.PostOfficeBox = txtApplicantPostofficeBox.EditValue
	'	applicantData.Postcode = txtApplicantPostcode.EditValue
	'	applicantData.Location = txtApplicantLocation.EditValue
	'	applicantData.Country = txtApplicantCountry.EditValue
	'	applicantData.Nationality = txtApplicantNationality.EditValue
	'	applicantData.EMail = txtApplicantEMail.EditValue
	'	applicantData.Telephone = txtApplicantTelephone.EditValue
	'	applicantData.MobilePhone = txtApplicantMobilePhone.EditValue
	'	If Not String.IsNullOrWhiteSpace(txtApplicantBirthDate.EditValue) Then
	'		applicantData.Birthdate = CDate(txtApplicantBirthDate.EditValue)
	'	End If

	'	applicantData.Profession = txtApplicantProfession.EditValue
	'	applicantData.DrivingLicence1 = txtApplicantDrivingLicence1.EditValue
	'	applicantData.DrivingLicence2 = txtApplicantDrivingLicence2.EditValue
	'	applicantData.DrivingLicence3 = txtApplicantDrivingLicence3.EditValue

	'	Dim civilState As String = txtApplicantCivilstate.EditValue.ToString.ToLower
	'	If String.IsNullOrWhiteSpace(civilState) Or civilState.Contains("bekannt") Then
	'		applicantData.CivilState = 0
	'	ElseIf civilState.Contains("verwi") Then
	'		applicantData.CivilState = 4
	'	ElseIf civilState.Contains("unverhei") Then
	'		applicantData.CivilState = 2
	'	ElseIf civilState.Contains("verhei") Then
	'		applicantData.CivilState = 1
	'	ElseIf civilState.Contains("gesch") Then
	'		applicantData.CivilState = 5
	'	ElseIf civilState.Contains("zusammenle") Then
	'		applicantData.CivilState = 3
	'	Else
	'		applicantData.CivilState = 1
	'	End If


	'	applicantData.Permission = txtApplicantPermission.EditValue
	'	If Not String.IsNullOrWhiteSpace(txtApplicantCreatedOn.EditValue) Then
	'		applicantData.CreatedOn = CDate(txtApplicantCreatedOn.EditValue)
	'	End If
	'	applicantData.CreatedFrom = txtApplicantCreatedFrom.EditValue

	'	If Not String.IsNullOrWhiteSpace(txtApplicantChangedOn.EditValue) Then
	'		applicantData.ChangedOn = ReplaceMissing(txtApplicantChangedOn.EditValue, Nothing)
	'	End If
	'	applicantData.ChangedFrom = txtApplicantChangedFrom.EditValue

	'	applicantData.Auto = chkApplicationAuto.Checked
	'	applicantData.Motorcycle = chkApplicationMotorcycle.Checked
	'	applicantData.Bicycle = chkApplicationBicycle.Checked


	'	success = success AndAlso m_AppDatabaseAccess.AddApplicationWithApplicant(applicationData, applicantData)

	'	If Not success Then
	'		SplashScreenManager.CloseForm(False)
	'		m_Logger.LogError("Ihre Bewerberdaten konnten nicht gespeichert werden.")
	'		m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ihre Daten konnten nicht gespeichert werden."))
	'		Return False
	'	End If

	'	documentData.Customer_ID = m_customerID
	'	documentData.ApplicationNumber = applicationData.ApplicationNumber
	'	documentData.FK_ApplicantID = applicationData.ApplicantNumber

	'	documentData.TrXMLResult = rtfTrXML.Text
	'	Dim filecontent As Byte() = m_Utility.LoadFileBytes(m_SelectedFile)
	'	documentData.Content = filecontent
	'	documentData.TrXMLID = m_Trxmldata.TrxmlID
	'	documentData.CreatedFrom = "System"
	'	documentData.CreatedOn = Now

	'	If Not peProfilePicture.Image Is Nothing Then
	'		documentData.ProfilePicture = imageToByteArray(peProfilePicture.Image)
	'	End If

	'	documentData.Type = 201
	'	documentData.Flag = 0
	'	documentData.FileExtension = txtDocumentFileExtension.EditValue
	'	documentData.Title = String.Format("{0} {1}", txtApplicantFirstname.EditValue, txtApplicantLastName.EditValue)
	'	documentData.TrXMLCreatedOn = txtDocumentTrXMLCreatedOn.EditValue

	'	success = success AndAlso m_AppDatabaseAccess.AddApplicantDocument(documentData)

	'	If Not success Then
	'		SplashScreenManager.CloseForm(False)
	'		m_Logger.LogError("Ihr Dokument konnten nicht gespeichert werden.")
	'		m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ihr Dokument konnten nicht gespeichert werden."))
	'		Return False
	'	End If


	'	Return success

	'End Function

#End Region


	Sub OngvFiles_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvFiles.RowCellClick
		Dim data = SelectedFileViewData
		If data Is Nothing Then
			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verzeichnis-Dateien konnten nicht geladen werden."))
			Return
		End If

		m_SelectedFile = Path.Combine(txtPath.EditValue, data.Filename)
		DisplaySelectedDirectoryFileDetail()

		If e.Clicks = 2 Then
			If String.IsNullOrWhiteSpace(m_SelectedFile) Then Return
			m_Utility.OpenFileWithDefaultProgram(m_SelectedFile)
		End If

	End Sub

	Sub OngvApplication_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvApplication.RowCellClick
		DisplayAssignedApplicationData()
	End Sub

	Sub OngvApplicant_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvApplicant.RowCellClick
		DisplayAssignedApplicantData()
	End Sub

	Sub OngvDocument_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvDocument.RowCellClick

		Dim data = SelectedApplicantDocumentViewData
		If data Is Nothing Then
			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Dokumentdaten konnte nicht geladen werden."))
			Return
		End If

		DisplayAssignedDocumentData(data.ID)

		If e.Clicks = 2 Then LoadAssignedFile(data.ID)

	End Sub

	Sub OngvCVProfile_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvCVProfile.RowCellClick
		DisplayAssignedCvProfileData()
	End Sub



#Region "iFrame methodes"

	Private Function DoLogin() As String
		m_JSessionID = Nothing

		Dim loginUrl = String.Format(accountUrl, Account, User, PW)
		WebBrowser1.Navigate(loginUrl)

		Dim maxTimeUtc = DateTime.UtcNow.AddSeconds(5D)
		While m_JSessionID Is Nothing AndAlso DateTime.UtcNow < maxTimeUtc
			' Warten bis JSessionID in WebBrowser1_DocumentCompleted gesetzt wird
			Application.DoEvents()
		End While
		Return m_JSessionID
	End Function

	Private Sub DoLogin2()
		Dim loginUrl = String.Format(accountUrl, Account, User, PW)

		Dim responsebody As String
		Using client As New Net.WebClient
			Dim reqparm As New Specialized.NameValueCollection
			reqparm.Add("account", Account)
			reqparm.Add("username", User)
			reqparm.Add("password", PW)
			Dim responsebytes = client.UploadValues(TEXTKERNEL_LOGINURI, "POST", reqparm)
			responsebody = (New UTF8Encoding).GetString(responsebytes)
			Console.WriteLine(responsebody)
			Dim cookies = client.ResponseHeaders("Set-Cookie")
			Console.WriteLine(cookies)
		End Using
	End Sub

	Private Sub ParseDocument(ByVal sender As Object, ByVal e As WebBrowserDocumentCompletedEventArgs)
		If txtSearchURL.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(txtSearchURL.EditValue) Then
			Dim jsessionID As String = txtJSessionID.EditValue
			Dim trXmlID As String = txtTrxmlID.EditValue

			'If jsessionID = String.Empty Then jsessionID = "4919AD2527FA1DA5BDED763EC011F7F8-986"
			If trXmlID = String.Empty Then
				trXmlID = "53854028"
				txtTrxmlID.EditValue = trXmlID
			End If

			'Dim webClient As New System.Net.WebClient
			'Dim result As String = webClient.DownloadString(accountUrl)

			Dim editContentUrl = String.Format(changeContentUrl, jsessionID, trXmlID)
			WebBrowser1.Navigate(editContentUrl)
			txtSearchURL.EditValue = editContentUrl

		Else
			WebBrowser1.Navigate(txtSearchURL.EditValue)
		End If

	End Sub

	Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
		WebBrowser1.Navigate("https://staging.textkernel.nl/match/logout.jsp")
	End Sub

	Private Sub btnLoadSite_Click(sender As Object, e As EventArgs) Handles btnLoadSite.Click
		Dim frm As New frmSourceBox

		frm.TrXMLID = 50032520
		frm.LoadCV(50032520)
		frm.Show()
		frm.BringToFront()

		'txtJSessionID.EditValue = DoLogin()
		'  If txtJSessionID.Text.Length > 0 Then
		'    LoadDoc()
		'  End If
	End Sub

	Private Sub LoadDoc()
		If txtSearchURL.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(txtSearchURL.EditValue) Then
			Dim jsessionID As String = txtJSessionID.EditValue
			Dim trXmlID As String = txtTrxmlID.EditValue

			'If jsessionID = String.Empty Then jsessionID = "4919AD2527FA1DA5BDED763EC011F7F8-986"
			If trXmlID = String.Empty Then
				trXmlID = "49964204" '"53854028"
				txtTrxmlID.EditValue = trXmlID
			End If

			'Dim webClient As New System.Net.WebClient
			'Dim result As String = webClient.DownloadString(accountUrl)

			Dim editContentUrl = String.Format(changeContentUrl, jsessionID, trXmlID)
			WebBrowser1.Navigate(editContentUrl)
			txtSearchURL.EditValue = editContentUrl

		Else
			WebBrowser1.Navigate(txtSearchURL.EditValue)
		End If
	End Sub

#End Region

	Private Sub OnbtnTKSend_Click(sender As Object, e As EventArgs) Handles btnTKSend.Click
		Dim success = PerformSendingCVFileWithWebService(m_SelectedFile)
	End Sub

	Private Sub OnbtnGetTrxmlID_Click(sender As Object, e As EventArgs) Handles btnGetTrxmlID.Click
		Dim m_Utility As New Utility
		Dim cvFileName As String = m_SelectedFile

		Dim filecontent As Byte() = m_Utility.LoadFileBytes(cvFileName)
		Dim myText As String = rtfTrXML.Document.Text
		Dim Stopwatch As Stopwatch = New Stopwatch()
		lblTKTime.Text = "Gestartet..."
		Stopwatch.Reset()
		Stopwatch.Start()

		txtTrxmlID.Text = 0

		Try
			Dim trxmlid As New List(Of String)()

			For Each i As Match In Regex.Matches(myText, "<Trxmlid>(?<trxmlid>.*?)</Trxmlid>", RegexOptions.IgnoreCase)
				trxmlid.Add(i.Groups("trxmlid").Value)
			Next
			txtTrxmlID.Text = trxmlid(0)

			lblTKTime.Text = String.Format("Time: {0} Sekunden", Stopwatch.ElapsedMilliseconds() / 1000)

		Catch ex As Exception

		End Try

	End Sub

	Private Sub btnTKGet_Click(sender As Object, e As EventArgs) Handles btnTKGet.Click
		Dim success = PerformLoadTrXMLWithWebService(m_currentTrXMLID)
	End Sub

	Private Sub OnbtnTKDelete_Click(sender As Object, e As EventArgs) Handles btnTKDelete.Click
		Dim success = PerformDeletingCVFileWithWebService(m_currentTrXMLID)
		If Not success Then
			Return
		End If

		success = success AndAlso m_AppDatabaseAccess.DeleteAssignedApplication(m_customerID, txtApplicationApplicantNumber.EditValue)
		m_currentTrXMLID = 0

	End Sub

	Private Sub btnLoadData_Click(sender As Object, e As EventArgs) Handles btnLoadData.Click
		Dim success = LoadData()
	End Sub

	Private Sub btnOpenDocument_Click(sender As Object, e As EventArgs) Handles btnOpenDocument.Click
		Dim success = LoadAssignedFile(txtDocumentDocumentID.EditValue)
	End Sub

	Private Sub btnSaveToSourceBox_Click(sender As Object, e As EventArgs) Handles btnSaveToSourceBox.Click

	End Sub

	Private Sub WebBrowser1_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles WebBrowser1.DocumentCompleted
		'Dim header = WebBrowser1.Document.GetElementsByTagName("head")(0)
		Console.WriteLine(e.Url)
		If (String.IsNullOrWhiteSpace(m_JSessionID)) Then
			If Not WebBrowser1.Document.Cookie Is Nothing Then
				Dim cookies = WebBrowser1.Document.Cookie.Split(";")
				For Each cookie In cookies
					Dim cookieParts = cookie.Split("=")
					Select Case cookieParts(0).Trim()
						Case "JSESSIONID"
							m_JSessionID = cookieParts(1).Trim()
							Exit For
					End Select
				Next
			End If
		End If
	End Sub




#Region "parsing CVLizer-XML file"


	Private Sub OnbtnReadCVLizerXML_Click(sender As Object, e As EventArgs) Handles btnReadCVLizerXML.Click

		LoadCVFileFromXML(m_SelectedFile)

	End Sub

	Private Function LoadCVFileFromXML(ByVal cvFileName As String) As Boolean
		Dim success = True

		txtDocumentFileExtension.EditValue = New FileInfo(m_SelectedFile).Extension

		m_TrxmlContent = String.Empty
		Dim file As New FileInfo(cvFileName)
		Dim xmlFieContent As String = file.OpenText().ReadToEnd()

		m_TrxmlContent = xmlFieContent

		success = success AndAlso DisplayCvLizerXMLDetail()

		Return success

	End Function

	Private Function PerformSendingCVFileToCVLizerWithWebService(ByVal cvFileName As String) As Boolean
		Dim success = True

		'txtDocumentFileExtension.EditValue = New FileInfo(m_SelectedFile).Extension

		'Dim file As New FileInfo(cvFileName)
		'Dim xmlFieContent As String = file.OpenText().ReadToEnd()





		'Dim filecontent As Byte() = m_Utility.LoadFileBytes(cvFileName)

		'm_TrxmlContent = String.Empty
		'txtTrxmlID.Text = 0
		'lblTKTime.Text = "Gestartet..."

		'Dim Stopwatch As Stopwatch = New Stopwatch()

		'Stopwatch.Reset()
		'Stopwatch.Start()

		'Try

		'	SplashScreenManager.CloseForm(False)
		'	SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
		'	SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihr Dokument wird analysiert.") & Space(20))
		'	SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		'	ResetDetailFields()

		'	Dim webservice As New SPTextKernelService.ProcessDocumentInterfaceClient
		'	webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(webServiceTKProcessUri)

		'	m_TrxmlContent = webservice.processDocument(Account, User, PW, cvFileName, filecontent, Nothing, Nothing)

		'	rtfTrXML.Document.Text = m_TrxmlContent

		'	lblTKTime.Text = String.Format("Time: {0} Sekunden", Stopwatch.ElapsedMilliseconds() / 1000)

		'	success = success AndAlso SaveTrXMLDataToDatabase()
		'	'success = success AndAlso ParseTRXMLFile(m_TrxmlContent)

		'	If Not success Then
		'		m_UtilityUI.ShowErrorDialog("Die Trxml-Daten konnten nicht geladen werden.")

		'		Return False
		'	End If

		'	'success = success AndAlso DisplayTrXMLDetail()
		'	success = success AndAlso LoadCvProfileData()

		'	If success And m_currentTrXMLID.GetValueOrDefault(0) > 0 Then
		'		FocusCvProfileData(m_currentTrXMLID.GetValueOrDefault(0))

		'		'success = success AndAlso SaveApplicationDataIntoDatabase()

		'		'success = success AndAlso LoadData()
		'	End If


		'Catch ex As Exception
		'	SplashScreenManager.CloseForm(False)
		'	m_UtilityUI.ShowErrorDialog(ex.ToString)


		'Finally
		'	SplashScreenManager.CloseForm(False)

		'End Try

	End Function


	Private Function DisplayCvLizerXMLDetail() As Boolean
		Dim success As Boolean = True
		'Dim cvLizerUtility = New Xsd.CVLizerLoader.CustomAreaType(m_SelectedFile)

		'm_CVLizerXMLData = cvLizerUtility.ImportFromText(m_TrxmlContent)
		'success = success AndAlso Not (m_CVLizerXMLData Is Nothing)
		'If Not m_CVLizerXMLData Is Nothing OrElse Not success Then Return False

		Return False




		txtTrxmlID.EditValue = m_currentTrXMLID

		txtApplicantLastName.EditValue = m_Trxmldata.LastName
		txtApplicantFirstname.EditValue = m_Trxmldata.FirstName
		txtApplicantGender.EditValue = m_Trxmldata.GenderLabel
		txtApplicantStreet.EditValue = m_Trxmldata.StreetName
		txtApplicantPostofficeBox.EditValue = String.Empty
		txtApplicantPostcode.EditValue = m_Trxmldata.PostalCode
		txtApplicantLocation.EditValue = m_Trxmldata.City
		txtApplicantCountry.EditValue = m_Trxmldata.CountryCode
		txtApplicantNationality.EditValue = m_Trxmldata.Nationality

		txtApplicantEMail.EditValue = m_Trxmldata.Email
		txtApplicantTelephone.EditValue = m_Trxmldata.HomePhone
		txtApplicantMobilePhone.EditValue = m_Trxmldata.MobilePhone
		If Not m_Trxmldata.DateOfBirth Is Nothing AndAlso Not String.IsNullOrWhiteSpace(m_Trxmldata.DateOfBirth) Then
			txtApplicantBirthDate.EditValue = String.Format("{0:d}", Date.Parse(m_Trxmldata.DateOfBirth))
		End If
		txtApplicantPermission.EditValue = String.Empty
		txtApplicantProfession.EditValue = String.Empty

		txtApplicantDrivingLicence1.EditValue = m_Trxmldata.DrivingLicence
		txtApplicantDrivingLicence2.EditValue = String.Empty
		txtApplicantDrivingLicence3.EditValue = String.Empty
		txtApplicantCivilstate.EditValue = m_Trxmldata.MaritalStatusLable
		lstLanguage.Items.Add(String.Empty)

		txtApplicantCreatedOn.EditValue = String.Empty
		txtApplicantCreatedFrom.EditValue = String.Empty
		txtApplicantChangedOn.EditValue = String.Empty
		txtApplicantChangedFrom.EditValue = String.Empty

		chkApplicationAuto.Checked = False
		chkApplicationMotorcycle.Checked = False
		chkApplicationBicycle.Checked = False

		Try
			If Not String.IsNullOrWhiteSpace(m_Trxmldata.ProfilePicture) Then
				LoadAplicantImageFromCVLizerXML()
			End If

		Catch ex As Exception

		End Try

		Return success

	End Function

	'Private Function ParseCVLizserXMLFile(ByVal xmlContent As String) As Boolean
	'	Dim match As Match = Nothing
	'	Dim success As Boolean = True

	'	Dim cvLizerData = New CVLizerXMLData
	'	Dim personalinfomationData = New PersonalInformationData
	'	Dim workphaseData = New List(Of WorkPhaseData)
	'	Dim works As New WPhaseData


	'	Try

	'		Dim doc As XDocument = XDocument.Load(m_SelectedFile)
	'		Dim ns As XNamespace = doc.Root.Name.Namespace

	'		Dim xelement As XElement = XElement.Load(m_SelectedFile)
	'		Dim addresses = From address In xelement.Elements(ns + "personalInformation")
	'										Select address

	'		For Each XEL1 As XElement In addresses
	'			Console.WriteLine(XEL1)

	'			personalinfomationData.FirstName = GetSafeStringFromXElement(XEL1.Element(ns + "firstname"))
	'			personalinfomationData.LastName = GetSafeStringFromXElement(XEL1.Element(ns + "lastname"))
	'			personalinfomationData.Email = GetSafeStringFromXElement(XEL1.Element(ns + "email"))
	'			personalinfomationData.Title = GetSafeStringFromXElement(XEL1.Element(ns + "title"))

	'			Dim genderCode = GetSafeStringFromXElement(XEL1.Element(ns + "gender").Element(ns + "code"))
	'			Dim genderName = GetSafeStringFromXElement(XEL1.Element(ns + "gender").Element(ns + "name"))
	'			personalinfomationData.Gender = New GenderData With {.Code = genderCode, .CodeName = genderName}

	'			Dim iscedCode = GetSafeStringFromXElement(XEL1.Element(ns + "isced").Element(ns + "code"))
	'			Dim iscedName = GetSafeStringFromXElement(XEL1.Element(ns + "isced").Element(ns + "name"))
	'			personalinfomationData.IsCed = New IsCedData With {.Code = iscedCode, .CodeName = iscedName}

	'			personalinfomationData.DateOfBirth = GetSafeStringFromXElement(XEL1.Element(ns + "birthdate"))
	'			personalinfomationData.DateOfBirthYear = GetSafeStringFromXElement(XEL1.Element(ns + "birthyear"))
	'			personalinfomationData.DateOfBirthPlace = GetSafeStringFromXElement(XEL1.Element(ns + "birthplace"))

	'			Dim nationalityCode = GetSafeStringFromXElement(XEL1.Element(ns + "nationality").Element(ns + "code"))
	'			Dim nationalityName = GetSafeStringFromXElement(XEL1.Element(ns + "nationality").Element(ns + "name"))
	'			personalinfomationData.Nationality = New NationalityData With {.Code = nationalityCode, .CodeName = nationalityName}

	'			Dim civilstateCode = GetSafeStringFromXElement(XEL1.Element(ns + "civilState").Element(ns + "code"))
	'			Dim civilstateName = GetSafeStringFromXElement(XEL1.Element(ns + "civilState").Element(ns + "name"))
	'			personalinfomationData.CivilStatus = New CivilStateData With {.Code = civilstateCode, .CodeName = civilstateName}

	'			Dim addressStreet = GetSafeStringFromXElement(XEL1.Element(ns + "address").Element(ns + "street"))
	'			Dim addressPostcode = GetSafeStringFromXElement(XEL1.Element(ns + "address").Element(ns + "postcode"))
	'			Dim addressCity = GetSafeStringFromXElement(XEL1.Element(ns + "address").Element(ns + "city"))
	'			Dim addressCountryCode = GetSafeStringFromXElement(XEL1.Element(ns + "address").Element(ns + "country").Element(ns + "code"))
	'			Dim addressCountryName = GetSafeStringFromXElement(XEL1.Element(ns + "address").Element(ns + "country").Element(ns + "name"))
	'			Dim addressState = GetSafeStringFromXElement(XEL1.Element(ns + "address").Element(ns + "state"))
	'			personalinfomationData.Address = New AddressData With {.Street = addressStreet,
	'					.Postcode = addressPostcode,
	'					.City = addressCity,
	'					.Country = New CountryData With {.Code = addressCountryCode, .Name = addressCountryName},
	'					.State = addressState}

	'			If Not XEL1.Element(ns + "phoneNumber") Is Nothing Then
	'				Dim phone As New List(Of PhoneNumberData)
	'				For Each XEL2 As XElement In XEL1.Elements.Where(Function(d) d.Name = ns + "phoneNumber")
	'					Dim data = New PhoneNumberData
	'					data = (New PhoneNumberData With {.Code = "1", .PhoneNumber = XEL2.Value})
	'					phone.Add(data)
	'				Next
	'				personalinfomationData.PhoneNumbers = phone
	'			End If

	'		Next


	'		Dim work = From address In xelement.Elements(ns + "work").Elements(ns + "phase")
	'							 Select address


	'		For Each phase As XElement In work
	'			Console.WriteLine(phase)
	'			Dim data = New WorkPhaseData

	'			data.DateFrom = GetSafeStringFromXElement(phase.Element(ns + "dateFrom"))
	'			data.DateFromFuzzy = GetSafeStringFromXElement(phase.Element(ns + "dateFromFuzzy"))
	'			data.SubPhase = GetSafeStringFromXElement(phase.Element(ns + "subphase"))
	'			data.Current = GetSafeStringFromXElement(phase.Element(ns + "current"))
	'			data.Comments = GetSafeStringFromXElement(phase.Element(ns + "comments"))
	'			data.PlainText = GetSafeStringFromXElement(phase.Element(ns + "plainText"))
	'			data.Project = GetSafeStringFromXElement(phase.Element(ns + "project"))
	'			data.Duration = GetSafeStringFromXElement(phase.Element(ns + "duration"))

	'			If Not phase.Element(ns + "location") Is Nothing Then
	'				Dim addressCity = GetSafeStringFromXElement(phase.Element(ns + "location").Element(ns + "city"))
	'				Dim addressCountryCode = GetSafeStringFromXElement(phase.Element(ns + "location").Element(ns + "country").Element(ns + "code"))
	'				Dim addressCountryName = GetSafeStringFromXElement(phase.Element(ns + "location").Element(ns + "country").Element(ns + "name"))
	'				Dim addressState = GetSafeStringFromXElement(phase.Element(ns + "location").Element(ns + "state"))
	'				data.Location = New PhaseLocationData With {.City = addressCity,
	'					.Country = New CountryData With {.Code = addressCountryCode, .Name = addressCountryName},
	'					.State = addressState}
	'			End If

	'			If Not phase.Element(ns + "skill") Is Nothing Then
	'				Dim skillCode = GetSafeStringFromXElement(phase.Element(ns + "skill").Element(ns + "code"))
	'				Dim skillName = GetSafeStringFromXElement(phase.Element(ns + "skill").Element(ns + "name"))
	'				Dim skillWeight = GetSafeStringFromXElement(phase.Element(ns + "skill").Element(ns + "weight"))
	'				data.Skill = New PhaseSkillData With {.Code = skillCode, .Name = skillName, .Weight = skillWeight}
	'			End If

	'			If Not phase.Element(ns + "operationArea") Is Nothing Then
	'				Dim skill As New List(Of PhaseOperationAreaData)
	'				For Each XEL2 As XElement In phase.Elements.Where(Function(d) d.Name = ns + "operationArea")
	'					Dim operationData = New PhaseOperationAreaData
	'					Dim operationAreaCode = GetSafeStringFromXElement(phase.Element(ns + "operationArea").Element(ns + "code"))
	'					Dim operationAreaName = GetSafeStringFromXElement(phase.Element(ns + "operationArea").Element(ns + "name"))
	'					Dim operationAreaWeight = GetSafeStringFromXElement(phase.Element(ns + "operationArea").Element(ns + "weight"))

	'					operationData = (New PhaseOperationAreaData With {.Code = operationAreaCode, .Name = operationAreaName, .Weight = operationAreaWeight})
	'					skill.Add(operationData)
	'				Next
	'				data.OperationAreas = skill
	'			End If


	'			If Not phase.Element(ns + "industry") Is Nothing Then
	'				Dim industry As New List(Of PhaseIndustryData)
	'				For Each XEL2 As XElement In phase.Elements.Where(Function(d) d.Name = ns + "industry")
	'					Dim industrydata = New PhaseIndustryData
	'					Dim industryCode = GetSafeStringFromXElement(phase.Element(ns + "industry").Element(ns + "code"))
	'					Dim industryName = GetSafeStringFromXElement(phase.Element(ns + "industry").Element(ns + "name"))
	'					Dim industryWeight = GetSafeStringFromXElement(phase.Element(ns + "industry").Element(ns + "weight"))

	'					industrydata = New PhaseIndustryData With {.Code = industryCode, .Name = industryName, .Weight = industryWeight}
	'					industry.Add(industrydata)
	'				Next
	'				data.Industries = industry
	'			End If


	'			If Not phase.Element(ns + "function") Is Nothing Then
	'				Dim phasefunction As New List(Of PhasefunctionData)
	'				For Each XEL2 As XElement In phase.Elements.Where(Function(d) d.Name = ns + "function")
	'					Dim functiondata = New PhasefunctionData
	'					functiondata = (New PhasefunctionData With {.Code = "1", .Name = XEL2.Value})
	'					phasefunction.Add(functiondata)
	'				Next
	'				data.Functions = phasefunction
	'			End If

	'			workphaseData.Add(data)

	'		Next
	'		works = New WPhaseData With {.WorkPhases = workphaseData}

	'		cvLizerData = New CVLizerXMLData With {.PersonalInformation = personalinfomationData, .Work = works}


	'	Catch ex As Exception
	'		Return False

	'	End Try

	'	Return Not (cvLizerData Is Nothing)

	'End Function


	'Dim employees As IEnumerable(Of XElement) = doc.Root.Elements()
	'For Each itm In employees
	'	Trace.WriteLine(itm)

	'	Dim XMLpersonalInformation_ As IEnumerable(Of XElement) = doc.Descendants(ns + "personalInformation")
	'	For Each XEL1 As XElement In XMLpersonalInformation_
	'		personalinfomationData.FirstName = GetSafeStringFromXElement(XEL1.Element(ns + "firstname"))
	'		personalinfomationData.LastName = GetSafeStringFromXElement(XEL1.Element(ns + "lastname"))

	'		Dim genderCode = GetSafeStringFromXElement(XEL1.Element(ns + "gender").Element(ns + "code"))
	'		Dim genderName = GetSafeStringFromXElement(XEL1.Element(ns + "gender").Element(ns + "name"))
	'		personalinfomationData.Gender = New GenderData With {.Code = genderCode, .CodeName = genderName}

	'		Dim iscedCode = GetSafeStringFromXElement(XEL1.Element(ns + "isced").Element(ns + "code"))
	'		Dim iscedName = GetSafeStringFromXElement(XEL1.Element(ns + "isced").Element(ns + "name"))
	'		personalinfomationData.IsCed = New IsCedData With {.Code = iscedCode, .CodeName = iscedName}

	'		personalinfomationData.DateOfBirth = GetSafeStringFromXElement(XEL1.Element(ns + "birthdate"))
	'		personalinfomationData.DateOfBirthYear = GetSafeStringFromXElement(XEL1.Element(ns + "birthyear"))
	'		personalinfomationData.DateOfBirthPlace = GetSafeStringFromXElement(XEL1.Element(ns + "birthplace"))

	'		Dim nationalityCode = GetSafeStringFromXElement(XEL1.Element(ns + "nationality").Element(ns + "code"))
	'		Dim nationalityName = GetSafeStringFromXElement(XEL1.Element(ns + "nationality").Element(ns + "name"))
	'		personalinfomationData.Nationality = New NationalityData With {.Code = nationalityCode, .CodeName = nationalityName}

	'		Dim civilstateCode = GetSafeStringFromXElement(XEL1.Element(ns + "civilState").Element(ns + "code"))
	'		Dim civilstateName = GetSafeStringFromXElement(XEL1.Element(ns + "civilState").Element(ns + "name"))
	'		personalinfomationData.CivilStatus = New CivilStateData With {.Code = civilstateCode, .CodeName = civilstateName}

	'		Dim addressStreet = GetSafeStringFromXElement(XEL1.Element(ns + "address").Element(ns + "street"))
	'		Dim addressPostcode = GetSafeStringFromXElement(XEL1.Element(ns + "address").Element(ns + "postcode"))
	'		Dim addressCity = GetSafeStringFromXElement(XEL1.Element(ns + "address").Element(ns + "city"))
	'		Dim addressCountryCode = GetSafeStringFromXElement(XEL1.Element(ns + "address").Element(ns + "country").Element(ns + "code"))
	'		Dim addressCountryName = GetSafeStringFromXElement(XEL1.Element(ns + "address").Element(ns + "country").Element(ns + "name"))
	'		Dim addressState = GetSafeStringFromXElement(XEL1.Element(ns + "address").Element(ns + "state"))
	'		personalinfomationData.Address = New AddressData With {.Street = addressStreet,
	'			.Postcode = addressPostcode,
	'			.City = addressCity,
	'			.Country = New CountryData With {.Code = addressCountryCode, .Name = addressCountryName},
	'			.State = addressState}

	'		If Not XEL1.Element(ns + "phoneNumber") Is Nothing Then
	'			Dim phone As New List(Of PhoneNumberData)
	'			For Each XEL2 As XElement In XEL1.Elements.Where(Function(d) d.Name = ns + "phoneNumber")
	'				Dim data = New PhoneNumberData
	'				data = (New PhoneNumberData With {.Code = "1", .PhoneNumber = XEL2.Value})
	'				phone.Add(data)
	'			Next
	'			personalinfomationData.PhoneNumbers = phone
	'		End If

	'	Next

	'Next


	'For Each itm In employees
	'	Dim XMLwork_ As IEnumerable(Of XElement) = doc.Descendants(ns + "work")
	'	For Each phase As XElement In XMLwork_
	'		workData.DateFrom = GetSafeStringFromXElement(phase.Element(ns + "phase").Element(ns + "dateFrom"))
	'		workData.DateFromFuzzy = GetSafeStringFromXElement(phase.Element(ns + "phase").Element(ns + "dateFromFuzzy"))
	'		workData.SubPhase = GetSafeStringFromXElement(phase.Element(ns + "phase").Element(ns + "subphase"))
	'		workData.Current = GetSafeStringFromXElement(phase.Element(ns + "phase").Element(ns + "current"))
	'		workData.Comments = GetSafeStringFromXElement(phase.Element(ns + "phase").Element(ns + "comments"))
	'		workData.PlainText = GetSafeStringFromXElement(phase.Element(ns + "phase").Element(ns + "plainText"))
	'		workData.Project = GetSafeStringFromXElement(phase.Element(ns + "phase").Element(ns + "project"))
	'		workData.Duration = GetSafeStringFromXElement(phase.Element(ns + "phase").Element(ns + "duration"))

	'		If Not phase.Element(ns + "phase").Element(ns + "location") Is Nothing Then
	'			Dim addressCity = GetSafeStringFromXElement(phase.Element(ns + "phase").Element(ns + "location").Element(ns + "city"))
	'			Dim addressCountryCode = GetSafeStringFromXElement(phase.Element(ns + "phase").Element(ns + "location").Element(ns + "country").Element(ns + "code"))
	'			Dim addressCountryName = GetSafeStringFromXElement(phase.Element(ns + "phase").Element(ns + "location").Element(ns + "country").Element(ns + "name"))
	'			Dim addressState = GetSafeStringFromXElement(phase.Element(ns + "phase").Element(ns + "location").Element(ns + "state"))
	'			workData.Location = New PhaseLocationData With {.City = addressCity,
	'			.Country = New CountryData With {.Code = addressCountryCode, .Name = addressCountryName},
	'			.State = addressState}
	'		End If

	'		If Not phase.Element(ns + "phase").Element(ns + "skill") Is Nothing Then
	'			Dim skillCode = GetSafeStringFromXElement(phase.Element(ns + "phase").Element(ns + "skill").Element(ns + "code"))
	'			Dim skillName = GetSafeStringFromXElement(phase.Element(ns + "phase").Element(ns + "skill").Element(ns + "name"))
	'			Dim skillWeight = GetSafeStringFromXElement(phase.Element(ns + "phase").Element(ns + "skill").Element(ns + "weight"))
	'			workData.Skill = New PhaseSkillData With {.Code = skillCode, .Name = skillName, .Weight = skillWeight}
	'		End If

	'		If Not phase.Element(ns + "phase").Element(ns + "operationArea") Is Nothing Then
	'			Dim skill As New List(Of PhaseSkillData)
	'			For Each XEL2 As XElement In phase.Elements.Where(Function(d) d.Name = ns + "operationArea")
	'				Dim data = New PhaseSkillData
	'				Dim operationAreaCode = GetSafeStringFromXElement(phase.Element(ns + "phase").Element(ns + "operationArea").Element(ns + "code"))
	'				Dim operationAreaName = GetSafeStringFromXElement(phase.Element(ns + "phase").Element(ns + "operationArea").Element(ns + "name"))
	'				Dim operationAreaWeight = GetSafeStringFromXElement(phase.Element(ns + "phase").Element(ns + "operationArea").Element(ns + "weight"))

	'				data = (New PhaseSkillData With {.Code = operationAreaCode, .Name = operationAreaName, .Weight = operationAreaWeight})
	'				skill.Add(data)
	'			Next
	'			workData.OperationAreas = skill
	'		End If


	'		If Not phase.Element(ns + "phase").Element(ns + "industry") Is Nothing Then
	'			Dim industry As New List(Of PhaseIndustryData)
	'			For Each XEL2 As XElement In phase.Elements.Where(Function(d) d.Name = ns + "industry")
	'				Dim data = New PhaseIndustryData
	'				Dim industryCode = GetSafeStringFromXElement(phase.Element(ns + "phase").Element(ns + "industry").Element(ns + "code"))
	'				Dim industryName = GetSafeStringFromXElement(phase.Element(ns + "phase").Element(ns + "industry").Element(ns + "name"))
	'				Dim industryWeight = GetSafeStringFromXElement(phase.Element(ns + "phase").Element(ns + "industry").Element(ns + "weight"))

	'				data = New PhaseIndustryData With {.Code = industryCode, .Name = industryName, .Weight = industryWeight}
	'				industry.Add(data)
	'			Next
	'			workData.Industries = industry
	'		End If


	'		If Not phase.Element(ns + "phase").Element(ns + "function") Is Nothing Then
	'			Dim phasefunction As New List(Of PhasefunctionData)
	'			For Each XEL2 As XElement In phase.Elements.Where(Function(d) d.Name = ns + "function")
	'				Dim data = New PhasefunctionData
	'				data = (New PhasefunctionData With {.Code = "1", .Name = XEL2.Value})
	'				phasefunction.Add(data)
	'			Next
	'			workData.Functions = phasefunction
	'		End If

	'	Next



	'Next


	'Dim XMLpersonalInformation As IEnumerable(Of XElement) = doc.Root.Elements("personalInformation")
	'For Each XEL1 As XElement In XMLpersonalInformation
	'	Dim material As New CVLizerXMLData
	'	material.FirstName = GetSafeStringFromXElement(XEL1.Element("firstname"))
	'	material.LastName = GetSafeStringFromXElement(XEL1.Element("lastname"))

	'	Dim genderCode = GetSafeStringFromXElement(XEL1.Element("gender").Element("code"))
	'	Dim genderName = GetSafeStringFromXElement(XEL1.Element("gender").Element("name"))
	'	m_CzLizerXMLData.Gender = New GenderData With {.Code = genderCode, .CodeName = genderName}

	'	Dim iscedCode = GetSafeStringFromXElement(XEL1.Element("isced").Element("code"))
	'	Dim iscedName = GetSafeStringFromXElement(XEL1.Element("isced").Element("name"))
	'	m_CzLizerXMLData.IsCed = New IsCedData With {.Code = iscedCode, .CodeName = iscedName}

	'	m_CzLizerXMLData.DateOfBirth = GetSafeStringFromXElement(XEL1.Element("birthdate"))
	'	m_CzLizerXMLData.DateOfBirthYear = GetSafeStringFromXElement(XEL1.Element("birthyear"))
	'	m_CzLizerXMLData.DateOfBirthPlace = GetSafeStringFromXElement(XEL1.Element("birthplace"))

	'	Dim nationalityCode = GetSafeStringFromXElement(XEL1.Element("nationality").Element("code"))
	'	Dim nationalityName = GetSafeStringFromXElement(XEL1.Element("nationality").Element("name"))
	'	m_CzLizerXMLData.Nationality = New NationalityData With {.Code = nationalityCode, .CodeName = nationalityName}

	'	Dim civilstateCode = GetSafeStringFromXElement(XEL1.Element("civilState").Element("code"))
	'	Dim civilstateName = GetSafeStringFromXElement(XEL1.Element("civilState").Element("name"))
	'	m_CzLizerXMLData.CivilStatus = New CivilStateData With {.Code = civilstateCode, .CodeName = civilstateName}

	'	Dim addressStreet = GetSafeStringFromXElement(XEL1.Element("address").Element("street"))
	'	Dim addressPostcode = GetSafeStringFromXElement(XEL1.Element("address").Element("postcode"))
	'	Dim addressCity = GetSafeStringFromXElement(XEL1.Element("address").Element("city"))
	'	Dim addressCountryCode = GetSafeStringFromXElement(XEL1.Element("address").Element("country").Element("code"))
	'	Dim addressCountryName = GetSafeStringFromXElement(XEL1.Element("address").Element("country").Element("name"))
	'	Dim addressState = GetSafeStringFromXElement(XEL1.Element("address").Element("state"))
	'	m_CzLizerXMLData.Address = New AddressData With {.Street = addressStreet,
	'		.Postcode = addressPostcode,
	'		.City = addressCity,
	'		.Country = New CountryData With {.Code = addressCountryCode, .Name = addressCountryName},
	'		.State = addressState}

	'	For Each XEL2 As XElement In XEL1.Elements.Where(Function(d) d.Name = "phoneNumber")
	'		m_CzLizerXMLData.PhoneNumbers.Add(New PhoneNumberData With {.Code = "1", .PhoneNumber = XEL2.Value})
	'	Next


	'	For Each XEL2 As XElement In XEL1.Element("work").Elements.Where(Function(d) d.Name = "phase")
	'		If XEL2.Attribute("property").Value = "Mass Density (RHO)_1" Then
	'			material.Density = XEL2.Value
	'		ElseIf XEL2.Attribute("property").Value = "Spec Organization" Then
	'			material.Org = XEL2.Value
	'		ElseIf XEL2.Attribute("property").Value = "Spec Name" Then
	'			material.Spec = XEL2.Value
	'		ElseIf XEL2.Attribute("property").Value = "Spec Grade" Then
	'			material.Grade = XEL2.Value
	'		End If
	'	Next
	'	MaterialsList.Add(material)
	'	If Not CatagoryNames.Contains(material.Category) Then CatagoryNames.Add(material.Category)
	'	If Not Organizations.Contains(material.Org) Then Organizations.Add(material.Org)





	'match = Regex.Match(xmlContent, DATAMATRIX_VALUE_PATTERN_FIRSTNAME, RegexOptions.IgnoreCase)
	'm_CzLizerXMLData.FirstName = match.Groups(1).Value
	'match = Regex.Match(xmlContent, DATAMATRIX_VALUE_PATTERN_LASTNAME, RegexOptions.IgnoreCase)
	'm_CzLizerXMLData.LastName = match.Groups(1).Value
	'match = Regex.Match(xmlContent, DATAMATRIX_VALUE_PATTERN_DATEOFBIRTH, RegexOptions.IgnoreCase)
	'm_CzLizerXMLData.DateOfBirth = match.Groups(1).Value
	'match = Regex.Match(xmlContent, DATAMATRIX_VALUE_PATTERN_DATEOFBIRTH_YEAR, RegexOptions.IgnoreCase)
	'm_CzLizerXMLData.DateOfBirthYear = match.Groups(1).Value

	'match = Regex.Match(xmlContent, DATAMATRIX_VALUE_PATTERN_EMAIL, RegexOptions.IgnoreCase)
	'm_CzLizerXMLData.Email = match.Groups(1).Value

	'match = Regex.Match(xmlContent, "(?s)<gender>.*?</gender>", RegexOptions.IgnoreCase)
	'If match.Success Then
	'	Dim value = match.Value
	'	match = Regex.Match(value, "<code>(?<code>.*?)</code>", RegexOptions.IgnoreCase)
	'	Dim genderCode = match.Groups("code").Value

	'	match = Regex.Match(value, "<name>(?<name>.*?)</name>", RegexOptions.IgnoreCase)
	'	Dim genderName = match.Groups("name").Value

	'	m_CzLizerXMLData.Gender.Add(New GenderData With {.Code = genderCode, .CodeName = genderName})

	'End If

	'match = Regex.Match(xmlContent, DATAMATRIX_VALUE_PATTERN_PHONENUMBER, RegexOptions.IgnoreCase)
	'For Each itm In match.Groups
	'	m_CzLizerXMLData.PhoneNumbers.Add(New PhoneNumber With {.Code = "0", .PhoneNumber = match.Groups("phoneNumber").Value})
	'Next

	'match = Regex.Match(xmlContent, DATAMATRIX_VALUE_PATTERN_DOCUMENT, RegexOptions.IgnoreCase)
	'For Each itm In match.Groups
	'	match = Regex.Match(xmlContent, DATAMATRIX_VALUE_PATTERN_DOCUMENT_CLASS, RegexOptions.IgnoreCase)
	'	For Each itmDoc In match.Groups
	'		m_CzLizerXMLData.BinaryDocuments.Add(New BinaryDocument With {.DocClass = itmDoc})
	'	Next
	'Next


	Private Sub LoadAplicantImageFromCVLizerXML()

		Dim val As String = m_Trxmldata.ProfilePicture
		If String.IsNullOrWhiteSpace(val) Then Return
		Dim bytes() As Byte = Convert.FromBase64String(val)
		Dim mem As MemoryStream = New MemoryStream(bytes)
		Dim bmp2 As Bitmap = New Bitmap(mem)

		peProfilePicture.Image = bmp2

	End Sub



#End Region


#Region "Helpers"

	Private Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

		Dim m_md As New SPProgUtility.Mandanten.Mandant
		Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
		Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
		Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

		Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
		Dim translate = clsTransalation.GetTranslationInObject

		Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

	End Function

	Sub ReadHeader(ByVal url As String)
		Dim loop1, loop2 As Integer
		Dim arr1(), arr2() As String
		Dim coll As NameValueCollection

		If String.IsNullOrWhiteSpace(url) Then Return
		' Creates an HttpWebRequest with the specified URL. 
		Dim myHttpWebRequest As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
		' Sends the HttpWebRequest and waits for a response.
		Dim myHttpWebResponse As HttpWebResponse = CType(myHttpWebRequest.GetResponse(), HttpWebResponse)
		' Displays all the Headers present in the response received from the URI.
		Console.WriteLine(ControlChars.Lf + ControlChars.Cr + "The following headers were received in the response")
		'The Headers property is a WebHeaderCollection. Use it's properties to traverse the collection and display each header.
		Dim i As Integer
		Trace.WriteLine(myHttpWebResponse.Headers)
		While i < myHttpWebResponse.Headers.Count
			Console.WriteLine(ControlChars.Cr + "Header Name:{0}, Value :{1}", myHttpWebResponse.Headers.Keys(i), myHttpWebResponse.Headers(i))
			Dim msg = String.Format("{0} | {1}", myHttpWebResponse.Headers.Keys(i), myHttpWebResponse.Headers(i))
			Trace.WriteLine(msg)
			i = i + 1
		End While
		myHttpWebResponse.Close()

	End Sub


	Private Function imageToByteArray(imageIn As System.Drawing.Image) As Byte()

		Dim ms As MemoryStream = New MemoryStream()
		imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp)

		Return ms.ToArray()

	End Function

	''' <summary>
	''' Replaces a missing object with another object.
	''' </summary>
	Private Function ReplaceMissing(ByVal obj As Object, ByVal replacementObject As Object) As Object

		If (obj Is Nothing) Then
			Return replacementObject
		Else
			Return obj
		End If

	End Function


	Private Function GetSafeStringFromXElement(ByVal xelment As XElement) As String

		If xelment Is Nothing Then
			Return String.Empty
		Else

			Return xelment.Value
		End If

	End Function

	Private Function FromBase64(ByVal sText As String) As String
		' Base64-String zunächst in ByteArray konvertieren
		Dim nBytes() As Byte = System.Convert.FromBase64String(sText)

		' ByteArray in String umwandeln
		Return System.Text.Encoding.Default.GetString(nBytes)
	End Function


	Private Class FileDirectoryData
		Public Property Filename As String
		Public Property FileDate As DateTime?

	End Class


#End Region

End Class


