

Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls

Imports DevExpress.LookAndFeel

Imports SP.DatabaseAccess

Imports SP.DatabaseAccess.TableSetting
Imports SP.DatabaseAccess.TableSetting.DataObjects
Imports SP.DatabaseAccess.TableSetting.DataObjects.MandantData


Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Infrastructure

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages


Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.Mandanten
Imports DevExpress.XtraSplashScreen
Imports SPProgUtility.ProgPath
Imports SP.Infrastructure.Initialization


Public Class ucMDMarginFees

#Region "Private Fields"


	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	Private m_TablesettingDatabaseAccess As ITablesDatabaseAccess
	''' <summary>
	''' The common data access object.
	''' </summary>
	Private m_MandantDatabaseAccess As MandantData

	Private m_SuppressUIEvents As Boolean

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

	Private m_MandantXMLFile As String
	Private m_MandantFormXMLFileName As String
	Private m_MandantSetting As String
	Private m_PayrollSetting As String

	Private m_MandantSettingsXml As SPProgUtility.CommonXmlUtility.SettingsXml

	Private m_ProgPath As ClsProgPath
	Private m_Year As Integer

#End Region


#Region "private consts"

	Private Const MANDANT_XML_MAIN_KEY As String = "MD_{0}/"
	Private Const MANDANT_XML_SETTING_SPUTNIK_PAYROLL_SETTING As String = "MD_{0}/Lohnbuchhaltung"

#End Region


#Region "public property"

	Public Property IsDataValid As Boolean


#End Region


#Region "Constructor"


	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New()

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_SuppressUIEvents = True
		InitializeComponent()
		m_SuppressUIEvents = False

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_InitializationData = ClsDataDetail.m_InitialData ' _setting

		IsDataValid = True
		m_mandant = New Mandant

		m_Utility = New Utility
		m_UtilityUI = New UtilityUI
		m_ProgPath = New ClsProgPath

		If m_InitializationData Is Nothing Then Exit Sub
		m_Year = m_InitializationData.MDData.MDYear
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

		Try

			m_MandantSetting = String.Format(MANDANT_XML_MAIN_KEY, m_InitializationData.MDData.MDNr)

			m_PayrollSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_PAYROLL_SETTING, m_InitializationData.MDData.MDNr)
			m_MandantXMLFile = m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, m_Year)
			If Not System.IO.File.Exists(m_MandantXMLFile) Then
				m_UtilityUI.ShowErrorDialog(String.Format("Die Einstellung-Datei wurde nicht gefunden.{0}{1}", vbNewLine, m_MandantXMLFile))
				IsDataValid = False
			Else
				m_MandantSettingsXml = New SPProgUtility.CommonXmlUtility.SettingsXml(m_MandantXMLFile)
			End If

			connectionString = m_InitializationData.MDData.MDDbConn
			m_TablesettingDatabaseAccess = New TablesDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
			m_MandantDatabaseAccess = m_TablesettingDatabaseAccess.LoadMandantData(m_InitializationData.MDData.MDNr, m_Year)

			Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

		Catch ex As Exception
			IsDataValid = False

		End Try

		Reset()

		TranslateControls()

	End Sub



#End Region


	''' <summary>
	''' Inits the control with configuration information.
	''' </summary>
	'''<param name="initializationClass">The initialization class.</param>
	'''<param name="translationHelper">The translation helper.</param>
	Public Overridable Sub InitWithConfigurationData(ByVal initializationClass As InitializeClass, ByVal translationHelper As TranslateValuesHelper, _Year As Integer)

		m_InitializationData = initializationClass
		m_Translate = translationHelper
		m_Year = _Year
		IsDataValid = True

		Try
			connectionString = m_InitializationData.MDData.MDDbConn
			m_TablesettingDatabaseAccess = New TablesDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
			m_MandantDatabaseAccess = m_TablesettingDatabaseAccess.LoadMandantData(m_InitializationData.MDData.MDNr, m_Year)

			m_MandantSetting = String.Format(MANDANT_XML_MAIN_KEY, m_InitializationData.MDData.MDNr)
			m_MandantFormXMLFileName = m_mandant.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr)

			m_PayrollSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_PAYROLL_SETTING, m_InitializationData.MDData.MDNr)

			m_MandantXMLFile = m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, m_Year)
			If Not System.IO.File.Exists(m_MandantXMLFile) Then
				m_UtilityUI.ShowErrorDialog(String.Format("Die Einstellung-Datei wurde nicht gefunden.{0}{1}", vbNewLine, m_MandantXMLFile))
				IsDataValid = False
			Else
				m_MandantSettingsXml = New SPProgUtility.CommonXmlUtility.SettingsXml(m_MandantXMLFile)
			End If

		Catch ex As Exception
			IsDataValid = False

		End Try

		TranslateControls()

	End Sub


	''' <summary>
	'''  Trannslate controls.
	''' </summary>
	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)


	End Sub

	Private Sub Reset()

		Dim suppressState = m_SuppressUIEvents
		m_SuppressUIEvents = True

		' margin
		txt_MarginCHF.EditValue = 0D

		txt_MarginProz.EditValue = 0D

		txt_MarginVerwaltung.EditValue = 0D
		txt_MarginAGTarifrechner.EditValue = 0D
		txt_MarginZGNOQST.EditValue = 0D
		txt_MarginZGWithQST.EditValue = 0D
		txt_MarginKreditKandidat.EditValue = 0D

		' fee
		txt_FeeZGCheck.EditValue = 0D
		txt_FeeLOCheck.EditValue = 0D
		txt_FeeZGBankCH.EditValue = 0D
		txt_FeeZGBar.EditValue = 0D
		txt_FeeZGBank.EditValue = 0D
		txt_FeeLONoCH.EditValue = 0D


		m_SuppressUIEvents = False

	End Sub


	Public Function LoadMandantenData() As Boolean

		Try
			If (m_MandantDatabaseAccess Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mandantendaten konnten nicht geladen werden."))
				Return False
			End If

			' margin
			txt_MarginCHF.EditValue = m_MandantDatabaseAccess.b_marge
			txt_MarginProz.EditValue = m_MandantDatabaseAccess.b_margep
			txt_MarginVerwaltung.EditValue = m_MandantDatabaseAccess.x_marge

			txt_MarginAGTarifrechner.EditValue = m_MandantDatabaseAccess.ag_tar_proz

			txt_MarginZGNOQST.EditValue = m_MandantDatabaseAccess.n_zhlg
			txt_MarginZGWithQST.EditValue = m_MandantDatabaseAccess.q_zhlg
			txt_MarginKreditKandidat.EditValue = m_MandantDatabaseAccess.ma_kl


			' fee
			Dim advancepaymentcheckfee As Decimal = ParseToDec(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/advancepaymentcheckfee", m_PayrollSetting)), 0D)
			Dim payrollcheckfee As Decimal = ParseToDec(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/payrollcheckfee", m_PayrollSetting)), 0D)
			Dim advancepaymenttransferfee As Decimal = ParseToDec(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/advancepaymenttransferfee", m_PayrollSetting)), 0D)
			Dim advancepaymentcashfee As Decimal = ParseToDec(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/advancepaymentcashfee", m_PayrollSetting)), 0D)
			Dim advancepaymenttransferinternationalfee As Decimal = ParseToDec(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/advancepaymenttransferinternationalfee", m_PayrollSetting)), 0D)
			Dim payrolltransferinternationalfee As Decimal = ParseToDec(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/payrolltransferinternationalfee", m_PayrollSetting)), 0D)


			txt_FeeZGCheck.EditValue = advancepaymentcheckfee
			txt_FeeLOCheck.EditValue = payrollcheckfee

			txt_FeeZGBankCH.EditValue = advancepaymenttransferfee
			txt_FeeZGBar.EditValue = advancepaymentcashfee

			txt_FeeZGBank.EditValue = advancepaymenttransferinternationalfee
			txt_FeeLONoCH.EditValue = payrolltransferinternationalfee


			Return Not m_MandantDatabaseAccess Is Nothing

		Catch ex As Exception

		Finally

		End Try


	End Function

	Public Function SaveMandantenData() As Boolean
		Dim success As Boolean = True
		If Not IsDataValid Then Return False

		Dim suppressUIEventState = m_SuppressUIEvents
		m_SuppressUIEvents = False

		Try
			If (m_MandantDatabaseAccess Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mandantendaten konnten nicht geladen werden."))
				Return False
			End If

			' margin
			m_MandantDatabaseAccess.b_marge = txt_MarginCHF.EditValue
			m_MandantDatabaseAccess.b_margep = txt_MarginProz.EditValue
			m_MandantDatabaseAccess.x_marge = txt_MarginVerwaltung.EditValue
			m_MandantDatabaseAccess.ag_tar_proz = txt_MarginAGTarifrechner.EditValue

			m_MandantDatabaseAccess.n_zhlg = txt_MarginZGNOQST.EditValue
			m_MandantDatabaseAccess.q_zhlg = txt_MarginZGWithQST.EditValue
			m_MandantDatabaseAccess.ma_kl = txt_MarginKreditKandidat.EditValue

			success = m_TablesettingDatabaseAccess.SaveMandantData(m_InitializationData.MDData.MDNr, m_Year, m_MandantDatabaseAccess)


			' erst wenn in der DB alles OK ist...			
			' fee
			If success Then
				Dim advancepaymentcheckfee As Decimal = txt_FeeZGCheck.EditValue
				Dim payrollcheckfee As Decimal = txt_FeeLOCheck.EditValue
				Dim advancepaymenttransferfee As Decimal = txt_FeeZGBankCH.EditValue
				Dim advancepaymentcashfee As Decimal = txt_FeeZGBar.EditValue

				Dim advancepaymenttransferinternationalfee As Decimal = txt_FeeZGBank.EditValue
				Dim payrolltransferinternationalfee As Decimal = txt_FeeLONoCH.EditValue


				m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/advancepaymentcheckfee", m_PayrollSetting), advancepaymentcheckfee)
				m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/payrollcheckfee", m_PayrollSetting), payrollcheckfee)
				m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/advancepaymenttransferfee", m_PayrollSetting), advancepaymenttransferfee)
				m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/advancepaymentcashfee", m_PayrollSetting), advancepaymentcashfee)

				m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/advancepaymenttransferinternationalfee", m_PayrollSetting), advancepaymenttransferinternationalfee)
				m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/payrolltransferinternationalfee", m_PayrollSetting), payrolltransferinternationalfee)

			End If

			m_SuppressUIEvents = suppressUIEventState


		Catch ex As Exception
      m_Logger.LogError(String.Format("ucMDMarginFees: {0}", ex.ToString))
      success = False

    Finally

		End Try

		Return success

	End Function


#Region "Helpers"

	Private Function ParseToBoolean(ByVal stringvalue As String, ByVal value As Boolean?) As Boolean
		Dim result As Boolean
		If (Not Boolean.TryParse(stringvalue, result)) Then
			Return value
		End If
		Return result
	End Function

	Private Function ParseToInteger(ByVal stringvalue As String, ByVal value As Integer?) As Integer
		Dim result As Integer
		If (Not Integer.TryParse(stringvalue, result)) Then
			Return value
		End If
		Return result
	End Function

	Private Function ParseToDec(ByVal stringvalue As String, ByVal value As Decimal?) As Decimal
		Dim result As Decimal
		If (Not Decimal.TryParse(stringvalue, result)) Then
			Return value
		End If
		Return result
	End Function

	Private Function StrToBool(ByVal str As String) As Boolean

		Dim result As Boolean = False

		If String.IsNullOrWhiteSpace(str) Then
			Return False
		End If

		Boolean.TryParse(str, result)

		Return result
	End Function


#End Region


	Private Sub Label17_Click(sender As Object, e As EventArgs) Handles Label17.Click

	End Sub
	Private Sub txt_FeeZGBar_EditValueChanged(sender As Object, e As EventArgs) Handles txt_FeeZGBar.EditValueChanged

	End Sub
	Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click

	End Sub
End Class


