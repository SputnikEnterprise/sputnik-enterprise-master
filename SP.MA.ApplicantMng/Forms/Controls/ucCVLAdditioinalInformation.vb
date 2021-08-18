
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Applicant
Imports SP.DatabaseAccess.CVLizer.DataObjects

Imports SP.Infrastructure.Logging
Imports System.ComponentModel
Imports SPProgUtility.Mandanten
Imports SP.Infrastructure.UI

Imports DevExpress.XtraSplashScreen
Imports SPProgUtility.CommonXmlUtility

Imports SP.DatabaseAccess.CVLizer
Imports SP.Infrastructure.Initialization
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid.Views.Layout
Imports DevExpress.XtraLayout.Utils
Imports DevExpress.XtraGrid.Views.Layout.Events
Imports SP.MA.ApplicantMng.SPApplicationWebService
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.Internal.Automations.SPApplicationWebService

Namespace UI

	Public Class ucCVLAdditioinalInformation

#Region "Private Consts"

		Private Const DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPApplication.asmx"

#End Region

#Region "private fields"

		''' <summary>
		''' Service Uri of Sputnik notification util webservice.
		''' </summary>
		Private m_ApplicationUtilWebServiceUri As String

		Private m_AppDatabaseAccess As IAppDatabaseAccess

		''' <summary>
		''' The cv data access object.
		''' </summary>
		Private m_AppCVDatabaseAccess As IAppCvDatabaseAccess

		''' <summary>
		''' The cv data access object.
		''' </summary>
		Private m_CVLDatabaseAccess As ICVLizerDatabaseAccess


		Private m_applicantDb As String
		'Private m_customerID As String

		Private m_CVLizerXMLData As CVLizerXMLData
		Private m_CurrentFileExtension As String


		Private m_AdditionalData As AdditionalInfoLocalViewData
		Private m_AddID As Integer
		Private m_PhaseID As Integer

		''' <summary>
		''' Settings xml.
		''' </summary>
		Private m_MandantSettingsXml As SettingsXml

		''' <summary>
		''' List of user controls.
		''' </summary>
		Private m_connectionString As String

		Private m_mandant As Mandant
		Private m_path As SPProgUtility.ProgPath.ClsProgPath


#End Region


#Region "constructor"

		Public Sub New()

			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()

		End Sub


#End Region


#Region "private property"

		''' <summary>
		''' Gets the selected work phase data.
		''' </summary>
		Private ReadOnly Property SelectedAddtionalViewData As AdditionalInfoLocalViewData
			Get
				Dim grdView = TryCast(grdAddtional.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (grdView Is Nothing) Then

					Dim selectedRows = grdView.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim viewData = CType(grdView.GetRow(selectedRows(0)), AdditionalInfoLocalViewData)
						Return viewData
					End If

				End If

				Return Nothing
			End Get

		End Property


#End Region

#Region "Public Properties"

		''' <summary>
		''' Activates the control.
		''' </summary>
		''' <returns>Boolean value indicating success.</returns>
		Public Overrides Function ActivateCVLWork(ByVal cvlProfileID As Integer?, ByVal cvlWorkID As Integer?) As Boolean
			m_SuppressUIEvents = True

			Dim success = True

			If (Not m_CVLProfileID.GetValueOrDefault(0) = cvlProfileID) Then
				success = success AndAlso LoadAssignedAdditionalData(cvlProfileID, cvlWorkID)

				m_EmployeeNumber = IIf(success, EmployeeNumber, 0)
			End If

			m_SuppressUIEvents = False

			Return success
		End Function

		''' <summary>
		''' Boolean flag indicating if employee data is loaded.
		''' </summary>
		Public ReadOnly Property IsWorkDataLoaded As Boolean
			Get
				Return m_CVLProfileID.HasValue
			End Get

		End Property

		''' <summary>
		''' Deactivates the control.
		''' </summary>
		Public Overrides Sub Deactivate()
			' Do nothing
		End Sub

		''' <summary>
		''' Validated data.
		''' </summary>
		Public Overrides Function ValidateData() As Boolean
			' Do nothing
			Return True
		End Function

		''' <summary>
		''' Merges the employee master data.
		''' </summary>
		''' <param name="employeeMasterData">The employee master data object where the data gets filled into.</param>
		''' <param name="forceMerge">Optional flag indicating if the merge should be forced altough no data has been loaded. </param>
		Public Overrides Sub MergeEmployeeMasterData(ByVal employeeMasterData As EmployeeMasterData, Optional forceMerge As Boolean = False)
			If ((IsEmployeeDataLoaded AndAlso
					m_EmployeeNumber = employeeMasterData.EmployeeNumber) Or forceMerge) Then
				' No employee master data (table Mitarbeiter) to merge.
			End If
		End Sub

		''' <summary>
		'''  Merges the employee contact other data (MASonstiges).
		''' </summary>
		''' <param name="employeeOtherData">The employee other data.</param>
		Public Overrides Sub MergeEmployeeOtherData(ByVal employeeOtherData As EmployeeOtherData)
			If (IsEmployeeDataLoaded AndAlso m_EmployeeNumber = employeeOtherData.EmployeeNumber) Then
				' No employee other data (MASonstiges) to merge
			End If
		End Sub

		''' <summary>
		'''  Merges the employee contact comm data.
		''' </summary>
		''' <param name="employeeContactCommData">The employee contact comm data.</param>
		Public Overrides Sub MergeEmployeeContactCommData(ByVal employeeContactCommData As EmployeeContactComm)
			If (IsEmployeeDataLoaded AndAlso m_EmployeeNumber = employeeContactCommData.EmployeeNumber) Then
				' No employee other data (MA_KontaktKomm) to merge
			End If
		End Sub

		''' <summary>
		''' Cleanup control.
		''' </summary>
		Public Overrides Sub CleanUp()
			' Do nothing
		End Sub


#End Region


#Region "public Methodes"

		Public Overrides Sub InitWithConfigurationData(ByVal initializationClass As SP.Infrastructure.Initialization.InitializeClass, ByVal translationHelper As SP.Infrastructure.Initialization.TranslateValuesHelper)
			MyBase.InitWithConfigurationData(initializationClass, translationHelper)

			'm_ApplicationUtilWebServiceUri = DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI
			Dim domainName = m_InitializationData.MDData.WebserviceDomain
			m_ApplicationUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI)

		End Sub

		Public Function LoadAssignedAdditionalData(ByVal cvlProfileID As Integer?, ByVal cvlAddID As Integer?) As Boolean
			Dim result As Boolean = True

			m_AdditionalData = New AdditionalInfoLocalViewData
			m_CVLProfileID = cvlProfileID
			m_AddID = cvlAddID

			result = result AndAlso PerformAdditionalDataWebservice(m_CVLProfileID, m_AddID)

			Return True

		End Function


#End Region


#Region "reset"

		Public Overrides Sub Reset()

			m_CVLProfileID = Nothing
			m_AddID = Nothing

			ResetLanguageFields()
			ResetAdditionalFields()

		End Sub

		Private Sub ResetLanguageFields()
			Dim repItemBoolean As New RepositoryItemCheckEdit()
			Dim repItemEdit As New RepositoryItemMemoEdit()

			lvLanguage.Columns.Clear()
			With lvLanguage
				.OptionsView.ShowCardBorderIfCaptionHidden = True
				.OptionsView.ShowCardCaption = True
				.OptionsView.ShowHeaderPanel = False
				.OptionsCustomization.AllowSort = False
				.OptionsCustomization.AllowFilter = False
				.Appearance.FieldCaption.Font = New System.Drawing.Font(.Appearance.FieldCaption.Font.Name, .Appearance.FieldCaption.Font.Size, System.Drawing.FontStyle.Bold)
				.Appearance.FieldCaption.Options.UseFont = True
				.Appearance.FieldCaption.Options.UseTextOptions = True
				.Appearance.FieldCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
				.Appearance.FieldCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
				.Appearance.FieldValue.Options.UseTextOptions = True
				.Appearance.FieldValue.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
				.Appearance.FieldValue.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top

				.OptionsView.ViewMode = LayoutViewMode.MultiRow
				.OptionsView.ShowHeaderPanel = False
				'.CardMinSize = New Size(200, grdLanguage.Height - 100)
			End With
			lvLanguage.CardCaptionFormat = m_Translate.GetSafeTranslationValue("[Sprachen: {0} von {1}]")


			Dim columnCompetences As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnCompetences.OptionsColumn.AllowEdit = False
			columnCompetences.Caption = m_Translate.GetSafeTranslationValue("Sprache")
			columnCompetences.Name = "Code"
			columnCompetences.FieldName = "Code"
			lvLanguage.Columns.Add(columnCompetences)

			Dim columnInterests As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnInterests.OptionsColumn.AllowEdit = False
			columnInterests.Caption = m_Translate.GetSafeTranslationValue("Level")
			columnInterests.Name = "Name"
			columnInterests.FieldName = "Name"
			lvLanguage.Columns.Add(columnInterests)

			For Each col As DevExpress.XtraGrid.Columns.LayoutViewColumn In lvLanguage.Columns
				col.Visible = True
			Next

			grdLanguage.DataSource = Nothing

		End Sub

		Private Sub ResetAdditionalFields()
			Dim repItemBoolean As New RepositoryItemCheckEdit()
			Dim repItemEdit As New RepositoryItemMemoEdit()

			lvAddtional.Columns.Clear()
			With lvAddtional
				.OptionsView.ShowCardBorderIfCaptionHidden = True
				.OptionsView.ShowCardCaption = True
				.OptionsView.ShowHeaderPanel = False
				.OptionsCustomization.AllowSort = False
				.OptionsCustomization.AllowFilter = False
				.Appearance.FieldCaption.Font = New System.Drawing.Font(.Appearance.FieldCaption.Font.Name, .Appearance.FieldCaption.Font.Size, System.Drawing.FontStyle.Bold)
				.Appearance.FieldCaption.Options.UseFont = True
				.Appearance.FieldCaption.Options.UseTextOptions = True
				.Appearance.FieldCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
				.Appearance.FieldCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
				.Appearance.FieldValue.Options.UseTextOptions = True
				.Appearance.FieldValue.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
				.Appearance.FieldValue.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top

				.OptionsView.ViewMode = LayoutViewMode.SingleRecord
				.OptionsSingleRecordMode.StretchCardToViewWidth = True
				.OptionsSingleRecordMode.StretchCardToViewHeight = True
				.OptionsView.ShowHeaderPanel = False

			End With
			lvAddtional.CardCaptionFormat = m_Translate.GetSafeTranslationValue("[Sonstige Informationen]")

			repItemEdit.ScrollBars = ScrollBars.Both
			repItemEdit.LinesCount = 3

			Dim columnID As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnID.OptionsColumn.AllowEdit = False
			columnID.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnID.Name = "ID"
			columnID.FieldName = "ID"
			lvAddtional.Columns.Add(columnID)
			columnID.LayoutViewField.Visibility = LayoutVisibility.Never

			Dim columnMilitaryServiceViewData As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnMilitaryServiceViewData.OptionsColumn.AllowEdit = False
			columnMilitaryServiceViewData.Caption = m_Translate.GetSafeTranslationValue("Zivildienst abgeleistet")
			columnMilitaryServiceViewData.Name = "MilitaryServiceViewData"
			columnMilitaryServiceViewData.FieldName = "MilitaryServiceViewData"
			lvAddtional.Columns.Add(columnMilitaryServiceViewData)

			Dim columnCompetences As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			'columnCompetences.OptionsColumn.AllowEdit = False
			columnCompetences.Caption = m_Translate.GetSafeTranslationValue("Kompetenzen")
			columnCompetences.Name = "Competences"
			columnCompetences.FieldName = "Competences"
			columnCompetences.ColumnEdit = repItemEdit
			columnCompetences.UnboundType = DevExpress.Data.UnboundColumnType.String
			lvAddtional.Columns.Add(columnCompetences)
			columnCompetences.LayoutViewField.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom

			Dim columnInterests As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnInterests.OptionsColumn.AllowEdit = False
			columnInterests.Caption = m_Translate.GetSafeTranslationValue("Interessen")
			columnInterests.Name = "Interests"
			columnInterests.FieldName = "Interests"
			columnInterests.ColumnEdit = repItemEdit
			columnInterests.UnboundType = DevExpress.Data.UnboundColumnType.String
			lvAddtional.Columns.Add(columnInterests)
			columnInterests.LayoutViewField.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom

			Dim columnAdditionals As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnAdditionals.OptionsColumn.AllowEdit = False
			columnAdditionals.Caption = m_Translate.GetSafeTranslationValue("Zusatzinfos")
			columnAdditionals.Name = "Additionals"
			columnAdditionals.FieldName = "Additionals"
			columnAdditionals.ColumnEdit = repItemEdit
			columnAdditionals.UnboundType = DevExpress.Data.UnboundColumnType.String
			lvAddtional.Columns.Add(columnAdditionals)
			columnAdditionals.LayoutViewField.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom

			Dim columnDrivingLicenceViewData As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnDrivingLicenceViewData.OptionsColumn.AllowEdit = False
			columnDrivingLicenceViewData.Caption = m_Translate.GetSafeTranslationValue("Führerschein")
			columnDrivingLicenceViewData.Name = "DrivingLicenceViewData"
			columnDrivingLicenceViewData.FieldName = "DrivingLicenceViewData"
			columnDrivingLicenceViewData.ColumnEdit = repItemEdit
			columnDrivingLicenceViewData.UnboundType = DevExpress.Data.UnboundColumnType.String
			lvAddtional.Columns.Add(columnDrivingLicenceViewData)
			columnDrivingLicenceViewData.LayoutViewField.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom

			Dim columnUndatedSkillViewData As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnUndatedSkillViewData.OptionsColumn.AllowEdit = False
			columnUndatedSkillViewData.Caption = m_Translate.GetSafeTranslationValue("Nichtdatierte Fähigkeiten (Skills)")
			columnUndatedSkillViewData.Name = "UndatedSkillViewData"
			columnUndatedSkillViewData.FieldName = "UndatedSkillViewData"
			columnUndatedSkillViewData.ColumnEdit = repItemEdit
			columnUndatedSkillViewData.UnboundType = DevExpress.Data.UnboundColumnType.String
			lvAddtional.Columns.Add(columnUndatedSkillViewData)
			columnUndatedSkillViewData.LayoutViewField.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom

			Dim columnUndatedOperationAreViewData As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnUndatedOperationAreViewData.OptionsColumn.AllowEdit = False
			columnUndatedOperationAreViewData.Caption = m_Translate.GetSafeTranslationValue("Nichtdatierte Tätigkeitsgebiete")
			columnUndatedOperationAreViewData.Name = "UndatedOperationAreViewData"
			columnUndatedOperationAreViewData.FieldName = "UndatedOperationAreViewData"
			columnUndatedOperationAreViewData.ColumnEdit = repItemEdit
			columnUndatedOperationAreViewData.UnboundType = DevExpress.Data.UnboundColumnType.String
			lvAddtional.Columns.Add(columnUndatedOperationAreViewData)
			columnUndatedOperationAreViewData.LayoutViewField.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom

			Dim columnUndatedIndustryViewData As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnUndatedIndustryViewData.OptionsColumn.AllowEdit = False
			columnUndatedIndustryViewData.Caption = m_Translate.GetSafeTranslationValue("Nichtdatierte Branchen")
			columnUndatedIndustryViewData.Name = "UndatedIndustryViewData"
			columnUndatedIndustryViewData.FieldName = "UndatedIndustryViewData"
			columnUndatedIndustryViewData.ColumnEdit = repItemEdit
			columnUndatedIndustryViewData.UnboundType = DevExpress.Data.UnboundColumnType.String
			lvAddtional.Columns.Add(columnUndatedIndustryViewData)
			columnUndatedIndustryViewData.LayoutViewField.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom

			Dim columnInternetResourcesViewData As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnInternetResourcesViewData.OptionsColumn.AllowEdit = False
			columnInternetResourcesViewData.Caption = m_Translate.GetSafeTranslationValue("InternetResourcesViewData")
			columnInternetResourcesViewData.Name = "InternetResourcesViewData"
			columnInternetResourcesViewData.FieldName = "InternetResourcesViewData"
			columnInternetResourcesViewData.ColumnEdit = repItemEdit
			columnInternetResourcesViewData.UnboundType = DevExpress.Data.UnboundColumnType.String
			lvAddtional.Columns.Add(columnInternetResourcesViewData)
			columnInternetResourcesViewData.LayoutViewField.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom

			For Each col As DevExpress.XtraGrid.Columns.LayoutViewColumn In lvAddtional.Columns
				col.Visible = True
			Next

			grdAddtional.DataSource = Nothing

		End Sub

#End Region


		''' <summary>
		'''  Performs loading cvl personal data.
		''' </summary>
		Private Function PerformAdditionalDataWebservice(ByVal profileID As Integer?, ByVal personalID As Integer?) As Boolean
			Dim result As Boolean = True

			Dim webservice As New SPApplicationWebService.SPApplicationSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

			' Read data over webservice
			Dim searchResult = webservice.LoadCVLAdditionalInfoViewData(m_customerID, profileID, personalID)
			If searchResult Is Nothing Then
				m_Logger.LogError(String.Format("additional informations could not be loaded from webservice! {0} | {1} | {2}", m_customerID, profileID, personalID))

				Return False
			End If
			m_AdditionalData = New AdditionalInfoLocalViewData With {.Additionals = searchResult.Additionals,
							.Competences = searchResult.Competences,
							.DrivingLicences = searchResult.DrivingLicences, .ID = searchResult.ID,
							.Interests = searchResult.Interests, .InternetResources = searchResult.InternetResources,
							.Languages = searchResult.Languages,
							.MilitaryService = searchResult.MilitaryService,
							.UndatedIndustries = searchResult.UndatedIndustries,
							.UndatedOperationArea = searchResult.UndatedOperationArea,
							.UndatedSkills = searchResult.UndatedSkills
						}

			Dim listAdditionalDataSource As BindingList(Of AdditionalInfoLocalViewData) = New BindingList(Of AdditionalInfoLocalViewData)
			listAdditionalDataSource.Add(New AdditionalInfoLocalViewData With {.Additionals = searchResult.Additionals,
							.Competences = searchResult.Competences,
							.DrivingLicences = searchResult.DrivingLicences, .ID = searchResult.ID,
							.Interests = searchResult.Interests, .InternetResources = searchResult.InternetResources,
							.Languages = searchResult.Languages,
							.MilitaryService = searchResult.MilitaryService,
							.UndatedIndustries = searchResult.UndatedIndustries,
							.UndatedOperationArea = searchResult.UndatedOperationArea,
							.UndatedSkills = searchResult.UndatedSkills
						})

			grdAddtional.DataSource = listAdditionalDataSource

			Dim listDataSource As BindingList(Of DatabaseAccess.CVLizer.DataObjects.CodeNameViewData) = New BindingList(Of DatabaseAccess.CVLizer.DataObjects.CodeNameViewData)
			If Not m_AdditionalData.Languages Is Nothing Then
				For Each language In m_AdditionalData.Languages
					listDataSource.Add(New DatabaseAccess.CVLizer.DataObjects.CodeNameViewData With {.Code = language.CodeName, .Name = language.Level.CodeName})
				Next
			End If

			grdLanguage.DataSource = listDataSource

			Return Not (m_AdditionalData Is Nothing)

		End Function

		Sub OngvgvAddtional_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)
			Dim success As Boolean = True

			Dim data = SelectedAddtionalViewData
			If data Is Nothing Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Education Phase-Daten konnte nicht geladen werden."))
				Return
			End If

			success = success AndAlso DisplayAssignedEducationPhaseData(data)

		End Sub

		Private Function DisplayAssignedEducationPhaseData(ByVal data As AdditionalInfoLocalViewData) As Boolean
			Dim success As Boolean = True

			Return success

		End Function

		Private Sub OnlvAddtional_CustomFieldValueStyle(ByVal sender As System.Object, ByVal e As LayoutViewFieldValueStyleEventArgs) Handles lvAddtional.CustomFieldValueStyle

			If e Is Nothing Then Return
			Try

				If Not (e.Column.FieldName <> "Competences" OrElse e.Column.FieldName <> "Interests" OrElse e.Column.FieldName <> "Additionals" OrElse e.Column.FieldName <> "DrivingLicenceViewData" _
						OrElse e.Column.FieldName <> "UndatedSkillViewData" OrElse e.Column.FieldName <> "UndatedOperationAreViewData" OrElse e.Column.FieldName <> "UndatedIndustryViewData" OrElse e.Column.FieldName <> "InternetResourcesViewData") Then Return

				Dim view As LayoutView = TryCast(sender, LayoutView)
				If view Is Nothing Then Return
				Dim showCompetence As Boolean = Not (view.GetRowCellValue(e.RowHandle, "Competences") Is Nothing OrElse String.IsNullOrWhiteSpace(view.GetRowCellValue(e.RowHandle, "Competences").ToString()))
				Dim showInterests As Boolean = Not (view.GetRowCellValue(e.RowHandle, "Interests") Is Nothing OrElse String.IsNullOrWhiteSpace(view.GetRowCellValue(e.RowHandle, "Interests").ToString()))
				Dim showAdditionals As Boolean = Not (view.GetRowCellValue(e.RowHandle, "Additionals") Is Nothing OrElse String.IsNullOrWhiteSpace(view.GetRowCellValue(e.RowHandle, "Additionals").ToString()))
				Dim showDrivingLicenceViewData As Boolean = Not (view.GetRowCellValue(e.RowHandle, "DrivingLicenceViewData") Is Nothing OrElse String.IsNullOrWhiteSpace(view.GetRowCellValue(e.RowHandle, "DrivingLicenceViewData").ToString()))
				Dim showUndatedSkillViewData As Boolean = Not (view.GetRowCellValue(e.RowHandle, "UndatedSkillViewData") Is Nothing OrElse String.IsNullOrWhiteSpace(view.GetRowCellValue(e.RowHandle, "UndatedSkillViewData").ToString()))
				Dim showUndatedOperationAreViewData As Boolean = Not (view.GetRowCellValue(e.RowHandle, "UndatedOperationAreViewData") Is Nothing OrElse String.IsNullOrWhiteSpace(view.GetRowCellValue(e.RowHandle, "UndatedOperationAreViewData").ToString()))
				Dim showUndatedIndustryViewData As Boolean = Not (view.GetRowCellValue(e.RowHandle, "UndatedIndustryViewData") Is Nothing OrElse String.IsNullOrWhiteSpace(view.GetRowCellValue(e.RowHandle, "UndatedIndustryViewData").ToString()))
				Dim showInternetResourcesViewData As Boolean = Not (view.GetRowCellValue(e.RowHandle, "InternetResourcesViewData") Is Nothing OrElse String.IsNullOrWhiteSpace(view.GetRowCellValue(e.RowHandle, "InternetResourcesViewData").ToString()))

				If Not view.Columns("Competences") Is Nothing Then view.Columns("Competences").LayoutViewField.Visibility = If(showCompetence, LayoutVisibility.Always, LayoutVisibility.Never)
				If Not view.Columns("Interests") Is Nothing Then view.Columns("Interests").LayoutViewField.Visibility = If(showInterests, LayoutVisibility.Always, LayoutVisibility.Never)
				If Not view.Columns("Additionals") Is Nothing Then view.Columns("Additionals").LayoutViewField.Visibility = If(showAdditionals, LayoutVisibility.Always, LayoutVisibility.Never)
				If Not view.Columns("DrivingLicenceViewData") Is Nothing Then view.Columns("DrivingLicenceViewData").LayoutViewField.Visibility = If(showDrivingLicenceViewData, LayoutVisibility.Always, LayoutVisibility.Never)
				If Not view.Columns("UndatedSkillViewData") Is Nothing Then view.Columns("UndatedSkillViewData").LayoutViewField.Visibility = If(showUndatedSkillViewData, LayoutVisibility.Always, LayoutVisibility.Never)
				If Not view.Columns("UndatedOperationAreViewData") Is Nothing Then view.Columns("UndatedOperationAreViewData").LayoutViewField.Visibility = If(showUndatedOperationAreViewData, LayoutVisibility.Always, LayoutVisibility.Never)
				If Not view.Columns("UndatedIndustryViewData") Is Nothing Then view.Columns("UndatedIndustryViewData").LayoutViewField.Visibility = If(showUndatedIndustryViewData, LayoutVisibility.Always, LayoutVisibility.Never)
				If Not view.Columns("InternetResourcesViewData") Is Nothing Then view.Columns("InternetResourcesViewData").LayoutViewField.Visibility = If(showInternetResourcesViewData, LayoutVisibility.Always, LayoutVisibility.Never)


			Catch ex As Exception

			End Try

		End Sub



#Region "Helpers Class"

		Public Class AdditionalInfoLocalViewData
			Inherits AdditionalInfoViewDataDTO


#Region "readonly properties"

			Public ReadOnly Property MilitaryServiceViewData As String
				Get
					Dim value As String = "Ja"
					If Not MilitaryService.GetValueOrDefault(False) Then value = "Nein"

					Return value
				End Get
			End Property

			Public ReadOnly Property DrivingLicenceViewData As String
				Get
					If DrivingLicences Is Nothing OrElse DrivingLicences.Count = 0 Then Return Nothing
					Dim value As String = String.Empty
					For Each itm In DrivingLicences
						value &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(value), "", ", "), itm.Lable)
					Next

					Return value
				End Get
			End Property

			Public ReadOnly Property UndatedSkillViewData As String
				Get
					If UndatedSkills Is Nothing OrElse UndatedSkills.Count = 0 Then Return Nothing
					Dim value As String = String.Empty
					For Each itm In UndatedSkills
						value &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(value), "", ", "), itm.Name)
					Next

					Return value
				End Get
			End Property

			Public ReadOnly Property UndatedOperationAreViewData As String
				Get
					If UndatedOperationArea Is Nothing OrElse UndatedOperationArea.Count = 0 Then Return Nothing
					Dim value As String = String.Empty
					For Each itm In UndatedOperationArea
						value &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(value), "", ", "), itm.Name)
					Next

					Return value
				End Get
			End Property

			Public ReadOnly Property UndatedIndustryViewData As String
				Get
					If UndatedIndustries Is Nothing OrElse UndatedIndustries.Count = 0 Then Return Nothing
					Dim value As String = String.Empty
					For Each itm In UndatedIndustries
						value &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(value), "", ", "), itm.Name)
					Next

					Return value
				End Get
			End Property

			Public ReadOnly Property InternetResourcesViewData As String
				Get
					If InternetResources Is Nothing OrElse InternetResources.Count = 0 Then Return Nothing
					Dim value As String = String.Empty
					For Each itm In InternetResources
						value &= String.Format("{0}{1}{2}", If(String.IsNullOrWhiteSpace(value), "", vbNewLine), itm.Title, itm.URL)
					Next

					Return value
				End Get
			End Property

			Public ReadOnly Property LanguageViewData As IEnumerable(Of DatabaseAccess.CVLizer.DataObjects.CodeNameViewData)
				Get
					If Languages Is Nothing OrElse Languages.Count = 0 Then Return Nothing
					Dim value As List(Of DatabaseAccess.CVLizer.DataObjects.CodeNameViewData) = Nothing
					For Each itm In Languages
						Dim data = New DatabaseAccess.CVLizer.DataObjects.CodeNameViewData
						data.Code = itm.CodeName
						data.Name = itm.Level.CodeName

						value.Add(data)
					Next

					Return value
				End Get
			End Property


#End Region


		End Class


#End Region
	End Class

End Namespace
