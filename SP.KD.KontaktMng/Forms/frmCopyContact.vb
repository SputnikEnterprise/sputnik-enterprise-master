Imports SP.DatabaseAccess.Customer
Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.Infrastructure.Logging
Imports System.ComponentModel
Imports DevExpress.XtraGrid.Views.Grid

''' <summary>
''' Copy contacts.
''' </summary>
Public Class frmCopyContact

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
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The customer data access object.
	''' </summary>
	Private m_CustomerDataAccess As ICustomerDatabaseAccess

	''' <summary>
	''' The settings manager.
	''' </summary>
	Protected m_SettingsManager As ISettingsManager

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As Utility

	''' <summary>
	''' List of all employee view data.
	''' </summary>
	Private m_ListOfAllEmplyoeeViewData As BindingList(Of EmployeeViewData)

	''' <summary>
	''' List of selected employee view data.
	''' </summary>
	Private m_ListOfSelectedEmployeeViewData As New BindingList(Of EmployeeViewData)

#End Region

#Region "Constructor"

	''' <summary>
	''' The consturctor.
	''' </summary>
	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		' Dieser Aufruf ist für den Designer erforderlich.
		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

		Try

			m_InitializationData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		m_CustomerDataAccess = New DatabaseAccess.Customer.CustomerDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_SettingsManager = New SettingsManager
		m_UtilityUI = New UtilityUI
		m_Utility = New Utility

		Reset()

	End Sub

#End Region

#Region "Public Properties"

	''' <summary>
	''' Gets the selected employee in the all employees grid.
	''' </summary>
	''' <returns>The selected employee or nothing if none is selected.</returns>
	Public ReadOnly Property SelectedEmployeeInAllEmployeesGridViewData As EmployeeViewData
		Get
			Dim grdView = TryCast(gridAllEmployees.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim employee = CType(grdView.GetRow(selectedRows(0)), EmployeeViewData)
					Return employee
				End If

			End If

			Return Nothing
		End Get

	End Property

	''' <summary>
	''' Gets the selected employee in the 'selected' employees grid.
	''' </summary>
	''' <returns>The selected employee or nothing if none is selected.</returns>
	Public ReadOnly Property SelectedEmployeeInSelectedEmployeesGridViewData As EmployeeViewData
		Get
			Dim grdView = TryCast(gridSelectedEmployees.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim employee = CType(grdView.GetRow(selectedRows(0)), EmployeeViewData)
					Return employee
				End If

			End If

			Return Nothing
		End Get

	End Property

	''' <summary>
	''' Gets the selected employee numbers.
	''' </summary>
	''' <value>Array of selected employee numbers </value>
	''' <returns></returns>
	Public ReadOnly Property SelectedEmployeeNumbers As Integer()
		Get
			Return (From employeeNumber In
						 m_ListOfSelectedEmployeeViewData.Select(Function(data) data.EmployeeNumber)).ToArray()
		End Get
	End Property

#End Region

#Region "Public Methods"

	''' <summary>
	''' Loads the data.
	''' </summary>
	''' <param name="initialSelectedEmployees">The inital selected employees.</param>
	Public Sub LoadData(Optional ByVal initialSelectedEmployees As Integer() = Nothing)

		Reset()

		LoadAllEmployeesGridData()
		InitSelectedEmployeesGridData(initialSelectedEmployees)

	End Sub

	''' <summary>
	''' Focuses a record.
	''' </summary>
	''' <param name="employeeNumber">The employee number.</param>
	Public Sub FocusEmployee(ByVal employeeNumber As Integer)

		If Not gridAllEmployees.DataSource Is Nothing Then

			Dim contactViewData = CType(gvAllEmployees.DataSource, BindingList(Of EmployeeViewData))
			gvAllEmployees.OptionsSelection.EnableAppearanceFocusedRow = True

			Dim index = contactViewData.ToList().FindIndex(Function(data) data.EmployeeNumber = employeeNumber)
			Dim rowHandle = gvAllEmployees.GetRowHandle(index)
			gvAllEmployees.FocusedRowHandle = rowHandle

			If (gvAllEmployees.IsRowVisible(rowHandle) = RowVisibleState.Hidden) Then
				gvAllEmployees.TopRowIndex = rowHandle - 1
			End If

		End If

	End Sub

#End Region


#Region "Private Methods"

	''' <summary>
	'''  Trannslate controls.
	''' </summary>
	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

		Me.btnCopyContact.Text = m_Translate.GetSafeTranslationValue(Me.btnCopyContact.Text)
		Me.btnCancel.Text = m_Translate.GetSafeTranslationValue(Me.btnCancel.Text)

	End Sub

	''' <summary>
	''' Resets the from.
	''' </summary>
	Private Sub Reset()

		ResetAllEmployeesGrid()
		ResetSelectedEmployeesGrid()

		TranslateControls()

	End Sub

	''' <summary>
	''' Resets the all employees grid.
	''' </summary>
	Private Sub ResetAllEmployeesGrid()

		gvAllEmployees.OptionsView.ShowAutoFilterRow = True

		' Reset the grid
		gvAllEmployees.Columns.Clear()
		gvAllEmployees.OptionsView.ShowIndicator = False

		Dim columnEmployeeNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeeNumber.Caption = m_Translate.GetSafeTranslationValue("Kandidaten-Nr.")
		columnEmployeeNumber.Name = "EmployeeNumber"
		columnEmployeeNumber.FieldName = "EmployeeNumber"
		columnEmployeeNumber.Visible = True
		gvAllEmployees.Columns.Add(columnEmployeeNumber)

		Dim columnName As New DevExpress.XtraGrid.Columns.GridColumn()
		columnName.Caption = m_Translate.GetSafeTranslationValue("Name")
		columnName.Name = "Name"
		columnName.FieldName = "Name"
		columnName.Visible = True
		columnName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvAllEmployees.Columns.Add(columnName)

		Dim columnAddress As New DevExpress.XtraGrid.Columns.GridColumn()
		columnAddress.Caption = m_Translate.GetSafeTranslationValue("Addresse")
		columnAddress.Name = "Address"
		columnAddress.FieldName = "Address"
		columnAddress.Visible = True
		columnAddress.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvAllEmployees.Columns.Add(columnAddress)

	End Sub

	''' <summary>
	''' Resets selected employees grid.
	''' </summary>
	Private Sub ResetSelectedEmployeesGrid()

		' Reset the grid
		gvSelectedEmployees.Columns.Clear()
		gvSelectedEmployees.OptionsView.ShowIndicator = False

		Dim columnName As New DevExpress.XtraGrid.Columns.GridColumn()
		columnName.Caption = m_Translate.GetSafeTranslationValue("Name")
		columnName.Name = "Name"
		columnName.FieldName = "Name"
		columnName.Visible = True
		columnName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvSelectedEmployees.Columns.Add(columnName)

		Dim columnAddress As New DevExpress.XtraGrid.Columns.GridColumn()
		columnAddress.Caption = m_Translate.GetSafeTranslationValue("Addresse")
		columnAddress.Name = "Address"
		columnAddress.FieldName = "Address"
		columnAddress.Visible = True
		columnAddress.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvSelectedEmployees.Columns.Add(columnAddress)

	End Sub


	''' <summary>
	''' Loads all employee grid data..
	''' </summary>
	''' <returns>Boolean value indicating success.</returns>
	Private Function LoadAllEmployeesGridData() As Boolean

		Dim employeeData = m_CustomerDataAccess.LoadEmployeeData()

		If employeeData Is Nothing Then
			Return False
		End If

		m_ListOfAllEmplyoeeViewData = New BindingList(Of EmployeeViewData)

		For Each emplData In employeeData
			m_ListOfAllEmplyoeeViewData.Add(TranformToEmployeeViewData(emplData))
		Next

		gridAllEmployees.DataSource = m_ListOfAllEmplyoeeViewData

		Return True

	End Function

	''' <summary>
	''' Inits the selected employees grid data.
	''' </summary>
	''' <param name="initialSelectedEmployees">The inital selected employees.</param>
	Private Sub InitSelectedEmployeesGridData(ByVal initialSelectedEmployees As Integer())
		If Not initialSelectedEmployees Is Nothing Then

			Dim emplyoees = m_ListOfAllEmplyoeeViewData.Where(Function(data) initialSelectedEmployees.Contains(data.EmployeeNumber)).ToArray

			For Each emplyoeeViewData In emplyoees
				m_ListOfSelectedEmployeeViewData.Add(emplyoeeViewData)
			Next

		End If

		gridSelectedEmployees.DataSource = m_ListOfSelectedEmployeeViewData

	End Sub

	''' <summary>
	''' Transforms employee data to view data.
	''' </summary>
	''' <param name="employeeData">The employee data.</param>
	''' <returns>The employee view data.</returns>
	Private Function TranformToEmployeeViewData(ByVal employeeData As EmployeeData) As EmployeeViewData

		Dim employeeVieData As New EmployeeViewData
		Dim genderText As String = String.Empty
		Dim gender As String = If(employeeData.Gender Is Nothing, "M", employeeData.Gender)

		Select Case gender.ToUpper()
			Case "M"
				genderText = "Herr"
			Case "W"
				genderText = "Frau"
			Case Else
				genderText = String.Empty
		End Select
		genderText = m_Translate.GetSafeTranslationValue(genderText)

		employeeVieData.EmployeeNumber = employeeData.EmployeeNumber
		employeeVieData.Name = String.Format("{0}, {1}", employeeData.Lastname, employeeData.Firstname).Trim()
		employeeVieData.Address = String.Format("{0}, {1}-{2} {3}", employeeData.Street, employeeData.CountryCode, employeeData.Postcode, employeeData.Location).Trim()

		Return employeeVieData
	End Function

	''' <summary>
	''' Handles click on all employees row.
	''' </summary>
	Private Sub OnGridAllEmployees_DoubleClick(sender As System.Object, e As System.EventArgs) Handles gridAllEmployees.DoubleClick

		Dim selectedEmployee = SelectedEmployeeInAllEmployeesGridViewData

		If Not selectedEmployee Is Nothing AndAlso
			Not m_ListOfSelectedEmployeeViewData.Any(Function(data) data.EmployeeNumber = selectedEmployee.EmployeeNumber) Then

			m_ListOfSelectedEmployeeViewData.Add(selectedEmployee)

		End If

	End Sub

	''' <summary>
	''' Handles click on copy contact button.
	''' </summary>
	Private Sub OnBtnCopyContact_Click(sender As System.Object, e As System.EventArgs) Handles btnCopyContact.Click
		DialogResult = DialogResult.OK
		Close()
	End Sub

	''' <summary>
	''' Handles click on cancel button.
	''' </summary>
	''' <param name="sender"></param>
	Private Sub OnBtnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
		DialogResult = DialogResult.Cancel
		Close()
	End Sub

#End Region

#Region "View helper classes"

	''' <summary>
	''' Employee view data.
	''' </summary>
	Class EmployeeViewData

		Public Property EmployeeNumber As Integer
		Public Property Name As String
		Public Property Address As String

	End Class

#End Region

End Class