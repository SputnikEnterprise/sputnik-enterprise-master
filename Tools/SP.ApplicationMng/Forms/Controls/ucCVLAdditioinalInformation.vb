
Imports TrxmlUtility.Xsd
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.Applicant
Imports SP.DatabaseAccess.CVLizer.DataObjects


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
Imports SP.DatabaseAccess.CVLizer
Imports SP.Infrastructure.Initialization
Imports SP.ApplicationMng.GraphicsEditor
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid.Views.Layout
Imports DevExpress.XtraLayout.Utils
Imports DevExpress.XtraGrid.Views.Layout.Events
Imports SP.ApplicationMng.SPApplicationWebService
Imports SP.Main.Notify.SPApplicationWebService

Namespace CVLizer.UI

	Public Class ucCVLAdditioinalInformation

#Region "Private Consts"

		'Private Const DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI As String = "http://asmx.domain.com/wsSPS_services/SPApplication.asmx"

#End Region

#Region "private fields"

		'''' <summary>
		'''' The Initialization data.
		'''' </summary>
		'Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

		'''' <summary>
		'''' Boolean flag indicating if form is initializing.
		'''' </summary>
		'Protected m_SuppressUIEvents As Boolean = False

		'''' <summary>
		'''' The logger.
		'''' </summary>
		'Private Shared m_Logger As ILogger = New Logger()

		''''' <summary>
		''''' The translation value helper.
		''''' </summary>
		''Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

		'''' <summary>
		'''' Service Uri of Sputnik notification util webservice.
		'''' </summary>
		'Private m_ApplicationUtilWebServiceUri As String

		'''' <summary>
		'''' The common data access object.
		'''' </summary>
		'Private m_CommonDatabaseAccess As ICommonDatabaseAccess
		'Private m_AppDatabaseAccess As IAppDatabaseAccess

		'''' <summary>
		'''' The cv data access object.
		'''' </summary>
		'Private m_AppCVDatabaseAccess As IAppCvDatabaseAccess

		'''' <summary>
		'''' The cv data access object.
		'''' </summary>
		'Private m_CVLDatabaseAccess As ICVLizerDatabaseAccess


		Private m_applicantDb As String
		Private m_customerID As String

		Private m_CVLizerXMLData As CVLizerXMLData
		Private m_CurrentFileExtension As String


		Private m_AdditionalData As AdditionalInfoLocalViewData
		Private m_CVLProfileID As Integer
		Private m_AddID As Integer
		Private m_PhaseID As Integer

		'''' <summary>
		'''' Settings xml.
		'''' </summary>
		'Private m_MandantSettingsXml As SettingsXml

		'''' <summary>
		'''' List of user controls.
		'''' </summary>
		'Private m_connectionString As String

		'''' <summary>
		'''' UI Utility functions.
		'''' </summary>
		'Private m_UtilityUI As UtilityUI

		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As SP.Infrastructure.Utility

		Private m_mandant As Mandant
		Private m_path As SPProgUtility.ProgPath.ClsProgPath


#End Region


#Region "constructor"

		Public Sub New() ' ByVal _setting As InitializeClass)

			' Dieser Aufruf ist für den Designer erforderlich.
			DevExpress.UserSkins.BonusSkins.Register()
			DevExpress.Skins.SkinManager.EnableFormSkins()

			'm_InitializationData = _setting
			m_mandant = New Mandant
			m_UtilityUI = New UtilityUI
			m_Utility = New SP.Infrastructure.Utility
			m_path = New SPProgUtility.ProgPath.ClsProgPath
			'm_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)


			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

			Reset()

#If DEBUG Then
			'm_customerID = "EFFE4D79-2AC6-4f9f-839F-1E4E9D6D9E2A"
#End If


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


#Region "public Methodes"

		Public Function LoadAssignedAdditionalData(ByVal cvlProfileID As Integer?, ByVal cvlAddID As Integer?) As Boolean
			Dim result As Boolean = True

			m_CVLProfileID = cvlProfileID
			m_AddID = cvlAddID

			result = result AndAlso PerformAdditionalDataWebservice(m_CVLProfileID, m_AddID)

			Return True

		End Function


#End Region


#Region "reset"

		Private Sub Reset()

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
			lvLanguage.CardCaptionFormat = ("[Sprachen: {0} von {1}]")


			Dim columnCompetences As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnCompetences.OptionsColumn.AllowEdit = False
			columnCompetences.Caption = ("Sprache")
			columnCompetences.Name = "Code"
			columnCompetences.FieldName = "Code"
			lvLanguage.Columns.Add(columnCompetences)

			Dim columnInterests As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnInterests.OptionsColumn.AllowEdit = False
			columnInterests.Caption = ("Level")
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
			lvAddtional.CardCaptionFormat = ("[Sonstige Informationen]")

			repItemEdit.ScrollBars = ScrollBars.Both
			repItemEdit.LinesCount = 3

			Dim columnID As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnID.OptionsColumn.AllowEdit = False
			columnID.Caption = ("ID")
			columnID.Name = "ID"
			columnID.FieldName = "ID"
			lvAddtional.Columns.Add(columnID)
			columnID.LayoutViewField.Visibility = LayoutVisibility.Never

			Dim columnMilitaryServiceViewData As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnMilitaryServiceViewData.OptionsColumn.AllowEdit = False
			columnMilitaryServiceViewData.Caption = ("Zivildienst abgeleistet")
			columnMilitaryServiceViewData.Name = "MilitaryServiceViewData"
			columnMilitaryServiceViewData.FieldName = "MilitaryServiceViewData"
			lvAddtional.Columns.Add(columnMilitaryServiceViewData)

			Dim columnCompetences As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			'columnCompetences.OptionsColumn.AllowEdit = False
			columnCompetences.Caption = ("Kompetenzen")
			columnCompetences.Name = "Competences"
			columnCompetences.FieldName = "Competences"
			columnCompetences.ColumnEdit = repItemEdit
			columnCompetences.UnboundType = DevExpress.Data.UnboundColumnType.String
			lvAddtional.Columns.Add(columnCompetences)
			columnCompetences.LayoutViewField.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom

			Dim columnInterests As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnInterests.OptionsColumn.AllowEdit = False
			columnInterests.Caption = ("Interessen")
			columnInterests.Name = "Interests"
			columnInterests.FieldName = "Interests"
			columnInterests.ColumnEdit = repItemEdit
			columnInterests.UnboundType = DevExpress.Data.UnboundColumnType.String
			lvAddtional.Columns.Add(columnInterests)
			columnInterests.LayoutViewField.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom

			Dim columnAdditionals As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnAdditionals.OptionsColumn.AllowEdit = False
			columnAdditionals.Caption = ("Zusatzinfos")
			columnAdditionals.Name = "Additionals"
			columnAdditionals.FieldName = "Additionals"
			columnAdditionals.ColumnEdit = repItemEdit
			columnAdditionals.UnboundType = DevExpress.Data.UnboundColumnType.String
			lvAddtional.Columns.Add(columnAdditionals)
			columnAdditionals.LayoutViewField.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom

			Dim columnDrivingLicenceViewData As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnDrivingLicenceViewData.OptionsColumn.AllowEdit = False
			columnDrivingLicenceViewData.Caption = ("Führerschein")
			columnDrivingLicenceViewData.Name = "DrivingLicenceViewData"
			columnDrivingLicenceViewData.FieldName = "DrivingLicenceViewData"
			columnDrivingLicenceViewData.ColumnEdit = repItemEdit
			columnDrivingLicenceViewData.UnboundType = DevExpress.Data.UnboundColumnType.String
			lvAddtional.Columns.Add(columnDrivingLicenceViewData)
			columnDrivingLicenceViewData.LayoutViewField.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom

			Dim columnUndatedSkillViewData As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnUndatedSkillViewData.OptionsColumn.AllowEdit = False
			columnUndatedSkillViewData.Caption = ("Nichtdatierte Fähigkeiten (Skills)")
			columnUndatedSkillViewData.Name = "UndatedSkillViewData"
			columnUndatedSkillViewData.FieldName = "UndatedSkillViewData"
			columnUndatedSkillViewData.ColumnEdit = repItemEdit
			columnUndatedSkillViewData.UnboundType = DevExpress.Data.UnboundColumnType.String
			lvAddtional.Columns.Add(columnUndatedSkillViewData)
			columnUndatedSkillViewData.LayoutViewField.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom

			Dim columnUndatedOperationAreViewData As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnUndatedOperationAreViewData.OptionsColumn.AllowEdit = False
			columnUndatedOperationAreViewData.Caption = ("Nichtdatierte Tätigkeitsgebiete")
			columnUndatedOperationAreViewData.Name = "UndatedOperationAreViewData"
			columnUndatedOperationAreViewData.FieldName = "UndatedOperationAreViewData"
			columnUndatedOperationAreViewData.ColumnEdit = repItemEdit
			columnUndatedOperationAreViewData.UnboundType = DevExpress.Data.UnboundColumnType.String
			lvAddtional.Columns.Add(columnUndatedOperationAreViewData)
			columnUndatedOperationAreViewData.LayoutViewField.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom

			Dim columnUndatedIndustryViewData As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnUndatedIndustryViewData.OptionsColumn.AllowEdit = False
			columnUndatedIndustryViewData.Caption = ("Nichtdatierte Branchen")
			columnUndatedIndustryViewData.Name = "UndatedIndustryViewData"
			columnUndatedIndustryViewData.FieldName = "UndatedIndustryViewData"
			columnUndatedIndustryViewData.ColumnEdit = repItemEdit
			columnUndatedIndustryViewData.UnboundType = DevExpress.Data.UnboundColumnType.String
			lvAddtional.Columns.Add(columnUndatedIndustryViewData)
			columnUndatedIndustryViewData.LayoutViewField.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom

			Dim columnInternetResourcesViewData As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnInternetResourcesViewData.OptionsColumn.AllowEdit = False
			columnInternetResourcesViewData.Caption = ("InternetResourcesViewData")
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

#If DEBUG Then
			'm_ApplicationUtilWebServiceUri = "http://localhost:44721/SPApplication.asmx"
#End If

			Dim webservice As New Main.Notify.SPApplicationWebService.SPApplicationSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

			Trace.WriteLine(String.Format("LoadCVLAdditionalInfoViewData: Customer_ID: {0} contacting...", m_customerID))

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
				m_UtilityUI.ShowErrorDialog(("Education Phase-Daten konnte nicht geladen werden."))
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
			Inherits Main.Notify.SPApplicationWebService.AdditionalInfoViewDataDTO


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
