Imports SP.DatabaseAccess.PayrollMng.DataObjects
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.DatabaseAccess.Employee
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath
Imports SP.DatabaseAccess.PayrollMng
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Employee.DataObjects.Salary
Imports SP.DatabaseAccess.AdvancePaymentMng
Imports SP.Infrastructure.Misc
Imports SP.DatabaseAccess

Partial Public Class EmployeePayroll

#Region "Public const"

	Private Const MANDANT_XML_SETTING_SPUTNIK_PAYROLL_SETTING As String = "MD_{0}/Lohnbuchhaltung"
	Private Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"

	Private Const MANDANT_XML_SETTING_SPUTNIK_EMPLOYEE_TAXINFO_WEBSERVICE_URI As String = "MD_{0}/Interfaces/webservices/webservicetaxinfoservices"
	Private Const DEFAULT_SPUTNIK_EMPLOYEE_TAXINFO_WEBSERVCIE_URI = "wsSPS_services/SPEmployeeTaxInfoService.asmx" ' "http://asmx.domain.com/wsSPS_services/SPEmployeeTaxInfoService.asmx"

#End Region

#Region "Private Fields"

	Private Shared m_Logger As ILogger = New Logger()
	Private m_UtilityUI As UtilityUI
	Private m_Utility As Utility
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper
	Private m_NumberToWordConverter As New NumberToWord()

	Private m_CommonDatabaseAccess As ICommonDatabaseAccess
	Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess
	Private m_PayrollDatabaseAccess As IPayrollDatabaseAccess
	Private m_AdvancePaymentDatabaseAccess As IAdvancePaymentDatabaseAccess

	Private m_md As Mandant
	Private m_ProgPath As ClsProgPath
	Private m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	Private m_EmployeePayrollCommonData As EmployeePayroll_CommonData

	Private m_MandantData As MandantData
	Private m_TaskHelper As TaskHelper

	Private m_Protocol As New System.Text.StringBuilder

	Private m_path As ClsProgPath
	Private m_PayrollSetting As String
	Private m_DONotShowAgainQSTForm As Boolean

	''' <summary>
	''' Settings xml.
	''' </summary>
	Private m_MandantSettingsXml As SPProgUtility.CommonXmlUtility.SettingsXml

	''' <summary>
	''' Tax info service URL.
	''' </summary>
	Private m_TaxInfoServiceUrl As String

#End Region

#Region "Constructor"

	''' <summary>
	''' The constructor.
	''' </summary>
	''' <param name="mdNr">The mandant number.</param>
	''' <param name="_setting">The settings object.</param>
	Public Sub New(ByVal mdNr As Integer,
									 ByVal _setting As SP.Infrastructure.Initialization.InitializeClass,
									 ByVal commonData As EmployeePayroll_CommonData,
									 ByVal taskHelper As TaskHelper, _ShowAgainQSTForm As Boolean)

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		Try
			m_md = New Mandant
			m_ProgPath = New ClsProgPath
			m_InitializationData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
			m_EmployeePayrollCommonData = commonData
			m_PayrollSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_PAYROLL_SETTING, mdNr)
			m_path = New ClsProgPath
			m_TaskHelper = taskHelper
			m_MandantSettingsXml = New SPProgUtility.CommonXmlUtility.SettingsXml(m_md.GetSelectedMDDataXMLFilename(mdNr, Now.Year))

			Dim domainName = m_InitializationData.MDData.WebserviceDomain
			m_TaxInfoServiceUrl = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_EMPLOYEE_TAXINFO_WEBSERVCIE_URI)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			'm_TaxInfoServiceUrl = DEFAULT_SPUTNIK_EMPLOYEE_TAXINFO_WEBSERVCIE_URI
		End Try

		m_UtilityUI = New SP.Infrastructure.UI.UtilityUI
		m_Utility = New Utility

		Dim conStr = m_md.GetSelectedMDData(mdNr).MDDbConn

		m_CommonDatabaseAccess = New DatabaseAccess.Common.CommonDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
		m_EmployeeDatabaseAccess = New DatabaseAccess.Employee.EmployeeDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
		m_PayrollDatabaseAccess = New DatabaseAccess.PayrollMng.PayrollDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
		m_AdvancePaymentDatabaseAccess = New DatabaseAccess.AdvancePaymentMng.AdvancePaymentDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)

		DONotShowAgainQSTForm = _ShowAgainQSTForm
		m_DONotShowAgainQSTForm = _ShowAgainQSTForm

		InitialGAVArayy()

	End Sub

#End Region

#Region "Public Properties"

	''' <summary>
	''' Gets the protocol text.
	''' </summary>
	Public ReadOnly Property ProtocolText As String
		Get
			Return m_Protocol.ToString()
		End Get
	End Property

	''' <summary>
	''' Gets the Checkbox from QST-Form.
	''' </summary>
	Public ReadOnly Property ShowAgainQSTForm As Boolean
		Get
			Return Not m_DONotShowAgainQSTForm
		End Get
	End Property

	Public Property AllowedZeroWorkdaysWithSocialLA As Boolean


#End Region


#Region "Public Methods"

	Public Function CreatePayRollForEmployee(ByVal maNr As Integer,
											   ByVal mdNr As Integer,
											   ByVal lp As Integer,
											   ByVal year As Integer,
											   ByVal setEmployeeLOBackSetting As Boolean) As Boolean

		Dim success As Boolean = False

		Try
			Me.MDNr = mdNr
			LPMonth = lp
			LPYear = year
			Me.SetEmployeeLOBackSetting = setEmployeeLOBackSetting

			WriteToProtocol("")

			m_MandantData = m_PayrollDatabaseAccess.LoadMandantData(year, mdNr)

			If m_MandantData Is Nothing Then
				Dim msg_1 = String.Format(m_Translate.GetSafeTranslationValue("Für das Jahr {0} existieren keine Daten."), LPYear) & vbCrLf &
																																			m_Translate.GetSafeTranslationValue("Bitte erfassen Sie die Mandantendaten in Ihren Systemkonstanen.")
				WriteToProtocol(msg_1)
				' TODO:
				'm_TaskHelper.InUIAndWait(Function()
				'													 m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Für das Jahr {0} existieren keine Daten."), LPYear) & vbCrLf &
				'																														m_Translate.GetSafeTranslationValue("Bitte erfassen Sie die Mandantendaten in Ihren Systemkonstanen."))
				'													 Return True
				'												 End Function)


				Return False
			End If

			LpDate = CDate("01." & LPMonth & "." & LPYear)

			If Not GetMDAnsatzData() Then
				Return False
			End If

			m_EmployeeData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(maNr, False)
			m_EmployeeLOSetting = m_EmployeeDatabaseAccess.LoadEmployeeLOSettings(maNr)

			If AddLorec() Then

				If AddDataToLOLrec() Then
					If (AddALaToLOL()) Then

						SaveFinalDataToLO()
						UpdateZGAndRP()
						UpdateEmployeeLOBackSettings()

						success = IsLOFinished()
						If success Then m_PayrollDatabaseAccess.SetLOIsCompleteFlag(LONewNr, m_EmployeeData.EmployeeNumber, mdNr)
						m_DONotShowAgainQSTForm = DONotShowAgainQSTForm

					End If
				End If

			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			success = False
			WriteToProtocol(ex.ToString())
		Finally
			If Not success AndAlso LONewNr > 0 Then
				Dim cleanupSuccess = m_PayrollDatabaseAccess.CleanupLO(m_EmployeeData.EmployeeNumber, mdNr, LONewNr)
				ThrowExceptionOnError(Not cleanupSuccess, "LO Cleanup ist fehlgeschlagen.")
			End If

		End Try

		If success Then
			WriteToProtocol(String.Format(m_Translate.GetSafeTranslationValue("LONr: {0} ({1} - {2}) war erfolgreich."), LONewNr, lp, year))
		Else
			WriteToProtocol(Padright("LO Task war NICHT erfolgreich!", 30, " "))
		End If

		m_PayrollDatabaseAccess.AddLOProtocolData(New LOProtocolData With {.MANr = maNr, .MDNr = mdNr, .LONr = LONewNr, .LP = lp, .Jahr = year,
												  .Protokoll = m_Protocol.ToString(), .DebugValue = strOriginData,
												  .CreatedOn = DateTime.Now})


		Return success
	End Function

	Private Function GetLAData(ByVal laNr As Decimal, ByVal laList As IEnumerable(Of LAData)) As LAData

		Dim laData = laList.Where(Function(data) data.LANr = laNr).FirstOrDefault()

		Return laData
	End Function

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


End Class
