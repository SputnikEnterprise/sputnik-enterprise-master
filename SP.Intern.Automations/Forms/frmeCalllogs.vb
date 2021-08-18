
Imports SP.DatabaseAccess.Employee
Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports System.ComponentModel
Imports System.Threading.Tasks
Imports System.Threading
Imports SP.Internal.Automations.Settings_
Imports SP.Internal.Automations.SPCustomerPaymentServicesWebService
Imports SP.Internal.Automations.SPeCallWebService
Imports DevExpress.LookAndFeel
Imports SPProgUtility.Mandanten
Imports SPProgUtility.CommonXmlUtility
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Listing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Columns
Imports SP.DatabaseAccess.Listing.DataObjects
Imports DevExpress.XtraSplashScreen
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
'Imports SP.KD.CPersonMng.UI
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports SP.Internal.Automations.SPWOSCustomerWebService
Imports SP.Internal.Automations.SPWOSEmployeeWebService
Imports SP.Internal.Automations.SPNotificationWebService
Imports System.Net
Imports System.ServiceModel

''' <summary>
''' Search bank data.
''' </summary>
Public Class frmeCalllogs

#Region "Private Consts"

	Private Const DEFAULT_SPUTNIK_BANK_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPCustomerPaymentServices.asmx"
	Private Const DEFAULT_SPUTNIK_IBAN_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPIBANUtil.asmx"
	Private Const DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPNotification.asmx"
	Public Const DEFAULT_SPUTNIK_CUSTOMERWOS_WEBSERVICE_URI As String = "wsSPS_services/SPWOSCustomerUtilities.asmx"
	Private Const DEFAULT_SPUTNIK_EMPLOYEEWOS_WEBSERVICE_URI As String = "wsSPS_services/SPWOSEmployeeUtilities.asmx"

	Private Const MANDANT_XML_SETTING_SPUTNIK_PAYMENT_UTIL_WEBSERVICE_URI As String = "MD_{0}/Interfaces/webservices/webservicepaymentservices"
	Private Const MANDANT_XML_SETTING_SPUTNIK_ECALL_URI As String = "https://soap.ecall.ch/eCall.asmx"

	Private Const MANDANT_XML_SETTING_SPUTNIK_ECALL_USERNAME As String = "MD_{0}/Mailing/faxusername"
	Private Const MANDANT_XML_SETTING_SPUTNIK_ECALL_USERPW As String = "MD_{0}/Mailing/faxuserpw"
	Private Const MANDANT_XML_SETTING_WOS_EMPLOYEE_GUID As String = "MD_{0}/Export/MA_SPUser_ID"
	Private Const MANDANT_XML_SETTING_WOS_CUSTOMER_GUID As String = "MD_{0}/Export/KD_SPUser_ID"

#End Region

#Region "Privte Fields"

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' The employee data access object.
	''' </summary>
	Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

	''' <summary>
	''' The Listing data access object.
	''' </summary>
	Private m_ListingDatabaseAccess As IListingDatabaseAccess

	Private m_PaymentUtilWebServiceUri As String

	''' <summary>
	''' Service Uri of Sputnik bank util webservice.
	''' </summary>
	Private m_CustomerWosUtilWebServiceUri As String

	''' <summary>
	''' Service Uri of Sputnik bank util webservice.
	''' </summary>
	Private m_EmployeeWosUtilWebServiceUri As String
	Private m_NotificationUtilWebServiceUri As String

	'''<summary>
	'''Service Uri of eCall webservice.
	'''</summary>
	Private m_eCallWebServiceUri As String

	Private m_AccountName As String
	Private m_AccountPassword As String

	''' <summary>
	''' The settings manager.
	''' </summary>
	Protected m_SettingsManager As ISettingsManager

	''' <summary>
	''' The SPProgUtility object.
	''' </summary>
	Private m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI


	''' <summary>
	''' The common database access.
	''' </summary>
	Protected m_CommonDatabaseAccess As ICommonDatabaseAccess

	''' <summary>
	''' Settings xml.
	''' </summary>
	Private m_MandantSettingsXml As SettingsXml

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
	''' </summary>
	Private m_SuppressUIEvents As Boolean = False

	''' <summary>
	''' The mandant.
	''' </summary>
	Private m_MandantData As Mandant

	Private m_CurrentJobID As String
	Private m_eCallMessageInfo As MessageInfo

	''' <summary>
	''' is wos allowed
	''' </summary>
	Private m_WOSFunctionalityAllowed As Boolean
	Private m_EmployeeWOSID As String
	Private m_CustomerWOSID As String

#End Region


#Region "Constructor"

	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
		m_InitializationData = _setting
		m_SettingsManager = New SettingsManager
		m_MandantData = New Mandant
		m_UtilityUI = New UtilityUI

		m_CommonDatabaseAccess = New CommonDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)
		m_ListingDatabaseAccess = New ListingDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)

		m_SuppressUIEvents = True

		' Dieser Aufruf ist für den Designer erforderlich.
		InitializeComponent()

		Try
			m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))

			m_AccountName = m_ClsProgSetting.DecryptString(m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_ECALL_USERNAME, m_InitializationData.MDData.MDNr)))
			m_AccountPassword = m_ClsProgSetting.DecryptString(m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_ECALL_USERPW, m_InitializationData.MDData.MDNr)))

			m_eCallWebServiceUri = MANDANT_XML_SETTING_SPUTNIK_ECALL_URI

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try
		gvCurrentList.OptionsView.ShowIndicator = False


		Dim domainName As String = m_InitializationData.MDData.WebserviceDomain ' "http://asmx.domain.com"

		m_NotificationUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI)
		m_PaymentUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_BANK_UTIL_WEBSERVICE_URI)
		m_CustomerWosUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_CUSTOMERWOS_WEBSERVICE_URI)
		m_EmployeeWosUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_EMPLOYEEWOS_WEBSERVICE_URI)


		Reset()

		m_EmployeeWOSID = EmployeeWOSID
		m_CustomerWOSID = CustomerWOSID
		m_WOSFunctionalityAllowed = (m_EmployeeWOSID & m_CustomerWOSID).Length > 50

		LoadDropDownData()

		AddHandler lueAdvisor.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueMonth.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler dateEditServiceDate.ButtonClick, AddressOf OnDropDown_ButtonClick

		AddHandler Me.gvCurrentList.ColumnFilterChanged, AddressOf OnGVDetail_ColumnFilterChanged
		AddHandler Me.gvPaidlist.ColumnFilterChanged, AddressOf OnGVDetail_ColumnFilterChanged

	End Sub

#End Region

#Region "private properties"

	Private ReadOnly Property EmployeeWOSID() As String
		Get
			Dim value = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_WOS_EMPLOYEE_GUID, m_InitializationData.MDData.MDNr))

			Return value
		End Get
	End Property

	Private ReadOnly Property CustomerWOSID() As String
		Get
			Dim value = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_WOS_CUSTOMER_GUID, m_InitializationData.MDData.MDNr))

			Return value
		End Get
	End Property

#End Region


#Region "Public Properties"

	''' <summary>
	''' Gets the selected bank.
	''' </summary>
	''' <returns>The selected bank or nothing if none is selected.</returns>
	Public ReadOnly Property SelectedBankData As PaymentSearchViewData
		Get
			Dim grdView = TryCast(grdCurrentList.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim bank = CType(grdView.GetRow(selectedRows(0)), PaymentSearchViewData)
					Return bank
				End If

			End If

			Return Nothing
		End Get

	End Property

#End Region


#Region "Public Methods"

	Public Sub LoadData()
		Dim data = CType(lueServiceName.GetSelectedDataRow, ServiceNameViewData)
		SearchPaidlistViaWebService()

		If data Is Nothing Then Return
		bsiDefinitivCount.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
		bsiPaidReccount.Visibility = DevExpress.XtraBars.BarItemVisibility.Never

		Select Case data.ItemValue
			Case "Interne Nachrichten (EMail und Telefax)"
				ResetSentGrid()
				PerformSentMessageCallAsync()

			Case "CVLIZER_SCAN"
				ResetCVLizerGrid()
				SearchCVLizerlistViaWebService()

			Case "WOSEmployee"
				ResetWOSEmployeeDocGrid()
				ResetWOSMessageGrid()
				SearchWOSEmployeeDoclistViaWebService()
				SearchWOSNotificationlistViaWebService()

			Case "WOSCustomer"
				ResetWOSCustomerDocGrid()
				ResetWOSMessageGrid()
				SearchWOSCustomerDoclistViaWebService()
				SearchWOSNotificationlistViaWebService()

			Case Else
				ResetCurrentGrid()
				SearchCurrentlistViaWebService()

		End Select
		SetupUI()

	End Sub


#End Region

#Region "Private Methods"

	''' <summary>
	'''  Translate controls
	''' </summary>
	Private Sub TranslateControls()

		Text = m_Translate.GetSafeTranslationValue(Text)

		btnSearch.Text = m_Translate.GetSafeTranslationValue(btnSearch.Text)
		btnClose.Text = m_Translate.GetSafeTranslationValue(btnClose.Text)

		grpFilter.Text = m_Translate.GetSafeTranslationValue(grpFilter.Text)
		grpDetails.Text = m_Translate.GetSafeTranslationValue(grpDetails.Text)

		lblBeraterIn.Text = m_Translate.GetSafeTranslationValue(lblBeraterIn.Text)
		lblDienstleistung.Text = m_Translate.GetSafeTranslationValue(lblDienstleistung.Text)
		lblJahr.Text = m_Translate.GetSafeTranslationValue(lblJahr.Text)
		lblMonat.Text = m_Translate.GetSafeTranslationValue(lblMonat.Text)
		lblDatum.Text = m_Translate.GetSafeTranslationValue(lblDatum.Text)

		xtabAktuellList.Text = m_Translate.GetSafeTranslationValue(xtabAktuellList.Text)
		xtabDefinitiv.Text = m_Translate.GetSafeTranslationValue(xtabDefinitiv.Text)

		bsiCurrentCount.Caption = m_Translate.GetSafeTranslationValue(bsiCurrentCount.Caption)
		bsiDefinitivCount.Caption = m_Translate.GetSafeTranslationValue(bsiDefinitivCount.Caption)
		bbiPrint.Caption = m_Translate.GetSafeTranslationValue(bbiPrint.Caption)

	End Sub

	''' <summary>
	''' Resets the form.
	''' </summary>
	Private Sub Reset()

		lbleCallResponseInfo.Text = String.Empty
		m_CurrentJobID = String.Empty
		btnUpdateeCallResponse.Visible = m_InitializationData.UserData.UserNr = 1

		ResetCustomerDropDown()
		ResetAdvisorDropDown()
		ResetServiceNameDropDown()
		ResetYearDropDown()
		ResetMonthDropDown()

		bsiCurrentReccount.Caption = String.Empty
		bsiPaidReccount.Caption = String.Empty

		' ---Reset drop downs, grids and lists---
		ResetPaidGrid()
		ResetCurrentGrid()
		ResetWOSMessageGrid()

		' Translate controls
		TranslateControls()

	End Sub

	Private Sub SetupUI()
		Dim data = CType(lueServiceName.GetSelectedDataRow, ServiceNameViewData)

		xtabDefinitiv.PageVisible = (data.ItemValue = "ECALL_FAXCREDIT" OrElse data.ItemValue = "ECALL_SMSCREDIT")
		xtabWOS.PageVisible = (data.ItemValue = "WOSEmployee" OrElse data.ItemValue = "WOSCustomer")
		xtabAktuellList.PageVisible = Not (data.ItemValue = "WOSEmployee" OrElse data.ItemValue = "WOSCustomer")

		Select Case data.ItemValue
			Case "Interne Nachrichten (EMail und Telefax)", "CVLIZER_SCAN"
				xtabMain.SelectedTabPage = xtabAktuellList

			Case "WOSEmployee", "WOSCustomer"
				xtabMain.SelectedTabPage = xtabWOS

				lblBeraterIn.Visible = False
				lueAdvisor.Visible = False
				lblDatum.Visible = False
				dateEditServiceDate.Visible = False


			Case Else
				gvCurrentList.Columns("BookedPayment").Visible = (data.ItemValue = "SOLVENCY_QUICK_CHECK" OrElse data.ItemValue = "SOLVENCY_BUSINESS_CHECK")
				gvCurrentList.Columns("BookedDate").Visible = (data.ItemValue = "SOLVENCY_QUICK_CHECK" OrElse data.ItemValue = "SOLVENCY_BUSINESS_CHECK")

				xtabMain.SelectedTabPage = xtabAktuellList
				lblBeraterIn.Visible = True
				lueAdvisor.Visible = True
				lblDatum.Visible = True
				dateEditServiceDate.Visible = True

		End Select

	End Sub

	''' <summary>
	''' Resets the advisors drop down.
	''' </summary>
	Private Sub ResetAdvisorDropDown()

		lueAdvisor.Properties.DropDownRows = 20

		lueAdvisor.Properties.DisplayMember = "UserName" ' "LastName_FirstNameWithComa"
		lueAdvisor.Properties.ValueMember = "UserName" '  "FirstName_LastName"

		Dim columns = lueAdvisor.Properties.Columns
		columns.Clear()
		'columns.Add(New LookUpColumnInfo("KST", 0))
		columns.Add(New LookUpColumnInfo("UserName", 0, m_Translate.GetSafeTranslationValue("BeraterIn")))

		lueAdvisor.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueAdvisor.Properties.SearchMode = SearchMode.AutoComplete
		lueAdvisor.Properties.AutoSearchColumnIndex = 1

		lueAdvisor.Properties.NullText = String.Empty
		lueAdvisor.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets the Day, Month, Std drop down data.
	''' </summary>
	Private Sub ResetServiceNameDropDown()

		lueServiceName.Properties.DisplayMember = "DisplayText"
		lueServiceName.Properties.ValueMember = "ItemValue"

		Dim columns = lueServiceName.Properties.Columns
		columns.Clear()

		columns.Add(New LookUpColumnInfo("DisplayText", 0))

		lueServiceName.Properties.ShowHeader = False
		lueServiceName.Properties.ShowFooter = False
		lueServiceName.Properties.DropDownRows = 10
		lueServiceName.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueServiceName.Properties.SearchMode = SearchMode.AutoComplete
		lueServiceName.Properties.AutoSearchColumnIndex = 0
		lueServiceName.Properties.NullText = String.Empty

		Dim suppressUIEventsState = m_SuppressUIEvents
		m_SuppressUIEvents = True
		lueServiceName.EditValue = Nothing
		m_SuppressUIEvents = suppressUIEventsState

	End Sub


	''' <summary>
	''' Resets the year drop down.
	''' </summary>
	Private Sub ResetYearDropDown()

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
	''' Resets the month drop down.
	''' </summary>
	Private Sub ResetMonthDropDown()

		lueMonth.Properties.DisplayMember = "Value"
		lueMonth.Properties.ValueMember = "Value"
		lueMonth.Properties.ShowHeader = False

		lueMonth.Properties.Columns.Clear()
		lueMonth.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "Value",
																					 .Width = 100,
																					 .Caption = m_Translate.GetSafeTranslationValue("Value")})

		lueMonth.Properties.ShowFooter = False
		lueMonth.Properties.DropDownRows = 10
		lueMonth.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueMonth.Properties.SearchMode = SearchMode.AutoComplete
		lueMonth.Properties.AutoSearchColumnIndex = 0

		lueMonth.Properties.NullText = String.Empty
		lueMonth.EditValue = Nothing
	End Sub


	''' <summary>
	''' Loads the drop down data.
	''' </summary>
	''' <returns>Boolean value indicating success.</returns>
	Private Function LoadDropDownData() As Boolean
		Dim success As Boolean = True

		m_SuppressUIEvents = True
		'success = success AndAlso LoadServiceNameDropDownData()
		'm_SuppressUIEvents = False

		SearchCustomerlistViaWebService()

		m_SuppressUIEvents = True

		success = success AndAlso LoadAdvisorDropDownData()
		success = success AndAlso LoadServiceNameDropDownData()
		success = success AndAlso LoadYearDropDownData()
		success = success AndAlso LoadMonthDropDownData()

		m_SuppressUIEvents = False

		Return success

	End Function

	''' <summary>
	''' Loads the advisor drop down data.
	''' </summary>
	'''<returns>Boolean flag indicating success.</returns>
	Private Function LoadAdvisorDropDownData() As Boolean

		If Not lueCustomer.EditValue Is Nothing Then
			Dim result = PerformUserNamelistWebserviceCallAsync()

			lueAdvisor.Properties.DataSource = result
			lueAdvisor.Properties.ForceInitialize()

			Return Not result Is Nothing
		End If

		Dim userDataList = m_CommonDatabaseAccess.LoadAdvisorData()

		Dim advisorViewDataList As New List(Of AdvisorViewData)

		If Not userDataList Is Nothing Then
			For Each userData In userDataList
				Dim advisorViewData As AdvisorViewData = New AdvisorViewData
				advisorViewData.KST = userData.KST
				advisorViewData.FirstName = userData.Firstname
				advisorViewData.LastName = userData.Lastname

				advisorViewDataList.Add(advisorViewData)
			Next
		Else
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Beraterdaten konnten nicht geladen werden."))
		End If

		lueAdvisor.Properties.DataSource = advisorViewDataList
		lueAdvisor.Properties.ForceInitialize()

		Return Not userDataList Is Nothing
	End Function

	''' <summary>
	''' Loads the service drop down data.
	''' </summary>
	Private Function LoadServiceNameDropDownData() As Boolean
		Dim serviceNameData = New List(Of ServiceNameViewData) From {
		New ServiceNameViewData With {.ItemValue = "SOLVENCY_QUICK_CHECK", .DisplayText = m_Translate.GetSafeTranslationValue("LOG: Bonitätsprüfung: Quick-Check")},
		New ServiceNameViewData With {.ItemValue = "SOLVENCY_BUSINESS_CHECK", .DisplayText = m_Translate.GetSafeTranslationValue("LOG: Bonitätsprüfung: Business-Check")},
		New ServiceNameViewData With {.ItemValue = "ECALL_FAXCREDIT", .DisplayText = m_Translate.GetSafeTranslationValue("LOG: Fax-Nachrichten")},
		New ServiceNameViewData With {.ItemValue = "ECALL_SMSCREDIT", .DisplayText = m_Translate.GetSafeTranslationValue("LOG: SMS-Nachrichten")},
		New ServiceNameViewData With {.ItemValue = "Interne Nachrichten (EMail und Telefax)", .DisplayText = m_Translate.GetSafeTranslationValue("Versandte EMail und Telefax-Nachrichten")},
		New ServiceNameViewData With {.ItemValue = "WOSEmployee", .DisplayText = m_Translate.GetSafeTranslationValue("WOS-Kandidaten (Dokumente und Benachrichtigungen)")},
		New ServiceNameViewData With {.ItemValue = "WOSCustomer", .DisplayText = m_Translate.GetSafeTranslationValue("WOS-Kunden (Dokumente und Benachrichtigungen)")},
		New ServiceNameViewData With {.ItemValue = "CVLIZER_SCAN", .DisplayText = m_Translate.GetSafeTranslationValue("Geparsten Lebensläufe")}
	}

		lueServiceName.Properties.DataSource = serviceNameData
		Dim myLastservice As String = My.Settings.ecallLogs_lastselectedservice
		If String.IsNullOrWhiteSpace(myLastservice) Then
			lueServiceName.EditValue = "ECALL_SMSCREDIT"
		Else
			lueServiceName.EditValue = myLastservice
		End If

		lueServiceName.Properties.ForceInitialize()

		Return True
	End Function

	''' <summary>
	''' Loads the year drop down data.
	''' </summary>
	Private Function LoadYearDropDownData() As Boolean

		Dim success As Boolean = True

		Dim wrappedValues As List(Of IntegerValueViewWrapper) = Nothing

		If Not m_InitializationData Is Nothing Then

			Dim yearData = m_CommonDatabaseAccess.LoadMandantYears(m_InitializationData.MDData.MDNr)

			If (yearData Is Nothing) Then
				success = False
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Jahre (Mandanten) konnten nicht geladen werden."))
			End If

			If Not yearData Is Nothing Then
				wrappedValues = New List(Of IntegerValueViewWrapper)

				For Each yearValue In yearData
					wrappedValues.Add(New IntegerValueViewWrapper With {.Value = yearValue})
				Next

			End If

		End If

		lueYear.EditValue = Nothing
		lueYear.Properties.DataSource = wrappedValues
		lueYear.EditValue = Now.Year
		lueYear.Properties.ForceInitialize()

		Return success
	End Function

	''' <summary>
	''' Loads the month drop down data.
	''' </summary>
	Private Function LoadMonthDropDownData() As Boolean

		Dim success As Boolean = True

		Dim year = lueYear.EditValue

		Dim wrappedValues As List(Of IntegerValueViewWrapper) = Nothing

		If Not m_InitializationData Is Nothing And
			 Not year Is Nothing Then

			wrappedValues = New List(Of IntegerValueViewWrapper)
			For i As Integer = 1 To 12
				wrappedValues.Add(New IntegerValueViewWrapper With {.Value = i})
			Next

		End If

		lueMonth.EditValue = Nothing
		lueMonth.Properties.DataSource = wrappedValues
		lueMonth.EditValue = Now.Month
		lueMonth.Properties.ForceInitialize()

		Return success
	End Function




	''' <summary>
	''' Resets the Customer grid.
	''' </summary>
	Private Sub ResetCustomerDropDown()

		' Reset the grid
		lueCustomer.Properties.DisplayMember = "customer_Name"
		lueCustomer.Properties.ValueMember = "customer_ID"

		gvCustomer.OptionsView.ShowIndicator = False
		gvCustomer.OptionsView.ShowColumnHeaders = True
		gvCustomer.OptionsView.ShowFooter = False

		gvCustomer.OptionsView.ShowAutoFilterRow = True
		gvCustomer.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvCustomer.Columns.Clear()

		Dim columnEmployeeNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeeNumber.Caption = m_Translate.GetSafeTranslationValue("Mandantenname")
		columnEmployeeNumber.Name = "customer_Name"
		columnEmployeeNumber.FieldName = "customer_Name"
		columnEmployeeNumber.Visible = True
		columnEmployeeNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvCustomer.Columns.Add(columnEmployeeNumber)

		Dim columnLastnameFirstname As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLastnameFirstname.Caption = m_Translate.GetSafeTranslationValue("Mandanten-ID")
		columnLastnameFirstname.Name = "customer_ID"
		columnLastnameFirstname.FieldName = "customer_ID"
		columnLastnameFirstname.Visible = True
		columnLastnameFirstname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvCustomer.Columns.Add(columnLastnameFirstname)


		m_SuppressUIEvents = True
		lueCustomer.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueCustomer.Properties.NullText = String.Empty
		lueCustomer.EditValue = Nothing

		m_SuppressUIEvents = False

	End Sub

	''' <summary>
	''' Resets the payment grid.
	''' </summary>
	Private Sub ResetCurrentGrid()

		' Reset the grid
		gvCurrentList.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvCurrentList.OptionsView.ShowIndicator = False
		gvCurrentList.OptionsView.ShowAutoFilterRow = True
		gvCurrentList.OptionsView.ColumnAutoWidth = False
		Dim showFooter As Boolean = False
		gvCurrentList.OptionsView.ShowFooter = showFooter

		gvCurrentList.Columns.Clear()

		Dim columnRecID As New DevExpress.XtraGrid.Columns.GridColumn()
		columnRecID.Caption = m_Translate.GetSafeTranslationValue("ID")
		columnRecID.Name = "RecID"
		columnRecID.FieldName = "RecID"
		columnRecID.Width = 60
		columnRecID.Visible = False
		gvCurrentList.Columns.Add(columnRecID)

		Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCreatedFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCreatedFrom.Caption = m_Translate.GetSafeTranslationValue("BeraterIn")
		columnCreatedFrom.Name = "CreatedFrom"
		columnCreatedFrom.FieldName = "CreatedFrom"
		columnCreatedFrom.Width = 150
		columnCreatedFrom.Visible = True
		gvCurrentList.Columns.Add(columnCreatedFrom)

		Dim columnServiceDate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnServiceDate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnServiceDate.Caption = m_Translate.GetSafeTranslationValue("Datum")
		columnServiceDate.Name = "ServiceDate"
		columnServiceDate.FieldName = "ServiceDate"
		columnServiceDate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
		columnServiceDate.AppearanceHeader.Options.UseTextOptions = True
		columnServiceDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columnServiceDate.DisplayFormat.FormatString = "G"
		columnServiceDate.Width = 120
		columnServiceDate.Visible = True
		gvCurrentList.Columns.Add(columnServiceDate)

		Dim columnServicename As New DevExpress.XtraGrid.Columns.GridColumn()
		columnServicename.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnServicename.Caption = m_Translate.GetSafeTranslationValue("Dienstleistung")
		columnServicename.Name = "ServiceName"
		columnServicename.FieldName = "ServiceName"
		columnServicename.Width = 200
		columnServicename.Visible = True
		gvCurrentList.Columns.Add(columnServicename)

		Dim columnJobID As New DevExpress.XtraGrid.Columns.GridColumn()
		columnJobID.Caption = m_Translate.GetSafeTranslationValue("Kennung")
		columnJobID.Name = "JobID"
		columnJobID.FieldName = "JobID"
		columnJobID.Width = 50
		columnJobID.Visible = False
		gvCurrentList.Columns.Add(columnJobID)

		Dim columnAuthorizedItems As New DevExpress.XtraGrid.Columns.GridColumn()
		columnAuthorizedItems.Caption = m_Translate.GetSafeTranslationValue("Anzahl")
		columnAuthorizedItems.Name = "AuthorizedItems"
		columnAuthorizedItems.FieldName = "AuthorizedItems"
		columnAuthorizedItems.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnAuthorizedItems.AppearanceHeader.Options.UseTextOptions = True
		columnAuthorizedItems.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnAuthorizedItems.DisplayFormat.FormatString = "N5"
		columnAuthorizedItems.Width = 100
		columnAuthorizedItems.Visible = showFooter
		columnAuthorizedItems.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		gvCurrentList.Columns.Add(columnAuthorizedItems)

		Dim columnAuthorizedCredit As New DevExpress.XtraGrid.Columns.GridColumn()
		columnAuthorizedCredit.Caption = m_Translate.GetSafeTranslationValue("Anzahl Credits")
		columnAuthorizedCredit.Name = "AuthorizedCredit"
		columnAuthorizedCredit.FieldName = "AuthorizedCredit"
		columnAuthorizedCredit.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnAuthorizedCredit.AppearanceHeader.Options.UseTextOptions = True
		columnAuthorizedCredit.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnAuthorizedCredit.DisplayFormat.FormatString = "N2"
		columnAuthorizedCredit.Width = 100
		columnAuthorizedCredit.Visible = False
		columnAuthorizedCredit.OptionsColumn.ShowInCustomizationForm = m_InitializationData.UserData.UserNr = 1
		columnAuthorizedCredit.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		gvCurrentList.Columns.Add(columnAuthorizedCredit)

		Dim columnBookedPayment As New DevExpress.XtraGrid.Columns.GridColumn()
		columnBookedPayment.Caption = m_Translate.GetSafeTranslationValue("Verrechnet?")
		columnBookedPayment.Name = "BookedPayment"
		columnBookedPayment.FieldName = "BookedPayment"
		columnBookedPayment.Width = 100
		columnBookedPayment.Visible = True
		gvCurrentList.Columns.Add(columnBookedPayment)

		Dim columnBookedDate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnBookedDate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnBookedDate.Caption = m_Translate.GetSafeTranslationValue("Fakturadatum")
		columnBookedDate.Name = "BookedDate"
		columnBookedDate.FieldName = "BookedDate"
		columnBookedDate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
		columnBookedDate.AppearanceHeader.Options.UseTextOptions = True
		columnBookedDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columnBookedDate.DisplayFormat.FormatString = "G"
		columnBookedDate.Width = 120
		columnBookedDate.Visible = True
		gvCurrentList.Columns.Add(columnBookedDate)


		m_SuppressUIEvents = True
		grdCurrentList.DataSource = Nothing
		m_SuppressUIEvents = False

	End Sub

	Private Sub ResetSentGrid()

		' Reset the grid
		gvCurrentList.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvCurrentList.OptionsView.ShowIndicator = False
		gvCurrentList.OptionsView.ShowAutoFilterRow = True
		gvCurrentList.OptionsView.ColumnAutoWidth = False
		gvCurrentList.OptionsView.ShowFooter = False

		gvCurrentList.Columns.Clear()

		Dim columnRecID As New DevExpress.XtraGrid.Columns.GridColumn()
		columnRecID.Caption = m_Translate.GetSafeTranslationValue("ID")
		columnRecID.Name = "ID"
		columnRecID.FieldName = "ID"
		columnRecID.Width = 60
		columnRecID.Visible = False
		gvCurrentList.Columns.Add(columnRecID)

		Dim columnrecNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnrecNr.Caption = m_Translate.GetSafeTranslationValue("recNr")
		columnrecNr.Name = "recNr"
		columnrecNr.FieldName = "recNr"
		columnrecNr.Width = 60
		columnrecNr.Visible = False
		gvCurrentList.Columns.Add(columnrecNr)

		Dim columnKDNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnKDNr.Caption = m_Translate.GetSafeTranslationValue("KDNr")
		columnKDNr.Name = "KDNr"
		columnKDNr.FieldName = "KDNr"
		columnKDNr.Width = 60
		columnKDNr.Visible = False
		gvCurrentList.Columns.Add(columnKDNr)

		Dim columnZHDNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnZHDNr.Caption = m_Translate.GetSafeTranslationValue("ZHDNr")
		columnZHDNr.Name = "ZHDNr"
		columnZHDNr.FieldName = "ZHDNr"
		columnZHDNr.Width = 60
		columnZHDNr.Visible = False
		gvCurrentList.Columns.Add(columnZHDNr)

		Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMANr.Caption = m_Translate.GetSafeTranslationValue("MANr")
		columnMANr.Name = "MANr"
		columnMANr.FieldName = "MANr"
		columnMANr.Width = 60
		columnMANr.Visible = False
		gvCurrentList.Columns.Add(columnMANr)

		Dim columnemployeeFullname As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeeFullname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemployeeFullname.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
		columnemployeeFullname.Name = "employeeFullname"
		columnemployeeFullname.FieldName = "employeeFullname"
		columnemployeeFullname.Width = 150
		columnemployeeFullname.Visible = False
		gvCurrentList.Columns.Add(columnemployeeFullname)

		Dim columncustomername As New DevExpress.XtraGrid.Columns.GridColumn()
		columncustomername.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncustomername.Caption = m_Translate.GetSafeTranslationValue("Kunde")
		columncustomername.Name = "customername"
		columncustomername.FieldName = "customername"
		columncustomername.Width = 150
		columncustomername.Visible = True
		gvCurrentList.Columns.Add(columncustomername)

		Dim columnresponsiblePersonFullname As New DevExpress.XtraGrid.Columns.GridColumn()
		columnresponsiblePersonFullname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnresponsiblePersonFullname.Caption = m_Translate.GetSafeTranslationValue("Zuständige Person")
		columnresponsiblePersonFullname.Name = "responsiblePersonFullname"
		columnresponsiblePersonFullname.FieldName = "responsiblePersonFullname"
		columnresponsiblePersonFullname.Width = 150
		columnresponsiblePersonFullname.Visible = True
		gvCurrentList.Columns.Add(columnresponsiblePersonFullname)

		Dim columnemail_to As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemail_to.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemail_to.Caption = m_Translate.GetSafeTranslationValue("email_to")
		columnemail_to.Name = "email_to"
		columnemail_to.FieldName = "email_to"
		columnemail_to.Width = 150
		columnemail_to.Visible = True
		gvCurrentList.Columns.Add(columnemail_to)

		Dim columnemail_from As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemail_from.Caption = m_Translate.GetSafeTranslationValue("email_from")
		columnemail_from.Name = "email_from"
		columnemail_from.FieldName = "email_from"
		columnemail_from.Width = 150
		columnemail_from.Visible = True
		gvCurrentList.Columns.Add(columnemail_from)

		Dim columnemail_subject As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemail_subject.Caption = m_Translate.GetSafeTranslationValue("email_subject")
		columnemail_subject.Name = "email_subject"
		columnemail_subject.FieldName = "email_subject"
		columnemail_subject.Width = 200
		columnemail_subject.Visible = True
		gvCurrentList.Columns.Add(columnemail_subject)

		Dim columnmessageID As New DevExpress.XtraGrid.Columns.GridColumn()
		columnmessageID.Caption = m_Translate.GetSafeTranslationValue("messageID")
		columnmessageID.Name = "messageID"
		columnmessageID.FieldName = "messageID"
		columnmessageID.Width = 30
		columnmessageID.Visible = False
		gvCurrentList.Columns.Add(columnmessageID)

		Dim columnServiceDate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnServiceDate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnServiceDate.Caption = m_Translate.GetSafeTranslationValue("Datum")
		columnServiceDate.Name = "createdon"
		columnServiceDate.FieldName = "createdon"
		columnServiceDate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
		columnServiceDate.AppearanceHeader.Options.UseTextOptions = True
		columnServiceDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columnServiceDate.DisplayFormat.FormatString = "G"
		columnServiceDate.Width = 120
		columnServiceDate.Visible = True
		gvCurrentList.Columns.Add(columnServiceDate)

		Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCreatedFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCreatedFrom.Caption = m_Translate.GetSafeTranslationValue("BeraterIn")
		columnCreatedFrom.Name = "createdfrom"
		columnCreatedFrom.FieldName = "createdfrom"
		columnCreatedFrom.Width = 150
		columnCreatedFrom.Visible = True
		gvCurrentList.Columns.Add(columnCreatedFrom)


		m_SuppressUIEvents = True
		grdCurrentList.DataSource = Nothing
		m_SuppressUIEvents = False

	End Sub

	Private Sub ResetCVLizerGrid()

		' Reset the grid
		gvCurrentList.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvCurrentList.OptionsView.ShowIndicator = False
		gvCurrentList.OptionsView.ShowAutoFilterRow = True
		gvCurrentList.OptionsView.ColumnAutoWidth = False
		Dim showFooter As Boolean = False
		gvCurrentList.OptionsView.ShowFooter = showFooter

		gvCurrentList.Columns.Clear()

		Dim columnRecID As New DevExpress.XtraGrid.Columns.GridColumn()
		columnRecID.Caption = m_Translate.GetSafeTranslationValue("ID")
		columnRecID.Name = "RecID"
		columnRecID.FieldName = "RecID"
		columnRecID.Width = 60
		columnRecID.Visible = False
		gvCurrentList.Columns.Add(columnRecID)

		Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCreatedFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCreatedFrom.Caption = m_Translate.GetSafeTranslationValue("BeraterIn")
		columnCreatedFrom.Name = "CreatedFrom"
		columnCreatedFrom.FieldName = "CreatedFrom"
		columnCreatedFrom.Width = 150
		columnCreatedFrom.Visible = True
		gvCurrentList.Columns.Add(columnCreatedFrom)

		Dim columnServiceDate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnServiceDate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnServiceDate.Caption = m_Translate.GetSafeTranslationValue("Datum")
		columnServiceDate.Name = "ServiceDate"
		columnServiceDate.FieldName = "ServiceDate"
		columnServiceDate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
		columnServiceDate.AppearanceHeader.Options.UseTextOptions = True
		columnServiceDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columnServiceDate.DisplayFormat.FormatString = "G"
		columnServiceDate.Width = 120
		columnServiceDate.Visible = True
		gvCurrentList.Columns.Add(columnServiceDate)

		Dim columnServicename As New DevExpress.XtraGrid.Columns.GridColumn()
		columnServicename.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnServicename.Caption = m_Translate.GetSafeTranslationValue("Dienstleistung")
		columnServicename.Name = "ServiceName"
		columnServicename.FieldName = "ServiceName"
		columnServicename.Width = 200
		columnServicename.Visible = True
		gvCurrentList.Columns.Add(columnServicename)

		Dim columnJobID As New DevExpress.XtraGrid.Columns.GridColumn()
		columnJobID.Caption = m_Translate.GetSafeTranslationValue("Kennung")
		columnJobID.Name = "JobID"
		columnJobID.FieldName = "JobID"
		columnJobID.Width = 50
		columnJobID.Visible = False
		gvCurrentList.Columns.Add(columnJobID)


		m_SuppressUIEvents = True
		grdCurrentList.DataSource = Nothing
		m_SuppressUIEvents = False

	End Sub

	Private Sub ResetWOSEmployeeDocGrid()

		' Reset the grid
		gvWOSDoc.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvWOSDoc.OptionsView.ShowIndicator = False
		gvWOSDoc.OptionsView.ShowAutoFilterRow = True
		gvWOSDoc.OptionsView.ColumnAutoWidth = False
		gvWOSDoc.OptionsView.ShowFooter = False

		gvWOSDoc.Columns.Clear()

		Dim columnRecID As New DevExpress.XtraGrid.Columns.GridColumn()
		columnRecID.Caption = m_Translate.GetSafeTranslationValue("ID")
		columnRecID.Name = "ID"
		columnRecID.FieldName = "ID"
		columnRecID.Width = 60
		columnRecID.Visible = False
		gvWOSDoc.Columns.Add(columnRecID)

		Dim columnEmployeeNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeeNumber.Caption = m_Translate.GetSafeTranslationValue("Kandidaten-Nr.")
		columnEmployeeNumber.Name = "EmployeeNumber"
		columnEmployeeNumber.FieldName = "EmployeeNumber"
		columnEmployeeNumber.Width = 60
		columnEmployeeNumber.Visible = False
		gvWOSDoc.Columns.Add(columnEmployeeNumber)

		Dim columnEmploymentNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmploymentNumber.Caption = m_Translate.GetSafeTranslationValue("Einsatz-Nr.")
		columnEmploymentNumber.Name = "EmploymentNumber"
		columnEmploymentNumber.FieldName = "EmploymentNumber"
		columnEmploymentNumber.Width = 60
		columnEmploymentNumber.Visible = False
		gvWOSDoc.Columns.Add(columnEmploymentNumber)

		Dim columnReportNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnReportNumber.Caption = m_Translate.GetSafeTranslationValue("Rapport-Nr.")
		columnReportNumber.Name = "ReportNumber"
		columnReportNumber.FieldName = "ReportNumber"
		columnReportNumber.Width = 60
		columnReportNumber.Visible = False
		gvWOSDoc.Columns.Add(columnReportNumber)

		Dim columnPayrollNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnPayrollNumber.Caption = m_Translate.GetSafeTranslationValue("Lohn-Nr.")
		columnPayrollNumber.Name = "PayrollNumber"
		columnPayrollNumber.FieldName = "PayrollNumber"
		columnPayrollNumber.Width = 60
		columnPayrollNumber.Visible = False
		gvWOSDoc.Columns.Add(columnPayrollNumber)

		Dim columnEmployeeFullName As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeeFullName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnEmployeeFullName.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
		columnEmployeeFullName.Name = "EmployeeFullName"
		columnEmployeeFullName.FieldName = "EmployeeFullName"
		columnEmployeeFullName.Width = 250
		columnEmployeeFullName.Visible = True
		gvWOSDoc.Columns.Add(columnEmployeeFullName)

		Dim columnDocumentArt As New DevExpress.XtraGrid.Columns.GridColumn()
		columnDocumentArt.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnDocumentArt.Caption = m_Translate.GetSafeTranslationValue("DocumentArt")
		columnDocumentArt.Name = "DocumentArt"
		columnDocumentArt.FieldName = "DocumentArt"
		columnDocumentArt.Width = 150
		columnDocumentArt.Visible = False
		gvWOSDoc.Columns.Add(columnDocumentArt)

		Dim columnDocumentInfo As New DevExpress.XtraGrid.Columns.GridColumn()
		columnDocumentInfo.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnDocumentInfo.Caption = m_Translate.GetSafeTranslationValue("DocumentInfo")
		columnDocumentInfo.Name = "DocumentInfo"
		columnDocumentInfo.FieldName = "DocumentInfo"
		columnDocumentInfo.Width = 150
		columnDocumentInfo.Visible = True
		gvWOSDoc.Columns.Add(columnDocumentInfo)

		Dim columnTransferedOn As New DevExpress.XtraGrid.Columns.GridColumn()
		columnTransferedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnTransferedOn.Caption = m_Translate.GetSafeTranslationValue("Erstellt am")
		columnTransferedOn.Name = "TransferedOn"
		columnTransferedOn.FieldName = "TransferedOn"
		columnTransferedOn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
		columnTransferedOn.AppearanceHeader.Options.UseTextOptions = True
		columnTransferedOn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columnTransferedOn.DisplayFormat.FormatString = "G"
		columnTransferedOn.Width = 120
		columnTransferedOn.Visible = True
		gvWOSDoc.Columns.Add(columnTransferedOn)

		Dim columnTransferedUser As New DevExpress.XtraGrid.Columns.GridColumn()
		columnTransferedUser.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnTransferedUser.Caption = m_Translate.GetSafeTranslationValue("BeraterIn")
		columnTransferedUser.Name = "TransferedUser"
		columnTransferedUser.FieldName = "TransferedUser"
		columnTransferedUser.Width = 150
		columnTransferedUser.Visible = False
		gvWOSDoc.Columns.Add(columnTransferedUser)

		Dim columnDocGuid As New DevExpress.XtraGrid.Columns.GridColumn()
		columnDocGuid.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnDocGuid.Caption = m_Translate.GetSafeTranslationValue("Doc_Guid")
		columnDocGuid.Name = "DocGuid"
		columnDocGuid.FieldName = "DocGuid"
		columnDocGuid.Width = 150
		columnDocGuid.Visible = False
		gvWOSDoc.Columns.Add(columnDocGuid)

		Dim columnEmployeeWOSLink As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeeWOSLink.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnEmployeeWOSLink.Caption = m_Translate.GetSafeTranslationValue("EmployeeWOSLink")
		columnEmployeeWOSLink.Name = "EmployeeWOSLink"
		columnEmployeeWOSLink.FieldName = "EmployeeWOSLink"
		columnEmployeeWOSLink.Width = 150
		columnEmployeeWOSLink.Visible = False
		gvWOSDoc.Columns.Add(columnEmployeeWOSLink)

		Dim columnLastNotification As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLastNotification.Caption = m_Translate.GetSafeTranslationValue("Benachrichtigt am")
		columnLastNotification.Name = "LastNotification"
		columnLastNotification.FieldName = "LastNotification"
		columnLastNotification.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
		columnLastNotification.AppearanceHeader.Options.UseTextOptions = True
		columnLastNotification.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columnLastNotification.DisplayFormat.FormatString = "G"
		columnLastNotification.Width = 120
		columnLastNotification.Visible = True
		gvWOSDoc.Columns.Add(columnLastNotification)


		m_SuppressUIEvents = True
		grdWOSDoc.DataSource = Nothing
		m_SuppressUIEvents = False

	End Sub

	Private Sub ResetWOSCustomerDocGrid()

		' Reset the grid
		gvWOSDoc.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvWOSDoc.OptionsView.ShowIndicator = False
		gvWOSDoc.OptionsView.ShowAutoFilterRow = True
		gvWOSDoc.OptionsView.ColumnAutoWidth = False
		gvWOSDoc.OptionsView.ShowFooter = False

		gvWOSDoc.Columns.Clear()

		Dim columnRecID As New DevExpress.XtraGrid.Columns.GridColumn()
		columnRecID.Caption = m_Translate.GetSafeTranslationValue("ID")
		columnRecID.Name = "ID"
		columnRecID.FieldName = "ID"
		columnRecID.Width = 60
		columnRecID.Visible = False
		gvWOSDoc.Columns.Add(columnRecID)

		Dim columnCustomerNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerNumber.Caption = m_Translate.GetSafeTranslationValue("Kunden-Nr.")
		columnCustomerNumber.Name = "CustomerNumber"
		columnCustomerNumber.FieldName = "CustomerNumber"
		columnCustomerNumber.Width = 60
		columnCustomerNumber.Visible = False
		gvWOSDoc.Columns.Add(columnCustomerNumber)

		Dim columnCRepesponsibleNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCRepesponsibleNumber.Caption = m_Translate.GetSafeTranslationValue("ZHD-Nr.")
		columnCRepesponsibleNumber.Name = "CRepesponsibleNumber"
		columnCRepesponsibleNumber.FieldName = "CRepesponsibleNumber"
		columnCRepesponsibleNumber.Width = 60
		columnCRepesponsibleNumber.Visible = False
		gvWOSDoc.Columns.Add(columnCRepesponsibleNumber)

		Dim columnEmploymentNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmploymentNumber.Caption = m_Translate.GetSafeTranslationValue("Einsatz-Nr.")
		columnEmploymentNumber.Name = "EmploymentNumber"
		columnEmploymentNumber.FieldName = "EmploymentNumber"
		columnEmploymentNumber.Width = 60
		columnEmploymentNumber.Visible = False
		gvWOSDoc.Columns.Add(columnEmploymentNumber)

		Dim columnReportNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnReportNumber.Caption = m_Translate.GetSafeTranslationValue("Rapport-Nr.")
		columnReportNumber.Name = "ReportNumber"
		columnReportNumber.FieldName = "ReportNumber"
		columnReportNumber.Width = 60
		columnReportNumber.Visible = False
		gvWOSDoc.Columns.Add(columnReportNumber)

		Dim columnInvoiceNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnInvoiceNumber.Caption = m_Translate.GetSafeTranslationValue("Rechnung-Nr.")
		columnInvoiceNumber.Name = "InvoiceNumber"
		columnInvoiceNumber.FieldName = "InvoiceNumber"
		columnInvoiceNumber.Width = 60
		columnInvoiceNumber.Visible = False
		gvWOSDoc.Columns.Add(columnInvoiceNumber)

		Dim columnCustomerName As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCustomerName.Caption = m_Translate.GetSafeTranslationValue("Kunde")
		columnCustomerName.Name = "CustomerName"
		columnCustomerName.FieldName = "CustomerName"
		columnCustomerName.Width = 250
		columnCustomerName.Visible = True
		gvWOSDoc.Columns.Add(columnCustomerName)

		Dim columnResponsiblePersonFullName As New DevExpress.XtraGrid.Columns.GridColumn()
		columnResponsiblePersonFullName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnResponsiblePersonFullName.Caption = m_Translate.GetSafeTranslationValue("Zuständige Person")
		columnResponsiblePersonFullName.Name = "ResponsiblePersonFullName"
		columnResponsiblePersonFullName.FieldName = "ResponsiblePersonFullName"
		columnResponsiblePersonFullName.Width = 250
		columnResponsiblePersonFullName.Visible = True
		gvWOSDoc.Columns.Add(columnResponsiblePersonFullName)

		Dim columnDocumentArt As New DevExpress.XtraGrid.Columns.GridColumn()
		columnDocumentArt.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnDocumentArt.Caption = m_Translate.GetSafeTranslationValue("DocumentArt")
		columnDocumentArt.Name = "DocumentArt"
		columnDocumentArt.FieldName = "DocumentArt"
		columnDocumentArt.Width = 150
		columnDocumentArt.Visible = True
		gvWOSDoc.Columns.Add(columnDocumentArt)

		Dim columnDocumentInfo As New DevExpress.XtraGrid.Columns.GridColumn()
		columnDocumentInfo.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnDocumentInfo.Caption = m_Translate.GetSafeTranslationValue("DocumentInfo")
		columnDocumentInfo.Name = "DocumentInfo"
		columnDocumentInfo.FieldName = "DocumentInfo"
		columnDocumentInfo.Width = 150
		columnDocumentInfo.Visible = True
		gvWOSDoc.Columns.Add(columnDocumentInfo)

		Dim columnTransferedOn As New DevExpress.XtraGrid.Columns.GridColumn()
		columnTransferedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnTransferedOn.Caption = m_Translate.GetSafeTranslationValue("Erstellt")
		columnTransferedOn.Name = "TransferedOn"
		columnTransferedOn.FieldName = "TransferedOn"
		columnTransferedOn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
		columnTransferedOn.AppearanceHeader.Options.UseTextOptions = True
		columnTransferedOn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columnTransferedOn.DisplayFormat.FormatString = "G"
		columnTransferedOn.Width = 120
		columnTransferedOn.Visible = True
		gvWOSDoc.Columns.Add(columnTransferedOn)

		Dim columnTransferedUser As New DevExpress.XtraGrid.Columns.GridColumn()
		columnTransferedUser.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnTransferedUser.Caption = m_Translate.GetSafeTranslationValue("BeraterIn")
		columnTransferedUser.Name = "TransferedUser"
		columnTransferedUser.FieldName = "TransferedUser"
		columnTransferedUser.Width = 150
		columnTransferedUser.Visible = False
		gvWOSDoc.Columns.Add(columnTransferedUser)

		Dim columnDocGuid As New DevExpress.XtraGrid.Columns.GridColumn()
		columnDocGuid.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnDocGuid.Caption = m_Translate.GetSafeTranslationValue("Doc_Guid")
		columnDocGuid.Name = "DocGuid"
		columnDocGuid.FieldName = "DocGuid"
		columnDocGuid.Width = 150
		columnDocGuid.Visible = False
		gvWOSDoc.Columns.Add(columnDocGuid)

		Dim columnCustomerWOSLink As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerWOSLink.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCustomerWOSLink.Caption = m_Translate.GetSafeTranslationValue("CustomerWOSLink")
		columnCustomerWOSLink.Name = "CustomerWOSLink"
		columnCustomerWOSLink.FieldName = "CustomerWOSLink"
		columnCustomerWOSLink.Width = 150
		columnCustomerWOSLink.Visible = False
		gvWOSDoc.Columns.Add(columnCustomerWOSLink)

		Dim columnResponsiblePersonWOSLink As New DevExpress.XtraGrid.Columns.GridColumn()
		columnResponsiblePersonWOSLink.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnResponsiblePersonWOSLink.Caption = m_Translate.GetSafeTranslationValue("EmployeeWOSLink")
		columnResponsiblePersonWOSLink.Name = "ResponsiblePersonWOSLink"
		columnResponsiblePersonWOSLink.FieldName = "ResponsiblePersonWOSLink"
		columnResponsiblePersonWOSLink.Width = 150
		columnResponsiblePersonWOSLink.Visible = False
		gvWOSDoc.Columns.Add(columnResponsiblePersonWOSLink)

		Dim columnGetOn As New DevExpress.XtraGrid.Columns.GridColumn()
		columnGetOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnGetOn.Caption = m_Translate.GetSafeTranslationValue("GetOn")
		columnGetOn.Name = "GetOn"
		columnGetOn.FieldName = "GetOn"
		columnGetOn.Width = 150
		columnGetOn.Visible = False
		gvWOSDoc.Columns.Add(columnGetOn)

		Dim columnGetResult As New DevExpress.XtraGrid.Columns.GridColumn()
		columnGetResult.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnGetResult.Caption = m_Translate.GetSafeTranslationValue("GetResult")
		columnGetResult.Name = "GetResult"
		columnGetResult.FieldName = "GetResult"
		columnGetResult.Width = 50
		columnGetResult.Visible = False
		gvWOSDoc.Columns.Add(columnGetResult)

		Dim columnLastNotification As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLastNotification.Caption = m_Translate.GetSafeTranslationValue("Benachrichtigt am")
		columnLastNotification.Name = "LastNotification"
		columnLastNotification.FieldName = "LastNotification"
		columnLastNotification.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
		columnLastNotification.AppearanceHeader.Options.UseTextOptions = True
		columnLastNotification.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columnLastNotification.DisplayFormat.FormatString = "G"
		columnLastNotification.Width = 120
		columnLastNotification.Visible = True
		gvWOSDoc.Columns.Add(columnLastNotification)


		m_SuppressUIEvents = True
		grdWOSDoc.DataSource = Nothing
		m_SuppressUIEvents = False

	End Sub

	Private Sub ResetWOSMessageGrid()

		' Reset the grid
		gvWOSMessage.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvWOSMessage.OptionsView.ShowIndicator = False
		gvWOSMessage.OptionsView.ShowAutoFilterRow = True
		gvWOSMessage.OptionsView.ColumnAutoWidth = False
		gvWOSMessage.OptionsView.ShowFooter = False

		gvWOSMessage.Columns.Clear()

		Dim columnRecID As New DevExpress.XtraGrid.Columns.GridColumn()
		columnRecID.Caption = m_Translate.GetSafeTranslationValue("ID")
		columnRecID.Name = "ID"
		columnRecID.FieldName = "ID"
		columnRecID.Width = 60
		columnRecID.Visible = False
		gvWOSMessage.Columns.Add(columnRecID)

		Dim columnMailTo As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMailTo.Caption = m_Translate.GetSafeTranslationValue("Empfänger")
		columnMailTo.Name = "MailTo"
		columnMailTo.FieldName = "MailTo"
		columnMailTo.Width = 200
		columnMailTo.Visible = True
		gvWOSMessage.Columns.Add(columnMailTo)

		Dim columnMailSubject As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMailSubject.Caption = m_Translate.GetSafeTranslationValue("Betreff")
		columnMailSubject.Name = "MailSubject"
		columnMailSubject.FieldName = "MailSubject"
		columnMailSubject.Width = 150
		columnMailSubject.Visible = False
		gvWOSMessage.Columns.Add(columnMailSubject)

		Dim columnDocLink As New DevExpress.XtraGrid.Columns.GridColumn()
		columnDocLink.Caption = m_Translate.GetSafeTranslationValue("Link")
		columnDocLink.Name = "DocLink"
		columnDocLink.FieldName = "DocLink"
		columnDocLink.Width = 60
		columnDocLink.Visible = False
		gvWOSMessage.Columns.Add(columnDocLink)

		Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCreatedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCreatedOn.Caption = m_Translate.GetSafeTranslationValue("Erstellt")
		columnCreatedOn.Name = "CreatedOn"
		columnCreatedOn.FieldName = "CreatedOn"
		columnCreatedOn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
		columnCreatedOn.AppearanceHeader.Options.UseTextOptions = True
		columnCreatedOn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columnCreatedOn.DisplayFormat.FormatString = "G"
		columnCreatedOn.Width = 150
		columnCreatedOn.Visible = True
		gvWOSMessage.Columns.Add(columnCreatedOn)


		m_SuppressUIEvents = True
		grdWOSMessage.DataSource = Nothing
		m_SuppressUIEvents = False

	End Sub

	''' <summary>
	''' Resets the Paid grid.
	''' </summary>
	Private Sub ResetPaidGrid()

		' Reset the grid
		gvPaidlist.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvPaidlist.OptionsView.ShowIndicator = False
		gvPaidlist.OptionsView.ShowAutoFilterRow = True
		gvPaidlist.OptionsView.ColumnAutoWidth = False
		gvPaidlist.OptionsView.ShowFooter = True
		gvPaidlist.OptionsView.AllowHtmlDrawGroups = True

		gvPaidlist.Columns.Clear()

		Dim columnRecID As New DevExpress.XtraGrid.Columns.GridColumn()
		columnRecID.Caption = m_Translate.GetSafeTranslationValue("ID")
		columnRecID.Name = "RecID"
		columnRecID.FieldName = "RecID"
		columnRecID.Width = 60
		columnRecID.Visible = False
		gvPaidlist.Columns.Add(columnRecID)

		Dim columnUserData As New DevExpress.XtraGrid.Columns.GridColumn()
		columnUserData.Caption = m_Translate.GetSafeTranslationValue("Benutzer")
		columnUserData.Name = "UserData"
		columnUserData.FieldName = "UserData"
		columnUserData.Width = 150
		columnUserData.Visible = True
		gvPaidlist.Columns.Add(columnUserData)


		Dim columnServiceDate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnServiceDate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnServiceDate.Caption = m_Translate.GetSafeTranslationValue("Datum")
		columnServiceDate.Name = "ServiceDate"
		columnServiceDate.FieldName = "ServiceDate"
		columnServiceDate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
		columnServiceDate.AppearanceHeader.Options.UseTextOptions = True
		columnServiceDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columnServiceDate.DisplayFormat.FormatString = "G"
		columnServiceDate.Width = 120
		columnServiceDate.Visible = True
		gvPaidlist.Columns.Add(columnServiceDate)

		Dim columnServicename As New DevExpress.XtraGrid.Columns.GridColumn()
		columnServicename.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnServicename.Caption = m_Translate.GetSafeTranslationValue("Dienstleistung")
		columnServicename.Name = "ServiceName"
		columnServicename.FieldName = "ServiceName"
		columnServicename.Width = 100
		columnServicename.Visible = True
		gvPaidlist.Columns.Add(columnServicename)



		Dim columnAuthorizedCreditCount As New DevExpress.XtraGrid.Columns.GridColumn()
		columnAuthorizedCreditCount.Caption = m_Translate.GetSafeTranslationValue("Anzahl")
		columnAuthorizedCreditCount.Name = "AuthorizedItems"
		columnAuthorizedCreditCount.FieldName = "AuthorizedItems"
		columnAuthorizedCreditCount.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnAuthorizedCreditCount.AppearanceHeader.Options.UseTextOptions = True
		columnAuthorizedCreditCount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnAuthorizedCreditCount.DisplayFormat.FormatString = "N5"
		columnAuthorizedCreditCount.Width = 80
		columnAuthorizedCreditCount.Visible = True
		columnAuthorizedCreditCount.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		gvPaidlist.Columns.Add(columnAuthorizedCreditCount)

		Dim columnContent As New DevExpress.XtraGrid.Columns.GridColumn()
		columnContent.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnContent.Caption = m_Translate.GetSafeTranslationValue("Inhalt")
		columnContent.Name = "Content"
		columnContent.FieldName = "Content"
		columnContent.Visible = False
		columnContent.OptionsColumn.ShowInCustomizationForm = m_InitializationData.UserData.UserNr = 1
		gvPaidlist.Columns.Add(columnContent)

		Dim columnRecipient As New DevExpress.XtraGrid.Columns.GridColumn()
		columnRecipient.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnRecipient.Caption = m_Translate.GetSafeTranslationValue("Empfänger")
		columnRecipient.Name = "Recipient"
		columnRecipient.FieldName = "Recipient"
		columnRecipient.Width = 100
		columnRecipient.Visible = True
		gvPaidlist.Columns.Add(columnRecipient)

		Dim columnSender As New DevExpress.XtraGrid.Columns.GridColumn()
		columnSender.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnSender.Caption = m_Translate.GetSafeTranslationValue("Absender")
		columnSender.Name = "Sender"
		columnSender.FieldName = "Sender"
		columnSender.Width = 100
		columnSender.Visible = False ' If(lueServiceName.EditValue = "ECALL_SMSCREDIT", False, True)
		gvPaidlist.Columns.Add(columnSender)



		Dim columnBookedPayment As New DevExpress.XtraGrid.Columns.GridColumn()
		columnBookedPayment.Caption = m_Translate.GetSafeTranslationValue("Verrechnet?")
		columnBookedPayment.Name = "BookedPayment"
		columnBookedPayment.FieldName = "BookedPayment"
		columnBookedPayment.Width = 50
		columnBookedPayment.Visible = True
		gvPaidlist.Columns.Add(columnBookedPayment)

		Dim columnStateCondition As New DevExpress.XtraGrid.Columns.GridColumn()
		columnStateCondition.Caption = m_Translate.GetSafeTranslationValue("Erfolgreich?")
		columnStateCondition.Name = "StateCondition"
		columnStateCondition.FieldName = "StateCondition"
		columnStateCondition.Width = 50
		columnStateCondition.Visible = True
		gvPaidlist.Columns.Add(columnStateCondition)

		Dim columnStatus As New DevExpress.XtraGrid.Columns.GridColumn()
		columnStatus.Caption = m_Translate.GetSafeTranslationValue("Status")
		columnStatus.Name = "Status"
		columnStatus.FieldName = "Status"
		columnStatus.Width = 50
		columnStatus.Visible = False
		gvPaidlist.Columns.Add(columnStatus)

		Dim columnResultCode As New DevExpress.XtraGrid.Columns.GridColumn()
		columnResultCode.Caption = m_Translate.GetSafeTranslationValue("ResultCode")
		columnResultCode.Name = "ResultCode"
		columnResultCode.FieldName = "ResultCode"
		columnResultCode.Width = 50
		columnResultCode.Visible = False
		gvPaidlist.Columns.Add(columnResultCode)

		Dim columnResultMessage As New DevExpress.XtraGrid.Columns.GridColumn()
		columnResultMessage.Caption = m_Translate.GetSafeTranslationValue("ResultMessage")
		columnResultMessage.Name = "ResultMessage"
		columnResultMessage.FieldName = "ResultMessage"
		columnResultMessage.Width = 50
		columnResultMessage.Visible = False
		gvPaidlist.Columns.Add(columnResultMessage)

		Dim columnBookedDate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnBookedDate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnBookedDate.Caption = m_Translate.GetSafeTranslationValue("Fakturadatum")
		columnBookedDate.Name = "BookedDate"
		columnBookedDate.FieldName = "BookedDate"
		columnBookedDate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
		columnBookedDate.AppearanceHeader.Options.UseTextOptions = True
		columnBookedDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columnBookedDate.DisplayFormat.FormatString = "G"
		columnBookedDate.Width = 120
		columnBookedDate.Visible = True
		gvPaidlist.Columns.Add(columnBookedDate)


		Dim grpAuthorizedItems = New GridGroupSummaryItem()
		grpAuthorizedItems.FieldName = "AuthorizedItems"
		grpAuthorizedItems.SummaryType = DevExpress.Data.SummaryItemType.Sum
		grpAuthorizedItems.DisplayFormat = m_Translate.GetSafeTranslationValue("Anzahl") & " = {0:n5}"
		gvPaidlist.GroupFormat = "{1}: {2}"
		gvPaidlist.GroupSummary.Add(grpAuthorizedItems)


		m_SuppressUIEvents = True
		grdpaidlist.DataSource = Nothing
		m_SuppressUIEvents = False

	End Sub


	''' <summary>
	''' Handles click on close button.
	''' </summary>
	Private Sub OnBtnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
		DialogResult = DialogResult.None
		Me.Dispose()
	End Sub

	''' <summary>
	''' Handles the form disposed event.
	''' </summary>
	Private Sub Onfrm_FormClosing(sender As Object, e As System.EventArgs) Handles Me.FormClosing

		' Save form location, width and height in setttings
		Try
			SplashScreenManager.CloseForm(False)

			If Not Me.WindowState = FormWindowState.Minimized Then
				m_SettingsManager.WriteString(SettingKeys.SETTING_FORM_ECALLLOGS_LOCATION, String.Format("{0};{1}", Me.Location.X, Me.Location.Y))
				m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_ECALLLOGS_WIDTH, Me.Width)
				m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_ECALLLOGS_HEIGHT, Me.Height)
				m_SettingsManager.WriteString(SettingKeys.SETTING_FORM_ECALLLOGS_LASTSELECTEDSERVICE, lueServiceName.EditValue)

				m_SettingsManager.SaveSettings()
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())

		End Try

	End Sub

	''' <summary>
	''' Handles form load event.
	''' </summary>
	Private Sub Onfrm_Load(sender As Object, e As System.EventArgs) Handles Me.Load

		Dim strStyleName As String = m_MandantData.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		Try
			Dim setting_form_search_height = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_ECALLLOGS_HEIGHT)
			Dim setting_form_search_width = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_ECALLLOGS_WIDTH)
			Dim setting_form_search_location = m_SettingsManager.ReadString(SettingKeys.SETTING_FORM_ECALLLOGS_LOCATION)

			If setting_form_search_height > 0 Then Me.Height = Math.Max(Me.Height, setting_form_search_height)
			If setting_form_search_width > 0 Then Me.Width = Math.Max(Me.Width, setting_form_search_width)

			If Not String.IsNullOrEmpty(setting_form_search_location) Then
				Dim aLoc As String() = setting_form_search_location.Split(CChar(";"))
				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = "0"
				End If
				Me.Location = New System.Drawing.Point(CInt(Val(aLoc(0))), CInt(Val(aLoc(1))))
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())

		End Try

	End Sub


#Region "Customer Data"

	''' <summary>
	''' Search customer data over web service.
	''' </summary>
	Private Sub SearchCustomerlistViaWebService()

		'Dim uiSynchronizationContext = TaskScheduler.Default 'FromCurrentSynchronizationContext()
		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		grpFilter.Enabled = False

		Dim data = PerformCustomerlistWebserviceCallAsync()

		m_SuppressUIEvents = True
		lueCustomer.Properties.DataSource = data

		lueCustomer.EditValue = m_InitializationData.MDData.MDGuid

		lueAdvisor.Enabled = Not lueCustomer.EditValue Is Nothing
		lueServiceName.Enabled = Not lueCustomer.EditValue Is Nothing

		'm_SuppressUIEvents = False
		grpFilter.Enabled = True
		LoadData()
		m_SuppressUIEvents = False

		SplashScreenManager.CloseForm(False)
		btnSearch.Enabled = True

	End Sub

	''' <summary>
	'''  Performs the check asynchronous.
	''' </summary>
	''' <returns>The report response.</returns>
	Private Function PerformCustomerlistWebserviceCallAsync() As BindingList(Of CustomerViewData)
		Dim listDataSource As BindingList(Of CustomerViewData) = New BindingList(Of CustomerViewData)
		Dim viewData As New CustomerViewData
		If Not m_InitializationData.UserData.UserNr = 1 Then
			Dim Data = m_CommonDatabaseAccess.LoadCompaniesListData
			For Each itm In Data
				viewData = New CustomerViewData With {
							.customer_ID = itm.MandantGuid,
							.customer_Name = itm.MandantName1
						}
				listDataSource.Add(viewData)
			Next

			Return listDataSource
		End If

		Dim Customer_ID As String = If(lueCustomer.EditValue Is Nothing, m_InitializationData.MDData.MDGuid, lueCustomer.EditValue)
		Dim serviceName As String = If(Me.lueServiceName.EditValue Is Nothing, String.Empty, Me.lueServiceName.EditValue.ToString)
		Dim dt As String = String.Empty
		If Not Me.dateEditServiceDate.EditValue Is Nothing Then
			dt = Me.dateEditServiceDate.EditValue.ToString
		End If
		Dim userName As String = String.Empty
		If Not lueAdvisor.EditValue Is Nothing Then userName = Me.lueAdvisor.EditValue.ToString

#If DEBUG Then
		'm_PaymentUtilWebServiceUri = "http://localhost:44721/SPCustomerPaymentServices.asmx"
#End If

		Dim webservice As New SPCustomerPaymentServicesWebService.SPCustomerPaymentServicesSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_PaymentUtilWebServiceUri)

		Dim searchResult As List(Of CustomerSearchResultDTO) = Nothing
		Try
			' Read data over webservice
			searchResult = webservice.GetCustomerListOfServices().ToList

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try


		For Each result In searchResult
			viewData = New CustomerViewData With {
					.customer_ID = result.customer_ID,
					.customer_Name = result.customer_Name
				}

			listDataSource.Add(viewData)
		Next

		Return listDataSource

	End Function

	''' <summary>
	''' Finish Customer web service call.
	''' </summary>
	Private Sub FinishCustomerlistWebserviceCallTask(ByVal t As Task(Of BindingList(Of CustomerViewData)))

		Try
			Select Case t.Status
				Case TaskStatus.RanToCompletion
					' Webservice call was successful.
					m_SuppressUIEvents = True
					lueCustomer.Properties.DataSource = t.Result

					lueAdvisor.Enabled = Not lueCustomer.EditValue Is Nothing
					lueServiceName.Enabled = Not lueCustomer.EditValue Is Nothing

					m_SuppressUIEvents = False
					lueCustomer.EditValue = m_InitializationData.MDData.MDGuid
					grpFilter.Enabled = True
					LoadData()

				Case TaskStatus.Faulted
					' Something went wrong -> log error.
					m_Logger.LogError(t.Exception.ToString())
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht geladen werden."))

				Case Else
					' Do nothing
			End Select

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		SplashScreenManager.CloseForm(False)
		btnSearch.Enabled = True

	End Sub


	''' <summary>
	''' Handles change of customer.
	''' </summary>
	Private Sub OnLueCustomer_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueCustomer.EditValueChanged

		If m_SuppressUIEvents Then
			Return
		End If
		grdCurrentList.DataSource = Nothing

		If Not lueCustomer.EditValue Is Nothing Then

			Dim customerData = PerformUserNamelistWebserviceCallAsync() '  (lueCustomer.EditValue, m_ClsProgSetting.GetUSFiliale)

			If customerData Is Nothing Then
				m_UtilityUI.ShowErrorDialog("Kundendaten konnten nicht geladen werden.")

			Else
				lueAdvisor.Properties.DataSource = customerData
				lueAdvisor.Enabled = True
				lueServiceName.Enabled = Not lueCustomer.EditValue Is Nothing

				m_EmployeeWOSID = lueCustomer.EditValue
				m_CustomerWOSID = lueCustomer.EditValue

			End If

		Else
			lueAdvisor.EditValue = Nothing
			lueAdvisor.Properties.DataSource = Nothing
		End If
		SearchPaidlistViaWebService()
		SearchCurrentlistViaWebService()

	End Sub


	Private Sub OnlueAdvisor_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueAdvisor.EditValueChanged

		If m_SuppressUIEvents Then
			Return
		End If
		LoadData()

	End Sub

	Private Sub OnlueServiceName_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueServiceName.EditValueChanged

		If m_SuppressUIEvents Then
			Return
		End If
		'SearchPaidlistViaWebService()
		LoadData()

	End Sub

	Private Sub OndateEditServiceDate_EditValueChanged(sender As Object, e As EventArgs) Handles dateEditServiceDate.EditValueChanged

		If m_SuppressUIEvents Then
			Return
		End If
		SearchPaidlistViaWebService()
		LoadData()

	End Sub

	Private Sub OnlueYear_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueYear.EditValueChanged

		If m_SuppressUIEvents Then
			Return
		End If
		'SearchPaidlistViaWebService()
		LoadData()

	End Sub

	Private Sub OnlueMonth_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueMonth.EditValueChanged

		If m_SuppressUIEvents Then
			Return
		End If
		'SearchPaidlistViaWebService()
		LoadData()

	End Sub


#End Region


	''' <summary>
	'''  Performs the check asynchronous.
	''' </summary>
	''' <returns>The report response.</returns>
	Private Function PerformUserNamelistWebserviceCallAsync() As BindingList(Of CustomerUserNameViewData)

		Dim listDataSource As BindingList(Of CustomerUserNameViewData) = New BindingList(Of CustomerUserNameViewData)
		Dim Customer_ID As String = If(lueCustomer.EditValue Is Nothing, m_InitializationData.MDData.MDGuid, lueCustomer.EditValue)

#If DEBUG Then
		m_PaymentUtilWebServiceUri = "http://asmx.domain.com/wsSPS_services/SPCustomerPaymentServices.asmx"
#End If

		Dim webservice As New SPCustomerPaymentServicesWebService.SPCustomerPaymentServicesSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_PaymentUtilWebServiceUri)

		Dim searchResult As List(Of CustomerUserNameSearchResultDTO) = Nothing
		Try
			' Read data over webservice
			searchResult = webservice.GetCustomerUserNameListOfServices(Customer_ID).ToList

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try


		For Each result In searchResult

			Dim viewData = New CustomerUserNameViewData With {
				.UserName = result.UserName
			}

			listDataSource.Add(viewData)

		Next

		Return listDataSource

	End Function



	''' <summary>
	''' Handles click on search button.
	''' </summary>
	Private Sub OnBtnSearch_Click(sender As System.Object, e As System.EventArgs) Handles btnSearch.Click

		LoadData()

	End Sub

	Private Sub btnUpdateeCallResponse_Click(sender As Object, e As EventArgs) Handles btnUpdateeCallResponse.Click
		UpdateCurrenteCallResponseDataViaWebService()
	End Sub


#Region "load data for currentlist over soap"

	''' <summary>
	''' Search for current list over web service.
	''' </summary>
	Private Sub SearchCurrentlistViaWebService()

		lbleCallResponseInfo.Text = String.Empty
		If lueCustomer.EditValue Is Nothing Then Return
		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")
		bsiCurrentReccount.Caption = ""

		Dim uiSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext()

		btnSearch.Enabled = False
		grdCurrentList.DataSource = Nothing

		Task(Of BindingList(Of PaymentSearchViewData)).Factory.StartNew(Function() PerformCurrentlistWebserviceCallAsync(),
																						CancellationToken.None,
																						TaskCreationOptions.None,
																						TaskScheduler.Default).ContinueWith(Sub(t) FinishCurrentlistWebserviceCallTask(t), CancellationToken.None,
																																								TaskContinuationOptions.None, uiSynchronizationContext)

	End Sub


	''' <summary>
	'''  Performs the check asynchronous.
	''' </summary>
	''' <returns>The report response.</returns>
	Private Function PerformCurrentlistWebserviceCallAsync() As BindingList(Of PaymentSearchViewData)
		Dim searcheCallData As Boolean = True
		Dim listDataSource As BindingList(Of PaymentSearchViewData) = New BindingList(Of PaymentSearchViewData)
		Dim Customer_ID As String = If(lueCustomer.EditValue Is Nothing, m_InitializationData.MDData.MDGuid, lueCustomer.EditValue)
		Dim serviceName As String = If(Me.lueServiceName.EditValue Is Nothing, String.Empty, Me.lueServiceName.EditValue.ToString)
		Dim dt As String = String.Empty
		Dim lastCheck As Boolean = True

		searcheCallData = Not serviceName.ToLower.Contains("_check")
		If Not Me.dateEditServiceDate.EditValue Is Nothing Then
			dt = Me.dateEditServiceDate.EditValue.ToString
		End If
		Dim userName As String = String.Empty
		If Not lueAdvisor.EditValue Is Nothing Then userName = Me.lueAdvisor.EditValue.ToString

#If DEBUG Then
		'm_PaymentUtilWebServiceUri = "http://asmx.domain.com/wsSPS_services/SPCustomerPaymentServices.asmx"
		Trace.WriteLine(Customer_ID)
#End If

		Dim webservice As New SPCustomerPaymentServicesWebService.SPCustomerPaymentServicesSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_PaymentUtilWebServiceUri)

		Dim searchResult As List(Of PaymentSearchResultDTO) = Nothing
		Try
			' Read data over webservice
			searchResult = webservice.GetCurrentListOfServices(Customer_ID, userName, dt, serviceName, lueYear.EditValue, lueMonth.EditValue).ToList

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		'Dim i As Integer = 0
		For Each result In searchResult

			Dim viewData = New PaymentSearchViewData With {
				.RecID = result.RecID,
				.CustomerGuid = result.CustomerGuid,
				.UserGuid = result.UserGuid,
				.ServiceName = result.ServiceName,
				.ServiceDate = result.ServiceDate,
				.CreatedOn = result.CreatedOn,
				.CreatedFrom = result.CreatedFrom,
				.JobID = result.JobID,
				.AuthorizedCredit = If(result.Validated, Math.Max(result.AuthorizedCredit, 1), 0),
				.AuthorizedItems = If(result.Validated, result.AuthorizedItems, 0),
				.BookedPayment = result.BookedPayment,
				.BookedDate = result.BookedDate
			}
#If Not DEBUG Then
			If m_InitializationData.UserData.UserNr = 1 AndAlso searcheCallData AndAlso lastCheck AndAlso Not result.Validated Then
				'LoadMessageData(result.JobID)
				'viewData.AuthorizedItems = m_eCallMessageInfo.ItemCount
				'viewData.AuthorizedCredit = result.AuthorizedCredit
				'searcheCallData = m_eCallMessageInfo.MessageResponse.ServiceResponse.ResponseCode = 0
			End If

#End If

			listDataSource.Add(viewData)


		Next

		Return listDataSource

	End Function

	''' <summary>
	''' Finish web service call.
	''' </summary>
	Private Sub FinishCurrentlistWebserviceCallTask(ByVal t As Task(Of BindingList(Of PaymentSearchViewData)))

		Try
			Select Case t.Status
				Case TaskStatus.RanToCompletion
					' Webservice call was successful.
					m_SuppressUIEvents = True
					'Dim data = t.Result.Where(Function(p) Year(p.ServiceDate) = lueYear.EditValue).FirstOrDefault()
					grdCurrentList.DataSource = t.Result
					m_SuppressUIEvents = False

				Case TaskStatus.Faulted
					' Something went wrong -> log error.
					m_Logger.LogError(t.Exception.ToString())
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht geladen werden."))

				Case Else
					' Do nothing
			End Select
		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		SplashScreenManager.CloseForm(False)
		m_eCallMessageInfo = Nothing
		btnSearch.Enabled = True
		bsiCurrentReccount.Caption = If(bsiCurrentCount.Visibility = DevExpress.XtraBars.BarItemVisibility.Always, String.Format("{0}", gvCurrentList.RowCount), String.Empty)

	End Sub

#End Region



#Region "Grid for Paid records"


	''' <summary>
	''' Search for Paidlist over web service.
	''' </summary>
	Private Sub SearchPaidlistViaWebService()

		If lueCustomer.EditValue Is Nothing Then Return
		Dim uiSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext()
		bsiPaidReccount.Caption = ""

		'ResetPaidGrid()
		btnSearch.Enabled = False
		grdpaidlist.DataSource = Nothing

		Task(Of BindingList(Of PaidSearchViewData)).Factory.StartNew(Function() PerformPaidlistWebserviceCallAsync(),
																						CancellationToken.None,
																						TaskCreationOptions.None,
																						TaskScheduler.Default).ContinueWith(Sub(t) FinishPaidlistWebserviceCallTask(t), CancellationToken.None,
																																								TaskContinuationOptions.None, uiSynchronizationContext)

	End Sub

	''' <summary>
	'''  Performs Paidlist check asynchronous.
	''' </summary>
	''' <returns>The report response.</returns>
	Private Function PerformPaidlistWebserviceCallAsync() As BindingList(Of PaidSearchViewData)

		Dim listDataSource As BindingList(Of PaidSearchViewData) = New BindingList(Of PaidSearchViewData)
		Dim Customer_ID As String = If(lueCustomer.EditValue Is Nothing, m_InitializationData.MDData.MDGuid, lueCustomer.EditValue)
		Dim serviceName As String = If(Me.lueServiceName.EditValue Is Nothing, String.Empty, Me.lueServiceName.EditValue.ToString)
		Dim dt As String = String.Empty
		If Not Me.dateEditServiceDate.EditValue Is Nothing Then
			dt = Me.dateEditServiceDate.EditValue.ToString
		End If
		Dim userName As String = String.Empty
		If Not String.IsNullOrWhiteSpace(Me.lueAdvisor.EditValue) Then userName = Me.lueAdvisor.EditValue

#If DEBUG Then
		Customer_ID = lueCustomer.EditValue
#End If

		Dim webservice As New SPCustomerPaymentServicesWebService.SPCustomerPaymentServicesSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_PaymentUtilWebServiceUri)

		Dim searchResult As List(Of PaidSearchResultDTO) = Nothing
		Try
			' Read data over webservice
			searchResult = webservice.GetPaidListOfServices(Customer_ID, String.Empty, dt, serviceName, lueYear.EditValue, lueMonth.EditValue).ToList

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try


		For Each result In searchResult

			Dim viewData = New PaidSearchViewData With {
				.RecID = result.RecID,
				.CustomerGuid = result.CustomerGuid,
				.Content = result.Content,
				.ServiceName = result.ServiceName,
				.ServiceDate = result.ServiceDate,
				.Sender = result.Sender,
				.Recipient = result.Recipient,
				.Status = result.Status,
				.UserData = result.UserData,
				.AuthorizedItems = result.AuthorizedItems,
				.BookedPayment = result.BookedPayment,
				.BookedDate = result.BookedDate,
				.ResultCode = result.ResultCode,
				.ResultMessage = result.ResultMessage
			}

			listDataSource.Add(viewData)

		Next


		Return listDataSource

	End Function

	''' <summary>
	''' Finish Paidlist web service call.
	''' </summary>
	Private Sub FinishPaidlistWebserviceCallTask(ByVal t As Task(Of BindingList(Of PaidSearchViewData)))

		Try

			Select Case t.Status
				Case TaskStatus.RanToCompletion
					' Webservice call was successful.
					m_SuppressUIEvents = True
					grdpaidlist.DataSource = t.Result

					m_SuppressUIEvents = False

				Case TaskStatus.Faulted
					' Something went wrong -> log error.
					m_Logger.LogError(t.Exception.ToString())
					SplashScreenManager.CloseForm(False)
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht geladen werden."))

				Case Else
					' Do nothing
			End Select

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		btnSearch.Enabled = True
		bsiPaidReccount.Caption = String.Format("{0}", gvPaidlist.RowCount)
		SplashScreenManager.CloseForm(False)

	End Sub


#End Region



#Region "save data for currentlist over soap"

	''' <summary>
	''' save data with ecall response.
	''' </summary>
	Private Sub UpdateCurrenteCallResponseDataViaWebService()

		lbleCallResponseInfo.Text = String.Empty
		If lueCustomer.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(m_CurrentJobID) Then Return

		If Not m_eCallMessageInfo Is Nothing Then
			If m_eCallMessageInfo.ItemCount = 0 OrElse m_eCallMessageInfo.MessageResponse.ServiceResponse.ResponseCode <> 0 Then Return
		End If

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Daten werden gespeichert.") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Dim uiSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext()

		btnSearch.Enabled = False
		Task(Of Boolean).Factory.StartNew(Function() PerformUpdateCurrenteCallResponseWebserviceCallAsync(),
																						CancellationToken.None,
																						TaskCreationOptions.None,
																						TaskScheduler.Default).ContinueWith(Sub(t) FinishUpdateingeCallResposeDataWebserviceCallTask(t), CancellationToken.None,
																																								TaskContinuationOptions.None, uiSynchronizationContext)

	End Sub


	''' <summary>
	'''  Performs the update ecall response data asynchronous.
	''' </summary>
	''' <returns>The report response.</returns>
	Private Function PerformUpdateCurrenteCallResponseWebserviceCallAsync() As Boolean
		Dim searcheCallData As Boolean = True
		Dim Customer_ID As String = If(lueCustomer.EditValue Is Nothing, m_InitializationData.MDData.MDGuid, lueCustomer.EditValue)

#If DEBUG Then
		m_PaymentUtilWebServiceUri = "http://asmx.domain.com/wsSPS_services/SPCustomerPaymentServices.asmx"
		Trace.WriteLine(Customer_ID)
#End If

		Dim webservice As New SPCustomerPaymentServicesWebService.SPCustomerPaymentServicesSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_PaymentUtilWebServiceUri)

		Dim success As Boolean = True
		Try
			' update data over webservice
			success = webservice.UpdateeCallResponseDataForSelectedJobID(Customer_ID, m_CurrentJobID, m_eCallMessageInfo.UsedCredits, m_eCallMessageInfo.ItemCount)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try


		Return success

	End Function

	''' <summary>
	''' Finish web service call for updateing ecall response.
	''' </summary>
	Private Sub FinishUpdateingeCallResposeDataWebserviceCallTask(ByVal t As Task(Of Boolean))

		Try
			Select Case t.Status
				Case TaskStatus.RanToCompletion
					' Webservice call was successful.
					m_SuppressUIEvents = True
					'grdCurrentList.DataSource = t.Result
					m_SuppressUIEvents = False

				Case TaskStatus.Faulted
					' Something went wrong -> log error.
					m_Logger.LogError(t.Exception.ToString())
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht aktualisiert werden."))

				Case Else
					' Do nothing
			End Select

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		SplashScreenManager.CloseForm(False)
		btnSearch.Enabled = True
		bsiCurrentReccount.Caption = String.Format("{0}", gvCurrentList.RowCount)

	End Sub

#End Region


#Region "load data for messages in dbselect database"


	''' <summary>
	'''  Performs the check asynchronous.
	''' </summary>
	''' <returns>The report response.</returns>
	Private Function PerformSentMessageCallAsync() As Boolean
		Dim listingDataList As List(Of ListingMailData)

		If lueCustomer.EditValue Is Nothing Then Return False
		Try
			listingDataList = m_ListingDatabaseAccess.LoadEMailData(lueCustomer.EditValue, lueYear.EditValue, lueMonth.EditValue, dateEditServiceDate.EditValue)

			If (listingDataList Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Nachrichten-Daten konnten nicht geladen werden."))

				Return Nothing
			End If

			Dim reportGridData = (From report In listingDataList
								  Select New ListingMailData With
						 {.ID = report.ID,
							.recNr = report.recNr,
							.customer_id = report.customer_id,
							.KDNr = report.KDNr,
							.ZHDNr = report.ZHDNr,
							.MANr = report.MANr,
							.email_to = report.email_to,
							.email_from = report.email_from,
							.email_subject = report.email_subject,
							.email_body = report.email_body,
							.employeefirstname = report.employeefirstname,
							.employeelastname = report.employeelastname,
							.customername = report.customername,
							.cresponsiblefirstname = report.cresponsiblefirstname,
							.cresponsiblelastname = report.cresponsiblelastname,
							.cresponsiblesalution = report.cresponsiblesalution,
							.createdon = report.createdon,
							.createdfrom = report.createdfrom,
							.messageID = report.messageID
						 }).ToList()

			Dim listDataSource As BindingList(Of ListingMailData) = New BindingList(Of ListingMailData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			m_SuppressUIEvents = True
			grdCurrentList.DataSource = listDataSource

			btnSearch.Enabled = True
			bsiCurrentReccount.Caption = String.Format("{0}", gvCurrentList.RowCount)
			m_SuppressUIEvents = False

			Return Not listDataSource Is Nothing


		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(ex.ToString))
			Return False

		End Try

	End Function


#End Region


#Region "load data for cvlizer"


	''' <summary>
	''' Search for current list over web service.
	''' </summary>
	Private Sub SearchCVLizerlistViaWebService()

		lbleCallResponseInfo.Text = String.Empty
		If lueCustomer.EditValue Is Nothing Then Return
		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")
		bsiCurrentReccount.Caption = ""

		Dim uiSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext()

		btnSearch.Enabled = False
		grdCurrentList.DataSource = Nothing

		Task(Of BindingList(Of PaymentSearchViewData)).Factory.StartNew(Function() PerformCVLizerlistWebserviceCallAsync(),
																						CancellationToken.None,
																						TaskCreationOptions.None,
																						TaskScheduler.Default).ContinueWith(Sub(t) FinishCVLizerlistWebserviceCallTask(t), CancellationToken.None,
																																								TaskContinuationOptions.None, uiSynchronizationContext)

	End Sub


	''' <summary>
	'''  Performs the check asynchronous.
	''' </summary>
	''' <returns>The report response.</returns>
	Private Function PerformCVLizerlistWebserviceCallAsync() As BindingList(Of PaymentSearchViewData)
		Dim listDataSource As BindingList(Of PaymentSearchViewData) = New BindingList(Of PaymentSearchViewData)
		Dim Customer_ID As String = If(lueCustomer.EditValue Is Nothing, m_InitializationData.MDData.MDGuid, lueCustomer.EditValue)
		Dim serviceName As String = If(Me.lueServiceName.EditValue Is Nothing, String.Empty, Me.lueServiceName.EditValue.ToString)
		Dim dt As String = String.Empty
		Dim lastCheck As Boolean = True

		If Not Me.dateEditServiceDate.EditValue Is Nothing Then
			dt = Me.dateEditServiceDate.EditValue.ToString
		End If
		Dim userName As String = String.Empty
		If Not lueAdvisor.EditValue Is Nothing Then userName = Me.lueAdvisor.EditValue.ToString

#If DEBUG Then
		m_PaymentUtilWebServiceUri = "http://asmx.domain.com/wsSPS_services/SPCustomerPaymentServices.asmx"
		Trace.WriteLine(Customer_ID)
#End If

		Dim webservice As New SPCustomerPaymentServicesWebService.SPCustomerPaymentServicesSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_PaymentUtilWebServiceUri)

		Dim searchResult As List(Of PaymentSearchResultDTO) = Nothing
		Try
			' Read data over webservice
			searchResult = webservice.GetCurrentListOfServices(Customer_ID, userName, dt, serviceName, lueYear.EditValue, lueMonth.EditValue).ToList

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		'Dim i As Integer = 0
		For Each result In searchResult

			Dim viewData = New PaymentSearchViewData With {
				.RecID = result.RecID,
				.CustomerGuid = result.CustomerGuid,
				.UserGuid = result.UserGuid,
				.ServiceName = result.ServiceName,
				.ServiceDate = result.ServiceDate,
				.CreatedOn = result.CreatedOn,
				.CreatedFrom = result.CreatedFrom,
				.JobID = result.JobID,
				.AuthorizedCredit = If(result.Validated, Math.Max(result.AuthorizedCredit, 1), 0),
				.AuthorizedItems = If(result.Validated, result.AuthorizedItems, 0),
				.BookedPayment = result.BookedPayment,
				.BookedDate = result.BookedDate
			}
			'#If Not DEBUG Then
			'			If m_InitializationData.UserData.UserNr = 1 AndAlso searcheCallData AndAlso lastCheck AndAlso Not result.Validated Then
			'				'LoadMessageData(result.JobID)
			'				'viewData.AuthorizedItems = m_eCallMessageInfo.ItemCount
			'				'viewData.AuthorizedCredit = result.AuthorizedCredit
			'				'searcheCallData = m_eCallMessageInfo.MessageResponse.ServiceResponse.ResponseCode = 0
			'			End If

			'#End If

			listDataSource.Add(viewData)


		Next

		Return listDataSource

	End Function

	''' <summary>
	''' Finish web service call.
	''' </summary>
	Private Sub FinishCVLizerlistWebserviceCallTask(ByVal t As Task(Of BindingList(Of PaymentSearchViewData)))

		Try
			Select Case t.Status
				Case TaskStatus.RanToCompletion
					' Webservice call was successful.
					m_SuppressUIEvents = True
					'Dim data = t.Result.Where(Function(p) Year(p.ServiceDate) = lueYear.EditValue).FirstOrDefault()
					grdCurrentList.DataSource = t.Result
					m_SuppressUIEvents = False

				Case TaskStatus.Faulted
					' Something went wrong -> log error.
					m_Logger.LogError(t.Exception.ToString())
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht geladen werden."))

				Case Else
					' Do nothing
			End Select
		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		SplashScreenManager.CloseForm(False)
		m_eCallMessageInfo = Nothing
		btnSearch.Enabled = True
		bsiCurrentReccount.Caption = If(bsiCurrentCount.Visibility = DevExpress.XtraBars.BarItemVisibility.Always, String.Format("{0}", gvCurrentList.RowCount), String.Empty)

	End Sub


#End Region


#Region "load WOS data"

	''' <summary>
	''' Search for employee doc wos over web service.
	''' </summary>
	Private Sub SearchWOSEmployeeDoclistViaWebService()

		If lueCustomer.EditValue Is Nothing Then Return
		Dim uiSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext()
		bsiPaidReccount.Caption = ""

		btnSearch.Enabled = False
		grdWOSDoc.DataSource = Nothing

		Task(Of BindingList(Of EmployeeWOSViewData)).Factory.StartNew(Function() PerformWOSEmployeeDoclistWebserviceCallAsync(),
																						CancellationToken.None,
																						TaskCreationOptions.None,
																						TaskScheduler.Default).ContinueWith(Sub(t) FinishWOSEmployeeDoclistWebserviceCallTask(t), CancellationToken.None,
																																								TaskContinuationOptions.None, uiSynchronizationContext)

	End Sub

	''' <summary>
	'''  Performs employee wos doc check asynchronous.
	''' </summary>
	''' <returns>The report response.</returns>
	Private Function PerformWOSEmployeeDoclistWebserviceCallAsync() As BindingList(Of EmployeeWOSViewData)

		Dim listDataSource As BindingList(Of EmployeeWOSViewData) = New BindingList(Of EmployeeWOSViewData)
		Dim Customer_ID As String = m_EmployeeWOSID
		Dim serviceName As String = If(Me.lueServiceName.EditValue Is Nothing, String.Empty, Me.lueServiceName.EditValue.ToString)
		Dim dt As String = String.Empty
		If Not Me.dateEditServiceDate.EditValue Is Nothing Then
			dt = Me.dateEditServiceDate.EditValue.ToString
		End If
		Dim userName As String = String.Empty
		If Not String.IsNullOrWhiteSpace(Me.lueAdvisor.EditValue) Then userName = Me.lueAdvisor.EditValue

#If DEBUG Then
		Customer_ID = m_EmployeeWOSID
#End If

		Dim webservice As New SPWOSEmployeeWebService.SPWOSEmployeeUtilitiesSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_EmployeeWosUtilWebServiceUri)

		Dim searchResult As List(Of EmployeeWOSDataDTO) = Nothing
		Try
			m_Logger.LogDebug(String.Format("WOS-Query for Customer_ID: {0}", Customer_ID))

			' Read data over webservice
			searchResult = webservice.ListEmployeeDocuments(Customer_ID, lueYear.EditValue, lueMonth.EditValue).ToList

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try


		For Each result In searchResult

			Dim viewData = New EmployeeWOSViewData With {
				.ID = result.ID,
				.Customer_ID = result.Customer_ID,
				.EmployeeNumber = result.EmployeeNumber,
				.DocGuid = result.DocGuid,
				.EmployeeFirstName = result.EmployeeFirstName,
				.EmployeeLastName = result.EmployeeLastName,
				.DocumentArt = result.DocumentArt,
				.DocumentInfo = result.DocumentInfo,
				.EmploymentNumber = result.EmploymentNumber,
				.PayrollNumber = result.PayrollNumber,
				.LastNotification = result.LastNotification,
				.ReportNumber = result.ReportNumber,
				.TransferedOn = result.TransferedOn,
				.TransferedUser = result.TransferedUser
			}

			listDataSource.Add(viewData)

		Next


		Return listDataSource

	End Function

	''' <summary>
	''' Finish employee wos doc web service call.
	''' </summary>
	Private Sub FinishWOSEmployeeDoclistWebserviceCallTask(ByVal t As Task(Of BindingList(Of EmployeeWOSViewData)))

		Try

			Select Case t.Status
				Case TaskStatus.RanToCompletion
					' Webservice call was successful.
					m_SuppressUIEvents = True
					grdWOSDoc.DataSource = t.Result

					m_SuppressUIEvents = False

				Case TaskStatus.Faulted
					' Something went wrong -> log error.
					m_Logger.LogError(t.Exception.ToString())
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht geladen werden."))

				Case Else
					' Do nothing
			End Select

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		btnSearch.Enabled = True
		bsiPaidReccount.Caption = String.Format("{0}", gvWOSDoc.RowCount)

	End Sub


	''' <summary>
	''' Search for customer doc wos over web service.
	''' </summary>
	Private Sub SearchWOSCustomerDoclistViaWebService()

		If lueCustomer.EditValue Is Nothing Then Return
		Dim uiSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext()
		bsiPaidReccount.Caption = ""

		btnSearch.Enabled = False
		grdWOSDoc.DataSource = Nothing

		Task(Of BindingList(Of CustomerWOSViewData)).Factory.StartNew(Function() PerformWOSCustomerDoclistWebserviceCallAsync(),
																						CancellationToken.None,
																						TaskCreationOptions.None,
																						TaskScheduler.Default).ContinueWith(Sub(t) FinishWOSCustomerDoclistWebserviceCallTask(t), CancellationToken.None,
																																								TaskContinuationOptions.None, uiSynchronizationContext)

	End Sub

	''' <summary>
	'''  Performs customer wos doc check asynchronous.
	''' </summary>
	''' <returns>The report response.</returns>
	Private Function PerformWOSCustomerDoclistWebserviceCallAsync() As BindingList(Of CustomerWOSViewData)

		Dim listDataSource As BindingList(Of CustomerWOSViewData) = New BindingList(Of CustomerWOSViewData)
		Dim Customer_ID As String = m_CustomerWOSID
		Dim serviceName As String = If(Me.lueServiceName.EditValue Is Nothing, String.Empty, Me.lueServiceName.EditValue.ToString)
		Dim dt As String = String.Empty
		If Not Me.dateEditServiceDate.EditValue Is Nothing Then
			dt = Me.dateEditServiceDate.EditValue.ToString
		End If
		Dim userName As String = String.Empty
		If Not String.IsNullOrWhiteSpace(Me.lueAdvisor.EditValue) Then userName = Me.lueAdvisor.EditValue

#If DEBUG Then
		Customer_ID = m_CustomerWOSID
#End If

		Dim webservice As New SPWOSCustomerWebService.SPWOSCustomerUtilitiesSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_CustomerWosUtilWebServiceUri)

		Dim searchResult As List(Of CustomerWOSDataDTO) = Nothing
		Try
			m_Logger.LogDebug(String.Format("WOS-Query for Customer_ID: {0}", Customer_ID))

			' Read data over webservice
			searchResult = webservice.ListCustomerDocuments(Customer_ID, lueYear.EditValue, lueMonth.EditValue).ToList

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try


		For Each result In searchResult

			Dim viewData = New CustomerWOSViewData With {
				.ID = result.ID,
				.CustomerGuid = result.CustomerGuid,
				.CheckedOn = result.CheckedOn,
				.CRepesponsibleNumber = result.CRepesponsibleNumber,
				.Customer_ID = result.Customer_ID,
				.DocGuid = result.DocGuid,
				.CustomerName = result.CustomerName,
				.CustomerNumber = result.CustomerNumber,
				.DocumentArt = result.DocumentArt,
				.DocumentInfo = result.DocumentInfo,
				.EmploymentNumber = result.EmploymentNumber,
				.GetOn = result.GetOn,
				.GetResult = result.GetResult,
				.InvoiceNumber = result.InvoiceNumber,
				.LastNotification = result.LastNotification,
				.ReportNumber = result.ReportNumber,
				.TransferedOn = result.TransferedOn,
				.TransferedUser = result.TransferedUser,
				.ZFirstName = result.ZFirstName,
				.ZHDGuid = result.ZHDGuid,
				.ZLastName = result.ZLastName
			}

			listDataSource.Add(viewData)

		Next


		Return listDataSource

	End Function

	''' <summary>
	''' Finish customer wos doc web service call.
	''' </summary>
	Private Sub FinishWOSCustomerDoclistWebserviceCallTask(ByVal t As Task(Of BindingList(Of CustomerWOSViewData)))

		Try

			Select Case t.Status
				Case TaskStatus.RanToCompletion
					' Webservice call was successful.
					m_SuppressUIEvents = True
					grdWOSDoc.DataSource = t.Result

					m_SuppressUIEvents = False

				Case TaskStatus.Faulted
					' Something went wrong -> log error.
					m_Logger.LogError(t.Exception.ToString())
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht geladen werden."))

				Case Else
					' Do nothing
			End Select

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		btnSearch.Enabled = True
		bsiPaidReccount.Caption = String.Format("{0}", gvWOSDoc.RowCount)

	End Sub



	''' <summary>
	''' Search for wos Notification over web service.
	''' </summary>
	Private Sub SearchWOSNotificationlistViaWebService()

		If lueCustomer.EditValue Is Nothing Then Return
		Dim uiSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext()
		bsiPaidReccount.Caption = ""

		btnSearch.Enabled = False
		grdWOSMessage.DataSource = Nothing

		Task(Of BindingList(Of WOSNotificationViewData)).Factory.StartNew(Function() PerformWOSNotificationlistWebserviceCallAsync(),
																						CancellationToken.None,
																						TaskCreationOptions.None,
																						TaskScheduler.Default).ContinueWith(Sub(t) FinishWOSNotificationlistWebserviceCallTask(t), CancellationToken.None,
																																								TaskContinuationOptions.None, uiSynchronizationContext)

	End Sub

	''' <summary>
	'''  Performs wos notification check asynchronous.
	''' </summary>
	''' <returns>The report response.</returns>
	Private Function PerformWOSNotificationlistWebserviceCallAsync() As BindingList(Of WOSNotificationViewData)

		Dim listDataSource As BindingList(Of WOSNotificationViewData) = New BindingList(Of WOSNotificationViewData)
		Dim Customer_ID As String = m_EmployeeWOSID
		Dim serviceName As String = If(Me.lueServiceName.EditValue Is Nothing, String.Empty, Me.lueServiceName.EditValue.ToString)
		Dim dt As String = String.Empty
		If Not Me.dateEditServiceDate.EditValue Is Nothing Then
			dt = Me.dateEditServiceDate.EditValue.ToString
		End If
		Dim userName As String = String.Empty
		If Not String.IsNullOrWhiteSpace(Me.lueAdvisor.EditValue) Then userName = Me.lueAdvisor.EditValue

#If DEBUG Then
		Customer_ID = m_EmployeeWOSID
#End If

		Dim webservice As New SPNotificationWebService.SPNotificationSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_NotificationUtilWebServiceUri)

		Dim searchResult As List(Of WOSNotificationDTO) = Nothing
		Try
			' Read data over webservice
			Dim searchDataResult = webservice.GetWOSMailNotifications(Customer_ID, lueYear.EditValue, lueMonth.EditValue).ToList

			Dim data = CType(lueServiceName.GetSelectedDataRow, ServiceNameViewData)
			If data.ItemValue = "WOSEmployee" Then
				searchResult = searchDataResult.Where(Function(s) s.DocLink = "http://edoc.domain.com/sponlinedoc/DefaultPage.aspx?sp={TMPL_VAR name='MAOwner_Guid'}").ToList()
			ElseIf data.ItemValue = "WOSCustomer" Then
				searchResult = searchDataResult.Where(Function(s) s.DocLink = "http://edoc.domain.com/sponlinedoc/DefaultPage.aspx?zhd={TMPL_VAR name='ZHD_Guid'}" OrElse s.DocLink = "http://edoc.domain.com/sponlinedoc/DefaultPage.aspx?kd={TMPL_VAR name='KD_Guid'}").ToList()
			End If
			If searchResult Is Nothing Then
				m_Logger.LogWarning(String.Format("WOS Document could not be loaded! {0}", Customer_ID))
				Return Nothing
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try
		If searchResult Is Nothing Then Return Nothing

		For Each result In searchResult

			Dim viewData = New WOSNotificationViewData With {
				.ID = result.ID,
				.CreatedOn = result.CreatedOn,
				.Customer_ID = result.Customer_ID,
				.DocLink = result.DocLink,
				.MailBody = result.MailBody,
				.MailFrom = result.MailFrom,
				.MailSubject = result.MailSubject,
				.MailTo = result.MailTo,
				.RecipientGuid = result.RecipientGuid,
				.Result = result.Result
			}

			listDataSource.Add(viewData)

		Next


		Return listDataSource

	End Function

	''' <summary>
	''' Finish wos notification web service call.
	''' </summary>
	Private Sub FinishWOSNotificationlistWebserviceCallTask(ByVal t As Task(Of BindingList(Of WOSNotificationViewData)))

		Try

			Select Case t.Status
				Case TaskStatus.RanToCompletion
					' Webservice call was successful.
					m_SuppressUIEvents = True
					grdWOSMessage.DataSource = t.Result

					m_SuppressUIEvents = False

				Case TaskStatus.Faulted
					' Something went wrong -> log error.
					m_Logger.LogError(t.Exception.ToString())
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht geladen werden."))

				Case Else
					' Do nothing
			End Select

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		btnSearch.Enabled = True
		bsiPaidReccount.Caption = String.Format("{0}", gvPaidlist.RowCount)

	End Sub



#End Region


	''' <summary>
	''' Handles drop down button clicks.
	''' </summary>
	Private Sub OnDropDown_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

		Const ID_OF_DELETE_BUTTON As Int32 = 1

		' If delete button has been clicked reset the drop down.
		If e.Button.Index = ID_OF_DELETE_BUTTON Then

			If TypeOf sender Is LookUpEdit Then
				Dim lookupEdit As LookUpEdit = CType(sender, LookUpEdit)
				lookupEdit.EditValue = Nothing
			ElseIf TypeOf sender Is ComboBoxEdit Then
				Dim comboboxEdit As ComboBoxEdit = CType(sender, ComboBoxEdit)
				comboboxEdit.EditValue = Nothing
			End If
		End If
	End Sub

#End Region

	Sub OngvCurrentList_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvCurrentList.RowCellClick
		lbleCallResponseInfo.Text = String.Empty
		Dim eCall As Boolean = False

		m_CurrentJobID = String.Empty
		m_eCallMessageInfo = Nothing

		Dim data = CType(lueServiceName.GetSelectedDataRow, ServiceNameViewData)
		If data Is Nothing Then Return

		Select Case data.ItemValue
			Case "Interne Nachrichten (EMail und Telefax)", "WOS", "CVLIZER_SCAN"

			Case "ECALL_FAXCREDIT"
				eCall = True

			Case "ECALL_SMSCREDIT"
				eCall = True

			Case Else
				Return

		End Select

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvCurrentList.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then

				If eCall Then
					Dim viewData = CType(dataRow, PaymentSearchViewData)

					If Not viewData Is Nothing Then
						m_CurrentJobID = viewData.JobID
						If Not String.IsNullOrWhiteSpace(m_CurrentJobID) Then
							Trace.WriteLine(m_CurrentJobID)
							If m_InitializationData.UserData.UserNr = 1 Then
								If data.ItemValue = "ECALL_FAXCREDIT" Then
									m_CurrentJobID = Mid(m_CurrentJobID, 1, 50)
								End If

								LoadMessageData(m_CurrentJobID)
								ShowMessageInfo(m_CurrentJobID)
							End If

						End If
					End If

				Else
					Dim viewData = CType(dataRow, ListingMailData)
					If Not viewData.ID.GetValueOrDefault(0) Then

						Select Case column.Name.ToLower
							Case "manr".ToLower, "employeefullname".ToLower, "employeelastname".ToLower, "employeefirstname".ToLower
								If viewData.MANr.HasValue Then OpenSelectedEmployee(viewData.MANr)

							Case "kdnr".ToLower, "customername".ToLower
								If viewData.MANr.HasValue Then OpenSelectedCustomer(viewData.KDNr)

							Case "ZHDNr".ToLower, "cresponsiblesalution".ToLower, "cresponsiblelastname".ToLower, "cresponsiblefirstname".ToLower, "responsiblePersonFullname".ToLower
								If viewData.ZHDNr.HasValue Then OpenSelectedCResponsible(viewData.KDNr, viewData.ZHDNr)


							Case Else
								Dim frm As New frmMessageDetails(m_InitializationData)
								Dim success = frm.LoadData(viewData.customer_id, viewData.ID)

								frm.Show()
								frm.BringToFront()

						End Select


					End If

				End If

			End If

		End If

	End Sub

	Private Sub OnGVDetail_ColumnFilterChanged(sender As Object, e As System.EventArgs)
		Me.bsiCurrentReccount.Caption = gvCurrentList.RowCount
		Me.bsiPaidReccount.Caption = gvPaidlist.RowCount
	End Sub

	Private Sub OnGvCurrentList_CustomColumnDisplayText(ByVal sender As Object, ByVal e As CustomColumnDisplayTextEventArgs) Handles gvCurrentList.CustomColumnDisplayText

		If e.Column.FieldName = "AuthorizedCreditCount" OrElse e.Column.FieldName = "AuthorizedItems" Then
			If e.Value = 0 Then e.DisplayText = String.Empty
		End If

	End Sub

	Private Sub OngvPaidlist_CustomColumnDisplayText(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs) Handles gvPaidlist.CustomColumnDisplayText

		If e.Column.FieldName = "AuthorizedCreditCount" OrElse e.Column.FieldName = "AuthorizedItems" Then
			If e.Value = 0 Then e.DisplayText = String.Empty
		End If

	End Sub

	Private Sub OngvWOSDocCustomColumnDisplayText(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs) Handles gvWOSDoc.CustomColumnDisplayText

		If e.Column.FieldName = "EmploymentNumber" OrElse e.Column.FieldName = "ReportNumber" OrElse e.Column.FieldName = "PayrollNumber" OrElse e.Column.FieldName = "InvoiceNumber" OrElse e.Column.FieldName = "CRepesponsibleNumber" OrElse e.Column.FieldName = "CustomerNumber" OrElse e.Column.FieldName = "CustomerNumber" Then
			If e.Value = 0 Then e.DisplayText = String.Empty
		End If
	End Sub


	''' <summary>
	''' handels printing grid for gridcontent
	''' </summary>
	Private Sub bbiPrint_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick
		Dim grd = grdCurrentList

		If xtabMain.SelectedTabPage Is xtabAktuellList Then
			grd = grdCurrentList
			If Not grdCurrentList.IsPrintingAvailable Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Drucken ist nicht möglich. Bitte kontaktieren Sie Ihr Softwarehersteller."))
				Return
			End If

		ElseIf xtabMain.SelectedTabPage Is xtabDefinitiv Then
			grd = grdpaidlist
		End If

		' Opens the Preview window. 
		grd.ShowPrintPreview()


	End Sub


#Region "WOS methods"

	Sub OngvWOSDocRowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvWOSDoc.RowCellClick

		Dim data = CType(lueServiceName.GetSelectedDataRow, ServiceNameViewData)
		If data Is Nothing Then Return

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvWOSDoc.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim docData As New WOSDocumentData(m_InitializationData)
				docData.EmployeeWOSID = m_EmployeeWOSID
				docData.CustomerWOSID = m_CustomerWOSID


				Select Case data.ItemValue
					Case "WOSEmployee"
						Dim viewData = CType(dataRow, EmployeeWOSViewData)
						docData.WOSModul = WOSDocumentData.WOSEnum.EMPLOYEE
						docData.LoadDocument(WOSDocumentData.WOSEnum.EMPLOYEE, viewData.ID)

					Case "WOSCustomer"
						Dim viewData = CType(dataRow, CustomerWOSViewData)
						docData.WOSModul = WOSDocumentData.WOSEnum.CUSTOMER
						docData.LoadDocument(WOSDocumentData.WOSEnum.CUSTOMER, viewData.ID)

					Case Else
						Return

				End Select

			End If
		End If

	End Sub


#End Region


#Region "eCall methodes"

	Public Function GetState(ByVal jobID As String) As StatusResponse
		Dim result As SPeCallWebService.StatusResponse
		Dim m_eCallService As SPeCallWebService.eCallSoapClient

		m_eCallService = New SPeCallWebService.eCallSoapClient
		ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
		m_eCallService.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_eCallWebServiceUri)

		Try
			result = m_eCallService.GetStateBasic(m_AccountName, m_AccountPassword, jobID, Nothing)

			Return result

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		Return Nothing

	End Function

	Private Sub ShowMessageInfo(ByVal jobID As String)
		lbleCallResponseInfo.Text = String.Empty
		If m_InitializationData.UserData.UserNr <> 1 Then Return

		Dim result = m_eCallMessageInfo
		If result Is Nothing Then Return

		lbleCallResponseInfo.Text = String.Format("ErrorState: {1}{0}ResponseCode/Text: ({2}) {3}{0}Adresse: {4}{0}Credits/Anzahl: ({5}) {6:f6}{0}FinishDate: {7}",
																		 vbNewLine,
																		 result.MessageResponse.JobResponse.ErrorState,
																		 result.MessageResponse.ServiceResponse.ResponseCode,
																		 result.MessageResponse.ServiceResponse.ResponseText,
																		 result.MessageResponse.JobResponse.Address,
																		 result.UsedCredits,
																		 result.ItemCount,
																		 result.MessageResponse.JobResponse.FinishDate)

	End Sub

	Private Function LoadMessageData(ByVal jobID As String) As MessageInfo

		Dim validatedMessage = GetState(jobID)
		Dim result As MessageInfo = Nothing

		If Not validatedMessage Is Nothing Then

			Dim usedCredits As Decimal = CType(validatedMessage.JobResponse.PointsUsed, Decimal)
			Dim itemCount As Decimal
			Select Case validatedMessage.JobResponse.JobType
				Case 1
					itemCount = usedCredits / 1.1

				Case 6
					itemCount = usedCredits / 1.5

				Case Else
					itemCount = 0
			End Select
			itemCount = If(itemCount <> 0, Math.Max(itemCount, 1), itemCount)

			result = New MessageInfo With {.MessageResponse = validatedMessage,
																	 .ItemCount = itemCount,
																	 .UsedCredits = usedCredits}

		End If
		m_eCallMessageInfo = result

		Return result

	End Function

#End Region



	Private Sub OpenSelectedEmployee(ByVal employeeNumber As Integer)

		Try
			Dim hub = MessageService.Instance.Hub
			Dim openMng As New OpenEmployeeMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, employeeNumber)
			hub.Publish(openMng)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
		End Try

	End Sub

	Private Sub OpenSelectedCustomer(ByVal customerNumber As Integer)

		Try
			Dim hub = MessageService.Instance.Hub
			Dim openMng As New OpenCustomerMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, customerNumber)
			hub.Publish(openMng)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
		End Try

	End Sub

	Sub OpenSelectedCResponsible(ByVal customerNumber As Integer, ByVal responsibleNumber As Integer)

		'TODO: 
		' Antonino: 22.12.17: Sovle this
		'Dim responsiblePersonsFrom = New frmResponsiblePerson(m_InitializationData)

		'If (responsiblePersonsFrom.LoadResponsiblePersonData(customerNumber, responsibleNumber)) Then
		'	responsiblePersonsFrom.Show()
		'End If

	End Sub



#Region "View helper classes"

	Class MessageInfo
		Public Property MessageResponse As StatusResponse
		Public Property UsedCredits As Decimal
		Public Property ItemCount As Decimal

	End Class

	''' <summary>
	''' Customer search view data.
	''' </summary>
	Class CustomerViewData

		Public Property customer_Name As String
		Public Property customer_ID As String

	End Class

	''' <summary>
	''' Customer Username search view data.
	''' </summary>
	Class CustomerUserNameViewData
		Public Property UserName As String
	End Class

	''' <summary>
	''' Bank search view data.
	''' </summary>
	Class PaymentSearchViewData

		Public Property RecID As Integer
		Public Property CustomerGuid As String
		Public Property UserGuid As String
		Public Property ServiceName As String
		Public Property ServiceDate As DateTime?
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property JobID As String
		Public Property AuthorizedItems As Decimal
		Public Property AuthorizedCredit As Decimal?
		Public Property BookedPayment As Boolean?
		Public Property BookedDate As DateTime?
		Public Property validated As Boolean

		Public ReadOnly Property JobID2Validate As String
			Get
				Dim job As String() = Nothing

				job = JobID.Split("|")

				Return job(0)
			End Get
		End Property
	End Class


	''' <summary>
	''' Paid search view data (tbl_eCallLog).
	''' </summary>
	Class PaidSearchViewData

		Public Property RecID As Integer
		Public Property ServiceDate As DateTime?
		Public Property CustomerGuid As String
		Public Property Content As String
		Public Property ServiceName As String
		Public Property Recipient As String
		Public Property AuthorizedItems As Decimal
		Public Property Status As Integer
		Public Property Sender As String
		Public Property UserData As String
		Public Property BookedPayment As Boolean?
		Public Property BookedDate As DateTime?
		Public Property ResultCode As Integer?
		Public Property ResultMessage As String

		Public ReadOnly Property StateCondition As Boolean
			Get
				Return (If(Status = 2 OrElse Status = 42, False, True))
			End Get
		End Property

	End Class


	''' <summary>
	''' wos document view data.
	''' </summary>
	Class CustomerWOSViewData

		Public Property ID As Integer
		Public Property Customer_ID As String
		Public Property ZHDGuid As String
		Public Property EmploymentNumber As Integer?
		Public Property ReportNumber As Integer?
		Public Property InvoiceNumber As Integer?
		Public Property CRepesponsibleNumber As Integer?
		Public Property CustomerGuid As String
		Public Property CustomerNumber As Integer?
		Public Property CustomerName As String
		Public Property ZLastName As String
		Public Property ZFirstName As String
		Public Property DocumentArt As String
		Public Property DocumentInfo As String

		Public Property TransferedOn As DateTime?
		Public Property TransferedUser As String
		Public Property CheckedOn As DateTime?
		Public Property GetResult As Integer?
		Public Property GetOn As DateTime?
		Public Property LastNotification As DateTime?

		Public Property DocGuid As String
		Public Property ScanContent As Byte()

		Public ReadOnly Property CustomerWOSLink() As String
			Get
				Return String.Format("http://edoc.domain.com/sponlinedoc/DefaultPage.aspx?kd={0}", ZHDGuid)
			End Get
		End Property

		Public ReadOnly Property ResponsiblePersonFullName() As String
			Get
				Return String.Format("{0}{1}{2}", ZLastName, If(String.IsNullOrWhiteSpace(ZFirstName), "", ", "), ZFirstName)
			End Get
		End Property

		Public ReadOnly Property ResponsiblePersonWOSLink() As String
			Get
				Return String.Format("http://edoc.domain.com/sponlinedoc/DefaultPage.aspx?ZHD={0}", ZHDGuid)
			End Get
		End Property


	End Class


	Class EmployeeWOSViewData

		Public Property ID As Integer
		Public Property Customer_ID As String
		Public Property EmployeeGuid As String
		Public Property EmploymentNumber As Integer?
		Public Property ReportNumber As Integer?
		Public Property PayrollNumber As Integer?

		Public Property EmployeeNumber As Integer?
		Public Property EmployeeLastName As String
		Public Property EmployeeFirstName As String
		Public Property DocumentArt As String
		Public Property DocumentInfo As String

		Public Property TransferedOn As DateTime?
		Public Property TransferedUser As String
		Public Property LastNotification As DateTime?

		Public Property DocGuid As String
		Public Property ScanContent As Byte()


		Public ReadOnly Property EmployeeWOSLink() As String
			Get
				Return String.Format("http://edoc.domain.com/sponlinedoc/DefaultPage.aspx?sp={0}", EmployeeGuid)
			End Get
		End Property

		Public ReadOnly Property EmployeeFullName() As String
			Get
				Return String.Format("{0}{1}{2}", EmployeeLastName, If(String.IsNullOrWhiteSpace(EmployeeFirstName), "", ", "), EmployeeFirstName)
			End Get
		End Property

	End Class


	Class WOSNotificationViewData

		Public Property ID As Integer
		Public Property Customer_ID As String
		Public Property MailFrom As String
		Public Property MailTo As String
		Public Property Result As String
		Public Property MailSubject As String
		Public Property MailBody As String
		Public Property DocLink As String
		Public Property RecipientGuid As String

		Public Property CreatedOn As DateTime?


	End Class

	''' <summary>
	''' Advisor view data.
	''' </summary>
	Private Class AdvisorViewData

		Public Property KST As String
		Public Property FirstName As String
		Public Property LastName As String

		Public ReadOnly Property LastName_FirstNameWithComa As String
			Get
				Return String.Format("{0}, {1}", LastName, FirstName)
			End Get
		End Property

		Public ReadOnly Property LastName_FirstName As String
			Get
				Return String.Format("{0} {1}", LastName, FirstName)
			End Get
		End Property

		Public ReadOnly Property FirstName_LastName As String
			Get
				Return String.Format("{0} {1}", FirstName, LastName)
			End Get
		End Property

	End Class


	Class ServiceNameViewData

		Public Property ItemValue As String
		Public Property DisplayText As String

	End Class


	''' <summary>
	''' Wraps an integer value.
	''' </summary>
	Class IntegerValueViewWrapper
		Public Property Value As Integer
	End Class


#End Region


	Private Sub XtraTabControl1_SelectedPageChanged(sender As Object, e As DevExpress.XtraTab.TabPageChangedEventArgs) Handles xtabMain.SelectedPageChanged

		lbleCallResponseInfo.Text = String.Empty
		lblBeraterIn.Visible = False
		lueAdvisor.Visible = False
		bsiCurrentCount.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
		bsiCurrentReccount.Caption = String.Empty

		If Not e.Page Is xtabDefinitiv Then
			lblBeraterIn.Visible = True
			lueAdvisor.Visible = True

			bsiCurrentCount.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
			bsiCurrentReccount.Caption = gvCurrentList.RowCount

		End If

	End Sub

End Class



Public Class ValidatingShortMessage


	Public Property ReceiverId As Integer

	Public Property Address As String
	Public Property Message As String

	Public Property JobId As String

	Public Property ResponseCode As Long
	Public Property ResponseText As String

	Public Property SendState As Long
	Public Property ErrorState As Long

	Public Property FinishDate As Date

	Public Property PointsUsed As Double
	Public Property AnswerAddress As String


	Sub New()
		ResponseCode = 0
		SendState = 0
		ErrorState = 0
		ResponseText = String.Empty
		PointsUsed = 0.0
	End Sub

	Public Sub UpdateStatus(ByVal status As ValidatingShortMessage)
		If status.ResponseCode <> -1 Then
			ResponseCode = status.ResponseCode
		End If

		If status.SendState <> -1 Then
			SendState = status.SendState
		End If

		If status.ErrorState <> -1 Then
			ErrorState = status.ErrorState
		End If

		If Not String.IsNullOrEmpty(status.ResponseText) Then
			ResponseText = status.ResponseText
		End If

	End Sub

	Public Function Clone() As ValidatingShortMessage
		Return DirectCast(Me.MemberwiseClone(), ValidatingShortMessage)
	End Function


End Class
