

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
Imports SP.Main.Notify.SPApplicationWebService
Imports SP.ApplicationMng.SPApplicationWebService

Namespace CVLizer.UI

	Public Class ucCVLPublication

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

		'''' <summary>
		'''' The translation value helper.
		'''' </summary>
		'Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

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


		Private m_PublicationData As IEnumerable(Of PublicationLocalViewData)
		Private m_CurrentEducation As PublicationLocalViewData

		Private m_CVLProfileID As Integer
		'Private m_PublicationID As Integer
		Private m_PhaseID As Integer

		''' <summary>
		''' Settings xml.
		''' </summary>
		Private m_MandantSettingsXml As SettingsXml

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
		Private m_SettingFile As ProgramSettings


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
			'm_connectionString = My.Settings.ConnString_Application

			'm_CommonDatabaseAccess = New CommonDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			'm_AppDatabaseAccess = New AppDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			'm_AppCVDatabaseAccess = New CvlDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			'm_CVLDatabaseAccess = New CVLizerDatabaseAccess(My.Settings.CVL_Connstring, m_InitializationData.UserData.UserLanguage)

			'm_MandantSettingsXml = New SettingsXml(m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))

			'm_ApplicationUtilWebServiceUri = DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI
			'm_SettingFile = ProgSettingData
			'Dim domainName = m_SettingFile.WebserviceDomain
			'm_ApplicationUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI)

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
		Private ReadOnly Property SelectedRecordViewData As PublicationLocalViewData
			Get

				lvPublicationPhase.FocusedRowHandle = lvPublicationPhase.VisibleRecordIndex
				Dim layoutview = TryCast(grdPublicationPhase.MainView, DevExpress.XtraGrid.Views.Layout.LayoutView)

				If Not (layoutview Is Nothing) Then

					Dim selectedRows = layoutview.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim document = CType(layoutview.GetRow(selectedRows(0)), PublicationLocalViewData)
						Return document
					End If

				End If

				Return Nothing
			End Get

		End Property


#End Region


#Region "public Methodes"

		Public Function LoadAssignedEducationData(ByVal cvlProfileID As Integer?) As Boolean
			Dim result As Boolean = True

			m_CVLProfileID = cvlProfileID
			'm_PublicationID = cvlPublicationID

			result = result AndAlso PerformPublicationPhaseWebservice(m_CVLProfileID)

			Return True

		End Function


#End Region


		Public Overrides Sub Reset()

			ResetNavigationGrid()
			ResetPublicationPhaseFields()

		End Sub

		''' <summary>
		''' Resets cv navigation grid.
		''' </summary>
		Private Sub ResetNavigationGrid()

			gvNavigation.OptionsView.ShowIndicator = False
			gvNavigation.OptionsView.ShowAutoFilterRow = True
			gvNavigation.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvNavigation.OptionsView.ShowFooter = False
			gvNavigation.OptionsView.ShowDetailButtons = False
			gvNavigation.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False

			gvNavigation.Columns.Clear()


			Dim columnProfileID As New DevExpress.XtraGrid.Columns.GridColumn()
			columnProfileID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnProfileID.OptionsColumn.AllowEdit = False
			columnProfileID.Caption = ("ID")
			columnProfileID.Name = "ID"
			columnProfileID.FieldName = "ID"
			columnProfileID.Visible = False
			columnProfileID.Width = 10
			gvNavigation.Columns.Add(columnProfileID)

			Dim columnDateFromToViewData As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDateFromToViewData.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDateFromToViewData.OptionsColumn.AllowEdit = False
			columnDateFromToViewData.Caption = ("Zeitraum")
			columnDateFromToViewData.Name = "DateFromToViewData"
			columnDateFromToViewData.FieldName = "DateFromToViewData"
			columnDateFromToViewData.Visible = True
			columnDateFromToViewData.Width = 50
			gvNavigation.Columns.Add(columnDateFromToViewData)


			grdNavigation.DataSource = Nothing

		End Sub

		Private Sub ResetPublicationPhaseFields()
			Dim repItemBoolean As New RepositoryItemCheckEdit()
			Dim repItemEdit As New RepositoryItemMemoEdit()

			lvPublicationPhase.Columns.Clear()
			With lvPublicationPhase
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
				.CardMinSize = New Size(grdPublicationPhase.Width - 200, 100)
			End With
			lvPublicationPhase.CardCaptionFormat = ("[Publikationen: {0} von {1}]")

			repItemEdit.LinesCount = 2
			repItemEdit.ScrollBars = ScrollBars.Both


			Dim columnID As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnID.OptionsColumn.AllowEdit = False
			columnID.Caption = ("ID")
			columnID.Name = "ID"
			columnID.FieldName = "ID"
			lvPublicationPhase.Columns.Add(columnID)
			columnID.LayoutViewField.Visibility = LayoutVisibility.Never

			Dim columnDateFromToViewData As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnDateFromToViewData.OptionsColumn.AllowEdit = False
			columnDateFromToViewData.Caption = ("Zeitraum")
			columnDateFromToViewData.Name = "DateFromToViewData"
			columnDateFromToViewData.FieldName = "DateFromToViewData"
			columnDateFromToViewData.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
			columnDateFromToViewData.AppearanceHeader.Options.UseTextOptions = True
			columnDateFromToViewData.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom
			columnDateFromToViewData.DisplayFormat.FormatString = "YYYY"
			lvPublicationPhase.Columns.Add(columnDateFromToViewData)

			Dim columnDurationViewData As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnDurationViewData.OptionsColumn.AllowEdit = False
			columnDurationViewData.Caption = ("Dauer")
			columnDurationViewData.Name = "DurationViewData"
			columnDurationViewData.FieldName = "DurationViewData"
			lvPublicationPhase.Columns.Add(columnDurationViewData)
			columnDurationViewData.LayoutViewField.Visibility = LayoutVisibility.Never

			Dim columnOperationAreasViewData As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnOperationAreasViewData.OptionsColumn.AllowEdit = False
			columnOperationAreasViewData.Caption = ("Tätigkeiten")
			columnOperationAreasViewData.Name = "OperationAreasViewData"
			columnOperationAreasViewData.FieldName = "OperationAreasViewData"
			columnOperationAreasViewData.ColumnEdit = repItemEdit
			columnOperationAreasViewData.UnboundType = DevExpress.Data.UnboundColumnType.String
			lvPublicationPhase.Columns.Add(columnOperationAreasViewData)
			columnOperationAreasViewData.LayoutViewField.Size = New Size(50, 50)
			columnOperationAreasViewData.LayoutViewField.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom

			Dim columnIndustryViewData As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnIndustryViewData.OptionsColumn.AllowEdit = False
			columnIndustryViewData.Caption = ("Branchen")
			columnIndustryViewData.Name = "IndustryViewData"
			columnIndustryViewData.FieldName = "IndustryViewData"
			columnIndustryViewData.ColumnEdit = repItemEdit
			columnIndustryViewData.UnboundType = DevExpress.Data.UnboundColumnType.String
			lvPublicationPhase.Columns.Add(columnIndustryViewData)
			columnIndustryViewData.LayoutViewField.Size = New Size(50, 50)
			columnIndustryViewData.LayoutViewField.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom

			Dim columnLocationViewData As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnLocationViewData.OptionsColumn.AllowEdit = False
			columnLocationViewData.Caption = ("Einsatzort(e)")
			columnLocationViewData.Name = "LocationViewData"
			columnLocationViewData.FieldName = "LocationViewData"
			lvPublicationPhase.Columns.Add(columnLocationViewData)

			Dim columnPlainText As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnPlainText.OptionsColumn.AllowEdit = False
			columnPlainText.Caption = ("Weitere Informationen")
			columnPlainText.Name = "PlainText"
			columnPlainText.FieldName = "PlainText"
			repItemEdit.LinesCount = 5
			columnPlainText.ColumnEdit = repItemEdit
			lvPublicationPhase.Columns.Add(columnPlainText)
			'columnPlainText.LayoutViewField.Size = New Size(50, 50)
			'columnPlainText.LayoutViewField.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom

			Dim columnTopicViewData As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnTopicViewData.OptionsColumn.AllowEdit = False
			columnTopicViewData.Caption = ("Titel der Publikation")
			columnTopicViewData.Name = "TopicViewData"
			columnTopicViewData.FieldName = "TopicViewData"
			lvPublicationPhase.Columns.Add(columnTopicViewData)

			Dim columnAuthorViewData As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnAuthorViewData.OptionsColumn.AllowEdit = False
			columnAuthorViewData.Caption = ("Autor(en)")
			columnAuthorViewData.Name = "AuthorLable"
			columnAuthorViewData.FieldName = "AuthorLable"
			columnAuthorViewData.Width = 10
			columnAuthorViewData.Visible = True
			lvPublicationPhase.Columns.Add(columnAuthorViewData)

			Dim columnProceedingsViewData As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnProceedingsViewData.OptionsColumn.AllowEdit = False
			columnProceedingsViewData.Caption = ("Proceedings")
			columnProceedingsViewData.Name = "Proceedings"
			columnProceedingsViewData.FieldName = "Proceedings"
			repItemEdit.LinesCount = 2
			columnProceedingsViewData.ColumnEdit = repItemEdit
			lvPublicationPhase.Columns.Add(columnProceedingsViewData)

			For Each col As DevExpress.XtraGrid.Columns.LayoutViewColumn In lvPublicationPhase.Columns
				col.Visible = True
			Next

			grdPublicationPhase.DataSource = Nothing


		End Sub


		'#End Region


		''' <summary>
		'''  Performs loading cvl publication data.
		''' </summary>
		Private Function PerformPublicationPhaseWebservice(ByVal profileID As Integer?) As Boolean

#If DEBUG Then
			'm_ApplicationUtilWebServiceUri = "http://localhost:44721/SPApplication.asmx"
#End If

			Dim webservice As New Main.Notify.SPApplicationWebService.SPApplicationSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

			Trace.WriteLine(String.Format("LoadCVLPublicationViewData: Customer_ID: {0} contacting...", m_customerID))

			' Read data over webservice
			Dim searchResult = webservice.LoadCVLPublicationViewData(m_customerID, profileID)
			If searchResult Is Nothing Then
				m_Logger.LogError(String.Format("Documents could not be loaded from webservice! {0} | {1}", m_customerID, profileID))

				Return False
			End If

			'If searchResult.Count > 0 Then
			Dim gridData = (From person In searchResult
											Select New PublicationLocalViewData With {.ID = person.ID,
														.PhaseID = person.PhaseID,
														.Author = person.Author,
														.Institute = person.Institute,
														.Proceedings = person.Proceedings,
														.DateFrom = person.DateFrom,
														.DateTo = person.DateTo,
														.DateFromFuzzy = person.DateFromFuzzy,
														.DateToFuzzy = person.DateToFuzzy,
														.Duration = person.Duration,
														.Current = person.Current,
														.SubPhase = person.SubPhase,
														.Comments = person.Comments,
														.PlainText = person.PlainText,
														.Locations = person.Locations,
														.Skills = person.Skills,
														.SoftSkills = person.SoftSkills,
														.OperationAreas = person.OperationAreas,
														.Industries = person.Industries,
														.CustomCodes = person.CustomCodes,
														.Topic = person.Topic,
														.InternetResources = person.InternetResources,
														.DocumentID = person.DocumentID
														}).ToList()

			Dim listDataSource As BindingList(Of PublicationLocalViewData) = New BindingList(Of PublicationLocalViewData)
			For Each p In gridData
				listDataSource.Add(p)
			Next
			'End If

			m_PublicationData = listDataSource
			grdPublicationPhase.DataSource = m_PublicationData
			grdNavigation.DataSource = m_PublicationData


			Return Not m_PublicationData Is Nothing

		End Function

		Private Sub lvEducationPhase_VisibleRecordIndexChanged(sender As Object, e As LayoutViewVisibleRecordIndexChangedEventArgs) Handles lvPublicationPhase.VisibleRecordIndexChanged
			Dim view As LayoutView = TryCast(sender, LayoutView)
			If view Is Nothing Then Return
			If m_PublicationData Is Nothing OrElse m_PublicationData.Count = 0 Then Return

			lvPublicationPhase.Focus()
			Dim data = SelectedRecordViewData
			If data Is Nothing Then
				Trace.WriteLine("error finding data!!!")

				Return
			End If
			Dim myID As Integer = data.PhaseID
			FocusMainGrid(myID)

		End Sub

		Private Sub lvEducationPhase_MouseWheel(sender As Object, e As MouseEventArgs) Handles lvPublicationPhase.MouseWheel
			Dim view As LayoutView = TryCast(sender, LayoutView)
			If view Is Nothing Then Return
			If m_PublicationData Is Nothing OrElse m_PublicationData.Count = 0 Then Return

			lvPublicationPhase.Focus()
			Dim data = SelectedRecordViewData
			If data Is Nothing Then
				Trace.WriteLine("error finding data!!!")

				Return
			End If
			Dim myID As Integer = data.PhaseID
			FocusMainGrid(myID)

		End Sub

		''' <summary>
		''' Focuses a grid view.
		''' </summary>
		Private Sub FocusMainGrid(ByVal phaseID As Integer)

			If Not grdNavigation.DataSource Is Nothing Then

				Dim documentViewData = CType(gvNavigation.DataSource, BindingList(Of PublicationViewData))

				Dim index = documentViewData.ToList().FindIndex(Function(data) data.ID = phaseID)

				Dim rowHandle = gvNavigation.GetRowHandle(index)
				gvNavigation.FocusedRowHandle = rowHandle

			End If

		End Sub

		Sub OngvgvWorkPhase_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)
			Dim success As Boolean = True

			Dim data = SelectedRecordViewData
			If data Is Nothing Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(("Publication Phase-Daten konnte nicht geladen werden."))
				Return
			End If

			success = success AndAlso DisplayAssignedEducationPhaseData(data)

		End Sub

		Private Function DisplayAssignedEducationPhaseData(ByVal data As PublicationLocalViewData) As Boolean
			Dim success As Boolean = True

			Return success

		End Function

		Private Sub OnlvEducationPhase_CustomFieldValueStyle(ByVal sender As System.Object, ByVal e As LayoutViewFieldValueStyleEventArgs) Handles lvPublicationPhase.CustomFieldValueStyle

			If e Is Nothing Then Return
			Try

				If Not (e.Column.FieldName <> "DurationViewData" OrElse e.Column.FieldName <> "LocationViewData" OrElse e.Column.FieldName <> "PlainText" OrElse e.Column.FieldName <> "OperationAreasViewData" OrElse e.Column.FieldName <> "IndustryViewData" OrElse e.Column.FieldName <> "TopicViewData" OrElse e.Column.FieldName <> "Proceedings") Then Return

				Dim view As LayoutView = TryCast(sender, LayoutView)
				If view Is Nothing Then Return
				Dim showDurationViewData As Boolean = Not (view.GetRowCellValue(e.RowHandle, "DurationViewData") Is Nothing OrElse String.IsNullOrWhiteSpace(view.GetRowCellValue(e.RowHandle, "DurationViewData").ToString()))
				Dim showLocationViewData As Boolean = Not (view.GetRowCellValue(e.RowHandle, "LocationViewData") Is Nothing OrElse String.IsNullOrWhiteSpace(view.GetRowCellValue(e.RowHandle, "LocationViewData").ToString()))
				Dim showComments As Boolean = Not (view.GetRowCellValue(e.RowHandle, "PlainText") Is Nothing OrElse String.IsNullOrWhiteSpace(view.GetRowCellValue(e.RowHandle, "PlainText").ToString()))
				Dim showOperationAreasViewData As Boolean = Not (view.GetRowCellValue(e.RowHandle, "OperationAreasViewData") Is Nothing OrElse String.IsNullOrWhiteSpace(view.GetRowCellValue(e.RowHandle, "OperationAreasViewData").ToString()))
				Dim showIndustryViewData As Boolean = Not (view.GetRowCellValue(e.RowHandle, "IndustryViewData") Is Nothing OrElse String.IsNullOrWhiteSpace(view.GetRowCellValue(e.RowHandle, "IndustryViewData").ToString()))
				Dim showTopic As Boolean = Not (view.GetRowCellValue(e.RowHandle, "TopicViewData") Is Nothing OrElse String.IsNullOrWhiteSpace(view.GetRowCellValue(e.RowHandle, "TopicViewData").ToString()))
				Dim showProcceding As Boolean = Not (view.GetRowCellValue(e.RowHandle, "Proceedings") Is Nothing OrElse String.IsNullOrWhiteSpace(view.GetRowCellValue(e.RowHandle, "Proceedings").ToString()))

				If Not view.Columns("DurationViewData") Is Nothing Then view.Columns("DurationViewData").LayoutViewField.Visibility = If(showDurationViewData, LayoutVisibility.Always, LayoutVisibility.Never)
				If Not view.Columns("LocationViewData") Is Nothing Then view.Columns("LocationViewData").LayoutViewField.Visibility = If(showLocationViewData, LayoutVisibility.Always, LayoutVisibility.Never)
				If Not view.Columns("PlainText") Is Nothing Then view.Columns("PlainText").LayoutViewField.Visibility = If(showComments, LayoutVisibility.Always, LayoutVisibility.Never)
				If Not view.Columns("TopicViewData") Is Nothing Then view.Columns("TopicViewData").LayoutViewField.Visibility = If(showTopic, LayoutVisibility.Always, LayoutVisibility.Never)
				If Not view.Columns("OperationAreasViewData") Is Nothing Then view.Columns("OperationAreasViewData").LayoutViewField.Visibility = If(showOperationAreasViewData, LayoutVisibility.Always, LayoutVisibility.Never)
				If Not view.Columns("IndustryViewData") Is Nothing Then view.Columns("IndustryViewData").LayoutViewField.Visibility = If(showIndustryViewData, LayoutVisibility.Always, LayoutVisibility.Never)
				If Not view.Columns("Proceedings") Is Nothing Then view.Columns("Proceedings").LayoutViewField.Visibility = If(showProcceding, LayoutVisibility.Always, LayoutVisibility.Never)


			Catch ex As Exception

			End Try

		End Sub



#Region "Helpers Class"


		Private Class PublicationLocalViewData
			Inherits Main.Notify.SPApplicationWebService.PublicationViewDataDTO


#Region "readonly properties"

			Public ReadOnly Property DateFromToViewData As String
				Get
					Dim value As String = String.Empty
					If DateFrom.HasValue Then
						If DateFrom = CDate("01.01." & Year(DateFrom)) Then
							value = String.Format("{0:F0}", Year(DateFrom))
						Else
							value = String.Format("{0}", DateViewData(DateFrom))
						End If

					End If
					If DateTo.HasValue Then
						If DateTo = CDate("01.01." & Year(DateTo)) Then
							value &= String.Format(" - {0:F0}", Year(DateTo))
						Else
							value &= String.Format(" - {0}", DateViewData(DateTo))
						End If
					End If
					value = String.Format("{0} {1}", value, CurrentViewData)

					Return value
				End Get
			End Property

			Public ReadOnly Property DateViewData(ByVal dt As Date?) As String
				Get
					Dim value As String = String.Empty
					If Not dt.HasValue Then Return value
					If DateAndTime.Day(dt) = 1 Then
						value = String.Format("{0:MM.yyyy}", dt)
					Else
						value = String.Format("{0:d}", dt)
					End If

					Return value
				End Get
			End Property

			Public ReadOnly Property DurationViewData As String
				Get
					Dim value As String = String.Empty
					If Duration.GetValueOrDefault(0) = 0 Then Return value

					Dim yrs As Integer
					Dim mos As Integer

					yrs = Math.DivRem(Duration.GetValueOrDefault(0), 12, mos)
					If yrs > 0 Then
						If yrs = 1 Then
							value = String.Format("{0:F0} Monat{1}", Duration.GetValueOrDefault(0), If(Duration.GetValueOrDefault(0) > 1, "e", ""))

							Return value
						Else
							value = String.Format("{0:F0} Jahr{1}", yrs, If(yrs > 1, "e", ""))
						End If

					End If

					If mos > 0 Then
						value = String.Format("{0}{1}{2:F0} Monat{3}", value, If(yrs > 0, " und ", ""), mos, If(mos > 1, "e", ""))
					End If


					Return If(Duration.GetValueOrDefault(0) > 0, value, String.Empty)
				End Get
			End Property

			Public ReadOnly Property CurrentViewData As String
				Get
					Dim value As String = "(derzeit)"
					If Not Current.GetValueOrDefault(False) Then value = ""

					Return value
				End Get
			End Property

			Public ReadOnly Property AuthorLable As String
				Get
					Dim value As String = String.Empty
					If Not Author Is Nothing AndAlso Author.Count > 0 Then
						For Each itm In Author
							value &= If(String.IsNullOrWhiteSpace(value), String.Empty, vbNewLine) & itm.Lable
						Next
					End If

					Return value
				End Get
			End Property

		End Class
#End Region


#End Region


	End Class


End Namespace


