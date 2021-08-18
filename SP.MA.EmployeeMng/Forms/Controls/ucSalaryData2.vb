
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports DevExpress.XtraEditors
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.Employee.DataObjects.Salary
Imports SPProgUtility.Mandanten
Imports System.Threading.Tasks
Imports System.Threading
Imports System.ComponentModel
Imports SPProgUtility.CommonXmlUtility
Imports SP.Internal.Automations.SPALKUtilWebService

Namespace UI

	Public Class ucSalaryData2


#Region "Private Consts"

		Private Const MANDANT_XML_SETTING_SPUTNIK_ALK_UTIL_WEBSERVICE_URI As String = "MD_{0}/Interfaces/webservices/webservicealkdatabase"
		Private Const DEFAULT_SPUTNIK_ALK_UTIL_WEBSERVICE_URI As String = "wssps_services/spalkutil.asmx"
		Private Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"
		Private Const MANDANT_XML_SETTING_SPUTNIK_LICENCING_URI As String = "MD_{0}/Licencing"

#End Region

#Region "Private Fields"

		''' <summary>
		''' The mandant.
		''' </summary>
		Private m_MD As Mandant

		''' <summary>
		''' The prog path.
		''' </summary>
		Private m_Path As SPProgUtility.ProgPath.ClsProgPath

		Private m_MandantSettingsXml As SettingsXml
		Private m_ALKUtilWebServiceUri As String
		Private m_Allowedemployeeweeklypament As Boolean

#End Region


#Region "Constructor"

		Public Sub New()

			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()

			AddHandler lueEmployeeALK.ButtonClick, AddressOf OnDropDown_ButtonClick

			AddHandler lueAHVCode.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler dateEditAHVAnAM.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueALVCode.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueBVGCode.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueSuva2.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler luePaymentMethod.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueCurrencyData.ButtonClick, AddressOf OnDropDown_ButtonClick

			Try
				m_MD = New Mandant
				m_Path = New SPProgUtility.ProgPath.ClsProgPath

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
			End Try

		End Sub

#End Region

#Region "Public Methods"



		''' <summary>
		''' Inits the control with configuration information.
		''' </summary>
		'''<param name="initializationClass">The initialization class.</param>
		'''<param name="translationHelper">The translation helper.</param>
		Public Overrides Sub InitWithConfigurationData(ByVal initializationClass As SP.Infrastructure.Initialization.InitializeClass, ByVal translationHelper As SP.Infrastructure.Initialization.TranslateValuesHelper)
			MyBase.InitWithConfigurationData(initializationClass, translationHelper)

			Try
				m_MandantSettingsXml = New SettingsXml(m_MD.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))
				'm_ALKUtilWebServiceUri = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_ALK_UTIL_WEBSERVICE_URI, m_InitializationData.MDData.MDNr))

				Dim m_licencing_uri As String = String.Format(MANDANT_XML_SETTING_SPUTNIK_LICENCING_URI, m_InitializationData.MDData.MDNr)
				m_Allowedemployeeweeklypament = m_Utility.ParseToBoolean(m_Path.GetXMLNodeValue(m_MD.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year),
																																				String.Format("{0}/allowedemployeeweeklypayment", m_licencing_uri)), False)

				'If String.IsNullOrWhiteSpace(m_ALKUtilWebServiceUri) Then
				'	m_ALKUtilWebServiceUri = DEFAULT_SPUTNIK_ALK_UTIL_WEBSERVICE_URI
				'End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				'm_ALKUtilWebServiceUri = DEFAULT_SPUTNIK_ALK_UTIL_WEBSERVICE_URI
			End Try

			Dim domainName As String = m_InitializationData.MDData.WebserviceDomain ' "http://asmx.domain.com"
			m_ALKUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_ALK_UTIL_WEBSERVICE_URI)

		End Sub



		''' <summary>
		''' Activates the control.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <returns>Boolean value indicating success.</returns>
		Public Overrides Function Activate(ByVal employeeNumber As Integer) As Boolean

			Dim success As Boolean = True

			If (Not IsIntialControlDataLoaded) Then
				success = success AndAlso LoadDropDownData()
				IsIntialControlDataLoaded = True
			End If

			If (Not IsEmployeeDataLoaded OrElse (Not m_EmployeeNumber = employeeNumber)) Then
				success = success AndAlso LoadEmployeeData(employeeNumber)

				m_EmployeeNumber = IIf(success, employeeNumber, 0)
			End If

			m_SuppressUIEvents = False

			Return success
		End Function

		''' <summary>
		''' Deactivates the control.
		''' </summary>
		Public Overrides Sub Deactivate()
			' Do nothing
		End Sub

		''' <summary>
		''' Resets the control.
		''' </summary>
		Public Overrides Sub Reset()

			m_EmployeeNumber = Nothing

			Dim suppressUIEventsState As Boolean = m_SuppressUIEvents
			m_SuppressUIEvents = True

			' Check Prozess
			chkAHVCardSubmited.Checked = False
			txtAHVCardSubmittedText.Text = String.Empty
			txtAHVCardReturnedText.Properties.MaxLength = 50

			chkAHVCardReturned.Checked = False
			txtAHVCardReturnedText.Text = String.Empty
			txtAHVCardReturnedText.Properties.MaxLength = 50


			chkFrameWorkContractSubscribed.Checked = False
			txtFrameWorkContract.Text = String.Empty
			txtFrameWorkContract.Properties.MaxLength = 50

			chkZVSince.Checked = False
			txtZVSince.Text = String.Empty
			txtZVSince.Properties.MaxLength = 50

			txtZVEmail.Text = String.Empty
			txtZVEmail.Properties.MaxLength = 255

			txtZVVersand.Text = String.Empty
			txtZVVersand.Properties.MaxLength = 255

			lueEmployeeALK.EditValue = Nothing

			chkDoNotPrintReports.Checked = False
			'chkSendEmployeeToWeb.Checked = False

			' Einstellungen
			dateEditAHVAnAM.EditValue = Nothing
			chkKI.Checked = False
			chkKTG.Checked = False

			chkWeeklyPayment.Enabled = m_Allowedemployeeweeklypament
			chkWeeklyPayment.Checked = False

			' Sperren
			chkNoLO.Checked = False
			txtNoLOWhy.Text = String.Empty
			txtNoLOWhy.Properties.MaxLength = 50

			chkNoZG.Checked = False
			txtNoZGWhy.Text = String.Empty
			txtNoZGWhy.Properties.MaxLength = 50

			txtMaxNegativeSalary.Text = "0"
			txtMaxNegativeSalary.Properties.MaxLength = 15

			' Rückstellung
			chkFerienBack.Checked = False
			chkFeiertagBack.Checked = False
			chkLohn13Back.Checked = False
			chkEnableGleitzeit.Checked = False

			'  Reset drop downs and lists
			ResetALKNumberDropDown()
			ResetAHVCodeDropDown()
			ResetALVCodeDropDown()
			ResetBVGCodeDropDown()
			ResetSuva2DropDown()
			ResetPaymentMethodDropDown()
			ResetCurrencyDropDown()

			Dim userSec131 As Boolean = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 131, m_InitializationData.MDData.MDNr)
			chkFrameWorkContractSubscribed.Enabled = userSec131
			txtFrameWorkContract.Enabled = userSec131

			grpRuekstellungen.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 113, m_InitializationData.MDData.MDNr)
			grpSperren.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 141, m_InitializationData.MDData.MDNr)

			Dim userSec119 = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 119, m_InitializationData.MDData.MDNr)
			lueAHVCode.Enabled = userSec119
			dateEditAHVAnAM.Enabled = userSec119
			lueALVCode.Enabled = userSec119
			chkKI.Enabled = userSec119
			lueBVGCode.Enabled = userSec119
			chkKTG.Enabled = userSec119

			m_SuppressUIEvents = suppressUIEventsState

			errorProvider.Clear()
		End Sub

		Private Sub ResetALKNumberDropDown()

			lueEmployeeALK.Properties.DisplayMember = "ALKName"
			lueEmployeeALK.Properties.ValueMember = "ALKNumber"

			gvEmployeeALK.OptionsView.ShowIndicator = False
			gvEmployeeALK.OptionsView.ShowColumnHeaders = True
			gvEmployeeALK.OptionsView.ShowFooter = False

			gvEmployeeALK.OptionsView.ShowAutoFilterRow = True
			gvEmployeeALK.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvEmployeeALK.Columns.Clear()

			Dim columnALKNumber As New DevExpress.XtraGrid.Columns.GridColumn()
			columnALKNumber.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnALKNumber.Name = "ALKNumber"
			columnALKNumber.FieldName = "ALKNumber"
			columnALKNumber.Visible = True
			columnALKNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvEmployeeALK.Columns.Add(columnALKNumber)

			Dim columnALKName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnALKName.Caption = m_Translate.GetSafeTranslationValue("Name")
			columnALKName.Name = "ALKName"
			columnALKName.FieldName = "ALKName"
			columnALKName.Visible = True
			columnALKName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvEmployeeALK.Columns.Add(columnALKName)

			Dim columnALKPOBox As New DevExpress.XtraGrid.Columns.GridColumn()
			columnALKPOBox.Caption = m_Translate.GetSafeTranslationValue("Postfach")
			columnALKPOBox.Name = "POBox"
			columnALKPOBox.FieldName = "POBox"
			columnALKPOBox.Visible = True
			columnALKPOBox.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvEmployeeALK.Columns.Add(columnALKPOBox)

			Dim columnALKStreet As New DevExpress.XtraGrid.Columns.GridColumn()
			columnALKStreet.Caption = m_Translate.GetSafeTranslationValue("Strasse")
			columnALKStreet.Name = "Street"
			columnALKStreet.FieldName = "Street"
			columnALKStreet.Visible = True
			columnALKStreet.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvEmployeeALK.Columns.Add(columnALKStreet)

			Dim columnALKLocation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnALKLocation.Caption = m_Translate.GetSafeTranslationValue("Ort")
			columnALKLocation.Name = "Location"
			columnALKLocation.FieldName = "Location"
			columnALKLocation.Visible = True
			gvEmployeeALK.Columns.Add(columnALKLocation)

			Dim columnALKTelephone As New DevExpress.XtraGrid.Columns.GridColumn()
			columnALKTelephone.Caption = m_Translate.GetSafeTranslationValue("Telefon")
			columnALKTelephone.Name = "Telephone"
			columnALKTelephone.FieldName = "Telephone"
			columnALKTelephone.Visible = False
			gvEmployeeALK.Columns.Add(columnALKTelephone)

			Dim columnALKTelefax As New DevExpress.XtraGrid.Columns.GridColumn()
			columnALKTelefax.Caption = m_Translate.GetSafeTranslationValue("Telefax")
			columnALKTelefax.Name = "Telefax"
			columnALKTelefax.FieldName = "Telefax"
			columnALKTelefax.Visible = False
			gvEmployeeALK.Columns.Add(columnALKTelefax)

			Dim columnALKEMail As New DevExpress.XtraGrid.Columns.GridColumn()
			columnALKEMail.Caption = m_Translate.GetSafeTranslationValue("E-Mail")
			columnALKEMail.Name = "EMail"
			columnALKEMail.FieldName = "EMail"
			columnALKEMail.Visible = False
			gvEmployeeALK.Columns.Add(columnALKEMail)

			lueEmployeeALK.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueEmployeeALK.Properties.NullText = String.Empty
			lueEmployeeALK.EditValue = Nothing


		End Sub

		''' <summary>
		''' Validated data.
		''' </summary>
		Public Overrides Function ValidateData() As Boolean

			Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")

			Dim isValid As Boolean = True

			isValid = isValid AndAlso SetErrorIfInvalid(luePaymentMethod, errorProvider, luePaymentMethod.EditValue Is Nothing, errorText)

			Return isValid

		End Function

		''' <summary>
		''' Merges the employee master data.
		''' </summary>
		''' <param name="employeeMasterData">The employee master data object where the data gets filled into.</param>
		''' <param name="forceMerge">Optional flag indicating if the merge should be forced altough no data has been loaded. </param>
		Public Overrides Sub MergeEmployeeMasterData(ByVal employeeMasterData As EmployeeMasterData, Optional forceMerge As Boolean = False)
			If ((IsEmployeeDataLoaded AndAlso
					m_EmployeeNumber = employeeMasterData.EmployeeNumber) OrElse forceMerge) Then

			End If
		End Sub

		''' <summary>
		'''  Merges the employee contact other data (MASonstiges).
		''' </summary>
		''' <param name="employeeOtherData">The employee other data.</param>
		Public Overrides Sub MergeEmployeeOtherData(ByVal employeeOtherData As EmployeeOtherData)
			If (IsEmployeeDataLoaded AndAlso m_EmployeeNumber = employeeOtherData.EmployeeNumber) Then


			End If
		End Sub

		''' <summary>
		'''  Merges the employee contact comm data.
		''' </summary>
		''' <param name="employeeContactCommData">The employee contact comm data.</param>
		Public Overrides Sub MergeEmployeeContactCommData(ByVal employeeContactCommData As EmployeeContactComm)
			If (IsEmployeeDataLoaded AndAlso m_EmployeeNumber = employeeContactCommData.EmployeeNumber) Then

				Try

					employeeContactCommData.GetAHVKarte = chkAHVCardSubmited.Checked
					employeeContactCommData.GetAHVKarteBez = txtAHVCardSubmittedText.Text

					employeeContactCommData.AHVKarteBacked = chkAHVCardReturned.Checked
					employeeContactCommData.AHVKateBackedBez = txtAHVCardReturnedText.Text

					employeeContactCommData.RahmenArbeit = chkFrameWorkContractSubscribed.Checked
					employeeContactCommData.RahemArbeitBez = txtZVSince.Text
					employeeContactCommData.InZV = chkZVSince.Checked
					employeeContactCommData.InZVBez = txtFrameWorkContract.Text

					employeeContactCommData.ZVeMail = txtZVEmail.Text
					employeeContactCommData.ZVVersand = txtZVVersand.Text

					employeeContactCommData.ALKNumber = lueEmployeeALK.EditValue

					Dim employeeALK = SelectedRecord
					If Not lueEmployeeALK.EditValue Is Nothing AndAlso Not employeeALK Is Nothing Then

						employeeContactCommData.ALKNumber = employeeALK.ALKNumber
						employeeContactCommData.ALKName = employeeALK.ALKName
						employeeContactCommData.ALKPOBox = employeeALK.POBox
						employeeContactCommData.ALKStreet = employeeALK.Street
						employeeContactCommData.ALKPostcode = employeeALK.Postcode
						employeeContactCommData.ALKLocation = employeeALK.Location
						employeeContactCommData.ALKTelephone = employeeALK.Telephone
						employeeContactCommData.ALKTelefax = employeeALK.Telefax

					Else
						employeeContactCommData.ALKNumber = Nothing
						employeeContactCommData.ALKName = Nothing
						employeeContactCommData.ALKPOBox = Nothing
						employeeContactCommData.ALKStreet = Nothing
						employeeContactCommData.ALKPostcode = Nothing
						employeeContactCommData.ALKLocation = Nothing
						employeeContactCommData.ALKTelephone = Nothing
						employeeContactCommData.ALKTelefax = Nothing

					End If

				Catch ex As Exception
					m_Logger.LogError(String.Format("m_EmployeeNumber: {0} | {1}", m_EmployeeNumber, ex.ToString))

				End Try

			End If

		End Sub

		''' <summary>
		'''  Merges the employee LOSetting data data (MA_LOSetting).
		''' </summary>
		''' <param name="employeeLOSettings">The employee LOSetting data.</param>
		Public Overrides Sub MergeEmployeeLOSettingsData(ByVal employeeLOSettings As EmployeeLOSettingsData)
			If (IsEmployeeDataLoaded AndAlso m_EmployeeNumber = employeeLOSettings.EmployeeNumber) Then

				employeeLOSettings.NoRPPrint = chkDoNotPrintReports.Checked
				employeeLOSettings.PayrollSendAsZip = chkSendAsZIP.Checked

				employeeLOSettings.AHVCode = lueAHVCode.EditValue
				employeeLOSettings.AHVAnAm = dateEditAHVAnAM.EditValue
				employeeLOSettings.ALVCode = lueALVCode.EditValue
				employeeLOSettings.KI = chkKI.Checked
				employeeLOSettings.BVGCode = lueBVGCode.EditValue
				employeeLOSettings.SecSuvaCode = lueSuva2.EditValue
				employeeLOSettings.KTGPflicht = chkKTG.EditValue
				employeeLOSettings.WeeklyPayment = chkWeeklyPayment.EditValue
				employeeLOSettings.Zahlart = luePaymentMethod.EditValue
				employeeLOSettings.Currency = lueCurrencyData.EditValue

				employeeLOSettings.NoLO = chkNoLO.Checked
				employeeLOSettings.NoLOWhy = txtNoLOWhy.Text
				employeeLOSettings.NoZG = chkNoZG.Checked
				employeeLOSettings.NoZGWhy = txtNoZGWhy.Text

				If Not String.IsNullOrEmpty(txtMaxNegativeSalary.Text) Then
					employeeLOSettings.Max_NegativSalary = Decimal.Parse(txtMaxNegativeSalary.Text)
				Else
					employeeLOSettings.Max_NegativSalary = Nothing
				End If

				employeeLOSettings.FerienBack = chkFerienBack.Checked
				employeeLOSettings.FeierBack = chkFeiertagBack.Checked
				employeeLOSettings.Lohn13Back = chkLohn13Back.Checked
				employeeLOSettings.MAGleitzeit = chkEnableGleitzeit.Checked

			End If
		End Sub

		''' <summary>
		''' Cleanup control.
		''' </summary>
		Public Overrides Sub CleanUp()
			' Do nothing
		End Sub

		Public Sub ZahlarHasChanged(ByVal employeeNumber As Integer)

			If (m_EmployeeNumber = employeeNumber) Then
				LoadPaymentMethodDropDownData()

				Dim suppressUIEventsState As Boolean = m_SuppressUIEvents
				m_SuppressUIEvents = True

				Dim employeeLOSettingsData As EmployeeLOSettingsData = m_EmployeeDataAccess.LoadEmployeeLOSettings(employeeNumber)

				If (employeeLOSettingsData Is Nothing) Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mitarbeiter-Lohneinststellungsdaten konnten nicht geladen werden."))
					Return
				End If

				luePaymentMethod.EditValue = employeeLOSettingsData.Zahlart

				m_SuppressUIEvents = suppressUIEventsState

			Else
				' Ignore this event because the employee number does not match with the requested employee number.
			End If

		End Sub


#End Region

#Region "Private Methods"

		''' <summary>
		'''  Translate controls.
		''' </summary>
		Protected Overrides Sub TranslateControls()

			' Check-Prozess
			Me.grpCheckProzess.Text = m_Translate.GetSafeTranslationValue(Me.grpCheckProzess.Text)
			Me.chkAHVCardSubmited.Text = m_Translate.GetSafeTranslationValue(Me.chkAHVCardSubmited.Text)
			Me.chkAHVCardReturned.Text = m_Translate.GetSafeTranslationValue(Me.chkAHVCardReturned.Text)
			Me.chkFrameWorkContractSubscribed.Text = m_Translate.GetSafeTranslationValue(Me.chkFrameWorkContractSubscribed.Text)
			Me.chkZVSince.Text = m_Translate.GetSafeTranslationValue(Me.chkZVSince.Text)
			Me.lblEmailForZv.Text = m_Translate.GetSafeTranslationValue(Me.lblEmailForZv.Text)
			Me.lblVersandZV.Text = m_Translate.GetSafeTranslationValue(Me.lblVersandZV.Text)
			Me.chkDoNotPrintReports.Text = m_Translate.GetSafeTranslationValue(Me.chkDoNotPrintReports.Text)
			Me.chkSendAsZIP.Text = m_Translate.GetSafeTranslationValue(Me.chkSendAsZIP.Text)

			' Einstellungen
			Me.grpEinstellungen.Text = m_Translate.GetSafeTranslationValue(Me.grpEinstellungen.Text)
			Me.lblAHVCode.Text = m_Translate.GetSafeTranslationValue(Me.lblAHVCode.Text)
			Me.lblAnmeldung.Text = m_Translate.GetSafeTranslationValue(Me.lblAnmeldung.Text)
			Me.lblALVCode.Text = m_Translate.GetSafeTranslationValue(Me.lblALVCode.Text)
			Me.chkKI.Text = m_Translate.GetSafeTranslationValue(Me.chkKI.Text)
			Me.lblBVGCode.Text = m_Translate.GetSafeTranslationValue(Me.lblBVGCode.Text)
			Me.chkKTG.Text = m_Translate.GetSafeTranslationValue(Me.chkKTG.Text)
			Me.chkWeeklyPayment.Text = m_Translate.GetSafeTranslationValue(Me.chkWeeklyPayment.Text)
			Me.lblSuva2Code.Text = m_Translate.GetSafeTranslationValue(Me.lblSuva2Code.Text)
			Me.lblZahlart.Text = m_Translate.GetSafeTranslationValue(Me.lblZahlart.Text)
			Me.lblWaehrung.Text = m_Translate.GetSafeTranslationValue(Me.lblWaehrung.Text)

			' Sperren
			Me.grpSperren.Text = m_Translate.GetSafeTranslationValue(Me.grpSperren.Text)
			Me.chkNoLO.Text = m_Translate.GetSafeTranslationValue(Me.chkNoLO.Text)
			Me.chkNoZG.Text = m_Translate.GetSafeTranslationValue(Me.chkNoZG.Text)
			Me.lblUnterschreibungslimite.Text = m_Translate.GetSafeTranslationValue(lblUnterschreibungslimite.Text)

			' Rückstellungen
			Me.grpRuekstellungen.Text = m_Translate.GetSafeTranslationValue(Me.grpRuekstellungen.Text)
			Me.chkFerienBack.Text = m_Translate.GetSafeTranslationValue(Me.chkFerienBack.Text)
			Me.chkFeiertagBack.Text = m_Translate.GetSafeTranslationValue(Me.chkFeiertagBack.Text)
			Me.chkLohn13Back.Text = m_Translate.GetSafeTranslationValue(Me.chkLohn13Back.Text)
			Me.chkEnableGleitzeit.Text = m_Translate.GetSafeTranslationValue(Me.chkEnableGleitzeit.Text)

		End Sub

		''' <summary>
		''' Resets the AHV code drop down.
		''' </summary>
		Private Sub ResetAHVCodeDropDown()

			lueAHVCode.Properties.DisplayMember = "TranslatedAHVText"
			lueAHVCode.Properties.ValueMember = "AHVCodeStr"

			Dim columns = lueAHVCode.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("AHVCodeStr", 0, m_Translate.GetSafeTranslationValue("Code")))
			columns.Add(New LookUpColumnInfo("TranslatedAHVText", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

			lueAHVCode.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueAHVCode.Properties.SearchMode = SearchMode.AutoComplete
			lueAHVCode.Properties.AutoSearchColumnIndex = 0
			lueAHVCode.Properties.NullText = String.Empty
			lueAHVCode.EditValue = Nothing
		End Sub

		''' <summary>
		''' Resets the ALV code drop down.
		''' </summary>
		Private Sub ResetALVCodeDropDown()

			lueALVCode.Properties.DisplayMember = "TranslatedALVText"
			lueALVCode.Properties.ValueMember = "ALVCodeStr"

			Dim columns = lueALVCode.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("ALVCodeStr", 0, m_Translate.GetSafeTranslationValue("Code")))
			columns.Add(New LookUpColumnInfo("TranslatedALVText", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

			lueALVCode.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueALVCode.Properties.SearchMode = SearchMode.AutoComplete
			lueALVCode.Properties.AutoSearchColumnIndex = 0
			lueALVCode.Properties.NullText = String.Empty
			lueALVCode.EditValue = Nothing
		End Sub

		''' <summary>
		''' Resets the BVG code drop down.
		''' </summary>
		Private Sub ResetBVGCodeDropDown()

			lueBVGCode.Properties.DisplayMember = "TranslatedBVGText"
			lueBVGCode.Properties.ValueMember = "GetField"

			Dim columns = lueBVGCode.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("GetField", 0, m_Translate.GetSafeTranslationValue("Code")))
			columns.Add(New LookUpColumnInfo("TranslatedBVGText", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

			lueBVGCode.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueBVGCode.Properties.SearchMode = SearchMode.AutoComplete
			lueBVGCode.Properties.AutoSearchColumnIndex = 0
			lueBVGCode.Properties.NullText = String.Empty
			lueBVGCode.EditValue = Nothing
			lueBVGCode.Tag = Nothing ' Tag value stores previous value when user checks KI checkbox.
		End Sub

		''' <summary>
		''' Resets the Suva2 drop down.
		''' </summary>
		Private Sub ResetSuva2DropDown()

			lueSuva2.Properties.DisplayMember = "TranslatedSuva2Text"
			lueSuva2.Properties.ValueMember = "GetField"

			Dim columns = lueSuva2.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("GetField", 0, m_Translate.GetSafeTranslationValue("Code")))
			columns.Add(New LookUpColumnInfo("TranslatedSuva2Text", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

			lueSuva2.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueSuva2.Properties.SearchMode = SearchMode.AutoComplete
			lueSuva2.Properties.AutoSearchColumnIndex = 0
			lueSuva2.Properties.NullText = String.Empty
			lueSuva2.EditValue = Nothing
		End Sub

		''' <summary>
		''' Resets the payment method drop down.
		''' </summary>
		Private Sub ResetPaymentMethodDropDown()

			luePaymentMethod.Properties.DisplayMember = "TranslatedPaymentMethod"
			luePaymentMethod.Properties.ValueMember = "RecValue"

			Dim columns = luePaymentMethod.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("RecValue", 0, m_Translate.GetSafeTranslationValue("Code")))
			columns.Add(New LookUpColumnInfo("TranslatedPaymentMethod", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

			luePaymentMethod.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			luePaymentMethod.Properties.SearchMode = SearchMode.AutoComplete
			luePaymentMethod.Properties.AutoSearchColumnIndex = 0
			luePaymentMethod.Properties.NullText = String.Empty
			luePaymentMethod.EditValue = Nothing
		End Sub

		''' <summary>
		''' Resets the currency drop down.
		''' </summary>
		Private Sub ResetCurrencyDropDown()

			lueCurrencyData.Properties.DisplayMember = "TranslatedCurrencyText"
			lueCurrencyData.Properties.ValueMember = "RecValue"

			Dim columns = lueCurrencyData.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("RecValue", 0, m_Translate.GetSafeTranslationValue("Code")))
			columns.Add(New LookUpColumnInfo("TranslatedCurrencyText", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

			lueCurrencyData.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueCurrencyData.Properties.SearchMode = SearchMode.AutoComplete
			lueCurrencyData.Properties.AutoSearchColumnIndex = 0
			lueCurrencyData.Properties.NullText = String.Empty
			lueCurrencyData.EditValue = Nothing
		End Sub

		''' <summary>
		''' Loads the drop down data.
		''' </summary>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadDropDownData() As Boolean
			Dim success As Boolean = True

			LoadEmployeeDropDownData()

			success = success AndAlso LoadAHVCodeDropDownData()
			success = success AndAlso LoadALVCodeDropDownData()
			success = success AndAlso LoadBVGCodeDropDownData()
			success = success AndAlso LoadSuva2DropDownData()
			success = success AndAlso LoadPaymentMethodDropDownData()
			success = success AndAlso LoadCurrencyDropDownData()

			Return success
		End Function

		Private Function LoadEmployeeDropDownData() As Boolean

			SearchViaWebService()

			Return True
		End Function

		Private Sub SearchViaWebService()

			Dim uiSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext()

			Task(Of BindingList(Of ALKSearchViewData)).Factory.StartNew(Function() PerformWebserviceCallAsync(),
																							CancellationToken.None,
																							TaskCreationOptions.None,
																							TaskScheduler.Default).ContinueWith(Sub(t) FinishWebserviceCallTask(t), CancellationToken.None, TaskContinuationOptions.None, uiSynchronizationContext)

		End Sub

		Private Function PerformWebserviceCallAsync() As BindingList(Of ALKSearchViewData)

			Dim listDataSource As BindingList(Of ALKSearchViewData) = New BindingList(Of ALKSearchViewData)

			Dim webservice As New Internal.Automations.SPALKUtilWebService.SPALKUtilSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ALKUtilWebServiceUri)

			' Read data over webservice
			Dim searchResult As ALKResultDTO() = webservice.GetALKData()

			For Each result In searchResult

				Dim viewData = New ALKSearchViewData With {
					.ALKNumber = result.ALKNumber,
					.ALKName = result.ALKName,
					.POBox = result.POBox,
					.Street = result.Street,
					.Postcode = result.Postcode,
					.Location = result.Location,
					.Telephone = result.Telephone,
					.Telefax = result.Telefax,
					.EMail = result.EMail
				}

				listDataSource.Add(viewData)

			Next

			Return listDataSource

		End Function

		''' <summary>
		''' Finish web service call.
		''' </summary>
		Private Sub FinishWebserviceCallTask(ByVal t As Task(Of BindingList(Of ALKSearchViewData)))

			Select Case t.Status
				Case TaskStatus.RanToCompletion
					' Webservice call was successful.
					m_SuppressUIEvents = True
					lueEmployeeALK.Properties.DataSource = t.Result
					m_SuppressUIEvents = False

				Case TaskStatus.Faulted
					' Something went wrong -> log error.
					m_Logger.LogError(t.Exception.ToString())
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("ALK-Daten konnten nicht geladen werden."))

				Case Else
					' Do nothing
			End Select

		End Sub



		''' <summary>
		''' Loads the AHV code drop down data.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadAHVCodeDropDownData() As Boolean
			Dim ahvData = m_EmployeeDataAccess.LoadAHVData()

			If (ahvData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("AHV Code-Daten konnten nicht geladen werden."))
			End If

			lueAHVCode.Properties.DataSource = ahvData
			lueAHVCode.Properties.ForceInitialize()

			Return Not ahvData Is Nothing
		End Function

		''' <summary>
		''' Loads the ALV code drop down data.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadALVCodeDropDownData() As Boolean
			Dim alvCodeData = m_EmployeeDataAccess.LoadALVData()

			If (alvCodeData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("ALV Code-Daten konnten nicht geladen werden."))
			End If

			lueALVCode.Properties.DataSource = alvCodeData
			lueALVCode.Properties.ForceInitialize()

			Return Not alvCodeData Is Nothing
		End Function

		''' <summary>
		''' Loads the BVG code drop down data.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadBVGCodeDropDownData() As Boolean
			Dim bvgCodeData = m_EmployeeDataAccess.LoadBVGData()

			If (bvgCodeData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("BVG Code-Daten konnten nicht geladen werden."))
			End If

			lueBVGCode.Properties.DataSource = bvgCodeData
			lueBVGCode.Properties.ForceInitialize()

			Return Not bvgCodeData Is Nothing
		End Function

		''' <summary>
		''' Loads the Suva2 code drop down data.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadSuva2DropDownData() As Boolean
			Dim suva2Data = m_EmployeeDataAccess.LoadSuva2Data()

			If (suva2Data Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Suva2-Daten konnten nicht geladen werden."))
			End If

			lueSuva2.Properties.DataSource = suva2Data
			lueSuva2.Properties.ForceInitialize()

			Return Not suva2Data Is Nothing
		End Function

		''' <summary>
		''' Loads the payment method drop down data.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadPaymentMethodDropDownData() As Boolean
			Dim paymentMethod = m_EmployeeDataAccess.LoadPaymentMethodData()

			If (paymentMethod Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Zahlartdaten konnten nicht geladen werden."))
			End If

			luePaymentMethod.Properties.DataSource = paymentMethod
			luePaymentMethod.Properties.ForceInitialize()

			Return Not paymentMethod Is Nothing
		End Function

		''' <summary>
		''' Loads the currency drop down data.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadCurrencyDropDownData() As Boolean
			Dim currencyData = m_EmployeeDataAccess.LoadCurrenyData()

			If (currencyData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Währungsdaten-Daten konnten nicht geladen werden."))
			End If

			lueCurrencyData.Properties.DataSource = currencyData
			lueCurrencyData.Properties.ForceInitialize()

			Return Not currencyData Is Nothing
		End Function

		''' <summary>
		'''  Loads responsible person data.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadEmployeeData(ByVal employeeNumber As Integer) As Boolean

			Dim success As Boolean = True

			success = LoadEmployeeMasterData(employeeNumber)
			success = success AndAlso LoadEmployeeContactCommData(employeeNumber)
			success = success AndAlso LoadEmployeeLOSettingsData(employeeNumber)

			errorProvider.Clear()

			Return success
		End Function

		''' <summary>
		'''  Loads the employee master data..
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		Private Function LoadEmployeeMasterData(ByVal employeeNumber As Integer) As Boolean

			Dim suppressUIEventsState As Boolean = m_SuppressUIEvents
			m_SuppressUIEvents = True

			Dim employeeMasterData = m_EmployeeDataAccess.LoadEmployeeMasterData(employeeNumber, False)

			If (employeeMasterData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Lohnangaben(2) konnten nicht geladen werden."))
				Return False
			End If

			m_SuppressUIEvents = suppressUIEventsState

			Return True

		End Function

		''' <summary>
		''' Loads the employee contact comm data.
		''' </summary>
		''' <param name="enmployeeNumber">The employee number.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadEmployeeContactCommData(ByVal enmployeeNumber As Integer) As Boolean

			Dim suppressUIEventsState As Boolean = m_SuppressUIEvents
			m_SuppressUIEvents = True

			Dim employeeContactCommData As EmployeeContactComm = m_EmployeeDataAccess.LoadEmployeeContactCommData(enmployeeNumber)

			If (employeeContactCommData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mitarbeiter-Kontaktdaten konnten nicht geladen werden."))
				Return False
			End If

			chkAHVCardSubmited.Checked = employeeContactCommData.GetAHVKarte.HasValue AndAlso employeeContactCommData.GetAHVKarte = True
			txtAHVCardSubmittedText.Text = employeeContactCommData.GetAHVKarteBez
			chkAHVCardReturned.Checked = employeeContactCommData.AHVKarteBacked.HasValue AndAlso employeeContactCommData.AHVKarteBacked = True
			txtAHVCardReturnedText.Text = employeeContactCommData.AHVKateBackedBez

			chkFrameWorkContractSubscribed.Checked = employeeContactCommData.RahmenArbeit.HasValue AndAlso employeeContactCommData.RahmenArbeit = True
			txtFrameWorkContract.Text = employeeContactCommData.InZVBez

			chkZVSince.Checked = employeeContactCommData.InZV.HasValue AndAlso employeeContactCommData.InZV = True
			txtZVSince.Text = employeeContactCommData.RahemArbeitBez

			lueEmployeeALK.EditValue = employeeContactCommData.ALKNumber

			' this must be here after setting fields with alkNumber!
			txtZVEmail.Text = employeeContactCommData.ZVeMail
			txtZVVersand.Text = employeeContactCommData.ZVVersand

			lblZVStreet.Text = employeeContactCommData.ALKStreet
			lblZVLocation.Text = String.Format("{0} {1}", employeeContactCommData.ALKPostcode, employeeContactCommData.ALKLocation)
			'chkSendEmployeeToWeb.Checked = employeeContactCommData.WebExport.HasValue AndAlso employeeContactCommData.WebExport = True

			m_SuppressUIEvents = suppressUIEventsState

			Return True
		End Function

		''' <summary>
		''' Loads the employee LO settings data.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadEmployeeLOSettingsData(ByVal employeeNumber As Integer) As Boolean

			Dim suppressUIEventsState As Boolean = m_SuppressUIEvents
			m_SuppressUIEvents = True

			Dim employeeLOSettingsData As EmployeeLOSettingsData = m_EmployeeDataAccess.LoadEmployeeLOSettings(employeeNumber)

			If (employeeLOSettingsData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mitarbeiter-Lohneinststellungsdaten konnten nicht geladen werden."))
				Return False
			End If

			chkDoNotPrintReports.Checked = employeeLOSettingsData.NoRPPrint.HasValue AndAlso employeeLOSettingsData.NoRPPrint = True
			chkSendAsZIP.Checked = employeeLOSettingsData.PayrollSendAsZip.GetValueOrDefault(False)

			lueAHVCode.EditValue = employeeLOSettingsData.AHVCode
			dateEditAHVAnAM.EditValue = employeeLOSettingsData.AHVAnAm
			lueALVCode.EditValue = employeeLOSettingsData.ALVCode
			chkKI.Checked = employeeLOSettingsData.KI.HasValue AndAlso employeeLOSettingsData.KI = True
			lueBVGCode.EditValue = employeeLOSettingsData.BVGCode
			lueBVGCode.Tag = Nothing ' Tag value stores previous value when user checks KI checkbox.

			lueSuva2.EditValue = employeeLOSettingsData.SecSuvaCode
			chkKTG.EditValue = employeeLOSettingsData.KTGPflicht
			chkWeeklyPayment.EditValue = employeeLOSettingsData.WeeklyPayment
			luePaymentMethod.EditValue = employeeLOSettingsData.Zahlart
			lueCurrencyData.EditValue = employeeLOSettingsData.Currency

			chkNoLO.Checked = employeeLOSettingsData.NoLO.HasValue AndAlso employeeLOSettingsData.NoLO = True
			txtNoLOWhy.Text = employeeLOSettingsData.NoLOWhy
			chkNoZG.Checked = employeeLOSettingsData.NoZG.HasValue AndAlso employeeLOSettingsData.NoZG = True
			txtNoZGWhy.Text = employeeLOSettingsData.NoZGWhy
			txtMaxNegativeSalary.Text = If(employeeLOSettingsData.Max_NegativSalary Is Nothing, 0, employeeLOSettingsData.Max_NegativSalary)

			chkFerienBack.Checked = employeeLOSettingsData.FerienBack.HasValue AndAlso employeeLOSettingsData.FerienBack = True
			chkFeiertagBack.Checked = employeeLOSettingsData.FeierBack.HasValue AndAlso employeeLOSettingsData.FeierBack = True
			chkLohn13Back.Checked = employeeLOSettingsData.Lohn13Back.HasValue AndAlso employeeLOSettingsData.Lohn13Back = True
			chkEnableGleitzeit.Checked = employeeLOSettingsData.MAGleitzeit.HasValue AndAlso employeeLOSettingsData.MAGleitzeit = True

			m_SuppressUIEvents = suppressUIEventsState

			Return True
		End Function

		''' <summary>
		''' Handles change of KI check box.
		''' </summary>
		Private Sub OnChkKI_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkKI.CheckedChanged

			If m_SuppressUIEvents Then
				Return
			End If

			If chkKI.Checked Then
				Dim employeebvgcodewithchild As String = m_Path.GetXMLNodeValue(m_MD.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr),
																																				String.Format("{0}/employeebvgcodewithchild", FORM_XML_MAIN_KEY))

				' Remember current value in tag property. This allows to set the value back if user unchecks the chkKI checkbox.
				lueBVGCode.Tag = lueBVGCode.EditValue
				lueBVGCode.EditValue = employeebvgcodewithchild
			Else

				' The user has unchecked the chkKI checkbox. 
				' If a the previous value is stored in the llueBVGCode.Tag property then set the bgvcode back to this value.
				If Not lueBVGCode.Tag Is Nothing Then
					lueBVGCode.EditValue = lueBVGCode.Tag
					lueBVGCode.Tag = Nothing
				End If

			End If
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
					Dim grdlookupEdit As GridLookUpEdit = CType(sender, GridLookUpEdit)
					grdlookupEdit.EditValue = Nothing

				ElseIf TypeOf sender Is DateEdit Then
					Dim dateEdit As DateEdit = CType(sender, DateEdit)
					dateEdit.EditValue = Nothing
				End If
			End If
		End Sub

#End Region


		Class ALKSearchViewData

			Public Property ALKNumber As Integer?
			Public Property ALKName As String
			Public Property POBox As String
			Public Property Street As String
			Public Property Postcode As Integer?
			Public Property Location As String
			Public Property Telephone As String
			Public Property Telefax As String
			Public Property EMail As String

			Public ReadOnly Property PostcodeAndLocation As String
				Get
					Return String.Format("{0} {1}", If(Not Postcode.HasValue, 0, Postcode), Location)
				End Get
			End Property

		End Class


		Public ReadOnly Property SelectedRecord As ALKSearchViewData
			Get
				'Dim gvRP = TryCast(gvEmployeeALK, DevExpress.XtraGrid.Views.Grid.GridView)

				Dim gvRP = CType(lueEmployeeALK.GetSelectedDataRow(), ALKSearchViewData)

				'If Not (gvRP Is Nothing) Then

				'	Dim selectedRows = gvRP.GetSelectedRows()

				'	If (selectedRows.Count > 0) Then
				'		Dim alkEmployee = CType(gvRP.GetRow(selectedRows(0)), ALKSearchViewData)
				'		Return alkEmployee
				'	End If

				'End If

				Return gvRP
			End Get

		End Property

		Private Sub lueEmployeeALK_EditValueChanged(sender As Object, e As EventArgs) Handles lueEmployeeALK.EditValueChanged

			If Not lueEmployeeALK.EditValue Is Nothing Then

				Dim alkEmployee = SelectedRecord

				If Not alkEmployee Is Nothing Then
					DisplayEmployeeALKData(alkEmployee)
				Else
					ResetEmployeeDetailData()
				End If

			Else
				ResetEmployeeDetailData()

			End If

		End Sub

		Private Function DisplayEmployeeALKData(ByVal alkEmployee As ALKSearchViewData) As Boolean

			' EMail
			txtZVEmail.Text = alkEmployee.EMail
			' Street
			lblZVStreet.Text = String.Format("{0}", alkEmployee.Street)
			' PLZ und Ort
			lblZVLocation.Text = String.Format("{0}", alkEmployee.PostcodeAndLocation)
			'lblZVLocation.Text = String.Format("{1}", alkEmployee.Telephone)
			'lblZVLocation.Text = String.Format("{1}", alkEmployee.Telefax)


			Return True

		End Function

		Private Sub ResetEmployeeDetailData()

			lblZVStreet.Text = String.Empty
			lblZVLocation.Text = String.Empty

		End Sub


	End Class

End Namespace