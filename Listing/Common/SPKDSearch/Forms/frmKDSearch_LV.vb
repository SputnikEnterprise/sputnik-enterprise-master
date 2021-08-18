
Option Strict Off

Imports System.Reflection

Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo

Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports System.Threading

Imports DevExpress.LookAndFeel
Imports System.IO

Imports SPProgUtility.ClsMDData
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities

Imports SP.KD.CPersonMng.UI
Imports SPProgUtility.CommonXmlUtility
Imports SP.Infrastructure.UI

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SP.Infrastructure.Logging

Public Class frmKDSearch_LV
	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private m_xml As New ClsXML
	Private _ClsFunc As New ClsDivFunc

	Dim strLastSortBez As String

	Dim _FarbenHauptMaske As ArrayList = New ArrayList()


	Private m_utilityUI As UtilityUI

	Private m_utility As Utilities
	Private m_md As Mandant

	Private m_GridSettingPath As String
	Private UserGridSettingsXml As SettingsXml

	Private m_GVSearchSettingfilename As String

	Private m_xmlSettingRestoreSearchSetting As String
	Private m_xmlSettingSearchFilter As String


	Public Property RecCount As Integer
	Private Property Sql2Open As String
	Private m_ShowcustomerinColor As Boolean



#Region "Private Consts"


	Private Const MODUL_NAME_SETTING = "customersearch"

	Private Const USER_XML_SETTING_SPUTNIK_SEARCH_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/customersearch/{1}/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_SEARCH_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/customersearch/{1}/keepfilter"


#End Region


#Region "Constructor..."

	Public Sub New(ByVal strQuery As String)

		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		m_md = New Mandant
		m_utility = New Utilities
		m_utilityUI = New UtilityUI

		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(ClsDataDetail.TranslationData, ClsDataDetail.ProsonalizedData)
		Try
			m_GridSettingPath = String.Format("{0}CustomerSearch\", m_md.GetGridSettingPath(ClsDataDetail.MDData.MDNr))
			If Not Directory.Exists(m_GridSettingPath) Then Directory.CreateDirectory(m_GridSettingPath)

			m_GVSearchSettingfilename = String.Format("{0}{1}{2}.xml", m_GridSettingPath, Me.grdRP.Name, ClsDataDetail.UserData.UserNr)

			m_xmlSettingRestoreSearchSetting = String.Format(USER_XML_SETTING_SPUTNIK_SEARCH_GRIDSETTING_RESTORE, ClsDataDetail.UserData.UserNr, MODUL_NAME_SETTING)
			m_xmlSettingSearchFilter = String.Format(USER_XML_SETTING_SPUTNIK_SEARCH_GRIDSETTING_FILTER, ClsDataDetail.UserData.UserNr, MODUL_NAME_SETTING)

			UserGridSettingsXml = New SettingsXml(m_md.GetAllUserGridSettingXMLFilename(ClsDataDetail.MDData.MDNr))

			m_ShowcustomerinColor = ShowCustomerRecordsInColor

		Catch ex As Exception

		End Try


		Me.pnlMain.Dock = DockStyle.Fill
		Me.Sql2Open = strQuery

		ResetGridCustomerData()

	End Sub

#End Region


#Region "Private property"

	Private ReadOnly Property ShowCustomerRecordsInColor() As Boolean
		Get
			Dim FORM_XML_MAIN_KEY As String = "UserProfile/programsetting"
			Dim m_path As New SPProgUtility.ProgPath.ClsProgPath
			Dim mdNumber As Integer = 0
			Dim userNumber As Integer = 0
			mdNumber = ClsDataDetail.MDData.MDNr
			userNumber = ClsDataDetail.UserData.UserNr

			Dim UserXMLFileName = m_md.GetSelectedMDUserProfileXMLFilename(mdNumber, userNumber)
			Dim value As Boolean? = StrToBool(m_path.GetXMLNodeValue(UserXMLFileName, String.Format("{0}/showcustomerrecordsincolor", FORM_XML_MAIN_KEY)))


			Return value
		End Get

	End Property

#End Region



	''' <summary>
	''' Gets the selected employee.
	''' </summary>
	''' <returns>The selected employee or nothing if none is selected.</returns>
	Public ReadOnly Property SelectedRecord As FoundedData
		Get
			Dim gvRP = TryCast(grdRP.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvRP Is Nothing) Then

				Dim selectedRows = gvRP.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim employee = CType(gvRP.GetRow(selectedRows(0)), FoundedData)
					Return employee
				End If

			End If

			Return Nothing
		End Get

	End Property

	Function GetDbCustomerData4Show() As IEnumerable(Of FoundedData)
		Dim result As List(Of FoundedData) = Nothing
		m_utility = New Utilities

		Dim sql As String

		sql = Sql2Open

		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ClsDataDetail.GetSelectedMDConnstring, sql, Nothing)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of FoundedData)

				While reader.Read()
					Dim overviewData As New FoundedData

					overviewData.KDNr = m_utility.SafeGetInteger(reader, "KDNr", 0)
					overviewData.KDZNr = m_utility.SafeGetInteger(reader, "zhdrecnr", 0)
					overviewData.customerfproperty = m_utility.SafeGetInteger(reader, "FProperty", 0)

					overviewData.customername = String.Format("{0}", m_utility.SafeGetString(reader, "firma1"))
					overviewData.customername2 = String.Format("{0}", m_utility.SafeGetString(reader, "kdfirma2"))
					overviewData.customername3 = String.Format("{0}", m_utility.SafeGetString(reader, "kdfirma3"))

					overviewData.customerstreet = String.Format("{0}", m_utility.SafeGetString(reader, "kdstrasse"))
					overviewData.customeraddress = String.Format("{0} {1}", m_utility.SafeGetString(reader, "kdplz"), m_utility.SafeGetString(reader, "kdort"))
					overviewData.customerpostcode = String.Format("{0}", m_utility.SafeGetString(reader, "kdplz"))
					overviewData.customercity = String.Format("{0}", m_utility.SafeGetString(reader, "kdort"))
					overviewData.customerbox = String.Format("{0}", m_utility.SafeGetString(reader, "kdpostfach"))

					overviewData.customertelefon = String.Format("{0}", m_utility.SafeGetString(reader, "kdtelefon"))
					overviewData.customertelefax = String.Format("{0}", m_utility.SafeGetString(reader, "kdtelefax"))
					overviewData.customeremail = String.Format("{0}", m_utility.SafeGetString(reader, "KDeMail"))

					overviewData.kreditlimite = m_utility.SafeGetDecimal(reader, "kdkreditlimite", 0)
					overviewData.kreditlimite2 = m_utility.SafeGetDecimal(reader, "kdkreditlimite_2", 0)
					overviewData.kreditab = m_utility.SafeGetDateTime(reader, "kdkreditlimiteab", Nothing)
					overviewData.kreditbis = m_utility.SafeGetDateTime(reader, "kdkreditlimitebis", Nothing)
					overviewData.kreditrefnr = String.Format("{0}", m_utility.SafeGetString(reader, "kredit_refnr"))
					overviewData.kreditwarning = m_utility.SafeGetBoolean(reader, "kreditwarnung", False)
					overviewData.customerumsatzmin = m_utility.SafeGetDecimal(reader, "kd_umsmin", 0)

					overviewData.cresponsiblefullname = String.Format("{0}, {1}", m_utility.SafeGetString(reader, "nachname"), m_utility.SafeGetString(reader, "vorname"))
					overviewData.cresponsibletelefon = String.Format("{0}", m_utility.SafeGetString(reader, "zhdtelefon"))
					overviewData.cresponsibletelefax = String.Format("{0}", m_utility.SafeGetString(reader, "zhdtelefax"))
					overviewData.cresponsiblemobil = String.Format("{0}", m_utility.SafeGetString(reader, "zhdnatel"))
					overviewData.cresponsibleemail = String.Format("{0}", m_utility.SafeGetString(reader, "zhdemail"))

					overviewData.cresponsibleabteilung = m_utility.SafeGetString(reader, "zhdabt")
					overviewData.cresponsibleposition = String.Format("{0}", m_utility.SafeGetString(reader, "zhdpos"))

					overviewData.customeradvisor = String.Format("{0}", m_utility.SafeGetString(reader, "KDBeraterFullname"))
					overviewData.cresponsibleadvisor = String.Format("{0}", m_utility.SafeGetString(reader, "ZHDBeraterFullname"))

					overviewData.cresponsiblefstate = m_utility.SafeGetString(reader, "kdzstate1")
					overviewData.cresponsiblesstate = m_utility.SafeGetString(reader, "kdzstate2")


					result.Add(overviewData)

				End While

			End If

		Catch e As Exception
			result = Nothing
			m_Logger.LogError(e.ToString())
			m_utilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Fehler in der Abfrage.{0}{1}"), vbNewLine, e.ToString))

		Finally
			m_utility.CloseReader(reader)

		End Try

		Return result
	End Function

	Private Function LoadFoundedCustomerList() As Boolean

		Dim listOfEmployees = GetDbCustomerData4Show()
		If listOfEmployees Is Nothing Then
			m_utilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Fehler in der Abfrage.{0}{0}Es können keine Daten aufgelistet werden.{0}Mögliche timeout auf dem Server!"), vbNewLine))
			Return False
		End If

		Dim responsiblePersonsGridData = (From person In listOfEmployees
																			Select New FoundedData With
					 {.KDNr = person.KDNr, .KDZNr = person.KDZNr,
						.customerfproperty = person.customerfproperty,
						.customername = person.customername,
						.customername2 = person.customername2,
						.customername3 = person.customername3,
						.customerstreet = person.customerstreet,
						.customeraddress = person.customeraddress,
					.customerpostcode = person.customerpostcode,
					.customercity = person.customercity,
					 .customerbox = person.customerbox,
					 .customertelefon = person.customertelefon,
					 .customertelefax = person.customertelefax,
					 .customeremail = person.customeremail,
					 .kreditlimite = person.kreditlimite,
					 .kreditlimite2 = person.kreditlimite2,
					 .kreditab = person.kreditab,
					 .kreditbis = person.kreditbis,
					 .kreditrefnr = person.kreditrefnr,
					 .kreditwarning = person.kreditwarning,
						.customerumsatzmin = person.customerumsatzmin,
								.cresponsiblefullname = person.cresponsiblefullname,
						.cresponsibletelefon = person.cresponsibletelefon,
						.cresponsibletelefax = person.cresponsibletelefax,
						.cresponsiblemobil = person.cresponsiblemobil,
						.cresponsibleemail = person.cresponsibleemail,
						.cresponsibleabteilung = person.cresponsibleabteilung,
						.cresponsibleposition = person.cresponsibleposition,
						.customeradvisor = person.customeradvisor,
						.cresponsibleadvisor = person.cresponsibleadvisor,
						.cresponsiblefstate = person.cresponsiblefstate,
						.cresponsiblesstate = person.cresponsiblesstate
}).ToList()

		Dim listDataSource As BindingList(Of FoundedData) = New BindingList(Of FoundedData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdRP.DataSource = listDataSource

		Return Not listOfEmployees Is Nothing
	End Function

	Private Sub ResetGridCustomerData()

		gvRP.OptionsView.ShowIndicator = False
		gvRP.OptionsView.ShowAutoFilterRow = True
		gvRP.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never

		gvRP.Columns.Clear()

		Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMANr.Caption = m_xml.GetSafeTranslationValue("Kunden-Nr.")
		columnMANr.Name = "KDNr"
		columnMANr.FieldName = "KDNr"
		columnMANr.Visible = False
		gvRP.Columns.Add(columnMANr)

		Dim columnzNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnzNr.Caption = m_xml.GetSafeTranslationValue("ZHD-Nr.")
		columnzNr.Name = "KDZNr"
		columnzNr.FieldName = "KDZNr"
		columnzNr.Visible = False
		gvRP.Columns.Add(columnzNr)

		Dim columncustomername As New DevExpress.XtraGrid.Columns.GridColumn()
		columncustomername.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncustomername.Caption = m_xml.GetSafeTranslationValue("Kunde")
		columncustomername.Name = "customername"
		columncustomername.FieldName = "customername"
		columncustomername.BestFit()
		columncustomername.Visible = True
		gvRP.Columns.Add(columncustomername)

		Dim columncustomername2 As New DevExpress.XtraGrid.Columns.GridColumn()
		columncustomername2.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncustomername2.Caption = m_xml.GetSafeTranslationValue("2. Adresszeile")
		columncustomername2.Name = "customername2"
		columncustomername2.FieldName = "customername2"
		columncustomername2.BestFit()
		columncustomername2.Visible = False
		gvRP.Columns.Add(columncustomername2)

		Dim columncustomername3 As New DevExpress.XtraGrid.Columns.GridColumn()
		columncustomername3.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncustomername3.Caption = m_xml.GetSafeTranslationValue("3. Adresszeile")
		columncustomername3.Name = "customername3"
		columncustomername3.FieldName = "customername3"
		columncustomername3.BestFit()
		columncustomername3.Visible = False
		gvRP.Columns.Add(columncustomername3)

		Dim columncustomerstreet As New DevExpress.XtraGrid.Columns.GridColumn()
		columncustomerstreet.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncustomerstreet.Caption = m_xml.GetSafeTranslationValue("Strasse")
		columncustomerstreet.Name = "customerstreet"
		columncustomerstreet.FieldName = "customerstreet"
		columncustomerstreet.BestFit()
		columncustomerstreet.Visible = False
		gvRP.Columns.Add(columncustomerstreet)

		Dim columnEcustomeradress As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEcustomeradress.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnEcustomeradress.Caption = m_xml.GetSafeTranslationValue("Adresse")
		columnEcustomeradress.Name = "customeradress"
		columnEcustomeradress.FieldName = "customeraddress"
		columnEcustomeradress.BestFit()
		columnEcustomeradress.Visible = True
		gvRP.Columns.Add(columnEcustomeradress)

		Dim columncustomertelefon As New DevExpress.XtraGrid.Columns.GridColumn()
		columncustomertelefon.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncustomertelefon.Caption = m_xml.GetSafeTranslationValue("Telefon")
		columncustomertelefon.Name = "customertelefon"
		columncustomertelefon.FieldName = "customertelefon"
		columncustomertelefon.BestFit()
		columncustomertelefon.Visible = True
		gvRP.Columns.Add(columncustomertelefon)

		Dim columncustomertelefax As New DevExpress.XtraGrid.Columns.GridColumn()
		columncustomertelefax.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncustomertelefax.Caption = m_xml.GetSafeTranslationValue("Telefax")
		columncustomertelefax.Name = "customertelefax"
		columncustomertelefax.FieldName = "customertelefax"
		columncustomertelefax.BestFit()
		columncustomertelefax.Visible = False
		gvRP.Columns.Add(columncustomertelefax)

		Dim columncustomeremail As New DevExpress.XtraGrid.Columns.GridColumn()
		columncustomeremail.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncustomeremail.Caption = m_xml.GetSafeTranslationValue("E-Mail")
		columncustomeremail.Name = "customeremail"
		columncustomeremail.FieldName = "customeremail"
		columncustomeremail.BestFit()
		columncustomeremail.Visible = False
		gvRP.Columns.Add(columncustomeremail)

		Dim columnkreditlimite As New DevExpress.XtraGrid.Columns.GridColumn()
		columnkreditlimite.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnkreditlimite.Caption = m_xml.GetSafeTranslationValue("Kreditlimite")
		columnkreditlimite.Name = "kreditlimite"
		columnkreditlimite.FieldName = "kreditlimite"
		columnkreditlimite.BestFit()
		columnkreditlimite.Visible = False
		gvRP.Columns.Add(columnkreditlimite)

		Dim columnkreditab As New DevExpress.XtraGrid.Columns.GridColumn()
		columnkreditab.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnkreditab.Caption = m_xml.GetSafeTranslationValue("Ab")
		columnkreditab.Name = "kreditab"
		columnkreditab.FieldName = "kreditab"
		columnkreditab.BestFit()
		columnkreditab.Visible = False
		gvRP.Columns.Add(columnkreditab)

		Dim columnkreditbis As New DevExpress.XtraGrid.Columns.GridColumn()
		columnkreditbis.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnkreditbis.Caption = m_xml.GetSafeTranslationValue("Bis")
		columnkreditbis.Name = "kreditbis"
		columnkreditbis.FieldName = "kreditbis"
		columnkreditbis.BestFit()
		columnkreditbis.Visible = False
		gvRP.Columns.Add(columnkreditbis)

		Dim columnresponsiblefullname As New DevExpress.XtraGrid.Columns.GridColumn()
		columnresponsiblefullname.Caption = m_xml.GetSafeTranslationValue("Zuständige Person")
		columnresponsiblefullname.Name = "cresponsiblefullname"
		columnresponsiblefullname.FieldName = "cresponsiblefullname"
		columnresponsiblefullname.Visible = True
		columnresponsiblefullname.BestFit()
		gvRP.Columns.Add(columnresponsiblefullname)

		Dim columncresponsibletelefon As New DevExpress.XtraGrid.Columns.GridColumn()
		columncresponsibletelefon.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncresponsibletelefon.Caption = m_xml.GetSafeTranslationValue("ZHD Telefon")
		columncresponsibletelefon.Name = "cresponsibletelefon"
		columncresponsibletelefon.FieldName = "cresponsibletelefon"
		columncresponsibletelefon.Visible = True
		columncresponsibletelefon.BestFit()
		gvRP.Columns.Add(columncresponsibletelefon)

		Dim columncresponsibletelefax As New DevExpress.XtraGrid.Columns.GridColumn()
		columncresponsibletelefax.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncresponsibletelefax.Caption = m_xml.GetSafeTranslationValue("ZHD Telefax")
		columncresponsibletelefax.Name = "cresponsibletelefax"
		columncresponsibletelefax.FieldName = "cresponsibletelefax"
		columncresponsibletelefax.Visible = False
		columncresponsibletelefax.BestFit()
		gvRP.Columns.Add(columncresponsibletelefax)

		Dim columncresponsiblemobil As New DevExpress.XtraGrid.Columns.GridColumn()
		columncresponsiblemobil.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncresponsiblemobil.Caption = m_xml.GetSafeTranslationValue("ZHD Natel")
		columncresponsiblemobil.Name = "cresponsiblemobil"
		columncresponsiblemobil.FieldName = "cresponsiblemobil"
		columncresponsiblemobil.Visible = True
		columncresponsiblemobil.BestFit()
		gvRP.Columns.Add(columncresponsiblemobil)

		Dim columncresponsibleemail As New DevExpress.XtraGrid.Columns.GridColumn()
		columncresponsibleemail.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncresponsibleemail.Caption = m_xml.GetSafeTranslationValue("ZHD E-Mail")
		columncresponsibleemail.Name = "cresponsibleemail"
		columncresponsibleemail.FieldName = "cresponsibleemail"
		columncresponsibleemail.Visible = True
		columncresponsibleemail.BestFit()
		gvRP.Columns.Add(columncresponsibleemail)


		Dim columncustomeradvisor As New DevExpress.XtraGrid.Columns.GridColumn()
		columncustomeradvisor.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncustomeradvisor.Caption = m_xml.GetSafeTranslationValue("KD Berater")
		columncustomeradvisor.Name = "customeradvisor"
		columncustomeradvisor.FieldName = "customeradvisor"
		columncustomeradvisor.Visible = False
		columncustomeradvisor.BestFit()
		gvRP.Columns.Add(columncustomeradvisor)

		Dim columncresponsibleadvisor As New DevExpress.XtraGrid.Columns.GridColumn()
		columncresponsibleadvisor.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncresponsibleadvisor.Caption = m_xml.GetSafeTranslationValue("ZHD Berater")
		columncresponsibleadvisor.Name = "cresponsibleadvisor"
		columncresponsibleadvisor.FieldName = "cresponsibleadvisor"
		columncresponsibleadvisor.Visible = True
		columncresponsibleadvisor.BestFit()
		gvRP.Columns.Add(columncresponsibleadvisor)

		Dim columnkreditrefnr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnkreditrefnr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnkreditrefnr.Caption = m_xml.GetSafeTranslationValue("Referenznummer")
		columnkreditrefnr.Name = "kreditrefnr"
		columnkreditrefnr.FieldName = "kreditrefnr"
		columnkreditrefnr.Visible = False
		columnkreditrefnr.BestFit()
		gvRP.Columns.Add(columnkreditrefnr)

		Dim columnkreditwarning As New DevExpress.XtraGrid.Columns.GridColumn()
		columnkreditwarning.Caption = m_xml.GetSafeTranslationValue("Kreditwarnung")
		columnkreditwarning.Name = "kreditwarning"
		columnkreditwarning.FieldName = "kreditwarning"
		columnkreditwarning.Visible = False
		columnkreditwarning.BestFit()
		gvRP.Columns.Add(columnkreditwarning)

		Dim columncustomerumsatzmin As New DevExpress.XtraGrid.Columns.GridColumn()
		columncustomerumsatzmin.Caption = m_xml.GetSafeTranslationValue("Umsatzminderung")
		columncustomerumsatzmin.Name = "customerumsatzmin"
		columncustomerumsatzmin.FieldName = "customerumsatzmin"
		columncustomerumsatzmin.Visible = False
		columncustomerumsatzmin.BestFit()
		gvRP.Columns.Add(columncustomerumsatzmin)

		Dim columncresponsibleabteilung As New DevExpress.XtraGrid.Columns.GridColumn()
		columncresponsibleabteilung.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncresponsibleabteilung.Caption = m_xml.GetSafeTranslationValue("Abteilung")
		columncresponsibleabteilung.Name = "cresponsibleabteilung"
		columncresponsibleabteilung.FieldName = "cresponsibleabteilung"
		columncresponsibleabteilung.Visible = False
		gvRP.Columns.Add(columncresponsibleabteilung)

		Dim columncresponsibleposition As New DevExpress.XtraGrid.Columns.GridColumn()
		columncresponsibleposition.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncresponsibleposition.Caption = m_xml.GetSafeTranslationValue("Position")
		columncresponsibleposition.Name = "cresponsibleposition"
		columncresponsibleposition.FieldName = "cresponsibleposition"
		columncresponsibleposition.Visible = False
		gvRP.Columns.Add(columncresponsibleposition)

		Dim columncresponsiblefstate As New DevExpress.XtraGrid.Columns.GridColumn()
		columncresponsiblefstate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncresponsiblefstate.Caption = m_xml.GetSafeTranslationValue("1. Status", True)
		columncresponsiblefstate.Name = "cresponsiblefstate"
		columncresponsiblefstate.FieldName = "cresponsiblefstate"
		columncresponsiblefstate.Visible = False
		gvRP.Columns.Add(columncresponsiblefstate)

		Dim columncresponsiblesstate As New DevExpress.XtraGrid.Columns.GridColumn()
		columncresponsiblesstate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncresponsiblesstate.Caption = m_xml.GetSafeTranslationValue("2. Status", True)
		columncresponsiblesstate.Name = "cresponsiblesstate"
		columncresponsiblesstate.FieldName = "cresponsiblesstate"
		columncresponsiblesstate.Visible = False
		gvRP.Columns.Add(columncresponsiblesstate)

		Dim columncustomerfproperty As New DevExpress.XtraGrid.Columns.GridColumn()
		columncustomerfproperty.Caption = m_xml.GetSafeTranslationValue("1. Eigenschaft", True)
		columncustomerfproperty.Name = "customerfproperty"
		columncustomerfproperty.FieldName = "customerfproperty"
		columncustomerfproperty.Visible = False
		gvRP.Columns.Add(columncustomerfproperty)

		RestoreGridLayoutFromXml()

		grdRP.DataSource = Nothing


	End Sub


	Private Sub frmSearchKD_LV_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.frm_LVLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.ifrmLVHeight = Me.Height
				My.Settings.ifrmLVWidth = Me.Width

				My.Settings.Save()
			End If


		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub

	Sub TranslateControls()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Me.Text = m_xml.GetSafeTranslationValue(Me.Text)
			m_xml.GetChildChildBez(Me)
			Me.bsiInfo.Caption = m_xml.GetSafeTranslationValue(Me.bsiInfo.Caption)
			bbiPrintList.Caption = m_xml.GetSafeTranslationValue(Me.bbiPrintList.Caption)

		Catch ex As Exception
			m_Logger.LogError(String.Format("1=>{0}.{1}", strMethodeName, ex.Message))
		End Try

	End Sub

	Private Sub frmOnLoad(sender As Object, e As System.EventArgs) Handles Me.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim m_md As New Mandant

		Dim strStyleName As String = m_md.GetSelectedUILayoutName(ClsDataDetail.ProgSettingData.SelectedMDNr, 0, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		TranslateControls()
		Try
			Me.Width = Math.Max(My.Settings.ifrmLVWidth, Me.Width)
			Me.Height = Math.Max(My.Settings.ifrmLVHeight, Me.Height)

			If My.Settings.frm_LVLocation <> String.Empty Then
				Dim aLoc As String() = My.Settings.frm_LVLocation.Split(CChar(";"))

				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = CStr(0)
				End If
				Me.Location = New System.Drawing.Point(CInt(Math.Max(Val(aLoc(0)), 0)), CInt(Math.Max(Val(aLoc(1)), 0)))
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.Message))

		End Try

		Try
			LoadFoundedCustomerList()

			Me.RecCount = gvRP.RowCount
			Me.bsiInfo.Caption = String.Format("Anzahl Datensätze: {0}", Me.RecCount)

			AddHandler gvRP.RowCellClick, AddressOf Ongv_RowCellClick

			AddHandler Me.gvRP.RowCountChanged, AddressOf OngvMain_RowCountChanged
			AddHandler Me.gvRP.ColumnPositionChanged, AddressOf OngvColumnPositionChanged
			AddHandler Me.gvRP.ColumnWidthChanged, AddressOf OngvColumnPositionChanged

		Catch ex As Exception

		End Try

	End Sub


	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvRP.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, FoundedData)

				Select Case column.Name.ToLower
					Case "customertelefon"
						If viewData.customertelefon.Length > 0 Then StartCalling(viewData.customertelefon) ' RunTapi_KDZhd(viewData.customertelefon, viewData.KDNr, 0)
					Case "customeremail"
						If viewData.customeremail.Length > 0 Then MailTo(viewData.customeremail)

					Case "cresponsibletelefon"
						If viewData.cresponsibletelefon.Length > 0 Then StartCalling(viewData.cresponsibletelefon) ', viewData.KDNr, viewData.KDZNr)
					Case "cresponsiblemobil"
						If viewData.cresponsiblemobil.Length > 0 Then StartCalling(viewData.cresponsiblemobil) ', viewData.KDNr, viewData.KDZNr)
					Case "customeremail"
						If viewData.cresponsibleemail.Length > 0 Then MailTo(viewData.cresponsibleemail)

					Case "cresponsiblefullname"
						If viewData.cresponsiblefullname.Length > 0 Then
							Dim obj As New ThreadTesting.OpenFormsWithThreading(viewData.KDNr, viewData.KDZNr, String.Empty)
							obj._OpenKDZHD()
						End If

					Case Else
						If viewData.KDNr > 0 Then
							Dim obj As New ThreadTesting.OpenFormsWithThreading(viewData.KDNr, viewData.KDZNr, String.Empty)
							obj._OpenKD()
						End If

				End Select

			End If

		End If

	End Sub

	Private Sub StartCalling(ByVal number As String)

		Dim oMyProg As New SPSTapi.UI.frmCaller(ClsDataDetail.m_InitialData)

		oMyProg.LoadData(number)
		oMyProg.Show()
		oMyProg.BringToFront()

	End Sub

	''' <summary>
	'''  Handles RowStyle event of main grid view.
	''' </summary>
	Private Sub OngvRPn_RowStyle(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs) Handles gvRP.RowStyle

		If Not m_ShowcustomerinColor Then Return
		If e.RowHandle >= 0 Then

			Dim rowData = CType(gvRP.GetRow(e.RowHandle), FoundedData)

			If rowData.customerfproperty > 0 Then
				e.Appearance.ForeColor = ColorTranslator.FromWin32(rowData.customerfproperty)
			End If

		End If

	End Sub

	Private Sub bbiPrintList_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrintList.ItemClick
		If gvRP.RowCount > 0 Then
			' Opens the Preview window. 
			grdRP.ShowPrintPreview()
		End If

	End Sub

	Private Sub OngvMain_RowCountChanged(sender As Object, e As EventArgs)

		Me.bsiInfo.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True
		Me.bsiInfo.Caption = String.Format(m_xml.GetSafeTranslationValue("Anzahl Datensätze: {0}"), Me.gvRP.RowCount)

	End Sub

	Private Sub OngvColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvRP.SaveLayoutToXml(m_GVSearchSettingfilename)

	End Sub

	Private Sub RestoreGridLayoutFromXml()
		Dim keepFilter = False
		Dim restoreLayout = True

		Try
			restoreLayout = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreSearchSetting), True)
			keepFilter = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingSearchFilter), False)
		Catch ex As Exception

		End Try
		If File.Exists(m_GVSearchSettingfilename) Then gvRP.RestoreLayoutFromXml(m_GVSearchSettingfilename)

		If restoreLayout AndAlso Not keepFilter Then gvRP.ActiveFilterCriteria = Nothing

	End Sub



#Region "Helpers"

	Private Function StrToBool(ByVal str As String) As Boolean

		Dim result As Boolean = False

		If String.IsNullOrWhiteSpace(str) Then
			Return False
		ElseIf str = "1" Then
			Return True

		End If

		Boolean.TryParse(str, result)

		Return result
	End Function


#End Region


End Class



Namespace ThreadTesting


	Public Class OpenFormsWithThreading

		''' <summary>
		''' The logger.
		''' </summary>
		Private m_Logger As ILogger = New Logger()
		Private _ClsReg As New SPProgUtility.ClsDivReg

		Private Property SelectedKDNr As Integer
		Private Property SelectedKDZHDNr As Integer
		Private Property SelectedTelNr As String
		Private Property SQL2Open As String
		Private m_utility As New Utilities


		Sub _OpenKD()
			Dim iKDNr As Integer = Me.SelectedKDNr

			Try
				Dim hub = MessageService.Instance.Hub
				Dim openMng As New OpenCustomerMngRequest(Me, ClsDataDetail.UserData.UserNr, ClsDataDetail.MDData.MDNr, iKDNr)
				hub.Publish(openMng)


				'Dim frm_Kunden As New frmCustomers(CreateInitialData(0, 0))
				'If iKDNr > 0 Then
				'	frm_Kunden.LoadCustomerData(iKDNr)
				'Else
				'	frm_Kunden.LoadCustomerData(Nothing)

				'End If
				'frm_Kunden.Show()

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

		End Sub

		Sub _OpenKDZHD()
			Dim iKDNr As Integer = Me.SelectedKDNr
			Dim iKDZhdNr As Integer = Me.SelectedKDZHDNr

			Try

				Dim responsiblePersonsFrom = New frmResponsiblePerson(CreateInitialData(ClsDataDetail.MDData.MDNr, ClsDataDetail.UserData.UserNr))

				If (responsiblePersonsFrom.LoadResponsiblePersonData(iKDNr, iKDZhdNr)) Then
					responsiblePersonsFrom.Show()
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

		End Sub

		'Private Sub OpenKDTapiWithThreading()
		'	Dim iKDNr As Integer = Me.SelectedKDNr
		'	Dim iKDZhdNr As Integer = Me.SelectedKDZHDNr

		'	Dim strTranslationProgName As String = String.Empty
		'	Try
		'		Dim oMyProg As New SPSTapi.ClsMain_Net
		'		Dim iTest As Integer = oMyProg.ShowfrmTapi(Me.SelectedTelNr, 0, Me.SelectedKDNr, 0, 0)

		'	Catch e As Exception
		'		MsgBox(e.Message, MsgBoxStyle.Critical, "OpenKDTapi")

		'	End Try

		'End Sub

		'Private Sub OpenZHDTapiWithThreading()
		'	Dim iKDNr As Integer = Me.SelectedKDNr
		'	Dim iKDZhdNr As Integer = Me.SelectedKDZHDNr

		'	Try
		'		Dim oMyProg As New SPSTapi.ClsMain_Net
		'		Dim iTest As Integer = oMyProg.ShowfrmTapi(Me.SelectedTelNr, 0, Me.SelectedKDNr, Me.SelectedKDZHDNr.ToString, 0)

		'	Catch e As Exception
		'		MsgBox(e.Message, MsgBoxStyle.Critical, "OpenZHDTapi")

		'	End Try

		'End Sub


		'Sub _OpenOffMailingModul()
		'	Dim t As Thread = New Thread(AddressOf OpenOffMailingWithThreading)

		'	t.IsBackground = True
		'	t.Name = "OpenOffMailingWithThreading"
		'	't.SetApartmentState(ApartmentState.STA)
		'	t.Start()

		'End Sub

		'Private Sub OpenOffMailingWithThreading()
		'	Dim iKDNr As Integer = Me.SelectedKDNr
		'	Dim iKDZhdNr As Integer = Me.SelectedKDZHDNr

		'	Dim strTranslationProgName As String = String.Empty
		'	Try
		'		Dim ProgObj As New SPSOfferUtility_Net.ClsMain_Net
		'		ProgObj.ShowMainForm(Me.SQL2Open)

		'	Catch e As Exception
		'		MsgBox(e.Message, MsgBoxStyle.Critical, "OpenOffMailing")

		'	End Try

		'End Sub

		Sub _OpenBewForm(ByVal _SQL As String)
			Me.SQL2Open = _SQL
			Dim t As Thread = New Thread(AddressOf OpenBewFormWithThreading)

			t.IsBackground = True
			t.Name = "OpenESFormWithThreading"
			't.SetApartmentState(ApartmentState.STA)
			t.Start()

		End Sub

		Private Sub OpenBewFormWithThreading()
			Dim oMyProg As Object
			Dim strTranslationProgName As String = String.Empty

			Try
				strTranslationProgName = m_utility.GetMyDocumentsPathWithBackSlash & "SPTranslationProg" & ClsDataDetail.ProgSettingData.LogedUSNr
				_ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSBewUtility.ClsMain")
				_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", Me.SQL2Open)

				oMyProg = CreateObject("SPSBewUtility.ClsMain")
				oMyProg.OpenKDFieldsform(Me.SQL2Open)

			Catch e As Exception
				MsgBox(e.Message, MsgBoxStyle.Critical, "OpenBewForm")

			End Try

		End Sub






		Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, ByVal value As T) As T
			target = value

			Return value
		End Function

		Public Sub New(ByVal _iKDNr As Integer, _
									 ByVal _iZHDnr As Integer, _
									 ByVal _strTelNr As String)
			Me.SelectedKDNr = _iKDNr
			Me.SelectedKDZHDNr = _iZHDnr
			Me.SelectedTelNr = _strTelNr
		End Sub

		Public Sub New(ByVal _strSQL As String)
			Me.SQL2Open = _strSQL
		End Sub

	End Class

End Namespace
