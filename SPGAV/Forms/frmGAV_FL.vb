

Option Strict Off

Imports System.Reflection.Assembly
Imports System.IO.File
Imports System.Data

Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports System.Runtime.CompilerServices

Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.ProgPath
Imports SPProgUtility.Mandanten

Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Utility

Imports DevExpress.XtraBars.Alerter
Imports DevExpress.XtraBars
Imports DevExpress.Utils.Menu
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraRichEdit
Imports System.Drawing.Printing
Imports SPProgUtility.SPUserSec.ClsUserSec

Imports SPGAV.ClsDataDetail
Imports System.ComponentModel

Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Common.DataObjects

Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.ES
Imports SP.DatabaseAccess.ES.DataObjects.ESMng
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.DatabaseAccess.Employee
Imports SPGAV.SPPVLGAVUtilWebService
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports DevExpress.XtraEditors

Public Class frmGAV_FL
	Inherits DevExpress.XtraEditors.XtraForm


#Region "Constants"

	Private Const DEFAULT_SPUTNIK_PVL_UTILITIES_WEBSERVICE_URI = "wsSPS_services/SPPVLGAVUtil.asmx" ' "http://asmx.domain.com/wsSPS_services/SPPVLGAVUtil.asmx"
	Private Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"

#End Region


#Region "private fields"

	Protected Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private m_CommonDatabaseAccess As ICommonDatabaseAccess
	Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess
	Private m_CustomerDatabaseAccess As ICustomerDatabaseAccess
	Private m_ESDatabaseAccess As IESDatabaseAccess



	Private _MAAlter As String = "19"
	Private _KDPLZ As String = "5000"

	Private m_path As ClsProgPath
	Private m_md As Mandant
	Private m_UtilityUI As UtilityUI


	Private m_SPPVLUtilitiesServiceUrl As String

	Private m_ExistingEmployeeData As EmployeeMasterData
	Private m_ExistingCustomerGAVData As IEnumerable(Of CustomerAssignedGAVGroupData)
	Private m_ExistingESLohnData As IEnumerable(Of ESSalaryData)

	Private m_Gruppe0ListData As IEnumerable(Of FLGAVGruppe0ResultDTO)
	Private m_Gruppe1ListData As IEnumerable(Of FLGAVGruppe1ResultDTO)
	Private m_Gruppe2ListData As IEnumerable(Of FLGAVGruppe2ResultDTO)
	Private m_Gruppe3ListData As IEnumerable(Of FLGAVGruppe3ResultDTO)
	Private m_AlterCategoryListData As IEnumerable(Of FLGAVTextResultDTO)

	Private m_CurrentGruppe0Data As FLGAVGruppe0ResultDTO
	Private m_CurrentGruppe1Data As FLGAVGruppe1ResultDTO
	Private m_CurrentGruppe2Data As FLGAVGruppe2ResultDTO
	Private m_CurrentGruppe3Data As FLGAVGruppe3ResultDTO
	Private m_CurrentAlterCategoryData As FLGAVTextResultDTO
	Private m_CurrentGAVSalary As FLGAVSalaryResultDTO



	''' <summary>
	''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
	''' </summary>
	Private m_SuppressUIEvents As Boolean = False


#End Region


#Region "Public Properties"

	Public Property EmployeeNumber As Integer
	Public Property CustomerNumber As Integer
	Public Property EmploymentNumber As Integer
	Public Property CustomerCanton As String
	Public Property ExistingGAVInfo As String

#End Region


#Region "private properties"

	Private ReadOnly Property SelectedPVLData As FLGAVGruppe0ResultDTO
		Get
			Dim SelectedData = TryCast(lueGAVGruppe0.GetSelectedDataRow(), FLGAVGruppe0ResultDTO)
			If SelectedData Is Nothing Then SelectedData = TryCast(lueGAVGruppe0.EditValue, FLGAVGruppe0ResultDTO)

			Return SelectedData
		End Get

	End Property

	Private ReadOnly Property SelectedGruppe1Data As FLGAVGruppe1ResultDTO
		Get
			Dim SelectedData = TryCast(lueGAVGruppe1.GetSelectedDataRow(), FLGAVGruppe1ResultDTO)
			If SelectedData Is Nothing Then SelectedData = TryCast(lueGAVGruppe1.EditValue, FLGAVGruppe1ResultDTO)

			Return SelectedData
		End Get

	End Property

	Private ReadOnly Property SelectedGruppe2Data As FLGAVGruppe2ResultDTO
		Get
			Dim SelectedData = TryCast(lueGAVGruppe2.GetSelectedDataRow(), FLGAVGruppe2ResultDTO)
			If SelectedData Is Nothing Then SelectedData = TryCast(lueGAVGruppe2.EditValue, FLGAVGruppe2ResultDTO)

			Return SelectedData
		End Get

	End Property

	Private ReadOnly Property SelectedGruppe3Data As FLGAVGruppe3ResultDTO
		Get
			Dim SelectedData = TryCast(lueGAVGruppe3.GetSelectedDataRow(), FLGAVGruppe3ResultDTO)
			If SelectedData Is Nothing Then SelectedData = TryCast(lueGAVGruppe3.EditValue, FLGAVGruppe3ResultDTO)

			Return SelectedData
		End Get

	End Property

	Private ReadOnly Property SelectedAlterCategoryData As FLGAVTextResultDTO
		Get
			Dim SelectedData = TryCast(lueAlterKategorie.GetSelectedDataRow(), FLGAVTextResultDTO)
			If SelectedData Is Nothing Then SelectedData = TryCast(lueAlterKategorie.EditValue, FLGAVTextResultDTO)

			Return SelectedData
		End Get

	End Property


#End Region


#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
		m_md = New Mandant
		m_path = New ClsProgPath
		m_UtilityUI = New UtilityUI


		Dim domainName = m_InitializationData.MDData.WebserviceDomain
		m_SPPVLUtilitiesServiceUrl = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_PVL_UTILITIES_WEBSERVICE_URI)

		InitializeComponent()

		Me.KeyPreview = True
		Dim strStyleName As String = m_md.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, 0, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		Dim m_connectionString = m_InitializationData.MDData.MDDbConn
		m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_CustomerDatabaseAccess = New CustomerDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_ESDatabaseAccess = New ESDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

		TranslateControls()
		Reset()

		InitialCommonControls()

	End Sub


#End Region


#Region "public methodes"

	Public Sub LoadData()

		Dim suppressUIEventsState = m_SuppressUIEvents
		m_SuppressUIEvents = True

		LoadEmployeeData()
		LoadCustomerData()
		If EmploymentNumber > 0 Then LoadEmploymentData()

		m_ExistingCustomerGAVData = m_CustomerDatabaseAccess.LoadAssignedGAVGroupDataOfCustomer(CustomerNumber)

		LoadGruppe0DropDownData()

		If EmploymentNumber > 0 Then
			m_ExistingESLohnData = m_ESDatabaseAccess.LoadESSalaryData(EmploymentNumber)

			Dim actualESLohnData = m_ExistingESLohnData.Where(Function(x) x.AktivLODaten = True).ToList()
			Dim esGAVData As IEnumerable(Of FLGAVGruppe0ResultDTO) = Nothing
			If Not actualESLohnData Is Nothing AndAlso actualESLohnData.Count > 0 Then
				esGAVData = m_Gruppe0ListData.Where(Function(x) x.Gruppe0Label = actualESLohnData(0).GAVGruppe0).ToList()
			End If
			If Not esGAVData Is Nothing AndAlso esGAVData.Count > 0 Then
				m_Gruppe0ListData = esGAVData

			End If

		ElseIf CustomerNumber > 0 Then
			'Dim mainPVLData As New BindingList(Of FLGAVGruppe0ResultDTO)
			'For Each itm In m_ExistingCustomerGAVData
			'	Dim data = m_Gruppe0ListData.Where(Function(x) x.Gruppe0Label = itm.Description).FirstOrDefault()
			'	If Not data Is Nothing Then
			'		mainPVLData.Add(data)
			'	End If
			'Next

			'm_Gruppe0ListData = mainPVLData
		End If

		lueGAVGruppe0.Properties.DataSource = m_Gruppe0ListData
		If m_Gruppe0ListData.Count = 1 Then lueGAVGruppe0.EditValue = m_Gruppe0ListData(0).Gruppe0Label

		m_SuppressUIEvents = suppressUIEventsState

	End Sub


#End Region


	Private Sub InitialCommonControls()

		AddHandler lueGAVGruppe0.ButtonClick, AddressOf OnDropDown_ButtonClick

		AddHandler lueGAVGruppe1.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueGAVGruppe2.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueGAVGruppe3.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueAlterKategorie.ButtonClick, AddressOf OnDropDown_ButtonClick

	End Sub

	Private Sub OnDropDown_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

		Const ID_OF_DELETE_BUTTON As Int32 = 1

		' If delete button has been clicked reset the drop down.
		If e.Button.Index = ID_OF_DELETE_BUTTON Then

			If TypeOf sender Is LookUpEdit Then
				Dim lookupEdit As LookUpEdit = CType(sender, LookUpEdit)
				lookupEdit.EditValue = Nothing
			ElseIf TypeOf sender Is GridLookUpEdit Then
				Dim dateEdit As GridLookUpEdit = CType(sender, GridLookUpEdit)
				dateEdit.EditValue = Nothing
			End If
		End If
	End Sub



	Private Sub Reset()

		ResetGruppe0DropDown()
		ResetGruppe1DropDown()
		ResetGruppe2DropDown()
		ResetGruppe3DropDown()
		ResetAlterCategoryDropDown()
		ResetSalayDetails()

	End Sub

#Region "reset dropdowns"

	Private Sub ResetGruppe0DropDown()

		lueGAVGruppe0.Properties.DisplayMember = "Gruppe0Label"
		lueGAVGruppe0.Properties.ValueMember = "Gruppe0Label"

		Dim columns = lueGAVGruppe0.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("Gruppe0Label", 0, m_Translate.GetSafeTranslationValue("GAV-Beruf")))

		lueGAVGruppe0.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueGAVGruppe0.Properties.SearchMode = SearchMode.AutoComplete
		lueGAVGruppe0.Properties.AutoSearchColumnIndex = 0
		lueGAVGruppe0.Properties.NullText = String.Empty
		lueGAVGruppe0.EditValue = Nothing

	End Sub

	Private Sub ResetGruppe1DropDown()

		lueGAVGruppe1.Properties.DisplayMember = "Gruppe1Label"
		lueGAVGruppe1.Properties.ValueMember = "Gruppe1Label"

		Dim columns = lueGAVGruppe1.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("Gruppe1Label", 0, "1. " & m_Translate.GetSafeTranslationValue("Kategorie")))

		lueGAVGruppe1.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueGAVGruppe1.Properties.SearchMode = SearchMode.AutoComplete
		lueGAVGruppe1.Properties.AutoSearchColumnIndex = 0
		lueGAVGruppe1.Properties.NullText = String.Empty
		lueGAVGruppe1.EditValue = Nothing

	End Sub

	Private Sub ResetGruppe2DropDown()

		lueGAVGruppe2.Properties.DisplayMember = "Gruppe2Label"
		lueGAVGruppe2.Properties.ValueMember = "Gruppe2Label"

		Dim columns = lueGAVGruppe2.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("Gruppe2Label", 0, "2. " & m_Translate.GetSafeTranslationValue("Kategorie")))

		lueGAVGruppe2.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueGAVGruppe2.Properties.SearchMode = SearchMode.AutoComplete
		lueGAVGruppe2.Properties.AutoSearchColumnIndex = 0
		lueGAVGruppe2.Properties.NullText = String.Empty
		lueGAVGruppe2.EditValue = Nothing

	End Sub

	Private Sub ResetGruppe3DropDown()

		lueGAVGruppe3.Properties.DisplayMember = "Gruppe3Label"
		lueGAVGruppe3.Properties.ValueMember = "Gruppe3Label"

		Dim columns = lueGAVGruppe3.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("Gruppe3Label", 0, "3. " & m_Translate.GetSafeTranslationValue("Kategorie")))

		lueGAVGruppe3.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueGAVGruppe3.Properties.SearchMode = SearchMode.AutoComplete
		lueGAVGruppe3.Properties.AutoSearchColumnIndex = 0
		lueGAVGruppe3.Properties.NullText = String.Empty
		lueGAVGruppe3.EditValue = Nothing

	End Sub

	Private Sub ResetAlterCategoryDropDown()

		lueAlterKategorie.Properties.DisplayMember = "GAVLabel"
		lueAlterKategorie.Properties.ValueMember = "GAVLabel"

		Dim columns = lueAlterKategorie.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("GAVLabel", 0, m_Translate.GetSafeTranslationValue("Alterskategorie")))

		lueAlterKategorie.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueAlterKategorie.Properties.SearchMode = SearchMode.AutoComplete
		lueAlterKategorie.Properties.AutoSearchColumnIndex = 0
		lueAlterKategorie.Properties.NullText = String.Empty
		lueAlterKategorie.EditValue = Nothing

	End Sub

	Private Sub ResetSalayDetails()

		txtMinlohn.EditValue = 0D
		txtFeiertagLohn.EditValue = 0D
		txtFerienLohn.EditValue = 0D
		txtLohn13.EditValue = 0D
		txtStdLohn.EditValue = 0D

		txtFeiertagLohn.EditValue = 0D
		lblFeierbtr.Text = 0D
		txtFerienLohn.EditValue = 0D
		lblFerienbtr.Text = 0D
		txtLohn13.EditValue = 0D
		lblLohn13btr.Text = 0D

		txtMinlohn.ReadOnly = True
		txtFeiertagLohn.ReadOnly = True
		txtFerienLohn.ReadOnly = True
		txtLohn13.ReadOnly = True
		txtStdLohn.ReadOnly = True
		txtFeiertagLohn.ReadOnly = True
		txtFerienLohn.ReadOnly = True
		txtLohn13.ReadOnly = True


		txtStdWeek.EditValue = 0D
		txtStdMonth.EditValue = 0D
		txtStdYear.EditValue = 0D

		txtFAG.EditValue = 0D
		txtFAN.EditValue = 0D
		txtWAG.EditValue = 0D
		txtWAN.EditValue = 0D
		txtVAG.EditValue = 0D
		txtVAN.EditValue = 0D

		txtFAG_S.EditValue = 0D
		txtFAN_S.EditValue = 0D
		txtWAG_S.EditValue = 0D
		txtWAN_S.EditValue = 0D
		txtVAG_S.EditValue = 0D
		txtVAN_S.EditValue = 0D
		txtFAG_M.EditValue = 0D
		txtFAN_M.EditValue = 0D
		txtWAG_M.EditValue = 0D
		txtWAN_M.EditValue = 0D
		txtVAG_M.EditValue = 0D
		txtVAN_M.EditValue = 0D
		txtFAG_J.EditValue = 0D
		txtFAN_J.EditValue = 0D
		txtWAG_J.EditValue = 0D
		txtWAN_J.EditValue = 0D
		txtVAG_J.EditValue = 0D
		txtVAN_J.EditValue = 0D

		txtMinlohn.EditValue = 0D
		txtMinlohn.EditValue = 0D
		txtMinlohn.EditValue = 0D
		txtMinlohn.EditValue = 0D
		txtMinlohn.EditValue = 0D
		txtMinlohn.EditValue = 0D
		txtMinlohn.EditValue = 0D
		txtMinlohn.EditValue = 0D
		txtMinlohn.EditValue = 0D

		lblZusatz1.Text = String.Empty
		lblZusatz2.Text = String.Empty
		lblZusatz3.Text = String.Empty
		lblZusatz4.Text = String.Empty
		lblZusatz5.Text = String.Empty
		lblZusatz6.Text = String.Empty
		lblZusatz7.Text = String.Empty

	End Sub

#End Region

	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
		CmdClose.Text = m_Translate.GetSafeTranslationValue(Me.CmdClose.Text)

		lblHeaderFett.Text = m_Translate.GetSafeTranslationValue(lblHeaderFett.Text)
		lblDatenQuelle.Text = m_Translate.GetSafeTranslationValue(lblDatenQuelle.Text)
		lblNoGarant.Text = m_Translate.GetSafeTranslationValue(Me.lblNoGarant.Text)

		lblKandidat.Text = m_Translate.GetSafeTranslationValue(Me.lblKandidat.Text)
		lblGeburtsdatum.Text = m_Translate.GetSafeTranslationValue(lblGeburtsdatum.Text)
		lblBewilligung.Text = m_Translate.GetSafeTranslationValue(lblBewilligung.Text)
		lblQualifikation.Text = m_Translate.GetSafeTranslationValue(Me.lblQualifikation.Text)
		lblKunde.Text = m_Translate.GetSafeTranslationValue(Me.lblKunde.Text)
		lblEinsatz.Text = m_Translate.GetSafeTranslationValue(Me.lblEinsatz.Text)

		grpGAVCategories.Text = m_Translate.GetSafeTranslationValue(Me.grpGAVCategories.Text)
		lblGAVBeruf.Text = m_Translate.GetSafeTranslationValue(Me.lblGAVBeruf.Text)
		lblGAVGruppe1.Text = m_Translate.GetSafeTranslationValue(Me.lblGAVGruppe1.Text)
		lblGAVGruppe2.Text = m_Translate.GetSafeTranslationValue(Me.lblGAVGruppe2.Text)
		lblGAVGruppe3.Text = m_Translate.GetSafeTranslationValue(Me.lblGAVGruppe3.Text)

		grpLODetails.Text = m_Translate.GetSafeTranslationValue(grpLODetails.Text)
		lblGrundlohn.Text = m_Translate.GetSafeTranslationValue(Me.lblGrundlohn.Text)
		lblFeiertag.Text = m_Translate.GetSafeTranslationValue(Me.lblFeiertag.Text)
		lblFerien.Text = m_Translate.GetSafeTranslationValue(Me.lblFerien.Text)
		lbl13Lohn.Text = m_Translate.GetSafeTranslationValue(Me.lbl13Lohn.Text)
		lblStdLohn.Text = m_Translate.GetSafeTranslationValue(Me.lblStdLohn.Text)

		grpSollStunden.Text = m_Translate.GetSafeTranslationValue(grpSollStunden.Text)
		lblStunden.Text = m_Translate.GetSafeTranslationValue(lblStunden.Text)
		lblWoche.Text = m_Translate.GetSafeTranslationValue(lblWoche.Text)
		lblMonat.Text = m_Translate.GetSafeTranslationValue(lblMonat.Text)
		lblJahr.Text = m_Translate.GetSafeTranslationValue(lblJahr.Text)

		xtabLohndaten.Text = m_Translate.GetSafeTranslationValue(xtabLohndaten.Text)
		xtabZusatzinfo.Text = m_Translate.GetSafeTranslationValue(xtabZusatzinfo.Text)

		grpParifond.Text = m_Translate.GetSafeTranslationValue(grpParifond.Text)
		lblFARAG.Text = m_Translate.GetSafeTranslationValue(lblFARAG.Text)
		lblFARAN.Text = m_Translate.GetSafeTranslationValue(lblFARAN.Text)
		lblWeiterbildungAG.Text = m_Translate.GetSafeTranslationValue(lblWeiterbildungAG.Text)
		lblWeiterbildungAN.Text = m_Translate.GetSafeTranslationValue(lblWeiterbildungAN.Text)
		lblVollzugAG.Text = m_Translate.GetSafeTranslationValue(lblVollzugAG.Text)
		lblVollzugAN.Text = m_Translate.GetSafeTranslationValue(lblVollzugAN.Text)

		bsiInfo.Caption = m_Translate.GetSafeTranslationValue(bsiInfo.Caption)
		Me.bbiSave.Caption = m_Translate.GetSafeTranslationValue(Me.bbiSave.Caption)
		Me.bbiPrint.Caption = m_Translate.GetSafeTranslationValue(Me.bbiPrint.Caption)

	End Sub

	Private Sub LoadEmployeeData()

		m_ExistingEmployeeData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(EmployeeNumber, False)

		If m_ExistingEmployeeData Is Nothing Then
			Dim msg As String = m_Translate.GetSafeTranslationValue("Kandidatendaten konnten nicht geladen werden.")
			m_UtilityUI.ShowErrorDialog(msg)
			m_Logger.LogError(msg)

			Return
		End If

		Me.lblQualifikationValue.Text = String.Format("({0}) {1}", m_ExistingEmployeeData.QLand, m_ExistingEmployeeData.Profession)
		Me.lblGebValue.Text = String.Format("({0:d}) {1:f0}", m_ExistingEmployeeData.Birthdate, m_ExistingEmployeeData.EmployeeSUVABirthdateAge)
		Me.lblMANR.Text = String.Format("({0}) {1}", EmployeeNumber, m_ExistingEmployeeData.EmployeeFullname)
		Me.lblBewValue.Text = String.Format("({0}) {1:d}", m_ExistingEmployeeData.Permission, m_ExistingEmployeeData.PermissionToDate)

		_MAAlter = String.Format("{0}", m_ExistingEmployeeData.EmployeeSUVABirthdateAge)

	End Sub

	Private Sub LoadCustomerData()

		Dim customerMasterData = m_CustomerDatabaseAccess.LoadCustomerMasterData(CustomerNumber, False)

		If customerMasterData Is Nothing Then
			Dim msg As String = m_Translate.GetSafeTranslationValue("Kundendaten konnten nicht geladen werden.")
			m_UtilityUI.ShowErrorDialog(msg)
			m_Logger.LogError(msg)

			Return
		End If

		Me.lblKDNR.Text = String.Format("({0}) {1}", customerMasterData.CustomerNumber, customerMasterData.Company1)
		_KDPLZ = String.Format("{0}", customerMasterData.Postcode)

	End Sub

	Private Sub LoadEmploymentData()

		If EmploymentNumber = 0 Then Return
		Dim employmentMasterData = m_ESDatabaseAccess.LoadESMasterData(EmploymentNumber)

		If employmentMasterData Is Nothing Then
			Dim msg As String = m_Translate.GetSafeTranslationValue("Einsatzdaten konnten nicht geladen werden.")
			m_UtilityUI.ShowErrorDialog(msg)
			m_Logger.LogError(msg)

			Return
		End If

		Me.lblESNR.Text = String.Format("{0} | {1:d} {2:d}", employmentMasterData.ES_Als, employmentMasterData.ES_Ab, employmentMasterData.ES_Ende)

	End Sub

	Private Function LoadGruppe0DropDownData() As Boolean

		Dim data = PerformFLGAVGruppe0Webservice()

		If data Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("GAV-Berufe konnen nicht geladen werden."))

			Return False
		End If
		m_Gruppe0ListData = data
		lueGAVGruppe0.Enabled = data.Count > 0

		lueGAVGruppe0.EditValue = Nothing
		lueGAVGruppe0.Properties.DataSource = data

		Return True

	End Function

	Private Function LoadGruppe1DropDownData() As Boolean

		Dim data = PerformFLGAVGruppe1Webservice()

		If data Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Gruppe1-Daten konnen nicht geladen werden."))

			Return False
		End If
		m_Gruppe1ListData = data
		lueGAVGruppe1.Enabled = data.Count > 0

		If data.Count = 0 Then
			lueGAVGruppe1.Enabled = False

			LoadGruppe2DropDownData()

			Return True
		End If

		lueGAVGruppe1.EditValue = Nothing
		lueGAVGruppe1.Properties.DataSource = data

		Return True

	End Function

	Private Function LoadGruppe2DropDownData() As Boolean

		Dim data = PerformFLGAVGruppe2Webservice()

		If data Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Gruppe2-Daten konnen nicht geladen werden."))

			Return False
		End If
		m_Gruppe2ListData = data
		lueGAVGruppe2.Enabled = data.Count > 0

		If data.Count = 0 Then
			lueGAVGruppe2.Enabled = False

			LoadGruppe3DropDownData()

			Return True
		End If

		lueGAVGruppe2.EditValue = Nothing
		lueGAVGruppe2.Properties.DataSource = data

		Return True

	End Function

	Private Function LoadGruppe3DropDownData() As Boolean

		Dim data = PerformFLGAVGruppe3Webservice()

		If data Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Gruppe3-Daten konnen nicht geladen werden."))

			Return False
		End If
		m_Gruppe3ListData = data
		lueGAVGruppe3.Enabled = data.Count > 0

		If data.Count = 0 Then
			lueGAVGruppe3.Enabled = False

			LoadAlterCategoryDropDownData()

			Return True
		End If

		lueGAVGruppe3.EditValue = Nothing
		lueGAVGruppe3.Properties.DataSource = data

		Return True

	End Function

	Private Function LoadAlterCategoryDropDownData() As Boolean

		Dim data = PerformFLGAVTextWebservice()

		If data Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Alterkategorie-Daten konnen nicht geladen werden."))

			Return False
		End If
		m_AlterCategoryListData = data
		lueAlterKategorie.Enabled = data.Count > 0

		lueAlterKategorie.EditValue = Nothing
		lueAlterKategorie.Properties.DataSource = data

		Return True

	End Function

	Private Function LoadSalaryData() As Boolean

		Dim data = PerformFLGAVSakaryWebservice()

		If data Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Lohndaten konnen nicht geladen werden."))

			Return False
		End If
		If data.ID > 0 Then
			DisplaySalaryDetails()
		Else
			ResetSalayDetails()
		End If

		Return True

	End Function

	Private Sub OnlueGAVGruppe0_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lueGAVGruppe0.EditValueChanged

		Try
			If lueGAVGruppe0.EditValue Is Nothing Then Return

			m_CurrentGruppe0Data = SelectedPVLData

			Dim suppressUIEventsState = m_SuppressUIEvents
			m_SuppressUIEvents = True

			ResetGruppe1DropDown()
			LoadGruppe1DropDownData()

			m_SuppressUIEvents = suppressUIEventsState


		Catch ex As Exception

		End Try

	End Sub

	Private Sub OnlueGAVGruppe1_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lueGAVGruppe1.EditValueChanged

		Try
			If lueGAVGruppe0.EditValue Is Nothing Then Return

			m_CurrentGruppe1Data = SelectedGruppe1Data

			Dim suppressUIEventsState = m_SuppressUIEvents
			m_SuppressUIEvents = True

			ResetGruppe2DropDown()
			LoadGruppe2DropDownData()

			m_SuppressUIEvents = suppressUIEventsState


		Catch ex As Exception

		End Try

	End Sub

	Private Sub OnlueGAVGruppe2_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lueGAVGruppe2.EditValueChanged

		Try
			If lueGAVGruppe0.EditValue Is Nothing Then Return

			m_CurrentGruppe2Data = SelectedGruppe2Data

			Dim suppressUIEventsState = m_SuppressUIEvents
			m_SuppressUIEvents = True

			ResetGruppe3DropDown()
			LoadGruppe3DropDownData()

			m_SuppressUIEvents = suppressUIEventsState


		Catch ex As Exception

		End Try

	End Sub

	Private Sub OnlueGAVGruppe3_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lueGAVGruppe3.EditValueChanged

		Try
			If lueGAVGruppe0.EditValue Is Nothing Then Return

			m_CurrentGruppe3Data = SelectedGruppe3Data

			Dim suppressUIEventsState = m_SuppressUIEvents
			m_SuppressUIEvents = True

			ResetAlterCategoryDropDown()
			LoadaltercategoryDropDownData

			m_SuppressUIEvents = suppressUIEventsState


		Catch ex As Exception

		End Try

	End Sub

	Private Sub OnlueAlterKategorie_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lueAlterKategorie.EditValueChanged

		Try
			If lueGAVGruppe0.EditValue Is Nothing Then Return

			m_CurrentAlterCategoryData = SelectedAlterCategoryData

			Dim suppressUIEventsState = m_SuppressUIEvents
			m_SuppressUIEvents = True

			ResetSalayDetails()
			LoadSalaryData()


			m_SuppressUIEvents = suppressUIEventsState


		Catch ex As Exception

		End Try

	End Sub

	Private Sub DisplaySalaryDetails()
		Dim data = m_CurrentGAVSalary

		txtMinlohn.EditValue = data.Minlohn
		txtFeiertagLohn.EditValue = data.FeiertagLohn
		txtFerienLohn.EditValue = data.FerienLohn
		txtLohn13.EditValue = data.Lohn13
		txtStdLohn.EditValue = data.StdLohn

		txtFeiertagLohn.EditValue = data.FeiertagLohn
		lblFeierbtr.Text = String.Format("{0:n2}", data.Feierbtr)
		txtFerienLohn.EditValue = data.FerienLohn
		lblFerienbtr.Text = String.Format("{0:n2}", data.Ferienbtr)
		txtLohn13.EditValue = data.Lohn13
		lblLohn13btr.Text = String.Format("{0:n2}", data.Lohn13btr)

		txtStdWeek.EditValue = data.StdWeek
		txtStdMonth.EditValue = data.StdMonth
		txtStdYear.EditValue = data.StdYear

		txtFAG.EditValue = data.FAG
		txtFAN.EditValue = data.FAN
		txtWAG.EditValue = data.WAG
		txtWAN.EditValue = data.WAN
		txtVAG.EditValue = data.VAG
		txtVAN.EditValue = data.VAN

		txtFAG_S.EditValue = data.FAG_S
		txtFAN_S.EditValue = data.FAN_S
		txtWAG_S.EditValue = data.WAG_S
		txtWAN_S.EditValue = data.WAN_S
		txtVAG_S.EditValue = data.VAG_S
		txtVAN_S.EditValue = data.VAN_S
		txtFAG_M.EditValue = data.FAG_M
		txtFAN_M.EditValue = data.FAN_M
		txtWAG_M.EditValue = data.WAG_M
		txtWAN_M.EditValue = data.WAN_M
		txtVAG_M.EditValue = data.VAG_M
		txtVAN_M.EditValue = data.VAN_M
		txtFAG_J.EditValue = data.FAG_J
		txtFAN_J.EditValue = data.FAN_J
		txtWAG_J.EditValue = data.WAG_J
		txtWAN_J.EditValue = data.WAN_J
		txtVAG_J.EditValue = data.VAG_J
		txtVAN_J.EditValue = data.VAN_J

		lblZusatz1.Text = data.Zusatz1
		lblZusatz2.Text = data.Zusatz2
		lblZusatz3.Text = data.Zusatz3
		lblZusatz4.Text = data.Zusatz4
		lblZusatz5.Text = data.Zusatz5
		lblZusatz6.Text = data.Zusatz6
		lblZusatz7.Text = data.Zusatz7

	End Sub

	Private Function PerformFLGAVGruppe0Webservice() As BindingList(Of FLGAVGruppe0ResultDTO)

		Dim listDataSource As BindingList(Of FLGAVGruppe0ResultDTO) = New BindingList(Of FLGAVGruppe0ResultDTO)

		Dim webservice = New SPPVLGAVUtilWebService.SPPVLGAVUtilSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_SPPVLUtilitiesServiceUrl)

		' Read data over webservice
		Dim searchResult = webservice.GetFLGAVGruppe0Data(m_InitializationData.MDData.MDGuid, m_InitializationData.UserData.UserLanguage).ToList

		For Each result In searchResult

			Dim viewData = New FLGAVGruppe0ResultDTO With {
						.Gruppe0Label = result.Gruppe0Label
			}

			listDataSource.Add(viewData)

		Next

		m_Gruppe0ListData = listDataSource

		Return listDataSource

	End Function

	Private Function PerformFLGAVGruppe1Webservice() As BindingList(Of FLGAVGruppe1ResultDTO)

		Dim listDataSource As BindingList(Of FLGAVGruppe1ResultDTO) = New BindingList(Of FLGAVGruppe1ResultDTO)

		Dim webservice = New SPPVLGAVUtilWebService.SPPVLGAVUtilSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_SPPVLUtilitiesServiceUrl)

		' Read data over webservice
		Dim searchResult = webservice.GetFLGAVGruppe1Data(m_InitializationData.MDData.MDGuid, lueGAVGruppe0.EditValue, m_InitializationData.UserData.UserLanguage).ToList

		For Each result In searchResult

			Dim viewData = New FLGAVGruppe1ResultDTO With {
						.Gruppe1Label = result.Gruppe1Label
			}

			listDataSource.Add(viewData)

		Next
		m_Gruppe1ListData = listDataSource

		Return listDataSource

	End Function

	Private Function PerformFLGAVGruppe2Webservice() As BindingList(Of FLGAVGruppe2ResultDTO)

		Dim listDataSource As BindingList(Of FLGAVGruppe2ResultDTO) = New BindingList(Of FLGAVGruppe2ResultDTO)

		Dim webservice = New SPPVLGAVUtilWebService.SPPVLGAVUtilSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_SPPVLUtilitiesServiceUrl)

		' Read data over webservice
		Dim searchResult = webservice.GetFLGAVgruppe2Data(m_InitializationData.MDData.MDGuid, lueGAVGruppe0.EditValue, lueGAVGruppe1.EditValue, m_InitializationData.UserData.UserLanguage).ToList

		For Each result In searchResult

			Dim viewData = New FLGAVGruppe2ResultDTO With {
						.Gruppe2Label = result.Gruppe2Label
			}

			listDataSource.Add(viewData)

		Next
		m_Gruppe2ListData = listDataSource

		Return listDataSource

	End Function

	Private Function PerformFLGAVGruppe3Webservice() As BindingList(Of FLGAVGruppe3ResultDTO)

		Dim listDataSource As BindingList(Of FLGAVGruppe3ResultDTO) = New BindingList(Of FLGAVGruppe3ResultDTO)

		Dim webservice = New SPPVLGAVUtilWebService.SPPVLGAVUtilSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_SPPVLUtilitiesServiceUrl)

		' Read data over webservice
		Dim searchResult = webservice.GetFLGAVgruppe3Data(m_InitializationData.MDData.MDGuid, lueGAVGruppe0.EditValue, lueGAVGruppe1.EditValue, lueGAVGruppe2.EditValue, m_InitializationData.UserData.UserLanguage).ToList

		For Each result In searchResult

			Dim viewData = New FLGAVGruppe3ResultDTO With {
						.Gruppe3Label = result.Gruppe3Label
			}

			listDataSource.Add(viewData)

		Next
		m_Gruppe3ListData = listDataSource

		Return listDataSource

	End Function

	Private Function PerformFLGAVTextWebservice() As BindingList(Of FLGAVTextResultDTO)

		Dim listDataSource As BindingList(Of FLGAVTextResultDTO) = New BindingList(Of FLGAVTextResultDTO)

		Dim webservice = New SPPVLGAVUtilWebService.SPPVLGAVUtilSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_SPPVLUtilitiesServiceUrl)

		' Read data over webservice
		Dim searchResult = webservice.GetFLGAVTextData(m_InitializationData.MDData.MDGuid, lueGAVGruppe0.EditValue, lueGAVGruppe1.EditValue, lueGAVGruppe2.EditValue, lueGAVGruppe3.EditValue, m_InitializationData.UserData.UserLanguage).ToList

		For Each result In searchResult

			Dim viewData = New FLGAVTextResultDTO With {
						.GAVLabel = result.GAVLabel
			}

			listDataSource.Add(viewData)

		Next
		m_AlterCategoryListData = listDataSource

		Return listDataSource

	End Function

	Private Function PerformFLGAVSakaryWebservice() As FLGAVSalaryResultDTO

		Dim listDataSource As FLGAVSalaryResultDTO = Nothing

		Dim webservice = New SPPVLGAVUtilWebService.SPPVLGAVUtilSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_SPPVLUtilitiesServiceUrl)

		' Read data over webservice
		Dim gruppe1 As String = String.Format("{0}", lueGAVGruppe1.EditValue)
		Dim gruppe2 As String = String.Format("{0}", lueGAVGruppe2.EditValue)
		Dim gruppe3 As String = String.Format("{0}", lueGAVGruppe3.EditValue)
		Dim gruppe4 As String = String.Format("{0}", lueAlterKategorie.EditValue)


		Dim searchResult = webservice.GetFLGAVSalaryData(m_InitializationData.MDData.MDGuid, lueGAVGruppe0.EditValue, gruppe1, gruppe2, gruppe3, gruppe4, m_InitializationData.UserData.UserLanguage)

		Dim viewData = New FLGAVSalaryResultDTO With {
			.ID = searchResult.ID,
			.GAVNr = searchResult.GAVNr,
			.GavKanton = searchResult.GavKanton,
			.Gruppe0 = searchResult.Gruppe0,
			.Gruppe1 = searchResult.Gruppe1,
			.Gruppe2 = searchResult.Gruppe2,
			.Gruppe3 = searchResult.Gruppe3,
			.GavText = searchResult.GavText,
			.CalcFerien = searchResult.CalcFerien,
			.Calc13Lohn = searchResult.Calc13Lohn,
			.Minlohn = searchResult.Minlohn,
			.FeiertagLohn = searchResult.FeiertagLohn,
			.Feierbtr = searchResult.Feierbtr,
			.FerienLohn = searchResult.FerienLohn,
			.Ferienbtr = searchResult.Ferienbtr,
			.Lohn13 = searchResult.Lohn13,
			.Lohn13btr = searchResult.Lohn13btr,
			.StdLohn = searchResult.StdLohn,
			.Monatslohn = searchResult.Monatslohn,
			.Mittagszulagen = searchResult.Mittagszulagen,
			.FAG = searchResult.FAG,
			.FAN = searchResult.FAN,
			.WAG = searchResult.WAG,
			.WAN = searchResult.WAN,
			.VAG = searchResult.VAG,
			.VAN = searchResult.VAN,
			.FAG_S = searchResult.FAG_S,
			.FAN_S = searchResult.FAN_S,
			.WAG_S = searchResult.WAG_S,
			.WAN_S = searchResult.WAN_S,
			.VAG_S = searchResult.VAG_S,
			.VAN_S = searchResult.VAN_S,
			.FAG_M = searchResult.FAG_M,
			.FAN_M = searchResult.FAN_M,
			.WAG_M = searchResult.WAG_M,
			.WAN_M = searchResult.WAN_M,
			.VAG_M = searchResult.VAG_M,
			.VAN_M = searchResult.VAN_M,
			.FAG_J = searchResult.FAG_J,
			.FAN_J = searchResult.FAN_J,
			.WAG_J = searchResult.WAG_J,
			.WAN_J = searchResult.WAN_J,
			.VAG_J = searchResult.VAG_J,
			.VAN_J = searchResult.VAN_J,
			.GueltigAb = searchResult.GueltigAb,
			.GueltigBis = searchResult.GueltigBis,
			.ZusatzFeier = searchResult.ZusatzFeier,
			.Zusatz13Lohn = searchResult.Zusatz13Lohn,
			.Ferientext = searchResult.Ferientext,
			.Lohn13text = searchResult.Lohn13text,
			.StdWeek = searchResult.StdWeek,
			.StdMonth = searchResult.StdMonth,
			.StdYear = searchResult.StdYear,
			.F_Alter = searchResult.F_Alter,
			.L_Alter = searchResult.L_Alter,
			.Zusatz1 = searchResult.Zusatz1,
			.Zusatz2 = searchResult.Zusatz2,
			.Zusatz3 = searchResult.Zusatz3,
			.Zusatz4 = searchResult.Zusatz4,
			.Zusatz5 = searchResult.Zusatz5,
			.Zusatz6 = searchResult.Zusatz6,
			.Zusatz7 = searchResult.Zusatz7,
			.Zusatz8 = searchResult.Zusatz8,
			.Zusatz9 = searchResult.Zusatz9,
			.Zusatz10 = searchResult.Zusatz10,
			.Zusatz11 = searchResult.Zusatz11,
			.Zusatz12 = searchResult.Zusatz12
		}

		listDataSource = viewData

		m_CurrentGAVSalary = listDataSource


		Return listDataSource

	End Function

	Private Sub bbiSave_ItemClick(sender As Object, e As ItemClickEventArgs) Handles bbiSave.ItemClick
		ClsDataDetail.strGAVData = GetSelectedFLGAVData()
		Me.Close()
	End Sub

	Private Sub CmdClose_Click(sender As Object, e As EventArgs) Handles CmdClose.Click
		Me.Close()
	End Sub

	Private Function GetSelectedFLGAVData() As String
		Dim result As String

		result = String.Format("GAVNr:{0}¦", m_CurrentGAVSalary.GAVNr)
		result &= "MetaNr:0¦"
		result &= "CalNr:0¦"
		result &= "CatNr:0¦"
		result &= "CatBaseNr:0¦"
		result &= "CatValueNr:0¦"
		result &= "LONr:0¦"
		result &= "Kanton:FL¦"
		result &= String.Format("Beruf:{0}¦", lueGAVGruppe0.EditValue)
		result &= String.Format("Gruppe1:{0}¦", lueGAVGruppe1.EditValue)
		result &= String.Format("Gruppe2:{0}¦", lueGAVGruppe2.EditValue)
		result &= String.Format("Gruppe3:{0}¦", lueGAVGruppe3.EditValue)
		result &= String.Format("Bezeichnung:{0}¦", lueAlterKategorie.EditValue)
		'result &= "Kanton:FL¦"
		result &= "Sonstige:0¦"
		result &= "Res_13: ¦"
		result &= "Res_14: ¦"
		result &= "Res_15: ¦"
		result &= "Res_16: ¦"
		result &= "Res_17: ¦"
		result &= "Res_18: ¦"
		result &= "Res_19: ¦"
		result &= "Monatslohn:0¦"
		result &= "FeiertagJahr:9¦"
		result &= "FierienJahr:20¦"
		result &= "13.Lohn:True¦"
		result &= String.Format("BasisLohn:{0:n2}¦", m_CurrentGAVSalary.Minlohn)
		result &= String.Format("FerienBetrag:{0:n5}¦", m_CurrentGAVSalary.Ferienbtr)
		result &= String.Format("FerienProz:{0:n5}¦", m_CurrentGAVSalary.FerienLohn / 100)
		result &= String.Format("FeierBetrag:{0:n2}¦", m_CurrentGAVSalary.Feierbtr)
		result &= String.Format("FeierProz:{0:n5}¦", m_CurrentGAVSalary.FeiertagLohn / 100)
		result &= String.Format("13.Betrag:{0:n2}¦", m_CurrentGAVSalary.Lohn13btr)
		result &= String.Format("13.Proz:{0:n5}¦", m_CurrentGAVSalary.Lohn13 / 100)


		Dim CalcFeierWay As Integer = 1
		Dim CalcFerienWay As Integer = m_CurrentGAVSalary.CalcFerien
		If CalcFerienWay = 0 Then CalcFerienWay = 3  ' Grundlohn + Feiertag
		If CalcFerienWay = 1 Then CalcFerienWay = 1  ' Grundlohn

		Dim calc13Way As Integer = m_CurrentGAVSalary.Calc13Lohn
		If calc13Way = 0 Then calc13Way = 1  ' Grundlohn
		If calc13Way = 1 Then calc13Way = 3  ' Grundlohn + Feier
		If calc13Way = 2 Then calc13Way = 4  ' Grundlohn + Feier + Ferien
		If calc13Way = 3 Then calc13Way = 2  ' Grundlohn + Ferien


		result &= String.Format("CalcFerien:{0:f0}¦", CalcFerienWay)
		result &= String.Format("CalcFeier:{0:f0}¦", CalcFeierWay)
		result &= String.Format("Calc13:{0:f0}¦", calc13Way)
		result &= String.Format("StdLohn:{0:n2}¦", m_CurrentGAVSalary.StdLohn)
		result &= String.Format("FARAN:{0:n2}¦", Val(txtFAN.EditValue))
		result &= String.Format("FARAG:{0:n2}¦", Val(txtFAG.EditValue))
		result &= String.Format("VAN:{0:n2}¦", Val(txtVAN.EditValue))
		result &= String.Format("VAG:{0:n2}¦", Val(txtVAG.EditValue))
		result &= String.Format("StdWeek:{0:n2}¦", Val(txtStdWeek.EditValue))
		result &= String.Format("StdMonth:{0:n2}¦", Val(txtStdMonth.EditValue))
		result &= String.Format("StdYear:{0:n2}¦", Val(txtStdYear.EditValue))
		result &= "IsPVL:0¦"
		result &= "_WAG:0¦"
		result &= "_WAN:0¦"
		result &= "_WAG_S:0¦"
		result &= "_WAN_S:0¦"
		result &= "_WAG_J:0¦"
		result &= "_WAN_J:0¦"
		result &= "_VAG:0.5¦"
		result &= "_VAN:0.7¦"
		result &= "_VAG_S:0¦"
		result &= "_VAN_S:0¦"
		result &= "_VAG_J:0¦"
		result &= "_VAN_J:0¦"
		result &= "_FAG:0¦"
		result &= "_FAN:0¦"
		result &= "BauQ12:0¦"
		result &= "iFANCalc:1¦"
		result &= "bFANWithBVG: False¦"


		Return result

	End Function

	Public Overloads Function ShowDialog() As String

		MyBase.ShowDialog()

		Return ClsDataDetail.strGAVData
	End Function


#Region "Form drucken..."

	' create a printing component
	Private WithEvents pd As PrintDocument
	Dim WithEvents mPrintDocument As New PrintDocument
	Dim mPrintBitMap As Bitmap

	Dim formImage As Bitmap
	Private Declare Function BitBlt Lib "gdi32.dll" Alias "BitBlt" (
				ByVal hdcDest As IntPtr, ByVal nXDest As Integer, ByVal _
				nYDest As Integer, ByVal nWidth As Integer, ByVal nHeight _
				As Integer, ByVal hdcSrc As IntPtr, ByVal nXSrc As _
				Integer, ByVal nYSrc As Integer, ByVal dwRop As _
				System.Int32) As Long

	Private Const SRCCOPY As Integer = &HCC0020
	Dim memoryImage As Bitmap

	' Callback from PrintDocument component to do the actual printing
	Private Sub pd_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles pd.PrintPage
		e.Graphics.DrawImage(formImage, 0, 0)
	End Sub

	Private Sub GetFormImage()
		Dim g As Graphics = Me.CreateGraphics()
		Dim s As Size = Me.Size ' Me.XtraTabControl1.Size
		formImage = New Bitmap(s.Width, s.Height, g)
		Dim mg As Graphics = Graphics.FromImage(formImage)
		Dim dc1 As IntPtr = g.GetHdc
		Dim dc2 As IntPtr = mg.GetHdc
		' added code to compute and capture the form 
		' title bar and borders 
		Dim widthDiff As Integer = (Me.Width - Me.ClientRectangle.Width)
		Dim heightDiff As Integer = (Me.Height - Me.ClientRectangle.Height)
		Dim borderSize As Integer = widthDiff \ 2
		Dim heightTitleBar As Integer = heightDiff - borderSize

		' Mit Titlebar!!!
		' BitBlt(dc2, 0, 0, Me.ClientRectangle.Width + widthDiff, Me.ClientRectangle.Height + heightDiff, dc1, 0 - borderSize, 0 - heightTitleBar, 13369376)

		BitBlt(dc2, 0, 10,
			 Me.ClientRectangle.Width - widthDiff,
			 Me.ClientRectangle.Height - heightDiff, dc1,
			 -10 + borderSize, -10 + heightTitleBar, 13369376)

		g.ReleaseHdc(dc1)
		mg.ReleaseHdc(dc2)

	End Sub

	Private Sub OnbbiPrint_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick

		If pd Is Nothing Then pd = New PrintDocument()

		Me.lblDatenQuelle.Text = String.Format("FL-GAV Abfrage. Gedruckt: {0} | {1}", m_InitializationData.UserData.UserFullName, Now.ToString)
		Me.Refresh()

		GetFormImage()
		pd.DefaultPageSettings.Landscape = False
		pd.Print()

		Me.lblDatenQuelle.Text = String.Format("FL-GAV Abfrage")
		Me.Refresh()

	End Sub


#End Region


End Class