

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




Public Class ucMDProgramm

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

	Private m_PayrollSetting As String
	Private m_MandantXMLFile As String
	Private m_MandantXMLSetting As SPProgUtility.CommonXmlUtility.SettingsXml

	Private m_ProgPath As ClsProgPath
	Private m_Year As Integer

#End Region


#Region "private consts"

	Private Const MANDANT_XML_SETTING_SPUTNIK_PAYROLL_SETTING As String = "MD_{0}/Lohnbuchhaltung"
	Private Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"

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

		m_mandant = New Mandant

		m_Utility = New Utility
		m_UtilityUI = New UtilityUI
		m_ProgPath = New ClsProgPath

		If m_InitializationData Is Nothing Then Exit Sub
		m_Year = m_InitializationData.MDData.MDYear
		IsDataValid = True

		Try
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

			m_PayrollSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_PAYROLL_SETTING, m_InitializationData.MDData.MDNr)
			m_MandantXMLFile = m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, m_Year)
			If Not System.IO.File.Exists(m_MandantXMLFile) Then
				m_UtilityUI.ShowErrorDialog(String.Format("Die Einstellung-Datei wurde nicht gefunden.{0}{1}", vbNewLine, m_MandantXMLFile))
				IsDataValid = False
			Else
				m_MandantXMLSetting = New SPProgUtility.CommonXmlUtility.SettingsXml(m_MandantXMLFile)
			End If

			connectionString = m_InitializationData.MDData.MDDbConn
			m_TablesettingDatabaseAccess = New TablesDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
			m_MandantDatabaseAccess = m_TablesettingDatabaseAccess.LoadMandantData(m_InitializationData.MDData.MDNr, m_Year)

			Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If
			Me.xtabMDData.SelectedTabPage = xtabBVG
			Me.xtabNBU.SelectedTabPage = xtabSuva

			Reset()

			TranslateControls()

		Catch ex As Exception
			IsDataValid = False
		End Try

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

			m_PayrollSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_PAYROLL_SETTING, m_InitializationData.MDData.MDNr)
			m_MandantXMLFile = m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, m_Year)

			If Not System.IO.File.Exists(m_MandantXMLFile) Then
				m_UtilityUI.ShowErrorDialog(String.Format("Die Einstellung-Datei wurde nicht gefunden.{0}{1}", vbNewLine, m_MandantXMLFile))
				IsDataValid = False
			Else
				m_MandantXMLSetting = New SPProgUtility.CommonXmlUtility.SettingsXml(m_MandantXMLFile)
			End If
			Me.xtabMDData.SelectedTabPage = xtabBVG

			TranslateControls()

		Catch ex As Exception
			IsDataValid = False

		End Try

	End Sub


	''' <summary>
	'''  Trannslate controls.
	''' </summary>
	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

		chkallowedsociallawithoutemployment.Text = m_Translate.GetSafeTranslationValue(chkallowedsociallawithoutemployment.Text)
		lblALVCheckInfo.Text = m_Translate.GetSafeTranslationValue(lblALVCheckInfo.Text)

	End Sub

	Private Sub Reset()

		Dim suppressState = m_SuppressUIEvents
		m_SuppressUIEvents = True

		' BVG-Data
		txt_BVGMaximalJahr.Text = 0D
		txt_BVGKoordinationJahr.Text = 0D
		txt_BVGMinimalJahr.Text = 0D
		txt_BVGStdGrundlage.Text = 0D
		lblBVGKoordniationStd.Text = 0D

		sp_BVGUnerbruch.EditValue = 0D
		sp_BVGabzugafter.EditValue = 0D
		txt_BVGListeNormal.Text = String.Empty
		txt_BVGListeGruppiert.Text = String.Empty
		chkBVGESDauer.Checked = False
		chkbvgasbusinessdays.Checked = False


		' KTG-Data
		txt_KKANFA.Text = 0D
		txt_KKANFZ.Text = 0D
		txt_KKANMA.Text = 0D
		txt_KKANMZ.Text = 0D

		txt_KKAGFA.Text = 0D
		txt_KKAGFZ.Text = 0D
		txt_KKAGMA.Text = 0D
		txt_KKAGMZ.Text = 0D


		' AHV-Data
		txt_AHVRentenalterF.Text = 0D
		txt_AHVRentenalterM.Text = 0D
		txt_AHVMindestalter.Text = 0D
		txt_AHVBeitragAN.Text = 0D
		txt_AHVBeitragAG.Text = 0D
		txt_AHVUeberAN.Text = 0D
		txt_AHVUeberAG.Text = 0D
		txt_AHVFreiMonat.Text = 0D
		txt_AHVFreiJahr.Text = 0D


		' NBUV-Data
		txt_NBUVFA.Text = 0D
		txt_NBUVFZ.Text = 0D
		txt_NBUVMA.Text = 0D
		txt_NBUVMZ.Text = 0D

		txt_SuvaJahr.Text = 0D
		txt_NBUVWoche.Text = 0D
		txt_FAK.Text = 0D

		txt_UVGA.Text = 0D
		txt_UVGB.Text = 0D
		txt_UVGZA.Text = 0D
		txt_UVGZB.Text = 0D
		txt_UVGZ2A.Text = 0D
		txt_UVGZ2B.Text = 0D


		' ALV1-Data
		txt_ALV1Jahr.Text = 0D
		txt_ALV1AN.Text = 0D
		txt_ALV1AG.Text = 0D
		chkallowedsociallawithoutemployment.Checked = False


		' ALV2-Data
		txt_ALV2Jahr.Text = 0D
		txt_ALV2AN.Text = 0D
		txt_ALV2AG.Text = 0D

		m_SuppressUIEvents = False

	End Sub


	''' <summary>
	''' Handles change of end date.
	''' </summary>
	Private Sub OnBVGKoordination_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles txt_BVGKoordinationJahr.EditValueChanged, txt_BVGStdGrundlage.EditValueChanged

		If m_SuppressUIEvents Then
			Return
		End If

		lblBVGKoordniationStd.Text = String.Format("{0:n2}", txt_BVGKoordinationJahr.EditValue / txt_BVGStdGrundlage.EditValue)

	End Sub


	Public Function LoadMandantenData() As Boolean

		Try
			Dim calculatebvgwithesdays As Boolean = StrToBool(m_ProgPath.GetXMLNodeValue(m_MandantXMLFile, String.Format("{0}/calculatebvgwithesdays", m_PayrollSetting)))
			Dim bvgasbusinessdays As Boolean = StrToBool(m_ProgPath.GetXMLNodeValue(m_MandantXMLFile, String.Format("{0}/bvgasbusinessdays", m_PayrollSetting)))
			Dim bvgintervaladd As Integer = ParseToInteger(m_ProgPath.GetXMLNodeValue(m_MandantXMLFile, String.Format("{0}/bvgintervaladd", m_PayrollSetting)), 13)
			Dim allowedsociallawithoutemployment As Boolean = StrToBool(m_ProgPath.GetXMLNodeValue(m_MandantXMLFile, String.Format("{0}/allowedsociallawithoutemployment", m_PayrollSetting)))

			If (m_MandantDatabaseAccess Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mandantendaten konnten nicht geladen werden."))
				Return False
			End If

			txt_BVGMaximalJahr.EditValue = m_MandantDatabaseAccess.BVG_Max_Jahr
			txt_BVGKoordinationJahr.EditValue = m_MandantDatabaseAccess.BVG_Koordination_Jahr
			txt_BVGMinimalJahr.EditValue = m_MandantDatabaseAccess.BVG_Min_Jahr
			txt_BVGStdGrundlage.EditValue = m_MandantDatabaseAccess.BVG_Std
			lblBVGKoordniationStd.Text = String.Format("{0:n2}", m_MandantDatabaseAccess.BVG_Koordination_Jahr / m_MandantDatabaseAccess.BVG_Std)

			sp_BVGUnerbruch.EditValue = m_MandantDatabaseAccess.BVG_Aus1Woche
			sp_BVGabzugafter.EditValue = bvgintervaladd

			txt_BVGListeNormal.EditValue = m_MandantDatabaseAccess.BVG_List
			txt_BVGListeGruppiert.EditValue = m_MandantDatabaseAccess.BVG_List_Grouped

			chkBVGESDauer.Checked = calculatebvgwithesdays
			chkbvgasbusinessdays.Checked = bvgasbusinessdays


			' KTG-Data
			txt_KKANFA.EditValue = m_MandantDatabaseAccess.KK_An_WA
			txt_KKANFZ.EditValue = m_MandantDatabaseAccess.KK_An_WZ
			txt_KKANMA.EditValue = m_MandantDatabaseAccess.KK_An_MA
			txt_KKANMZ.EditValue = m_MandantDatabaseAccess.KK_An_MZ

			txt_KKAGFA.EditValue = m_MandantDatabaseAccess.KK_AG_WA
			txt_KKAGFZ.EditValue = m_MandantDatabaseAccess.KK_AG_WZ
			txt_KKAGMA.EditValue = m_MandantDatabaseAccess.KK_AG_MA
			txt_KKAGMZ.EditValue = m_MandantDatabaseAccess.KK_AG_MZ


			' AHV-Data
			txt_AHVRentenalterF.EditValue = m_MandantDatabaseAccess.RentAlter_W
			txt_AHVRentenalterM.EditValue = m_MandantDatabaseAccess.RentAlter_M

			txt_AHVMindestalter.EditValue = m_MandantDatabaseAccess.MindestAlter

			txt_AHVBeitragAN.EditValue = m_MandantDatabaseAccess.AHV_AN
			txt_AHVBeitragAG.EditValue = m_MandantDatabaseAccess.AHV_AG
			txt_AHVUeberAN.EditValue = m_MandantDatabaseAccess.AHV_2_AN
			txt_AHVUeberAG.EditValue = m_MandantDatabaseAccess.AHV_2_AG
			txt_AHVFreiMonat.EditValue = m_MandantDatabaseAccess.RentFrei_Monat
			txt_AHVFreiJahr.EditValue = m_MandantDatabaseAccess.RentFrei_Jahr


			' NBUV-Data
			txt_NBUVFA.EditValue = m_MandantDatabaseAccess.NBUV_W
			txt_NBUVFZ.EditValue = m_MandantDatabaseAccess.NBUV_W_Z
			txt_NBUVMA.EditValue = m_MandantDatabaseAccess.NBUV_M
			txt_NBUVMZ.EditValue = m_MandantDatabaseAccess.NBUV_M_Z

			txt_SuvaJahr.EditValue = m_MandantDatabaseAccess.Suva_HL
			txt_NBUVWoche.EditValue = m_MandantDatabaseAccess.NBUV_WStd
			txt_FAK.EditValue = m_MandantDatabaseAccess.Fak_Proz

			txt_UVGA.EditValue = m_MandantDatabaseAccess.Suva_A
			txt_UVGB.EditValue = m_MandantDatabaseAccess.Suva_Z
			txt_UVGZA.EditValue = m_MandantDatabaseAccess.UVGZ_A
			txt_UVGZB.EditValue = m_MandantDatabaseAccess.UVGZ_B
			txt_UVGZ2A.EditValue = m_MandantDatabaseAccess.UVGZ2_A
			txt_UVGZ2B.EditValue = m_MandantDatabaseAccess.UVGZ2_B


			' ALV1-Data
			txt_ALV1Jahr.EditValue = m_MandantDatabaseAccess.ALV1_HL
			txt_ALV1AN.EditValue = m_MandantDatabaseAccess.ALV_AN
			txt_ALV1AG.EditValue = m_MandantDatabaseAccess.ALV_AG
			chkallowedsociallawithoutemployment.Checked = allowedsociallawithoutemployment


			' ALV2-Data
			txt_ALV2Jahr.EditValue = m_MandantDatabaseAccess.ALV2_HL
			txt_ALV2AN.EditValue = m_MandantDatabaseAccess.ALV2_An
			txt_ALV2AG.EditValue = m_MandantDatabaseAccess.ALV2_AG


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

			m_MandantDatabaseAccess.BVG_Max_Jahr = txt_BVGMaximalJahr.EditValue
			m_MandantDatabaseAccess.BVG_Koordination_Jahr = txt_BVGKoordinationJahr.EditValue
			m_MandantDatabaseAccess.BVG_Min_Jahr = txt_BVGMinimalJahr.EditValue
			m_MandantDatabaseAccess.BVG_Std = txt_BVGStdGrundlage.EditValue

			m_MandantDatabaseAccess.BVG_Aus1Woche = sp_BVGUnerbruch.EditValue
			m_MandantDatabaseAccess.BVG_List = txt_BVGListeNormal.EditValue
			m_MandantDatabaseAccess.BVG_List_Grouped = txt_BVGListeGruppiert.EditValue

			' KTG-Data
			m_MandantDatabaseAccess.KK_An_WA = txt_KKANFA.EditValue
			m_MandantDatabaseAccess.KK_An_WZ = txt_KKANFZ.EditValue
			m_MandantDatabaseAccess.KK_An_MA = txt_KKANMA.EditValue
			m_MandantDatabaseAccess.KK_An_MZ = txt_KKANMZ.EditValue

			m_MandantDatabaseAccess.KK_AG_WA = txt_KKAGFA.EditValue
			m_MandantDatabaseAccess.KK_AG_WZ = txt_KKAGFZ.EditValue
			m_MandantDatabaseAccess.KK_AG_MA = txt_KKAGMA.EditValue
			m_MandantDatabaseAccess.KK_AG_MZ = txt_KKAGMZ.EditValue


			' AHV-Data
			m_MandantDatabaseAccess.RentAlter_W = txt_AHVRentenalterF.EditValue
			m_MandantDatabaseAccess.RentAlter_M = txt_AHVRentenalterM.EditValue

			m_MandantDatabaseAccess.MindestAlter = txt_AHVMindestalter.EditValue

			m_MandantDatabaseAccess.AHV_AN = txt_AHVBeitragAN.EditValue
			m_MandantDatabaseAccess.AHV_AG = txt_AHVBeitragAG.EditValue
			m_MandantDatabaseAccess.AHV_2_AN = txt_AHVUeberAN.EditValue
			m_MandantDatabaseAccess.AHV_2_AG = txt_AHVUeberAG.EditValue
			m_MandantDatabaseAccess.RentFrei_Monat = txt_AHVFreiMonat.EditValue
			m_MandantDatabaseAccess.RentFrei_Jahr = txt_AHVFreiJahr.EditValue


			' NBUV-Data
			m_MandantDatabaseAccess.NBUV_W = txt_NBUVFA.EditValue
			m_MandantDatabaseAccess.NBUV_W_Z = txt_NBUVFZ.EditValue
			m_MandantDatabaseAccess.NBUV_M = txt_NBUVMA.EditValue
			m_MandantDatabaseAccess.NBUV_M_Z = txt_NBUVMZ.EditValue
			m_MandantDatabaseAccess.Suva_A = txt_UVGA.EditValue
			m_MandantDatabaseAccess.Suva_Z = txt_UVGB.EditValue
			m_MandantDatabaseAccess.UVGZ_A = txt_UVGZA.EditValue
			m_MandantDatabaseAccess.UVGZ_B = txt_UVGZB.EditValue
			m_MandantDatabaseAccess.UVGZ2_A = txt_UVGZ2A.EditValue
			m_MandantDatabaseAccess.UVGZ2_B = txt_UVGZ2B.EditValue

			m_MandantDatabaseAccess.Suva_HL = txt_SuvaJahr.EditValue
			m_MandantDatabaseAccess.NBUV_WStd = txt_NBUVWoche.EditValue
			m_MandantDatabaseAccess.Fak_Proz = txt_FAK.EditValue


			' ALV1-Data
			m_MandantDatabaseAccess.ALV1_HL = txt_ALV1Jahr.EditValue
			m_MandantDatabaseAccess.ALV_AN = txt_ALV1AN.EditValue
			m_MandantDatabaseAccess.ALV_AG = txt_ALV1AG.EditValue


			' ALV2-Data
			m_MandantDatabaseAccess.ALV2_HL = txt_ALV2Jahr.EditValue
			m_MandantDatabaseAccess.ALV2_An = txt_ALV2AN.EditValue
			m_MandantDatabaseAccess.ALV2_AG = txt_ALV2AG.EditValue


			success = m_TablesettingDatabaseAccess.SaveMandantData(m_InitializationData.MDData.MDNr, m_Year, m_MandantDatabaseAccess)

			' erst wenn in der DB alles OK ist...
			If success Then
				Dim calculatebvgwithesdays = chkBVGESDauer.Checked
				m_MandantXMLSetting.AddOrUpdateSetting(String.Format("{0}/calculatebvgwithesdays", m_PayrollSetting), calculatebvgwithesdays)

				Dim bvgasbusinessdays = chkbvgasbusinessdays.Checked
				m_MandantXMLSetting.AddOrUpdateSetting(String.Format("{0}/bvgasbusinessdays", m_PayrollSetting), bvgasbusinessdays)

				Dim bvgintervaladd = If(sp_BVGabzugafter.EditValue Is Nothing, 13, sp_BVGabzugafter.EditValue)
				m_MandantXMLSetting.AddOrUpdateSetting(String.Format("{0}/bvgintervaladd", m_PayrollSetting), bvgintervaladd)

				Dim allowedsociallawithoutemployment = chkallowedsociallawithoutemployment.Checked
				m_MandantXMLSetting.AddOrUpdateSetting(String.Format("{0}/allowedsociallawithoutemployment", m_PayrollSetting), allowedsociallawithoutemployment)
			End If

			m_SuppressUIEvents = suppressUIEventState


		Catch ex As Exception
      m_Logger.LogError(String.Format("ucMDProgramm: {0}", ex.ToString))
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


End Class


