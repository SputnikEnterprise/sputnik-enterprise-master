
Imports System.ComponentModel
Imports System.IO.File
Imports SP.Infrastructure.UI
Imports SPGAV.SPPVLGAVUtilWebService

Public Class ClsMain_Net


#Region "private consts"

	Private Const MANDANT_XML_MAIN_KEY As String = "MD_{0}/"
	Private Const MANDANT_XML_SETTING_SPUTNIK_PAYROLL_SETTING As String = "MD_{0}/Lohnbuchhaltung"
	Public Const DEFAULT_SPUTNIK_PVLGAV_UTIL_WEBSERVICE_URI As String = "http://asmx.domain.com/wsSPS_services/SPPVLGAVUtil.asmx"

#End Region


	Private m_UtilityUI As UtilityUI

	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	Private _clsEventlog As New SPProgUtility.ClsEventLog


	Protected Overrides Sub Finalize()
		MyBase.Finalize()
	End Sub

	Function GetUserID(ByVal strIDNr As String) As String
		Dim strResult As String = _ClsProgSetting.GetMDGuid
		' "7BDD25A4FBB1A9AA5FA5DD9BC8BB6546EFF712FAE3B8C01E20241B03EC9697F04E2DB80838B3F33C"
		Return strResult
	End Function


#Region "Setzen von Start-Eigenschaften..."

	<Obsolete("This method is not used!")>
	Function ShowfrmPVLDialog(ByVal modul As String, ByVal manr As Integer,
													ByVal kdnr As Integer, ByVal esnr As Integer,
													ByVal kanton As String, ByVal strOldGAVInfo As String) As String
		m_UtilityUI.ShowErrorDialog("ShowfrmPVLDialog: Funktion wird nicht mehr unterstützt.")

		Return String.Empty

		'ClsDataDetail.SelectedModulName = modul
		'ClsDataDetail.SelectedMANumber = manr
		'ClsDataDetail.SelectedKDNumber = kdnr
		'ClsDataDetail.SelectedESNumber = esnr
		'ClsDataDetail.SelectedKDKanton = kanton
		'ClsDataDetail.GetOldGAVInfo = strOldGAVInfo

		'Dim frm_test = New frmGAV_PVL(ClsDataDetail.m_InitialData)
		'frm_test.ShowDialog()
		'Dim strResult As String = ClsDataDetail.strGAVData
		''Dim _ClsConvert As New ClsConvert
		''strResult = _ClsConvert.ConvListObject2String(ClsDataDetail.SelectedGAVData, "¦")
		''For i As Integer = 0 To ClsDataDetail.strGAVData.Count - 1
		''  result &= ClsDataDetail.strGAVData(i) & "¦"
		''Next

		'Return strResult
	End Function

	<Obsolete("This method is not used!")>
	Function ListPVLBerufe(ByVal iKDNr As Integer, ByVal strKanton As String) As String
		m_UtilityUI.ShowErrorDialog("ListPVLBerufe: Funktion wird nicht mehr unterstützt.")

		Return String.Empty
	End Function


	<Obsolete("This method is not used!")>
	Function ListPVLBerufeAnhang1() As String
		m_UtilityUI.ShowErrorDialog("ListPVLBerufeAnhang1: Funktion wird nicht mehr unterstützt.")

		Return String.Empty

		'_clsEventlog.WriteMainLog("ListPVLBerufeAnhang1")
		'Dim liBerufe As List(Of String) = GetPVLAnhang1Berufe()
		'Dim strResult As String = String.Empty

		'For i As Integer = 0 To liBerufe.Count - 1
		'	Dim aBeruf As String() = liBerufe(i).Split(CChar("¦"))
		'	strResult &= String.Format("{0}|", aBeruf(1))
		'Next

		'Return strResult
	End Function

	<Obsolete("This method is not used!")>
	Function ShowfrmPVLD_Info(ByVal iMetaNr As Integer) As String
		m_UtilityUI.ShowErrorDialog("ShowfrmPVLD_Info: Funktion wird nicht mehr unterstützt.")

		Return String.Empty

		'Dim liPVLD_Info As List(Of String) = GetPVLCeriterion(iMetaNr)
		'Dim strResult As String = String.Empty

		'Dim frm_test As frmPVLD_Info = New frmPVLD_Info(iMetaNr)
		'frm_test.Show()

		'Return strResult
	End Function

	Function ListGAVData4_FL(ByVal strKanton As String, ByVal strModulName As String,
													ByVal strGAVG0 As String, ByVal strGAVG1 As String,
													ByVal strGAVG2 As String, ByVal strGAVG3 As String,
													ByVal strGAVText As String) As String
		_clsEventlog.WriteMainLog("ListGAVBerufe4_FL")
		Dim liBerufe As List(Of String) = GetGAVData4_FL(strKanton, strModulName, strGAVG0, strGAVG1, strGAVG2, strGAVG3, strGAVText)
		Dim strResult As String = String.Empty

		For i As Integer = 0 To liBerufe.Count - 1
			strResult &= String.Format("{0}|", liBerufe(i).Replace(",", "."))
		Next

		Return strResult
	End Function




	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		ClsDataDetail.m_InitialData = _setting
		ClsDataDetail.m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		Application.EnableVisualStyles()

	End Sub

	Public Sub New()
		Dim m_md As New SPProgUtility.Mandanten.Mandant

		m_UtilityUI = New UtilityUI

		Dim _setting = CreateInitialData(m_md.GetDefaultMDNr, m_md.GetDefaultUSNr)
		ClsDataDetail.m_InitialData = _setting
		ClsDataDetail.m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		Application.EnableVisualStyles()

	End Sub

	Private Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

		Dim m_md As New SPProgUtility.Mandanten.Mandant
		Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
		Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
		Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

		Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
		Dim translate = clsTransalation.GetTranslationInObject

		Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

	End Function


#End Region


End Class


