
Option Strict Off

Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports System.Threading

Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities

Imports DevExpress.LookAndFeel
Imports System.ComponentModel
Imports System.IO
Imports SPProgUtility.CommonXmlUtility
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SP.Infrastructure.Logging

Public Class frmMASearch_LV
	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private _ClsFunc As New ClsDivFunc
	Private _ClsReg As New SPProgUtility.ClsDivReg

	Private m_utility As Utilities
	Private m_md As Mandant
	Private m_xml As New ClsXML

	Public Property RecCount As Integer
	Private Property Sql2Open As String
	Private Property AsCustomerList As Boolean

	Private m_GridSettingPath As String
	Private UserGridSettingsXml As SettingsXml

	Private m_GVEmployeeSearchSettingfilename As String
	Private m_GVEmployeeSearchVacancySettingfilename As String

	Private m_xmlSettingRestoreEmployeeSearchSetting As String
	Private m_xmlSettingEmployeeSearchFilter As String

	Private m_xmlSettingRestoreEmployeeSearchVacancySetting As String
	Private m_xmlSettingEmployeeSearchVacancyFilter As String


#Region "Private Consts"


	Private Const MODUL_NAME_SETTING = "employeesearch"

	Private Const USER_XML_SETTING_SPUTNIK_EMPLOYEE_SEARCH_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/employeesearch/{1}/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_EMPLOYEE_SEARCH_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/employeesearch/{1}/keepfilter"

	Private Const USER_XML_SETTING_SPUTNIK_EMPLOYEE_SEARCH_VACANCY_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/employeesearch/{1}_vac/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_EMPLOYEE_SEARCH_VACANCY_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/employeesearch/{1}_vac/keepfilter"


#End Region


#Region "Constructor..."

	Public Sub New(ByVal strQuery As String, ByVal _bCustomerList As Boolean)

		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		m_md = New Mandant
		m_utility = New Utilities

		Me.pnlMain.Dock = DockStyle.Fill
		Me.Sql2Open = strQuery
		Me.AsCustomerList = _bCustomerList

		Try
			m_GridSettingPath = String.Format("{0}EmployeeSearch\", m_md.GetGridSettingPath(ClsDataDetail.MDData.MDNr))
			If Not Directory.Exists(m_GridSettingPath) Then Directory.CreateDirectory(m_GridSettingPath)

			m_GVEmployeeSearchSettingfilename = String.Format("{0}{1}{2}.xml", m_GridSettingPath, Me.grdRP.Name, ClsDataDetail.UserData.UserNr)
			m_GVEmployeeSearchVacancySettingfilename = String.Format("{0}{1}_Vacancy{2}.xml", m_GridSettingPath, Me.grdRP.Name, ClsDataDetail.UserData.UserNr)

			m_xmlSettingRestoreEmployeeSearchSetting = String.Format(USER_XML_SETTING_SPUTNIK_EMPLOYEE_SEARCH_GRIDSETTING_RESTORE, ClsDataDetail.UserData.UserNr, MODUL_NAME_SETTING)
			m_xmlSettingEmployeeSearchFilter = String.Format(USER_XML_SETTING_SPUTNIK_EMPLOYEE_SEARCH_GRIDSETTING_FILTER, ClsDataDetail.UserData.UserNr, MODUL_NAME_SETTING)

			m_xmlSettingRestoreEmployeeSearchVacancySetting = String.Format(USER_XML_SETTING_SPUTNIK_EMPLOYEE_SEARCH_VACANCY_GRIDSETTING_RESTORE, ClsDataDetail.UserData.UserNr, MODUL_NAME_SETTING)
			m_xmlSettingEmployeeSearchVacancyFilter = String.Format(USER_XML_SETTING_SPUTNIK_EMPLOYEE_SEARCH_VACANCY_GRIDSETTING_FILTER, ClsDataDetail.UserData.UserNr, MODUL_NAME_SETTING)

			UserGridSettingsXml = New SettingsXml(m_md.GetAllUserGridSettingXMLFilename(ClsDataDetail.MDData.MDNr))

		Catch ex As Exception

		End Try

		If _bCustomerList Then
			ResetGridCustomerData()

		Else
			ResetGridVacancieData()

		End If

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
		m_utility = New Utilities

		Dim sql As String

		sql = Sql2Open

		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ClsDataDetail.GetSelectedMDConnstring, sql, Nothing)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of FoundedData)

				While reader.Read()
					Dim overviewData As New FoundedData

					overviewData.MANr = CInt(m_utility.SafeGetInteger(reader, "MANr", 0))

					overviewData.employeefullname = String.Format("{0}, {1}", m_utility.SafeGetString(reader, "MANachname", ""), m_utility.SafeGetString(reader, "MAVorname", ""))
					overviewData.employeestreet = String.Format("{0}", m_utility.SafeGetString(reader, "mastrasse", ""))
					overviewData.employeeaddress = String.Format("{0} {1}", m_utility.SafeGetString(reader, "maplz", ""), m_utility.SafeGetString(reader, "maort", ""))
					overviewData.employeeplz = String.Format("{0}", m_utility.SafeGetString(reader, "maplz", ""))
					overviewData.employeecity = String.Format("{0}", m_utility.SafeGetString(reader, "maort", ""))
					overviewData.employeepostcode = String.Format("{0}", m_utility.SafeGetString(reader, "mapostfach", ""))

					overviewData.employeetelefon = String.Format("{0}", m_utility.SafeGetString(reader, "matelefonp", ""))
					overviewData.employeemobil = String.Format("{0}", m_utility.SafeGetString(reader, "manatel", ""))

					overviewData.employeequalification = String.Format("{0}", m_utility.SafeGetString(reader, "maberuf", ""))
					overviewData.employeeemail = String.Format("{0}", m_utility.SafeGetString(reader, "maemail", ""))
					overviewData.employeeadvisor = String.Format("{0}", m_utility.SafeGetString(reader, "maberater", ""))


					overviewData.employeebirthday = m_utility.SafeGetDateTime(reader, "magebdat", Nothing)
					overviewData.employeeahvnumber = String.Format("{0}", If(m_utility.SafeGetString(reader, "maahv_nr_new", "").Length = 16,
																																	 m_utility.SafeGetString(reader, "maahv_nr_new", ""),
																																	 m_utility.SafeGetString(reader, "maahv_nr", "")))
					overviewData.employeepermision = String.Format("{0}", m_utility.SafeGetString(reader, "mabewillig", ""))
					overviewData.employeepermisiontil = m_utility.SafeGetDateTime(reader, "mabew_bis", Nothing)
					overviewData.employeenationality = String.Format("{0}", m_utility.SafeGetString(reader, "manationality", ""))
					overviewData.employeezivilstand = String.Format("{0}", m_utility.SafeGetString(reader, "mazivilstand", ""))
					overviewData.employeecanton = String.Format("{0}", m_utility.SafeGetString(reader, "mas_kanton", ""))

					overviewData.employeefstate = String.Format("{0}", m_utility.SafeGetString(reader, "makstat1", ""))
					overviewData.employeesstate = String.Format("{0}", m_utility.SafeGetString(reader, "makstat2", ""))
					overviewData.employeeHomeland = String.Format("{0}", m_utility.SafeGetString(reader, "GebOrt", ""))

					result.Add(overviewData)

				End While

			End If

		Catch e As Exception
			result = Nothing
			m_Logger.LogError(e.ToString())

		Finally
			m_utility.CloseReader(reader)

		End Try

		Return result
	End Function

	Private Function LoadFoundedCustomerList() As Boolean

		Dim listOfEmployees = GetDbCustomerData4Show()

		Dim responsiblePersonsGridData = (From person In listOfEmployees
																			Select New FoundedData With
																						 {.MANr = person.MANr,
																							.employeefullname = person.employeefullname,
																							.employeestreet = person.employeestreet,
																							.employeeaddress = person.employeeaddress,
																							.employeepostcode = person.employeepostcode,
																							.employeetelefon = person.employeetelefon,
																						.employeemobil = person.employeemobil,
																						.employeequalification = person.employeequalification,
																						 .employeeemail = person.employeeemail,
																						 .employeebirthday = person.employeebirthday,
																						 .employeeahvnumber = person.employeeahvnumber,
																						 .employeepermision = person.employeepermision,
																						 .employeepermisiontil = person.employeepermisiontil,
																						 .employeenationality = person.employeenationality,
																						 .employeezivilstand = person.employeezivilstand,
																						 .employeecanton = person.employeecanton,
																						 .employeefstate = person.employeefstate,
																						 .employeesstate = person.employeesstate,
																						 .employeeHomeland = person.employeeHomeland,
																							.employeeadvisor = person.employeeadvisor}).ToList()

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

		Dim AutofilterconditionNumber = ClsDataDetail.MDData.AutoFilterConditionNumber
		Dim AutofilterconditionDate = ClsDataDetail.MDData.AutoFilterConditionDate

		gvRP.Columns.Clear()

		Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMANr.OptionsFilter.AutoFilterCondition = AutofilterconditionNumber ' DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnMANr.Caption = m_xml.GetSafeTranslationValue("Kandidaten-Nr.")
		columnMANr.Name = "MANr"
		columnMANr.FieldName = "MANr"
		columnMANr.Visible = False
		gvRP.Columns.Add(columnMANr)

		Dim columnEmployee As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployee.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnEmployee.Caption = m_xml.GetSafeTranslationValue("Kandidat")
		columnEmployee.Name = "employeefullname"
		columnEmployee.FieldName = "employeefullname"
		columnEmployee.BestFit()
		columnEmployee.Visible = True
		gvRP.Columns.Add(columnEmployee)

		Dim columnemployeepostcode As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeepostcode.Caption = m_xml.GetSafeTranslationValue("Postfach")
		columnemployeepostcode.Name = "employeepostcode"
		columnemployeepostcode.FieldName = "employeepostcode"
		columnemployeepostcode.Visible = True
		columnemployeepostcode.BestFit()
		gvRP.Columns.Add(columnemployeepostcode)

		Dim columnStreet As New DevExpress.XtraGrid.Columns.GridColumn()
		columnStreet.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnStreet.Caption = m_xml.GetSafeTranslationValue("Strasse")
		columnStreet.Name = "employeestreet"
		columnStreet.FieldName = "employeestreet"
		columnStreet.Visible = True
		columnStreet.BestFit()
		gvRP.Columns.Add(columnStreet)

		Dim columnAddress As New DevExpress.XtraGrid.Columns.GridColumn()
		columnAddress.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnAddress.Caption = m_xml.GetSafeTranslationValue("Adresse")
		columnAddress.Name = "employeeaddress"
		columnAddress.FieldName = "employeeaddress"
		columnAddress.Visible = True
		columnAddress.BestFit()
		gvRP.Columns.Add(columnAddress)

		Dim columnTelefon As New DevExpress.XtraGrid.Columns.GridColumn()
		columnTelefon.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnTelefon.Caption = m_xml.GetSafeTranslationValue("Telefon")
		columnTelefon.Name = "employeetelefon"
		columnTelefon.FieldName = "employeetelefon"
		columnTelefon.Visible = True
		columnTelefon.BestFit()
		gvRP.Columns.Add(columnTelefon)

		Dim columnMobile As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMobile.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnMobile.Caption = m_xml.GetSafeTranslationValue("Natel")
		columnMobile.Name = "employeemobil"
		columnMobile.FieldName = "employeemobil"
		columnMobile.Visible = True
		columnMobile.BestFit()
		gvRP.Columns.Add(columnMobile)

		Dim columnQualification As New DevExpress.XtraGrid.Columns.GridColumn()
		columnQualification.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnQualification.Caption = m_xml.GetSafeTranslationValue("Hauptqualifikation")
		columnQualification.Name = "employeequalification"
		columnQualification.FieldName = "employeequalification"
		columnQualification.Visible = True
		columnQualification.BestFit()
		gvRP.Columns.Add(columnQualification)

		Dim columnEMail As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEMail.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnEMail.Caption = m_xml.GetSafeTranslationValue("E-Mail")
		columnEMail.Name = "employeeemail"
		columnEMail.FieldName = "employeeemail"
		columnEMail.Visible = True
		columnEMail.BestFit()
		gvRP.Columns.Add(columnEMail)

		Dim columnAdvisor As New DevExpress.XtraGrid.Columns.GridColumn()
		columnAdvisor.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnAdvisor.Caption = m_xml.GetSafeTranslationValue("Berater")
		columnAdvisor.Name = "employeeadvisor"
		columnAdvisor.FieldName = "employeeadvisor"
		columnAdvisor.Visible = True
		columnAdvisor.BestFit()
		gvRP.Columns.Add(columnAdvisor)

		Dim columnBirthday As New DevExpress.XtraGrid.Columns.GridColumn()
		columnBirthday.OptionsFilter.AutoFilterCondition = AutofilterconditionDate
		columnBirthday.Caption = m_xml.GetSafeTranslationValue("Geburtsdatum")
		columnBirthday.Name = "employeebirthday"
		columnBirthday.FieldName = "employeebirthday"
		columnBirthday.Visible = False
		gvRP.Columns.Add(columnBirthday)

		Dim columnemployeeahvnumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeeahvnumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemployeeahvnumber.Caption = m_xml.GetSafeTranslationValue("AHV-Nummer")
		columnemployeeahvnumber.Name = "employeeahvnumber"
		columnemployeeahvnumber.FieldName = "employeeahvnumber"
		columnemployeeahvnumber.Visible = False
		gvRP.Columns.Add(columnemployeeahvnumber)

		Dim columnemployeepermision As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeepermision.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemployeepermision.Caption = m_xml.GetSafeTranslationValue("Bewilligung")
		columnemployeepermision.Name = "employeepermision"
		columnemployeepermision.FieldName = "employeepermision"
		columnemployeepermision.Visible = False
		gvRP.Columns.Add(columnemployeepermision)

		Dim columnemployeepermisiontil As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeepermisiontil.OptionsFilter.AutoFilterCondition = AutofilterconditionDate '  DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemployeepermisiontil.Caption = m_xml.GetSafeTranslationValue("Bewilligung gültig bis")
		columnemployeepermisiontil.Name = "employeepermisiontil"
		columnemployeepermisiontil.FieldName = "employeepermisiontil"
		columnemployeepermisiontil.Visible = False
		gvRP.Columns.Add(columnemployeepermisiontil)

		Dim columnemployeenationality As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeenationality.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemployeenationality.Caption = m_xml.GetSafeTranslationValue("Nationalität")
		columnemployeenationality.Name = "employeenationality"
		columnemployeenationality.FieldName = "employeenationality"
		columnemployeenationality.Visible = False
		gvRP.Columns.Add(columnemployeenationality)

		Dim columnemployeezivilstand As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeezivilstand.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemployeezivilstand.Caption = m_xml.GetSafeTranslationValue("Zivilstand")
		columnemployeezivilstand.Name = "employeezivilstand"
		columnemployeezivilstand.FieldName = "employeezivilstand"
		columnemployeezivilstand.Visible = False
		gvRP.Columns.Add(columnemployeezivilstand)

		Dim columnemployeecanton As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeecanton.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemployeecanton.Caption = m_xml.GetSafeTranslationValue("Steuerkanton")
		columnemployeecanton.Name = "employeecanton"
		columnemployeecanton.FieldName = "employeecanton"
		columnemployeecanton.Visible = False
		gvRP.Columns.Add(columnemployeecanton)

		Dim columnemployeefstate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeefstate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemployeefstate.Caption = m_xml.GetSafeTranslationValue("MA1Status", True)
		columnemployeefstate.Name = "employeefstate"
		columnemployeefstate.FieldName = "employeefstate"
		columnemployeefstate.Visible = False
		gvRP.Columns.Add(columnemployeefstate)

		Dim columnemployeesstate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeesstate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemployeesstate.Caption = m_xml.GetSafeTranslationValue("MA2Status", True)
		columnemployeesstate.Name = "employeesstate"
		columnemployeesstate.FieldName = "employeesstate"
		columnemployeesstate.Visible = False
		gvRP.Columns.Add(columnemployeesstate)

		Dim columnemployeeHomeland As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeeHomeland.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemployeeHomeland.Caption = m_xml.GetSafeTranslationValue("Heimatort", True)
		columnemployeeHomeland.Name = "employeeHomeland"
		columnemployeeHomeland.FieldName = "employeeHomeland"
		columnemployeeHomeland.Visible = False
		gvRP.Columns.Add(columnemployeeHomeland)


		RestoreGridLayoutFromXml()

		grdRP.DataSource = Nothing

	End Sub


	Function GetDbCustomer4VacanciesData4Show() As IEnumerable(Of FoundedData)
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

					overviewData.MANr = CInt(m_utility.SafeGetInteger(reader, "MANr", 0))

					overviewData.employeefullname = String.Format("{0}, {1}", m_utility.SafeGetString(reader, "MANachname"), m_utility.SafeGetString(reader, "MAVorname"))
					overviewData.employeestreet = String.Format("{0}", m_utility.SafeGetString(reader, "mastrasse"))
					overviewData.employeeaddress = String.Format("{0} {1}", m_utility.SafeGetString(reader, "maplz"), m_utility.SafeGetString(reader, "maort"))
					overviewData.employeeplz = String.Format("{0}", m_utility.SafeGetString(reader, "maplz"))
					overviewData.employeecity = String.Format("{0}", m_utility.SafeGetString(reader, "maort"))
					overviewData.employeepostcode = String.Format("{0}", m_utility.SafeGetString(reader, "mapostfach"))

					overviewData.employeetelefon = String.Format("{0}", m_utility.SafeGetString(reader, "matelefonp"))
					overviewData.employeemobil = String.Format("{0}", m_utility.SafeGetString(reader, "manatel"))

					overviewData.employeequalification = String.Format("{0}", m_utility.SafeGetString(reader, "maberuf"))
					overviewData.employeeemail = String.Format("{0}", m_utility.SafeGetString(reader, "maemail"))
					overviewData.employeeadvisor = String.Format("{0}", m_utility.SafeGetString(reader, "maberater"))


					overviewData.employeebirthday = m_utility.SafeGetDateTime(reader, "magebdat", Nothing)
					overviewData.employeeahvnumber = String.Format("{0}", If(m_utility.SafeGetString(reader, "maahv_nr_new").Length = 16,
																																	 m_utility.SafeGetString(reader, "maahv_nr_new"),
																																	 m_utility.SafeGetString(reader, "maahv_nr")))
					overviewData.employeepermision = String.Format("{0}", m_utility.SafeGetString(reader, "mabewillig"))
					overviewData.employeepermisiontil = m_utility.SafeGetDateTime(reader, "mabew_bis", Nothing)
					overviewData.employeenationality = String.Format("{0}", m_utility.SafeGetString(reader, "manationality"))
					overviewData.employeezivilstand = String.Format("{0}", m_utility.SafeGetString(reader, "mazivilstand"))
					overviewData.employeecanton = String.Format("{0}", m_utility.SafeGetString(reader, "mas_kanton"))

					overviewData.employeefstate = String.Format("{0}", m_utility.SafeGetString(reader, "makstat1"))
					overviewData.employeesstate = String.Format("{0}", m_utility.SafeGetString(reader, "makstat2"))

					result.Add(overviewData)

				End While

			End If

		Catch e As Exception
			result = Nothing
			m_Logger.LogError(e.ToString())

		Finally
			m_utility.CloseReader(reader)

		End Try

		Return result
	End Function

	Private Function LoadFoundedCustomer4VacanciesList() As Boolean

		Dim listOfEmployees = GetDbCustomer4VacanciesData4Show()

		Dim responsiblePersonsGridData = (From person In listOfEmployees
																			Select New FoundedData With
																						 {.MANr = person.MANr,
																							.employeefullname = person.employeefullname,
																							.employeestreet = person.employeestreet,
																							.employeeaddress = person.employeeaddress,
																							.employeepostcode = person.employeepostcode,
																							.employeetelefon = person.employeetelefon,
																						.employeemobil = person.employeemobil,
																						.employeequalification = person.employeequalification,
																						 .employeeemail = person.employeeemail,
																						 .employeebirthday = person.employeebirthday,
																						 .employeeahvnumber = person.employeeahvnumber,
																						 .employeepermision = person.employeepermision,
																						 .employeepermisiontil = person.employeepermisiontil,
																						 .employeenationality = person.employeenationality,
																						 .employeezivilstand = person.employeezivilstand,
																						 .employeecanton = person.employeecanton,
																						 .employeefstate = person.employeefstate,
																						 .employeesstate = person.employeesstate,
																							.employeeadvisor = person.employeeadvisor}).ToList()

		Dim listDataSource As BindingList(Of FoundedData) = New BindingList(Of FoundedData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdRP.DataSource = listDataSource

		Return Not listOfEmployees Is Nothing
	End Function

	Private Sub ResetGridVacancieData()

		gvRP.OptionsView.ShowIndicator = False
		gvRP.OptionsView.ShowAutoFilterRow = True
		gvRP.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never

		gvRP.Columns.Clear()

		Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMANr.Caption = m_xml.GetSafeTranslationValue("Kandidaten-Nr.")
		columnMANr.Name = "MANr"
		columnMANr.FieldName = "MANr"
		columnMANr.Visible = False
		gvRP.Columns.Add(columnMANr)

		Dim columnEmployee As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployee.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnEmployee.Caption = m_xml.GetSafeTranslationValue("Kandidat")
		columnEmployee.Name = "employeefullname"
		columnEmployee.FieldName = "employeefullname"
		columnEmployee.BestFit()
		columnEmployee.Visible = True
		gvRP.Columns.Add(columnEmployee)

		Dim columnemployeepostcode As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeepostcode.Caption = m_xml.GetSafeTranslationValue("Postfach")
		columnemployeepostcode.Name = "employeepostcode"
		columnemployeepostcode.FieldName = "employeepostcode"
		columnemployeepostcode.Visible = True
		columnemployeepostcode.BestFit()
		gvRP.Columns.Add(columnemployeepostcode)

		Dim columnStreet As New DevExpress.XtraGrid.Columns.GridColumn()
		columnStreet.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnStreet.Caption = m_xml.GetSafeTranslationValue("Strasse")
		columnStreet.Name = "employeestreet"
		columnStreet.FieldName = "employeestreet"
		columnStreet.Visible = True
		columnStreet.BestFit()
		gvRP.Columns.Add(columnStreet)

		Dim columnAddress As New DevExpress.XtraGrid.Columns.GridColumn()
		columnAddress.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnAddress.Caption = m_xml.GetSafeTranslationValue("Adresse")
		columnAddress.Name = "employeeaddress"
		columnAddress.FieldName = "employeeaddress"
		columnAddress.Visible = True
		columnAddress.BestFit()
		gvRP.Columns.Add(columnAddress)

		Dim columnTelefon As New DevExpress.XtraGrid.Columns.GridColumn()
		columnTelefon.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnTelefon.Caption = m_xml.GetSafeTranslationValue("Telefon")
		columnTelefon.Name = "employeetelefon"
		columnTelefon.FieldName = "employeetelefon"
		columnTelefon.Visible = True
		columnTelefon.BestFit()
		gvRP.Columns.Add(columnTelefon)

		Dim columnMobile As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMobile.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnMobile.Caption = m_xml.GetSafeTranslationValue("Natel")
		columnMobile.Name = "employeemobil"
		columnMobile.FieldName = "employeemobil"
		columnMobile.Visible = True
		columnMobile.BestFit()
		gvRP.Columns.Add(columnMobile)

		Dim columnQualification As New DevExpress.XtraGrid.Columns.GridColumn()
		columnQualification.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnQualification.Caption = m_xml.GetSafeTranslationValue("Hauptqualifikation")
		columnQualification.Name = "employeequalification"
		columnQualification.FieldName = "employeequalification"
		columnQualification.Visible = True
		columnQualification.BestFit()
		gvRP.Columns.Add(columnQualification)

		Dim columnEMail As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEMail.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnEMail.Caption = m_xml.GetSafeTranslationValue("E-Mail")
		columnEMail.Name = "employeeemail"
		columnEMail.FieldName = "employeeemail"
		columnEMail.Visible = True
		columnEMail.BestFit()
		gvRP.Columns.Add(columnEMail)

		Dim columnAdvisor As New DevExpress.XtraGrid.Columns.GridColumn()
		columnAdvisor.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnAdvisor.Caption = m_xml.GetSafeTranslationValue("Berater")
		columnAdvisor.Name = "employeeadvisor"
		columnAdvisor.FieldName = "employeeadvisor"
		columnAdvisor.Visible = True
		columnAdvisor.BestFit()
		gvRP.Columns.Add(columnAdvisor)

		Dim columnBirthday As New DevExpress.XtraGrid.Columns.GridColumn()
		columnBirthday.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnBirthday.Caption = m_xml.GetSafeTranslationValue("Geburtsdatum")
		columnBirthday.Name = "employeebirthday"
		columnBirthday.FieldName = "employeebirthday"
		columnBirthday.Visible = False
		gvRP.Columns.Add(columnBirthday)

		Dim columnemployeeahvnumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeeahvnumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemployeeahvnumber.Caption = m_xml.GetSafeTranslationValue("AHV-Nummer")
		columnemployeeahvnumber.Name = "employeeahvnumber"
		columnemployeeahvnumber.FieldName = "employeeahvnumber"
		columnemployeeahvnumber.Visible = False
		gvRP.Columns.Add(columnemployeeahvnumber)

		Dim columnemployeepermision As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeepermision.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemployeepermision.Caption = m_xml.GetSafeTranslationValue("Bewilligung")
		columnemployeepermision.Name = "employeepermision"
		columnemployeepermision.FieldName = "employeepermision"
		columnemployeepermision.Visible = False
		gvRP.Columns.Add(columnemployeepermision)

		Dim columnemployeepermisiontil As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeepermisiontil.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemployeepermisiontil.Caption = m_xml.GetSafeTranslationValue("Bewilligung gültig bis")
		columnemployeepermisiontil.Name = "employeepermisiontil"
		columnemployeepermisiontil.FieldName = "employeepermisiontil"
		columnemployeepermisiontil.Visible = False
		gvRP.Columns.Add(columnemployeepermisiontil)

		Dim columnemployeenationality As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeenationality.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemployeenationality.Caption = m_xml.GetSafeTranslationValue("Nationalität")
		columnemployeenationality.Name = "employeenationality"
		columnemployeenationality.FieldName = "employeenationality"
		columnemployeenationality.Visible = False
		gvRP.Columns.Add(columnemployeenationality)

		Dim columnemployeezivilstand As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeezivilstand.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemployeezivilstand.Caption = m_xml.GetSafeTranslationValue("Zivilstand")
		columnemployeezivilstand.Name = "employeezivilstand"
		columnemployeezivilstand.FieldName = "employeezivilstand"
		columnemployeezivilstand.Visible = False
		gvRP.Columns.Add(columnemployeezivilstand)

		Dim columnemployeecanton As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeecanton.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemployeecanton.Caption = m_xml.GetSafeTranslationValue("Steuerkanton")
		columnemployeecanton.Name = "employeecanton"
		columnemployeecanton.FieldName = "employeecanton"
		columnemployeecanton.Visible = False
		gvRP.Columns.Add(columnemployeecanton)

		Dim columnemployeefstate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeefstate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemployeefstate.Caption = m_xml.GetSafeTranslationValue("MA1Status", True)
		columnemployeefstate.Name = "employeefstate"
		columnemployeefstate.FieldName = "employeefstate"
		columnemployeefstate.Visible = False
		gvRP.Columns.Add(columnemployeefstate)

		Dim columnemployeesstate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeesstate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemployeesstate.Caption = m_xml.GetSafeTranslationValue("MA2Status", True)
		columnemployeesstate.Name = "employeesstate"
		columnemployeesstate.FieldName = "employeesstate"
		columnemployeesstate.Visible = False
		gvRP.Columns.Add(columnemployeesstate)

		RestoreGridLayoutFromXml()

		grdRP.DataSource = Nothing

	End Sub

	Private Sub frmOnDisposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

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
			If Me.AsCustomerList Then
				LoadFoundedCustomerList()

			Else
				LoadFoundedCustomer4VacanciesList()

			End If

			Me.RecCount = gvRP.RowCount
			Me.bsiInfo.Caption = String.Format(m_xml.GetSafeTranslationValue("Anzahl Datensätze: {0}"), Me.RecCount)

			AddHandler gvRP.RowCellClick, AddressOf Ongv_RowCellClick
			AddHandler Me.gvRP.RowCountChanged, AddressOf OngvMain_RowCountChanged
			AddHandler Me.gvRP.ColumnPositionChanged, AddressOf OngvColumnPositionChanged
			AddHandler Me.gvRP.ColumnWidthChanged, AddressOf OngvColumnPositionChanged

		Catch ex As Exception

		End Try

	End Sub

	Private Sub OngvMain_ColumnFilterChanged(sender As Object, e As System.EventArgs)

		Me.bsiInfo.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True
		Me.bsiInfo.Caption = String.Format(m_xml.GetSafeTranslationValue("Anzahl Datensätze: {0}"), Me.gvRP.RowCount)

		OngvColumnPositionChanged(sender, New System.EventArgs)

	End Sub

	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvRP.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, FoundedData)

				Select Case column.Name.ToLower
					Case "employeetelefon"
						If viewData.employeetelefon.Length > 0 Then StartCalling(viewData.employeetelefon) ', viewData.MANr)

					Case "employeemobil"
						If viewData.employeemobil.Length > 0 Then StartCalling(viewData.employeemobil) ', viewData.MANr)

					Case "employeeemail"
						If viewData.employeeemail.Length > 0 Then MailTo(viewData.employeeemail)


					Case Else
						If viewData.MANr > 0 Then
							Dim hub = MessageService.Instance.Hub
							Dim openMng As New OpenEmployeeMngRequest(Me, ClsDataDetail.UserData.UserNr, ClsDataDetail.MDData.MDNr, viewData.MANr)
							hub.Publish(openMng)

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

		If AsCustomerList Then
			gvRP.SaveLayoutToXml(m_GVEmployeeSearchSettingfilename)
		Else
			gvRP.SaveLayoutToXml(m_GVEmployeeSearchVacancySettingfilename)

		End If

	End Sub

	Private Sub RestoreGridLayoutFromXml()
		Dim keepFilter = False
		Dim restoreLayout = True


		If AsCustomerList Then
			Try
				restoreLayout = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreEmployeeSearchSetting), True)
				keepFilter = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingEmployeeSearchFilter), False)
			Catch ex As Exception

			End Try

			If File.Exists(m_GVEmployeeSearchSettingfilename) Then gvRP.RestoreLayoutFromXml(m_GVEmployeeSearchSettingfilename)
		Else
			Try
				restoreLayout = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreEmployeeSearchVacancySetting), True)
				keepFilter = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingEmployeeSearchVacancyFilter), False)
			Catch ex As Exception

			End Try
			If File.Exists(m_GVEmployeeSearchVacancySettingfilename) Then gvRP.RestoreLayoutFromXml(m_GVEmployeeSearchVacancySettingfilename)

		End If

		If restoreLayout AndAlso Not keepFilter Then gvRP.ActiveFilterCriteria = Nothing

	End Sub



End Class


Namespace ThreadTesting


	Public Class OpenFormsWithThreading

		''' <summary>
		''' The logger.
		''' </summary>
		Private m_Logger As ILogger = New Logger()

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

	End Class

End Namespace
