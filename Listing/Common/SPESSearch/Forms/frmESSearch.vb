
Option Strict Off

Imports SP.Infrastructure.UI.UtilityUI

Imports System.Reflection.Assembly
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports System.Runtime.CompilerServices
Imports DevExpress.XtraEditors.Controls
Imports System.Threading

Imports SPProgUtility.SPUserSec.ClsUserSec
Imports DevComponents.DotNetBar.Metro.ColorTables
Imports SPProgUtility.ColorUtility.ClsColorUtility

Imports SP.Infrastructure.Logging
Imports DevComponents.DotNetBar
Imports DevExpress.LookAndFeel

Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports SPESSearch.ClsDataDetail
Imports SPS.Listing.Print.Utility
Imports SP.Internal.Automations.BaseTable
Imports System.ComponentModel
Imports SP.Internal.Automations
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Listing.DataObjects
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess

Public Class frmESSearch
	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Protected m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	Private m_ConnectionString As String

	''' <summary>
	''' The data access object.
	''' </summary>
	Private m_CommonDatabaseAccess As ICommonDatabaseAccess
	Private m_ListingDatabaseAccess As IListingDatabaseAccess

	Private _ClsFunc As New ClsDivFunc

	Private _ClsReg As New SPProgUtility.ClsDivReg
	Private strValueSeprator As String = "#@"

	Private strLastSortBez As String
	Public Shared frmMyLV As frmESSearch_LV

	Private bKeyUpEventOnWork As Boolean
	Private m_md As Mandant

	Private Property ShortSQLQuery As String
	Private Property GetMDDbName As String
	Private Property GetMDGuid As String

	Private Property PrintJobNr As String
	Private Property SQL4Print As String
	Private Property bPrintAsDesign As Boolean

	Public Property MetroForeColor As System.Drawing.Color
	Public Property MetroBorderColor As System.Drawing.Color

	Private m_UtilityUI As SP.Infrastructure.UI.UtilityUI
	Private m_AllowedDesign As Boolean

	Private m_BaseTableData As BaseTable.SPSBaseTables
	Private m_PermissionData As BindingList(Of SP.Internal.Automations.PermissionData)
	Private m_CountryData As BindingList(Of SP.Internal.Automations.CVLBaseTableViewData)
	Private m_TaxData As BindingList(Of SP.Internal.Automations.TaxCodeData)

#Region "Constructor"

	Public Sub New()

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ClsDataDetail.TranslationData, ClsDataDetail.ProsonalizedData, ClsDataDetail.MDData, ClsDataDetail.UserData)
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)
		m_md = New Mandant
		m_UtilityUI = New SP.Infrastructure.UI.UtilityUI

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_ConnectionString = m_InitializationData.MDData.MDDbConn
		m_CommonDatabaseAccess = New CommonDatabaseAccess(m_ConnectionString, m_InitializationData.UserData.UserLanguage)
		m_ListingDatabaseAccess = New ListingDatabaseAccess(m_ConnectionString, m_InitializationData.UserData.UserLanguage)

		m_BaseTableData = New SPSBaseTables(m_InitializationData)
		m_PermissionData = m_BaseTableData.PerformPermissionDataOverWebService(m_InitializationData.UserData.UserLanguage)
		m_BaseTableData.BaseTableName = "Country"
		m_CountryData = m_BaseTableData.PerformCVLBaseTablelistWebserviceCall()
		m_TaxData = m_BaseTableData.PerformTaxCodeDataOverWebService(m_InitializationData.UserData.UserLanguage)


		Reset()

		LoadDropStaticDropDown()

		Dim liColor As List(Of Color) = GetMetroColor("INFO")    ' Color.White |  Color.Orange

		If liColor.Count < 1 Then liColor = New List(Of Color)(New Color() {Color.White, Color.Orange})
		Me.MetroForeColor = liColor(0)
		Me.MetroBorderColor = liColor(1)

		m_AllowedDesign = IsUserActionAllowed(ClsDataDetail.UserData.UserNr, 613, ClsDataDetail.MDData.MDNr)

		AddHandler lueCountry.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueNationality.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler luePermissionCode.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler luePermissionCode.QueryPopUp, AddressOf checkedComboBoxEdit1_QueryPopUp

		AddHandler lueQSTCode.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueQSTCode.QueryPopUp, AddressOf checkedComboBoxEdit1_QueryPopUp

		AddHandler lueCivilstate.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueTaxCanton.ButtonClick, AddressOf OnDropDown_ButtonClick

	End Sub

#End Region


#Region "Lookup Edit Reset und Load..."

	Private Sub Reset()

		ResetMandantenDropDown()
		ResetEmploymentKSTDropDown()
		ResetCountryDropDown()
		ResetNationalityDropDown()
		ResetPermissionCodeDropDown()
		ResetQSTCodeDropDown()
		ResetCivilstateDropDown()
		ResetTaxCantonDropDown()

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

	Private Sub ResetPermissionCodeDropDown()

		luePermissionCode.Properties.Items.Clear()

		luePermissionCode.Properties.DisplayMember = "PermissionCodeViewData"
		luePermissionCode.Properties.ValueMember = "RecValue"

		luePermissionCode.Properties.DropDownRows = 20
		luePermissionCode.Properties.ForceUpdateEditValue = DevExpress.Utils.DefaultBoolean.True

		luePermissionCode.Properties.PopupSizeable = True
		luePermissionCode.Properties.NullText = String.Empty
		luePermissionCode.EditValue = Nothing

	End Sub

	Private Sub ResetQSTCodeDropDown()

		lueQSTCode.Properties.Items.Clear()

		lueQSTCode.Properties.DisplayMember = "QSTCodeViewData"
		lueQSTCode.Properties.ValueMember = "QSTCode"

		lueQSTCode.Properties.DropDownRows = 10
		lueQSTCode.Properties.ForceUpdateEditValue = DevExpress.Utils.DefaultBoolean.True
		lueQSTCode.Properties.PopupSizeable = True

		lueQSTCode.Properties.NullText = String.Empty
		lueQSTCode.EditValue = Nothing

	End Sub

	Private Sub ResetCivilstateDropDown()

		lueCivilstate.Properties.Items.Clear()

		lueCivilstate.Properties.DisplayMember = "CivilStateViewData"
		lueCivilstate.Properties.ValueMember = "GetField"

		lueCivilstate.Properties.DropDownRows = 10
		lueCivilstate.Properties.ForceUpdateEditValue = DevExpress.Utils.DefaultBoolean.True

		lueCivilstate.Properties.NullText = String.Empty
		lueCivilstate.EditValue = Nothing

	End Sub

	Private Sub ResetTaxCantonDropDown()

		lueTaxCanton.Properties.Items.Clear()

		lueTaxCanton.Properties.DisplayMember = "CantonViewData"
		lueTaxCanton.Properties.ValueMember = "GetField"

		lueTaxCanton.Properties.DropDownRows = 10
		lueTaxCanton.Properties.ForceUpdateEditValue = DevExpress.Utils.DefaultBoolean.True

		lueTaxCanton.Properties.NullText = String.Empty
		lueTaxCanton.EditValue = Nothing

	End Sub

	Private Sub ResetEmploymentKSTDropDown()

		cboEmploymentKST.Properties.Items.Clear()

		cboEmploymentKST.Properties.DisplayMember = "CostcenterLabel"
		cboEmploymentKST.Properties.ValueMember = "CostcenterLabel"

		cboEmploymentKST.EditValue = Nothing

	End Sub

	Private Sub OnlueMandant_EditValueChanged(sender As Object, e As System.EventArgs)
		Dim bSetToDefault As Boolean = False

		If String.IsNullOrWhiteSpace(Me.lueMandant.Properties.GetCheckedItems) Then Return

		If ClsDataDetail.MDData.MultiMD = 0 AndAlso Me.lueMandant.EditValue.ToString.Contains(",") Then
			m_UtilityUI.ShowInfoDialog("Es kann nur aus einer Mandant gesucht werden. Ich wähle den Hauptmandant.")

			bSetToDefault = True

		End If
		If Me.lueMandant.EditValue.ToString.Contains(",") Then bSetToDefault = True

		If Not bSetToDefault Then
			Dim SelectedData = lueMandant.Properties.GetItems.GetCheckedValues(0)

			If Not SelectedData Is Nothing Then
				ClsDataDetail.MDData = ClsDataDetail.SelectedMDData(Me.lueMandant.EditValue)
				ClsDataDetail.UserData = ClsDataDetail.LogededUSData(Me.lueMandant.EditValue, ClsDataDetail.UserData.UserNr)

				bSetToDefault = False

			Else
				bSetToDefault = True

			End If

		Else
			ClsDataDetail.MDData = ClsDataDetail.SelectedMDData(0)
			ClsDataDetail.UserData = ClsDataDetail.LogededUSData(0, ClsDataDetail.UserData.UserNr)

		End If
		ClsDataDetail.m_InitialData = New SP.Infrastructure.Initialization.InitializeClass(ClsDataDetail.TranslationData, ClsDataDetail.ProsonalizedData, ClsDataDetail.MDData, ClsDataDetail.UserData)
		m_InitializationData = ClsDataDetail.m_InitialData
		LoadDropStaticDropDown()


		Me.bbiSearch.Enabled = Not (ClsDataDetail.MDData Is Nothing)
		Me.bbiClear.Enabled = Not (ClsDataDetail.MDData Is Nothing)
		Me.bbiPrint.Enabled = Not (ClsDataDetail.MDData Is Nothing)
		Me.bbiExport.Enabled = Not (ClsDataDetail.MDData Is Nothing)

	End Sub


#End Region


#Region "Dropdown Funktionen Allgmeine"

	Private Sub CboSort_QueryPopUp(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CboSort.QueryPopUp
		If Me.CboSort.Properties.Items.Count = 0 Then ListSort(CboSort)
	End Sub

	Private Sub Cbo_ESKST1_QueryPopUp(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_ESKST1.QueryPopUp
		If Me.Cbo_ESKST1.Properties.Items.Count = 0 Then ListESKST1(Me.Cbo_ESKST1)
	End Sub

	Private Sub Cbo_ESKST2_QueryPopUp(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_ESKST2.QueryPopUp
		If Me.Cbo_ESKST2.Properties.Items.Count = 0 Then ListESKST2(Me.Cbo_ESKST2)
	End Sub

	Private Sub Cbo_ESBerater_QueryPopUp(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_ESBerater.QueryPopUp
		ListBerater(Me.Cbo_ESBerater, Me.Cbo_ESFiliale.Text)
	End Sub

	Private Sub Cbo_ESSuvaCode_QueryPopUp(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_ESSuvaCode.QueryPopUp
		If Me.Cbo_ESSuvaCode.Properties.Items.Count = 0 Then ListSuva(Me.Cbo_ESSuvaCode)
	End Sub

	Private Sub OnChkESNurAktiveEinsaetze_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkESNurAktiveEinsaetze.CheckedChanged
		Dim toggle As Boolean = Not ChkESNurAktiveEinsaetze.Checked

		Cbo_ESAktivImSelektion.Enabled = toggle
		deAktiv_1.Enabled = toggle
		deAktiv_2.Enabled = toggle

		Cbo_ESAktivImSelektion.SelectedIndex = -1
		If toggle Then
			deAktiv_1.EditValue = Nothing
			deAktiv_2.EditValue = Nothing
		Else
			deAktiv_1.EditValue = Date.Now.ToString("dd.MM.yyyy")
			deAktiv_2.EditValue = Date.Now.ToString("dd.MM.yyyy")
			'Cbo_ESAktivImSelektion.SelectedIndex = -1
		End If
	End Sub

	Private Sub Cbo_ESFiliale_QueryPopUp(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_ESFiliale.QueryPopUp
		If Me.Cbo_ESBerater.Text.Length > 0 Then
			ListUSFilialen(Me.Cbo_ESFiliale, Me.Cbo_ESBerater.Text)
		Else
			ListUSFilialen(Me.Cbo_ESFiliale)
		End If
	End Sub

	Private Sub LoadDropStaticDropDown()

		LoadEmploymentCostcenterDropDown()
		LoadCountryDropDown()
		LoadNationalityDropDown()
		LoadPermissionCodeDropDown()
		LoadTaxCodeDropDown()
		LoadCivilstateCodeDropDown()
		LoadTaxCantonDropDown()

	End Sub

	Private Function LoadEmploymentCostcenterDropDown() As Boolean
		Dim result As Boolean = True

		Dim data = m_ListingDatabaseAccess.LoadCustomerKSTForEmploymentData(m_InitializationData.MDData.MDNr)
		If data Is Nothing Then
			Dim msg As String = "Einsatz Kunden-Kostenstellen Daten konnten nicht geladen werden."
			msg = m_Translate.GetSafeTranslationValue(msg)

			m_UtilityUI.ShowErrorDialog(msg)

			Return False
		End If

		cboEmploymentKST.Properties.DataSource = data

		Return Not data Is Nothing
	End Function

	Private Function LoadCountryDropDown() As Boolean
		Dim result As Boolean = True

		If lueMandant.EditValue Is Nothing Then Return True
		Dim data = m_ListingDatabaseAccess.LoadCountryForEmploymentData(m_InitializationData.MDData.MDNr)
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
		Dim data = m_ListingDatabaseAccess.LoadNationalityForEmploymentData(m_InitializationData.MDData.MDNr)
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
		Dim data = m_ListingDatabaseAccess.LoadPermissionForEmploymentData(m_InitializationData.MDData.MDNr)
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
		Dim data = m_ListingDatabaseAccess.LoadTaxForEmploymentData(m_InitializationData.MDData.MDNr)
		If data Is Nothing Then
			Dim msg As String = "Quellensteuer-Tarif Daten konnten nicht geladen werden."
			msg = m_Translate.GetSafeTranslationValue(msg)

			m_UtilityUI.ShowErrorDialog(msg)

			Return False
		End If
		Dim listData = New BindingList(Of Listing.DataObjects.QSTCodeData)

		For Each itm In data
			Dim code = New QSTCodeData


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

	Private Function LoadCivilstateCodeDropDown() As Boolean
		Dim result As Boolean = True

		If lueMandant.EditValue Is Nothing Then Return True
		Dim data = m_ListingDatabaseAccess.LoadCivilstateForEmploymentData(m_InitializationData.MDData.MDNr)
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

	Private Function LoadTaxCantonDropDown() As Boolean
		Dim result As Boolean = True

		If lueMandant.EditValue Is Nothing Then Return True
		Dim data = m_ListingDatabaseAccess.LoadTaxCantonForEmploymentData(m_InitializationData.MDData.MDNr)
		If data Is Nothing Then
			Dim msg As String = "Quellensteuer-Kanton Daten konnten nicht geladen werden."
			msg = m_Translate.GetSafeTranslationValue(msg)

			m_UtilityUI.ShowErrorDialog(msg)

			Return False
		End If

		lueTaxCanton.Properties.DataSource = data
		lueTaxCanton.Properties.DropDownRows = Math.Min(20, data.Count + 1)
		lueTaxCanton.Enabled = data.Count > 0


		Return Not data Is Nothing

	End Function


#End Region


#Region "Click Event Sonstiges"

	''' <summary>
	''' Selektionsfenster für die Branchen öffnen
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub LnkESBranche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LibESBranche.Click
		Dim frmTest As New frmSearchRec("Einsatz-Branche")

		Dim m As String
		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iESValue(_ClsFunc.GetSelektion)
		If m.ToString <> String.Empty Then
			Dim strEintrag As String() = Regex.Split(m.ToString, strValueSeprator)
			For i As Integer = 0 To strEintrag.Count - 1
				If Not Me.Lst_ESBranche.Items.Contains(strEintrag(i)) Then
					Me.Lst_ESBranche.Items.Add(strEintrag(i))
				End If
			Next
		End If
		frmTest.Dispose()

	End Sub

#End Region


#Region "DropDown Funktionen Sonstiges"

	Private Sub FillUnterschrieben(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_ESVerleihUntersch.QueryPopUp, Cbo_ESESVertragUntersch.QueryPopUp

		Dim Coll As ComboBoxItemCollection = sender.Properties.Items
		sender.Properties.Items.clear()
		Coll.BeginUpdate()
		Try
			Coll.Add(New ComboValue("", ""))
			Coll.Add(New ComboValue(m_Translate.GetSafeTranslationValue("nicht unterschrieben"), 0))
			Coll.Add(New ComboValue(m_Translate.GetSafeTranslationValue("unterschrieben"), 1))
		Finally
			Coll.EndUpdate()
		End Try

	End Sub

	Private Sub Oncbo_ESAuflistenQueryPopUp(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_ESVerleihUntersch.QueryPopUp, cbo_ESAuflisten.QueryPopUp

		Dim Coll As ComboBoxItemCollection = sender.Properties.Items
		sender.Properties.Items.clear()
		Coll.BeginUpdate()
		Try
			Coll.Add(New ComboValue("", ""))
			Coll.Add(New ComboValue(m_Translate.GetSafeTranslationValue("nicht aktiviert"), 0))
			Coll.Add(New ComboValue(m_Translate.GetSafeTranslationValue("aktiviert"), 1))
		Finally
			Coll.EndUpdate()
		End Try

	End Sub

	' Dropdownlistbox gedruckt / nicht gedruckt
	Private Sub FillGedruckt(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_ESEinsatzvertragGedruckt.QueryPopUp, Cbo_ESVerleihvertragGedruckt.QueryPopUp

		If TypeOf (sender) Is DevExpress.XtraEditors.ComboBoxEdit Then
			Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = DirectCast(sender, DevExpress.XtraEditors.ComboBoxEdit)
			If cbo.Properties.Items.Count = 0 Then
				cbo.Properties.Items.Add(New ComboValue("", ""))
				cbo.Properties.Items.Add(New ComboValue(m_Translate.GetSafeTranslationValue("nicht gedruckt"), "0"))
				cbo.Properties.Items.Add(New ComboValue(m_Translate.GetSafeTranslationValue("gedruckt"), "1"))
			End If
		End If
	End Sub
	' Gruppe 1
	Private Sub Cbo_ESGruppe1_QueryPopUp(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_ESGruppe1.QueryPopUp
		If Me.Cbo_ESGruppe1.Properties.Items.Count = 0 Then ListGruppe1(Me.Cbo_ESGruppe1)
	End Sub
	' GAV-Kanton
	Private Sub Cbo_ESGAVKanton_QueryPopUp(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_ESGAVKanton.QueryPopUp
		If Me.Cbo_ESGAVKanton.Properties.Items.Count = 0 Then ListGAVKanton(Me.Cbo_ESGAVKanton)
	End Sub
	' Einstufung
	Private Sub Cbo_ESEinstufung_QueryPopUp(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_ESEinstufung.QueryPopUp
		If Me.Cbo_ESEinstufung.Properties.Items.Count = 0 Then ListEinstufung(Me.Cbo_ESEinstufung)
	End Sub
	' Unterzeichner
	Private Sub Cbo_ESUnterzeichner_QueryPopUp(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_ESUnterzeichner.QueryPopUp
		If Me.Cbo_ESUnterzeichner.Properties.Items.Count = 0 Then ListESUnterzeichner(Me.Cbo_ESUnterzeichner)
	End Sub

#End Region


#Region "DropDown Funktionen Kandidaten"

	Private Sub cbo_MAKontaktArten_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cbo_MAKontaktArten.QueryPopUp
		ListKontaktArten(Me.cbo_MAKontaktArten)
	End Sub

	Private Sub OnMAALK_QueryPopUp(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MAAHVVollstaendig.QueryPopUp, cboALKKasse.QueryPopUp

		ListEmployeeALKKasse(cboALKKasse)

	End Sub

	' 1 - Ja // 0 - Nein
	Private Sub FillCboJaNeinK(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MABankverbindung.QueryPopUp

		If TypeOf (sender) Is DevExpress.XtraEditors.ComboBoxEdit Then
			Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = DirectCast(sender, DevExpress.XtraEditors.ComboBoxEdit)
			If cbo.Properties.Items.Count = 0 Then ListCboJaNeinK(cbo)
		End If

	End Sub

	' Kontakt
	Private Sub Cbo_MAKontakt_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MAKontakt.QueryPopUp
		If Cbo_MAKontakt.Properties.Items.Count = 0 Then ListMAKontakt(Cbo_MAKontakt)
	End Sub

	Private Sub Cbo_MAStatus1_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MAStatus1.QueryPopUp
		If Cbo_MAStatus1.Properties.Items.Count = 0 Then ListMAStatus1(Cbo_MAStatus1)
	End Sub

	Private Sub Cbo_MAStatus2_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MAStatus2.QueryPopUp
		If Cbo_MAStatus2.Properties.Items.Count = 0 Then ListMAStatus2(Cbo_MAStatus2)
	End Sub


#End Region


#Region "DropDown Funktionen Kunden"
	Private Sub Cbo_KDKanton_QueryPopUp(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_KDKanton.QueryPopUp
		If Me.Cbo_KDKanton.Properties.Items.Count = 0 Then ListKDKanton(Me.Cbo_KDKanton)
	End Sub
	Private Sub Cbo_KDSprache_QueryPopUp(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_KDSprache.QueryPopUp
		If Me.Cbo_KDSprache.Properties.Items.Count = 0 Then ListKDSprachen(Me.Cbo_KDSprache)
	End Sub


#End Region


#Region "Allgemeine Funktionen"

	' 0 - Nicht aktiviert // 1 - Aktiviert
	Private Sub FillCboAktiviertNichtAktviert(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MARapporteDrucken.QueryPopUp,
		Cbo_KDImWeb.QueryPopUp,
		Cbo_KDEinsatzSperren.QueryPopUp,
		Cbo_KDRapporteDrucken.QueryPopUp,
		Cbo_KDKreditwarnung.QueryPopUp, Cbo_MAZwischenverdienst.QueryPopUp

		If TypeOf (sender) Is DevExpress.XtraEditors.ComboBoxEdit Then
			Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = DirectCast(sender, DevExpress.XtraEditors.ComboBoxEdit)
			If cbo.Properties.Items.Count = 0 Then ListCboAktiviertNichtAktiviert(cbo)
		End If

	End Sub

	' Ja - Nein
	Private Sub FillJaNein(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MALohnSperren.QueryPopUp,
		Cbo_MAVorschussSperren.QueryPopUp,
		Cbo_MANewAHVVollstaendig.QueryPopUp,
		Cbo_MAAHVVollstaendig.QueryPopUp,
		Cbo_MAWOS.QueryPopUp,
		Cbo_MAWeb.QueryPopUp,
		cbo_MANotTelefon.QueryPopUp,
		cbo_MANotNatel.QueryPopUp,
		cbo_MANotEMail.QueryPopUp

		If TypeOf (sender) Is DevExpress.XtraEditors.ComboBoxEdit Then
			Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = DirectCast(sender, DevExpress.XtraEditors.ComboBoxEdit)
			If cbo.Properties.Items.Count = 0 Then ListJaNein(cbo)
		End If

	End Sub

	Private Sub FillVorhanden(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MABankverbindung.QueryPopUp,
		Cbo_MAQualifikationNachweis.QueryPopUp

		If TypeOf (sender) Is DevExpress.XtraEditors.ComboBoxEdit Then
			Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = DirectCast(sender, DevExpress.XtraEditors.ComboBoxEdit)
			If cbo.Properties.Items.Count = 0 Then ListVorhanden(cbo)
		End If

	End Sub

#End Region

	Private Sub CmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
		Me.Dispose()
	End Sub

	Private Sub frmESSearch_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

		FormIsLoaded("frmESSearch_LV", True)
		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.frm_Location = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.ifrmWidth = Me.Width
				My.Settings.ifrmHeight = Me.Height
				My.Settings.SortBez = Me.CboSort.Text

				My.Settings.Save()
			End If

		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub

	Private Sub frmESSearch_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Move
		If FormIsLoaded("frmESSearch_LV", False) Then
			frmMyLV.Location = New Point(Me.Location.X - frmMyLV.Width - 5, Me.Location.Y)
			frmMyLV.TopMost = True
			frmMyLV.TopMost = False
		End If
	End Sub

	Sub StartTranslation()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

		lblHeaderFett.Text = m_Translate.GetSafeTranslationValue(lblHeaderFett.Text)
		lblHeaderNormal.Text = m_Translate.GetSafeTranslationValue(lblHeaderNormal.Text)
		CmdClose.Text = m_Translate.GetSafeTranslationValue(CmdClose.Text)

		xtabAllgemein.Text = m_Translate.GetSafeTranslationValue(xtabAllgemein.Text)
		xtabSonstiges.Text = m_Translate.GetSafeTranslationValue(xtabSonstiges.Text)
		xtabMA.Text = m_Translate.GetSafeTranslationValue(xtabMA.Text)
		xtabKD.Text = m_Translate.GetSafeTranslationValue(xtabKD.Text)
		xtabErweitert.Text = m_Translate.GetSafeTranslationValue(xtabErweitert.Text)
		xtabSQLAbfrage.Text = m_Translate.GetSafeTranslationValue(xtabSQLAbfrage.Text)
		xtabAdmin.Text = m_Translate.GetSafeTranslationValue(xtabAdmin.Text)

		lblMDName.Text = m_Translate.GetSafeTranslationValue(lblMDName.Text)
		lblSortieren.Text = m_Translate.GetSafeTranslationValue(lblSortieren.Text)
		lblNummer.Text = m_Translate.GetSafeTranslationValue(lblNummer.Text)
		lblKandidatennummer.Text = m_Translate.GetSafeTranslationValue(lblKandidatennummer.Text)
		lblKundennummer.Text = m_Translate.GetSafeTranslationValue(lblKundennummer.Text)
		ChkESNurAktiveEinsaetze.Text = m_Translate.GetSafeTranslationValue(ChkESNurAktiveEinsaetze.Text)
		lblAktiv.Text = m_Translate.GetSafeTranslationValue(lblAktiv.Text)
		lblAktivzwischen.Text = m_Translate.GetSafeTranslationValue(lblAktivzwischen.Text)
		lblErfasstzwischen.Text = m_Translate.GetSafeTranslationValue(lblErfasstzwischen.Text)
		lblKostenstelle.Text = m_Translate.GetSafeTranslationValue(lblKostenstelle.Text)
		lbl1KST.Text = m_Translate.GetSafeTranslationValue(lbl1KST.Text, True)
		lbl2KST.Text = m_Translate.GetSafeTranslationValue(lbl2KST.Text, True)
		lblBerater.Text = m_Translate.GetSafeTranslationValue(lblBerater.Text, True)
		lblSuvaCode.Text = m_Translate.GetSafeTranslationValue(lblSuvaCode.Text)
		lblFiliale.Text = m_Translate.GetSafeTranslationValue(lblFiliale.Text)

		xtabMAKomm.Text = m_Translate.GetSafeTranslationValue(xtabMAKomm.Text)
		xtabMAAllgemein.Text = m_Translate.GetSafeTranslationValue(xtabMAAllgemein.Text)
		xtabZVQstRes.Text = m_Translate.GetSafeTranslationValue(xtabZVQstRes.Text)
		xtabExclude.Text = m_Translate.GetSafeTranslationValue(xtabExclude.Text)

		grpUnterschrieben.Text = m_Translate.GetSafeTranslationValue(grpUnterschrieben.Text)
		lblEinsatzvertrag.Text = m_Translate.GetSafeTranslationValue(lblEinsatzvertrag.Text)
		lblVerleihvertrag.Text = m_Translate.GetSafeTranslationValue(lblVerleihvertrag.Text)

		grpGAVDaten.Text = m_Translate.GetSafeTranslationValue(grpGAVDaten.Text)
		lblGAVKanton.Text = m_Translate.GetSafeTranslationValue(lblGAVKanton.Text)
		LibESGAVBeruf.Text = m_Translate.GetSafeTranslationValue(LibESGAVBeruf.Text)
		lbl1Gruppe.Text = m_Translate.GetSafeTranslationValue(LibESGAVBeruf.Text)

		grpGedruckt.Text = m_Translate.GetSafeTranslationValue(grpGedruckt.Text)
		lblPrintEinsatzvertrag.Text = m_Translate.GetSafeTranslationValue(lblPrintEinsatzvertrag.Text)
		lblPrintVerleihvertrag.Text = m_Translate.GetSafeTranslationValue(lblPrintVerleihvertrag.Text)

		LibESEinsatzAls.Text = m_Translate.GetSafeTranslationValue(LibESEinsatzAls.Text)
		LibESBranche.Text = m_Translate.GetSafeTranslationValue(LibESBranche.Text)

		lblESEinstufung.Text = m_Translate.GetSafeTranslationValue(lblESEinstufung.Text)
		lblUnterzeichner.Text = m_Translate.GetSafeTranslationValue(lblUnterzeichner.Text)
		lblESListauflisten.Text = m_Translate.GetSafeTranslationValue(lblESListauflisten.Text)
		ChkESNurAktiveLohn.Text = m_Translate.GetSafeTranslationValue(ChkESNurAktiveLohn.Text)

		grpAHVNrVollstaendig.Text = m_Translate.GetSafeTranslationValue(grpAHVNrVollstaendig.Text)
		lblAlteAHVNr.Text = m_Translate.GetSafeTranslationValue(lblAlteAHVNr.Text)
		lblNeueAHVNr.Text = m_Translate.GetSafeTranslationValue(lblNeueAHVNr.Text)
		grpSperrung.Text = m_Translate.GetSafeTranslationValue(grpSperrung.Text)
		lblSperrungLohn.Text = m_Translate.GetSafeTranslationValue(lblSperrungLohn.Text)
		lblSperrungVorschuss.Text = m_Translate.GetSafeTranslationValue(lblSperrungVorschuss.Text)
		grpSpracheRapportdruck.Text = m_Translate.GetSafeTranslationValue(grpSpracheRapportdruck.Text)
		lblSprache.Text = m_Translate.GetSafeTranslationValue(lblSprache.Text)
		lblRPNichtdrucken.Text = m_Translate.GetSafeTranslationValue(lblRPNichtdrucken.Text)
		grpUnerschreitungslimite.Text = m_Translate.GetSafeTranslationValue(grpUnerschreitungslimite.Text)
		chkKDKeinKreditlimite.Text = m_Translate.GetSafeTranslationValue(chkKDKeinKreditlimite.Text)
		chkKDKreditlimiteUeberschritten.Text = m_Translate.GetSafeTranslationValue(chkKDKreditlimiteUeberschritten.Text)

		lblWOSUebermitteln.Text = m_Translate.GetSafeTranslationValue(lblWOSUebermitteln.Text)
		lblimWebVeroeffentlichen.Text = m_Translate.GetSafeTranslationValue(lblimWebVeroeffentlichen.Text)
		lblZwischenverdienst.Text = m_Translate.GetSafeTranslationValue(lblZwischenverdienst.Text)
		grpLeereFelder.Text = m_Translate.GetSafeTranslationValue(grpLeereFelder.Text)
		lblLeerTelefon.Text = m_Translate.GetSafeTranslationValue(lblLeerTelefon.Text)
		lblLeerNatel.Text = m_Translate.GetSafeTranslationValue(lblLeerNatel.Text)
		lblLeerEMail.Text = m_Translate.GetSafeTranslationValue(lblLeerEMail.Text)

		grpBewilligung.Text = m_Translate.GetSafeTranslationValue(grpBewilligung.Text)
		lblBewCode.Text = m_Translate.GetSafeTranslationValue(lblBewCode.Text)
		lblBewGueltig.Text = m_Translate.GetSafeTranslationValue(lblBewGueltig.Text)
		chkMABew.Text = m_Translate.GetSafeTranslationValue(chkMABew.Text)
		grpGeburtsdaten.Text = m_Translate.GetSafeTranslationValue(grpGeburtsdaten.Text)
		lblGebZwischen.Text = m_Translate.GetSafeTranslationValue(lblGebZwischen.Text)
		lblGebMonat.Text = m_Translate.GetSafeTranslationValue(lblGebMonat.Text)
		lblBankverbindung.Text = m_Translate.GetSafeTranslationValue(lblBankverbindung.Text)
		lblQualifikationsnachweis.Text = m_Translate.GetSafeTranslationValue(lblQualifikationsnachweis.Text, True)

		lblZivilstand.Text = m_Translate.GetSafeTranslationValue(lblZivilstand.Text)
		lblLand.Text = m_Translate.GetSafeTranslationValue(lblLand.Text)
		lblQSteuer.Text = m_Translate.GetSafeTranslationValue(lblQSteuer.Text)
		lblSKanton.Text = m_Translate.GetSafeTranslationValue(lblSKanton.Text)
		lblNationalitaet.Text = m_Translate.GetSafeTranslationValue(lblNationalitaet.Text)
		lblMAKontakt.Text = m_Translate.GetSafeTranslationValue(lblMAKontakt.Text, True)
		lblMA1Status.Text = m_Translate.GetSafeTranslationValue(lblMA1Status.Text, True)
		lblMA2Status.Text = m_Translate.GetSafeTranslationValue(lblMA2Status.Text, True)

		grpStammDaten.Text = m_Translate.GetSafeTranslationValue(grpStammDaten.Text)
		lblKanton.Text = m_Translate.GetSafeTranslationValue(lblKanton.Text)
		lblKDSprache.Text = m_Translate.GetSafeTranslationValue(lblKDSprache.Text)

		grpVermittlungundVerleih.Text = m_Translate.GetSafeTranslationValue(grpVermittlungundVerleih.Text)
		lblEinsatzspreren.Text = m_Translate.GetSafeTranslationValue(lblEinsatzspreren.Text)
		lblImWeb.Text = m_Translate.GetSafeTranslationValue(lblImWeb.Text)
		grpKredit.Text = m_Translate.GetSafeTranslationValue(grpKredit.Text)
		lblKreditwarnung.Text = m_Translate.GetSafeTranslationValue(lblKreditwarnung.Text)
		LibKDKreditlimite1.Text = m_Translate.GetSafeTranslationValue(LibKDKreditlimite1.Text)
		chkKDKreditlimiteUeberschritten.Text = m_Translate.GetSafeTranslationValue(chkKDKreditlimiteUeberschritten.Text)
		LibKDKreditlimite2.Text = m_Translate.GetSafeTranslationValue(LibKDKreditlimite2.Text)
		lblGueltigzwischen.Text = m_Translate.GetSafeTranslationValue(lblGueltigzwischen.Text)
		chkKDKeinKreditlimite.Text = m_Translate.GetSafeTranslationValue(chkKDKeinKreditlimite.Text)
		lblRPNichtdrucken.Text = m_Translate.GetSafeTranslationValue(lblRPNichtdrucken.Text)

		lblIhreAbfrage.Text = m_Translate.GetSafeTranslationValue(lblIhreAbfrage.Text)
		lblIhrederzeitigeAbfrage.Text = m_Translate.GetSafeTranslationValue(lblIhrederzeitigeAbfrage.Text)
		lblAdminversion.Text = m_Translate.GetSafeTranslationValue(lblAdminversion.Text)


		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)
		Me.bbiSearch.Caption = m_Translate.GetSafeTranslationValue(Me.bbiSearch.Caption)
		Me.bbiClear.Caption = m_Translate.GetSafeTranslationValue(Me.bbiClear.Caption)
		Me.bbiPrint.Caption = m_Translate.GetSafeTranslationValue(Me.bbiPrint.Caption)
		Me.bbiExport.Caption = m_Translate.GetSafeTranslationValue(Me.bbiExport.Caption)

	End Sub

	''' <summary>
	''' Starten von Anwendung.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub OnFormLoad(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		SetDefaultSortValues()

		Dim UpdateDelegate As New MethodInvoker(AddressOf StartTranslation)
		Me.Invoke(UpdateDelegate)

		Try
			Me.KeyPreview = True
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(ClsDataDetail.ProgSettingData.SelectedMDNr, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

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

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.FormStyle: {1}", strMethodeName, ex.ToString))

		End Try

		SetBeraterValues()

		Me.bbiExport.Enabled = False
		Me.bbiPrint.Enabled = False

		' Tab-Stops von LinkButtons müssen programmatisch deaktiviert werden
		Me.LibESBranche.TabStop = False
		Me.LibESEinsatzAls.TabStop = False
		Me.LibESGAVBeruf.TabStop = False
		Me.LibKDKreditlimite1.TabStop = False
		Me.LibKDKreditlimite2.TabStop = False

		Me.xtabESSearch.SelectedTabPage = Me.xtabAllgemein

		If ClsDataDetail.UserData.UserNr <> 1 Then
			Me.xtabESSearch.TabPages.Remove(Me.xtabSQLAbfrage)
			Me.xtabESSearch.TabPages.Remove(Me.xtabErweitert)
			Me.xtabESSearch.TabPages.Remove(Me.xtabAdmin)
		End If

	End Sub

	Sub SetDefaultSortValues()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Try
				Dim strSort As String = My.Settings.SortBez
				If strSort = String.Empty Then strSort = String.Format("5 - {0}", m_Translate.GetSafeTranslationValue("Firmenname"))
				ListSort(Me.CboSort)
				Dim aSortlist As String() = strSort.Split(CChar(","))

				For Each bez As String In aSortlist
					CboSort.Properties.Items(CInt(Val(bez))).CheckState = CheckState.Checked
				Next

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Fill Sortinglist: {1}", strMethodeName, ex.ToString))

			End Try

			Try
				Me.lueMandant.SetEditValue(ClsDataDetail.ProgSettingData.SelectedMDNr)
				Me.lueMandant.Visible = IsUserActionAllowed(ClsDataDetail.ProgSettingData.LogedUSNr, 642, ClsDataDetail.ProgSettingData.SelectedMDNr)
				Me.lblMDName.Visible = Me.lueMandant.Visible

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Mandantenauswahl anzeigen: {1}", strMethodeName, ex.ToString))
				Me.lueMandant.Visible = False
				Me.lblMDName.Visible = False
			End Try

			AddHandler Me.lueMandant.EditValueChanged, AddressOf OnlueMandant_EditValueChanged

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Definition der Sortierreihenfolge. {1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Private Sub frmESSearch_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

		If e.KeyCode = Keys.F12 And ClsDataDetail.UserData.UserNr = 1 Then
			Dim frm As New frmLibraryInfo
			frm.LoadAssemblyData()

			frm.Show()
			frm.BringToFront()
		End If

	End Sub

	Sub SetBeraterValues()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim m_common As New CommonSetting

		' Berechtigung Fililale/Kostenstelle wählen
		If Not IsUserActionAllowed(ClsDataDetail.UserData.UserNr, 672, ClsDataDetail.MDData.MDNr) Then
			Try
				Me.Cbo_ESFiliale.Enabled = False
				Me.Cbo_ESKST1.Enabled = False
				Me.Cbo_ESKST2.Enabled = False
				Dim strUSTitle As String = String.Format("{0}|{1}", ClsDataDetail.UserData.UserFTitel, ClsDataDetail.UserData.UserSTitel)

				Me.Cbo_ESBerater.Enabled = strUSTitle.ToLower.Contains("leiter") Or strUSTitle.ToLower.Contains("führer")
				Me.Cbo_ESFiliale.Text = ClsDataDetail.UserData.UserFiliale

				Cbo_ESBerater.Properties.Items.Clear()
				Cbo_ESBerater.Properties.Items.Add(New ComboValue(String.Format("{0}, {1}", ClsDataDetail.UserData.UserLName, ClsDataDetail.UserData.UserFName), ClsDataDetail.UserData.UserKST))
				Me.Cbo_ESBerater.SelectedIndex = 0
				Me.Cbo_ESBerater.Refresh()


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.BenutzerTitel auflisten: {1}", strMethodeName, ex.ToString))
			End Try

		End If

	End Sub


#Region "Printing"


	Private Sub GetESData4Print(ByVal bForExport As Boolean, ByVal strJobInfo As String)
		Dim sSql As String = Me.txtAdminQuery.Text
		Dim storedProc As String = String.Empty

		If sSql = String.Empty Then
			m_UtilityUI.ShowErrorDialog("Keine Suche wurde gestartet!")

			Return
		End If

		If SQL4Print = "4.0.3" Then
			storedProc = String.Format("[Get KST Statistik For ESSearch] '{0}', '{1}', '{2}'",
																 ClsDataDetail.UserData.UserNr,
																 ClsDataDetail.SPTabNamenES, ClsDataDetail.SPTabNamenESKSTStatistik)

		ElseIf SQL4Print = "4.0.6" Then
			storedProc = String.Format("[Get KST Statistik For ESSearch] '{0}', '{1}', '{2}'",
																 ClsDataDetail.UserData.UserNr,
																 ClsDataDetail.SPTabNamenES, ClsDataDetail.SPTabNamenESKSTStatistik)

		End If
		If Not String.IsNullOrWhiteSpace(storedProc) Then sSql = String.Format(" EXECUTE {0} ", storedProc)

		Me.SQL4Print = sSql
		Me.PrintJobNr = strJobInfo

		StartPrinting()

	End Sub

	Sub StartPrinting()
		Dim result As PrintResult
		Dim _ClsDb As New ClsDbFunc
		Dim firstDate As Date? = CType(deAktiv_1.EditValue, Date)

		Dim success = _ClsDb.CreatePVLTable()

		Dim showDesign As Boolean = m_AllowedDesign AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)
		Dim filterConditions As List(Of String) = New List(Of String)(New String() {ClsDataDetail.GetFilterBez,
																																								ClsDataDetail.GetFilterBez2,
																																								ClsDataDetail.GetFilterBez3,
																																								ClsDataDetail.GetFilterBez4})
		Dim _Setting As New SPS.Listing.Print.Utility.ClsLLESSearchPrintSetting With {.DbConnString2Open = ClsDataDetail.MDData.MDDbConn,
																																									.SQL2Open = Me.SQL4Print,
																																									.JobNr2Print = Me.PrintJobNr,
																																									.SelectedMDNr = ClsDataDetail.MDData.MDNr,
																																									.LogedUSNr = ClsDataDetail.UserData.UserNr,
																																									.bAsDesign = showDesign,
																																									.ListSortBez = ClsDataDetail.GetSortBez,
																																									.ListFilterBez = filterConditions,
																																									.ShowBewilligStatistik = ClsDataDetail.ShowBewilligStatistik,
																																									.DbTblName = ClsDataDetail.SPTabNamenES,
																																									.FirstEmployDate = firstDate,
																																									.PermissionData = m_PermissionData,
																																									.DbTblName4KSTStatistik = ClsDataDetail.SPTabNamenESKSTStatistik,
																																									.BranchesNameForGAVStatistik = Cbo_ESFiliale.EditValue}

		Dim obj As New SPS.Listing.Print.Utility.ESSearchListing.ClsPrintESSearchList(m_InitialData)
		obj.PrintData = _Setting

		If PrintJobNr = "4.0.6" Then
			result = obj.PrintESGAVStatistiken()
		ElseIf PrintJobNr = "4.0.7" Then
			result = obj.PrintESEmployeesATStatistiken()

		Else
			obj.PrintESSearchList_1(showDesign, ClsDataDetail.GetSortBez,
															filterConditions,
															ClsDataDetail.ShowBewilligStatistik,
															ClsDataDetail.SPTabNamenES,
															ClsDataDetail.SPTabNamenESKSTStatistik)

		End If

	End Sub

#End Region




#Region "bbiSearch"


	Private Sub bbiSearch_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSearch.ItemClick
		Dim sSql1Query As String = String.Empty
		Dim sSql2Query As String = String.Empty
		Dim strSort As String = String.Empty

		Me.bbiPrint.Enabled = False
		Me.bbiExport.Enabled = False

		Try
			Me.txt_SQLQuery.Text = String.Empty
			Me.txtAdminQuery.Text = String.Empty
			Me.ShortSQLQuery = String.Empty

			If Not Me.txtESESNr_2.Visible Then Me.txtESESNr_2.Text = Me.txtESESNr_1.Text
			If Not Me.txtESKDNr_2.Visible Then Me.txtESKDNr_2.Text = Me.txtESKDNr_1.Text
			If Not Me.txtESMANr_2.Visible Then Me.txtESMANr_2.Text = Me.txtESMANr_1.Text

			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Nach Daten wird gesucht") & "..."

			FormIsLoaded("frmESSearch_LV", True)
			Try

				' Die Query-String aufbauen...
				GetMyQueryString()

			Catch ex As Exception
				m_UtilityUI.ShowErrorDialog(ex.ToString)

			End Try

			' Daten auflisten...
			If Not FormIsLoaded("frmESSearch_LV", True) Then
				frmMyLV = New frmESSearch_LV(Me.txt_SQLQuery.Text)
				frmMyLV.Show()
				Me.Select()
			End If

			Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet") & "...", frmMyLV.RecCount)
			frmMyLV.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet") & "...", frmMyLV.RecCount)

			' Die Buttons Drucken und Export aktivieren
			If frmMyLV.gvRP.RowCount > 0 Then
				Me.bbiPrint.Enabled = True
				Me.bbiExport.Enabled = True

				CreatePrintPopupMenu()
				CreateExportPopupMenu()
			End If

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		Finally

		End Try

	End Sub

	Function GetMyQueryString() As Boolean
		Dim sSql1Query As String = String.Empty
		Dim sSql2Query As String = String.Empty
		Dim strSort As String = String.Empty
		Dim sSqlUpdate As String = String.Empty
		Dim sSqlInsert As String = String.Empty
		Dim sSqlSelect As String = String.Empty

		Dim _ClsDb As New ClsDbFunc

		Dim result = lueMandant.Properties.GetItems.GetCheckedValues()
		For Each itm In result
			If CInt(Val(itm.ToString)) > 0 Then _ClsDb.mandantNumber.Add(CInt(itm.ToString))
		Next
		result = Nothing

		result = cboEmploymentKST.Properties.GetItems.GetCheckedValues()
		For Each itm In result
			If Not String.IsNullOrWhiteSpace(itm.ToString) Then _ClsDb.KstData.Add(itm.ToString)
		Next

		If Me.txt_IndSQLQuery.Text = String.Empty Then
			Try

				ClsDataDetail.SPTabNamenES = String.Format("_EinsatzListe_{0}", m_InitializationData.UserData.UserNr)
				ClsDataDetail.SPTabNamenESKSTStatistik = String.Format("_EinsatzListeKSTStatistik_{0}", m_InitializationData.UserData.UserNr)

				sSql1Query = _ClsDb.GetStartSQLString(Me)               ' Selektion in einer temp. Tabelle _Einsatzliste_<UserNr>
				sSql2Query = _ClsDb.GetQuerySQLString(sSql1Query, Me)   ' Where Klausel

				If Trim(sSql2Query) <> String.Empty Then
					sSql1Query += " Where "
				End If
				strSort = _ClsDb.GetSortString(Me)                      ' Sort Klausel

			Catch ex As Exception
				m_UtilityUI.ShowErrorDialog(ex.ToString)

			End Try

			' Update integrieren, da geteilte Kostenstellen noch nicht berücktsichtigt sind. 01.03.2010
			' Update durchführen, wenn nach Berater gesucht wird oder 
			' nach Filialen ohne Berater und verschiedene Fililalen gefunden werden. 17.03.2010
			' Update durchführen, wenn nach Berater gesucht wird und der Einsatz geteilt wurde. 17.03.2010
			Try

				If Me.Cbo_ESFiliale.SelectedIndex > 0 Or Me.Cbo_ESBerater.SelectedIndex > 0 Then
					sSqlUpdate = " UPDATE #Einsatzliste "
					sSqlUpdate &= "SET Stundenlohn = Stundenlohn/2, Tarif = Tarif/2, BruttoMarge = BruttoMarge/2, "
					sSqlUpdate &= "MargeMitBVG = MargeMitBVG/2, Geteilt = 1 "
					sSqlUpdate &= "WHERE (Filiale1 <> Filiale2 "

					If Me.Cbo_ESBerater.SelectedIndex > 0 Then
						sSqlUpdate &= "Or CHARINDEX('/', ESKST) > 0) "
					Else
						sSqlUpdate &= "); "
					End If

				Else
					sSqlUpdate = " UPDATE #Einsatzliste "
					sSqlUpdate &= "SET Geteilt = 1 WHERE (Filiale1 <> Filiale2 Or CHARINDEX('/', ESKST) > 0); "

				End If
				sSqlUpdate &= " UPDATE #Einsatzliste "
				sSqlUpdate &= "SET FilialGeteilt = 1 WHERE (Filiale1 <> Filiale2); "

			Catch ex As Exception
				m_UtilityUI.ShowErrorDialog(ex.ToString)

			End Try

			Try
				' Einsatzliste physikalisch auf der Datenbank schreiben
				sSqlInsert = String.Format("SELECT * INTO {0} FROM #Einsatzliste ", ClsDataDetail.SPTabNamenES)
				sSqlSelect = String.Format(" Select * From {0} ", ClsDataDetail.SPTabNamenES)

				Me.txt_SQLQuery.Text = sSql1Query + sSql2Query + sSqlUpdate + sSqlInsert + sSqlSelect + strSort
				If strLastSortBez = String.Empty Then strLastSortBez = strSort

				Me.ShortSQLQuery = sSqlSelect + strSort
				Me.txtAdminQuery.Text = Me.ShortSQLQuery


			Catch ex As Exception
				m_UtilityUI.ShowErrorDialog(ex.ToString)

			End Try

		Else
			Me.txt_SQLQuery.Text = Me.txt_IndSQLQuery.Text
			Me.ShortSQLQuery = Me.txt_SQLQuery.Text
			Me.txtAdminQuery.Text = Me.ShortSQLQuery

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
		Dim bShowDesign As Boolean = IsUserActionAllowed(0, 613)
		Dim liMnu As New List(Of String) From {"Einsatzliste#mnuESListe",
																					 "-KST. Statistiken#mnuESKSTStatistiken",
																					 "-Einsatzliste mit GAV-Daten#mnuESWithGAV",
																					 "GAV-Statistikliste#mnuGAVStatistik",
																					 "-Liste für Ticino#mnuESListTicino",
																					 "Daten für Deutschland#mnuESListGermany",
																					 If(m_InitialData.MDData.MDCanton = "FL", "-Meldung der Beschäftigtenangaben (AT)#mnuESListAT", "")}

		If Not IsUserActionAllowed(0, 603) Then Exit Sub
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
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

		End Try
	End Sub

	Sub GetPrintMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)

		ClsDataDetail.ShowBewilligStatistik = False
		Me.SQL4Print = String.Empty
		Me.bPrintAsDesign = False

		Select Case e.Item.Name.ToUpper
			Case "mnuESListe".ToUpper
				ClsDataDetail.ShowBewilligStatistik = True
				SQL4Print = "4.0.1"

			Case "mnuESKSTStatistiken".ToUpper
				SQL4Print = "4.0.3"

			Case "mnuESWithGAV".ToUpper
				SQL4Print = "4.0.5"

			Case "mnuGAVStatistik".ToUpper
				SQL4Print = "4.0.6"

			Case "mnuESListTicino".ToUpper
				SQL4Print = "4.0.11"

			Case "mnuESListGermany".ToUpper
				SQL4Print = "4.0.4"

			Case "mnuESListAT".ToUpper
				SQL4Print = "4.0.7"


			Case Else
				Return

		End Select

		GetESData4Print(False, SQL4Print)

	End Sub


#End Region


#Region "bbExport"

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
			bbiExport.Manager = Me.BarManager1
			BarManager1.ForceInitialize()

			Me.bbiExport.ActAsDropDown = False
			Me.bbiExport.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
			Me.bbiExport.DropDownEnabled = True
			Me.bbiExport.DropDownControl = popupMenu
			Me.bbiExport.Enabled = True

			For i As Integer = 0 To liMnu.Count - 1
				Dim myValue As String() = liMnu(i).Split(CChar("#"))
				If myValue(1).ToString.ToLower.Contains("parifond") Then
					bshowMnu = IsUserActionAllowed(ClsDataDetail.UserData.UserNr, 274, ClsDataDetail.MDData.MDNr)
				End If

				If bshowMnu Then
					popupMenu.Manager = BarManager1

					Dim itm As New DevExpress.XtraBars.BarButtonItem
					itm.Caption = m_Translate.GetSafeTranslationValue(myValue(0).ToString)
					itm.Name = myValue(1).ToString
					itm.AccessibleName = myValue(2).ToString

					If myValue(0).ToString.ToLower.Contains("-".ToLower) Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)

					AddHandler itm.ItemClick, AddressOf GetExportMenuItem
				End If
				bshowMnu = True
			Next

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

		End Try
	End Sub

	Sub GetExportMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
		Dim strSQL As String = Me.ShortSQLQuery

		' Query umstellen für die korrekte Interpretation des Selects von den Komponenten. 
		Dim query As String = Me.txtAdminQuery.Text ' String.Format("Select * From {0}", ClsDataDetail.SPTabNamenES)

		Select Case UCase(e.Item.Name.ToUpper)
			Case UCase("XLS")

			Case UCase("Contact")
				Call ExportDataToOutlook(query)

			Case UCase("MAIL")
				Call RunMailModul(query)

			Case UCase("FAX")
				Call RunTobitFaxModul(query)

	'		Case UCase("SMS")
	'			query = "Select DISTINCT(ES.MANr), ES.Nachname, ES.Vorname, " +
	'"( " +
	'		"Case ES.MA_SMS_Mailing " +
	'			"When 0 Then ES.MANatel Else '' End) As Natel, " +
	'			"ES.Geschlecht, " +
	'			"mak.Briefanrede AS Anredeform, " +
	'			"(ES.MAStrasse + ', ' + ES.MALand + '-' + ES.MAPlz + ' ' + ES.MAOrt) AS Adresse " +
	'			"From _EinsatzListe_{0} ES " +
	'"LEFT JOIN dbo.MAKontakt_Komm mak ON mak.manr = ES.manr " +
	'"Where (ES.MANatel <> '' And ES.MANatel Is Not Null ) And ES.MA_SMS_Mailing <> 1 " +
	'"Order by ES.Nachname, ES.Vorname"
	'			query = String.Format(query, ClsDataDetail.UserData.UserNr)

	'			Call RunSMSProg(query)

			Case UCase("eCall-SMS")
				Dim sql As String
				sql = "Select DISTINCT(ES.MANr), ES.Nachname, ES.Vorname, "
				sql &= "( "
				sql &= "Case ES.MA_SMS_Mailing "
				sql &= "When 0 Then ES.MANatel Else '' End) As Natel, "
				sql &= "ES.Geschlecht, "
				sql &= "mak.Briefanrede AS Anredeform, "
				sql &= "ES.MAStrasse Strasse, ES.MALand Land, ES.MAPlz PLZ, ES.MAOrt Ort "
				sql &= "From _EinsatzListe_{0} ES "
				sql &= "LEFT JOIN dbo.MAKontakt_Komm mak ON mak.manr = ES.manr "
				sql &= "Where (ES.MANatel <> '' And ES.MANatel Is Not Null ) And ES.MA_SMS_Mailing <> 1 "
				sql &= "Order by ES.Nachname, ES.Vorname"

				query = String.Format(sql, ClsDataDetail.UserData.UserNr)

				Call RuneCallSMSModul(query)


			Case UCase("Parifond")
				If Me.deAktiv_1.EditValue Is Nothing OrElse Me.deAktiv_2.EditValue Is Nothing Then
					m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Sie müssen die Felder für ""Aktiv zwischen"" ausfüllen. Bitte ändern Sie ihre Suchkrieterien."))

				Else
					StartExport4ParifondControl()

				End If


			Case UCase("TXT"), UCase("CSV")
				StartExportESModul()


			Case UCase("KD_CSV")
				query = "Select DISTINCT(E.KDNr), E.Firma1, ISNULL(z.Nachname, '') AS Nachname, ISNULL(z.Vorname, '' ) AS Vorname, "
				query &= "( "
				query &= "Case z.ZHD_SMS_Mailing "
				query &= "When 0 Then Z.Natel Else '' End) As ZNatel, "
				query &= "( "
				query &= "Case Z.ZHD_Mail_Mailing "
				query &= "When 0 Then Z.eMail Else '' End) As ZeMail, "
				query &= "( "
				query &= "Case KD.KD_Mail_Mailing "
				query &= "When 0 Then KD.eMail Else '' End) As KDeMail, "
				query &= "ISNULL(Z.Anrede, '') AS Anrede, "
				query &= "ISNULL(Z.AnredeForm, '') AS AnredeForm, "
				query &= "KD.Strasse AS KDStrasse, KD.Land AS KDLand, KD.Plz AS KDPLZ, KD.Ort AS KDOrt "
				query &= "From _EinsatzListe_{0} E "
				query &= "LEFT JOIN dbo.Kunden KD ON E.KDnr = KD.KDnr "
				query &= "LEFT JOIN dbo.KD_Zustaendig Z ON E.ESKDZHDNr = z.recnr AND E.kdnr = z.KDNr "
				query &= "Order by E.Firma1"

				query = String.Format(query, ClsDataDetail.UserData.UserNr)

				StartExportESKDModul(query)


			Case UCase("MA_CSV")
				query = "Select DISTINCT(E.MANr), E.Nachname, E.Vorname, "
				query &= "( "
				query &= "Case E.MA_SMS_Mailing "
				query &= "When 0 Then E.MANatel Else '' End) As Natel, "
				query &= "( "
				query &= "Case MA.MA_EMail_Mailing "
				query &= "When 0 Then ma.eMail Else '' End) As eMail, "
				query &= "E.Geschlecht, E.MACo, E.MAPostfach, "
				query &= "mak.Briefanrede AS Anredeform, "
				query &= "E.MAStrasse, E.MALand, E.MAPlz, E.MAOrt "
				query &= "From _EinsatzListe_{0} E "
				query &= "LEFT JOIN dbo.Mitarbeiter MA ON E.manr = ma.manr "
				query &= "LEFT JOIN dbo.MAKontakt_Komm mak ON mak.manr = E.manr "
				query &= "Order by E.Nachname, E.Vorname"

				query = String.Format(query, ClsDataDetail.UserData.UserNr)


				StartExportESMAModul(query)

		End Select

	End Sub

	Sub StartExport4ParifondControl()
		Dim _Setting As New SPS.Export.Listing.Utility.ClsParifondSetting With {.DbConnString2Open = ClsDataDetail.MDData.MDDbConn,
																																					 .SQL2Open = Me.txtAdminQuery.Text,
																																						.ExportTablename = ClsDataDetail.SPTabNamenES,
																																						.SelectedVonDate = deAktiv_1.EditValue,
																																						.SelectedBisDate = deAktiv_1.EditValue,
																																						.FirstMonth = Month(Me.deAktiv_1.EditValue),
																																						.FirstYear = Year(Me.deAktiv_1.EditValue),
																																						.LastMonth = Month(Me.deAktiv_2.EditValue),
																																						.LastYear = Year(Me.deAktiv_2.EditValue)
																																																}
		Dim obj As New SPS.Export.Listing.Utility.ClsParifondStart(m_InitialData, _Setting)
		obj.ShowfrmParifondControl()

	End Sub

	Sub StartExportESModul()
		Dim _Setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = ClsDataDetail.MDData.MDDbConn,
																																					 .SQL2Open = Me.txtAdminQuery.Text,
																																					 .ModulName = "ESSearch"}
		Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_Setting)
		obj.ExportCSVFromESSearchListing(Me.txtAdminQuery.Text)

	End Sub

	Sub StartExportESKDModul(ByVal sql As String)
		Dim _Setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = ClsDataDetail.MDData.MDDbConn,
																																					 .SQL2Open = sql,
																																					 .ModulName = "ESKDSearch"}
		Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_Setting)
		obj.ExportCSVFromESKDSearchListing(sql)

	End Sub

	Sub StartExportESMAModul(ByVal sql As String)
		Dim _Setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = ClsDataDetail.MDData.MDDbConn,
																																					 .SQL2Open = sql,
																																					 .ModulName = "ESMASearch"}
		Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_Setting)
		obj.ExportCSVFromESMASearchListing(sql)

	End Sub



#End Region


#Region "Clear Fields"

	''' <summary>
	''' Funktion für das Leeren der Felder...
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub bbiClear_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiClear.ItemClick

		Dim strText As String = Me.CboSort.Text
		Dim strKst3 As String = Me.Cbo_ESBerater.Text
		Dim strFiliale As String = ClsDataDetail.UserData.UserFiliale

		FormIsLoaded("frmESSearch_LV", True)
		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Bereit")

		Me.deAktiv_1.Enabled = True
		Me.deAktiv_2.Enabled = True
		ResetAllTabEntries()

		' Default Checkbox-State
		Me.ChkESNurAktiveEinsaetze.Checked = False
		Me.ChkESNurAktiveLohn.Checked = True
		Me.Chk_ESKST1.Checked = True
		Me.Chk_ESKST2.Checked = True
		Me.Chk_ESBerater.Checked = True

		' Die Buttons Drucken und Export deaktivieren
		Me.bbiPrint.Enabled = False
		Me.bbiExport.Enabled = False

		' Checkbox defaults
		Me.chkKDKreditlimiteUeberschritten.Checked = False

		' Füllen, wenn Disabled
		If Not Me.Cbo_ESBerater.Enabled Then Me.Cbo_ESBerater.Text = strKst3
		If Not Me.Cbo_ESFiliale.Enabled Then Me.Cbo_ESFiliale.Text = strFiliale
		Me.txt_SQLQuery.Text = m_Translate.GetSafeTranslationValue("Wurde geleert...")

		SetBeraterValues()

	End Sub

	''' <summary>
	''' Alle vorhandene TabPages und jedes darin befindliches Controls durchlaufen.
	''' </summary>
	''' <remarks></remarks>
	Private Sub ResetAllTabEntries()
		For Each tabPg As DevExpress.XtraTab.XtraTabPage In Me.Controls.Item("xtabESSearch").Controls
			For Each ctrls In tabPg.Controls
				ResetControl(ctrls)
			Next
		Next
	End Sub

	''' <summary>
	''' Jedes Control wird anhand des Typs zurückgesetzt.
	''' </summary>
	''' <param name="con"></param>
	''' <remarks>Bei GroupBox wird die Funktion rekursiv aufgerufen.</remarks>
	Private Sub ResetControl(ByVal con As Control)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Try

			If con.Name.ToLower = "cboemploymentkst" Then
				Trace.WriteLine(String.Format("{0}: {1} | {2}", con.Name, con.GetType, con.Text))

			Else
				Trace.WriteLine(String.Format("{0}: {1} | {2}", con.Name, con.GetType, con.Text))
			End If
			If con.Name.ToLower.Contains("cbosort") OrElse con.Name.ToLower.Contains("luemandant") Then Exit Sub

			If con.Enabled Then
				Trace.WriteLine(con.Name)
				If TypeOf (con) Is TextBox Then
					Dim tb As TextBox = con
					tb.Text = String.Empty

				ElseIf TypeOf (con) Is DevExpress.XtraEditors.DateEdit Then
					Dim de As DevExpress.XtraEditors.DateEdit = con
					de.EditValue = Nothing

				ElseIf TypeOf (con) Is DevExpress.XtraEditors.MemoEdit Then
					Dim tb As DevExpress.XtraEditors.MemoEdit = con
					tb.Text = String.Empty

				ElseIf TypeOf (con) Is System.Windows.Forms.ComboBox Then
					Dim cbo As System.Windows.Forms.ComboBox = con
					cbo.Text = String.Empty
					cbo.SelectedIndex = -1

				ElseIf TypeOf (con) Is DevExpress.XtraEditors.ComboBoxEdit Then
					Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = con
					cbo.Properties.Items.Clear()
					cbo.EditValue = Nothing

				ElseIf TypeOf (con) Is DevExpress.XtraEditors.CheckedComboBoxEdit Then
					Dim cbo As DevExpress.XtraEditors.CheckedComboBoxEdit = con
					cbo.Properties.Items.Clear()
					cbo.EditValue = Nothing

				ElseIf TypeOf (con) Is CheckBox Then
					Dim cbo As CheckBox = con
					cbo.CheckState = CheckState.Unchecked

				ElseIf TypeOf (con) Is DevExpress.XtraEditors.CheckEdit Then
					Dim cbo As DevExpress.XtraEditors.CheckEdit = con
					cbo.CheckState = CheckState.Unchecked

				ElseIf TypeOf (con) Is ListBox Then
					Dim lst As ListBox = con
					lst.Items.Clear()

				ElseIf TypeOf (con) Is DevExpress.XtraEditors.TextEdit Then
					Dim tb As DevExpress.XtraEditors.TextEdit = con
					tb.Text = String.Empty


				ElseIf TypeOf (con) Is DevExpress.XtraEditors.ListBoxControl Then
					Dim lst As DevExpress.XtraEditors.ListBoxControl = con
					lst.Items.Clear()

				Else
					For Each childCon As Control In con.Controls
						ResetControl(childCon)
					Next

				End If
			End If

		Catch ex As Exception ' Manager
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
		End Try

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

	'Private Sub Lst_KDBerufe_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
	'	Dim eTest As System.Windows.Forms.MouseEventArgs

	'	Try
	'		If e.KeyValue = Keys.Delete Then
	'			Me.Lst_MABeurteilung.Items.Remove(Me.Lst_MABeurteilung.SelectedItem)
	'		ElseIf e.KeyValue = Keys.F4 Then
	'			LBBeruf_1_MouseClick(sender, eTest)
	'		End If

	'	Catch ex As Exception

	'	End Try

	'End Sub


#End Region


#Region "von-bis Buttons"

	Private Sub txtESESNr_1_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtESESNr_1.ButtonClick
		Dim frmTest As New frmSearchRec("ES-Nr.")

		Dim m As String
		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iESValue(_ClsFunc.GetSelektion)
		Me.txtESESNr_1.Text = If(m = Nothing, String.Empty, CStr(m.ToString.Replace(strValueSeprator, ",")))
		frmTest.Dispose()

	End Sub

	Private Sub txtESESNr_2_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtESESNr_2.ButtonClick
		Dim frmTest As New frmSearchRec("ES-Nr.")

		Dim m As String
		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iESValue(_ClsFunc.GetSelektion)
		Me.txtESESNr_2.Text = If(m = Nothing, String.Empty, CStr(m.ToString.Replace(strValueSeprator, ",")))
		frmTest.Dispose()

	End Sub

	Private Sub txtESMANr_1_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtESMANr_1.ButtonClick
		Dim frmTest As New frmSearchRec("MA-Nr.")

		Dim m As String
		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iESValue(_ClsFunc.GetSelektion)
		Me.txtESMANr_1.Text = If(m = Nothing, String.Empty, CStr(m.ToString.Replace(strValueSeprator, ",")))
		frmTest.Dispose()

	End Sub

	Private Sub txtESMANr_2_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtESMANr_2.ButtonClick
		Dim frmTest As New frmSearchRec("MA-Nr.")

		Dim m As String
		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iESValue(_ClsFunc.GetSelektion)
		Me.txtESMANr_2.Text = If(m = Nothing, String.Empty, CStr(m.ToString.Replace(strValueSeprator, ",")))
		frmTest.Dispose()

	End Sub

	Private Sub txtESKDNr_1_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtESKDNr_1.ButtonClick
		Dim frmTest As New frmSearchRec("KD-Nr.")

		Dim m As String
		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iESValue(_ClsFunc.GetSelektion)
		Me.txtESKDNr_1.Text = If(m = Nothing, String.Empty, CStr(m.ToString.Replace(strValueSeprator, ",")))
		frmTest.Dispose()

	End Sub

	Private Sub txtESKDNr_2_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtESKDNr_2.ButtonClick
		Dim frmTest As New frmSearchRec("KD-Nr.")

		Dim m As String
		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iESValue(_ClsFunc.GetSelektion)
		Me.txtESKDNr_2.Text = If(m = Nothing, String.Empty, CStr(m.ToString.Replace(strValueSeprator, ",")))
		frmTest.Dispose()

	End Sub


#End Region




	''' <summary>
	''' Klick-Event der Controls auffangen und verarbeiten
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub KeyPressEvent(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles txtESMANr_2.KeyPress, txtESMANr_1.KeyPress, txtESKDNr_2.KeyPress, txtESKDNr_1.KeyPress, txtESESNr_1.KeyPress, txtESESNr_2.KeyPress, Cbo_ESESVertragUntersch.KeyPress, Lst_ESGAVBeruf.KeyPress, Lst_ESEinsatzAls.KeyPress, Lst_ESBranche.KeyPress, Cbo_ESVerleihUntersch.KeyPress, Cbo_ESGruppe1.KeyPress, Cbo_ESGAVKanton.KeyPress, Cbo_ESVerleihvertragGedruckt.KeyPress, Cbo_ESUnterzeichner.KeyPress, Cbo_ESEinstufung.KeyPress, Cbo_ESEinsatzvertragGedruckt.KeyPress, Cbo_ESSuvaCode.KeyPress, Cbo_ESKST2.KeyPress, Cbo_ESKST1.KeyPress, Cbo_ESBerater.KeyPress, Cbo_ESAktivImSelektion.KeyPress, Cbo_MAVorschussSperren.KeyPress, Cbo_MARapporteDrucken.KeyPress, Cbo_MANewAHVVollstaendig.KeyPress, Cbo_MALohnSperren.KeyPress, Cbo_MAKorrSprache.KeyPress, Cbo_MABewBisMonat.KeyPress, Cbo_MABewBisJahr.KeyPress, Cbo_MAAHVVollstaendig.KeyPress, Cbo_KDSprache.KeyPress, Cbo_KDKreditwarnung.KeyPress, Cbo_KDKanton.KeyPress, Cbo_KDImWeb.KeyPress, Cbo_KDEinsatzSperren.KeyPress, Cbo_KDRapporteDrucken.KeyPress, deCreated_2.KeyPress, deCreated_1.KeyPress, deAktiv_2.KeyPress, deAktiv_1.KeyPress, ChkESNurAktiveEinsaetze.KeyPress, Chk_ESKST2.KeyPress, Chk_ESKST1.KeyPress, Chk_ESBerater.KeyPress, deMAGeb_2.KeyPress, deMAGeb_1.KeyPress, Cbo_MAGeborenMonat.KeyPress, CboSort.KeyPress, ChkESNurAktiveLohn.KeyPress, Lst_KDKreditlimite1.KeyPress, deKredit_1.KeyPress, deKredit_2.KeyPress, Lst_KDKreditlimite2.KeyPress, Cbo_ESFiliale.KeyPress          ' System.EventArgs)

		Try
			If e.KeyChar = Chr(13) Then ' Enter
				SendKeys.Send("{tab}")
				e.Handled = True
			End If

		Catch ex As Exception
			MessageBox.Show(ex.ToString, "KeyPress", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try

	End Sub

	Private Sub btnDeleteBerufe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
		If Me.Lst_ESEinsatzAls.SelectedIndex > -1 Then
			Me.Lst_ESEinsatzAls.Items.RemoveAt(Me.Lst_ESEinsatzAls.SelectedIndex)
		End If
	End Sub


	''' <summary>
	''' Allgemeiner Delete-Event für die Listbox und Textbox
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub KeyUpEvent(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtESESNr_1.KeyUp, txtESMANr_2.KeyUp, txtESMANr_1.KeyUp,
		txtESKDNr_2.KeyUp, txtESKDNr_1.KeyUp, txtESESNr_2.KeyUp, Lst_ESGAVBeruf.KeyUp, Lst_ESEinsatzAls.KeyUp, Lst_ESBranche.KeyUp,
		Lst_KDKreditlimite1.KeyUp, Lst_KDKreditlimite2.KeyUp
		If TypeOf (sender) Is DevExpress.XtraEditors.ListBoxControl Then
			Dim _lst As DevExpress.XtraEditors.ListBoxControl = DirectCast(sender, DevExpress.XtraEditors.ListBoxControl)

			If e.KeyCode = Keys.Delete And _lst.SelectedIndices.Count > 0 Then
				For i As Integer = 0 To _lst.SelectedIndices.Count - 1
					' Wenn der erste selektierte Inidices gelöscht wird,
					' rückt der nächste automatisch nach bis keine mehr
					' vorhanden sind. Darum immer 0. 19.08.2009 A.Ragusa
					Try
						_lst.Items.RemoveAt(_lst.SelectedIndices(0))
					Catch ex As Exception
						' Wenn der Benutzer einen schnellen Finger hat und es unbedingt beweisen möchte. 22.10.2010 ar
					End Try
				Next

			ElseIf e.KeyCode = Keys.F4 Then
				Select Case _lst.Tag
					Case LibESGAVBeruf.Name
						LnkESGAVBeruf_Click(sender, New System.EventArgs)
					Case LibESEinsatzAls.Name
						LnkESEinsatzAls_Click(sender, New System.EventArgs)
					Case LibESBranche.Name
						LnkESBranche_Click(sender, New System.EventArgs)
					Case LibKDKreditlimite1.Name
						LnkKDKreditlimite1_Click(sender, New System.EventArgs)
					Case LibKDKreditlimite2.Name
						LnkKDKreditlimite2_Click(sender, New System.EventArgs)
				End Select
			End If

		ElseIf TypeOf (sender) Is TextBox Then
			Dim _txt As TextBox = DirectCast(sender, TextBox)
			If e.KeyCode = Keys.F4 Then
				Select Case _txt.Tag
					Case txtESESNr_1.Name
						txtESESNr_1_ButtonClick(sender, New System.EventArgs)
					Case txtESESNr_2.Name
						txtESESNr_2_ButtonClick(sender, New System.EventArgs)

					Case txtESMANr_1.Name
						txtESMANr_1_ButtonClick(sender, New System.EventArgs)
					Case txtESMANr_2.Name
						txtESMANr_2_ButtonClick(sender, New System.EventArgs)

					Case txtESKDNr_1.Name
						txtESKDNr_1_ButtonClick(sender, New System.EventArgs)
					Case txtESKDNr_2.Name
						txtESKDNr_2_ButtonClick(sender, New System.EventArgs)

				End Select

			End If

		End If

	End Sub


	Private Sub frmESSearch_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
		Me.txtESESNr_1.Focus()
	End Sub








	''' <summary>
	''' Selektionsfenster für die Mitarbeiter-Berufe (Einsatz als) öffnen
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub LBESEinsatzAls_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
		Dim frmTest As New frmSearchRec("Einsatz-als")

		Dim m As String
		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iESValue(_ClsFunc.GetSelektion)
		If m.ToString <> String.Empty Then
			Dim strEintrag As String() = Regex.Split(m.ToString, strValueSeprator)
			For i As Integer = 0 To strEintrag.Count - 1
				If Not Me.Lst_ESEinsatzAls.Items.Contains(strEintrag(i)) Then
					Me.Lst_ESEinsatzAls.Items.Add(strEintrag(i))
				End If
			Next
		End If
		frmTest.Dispose()

		'Dim frmTest As New frmSearchRec
		'Dim i As Integer = 0

		'_ClsFunc.Get4What = "ES_Als"
		'ClsDataDetail.strButtonValue = "EinsatzAls"
		'ClsDataDetail.Get4What = "ES_Als"

		'Dim m As String
		'frmTest.ShowDialog()
		'frmTest.MdiParent = Me.MdiParent

		'm = frmTest.iESValue(_ClsFunc.Get4What)
		'If m.ToString <> String.Empty Then
		'  Dim strEintrag As String() = Regex.Split(m.ToString, strValueseprator)
		'  For i = 0 To strEintrag.Count - 1
		'    If Not Me.Lst_ESEinsatzAls.Items.Contains(strEintrag(i)) Then
		'      Me.Lst_ESEinsatzAls.Items.Add(strEintrag(i))
		'    End If
		'  Next
		'End If

		'frmTest.Dispose()
	End Sub

	''' <summary>
	''' Selektionsfenster für GAV-Berufe öffnen
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub LBESGAVBeruf_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
		Dim frmTest As New frmSearchRec("GAV")

		Dim m As String
		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iESValue(_ClsFunc.GetSelektion)
		If m.ToString <> String.Empty Then
			Dim strEintrag As String() = Regex.Split(m.ToString, strValueSeprator)
			For i As Integer = 0 To strEintrag.Count - 1
				If Not Me.Lst_ESGAVBeruf.Items.Contains(strEintrag(i)) Then
					Me.Lst_ESGAVBeruf.Items.Add(strEintrag(i))
				End If
			Next
		End If
		frmTest.Dispose()

		'Dim frmTest As New frmSearchRec
		'Dim i As Integer = 0

		'_ClsFunc.Get4What = "GAVBerufe"
		'ClsDataDetail.strButtonValue = "GAVBerufe"
		'ClsDataDetail.Get4What = "GAVBerufe"

		'Dim m As String
		'frmTest.ShowDialog()
		'frmTest.MdiParent = Me.MdiParent

		'm = frmTest.iESValue(_ClsFunc.Get4What)
		'If m.ToString <> String.Empty Then
		'  Dim strEintrag As String() = Regex.Split(m.ToString, strValueseprator)
		'  For i = 0 To strEintrag.Count - 1
		'    If Not Me.Lst_ESGAVBeruf.Items.Contains(strEintrag(i)) Then
		'      Me.Lst_ESGAVBeruf.Items.Add(strEintrag(i))
		'    End If
		'  Next
		'End If

		'frmTest.Dispose()
	End Sub

	Private Sub Cbo_MAGeborenMonat_DropDown_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MAGeborenMonat.QueryPopUp

		If Me.Cbo_MAGeborenMonat.Properties.Items.Count = 0 Then
			Me.Cbo_MAGeborenMonat.Properties.Items.Add("") ' Leeres Feld zur Abwahl
			For m As Integer = 1 To 12
				Me.Cbo_MAGeborenMonat.Properties.Items.Add(m)
			Next
			Me.Cbo_MAGeborenMonat.Properties.DropDownRows = 12

		End If

	End Sub
	Private Sub Cbo_MABewBisMonat_DropDown_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MABewBisMonat.QueryPopUp
		If Me.Cbo_MABewBisMonat.Properties.Items.Count = 0 Then ListBewBisMonat(Me.Cbo_MABewBisMonat)
	End Sub

	Private Sub Cbo_MABewBisJahr_DropDown_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MABewBisJahr.QueryPopUp
		If Me.Cbo_MABewBisJahr.Properties.Items.Count = 0 Then ListMABewBisJahr(Me.Cbo_MABewBisJahr)
	End Sub

	Private Sub Cbo_ESAktivImSelektion_QueryPopUp(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_ESAktivImSelektion.QueryPopUp
		If Me.Cbo_ESAktivImSelektion.Properties.Items.Count = 0 Then
			' Berechnung der Kalenderwoche (Achtung: In der letzte Woche des Jahres wird die Zahl nicht immer korrekt zurückgegeben.)
			Dim kw As Integer = DatePart(DateInterval.WeekOfYear, Date.Now, FirstDayOfWeek.Monday, FirstWeekOfYear.FirstFourDays)
			' Berechnung des Monats
			Dim mon As Integer = DatePart(DateInterval.Month, Date.Now)
			' Berechnung des Jahres
			Dim jahr As Integer = DatePart(DateInterval.Year, Date.Now)

			Me.Cbo_ESAktivImSelektion.Properties.Items.Add(New ComboValue("", ""))
			Me.Cbo_ESAktivImSelektion.Properties.Items.Add(New ComboValue(String.Format(m_Translate.GetSafeTranslationValue("Letzte Woche / KW {0}"), kw - 1), "LW"))
			Me.Cbo_ESAktivImSelektion.Properties.Items.Add(New ComboValue(String.Format(m_Translate.GetSafeTranslationValue("Diese Woche / KW {0}"), kw), "DW"))
			Me.Cbo_ESAktivImSelektion.Properties.Items.Add(New ComboValue(String.Format(m_Translate.GetSafeTranslationValue("Nächste Woche / KW {0}"), kw + 1), "NW"))
			Me.Cbo_ESAktivImSelektion.Properties.Items.Add(New ComboValue(String.Format(m_Translate.GetSafeTranslationValue("Letzten Monat ({0}{1})"), If(mon = 1, 12, mon - 1), If(mon = 1, " / " & jahr - 1, "")), "LM"))
			Me.Cbo_ESAktivImSelektion.Properties.Items.Add(New ComboValue(String.Format(m_Translate.GetSafeTranslationValue("Diesen Monat ({0})"), mon), "DM"))
			Me.Cbo_ESAktivImSelektion.Properties.Items.Add(New ComboValue(String.Format(m_Translate.GetSafeTranslationValue("Nächsten Monat ({0})"), mon + 1), "NM"))
			Me.Cbo_ESAktivImSelektion.Properties.Items.Add(New ComboValue(String.Format(m_Translate.GetSafeTranslationValue("Letztes Jahr ({0})"), jahr - 1), "LJ"))
			Me.Cbo_ESAktivImSelektion.Properties.Items.Add(New ComboValue(String.Format(m_Translate.GetSafeTranslationValue("Dieses Jahr ({0})"), jahr), "DJ"))
			Me.Cbo_ESAktivImSelektion.Properties.Items.Add(New ComboValue(String.Format(m_Translate.GetSafeTranslationValue("Nächstes Jahr ({0})"), jahr + 1), "NJ"))

			Cbo_ESAktivImSelektion.Properties.DropDownRows = 10
		End If
	End Sub

	''' <summary>
	''' Selektionsfenster für die GAV-Berufe öffnen
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub LnkESGAVBeruf_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LibESGAVBeruf.Click
		Dim frmTest As New frmSearchRec("GAV-Berufe")

		Dim m As String
		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iESValue(_ClsFunc.GetSelektion)
		If Not m Is Nothing Then
			If m.ToString <> String.Empty Then
				Dim strEintrag As String() = Regex.Split(m.ToString, strValueSeprator)
				For i As Integer = 0 To strEintrag.Count - 1
					If Not Me.Lst_ESGAVBeruf.Items.Contains(strEintrag(i)) Then
						Me.Lst_ESGAVBeruf.Items.Add(strEintrag(i))
					End If
				Next
			End If
		End If
		frmTest.Dispose()

	End Sub

	''' <summary>
	''' Selektionsfenster für 'Einsatz als' öffnen
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub LnkESEinsatzAls_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LibESEinsatzAls.Click
		Dim frmTest As New frmSearchRec("Einsatz-als")

		Dim m As String
		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iESValue(_ClsFunc.GetSelektion)
		If m.ToString <> String.Empty Then
			Dim strEintrag As String() = Regex.Split(m.ToString, strValueSeprator)
			For i As Integer = 0 To strEintrag.Count - 1
				If Not Me.Lst_ESEinsatzAls.Items.Contains(strEintrag(i)) Then
					Me.Lst_ESEinsatzAls.Items.Add(strEintrag(i))
				End If
			Next
		End If
		frmTest.Dispose()

	End Sub

	Private Sub Cbo_ESAktivImSelektion_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_ESAktivImSelektion.SelectedIndexChanged
		Dim selectedItem As String = Me.Cbo_ESAktivImSelektion.Text '.ToItem.Value_1
		Dim dayOfWeek As Integer = Integer.Parse(Date.Now.DayOfWeek.ToString("D"))
		Dim month As Integer = Date.Now.Month
		Dim year As Integer = Date.Now.Year

		' Eingabe von ein neuenm Datum sperren

		' Die Woche fängt mit Sonntag=0 an, somit muss eine Woche abgezogen werden,
		' falls am Sonntag die Abfrage gestartet wird.
		If dayOfWeek = 0 Then
			dayOfWeek = 7
		End If
		Dim cv As ComboValue = DirectCast(sender.selecteditem, ComboValue)
		If cv Is Nothing Then Exit Sub
		Me.deAktiv_1.Enabled = False
		Me.deAktiv_2.Enabled = False

		Select Case cv.ComboValue
			Case "LW"   ' Letzte Woche
				' Ob er am Montag oder Sonntag die Daten der letzten Woche abfragt,
				' so das Wochentag berücksichtigen und abziehen
				Me.deAktiv_1.Text = Date.Now.AddDays(-6 - dayOfWeek).ToString("dd.MM.yyyy")
				Me.deAktiv_2.Text = Date.Now.AddDays(0 - dayOfWeek).ToString("dd.MM.yyyy")
			Case "DW"   ' Diese Woche
				Me.deAktiv_1.Text = Date.Now.AddDays(1 - dayOfWeek).ToString("dd.MM.yyyy")
				Me.deAktiv_2.Text = Date.Now.AddDays(7 - dayOfWeek).ToString("dd.MM.yyyy")
			Case "NW"   ' Nächste Woche
				Me.deAktiv_1.Text = Date.Now.AddDays(8 - dayOfWeek).ToString("dd.MM.yyyy")
				Me.deAktiv_2.Text = Date.Now.AddDays(14 - dayOfWeek).ToString("dd.MM.yyyy")
			Case "LM"   ' Letzter Monat
				If month = 1 Then
					month = 12
					year -= 1
				Else
					month -= 1
				End If
				Me.deAktiv_1.Text = Date.Parse(String.Format("01.{0}.{1}", month, year))
				Me.deAktiv_2.Text = Date.Parse(String.Format("{0}.{1}.{2}",
										Date.DaysInMonth(year, month), month, year))
			Case "DM"   ' Diesen Monat
				Me.deAktiv_1.Text = Date.Parse(String.Format("01.{0}.{1}", month, year))
				Me.deAktiv_2.Text = Date.Parse(String.Format("{0}.{1}.{2}",
										Date.DaysInMonth(year, month), month, year))
			Case "NM"   ' Nächsten Monat
				month += 1
				Me.deAktiv_1.Text = Date.Parse(String.Format("01.{0}.{1}", month, year))
				Me.deAktiv_2.Text = Date.Parse(String.Format("{0}.{1}.{2}",
										Date.DaysInMonth(year, month), month, year))
			Case "LJ"   ' Letztes Jahr
				year -= 1
				Me.deAktiv_1.Text = Date.Parse(String.Format("01.01.{0}", year))
				Me.deAktiv_2.Text = Date.Parse(String.Format("31.12.{0}", year))
			Case "DJ"   ' Dieses Jahr
				Me.deAktiv_1.Text = Date.Parse(String.Format("01.01.{0}", year))
				Me.deAktiv_2.Text = Date.Parse(String.Format("31.12.{0}", year))
			Case "NJ"   ' Nächstes Jahr
				year += 1
				Me.deAktiv_1.Text = Date.Parse(String.Format("01.01.{0}", year))
				Me.deAktiv_2.Text = Date.Parse(String.Format("31.12.{0}", year))


			Case Else

				deAktiv_1.EditValue = Nothing
				deAktiv_2.EditValue = Nothing
				deAktiv_1.Enabled = True
				deAktiv_2.Enabled = True

		End Select

	End Sub

	' Korrespondenzsprache
	Private Sub Cbo_MAKorrSprache_QueryPopUp(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MAKorrSprache.QueryPopUp
		If Me.Cbo_MAKorrSprache.Properties.Items.Count = 0 Then ListMAKorrSprachen(Me.Cbo_MAKorrSprache)
	End Sub


	Private Sub Chk_ESKST1_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs)
		Me.Chk_ESKST1.BackColor = Color.Gray
	End Sub

	Private Sub Checkbox_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chk_ESKST1.Enter, Chk_ESKST2.Enter, Chk_ESBerater.Enter
		If TypeOf (sender) Is CheckBox Then
			Dim chk As CheckBox = DirectCast(sender, CheckBox)
			chk.BackColor = Color.Gray
		End If
	End Sub

	Private Sub Checkbox_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chk_ESKST2.Leave, Chk_ESKST1.Leave, Chk_ESBerater.Leave
		If TypeOf (sender) Is CheckBox Then
			Dim chk As CheckBox = DirectCast(sender, CheckBox)
			chk.BackColor = Color.Transparent
		End If
	End Sub

	Private Sub Link_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles LibESEinsatzAls.PreviewKeyDown, LibESGAVBeruf.PreviewKeyDown, LibESBranche.PreviewKeyDown,
		LibKDKreditlimite1.PreviewKeyDown, LibKDKreditlimite2.PreviewKeyDown

		If e.KeyCode = Keys.Space Or e.KeyCode = Keys.F4 Then
			Dim link As LinkLabel = DirectCast(sender, LinkLabel)
			Select Case link.Tag
				Case Lst_ESEinsatzAls.Name
					LnkESEinsatzAls_Click(sender, e)
				Case Lst_ESBranche.Name
					LnkESBranche_Click(sender, e)
				Case Lst_ESGAVBeruf.Name
					LnkESGAVBeruf_Click(sender, e)

			End Select

		ElseIf e.KeyCode = Keys.Enter Then
			KeyPressEvent(sender, New System.Windows.Forms.KeyPressEventArgs(keyChar:=ChrW(13)))
		End If

	End Sub

	''' <summary>
	''' Selektionsfenster für die 1.Kreditlimite des Kunden öffnen.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub LnkKDKreditlimite1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LibKDKreditlimite1.Click
		Dim frmTest As New frmSearchRec("1. KD-Kredit")

		Dim m As String
		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iESValue(_ClsFunc.GetSelektion)
		If m Is Nothing Then Return
		If m.ToString <> String.Empty Then
			Dim strEintrag As String() = Regex.Split(m.ToString, strValueSeprator)
			For i As Integer = 0 To strEintrag.Count - 1
				If Not Me.Lst_KDKreditlimite1.Items.Contains(strEintrag(i)) Then
					Me.Lst_KDKreditlimite1.Items.Add(strEintrag(i))
				End If
			Next
		End If
		frmTest.Dispose()

	End Sub


	''' <summary>
	''' Selektionsfenster für die 2.Kreditlimite des Kunden öffnen.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub LnkKDKreditlimite2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LibKDKreditlimite2.Click
		Dim frmTest As New frmSearchRec("2. KD-Kredit")

		Dim m As String
		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iESValue(_ClsFunc.GetSelektion)
		If m Is Nothing Then Return
		If Not m Is Nothing Then

			If m.ToString <> String.Empty Then
				Dim strEintrag As String() = Regex.Split(m.ToString, strValueSeprator)
				For i As Integer = 0 To strEintrag.Count - 1
					If Not Me.Lst_KDKreditlimite2.Items.Contains(strEintrag(i)) Then
						Me.Lst_KDKreditlimite2.Items.Add(strEintrag(i))
					End If
				Next
			End If
		End If

		frmTest.Dispose()

	End Sub


	Private Sub Lst_ESGAVBeruf_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Lst_ESGAVBeruf.SelectedIndexChanged
		Me.Cbo_ESGruppe1.Enabled = Me.Lst_ESGAVBeruf.Items.Count = 0
	End Sub

	Private Sub Cbo_ESGruppe1_DropDownClosed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_ESGruppe1.QueryCloseUp, Cbo_ESGruppe1.TextChanged
		' GAV-Berufe deaktivieren, wenn GAVGruppe1 ausgewählt ist
		If Me.Cbo_ESGruppe1.Text.Length > 0 Then
			Me.LibESGAVBeruf.Enabled = False
			Me.Lst_ESGAVBeruf.Enabled = False
		Else
			Me.LibESGAVBeruf.Enabled = True
			Me.Lst_ESGAVBeruf.Enabled = True
		End If
	End Sub





	' Private Sub mnuESWithGAV_Click(sender As System.Object, e As System.EventArgs)
	'	ClsDataDetail.ShowBewilligStatistik = True
	'	Me.PrintJobNr = "4.0"
	'	GetESData4Print(False, "4.0")	 ' Einsatzliste mit GAV-Details
	'End Sub


#Region "Switch button"

	Private Sub SwitchButton1_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton1.ValueChanged
		Me.txtESESNr_2.Visible = Me.SwitchButton1.Value
		Me.txtESESNr_2.Text = String.Empty
	End Sub

	Private Sub SwitchButton2_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton2.ValueChanged
		Me.txtESMANr_2.Visible = Me.SwitchButton2.Value
		Me.txtESMANr_2.Text = String.Empty
	End Sub

	Private Sub SwitchButton3_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton3.ValueChanged
		Me.txtESKDNr_2.Visible = Me.SwitchButton3.Value
		Me.txtESKDNr_2.Text = String.Empty
	End Sub


#End Region


	Private Sub chkMABew_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkMABew.CheckedChanged
		If Me.chkMABew.Checked Then
			Me.Cbo_MABewBisMonat.Text = String.Empty
			Me.Cbo_MABewBisJahr.Text = String.Empty
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


#End Region


End Class


