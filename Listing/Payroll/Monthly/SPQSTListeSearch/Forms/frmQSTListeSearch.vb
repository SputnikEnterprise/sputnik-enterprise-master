
Option Strict Off

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Utility

Imports System.Reflection.Assembly
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPS.Listing.Print.Utility

Imports DevExpress.LookAndFeel
Imports System.Threading
Imports DevExpress.XtraEditors.Controls
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports SP.Infrastructure.Logging
Imports System.ComponentModel
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Listing.DataObjects
Imports DevExpress.XtraEditors.DXErrorProvider
Imports SP.Internal.Automations
Imports System.Text.RegularExpressions
Imports SP.DatabaseAccess.Common.DataObjects
Imports DevExpress.XtraSplashScreen
Imports SP.Internal.Automations.BaseTable

Public Class frmQSTListeSearch
	Inherits DevExpress.XtraEditors.XtraForm


#Region "private fields"

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper


	Private m_ConnectionString As String

	''' <summary>
	''' The data access object.
	''' </summary>
	Private m_CommonDatabaseAccess As ICommonDatabaseAccess
	Private m_ListingDatabaseAccess As IListingDatabaseAccess

	Private m_md As Mandant
	Private m_Utility As SPProgUtility.MainUtilities.Utilities
	Private m_UtilityUi As SP.Infrastructure.UI.UtilityUI

	''' <summary>
	''' Boolean flag indicating if form is initializing.
	''' </summary>
	Protected m_SuppressUIEvents As Boolean = False

	Private m_SearchCriteriums As QSTListingSearchData

	'Private _ClsFunc As New ClsDivFunc
	'Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	Private strValueSeprator As String = "#@"
	Private m_LoadedQSTData As IEnumerable(Of SearchRestulOfTaxData)

	Private strLastSortBez As String
	Private m_BaseTableData As BaseTable.SPSBaseTables
	Private m_PermissionData As BindingList(Of SP.Internal.Automations.PermissionData)
	Private m_CountryData As BindingList(Of SP.Internal.Automations.CVLBaseTableViewData)

#End Region


	Public Shared frmMyLV As frmQSTListeSearch_LV



#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_md = New Mandant
		m_Utility = New Utilities
		m_UtilityUi = New UtilityUI

		m_InitializationData = _setting
		ClsDataDetail.m_InitialData = m_InitializationData
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
		m_BaseTableData = New BaseTable.SPSBaseTables(m_InitializationData)

		m_ConnectionString = m_InitializationData.MDData.MDDbConn
		m_CommonDatabaseAccess = New CommonDatabaseAccess(m_ConnectionString, m_InitializationData.UserData.UserLanguage)
		m_ListingDatabaseAccess = New ListingDatabaseAccess(m_ConnectionString, m_InitializationData.UserData.UserLanguage)

		m_SuppressUIEvents = True

		InitializeComponent()
		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

		Me.KeyPreview = True
		Dim strStyleName As String = m_md.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		Reset()


		Try
			Me.chk_QSTFranz.Checked = CBool(My.Settings.checkedQSTFranz)
			Me.chk_QSTJustFranz.Checked = CBool(My.Settings.checkedQSTjustFranz)
			Me.Chk_QSTListeNullBetrag.Checked = CBool(My.Settings.CHECKZEROAMOUNT)
			Me.Chk_QSTListeNurErstenES.Checked = CBool(My.Settings.CHECKFIRSTES)

			chkHideBruttolohn.Checked = CBool(My.Settings.chkHideBruttolohn)
			chkHideQSTBasis.Checked = CBool(My.Settings.chkHideQSTBasis)
			chkHideQSTBasis2.Checked = CBool(My.Settings.chkHideQSTBasis2)

		Catch ex As Exception
			Me.chk_QSTFranz.Checked = True
			Me.chk_QSTJustFranz.Checked = False
			Me.Chk_QSTListeNullBetrag.Checked = True

		End Try
		m_PermissionData = m_BaseTableData.PerformPermissionDataOverWebService(m_InitializationData.UserData.UserLanguage)
		m_BaseTableData.BaseTableName = "Country"
		m_countryData = m_BaseTableData.PerformCVLBaseTablelistWebserviceCall()


		m_SearchCriteriums = New QSTListingSearchData With {.MDNr = m_InitializationData.MDData.MDNr,
			.FirstEmployment = Chk_QSTListeNurErstenES.Checked, .HideZeroAmount = Chk_QSTListeNullBetrag.Checked, .HideFranz = chk_QSTFranz.Checked, .ShowFranz = chk_QSTJustFranz.Checked}


		m_SuppressUIEvents = False

		AddHandler lueCountry.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueNationality.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueQSTCode.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueQSTCode.QueryPopUp, AddressOf checkedComboBoxEdit1_QueryPopUp
		AddHandler lueQSTCode.EditValueChanged, AddressOf OnlueQSTCode_EditValueChanged

		AddHandler luePermissionCode.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler luePermissionCode.QueryPopUp, AddressOf checkedComboBoxEdit1_QueryPopUp


		AddHandler lueMandant.EditValueChanged, AddressOf OnlueMandant_EditValueChanged

		AddHandler lueYear.EditValueChanged, AddressOf OnlueYear_EditValueChanged
		AddHandler lueMonthFrom.EditValueChanged, AddressOf OnlueMonth_EditValueChanged
		AddHandler lueMonthTo.EditValueChanged, AddressOf OnlueMonth_EditValueChanged
		AddHandler lueLAData.EditValueChanged, AddressOf OnlueLAData_EditValueChanged
		AddHandler lueCanton.EditValueChanged, AddressOf OnlueCanton_EditValueChanged
		AddHandler lueCommunity.EditValueChanged, AddressOf OnlueCommunity_EditValueChanged



	End Sub

#End Region


#Region "private property"

	Private Property GetMDDbName As String
	Private Property GetMDGuid As String

	Private PrintListingThread As Thread
	Private Property PrintJobNr As String
	Private Property SQL4Print As String
	Private Property bPrintAsDesign As Boolean

	Private ReadOnly Property GetHwnd() As String
		Get
			Return Me.Handle
		End Get
	End Property

#End Region


#Region "public methodes"

	Public Sub LoadData()

		m_SuppressUIEvents = True

		LoadMandantenDropDown()
		lueMandant.EditValue = m_InitializationData.MDData.MDNr
		LoadPeriodData()

		LoadQSTYearData()
		LoadQSTMonthData()

		m_SuppressUIEvents = False

		SetPreSelectedData()

	End Sub

#End Region


#Region "Reset"

	Private Sub Reset()

		ResetMandantenDropDown()
		ResetYearDropDown()
		ResetMonthDropDown()

		ResetLADropDown()
		ResetCantonDropDown()
		ResetCommunityDropDown()
		ResetXMLListArtDropDown()

		lueMonthTo.Visible = False
		lblArt.Visible = False
		lueXMLArt.Visible = False
		Chk_LeereDeklaration.Visible = False
		Cbo_Periode.Properties.DropDownRows = 9

		ResetCountryDropDown()
		ResetNationalityDropDown()
		ResetQSTCodeDropDown()
		ResetPermissionCodeDropDown()

	End Sub

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

	Private Sub ResetYearDropDown()

		lueYear.Properties.DisplayMember = "Value"
		lueYear.Properties.ValueMember = "Value"
		lueYear.Properties.ShowHeader = False

		lueYear.Properties.Columns.Clear()
		lueYear.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "Value", .Width = 100, .Caption = m_Translate.GetSafeTranslationValue("Value")})

		lueYear.Properties.ShowFooter = False
		lueYear.Properties.DropDownRows = 10
		lueYear.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueYear.Properties.SearchMode = SearchMode.AutoComplete
		lueYear.Properties.AutoSearchColumnIndex = 0

		lueYear.Properties.NullText = String.Empty
		lueYear.EditValue = Nothing
	End Sub

	Private Sub ResetMonthDropDown()

		lueMonthFrom.Properties.DisplayMember = "Value"
		lueMonthFrom.Properties.ValueMember = "Value"
		lueMonthFrom.Properties.ShowHeader = False

		lueMonthFrom.Properties.Columns.Clear()
		lueMonthFrom.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "Value", .Width = 100, .Caption = m_Translate.GetSafeTranslationValue("Monat")})

		lueMonthFrom.Properties.ShowFooter = False
		lueMonthFrom.Properties.DropDownRows = 10
		lueMonthFrom.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueMonthFrom.Properties.SearchMode = SearchMode.AutoComplete
		lueMonthFrom.Properties.AutoSearchColumnIndex = 0

		lueMonthFrom.Properties.NullText = String.Empty
		lueMonthFrom.EditValue = Nothing


		lueMonthTo.Properties.DisplayMember = "Value"
		lueMonthTo.Properties.ValueMember = "Value"
		lueMonthTo.Properties.ShowHeader = False

		lueMonthTo.Properties.Columns.Clear()
		lueMonthTo.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "Value", .Width = 100, .Caption = m_Translate.GetSafeTranslationValue("Monat")})

		lueMonthTo.Properties.ShowFooter = False
		lueMonthTo.Properties.DropDownRows = 10
		lueMonthTo.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueMonthTo.Properties.SearchMode = SearchMode.AutoComplete
		lueMonthTo.Properties.AutoSearchColumnIndex = 0

		lueMonthTo.Properties.NullText = String.Empty
		lueMonthTo.EditValue = Nothing


	End Sub

	Private Sub ResetLADropDown()

		lueLAData.Properties.Items.Clear()
		lueLAData.Properties.DisplayMember = "DisplayText"
		lueLAData.Properties.ValueMember = "LANr"

		lueLAData.Properties.DropDownRows = 10
		lueLAData.Properties.ForceUpdateEditValue = DevExpress.Utils.DefaultBoolean.True

		lueLAData.Properties.NullText = String.Empty
		lueLAData.EditValue = Nothing

	End Sub

	Private Sub ResetCantonDropDown()

		lueCanton.Enabled = True

		lueCanton.Properties.DisplayMember = "Canton"
		lueCanton.Properties.ValueMember = "Canton"

		Dim columns = lueCanton.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("Canton", 0))

		lueCanton.Properties.ShowHeader = False
		lueCanton.Properties.ShowFooter = False
		lueCanton.Properties.DropDownRows = 10
		lueCanton.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueCanton.Properties.SearchMode = SearchMode.AutoComplete
		lueCanton.Properties.AutoSearchColumnIndex = 0

		lueCanton.Properties.NullText = String.Empty

		lueCanton.EditValue = Nothing

	End Sub

	Private Sub ResetCommunityDropDown()

		'lueCommunity.Properties.SearchMode = SearchMode.OnlyInPopup
		'lueCommunity.Properties.TextEditStyle = TextEditStyles.Standard

		lueCommunity.Properties.DisplayMember = "CommunityName"
		lueCommunity.Properties.ValueMember = "CommunityCode"

		Dim columns = lueCommunity.Properties.Columns
		columns.Clear()
		'columns.Add(New LookUpColumnInfo("CommunityCode", 0, String.Empty))
		columns.Add(New LookUpColumnInfo("CommunityName", 0, String.Empty))

		lueCommunity.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueCommunity.Properties.SearchMode = SearchMode.AutoComplete
		lueCommunity.Properties.AutoSearchColumnIndex = 1
		lueCommunity.Properties.NullText = String.Empty
		lueCommunity.EditValue = Nothing

	End Sub

	Private Sub ResetXMLListArtDropDown()

		lueXMLArt.Enabled = True

		lueXMLArt.Properties.DisplayMember = "DisplayLable"
		lueXMLArt.Properties.ValueMember = "Value"

		Dim columns = lueXMLArt.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("DisplayLable", 0))

		lueXMLArt.Properties.ShowHeader = False
		lueXMLArt.Properties.ShowFooter = False
		lueXMLArt.Properties.DropDownRows = 10
		lueXMLArt.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueXMLArt.Properties.SearchMode = SearchMode.AutoComplete
		lueXMLArt.Properties.AutoSearchColumnIndex = 0

		lueXMLArt.Properties.NullText = String.Empty

		lueXMLArt.EditValue = Nothing

	End Sub

	Private Sub ResetCountryDropDown()

		lueCountry.Properties.Items.Clear()

		lueCountry.Properties.DisplayMember = "CountryDataViewData"
		lueCountry.Properties.ValueMember = "Code"

		lueCountry.Properties.DropDownRows = 10
		lueCountry.Properties.ForceUpdateEditValue = DevExpress.Utils.DefaultBoolean.True

		lueCountry.Properties.NullText = String.Empty
		lueCountry.EditValue = Nothing

	End Sub

	Private Sub ResetNationalityDropDown()

		lueNationality.Properties.Items.Clear()

		lueNationality.Properties.DisplayMember = "CountryDataViewData"
		lueNationality.Properties.ValueMember = "Code"

		lueNationality.Properties.DropDownRows = 10
		lueNationality.Properties.ForceUpdateEditValue = DevExpress.Utils.DefaultBoolean.True

		lueNationality.Properties.NullText = String.Empty
		lueNationality.EditValue = Nothing

	End Sub

	Private Sub ResetQSTCodeDropDown()

		lueQSTCode.Properties.Items.Clear()

		lueQSTCode.Properties.DisplayMember = "QSTCodeViewData"
		lueQSTCode.Properties.ValueMember = "QSTCode"

		lueQSTCode.Properties.DropDownRows = 10
		lueQSTCode.Properties.ForceUpdateEditValue = DevExpress.Utils.DefaultBoolean.True

		lueQSTCode.Properties.NullText = String.Empty
		lueQSTCode.EditValue = Nothing

	End Sub

	Private Sub ResetPermissionCodeDropDown()

		luePermissionCode.Properties.Items.Clear()

		luePermissionCode.Properties.DisplayMember = "PermissionCodeViewData"
		luePermissionCode.Properties.ValueMember = "PermissionCode"

		luePermissionCode.Properties.DropDownRows = 20
		luePermissionCode.Properties.ForceUpdateEditValue = DevExpress.Utils.DefaultBoolean.True

		luePermissionCode.Properties.NullText = String.Empty
		luePermissionCode.EditValue = Nothing

	End Sub

#End Region


#Region "loading data"

	''' <summary>
	''' Load Mandanten drop down
	''' </summary>
	''' <remarks></remarks>
	Private Sub LoadMandantenDropDown()

		Dim Data = m_CommonDatabaseAccess.LoadCompaniesListData

		lueMandant.Properties.DataSource = Data
		lueMandant.Properties.ForceInitialize()

	End Sub

	Private Function LoadQSTLAData() As Boolean
		Dim result As Boolean = True

		m_SuppressUIEvents = True
		LoadLohnartData()

		For i As Integer = 0 To lueLAData.Properties.Items.Count - 1
			If lueLAData.Properties.Items(i).Value = 7600 Then lueLAData.Properties.Items(i).CheckState = CheckState.Checked
		Next

		m_SuppressUIEvents = False

		LoadQSTCantonData()
		LoadQSTCCommunityData()

		LoadNationalityDropDown()
		LoadCountryDropDown()
		LoadQSTCodeDropDown()
		LoadPermissionCodeDropDown()


		Return result

	End Function

	Private Function LoadQSTYearData() As Boolean
		Dim result As Boolean = True

		If m_InitializationData Is Nothing Then Return False
		Dim wrappedValues As List(Of IntegerValueViewWrapper) = Nothing
		Dim yearData = m_ListingDatabaseAccess.LoadQSTYearData(lueMandant.EditValue)


		If (yearData Is Nothing) Then
			result = False
			m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten für Jahre konnten nicht geladen werden."))
		End If

		If Not yearData Is Nothing Then
			wrappedValues = New List(Of IntegerValueViewWrapper)

			For Each yearValue In yearData.OrderByDescending(Function(p) p)
				wrappedValues.Add(New IntegerValueViewWrapper With {.Value = yearValue})
			Next

		End If

		lueYear.EditValue = Nothing
		lueYear.Properties.DataSource = wrappedValues
		lueYear.Properties.DropDownRows = Math.Min(12, wrappedValues.Count + 1)
		lueYear.Properties.ForceInitialize()


		Return Not yearData Is Nothing

	End Function

	Private Function LoadQSTMonthData() As Boolean
		Dim result As Boolean = True

		If lueYear.EditValue Is Nothing Then lueYear.EditValue = Now.Year
		Dim wrappedValues As List(Of IntegerValueViewWrapper) = Nothing
		Dim monthData = m_ListingDatabaseAccess.LoadQSTMonthData(lueMandant.EditValue, lueYear.EditValue)

		If (monthData Is Nothing) Then
			result = False
			m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten für Monaten konnten nicht geladen werden."))
		End If

		If Not monthData Is Nothing Then
			wrappedValues = New List(Of IntegerValueViewWrapper)

			For Each monthValue In monthData
				wrappedValues.Add(New IntegerValueViewWrapper With {.Value = monthValue})
			Next

		End If

		lueMonthFrom.Properties.DataSource = wrappedValues
		lueMonthFrom.Properties.DropDownRows = Math.Min(12, wrappedValues.Count + 1)
		lueMonthFrom.Properties.ForceInitialize()

		lueMonthTo.Properties.DataSource = wrappedValues
		lueMonthTo.Properties.DropDownRows = Math.Min(12, wrappedValues.Count + 1)
		lueMonthTo.Properties.ForceInitialize()

		Return Not monthData Is Nothing

	End Function

	Private Function LoadLohnartData() As Boolean
		Dim result As Boolean = True

		If lueYear.EditValue Is Nothing OrElse lueMonthFrom.EditValue Is Nothing Then Return True
		Dim data = m_ListingDatabaseAccess.LoadQSTLAData(lueMandant.EditValue, lueYear.EditValue, lueMonthFrom.EditValue, lueMonthTo.EditValue)
		If data Is Nothing Then
			Dim msg As String = "Lohnarten Daten konnten nicht geladen werden."
			msg = m_Translate.GetSafeTranslationValue(msg)

			m_UtilityUi.ShowErrorDialog(msg)

			Return False
		End If

		lueLAData.Properties.DataSource = data
		lueLAData.Properties.DropDownRows = Math.Min(20, data.Count + 1)
		lueLAData.Properties.RefreshDataSource()
		lueLAData.Enabled = data.Count > 0


		Return Not data Is Nothing

	End Function

	Private Function LoadQSTCantonData() As Boolean
		Dim result As Boolean = True

		If lueMandant.EditValue Is Nothing OrElse lueYear.EditValue Is Nothing OrElse lueMonthFrom.EditValue Is Nothing OrElse lueLAData.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(lueLAData.EditValue) Then Return True
		Dim data = m_ListingDatabaseAccess.LoadQSTCantonData(lueMandant.EditValue, lueYear.EditValue, lueMonthFrom.EditValue, lueMonthTo.EditValue, lueLAData.EditValue)
		If data Is Nothing Then
			Dim msg As String = "Kanton Daten konnten nicht geladen werden."
			msg = m_Translate.GetSafeTranslationValue(msg)

			m_UtilityUi.ShowErrorDialog(msg)

			Return False
		End If

		lueCanton.Properties.DataSource = data
		lueCanton.Properties.DropDownRows = Math.Min(20, data.Count + 1)
		lueCanton.Properties.ForceInitialize()
		lueCanton.Enabled = data.Count > 0


		Return Not data Is Nothing

	End Function

	Private Function LoadQSTCCommunityData() As Boolean
		Dim result As Boolean = True

		If lueMandant.EditValue Is Nothing OrElse lueYear.EditValue Is Nothing OrElse lueMonthFrom.EditValue Is Nothing OrElse lueLAData.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(lueLAData.EditValue) OrElse lueCanton.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(lueCanton.EditValue) Then Return True
		Dim data = m_ListingDatabaseAccess.LoadQSTCommunityData(lueMandant.EditValue, lueYear.EditValue, lueMonthFrom.EditValue, lueMonthTo.EditValue, lueLAData.EditValue, lueCanton.EditValue)
		If data Is Nothing Then
			Dim msg As String = "Gemeinde Daten konnten nicht geladen werden."
			msg = m_Translate.GetSafeTranslationValue(msg)

			m_UtilityUi.ShowErrorDialog(msg)

			Return False
		End If
		Dim communityData = m_BaseTableData.PerformCommunityDataOverWebService(String.Empty, m_InitializationData.UserData.UserLanguage)

		Dim listData As New BindingList(Of QSTCommunityData)
		For Each itm In data
			Dim viewData = New QSTCommunityData
			Dim communityCode As String = itm.CommunityCode
			Dim communityName As String = itm.CommunityName

			If Not String.IsNullOrWhiteSpace(Trim(Regex.Replace(communityCode, "\d", ""))) Then
				communityName = Trim(Regex.Replace(communityCode, "\d", ""))
				communityCode = String.Format("{0}", Val(communityCode))
			End If

			If communityCode = communityName Then
				Dim community = communityData.Where(Function(x) x.BFSNumber = Val(communityName)).FirstOrDefault
				If Not community Is Nothing Then
					communityName = community.Translated_Value
				End If
			End If

			viewData.CommunityCode = itm.CommunityCode
			viewData.CommunityName = communityName


			listData.Add(viewData)

		Next

		lueCommunity.Properties.DataSource = listData
		lueCommunity.Properties.DropDownRows = Math.Min(20, listData.Count)

		lueCommunity.Properties.ForceInitialize()
		lueCommunity.Enabled = listData.Count > 0


		Return Not data Is Nothing

	End Function

	Private Function LoadXMLDeklarationnArt() As Boolean
		Dim result As Boolean = True

		Dim dataList = New List(Of ComboboxValue)
		dataList.Add(New ComboboxValue With {.DisplayLable = "Deklaration", .Value = "LR_Deklaration"})
		dataList.Add(New ComboboxValue With {.DisplayLable = "Korrektur", .Value = "LR_Korrektur"})

		lueXMLArt.Properties.DataSource = dataList
		lueXMLArt.EditValue = "LR_Deklaration"


		Return result

	End Function

	Private Function LoadCountryDropDown() As Boolean
		Dim result As Boolean = True

		If lueMandant.EditValue Is Nothing OrElse lueYear.EditValue Is Nothing OrElse lueMonthFrom.EditValue Is Nothing Then Return True
		Dim data = m_ListingDatabaseAccess.LoadTaxCountryCodeData(lueMandant.EditValue, lueYear.EditValue, lueMonthFrom.EditValue, lueMonthTo.EditValue, lueCanton.EditValue)
		If data Is Nothing Then
			Dim msg As String = "Länder Daten konnten nicht geladen werden."
			msg = m_Translate.GetSafeTranslationValue(msg)

			m_UtilityUi.ShowErrorDialog(msg)

			Return False
		End If
		Dim listData = New BindingList(Of CountryData)

		For Each itm In data
			Dim code = New CountryData


			If Not String.IsNullOrWhiteSpace(itm.Code) Then

				If Not m_CountryData Is Nothing AndAlso m_CountryData.Count > 0 Then
					Dim lndData = New SP.Internal.Automations.CVLBaseTableViewData
					lndData = m_CountryData.Where(Function(x) x.Code = itm.Code).FirstOrDefault()
					If Not lndData Is Nothing AndAlso Not String.IsNullOrWhiteSpace(lndData.Translated_Value) Then
						code.Code = itm.Code
						code.Name = lndData.Translated_Value
					End If

				Else
					code.Code = itm.Code
					code.Name = itm.Name

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

		If lueMandant.EditValue Is Nothing OrElse lueYear.EditValue Is Nothing OrElse lueMonthFrom.EditValue Is Nothing Then Return True
		Dim data = m_ListingDatabaseAccess.LoadTaxNationalityCodeData(lueMandant.EditValue, lueYear.EditValue, lueMonthFrom.EditValue, lueMonthTo.EditValue, lueCanton.EditValue)
		If data Is Nothing Then
			Dim msg As String = "Nationalität Daten konnten nicht geladen werden."
			msg = m_Translate.GetSafeTranslationValue(msg)

			m_UtilityUi.ShowErrorDialog(msg)

			Return False
		End If
		Dim listData = New BindingList(Of CountryData)

		For Each itm In data
			Dim code = New CountryData


			If Not String.IsNullOrWhiteSpace(itm.Code) Then

				If Not m_CountryData Is Nothing AndAlso m_CountryData.Count > 0 Then
					Dim lndData = New SP.Internal.Automations.CVLBaseTableViewData
					lndData = m_CountryData.Where(Function(x) x.Code = itm.Code).FirstOrDefault()
					If Not lndData Is Nothing AndAlso Not String.IsNullOrWhiteSpace(lndData.Translated_Value) Then
						code.Code = itm.Code
						code.Name = lndData.Translated_Value
					End If

				Else
					code.Code = itm.Code
					code.Name = itm.Name

				End If

				listData.Add(code)
			End If

		Next

		lueNationality.Properties.DataSource = listData
		lueNationality.Properties.DropDownRows = Math.Min(20, listData.Count + 1)
		lueNationality.Enabled = listData.Count > 0


		Return Not listData Is Nothing

	End Function

	Private Function LoadQSTCodeDropDown() As Boolean
		Dim result As Boolean = True

		If lueMandant.EditValue Is Nothing OrElse lueYear.EditValue Is Nothing OrElse lueMonthFrom.EditValue Is Nothing OrElse lueLAData.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(lueLAData.EditValue) Then Return True
		Dim data = m_ListingDatabaseAccess.LoadQSTCodeData(lueMandant.EditValue, lueYear.EditValue, lueMonthFrom.EditValue, lueMonthTo.EditValue, lueCanton.EditValue)
		If data Is Nothing Then
			Dim msg As String = "Quellensteuer-Code Daten konnten nicht geladen werden."
			msg = m_Translate.GetSafeTranslationValue(msg)

			m_UtilityUi.ShowErrorDialog(msg)

			Return False
		End If

		lueQSTCode.Properties.DataSource = data
		lueQSTCode.Properties.DropDownRows = Math.Min(20, data.Count + 1)
		lueQSTCode.Enabled = data.Count > 0


		Return Not data Is Nothing

	End Function

	Private Function LoadPermissionCodeDropDown() As Boolean
		Dim result As Boolean = True
		Dim listData = New BindingList(Of QSTPermissionData)

		luePermissionCode.EditValue = Nothing
		If lueMandant.EditValue Is Nothing OrElse lueYear.EditValue Is Nothing OrElse lueMonthFrom.EditValue Is Nothing OrElse lueLAData.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(lueLAData.EditValue) Then Return True
		Try

			Dim data = m_ListingDatabaseAccess.LoadQSTPermissionData(lueMandant.EditValue, lueYear.EditValue, lueMonthFrom.EditValue, lueMonthTo.EditValue, lueCanton.EditValue, lueQSTCode.EditValue)
			If data Is Nothing Then
				Dim msg As String = "Bewilligung Daten konnten nicht geladen werden."
				msg = m_Translate.GetSafeTranslationValue(msg)

				m_UtilityUi.ShowErrorDialog(msg)

				Return False
			End If

			For Each itm In data
				Dim code = New QSTPermissionData


				If Not String.IsNullOrWhiteSpace(itm.PermissionCode) Then
					code.PermissionCode = itm.PermissionCode
					code.PermissionCodeLabel = itm.PermissionCodeLabel

					If Not m_PermissionData Is Nothing AndAlso m_PermissionData.Count > 0 Then
						Dim bewdata = New SP.Internal.Automations.PermissionData
						bewdata = m_PermissionData.Where(Function(x) x.Code = itm.PermissionCode).FirstOrDefault()
						If Not bewdata Is Nothing AndAlso Not String.IsNullOrWhiteSpace(bewdata.Translated_Value) Then
							code.PermissionCode = itm.PermissionCode
							code.PermissionCodeLabel = bewdata.Translated_Value
						End If
					End If

					listData.Add(code)
				End If

			Next
			luePermissionCode.Properties.DataSource = listData
			luePermissionCode.Properties.DropDownRows = Math.Min(20, listData.Count + 1)
			luePermissionCode.Enabled = listData.Count > 0

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

			Return True
		End Try


		Return Not listData Is Nothing

	End Function


#End Region


	Private Sub SetPreSelectedData()
		Dim allowedMDSelection As Boolean = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 675, m_InitializationData.MDData.MDNr)

		lueMandant.EditValue = m_InitializationData.MDData.MDNr
		lueMandant.Visible = allowedMDSelection OrElse m_InitializationData.UserData.UserNr = 1
		lblMDName.Visible = allowedMDSelection OrElse m_InitializationData.UserData.UserNr = 1

		' Bis am 14. des Monats wird der Vormonat selektiert. Ab 15. den aktuellen Monat
		Dim datum As Date = Date.Now
		If datum.Day < 15 Then
			datum = datum.AddMonths(-1)
		End If

		Dim supressUIState = m_SuppressUIEvents
		m_SuppressUIEvents = True

		Me.lueYear.EditValue = Now.Year
		Me.lueMonthFrom.EditValue = datum.Month
		Dim monthData = TryCast(lueMonthFrom.Properties.DataSource, List(Of IntegerValueViewWrapper))
		If Not monthData Is Nothing AndAlso monthData.Count > 0 Then
			Dim selectedMonthData = monthData.Where(Function(data) data.Value = datum.Month).FirstOrDefault
			If selectedMonthData Is Nothing Then
				lueMonthFrom.EditValue = monthData(monthData.Count - 1).Value
			End If
		End If

		Me.lueMonthTo.EditValue = Nothing

		LoadQSTLAData()

		Me.bbiExport.Enabled = False
		Me.bbiPrint.Enabled = False

		m_SuppressUIEvents = supressUIState


	End Sub

	Private Sub OnlueMandant_EditValueChanged(sender As Object, e As System.EventArgs)
		If m_SuppressUIEvents Then Return

		Dim SelectedData As MandantData = TryCast(Me.lueMandant.GetSelectedDataRow(), MandantData)

		If Not SelectedData Is Nothing Then
			Dim MandantData = ClsDataDetail.ChangeMandantData(lueMandant.EditValue, m_InitializationData.UserData.UserNr)
			m_InitializationData = MandantData

			m_ConnectionString = m_InitializationData.MDData.MDDbConn
			m_CommonDatabaseAccess = New CommonDatabaseAccess(m_ConnectionString, m_InitializationData.UserData.UserLanguage)
			m_ListingDatabaseAccess = New ListingDatabaseAccess(m_ConnectionString, m_InitializationData.UserData.UserLanguage)


			LoadQSTLAData()
		Else
			' do nothing
		End If

		Me.bbiSearch.Enabled = Not (ClsDataDetail.m_InitialData.MDData Is Nothing)
		Me.bbiPrint.Enabled = Not (ClsDataDetail.m_InitialData.MDData Is Nothing)
		Me.bbiExport.Enabled = Not (ClsDataDetail.m_InitialData.MDData Is Nothing)

	End Sub

	Private Sub OnlueYear_EditValueChanged(sender As Object, e As System.EventArgs)
		If m_SuppressUIEvents Then Return

		If Not lueYear.EditValue Is Nothing Then
			LoadQSTMonthData()
		Else
			' do nothing
		End If

		LoadQSTLAData()

	End Sub

	Private Sub OnlueMonth_EditValueChanged(sender As Object, e As System.EventArgs)
		If m_SuppressUIEvents Then Return

		'If Not sender.EditValue Is Nothing Then
		'	Dim supressUIState = m_SuppressUIEvents
		'	m_SuppressUIEvents = True

		'	LoadQSTLAData()
		'	m_SuppressUIEvents = supressUIState

		'	Return
		'Else
		'	' do nothing
		'End If

		LoadQSTLAData()

	End Sub

	Private Sub OnlueLAData_EditValueChanged(sender As Object, e As System.EventArgs)
		If m_SuppressUIEvents Then Return

		lueCanton.EditValue = Nothing
		lueCommunity.EditValue = Nothing
		If Not lueLAData.EditValue Is Nothing AndAlso Not String.IsNullOrWhiteSpace(lueLAData.EditValue) Then
			LoadQSTCantonData()
			LoadQSTCCommunityData()

			LoadCountryDropDown()
			LoadNationalityDropDown()
			LoadQSTCodeDropDown()
			LoadPermissionCodeDropDown()
		Else
			' do nothing
		End If

	End Sub

	Private Sub OnlueCanton_EditValueChanged(sender As Object, e As System.EventArgs)
		If m_SuppressUIEvents Then Return
		Dim showXMLSettingFields As Boolean = False

		lueCommunity.EditValue = Nothing
		lueXMLArt.EditValue = Nothing
		Chk_LeereDeklaration.Checked = False
		lueQSTCode.EditValue = Nothing
		luePermissionCode.EditValue = Nothing

		If Not lueLAData.EditValue Is Nothing AndAlso Not String.IsNullOrWhiteSpace(lueLAData.EditValue) AndAlso Not lueCanton.EditValue Is Nothing AndAlso Not String.IsNullOrWhiteSpace(lueCanton.EditValue) Then
			LoadQSTCCommunityData()

			showXMLSettingFields = lueCanton.EditValue = "VD"
			If showXMLSettingFields Then LoadXMLDeklarationnArt()

		Else
			' do nothing
		End If

		lueXMLArt.Visible = showXMLSettingFields
		lblArt.Visible = showXMLSettingFields
		Chk_LeereDeklaration.Visible = showXMLSettingFields

	End Sub

	Private Sub OnlueCommunity_EditValueChanged(sender As Object, e As System.EventArgs)
		If m_SuppressUIEvents Then Return

		lueQSTCode.EditValue = Nothing
		luePermissionCode.EditValue = Nothing
		'If Not lueLAData.EditValue Is Nothing AndAlso Not String.IsNullOrWhiteSpace(lueLAData.EditValue) Then
		'	'LoadQSTCodeData()
		'	'LoadPermissionCodeData()
		'Else
		'	' do nothing
		'End If

	End Sub

	Private Sub OnlueQSTCode_EditValueChanged(sender As Object, e As System.EventArgs)
		If m_SuppressUIEvents Then Return

		luePermissionCode.EditValue = Nothing
		If Not lueLAData.EditValue Is Nothing AndAlso Not String.IsNullOrWhiteSpace(lueLAData.EditValue) Then
			LoadPermissionCodeDropDown()
		Else
			' do nothing
		End If

	End Sub


#Region "Dropdown Funktionen Allgemein"

	Private Sub LoadPeriodData()
		Dim datum As Date = Date.Now

		Cbo_Periode.Properties.Items.Clear()

		Cbo_Periode.Properties.Items.Add(String.Empty)
		Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("Diesen Monat ({0:00}/{1})"), datum.Month, datum.Year))
		Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("Letzten Monat ({0:00}/{1})"), datum.AddMonths(-1).Month, datum.AddMonths(-1).Year))

		Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}. Quartal ({1})"), 1, datum.Year))
		Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}. Quartal ({1})"), 2, datum.Year))
		Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}. Quartal ({1})"), 3, datum.Year))
		Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}. Quartal ({1})"), 4, datum.Year))
		Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("Dieses Jahr ({0})"), datum.Year))

		Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("Letztes Jahr ({0})"), datum.AddYears(-1).Year))
	End Sub

	'Private Sub Cbo_QSTListeJahr_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lueYear.QueryPopUp
	'	If Me.lueYear.Properties.Items.Count = 0 Then ListQSTListeJahr(Me.lueYear)
	'End Sub

#End Region


	Private Sub CmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
		Me.Dispose()
	End Sub

	Private Sub frmQSTListeSearch_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

		FormIsLoaded("frmQSTListeSearch_LV", True)
		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.frm_Location = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.ifrmHeight = Me.Height
				My.Settings.ifrmWidth = Me.Width

				My.Settings.checkedQSTFranz = If(Me.chk_QSTFranz.Checked, True, False)
				My.Settings.checkedQSTjustFranz = If(Me.chk_QSTJustFranz.Checked, True, False)
				My.Settings.CHECKZEROAMOUNT = If(Me.Chk_QSTListeNullBetrag.Checked, True, False)
				My.Settings.CHECKFIRSTES = If(Me.Chk_QSTListeNurErstenES.Checked, True, False)

				My.Settings.chkHideBruttolohn = If(Me.chkHideBruttolohn.Checked, True, False)
				My.Settings.chkHideQSTBasis = If(Me.chkHideQSTBasis.Checked, True, False)
				My.Settings.chkHideQSTBasis2 = If(Me.chkHideQSTBasis2.Checked, True, False)


				My.Settings.Save()
			End If


		Catch ex As Exception

		End Try

	End Sub

	Private Sub frmQSTListeSearch_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Move
		If FormIsLoaded("frmQSTListeSearch_LV", False) Then
			frmMyLV.Location = New Point(Me.Location.X - frmMyLV.Width - 5, Me.Location.Y)
			frmMyLV.TopMost = True
			frmMyLV.TopMost = False
		End If
	End Sub

	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

		Me.lblHeaderFett.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderFett.Text)
		Me.lblHeaderNormal.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderNormal.Text)

		Me.CmdClose.Text = m_Translate.GetSafeTranslationValue(Me.CmdClose.Text)

		Me.lblMDName.Text = m_Translate.GetSafeTranslationValue(Me.lblMDName.Text)
		Me.lblMANr.Text = m_Translate.GetSafeTranslationValue(Me.lblMANr.Text)

		Me.lblPeriode.Text = m_Translate.GetSafeTranslationValue(Me.lblPeriode.Text)
		Me.lblJahr.Text = m_Translate.GetSafeTranslationValue(Me.lblJahr.Text)
		Me.lblMonat.Text = m_Translate.GetSafeTranslationValue(Me.lblMonat.Text)
		Me.lblLohnart.Text = m_Translate.GetSafeTranslationValue(Me.lblLohnart.Text)
		Me.lblKanton.Text = m_Translate.GetSafeTranslationValue(Me.lblKanton.Text)
		Me.lblGemeinde.Text = m_Translate.GetSafeTranslationValue(Me.lblGemeinde.Text)
		Me.lblArt.Text = m_Translate.GetSafeTranslationValue(Me.lblArt.Text)
		lblQSTCode.Text = m_Translate.GetSafeTranslationValue(lblQSTCode.Text)
		Me.lblBewilligung.Text = m_Translate.GetSafeTranslationValue(Me.lblBewilligung.Text)

		Me.grpEigenschaften.Text = m_Translate.GetSafeTranslationValue(Me.grpEigenschaften.Text)
		Me.chk_QSTFranz.Text = m_Translate.GetSafeTranslationValue(Me.chk_QSTFranz.Text)
		Me.chk_QSTJustFranz.Text = m_Translate.GetSafeTranslationValue(Me.chk_QSTJustFranz.Text)
		Me.Chk_LeereDeklaration.Text = m_Translate.GetSafeTranslationValue(Me.Chk_LeereDeklaration.Text)
		Me.Chk_QSTListeNullBetrag.Text = m_Translate.GetSafeTranslationValue(Me.Chk_QSTListeNullBetrag.Text)
		Me.Chk_QSTListeNurErstenES.Text = m_Translate.GetSafeTranslationValue(Me.Chk_QSTListeNurErstenES.Text)

		chkSetOrtAsGemeinde.Text = m_Translate.GetSafeTranslationValue(chkSetOrtAsGemeinde.Text)
		Me.grpListenEigenschaften.Text = m_Translate.GetSafeTranslationValue(Me.grpListenEigenschaften.Text)
		Me.chkHideBruttolohn.Text = m_Translate.GetSafeTranslationValue(Me.chkHideBruttolohn.Text)
		Me.chkHideQSTBasis.Text = m_Translate.GetSafeTranslationValue(Me.chkHideQSTBasis.Text)
		Me.chkHideQSTBasis2.Text = m_Translate.GetSafeTranslationValue(Me.chkHideQSTBasis2.Text)

		Me.lblSQLAbfrage.Text = m_Translate.GetSafeTranslationValue(Me.lblSQLAbfrage.Text)

		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)
		Me.bbiPrint.Caption = m_Translate.GetSafeTranslationValue(Me.bbiPrint.Caption)
		Me.bbiClearFields.Caption = m_Translate.GetSafeTranslationValue(Me.bbiClearFields.Caption)
		Me.bbiExport.Caption = m_Translate.GetSafeTranslationValue(Me.bbiExport.Caption)

		For Each tbp As DevExpress.XtraTab.XtraTabPage In Me.XtraTabControl1.TabPages
			tbp.Text = m_Translate.GetSafeTranslationValue(tbp.Text)
		Next

	End Sub

	''' <summary>
	''' Starten von Anwendung.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub frmQSTListeSearch_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

		Me.Width = Math.Max(My.Settings.ifrmWidth, Me.Width)
		Me.Height = Math.Max(My.Settings.ifrmHeight, Me.Height)
		If My.Settings.frm_Location <> String.Empty Then
			Dim aLoc As String() = My.Settings.frm_Location.Split(CChar(";"))

			If Screen.AllScreens.Length = 1 Then
				If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
			End If
			Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
		End If


		'Dim UpdateDelegate As New MethodInvoker(AddressOf TranslateControls)
		'Me.Invoke(UpdateDelegate)

		Me.xtabSQLQuery.PageVisible = m_InitializationData.UserData.UserNr = 1

		If m_InitializationData.UserData.UserNr = 1 Then
			Me.bbiExport.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
		Else
			If IsUserAllowed4DocExport("11.7") Then Me.bbiExport.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
		End If

	End Sub


	''' <summary>
	''' starts printing list
	''' </summary>
	''' <remarks></remarks>
	Sub StartPrinting()
		bPrintAsDesign = (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown) AndAlso IsUserActionAllowed(m_InitializationData.UserData.UserNr, 560)
		Dim loarten As String = lueLAData.EditValue.ToString

		Me.SQL4Print = Me.txt_SQLQuery.Text

		Dim countryCodes As String = String.Empty
		Dim countryData = lueCountry.Properties.GetItems.GetCheckedValues()
		For Each itm In countryData
			If Not String.IsNullOrWhiteSpace(itm) Then countryCodes &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(countryCodes), "", ", "), itm)
		Next

		Dim nationalityCodes As String = String.Empty
		Dim nationalityData = lueNationality.Properties.GetItems.GetCheckedValues()
		For Each itm In nationalityData
			If Not String.IsNullOrWhiteSpace(itm) Then nationalityCodes &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(nationalityCodes), "", ", "), itm)
		Next


		Dim qstCodes As String = String.Empty
		Dim qstData = lueQSTCode.Properties.GetItems.GetCheckedValues()
		For Each itm In qstData
			If Not String.IsNullOrWhiteSpace(itm) Then qstCodes &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(qstCodes), "", ", "), itm)
		Next

		Dim permissionCodes As String = String.Empty
		Dim permissionData = luePermissionCode.Properties.GetItems.GetCheckedValues()
		For Each itm In permissionData
			If Not String.IsNullOrWhiteSpace(itm) Then permissionCodes &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(permissionCodes), "", ", "), itm)
		Next

		Dim _Setting As New ClsLLQSTSearchPrintSetting With {.DbConnString2Open = ClsDataDetail.m_InitialData.MDData.MDDbConn,
																												 .SQL2Open = Me.SQL4Print,
																												 .JobNr2Print = Me.PrintJobNr,
																												 .ShowAsDesgin = Me.bPrintAsDesign,
																												 .ShowPrintBox = True,
																												 .SelectedCommunity = Me.lueCommunity.Text,
																												 .SelectedCanton = Me.lueCanton.Text,
																												 .SelectedMDNr = ClsDataDetail.m_InitialData.MDData.MDNr,
																												 .frmhwnd = GetHwnd,
																												 .SetEmptyGemeindeWithCity = chkSetOrtAsGemeinde.Checked,
																												 .HideBruttolohnColumn = chkHideBruttolohn.Checked,
																												 .HideQSTBasisColumn = chkHideQSTBasis.Checked,
																												 .HideQSTBasis2Column = chkHideQSTBasis2.Checked,
																												 .LogedUSNr = ClsDataDetail.m_InitialData.UserData.UserNr,
																												 .ListFilterBez = New List(Of String)(New String() {
																												 String.Format("Mandant: {0}", Me.lueMandant.Text),
																												 String.Format(m_Translate.GetSafeTranslationValue("Lohnarten: {0}"), loarten),
																												 If(Not String.IsNullOrWhiteSpace(countryCodes), String.Format(m_Translate.GetSafeTranslationValue("Wohnland: {0}"), countryCodes), String.Empty),
																												 If(Not String.IsNullOrWhiteSpace(nationalityCodes), String.Format(m_Translate.GetSafeTranslationValue("Nationalität: {0}"), nationalityCodes), String.Empty),
																												 If(Not String.IsNullOrWhiteSpace(qstCodes), String.Format(m_Translate.GetSafeTranslationValue("Tarif: {0}"), qstCodes), String.Empty),
																												 If(Not String.IsNullOrWhiteSpace(permissionCodes), String.Format(m_Translate.GetSafeTranslationValue("Bewilligung: {0}"), permissionCodes), String.Empty),
																												 If(chk_QSTFranz.Checked, "Ohne französische Grenzgänger", If(chk_QSTJustFranz.Checked, "Nur französiche Grenzgänger", "")),
																												 If(chkSetOrtAsGemeinde.Checked, "Adresse als Gemeinde (wenn leer)", "")})}
		Dim obj As New QSTSearchListing.ClsPrintQSTSearchList(m_InitializationData)
		obj.PrintData = _Setting

		obj.PrintQSTSearchList()

	End Sub

	Function GetJobNr() As String
		Dim strJobNr As String = String.Empty

		If Val(lueMonthTo.EditValue) > 0 AndAlso Val(lueMonthFrom.EditValue) <> Val(lueMonthTo.EditValue) Then
			strJobNr = "11.7.G"
		Else
			strJobNr = "11.7"
		End If

		Return strJobNr

	End Function



#Region "Funktionen zur Menüaufbau..."

	Private Sub bbiSearch_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSearch.ItemClick
		Dim sSql1Query As String = String.Empty
		Dim sSql2Query As String = String.Empty
		Dim strSort As String = String.Empty
		Me.PrintJobNr = GetJobNr()

		Me.Cursor = Cursors.WaitCursor

		Try
			SplashScreenManager.CloseForm(False)
			SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
			SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Vorlage wird übersetzt") & Space(20))
			SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

			Dim selVonDt As DateTime = DateTime.Parse(String.Format("01.{0}.{1}", Me.lueMonthFrom.Text, Me.lueYear.Text))
			Dim selBisDt As DateTime = DateTime.Parse(String.Format("01.{0}.{1}", Me.lueMonthTo.Text, Me.lueYear.Text)).AddMonths(1).AddDays(-1)
			ClsDataDetail.SelPeriodeVon = selVonDt
			ClsDataDetail.SelPeriodeBis = selBisDt

			Me.bbiPrint.Enabled = False
			Me.bbiExport.Enabled = False

			If Not LoadSearchConditions() Then Return


			txt_SQLQuery.Text = String.Empty
			bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Nach Daten wird gesucht...")
			FormIsLoaded("frmQSTListeSearch_LV", True)

			' Die Query-String aufbauen...
			Dim success As Boolean = GetMyQueryString()
			If Not success Then Return

			' Daten auflisten...
			If Not FormIsLoaded("frmQSTListeSearch_LV", True) Then
				frmMyLV = New frmQSTListeSearch_LV(txt_SQLQuery.Text)
				frmMyLV.Show()
				Me.BringToFront()
			End If

			bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet..."), frmMyLV.RecCount)
			frmMyLV.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet..."), frmMyLV.RecCount)

			If frmMyLV.RecCount > 0 Then
				m_LoadedQSTData = frmMyLV.LoadFoundedData

				Me.bbiPrint.Enabled = True
				If Me.lueCanton.Text.ToLower.Contains("vd") Then Me.bbiExport.Enabled = True
				CreateExportPopupMenu()
			End If

			ClsDataDetail.LLSelektionText = ""
			ClsDataDetail.GetFilterBez += ClsDataDetail.LLSelektionText

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			m_UtilityUi.ShowErrorDialog(ex.ToString)

		Finally
			Me.Cursor = Cursors.Default
			SplashScreenManager.CloseForm(False)

		End Try

	End Sub

	Private Function LoadSearchConditions() As Boolean

		m_SearchCriteriums = Nothing
		Dim searchData = New QSTListingSearchData

		DxErrorProvider1.ClearErrors()

		Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")

		Dim isValid As Boolean = True

		isValid = isValid And SetErrorIfInvalid(lueMandant, DxErrorProvider1, lueMandant.EditValue Is Nothing OrElse String.IsNullOrEmpty(lueMandant.Text), errorText)
		isValid = isValid And SetErrorIfInvalid(lueYear, DxErrorProvider1, lueYear.EditValue Is Nothing OrElse (lueYear.EditValue) = 0, errorText)
		isValid = isValid And SetErrorIfInvalid(lueMonthFrom, DxErrorProvider1, lueMonthFrom.EditValue Is Nothing OrElse (lueMonthFrom.EditValue) = 0, errorText)
		isValid = isValid And SetErrorIfInvalid(lueLAData, DxErrorProvider1, lueLAData.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(lueLAData.EditValue), errorText)

		If Not isValid Then Return False

		searchData.MDNr = lueMandant.EditValue

		If Not String.IsNullOrWhiteSpace(txt_MANR.EditValue) Then
			Dim maNrList = txt_MANR.EditValue.ToString.Split(New Char() {",", ";", "#"}).ToList()
			Dim employeeList As New List(Of Integer?)
			For Each itm In maNrList
				Dim laNr As Integer = itm

				employeeList.Add(laNr)
			Next
			searchData.EmployeeNumbers = employeeList ' laNrList.ConvertAll(Function(str) Decimal.Parse(str))
		End If


		searchData.Year = lueYear.EditValue
		searchData.MonthFrom = lueMonthFrom.EditValue
		searchData.MonthTo = lueMonthTo.EditValue
		If searchData.MonthTo Is Nothing Then searchData.MonthTo = searchData.MonthFrom

		Dim laNrList = lueLAData.EditValue.ToString.Split(New Char() {",", ";", "#"}).ToList()
		Dim laList As New List(Of Decimal?)
		For Each itm In laNrList
			Dim laNr As Decimal = Decimal.Parse(itm)

			laList.Add(laNr)
		Next
		searchData.LANr = laList ' laNrList.ConvertAll(Function(str) Decimal.Parse(str))

		searchData.Canton = lueCanton.EditValue
		searchData.Community = lueCommunity.EditValue

		If Not String.IsNullOrWhiteSpace(lueCountry.EditValue) Then searchData.CountryList = lueCountry.EditValue.ToString.Split(New Char() {",", ";", "#"}).ToList()
		If Not String.IsNullOrWhiteSpace(lueNationality.EditValue) Then searchData.NationaliyList = lueNationality.EditValue.ToString.Split(New Char() {",", ";", "#"}).ToList()

		If Not String.IsNullOrWhiteSpace(lueQSTCode.EditValue) Then searchData.QSTCode = lueQSTCode.EditValue.ToString.Split(New Char() {",", ";", "#"}).ToList()
		If Not String.IsNullOrWhiteSpace(luePermissionCode.EditValue) Then searchData.Permission = luePermissionCode.EditValue.ToString.Split(New Char() {",", ";", "#"}).ToList()

		searchData.XMLListingArt = lueXMLArt.EditValue
		searchData.OupputAsEmptyDeclaretion = Chk_LeereDeklaration.Checked
		searchData.CityAsCommunityIFEmpty = chkSetOrtAsGemeinde.Checked

		searchData.FirstEmployment = Chk_QSTListeNurErstenES.Checked
		searchData.HideZeroAmount = Chk_QSTListeNullBetrag.Checked
		searchData.HideFranz = chk_QSTFranz.Checked
		searchData.ShowFranz = chk_QSTJustFranz.Checked


		m_SearchCriteriums = searchData


		Return Not m_SearchCriteriums Is Nothing

	End Function

	Function GetMyQueryString() As Boolean
		Dim sSqlQuerySelect As String = String.Empty
		Dim _ClsDb As New ClsDbFunc(m_InitializationData)

		' Selektion 
		_ClsDb.SearchCriteriums = m_SearchCriteriums
		sSqlQuerySelect = _ClsDb.GetStartSQLString()
		' Query zusammenstellen
		Me.txt_SQLQuery.Text = sSqlQuerySelect

		Return Not String.IsNullOrWhiteSpace(sSqlQuerySelect)

	End Function


	''' <summary>
	''' Funktion für das Leeren der Felder...
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub bbiClearFields_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiClearFields.ItemClick

		FormIsLoaded("frmQSTListeSearch_LV", True)
		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Bereit")
		Me.txt_SQLQuery.Text = m_Translate.GetSafeTranslationValue("Wurde geleert...")

		Try
			Dim supressUIState = m_SuppressUIEvents
			m_SuppressUIEvents = True

			'For Each myItm In lueCountry.Properties.Items
			'	myItm.CheckState = CheckState.Unchecked
			'Next
			'For Each myItm In lueNationality.Properties.Items
			'	myItm.CheckState = CheckState.Unchecked
			'Next

			'lueCountry.DeselectAll()
			'lueNationality.DeselectAll()
			'lueQSTCode.DeselectAll()

			txt_MANR.EditValue = Nothing
			Cbo_Periode.SelectedItem = Nothing

			lueCountry.Properties.DataSource = Nothing
			lueNationality.Properties.DataSource = Nothing
			lueQSTCode.Properties.DataSource = Nothing
			luePermissionCode.Properties.DataSource = Nothing


			lueCanton.EditValue = Nothing
			lueCommunity.EditValue = Nothing
			lueQSTCode.EditValue = Nothing
			luePermissionCode.EditValue = Nothing
			lueNationality.EditValue = Nothing
			lueCountry.EditValue = Nothing

			SetPreSelectedData()
			m_SuppressUIEvents = supressUIState


		Catch ex As Exception

		End Try

		' Die Buttons Drucken und Export deaktivieren
		Me.bbiPrint.Enabled = False
		Me.bbiExport.Enabled = False

	End Sub

	''' <summary>
	''' Alle vorhandene TabPages und jedes darin befindliches Controls durchlaufen.
	''' </summary>
	''' <remarks></remarks>
	Private Sub ResetAllTabEntries()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Try
			For Each tabPg As DevExpress.XtraTab.XtraTabPage In Me.Controls.Item(Me.XtraTabControl1.Name).Controls
				For Each con As Control In tabPg.Controls
					ResetControl(con)
				Next
			Next

		Catch ex As Exception ' Manager
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
		End Try

	End Sub

	''' <summary>
	''' Jedes Control wird anhand des Typs zurückgesetzt.
	''' </summary>
	''' <param name="con"></param>
	''' <remarks>Bei GroupBox wird die Funktion rekursiv aufgerufen.</remarks>
	Private Sub ResetControl(ByVal con As Control)

		Try
			If con.Name.ToLower = "CboSort".ToLower Or con.Name.ToLower = "luemandant".ToLower Then Exit Sub

			' Rekursiver Aufruf
			If con.HasChildren Then
				For Each childCon As Control In con.Controls
					ResetControl(childCon)
				Next
			Else
				' Sonst Control zurücksetzen
				If TypeOf (con) Is DevExpress.XtraEditors.TextEdit Then
					Dim tb As DevExpress.XtraEditors.TextEdit = con
					tb.EditValue = Nothing

				ElseIf TypeOf (con) Is DevExpress.XtraEditors.ComboBoxEdit Then
					Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = con
					cbo.EditValue = Nothing

				ElseIf TypeOf (con) Is DevExpress.XtraEditors.CheckedComboBoxEdit Then
					Dim lookupEdit As DevExpress.XtraEditors.CheckedComboBoxEdit = CType(con, DevExpress.XtraEditors.CheckedComboBoxEdit)
					For Each myItm In lookupEdit.Properties.Items
						myItm.CheckState = CheckState.Unchecked
					Next


				End If
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.Message))

		End Try

	End Sub

#End Region


	Private Sub bbiPrint_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick
		If ClsDataDetail.QSTListeDataTable.Rows.Count > 0 Then StartPrinting()

		'Dim popupMenu As DevExpress.XtraBars.PopupMenu = Me.bbiPrint.DropDownControl

		'If Not (popupMenu Is Nothing) Then popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

	End Sub

	'Private Sub CreatePrintPopupMenu()
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim bshowMnu As Boolean = True
	'	Dim popupMenu As New DevExpress.XtraBars.PopupMenu
	'	Dim liMnu As New List(Of String) From {"Liste drucken#PrintList"}
	'	Try
	'		bbiPrint.Manager = Me.BarManager1
	'		BarManager1.ForceInitialize()

	'		Me.bbiPrint.ActAsDropDown = False
	'		Me.bbiPrint.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
	'		Me.bbiPrint.DropDownEnabled = True
	'		Me.bbiPrint.DropDownControl = popupMenu
	'		Me.bbiPrint.Enabled = True

	'		For i As Integer = 0 To liMnu.Count - 1
	'			Dim myValue As String() = liMnu(i).Split(CChar("#"))

	'			If myValue(1).ToString.ToLower.Contains("PrintDesign".ToLower) Then bshowMnu = IsUserActionAllowed(0, 560) Else bshowMnu = myValue(0).ToString <> String.Empty
	'			If bshowMnu Then
	'				popupMenu.Manager = BarManager1

	'				Dim itm As New DevExpress.XtraBars.BarButtonItem
	'				itm.Caption = m_Translate.GetSafeTranslationValue(myValue(0).ToString)
	'				itm.Name = m_Translate.GetSafeTranslationValue(myValue(1).ToString)

	'				If myValue(1).ToString.ToLower.Contains("PrintDesign".ToLower) Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)
	'				AddHandler itm.ItemClick, AddressOf GetMenuItem
	'			End If

	'		Next

	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

	'	End Try

	'End Sub


	Private Sub bbiExport_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiExport.ItemClick
		Dim popupMenu As DevExpress.XtraBars.PopupMenu = Me.bbiExport.DropDownControl
		If Not (popupMenu Is Nothing) Then popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))
	End Sub

	Private Sub CreateExportPopupMenu()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim bshowMnu As Boolean = True
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim liMnu As New List(Of String) From {"Daten in CSV- / TXT exportieren...#CSV",
			"-Daten in eQuest exportieren...#eQuest",
			If(lueCanton.Text = "VD" OrElse lueCanton.Text = "GE", String.Format(m_Translate.GetSafeTranslationValue("-Daten für Kanton {0} exportieren#XML"), lueCanton.Text), "#")
			}

		If Not IsUserAllowed4DocExport(PrintJobNr) Then Return
		Try
			bbiExport.Manager = Me.BarManager1
			BarManager1.ForceInitialize()

			Me.bbiExport.ActAsDropDown = False
			Me.bbiExport.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
			Me.bbiExport.DropDownEnabled = True
			Me.bbiExport.DropDownControl = popupMenu
			Me.bbiExport.Enabled = True

			For i As Integer = 0 To liMnu.Count - 1
				Dim myValue As String() = liMnu(i).Split(CChar("#"))
				Dim startAsGroup As Boolean = False
				bshowMnu = Not myValue(0) = String.Empty
				If bshowMnu Then
					popupMenu.Manager = BarManager1

					Dim itm As New DevExpress.XtraBars.BarButtonItem
					Dim mnuCaption As String = myValue(0).ToString
					If mnuCaption.StartsWith("-") Then
						startAsGroup = True
						mnuCaption = mnuCaption.Substring(1, Len(mnuCaption) - 1)
					End If

					itm.Caption = m_Translate.GetSafeTranslationValue(mnuCaption)
					itm.Name = myValue(1).ToString

					If startAsGroup Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)
					AddHandler itm.ItemClick, AddressOf GetExportMenuItem
				End If
			Next

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		End Try

	End Sub


	'Try
	'	Dim barButtonItem As DevExpress.XtraBars.BarButtonItem
	'	Dim popupMenu As New DevExpress.XtraBars.PopupMenu

	'	Me.bbiExport.Manager = Me.BarManager1
	'	Me.bbiExport.Manager.ForceInitialize()
	'	Me.bbiExport.ActAsDropDown = False
	'	Me.bbiExport.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
	'	Me.bbiExport.DropDownEnabled = True
	'	Me.bbiExport.DropDownControl = popupMenu
	'	Me.bbiExport.Enabled = True

	'	popupMenu.Manager = BarManager1

	'	' CSV Export
	'	barButtonItem = New DevExpress.XtraBars.BarButtonItem()
	'	barButtonItem.Caption = m_Translate.GetSafeTranslationValue("Daten in CSV- / TXT exportieren")
	'	'barButtonItem.Name = "bbiCsvExport"
	'	'barButtonItem.Tag = New String() {"CSV", ""}
	'	'popupMenu.AddItem(barButtonItem).BeginGroup = True
	'	'AddHandler barButtonItem.ItemClick, AddressOf GetExportMenuItem

	'	' XML Export VD
	'	If Cbo_QSTListeKanton.Text = "VD" OrElse Cbo_QSTListeKanton.Text = "GE" Then
	'		'barButtonItem = New DevExpress.XtraBars.BarButtonItem()
	'		barButtonItem.Caption = String.Format(m_Translate.GetSafeTranslationValue("Daten für Kanton {0} exportieren"), Cbo_QSTListeKanton.Text)
	'		'barButtonItem.Name = "bbiXmlExportVd"
	'		'barButtonItem.Tag = New String() {"XML", "VD"}
	'		'popupMenu.AddItem(barButtonItem)
	'		'AddHandler barButtonItem.ItemClick, AddressOf GetExportMenuItem
	'	End If

	'	popupMenu.AddItem(barButtonItem)
	'	AddHandler barButtonItem.ItemClick, AddressOf GetExportMenuItem

	'	'' XML Export GE
	'	'If Me.Cbo_QSTListeKanton.Text = "GE" Then
	'	'	barButtonItem = New DevExpress.XtraBars.BarButtonItem()
	'	'	barButtonItem.Caption = m_Translate.GetSafeTranslationValue("Daten für Kanton GE exportieren")
	'	'	barButtonItem.Name = "bbiXmlExportGe"
	'	'	barButtonItem.Tag = New String() {"XML", "GE"}
	'	'	popupMenu.AddItem(barButtonItem)
	'	'	AddHandler barButtonItem.ItemClick, AddressOf GetExportMenuItem
	'	'End If

	'Catch ex As Exception
	'	m_Logger.LogError(String.Format("{0}", ex.ToString))

	'End Try


	Sub GetExportMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
		Dim strSQL As String = Me.txt_SQLQuery.Text

		Select Case e.Item.Name.ToUpper
			Case "TXT".ToUpper, "CSV".ToUpper
				StartCSVExportModul()

			Case "XML".ToUpper
				StartXMLExportModul()

			Case "eQuest".ToUpper
				StarteQuestExportModul()

		End Select

	End Sub

	Sub StartCSVExportModul()
		Dim _Setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = ClsDataDetail.m_InitialData.MDData.MDDbConn, .SQL2Open = Me.txt_SQLQuery.Text, .ModulName = "QSTTOCSV"}
		_Setting.SelectedMDNr = lueMandant.EditValue
		_Setting.LogedUSNr = m_InitializationData.UserData.UserNr

		Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_Setting)

		obj.ExportCSVFromQSTListing(Me.txt_SQLQuery.Text)

	End Sub

	Sub StartXMLExportModul()

		Dim qstArt As String = String.Empty
		If Not String.IsNullOrWhiteSpace(m_SearchCriteriums.XMLListingArt) Then qstArt = m_SearchCriteriums.XMLListingArt  ' DirectCast(Me.lueXMLArt.SelectedItem, ComboValue).ComboValue

		Dim frmExport As New frmCSV(ClsDataDetail.m_InitialData)

		frmExport.AssignedSQLQuery = txt_SQLQuery.EditValue
		frmExport.Canton = lueCanton.Text
		frmExport.ListArt = qstArt
		frmExport.EmptyDeclaration = Chk_LeereDeklaration.Checked
		If lueCanton.Text = "GE" Then
			frmExport.CallerModulName = frmCSV.CallerModulNum.XMLGENEVE
		ElseIf lueCanton.Text = "VD" Then
			frmExport.CallerModulName = frmCSV.CallerModulNum.XMLVAUD
		End If

		If Not frmExport.LoadExporterData Then Return
		frmExport.Show()
		frmExport.BringToFront()

	End Sub

	Sub StarteQuestExportModul()

		Dim qstArt As String = String.Empty
		If Not String.IsNullOrWhiteSpace(m_SearchCriteriums.XMLListingArt) Then qstArt = m_SearchCriteriums.XMLListingArt  ' DirectCast(Me.lueXMLArt.SelectedItem, ComboValue).ComboValue

		Dim frmExport As New frmCSV(m_InitializationData)

		frmExport.AssignedSQLQuery = txt_SQLQuery.EditValue
		frmExport.Canton = lueCanton.Text
		frmExport.ListArt = qstArt
		frmExport.EmptyDeclaration = Chk_LeereDeklaration.Checked
		frmExport.CallerModulName = frmCSV.CallerModulNum.EQUEST
		frmExport.LoadedQSTData = m_LoadedQSTData

		If Not frmExport.LoadExporterData Then Return
		frmExport.Show()
		frmExport.BringToFront()

	End Sub

	''' <summary>
	''' KeyPressEvent der Controls auffangen und verarbeiten. (Enter --> Tab)
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub KeyPressEvent(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles Chk_QSTListeNurErstenES.KeyPress, Chk_QSTListeNullBetrag.KeyPress, Cbo_Periode.KeyPress, txt_MANR.KeyPress

		Try
			If e.KeyChar = Chr(13) Then ' Enter
				SendKeys.Send("{tab}")
				e.Handled = True
			End If

		Catch ex As Exception
			MessageBox.Show(ex.Message, "KeyPress", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try

	End Sub

	Private Sub Cbo_QSTListePeriode_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_Periode.SelectedIndexChanged

		If m_SuppressUIEvents Then Return

		Try
			Dim supressUIState = m_SuppressUIEvents
			m_SuppressUIEvents = True

			Dim dauer As Integer = 0
			Dim datum As Date = Date.Now
			Dim index1 As Integer = datum.Month - 1
			Dim bSetSelectedDate As Boolean = True
			Me.lueMonthTo.Visible = False
			Me.SwitchButton1.Value = False

			Select Case Me.Cbo_Periode.SelectedIndex
				Case 0
					bSetSelectedDate = False

				Case 1 ' "DM"
					index1 = datum.Month - 1
					dauer = 0
				Case 2 ' "LM"
					'If datum.Month = 1 Then
					'	datum = datum.AddMonths(-1)
					'	index1 = datum.Month
					'Else
					datum = datum.AddMonths(-1)
					index1 = datum.Month - 1
					'End If

					dauer = 0

				Case 3 '"1Q"
					index1 = 0
					dauer = 2
				Case 4 ' "2Q"
					index1 = 3
					dauer = 2
				Case 5 ' "3Q"
					index1 = 6
					dauer = 2
				Case 6 ' "4Q"
					index1 = 9
					dauer = 2
				Case 7
					index1 = 0
					dauer = 11
					datum = datum.AddYears(0)

				Case 8
					index1 = 0
					dauer = 11
					datum = datum.AddYears(-1)

			End Select

			If bSetSelectedDate Then
				Dim fromValue As Integer = 0
				Dim toValue As Integer = 0

				Me.SwitchButton1.Value = True
				Me.lueYear.EditValue = datum.Year
				LoadQSTMonthData()

				Dim wrappedValues = CType(lueMonthFrom.Properties.DataSource, List(Of IntegerValueViewWrapper))
				If wrappedValues Is Nothing Then Return
				Dim data = From itm In wrappedValues Where itm.Value = (index1 + 1)
				If Not data.Any() Then
					fromValue = wrappedValues(0).Value
				Else
					fromValue = index1 + 1
				End If
				Me.lueMonthFrom.EditValue = fromValue

				wrappedValues = CType(lueMonthTo.Properties.DataSource, List(Of IntegerValueViewWrapper))
				If wrappedValues Is Nothing Then Return
				data = From itm In wrappedValues Where itm.Value = (index1 + 1 + dauer)
				If Not data.Any() Then
					toValue = wrappedValues(wrappedValues.Count - 1).Value
				Else
					toValue = index1 + 1 + dauer
				End If
				Me.lueMonthTo.EditValue = toValue

				Me.lueMonthTo.Visible = True

				LoadQSTLAData()

			Else
				SetPreSelectedData()

			End If

			m_SuppressUIEvents = supressUIState


		Catch ex As Exception ' Manager
			m_Logger.LogError(String.Format("{0}", ex.ToString))
		End Try

	End Sub

	Private Sub FillDefaultDates()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Dim datum As Date = Date.Now
			If datum.Day < 11 Then
				datum = datum.AddMonths(-1)
			End If
			' NEU: Nur Von-Monat und Von-Jahr vorbelegen. Bis-Monat und Bis-Jahr bleibt leer.
			Me.lueMonthFrom.EditValue = datum.Month
			Me.lueMonthTo.EditValue = Nothing
			Me.lueYear.EditValue = datum.Year

		Catch ex As Exception ' Manager
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
		End Try


	End Sub

	Private Sub txt_MANr_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txt_MANR.ButtonClick
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim frmTest As New frmSearchRec("MA-Nr.")

		Dim m As String

		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iQSTListeValue(ClsDataDetail.strQSTListeData)
		Me.txt_MANR.Text = If(m = Nothing, String.Empty, CStr(m.ToString.Replace(strValueSeprator, ",")))
		frmTest.Dispose()

	End Sub

	Private Sub SwitchButton1_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton1.ValueChanged
		Me.lueMonthTo.Visible = Me.SwitchButton1.Value
		Me.lueMonthTo.Text = String.Empty
	End Sub

	Private Sub chk_QSTJustFranz_CheckedChanged(sender As Object, e As EventArgs) Handles chk_QSTJustFranz.CheckedChanged

		If Me.chk_QSTJustFranz.Checked Then
			Me.chk_QSTFranz.Checked = Not chk_QSTJustFranz.Checked
		End If

	End Sub

	Private Sub chk_QSTFranz_CheckedChanged(sender As Object, e As EventArgs) Handles chk_QSTFranz.CheckedChanged

		If Me.chk_QSTJustFranz.Checked Then
			Me.chk_QSTJustFranz.Checked = Not chk_QSTFranz.Checked
		End If

	End Sub


#Region "Helpers"

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


	Private Sub OnDropDown_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

		Const ID_OF_DELETE_BUTTON As Int32 = 1

		If e.Button.Index = ID_OF_DELETE_BUTTON Then

			Try

				If TypeOf sender Is DevExpress.XtraEditors.LookUpEdit Then
					Dim lookupEdit As DevExpress.XtraEditors.LookUpEdit = CType(sender, DevExpress.XtraEditors.LookUpEdit)
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

			Catch ex As Exception

			End Try

		End If

	End Sub

	Private Function SetErrorIfInvalid(ByVal control As Control, ByVal errorProvider As DXErrorProvider, ByVal invalid As Boolean, ByVal errorText As String) As Boolean

		If (invalid) Then
			errorProvider.SetError(control, errorText)
		Else
			errorProvider.SetError(control, String.Empty)
		End If

		Return Not invalid

	End Function


#End Region


	Class IntegerValueViewWrapper
		Public Property Value As Integer
	End Class

	Class ComboboxValue
		Public Property DisplayLable As String
		Public Property Value As String
	End Class

End Class
