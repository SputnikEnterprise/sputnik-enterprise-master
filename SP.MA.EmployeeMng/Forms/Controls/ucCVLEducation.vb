
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
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid.Views.Layout
Imports DevExpress.XtraLayout.Utils
Imports DevExpress.XtraGrid.Views.Layout.Events
'Imports SP.MA.EmployeeMng.SPApplicationWebService
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.Internal.Automations.SPApplicationWebService


Namespace UI

	Public Class ucCVLEducation

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


		Private m_EducationPhaseData As IEnumerable(Of EducationPhaseLocalViewData)
		Private m_CurrentEducation As EducationPhaseLocalViewData

		Private m_EducationID As Integer
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
		Private ReadOnly Property SelectedRecordViewData As EducationPhaseLocalViewData
			Get

				lvEducationPhase.FocusedRowHandle = lvEducationPhase.VisibleRecordIndex
				Dim layoutview = TryCast(grdEducationPhase.MainView, DevExpress.XtraGrid.Views.Layout.LayoutView)

				If Not (layoutview Is Nothing) Then

					Dim selectedRows = layoutview.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim document = CType(layoutview.GetRow(selectedRows(0)), EducationPhaseLocalViewData)
						Return document
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
				Dim domainName As String = m_InitializationData.MDData.WebserviceDomain
				m_ApplicationUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI)

				success = success AndAlso LoadAssignedEducationData(cvlProfileID, cvlWorkID)

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

		Private Function LoadAssignedEducationData(ByVal cvlProfileID As Integer?, ByVal cvlEducationID As Integer?) As Boolean
			Dim result As Boolean = True

			m_CVLProfileID = cvlProfileID
			m_EducationID = cvlEducationID

			result = result AndAlso PerformEducationPhaseWebservice(m_CVLProfileID, m_EducationID)

			Return True

		End Function

		Public Overrides Sub Reset()

			m_CVLProfileID = Nothing
			m_EducationID = Nothing

			ResetNavigationGrid()
			ResetEducationPhaseFields()

		End Sub

#End Region


#Region "reset"

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
			columnProfileID.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnProfileID.Name = "ID"
			columnProfileID.FieldName = "ID"
			columnProfileID.Visible = False
			columnProfileID.Width = 10
			gvNavigation.Columns.Add(columnProfileID)

			Dim columnDateFromToViewData As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDateFromToViewData.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDateFromToViewData.OptionsColumn.AllowEdit = False
			columnDateFromToViewData.Caption = m_Translate.GetSafeTranslationValue("Zeitraum")
			columnDateFromToViewData.Name = "DateFromToViewData"
			columnDateFromToViewData.FieldName = "DateFromToViewData"
			columnDateFromToViewData.Visible = True
			columnDateFromToViewData.Width = 50
			gvNavigation.Columns.Add(columnDateFromToViewData)


			grdNavigation.DataSource = Nothing

		End Sub

		Private Sub ResetEducationPhaseFields()
			Dim repItemBoolean As New RepositoryItemCheckEdit()
			Dim repItemEdit As New RepositoryItemMemoEdit()

			lvEducationPhase.Columns.Clear()
			With lvEducationPhase
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
				.CardMinSize = New Size(grdEducationPhase.Width - 200, 100) 'grdEducationPhase.Height - 100)
			End With
			lvEducationPhase.CardCaptionFormat = m_Translate.GetSafeTranslationValue("[Ausbildung: {0} von {1}]")

			repItemEdit.ScrollBars = ScrollBars.Both
			repItemEdit.LinesCount = 2


			Dim columnID As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnID.OptionsColumn.AllowEdit = False
			columnID.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnID.Name = "ID"
			columnID.FieldName = "ID"
			lvEducationPhase.Columns.Add(columnID)
			columnID.LayoutViewField.Visibility = LayoutVisibility.Never

			Dim columnDateFromToViewData As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnDateFromToViewData.OptionsColumn.AllowEdit = False
			columnDateFromToViewData.Caption = m_Translate.GetSafeTranslationValue("Zeitraum")
			columnDateFromToViewData.Name = "DateFromToViewData"
			columnDateFromToViewData.FieldName = "DateFromToViewData"
			lvEducationPhase.Columns.Add(columnDateFromToViewData)

			Dim columnDurationViewData As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnDurationViewData.OptionsColumn.AllowEdit = False
			columnDurationViewData.Caption = m_Translate.GetSafeTranslationValue("Dauer")
			columnDurationViewData.Name = "DurationViewData"
			columnDurationViewData.FieldName = "DurationViewData"
			lvEducationPhase.Columns.Add(columnDurationViewData)

			Dim columnSkillViewData As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnSkillViewData.OptionsColumn.AllowEdit = False
			columnSkillViewData.Caption = m_Translate.GetSafeTranslationValue("Erworbene Fähigkeit(en)")
			columnSkillViewData.Name = "SkillViewData"
			columnSkillViewData.FieldName = "SkillViewData"
			columnSkillViewData.ColumnEdit = repItemEdit
			columnSkillViewData.UnboundType = DevExpress.Data.UnboundColumnType.String
			lvEducationPhase.Columns.Add(columnSkillViewData)
			'columnSkillViewData.LayoutViewField.Size = New Size(50, 50)
			'columnSkillViewData.LayoutViewField.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom

			Dim columnOperationAreasViewData As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnOperationAreasViewData.OptionsColumn.AllowEdit = False
			columnOperationAreasViewData.Caption = m_Translate.GetSafeTranslationValue("Tätigkeiten")
			columnOperationAreasViewData.Name = "OperationAreasViewData"
			columnOperationAreasViewData.FieldName = "OperationAreasViewData"
			columnOperationAreasViewData.ColumnEdit = repItemEdit
			columnOperationAreasViewData.UnboundType = DevExpress.Data.UnboundColumnType.String
			lvEducationPhase.Columns.Add(columnOperationAreasViewData)
			columnOperationAreasViewData.LayoutViewField.Size = New Size(50, 50)
			columnOperationAreasViewData.LayoutViewField.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom

			Dim columnLocationViewData As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnLocationViewData.OptionsColumn.AllowEdit = False
			columnLocationViewData.Caption = m_Translate.GetSafeTranslationValue("Einsatzort(e)")
			columnLocationViewData.Name = "LocationViewData"
			columnLocationViewData.FieldName = "LocationViewData"
			lvEducationPhase.Columns.Add(columnLocationViewData)

			Dim columnComments As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnComments.OptionsColumn.AllowEdit = False
			columnComments.Caption = m_Translate.GetSafeTranslationValue("Weitere Informationen")
			columnComments.Name = "Comments"
			columnComments.FieldName = "Comments"
			repItemEdit.LinesCount = 3
			repItemEdit.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
			columnComments.ColumnEdit = repItemEdit

			lvEducationPhase.Columns.Add(columnComments)
			'columnComments.LayoutViewField.Size = New Size(50, 50)
			'columnComments.LayoutViewField.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.SupportHorzAlignment

			Dim columnSchoolnameViewData As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnSchoolnameViewData.OptionsColumn.AllowEdit = False
			columnSchoolnameViewData.Caption = m_Translate.GetSafeTranslationValue("Ausbildungsstätte")
			columnSchoolnameViewData.Name = "SchoolnameViewData"
			columnSchoolnameViewData.FieldName = "SchoolnameViewData"
			lvEducationPhase.Columns.Add(columnSchoolnameViewData)

			Dim columnGraduationViewData As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnGraduationViewData.OptionsColumn.AllowEdit = False
			columnGraduationViewData.Caption = m_Translate.GetSafeTranslationValue("Erworbener Title")
			columnGraduationViewData.Name = "GraduationViewData"
			columnGraduationViewData.FieldName = "GraduationViewData"
			columnGraduationViewData.Width = 10
			columnGraduationViewData.Visible = True
			lvEducationPhase.Columns.Add(columnGraduationViewData)

			Dim columnEducationTypeViewData As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnEducationTypeViewData.OptionsColumn.AllowEdit = False
			columnEducationTypeViewData.Caption = m_Translate.GetSafeTranslationValue("Ausbildungstyp")
			columnEducationTypeViewData.Name = "EducationTypeViewData"
			columnEducationTypeViewData.FieldName = "EducationTypeViewData"
			lvEducationPhase.Columns.Add(columnEducationTypeViewData)

			Dim columnIsCedCodeLable As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnIsCedCodeLable.OptionsColumn.AllowEdit = False
			columnIsCedCodeLable.Caption = m_Translate.GetSafeTranslationValue("Bildungsniveau")
			columnIsCedCodeLable.Name = "IsCedCodeLable"
			columnIsCedCodeLable.FieldName = "IsCedCodeLable"
			lvEducationPhase.Columns.Add(columnIsCedCodeLable)

			Dim columnCompleted As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnCompleted.OptionsColumn.AllowEdit = False
			columnCompleted.Caption = m_Translate.GetSafeTranslationValue("Absolviert")
			columnCompleted.Name = "CompletedViewData"
			columnCompleted.FieldName = "CompletedViewData"
			lvEducationPhase.Columns.Add(columnCompleted)

			Dim columnScore As New DevExpress.XtraGrid.Columns.LayoutViewColumn()
			columnScore.OptionsColumn.AllowEdit = False
			columnScore.Caption = m_Translate.GetSafeTranslationValue("Durchschnittsnote (0-100%)")
			columnScore.Name = "Score"
			columnScore.FieldName = "Score"
			lvEducationPhase.Columns.Add(columnScore)

			For Each col As DevExpress.XtraGrid.Columns.LayoutViewColumn In lvEducationPhase.Columns
				col.Visible = True
			Next

			grdEducationPhase.DataSource = Nothing


		End Sub

#End Region


		''' <summary>
		'''  Performs loading cvl education data.
		''' </summary>
		Private Function PerformEducationPhaseWebservice(ByVal profileID As Integer?, ByVal cvlEducationID As Integer?) As Boolean

			Dim webservice As New Internal.Automations.SPApplicationWebService.SPApplicationSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

			Trace.WriteLine(String.Format("Customer_ID: {0} contacting...", m_customerID))

			' Read data over webservice
			Dim searchResult = webservice.LoadCVLEducationPhaseViewData(m_customerID, profileID, cvlEducationID)
			If searchResult Is Nothing Then
				m_Logger.LogError("LoadCVLEducationPhaseViewData: value is nothing!")
				Return False
			End If

			Dim gridData = (From person In searchResult
											Select New EducationPhaseLocalViewData With {.ID = person.ID,
											.EducationID = person.EducationID,
											.PhaseID = person.PhaseID,
											.EducationPhaseID = person.EducationPhaseID,
											.SchooolNames = person.SchooolNames,
											.Graduations = person.Graduations,
											.EducationTypes = person.EducationTypes,
											.Completed = person.Completed,
											.Score = person.Score,
											.IsCedCode = person.IsCedCode,
											.IsCedCodeLable = person.IsCedCodeLable,
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

			Dim listDataSource As BindingList(Of EducationPhaseLocalViewData) = New BindingList(Of EducationPhaseLocalViewData)
			For Each p In gridData
				listDataSource.Add(p)
			Next

			m_EducationPhaseData = listDataSource
			grdEducationPhase.DataSource = m_EducationPhaseData
			grdNavigation.DataSource = m_EducationPhaseData


			Return Not (m_EducationPhaseData Is Nothing)

		End Function

		Private Sub lvEducationPhase_VisibleRecordIndexChanged(sender As Object, e As LayoutViewVisibleRecordIndexChangedEventArgs) Handles lvEducationPhase.VisibleRecordIndexChanged
			Dim view As LayoutView = TryCast(sender, LayoutView)
			If view Is Nothing Then Return
			If m_EducationPhaseData Is Nothing OrElse m_EducationPhaseData.Count = 0 Then Return

			lvEducationPhase.Focus()
			Dim data = SelectedRecordViewData
			If data Is Nothing Then
				'Trace.WriteLine("error finding data!!!")

				Return
			End If
			Dim myID As Integer = data.PhaseID
			FocusMainGrid(myID)

		End Sub

		Private Sub lvEducationPhase_MouseWheel(sender As Object, e As MouseEventArgs) Handles lvEducationPhase.MouseWheel
			Dim view As LayoutView = TryCast(sender, LayoutView)
			If view Is Nothing Then Return
			If m_EducationPhaseData Is Nothing OrElse m_EducationPhaseData.Count = 0 Then Return

			lvEducationPhase.Focus()
			Dim data = SelectedRecordViewData
			If data Is Nothing Then
				'Trace.WriteLine("error finding data!!!")

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

				Dim documentViewData = CType(gvNavigation.DataSource, BindingList(Of EducationPhaseLocalViewData))

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
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Education Phase-Daten konnte nicht geladen werden."))
				Return
			End If

			success = success AndAlso DisplayAssignedEducationPhaseData(data)

		End Sub

		Private Function DisplayAssignedEducationPhaseData(ByVal data As EducationPhaseLocalViewData) As Boolean
			Dim success As Boolean = True

			Return success

		End Function

		Private Sub OnlvEducationPhase_CustomFieldValueStyle(ByVal sender As System.Object, ByVal e As LayoutViewFieldValueStyleEventArgs) Handles lvEducationPhase.CustomFieldValueStyle

			If e Is Nothing Then Return
			Try
				Dim view As LayoutView = TryCast(sender, LayoutView)
				If view Is Nothing Then Return
				If Not view.Columns("DurationViewData") Is Nothing AndAlso Not view.Columns("LocationViewData") Is Nothing AndAlso Not view.Columns("SkillViewData") Is Nothing AndAlso Not view.Columns("Comments") Is Nothing AndAlso
					Not view.Columns("SchoolnameViewData") Is Nothing AndAlso Not view.Columns("OperationAreasViewData") Is Nothing AndAlso Not view.Columns("GraduationViewData") Is Nothing AndAlso Not view.Columns("EducationTypeViewData") Is Nothing AndAlso
					 Not view.Columns("IsCedCodeLable") Is Nothing AndAlso Not view.Columns("Score") Is Nothing Then
					Return
				End If


				If Not (e.Column.FieldName <> "DurationViewData" OrElse e.Column.FieldName <> "LocationViewData" OrElse e.Column.FieldName <> "Comments" OrElse e.Column.FieldName <> "OperationAreasViewData" OrElse e.Column.FieldName <> "SkillViewData" OrElse
					e.Column.FieldName <> "SchoolnameViewData" OrElse e.Column.FieldName <> "GraduationViewData" _
					 OrElse e.Column.FieldName <> "IsCedCodeLable" OrElse e.Column.FieldName <> "Score" OrElse e.Column.FieldName <> "EducationTypeViewData") Then Return


				Dim showDurationViewData As Boolean = Not (view.GetRowCellValue(e.RowHandle, "DurationViewData") Is Nothing OrElse String.IsNullOrWhiteSpace(view.GetRowCellValue(e.RowHandle, "DurationViewData").ToString()))
				Dim showLocationViewData As Boolean = Not (view.GetRowCellValue(e.RowHandle, "LocationViewData") Is Nothing OrElse String.IsNullOrWhiteSpace(view.GetRowCellValue(e.RowHandle, "LocationViewData").ToString()))
				Dim showComments As Boolean = Not (view.GetRowCellValue(e.RowHandle, "Comments") Is Nothing OrElse String.IsNullOrWhiteSpace(view.GetRowCellValue(e.RowHandle, "Comments").ToString()))
				Dim showSchoolnameViewData As Boolean = Not (view.GetRowCellValue(e.RowHandle, "SchoolnameViewData") Is Nothing OrElse String.IsNullOrWhiteSpace(view.GetRowCellValue(e.RowHandle, "SchoolnameViewData").ToString()))
				Dim showSkillViewData As Boolean = Not (view.GetRowCellValue(e.RowHandle, "SkillViewData") Is Nothing OrElse String.IsNullOrWhiteSpace(view.GetRowCellValue(e.RowHandle, "SkillViewData").ToString()))
				Dim showOperationAreasViewData As Boolean = Not (view.GetRowCellValue(e.RowHandle, "OperationAreasViewData") Is Nothing OrElse String.IsNullOrWhiteSpace(view.GetRowCellValue(e.RowHandle, "OperationAreasViewData").ToString()))
				Dim showGraduationViewData As Boolean = Not (view.GetRowCellValue(e.RowHandle, "GraduationViewData") Is Nothing OrElse String.IsNullOrWhiteSpace(view.GetRowCellValue(e.RowHandle, "GraduationViewData").ToString()))
				Dim showEducationTypeViewData As Boolean = Not (view.GetRowCellValue(e.RowHandle, "EducationTypeViewData") Is Nothing OrElse String.IsNullOrWhiteSpace(view.GetRowCellValue(e.RowHandle, "EducationTypeViewData").ToString()))
				Dim showIsCedCodeLable As Boolean = Not (view.GetRowCellValue(e.RowHandle, "IsCedCodeLable") Is Nothing OrElse String.IsNullOrWhiteSpace(view.GetRowCellValue(e.RowHandle, "IsCedCodeLable").ToString()))
				Dim showScore As Boolean = Not (view.GetRowCellValue(e.RowHandle, "Score") Is Nothing OrElse Val(view.GetRowCellValue(e.RowHandle, "Score").ToString()) = 0)

				If Not view.Columns("DurationViewData") Is Nothing Then view.Columns("DurationViewData").LayoutViewField.Visibility = If(showDurationViewData, LayoutVisibility.Always, LayoutVisibility.Never)
				If Not view.Columns("LocationViewData") Is Nothing Then view.Columns("LocationViewData").LayoutViewField.Visibility = If(showLocationViewData, LayoutVisibility.Always, LayoutVisibility.Never)
				If Not view.Columns("SkillViewData") Is Nothing Then view.Columns("SkillViewData").LayoutViewField.Visibility = If(showSkillViewData, LayoutVisibility.Always, LayoutVisibility.Never)
				If Not view.Columns("Comments") Is Nothing Then view.Columns("Comments").LayoutViewField.Visibility = If(showComments, LayoutVisibility.Always, LayoutVisibility.Never)
				If Not view.Columns("SchoolnameViewData") Is Nothing Then view.Columns("SchoolnameViewData").LayoutViewField.Visibility = If(showSchoolnameViewData, LayoutVisibility.Always, LayoutVisibility.Never)
				If Not view.Columns("OperationAreasViewData") Is Nothing Then view.Columns("OperationAreasViewData").LayoutViewField.Visibility = If(showOperationAreasViewData, LayoutVisibility.Always, LayoutVisibility.Never)
				If Not view.Columns("GraduationViewData") Is Nothing Then view.Columns("GraduationViewData").LayoutViewField.Visibility = If(showGraduationViewData, LayoutVisibility.Always, LayoutVisibility.Never)
				If Not view.Columns("EducationTypeViewData") Is Nothing Then view.Columns("EducationTypeViewData").LayoutViewField.Visibility = If(showEducationTypeViewData, LayoutVisibility.Always, LayoutVisibility.Never)
				If Not view.Columns("IsCedCodeLable") Is Nothing Then view.Columns("IsCedCodeLable").LayoutViewField.Visibility = If(showIsCedCodeLable, LayoutVisibility.Always, LayoutVisibility.Never)
				If Not view.Columns("Score") Is Nothing Then view.Columns("Score").LayoutViewField.Visibility = If(showScore, LayoutVisibility.Always, LayoutVisibility.Never)


			Catch ex As Exception

			End Try

		End Sub


#Region "Helpers Class"

		Private Class EducationPhaseLocalViewData
			Inherits EducationPhaseViewDataDTO


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

			Public ReadOnly Property SchoolnameViewData As String
				Get
					If SchooolNames Is Nothing OrElse SchooolNames.Count = 0 Then Return Nothing
					Dim value As String = String.Empty
					For Each itm In SchooolNames
						value &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(value), "", ", "), itm.Lable)
					Next

					Return value
				End Get
			End Property

			Public ReadOnly Property GraduationViewData As String
				Get
					If Graduations Is Nothing OrElse Graduations.Count = 0 Then Return Nothing
					Dim value As String = String.Empty
					For Each itm In Graduations
						value &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(value), "", ", "), itm.Lable)
					Next

					Return value
				End Get
			End Property

			Public ReadOnly Property EducationTypeViewData As String
				Get
					If EducationTypes Is Nothing OrElse EducationTypes.Count = 0 Then Return Nothing
					Dim value As String = String.Empty
					For Each itm In EducationTypes
						value &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(value), "", ", "), itm.Name)
					Next

					Return value
				End Get
			End Property

			Public ReadOnly Property CompletedViewData As String
				Get
					Dim value As String = "Ja"
					If Not Completed.GetValueOrDefault(False) Then value = "Nein"

					Return value
				End Get
			End Property

#End Region


		End Class

#End Region

	End Class

End Namespace

