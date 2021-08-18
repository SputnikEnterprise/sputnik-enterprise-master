Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.PayrollMng
Imports SPProgUtility.Mandanten
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports SP.DatabaseAccess.PayrollMng.DataObjects

Namespace UI

	Public Class frmZGMA

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
		''' The common database access.
		''' </summary>
		Private m_CommonDatabaseAccess As ICommonDatabaseAccess

		''' <summary>
		''' The Payroll data access object.
		''' </summary>
		Private m_PayrollDatabaseAccess As IPayrollDatabaseAccess

		''' <summary>
		''' The mandant.
		''' </summary>
		Private m_md As Mandant

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI

		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As Utility

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		''' <summary>
		''' Boolean flag indicating if initial data has been loaded.
		''' </summary>
		Private m_IsInitialDataLoaded As Boolean = False

		''' <summary>
		''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
		''' </summary>
		Private m_SuppressUIEvents As Boolean = False

		''' <summary>
		''' The Mandant number.
		''' </summary>
		Private m_MDNr As Integer

#End Region

#Region "Constructor"


		''' <summary>
		''' The constructor.
		''' </summary>
		Public Sub New(ByVal mdnr As Integer, ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			m_MDNr = mdnr

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
			Try
				m_md = New Mandant
				m_InitializationData = _setting
				m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
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

			Dim conStr = m_md.GetSelectedMDData(mdnr).MDDbConn
			m_CommonDatabaseAccess = New DatabaseAccess.Common.CommonDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
			m_PayrollDatabaseAccess = New DatabaseAccess.PayrollMng.PayrollDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)

			m_UtilityUI = New UtilityUI
			m_Utility = New Utility

			' Translate controls.
			TranslateControls()

			Reset()

		End Sub

#End Region

#Region "Public Properties"


		''' <summary>
		''' Gets the selected employee data.
		''' </summary>
		''' <returns>The selected employee data or nothing if none is selected.</returns>
		Public ReadOnly Property SelectedEmployeeData As EmployeeDataForZV
			Get
				Dim grdView = TryCast(grdEmployees.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (grdView Is Nothing) Then

					Dim selectedRows = grdView.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim employeeDataForZV = CType(grdView.GetRow(selectedRows(0)), EmployeeDataForZV)
						Return employeeDataForZV
					End If

				End If

				Return Nothing
			End Get

		End Property

#End Region

#Region "Public Methods"

		''' <summary>
		''' Loads the data.
		''' </summary>
		''' <param name="employeeNumbers">The employee numbers to load.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function LoadData(ByVal employeeNumbers As Integer()) As Boolean

			Dim success As Boolean = True

			success = success AndAlso LoadEmployeeDataForZV(employeeNumbers)

			Return success
		End Function

#End Region

#Region "Private Methods"

		''' <summary>
		'''  Trannslate controls.
		''' </summary>
		Private Sub TranslateControls()
			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

			Me.lblHeadingText.Text = m_Translate.GetSafeTranslationValue(Me.lblHeadingText.Text)
			Me.lblHeadingSubText.Text = m_Translate.GetSafeTranslationValue(Me.lblHeadingSubText.Text)

			Me.lblGridCaption.Text = m_Translate.GetSafeTranslationValue(Me.lblGridCaption.Text)

			Me.btnClose.Text = m_Translate.GetSafeTranslationValue(Me.btnClose.Text)
			Me.btnOpenZVForm.Text = m_Translate.GetSafeTranslationValue(Me.btnOpenZVForm.Text)
			Me.btnOpenAGForm.Text = m_Translate.GetSafeTranslationValue(Me.btnOpenAGForm.Text)

		End Sub

		''' <summary>
		''' Resets the form.
		''' </summary>
		Private Sub Reset()

			' ---Reset drop downs, grids and lists---
			ResetEmployeeGrid()
		End Sub


		''' <summary>
		''' Resets the employee data grid.
		''' </summary>
		Private Sub ResetEmployeeGrid()

			' Reset the grid

			gvEmployees.OptionsView.ShowIndicator = False
			'gvEmployees.OptionsSelection.EnableAppearanceFocusedRow = False
			gvEmployees.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvEmployees.Columns.Clear()

			Dim columnZGNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZGNr.Caption = m_Translate.GetSafeTranslationValue("Vorschuss-Nr.")
			columnZGNr.Name = "ZGNr"
			columnZGNr.FieldName = "ZGNr"
			columnZGNr.Visible = True
			gvEmployees.Columns.Add(columnZGNr)

			Dim columnLastName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLastName.Caption = m_Translate.GetSafeTranslationValue("Nachname")
			columnLastName.Name = "LastName"
			columnLastName.FieldName = "LastName"
			columnLastName.Visible = True
			gvEmployees.Columns.Add(columnLastName)

			Dim columnFirstname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnFirstname.Caption = m_Translate.GetSafeTranslationValue("Vorname")
			columnFirstname.Name = "FirstName"
			columnFirstname.FieldName = "FirstName"
			columnFirstname.Visible = True
			gvEmployees.Columns.Add(columnFirstname)

			grdEmployees.DataSource = Nothing

		End Sub

		''' <summary>
		''' Loads employee data for ZV.
		''' </summary>
		''' <param name="employeeNumbers">The employee numbers.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadEmployeeDataForZV(ByVal employeeNumbers As Integer()) As Boolean

			Dim listOfEmployeeDataForZV As List(Of EmployeeDataForZV) = Nothing

			If employeeNumbers Is Nothing Then

				listOfEmployeeDataForZV = m_PayrollDatabaseAccess.LoadListOfAllKandidaten4NotCreatedZV(m_InitializationData.UserData.UserFName & " " & m_InitializationData.UserData.UserLName, m_MDNr)

			Else
				listOfEmployeeDataForZV = m_PayrollDatabaseAccess.LoadEmployeeData4NotCreatedZV(employeeNumbers)

			End If

			grdEmployees.DataSource = listOfEmployeeDataForZV

			If listOfEmployeeDataForZV Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mitarbeiterdaten konnten nicht geladnen werden"))
				Return False
			End If

			Return Not listOfEmployeeDataForZV Is Nothing

		End Function

#End Region

#Region "Event Handlers"

		''' <summary>
		''' Handles click on close button.
		''' </summary>
		Private Sub OnBtnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
			Close()
		End Sub

		''' <summary>
		'''Handles clik on btnOpenZVForm.
		''' </summary>
		Private Sub OnBtnOpenZVForm_Click(sender As Object, e As EventArgs) Handles btnOpenZVForm.Click
			Dim employeeData = SelectedEmployeeData

			If Not employeeData Is Nothing Then
				Dim oMyProg As Object
				Dim strTranslationProgName As String = String.Empty
				Dim _ClsReg As New SPProgUtility.ClsDivReg

				Try
					oMyProg = CreateObject("SPSModulsView.ClsMain")
					oMyProg.TranslateProg4Net("SPSMAZVUtil.ClsMain", employeeData.MANr)


				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}", ex.ToString()))
					m_UtilityUI.ShowErrorDialog(String.Format("{0}", ex.ToString()))

				End Try

			End If

		End Sub

		''' <summary>
		'''Handles clik on btnOpenAGForm.
		''' </summary>
		Private Sub OnBtnOpenAGForm_Click(sender As Object, e As EventArgs) Handles btnOpenAGForm.Click
			Dim employeeData = SelectedEmployeeData

			If Not employeeData Is Nothing Then
				Dim oMyProg As Object
				Dim strTranslationProgName As String = String.Empty
				Dim _ClsReg As New SPProgUtility.ClsDivReg

				Try
					oMyProg = CreateObject("SPSModulsView.ClsMain")
					oMyProg.TranslateProg4Net("SPSMaArgbUtil.ClsMain", employeeData.MANr)


				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}", ex.ToString()))
					m_UtilityUI.ShowErrorDialog(String.Format("{0}", ex.ToString()))

				End Try
			End If

		End Sub

#End Region

	End Class

End Namespace
