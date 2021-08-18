
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

Imports DevExpress.Utils
Imports DevExpress.XtraSplashScreen
Imports SP.Internal.Automations

Namespace UI

	Public Class frmTempDataPVL


#Region "Constants"

		Private Const JOBROOM_USER As String = "TempData-Api-Key"
		Private Const JOBROOM_PASSWORD As String = "v4k78GdE4s2a"
		Private Const DEFAULT_LANGUAGE As String = "de-ch"
		Private Const SERVICENAME As String = "TEMPDATAAPI"

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

		Private Const JOBROOM_MINDESTLOHN_RESULT_URI As String = "{0}minimumsalary/{2}/result?language={1}&roundedResults={3}"

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

		'Private m_LOData As GAVCalculationDTO
		'Private liLOData As New List(Of String)

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

		Private m_CurrentPVLHourData As GAVNameResultDTO
		'Private m_CurrentPVLData As GAVNameResultDTO
		'Private m_CategoryData As BindingList(Of GAVCategoryDTO)
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


		Private m_ResultContent As String

		Private m_UserName As String
		Private m_Password As String
		Private m_JobroomURI As String
		Private m_JobroomAllRecordURI As String
		Private m_JobroomSingleRecordURI As String
		Private m_Language As String
		Private m_ErrorResult As ResponseErrorData
		Private m_FirstURI As String

		Private m_SalaryCriteriasData As List(Of MindestLohnInputCriteriasData)
		Private m_SalaryData As MindestLohnResultData
		Private m_CantonID As Integer?
		Private m_AlterID As Integer?
		Private m_YearID As Integer?
		Private m_cantonValue As String


		Private m_FirstHandle As IOverlaySplashScreenHandle
		Private m_SecHandle As IOverlaySplashScreenHandle
		Private m_ThirdHandle As IOverlaySplashScreenHandle
		Private m_FirstCall As Boolean
		Private m_AllowedServicetoUse As Boolean

#End Region


#Region "Public Properties"

		Public Property EmployeeNumber As Integer
		Public Property CustomerNumber As Integer
		Public Property EmploymentNumber As Integer
		Public Property CustomerCanton As String
		Public Property ExistingGAVInfo As String
		Public Property Staging As Boolean


		Public ReadOnly Property GetAssignedPVLData As String
			Get
				Return m_PVLSelectedData
			End Get
		End Property

#End Region


#Region "private properties"

		Private ReadOnly Property SelectedBranchesData As BranchesData
			Get
				Dim SelectedData = TryCast(lueBranches.GetSelectedDataRow(), BranchesData)

				Return SelectedData
			End Get

		End Property

		Private ReadOnly Property SelectedPVLData As ContractData
			Get
				Dim SelectedData = TryCast(lueContracts.GetSelectedDataRow(), ContractData)

				Return SelectedData
			End Get

		End Property

		Private ReadOnly Property SelectedPVLEditionData As ContractVersionEditionData
			Get
				Dim SelectedData = TryCast(lueContractEditions.GetSelectedDataRow(), ContractVersionEditionData)

				Return SelectedData
			End Get

		End Property

		Private ReadOnly Property SelectedContractDocumentData As ContractEditionDocumentData
			Get
				Dim gvData = TryCast(grdContractDocument.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (gvData Is Nothing) Then

					Dim selectedRows = gvData.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim employee = CType(gvData.GetRow(selectedRows(0)), ContractEditionDocumentData)

						Return employee
					End If

				End If

				Return Nothing
			End Get

		End Property

		Private ReadOnly Property SelectedLinksData As ContractEditionLinksData
			Get
				Dim gvData = TryCast(grdLinks.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (gvData Is Nothing) Then

					Dim selectedRows = gvData.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim employee = CType(gvData.GetRow(selectedRows(0)), ContractEditionLinksData)

						Return employee
					End If

				End If

				Return Nothing
			End Get

		End Property


#Region "categories"

		Private ReadOnly Property SelectedAssignedPVLCategoryData(ByVal id As Integer) As InputValueData
			Get
				Dim SelectedData As InputValueData

				Select Case id
					Case 0
						SelectedData = TryCast(lue0.GetSelectedDataRow(), InputValueData)
					Case 1
						SelectedData = TryCast(lue1.GetSelectedDataRow(), InputValueData)
					Case 2
						SelectedData = TryCast(lue2.GetSelectedDataRow(), InputValueData)
					Case 3
						SelectedData = TryCast(lue3.GetSelectedDataRow(), InputValueData)
					Case 4
						SelectedData = TryCast(lue4.GetSelectedDataRow(), InputValueData)
					Case 5
						SelectedData = TryCast(lue5.GetSelectedDataRow(), InputValueData)
					Case 6
						SelectedData = TryCast(lue6.GetSelectedDataRow(), InputValueData)
					Case 7
						SelectedData = TryCast(lue7.GetSelectedDataRow(), InputValueData)
					Case 8
						SelectedData = TryCast(lue8.GetSelectedDataRow(), InputValueData)
					Case 9
						SelectedData = TryCast(lue9.GetSelectedDataRow(), InputValueData)
					Case 10
						SelectedData = TryCast(lue10.GetSelectedDataRow(), InputValueData)
					Case 11
						SelectedData = TryCast(lue11.GetSelectedDataRow(), InputValueData)
					Case 12
						SelectedData = TryCast(lue12.GetSelectedDataRow(), InputValueData)


					Case Else
						Return Nothing

				End Select

				Return SelectedData
			End Get

		End Property


#End Region


		''' <summary>
		''' Gets the flexible time from database setting
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private ReadOnly Property GetflexibletimeFromDatabase As Boolean
			Get

				Dim FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"

				Dim value As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_md.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr), String.Format("{0}/getflextimefrommandantdatabase", FORM_XML_MAIN_KEY)), False)

				Return value

			End Get
		End Property

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
			m_JsonUtility = New JsonUtility
			m_HtmlUtil = New HTMLUtiles.Utilities
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

			InitialCommonControls()
			LoadLanguageDrowpDownData()

			m_SuppressUIEvents = False

			AddHandler gvSalaryDetail.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground
			AddHandler gvAdditionalText.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground

		End Sub


#End Region

#Region "public methodes"

		Public Function LoadData() As Boolean
			Dim result As Boolean = True

			If Not m_AllowedServicetoUse Then
				m_Logger.LogWarning(String.Format("{0} modul is not allowed to use!!!", SERVICENAME))

				Return False
			End If

			Dim suppressUIEventsState = m_SuppressUIEvents

			m_SuppressUIEvents = True
			Staging = False

			SplashScreenManager.CloseForm(False)
			SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
			SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Vorlage wird übersetzt") & Space(20))
			SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

			If Staging Then
				m_FirstURI = STAGING_JOBROOM_URI
			Else
				m_FirstURI = JOBROOM_URI
			End If

			If EmploymentNumber > 0 Then
				LoadEmploymentData()
				lueBranches.Enabled = False
				lueContracts.Enabled = False
			End If
			m_FirstCall = True
			LoadEmployeeData()
			LoadCustomerData()
			m_ExistingCustomerGAVData = m_CustomerDatabaseAccess.LoadAssignedGAVGroupDataOfCustomer(CustomerNumber)

			LoadKeywordsData()
			LoadBranchesData()
			LoadAllPVLContractData()


			m_SuppressUIEvents = suppressUIEventsState

			Try
				If Not m_ContractData Is Nothing AndAlso m_ContractData.Count = 1 Then
					lueContracts.EditValue = m_ContractData(0).ContractNumber
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0} >>> {1}", m_ContractData(0).ContractNumber, ex.ToString))
			End Try

			SplashScreenManager.CloseForm(False)

			Return result
		End Function

		Public Function LoadPVLContractData() As List(Of ContractData)
			Dim suppressUIEventsState = m_SuppressUIEvents

			If Not m_AllowedServicetoUse Then
				m_Logger.LogWarning(String.Format("{0} modul is not allowed to use!!!", SERVICENAME))

				Return Nothing
			End If

			m_SuppressUIEvents = True

			SplashScreenManager.CloseForm(False)
			SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
			SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Nach Daten wird gesucht") & Space(20))
			SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

			Try
				m_FirstURI = JOBROOM_URI
				'If m_InitializationData.UserData.UserLanguage = "D" Then m_Language = "de-CH"
				'If m_InitializationData.UserData.UserLanguage = "F" Then m_Language = "fr-CH"
				'If m_InitializationData.UserData.UserLanguage = "I" Then m_Language = "it-CH"

				Dim result = LoadAllPVLContractData()


				Return result

			Catch ex As Exception
				Return Nothing

			Finally
				SplashScreenManager.CloseForm(False)
				m_SuppressUIEvents = suppressUIEventsState
			End Try

			Return Nothing
		End Function



#End Region

		Private Sub frmTempDataPVL_Activated(sender As Object, e As EventArgs) Handles Me.Activated

			If m_FirstCall Then
				m_FirstCall = False
				If Not lue0.EditValue Is Nothing Then
					Dim value As Integer = lue0.EditValue

					Dim data = SelectedAssignedPVLCategoryData(0)
					If data Is Nothing Then
						Dim lueData As List(Of InputValueData) = lue0.Properties.DataSource
						data = lueData.Where(Function(x) x.CriteriaListEntryID = value).FirstOrDefault
					End If
					If Not data Is Nothing Then
						CreateSubCategory_Staging(data, 0)
					End If
				End If
			End If

			'ShowPublicationNewsDetail()
		End Sub

		Private Sub InitialCommonControls()

			AddHandler lueKeywords.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueLanguage.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueBranches.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueContracts.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueContractVersions.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueContractEditions.ButtonClick, AddressOf OnDropDown_ButtonClick

			AddHandler gvContractDocument.RowCellClick, AddressOf OngvDocuments_RowCellClick
			AddHandler gvLinks.RowCellClick, AddressOf OngvLinks_RowCellClick

		End Sub

		Private Sub OnDropDown_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

			Const ID_OF_DELETE_BUTTON As Int32 = 1

			' If delete button has been clicked reset the drop down.
			If e.Button.Index = ID_OF_DELETE_BUTTON Then

				If TypeOf sender Is LookUpEdit Then
					Dim lookupEdit As LookUpEdit = CType(sender, LookUpEdit)
					lookupEdit.EditValue = Nothing
				ElseIf TypeOf sender Is GridLookUpEdit Then
					Dim dateEdit As GridLookUpEdit = CType(sender, GridLookUpEdit)
					dateEdit.EditValue = Nothing
				End If
			End If
		End Sub

		Sub OngvDocuments_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

			If (e.Clicks = 2) Then
				Dim data = SelectedContractDocumentData
				If data Is Nothing Then Return

				LoadAssignedContractDocumentFileIDData(data.FileID)
			End If

		End Sub

		Sub OngvLinks_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

			If (e.Clicks = 2) Then
				Dim data = SelectedLinksData
				If data Is Nothing Then Return

				Process.Start(data.URL)
			End If

		End Sub


#Region "reset"

		Private Sub Reset()

			flpBodyContentToLoadSalary.OptionsButtonPanel.ShowButtonPanel = True

			grpGAVCategory.CustomHeaderButtons(0).Properties.Enabled = m_InitializationData.UserData.UserNr = 1

			ResetLanguageDropDown()
			ResetKeywrodsDropDown()
			ResetBranchesDropDown()

			ResetContractsDropDown()
			ResetContractVersionDropDown()
			ResetContractEditionsDropDown()

			ResetContractEditionsDocumentGridView()
			ResetContractEditionsLinksGridView()

			ResetGridMindestLohn()
			ResetAdditionalTextFields()


			m_PVLSelectedData = String.Empty
			m_Language = DEFAULT_LANGUAGE

		End Sub

		Private Sub ResetLanguageDropDown()

			lueLanguage.Properties.DisplayMember = "Value"
			lueLanguage.Properties.ValueMember = "Value"

			gvLanguage.OptionsView.ShowIndicator = False
			gvLanguage.OptionsView.ShowColumnHeaders = True
			gvLanguage.OptionsView.ShowFooter = False

			gvLanguage.OptionsView.ShowAutoFilterRow = True
			gvLanguage.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvLanguage.Columns.Clear()

			Dim columnLanguage As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLanguage.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnLanguage.Caption = m_Translate.GetSafeTranslationValue("Sprache")
			columnLanguage.Name = "Value"
			columnLanguage.FieldName = "Value"
			columnLanguage.Visible = True
			gvLanguage.Columns.Add(columnLanguage)

			lueLanguage.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueLanguage.Properties.NullText = String.Empty
			lueLanguage.EditValue = Nothing

		End Sub

		Private Sub ResetKeywrodsDropDown()

			lueKeywords.Properties.DisplayMember = "Value"
			lueKeywords.Properties.ValueMember = "Value"

			gvKeywords.OptionsView.ShowIndicator = False
			gvKeywords.OptionsView.ShowColumnHeaders = True
			gvKeywords.OptionsView.ShowFooter = False

			gvKeywords.OptionsView.ShowAutoFilterRow = True
			gvKeywords.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvKeywords.Columns.Clear()

			Dim columngav_number As New DevExpress.XtraGrid.Columns.GridColumn()
			columngav_number.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columngav_number.Caption = m_Translate.GetSafeTranslationValue("Keywords")
			columngav_number.Name = "Value"
			columngav_number.FieldName = "Value"
			columngav_number.Visible = True
			gvKeywords.Columns.Add(columngav_number)

			lueKeywords.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueKeywords.Properties.NullText = String.Empty
			lueKeywords.EditValue = Nothing

		End Sub

		Private Sub ResetBranchesDropDown()

			lueBranches.Properties.DisplayMember = "Branch"
			lueBranches.Properties.ValueMember = "Branch"

			gvBranches.OptionsView.ShowIndicator = False
			gvBranches.OptionsView.ShowColumnHeaders = True
			gvBranches.OptionsView.ShowFooter = False

			gvBranches.OptionsView.ShowAutoFilterRow = True
			gvBranches.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvBranches.Columns.Clear()

			Dim columnBranch As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBranch.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnBranch.Caption = m_Translate.GetSafeTranslationValue("Branche")
			columnBranch.Name = "Branch"
			columnBranch.FieldName = "Branch"
			columnBranch.Visible = True
			gvBranches.Columns.Add(columnBranch)


			lueBranches.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueBranches.Properties.NullText = String.Empty
			lueBranches.EditValue = Nothing

		End Sub

		Private Sub ResetContractsDropDown()

			lueContracts.Properties.DisplayMember = "ContractViewData"
			lueContracts.Properties.ValueMember = "ContractNumber"

			gvContracts.OptionsView.ShowIndicator = False
			gvContracts.OptionsView.ShowColumnHeaders = True
			gvContracts.OptionsView.ShowFooter = False

			gvContracts.OptionsView.ShowAutoFilterRow = True
			gvContracts.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvContracts.Columns.Clear()

			Dim columngav_number As New DevExpress.XtraGrid.Columns.GridColumn()
			columngav_number.Caption = m_Translate.GetSafeTranslationValue("Nr")
			columngav_number.Name = "ContractNumber"
			columngav_number.FieldName = "ContractNumber"
			columngav_number.Visible = False
			columngav_number.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvContracts.Columns.Add(columngav_number)

			Dim columnname_de As New DevExpress.XtraGrid.Columns.GridColumn()
			columnname_de.Caption = m_Translate.GetSafeTranslationValue("Name")
			columnname_de.Name = "ContractViewData"
			columnname_de.FieldName = "ContractViewData"
			columnname_de.Visible = True
			columnname_de.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvContracts.Columns.Add(columnname_de)

			lueContracts.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueContracts.Properties.NullText = String.Empty
			lueContracts.EditValue = Nothing

		End Sub

		Private Sub ResetContractVersionDropDown()

			lueContractVersions.Properties.DisplayMember = "ContractVersionViewData"
			lueContractVersions.Properties.ValueMember = "Number"

			gvContractVersions.OptionsView.ShowIndicator = False
			gvContractVersions.OptionsView.ShowColumnHeaders = True
			gvContractVersions.OptionsView.ShowFooter = False

			gvContractVersions.OptionsView.ShowAutoFilterRow = True
			gvContractVersions.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvContractVersions.Columns.Clear()

			Dim columngav_number As New DevExpress.XtraGrid.Columns.GridColumn()
			columngav_number.Caption = m_Translate.GetSafeTranslationValue("Nr")
			columngav_number.Name = "Number"
			columngav_number.FieldName = "Number"
			columngav_number.Visible = False
			columngav_number.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvContractVersions.Columns.Add(columngav_number)

			Dim columnname_de As New DevExpress.XtraGrid.Columns.GridColumn()
			columnname_de.Caption = m_Translate.GetSafeTranslationValue("Name")
			columnname_de.Name = "ContractVersionViewData"
			columnname_de.FieldName = "ContractVersionViewData"
			columnname_de.Visible = True
			columnname_de.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvContractVersions.Columns.Add(columnname_de)

			lueContractVersions.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueContractVersions.Properties.NullText = String.Empty
			lueContractVersions.EditValue = Nothing

		End Sub

		Private Sub ResetContractEditionsDropDown()

			lueContractEditions.Properties.DisplayMember = "ContractVersionEditionViewData"
			lueContractEditions.Properties.ValueMember = "ID"

			gvContractEditions.OptionsView.ShowIndicator = False
			gvContractEditions.OptionsView.ShowColumnHeaders = True
			gvContractEditions.OptionsView.ShowFooter = False

			gvContractEditions.OptionsView.ShowAutoFilterRow = True
			gvContractEditions.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvContractEditions.Columns.Clear()

			Dim columngav_number As New DevExpress.XtraGrid.Columns.GridColumn()
			columngav_number.Caption = m_Translate.GetSafeTranslationValue("ID")
			columngav_number.Name = "ID"
			columngav_number.FieldName = "ID" ' "PVLEditionID"
			columngav_number.Visible = False
			columngav_number.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvContractEditions.Columns.Add(columngav_number)

			Dim columnname_de As New DevExpress.XtraGrid.Columns.GridColumn()
			columnname_de.Caption = m_Translate.GetSafeTranslationValue("Name")
			columnname_de.Name = "ContractVersionEditionViewData"
			columnname_de.FieldName = "ContractVersionEditionViewData"
			columnname_de.Visible = True
			columnname_de.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvContractEditions.Columns.Add(columnname_de)

			lueContractEditions.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueContractEditions.Properties.NullText = String.Empty
			lueContractEditions.EditValue = Nothing

		End Sub

		Private Sub ResetContractEditionsDocumentGridView()

			gvContractDocument.OptionsView.ShowIndicator = False
			gvContractDocument.OptionsView.ShowAutoFilterRow = False
			gvContractDocument.OptionsView.ShowColumnHeaders = False
			gvContractDocument.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvContractDocument.OptionsView.ShowFooter = False
			gvContractDocument.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
			gvContractDocument.OptionsView.ShowDetailButtons = False
			gvContractDocument.Columns.Clear()


			Dim repoHTML = New RepositoryItemHypertextLabel
			repoHTML.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True
			repoHTML.Appearance.TextOptions.WordWrap = DevExpress.Utils.DefaultBoolean.True
			repoHTML.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
			repoHTML.Appearance.Options.UseTextOptions = True
			grdContractDocument.RepositoryItems.Add(repoHTML)

			Dim richHtml As RepositoryItemRichTextEdit = New RepositoryItemRichTextEdit
			richHtml.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True
			richHtml.DocumentFormat = DocumentFormat.Html
			richHtml.Appearance.TextOptions.WordWrap = DevExpress.Utils.DefaultBoolean.True
			richHtml.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
			richHtml.Appearance.Options.UseTextOptions = True
			grdContractDocument.RepositoryItems.Add(richHtml)


			Dim columnProfileID As New DevExpress.XtraGrid.Columns.GridColumn()
			columnProfileID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnProfileID.OptionsColumn.AllowEdit = False
			columnProfileID.Caption = "FileID"
			columnProfileID.Name = "FileID"
			columnProfileID.FieldName = "FileID"
			columnProfileID.Visible = False
			columnProfileID.Width = 50
			gvContractDocument.Columns.Add(columnProfileID)

			Dim columnDateFromToViewData As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDateFromToViewData.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDateFromToViewData.OptionsColumn.AllowEdit = False
			columnDateFromToViewData.Caption = "Name"
			columnDateFromToViewData.Name = "ContractEditionDocumentViewData"
			columnDateFromToViewData.FieldName = "ContractEditionDocumentViewData"
			columnDateFromToViewData.ColumnEdit = repoHTML

			columnDateFromToViewData.Visible = True
			columnDateFromToViewData.Width = 100
			gvContractDocument.Columns.Add(columnDateFromToViewData)


			grdContractDocument.DataSource = Nothing

		End Sub

		Private Sub ResetContractEditionsLinksGridView()

			gvLinks.OptionsView.ShowIndicator = False
			gvLinks.OptionsView.ShowAutoFilterRow = False
			gvLinks.OptionsView.ShowColumnHeaders = False
			gvLinks.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvLinks.OptionsView.ShowFooter = False
			gvLinks.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
			gvLinks.OptionsView.ShowDetailButtons = False
			gvLinks.Columns.Clear()


			Dim repoHTML = New RepositoryItemHypertextLabel
			repoHTML.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True
			repoHTML.Appearance.TextOptions.WordWrap = DevExpress.Utils.DefaultBoolean.True
			repoHTML.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
			repoHTML.Appearance.Options.UseTextOptions = True
			grdLinks.RepositoryItems.Add(repoHTML)

			Dim richHtml As RepositoryItemRichTextEdit = New RepositoryItemRichTextEdit
			richHtml.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True
			richHtml.DocumentFormat = DocumentFormat.Html
			richHtml.Appearance.TextOptions.WordWrap = DevExpress.Utils.DefaultBoolean.True
			richHtml.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
			richHtml.Appearance.Options.UseTextOptions = True
			grdLinks.RepositoryItems.Add(richHtml)


			Dim columnProfileID As New DevExpress.XtraGrid.Columns.GridColumn()
			columnProfileID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnProfileID.OptionsColumn.AllowEdit = False
			columnProfileID.Caption = "URL"
			columnProfileID.Name = "URL"
			columnProfileID.FieldName = "URL"
			columnProfileID.Visible = False
			columnProfileID.Width = 50
			gvLinks.Columns.Add(columnProfileID)

			Dim columnDateFromToViewData As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDateFromToViewData.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDateFromToViewData.OptionsColumn.AllowEdit = False
			columnDateFromToViewData.Caption = "Description"
			columnDateFromToViewData.Name = "Description"
			columnDateFromToViewData.FieldName = "Description"
			columnDateFromToViewData.ColumnEdit = repoHTML

			columnDateFromToViewData.Visible = True
			columnDateFromToViewData.Width = 100
			gvLinks.Columns.Add(columnDateFromToViewData)


			grdLinks.DataSource = Nothing

		End Sub


		Private Sub ResetAdditionalTextFields()


			gvAdditionalText.OptionsView.ShowIndicator = False
			gvAdditionalText.OptionsView.ShowAutoFilterRow = False
			gvAdditionalText.OptionsView.ShowColumnHeaders = False
			gvAdditionalText.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvAdditionalText.OptionsView.ShowFooter = False
			gvAdditionalText.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
			gvAdditionalText.OptionsView.ShowDetailButtons = False
			gvAdditionalText.OptionsView.AutoCalcPreviewLineCount = True
			gvAdditionalText.OptionsView.RowAutoHeight = True


			gvAdditionalText.Columns.Clear()


			Dim repoHTML = New RepositoryItemHypertextLabel
			repoHTML.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True
			repoHTML.Appearance.TextOptions.WordWrap = DevExpress.Utils.DefaultBoolean.True
			repoHTML.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
			repoHTML.Appearance.Options.UseTextOptions = True
			grdAdditionalText.RepositoryItems.Add(repoHTML)

			Dim richHtml As RepositoryItemRichTextEdit = New RepositoryItemRichTextEdit
			richHtml.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True
			richHtml.DocumentFormat = DocumentFormat.Html
			richHtml.Appearance.TextOptions.WordWrap = DevExpress.Utils.DefaultBoolean.True
			richHtml.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
			richHtml.Appearance.Options.UseTextOptions = True
			grdAdditionalText.RepositoryItems.Add(richHtml)


			Dim columnVariabelnText As New DevExpress.XtraGrid.Columns.GridColumn()
			columnVariabelnText.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnVariabelnText.OptionsColumn.AllowEdit = False
			columnVariabelnText.Caption = ""
			columnVariabelnText.Name = "VariabelnText"
			columnVariabelnText.FieldName = "VariabelnText"
			columnVariabelnText.ColumnEdit = repoHTML
			columnVariabelnText.Visible = True
			columnVariabelnText.Width = 100
			gvAdditionalText.Columns.Add(columnVariabelnText)

			'gvAdditionalText.Columns("VariabelnText").Visible = False
			'gvAdditionalText.PreviewFieldName = "VariabelnText"
			'gvAdditionalText.OptionsView.ShowPreview = True


			grdAdditionalText.DataSource = Nothing

		End Sub

		Private Sub ResetGridMindestLohn()

			gvSalaryDetail.OptionsView.ShowIndicator = False
			gvSalaryDetail.OptionsView.ShowColumnHeaders = False
			gvSalaryDetail.OptionsView.ShowAutoFilterRow = False
			gvSalaryDetail.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvSalaryDetail.OptionsView.ShowFooter = False
			gvSalaryDetail.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
			gvSalaryDetail.OptionsView.ShowDetailButtons = False
			gvSalaryDetail.OptionsView.AutoCalcPreviewLineCount = True

			gvSalaryDetail.Columns.Clear()


			Dim repoHTML = New RepositoryItemHypertextLabel
			repoHTML.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True
			repoHTML.Appearance.TextOptions.WordWrap = DevExpress.Utils.DefaultBoolean.True
			repoHTML.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
			repoHTML.Appearance.Options.UseTextOptions = True
			grdSalaryDetail.RepositoryItems.Add(repoHTML)

			Dim richHtml As RepositoryItemRichTextEdit = New RepositoryItemRichTextEdit
			richHtml.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True
			richHtml.DocumentFormat = DocumentFormat.Html
			richHtml.Appearance.TextOptions.WordWrap = DevExpress.Utils.DefaultBoolean.True
			richHtml.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
			richHtml.Appearance.Options.UseTextOptions = True
			grdSalaryDetail.RepositoryItems.Add(richHtml)

			Dim repoMemoHTML = New RepositoryItemMemoExEdit
			repoMemoHTML.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True
			repoMemoHTML.Appearance.TextOptions.WordWrap = DevExpress.Utils.DefaultBoolean.True
			repoMemoHTML.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
			repoMemoHTML.Appearance.Options.UseTextOptions = True
			grdSalaryDetail.RepositoryItems.Add(repoMemoHTML)


			Dim columnProfileID As New DevExpress.XtraGrid.Columns.GridColumn()
			columnProfileID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnProfileID.OptionsColumn.AllowEdit = False
			columnProfileID.Caption = "" ' m_Translate.GetSafeTranslationValue("Label")
			columnProfileID.Name = "Label"
			columnProfileID.FieldName = "Label"
			columnProfileID.Visible = True
			columnProfileID.Width = 50
			gvSalaryDetail.Columns.Add(columnProfileID)

			Dim columnDateFromToViewData As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDateFromToViewData.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDateFromToViewData.OptionsColumn.AllowEdit = False
			columnDateFromToViewData.Caption = ""
			columnDateFromToViewData.Name = "VariabelnText"
			columnDateFromToViewData.FieldName = "VariabelnText"
			columnDateFromToViewData.ColumnEdit = repoHTML
			columnDateFromToViewData.Visible = True
			columnDateFromToViewData.Width = 10
			gvSalaryDetail.Columns.Add(columnDateFromToViewData)

			Dim columnUnit As New DevExpress.XtraGrid.Columns.GridColumn()
			columnUnit.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnUnit.OptionsColumn.AllowEdit = False
			columnUnit.Caption = ""
			columnUnit.Name = "Unit"
			columnUnit.FieldName = "Unit"
			columnUnit.Visible = True
			columnUnit.Width = 10
			gvSalaryDetail.Columns.Add(columnUnit)

			Dim columnAmount As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAmount.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnAmount.OptionsColumn.AllowEdit = False
			columnAmount.Caption = m_Translate.GetSafeTranslationValue("Betrag")
			columnAmount.Name = "Amount"
			columnAmount.FieldName = "Amount"
			columnAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnAmount.DisplayFormat.FormatString = "0.###"
			columnAmount.Visible = True
			columnAmount.Width = 10
			gvSalaryDetail.Columns.Add(columnAmount)


			gvSalaryDetail.Columns("VariabelnText").Visible = False
			gvSalaryDetail.PreviewFieldName = "VariabelnText"
			gvSalaryDetail.OptionsView.ShowPreview = True
			'gvGavDetail.PreviewLineCount = 3


			grdSalaryDetail.DataSource = Nothing

		End Sub



#End Region

		Private Sub TranslateControls()

			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

			lblKandidat.Text = m_Translate.GetSafeTranslationValue(lblKandidat.Text)
			lblKunde.Text = m_Translate.GetSafeTranslationValue(lblKunde.Text)
			lblEinsatz.Text = m_Translate.GetSafeTranslationValue(lblEinsatz.Text)
			lblGeburtsdatum.Text = m_Translate.GetSafeTranslationValue(lblGeburtsdatum.Text)
			lblQualifikation.Text = m_Translate.GetSafeTranslationValue(lblQualifikation.Text)
			chkAlter.Text = m_Translate.GetSafeTranslationValue(chkAlter.Text)
			lblLanguage.Text = m_Translate.GetSafeTranslationValue(lblLanguage.Text)

			lblBranches.Text = m_Translate.GetSafeTranslationValue(lblBranches.Text)
			lblgavBeruf.Text = m_Translate.GetSafeTranslationValue(lblgavBeruf.Text)
			lblVersion.Text = m_Translate.GetSafeTranslationValue(lblVersion.Text)
			lblEdition.Text = m_Translate.GetSafeTranslationValue(lblEdition.Text)
			lblKWStd.Text = m_Translate.GetSafeTranslationValue(lblKWStd.Text)
			lblLinkToPVLData.Text = m_Translate.GetSafeTranslationValue(lblLinkToPVLData.Text)

			tnpGAV.Caption = m_Translate.GetSafeTranslationValue(tnpGAV.Caption)
			tnpDocuments.Caption = m_Translate.GetSafeTranslationValue(tnpDocuments.Caption)
			tnpLinks.Caption = m_Translate.GetSafeTranslationValue(tnpLinks.Caption)

			grpGAVCategory.Text = m_Translate.GetSafeTranslationValue(grpGAVCategory.Text)
			grpGAVCategory.CustomHeaderButtons(0).Properties.Caption = m_Translate.GetSafeTranslationValue(grpGAVCategory.CustomHeaderButtons(0).Properties.Caption)
			grpGAVCategory.CustomHeaderButtons(1).Properties.Caption = m_Translate.GetSafeTranslationValue(grpGAVCategory.CustomHeaderButtons(1).Properties.Caption)
			grpSalaryDetail.Text = m_Translate.GetSafeTranslationValue(grpSalaryDetail.Text)

			bsiInfo.Caption = m_Translate.GetSafeTranslationValue(bsiInfo.Caption)
			bbiSave.Caption = m_Translate.GetSafeTranslationValue(bbiSave.Caption)

		End Sub


#Region "Load data"

		Private Function LoadLanguageDrowpDownData() As Boolean
			Dim languageList = New List(Of KeywordsData)

			languageList.Add(New KeywordsData With {.Value = "de-CH"})
			languageList.Add(New KeywordsData With {.Value = "fr-CH"})
			languageList.Add(New KeywordsData With {.Value = "it-CH"})

			lueLanguage.Properties.DataSource = languageList

			If m_InitializationData.UserData.UserLanguage = "D" Then lueLanguage.EditValue = "de-CH"
			If m_InitializationData.UserData.UserLanguage = "F" Then lueLanguage.EditValue = "fr-CH"
			If m_InitializationData.UserData.UserLanguage = "I" Then lueLanguage.EditValue = "it-CH"

			m_Language = lueLanguage.EditValue

			Return True
		End Function

		Private Sub LoadEmployeeData()

			m_EmployeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(EmployeeNumber, False)

			If m_EmployeeMasterData Is Nothing Then
				Dim msg As String = m_Translate.GetSafeTranslationValue("Kandidatendaten konnten nicht geladen werden.")
				m_UtilityUI.ShowErrorDialog(msg)
				m_Logger.LogError(msg)

				Return
			End If

			Me.lblQualifikationValue.Text = String.Format("({0}) {1}", m_EmployeeMasterData.QLand, m_EmployeeMasterData.Profession)
			Me.lblGebValue.Text = String.Format("({0:d}) {1:f0}", m_EmployeeMasterData.Birthdate, m_EmployeeMasterData.EmployeeSUVABirthdateAge)
			Me.lblMANR.Text = String.Format("({0}) {1}", EmployeeNumber, m_EmployeeMasterData.EmployeeFullname)
			Me.lblBewValue.Text = String.Format("({0}) {1:d}", m_EmployeeMasterData.Permission, m_EmployeeMasterData.PermissionToDate)

		End Sub

		Private Sub LoadCustomerData()

			m_CustomerMasterData = m_CustomerDatabaseAccess.LoadCustomerMasterData(CustomerNumber, False)

			If m_CustomerMasterData Is Nothing Then
				Dim msg As String = m_Translate.GetSafeTranslationValue("Kundendaten konnten nicht geladen werden.")
				m_UtilityUI.ShowErrorDialog(msg)
				m_Logger.LogError(msg)

				Return
			End If

			Me.lblKDNR.Text = String.Format("({0}) {1}", m_CustomerMasterData.CustomerNumber, m_CustomerMasterData.Company1)

		End Sub

		Private Sub LoadEmploymentData()

			If EmploymentNumber = 0 Then Return
			m_EmploymentMasterData = m_ESDatabaseAccess.LoadESMasterData(EmploymentNumber)

			If m_EmploymentMasterData Is Nothing Then
				Dim msg As String = m_Translate.GetSafeTranslationValue("Einsatzdaten konnten nicht geladen werden.")
				m_UtilityUI.ShowErrorDialog(msg)
				m_Logger.LogError(msg)

				Return
			End If

			Dim employmentSalaryData = m_ESDatabaseAccess.LoadESSalaryData(EmploymentNumber)
			m_CurrentESLohnData = employmentSalaryData.Where(Function(x) x.AktivLODaten = True).ToList()

			Me.lblESNR.Text = String.Format("{0} | {1:d} {2:d}", m_EmploymentMasterData.ES_Als, m_EmploymentMasterData.ES_Ab, m_EmploymentMasterData.ES_Ende)

		End Sub

		Private Function LoadPVLDropDownData() As Boolean

			Dim data = PerformPVLlistWebserviceCall()

			If data Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("PVL-Daten konnen nicht geladen werden."))
				Return False
			End If
			m_CurrentPVLHourData = data


			Return True

		End Function

		Private Function PerformPVLlistWebserviceCall() As GAVNameResultDTO

			If lueContracts.EditValue Is Nothing Then Return Nothing

			Dim webservice = New SPPVLGAVUtilWebService.SPPVLGAVUtilSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_SPPVLUtilitiesServiceUrl)

			' Read data over webservice
			Dim searchResult = webservice.LoadCurrentMetaInfo(m_InitializationData.MDData.MDGuid, CustomerCanton, lueContracts.EditValue)

			Return searchResult

		End Function

#End Region


#Region "Load api data"

		Private Function LoadKeywordsData() As List(Of KeywordsData)
			Dim result As New List(Of KeywordsData)
			Dim baseUri As Uri = New Uri(String.Format(JOBROOM_KEYWORDS_URI, m_FirstURI, m_Language))
			Dim JSonString As New StringBuilder()
			Dim response As New HttpResponseMessage

			Dim data = Task.Run(Function() m_PVLUtility.WebserviceResponse(Nothing, baseUri, "GET", m_UserName, m_Password))
			data.Wait()

			response = data.Result
			If response Is Nothing Then Return Nothing


			Dim myStreamReader As New StreamReader(response.Content.ReadAsStreamAsync().Result)
			Dim responseData = myStreamReader.ReadToEnd

			If response.StatusCode <> 200 Then
				Return Nothing
			End If

			Dim searchResult = m_JsonUtility.ParseKeywordsJSonResult(responseData)
			lueKeywords.Properties.DataSource = searchResult

			Return searchResult
		End Function

		Private Function LoadBranchesData() As List(Of BranchesData)
			Dim result As New List(Of TempData.BranchesData)
			Dim baseUri As Uri = New Uri(String.Format(JOBROOM_BRACHES_URI, m_FirstURI, m_Language))
			Dim JSonString As New StringBuilder()
			Dim response As New HttpResponseMessage

			Dim data = Task.Run(Function() m_PVLUtility.WebserviceResponse(Nothing, baseUri, "GET", m_UserName, m_Password))
			data.Wait()

			response = data.Result
			If response Is Nothing Then Return Nothing


			Dim myStreamReader As New StreamReader(response.Content.ReadAsStreamAsync().Result)
			Dim responseData = myStreamReader.ReadToEnd

			If response.StatusCode <> 200 Then
				Return Nothing
			End If

			Dim searchResult = m_JsonUtility.ParseBrachesContractJSonResult(responseData)
			lueBranches.Properties.DataSource = searchResult


			Return searchResult
		End Function

		Private Function LoadAllPVLContractData() As List(Of ContractData)
			Dim result As New List(Of TempData.ContractData)
			Dim baseUri As Uri = New Uri(String.Format(JOBROOM_CONTRACTS_URI, m_FirstURI, m_Language))
			Dim JSonString As New StringBuilder()
			Dim response As New HttpResponseMessage

			Dim data = Task.Run(Function() m_PVLUtility.WebserviceResponse(Nothing, baseUri, "GET", m_UserName, m_Password))
			data.Wait()

			response = data.Result
			If response Is Nothing Then Return Nothing


			Dim myStreamReader As New StreamReader(response.Content.ReadAsStreamAsync().Result)
			Dim responseData = myStreamReader.ReadToEnd

			If response.StatusCode <> 200 Then
				Return Nothing
			End If

			Dim listData = m_JsonUtility.ParseContractJSonResult(responseData)

			If m_CurrentBranche Is Nothing Then

				result = listData

			Else

				For Each itm In listData

					For Each branche In m_CurrentBranche.Contracts
						If branche.ContractNumber = itm.ContractNumber Then
							result.Add(itm)
						End If
					Next

				Next
			End If
			If m_ExistingCustomerGAVData Is Nothing Then Return result


			Try
				Dim allowedEmploymentPVLData As New List(Of ContractData)
				If Not m_CurrentESLohnData Is Nothing Then

					Dim allowedList = result.Where(Function(x) x.ContractNumber = m_CurrentESLohnData(0).GAVNr).FirstOrDefault()
					If Not data Is Nothing Then
						allowedEmploymentPVLData.Add(allowedList)
					End If

				Else

					For Each itm In m_ExistingCustomerGAVData
						Dim allowedPVL = result.Where(Function(x) x.ContractNumber = itm.GAVNUmber).FirstOrDefault()
						If Not data Is Nothing Then
							allowedEmploymentPVLData.Add(allowedPVL)
						End If
					Next

				End If

				If allowedEmploymentPVLData Is Nothing OrElse allowedEmploymentPVLData.Count = 0 OrElse (allowedEmploymentPVLData(0) Is Nothing) Then
					allowedEmploymentPVLData = result
				Else
					result = allowedEmploymentPVLData
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				result = listData
			End Try
			lueContracts.Properties.DataSource = result
			m_ContractData = result

			Return result
		End Function

		Private Function LoadAssignedContractData(ByVal contractNumber As String) As ContractData
			Dim result As New ContractData
			Dim baseUri As Uri = New Uri(String.Format(JOBROOM_CONTRACT_NUMBER_URI, m_Language, contractNumber))
			Dim JSonString As New StringBuilder()
			Dim response As New HttpResponseMessage

			Dim data = Task.Run(Function() m_PVLUtility.WebserviceResponse(Nothing, baseUri, "GET", m_UserName, m_Password))
			data.Wait()

			response = data.Result
			If response Is Nothing Then Return Nothing

			Dim myStreamReader As New StreamReader(response.Content.ReadAsStreamAsync().Result)
			Dim responseData = myStreamReader.ReadToEnd

			If response.StatusCode <> 200 Then
				m_ErrorResult = m_JsonUtility.ParseJSonError(responseData)

				Return Nothing
			End If

			Dim contract As ContractData = m_JsonUtility.ParseContractJSonResult(responseData)(0)
			result = contract


			Return result
		End Function

		Private Function LoadAllVersionsForAssignedContractData(ByVal contractdNumber As Integer, ByVal validBy As Date) As List(Of ContractVersionData)
			Dim result As New List(Of ContractVersionData)
			Dim baseUri As Uri = New Uri(String.Format(JOBROOM_CONTRACT_VERSIONS_URI, m_FirstURI, m_Language, contractdNumber))
			Dim JSonString As New StringBuilder()
			Dim response As New HttpResponseMessage

			Dim data = Task.Run(Function() m_PVLUtility.WebserviceResponse(Nothing, baseUri, "GET", m_UserName, m_Password))
			data.Wait()

			response = data.Result
			If response Is Nothing Then Return Nothing


			Dim myStreamReader As New StreamReader(response.Content.ReadAsStreamAsync().Result)
			Dim responseData = myStreamReader.ReadToEnd

			If response.StatusCode <> 200 Then
				Return Nothing
			End If

			Dim searchResult = m_JsonUtility.ParseContractVersionsJSonResult(responseData)
			searchResult = searchResult.OrderByDescending(Function(x) x.ValidFrom).ToList

			lueContractVersions.Properties.DataSource = searchResult


			Return searchResult
		End Function

		Private Function LoadAllEditionsForAssignedContractVersionData(ByVal contractdNumber As Integer, ByVal versionNumber As Integer, ByVal validBy As Date) As List(Of ContractVersionEditionData)
			Dim result As New List(Of ContractVersionEditionData)
			Dim baseUri As Uri = New Uri(String.Format(JOBROOM_CONTRACTS_EDITIONS_URI, m_FirstURI, m_Language, contractdNumber, versionNumber))
			Dim JSonString As New StringBuilder()
			Dim response As New HttpResponseMessage

			Dim data = Task.Run(Function() m_PVLUtility.WebserviceResponse(Nothing, baseUri, "GET", m_UserName, m_Password))
			data.Wait()

			response = data.Result
			If response Is Nothing Then Return Nothing


			Dim myStreamReader As New StreamReader(response.Content.ReadAsStreamAsync().Result)
			Dim responseData = myStreamReader.ReadToEnd

			If response.StatusCode <> 200 Then
				Return Nothing
			End If

			Dim searchResult = m_JsonUtility.ParseEditionJSonResult(responseData)
			searchResult = searchResult.OrderByDescending(Function(x) x.Created).ThenBy(Function(x) x.Status).ToList
			lueContractEditions.Properties.DataSource = searchResult

			Return searchResult
		End Function

		Private Function LoadDocumentsForAssignedEditionData(ByVal editionID As Integer) As List(Of ContractEditionDocumentData)
			Dim result As New List(Of ContractEditionDocumentData)
			Dim baseUri As Uri = New Uri(String.Format(JOBROOM_DOCUMENT_EDITION_NUMBER_URI, m_FirstURI, m_Language, m_ContractNumber, m_ContractVersionNumber, m_ContractEditionNumber))
			Dim JSonString As New StringBuilder()
			Dim response As New HttpResponseMessage

			Dim data = Task.Run(Function() m_PVLUtility.WebserviceResponse(Nothing, baseUri, "GET", m_UserName, m_Password))
			data.Wait()

			response = data.Result
			If response Is Nothing Then Return Nothing

			Dim myStreamReader As New StreamReader(response.Content.ReadAsStreamAsync().Result)
			Dim responseData = myStreamReader.ReadToEnd

			If response.StatusCode <> 200 Then
				m_ErrorResult = m_JsonUtility.ParseJSonError(responseData)

				Return Nothing
			End If
			result = m_JsonUtility.ParseContractDocumentJSonResult(responseData)
			grdContractDocument.DataSource = result

			Return result
		End Function

		Private Function LoadAssignedContractDocumentFileIDData(ByVal fileID As String) As Boolean
			Dim result As Boolean = True
			Dim baseUri As Uri = New Uri(String.Format(JOBROOM_DOCUMENT_FILE_ID_URI, m_FirstURI, fileID))
			Dim JSonString As New StringBuilder()
			Dim response As New HttpResponseMessage

			Dim data = Task.Run(Function() m_PVLUtility.WebserviceResponse(Nothing, baseUri, "GET", m_UserName, m_Password))
			data.Wait()

			response = data.Result
			If response Is Nothing Then Return Nothing

			Dim myStreamReader As New StreamReader(response.Content.ReadAsStreamAsync().Result)
			Dim responseData = myStreamReader.ReadToEnd

			If response.StatusCode <> 200 Then
				m_ErrorResult = m_JsonUtility.ParseJSonError(responseData)

				Return Nothing
			End If

			Dim fileData = m_JsonUtility.ParseDocumentAssignedFileJSonResult(responseData)
			If fileData Is Nothing Then Return False

			Dim tmpFilename = Path.Combine(Path.GetTempPath, fileData.Filename)

			result = m_Utility.WriteFileBytes(tmpFilename, fileData.Data)
			If result Then
				Process.Start(tmpFilename)
			End If


			Return result
		End Function

		Private Function LoadLinksForAssignedEditionData(ByVal editionID As Integer) As List(Of ContractEditionLinksData)
			Dim result As New List(Of ContractEditionLinksData)
			Dim baseUri As Uri = New Uri(String.Format(JOBROOM_LINKS_EDITION_NUMBER_URI, m_FirstURI, m_Language, m_ContractNumber, m_ContractVersionNumber, m_ContractEditionNumber))
			Dim JSonString As New StringBuilder()
			Dim response As New HttpResponseMessage

			Dim data = Task.Run(Function() m_PVLUtility.WebserviceResponse(Nothing, baseUri, "GET", m_UserName, m_Password))
			data.Wait()

			response = data.Result
			If response Is Nothing Then Return Nothing

			Dim myStreamReader As New StreamReader(response.Content.ReadAsStreamAsync().Result)
			Dim responseData = myStreamReader.ReadToEnd

			If response.StatusCode <> 200 Then
				m_ErrorResult = m_JsonUtility.ParseJSonError(responseData)

				Return Nothing
			End If
			result = m_JsonUtility.ParseContractLinksJSonResult(responseData)
			grdLinks.DataSource = result

			Return result
		End Function

		Private Function LoadMindestLohnCriteriasForAssignedEditionData(ByVal editionID As Integer) As List(Of MindestLohnInputCriteriasData)
			Dim result As New List(Of MindestLohnInputCriteriasData)
			Dim baseUri As Uri = New Uri(String.Format(JOBROOM_MINDESTLOHN_EDITION_URI, m_FirstURI, m_Language, editionID))
			Dim JSonString As New StringBuilder()
			Dim response As New HttpResponseMessage

			Dim data = Task.Run(Function() m_PVLUtility.WebserviceResponse(Nothing, baseUri, "GET", m_UserName, m_Password))
			data.Wait()

			response = data.Result
			If response Is Nothing Then Return Nothing

			Dim myStreamReader As New StreamReader(response.Content.ReadAsStreamAsync().Result)
			Dim responseData = myStreamReader.ReadToEnd

			If response.StatusCode <> 200 Then
				m_ErrorResult = m_JsonUtility.ParseJSonError(responseData)

				Return Nothing
			End If
			result = m_JsonUtility.ParseMindestLohnCriteriasJSonResult(responseData)

			Return result
		End Function



		Private Function LoadInputValuesForAssignedCriteriaStructureIDData(ByVal criteriaStrucutreId As Integer, ByVal editionId As Integer) As List(Of InputValueData)
			Dim result As New List(Of InputValueData)
			Dim baseUri As Uri = New Uri(String.Format(JOBROOM_MINDESTLOHN_EDITION_CRITERIA_URI, m_FirstURI, m_Language, editionId, criteriaStrucutreId))
			Dim JSonString As New StringBuilder()
			Dim response As New HttpResponseMessage
			Dim criteriasData As List(Of InputValueData_Org) = GetListOfGivenCriteriasData()

			Dim jQueryChoosenInputValuesContent = BuildJasonstringForGivenInputCriterias(criteriasData)
			If String.IsNullOrWhiteSpace(jQueryChoosenInputValuesContent.ToString) Then
				m_Logger.LogError("build BuildJasonstring was failed!")

				Return Nothing
			End If
			m_Logger.LogDebug(String.Format("{1}: {2}{0}body: {3}", vbNewLine, criteriaStrucutreId, baseUri.ToString, jQueryChoosenInputValuesContent))


			Dim data = Task.Run(Function() m_PVLUtility.WebserviceResponse(jQueryChoosenInputValuesContent, baseUri, "POST", m_UserName, m_Password))
			data.Wait()

			response = data.Result
			If response Is Nothing Then Return Nothing

			Dim myStreamReader As New StreamReader(response.Content.ReadAsStreamAsync().Result)
			Dim responseData = myStreamReader.ReadToEnd

			If response.StatusCode <> 200 Then
				m_ErrorResult = m_JsonUtility.ParseJSonError(responseData)

				Return Nothing
			End If

			result = m_JsonUtility.ParseInputValuesForAssignedCriteriaStructureID(responseData)

			Return result
		End Function

		Private Function LoadMindestLohnCalculatedResultForAssignedCriteriaData(ByVal editionId As Integer) As MindestLohnResultData
			Dim result As New MindestLohnResultData
			Dim baseUri As Uri = New Uri(String.Format(JOBROOM_MINDESTLOHN_RESULT_URI, m_FirstURI, m_Language, editionId, "false"))
			Dim JSonString As New StringBuilder()
			Dim response As New HttpResponseMessage
			Dim criteriasData As List(Of InputValueData_Org) = GetListOfGivenCriteriasData()

			Dim jQueryChoosenInputValuesContent = BuildJasonstringForGivenInputCriterias(criteriasData)
			If String.IsNullOrWhiteSpace(jQueryChoosenInputValuesContent.ToString) Then
				m_Logger.LogError("build BuildJasonstring was failed!")

				Return Nothing
			End If

			Dim data = Task.Run(Function() m_PVLUtility.WebserviceResponse(jQueryChoosenInputValuesContent, baseUri, "POST", m_UserName, m_Password))
			data.Wait()

			response = data.Result
			If response Is Nothing Then Return Nothing

			Dim myStreamReader As New StreamReader(response.Content.ReadAsStreamAsync().Result)
			Dim responseData = myStreamReader.ReadToEnd

			If response.StatusCode <> 200 Then
				m_ErrorResult = m_JsonUtility.ParseJSonError(responseData)

				Return Nothing
			End If

			result = m_JsonUtility.ParseMindestLohnResultJSonResult(responseData)

			Return result
		End Function

		Private Sub grpGAVCategory_CustomButtonClick(sender As Object, e As DevExpress.XtraBars.Docking2010.BaseButtonEventArgs) Handles grpGAVCategory.CustomButtonClick

			Dim index As Integer = e.Button.Properties.GroupIndex
			If index = 0 Then
				Dim criteriasData As List(Of InputValueData_Org) = GetListOfGivenCriteriasData()
				Dim jQueryChoosenInputValuesContent = BuildJasonstringForGivenInputCriterias(criteriasData)

				If String.IsNullOrWhiteSpace(jQueryChoosenInputValuesContent.ToString) Then Return


				If flpBodyContentToLoadSalary.FlyoutPanelState.IsActive Then Return

				flpBodyContentToLoadSalary.ShowBeakForm(Control.MousePosition) ' GetFocusedRowPoint)
				Dim debugValue As String = jQueryChoosenInputValuesContent.ToString
				debugValue = debugValue.Replace("|", "<br>").Replace("¦", "<br>")
				rtfContent.HtmlText = debugValue

			ElseIf index = 1 Then
				If gvSalaryDetail.RowCount > 0 Then grdSalaryDetail.ShowPrintPreview()
				If gvAdditionalText.RowCount > 0 Then grdAdditionalText.ShowPrintPreview()

			End If


		End Sub

		Private Function GetListOfGivenCriteriasData() As List(Of InputValueData_Org)
			Dim result As New List(Of InputValueData_Org)
			Dim data As New InputValueData_Org

			Try

				If Not lue0.EditValue Is Nothing OrElse lue0.Visible Then
					Dim value = SelectedAssignedPVLCategoryData(0)
					data = New InputValueData_Org

					If value Is Nothing Then
						data.CriteriaListEntryID = Nothing
						data.CriteriaStructureID = Val(lblCat0.Text.Split("(")(1))
						data.Value = String.Empty

						data = New InputValueData_Org With {.CriteriaListEntryID = data.CriteriaListEntryID, .CriteriaStructureID = data.CriteriaStructureID, .Value = String.Empty}

					Else
						data.CriteriaListEntryID = value.CriteriaListEntryID_Org
						data.CriteriaStructureID = value.CriteriaStructureID
						data.Value = value.Value

					End If

					result.Add(data)
				End If

				If Not lue1.EditValue Is Nothing OrElse lue1.Visible Then
					Dim value = SelectedAssignedPVLCategoryData(1)
					data = New InputValueData_Org

					If value Is Nothing Then
						data.CriteriaListEntryID = Nothing
						data.CriteriaStructureID = Val(lblCat1.Text.Split("(")(1))
						data.Value = String.Empty

						data = New InputValueData_Org With {.CriteriaListEntryID = data.CriteriaListEntryID, .CriteriaStructureID = data.CriteriaStructureID, .Value = String.Empty}
					Else
						data.CriteriaListEntryID = value.CriteriaListEntryID_Org
						data.CriteriaStructureID = value.CriteriaStructureID
						data.Value = value.Value
					End If

					result.Add(data)
				End If
				If Not lue2.EditValue Is Nothing OrElse lue2.Visible Then
					Dim value = SelectedAssignedPVLCategoryData(2) ' SelectedPVLCategory2Data
					data = New InputValueData_Org

					If value Is Nothing Then
						data.CriteriaListEntryID = Nothing
						data.CriteriaStructureID = Val(lblCat2.Text.Split("(")(1))
						data.Value = String.Empty

						data = New InputValueData_Org With {.CriteriaListEntryID = data.CriteriaListEntryID, .CriteriaStructureID = data.CriteriaStructureID, .Value = String.Empty}
					Else
						data.CriteriaListEntryID = value.CriteriaListEntryID_Org
						data.CriteriaStructureID = value.CriteriaStructureID
						data.Value = value.Value
					End If

					result.Add(data)
				End If
				If Not lue3.EditValue Is Nothing OrElse lue3.Visible Then
					Dim value = SelectedAssignedPVLCategoryData(3) ' SelectedPVLCategory3Data
					data = New InputValueData_Org

					If value Is Nothing Then
						data.CriteriaListEntryID = Nothing
						data.CriteriaStructureID = Val(lblCat3.Text.Split("(")(1))
						data.Value = String.Empty

						data = New InputValueData_Org With {.CriteriaListEntryID = data.CriteriaListEntryID, .CriteriaStructureID = data.CriteriaStructureID, .Value = String.Empty}
					Else
						data.CriteriaListEntryID = value.CriteriaListEntryID_Org
						data.CriteriaStructureID = value.CriteriaStructureID
						data.Value = value.Value

					End If

					result.Add(data)
				End If
				If Not lue4.EditValue Is Nothing OrElse lue4.Visible Then
					Dim value = SelectedAssignedPVLCategoryData(4) ' SelectedPVLCategory4Data
					data = New InputValueData_Org

					If value Is Nothing Then
						data.CriteriaListEntryID = Nothing
						data.CriteriaStructureID = Val(lblCat4.Text.Split("(")(1))
						data.Value = String.Empty

						data = New InputValueData_Org With {.CriteriaListEntryID = data.CriteriaListEntryID, .CriteriaStructureID = data.CriteriaStructureID, .Value = String.Empty}
					Else
						data.CriteriaListEntryID = value.CriteriaListEntryID_Org
						data.CriteriaStructureID = value.CriteriaStructureID
						data.Value = value.Value
					End If

					result.Add(data)
				End If
				If Not lue5.EditValue Is Nothing OrElse lue5.Visible Then
					Dim value = SelectedAssignedPVLCategoryData(5) ' SelectedPVLCategory5Data
					data = New InputValueData_Org

					If value Is Nothing Then
						data.CriteriaListEntryID = Nothing
						data.CriteriaStructureID = Val(lblCat5.Text.Split("(")(1))
						data.Value = String.Empty

						data = New InputValueData_Org With {.CriteriaListEntryID = data.CriteriaListEntryID, .CriteriaStructureID = data.CriteriaStructureID, .Value = String.Empty}
					Else
						data.CriteriaListEntryID = value.CriteriaListEntryID_Org
						data.CriteriaStructureID = value.CriteriaStructureID
						data.Value = value.Value
					End If

					result.Add(data)
				End If
				If Not lue6.EditValue Is Nothing OrElse lue6.Visible Then
					Dim value = SelectedAssignedPVLCategoryData(6) ' SelectedPVLCategory6Data
					data = New InputValueData_Org

					If value Is Nothing Then
						data.CriteriaListEntryID = Nothing
						data.CriteriaStructureID = Val(lblCat6.Text.Split("(")(1))
						data.Value = String.Empty

						data = New InputValueData_Org With {.CriteriaListEntryID = data.CriteriaListEntryID, .CriteriaStructureID = data.CriteriaStructureID, .Value = String.Empty}
					Else
						data.CriteriaListEntryID = value.CriteriaListEntryID_Org
						data.CriteriaStructureID = value.CriteriaStructureID
						data.Value = value.Value
					End If

					result.Add(data)
				End If
				If Not lue7.EditValue Is Nothing OrElse lue7.Visible Then
					Dim value = SelectedAssignedPVLCategoryData(7) ' SelectedPVLCategory7Data
					data = New InputValueData_Org

					If value Is Nothing Then
						data.CriteriaListEntryID = Nothing
						data.CriteriaStructureID = Val(lblCat7.Text.Split("(")(1))
						data.Value = String.Empty

						'value = New InputValueData With {.CriteriaListEntryID = data.CriteriaListEntryID, .CriteriaStructureID = data.CriteriaStructureID, .Value = String.Empty}
						data = New InputValueData_Org With {.CriteriaListEntryID = data.CriteriaListEntryID, .CriteriaStructureID = data.CriteriaStructureID, .Value = String.Empty}
					Else
						data.CriteriaListEntryID = value.CriteriaListEntryID_Org
						data.CriteriaStructureID = value.CriteriaStructureID
						data.Value = value.Value
					End If

					result.Add(data)
				End If


				If Not lue8.EditValue Is Nothing OrElse lue8.Visible Then
					Dim value = SelectedAssignedPVLCategoryData(8)
					data = New InputValueData_Org

					If value Is Nothing Then
						data.CriteriaListEntryID = Nothing
						data.CriteriaStructureID = Val(lblCat8.Text.Split("(")(1))
						data.Value = String.Empty

						data = New InputValueData_Org With {.CriteriaListEntryID = data.CriteriaListEntryID, .CriteriaStructureID = data.CriteriaStructureID, .Value = String.Empty}
					Else
						data.CriteriaListEntryID = value.CriteriaListEntryID_Org
						data.CriteriaStructureID = value.CriteriaStructureID
						data.Value = value.Value
					End If

					result.Add(data)
				End If
				If Not lue9.EditValue Is Nothing OrElse lue9.Visible Then
					Dim value = SelectedAssignedPVLCategoryData(9)
					data = New InputValueData_Org

					If value Is Nothing Then
						data.CriteriaListEntryID = Nothing
						data.CriteriaStructureID = Val(lblCat9.Text.Split("(")(1))
						data.Value = String.Empty

						data = New InputValueData_Org With {.CriteriaListEntryID = data.CriteriaListEntryID, .CriteriaStructureID = data.CriteriaStructureID, .Value = String.Empty}
					Else
						data.CriteriaListEntryID = value.CriteriaListEntryID_Org
						data.CriteriaStructureID = value.CriteriaStructureID
						data.Value = value.Value
					End If

					result.Add(data)
				End If
				If Not lue10.EditValue Is Nothing OrElse lue10.Visible Then
					Dim value = SelectedAssignedPVLCategoryData(10)
					data = New InputValueData_Org

					If value Is Nothing Then
						data.CriteriaListEntryID = Nothing
						data.CriteriaStructureID = Val(lblCat10.Text.Split("(")(1))
						data.Value = String.Empty

						data = New InputValueData_Org With {.CriteriaListEntryID = data.CriteriaListEntryID, .CriteriaStructureID = data.CriteriaStructureID, .Value = String.Empty}
					Else
						data.CriteriaListEntryID = value.CriteriaListEntryID_Org
						data.CriteriaStructureID = value.CriteriaStructureID
						data.Value = value.Value
					End If

					result.Add(data)
				End If
				If Not lue11.EditValue Is Nothing OrElse lue11.Visible Then
					Dim value = SelectedAssignedPVLCategoryData(11)
					data = New InputValueData_Org

					If value Is Nothing Then
						data.CriteriaListEntryID = Nothing
						data.CriteriaStructureID = Val(lblCat11.Text.Split("(")(1))
						data.Value = String.Empty

						data = New InputValueData_Org With {.CriteriaListEntryID = data.CriteriaListEntryID, .CriteriaStructureID = data.CriteriaStructureID, .Value = String.Empty}
					Else
						data.CriteriaListEntryID = value.CriteriaListEntryID_Org
						data.CriteriaStructureID = value.CriteriaStructureID
						data.Value = value.Value
					End If

					result.Add(data)
				End If
				If Not lue12.EditValue Is Nothing OrElse lue12.Visible Then
					Dim value = SelectedAssignedPVLCategoryData(12)
					data = New InputValueData_Org

					If value Is Nothing Then
						data.CriteriaListEntryID = Nothing
						data.CriteriaStructureID = Val(lblCat12.Text.Split("(")(1))
						data.Value = String.Empty

						data = New InputValueData_Org With {.CriteriaListEntryID = data.CriteriaListEntryID, .CriteriaStructureID = data.CriteriaStructureID, .Value = String.Empty}
					Else
						data.CriteriaListEntryID = value.CriteriaListEntryID_Org
						data.CriteriaStructureID = value.CriteriaStructureID
						data.Value = value.Value
					End If

					result.Add(data)
				End If


			Catch ex As Exception
				result = Nothing
			End Try

			Return result
		End Function

		Private Function BuildJasonstringForGivenInputCriterias(ByVal criteriasData As List(Of InputValueData_Org)) As StringBuilder
			Dim msgContent = "building jason string is started..."

			Dim sb As New StringBuilder()
			Dim sw As New StringWriter(sb)

			Try
				Dim test = JsonConvert.SerializeObject(criteriasData)
				sb.Append(test)


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				m_UtilityUI.ShowErrorDialog(ex.ToString, "BuildJasonstringForGivenInputCriterias")

				Return Nothing

			End Try


			Return sb

		End Function

		Private Function BuildJasonstringForResultGivenInputCriterias(ByVal criteriaStructureIDs As List(Of Integer), ByVal names As List(Of String), ByVal values As List(Of String), ByVal criteriaListEntryIds As List(Of Integer)) As StringBuilder
			Dim msgContent = "building jason string is started..."

			Dim sb As New StringBuilder()
			Dim sw As New StringWriter(sb)

			Try
				Using writer As JsonWriter = New JsonTextWriter(sw)

					Dim i As Integer = 0
					If Not criteriaStructureIDs Is Nothing AndAlso criteriaStructureIDs.Count > 0 Then
						writer.WriteStartArray()

						' TODO: if not exists, must be "de" and "NONE"
						For Each itm In criteriaStructureIDs
							writer.WriteStartObject()
							writer.WritePropertyName("criteriaStructureId")
							writer.WriteValue(String.Format("{0}", itm))

							writer.WritePropertyName("name")
							writer.WriteValue(String.Format("{0}", names(i)))

							writer.WritePropertyName("value")
							writer.WriteValue(String.Format("{0}", values(i)))

							writer.WritePropertyName("criteriaListEntryId")
							writer.WriteValue(String.Format("{0}", criteriaListEntryIds(i)))

							writer.WriteEndObject()
							i += 1
						Next
						writer.WriteEndArray()

					End If


				End Using

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				m_UtilityUI.ShowErrorDialog(ex.ToString, "BuildJasonstringForResultGivenInputCriterias")

				Return Nothing

			End Try


			Return sb

		End Function


#End Region




		Private Sub OnlueLanguage_EditValueChanged(sender As Object, e As EventArgs) Handles lueLanguage.EditValueChanged

			If m_SuppressUIEvents Then Return
			If lueLanguage.EditValue Is Nothing Then Return

			SplashScreenManager.CloseForm(False)
			SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
			SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Vorlage wird übersetzt") & Space(20))
			SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

			m_SuppressUIEvents = True

			m_Language = lueLanguage.EditValue

			grpGAVCategory.Visible = False
			m_CurrentBranche = SelectedBranchesData

			lueContracts.Properties.DataSource = Nothing
			lueContractVersions.Properties.DataSource = Nothing
			lueContractEditions.Properties.DataSource = Nothing

			LoadKeywordsData()
			LoadBranchesData()
			LoadAllPVLContractData()

			lueContracts.EditValue = Nothing
			lueContractVersions.EditValue = Nothing
			lueContractEditions.EditValue = Nothing

			grdSalaryDetail.DataSource = Nothing
			grdAdditionalText.DataSource = Nothing

			grdContractDocument.DataSource = Nothing
			grdLinks.DataSource = Nothing

			ClearCategorieDropDown()

			m_SuppressUIEvents = False

			If m_ContractData.Count = 1 Then
				lueContracts.EditValue = m_ContractData(0).ContractNumber
			End If

			SplashScreenManager.CloseForm(False)

		End Sub

		Private Sub OnlueBranches_EditValueChanged(sender As Object, e As EventArgs) Handles lueBranches.EditValueChanged
			If lueBranches.EditValue Is Nothing Then Return

			SplashScreenManager.CloseForm(False)
			SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
			SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Vorlage wird übersetzt") & Space(20))
			SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

			m_SuppressUIEvents = True

			grpGAVCategory.Visible = False
			m_CurrentBranche = SelectedBranchesData

			lueContracts.Properties.DataSource = Nothing
			lueContractVersions.Properties.DataSource = Nothing
			lueContractEditions.Properties.DataSource = Nothing

			LoadAllPVLContractData()

			lueContracts.EditValue = Nothing
			lueContractVersions.EditValue = Nothing
			lueContractEditions.EditValue = Nothing

			grdSalaryDetail.DataSource = Nothing
			grdAdditionalText.DataSource = Nothing

			grdContractDocument.DataSource = Nothing
			grdLinks.DataSource = Nothing

			ClearCategorieDropDown()

			m_SuppressUIEvents = False

			If m_ContractData.Count = 1 Then
				lueContracts.EditValue = m_ContractData(0).ContractNumber
			End If

			SplashScreenManager.CloseForm(False)

		End Sub

		Private Sub OnlueContracts_EditValueChanged(sender As Object, e As EventArgs) Handles lueContracts.EditValueChanged
			If lueContracts.EditValue Is Nothing Then Return
			m_ContractNumber = lueContracts.EditValue

			SplashScreenManager.CloseForm(False)
			SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
			SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Vorlage wird übersetzt") & Space(20))
			SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

			m_SuppressUIEvents = True

			lueContractVersions.Properties.DataSource = Nothing
			lueContractEditions.Properties.DataSource = Nothing

			Dim contractData = SelectedPVLData
			If contractData Is Nothing Then
				m_UtilityUI.ShowOKDialog("Ausgewählte PVL-Daten können nicht heruntergeladen werden.")

				Return
			End If

			'm_Language = contractData.ContractLanguage
			Dim contractVersions = LoadAllVersionsForAssignedContractData(m_ContractNumber, Nothing)

			lueContractVersions.EditValue = Nothing
			If contractVersions Is Nothing OrElse contractVersions.Count = 0 Then
				m_Language = contractData.ContractLanguage
				lueLanguage.EditValue = m_Language

				contractVersions = LoadAllVersionsForAssignedContractData(m_ContractNumber, Nothing)

			End If
			If Not contractVersions Is Nothing AndAlso contractVersions.Count >= 1 Then

					lueContractVersions.EditValue = contractVersions(0).Number
					m_ContractVersionNumber = lueContractVersions.EditValue

					lueContractEditions.EditValue = Nothing
					Dim contractEditions = LoadAllEditionsForAssignedContractVersionData(m_ContractNumber, m_ContractVersionNumber, Nothing)

					If Not contractEditions Is Nothing Then
						Dim allowedEditions = contractEditions.Where(Function(x) x.Status <> "Superseded").ToList
						lueContractEditions.EditValue = allowedEditions(0).ID
					End If

					DisplayContractDetails()
				End If

				m_SuppressUIEvents = False

			SplashScreenManager.CloseForm(False)

		End Sub

		Private Sub OnlueContractVersions_EditValueChanged(sender As Object, e As EventArgs) Handles lueContractVersions.EditValueChanged
			If m_SuppressUIEvents Then Return

			SplashScreenManager.CloseForm(False)
			SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
			SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Vorlage wird übersetzt") & Space(20))
			SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

			If lueContracts.EditValue Is Nothing OrElse lueContractVersions.EditValue Is Nothing Then Return
			m_ContractVersionNumber = lueContractVersions.EditValue

			m_SuppressUIEvents = True
			lueContractEditions.EditValue = Nothing
			Dim contractEditions = LoadAllEditionsForAssignedContractVersionData(m_ContractNumber, m_ContractVersionNumber, Nothing)

			If Not contractEditions Is Nothing Then
				Dim allowedEditions = contractEditions.Where(Function(x) x.Status <> "Superseded").ToList
				lueContractEditions.EditValue = allowedEditions(0).ID
			End If

			DisplayContractDetails()

			m_SuppressUIEvents = False

			SplashScreenManager.CloseForm(False)

		End Sub

		Private Sub OnlueContractEditions_EditValueChanged(sender As Object, e As EventArgs) Handles lueContractEditions.EditValueChanged
			If m_SuppressUIEvents Then Return

			SplashScreenManager.CloseForm(False)
			SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
			SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Vorlage wird übersetzt") & Space(20))
			SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

			If lueContracts.EditValue Is Nothing OrElse lueContractVersions.EditValue Is Nothing Then Return
			m_ContractEditionNumber = lueContractEditions.EditValue

			m_SuppressUIEvents = True

			DisplayContractDetails()

			m_SuppressUIEvents = False

			SplashScreenManager.CloseForm(False)

		End Sub

		Private Sub DisplayContractDetails()

			m_ContractEditionNumber = lueContractEditions.EditValue
			lblLinkToPVLData.Tag = String.Empty
			lblLinkToPVLData.Visible = False

			grdSalaryDetail.DataSource = Nothing
			grdAdditionalText.DataSource = Nothing
			grdContractDocument.DataSource = Nothing
			grdLinks.DataSource = Nothing

			ClearCategorieDropDown()
			m_SalaryCriteriasData = LoadMindestLohnCriteriasForAssignedEditionData(m_ContractEditionNumber)

			Try
				If LoadPVLDropDownData() Then
					txtStdWeek.EditValue = m_CurrentPVLHourData.stdweek.GetValueOrDefault(40)
				End If
				LoadDocumentsForAssignedEditionData(m_ContractEditionNumber)
				LoadLinksForAssignedEditionData(m_ContractEditionNumber)

				lblLinkToPVLData.Tag = String.Format("https://www.tempservice.ch/de/tempdata/detail.php?cid={0}&vid={1}&eid={2}", m_ContractNumber, m_ContractVersionNumber, m_ContractEditionNumber)
				lblLinkToPVLData.Visible = True

			Catch ex As Exception

			End Try

			Try
				If m_SalaryCriteriasData Is Nothing OrElse m_SalaryCriteriasData.Count = 0 Then Return
				SetCategoriesLabels()


			Catch ex As Exception

			End Try

			grpGAVCategory.Visible = True

		End Sub


		Private Sub OnlueMindestLohnCategories_EditValueChanged(sender As Object, e As EventArgs)
			If m_SuppressUIEvents Then Return
			If lueContracts.EditValue Is Nothing Then Return
			ReadSelectedCategoryValues()

			Dim data As New InputValueData
			Dim i As Integer
			Select Case sender.name.tolower
				Case "lue0"
					data = SelectedAssignedPVLCategoryData(0)
					i = 0
				Case "lue1"
					data = SelectedAssignedPVLCategoryData(1)
					i = 1
				Case "lue2"
					data = SelectedAssignedPVLCategoryData(2)
					i = 2
				Case "lue3"
					data = SelectedAssignedPVLCategoryData(3)
					i = 3
				Case "lue4"
					data = SelectedAssignedPVLCategoryData(4)
					i = 4
				Case "lue5"
					data = SelectedAssignedPVLCategoryData(5)
					i = 5
				Case "lue6"
					data = SelectedAssignedPVLCategoryData(6)
					i = 6
				Case "lue7"
					data = SelectedAssignedPVLCategoryData(7)
					i = 7
				Case "lue8"
					data = SelectedAssignedPVLCategoryData(8)
					i = 8
				Case "lue9"
					data = SelectedAssignedPVLCategoryData(9)
					i = 9
				Case "lue10"
					data = SelectedAssignedPVLCategoryData(10)
					i = 10
				Case "lue11"
					data = SelectedAssignedPVLCategoryData(11)
					i = 11
				Case "lue12"
					data = SelectedAssignedPVLCategoryData(12)
					i = 12

				Case Else
					Return

			End Select
			If data Is Nothing Then Return
			CreateSubCategory_Staging(data, i)

		End Sub

		Private Sub LoadMinumumSalaryData()

			CloseProgressPanel(m_FirstHandle)
			CloseProgressPanel(m_SecHandle)
			m_FirstHandle = ShowProgressPanel(grdSalaryDetail)
			m_SecHandle = ShowProgressPanel(grdAdditionalText)

			Dim mindestLohnData = LoadMindestLohnCalculatedResultForAssignedCriteriaData(m_ContractEditionNumber)

			ResetGridMindestLohn()
			ResetAdditionalTextFields()

			Dim listDataSource As BindingList(Of MindestLohnViewData) = New BindingList(Of MindestLohnViewData)

			Dim data As New MindestLohnViewData
			If mindestLohnData Is Nothing Then
				CloseProgressPanel(m_FirstHandle)
				CloseProgressPanel(m_SecHandle)

				Return
			End If

			Try
				data.Label = m_Translate.GetSafeTranslationValueInOtherLanguage("Mindestmonatslohn", m_Language)
				data.VariabelnText = String.Empty
				data.Unit = "CHF"
				data.Amount = String.Format("{0:n2}", mindestLohnData.Salary.MonthSalary)
				listDataSource.Add(data)

				data = New MindestLohnViewData
				data.Label = m_Translate.GetSafeTranslationValue("Feiertage pro Jahr")
				If Not mindestLohnData.VariableText.publicHolidays Is Nothing Then data.VariabelnText = m_HtmlUtil.ConvertToPlainText(mindestLohnData.VariableText.publicHolidays.description)
				data.Unit = m_Translate.GetSafeTranslationValue("Tage")
				data.Amount = String.Format("{0:n2}", mindestLohnData.Salary.PublicHolidays)
				listDataSource.Add(data)

				data = New MindestLohnViewData
				data.Label = m_Translate.GetSafeTranslationValue("Ferientage pro Jahr")
				data.VariabelnText = String.Empty
				data.Unit = m_Translate.GetSafeTranslationValue("Tage")
				data.Amount = String.Format("{0:n2}", mindestLohnData.Salary.VacationDays)
				listDataSource.Add(data)

				data = New MindestLohnViewData
				data.Label = m_Translate.GetSafeTranslationValue("13. Monatslohn")
				data.VariabelnText = ""
				data.Unit = ""
				data.Amount = m_Translate.GetSafeTranslationValue(If(mindestLohnData.Salary.Has13thMonthSalary.GetValueOrDefault(False), "Ja", "Nein"))
				listDataSource.Add(data)


				data = New MindestLohnViewData
				data.Label = m_Translate.GetSafeTranslationValue("Basisstundenlohn")
				data.VariabelnText = ""
				data.Unit = "CHF"
				data.Amount = String.Format("{0:n2}", mindestLohnData.Salary.BasicSalary)
				listDataSource.Add(data)


				data = New MindestLohnViewData
				If Not mindestLohnData.VariableText.AdditionalProp2 Is Nothing Then data.Label = mindestLohnData.VariableText.AdditionalProp2.Titel
				data.Label = m_Translate.GetSafeTranslationValue("Feiertagsentschädigung")
				If Not mindestLohnData.VariableText.pctHolidaysCompensation Is Nothing Then data.VariabelnText = m_HtmlUtil.ConvertToPlainText(mindestLohnData.VariableText.pctHolidaysCompensation.description)
				data.Unit = "CHF"
				data.Amount = String.Format("{0:n2}", mindestLohnData.Salary.PublicHolidaysCompensation)
				listDataSource.Add(data)

				data = New MindestLohnViewData
				If Not mindestLohnData.VariableText.AdditionalProp1 Is Nothing Then data.Label = mindestLohnData.VariableText.AdditionalProp1.Titel '  "Feiertagsentschädigung"
				If Not mindestLohnData.VariableText.pctVacationCompensation Is Nothing Then data.VariabelnText = m_HtmlUtil.ConvertToPlainText(mindestLohnData.VariableText.pctVacationCompensation.description)
				data.Label = m_Translate.GetSafeTranslationValue("Ferienentschädigung")
				data.Unit = "CHF"
				data.Amount = String.Format("{0:n2}", mindestLohnData.Salary.VacationCompensation)
				listDataSource.Add(data)

				data = New MindestLohnViewData
				If Not mindestLohnData.VariableText.AdditionalProp3 Is Nothing Then data.Label = mindestLohnData.VariableText.AdditionalProp3.Titel ' "Entschädigung 13. Monatslohn"
				If Not mindestLohnData.VariableText.compensation13thSalary Is Nothing Then data.VariabelnText = m_HtmlUtil.ConvertToPlainText(mindestLohnData.VariableText.compensation13thSalary.description)
				data.Label = m_Translate.GetSafeTranslationValue("Entschädigung 13. Monatslohn")
				data.Unit = "CHF"
				data.Amount = String.Format("{0:n2}", mindestLohnData.Salary.Compensation13thSalary)
				listDataSource.Add(data)


				data = New MindestLohnViewData
				data.Label = m_Translate.GetSafeTranslationValue("Mindeststundenlohn mit 13. Monatslohn")
				data.VariabelnText = m_Translate.GetSafeTranslationValue("Rundungsdifferenzen möglich")
				data.Unit = "CHF"
				Dim totalAmount As Decimal = mindestLohnData.Salary.BasicSalary.GetValueOrDefault(0) + mindestLohnData.Salary.VacationCompensation.GetValueOrDefault(0) + mindestLohnData.Salary.PublicHolidaysCompensation.GetValueOrDefault(0) + mindestLohnData.Salary.Compensation13thSalary.GetValueOrDefault(0)
				data.Amount = String.Format("{0:n2}", totalAmount)
				listDataSource.Add(data)


				data = New MindestLohnViewData
				'data.Label = String.Format("{0} {1}", mindestLohnData.VariableText.faragBeitrag.Titel, mindestLohnData.VariableText.faragBeitrag.Text) '  "Arbeitgeberbeitrag"
				'data.VariabelnText = mindestLohnData.VariableText.faragBeitrag.Description
				data.Label = m_Translate.GetSafeTranslationValue("Arbeitgeberbeitrag")
				If Not mindestLohnData.VariableText.ereContribution Is Nothing Then data.VariabelnText = m_HtmlUtil.ConvertToPlainText(mindestLohnData.VariableText.ereContribution.text)
				data.Unit = "%"
				data.Amount = String.Format("{0:n2}", mindestLohnData.Salary.EREContribution.GetValueOrDefault(0) * 100)
				listDataSource.Add(data)

				data = New MindestLohnViewData
				data.Label = m_Translate.GetSafeTranslationValue("Arbeitnehmerbeitrag")
				data.VariabelnText = ""
				data.Unit = "%"
				data.Amount = String.Format("{0:n2}", mindestLohnData.Salary.ERWContribution.GetValueOrDefault(0) * 100)
				listDataSource.Add(data)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				listDataSource = Nothing

				CloseProgressPanel(m_FirstHandle)

			End Try

			m_SalaryData = mindestLohnData
			grdSalaryDetail.DataSource = listDataSource

			CloseProgressPanel(m_FirstHandle)

			Try

				Dim listAdditionalTextData As BindingList(Of MindestLohnViewData) = New BindingList(Of MindestLohnViewData)
				For Each p In mindestLohnData.AdditionalText
					data = New MindestLohnViewData

					data.Label = String.Empty ' p.Text
					data.VariabelnText = m_HtmlUtil.ConvertToPlainText(p.Text)
					data.Unit = String.Empty

					listAdditionalTextData.Add(data)
				Next

				For Each p In mindestLohnData.Footnotes
					data = New MindestLohnViewData

					data.Label = String.Empty
					data.VariabelnText = m_HtmlUtil.ConvertToPlainText(String.Format("{0} - {1}", p.Titel, p.Text))
					data.Unit = String.Empty

					listAdditionalTextData.Add(data)
				Next

				grdAdditionalText.DataSource = listAdditionalTextData

				CloseProgressPanel(m_SecHandle)


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			Finally
				CloseProgressPanel(m_FirstHandle)
				CloseProgressPanel(m_SecHandle)

			End Try

		End Sub

#Region "controls event"

		Private Sub btnTestXML_Click(sender As Object, e As EventArgs) Handles btnTestXML.Click
			LoadMinumumSalaryData()
		End Sub

		Private Sub OngvGavDetail_RowStyle(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs) Handles gvSalaryDetail.RowStyle

			If e.RowHandle >= 0 Then

				Dim rowData = CType(gvSalaryDetail.GetRow(e.RowHandle), MindestLohnViewData)
				If rowData.Label = "Monatslohn" OrElse rowData.Label = "Basisstundenlohn" OrElse rowData.Label = "Mindeststundenlohn mit 13. Monatslohn" Then
					e.Appearance.Font = New Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
					e.Appearance.BackColor = Color.LightBlue
					e.Appearance.BackColor2 = Color.White
				End If

			End If

		End Sub


#End Region


#Region "Helpers"

		Private Function ShowProgressPanel(ByVal ctl As Control) As IOverlaySplashScreenHandle
			Dim handle = SplashScreenManager.ShowOverlayForm(ctl)
			Return handle
		End Function

		Private Sub CloseProgressPanel(ByVal handle As IOverlaySplashScreenHandle)
			If Not handle Is Nothing Then SplashScreenManager.CloseOverlayForm(handle)
		End Sub


#End Region


#Region "private class"

		Private Class MindestLohnViewData
			Public Property Label As String
			Public Property VariabelnText As String
			Public Property Unit As String
			Public Property Amount As String

		End Class

		Private Class PublicationNewsViewData
			Inherits PublicationNewsData

			Public Property ID As Integer?
			Public Property Viewed As Boolean?
			Public Property CreatedFrom As String
			Public Property CreatedOn As DateTime?

		End Class

#End Region

		Private Sub OnbbiSave_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSave.ItemClick
			m_PVLSelectedData = BuildGAVString()

			Me.Close()

		End Sub

		Private Sub lblLinkToPVLData_Click(sender As Object, e As EventArgs) Handles lblLinkToPVLData.Click
			If String.IsNullOrWhiteSpace(lblLinkToPVLData.Tag) Then Return

			Process.Start(lblLinkToPVLData.Tag)

		End Sub

		Private Function BuildGAVString() As String
			Dim result As String = String.Empty
			Dim strData2Save As String
			Dim strCategoryLbl As String = String.Empty
			Dim bGetLOData As Boolean = True
			Dim loDetail = New List(Of String)

			Dim criteriaList = GetListOfGivenCriteriasData()

			Dim pvlData = SelectedPVLData
			If pvlData Is Nothing Then Return result

			Dim pvlEditionData = SelectedPVLEditionData
			If pvlEditionData Is Nothing Then Return result
			Dim salaryParam = BuildSalaryParameterString()
			If String.IsNullOrWhiteSpace(m_cantonValue) Then m_cantonValue = CustomerCanton
			strData2Save = "GAVNr:{0}¦MetaNr:{1}¦CalNr:{2}¦CatNr:{3}¦CatBaseNr:{4}¦CatValueNr:{5}¦LONr:{6}¦Kanton:{7}¦Beruf:{8}¦"
			strData2Save = String.Format(strData2Save,
									 pvlData.ContractNumber,
									 m_ContractVersionNumber,
									 m_ContractEditionNumber,
									 0,
									 0,
									 0,
									 salaryParam,
									 m_cantonValue,
									 pvlData.ContractName
									 )


			Try
				Dim j As Integer = 0
				For j = 0 To 12
					Dim strCboText As String = String.Empty
					Dim ctl As DevExpress.XtraEditors.LabelControl
					Dim ctlValue As DevExpress.XtraEditors.LabelControl
					Dim lue As DevExpress.XtraEditors.LookUpEdit
					Dim catData = SelectedAssignedPVLCategoryData(j)

					Select Case j
						Case 0
							ctl = lblCat0
							ctlValue = lblCatValue0
							lue = lue0
						Case 1
							ctl = lblCat1
							ctlValue = lblCatValue1
							lue = lue1
						Case 2
							ctl = lblCat2
							ctlValue = lblCatValue2
							lue = lue2
						Case 3
							ctl = lblCat3
							ctlValue = lblCatValue3
							lue = lue3
						Case 4
							ctl = lblCat4
							ctlValue = lblCatValue4
							lue = lue4
						Case 5
							ctl = lblCat5
							ctlValue = lblCatValue5
							lue = lue5
						Case 6
							ctl = lblCat6
							ctlValue = lblCatValue6
							lue = lue6
						Case 7
							ctl = lblCat7
							ctlValue = lblCatValue7
							lue = lue7
						Case 8
							ctl = lblCat8
							ctlValue = lblCatValue8
							lue = lue8
						Case 9
							ctl = lblCat9
							ctlValue = lblCatValue9
							lue = lue9
						Case 10
							ctl = lblCat10
							ctlValue = lblCatValue10
							lue = lue10
						Case 11
							ctl = lblCat11
							ctlValue = lblCatValue11
							lue = lue11
						Case 12
							ctl = lblCat12
							ctlValue = lblCatValue12
							lue = lue12


						Case Else
							Return False

					End Select
					If Not lue.Visible Then Exit For
					If lue.EditValue Is Nothing Then
						'liEmptyFields.Add(ctl.Text)
					End If
					If catData Is Nothing Then
						strCategoryLbl &= String.Format("{0}:{1}¦", ctl.Text.Split("("c)(0).Trim, String.Empty)
					Else
						strCboText = m_HtmlUtil.ConvertToPlainText(catData.Value)
						If catData.CriteriaListEntryID = 4231138 Then strCboText = strCboText.Split("(")(0).Trim
						strCategoryLbl &= String.Format("{0}:{1}¦", ctl.Text.Split("("c)(0).Trim, strCboText)

						' chemisch pharmazeutische...
						If catData.CriteriaListEntryID.GetValueOrDefault(0) = 6741 Then bGetLOData = False
					End If

				Next

				Dim gavEinstufung As String = strCategoryLbl.Replace("¦", "#")
				If pvlData.ContractNumber = 815001 Then
					gavEinstufung = gavEinstufung.Replace(":alle anderen Branchen (Achtung: Lohn- und Arbeitszeitbestimmungen der allgemeinverbindlich erklärten und der im Anhang 1 aufgeführten GAV haben Vorrang.)", ":alle anderen Branchen")
					gavEinstufung = gavEinstufung.Replace(":autres branches (attention: les dispositions concernant le salaire et le temps de travail des CCT étendues ou listées en annexe 1 sont prioritaires.", ":autres branches")
					gavEinstufung = gavEinstufung.Replace(":altri rami (attenzione: le disposizioni del salario e la durata di lavoro dei CCL dichiarati d’obbligatorietà generale e di cui all'appendice 1 della replica CCL hanno la precedenza.", ":altri rami")
				End If

				For i As Integer = criteriaList.Count + 8 To 18
					strCategoryLbl &= String.Format("Res_{0}:{1}¦", i, If(i = 18, gavEinstufung, String.Empty))
				Next

				strCategoryLbl &= String.Format("PublicationDate:{0:d}¦", pvlEditionData.Created)


				Dim keyValuePairs = strCategoryLbl.Split("¦")

				Dim tuples As New List(Of Tuple(Of String, String))
				For Each keyValue In keyValuePairs

					Dim tuple As Tuple(Of String, String) = SplitKeyValue(keyValue)
					If Not tuple Is Nothing Then tuples.Add(tuple)
				Next

				Trace.WriteLine(String.Format("strCategoryLbl: {0}", strCategoryLbl))

			Catch ex As Exception
				m_UtilityUI.ShowErrorDialog(ex.ToString)

				strData2Save = String.Empty
				Return String.Empty

			End Try
			strData2Save &= String.Format("{0}", strCategoryLbl)


			Try
				Dim loString As String = "Monatslohn:{0}¦FeiertagJahr:{1}¦FerienJahr:{2}¦13.Lohn:{3}¦"
				loDetail.Add(String.Format(loString,
										   m_SalaryData.Salary.MonthSalary,
										   m_SalaryData.Salary.PublicHolidays,
										   m_SalaryData.Salary.VacationDays,
										   m_SalaryData.Salary.Has13thMonthSalary.GetValueOrDefault(False)))

				loString = "BasisLohn:{0}¦FerienBetrag:{1}¦FerienProz:{2}¦FeierBetrag:{3}¦FeierProz:{4}¦"
				loDetail.Add(String.Format(loString,
										   If(bGetLOData, m_SalaryData.Salary.BasicSalary.GetValueOrDefault(0), "1"),
										   m_SalaryData.Salary.VacationCompensation,
										   m_SalaryData.Salary.PCTVacationCompensation,
										   m_SalaryData.Salary.PublicHolidaysCompensation,
										   m_SalaryData.Salary.PCTHolidaysCompensation))

				loString = "13.Betrag:{0}¦13.Proz:{1}¦CalcFerien:{2}¦CalcFeier:{3}¦Calc13:{4}¦"
				loDetail.Add(String.Format(loString,
										   m_SalaryData.Salary.Compensation13thSalary,
										   m_SalaryData.Salary.PCT13thMonthSalary,
										   m_SalaryData.Salary.VacationCalculationType,
										   m_SalaryData.Salary.PublicHolidaysCalculationType,
										   m_SalaryData.Salary.Month13thSalaryCalculationType))

				Dim totalAmount As Decimal = m_SalaryData.Salary.BasicSalary.GetValueOrDefault(0) +
					m_SalaryData.Salary.VacationCompensation.GetValueOrDefault(0) +
					m_SalaryData.Salary.PublicHolidaysCompensation.GetValueOrDefault(0) +
					m_SalaryData.Salary.Compensation13thSalary.GetValueOrDefault(0)
				loString = "StdLohn:{0}¦FARAN:{1}¦FARAG:{2}¦VAN:{3}¦VAG:{4}¦"
				loDetail.Add(String.Format(loString,
										   If(bGetLOData, totalAmount, "1"),
										   m_SalaryData.Salary.ERWContribution.GetValueOrDefault(0) * 100,
										   m_SalaryData.Salary.EREContribution.GetValueOrDefault(0) * 100,
										   "0.7",
										   "0.3"))
				Dim weekHour As Decimal?
				Dim monthHour As Decimal?
				Dim yearHour As Decimal?

				If Not m_CurrentPVLHourData Is Nothing AndAlso m_CurrentPVLHourData.stdweek.GetValueOrDefault(0) > 0 Then
					weekHour = m_CurrentPVLHourData.stdweek
					monthHour = m_CurrentPVLHourData.stdmonth
					yearHour = m_CurrentPVLHourData.stdyear
				End If
				If Not txtStdWeek.EditValue Is Nothing AndAlso txtStdWeek.EditValue > 0 Then
					weekHour = txtStdWeek.EditValue
				End If

				loString = "StdWeek:{0}¦StdMonth:{1}¦StdYear:{2}¦IsPVL:{3}¦"
				loDetail.Add(String.Format(loString,
										   weekHour.GetValueOrDefault(40),
										   monthHour.GetValueOrDefault(176),
										   yearHour.GetValueOrDefault(2260),
										   If(pvlData.ContractNumber = 815001, 1, 0)))

				loString = "_WAG:{0}¦_WAN:{1}¦_WAG_S:{2}¦_WAN_S:{3}¦_WAG_J:{4}¦_WAN_J:{5}¦"
				loDetail.Add(String.Format(loString,
										   0,
										   0,
										   0,
										   0,
										   0,
										   0))

				loString = "_VAG:{0}¦_VAN:{1}¦_VAG_S:{2}¦_VAN_S:{3}¦_VAG_J:{4}¦_VAN_J:{5}¦"
				loDetail.Add(String.Format(loString,
										   0,
										   0,
										   0,
										   0,
										   0,
										   0))

				loString = "_FAG:{0}¦_FAN:{1}¦BauQ12:{2}¦iFANCalc:{3}¦bFANWithBVG:false¦"
				loDetail.Add(String.Format(loString,
										   0,
										   0,
										   "0",
										   0))

				strData2Save &= String.Join("", loDetail.ToList())

				result = strData2Save


			Catch ex As Exception
				m_UtilityUI.ShowErrorDialog(ex.ToString)
				strData2Save = String.Empty

				Return String.Empty

			End Try


			Return result

		End Function

		Private Function SplitKeyValue(ByVal keyValuePair As String) As Tuple(Of String, String)

			Dim result As Tuple(Of String, String) = Nothing

			If String.IsNullOrWhiteSpace(keyValuePair) Then
				Return Nothing
			End If

			Dim indexOfFirstColon = keyValuePair.IndexOf(":"c)
			If indexOfFirstColon < 0 Then Return result

			Dim key As String = keyValuePair.Substring(0, indexOfFirstColon)
			Dim value As String = keyValuePair.Substring(indexOfFirstColon + 1, keyValuePair.Length - (indexOfFirstColon + 1))
			If value.IndexOf("|") > 0 Then
				indexOfFirstColon = value.IndexOf("|")
				value = value.Substring(0, indexOfFirstColon)
			End If

			result = New Tuple(Of String, String)(key, value)

			Return result
		End Function


		Private Function BuildSalaryParameterString() As String
			Dim result As String = String.Empty

			For i As Integer = 0 To 12
				Dim lue As DevExpress.XtraEditors.LookUpEdit

				Select Case i
					Case 0
						lue = lue0
					Case 1
						lue = lue1
					Case 2
						lue = lue2
					Case 3
						lue = lue3
					Case 4
						lue = lue4
					Case 5
						lue = lue5
					Case 6
						lue = lue6
					Case 7
						lue = lue7
					Case 8
						lue = lue8
					Case 9
						lue = lue9
					Case 10
						lue = lue10
					Case 11
						lue = lue11
					Case 12
						lue = lue12


					Case Else
						Continue For

				End Select

				If Not lue.Visible Then Continue For
				If i = m_CantonID Then m_cantonValue = lue.Text

				result = String.Format("{0}{1} {2}", result, If(String.IsNullOrWhiteSpace(result), "", ","), lue.EditValue)

			Next


			Return result

		End Function

		Private Sub CustomDrawEmptyForeground(ByVal sender As Object, ByVal e As CustomDrawEventArgs)
			Dim s As String = m_Translate.GetSafeTranslationValue("Keine Daten sind vorhanden")

			Try
				s = m_Translate.GetSafeTranslationValue(s)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			End Try

			Dim font As Font = New Font("Calibri", 8, FontStyle.Regular)
			Dim r As RectangleF = New RectangleF(e.Bounds.Left + 5, e.Bounds.Top + 5, e.Bounds.Width - 5, e.Bounds.Height - 5)
			e.Graphics.DrawString(s, font, Brushes.Black, r)

		End Sub

		Private Function IsCustomerServiceAllowed(ByVal serviceName As String) As Boolean
			Dim providerObj As New ProviderData(m_InitializationData)
			Dim result = providerObj.IsCustomerAllowedToUseServiceData(m_InitializationData.MDData.MDGuid, serviceName)

			Return result

		End Function

		Private Sub grpGAVCategory_Paint(sender As Object, e As PaintEventArgs) Handles grpGAVCategory.Paint

		End Sub
	End Class

End Namespace
