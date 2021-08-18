
Imports SPS.Listing.Print.Utility.PrintLOListing
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
Imports System.IO
Imports System.Xml.Serialization
Imports DevExpress.XtraBars
Imports SPS.Listing.Print.Utility

Public Class frmLAStamm


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

	Private connectionString As String

	''' <summary>
	''' Record number of selected row.
	''' </summary>
	Private m_CurrentRecordNumber As Integer?
	Private m_selectedLohnartData As LAStammData

	Private m_MandantXMLFile As String
	Private m_MandantFormXMLFileName As String
	Private m_MandantSetting As String
	Private m_PayrollSetting As String
	Private m_SelectedLAData As List(Of LAStammData)

	Private m_MandantSettingsXml As SPProgUtility.CommonXmlUtility.SettingsXml

	Private errorProviderMangement As DXErrorProvider.DXErrorProvider

#End Region


#Region "private consts"

	Private Const MANDANT_XML_MAIN_KEY As String = "MD_{0}/"
	Private Const MANDANT_XML_SETTING_SPUTNIK_PAYROLL_SETTING As String = "MD_{0}/Lohnbuchhaltung"
	Public Const DEFAULT_SPUTNIK_PVLGAV_UTIL_WEBSERVICE_URI As String = "http://asmx.domain.com/wsSPS_services/SPPVLGAVUtil.asmx"

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

#If Not DEBUG Then
		'acMain.Visible = False
#End If
		' Translate controls.
		TranslateControls()

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

		Reset()

		LoadYearDropDownData()
		LoadFBKData()
		LoadSignDropDownData()
		LoadErfassInDropDownData()
		LoadLAWDropDownData()
		LoadLSEFieldDropDownData()
		LoadGroupKeyDropDownData()
		LoadRoundingDropDownData()

		AddHandler lue_Soll.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lue_Haben.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler cbo_1Lohnbeleg.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler cbo_2Lohnbeleg.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lue_LAWField.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lue_LSEField.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueGroupKey.ButtonClick, AddressOf OnDropDown_ButtonClick

		AddHandler Me.lueYear.EditValueChanged, AddressOf OnlueYear_EditValueChanged
		AddHandler gvLohnart.RowCellClick, AddressOf Ongv_RowCellClick
		AddHandler gvLohnart.FocusedRowChanged, AddressOf Ongv_FocusedRowChanged


	End Sub


#End Region


#Region "public property"

	''' <summary>
	''' Gets the selected employee.
	''' </summary>
	''' <returns>The selected employee or nothing if none is selected.</returns>
	Public ReadOnly Property SelectedExitingRecord As LAStammData
		Get
			Dim gvRP = TryCast(grdLohnart.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvRP Is Nothing) Then

				Dim selectedRows = gvLohnart.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim employee = CType(gvLohnart.GetRow(selectedRows(0)), LAStammData)
					Return employee
				End If

			End If

			Return Nothing
		End Get

	End Property


#End Region


#Region "private properties"

	Private ReadOnly Property GetHwnd() As String
		Get
			Return Me.Handle
		End Get
	End Property


#End Region


#Region "public Methods"

	Public Function LoadLAStammData() As Boolean

		Dim success = LoadLohnartList()

		Return success
	End Function


#End Region


#Region "Private Methods"



	Private Sub Reset()

		m_CurrentRecordNumber = Nothing
		acMain.OptionsMinimizing.State = Navigation.AccordionControlState.Minimized

		txt_Jahr.EditValue = 0
		txt_LANr.EditValue = 0
		cbo_Zeichen.EditValue = String.Empty
		txt_TextRE.EditValue = String.Empty
		txt_TextLO.EditValue = String.Empty

		chk_Deaktivate.Checked = False
		txt_Filter.EditValue = String.Empty
		txt_FunkVorCreate.EditValue = String.Empty
		cbo_Rundung.EditValue = 0
		cbo_Erfassen.EditValue = String.Empty

		cbo_Anzahl.EditValue = String.Empty
		chk_Anzahl_Allowedmore.Checked = False
		txt_Anzahl_VarMA.EditValue = String.Empty
		txt_Anzahl_VarKD.EditValue = String.Empty
		txt_Anzahl_Fix.EditValue = 0D
		cbo_AnzahlSum0.EditValue = String.Empty
		cbo_AnzahlSum1.EditValue = String.Empty
		chk_Anzahl_Print.Checked = False

		cbo_Basis.EditValue = String.Empty
		chk_Basis_Allowedmore.Checked = False
		txt_Basis_VarMA.EditValue = String.Empty
		txt_Basis_VarKD.EditValue = String.Empty
		txt_Basis_Fix.EditValue = 0D
		cbo_BasisSum0.EditValue = String.Empty
		cbo_BasisSum1.EditValue = String.Empty
		cbo_BasisSum2.EditValue = String.Empty
		chk_Basis_Print.Checked = False

		cbo_Ansatz.EditValue = String.Empty
		chk_Ansatz_Allowedmore.Checked = False
		txt_Ansatz_VarMA.EditValue = String.Empty
		txt_Ansatz_VarKD.EditValue = String.Empty
		txt_Ansatz_Fix.EditValue = 0D
		cbo_AnsatzSum0.EditValue = String.Empty
		chk_Ansatz_Print.Checked = False

		chk_Betrag_Allowedmore.Checked = False
		cbo_BetragSum0.EditValue = String.Empty
		cbo_BetragSum1.EditValue = String.Empty
		cbo_BetragSum2.EditValue = String.Empty
		cbo_BetragSum3.EditValue = String.Empty
		chk_Betrag_Print.Checked = False
		chk_BetragNoPrintZero.Checked = False

		chk_proTag.Checked = False
		chk_NoZV.Checked = False
		chk_Gleitzeit.Checked = False
		chk_AG.Checked = False
		chk_kantonUnterschiedlich.Checked = False
		chkARGB18_1.Checked = False
		chkARGB18_2.Checked = False

		chk_Brutto.Checked = False
		chk_AHV.Checked = False
		chk_ALV.Checked = False
		chk_NBUV.Checked = False
		chk_UV.Checked = False
		chk_BruttoDB1.Checked = False
		chk_AHVDB1.Checked = False
		chk_BVG.Checked = False
		chk_KTG.Checked = False
		chk_QST.Checked = False
		chk_Res1.Checked = False
		chk_Res2.Checked = False
		chk_Res3.Checked = False
		chk_Res4.Checked = False
		chk_Res5.Checked = False
		chk_Ferien.Checked = False
		chk_Feier.Checked = False
		chk_13Lohn.Checked = False
		chk_Ferienmore.Checked = False
		chk_Feiermore.Checked = False
		chk_13Lohnmore.Checked = False

		chk_MwSt.Checked = False
		chk_ESPflicht.Checked = False
		chk_TSpesen.Checked = False
		chk_StdSpesen.Checked = False

		chk_Betrag0Create.Checked = False
		chk_Betrag0Warn.Checked = False
		chk_Dupplikat.Checked = False
		chk_NoListing.Checked = False
		chk_ZGWarn.Checked = False
		chk_KumYear.Checked = False
		chk_KumMonth.Checked = False
		txt_LANrMonth.EditValue = 0

		chk_LAWBrutto.Checked = False
		cbo_LAWZeichen.EditValue = String.Empty
		lue_LAWField.EditValue = String.Empty

		lue_Soll.EditValue = Nothing
		lue_Haben.EditValue = Nothing

		cbo_IfPositiv.EditValue = String.Empty
		cbo_Ifnegativ.EditValue = String.Empty
		cbo_1Lohnbeleg.EditValue = String.Empty
		cbo_2Lohnbeleg.EditValue = String.Empty

		lue_LSEField.EditValue = String.Empty
		lueGroupKey.EditValue = Nothing

		bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Bereit")
		bsilblCreated.Caption = m_Translate.GetSafeTranslationValue(bsilblCreated.Caption)
		bsilblCreated.Caption = m_Translate.GetSafeTranslationValue(bsilblChanged.Caption)

		ResetYearDataDropDown()
		ResetSOLLDropDown()
		ResetHABENDropDown()
		ResetVerwendungDropDown()
		ResetSignDropDown()
		ResetLAWDropDown()
		ResetLSEDropDown()
		ResetGroupKeyDropDown()
		ResetRoundingDataDropDown()
		ResetLohnartGrid()

		errorProviderMangement.ClearErrors()

	End Sub

#End Region


#Region "reset"

	Private Sub ResetYearDataDropDown()
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

	Private Sub ResetLohnartGrid()

		gvLohnart.OptionsView.ShowIndicator = False
		gvLohnart.OptionsBehavior.Editable = True
		gvLohnart.OptionsView.ShowAutoFilterRow = True
		gvLohnart.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvLohnart.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
		gvLohnart.OptionsView.ShowFooter = False

		gvLohnart.Columns.Clear()


		Dim columnSelectedRec As New DevExpress.XtraGrid.Columns.GridColumn()
		columnSelectedRec.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnSelectedRec.OptionsColumn.AllowEdit = True
		columnSelectedRec.Caption = m_Translate.GetSafeTranslationValue("Auswahl")
		columnSelectedRec.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
		columnSelectedRec.Name = "Selected"
		columnSelectedRec.FieldName = "Selected"
		columnSelectedRec.Visible = True
		columnSelectedRec.Width = 10
		gvLohnart.Columns.Add(columnSelectedRec)

		Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
		columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
		columnrecid.OptionsColumn.AllowEdit = False
		columnrecid.Name = "recid"
		columnrecid.FieldName = "recid"
		columnrecid.Visible = False
		gvLohnart.Columns.Add(columnrecid)

		Dim columnLANr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLANr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnLANr.Caption = m_Translate.GetSafeTranslationValue("Nummer")
		columnLANr.OptionsColumn.AllowEdit = False
		columnLANr.Name = "LANr"
		columnLANr.FieldName = "LANr"
		columnLANr.Width = 30
		columnLANr.Visible = True
		gvLohnart.Columns.Add(columnLANr)


		Dim columnLALoText As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLALoText.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnLALoText.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
		columnLALoText.OptionsColumn.AllowEdit = False
		columnLALoText.Name = "LALoText"
		columnLALoText.FieldName = "LALoText"
		columnLALoText.Visible = True
		columnLALoText.BestFit()
		gvLohnart.Columns.Add(columnLALoText)


		grdLohnart.DataSource = Nothing

	End Sub

	''' <summary>
	''' Resets the SOLL drop down.
	''' </summary>
	Private Sub ResetSOLLDropDown()

		lue_Soll.Properties.DisplayMember = "KontoNr"
		lue_Soll.Properties.ValueMember = "KontoNr"

		gvSoll.OptionsView.ShowIndicator = False
		gvSoll.OptionsView.ShowColumnHeaders = True
		gvSoll.OptionsView.ShowFooter = False

		gvSoll.OptionsView.ShowAutoFilterRow = True
		gvSoll.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvSoll.Columns.Clear()

		Dim columnCustomerNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerNumber.Caption = m_Translate.GetSafeTranslationValue("Nummer")
		columnCustomerNumber.Name = "KontoNr"
		columnCustomerNumber.FieldName = "KontoNr"
		columnCustomerNumber.Visible = True
		columnCustomerNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvSoll.Columns.Add(columnCustomerNumber)

		Dim columnCompany1 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCompany1.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
		columnCompany1.Name = "KontoName"
		columnCompany1.FieldName = "KontoName"
		columnCompany1.Visible = True
		columnCompany1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvSoll.Columns.Add(columnCompany1)


		lue_Soll.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lue_Soll.Properties.NullText = String.Empty
		lue_Soll.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets the HABEN drop down.
	''' </summary>
	Private Sub ResetHABENDropDown()

		lue_Haben.Properties.DisplayMember = "KontoNr"
		lue_Haben.Properties.ValueMember = "KontoNr"

		gvHaben.OptionsView.ShowIndicator = False
		gvHaben.OptionsView.ShowColumnHeaders = True
		gvHaben.OptionsView.ShowFooter = False

		gvHaben.OptionsView.ShowAutoFilterRow = True
		gvHaben.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvHaben.Columns.Clear()

		Dim columnCustomerNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerNumber.Caption = m_Translate.GetSafeTranslationValue("Nummer")
		columnCustomerNumber.Name = "KontoNr"
		columnCustomerNumber.FieldName = "KontoNr"
		columnCustomerNumber.Visible = True
		columnCustomerNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvHaben.Columns.Add(columnCustomerNumber)

		Dim columnCompany1 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCompany1.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
		columnCompany1.Name = "KontoName"
		columnCompany1.FieldName = "KontoName"
		columnCompany1.Visible = True
		columnCompany1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvHaben.Columns.Add(columnCompany1)


		lue_Haben.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lue_Haben.Properties.NullText = String.Empty
		lue_Haben.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets the Verwenden drop down data.
	''' </summary>
	Private Sub ResetVerwendungDropDown()

		cbo_Erfassen.Properties.DisplayMember = "Value"
		cbo_Erfassen.Properties.ValueMember = "Value"

		Dim columns = cbo_Erfassen.Properties.Columns
		columns.Clear()

		columns.Add(New LookUpColumnInfo("DisplayText", 0))

		cbo_Erfassen.Properties.ShowHeader = False
		cbo_Erfassen.Properties.ShowFooter = False
		cbo_Erfassen.Properties.DropDownRows = 10
		cbo_Erfassen.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		cbo_Erfassen.Properties.SearchMode = SearchMode.AutoComplete
		cbo_Erfassen.Properties.AutoSearchColumnIndex = 0
		cbo_Erfassen.Properties.NullText = String.Empty

		cbo_Erfassen.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets the sign drop down data.
	''' </summary>
	Private Sub ResetSignDropDown()

		cbo_Zeichen.Properties.DisplayMember = "Value"
		cbo_Zeichen.Properties.ValueMember = "Value"

		cbo_LAWZeichen.Properties.DisplayMember = "Value"
		cbo_LAWZeichen.Properties.ValueMember = "Value"

		cbo_IfPositiv.Properties.DisplayMember = "Value"
		cbo_IfPositiv.Properties.ValueMember = "Value"

		cbo_Ifnegativ.Properties.DisplayMember = "Value"
		cbo_Ifnegativ.Properties.ValueMember = "Value"

		Dim columns = cbo_Zeichen.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("DisplayText", 0))

		columns = cbo_LAWZeichen.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("DisplayText", 0))

		columns = cbo_IfPositiv.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("DisplayText", 0))

		columns = cbo_Ifnegativ.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("DisplayText", 0))


		cbo_Zeichen.Properties.ShowHeader = False
		cbo_Zeichen.Properties.ShowFooter = False
		cbo_Zeichen.Properties.DropDownRows = 10
		cbo_Zeichen.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		cbo_Zeichen.Properties.SearchMode = SearchMode.AutoComplete
		cbo_Zeichen.Properties.AutoSearchColumnIndex = 0
		cbo_Zeichen.Properties.NullText = String.Empty

		cbo_LAWZeichen.Properties.ShowHeader = False
		cbo_LAWZeichen.Properties.ShowFooter = False
		cbo_LAWZeichen.Properties.DropDownRows = 10
		cbo_LAWZeichen.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		cbo_LAWZeichen.Properties.SearchMode = SearchMode.AutoComplete
		cbo_LAWZeichen.Properties.AutoSearchColumnIndex = 0
		cbo_LAWZeichen.Properties.NullText = String.Empty

		cbo_IfPositiv.Properties.ShowHeader = False
		cbo_IfPositiv.Properties.ShowFooter = False
		cbo_IfPositiv.Properties.DropDownRows = 10
		cbo_IfPositiv.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		cbo_IfPositiv.Properties.SearchMode = SearchMode.AutoComplete
		cbo_IfPositiv.Properties.AutoSearchColumnIndex = 0
		cbo_IfPositiv.Properties.NullText = String.Empty

		cbo_Ifnegativ.Properties.ShowHeader = False
		cbo_Ifnegativ.Properties.ShowFooter = False
		cbo_Ifnegativ.Properties.DropDownRows = 10
		cbo_Ifnegativ.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		cbo_Ifnegativ.Properties.SearchMode = SearchMode.AutoComplete
		cbo_Ifnegativ.Properties.AutoSearchColumnIndex = 0
		cbo_Ifnegativ.Properties.NullText = String.Empty

		cbo_Zeichen.EditValue = Nothing
		cbo_LAWZeichen.EditValue = Nothing
		cbo_IfPositiv.EditValue = Nothing
		cbo_Ifnegativ.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets the LAW drop down data.
	''' </summary>
	Private Sub ResetLAWDropDown()

		lue_LAWField.Properties.DisplayMember = "Value"
		lue_LAWField.Properties.ValueMember = "Value"

		Dim columns = lue_LAWField.Properties.Columns
		columns.Clear()

		columns.Add(New LookUpColumnInfo("DisplayText", 0))

		lue_LAWField.Properties.ShowHeader = False
		lue_LAWField.Properties.ShowFooter = False
		lue_LAWField.Properties.DropDownRows = 10
		lue_LAWField.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lue_LAWField.Properties.SearchMode = SearchMode.AutoComplete
		lue_LAWField.Properties.AutoSearchColumnIndex = 0
		lue_LAWField.Properties.NullText = String.Empty

		lue_LAWField.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets the LSE drop down data.
	''' </summary>
	Private Sub ResetLSEDropDown()

		lue_LSEField.Properties.DisplayMember = "Value"
		lue_LSEField.Properties.ValueMember = "Value"

		Dim columns = lue_LSEField.Properties.Columns
		columns.Clear()

		columns.Add(New LookUpColumnInfo("DisplayText", 0))

		lue_LSEField.Properties.ShowHeader = False
		lue_LSEField.Properties.ShowFooter = False
		lue_LSEField.Properties.DropDownRows = 10
		lue_LSEField.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lue_LSEField.Properties.SearchMode = SearchMode.AutoComplete
		lue_LSEField.Properties.AutoSearchColumnIndex = 0
		lue_LSEField.Properties.NullText = String.Empty

		lue_LSEField.EditValue = Nothing

	End Sub

	Private Sub ResetGroupKeyDropDown()

		lueGroupKey.Properties.DisplayMember = "ValueLabel"
		lueGroupKey.Properties.ValueMember = "Value"

		Dim columns = lueGroupKey.Properties.Columns
		columns.Clear()

		columns.Add(New LookUpColumnInfo("ValueLabel", 0))

		lueGroupKey.Properties.ShowHeader = False
		lueGroupKey.Properties.ShowFooter = False
		lueGroupKey.Properties.DropDownRows = 10
		lueGroupKey.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueGroupKey.Properties.SearchMode = SearchMode.AutoComplete
		lueGroupKey.Properties.AutoSearchColumnIndex = 0
		lueGroupKey.Properties.NullText = String.Empty

		lueGroupKey.EditValue = Nothing

	End Sub

	Private Sub ResetRoundingDataDropDown()

		cbo_Rundung.Properties.DisplayMember = "Value"
		cbo_Rundung.Properties.ValueMember = "Value"

		Dim columns = cbo_Rundung.Properties.Columns
		columns.Clear()

		columns.Add(New LookUpColumnInfo("Value", 0))

		cbo_Rundung.Properties.ShowHeader = False
		cbo_Rundung.Properties.ShowFooter = False
		cbo_Rundung.Properties.DropDownRows = 10
		cbo_Rundung.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		cbo_Rundung.Properties.SearchMode = SearchMode.AutoComplete
		cbo_Rundung.Properties.AutoSearchColumnIndex = 0
		cbo_Rundung.Properties.NullText = String.Empty

		cbo_Rundung.EditValue = Nothing

	End Sub


#End Region


	''' <summary>
	'''  Trannslate controls.
	''' </summary>
	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
		lblHeader.Text = m_Translate.GetSafeTranslationValue(lblHeader.Text)
		lblHeaderDescription.Text = m_Translate.GetSafeTranslationValue(lblHeaderDescription.Text)
		Me.btnClose.Text = m_Translate.GetSafeTranslationValue(Me.btnClose.Text)

		tgsSelection.Properties.OffText = m_Translate.GetSafeTranslationValue(tgsSelection.Properties.OffText)
		tgsSelection.Properties.OnText = m_Translate.GetSafeTranslationValue(tgsSelection.Properties.OnText)

		Me.chk_Res1.Text = If(m_Translate.GetSafeTranslationValue("LAReserve1", True) = "LAReserve1", "", m_Translate.GetSafeTranslationValue("LAReserve1", True))
		Me.chk_Res2.Text = If(m_Translate.GetSafeTranslationValue("LAReserve2", True) = "LAReserve2", "", m_Translate.GetSafeTranslationValue("LAReserve2", True))
		Me.chk_Res3.Text = If(m_Translate.GetSafeTranslationValue("LAReserve3", True) = "LAReserve3", "", m_Translate.GetSafeTranslationValue("LAReserve3", True))
		Me.chk_Res4.Text = If(m_Translate.GetSafeTranslationValue("LAReserve4", True) = "LAReserve4", "", m_Translate.GetSafeTranslationValue("LAReserve4", True))
		Me.chk_Res5.Text = If(m_Translate.GetSafeTranslationValue("LAReserve5", True) = "LAReserve5", "", m_Translate.GetSafeTranslationValue("LAReserve5", True))

		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)
		Me.bbiSave.Caption = m_Translate.GetSafeTranslationValue(Me.bbiSave.Caption)
		Me.bbiDelete.Caption = m_Translate.GetSafeTranslationValue(Me.bbiDelete.Caption)
		Me.bbiCopy.Caption = m_Translate.GetSafeTranslationValue(Me.bbiCopy.Caption)
		Me.bbiExport.Caption = m_Translate.GetSafeTranslationValue(Me.bbiExport.Caption)
		Me.bbiImport.Caption = m_Translate.GetSafeTranslationValue(Me.bbiImport.Caption)
		Me.bbiLanguage.Caption = m_Translate.GetSafeTranslationValue(Me.bbiLanguage.Caption)

	End Sub

	Private Function LoadYearDropDownData() As Boolean
		Dim success As Boolean = True

		Dim yearData = m_CommonDatabaseAccess.LoadMandantYears(m_InitializationData.MDData.MDNr)

		If (yearData Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Jahre (Mandanten) konnten nicht geladen werden."))
		End If

		Dim yearValues As List(Of YearValueView) = Nothing

		If Not yearData Is Nothing Then
			yearValues = New List(Of YearValueView)

			For Each yearValue In yearData
				yearValues.Add(New YearValueView With {.Value = yearValue})
			Next

		End If

		lueYear.Properties.DataSource = yearValues
		If lueYear.EditValue Is Nothing OrElse Not yearValues.Any(Function(data) data.Value = lueYear.EditValue) Then
			lueYear.EditValue = Now.Year
		Else
			lueYear.EditValue = lueYear.EditValue
		End If

		lueYear.Properties.ForceInitialize()

		Return yearData IsNot Nothing

	End Function

	Private Sub OnlueYear_EditValueChanged(sender As Object, e As System.EventArgs)
		If lueYear.EditValue Is Nothing Then Return

		If m_InitializationData.MDData.MDYear <> lueYear.EditValue Then
			m_InitializationData.MDData.MDYear = lueYear.EditValue

			Dim selectedData = SelectedExitingRecord

			LoadLAStammData()
			FocusLAData(100)

			If Not selectedData Is Nothing Then FocusLAData(selectedData.LANr)

		End If

	End Sub


	Private Function LoadLohnartList() As Boolean

		Dim listOfData = m_TablesettingDatabaseAccess.LoadLAStammData(m_InitializationData.MDData.MDNr, lueYear.EditValue)

		Dim gridData = (From person In listOfData
						Select New LAStammData With
			 {.LANr = person.LANr,
				.Selected = tgsSelection.EditValue,
				.LAText = person.LAText,
				.LALoText = person.LALoText,
				.LAOpText = person.LAOpText,
				.Bedingung = person.Bedingung,
				.RunFuncBefore = person.RunFuncBefore,
				.Verwendung = person.Verwendung,
				.Vorzeichen = person.Vorzeichen,
				.Rundung = person.Rundung,
				.TypeAnzahl = person.TypeAnzahl,
				.MAAnzVar = person.MAAnzVar,
				.FixAnzahl = person.FixAnzahl,
				.Sum0Anzahl = person.Sum0Anzahl,
				.Sum1Anzahl = person.Sum1Anzahl,
				.PrintAnzahl = person.PrintAnzahl,
				.TypeBasis = person.TypeBasis,
				.MABasVar = person.MABasVar,
				.FixBasis = person.FixBasis,
				.Sum0Basis = person.Sum0Basis,
				.Sum1Basis = person.Sum1Basis,
				.Sum2Basis = person.Sum2Basis,
				.PrintBasis = person.PrintBasis,
				.TypeAnsatz = person.TypeAnsatz,
				.MAAnsVar = person.MAAnsVar,
				.FixAnsatz = person.FixAnsatz,
				.SumAnsatz = person.SumAnsatz,
				.PrintAnsatz = person.PrintAnsatz,
				.Sum0Betrag = person.Sum0Betrag,
				.Sum1Betrag = person.Sum1Betrag,
				.Sum2Betrag = person.Sum2Betrag,
				.Sum3Betrag = person.Sum3Betrag,
				.PrintBetrag = person.PrintBetrag,
				.PrintLA = person.PrintLA,
				.BruttoPflichtig = person.BruttoPflichtig,
				.AHVPflichtig = person.AHVPflichtig,
				.ALVPflichtig = person.ALVPflichtig,
				.NBUVPflichtig = person.NBUVPflichtig,
				.UVPflichtig = person.UVPflichtig,
				.BVGPflichtig = person.BVGPflichtig,
				.KKPflichtig = person.KKPflichtig,
				.QSTPflichtig = person.QSTPflichtig,
				.MWSTPflichtig = person.MWSTPflichtig,
				.Reserve1 = person.Reserve1,
				.Reserve2 = person.Reserve2,
				.Reserve3 = person.Reserve3,
				.Reserve4 = person.Reserve4,
				.Reserve5 = person.Reserve5,
				.FerienInklusiv = person.FerienInklusiv,
				.FeierInklusiv = person.FeierInklusiv,
				._13Inklusiv = person._13Inklusiv,
				.ByNullCreate = person.ByNullCreate,
				.KDAnzahl = person.KDAnzahl,
				.KDBasis = person.KDBasis,
				.KDAnsatz = person.KDAnsatz,
				.Leerzeile = person.Leerzeile,
				.SKonto = person.SKonto,
				.HKonto = person.HKonto,
				.VorzeichenLAW = person.VorzeichenLAW,
				.BruttoLAWPflichtig = person.BruttoLAWPflichtig,
				.Kumulativ = person.Kumulativ,
				.LAWFeld = person.LAWFeld,
				.ES_Pflichtig = person.ES_Pflichtig,
				.DuppInKD = person.DuppInKD,
				.Result = person.Result,
				.LAJahr = person.LAJahr,
				.nolisting = person.nolisting,
				.ShowInZG = person.ShowInZG,
				.KumulativMonth = person.KumulativMonth,
				.TagesSpesen = person.TagesSpesen,
				.StdSpesen = person.StdSpesen,
				.KumLANr = person.KumLANr,
				.recid = person.recid,
				.LADeactivated = person.LADeactivated,
				.AGLA = person.AGLA,
				.ProTag = person.ProTag,
				.LOBeleg1 = person.LOBeleg1,
				.LOBeleg2 = person.LOBeleg2,
				.GleitTime = person.GleitTime,
				.AllowedMore_Anz = person.AllowedMore_Anz,
				.AllowedMore_Bas = person.AllowedMore_Bas,
				.AllowedMore_Ans = person.AllowedMore_Ans,
				.AllowedMore_Btr = person.AllowedMore_Btr,
				.Vorzeichen_2 = person.Vorzeichen_2,
				.WarningByZero = person.WarningByZero,
				.LAWFeld_0 = person.LAWFeld_0,
				.SeeKanton = person.SeeKanton,
				.ARGB_Verdienst_Unterkunft = person.ARGB_Verdienst_Unterkunft,
				.ARGB_Verdienst_Mahlzeit = person.ARGB_Verdienst_Mahlzeit,
				.CreatedOn = person.CreatedOn,
				.CreatedFrom = person.CreatedFrom,
				.ChangedOn = person.ChangedOn,
				.ChangedFrom = person.ChangedFrom,
				.NotForZV = person.NotForZV,
				.Vorzeichen_3 = person.Vorzeichen_3,
				.CalcFer13BasAsStd = person.CalcFer13BasAsStd,
				.FuncBeforePrint = person.FuncBeforePrint,
				.DB1_Bruttopflichtig = person.DB1_Bruttopflichtig,
				.Db1_AHVpflichtig = person.Db1_AHVpflichtig,
				.MoreProz4Fer = person.MoreProz4Fer,
				.MoreProz4Feier = person.MoreProz4Feier,
				.MoreProz413 = person.MoreProz413,
				.MoreProz4FerAmount = person.MoreProz4FerAmount,
				.MoreProz4FeierAmount = person.MoreProz4FeierAmount,
				.MoreProz413Amount = person.MoreProz413Amount,
				.MDNr = person.MDNr,
				.USNr = person.USNr,
				.LSE_Field = person.LSE_Field
			 }).ToList()

		Dim listDataSource As BindingList(Of LAStammData) = New BindingList(Of LAStammData)
		If gridData Is Nothing Then Return False
		lblSetActivFilter.Visible = False

		If chkFilterDeletedLA.Checked Then
			gridData = gridData.Where(Function(data) data.LADeactivated = False AndAlso data.Bedingung.ToLower <> "false").ToList()
		End If
		If chkFilterBruttoLA.Checked Then
			gridData = gridData.Where(Function(data) data.BruttoPflichtig = True).ToList()
		End If
		If chkFilterAHVLA.Checked Then
			gridData = gridData.Where(Function(data) data.AHVPflichtig = True).ToList()
		End If
		If chkFilterQSTLA.Checked Then
			gridData = gridData.Where(Function(data) data.QSTPflichtig = True).ToList()
		End If
		If chkFilterTopBrutto.Checked Then
			gridData = gridData.Where(Function(data) data.LANr <= 7000).ToList()
		End If
		If chkFilterNLA.Checked Then
			gridData = gridData.Where(Function(data) data.BruttoLAWPflichtig = True OrElse data.LAWFeld <> "").ToList()
		End If
		If chkFilterButtomNetto.Checked Then
			gridData = gridData.Where(Function(data) data.LANr >= 8000).ToList()
		End If

		If chkFilterFIBULa.Checked Then
			gridData = gridData.Where(Function(data) data.HKonto > 0 OrElse data.SKonto > 0).ToList()
		End If
		If chkFilterAGLA.Checked Then
			gridData = gridData.Where(Function(data) data.AGLA = True).ToList()
		End If

		If chkFilterBruttoLA.Checked OrElse chkFilterBruttoLA.Checked OrElse chkFilterBruttoLA.Checked OrElse chkFilterAHVLA.Checked _
			 OrElse chkFilterQSTLA.Checked OrElse chkFilterTopBrutto.Checked OrElse chkFilterNLA.Checked OrElse chkFilterButtomNetto.Checked OrElse chkFilterFIBULa.Checked OrElse chkFilterAGLA.Checked Then
			lblSetActivFilter.Visible = True
		End If


		For Each p In gridData
			listDataSource.Add(p)
		Next

		grdLohnart.DataSource = listDataSource
		Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), gvLohnart.RowCount)


		Return Not listOfData Is Nothing

	End Function


	Private Function LoadFBKData()

		Dim listOfData = m_TablesettingDatabaseAccess.LoadFIBUKontenData(m_InitializationData.UserData.UserLanguage)

		Dim gridData = (From person In listOfData
						Select New FIBUData With
													 {.KontoNr = person.KontoNr,
														.KontoName = person.KontoName
													 }).ToList()

		Dim listDataSource As BindingList(Of FIBUData) = New BindingList(Of FIBUData)

		For Each p In gridData
			listDataSource.Add(p)
		Next

		lue_Soll.Properties.DataSource = listDataSource
		lue_Haben.Properties.DataSource = listDataSource


		Return Not listOfData Is Nothing

	End Function

	''' <summary>
	''' Loads the Sign drop down data.
	''' </summary>
	Private Function LoadSignDropDownData() As Boolean
		Dim data = New List(Of SignViewData) From {
			New SignViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Positiv"), .Value = "+"},
			New SignViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Negativ"), .Value = "-"},
			New SignViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Neutral"), .Value = ""}
		}

		cbo_Zeichen.Properties.DataSource = data
		cbo_LAWZeichen.Properties.DataSource = data
		cbo_IfPositiv.Properties.DataSource = data
		cbo_Ifnegativ.Properties.DataSource = data

		cbo_Zeichen.Properties.ForceInitialize()
		cbo_LAWZeichen.Properties.ForceInitialize()
		cbo_IfPositiv.Properties.ForceInitialize()
		cbo_Ifnegativ.Properties.ForceInitialize()


		Return True
	End Function

	''' <summary>
	''' Loads the Verwendung drop down data.
	''' </summary>
	Private Function LoadErfassInDropDownData() As Boolean
		Dim data = New List(Of VerwendungViewData) From {
			New VerwendungViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Automatische Lohnart"), .Value = 0},
			New VerwendungViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("NUR Rapportverwaltung"), .Value = 1},
			New VerwendungViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("NUR Monatliche Lohnangaben"), .Value = 2},
			New VerwendungViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Rapport + Monatliche Lohnangaben"), .Value = 3},
			New VerwendungViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("NUR Vorschussverwaltung"), .Value = 4}
		}
		cbo_Erfassen.Properties.DataSource = data
		cbo_Erfassen.Properties.ForceInitialize()


		Return True
	End Function

	''' <summary>
	''' Loads the NLA drop down data.
	''' </summary>
	Private Function LoadLAWDropDownData() As Boolean
		Dim data = New List(Of LAWViewData) From {
New LAWViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Ziffer 1; Lohn"), .Value = "1_0"},
New LAWViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Ziffer 2; 2.1 Verpflegung, Unterkunft"), .Value = "2_1"},
New LAWViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Ziffer 2; 2.2 Privatanteil Geschäftswagen"), .Value = "2_2"},
New LAWViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Ziffer 2; 2.3 Andere Leistungen"), .Value = "2_3"},
New LAWViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Ziffer 3; Unregelmässige Leistungen"), .Value = "3_0"},
New LAWViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Ziffer 4; Kapitalleistungen"), .Value = "4_0"},
New LAWViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Ziffer 5; Beteiligung"), .Value = "5_0"},
New LAWViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Ziffer 6; Verwaltungsrathonorar"), .Value = "6_0"},
New LAWViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Ziffer 7; Andere Leistungen"), .Value = "7_0"},
New LAWViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Ziffer 9; Beiträge AHV/IV/EO/ALV/NBUV"), .Value = "9_0"},
New LAWViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Ziffer 10; 10.1 Ordentliche Beiträge"), .Value = "10_1"},
New LAWViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Ziffer 10; 10.2 Beiträge für den Einkauf"), .Value = "10_2"},
New LAWViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Ziffer 12; Quellensteuerabzug"), .Value = "12_0"},
New LAWViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Ziffer 13; 13.1.1 Reise, Verpflegung, Übernachtung"), .Value = "13_1_1"},
New LAWViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Ziffer 13; 13.1.2 Übrige"), .Value = "13_1_2"},
New LAWViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Ziffer 13; 13.2.1 Repräsentation"), .Value = "13_2_1"},
New LAWViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Ziffer 13; 13.2.2 Auto"), .Value = "13_2_2"},
New LAWViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Ziffer 13; 13.2.3 Übrige"), .Value = "13_2_3"},
New LAWViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Ziffer 13; 13.3 Beiträge an die Weiterbildung"), .Value = "13_3_0"}
}
		lue_LAWField.Properties.DataSource = data
		lue_LAWField.Properties.ForceInitialize()


		Return True
	End Function

	''' <summary>
	''' Loads the LSE_Field drop down data.
	''' </summary>
	Private Function LoadLSEFieldDropDownData() As Boolean
		Dim data = New List(Of LSEFieldViewData) From {
			New LSEFieldViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Grundlohn"), .Value = "I"},
			New LSEFieldViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Zulagen"), .Value = "J"},
			New LSEFieldViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Familienzulage"), .Value = "K"},
			New LSEFieldViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Beiträge an AHV/IV/EO/ALV/NBUV"), .Value = "L"},
			New LSEFieldViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Beiträge an die berufliche Vorsorge BVG"), .Value = "M"},
			New LSEFieldViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("13. Monatslohn"), .Value = "O"},
			New LSEFieldViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Entlöhnung aus geleisteten Überstunden"), .Value = "P"},
			New LSEFieldViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Unregelmässige Leistungen und Verwaltungsratsentschädigungen"), .Value = "Q"}
		}
		lue_LSEField.Properties.DataSource = data
		lue_LSEField.Properties.ForceInitialize()


		Return True
	End Function

	Private Function LoadGroupKeyDropDownData() As Boolean

		Dim resultData = New List(Of GroupKeyViewData)
		For i As Decimal = 150 To 160
			Dim data As New GroupKeyViewData

			data.Value = i
			If i = 153 Then
				data.DisplayText = "Exception Lohnarten für Quellensteuer-Hochrechnung"

			ElseIf i = 152 Then
				data.DisplayText = "Kurzarbeit Lohnart 103.01 wegen Parifond"

			ElseIf i = 154 Then
				data.DisplayText = "Quellensteuer-Lohnarten"

			Else
				data.DisplayText = "Reserve"

			End If

			resultData.Add(data)
		Next

		lueGroupKey.Properties.DataSource = resultData
		lueGroupKey.Properties.ForceInitialize()


		Return True
	End Function

	Private Function LoadRoundingDropDownData() As Boolean
		Dim data = New List(Of RoundingValueView) From {
			New RoundingValueView With {.Value = 1},
			New RoundingValueView With {.Value = 2},
			New RoundingValueView With {.Value = 3},
			New RoundingValueView With {.Value = 4},
			New RoundingValueView With {.Value = 5}
		}
		cbo_Rundung.Properties.DataSource = data
		cbo_Rundung.Properties.ForceInitialize()

	End Function


	''' <summary>
	''' Handles focus click of row.
	''' </summary>
	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		'm_selectedLohnartData = SelectedExitingRecord

		'If Not m_selectedLohnartData Is Nothing Then
		'	Dim success = LoadSelectedDetailData(m_selectedLohnartData.recid)

		'	If Not success Then
		'		m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht geladen werden."))
		'	End If

		'End If

	End Sub

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
			ElseIf TypeOf sender Is GridLookUpEdit Then
				Dim lookupEdit As GridLookUpEdit = CType(sender, GridLookUpEdit)
				lookupEdit.EditValue = Nothing

			ElseIf TypeOf sender Is ComboBoxEdit Then
				Dim comboboxEdit As ComboBoxEdit = CType(sender, ComboBoxEdit)
				comboboxEdit.EditValue = Nothing
			End If
		End If
	End Sub

	''' <summary>
	''' Handles focus change of row.
	''' </summary>
	Sub Ongv_FocusedRowChanged(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs)

		m_selectedLohnartData = SelectedExitingRecord

		If Not m_selectedLohnartData Is Nothing Then
			Dim success = LoadSelectedDetailData(m_selectedLohnartData.recid)

			If Not success Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht geladen werden."))
			End If

		End If

	End Sub

	''' <summary>
	''' Loads founded detail data.
	''' </summary>
	Private Function LoadSelectedDetailData(ByVal recid As Integer?) As Boolean
		Dim success As Boolean = True
		errorProviderMangement.ClearErrors()

		Dim data = m_TablesettingDatabaseAccess.LoadAssignedLAStammData(recid, lueYear.EditValue)

		If Not data Is Nothing Then

			txt_Jahr.EditValue = Format(data.LAJahr, "f0")
			txt_LANr.EditValue = Format(data.LANr, "f4")
			cbo_Erfassen.EditValue = data.Verwendung
			txt_TextRE.EditValue = data.LAOpText
			txt_TextLO.EditValue = data.LALoText

			chk_Deaktivate.Checked = data.LADeactivated
			txt_Filter.EditValue = data.Bedingung
			txt_FunkVorCreate.EditValue = data.RunFuncBefore
			cbo_Zeichen.EditValue = data.Vorzeichen
			cbo_Rundung.EditValue = data.Rundung

			cbo_Anzahl.EditValue = data.TypeAnzahl
			chk_Anzahl_Allowedmore.Checked = data.AllowedMore_Anz
			txt_Anzahl_VarMA.EditValue = data.MAAnzVar
			txt_Anzahl_VarKD.EditValue = data.KDAnzahl
			txt_Anzahl_Fix.EditValue = data.FixAnzahl
			cbo_AnzahlSum0.EditValue = data.Sum0Anzahl
			cbo_AnzahlSum1.EditValue = data.Sum1Anzahl
			chk_Anzahl_Print.Checked = data.PrintAnzahl

			cbo_Basis.EditValue = data.TypeBasis
			chk_Basis_Allowedmore.Checked = data.AllowedMore_Bas
			txt_Basis_VarMA.EditValue = data.MABasVar
			txt_Basis_VarKD.EditValue = data.KDBasis
			txt_Basis_Fix.EditValue = data.FixBasis
			cbo_BasisSum0.EditValue = data.Sum0Basis
			cbo_BasisSum1.EditValue = data.Sum1Basis
			cbo_BasisSum2.EditValue = data.Sum2Basis
			chk_Basis_Print.Checked = data.PrintBasis

			cbo_Ansatz.EditValue = data.TypeAnsatz
			chk_Ansatz_Allowedmore.Checked = data.AllowedMore_Ans
			txt_Ansatz_VarMA.EditValue = data.MAAnsVar
			txt_Ansatz_VarKD.EditValue = data.KDAnsatz
			txt_Ansatz_Fix.EditValue = data.FixAnsatz
			cbo_AnsatzSum0.EditValue = data.SumAnsatz
			chk_Ansatz_Print.Checked = data.PrintAnsatz

			chk_Betrag_Allowedmore.Checked = data.AllowedMore_Btr
			cbo_BetragSum0.EditValue = data.Sum0Betrag
			cbo_BetragSum1.EditValue = data.Sum1Betrag
			cbo_BetragSum2.EditValue = data.Sum2Betrag
			cbo_BetragSum3.EditValue = data.Sum3Betrag
			chk_Betrag_Print.Checked = data.PrintBetrag
			chk_BetragNoPrintZero.Checked = data.PrintLA

			chk_proTag.Checked = data.ProTag
			chk_NoZV.Checked = data.NotForZV
			chk_Gleitzeit.Checked = data.GleitTime
			chk_AG.Checked = data.AGLA
			chk_kantonUnterschiedlich.Checked = data.SeeKanton
			chkARGB18_1.Checked = data.ARGB_Verdienst_Unterkunft
			chkARGB18_2.Checked = data.ARGB_Verdienst_Mahlzeit

			chk_Brutto.Checked = data.BruttoPflichtig
			chk_AHV.Checked = data.AHVPflichtig
			chk_ALV.Checked = data.ALVPflichtig
			chk_NBUV.Checked = data.NBUVPflichtig
			chk_UV.Checked = data.UVPflichtig
			chk_BruttoDB1.Checked = data.DB1_Bruttopflichtig
			chk_AHVDB1.Checked = data.Db1_AHVpflichtig
			chk_BVG.Checked = data.BVGPflichtig
			chk_KTG.Checked = data.KKPflichtig
			chk_QST.Checked = data.QSTPflichtig
			chk_Res1.Checked = data.Reserve1
			chk_Res2.Checked = data.Reserve2
			chk_Res3.Checked = data.Reserve3
			chk_Res4.Checked = data.Reserve4
			chk_Res5.Checked = data.Reserve5
			chk_Ferien.Checked = data.FerienInklusiv
			chk_Feier.Checked = data.FeierInklusiv
			chk_13Lohn.Checked = data._13Inklusiv
			chk_Ferienmore.Checked = data.MoreProz4Fer
			chk_Feiermore.Checked = data.MoreProz4Feier
			chk_13Lohnmore.Checked = data.MoreProz413

			txtMoreProz4FerAmount.EditValue = Val(data.MoreProz4FerAmount)
			txtMoreProz4FeierAmount.EditValue = Val(data.MoreProz4FeierAmount)
			txtMoreProz413Amount.EditValue = Val(data.MoreProz413Amount)

			chk_MwSt.Checked = data.MWSTPflichtig
			chk_ESPflicht.Checked = data.ES_Pflichtig
			chk_TSpesen.Checked = data.TagesSpesen
			chk_StdSpesen.Checked = data.StdSpesen

			chk_Betrag0Create.Checked = data.ByNullCreate
			chk_Betrag0Warn.Checked = data.WarningByZero
			chk_Dupplikat.Checked = data.DuppInKD
			chk_NoListing.Checked = data.nolisting
			chk_ZGWarn.Checked = data.ShowInZG
			chk_KumYear.Checked = data.Kumulativ
			chk_KumMonth.Checked = data.KumulativMonth
			txt_LANrMonth.EditValue = data.KumLANr

			chk_LAWBrutto.Checked = data.BruttoLAWPflichtig
			cbo_LAWZeichen.EditValue = data.VorzeichenLAW
			lue_LAWField.EditValue = data.LAWFeld

			lue_Soll.EditValue = data.SKonto
			lue_Haben.EditValue = data.HKonto

			cbo_IfPositiv.EditValue = data.Vorzeichen_2
			cbo_Ifnegativ.EditValue = data.Vorzeichen_3
			cbo_1Lohnbeleg.EditValue = data.LOBeleg1
			cbo_2Lohnbeleg.EditValue = data.LOBeleg2

			lue_LSEField.EditValue = data.LSE_Field
			lueGroupKey.EditValue = data.GroupKey

			bsiCreated.Caption = String.Format("{0:G}, {1}", data.CreatedOn, data.CreatedFrom)
			bsiChanged.Caption = String.Format("{0:G}, {1}", data.ChangedOn, data.ChangedFrom)


			m_CurrentRecordNumber = data.recid
			success = True

		Else
			success = False

		End If

		Return success

	End Function

	Private Function SaveLohnartData() As Boolean
		Dim success As Boolean = True
		Dim msg As String

		success = success AndAlso ValidateInputData()
		If Not success Then Return success
		Try

			Dim data As LAStammData = Nothing
			data = New LAStammData

			data.recid = m_CurrentRecordNumber.GetValueOrDefault(0)

			data.LANr = CType(txt_LANr.EditValue, Decimal)
			data.LAJahr = CType(txt_Jahr.EditValue, Integer)

			data.Verwendung = cbo_Erfassen.EditValue
			data.LAText = txt_TextLO.EditValue
			data.LAOpText = txt_TextRE.EditValue
			data.LALoText = txt_TextLO.EditValue

			data.LADeactivated = chk_Deaktivate.Checked
			data.Bedingung = txt_Filter.EditValue
			data.RunFuncBefore = txt_FunkVorCreate.EditValue
			data.Vorzeichen = cbo_Zeichen.EditValue
			data.Rundung = cbo_Rundung.EditValue

			data.TypeAnzahl = cbo_Anzahl.EditValue
			data.AllowedMore_Anz = chk_Anzahl_Allowedmore.Checked
			data.MAAnzVar = txt_Anzahl_VarMA.EditValue
			data.KDAnzahl = txt_Anzahl_VarKD.EditValue
			data.FixAnzahl = txt_Anzahl_Fix.EditValue
			data.Sum0Anzahl = cbo_AnzahlSum0.EditValue
			data.Sum1Anzahl = cbo_AnzahlSum1.EditValue
			data.PrintAnzahl = chk_Anzahl_Print.Checked

			data.TypeBasis = cbo_Basis.EditValue
			data.AllowedMore_Bas = chk_Basis_Allowedmore.Checked
			data.MABasVar = txt_Basis_VarMA.EditValue
			data.KDBasis = txt_Basis_VarKD.EditValue
			data.FixBasis = txt_Basis_Fix.EditValue
			data.Sum0Basis = cbo_BasisSum0.EditValue
			data.Sum1Basis = cbo_BasisSum1.EditValue
			data.Sum2Basis = cbo_BasisSum2.EditValue
			data.PrintBasis = chk_Basis_Print.Checked

			data.TypeAnsatz = cbo_Ansatz.EditValue
			data.AllowedMore_Ans = chk_Ansatz_Allowedmore.Checked
			data.MAAnsVar = txt_Ansatz_VarMA.EditValue
			data.KDAnsatz = txt_Ansatz_VarKD.EditValue
			data.FixAnsatz = txt_Ansatz_Fix.EditValue
			data.SumAnsatz = cbo_AnsatzSum0.EditValue
			data.PrintAnsatz = chk_Ansatz_Print.Checked

			data.AllowedMore_Btr = chk_Betrag_Allowedmore.Checked
			data.Sum0Betrag = cbo_BetragSum0.EditValue
			data.Sum1Betrag = cbo_BetragSum1.EditValue
			data.Sum2Betrag = cbo_BetragSum2.EditValue
			data.Sum3Betrag = cbo_BetragSum3.EditValue
			data.PrintBetrag = chk_Betrag_Print.Checked
			data.PrintLA = chk_BetragNoPrintZero.Checked

			data.ProTag = chk_proTag.Checked
			data.NotForZV = chk_NoZV.Checked
			data.GleitTime = chk_Gleitzeit.Checked
			data.AGLA = chk_AG.Checked
			data.SeeKanton = chk_kantonUnterschiedlich.Checked
			data.ARGB_Verdienst_Unterkunft = chkARGB18_1.Checked
			data.ARGB_Verdienst_Mahlzeit = chkARGB18_2.Checked

			data.BruttoPflichtig = chk_Brutto.Checked
			data.AHVPflichtig = chk_AHV.Checked
			data.ALVPflichtig = chk_ALV.Checked
			data.NBUVPflichtig = chk_NBUV.Checked
			data.UVPflichtig = chk_UV.Checked
			data.DB1_Bruttopflichtig = chk_BruttoDB1.Checked
			data.Db1_AHVpflichtig = chk_AHVDB1.Checked
			data.BVGPflichtig = chk_BVG.Checked
			data.KKPflichtig = chk_KTG.Checked
			data.QSTPflichtig = chk_QST.Checked
			data.Reserve1 = chk_Res1.Checked
			data.Reserve2 = chk_Res2.Checked
			data.Reserve3 = chk_Res3.Checked
			data.Reserve4 = chk_Res4.Checked
			data.Reserve5 = chk_Res5.Checked
			data.FerienInklusiv = chk_Ferien.Checked
			data.FeierInklusiv = chk_Feier.Checked
			data._13Inklusiv = chk_13Lohn.Checked
			data.MoreProz4Fer = chk_Ferienmore.Checked
			data.MoreProz4Feier = chk_Feiermore.Checked
			data.MoreProz413 = chk_13Lohnmore.Checked

			data.MoreProz4FerAmount = Val(txtMoreProz4FerAmount.EditValue)
			data.MoreProz4FeierAmount = Val(txtMoreProz4FeierAmount.EditValue)
			data.MoreProz413Amount = Val(txtMoreProz413Amount.EditValue)

			data.MWSTPflichtig = chk_MwSt.Checked
			data.ES_Pflichtig = chk_ESPflicht.Checked
			data.TagesSpesen = chk_TSpesen.Checked
			data.StdSpesen = chk_StdSpesen.Checked

			data.ByNullCreate = chk_Betrag0Create.Checked
			data.WarningByZero = chk_Betrag0Warn.Checked
			data.DuppInKD = chk_Dupplikat.Checked
			data.nolisting = chk_NoListing.Checked
			data.ShowInZG = chk_ZGWarn.Checked
			data.Kumulativ = chk_KumYear.Checked
			data.KumulativMonth = chk_KumMonth.Checked
			data.KumLANr = txt_LANrMonth.EditValue

			data.BruttoLAWPflichtig = chk_LAWBrutto.Checked
			data.VorzeichenLAW = cbo_LAWZeichen.EditValue
			data.LAWFeld = lue_LAWField.EditValue

			data.SKonto = lue_Soll.EditValue
			data.HKonto = lue_Haben.EditValue

			data.Vorzeichen_2 = cbo_IfPositiv.EditValue
			data.Vorzeichen_3 = cbo_Ifnegativ.EditValue
			data.LOBeleg1 = cbo_1Lohnbeleg.EditValue
			data.LOBeleg2 = cbo_2Lohnbeleg.EditValue

			data.LSE_Field = lue_LSEField.EditValue
			data.GroupKey = lueGroupKey.EditValue


			data.CreatedFrom = m_InitializationData.UserData.UserFullName
			data.ChangedFrom = m_InitializationData.UserData.UserFullName
			data.MDNr = m_InitializationData.MDData.MDNr
			data.USNr = m_InitializationData.UserData.UserNr

			If m_CurrentRecordNumber.GetValueOrDefault(0) = 0 Then

				success = m_TablesettingDatabaseAccess.AddLAStammData(m_InitializationData.MDData.MDNr, lueYear.EditValue, data)

			Else
				success = m_TablesettingDatabaseAccess.UpdateAssignedLAStammData(m_InitializationData.MDData.MDNr, lueYear.EditValue, data)

			End If

			If success Then
				msg = "Die Daten wurden gespeichert."
				m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue(msg))
				If m_CurrentRecordNumber.GetValueOrDefault(0) = 0 AndAlso LoadLAStammData() Then FocusLAData(data.LANr)

			Else
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))

			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

			success = False
		End Try


		Return success

	End Function

	Private Function DeleteLohntartData() As Boolean
		Dim success As Boolean = False

		Dim selectedData = SelectedExitingRecord

		If Not selectedData Is Nothing Then

			If (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Möchten Sie die ausgewählen Daten wirklich löschen?"),
																m_Translate.GetSafeTranslationValue("Daten endgültig löschen?"))) Then

				success = m_TablesettingDatabaseAccess.DeleteLAStammData(selectedData.recid, m_InitializationData.UserData.UserFullName, m_InitializationData.UserData.UserNr)
			Else
				Return False

			End If

		End If

		If Not success Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Daten konnte nicht gelöscht werden."))
		End If

		If success AndAlso LoadLAStammData() Then
			FocusLAData(selectedData.LANr)
		End If

		Return success

	End Function

	Private Sub OnbbiPrint_ItemClick(sender As Object, e As ItemClickEventArgs) Handles bbiPrint.ItemClick
		Dim success = StartPrinting()
	End Sub

	Private Function StartPrinting() As Boolean
		Dim success As Boolean = True
		Dim ShowDesign As Boolean = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 506, m_InitializationData.MDData.MDNr)
		ShowDesign = ShowDesign AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)

		Try
			Dim esSalaryData = CType(gvLohnart.DataSource, BindingList(Of LAStammData))
			Dim index = esSalaryData.Where(Function(data) data.LANr <> 0 And data.Selected = True).ToList()


			Dim _Setting As New LAStammPrintSetting With {.m_initData = m_InitializationData, .m_Translate = m_Translate,
																											 .ShowAsDesgin = ShowDesign,
																											 .frmhwnd = GetHwnd,
																											 .JobNr2Print = "9.0",
																											 .LADataList = index,
																											 .ListFilterBez = New List(Of String) From {String.Format("Jahr: {0:F0}", lueYear.EditValue)}}

			Dim o2Open As New PrintLOListing.LOListing_LAStamm(_Setting)
			If ShowDesign Then
				o2Open.ShowInDesign()
			Else
				success = success AndAlso o2Open.ShowInPrint(True)

			End If
			o2Open.Dispose()


		Catch ex As Exception
			Return False

		End Try

		Return success

	End Function

	Private Sub OnbbiSave_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSave.ItemClick
		Dim success = SaveLohnartData()
	End Sub

	Private Sub OnbbiDelete_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiDelete.ItemClick
		DeleteLohntartData()
	End Sub

	Private Sub OnbbiCopy_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiCopy.ItemClick
		Dim oldIDNumber = m_CurrentRecordNumber

		m_CurrentRecordNumber = Nothing
		txt_LANr.EditValue = Nothing

	End Sub

	Private Sub OnbbiExport_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiExport.ItemClick

		Dim selectedData = GetSelectedLAItems()


		'Dim esSalaryData = CType(gvLohnart.DataSource, BindingList(Of LAStammData))
		'Dim index = esSalaryData.Where(Function(data) data.LANr <> 0 And data.Selected = 1).ToList()


		Dim m_path As New SPProgUtility.ProgPath.ClsProgPath
		Dim filename = Path.Combine(m_path.GetSpS2DeleteHomeFolder, "lastamm.xml")
		Dim objStreamWriter As New StreamWriter(filename)

		Dim b As New XmlSerializer(m_SelectedLAData.GetType)
		b.Serialize(objStreamWriter, m_SelectedLAData)
		objStreamWriter.Close()


		'Dim x As New XmlSerializer(selectedData.GetType)
		'x.Serialize(objStreamWriter, selectedData)
		'objStreamWriter.Close()

		m_UtilityUI.ShowInfoDialog(String.Format(m_Translate.GetSafeTranslationValue("Die Daten wurdne in {0} gespeichert."), filename))
		Process.Start("explorer.exe", "/select," & filename)

	End Sub

	Private Sub OnbbiImport_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiImport.ItemClick

		Dim m_path As New SPProgUtility.ProgPath.ClsProgPath
		Dim filename = Path.Combine(m_path.GetSpS2DeleteHomeFolder, "lastamm.xml")
		Dim openFileDialog1 As New OpenFileDialog()
		openFileDialog1.Filter = "XML-Dateien (*.xml)|*.xml|Alle Dateien|*.*"
		openFileDialog1.InitialDirectory = m_path.GetSpS2DeleteHomeFolder
		openFileDialog1.FileName = "lastamm.xml"

		Dim result = openFileDialog1.ShowDialog()
		If result = DialogResult.Cancel Then
			Return
		Else
			filename = openFileDialog1.FileName
		End If

		'Dim selectedData = SelectedExitingRecord
		'Dim x As New XmlSerializer(selectedData.GetType)

		Try

			'Deserialize text file to a new object.
			Dim objStreamReader As New StreamReader(filename)
			Dim dataToImport As New List(Of LAStammData)
			Dim x As New XmlSerializer(dataToImport.GetType)
			dataToImport = x.Deserialize(objStreamReader)
			objStreamReader.Close()

			Dim msg As String = String.Empty
			Dim success = True
			success = m_UtilityUI.ShowYesNoDialog(String.Format(m_Translate.GetSafeTranslationValue("Hiermit Sie <b>importieren/aktualisieren {0} Lohnarten</b>.<br>Möchten Sie mit dem Vorgang fortfahren?"), dataToImport.Count), m_Translate.GetSafeTranslationValue("Lohnarten importieren"))
			If Not success Then Return

			For Each laRecord In dataToImport
				Dim esSalaryData = CType(gvLohnart.DataSource, BindingList(Of LAStammData))
				Dim index = esSalaryData.Where(Function(data) data.LANr = laRecord.LANr).ToList()
				If index Is Nothing OrElse index.Count = 0 Then
					m_CurrentRecordNumber = Nothing
					laRecord.CreatedFrom = "System Import"
					laRecord.CreatedOn = Now
					laRecord.ChangedFrom = String.Empty
					laRecord.ChangedOn = Nothing
					laRecord.MDNr = m_InitializationData.MDData.MDNr

					success = m_TablesettingDatabaseAccess.AddLAStammData(m_InitializationData.MDData.MDNr, lueYear.EditValue, laRecord)

				Else
					msg = m_Translate.GetSafeTranslationValue("Es existiert eine Lohnart mit der Nummer Lohnart {0:f4}. Möchten Sie die Lohnart ändern?")
					msg = String.Format(msg, laRecord.LANr)
					If m_UtilityUI.ShowYesNoDialog(msg, m_Translate.GetSafeTranslationValue("Daten importieren")) = True Then
						laRecord.recid = index(0).recid
						laRecord.ChangedFrom = "System Import"
						laRecord.ChangedOn = Now
						laRecord.MDNr = m_InitializationData.MDData.MDNr

						success = m_TablesettingDatabaseAccess.UpdateAssignedLAStammData(m_InitializationData.MDData.MDNr, lueYear.EditValue, laRecord)
					Else

						Continue For
					End If

				End If

			Next




			'Dim esSalaryData = CType(gvLohnart.DataSource, BindingList(Of LAStammData))
			'Dim index = esSalaryData.Where(Function(data) data.Selected = 1).ToList() ' data.LANr = dataToImport.LANr).ToList()
			'If index Is Nothing OrElse index.Count = 0 Then
			'	m_CurrentRecordNumber = Nothing
			'	dataToImport.CreatedFrom = "System Import"
			'	dataToImport.CreatedOn = Now
			'	dataToImport.ChangedFrom = String.Empty
			'	dataToImport.ChangedOn = Nothing
			'	dataToImport.MDNr = m_InitializationData.MDData.MDNr

			'	success = m_TablesettingDatabaseAccess.AddLAStammData(m_InitializationData.MDData.MDNr, lueYear.EditValue, dataToImport)

			'Else
			'	msg = m_Translate.GetSafeTranslationValue("Es existiert eine Lohnart mit der Nummer Lohnart {0:f4}. Möchten Sie die Lohnart ändern?")
			'	msg = String.Format(msg, dataToImport.LANr)
			'	If m_UtilityUI.ShowYesNoDialog(msg, m_Translate.GetSafeTranslationValue("Daten importieren")) = True Then
			'		dataToImport.recid = index(0).recid
			'		dataToImport.ChangedFrom = "System Import"
			'		dataToImport.ChangedOn = Now
			'		dataToImport.MDNr = m_InitializationData.MDData.MDNr

			'		success = m_TablesettingDatabaseAccess.UpdateAssignedLAStammData(m_InitializationData.MDData.MDNr, lueYear.EditValue, dataToImport)
			'	Else

			'		Return
			'	End If

			'End If

			If success Then
				m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Die Daten wurden gespeichert."))
				If LoadLAStammData() Then FocusLAData(100)

			Else
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))

			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

	End Sub

	Private Sub bbiLanguage_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiLanguage.ItemClick
		Dim frm As frmLATranslation

		frm = New frmLATranslation
		frm.LoadLAStammTranslationData()

		frm.Show()
		frm.BringToFront()

	End Sub

	Private Sub FocusLAData(ByVal LANr As Decimal)

		If Not grdLohnart.DataSource Is Nothing Then

			Dim esSalaryData = CType(gvLohnart.DataSource, BindingList(Of LAStammData))

			Dim index = esSalaryData.ToList().FindIndex(Function(data) data.LANr >= LANr)

			Dim rowHandle = gvLohnart.GetRowHandle(index)
			gvLohnart.FocusedRowHandle = rowHandle

		End If

	End Sub

	Private Sub OngvLohnart_RowCellStyle(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs) Handles gvLohnart.RowCellStyle

		If (e.RowHandle >= 0) Then
			Dim view As GridView = CType(sender, GridView)
			Dim data = CType(view.GetRow(e.RowHandle), LAStammData)

			If data.LADeactivated OrElse data.Bedingung.ToLower = "false" Then e.Appearance.BackColor = Color.PaleVioletRed

		End If

	End Sub

	Private Sub OnchkBruttoLA_CheckedChanged(sender As Object, e As EventArgs) Handles chkFilterBruttoLA.CheckedChanged
		LoadLohnartList()
	End Sub

	Private Sub OnchkFilterAHVLA_CheckedChanged(sender As Object, e As EventArgs) Handles chkFilterAHVLA.CheckedChanged
		LoadLohnartList()
	End Sub

	Private Sub OnchkFilterQSTLA_CheckedChanged(sender As Object, e As EventArgs) Handles chkFilterQSTLA.CheckedChanged
		LoadLohnartList()
	End Sub

	Private Sub OnchkFilterTopBrutto_CheckedChanged(sender As Object, e As EventArgs) Handles chkFilterTopBrutto.CheckedChanged
		LoadLohnartList()
	End Sub

	Private Sub OnchkFilterAGLA_CheckedChanged(sender As Object, e As EventArgs) Handles chkFilterAGLA.CheckedChanged
		LoadLohnartList()
	End Sub

	Private Sub OnchkFilterButtomNetto_CheckedChanged(sender As Object, e As EventArgs) Handles chkFilterButtomNetto.CheckedChanged
		LoadLohnartList()
	End Sub

	Private Sub OnchkFilterFIBULa_CheckedChanged(sender As Object, e As EventArgs) Handles chkFilterFIBULa.CheckedChanged
		LoadLohnartList()
	End Sub

	Private Sub OnchkFilterNLA_CheckedChanged(sender As Object, e As EventArgs) Handles chkFilterNLA.CheckedChanged
		LoadLohnartList()
	End Sub

	Private Sub OnchkFilterDeletedLA_CheckedChanged(sender As Object, e As EventArgs) Handles chkFilterDeletedLA.CheckedChanged
		LoadLohnartList()
	End Sub


	Private Function GetSelectedLAItems() As List(Of LAStammData)

		m_SelectedLAData = New List(Of LAStammData)
		gvLohnart.FocusedColumn = gvLohnart.VisibleColumns(1)
		grdLohnart.RefreshDataSource()
		Dim printList As BindingList(Of LAStammData) = grdLohnart.DataSource
		Dim sentList = (From r In printList Where r.Selected = True).ToList()

		'SelectedLONr.Clear()
		'SelectedMANr.Clear()
		'SelectedData2WOS.Clear()
		'SelectedMALang.Clear()

		For Each receiver In sentList
			Trace.WriteLine(String.Format("lanr: {0:F5}", receiver.LANr))
			'	If receiver.SelectedRec Then
			'		m_SelectedPayroll.Add(New LAStammData With {
			'			.recid = receiver.recid,
			'			.CreatedFrom = receiver.CreatedFrom,
			'			.CreatedOn = receiver.CreatedOn,
			'			.employeefirstname = receiver.employeefirstname,
			'			.employeeLanguage = receiver.employeeLanguage,
			'			.employeelastname = receiver.employeelastname,
			'			.Jahr = receiver.jahr,
			'			.LONr = receiver.LONr,
			'			.MANr = receiver.MANr,
			'			.MDNr = receiver.MDNr,
			'			.monat = receiver.monat,
			'			.Send2WOS = receiver.Send2WOS
			'		})

			'		SelectedLONr.Add(receiver.LONr)
			'		SelectedMANr.Add(receiver.MANr)
			'		SelectedData2WOS.Add(If(m_SelectedPrintValue = WOSSENDValue.PrintWithoutSending, False, receiver.Send2WOS))
			'		SelectedMALang.Add(receiver.employeeLanguage)

			'	End If

		Next

		m_SelectedLAData = sentList

		Return m_SelectedLAData

	End Function

	''' <summary>
	''' Validates input data.
	''' </summary>
	Private Function ValidateInputData() As Boolean
		Dim success As Boolean = True

		errorProviderMangement.ClearErrors()

		Dim errorText As String = "Bitte geben Sie einen Wert ein."

		Dim isValid As Boolean = True


		isValid = isValid And SetErrorIfInvalid(txt_Jahr, errorProviderMangement, txt_Jahr.EditValue <= 0, errorText)
		'isValid = isValid And SetErrorIfInvalid(txt_LANr, errorProviderMangement, txt_LANr.EditValue <= 0, errorText)

		Dim listOfData = m_TablesettingDatabaseAccess.LoadLAStammData(m_InitializationData.MDData.MDNr, lueYear.EditValue)
		If m_CurrentRecordNumber.GetValueOrDefault(0) = 0 Then
			Dim index = listOfData.Where(Function(dlist) dlist.LANr = Val(txt_LANr.EditValue) AndAlso dlist.LAJahr = Val(txt_Jahr.EditValue)).FirstOrDefault
			If Not index Is Nothing Then
				If index.LADeactivated.GetValueOrDefault(False) Then
					errorText = "Es existiert bereits eine gelöschte Lohnart mit der gleichen Nummer >>> {0}: {1}"
				Else
					errorText = "Es existiert bereits eine Lohnart mit der gleichen Nummer >>> {0}: {1}"
				End If
				errorText = String.Format(m_Translate.GetSafeTranslationValue(errorText), index.LANr, index.LALoText)

				success = False

			ElseIf Val(txt_LANr.EditValue) > 0 Then
				Dim value = Val(txt_LANr.EditValue) Mod 1
				If m_InitializationData.UserData.UserNr <> 1 AndAlso Format(value, "f4") < 0.8 Then
					errorText = "Achtung: Aus Sicherheitsgründen empfehlen wir Ihnen die Lohnart wie folgt zu erfassen: Nummer.xxxx(x = eine Zahl zwichen 0.80 and 0.99)."
					Dim msg As String = "Achtung: Aus Sicherheitsgründen empfehlen wir Ihnen die Lohnart wie folgt zu erfassen: Nummer.xxxx(x = eine Zahl zwichen 0.80 and 0.99). Möchten Sie Ihre Daten trotzdem speichern?"
					If Not m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue(msg), m_Translate.GetSafeTranslationValue("Neue Lohnart erfassen"), MessageBoxDefaultButton.Button2) Then
						success = False
					End If
				End If
			End If

		Else

			Dim index = listOfData.Where(Function(dlist) dlist.LANr = Val(txt_LANr.EditValue) AndAlso dlist.LAJahr = Val(txt_Jahr.EditValue) AndAlso dlist.recid <> m_selectedLohnartData.recid).FirstOrDefault
			If Not index Is Nothing Then
				If index.LADeactivated.GetValueOrDefault(False) Then
					errorText = "Es existiert bereits eine gelöschte Lohnart mit der gleichen Nummer >>> {0}: {1}"
				Else
					errorText = "Es existiert bereits eine Lohnart mit der gleichen Nummer >>> {0}: {1}"
				End If
				errorText = String.Format(m_Translate.GetSafeTranslationValue(errorText), index.LANr, index.LALoText)

				success = False

			End If

		End If

		isValid = isValid And SetErrorIfInvalid(txt_LANr, errorProviderMangement, txt_LANr.EditValue <= 0 OrElse Not success, m_Translate.GetSafeTranslationValue(errorText))


		Return isValid

	End Function

	Private Sub tgsSelection_Toggled(sender As Object, e As EventArgs) Handles tgsSelection.Toggled
		SelDeSelectItems(tgsSelection.EditValue)
	End Sub

	Private Sub SelDeSelectItems(ByVal selectItem As Boolean)
		Dim data As BindingList(Of LAStammData) = grdLohnart.DataSource

		If Not data Is Nothing Then
			For Each item In data
				item.Selected = selectItem
			Next
		End If

		gvLohnart.RefreshData()

	End Sub

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



#Region "Forms"

	Private Sub Onfrm_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
		SaveFromSettings()
	End Sub

	''' <summary>
	''' Loads form settings if form gets visible.
	''' </summary>
	Private Sub OnFrm_VisibleChanged(sender As System.Object, e As System.EventArgs) Handles MyBase.VisibleChanged

		If Visible Then
			LoadFormSettings()
		End If

	End Sub

	''' <summary>
	''' Loads form settings.
	''' </summary>
	Private Sub LoadFormSettings()

		Try
			Dim setting_form_height = My.Settings.ifrmHeightLAStamm
			Dim setting_form_width = My.Settings.ifrmWidthLAStamm
			Dim setting_form_location = My.Settings.frmLocationLAStamm

			If setting_form_height > 0 Then Me.Height = Math.Max(Me.Height, setting_form_height)
			If setting_form_width > 0 Then Me.Width = Math.Max(Me.Width, setting_form_width)

			If Not String.IsNullOrEmpty(setting_form_location) Then
				Dim aLoc As String() = setting_form_location.Split(CChar(";"))
				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Val(aLoc(0)), Val(aLoc(1)))
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())

		End Try

	End Sub

	''' <summary>
	''' Saves the form settings.
	''' </summary>
	Private Sub SaveFromSettings()

		' Save form location, width and height in setttings
		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.frmLocationLAStamm = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.ifrmWidthLAStamm = Me.Width
				My.Settings.ifrmHeightLAStamm = Me.Height

				My.Settings.Save()

			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())

		End Try

	End Sub

	Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
		Me.Close()
	End Sub



#End Region



#Region "Helpers"

	'Private Class LAStammViewData
	'	Inherits LAStammData
	'	Public Property Selected As Boolean

	'End Class

	Private Class YearValueView
		Public Property Value As Integer

	End Class


	''' <summary>
	''' Sing view data.
	''' </summary>
	Class SignViewData

		Public Property DisplayText As String
		Public Property Value As String

	End Class

	''' <summary>
	''' Verwendung view data.
	''' </summary>
	Class VerwendungViewData

		Public Property DisplayText As String
		Public Property Value As String

	End Class


	''' <summary>
	''' LAW view data.
	''' </summary>
	Class LAWViewData

		Public Property DisplayText As String
		Public Property Value As String

	End Class

	''' <summary>
	''' LSE_LOArt view data.
	''' </summary>
	Class LSELOArtViewData

		Public Property DisplayText As String
		Public Property Value As Integer

	End Class


	''' <summary>
	''' LSE_Field view data.
	''' </summary>
	Class LSEFieldViewData

		Public Property DisplayText As String
		Public Property Value As String

	End Class

	Class GroupKeyViewData

		Public Property Value As Decimal?
		Public Property DisplayText As String

		Public ReadOnly Property ValueLabel As String
			Get
				Return (String.Format("{0} - {1}", Value, DisplayText))
			End Get
		End Property
	End Class

	Private Class RoundingValueView
		Public Property Value As Integer

	End Class



#End Region


End Class