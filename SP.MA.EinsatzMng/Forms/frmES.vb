
Imports SP.TodoMng

Imports SP.DatabaseAccess.Customer
Imports System.Reflection.Assembly
Imports DevExpress.LookAndFeel
Imports System.Windows.Forms
Imports System.Threading
Imports System.IO
Imports DevExpress.XtraNavBar
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Settings
Imports SP.DatabaseAccess.ES
Imports SP.Infrastructure.Logging
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath
Imports SPS.Listing.Print.Utility.ESTemplate

Imports SP.DatabaseAccess.ES.DataObjects.ESMng

Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SP.MA.EinsatzMng.Settings
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SP.DatabaseAccess.Employee
Imports SPS.Listing.Print.Utility
Imports SPProgUtility.CommonXmlUtility
Imports SP.Infrastructure.DateAndTimeCalculation
Imports SPRPListSearch
Imports SPGAV.TempData

Namespace UI

	''' <summary>
	''' Employee management.
	''' </summary>
	Public Class frmES

#Region "Constants"

		Private Const DEFAULT_SPUTNIK_UPDATE_UTILITIES_WEBSERVICE_URI = "http://asmx.domain.com/wsSPS_services/SPUpdateUtilities.asmx"
		Private Const MANDANT_XML_SETTING_SPUTNIK_UTILITIES_WEBSERVICE_WEBSERVICE_URI As String = "MD_{0}/Interfaces/webservices/webserviceupdateinfoservices"

#End Region

#Region "Private Fields"

		''' <summary>
		''' The Initialization data.
		''' </summary>
		Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

		''' <summary>
		''' The translation value helper.
		''' </summary>
		Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

		''' <summary>
		''' The es data access object.
		''' </summary>
		Private m_ESDatabaseAccess As IESDatabaseAccess

		''' <summary>
		''' The employee data access object.
		''' </summary>
		Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

		''' <summary>
		''' The customer data access object.
		''' </summary>
		Private m_CustomerDatabaseAccess As ICustomerDatabaseAccess

		''' <summary>
		''' The settings manager.
		''' </summary>
		Private m_SettingsManager As ISettingsManager

		''' <summary>
		''' Contains the employee number of the loaded employee data.
		''' </summary>
		Private m_ESNumber As Integer?

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI

		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As Utility

		''' <summary>
		''' Date and time utitlity.
		''' </summary>
		Private m_DateAndTimeUtility As New DateAndTimeUtily

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		''' <summary>
		''' Boolean flag indicating if initial data has been loaded.
		''' </summary>
		Private m_IsInitialDataLoaded As Boolean = False

		''' <summary>
		''' List of user controls.
		''' </summary>
		Private m_ListOfUserControls As New List(Of ucBaseControl)

		''' <summary>
		''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
		''' </summary>
		Private m_SuppressUIEvents As Boolean = False

		''' <summary>
		''' The SPProgUtility object.
		''' </summary>
		Protected m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath

		Private m_md As Mandant
		Private m_path As ClsProgPath


		Private m_SaveButton As NavBarItem
		Private m_IsDataValid As Boolean = True

		''' <summary>
		''' Boolan flag indicating if the form has been initialized.
		''' </summary>
		Private m_IsInitialized = False

		''' <summary>
		''' Communication support between controls.
		''' </summary>
		Protected m_UCMediator As UserControlFormMediator

		''' <summary>
		''' WOS NavBar Item.
		''' </summary>
		Private m_Wos_ESVertrag_Data As NavBarItem
		Private m_Wos_VerleihVertrag_Data As NavBarItem

		''' <summary>
		''' Settings xml.
		''' </summary>
		Private m_MandantSettingsXml As SettingsXml

		''' <summary>
		''' SPUpdateUtilities service url.
		''' </summary>
		Private m_SPUpdateUtilitiesServiceUrl As String

		''' <summary>
		''' The GAV update info of last check.
		''' </summary>
		Private m_GavUpdateInfoOfLastCheck As SP.Internal.Automations.GAVVersionData
		Private m_tempDataMergedNews As PublicationNewsViewData

		''' <summary>
		''' Error icon.
		''' </summary>
		Private m_ErrorIcon As Image

		Private m_PropertyForm As frmFoundedReports

		Private m_AllowedESVerDesign As Boolean
		Private m_AllowedVerleihVerDesign As Boolean

		Private m_AllowedTempDataPVL As Boolean

#End Region

#Region "Constructor"

		''' <summary>
		''' The constructor.
		''' </summary>
		''' <param name="_setting">The settings.</param>
		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
			Try

				m_ErrorIcon = My.Resources.error_small

				' Mandantendaten
				m_md = New Mandant
				m_path = New ClsProgPath
				m_AllowedTempDataPVL = True

				m_InitializationData = _setting
				m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

				m_MandantSettingsXml = New SettingsXml(m_md.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))
				m_SPUpdateUtilitiesServiceUrl = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_UTILITIES_WEBSERVICE_WEBSERVICE_URI, m_InitializationData.MDData.MDNr))

				If String.IsNullOrWhiteSpace(m_SPUpdateUtilitiesServiceUrl) Then
					m_SPUpdateUtilitiesServiceUrl = DEFAULT_SPUTNIK_UPDATE_UTILITIES_WEBSERVICE_URI
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

			' Dieser Aufruf ist für den Designer erforderlich.
			DevExpress.UserSkins.BonusSkins.Register()
			DevExpress.Skins.SkinManager.EnableFormSkins()

			m_SuppressUIEvents = True
			InitializeComponent()
			m_SuppressUIEvents = False

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

			' Top tabs
			sccMain.Dock = DockStyle.Fill

			m_ListOfUserControls.Add(UcCandidateAndCustomer1)
			m_ListOfUserControls.Add(UcESData1)
			m_ListOfUserControls.Add(UcSalaryData1)
			m_ListOfUserControls.Add(UcAdditionalInfoFields1)
			m_ListOfUserControls.Add(UcAdditionalSalaryTypes1)
			m_ListOfUserControls.Add(UcKostenteilung1)

			' Init sub controls with configuration information
			For Each ctrl In m_ListOfUserControls
				ctrl.InitWithConfigurationData(m_InitializationData, m_Translate)
			Next

			m_UCMediator = New UserControlFormMediator(Me,
													UcCandidateAndCustomer1,
													UcESData1,
													UcSalaryData1,
													UcAdditionalInfoFields1,
													UcAdditionalSalaryTypes1,
													UcKostenteilung1)

			Dim connectionString As String = m_InitializationData.MDData.MDDbConn
			m_ESDatabaseAccess = New DatabaseAccess.ES.ESDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
			m_EmployeeDatabaseAccess = New DatabaseAccess.Employee.EmployeeDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
			m_CustomerDatabaseAccess = New DatabaseAccess.Customer.CustomerDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)

			m_SettingsManager = New SettingsManager
			m_UtilityUI = New UtilityUI
			m_Utility = New Utility

			m_AllowedESVerDesign = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 254, m_InitializationData.MDData.MDNr)
			m_AllowedVerleihVerDesign = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 256, m_InitializationData.MDData.MDNr)

			' Translate controls
			TranslateControls()

			' Creates the navigation bar.
			CreateMyNavBar()

		End Sub

#End Region

#Region "Public Properties"

		''' <summary>
		''' Boolean flag indicating if ES data is loaded.
		''' </summary>
		Public ReadOnly Property IsESDataLoaded As Boolean
			Get
				Return m_ESNumber.HasValue
			End Get

		End Property

		''' <summary>
		''' Get the user control mediator.
		''' </summary>
		Public ReadOnly Property UCMediator As UserControlFormMediator
			Get
				Return m_UCMediator
			End Get
		End Property

		''' <summary>
		''' Gets or sets data valid flag.
		''' </summary>
		''' <returns>Data valid flag</returns>
		Public Property IsDataValid As Boolean
			Get
				Return m_IsDataValid
			End Get
			Set(value As Boolean)

				m_IsDataValid = value

				If Not m_IsDataValid AndAlso Not m_SaveButton Is Nothing Then
					m_SaveButton.Enabled = False
				End If

			End Set
		End Property

		''' <summary>
		''' Gets the initialisation data.
		''' </summary>
		''' <returns>The initialisation data.</returns>
		Public ReadOnly Property InitializationData As SP.Infrastructure.Initialization.InitializeClass
			Get
				Return m_InitializationData
			End Get
		End Property

#End Region

#Region "Public Methods"

		''' <summary>
		''' Show the data of an ES.
		''' </summary>
		''' <param name="esNumber">The ES number.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function LoadESData(ByVal esNumber As Integer) As Boolean

			If Not m_SaveButton Is Nothing Then
				m_SaveButton.Enabled = True
			End If

			If Not m_IsInitialized Then
				Reset()
				m_IsInitialized = True
			End If

			CleanUp()

			m_SuppressUIEvents = True

			Dim success As Boolean = True

			success = success AndAlso LoadUserControlData(esNumber)

			m_ESNumber = IIf(success, esNumber, Nothing)

			IsDataValid = success

			If IsDataValid Then
				CheckGAVValidityOfSelectedESSalaryData()
			End If

			m_SuppressUIEvents = False

			Return success
		End Function

		''' <summary>
		''' Checks the GAV validity of the selected ES salary data.
		''' </summary>
		Public Sub CheckGAVValidityOfSelectedESSalaryData()

			Dim selectedSalaryData = m_UCMediator.SelectedESSalaryOnSalaryDataTab
			If selectedSalaryData Is Nothing Then Return

			CheckGAVValidityOfESSalaryData(selectedSalaryData)

		End Sub

		''' <summary>
		''' Checks GAV validity of ES salary data.
		''' </summary>
		''' <param name="esLohn">The ES Salary data.</param>
		Public Sub CheckGAVValidityOfESSalaryData(ByVal esLohn As ESSalaryData)

			bsiBtnGavInfo.Glyph = Nothing
			bsiBtnGavInfo.Caption = String.Empty

			If m_AllowedTempDataPVL Then
				CheckGAVValidityData_API(esLohn)
			Else
				CheckGAVValidityData_XML(esLohn)
			End If

			'Dim esEnde = If(m_UCMediator.SelectedESEnde.HasValue, m_UCMediator.SelectedESEnde.Value.Date, New DateTime(3999, 12, 31))
			'Dim esLohnBis = If(esLohn.LOBis.HasValue, esLohn.LOBis.Value.Date, New DateTime(3999, 12, 31))
			'Dim effectiveESLohnBis = m_DateAndTimeUtility.MinDate(esEnde, esLohnBis)

			'If effectiveESLohnBis < DateTime.Now.Date Then
			'	' Do not check ESLohn data that are not valid any more.
			'	Return
			'End If

			'Dim errorMsg = m_Translate.GetSafeTranslationValue("GAV Version konnte nicht geprüft werden.")
			'Dim gavVersionChecked As String = m_Translate.GetSafeTranslationValue("GAV Version wurde überprüft.")
			'Dim gavDataHasChanged As String = m_Translate.GetSafeTranslationValue("GAV Daten haben sich geändert.")

			'Try
			'	If esLohn.GAVNr > 10000 AndAlso esLohn.GAVNr <> 99999 Then
			'		m_GavUpdateInfoOfLastCheck = New SP.Internal.Automations.GAVVersionData
			'		Dim obj As New SP.Internal.Automations.PVLSearchData(m_InitializationData)
			'		m_GavUpdateInfoOfLastCheck = obj.LoadGAVUpdateNotificationData(esLohn.GAVNr)

			'		If (m_GavUpdateInfoOfLastCheck Is Nothing) Then
			'			m_UtilityUI.ShowErrorDialog(errorMsg)

			'			Return
			'		End If

			'		If m_GavUpdateInfoOfLastCheck.GAVDate.HasValue Then

			'			Dim esLohnVonDate As DateTime = esLohn.CreatedOn
			'			Dim esLohnBisDate As DateTime = If(esLohn.LOBis.HasValue, esLohn.LOBis.Value.Date, New DateTime(3999, 12, 31))
			'			Dim gavUpdateDate As DateTime = m_GavUpdateInfoOfLastCheck.GAVDate.Value

			'			' Check if the GAVNr has between updated between ESLohnVon and ESLohnBis.
			'			If gavUpdateDate >= esLohnVonDate AndAlso gavUpdateDate <= esLohnBisDate Then

			'				bsiBtnGavInfo.Glyph = m_ErrorIcon
			'				bsiBtnGavInfo.Caption = gavDataHasChanged

			'			Else
			'				bsiBtnGavInfo.Caption = gavVersionChecked
			'			End If

			'		Else
			'			bsiBtnGavInfo.Caption = gavVersionChecked
			'		End If
			'	Else
			'		bsiBtnGavInfo.Caption = errorMsg

			'	End If

			'Catch ex As Exception
			'	m_Logger.LogError(String.Format("Service-URL: {0} | GAVNumber: {1} | Error: {2}", m_SPUpdateUtilitiesServiceUrl, esLohn.GAVNr, ex.ToString))
			'	m_UtilityUI.ShowErrorDialog(errorMsg)

			'End Try

		End Sub

		Private Sub CheckGAVValidityData_XML(ByVal eslohn As ESSalaryData)

			Dim esEnde = If(m_UCMediator.SelectedESEnde.HasValue, m_UCMediator.SelectedESEnde.Value.Date, New DateTime(3999, 12, 31))
			Dim esLohnBis = If(eslohn.LOBis.HasValue, eslohn.LOBis.Value.Date, New DateTime(3999, 12, 31))
			Dim effectiveESLohnBis = m_DateAndTimeUtility.MinDate(esEnde, esLohnBis)

			If effectiveESLohnBis < DateTime.Now.Date Then
				' Do not check ESLohn data that are not valid any more.
				Return
			End If

			Dim errorMsg = m_Translate.GetSafeTranslationValue("GAV Version konnte nicht geprüft werden.")
			Dim gavVersionChecked As String = m_Translate.GetSafeTranslationValue("GAV Version wurde überprüft.")
			Dim gavDataHasChanged As String = m_Translate.GetSafeTranslationValue("GAV Daten haben sich geändert.")

			Try
				If eslohn.GAVNr > 10000 AndAlso eslohn.GAVNr <> 99999 Then
					m_GavUpdateInfoOfLastCheck = New SP.Internal.Automations.GAVVersionData
					Dim obj As New SP.Internal.Automations.PVLSearchData(m_InitializationData)
					m_GavUpdateInfoOfLastCheck = obj.LoadGAVUpdateNotificationData(eslohn.GAVNr)

					If (m_GavUpdateInfoOfLastCheck Is Nothing) Then
						m_UtilityUI.ShowErrorDialog(errorMsg)

						Return
					End If

					If m_GavUpdateInfoOfLastCheck.GAVDate.HasValue Then

						Dim esLohnVonDate As DateTime = eslohn.CreatedOn
						Dim esLohnBisDate As DateTime = If(eslohn.LOBis.HasValue, eslohn.LOBis.Value.Date, New DateTime(3999, 12, 31))
						Dim gavUpdateDate As DateTime = m_GavUpdateInfoOfLastCheck.GAVDate.Value

						' Check if the GAVNr has between updated between ESLohnVon and ESLohnBis.
						If gavUpdateDate >= esLohnVonDate AndAlso gavUpdateDate <= esLohnBisDate Then

							bsiBtnGavInfo.Glyph = m_ErrorIcon
							bsiBtnGavInfo.Caption = gavDataHasChanged

						Else
							bsiBtnGavInfo.Caption = gavVersionChecked
						End If

					Else
						bsiBtnGavInfo.Caption = gavVersionChecked
					End If
				Else
					bsiBtnGavInfo.Caption = errorMsg

				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("Service-URL: {0} | GAVNumber: {1} | Error: {2}", m_SPUpdateUtilitiesServiceUrl, eslohn.GAVNr, ex.ToString))
				m_UtilityUI.ShowErrorDialog(errorMsg)

			End Try

		End Sub

		Private Sub CheckGAVValidityData_API(ByVal esLohn As ESSalaryData)

			Dim esEnde = If(m_UCMediator.SelectedESEnde.HasValue, m_UCMediator.SelectedESEnde.Value.Date, New DateTime(3999, 12, 31))
			Dim esLohnBis = If(esLohn.LOBis.HasValue, esLohn.LOBis.Value.Date, New DateTime(3999, 12, 31))
			Dim effectiveESLohnBis = m_DateAndTimeUtility.MinDate(esEnde, esLohnBis)

			If effectiveESLohnBis < DateTime.Now.Date Then
				' Do not check ESLohn data that are not valid any more.
				'Return
			End If

			Dim errorMsg = m_Translate.GetSafeTranslationValue("GAV Version konnte nicht geprüft werden.")
			Dim gavVersionChecked As String = m_Translate.GetSafeTranslationValue("GAV Version wurde überprüft.")
			Dim gavDataHasChanged As String = m_Translate.GetSafeTranslationValue("GAV Daten haben sich geändert.")

			Try
				If esLohn.GAVNr > 10000 AndAlso esLohn.GAVNr <> 99999 Then
					Dim esLohnBisDate As DateTime = If(esLohn.LOBis.HasValue, esLohn.LOBis.Value.Date, esEnde)

					Dim newsObj = New SPGAV.UI.frmPublicationNews(m_InitializationData)
					Dim newsData = newsObj.LoadMergedNewsForAssignedConctractData(esLohn.GAVNr)

					If (newsData Is Nothing) Then
						m_UtilityUI.ShowErrorDialog(errorMsg)

						Return
					End If

					m_tempDataMergedNews = newsData.Where(Function(x) x.ContractNumber = esLohn.GAVNr And x.PublicationDate >= esLohn.LOVon And x.PublicationDate <= esLohnBisDate And x.PublicationDate <= esEnde).FirstOrDefault

					If m_tempDataMergedNews Is Nothing Then
						bsiBtnGavInfo.Caption = gavVersionChecked
						bsiBtnGavInfo.Glyph = My.Resources.apply_16x16

						Return
					End If

					Dim gavUpdateDate As DateTime = m_tempDataMergedNews.PublicationDate

					' Check if the GAVNr has between updated between ESLohnVon and ESLohnBis.
					If gavUpdateDate >= esLohn.LOVon AndAlso gavUpdateDate <= esLohnBisDate Then

						bsiBtnGavInfo.Glyph = m_ErrorIcon
						bsiBtnGavInfo.Caption = gavDataHasChanged

					Else
						bsiBtnGavInfo.Caption = gavVersionChecked
						bsiBtnGavInfo.Glyph = My.Resources.apply_16x16

					End If

				Else
					bsiBtnGavInfo.Caption = errorMsg

				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("Service-URL: {0} | GAVNumber: {1} | Error: {2}", m_SPUpdateUtilitiesServiceUrl, esLohn.GAVNr, ex.ToString))
				m_UtilityUI.ShowErrorDialog(errorMsg)

			End Try

		End Sub


#End Region

#Region "Private Methods"

		''' <summary>
		'''  Trannslate controls.
		''' </summary>
		Private Sub TranslateControls()

			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

			Me.xtabLohndaten.Text = m_Translate.GetSafeTranslationValue(Me.xtabLohndaten.Text)
			Me.xtabAdditionalInfoFelder.Text = m_Translate.GetSafeTranslationValue(Me.xtabAdditionalInfoFelder.Text)
			Me.xtabAdditionalSalaryTypes.Text = m_Translate.GetSafeTranslationValue(Me.xtabAdditionalSalaryTypes.Text)

			bsiLblErstellt.Caption = m_Translate.GetSafeTranslationValue(bsiLblErstellt.Caption)
			bsiLblGeaendert.Caption = m_Translate.GetSafeTranslationValue(bsiLblGeaendert.Caption)

		End Sub

		''' <summary>
		''' Resets the from.
		''' </summary>
		Private Sub Reset()
			Dim supressState = m_SuppressUIEvents
			m_SuppressUIEvents = True

			' Reset all the child controls
			For Each ctrl In m_ListOfUserControls
				ctrl.Reset()
			Next

			' Reset GAV Info check information
			bsiBtnGavInfo.Caption = String.Empty
			bsiBtnGavInfo.Glyph = Nothing

			' Bottom page
			m_SuppressUIEvents = False

			m_SuppressUIEvents = supressState
		End Sub

		''' <summary>
		''' CleanUp the form
		''' </summary>
		Private Sub CleanUp()

			' Cleanup all the child controls
			For Each ctrl In m_ListOfUserControls
				ctrl.CleanUp()
			Next

		End Sub

		''' <summary>
		''' Loads the user control data.
		''' </summary>
		''' <param name="esNumber">The es number.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadUserControlData(ByVal esNumber As Integer) As Boolean

			Dim esMasterData As ESMasterData = m_ESDatabaseAccess.LoadESMasterData(esNumber)

			If esMasterData Is Nothing Then
				m_UtilityUI.ShowErrorDialog("Einsatzdaten konnten nicht geladen werden")
				Return (False)
			End If

			Dim success As Boolean = True

			For Each ctrl As ucBaseControl In m_ListOfUserControls
				success = success AndAlso ctrl.LoadData(esMasterData)
			Next

			success = success AndAlso PrepareStatusAndNavigationBar(esMasterData)

			Return success

		End Function

		''' <summary>
		''' Saves ES data.
		''' </summary>
		Public Sub SaveESData()

			If (IsESDataLoaded AndAlso ValidateData()) Then

				' 1. Load the ES master data
				Dim esMasterData = m_ESDatabaseAccess.LoadESMasterData(m_ESNumber)
				If esMasterData Is Nothing Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))
					Return
				End If

				' 2. Ask all tabs to merge its data with the records just loaded.
				For Each ctrl In m_ListOfUserControls
					ctrl.MergeESMasterData(esMasterData)
				Next

				' 3. Update the data in the database
				esMasterData.ChangedOn = DateTime.Now
				esMasterData.ChangedFrom = m_InitializationData.UserData.UserFullName

				Dim success = m_ESDatabaseAccess.UpdateESMasterData(esMasterData)
				If (success) Then
					Dim reportFinishFlagUpdate = New ReportMng.ReportFinishedFlagUpdater(m_InitializationData, m_Translate)
					Dim successUpdateFinishedFlagOfReport = reportFinishFlagUpdate.UpdateFinishedFlagOfAllReportsOfES(esMasterData.ESNR)

					If Not successUpdateFinishedFlagOfReport Then
						m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Aktualisierung des Abgeschlossen-Status der abhängigen Rapport ist fehlgeschlagen."))
					End If
				End If

				Dim message As String = String.Empty
				If (success) Then

					m_UCMediator.RefreshDataAfterESSave(esMasterData)

					bsiChanged.Caption = String.Format(" {0:f}, {1}", esMasterData.ChangedOn, m_Utility.SafeTrim(esMasterData.ChangedFrom))
					message = m_Translate.GetSafeTranslationValue("Die Daten wurden gespeichert.")
					m_UtilityUI.ShowInfoDialog(message, m_Translate.GetSafeTranslationValue("Daten speichern"), MessageBoxIcon.Information)

					' Send a notification for changed ES data.
					Dim hub = MessageService.Instance.Hub
					Dim esDataChangedNotification As New ESDataHasChanged(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, m_ESNumber)
					hub.Publish(esDataChangedNotification)

				Else
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Daten konnten nicht gespeichert werden."))
				End If

			End If
		End Sub

		''' <summary>
		''' Validates the data on the tabs.
		''' </summary>
		Private Function ValidateData() As Boolean

			Dim valid As Boolean = True
			valid = valid AndAlso m_InitializationData.MDData.ClosedMD = 0
			For Each ctrl In m_ListOfUserControls

				' Only validate tabs with the correct employee number.
				valid = valid AndAlso ctrl.ValidateData()

			Next

			Return valid

		End Function

		''' <summary>
		''' Handles form load event.
		''' </summary>
		Private Sub OnFrmES_Load(sender As Object, e As System.EventArgs) Handles Me.Load

			Me.KeyPreview = True
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_md.GetDefaultUSNr, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

		End Sub

		''' <summary>
		''' Loads form settings if form gets visible.
		''' </summary>
		Private Sub OnFrmES_VisibleChanged(sender As System.Object, e As System.EventArgs) Handles MyBase.VisibleChanged

			If Visible Then
				LoadFormSettings()
			End If

		End Sub

		''' <summary>
		''' Handles form closing event.
		''' </summary>
		Private Sub OnFrmES_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

			CleanupAndHideForm()

			e.Cancel = True

		End Sub

		''' <summary>
		''' Loads form settings.
		''' </summary>
		Private Sub LoadFormSettings()

			Try
				Dim setting_form_height = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_HEIGHT)
				Dim setting_form_width = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_WIDTH)
				Dim setting_form_location = m_SettingsManager.ReadString(SettingKeys.SETTING_FORM_LOCATION)

				If setting_form_height > 0 Then Me.Height = Math.Max(Me.Height, setting_form_height)
				If setting_form_width > 0 Then Me.Width = Math.Max(Me.Width, setting_form_width)

				If Not String.IsNullOrEmpty(setting_form_location) Then
					Dim aLoc As String() = setting_form_location.Split(CChar(";"))
					If Screen.AllScreens.Length = 1 Then
						If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
					End If
					Me.Location = New System.Drawing.Point(Val(aLoc(0)), Val(aLoc(1)))
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())

			End Try

		End Sub

		''' <summary>
		''' Saves the form settings.
		''' </summary>
		Private Sub SaveFromSettings()

			' Save form location, width and height in setttings
			Try
				If Not Me.WindowState = FormWindowState.Minimized Then
					m_SettingsManager.WriteString(SettingKeys.SETTING_FORM_LOCATION, String.Format("{0};{1}", Me.Location.X, Me.Location.Y))
					m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_WIDTH, Me.Width)
					m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_HEIGHT, Me.Height)

					m_SettingsManager.SaveSettings()
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())

			End Try

		End Sub

		''' <summary>
		''' Creates Navigationbar
		''' </summary>
		Private Sub CreateMyNavBar()
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

			Me.navMain.Items.Clear()
			Try
				navMain.PaintStyleName = "SkinExplorerBarView"

				' Create a Local group.
				Dim groupDatei As NavBarGroup = New NavBarGroup(m_Translate.GetSafeTranslationValue("Datei"))
				groupDatei.Name = "gNavDatei"

				Dim New_P As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Neu"))
				New_P.Name = "New_ES"
				New_P.SmallImage = Me.ImageCollection1.Images(0)
				New_P.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 251, m_InitializationData.MDData.MDNr)

				m_SaveButton = New NavBarItem(m_Translate.GetSafeTranslationValue("Daten sichern"))
				m_SaveButton.Name = "Save_ES_Data"
				m_SaveButton.SmallImage = Me.ImageCollection1.Images(1)
				m_SaveButton.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 251, m_InitializationData.MDData.MDNr)

				Dim Print_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Drucken"))
				Print_P_Data.Name = "Print_ES_Data"
				Print_P_Data.SmallImage = Me.ImageCollection1.Images(2)
				Print_P_Data.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 253, m_InitializationData.MDData.MDNr) Or IsUserActionAllowed(m_InitializationData.UserData.UserNr, 255, m_InitializationData.MDData.MDNr)

				Dim Close_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Schliessen"))
				Close_P_Data.Name = "Close_ES_Form"
				Close_P_Data.SmallImage = Me.ImageCollection1.Images(3)

				Dim groupDelete As NavBarGroup = New NavBarGroup(m_Translate.GetSafeTranslationValue("Löschen"))
				groupDelete.Name = "gNavDelete"
				groupDelete.Appearance.ForeColor = Color.Red

				Dim Delete_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Löschen"))
				Delete_P_Data.Name = "Delete_ES_Data"
				Delete_P_Data.SmallImage = Me.ImageCollection1.Images(4)
				Delete_P_Data.Appearance.ForeColor = Color.Red
				Delete_P_Data.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 252, m_InitializationData.MDData.MDNr) AndAlso IsUserActionAllowed(m_InitializationData.UserData.UserNr, 302, m_InitializationData.MDData.MDNr)

				Dim groupExtra As NavBarGroup = New NavBarGroup(m_Translate.GetSafeTranslationValue("Extras"))
				groupExtra.Name = "gNavExtra"

				Dim Property_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Rapporte anzeigen"))
				Property_P_Data.Name = "showallrp"
				Property_P_Data.SmallImage = Me.ImageCollection1.Images(9)
				Property_P_Data.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 300, m_InitializationData.MDData.MDNr)

				m_Wos_ESVertrag_Data = New NavBarItem(m_Translate.GetSafeTranslationValue("Einsatzvertrag übermitteln"))
				m_Wos_ESVertrag_Data.Name = "wos_ESVertrag_Data"
				m_Wos_ESVertrag_Data.SmallImage = Me.ImageCollection1.Images(5)

				m_Wos_VerleihVertrag_Data = New NavBarItem(m_Translate.GetSafeTranslationValue("Verleihvertrag übermitteln"))
				m_Wos_VerleihVertrag_Data.Name = "wos_ESVerleihVertrag_Data"
				m_Wos_VerleihVertrag_Data.SmallImage = Me.ImageCollection1.Images(5)

				Dim TODO_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("To-do erstellen"))
				TODO_P_Data.Name = "CreateTODO"
				TODO_P_Data.SmallImage = Me.ImageCollection1.Images(8)

				Try
					navMain.BeginUpdate()

					navMain.Groups.Add(groupDatei)
					If IsUserActionAllowed(m_InitializationData.UserData.UserNr, 251, m_InitializationData.MDData.MDNr) Then
						groupDatei.ItemLinks.Add(New_P)
						groupDatei.ItemLinks.Add(m_SaveButton)
					End If

					If IsUserActionAllowed(m_InitializationData.UserData.UserNr, 253, m_InitializationData.MDData.MDNr) Or IsUserActionAllowed(m_InitializationData.UserData.UserNr, 255, m_InitializationData.MDData.MDNr) Then groupDatei.ItemLinks.Add(Print_P_Data)
					groupDatei.ItemLinks.Add(Close_P_Data)
					groupDatei.Expanded = True

					navMain.Groups.Add(groupDelete)
					groupDelete.ItemLinks.Add(Delete_P_Data)
					groupDelete.Expanded = False

					navMain.Groups.Add(groupExtra)
					groupExtra.ItemLinks.Add(TODO_P_Data)
					groupExtra.ItemLinks.Add(Property_P_Data)

					If m_md.AllowedExportEmployee2WOS(m_InitializationData.MDData.MDNr, Now.Year) Then groupExtra.ItemLinks.Add(m_Wos_ESVertrag_Data)
					If m_md.AllowedExportCustomer2WOS(m_InitializationData.MDData.MDNr, Now.Year) Then groupExtra.ItemLinks.Add(m_Wos_VerleihVertrag_Data)

					groupExtra.Expanded = True

					navMain.EndUpdate()

					If m_ESNumber.HasValue Then
						Dim esMasterData = m_ESDatabaseAccess.LoadESMasterData(m_ESNumber)
						bsiCreated.Caption = String.Format("{0:f}, {1}", esMasterData.CreatedOn, m_Utility.SafeTrim(esMasterData.CreatedFrom))
						bsiChanged.Caption = String.Format("{0:f}, {1}", esMasterData.ChangedOn, m_Utility.SafeTrim(esMasterData.ChangedFrom))

					End If

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.Navbar Controls binden. {1}", strMethodeName, ex.Message))
					DevExpress.XtraEditors.XtraMessageBox.Show(String.Format("Fehler (navBarMain): {0}", ex.Message), "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error)

				End Try

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
				DevExpress.XtraEditors.XtraMessageBox.Show(String.Format("Fehler (navBarMain): {0}", ex.Message),
													   "Menüleiste", MessageBoxButtons.OK, MessageBoxIcon.Error)

			End Try

		End Sub

		''' <summary>
		''' Prepares status and navigation bar.
		''' </summary>
		''' <param name="esMasterData">The ES master data.</param>
		''' <returns>Boolean flag indicating success</returns>
		Private Function PrepareStatusAndNavigationBar(ByVal esMasterData As ESMasterData)

			bsiCreated.Caption = String.Format(" {0:f}, {1}", esMasterData.CreatedOn, m_Utility.SafeTrim(esMasterData.CreatedFrom))
			bsiChanged.Caption = String.Format(" {0:f}, {1}", esMasterData.ChangedOn, m_Utility.SafeTrim(esMasterData.ChangedFrom))

			Dim employeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(esMasterData.EmployeeNumber)
			Dim customerMasterData = m_CustomerDatabaseAccess.LoadCustomerMasterData(esMasterData.CustomerNumber, m_InitializationData.UserData.UserFiliale)

			If employeeMasterData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Einsatz (Statuszeile/Navigation) konnten nicht geladen werden."))
				Return False
			End If

			Return True
		End Function

		''' <summary>
		''' Clickevent for Navbar.
		''' </summary>
		Private Sub OnnbMain_LinkClicked(ByVal sender As Object,
									 ByVal e As DevExpress.XtraNavBar.NavBarLinkEventArgs) Handles navMain.LinkClicked
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim bForDesign As Boolean = False
			Try
				Dim strLinkName As String = e.Link.ItemName
				Dim strLinkCaption As String = e.Link.Caption

				For i As Integer = 0 To Me.navMain.Groups(0).NavBar.Items.Count - 1
					e.Link.NavBar.Items(i).Appearance.ForeColor = Color.Black
				Next
				e.Link.Item.Appearance.ForeColor = Color.Orange

				Select Case strLinkName.ToLower
					Case "New_ES".ToLower()
						ShowNewESWizard()

					Case "Save_ES_Data".ToLower
						SaveESData()

					Case "Print_ES_Data".ToLower
						GetMenuItems4Print()

					Case "Close_ES_Form".ToLower
						CleanupAndHideForm()

					Case "delete_ES_Data".ToLower
						If DeleteSelectedES() Then Me.Close()

					Case "showallrp".ToLower
						ShowAllRP4SelectedES()

					Case "wos_ESVertrag_Data".ToLower
						SendESVertrag4SelectedESToWOS()

					Case "wos_ESVerleihVertrag_Data".ToLower
						SendVerleihVertrag4SelectedESToWOS()

					Case "CreateTODO".ToLower
						ShowTodo()


					Case Else
						' Do nothing
				End Select

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
				m_UtilityUI.ShowErrorDialog(ex.Message)

			Finally

			End Try

		End Sub

		''' <summary>
		''' Shows new ES wizard.
		''' </summary>
		Private Sub ShowNewESWizard()
			If m_InitializationData.MDData.ClosedMD = 1 Then Return

			Dim frmNewEs As SP.MA.EinsatzMng.UI.frmNewES = New SP.MA.EinsatzMng.UI.frmNewES(m_InitializationData, Nothing)

			frmNewEs.Show()
			frmNewEs.BringToFront()

		End Sub


#Region "Print functions"

		''' <summary>
		''' Build contextmenu for print.
		''' </summary>
		Private Sub GetMenuItems4Print()
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

			Dim mnuData = m_ESDatabaseAccess.LoadContextMenu4PrintData
			If (mnuData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Inhalte konnten nicht geladen werden."))
				Exit Sub
			End If
			BarManager1.BeginUpdate()
			BarManager1.ForceInitialize()

			Dim popupMenu As New DevExpress.XtraBars.PopupMenu
			popupMenu.Manager = Me.BarManager1

			Dim itm As New DevExpress.XtraBars.BarButtonItem

			For i As Integer = 0 To mnuData.Count - 1
				itm = New DevExpress.XtraBars.BarButtonItem
				Dim strMnuBez As String = mnuData(i).MnuCaption

				itm.Caption = strMnuBez
				itm.Name = mnuData(i).MnuName

				If itm.Name.ToLower = "1.5".ToLower Then
					If Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 108, m_InitializationData.MDData.MDNr) Then Continue For
				ElseIf itm.Name.ToLower = "1.7".ToLower Then
					If Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 110, m_InitializationData.MDData.MDNr) Then Continue For
				ElseIf itm.Name.ToLower = "1.4".ToLower Then
					If Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 106, m_InitializationData.MDData.MDNr) Then Continue For
				ElseIf itm.Name.ToLower = "10.4".ToLower Then
					If Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 303, m_InitializationData.MDData.MDNr) Then Continue For

				ElseIf itm.Name.ToLower = "4.3".ToLower Then
					If Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 253, m_InitializationData.MDData.MDNr) Then Continue For
				ElseIf itm.Name.ToLower = "4.2".ToLower Then
					If Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 255, m_InitializationData.MDData.MDNr) Then Continue For
				End If


				If strMnuBez.StartsWith("_") OrElse strMnuBez.StartsWith("-") Then
					itm.Caption = m_Translate.GetSafeTranslationValue(strMnuBez.Remove(0, 1))
					popupMenu.AddItem(itm).BeginGroup = True
				Else
					itm.Caption = m_Translate.GetSafeTranslationValue(itm.Caption)
					popupMenu.AddItem(itm)
				End If




				'If bAsGroup Then
				'      popupMenu.AddItem(itm).BeginGroup = True
				'    Else
				'      popupMenu.AddItem(itm)
				'    End If

				If itm.Name = "1.4" Then
					AddHandler itm.ItemClick, AddressOf PrintSuvaStdListe4SelectedES

				ElseIf itm.Name = "1.5" Then
					AddHandler itm.ItemClick, AddressOf PrintZV4SelectedES

				ElseIf itm.Name = "1.7" Then
					AddHandler itm.ItemClick, AddressOf PrintARG4SelectedES

				ElseIf itm.Name = "4.2" Then
					AddHandler itm.ItemClick, AddressOf PrintVerleihVertrag4SelectedES

				ElseIf itm.Name = "4.3" Then
					AddHandler itm.ItemClick, AddressOf PrintESVertrag4SelectedES

					'ElseIf itm.Name = "10.0.2" Then
					'  AddHandler itm.ItemClick, AddressOf PrintKstStdData4SelectedES

				ElseIf itm.Name = "10.4" Then
					Dim allowedtoprint = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 303, m_InitializationData.MDData.MDNr)
					If m_ESNumber.HasValue Then
						Dim esMasterData = m_ESDatabaseAccess.LoadESMasterData(m_ESNumber)
						If (esMasterData.PrintNoRP.HasValue AndAlso esMasterData.PrintNoRP) OrElse Not allowedtoprint Then itm.Enabled = False
					End If
					AddHandler itm.ItemClick, AddressOf PrintRP4SelectedES

				ElseIf itm.Name = "100.2" Then
					AddHandler itm.ItemClick, AddressOf PrintTemplate4SelectedES

				Else
					AddHandler itm.ItemClick, AddressOf PrintAllVertrag4SelectedES

				End If

			Next

			' fill templates
			Dim mnuTemplatesData = m_ESDatabaseAccess.LoadContextMenu4PrintTemplatesData
			If Not (mnuTemplatesData Is Nothing) Then
				For i As Integer = 0 To mnuTemplatesData.Count - 1
					itm = New DevExpress.XtraBars.BarButtonItem

					Dim strMnuBez As String = m_Translate.GetSafeTranslationValue(mnuTemplatesData(i).MnuCaption)
					Dim bAsGroup As Boolean = strMnuBez.StartsWith("-") OrElse i = 0
					itm.Caption = strMnuBez.Replace("-", "")
					itm.Name = String.Format("{0}|{1}", mnuTemplatesData(i).MnuDocPath, mnuTemplatesData(i).MnuDocMacro)

					If bAsGroup Then
						popupMenu.AddItem(itm).BeginGroup = True
					Else
						popupMenu.AddItem(itm)
					End If

					AddHandler itm.ItemClick, AddressOf PrintDocs

					'itm.Caption = m_Translate.GetSafeTranslationValue((mnuTemplatesData(i).MnuCaption))
					'itm.Name = String.Format("{0}|{1}", mnuTemplatesData(i).MnuDocPath, mnuTemplatesData(i).MnuDocMacro)

					'If i = 0 Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)
					'AddHandler itm.ItemClick, AddressOf PrintDocs
				Next
			End If
			BarManager1.EndUpdate()

			' show contextmenu
			popupMenu.ShowPopup(New Point(Me.navMain.Width + Me.Left, Cursor.Position.Y))

		End Sub

		Sub PrintAllVertrag4SelectedES()
			Dim success = PrintESVertrag_(m_ESNumber, False, True, 0)

			If success Then
				SetVerleihAndEinsatzVertragPrintedFlag(True, True)
			End If

		End Sub

		Sub PrintESVertrag4SelectedES()
			Dim success = PrintESVertrag_(m_ESNumber, False, False, 0)

			If success Then
				SetVerleihAndEinsatzVertragPrintedFlag(False, True)
			End If

		End Sub

		Sub PrintVerleihVertrag4SelectedES()
			Dim success = PrintESVertrag_(m_ESNumber, True, False, 0)

			If success Then
				SetVerleihAndEinsatzVertragPrintedFlag(True, False)
			End If

		End Sub

		Sub SendESVertrag4SelectedESToWOS()
			PrintESVertrag_(m_ESNumber, False, False, 2)
		End Sub

		Sub SendVerleihVertrag4SelectedESToWOS()
			PrintESVertrag_(m_ESNumber, True, False, 2)
		End Sub

		Sub PrintTemplate4SelectedES(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
			PrintESTemplate_(m_ESNumber, e.Item.Name)
		End Sub


		''' <summary>
		''' druckt den ausgewählten Einsatzvertrag aus
		''' </summary>
		''' <param name="iESNr"></param>
		''' <param name="sWOS">0 = NUR Drucken | 1 = Drucken und Senden | 2 = NUR Senden</param>
		''' <returns>Boolean flag indicating success.</returns>
		''' <remarks></remarks>
		Function PrintESVertrag_(ByVal iESNr As Integer, ByVal _bAsVerleih As Boolean, ByVal _bPrintWithVerleih As Boolean, ByVal sWOS As Short) As Boolean
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim liESNr As List(Of Integer) = New List(Of Integer)(New Integer() {iESNr})
			Dim liMANr As New List(Of Integer)
			Dim ShowDesign As Boolean = m_AllowedESVerDesign AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)

			Try
				Dim _settring As New SP.ES.PrintUtility.ClsESSetting With {.SelectedESNr = liESNr,
																																		 .SearchAutomatic = False,
																																		 .ShowDesign = ShowDesign,
																																		 .MDData = m_InitializationData.MDData,
																																		 .UserData = m_InitializationData.UserData,
																																		 .PerosonalizedData = m_InitializationData.ProsonalizedData,
																																		 .TranslationData = m_InitializationData.TranslationData}
				Dim obj As New SP.ES.PrintUtility.ClsMain_Net(_settring)
				obj.PrintSelectedES(_bAsVerleih, _bPrintWithVerleih, sWOS)
				m_Logger.LogDebug(String.Format("{0}: ESNr: {1} | _bAsVerleih:({2}) | Wos:({3})", strMethodeName, liESNr(0), _bAsVerleih, sWOS))

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}:ESNr: {1}:{2}", strMethodeName, liESNr(0), ex.Message))
				Return False
			End Try

			Return True
		End Function

		Function PrintESTemplate_(ByVal iESNr As Integer, ByVal jobNr As String) As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strResult As String = "Success..."

			Try
				Dim _settring As New ClsLLESTemplateSetting With {.SelectedESNr2Print = m_ESNumber,
																 .DbConnString2Open = m_InitializationData.MDData.MDDbConn,
																 .LogedUSNr = m_InitializationData.UserData.UserNr,
																 .SelectedMDNr = m_InitializationData.MDData.MDNr,
																 .SQL2Open = String.Empty,
																 .JobNr2Print = jobNr,
																 .PerosonalizedData = m_InitializationData.ProsonalizedData,
																 .TranslationData = m_InitializationData.TranslationData,
															  .bAsDesign = False}
				Dim obj As New ClsPrintESTemplate(_settring)
				strResult = obj.PrintESTemplate()

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}:ESNr: {1}:{2}", strMethodeName, m_ESNumber, ex.Message))
				strResult = "Error: " & String.Format("{0}:ESNr: {1}:{2}", strMethodeName, m_ESNumber, ex.Message)

			End Try

			Return strResult
		End Function

		''' <summary>
		''' startet das Threading zum öffnen der Einsatzverwaltung
		''' </summary>
		''' <remarks></remarks>
		Sub PrintSuvaStdListe4SelectedES()
			Dim frm As New SP.Employee.SuvaSTDSearch.frmSUVAStd(m_InitializationData)

			Dim employeeNumbers As New List(Of Integer?) '(New Integer() {1352655})
			employeeNumbers.Add(UCMediator.EmployeeNumber)

			Dim preselectionSetting As New SP.Employee.SuvaSTDSearch.PreselectionData With {.MDNr = m_InitializationData.MDData.MDNr, .EmployeeNumbers = employeeNumbers, .ListYear = Now.Year} ' New List(Of Integer?)(New Integer() {1352655})}
			frm.PreselectionData = preselectionSetting
			frm.PreselectData()

			frm.Show()
			frm.BringToFront()

		End Sub

		''' <summary>
		''' öffnet die Suva-Stundenliste über Hauptübersicht
		''' </summary>
		''' <remarks></remarks>
		Private Sub OpenForm4SuvaStdListeWithThreading()
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim oMyProg As Object
			Dim strTranslationProgName As String = String.Empty
			Dim _ClsReg As New SPProgUtility.ClsDivReg

			Try
				oMyProg = CreateObject("SPSModulsView.ClsMain")
				oMyProg.TranslateProg4Net("SPSMASuvaList.ClsMain", UCMediator.EmployeeNumber)


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.Message))
				m_UtilityUI.ShowErrorDialog(String.Format("{0}: {1}", strMethodeName, ex.Message))

			End Try

		End Sub

		''' <summary>
		''' startet das Threading zum öffnen der Zwischenverdienstverwaltung
		''' </summary>
		''' <remarks></remarks>
		Sub PrintZV4SelectedES()
			Dim ShowDesign As Boolean = (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)

			Try
				Dim frmZV = New SPS.MA.Guthaben.frmZV(m_InitializationData)

				Dim preselectionSetting As New SPS.MA.Guthaben.PreselectionZVData With {.MDNr = m_InitializationData.MDData.MDNr, .EmployeeNumber = UCMediator.EmployeeNumber}
				frmZV.PreselectionData = preselectionSetting

				frmZV.LoadData()
				frmZV.DisplayEmployeeData()

				frmZV.Show()
				frmZV.BringToFront()

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))
				m_UtilityUI.ShowErrorDialog(String.Format("{0}", ex.ToString))

			End Try

			'Dim t As Thread = New Thread(AddressOf OpenZVFormWithThreading)

			't.IsBackground = True
			'   t.Name = "OpenZVFormWithThreading"
			'   't.SetApartmentState(ApartmentState.STA)
			'   t.Start()

		End Sub


		''' <summary>
		''' startet das Threading zum öffnen der Arbeitgeberbescheinigung 
		''' </summary>
		''' <remarks></remarks>
		Sub PrintARG4SelectedES()
			Dim ShowDesign As Boolean = (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)

			Try

				Dim frmARGB = New SPS.MA.Guthaben.frmARGB(m_InitializationData)

				Dim preselectionSetting As New SPS.MA.Guthaben.PreselectionARGBData With {.MDNr = m_InitializationData.MDData.MDNr, .EmployeeNumber = UCMediator.EmployeeNumber}
				frmARGB.PreselectionData = preselectionSetting

				frmARGB.LoadData()
				frmARGB.DisplayEmployeeData()

				frmARGB.Show()
				frmARGB.BringToFront()

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))
				m_UtilityUI.ShowErrorDialog(String.Format("{0}", ex.ToString))

			End Try

		End Sub

		''' <summary>
		''' Druckt Rapprote über selektierten Einsatz
		''' </summary>
		''' <remarks></remarks>
		Sub PrintRP4SelectedES()

			'If My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown Then
			Try
				Dim frm = New frmRPListSearch(m_InitializationData)
				Dim EmployeeNumber As Integer? = UCMediator.EmployeeNumber
				Dim CustomerNumber As Integer? = UCMediator.CustomerNumber

				frm.EmployeeNumber = EmployeeNumber
				frm.CustomerNumber = CustomerNumber
				frm.EmploymentNumber = m_ESNumber
				frm.LoadData()

				frm.Show()
				frm.BringToFront()

			Catch ex As Exception
				m_UtilityUI.ShowErrorDialog(ex.ToString)
				m_Logger.LogError(ex.ToString)

				Return
			End Try

		End Sub

		'''' <summary>
		'''' öffnet die Stundenliste pro KD-KST
		'''' </summary>
		'''' <remarks></remarks>
		'Private Sub OpenForm4PrintingRapportWithThreading()
		'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		'	Dim oMyProg As Object
		'	Dim strTranslationProgName As String = String.Empty
		'	Dim _ClsReg As New SPProgUtility.ClsDivReg

		'	Try

		'		If Not m_ESNumber.HasValue Then
		'			m_UtilityUI.ShowErrorDialog("Einsatzdaten konnten nicht geladen werden")
		'			Exit Sub
		'		End If

		'		oMyProg = CreateObject("SPSModulsView.ClsMain")
		'		oMyProg.TranslateProg4Net("SPSPrintESRPUtil.ClsMain", m_ESNumber)


		'	Catch ex As Exception
		'		m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.Message))
		'		m_UtilityUI.ShowErrorDialog(String.Format("{0}: {1}", strMethodeName, ex.Message))

		'	End Try

		'End Sub

		''' <summary>
		''' Prints documents.
		''' </summary>
		Private Sub PrintDocs(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

			Dim EmployeeNumber As Integer? = UCMediator.EmployeeNumber
			Dim CustomerNumber As Integer? = UCMediator.CustomerNumber
			Dim CResponsibleNumber As Integer? = UCMediator.CustomerResponsibleNumber

			Dim strMenuName As String() = e.Item.Name.Split("|")
			If strMenuName.Length = 2 Then
				' then office-templates...
				If Not strMenuName(0).Substring(1, 2) = ":\" And Not strMenuName(0).StartsWith("\\") Then
					strMenuName(0) = Path.Combine(m_md.GetSelectedMDTemplatePath(m_InitializationData.MDData.MDNr), strMenuName(0))
				End If
				If Not File.Exists(strMenuName(0)) Then Return
				Dim fi As New FileInfo(strMenuName(0))
				Dim newFilename As String = String.Format("{0}{1}", m_path.GetSpS2DeleteHomeFolder, fi.Name)

				Try
					File.Copy(strMenuName(0), newFilename, True)
				Catch ex As Exception
					m_Logger.LogError(String.Format(m_Translate.GetSafeTranslationValue("{0}.Datei konnte nicht kopiert werden. {1}"), strMethodeName, ex.Message))
					newFilename = strMenuName(0)
				End Try
				Try

					Dim _reg As New SPProgUtility.ClsDivReg
					_reg.SetRegKeyValue("Software\yourregistrykeyname\Sputnik Suite\ProgOptions", "ESNr", m_ESNumber.GetValueOrDefault(0))
					_reg.SetRegKeyValue("Software\yourregistrykeyname\Sputnik Suite\ProgOptions", "MANr", EmployeeNumber.GetValueOrDefault(0))
					_reg.SetRegKeyValue("Software\yourregistrykeyname\Sputnik Suite\ProgOptions", "KDNr", CustomerNumber.GetValueOrDefault(0))
					_reg.SetRegKeyValue("Software\yourregistrykeyname\Sputnik Suite\ProgOptions", "KDZHDNr", CResponsibleNumber.GetValueOrDefault(0))
					_reg.SetRegKeyValue("Software\yourregistrykeyname\Sputnik Suite\ProgOptions", "KDZuNr", CResponsibleNumber.GetValueOrDefault(0))
					_reg.SetRegKeyValue("Software\yourregistrykeyname\Sputnik Suite\ProgOptions", "MandantNumber", UCMediator.MandantNumber)
					_reg.SetRegKeyValue("Software\yourregistrykeyname\Sputnik Suite\ProgOptions", "CurrentRecordMandantPath", m_InitializationData.MDData.MDMainPath)
					_reg.SetRegKeyValue("Software\yourregistrykeyname\Sputnik Suite\ProgOptions", "CurrentRecordMandantTemplatePath", m_InitializationData.MDData.MDTemplatePath)

					If fi.Extension.ToUpper = ".DOC" OrElse fi.Extension.ToUpper = ".DOCX" OrElse fi.Extension.ToUpper = ".DOCM" Then
						Dim _clsBrowser As New ClassBrowserPath
						_clsBrowser.GetBrowserApplicationPath(strMenuName(0))
						Dim startInfo As New ProcessStartInfo

						startInfo.FileName = _clsBrowser.GetBrowserPath
						startInfo.Arguments = Chr(34) & newFilename & Chr(34) & If(strMenuName(1) <> String.Empty, " /m" & strMenuName(1), "")
						startInfo.UseShellExecute = False
						Process.Start(startInfo)

					ElseIf fi.Extension.ToUpper = ".PDF" Then
						If strMenuName(1) = "1.0.1" Then
							Dim _Setting As New SPS.Listing.Print.Utility.ClsLLMATemplateSetting With {.TemplateName = newFilename,
								.EmploymentNumbers2Print = New List(Of Integer)(New Integer() {m_ESNumber}),
								.CustomerNumbers2Print = New List(Of Integer)(New Integer() {CustomerNumber}),
								.EmployeeNumbers2Print = New List(Of Integer)(New Integer() {EmployeeNumber})}

							Dim obj As New SPS.Listing.Print.Utility.MATemplates.ClsPrintMATemplates(m_InitializationData)
							Dim success = obj.PrintMATemplatePDU1PDF(_Setting)

							If Not success.Printresult Then
								m_UtilityUI.ShowInfoDialog(String.Format(m_Translate.GetSafeTranslationValue("Ihre Vorlage konnte nicht geöffnet werden!{0}{1}"), vbNewLine, newFilename))

								Return
							End If

						ElseIf strMenuName(1) = "1.0.2" Then
							Dim _Setting As New SPS.Listing.Print.Utility.ClsLLMATemplateSetting With {.TemplateName = newFilename,
									.EmploymentNumbers2Print = New List(Of Integer)(New Integer() {m_ESNumber}),
									.CustomerNumbers2Print = New List(Of Integer)(New Integer() {CustomerNumber}),
									.EmployeeNumbers2Print = New List(Of Integer)(New Integer() {EmployeeNumber})}

							Dim obj As New SPS.Listing.Print.Utility.MATemplates.ClsPrintMATemplates(m_InitializationData)
							Dim success = obj.PrintTGQuest110PDF(_Setting)

							If Not success.Printresult Then
								m_UtilityUI.ShowInfoDialog(String.Format(m_Translate.GetSafeTranslationValue("Ihre Vorlage konnte nicht geöffnet werden!{0}{1}"), vbNewLine, newFilename))

								Return
							End If

						Else

							Process.Start(newFilename)

						End If

					End If


				Catch ex As Exception
					m_Logger.LogError(String.Format(m_Translate.GetSafeTranslationValue("{0}.Datei konnte nicht geöffnet werden. {1}"), strMethodeName, ex.Message))
					m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Datei konnte nicht geöffnet werden. {0}"), ex.Message))
				End Try


			End If

		End Sub


#End Region


		''' <summary>
		''' delete selected es
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function DeleteSelectedES() As Boolean
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strResult As String = "Success..."
			Dim liESNr As New List(Of Integer)
			Dim liMANr As New List(Of Integer)

			liESNr.Add(m_ESNumber)

			Try
				Dim _settring As New SP.ES.PrintUtility.ClsESSetting With {.SelectedESNr = liESNr}
				Dim obj As New SP.ES.PrintUtility.ClsMain_Net(_settring)

				strResult = obj.StartDeleteingSelectedES(True)

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.ToString))
				m_UtilityUI.ShowErrorDialog(ex.ToString)

			End Try

			Return strResult.ToLower.Contains("success")
		End Function

		''' <summary>
		''' öffnet die Maske mit allen Rapporten
		''' </summary>
		''' <remarks></remarks>
		Private Sub ShowAllRP4SelectedES()
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

			Try
				If Not m_ESNumber.HasValue Then
					m_UtilityUI.ShowErrorDialog("Einsatzdaten konnten nicht geladen werden")
					Exit Sub
				End If

				If m_PropertyForm Is Nothing OrElse m_PropertyForm.IsDisposed Then
					m_PropertyForm = New frmFoundedReports(m_InitializationData, m_Translate, m_ESNumber)
				Else
				End If
				m_PropertyForm.LoadFoundedCustomerList(m_ESDatabaseAccess, m_ESNumber, Nothing, Nothing)
				m_PropertyForm.Show()
				m_PropertyForm.BringToFront()


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.Message))
				m_UtilityUI.ShowErrorDialog(String.Format("{0}: {1}", strMethodeName, ex.Message))

			End Try

		End Sub



		''' <summary>
		''' Shows a todo From.
		''' </summary>
		Private Sub ShowTodo()
			Dim frmTodo As New frmTodo(m_InitializationData)
			' optional init new todo
			Dim UserNumber As Integer = m_InitializationData.UserData.UserNr
			Dim EmployeeNumber As Integer? = UCMediator.EmployeeNumber
			Dim CustomerNumber As Integer? = UCMediator.CustomerNumber
			Dim ResponsiblePersonRecordNumber As Integer? = Nothing
			Dim VacancyNumber As Integer? = Nothing
			Dim ProposeNumber As Integer? = Nothing
			Dim ESNumber As Integer? = m_ESNumber
			Dim RPNumber As Integer? = Nothing
			Dim LMNumber As Integer? = Nothing
			Dim RENumber As Integer? = Nothing
			Dim ZENumber As Integer? = Nothing
			Dim Subject As String = String.Empty
			Dim Body As String = ""

			frmTodo.EmployeeNumber = EmployeeNumber
			frmTodo.CustomerNumber = CustomerNumber
			frmTodo.ESNumber = m_ESNumber
			frmTodo.InitNewTodo(UserNumber, Subject, Body, EmployeeNumber, CustomerNumber, ResponsiblePersonRecordNumber,
														VacancyNumber, ProposeNumber, ESNumber, RPNumber, LMNumber, RENumber, ZENumber)

			frmTodo.Show()

		End Sub

		''' <summary>
		''' Cleanup and close form.
		''' </summary>
		Public Sub CleanupAndHideForm()

			SaveFromSettings()

			' Cleanup child panels.
			If Not m_PropertyForm Is Nothing AndAlso Not m_PropertyForm.IsDisposed Then

				Try
					m_PropertyForm.Close()
					m_PropertyForm.Dispose()
				Catch
					' Do nothing
				End Try
			End If

			For Each ctrl In m_ListOfUserControls
				ctrl.CleanUp()
			Next

			Me.Hide()
			Me.Reset() 'Clear all data.

		End Sub

		''' <summary>
		''' Sets the Verleih- and Einatz printed flag.
		''' </summary>
		Private Sub SetVerleihAndEinsatzVertragPrintedFlag(ByVal updateVerleihVertragFlag As Boolean, ByVal updateEinsatzVertragFlag As Boolean)

			Dim errorMessage = m_Translate.GetSafeTranslationValue("Verleihvertrag gedruckt Status konnte nicht aktualisiert werden.")

			Dim esMasterData = m_ESDatabaseAccess.LoadESMasterData(m_ESNumber)

			If (esMasterData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(errorMessage)
				Return
			End If

			If (updateVerleihVertragFlag) Then esMasterData.Print_KD = True
			If updateEinsatzVertragFlag Then esMasterData.Print_MA = True


			If Not m_ESDatabaseAccess.UpdateESMasterData(esMasterData) Then
				m_UtilityUI.ShowErrorDialog(errorMessage)

			Else
				m_UCMediator.RefreshVerleihAndEinsatzVertragPrintedData(esMasterData)

			End If

		End Sub

		''' <summary>
		''' Handles click on bsiBtnGavInfo.
		''' </summary>
		Private Sub OnbsiBtnGavInfo_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bsiBtnGavInfo.ItemClick

			If m_AllowedTempDataPVL Then
				ShowGAVNotificationMessage_API()

			Else
				ShowGAVNotificationMessage_XML()

			End If

		End Sub

		Private Sub ShowGAVNotificationMessage_XML()

			If Not m_GavUpdateInfoOfLastCheck Is Nothing AndAlso Not String.IsNullOrWhiteSpace(m_GavUpdateInfoOfLastCheck.GAVInfo) Then

				Dim infoText = String.Format(m_GavUpdateInfoOfLastCheck.GAVInfo, vbCrLf)

				m_UtilityUI.ShowOKDialog(infoText, String.Format("{0} (Nr.: {1}): {2:g}", m_Translate.GetSafeTranslationValue("Datum letzte GAV Anpassung"),
																													 m_GavUpdateInfoOfLastCheck.GAVNumber,
																													 m_GavUpdateInfoOfLastCheck.GAVDate.Value))

			End If

		End Sub

		Private Sub ShowGAVNotificationMessage_API()

			If m_tempDataMergedNews Is Nothing OrElse m_tempDataMergedNews.VersionNumber.GetValueOrDefault(0) = 0 Then Return

			Dim infoText = String.Format("{0:G}:<br>{1}", m_tempDataMergedNews.PublicationDate, m_tempDataMergedNews.Content)
			m_UtilityUI.ShowOKDialog(Me, infoText, m_Translate.GetSafeTranslationValue("GAV Anpassung"), MessageBoxIcon.Asterisk)

		End Sub

#End Region


	End Class

End Namespace