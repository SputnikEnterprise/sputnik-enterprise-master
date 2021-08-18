
Imports SPGAV.PVLWebServiceProcess
Imports System.Net.Http
Imports System.Threading.Tasks
Imports System.ComponentModel

Imports SP.DatabaseAccess.Common

Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.ES
Imports SP.DatabaseAccess.ES.DataObjects.ESMng
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.DatabaseAccess.Employee
Imports SPGAV.SPPVLGAVUtilWebService
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports DevExpress.XtraEditors
Imports SP.DatabaseAccess.Report
Imports SP.Infrastructure
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath
Imports SP.Infrastructure.UI
Imports DevExpress.LookAndFeel
Imports SP.Infrastructure.Logging
Imports System.Text
Imports System.IO
Imports SPGAV.TempData
Imports Newtonsoft.Json
Imports DevExpress.XtraGrid.Views.Layout
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraLayout.Utils
Imports DevExpress.XtraRichEdit
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.Utils.Text
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.Utils.Drawing
'Imports DevExpress.XtraBars.Docking2010
Imports DevExpress.Utils
Imports DevExpress.XtraSplashScreen
Imports SP.Internal.Automations

Namespace UI

	Public Class frmPublicationNews


#Region "Constants"

		Private Const SERVICENAME As String = "TEMPDATANOTIFICATION"

		Private Const JOBROOM_USER As String = "TempData-Api-Key"
		Private Const JOBROOM_PASSWORD As String = "v4k78GdE4s2a"
		Private Const DEFAULT_LANGUAGE As String = "de-ch"

		Private Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"

		Private Const DEFAULT_SPUTNIK_PVL_UTILITIES_WEBSERVICE_URI As String = "wsSPS_services/SPPVLGAVUtil.asmx"

		Private Const JOBROOM_URI As String = "https://www.tempdata-api.ch/api/"
		Private Const STAGING_JOBROOM_URI As String = "https://www.int.tempdata-api.ch/api/"


		Private Const JOBROOM_KEYWORDS_URI As String = "{0}keywords?language={1}"

		Private Const JOBROOM_PUBLICATION_NEWS_URI As String = "{0}publicationnews?language={1}"
		Private Const JOBROOM_BRACHES_URI As String = "{0}branchescontracts?language={1}"
		Private Const JOBROOM_CONTRACTS_URI As String = "{0}contracts?language={1}"
		Private Const JOBROOM_CONTRACT_NUMBER_URI As String = "{0}contracts/{2}?language={1}"
		Private Const JOBROOM_CONTRACT_VERSIONS_URI As String = "{0}contracts/{2}/versions?language={1}"
		Private Const JOBROOM_CONTRACTS_EDITIONS_URI As String = "{0}contracts/{2}/versions/{3}/editions?language={1}"
		Private Const JOBROOM_CONTRACT_EDITION_NUMBER_URI As String = "{0}contracts/{2}/versions/{3}/editions/{4}?language={1}"

		Private Const JOBROOM_DOCUMENT_EDITION_NUMBER_URI As String = "{0}contracts/{2}/versions/{3}/editions/{4}/documents?language={1}"
		Private Const JOBROOM_DOCUMENT_FILE_ID_URI As String = "{0}files/{1}"

		Private Const JOBROOM_LINKS_EDITION_NUMBER_URI As String = "{0}contracts/{2}/versions/{3}/editions/{4}/links?language={1}"

		Private Const JOBROOM_MINDESTLOHN_EDITION_URI As String = "{0}minimumsalary/{2}/structure?language={1}"
		Private Const JOBROOM_MINDESTLOHN_EDITION_CRITERIA_URI As String = "{0}minimumsalary/{2}/criteria/{3}?language={1}"

		Private Const JOBROOM_MINDESTLOHN_RESULT_URI As String = "{0}minimumsalary/{2}/result?language={1}"

#End Region


#Region "private fields"

		Protected Shared m_Logger As ILogger = New Logger()

		''' <summary>
		''' The Initialization data.
		''' </summary>
		Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

		''' <summary>
		''' The translation value helper.
		''' </summary>
		Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

		Private m_CommonDatabaseAccess As ICommonDatabaseAccess
		Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess
		Private m_CustomerDatabaseAccess As ICustomerDatabaseAccess
		Private m_ESDatabaseAccess As IESDatabaseAccess
		Private m_ReportDataAccess As IReportDatabaseAccess

		Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

		Private m_LOData As GAVCalculationDTO
		Private liLOData As New List(Of String)

		Private m_path As ClsProgPath
		Private m_md As Mandant
		Private m_UtilityUI As UtilityUI
		Private m_HtmlUtil As HTMLUtiles.Utilities
		Private m_JsonUtility As JsonUtility

		Private m_SPPVLUtilitiesServiceUrl As String

		Private m_EmployeeMasterData As EmployeeMasterData
		Private m_CustomerMasterData As CustomerMasterData
		Private m_ExistingCustomerGAVData As IEnumerable(Of CustomerAssignedGAVGroupData)

		Private m_EmploymentMasterData As ESMasterData
		Private m_CurrentESLohnData As IEnumerable(Of ESSalaryData)
		Private m_PVLListData As IEnumerable(Of GAVNameResultDTO)
		Private m_CurrentPVLData As GAVNameResultDTO

		Private m_CategoryData As BindingList(Of GAVCategoryDTO)


		Private m_ContractData As IEnumerable(Of ContractData)

		Private m_ContractNumber As Integer
		Private m_ContractVersionNumber As Integer
		Private m_ContractEditionNumber As Integer

		''' <summary>
		''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
		''' </summary>
		Private m_SuppressUIEvents As Boolean = False

		''' <summary>
		''' The utility.
		''' </summary>
		Private m_Utility As New Utility
		Private m_CurrentBranche As BranchesData
		Private m_PVLUtility As WebServiceProcess
		Private m_PVLArchiveDbName As String
		Private m_PVLSelectedData As String

		Private m_APIResponse As String
		'Private m_QueryResultData As SPAVAMQueryResultData

		Private m_ResultContent As String

		Private m_UserName As String
		Private m_Password As String
		Private m_JobroomURI As String
		Private m_JobroomAllRecordURI As String
		Private m_JobroomSingleRecordURI As String
		Private m_Language As String
		Private m_ErrorResult As ResponseErrorData
		Private m_FirstURI As String

		Private m_MindesLohnCriteriasData As List(Of MindestLohnInputCriteriasData)
		Private m_AllowedServicetoUse As Boolean

		Private m_FirstHandle As IOverlaySplashScreenHandle
		Private m_SecHandle As IOverlaySplashScreenHandle
		Private m_ThirdHandle As IOverlaySplashScreenHandle

#End Region


#Region "Constructor"

		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
			DevExpress.UserSkins.BonusSkins.Register()
			DevExpress.Skins.SkinManager.EnableFormSkins()

			m_InitializationData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
			m_md = New Mandant
			m_path = New ClsProgPath
			m_UtilityUI = New UtilityUI
			m_Utility = New Utility
			m_HtmlUtil = New HTMLUtiles.Utilities
			m_JsonUtility = New JsonUtility
			m_PVLUtility = New WebServiceProcess(m_InitializationData)
			m_AllowedServicetoUse = IsCustomerServiceAllowed(SERVICENAME)


			Dim m_connectionString = m_InitializationData.MDData.MDDbConn
			m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_CustomerDatabaseAccess = New CustomerDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_ESDatabaseAccess = New ESDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_ReportDataAccess = New ReportDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

			m_PVLArchiveDbName = String.Empty
			Dim domainName = m_InitializationData.MDData.WebserviceDomain
			m_SPPVLUtilitiesServiceUrl = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_PVL_UTILITIES_WEBSERVICE_URI)

			m_UserName = JOBROOM_USER
			m_Password = JOBROOM_PASSWORD

			m_SuppressUIEvents = True

			InitializeComponent()

			Me.KeyPreview = True
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

			TranslateControls()
			Reset()

			m_SuppressUIEvents = False


		End Sub


#End Region

#Region "public methodes"

		Public Function LoadData(ByVal hideReadeNews As Boolean) As Boolean
			Dim result As Boolean = True

			If Not m_AllowedServicetoUse Then
				m_Logger.LogWarning(String.Format("{0} modul is not allowed to use!!!", SERVICENAME))

				Return False
			End If


			Dim suppressUIEventsState = m_SuppressUIEvents
			m_SuppressUIEvents = True

			SplashScreenManager.CloseForm(False)
			SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
			SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Nach Daten wird gesucht") & Space(20))
			SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

			m_FirstURI = JOBROOM_URI
			If m_InitializationData.UserData.UserLanguage = "D" Then m_Language = "de-CH"
			If m_InitializationData.UserData.UserLanguage = "F" Then m_Language = "fr-CH"
			If m_InitializationData.UserData.UserLanguage = "I" Then m_Language = "it-CH"

			Dim listDataSource = LoadMergedPublicationNewsData(hideReadeNews)
			grdPublicationNews.DataSource = listDataSource

			m_SuppressUIEvents = suppressUIEventsState

			SplashScreenManager.CloseForm(False)


			Return Not listDataSource Is Nothing AndAlso listDataSource.Count > 0

		End Function

		Public Function LoadMergedNewsData(ByVal hideReadeNews As Boolean) As BindingList(Of PublicationNewsViewData)

			If Not m_AllowedServicetoUse Then
				m_Logger.LogWarning(String.Format("{0} modul is not allowed to use!!!", SERVICENAME))

				Return Nothing
			End If


			Dim suppressUIEventsState = m_SuppressUIEvents
			m_SuppressUIEvents = True

			SplashScreenManager.CloseForm(False)
			SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
			SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Nach Daten wird gesucht") & Space(20))
			SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

			Try
				m_FirstURI = JOBROOM_URI
				If m_InitializationData.UserData.UserLanguage = "D" Then m_Language = "de-CH"
				If m_InitializationData.UserData.UserLanguage = "F" Then m_Language = "fr-CH"
				If m_InitializationData.UserData.UserLanguage = "I" Then m_Language = "it-CH"

				Dim result = LoadMergedPublicationNewsData(hideReadeNews)

				Return result

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}: {1}", SERVICENAME, ex.ToString))
				Return Nothing

			Finally
				SplashScreenManager.CloseForm(False)
				m_SuppressUIEvents = suppressUIEventsState
			End Try

			Return Nothing
		End Function

		Public Function LoadMergedNewsForAssignedConctractData(ByVal contractNumber As Integer) As List(Of PublicationNewsViewData)

			If Not m_AllowedServicetoUse Then
				m_Logger.LogWarning(String.Format("{0} modul is not allowed to use!!!", SERVICENAME))

				Return Nothing
			End If

			Dim suppressUIEventsState = m_SuppressUIEvents
			m_SuppressUIEvents = True

			SplashScreenManager.CloseForm(False)
			SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
			SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Nach Daten wird gesucht") & Space(20))
			SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

			Try
				m_FirstURI = JOBROOM_URI
				If m_InitializationData.UserData.UserLanguage = "D" Then m_Language = "de-CH"
				If m_InitializationData.UserData.UserLanguage = "F" Then m_Language = "fr-CH"
				If m_InitializationData.UserData.UserLanguage = "I" Then m_Language = "it-CH"

				Dim mergedData = LoadMergedPublicationNewsData(False)
				If mergedData Is Nothing Then Return Nothing

				For Each gav In mergedData
					Trace.WriteLine(String.Format("{0}-{1}", gav.ContractNumber, gav.PublicationDate))
				Next

				Dim result = mergedData.Where(Function(x) x.ContractNumber = contractNumber).ToList()
				If result Is Nothing Then result.Add(New PublicationNewsViewData With {.ContractNumber = contractNumber})

				Return result


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}: {1}", SERVICENAME, ex.ToString))
				Return Nothing

			Finally
				SplashScreenManager.CloseForm(False)
				m_SuppressUIEvents = suppressUIEventsState
			End Try

			Return Nothing
		End Function


#End Region


#Region "private properties"

		Private ReadOnly Property SelectedPublicationNewsData As PublicationNewsViewData
			Get
				Dim gvRP = TryCast(grdPublicationNews.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (gvRP Is Nothing) Then

					Dim selectedRows = gvRP.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim employee = CType(gvRP.GetRow(selectedRows(0)), PublicationNewsViewData)
						Return employee
					End If

				End If

				Return Nothing
			End Get

		End Property

#End Region


		Private Sub TranslateControls()

			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
			tgsHideReadNews.Properties.OnText = m_Translate.GetSafeTranslationValue(tgsHideReadNews.Properties.OnText)
			tgsHideReadNews.Properties.OffText = m_Translate.GetSafeTranslationValue(tgsHideReadNews.Properties.OffText)

		End Sub


#Region "reset"

		Private Sub Reset()

			lblCountOfNews.Text = String.Empty
			ResetPublicationNews()

		End Sub

		Private Sub ResetPublicationNews()


			gvPublicationNews.OptionsView.ShowIndicator = False
			gvPublicationNews.OptionsView.ShowAutoFilterRow = True
			gvPublicationNews.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvPublicationNews.OptionsView.ShowFooter = False
			gvPublicationNews.OptionsBehavior.Editable = True
			gvPublicationNews.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
			gvPublicationNews.OptionsView.ShowDetailButtons = False
			gvPublicationNews.OptionsView.AutoCalcPreviewLineCount = True

			gvPublicationNews.Columns.Clear()


			Dim repoHTML = New RepositoryItemHypertextLabel
			repoHTML.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True
			repoHTML.Appearance.TextOptions.WordWrap = DevExpress.Utils.DefaultBoolean.True
			repoHTML.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
			repoHTML.Appearance.Options.UseTextOptions = True
			grdPublicationNews.RepositoryItems.Add(repoHTML)

			Dim richHtml As RepositoryItemRichTextEdit = New RepositoryItemRichTextEdit
			richHtml.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True
			richHtml.DocumentFormat = DocumentFormat.Html
			richHtml.Appearance.Font = New Font("thama", 8.25!, System.Drawing.FontStyle.Bold)
			richHtml.Appearance.TextOptions.WordWrap = DevExpress.Utils.DefaultBoolean.True
			richHtml.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
			richHtml.Appearance.Options.UseTextOptions = True
			grdPublicationNews.RepositoryItems.Add(richHtml)

			Dim m_CheckReadUpdate = New RepositoryItemButtonEdit With {.Name = "m_CheckEditUpdate", .Tag = 2, .ButtonsStyle = BorderStyles.Default, .HideSelection = True, .TextEditStyle = TextEditStyles.HideTextEditor}
			m_CheckReadUpdate.Buttons.Clear()
			m_CheckReadUpdate.Buttons.AddRange(New EditorButton() {New EditorButton(ButtonPredefines.Glyph, "Un/-gelesen", -1, True, True, True, ImageLocation.MiddleRight, DemoHelper.GetEditImage())})
			m_CheckReadUpdate.Buttons(0).ImageOptions.Image = My.Resources.updatetable_16x16
			grdPublicationNews.RepositoryItems.Add(m_CheckReadUpdate)
			AddHandler m_CheckReadUpdate.ButtonClick, AddressOf OnGVPublicationNews_UpdateButtonClick


			Dim columnRead As New DevExpress.XtraGrid.Columns.GridColumn()
			columnRead.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			columnRead.OptionsColumn.AllowEdit = True
			columnRead.Caption = " "
			columnRead.Name = "Update"
			columnRead.FieldName = "Update"
			columnRead.Visible = True
			columnRead.Width = 30
			columnRead.ColumnEdit = m_CheckReadUpdate
			gvPublicationNews.Columns.Add(columnRead)
			columnRead.ShowButtonMode = ShowButtonModeEnum.ShowAlways


			Dim columnViewed As New DevExpress.XtraGrid.Columns.GridColumn()
			columnViewed.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnViewed.OptionsColumn.AllowEdit = False
			columnViewed.Caption = m_Translate.GetSafeTranslationValue("Gelesen")
			columnViewed.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			columnViewed.Name = "Viewed"
			columnViewed.FieldName = "Viewed"
			columnViewed.Visible = True
			columnViewed.Width = 10
			gvPublicationNews.Columns.Add(columnViewed)


			Dim columncontractNumber As New DevExpress.XtraGrid.Columns.GridColumn()
			columncontractNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columncontractNumber.OptionsColumn.AllowEdit = False
			columncontractNumber.Caption = ""
			columncontractNumber.Name = "ContractNumber"
			columncontractNumber.FieldName = "ContractNumber"
			columncontractNumber.ColumnEdit = repoHTML
			columncontractNumber.Visible = False
			columncontractNumber.Width = 100
			gvPublicationNews.Columns.Add(columncontractNumber)

			Dim columnTitle As New DevExpress.XtraGrid.Columns.GridColumn()
			columnTitle.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnTitle.OptionsColumn.AllowEdit = False
			columnTitle.Caption = ""
			columnTitle.Name = "Title"
			columnTitle.FieldName = "Title"
			columnTitle.ColumnEdit = richHtml
			columnTitle.Visible = True
			columnTitle.Width = 100
			gvPublicationNews.Columns.Add(columnTitle)

			Dim columnPublicationDate As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPublicationDate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnPublicationDate.OptionsColumn.AllowEdit = False
			columnPublicationDate.Caption = ""
			columnPublicationDate.Name = "PublicationDate"
			columnPublicationDate.FieldName = "PublicationDate"
			columnPublicationDate.ColumnEdit = repoHTML
			columnPublicationDate.Visible = True
			columnPublicationDate.Width = 50
			gvPublicationNews.Columns.Add(columnPublicationDate)

			Dim columnContent As New DevExpress.XtraGrid.Columns.GridColumn()
			columnContent.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnContent.OptionsColumn.AllowEdit = False
			columnContent.Caption = ""
			columnContent.Name = "Content"
			columnContent.FieldName = "Content"
			columnContent.ColumnEdit = repoHTML
			columnContent.Visible = False
			columnContent.Width = 100
			gvPublicationNews.Columns.Add(columnContent)


			gvPublicationNews.Columns("Content").Visible = False
			gvPublicationNews.PreviewFieldName = "Content"
			gvPublicationNews.OptionsView.ShowPreview = True
			'gvGavDetail.PreviewLineCount = 3


			grdPublicationNews.DataSource = Nothing

		End Sub

#End Region


		Private Function LoadTempDataPublicationNewsData() As List(Of PublicationNewsData)
			Dim result As New List(Of PublicationNewsData)
			Dim baseUri As Uri = New Uri(String.Format(JOBROOM_PUBLICATION_NEWS_URI, m_FirstURI, m_Language))
			Dim JSonString As New StringBuilder()
			Dim response As New HttpResponseMessage

			Dim data = Task.Run(Function() m_PVLUtility.WebserviceResponse(Nothing, baseUri, "GET", m_UserName, m_Password))
			data.Wait()

			response = data.Result
			If response Is Nothing Then Return Nothing

			Dim myStreamReader As New StreamReader(response.Content.ReadAsStreamAsync().Result)
			Dim responseData = myStreamReader.ReadToEnd

			If response.StatusCode <> 200 Then
				m_Logger.LogWarning(String.Format("LoadTempDataPublicationNewsData: {0}", response.StatusCode))

				SplashScreenManager.CloseForm(False)
				m_ErrorResult = m_JsonUtility.ParseJSonError(responseData)

				Return Nothing
			End If
			result = m_JsonUtility.ParsePublicationNewsResultJSonResult(responseData)
			result = result.OrderByDescending(Function(x) x.PublicationDate).ToList

			If (result Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("News Daten konnten nicht geladen werden."))

				Return Nothing
			End If


			Return result

		End Function

		Private Function LoadMergedPublicationNewsData(ByVal hideReadNews As Boolean) As BindingList(Of PublicationNewsViewData)
			Dim listDataSource As BindingList(Of PublicationNewsViewData) = New BindingList(Of PublicationNewsViewData)

			Try
				Dim tempdataPublicationData = LoadTempDataPublicationNewsData()

				If (tempdataPublicationData Is Nothing) Then
					m_Logger.LogWarning("publications could not be downloaded!")

					Return Nothing
				End If

				Dim customerGAVData = m_CustomerDatabaseAccess.LoadAllCustomerGAVGroupData(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserKST)
				If customerGAVData Is Nothing OrElse customerGAVData.Count = 0 Then Return Nothing

				Dim customerMatchedGAVData As BindingList(Of SPGAV.TempData.PublicationNewsData) = New BindingList(Of SPGAV.TempData.PublicationNewsData)

				For Each news In tempdataPublicationData
					Dim assignedGAV = customerGAVData.Where(Function(x) x.GAVNUmber = news.ContractNumber).FirstOrDefault

					If Not assignedGAV Is Nothing Then customerMatchedGAVData.Add(news)

				Next

				If customerMatchedGAVData Is Nothing OrElse customerMatchedGAVData.Count = 0 Then Return Nothing

				Dim gridData = (From person In customerMatchedGAVData
								Select New PublicationNewsViewData With {.Content = person.Content,
									.ContractNumber = person.ContractNumber,
									.PublicationDate = person.PublicationDate,
									.Title = person.Title,
									.VersionNumber = person.VersionNumber
									}).ToList()

				Dim publicationViewData = PerformLoadPVLPublicationViewDataWebservice()
				Dim viewedCount As Integer = 0
				Dim unViewedCount As Integer = 0

				For Each p In gridData
					If Not publicationViewData Is Nothing AndAlso publicationViewData.Count > 0 Then
						Dim viewData = publicationViewData.Where(Function(x) x.ContractNumber = p.ContractNumber And x.VersionNumber = p.VersionNumber).FirstOrDefault

						If Not viewData Is Nothing Then
							If p.PublicationDate <> viewData.PublicationDate Then
								p.Viewed = False
							Else
								p.Viewed = viewData.Viewed.GetValueOrDefault(False)
							End If
							p.ID = viewData.ID

						Else
							p.Viewed = False

						End If

					End If

					If p.Viewed.GetValueOrDefault(False) Then
						viewedCount += 1
					Else
						unViewedCount += 1
					End If
					If hideReadNews Then
						If Not p.Viewed.GetValueOrDefault(False) Then listDataSource.Add(p)
					Else
						listDataSource.Add(p)
					End If

				Next

				lblCountOfNews.Text = String.Format(m_Translate.GetSafeTranslationValue("Total Publikation: {0}<br>Gelesen: {1}<br>Ungelesen: {2}"), tempdataPublicationData.Count, viewedCount, unViewedCount)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				Return Nothing

			End Try

			Return listDataSource
		End Function

		Private Function PerformLoadPVLPublicationViewDataWebservice() As BindingList(Of PVLPublicationViewDataDTO)

			Dim listDataSource As BindingList(Of PVLPublicationViewDataDTO) = New BindingList(Of PVLPublicationViewDataDTO)

			Dim webservice = New SPPVLGAVUtilWebService.SPPVLGAVUtilSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_SPPVLUtilitiesServiceUrl)

			Try

				' Read data over webservice
				Dim searchResult = webservice.LoadPublicationInfoData(m_InitializationData.MDData.MDGuid, m_InitializationData.UserData.UserGuid).ToList

				Dim pvlGridData = (From Result In searchResult
								   Select New PVLPublicationViewDataDTO With
							 {
								.ID = Result.ID,
								.Customer_ID = Result.Customer_ID,
								.User_ID = Result.User_ID,
								.ContractNumber = Result.ContractNumber,
								.VersionNumber = Result.VersionNumber,
								.PublicationDate = Result.PublicationDate,
								.Title = Result.Title,
								.Content = Result.Content,
								.CreatedOn = Result.CreatedOn,
								.Viewed = Result.Viewed
							 }).ToList()


				For Each p In pvlGridData
					listDataSource.Add(p)
				Next

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				Return Nothing

			End Try

			Return listDataSource
		End Function

		Private Function PerformUpdatePVLPublicationViewDataWebservice(ByVal updateData As PublicationNewsViewData) As Boolean
			Dim success As Boolean? = True

			Dim webservice = New SPPVLGAVUtilWebService.SPPVLGAVUtilSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_SPPVLUtilitiesServiceUrl)

			Try
				' Read data over webservice
				success = webservice.UpdateViewedPublicationData(m_InitializationData.MDData.MDGuid,
																		  m_InitializationData.UserData.UserGuid,
																		  updateData.ID.GetValueOrDefault(0),
																		  updateData.ContractNumber,
																		  updateData.VersionNumber,
																		  updateData.PublicationDate,
																		  updateData.Title,
																		  updateData.Viewed,
																		  m_InitializationData.UserData.UserFullName)


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				Return Nothing

			End Try

			Return success
		End Function

		Private Sub OnGVPublicationNews_UpdateButtonClick(sender As Object, e As ButtonPressedEventArgs)
			Dim result As Boolean = True

			Dim updateData = SelectedPublicationNewsData
			If updateData Is Nothing Then Return

			updateData.Viewed = Not updateData.Viewed.GetValueOrDefault(False)
			result = result AndAlso PerformUpdatePVLPublicationViewDataWebservice(updateData)
			If Not result Then Return

			Dim listDataSource = LoadMergedPublicationNewsData(tgsHideReadNews.EditValue)
			grdPublicationNews.DataSource = listDataSource

		End Sub

		Private Sub tgsHideReadNews_Toggled(sender As Object, e As EventArgs) Handles tgsHideReadNews.Toggled
			If m_SuppressUIEvents Then Return

			Dim listDataSource = LoadMergedPublicationNewsData(tgsHideReadNews.EditValue)
			grdPublicationNews.DataSource = listDataSource

		End Sub

		Private Sub Onfrm_Load(sender As Object, e As EventArgs) Handles Me.Load

			Try
				If My.Settings.NewsHeight > 0 Then Me.Height = Math.Max(My.Settings.NewsHeight, Me.Height)
				If My.Settings.NewsWidth > 0 Then Me.Width = Math.Max(My.Settings.NewsWidth, Me.Width)

				If Not String.IsNullOrWhiteSpace(My.Settings.NewsLocation) Then
					Dim aLoc As String() = My.Settings.NewsLocation.Split(CChar(";"))
					If Screen.AllScreens.Length = 1 Then
						If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
					End If
					Me.Location = New System.Drawing.Point(Val(aLoc(0)), Val(aLoc(1)))
				End If

			Catch ex As Exception

			End Try

		End Sub

		Private Sub Onfrm_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

			If Me.WindowState = FormWindowState.Minimized Then Return

			My.Settings.NewsLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
			My.Settings.NewsHeight = Me.Height
			My.Settings.NewsWidth = Me.Width

			My.Settings.Save()

		End Sub



#Region "helpers"

		Private Function IsCustomerServiceAllowed(ByVal serviceName As String) As Boolean
			Dim providerObj As New ProviderData(m_InitializationData)
			Dim result = providerObj.IsCustomerAllowedToUseServiceData(m_InitializationData.MDData.MDGuid, serviceName)

			Return result

		End Function



#End Region

	End Class


	Module DemoHelper
		Function GetDeleteImage() As Image
			Return GetImage(Brushes.Red)
		End Function

		Function GetEditImage() As Image
			Return GetImage(Brushes.Green)
		End Function

		Function GetImage(ByVal b As Brush) As Image
			Dim img As Image = New Bitmap(16, 16)

			Using g As Graphics = Graphics.FromImage(img)
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias
				g.FillEllipse(b, New Rectangle(0, 0, img.Width - 1, img.Height - 1))
			End Using

			Return img
		End Function
	End Module


End Namespace
