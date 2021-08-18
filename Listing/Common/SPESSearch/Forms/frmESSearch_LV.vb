
Option Strict Off

Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports System.Threading
Imports DevExpress.LookAndFeel
Imports System.ComponentModel

Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports System.IO

Imports SP.Infrastructure.UI
Imports SP.KD.CPersonMng.UI
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SPProgUtility.CommonXmlUtility
Imports SP.Infrastructure.Logging
Imports SPESSearch.ClsDataDetail


Public Class frmESSearch_LV
	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private m_md As Mandant
	Private m_utility As Utilities

	Public Property RecCount As Integer
	Private Property Sql2Open As String

	Private m_GridSettingPath As String
	Private UserGridSettingsXml As SettingsXml

	Private m_GVESSearchSettingfilename As String

	Private m_xmlSettingRestoreESSearchSetting As String
	Private m_xmlSettingESSearchFilter As String

#Region "Private Consts"


	Private Const MODUL_NAME_SETTING = "essearch"

	Private Const USER_XML_SETTING_SPUTNIK_CUSTOMER_SEARCH_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/essearch/{1}/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_CUSTOMER_SEARCH_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/essearch/{1}/keepfilter"


#End Region


#Region "Constructor"

	Public Sub New(ByVal strQuery As String)

		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		m_md = New Mandant
		m_utility = New Utilities
		Me.Sql2Open = strQuery

		Try
			m_GridSettingPath = String.Format("{0}ESSearch\", m_md.GetGridSettingPath(ClsDataDetail.MDData.MDNr))
			If Not Directory.Exists(m_GridSettingPath) Then Directory.CreateDirectory(m_GridSettingPath)

			m_GVESSearchSettingfilename = String.Format("{0}{1}{2}.xml", m_GridSettingPath, Me.grdRP.Name, ClsDataDetail.UserData.UserNr)

			m_xmlSettingRestoreESSearchSetting = String.Format(USER_XML_SETTING_SPUTNIK_CUSTOMER_SEARCH_GRIDSETTING_RESTORE, ClsDataDetail.UserData.UserNr, MODUL_NAME_SETTING)
			m_xmlSettingESSearchFilter = String.Format(USER_XML_SETTING_SPUTNIK_CUSTOMER_SEARCH_GRIDSETTING_FILTER, ClsDataDetail.UserData.UserNr, MODUL_NAME_SETTING)

			UserGridSettingsXml = New SettingsXml(m_md.GetAllUserGridSettingXMLFilename(ClsDataDetail.MDData.MDNr))

		Catch ex As Exception

		End Try


		ResetGridCustomerData()

	End Sub

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

		Dim sql As String

		sql = Sql2Open

		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ClsDataDetail.GetSelectedMDConnstring, sql, Nothing)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of FoundedData)

				While reader.Read()
					Dim overviewData As New FoundedData

					overviewData.ESNr = CInt(m_utility.SafeGetInteger(reader, "ESNr", 0))
					overviewData.MANr = CInt(m_utility.SafeGetInteger(reader, "MANr", 0))
					overviewData.KDNr = CInt(m_utility.SafeGetInteger(reader, "KDNr", 0))
					overviewData.ZHDNr = CInt(m_utility.SafeGetInteger(reader, "ESKDZHDNr", 0))

					overviewData.VakNr = CInt(m_utility.SafeGetInteger(reader, "VakNr", 0))
					overviewData.ProposeNr = CInt(m_utility.SafeGetInteger(reader, "ProposeNr", 0))

					overviewData.esals = String.Format("{0}", m_utility.SafeGetString(reader, "ES_Als"))
					overviewData.esbeginn = Format(m_utility.SafeGetDateTime(reader, "ES_Ab", Nothing), "d")
					overviewData.esend = m_utility.SafeGetDateTime(reader, "ES_Ende", Nothing)

					overviewData.esbeginnend = String.Format("{0} - {1}", Format(m_utility.SafeGetDateTime(reader, "ES_Ab", Nothing), "d"),
																									 Format(m_utility.SafeGetDateTime(reader, "ES_Ende", Nothing), "d"))

					overviewData.gavqualification = String.Format("{0}", m_utility.SafeGetString(reader, "gavgruppe0"))
					overviewData.esmargewithbvg = m_utility.SafeGetDecimal(reader, "margemitbvg", 0)
					overviewData.esmargewithoutbvg = m_utility.SafeGetDecimal(reader, "bruttomarge", 0)

					overviewData.estarif = m_utility.SafeGetDecimal(reader, "tarif", 0)
					overviewData.esstdlohn = m_utility.SafeGetDecimal(reader, "stundenlohn", 0)
					overviewData.gavstdlohn = m_utility.SafeGetDecimal(reader, "gavstdlohn", 0)

					overviewData.employeename = String.Format("{0}, {1}", m_utility.SafeGetString(reader, "Nachname"), m_utility.SafeGetString(reader, "Vorname"))
					overviewData.employeeaddress = String.Format("{0}, {1} {2}",
																												m_utility.SafeGetString(reader, "mastrasse"),
																												m_utility.SafeGetString(reader, "maplz"),
																												m_utility.SafeGetString(reader, "maort"))
					overviewData.employeeemail = String.Format("{0}", m_utility.SafeGetString(reader, "MAEMail"))
					overviewData.employeepermission = String.Format("{0}", m_utility.SafeGetString(reader, "Bewillig"))
					overviewData.employeepermissionuntil = m_utility.SafeGetDateTime(reader, "Bew_bis", Nothing)
					overviewData.employeequalification = String.Format("{0}", m_utility.SafeGetString(reader, "maberuf"))


					overviewData.customername = String.Format("{0}", m_utility.SafeGetString(reader, "Firma1"))
					overviewData.customeraddress = String.Format("{0}, {1} {2}",
																												m_utility.SafeGetString(reader, "kdstrasse"),
																												m_utility.SafeGetString(reader, "kdplz"),
																												m_utility.SafeGetString(reader, "kdort"))

					overviewData.customertelefon = String.Format("{0}", m_utility.SafeGetString(reader, "kdtelefon"))
					overviewData.customeremail = String.Format("{0}", m_utility.SafeGetString(reader, "kdemail"))

					overviewData.responsiblename = String.Format("{0}, {1}", m_utility.SafeGetString(reader, "kdzNachname"), m_utility.SafeGetString(reader, "kdzvorname"))
					overviewData.responsibleemail = String.Format("{0}", m_utility.SafeGetString(reader, "kdzemail"))

					overviewData.foffice = String.Format("{0}", m_utility.SafeGetString(reader, "filiale2"))
					overviewData.soffice = String.Format("{0}", m_utility.SafeGetString(reader, "filiale1"))

					overviewData.kst1 = String.Format("{0}", m_utility.SafeGetString(reader, "eskst1"))
					overviewData.kst2 = String.Format("{0}", m_utility.SafeGetString(reader, "eskst2"))
					overviewData.kst3 = String.Format("{0}", m_utility.SafeGetString(reader, "eskst"))

					overviewData.employeeadvisor = String.Format("{0}", m_utility.SafeGetString(reader, "maberater"))
					overviewData.customeradvisor = String.Format("{0}", m_utility.SafeGetString(reader, "kdberater"))
					overviewData.esadvisor = String.Format("{0}{1}{2}",
																								 m_utility.SafeGetString(reader, "maberater"),
																								 If(m_utility.SafeGetString(reader, "maberater").ToUpper = m_utility.SafeGetString(reader, "kdberater"), String.Empty, " - "),
																								 m_utility.SafeGetString(reader, "kdberater"))

					overviewData.esvbacked = CBool(m_utility.SafeGetBoolean(reader, "ESVerBacked", False))
					overviewData.vvbacked = CBool(m_utility.SafeGetBoolean(reader, "VerleihBacked", False))

					overviewData.esvisexported = CBool(m_utility.SafeGetString(reader, "ESDoc_Guid").Length > 0)
					overviewData.vvisexported = CBool(m_utility.SafeGetString(reader, "VerleihDoc_Guid").Length > 0)


					result.Add(overviewData)

				End While

			End If

		Catch e As Exception
			result = Nothing
			m_Logger.logerror(e.ToString())

		Finally
			m_utility.CloseReader(reader)

		End Try

		Return result
	End Function

	Private Function LoadFoundedCustomerList() As Boolean

		Dim listOfEmployees = GetDbCustomerData4Show()

		Dim responsiblePersonsGridData = (From person In listOfEmployees
		Select New FoundedData With
					 {.ESNr = person.ESNr, .MANr = person.MANr, .VakNr = person.VakNr,
						.KDNr = person.KDNr, .ZHDNr = person.ZHDNr, .ProposeNr = person.ProposeNr,
						.esals = person.esals,
						.esbeginn = person.esbeginn,
						.esend = person.esend,
						.esbeginnend = person.esbeginnend,
						.gavqualification = person.gavqualification,
						.esmargewithbvg = person.esmargewithbvg,
						.esmargewithoutbvg = person.esmargewithoutbvg,
						.estarif = person.estarif,
						.esstdlohn = person.esstdlohn,
						.gavstdlohn = person.gavstdlohn,
						.employeename = person.employeename,
						.employeeaddress = person.employeeaddress,
						.employeeemail = person.employeeemail,
								.employeepermission = person.employeepermission,
								.employeepermissionuntil = person.employeepermissionuntil,
								.employeequalification = person.employeequalification,
						.customername = person.customername,
						.customeraddress = person.customeraddress,
						.customertelefon = person.customertelefon,
						.customeremail = person.customeremail,
						.responsiblename = person.responsiblename,
						.responsibleemail = person.responsibleemail,
						.foffice = person.foffice,
						.soffice = person.soffice,
						.kst1 = person.kst1,
						.kst2 = person.kst2, .kst3 = person.kst3,
						.employeeadvisor = person.employeeadvisor, .customeradvisor = person.customeradvisor,
						.esadvisor = person.esadvisor,
						.esvbacked = person.esvbacked,
						.vvbacked = person.vvbacked,
						.esvisexported = person.esvisexported,
						.vvisexported = person.vvisexported}).ToList()

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

		Dim columnEsNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEsNr.Caption = m_translate.GetSafeTranslationValue("Einsatz-Nr.")
		columnEsNr.Name = "ESNr"
		columnEsNr.FieldName = "ESNr"
		columnEsNr.Visible = True
		gvRP.Columns.Add(columnEsNr)

		Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMANr.Caption = m_translate.GetSafeTranslationValue("Kandidaten-Nr.")
		columnMANr.Name = "MaNr"
		columnMANr.FieldName = "MANr"
		columnMANr.Visible = False
		gvRP.Columns.Add(columnMANr)

		Dim columnKDNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnKDNr.Caption = m_translate.GetSafeTranslationValue("Kunden-Nr.")
		columnKDNr.Name = "KDNr"
		columnKDNr.FieldName = "KDNr"
		columnKDNr.Visible = False
		gvRP.Columns.Add(columnKDNr)

		Dim columnZHDNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnZHDNr.Caption = m_Translate.GetSafeTranslationValue("Zuständige Person-Nr.")
		columnZHDNr.Name = "ZHDNr"
		columnZHDNr.FieldName = "ZHDNr"
		columnZHDNr.Visible = False
		gvRP.Columns.Add(columnZHDNr)

		Dim columnPNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnPNr.Caption = m_translate.GetSafeTranslationValue("Vorschlag-Nr.")
		columnPNr.Name = "PNr"
		columnPNr.FieldName = "PNr"
		columnPNr.Visible = False
		gvRP.Columns.Add(columnPNr)

		Dim columnvakNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnvakNr.Caption = m_translate.GetSafeTranslationValue("Vakanzen-Nr.")
		columnvakNr.Name = "VakNr"
		columnvakNr.FieldName = "VakNr"
		columnvakNr.Visible = False
		gvRP.Columns.Add(columnvakNr)

		Dim columnEmployeename As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeename.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnEmployeename.Caption = m_translate.GetSafeTranslationValue("Kandidat")
		columnEmployeename.Name = "employeename"
		columnEmployeename.FieldName = "employeename"
		columnEmployeename.Visible = True
		columnEmployeename.BestFit()
		gvRP.Columns.Add(columnEmployeename)

		Dim columnemployeeEMail As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeeEMail.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemployeeEMail.Caption = m_translate.GetSafeTranslationValue("MA-EMail")
		columnemployeeEMail.Name = "employeeemail"
		columnemployeeEMail.FieldName = "employeeemail"
		columnemployeeEMail.Visible = False
		columnemployeeEMail.BestFit()
		gvRP.Columns.Add(columnemployeeEMail)

		Dim columnemployeePermission As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeePermission.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemployeePermission.Caption = m_translate.GetSafeTranslationValue("Bewilligung")
		columnemployeePermission.Name = "employeepermission"
		columnemployeePermission.FieldName = "employeepermission"
		columnemployeePermission.Visible = False
		columnemployeePermission.BestFit()
		gvRP.Columns.Add(columnemployeePermission)

		Dim columnemployeePermissionUntil As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeePermissionUntil.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemployeePermissionUntil.Caption = m_translate.GetSafeTranslationValue("Bewilligung gültig")
		columnemployeePermissionUntil.Name = "employeepermissionuntil"
		columnemployeePermissionUntil.FieldName = "employeepermissionuntil"
		columnemployeePermissionUntil.Visible = False
		columnemployeePermissionUntil.BestFit()
		gvRP.Columns.Add(columnemployeePermissionUntil)

		Dim columnemployeeQualification As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeeQualification.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemployeeQualification.Caption = m_translate.GetSafeTranslationValue("Qualifikation")
		columnemployeeQualification.Name = "employeequalification"
		columnemployeeQualification.FieldName = "employeequalification"
		columnemployeeQualification.Visible = False
		columnemployeeQualification.BestFit()
		gvRP.Columns.Add(columnemployeeQualification)


		Dim columnCustomer As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomer.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCustomer.Caption = m_translate.GetSafeTranslationValue("Kunde")
		columnCustomer.Name = "customername"
		columnCustomer.FieldName = "customername"
		columnCustomer.BestFit()
		columnCustomer.Visible = True
		gvRP.Columns.Add(columnCustomer)


		Dim columnESAls As New DevExpress.XtraGrid.Columns.GridColumn()
		columnESAls.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnESAls.Caption = m_translate.GetSafeTranslationValue("Einsatz als")
		columnESAls.Name = "esals"
		columnESAls.FieldName = "esals"
		columnESAls.BestFit()
		columnESAls.Visible = True
		gvRP.Columns.Add(columnESAls)

		Dim columnESBeginnEnd As New DevExpress.XtraGrid.Columns.GridColumn()
		columnESBeginnEnd.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnESBeginnEnd.Caption = m_translate.GetSafeTranslationValue("Zeitperiode")
		columnESBeginnEnd.Name = "esbeginnend"
		columnESBeginnEnd.FieldName = "esbeginnend"
		columnESBeginnEnd.BestFit()
		columnESBeginnEnd.Visible = True
		gvRP.Columns.Add(columnESBeginnEnd)


		Dim columnESMargeWithBVG As New DevExpress.XtraGrid.Columns.GridColumn()
		columnESMargeWithBVG.Caption = m_translate.GetSafeTranslationValue("Marge mit BVG")
		columnESMargeWithBVG.Name = "esmargewithbvg"
		columnESMargeWithBVG.FieldName = "esmargewithbvg"
		columnESMargeWithBVG.Visible = True
		columnESMargeWithBVG.BestFit()
		gvRP.Columns.Add(columnESMargeWithBVG)

		Dim columnESMargeWithoutBVG As New DevExpress.XtraGrid.Columns.GridColumn()
		columnESMargeWithoutBVG.Caption = m_translate.GetSafeTranslationValue("Marge ohne BVG")
		columnESMargeWithoutBVG.Name = "esmargewithoutbvg"
		columnESMargeWithoutBVG.FieldName = "esmargewithoutbvg"
		columnESMargeWithoutBVG.Visible = True
		columnESMargeWithoutBVG.BestFit()
		gvRP.Columns.Add(columnESMargeWithoutBVG)

		Dim columnestarif As New DevExpress.XtraGrid.Columns.GridColumn()
		columnestarif.Caption = m_translate.GetSafeTranslationValue("Tarif")
		columnestarif.Name = "estarif"
		columnestarif.FieldName = "estarif"
		columnestarif.Visible = True
		columnestarif.BestFit()
		columnestarif.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnestarif.DisplayFormat.FormatString = "N2"
		gvRP.Columns.Add(columnestarif)

		Dim columnesstdlohn As New DevExpress.XtraGrid.Columns.GridColumn()
		columnesstdlohn.Caption = m_translate.GetSafeTranslationValue("Stundenlohn")
		columnesstdlohn.Name = "esstdlohn"
		columnesstdlohn.FieldName = "esstdlohn"
		columnesstdlohn.Visible = True
		columnesstdlohn.BestFit()
		columnesstdlohn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnesstdlohn.DisplayFormat.FormatString = "N2"
		gvRP.Columns.Add(columnesstdlohn)

		Dim columngavstdlohn As New DevExpress.XtraGrid.Columns.GridColumn()
		columngavstdlohn.Caption = m_translate.GetSafeTranslationValue("GAV-Lohn")
		columngavstdlohn.Name = "gavstdlohn"
		columngavstdlohn.FieldName = "gavstdlohn"
		columngavstdlohn.Visible = True
		columngavstdlohn.BestFit()
		columngavstdlohn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columngavstdlohn.DisplayFormat.FormatString = "N2"
		gvRP.Columns.Add(columngavstdlohn)

		Dim columncustomeraddress As New DevExpress.XtraGrid.Columns.GridColumn()
		columncustomeraddress.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncustomeraddress.Caption = m_translate.GetSafeTranslationValue("Kundenadresse")
		columncustomeraddress.Name = "customeraddress"
		columncustomeraddress.FieldName = "customeraddress"
		columncustomeraddress.Visible = False
		columncustomeraddress.BestFit()
		gvRP.Columns.Add(columncustomeraddress)

		Dim columnkst1 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnkst1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnkst1.Caption = m_translate.GetSafeTranslationValue("1. KST")
		columnkst1.Name = "kst1"
		columnkst1.FieldName = "kst1"
		columnkst1.Visible = False
		columnkst1.BestFit()
		gvRP.Columns.Add(columnkst1)

		Dim columnkst2 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnkst2.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnkst2.Caption = m_translate.GetSafeTranslationValue("2. KST")
		columnkst2.Name = "kst2"
		columnkst2.FieldName = "kst2"
		columnkst2.Visible = False
		columnkst2.BestFit()
		gvRP.Columns.Add(columnkst2)

		Dim columnkst3 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnkst3.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnkst3.Caption = m_translate.GetSafeTranslationValue("Berater")
		columnkst3.Name = "kst3"
		columnkst3.FieldName = "kst3"
		columnkst3.Visible = True
		columnkst3.BestFit()
		gvRP.Columns.Add(columnkst3)

		Dim columncustomerTelfon As New DevExpress.XtraGrid.Columns.GridColumn()
		columncustomerTelfon.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncustomerTelfon.Caption = m_translate.GetSafeTranslationValue("KD-Telefon")
		columncustomerTelfon.Name = "customertelefon"
		columncustomerTelfon.FieldName = "customertelefon"
		columncustomerTelfon.Visible = False
		columncustomerTelfon.BestFit()
		gvRP.Columns.Add(columncustomerTelfon)

		Dim columncustomerEMail As New DevExpress.XtraGrid.Columns.GridColumn()
		columncustomerEMail.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncustomerEMail.Caption = m_translate.GetSafeTranslationValue("KD-EMail")
		columncustomerEMail.Name = "customeremail"
		columncustomerEMail.FieldName = "customeremail"
		columncustomerEMail.Visible = False
		columncustomerEMail.BestFit()
		gvRP.Columns.Add(columncustomerEMail)


		Dim columnfOffice As New DevExpress.XtraGrid.Columns.GridColumn()
		columnfOffice.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnfOffice.Caption = m_translate.GetSafeTranslationValue("1. Filiale")
		columnfOffice.Name = "foffice"
		columnfOffice.FieldName = "foffice"
		columnfOffice.Visible = False
		gvRP.Columns.Add(columnfOffice)

		Dim columnsOffice As New DevExpress.XtraGrid.Columns.GridColumn()
		columnsOffice.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnsOffice.Caption = m_translate.GetSafeTranslationValue("2. Filiale")
		columnsOffice.Name = "foffice"
		columnsOffice.FieldName = "foffice"
		columnsOffice.Visible = False
		gvRP.Columns.Add(columnsOffice)


		Dim columnESvBacked As New DevExpress.XtraGrid.Columns.GridColumn()
		columnESvBacked.Caption = m_translate.GetSafeTranslationValue("ES-Vertrag zurück?")
		columnESvBacked.Name = "esvbacked"
		columnESvBacked.FieldName = "esvbacked"
		columnESvBacked.Visible = False
		gvRP.Columns.Add(columnESvBacked)

		Dim columnvvBacked As New DevExpress.XtraGrid.Columns.GridColumn()
		columnvvBacked.Caption = m_translate.GetSafeTranslationValue("Verleih zurück?")
		columnvvBacked.Name = "vvbacked"
		columnvvBacked.FieldName = "vvbacked"
		columnvvBacked.Visible = False
		gvRP.Columns.Add(columnvvBacked)

		Dim columnESExported As New DevExpress.XtraGrid.Columns.GridColumn()
		columnESExported.Caption = m_translate.GetSafeTranslationValue("ES-Vertrag in WOS?")
		columnESExported.Name = "esvisexported"
		columnESExported.FieldName = "esvisexported"
		columnESExported.Visible = False
		gvRP.Columns.Add(columnESExported)

		Dim columnemployeeadvisor As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeeadvisor.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemployeeadvisor.Caption = m_translate.GetSafeTranslationValue("MA-Berater")
		columnemployeeadvisor.Name = "employeeadvisor"
		columnemployeeadvisor.FieldName = "employeeadvisor"
		columnemployeeadvisor.Visible = False
		gvRP.Columns.Add(columnemployeeadvisor)

		Dim columncustomeradvisor As New DevExpress.XtraGrid.Columns.GridColumn()
		columncustomeradvisor.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncustomeradvisor.Caption = m_translate.GetSafeTranslationValue("KD-Berater")
		columncustomeradvisor.Name = "customeradvisor"
		columncustomeradvisor.FieldName = "customeradvisor"
		columncustomeradvisor.Visible = False
		gvRP.Columns.Add(columncustomeradvisor)

		Dim columnesadvisor As New DevExpress.XtraGrid.Columns.GridColumn()
		columnesadvisor.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnesadvisor.Caption = m_translate.GetSafeTranslationValue("Einsatz-Berater")
		columnesadvisor.Name = "esadvisor"
		columnesadvisor.FieldName = "esadvisor"
		columnesadvisor.Visible = True
		gvRP.Columns.Add(columnesadvisor)

		Dim columnvvExported As New DevExpress.XtraGrid.Columns.GridColumn()
		columnvvExported.Caption = m_translate.GetSafeTranslationValue("Verleih in WOS?")
		columnvvExported.Name = "vvisexported"
		columnvvExported.FieldName = "vvisexported"
		columnvvExported.Visible = False
		gvRP.Columns.Add(columnvvExported)

		RestoreGridLayoutFromXml()


		grdRP.DataSource = Nothing

	End Sub


#Region "Form Properties..."


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
			Me.Text = m_translate.GetSafeTranslationValue(Me.Text)
			bsiInfo.Caption = m_Translate.GetSafeTranslationValue(bsiInfo.Caption)
			bbiPrintList.Caption = m_Translate.GetSafeTranslationValue(bbiPrintList.Caption)

		Catch ex As Exception
			m_Logger.logerror(String.Format("1=>{0}.{1}", strMethodeName, ex.Message))
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
			m_Logger.logerror(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.Message))

		End Try

		Try

			LoadFoundedCustomerList()
			Me.RecCount = gvRP.RowCount
			Me.bsiInfo.Caption = String.Format("Anzahl Datensätze: {0}", Me.RecCount)

			AddHandler gvRP.RowCellClick, AddressOf Ongv_RowCellClick

			AddHandler Me.gvRP.ColumnFilterChanged, AddressOf OngvMain_ColumnFilterChanged
			AddHandler Me.gvRP.ColumnPositionChanged, AddressOf OngvColumnPositionChanged
			AddHandler Me.gvRP.ColumnWidthChanged, AddressOf OngvColumnPositionChanged


		Catch ex As Exception

		End Try

	End Sub


#End Region


	Private Sub OngvMain_ColumnFilterChanged(sender As Object, e As System.EventArgs)

		Me.bsiInfo.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True
		Me.bsiInfo.Caption = String.Format(m_translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), Me.gvRP.RowCount)

	End Sub



	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then
			Dim obj As New ThreadTesting.OpenFormsWithThreading()

			Dim column = e.Column
			Dim dataRow = gvRP.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, FoundedData)

				Select Case column.Name.ToLower
					Case "employeename", "manr", "employeeaddress"
						If viewData.employeename.Length > 0 Then obj.OpenSelectedEmployee(viewData.MANr)

					Case "customername", "kdnr"
						If viewData.customername.Length > 0 Then obj._OpenKDForm(viewData.KDNr)

					Case "customeremail"
						If viewData.customeremail.Length > 0 Then MailTo(viewData.customeremail)
					Case "customertelefon"
						If viewData.customertelefon.Length > 0 Then obj._OpenKDTapi(viewData.KDNr, viewData.customertelefon)


					Case "responsiblename", "zhdnr"
						If viewData.responsiblename.Length > 0 Then obj._OpenKDZHD(viewData.KDNr, viewData.ZHDNr)

					Case "responsibleemail"
						If viewData.responsibleemail.Length > 0 Then MailTo(viewData.responsibleemail)

					Case "responsibletelefon", "responsiblemobile"
						If viewData.responsibletelefon.Length > 0 Then obj._OpenKDZHDTapi(viewData.KDNr, viewData.ZHDNr, viewData.responsibletelefon)


					Case Else
						If viewData.ESNr > 0 Then obj._OpenESForm(viewData.ESNr)


				End Select

			End If

		End If

	End Sub

	Private Sub bbiPrintList_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrintList.ItemClick
		If gvRP.RowCount > 0 Then
			' Opens the Preview window. 
			grdRP.ShowPrintPreview()
		End If

	End Sub

	Private Sub OngvColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvRP.SaveLayoutToXml(m_GVESSearchSettingfilename)

	End Sub

	Private Sub RestoreGridLayoutFromXml()
		Dim keepFilter = False
		Dim restoreLayout = True

		Try
			restoreLayout = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreESSearchSetting), True)
			keepFilter = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingESSearchFilter), False)
		Catch ex As Exception

		End Try
		If File.Exists(m_GVESSearchSettingfilename) Then gvRP.RestoreLayoutFromXml(m_GVESSearchSettingfilename)

		If restoreLayout AndAlso Not keepFilter Then gvRP.ActiveFilterCriteria = Nothing

	End Sub


End Class



Namespace ThreadTesting


	Public Class OpenFormsWithThreading

		''' <summary>
		''' The logger.
		''' </summary>
		Private m_Logger As ILogger = New Logger()

		Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
		Dim _ClsReg As New SPProgUtility.ClsDivReg

		Private Property SelectedMANr As Integer

		Private Property SelectedKDNr As Integer
		Private Property SelectedKDZHDNr As Integer

		Private Property SelectedVakNr As Integer
		Private Property SelectedProposeNr As Integer

		Private Property SelectedESNr As Integer
		Private Property SelectedRPNr As Integer
		Private Property SelectedOPNr As Integer
		Private Property SelectedZENr As Integer
		Private Property SelectedZGNr As Integer

		Private Property SelectedTelNr As String
		Private Property SQL2Open As String

		Private m_ui As New UtilityUI

		Sub _OpenKDForm(ByVal _iKDNr As Integer)

			Try
				Dim hub = MessageService.Instance.Hub
				Dim openMng As New OpenCustomerMngRequest(Me, ClsDataDetail.UserData.UserNr, ClsDataDetail.MDData.MDNr, _iKDNr)
				hub.Publish(openMng)

			Catch ex As Exception
				m_Logger.logerror(ex.ToString)

			End Try


		End Sub


		Sub _OpenKDZHD(ByVal _iKDNr As Integer, ByVal _iKDZHDNr As Integer)
			Me.SelectedKDNr = _iKDNr
			Me.SelectedKDZHDNr = _iKDZHDNr

			Dim iKDNr As Integer = Me.SelectedKDNr
			Dim iKDZhdNr As Integer = Me.SelectedKDZHDNr

			Dim responsiblePersonsFrom = New frmResponsiblePerson(CreateInitialData(ClsDataDetail.MDData.MDNr, ClsDataDetail.UserData.UserNr))

			If (responsiblePersonsFrom.LoadResponsiblePersonData(iKDNr, iKDZhdNr)) Then
				responsiblePersonsFrom.Show()
			End If

		End Sub

		Sub _OpenKDTapi(ByVal _iKDNr As Integer, ByVal _strTelNr As String)
			Me.SelectedKDNr = _iKDNr
			Me.SelectedTelNr = _strTelNr

			Dim oMyProg As New SPSTapi.UI.frmCaller(m_InitialData)

			oMyProg.LoadData(SelectedTelNr)
			oMyProg.Show()
			oMyProg.BringToFront()

		End Sub

		Sub _OpenKDZHDTapi(ByVal _iKDNr As Integer, ByVal _iKDZHDNr As Integer, ByVal _strTelNr As String)
			Me.SelectedKDNr = _iKDNr
			Me.SelectedKDZHDNr = _iKDZHDNr
			Me.SelectedTelNr = _strTelNr

			Dim oMyProg As New SPSTapi.UI.frmCaller(m_InitialData)

			oMyProg.LoadData(SelectedTelNr)
			oMyProg.Show()
			oMyProg.BringToFront()

		End Sub

		Sub OpenSelectedEmployee(ByVal Employeenumber As Integer)

			Try

				Dim hub = MessageService.Instance.Hub
				Dim openMng As New OpenEmployeeMngRequest(Me, ClsDataDetail.UserData.UserNr, ClsDataDetail.MDData.MDNr, Employeenumber)
				hub.Publish(openMng)

			Catch ex As Exception
				m_Logger.logerror(ex.ToString())
			End Try

		End Sub

		Sub _OpenESForm(ByVal _iESNr As Integer)

			Dim hub = MessageService.Instance.Hub
			Dim openMng As New OpenEinsatzMngRequest(Me, ClsDataDetail.UserData.UserNr, ClsDataDetail.MDData.MDNr, _iESNr)
			hub.Publish(openMng)

		End Sub

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
				strTranslationProgName = _ClsProgSetting.GetPersonalFolder() & "SPTranslationProg" & ClsDataDetail.UserData.UserNr
				_ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSBewUtility.ClsMain")
				_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", Me.SQL2Open)

				oMyProg = CreateObject("SPSBewUtility.ClsMain")
				oMyProg.OpenKDFieldsform(Me.SQL2Open)

			Catch e As Exception
				MsgBox(e.Message, MsgBoxStyle.Critical, "OpenBewForm")

			End Try

		End Sub


	End Class

End Namespace
