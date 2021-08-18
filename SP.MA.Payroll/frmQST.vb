Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Employee.DataObjects.Salary
Imports SP.DatabaseAccess.PayrollMng
Imports SPProgUtility.Mandanten
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports SP.DatabaseAccess.Employee
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.PayrollMng.DataObjects
Imports SPProgUtility.CommonXmlUtility
Imports SPProgUtility.ProgPath
Imports DevExpress.LookAndFeel
Imports System.ComponentModel
Imports SP.Internal.Automations
Imports SP.Internal.Automations.BaseTable

Public Class frmQST

#Region "Private Consts"

	Private Const MANDANT_XML_SETTING_SPUTNIK_PAYROLL_SETTING As String = "MD_{0}/Lohnbuchhaltung"
	Private Const MANDANT_XML_SETTING_SPUTNIK_EMPLOYEE_TAXINFO_WEBSERVICE_URI As String = "MD_{0}/Interfaces/webservices/webservicetaxinfoservices"
	Private Const DEFAULT_SPUTNIK_EMPLOYEE_TAXINFO_WEBSERVCIE_URI = "wsSPS_services/SPEmployeeTaxInfoService.asmx" ' "http://asmx.domain.com/wsSPS_services/SPEmployeeTaxInfoService.asmx"
	Private Const CP_NOCLOSE_BUTTON As Integer = &H200
#End Region

#Region "Private Fields"

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' The common database access.
	''' </summary>
	Private m_CommonDatabaseAccess As ICommonDatabaseAccess

	''' <summary>
	''' The Payroll data access object.
	''' </summary>
	Private m_PayrollDatabaseAccess As IPayrollDatabaseAccess

	''' <summary>
	''' The emplyoee data access object.
	''' </summary>
	Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

	''' <summary>
	''' The mandant.
	''' </summary>
	Private m_md As Mandant

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As Utility

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' Boolean flag indicating if initial data has been loaded.
	''' </summary>
	Private m_IsInitialDataLoaded As Boolean = False

	''' <summary>
	''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
	''' </summary>
	Private m_SuppressUIEvents As Boolean = False

	''' <summary>
	''' The employee master data.
	''' </summary>
	Private m_EmployeeData As EmployeeMasterData

	''' <summary>
	''' The emplyoee LO setting.
	''' </summary>
	Private m_EmployeeLOSetting As EmployeeLOSettingsData

	''' <summary>
	''' Payroll context data.
	''' </summary>
	Private m_PrCtx As PayrollContextData

	''' <summary>
	''' Taggeld Betrag for month
	''' </summary>
	Private m_TagGeldBetragForMonth As TagGeldBetragForMonth

	''' <summary>
	''' QST Info data.
	''' </summary>
	Private m_TabQSTInfoData As TabQSTInfoData

	''' <summary>
	''' Settings xml.
	''' </summary>
	Private m_MandantSettingsXml As SettingsXml

	''' <summary>
	''' Tax info service URL.
	''' </summary>
	Private m_TaxInfoServiceUrl As String

	Private m_countryAndLocation As EmployeePropertyViewData
	Private m_birthdate As EmployeePropertyViewData
	Private m_gender As EmployeePropertyViewData
	Private m_permission As EmployeePropertyViewData
	Private m_civilState As EmployeePropertyViewData
	Private m_table As EmployeePropertyViewData
	Private m_children As EmployeePropertyViewData
	Private m_qstBase As EmployeePropertyViewData
	Private m_sicknessDayMoney As EmployeePropertyViewData
	Private m_suvaDayMoney As EmployeePropertyViewData
	Private m_sExceptions As EmployeePropertyViewData
	Private m_OtherServicesAmounts As EmployeePropertyViewData
	Private m_OtherNotDefinedAmounts As EmployeePropertyViewData
	Private m_kiEducation As EmployeePropertyViewData

	Private m_path As ClsProgPath
	Private m_PayrollSetting As String

	Private m_ESBeginEndInMonth As Boolean
	Private m_MinDeducation As Boolean

	Private m_BaseTableData As BaseTable.SPSBaseTables
	Private m_PermissionData As BindingList(Of SP.Internal.Automations.PermissionData)

#End Region


#Region "Constructor"

	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New(ByVal mdnr As Integer, ByVal _setting As SP.Infrastructure.Initialization.InitializeClass, ByVal payrollContextData As PayrollContextData)

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		Try
			m_md = New Mandant
			m_InitializationData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
			m_PrCtx = payrollContextData
			m_PayrollSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_PAYROLL_SETTING, mdnr)
			m_path = New ClsProgPath

			m_BaseTableData = New SPSBaseTables(m_InitializationData)
			m_PermissionData = m_BaseTableData.PerformPermissionDataOverWebService(m_InitializationData.UserData.UserLanguage)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_SuppressUIEvents = True
		InitializeComponent()

		Me.KeyPreview = True
		Dim strStyleName As String = m_md.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If


		m_SuppressUIEvents = False

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

		Dim conStr = m_md.GetSelectedMDData(mdnr).MDDbConn
		m_CommonDatabaseAccess = New DatabaseAccess.Common.CommonDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
		m_PayrollDatabaseAccess = New DatabaseAccess.PayrollMng.PayrollDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
		m_EmployeeDatabaseAccess = New DatabaseAccess.Employee.EmployeeDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)

		m_UtilityUI = New UtilityUI
		m_Utility = New Utility

		' Translate controls.
		TranslateControls()

		Try
			m_MandantSettingsXml = New SettingsXml(m_md.GetSelectedMDDataXMLFilename(mdnr, Now.Year))

			Dim domainName = m_InitializationData.MDData.WebserviceDomain
			m_TaxInfoServiceUrl = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_EMPLOYEE_TAXINFO_WEBSERVCIE_URI)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			'm_TaxInfoServiceUrl = DEFAULT_SPUTNIK_EMPLOYEE_TAXINFO_WEBSERVCIE_URI
		End Try
		m_Logger.LogDebug("entring frmQST")

		Reset()
	End Sub

#End Region

#Region "Public Methods"

	''' <summary>
	''' Loads the form data.
	''' </summary>
	Public Sub LoadData()

		m_Logger.LogDebug("entring frmQST: loaddata")
		m_EmployeeData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(m_PrCtx.MANr, False)
		m_Logger.LogDebug("entring frmQST: loaddata: m_EmployeeData")

		m_EmployeeLOSetting = m_EmployeeDatabaseAccess.LoadEmployeeLOSettings(m_PrCtx.MANr)
		m_Logger.LogDebug("entring frmQST: loaddata: m_EmployeeLOSetting")

		LoadDataInternal()

	End Sub

	Public Sub AcceptFormDat()

		HandleChangeOfAnsatz()
		m_PrCtx.U(50) = Val(Me.txtAnsatz.EditValue)

		m_PrCtx.U(49) = Val(Me.txtQstBasis.EditValue)
		m_PrCtx.EmplPayroll.QSTTarif = Me.txtTarif.EditValue       ' Tarifbezeichnung für Quellensteuer
		m_PrCtx.EmplPayroll.strESData = Me.txtEntryAndExit.EditValue
		m_PrCtx.EmplPayroll.strQSTKanton = Me.txtSteuerKanton.EditValue

		m_PrCtx.DONotShowAgainQSTForm = chkDoNOTShowForm.Checked
		m_PrCtx.EmplPayroll.WriteToProtocol(m_PrCtx.EmplPayroll.Padright("Quellensteuermaske: ", 30, " ") & String.Format("Anzeigen: {0}", m_PrCtx.DONotShowAgainQSTForm))

		m_PrCtx.EmplPayroll.strOriginData &= String.Format("DoNotShowForm: {0}", m_PrCtx.DONotShowAgainQSTForm)
		m_PrCtx.EmplPayroll.strOriginData &= IIf(m_MinDeducation, String.Format("(Achtung: Mindestabzug vom {0:n2} sFr.)", Val(lblMindestabzug.Text)), "")

		Close()

	End Sub

#End Region

#Region "Private Methods"

	''' <summary>
	''' Resets the form.
	''' </summary>
	Private Sub Reset()

		txtSteuerKanton.Properties.ReadOnly = True

		' ---Reset drop downs, grids and lists---
		lblMindestAbzugInfo.Text = String.Empty

		ResetEmployeePersonalDataGrid()
		ResetTaxDataGrid()
		ResetESDataGrid()

		m_ESBeginEndInMonth = False

	End Sub

	''' <summary>
	''' Resets the employee personal detail grid.
	''' </summary>
	Private Sub ResetEmployeePersonalDataGrid()

		gvPersonalien.BorderStyle = BorderStyles.NoBorder
		gvPersonalien.OptionsView.ShowIndicator = False

		gvPersonalien.OptionsView.ShowColumnHeaders = False
		gvPersonalien.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False
		gvPersonalien.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False
		gvPersonalien.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None
		gvPersonalien.OptionsSelection.EnableAppearanceFocusedRow = False
		gvPersonalien.OptionsSelection.EnableAppearanceHideSelection = False
		gvPersonalien.OptionsView.ColumnAutoWidth = False

		' Reset the grid
		gvPersonalien.Columns.Clear()

		Dim columnDescriptionName As New DevExpress.XtraGrid.Columns.GridColumn()
		columnDescriptionName.Caption = "Description"
		columnDescriptionName.Name = "Description"
		columnDescriptionName.FieldName = "Description"
		columnDescriptionName.Visible = True
		columnDescriptionName.Width = 150
		gvPersonalien.Columns.Add(columnDescriptionName)

		Dim columnValue As New DevExpress.XtraGrid.Columns.GridColumn()
		columnValue.Caption = "Value"
		columnValue.Name = "Value"
		columnValue.FieldName = "Value"
		columnValue.Visible = True
		columnValue.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
		columnValue.AppearanceHeader.Options.UseTextOptions = True
		columnValue.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
		columnValue.AppearanceCell.Options.UseTextOptions = True
		columnValue.Width = 120
		gvPersonalien.Columns.Add(columnValue)

	End Sub

	Private Sub ResetTaxDataGrid()

		'gvTaxData.BorderStyle = BorderStyles.NoBorder
		gvTaxData.OptionsView.ShowIndicator = False

		'gvTaxData.OptionsView.ShowColumnHeaders = False
		'gvTaxData.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False
		'gvTaxData.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False
		'gvTaxData.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None
		'gvTaxData.OptionsSelection.EnableAppearanceFocusedRow = False
		'gvTaxData.OptionsSelection.EnableAppearanceHideSelection = False
		'gvTaxData.OptionsView.ColumnAutoWidth = False

		' Reset the grid
		gvTaxData.Columns.Clear()

		Dim columnEinkommen As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEinkommen.Caption = "Einkommen"
		columnEinkommen.Name = "Einkommen"
		columnEinkommen.FieldName = "Einkommen"
		columnEinkommen.Visible = True
		columnEinkommen.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
		columnEinkommen.AppearanceHeader.Options.UseTextOptions = True
		columnEinkommen.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
		columnEinkommen.AppearanceCell.Options.UseTextOptions = True
		columnEinkommen.Width = 150
		gvTaxData.Columns.Add(columnEinkommen)

		Dim columnSteuer_Fr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnSteuer_Fr.Caption = "Steuer_Fr"
		columnSteuer_Fr.Name = "Steuer_Fr"
		columnSteuer_Fr.FieldName = "Steuer_Fr"
		columnSteuer_Fr.Visible = True
		columnSteuer_Fr.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
		columnSteuer_Fr.AppearanceHeader.Options.UseTextOptions = True
		columnSteuer_Fr.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
		columnSteuer_Fr.AppearanceCell.Options.UseTextOptions = True
		columnSteuer_Fr.Width = 120
		gvTaxData.Columns.Add(columnSteuer_Fr)

		Dim columnSteuer_Proz As New DevExpress.XtraGrid.Columns.GridColumn()
		columnSteuer_Proz.Caption = "Steuer_Proz"
		columnSteuer_Proz.Name = "Steuer_Proz"
		columnSteuer_Proz.FieldName = "Steuer_Proz"
		columnSteuer_Proz.Visible = True
		columnSteuer_Proz.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
		columnSteuer_Proz.AppearanceHeader.Options.UseTextOptions = True
		columnSteuer_Proz.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
		columnSteuer_Proz.AppearanceCell.Options.UseTextOptions = True
		columnSteuer_Proz.Width = 120
		gvTaxData.Columns.Add(columnSteuer_Proz)


		grdTaxData.DataSource = Nothing

	End Sub

	''' <summary>
	''' Resets the ES data grid.
	''' </summary>
	Private Sub ResetESDataGrid()

		' Reset the grid
		gvListOfES.Columns.Clear()

		Dim columnESNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnESNr.Caption = m_Translate.GetSafeTranslationValue("ESNr")
		columnESNr.Name = "ESNr"
		columnESNr.FieldName = "ESNr"
		columnESNr.Visible = True
		gvListOfES.Columns.Add(columnESNr)

		Dim columnFirma As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFirma.Caption = m_Translate.GetSafeTranslationValue("Firma")
		columnFirma.Name = "Firma1"
		columnFirma.FieldName = "Firma1"
		columnFirma.Visible = True
		gvListOfES.Columns.Add(columnFirma)

		Dim columnESAB As New DevExpress.XtraGrid.Columns.GridColumn()
		columnESAB.Caption = m_Translate.GetSafeTranslationValue("ES Ab")
		columnESAB.Name = "ES_Ab"
		columnESAB.FieldName = "ES_Ab"
		columnESAB.Visible = True
		gvListOfES.Columns.Add(columnESAB)

		Dim columnESEnde As New DevExpress.XtraGrid.Columns.GridColumn()
		columnESEnde.Caption = m_Translate.GetSafeTranslationValue("ES Ende")
		columnESEnde.Name = "ES_Ende"
		columnESEnde.FieldName = "ES_Ende"
		columnESEnde.Visible = True
		gvListOfES.Columns.Add(columnESEnde)

		grdListOfES.DataSource = Nothing

	End Sub

	''' <summary>
	''' Loads internal data.
	''' </summary>
	Private Sub LoadDataInternal()
		LoadTagGeldBetragForMonthData()
		LoadQstInfoData()
		m_Logger.LogDebug("entring frmQST: LoadDataInternal: LoadQstInfoData")

		GetFormData()
		m_Logger.LogDebug("entring frmQST: LoadDataInternal: GetFormData")

		Dim successWSCall = LoadQSTDataOverWebService()
		m_Logger.LogDebug("entring frmQST: LoadDataInternal: successWSCall")

		m_PrCtx.EmplPayroll.strOriginData &= If(successWSCall, String.Empty, "error") &
							"|QST-Data:" &
							"#Steuerkanton: " & txtSteuerKanton.EditValue &
							"# Tarif:" & txtTarif.EditValue &
							"# QST-Basis: " & txtQstBasis.EditValue &
							"# Ansatz: " & txtAnsatz.EditValue &
							"# Abzug: " & txtAbzug.EditValue &
							"# QstData: " & txtEntryAndExit.EditValue &
							"# AnStd: " & lblMonatsStunden.Text & "# <br>"


	End Sub

	''' <summary>
	''' Gets the form data.
	''' </summary>
	Private Sub GetFormData()

		Dim cTotalTaggeld As Decimal = 0

		Dim cTotalPayed As Decimal       ' Auszahlung von Ferien, Feiertag und 13. Lohn
		Dim cTotalBacked As Decimal      ' Rückstellung von ...

		Dim cMonthStd As Decimal
		Dim bStdDown As Boolean
		Dim bStdDownAtBeginEnd As Boolean
		Dim bStdUp As Boolean
		Dim bJustEndBegin As Boolean
		Dim bDEAsCH As Boolean
		Dim bCalendarDay As Boolean
		Dim bWithFLeistung As Boolean
		Dim bHandleAsAutomation As Boolean

		m_countryAndLocation = New EmployeePropertyViewData With {.Description = m_Translate.GetSafeTranslationValue("Land / Wohnort")}
		m_birthdate = New EmployeePropertyViewData With {.Description = m_Translate.GetSafeTranslationValue("Geburtstag")}
		m_gender = New EmployeePropertyViewData With {.Description = m_Translate.GetSafeTranslationValue("Geschlecht")}
		m_permission = New EmployeePropertyViewData With {.Description = m_Translate.GetSafeTranslationValue("Bewilligung")}
		m_civilState = New EmployeePropertyViewData With {.Description = m_Translate.GetSafeTranslationValue("Zivilstand")}
		m_table = New EmployeePropertyViewData With {.Description = m_Translate.GetSafeTranslationValue("Tabelle")}
		m_children = New EmployeePropertyViewData With {.Description = m_Translate.GetSafeTranslationValue("Kinder")}
		m_qstBase = New EmployeePropertyViewData With {.Description = m_Translate.GetSafeTranslationValue("QST-Basis")}

		m_sExceptions = New EmployeePropertyViewData With {.Description = m_Translate.GetSafeTranslationValue("Auszahlungen Ferien, Feiertag und 13. Lohn (Brutto)")}

		m_kiEducation = New EmployeePropertyViewData With {.Description = m_Translate.GetSafeTranslationValue("Ki.-Ausbildung")}
		m_sicknessDayMoney = New EmployeePropertyViewData With {.Description = m_Translate.GetSafeTranslationValue("Krankentaggeld")}
		m_suvaDayMoney = New EmployeePropertyViewData With {.Description = m_Translate.GetSafeTranslationValue("Suva-Taggeld")}
		m_OtherServicesAmounts = New EmployeePropertyViewData With {.Description = m_Translate.GetSafeTranslationValue("Sonstige Fremdleistungen")}
		m_OtherNotDefinedAmounts = New EmployeePropertyViewData With {.Description = m_Translate.GetSafeTranslationValue("Sonstige Ausnahmen")}

		Dim listOfEmployeePersonalData = New List(Of EmployeePropertyViewData)
		listOfEmployeePersonalData.Add(m_countryAndLocation)
		listOfEmployeePersonalData.Add(m_birthdate)
		listOfEmployeePersonalData.Add(m_gender)
		listOfEmployeePersonalData.Add(m_permission)
		listOfEmployeePersonalData.Add(m_civilState)
		listOfEmployeePersonalData.Add(m_table)
		listOfEmployeePersonalData.Add(m_children)
		listOfEmployeePersonalData.Add(m_qstBase)

		listOfEmployeePersonalData.Add(m_sExceptions)

		listOfEmployeePersonalData.Add(m_kiEducation)
		listOfEmployeePersonalData.Add(m_sicknessDayMoney)
		listOfEmployeePersonalData.Add(m_suvaDayMoney)

		listOfEmployeePersonalData.Add(m_OtherServicesAmounts)
		listOfEmployeePersonalData.Add(m_OtherNotDefinedAmounts)

		grdPersonalien.DataSource = listOfEmployeePersonalData

		lblMANrValue.Text = m_EmployeeData.EmployeeNumber
		lblMANameValue.Text = m_EmployeeData.Lastname + ", " + m_EmployeeData.Firstname
		m_countryAndLocation.Value = String.Format("{0} / {1}", m_EmployeeData.Country, m_EmployeeData.Location)
		m_sicknessDayMoney.Value = String.Empty
		m_birthdate.Value = String.Format("{0:dd.MM.yyyy}", m_EmployeeData.Birthdate)
		m_gender.Value = m_EmployeeData.Gender

		Dim employeePermissionCode = m_EmployeeData.Permission
		If Not String.IsNullOrWhiteSpace(employeePermissionCode) AndAlso Not m_PermissionData Is Nothing AndAlso m_PermissionData.Count > 0 Then
			Dim bewData = m_PermissionData.Where(Function(x) x.Code = employeePermissionCode).FirstOrDefault()
			If Not bewData Is Nothing AndAlso Not String.IsNullOrWhiteSpace(bewData.Translated_Value) Then employeePermissionCode = String.Format("({0}) {1}", bewData.Code, bewData.Translated_Value)
		End If
		m_permission.Value = String.Format("{0}", employeePermissionCode)


		m_permission.Value = "(" & m_EmployeeData.Permission & ") " & m_CommonDatabaseAccess.TranslatePermissionCode(m_EmployeeData.Permission, m_InitializationData.UserData.UserLanguage)
		m_civilState.Value = "(" & m_EmployeeData.CivilStatus & ")"

		lblQSTWaehrung.Text = m_EmployeeLOSetting.Currency
		lblAbzugWaehrung.Text = m_EmployeeLOSetting.Currency

		txtSteuerKanton.Text = m_EmployeeData.S_Canton

		If String.IsNullOrEmpty(m_EmployeeData.ChurchTax) Then
			lblTarifInfo.Text = m_Translate.GetSafeTranslationValue("ohne Kirchensteuer")
		Else
			lblTarifInfo.Text = IIf(m_EmployeeData.ChurchTax = "Y" OrElse m_EmployeeData.ChurchTax = "+", m_Translate.GetSafeTranslationValue("mit Kirchensteuer"), m_Translate.GetSafeTranslationValue("ohne Kirchensteuer"))
		End If
		m_table.Value = String.Format("({0})", m_EmployeeData.Q_Steuer)

		txtTarif.Text = String.Format("{0}{1}{2}", m_EmployeeData.Q_Steuer, Trim(Str(m_EmployeeData.ChildsCount)), m_EmployeeData.ChurchTax)
		m_children.Value = m_EmployeeData.ChildsCount

		If m_PrCtx.S(2) = 0 Then
			stdPanel.Visible = False
		Else
			lblTag.Text = m_PrCtx.S(2)
		End If

		If m_TagGeldBetragForMonth Is Nothing Then
			m_sicknessDayMoney.Value = "0.00"
			m_suvaDayMoney.Value = "0.00"
			m_kiEducation.Value = "0.00"

			m_sExceptions.Value = "0.00"
			m_OtherServicesAmounts.Value = "0.00"
			m_OtherNotDefinedAmounts.Value = "0.00"

		Else
			m_sicknessDayMoney.Value = Format(m_TagGeldBetragForMonth.KTGBetrag, "0.00")
			m_suvaDayMoney.Value = Format(m_TagGeldBetragForMonth.SuvaBetrag, "0.00")
			m_kiEducation.Value = Format(m_TagGeldBetragForMonth.KiAuBetrag, "0.00")

			cTotalPayed = m_TagGeldBetragForMonth.SPayed
			cTotalBacked = m_TagGeldBetragForMonth.SBacked

			cTotalTaggeld = Val((m_TagGeldBetragForMonth.KTGBetrag)) + Val((m_TagGeldBetragForMonth.SuvaBetrag)) + Val((m_TagGeldBetragForMonth.SPayed)) + 0

			m_sExceptions.Value = Format(m_TagGeldBetragForMonth.SPayed, "0.00")
			m_OtherServicesAmounts.Value = m_TagGeldBetragForMonth.OtherServicesAmounts
			m_OtherNotDefinedAmounts.Value = m_TagGeldBetragForMonth.OtherNotDefinedAmounts

		End If

		If m_TabQSTInfoData Is Nothing Then
			bStdUp = True
			bStdDown = False
			bStdDownAtBeginEnd = False
			cMonthStd = 180
			bJustEndBegin = False
			bDEAsCH = (m_EmployeeData.Permission <> "G")
			bCalendarDay = False
			bWithFLeistung = False
			bHandleAsAutomation = True

		Else

			bStdUp = m_TabQSTInfoData.StdUp.GetValueOrDefault(False)
			bStdDown = m_TabQSTInfoData.StdDown.GetValueOrDefault(False)
			bStdDownAtBeginEnd = m_TabQSTInfoData.StdDownAtEndBegin.GetValueOrDefault(False)
			bDEAsCH = m_TabQSTInfoData.DeSameAsCH.GetValueOrDefault(False)
			bJustEndBegin = m_TabQSTInfoData.JustAtEndBegin.GetValueOrDefault(False)
			cMonthStd = m_TabQSTInfoData.MonthStd.GetValueOrDefault(0)
			bWithFLeistung = m_TabQSTInfoData.WithFLeistung.GetValueOrDefault(False)
			bHandleAsAutomation = m_TabQSTInfoData.HandleAsAutomation.GetValueOrDefault(False)

		End If

		Call FillESLvg()
		If Not m_ESBeginEndInMonth Then
			If bJustEndBegin Then bStdUp = False
			If bStdDownAtBeginEnd Then bStdDown = False

		Else
			If bJustEndBegin Then bStdUp = True
			If bStdDownAtBeginEnd Then bStdDown = True

		End If

		If m_EmployeeData.Country = "D" Then
			If bDEAsCH Then
				m_qstBase.Value = m_PrCtx.U(9)
			Else
				m_qstBase.Value = m_PrCtx.U(1)
			End If

		Else
			m_qstBase.Value = m_PrCtx.U(9)

		End If

		m_PrCtx.EmplPayroll.strOriginData &= String.Format("<br><br><b>QST-Daten:</b>")
		m_PrCtx.EmplPayroll.strOriginData &= String.Format("<br>QST-Kanton: {0} ¦ m_PrCtx.S(2): {1:n2} ¦ cMonthStd: {2} ¦ iESLP4QST: {3} ¦ 1. m_qstBase.Value: {4:n2} ¦ m_ESBeginEndInMonth: {5}",
														   m_EmployeeData.S_Canton, m_PrCtx.S(2), cMonthStd, m_PrCtx.EmplPayroll.iESLP4QST, Val(m_qstBase.Value), m_ESBeginEndInMonth)


		'm_qstBase.Value = Format(IIf(m_EmployeeData.Country = "D", IIf(bDEAsCH, m_PrCtx.U(9), m_PrCtx.U(1)), m_PrCtx.U(9)), "0.00")

		Dim exceptionsAmounts As Decimal = IIf(bWithFLeistung, 0, (Val(m_kiEducation.Value) + Val(m_OtherServicesAmounts.Value) + Val(m_OtherNotDefinedAmounts.Value) + cTotalTaggeld))
		m_qstBase.Value = Format(Val(m_qstBase.Value) - exceptionsAmounts, "0.00")  'IIf(bWithFLeistung, 0, (Val(m_kiEducation.Value) + Val(m_OtherServicesAmounts.Value) + Val(m_OtherNotDefinedAmounts.Value) + cTotalTaggeld)), "0.00")

		Dim calculatedValue As Decimal = Val(m_qstBase.Value)
		m_PrCtx.EmplPayroll.strOriginData &= String.Format(" ¦ bStdUp/bStdDown/stdDownAtEndBegin: {0}/{1}/{2} ¦ bWithFLeistung: {3}", bStdUp, bStdDown, bStdDownAtBeginEnd, bWithFLeistung)

		If Val(m_kiEducation.Value) <> 0 Then m_PrCtx.EmplPayroll.strOriginData &= String.Format(" ¦ m_kiEducation: {0:n2}", Val(m_kiEducation.Value))
		If Val(m_OtherServicesAmounts.Value) <> 0 Then m_PrCtx.EmplPayroll.strOriginData &= String.Format(" ¦ m_OtherServicesAmounts (36): {0:n2} >>> ({1}) ", Val(m_OtherServicesAmounts.Value), m_TagGeldBetragForMonth.OtherServicesAmountsLAData)
		If Val(m_OtherNotDefinedAmounts.Value) <> 0 Then m_PrCtx.EmplPayroll.strOriginData &= String.Format(" ¦ m_OtherNotDefinedAmounts: {0:n2} >>> ({1}) ", Val(m_OtherNotDefinedAmounts.Value), m_TagGeldBetragForMonth.OtherNotDefinedAmountsLAData)
		If exceptionsAmounts <> 0 Then m_PrCtx.EmplPayroll.strOriginData &= String.Format(" ¦ Total exceptionsAmounts: {0:n2}", exceptionsAmounts)

		m_PrCtx.EmplPayroll.strOriginData &= String.Format(" ¦ 2. qstBase: {0:n2} ", Val(m_qstBase.Value))

		If m_EmployeeData.S_Canton = "UR" AndAlso bHandleAsAutomation Then
			m_qstBase.Value = GetQSTBasis4_UR()
			calculatedValue = Val(m_qstBase.Value)

		ElseIf m_PrCtx.S(2) <> 0 Then

			lblMonatsStunden.Text = cMonthStd
			If cMonthStd > 0 And cMonthStd <= 31 Then
				' Rechnet er nach Tagen...
				lblMonatsstudenVon.Text = "Monatstage"
				If m_PrCtx.EmplPayroll.iESLP4QST = 0 Then
					lblMonatsStunden.Text = m_PrCtx.EmplPayroll.iESLP4QST
					calculatedValue = Format(Val(m_qstBase.Value), "0.00")
					m_PrCtx.EmplPayroll.strOriginData &= String.Format(" ¦ monthly iESLP4QST: {0}", m_PrCtx.EmplPayroll.iESLP4QST)

				Else

					If m_PrCtx.EmplPayroll.iESLP4QST < cMonthStd AndAlso bStdUp Then
						calculatedValue = Format((Val(m_qstBase.Value) / m_PrCtx.EmplPayroll.iESLP4QST) * cMonthStd, "0.00")
						m_PrCtx.EmplPayroll.strOriginData &= String.Format(" ¦ monhtly calculatedValue, bStdUp: {0:n2} = {1:n2} / {2:n2} * {3:n2}", calculatedValue, Val(m_qstBase.Value), m_PrCtx.EmplPayroll.iESLP4QST, cMonthStd)

					ElseIf m_PrCtx.EmplPayroll.iESLP4QST > cMonthStd AndAlso bStdDown Then
						calculatedValue = Format((Val(m_qstBase.Value) / m_PrCtx.EmplPayroll.iESLP4QST) * cMonthStd, "0.00")
						m_PrCtx.EmplPayroll.strOriginData &= String.Format("monhtly calculatedValue, bStdDown: {0:n2} = {1:n2} / {2:n2} * {3:n2}", calculatedValue, Val(m_qstBase.Value), m_PrCtx.EmplPayroll.iESLP4QST, cMonthStd)

					Else
						lblMonatsStunden.Text = m_PrCtx.EmplPayroll.iESLP4QST
						calculatedValue = Format(Val(m_qstBase.Value), "0.00")

					End If
				End If

			Else
				' Rechnet nach Stunden...
				If m_PrCtx.S(2) = 0 Then
					lblMonatsStunden.Text = m_PrCtx.S(2)
					calculatedValue = Format(Val(m_qstBase.Value), "0.00")
					m_PrCtx.EmplPayroll.strOriginData &= String.Format(" ¦ hourly S(2): {0}", m_PrCtx.S(2))

				Else
					If m_PrCtx.S(2) < cMonthStd AndAlso bStdUp Then
						calculatedValue = Format((Val(m_qstBase.Value) / m_PrCtx.S(2)) * cMonthStd, "0.00")
						m_PrCtx.EmplPayroll.strOriginData &= String.Format(" ¦ hourly calculatedValue, bStdUp: {0:n2} = {1:n2} / {2:n2} * {3:n2}", calculatedValue, Val(m_qstBase.Value), m_PrCtx.S(2), cMonthStd)

					ElseIf m_PrCtx.S(2) > cMonthStd AndAlso bStdDown Then
						calculatedValue = Format((Val(m_qstBase.Value) / m_PrCtx.S(2)) * cMonthStd, "0.00")
						m_PrCtx.EmplPayroll.strOriginData &= String.Format(" ¦ hourly calculatedValue, bStdDown: {0:n2} = {1:n2} / {2:n2} * {3:n2}", calculatedValue, Val(m_qstBase.Value), m_PrCtx.S(2), cMonthStd)

					Else
						lblMonatsStunden.Text = m_PrCtx.S(2)
						calculatedValue = Format(Val(m_qstBase.Value), "0.00")

					End If
				End If
			End If

		End If

		If Not (m_EmployeeData.S_Canton = "UR" AndAlso bHandleAsAutomation) Then
			m_PrCtx.EmplPayroll.strOriginData &= String.Format(" ¦ m_qstBase.Value: {0:n2} ¦ calculatedValue: {1:n2} = ({2:n2} + {3:n2})", Val(m_qstBase.Value), calculatedValue + exceptionsAmounts, calculatedValue, exceptionsAmounts)

			calculatedValue += exceptionsAmounts 'IIf(bWithFLeistung, 0, (Val(m_kiEducation.Value) + Val(m_OtherServicesAmounts.Value) + Val(m_OtherNotDefinedAmounts.Value) + cTotalTaggeld)) 
		Else
			m_PrCtx.EmplPayroll.strOriginData &= String.Format(" ¦ m_qstBase.Value: {0:n2} ¦ calculatedValue: {1:n2}", Val(m_qstBase.Value), calculatedValue)

		End If

		lblMonatsBruttoValue.Text = Format(calculatedValue, "0.00")
		txtQstBasis.Text = Format(calculatedValue, "0.00")

		'If UCase(m_EmployeeData.Q_Steuer) = "G" Then
		'	Dim taxprocentforborderforeigner As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(m_PrCtx.MDnr, m_PrCtx.LPYear), String.Format("{0}/taxprocentforborderforeigner", m_PayrollSetting))
		'	txtAnsatz.Text = Format(Val(taxprocentforborderforeigner), "0.00") ' Format(Val(DivReg.GetINIString(MDIniFullname, LoadResString(377),    LoadResString(573))), "0.00")
		'End If

	End Sub

	Private Function GetQSTBasis4_UR() As Decimal

		Dim dBasis As Decimal

		dBasis = Val(m_PrCtx.U(9))

		'sSql = "Select Sum(LOL.m_Btr) As QST_Basis From LOL Left Join LA On LOL.LANr = LA.LANr And LOL.Jahr = LA.LAJahr "
		'sSql = sSql & "Where LA.QSTpflichtig = 1 And LA.Sum0Anzahl  <> '2' And (LA.Kumulativ = 0 and LA.KumulativMonth = 0) "
		'sSql = sSql & "And LOL.LONr = " & LONewNr & " "

		Dim lolData = m_PayrollDatabaseAccess.LoadLOLDataForQSTCantonUR(m_PrCtx.LONewNr, m_PrCtx.MDnr)


		Dim sMsgText As String
		sMsgText = "Achtung: QST-Kanton = UR (Besonderheiten): "
		If m_PrCtx.EmplPayroll.iESLP4QST > 29 Then GetQSTBasis4_UR = m_PrCtx.U(9) : Exit Function

		If lolData Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Fehler in der Berechnung der 'Satzbestimmendes Einkommen' der Quellensteuer.")
			dBasis = 0
			sMsgText &= "Fehler in der Berechnung der 'Satzbestimmendes Einkommen' der Quellensteuer."

		Else
			If m_PrCtx.S(2) < 180 And m_PrCtx.S(2) <> 0 Then
				sMsgText &= String.Format("Bruttolohn zum Hochrechnung: {1:n2} für geleistet Arbeitszeit: {2:n2}{0}", vbNewLine, Val(lolData.Basis_Brutto), m_PrCtx.S(2))
				sMsgText &= String.Format("Restliche Lohndaten (ohne Arbeitszeit): {1:n2}{0}", vbNewLine, Val(lolData.Basis_Rest))
				sMsgText &= String.Format("Monatliche Kinder- und Ausbildungszulagen: {0:n2} ", m_PrCtx.EmplPayroll.cBetragKIZulage + m_PrCtx.EmplPayroll.cBetragAuZulage)

				dBasis = ((Val(lolData.Basis_Brutto) / m_PrCtx.S(2)) * 180) + Val(lolData.Basis_Rest) + m_PrCtx.EmplPayroll.cBetragKIZulage + m_PrCtx.EmplPayroll.cBetragAuZulage

				m_PrCtx.EmplPayroll.strOriginData &= String.Format(" | QST-Kanton: UR / Basis_Brutto: {0:n2} ", Val(lolData.Basis_Brutto))
				m_PrCtx.EmplPayroll.strOriginData &= String.Format("Basis_Rest: {0:n2} ", Val(lolData.Basis_Rest))
				m_PrCtx.EmplPayroll.strOriginData &= String.Format("KiAu-Zulagen: {0:n2} ", m_PrCtx.EmplPayroll.cBetragKIZulage + m_PrCtx.EmplPayroll.cBetragAuZulage)

			Else
				dBasis = Val(m_qstBase.Value)

			End If

		End If
		m_PrCtx.EmplPayroll.WriteToProtocol(m_PrCtx.EmplPayroll.Padright("GetQSTBasis4_UR: ", 30, " ") & sMsgText)

		GetQSTBasis4_UR = dBasis

	End Function

	Private Sub HandleChangeOfAnsatz()
		Dim cTotalBetrag As Decimal

		m_MinDeducation = False

		Dim cLbl9 As Double
		'cLbl9 = Val(m_qstBase.Value)
		cLbl9 = Val(m_qstBase.Value) + Val(m_sicknessDayMoney.Value) + Val(Me.m_suvaDayMoney.Value) + Val(Me.m_kiEducation.Value)
		If Val(m_qstBase.Value) = 0 AndAlso Val(Me.txtQstBasis.EditValue) > 0 Then
			cLbl9 = Val(Me.txtQstBasis.EditValue)
		End If

		cTotalBetrag = Math.Max(Val(Me.lblMindestAbzugValue.Text), Val(cLbl9) * Val(Me.txtAnsatz.EditValue) / 100)

		'  If Val(Me.txtQst(2).Text) > 0 And Val(cLbl9) > 0 Then
		If Val(cLbl9) > 0 Then
			If cTotalBetrag > (Val(cLbl9) * Val(Me.txtAnsatz.EditValue) / 100) Then
				Me.txtAnsatz.Text = cTotalBetrag / Val(cLbl9) * 100
				m_MinDeducation = True
			End If
		End If
		Me.txtAbzug.Text = m_PrCtx.EmplPayroll.NumberRound(Val(cLbl9) * Val(txtAnsatz.EditValue) / 100, 4)

		'  Me.txtQst(3).Text = NumberRound(Val(Me.txtQst(1).Text) * _ Val(Me.txtQst(2).Text) / 100, 4)

	End Sub

	Private Sub FillESLvg()
		Dim i As Integer
		Dim dStartofMonth As Date
		Dim dEndofMonth As Date
		Dim dESEnde As Date
		Dim cBasis As Decimal

		dStartofMonth = CDate("01." & m_PrCtx.LPMonth & "." & m_PrCtx.LPYear)
		dEndofMonth = CDate(DateAdd("m", 1, dStartofMonth.AddDays(-dStartofMonth.Day + 1))).AddDays(-1)

		Dim esDataList1 = m_PayrollDatabaseAccess.LoadESData1ForQSTDataForm(m_PrCtx.MANr, m_PrCtx.MDnr)
		ThrowExceptionOnError(esDataList1 Is Nothing, "Einsatz(1) konnten nicht geladen werden (frmQst)")

		grdListOfES.DataSource = esDataList1

		Dim esDataList2 = m_PayrollDatabaseAccess.LoadESData2ForQSTDataForm(m_PrCtx.MANr, m_PrCtx.MDnr, dStartofMonth, dEndofMonth)
		ThrowExceptionOnError(esDataList2 Is Nothing, "Einsatz(2) konnten nicht geladen werden (frmQst)")
		i = 0
		cBasis = 0

		Dim currentESRec As ESData2ForQSTDataForm = Nothing

		If esDataList2.Count > 0 Then
			currentESRec = esDataList2(0)
		End If

		If Not currentESRec Is Nothing Then
			m_PrCtx.EmplPayroll.strESData = Format(currentESRec.ES_Ab, "dd.MM.yyyy") & "-"
			Do While Not currentESRec Is Nothing

				cBasis = currentESRec.Grundlohn + cBasis
				If Not currentESRec.ES_Ende.HasValue Then
					' first of month is deactivated!
					If currentESRec.ES_Ab > dStartofMonth Then
						' ist Beginn oder Ende im gleichen Monat
						m_ESBeginEndInMonth = True
					End If

				Else

					If currentESRec.ES_Ab <= dStartofMonth AndAlso currentESRec.ES_Ende >= dEndofMonth Then
						' nix machen

					ElseIf currentESRec.ES_Ab < dEndofMonth OrElse currentESRec.ES_Ende < dEndofMonth Then
						' ist Beginn oder Ende im gleichen Monat
						m_ESBeginEndInMonth = True

					ElseIf currentESRec.ES_Ab > dStartofMonth AndAlso currentESRec.ES_Ende > dEndofMonth Then
						' ist Beginn oder Ende im gleichen Monat
						m_ESBeginEndInMonth = True

					ElseIf currentESRec.ES_Ab < dEndofMonth AndAlso currentESRec.ES_Ende < dEndofMonth Then
						' ist Beginn oder Ende im gleichen Monat
						m_ESBeginEndInMonth = True


					ElseIf currentESRec.ES_Ende < dEndofMonth Then
						m_ESBeginEndInMonth = True

					End If

				End If
				dESEnde = Format(IIf(Not currentESRec.ES_Ende.HasValue, CDate("31.12.2999"), currentESRec.ES_Ende))

				i = i + 1
				If i > esDataList2.Count - 1 Then
					currentESRec = Nothing
				Else
					currentESRec = esDataList2(i)
				End If
			Loop

			m_PrCtx.EmplPayroll.strESData &= IIf(Year(dESEnde) = 2999, "", dESEnde)
		End If
		txtEntryAndExit.Text = m_PrCtx.EmplPayroll.strESData

		If m_PrCtx.EmplPayroll.strESData = "" Then
			Dim msg = "Achtung: Keine aktiven Einsatzdaten vorhanden.{0}"
			msg &= "Möglicherweise haben Sie keine Einsatzdaten im aktuellen Monat.{0}"
			msg &= "Die Einsatzdaten in der Quellensteuerliste werden daher nicht aufgelistet."
			msg = String.Format(msg, vbNewLine)
			m_PrCtx.EmplPayroll.WriteToProtocol(m_PrCtx.EmplPayroll.Padright("Einsatzdaten für Quellensteuer: ", 30, " ") & msg)

			If Not m_PrCtx.DONotShowAgainQSTForm Then
				m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue(msg), m_Translate.GetSafeTranslationValue("Einsatzdaten auflisten"), MessageBoxIcon.Exclamation)
			End If

		End If

	End Sub

	''' <summary>
	''' Loads tax info data over webservice.
	''' </summary>
	Private Function LoadQSTDataOverWebService() As Boolean

		Dim success As Boolean = True

		Try
			lblMindestAbzugInfo.Text = String.Empty

			'Dim ws = New EmployeeTaxInfoWebService.SPEmployeeTaxInfoServiceSoapClient
			Dim ws = New SP.Internal.Automations.EmployeeTaxInfoWebService.SPEmployeeTaxInfoServiceSoapClient

			ws.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_TaxInfoServiceUrl)
			Dim employeeEinkommen As Integer = Int(Val(txtQstBasis.EditValue)) * 100
			Dim gender2021 As String
			If m_PrCtx.LPYear >= 2021 Then
				gender2021 = String.Empty
			Else
				gender2021 = m_EmployeeData.Gender
			End If

			Dim aQstRecords = ws.GetQstData(m_InitializationData.MDData.MDGuid, txtSteuerKanton.EditValue,
																								m_PrCtx.LPYear, employeeEinkommen, m_EmployeeData.ChildsCount,
																								m_EmployeeData.Q_Steuer,
																								m_EmployeeData.ChurchTax,
																								gender2021)

			Dim listDataSource = New BindingList(Of EmployeeTaxInfoWebService.QstDataDTO)
			For Each itm In aQstRecords
				listDataSource.Add(New EmployeeTaxInfoWebService.QstDataDTO With {.Einkommen = itm.Einkommen / 100, .Steuer_Fr = itm.Steuer_Fr / 100, .Steuer_Proz = itm.Steuer_Proz / 100, .Mindest_Abzug = itm.Mindest_Abzug})
			Next
			grdTaxData.DataSource = listDataSource
			grdTaxData.Visible = m_InitializationData.UserData.UserNr = 1


			txtAnsatz.Text = "0.00"
			If (aQstRecords Is Nothing) Then
				HandleWebserviceError()
				success = False
			Else


				For i = 0 To aQstRecords.Count - 1

					'If m_PrCtx.LPYear <= 2020 Then
					lblMindestAbzugValue.Text = Format(Val(aQstRecords(i).Mindest_Abzug), "0.00")
					'End If

					lblQSTBasisInfo.Text = String.Format("({0:n2})", Val(aQstRecords(i).Einkommen) / 100)

					If Val(aQstRecords(i).Steuer_Proz) > 0 Then

						txtAnsatz.Text = Format(aQstRecords(i).Steuer_Proz / 100, "0.0000")
						'lblQSTBasisInfo.Text = String.Format("({0:n2})", Val(aQstRecords(i).Einkommen) / 100)

						'If m_PrCtx.LPYear >= 2021 Then
						'	lblMindestAbzugValue.Text = Format(Val(aQstRecords(i).Mindest_Abzug), "0.00")
						'	'If (Val(aQstRecords(i).Steuer_Proz / 100) * txtQstBasis.EditValue) / 100 < (Val(aQstRecords(i).Steuer_Fr) / 100) Then
						'	'	lblMindestAbzugInfo.Text = String.Format("Basis*Ansatz%: {0:n2} < Steuer_Fr ({1:n2})", (Val(aQstRecords(i).Steuer_Proz / 100) * txtQstBasis.EditValue) / 100, Val(aQstRecords(i).Steuer_Fr) / 100)

						'	'	lblMindestAbzugValue.Text = Format(Val(aQstRecords(i).Steuer_Fr) / 100, "0.00")

						'	'Else
						'	'	lblMindestAbzugValue.Text = "0.00"
						'	'	lblMindestAbzugInfo.Text = String.Empty

						'	'End If

						'End If

						Exit For

					Else
						'If m_PrCtx.LPYear >= 2021 Then
						'	lblMindestAbzugValue.Text = Format(Val(aQstRecords(i).Steuer_Fr) / 100, "0.00")
						'	lblMindestAbzugInfo.Text = String.Format("Steuer_Fr ({0:n2})", Val(aQstRecords(i).Steuer_Fr) / 100)
						'End If

						If Val(aQstRecords(i).Einkommen) + Val(aQstRecords(i).Schritt) > employeeEinkommen Then
							Exit For
						End If

					End If

				Next

				If Val(lblMindestAbzugValue.Text) > 0 Then
					lblMindestAbzugValue.Appearance.ForeColor = Color.Red
					lblMindestabzug.Appearance.ForeColor = Color.Red
					lblMindestAbzugInfo.Appearance.ForeColor = Color.Red

				Else
					lblMindestAbzugValue.Appearance.ForeColor = Color.Black
					lblMindestabzug.Appearance.ForeColor = Color.Black
					lblMindestAbzugInfo.Appearance.ForeColor = Color.Black

				End If
			End If

			HandleChangeOfAnsatz()

			m_PrCtx.EmplPayroll.strOriginData &= String.Format(" | WebserviceCall: {0} | employeeEinkommen: {1} | Year: {2} | ChildsCount: {3} | Q_Steuer: {4} | ChurchTax: {5} | txtAnsatz: {6} |",
																											 txtSteuerKanton.EditValue, employeeEinkommen, m_PrCtx.LPYear,
																												 m_EmployeeData.ChildsCount, m_EmployeeData.Q_Steuer, m_EmployeeData.ChurchTax,
																												 txtAnsatz.EditValue)


		Catch ex As Exception

			m_Logger.LogError(ex.ToString)
			success = False

			HandleWebserviceError()

		End Try

		Return success
	End Function

	Private Sub HandleWebserviceError()
		m_PrCtx.EmplPayroll.WriteToProtocol(m_PrCtx.EmplPayroll.Padright("*** -> LoadQSTDataOverWebService: ", 30, " ") &
			"(-1) Fehler in Quellensteuercode...")

		m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Quellsteneuer Tarife konnten nicht über den Webservice geladen werden.") & vbCrLf &
																m_Translate.GetSafeTranslationValue("Möglicherweise existiert die Tabelle für die") & vbCrLf &
																String.Format(m_Translate.GetSafeTranslationValue("Quellensteuer-Tarife von {0} nicht."), txtSteuerKanton.Text))
	End Sub

	''' <summary>
	''' Loads Tag Geld Betrag for Month
	''' </summary>
	Private Sub LoadTagGeldBetragForMonthData()

		Dim err As Boolean = False

		m_TagGeldBetragForMonth = m_PayrollDatabaseAccess.LoadTagGeldBetragForMonth(m_PrCtx.MANr, m_PrCtx.MDnr, m_PrCtx.LPMonth, m_PrCtx.LPYear, m_EmployeeData.S_Canton, err)
		ThrowExceptionOnError(err, "Taggeldbetrag für Monat konnte nicht geladen werden.")

	End Sub

	''' <summary>
	''' Loads QST info data.
	''' </summary>
	Private Sub LoadQstInfoData()

		Dim err As Boolean = False
		m_TabQSTInfoData = m_PayrollDatabaseAccess.LoadQSTInfo(m_EmployeeData.S_Canton, err)
		ThrowExceptionOnError(err, "Qst Information konnte nicht geladen werden.")

	End Sub

	Private Sub ThrowExceptionOnError(ByVal err As Boolean, ByVal errorText As String)
		If err Then
			Throw New Exception(errorText)
		End If

	End Sub

	Private Function StrToBool(ByVal str As String) As Boolean

		Dim result As Boolean = False

		If String.IsNullOrWhiteSpace(str) Then
			Return False
		End If

		Boolean.TryParse(str, result)

		Return result
	End Function

#End Region

#Region "Helper Methods"

	''' <summary>
	'''  Trannslate controls.
	''' </summary>
	Private Sub TranslateControls()
		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
		Me.lblAngabenQSTAbzug.Text = m_Translate.GetSafeTranslationValue(Me.lblAngabenQSTAbzug.Text)
		Me.lblMANr.Text = m_Translate.GetSafeTranslationValue(Me.lblMANr.Text)
		Me.lblMAName.Text = m_Translate.GetSafeTranslationValue(Me.lblMAName.Text)

		Me.lblMonatsbrutto.Text = m_Translate.GetSafeTranslationValue(Me.lblMonatsbrutto.Text)
		Me.lblBerechnetFuer.Text = m_Translate.GetSafeTranslationValue(Me.lblBerechnetFuer.Text)

		Me.grpPersonalien.Text = m_Translate.GetSafeTranslationValue(Me.grpPersonalien.Text)
		Me.grpLohndaten.Text = m_Translate.GetSafeTranslationValue(Me.grpLohndaten.Text)

		Me.lblSteuerkanton.Text = m_Translate.GetSafeTranslationValue(Me.lblSteuerkanton.Text)
		Me.lblTarif.Text = m_Translate.GetSafeTranslationValue(Me.lblTarif.Text)
		Me.lblQSTBasis.Text = m_Translate.GetSafeTranslationValue(Me.lblQSTBasis.Text)
		Me.lblAnsatz.Text = m_Translate.GetSafeTranslationValue(Me.lblAnsatz.Text)
		Me.lblAbzug.Text = m_Translate.GetSafeTranslationValue(Me.lblAbzug.Text)
		Me.lblMindestabzug.Text = m_Translate.GetSafeTranslationValue(Me.lblMindestabzug.Text)

		Me.btnAdopt.Text = m_Translate.GetSafeTranslationValue(Me.btnAdopt.Text)
		Me.lblListeES.Text = m_Translate.GetSafeTranslationValue(Me.lblListeES.Text)

	End Sub

	''' <summary>
	''' Disables the window close button.
	''' </summary>
	Protected Overloads Overrides ReadOnly Property CreateParams() As CreateParams
		Get
			Dim myCp As CreateParams = MyBase.CreateParams
			myCp.ClassStyle = myCp.ClassStyle Or CP_NOCLOSE_BUTTON
			Return myCp
		End Get
	End Property

#End Region

#Region "Event Handling"

	''' <summary>
	''' Handles change of Ansatz.
	''' </summary>
	Private Sub OnTxtAnsatz_TextChanged(sender As Object, e As EventArgs) Handles txtAnsatz.TextChanged
		HandleChangeOfAnsatz()
	End Sub

	''' <summary>
	''' Adnles click on adopt button.
	''' </summary>
	Private Sub OnBtnAdopt_Click(sender As Object, e As EventArgs) Handles btnAdopt.Click

		HandleChangeOfAnsatz()
		m_PrCtx.U(50) = Val(Me.txtAnsatz.EditValue)

		m_PrCtx.U(49) = Val(Me.txtQstBasis.EditValue)
		m_PrCtx.EmplPayroll.QSTTarif = Me.txtTarif.EditValue       ' Tarifbezeichnung für Quellensteuer
		m_PrCtx.EmplPayroll.strESData = Me.txtEntryAndExit.EditValue
		m_PrCtx.EmplPayroll.strQSTKanton = Me.txtSteuerKanton.EditValue

		m_PrCtx.EmplPayroll.strOriginData &= IIf(m_MinDeducation, String.Format("(Achtung: Mindestabzug vom {0:n2} sFr.)", Val(lblMindestabzug.Text)), "")
		m_PrCtx.DONotShowAgainQSTForm = chkDoNOTShowForm.Checked

		Close()

	End Sub


	''' <summary>
	''' Handles click on txtAnsatz redo button.
	''' </summary>
	Private Sub OntxtAnsatzButtonClick(sender As System.Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtAnsatz.ButtonClick
		If e.Button.Index = 0 Then
			LoadQSTDataOverWebService()
		End If
	End Sub

#End Region

#Region "Helper Classes"

	Public Class PayrollContextData

		Public Property EmplPayroll As EmployeePayroll
		Public Property MANr As Integer
		Public Property MDnr As Integer
		Public Property LPMonth As Integer
		Public Property LPYear As Integer
		Public Property LONewNr As Integer
		Public Property S As Decimal()       ' Summen Variable
		Public Property U As Decimal()       ' Summen Variable
		Public Property DONotShowAgainQSTForm As Boolean

	End Class

	''' <summary>
	''' Employee property view data.
	''' </summary>
	Class EmployeePropertyViewData
		Public Property Description As String
		Public Property Value As String
	End Class

#End Region

End Class