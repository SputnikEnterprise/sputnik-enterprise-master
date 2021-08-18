
Imports SP.DatabaseAccess.Employee

Imports System.ComponentModel
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraNavBar
Imports DevExpress.XtraEditors
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors.Repository

Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.TableSetting
Imports SP.DatabaseAccess.TableSetting.DataObjects

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
Imports DevExpress.XtraGrid.Columns
Imports SP.Internal.Automations

Public Class frmTables


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
	''' The common data access object.
	''' </summary>
	Private m_CommonDatabaseAccess As Common.ICommonDatabaseAccess


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

	Private m_SuppressUIEvents As Boolean

	Private connectionString As String
	Private m_EmployeecontactData As IEnumerable(Of EmployeeContactData)

	Private Property m_Selectedmodul As SelectedModulKey
	Private Property m_Reccount As Integer

	Private rdGroup As New PanelControl ' RadioGroup

#End Region


#Region "private consts"

	Private Enum SelectedModulKey

		MODUL_EMPLOYEE_CONTACT
		MODUL_EMPLOYEE_FSTATE
		MODUL_EMPLOYEE_SSTATE
		MODUL_EMPLOYEE_CIVILSTATE

		MODUL_EMPLOYEE_LANGUAGELETTER
		MODUL_EMPLOYEE_ASSESSMENT
		MODUL_EMPLOYEE_COMMUNICATIONTYPE

		MODUL_EMPLOYEE_CARRESERVE
		MODUL_EMPLOYEE_DRIVINGLICENCE
		MODUL_EMPLOYEE_VEHICLE

		MODUL_EMPLOYEE_CONTACTRES1
		MODUL_EMPLOYEE_CONTACTRES2
		MODUL_EMPLOYEE_CONTACTRES3
		MODUL_EMPLOYEE_CONTACTRES4

		MODUL_EMPLOYEE_DEADLINE
		MODUL_EMPLOYEE_WORKPENSUM
		MODUL_EMPLOYEE_EMPLOYEMENTTYPE
		MODUL_EMPLOYEE_DOCUMENTCATEGORY

		MODUL_EMPLOYEE_INTERVIEWSTATE

		' Customer
		MODUL_CUSTOMER_PROPERTY
		MODUL_CUSTOMER_CONTACT
		MODUL_CUSTOMER_FSTATE
		MODUL_CUSTOMER_SSTATE
		MODUL_CUSTOMER_STICHWORT
		MODUL_CUSTOMER_EMPLOYEMENTTYPE
		MODUL_CUSTOMER_CONTACTRES1
		MODUL_CUSTOMER_CONTACTRES2
		MODUL_CUSTOMER_CONTACTRES3
		MODUL_CUSTOMER_CONTACTRES4
		MODUL_CUSTOMER_PAYMENTREMINDERCODE
		MODUL_CUSTOMER_PAYMENTCONDITION
		MODUL_CUSTOMER_INVOICEOPTIONS
		MODUL_CUSTOMER_INVOICETYPE
		MODUL_CUSTOMER_INVOICESHIPMENT
		MODUL_CUSTOMER_NUMBEROFEMPLOYEES
		MODUL_CUSTOMER_DOCUMENTCATEGORY


		' responsible person
		MODUL_RESPONSIBLEPERSON_CONTACT
		MODUL_RESPONSIBLEPERSON_FSTATE
		MODUL_RESPONSIBLEPERSON_SSTATE
		MODUL_RESPONSIBLEPERSON_COMMUNICATION
		MODUL_RESPONSIBLEPERSON_COMMUNICATIONTYPE

		MODUL_RESPONSIBLEPERSON_DEPARTMENT
		MODUL_RESPONSIBLEPERSON_POSITION

		MODUL_RESPONSIBLEPERSON_CONTACTRES1
		MODUL_RESPONSIBLEPERSON_CONTACTRES2
		MODUL_RESPONSIBLEPERSON_CONTACTRES3
		MODUL_RESPONSIBLEPERSON_CONTACTRES4


		' Vacancy
		MODUL_VACANCY_CONTACT
		MODUL_VACANCY_STATE
		MODUL_VACANCY_GROUP

		' Offer
		MODUL_OFFER_CONTACT
		MODUL_OFFER_STATE
		MODUL_OFFER_GROUP

		' Propse
		MODUL_PROPOSE_STATE
		MODUL_PROPOSE_EMPLOYEMENTTYPE
		MODUL_PROPOSE_ART


		' common tables
		MODUL_CONTACT_CATEGORY
		MODUL_EMPLOYEMENT_CATEGORIZATION
		MODUL_DIVERSE_SALUTATION
		MODUL_DIVERSE_SMSTEMPLATE

		MODUL_DIVERSE_COUNTRY
		MODUL_DIVERSE_BRANCHES
		MODUL_DIVERSE_QUALIFICATION

		MODUL_DIVERSE_FCOSTCENTER
		MODUL_DIVERSE_SCOSTCENTER

		MODUL_DIVERSE_BVGGENTS
		MODUL_DIVERSE_BVGFEMALE
		MODUL_DIVERSE_FF13SALARY
		MODUL_DIVERSE_QST

		MODUL_DIVERSE_BUSINESSBRANCHS
		MODUL_DIVERSE_FIBU
		MODUL_DIVERSE_ABSENCE
		MODUL_DIVERSE_AGBWOS
		MODUL_MAIN_PRINTTEMPLATES
		MODUL_MAIN_EXPORTTEMPLATES



		MODUL_MAIN_EXTERNALMODUL

	End Enum


	Private Const MODUL_NAME_SETTING = "deletedreclist"

	Private Const USER_XML_SETTING_SPUTNIK_DELETED_SEARCH_LIST_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/deletedreclist/{1}/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_DELETED_SEARCH_LIST_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/deletedreclist/{1}/keepfilter"

#End Region


#Region "Constructor"

	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_SuppressUIEvents = True
		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_InitializationData = _setting

		m_Selectedmodul = SelectedModulKey.MODUL_EMPLOYEE_CONTACT   ' MODUL_EMPLOYEE_CONTACT

		m_mandant = New Mandant

		m_Utility = New Utility
		m_UtilityUI = New UtilityUI
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)


		connectionString = m_InitializationData.MDData.MDDbConn
		m_TablesettingDatabaseAccess = New SP.DatabaseAccess.TableSetting.TablesDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
		m_CommonDatabaseAccess = New Common.CommonDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)


		Me.KeyPreview = True
		Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		' Translate controls.
		TranslateControls()
		ResetMandantenDropDown()
		LoadMandantenDropDown()
		lueMandant.EditValue = m_InitializationData.MDData.MDNr

		m_SuppressUIEvents = False

		grpFilter.Visible = False

		' Creates the navigation bar.
		CreateMyNavBar()

		gvTableContent.OptionsBehavior.Editable = True

		AddHandler gvTableContent.RowCellClick, AddressOf Ongv_RowCellClick
		AddHandler gvTableContent.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground

		AddHandler Me.lueMandant.EditValueChanged, AddressOf OnlueMandant_EditValueChanged
		AddHandler Me.lueYear.EditValueChanged, AddressOf OnlueYear_EditValueChanged

	End Sub


#End Region


#Region "private readonly properties"

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

	Private ReadOnly Property SelectedEmployeeFStateViewData As SP.DatabaseAccess.TableSetting.DataObjects.EmployeeStateData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), SP.DatabaseAccess.TableSetting.DataObjects.EmployeeStateData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedEmployeeSStateViewData As SP.DatabaseAccess.TableSetting.DataObjects.EmployeeStateData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), SP.DatabaseAccess.TableSetting.DataObjects.EmployeeStateData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedEmployeeCivilstateViewData As Common.DataObjects.CivilStateData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), Common.DataObjects.CivilStateData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedEmployeeJobLanguageViewData As JobLanguageData
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

	Private ReadOnly Property SelectedEmployeeAssesmentViewData As AssessmentData
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

	Private ReadOnly Property SelectedEmployeeCommunicationTypeViewData As CommunicationTypeData
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

	Private ReadOnly Property SelectedEmployeeCarReserveViewData As CarReserveData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), CarReserveData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedEmployeeDrivingLicenceViewData As DrivingLicenceData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), DrivingLicenceData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedEmployeeVehicleViewData As VehicleData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), VehicleData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedEmployeeContactReserveViewData(ByVal data As ContactReserveType) As ContactReserveData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), ContactReserveData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedEmployeeDeadlineViewData As DeadlineData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), DeadlineData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedEmployeeWorkPensumViewData As WorkPensumData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), WorkPensumData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedEmployeeEmployementTypeViewData As EmployeeEmployementTypeData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), EmployeeEmployementTypeData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedEmployeeDocumentCategoryViewData As SP.DatabaseAccess.Employee.DataObjects.DocumentMng.EmployeeDocumentCategoryData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), SP.DatabaseAccess.Employee.DataObjects.DocumentMng.EmployeeDocumentCategoryData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedEmployeeInterviewStateViewData As EmployeeInteriviewStateData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), EmployeeInteriviewStateData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property



	''' <summary>
	''' Customer
	''' </summary>
	Private ReadOnly Property SelectedCustomerpropertyViewData As CustomerPropertyData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), CustomerPropertyData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedCustomerContactViewData As CustomerContactData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), CustomerContactData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedCustomerFStateViewData As SP.DatabaseAccess.TableSetting.DataObjects.CustomerStateData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), SP.DatabaseAccess.TableSetting.DataObjects.CustomerStateData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedCustomerSStateViewData As SP.DatabaseAccess.TableSetting.DataObjects.CustomerStateData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), SP.DatabaseAccess.TableSetting.DataObjects.CustomerStateData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedCustomerStichwortViewData As CustomerStichwortData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), CustomerStichwortData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedCustomerEmployementTypeViewData As CustomerEmployementTypeData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), CustomerEmployementTypeData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedCustomerPaymentReminderCodeViewData As Customer.DataObjects.PaymentReminderCodeData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), Customer.DataObjects.PaymentReminderCodeData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedCustomerPaymentConditionViewData As Customer.DataObjects.PaymentConditionData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), Customer.DataObjects.PaymentConditionData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedCustomerInvoiceOptionsViewData As Customer.DataObjects.InvoiceOptionData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), Customer.DataObjects.InvoiceOptionData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedCustomerInvoiceTypeViewData As Customer.DataObjects.InvoiceTypeData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), Customer.DataObjects.InvoiceTypeData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedCustomerInvoiceShipmentViewData() As Customer.DataObjects.OPShipmentData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), Customer.DataObjects.OPShipmentData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedCustomerNumberOfEmployeesViewData As Customer.DataObjects.NumberOfEmployeesData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), Customer.DataObjects.NumberOfEmployeesData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedCustomerContactReserveViewData(ByVal data As ContactReserveType) As Customer.DataObjects.CustomerReserveData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), Customer.DataObjects.CustomerReserveData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedCustomerDocumentCategoryViewData As SP.DatabaseAccess.Customer.DataObjects.CustomerDocumentCategoryData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), SP.DatabaseAccess.Customer.DataObjects.CustomerDocumentCategoryData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property



	''' <summary>
	''' responsible persons
	''' </summary>
	Private ReadOnly Property SelectedResponsiblePersonContactViewData As Customer.DataObjects.ResponsiblePersonContactInfo
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), Customer.DataObjects.ResponsiblePersonContactInfo)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedResponsiblePersonFStateViewData As Customer.DataObjects.ResponsiblePersonStateData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), Customer.DataObjects.ResponsiblePersonStateData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedResponsiblePersonSStateViewData As Customer.DataObjects.ResponsiblePersonStateData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), Customer.DataObjects.ResponsiblePersonStateData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedResponsiblePersonDepartmentViewData As Customer.DataObjects.DepartmentData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), Customer.DataObjects.DepartmentData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedResponsiblePersonPositionViewData As Customer.DataObjects.PositionData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), Customer.DataObjects.PositionData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedResponsiblePersonCommunicationViewData As Customer.DataObjects.CustomerCommunicationData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), Customer.DataObjects.CustomerCommunicationData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedResponsiblePersonCommunicationTypeViewData As Customer.DataObjects.CustomerCommunicationTypeData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), Customer.DataObjects.CustomerCommunicationTypeData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedCResponsibleContactReserveViewData(ByVal data As ContactReserveType) As Customer.DataObjects.ResponsiblePersonReserveData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), Customer.DataObjects.ResponsiblePersonReserveData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedCustomerPrintTemplatesViewData As PrintTemplatesData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), PrintTemplatesData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property



	''' <summary>
	''' Vacacny
	''' </summary>
	Private ReadOnly Property SelectedVacancyContactViewData As VacancyContactData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), VacancyContactData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedVacancyStateViewData As VacancyStateData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), VacancyStateData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedVacancyGroupViewData As VacancyGroupData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), VacancyGroupData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property



	''' <summary>
	''' Offer
	''' </summary>
	Private ReadOnly Property SelectedOfferContactViewData As OfferContactData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), OfferContactData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedOfferStateViewData As OfferStateData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), OfferStateData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedOfferGroupViewData As OfferGroupData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), OfferGroupData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property




	''' <summary>
	''' propose
	''' </summary>
	Private ReadOnly Property SelectedProposeStateViewData As ProposeStateData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), ProposeStateData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedProposeEmployementTypeViewData As ProposeEmployementTypeData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), ProposeEmployementTypeData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedProposeArtViewData As ProposeArtData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), ProposeArtData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property




	''' <summary>
	''' common tables
	''' </summary>
	Private ReadOnly Property SelectedContactCategoryViewData As SP.DatabaseAccess.Common.DataObjects.ContactType1Data
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), SP.DatabaseAccess.Common.DataObjects.ContactType1Data)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedEmployementCategorizedViewData As ES.DataObjects.ESMng.ESCategorizationData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), ES.DataObjects.ESMng.ESCategorizationData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedBusinessBranchesViewData As AvilableBusinessBranchData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), AvilableBusinessBranchData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedfCostcenterViewData As CostCenter1Data
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), CostCenter1Data)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedSCostcenterViewData As CostCenter2Data
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), CostCenter2Data)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedCurrencyViewData As CurrencyData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), CurrencyData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedBVGViewData As BVGData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), BVGData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedQSTInfoViewData As QstInfoData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), QstInfoData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedFF13LohnViewData As FF13LohnData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), FF13LohnData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedFibuKontenViewData As FIBUData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), FIBUData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedCountryViewData As CountryData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), CountryData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedJobViewData As JobData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), JobData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedSectorViewData As SectorData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), SectorData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedTerminAndConditionsViewData As TermsAndConditionsData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), TermsAndConditionsData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedAbsenceViewData As AbsenceData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), AbsenceData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property




	Private ReadOnly Property SelectedSalutationViewData As SalutationData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), SalutationData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedSMSTemplateViewData As SMSTemplateData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), SMSTemplateData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property


	Private ReadOnly Property SelectedPrintTemplatesViewData As PrintTemplatesData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), PrintTemplatesData)

					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedExportTemplatesViewData As ExportTemplatesData
		Get
			Dim grdView = TryCast(grdTableContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), ExportTemplatesData)

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

		Me.lblTableCaption.Text = m_Translate.GetSafeTranslationValue(Me.lblTableCaption.Text)
		Me.lblJahr.Text = m_Translate.GetSafeTranslationValue(Me.lblJahr.Text)

		Me.bsiLblRecCount.Caption = m_Translate.GetSafeTranslationValue(Me.bsiLblRecCount.Caption)

	End Sub

	' Mandantendaten...
	Private Sub OnlueMandant_EditValueChanged(sender As Object, e As System.EventArgs)
		If m_SuppressUIEvents Then Return

		Dim SelectedData As SP.DatabaseAccess.Common.DataObjects.MandantData = TryCast(Me.lueMandant.GetSelectedDataRow(), SP.DatabaseAccess.Common.DataObjects.MandantData)

		If Not SelectedData Is Nothing Then

			' not allowed, because of mandant changing back again
			'If m_InitializationData.MDData.MDNr <> SelectedData.MandantNumber Then

			ResetBVGDataGrid()
			LoadYearDropDownData()

			If lueYear.EditValue Is Nothing Then Return
			Select Case m_Selectedmodul
				Case SelectedModulKey.MODUL_DIVERSE_BVGFEMALE
					LoadBVGList("F")

				Case SelectedModulKey.MODUL_DIVERSE_BVGGENTS
					LoadBVGList("M")

			End Select

		End If

		'End If

	End Sub

	Private Sub OnlueYear_EditValueChanged(sender As Object, e As System.EventArgs)

		If m_SuppressUIEvents Then Return

		If lueYear.EditValue Is Nothing Then Return
		ResetBVGDataGrid()
		Select Case m_Selectedmodul
			Case SelectedModulKey.MODUL_DIVERSE_BVGFEMALE
				LoadBVGList("F")

			Case SelectedModulKey.MODUL_DIVERSE_BVGGENTS
				LoadBVGList("M")

		End Select

	End Sub

	''' <summary>
	''' Creates Navigationbar
	''' </summary>
	Private Sub CreateMyNavBar()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Me.navMain.Items.Clear()
		Try
			navMain.PaintStyleName = "SkinExplorerBarView"

			' Create a Local group.
			Dim groupEmployee As NavBarGroup = New NavBarGroup(("Kandidatentabellen"))
			groupEmployee.Name = "gNavEmployee"

			Dim groupCustomer As NavBarGroup = New NavBarGroup(("Kundentabellen"))
			groupCustomer.Name = "gNavCustomer"

			Dim groupVacancy As NavBarGroup = New NavBarGroup(("Vakanzentabellen"))
			groupVacancy.Name = "gNavVacancy"

			Dim groupOffer As NavBarGroup = New NavBarGroup(("Offerttabellen"))
			groupOffer.Name = "gNavOffer"

			Dim groupPropose As NavBarGroup = New NavBarGroup(("Vorschlagstabellen"))
			groupPropose.Name = "gNavPropose"

			Dim groupES As NavBarGroup = New NavBarGroup(("Einsatztabellen"))
			groupES.Name = "gNavES"

			Dim groupDiverses As NavBarGroup = New NavBarGroup(("Sonstige Tabellen"))
			groupDiverses.Name = "gNavDiverses"

			Dim groupTransferFromDatabases As NavBarGroup = New NavBarGroup(("Stammdaten-Import"))
			groupTransferFromDatabases.Name = "gNavTransferFromDatabases"


			Dim nbiEmployee_Kontakte As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Kontakte"))
			nbiEmployee_Kontakte.Name = "Show_Employee_Kontakte"

			Dim nbiEmployee_FStatus As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("1. Status"))
			nbiEmployee_FStatus.Name = "Show_Employee_FStatus"
			Dim nbiEmployee_SStatus As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("2. Status"))
			nbiEmployee_SStatus.Name = "Show_Employee_SStatus"
			Dim nbiEmployee_Civilstate As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Zivilstand"))
			nbiEmployee_Civilstate.Name = "Show_Employee_Civilstate"

			Dim nbiEmployee_Sprachfaehigkeit As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Sprachfähigkeit"))
			nbiEmployee_Sprachfaehigkeit.Name = "Show_Employee_Sprachfaehigkeit"

			Dim nbiEmployee_Hauptsprache As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Hauptsprache"))
			nbiEmployee_Hauptsprache.Name = "Show_Employee_Hauptsprache"

			Dim nbiEmployee_Beurteilung As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Beurteilung"))
			nbiEmployee_Beurteilung.Name = "Show_Employee_Beurteilung"

			Dim nbiEmployee_Kommunikationsart As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Kommunikationsart"))
			nbiEmployee_Kommunikationsart.Name = "Show_Employee_Kommunikationsart"

			Dim nbiEmployee_Fahrzeugsreserve As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Fahrzeugsreserve"))
			nbiEmployee_Fahrzeugsreserve.Name = "Show_Employee_Fahrzeugsreserve"

			Dim nbiEmployee_DrivingLicence As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Führerscheine"))
			nbiEmployee_DrivingLicence.Name = "Show_Employee_Fuehrerschein"

			Dim nbiEmployee_Vehicle As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Fahrzeuge"))
			nbiEmployee_Vehicle.Name = "Show_Employee_Fahrzeuge"

			Dim nbiEmployee_Res_1 As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("1. Reserve"))
			nbiEmployee_Res_1.Name = "Show_Employee_Res_1"
			Dim nbiEmployee_Res_2 As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("2. Reserve"))
			nbiEmployee_Res_2.Name = "Show_Employee_Res_2"
			Dim nbiEmployee_Res_3 As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("3. Reserve"))
			nbiEmployee_Res_3.Name = "Show_Employee_Res_3"
			Dim nbiEmployee_Res_4 As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("4. Reserve"))
			nbiEmployee_Res_4.Name = "Show_Employee_Res_4"

			Dim nbiEmployee_Kuendigungsfristen As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Kündigungsfristen"))
			nbiEmployee_Kuendigungsfristen.Name = "Show_Employee_Kuendigungsfristen"
			Dim nbiEmployee_Arbeitspensum As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Arbeitspensum"))
			nbiEmployee_Arbeitspensum.Name = "Show_Employee_Arbeitspensum"

			Dim nbiEmployee_Anstellungsart As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Anstellungsart"))
			nbiEmployee_Anstellungsart.Name = "Show_Employee_Anstellungsart"
			Dim nbiEmployee_Document_Category As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Dokumenten-Kategorie"))
			nbiEmployee_Document_Category.Name = "Show_Employee_Document_Category"

			Dim nbiEmployee_StatusTermin As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Status (Vorstellungstermin)"))
			nbiEmployee_StatusTermin.Name = "Show_Employee_StatusTermin"



			' Kundentabelle
			Dim nbiCustomer_Property As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Eigenschaften"))
			nbiCustomer_Property.Name = "Show_Customer_Property"
			Dim nbiCustomer_Kontakte As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Kontakte"))
			nbiCustomer_Kontakte.Name = "Show_Customer_Kontakte"
			Dim nbiCustomer_FStatus As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("1. Status"))
			nbiCustomer_FStatus.Name = "Show_Customer_FStatus"
			Dim nbiCustomer_SStatus As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("2. Status"))
			nbiCustomer_SStatus.Name = "Show_Customer_SStatus"
			Dim nbiCustomer_Stichwort As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Stichtwort"))
			nbiCustomer_Stichwort.Name = "Show_Customer_Stichwort"
			Dim nbiCustomer_Anstellung As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Anstellungsarten"))
			nbiCustomer_Anstellung.Name = "Show_Customer_Anstellung"

			Dim nbiCustomer_PaymentReminder As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Mahncode"))
			nbiCustomer_PaymentReminder.Name = "Show_Customer_Paymentreminder"
			Dim nbiCustomer_PaymentCondition As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Zahlungskonditionen"))
			nbiCustomer_PaymentCondition.Name = "Show_Customer_PaymentCondition"
			Dim nbiCustomer_InvoiceOptions As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Faktura-Optionen"))
			nbiCustomer_InvoiceOptions.Name = "Show_Customer_InvoiceOptions"
			Dim nbiCustomer_InvoiceType As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Fakturaarten"))
			nbiCustomer_InvoiceType.Name = "Show_Customer_InvoiceType"
			Dim nbiCustomer_Invoiceshipment As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Rechnung senden mit"))
			nbiCustomer_Invoiceshipment.Name = "Show_Customer_Invoiceshipment"

			Dim nbiCustomer_NumberOfEmployees As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Betriebsgrösse"))
			nbiCustomer_NumberOfEmployees.Name = "Show_Customer_NumberOfEmployees"

			Dim nbiCustomer_Res_1 As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("1. Reserve"))
			nbiCustomer_Res_1.Name = "Show_Customer_Res_1"
			Dim nbiCustomer_Res_2 As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("2. Reserve"))
			nbiCustomer_Res_2.Name = "Show_Customer_Res_2"
			Dim nbiCustomer_Res_3 As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("3. Reserve"))
			nbiCustomer_Res_3.Name = "Show_Customer_Res_3"
			Dim nbiCustomer_Res_4 As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("4. Reserve"))
			nbiCustomer_Res_4.Name = "Show_Customer_Res_4"
			Dim nbiCustomer_Document_Category As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Dokumenten-Kategorie"))
			nbiCustomer_Document_Category.Name = "Show_Customer_Document_Category"


			' Zuständige Personen
			Dim nbiCResponsible_Kontakte As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("ZHD-Kontakte"))
			nbiCResponsible_Kontakte.Name = "Show_CResponsible_Kontakte"
			Dim nbiCResponsible_FStatus As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("ZHD-1. Status"))
			nbiCResponsible_FStatus.Name = "Show_CResponsible_FStatus"
			Dim nbiCResponsible_SStatus As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("ZHD-2. Status"))
			nbiCResponsible_SStatus.Name = "Show_CResponsible_SStatus"
			Dim nbiCResponsible_Abteilung As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("ZHD-Abteilung"))
			nbiCResponsible_Abteilung.Name = "Show_CResponsible_Department"
			Dim nbiCResponsible_Position As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("ZHD-Position"))
			nbiCResponsible_Position.Name = "Show_CResponsible_Position"

			Dim nbiCResponsible_Kommunikation As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("ZHD-Kommunikation"))
			nbiCResponsible_Kommunikation.Name = "Show_CResponsible_Communication"
			Dim nbiCResponsible_Versand As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("ZHD-Versand"))
			nbiCResponsible_Versand.Name = "Show_CResponsible_CommunicationType"

			Dim nbiCResponsible_Res_1 As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("ZHD-1. Reserve"))
			nbiCResponsible_Res_1.Name = "Show_CResponsible_Res_1"
			Dim nbiCResponsible_Res_2 As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("ZHD-2. Reserve"))
			nbiCResponsible_Res_2.Name = "Show_CResponsible_Res_2"
			Dim nbiCResponsible_Res_3 As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("ZHD-3. Reserve"))
			nbiCResponsible_Res_3.Name = "Show_CResponsible_Res_3"
			Dim nbiCResponsible_Res_4 As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("ZHD-4. Reserve"))
			nbiCResponsible_Res_4.Name = "Show_CResponsible_Res_4"







			Dim nbiVacancy As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Vakanzen"))
			nbiVacancy.Name = "Show_Vacancytables"

			Dim nbiVacancy_Contact As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Kontakt"))
			nbiVacancy_Contact.Name = "Show_Vacancy_Contact"

			Dim nbiVacancy_State As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Status"))
			nbiVacancy_State.Name = "Show_Vacancy_Status"

			Dim nbiVacancy_Group As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Gruppe"))
			nbiVacancy_Group.Name = "Show_Vacancy_Group"



			Dim nbiOffers As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Offerte"))
			nbiOffers.Name = "Show_Offerstables"

			Dim nbiOffer_Contact As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Kontakt"))
			nbiOffer_Contact.Name = "Show_Offer_Contact"

			Dim nbiOffer_State As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Status"))
			nbiOffer_State.Name = "Show_Offer_State"

			Dim nbiOffer_Group As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Gruppe"))
			nbiOffer_Group.Name = "Show_Offer_Group"


			Dim nbiPropse As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Vorschläge"))
			nbiPropse.Name = "Show_Proposetables"

			Dim nbiPropose_State As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Status"))
			nbiPropose_State.Name = "Show_Propose_State"

			Dim nbiPropose_Anstellung As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Anstellung"))
			nbiPropose_Anstellung.Name = "Show_Propose_Anstellung"

			Dim nbiPropose_Art As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Art"))
			nbiPropose_Art.Name = "Show_Propose_Art"




			' common tables
			Dim nbi_Contact_Category As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Kontakt-Kategorie"))
			nbi_Contact_Category.Name = "Show_Contact_Category"

			Dim nbi_Anrede As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Anrede"))
			nbi_Anrede.Name = "Show_Anrede"
			Dim nbiSMSTemplates As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("SMS-Vorlagen"))
			nbiSMSTemplates.Name = "Show_SMSTemplates"

			Dim nbiEmployementCategorized As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Einsatz-Einstufung"))
			nbiEmployementCategorized.Name = "Show_EmployementCategorized"

			Dim nbiBVGGents As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("BVG-Mann"))
			nbiBVGGents.Name = "Show_BVGGents"
			Dim nbiBVGFamele As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("BVG-Frau"))
			nbiBVGFamele.Name = "Show_BVGFemale"

			Dim nbiQSTInfo As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Quellensteuer-Automatisation"))
			nbiQSTInfo.Name = "Show_QSTInfo"
			Dim nbiff13Lohn As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Ferien, Feiertag und 13. Monatslohn"))
			nbiff13Lohn.Name = "Show_FF13Lohn"

			Dim nbiFCostcenter As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("1. Kostenstelle"))
			nbiFCostcenter.Name = "Show_fCostcenter"
			Dim nbiSCostcenter As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("2. Kostenstelle"))
			nbiSCostcenter.Name = "Show_SCostcenter"

			Dim nbiBranches As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Branchen"))
			nbiBranches.Name = "Show_Branches"
			Dim nbiQualification As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Qualifikation"))
			nbiQualification.Name = "Show_Qualification"
			Dim nbiCountry As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Länder"))
			nbiCountry.Name = "Show_Country"
			Dim nbiFiliale As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Filiale"))
			nbiFiliale.Name = "Show_Filiale"
			Dim nbiFibu As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("FIBU-Konten"))
			nbiFibu.Name = "Show_Fibu"

			Dim nbiAGBWOS As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("AGB für WOS"))
			nbiAGBWOS.Name = "Show_AGB_WOS"
			Dim nbiAbsence As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Fehlcode"))
			nbiAbsence.Name = "Show_Absence"

			Dim nbi_Printtemplates As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Druck-Vorlagen"))
			nbi_Printtemplates.Name = "Show_Printtemplates"

			Dim nbi_Exporttemplates As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Export-Menü"))
			nbi_Exporttemplates.Name = "Show_Exporttemplates"


			' transfer another databases
			Dim nbi_transferfromanotherDatabases As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Stammdaten-Import"))
			nbi_transferfromanotherDatabases.Name = "nbi_transferfromanotherDatabases"
			Dim nbi_ExportEndData As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Export END-DATA"))
			nbi_ExportEndData.Name = "nbi_ExportEndData"


			Try
				navMain.BeginUpdate()

				navMain.Groups.Add(groupEmployee)

				groupEmployee.ItemLinks.Add(nbiEmployee_Kontakte)
				groupEmployee.ItemLinks.Add(nbiEmployee_FStatus)
				groupEmployee.ItemLinks.Add(nbiEmployee_SStatus)
				groupEmployee.ItemLinks.Add(nbiEmployee_Civilstate)

				groupEmployee.ItemLinks.Add(nbiEmployee_Sprachfaehigkeit)
				groupEmployee.ItemLinks.Add(nbiEmployee_Kommunikationsart)
				groupEmployee.ItemLinks.Add(nbiEmployee_Anstellungsart)
				groupEmployee.ItemLinks.Add(nbiEmployee_Arbeitspensum)
				groupEmployee.ItemLinks.Add(nbiEmployee_Beurteilung)

				groupEmployee.ItemLinks.Add(nbiEmployee_Fahrzeugsreserve)
				groupEmployee.ItemLinks.Add(nbiEmployee_DrivingLicence)
				groupEmployee.ItemLinks.Add(nbiEmployee_Vehicle)

				groupEmployee.ItemLinks.Add(nbiEmployee_Kuendigungsfristen)

				groupEmployee.ItemLinks.Add(nbiEmployee_Res_1)
				groupEmployee.ItemLinks.Add(nbiEmployee_Res_2)
				groupEmployee.ItemLinks.Add(nbiEmployee_Res_3)
				groupEmployee.ItemLinks.Add(nbiEmployee_Res_4)
				groupEmployee.ItemLinks.Add(nbiEmployee_Document_Category)

				groupEmployee.ItemLinks.Add(nbiEmployee_StatusTermin)



				navMain.Groups.Add(groupCustomer)
				groupCustomer.ItemLinks.Add(nbiCustomer_Property)
				groupCustomer.ItemLinks.Add(nbiCustomer_Kontakte)
				groupCustomer.ItemLinks.Add(nbiCustomer_FStatus)
				groupCustomer.ItemLinks.Add(nbiCustomer_SStatus)

				groupCustomer.ItemLinks.Add(nbiCustomer_Stichwort)
				groupCustomer.ItemLinks.Add(nbiCustomer_Anstellung)

				groupCustomer.ItemLinks.Add(nbiCustomer_Res_1)
				groupCustomer.ItemLinks.Add(nbiCustomer_Res_2)
				groupCustomer.ItemLinks.Add(nbiCustomer_Res_3)
				groupCustomer.ItemLinks.Add(nbiCustomer_Res_4)

				groupCustomer.ItemLinks.Add(nbiCustomer_PaymentReminder)
				groupCustomer.ItemLinks.Add(nbiCustomer_PaymentCondition)
				groupCustomer.ItemLinks.Add(nbiCustomer_InvoiceOptions)
				groupCustomer.ItemLinks.Add(nbiCustomer_InvoiceType)
				groupCustomer.ItemLinks.Add(nbiCustomer_Invoiceshipment)
				groupCustomer.ItemLinks.Add(nbiCustomer_NumberOfEmployees)
				groupCustomer.ItemLinks.Add(nbiCustomer_Document_Category)


				groupCustomer.ItemLinks.Add(nbiCResponsible_Kontakte)
				groupCustomer.ItemLinks.Add(nbiCResponsible_FStatus)
				groupCustomer.ItemLinks.Add(nbiCResponsible_SStatus)
				groupCustomer.ItemLinks.Add(nbiCResponsible_Abteilung)
				groupCustomer.ItemLinks.Add(nbiCResponsible_Position)
				groupCustomer.ItemLinks.Add(nbiCResponsible_Kommunikation)
				groupCustomer.ItemLinks.Add(nbiCResponsible_Versand)

				groupCustomer.ItemLinks.Add(nbiCResponsible_Res_1)
				groupCustomer.ItemLinks.Add(nbiCResponsible_Res_2)
				groupCustomer.ItemLinks.Add(nbiCResponsible_Res_3)
				groupCustomer.ItemLinks.Add(nbiCResponsible_Res_4)


				navMain.Groups.Add(groupVacancy)

				groupVacancy.ItemLinks.Add(nbiVacancy_Contact)
				groupVacancy.ItemLinks.Add(nbiVacancy_State)
				groupVacancy.ItemLinks.Add(nbiVacancy_Group)


				navMain.Groups.Add(groupOffer)
				groupOffer.ItemLinks.Add(nbiOffer_Contact)
				groupOffer.ItemLinks.Add(nbiOffer_State)
				groupOffer.ItemLinks.Add(nbiOffer_Group)

				navMain.Groups.Add(groupPropose)
				groupPropose.ItemLinks.Add(nbiPropose_State)
				groupPropose.ItemLinks.Add(nbiPropose_Anstellung)
				groupPropose.ItemLinks.Add(nbiPropose_Art)


				' common
				navMain.Groups.Add(groupDiverses)
				groupDiverses.ItemLinks.Add(nbi_Contact_Category)
				groupDiverses.ItemLinks.Add(nbiEmployementCategorized)
				groupDiverses.ItemLinks.Add(nbiBVGGents)
				groupDiverses.ItemLinks.Add(nbiBVGFamele)

				groupDiverses.ItemLinks.Add(nbiQSTInfo)
				groupDiverses.ItemLinks.Add(nbiff13Lohn)

				groupDiverses.ItemLinks.Add(nbiFCostcenter)
				groupDiverses.ItemLinks.Add(nbiSCostcenter)

				groupDiverses.ItemLinks.Add(nbiBranches)
				groupDiverses.ItemLinks.Add(nbiQualification)
				groupDiverses.ItemLinks.Add(nbiCountry)
				groupDiverses.ItemLinks.Add(nbiFiliale)
				groupDiverses.ItemLinks.Add(nbiFibu)

				groupDiverses.ItemLinks.Add(nbiAGBWOS)
				groupDiverses.ItemLinks.Add(nbiAbsence)

				groupDiverses.ItemLinks.Add(nbi_Anrede)
				groupDiverses.ItemLinks.Add(nbiSMSTemplates)
				groupDiverses.ItemLinks.Add(nbi_Printtemplates)
				groupDiverses.ItemLinks.Add(nbi_Exporttemplates)

				groupEmployee.Expanded = False


				navMain.EndUpdate()


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Navbar Controls binden. {1}", strMethodeName, ex.Message))
				m_UtilityUI.ShowErrorDialog(String.Format("Fehler (navBarMain): {0}", ex.Message))

			End Try


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			m_UtilityUI.ShowOKDialog(Me, String.Format("Fehler (navBarMain): {0}", ex.Message), "Menüleiste", MessageBoxIcon.Error)

		End Try

	End Sub

	Private Function IsCustomerServiceAllowed(ByVal serviceName As String) As Boolean
		Dim providerObj As New ProviderData(m_InitializationData)
		Dim result = providerObj.IsCustomerAllowedToUseServiceData(m_InitializationData.MDData.MDGuid, serviceName)

		Return result

	End Function


	''' <summary>
	''' Clickevent for Navbar.
	''' </summary>
	Private Sub OnnbMain_LinkClicked(ByVal sender As Object, ByVal e As DevExpress.XtraNavBar.NavBarLinkEventArgs) Handles navMain.LinkClicked

		For i As Integer = 0 To Me.navMain.Groups(0).NavBar.Items.Count - 1
			e.Link.NavBar.Items(i).Appearance.ForeColor = Color.Black
		Next
		e.Link.Item.Appearance.ForeColor = Color.Orange

		ShowSelectedData(e.Link.ItemName)

	End Sub

	Private Sub ShowSelectedData(ByVal itemName As String)

		Try
			grpFilter.Visible = False
			m_SuppressUIEvents = True

			Select Case itemName.ToLower
				Case "Show_Employee_Kontakte".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_EMPLOYEE_CONTACT
					ResetEmployeeContactGrid()
					LoadEmployeeContactList()

				Case "Show_Employee_FStatus".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_EMPLOYEE_FSTATE
					ResetEmployeeFStateGrid()
					LoadEmployeeFStateList()

				Case "Show_Employee_SStatus".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_EMPLOYEE_SSTATE
					ResetEmployeeSStateGrid()
					LoadEmployeeSStateList()

				Case "Show_Employee_Civilstate".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_EMPLOYEE_CIVILSTATE
					ResetEmployeeCivilstateGrid()
					LoadEmployeeCivilstateList()

				Case "Show_Employee_Sprachfaehigkeit".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_EMPLOYEE_LANGUAGELETTER
					ResetEmployeeLanguageLetterGrid()
					LoadEmployeeLanguageLetterList()

				Case "Show_Employee_Beurteilung".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_EMPLOYEE_ASSESSMENT
					ResetEmployeeAssessmentGrid()
					LoadEmployeeAssessmentList()

				Case "Show_Employee_Kommunikationsart".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_EMPLOYEE_COMMUNICATIONTYPE
					ResetEmployeeCommunicationTypeGrid()
					LoadEmployeeCommunicationTypeList()

				Case "Show_Employee_Fahrzeugsreserve".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_EMPLOYEE_CARRESERVE
					ResetEmployeeCarReserveGrid()
					LoadEmployeeCarReserveList()

				Case "Show_Employee_Fuehrerschein".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_EMPLOYEE_DRIVINGLICENCE
					ResetEmployeeDrivinglicenceGrid()
					LoadEmployeeDrivingLicenceList()

				Case "Show_Employee_Fahrzeuge".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_EMPLOYEE_VEHICLE
					ResetEmployeeVehicleGrid()
					LoadEmployeeVehicleList()

				Case "Show_Employee_Res_1".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_EMPLOYEE_CONTACTRES1
					ResetEmployeeContactReserveGrid()
					LoadEmployeeContactReserveList(ContactReserveType.Reserve1)

				Case "Show_Employee_Res_2".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_EMPLOYEE_CONTACTRES2
					ResetEmployeeContactReserveGrid()
					LoadEmployeeContactReserveList(ContactReserveType.Reserve2)

				Case "Show_Employee_Res_3".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_EMPLOYEE_CONTACTRES3
					ResetEmployeeContactReserveGrid()
					LoadEmployeeContactReserveList(ContactReserveType.Reserve3)

				Case "Show_Employee_Res_4".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_EMPLOYEE_CONTACTRES4
					ResetEmployeeContactReserveGrid()
					LoadEmployeeContactReserveList(ContactReserveType.Reserve4)

				Case "Show_Employee_Kuendigungsfristen".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_EMPLOYEE_DEADLINE
					ResetEmployeeDeadLineGrid()
					LoadEmployeeDeadlineList()

				Case "Show_Employee_Arbeitspensum".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_EMPLOYEE_WORKPENSUM
					ResetEmployeeWorkPensumGrid()
					LoadEmployeeWorkPensumList()

				Case "Show_Employee_Anstellungsart".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_EMPLOYEE_EMPLOYEMENTTYPE
					ResetEmployeeEmployementTypeGrid()
					LoadEmployeeEmployementTypeList()

				Case "Show_Employee_Document_Category".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_EMPLOYEE_DOCUMENTCATEGORY
					ResetEmployeeDocumentCategoryGrid()
					LoadEmployeeDocumentCategoryList()

				Case "Show_Employee_StatusTermin".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_EMPLOYEE_INTERVIEWSTATE
					ResetEmployeeInterviewStateGrid()
					LoadEmployeeInterviewStateList()




				Case "Show_Customer_Property".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_CUSTOMER_PROPERTY
					ResetCustomerPropertyGrid()
					LoadCustomerPropertyList()

				Case "Show_Customer_Kontakte".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_CUSTOMER_CONTACT
					ResetCustomerContactGrid()
					LoadCustomerContactList()

				Case "Show_Customer_FStatus".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_CUSTOMER_FSTATE
					ResetCustomerFStateGrid()
					LoadCustomerFStateList()

				Case "Show_Customer_SStatus".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_CUSTOMER_SSTATE
					ResetCustomerSStateGrid()
					LoadCustomerSStateList()

				Case "Show_Customer_Stichwort".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_CUSTOMER_STICHWORT
					ResetCustomerStichwortGrid()
					LoadCustomerStichwortList()

				Case "Show_Customer_Anstellung".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_CUSTOMER_EMPLOYEMENTTYPE
					ResetCustomerEmployementTypeGrid()
					LoadCustomerEmployementTypeList()



				Case "Show_Customer_Res_1".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_CUSTOMER_CONTACTRES1
					ResetCustomerContactReserveGrid()
					LoadCustomerContactReserveList(ContactReserveType.Reserve1)

				Case "Show_Customer_Res_2".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_CUSTOMER_CONTACTRES2
					ResetCustomerContactReserveGrid()
					LoadCustomerContactReserveList(ContactReserveType.Reserve2)

				Case "Show_Customer_Res_3".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_CUSTOMER_CONTACTRES3
					ResetCustomerContactReserveGrid()
					LoadCustomerContactReserveList(ContactReserveType.Reserve3)

				Case "Show_Customer_Res_4".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_CUSTOMER_CONTACTRES4
					ResetCustomerContactReserveGrid()
					LoadCustomerContactReserveList(ContactReserveType.Reserve4)



				Case "Show_Customer_PaymentReminder".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_CUSTOMER_PAYMENTREMINDERCODE
					ResetCustomerPaymentReminderCodeGrid()
					LoadCustomerPaymentReminderCodeList()


				Case "Show_Customer_PaymentCondition".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_CUSTOMER_PAYMENTCONDITION
					ResetCustomerPaymentConditionGrid()
					LoadCustomerPaymentConditionList()

				Case "Show_Customer_InvoiceOptions".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_CUSTOMER_INVOICEOPTIONS
					ResetCustomerInvoiceOptionsGrid()
					LoadCustomerInvoiceOptionsList()

				Case "Show_Customer_InvoiceType".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_CUSTOMER_INVOICETYPE
					ResetCustomerInvoiceTypeGrid()
					LoadCustomerInvoiceTypeList()

				Case "Show_Customer_InvoiceShipment".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_CUSTOMER_INVOICESHIPMENT
					ResetCustomerInvoiceShipmentGrid()
					LoadCustomerInvoiceShipmentList()

				Case "Show_Customer_NumberOfEmployees".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_CUSTOMER_NUMBEROFEMPLOYEES
					ResetCustomerNumberOfEmployeesGrid()
					LoadCustomerNumberOfEmployeesList()

				Case "Show_Customer_Document_Category".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_CUSTOMER_DOCUMENTCATEGORY
					ResetCustomerDocumentCategoryGrid()
					LoadCustomerDocumentCategoryList()




					' Zuständige Personen
				Case "Show_CResponsible_Kontakte".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_RESPONSIBLEPERSON_CONTACT
					ResetResponsiblepersonContactGrid()
					LoadCResponsibleContactList()

				Case "Show_CResponsible_FStatus".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_RESPONSIBLEPERSON_FSTATE
					ResetResponsiblepersonFStateGrid()
					LoadCResponsibleFStateList()

				Case "Show_CResponsible_SStatus".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_RESPONSIBLEPERSON_SSTATE
					ResetResponsiblepersonSStateGrid()
					LoadCResponsibleSStateList()

				Case "Show_CResponsible_Department".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_RESPONSIBLEPERSON_DEPARTMENT
					ResetResponsiblepersonDepartmentGrid()
					LoadCResponsibleDepartmentList()

				Case "Show_CResponsible_Position".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_RESPONSIBLEPERSON_POSITION
					ResetResponsiblepersonPositionGrid()
					LoadCResponsiblePositionList()

				Case "Show_CResponsible_Communication".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_RESPONSIBLEPERSON_COMMUNICATION
					ResetResponsiblepersonCommunicationGrid()
					LoadCResponsibleCommunicationList()

				Case "Show_CResponsible_CommunicationType".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_RESPONSIBLEPERSON_COMMUNICATIONTYPE
					ResetResponsiblepersonCommunicationTypeGrid()
					LoadCResponsibleCommunicationTypeList()




				Case "Show_CResponsible_Res_1".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_RESPONSIBLEPERSON_CONTACTRES1
					ResetResponsiblepersonContactReserveGrid()
					LoadCResponsibleContactReserveList(ContactReserveType.Reserve1)

				Case "Show_CResponsible_Res_2".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_RESPONSIBLEPERSON_CONTACTRES2
					ResetResponsiblepersonContactReserveGrid()
					LoadCResponsibleContactReserveList(ContactReserveType.Reserve2)

				Case "Show_CResponsible_Res_3".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_RESPONSIBLEPERSON_CONTACTRES3
					ResetResponsiblepersonContactReserveGrid()
					LoadCResponsibleContactReserveList(ContactReserveType.Reserve3)

				Case "Show_CResponsible_Res_4".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_RESPONSIBLEPERSON_CONTACTRES4
					ResetResponsiblepersonContactReserveGrid()
					LoadCResponsibleContactReserveList(ContactReserveType.Reserve4)




					' Vacancy tables
				Case "Show_Vacancy_Contact".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_VACANCY_CONTACT
					ResetVacancyContactGrid()
					LoadVacancyContactList()

				Case "Show_Vacancy_Status".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_VACANCY_STATE
					ResetVacancyStateGrid()
					LoadVacancyStateList()

				Case "Show_Vacancy_Group".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_VACANCY_GROUP
					ResetVacancyGroupGrid()
					LoadVacancyGroupList()



					' Offer tables
				Case "Show_Offer_Contact".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_OFFER_CONTACT
					ResetOfferContactGrid()
					LoadOfferContactList()

				Case "Show_Offer_State".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_OFFER_STATE
					ResetOfferStateGrid()
					LoadOfferStateList()

				Case "Show_Offer_Group".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_OFFER_GROUP
					ResetOfferGroupGrid()
					LoadOfferGroupList()


					' Propose tables
				Case "Show_Propose_state".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_PROPOSE_STATE
					ResetProposeStateGrid()
					LoadProposeStateList()

				Case "Show_Propose_Anstellung".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_PROPOSE_EMPLOYEMENTTYPE
					ResetProposeEmployementTypeGrid()
					LoadProposeEmployementTypeList()

				Case "Show_Propose_Art".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_PROPOSE_ART
					ResetProposeArtGrid()
					LoadProposeArtList()






					' common tables
				Case "Show_Contact_Category".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_CONTACT_CATEGORY
					ResetContactCategoryGrid()
					LoadContactCategoryList()

				Case "Show_EmployementCategorized".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_EMPLOYEMENT_CATEGORIZATION
					ResetEmployementCategorizedGrid()
					LoadEmployementCategorizedList()

				Case "Show_fCostcenter".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_DIVERSE_FCOSTCENTER
					ResetFCostcenterGrid()
					LoadFCostcenterList()

				Case "Show_SCostcenter".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_DIVERSE_SCOSTCENTER
					ResetSCostcenterGrid()
					LoadSCostcenterList()

				Case "Show_BVGGents".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_DIVERSE_BVGGENTS
					grpFilter.Visible = True
					ResetBVGDataGrid()
					ResetYearDataDropDown()
					LoadBVGList("M")

				Case "Show_BVGFemale".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_DIVERSE_BVGFEMALE
					grpFilter.Visible = True
					ResetBVGDataGrid()
					ResetYearDataDropDown()
					LoadBVGList("F")

				Case "Show_FF13Lohn".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_DIVERSE_FF13SALARY
					ResetFF13LohnDataGrid()
					LoadFF13LohnList()

				Case "Show_QSTInfo".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_DIVERSE_QST
					ResetQSTDataGrid()
					LoadQSTInfoList()

				Case "Show_Branches".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_DIVERSE_BRANCHES
					ResetSectorGrid()
					LoadSectorList()

				Case "Show_Qualification".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_DIVERSE_QUALIFICATION
					ResetJobGrid()
					LoadJobList()

				Case "Show_Country".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_DIVERSE_COUNTRY
					ResetCountryGrid()
					LoadCountryList()

				Case "Show_Filiale".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_DIVERSE_BUSINESSBRANCHS
					ResetFilialeGrid()
					LoadBusinessBranchsList()

				Case "Show_Fibu".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_DIVERSE_FIBU
					ResetFibuKontenGrid()
					LoadFibukontenList()

				Case "Show_Absence".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_DIVERSE_ABSENCE
					ResetAbsenceGrid()
					LoadAbsenceList()

				Case "Show_AGB_WOS".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_DIVERSE_AGBWOS
					ResetTermsAndConditionsGrid()
					LoadTermsandConditionsList()


				Case "Show_Anrede".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_DIVERSE_SALUTATION
					ResetSalutationGrid()
					LoadSalutationList()

				Case "Show_SMSTemplates".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_DIVERSE_SMSTEMPLATE
					ResetSMSTemplateGrid()
					LoadSMSTemplatesList()

				Case "Show_Printtemplates".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_MAIN_PRINTTEMPLATES
					ResetPrintTemplatesGrid()
					LoadPrintTemplatesList()

				Case "Show_Exporttemplates".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_MAIN_EXPORTTEMPLATES
					ResetExportTemplatesGrid()
					LoadExportTemplatesList()


				Case Else
					grdTableContent.DataSource = Nothing


			End Select
			ChangeHeaderInfo()
			m_SuppressUIEvents = False

		Catch ex As Exception

		End Try

	End Sub

	Sub ChangeHeaderInfo()
		Dim modulCaption As String = String.Empty

		Select Case m_Selectedmodul
			Case SelectedModulKey.MODUL_EMPLOYEMENT_CATEGORIZATION
				modulCaption = m_Translate.GetSafeTranslationValue("Einsatz-Einstufung")

			Case SelectedModulKey.MODUL_EMPLOYEE_CONTACT
				modulCaption = m_Translate.GetSafeTranslationValue("Kandidaten-Kontaktdaten")

			Case SelectedModulKey.MODUL_EMPLOYEE_FSTATE
				modulCaption = m_Translate.GetSafeTranslationValue("Kandidaten-1. Status")

			Case SelectedModulKey.MODUL_EMPLOYEE_SSTATE
				modulCaption = m_Translate.GetSafeTranslationValue("Kandidaten-2. Status")

			Case SelectedModulKey.MODUL_EMPLOYEE_ASSESSMENT
				modulCaption = m_Translate.GetSafeTranslationValue("Kandidaten Beurteilung")

			Case SelectedModulKey.MODUL_EMPLOYEE_COMMUNICATIONTYPE
				modulCaption = m_Translate.GetSafeTranslationValue("Kandidaten Beurteilung")

			Case SelectedModulKey.MODUL_EMPLOYEE_LANGUAGELETTER
				modulCaption = m_Translate.GetSafeTranslationValue("Kandidaten Sprachen")

			Case SelectedModulKey.MODUL_EMPLOYEE_CARRESERVE
				modulCaption = m_Translate.GetSafeTranslationValue("Fahrzeug-Reserve")

			Case SelectedModulKey.MODUL_EMPLOYEE_DRIVINGLICENCE
				modulCaption = m_Translate.GetSafeTranslationValue("Führerschein")

			Case SelectedModulKey.MODUL_EMPLOYEE_VEHICLE
				modulCaption = m_Translate.GetSafeTranslationValue("Fahrzeuge")

			Case SelectedModulKey.MODUL_EMPLOYEE_CONTACTRES1
				modulCaption = m_Translate.GetSafeTranslationValue("1. Reserve")
			Case SelectedModulKey.MODUL_EMPLOYEE_CONTACTRES2
				modulCaption = m_Translate.GetSafeTranslationValue("2. Reserve")
			Case SelectedModulKey.MODUL_EMPLOYEE_CONTACTRES3
				modulCaption = m_Translate.GetSafeTranslationValue("3. Reserve")
			Case SelectedModulKey.MODUL_EMPLOYEE_CONTACTRES4
				modulCaption = m_Translate.GetSafeTranslationValue("4. Reserve")

			Case SelectedModulKey.MODUL_EMPLOYEE_DEADLINE
				modulCaption = m_Translate.GetSafeTranslationValue("Kündigungsfristen")

			Case SelectedModulKey.MODUL_EMPLOYEE_WORKPENSUM
				modulCaption = m_Translate.GetSafeTranslationValue("Arbeitspensum")

			Case SelectedModulKey.MODUL_EMPLOYEE_EMPLOYEMENTTYPE
				modulCaption = m_Translate.GetSafeTranslationValue("Anstellungen")

			Case SelectedModulKey.MODUL_EMPLOYEE_DOCUMENTCATEGORY
				modulCaption = m_Translate.GetSafeTranslationValue("Dokumenten-Kategorie")

			Case SelectedModulKey.MODUL_EMPLOYEE_INTERVIEWSTATE
				modulCaption = m_Translate.GetSafeTranslationValue("Vorstellung-Status")




			Case SelectedModulKey.MODUL_CUSTOMER_PROPERTY
				modulCaption = m_Translate.GetSafeTranslationValue("1. Eigenschaften")

			Case SelectedModulKey.MODUL_CUSTOMER_CONTACT
				modulCaption = m_Translate.GetSafeTranslationValue("Kontakt")

			Case SelectedModulKey.MODUL_CUSTOMER_FSTATE
				modulCaption = m_Translate.GetSafeTranslationValue("1. Status")

			Case SelectedModulKey.MODUL_CUSTOMER_SSTATE
				modulCaption = m_Translate.GetSafeTranslationValue("2. Status")

			Case SelectedModulKey.MODUL_CUSTOMER_STICHWORT
				modulCaption = m_Translate.GetSafeTranslationValue("Stichworte")

			Case SelectedModulKey.MODUL_CUSTOMER_EMPLOYEMENTTYPE
				modulCaption = m_Translate.GetSafeTranslationValue("Anstellungsart")

			Case SelectedModulKey.MODUL_CUSTOMER_PAYMENTREMINDERCODE
				modulCaption = m_Translate.GetSafeTranslationValue("Mahncode")

			Case SelectedModulKey.MODUL_CUSTOMER_PAYMENTCONDITION
				modulCaption = m_Translate.GetSafeTranslationValue("Zahlungskondition")

			Case SelectedModulKey.MODUL_CUSTOMER_INVOICETYPE
				modulCaption = m_Translate.GetSafeTranslationValue("Faktura-Art")

			Case SelectedModulKey.MODUL_CUSTOMER_INVOICEOPTIONS
				modulCaption = m_Translate.GetSafeTranslationValue("Faktura-Optionen")

			Case SelectedModulKey.MODUL_CUSTOMER_INVOICESHIPMENT
				modulCaption = m_Translate.GetSafeTranslationValue("Mit Rechnung senden")

			Case SelectedModulKey.MODUL_CUSTOMER_NUMBEROFEMPLOYEES
				modulCaption = m_Translate.GetSafeTranslationValue("Betriebsgrösse")

			Case SelectedModulKey.MODUL_CUSTOMER_CONTACTRES1
				modulCaption = m_Translate.GetSafeTranslationValue("1. Reserve")
			Case SelectedModulKey.MODUL_CUSTOMER_CONTACTRES2
				modulCaption = m_Translate.GetSafeTranslationValue("2. Reserve")
			Case SelectedModulKey.MODUL_CUSTOMER_CONTACTRES3
				modulCaption = m_Translate.GetSafeTranslationValue("3. Reserve")
			Case SelectedModulKey.MODUL_CUSTOMER_CONTACTRES4
				modulCaption = m_Translate.GetSafeTranslationValue("4. Reserve")

			Case SelectedModulKey.MODUL_CUSTOMER_DOCUMENTCATEGORY
				modulCaption = m_Translate.GetSafeTranslationValue("Dokumenten-Kategorie")


			Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_CONTACT
				modulCaption = m_Translate.GetSafeTranslationValue("ZHD-Kontakte")

			Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_FSTATE
				modulCaption = m_Translate.GetSafeTranslationValue("ZHD-1. Status")

			Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_SSTATE
				modulCaption = m_Translate.GetSafeTranslationValue("ZHD-2. Status")

			Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_COMMUNICATION
				modulCaption = m_Translate.GetSafeTranslationValue("ZHD-Kommunikation")

			Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_COMMUNICATIONTYPE
				modulCaption = m_Translate.GetSafeTranslationValue("ZHD-Versandart")

			Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_DEPARTMENT
				modulCaption = m_Translate.GetSafeTranslationValue("ZHD-Abteilung")

			Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_POSITION
				modulCaption = m_Translate.GetSafeTranslationValue("ZHD-Position")

			Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_CONTACTRES1
				modulCaption = m_Translate.GetSafeTranslationValue("1. Reserve")
			Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_CONTACTRES2
				modulCaption = m_Translate.GetSafeTranslationValue("2. Reserve")
			Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_CONTACTRES3
				modulCaption = m_Translate.GetSafeTranslationValue("3. Reserve")
			Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_CONTACTRES4
				modulCaption = m_Translate.GetSafeTranslationValue("4. Reserve")




				' Vacancy
			Case SelectedModulKey.MODUL_VACANCY_CONTACT
				modulCaption = m_Translate.GetSafeTranslationValue("Vakanzen-Konatdaten")

			Case SelectedModulKey.MODUL_VACANCY_STATE
				modulCaption = m_Translate.GetSafeTranslationValue("Vakanzen-Status")

			Case SelectedModulKey.MODUL_VACANCY_GROUP
				modulCaption = m_Translate.GetSafeTranslationValue("Vakanzen-Gruppe")


				' Offer
			Case SelectedModulKey.MODUL_OFFER_CONTACT
				modulCaption = m_Translate.GetSafeTranslationValue("Offerten-Konatdaten")

			Case SelectedModulKey.MODUL_OFFER_STATE
				modulCaption = m_Translate.GetSafeTranslationValue("Offerten-Status")

			Case SelectedModulKey.MODUL_OFFER_GROUP
				modulCaption = m_Translate.GetSafeTranslationValue("Offerten-Gruppe")




				' common
			Case SelectedModulKey.MODUL_CONTACT_CATEGORY
				modulCaption = m_Translate.GetSafeTranslationValue("Kontakt-Kategorie")

			Case SelectedModulKey.MODUL_DIVERSE_BRANCHES
				modulCaption = m_Translate.GetSafeTranslationValue("Brachen")

			Case SelectedModulKey.MODUL_DIVERSE_QUALIFICATION
				modulCaption = m_Translate.GetSafeTranslationValue("Qualifikation")

			Case SelectedModulKey.MODUL_DIVERSE_BVGGENTS
				modulCaption = m_Translate.GetSafeTranslationValue("BVG-Ansätze Männer")

			Case SelectedModulKey.MODUL_DIVERSE_BVGFEMALE
				modulCaption = m_Translate.GetSafeTranslationValue("BVG-Ansätze Frauen")

			Case SelectedModulKey.MODUL_DIVERSE_QST
				modulCaption = m_Translate.GetSafeTranslationValue("Quellensteuer-Informationen")

			Case SelectedModulKey.MODUL_DIVERSE_COUNTRY
				modulCaption = m_Translate.GetSafeTranslationValue("Länder")

			Case SelectedModulKey.MODUL_DIVERSE_FCOSTCENTER
				modulCaption = m_Translate.GetSafeTranslationValue("1. Kostenstelle")

			Case SelectedModulKey.MODUL_DIVERSE_SCOSTCENTER
				modulCaption = m_Translate.GetSafeTranslationValue("2. Kostenstelle")

			Case SelectedModulKey.MODUL_DIVERSE_FF13SALARY
				modulCaption = m_Translate.GetSafeTranslationValue("Ferien, Feiertag und 13. Monatslohn")

			Case SelectedModulKey.MODUL_DIVERSE_FIBU
				modulCaption = m_Translate.GetSafeTranslationValue("FIBU-Konten")

			Case SelectedModulKey.MODUL_DIVERSE_BUSINESSBRANCHS
				modulCaption = m_Translate.GetSafeTranslationValue("Filialen")

			Case SelectedModulKey.MODUL_DIVERSE_ABSENCE
				modulCaption = m_Translate.GetSafeTranslationValue("Fehlcode")

			Case SelectedModulKey.MODUL_DIVERSE_AGBWOS
				modulCaption = m_Translate.GetSafeTranslationValue("AGB für WOS")

			Case SelectedModulKey.MODUL_DIVERSE_SALUTATION
				modulCaption = m_Translate.GetSafeTranslationValue("Anrede und Anredeformen")

			Case SelectedModulKey.MODUL_DIVERSE_SMSTEMPLATE
				modulCaption = m_Translate.GetSafeTranslationValue("SMS-Vorlagen")

			Case SelectedModulKey.MODUL_MAIN_PRINTTEMPLATES
				modulCaption = m_Translate.GetSafeTranslationValue("Druckvorlage")

			Case SelectedModulKey.MODUL_MAIN_EXPORTTEMPLATES
				modulCaption = m_Translate.GetSafeTranslationValue("Exportmodule")


			Case Else
				modulCaption = ""

		End Select
		lblTableCaption.Text = String.Format("<b>{0}</b>", modulCaption)

	End Sub


#Region "Reset"


	''' <summary>
	''' Resets the Mandanten drop down.
	''' </summary>
	Private Sub ResetMandantenDropDown()

		lueMandant.Properties.DisplayMember = "MandantName1"
		lueMandant.Properties.ValueMember = "MandantNumber"

		lueMandant.Properties.Columns.Clear()
		lueMandant.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "MandantName1",
																					 .Width = 100,
																					 .Caption = m_Translate.GetSafeTranslationValue("Mandant")})

		lueMandant.Properties.ShowHeader = False
		lueMandant.Properties.ShowFooter = False

		lueMandant.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueMandant.Properties.SearchMode = SearchMode.AutoComplete
		lueMandant.Properties.AutoSearchColumnIndex = 0

		lueMandant.Properties.NullText = String.Empty
		lueMandant.EditValue = Nothing

	End Sub

	Private Sub ResetYearDataDropDown()
		lueYear.Properties.DisplayMember = "Value"
		lueYear.Properties.ValueMember = "Value"
		lueYear.Properties.ShowHeader = False

		lueYear.Properties.Columns.Clear()
		lueYear.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "Value",
																					 .Width = 100,
																					 .Caption = m_Translate.GetSafeTranslationValue("Value")})

		lueYear.Properties.ShowFooter = False
		lueYear.Properties.DropDownRows = 10
		lueYear.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueYear.Properties.SearchMode = SearchMode.AutoComplete
		lueYear.Properties.AutoSearchColumnIndex = 0

		lueYear.Properties.NullText = String.Empty
		lueYear.EditValue = Nothing
	End Sub


	''' <summary>
	''' reset grid
	''' </summary>
	Private Sub ResetEmployeeContactGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()

		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recid"
			columnrecid.FieldName = "recid"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnbez_value As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_value.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_value.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnbez_value.Name = "bez_value"
			columnbez_value.FieldName = "bez_value"
			columnbez_value.Visible = True
			columnbez_value.BestFit()
			gvTableContent.Columns.Add(columnbez_value)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetEmployeeFStateGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()

		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recid"
			columnrecid.FieldName = "recid"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnsalutation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnsalutation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnsalutation.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnsalutation.Name = "bez_value"
			columnsalutation.FieldName = "bez_value"
			columnsalutation.Visible = True
			columnsalutation.BestFit()
			gvTableContent.Columns.Add(columnsalutation)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetEmployeeSStateGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()

		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recid"
			columnrecid.FieldName = "recid"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnbez_value As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_value.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_value.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnbez_value.Name = "bez_value"
			columnbez_value.FieldName = "bez_value"
			columnbez_value.Visible = True
			columnbez_value.BestFit()
			gvTableContent.Columns.Add(columnbez_value)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetEmployeeCivilstateGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()

		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recid"
			columnrecid.FieldName = "recid"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnGetField As New DevExpress.XtraGrid.Columns.GridColumn()
			columnGetField.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnGetField.Caption = m_Translate.GetSafeTranslationValue("Code")
			columnGetField.Name = "GetField"
			columnGetField.FieldName = "GetField"
			columnGetField.Visible = True
			columnGetField.BestFit()
			gvTableContent.Columns.Add(columnGetField)

			Dim columnDescription As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDescription.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDescription.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnDescription.Name = "Description"
			columnDescription.FieldName = "Description"
			columnDescription.Visible = True
			columnDescription.BestFit()
			gvTableContent.Columns.Add(columnDescription)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetEmployeeLanguageLetterGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recid"
			columnrecid.FieldName = "recid"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnsalutation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnsalutation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnsalutation.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnsalutation.Name = "bez_value"
			columnsalutation.FieldName = "bez_value"
			columnsalutation.Visible = True
			columnsalutation.BestFit()
			gvTableContent.Columns.Add(columnsalutation)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recid"
			columnrecid.FieldName = "recid"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnsalutation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnsalutation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnsalutation.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnsalutation.Name = "bez_value"
			columnsalutation.FieldName = "bez_value"
			columnsalutation.Visible = True
			columnsalutation.BestFit()
			gvTableContent.Columns.Add(columnsalutation)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetEmployeeCommunicationTypeGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recid"
			columnrecid.FieldName = "recid"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnsalutation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnsalutation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnsalutation.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnsalutation.Name = "bez_value"
			columnsalutation.FieldName = "bez_value"
			columnsalutation.Visible = True
			columnsalutation.BestFit()
			gvTableContent.Columns.Add(columnsalutation)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetEmployeeCarReserveGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recid"
			columnrecid.FieldName = "recid"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnsalutation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnsalutation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnsalutation.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnsalutation.Name = "bez_value"
			columnsalutation.FieldName = "bez_value"
			columnsalutation.Visible = True
			columnsalutation.BestFit()
			gvTableContent.Columns.Add(columnsalutation)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetEmployeeDrivinglicenceGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recid"
			columnrecid.FieldName = "recid"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnsalutation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnsalutation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnsalutation.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnsalutation.Name = "bez_value"
			columnsalutation.FieldName = "bez_value"
			columnsalutation.Width = 20
			columnsalutation.Visible = True
			gvTableContent.Columns.Add(columnsalutation)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.BestFit()
			columnbez_d.Visible = True
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetEmployeeVehicleGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recid"
			columnrecid.FieldName = "recid"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnsalutation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnsalutation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnsalutation.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnsalutation.Name = "bez_value"
			columnsalutation.FieldName = "bez_value"
			columnsalutation.Visible = True
			columnsalutation.BestFit()
			gvTableContent.Columns.Add(columnsalutation)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetEmployeeContactReserveGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recid"
			columnrecid.FieldName = "recid"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnsalutation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnsalutation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnsalutation.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnsalutation.Name = "bez_value"
			columnsalutation.FieldName = "bez_value"
			columnsalutation.Visible = True
			columnsalutation.BestFit()
			gvTableContent.Columns.Add(columnsalutation)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetEmployeeDeadLineGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recid"
			columnrecid.FieldName = "recid"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnsalutation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnsalutation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnsalutation.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnsalutation.Name = "bez_value"
			columnsalutation.FieldName = "bez_value"
			columnsalutation.Visible = True
			columnsalutation.BestFit()
			gvTableContent.Columns.Add(columnsalutation)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetEmployeeWorkPensumGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recid"
			columnrecid.FieldName = "recid"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnsalutation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnsalutation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnsalutation.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnsalutation.Name = "bez_value"
			columnsalutation.FieldName = "bez_value"
			columnsalutation.Visible = True
			columnsalutation.BestFit()
			gvTableContent.Columns.Add(columnsalutation)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdTableContent.DataSource = Nothing

	End Sub

	Private Sub ResetEmployeeEmployementTypeGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recid"
			columnrecid.FieldName = "recid"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnsalutation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnsalutation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnsalutation.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnsalutation.Name = "bez_value"
			columnsalutation.FieldName = "bez_value"
			columnsalutation.Visible = True
			columnsalutation.BestFit()
			gvTableContent.Columns.Add(columnsalutation)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetEmployeeDocumentCategoryGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "ID"
			columnrecid.FieldName = "ID"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnrecnr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecnr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnrecnr.Caption = m_Translate.GetSafeTranslationValue("Kategorie-Nr.")
			columnrecnr.Name = "CategoryNumber"
			columnrecnr.FieldName = "CategoryNumber"
			columnrecnr.Visible = True
			columnrecnr.BestFit()
			gvTableContent.Columns.Add(columnrecnr)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "DescriptionGerman"
			columnbez_d.FieldName = "DescriptionGerman"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "DescriptionItalian"
			columnbez_i.FieldName = "DescriptionItalian"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "DescriptionFrench"
			columnbez_f.FieldName = "DescriptionFrench"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

			Dim columnbez_e As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_e.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_e.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (EN)")
			columnbez_e.Name = "DescriptionEnglish"
			columnbez_e.FieldName = "DescriptionEnglish"
			columnbez_e.Visible = True
			columnbez_e.BestFit()
			gvTableContent.Columns.Add(columnbez_e)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdTableContent.DataSource = Nothing

	End Sub

	Private Sub ResetEmployeeInterviewStateGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recid"
			columnrecid.FieldName = "recid"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnsalutation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnsalutation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnsalutation.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnsalutation.Name = "bez_value"
			columnsalutation.FieldName = "bez_value"
			columnsalutation.Visible = True
			columnsalutation.BestFit()
			gvTableContent.Columns.Add(columnsalutation)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetEmployeePrintTemplatesGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()

		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("recid")
			columnrecid.Name = "recid"
			columnrecid.FieldName = "recid"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnrecnr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecnr.Caption = m_Translate.GetSafeTranslationValue("Reihennummer")
			columnrecnr.Name = "recnr"
			columnrecnr.FieldName = "recnr"
			columnrecnr.Width = 10
			columnrecnr.Visible = True
			gvTableContent.Columns.Add(columnrecnr)

			Dim columnsecnr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnsecnr.Caption = m_Translate.GetSafeTranslationValue("secnr")
			columnsecnr.Name = "secnr"
			columnsecnr.FieldName = "secnr"
			columnsecnr.Visible = False
			gvTableContent.Columns.Add(columnsecnr)

			Dim columndocnr As New DevExpress.XtraGrid.Columns.GridColumn()
			columndocnr.Caption = m_Translate.GetSafeTranslationValue("docnr")
			columndocnr.Name = "docnr"
			columndocnr.FieldName = "docnr"
			columndocnr.Visible = False
			gvTableContent.Columns.Add(columndocnr)

			Dim columndocfullname As New DevExpress.XtraGrid.Columns.GridColumn()
			columndocfullname.Caption = m_Translate.GetSafeTranslationValue("Dokumentenname")
			columndocfullname.Name = "docfullname"
			columndocfullname.FieldName = "docfullname"
			columndocfullname.Visible = True
			gvTableContent.Columns.Add(columndocfullname)

			Dim columnmakroname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmakroname.Caption = m_Translate.GetSafeTranslationValue("Makroname")
			columnmakroname.Name = "makroname"
			columnmakroname.FieldName = "makroname"
			columnmakroname.Width = 50
			columnmakroname.Visible = True
			gvTableContent.Columns.Add(columnmakroname)

			Dim columnmenulabel As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmenulabel.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnmenulabel.Name = "menulabel"
			columnmenulabel.FieldName = "menulabel"
			columnmenulabel.Width = 100
			columnmenulabel.Visible = True
			gvTableContent.Columns.Add(columnmenulabel)

			Dim columnitemshowin As New DevExpress.XtraGrid.Columns.GridColumn()
			columnitemshowin.Caption = m_Translate.GetSafeTranslationValue("itemshowin")
			columnitemshowin.Name = "itemshowin"
			columnitemshowin.FieldName = "itemshowin"
			columnitemshowin.Visible = False
			gvTableContent.Columns.Add(columnitemshowin)

			Dim columncreatedon As New DevExpress.XtraGrid.Columns.GridColumn()
			columncreatedon.Caption = m_Translate.GetSafeTranslationValue("createdon")
			columncreatedon.Name = "createdon"
			columncreatedon.FieldName = "createdon"
			columncreatedon.Visible = False
			gvTableContent.Columns.Add(columncreatedon)

			Dim columncreatedfrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columncreatedfrom.Caption = m_Translate.GetSafeTranslationValue("createdfrom")
			columncreatedfrom.Name = "createdfrom"
			columncreatedfrom.FieldName = "createdfrom"
			columncreatedfrom.Visible = False
			gvTableContent.Columns.Add(columncreatedfrom)

			Dim columnchangedon As New DevExpress.XtraGrid.Columns.GridColumn()
			columnchangedon.Caption = m_Translate.GetSafeTranslationValue("changedon")
			columnchangedon.Name = "changedon"
			columnchangedon.FieldName = "changedon"
			columnchangedon.Visible = False
			gvTableContent.Columns.Add(columnchangedon)

			Dim columnchangedfrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnchangedfrom.Caption = m_Translate.GetSafeTranslationValue("changedfrom")
			columnchangedfrom.Name = "changedfrom"
			columnchangedfrom.FieldName = "changedfrom"
			columnchangedfrom.Visible = False
			gvTableContent.Columns.Add(columnchangedfrom)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdTableContent.DataSource = Nothing

	End Sub


	Private Sub ResetSalutationGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recid"
			columnrecid.FieldName = "recid"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnsalutation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnsalutation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnsalutation.Caption = m_Translate.GetSafeTranslationValue("Anrede")
			columnsalutation.Name = "salutation"
			columnsalutation.FieldName = "salutation"
			columnsalutation.Visible = True
			columnsalutation.Width = 40
			gvTableContent.Columns.Add(columnsalutation)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Anrede (DE)")
			columnbez_d.Name = "salutation_d"
			columnbez_d.FieldName = "salutation_d"
			columnbez_d.Visible = True
			columnbez_d.Width = 40
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Anrede (IT)")
			columnbez_i.Name = "salutation_i"
			columnbez_i.FieldName = "salutation_i"
			columnbez_i.Visible = True
			columnbez_i.Width = 40
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Anrede (FR)")
			columnbez_f.Name = "salutation_f"
			columnbez_f.FieldName = "salutation_f"
			columnbez_f.Visible = True
			columnbez_f.Width = 40
			gvTableContent.Columns.Add(columnbez_f)

			Dim columnbez_e As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_e.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_e.Caption = m_Translate.GetSafeTranslationValue("Anrede (EN)")
			columnbez_e.Name = "salutation_e"
			columnbez_e.FieldName = "salutation_e"
			columnbez_e.Visible = True
			columnbez_e.Width = 40
			gvTableContent.Columns.Add(columnbez_e)

			Dim columnletterform As New DevExpress.XtraGrid.Columns.GridColumn()
			columnletterform.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnletterform.Caption = m_Translate.GetSafeTranslationValue("Anredeform")
			columnletterform.Name = "letterform"
			columnletterform.FieldName = "letterform"
			columnletterform.Visible = True
			columnletterform.BestFit()
			gvTableContent.Columns.Add(columnletterform)

			Dim columnletterform_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnletterform_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnletterform_d.Caption = m_Translate.GetSafeTranslationValue("Anredeform (DE)")
			columnletterform_d.Name = "letterform_d"
			columnletterform_d.FieldName = "letterform_d"
			columnletterform_d.Visible = True
			columnletterform_d.BestFit()
			gvTableContent.Columns.Add(columnletterform_d)

			Dim columnletterform_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnletterform_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnletterform_i.Caption = m_Translate.GetSafeTranslationValue("Anredeform (IT)")
			columnletterform_i.Name = "letterform_i"
			columnletterform_i.FieldName = "letterform_i"
			columnletterform_i.Visible = True
			columnletterform_i.BestFit()
			gvTableContent.Columns.Add(columnletterform_i)

			Dim columnletterform_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnletterform_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnletterform_f.Caption = m_Translate.GetSafeTranslationValue("Anredeform (FR)")
			columnletterform_f.Name = "letterform_f"
			columnletterform_f.FieldName = "letterform_f"
			columnletterform_f.Visible = True
			columnletterform_f.BestFit()
			gvTableContent.Columns.Add(columnletterform_f)

			Dim columnletterform_e As New DevExpress.XtraGrid.Columns.GridColumn()
			columnletterform_e.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnletterform_e.Caption = m_Translate.GetSafeTranslationValue("Anredeform (EN)")
			columnletterform_e.Name = "letterform_e"
			columnletterform_e.FieldName = "letterform_e"
			columnletterform_e.Visible = True
			columnletterform_e.BestFit()
			gvTableContent.Columns.Add(columnletterform_e)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdTableContent.DataSource = Nothing

	End Sub



	Private Sub ResetSMSTemplateGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recid"
			columnrecid.FieldName = "recid"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columndescription As New DevExpress.XtraGrid.Columns.GridColumn()
			columndescription.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columndescription.Caption = m_Translate.GetSafeTranslationValue("Anzeige")
			columndescription.Name = "bez_value"
			columndescription.FieldName = "bez_value"
			columndescription.Visible = True
			columndescription.BestFit()
			gvTableContent.Columns.Add(columndescription)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
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


	Private Sub ResetDeletedGrid_()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnrecid.Name = "recid"
			columnrecid.FieldName = "recid"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCreatedFrom.Caption = m_Translate.GetSafeTranslationValue("Benutzer")
			columnCreatedFrom.Name = "createdfrom"
			columnCreatedFrom.FieldName = "createdfrom"
			columnCreatedFrom.Visible = True
			columnCreatedFrom.BestFit()
			gvTableContent.Columns.Add(columnCreatedFrom)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.Caption = m_Translate.GetSafeTranslationValue("Datum")
			columnCreatedOn.Name = "createdon"
			columnCreatedOn.FieldName = "createdon"
			columnCreatedOn.Visible = True
			columnCreatedOn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
			columnCreatedOn.BestFit()
			gvTableContent.Columns.Add(columnCreatedOn)

			Dim columndeletednumber As New DevExpress.XtraGrid.Columns.GridColumn()
			columndeletednumber.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columndeletednumber.Name = "deletenumber"
			columndeletednumber.FieldName = "deletenumber"
			columndeletednumber.Visible = True
			columndeletednumber.BestFit()
			gvTableContent.Columns.Add(columndeletednumber)

			Dim columndeleteinfo As New DevExpress.XtraGrid.Columns.GridColumn()
			columndeleteinfo.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columndeleteinfo.Caption = m_Translate.GetSafeTranslationValue("Info")
			columndeleteinfo.Name = "deleteinfo"
			columndeleteinfo.FieldName = "deleteinfo"
			columndeleteinfo.Visible = True
			columndeleteinfo.BestFit()
			gvTableContent.Columns.Add(columndeleteinfo)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdTableContent.DataSource = Nothing

	End Sub



	''' <summary>
	''' reset Customer grid
	''' </summary>
	Private Sub ResetCustomerPropertyGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recid"
			columnrecid.FieldName = "recid"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnsalutation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnsalutation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnsalutation.Caption = m_Translate.GetSafeTranslationValue("Farbnummer")
			columnsalutation.Name = "bez_value"
			columnsalutation.FieldName = "bez_value"
			columnsalutation.Visible = True
			columnsalutation.BestFit()
			gvTableContent.Columns.Add(columnsalutation)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetCustomerContactGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recid"
			columnrecid.FieldName = "recid"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnsalutation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnsalutation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnsalutation.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnsalutation.Name = "bez_value"
			columnsalutation.FieldName = "bez_value"
			columnsalutation.Visible = True
			columnsalutation.BestFit()
			gvTableContent.Columns.Add(columnsalutation)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetCustomerFStateGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recid"
			columnrecid.FieldName = "recid"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnsalutation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnsalutation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnsalutation.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnsalutation.Name = "bez_value"
			columnsalutation.FieldName = "bez_value"
			columnsalutation.Visible = True
			columnsalutation.BestFit()
			gvTableContent.Columns.Add(columnsalutation)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetCustomerSStateGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recid"
			columnrecid.FieldName = "recid"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnsalutation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnsalutation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnsalutation.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnsalutation.Name = "bez_value"
			columnsalutation.FieldName = "bez_value"
			columnsalutation.Visible = True
			columnsalutation.BestFit()
			gvTableContent.Columns.Add(columnsalutation)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetCustomerStichwortGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recid"
			columnrecid.FieldName = "recid"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnsalutation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnsalutation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnsalutation.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnsalutation.Name = "bez_value"
			columnsalutation.FieldName = "bez_value"
			columnsalutation.Visible = True
			columnsalutation.BestFit()
			gvTableContent.Columns.Add(columnsalutation)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetCustomerEmployementTypeGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recid"
			columnrecid.FieldName = "recid"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnsalutation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnsalutation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnsalutation.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnsalutation.Name = "bez_value"
			columnsalutation.FieldName = "bez_value"
			columnsalutation.Visible = True
			columnsalutation.BestFit()
			gvTableContent.Columns.Add(columnsalutation)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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


	Private Sub ResetCustomerContactReserveGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "ID"
			columnrecid.FieldName = "ID"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnDescription As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDescription.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDescription.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnDescription.Name = "Description"
			columnDescription.FieldName = "Description"
			columnDescription.Visible = True
			columnDescription.BestFit()
			gvTableContent.Columns.Add(columnDescription)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetCustomerPaymentReminderCodeGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "ID"
			columnrecid.FieldName = "ID"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnbez_e As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_e.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_e.Caption = m_Translate.GetSafeTranslationValue("Code")
			columnbez_e.Name = "GetField"
			columnbez_e.FieldName = "GetField"
			columnbez_e.Visible = True
			columnbez_e.Width = 20
			gvTableContent.Columns.Add(columnbez_e)

			Dim columnsalutation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnsalutation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnsalutation.Caption = m_Translate.GetSafeTranslationValue("Kontoauszug: Tage nach Fakturadatum")
			columnsalutation.Name = "Reminder1"
			columnsalutation.FieldName = "Reminder1"
			columnsalutation.Visible = True
			columnsalutation.BestFit()
			gvTableContent.Columns.Add(columnsalutation)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("1. Mahnung: Tage nach Kontoauszug")
			columnbez_d.Name = "Reminder2"
			columnbez_d.FieldName = "Reminder2"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("2. Mahnung: Tage nach 1. Mahnung")
			columnbez_i.Name = "Reminder3"
			columnbez_i.FieldName = "Reminder3"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Inkasso: Tage nach 2. Mahnung")
			columnbez_f.Name = "Reminder4"
			columnbez_f.FieldName = "Reminder4"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdTableContent.DataSource = Nothing

	End Sub

	Private Sub ResetCustomerPaymentConditionGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "ID"
			columnrecid.FieldName = "ID"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnsalutation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnsalutation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnsalutation.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnsalutation.Name = "GetField"
			columnsalutation.FieldName = "GetField"
			columnsalutation.Visible = True
			columnsalutation.BestFit()
			gvTableContent.Columns.Add(columnsalutation)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetCustomerInvoiceOptionsGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "ID"
			columnrecid.FieldName = "ID"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnsalutation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnsalutation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnsalutation.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnsalutation.Name = "Description"
			columnsalutation.FieldName = "Description"
			columnsalutation.Visible = True
			columnsalutation.BestFit()
			gvTableContent.Columns.Add(columnsalutation)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetCustomerInvoiceTypeGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "ID"
			columnrecid.FieldName = "ID"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnsalutation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnsalutation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnsalutation.Caption = m_Translate.GetSafeTranslationValue("Code")
			columnsalutation.Name = "Code"
			columnsalutation.FieldName = "Code"
			columnsalutation.Visible = True
			columnsalutation.MaxWidth = 80
			gvTableContent.Columns.Add(columnsalutation)

			Dim columnBezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBezeichnung.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnBezeichnung.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnBezeichnung.Name = "Description"
			columnBezeichnung.FieldName = "Description"
			columnBezeichnung.Visible = True
			columnBezeichnung.BestFit()
			gvTableContent.Columns.Add(columnBezeichnung)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetCustomerInvoiceShipmentGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "ID"
			columnrecid.FieldName = "ID"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnDescription As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDescription.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDescription.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnDescription.Name = "Description"
			columnDescription.FieldName = "Description"
			columnDescription.Visible = True
			columnDescription.BestFit()
			gvTableContent.Columns.Add(columnDescription)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetCustomerNumberOfEmployeesGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "ID"
			columnrecid.FieldName = "ID"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnsalutation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnsalutation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnsalutation.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnsalutation.Name = "NumberOfEmployees"
			columnsalutation.FieldName = "NumberOfEmployees"
			columnsalutation.Visible = True
			columnsalutation.Width = 40
			gvTableContent.Columns.Add(columnsalutation)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdTableContent.DataSource = Nothing

	End Sub

	Private Sub ResetCustomerDocumentCategoryGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "ID"
			columnrecid.FieldName = "ID"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnrecnr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecnr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnrecnr.Caption = m_Translate.GetSafeTranslationValue("Kategorie-Nr.")
			columnrecnr.Name = "CategoryNumber"
			columnrecnr.FieldName = "CategoryNumber"
			columnrecnr.Visible = True
			columnrecnr.BestFit()
			gvTableContent.Columns.Add(columnrecnr)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "DescriptionGerman"
			columnbez_d.FieldName = "DescriptionGerman"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "DescriptionItalian"
			columnbez_i.FieldName = "DescriptionItalian"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "DescriptionFrench"
			columnbez_f.FieldName = "DescriptionFrench"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

			Dim columnbez_e As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_e.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_e.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (EN)")
			columnbez_e.Name = "DescriptionEnglish"
			columnbez_e.FieldName = "DescriptionEnglish"
			columnbez_e.Visible = True
			columnbez_e.BestFit()
			gvTableContent.Columns.Add(columnbez_e)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdTableContent.DataSource = Nothing

	End Sub



	' Description

	''' <summary>
	''' reset repsonsible person
	''' </summary>
	Private Sub ResetResponsiblepersonContactGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "ID"
			columnrecid.FieldName = "ID"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnDescription As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDescription.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDescription.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnDescription.Name = "Description"
			columnDescription.FieldName = "Description"
			columnDescription.Visible = True
			columnDescription.BestFit()
			gvTableContent.Columns.Add(columnDescription)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetResponsiblepersonFStateGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "ID"
			columnrecid.FieldName = "ID"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnDescription As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDescription.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDescription.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnDescription.Name = "Description"
			columnDescription.FieldName = "Description"
			columnDescription.Visible = True
			columnDescription.BestFit()
			gvTableContent.Columns.Add(columnDescription)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetResponsiblepersonSStateGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "ID"
			columnrecid.FieldName = "ID"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnDescription As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDescription.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDescription.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnDescription.Name = "Description"
			columnDescription.FieldName = "Description"
			columnDescription.Visible = True
			columnDescription.BestFit()
			gvTableContent.Columns.Add(columnDescription)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetResponsiblepersonDepartmentGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "ID"
			columnrecid.FieldName = "ID"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnDescription As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDescription.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDescription.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnDescription.Name = "Description"
			columnDescription.FieldName = "Description"
			columnDescription.Visible = True
			columnDescription.BestFit()
			gvTableContent.Columns.Add(columnDescription)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetResponsiblepersonPositionGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "ID"
			columnrecid.FieldName = "ID"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnDescription As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDescription.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDescription.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnDescription.Name = "Description"
			columnDescription.FieldName = "Description"
			columnDescription.Visible = True
			columnDescription.BestFit()
			gvTableContent.Columns.Add(columnDescription)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetResponsiblepersonCommunicationGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "ID"
			columnrecid.FieldName = "ID"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnDescription As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDescription.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDescription.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnDescription.Name = "Description"
			columnDescription.FieldName = "Description"
			columnDescription.Visible = True
			columnDescription.BestFit()
			gvTableContent.Columns.Add(columnDescription)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetResponsiblepersonCommunicationTypeGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "ID"
			columnrecid.FieldName = "ID"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnDescription As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDescription.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDescription.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnDescription.Name = "Description"
			columnDescription.FieldName = "Description"
			columnDescription.Visible = True
			columnDescription.BestFit()
			gvTableContent.Columns.Add(columnDescription)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetResponsiblepersonContactReserveGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "ID"
			columnrecid.FieldName = "ID"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnDescription As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDescription.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDescription.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnDescription.Name = "Description"
			columnDescription.FieldName = "Description"
			columnDescription.Visible = True
			columnDescription.BestFit()
			gvTableContent.Columns.Add(columnDescription)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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




	''' <summary>
	'''  Vacancy 
	''' </summary>
	Private Sub ResetVacancyContactGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "ID"
			columnrecid.FieldName = "ID"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnrecvalue As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecvalue.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnrecvalue.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnrecvalue.Name = "recvalue"
			columnrecvalue.FieldName = "recvalue"
			columnrecvalue.Visible = True
			columnrecvalue.BestFit()
			gvTableContent.Columns.Add(columnrecvalue)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetVacancyStateGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "ID"
			columnrecid.FieldName = "ID"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnrecvalue As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecvalue.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnrecvalue.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnrecvalue.Name = "recvalue"
			columnrecvalue.FieldName = "recvalue"
			columnrecvalue.Visible = True
			columnrecvalue.BestFit()
			gvTableContent.Columns.Add(columnrecvalue)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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


	' bez_value
	Private Sub ResetVacancyGroupGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "ID"
			columnrecid.FieldName = "ID"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnbez_value As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_value.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_value.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnbez_value.Name = "Bez_Value"
			columnbez_value.FieldName = "Bez_Value"
			columnbez_value.Visible = True
			columnbez_value.BestFit()
			gvTableContent.Columns.Add(columnbez_value)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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




	''' <summary>
	'''  Offer
	''' </summary>
	Private Sub ResetOfferContactGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "ID"
			columnrecid.FieldName = "ID"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnDescription As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDescription.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDescription.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnDescription.Name = "Description"
			columnDescription.FieldName = "Description"
			columnDescription.Visible = True
			columnDescription.BestFit()
			gvTableContent.Columns.Add(columnDescription)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetOfferStateGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "ID"
			columnrecid.FieldName = "ID"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnDescription As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDescription.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDescription.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnDescription.Name = "Description"
			columnDescription.FieldName = "Description"
			columnDescription.Visible = True
			columnDescription.BestFit()
			gvTableContent.Columns.Add(columnDescription)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetOfferGroupGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()

		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "ID"
			columnrecid.FieldName = "ID"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnDescription As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDescription.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDescription.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnDescription.Name = "Description"
			columnDescription.FieldName = "Description"
			columnDescription.Visible = True
			columnDescription.BestFit()
			gvTableContent.Columns.Add(columnDescription)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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





	''' <summary>
	''' propose
	''' </summary>
	Private Sub ResetProposeStateGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "ID"
			columnrecid.FieldName = "ID"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnBezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBezeichnung.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnBezeichnung.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnBezeichnung.Name = "bez_value"
			columnBezeichnung.FieldName = "bez_value"
			columnBezeichnung.Visible = True
			columnBezeichnung.BestFit()
			gvTableContent.Columns.Add(columnBezeichnung)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetProposeEmployementTypeGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "ID"
			columnrecid.FieldName = "ID"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnBezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBezeichnung.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnBezeichnung.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnBezeichnung.Name = "bez_value"
			columnBezeichnung.FieldName = "bez_value"
			columnBezeichnung.Visible = True
			columnBezeichnung.BestFit()
			gvTableContent.Columns.Add(columnBezeichnung)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetProposeArtGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "ID"
			columnrecid.FieldName = "ID"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnBezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBezeichnung.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnBezeichnung.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnBezeichnung.Name = "bez_value"
			columnBezeichnung.FieldName = "bez_value"
			columnBezeichnung.Visible = True
			columnBezeichnung.BestFit()
			gvTableContent.Columns.Add(columnBezeichnung)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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




	''' <summary>
	''' common
	''' </summary>
	Private Sub ResetContactCategoryGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "ID"
			columnrecid.FieldName = "ID"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnrecnr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecnr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnrecnr.Caption = m_Translate.GetSafeTranslationValue("Reihenfolge")
			columnrecnr.Name = "RecNr"
			columnrecnr.FieldName = "RecNr"
			columnrecnr.Visible = True
			columnrecnr.BestFit()
			gvTableContent.Columns.Add(columnrecnr)

			Dim columnbez_value As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_value.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_value.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnbez_value.Name = "Bez_ID"
			columnbez_value.FieldName = "Bez_ID"
			columnbez_value.Visible = True
			columnbez_value.BestFit()
			gvTableContent.Columns.Add(columnbez_value)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "Caption_DE"
			columnbez_d.FieldName = "Caption_DE"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "Caption_IT"
			columnbez_i.FieldName = "Caption_IT"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "Caption_FR"
			columnbez_f.FieldName = "Caption_FR"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

			Dim columnbez_e As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_e.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_e.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (EN)")
			columnbez_e.Name = "Caption_EN"
			columnbez_e.FieldName = "Caption_EN"
			columnbez_e.Visible = True
			columnbez_e.BestFit()
			gvTableContent.Columns.Add(columnbez_e)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdTableContent.DataSource = Nothing

	End Sub

	Private Sub ResetEmployementCategorizedGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()

		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "ID"
			columnrecid.FieldName = "ID"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnDescription As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDescription.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDescription.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnDescription.Name = "Description"
			columnDescription.FieldName = "Description"
			columnDescription.Visible = True
			columnDescription.BestFit()
			gvTableContent.Columns.Add(columnDescription)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetFilialeGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()

		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recid"
			columnrecid.FieldName = "recid"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnCode_1 As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCode_1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCode_1.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnCode_1.Name = "Code_1"
			columnCode_1.FieldName = "Code_1"
			columnCode_1.Visible = True
			columnCode_1.BestFit()
			gvTableContent.Columns.Add(columnCode_1)

			Dim columnbez_value As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_value.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_value.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnbez_value.Name = "bez_value"
			columnbez_value.FieldName = "bez_value"
			columnbez_value.Visible = True
			columnbez_value.BestFit()
			gvTableContent.Columns.Add(columnbez_value)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdTableContent.DataSource = Nothing

	End Sub

	Private Sub ResetFCostcenterGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()

		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recId"
			columnrecid.FieldName = "recId"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnkstname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnkstname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnkstname.Caption = m_Translate.GetSafeTranslationValue("Name")
			columnkstname.Name = "kstname"
			columnkstname.FieldName = "kstname"
			columnkstname.Visible = True
			columnkstname.BestFit()
			gvTableContent.Columns.Add(columnkstname)

			Dim columnkstbezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
			columnkstbezeichnung.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnkstbezeichnung.Name = "kstbezeichnung"
			columnkstbezeichnung.FieldName = "kstbezeichnung"
			columnkstbezeichnung.Visible = True
			columnkstbezeichnung.BestFit()
			gvTableContent.Columns.Add(columnkstbezeichnung)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdTableContent.DataSource = Nothing

	End Sub

	Private Sub ResetSCostcenterGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()

		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recId"
			columnrecid.FieldName = "recId"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnkstname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnkstname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnkstname.Caption = m_Translate.GetSafeTranslationValue("Kostenstelle")
			columnkstname.Name = "kstname"
			columnkstname.FieldName = "kstname"
			columnkstname.Visible = True
			columnkstname.BestFit()
			gvTableContent.Columns.Add(columnkstname)

			Dim columnkstbezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
			columnkstbezeichnung.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnkstbezeichnung.Name = "kstbezeichnung"
			columnkstbezeichnung.FieldName = "kstbezeichnung"
			columnkstbezeichnung.Visible = True
			columnkstbezeichnung.BestFit()
			gvTableContent.Columns.Add(columnkstbezeichnung)

			Dim columnkstname1 As New DevExpress.XtraGrid.Columns.GridColumn()
			columnkstname1.Caption = m_Translate.GetSafeTranslationValue("1. Kostenstelle")
			columnkstname1.Name = "kstname1"
			columnkstname1.FieldName = "kstname1"
			columnkstname1.Visible = True
			columnkstname1.BestFit()
			gvTableContent.Columns.Add(columnkstname1)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdTableContent.DataSource = Nothing

	End Sub

	Private Sub ResetBVGDataGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()

		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recId"
			columnrecid.FieldName = "recId"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnAlter As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAlter.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnAlter.Caption = m_Translate.GetSafeTranslationValue("Alter")
			columnAlter.Name = "Alter"
			columnAlter.FieldName = "Alter"
			columnAlter.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnAlter.AppearanceHeader.Options.UseTextOptions = True
			columnAlter.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnAlter.AppearanceCell.Options.UseTextOptions = True
			columnAlter.MaxWidth = 50
			columnAlter.Visible = True
			gvTableContent.Columns.Add(columnAlter)

			Dim columnProzentSatz As New DevExpress.XtraGrid.Columns.GridColumn()
			columnProzentSatz.Caption = m_Translate.GetSafeTranslationValue("Prozentsatz")
			columnProzentSatz.Name = "ProzentSatz"
			columnProzentSatz.FieldName = "ProzentSatz"
			columnProzentSatz.Visible = True
			columnProzentSatz.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnProzentSatz.AppearanceHeader.Options.UseTextOptions = True
			columnProzentSatz.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnProzentSatz.AppearanceCell.Options.UseTextOptions = True
			columnProzentSatz.MaxWidth = 100
			gvTableContent.Columns.Add(columnProzentSatz)

			Dim columnProzJahr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnProzJahr.Caption = m_Translate.GetSafeTranslationValue("Jahr")
			columnProzJahr.Name = "ProzJahr"
			columnProzJahr.FieldName = "ProzJahr"
			columnProzJahr.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnProzJahr.AppearanceHeader.Options.UseTextOptions = True
			columnProzJahr.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnProzJahr.AppearanceCell.Options.UseTextOptions = True
			columnProzJahr.Visible = True
			columnProzJahr.MaxWidth = 100
			gvTableContent.Columns.Add(columnProzJahr)

			Dim columnMDNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMDNr.Caption = m_Translate.GetSafeTranslationValue("Mandanten-Nr.")
			columnMDNr.Name = "MDNr"
			columnMDNr.FieldName = "MDNr"
			columnMDNr.Visible = False
			columnMDNr.MaxWidth = 100
			gvTableContent.Columns.Add(columnMDNr)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdTableContent.DataSource = Nothing

	End Sub

	Private Sub ResetFF13LohnDataGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()

		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recId"
			columnrecid.FieldName = "recId"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnJahrgang As New DevExpress.XtraGrid.Columns.GridColumn()
			columnJahrgang.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnJahrgang.AppearanceCell.Options.UseTextOptions = True
			columnJahrgang.Caption = m_Translate.GetSafeTranslationValue("Alter")
			columnJahrgang.Name = "Jahrgang"
			columnJahrgang.FieldName = "Jahrgang"
			columnJahrgang.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnJahrgang.AppearanceHeader.Options.UseTextOptions = True
			columnJahrgang.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnJahrgang.AppearanceCell.Options.UseTextOptions = True
			columnJahrgang.Visible = True
			columnJahrgang.MaxWidth = 50
			gvTableContent.Columns.Add(columnJahrgang)

			Dim columnFerProzentSatz As New DevExpress.XtraGrid.Columns.GridColumn()
			columnFerProzentSatz.Caption = m_Translate.GetSafeTranslationValue("Ferienentschädigung")
			columnFerProzentSatz.Name = "FerProzentSatz"
			columnFerProzentSatz.FieldName = "FerProzentSatz"
			columnFerProzentSatz.Visible = True
			columnFerProzentSatz.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnFerProzentSatz.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnFerProzentSatz.AppearanceHeader.Options.UseTextOptions = True
			columnFerProzentSatz.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnFerProzentSatz.AppearanceCell.Options.UseTextOptions = True
			columnFerProzentSatz.MaxWidth = 200
			gvTableContent.Columns.Add(columnFerProzentSatz)

			Dim columnFeierProzentSatz As New DevExpress.XtraGrid.Columns.GridColumn()
			columnFeierProzentSatz.Caption = m_Translate.GetSafeTranslationValue("Feiertagsentschädigung")
			columnFeierProzentSatz.Name = "FeierProzentSatz"
			columnFeierProzentSatz.FieldName = "FeierProzentSatz"
			columnFeierProzentSatz.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnFeierProzentSatz.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnFeierProzentSatz.AppearanceHeader.Options.UseTextOptions = True
			columnFeierProzentSatz.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnFeierProzentSatz.AppearanceCell.Options.UseTextOptions = True
			columnFeierProzentSatz.Visible = True
			columnFeierProzentSatz.MaxWidth = 200
			gvTableContent.Columns.Add(columnFeierProzentSatz)

			Dim columnProzent13Satz As New DevExpress.XtraGrid.Columns.GridColumn()
			columnProzent13Satz.Caption = m_Translate.GetSafeTranslationValue("13. Lohn")
			columnProzent13Satz.Name = "Prozent13Satz"
			columnProzent13Satz.FieldName = "Prozent13Satz"
			columnProzent13Satz.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnProzent13Satz.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnProzent13Satz.AppearanceHeader.Options.UseTextOptions = True
			columnProzent13Satz.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnProzent13Satz.AppearanceCell.Options.UseTextOptions = True
			columnProzent13Satz.Visible = True
			columnProzent13Satz.MaxWidth = 200
			gvTableContent.Columns.Add(columnProzent13Satz)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdTableContent.DataSource = Nothing

	End Sub

	Private Sub ResetQSTDataGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()

		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recId"
			columnrecid.FieldName = "recId"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnKanton As New DevExpress.XtraGrid.Columns.GridColumn()
			columnKanton.Caption = m_Translate.GetSafeTranslationValue("Kanton")
			columnKanton.Name = "SKanton"
			columnKanton.FieldName = "SKanton"
			columnKanton.Visible = True
			columnKanton.MaxWidth = 50
			gvTableContent.Columns.Add(columnKanton)

			Dim columnMonthStd As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMonthStd.Caption = m_Translate.GetSafeTranslationValue("Stunden / Tage")
			columnMonthStd.Name = "MonthStd"
			columnMonthStd.FieldName = "MonthStd"
			columnMonthStd.Visible = True
			columnMonthStd.MaxWidth = 100
			gvTableContent.Columns.Add(columnMonthStd)

			'Dim columnCalendarDay As New DevExpress.XtraGrid.Columns.GridColumn()
			'columnCalendarDay.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			'columnCalendarDay.Caption = m_Translate.GetSafeTranslationValue("Monats-Kalender")
			'columnCalendarDay.Name = "CalendarDay"
			'columnCalendarDay.FieldName = "CalendarDay"
			'columnCalendarDay.Visible = True
			'columnCalendarDay.BestFit()
			'gvTableContent.Columns.Add(columnCalendarDay)

			Dim columnDESameAsCH As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDESameAsCH.Caption = m_Translate.GetSafeTranslationValue("DE wie CH")
			columnDESameAsCH.Name = "DESameAsCH"
			columnDESameAsCH.FieldName = "DESameAsCH"
			columnDESameAsCH.Visible = True
			columnDESameAsCH.BestFit()
			gvTableContent.Columns.Add(columnDESameAsCH)

			Dim columnStdDown As New DevExpress.XtraGrid.Columns.GridColumn()
			columnStdDown.Caption = m_Translate.GetSafeTranslationValue("Runterrechnen")
			columnStdDown.Name = "StdDown"
			columnStdDown.FieldName = "StdDown"
			columnStdDown.Visible = True
			columnStdDown.BestFit()
			gvTableContent.Columns.Add(columnStdDown)

			Dim columnStdUp As New DevExpress.XtraGrid.Columns.GridColumn()
			columnStdUp.Caption = m_Translate.GetSafeTranslationValue("Hochrechnen")
			columnStdUp.Name = "StdUp"
			columnStdUp.FieldName = "StdUp"
			columnStdUp.Visible = True
			columnStdUp.BestFit()
			gvTableContent.Columns.Add(columnStdUp)

			Dim columnJustAtEndBegin As New DevExpress.XtraGrid.Columns.GridColumn()
			columnJustAtEndBegin.Caption = m_Translate.GetSafeTranslationValue("NUR beim Ein-/Austritt")
			columnJustAtEndBegin.Name = "JustAtEndBegin"
			columnJustAtEndBegin.FieldName = "JustAtEndBegin"
			columnJustAtEndBegin.Visible = True
			columnJustAtEndBegin.BestFit()
			gvTableContent.Columns.Add(columnJustAtEndBegin)

			Dim columnStdDownAtEndBegin As New DevExpress.XtraGrid.Columns.GridColumn()
			columnStdDownAtEndBegin.Caption = m_Translate.GetSafeTranslationValue("Runterrechnen NUR beim Ein-/Austritt")
			columnStdDownAtEndBegin.Name = "StdDownAtEndBegin"
			columnStdDownAtEndBegin.FieldName = "StdDownAtEndBegin"
			columnStdDownAtEndBegin.Visible = True
			columnStdDownAtEndBegin.BestFit()
			gvTableContent.Columns.Add(columnStdDownAtEndBegin)

			Dim columnWithFLeistung As New DevExpress.XtraGrid.Columns.GridColumn()
			columnWithFLeistung.Caption = m_Translate.GetSafeTranslationValue("Mit Leistungen")
			columnWithFLeistung.Name = "WithFLeistung"
			columnWithFLeistung.FieldName = "WithFLeistung"
			columnWithFLeistung.Visible = True
			columnWithFLeistung.BestFit()
			gvTableContent.Columns.Add(columnWithFLeistung)

			Dim columnHandleAsAutomation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnHandleAsAutomation.Caption = m_Translate.GetSafeTranslationValue("HandleAsAutomation (UR)")
			columnHandleAsAutomation.Name = "HandleAsAutomation"
			columnHandleAsAutomation.FieldName = "HandleAsAutomation"
			columnHandleAsAutomation.Visible = True
			columnHandleAsAutomation.BestFit()
			gvTableContent.Columns.Add(columnHandleAsAutomation)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdTableContent.DataSource = Nothing

	End Sub


	Private Sub ResetSectorGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()

		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recId"
			columnrecid.FieldName = "recId"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columncode As New DevExpress.XtraGrid.Columns.GridColumn()
			columncode.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columncode.Caption = m_Translate.GetSafeTranslationValue("code")
			columncode.Name = "code"
			columncode.FieldName = "code"
			columncode.Visible = True
			columncode.MaxWidth = 100
			gvTableContent.Columns.Add(columncode)

			Dim columnbranche As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbranche.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbranche.Caption = m_Translate.GetSafeTranslationValue("Branche")
			columnbranche.Name = "branche"
			columnbranche.FieldName = "branche"
			columnbranche.Visible = True
			columnbranche.Width = 100
			gvTableContent.Columns.Add(columnbranche)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.Width = 100
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.Width = 100
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.Width = 100
			gvTableContent.Columns.Add(columnbez_f)

			Dim columnbez_e As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_e.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_e.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (EN)")
			columnbez_e.Name = "bez_e"
			columnbez_e.FieldName = "bez_e"
			columnbez_e.Visible = True
			columnbez_e.BestFit()
			gvTableContent.Columns.Add(columnbez_e)

			Dim columncreatedon As New DevExpress.XtraGrid.Columns.GridColumn()
			columncreatedon.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columncreatedon.Caption = m_Translate.GetSafeTranslationValue("Erstellt am")
			columncreatedon.Name = "createdon"
			columncreatedon.FieldName = "createdon"
			columncreatedon.Visible = False
			columncreatedon.BestFit()
			gvTableContent.Columns.Add(columncreatedon)

			Dim columncreatedfrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columncreatedfrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columncreatedfrom.Caption = m_Translate.GetSafeTranslationValue("Erstellt durch")
			columncreatedfrom.Name = "createdfrom"
			columncreatedfrom.FieldName = "createdfrom"
			columncreatedfrom.Visible = False
			columncreatedfrom.BestFit()
			gvTableContent.Columns.Add(columncreatedfrom)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdTableContent.DataSource = Nothing

	End Sub


	Private Sub ResetJobGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()

		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recId"
			columnrecid.FieldName = "recId"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columncode As New DevExpress.XtraGrid.Columns.GridColumn()
			columncode.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columncode.Caption = m_Translate.GetSafeTranslationValue("code")
			columncode.Name = "code"
			columncode.FieldName = "code"
			columncode.Visible = False
			columncode.Width = 50
			gvTableContent.Columns.Add(columncode)

			Dim columnberuf As New DevExpress.XtraGrid.Columns.GridColumn()
			columnberuf.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnberuf.Caption = m_Translate.GetSafeTranslationValue("Beruf")
			columnberuf.Name = "beruf"
			columnberuf.FieldName = "beruf"
			columnberuf.Visible = True
			columnberuf.Width = 100
			gvTableContent.Columns.Add(columnberuf)

			Dim columnberuf_d_m As New DevExpress.XtraGrid.Columns.GridColumn()
			columnberuf_d_m.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnberuf_d_m.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE_M)")
			columnberuf_d_m.Name = "beruf_d_m"
			columnberuf_d_m.FieldName = "beruf_d_m"
			columnberuf_d_m.Visible = True
			columnberuf_d_m.BestFit()
			gvTableContent.Columns.Add(columnberuf_d_m)

			Dim columnberuf_d_w As New DevExpress.XtraGrid.Columns.GridColumn()
			columnberuf_d_w.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnberuf_d_w.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE_W)")
			columnberuf_d_w.Name = "beruf_d_w"
			columnberuf_d_w.FieldName = "beruf_d_w"
			columnberuf_d_w.Visible = True
			columnberuf_d_w.BestFit()
			gvTableContent.Columns.Add(columnberuf_d_w)

			Dim columnberuf_i_m As New DevExpress.XtraGrid.Columns.GridColumn()
			columnberuf_i_m.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnberuf_i_m.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT_M)")
			columnberuf_i_m.Name = "beruf_i_m"
			columnberuf_i_m.FieldName = "beruf_i_m"
			columnberuf_i_m.Visible = True
			columnberuf_i_m.BestFit()
			gvTableContent.Columns.Add(columnberuf_i_m)

			Dim columnberuf_i_w As New DevExpress.XtraGrid.Columns.GridColumn()
			columnberuf_i_w.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnberuf_i_w.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT_W)")
			columnberuf_i_w.Name = "beruf_i_w"
			columnberuf_i_w.FieldName = "beruf_i_w"
			columnberuf_i_w.Visible = True
			columnberuf_i_w.BestFit()
			gvTableContent.Columns.Add(columnberuf_i_w)

			Dim columnberuf_f_m As New DevExpress.XtraGrid.Columns.GridColumn()
			columnberuf_f_m.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnberuf_f_m.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR_M)")
			columnberuf_f_m.Name = "beruf_f_m"
			columnberuf_f_m.FieldName = "beruf_f_m"
			columnberuf_f_m.Visible = True
			columnberuf_f_m.BestFit()
			gvTableContent.Columns.Add(columnberuf_f_m)

			Dim columnberuf_f_w As New DevExpress.XtraGrid.Columns.GridColumn()
			columnberuf_f_w.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnberuf_f_w.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR_W)")
			columnberuf_f_w.Name = "beruf_f_w"
			columnberuf_f_w.FieldName = "beruf_f_w"
			columnberuf_f_w.Visible = True
			columnberuf_f_w.BestFit()
			gvTableContent.Columns.Add(columnberuf_f_w)


			Dim columnberuf_e_m As New DevExpress.XtraGrid.Columns.GridColumn()
			columnberuf_e_m.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnberuf_e_m.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (EN_M)")
			columnberuf_e_m.Name = "beruf_e_m"
			columnberuf_e_m.FieldName = "beruf_e_m"
			columnberuf_e_m.Visible = True
			columnberuf_e_m.BestFit()
			gvTableContent.Columns.Add(columnberuf_f_m)

			Dim columnberuf_e_w As New DevExpress.XtraGrid.Columns.GridColumn()
			columnberuf_e_w.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnberuf_e_w.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (EN_W)")
			columnberuf_e_w.Name = "beruf_e_w"
			columnberuf_e_w.FieldName = "beruf_e_w"
			columnberuf_e_w.Visible = True
			columnberuf_e_w.BestFit()
			gvTableContent.Columns.Add(columnberuf_e_w)


			Dim columnfach_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnfach_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnfach_d.Caption = m_Translate.GetSafeTranslationValue("Fach (DE)")
			columnfach_d.Name = "fach_d"
			columnfach_d.FieldName = "fach_d"
			columnfach_d.Visible = True
			columnfach_d.BestFit()
			gvTableContent.Columns.Add(columnfach_d)

			Dim columnfach_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnfach_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnfach_i.Caption = m_Translate.GetSafeTranslationValue("Fach (IT)")
			columnfach_i.Name = "fach_i"
			columnfach_i.FieldName = "fach_i"
			columnfach_i.Visible = True
			columnfach_i.BestFit()
			gvTableContent.Columns.Add(columnfach_i)

			Dim columnfach_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnfach_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnfach_f.Caption = m_Translate.GetSafeTranslationValue("Fach (FR)")
			columnfach_f.Name = "fach_f"
			columnfach_f.FieldName = "fach_f"
			columnfach_f.Visible = True
			columnfach_f.BestFit()
			gvTableContent.Columns.Add(columnfach_f)


			Dim columncreatedon As New DevExpress.XtraGrid.Columns.GridColumn()
			columncreatedon.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columncreatedon.Caption = m_Translate.GetSafeTranslationValue("Erstellt am")
			columncreatedon.Name = "createdon"
			columncreatedon.FieldName = "createdon"
			columncreatedon.Visible = False
			columncreatedon.BestFit()
			gvTableContent.Columns.Add(columncreatedon)

			Dim columncreatedfrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columncreatedfrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columncreatedfrom.Caption = m_Translate.GetSafeTranslationValue("Erstellt durch")
			columncreatedfrom.Name = "createdfrom"
			columncreatedfrom.FieldName = "createdfrom"
			columncreatedfrom.Visible = False
			columncreatedfrom.BestFit()
			gvTableContent.Columns.Add(columncreatedfrom)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdTableContent.DataSource = Nothing

	End Sub

	Private Sub ResetCountryGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recid"
			columnrecid.FieldName = "recid"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columncode As New DevExpress.XtraGrid.Columns.GridColumn()
			columncode.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columncode.Caption = m_Translate.GetSafeTranslationValue("Code")
			columncode.Name = "code"
			columncode.FieldName = "code"
			columncode.Visible = True
			columncode.MaxWidth = 50
			gvTableContent.Columns.Add(columncode)

			Dim columnbez_value As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_value.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_value.Caption = m_Translate.GetSafeTranslationValue("Land")
			columnbez_value.Name = "bez_value"
			columnbez_value.FieldName = "bez_value"
			columnbez_value.Visible = True
			columnbez_value.Width = 100
			gvTableContent.Columns.Add(columnbez_value)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetFibuKontenGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()

		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recid"
			columnrecid.FieldName = "recid"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnKontoNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnKontoNr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnKontoNr.Caption = m_Translate.GetSafeTranslationValue("KontoNr")
			columnKontoNr.Name = "KontoNr"
			columnKontoNr.FieldName = "KontoNr"
			columnKontoNr.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnKontoNr.AppearanceHeader.Options.UseTextOptions = True
			columnKontoNr.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnKontoNr.AppearanceCell.Options.UseTextOptions = True
			columnKontoNr.Visible = True
			columnKontoNr.MaxWidth = 100
			gvTableContent.Columns.Add(columnKontoNr)

			Dim columnKontoName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnKontoName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnKontoName.Caption = m_Translate.GetSafeTranslationValue("KontoName")
			columnKontoName.Name = "KontoName"
			columnKontoName.FieldName = "KontoName"
			columnKontoName.Visible = True
			columnKontoName.BestFit()
			gvTableContent.Columns.Add(columnKontoName)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetCurrencyGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()

		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recId"
			columnrecid.FieldName = "recId"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columncode As New DevExpress.XtraGrid.Columns.GridColumn()
			columncode.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columncode.Caption = m_Translate.GetSafeTranslationValue("Code")
			columncode.Name = "code"
			columncode.FieldName = "code"
			columncode.Visible = True
			columncode.BestFit()
			gvTableContent.Columns.Add(columncode)

			Dim columndescription As New DevExpress.XtraGrid.Columns.GridColumn()
			columndescription.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columndescription.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columndescription.Name = "description"
			columndescription.FieldName = "description"
			columndescription.Visible = True
			columndescription.BestFit()
			gvTableContent.Columns.Add(columndescription)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdTableContent.DataSource = Nothing

	End Sub

	Private Sub ResetTermsAndConditionsGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()

		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recid"
			columnrecid.FieldName = "recid"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnbez_value As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_value.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_value.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnbez_value.Name = "bez_value"
			columnbez_value.FieldName = "bez_value"
			columnbez_value.Visible = True
			columnbez_value.BestFit()
			gvTableContent.Columns.Add(columnbez_value)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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

	Private Sub ResetAbsenceGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()

		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recid"
			columnrecid.FieldName = "recid"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnsalutation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnsalutation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnsalutation.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnsalutation.Name = "bez_value"
			columnsalutation.FieldName = "bez_value"
			columnsalutation.Visible = True
			columnsalutation.MaxWidth = 50
			gvTableContent.Columns.Add(columnsalutation)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
			columnbez_d.Name = "bez_d"
			columnbez_d.FieldName = "bez_d"
			columnbez_d.Visible = True
			columnbez_d.BestFit()
			gvTableContent.Columns.Add(columnbez_d)

			Dim columnbez_i As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_i.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_i.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
			columnbez_i.Name = "bez_i"
			columnbez_i.FieldName = "bez_i"
			columnbez_i.Visible = True
			columnbez_i.BestFit()
			gvTableContent.Columns.Add(columnbez_i)

			Dim columnbez_f As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_f.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnbez_f.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
			columnbez_f.Name = "bez_f"
			columnbez_f.FieldName = "bez_f"
			columnbez_f.Visible = True
			columnbez_f.BestFit()
			gvTableContent.Columns.Add(columnbez_f)

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


	Private Sub ResetPrintTemplatesGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("recid")
			columnrecid.Name = "recid"
			columnrecid.FieldName = "recid"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnrecnr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecnr.Caption = m_Translate.GetSafeTranslationValue("Reihennummer")
			columnrecnr.Name = "recnr"
			columnrecnr.FieldName = "recnr"
			columnrecnr.Width = 10
			columnrecnr.Visible = True
			gvTableContent.Columns.Add(columnrecnr)

			Dim columnsecnr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnsecnr.Caption = m_Translate.GetSafeTranslationValue("secnr")
			columnsecnr.Name = "secnr"
			columnsecnr.FieldName = "secnr"
			columnsecnr.Visible = False
			gvTableContent.Columns.Add(columnsecnr)

			Dim columndocnr As New DevExpress.XtraGrid.Columns.GridColumn()
			columndocnr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columndocnr.Caption = m_Translate.GetSafeTranslationValue("docnr")
			columndocnr.Name = "docnr"
			columndocnr.FieldName = "docnr"
			columndocnr.Visible = False
			gvTableContent.Columns.Add(columndocnr)

			Dim columndocfullname As New DevExpress.XtraGrid.Columns.GridColumn()
			columndocfullname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columndocfullname.Caption = m_Translate.GetSafeTranslationValue("Dokumentenname")
			columndocfullname.Name = "docfullname"
			columndocfullname.FieldName = "docfullname"
			columndocfullname.Visible = True
			gvTableContent.Columns.Add(columndocfullname)

			Dim columnmakroname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmakroname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnmakroname.Caption = m_Translate.GetSafeTranslationValue("Makroname")
			columnmakroname.Name = "makroname"
			columnmakroname.FieldName = "makroname"
			columnmakroname.Width = 50
			columnmakroname.Visible = True
			gvTableContent.Columns.Add(columnmakroname)

			Dim columnmenulabel As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmenulabel.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnmenulabel.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnmenulabel.Name = "menulabel"
			columnmenulabel.FieldName = "menulabel"
			columnmenulabel.Width = 100
			columnmenulabel.Visible = True
			gvTableContent.Columns.Add(columnmenulabel)

			Dim columnitemshowin As New DevExpress.XtraGrid.Columns.GridColumn()
			columnitemshowin.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnitemshowin.Caption = m_Translate.GetSafeTranslationValue("itemshowin")
			columnitemshowin.Name = "itemshowin"
			columnitemshowin.FieldName = "itemshowin"
			columnitemshowin.Visible = True
			gvTableContent.Columns.Add(columnitemshowin)

			Dim columncreatedon As New DevExpress.XtraGrid.Columns.GridColumn()
			columncreatedon.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columncreatedon.Caption = m_Translate.GetSafeTranslationValue("createdon")
			columncreatedon.Name = "createdon"
			columncreatedon.FieldName = "createdon"
			columncreatedon.Visible = False
			gvTableContent.Columns.Add(columncreatedon)

			Dim columncreatedfrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columncreatedfrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columncreatedfrom.Caption = m_Translate.GetSafeTranslationValue("createdfrom")
			columncreatedfrom.Name = "createdfrom"
			columncreatedfrom.FieldName = "createdfrom"
			columncreatedfrom.Visible = False
			gvTableContent.Columns.Add(columncreatedfrom)

			Dim columnchangedon As New DevExpress.XtraGrid.Columns.GridColumn()
			columnchangedon.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnchangedon.Caption = m_Translate.GetSafeTranslationValue("changedon")
			columnchangedon.Name = "changedon"
			columnchangedon.FieldName = "changedon"
			columnchangedon.Visible = False
			gvTableContent.Columns.Add(columnchangedon)

			Dim columnchangedfrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnchangedfrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnchangedfrom.Caption = m_Translate.GetSafeTranslationValue("changedfrom")
			columnchangedfrom.Name = "changedfrom"
			columnchangedfrom.FieldName = "changedfrom"
			columnchangedfrom.Visible = False
			gvTableContent.Columns.Add(columnchangedfrom)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdTableContent.DataSource = Nothing

	End Sub

	Private Sub ResetExportTemplatesGrid()

		gvTableContent.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTableContent.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvTableContent.OptionsView.ShowGroupPanel = False
		gvTableContent.OptionsView.ShowIndicator = False
		gvTableContent.OptionsView.ShowAutoFilterRow = True

		gvTableContent.Columns.Clear()


		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("recid")
			columnrecid.Name = "recid"
			columnrecid.FieldName = "recid"
			columnrecid.Visible = False
			gvTableContent.Columns.Add(columnrecid)

			Dim columnrecnr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecnr.Caption = m_Translate.GetSafeTranslationValue("Reihennummer")
			columnrecnr.Name = "recnr"
			columnrecnr.FieldName = "recnr"
			columnrecnr.Width = 50
			columnrecnr.Visible = True
			gvTableContent.Columns.Add(columnrecnr)

			Dim columnBezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBezeichnung.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnBezeichnung.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnBezeichnung.Name = "Bezeichnung"
			columnBezeichnung.FieldName = "Bezeichnung"
			columnBezeichnung.Width = 250
			columnBezeichnung.Visible = True
			gvTableContent.Columns.Add(columnBezeichnung)

			Dim columnDocName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDocName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDocName.Caption = m_Translate.GetSafeTranslationValue("Dokument")
			columnDocName.Name = "DocName"
			columnDocName.FieldName = "DocName"
			columnDocName.Visible = True
			gvTableContent.Columns.Add(columnDocName)

			Dim columnModulName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnModulName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnModulName.Caption = m_Translate.GetSafeTranslationValue("ModulName")
			columnModulName.Name = "ModulName"
			columnModulName.FieldName = "ModulName"
			columnModulName.Width = 250
			columnModulName.Visible = True
			gvTableContent.Columns.Add(columnModulName)

			Dim columnTranslatedModul As New DevExpress.XtraGrid.Columns.GridColumn()
			columnTranslatedModul.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnTranslatedModul.Caption = m_Translate.GetSafeTranslationValue("Programm-Modul")
			columnTranslatedModul.Name = "TranslatedModul"
			columnTranslatedModul.FieldName = "TranslatedModul"
			columnTranslatedModul.Width = 200
			columnTranslatedModul.Visible = True
			gvTableContent.Columns.Add(columnTranslatedModul)

			Dim columnTooltip As New DevExpress.XtraGrid.Columns.GridColumn()
			columnTooltip.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnTooltip.Caption = m_Translate.GetSafeTranslationValue("Tooltip")
			columnTooltip.Name = "Tooltip"
			columnTooltip.FieldName = "Tooltip"
			columnTooltip.Width = 200
			columnTooltip.Visible = False
			gvTableContent.Columns.Add(columnTooltip)

			Dim columnMnuName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMnuName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnMnuName.Caption = m_Translate.GetSafeTranslationValue("Export-Format")
			columnMnuName.Name = "MnuName"
			columnMnuName.FieldName = "MnuName"
			columnMnuName.Width = 100
			columnMnuName.Visible = True
			gvTableContent.Columns.Add(columnMnuName)



		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdTableContent.DataSource = Nothing

	End Sub



#End Region




	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvTableContent.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then

				Select Case m_Selectedmodul
					'Case 9, 10
					'	HandleRowClickForDocument(column.Name, dataRow)


					Case Else
						Exit Sub

				End Select

			End If

		End If

	End Sub

	Sub HandleRowClickForDocument(ByVal column As String, ByVal dataRow As Object)

		If Not dataRow Is Nothing Then
			Dim viewData = CType(dataRow, EmployeeContactData)

			'If viewData.recid > 0 Then OpenSelectedDocument(viewData.recid)

		End If

	End Sub


#End Region



#Region "Form setting"

	Private Sub Onfrm_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
		SaveFromSettings()
	End Sub

	''' <summary>
	''' Loads form settings if form gets visible.
	''' </summary>
	Private Sub OnFrm_VisibleChanged(sender As System.Object, e As System.EventArgs) Handles MyBase.VisibleChanged

		If Visible Then
			LoadFormSettings()
		End If

	End Sub

	''' <summary>
	''' Loads form settings.
	''' </summary>
	Private Sub LoadFormSettings()

		Try
			Dim setting_form_height = My.Settings.ifrmHeightTables
			Dim setting_form_width = My.Settings.ifrmWidthTables
			Dim setting_form_location = My.Settings.frmLocationTables

			If setting_form_height > 0 Then Me.Height = Math.Max(Me.Height, setting_form_height)
			If setting_form_width > 0 Then Me.Width = Math.Max(Me.Width, setting_form_width)

			If Not String.IsNullOrEmpty(setting_form_location) Then
				Dim aLoc As String() = setting_form_location.Split(CChar(";"))
				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Val(aLoc(0)), Val(aLoc(1)))
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())

		End Try

	End Sub

	''' <summary>
	''' Saves the form settings.
	''' </summary>
	Private Sub SaveFromSettings()

		' Save form location, width and height in setttings
		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.frmLocationTable = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.ifrmWidthTable = Me.Width
				My.Settings.ifrmHeightTable = Me.Height

				My.Settings.Save()

			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())

		End Try

	End Sub


#End Region


	''' <summary>
	''' Load Mandanten drop down
	''' </summary>
	''' <remarks></remarks>
	Private Sub LoadMandantenDropDown()
		Dim m_CommonDatabaseAccess = New SP.DatabaseAccess.Common.CommonDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

		Dim Data = m_CommonDatabaseAccess.LoadCompaniesListData

		lueMandant.Properties.DataSource = Data
		lueMandant.Properties.ForceInitialize()

	End Sub

	Private Function LoadYearDropDownData() As Boolean
		Dim success As Boolean = True
		Dim mdNumber As Integer = m_InitializationData.MDData.MDNr
		If Not lueMandant.EditValue Is Nothing Then mdNumber = lueMandant.EditValue
		Dim yearData = m_CommonDatabaseAccess.LoadMandantYears(mdNumber)

		If (yearData Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Jahre (Mandanten) konnten nicht geladen werden."))
		End If

		Dim yearValues As List(Of YearValueView) = Nothing

		If Not yearData Is Nothing Then
			yearValues = New List(Of YearValueView)

			For Each yearValue In yearData
				yearValues.Add(New YearValueView With {.Value = yearValue})
			Next

		End If

		lueYear.Properties.DataSource = yearValues
		If lueYear.EditValue Is Nothing OrElse Not yearValues.Any(Function(data) data.Value = lueYear.EditValue) Then
			lueYear.EditValue = Now.Year
		Else
			lueYear.EditValue = lueYear.EditValue
		End If

		lueYear.Properties.ForceInitialize()

		Return yearData IsNot Nothing

	End Function


	Public Function LoadEmployeeContactList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadEmployeeContactData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kontakt-Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New EmployeeContactData With
											   {.recid = report.recid,
												  .bez_value = report.bez_value,
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
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadEmployeeFStateList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadEmployeeStateData1()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("1. Status-Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New SP.DatabaseAccess.TableSetting.DataObjects.EmployeeStateData With
											   {.recid = report.recid,
												  .bez_value = report.bez_value,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_i,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of SP.DatabaseAccess.TableSetting.DataObjects.EmployeeStateData) = New BindingList(Of SP.DatabaseAccess.TableSetting.DataObjects.EmployeeStateData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadEmployeeSStateList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadEmployeeStateData2()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("2. Status-Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New SP.DatabaseAccess.TableSetting.DataObjects.EmployeeStateData With
											   {.recid = report.recid,
												  .bez_value = report.bez_value,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_i,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of SP.DatabaseAccess.TableSetting.DataObjects.EmployeeStateData) = New BindingList(Of SP.DatabaseAccess.TableSetting.DataObjects.EmployeeStateData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadEmployeeCivilstateList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadEmployeeCivilstateData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Zivilstatnd-Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New Common.DataObjects.CivilStateData With
											   {.recid = report.recid,
												  .GetField = report.GetField,
												  .Description = report.Description,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_i,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of Common.DataObjects.CivilStateData) = New BindingList(Of Common.DataObjects.CivilStateData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadEmployeeLanguageLetterList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadJobLanguageData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Sprach-Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New JobLanguageData With
											   {.recid = report.recid,
												  .bez_value = report.bez_value,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_i,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of JobLanguageData) = New BindingList(Of JobLanguageData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadEmployeeAssessmentList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadAssessmentData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Beurteilungs-Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New AssessmentData With
											   {.recid = report.recid,
												  .bez_value = report.bez_value,
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
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadEmployeeCommunicationTypeList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadCommunicationTypeData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kommunikationsarten-Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New CommunicationTypeData With
											   {.recid = report.recid,
												  .bez_value = report.bez_value,
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
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadEmployeeCarReserveList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadCarReserveData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten für Auto-Reserve konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New CarReserveData With
											   {.recid = report.recid,
												  .bez_value = report.bez_value,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_i,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of CarReserveData) = New BindingList(Of CarReserveData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadEmployeeDrivingLicenceList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadDrivingLicenceData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten für Führerscheine konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New DrivingLicenceData With
											   {.recid = report.recid,
												  .bez_value = report.bez_value,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_i,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of DrivingLicenceData) = New BindingList(Of DrivingLicenceData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadEmployeeVehicleList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadVehicleData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten für Fahrzeuge konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New VehicleData With
											   {.recid = report.recid,
												  .bez_value = report.bez_value,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_i,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of VehicleData) = New BindingList(Of VehicleData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadEmployeeContactReserveList(ByVal ResType As ContactReserveType) As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadContactReserveData(ResType)

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten für Reserve konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New ContactReserveData With
											   {.recid = report.recid,
												  .bez_value = report.bez_value,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_i,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of ContactReserveData) = New BindingList(Of ContactReserveData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function


	Public Function LoadEmployeeDeadlineList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadDeadLineData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten für Kündigungsfristen konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New DeadlineData With
											   {.recid = report.recid,
												  .bez_value = report.bez_value,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_i,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of DeadlineData) = New BindingList(Of DeadlineData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadEmployeeWorkPensumList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadWorkPensumData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten für Arbeitspensum konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New WorkPensumData With
											   {.recid = report.recid,
												  .bez_value = report.bez_value
											   }).ToList()

			Dim listDataSource As BindingList(Of WorkPensumData) = New BindingList(Of WorkPensumData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadEmployeeEmployementTypeList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadEmployementTypeData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten für Anstellungsarten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New EmployeeEmployementTypeData With
											   {.recid = report.recid,
												  .bez_value = report.bez_value,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_i,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of EmployeeEmployementTypeData) = New BindingList(Of EmployeeEmployementTypeData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadEmployeeDocumentCategoryList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadEmployeeDocumentCategoryData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Dokument-Kategorien konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New Employee.DataObjects.DocumentMng.EmployeeDocumentCategoryData With
											   {.ID = report.ID,
												  .CategoryNumber = report.CategoryNumber,
												  .DescriptionEnglish = report.DescriptionEnglish,
												  .DescriptionFrench = report.DescriptionFrench,
												  .DescriptionGerman = report.DescriptionGerman,
												  .DescriptionItalian = report.DescriptionItalian
											   }).ToList()

			Dim listDataSource As BindingList(Of Employee.DataObjects.DocumentMng.EmployeeDocumentCategoryData) = New BindingList(Of Employee.DataObjects.DocumentMng.EmployeeDocumentCategoryData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadEmployeeInterviewStateList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadInterviewStateData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten für Status der Vorstellungsgespräche konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New EmployeeInteriviewStateData With
											   {.recid = report.recid,
												  .bez_value = report.bez_value,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_i,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of EmployeeInteriviewStateData) = New BindingList(Of EmployeeInteriviewStateData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function








	Public Function LoadCustomerPropertyList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadCustomerPropertyData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Eigenschaft-Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New CustomerPropertyData With
											   {.recid = report.recid,
												  .bez_value = report.bez_value,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_i,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of CustomerPropertyData) = New BindingList(Of CustomerPropertyData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadCustomerContactList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadCustomerContactData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kontakt-Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New CustomerContactData With
											   {.recid = report.recid,
												  .bez_value = report.bez_value,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_i,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of CustomerContactData) = New BindingList(Of CustomerContactData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadCustomerFStateList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadCustomerStateData1()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("1. Status-Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New CustomerStateData With
											   {.recid = report.recid,
												  .bez_value = report.bez_value,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_i,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of CustomerStateData) = New BindingList(Of CustomerStateData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadCustomerSStateList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadCustomerStateData2()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("2. Status-Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New CustomerStateData With
											   {.recid = report.recid,
												  .bez_value = report.bez_value,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_i,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of CustomerStateData) = New BindingList(Of CustomerStateData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadCustomerStichwortList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadCustomerStichwortData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Stichwörter konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New CustomerStichwortData With
											   {.recid = report.recid,
												  .bez_value = report.bez_value,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_i,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of CustomerStichwortData) = New BindingList(Of CustomerStichwortData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadCustomerEmployementTypeList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadCustomerEmployementTypeData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Anstellungsarten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New CustomerEmployementTypeData With
											   {.recid = report.recid,
												  .bez_value = report.bez_value,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_i,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of CustomerEmployementTypeData) = New BindingList(Of CustomerEmployementTypeData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadCustomerPaymentReminderCodeList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadCustomerPaymentReminderCodeData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mahncodes konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New Customer.DataObjects.PaymentReminderCodeData With
											   {.ID = report.ID,
												  .GetField = report.GetField,
												  .Reminder1 = report.Reminder1,
												  .Reminder2 = report.Reminder2,
												  .Reminder3 = report.Reminder3,
												  .Reminder4 = report.Reminder4
											   }).ToList()

			Dim listDataSource As BindingList(Of Customer.DataObjects.PaymentReminderCodeData) = New BindingList(Of Customer.DataObjects.PaymentReminderCodeData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadCustomerPaymentConditionList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadCustomerPaymentConditionData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Zahlungskonditionen konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New Customer.DataObjects.PaymentConditionData With
											   {.ID = report.ID,
												  .GetField = report.GetField,
												  .Description = report.Description,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_e,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of Customer.DataObjects.PaymentConditionData) = New BindingList(Of Customer.DataObjects.PaymentConditionData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadCustomerInvoiceOptionsList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadCustomerInvoiceOptionData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Faktura-Optionen konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New Customer.DataObjects.InvoiceOptionData With
											   {.ID = report.ID,
												  .Description = report.Description,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_e,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of Customer.DataObjects.InvoiceOptionData) = New BindingList(Of Customer.DataObjects.InvoiceOptionData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadCustomerInvoiceTypeList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadCustomerInvoiceTypeData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Fakturaarten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New Customer.DataObjects.InvoiceTypeData With
											   {.ID = report.ID,
												  .Code = report.Code,
												  .Description = report.Description,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_e,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of Customer.DataObjects.InvoiceTypeData) = New BindingList(Of Customer.DataObjects.InvoiceTypeData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadCustomerInvoiceShipmentList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadCustomerInvoiceShipment()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New Customer.DataObjects.OPShipmentData With
											   {.ID = report.ID,
												  .Description = report.Description,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_e,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of Customer.DataObjects.OPShipmentData) = New BindingList(Of Customer.DataObjects.OPShipmentData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function


	Public Function LoadCustomerNumberOfEmployeesList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadCustomerNumberOfEmployeesData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Betriebsgrössen konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New Customer.DataObjects.NumberOfEmployeesData With
											   {.ID = report.ID,
												  .NumberOfEmployees = report.NumberOfEmployees
											   }).ToList()

			Dim listDataSource As BindingList(Of Customer.DataObjects.NumberOfEmployeesData) = New BindingList(Of Customer.DataObjects.NumberOfEmployeesData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function


	Public Function LoadCustomerContactReserveList(ByVal ResType As ContactReserveType) As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadCustomerContactReserveData(ResType)

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten für Reserve konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New Customer.DataObjects.CustomerReserveData With
											   {.ID = report.ID,
												  .Description = report.Description,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_i,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of Customer.DataObjects.CustomerReserveData) = New BindingList(Of Customer.DataObjects.CustomerReserveData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadCustomerDocumentCategoryList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadCustomerDocumentCategoryData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Dokument-Kategorien konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New Customer.DataObjects.CustomerDocumentCategoryData With
											   {.ID = report.ID,
												  .CategoryNumber = report.CategoryNumber,
												  .DescriptionEnglish = report.DescriptionEnglish,
												  .DescriptionFrench = report.DescriptionFrench,
												  .DescriptionGerman = report.DescriptionGerman,
												  .DescriptionItalian = report.DescriptionItalian
											   }).ToList()

			Dim listDataSource As BindingList(Of Customer.DataObjects.CustomerDocumentCategoryData) = New BindingList(Of Customer.DataObjects.CustomerDocumentCategoryData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function




	''' <summary>
	''' responsible person
	''' </summary>
	Public Function LoadCResponsibleContactList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadResponsiblepersonContactData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kontakt-Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New Customer.DataObjects.ResponsiblePersonContactInfo With
											   {.ID = report.ID,
												  .Description = report.Description,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_i,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of Customer.DataObjects.ResponsiblePersonContactInfo) = New BindingList(Of Customer.DataObjects.ResponsiblePersonContactInfo)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadCResponsibleFStateList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadResponsiblepersonStateData1()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("1. Status-Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New Customer.DataObjects.ResponsiblePersonStateData With
											   {.ID = report.ID,
												  .Description = report.Description,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_i,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of Customer.DataObjects.ResponsiblePersonStateData) = New BindingList(Of Customer.DataObjects.ResponsiblePersonStateData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadCResponsibleSStateList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadResponsiblepersonStateData2()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("2. Status-Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New Customer.DataObjects.ResponsiblePersonStateData With
											   {.ID = report.ID,
												  .Description = report.Description,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_i,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of Customer.DataObjects.ResponsiblePersonStateData) = New BindingList(Of Customer.DataObjects.ResponsiblePersonStateData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadCResponsibleDepartmentList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadResponsiblepersonDepartment()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Abteilung-Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New Customer.DataObjects.DepartmentData With
											   {.ID = report.ID,
												  .Description = report.Description,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_i,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of Customer.DataObjects.DepartmentData) = New BindingList(Of Customer.DataObjects.DepartmentData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadCResponsiblePositionList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadResponsiblepersonPosition()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Positions-Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New Customer.DataObjects.PositionData With
											   {.ID = report.ID,
												  .Description = report.Description,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_i,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of Customer.DataObjects.PositionData) = New BindingList(Of Customer.DataObjects.PositionData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadCResponsibleCommunicationList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadResponsiblepersonCommunication()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kommunikations-Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New Customer.DataObjects.CustomerCommunicationData With
											   {.ID = report.ID,
												  .Description = report.Description,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_i,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of Customer.DataObjects.CustomerCommunicationData) = New BindingList(Of Customer.DataObjects.CustomerCommunicationData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadCResponsibleCommunicationTypeList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadResponsiblepersonCommunicationType()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Versand-Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New Customer.DataObjects.CustomerCommunicationTypeData With
											   {.ID = report.ID,
												  .Description = report.Description,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_i,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of Customer.DataObjects.CustomerCommunicationTypeData) = New BindingList(Of Customer.DataObjects.CustomerCommunicationTypeData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function


	Public Function LoadCResponsibleContactReserveList(ByVal ResType As ContactReserveType) As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadCResponsibleContactReserveData(ResType)

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten für Reserve konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New Customer.DataObjects.ResponsiblePersonReserveData With
											   {.ID = report.ID,
												  .Description = report.Description,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_i,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of Customer.DataObjects.ResponsiblePersonReserveData) = New BindingList(Of Customer.DataObjects.ResponsiblePersonReserveData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function




	''' <summary>
	''' Vacancy
	''' </summary>
	Public Function LoadVacancyContactList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadVacancyContactData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kontakt-Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New VacancyContactData With
											   {.ID = report.ID,
												  .recvalue = report.recvalue,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_i,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of VacancyContactData) = New BindingList(Of VacancyContactData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadVacancyStateList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadVacancyStateData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Status-Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New VacancyStateData With
											   {.ID = report.ID,
												  .recvalue = report.recvalue,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_i,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of VacancyStateData) = New BindingList(Of VacancyStateData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadVacancyGroupList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadVacancyGroupData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Status-Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New VacancyGroupData With
											   {.ID = report.ID,
												  .Bez_Value = report.Bez_Value,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_i,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of VacancyGroupData) = New BindingList(Of VacancyGroupData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function






	''' <summary>
	''' Offer
	''' </summary>
	Public Function LoadOfferContactList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadOfferContactData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kontakt-Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New OfferContactData With
											   {.ID = report.ID,
												  .Description = report.Description,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_i,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of OfferContactData) = New BindingList(Of OfferContactData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadOfferStateList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadOfferStateData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Status-Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New OfferStateData With
											   {.ID = report.ID,
												  .Description = report.Description,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_i,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of OfferStateData) = New BindingList(Of OfferStateData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadOfferGroupList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadOfferGroupData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Status-Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New OfferGroupData With
											   {.ID = report.ID,
												  .Description = report.Description,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_i,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of OfferGroupData) = New BindingList(Of OfferGroupData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function





	''' <summary>
	''' Propose
	''' </summary>
	Private Function LoadProposeStateList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadProposeStateData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Vorschlag-Status-Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New ProposeStateData With
											   {.recid = report.recid,
												  .bez_value = report.bez_value,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_i,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of ProposeStateData) = New BindingList(Of ProposeStateData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Private Function LoadProposeEmployementTypeList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadProposeEmployementTypeData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Vorschlag-Anstellungsart-Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New ProposeEmployementTypeData With
											   {.recid = report.recid,
												  .bez_value = report.bez_value,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_i,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of ProposeEmployementTypeData) = New BindingList(Of ProposeEmployementTypeData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Private Function LoadProposeArtList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadProposeArtData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Vorschlag-Art-Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New ProposeArtData With
											   {.recid = report.recid,
												  .bez_value = report.bez_value,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_i,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of ProposeArtData) = New BindingList(Of ProposeArtData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function



	''' <summary>
	''' common tables
	''' </summary>
	Public Function LoadContactCategoryList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadContactCategoryData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kontakt-Kategorien konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New SP.DatabaseAccess.Common.DataObjects.ContactType1Data With
											   {.ID = report.ID,
												  .Bez_ID = report.Bez_ID,
												  .Caption_DE = report.Caption_DE,
												  .Caption_EN = report.Caption_EN,
												  .Caption_FR = report.Caption_FR,
												  .Caption_IT = report.Caption_IT,
												  .IconIndex = report.IconIndex,
												  .RecNr = report.RecNr,
												  .Result = report.Result
											   }).ToList()

			Dim listDataSource As BindingList(Of SP.DatabaseAccess.Common.DataObjects.ContactType1Data) = New BindingList(Of SP.DatabaseAccess.Common.DataObjects.ContactType1Data)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadEmployementCategorizedList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadEmployementCategorizedData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Einsatz-Einstufung konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New ES.DataObjects.ESMng.ESCategorizationData With
											   {.ID = report.ID,
												  .Description = report.Description,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_i,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of ES.DataObjects.ESMng.ESCategorizationData) = New BindingList(Of ES.DataObjects.ESMng.ESCategorizationData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadFCostcenterList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadCostCenter1()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("1. Kostenstellen konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New CostCenter1Data With
											   {.recId = report.recId,
												  .kstbezeichnung = report.kstbezeichnung,
												  .kstname = report.kstname
											   }).ToList()

			Dim listDataSource As BindingList(Of CostCenter1Data) = New BindingList(Of CostCenter1Data)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count

			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadSCostcenterList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadCostCenter2()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("2. Kostenstellen konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New CostCenter2Data With
											   {.recId = report.recId,
												  .kstbezeichnung = report.kstbezeichnung,
												  .kstname1 = report.kstname1,
												  .kstname = report.kstname
											   }).ToList()

			Dim listDataSource As BindingList(Of CostCenter2Data) = New BindingList(Of CostCenter2Data)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadBVGList(ByVal gender As String) As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Dim mdNumber As Integer = m_InitializationData.MDData.MDNr
		If Not lueMandant.EditValue Is Nothing Then mdNumber = lueMandant.EditValue

		Try
			LoadYearDropDownData()
			Dim data = m_TablesettingDatabaseAccess.LoadBVGData(mdNumber, gender, lueYear.EditValue)

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("BVG-Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New BVGData With
											   {.recId = report.recId,
												  .Alter = report.Alter,
												  .MDNr = report.MDNr,
												  .ProzentSatz = report.ProzentSatz,
												  .ProzJahr = report.ProzJahr
											   }).ToList()

			Dim listDataSource As BindingList(Of BVGData) = New BindingList(Of BVGData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadFF13LohnList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadFF13Lohn()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ferien, Feiertag und 13. Lohn Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New FF13LohnData With
											   {.recId = report.recId,
												  .FeierProzentSatz = report.FeierProzentSatz,
												  .FerProzentSatz = report.FerProzentSatz,
												  .Jahrgang = report.Jahrgang,
												  .Prozent13Satz = report.Prozent13Satz
											   }).ToList()

			Dim listDataSource As BindingList(Of FF13LohnData) = New BindingList(Of FF13LohnData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadQSTInfoList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadQSTInfo()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Quellensteuerinformationen konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New QstInfoData With
   {.recId = report.recId,
	  .CalendarDay = report.CalendarDay,
	  .DESameAsCH = report.DESameAsCH,
	  .MonthStd = report.MonthStd,
	  .SKanton = report.SKanton,
	  .StdDown = report.StdDown,
	  .StdDownAtEndBegin = report.StdDownAtEndBegin,
	  .StdUp = report.StdUp,
	  .JustAtEndBegin = report.JustAtEndBegin,
	  .WithFLeistung = report.WithFLeistung,
	  .HandleAsAutomation = report.HandleAsAutomation
   }).ToList()

			Dim listDataSource As BindingList(Of QstInfoData) = New BindingList(Of QstInfoData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadSectorList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadSectorData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Branchen konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New SectorData With
											   {.recid = report.recid,
												  .bez_d = report.bez_d,
												  .bez_e = report.bez_e,
												  .bez_f = report.bez_f,
												  .bez_i = report.bez_i,
												  .branche = report.branche,
												  .code = report.code,
												  .createdfrom = report.createdfrom,
												  .createdon = report.createdon
											   }).ToList()

			Dim listDataSource As BindingList(Of SectorData) = New BindingList(Of SectorData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadJobList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadJobData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Qualifikationen konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New JobData With
											   {.recid = report.recid,
												  .beruf = report.beruf,
												  .beruf_d_m = report.beruf_d_m,
												  .beruf_d_w = report.beruf_d_w,
												  .beruf_e_m = report.beruf_e_m,
												  .beruf_e_w = report.beruf_e_w,
												  .beruf_f_m = report.beruf_f_m,
												  .beruf_f_w = report.beruf_f_w,
												  .beruf_i_m = report.beruf_i_m,
												  .beruf_i_w = report.beruf_i_w,
												  .code = report.code,
												  .fach_d = report.fach_d,
												  .fach_f = report.fach_f,
												  .fach_i = report.fach_i
											   }).ToList()

			Dim listDataSource As BindingList(Of JobData) = New BindingList(Of JobData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadCountryList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadCountryData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Länder-Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New CountryData With
											   {.recid = report.recid,
												  .bez_d = report.bez_d,
												  .bez_e = report.bez_e,
												  .bez_f = report.bez_f,
												  .bez_i = report.bez_i,
												  .bez_value = report.bez_value,
												  .code = report.code
											   }).ToList()

			Dim listDataSource As BindingList(Of CountryData) = New BindingList(Of CountryData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadBusinessBranchsList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadBusinessBranchsData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Filial-Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New AvilableBusinessBranchData With
											   {.recid = report.recid,
												  .bez_value = report.bez_value,
												  .Code_1 = report.Code_1
											   }).ToList()

			Dim listDataSource As BindingList(Of AvilableBusinessBranchData) = New BindingList(Of AvilableBusinessBranchData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadFibukontenList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadFIBUKontenData(m_InitializationData.UserData.UserLanguage)

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("FIBU-Konten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New FIBUData With
											   {.recid = report.recid,
												  .bez_d = report.bez_d,
												  .bez_e = report.bez_e,
												  .bez_f = report.bez_f,
												  .bez_i = report.bez_i,
												  .KontoName = report.KontoName,
												  .KontoNr = report.KontoNr
											   }).ToList()

			Dim listDataSource As BindingList(Of FIBUData) = New BindingList(Of FIBUData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadAbsenceList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadAbsenceData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("FIBU-Konten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New AbsenceData With
											   {.recid = report.recid,
												  .bez_d = report.bez_d,
												  .bez_e = report.bez_e,
												  .bez_f = report.bez_f,
												  .bez_i = report.bez_i,
												  .Description = report.Description,
												  .bez_value = report.bez_value
											   }).ToList()

			Dim listDataSource As BindingList(Of AbsenceData) = New BindingList(Of AbsenceData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadTermsandConditionsList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadTermsAndConditionsData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("AGB-Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New TermsAndConditionsData With
											   {.recid = report.recid,
												  .bez_d = report.bez_d,
												  .bez_e = report.bez_e,
												  .bez_f = report.bez_f,
												  .bez_i = report.bez_i,
												  .bez_value = report.bez_value
											   }).ToList()

			Dim listDataSource As BindingList(Of TermsAndConditionsData) = New BindingList(Of TermsAndConditionsData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function



	Public Function LoadSalutationList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim m_SalutationData = m_TablesettingDatabaseAccess.LoadSalutationData()

			If (m_SalutationData Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Anrede-Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In m_SalutationData
								  Select New SalutationData With
											   {.recId = report.recId,
												  .salutation = report.salutation,
												  .salutation_d = report.salutation_d,
												  .salutation_i = report.salutation_i,
												  .salutation_f = report.salutation_f,
												  .salutation_e = report.salutation_e,
												  .letterform = report.letterform,
												  .letterform_d = report.letterform_d,
												  .letterform_i = report.letterform_i,
												  .letterform_f = report.letterform_f,
												  .letterform_e = report.letterform_e
											   }).ToList()

			Dim listDataSource As BindingList(Of SalutationData) = New BindingList(Of SalutationData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try


	End Function

	Public Function LoadSMSTemplatesList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim m_SMSTemplateData = m_TablesettingDatabaseAccess.LoadSMSTemplateData()

			If (m_SMSTemplateData Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Vorlagen-Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In m_SMSTemplateData
								  Select New SMSTemplateData With
											   {.bez_value = report.bez_value,
												  .recid = report.recid,
												  .bez_d = report.bez_d,
												  .bez_i = report.bez_i,
												  .bez_f = report.bez_f,
												  .bez_e = report.bez_e
											   }).ToList()

			Dim listDataSource As BindingList(Of SMSTemplateData) = New BindingList(Of SMSTemplateData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try


	End Function



	Public Function LoadPrintTemplatesList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadPrintTemplatesData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten für Druckvorlagen konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New PrintTemplatesData With
													   {.recid = report.recid,
													   .recnr = report.recnr,
													   .secnr = report.secnr,
													   .docnr = report.docnr,
													   .docfullname = report.docfullname,
													   .makroname = report.makroname,
													   .menulabel = report.menulabel,
													   .itemshowin = report.itemshowin,
													   .createdon = report.createdon,
													   .createdfrom = report.createdfrom,
													   .changedon = report.changedon,
													   .changedfrom = report.changedfrom
													   }).ToList()

			Dim listDataSource As BindingList(Of PrintTemplatesData) = New BindingList(Of PrintTemplatesData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function

	Public Function LoadExportTemplatesList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadExportTemplatesData()

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Export-Menüs konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New ExportTemplatesData With
													   {.recid = report.recid,
													   .recnr = report.recnr,
													   .Bezeichnung = report.Bezeichnung,
													   .DocName = report.DocName,
													   .ModulName = report.ModulName,
													   .MnuName = report.MnuName,
													   .Tooltip = report.Tooltip
													   }).ToList()

			Dim listDataSource As BindingList(Of ExportTemplatesData) = New BindingList(Of ExportTemplatesData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdTableContent.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count


			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

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


	Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
		Me.Close()
	End Sub

	Private Sub Onfrm_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.frmLocationTables = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.ifrmHeightTables = Me.Height
				My.Settings.ifrmWidthTables = Me.Width

				My.Settings.Save()
			End If

		Catch ex As Exception

		End Try

	End Sub

	Private Overloads Function Validate(ByVal rowobject As Object) As Boolean
		Dim success As Boolean = True

		Try
			Select Case m_Selectedmodul
				Case SelectedModulKey.MODUL_EMPLOYEE_CONTACT
					Dim SelectedData = SelectedEmployeeContactViewData


				Case SelectedModulKey.MODUL_EMPLOYEE_FSTATE
					Dim SelectedData = SelectedEmployeeFStateViewData()

				Case SelectedModulKey.MODUL_EMPLOYEE_SSTATE
					Dim SelectedData = SelectedEmployeeSStateViewData()

				Case SelectedModulKey.MODUL_EMPLOYEE_CIVILSTATE
					Dim SelectedData = SelectedEmployeeCivilstateViewData()

				Case SelectedModulKey.MODUL_EMPLOYEE_LANGUAGELETTER
					Dim SelectedData = SelectedEmployeeJobLanguageViewData()

				Case SelectedModulKey.MODUL_EMPLOYEE_ASSESSMENT
					Dim SelectedData = SelectedEmployeeAssesmentViewData()

				Case SelectedModulKey.MODUL_EMPLOYEE_COMMUNICATIONTYPE
					Dim SelectedData = SelectedEmployeeCommunicationTypeViewData()

				Case SelectedModulKey.MODUL_EMPLOYEE_CARRESERVE
					Dim SelectedData = SelectedEmployeeCarReserveViewData()

				Case SelectedModulKey.MODUL_EMPLOYEE_DRIVINGLICENCE
					Dim SelectedData = SelectedEmployeeDrivingLicenceViewData()

				Case SelectedModulKey.MODUL_EMPLOYEE_VEHICLE
					Dim SelectedData = SelectedEmployeeVehicleViewData()

				Case SelectedModulKey.MODUL_EMPLOYEE_CONTACTRES1
					Dim SelectedData = SelectedEmployeeContactReserveViewData(ContactReserveType.Reserve1)

				Case SelectedModulKey.MODUL_EMPLOYEE_CONTACTRES2
					Dim SelectedData = SelectedEmployeeContactReserveViewData(ContactReserveType.Reserve2)

				Case SelectedModulKey.MODUL_EMPLOYEE_CONTACTRES3
					Dim SelectedData = SelectedEmployeeContactReserveViewData(ContactReserveType.Reserve3)

				Case SelectedModulKey.MODUL_EMPLOYEE_CONTACTRES4
					Dim SelectedData = SelectedEmployeeContactReserveViewData(ContactReserveType.Reserve4)

				Case SelectedModulKey.MODUL_EMPLOYEE_DEADLINE
					Dim SelectedData = SelectedEmployeeDeadlineViewData()

				Case SelectedModulKey.MODUL_EMPLOYEE_WORKPENSUM
					Dim SelectedData = SelectedEmployeeWorkPensumViewData()

				Case SelectedModulKey.MODUL_EMPLOYEE_EMPLOYEMENTTYPE
					Dim SelectedData = SelectedEmployeeEmployementTypeViewData()

				Case SelectedModulKey.MODUL_EMPLOYEE_DOCUMENTCATEGORY
					Dim SelectedData = CType(rowobject, Employee.DataObjects.DocumentMng.EmployeeDocumentCategoryData)
					If Not SelectedData Is Nothing AndAlso IsNumberInRange(SelectedData.CategoryNumber.GetValueOrDefault(0), 200, 250) Then
						Return False
					End If

				Case SelectedModulKey.MODUL_EMPLOYEE_INTERVIEWSTATE
					Dim SelectedData = SelectedEmployeeInterviewStateViewData()



					' Customer
				Case SelectedModulKey.MODUL_CUSTOMER_PROPERTY
					Dim SelectedData = SelectedCustomerpropertyViewData()

				Case SelectedModulKey.MODUL_CUSTOMER_CONTACT
					Dim SelectedData = SelectedCustomerContactViewData()

				Case SelectedModulKey.MODUL_CUSTOMER_FSTATE
					Dim SelectedData = SelectedCustomerFStateViewData()


				Case SelectedModulKey.MODUL_CUSTOMER_SSTATE
					Dim SelectedData = SelectedCustomerSStateViewData()


				Case SelectedModulKey.MODUL_CUSTOMER_CONTACTRES1
					Dim SelectedData = SelectedCustomerContactReserveViewData(ContactReserveType.Reserve1)

				Case SelectedModulKey.MODUL_CUSTOMER_CONTACTRES2
					Dim SelectedData = SelectedCustomerContactReserveViewData(ContactReserveType.Reserve2)

				Case SelectedModulKey.MODUL_CUSTOMER_CONTACTRES3
					Dim SelectedData = SelectedCustomerContactReserveViewData(ContactReserveType.Reserve3)

				Case SelectedModulKey.MODUL_CUSTOMER_CONTACTRES4
					Dim SelectedData = SelectedCustomerContactReserveViewData(ContactReserveType.Reserve4)

				Case SelectedModulKey.MODUL_CUSTOMER_STICHWORT
					Dim SelectedData = SelectedCustomerStichwortViewData()

				Case SelectedModulKey.MODUL_CUSTOMER_EMPLOYEMENTTYPE
					Dim SelectedData = SelectedCustomerEmployementTypeViewData()

				Case SelectedModulKey.MODUL_CUSTOMER_PAYMENTREMINDERCODE
					Dim SelectedData = SelectedCustomerPaymentReminderCodeViewData()

				Case SelectedModulKey.MODUL_CUSTOMER_PAYMENTCONDITION
					Dim SelectedData = SelectedCustomerPaymentConditionViewData()

				Case SelectedModulKey.MODUL_CUSTOMER_INVOICEOPTIONS
					Dim SelectedData = SelectedCustomerInvoiceOptionsViewData()

				Case SelectedModulKey.MODUL_CUSTOMER_INVOICETYPE
					Dim SelectedData = SelectedCustomerInvoiceTypeViewData()

				Case SelectedModulKey.MODUL_CUSTOMER_INVOICESHIPMENT
					Dim SelectedData = SelectedCustomerInvoiceShipmentViewData()


				Case SelectedModulKey.MODUL_CUSTOMER_NUMBEROFEMPLOYEES
					Dim SelectedData = SelectedCustomerNumberOfEmployeesViewData()

				Case SelectedModulKey.MODUL_CUSTOMER_DOCUMENTCATEGORY
					Dim SelectedData = SelectedCustomerDocumentCategoryViewData()
					If Not SelectedData Is Nothing AndAlso IsNumberInRange(SelectedData.CategoryNumber.GetValueOrDefault(0), 100, 150) Then
						success = False
					End If


					' Responsible person
				Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_CONTACT
					Dim SelectedData = SelectedResponsiblePersonContactViewData()

				Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_FSTATE
					Dim SelectedData = SelectedResponsiblePersonFStateViewData()

				Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_SSTATE
					Dim SelectedData = SelectedResponsiblePersonSStateViewData()

				Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_DEPARTMENT
					Dim SelectedData = SelectedResponsiblePersonDepartmentViewData()

				Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_POSITION
					Dim SelectedData = SelectedResponsiblePersonPositionViewData()

				Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_COMMUNICATION
					Dim SelectedData = SelectedResponsiblePersonCommunicationViewData()

				Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_COMMUNICATIONTYPE
					Dim SelectedData = SelectedResponsiblePersonCommunicationTypeViewData()


				Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_CONTACTRES1
					Dim SelectedData = SelectedCResponsibleContactReserveViewData(ContactReserveType.Reserve1)

				Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_CONTACTRES2
					Dim SelectedData = SelectedCResponsibleContactReserveViewData(ContactReserveType.Reserve2)

				Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_CONTACTRES3
					Dim SelectedData = SelectedCResponsibleContactReserveViewData(ContactReserveType.Reserve3)

				Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_CONTACTRES4
					Dim SelectedData = SelectedCResponsibleContactReserveViewData(ContactReserveType.Reserve4)


					' Vacancy
				Case SelectedModulKey.MODUL_VACANCY_CONTACT
					Dim SelectedData = SelectedVacancyContactViewData()

				Case SelectedModulKey.MODUL_VACANCY_STATE
					Dim SelectedData = SelectedVacancyStateViewData()

				Case SelectedModulKey.MODUL_VACANCY_GROUP
					Dim SelectedData = SelectedVacancyGroupViewData()


					' Offer
				Case SelectedModulKey.MODUL_OFFER_CONTACT
					Dim SelectedData = SelectedOfferContactViewData()

				Case SelectedModulKey.MODUL_OFFER_STATE
					Dim SelectedData = SelectedOfferStateViewData()

				Case SelectedModulKey.MODUL_OFFER_GROUP
					Dim SelectedData = SelectedOfferGroupViewData()



					' propose
				Case SelectedModulKey.MODUL_PROPOSE_STATE
					Dim SelectedData = SelectedProposeStateViewData()

				Case SelectedModulKey.MODUL_PROPOSE_EMPLOYEMENTTYPE
					Dim SelectedData = SelectedProposeEmployementTypeViewData()

				Case SelectedModulKey.MODUL_PROPOSE_ART
					Dim SelectedData = SelectedProposeArtViewData()


					' common tables
				Case SelectedModulKey.MODUL_CONTACT_CATEGORY
					Dim SelectedData = SelectedContactCategoryViewData

				Case SelectedModulKey.MODUL_EMPLOYEMENT_CATEGORIZATION
					Dim SelectedData = SelectedEmployementCategorizedViewData

				Case SelectedModulKey.MODUL_DIVERSE_COUNTRY
					Dim SelectedData = SelectedCountryViewData

				Case SelectedModulKey.MODUL_DIVERSE_BRANCHES
					Dim SelectedData = SelectedSectorViewData
					'success = m_TablesettingDatabaseAccess.DeleteSectorData(SelectedData.recid)
					'success = success AndAlso LoadSectorList()

				Case SelectedModulKey.MODUL_DIVERSE_QUALIFICATION
					Dim SelectedData = SelectedJobViewData

				Case SelectedModulKey.MODUL_DIVERSE_FCOSTCENTER
					Dim SelectedData = SelectedfCostcenterViewData

				Case SelectedModulKey.MODUL_DIVERSE_SCOSTCENTER
					Dim SelectedData = SelectedSCostcenterViewData

				Case SelectedModulKey.MODUL_DIVERSE_BVGGENTS
					Dim gender = "M"
					Dim SelectedData = SelectedBVGViewData

				Case SelectedModulKey.MODUL_DIVERSE_BVGFEMALE
					Dim gender = "F"
					Dim SelectedData = SelectedBVGViewData

				Case SelectedModulKey.MODUL_DIVERSE_FF13SALARY
					Dim SelectedData = SelectedFF13LohnViewData

				Case SelectedModulKey.MODUL_DIVERSE_QST
					Dim SelectedData = SelectedQSTInfoViewData

				Case SelectedModulKey.MODUL_DIVERSE_BUSINESSBRANCHS
					Dim SelectedData = SelectedBusinessBranchesViewData

				Case SelectedModulKey.MODUL_DIVERSE_FIBU
					Dim SelectedData = SelectedFibuKontenViewData

				Case SelectedModulKey.MODUL_DIVERSE_ABSENCE
					Dim SelectedData = SelectedAbsenceViewData

				Case SelectedModulKey.MODUL_DIVERSE_AGBWOS
					Dim SelectedData = SelectedTerminAndConditionsViewData

				Case SelectedModulKey.MODUL_DIVERSE_SALUTATION
					Dim SelectedData = SelectedSalutationViewData

				Case SelectedModulKey.MODUL_DIVERSE_SMSTEMPLATE
					Dim SelectedData = SelectedSMSTemplateViewData

				Case SelectedModulKey.MODUL_MAIN_PRINTTEMPLATES
					Dim SelectedData = SelectedPrintTemplatesViewData()

				Case SelectedModulKey.MODUL_MAIN_EXPORTTEMPLATES
					Dim SelectedData = SelectedExportTemplatesViewData()

				Case Else
					success = False

			End Select


		Catch ex As Exception
			success = False

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

		Return success

	End Function

	Function UpdateRecord(ByVal rowobject As Object) As Boolean
		Dim success As Boolean = True

		Try

			Select Case m_Selectedmodul
				Case SelectedModulKey.MODUL_EMPLOYEE_CONTACT
					Dim SelectedData = CType(rowobject, EmployeeContactData)
					If SelectedData.recid = 0 Then
						success = m_TablesettingDatabaseAccess.AddEmployeeContactData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateEmployeeContactData(SelectedData)
					End If
					success = success AndAlso LoadEmployeeContactList()

				Case SelectedModulKey.MODUL_EMPLOYEE_FSTATE
					Dim SelectedData = CType(rowobject, EmployeeStateData)
					If SelectedData.recid = 0 Then
						success = m_TablesettingDatabaseAccess.AddEmployeeStateData1(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateEmployeeStateData1(SelectedData)
					End If
					success = success AndAlso LoadEmployeeFStateList()

				Case SelectedModulKey.MODUL_EMPLOYEE_SSTATE
					Dim SelectedData = CType(rowobject, EmployeeStateData)
					If SelectedData.recid = 0 Then
						success = m_TablesettingDatabaseAccess.AddEmployeeStateData2(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateEmployeeStateData2(SelectedData)
					End If
					success = success AndAlso LoadEmployeeSStateList()

				Case SelectedModulKey.MODUL_EMPLOYEE_CIVILSTATE
					Dim SelectedData = CType(rowobject, Common.DataObjects.CivilStateData)
					If SelectedData.recid = 0 Then
						success = m_TablesettingDatabaseAccess.AddEmployeeCivilstateData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateEmployeeCivilstateData(SelectedData)
					End If
					success = success AndAlso LoadEmployeeCivilstateList()

				Case SelectedModulKey.MODUL_EMPLOYEE_LANGUAGELETTER
					Dim SelectedData = CType(rowobject, JobLanguageData)
					If SelectedData.recid = 0 Then
						success = m_TablesettingDatabaseAccess.AddJobLanguageData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateJobLanguageData(SelectedData)
					End If
					success = success AndAlso LoadEmployeeLanguageLetterList()

				Case SelectedModulKey.MODUL_EMPLOYEE_ASSESSMENT
					Dim SelectedData = CType(rowobject, AssessmentData)
					If SelectedData.recid = 0 Then
						success = m_TablesettingDatabaseAccess.AddAssessmentData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateAssessmentData(SelectedData)
					End If
					success = success AndAlso LoadEmployeeAssessmentList()

				Case SelectedModulKey.MODUL_EMPLOYEE_COMMUNICATIONTYPE
					Dim SelectedData = CType(rowobject, CommunicationTypeData)
					If SelectedData.recid = 0 Then
						success = m_TablesettingDatabaseAccess.AddCommunicationTypeData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateCommunicationTypeData(SelectedData)
					End If
					success = success AndAlso LoadEmployeeCommunicationTypeList()

				Case SelectedModulKey.MODUL_EMPLOYEE_CARRESERVE
					Dim SelectedData = CType(rowobject, CarReserveData)
					If SelectedData.recid = 0 Then
						success = m_TablesettingDatabaseAccess.AddCarReserveData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateCarReserveData(SelectedData)
					End If
					success = success AndAlso LoadEmployeeCarReserveList()

				Case SelectedModulKey.MODUL_EMPLOYEE_DRIVINGLICENCE
					Dim SelectedData = CType(rowobject, DrivingLicenceData)
					If SelectedData.recid = 0 Then
						success = m_TablesettingDatabaseAccess.AddDrivingLicenceData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateDrivingLicenceData(SelectedData)
					End If
					success = success AndAlso LoadEmployeeDrivingLicenceList()

				Case SelectedModulKey.MODUL_EMPLOYEE_VEHICLE
					Dim SelectedData = CType(rowobject, VehicleData)
					If SelectedData.recid = 0 Then
						success = m_TablesettingDatabaseAccess.AddVehicleData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateVehicleData(SelectedData)
					End If
					success = success AndAlso LoadEmployeeVehicleList()

				Case SelectedModulKey.MODUL_EMPLOYEE_CONTACTRES1
					Dim SelectedData = CType(rowobject, ContactReserveData)
					If SelectedData.recid = 0 Then
						success = m_TablesettingDatabaseAccess.AddContactReserveData(ContactReserveType.Reserve1, SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateContactReserveData(ContactReserveType.Reserve1, SelectedData)
					End If
					success = success AndAlso LoadEmployeeContactReserveList(ContactReserveType.Reserve1)

				Case SelectedModulKey.MODUL_EMPLOYEE_CONTACTRES2
					Dim SelectedData = CType(rowobject, ContactReserveData)
					If SelectedData.recid = 0 Then
						success = m_TablesettingDatabaseAccess.AddContactReserveData(ContactReserveType.Reserve2, SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateContactReserveData(ContactReserveType.Reserve2, SelectedData)
					End If
					success = success AndAlso LoadEmployeeContactReserveList(ContactReserveType.Reserve2)

				Case SelectedModulKey.MODUL_EMPLOYEE_CONTACTRES3
					Dim SelectedData = CType(rowobject, ContactReserveData)
					If SelectedData.recid = 0 Then
						success = m_TablesettingDatabaseAccess.AddContactReserveData(ContactReserveType.Reserve3, SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateContactReserveData(ContactReserveType.Reserve3, SelectedData)
					End If
					success = success AndAlso LoadEmployeeContactReserveList(ContactReserveType.Reserve3)

				Case SelectedModulKey.MODUL_EMPLOYEE_CONTACTRES4
					Dim SelectedData = CType(rowobject, ContactReserveData)
					If SelectedData.recid = 0 Then
						success = m_TablesettingDatabaseAccess.AddContactReserveData(ContactReserveType.Reserve4, SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateContactReserveData(ContactReserveType.Reserve4, SelectedData)
					End If
					success = success AndAlso LoadEmployeeContactReserveList(ContactReserveType.Reserve4)

				Case SelectedModulKey.MODUL_EMPLOYEE_DEADLINE
					Dim SelectedData = CType(rowobject, DeadlineData)
					If SelectedData.recid = 0 Then
						success = m_TablesettingDatabaseAccess.AddDeadLineData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateDeadLineData(SelectedData)
					End If
					success = success AndAlso LoadEmployeeDeadlineList()

				Case SelectedModulKey.MODUL_EMPLOYEE_WORKPENSUM
					Dim SelectedData = CType(rowobject, WorkPensumData)
					If SelectedData.recid = 0 Then
						success = m_TablesettingDatabaseAccess.AddWorkPensumData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateWorkPensumData(SelectedData)
					End If
					success = success AndAlso LoadEmployeeWorkPensumList()

				Case SelectedModulKey.MODUL_EMPLOYEE_EMPLOYEMENTTYPE
					Dim SelectedData = CType(rowobject, EmployeeEmployementTypeData)
					If SelectedData.recid = 0 Then
						success = m_TablesettingDatabaseAccess.AddEmployementTypeData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateEmployementTypeData(SelectedData)
					End If
					success = success AndAlso LoadEmployeeEmployementTypeList()

				Case SelectedModulKey.MODUL_EMPLOYEE_DOCUMENTCATEGORY
					Dim SelectedData = CType(rowobject, Employee.DataObjects.DocumentMng.EmployeeDocumentCategoryData)
					If Not SelectedData Is Nothing AndAlso IsNumberInRange(SelectedData.CategoryNumber.GetValueOrDefault(0), 200, 250) Then
						Dim msg As String = "Die Kategorie-Nummern zwischen 200 und 250 sind vom System reserviert. Ihr Datensatz kann nicht verändert werden."
						m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(msg))
						Return False
					End If

					If SelectedData.ID = 0 Then
						success = success AndAlso m_TablesettingDatabaseAccess.AddEmployeeDocumentCategoryData(SelectedData)
					Else
						success = success AndAlso m_TablesettingDatabaseAccess.UpdateEmployeeDocumentCategoryData(SelectedData)
					End If
					success = success AndAlso LoadEmployeeDocumentCategoryList()

				Case SelectedModulKey.MODUL_EMPLOYEE_INTERVIEWSTATE
					Dim SelectedData = CType(rowobject, EmployeeInteriviewStateData)
					If SelectedData.recid = 0 Then
						success = m_TablesettingDatabaseAccess.AddInterviewStateData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateInterviewStateData(SelectedData)
					End If
					success = success AndAlso LoadEmployeeInterviewStateList()




				Case SelectedModulKey.MODUL_CUSTOMER_PROPERTY
					Dim SelectedData = CType(rowobject, CustomerPropertyData)
					If SelectedData.recid = 0 Then
						success = m_TablesettingDatabaseAccess.AddCustomerPropertyData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateCustomerPropertyData(SelectedData)
					End If
					success = success AndAlso LoadCustomerPropertyList()

				Case SelectedModulKey.MODUL_CUSTOMER_CONTACT
					Dim SelectedData = CType(rowobject, CustomerContactData)
					If SelectedData.recid = 0 Then
						success = m_TablesettingDatabaseAccess.AddCustomerContactData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateCustomerContactData(SelectedData)
					End If
					success = success AndAlso LoadCustomerContactList()

				Case SelectedModulKey.MODUL_CUSTOMER_FSTATE
					Dim SelectedData = CType(rowobject, CustomerStateData)
					If SelectedData.recid = 0 Then
						success = m_TablesettingDatabaseAccess.AddCustomerStateData1(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateCustomerStateData1(SelectedData)
					End If
					success = success AndAlso LoadCustomerFStateList()

				Case SelectedModulKey.MODUL_CUSTOMER_SSTATE
					Dim SelectedData = CType(rowobject, CustomerStateData)
					If SelectedData.recid = 0 Then
						success = m_TablesettingDatabaseAccess.AddCustomerStateData2(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateCustomerStateData2(SelectedData)
					End If
					success = success AndAlso LoadCustomerSStateList()

				Case SelectedModulKey.MODUL_CUSTOMER_STICHWORT
					Dim SelectedData = CType(rowobject, CustomerStichwortData)
					If SelectedData.recid = 0 Then
						success = m_TablesettingDatabaseAccess.AddCustomerStichwortData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateCustomerStichwortData(SelectedData)
					End If
					success = success AndAlso LoadCustomerStichwortList()

				Case SelectedModulKey.MODUL_CUSTOMER_EMPLOYEMENTTYPE
					Dim SelectedData = CType(rowobject, CustomerEmployementTypeData)
					If SelectedData.recid = 0 Then
						success = m_TablesettingDatabaseAccess.AddCustomerEmployementTypeData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateCustomerEmployementTypeData(SelectedData)
					End If
					success = success AndAlso LoadCustomerEmployementTypeList()


				Case SelectedModulKey.MODUL_CUSTOMER_CONTACTRES1
					Dim SelectedData = CType(rowobject, Customer.DataObjects.CustomerReserveData)
					If SelectedData.ID = 0 Then
						success = m_TablesettingDatabaseAccess.AddCustomerContactReserveData(ContactReserveType.Reserve1, SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateCustomerContactReserveData(ContactReserveType.Reserve1, SelectedData)
					End If
					success = success AndAlso LoadCustomerContactReserveList(ContactReserveType.Reserve1)

				Case SelectedModulKey.MODUL_CUSTOMER_CONTACTRES2
					Dim SelectedData = CType(rowobject, Customer.DataObjects.CustomerReserveData)
					If SelectedData.ID = 0 Then
						success = m_TablesettingDatabaseAccess.AddCustomerContactReserveData(ContactReserveType.Reserve2, SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateCustomerContactReserveData(ContactReserveType.Reserve2, SelectedData)
					End If
					success = success AndAlso LoadCustomerContactReserveList(ContactReserveType.Reserve2)

				Case SelectedModulKey.MODUL_CUSTOMER_CONTACTRES3
					Dim SelectedData = CType(rowobject, Customer.DataObjects.CustomerReserveData)
					If SelectedData.ID = 0 Then
						success = m_TablesettingDatabaseAccess.AddCustomerContactReserveData(ContactReserveType.Reserve3, SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateCustomerContactReserveData(ContactReserveType.Reserve3, SelectedData)
					End If
					success = success AndAlso LoadCustomerContactReserveList(ContactReserveType.Reserve3)

				Case SelectedModulKey.MODUL_CUSTOMER_CONTACTRES4
					Dim SelectedData = CType(rowobject, Customer.DataObjects.CustomerReserveData)
					If SelectedData.ID = 0 Then
						success = m_TablesettingDatabaseAccess.AddCustomerContactReserveData(ContactReserveType.Reserve4, SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateCustomerContactReserveData(ContactReserveType.Reserve4, SelectedData)
					End If
					success = success AndAlso LoadCustomerContactReserveList(ContactReserveType.Reserve4)


				Case SelectedModulKey.MODUL_CUSTOMER_PAYMENTREMINDERCODE
					Dim SelectedData = CType(rowobject, Customer.DataObjects.PaymentReminderCodeData)
					If SelectedData.ID = 0 Then
						success = m_TablesettingDatabaseAccess.AddCustomerPaymentReminderCodeData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateCustomerPaymentReminderCodeData(SelectedData)
					End If
					success = success AndAlso LoadCustomerPaymentReminderCodeList()

				Case SelectedModulKey.MODUL_CUSTOMER_PAYMENTCONDITION
					Dim SelectedData = CType(rowobject, Customer.DataObjects.PaymentConditionData)
					If SelectedData.ID = 0 Then
						success = m_TablesettingDatabaseAccess.AddCustomerPaymentConditionData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateCustomerPaymentConditionData(SelectedData)
					End If
					success = success AndAlso LoadCustomerPaymentConditionList()

				Case SelectedModulKey.MODUL_CUSTOMER_INVOICEOPTIONS
					Dim SelectedData = CType(rowobject, Customer.DataObjects.InvoiceOptionData)
					If SelectedData.ID = 0 Then
						success = m_TablesettingDatabaseAccess.AddCustomerInvoiceOptionData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateCustomerInvoiceOptionData(SelectedData)
					End If
					success = success AndAlso LoadCustomerInvoiceOptionsList()

				Case SelectedModulKey.MODUL_CUSTOMER_INVOICETYPE
					Dim SelectedData = CType(rowobject, Customer.DataObjects.InvoiceTypeData)
					If SelectedData.ID.GetValueOrDefault(0) = 0 Then
						success = m_TablesettingDatabaseAccess.AddCustomerInvoiceTypeData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateCustomerInvoiceTypeData(SelectedData)
					End If
					success = success AndAlso LoadCustomerInvoiceTypeList()

				Case SelectedModulKey.MODUL_CUSTOMER_INVOICESHIPMENT
					Dim SelectedData = CType(rowobject, Customer.DataObjects.OPShipmentData)
					If SelectedData.ID = 0 Then
						success = m_TablesettingDatabaseAccess.AddCustomerInvoiceShipment(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateCustomerInvoiceShipment(SelectedData)
					End If
					success = success AndAlso LoadCustomerInvoiceShipmentList()


				Case SelectedModulKey.MODUL_CUSTOMER_NUMBEROFEMPLOYEES
					Dim SelectedData = CType(rowobject, Customer.DataObjects.NumberOfEmployeesData)
					If SelectedData.ID = 0 Then
						success = m_TablesettingDatabaseAccess.AddCustomerNumberOfEmployeesData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateCustomerNumberOfEmployeesData(SelectedData)
					End If
					success = success AndAlso LoadCustomerNumberOfEmployeesList()

				Case SelectedModulKey.MODUL_CUSTOMER_DOCUMENTCATEGORY
					Dim SelectedData = CType(rowobject, Customer.DataObjects.CustomerDocumentCategoryData)
					If Not SelectedData Is Nothing AndAlso IsNumberInRange(SelectedData.CategoryNumber.GetValueOrDefault(0), 100, 150) Then
						Dim msg As String = "Die Kategorie-Nummern zwischen 100 und 150 sind vom System reserviert. Ihr Datensatz kann nicht verändert werden."
						m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(msg))
						success = False
					End If

					If SelectedData.ID = 0 Then
						success = success AndAlso m_TablesettingDatabaseAccess.AddCustomerDocumentCategoryData(SelectedData)
					Else
						success = success AndAlso m_TablesettingDatabaseAccess.UpdateCustomerDocumentCategoryData(SelectedData)
					End If
					success = success AndAlso LoadCustomerDocumentCategoryList()


					' Responsible person
				Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_CONTACT
					Dim SelectedData = CType(rowobject, Customer.DataObjects.ResponsiblePersonContactInfo)
					If SelectedData.ID = 0 Then
						success = m_TablesettingDatabaseAccess.AddResponsiblepersonContactData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateResponsiblepersonContactData(SelectedData)
					End If
					success = success AndAlso LoadCResponsibleContactList()

				Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_FSTATE
					Dim SelectedData = CType(rowobject, Customer.DataObjects.ResponsiblePersonStateData)
					If SelectedData.ID = 0 Then
						success = m_TablesettingDatabaseAccess.AddResponsiblepersonStateData1(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateResponsiblepersonStateData1(SelectedData)
					End If
					success = success AndAlso LoadCResponsibleFStateList()

				Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_SSTATE
					Dim SelectedData = CType(rowobject, Customer.DataObjects.ResponsiblePersonStateData)
					If SelectedData.ID = 0 Then
						success = m_TablesettingDatabaseAccess.AddResponsiblepersonStateData2(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateResponsiblepersonStateData2(SelectedData)
					End If
					success = success AndAlso LoadCResponsibleSStateList()

				Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_DEPARTMENT
					Dim SelectedData = CType(rowobject, Customer.DataObjects.DepartmentData)
					If SelectedData.ID = 0 Then
						success = m_TablesettingDatabaseAccess.AddResponsiblepersonDepartment(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateResponsiblepersonDepartment(SelectedData)
					End If
					success = success AndAlso LoadCResponsibleDepartmentList()

				Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_POSITION
					Dim SelectedData = CType(rowobject, Customer.DataObjects.PositionData)
					If SelectedData.ID = 0 Then
						success = m_TablesettingDatabaseAccess.AddResponsiblepersonPosition(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateResponsiblepersonPosition(SelectedData)
					End If
					success = success AndAlso LoadCResponsiblePositionList()

				Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_COMMUNICATION
					Dim SelectedData = CType(rowobject, Customer.DataObjects.CustomerCommunicationData)
					If SelectedData.ID = 0 Then
						success = m_TablesettingDatabaseAccess.AddResponsiblepersonCommunication(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateResponsiblepersonCommunication(SelectedData)
					End If
					success = success AndAlso LoadCResponsibleCommunicationList()

				Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_COMMUNICATIONTYPE
					Dim SelectedData = CType(rowobject, Customer.DataObjects.CustomerCommunicationTypeData)
					If SelectedData.ID = 0 Then
						success = m_TablesettingDatabaseAccess.AddResponsiblepersonCommunicationType(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateResponsiblepersonCommunicationType(SelectedData)
					End If
					success = success AndAlso LoadCResponsibleCommunicationTypeList()



				Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_CONTACTRES1
					Dim SelectedData = CType(rowobject, Customer.DataObjects.ResponsiblePersonReserveData)
					If SelectedData.ID = 0 Then
						success = m_TablesettingDatabaseAccess.AddCResponsibleContactReserveData(ContactReserveType.Reserve1, SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateCResponsibleContactReserveData(ContactReserveType.Reserve1, SelectedData)
					End If
					success = success AndAlso LoadCResponsibleContactReserveList(ContactReserveType.Reserve1)

				Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_CONTACTRES2
					Dim SelectedData = CType(rowobject, Customer.DataObjects.ResponsiblePersonReserveData) ' SelectedCResponsibleContactReserveViewData(ContactReserveType.Reserve2)
					If SelectedData.ID = 0 Then
						success = m_TablesettingDatabaseAccess.AddCResponsibleContactReserveData(ContactReserveType.Reserve2, SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateCResponsibleContactReserveData(ContactReserveType.Reserve2, SelectedData)
					End If
					success = success AndAlso LoadCResponsibleContactReserveList(ContactReserveType.Reserve2)

				Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_CONTACTRES3
					Dim SelectedData = CType(rowobject, Customer.DataObjects.ResponsiblePersonReserveData)
					If SelectedData.ID = 0 Then
						success = m_TablesettingDatabaseAccess.AddCResponsibleContactReserveData(ContactReserveType.Reserve3, SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateCResponsibleContactReserveData(ContactReserveType.Reserve3, SelectedData)
					End If
					success = success AndAlso LoadCResponsibleContactReserveList(ContactReserveType.Reserve3)

				Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_CONTACTRES4
					Dim SelectedData = CType(rowobject, Customer.DataObjects.ResponsiblePersonReserveData)
					If SelectedData.ID = 0 Then
						success = m_TablesettingDatabaseAccess.AddCResponsibleContactReserveData(ContactReserveType.Reserve4, SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateCResponsibleContactReserveData(ContactReserveType.Reserve4, SelectedData)
					End If
					success = success AndAlso LoadCResponsibleContactReserveList(ContactReserveType.Reserve4)


					' Vacancy
				Case SelectedModulKey.MODUL_VACANCY_CONTACT
					Dim SelectedData = CType(rowobject, VacancyContactData)
					If SelectedData.ID = 0 Then
						success = m_TablesettingDatabaseAccess.AddVacancyContactData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateVacancyContactData(SelectedData)
					End If
					success = success AndAlso LoadVacancyContactList()

				Case SelectedModulKey.MODUL_VACANCY_STATE
					Dim SelectedData = CType(rowobject, VacancyStateData)
					If SelectedData.ID = 0 Then
						success = m_TablesettingDatabaseAccess.AddVacancyStateData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateVacancyStateData(SelectedData)
					End If
					success = success AndAlso LoadVacancyStateList()

				Case SelectedModulKey.MODUL_VACANCY_GROUP
					Dim SelectedData = CType(rowobject, VacancyGroupData)
					If SelectedData.ID = 0 Then
						success = m_TablesettingDatabaseAccess.AddVacancyGroupData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateVacancyGroupData(SelectedData)
					End If
					success = success AndAlso LoadVacancyGroupList()


					' Offer
				Case SelectedModulKey.MODUL_OFFER_CONTACT
					Dim SelectedData = CType(rowobject, OfferContactData)
					If SelectedData.ID = 0 Then
						success = m_TablesettingDatabaseAccess.AddOfferContactData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateOfferContactData(SelectedData)
					End If
					success = success AndAlso LoadOfferContactList()

				Case SelectedModulKey.MODUL_OFFER_STATE
					Dim SelectedData = CType(rowobject, OfferStateData)
					If SelectedData.ID = 0 Then
						success = m_TablesettingDatabaseAccess.AddOfferStateData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateOfferStateData(SelectedData)
					End If
					success = success AndAlso LoadOfferStateList()

				Case SelectedModulKey.MODUL_OFFER_GROUP
					Dim SelectedData = CType(rowobject, OfferGroupData)
					If SelectedData.ID = 0 Then
						success = m_TablesettingDatabaseAccess.AddOfferGroupData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateOfferGroupData(SelectedData)
					End If
					success = success AndAlso LoadOfferGroupList()



					' Propose
				Case SelectedModulKey.MODUL_PROPOSE_STATE
					Dim SelectedData = CType(rowobject, ProposeStateData)
					If SelectedData.recid = 0 Then
						success = m_TablesettingDatabaseAccess.AddProposeStateData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateProposeStateData(SelectedData)
					End If
					success = success AndAlso LoadProposeStateList()

				Case SelectedModulKey.MODUL_PROPOSE_EMPLOYEMENTTYPE
					Dim SelectedData = CType(rowobject, ProposeEmployementTypeData)
					If SelectedData.recid = 0 Then
						success = m_TablesettingDatabaseAccess.AddProposeEmployementTypeData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateProposeEmployementTypeData(SelectedData)
					End If
					success = success AndAlso LoadProposeEmployementTypeList()

				Case SelectedModulKey.MODUL_PROPOSE_ART
					Dim SelectedData = CType(rowobject, ProposeArtData)
					If SelectedData.recid = 0 Then
						success = m_TablesettingDatabaseAccess.AddProposeArtData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateProposeArtData(SelectedData)
					End If
					success = success AndAlso LoadProposeArtList()



					' common tables
				Case SelectedModulKey.MODUL_CONTACT_CATEGORY
					Dim SelectedData = CType(rowobject, SP.DatabaseAccess.Common.DataObjects.ContactType1Data)
					If SelectedData.ID = 0 Then
						success = m_TablesettingDatabaseAccess.AddContactCategoryData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateContactCategoryData(SelectedData)
					End If
					success = success AndAlso LoadContactCategoryList()

				Case SelectedModulKey.MODUL_EMPLOYEMENT_CATEGORIZATION
					Dim SelectedData = CType(rowobject, ES.DataObjects.ESMng.ESCategorizationData)
					If SelectedData.ID = 0 Then
						success = m_TablesettingDatabaseAccess.AddEmployementCategorizedData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateEmployementCategorizedData(SelectedData)
					End If
					success = success AndAlso LoadEmployementCategorizedList()

				Case SelectedModulKey.MODUL_DIVERSE_COUNTRY
					Dim SelectedData = CType(rowobject, CountryData)
					If SelectedData.recid = 0 Then
						success = m_TablesettingDatabaseAccess.AddCountryData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateCountryData(SelectedData)
					End If
					success = success AndAlso LoadCountryList()

				Case SelectedModulKey.MODUL_DIVERSE_BRANCHES
					Dim SelectedData = CType(rowobject, SectorData)
					If SelectedData.recid = 0 Then
						success = m_TablesettingDatabaseAccess.AddSectorData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateSectorData(SelectedData)
					End If
					success = success AndAlso LoadSectorList()

				Case SelectedModulKey.MODUL_DIVERSE_QUALIFICATION
					Dim SelectedData = CType(rowobject, JobData)
					If SelectedData.recid = 0 Then
						success = m_TablesettingDatabaseAccess.AddJobData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateJobData(SelectedData)
					End If
					success = success AndAlso LoadJobList()

				Case SelectedModulKey.MODUL_DIVERSE_FCOSTCENTER
					Dim SelectedData = CType(rowobject, CostCenter1Data)
					If SelectedData.recId = 0 Then
						success = m_TablesettingDatabaseAccess.AddCostCenter1Data(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateCostCenter1Data(SelectedData)
					End If
					success = success AndAlso LoadFCostcenterList()

				Case SelectedModulKey.MODUL_DIVERSE_SCOSTCENTER
					Dim SelectedData = CType(rowobject, CostCenter2Data)
					If SelectedData.recId = 0 Then
						success = m_TablesettingDatabaseAccess.AddCostCenter2Data(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateCostCenter2Data(SelectedData)
					End If
					success = success AndAlso LoadSCostcenterList()

				Case SelectedModulKey.MODUL_DIVERSE_BVGGENTS
					Dim gender = "M"
					Dim SelectedData = CType(rowobject, BVGData)
					If SelectedData.recId = 0 Then
						SelectedData.MDNr = lueMandant.EditValue
						SelectedData.ProzJahr = lueYear.EditValue
						success = m_TablesettingDatabaseAccess.AddBVGData(gender, SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateBVGData(gender, SelectedData)
					End If
					success = success AndAlso LoadBVGList(gender)

				Case SelectedModulKey.MODUL_DIVERSE_BVGFEMALE
					Dim gender = "F"
					Dim SelectedData = CType(rowobject, BVGData)
					If SelectedData.recId = 0 Then
						SelectedData.MDNr = lueMandant.EditValue
						SelectedData.ProzJahr = lueYear.EditValue
						success = m_TablesettingDatabaseAccess.AddBVGData(gender, SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateBVGData(gender, SelectedData)
					End If
					success = success AndAlso LoadBVGList(gender)

				Case SelectedModulKey.MODUL_DIVERSE_FF13SALARY
					Dim SelectedData = CType(rowobject, FF13LohnData)
					If SelectedData.recId = 0 Then
						success = m_TablesettingDatabaseAccess.AddFF13LohnData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateFF13LohnData(SelectedData)
					End If
					success = success AndAlso LoadFF13LohnList()

				Case SelectedModulKey.MODUL_DIVERSE_QST
					Dim SelectedData = CType(rowobject, QstInfoData)
					If SelectedData.recId = 0 Then
						success = m_TablesettingDatabaseAccess.AddQSTInfoData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateQSTInfoData(SelectedData)
					End If
					success = success AndAlso LoadQSTInfoList()

				Case SelectedModulKey.MODUL_DIVERSE_BUSINESSBRANCHS
					Dim SelectedData = CType(rowobject, AvilableBusinessBranchData)
					If SelectedData.recid = 0 Then
						success = m_TablesettingDatabaseAccess.AddBusinessBranchsData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateBusinessBranchsData(SelectedData)
					End If
					success = success AndAlso LoadBusinessBranchsList()

				Case SelectedModulKey.MODUL_DIVERSE_FIBU
					Dim SelectedData = CType(rowobject, FIBUData)
					If SelectedData.recid = 0 Then
						success = m_TablesettingDatabaseAccess.AddFIBUKontenData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateFIBUKontenData(SelectedData)
					End If
					success = success AndAlso LoadFibukontenList()

				Case SelectedModulKey.MODUL_DIVERSE_ABSENCE
					Dim SelectedData = CType(rowobject, AbsenceData)
					If SelectedData.recid = 0 Then
						success = m_TablesettingDatabaseAccess.AddAbsenceData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateAbsenceData(SelectedData)
					End If
					success = success AndAlso LoadAbsenceList()

				Case SelectedModulKey.MODUL_DIVERSE_AGBWOS
					Dim SelectedData = CType(rowobject, TermsAndConditionsData)
					If SelectedData.recid = 0 Then
						success = m_TablesettingDatabaseAccess.AddTermsAndConditionsData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateTermsAndConditionsData(SelectedData)
					End If
					success = success AndAlso LoadTermsandConditionsList()

				Case SelectedModulKey.MODUL_DIVERSE_SALUTATION
					Dim SelectedData = CType(rowobject, SalutationData) '  SelectedSalutationViewData
					If SelectedData.recId = 0 Then
						success = m_TablesettingDatabaseAccess.AddSalutationData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateSalutationData(SelectedData)
					End If
					success = success AndAlso LoadSalutationList()

				Case SelectedModulKey.MODUL_DIVERSE_SMSTEMPLATE
					Dim SelectedData = CType(rowobject, SMSTemplateData) '  SelectedSMSTemplateViewData
					If SelectedData.recid = 0 Then
						success = m_TablesettingDatabaseAccess.AddSMSTemplateData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateSMSTemplateData(SelectedData)
					End If
					success = success AndAlso LoadSMSTemplatesList()

				Case SelectedModulKey.MODUL_MAIN_PRINTTEMPLATES
					Dim SelectedData = CType(rowobject, PrintTemplatesData)
					If SelectedData.recid = 0 Then
						success = m_TablesettingDatabaseAccess.AddPrintTemplatesData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdatePrintTemplatesData(SelectedData)
					End If
					success = success AndAlso LoadPrintTemplatesList()

				Case SelectedModulKey.MODUL_MAIN_EXPORTTEMPLATES
					Dim SelectedData = CType(rowobject, ExportTemplatesData)
					If SelectedData.recid = 0 Then
						success = m_TablesettingDatabaseAccess.AddExportTemplatesData(SelectedData)
					Else
						success = m_TablesettingDatabaseAccess.UpdateExportTemplatesData(SelectedData)
					End If
					success = success AndAlso LoadExportTemplatesList()


				Case Else
					m_UtilityUI.ShowErrorDialog(String.Format("Methode was not implemented!{0}{1}", vbNewLine, m_Selectedmodul.ToString))
					success = False

			End Select


		Catch ex As Exception
			success = False

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

		Return success

	End Function

	Private Function DeleteRecord() As Boolean
		Dim success As Boolean = True

		Try
			Select Case m_Selectedmodul
				Case SelectedModulKey.MODUL_EMPLOYEE_CONTACT
					Dim SelectedData = SelectedEmployeeContactViewData
					success = m_TablesettingDatabaseAccess.DeleteEmployeeContactData(SelectedData.recid)
					success = success AndAlso LoadEmployeeContactList()

				Case SelectedModulKey.MODUL_EMPLOYEE_FSTATE
					Dim SelectedData = SelectedEmployeeFStateViewData()
					success = m_TablesettingDatabaseAccess.DeleteEmployeeStateData1(SelectedData.recid)
					success = success AndAlso LoadEmployeeFStateList()

				Case SelectedModulKey.MODUL_EMPLOYEE_SSTATE
					Dim SelectedData = SelectedEmployeeSStateViewData()
					success = m_TablesettingDatabaseAccess.DeleteEmployeeStateData2(SelectedData.recid)
					success = success AndAlso LoadEmployeeSStateList()

				Case SelectedModulKey.MODUL_EMPLOYEE_CIVILSTATE
					Dim SelectedData = SelectedEmployeeCivilstateViewData()
					success = m_TablesettingDatabaseAccess.DeleteEmployeeCivilstateData(SelectedData.recid)
					success = success AndAlso LoadEmployeeCivilstateList()

				Case SelectedModulKey.MODUL_EMPLOYEE_LANGUAGELETTER
					Dim SelectedData = SelectedEmployeeJobLanguageViewData()
					success = m_TablesettingDatabaseAccess.DeleteJobLanguageData(SelectedData.recid)
					success = success AndAlso LoadEmployeeLanguageLetterList()

				Case SelectedModulKey.MODUL_EMPLOYEE_ASSESSMENT
					Dim SelectedData = SelectedEmployeeAssesmentViewData()
					success = m_TablesettingDatabaseAccess.DeleteAssessmentData(SelectedData.recid)
					success = success AndAlso LoadEmployeeAssessmentList()

				Case SelectedModulKey.MODUL_EMPLOYEE_COMMUNICATIONTYPE
					Dim SelectedData = SelectedEmployeeCommunicationTypeViewData()
					success = m_TablesettingDatabaseAccess.DeleteCommunicationTypeData(SelectedData.recid)
					success = success AndAlso LoadEmployeeCommunicationTypeList()

				Case SelectedModulKey.MODUL_EMPLOYEE_CARRESERVE
					Dim SelectedData = SelectedEmployeeCarReserveViewData()
					success = m_TablesettingDatabaseAccess.DeleteCarReserveData(SelectedData.recid)
					success = success AndAlso LoadEmployeeCarReserveList()

				Case SelectedModulKey.MODUL_EMPLOYEE_DRIVINGLICENCE
					Dim SelectedData = SelectedEmployeeDrivingLicenceViewData()
					success = m_TablesettingDatabaseAccess.DeleteDrivingLicenceData(SelectedData.recid)
					success = success AndAlso LoadEmployeeDrivingLicenceList()

				Case SelectedModulKey.MODUL_EMPLOYEE_VEHICLE
					Dim SelectedData = SelectedEmployeeVehicleViewData()
					success = m_TablesettingDatabaseAccess.DeleteVehicleData(SelectedData.recid)
					success = success AndAlso LoadEmployeeVehicleList()

				Case SelectedModulKey.MODUL_EMPLOYEE_CONTACTRES1
					Dim SelectedData = SelectedEmployeeContactReserveViewData(ContactReserveType.Reserve1)
					success = m_TablesettingDatabaseAccess.DeleteContactReserveData(ContactReserveType.Reserve1, SelectedData.recid)
					success = success AndAlso LoadEmployeeContactReserveList(ContactReserveType.Reserve1)

				Case SelectedModulKey.MODUL_EMPLOYEE_CONTACTRES2
					Dim SelectedData = SelectedEmployeeContactReserveViewData(ContactReserveType.Reserve2)
					success = m_TablesettingDatabaseAccess.DeleteContactReserveData(ContactReserveType.Reserve2, SelectedData.recid)
					success = success AndAlso LoadEmployeeContactReserveList(ContactReserveType.Reserve2)

				Case SelectedModulKey.MODUL_EMPLOYEE_CONTACTRES3
					Dim SelectedData = SelectedEmployeeContactReserveViewData(ContactReserveType.Reserve3)
					success = m_TablesettingDatabaseAccess.DeleteContactReserveData(ContactReserveType.Reserve3, SelectedData.recid)
					success = success AndAlso LoadEmployeeContactReserveList(ContactReserveType.Reserve3)

				Case SelectedModulKey.MODUL_EMPLOYEE_CONTACTRES4
					Dim SelectedData = SelectedEmployeeContactReserveViewData(ContactReserveType.Reserve4)
					success = m_TablesettingDatabaseAccess.DeleteContactReserveData(ContactReserveType.Reserve4, SelectedData.recid)
					success = success AndAlso LoadEmployeeContactReserveList(ContactReserveType.Reserve4)

				Case SelectedModulKey.MODUL_EMPLOYEE_DEADLINE
					Dim SelectedData = SelectedEmployeeDeadlineViewData()
					success = m_TablesettingDatabaseAccess.DeleteDeadLineData(SelectedData.recid)
					success = success AndAlso LoadEmployeeDeadlineList()

				Case SelectedModulKey.MODUL_EMPLOYEE_WORKPENSUM
					Dim SelectedData = SelectedEmployeeWorkPensumViewData()
					success = m_TablesettingDatabaseAccess.DeleteWorkPensumData(SelectedData.recid)
					success = success AndAlso LoadEmployeeWorkPensumList()

				Case SelectedModulKey.MODUL_EMPLOYEE_EMPLOYEMENTTYPE
					Dim SelectedData = SelectedEmployeeEmployementTypeViewData()
					success = m_TablesettingDatabaseAccess.DeleteEmployementTypeData(SelectedData.recid)
					success = success AndAlso LoadEmployeeEmployementTypeList()

				Case SelectedModulKey.MODUL_EMPLOYEE_DOCUMENTCATEGORY
					Dim SelectedData = SelectedEmployeeDocumentCategoryViewData()
					If Not SelectedData Is Nothing AndAlso IsNumberInRange(SelectedData.CategoryNumber.GetValueOrDefault(0), 200, 250) Then
						Dim msg As String = "Die Kategorie-Nummern zwischen 200 und 250 sind vom System reserviert. Ihr Datensatz kann nicht verändert werden."
						m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(msg))
						Return False
					End If
					success = success AndAlso m_TablesettingDatabaseAccess.DeleteEmployeeDocumentCategoryData(SelectedData.ID)
					success = success AndAlso LoadEmployeeDocumentCategoryList()

				Case SelectedModulKey.MODUL_EMPLOYEE_INTERVIEWSTATE
					Dim SelectedData = SelectedEmployeeInterviewStateViewData()
					success = m_TablesettingDatabaseAccess.DeleteInterviewStateData(SelectedData.recid)
					success = success AndAlso LoadEmployeeInterviewStateList()



					' Customer
				Case SelectedModulKey.MODUL_CUSTOMER_PROPERTY
					Dim SelectedData = SelectedCustomerpropertyViewData()
					success = m_TablesettingDatabaseAccess.DeleteCustomerPropertyData(SelectedData.recid)
					success = success AndAlso LoadCustomerPropertyList()

				Case SelectedModulKey.MODUL_CUSTOMER_CONTACT
					Dim SelectedData = SelectedCustomerContactViewData()
					success = m_TablesettingDatabaseAccess.DeleteCustomerContactData(SelectedData.recid)
					success = success AndAlso LoadCustomerContactList()

				Case SelectedModulKey.MODUL_CUSTOMER_FSTATE
					Dim SelectedData = SelectedCustomerFStateViewData()
					success = m_TablesettingDatabaseAccess.DeleteCustomerStateData1(SelectedData.recid)
					success = success AndAlso LoadCustomerFStateList()

				Case SelectedModulKey.MODUL_CUSTOMER_SSTATE
					Dim SelectedData = SelectedCustomerSStateViewData()
					success = m_TablesettingDatabaseAccess.DeleteCustomerStateData2(SelectedData.recid)
					success = success AndAlso LoadCustomerSStateList()


				Case SelectedModulKey.MODUL_CUSTOMER_CONTACTRES1
					Dim SelectedData = SelectedCustomerContactReserveViewData(ContactReserveType.Reserve1)
					success = m_TablesettingDatabaseAccess.DeleteCustomerContactReserveData(ContactReserveType.Reserve1, SelectedData.ID)
					success = success AndAlso LoadCustomerContactReserveList(ContactReserveType.Reserve1)

				Case SelectedModulKey.MODUL_CUSTOMER_CONTACTRES2
					Dim SelectedData = SelectedCustomerContactReserveViewData(ContactReserveType.Reserve2)
					success = m_TablesettingDatabaseAccess.DeleteCustomerContactReserveData(ContactReserveType.Reserve2, SelectedData.ID)
					success = success AndAlso LoadCustomerContactReserveList(ContactReserveType.Reserve2)

				Case SelectedModulKey.MODUL_CUSTOMER_CONTACTRES3
					Dim SelectedData = SelectedCustomerContactReserveViewData(ContactReserveType.Reserve3)
					success = m_TablesettingDatabaseAccess.DeleteCustomerContactReserveData(ContactReserveType.Reserve3, SelectedData.ID)
					success = success AndAlso LoadCustomerContactReserveList(ContactReserveType.Reserve3)

				Case SelectedModulKey.MODUL_CUSTOMER_CONTACTRES4
					Dim SelectedData = SelectedCustomerContactReserveViewData(ContactReserveType.Reserve4)
					success = m_TablesettingDatabaseAccess.DeleteCustomerContactReserveData(ContactReserveType.Reserve4, SelectedData.ID)
					success = success AndAlso LoadCustomerContactReserveList(ContactReserveType.Reserve4)

				Case SelectedModulKey.MODUL_CUSTOMER_STICHWORT
					Dim SelectedData = SelectedCustomerStichwortViewData()
					success = m_TablesettingDatabaseAccess.DeleteCustomerStichwortData(SelectedData.recid)
					success = success AndAlso LoadCustomerStichwortList()

				Case SelectedModulKey.MODUL_CUSTOMER_EMPLOYEMENTTYPE
					Dim SelectedData = SelectedCustomerEmployementTypeViewData()
					success = m_TablesettingDatabaseAccess.DeleteCustomerEmployementTypeData(SelectedData.recid)
					success = success AndAlso LoadCustomerEmployementTypeList()

				Case SelectedModulKey.MODUL_CUSTOMER_PAYMENTREMINDERCODE
					Dim SelectedData = SelectedCustomerPaymentReminderCodeViewData()
					success = m_TablesettingDatabaseAccess.DeleteCustomerPaymentReminderCodeData(SelectedData.ID)
					success = success AndAlso LoadCustomerPaymentReminderCodeList()

				Case SelectedModulKey.MODUL_CUSTOMER_PAYMENTCONDITION
					Dim SelectedData = SelectedCustomerPaymentConditionViewData()
					success = m_TablesettingDatabaseAccess.DeleteCustomerPaymentConditionData(SelectedData.ID)
					success = success AndAlso LoadCustomerPaymentConditionList()

				Case SelectedModulKey.MODUL_CUSTOMER_INVOICEOPTIONS
					Dim SelectedData = SelectedCustomerInvoiceOptionsViewData()
					success = m_TablesettingDatabaseAccess.DeleteCustomerInvoiceOptionData(SelectedData.ID)
					success = success AndAlso LoadCustomerInvoiceOptionsList()

				Case SelectedModulKey.MODUL_CUSTOMER_INVOICETYPE
					Dim SelectedData = SelectedCustomerInvoiceTypeViewData()
					success = m_TablesettingDatabaseAccess.DeleteCustomerInvoiceTypeData(SelectedData.ID)
					success = success AndAlso LoadCustomerInvoiceTypeList()

				Case SelectedModulKey.MODUL_CUSTOMER_INVOICESHIPMENT
					Dim SelectedData = SelectedCustomerInvoiceShipmentViewData()
					success = m_TablesettingDatabaseAccess.DeleteCustomerInvoiceShipment(SelectedData.ID)
					success = success AndAlso LoadCustomerInvoiceShipmentList()


				Case SelectedModulKey.MODUL_CUSTOMER_NUMBEROFEMPLOYEES
					Dim SelectedData = SelectedCustomerNumberOfEmployeesViewData()
					success = m_TablesettingDatabaseAccess.DeleteCustomerNumberOfEmployeesData(SelectedData.ID)
					success = success AndAlso LoadCustomerNumberOfEmployeesList()

				Case SelectedModulKey.MODUL_CUSTOMER_DOCUMENTCATEGORY
					Dim SelectedData = SelectedCustomerDocumentCategoryViewData()

					If Not SelectedData Is Nothing AndAlso IsNumberInRange(SelectedData.CategoryNumber.GetValueOrDefault(0), 100, 150) Then
						Dim msg As String = "Die Kategorie-Nummern zwischen 100 und 150 sind vom System reserviert. Ihr Datensatz kann nicht verändert werden."
						m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(msg))
						Return False
					End If
					success = success AndAlso m_TablesettingDatabaseAccess.DeleteCustomerDocumentCategoryData(SelectedData.ID)
					success = success AndAlso LoadCustomerDocumentCategoryList()



					' Responsible person
				Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_CONTACT
					Dim SelectedData = SelectedResponsiblePersonContactViewData()
					success = m_TablesettingDatabaseAccess.DeleteResponsiblepersonContactData(SelectedData.ID)
					success = success AndAlso LoadCResponsibleContactList()

				Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_FSTATE
					Dim SelectedData = SelectedResponsiblePersonFStateViewData()
					success = m_TablesettingDatabaseAccess.DeleteResponsiblepersonStateData1(SelectedData.ID)
					success = success AndAlso LoadCResponsibleFStateList()

				Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_SSTATE
					Dim SelectedData = SelectedResponsiblePersonSStateViewData()
					success = m_TablesettingDatabaseAccess.DeleteResponsiblepersonStateData2(SelectedData.ID)
					success = success AndAlso LoadCResponsibleSStateList()

				Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_DEPARTMENT
					Dim SelectedData = SelectedResponsiblePersonDepartmentViewData()
					success = m_TablesettingDatabaseAccess.DeleteResponsiblepersonDepartment(SelectedData.ID)
					success = success AndAlso LoadCResponsibleDepartmentList()

				Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_POSITION
					Dim SelectedData = SelectedResponsiblePersonPositionViewData()
					success = m_TablesettingDatabaseAccess.DeleteResponsiblepersonPosition(SelectedData.ID)
					success = success AndAlso LoadCResponsiblePositionList()

				Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_COMMUNICATION
					Dim SelectedData = SelectedResponsiblePersonCommunicationViewData()
					success = m_TablesettingDatabaseAccess.DeleteResponsiblepersonCommunication(SelectedData.ID)
					success = success AndAlso LoadCResponsibleCommunicationList()

				Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_COMMUNICATIONTYPE
					Dim SelectedData = SelectedResponsiblePersonCommunicationTypeViewData()
					success = m_TablesettingDatabaseAccess.DeleteResponsiblepersonCommunicationType(SelectedData.ID)
					success = success AndAlso LoadCResponsibleCommunicationTypeList()


				Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_CONTACTRES1
					Dim SelectedData = SelectedCResponsibleContactReserveViewData(ContactReserveType.Reserve1)
					success = m_TablesettingDatabaseAccess.DeleteCResponsibleContactReserveData(ContactReserveType.Reserve1, SelectedData.ID)
					success = success AndAlso LoadCResponsibleContactReserveList(ContactReserveType.Reserve1)

				Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_CONTACTRES2
					Dim SelectedData = SelectedCResponsibleContactReserveViewData(ContactReserveType.Reserve2)
					success = m_TablesettingDatabaseAccess.DeleteCResponsibleContactReserveData(ContactReserveType.Reserve2, SelectedData.ID)
					success = success AndAlso LoadCResponsibleContactReserveList(ContactReserveType.Reserve2)

				Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_CONTACTRES3
					Dim SelectedData = SelectedCResponsibleContactReserveViewData(ContactReserveType.Reserve3)
					success = m_TablesettingDatabaseAccess.DeleteCResponsibleContactReserveData(ContactReserveType.Reserve3, SelectedData.ID)
					success = success AndAlso LoadCResponsibleContactReserveList(ContactReserveType.Reserve3)

				Case SelectedModulKey.MODUL_RESPONSIBLEPERSON_CONTACTRES4
					Dim SelectedData = SelectedCResponsibleContactReserveViewData(ContactReserveType.Reserve4)
					success = m_TablesettingDatabaseAccess.DeleteCResponsibleContactReserveData(ContactReserveType.Reserve4, SelectedData.ID)
					success = success AndAlso LoadCResponsibleContactReserveList(ContactReserveType.Reserve4)


					' Vacancy
				Case SelectedModulKey.MODUL_VACANCY_CONTACT
					Dim SelectedData = SelectedVacancyContactViewData()
					success = m_TablesettingDatabaseAccess.DeleteVacancyContactData(SelectedData.ID)
					success = success AndAlso LoadVacancyContactList()

				Case SelectedModulKey.MODUL_VACANCY_STATE
					Dim SelectedData = SelectedVacancyStateViewData()
					success = m_TablesettingDatabaseAccess.DeleteVacancyStateData(SelectedData.ID)
					success = success AndAlso LoadVacancyStateList()

				Case SelectedModulKey.MODUL_VACANCY_GROUP
					Dim SelectedData = SelectedVacancyGroupViewData()
					success = m_TablesettingDatabaseAccess.DeleteVacancyGroupData(SelectedData.ID)
					success = success AndAlso LoadVacancyGroupList()


					' Offer
				Case SelectedModulKey.MODUL_OFFER_CONTACT
					Dim SelectedData = SelectedOfferContactViewData()
					success = m_TablesettingDatabaseAccess.DeleteOfferContactData(SelectedData.ID)
					success = success AndAlso LoadOfferContactList()

				Case SelectedModulKey.MODUL_OFFER_STATE
					Dim SelectedData = SelectedOfferStateViewData()
					success = m_TablesettingDatabaseAccess.DeleteOfferStateData(SelectedData.ID)
					success = success AndAlso LoadOfferStateList()

				Case SelectedModulKey.MODUL_OFFER_GROUP
					Dim SelectedData = SelectedOfferGroupViewData()
					success = m_TablesettingDatabaseAccess.DeleteOfferGroupData(SelectedData.ID)
					success = success AndAlso LoadOfferGroupList()



					' propose
				Case SelectedModulKey.MODUL_PROPOSE_STATE
					Dim SelectedData = SelectedProposeStateViewData()
					success = m_TablesettingDatabaseAccess.DeleteProposeStateData(SelectedData.recid)
					success = success AndAlso LoadProposeStateList()

				Case SelectedModulKey.MODUL_PROPOSE_EMPLOYEMENTTYPE
					Dim SelectedData = SelectedProposeEmployementTypeViewData()
					success = m_TablesettingDatabaseAccess.DeleteProposeEmployementTypeData(SelectedData.recid)
					success = success AndAlso LoadProposeEmployementTypeList()

				Case SelectedModulKey.MODUL_PROPOSE_ART
					Dim SelectedData = SelectedProposeArtViewData()
					success = m_TablesettingDatabaseAccess.DeleteProposeArtData(SelectedData.recid)
					success = success AndAlso LoadProposeArtList()


					' common tables
				Case SelectedModulKey.MODUL_CONTACT_CATEGORY
					Dim SelectedData = SelectedContactCategoryViewData
					success = m_TablesettingDatabaseAccess.DeleteContactCategoryData(SelectedData.ID)
					success = success AndAlso LoadContactCategoryList()

				Case SelectedModulKey.MODUL_EMPLOYEMENT_CATEGORIZATION
					Dim SelectedData = SelectedEmployementCategorizedViewData
					success = m_TablesettingDatabaseAccess.DeleteEmployementCategorizedData(SelectedData.ID)
					success = success AndAlso LoadEmployementCategorizedList()

				Case SelectedModulKey.MODUL_DIVERSE_COUNTRY
					Dim SelectedData = SelectedCountryViewData
					success = m_TablesettingDatabaseAccess.DeleteCountryData(SelectedData.recid)
					success = success AndAlso LoadCountryList()

				Case SelectedModulKey.MODUL_DIVERSE_BRANCHES
					Dim SelectedData = SelectedSectorViewData
					success = m_TablesettingDatabaseAccess.DeleteSectorData(SelectedData.recid)
					success = success AndAlso LoadSectorList()

				Case SelectedModulKey.MODUL_DIVERSE_QUALIFICATION
					Dim SelectedData = SelectedJobViewData
					success = m_TablesettingDatabaseAccess.DeleteJobData(SelectedData.recid)
					success = success AndAlso LoadJobList()

				Case SelectedModulKey.MODUL_DIVERSE_FCOSTCENTER
					Dim SelectedData = SelectedfCostcenterViewData
					success = m_TablesettingDatabaseAccess.DeleteCostCenter1Data(SelectedData.recId)
					success = success AndAlso LoadFCostcenterList()

				Case SelectedModulKey.MODUL_DIVERSE_SCOSTCENTER
					Dim SelectedData = SelectedSCostcenterViewData
					success = m_TablesettingDatabaseAccess.DeleteCostCenter2Data(SelectedData.recId)
					success = success AndAlso LoadSCostcenterList()

				Case SelectedModulKey.MODUL_DIVERSE_BVGGENTS
					Dim gender = "M"
					Dim SelectedData = SelectedBVGViewData
					success = m_TablesettingDatabaseAccess.DeleteBVGData(gender, SelectedData.recId)
					success = success AndAlso LoadBVGList(gender)

				Case SelectedModulKey.MODUL_DIVERSE_BVGFEMALE
					Dim gender = "F"
					Dim SelectedData = SelectedBVGViewData
					success = m_TablesettingDatabaseAccess.DeleteBVGData(gender, SelectedData.recId)
					success = success AndAlso LoadBVGList(gender)

				Case SelectedModulKey.MODUL_DIVERSE_FF13SALARY
					Dim SelectedData = SelectedFF13LohnViewData
					success = m_TablesettingDatabaseAccess.DeleteFF13LohnData(SelectedData.recId)
					success = success AndAlso LoadFF13LohnList()

				Case SelectedModulKey.MODUL_DIVERSE_QST
					Dim SelectedData = SelectedQSTInfoViewData
					success = m_TablesettingDatabaseAccess.DeleteQSTInfoData(SelectedData.recId)
					success = success AndAlso LoadQSTInfoList()

				Case SelectedModulKey.MODUL_DIVERSE_BUSINESSBRANCHS
					Dim SelectedData = SelectedBusinessBranchesViewData
					success = m_TablesettingDatabaseAccess.DeleteBusinessBranchsData(SelectedData.recid)
					success = success AndAlso LoadBusinessBranchsList()

				Case SelectedModulKey.MODUL_DIVERSE_FIBU
					Dim SelectedData = SelectedFibuKontenViewData
					success = m_TablesettingDatabaseAccess.DeleteFIBUKontenData(SelectedData.recid)
					success = success AndAlso LoadFibukontenList()

				Case SelectedModulKey.MODUL_DIVERSE_ABSENCE
					Dim SelectedData = SelectedAbsenceViewData
					success = m_TablesettingDatabaseAccess.DeleteAbsenceData(SelectedData.recid)
					success = success AndAlso LoadAbsenceList()

				Case SelectedModulKey.MODUL_DIVERSE_AGBWOS
					Dim SelectedData = SelectedTerminAndConditionsViewData
					success = m_TablesettingDatabaseAccess.DeleteTermsAndConditionsData(SelectedData.recid)
					success = success AndAlso LoadTermsandConditionsList()

				Case SelectedModulKey.MODUL_DIVERSE_SALUTATION
					Dim SelectedData = SelectedSalutationViewData
					success = m_TablesettingDatabaseAccess.DeleteSalutationData(SelectedData.recId)
					success = success AndAlso LoadSalutationList()

				Case SelectedModulKey.MODUL_DIVERSE_SMSTEMPLATE
					Dim SelectedData = SelectedSMSTemplateViewData
					success = m_TablesettingDatabaseAccess.DeleteSMSTemplateData(SelectedData.recid)
					success = success AndAlso LoadSMSTemplatesList()

				Case SelectedModulKey.MODUL_MAIN_PRINTTEMPLATES
					Dim SelectedData = SelectedPrintTemplatesViewData()
					success = m_TablesettingDatabaseAccess.DeletePrintTemplatesData(SelectedData.recid)
					success = success AndAlso LoadPrintTemplatesList()

				Case SelectedModulKey.MODUL_MAIN_EXPORTTEMPLATES
					Dim SelectedData = SelectedExportTemplatesViewData()
					success = m_TablesettingDatabaseAccess.DeleteExportTemplatesData(SelectedData.recid)
					success = success AndAlso LoadExportTemplatesList()

				Case Else
					m_UtilityUI.ShowErrorDialog(String.Format("Methode was not implemented!{0}{1}", vbNewLine, m_Selectedmodul.ToString))
					success = False

			End Select


		Catch ex As Exception
			success = False

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

		Return success

	End Function

	Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click

		grdTableContent.FocusedView.CloseEditor()

		Dim success = (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Wollen Sie den Datensatz wirklich löschen?"),
																									m_Translate.GetSafeTranslationValue("Datensatz löschen")))
		If Not success Then Return
		success = success AndAlso DeleteRecord()
		If success Then
			m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Die Daten wurden gelöscht."))
		Else
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gelöscht werden."))
		End If

	End Sub

	Private Sub gvTableContent_EditFormShowing(sender As Object, e As EditFormShowingEventArgs) Handles gvTableContent.EditFormShowing

		Select Case m_Selectedmodul
			Case SelectedModulKey.MODUL_EMPLOYEE_DOCUMENTCATEGORY
				Dim SelectedData = SelectedEmployeeDocumentCategoryViewData()
				If Not SelectedData Is Nothing AndAlso IsNumberInRange(SelectedData.CategoryNumber.GetValueOrDefault(0), 200, 250) Then
					Dim msg As String = "Die Kategorie-Nummern zwischen 200 und 250 sind vom System reserviert. Ihr Datensatz kann nicht verändert werden."
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(msg))
					e.Allow = False
				End If

			Case SelectedModulKey.MODUL_CUSTOMER_DOCUMENTCATEGORY
				Dim SelectedData = SelectedCustomerDocumentCategoryViewData()
				If Not SelectedData Is Nothing AndAlso IsNumberInRange(SelectedData.CategoryNumber.GetValueOrDefault(0), 100, 150) Then
					Dim msg As String = "Die Kategorie-Nummern zwischen 100 und 150 sind vom System reserviert. Ihr Datensatz kann nicht verändert werden."
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(msg))
					e.Allow = False
				End If

		End Select

	End Sub

	Private Sub gvTableContent_RowUpdated(sender As Object, e As RowObjectEventArgs) Handles gvTableContent.RowUpdated
		Dim success As Boolean = True

		grdTableContent.FocusedView.CloseEditor()
		success = success AndAlso UpdateRecord(e.Row)

		If success Then
			m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Die Daten wurden gespeichert."))
		Else

			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))
		End If

	End Sub


	Private Sub OngvTableContent_ValidateRow(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs) Handles gvTableContent.ValidateRow
		Dim success As Boolean = True
		Dim errEmployeeDocCategory As String = "Die Kategorie-Nummern zwischen 200 und 250 sind vom System reserviert. Ihr Datensatz kann nicht verändert werden."
		Dim errCustomerDocCategory As String = "Die Kategorie-Nummern zwischen 100 und 150 sind vom System reserviert. Ihr Datensatz kann nicht verändert werden."

		Dim View As GridView = CType(sender, GridView)

		success = success AndAlso Validate(e.Row)

		If Not success Then
			e.Valid = False
			'Set errors with specific descriptions for the columns

			Select Case m_Selectedmodul
				Case SelectedModulKey.MODUL_EMPLOYEE_DOCUMENTCATEGORY
					Dim col As GridColumn = View.Columns("CategoryNumber")
					View.SetColumnError(col, errEmployeeDocCategory)

				Case SelectedModulKey.MODUL_CUSTOMER_DOCUMENTCATEGORY
					Dim col As GridColumn = View.Columns("CategoryNumber")
					View.SetColumnError(col, errCustomerDocCategory)

			End Select

		End If

	End Sub

	Private Sub OngvTableContent_InvalidRowException(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs) Handles gvTableContent.InvalidRowException
		'Suppress displaying the error message box
		e.ExceptionMode = ExceptionMode.NoAction
	End Sub




#Region "helpers"

	Private Function IsNumberInRange(myNumber As Integer, fromNumber As Integer, toNumber As Integer) As Boolean
		Dim result As Boolean
		result = (myNumber >= fromNumber And myNumber <= toNumber)


		Return result

	End Function

	Private Class YearValueView
		Public Property Value As Integer

	End Class


#End Region




End Class