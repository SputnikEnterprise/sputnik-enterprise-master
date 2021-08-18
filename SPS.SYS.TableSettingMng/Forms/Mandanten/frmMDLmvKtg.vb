﻿
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Common.DataObjects

Imports SP.DatabaseAccess.TableSetting
Imports SP.DatabaseAccess.TableSetting.DataObjects
Imports SP.DatabaseAccess.TableSetting.DataObjects.MandantData

Imports System.ComponentModel
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraNavBar
Imports DevExpress.XtraEditors
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraSplashScreen
Imports DevExpress.XtraEditors.Repository

Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Infrastructure

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages

Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.Mandanten
Imports SPS.SYS.TableSettingMng.SPPVLGAVUtilWebService
Imports System.Threading.Tasks
Imports System.Threading



Public Class frmMDLmvKtg


#Region "private consts"

	Private Const MANDANT_XML_MAIN_KEY As String = "MD_{0}/"
	Private Const MANDANT_XML_SETTING_SPUTNIK_PAYROLL_SETTING As String = "MD_{0}/Lohnbuchhaltung"
	Public Const DEFAULT_SPUTNIK_PVLGAV_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPPVLGAVUtil.asmx"

#End Region


#Region "Private Fields"

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The common data access object.
	''' </summary>
	Private m_CommonDatabaseAccess As ICommonDatabaseAccess

	Private m_TablesettingDatabaseAccess As ITablesDatabaseAccess
	''' <summary>
	''' The common data access object.
	''' </summary>
	Private m_MandantDatabaseAccess As ChildEducationData

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

	''' <summary>
	''' List of user controls.
	''' </summary>
	Private m_ListOfUserControls As New List(Of DevExpress.XtraEditors.XtraUserControl)

	Private m_ISAlreadySaved As Boolean

	Private connectionString As String

	''' <summary>
	''' Record number of selected row.
	''' </summary>
	Private m_CurrentRecordNumber As Integer?

	Private m_MandantXMLFile As String
	Private m_MandantFormXMLFileName As String
	Private m_MandantSetting As String
	Private m_PayrollSetting As String

	Private m_MandantSettingsXml As SPProgUtility.CommonXmlUtility.SettingsXml

	Private m_SPPVLGAVUtilServiceUrl As String

	Private m_selectedRemoteData As PVLGAVViewData

	Private errorProviderMangement As DXErrorProvider.DXErrorProvider

#End Region


#Region "Constructor"

	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_InitializationData = _setting
		ClsDataDetail.m_InitialData = m_InitializationData

		m_mandant = New Mandant

		m_Utility = New Utility
		m_UtilityUI = New UtilityUI
		errorProviderMangement = New DXErrorProvider.DXErrorProvider
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)


		Me.KeyPreview = True
		Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		InitializeComponent()


		' Translate controls.
		TranslateControls()

		bbiPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never

		connectionString = m_InitializationData.MDData.MDDbConn
		m_CommonDatabaseAccess = New CommonDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
		m_TablesettingDatabaseAccess = New TablesDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)


		m_MandantSetting = String.Format(MANDANT_XML_MAIN_KEY, m_InitializationData.MDData.MDNr)

		m_PayrollSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_PAYROLL_SETTING, m_InitializationData.MDData.MDNr)
		m_MandantXMLFile = m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, m_InitializationData.MDData.MDYear)
		If Not System.IO.File.Exists(m_MandantXMLFile) Then
			m_UtilityUI.ShowErrorDialog(String.Format("Die Einstellung-Datei wurde nicht gefunden.{0}{1}", vbNewLine, m_MandantXMLFile))
		Else
			m_MandantSettingsXml = New SPProgUtility.CommonXmlUtility.SettingsXml(m_MandantXMLFile)
		End If

		'm_SPPVLGAVUtilServiceUrl = DEFAULT_SPUTNIK_PVLGAV_UTIL_WEBSERVICE_URI

		Dim domainName = m_InitializationData.MDData.WebserviceDomain
		m_SPPVLGAVUtilServiceUrl = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_PVLGAV_UTIL_WEBSERVICE_URI)

		Reset()
		ResetExitingKTGGridSalaryData()
		ResetRemoteLMVGridData()

		AddHandler Me.gvRemoteLMV.ColumnFilterChanged, AddressOf OnGVDetail_ColumnFilterChanged
		AddHandler gvExitingKTG.RowCellClick, AddressOf OngvExistingGrid_RowCellClick
		AddHandler gvRemoteLMV.RowCellClick, AddressOf OngvRemoteGrid_RowCellClick


	End Sub


#End Region


#Region "public property"

	''' <summary>
	''' Gets the selected employee.
	''' </summary>
	''' <returns>The selected employee or nothing if none is selected.</returns>
	Public ReadOnly Property SelectedExitingRecord As lmvKTGData
		Get
			Dim gvRP = TryCast(grdExitingKTG.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvRP Is Nothing) Then

				Dim selectedRows = gvExitingKTG.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim employee = CType(gvExitingKTG.GetRow(selectedRows(0)), lmvKTGData)
					Return employee
				End If

			End If

			Return Nothing
		End Get

	End Property

	Public ReadOnly Property SelectedRemoteRecord As PVLGAVViewData
		Get
			Dim gvRP = TryCast(grdRemoteLMV.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvRP Is Nothing) Then

				Dim selectedRows = gvRemoteLMV.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim alkEmployee = CType(gvRemoteLMV.GetRow(selectedRows(0)), PVLGAVViewData)
					Return alkEmployee
				End If

			End If

			Return Nothing
		End Get

	End Property


#End Region



#Region "public Methods"

	Public Function LoadKTGForLMVData() As Boolean

		LoadExitingKTGForLMVList()
		SearchViaWebService()

	End Function


#End Region


#Region "Private Methods"


	Private Sub Reset()

		m_CurrentRecordNumber = Nothing

		txt_KK_AN_WA_Proz.EditValue = 0D
		txt_KK_AN_WZ_Proz.EditValue = 0D
		txt_KK_AN_MA_Proz.EditValue = 0D
		txt_KK_AN_MZ_Proz.EditValue = 0D

		txt_KK_AG_WA_Proz.EditValue = 0D
		txt_KK_AG_WZ_Proz.EditValue = 0D
		txt_KK_AG_MA_Proz.EditValue = 0D
		txt_KK_AG_MZ_Proz.EditValue = 0D


		txt_KK_AN_WA_Proz_72.EditValue = 0D
		txt_KK_AN_WZ_Proz_72.EditValue = 0D
		txt_KK_AN_MA_Proz_72.EditValue = 0D
		txt_KK_AN_MZ_Proz_72.EditValue = 0D

		txt_KK_AG_WA_Proz_72.EditValue = 0D
		txt_KK_AG_WZ_Proz_72.EditValue = 0D
		txt_KK_AG_MA_Proz_72.EditValue = 0D
		txt_KK_AG_MZ_Proz_72.EditValue = 0D


		errorProviderMangement.ClearErrors()

	End Sub


	''' <summary>
	'''  Trannslate controls.
	''' </summary>
	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
		Me.btnClose.Text = m_Translate.GetSafeTranslationValue(Me.btnClose.Text)

		Me.bbiPrint.Caption = m_Translate.GetSafeTranslationValue(Me.bbiPrint.Caption)
		Me.bbiDelete.Caption = m_Translate.GetSafeTranslationValue(Me.bbiDelete.Caption)

	End Sub





	Private Sub SearchViaWebService()

		Dim uiSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext()

		Task(Of BindingList(Of PVLGAVViewData)).Factory.StartNew(Function() PerformWebserviceCallAsync(),
																						CancellationToken.None,
																						TaskCreationOptions.None,
																						TaskScheduler.Default).ContinueWith(Sub(t) FinishWebserviceCallTask(t), CancellationToken.None, TaskContinuationOptions.None, uiSynchronizationContext)

	End Sub

	Private Function PerformWebserviceCallAsync() As BindingList(Of PVLGAVViewData)

		Dim listDataSource As BindingList(Of PVLGAVViewData) = New BindingList(Of PVLGAVViewData)

		Dim webservice As New SPPVLGAVUtilWebService.SPPVLGAVUtilSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_SPPVLGAVUtilServiceUrl)

		' Read data over webservice
		Dim searchResult As GAVNameResultDTO() = webservice.GetCurrentPVLData(m_InitializationData.MDData.MDGuid, String.Empty, String.Empty, String.Empty)

		For Each result In searchResult

			Dim viewData = New PVLGAVViewData With {
				.gav_number = result.gav_number,
				.name_de = result.name_de,
				.name_it = result.name_it,
				.name_fr = result.name_fr,
				.stdweek = result.stdweek,
				.stdmonth = result.stdmonth,
				.stdyear = result.stdyear,
				.fag = result.fag,
				.fan = result.fan,
				.vag = result.vag,
				.van = result.van
			}

			listDataSource.Add(viewData)

		Next
		Me.bsiLMVReccount.Caption = String.Format("{0}", listDataSource.Count)


		Return listDataSource

	End Function

	''' <summary>
	''' Finish web service call.
	''' </summary>
	Private Sub FinishWebserviceCallTask(ByVal t As Task(Of BindingList(Of PVLGAVViewData)))

		Select Case t.Status
			Case TaskStatus.RanToCompletion
				' Webservice call was successful.
				grdRemoteLMV.DataSource = t.Result

			Case TaskStatus.Faulted
				' Something went wrong -> log error.
				m_Logger.LogError(t.Exception.ToString())
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("LMV-Daten konnten nicht geladen werden."))

			Case Else
				' Do nothing
		End Select

	End Sub




	Private Function LoadExitingKTGForLMVList()

		Dim listOfEmployees = m_TablesettingDatabaseAccess.LoadKTGForLmvData(m_InitializationData.MDData.MDNr, m_InitializationData.MDData.MDYear)
		If listOfEmployees Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Keine Daten konnten geladen werden."))

			Return False
		End If

		Dim responsiblePersonsGridData = (From person In listOfEmployees
Select New lmvKTGData With
			 {.KK_AN_WA_Proz = person.KK_AN_WA_Proz,
				.KK_AN_WZ_Proz = person.KK_AN_WZ_Proz,
				.KK_AN_MA_Proz = person.KK_AN_MA_Proz,
				.KK_AN_MZ_Proz = person.KK_AN_MZ_Proz,
				.KK_AG_WA_Proz = person.KK_AG_WA_Proz,
				.KK_AG_WZ_Proz = person.KK_AG_WZ_Proz,
				.KK_AG_MA_Proz = person.KK_AG_MA_Proz,
				.KK_AG_MZ_Proz = person.KK_AG_MZ_Proz,
				.gavnumber = person.gavnumber,
				.KK_AN_WA_Proz_72 = person.KK_AN_WA_Proz_72,
				.KK_AN_WZ_Proz_72 = person.KK_AN_WZ_Proz_72,
				.KK_AN_MA_Proz_72 = person.KK_AN_MA_Proz_72,
				.KK_AN_MZ_Proz_72 = person.KK_AN_MZ_Proz_72,
				.KK_AG_WA_Proz_72 = person.KK_AG_WA_Proz_72,
				.KK_AG_WZ_Proz_72 = person.KK_AG_WZ_Proz_72,
				.KK_AG_MA_Proz_72 = person.KK_AG_MA_Proz_72,
				.KK_AG_MZ_Proz_72 = person.KK_AG_MZ_Proz_72,
				.mdyear = person.mdyear,
				.mdnr = person.mdnr,
				.berufbez = person.berufbez,
				.createdon = person.createdon,
				.createdfrom = person.createdfrom,
				.changedon = person.changedon,
				.changedfrom = person.changedfrom,
				.recid = person.recid,
				.recnr = person.recnr
			 }).ToList()

		Dim listDataSource As BindingList(Of lmvKTGData) = New BindingList(Of lmvKTGData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdExitingKTG.DataSource = listDataSource
		Me.bsiExitingReccount.Caption = String.Format("{0}", gvExitingKTG.RowCount)


		Return Not listOfEmployees Is Nothing

	End Function


	Private Sub ResetExitingKTGGridSalaryData()

		gvExitingKTG.OptionsView.ShowIndicator = False
		gvExitingKTG.OptionsView.ShowAutoFilterRow = True
		gvExitingKTG.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvExitingKTG.OptionsView.ShowFooter = False

		gvExitingKTG.Columns.Clear()


		Dim columnRowNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnRowNr.Caption = m_Translate.GetSafeTranslationValue("GAV-Nummer")
		columnRowNr.Name = "gavnumber"
		columnRowNr.FieldName = "gavnumber"
		columnRowNr.Width = 50
		columnRowNr.Visible = True
		gvExitingKTG.Columns.Add(columnRowNr)

		Dim columnberufbez As New DevExpress.XtraGrid.Columns.GridColumn()
		columnberufbez.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnberufbez.Caption = m_Translate.GetSafeTranslationValue("GAV-Beruf")
		columnberufbez.Name = "berufbez"
		columnberufbez.FieldName = "berufbez"
		columnberufbez.Width = 100
		columnberufbez.Visible = True
		gvExitingKTG.Columns.Add(columnberufbez)

		Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMANr.Caption = m_Translate.GetSafeTranslationValue("Jahr")
		columnMANr.Name = "mdyear"
		columnMANr.FieldName = "mdyear"
		columnMANr.Width = 50
		columnMANr.Visible = True
		gvExitingKTG.Columns.Add(columnMANr)


		Dim columnKK_ANAG_WA_Proz As New DevExpress.XtraGrid.Columns.GridColumn()
		columnKK_ANAG_WA_Proz.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnKK_ANAG_WA_Proz.Caption = m_Translate.GetSafeTranslationValue("Frauen A (AN-AG) - 60 Tage")
		columnKK_ANAG_WA_Proz.Name = "KK_ANAG_WA_Proz"
		columnKK_ANAG_WA_Proz.FieldName = "KK_ANAG_WA_Proz"
		columnKK_ANAG_WA_Proz.Visible = True
		columnKK_ANAG_WA_Proz.BestFit()
		gvExitingKTG.Columns.Add(columnKK_ANAG_WA_Proz)

		Dim columnKK_ANAG_WZ_Proz As New DevExpress.XtraGrid.Columns.GridColumn()
		columnKK_ANAG_WZ_Proz.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnKK_ANAG_WZ_Proz.Caption = m_Translate.GetSafeTranslationValue("Frauen Z (AN-AG) - 60 Tage")
		columnKK_ANAG_WZ_Proz.Name = "KK_ANAG_WZ_Proz"
		columnKK_ANAG_WZ_Proz.FieldName = "KK_ANAG_WZ_Proz"
		columnKK_ANAG_WZ_Proz.Visible = False
		columnKK_ANAG_WZ_Proz.BestFit()
		gvExitingKTG.Columns.Add(columnKK_ANAG_WZ_Proz)


		Dim columnKK_ANAG_MA_Proz As New DevExpress.XtraGrid.Columns.GridColumn()
		columnKK_ANAG_MA_Proz.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnKK_ANAG_MA_Proz.Caption = m_Translate.GetSafeTranslationValue("Männer A (AN-AG) - 60 Tage")
		columnKK_ANAG_MA_Proz.Name = "KK_ANAG_MA_Proz"
		columnKK_ANAG_MA_Proz.FieldName = "KK_ANAG_MA_Proz"
		columnKK_ANAG_MA_Proz.Visible = True
		columnKK_ANAG_MA_Proz.BestFit()
		gvExitingKTG.Columns.Add(columnKK_ANAG_MA_Proz)

		Dim columnKK_ANAG_MZ_Proz As New DevExpress.XtraGrid.Columns.GridColumn()
		columnKK_ANAG_MZ_Proz.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnKK_ANAG_MZ_Proz.Caption = m_Translate.GetSafeTranslationValue("Männer Z (AN-AG) - 60 Tage")
		columnKK_ANAG_MZ_Proz.Name = "KK_ANAG_MZ_Proz"
		columnKK_ANAG_MZ_Proz.FieldName = "KK_ANAG_MZ_Proz"
		columnKK_ANAG_MZ_Proz.Visible = False
		columnKK_ANAG_MZ_Proz.BestFit()
		gvExitingKTG.Columns.Add(columnKK_ANAG_MZ_Proz)



		Dim columnKK_ANAG_WA_Proz_72 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnKK_ANAG_WA_Proz_72.Caption = m_Translate.GetSafeTranslationValue("Frauen A (AN-AG) - 720 Tag")
		columnKK_ANAG_WA_Proz_72.Name = "KK_ANAG_WA_Proz_72"
		columnKK_ANAG_WA_Proz_72.FieldName = "KK_ANAG_WA_Proz_72"
		columnKK_ANAG_WA_Proz_72.Visible = True
		gvExitingKTG.Columns.Add(columnKK_ANAG_WA_Proz_72)

		Dim columnKK_ANAG_WZ_Proz_72 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnKK_ANAG_WZ_Proz_72.Caption = m_Translate.GetSafeTranslationValue("Frauen Z (AN-AG) - 720 Tag")
		columnKK_ANAG_WZ_Proz_72.Name = "KK_ANAG_WZ_Proz_72"
		columnKK_ANAG_WZ_Proz_72.FieldName = "KK_ANAG_WZ_Proz_72"
		columnKK_ANAG_WZ_Proz_72.Visible = False
		gvExitingKTG.Columns.Add(columnKK_ANAG_WZ_Proz_72)

		Dim columnKK_ANAG_MA_Proz_72 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnKK_ANAG_MA_Proz_72.Caption = m_Translate.GetSafeTranslationValue("Männer A (AN-AG) - 720 Tag")
		columnKK_ANAG_MA_Proz_72.Name = "KK_ANAG_MA_Proz_72"
		columnKK_ANAG_MA_Proz_72.FieldName = "KK_ANAG_MA_Proz_72"
		columnKK_ANAG_MA_Proz_72.Visible = True
		gvExitingKTG.Columns.Add(columnKK_ANAG_MA_Proz_72)

		Dim columnKK_ANAG_MZ_Proz_72 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnKK_ANAG_MZ_Proz_72.Caption = m_Translate.GetSafeTranslationValue("Männer Z (AN-AG) - 720 Tag")
		columnKK_ANAG_MZ_Proz_72.Name = "KK_ANAG_MZ_Proz_72"
		columnKK_ANAG_MZ_Proz_72.FieldName = "KK_ANAG_MZ_Proz_72"
		columnKK_ANAG_MZ_Proz_72.Visible = False
		gvExitingKTG.Columns.Add(columnKK_ANAG_MZ_Proz_72)



		Dim columncreatedon As New DevExpress.XtraGrid.Columns.GridColumn()
		columncreatedon.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncreatedon.Caption = m_Translate.GetSafeTranslationValue("Erstellt am")
		columncreatedon.Name = "createdon"
		columncreatedon.FieldName = "createdon"
		columncreatedon.Visible = True
		columncreatedon.BestFit()
		gvExitingKTG.Columns.Add(columncreatedon)

		Dim columncreatedfrom As New DevExpress.XtraGrid.Columns.GridColumn()
		columncreatedfrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncreatedfrom.Caption = m_Translate.GetSafeTranslationValue("Erstellt durch")
		columncreatedfrom.Name = "createdfrom"
		columncreatedfrom.FieldName = "createdfrom"
		columncreatedfrom.Visible = True
		columncreatedfrom.BestFit()
		gvExitingKTG.Columns.Add(columncreatedfrom)

		Dim columnchangedon As New DevExpress.XtraGrid.Columns.GridColumn()
		columnchangedon.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnchangedon.Caption = m_Translate.GetSafeTranslationValue("Geändert am")
		columnchangedon.Name = "changedon"
		columnchangedon.FieldName = "changedon"
		columnchangedon.Visible = True
		columnchangedon.BestFit()
		gvExitingKTG.Columns.Add(columnchangedon)

		Dim columnchangedfrom As New DevExpress.XtraGrid.Columns.GridColumn()
		columnchangedfrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnchangedfrom.Caption = m_Translate.GetSafeTranslationValue("Geändert durch")
		columnchangedfrom.Name = "changedfrom"
		columnchangedfrom.FieldName = "changedfrom"
		columnchangedfrom.Visible = True
		columnchangedfrom.BestFit()
		gvExitingKTG.Columns.Add(columnchangedfrom)


		grdExitingKTG.DataSource = Nothing

	End Sub

	Private Sub ResetRemoteLMVGridData()

		gvRemoteLMV.OptionsView.ShowIndicator = False
		gvRemoteLMV.OptionsView.ShowAutoFilterRow = True
		gvRemoteLMV.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvRemoteLMV.OptionsView.ShowFooter = False

		gvRemoteLMV.Columns.Clear()


		Dim columngav_number As New DevExpress.XtraGrid.Columns.GridColumn()
		columngav_number.Caption = m_Translate.GetSafeTranslationValue("GAV-Nummer")
		columngav_number.Name = "gav_number"
		columngav_number.FieldName = "gav_number"
		columngav_number.Width = 50
		columngav_number.Visible = True
		gvRemoteLMV.Columns.Add(columngav_number)


		Dim columnname_de As New DevExpress.XtraGrid.Columns.GridColumn()
		columnname_de.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (DE)")
		columnname_de.Name = "name_de"
		columnname_de.FieldName = "name_de"
		columnname_de.Width = 300
		columnname_de.Visible = True
		gvRemoteLMV.Columns.Add(columnname_de)

		Dim columnname_it As New DevExpress.XtraGrid.Columns.GridColumn()
		columnname_it.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
		columnname_it.Name = "name_it"
		columnname_it.FieldName = "name_it"
		columnname_it.Width = 300
		columnname_it.Visible = m_InitializationData.UserData.UserLanguage.StartsWith("I")
		gvRemoteLMV.Columns.Add(columnname_it)

		Dim columnname_fr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnname_fr.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
		columnname_fr.Name = "name_fr"
		columnname_fr.FieldName = "name_fr"
		columnname_fr.Width = 300
		columnname_fr.Visible = m_InitializationData.UserData.UserLanguage.StartsWith("F")
		gvRemoteLMV.Columns.Add(columnname_fr)

		Dim columnstdweek As New DevExpress.XtraGrid.Columns.GridColumn()
		columnstdweek.Caption = m_Translate.GetSafeTranslationValue("Stunden Woch")
		columnstdweek.Name = "stdweek"
		columnstdweek.FieldName = "stdweek"
		columnstdweek.Width = 50
		columnstdweek.Visible = True
		gvRemoteLMV.Columns.Add(columnstdweek)

		Dim columnstdmonth As New DevExpress.XtraGrid.Columns.GridColumn()
		columnstdmonth.Caption = m_Translate.GetSafeTranslationValue("Stunden Monat")
		columnstdmonth.Name = "stdmonth"
		columnstdmonth.FieldName = "stdmonth"
		columnstdmonth.Width = 50
		columnstdmonth.Visible = True
		gvRemoteLMV.Columns.Add(columnstdmonth)

		Dim columnsstdyear As New DevExpress.XtraGrid.Columns.GridColumn()
		columnsstdyear.Caption = m_Translate.GetSafeTranslationValue("Stunden Jahr")
		columnsstdyear.Name = "stdyear"
		columnsstdyear.FieldName = "stdyear"
		columnsstdyear.Visible = True
		columnsstdyear.Width = 50
		'columns_kanton.BestFit()
		gvRemoteLMV.Columns.Add(columnsstdyear)


		Dim columnfan_fag As New DevExpress.XtraGrid.Columns.GridColumn()
		columnfan_fag.Caption = m_Translate.GetSafeTranslationValue("FAR (AN-AG)")
		columnfan_fag.Name = "fan_fag"
		columnfan_fag.FieldName = "fan_fag"
		columnfan_fag.Width = 50
		columnfan_fag.Visible = True
		gvRemoteLMV.Columns.Add(columnfan_fag)


		Dim columnvan_vag As New DevExpress.XtraGrid.Columns.GridColumn()
		columnvan_vag.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnvan_vag.Caption = m_Translate.GetSafeTranslationValue("Vollzug (AN-AG)")
		columnvan_vag.Name = "van_vag"
		columnvan_vag.FieldName = "van_vag"
		columnvan_vag.Width = 50
		columnvan_vag.Visible = True
		'columnahv_nr_new.BestFit()
		gvRemoteLMV.Columns.Add(columnvan_vag)


		grdRemoteLMV.DataSource = Nothing

	End Sub



	Private Sub OnGVDetail_ColumnFilterChanged(sender As Object, e As System.EventArgs)

		Me.bsiExitingReccount.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True
		Me.bsiExitingReccount.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), gvRemoteLMV.RowCount)

	End Sub

	Private Sub OngvRemoteLMV_CustomColumnDisplayText(ByVal sender As Object, ByVal e As CustomColumnDisplayTextEventArgs) Handles gvRemoteLMV.CustomColumnDisplayText

		If e.Column.FieldName = "kinder" Or e.Column.FieldName = "m_anz" Or e.Column.FieldName = "m_ans" Or e.Column.FieldName = "m_bas" Or e.Column.FieldName = "m_btr" Or e.Column.FieldName = "bvgstd" Or e.Column.FieldName = "ahvlohn" Then
			If e.Value = 0 Then e.DisplayText = String.Empty
		End If

	End Sub

	''' <summary>
	''' Handles focus change of row.
	''' </summary>
	Sub OngvRemoteGrid_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		m_selectedRemoteData = SelectedRemoteRecord

		If Not m_selectedRemoteData Is Nothing Then
			Dim success = LoadExitingKTGDetailData(Nothing, m_InitializationData.MDData.MDNr, m_InitializationData.MDData.MDYear, m_selectedRemoteData.gav_number)

			If Not success Then
				' no message, maybe it will be a new record
				'm_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht geladen werden."))
			End If

		End If

	End Sub


	''' <summary>
	''' Handles rowcell click.
	''' </summary>
	Sub OngvExistingGrid_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		Dim selectedData = SelectedExitingRecord

		If Not selectedData Is Nothing Then
			Dim success = LoadExitingKTGDetailData(selectedData.recid, selectedData.mdnr, selectedData.mdyear, selectedData.gavnumber)

			If Not success Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht geladen werden."))
			End If

		End If

	End Sub

	Private Sub OnGV_CustomColumnDisplayText(ByVal sender As Object, ByVal e As CustomColumnDisplayTextEventArgs) Handles gvExitingKTG.CustomColumnDisplayText

		If e.Column.FieldName = "kinder" Or e.Column.FieldName = "m_anz" Or e.Column.FieldName = "m_ans" Or e.Column.FieldName = "m_bas" Or e.Column.FieldName = "m_btr" Or e.Column.FieldName = "bvgstd" Or e.Column.FieldName = "ahvlohn" Then
			If e.Value = 0 Then e.DisplayText = String.Empty
		End If

	End Sub


	''' <summary>
	''' Loads founded KTG detail data.
	''' </summary>
	Private Function LoadExitingKTGDetailData(ByVal recid As Integer?, ByVal mdNr As Integer, ByVal mdYear As Integer, ByVal gavNumber As Integer) As Boolean

		Dim recordSearchResult = m_TablesettingDatabaseAccess.LoadAssignedKTGForLmvData(recid, mdNr, mdYear, gavNumber)

		If Not recordSearchResult Is Nothing Then
			txt_GAVNumber.EditValue = recordSearchResult.gavnumber
			txt_Berufbez.EditValue = recordSearchResult.berufbez

			txt_KK_AN_WA_Proz.EditValue = recordSearchResult.KK_AN_WA_Proz
			txt_KK_AN_WZ_Proz.EditValue = recordSearchResult.KK_AN_WZ_Proz
			txt_KK_AN_MA_Proz.EditValue = recordSearchResult.KK_AN_MA_Proz
			txt_KK_AN_MZ_Proz.EditValue = recordSearchResult.KK_AN_MZ_Proz

			txt_KK_AG_WA_Proz.EditValue = recordSearchResult.KK_AG_WA_Proz
			txt_KK_AG_WZ_Proz.EditValue = recordSearchResult.KK_AG_WZ_Proz
			txt_KK_AG_MA_Proz.EditValue = recordSearchResult.KK_AG_MA_Proz
			txt_KK_AG_MZ_Proz.EditValue = recordSearchResult.KK_AG_MZ_Proz


			txt_KK_AN_WA_Proz_72.EditValue = recordSearchResult.KK_AN_WA_Proz_72
			txt_KK_AN_WZ_Proz_72.EditValue = recordSearchResult.KK_AN_WZ_Proz_72
			txt_KK_AN_MA_Proz_72.EditValue = recordSearchResult.KK_AN_MA_Proz_72
			txt_KK_AN_MZ_Proz_72.EditValue = recordSearchResult.KK_AN_MZ_Proz_72

			txt_KK_AG_WA_Proz_72.EditValue = recordSearchResult.KK_AG_WA_Proz_72
			txt_KK_AG_WZ_Proz_72.EditValue = recordSearchResult.KK_AG_WZ_Proz_72
			txt_KK_AG_MA_Proz_72.EditValue = recordSearchResult.KK_AG_MA_Proz_72
			txt_KK_AG_MZ_Proz_72.EditValue = recordSearchResult.KK_AG_MZ_Proz_72


			m_CurrentRecordNumber = recordSearchResult.recid

			Return True
		Else
			Reset()
			txt_GAVNumber.EditValue = m_selectedRemoteData.gav_number
			txt_Berufbez.EditValue = If(String.IsNullOrWhiteSpace(m_selectedRemoteData.name_de),
																	 If(String.IsNullOrWhiteSpace(m_selectedRemoteData.name_it), m_selectedRemoteData.name_fr, m_selectedRemoteData.name_it),
																	 m_selectedRemoteData.name_de)

			Return False
		End If

	End Function


	Private Function SaveKTGForLMVData() As Boolean
		Dim success As Boolean = True

		success = success AndAlso ValidateInputData()
		If Not success Then Return success
		Try

			Dim recordData As lmvKTGData = Nothing
			recordData = New lmvKTGData

			recordData.recid = m_CurrentRecordNumber.GetValueOrDefault(0)
			'If m_CurrentRecordNumber.GetValueOrDefault(0) = 0 Then
			'	If Not m_selectedRemoteData Is Nothing Then
			'		recordData.gavnumber = m_selectedRemoteData.gav_number
			'		recordData.berufbez = If(String.IsNullOrWhiteSpace(m_selectedRemoteData.name_de),
			'														 If(String.IsNullOrWhiteSpace(m_selectedRemoteData.name_it), m_selectedRemoteData.name_fr, m_selectedRemoteData.name_it),
			'														 m_selectedRemoteData.name_de)
			'	End If

			'End If
			recordData.gavnumber = txt_GAVNumber.EditValue
			recordData.berufbez = txt_Berufbez.EditValue

			recordData.KK_AN_WA_Proz = txt_KK_AN_WA_Proz.EditValue
			recordData.KK_AN_WZ_Proz = txt_KK_AN_WZ_Proz.EditValue
			recordData.KK_AN_MA_Proz = txt_KK_AN_MA_Proz.EditValue
			recordData.KK_AN_MZ_Proz = txt_KK_AN_MZ_Proz.EditValue

			recordData.KK_AG_WA_Proz = txt_KK_AG_WA_Proz.EditValue
			recordData.KK_AG_WZ_Proz = txt_KK_AG_WZ_Proz.EditValue
			recordData.KK_AG_MA_Proz = txt_KK_AG_MA_Proz.EditValue
			recordData.KK_AG_MZ_Proz = txt_KK_AG_MZ_Proz.EditValue

			recordData.KK_AN_WA_Proz_72 = txt_KK_AN_WA_Proz_72.EditValue
			recordData.KK_AN_WZ_Proz_72 = txt_KK_AN_WZ_Proz_72.EditValue
			recordData.KK_AN_MA_Proz_72 = txt_KK_AN_MA_Proz_72.EditValue
			recordData.KK_AN_MZ_Proz_72 = txt_KK_AN_MZ_Proz_72.EditValue


			recordData.KK_AG_WA_Proz_72 = txt_KK_AG_WA_Proz_72.EditValue
			recordData.KK_AG_WZ_Proz_72 = txt_KK_AG_WZ_Proz_72.EditValue
			recordData.KK_AG_MA_Proz_72 = txt_KK_AG_MA_Proz_72.EditValue
			recordData.KK_AG_MZ_Proz_72 = txt_KK_AG_MZ_Proz_72.EditValue



			recordData.createdfrom = m_InitializationData.UserData.UserFullName
			recordData.changedfrom = m_InitializationData.UserData.UserFullName

			recordData.mdnr = m_InitializationData.MDData.MDNr
			recordData.mdyear = m_InitializationData.MDData.MDYear


			If m_CurrentRecordNumber.GetValueOrDefault(0) = 0 Then
				success = m_TablesettingDatabaseAccess.AddKTGForLmvData(m_InitializationData.MDData.MDNr, m_InitializationData.MDData.MDYear, recordData)

			Else
				success = m_TablesettingDatabaseAccess.UpdateAssignedKTGForLmvData(m_InitializationData.MDData.MDNr, m_InitializationData.MDData.MDYear, recordData)

			End If
			If success Then
				m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Die Daten wurden gespeichert."))

			Else
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))

			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

			success = False
		End Try


		Return success

	End Function

	Private Function DeleteKTGForLMVData() As Boolean
		Dim success As Boolean = False

		Dim selectedData = SelectedExitingRecord

		If Not selectedData Is Nothing Then

			If (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Möchten Sie die ausgewählen Daten wirklich löschen?"),
																m_Translate.GetSafeTranslationValue("Daten endgültig löschen?"))) Then

				success = m_TablesettingDatabaseAccess.DeleteKTGForLmvData(selectedData.recid)

			Else
				Return False

			End If

		End If

		If Not success Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Daten konnte nicht gelöscht werden."))
		End If


		Return success

	End Function

	''' <summary>
	''' Validates document input data.
	''' </summary>
	Private Function ValidateInputData() As Boolean

		errorProviderMangement.ClearErrors()

		Dim errorText As String = "Bitte geben Sie einen Wert ein."

		Dim isValid As Boolean = True


		isValid = isValid And SetErrorIfInvalid(txt_GAVNumber, errorProviderMangement, txt_GAVNumber.EditValue <= 0, errorText)
		isValid = isValid And SetErrorIfInvalid(txt_Berufbez, errorProviderMangement, String.IsNullOrWhiteSpace(txt_Berufbez.EditValue), errorText)

		'isValid = isValid And SetErrorIfInvalid(txt_KK_AN_WA_Proz, errorProviderMangement, txt_KK_AN_WA_Proz.EditValue <= 0, errorText)
		'isValid = isValid And SetErrorIfInvalid(txt_KK_AN_WZ_Proz, errorProviderMangement, txt_KK_AN_WZ_Proz.EditValue <= 0, errorText)
		'isValid = isValid And SetErrorIfInvalid(txt_KK_AN_MA_Proz, errorProviderMangement, txt_KK_AN_MA_Proz.EditValue <= 0, errorText)
		'isValid = isValid And SetErrorIfInvalid(txt_KK_AN_MZ_Proz, errorProviderMangement, txt_KK_AN_MZ_Proz.EditValue <= 0, errorText)
		'isValid = isValid And SetErrorIfInvalid(txt_KK_AG_WA_Proz, errorProviderMangement, txt_KK_AG_WA_Proz.EditValue <= 0, errorText)
		'isValid = isValid And SetErrorIfInvalid(txt_KK_AG_WZ_Proz, errorProviderMangement, txt_KK_AG_WZ_Proz.EditValue <= 0, errorText)
		'isValid = isValid And SetErrorIfInvalid(txt_KK_AG_MA_Proz, errorProviderMangement, txt_KK_AG_MA_Proz.EditValue <= 0, errorText)
		'isValid = isValid And SetErrorIfInvalid(txt_KK_AG_MZ_Proz, errorProviderMangement, txt_KK_AG_MZ_Proz.EditValue <= 0, errorText)

		isValid = isValid And SetErrorIfInvalid(txt_KK_AN_WA_Proz_72, errorProviderMangement, txt_KK_AN_WA_Proz.EditValue + txt_KK_AN_WA_Proz_72.EditValue <= 0, errorText)
		isValid = isValid And SetErrorIfInvalid(txt_KK_AN_WZ_Proz_72, errorProviderMangement, txt_KK_AN_WZ_Proz.EditValue + txt_KK_AN_WZ_Proz_72.EditValue <= 0, errorText)
		isValid = isValid And SetErrorIfInvalid(txt_KK_AN_MA_Proz_72, errorProviderMangement, txt_KK_AN_MA_Proz.EditValue + txt_KK_AN_MA_Proz_72.EditValue <= 0, errorText)
		isValid = isValid And SetErrorIfInvalid(txt_KK_AN_MZ_Proz_72, errorProviderMangement, txt_KK_AN_MZ_Proz.EditValue + txt_KK_AN_MZ_Proz_72.EditValue <= 0, errorText)
		isValid = isValid And SetErrorIfInvalid(txt_KK_AG_WA_Proz_72, errorProviderMangement, txt_KK_AG_WA_Proz.EditValue + txt_KK_AG_WA_Proz_72.EditValue <= 0, errorText)
		isValid = isValid And SetErrorIfInvalid(txt_KK_AG_WZ_Proz_72, errorProviderMangement, txt_KK_AG_WZ_Proz.EditValue + txt_KK_AG_WZ_Proz_72.EditValue <= 0, errorText)
		isValid = isValid And SetErrorIfInvalid(txt_KK_AG_MA_Proz_72, errorProviderMangement, txt_KK_AG_MA_Proz.EditValue + txt_KK_AG_MA_Proz_72.EditValue <= 0, errorText)
		isValid = isValid And SetErrorIfInvalid(txt_KK_AG_MZ_Proz_72, errorProviderMangement, txt_KK_AG_MZ_Proz.EditValue + txt_KK_AG_MZ_Proz_72.EditValue <= 0, errorText)


		Return isValid

	End Function

	''' <summary>
	''' Validates a control.
	''' </summary>
	''' <param name="control">The control to validate.</param>
	''' <param name="errorProvider">The error providor.</param>
	''' <param name="invalid">Boolean flag if data is invalid.</param>
	''' <param name="errorText">The error text.</param>
	''' <returns>Valid flag</returns>
	Private Function SetErrorIfInvalid(ByVal control As Control, ByVal errorProvider As DXErrorProvider.DXErrorProvider, ByVal invalid As Boolean, ByVal errorText As String) As Boolean

		If (invalid) Then
			errorProvider.SetError(control, errorText)
		Else
			errorProvider.SetError(control, String.Empty)
		End If

		Return Not invalid

	End Function

#End Region


#Region "Form Properties..."

	Private Sub frmOnLoad(sender As Object, e As System.EventArgs) Handles Me.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			If My.Settings.ifrmHeightKTGForLmv > 0 Then Me.Height = Math.Max(Me.Height, My.Settings.ifrmHeightKTGForLmv)
			If My.Settings.ifrmWidthKTGForLmv > 0 Then Me.Width = Math.Max(Me.Width, My.Settings.ifrmWidthKTGForLmv)

			If My.Settings.frmLocationKTGForLmv <> String.Empty Then
				Dim aLoc As String() = My.Settings.frmLocationKTGForLmv.Split(CChar(";"))

				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = CStr(0)
				End If
				Me.Location = New System.Drawing.Point(CInt(Math.Max(Val(aLoc(0)), 0)), CInt(Math.Max(Val(aLoc(1)), 0)))
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.Message))

		End Try

	End Sub

	Private Sub frmSearchKD_LV_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.frmLocationKTGForLmv = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.ifrmHeightKTGForLmv = Me.Height
				My.Settings.ifrmWidthKTGForLmv = Me.Width

				My.Settings.Save()
			End If


		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub

	Private Sub OnxtabMain_SelectedPageChanged(sender As Object, e As DevExpress.XtraTab.TabPageChangedEventArgs) Handles xtabMain.SelectedPageChanged
		bbiDelete.Enabled = xtabMain.SelectedTabPage Is xtabExitingKTG
	End Sub

	Private Sub OnbbiSave_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSave.ItemClick
		Dim success = SaveKTGForLMVData()

		success = success AndAlso LoadExitingKTGForLMVList()

		If success Then OngvExistingGrid_RowCellClick(gvExitingKTG, Nothing)

	End Sub

	Private Sub OnbbiNew_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs)

		Reset()

	End Sub

	Private Sub OnbbiDelete_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiDelete.ItemClick

		If Not xtabMain.SelectedTabPage Is xtabExitingKTG Then Return

		Dim success = DeleteKTGForLMVData()
		Reset()
		success = success AndAlso LoadExitingKTGForLMVList()

	End Sub

	Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
		Me.Close()
	End Sub


#End Region



	Class PVLGAVViewData

		Public Property gav_number As Integer
		Public Property name_de As String
		Public Property name_fr As String
		Public Property name_it As String
		Public Property publication_date As DateTime?
		Public Property schema_version As String

		Public Property stdweek As Decimal?
		Public Property stdmonth As Decimal?
		Public Property stdyear As Decimal?
		Public Property fan As Decimal?
		Public Property fag As Decimal?
		Public Property van As Decimal?
		Public Property vag As Decimal?
		Public Property currdbname As String


		Public ReadOnly Property fan_fag As String
			Get
				Return String.Format("{0:n2} - {1:n2}", fan, fag)
			End Get
		End Property

		Public ReadOnly Property van_vag As String
			Get
				Return String.Format("{0:n2} - {1:n2}", van, vag)
			End Get
		End Property


	End Class






End Class