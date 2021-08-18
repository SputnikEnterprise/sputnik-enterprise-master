
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports DevExpress.LookAndFeel
Imports System.ComponentModel

Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports System.IO
Imports SPProgUtility.CommonXmlUtility
Imports SP.Infrastructure.Logging

Public Class frmVakSearch_LV
	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private _ClsFunc As New ClsDivFunc

	Private m_xml As New ClsXML
	Private m_utility As Utilities
	Private m_md As Mandant

	Public Property RecCount As Integer
	Private Property Sql2Open As String

	Private m_GridSettingPath As String
	Private UserGridSettingsXml As SettingsXml

	Private m_GVVacancySearchSettingfilename As String
	Private m_GVEmployeeSearchVacancySettingfilename As String

	Private m_xmlSettingRestoreVacancySearchSetting As String
	Private m_xmlSettingVacancySearchFilter As String

	Private m_xmlSettingRestoreEmployeeSearchVacancySetting As String
	Private m_xmlSettingEmployeeSearchVacancyFilter As String


#Region "Private Consts"


	Private Const MODUL_NAME_SETTING = "vacancysearch"

	Private Const USER_XML_SETTING_SPUTNIK_EMPLOYEE_SEARCH_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/vacancysearch/{1}/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_EMPLOYEE_SEARCH_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/vacancysearch/{1}/keepfilter"


#End Region


#Region "Constructor"

	Public Sub New(ByVal strQuery As String)

		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()
		m_md = New Mandant
		m_utility = New Utilities

		Try
			m_GridSettingPath = String.Format("{0}VacancySearch\", m_md.GetGridSettingPath(ClsDataDetail.MDData.MDNr))
			If Not Directory.Exists(m_GridSettingPath) Then Directory.CreateDirectory(m_GridSettingPath)

			m_GVVacancySearchSettingfilename = String.Format("{0}{1}{2}.xml", m_GridSettingPath, Me.grdRP.Name, ClsDataDetail.UserData.UserNr)
			m_GVEmployeeSearchVacancySettingfilename = String.Format("{0}{1}_Vacancy{2}.xml", m_GridSettingPath, Me.grdRP.Name, ClsDataDetail.UserData.UserNr)

			m_xmlSettingRestoreVacancySearchSetting = String.Format(USER_XML_SETTING_SPUTNIK_EMPLOYEE_SEARCH_GRIDSETTING_RESTORE, ClsDataDetail.UserData.UserNr, MODUL_NAME_SETTING)
			m_xmlSettingVacancySearchFilter = String.Format(USER_XML_SETTING_SPUTNIK_EMPLOYEE_SEARCH_GRIDSETTING_FILTER, ClsDataDetail.UserData.UserNr, MODUL_NAME_SETTING)

			UserGridSettingsXml = New SettingsXml(m_md.GetAllUserGridSettingXMLFilename(ClsDataDetail.MDData.MDNr))

		Catch ex As Exception

		End Try

		Me.pnlMain.Dock = DockStyle.Fill
		Me.Sql2Open = strQuery

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
		m_utility = New Utilities

		Dim sql As String

		sql = Sql2Open

		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ClsDataDetail.GetSelectedMDConnstring, sql, Nothing)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of FoundedData)

				While reader.Read()
					Dim overviewData As New FoundedData

					overviewData.VakNr = CInt(m_utility.SafeGetInteger(reader, "VakNr", 0))
					overviewData.KDNr = CInt(m_utility.SafeGetInteger(reader, "KDNr", 0))
					overviewData.ZHDNr = CInt(m_utility.SafeGetInteger(reader, "KDZHDNr", 0))

					overviewData.title = String.Format("{0}", m_utility.SafeGetString(reader, "Bezeichnung"))
					overviewData.customername = String.Format("{0}", m_utility.SafeGetString(reader, "Firma1"))
					overviewData.customeraddress = String.Format("{0}, {1} {2}", m_utility.SafeGetString(reader, "kdstrasse"),
																											m_utility.SafeGetString(reader, "kdplz"), m_utility.SafeGetString(reader, "kdort"))
					overviewData.customertelefon = String.Format("{0}", m_utility.SafeGetString(reader, "kdtelefon"))
					overviewData.customeremail = String.Format("{0}", m_utility.SafeGetString(reader, "kdemail"))

					overviewData.responsiblename = String.Format("{0}, {1}", m_utility.SafeGetString(reader, "kdznachname"), m_utility.SafeGetString(reader, "kdzVorname"))
					overviewData.appointed = String.Format("{0}", m_utility.SafeGetString(reader, "beginn"))
					overviewData.employment = String.Format("{0}", m_utility.SafeGetString(reader, "jobprozent"))
					overviewData.duration = String.Format("{0}", m_utility.SafeGetString(reader, "dauer"))

					overviewData.responsibletelefon = String.Format("{0}", m_utility.SafeGetString(reader, "kdztelefon"))
					overviewData.responsiblemobile = String.Format("{0}", m_utility.SafeGetString(reader, "kdznatel"))
					overviewData.responsibleemail = String.Format("{0}", m_utility.SafeGetString(reader, "kdzemail"))

					overviewData.creator = String.Format("{0} {1}", m_utility.SafeGetDateTime(reader, "createdon", Nothing), m_utility.SafeGetString(reader, "createdfrom"))
					overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
					overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")

					overviewData.office = String.Format("{0}", m_utility.SafeGetString(reader, "filiale"))
					overviewData.adviser = m_utility.SafeGetString(reader, "VakBeratername")
					overviewData.isexported = CBool(m_utility.SafeGetBoolean(reader, "ieexport", False))
					overviewData.jchonline = CBool(m_utility.SafeGetBoolean(reader, "jchonline", False))
					overviewData.ostjobonline = CBool(m_utility.SafeGetBoolean(reader, "ostjobonline", False))

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
																						 {.VakNr = person.VakNr,
																							.KDNr = person.KDNr,
																							.ZHDNr = person.ZHDNr,
																									.title = person.title,
																							.customername = person.customername,
																							.customeraddress = person.customeraddress,
																							.customertelefon = person.customertelefon,
																							.customeremail = person.customeremail,
																						.responsiblename = person.responsiblename,
																						.responsibletelefon = person.responsibletelefon,
																						 .responsiblemobile = person.responsiblemobile,
																						 .responsibleemail = person.responsibleemail,
																						 .appointed = person.appointed,
																						 .office = person.office,
																						 .employment = person.employment,
																						 .creator = person.creator,
																						 .createdon = person.createdon,
																						 .createdfrom = person.createdfrom,
																						 .duration = person.duration,
																						 .isexported = person.isexported,
																						 .jchonline = person.jchonline,
																						 .ostjobonline = person.ostjobonline,
																						 .adviser = person.adviser}).ToList()

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
		columnMANr.Caption = m_xml.GetSafeTranslationValue("Vakanzen-Nr.")
		columnMANr.Name = "VakNr"
		columnMANr.FieldName = "VakNr"
		columnMANr.Visible = False
		gvRP.Columns.Add(columnMANr)

		Dim columnKDNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnKDNr.Caption = m_xml.GetSafeTranslationValue("Kunden-Nr.")
		columnKDNr.Name = "KDNr"
		columnKDNr.FieldName = "KDNr"
		columnKDNr.Visible = False
		gvRP.Columns.Add(columnKDNr)

		Dim columnZHDNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnZHDNr.Caption = m_xml.GetSafeTranslationValue("Zuständige Person-Nr.")
		columnZHDNr.Name = "ZHDNr"
		columnZHDNr.FieldName = "ZHDNr"
		columnZHDNr.Visible = False
		gvRP.Columns.Add(columnZHDNr)

		Dim columnTitle As New DevExpress.XtraGrid.Columns.GridColumn()
		columnTitle.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnTitle.Caption = m_xml.GetSafeTranslationValue("Bezeichnung")
		columnTitle.Name = "title"
		columnTitle.FieldName = "title"
		columnTitle.BestFit()
		columnTitle.Visible = True
		gvRP.Columns.Add(columnTitle)


		Dim columnCustomer As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomer.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCustomer.Caption = m_xml.GetSafeTranslationValue("Kunde")
		columnCustomer.Name = "customername"
		columnCustomer.FieldName = "customername"
		columnCustomer.BestFit()
		columnCustomer.Visible = True
		gvRP.Columns.Add(columnCustomer)

		Dim columnappointed As New DevExpress.XtraGrid.Columns.GridColumn()
		columnappointed.Caption = m_xml.GetSafeTranslationValue("Antritt")
		columnappointed.Name = "appointed"
		columnappointed.FieldName = "appointed"
		columnappointed.Visible = True
		columnappointed.BestFit()
		gvRP.Columns.Add(columnappointed)

		Dim columnemployment As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployment.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemployment.Caption = m_xml.GetSafeTranslationValue("Beschäftigung")
		columnemployment.Name = "employment"
		columnemployment.FieldName = "employment"
		columnemployment.Visible = True
		columnemployment.BestFit()
		gvRP.Columns.Add(columnemployment)

		Dim columncustomeraddres As New DevExpress.XtraGrid.Columns.GridColumn()
		columncustomeraddres.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncustomeraddres.Caption = m_xml.GetSafeTranslationValue("Adresse")
		columncustomeraddres.Name = "customeraddres"
		columncustomeraddres.FieldName = "customeraddres"
		columncustomeraddres.Visible = False
		columncustomeraddres.BestFit()
		gvRP.Columns.Add(columncustomeraddres)

		Dim columnduration As New DevExpress.XtraGrid.Columns.GridColumn()
		columnduration.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnduration.Caption = m_xml.GetSafeTranslationValue("Dauer")
		columnduration.Name = "duration"
		columnduration.FieldName = "duration"
		columnduration.Visible = True
		columnduration.BestFit()
		gvRP.Columns.Add(columnduration)

		Dim columnTelefon As New DevExpress.XtraGrid.Columns.GridColumn()
		columnTelefon.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnTelefon.Caption = m_xml.GetSafeTranslationValue("KD-Telefon")
		columnTelefon.Name = "customertelefon"
		columnTelefon.FieldName = "customertelefon"
		columnTelefon.Visible = True
		columnTelefon.BestFit()
		gvRP.Columns.Add(columnTelefon)

		Dim columnCustomerEMail As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerEMail.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCustomerEMail.Caption = m_xml.GetSafeTranslationValue("KD-Email")
		columnCustomerEMail.Name = "customeremail"
		columnCustomerEMail.FieldName = "customeremail"
		columnCustomerEMail.Visible = True
		columnCustomerEMail.BestFit()
		gvRP.Columns.Add(columnCustomerEMail)

		Dim columnZHdTelefon As New DevExpress.XtraGrid.Columns.GridColumn()
		columnZHdTelefon.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnZHdTelefon.Caption = m_xml.GetSafeTranslationValue("ZHD-Telefon")
		columnZHdTelefon.Name = "responsibletelefon"
		columnZHdTelefon.FieldName = "responsibletelefon"
		columnZHdTelefon.Visible = True
		columnZHdTelefon.BestFit()
		gvRP.Columns.Add(columnZHdTelefon)

		Dim columnZHDMobile As New DevExpress.XtraGrid.Columns.GridColumn()
		columnZHDMobile.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnZHDMobile.Caption = m_xml.GetSafeTranslationValue("ZHD-Natel")
		columnZHDMobile.Name = "responsiblemobile"
		columnZHDMobile.FieldName = "responsiblemobile"
		columnZHDMobile.Visible = True
		columnZHDMobile.BestFit()
		gvRP.Columns.Add(columnZHDMobile)

		Dim columnEMail As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEMail.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnEMail.Caption = m_xml.GetSafeTranslationValue("ZHD-EMail")
		columnEMail.Name = "responsibleemail"
		columnEMail.FieldName = "responsibleemail"
		columnEMail.Visible = True
		columnEMail.BestFit()
		gvRP.Columns.Add(columnEMail)


		Dim columncreator As New DevExpress.XtraGrid.Columns.GridColumn()
		columncreator.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncreator.Caption = m_xml.GetSafeTranslationValue("Ersteller")
		columncreator.Name = "creator"
		columncreator.FieldName = "creator"
		columncreator.Visible = False
		columncreator.BestFit()
		gvRP.Columns.Add(columncreator)

		Dim columncreatedon As New DevExpress.XtraGrid.Columns.GridColumn()
		columncreatedon.Caption = m_xml.GetSafeTranslationValue("Erstellt am")
		columncreatedon.Name = "createdon"
		columncreatedon.FieldName = "createdon"
		columncreatedon.Visible = True
		columncreatedon.BestFit()
		gvRP.Columns.Add(columncreatedon)

		Dim columncreatedfrom As New DevExpress.XtraGrid.Columns.GridColumn()
		columncreatedfrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncreatedfrom.Caption = m_xml.GetSafeTranslationValue("Erstellt durch")
		columncreatedfrom.Name = "createdfrom"
		columncreatedfrom.FieldName = "createdfrom"
		columncreatedfrom.Visible = True
		columncreatedfrom.BestFit()
		gvRP.Columns.Add(columncreatedfrom)

		Dim columnAdvisor As New DevExpress.XtraGrid.Columns.GridColumn()
		columnAdvisor.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnAdvisor.Caption = m_xml.GetSafeTranslationValue("Berater")
		columnAdvisor.Name = "adviser"
		columnAdvisor.FieldName = "adviser"
		columnAdvisor.Visible = True
		columnAdvisor.BestFit()
		gvRP.Columns.Add(columnAdvisor)

		Dim columnOffice As New DevExpress.XtraGrid.Columns.GridColumn()
		columnOffice.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnOffice.Caption = m_xml.GetSafeTranslationValue("Filiale")
		columnOffice.Name = "office"
		columnOffice.FieldName = "office"
		columnOffice.Visible = False
		gvRP.Columns.Add(columnOffice)

		Dim columnisexported As New DevExpress.XtraGrid.Columns.GridColumn()
		columnisexported.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnisexported.Caption = m_xml.GetSafeTranslationValue("Exportiert?")
		columnisexported.Name = "isexported"
		columnisexported.FieldName = "isexported"
		columnisexported.Visible = True
		gvRP.Columns.Add(columnisexported)

		Dim columnjchonline As New DevExpress.XtraGrid.Columns.GridColumn()
		columnjchonline.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnjchonline.Caption = m_xml.GetSafeTranslationValue("Jobs.ch?")
		columnjchonline.Name = "jchonline"
		columnjchonline.FieldName = "jchonline"
		columnjchonline.Visible = True
		gvRP.Columns.Add(columnjchonline)

		Dim columnostjobonline As New DevExpress.XtraGrid.Columns.GridColumn()
		columnostjobonline.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnostjobonline.Caption = m_xml.GetSafeTranslationValue("Ostjob.ch?")
		columnostjobonline.Name = "ostjobonline"
		columnostjobonline.FieldName = "ostjobonline"
		columnostjobonline.Visible = True
		gvRP.Columns.Add(columnostjobonline)


		RestoreGridLayoutFromXml()

		grdRP.DataSource = Nothing

	End Sub


#Region "Menüs..."

	'''' <summary>
	'''' Vakanzenverwaltung öffnen
	'''' </summary>
	'''' <param name="sender"></param>
	'''' <param name="e"></param>
	'''' <remarks></remarks>
	'Sub OpenMyVAK(ByVal sender As Object, ByVal e As EventArgs)
	'	Dim iVakNr As Integer = 0

	'	iVakNr = CInt(_ClsFunc.GetVakNr)
	'	RunOpenVAKForm(iVakNr)
	'End Sub

	'''' <summary>
	'''' Kandidatenverwaltung öffnen
	'''' </summary>
	'''' <param name="sender"></param>
	'''' <param name="e"></param>
	'''' <remarks></remarks>
	'Sub OpenMyMA(ByVal sender As Object, ByVal e As EventArgs)
	'	Dim iMANr As Integer = 0
	'	iMANr = CInt(_ClsFunc.GetMANr)
	'	RunOpenMAForm(iMANr)
	'End Sub

	''' <summary>
	''' Kundenverwaltung öffnen
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Sub OpenMyKD(ByVal sender As Object, ByVal e As EventArgs)
		Dim iKDNr As Integer = 0
		iKDNr = CInt(_ClsFunc.GetKDNr)
		RunOpenKDform(iKDNr)
	End Sub

	''' <summary>
	''' Zuständige Person - Verwaltung öffnen
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Sub OpenMyKDZHD(ByVal sender As Object, ByVal e As EventArgs)
		Dim iKDZHDNr As Integer = 0
		Dim iKDNr As Integer = 0
		iKDNr = CInt(_ClsFunc.GetKDNr)
		iKDZHDNr = CInt(_ClsFunc.GetKDZHDNr)
		RunOpenKDZHDform(iKDNr, iKDZHDNr)
	End Sub

	''' <summary>
	''' Einsatzverwaltung öffnen
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Sub OpenMyES(ByVal sender As Object, ByVal e As EventArgs)
		Dim iESNr As Integer = 0

		iESNr = CInt(_ClsFunc.GetESNr)
		RunOpenESForm(iESNr)
	End Sub

	'''' <summary>
	'''' Telefonmanager für Kunden öffnen
	'''' </summary>
	'''' <param name="sender"></param>
	'''' <param name="e"></param>
	'''' <remarks></remarks>
	'Sub CallKDTapi(ByVal sender As Object, ByVal e As EventArgs)
	'	Dim iKDNr As Integer = 0
	'	iKDNr = CInt(_ClsFunc.GetKDNr)
	'	Dim tool As ToolStripButton = DirectCast(sender, ToolStripButton)
	'	'RunTapi(tool.Tag.ToString, 0, iKDNr, 0)
	'End Sub

	'''' <summary>
	'''' Telefonmanager für die Zuständige Person öffnen
	'''' </summary>
	'''' <param name="sender"></param>
	'''' <param name="e"></param>
	'''' <remarks></remarks>
	'Sub CallKDZHDTapi(ByVal sender As Object, ByVal e As EventArgs)
	'	Dim iKDZHDNr As Integer = 0
	'	Dim iKDNr As Integer = 0
	'	iKDNr = CInt(_ClsFunc.GetKDNr)
	'	iKDZHDNr = CInt(_ClsFunc.GetKDZHDNr)
	'	Dim tool As ToolStripButton = DirectCast(sender, ToolStripButton)

	'	'RunTapi(tool.Tag.ToString, 0, iKDNr, iKDZHDNr)

	'End Sub


	Sub CallMailTo(ByVal sender As Object, ByVal e As EventArgs)
		MailTo(sender.ToString)
	End Sub

#End Region

#Region "Properties..."

	Dim _imouseX As Integer
	Property GetMouseP_X() As Integer
		Get
			Return _imouseX
		End Get
		Set(ByVal value As Integer)
			_imouseX = value
		End Set
	End Property

	Dim _imouseY As Integer
	Property GetMouseP_Y() As Integer
		Get
			Return _imouseY
		End Get
		Set(ByVal value As Integer)
			_imouseY = value
		End Set
	End Property

	Dim _iColIndex As Integer
	Property GetLV_ColIndex() As Integer
		Get
			Return _iColIndex
		End Get
		Set(ByVal value As Integer)
			_iColIndex = value
		End Set
	End Property

	Dim _iColSort As String
	Property GetLV_ColSort() As String
		Get
			Return _iColSort
		End Get
		Set(ByVal value As String)
			_iColSort = value
		End Set
	End Property

#End Region


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

	Private Sub OnfrmLoad(sender As Object, e As System.EventArgs) Handles Me.Load
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

			AddHandler Me.gvRP.ColumnFilterChanged, AddressOf OngvMain_ColumnFilterChanged
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
					Case "customername", "kdnr"
						If viewData.customername.Length > 0 Then RunOpenKDform(viewData.KDNr)

					Case "customeremail"
						If viewData.customeremail.Length > 0 Then MailTo(viewData.customeremail)
					Case "customertelefon"
						If viewData.customertelefon.Length > 0 Then StartCalling(viewData.customertelefon) ', 0, viewData.KDNr, 0)


					Case "responsiblename", "zhdnr"
						If viewData.responsiblename.Length > 0 Then RunOpenKDZHDform(viewData.KDNr, viewData.ZHDNr)

					Case "responsibleemail"
						If viewData.responsibleemail.Length > 0 Then MailTo(viewData.responsibleemail)

					Case "responsibletelefon"
						If viewData.responsibletelefon.Length > 0 Then StartCalling(viewData.responsibletelefon) ', 0, viewData.KDNr, viewData.KDNr)

					Case "responsiblemobile"
						If viewData.responsiblemobile.Length > 0 Then StartCalling(viewData.responsiblemobile) ', 0, viewData.KDNr, viewData.KDNr)


					Case Else
						If viewData.VakNr > 0 Then RunOpenVAKForm(viewData.VakNr, viewData.KDNr, viewData.ZHDNr)


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

	Private Sub OngvMain_ColumnFilterChanged(sender As Object, e As System.EventArgs)

		Me.bsiInfo.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True
		Me.bsiInfo.Caption = String.Format(m_xml.GetSafeTranslationValue("Anzahl Datensätze: {0}"), Me.gvRP.RowCount)

	End Sub

	Private Sub OngvColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvRP.SaveLayoutToXml(m_GVVacancySearchSettingfilename)

	End Sub

	Private Sub RestoreGridLayoutFromXml()
		Dim keepFilter = False
		Dim restoreLayout = True

		Try
			restoreLayout = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreVacancySearchSetting), True)
			keepFilter = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingVacancySearchFilter), False)
		Catch ex As Exception

		End Try

		If File.Exists(m_GVVacancySearchSettingfilename) Then gvRP.RestoreLayoutFromXml(m_GVVacancySearchSettingfilename)

		If restoreLayout AndAlso Not keepFilter Then gvRP.ActiveFilterCriteria = Nothing

	End Sub



End Class