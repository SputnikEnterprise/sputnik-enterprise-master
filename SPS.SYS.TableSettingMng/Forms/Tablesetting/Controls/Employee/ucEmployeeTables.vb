
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng

Imports System.ComponentModel
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraNavBar
Imports DevExpress.XtraEditors
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors.Repository

Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.TableSetting
Imports SP.DatabaseAccess.TableSetting.DataObjects
Imports SP.DatabaseAccess.TableSetting.DataObjects.EmployeeContactData

Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Infrastructure

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages


Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.Mandanten
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraSplashScreen


Public Class ucEmployeeTables


#Region "Private Fields"

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The common data access object.
	''' </summary>
	Private m_TablesettingDatabaseAccess As ITablesDatabaseAccess


	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' The settings manager.
	''' </summary>
	Private m_SettingsManager As ISettingsManager

	Private m_mandant As Mandant

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As Utility

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	Private connectionString As String
	Private m_EmployeecontactData As IEnumerable(Of EmployeeContactData)
	'Private m_SMSTemplateData As IEnumerable(Of SMSTemplateData)

	Private Property m_Selectedmodul As SelectedModulKey

	Public m_Reccount As New Integer

#End Region


#Region "private consts"

	Private Enum SelectedModulKey

		MODUL_EMPLOYEE_CONTACT
		MODUL_EMPLOYEE_LANGUAGES
		MODUL_EMPLOYEE_MAIN_LANGUAGE
		MODUL_EMPLOYEE_MAIN_ASSESSMENT
		MODUL_EMPLOYEE_MAIN_COMMUNICATION

	End Enum


#End Region


#Region "Constructor"

	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_InitializationData = _setting

		m_Selectedmodul = SelectedModulKey.MODUL_EMPLOYEE_CONTACT	' MODUL_EMPLOYEE_CONTACT

		m_mandant = New Mandant

		m_Utility = New Utility
		m_UtilityUI = New UtilityUI
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)


		connectionString = m_InitializationData.MDData.MDDbConn
		m_TablesettingDatabaseAccess = New SP.DatabaseAccess.TableSetting.TablesDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)


		' Translate controls.
		TranslateControls()


		AddHandler gvTableContent.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground

	End Sub


#End Region


#Region "Private Properties"

	''' <summary>
	''' Gets the selected data.
	''' </summary>
	''' <returns>The selected document or nothing if none is selected.</returns>
	Private ReadOnly Property SelectedEmployeeContactViewData As EmployeeContactData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), EmployeeContactData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedLanguageViewData As LanguageData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), LanguageData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedJobLanguageViewData As JobLanguageData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), JobLanguageData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedAssessmentViewData As AssessmentData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), AssessmentData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedCommunicationViewData As CommunicationTypeData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), CommunicationTypeData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property


#End Region



#Region "Private Methods"

	''' <summary>
	'''  Trannslate controls.
	''' </summary>
	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

		Me.rlblHeader.Text = m_Translate.GetSafeTranslationValue(Me.rlblHeader.Text)
		Me.rlblHeader.Text = m_Translate.GetSafeTranslationValue(Me.rlblHeader.Text)

		Me.btnSave.Text = m_Translate.GetSafeTranslationValue(Me.btnSave.Text)
		Me.btnDelete.Text = m_Translate.GetSafeTranslationValue(Me.btnDelete.Text)

	End Sub

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


	Private Sub ResetEmployeeContactGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()

		ChangeHeaderInfo()

		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recid"
			columnrecid.FieldName = "recid"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columndescription As New DevExpress.XtraGrid.Columns.GridColumn()
			columndescription.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columndescription.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columndescription.Name = "description"
			columndescription.FieldName = "description"
			columndescription.Visible = True
			columndescription.BestFit()
			gvTableContent.Columns.Add(columndescription)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_fr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_fr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_fr.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_fr.Name = "bez_f"
			columnbez_fr.FieldName = "bez_f"
			columnbez_fr.Visible = True
			columnbez_fr.BestFit()
			gvTableContent.Columns.Add(columnbez_fr)

			Dim columnbez_e As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_e.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_e.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (EN)")
			columnbez_e.Name = "bez_e"
			columnbez_e.FieldName = "bez_e"
			columnbez_e.Visible = True
			columnbez_e.BestFit()
			gvTableContent.Columns.Add(columnbez_e)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdTableContent.DataSource = Nothing

	End Sub

	Private Sub ResetEmployeeJobLanguageGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()

		ChangeHeaderInfo()

		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recid"
			columnrecid.FieldName = "recid"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columndescription As New DevExpress.XtraGrid.Columns.GridColumn()
			columndescription.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columndescription.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columndescription.Name = "bez_value"
			columndescription.FieldName = "bez_value"
			columndescription.Visible = True
			columndescription.BestFit()
			gvTableContent.Columns.Add(columndescription)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_fr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_fr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_fr.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_fr.Name = "bez_f"
			columnbez_fr.FieldName = "bez_f"
			columnbez_fr.Visible = True
			columnbez_fr.BestFit()
			gvTableContent.Columns.Add(columnbez_fr)

			Dim columnbez_e As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_e.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_e.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (EN)")
			columnbez_e.Name = "bez_e"
			columnbez_e.FieldName = "bez_e"
			columnbez_e.Visible = True
			columnbez_e.BestFit()
			gvTableContent.Columns.Add(columnbez_e)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdTableContent.DataSource = Nothing

	End Sub

	Private Sub ResetEmployeeAssessmentGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()

		ChangeHeaderInfo()

		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recid"
			columnrecid.FieldName = "recid"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columndescription As New DevExpress.XtraGrid.Columns.GridColumn()
			columndescription.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columndescription.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columndescription.Name = "bez_value"
			columndescription.FieldName = "bez_value"
			columndescription.Visible = True
			columndescription.BestFit()
			gvTableContent.Columns.Add(columndescription)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_fr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_fr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_fr.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_fr.Name = "bez_f"
			columnbez_fr.FieldName = "bez_f"
			columnbez_fr.Visible = True
			columnbez_fr.BestFit()
			gvTableContent.Columns.Add(columnbez_fr)

			Dim columnbez_e As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_e.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_e.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (EN)")
			columnbez_e.Name = "bez_e"
			columnbez_e.FieldName = "bez_e"
			columnbez_e.Visible = True
			columnbez_e.BestFit()
			gvTableContent.Columns.Add(columnbez_e)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdTableContent.DataSource = Nothing

	End Sub

	Private Sub ResetEmployeeCommunicationGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()

		ChangeHeaderInfo()

		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recid"
			columnrecid.FieldName = "recid"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columndescription As New DevExpress.XtraGrid.Columns.GridColumn()
			columndescription.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columndescription.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columndescription.Name = "bez_value"
			columndescription.FieldName = "bez_value"
			columndescription.Visible = True
			columndescription.BestFit()
			gvTableContent.Columns.Add(columndescription)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_fr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_fr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_fr.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_fr.Name = "bez_f"
			columnbez_fr.FieldName = "bez_f"
			columnbez_fr.Visible = True
			columnbez_fr.BestFit()
			gvTableContent.Columns.Add(columnbez_fr)

			Dim columnbez_e As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_e.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_e.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (EN)")
			columnbez_e.Name = "bez_e"
			columnbez_e.FieldName = "bez_e"
			columnbez_e.Visible = True
			columnbez_e.BestFit()
			gvTableContent.Columns.Add(columnbez_e)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdTableContent.DataSource = Nothing

	End Sub


	Sub ChangeHeaderInfo()
		Dim modulCaption As String = String.Empty

		Select Case m_Selectedmodul
			Case SelectedModulKey.MODUL_EMPLOYEE_CONTACT
				modulCaption = m_Translate.GetSafeTranslationValue("Kandidaten-Kontaktdaten")

			Case SelectedModulKey.MODUL_EMPLOYEE_MAIN_ASSESSMENT
				modulCaption = m_Translate.GetSafeTranslationValue("Kandidaten Beurteilung")

			Case SelectedModulKey.MODUL_EMPLOYEE_MAIN_COMMUNICATION
				modulCaption = m_Translate.GetSafeTranslationValue("Kandidaten Beurteilung")


			Case Else
				Return


		End Select
		rlblHeader.Text = String.Format("<b><font size=""+6"">{0}</font></b>", modulCaption)

	End Sub


	Sub UpdateRecord()

		Try
			Dim result As Boolean = True

			Select Case m_Selectedmodul
				Case SelectedModulKey.MODUL_EMPLOYEE_CONTACT
					Dim SelectedData = SelectedEmployeeContactViewData

					If SelectedData.recid = 0 Then
						result = m_TablesettingDatabaseAccess.AddEmployeeContactData(SelectedData)
					Else
						result = m_TablesettingDatabaseAccess.UpdateEmployeeContactData(SelectedData)
					End If

					result = result AndAlso LoadEmployeeContactList()

				Case SelectedModulKey.MODUL_EMPLOYEE_MAIN_ASSESSMENT
					Dim SelectedData = SelectedAssessmentViewData

					If SelectedData.recId = 0 Then
						result = m_TablesettingDatabaseAccess.AddAssessmentData(SelectedData)
					Else
						result = m_TablesettingDatabaseAccess.UpdateAssessmentData(SelectedData)
					End If

					result = result AndAlso LoadEmployeeAssessmentList()

				Case SelectedModulKey.MODUL_EMPLOYEE_MAIN_COMMUNICATION
					Dim SelectedData = SelectedCommunicationViewData

					If SelectedData.recid = 0 Then
						result = m_TablesettingDatabaseAccess.AddCommunicationTypeData(SelectedData)
					Else
						result = m_TablesettingDatabaseAccess.UpdateCommunicationTypeData(SelectedData)
					End If

					result = result AndAlso LoadEmployeeAssessmentList()


				Case Else
					Return

			End Select

			If Not result Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))
				Return
			End If

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Sub

	Sub DeleteRecord()

		Try
			Select Case m_Selectedmodul
				Case SelectedModulKey.MODUL_EMPLOYEE_CONTACT
					Dim SelectedData = SelectedEmployeeContactViewData

					Dim result As Boolean = m_TablesettingDatabaseAccess.DeleteEmployeeContactData(SelectedData.recid)

					If Not result Then
						m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gelöscht werden."))
						Return
					End If
					result = LoadEmployeeContactList()



				Case Else
					Return

			End Select

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Sub

	Private Sub gvTableContent_FocusedRowChanged(sender As Object, e As FocusedRowChangedEventArgs) Handles gvTableContent.FocusedRowChanged
		gvTableContent.OptionsBehavior.Editable = False
	End Sub

	Private Sub gvTableContent_RowCellClick(sender As Object, e As RowCellClickEventArgs) Handles gvTableContent.RowCellClick
		If e.Clicks = 2 Then
			gvTableContent.OptionsBehavior.Editable = True
		End If
	End Sub

	Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

		grdTableContent.FocusedView.CloseEditor()
		UpdateRecord()

	End Sub

	Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click

		grdTableContent.FocusedView.CloseEditor()
		DeleteRecord()

	End Sub

#End Region



#Region "Public Methods"

	Public Function LoadEmployeeContactList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			m_EmployeecontactData = m_TablesettingDatabaseAccess.LoadEmployeeContactData()

			If (m_EmployeecontactData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mitarbeiter-Kontaktdaten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In m_EmployeecontactData
			Select New EmployeeContactData With
						 {.bez_value = report.bez_value,
							.recid = report.recid,
							.bez_d = report.bez_d,
							.bez_i = report.bez_i,
							.bez_f = report.bez_f,
							.bez_e = report.bez_e
						 }).ToList()

			Dim listDataSource As BindingList(Of EmployeeContactData) = New BindingList(Of EmployeeContactData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			m_Reccount = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try


	End Function


	Public Function LoadEmployeeAssessmentList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim EmployeeData = m_TablesettingDatabaseAccess.LoadAssessmentData()

			If (m_EmployeecontactData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mitarbeiter-Beurteilungsdaten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In EmployeeData
			Select New AssessmentData With
						 {.bez_value = report.bez_value,
							.recid = report.recid,
							.bez_d = report.bez_d,
							.bez_i = report.bez_i,
							.bez_f = report.bez_f,
							.bez_e = report.bez_e
						 }).ToList()

			Dim listDataSource As BindingList(Of AssessmentData) = New BindingList(Of AssessmentData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			m_Reccount = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try


	End Function

	Public Function LoadEmployeeCommunicationList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim EmployeeData = m_TablesettingDatabaseAccess.LoadCommunicationTypeData()

			If (m_EmployeecontactData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mitarbeiter-kommunikationsdaten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In EmployeeData
			Select New CommunicationTypeData With
						 {.bez_value = report.bez_value,
							.recid = report.recid,
							.bez_d = report.bez_d,
							.bez_i = report.bez_i,
							.bez_f = report.bez_f,
							.bez_e = report.bez_e
						 }).ToList()

			Dim listDataSource As BindingList(Of CommunicationTypeData) = New BindingList(Of CommunicationTypeData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			m_Reccount = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try


	End Function


#End Region


End Class

