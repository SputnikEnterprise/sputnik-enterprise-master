
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
Imports DevExpress.XtraLayout.Utils
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid.Views.Layout
Imports DevExpress.XtraGrid.Views.Layout.Events
Imports SP.ApplicationMng.SPApplicationWebService
Imports SP.Main.Notify.SPApplicationWebService
Imports System.IO

Namespace CVLizer.UI


	Public Class ucCVLWork

#Region "Private Consts"


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
		'''' Service Uri of Sputnik notification util webservice.
		'''' </summary>
		'Private m_ApplicationUtilWebServiceUri As String

		Private m_applicantDb As String
		Private m_customerID As String

		Private m_CVLizerXMLData As CVLizerXMLData
		Private m_CurrentFileExtension As String


		Private m_WorkPhaseData As IEnumerable(Of WorkPhaseLocalViewData)
		Private m_CVLProfileID As Integer
		Private m_WorkID As Integer
		Private m_PhaseID As Integer

		''' <summary>
		''' Settings xml.
		''' </summary>
		Private m_MandantSettingsXml As SettingsXml

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

		'Private m_SettingFile As ProgramSettings


#End Region


#Region "constructor"

		Public Sub New()

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
		Private ReadOnly Property SelectedRecordViewData As WorkPhaseLocalViewData
			Get

				lvWorkPhase.FocusedRowHandle = lvWorkPhase.VisibleRecordIndex
				Dim layoutview = TryCast(grdWorkPhase.MainView, DevExpress.XtraGrid.Views.Layout.LayoutView)

				If Not (layoutview Is Nothing) Then

					Dim selectedRows = layoutview.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim document = CType(layoutview.GetRow(selectedRows(0)), WorkPhaseLocalViewData)
						Return document
					End If

				End If

				Return Nothing
			End Get

		End Property


#End Region


#Region "public Methodes"

		Public Function LoadAssignedWorkData(ByVal cvlProfileID As Integer?, ByVal cvlWorkID As Integer?) As Boolean
			Dim result As Boolean = True

			m_CVLProfileID = cvlProfileID
			m_WorkID = cvlWorkID

			result = result AndAlso PerformWorkPhaseWebservice(m_CVLProfileID, m_WorkID)

			Return True

		End Function


#End Region


		Public Overrides Sub Reset()

			ResetNavigationGrid()
			ResetWorkPhaseFields()

		End Sub

#Region "reset"

		''' <summary>
		''' Resets cv navigation grid.
		''' </summary>
		Private Sub ResetNavigationGrid()

			gvNavigation.OptionsView.ShowIndicator = False
			gvNavigation.OptionsView.ShowAutoFilterRow = True
			gvNavigation.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvNavigation.OptionsView.ShowFooter = False
			gvNavigation.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
			gvNavigation.OptionsView.ShowDetailButtons = False
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

		Private Sub ResetWorkPhaseFields()
			Dim repItemEdit As New RepositoryItemMemoEdit()

			'm_SuppressUIEvents = True
			lvWorkPhase.Columns.Clear()

			lvWorkPhase.OptionsView.ShowCardBorderIfCaptionHidden = True
			lvWorkPhase.OptionsView.ShowCardCaption = True
			lvWorkPhase.OptionsView.ShowHeaderPanel = False
			lvWorkPhase.OptionsCustomization.AllowSort = False
			lvWorkPhase.OptionsCustomization.AllowFilter = False

			lvWorkPhase.Appearance.FieldCaption.Font = New System.Drawing.Font(lvWorkPhase.Appearance.FieldCaption.Font.Name, lvWorkPhase.Appearance.FieldCaption.Font.Size, System.Drawing.FontStyle.Bold)
			lvWorkPhase.Appearance.FieldCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
			lvWorkPhase.Appearance.FieldCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
			lvWorkPhase.Appearance.FieldCaption.Options.UseFont = True
			lvWorkPhase.Appearance.FieldValue.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
			lvWorkPhase.Appearance.FieldValue.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
			lvWorkPhase.Appearance.FieldValue.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
			lvWorkPhase.Appearance.FieldValue.Options.UseTextOptions = True
			lvWorkPhase.OptionsView.ViewMode = LayoutViewMode.SingleRecord
			lvWorkPhase.CardMinSize = New Size(grdWorkPhase.Width - 200, 100)

			lvWorkPhase.CardCaptionFormat = ("[Berufserfahrung: {0} von {1}]")

			repItemEdit.ScrollBars = ScrollBars.Both
			repItemEdit.LinesCount = 3


			Dim columnID As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnID.OptionsColumn.AllowEdit = False
			columnID.Caption = ("ID")
			columnID.Name = "ID"
			columnID.FieldName = "ID"
			lvWorkPhase.Columns.Add(columnID)
			columnID.LayoutViewField.Visibility = LayoutVisibility.Never


			Dim columnDateFromToViewData As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnDateFromToViewData.OptionsColumn.AllowEdit = False
			columnDateFromToViewData.Caption = ("Zeitraum")
			columnDateFromToViewData.Name = "DateFromToViewData"
			columnDateFromToViewData.FieldName = "DateFromToViewData"
			lvWorkPhase.Columns.Add(columnDateFromToViewData)

			Dim columnDurationViewData As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnDurationViewData.OptionsColumn.AllowEdit = False
			columnDurationViewData.Caption = ("Dauer")
			columnDurationViewData.Name = "DurationViewData"
			columnDurationViewData.FieldName = "DurationViewData"
			lvWorkPhase.Columns.Add(columnDurationViewData)

			Dim columnIndustryViewData As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnIndustryViewData.OptionsColumn.AllowEdit = False
			columnIndustryViewData.Caption = ("Branchen")
			columnIndustryViewData.Name = "IndustryViewData"
			columnIndustryViewData.FieldName = "IndustryViewData"
			columnIndustryViewData.ColumnEdit = repItemEdit
			columnIndustryViewData.UnboundType = DevExpress.Data.UnboundColumnType.String
			lvWorkPhase.Columns.Add(columnIndustryViewData)
			columnIndustryViewData.LayoutViewField.Size = New Size(50, 50)
			columnIndustryViewData.LayoutViewField.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom

			Dim columnSkillViewData As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnSkillViewData.OptionsColumn.AllowEdit = False
			columnSkillViewData.Caption = ("Erworbene Fähigkeit(en)")
			columnSkillViewData.Name = "SkillViewData"
			columnSkillViewData.FieldName = "SkillViewData"
			columnSkillViewData.ColumnEdit = repItemEdit
			columnSkillViewData.UnboundType = DevExpress.Data.UnboundColumnType.String
			lvWorkPhase.Columns.Add(columnSkillViewData)
			columnSkillViewData.LayoutViewField.Size = New Size(50, 50)
			columnSkillViewData.LayoutViewField.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom

			Dim columnOperationAreasViewData As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnOperationAreasViewData.OptionsColumn.AllowEdit = False
			columnOperationAreasViewData.Caption = ("Tätigkeitsgebiete")
			columnOperationAreasViewData.Name = "OperationAreasViewData"
			columnOperationAreasViewData.FieldName = "OperationAreasViewData"
			columnOperationAreasViewData.ColumnEdit = repItemEdit
			columnOperationAreasViewData.UnboundType = DevExpress.Data.UnboundColumnType.String
			lvWorkPhase.Columns.Add(columnOperationAreasViewData)
			columnOperationAreasViewData.LayoutViewField.Size = New Size(50, 50)
			columnOperationAreasViewData.LayoutViewField.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom

			Dim columnLocationViewData As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnLocationViewData.OptionsColumn.AllowEdit = False
			columnLocationViewData.Caption = ("Einsatzort(e)")
			columnLocationViewData.Name = "LocationViewData"
			columnLocationViewData.FieldName = "LocationViewData"
			lvWorkPhase.Columns.Add(columnLocationViewData)

			Dim columnComments As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnComments.OptionsColumn.AllowEdit = False
			columnComments.Caption = ("Weitere Informationen")
			columnComments.Name = "Comments"
			columnComments.FieldName = "Comments"
			columnComments.ColumnEdit = repItemEdit
			lvWorkPhase.Columns.Add(columnComments)
			columnComments.LayoutViewField.Size = New Size(50, 50)
			columnComments.LayoutViewField.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom

			Dim columnCompanyViewData As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnCompanyViewData.OptionsColumn.AllowEdit = False
			columnCompanyViewData.Caption = ("Arbeitgeber")
			columnCompanyViewData.Name = "CompanyViewData"
			columnCompanyViewData.FieldName = "CompanyViewData"
			lvWorkPhase.Columns.Add(columnCompanyViewData)

			Dim columnFunctionViewData As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnFunctionViewData.OptionsColumn.AllowEdit = False
			columnFunctionViewData.Caption = ("Berufsbezeichnung")
			columnFunctionViewData.Name = "FunctionViewData"
			columnFunctionViewData.FieldName = "FunctionViewData"
			lvWorkPhase.Columns.Add(columnFunctionViewData)

			Dim columnPositionViewData As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnPositionViewData.OptionsColumn.AllowEdit = False
			columnPositionViewData.Caption = ("Position")
			columnPositionViewData.Name = "PositionViewData"
			columnPositionViewData.FieldName = "PositionViewData"
			lvWorkPhase.Columns.Add(columnPositionViewData)

			Dim columnEmploymentViewData As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnEmploymentViewData.OptionsColumn.AllowEdit = False
			columnEmploymentViewData.Caption = ("Arbeitsverhältnis")
			columnEmploymentViewData.Name = "EmploymentViewData"
			columnEmploymentViewData.FieldName = "EmploymentViewData"
			lvWorkPhase.Columns.Add(columnEmploymentViewData)

			Dim columnProject As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnProject.OptionsColumn.AllowEdit = False
			columnProject.Caption = ("Explizite Projekttätigkeit")
			columnProject.Name = "ProjectViewData"
			columnProject.FieldName = "ProjectViewData"
			lvWorkPhase.Columns.Add(columnProject)

			'm_SuppressUIEvents = False

			For Each col As DevExpress.XtraGrid.Columns.LayoutViewColumn In lvWorkPhase.Columns
				col.Visible = True
			Next
			grdWorkPhase.DataSource = Nothing


		End Sub

#End Region


		''' <summary>
		'''  Performs loading cvl work data.
		''' </summary>
		Private Function PerformWorkPhaseWebservice(ByVal profileID As Integer?, ByVal workID As Integer?) As Boolean

#If DEBUG Then
			'm_ApplicationUtilWebServiceUri = "http://localhost:44721/SPApplication.asmx"
#End If

			Dim webservice As New Main.Notify.SPApplicationWebService.SPApplicationSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

			Trace.WriteLine(String.Format("m_customerID: {0} contacting...", m_customerID))

			' Read data over webservice
			Dim searchResult = webservice.LoadCVLWorkPhaseViewData(m_customerID, profileID, workID)
			If searchResult Is Nothing Then
				m_Logger.LogError(String.Format("LoadCVLWorkPhaseViewData: could not be loaded from webservice! {0} | {1} | {2}", m_customerID, profileID, workID))

				Return False
			End If

			Dim gridData = (From person In searchResult
							Select New WorkPhaseLocalViewData With {.ID = person.ID,
								.WorkPhaseID = person.WorkPhaseID,
								.PhaseID = person.PhaseID,
								.Project = person.Project,
								.DateFrom = person.DateFrom,
								.Companies = person.Companies,
								.DateTo = person.DateTo,
								.DateFromFuzzy = person.DateFromFuzzy,
								.DateToFuzzy = person.DateToFuzzy,
								.Duration = person.Duration,
								.Current = person.Current,
								.SubPhase = person.SubPhase,
								.Comments = person.Comments,
								.PlainText = person.PlainText,
								.Functions = person.Functions,
								.Positions = person.Positions,
								.Employments = person.Employments,
								.WorkTimes = person.WorkTimes,
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

			Dim listDataSource As BindingList(Of WorkPhaseLocalViewData) = New BindingList(Of WorkPhaseLocalViewData)

			For Each p In gridData
				listDataSource.Add(p)
			Next

			m_WorkPhaseData = listDataSource
			grdNavigation.DataSource = m_WorkPhaseData
			grdWorkPhase.DataSource = m_WorkPhaseData


			Return Not (m_WorkPhaseData Is Nothing)

		End Function

		Private Sub OnlvWorkPhase_MouseWheel(sender As Object, e As MouseEventArgs) Handles lvWorkPhase.MouseWheel
			Dim view As LayoutView = TryCast(sender, LayoutView)
			'If view Is Nothing Then Return
			'If m_WorkPhaseData Is Nothing OrElse m_WorkPhaseData.Count = 0 Then Return
			'For Each col As DevExpress.XtraGrid.Columns.LayoutViewColumn In lvWorkPhase.Columns
			'	col.Visible = True
			'Next

			lvWorkPhase.Focus()
			Dim data = SelectedRecordViewData
			If data Is Nothing Then
				Trace.WriteLine("error finding data!!!")

				Return
			End If
			Dim myID As Integer = data.ID

		End Sub

		Private Sub OnlvWorkPhase_CustomFieldValueStyle(ByVal sender As System.Object, ByVal e As LayoutViewFieldValueStyleEventArgs) Handles lvWorkPhase.CustomFieldValueStyle

			'If m_SuppressUIEvents Then Return
			'Return

			If e Is Nothing Then Return
			Try

				If Not (e.Column.FieldName <> "DurationViewData" OrElse e.Column.FieldName <> "LocationViewData" OrElse e.Column.FieldName <> "Comments" OrElse e.Column.FieldName <> "OperationAreasViewData" OrElse
					e.Column.FieldName <> "IndustryViewData" OrElse e.Column.FieldName <> "SkillViewData" _
					OrElse e.Column.FieldName <> "CompanyViewData" OrElse e.Column.FieldName <> "PositionViewData" OrElse e.Column.FieldName <> "EmploymentViewData" OrElse e.Column.FieldName <> "FunctionViewData") Then Return

				Dim view As LayoutView = TryCast(sender, LayoutView)
				If view Is Nothing Then Return

				Dim showDurationViewData As Boolean = Not (view.GetRowCellValue(e.RowHandle, "DurationViewData") Is Nothing OrElse String.IsNullOrWhiteSpace(view.GetRowCellValue(e.RowHandle, "DurationViewData").ToString()))
				Dim showLocationViewData As Boolean = Not (view.GetRowCellValue(e.RowHandle, "LocationViewData") Is Nothing OrElse String.IsNullOrWhiteSpace(view.GetRowCellValue(e.RowHandle, "LocationViewData").ToString()))
				Dim showComments As Boolean = Not (view.GetRowCellValue(e.RowHandle, "Comments") Is Nothing OrElse String.IsNullOrWhiteSpace(view.GetRowCellValue(e.RowHandle, "Comments").ToString()))
				Dim showOperationAreasViewData As Boolean = Not (view.GetRowCellValue(e.RowHandle, "OperationAreasViewData") Is Nothing OrElse String.IsNullOrWhiteSpace(view.GetRowCellValue(e.RowHandle, "OperationAreasViewData").ToString()))
				Dim showIndustryViewData As Boolean = Not (view.GetRowCellValue(e.RowHandle, "IndustryViewData") Is Nothing OrElse String.IsNullOrWhiteSpace(view.GetRowCellValue(e.RowHandle, "IndustryViewData").ToString()))
				Dim showCompanyViewData As Boolean = Not (view.GetRowCellValue(e.RowHandle, "CompanyViewData") Is Nothing OrElse String.IsNullOrWhiteSpace(view.GetRowCellValue(e.RowHandle, "CompanyViewData").ToString()))
				Dim showPositionViewData As Boolean = Not (view.GetRowCellValue(e.RowHandle, "PositionViewData") Is Nothing OrElse String.IsNullOrWhiteSpace(view.GetRowCellValue(e.RowHandle, "PositionViewData").ToString()))
				Dim showEmploymentViewData As Boolean = Not (view.GetRowCellValue(e.RowHandle, "EmploymentViewData") Is Nothing OrElse String.IsNullOrWhiteSpace(view.GetRowCellValue(e.RowHandle, "EmploymentViewData").ToString()))
				Dim showSkillViewData As Boolean = Not (view.GetRowCellValue(e.RowHandle, "SkillViewData") Is Nothing OrElse String.IsNullOrWhiteSpace(view.GetRowCellValue(e.RowHandle, "SkillViewData").ToString()))
				Dim showFunctionViewData As Boolean = Not (view.GetRowCellValue(e.RowHandle, "FunctionViewData") Is Nothing OrElse String.IsNullOrWhiteSpace(view.GetRowCellValue(e.RowHandle, "FunctionViewData").ToString()))

				If Not view.Columns("DurationViewData") Is Nothing Then view.Columns("DurationViewData").LayoutViewField.Visibility = If(showDurationViewData, LayoutVisibility.Always, LayoutVisibility.Never)
				If Not view.Columns("LocationViewData") Is Nothing Then view.Columns("LocationViewData").LayoutViewField.Visibility = If(showLocationViewData, LayoutVisibility.Always, LayoutVisibility.Never)
				If Not view.Columns("Comments") Is Nothing Then view.Columns("Comments").LayoutViewField.Visibility = If(showComments, LayoutVisibility.Always, LayoutVisibility.Never)
				If Not view.Columns("OperationAreasViewData") Is Nothing Then view.Columns("OperationAreasViewData").LayoutViewField.Visibility = If(showOperationAreasViewData, LayoutVisibility.Always, LayoutVisibility.Never)
				If Not view.Columns("SkillViewData") Is Nothing Then view.Columns("SkillViewData").LayoutViewField.Visibility = If(showSkillViewData, LayoutVisibility.Always, LayoutVisibility.Never)

				If Not view.Columns("IndustryViewData") Is Nothing Then view.Columns("IndustryViewData").LayoutViewField.Visibility = If(showIndustryViewData, LayoutVisibility.Always, LayoutVisibility.Never)
				If Not view.Columns("CompanyViewData") Is Nothing Then view.Columns("CompanyViewData").LayoutViewField.Visibility = If(showCompanyViewData, LayoutVisibility.Always, LayoutVisibility.Never)
				If Not view.Columns("PositionViewData") Is Nothing Then view.Columns("PositionViewData").LayoutViewField.Visibility = If(showPositionViewData, LayoutVisibility.Always, LayoutVisibility.Never)
				If Not view.Columns("EmploymentViewData") Is Nothing Then view.Columns("EmploymentViewData").LayoutViewField.Visibility = If(showEmploymentViewData, LayoutVisibility.Always, LayoutVisibility.Never)
				If Not view.Columns("FunctionViewData") Is Nothing Then view.Columns("FunctionViewData").LayoutViewField.Visibility = If(showFunctionViewData, LayoutVisibility.Always, LayoutVisibility.Never)


				'view.Columns("DurationViewData").LayoutViewField.Visibility = If(showDurationViewData, LayoutVisibility.Always, LayoutVisibility.Never)
				'view.Columns("LocationViewData").LayoutViewField.Visibility = If(showLocationViewData, LayoutVisibility.Always, LayoutVisibility.Never)
				'view.Columns("Comments").LayoutViewField.Visibility = If(showComments, LayoutVisibility.Always, LayoutVisibility.Never)
				'view.Columns("OperationAreasViewData").LayoutViewField.Visibility = If(showOperationAreasViewData, LayoutVisibility.Always, LayoutVisibility.Never)
				'view.Columns("SkillViewData").LayoutViewField.Visibility = If(showSkillViewData, LayoutVisibility.Always, LayoutVisibility.Never)

				'view.Columns("IndustryViewData").LayoutViewField.Visibility = If(showIndustryViewData, LayoutVisibility.Always, LayoutVisibility.Never)
				'view.Columns("CompanyViewData").LayoutViewField.Visibility = If(showCompanyViewData, LayoutVisibility.Always, LayoutVisibility.Never)
				'view.Columns("PositionViewData").LayoutViewField.Visibility = If(showPositionViewData, LayoutVisibility.Always, LayoutVisibility.Never)
				'view.Columns("EmploymentViewData").LayoutViewField.Visibility = If(showEmploymentViewData, LayoutVisibility.Always, LayoutVisibility.Never)
				'view.Columns("FunctionViewData").LayoutViewField.Visibility = If(showFunctionViewData, LayoutVisibility.Always, LayoutVisibility.Never)


			Catch ex As Exception

			End Try

		End Sub


#Region "Helper Class"

		Private Class WorkPhaseLocalViewData
			Inherits Main.Notify.SPApplicationWebService.WorkPhaseViewDataDTO

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

			Public ReadOnly Property CommentsLable As String
				Get
					Dim value As String = String.Empty
					If Not Comments Is Nothing Then
						value = Comments.Replace(vbNewLine, " ")
					End If

					Return value
				End Get
			End Property

			Public ReadOnly Property PlainTextLable As String
				Get
					Dim value As String = String.Empty
					If Not PlainText Is Nothing Then
						value = PlainText.Replace(vbNewLine, " ").Replace(vbLf, " ")
					End If

					Return value
				End Get
			End Property

			Public ReadOnly Property LocationViewData As String
				Get
					If Locations Is Nothing OrElse Locations.Count = 0 Then Return Nothing
					Dim value As String = String.Empty
					For Each itm In Locations
						value &= String.Format("{0}{1}{2}{3}", If(String.IsNullOrWhiteSpace(value), "", vbNewLine), itm.City, If(String.IsNullOrWhiteSpace(itm.CountryLable), "", " - "), itm.CountryLable)
					Next

					Return value
				End Get
			End Property

			Public ReadOnly Property SkillViewData As String
				Get
					If Skills Is Nothing OrElse Skills.Count = 0 Then Return Nothing
					Dim value As String = String.Empty
					For Each itm In Skills
						value &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(value), "", ", "), itm.Name)
					Next

					Return value
				End Get
			End Property

			Public ReadOnly Property SoftSkillViewData As String
				Get
					If SoftSkills Is Nothing OrElse SoftSkills.Count = 0 Then Return Nothing
					Dim value As String = String.Empty
					For Each itm In SoftSkills
						value &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(value), "", ", "), itm.Name)
					Next

					Return value
				End Get
			End Property

			Public ReadOnly Property OperationAreasViewData As String
				Get
					If OperationAreas Is Nothing OrElse OperationAreas.Count = 0 Then Return Nothing
					Dim value As String = String.Empty
					For Each itm In OperationAreas
						value &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(value), "", ", "), itm.Name)
					Next

					Return value
				End Get
			End Property

			Public ReadOnly Property IndustryViewData As String
				Get
					If Industries Is Nothing OrElse Industries.Count = 0 Then Return Nothing
					Dim value As String = String.Empty
					For Each itm In Industries
						value &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(value), "", ", "), itm.Name)
					Next

					Return value
				End Get
			End Property

			Public ReadOnly Property CustomCodesViewData As String
				Get
					If CustomCodes Is Nothing OrElse CustomCodes.Count = 0 Then Return Nothing
					Dim value As String = String.Empty
					For Each itm In CustomCodes
						value &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(value), "", ", "), itm.Name)
					Next

					Return value
				End Get
			End Property

			Public ReadOnly Property TopicViewData As String
				Get
					If Topic Is Nothing OrElse Topic.Count = 0 Then Return Nothing
					Dim value As String = String.Empty
					For Each itm In Topic
						value &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(value), "", ", "), itm.Lable)
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

			Public ReadOnly Property DocumentIDViewData As String
				Get
					If DocumentID Is Nothing OrElse DocumentID.Count = 0 Then Return Nothing
					Dim value As String = String.Empty
					For Each itm In DocumentID
						value &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(value), "", ", "), itm.CodeNumber)
					Next

					Return value
				End Get
			End Property

			Public ReadOnly Property CurrentViewData As String
				Get
					Dim value As String = "(derzeit)"
					If Not Current.GetValueOrDefault(False) Then value = ""

					Return value
				End Get
			End Property



			Public ReadOnly Property CompanyViewData As String
				Get
					If Companies Is Nothing OrElse Companies.Count = 0 Then Return Nothing
					Dim value As String = String.Empty
					For Each itm In Companies
						value &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(value), "", ", "), itm.Lable)
					Next

					Return value
				End Get
			End Property

			Public ReadOnly Property FunctionViewData As String
				Get
					If Functions Is Nothing OrElse Functions.Count = 0 Then Return Nothing
					Dim value As String = String.Empty
					For Each itm In Functions
						value &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(value), "", ", "), itm.Lable)
					Next

					Return value
				End Get
			End Property

			Public ReadOnly Property PositionViewData As String
				Get
					If Positions Is Nothing OrElse Positions.Count = 0 Then Return Nothing
					Dim value As String = String.Empty
					For Each itm In Positions
						value &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(value), "", ", "), itm.Name)
					Next

					Return value
				End Get
			End Property

			Public ReadOnly Property EmploymentViewData As String
				Get
					If Employments Is Nothing OrElse Employments.Count = 0 Then Return Nothing
					Dim value As String = String.Empty
					For Each itm In Employments
						value &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(value), "", ", "), itm.Name)
					Next

					Return value
				End Get
			End Property

			Public ReadOnly Property WorkTimeViewData As String
				Get
					If WorkTimes Is Nothing OrElse WorkTimes.Count = 0 Then Return Nothing
					Dim value As String = String.Empty
					For Each itm In WorkTimes
						value &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(value), "", ", "), itm.Name)
					Next

					Return value
				End Get
			End Property

			Public ReadOnly Property ProjectViewData As String
				Get
					Dim value As String = "Nein"
					If Not Project.GetValueOrDefault(False) Then value = "Nein" Else value = "Ja"

					Return value
				End Get
			End Property


#End Region


		End Class

#End Region

	End Class

End Namespace

