
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports DevExpress.XtraEditors
Imports SP.DatabaseAccess.Common.DataObjects
Imports SPProgUtility.CommonXmlUtility
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath
Imports SP.Internal.Automations.BaseTable
Imports System.ComponentModel
Imports SP.Internal.Automations
Imports DevExpress.Utils
Imports DevExpress.XtraBars.ToastNotifications
Imports DevExpress.XtraEditors.Popup
Imports DevExpress.Utils.Win
Imports DevExpress.XtraEditors.Repository
Imports System.Reflection

Namespace UI

	Public Class ucSalaryData

#Region "Private Consts"

		Private Const Code_NoQST As String = "0"
		Private Const PERMISSION_CODE_S = ""
		Private Const COUNTRY_CODE_CH = "CH"

		'Private Const MANDANT_XML_SETTING_SPUTNIK_EMPLOYEE_TAXINFO_WEBSERVICE_URI As String = "MD_{0}/Interfaces/webservices/webservicetaxinfoservices"
		'Private Const DEFAULT_SPUTNIK_EMPLOYEE_TAXINFO_WEBSERVCIE_URI = "wsSPS_services/SPEmployeeTaxInfoService.asmx" ' "http://asmx.domain.com/wsSPS_services/SPEmployeeTaxInfoService.asmx"
		Private Const FORM_XML_REQUIREDFIEKDS_KEY As String = "Forms_Normaly/requiredfields"

#End Region

#Region "Private Fields"

		Private m_TaxDataHelper As TaxDataHelper
		Private m_QSTTranslationData As IEnumerable(Of SP.Internal.Automations.TaxCodeData)
		Private m_ChurchTaxCodeTranslationData As IEnumerable(Of SP.Internal.Automations.TaxChurchCodeData)
		Private m_SalaryDataHelperFunctions As SalaryDataHelperFunctions

		''' <summary>
		''' The cls prog path.
		''' </summary>
		Private m_ProgPath As ClsProgPath

		''' <summary>
		''' The mandant.
		''' </summary>
		Private m_MandantData As Mandant

		''' <summary>
		''' Settings xml.
		''' </summary>
		Private m_MandantSettingsXml As SettingsXml

		''' <summary>
		''' Tax info service URL.
		''' </summary>
		Private m_TaxInfoServiceUrl As String
		Private m_BaseTableUtil As SPSBaseTables


		Private m_UserSec117 As Boolean
		Private m_UserSec118 As Boolean
		Private m_UserSec125 As Boolean

		Private m_PartnershipID As Integer?
		Private m_PartnershipBadge As DevExpress.Utils.VisualEffects.Badge
		Private m_BackupBadge As DevExpress.Utils.VisualEffects.Badge


#End Region


#Region "Constructor"

		Public Sub New()

			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()

			Try
				m_ProgPath = New ClsProgPath
				m_MandantData = New Mandant
				'm_BaseTableUtil = New SPSBaseTables(m_InitializationData)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			End Try

			m_SalaryDataHelperFunctions = New SalaryDataHelperFunctions()

			AddHandler lueTaxCanton.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler dateEditAnsQSTBis.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueCode.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueChurchTax.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueNumberOfChildren.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueCommunity.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler luePermission.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueForeignCategory.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler dateEditPermissionTo.ButtonClick, AddressOf OnDropDown_ButtonClick

			AddHandler lueEmploymentType.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueOtherEmploymentType.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueTypeofStay.ButtonClick, AddressOf OnDropDown_ButtonClick

			AddHandler lueExistingEmployee.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueGender.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler luePostcode.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueCountry.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler dateEditValidFrom.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler dateEditValidTo.ButtonClick, AddressOf OnDropDown_ButtonClick


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

			Dim domainName As String = "http://asmx.domain.com"
#If DEBUG Then
			domainName = "http://localhost"
#End If

			Try
				m_BaseTableUtil = New SPSBaseTables(m_InitializationData)
				m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

		End Sub


		''' <summary>
		''' Activates the control.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <returns>Boolean value indicating success.</returns>
		Public Overrides Function Activate(ByVal employeeNumber As Integer) As Boolean

			Dim success As Boolean = True

			If (Not IsIntialControlDataLoaded) Then
				success = success AndAlso LoadQSTAndChurchTaxCodeTranslationData()
				success = success AndAlso LoadDropDownData()
				IsIntialControlDataLoaded = True
			End If

			If (Not IsEmployeeDataLoaded OrElse (Not m_EmployeeNumber = employeeNumber)) Then
				success = success AndAlso LoadEmployeeData(employeeNumber)
				m_EmployeeNumber = IIf(success, employeeNumber, 0)

				success = success AndAlso LoadExistingEmployeeDropDownData(employeeNumber)
				success = success AndAlso LoadPartnershipData(employeeNumber)

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

			tnpTax.SelectedPage = tnpQSTInfo
			chkCertificateForResidenceReceived.Checked = False
			dateEditAnsQSTBis.EditValue = Nothing
			dateEditPermissionTo.EditValue = Nothing

			txtBirthPlace.Text = String.Empty
			txtBirthPlace.Properties.MaxLength = 100
			chkCHPartner.Checked = False
			chkNoSpecialTax.Checked = False
			tgsDoValidate.EditValue = True
			txtZemisNumber.Text = String.Empty

			'  Reset drop downs and lists
			ResetPermissionDropDown()
			ResetForeignCategoryDropDown()

			ResetTaxCantonDropDown()
			ResetChurchTaxDropDown()
			ResetNumberOfChildrenDropDown()
			ResetCodeDropDown()
			ResetCommunityDropDown()
			ResetEmploymentTypeDropDown()
			ResetOtherEmploymentTypeDropDown()

			ResetTypeofStayDropDown()
			ResetPartnershipGrid()
			ResetBackupBeforeEQuestHistoryGrid()

			ResetEmployeeDropDown()
			ResetGenderDropDown()
			ResetPostcodeDropDown()
			ResetCountryDropDown()

			grpBewilligung.Visible = True
			Me.Enabled = True


			m_UserSec117 = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 117, m_InitializationData.MDData.MDNr)
			m_UserSec118 = m_UserSec117 AndAlso IsUserActionAllowed(m_InitializationData.UserData.UserNr, 118, m_InitializationData.MDData.MDNr)
			If Not m_UserSec118 AndAlso m_UserSec117 Then m_UserSec117 = False
			m_UserSec125 = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 125, m_InitializationData.MDData.MDNr)
			tnpSecurity.PageVisible = m_UserSec125

			lueCode.Enabled = m_UserSec117
			lueChurchTax.Enabled = m_UserSec117
			lueNumberOfChildren.Enabled = m_UserSec117

			luePermission.Enabled = m_UserSec118
			'dateEditPermissionTo.Enabled = m_UserSec118
			chkCHPartner.Enabled = m_UserSec118


			m_SuppressUIEvents = suppressUIEventsState

			errorProvider.Clear()
		End Sub

		''' <summary>
		''' Validated data.
		''' </summary>
		Public Overrides Function ValidateData() As Boolean

			errorProvider.Clear()

			Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")

			Dim isValid As Boolean = True

			'isValid = isValid AndAlso SetErrorIfInvalid(lueCode, errorProvider, lueCode.EditValue Is Nothing, errorText)


			Dim mdNr = m_InitializationData.MDData.MDNr
			Dim mandantFormXMLFilename = m_InitializationData.MDData.MDFormXMLFileName

			Dim mustPermissionBeSelected As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(mandantFormXMLFilename, String.Format("{0}/emplyoeepermitselection", FORM_XML_REQUIREDFIEKDS_KEY)), False)
			Dim mustPermissionToDateBeSelected As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(mandantFormXMLFilename, String.Format("{0}/emplyoeepermitdateselection", FORM_XML_REQUIREDFIEKDS_KEY)), False)
			Dim mustBirthPlaceBeSelected As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(mandantFormXMLFilename, String.Format("{0}/emplyoeehometownselection", FORM_XML_REQUIREDFIEKDS_KEY)), False)
			Dim mustCurchTaxBeSelected As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(mandantFormXMLFilename, String.Format("{0}/emplyoeekirchensteuerselection", FORM_XML_REQUIREDFIEKDS_KEY)), False)
			Dim mustSCantonBeSelected As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(mandantFormXMLFilename, String.Format("{0}/emplyoeesteuercantonselection", FORM_XML_REQUIREDFIEKDS_KEY)), False)

			' Check permission
			If mustPermissionBeSelected AndAlso m_UserSec118 Then
				Dim nationalityCode As String = m_UCMediator.GetNationalityFromUi(m_EmployeeNumber)
				isValid = isValid And SetErrorIfInvalid(luePermission, errorProvider, ((luePermission.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(luePermission.EditValue)) AndAlso nationalityCode <> "CH"), errorText)
			End If

			Dim countryCode As String = m_UCMediator.GetCountryCodeFromUi(m_EmployeeNumber)
			Dim requiredTAX As Boolean = True
			countryCode = countryCode.ToUpper

			If countryCode = "LI" Then
				requiredTAX = False

			ElseIf countryCode = "CH" Then
				If Not ((luePermission.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(luePermission.EditValue)) OrElse luePermission.EditValue = "C" OrElse luePermission.EditValue = "13") Then
					requiredTAX = Not chkCHPartner.EditValue
				Else
					requiredTAX = False
				End If

			ElseIf countryCode = "FR" Then
				If chkCertificateForResidenceReceived.EditValue Then requiredTAX = False

			Else
				requiredTAX = True
			End If
			If requiredTAX Then
				If Not tgsDoValidate.EditValue Then requiredTAX = False
			End If

			mustCurchTaxBeSelected = requiredTAX
			mustSCantonBeSelected = requiredTAX

			If requiredTAX AndAlso m_UserSec117 Then
				isValid = isValid AndAlso SetErrorIfInvalid(lueCode, errorProvider, (lueCode.EditValue Is Nothing OrElse lueCode.EditValue = "0"), errorText)
			End If



			If (luePermission.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(luePermission.EditValue) OrElse luePermission.EditValue = "S") Then
				mustPermissionToDateBeSelected = False
				mustBirthPlaceBeSelected = False
			End If

			' Check permissionTo date
			If mustPermissionToDateBeSelected Then 'AndAlso m_UserSec118 Then
				isValid = isValid AndAlso SetErrorIfInvalid(dateEditPermissionTo, errorProvider, dateEditPermissionTo.EditValue Is Nothing, errorText)
			End If

			' Check birthplace
			If mustBirthPlaceBeSelected AndAlso m_UserSec118 Then
				isValid = isValid AndAlso SetErrorIfInvalid(txtBirthPlace, errorProvider, String.IsNullOrWhiteSpace(txtBirthPlace.Text), errorText)
			End If

			' Check church tax
			If Not lueChurchTax.Properties.DataSource Is Nothing AndAlso m_UserSec117 Then

				Dim listOfChurchTaxCodes = CType(lueChurchTax.Properties.DataSource, List(Of ChurchViewData))

				isValid = isValid AndAlso SetErrorIfInvalid(lueChurchTax, errorProvider, lueChurchTax.EditValue Is Nothing AndAlso listOfChurchTaxCodes.Count > 0, errorText)
			End If

			' Check S_Canton (Steuerkanton)
			If mustSCantonBeSelected Then
				isValid = isValid AndAlso SetErrorIfInvalid(lueTaxCanton, errorProvider, lueTaxCanton.EditValue Is Nothing, errorText)
			End If


			Return isValid

		End Function

		''' <summary>
		''' Merges the employee master data.
		''' </summary>
		''' <param name="employeeMasterData">The employee master data object where the data gets filled into.</param>
		''' <param name="forceMerge">Optional flag indicating if the merge should be forced altough no data has been loaded. </param>
		Public Overrides Sub MergeEmployeeMasterData(ByVal employeeMasterData As EmployeeMasterData, Optional forceMerge As Boolean = False)
			Dim expenddt As Date
			Dim dateFormatstring As String = "dd.MM.yyyy"
			Dim msg As String

			Try
				If ((IsEmployeeDataLoaded AndAlso m_EmployeeNumber = employeeMasterData.EmployeeNumber) OrElse forceMerge) Then
					employeeMasterData.S_Canton = lueTaxCanton.EditValue
					employeeMasterData.Residence = chkCertificateForResidenceReceived.Checked
					employeeMasterData.ANS_OST_Bis = dateEditAnsQSTBis.EditValue

					If m_UserSec117 Then
						employeeMasterData.Q_Steuer = lueCode.EditValue
						employeeMasterData.ChurchTax = lueChurchTax.EditValue
						employeeMasterData.ChildsCount = lueNumberOfChildren.EditValue
					End If

					employeeMasterData.QSTCommunity = lueCommunity.EditValue

					If m_UserSec117 Then
						employeeMasterData.Permission = luePermission.EditValue
						If Not Date.TryParseExact(dateEditPermissionTo.EditValue, dateFormatstring, System.Globalization.DateTimeFormatInfo.InvariantInfo, Globalization.DateTimeStyles.None, expenddt) Then
							employeeMasterData.PermissionToDate = Nothing
							msg = m_Translate.GetSafeTranslationValue("Achtung: Das Datum für Gültigkeit der Bewilligung ist ungültig und wird entfernt. Bitte ändern Sie das Datum.<br>{0}")
							If Not dateEditPermissionTo.EditValue Is Nothing Then m_UtilityUI.ShowErrorDialog(String.Format(msg, dateEditPermissionTo.EditValue), m_Translate.GetSafeTranslationValue("Falsche Format"))
						Else
							employeeMasterData.PermissionToDate = expenddt
						End If
					End If

					employeeMasterData.BirthPlace = txtBirthPlace.Text
					If m_UserSec118 Then employeeMasterData.CHPartner = chkCHPartner.EditValue

					employeeMasterData.ValidatePermissionWithTax = tgsDoValidate.EditValue
					employeeMasterData.NoSpecialTax = chkNoSpecialTax.EditValue
					employeeMasterData.ForeignCategory = lueForeignCategory.EditValue
					employeeMasterData.ZEMISNumber = txtZemisNumber.EditValue
					employeeMasterData.EmploymentType = lueEmploymentType.EditValue
					employeeMasterData.OtherEmploymentType = lueOtherEmploymentType.EditValue
					employeeMasterData.TypeOfStay = lueTypeofStay.EditValue

					If Not lueCommunity.EditValue Is Nothing Then
						Dim qstCommunities = CType(lueCommunity.Properties.DataSource, BindingList(Of CommunityData))
						Dim commData = qstCommunities.Where(Function(data) data.BFSNumber = lueCommunity.EditValue).FirstOrDefault

						employeeMasterData.TaxCommunityCode = commData.BFSNumber
						employeeMasterData.TaxCommunityLabel = commData.Translated_Value
					Else
						employeeMasterData.TaxCommunityCode = Nothing
						employeeMasterData.TaxCommunityLabel = Nothing

					End If

					If (employeeMasterData.CivilStatus = "V" OrElse employeeMasterData.CivilStatus = "P") AndAlso (Not lueCode.EditValue Is Nothing AndAlso lueCode.EditValue <> "0") Then
						Dim employeePartnerData = m_EmployeeDataAccess.LoadEmployeePartnershipData(m_EmployeeNumber)
						If employeePartnerData Is Nothing OrElse employeePartnerData.Count = 0 Then
							msg = "<b>Achtung:</b><br>Sie müssen die Daten für Partnerschaft eingeben.<br>Diese Informationen sind für die Quellensteuerlisten und Übermittlung der Daten <b>pflichtig</b>!<br><br>Quellensteuer und Bewilligung > Quellensteuer > Partner"
							Dim caption As String = "<b>Fehlende Partner-Daten</b>"
							m_UtilityUI.ShowInfoDialog(String.Format(m_Translate.GetSafeTranslationValue(msg)), m_Translate.GetSafeTranslationValue(caption), MessageBoxIcon.Hand)

							Return
						End If

						If lueCode.EditValue = "A" Then
							msg = m_Translate.GetSafeTranslationValue("<b>Achtung:</b><br>Anscheinend der eingegebene Tarif passt nicht mit Zivilstand des Kandidaten.<br>Zivilstand: {0} >>> Tarif: {1}")
							m_UtilityUI.ShowErrorDialog(String.Format(msg, employeeMasterData.CivilStatus, lueCode.Text))

							Return
						Else

							For Each rec In employeePartnerData

								If lueCode.EditValue = "C" Then
									If Not rec.InEmployment.GetValueOrDefault(False) Then
										msg = m_Translate.GetSafeTranslationValue("<b>Achtung:</b><br>Sie haben keine Erwärbstige Partnerdaten eingegeben. Bitte überprüfen Sie Partnerdaten.<br>{0} {1}")
										m_UtilityUI.ShowErrorDialog(String.Format(msg, rec.Firstname, rec.Lastname))
									End If

								ElseIf lueCode.EditValue = "B" Then
									If rec.InEmployment.GetValueOrDefault(False) Then
										msg = m_Translate.GetSafeTranslationValue("<b>Achtung:</b><br>Sie haben Erwärbstige Partnerdaten eingegeben. Bitte überprüfen Sie Partnerdaten.<br>{0} {1}")
										m_UtilityUI.ShowErrorDialog(String.Format(msg, rec.Firstname, rec.Lastname))
									End If

								End If

							Next
						End If

					End If
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("m_EmployeeNumber: {0} | {1}", m_EmployeeNumber, ex.ToString))

			End Try

		End Sub

		''' <summary>
		''' Cleanup control.
		''' </summary>
		Public Overrides Sub CleanUp()
			' Do nothing
		End Sub

		''' <summary>
		''' Handle change of country code.
		''' </summary>
		''' <param name="employeeNumber">The emplyoee number.</param>
		Public Sub CountryCodeHasChanged(ByVal employeeNumber As Integer)

			If (m_EmployeeNumber = employeeNumber) Then

				Dim selectedNationality = m_UCMediator.GetNationalityFromUi(m_EmployeeNumber)
				Dim selectedCountry = m_UCMediator.GetCountryCodeFromUi(m_EmployeeNumber)

				LoadPermissionDropDownData(selectedNationality, selectedCountry, luePermission.EditValue)
				ReLoadQSTCodeChurchTaxCodeAndNumberChildrenFromUIControlValues()
				ValidateCHNationatlityFields(selectedNationality, selectedCountry)

			Else
				' Ignore this event because the employee number does not match with the requested employee number.
			End If

		End Sub

		''' <summary>
		''' Handle change of nationality.
		''' </summary>
		''' <param name="employeeNumber">The emplyoee number.</param>
		Public Sub NationalityHasChanged(ByVal employeeNumber As Integer)

			If (m_EmployeeNumber = employeeNumber) Then

				Dim selectedNationality = m_UCMediator.GetNationalityFromUi(m_EmployeeNumber)
				Dim selectedCountry = m_UCMediator.GetCountryCodeFromUi(m_EmployeeNumber)

				LoadPermissionDropDownData(selectedNationality, selectedCountry, luePermission.EditValue)
				ReLoadQSTCodeChurchTaxCodeAndNumberChildrenFromUIControlValues()

				ValidateCHNationatlityFields(selectedNationality, selectedCountry)
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

			' Group Bewilligung
			grpBewilligung.Text = m_Translate.GetSafeTranslationValue(grpBewilligung.Text)
			lblBewilligung.Text = m_Translate.GetSafeTranslationValue(lblBewilligung.Text)
			lblBis.Text = m_Translate.GetSafeTranslationValue(lblBis.Text)
			lblHeimatort.Text = m_Translate.GetSafeTranslationValue(lblHeimatort.Text)
			chkCHPartner.Text = m_Translate.GetSafeTranslationValue(chkCHPartner.Text)

			lblValidateQstPermission.Text = m_Translate.GetSafeTranslationValue(lblValidateQstPermission.Text)
			tgsDoValidate.Properties.OnText = m_Translate.GetSafeTranslationValue(tgsDoValidate.Properties.OnText)
			tgsDoValidate.Properties.OffText = m_Translate.GetSafeTranslationValue(tgsDoValidate.Properties.OffText)

			chkNoSpecialTax.Text = m_Translate.GetSafeTranslationValue(chkNoSpecialTax.Text)
			lblKategorie.Text = m_Translate.GetSafeTranslationValue(lblKategorie.Text)
			lblZemisNummer.Text = m_Translate.GetSafeTranslationValue(lblZemisNummer.Text)
			lblPayrollRelevantBackupInfo.Text = m_Translate.GetSafeTranslationValue(lblPayrollRelevantBackupInfo.Text)

			' Quellensteuer
			tnpQSTInfo.PageText = m_Translate.GetSafeTranslationValue(tnpQSTInfo.PageText)
			tnpBorderCrossers.PageText = m_Translate.GetSafeTranslationValue(tnpBorderCrossers.PageText)
			tnpPartnership.PageText = m_Translate.GetSafeTranslationValue(tnpPartnership.PageText)
			grpQST.Text = m_Translate.GetSafeTranslationValue(grpQST.Text)
			lblKantonFuerQuellensteuer.Text = m_Translate.GetSafeTranslationValue(lblKantonFuerQuellensteuer.Text)
			chkCertificateForResidenceReceived.Text = m_Translate.GetSafeTranslationValue(chkCertificateForResidenceReceived.Text)
			lblBescheinigungGueltigBis.Text = m_Translate.GetSafeTranslationValue(lblBescheinigungGueltigBis.Text)
			lblCode.Text = m_Translate.GetSafeTranslationValue(lblCode.Text)
			lblKirchensteuer.Text = m_Translate.GetSafeTranslationValue(lblKirchensteuer.Text)
			lblAnzahlKinder.Text = m_Translate.GetSafeTranslationValue(lblAnzahlKinder.Text)
			lblGemeinde.Text = m_Translate.GetSafeTranslationValue(lblGemeinde.Text)
			lblBeschaeftigungsart.Text = m_Translate.GetSafeTranslationValue(lblBeschaeftigungsart.Text)
			lblWeitereBeschaeftigung.Text = m_Translate.GetSafeTranslationValue(lblWeitereBeschaeftigung.Text)
			lblAufenthaltsart.Text = m_Translate.GetSafeTranslationValue(lblAufenthaltsart.Text)

			lblBestehendeKandidat.Text = m_Translate.GetSafeTranslationValue(lblBestehendeKandidat.Text)
			lblGeschlecht.Text = m_Translate.GetSafeTranslationValue(lblGeschlecht.Text)
			lblNachname.Text = m_Translate.GetSafeTranslationValue(lblNachname.Text)
			lblVorname.Text = m_Translate.GetSafeTranslationValue(lblVorname.Text)
			lblstrasse.Text = m_Translate.GetSafeTranslationValue(lblstrasse.Text)
			lblland.Text = m_Translate.GetSafeTranslationValue(lblland.Text)
			lblplz.Text = m_Translate.GetSafeTranslationValue(lblplz.Text)
			lblort.Text = m_Translate.GetSafeTranslationValue(lblort.Text)
			lblPartnerSozialversicherungsnummer.Text = m_Translate.GetSafeTranslationValue(lblPartnerSozialversicherungsnummer.Text)
			lblGeburtsdatum.Text = m_Translate.GetSafeTranslationValue(lblGeburtsdatum.Text)
			lblPartnerGueltig.Text = m_Translate.GetSafeTranslationValue(lblPartnerGueltig.Text)
			chkInEmployment.Text = m_Translate.GetSafeTranslationValue(chkInEmployment.Text)

			tnpQSTInfo.Caption = m_Translate.GetSafeTranslationValue(tnpQSTInfo.Caption)
			tnpBorderCrossers.Caption = m_Translate.GetSafeTranslationValue(tnpBorderCrossers.Caption)
			tnpPartnership.Caption = m_Translate.GetSafeTranslationValue(tnpPartnership.Caption)
			tnpSecurity.Caption = m_Translate.GetSafeTranslationValue(tnpSecurity.Caption)


		End Sub

		''' <summary>
		''' Resets the tax canton drop down.
		''' </summary>
		Private Sub ResetTaxCantonDropDown()

			lueTaxCanton.Properties.DisplayMember = "Description"
			lueTaxCanton.Properties.ValueMember = "GetField"

			Dim columns = lueTaxCanton.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("GetField", 0, String.Empty))
			columns.Add(New LookUpColumnInfo("Description", 0, m_Translate.GetSafeTranslationValue("Kanton")))

			lueTaxCanton.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueTaxCanton.Properties.SearchMode = SearchMode.AutoComplete
			lueTaxCanton.Properties.AutoSearchColumnIndex = 0
			lueTaxCanton.Properties.NullText = String.Empty
			lueTaxCanton.EditValue = Nothing
		End Sub

		''' <summary>
		''' Resets the code drop down.
		''' </summary>
		Private Sub ResetCodeDropDown()

			lueCode.Properties.DisplayMember = "CodeAndTranslation"
			lueCode.Properties.ValueMember = "Code"

			Dim columns = lueCode.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Code", 0, String.Empty))
			columns.Add(New LookUpColumnInfo("Translation", 0, String.Empty))

			lueCode.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueCode.Properties.SearchMode = SearchMode.AutoComplete
			lueCode.Properties.AutoSearchColumnIndex = 1
			lueCode.Properties.NullText = String.Empty
			lueCode.EditValue = Nothing
		End Sub

		''' <summary>
		''' Resets the church tax drop down.
		''' </summary>
		Private Sub ResetChurchTaxDropDown()

			lueChurchTax.Properties.DisplayMember = "ChurchCodeAndTranslation"
			lueChurchTax.Properties.ValueMember = "ChurchTaxCode"

			Dim columns = lueChurchTax.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("ChurchTaxCode", 0, String.Empty))
			columns.Add(New LookUpColumnInfo("ChurchCodeAndTranslation", 0, String.Empty))

			lueChurchTax.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueChurchTax.Properties.SearchMode = SearchMode.AutoComplete
			lueChurchTax.Properties.AutoSearchColumnIndex = 1
			lueChurchTax.Properties.NullText = String.Empty
			lueChurchTax.EditValue = Nothing
		End Sub

		''' <summary>
		''' Resets the number of children drop down.
		''' </summary>
		Private Sub ResetNumberOfChildrenDropDown()

			lueNumberOfChildren.Properties.DisplayMember = "NumberOfChildren"
			lueNumberOfChildren.Properties.ValueMember = "NumberOfChildren"

			Dim columns = lueNumberOfChildren.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("NumberOfChildren", 0, String.Empty))

			lueNumberOfChildren.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueNumberOfChildren.Properties.SearchMode = SearchMode.AutoComplete
			lueNumberOfChildren.Properties.AutoSearchColumnIndex = 1
			lueNumberOfChildren.Properties.NullText = String.Empty
			lueNumberOfChildren.EditValue = Nothing
		End Sub

		''' <summary>
		''' Resets the community drop down.
		''' </summary>
		Private Sub ResetCommunityDropDown()

			lueCommunity.Properties.SearchMode = SearchMode.OnlyInPopup
			lueCommunity.Properties.TextEditStyle = TextEditStyles.Standard

			lueCommunity.Properties.DisplayMember = "ViewData"
			lueCommunity.Properties.ValueMember = "BFSNumber"

			Dim columns = lueCommunity.Properties.Columns
			columns.Clear()
			'columns.Add(New LookUpColumnInfo("BFSNumber", 0, "Code"))
			columns.Add(New LookUpColumnInfo("Translated_Value", 0, m_Translate.GetSafeTranslationValue("Gemeinde")))

			lueCommunity.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueCommunity.Properties.SearchMode = SearchMode.AutoComplete
			lueCommunity.Properties.AutoSearchColumnIndex = 1
			lueCommunity.Properties.NullText = String.Empty
			lueCommunity.EditValue = Nothing
		End Sub

		''' <summary>
		''' Resets the permission state drop down.
		''' </summary>
		Private Sub ResetPermissionDropDown()

			luePermission.Properties.DisplayMember = "Translated_Value"
			luePermission.Properties.ValueMember = "Rec_Value"

			Dim columns = luePermission.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Rec_Value", m_Translate.GetSafeTranslationValue("Code"), 100))
			columns.Add(New LookUpColumnInfo("Translated_Value", m_Translate.GetSafeTranslationValue("Kategorie"), 500))

			luePermission.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			luePermission.Properties.SearchMode = SearchMode.AutoComplete
			luePermission.Properties.AutoSearchColumnIndex = 1
			luePermission.Properties.NullText = String.Empty

			luePermission.Properties.PopupWidth = 700
			luePermission.Properties.PopupSizeable = True

			luePermission.EditValue = Nothing
		End Sub

		Private Sub ResetForeignCategoryDropDown()

			lueForeignCategory.Properties.DisplayMember = "Translated_Value"
			lueForeignCategory.Properties.ValueMember = "Rec_Value"

			Dim columns = lueForeignCategory.Properties.Columns
			columns.Clear()
			'columns.Add(New LookUpColumnInfo("Rec_Value", m_Translate.GetSafeTranslationValue("Code"), 50))
			columns.Add(New LookUpColumnInfo("Translated_Value", m_Translate.GetSafeTranslationValue("Kategorie"), 700))

			lueForeignCategory.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueForeignCategory.Properties.SearchMode = SearchMode.AutoComplete
			lueForeignCategory.Properties.AutoSearchColumnIndex = 0
			lueForeignCategory.Properties.NullText = String.Empty

			lueForeignCategory.Properties.PopupFormWidth = 1000
			lueForeignCategory.Properties.PopupSizeable = True

			lueForeignCategory.EditValue = Nothing
		End Sub

		Private Sub ResetEmploymentTypeDropDown()

			lueEmploymentType.Properties.ShowHeader = False
			lueEmploymentType.Properties.ShowFooter = False
			lueEmploymentType.Properties.DisplayMember = "Translated_Value"
			lueEmploymentType.Properties.ValueMember = "Rec_Value"

			Dim columns = lueEmploymentType.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Translated_Value", 0, m_Translate.GetSafeTranslationValue("Beschäftigungsart")))

			lueEmploymentType.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueEmploymentType.Properties.SearchMode = SearchMode.AutoComplete
			lueEmploymentType.Properties.AutoSearchColumnIndex = 0

			lueEmploymentType.Properties.NullText = String.Empty
			lueEmploymentType.EditValue = Nothing

		End Sub

		Private Sub ResetOtherEmploymentTypeDropDown()

			lueOtherEmploymentType.Properties.ShowHeader = False
			lueOtherEmploymentType.Properties.ShowFooter = False
			lueOtherEmploymentType.Properties.DisplayMember = "Translated_Value"
			lueOtherEmploymentType.Properties.ValueMember = "Rec_Value"

			Dim columns = lueOtherEmploymentType.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Translated_Value", 0, m_Translate.GetSafeTranslationValue("Weitere Beschäftigungsart")))

			lueOtherEmploymentType.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueOtherEmploymentType.Properties.SearchMode = SearchMode.AutoComplete
			lueOtherEmploymentType.Properties.AutoSearchColumnIndex = 0

			lueOtherEmploymentType.Properties.NullText = String.Empty
			lueOtherEmploymentType.EditValue = Nothing

		End Sub

		Private Sub ResetTypeofStayDropDown()

			lueTypeofStay.Properties.ShowHeader = False
			lueTypeofStay.Properties.ShowFooter = False
			lueTypeofStay.Properties.DisplayMember = "Translated_Value"
			lueTypeofStay.Properties.ValueMember = "Rec_Value"

			Dim columns = lueTypeofStay.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Translated_Value", 0, m_Translate.GetSafeTranslationValue("Aufgenthaltsart")))

			lueTypeofStay.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueTypeofStay.Properties.SearchMode = SearchMode.AutoComplete
			lueTypeofStay.Properties.AutoSearchColumnIndex = 0

			lueTypeofStay.Properties.NullText = String.Empty
			lueTypeofStay.EditValue = Nothing

		End Sub

		Private Sub ValidateCHNationatlityFields(ByVal nationalityCode As String, ByVal countryCode As String)

			lblCHNationality.Visible = nationalityCode = "CH" AndAlso countryCode = "CH"

			luePermission.Enabled = Not (nationalityCode = "CH" AndAlso countryCode = "CH") AndAlso m_UserSec118
			lueForeignCategory.Enabled = Not (nationalityCode = "CH" AndAlso countryCode = "CH") AndAlso m_UserSec118
			dateEditPermissionTo.Enabled = Not (nationalityCode = "CH" AndAlso countryCode = "CH") ' AndAlso m_UserSec118
			chkCHPartner.Enabled = Not (nationalityCode = "CH" AndAlso countryCode = "CH") AndAlso m_UserSec118
			chkNoSpecialTax.Enabled = Not (nationalityCode = "CH" AndAlso countryCode = "CH")

		End Sub

		''' <summary>
		''' Loads the drop down data.
		''' </summary>
		Private Function LoadDropDownData() As Boolean
			Dim success As Boolean = True

			success = success AndAlso LoadTaxCantonDropDownData()
			'success = success AndAlso LoadCommunityDropDownData()
			success = success AndAlso LoadCommunityOverWebService()

			success = success AndAlso LoadEmploymentTypeDataOverWebService()
			success = success AndAlso LoadOtherEmploymentTypeDataOverWebService()

			success = success AndAlso LoadTypeOfStayDataOverWebService()

			success = success AndAlso LoadPartnerDropDownData()


			Return success
		End Function

		'Private Function LoadPartnerDropDownData() As Boolean
		'	Dim success As Boolean = True

		'	success = success AndAlso LoadCountryDropDownData()
		'	success = success AndAlso LoadPostcodeDropDownData()

		'	success = success AndAlso LoadGenderDropDownData()

		'	Return success
		'End Function


#Region "loading master data"

		''' <summary>
		''' Loads QST and church tax code translation data.
		''' </summary>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadQSTAndChurchTaxCodeTranslationData() As Boolean
			Dim success As Boolean = True

			' TODO: must be from web service!!!
			m_QSTTranslationData = m_BaseTableUtil.PerformTaxCodeDataOverWebService(m_InitializationData.UserData.UserLanguage)

			'm_QSTTranslationData = m_EmployeeDataAccess.LoadQSTData()
			m_ChurchTaxCodeTranslationData = m_BaseTableUtil.PerformTaxChurchCodeDataOverWebService(m_InitializationData.UserData.UserLanguage) ' m_EmployeeDataAccess.LoadChurchTaxCodeData()

			If m_QSTTranslationData Is Nothing OrElse m_ChurchTaxCodeTranslationData Is Nothing Then
				success = False
			End If

			Return success

		End Function

		''' <summary>
		''' Loads the tax canton drop down data.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadTaxCantonDropDownData() As Boolean
			Dim cantonData = m_CommonDatabaseAccess.LoadCantonData()

			If (cantonData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kantondaten (für Quellensteuer) konnten nicht geladen werden."))
			End If

			lueTaxCanton.Properties.DataSource = cantonData
			lueTaxCanton.Properties.ForceInitialize()
			lueTaxCanton.Properties.DropDownRows = Math.Min(30, cantonData.Count)

			Return Not cantonData Is Nothing
		End Function

		''' <summary>
		''' Loads the community drop down data.
		''' </summary>
		Private Function LoadCommunityOverWebService() As Boolean
			Dim success As Boolean = True

			Try
				Dim language = m_InitializationData.UserData.UserLanguage
				Dim canton = lueTaxCanton.EditValue
				If String.IsNullOrWhiteSpace(canton) Then canton = m_InitializationData.MDData.MDCanton

				Dim result = m_BaseTableUtil.PerformCommunityDataOverWebService(canton, language)

				lueCommunity.Properties.DataSource = result
				If Not result Is Nothing Then lueCommunity.Properties.DropDownRows = Math.Min(20, result.Count + 1)

				success = Not result Is Nothing


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				success = False
			End Try

			Return success
		End Function

		''' <summary>
		''' Loads permission drop down data.
		''' </summary>
		'''<param name="nationality">The nationalilty code.</param>
		'''<param name="countryCode">The country code.</param>
		'''<param name="currentPermission">The current permission code.</param>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadPermissionDropDownData(ByVal nationality As String, ByVal countryCode As String, ByVal currentPermission As String) As Boolean
			'Dim permissionData = m_CommonDatabaseAccess.LoadPermissionData()
			Dim language = m_InitializationData.UserData.UserLanguage
			Dim searchResult = m_BaseTableUtil.PerformPermissionDataOverWebService(language)
			Dim permissionData As New BindingList(Of SP.Internal.Automations.PermissionData)

			For Each result In searchResult
				Dim viewData = New SP.Internal.Automations.PermissionData With {.Rec_Value = result.Code, .Translated_Value = result.Translated_Value}

				If nationality = COUNTRY_CODE_CH AndAlso countryCode = COUNTRY_CODE_CH Then
					If viewData.Rec_Value = "13" Then permissionData.Add(viewData)
				Else
					permissionData.Add(viewData)
				End If

				'permissionData.Add(viewData)
			Next

			If (permissionData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Bewilligungsdaten konnten nicht geladen werden."))
			End If

			' If nationality = Switzerland and country is also switzerland -> show only switzerland permisssion
			If nationality = COUNTRY_CODE_CH AndAlso countryCode = COUNTRY_CODE_CH Then
				permissionData = Nothing ' permissionData.Where(Function(data) data.RecValue = PERMISSION_CODE_S).ToList()
			End If

			Dim supressUIState = m_SuppressUIEvents
			m_SuppressUIEvents = True

			luePermission.Properties.DataSource = permissionData
			luePermission.Properties.ForceInitialize()

			If nationality = COUNTRY_CODE_CH Then
				luePermission.EditValue = PERMISSION_CODE_S
			Else
				luePermission.EditValue = currentPermission
			End If

			If Not permissionData Is Nothing Then luePermission.Properties.DropDownRows = permissionData.Count + 1
			LoadForeignCategoryDataOverWebService()
			ValidateCHNationatlityFields(nationality, countryCode)


			m_SuppressUIEvents = supressUIState

			Return Not permissionData Is Nothing
		End Function

		''' <summary>
		'''  Loads responsible person data.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadEmployeeData(ByVal employeeNumber As Integer) As Boolean

			Dim success As Boolean = True

			success = LoadEmployeeMasterData(employeeNumber)

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
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kandidaten Daten konnten nicht geladen werden."))
				Return False
			End If

			' Quellensteuer

			Dim residence As Boolean = (employeeMasterData.Residence.HasValue AndAlso employeeMasterData.Residence = True)
			Dim childsCount As Short = If(employeeMasterData.ChildsCount.HasValue, employeeMasterData.ChildsCount.Value, 0)
			Dim tax_canton As String = If(String.IsNullOrWhiteSpace(employeeMasterData.S_Canton), m_InitializationData.MDData.MDCanton, employeeMasterData.S_Canton)

			lueTaxCanton.EditValue = tax_canton
			chkCertificateForResidenceReceived.Checked = residence
			dateEditAnsQSTBis.EditValue = employeeMasterData.ANS_OST_Bis

			SetQSTCommunity(employeeMasterData.QSTCommunity)


			' Bewilligung (Take nationality and country code from other user control)
			LoadPermissionDropDownData(m_UCMediator.GetNationalityFromUi(employeeNumber), m_UCMediator.GetCountryCodeFromUi(employeeNumber), employeeMasterData.Permission)

			dateEditPermissionTo.EditValue = employeeMasterData.PermissionToDate
			txtBirthPlace.Text = employeeMasterData.BirthPlace
			chkCHPartner.EditValue = employeeMasterData.CHPartner
			chkNoSpecialTax.EditValue = employeeMasterData.NoSpecialTax
			tgsDoValidate.EditValue = employeeMasterData.ValidatePermissionWithTax

			txtZemisNumber.EditValue = employeeMasterData.ZEMISNumber
			lueForeignCategory.EditValue = employeeMasterData.ForeignCategory


			lblPayrollRelevantBackupInfo.Visible = employeeMasterData.ExistsOldBackupData.HasValue
			If employeeMasterData.ExistsOldBackupData.GetValueOrDefault(False) Then lblPayrollRelevantBackupInfo.ForeColor = Color.Green Else lblPayrollRelevantBackupInfo.ForeColor = Color.Red

			Dim success As Boolean = LoadTaxInfoDataOverWebService(tax_canton, DateTime.Now.Year, m_InitializationData.UserData.UserLanguage)
			If Not success Then m_UtilityUI.SendMailNotification("webservice may be not be available", "webservice may be not be available", String.Empty, Nothing)

			lueCode.Enabled = success AndAlso m_UserSec117
			lueChurchTax.Enabled = success AndAlso m_UserSec117
			lueNumberOfChildren.Enabled = success AndAlso m_UserSec117

			lueEmploymentType.EditValue = employeeMasterData.EmploymentType
			lueOtherEmploymentType.EditValue = employeeMasterData.OtherEmploymentType
			lueTypeofStay.EditValue = employeeMasterData.TypeOfStay

			Dim hasSettingsChanged As Boolean = False
			LoadQSTCodeChurchCodeAndNumberChildren(
											 employeeMasterData.Permission,
											  m_UCMediator.GetCountryCodeFromUi(employeeNumber),
											 residence,
											 employeeMasterData.Q_Steuer,
											 employeeMasterData.ChurchTax,
											childsCount,
											m_TaxDataHelper, m_QSTTranslationData,
											hasSettingsChanged)


			If hasSettingsChanged Then
				m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Die Quellstensteuerangaben bitte nochmals prüfen."))
			End If

			m_SuppressUIEvents = suppressUIEventsState

			Return True

		End Function

		''' <summary>
		''' Sets the QST community.
		''' </summary>
		''' <param name="communityName">The community name.</param>
		Private Sub SetQSTCommunity(ByVal communityName As String)

			'LoadCommunityDropDownData()
			LoadCommunityOverWebService()
			Dim bfsNumber As Integer = Val(communityName)
			If Not String.IsNullOrWhiteSpace(communityName) AndAlso Not lueCommunity.Properties.DataSource Is Nothing AndAlso communityName <> "0" Then
				Dim qstCommunities = CType(lueCommunity.Properties.DataSource, BindingList(Of CommunityData))

				Dim communityData = qstCommunities.Where(Function(data) data.BFSNumber = Val(communityName)).FirstOrDefault
				If Not communityData Is Nothing Then
					bfsNumber = communityData.BFSNumber
					Dim translated_value As String = communityData.Translated_Value

				Else

					Dim newCommunity As New CommunityData
					Dim tmpCommunityLabel As Integer = communityName
					If bfsNumber = 0 Then
						bfsNumber = CInt(Math.Ceiling(Rnd() * 99999)) + 1
					End If
					newCommunity.BFSNumber = bfsNumber
					newCommunity.Translated_Value = communityName
					qstCommunities.Add(newCommunity)
				End If

				lueCommunity.EditValue = bfsNumber
			End If

		End Sub


#End Region


		''' <summary>
		''' Handles new value event on community (Gemeinde) lookup edit.
		''' </summary>
		Private Sub OnLueCommunity_ProcessNewValue(sender As System.Object, e As DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs) Handles lueCommunity.ProcessNewValue

			If Not lueCommunity.Properties.DataSource Is Nothing Then

				Dim listOfCommunities = CType(lueCommunity.Properties.DataSource, BindingList(Of CommunityData))

				Dim newCommunity As New CommunityData With {.Translated_Value = e.DisplayValue.ToString()}
				listOfCommunities.Add(newCommunity)

				e.Handled = True
			End If
		End Sub

		''' <summary>
		'''  Handles edit value change of qst canton.
		''' </summary>
		Private Sub OnLueTaxCanton_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueTaxCanton.EditValueChanged
			If m_SuppressUIEvents Then
				Return
			End If

			If (LoadTaxInfoDataOverWebService(lueTaxCanton.EditValue, DateTime.Now.Year, m_InitializationData.UserData.UserLanguage)) Then
				ReLoadQSTCodeChurchTaxCodeAndNumberChildrenFromUIControlValues()
				' müsste über eine XML-Setting gesteuert werden!
				LoadCommunityOverWebService()
			End If

		End Sub


		''' <summary>
		'''  Handles edit value change of qst code.
		''' </summary>
		Private Sub lOnLueCode_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueCode.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			ReLoadChurchTaxCodeAndNumberOfChildrenFromUIControlValeus()

		End Sub

		''' <summary>
		'''  Handles edit value change of church tax code.
		''' </summary>
		Private Sub OnLueChurchTax_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueChurchTax.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			ReLoadNumberOfChildrenFromUIControlValues()

		End Sub

		''' <summary>
		''' Handles edit value change of permission lookupedit.
		''' </summary>
		Private Sub OnLuePermission_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles luePermission.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			ReLoadQSTCodeChurchTaxCodeAndNumberChildrenFromUIControlValues()
			LoadForeignCategoryDataOverWebService()

		End Sub

		''' <summary>
		''' Loads tax info data over webservice.
		''' </summary>
		''' <param name="canton">The canton.</param>
		''' <param name="year">The year.</param>
		''' <param name="language">The language.</param>
		Private Function LoadTaxInfoDataOverWebService(ByVal canton As String, ByVal year As Integer, ByVal language As String) As Boolean

			If String.IsNullOrEmpty(canton) Then
				canton = m_InitializationData.MDData.MDCanton
				m_Logger.LogWarning("S_Kanton is empty!")
			End If

			Dim success As Boolean = True

			Try
				Dim mdGuid As String = "no md selected!"
				If String.IsNullOrWhiteSpace(m_InitializationData.MDData.MDGuid) Then mdGuid = "No md selected!" Else mdGuid = m_InitializationData.MDData.MDGuid
				Dim result = m_BaseTableUtil.PerformTaxInfoDataOverWebService(canton, year, language)

				If (result Is Nothing) Then
					m_Logger.LogWarning(String.Format("result was NOT passed: Canton: {0} | Year: {1}", canton, year))
					HandleFailedWebServiceConnection()
					success = False
				Else
					m_TaxDataHelper = New TaxDataHelper(result, m_QSTTranslationData, m_ChurchTaxCodeTranslationData)
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				HandleFailedWebServiceConnection()
				success = False
			End Try

			Return success
		End Function

		Private Function LoadEmploymentTypeDataOverWebService() As Boolean
			Dim success As Boolean = True

			Try
				Dim language = m_InitializationData.UserData.UserLanguage
				Dim result = m_BaseTableUtil.PerformEmploymentTypeDataOverWebService(language)
				lueEmploymentType.Properties.DataSource = result

				If Not result Is Nothing Then lueEmploymentType.Properties.DropDownRows = result.Count + 1

				success = Not result Is Nothing


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				success = False
			End Try

			Return success
		End Function

		Private Function LoadOtherEmploymentTypeDataOverWebService() As Boolean
			Dim success As Boolean = True

			Try
				Dim language = m_InitializationData.UserData.UserLanguage
				Dim result = m_BaseTableUtil.PerformOtherEmploymentTypeDataOverWebService(language)
				lueOtherEmploymentType.Properties.DataSource = result

				If Not result Is Nothing Then lueOtherEmploymentType.Properties.DropDownRows = result.Count + 1

				success = Not result Is Nothing


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				success = False
			End Try

			Return success
		End Function

		Private Function LoadTypeOfStayDataOverWebService() As Boolean
			Dim success As Boolean = True

			Try
				Dim language = m_InitializationData.UserData.UserLanguage
				Dim result = m_BaseTableUtil.PerformTypeOfStayDataOverWebService(language)
				lueTypeofStay.Properties.DataSource = result

				If Not result Is Nothing Then lueTypeofStay.Properties.DropDownRows = result.Count + 1

				success = Not result Is Nothing


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				success = False
			End Try

			Return success
		End Function

		Private Function LoadForeignCategoryDataOverWebService() As Boolean
			Dim success As Boolean = True

			Try
				Dim language = m_InitializationData.UserData.UserLanguage
				Dim code = If(luePermission.EditValue Is Nothing, String.Empty, luePermission.EditValue)

				Dim result = m_BaseTableUtil.PerformForeignCategoryDataOverWebService(code, language)
				lueForeignCategory.Properties.DataSource = result

				If Not result Is Nothing Then lueForeignCategory.Properties.DropDownRows = result.Count + 1

				success = Not result Is Nothing


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				success = False
			End Try

			Return success
		End Function

		''' <summary>
		''' Handles failed webservice connection.
		''' </summary>
		Private Sub HandleFailedWebServiceConnection()
			m_TaxDataHelper = Nothing
			m_UCMediator.ReportInvalidData()
			'm_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Quellensteuer Daten konnten nicht über den WebService geladen werden."))
		End Sub

		''' <summary>
		''' Reloads QST, church tax code and number of children data from UI control values.
		''' </summary>
		Private Sub ReLoadQSTCodeChurchTaxCodeAndNumberChildrenFromUIControlValues()

			Dim hasSettingsChanged As Boolean = False ' not used
			LoadQSTCodeChurchCodeAndNumberChildren(
											 luePermission.EditValue,
											 m_UCMediator.GetCountryCodeFromUi(m_EmployeeNumber),
											 chkCertificateForResidenceReceived.Checked,
											 lueCode.EditValue,
											 lueChurchTax.EditValue,
											 lueNumberOfChildren.EditValue,
											 m_TaxDataHelper, m_QSTTranslationData, hasSettingsChanged)
		End Sub

		''' <summary>
		''' Reloads church tax code an number of children from u contol values.
		''' </summary>
		Private Sub ReLoadChurchTaxCodeAndNumberOfChildrenFromUIControlValeus()

			Dim newChurchTaxCode = String.Empty ' not used
			Dim newChildren As Short = 0 ' not used
			LoadChurchTaxCodeAndNumberOfChildren(luePermission.EditValue, lueCode.EditValue, lueChurchTax.EditValue, lueNumberOfChildren.EditValue, m_TaxDataHelper, newChurchTaxCode, newChildren)

		End Sub

		''' <summary>
		''' Reloads number of children from u contol values.
		''' </summary>
		Private Sub ReLoadNumberOfChildrenFromUIControlValues()
			Dim newChildren As Short = 0
			LoadNumberOfChildren(luePermission.EditValue, lueCode.EditValue, lueChurchTax.EditValue, lueNumberOfChildren.EditValue, m_TaxDataHelper, newChildren)
		End Sub

		''' <summary>
		''' Loads QST code, church tax code and number of children data.
		''' </summary>
		''' <param name="permission">Permission code.</param>
		''' <param name="countryCode">The country code.</param>
		''' <param name="certificateForResidenceReceived">The certificate for residence received value.</param>
		''' <param name="currentQSTCode">The current QST code value.</param>
		''' <param name="currentChurchTaxCode">The current church tax code.</param>
		''' <param name="currentChildren">The current children code.</param>
		''' <param name="taxHelper">The tax helper.</param>
		''' <param name="qstTranslationData">The qst translation data.</param>
		''' <param name="hasSettingsChanged">Boolean flag indicating if settings have changed.</param>
		Private Sub LoadQSTCodeChurchCodeAndNumberChildren(
								  ByVal permission As String,
								  ByVal countryCode As String,
								  ByVal certificateForResidenceReceived As Boolean,
								  ByVal currentQSTCode As String,
								  ByVal currentChurchTaxCode As String,
								  ByVal currentChildren As Short,
								  ByVal taxHelper As TaxDataHelper,
								  ByVal qstTranslationData As IEnumerable(Of SP.Internal.Automations.TaxCodeData),
								  ByRef hasSettingsChanged As Boolean)

			If String.IsNullOrEmpty(currentQSTCode) Then
				' Replace empty string with Code_NoQST
				currentQSTCode = Code_NoQST
			End If

			Dim newListOfCodeViewData As List(Of QSTCodeViewData) = New List(Of QSTCodeViewData)

			Dim newQSTCode As String = String.Empty
			Dim newChurchTaxCode = String.Empty
			Dim newChildren As Short = 0

			' Determine data of qst code field
			m_SalaryDataHelperFunctions.DetermineQSTCodeData(permission, countryCode, certificateForResidenceReceived, currentQSTCode, taxHelper, qstTranslationData,
								   newListOfCodeViewData, newQSTCode) ' Output

			' Load church tax and number of children field
			LoadChurchTaxCodeAndNumberOfChildren(permission, newQSTCode, currentChurchTaxCode, currentChildren, taxHelper,
												   newChurchTaxCode, newChildren) ' Output

			' Check if setings has changed
			hasSettingsChanged = ((Not newQSTCode = currentQSTCode) Or
									(Not newChurchTaxCode = currentChurchTaxCode) Or
									(Not newChildren = currentChildren))

			Dim supressUIStae = m_SuppressUIEvents
			m_SuppressUIEvents = True

			' Set QST code
			lueCode.Properties.DataSource = newListOfCodeViewData
			If Not newListOfCodeViewData Is Nothing Then lueCode.Properties.DropDownRows = newListOfCodeViewData.Count + 1

			lueCode.Properties.ForceInitialize()
			lueCode.EditValue = newQSTCode

			m_SuppressUIEvents = supressUIStae

		End Sub

		''' <summary>
		''' Loads church tax code and number of children data.
		''' </summary>
		''' <param name="permission">The permission code.</param>
		''' <param name="currentQSTCode">The current QST code.</param>
		''' <param name="currentChurchTaxCode">The current churchtax code.</param>
		''' <param name="currentChildren">The current number of children.</param>
		''' <param name="taxHelper">The tax helper.</param>
		''' <param name="newChurchTaxCode">The new church tax code.</param>
		''' <param name="newChildren">The new number of children.</param>
		Private Sub LoadChurchTaxCodeAndNumberOfChildren(
								 ByVal permission As String,
								 ByVal currentQSTCode As String,
								 ByVal currentChurchTaxCode As String,
								 ByVal currentChildren As Short,
								 ByVal taxHelper As TaxDataHelper,
								 ByRef newChurchTaxCode As String,
								 ByRef newChildren As Short)

			If String.IsNullOrEmpty(currentQSTCode) Then
				' Replace empty string with Code_NoQST
				currentQSTCode = Code_NoQST
			End If

			Dim newListOfChurchTaxCode = New List(Of ChurchViewData)
			newChurchTaxCode = String.Empty

			' Determine data of church tax code
			m_SalaryDataHelperFunctions.DetermineChurchTaxCodeData(currentQSTCode, permission, currentChurchTaxCode, taxHelper,
										 newListOfChurchTaxCode, newChurchTaxCode) ' Output

			' Load data of number of children field
			LoadNumberOfChildren(permission, currentQSTCode, newChurchTaxCode, currentChildren, taxHelper,
								   newChildren) ' Output

			Dim supressUIStae = m_SuppressUIEvents
			m_SuppressUIEvents = True

			' Set Church tax code
			lueChurchTax.Properties.DataSource = newListOfChurchTaxCode
			lueChurchTax.Properties.ForceInitialize()
			lueChurchTax.EditValue = newChurchTaxCode

			m_SuppressUIEvents = supressUIStae

		End Sub

		''' <summary>
		''' Load number of children data.
		''' </summary>
		''' <param name="permission">The permission code.</param>
		''' <param name="currentQSTCode">The current QST code.</param>
		''' <param name="currentCurchTaxCode">The current church tax code.</param>
		''' <param name="currentChildren">The current number of children.</param>
		''' <param name="taxHelper">The tax helper.</param>
		''' <param name="newChildren">The new number of children.</param>
		Private Sub LoadNumberOfChildren(
								 ByVal permission As String,
								 ByVal currentQSTCode As String,
								 ByVal currentCurchTaxCode As String,
								 ByVal currentChildren As Short,
								 ByVal taxHelper As TaxDataHelper,
								 ByRef newChildren As Short)

			Dim newListOfChildren As New List(Of NumberOfChildrenViewData)
			newChildren = 0

			m_SalaryDataHelperFunctions.DetermineNumberOfChildrenData(currentQSTCode, currentCurchTaxCode, permission, taxHelper, currentChildren, newListOfChildren, newChildren)

			Dim supressUIStae = m_SuppressUIEvents
			m_SuppressUIEvents = True

			' Set number of children 
			lueNumberOfChildren.Properties.DataSource = newListOfChildren
			lueNumberOfChildren.Properties.ForceInitialize()
			lueNumberOfChildren.EditValue = newChildren

			m_SuppressUIEvents = supressUIStae

		End Sub

		''' <summary>
		''' Handles certificate for residence received check box change event.
		''' </summary>
		Private Sub OnChkCertificateForResidenceReceived_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkCertificateForResidenceReceived.CheckedChanged

			If m_SuppressUIEvents Then
				Return
			End If

			ReLoadQSTCodeChurchTaxCodeAndNumberChildrenFromUIControlValues()

		End Sub

		Private Sub lblPayrollRelevantBackupInfo_Click(sender As Object, e As EventArgs) Handles lblPayrollRelevantBackupInfo.Click
			LoadAdornerBackupDataForm()
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
				ElseIf TypeOf sender Is DateEdit Then
					Dim dateEdit As DateEdit = CType(sender, DateEdit)
					dateEdit.EditValue = Nothing
				End If
			End If
		End Sub

		Private Sub lueForeignCategory_Popup(sender As Object, e As EventArgs) Handles lueForeignCategory.Popup

			Dim lookUp As LookUpEdit = TryCast(sender, LookUpEdit)
			lookUp.Properties.BestFit()

			CType(lookUp, IPopupControl).PopupWindow.Width = Math.Max(lookUp.Properties.Columns(0).Width + SystemInformation.VerticalScrollBarWidth, lookUp.Width)

		End Sub

		'Dim form As PopupLookUpEditForm = TryCast((TryCast(sender, IPopupControl)).PopupWindow, PopupLookUpEditForm)
		'Dim width As Integer = GetWidth(TryCast(sender, LookUpEdit))
		'Dim Text = (TryCast(form, DevExpress.Accessibility.IAccessibleGrid)).RowCount.ToString()
		'Trace.WriteLine(String.Format("width: {0}", width))

		'If Not form Is Nothing Then ' AndAlso form.Width > width Then
		'	'form.Width = width * 2
		'End If


		'Dim popupEdit As IPopupControl = TryCast(sender, IPopupControl)
		'If popupEdit Is Nothing Then
		'	Return
		'End If

		'Dim popupForm As PopupContainerForm = TryCast(popupEdit.PopupWindow, PopupContainerForm)
		'If popupForm Is Nothing Then
		'	Return
		'End If

		'Dim field As FieldInfo = GetType(PopupBaseForm).GetField("_viewInfo", BindingFlags.Instance Or BindingFlags.NonPublic)
		'If field Is Nothing Then
		'	Return
		'End If

		'Dim viewInfo As CustomBlobPopupFormViewInfo = TryCast(field.GetValue(popupForm), CustomBlobPopupFormViewInfo)
		'If viewInfo Is Nothing Then
		'	Return
		'End If

		'Dim nonClientHeight As Integer = viewInfo.Bounds.Height - viewInfo.ContentRect.Height
		'Dim test = nonClientHeight.ToString()


		'Private Function GetWidth(ByVal editor As LookUpEdit) As Integer
		'	If editor Is Nothing Then Return 0
		'	Return editor.Width
		'End Function

		'Private Sub lueForeignCategory_QueryPopUp(sender As Object, e As CancelEventArgs) Handles lueForeignCategory.QueryPopUp
		'	'Dim edit As LookUpEdit = CType(sender, LookUpEdit)
		'	'Dim fi As FieldInfo = GetType(RepositoryItem).GetField("_propertyStore", BindingFlags.NonPublic Or BindingFlags.Instance)
		'	'Dim store As IDictionary = CType(fi.GetValue(edit.Properties), IDictionary)
		'	'store("BlobSize") = New Size(50, 150)
		'End Sub


#End Region

	End Class


End Namespace
