
Option Strict Off

Imports SP.Infrastructure.UI.UtilityUI

Imports System.Reflection.Assembly
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports System.Runtime.CompilerServices
Imports DevExpress.XtraEditors.Controls

Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid.Views.Base
Imports System.Threading

Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports DevExpress.LookAndFeel

Imports SPMASearch.ClsDataDetail
Imports SP.Infrastructure.Logging
Imports SP.Internal.Automations.BaseTable
Imports SP.Internal.Automations
Imports System.ComponentModel
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.Listing
Imports DevExpress.XtraEditors


Public Class frmMASearch
	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The data access object.
	''' </summary>
	Protected m_EmployeeDataAccess As IEmployeeDatabaseAccess

	''' <summary>
	''' The common database access.
	''' </summary>
	Protected m_CommonDatabaseAccess As ICommonDatabaseAccess

	''' <summary>
	''' The listing database access.
	''' </summary>
	Protected m_ListingDatabaseAccess As IListingDatabaseAccess

	Private m_xml As New ClsXML
	Private _ClsFunc As New ClsDivFunc

	Private m_md As Mandant
	Private m_utilities As Utilities

	Private _ClsReg As New SPProgUtility.ClsDivReg
	Private _ClsLog As New SPProgUtility.ClsEventLog

	Private iLogedUSNr As Integer = 0
	Private strLastSortBez As String
	Private Stopwatch As Stopwatch = New Stopwatch()
	Public Shared frmMyLV As frmMASearch_LV


	Private pcc As New DevExpress.XtraBars.PopupControlContainer
	Private Property ShortSQLQuery As String
	Private Property GetMDDbName As String
	Private Property GetMDGuid As String

	Private PrintListingThread As Thread
	Private Property PrintJobNr As String
	Private Property SQL4Print As String
	Private Property bPrintAsDesign As Boolean

	Private Property TranslatedPage As New List(Of Boolean)

	Private m_UtilityUI As SP.Infrastructure.UI.UtilityUI
	Private m_BaseTableUtil As SPSBaseTables

	Private m_EmploymentTypeData As BindingList(Of EmploymentTypeData)
	Private m_OtherEmploymentTypeData As BindingList(Of EmploymentTypeData)
	Private m_TypeofStayData As BindingList(Of TypeOfStayData)
	Private m_PermissionCategoryData As BindingList(Of SP.Internal.Automations.PermissionData)
	Private m_CantonData As IEnumerable(Of CantonData)
	Private m_PermissionData As BindingList(Of SP.Internal.Automations.PermissionData)
	Private m_CountryData As BindingList(Of SP.Internal.Automations.CVLBaseTableViewData)
	Private m_TaxData As BindingList(Of SP.Internal.Automations.TaxCodeData)


#Region "Construrctor"

	Public Sub New()

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		m_InitializationData = ClsDataDetail.m_InitialData
		m_md = New Mandant
		m_utilities = New Utilities
		m_UtilityUI = New SP.Infrastructure.UI.UtilityUI
		m_BaseTableUtil = New SPSBaseTables(m_InitializationData)

		m_EmploymentTypeData = New BindingList(Of EmploymentTypeData)
		m_OtherEmploymentTypeData = New BindingList(Of EmploymentTypeData)
		m_TypeofStayData = New BindingList(Of TypeOfStayData)
		m_PermissionCategoryData = New BindingList(Of SP.Internal.Automations.PermissionData)

		m_EmployeeDataAccess = New SP.DatabaseAccess.Employee.EmployeeDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_CommonDatabaseAccess = New SP.DatabaseAccess.Common.CommonDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_ListingDatabaseAccess = New ListingDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

		LoadMainDataViaWebserviceCall()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		Reset()
		LoadDropDownData()

		AddHandler Cbo_MAKontakt.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler Cbo_MAStatus1.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler Cbo_MAStatus2.ButtonClick, AddressOf OnDropDown_ButtonClick

		AddHandler Cbo_MAArbeitspensum.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler Cbo_MAKuendigungsfristen.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler Cbo_MAZahlungsart.ButtonClick, AddressOf OnDropDown_ButtonClick

		AddHandler lueForeignCategory.ButtonClick, AddressOf OnLookupEditDropDown_ButtonClick
		AddHandler lueCHPartner.ButtonClick, AddressOf OnLookupEditDropDown_ButtonClick
		AddHandler lueNoSpecialTax.ButtonClick, AddressOf OnLookupEditDropDown_ButtonClick
		AddHandler lueCertificateForResidenceReceived.ButtonClick, AddressOf OnLookupEditDropDown_ButtonClick
		AddHandler lueTypeofStay.ButtonClick, AddressOf OnLookupEditDropDown_ButtonClick
		AddHandler lueEmploymentType.ButtonClick, AddressOf OnLookupEditDropDown_ButtonClick
		AddHandler lueOtherEmploymentType.ButtonClick, AddressOf OnLookupEditDropDown_ButtonClick

		AddHandler luePermissionCode.ButtonClick, AddressOf OnLookupEditDropDown_ButtonClick
		AddHandler lueQSTCode.ButtonClick, AddressOf OnLookupEditDropDown_ButtonClick
		AddHandler lueCivilstate.ButtonClick, AddressOf OnLookupEditDropDown_ButtonClick
		AddHandler lueNationality.ButtonClick, AddressOf OnLookupEditDropDown_ButtonClick
		AddHandler lueCountry.ButtonClick, AddressOf OnLookupEditDropDown_ButtonClick

		AddHandler btnHideBerufGruppe.Click, AddressOf OnHideShow_ButtonClick
		AddHandler btnHideFachbereich.Click, AddressOf OnHideShow_ButtonClick
		AddHandler btnHideMABerufe.Click, AddressOf OnHideShow_ButtonClick
		AddHandler btnHideMASonstigeQualifikation.Click, AddressOf OnHideShow_ButtonClick

		AddHandler btnHideMABranchen.Click, AddressOf OnHideShow_ButtonClick
		AddHandler btnHideMAKommunikationsart.Click, AddressOf OnHideShow_ButtonClick
		AddHandler btnHideMASSprachen.Click, AddressOf OnHideShow_ButtonClick
		AddHandler btnHideMAMSprachen.Click, AddressOf OnHideShow_ButtonClick
		AddHandler btnHideMAAnstellung.Click, AddressOf OnHideShow_ButtonClick
		AddHandler btnHideMABeurteilung.Click, AddressOf OnHideShow_ButtonClick

	End Sub

#End Region


#Region "Lookup Edit Reset und Load..."

	Private Sub Reset()

		ResetMandantenDropDown()
		ResetQualifikationLst()

		ResetForeignCategoryDropDown()
		ResetEmploymentTypeDropDown()
		ResetOtherEmploymentTypeDropDown()
		ResetTypeofStayDropDown()
		ResetBirthplaceDropDown()

		ResetTrueFalseDropDown(lueCHPartner)
		ResetTrueFalseDropDown(lueNoSpecialTax)
		ResetTrueFalseDropDown(lueCertificateForResidenceReceived)

		ResetCountryDropDown()
		ResetNationalityDropDown()
		ResetPermissionCodeDropDown()
		ResetQSTCodeDropDown()
		ResetCommunityDropDown()
		ResetCivilstateDropDown()
		ResetAddressTypeDropDown()


		Cbo_MAStatus1.Properties.SeparatorChar = "#"
		Cbo_MAStatus2.Properties.SeparatorChar = "#"
		Cbo_MAKontakt.Properties.SeparatorChar = "#"

	End Sub


	''' <summary>
	''' Resets the Mandanten drop down.
	''' </summary>
	Private Sub ResetMandantenDropDown()

		lueMandant.Properties.Items.Clear()

		Dim _ClsFunc As New ClsDbFunc
		Dim Data = _ClsFunc.LoadMandantenData()

		lueMandant.Properties.DisplayMember = "MDName"
		lueMandant.Properties.ValueMember = "MDNr"

		lueMandant.Properties.DataSource = Data

	End Sub

	Private Sub ResetQualifikationLst()

		Lst_MABerufe.DisplayMember = "ProfessionText"
		Lst_MABerufe.ValueMember = "ProfessionText"

		Lst_MABerufe.DataSource = Nothing

	End Sub

	Private Sub ResetForeignCategoryDropDown()

		lueForeignCategory.Properties.DisplayMember = "Translated_Value"
		lueForeignCategory.Properties.ValueMember = "Rec_Value"

		Dim columns = lueForeignCategory.Properties.Columns
		columns.Clear()
		'columns.Add(New LookUpColumnInfo("Rec_Value", m_Translate.GetSafeTranslationValue("Code"), 50))
		columns.Add(New LookUpColumnInfo("Translated_Value", m_Translate.GetSafeTranslationValue("Kategorie"), 700))

		lueForeignCategory.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueForeignCategory.Properties.SearchMode = SearchMode.AutoComplete
		lueForeignCategory.Properties.AutoSearchColumnIndex = 0
		lueForeignCategory.Properties.NullText = String.Empty

		lueForeignCategory.Properties.PopupFormWidth = 1000
		lueForeignCategory.Properties.PopupSizeable = True

		lueForeignCategory.EditValue = Nothing
	End Sub

	Private Sub ResetEmploymentTypeDropDown()

		lueEmploymentType.Properties.ShowHeader = False
		lueEmploymentType.Properties.ShowFooter = False
		lueEmploymentType.Properties.DisplayMember = "Translated_Value"
		lueEmploymentType.Properties.ValueMember = "Rec_Value"

		Dim columns = lueEmploymentType.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("Translated_Value", 0, m_Translate.GetSafeTranslationValue("Beschäftigungsart")))

		lueEmploymentType.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueEmploymentType.Properties.SearchMode = SearchMode.AutoComplete
		lueEmploymentType.Properties.AutoSearchColumnIndex = 0

		lueEmploymentType.Properties.NullText = String.Empty
		lueEmploymentType.EditValue = Nothing

	End Sub

	Private Sub ResetOtherEmploymentTypeDropDown()

		lueOtherEmploymentType.Properties.ShowHeader = False
		lueOtherEmploymentType.Properties.ShowFooter = False
		lueOtherEmploymentType.Properties.DisplayMember = "Translated_Value"
		lueOtherEmploymentType.Properties.ValueMember = "Rec_Value"

		Dim columns = lueOtherEmploymentType.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("Translated_Value", 0, m_Translate.GetSafeTranslationValue("Weitere Beschäftigungsart")))

		lueOtherEmploymentType.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueOtherEmploymentType.Properties.SearchMode = SearchMode.AutoComplete
		lueOtherEmploymentType.Properties.AutoSearchColumnIndex = 0

		lueOtherEmploymentType.Properties.NullText = String.Empty
		lueOtherEmploymentType.EditValue = Nothing

	End Sub

	Private Sub ResetTypeofStayDropDown()

		lueTypeofStay.Properties.ShowHeader = False
		lueTypeofStay.Properties.ShowFooter = False
		lueTypeofStay.Properties.DisplayMember = "Translated_Value"
		lueTypeofStay.Properties.ValueMember = "Rec_Value"

		Dim columns = lueTypeofStay.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("Translated_Value", 0, m_Translate.GetSafeTranslationValue("Aufgenthaltsart")))

		lueTypeofStay.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueTypeofStay.Properties.SearchMode = SearchMode.AutoComplete
		lueTypeofStay.Properties.AutoSearchColumnIndex = 0

		lueTypeofStay.Properties.NullText = String.Empty
		lueTypeofStay.EditValue = Nothing

	End Sub

	Private Sub ResetBirthplaceDropDown()

		lueBirthPlace.Properties.DisplayMember = "FieldValue"
		lueBirthPlace.Properties.ValueMember = "FieldValue"

		Dim columns = lueBirthPlace.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("FieldValue", m_Translate.GetSafeTranslationValue("Heimatort"), 700))

		lueBirthPlace.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueBirthPlace.Properties.SearchMode = SearchMode.AutoComplete
		lueBirthPlace.Properties.AutoSearchColumnIndex = 0
		lueBirthPlace.Properties.NullText = String.Empty

		'lueBirthPlace.Properties.PopupFormWidth = 1000
		'lueBirthPlace.Properties.PopupSizeable = True

		lueBirthPlace.EditValue = Nothing
	End Sub

	Private Sub ResetTrueFalseDropDown(ByVal lookupedit As DevExpress.XtraEditors.LookUpEdit)

		lookupedit.Properties.DisplayMember = "ValueLabel"
		lookupedit.Properties.ValueMember = "FieldValue"

		Dim columns = lookupedit.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("ValueLabel", String.Empty, 700))

		lookupedit.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lookupedit.Properties.SearchMode = SearchMode.AutoComplete
		lookupedit.Properties.AutoSearchColumnIndex = 0
		lookupedit.Properties.NullText = String.Empty

		lookupedit.Properties.PopupFormWidth = 1000
		lookupedit.Properties.PopupSizeable = True

		lookupedit.EditValue = Nothing
	End Sub

	Private Sub ResetCountryDropDown()

		lueCountry.Properties.DisplayMember = "CountryDataViewData"
		lueCountry.Properties.ValueMember = "Code"

		Dim columns = lueCountry.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("CountryDataViewData", m_Translate.GetSafeTranslationValue("Länder"), 700))

		lueCountry.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueCountry.Properties.SearchMode = SearchMode.AutoComplete
		lueCountry.Properties.AutoSearchColumnIndex = 0
		lueCountry.Properties.NullText = String.Empty

		lueCountry.EditValue = Nothing

	End Sub

	Private Sub ResetNationalityDropDown()

		lueNationality.Properties.DisplayMember = "CountryDataViewData"
		lueNationality.Properties.ValueMember = "Code"

		Dim columns = lueNationality.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("CountryDataViewData", m_Translate.GetSafeTranslationValue("Nationalität"), 700))

		lueNationality.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueNationality.Properties.SearchMode = SearchMode.AutoComplete
		lueNationality.Properties.AutoSearchColumnIndex = 0
		lueNationality.Properties.NullText = String.Empty

		lueNationality.EditValue = Nothing

	End Sub

	Private Sub ResetPermissionCodeDropDown()

		luePermissionCode.Properties.DisplayMember = "PermissionCodeViewData"
		luePermissionCode.Properties.ValueMember = "RecValue"

		Dim columns = luePermissionCode.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("PermissionCodeViewData", m_Translate.GetSafeTranslationValue("Bewilligung"), 700))

		luePermissionCode.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		luePermissionCode.Properties.SearchMode = SearchMode.AutoComplete
		luePermissionCode.Properties.AutoSearchColumnIndex = 0
		luePermissionCode.Properties.NullText = String.Empty

		luePermissionCode.EditValue = Nothing

	End Sub

	Private Sub ResetQSTCodeDropDown()

		lueQSTCode.Properties.DisplayMember = "QSTCodeViewData"
		lueQSTCode.Properties.ValueMember = "QSTCode"

		Dim columns = lueQSTCode.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("QSTCodeViewData", m_Translate.GetSafeTranslationValue("Tarif"), 700))

		lueQSTCode.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueQSTCode.Properties.SearchMode = SearchMode.AutoComplete
		lueQSTCode.Properties.AutoSearchColumnIndex = 0
		lueQSTCode.Properties.NullText = String.Empty

		lueQSTCode.EditValue = Nothing

	End Sub

	Private Sub ResetCommunityDropDown()

		lueCommunity.Properties.Items.Clear()

		lueCommunity.Properties.DisplayMember = "CommunityViewData"
		lueCommunity.Properties.ValueMember = "CommunityCode"

		lueCommunity.Properties.DropDownRows = 10
		lueCommunity.Properties.ForceUpdateEditValue = DevExpress.Utils.DefaultBoolean.True

		lueCommunity.Properties.NullText = String.Empty
		lueCommunity.EditValue = Nothing

	End Sub

	Private Sub ResetCivilstateDropDown()

		lueCivilstate.Properties.DisplayMember = "CivilStateViewData"
		lueCivilstate.Properties.ValueMember = "GetField"

		Dim columns = lueCivilstate.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("CivilStateViewData", m_Translate.GetSafeTranslationValue("Zivilstand"), 700))

		lueCivilstate.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueCivilstate.Properties.SearchMode = SearchMode.AutoComplete
		lueCivilstate.Properties.AutoSearchColumnIndex = 0
		lueCivilstate.Properties.NullText = String.Empty

		lueCivilstate.EditValue = Nothing

	End Sub

	Private Sub ResetAddressTypeDropDown()

		lueAddressSource.Properties.DisplayMember = "AddressTypeLabel"
		lueAddressSource.Properties.ValueMember = "AddressType"

		Dim columns = lueAddressSource.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("AddressTypeLabel", m_Translate.GetSafeTranslationValue("Quelle"), 700))

		lueAddressSource.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueAddressSource.Properties.SearchMode = SearchMode.AutoComplete
		lueAddressSource.Properties.AutoSearchColumnIndex = 0
		lueAddressSource.Properties.NullText = String.Empty

		lueAddressSource.EditValue = Nothing

	End Sub

	''' <summary>
	''' handle editvalue changed
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub OnlueMandant_EditValueChanged(sender As Object, e As System.EventArgs)
		Dim bSetToDefault As Boolean = False

		If String.IsNullOrWhiteSpace(Me.lueMandant.Properties.GetCheckedItems) Then Exit Sub

		If ClsDataDetail.MDData.MultiMD = 0 AndAlso Me.lueMandant.EditValue.ToString.Contains(",") Then
			m_UtilityUI.ShowInfoDialog("Es kann nur aus einer Mandant gesucht werden. Ich wähle den Hauptmandant.")

			bSetToDefault = True

		End If
		If Me.lueMandant.EditValue.ToString.Contains(",") Then bSetToDefault = True

		If Not bSetToDefault Then
			Dim SelectedData = lueMandant.Properties.GetItems.GetCheckedValues(0)

			If Not SelectedData Is Nothing Then
				ClsDataDetail.MDData = ClsDataDetail.SelectedMDData(Me.lueMandant.EditValue)
				ClsDataDetail.UserData = ClsDataDetail.LogededUSData(Me.lueMandant.EditValue, ClsDataDetail.UserData.UserLName, ClsDataDetail.UserData.UserFName)

				bSetToDefault = False

			Else
				bSetToDefault = True

			End If

		Else
			ClsDataDetail.MDData = ClsDataDetail.SelectedMDData(0)
			ClsDataDetail.UserData = ClsDataDetail.LogededUSData(0, ClsDataDetail.UserData.UserNr)
			ClsDataDetail.m_InitialData = New SP.Infrastructure.Initialization.InitializeClass(ClsDataDetail.TranslationData, ClsDataDetail.ProsonalizedData, ClsDataDetail.MDData, ClsDataDetail.UserData)

		End If
		m_InitializationData = ClsDataDetail.m_InitialData

		m_EmployeeDataAccess = New SP.DatabaseAccess.Employee.EmployeeDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_CommonDatabaseAccess = New SP.DatabaseAccess.Common.CommonDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_ListingDatabaseAccess = New ListingDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

		LoadDropDownData()

		Me.bbiSearch.Enabled = Not (ClsDataDetail.MDData Is Nothing)
		Me.bbiClear.Enabled = Not (ClsDataDetail.MDData Is Nothing)
		Me.bbiPrint.Enabled = Not (ClsDataDetail.MDData Is Nothing)
		Me.bbiExport.Enabled = Not (ClsDataDetail.MDData Is Nothing)

	End Sub

	Private Function LoadMainDataViaWebserviceCall() As Boolean
		Dim result As Boolean = True

		result = result AndAlso LoadMainDropDownDataOverWebService()


		Return result

	End Function

	Private Sub LoadDropDownData()
		Dim result As Boolean = True

		result = result AndAlso LoadEmploymentTypeDropDown()
		result = result AndAlso LoadOtherEmploymentTypeDropDown()
		result = result AndAlso LoadTypeOfStayDropDown()
		result = result AndAlso LoadForeignCategoryDropDown()
		result = result AndAlso LoadTaxCantonDropDown()
		result = result AndAlso LoadBirthPlaceDropDown()
		result = result AndAlso LoadActivatedOrNotActivatedDropDown()


		result = result AndAlso LoadCountryDropDown()
		result = result AndAlso LoadNationalityDropDown()
		result = result AndAlso LoadPermissionCodeDropDown()
		result = result AndAlso LoadTaxCodeDropDown()
		result = result AndAlso LoadCommunityData()
		result = result AndAlso LoadCivilstateCodeDropDown()
		result = result AndAlso LoadAddressSourceDropDown()


	End Sub

	Private Function LoadMainDropDownDataOverWebService() As Boolean
		Dim success As Boolean = True

		Try
			Dim language = m_InitializationData.UserData.UserLanguage
			m_EmploymentTypeData = m_BaseTableUtil.PerformEmploymentTypeDataOverWebService(language)
			m_OtherEmploymentTypeData = m_BaseTableUtil.PerformOtherEmploymentTypeDataOverWebService(language)
			m_TypeofStayData = m_BaseTableUtil.PerformTypeOfStayDataOverWebService(language)
			m_PermissionCategoryData = m_BaseTableUtil.PerformForeignCategoryDataOverWebService(String.Empty, language)
			m_CantonData = m_CommonDatabaseAccess.LoadCantonData()


			m_PermissionData = m_BaseTableUtil.PerformPermissionDataOverWebService(m_InitializationData.UserData.UserLanguage)
			m_BaseTableUtil.BaseTableName = "Country"
			m_CountryData = m_BaseTableUtil.PerformCVLBaseTablelistWebserviceCall()
			m_TaxData = m_BaseTableUtil.PerformTaxCodeDataOverWebService(m_InitializationData.UserData.UserLanguage)



		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

			success = False
		End Try


		Return success

	End Function


	Private Function LoadEmploymentTypeDropDown() As Boolean
		Dim success As Boolean = True

		Try
			Dim employmentData = m_ListingDatabaseAccess.LoadAllEmployeeEmploymentTypeData()
			If employmentData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten können nicht geladen werden."))

				Return False

			End If
			Dim result = New BindingList(Of EmploymentTypeData)

			For Each itm In employmentData
				Dim searchData = m_EmploymentTypeData.Where(Function(x) x.Rec_Value = itm.Rec_Value).FirstOrDefault
				If searchData Is Nothing Then Continue For

				result.Add(New EmploymentTypeData With {.ID = searchData.ID, .Rec_Value = itm.Rec_Value, .Translated_Value = searchData.Translated_Value})

			Next
			lueEmploymentType.Properties.DataSource = result

			If Not result Is Nothing Then lueEmploymentType.Properties.DropDownRows = result.Count + 1

			success = Not result Is Nothing


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

			success = False
		End Try

		Return success
	End Function

	Private Function LoadOtherEmploymentTypeDropDown() As Boolean
		Dim success As Boolean = True

		Try
			Dim employmentData = m_ListingDatabaseAccess.LoadAllEmployeeOtherEmploymentTypeData()
			If employmentData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten können nicht geladen werden."))

				Return False

			End If
			Dim result = New BindingList(Of EmploymentTypeData)

			For Each itm In employmentData
				Dim searchData = m_OtherEmploymentTypeData.Where(Function(x) x.Rec_Value = itm.Rec_Value).FirstOrDefault
				If searchData Is Nothing Then Continue For

				result.Add(New EmploymentTypeData With {.Rec_Value = itm.Rec_Value, .Translated_Value = searchData.Translated_Value})

			Next
			lueOtherEmploymentType.Properties.DataSource = result

			If Not result Is Nothing Then lueOtherEmploymentType.Properties.DropDownRows = result.Count + 1

			success = Not result Is Nothing


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

			success = False
		End Try

		Return success
	End Function

	Private Function LoadTypeOfStayDropDown() As Boolean
		Dim success As Boolean = True

		Try
			Dim employmentData = m_ListingDatabaseAccess.LoadAllEmployeeTypeOfStayData()
			If employmentData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten können nicht geladen werden."))

				Return False

			End If
			Dim result = New BindingList(Of TypeOfStayData)

			For Each itm In employmentData
				Dim searchData = m_TypeofStayData.Where(Function(x) x.Rec_Value = itm.Rec_Value).FirstOrDefault
				If searchData Is Nothing Then Continue For

				result.Add(New TypeOfStayData With {.Rec_Value = itm.Rec_Value, .Translated_Value = searchData.Translated_Value})

			Next
			lueTypeofStay.Properties.DataSource = result

			If Not result Is Nothing Then lueTypeofStay.Properties.DropDownRows = result.Count + 1

			success = Not result Is Nothing


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

			success = False
		End Try

		Return success
	End Function

	Private Function LoadForeignCategoryDropDown() As Boolean
		Dim success As Boolean = True

		Try
			Dim employmentData = m_ListingDatabaseAccess.LoadAllEmployeeForeignCategoryData()
			If employmentData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten können nicht geladen werden."))

				Return False

			End If
			Dim result = New BindingList(Of SP.Internal.Automations.PermissionData)

			For Each itm In employmentData
				Dim searchData = m_PermissionCategoryData.Where(Function(x) x.Rec_Value = itm.Rec_Value).FirstOrDefault
				If searchData Is Nothing Then Continue For

				result.Add(New SP.Internal.Automations.PermissionData With {.Rec_Value = itm.Rec_Value, .Translated_Value = searchData.Translated_Value})

			Next
			lueForeignCategory.Properties.DataSource = result

			If Not result Is Nothing Then lueForeignCategory.Properties.DropDownRows = result.Count + 1

			success = Not result Is Nothing


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

			success = False
		End Try

		Return success
	End Function

	Private Function LoadTaxCantonDropDown() As Boolean
		Dim success As Boolean = True

		Try
			Dim employmentData = m_ListingDatabaseAccess.LoadAllEmployeeTaxCantonData()
			If employmentData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten können nicht geladen werden."))

				Return False

			End If
			Dim result = New BindingList(Of SP.DatabaseAccess.Listing.DataObjects.CantonData)

			For Each itm In employmentData
				Dim searchData = m_CantonData.Where(Function(x) x.GetField = itm.Canton).FirstOrDefault
				If searchData Is Nothing Then Continue For

				result.Add(New SP.DatabaseAccess.Listing.DataObjects.CantonData With {.Canton = itm.Canton})
				Cbo_MASKanton.Properties.Items.Add(itm.Canton)

			Next


			If Not result Is Nothing Then Cbo_MASKanton.Properties.DropDownRows = result.Count + 1

			success = Not result Is Nothing


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

			success = False
		End Try

		Return success
	End Function

	Private Function LoadBirthPlaceDropDown() As Boolean
		Dim success As Boolean = True

		Try
			Dim employmentData = m_ListingDatabaseAccess.LoadAllEmployeeBirthPlaceData()
			If employmentData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten können nicht geladen werden."))

				Return False

			End If
			Dim result = New BindingList(Of SP.DatabaseAccess.Listing.DataObjects.AnyStringValueData)

			For Each itm In employmentData
				result.Add(itm)

			Next
			lueBirthPlace.Properties.DataSource = result

			If Not result Is Nothing Then lueBirthPlace.Properties.DropDownRows = Math.Min(20, result.Count + 1)

			success = Not result Is Nothing


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

			success = False
		End Try

		Return success
	End Function

	Private Function LoadActivatedOrNotActivatedDropDown() As Boolean
		Dim success As Boolean = True

		Try
			Dim result = New BindingList(Of SP.DatabaseAccess.Listing.DataObjects.TrueFalseValueData)

			Dim data As New SP.DatabaseAccess.Listing.DataObjects.TrueFalseValueData
			data.FieldValue = 0
			data.ValueLabel = m_Translate.GetSafeTranslationValue("nicht aktiviert")
			result.Add(data)

			data = New SP.DatabaseAccess.Listing.DataObjects.TrueFalseValueData
			data.FieldValue = 1
			data.ValueLabel = m_Translate.GetSafeTranslationValue("aktiviert")
			result.Add(data)

			lueCHPartner.Properties.DataSource = result
			lueNoSpecialTax.Properties.DataSource = result
			lueCertificateForResidenceReceived.Properties.DataSource = result


			If Not result Is Nothing Then
				lueCHPartner.Properties.DropDownRows = 2
				lueNoSpecialTax.Properties.DropDownRows = 2
				lueCertificateForResidenceReceived.Properties.DropDownRows = 2
			End If

			success = Not result Is Nothing


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

			success = False
		End Try

		Return success
	End Function

	Private Function LoadCountryDropDown() As Boolean
		Dim result As Boolean = True

		If lueMandant.EditValue Is Nothing Then Return True
		Dim data = m_ListingDatabaseAccess.LoadCountryForEmployeeSearchData(m_InitializationData.MDData.MDNr)
		If data Is Nothing Then
			Dim msg As String = "Länder Daten konnten nicht geladen werden."
			msg = m_Translate.GetSafeTranslationValue(msg)

			m_UtilityUI.ShowErrorDialog(msg)

			Return False
		End If
		Dim listData = New BindingList(Of CountryData)

		For Each itm In data
			Dim code = New CountryData


			If Not String.IsNullOrWhiteSpace(itm.Code) Then
				code.Code = itm.Code
				code.Name = itm.Name

				If Not m_CountryData Is Nothing AndAlso m_CountryData.Count > 0 Then
					Dim lndData = New SP.Internal.Automations.CVLBaseTableViewData
					lndData = m_CountryData.Where(Function(x) x.Code = itm.Code).FirstOrDefault()
					If Not lndData Is Nothing AndAlso Not String.IsNullOrWhiteSpace(lndData.Translated_Value) Then
						code.Code = itm.Code
						code.Name = lndData.Translated_Value
					End If
				End If

				listData.Add(code)
			End If

		Next
		lueCountry.Properties.DataSource = listData
		lueCountry.Properties.DropDownRows = Math.Min(20, listData.Count + 1)
		lueCountry.Enabled = listData.Count > 0


		Return Not listData Is Nothing

	End Function

	Private Function LoadNationalityDropDown() As Boolean
		Dim result As Boolean = True

		If lueMandant.EditValue Is Nothing Then Return True
		Dim data = m_ListingDatabaseAccess.LoadNationalityForEmployeeSearchData(m_InitializationData.MDData.MDNr)
		If data Is Nothing Then
			Dim msg As String = "Nationalität Daten konnten nicht geladen werden."
			msg = m_Translate.GetSafeTranslationValue(msg)

			m_UtilityUI.ShowErrorDialog(msg)

			Return False
		End If
		Dim listData = New BindingList(Of CountryData)

		For Each itm In data
			Dim code = New CountryData


			If Not String.IsNullOrWhiteSpace(itm.Code) Then
				code.Code = itm.Code
				code.Name = itm.Name

				If Not m_CountryData Is Nothing AndAlso m_CountryData.Count > 0 Then
					Dim lndData = New SP.Internal.Automations.CVLBaseTableViewData
					lndData = m_CountryData.Where(Function(x) x.Code = itm.Code).FirstOrDefault()
					If Not lndData Is Nothing AndAlso Not String.IsNullOrWhiteSpace(lndData.Translated_Value) Then
						code.Code = itm.Code
						code.Name = lndData.Translated_Value
					End If
				End If

				listData.Add(code)
			End If

		Next

		lueNationality.Properties.DataSource = listData
		lueNationality.Properties.DropDownRows = Math.Min(20, listData.Count + 1)
		lueNationality.Enabled = listData.Count > 0


		Return Not listData Is Nothing

	End Function

	Private Function LoadPermissionCodeDropDown() As Boolean
		Dim result As Boolean = True

		luePermissionCode.EditValue = Nothing
		If lueMandant.EditValue Is Nothing Then Return True
		Dim data = m_ListingDatabaseAccess.LoadPermissionForEmployeeSearchData(m_InitializationData.MDData.MDNr)
		If data Is Nothing Then
			Dim msg As String = "Bewilligung Daten konnten nicht geladen werden."
			msg = m_Translate.GetSafeTranslationValue(msg)

			m_UtilityUI.ShowErrorDialog(msg)

			Return False
		End If
		Dim listData = New BindingList(Of SP.DatabaseAccess.Common.DataObjects.PermissionData)

		For Each itm In data
			Dim code = New SP.DatabaseAccess.Common.DataObjects.PermissionData


			If Not String.IsNullOrWhiteSpace(itm.RecValue) Then
				code.RecValue = itm.RecValue
				code.TranslatedPermission = itm.TranslatedPermission

				If Not m_PermissionData Is Nothing AndAlso m_PermissionData.Count > 0 Then
					Dim bewdata = New SP.Internal.Automations.PermissionData
					bewdata = m_PermissionData.Where(Function(x) x.Rec_Value = itm.RecValue).FirstOrDefault()
					If Not bewdata Is Nothing AndAlso Not String.IsNullOrWhiteSpace(bewdata.Translated_Value) Then
						code.RecValue = itm.RecValue
						code.TranslatedPermission = bewdata.Translated_Value
					End If
				End If

				listData.Add(code)
			End If

		Next
		luePermissionCode.Properties.DataSource = listData
		luePermissionCode.Properties.DropDownRows = Math.Min(20, listData.Count + 1)
		luePermissionCode.Enabled = listData.Count > 0


		Return Not listData Is Nothing

	End Function

	Private Function LoadTaxCodeDropDown() As Boolean
		Dim result As Boolean = True

		If lueMandant.EditValue Is Nothing Then Return True
		Dim data = m_ListingDatabaseAccess.LoadTaxForEmployeeSearchData(m_InitializationData.MDData.MDNr)
		If data Is Nothing Then
			Dim msg As String = "Quellensteuer-Tarif Daten konnten nicht geladen werden."
			msg = m_Translate.GetSafeTranslationValue(msg)

			m_UtilityUI.ShowErrorDialog(msg)

			Return False
		End If
		Dim listData = New BindingList(Of SP.DatabaseAccess.Listing.DataObjects.QSTCodeData)

		For Each itm In data
			Dim code = New SP.DatabaseAccess.Listing.DataObjects.QSTCodeData


			If Not String.IsNullOrWhiteSpace(itm.QSTCode) Then
				code.QSTCode = itm.QSTCode
				code.QSTCodeLabel = itm.QSTCodeLabel

				If Not m_TaxData Is Nothing AndAlso m_TaxData.Count > 0 Then
					Dim lndData = New SP.Internal.Automations.TaxCodeData
					lndData = m_TaxData.Where(Function(x) x.Rec_Value = itm.QSTCode).FirstOrDefault()
					If Not lndData Is Nothing AndAlso Not String.IsNullOrWhiteSpace(lndData.Translated_Value) Then
						code.QSTCode = itm.QSTCode
						code.QSTCodeLabel = lndData.Translated_Value
					End If
				End If

				listData.Add(code)
			End If

		Next
		lueQSTCode.Properties.DataSource = listData
		lueQSTCode.Properties.DropDownRows = Math.Min(20, listData.Count + 1)
		lueQSTCode.Enabled = listData.Count > 0


		Return Not listData Is Nothing

	End Function

	Private Function LoadCommunityData() As Boolean
		Dim result As Boolean = True

		Dim data = m_ListingDatabaseAccess.LoadCommunityForEmployeeSearchData(0)
		If data Is Nothing Then
			Dim msg As String = "Gemeinde Daten konnten nicht geladen werden."
			msg = m_Translate.GetSafeTranslationValue(msg)

			m_UtilityUI.ShowErrorDialog(msg)

			Return False
		End If
		Dim communityData = m_BaseTableUtil.PerformCommunityDataOverWebService(String.Empty, m_InitializationData.UserData.UserLanguage)

		Dim listData As New BindingList(Of SP.DatabaseAccess.Listing.DataObjects.QSTCommunityData)
		For Each itm In data
			Dim viewData = New SP.DatabaseAccess.Listing.DataObjects.QSTCommunityData
			Dim communityCode As String = itm.CommunityCode
			Dim communityName As String = itm.CommunityName

			If Not String.IsNullOrWhiteSpace(Trim(Regex.Replace(communityCode, "\d", ""))) Then
				communityName = Trim(Regex.Replace(communityCode, "\d", ""))
				communityCode = String.Format("{0}", Val(communityCode))
			End If

			If communityCode = communityName OrElse String.IsNullOrWhiteSpace(communityName) Then
				Dim community = communityData.Where(Function(x) x.BFSNumber = Val(communityCode)).FirstOrDefault
				If Not community Is Nothing Then
					communityName = community.Translated_Value
				End If
			End If

			viewData.CommunityCode = itm.CommunityCode
			viewData.CommunityName = communityName

			Dim list = From commun In listData Where commun.CommunityCode = itm.CommunityCode
			If Not list.Any() Then
				listData.Add(viewData)
			End If

		Next
		Dim sortedListData = listData.OrderBy(Function(x) x.CommunityName)

		lueCommunity.Properties.DataSource = sortedListData
		lueCommunity.Properties.DropDownRows = Math.Min(20, listData.Count)

		lueCommunity.Enabled = listData.Count > 0


		Return Not data Is Nothing

	End Function

	Private Function LoadCivilstateCodeDropDown() As Boolean
		Dim result As Boolean = True

		If lueMandant.EditValue Is Nothing Then Return True
		Dim data = m_ListingDatabaseAccess.LoadCivilstateForEmployeeSearchData(m_InitializationData.MDData.MDNr)
		If data Is Nothing Then
			Dim msg As String = "Zivilstand Daten konnten nicht geladen werden."
			msg = m_Translate.GetSafeTranslationValue(msg)

			m_UtilityUI.ShowErrorDialog(msg)

			Return False
		End If

		lueCivilstate.Properties.DataSource = data
		lueCivilstate.Properties.DropDownRows = Math.Min(20, data.Count + 1)
		lueCivilstate.Enabled = data.Count > 0


		Return Not data Is Nothing

	End Function

	Private Function LoadAddressSourceDropDown() As Boolean
		Dim result As Boolean = True

		If lueMandant.EditValue Is Nothing Then Return True
		Dim data = New List(Of EmployeeAddressSourceData) From {New EmployeeAddressSourceData With {.AddressType = 0, .AddressTypeLabel = "Kandidatenstamm + Bewerberstamm"},
			New EmployeeAddressSourceData With {.AddressType = 1, .AddressTypeLabel = "Kandidatenstamm"},
			New EmployeeAddressSourceData With {.AddressType = 2, .AddressTypeLabel = "Bewerberstamm"}}

		lueAddressSource.Properties.DataSource = data
		lueAddressSource.Properties.DropDownRows = Math.Min(4, data.Count + 1)

		Dim bRight678 = (m_md.ModulLicenseKeys(m_InitializationData.MDData.MDNr, Now.Year, "").CVDropIN AndAlso IsUserActionAllowed(m_InitializationData.UserData.UserNr, 678))
		lueAddressSource.Enabled = bRight678 AndAlso data.Count > 0

		Return Not data Is Nothing

	End Function

#End Region


	Public ReadOnly Property ExcludeSelectedComboboxValue(ByVal sender As DevExpress.XtraEditors.ComboBoxEdit) As Boolean
		Get
			Dim cb As DevExpress.XtraEditors.ComboBoxEdit = CType(sender, DevExpress.XtraEditors.ComboBoxEdit)
			If cb.Properties.Buttons.Count > 1 Then
				Return CInt(cb.Properties.Buttons(1).Tag) = 1
			Else
				Return False
			End If

		End Get
	End Property

	Public ReadOnly Property ExcludeSelectedCheckedComboboxValue(ByVal sender As DevExpress.XtraEditors.CheckedComboBoxEdit) As Boolean
		Get
			Dim cb As DevExpress.XtraEditors.CheckedComboBoxEdit = CType(sender, DevExpress.XtraEditors.CheckedComboBoxEdit)
			If cb.Properties.Buttons.Count > 1 Then
				Return CInt(cb.Properties.Buttons(1).Tag) = 1
			Else
				Return False
			End If

		End Get
	End Property

	Public ReadOnly Property ExcludeSelectedLookupEditValue(ByVal sender As DevExpress.XtraEditors.LookUpEdit) As Boolean
		Get
			Dim cb As DevExpress.XtraEditors.LookUpEdit = CType(sender, DevExpress.XtraEditors.LookUpEdit)
			If cb.Properties.Buttons.Count > 1 Then
				Return CInt(cb.Properties.Buttons(3).Tag) = 1
			Else
				Return False
			End If

		End Get
	End Property

	Public ReadOnly Property ExcludeSelectedLSTValue(ByVal sender As DevExpress.XtraEditors.SimpleButton) As Boolean
		Get
			Dim btn As DevExpress.XtraEditors.SimpleButton = CType(sender, DevExpress.XtraEditors.SimpleButton)
			Return CInt(btn.Tag) = 1
		End Get
	End Property

	''' <summary>
	''' Handles drop down button clicks.
	''' </summary>
	Private Sub OnDropDown_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

		Const ID_OF_HIDESHOW_BUTTON As Int32 = 1

		' If hide/show button has been clicked reset the drop down.
		If e.Button.Index = ID_OF_HIDESHOW_BUTTON Then

			If e.Button.Tag = 0 Then
				e.Button.Image = My.Resources.hide_16x16
				e.Button.Tag = 1

			Else
				e.Button.Image = My.Resources.show_16x16
				e.Button.Tag = 0

			End If

		End If

	End Sub

	Private Sub OnLookupEditDropDown_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

		Const ID_OF_DELETE_BUTTON As Int32 = 1
		Const ID_OF_HIDESHOW_BUTTON As Int32 = 3

		' If delete button has been clicked reset the drop down.
		If e.Button.Index = ID_OF_DELETE_BUTTON Then

			If TypeOf sender Is LookUpEdit Then
				Dim lookupEdit As LookUpEdit = CType(sender, LookUpEdit)
				lookupEdit.EditValue = Nothing
			ElseIf TypeOf sender Is DevExpress.XtraEditors.GridLookUpEdit Then
				Dim lookupEdit As DevExpress.XtraEditors.GridLookUpEdit = CType(sender, DevExpress.XtraEditors.GridLookUpEdit)
				lookupEdit.EditValue = Nothing
			ElseIf TypeOf sender Is DevExpress.XtraEditors.DateEdit Then
				Dim dateEdit As DevExpress.XtraEditors.DateEdit = CType(sender, DevExpress.XtraEditors.DateEdit)
				dateEdit.EditValue = Nothing
			ElseIf TypeOf sender Is DevExpress.XtraEditors.CheckedComboBoxEdit Then
				Dim lookupEdit As DevExpress.XtraEditors.CheckedComboBoxEdit = CType(sender, DevExpress.XtraEditors.CheckedComboBoxEdit)
				For Each myItm In lookupEdit.Properties.Items
					myItm.CheckState = CheckState.Unchecked
				Next

			End If

		ElseIf e.Button.Index = ID_OF_HIDESHOW_BUTTON Then

			If e.Button.Tag = 0 Then
				e.Button.Image = My.Resources.hide_16x16
				e.Button.Tag = 1

			Else
				e.Button.Image = My.Resources.show_16x16
				e.Button.Tag = 0

			End If

		End If

	End Sub

	Private Sub OnHideShow_ButtonClick(sender As Object, e As EventArgs)

		' If hide/show button has been clicked reset the drop down.
		Dim btn As DevExpress.XtraEditors.SimpleButton = CType(sender, DevExpress.XtraEditors.SimpleButton)

		If btn.Tag = 0 Then
			btn.Image = My.Resources.hide_16x16
			btn.Tag = 1

		Else
			btn.Image = My.Resources.show_16x16
			btn.Tag = 0

		End If

	End Sub


#Region "btnAdd... clicks 1. Seite (Allgemeine)"

	''' <summary>
	''' Selektionsfenster für die Mitarbeiter-Berufe öffnen
	''' </summary>
	Private Sub OnbtnAddMABerufe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddMABerufe.Click
		Dim frmTest As New frmSearchRec("MABerufeCode")
		Dim i As Integer = 0

		Dim oldData = CType(Lst_MABerufe.DataSource, IEnumerable(Of EmployeeAssignedProfessionData))

		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		Dim m As String
		m = frmTest.iMAValue()
		Dim data = New List(Of EmployeeAssignedProfessionData)
		If Not oldData Is Nothing AndAlso oldData.Count > 0 Then
			For Each itm In oldData
				data.Add(itm)
			Next
		End If

		If m.ToString <> String.Empty Then
			Dim strEintrag As String() = Regex.Split(m.ToString, "#@")
			Dim separatorChars() As Char = {"|", "#"}

			For Each itm In strEintrag
				Dim tokens As String() = itm.Split(separatorChars)

				If tokens.Count Mod 2 = 0 Then

					For i = 0 To tokens.Count() - 1 Step 2

						Dim professionCodeStr As String = tokens(i)
						Dim professionDescription As String = tokens(i + 1)
						Dim professionCodeInt As Integer = 0

						If Integer.TryParse(professionCodeStr, professionCodeInt) Then
							If oldData Is Nothing OrElse Not oldData.Any(Function(mydata) mydata.ProfessionCode = professionCodeInt) Then
								Dim professionToInsert = New EmployeeAssignedProfessionData With {.ProfessionText = professionDescription, .ProfessionCode = professionCodeInt}
								data.Add(professionToInsert)
							End If
						End If

					Next

				End If

			Next
			Lst_MABerufe.DataSource = data

		End If

		frmTest.Dispose()
	End Sub


#End Region


#Region "Dropdown Funktionen 1. Seite (Allgemeine)"

	Private Sub CboSort_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CboSort.QueryPopUp
		If Me.CboSort.Properties.Items.Count = 0 Then ListSort(CboSort)
	End Sub

	Private Sub Cbo_MABewBisMonat_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_MABewBisMonat.QueryPopUp
		If Me.Cbo_MABewBisMonat.Properties.Items.Count = 0 Then ListBewBisMonat(Me.Cbo_MABewBisMonat)
	End Sub

	Private Sub Cbo_MABewBisJahr_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_MABewBisJahr.QueryPopUp
		If Me.Cbo_MABewBisJahr.Properties.Items.Count = 0 Then ListMABewBisJahr(Me.Cbo_MABewBisJahr)
	End Sub

	Private Sub Cbo_MAKanton_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_MAKanton.QueryPopUp
		If Me.Cbo_MAKanton.Properties.Items.Count = 0 Then ListMAKanton(Me.Cbo_MAKanton)
	End Sub

	Private Sub Cbo_MAGeschlecht_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_MAGeschlecht.QueryPopUp

		Cbo_MAGeschlecht.Properties.Items.Clear()
		Cbo_MAGeschlecht.Properties.Items.Add(New ComboBoxItem(m_Translate.GetSafeTranslationValue("männlich"), "M"))
		Cbo_MAGeschlecht.Properties.Items.Add(New ComboBoxItem(m_Translate.GetSafeTranslationValue("weiblich"), "W"))

	End Sub

	Private Sub Cbo_MAGeborenMonat_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MAGeborenMonat.QueryPopUp
		If Me.Cbo_MAGeborenMonat.Properties.Items.Count = 0 Then
			Me.Cbo_MAGeborenMonat.Properties.Items.Add("") ' Leeres Feld zur Abwahl
			For m As Integer = 1 To 12
				Me.Cbo_MAGeborenMonat.Properties.Items.Add(m)
			Next
		End If
	End Sub

#End Region


#Region "Lb Clicks 2. Seite (Betreuung)"

	''' <summary>
	''' Selektionsfenster für die Mitarbeiter-Beurteilung öffnen.
	''' </summary>
	Private Sub OnbtnAddMABeurteilung_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddMABeurteilung.Click
		Dim frmTest As New frmSearchRec("MABeurteilung")
		Dim i As Integer = 0

		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		Dim m As String
		m = frmTest.iMAValue()
		If m.ToString <> String.Empty Then

			Dim strEintrag As String() = Regex.Split(m.ToString, "#@")
			For i = 0 To strEintrag.Count - 1
				If Not Lst_MABeurteilung.Items.Contains(strEintrag(i)) Then
					Me.Lst_MABeurteilung.Items.Add(strEintrag(i))
				End If
			Next
		End If

		frmTest.Dispose()
	End Sub

	''' <summary>
	''' Selektionsfenster für die mündlichen Sprachen des Mitarbeiters
	''' </summary>
	Private Sub OnbtnAddMAMSprachen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddMAMSprachen.Click
		Dim frmTest As New frmSearchRec("MAMSprachen")
		Dim i As Integer = 0

		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		Dim m As String
		m = frmTest.iMAValue()
		If m.ToString <> String.Empty Then

			Dim strEintrag As String() = Regex.Split(m.ToString, "#@")
			For i = 0 To strEintrag.Count - 1
				If Not Lst_MAMSprachen.Items.Contains(strEintrag(i)) Then
					Me.Lst_MAMSprachen.Items.Add(strEintrag(i))
				End If
			Next
		End If

		frmTest.Dispose()
	End Sub

	Private Sub OnbtnAddMASSprachen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddMASSprachen.Click
		Dim frmTest As New frmSearchRec("MASSprachen")
		Dim i As Integer = 0

		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		Dim m As String
		m = frmTest.iMAValue()
		If m.ToString <> String.Empty Then

			Dim strEintrag As String() = Regex.Split(m.ToString, "#@")
			For i = 0 To strEintrag.Count - 1
				If Not Lst_MASSprachen.Items.Contains(strEintrag(i)) Then
					Me.Lst_MASSprachen.Items.Add(strEintrag(i))
				End If
			Next
		End If

		frmTest.Dispose()
	End Sub

#End Region


#Region "DropDown Funktionen 2. Seite (Betreuung)"

	Private Sub Cbo_MABetreuer_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MABetreuer.QueryPopUp
		ListMABetreuer(Cbo_MABetreuer)
	End Sub

	Private Sub Cbo_MAStatus1_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MAStatus1.QueryPopUp
		ListMAStatus1(Cbo_MAStatus1)
	End Sub

	Private Sub Cbo_MAStatus2_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MAStatus2.QueryPopUp
		ListMAStatus2(Cbo_MAStatus2)
	End Sub

	' Korrespondenzsprache
	Private Sub Cbo_MAKorrSprache_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MAKorrSprache.QueryPopUp
		ListMAKorrSprachen(Cbo_MAKorrSprache)
	End Sub

	' Geschäftsstellen
	Private Sub Cbo_MAGeschaeftsstellen_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MAGeschaeftsstellen.QueryPopUp
		ListMAGeschäftsstellen(Cbo_MAGeschaeftsstellen)
	End Sub

	' Kontakt
	Private Sub Cbo_MAKontakt_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MAKontakt.QueryPopUp
		ListMAKontakt(Cbo_MAKontakt)
	End Sub

	' Fahrzeuge
	Private Sub Cbo_MAFahrzeug_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MAFahrzeug.QueryPopUp
		ListMAFahrzeug(Cbo_MAFahrzeug)
	End Sub

	' Führerausweise
	Private Sub Cbo_MAFuehrerausweis_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MAFuehrerausweis.QueryPopUp
		ListMAFuehrerausweis(Cbo_MAFuehrerausweis)
	End Sub

	Private Sub Cbo_MASMS_NoMailing_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_MASMS_NoMailing.QueryPopUp
		ListForActivate(Me.Cbo_MASMS_NoMailing)
	End Sub

	Private Sub Cbo_MAMail_NoMailing_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_MAMail_NoMailing.QueryPopUp
		ListForActivate(Me.Cbo_MAMail_NoMailing)
	End Sub

#End Region


#Region "Lb clicks 3. Seite..."
	''' <summary>
	''' Selektionsfenster für die gewünschte Anstellungsart öffnen.
	''' </summary>
	Private Sub OnbtnAddMAAnstellung_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddMAAnstellung.Click
		Dim frmTest As New frmSearchRec("MAAnstellArt")
		Dim i As Integer = 0

		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		Dim m As String
		m = frmTest.iMAValue()
		If m.ToString <> String.Empty Then

			Dim strEintrag As String() = Regex.Split(m.ToString, "#@")
			For i = 0 To strEintrag.Count - 1
				If Not Lst_MAAnstellungsart.Items.Contains(strEintrag(i)) Then
					Me.Lst_MAAnstellungsart.Items.Add(strEintrag(i))
				End If
			Next
		End If

		frmTest.Dispose()
	End Sub

	''' <summary>
	''' Selektionsfenster für die Kommunikationsart öffnen
	''' </summary>
	Private Sub OnbtnAddMAKommunikationsart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddMAKommunikationsart.Click
		Dim frmTest As New frmSearchRec("MAKommArt")
		Dim i As Integer = 0

		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		Dim m As String
		m = frmTest.iMAValue()
		If m.ToString <> String.Empty Then

			Dim strEintrag As String() = Regex.Split(m.ToString, "#@")
			For i = 0 To strEintrag.Count - 1
				If Not Lst_MAKommunikationsart.Items.Contains(strEintrag(i)) Then
					Me.Lst_MAKommunikationsart.Items.Add(strEintrag(i))
				End If
			Next
		End If

		frmTest.Dispose()
	End Sub

	''' <summary>
	''' Selektionsfenster für sonstige Qualifikationen öffnen
	''' </summary>
	Private Sub OnbtnAddMASonstigeQualifikation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddMASonstigeQualifikation.Click
		Dim frmTest As New frmSearchRec("MASonstQual")
		Dim i As Integer = 0

		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		Dim m As String
		m = frmTest.iMAValue()
		If m.ToString <> String.Empty Then

			Dim strEintrag As String() = Regex.Split(m.ToString, "#@")
			For i = 0 To strEintrag.Count - 1
				If Not Lst_MASonstQualifikation.Items.Contains(strEintrag(i)) Then
					Me.Lst_MASonstQualifikation.Items.Add(strEintrag(i))
				End If
			Next
		End If

		frmTest.Dispose()
	End Sub

	''' <summary>
	''' Selektionsfenster für die Branchen öffnen
	''' </summary>
	Private Sub OnbtnAddMABranchen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddMABranchen.Click
		Dim frmTest As New frmSearchRec("MABranchen")
		Dim i As Integer = 0

		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		Dim m As String
		m = frmTest.iMAValue()
		If m.ToString <> String.Empty Then

			Dim strEintrag As String() = Regex.Split(m.ToString, "#@")
			For i = 0 To strEintrag.Count - 1
				If Not Lst_MABranchen.Items.Contains(strEintrag(i)) Then
					Me.Lst_MABranchen.Items.Add(strEintrag(i))
				End If
			Next
		End If

		frmTest.Dispose()
	End Sub

#End Region


#Region "DropDown Funktionen 4. Seite..."

	Private Sub OnMAALK_QueryPopUp(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboALKKasse.QueryPopUp

		ListEmployeeALKKasse(cboALKKasse)

	End Sub

	' 0 - Nicht aktiviert // 1 - Aktiviert
	Private Sub FillCboAktiviertNichtAktviert(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MAAHVabgegeben.QueryPopUp, Cbo_MAAHVretourniert.QueryPopUp, Cbo_MAZwischenverdienst.QueryPopUp, Cbo_MARahmenarbeitsvertrag.QueryPopUp, Cbo_MAVorschussSperren.QueryPopUp, Cbo_MARueckFerien.QueryPopUp, Cbo_MARueckFeiertag.QueryPopUp, Cbo_MARueck13Lohn.QueryPopUp, Cbo_MALohnSperren.QueryPopUp, Cbo_MAKTGpflicht.QueryPopUp, Cbo_MAQualNachweis.QueryPopUp, Cbo_MAImWeb.QueryPopUp, Cbo_MAGleitzeit.QueryPopUp, Cbo_MARapporteAusdrucken.QueryPopUp, Cbo_MAKinderVorhanden.QueryPopUp, Cbo_MAGesperrt.QueryPopUp, Cbo_MAVermittelt.QueryPopUp, Cbo_MAUnterlagenÜberWOS.QueryPopUp

		If TypeOf (sender) Is DevExpress.XtraEditors.ComboBoxEdit Then
			Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = DirectCast(sender, DevExpress.XtraEditors.ComboBoxEdit)
			If cbo.Properties.Items.Count = 0 Then ListCboAktiviertNichtAktiviert(cbo)
		End If

	End Sub

	' 0 - Nicht vollständig // 1 - Vollständig
	Private Sub FillCboVollstaendigNichtVoll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MAAHVvollstaendig.QueryPopUp, Cbo_MANewAHVvollstaendig.QueryPopUp

		If TypeOf (sender) Is DevExpress.XtraEditors.ComboBoxEdit Then
			Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = DirectCast(sender, DevExpress.XtraEditors.ComboBoxEdit)
			If cbo.Properties.Items.Count = 0 Then ListCboVollstaendigNichtVoll(cbo)
		End If

	End Sub

	' 1 - Ja // 0 - Nein
	Private Sub FillCboJaNein(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MAImEinsatz.QueryPopUp, Cbo_MAUnterschreitungslimiteVorhanden.QueryPopUp, Cbo_MAUnterschreitungslimiteUnter.QueryPopUp, cboTodayinES.QueryPopUp, cbo_AHVAnmeldung.QueryPopUp

		If TypeOf (sender) Is DevExpress.XtraEditors.ComboBoxEdit Then
			Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = DirectCast(sender, DevExpress.XtraEditors.ComboBoxEdit)
			If cbo.Properties.Items.Count = 0 Then ListCboJaNein(cbo)
		End If

	End Sub

	Private Sub FillCboVorhanden(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MAAdresseLohn.QueryPopUp, Cbo_MAAdresseRap.QueryPopUp, Cbo_MAAdresseES.QueryPopUp, Cbo_MAAngabenKiZu.QueryPopUp, Cbo_MAMonatlAngaben.QueryPopUp, Cbo_MALohnausweis.QueryPopUp

		If TypeOf (sender) Is DevExpress.XtraEditors.ComboBoxEdit Then
			Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = DirectCast(sender, DevExpress.XtraEditors.ComboBoxEdit)
			If cbo.Properties.Items.Count = 0 Then ListCboVorhanden(cbo)
		End If

	End Sub

	' 1 - Ja // 0 - Nein
	Private Sub FillCboJaNeinK(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MABankverbindung.QueryPopUp

		If TypeOf (sender) Is DevExpress.XtraEditors.ComboBoxEdit Then
			Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = DirectCast(sender, DevExpress.XtraEditors.ComboBoxEdit)
			If cbo.Properties.Items.Count = 0 Then ListCboJaNeinK(cbo)
		End If

	End Sub
	' 1 - Mit // 0 - Ohne
	Private Sub FillCboMitOhne(ByVal sender As System.Object, ByVal e As System.EventArgs)

		If TypeOf (sender) Is DevExpress.XtraEditors.ComboBoxEdit Then
			Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = DirectCast(sender, DevExpress.XtraEditors.ComboBoxEdit)
			If cbo.Properties.Items.Count = 0 Then ListCboMitOhne(cbo)
		End If

	End Sub
	' Reserve 1
	Private Sub Cbo_MAReserve1_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MAReserve1.QueryPopUp
		ListMAReserve(Cbo_MAReserve1, 1)
	End Sub

	' Reserve 2
	Private Sub Cbo_MAReserve2_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MAReserve2.QueryPopUp
		ListMAReserve(Cbo_MAReserve2, 2)
	End Sub

	' Reserve 3
	Private Sub Cbo_MAReserve3_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MAReserve3.QueryPopUp
		ListMAReserve(Cbo_MAReserve3, 3)
	End Sub

	' Reserve 4
	Private Sub Cbo_MAReserve4_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MAReserve4.QueryPopUp
		ListMAReserve(Cbo_MAReserve4, 4)
	End Sub

	' Reserve 5
	Private Sub Cbo_MAReserve5_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MAReserve5.QueryPopUp
		ListMAReserve(Cbo_MAReserve5, 5)
	End Sub

	' Arbeitspensum
	Private Sub Cbo_MAArbeitspensum_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MAArbeitspensum.QueryPopUp
		ListMAArbeitspensum(Cbo_MAArbeitspensum)
	End Sub

	' Kündigungsfristen
	Private Sub Cbo_MAKuendigungsfristen_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MAKuendigungsfristen.QueryPopUp
		ListMAKuendigungsfristen(Cbo_MAKuendigungsfristen)
	End Sub

	' AGB für WOS
	Private Sub Cbo_MAAGBfürWOS_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MAAGBfürWOS.QueryPopUp
		If Me.Cbo_MAAGBfürWOS.Properties.Items.Count = 0 Then ListMAAGBfürWOS(Cbo_MAAGBfürWOS)
	End Sub

	' Zahlungsarten
	Private Sub Cbo_MAZahlungsart_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MAZahlungsart.QueryPopUp
		If Me.Cbo_MAZahlungsart.Properties.Items.Count = 0 Then ListMAZahlungsarten(Cbo_MAZahlungsart)
	End Sub

	' Währung
	Private Sub Cbo_MAWaehrung_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MAWaehrung.QueryPopUp
		If Me.Cbo_MAWaehrung.Properties.Items.Count = 0 Then ListMAWaehrung(Me.Cbo_MAWaehrung)
	End Sub

	' AHV-Code
	Private Sub Cbo_MAAHVCode_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MAAHVCode.QueryPopUp
		If Me.Cbo_MAAHVCode.Properties.Items.Count = 0 Then ListMAAHVCode(Me.Cbo_MAAHVCode)
	End Sub

	' ALV-Code
	Private Sub Cbo_MAALVCode_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MAALVCode.QueryPopUp
		If Me.Cbo_MAALVCode.Properties.Items.Count = 0 Then ListMAALVCode(Me.Cbo_MAALVCode)
	End Sub

	' BVG-Code
	Private Sub Cbo_MABVGCode_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MABVGCode.QueryPopUp
		If Me.Cbo_MABVGCode.Properties.Items.Count = 0 Then ListMABVGCode(Me.Cbo_MABVGCode)
	End Sub

	'Suva Code 2
	Private Sub Cbo_MASuva2_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MASuva2.QueryPopUp
		If Me.Cbo_MASuva2.Properties.Items.Count = 0 Then ListMASUVACode2(Me.Cbo_MASuva2)
	End Sub

	Private Sub CmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
		Me.Dispose()
	End Sub

#End Region

	Private Sub frmMASearch_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

		FormIsLoaded("frmMASearch_LV", True)

		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.frm_Location = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.ifrmHeight = Me.Height
				My.Settings.ifrmWidth = Me.Width

				My.Settings.SortBez = Me.CboSort.Text

				My.Settings.Save()
			End If

			If Not PrintListingThread Is Nothing AndAlso PrintListingThread.IsAlive Then PrintListingThread.Abort()

		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub

	Private Sub frmMASearch_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Move

		If Me.Visible Then
			If FormIsLoaded("frmMASearch_LV", False) Then
				frmMyLV.Location = New Point(Me.Location.X - frmMyLV.Width - 5, Me.Location.Y)
				frmMyLV.TopMost = True
				frmMyLV.TopMost = False
			End If
		End If

	End Sub

	Sub StartTranslation()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim Time_1 As Double = System.Environment.TickCount
		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

		lblMA_Anstellungsarten.Text = m_Translate.GetSafeTranslationValue(lblMA_Anstellungsarten.Text, True)
		lblBeurteilung.Text = m_Translate.GetSafeTranslationValue(lblBeurteilung.Text, True)
		xtabAnsellung.Text = m_Translate.GetSafeTranslationValue(String.Format("{0} und {1}", m_Translate.GetSafeTranslationValue("MA_Anstellungsarten", True), m_Translate.GetSafeTranslationValue("Beurteilung", True)))

		lblBranchen.Text = m_Translate.GetSafeTranslationValue(lblBranchen.Text, True)
		lblKommunikationsart.Text = m_Translate.GetSafeTranslationValue(lblKommunikationsart.Text, True)
		xtabBranchen.Text = m_Translate.GetSafeTranslationValue(String.Format("Branchen und {0}", m_Translate.GetSafeTranslationValue("Kommunikationsart", True)))

		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)
		Me.bbiSearch.Caption = m_Translate.GetSafeTranslationValue(Me.bbiSearch.Caption)
		Me.bbiClear.Caption = m_Translate.GetSafeTranslationValue(Me.bbiClear.Caption)
		Me.bbiPrint.Caption = m_Translate.GetSafeTranslationValue(Me.bbiPrint.Caption)
		Me.bbiExport.Caption = m_Translate.GetSafeTranslationValue(Me.bbiExport.Caption)

		For i As Integer = 0 To Me.xtabMASearch.TabPages.Count - 1
			Me.TranslatedPage.Add(False)
		Next

		m_xml.GetChildChildBez(Me.xtabAllgemein)
		m_xml.GetChildChildBez(Me.GroupBox1)

		For Each tbp As DevExpress.XtraTab.XtraTabPage In Me.xtabMASearch.TabPages
			tbp.Text = m_Translate.GetSafeTranslationValue(tbp.Text)
		Next

		TranslatedPage(0) = True

	End Sub

	''' <summary>
	''' Starten von Anwendung.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub frmMASearch_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		SetDefaultSortValues()

		Dim Time_1 As Double = System.Environment.TickCount
		Me.KeyPreview = True
		Dim strStyleName As String = m_md.GetSelectedUILayoutName(ClsDataDetail.ProgSettingData.SelectedMDNr, 0, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If


		Dim UpdateDelegate As New MethodInvoker(AddressOf StartTranslation)
		Me.Invoke(UpdateDelegate)

		Try
			Me.Width = Math.Max(My.Settings.ifrmWidth, Me.Width)
			Me.Height = Math.Max(My.Settings.ifrmHeight, Me.Height)
			If My.Settings.frm_Location <> String.Empty Then
				Dim aLoc As String() = My.Settings.frm_Location.Split(CChar(";"))

				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
			End If

		Catch ex As Exception

		End Try

		' Berechtigung Fililale/Kostenstelle wählen
		If Not IsUserActionAllowed(0, 672) Then
			Me.Cbo_MAGeschaeftsstellen.Enabled = False
			Me.Cbo_MABetreuer.Enabled = False
			Dim strUSTitle As String = GetUSTitel(ClsDataDetail.UserData.UserNr)

			Me.Cbo_MABetreuer.Enabled = strUSTitle.ToLower.Contains("leiter") Or strUSTitle.ToLower.Contains("führer")
			Me.Cbo_MAGeschaeftsstellen.Text = ClsDataDetail.UserData.UserFiliale
			ListMABetreuer(Me.Cbo_MABetreuer)

			For i As Integer = 0 To Me.Cbo_MABetreuer.Properties.Items.Count - 1
				Dim item As ComboBoxItem = DirectCast(Me.Cbo_MABetreuer.Properties.Items(i), ComboBoxItem)
				Trace.WriteLine(item.Text)
				If item.Text.StartsWith(String.Format("{0}", ClsDataDetail.UserData.UserFullName)) Then
					Me.Cbo_MABetreuer.SelectedIndex = i
					Exit For
				End If
			Next
		End If

		If Not m_md.AllowedExportEmployee2WOS(ClsDataDetail.MDData.MDNr, Now.Year) Then
			Me.lblMAUnterlagenÜberWOS.Visible = False
			Me.Cbo_MAUnterlagenÜberWOS.Visible = False
			Me.lblMAAGBfürWOS.Visible = False
			Me.Cbo_MAAGBfürWOS.Visible = False
		End If

		' Berechtigung Mitarbeiterliste exportieren
		Dim _ClsDb As New ClsDbFunc
		_ClsDb.GetJobNr4Print(0)
		If Not IsUserAllowed4DocExport(ClsDataDetail.GetModulToPrint()) Then
			Me.bbiExport.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
		End If
		Me.bbiExport.Enabled = False
		Me.bbiPrint.Enabled = False

		Me.Cbo_MAGeborenMonat.Properties.Items.Clear()
		ClsDataDetail.IsFirstTapiCall = True

		Me.xtabMASearch.SelectedTabPage = Me.xtabAllgemein

	End Sub

	Sub SetDefaultSortValues()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Try
				Dim strSort As String = My.Settings.SortBez
				If strSort = String.Empty Then strSort = String.Format("1 - {0}", m_Translate.GetSafeTranslationValue("Kandidatennname"))
				ListSort(Me.CboSort)
				Dim aSortlist As String() = strSort.Split(CChar(","))

				For Each bez As String In aSortlist
					CboSort.Properties.Items(CInt(Val(bez))).CheckState = CheckState.Checked
				Next

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Fill Sortinglist: {1}", strMethodeName, ex.Message))

			End Try

			Me.xtabCtl_3.SelectedTabPage = xtabBranchen
			Me.xtabCtl_4.SelectedTabPage = xtabAuszahlung

			If ClsDataDetail.UserData.UserNr <> 1 Then
				Me.xtabMASearch.TabPages.Remove(Me.xtabSQLAbfrage)
				Me.xtabMASearch.TabPages.Remove(Me.xtabErweitert)
			End If

			Try
				Me.lueMandant.SetEditValue(ClsDataDetail.ProgSettingData.SelectedMDNr)
				Dim showMDSelection As Boolean = IsUserActionAllowed(ClsDataDetail.ProgSettingData.LogedUSNr, 642, ClsDataDetail.ProgSettingData.SelectedMDNr)
				Me.lueMandant.Visible = showMDSelection
				Me.lblMDName.Visible = showMDSelection

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Mandantenauswahl anzeigen: {1}", strMethodeName, ex.Message))
				Me.lueMandant.Visible = False
				Me.lblMDName.Visible = False
			End Try

			lueAddressSource.EditValue = 0

			AddHandler Me.lueMandant.EditValueChanged, AddressOf OnlueMandant_EditValueChanged


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Definition der Sortierreihenfolge. {1}", strMethodeName, ex.Message))

		End Try

	End Sub


	Private Sub frm_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

		If e.KeyCode = Keys.F12 And ClsDataDetail.ProgSettingData.LogedUSNr = 1 Then

			Try
				Dim frm As New frmLibraryInfo
				frm.LoadAssemblyData()

				frm.Show()
				frm.BringToFront()

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			End Try

		End If

	End Sub

	Private Sub xtabMASearch_SelectedPageChanged(sender As Object, e As DevExpress.XtraTab.TabPageChangedEventArgs) Handles xtabMASearch.SelectedPageChanged

		Select Case xtabMASearch.SelectedTabPageIndex
			Case 0
				If Not TranslatedPage(xtabMASearch.SelectedTabPageIndex) Then
					m_xml.GetChildChildBez(Me.xtabAllgemein)
					TranslatedPage(xtabMASearch.SelectedTabPageIndex) = True
				End If

			Case 1
				If Not TranslatedPage(xtabMASearch.SelectedTabPageIndex) Then
					m_xml.GetChildChildBez(Me.xtabBetreuung)
					TranslatedPage(xtabMASearch.SelectedTabPageIndex) = True
				End If

			Case 2
				If Not TranslatedPage(xtabMASearch.SelectedTabPageIndex) Then
					m_xml.GetChildChildBez(Me.xtabZusatz)
					TranslatedPage(xtabMASearch.SelectedTabPageIndex) = True
				End If

			Case 3
				If Not TranslatedPage(xtabMASearch.SelectedTabPageIndex) Then
					m_xml.GetChildChildBez(Me.xtabKontakt)
					TranslatedPage(xtabMASearch.SelectedTabPageIndex) = True
				End If

			Case 4
				If Not TranslatedPage(xtabMASearch.SelectedTabPageIndex) Then
					m_xml.GetChildChildBez(Me.xtabErweitert)
					TranslatedPage(xtabMASearch.SelectedTabPageIndex) = True
				End If

			Case 5
				If Not TranslatedPage(xtabMASearch.SelectedTabPageIndex) Then
					m_xml.GetChildChildBez(Me.xtabSQLAbfrage)
					TranslatedPage(xtabMASearch.SelectedTabPageIndex) = True
				End If

		End Select
	End Sub

	''' <summary>
	''' Daten fürs Drucken bereit stellen.
	''' </summary>
	Sub GetMAData4Print(ByVal bForExport As Boolean, ByVal strJobInfo As String)
		Dim iMANr As Integer = 0
		Dim bResult As Boolean = True
		Dim msg As String = String.Empty


		Dim sSql As String = Me.txt_SQLQuery.Text
		sSql = ShortSQLQuery

		If sSql = String.Empty Then
			msg = "Keine Suche wurde gestartet!"
			m_UtilityUI.ShowErrorDialog(msg)

			Return
		End If

		' Wenn die Kontaktliste ausgedruckt werden soll, so Tabelle und Felder hinzufügen
		If ClsDataDetail.GetModulToPrint = "1.3.4" Then
			Dim strFelder As String = ""

			'strFelder = ", Convert(nvarchar(10), Convert(DateTime, MA_Kontakte.KontaktDate),104) As MAKKontaktDateString, "
			strFelder = "Select MA.*, Convert(nvarchar(10), Convert(DateTime, MAK.KontaktDate),104) As MAKKontaktDateString, "
			strFelder += "Convert(nvarchar(5),Convert(DateTime, MAK.KontaktDate,104),108) as MAKKontaktTimeString, "
			strFelder += "MAK.KontaktType1 As MAKKontaktType1, MAK.KontaktDauer As MAKKontaktDauer, "
			strFelder += "MAK.Kontakte As MAKKontakte, MAK.CreatedFrom As MAKCreatedFrom "

			strFelder += "Into #MAKKTemplate From _MATemplate_{0} MA "
			strFelder += "Left Join MA_Kontakte MAK On MA.MANr = MAK.MANr "
			strFelder &= "Select * From #MAKKTemplate "
			strFelder &= "Order By convert(datetime, MAKKontaktDateString) DESC, convert(datetime, MAKKontaktTimeString) DESC"
			strFelder = String.Format(strFelder, ClsDataDetail.UserData.UserNr)
			sSql = strFelder
			'Dim strTabelle As String = "Left Join MA_Kontakte On MA.MANr = MA_Kontakte.MANr "
			'sSql = Replace(UCase(sSql), "FROM MITARBEITER MA ", String.Format("{0} FROM MITARBEITER MA {1} ", strFelder, strTabelle))
			'sSql = Replace(UCase(sSql), _
			'               String.Format("FROM _MATemplate_{0} MA ", ClsDataDetail.UserData.UserNr), _
			'               String.Format("{0} FROM _MATemplate_{1} MA {2} ", strFelder, ClsDataDetail.UserData.UserNr, strTabelle))

			' Sortierung hinzufügen
			'sSql = Replace(UCase(sSql), UCase(_ClsDb.GetSortString(Me.CboSort.Text.Trim)) _
			'               , String.Format("{0}, MA_Kontakte.KontaktDate DESC", UCase(_ClsDb.GetSortString(Me.CboSort.Text.Trim))))

		End If

		Try

			Me.SQL4Print = sSql
			Me.PrintJobNr = strJobInfo

			StartPrinting()

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

	End Sub

	Sub StartPrinting()
		Dim bShowDesign As Boolean = IsUserActionAllowed(0, 611)
		Dim ShowDesign As Boolean = (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown) AndAlso bShowDesign

		Dim _Setting As New SPS.Listing.Print.Utility.ClsLLMASearchPrintSetting With {.DbConnString2Open = ClsDataDetail.GetSelectedMDConnstring,
																																									 .SQL2Open = Me.SQL4Print,
																																									 .JobNr2Print = Me.PrintJobNr,
																																									.SelectedMDNr = ClsDataDetail.MDData.MDNr,
																																									.LogedUSNr = ClsDataDetail.UserData.UserNr}
		Dim obj As New SPS.Listing.Print.Utility.MASearchListing.ClsPrintMASearchList(_Setting)
		obj.PrintMASearchList_1(ShowDesign, ClsDataDetail.GetSortBez,
														New List(Of String)(New String() {ClsDataDetail.GetFilterBez,
																															ClsDataDetail.GetFilterBez2,
																															ClsDataDetail.GetFilterBez3,
																															ClsDataDetail.GetFilterBez4}))

	End Sub

#Region "Funktionen zur Menüaufbau..."



	Private Sub bbiSearch_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSearch.ItemClick
		Dim Stopwatch As Stopwatch = New Stopwatch()
		Dim sSql1Query As String = String.Empty
		Dim sSql2Query As String = String.Empty
		Dim strSort As String = String.Empty
		Dim bBuildAsVirtual As Boolean = CBool(Me.txt_SQLQuery.Text.Trim = String.Empty)

		Try
			Me.txt_SQLQuery.Text = String.Empty
			Me.ShortSQLQuery = String.Empty
			If Not Me.txtMANr_2.Visible Then Me.txtMANr_2.Text = Me.txtMANr_1.Text
			If Not Me.txtMANachname_2.Visible Then Me.txtMANachname_2.Text = Me.txtMANachname_1.Text
			If Not Me.txtMAVorname_2.Visible Then Me.txtMAVorname_2.Text = Me.txtMAVorname_1.Text

			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Nach Daten wird gesucht...")

			FormIsLoaded("frmMASearch_LV", True)
			If ClsDataDetail.GetLVSortBez <> String.Empty Then Me.CboSort.Text = ClsDataDetail.GetLVSortBez

			' Die Query-String aufbauen...
			GetMyQueryString()

			' Daten auflisten...
			If Not FormIsLoaded("frmMASearch_LV", True) Then
				frmMyLV = New frmMASearch_LV(Me.txt_SQLQuery.Text, True)

				frmMyLV.Show()
				Me.Select()
			End If

			Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet") & "...",
																				 frmMyLV.RecCount)
			frmMyLV.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet") & "...",
																															frmMyLV.RecCount)

			' Die Buttons Drucken und Export aktivieren
			If frmMyLV.RecCount > 0 Then
				CreatePrintPopupMenu()
				CreateExportPopupMenu()

				Me.bbiPrint.Enabled = True
				Me.bbiExport.Enabled = True

			Else
				Me.bbiPrint.Enabled = False
				Me.bbiExport.Enabled = False

			End If

		Catch ex As Exception
			MessageBox.Show(ex.Message, "btnSearch_Click")

		Finally

		End Try

	End Sub

	'Private Sub SaveFormControlValues()
	'  Dim filename As String = "C:\Path\test.xml"
	'  Dim employeeNumbers As String = txtMANr_1.EditValue

	'  Dim xml = GenericSerializer.Serialize(Of String)(employeeNumbers)
	'  Dim test = GenericSerializer.Deserialize(Of String)(xml)

	'End Sub

	Private Sub LoadFormControlValues()

		'    Dim test = GenericSerializer.Deserialize(Of String)(Xml)

	End Sub

	Function GetMyQueryString() As Boolean
		Dim sSql1Query As String = String.Empty
		Dim sSql2Query As String = String.Empty
		Dim strSort As String = String.Empty
		Dim strArtQuery As String = String.Empty

		Dim _ClsDb As New ClsDbFunc

		Dim result = lueMandant.Properties.GetItems.GetCheckedValues()
		For Each itm In result
			If CInt(Val(itm.ToString)) > 0 Then _ClsDb.mandantNumber.Add(CInt(itm.ToString))
		Next

		If Me.txt_IndSQLQuery.Text = String.Empty Then

			sSql1Query = _ClsDb.GetStartSQLString(Me)        ' 1. String
			sSql2Query = _ClsDb.GetQuerySQLString(sSql1Query, Me)       ' Where Klausel

			If Trim(sSql2Query) <> String.Empty Then
				sSql1Query += " Where "
			End If
			strSort = _ClsDb.GetSortString(Me.CboSort.Text.Trim)          ' Sort Klausel

			Me.txt_SQLQuery.Text = sSql1Query + sSql2Query ' + strSort
			If strLastSortBez = String.Empty Then strLastSortBez = strSort

			Me.ShortSQLQuery = String.Format("Select * From _MATemplate_{0} MA {1}",
												   ClsDataDetail.UserData.UserNr, strSort)
			Me.txt_SQLQuery.Text = String.Format(Me.txt_SQLQuery.Text & " " & ShortSQLQuery,
												   ClsDataDetail.UserData.UserNr, strSort)

		Else

			Me.txt_SQLQuery.Text = Me.txt_IndSQLQuery.Text
			Me.ShortSQLQuery = Me.txt_SQLQuery.Text

		End If

		Return True
	End Function


	Private Sub bbiPrint_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick
		Dim popupMenu As DevExpress.XtraBars.PopupMenu = Me.bbiPrint.DropDownControl

		If Not (popupMenu Is Nothing) Then popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

	End Sub

	Private Sub CreatePrintPopupMenu()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim bshowMnu As Boolean = True
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim liMnu As New List(Of String) From {"Kandidatenliste#mnumalistedrucken",
																					 "-Liste der Kontakte#mnumalistekontaktliste"}
		', _
		'																			 If(bShowDesign, "Entwurfanssicht Kandidatenliste#mnudesignmaliste", ""),
		'																			 If(bShowDesign, "Entwurfansicht Kontaktliste#mnudesignkontaktliste", "")}
		If Not IsUserActionAllowed(0, 601) Then Exit Sub
		Try
			bbiPrint.Manager = Me.BarManager1
			BarManager1.ForceInitialize()

			Me.bbiPrint.ActAsDropDown = False
			Me.bbiPrint.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
			Me.bbiPrint.DropDownEnabled = True
			Me.bbiPrint.DropDownControl = popupMenu
			Me.bbiPrint.Enabled = True

			For i As Integer = 0 To liMnu.Count - 1
				Dim myValue As String() = liMnu(i).Split(CChar("#"))
				Dim captionLbl = myValue(0).ToString
				bshowMnu = Not String.IsNullOrWhiteSpace(captionLbl)

				If bshowMnu Then
					popupMenu.Manager = BarManager1

					Dim itm As New DevExpress.XtraBars.BarButtonItem
					If captionLbl.StartsWith("-") Then captionLbl = captionLbl.Substring(1, captionLbl.Length - 1)
					captionLbl = m_Translate.GetSafeTranslationValue(captionLbl)
					itm.Caption = captionLbl

					itm.Name = m_Translate.GetSafeTranslationValue(myValue(1).ToString)
					If myValue(0).ToString.ToLower.StartsWith("-".ToLower) Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)

					AddHandler itm.ItemClick, AddressOf GetPrintMenuItem
				End If

			Next

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try
	End Sub

	Sub GetPrintMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
		Dim _clsdb As New ClsDbFunc

		Me.SQL4Print = String.Empty
		Me.bPrintAsDesign = False

		Dim strModultoPrint As String = ClsDataDetail.GetModulToPrint
		Select Case e.Item.Name.ToUpper
			Case "mnumalistedrucken".ToUpper
				_clsdb.GetJobNr4Print(0)
				GetMAData4Print(False, ClsDataDetail.GetModulToPrint())

			Case "mnumalistekontaktliste".ToUpper
				_clsdb.GetJobNr4Print(1)
				GetMAData4Print(False, ClsDataDetail.GetModulToPrint())


				'Case "mnudesignmaliste".ToUpper
				'	_clsdb.GetJobNr4Print(0)
				'	GetMAData4Print(, ClsDataDetail.GetModulToPrint())

				'Case "mnudesignkontaktliste".ToUpper
				'	_clsdb.GetJobNr4Print(1)
				'	GetMAData4Print(, ClsDataDetail.GetModulToPrint())



			Case Else
				Exit Sub

		End Select

	End Sub

	Private Sub bbiExport_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiExport.ItemClick
		Dim popupMenu As DevExpress.XtraBars.PopupMenu = Me.bbiExport.DropDownControl

		If Not (popupMenu Is Nothing) Then popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

	End Sub

	Private Sub CreateExportPopupMenu()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim bshowMnu As Boolean = True
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim liMnu As List(Of String) = GetMenuItems4Export()

		Try
			bbiPrint.Manager = Me.BarManager1
			BarManager1.ForceInitialize()
			Me.bbiExport.ActAsDropDown = False
			Me.bbiExport.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
			Me.bbiExport.DropDownEnabled = True
			Me.bbiExport.DropDownControl = popupMenu
			'Me.bbiExport.Visibility = BarItemVisibility.Always
			Me.bbiExport.Enabled = True

			For i As Integer = 0 To liMnu.Count - 1
				Dim myValue As String() = liMnu(i).Split(CChar("#"))
				If bshowMnu Then
					popupMenu.Manager = BarManager1

					Dim itm As New DevExpress.XtraBars.BarButtonItem
					itm.Caption = m_Translate.GetSafeTranslationValue(myValue(0).ToString).Replace("-", "")
					itm.Name = m_Translate.GetSafeTranslationValue(myValue(1).ToString)
					itm.AccessibleName = myValue(2).ToString
					If myValue(0).ToString.ToLower.Contains("-".ToLower) Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)
					AddHandler itm.ItemClick, AddressOf GetExportMenuItem
				End If
			Next

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try
	End Sub

	Sub GetExportMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
		Dim strSQL As String = Me.ShortSQLQuery

		Select Case UCase(e.Item.Name.ToUpper)
			Case UCase("TXT"), UCase("CSV")
				'Dim ExportThread As New Thread(AddressOf StartExportModul)
				'ExportThread.Name = "ExportMAListing"
				'ExportThread.Start()
				StartExportModul()


			Case UCase("Contact")
				Call ExportDataToOutlook(Me.ShortSQLQuery) ' Me.txt_SQLQuery.Text)

			Case UCase("MAIL")
				Call RunMailModul(Me.ShortSQLQuery) ' Me.txt_SQLQuery.Text)

			Case UCase("Multi-Db")
				Call RunBewModul(Me.ShortSQLQuery) ' Me.txt_SQLQuery.Text)

			Case UCase("FAX")
				Call RunTobitFaxModul(Me.ShortSQLQuery) ' Me.txt_SQLQuery.Text)

	'		Case UCase("SMS")
	'			Dim sSql As String = "Select DISTINCTMA.MANr), MA.MANachname As Nachname, MA.MAVorname As Vorname, ( " +
	'		"Case MA.MA_SMS_Mailing " +
	'			"When 0 Then MA.MANatel Else '' End) As Natel, " +
	'			"ma.mageschlecht AS Geschlecht, " +
	'			"mak.briefanrede AS Anredeform, " +
	'			"(ma.mastrasse + ', ' + maland + '-' + ma.maplz + ' ' + maort) AS Adresse " +
	'			"From _MATemplate_{0} MA " +
	'"LEFT JOIN dbo.MAKontakt_Komm mak ON mak.manr = ma.manr " +
	'"Where (MA.MANatel <> '' And MA.MANatel Is Not Null ) And MA.MA_SMS_Mailing <> 1 " +
	'			"Order by MA.MANachname, MA.MAVorname"
	'			sSql = String.Format(sSql, ClsDataDetail.UserData.UserNr)

	'			Call RunSMSProg(sSql)	' Me.txt_SQLQuery.Text)

			Case UCase("eCall-SMS")
				Dim sql As String
				sql = "Select DISTINCT(MA.MANr), MA.MANachname As Nachname, MA.MAVorname As Vorname, ( "
				sql &= "Case MA.MA_SMS_Mailing "
				sql &= "When 0 Then MA.MANatel Else '' End) As Natel, "
				sql &= "ma.mageschlecht AS Geschlecht, "
				sql &= "mak.briefanrede AS Anredeform, "
				sql &= "ma.mastrasse Strasse, maland Land, ma.maplz PLZ, MA.maort Ort "
				sql &= "From _MATemplate_{0} MA "
				sql &= "LEFT JOIN dbo.MAKontakt_Komm mak ON mak.manr = ma.manr "
				sql &= "Where (MA.MANatel <> '' And MA.MANatel Is Not Null ) And MA.MA_SMS_Mailing <> 1 "
				sql &= "Order by MA.MANachname, MA.MAVorname"

				sql = String.Format(sql, ClsDataDetail.UserData.UserNr)

				Call RuneCallSMSModul(sql)


		End Select

	End Sub

	Sub StartExportModul()
		Dim _Setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = ClsDataDetail.GetSelectedMDConnstring,
																																			 .SQL2Open = Me.ShortSQLQuery,
																																			 .ModulName = "MASearch"}
		Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_Setting)
		'Dim _clsExport As New SPS.Export.Listing.Utility.ClsExportStart(Me.ShortSQLQuery, "MASearch")
		obj.ExportCSVFromMASearchListing(Me.ShortSQLQuery)
	End Sub



	''' <summary>
	''' Funktion für das Leeren der Felder...
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub bbiClear_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiClear.ItemClick

		Dim strText As String = Me.CboSort.Text

		FormIsLoaded("frmMASearch_LV", True)
		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Bereit")

		ResetAllTabEntries()

		lueAddressSource.EditValue = 0
		Me.CboSort.Text = strText

		Me.txt_SQLQuery.Text = m_Translate.GetSafeTranslationValue("Wurde geleert...")
		LoadDropDownData()

		' Die Buttons Drucken und Export deaktivieren
		Me.bbiPrint.Enabled = False
		Me.bbiExport.Enabled = False

	End Sub

	Private Sub ResetAllTabEntries()

		For Each tabPg As DevExpress.XtraTab.XtraTabPage In Me.Controls.Item("xtabMASearch").Controls
			For Each ctrls In tabPg.Controls
				ResetControl(ctrls)
			Next
		Next

	End Sub

	''' <summary>
	''' Jedes Control wird anhand des Typs zurückgesetzt.
	''' </summary>
	''' <param name="con"></param>
	''' <remarks>Funktion mit rekursivem Aufruf.</remarks>
	Private Sub ResetControl(ByVal con As Control)

		If Not con.Enabled OrElse con.Name.ToLower.Contains("cbosort") Or con.Name.ToLower.Contains("luemandant") Then Exit Sub
		If TypeOf (con) Is TextBox Then
			Dim tb As TextBox = con
			tb.Text = String.Empty


		ElseIf TypeOf (con) Is DevExpress.XtraEditors.ComboBoxEdit Then
			Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = con
			cbo.Properties.Items.Clear()
			cbo.EditValue = Nothing

		ElseIf TypeOf (con) Is DevExpress.XtraEditors.LookUpEdit Then
			Dim cbo As DevExpress.XtraEditors.LookUpEdit = con
			cbo.EditValue = Nothing

		ElseIf TypeOf (con) Is DevExpress.XtraEditors.CheckedComboBoxEdit Then
			Dim cbo As DevExpress.XtraEditors.CheckedComboBoxEdit = con
			cbo.Properties.Items.Clear()
			cbo.EditValue = Nothing

		ElseIf TypeOf (con) Is DevExpress.XtraEditors.DateEdit Then
			Dim de As DevExpress.XtraEditors.DateEdit = con
			de.EditValue = Nothing

		ElseIf TypeOf (con) Is DevExpress.XtraEditors.CheckEdit Then
			Dim de As DevExpress.XtraEditors.CheckEdit = con
			de.CheckState = CheckState.Unchecked

		ElseIf TypeOf (con) Is CheckBox Then
			Dim cbo As CheckBox = con
			cbo.CheckState = CheckState.Unchecked

		ElseIf TypeOf (con) Is DevExpress.XtraEditors.SpinEdit Then
			Dim cbo As DevExpress.XtraEditors.SpinEdit = con
			cbo.Value = 0

		ElseIf TypeOf (con) Is DevExpress.XtraEditors.TextEdit Then
			Dim tb As DevExpress.XtraEditors.TextEdit = con
			tb.Text = String.Empty
		ElseIf TypeOf (con) Is TextBox Then
			Dim tb As DevExpress.XtraEditors.TextEdit = con
			tb.Text = String.Empty

		ElseIf TypeOf (con) Is DevExpress.XtraEditors.ListBoxControl Then
			Dim lst As DevExpress.XtraEditors.ListBoxControl = con
			lst.Items.Clear()

		ElseIf con.HasChildren Then
			For Each conChild As Control In con.Controls
				ResetControl(conChild)
			Next
		End If

	End Sub

#End Region

#Region "Sonstige Funktionen..."

	Private Function LV_GetItemIndex(ByRef lv As ListView) As Integer

		Try
			If lv.Items.Count > 0 Then
				Dim lvi As ListViewItem = lv.SelectedItems(0)    '.Item(0)
				If lvi.Selected Then
					Return lvi.Index
				Else
					Return -1
				End If
			End If

		Catch ex As Exception

		End Try

	End Function

#End Region


#Region "KeyDown für Lst und Textfelder..."


#End Region


	''' <summary>
	''' Übergebene Controls mit dem Klick-Event verbinden
	''' </summary>
	''' <param name="Ctrls"></param>
	''' <remarks></remarks>
	Private Sub InitClickHandler(ByVal ParamArray Ctrls() As Control)

		For Each Ctrl As Control In Ctrls
			AddHandler Ctrl.KeyPress, AddressOf KeyPressEvent
			'      AddHandler Ctrl.Click, AddressOf ClickEvents
		Next

	End Sub

	Private Sub txtMANr_1_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtMANr_1.ButtonClick
		Dim frmTest As New frmSearchRec("MANR")
		Dim strModulName As String = String.Empty

		Dim m As String = String.Empty
		Dim result = frmTest.ShowDialog()

		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iMAValue()
		m = CStr(m.ToString.Replace("#@", ","))

		frmTest.Dispose()
		Me.txtMANr_1.Text = m

	End Sub

	Private Sub txtMANr_2_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtMANr_2.ButtonClick
		Dim frmTest As New frmSearchRec("MANR")
		Dim strModulName As String = String.Empty

		Dim m As String = String.Empty
		Dim result = frmTest.ShowDialog()

		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iMAValue()
		m = CStr(m.ToString.Replace("#@", ","))

		frmTest.Dispose()

		Me.txtMANr_2.Text = m

	End Sub


	Private Sub txtMANachname_1_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtMANachname_1.ButtonClick
		Dim frmTest As New frmSearchRec("Nachname")
		Dim strModulName As String = String.Empty

		Dim m As String = String.Empty
		Dim result = frmTest.ShowDialog()

		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iMAValue()
		m = CStr(m.ToString.Replace("#@", ","))

		frmTest.Dispose()
		Me.txtMANachname_1.Text = m

	End Sub

	Private Sub txtMANachname_2_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtMANachname_2.ButtonClick
		Dim frmTest As New frmSearchRec("Nachname")
		Dim strModulName As String = String.Empty

		Dim m As String = String.Empty
		Dim result = frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iMAValue()
		m = CStr(m.ToString.Replace("#@", ","))

		frmTest.Dispose()
		Me.txtMANachname_2.Text = m

	End Sub

	Private Sub txtMAVorname_1_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtMAVorname_1.ButtonClick
		Dim frmTest As New frmSearchRec("Vorname")

		Dim m As String = String.Empty
		Dim result = frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iMAValue()
		m = CStr(m.ToString.Replace("#@", ","))

		frmTest.Dispose()
		Me.txtMAVorname_1.Text = m

	End Sub

	Private Sub txtMAVorname_2_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtMAVorname_2.ButtonClick
		Dim frmTest As New frmSearchRec("Vorname")

		Dim m As String = String.Empty
		Dim result = frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iMAValue()
		m = CStr(m.ToString.Replace("#@", ","))

		frmTest.Dispose()
		Me.txtMAVorname_2.Text = m

	End Sub




	''' <summary>
	''' Klick-Event der Controls auffangen und verarbeiten
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub KeyPressEvent(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles txtMANr_2.KeyPress, txtMANr_1.KeyPress, txtMANachname_2.KeyPress, txtMANachname_1.KeyPress, txtMAVorname_2.KeyPress, txtMAVorname_1.KeyPress, txtMAPLZ_1.KeyPress, txtMAOrt_1.KeyPress, Cbo_MAGeschlecht.KeyPress, Lst_MABerufe.KeyPress, Cbo_MAKanton.KeyPress, Cbo_MAKorrSprache.KeyPress, Cbo_MABewBisMonat.KeyPress, Cbo_MABewBisJahr.KeyPress, Cbo_MAStatus2.KeyPress, Cbo_MAStatus1.KeyPress, Cbo_MABetreuer.KeyPress, Cbo_MAGeborenMonat.KeyPress, txtMAGebKW_2.KeyPress, txtMAGebKW_1.KeyPress, txtMAAlter_2.KeyPress, txtMAAlter_1.KeyPress, Lst_MASSprachen.KeyPress, Lst_MASonstQualifikation.KeyPress, Lst_MAMSprachen.KeyPress, Lst_MAKommunikationsart.KeyPress, Lst_MABranchen.KeyPress, Lst_MABeurteilung.KeyPress, Lst_MAAnstellungsart.KeyPress, Cbo_MAZwischenverdienst.KeyPress, Cbo_MAZahlungsart.KeyPress, Cbo_MAWaehrung.KeyPress, Cbo_MAVorschussSperren.KeyPress, Cbo_MASuva2.KeyPress, Cbo_MARueckFerien.KeyPress, Cbo_MARueckFeiertag.KeyPress, Cbo_MARueck13Lohn.KeyPress, Cbo_MAReserve4.KeyPress, Cbo_MAReserve3.KeyPress, Cbo_MAReserve2.KeyPress, Cbo_MAReserve1.KeyPress, Cbo_MARahmenarbeitsvertrag.KeyPress, Cbo_MAQualNachweis.KeyPress, Cbo_MANewAHVvollstaendig.KeyPress, Cbo_MALohnSperren.KeyPress, Cbo_MAKuendigungsfristen.KeyPress, Cbo_MAKTGpflicht.KeyPress, Cbo_MAKontakt.KeyPress, Cbo_MAImWeb.KeyPress, Cbo_MAImEinsatz.KeyPress, Cbo_MAGleitzeit.KeyPress, Cbo_MAGeschaeftsstellen.KeyPress, Cbo_MAFuehrerausweis.KeyPress, Cbo_MAFahrzeug.KeyPress, Cbo_MABVGCode.KeyPress, Cbo_MAArbeitspensum.KeyPress, Cbo_MAALVCode.KeyPress, Cbo_MAAHVvollstaendig.KeyPress, Cbo_MAAHVretourniert.KeyPress, Cbo_MAAHVCode.KeyPress, Cbo_MAAHVabgegeben.KeyPress, Cbo_MAGeburtstag.KeyPress, Cbo_MARapporteAusdrucken.KeyPress, Cbo_MAKinderVorhanden.KeyPress, Cbo_MAUnterschreitungslimiteVorhanden.KeyPress, Cbo_MAUnterschreitungslimiteUnter.KeyPress, lst_BerufGruppe.KeyPress, lst_Fachbereich.KeyPress

		Try
			If e.KeyChar = Chr(13) Then ' Enter
				SendKeys.Send("{tab}")
				e.Handled = True
			End If

		Catch ex As Exception
			MessageBox.Show(ex.Message, "KeyPress", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try

	End Sub

	Private Sub CboSort_DropDownClosed(ByVal sender As Object, ByVal e As System.EventArgs) Handles CboSort.QueryCloseUp, CboSort.QueryCloseUp
		ClsDataDetail.GetLVSortBez = String.Empty
	End Sub

	Private Sub CboSort_Layout(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LayoutEventArgs) Handles CboSort.Layout
		'Inhalt aus XML-Datei holen
	End Sub


	''' <summary>
	''' Allgemeiner Delete-Event für die Listbox
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub KeyUpEvent(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Lst_MABerufe.KeyUp, Lst_MABeurteilung.KeyUp, Lst_MASonstQualifikation.KeyUp, Lst_MAKommunikationsart.KeyUp, Lst_MABranchen.KeyUp, Lst_MAAnstellungsart.KeyUp, txtMANr_1.KeyUp, txtMAVorname_2.KeyUp, txtMAVorname_1.KeyUp, txtMANr_2.KeyUp, txtMANachname_2.KeyUp, txtMANachname_1.KeyUp, Lst_MASSprachen.KeyUp, Lst_MAMSprachen.KeyUp, lst_BerufGruppe.KeyUp, lst_Fachbereich.KeyUp

		If e.KeyCode = Keys.Delete AndAlso TypeOf (sender) Is DevExpress.XtraEditors.ListBoxControl Then
			Dim _lst As DevExpress.XtraEditors.ListBoxControl = DirectCast(sender, DevExpress.XtraEditors.ListBoxControl)

			If _lst.Name.Contains("Lst_MABerufe") Then
				Dim data = TryCast(_lst.DataSource, List(Of EmployeeAssignedProfessionData))
				Dim selectedBusinessBranchData = TryCast(_lst.SelectedItem, EmployeeAssignedProfessionData)

				If (Not selectedBusinessBranchData Is Nothing) Then

					data.Remove(selectedBusinessBranchData)

					_lst.DataSource = data
				End If
				_lst.Refresh()

			Else
				If _lst.SelectedIndex > -1 Then
					For i As Integer = 0 To _lst.SelectedIndices.Count - 1
						' Wenn der erste selektierte Inidices gelöscht wird,
						' rückt der nächste automatisch nach bis keine mehr
						' vorhanden sind. Darum 0. 19.08.2009 A.Ragusa
						'_lst.Items.RemoveAt(_lst.SelectedIndices(0))
						_lst.Items.Remove(_lst.SelectedItem)
					Next

				End If

			End If

		End If

	End Sub

	Private Sub Link_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles LibVorname_1.PreviewKeyDown, LibNr_1.PreviewKeyDown, LibNachname_1.PreviewKeyDown

		If e.KeyCode = Keys.Space Or e.KeyCode = Keys.F4 Then
			Dim link As LinkLabel = DirectCast(sender, LinkLabel)

			Select Case link.Tag
				Case txtMANr_1.Name
					txtMANr_1_ButtonClick(sender, New System.EventArgs)

				Case txtMANr_2.Name
					txtMANr_2_ButtonClick(sender, New System.EventArgs)

				Case txtMANachname_1.Name
					txtMANachname_1_ButtonClick(sender, New System.EventArgs)

				Case txtMANachname_2.Name
					txtMANachname_2_ButtonClick(sender, New System.EventArgs)

				Case txtMAVorname_1.Name
					txtMAVorname_1_ButtonClick(sender, New System.EventArgs)

				Case txtMAVorname_2.Name
					txtMAVorname_2_ButtonClick(sender, New System.EventArgs)

			End Select

		ElseIf e.KeyCode = Keys.Enter Then
			KeyPressEvent(sender, New System.Windows.Forms.KeyPressEventArgs(keyChar:=ChrW(13)))

		End If

	End Sub

	Private Sub frmMASearch_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
		Me.txtMANr_1.Focus()
	End Sub

	Private Sub Cbo_MAGeburtstag_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MAGeburtstag.QueryPopUp
		If Me.Cbo_MAGeburtstag.Properties.Items.Count = 0 Then
			' Berechnung der Kalenderwoche (Achtung: In der letzte Woche des Jahres wird die Zahl nicht immer korrekt zurückgegeben.)
			Dim kw As Integer = DatePart(DateInterval.WeekOfYear, Date.Now, FirstDayOfWeek.Monday, FirstWeekOfYear.FirstFourDays)
			' Berechnung des Monats
			Dim mon As Integer = DatePart(DateInterval.Month, Date.Now)

			Me.Cbo_MAGeburtstag.Properties.Items.Add(New ComboBoxItem("", ""))
			Me.Cbo_MAGeburtstag.Properties.Items.Add(New ComboBoxItem(String.Format(m_Translate.GetSafeTranslationValue("Heute: {0}"),
																																	 Date.Now.ToString("dd.MM.yyyy")), "HE"))
			Me.Cbo_MAGeburtstag.Properties.Items.Add(New ComboBoxItem(String.Format(m_Translate.GetSafeTranslationValue("Letzte Woche / KW {0}"),
																																	 kw - 1), "LW"))
			Me.Cbo_MAGeburtstag.Properties.Items.Add(New ComboBoxItem(String.Format(m_Translate.GetSafeTranslationValue("Diese Woche / KW {0}"),
																																	 kw), "DW"))
			Me.Cbo_MAGeburtstag.Properties.Items.Add(New ComboBoxItem(String.Format(m_Translate.GetSafeTranslationValue("Nächste Woche / KW {0}"),
																																	 kw + 1), "NW"))
			Me.Cbo_MAGeburtstag.Properties.Items.Add(New ComboBoxItem(String.Format(m_Translate.GetSafeTranslationValue("Letzten Monat ({0})"),
																																	 mon - 1), "LM"))
			Me.Cbo_MAGeburtstag.Properties.Items.Add(New ComboBoxItem(String.Format(m_Translate.GetSafeTranslationValue("Diesen Monat ({0})"),
																																	 mon), "DM"))
			Me.Cbo_MAGeburtstag.Properties.Items.Add(New ComboBoxItem(String.Format(m_Translate.GetSafeTranslationValue("Nächsten Monat ({0})"),
																																	 mon + 1), "NM"))
		End If
	End Sub

	Private Sub btnPrint_DropDownOpened(sender As Object, e As System.EventArgs)
		Dim ts As ToolStripDropDownButton = CType(sender, ToolStripDropDownButton)
		For Each itm As ToolStripItem In ts.DropDownItems
			itm.Text = m_Translate.GetSafeTranslationValue(itm.Text)
			Trace.WriteLine(String.Format("ChildControlName: {0} Text: ", itm.Text))
		Next
	End Sub

	Private Sub btnExport_DropDownOpening(sender As Object, e As System.EventArgs)
		Dim ts As ToolStripDropDownButton = CType(sender, ToolStripDropDownButton)
		For Each itm As ToolStripItem In ts.DropDownItems
			itm.Text = m_Translate.GetSafeTranslationValue(itm.Text)
			Trace.WriteLine(String.Format("ChildControlName: {0} Text: ", itm.Text))
		Next
	End Sub



#Region "Funktionen für Suche nach Zusatztabellen..."

	Sub OpenGrd4Berufgruppe(sender As System.Object)
		Dim grdGrid As New DevExpress.XtraGrid.GridControl
		Dim grdView As New DevExpress.XtraGrid.Views.Grid.GridView
		Dim dt As New DataTable

		pcc = New DevExpress.XtraBars.PopupControlContainer
		pcc.Name = "pcc_BerufGruppeTemp"

		pcc.SuspendLayout()
		pcc.Manager = New DevExpress.XtraBars.BarManager
		pcc.Left = sender.Location.X
		pcc.Top = sender.Location.Y
		pcc.ShowCloseButton = True
		pcc.ShowSizeGrip = True
		grdGrid.Dock = DockStyle.Fill

		dt = GetDbData4BerufGruppe()
		grdGrid.DataSource = dt
		grdGrid.MainView = grdGrid.CreateView("view_BerufGruppetemp")
		grdGrid.Name = "grd_BerufGruppeTemp"

		grdGrid.ForceInitialize()
		grdGrid.Visible = False
		Me.Controls.AddRange(New Control() {pcc})
		pcc.Controls.AddRange(New Control() {grdGrid})
		If My.Settings.pcc_BerufgruppenSize <> "" Then
			Dim aSize As String() = My.Settings.pcc_BerufgruppenSize.Split(CChar(";"))
			pcc.Size = New Size(CInt(aSize(0)), CInt(aSize(1)))
		Else
			pcc.Size = New Size(200, 400)
		End If

		AddHandler pcc.SizeChanged, AddressOf pcc_BerufGruppeSizeChanged
		AddHandler grdGrid.DoubleClick, AddressOf ViewMABerufgruppe_RowClick

		pcc.ShowPopup(Cursor.Position)
		grdView = TryCast(grdGrid.MainView, DevExpress.XtraGrid.Views.Grid.GridView)
		'If My.Settings.bgrdView_EnterpriseShowGroup Then _
		'grdView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.Hidden ' DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways
		If grdView.RowCount > 20 Then grdView.ShowFindPanel()
		grdView.OptionsView.ShowGroupPanel = False
		grdView.OptionsBehavior.Editable = False
		grdView.OptionsSelection.EnableAppearanceFocusedCell = False
		grdView.OptionsSelection.InvertSelection = False
		grdView.OptionsSelection.EnableAppearanceFocusedRow = True
		grdView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus

		Dim i As Integer = 0
		For Each col As GridColumn In grdView.Columns
			Trace.WriteLine(String.Format("{0}", col.FieldName))
			col.MinWidth = 0
			Try
				col.Visible = col.FieldName.ToLower.Contains("BerufBez_DE".ToLower)
				col.Caption = m_Translate.GetSafeTranslationValue(col.GetCaption)

			Catch ex As Exception
				col.Visible = False

			End Try
			i += 1
		Next col
		grdGrid.Visible = True
		pcc.ResumeLayout()

	End Sub

	Private Sub OnbtnAddBerufGruppe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddBerufGruppe.Click
		OpenGrd4Berufgruppe(sender)
	End Sub

	Private Sub pcc_BerufGruppeSizeChanged(sender As Object,
																				e As System.EventArgs)

		My.Settings.pcc_BerufgruppenSize = String.Format("{0};{1}", sender.Size.Width, sender.Size.Height)
		My.Settings.Save()
	End Sub

	Sub ViewMABerufgruppe_RowClick(sender As Object, e As System.EventArgs)
		Dim strValue As String = String.Empty
		Dim strTbleName As String = String.Empty
		Dim grdView As New DevExpress.XtraGrid.Views.Grid.GridView

		grdView = TryCast(sender.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

		Try
			For i As Integer = 0 To grdView.SelectedRowsCount - 1
				Dim row As Integer = (grdView.GetSelectedRows()(i))
				If (grdView.GetSelectedRows()(i) >= 0) Then
					Dim dtr As DataRow
					dtr = grdView.GetDataRow(grdView.GetSelectedRows()(i))
					strValue = dtr.Item("BerufBez_DE").ToString
					If IsAllowedInsert2Lst(lst_BerufGruppe, strValue) Then
						Me.lst_BerufGruppe.Items.Add(strValue)
					End If
				End If
			Next i
			pcc.HidePopup()

		Catch ex As Exception
			pcc.HidePopup()

		End Try

	End Sub

	Sub OpenGrd4Fachbereiche(sender As System.Object)
		Dim grdGrid As New DevExpress.XtraGrid.GridControl
		Dim grdView As New DevExpress.XtraGrid.Views.Grid.GridView
		Dim dt As New DataTable

		pcc = New DevExpress.XtraBars.PopupControlContainer
		pcc.Name = "pcc_FachbereichTemp"

		pcc.SuspendLayout()
		pcc.Manager = New DevExpress.XtraBars.BarManager
		pcc.Left = sender.Location.X
		pcc.Top = sender.Location.Y
		pcc.ShowCloseButton = True
		pcc.ShowSizeGrip = True
		grdGrid.Dock = DockStyle.Fill

		dt = GetDbData4Fachbereich()
		grdGrid.DataSource = dt
		grdGrid.MainView = grdGrid.CreateView("view_Fachbereichtemp")
		grdGrid.Name = "grd_FachbereichTemp"

		grdGrid.ForceInitialize()
		grdGrid.Visible = False
		Me.Controls.AddRange(New Control() {pcc})
		pcc.Controls.AddRange(New Control() {grdGrid})
		If My.Settings.pcc_BerufgruppenSize <> "" Then
			Dim aSize As String() = My.Settings.pcc_BerufgruppenSize.Split(CChar(";"))
			pcc.Size = New Size(CInt(aSize(0)), CInt(aSize(1)))
		Else
			pcc.Size = New Size(200, 400)
		End If

		AddHandler pcc.SizeChanged, AddressOf pcc_FachbereichSizeChanged
		AddHandler grdGrid.DoubleClick, AddressOf ViewMAFachbereich_RowClick

		pcc.ShowPopup(Cursor.Position)
		grdView = TryCast(grdGrid.MainView, DevExpress.XtraGrid.Views.Grid.GridView)
		'If My.Settings.bgrdView_EnterpriseShowGroup Then _
		'grdView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.Hidden ' DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways

		If grdView.RowCount > 20 Then grdView.ShowFindPanel()
		grdView.OptionsView.ShowGroupPanel = False

		grdView.OptionsBehavior.Editable = False
		grdView.OptionsSelection.EnableAppearanceFocusedCell = False
		grdView.OptionsSelection.InvertSelection = False
		grdView.OptionsSelection.EnableAppearanceFocusedRow = True
		grdView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus

		Dim i As Integer = 0
		For Each col As GridColumn In grdView.Columns
			Trace.WriteLine(String.Format("{0}", col.FieldName))
			col.MinWidth = 0
			Try
				col.Visible = col.FieldName.ToLower.Contains("FachBez_DE".ToLower)
				col.Caption = m_Translate.GetSafeTranslationValue(col.GetCaption)

			Catch ex As Exception
				col.Visible = False

			End Try
			i += 1
		Next col
		grdGrid.Visible = True
		pcc.ResumeLayout()

	End Sub

	Private Sub OnbtnAddFachbereich_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddFachbereich.Click
		OpenGrd4Fachbereiche(sender)
	End Sub

	Private Sub pcc_FachbereichSizeChanged(sender As Object,
																				e As System.EventArgs)

		My.Settings.pcc_FachbereichSize = String.Format("{0};{1}", sender.Size.Width, sender.Size.Height)
		My.Settings.Save()
	End Sub

	Sub ViewMAFachbereich_RowClick(sender As Object, e As System.EventArgs)
		Dim strValue As String = String.Empty
		Dim strTbleName As String = String.Empty
		Dim grdView As New DevExpress.XtraGrid.Views.Grid.GridView

		grdView = TryCast(sender.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

		Try
			For i As Integer = 0 To grdView.SelectedRowsCount - 1
				Dim row As Integer = (grdView.GetSelectedRows()(i))
				If (grdView.GetSelectedRows()(i) >= 0) Then
					Dim dtr As DataRow
					dtr = grdView.GetDataRow(grdView.GetSelectedRows()(i))
					strValue = dtr.Item("FachBez_DE").ToString
					If IsAllowedInsert2Lst(lst_Fachbereich, strValue) Then
						Me.lst_Fachbereich.Items.Add(strValue)
					End If

				End If
			Next i
			pcc.HidePopup()

		Catch ex As Exception
			pcc.HidePopup()

		End Try

	End Sub

#End Region


	Private Sub SwitchButton1_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton1.ValueChanged
		Me.txtMANr_2.Visible = Me.SwitchButton1.Value
		Me.txtMANr_2.Text = String.Empty
	End Sub

	Private Sub SwitchButton2_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton2.ValueChanged
		Me.txtMANachname_2.Visible = Me.SwitchButton2.Value
		Me.txtMANachname_2.Text = String.Empty
	End Sub

	Private Sub SwitchButton3_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton3.ValueChanged
		Me.txtMAVorname_2.Visible = Me.SwitchButton3.Value
		Me.txtMAVorname_2.Text = String.Empty
	End Sub

	Private Sub checkedComboBoxEdit1_QueryPopUp(ByVal sender As Object, ByVal e As CancelEventArgs)
		Dim max As Integer = 0
		Dim g = sender.CreateGraphics()
		For i As Integer = 0 To sender.Properties.Items.Count - 1
			Dim w As SizeF = g.MeasureString(sender.Properties.Items(i).ToString(), Font)
			If CInt(w.Width) + 40 > max Then
				max = CInt(w.Width) + 40
			End If
		Next i

		sender.Properties.PopupFormSize = New Size(max, sender.Properties.PopupFormSize.Height)
	End Sub


End Class


''' <summary>
''' Klasse für die ComboBox, um Text und Wert zu haben.
''' Das Item wird mit den Parameter Text für die Anzeige und
''' Value für den Wert zur ComboBox hinzugefügt.
''' </summary>
''' <remarks></remarks>
Class ComboBoxItem
	Public Text As String
	Public Value As String
	Public Sub New(ByVal text As String, ByVal val As String)
		Me.Text = text
		Me.Value = val
	End Sub
	Public Overrides Function ToString() As String
		Return Text
	End Function
End Class


Module StringExtensions

	<Extension()>
	Public Sub KommasEntfernen(ByRef text As String)
		Do While (text.Contains(",,"))
			text = text.Replace(",,", ",")
		Loop
		If text.StartsWith(",") Then
			text = text.Remove(0, 1)
		End If
		If text.EndsWith(",") Then
			text = text.Remove(text.Length - 1, 1)
		End If
	End Sub

End Module