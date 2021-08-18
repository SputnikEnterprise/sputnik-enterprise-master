Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports System.ComponentModel
Imports SPProgUtility.ProgPath
Imports SPProgUtility.Mandanten
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages

Namespace UI

	Public Class ucPageEmployeeBasicData


#Region "Private Consts"
		Private Const FORM_XML_REQUIREDFIEKDS_KEY As String = "Forms_Normaly/requiredfields"
		Private Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"
#End Region


#Region "Private Fields"

		''' <summary>
		''' The Initialization data.
		''' </summary>
		Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

		''' <summary>
		''' The translation value helper.
		''' </summary>
		Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

#End Region


#Region "Constructor"

		''' <summary>
		''' The constructor.
		''' </summary>
		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			m_InitializationData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()

			TranslateControls()
			Reset()

			AddHandler gvExistingEmployees.RowCellClick, AddressOf OngvMain_RowCellClick

		End Sub

#End Region

#Region "Public Properties"

		Public Property ExistingEmployeeData As IEnumerable(Of ExistingEmployeeSearchData)

		'Public ReadOnly Property SelectedEmployeeBasicData As InitEmployeeBasicData
		'	Get

		'		Dim data As New InitEmployeeBasicData

		'		Return data
		'	End Get
		'End Property

#End Region


#Region "Public Methods"

		Public Function LoadData() As Boolean
			Dim success As Boolean = True

			SearchExistingEmployeeData()

			Return success
		End Function

		Public ReadOnly Property GetAssigendEmployeeData As ExistingEmployeeSearchData
			Get
				Dim gvData = TryCast(grdExistingEmployees.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (gvData Is Nothing) Then
					Dim selectedRows = gvData.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim employee = CType(gvData.GetRow(selectedRows(0)), ExistingEmployeeSearchData)
						Return employee
					End If

				End If

				Return Nothing
			End Get

		End Property

#End Region


#Region "Private Methods"

		''' <summary>
		''' Resets the control.
		''' </summary>
		Private Sub Reset()

			ResetGridExistingEmployees()

		End Sub



		''' <summary>
		'''  Translate controls.
		''' </summary>
		Private Sub TranslateControls()


		End Sub

		''' <summary>
		''' Resets the existing employees grid.
		''' </summary>
		Private Sub ResetGridExistingEmployees()

			' Reset the grid

			gvExistingEmployees.OptionsView.ShowIndicator = False

			gvExistingEmployees.Columns.Clear()

			Dim columnEmployeeNumber As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeeNumber.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnEmployeeNumber.Name = "EmployeeNumber"
			columnEmployeeNumber.FieldName = "EmployeeNumber"
			columnEmployeeNumber.Visible = True
			gvExistingEmployees.Columns.Add(columnEmployeeNumber)

			Dim columnName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnName.Caption = m_Translate.GetSafeTranslationValue("Name")
			columnName.Name = "LastnameFirstname"
			columnName.FieldName = "LastnameFirstname"
			columnName.Visible = True
			gvExistingEmployees.Columns.Add(columnName)

			Dim columnAddress As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAddress.Caption = m_Translate.GetSafeTranslationValue("Adresse")
			columnAddress.Name = "Address"
			columnAddress.FieldName = "Address"
			columnAddress.Visible = True
			gvExistingEmployees.Columns.Add(columnAddress)

			Dim columnAdvisor As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAdvisor.Caption = m_Translate.GetSafeTranslationValue("Berater")
			columnAdvisor.Name = "employeeadvisor"
			columnAdvisor.FieldName = "employeeadvisor"
			columnAdvisor.Visible = True
			gvExistingEmployees.Columns.Add(columnAdvisor)

			Dim columnEmployeeCreated As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeeCreated.Caption = m_Translate.GetSafeTranslationValue("Erfasst")
			columnEmployeeCreated.Name = "EmployeeCreated"
			columnEmployeeCreated.FieldName = "EmployeeCreated"
			columnEmployeeCreated.Visible = True
			gvExistingEmployees.Columns.Add(columnEmployeeCreated)


			grdExistingEmployees.DataSource = Nothing

		End Sub

		Private Sub SearchExistingEmployeeData()

			Dim existingEmployeesGridData = (From employeeData In ExistingEmployeeData
																			 Select New ExistingEmployeeGridViewItem With
								 {.Lastname = employeeData.Lastname,
																				 .Firstname = employeeData.Firstname,
																				 .Street = employeeData.Street,
																				 .CountryCode = employeeData.CountryCode,
																				 .Postcode = employeeData.Postcode,
																				 .Location = employeeData.Location,
																				 .EmployeeNumber = employeeData.EmployeeNumber,
																				 .ShowAsApplicant = employeeData.ShowAsApplicant,
																				 .employeekst = employeeData.employeeKST,
																				 .employeeadvisor = employeeData.employeeAdvisor,
																				 .CreatedOn = employeeData.CreatedOn,
																				 .CreatedFrom = employeeData.CreatedFrom
																				 }).ToList()


			Dim listDataSource As BindingList(Of ExistingEmployeeGridViewItem) = New BindingList(Of ExistingEmployeeGridViewItem)

			For Each employeerGridData In existingEmployeesGridData

				listDataSource.Add(employeerGridData)

			Next

			grdExistingEmployees.DataSource = listDataSource

		End Sub

		Sub OngvMain_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)
			Dim mandantnumber = m_InitializationData.MDData.MDNr
			Dim userNumber = m_InitializationData.UserData.UserNr

			If (e.Clicks = 2) Then

				Dim column = e.Column
				Dim dataRow = gvExistingEmployees.GetRow(e.RowHandle)
				If Not dataRow Is Nothing Then
					Dim viewData = CType(dataRow, ExistingEmployeeGridViewItem)

					If viewData.EmployeeNumber > 0 Then

						Dim hub = MessageService.Instance.Hub
						If viewData.ShowAsApplicant.GetValueOrDefault(False) Then

							Dim openMng As New OpenApplicantMngRequest(Me, userNumber, mandantnumber, viewData.EmployeeNumber)
							hub.Publish(openMng)

						Else
							Dim openMng As New OpenEmployeeMngRequest(Me, userNumber, mandantnumber, viewData.EmployeeNumber)
							hub.Publish(openMng)

						End If

					End If

				End If
			End If

		End Sub

#End Region


#Region "View helper classes"

		''' <summary>
		''' View data for existing employees.
		''' </summary>
		Private Class ExistingEmployeeGridViewItem

			Public Property EmployeeNumber As Integer
			Public Property Lastname As String
			Public Property Firstname As String

			Public Property Street As String
			Public Property CountryCode As String
			Public Property Postcode As String
			Public Property Location As String
			Public Property ShowAsApplicant As Boolean?

			Public Property employeekst As String
			Public Property employeeadvisor As String
			Public Property CreatedFrom As String
			Public Property CreatedOn As DateTime?


			Public ReadOnly Property LastnameFirstname
				Get
					Return String.Format("{0} {1}", Lastname, Firstname)
				End Get
			End Property

			Public ReadOnly Property Address As String
				Get
					Return String.Format("{0} {1}-{2}, {3}", Street, CountryCode, Postcode, Location)
				End Get
			End Property

			Public ReadOnly Property EmployeeCreated
				Get
					Return String.Format("{0:G}, {1}", CreatedOn, CreatedFrom)
				End Get
			End Property

		End Class

#End Region

	End Class

End Namespace
