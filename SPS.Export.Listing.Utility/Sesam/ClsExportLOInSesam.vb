
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.Mandanten
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.ProgPath
Imports SPProgUtility.MainUtilities


Namespace ExportSage

	Public Class ClsKonten

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
		Private _ClsReg As New SPProgUtility.ClsDivReg

		'Private strMDProgFile As String = _ClsProgSetting.GetMDIniFile()
		'Private strInitProgFile As String = _ClsProgSetting.GetInitIniFile()

		Private Property _ExportSetting As New ClsCSVSettings

		Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass
		Private m_md As Mandant
		Private m_common As CommonSetting
		Private m_utility As Utilities
		Private m_path As ClsProgPath

		Private m_UtilityUi As SP.Infrastructure.UI.UtilityUI

		Private m_MandantXMLFile As String
		Private m_FibuSetting As String
		Private m_MandantSettingsXml As SPProgUtility.CommonXmlUtility.SettingsXml


#Region "private consts"

		Private Const MANDANT_XML_MAIN_KEY As String = "MD_{0}/"
		Private Const MANDANT_XML_SETTING_SPUTNIK_FIBU_SETTING As String = "MD_{0}/BuchungsKonten"

#End Region


#Region "Constructor"

		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)


			m_md = New Mandant
			m_common = New CommonSetting
			m_utility = New Utilities
			m_UtilityUi = New UtilityUI
			m_path = New ClsProgPath

			m_InitializationData = _setting

			Try

				m_FibuSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_FIBU_SETTING, m_InitializationData.MDData.MDNr)
				m_MandantXMLFile = m_md.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, m_InitializationData.MDData.MDYear)
				If Not System.IO.File.Exists(m_MandantXMLFile) Then
					m_UtilityUi.ShowErrorDialog(String.Format("Die Einstellung-Datei wurde nicht gefunden.{0}{1}", vbNewLine, m_MandantXMLFile))

				Else
					m_MandantSettingsXml = New SPProgUtility.CommonXmlUtility.SettingsXml(m_MandantXMLFile)
				End If

			Catch ex As Exception
				m_UtilityUi.ShowErrorDialog(String.Format("Die Einstellung-Datei wurde nicht gefunden.{0}{1}", vbNewLine, m_MandantXMLFile))

			End Try

		End Sub

#End Region

		Function GetKontoNr() As List(Of Integer)
			Dim strResult As List(Of Integer) = FillMyLOiBetrag(35)		'(13)

			'Dim _ClsReg As New SPProgUtility.ClsDivReg
			'Dim strMDProgFile As String = _ClsProgSetting.GetMDIniFile()

			'Dim iSKAOPMwSt As Integer
			'Dim iSKAOP As Integer
			'Dim iSKIOPMwSt As Integer
			'Dim iSKIOP As Integer
			'Dim iSKFOPMwSt As Integer
			'Dim iSKFOP As Integer

			'Dim iErAOPMwSt As Integer
			'Dim iErAOP As Integer
			'Dim iErIOPMwSt As Integer
			'Dim iErIOP As Integer
			'Dim iErFOPMwSt As Integer
			'Dim iErFOP As Integer

			'Dim iVerAOPMwSt As Integer
			'Dim iVerAOP As Integer
			'Dim iVerIOPMwSt As Integer
			'Dim iVerIOP As Integer
			'Dim iVerFOPMwSt As Integer
			'Dim iVerFOP As Integer

			'Dim iRuAOPMwSt As Integer
			'Dim iRuAOP As Integer
			'Dim iRuIOPMwSt As Integer
			'Dim iRuIOP As Integer
			'Dim iRuFOPMwSt As Integer
			'Dim iRuFOP As Integer

			'Dim iGuAOPMwSt As Integer
			'Dim iGuAOP As Integer
			'Dim iGuIOPMwSt As Integer
			'Dim iGuIOP As Integer
			'Dim iGuFOPMwSt As Integer
			'Dim iGuFOP As Integer

			'Dim strQuery As String = String.Empty
			'Dim strBez As String = String.Empty

			'Dim iADebitorMwStOp As Integer = 0
			'Dim iIDebitorMwStOp As Integer = 0
			'Dim iFDebitorMwStOp As Integer = 0
			'Dim iMwStOp As Integer = 0


			Dim _1 As Integer = parseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_1", m_FibuSetting)), 0)
			Dim _2 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_2", m_FibuSetting)), 0)

			Dim _3 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_3", m_FibuSetting)), 0)
			Dim _4 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_4", m_FibuSetting)), 0)
			Dim _5 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_5", m_FibuSetting)), 0)
			Dim _6 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_6", m_FibuSetting)), 0)
			Dim _7 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_7", m_FibuSetting)), 0)
			Dim _8 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_8", m_FibuSetting)), 0)
			Dim _9 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_9", m_FibuSetting)), 0)
			Dim _10 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_10", m_FibuSetting)), 0)
			Dim _11 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_11", m_FibuSetting)), 0)
			Dim _12 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_12", m_FibuSetting)), 0)
			Dim _13 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_13", m_FibuSetting)), 0)
			Dim _14 As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_14", m_FibuSetting))
			Dim _15 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_15", m_FibuSetting)), 0)
			Dim _16 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_16", m_FibuSetting)), 0)
			Dim _17 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_17", m_FibuSetting)), 0)
			Dim _18 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_18", m_FibuSetting)), 0)
			Dim _19 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_19", m_FibuSetting)), 0)
			Dim _20 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_20", m_FibuSetting)), 0)
			Dim _21 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_21", m_FibuSetting)), 0)
			Dim _22 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_22", m_FibuSetting)), 0)
			Dim _23 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_23", m_FibuSetting)), 0)
			Dim _24 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_24", m_FibuSetting)), 0)

			Dim _25 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_25", m_FibuSetting)), 0)
			Dim _26 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_26", m_FibuSetting)), 0)
			Dim _27 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_27", m_FibuSetting)), 0)
			Dim _28 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_28", m_FibuSetting)), 0)
			Dim _29 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_29", m_FibuSetting)), 0)
			Dim _30 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_30", m_FibuSetting)), 0)
			Dim _31 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_31", m_FibuSetting)), 0)
			Dim _32 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_32", m_FibuSetting)), 0)
			Dim _33 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_33", m_FibuSetting)), 0)
			Dim _34 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_34", m_FibuSetting)), 0)

			Dim _35 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_35", m_FibuSetting)), 0)
			Dim _36 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_36", m_FibuSetting)), 0)

			Dim _37 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_37", m_FibuSetting)), 0)
			Dim _38 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_38", m_FibuSetting)), 0)





			Try

				' SKonto A Rechnungen -------------------------------------------------------------------------
				'strQuery = "//BuchungsKonten/_10"
				'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
				'If strBez <> String.Empty Then
				'	iSKAOPMwSt = CInt(Val(strBez))
				'Else
				'	iSKAOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "10")))
				'End If
				strResult(0) = _10 ' (iSKAOPMwSt)

				' SKonto I Rechnungen
				'strQuery = "//BuchungsKonten/_11"
				'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
				'If strBez <> String.Empty Then
				'	iSKIOPMwSt = CInt(Val(strBez))
				'Else
				'	iSKIOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "11")))
				'End If
				strResult(1) = _11 '(iSKIOPMwSt)

				' SKonto F Rechnungen
				'strQuery = "//BuchungsKonten/_19"
				'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
				'If strBez <> String.Empty Then
				'	iSKFOPMwSt = CInt(Val(strBez))
				'Else
				'	iSKFOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "19")))
				'End If
				strResult(2) = _19 '(iSKFOPMwSt)

				' SKonto A Rechnungen MwSt.-frei
				'strQuery = "//BuchungsKonten/_12"
				'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
				'If strBez <> String.Empty Then
				'	iSKAOP = CInt(Val(strBez))
				'Else
				'	iSKAOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "12")))
				'End If
				strResult(3) = _12 ' (iSKAOP)


				' SKonto I Rechnungen MwSt.-frei
				'strQuery = "//BuchungsKonten/_13"
				'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
				'If strBez <> String.Empty Then
				'	iSKIOP = CInt(Val(strBez))
				'Else
				'	iSKIOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "13")))
				'End If
				strResult(4) = _13 ' (iSKIOP)


				' SKonto F Rechnungen MwSt.-frei
				'strQuery = "//BuchungsKonten/_20"
				'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
				'If strBez <> String.Empty Then
				'	iSKFOP = CInt(Val(strBez))
				'Else
				'	iSKFOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "20")))
				'End If
				strResult(5) = _20 ' (iSKFOP)


				' Erlös A Rechnungen --------------------------------------------------------------------------
				'strQuery = "//BuchungsKonten/_2"
				'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
				'If strBez <> String.Empty Then
				'	iErAOPMwSt = CInt(Val(strBez))
				'Else
				'	iErAOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "2")))
				'End If
				strResult(6) = _2	' (iErAOPMwSt)

				' Erlös I Rechnungen 
				'strQuery = "//BuchungsKonten/_3"
				'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
				'If strBez <> String.Empty Then
				'	iErIOPMwSt = CInt(Val(strBez))
				'Else
				'	iErIOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "3")))
				'End If
				strResult(7) = _3	' (iErIOPMwSt)


				' Erlös F Rechnungen 
				'strQuery = "//BuchungsKonten/_17"
				'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
				'If strBez <> String.Empty Then
				'	iErFOPMwSt = CInt(Val(strBez))
				'Else
				'	iErFOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "17")))
				'End If
				strResult(8) = _17 ' (iErFOPMwSt)


				' Erlös A Rechnungen MwSt.-frei
				'strQuery = "//BuchungsKonten/_4"
				'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
				'If strBez <> String.Empty Then
				'	iErAOP = CInt(Val(strBez))
				'Else
				'	iErAOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "4")))
				'End If
				strResult(9) = _4	' (iErAOP)

				' Erlös I Rechnungen MwSt.-frei
				'strQuery = "//BuchungsKonten/_5"
				'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
				'If strBez <> String.Empty Then
				'	iErIOP = CInt(Val(strBez))
				'Else
				'	iErIOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "5")))
				'End If
				strResult(10) = _5 ' (iErIOP)

				' Erlös F Rechnungen MwSt.-frei
				'strQuery = "//BuchungsKonten/_18"
				'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
				'If strBez <> String.Empty Then
				'	iErFOP = CInt(Val(strBez))
				'Else
				'	iErFOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "18")))
				'End If
				strResult(11) = _18	' (iErFOP)


				'' Sonstige Verluskonten
				'strQuery = "//BuchungsKonten/_14"
				'strBez = _ClsReg.GetXMLNodeValue(_ClsSystem.GetMDData_XMLFile(), strQuery)
				'If strBez <> String.Empty Then
				'  strSOPVerluste = strBez
				'Else
				'  strSOPVerluste = _ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "14")
				'End If
				strResult(12) = Val(_14)	' (0)
				'    strResult(12) = (CInt(Val(strSOPVerluste)))

				' ---------------------------------------------------------------------------------------------

				' Verlust A Rechnungen MwSt.
				'strQuery = "//BuchungsKonten/_21"
				'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
				'If strBez <> String.Empty Then
				'	iVerAOPMwSt = CInt(Val(strBez))
				'Else
				'	iVerAOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "21")))
				'End If
				strResult(13) = _21	' (iVerAOPMwSt)

				' Verlust I Rechnungen MwSt.
				'strQuery = "//BuchungsKonten/_25"
				'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
				'If strBez <> String.Empty Then
				'	iVerIOPMwSt = CInt(Val(strBez))
				'Else
				'	iVerIOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "25")))
				'End If
				strResult(14) = _25	' (iVerIOPMwSt)

				' Verlust F Rechnungen MwSt.
				'strQuery = "//BuchungsKonten/_29"
				'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
				'If strBez <> String.Empty Then
				'	iVerFOPMwSt = CInt(Val(strBez))
				'Else
				'	iVerFOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "29")))
				'End If
				strResult(15) = _29	' (iVerFOPMwSt)

				' Verlust A Rechnungen MwSt.-frei
				'strQuery = "//BuchungsKonten/_22"
				'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
				'If strBez <> String.Empty Then
				'	iVerAOP = CInt(Val(strBez))
				'Else
				'	iVerAOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "22")))
				'End If
				strResult(16) = _22	' (iVerAOP)

				' Verlust I Rechnungen MwSt.-frei
				'strQuery = "//BuchungsKonten/_26"
				'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
				'If strBez <> String.Empty Then
				'	iVerIOP = CInt(Val(strBez))
				'Else
				'	iVerIOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "26")))
				'End If
				strResult(17) = _26	' (iVerIOP)

				' Verlust F Rechnungen MwSt.-frei
				'strQuery = "//BuchungsKonten/_30"
				'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
				'If strBez <> String.Empty Then
				'	iVerFOP = CInt(Val(strBez))
				'Else
				'	iVerFOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "30")))
				'End If
				strResult(18) = _30	' (iVerFOP)


				' Vergütungen ----------------------------------------------------------------------------------------------
				' Vergütungen A Rechnungen MwSt.
				'strQuery = "//BuchungsKonten/_23"
				'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
				'If strBez <> String.Empty Then
				'	iRuAOPMwSt = CInt(Val(strBez))
				'Else
				'	iRuAOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "23")))
				'End If
				strResult(19) = _23	' (iRuAOPMwSt)

				' Vergütungen I Rechnungen MwSt.
				'strQuery = "//BuchungsKonten/_27"
				'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
				'If strBez <> String.Empty Then
				'	iRuIOPMwSt = CInt(Val(strBez))
				'Else
				'	iRuIOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "27")))
				'End If
				strResult(20) = _27	' (iRuIOPMwSt)

				' Vergütungen F Rechnungen MwSt.
				'strQuery = "//BuchungsKonten/_31"
				'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
				'If strBez <> String.Empty Then
				'	iRuFOPMwSt = CInt(Val(strBez))
				'Else
				'	iRuFOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "31")))
				'End If
				strResult(21) = _31	' (iRuFOPMwSt)

				' Vergütungen A Rechnungen MwSt.-frei
				'strQuery = "//BuchungsKonten/_24"
				'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
				'If strBez <> String.Empty Then
				'	iRuAOP = CInt(Val(strBez))
				'Else
				'	iRuAOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "24")))
				'End If
				strResult(22) = _24	' (iRuAOP)

				' Vergütungen I Rechnungen MwSt.-frei
				'strQuery = "//BuchungsKonten/_28"
				'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
				'If strBez <> String.Empty Then
				'	iRuIOP = CInt(Val(strBez))
				'Else
				'	iRuIOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "28")))
				'End If
				strResult(23) = _28	' (iRuIOP)

				' Vergütungen F Rechnungen MwSt.-frei
				'strQuery = "//BuchungsKonten/_32"
				'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
				'If strBez <> String.Empty Then
				'	iRuFOP = CInt(Val(strBez))
				'Else
				'	iRuFOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "32")))
				'End If
				strResult(24) = _32	' (iRuFOP)


				' Gutschrift ----------------------------------------------------------------------------------------------
				' Gutschrift A Rechnungen MwSt.
				'strQuery = "//BuchungsKonten/_33"
				'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
				'If strBez <> String.Empty Then
				'	iGuAOPMwSt = CInt(Val(strBez))
				'Else
				'	iGuAOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "33")))
				'End If
				strResult(25) = _33	' (iGuAOPMwSt)

				' Gutschrift I Rechnungen MwSt.
				'strQuery = "//BuchungsKonten/_35"
				'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
				'If strBez <> String.Empty Then
				'	iGuIOPMwSt = CInt(Val(strBez))
				'Else
				'	iGuIOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "35")))
				'End If
				strResult(26) = _35	' (iGuIOPMwSt)

				' Gutschrift F Rechnungen MwSt.
				'strQuery = "//BuchungsKonten/_37"
				'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
				'If strBez <> String.Empty Then
				'	iGuFOPMwSt = CInt(Val(strBez))
				'Else
				'	iGuFOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "37")))
				'End If
				strResult(27) = _37	' (iGuFOPMwSt)

				' Gutschrift A Rechnungen MwSt.-frei
				'strQuery = "//BuchungsKonten/_34"
				'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
				'If strBez <> String.Empty Then
				'	iGuAOP = CInt(Val(strBez))
				'Else
				'	iGuAOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "34")))
				'End If
				strResult(28) = _34	' (iGuAOP)

				' Gutschrift I Rechnungen MwSt.-frei
				'strQuery = "//BuchungsKonten/_36"
				'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
				'If strBez <> String.Empty Then
				'	iGuIOP = CInt(Val(strBez))
				'Else
				'	iGuIOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "36")))
				'End If
				strResult(29) = _36	' (iGuIOP)

				' Gutschrift F Rechnungen MwSt.-frei
				'strQuery = "//BuchungsKonten/_38"
				'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
				'If strBez <> String.Empty Then
				'	iGuFOP = CInt(Val(strBez))
				'Else
				'	iGuFOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "38")))
				'End If
				strResult(30) = _38	' (iGuFOP)

				' ---------------------------------------------------------------------------------------------
				' Debitorenkonten für automatische Verbuchung ...
				' Automatische Debitoren
				'strQuery = "//BuchungsKonten/_1"
				'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
				'If strBez <> String.Empty Then
				'	iADebitorMwStOp = CInt(Val(strBez))
				'Else
				'	iADebitorMwStOp = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "1")))
				'End If
				strResult(31) = _1 ' (CInt(Val(iADebitorMwStOp)))

				' Individuelle Debitoren
				'strQuery = "//BuchungsKonten/_15"
				'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
				'If strBez <> String.Empty Then
				'	iIDebitorMwStOp = CInt(Val(strBez))
				'Else
				'	iIDebitorMwStOp = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "15")))
				'End If
				strResult(32) = _15	' (CInt(Val(iIDebitorMwStOp)))

				' Festanstellungen Debitoren
				'strQuery = "//BuchungsKonten/_16"
				'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
				'If strBez <> String.Empty Then
				'	iIDebitorMwStOp = CInt(Val(strBez))
				'Else
				'	iFDebitorMwStOp = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "16")))
				'End If
				strResult(33) = _16	' (CInt(Val(iFDebitorMwStOp)))

				' MwSt.
				'strQuery = "//BuchungsKonten/_6"
				'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
				'If strBez <> String.Empty Then
				'	iMwStOp = CInt(Val(strBez))
				'Else
				'	iMwStOp = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "6")))
				'End If
				strResult(34) = _6 ' (CInt(Val(iMwStOp)))


				' ---------------------------------------------------------------------------------------------

			Catch ex As Exception
				m_UtilityUi.ShowErrorDialog(ex.ToString)

			End Try

			Return strResult
		End Function

		Function FillMyLOiBetrag(ByVal iCount As Integer) As List(Of Integer)
			Dim loiBetrag As New List(Of Integer)

			If iCount = 0 Then iCount = 30
			For i As Integer = 0 To iCount - 1
				loiBetrag.Add(0)
				loiBetrag(i) = 0
			Next

			Return loiBetrag
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

	Public Class ClsExportLOInSesam

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

		Private m_xml As New ClsXML

		Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
		Private _ClsReg As New SPProgUtility.ClsDivReg

		Private m_tablename As String

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI

		Private strSepString As String

		Private iADebitorMwStOp As Integer ' = liKontoNr(31)	'(13)        ' Automatische Debitoren
		Private iIDebitorMwStOp As Integer ' = liKontoNr(32)	'(14)        ' Individuelle Debitoren
		Private iFDebitorMwStOp As Integer ' = liKontoNr(33)							 ' Individuelle Debitoren

		Private iErloesAutoMwStOp As Integer ' = liKontoNr(6)				 ' Erlös automatische OP
		Private iErloesIndMwStOp As Integer	' = liKontoNr(7)				 ' Erlös individuelle OP
		Private iErloesFestMwStOp As Integer ' = liKontoNr(8)				' Erlös Fest OP

		Private iSKontoAutoMwStOp As Integer ' = liKontoNr(0)				 ' sKonto automatische OP
		Private iSKontoIndMwStOp As Integer	' = liKontoNr(1)				 ' sKonto individuelle OP
		Private iSKontoFestMwStOp As Integer ' = liKontoNr(2)					' sKonto Fest OP

		Private iGuAMwStOp As Integer	' = liKontoNr(25)				 ' Gutschriften automatische OP
		Private iGuIndMwStOp As Integer	' = liKontoNr(26)					' Gutschriften individuelle OP
		Private iGuFMwStOp As Integer	' = liKontoNr(27)					' Gutschriften Fest OP

		Private iVerAMwStOp As Integer ' = liKontoNr(13)				' Verlus automatische OP
		Private iVerIndMwStOp As Integer ' = liKontoNr(14)				 ' Verlust individuelle OP
		Private iVerFMwStOp As Integer ' = liKontoNr(15)				 ' Verlust Fest OP

		Private iRuAMwStOp As Integer	' = liKontoNr(19)				 ' Rückvergütung automatische OP
		Private iRuIndMwStOp As Integer	' = liKontoNr(20)					' Rückvergütung individuelle OP
		Private iRuFMwStOp As Integer	' = liKontoNr(21)					' Rückvergütung Fest OP

		Private iMwStOp As Integer ' = liKontoNr(34)	'(15)                  ' MwSt.
		Private Property _ExportSetting As New ClsCSVSettings

		Private _sMDLp As Short

		Private _strFiliale As New List(Of String)
		Private _strFilialeNr As New List(Of String)
		Private _strYear As String

		Private _dMyDate As Date
		Private _dbTotalBetrag As Double
		Private _dbMwSt As Decimal

		Private m_MandantSettingsXml As SPProgUtility.CommonXmlUtility.SettingsXml
		Private m_MandantXMLFile As String
		Private m_MandantSetting As String
		Private m_MandantFormXMLFile As String
		Private m_mandant As Mandant
		Private m_InvoiceSetting As String


#Region "private consts"

		Private Const TABLE_NAME As String = "{0}"
		Private Const MANDANT_XML_MAIN_KEY As String = "MD_{0}/"

		Private Const MANDANT_XML_SETTING_SPUTNIK_INVOICE_SETTING As String = "MD_{0}/Debitoren"
		Private Const MANDANT_XML_SETTING_SPUTNIK_SONSTIGES_SETTING As String = "MD_{0}/Sonstiges"

		Private Const MANDANT_XML_SETTING_SPUTNIK_AHV_SETTING As String = "MD_{0}/AHV-Daten"
		Private Const MANDANT_XML_SETTING_SPUTNIK_SUVA_SETTING As String = "MD_{0}/SUVA-Daten"
		Private Const MANDANT_XML_SETTING_SPUTNIK_FAK_SETTING As String = "MD_{0}/Fak-Daten"
		Private Const FORM_XML_DEFAULTVALUES_KEY As String = "Forms_Normaly/Field_DefaultValues"

#End Region


#Region "Constructor"

		Sub New(ByVal _setting As ClsCSVSettings, ByVal _init As SP.Infrastructure.Initialization.InitializeClass)

			Me._ExportSetting = _setting
			m_InitializationData = _init
			m_UtilityUI = New UtilityUI
			m_mandant = New Mandant

			m_tablename = String.Format(TABLE_NAME, If(_setting.SQL2Open.Contains("[_LOLFibu_"), "_LOLFibu_", "LOLFibu_"))

			Me.MDLp = Me._ExportSetting.SelectedMonth
			Me.MDYear = Me._ExportSetting.SelectedMonth

			Dim dStartOfMonth As Date = CDate("01." & Me._ExportSetting.SelectedMonth & "." & Me._ExportSetting.SelectedYear)
			dStartOfMonth = CDate(dStartOfMonth.ToShortDateString)
			Me.MDDate = GetLastDayInMonth(dStartOfMonth)

			m_MandantSetting = String.Format(MANDANT_XML_MAIN_KEY, m_InitializationData.MDData.MDNr)
			m_MandantFormXMLFile = m_mandant.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr)
			m_InvoiceSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_INVOICE_SETTING, m_InitializationData.MDData.MDNr)

			m_MandantXMLFile = m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, _ExportSetting.SelectedYear)
			If Not System.IO.File.Exists(m_MandantXMLFile) Then
				m_UtilityUI.ShowErrorDialog(String.Format("Die Einstellung-Datei wurde nicht gefunden.{0}{1}", vbNewLine, m_MandantXMLFile))

			Else
				m_MandantSettingsXml = New SPProgUtility.CommonXmlUtility.SettingsXml(m_MandantXMLFile)
			End If


			Me._strFiliale = AllFilial()
			Me._dbMwSt = GetMwStProz(_ExportSetting.SelectedYear)


			Dim _ClsKonten As New ClsKonten(m_InitializationData)
			Dim liKontoNr As List(Of Integer) = _ClsKonten.GetKontoNr()

			iADebitorMwStOp = liKontoNr(31)	'(13)        ' Automatische Debitoren
			iIDebitorMwStOp = liKontoNr(32)	'(14)        ' Individuelle Debitoren
			iFDebitorMwStOp = liKontoNr(33)							 ' Individuelle Debitoren

			iErloesAutoMwStOp = liKontoNr(6)				 ' Erlös automatische OP
			iErloesIndMwStOp = liKontoNr(7)				 ' Erlös individuelle OP
			iErloesFestMwStOp = liKontoNr(8)				' Erlös Fest OP

			iSKontoAutoMwStOp = liKontoNr(0)				' sKonto automatische OP
			iSKontoIndMwStOp = liKontoNr(1)				 ' sKonto individuelle OP
			iSKontoFestMwStOp = liKontoNr(2)					' sKonto Fest OP

			iGuAMwStOp = liKontoNr(25)				 ' Gutschriften automatische OP
			iGuIndMwStOp = liKontoNr(26)					' Gutschriften individuelle OP
			iGuFMwStOp = liKontoNr(27)					' Gutschriften Fest OP

			iVerAMwStOp = liKontoNr(13)				' Verlus automatische OP
			iVerIndMwStOp = liKontoNr(14)				 ' Verlust individuelle OP
			iVerFMwStOp = liKontoNr(15)				 ' Verlust Fest OP

			iRuAMwStOp = liKontoNr(19)				 ' Rückvergütung automatische OP
			iRuIndMwStOp = liKontoNr(20)					' Rückvergütung individuelle OP
			iRuFMwStOp = liKontoNr(21)					' Rückvergütung Fest OP

			iMwStOp = liKontoNr(34) '(15)                  ' MwSt.

		End Sub

#End Region


#Region "Public Properties"

		Public Property MDLp() As Short
			Get
				Return _sMDLp
			End Get
			Set(ByVal value As Short)
				_sMDLp = value
			End Set
		End Property

		'// Jahr
		Public Property MDYear() As String
			Get
				Return _strYear
			End Get
			Set(ByVal value As String)
				_strYear = value
			End Set
		End Property

		Public Property AllFilial() As List(Of String)

			Get
				Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
				Dim Conn As SqlConnection = New SqlConnection(Me._ExportSetting.DbConnString2Open)

				'Dim strTableName As String = String.Format("[_LOLFibu_{0}]", _ClsProgSetting.GetLogedUSGuid) 
				Dim strTableName As String = String.Format("[{0}{1}]", m_tablename, _ClsProgSetting.GetLogedUSGuid)
				Dim sSql As String = String.Empty
				'If clsmainsetting.IsNewVersion Then
				sSql = "[dbo].[List AllFilial From New_LOLFibu]"
				'strTableName = String.Format("[_LOLFibu_{0}]", _ClsProgSetting.GetLogedUSGuid)
				'Else
				'  sSql = "[dbo].[List AllFilial From UJournal]"
				'  strTableName = strTableName

				'End If

				Try
					Conn.Open()
					Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
					cmd.CommandType = Data.CommandType.StoredProcedure
					Dim param As System.Data.SqlClient.SqlParameter
					param = cmd.Parameters.AddWithValue("@tblName", strTableName)

					Dim rDbrec As SqlDataReader = cmd.ExecuteReader

					Dim i As Integer = 0

					While rDbrec.Read()
						_strFiliale.Add(i.ToString)
						_strFilialeNr.Add(i.ToString)

						_strFiliale(i) = If(String.IsNullOrEmpty(rDbrec("USFiliale").ToString), _
															 "nicht definiert", rDbrec("USFiliale").ToString)
						_strFilialeNr(i) = If(String.IsNullOrEmpty(rDbrec("USFiliale").ToString), _
															 "0", rDbrec("_USFilialeNr").ToString)

						i += 1
					End While

				Catch ex As Exception
					Dim strMsg As String = String.Format(m_xml.GetSafeTranslationValue("Error: Möglicherweise wurde die DB1-Liste nicht erstellt.{0}{1}"), vbNewLine, ex.Message)
					m_Logger.LogError(String.Format("{0}.Datenbank lesen: {1}", strMethodeName, ex.Message))
					DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, m_xml.GetSafeTranslationValue("Alle Filialen auflisten"),
																										 System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error)

				Finally

				End Try

				Return _strFiliale
			End Get

			Set(ByVal value As List(Of String))
				_strFiliale = value
			End Set

		End Property

		'// Datum zur Übernahme
		Public Property MDDate() As Date
			Get
				Return _dMyDate
			End Get
			Set(ByVal value As Date)
				_dMyDate = value
			End Set
		End Property

		Private ReadOnly Property GetMwStProz(ByVal iYear As Integer) As Decimal
			Get
				Dim mdNumber = m_InitializationData.MDData.MDNr
				If iYear = 0 Then iYear = Now.Year
				Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}/Debitoren", mdNumber)
				Dim mwstsatz = ParseToDec(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/mwstsatz", FORM_XML_MAIN_KEY)), 8)

				Return mwstsatz
			End Get
		End Property


		'// TotalBetrag
		Public Property dTotalBetrag() As Double
			Get
				Return _dbTotalBetrag
			End Get
			Set(ByVal value As Double)
				_dbTotalBetrag = value
			End Set
		End Property


#End Region



		Private Function GetLastDayInMonth(ByVal dDate As Date) As Date
			dDate = DateAdd(DateInterval.Month, 1, dDate)
			dDate = DateAdd(DateInterval.Day, -1, dDate)
			'dDate = Convert.ToDateTime(Month(dDate).ToString() & "/" & "1/" & Year(dDate).ToString())
			Return dDate
		End Function

		Function GetAllValueToSesam() As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strResult As String = "Success"
			Dim strValue As String = String.Empty
			Dim strFileContent_0 As String = String.Empty
			Dim strFileContent_1 As String = String.Empty
			Dim strFileContent_2 As String = String.Empty
			Dim strFileContent_3 As String = String.Empty
			Dim strFiliale As String = String.Empty

			strSepString = Me._ExportSetting.FieldSeprator
			If GetUJournalQueryForGroupedOutput() = String.Empty Then Return strFileContent_0

			strFileContent_0 = vbNewLine & "//Sesam-Datenimport durch Sputnik " & vbNewLine
			strFileContent_0 &= "//Benutzername: " & _ClsProgSetting.GetUserFName & " " & _ClsProgSetting.GetUserLName & vbNewLine
			strFileContent_0 &= "//Datum: " & Now.ToString & vbNewLine
			strFileContent_0 &= "//Mandanten-Nr.: " & _ClsProgSetting.GetMDNr & vbNewLine & vbNewLine
			strFileContent_0 &= "{Sys Chars=ANSI MType=1}" & vbNewLine

			Try
				If _ExportSetting.ExportInvoiceData Then
					strFileContent_1 = InsertOPRecValueToFile() & vbNewLine & vbNewLine
					strFileContent_1 &= "// " & StrDup(90, "-") & vbNewLine
				End If

				Try
					strFileContent_2 = InsertLORecValueToFile()
					strFileContent_2 &= "// " & StrDup(90, "-") & vbNewLine


				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.Lohndaten: {1}", strMethodeName, ex.Message))
					MsgBox(ex.StackTrace & vbNewLine & vbNewLine & ex.Message, MsgBoxStyle.Critical, "GetAllValueToSesam_0")
					Return String.Empty

				End Try


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Debitorendaten: {1}", strMethodeName, ex.Message))
				MsgBox(ex.StackTrace & vbNewLine & vbNewLine & ex.Message, MsgBoxStyle.Critical, "GetAllValueToSesam_1")
				Return String.Empty

			End Try


			strValue = strFileContent_0 & strFileContent_1 & strFileContent_2

			If Not String.IsNullOrWhiteSpace(strValue) Then
				strResult = Me.WriteContent2File(strValue, Me._ExportSetting.ExportFileName)
				Dim strMsg As String = String.Empty

				If strResult.ToLower.Contains("error") Then
					strMsg = "Fehler bei Erstellung der Datei.{0}{1}"
					strMsg = String.Format(m_xml.GetSafeTranslationValue(strMsg), vbNewLine, strResult)

					DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, m_xml.GetSafeTranslationValue("Dateiexport"),
																										 System.Windows.Forms.MessageBoxButtons.OK,
																										System.Windows.Forms.MessageBoxIcon.Asterisk)
					Return strResult

				Else
					strMsg = "Die Datei wurde erfolgreich erstellt.{0}{1}"
					strMsg = String.Format(m_xml.GetSafeTranslationValue(strMsg), vbNewLine, strResult)

					DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, m_xml.GetSafeTranslationValue("Dateiexport"),
																										 System.Windows.Forms.MessageBoxButtons.OK,
																										 System.Windows.Forms.MessageBoxIcon.Information)

					Dim strDirectory As String = Path.GetDirectoryName(strResult)
					System.Diagnostics.Process.Start(strDirectory)
				End If
			End If

			Return strResult
		End Function

		Function GetUJournalQueryForGroupedOutput() As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim Conn As SqlConnection = New SqlConnection(Me._ExportSetting.DbConnString2Open)
			Dim strGroupTableName As String = String.Format("[UmsatzJournal_New_Grouped_Temp_{0}]", _ClsProgSetting.GetLogedUSGuid)
			Dim strResult As String = String.Format("BEGIN TRY DROP TABLE {0} END TRY BEGIN CATCH END CATCH ", strGroupTableName)

			Conn.Open()

			strResult += "Select sum(KDTotal) as KDTotal, sum(KDTotalInd) As KDTotalInd, sum(KDTotalFest) As KDTotalFest, "
			strResult += "sum(KDTotalTempSKonto) As KDTotalTempSKonto, sum(KDTotalIndSKonto) As KDTotalIndSKonto, "
			strResult += "sum(KDTotalFSKonto) As KDTotalFSKonto, "
			strResult += "sum(KDTotalTempErlos) As KDTotalTempErlos, sum(KDTotalIndErlos) As KDTotalIndErlos, "
			strResult += "sum(KDTotalFErlos) As KDTotalFErlos, "

			strResult += "sum(KDVerlustA) as KDVerlustA, sum(KDVerlustF) as KDVerlustF, "
			strResult += "sum(KDVerlustInd) As KDVerlustInd, "
			strResult += "sum(KDGuA) as KDGuA, sum(KDGuF) as KDGuF, "
			strResult += "sum(KDGuInd) As KDGuInd, "
			strResult += "sum(KDRuA) as KDRuA, sum(KDRuF) as KDRuF, "
			strResult += "sum(KDRuInd) As KDRuInd, "

			strResult += "sum(KDTotalFOp) as KDTotalFOp, "

			strResult += "sum(Bruttolohn) as Bruttolohn, sum(AHVLohn) As AHVLohn, "
			strResult += "sum(AGBetrag) As AGBetrag, sum(FremdLeistung) As FremdLeistung, "
			strResult += "sum(FerBack) as Ferback, sum(FeierBack) As FeierBack, sum(LO13Back) As LO13Back, sum(TimeBack) As TimeBack, "
			strResult += "sum(FerAus) as FerAus, sum(FeierAus) As FeierAus, sum(LO13Aus) As LO13Aus, sum(TimeAus) As TimeAus, USFiliale "
			strResult += String.Format("Into {0} From [UmsatzJournal_New_{1}] ", strGroupTableName, _ClsProgSetting.GetLogedUSGuid)

			strResult += "UmJ Where UserNr = " & _ClsProgSetting.GetLogedUSNr & " "
			strResult += "Group By USFiliale Order By USFiliale"

			Try
				Dim cmd As SqlClient.SqlCommand = New SqlCommand(strResult, Conn)
				cmd.ExecuteNonQuery()


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
				strResult = String.Empty

			End Try

			Return strResult
		End Function

		Function InsertOPRecValueToFile() As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim sSql As String
			Dim cBetrag_0 As Double
			Dim cBetrag_1 As Double

			Dim strFileContent_0 As String
			Dim strFileContent_1 As String

			Dim strFileContent_2_A As String = String.Empty						' Debitoren A
			Dim strFileContent_2_I As String = String.Empty						' Debitoren Ind
			Dim strFileContent_2_F As String = String.Empty						' Debitoren Fest

			Dim strFileContent_2_1_A As String = String.Empty					' Gutschrift A
			Dim strFileContent_2_1_I As String = String.Empty					' Gutschrift Ind
			Dim strFileContent_2_1_F As String = String.Empty					' Gutschrift Fest

			Dim strFileContent_2_2_A As String = String.Empty					' SKonto A
			Dim strFileContent_2_2_I As String = String.Empty					' SKonto Ind
			Dim strFileContent_2_2_F As String = String.Empty					' SKonto Fest

			Dim strFileContent_2_3_A As String = String.Empty					' Verlust A
			Dim strFileContent_2_3_I As String = String.Empty					' Verlust Ind
			Dim strFileContent_2_3_F As String = String.Empty					' Verlust Fest

			Dim strFileContent_2_4_A As String = String.Empty					' Rückvergütung A
			Dim strFileContent_2_4_I As String = String.Empty					' Rückvergütung Ind
			Dim strFileContent_2_4_F As String = String.Empty					' Rückvergütung Fest

			Dim cTotalTempBetrag As Double
			Dim cTotalTempBetrag_1 As Double

			Dim cTotalTempBetrag_0_1 As Double
			Dim cTotalTempBetrag_1_1 As Double

			'Dim _ClsKonten As New ClsKonten
			Dim strSqlQuery As String = Me._ExportSetting.SQL2Open
			Dim Conn As SqlConnection = New SqlConnection(Me._ExportSetting.DbConnString2Open)
			'Dim liKontoNr As List(Of Integer) = _ClsKonten.GetKontoNr()
			Dim strGroupTableName As String = String.Format("[UmsatzJournal_New_Grouped_Temp_{0}]", _ClsProgSetting.GetLogedUSGuid)

			Conn.Open()
			sSql = String.Format("Select Umj.*, Filialen.Code_1 As _USFilialeNr From {0} Umj ", strGroupTableName)
			sSql &= "Left Join Filialen On Umj.USFiliale = Filialen.Bezeichnung"

			Dim cmd As New SqlCommand(sSql, Conn)
			Dim rDbrec As SqlDataReader = cmd.ExecuteReader

			cBetrag_0 = GetKDBetrag()
			cBetrag_1 = cBetrag_0

			' Inklusive MwSt.
			cBetrag_0 += ((cBetrag_0 * Me._dbMwSt) / 100)

			' Daten schreiben...
			' {Blg GFNr=234 Date=31.01.08 Grp=S Orig=1 MType=15
			strFileContent_0 = "//Debitoren;" & vbNewLine
			strFileContent_0 &= "{Blg GFNr=1" & strSepString
			strFileContent_0 &= "Date=" & Me.MDDate.ToShortDateString & strSepString
			strFileContent_0 &= "Grp=S" & strSepString
			strFileContent_0 &= "Orig=1" & strSepString
			strFileContent_0 &= "MType=15" & vbNewLine & vbNewLine

			' {Bk AccId=1100 Type=0 CAcc=div ValNt=1076.00 Text=Debitoren}
			strFileContent_1 = "{BK AccId=" & Trim(Str(iADebitorMwStOp)) & strSepString
			strFileContent_1 &= "Type=0" & strSepString
			strFileContent_1 &= "CAcc=div" & strSepString
			strFileContent_1 &= "#ValNt=#"
			strFileContent_1 &= "Text=Debitoren" & "}" & vbNewLine
			strFileContent_0 &= strFileContent_1

			' --------------------------------------------------------------------------------------------------------------------------
			While rDbrec.Read
				Try

					strFileContent_2_A = "//Temporäre Rechnungen" & strSepString & vbNewLine
					' Temporäre Rechnungen: {Bk AccId=3000 Type=1 CAcc=1100 TaxId=USt76 TIdx=4 CIdx=3 ValNt=1000.00
					' ValTx=76.00 Text=TempOP}
					cTotalTempBetrag = Math.Round(Val(rDbrec("KDTotal").ToString), 4)
					cTotalTempBetrag_1 = Math.Round(cTotalTempBetrag * Me._dbMwSt / 100, 4)

					If cTotalTempBetrag <> 0 Then
						strFileContent_2_A &= "{BK AccId=" & Trim(Str(iErloesAutoMwStOp)) & strSepString
						strFileContent_2_A &= "Type=1" & strSepString
						strFileContent_2_A &= "CAcc=" & Trim(Str(Format(iADebitorMwStOp, "f4"))) & strSepString

						strFileContent_2_A &= String.Format("TaxId=USt{0}", Format(Me._dbMwSt * 10, "00")) & strSepString
						If Not String.IsNullOrEmpty(rDbrec("_USFilialeNr").ToString) Then
							strFileContent_2_A &= "TIdx=4" & strSepString
							strFileContent_2_A &= "CIdx=3" & strSepString
						End If
						strFileContent_2_A &= "ValNt=" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
						strFileContent_2_A &= "ValTx=" & Trim(Str(Format(cTotalTempBetrag_1, "f4"))) & strSepString
						strFileContent_2_A &= "Text=OP_Temporär" & "}" & vbNewLine & Space(2)

						If Not String.IsNullOrEmpty(rDbrec("_USFilialeNr").ToString) Then
							' Kostenstelle exkl. MwSt. ------------------------------------------------------------------------------------
							' {Bk AccId=940 Type=1 CAcc=3000 BType=1 ValNt=1000.00 Text=TempOP940}
							strFileContent_2_A &= "{BK AccId=" & rDbrec("_USFilialeNr").ToString.Trim & strSepString
							strFileContent_2_A &= "Type=1" & strSepString
							strFileContent_2_A &= "CAcc=" & Trim(Str(iErloesAutoMwStOp)) & strSepString
							strFileContent_2_A &= "BType=1" & strSepString
							strFileContent_2_A &= "ValNt=" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
							strFileContent_2_A &= "Text=OP_Temporär_" & rDbrec("_USFilialeNr").ToString.Trim & "}" & vbNewLine & Space(2)
						End If
						' Kostenstelle MwSt. Satz
						' {Bk AccId=2200 Type=1 CAcc=1100 BType=2 ValNt=76.00 ValTx=1000.00 Text=MwStOP940}
						strFileContent_2_A &= "{BK AccId=" & Trim(Str(iMwStOp)) & strSepString
						strFileContent_2_A &= "Type=1" & strSepString
						strFileContent_2_A &= "CAcc=" & Trim(Str(iADebitorMwStOp)) & strSepString
						strFileContent_2_A &= "BType=2" & strSepString
						strFileContent_2_A &= "ValNt=" & Trim(Str(Format(cTotalTempBetrag_1, "f4"))) & strSepString
						strFileContent_2_A &= "ValTx=" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
						strFileContent_2_A &= "Text=OP_Temporär_MwSt_" & rDbrec("_USFilialeNr").ToString.Trim & "}" & vbNewLine

					End If
					cTotalTempBetrag_0_1 += cTotalTempBetrag
					cTotalTempBetrag_1_1 += cTotalTempBetrag_1

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.Temporäre Rechnungen: {1}", strMethodeName, ex.Message))
					MsgBox(ex.Message, MsgBoxStyle.Critical, "InsertOPRecValueToFile_0")

				End Try

				Try

					' ------------------------------------------------------------------------------------------------------------------
					' Individuelle Rechnungen
					strFileContent_2_I &= "//Individuelle Rechnungen" & strSepString & vbNewLine
					' Individuelle Rechnungen: {Bk AccId=3200 Type=1 CAcc=1100
					' TaxId=USt76 TIdx=4 CIdx=3 ValNt=557.60 ValTx=42.40 Text=Testrechung}
					cTotalTempBetrag = Math.Round(Val(rDbrec("KDTotalInd").ToString), 4)
					cTotalTempBetrag_1 = Math.Round(cTotalTempBetrag * Me._dbMwSt / 100, 4)

					If cTotalTempBetrag <> 0 Then
						strFileContent_2_I &= "{BK AccId=" & Trim(Str(iErloesIndMwStOp)) & strSepString
						strFileContent_2_I &= "Type=1" & strSepString
						strFileContent_2_I &= "CAcc=" & Trim(Str(iADebitorMwStOp)) & strSepString

						strFileContent_2_I &= String.Format("TaxId=USt{0}", Format(Me._dbMwSt * 10, "00")) & strSepString
						If Not String.IsNullOrEmpty(rDbrec("_USFilialeNr").ToString) Then
							strFileContent_2_I &= "TIdx=4" & strSepString
							strFileContent_2_I &= "CIdx=3" & strSepString
						End If
						strFileContent_2_I &= "ValNt=" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
						strFileContent_2_I &= "ValTx=" & Trim(Str(Format(cTotalTempBetrag_1, "f4"))) & strSepString
						strFileContent_2_I &= "Text=OP_Sonstige" & "}" & vbNewLine & Space(2)

						If Not String.IsNullOrEmpty(rDbrec("_USFilialeNr").ToString) Then
							' Kostenstelle exkl. MwSt.
							' {Bk AccId=920 Type=1 CAcc=3000 BType=1 ValNt=557.60 Text=Testrechung}
							strFileContent_2_I &= "{BK AccId=" & rDbrec("_USFilialeNr").ToString.Trim & strSepString
							strFileContent_2_I &= "Type=1" & strSepString
							strFileContent_2_I &= "CAcc=" & Trim(Str(iErloesIndMwStOp)) & strSepString
							strFileContent_2_I &= "BType=1" & strSepString
							strFileContent_2_I &= "ValNt=" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
							strFileContent_2_I &= "Text=OP_Sonstige_" & rDbrec("_USFilialeNr").ToString.Trim & "}" & vbNewLine & Space(2)
						End If
						' Kostenstelle MwSt. Satz
						' {Bk AccId=2200 Type=1 CAcc=1100 BType=2 ValNt=42.40 ValTx=557.60 Text=Testrechung}
						strFileContent_2_I &= "{BK AccId=" & Trim(Str(iMwStOp)) & strSepString
						strFileContent_2_I &= "Type=1" & strSepString
						strFileContent_2_I &= "CAcc=" & Trim(Str(iADebitorMwStOp)) & strSepString
						strFileContent_2_I &= "BType=2" & strSepString
						strFileContent_2_I &= "ValNt=" & Trim(Str(Format(cTotalTempBetrag_1, "f4"))) & strSepString
						strFileContent_2_I &= "ValTx=" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
						strFileContent_2_I &= "Text=OP_Sonstige_MwSt_" & rDbrec("_USFilialeNr").ToString.Trim & "}" & vbNewLine

					End If
					cTotalTempBetrag_0_1 += cTotalTempBetrag
					cTotalTempBetrag_1_1 += cTotalTempBetrag_1

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.Individuelle Rechnungen: {1}", strMethodeName, ex.Message))
					MsgBox(ex.Message, MsgBoxStyle.Critical, "InsertOPRecValueToFile_1")

				End Try


				' -------------------------------------------------------------------------------------------------------------------
				' Fest Rechnungen
				' Fest Rechnungen: {Bk AccId=3200 Type=1 CAcc=1100
				' TaxId=USt76 TIdx=4 CIdx=3 ValNt=557.60 ValTx=42.40 Text=Testrechung}

				Try

					cTotalTempBetrag = Math.Round(Val(rDbrec("KDTotalFest").ToString), 4)
					cTotalTempBetrag_1 = Math.Round(cTotalTempBetrag * Me._dbMwSt / 100, 4)
					strFileContent_2_F &= "//Festanstellungen" & strSepString & vbNewLine

					If cTotalTempBetrag <> 0 Then
						strFileContent_2_F &= "{BK AccId=" & Trim(Str(iErloesFestMwStOp)) & strSepString
						strFileContent_2_F &= "Type=1" & strSepString
						strFileContent_2_F &= "CAcc=" & Trim(Str(iADebitorMwStOp)) & strSepString

						strFileContent_2_F &= String.Format("TaxId=USt{0}", Format(Me._dbMwSt * 10, "00")) & strSepString
						If Not String.IsNullOrEmpty(rDbrec("_USFilialeNr").ToString) Then
							strFileContent_2_F &= "TIdx=4" & strSepString
							strFileContent_2_F &= "CIdx=3" & strSepString
						End If
						strFileContent_2_F &= "ValNt=" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
						strFileContent_2_F &= "ValTx=" & Trim(Str(Format(cTotalTempBetrag_1, "f4"))) & strSepString
						strFileContent_2_F &= "Text=OP_Fest" & "}" & vbNewLine & Space(2)

						If Not String.IsNullOrEmpty(rDbrec("_USFilialeNr").ToString) Then
							' Kostenstelle exkl. MwSt.
							' {Bk AccId=920 Type=1 CAcc=3000 BType=1 ValNt=557.60 Text=Testrechung}
							strFileContent_2_F &= "{BK AccId=" & rDbrec("_USFilialeNr").ToString.Trim & strSepString
							strFileContent_2_F &= "Type=1" & strSepString
							strFileContent_2_F &= "CAcc=" & Trim(Str(iErloesFestMwStOp)) & strSepString
							strFileContent_2_F &= "BType=1" & strSepString
							strFileContent_2_F &= "ValNt=" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
							strFileContent_2_F &= "Text=OP_Fest_" & rDbrec("_USFilialeNr").ToString.Trim & "}" & vbNewLine & Space(2)
						End If
						' Kostenstelle MwSt. Satz
						' {Bk AccId=2200 Type=1 CAcc=1100 BType=2 ValNt=42.40 ValTx=557.60 Text=Testrechung}
						strFileContent_2_F &= "{BK AccId=" & Trim(Str(iMwStOp)) & strSepString
						strFileContent_2_F &= "Type=1" & strSepString
						strFileContent_2_F &= "CAcc=" & Trim(Str(iADebitorMwStOp)) & strSepString
						strFileContent_2_F &= "BType=2" & strSepString
						strFileContent_2_F &= "ValNt=" & Trim(Str(Format(cTotalTempBetrag_1, "f4"))) & strSepString
						strFileContent_2_F &= "ValTx=" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
						strFileContent_2_F &= "Text=OP_Fest_MwSt_" & rDbrec("_USFilialeNr").ToString.Trim & "}" & vbNewLine

					End If
					cTotalTempBetrag_0_1 += cTotalTempBetrag
					cTotalTempBetrag_1_1 += cTotalTempBetrag_1

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.Festanstellungen: {1}", strMethodeName, ex.Message))
					MsgBox(ex.Message, MsgBoxStyle.Critical, "InsertOPRecValueToFile_2")

				End Try

				' --------------------------------------------------------------------------------------------------------------------
				' Gutschriften Temporär Rechnungen
				Try
					strFileContent_2_1_A &= "//Gutschriften Temporär" & strSepString & vbNewLine
					' Gutschriften A: {Bk AccId=3000 Type=1 CAcc=1100
					' TaxId=USt76 TIdx=4 CIdx=3 ValNt=-9.30 ValTx=-0.70 Text=Testgutschrift}
					cTotalTempBetrag = Math.Round(Val(rDbrec("KDGuA").ToString), 4)
					cTotalTempBetrag_1 = Math.Round(cTotalTempBetrag * Me._dbMwSt / 100, 4)

					If cTotalTempBetrag <> 0 Then
						strFileContent_2_1_A &= "{BK AccId=" & Trim(Str(iGuAMwStOp)) & strSepString
						strFileContent_2_1_A &= "Type=1" & strSepString
						strFileContent_2_1_A &= "CAcc=" & Trim(Str(iADebitorMwStOp)) & strSepString

						strFileContent_2_1_A &= String.Format("TaxId=USt{0}", Format(Me._dbMwSt * 10, "00")) & strSepString
						If Not String.IsNullOrEmpty(rDbrec("_USFilialeNr").ToString) Then
							strFileContent_2_1_A &= "TIdx=4" & strSepString
							strFileContent_2_1_A &= "CIdx=3" & strSepString
						End If
						strFileContent_2_1_A &= "ValNt=-" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
						strFileContent_2_1_A &= "ValTx=-" & Trim(Str(Format(cTotalTempBetrag_1, "f4"))) & strSepString
						strFileContent_2_1_A &= "Text=GU_Temporär" & "}" & vbNewLine & Space(2)

						If Not String.IsNullOrEmpty(rDbrec("_USFilialeNr").ToString) Then
							' Kostenstelle exkl. MwSt.
							' {Bk AccId=920 Type=1 CAcc=3000 BType=1 ValNt=-9.30 Text=Testgutschrift}
							strFileContent_2_1_A &= "{BK AccId=" & rDbrec("_USFilialeNr").ToString.Trim & strSepString
							strFileContent_2_1_A &= "Type=1" & strSepString
							strFileContent_2_1_A &= "CAcc=" & Trim(Str(iGuAMwStOp)) & strSepString
							strFileContent_2_1_A &= "BType=1" & strSepString
							strFileContent_2_1_A &= "ValNt=-" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
							strFileContent_2_1_A &= "Text=GU_Temporär_" & rDbrec("_USFilialeNr").ToString.Trim & "}" & vbNewLine & Space(2)
						End If
						' Kostenstelle MwSt. Satz
						' {Bk AccId=2200 Type=1 CAcc=1100 BType=2 ValNt=-0.70 ValTx=-9.30 Text=Testgutschrift}
						strFileContent_2_1_A &= "{BK AccId=" & Trim(Str(iMwStOp)) & strSepString
						strFileContent_2_1_A &= "Type=1" & strSepString
						strFileContent_2_1_A &= "CAcc=" & Trim(Str(iADebitorMwStOp)) & strSepString
						strFileContent_2_1_A &= "BType=2" & strSepString
						strFileContent_2_1_A &= "ValNt=-" & Trim(Str(Format(cTotalTempBetrag_1, "f4"))) & strSepString
						strFileContent_2_1_A &= "ValTx=-" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
						strFileContent_2_1_A &= "Text=GU_Temporär_Mwst_" & rDbrec("_USFilialeNr").ToString.Trim & "}" & vbNewLine

					End If
					cTotalTempBetrag_0_1 -= cTotalTempBetrag
					cTotalTempBetrag_1_1 -= cTotalTempBetrag_1

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.Gutschriften: {1}", strMethodeName, ex.Message))
					MsgBox(ex.Message, MsgBoxStyle.Critical, "InsertOPRecValueToFile_3")

				End Try

				' --------------------------------------------------------------------------------------------------------------------
				' Gutschriften Ind
				Try
					strFileContent_2_1_I &= "//Gutschriften Ind" & strSepString & vbNewLine
					' Gutschriften Ind: {Bk AccId=3000 Type=1 CAcc=1100
					' TaxId=USt76 TIdx=4 CIdx=3 ValNt=-9.30 ValTx=-0.70 Text=Testgutschrift}
					cTotalTempBetrag = Math.Round(Val(rDbrec("KDGuInd").ToString), 4)
					cTotalTempBetrag_1 = Math.Round(cTotalTempBetrag * Me._dbMwSt / 100, 4)

					If cTotalTempBetrag <> 0 Then
						strFileContent_2_1_I &= "{BK AccId=" & Trim(Str(iGuIndMwStOp)) & strSepString
						strFileContent_2_1_I &= "Type=1" & strSepString
						strFileContent_2_1_I &= "CAcc=" & Trim(Str(iADebitorMwStOp)) & strSepString

						strFileContent_2_1_I &= String.Format("TaxId=USt{0}", Format(Me._dbMwSt * 10, "00")) & strSepString
						If Not String.IsNullOrEmpty(rDbrec("_USFilialeNr").ToString) Then
							strFileContent_2_1_I &= "TIdx=4" & strSepString
							strFileContent_2_1_I &= "CIdx=3" & strSepString
						End If
						strFileContent_2_1_I &= "ValNt=-" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
						strFileContent_2_1_I &= "ValTx=-" & Trim(Str(Format(cTotalTempBetrag_1, "f4"))) & strSepString
						strFileContent_2_1_I &= "Text=GU_Sonstige" & "}" & vbNewLine & Space(2)

						If Not String.IsNullOrEmpty(rDbrec("_USFilialeNr").ToString) Then
							' Kostenstelle exkl. MwSt.
							' {Bk AccId=920 Type=1 CAcc=3000 BType=1 ValNt=-9.30 Text=Testgutschrift}
							strFileContent_2_1_I &= "{BK AccId=" & rDbrec("_USFilialeNr").ToString.Trim & strSepString
							strFileContent_2_1_I &= "Type=1" & strSepString
							strFileContent_2_1_I &= "CAcc=" & Trim(Str(iGuIndMwStOp)) & strSepString
							strFileContent_2_1_I &= "BType=1" & strSepString
							strFileContent_2_1_I &= "ValNt=-" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
							strFileContent_2_1_I &= "Text=GU_Sonstige_" & rDbrec("_USFilialeNr").ToString.Trim & "}" & vbNewLine & Space(2)
						End If
						' Kostenstelle MwSt. Satz
						' {Bk AccId=2200 Type=1 CAcc=1100 BType=2 ValNt=-0.70 ValTx=-9.30 Text=Testgutschrift}
						strFileContent_2_1_I &= "{BK AccId=" & Trim(Str(iMwStOp)) & strSepString
						strFileContent_2_1_I &= "Type=1" & strSepString
						strFileContent_2_1_I &= "CAcc=" & Trim(Str(iADebitorMwStOp)) & strSepString
						strFileContent_2_1_I &= "BType=2" & strSepString
						strFileContent_2_1_I &= "ValNt=-" & Trim(Str(Format(cTotalTempBetrag_1, "f4"))) & strSepString
						strFileContent_2_1_I &= "ValTx=-" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
						strFileContent_2_1_I &= "Text=GU_Sonstige_MwSt_" & rDbrec("_USFilialeNr").ToString.Trim & "}" & vbNewLine

					End If
					cTotalTempBetrag_0_1 -= cTotalTempBetrag
					cTotalTempBetrag_1_1 -= cTotalTempBetrag_1

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.Gutschriften Individuelle: {1}", strMethodeName, ex.Message))
					MsgBox(ex.Message, MsgBoxStyle.Critical, "InsertOPRecValueToFile_4")

				End Try

				' --------------------------------------------------------------------------------------------------------------------
				' Gutschriften Fest
				Try
					strFileContent_2_1_F &= "//Gutschriften Fest" & strSepString & vbNewLine
					' Gutschriften Ind: {Bk AccId=3000 Type=1 CAcc=1100
					' TaxId=USt76 TIdx=4 CIdx=3 ValNt=-9.30 ValTx=-0.70 Text=Testgutschrift}
					cTotalTempBetrag = Math.Round(Val(rDbrec("KDGuF").ToString), 4)
					cTotalTempBetrag_1 = Math.Round(cTotalTempBetrag * Me._dbMwSt / 100, 4)

					If cTotalTempBetrag <> 0 Then
						strFileContent_2_1_F &= "{BK AccId=" & Trim(Str(iGuFMwStOp)) & strSepString
						strFileContent_2_1_F &= "Type=1" & strSepString
						strFileContent_2_1_F &= "CAcc=" & Trim(Str(iADebitorMwStOp)) & strSepString

						strFileContent_2_1_F &= String.Format("TaxId=USt{0}", Format(Me._dbMwSt * 10, "00")) & strSepString
						If Not String.IsNullOrEmpty(rDbrec("_USFilialeNr").ToString) Then
							strFileContent_2_1_F &= "TIdx=4" & strSepString
							strFileContent_2_1_F &= "CIdx=3" & strSepString
						End If
						strFileContent_2_1_F &= "ValNt=-" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
						strFileContent_2_1_F &= "ValTx=-" & Trim(Str(Format(cTotalTempBetrag_1, "f4"))) & strSepString
						strFileContent_2_1_F &= "Text=GU_Fest" & "}" & vbNewLine & Space(2)

						If Not String.IsNullOrEmpty(rDbrec("_USFilialeNr").ToString) Then
							' Kostenstelle exkl. MwSt.
							' {Bk AccId=920 Type=1 CAcc=3000 BType=1 ValNt=-9.30 Text=Testgutschrift}
							strFileContent_2_1_F &= "{BK AccId=" & rDbrec("_USFilialeNr").ToString.Trim & strSepString
							strFileContent_2_1_F &= "Type=1" & strSepString
							strFileContent_2_1_F &= "CAcc=" & Trim(Str(iGuFMwStOp)) & strSepString
							strFileContent_2_1_F &= "BType=1" & strSepString
							strFileContent_2_1_F &= "ValNt=-" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
							strFileContent_2_1_F &= "Text=GU_Fest_" & rDbrec("_USFilialeNr").ToString.Trim & "}" & vbNewLine & Space(2)
						End If
						' Kostenstelle MwSt. Satz
						' {Bk AccId=2200 Type=1 CAcc=1100 BType=2 ValNt=-0.70 ValTx=-9.30 Text=Testgutschrift}
						strFileContent_2_1_F &= "{BK AccId=" & Trim(Str(iMwStOp)) & strSepString
						strFileContent_2_1_F &= "Type=1" & strSepString
						strFileContent_2_1_F &= "CAcc=" & Trim(Str(iADebitorMwStOp)) & strSepString
						strFileContent_2_1_F &= "BType=2" & strSepString
						strFileContent_2_1_F &= "ValNt=-" & Trim(Str(Format(cTotalTempBetrag_1, "f4"))) & strSepString
						strFileContent_2_1_F &= "ValTx=-" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
						strFileContent_2_1_F &= "Text=GU_Fest_MwSt_" & rDbrec("_USFilialeNr").ToString.Trim & "}" & vbNewLine

					End If
					cTotalTempBetrag_0_1 -= cTotalTempBetrag
					cTotalTempBetrag_1_1 -= cTotalTempBetrag_1

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.Gutschriften Festanstellungen: {1}", strMethodeName, ex.Message))
					MsgBox(ex.Message, MsgBoxStyle.Critical, "InsertOPRecValueToFile_5")

				End Try

				' SKonto Minderung
				' --------------------------------------------------------------------------------------------------------------------
				Try
					strFileContent_2_2_A &= "//Temporär Skonto" & strSepString & vbNewLine
					' Temporär Skonto: {Bk AccId=3090 Type=1 CAcc=1100
					' TaxId=USt76 TIdx=4 CIdx=3 ValNt=-9.30 ValTx=-0.70 Text=TestsKonto}
					cTotalTempBetrag = Math.Round(CDbl(Val(rDbrec("KDTotalTempSKonto").ToString)), 4)
					cTotalTempBetrag_1 = Math.Round(cTotalTempBetrag * Me._dbMwSt / 100, 4)

					If cTotalTempBetrag <> 0 Then
						strFileContent_2_2_A &= "{BK AccId=" & Trim(Str(iSKontoAutoMwStOp)) & strSepString
						strFileContent_2_2_A &= "Type=1" & strSepString
						strFileContent_2_2_A &= "CAcc=" & Trim(Str(iADebitorMwStOp)) & strSepString

						strFileContent_2_2_A &= String.Format("TaxId=USt{0}", Format(Me._dbMwSt * 10, "00")) & strSepString
						If Not String.IsNullOrEmpty(rDbrec("_USFilialeNr").ToString) Then
							strFileContent_2_2_A &= "TIdx=4" & strSepString
							strFileContent_2_2_A &= "CIdx=3" & strSepString
						End If
						strFileContent_2_2_A &= "ValNt=-" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
						strFileContent_2_2_A &= "ValTx=-" & Trim(Str(Format(cTotalTempBetrag_1, "f4"))) & strSepString
						strFileContent_2_2_A &= "Text=SK_Temporär" & "}" & vbNewLine & Space(2)

						If Not String.IsNullOrEmpty(rDbrec("_USFilialeNr").ToString) Then
							' Kostenstelle exkl. MwSt.
							' {Bk AccId=920 Type=1 CAcc=3090 BType=1 ValNt=-9.30 Text=TestsKonto}
							strFileContent_2_2_A &= "{BK AccId=" & Str(rDbrec("_USFilialeNr").ToString.Trim) & strSepString
							strFileContent_2_2_A &= "Type=1" & strSepString
							strFileContent_2_2_A &= "CAcc=" & Trim(Str(iSKontoAutoMwStOp)) & strSepString
							strFileContent_2_2_A &= "BType=1" & strSepString
							strFileContent_2_2_A &= "ValNt=-" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
							strFileContent_2_2_A &= "Text=SK_Temporär_" & rDbrec("_USFilialeNr").ToString.Trim & "}" & vbNewLine & Space(2)
						End If
						' Kostenstelle MwSt. Satz
						' {Bk AccId=2200 Type=1 CAcc=1100 BType=2 ValNt=-0.70 ValTx=-9.30 Text=TestsKonto}
						strFileContent_2_2_A &= "{BK AccId=" & Trim(Str(iMwStOp)) & strSepString
						strFileContent_2_2_A &= "Type=1" & strSepString
						strFileContent_2_2_A &= "CAcc=" & Trim(Str(iADebitorMwStOp)) & strSepString
						strFileContent_2_2_A &= "BType=2" & strSepString
						strFileContent_2_2_A &= "ValNt=-" & Trim(Str(Format(cTotalTempBetrag_1, "f4"))) & strSepString
						strFileContent_2_2_A &= "ValTx=-" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
						strFileContent_2_2_A &= "Text=SK_Temporär_MwSt_" & rDbrec("_USFilialeNr").ToString.Trim & "}" & vbNewLine

					End If
					cTotalTempBetrag_0_1 -= cTotalTempBetrag
					cTotalTempBetrag_1_1 -= cTotalTempBetrag_1

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.Temporär Skonto: {1}", strMethodeName, ex.Message))
					MsgBox(ex.Message, MsgBoxStyle.Critical, "InsertOPRecValueToFile_6")

				End Try

				' --------------------------------------------------------------------------------------------------------------------
				Try
					strFileContent_2_2_I &= "//Ind Skonto" & strSepString & vbNewLine
					' Ind Skonto: {Bk AccId=3090 Type=1 CAcc=1100
					' TaxId=USt76 TIdx=4 CIdx=3 ValNt=-9.30 ValTx=-0.70 Text=TestsKonto}
					cTotalTempBetrag = Math.Round(CDbl(Val(rDbrec("KDTotalIndSKonto").ToString)), 4)
					cTotalTempBetrag_1 = Math.Round(cTotalTempBetrag * Me._dbMwSt / 100, 4)

					If cTotalTempBetrag <> 0 Then
						strFileContent_2_2_I &= "{BK AccId=" & Trim(Str(iSKontoIndMwStOp)) & strSepString
						strFileContent_2_2_I &= "Type=1" & strSepString
						strFileContent_2_2_I &= "CAcc=" & Trim(Str(iADebitorMwStOp)) & strSepString

						strFileContent_2_2_I &= String.Format("TaxId=USt{0}", Format(Me._dbMwSt * 10, "00")) & strSepString
						If Not String.IsNullOrEmpty(rDbrec("_USFilialeNr").ToString) Then
							strFileContent_2_2_I &= "TIdx=4" & strSepString
							strFileContent_2_2_I &= "CIdx=3" & strSepString
						End If
						strFileContent_2_2_I &= "ValNt=-" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
						strFileContent_2_2_I &= "ValTx=-" & Trim(Str(Format(cTotalTempBetrag_1, "f4"))) & strSepString
						strFileContent_2_2_I &= "Text=SK_Sonstige" & "}" & vbNewLine & Space(2)

						If Not String.IsNullOrEmpty(rDbrec("_USFilialeNr").ToString) Then
							' Kostenstelle exkl. MwSt.
							' {Bk AccId=920 Type=1 CAcc=3090 BType=1 ValNt=-9.30 Text=TestsKonto}
							strFileContent_2_2_I &= "{BK AccId=" & Str(rDbrec("_USFilialeNr").ToString.Trim) & strSepString
							strFileContent_2_2_I &= "Type=1" & strSepString
							strFileContent_2_2_I &= "CAcc=" & Trim(Str(iSKontoIndMwStOp)) & strSepString
							strFileContent_2_2_I &= "BType=1" & strSepString
							strFileContent_2_2_I &= "ValNt=-" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
							strFileContent_2_2_I &= "Text=SK_Sonstige_" & rDbrec("_USFilialeNr").ToString.Trim & "}" & vbNewLine & Space(2)
						End If
						' Kostenstelle MwSt. Satz
						' {Bk AccId=2200 Type=1 CAcc=1100 BType=2 ValNt=-0.70 ValTx=-9.30 Text=TestsKonto}
						strFileContent_2_2_I &= "{BK AccId=" & Trim(Str(iMwStOp)) & strSepString
						strFileContent_2_2_I &= "Type=1" & strSepString
						strFileContent_2_2_I &= "CAcc=" & Trim(Str(iADebitorMwStOp)) & strSepString
						strFileContent_2_2_I &= "BType=2" & strSepString
						strFileContent_2_2_I &= "ValNt=-" & Trim(Str(Format(cTotalTempBetrag_1, "f4"))) & strSepString
						strFileContent_2_2_I &= "ValTx=-" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
						strFileContent_2_2_I &= "Text=SK_Sonstige_MwSt_" & rDbrec("_USFilialeNr").ToString.Trim & "}" & vbNewLine

					End If
					cTotalTempBetrag_0_1 -= cTotalTempBetrag
					cTotalTempBetrag_1_1 -= cTotalTempBetrag_1

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.Ind Skonto: {1}", strMethodeName, ex.Message))
					MsgBox(ex.Message, MsgBoxStyle.Critical, "InsertOPRecValueToFile_7")

				End Try

				' --------------------------------------------------------------------------------------------------------------------
				Try
					strFileContent_2_2_F &= "//Fest Skonto" & strSepString & vbNewLine
					' Fest Skonto: {Bk AccId=3090 Type=1 CAcc=1100
					' TaxId=USt76 TIdx=4 CIdx=3 ValNt=-9.30 ValTx=-0.70 Text=TestsKonto}
					cTotalTempBetrag = Math.Round(CDbl(Val(rDbrec("KDTotalFSKonto").ToString)), 4)
					cTotalTempBetrag_1 = Math.Round(cTotalTempBetrag * Me._dbMwSt / 100, 4)

					If cTotalTempBetrag <> 0 Then
						strFileContent_2_2_F &= "{BK AccId=" & Trim(Str(iSKontoFestMwStOp)) & strSepString
						strFileContent_2_2_F &= "Type=1" & strSepString
						strFileContent_2_2_F &= "CAcc=" & Trim(Str(iADebitorMwStOp)) & strSepString

						strFileContent_2_2_F &= String.Format("TaxId=USt{0}", Format(Me._dbMwSt * 10, "00")) & strSepString
						If Not String.IsNullOrEmpty(rDbrec("_USFilialeNr").ToString) Then
							strFileContent_2_2_F &= "TIdx=4" & strSepString
							strFileContent_2_2_F &= "CIdx=3" & strSepString
						End If
						strFileContent_2_2_F &= "ValNt=-" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
						strFileContent_2_2_F &= "ValTx=-" & Trim(Str(Format(cTotalTempBetrag_1, "f4"))) & strSepString
						strFileContent_2_2_F &= "Text=SK_Fest" & "}" & vbNewLine & Space(2)

						If Not String.IsNullOrEmpty(rDbrec("_USFilialeNr").ToString) Then
							' Kostenstelle exkl. MwSt.
							' {Bk AccId=920 Type=1 CAcc=3090 BType=1 ValNt=-9.30 Text=TestsKonto}
							strFileContent_2_2_F &= "{BK AccId=" & Str(rDbrec("_USFilialeNr").ToString.Trim) & strSepString
							strFileContent_2_2_F &= "Type=1" & strSepString
							strFileContent_2_2_F &= "CAcc=" & Trim(Str(iSKontoFestMwStOp)) & strSepString
							strFileContent_2_2_F &= "BType=1" & strSepString
							strFileContent_2_2_F &= "ValNt=-" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
							strFileContent_2_2_F &= "Text=SK_Fest_" & rDbrec("_USFilialeNr").ToString.Trim & "}" & vbNewLine & Space(2)
						End If
						' Kostenstelle MwSt. Satz
						' {Bk AccId=2200 Type=1 CAcc=1100 BType=2 ValNt=-0.70 ValTx=-9.30 Text=TestsKonto}
						strFileContent_2_2_F &= "{BK AccId=" & Trim(Str(iMwStOp)) & strSepString
						strFileContent_2_2_F &= "Type=1" & strSepString
						strFileContent_2_2_F &= "CAcc=" & Trim(Str(iADebitorMwStOp)) & strSepString
						strFileContent_2_2_F &= "BType=2" & strSepString
						strFileContent_2_2_F &= "ValNt=-" & Trim(Str(Format(cTotalTempBetrag_1, "f4"))) & strSepString
						strFileContent_2_2_F &= "ValTx=-" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
						strFileContent_2_2_F &= "Text=SK_Fest_MwSt_" & rDbrec("_USFilialeNr").ToString.Trim & "}" & vbNewLine

					End If
					cTotalTempBetrag_0_1 -= cTotalTempBetrag
					cTotalTempBetrag_1_1 -= cTotalTempBetrag_1

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.Fest Skonto: {1}", strMethodeName, ex.Message))
					MsgBox(ex.Message, MsgBoxStyle.Critical, "InsertOPRecValueToFile_8")

				End Try

				' Ende der SKonto Minderung---------------------------------------------


				' Verlust Minderung
				' --------------------------------------------------------------------------------------------------------------------
				Try
					strFileContent_2_3_A &= "//Temporär Verlust" & strSepString & vbNewLine
					' Temporär Skonto: {Bk AccId=3090 Type=1 CAcc=1100
					' TaxId=USt76 TIdx=4 CIdx=3 ValNt=-9.30 ValTx=-0.70 Text=TestsKonto}
					cTotalTempBetrag = Math.Round(CDbl(Val(rDbrec("KDVerlustA").ToString)), 4)
					cTotalTempBetrag_1 = Math.Round(cTotalTempBetrag * Me._dbMwSt / 100, 4)

					If cTotalTempBetrag <> 0 Then
						strFileContent_2_3_A &= "{BK AccId=" & Trim(Str(iVerAMwStOp)) & strSepString
						strFileContent_2_3_A &= "Type=1" & strSepString
						strFileContent_2_3_A &= "CAcc=" & Trim(Str(iADebitorMwStOp)) & strSepString

						strFileContent_2_3_A &= String.Format("TaxId=USt{0}", Format(Me._dbMwSt * 10, "00")) & strSepString
						If Not String.IsNullOrEmpty(rDbrec("_USFilialeNr").ToString) Then
							strFileContent_2_3_A &= "TIdx=4" & strSepString
							strFileContent_2_3_A &= "CIdx=3" & strSepString
						End If
						strFileContent_2_3_A &= "ValNt=-" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
						strFileContent_2_3_A &= "ValTx=-" & Trim(Str(Format(cTotalTempBetrag_1, "f4"))) & strSepString
						strFileContent_2_3_A &= "Text=DV_Temporär" & "}" & vbNewLine & Space(2)

						If Not String.IsNullOrEmpty(rDbrec("_USFilialeNr").ToString) Then
							' Kostenstelle exkl. MwSt.
							' {Bk AccId=920 Type=1 CAcc=3090 BType=1 ValNt=-9.30 Text=TestVerlust}
							strFileContent_2_3_A &= "{BK AccId=" & Str(rDbrec("_USFilialeNr").ToString.Trim) & strSepString
							strFileContent_2_3_A &= "Type=1" & strSepString
							strFileContent_2_3_A &= "CAcc=" & Trim(Str(iVerAMwStOp)) & strSepString
							strFileContent_2_3_A &= "BType=1" & strSepString
							strFileContent_2_3_A &= "ValNt=-" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
							strFileContent_2_3_A &= "Text=DV_Temporär_" & rDbrec("_USFilialeNr").ToString.Trim & "}" & vbNewLine & Space(2)
						End If
						' Kostenstelle MwSt. Satz
						' {Bk AccId=2200 Type=1 CAcc=1100 BType=2 ValNt=-0.70 ValTx=-9.30 Text=TestVerlust}
						strFileContent_2_3_A &= "{BK AccId=" & Trim(Str(iMwStOp)) & strSepString
						strFileContent_2_3_A &= "Type=1" & strSepString
						strFileContent_2_3_A &= "CAcc=" & Trim(Str(iADebitorMwStOp)) & strSepString
						strFileContent_2_3_A &= "BType=2" & strSepString
						strFileContent_2_3_A &= "ValNt=-" & Trim(Str(Format(cTotalTempBetrag_1, "f4"))) & strSepString
						strFileContent_2_3_A &= "ValTx=-" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
						strFileContent_2_3_A &= "Text=DV_Temporär_MwSt_" & rDbrec("_USFilialeNr").ToString.Trim & "}" & vbNewLine

					End If
					cTotalTempBetrag_0_1 -= cTotalTempBetrag
					cTotalTempBetrag_1_1 -= cTotalTempBetrag_1

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.Temporär Verlust: {1}", strMethodeName, ex.Message))
					MsgBox(ex.Message, MsgBoxStyle.Critical, "InsertOPRecValueToFile_9")

				End Try

				' --------------------------------------------------------------------------------------------------------------------
				Try
					strFileContent_2_3_I &= "//Ind Verlust" & strSepString & vbNewLine
					' Ind Verlust: {Bk AccId=3090 Type=1 CAcc=1100
					' TaxId=USt76 TIdx=4 CIdx=3 ValNt=-9.30 ValTx=-0.70 Text=TestVerlust}
					cTotalTempBetrag = Math.Round(CDbl(Val(rDbrec("KDVerlustInd").ToString)), 4)
					cTotalTempBetrag_1 = Math.Round(cTotalTempBetrag * Me._dbMwSt / 100, 4)

					If cTotalTempBetrag <> 0 Then
						strFileContent_2_3_I &= "{BK AccId=" & Trim(Str(iVerIndMwStOp)) & strSepString
						strFileContent_2_3_I &= "Type=1" & strSepString
						strFileContent_2_3_I &= "CAcc=" & Trim(Str(iADebitorMwStOp)) & strSepString

						strFileContent_2_3_I &= String.Format("TaxId=USt{0}", Format(Me._dbMwSt * 10, "00")) & strSepString
						If Not String.IsNullOrEmpty(rDbrec("_USFilialeNr").ToString) Then
							strFileContent_2_3_I &= "TIdx=4" & strSepString
							strFileContent_2_3_I &= "CIdx=3" & strSepString
						End If
						strFileContent_2_3_I &= "ValNt=-" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
						strFileContent_2_3_I &= "ValTx=-" & Trim(Str(Format(cTotalTempBetrag_1, "f4"))) & strSepString
						strFileContent_2_3_I &= "Text=DV_Sonstige" & "}" & vbNewLine & Space(2)

						If Not String.IsNullOrEmpty(rDbrec("_USFilialeNr").ToString) Then
							' Kostenstelle exkl. MwSt.
							' {Bk AccId=920 Type=1 CAcc=3090 BType=1 ValNt=-9.30 Text=TestVerlust}
							strFileContent_2_3_I &= "{BK AccId=" & Str(rDbrec("_USFilialeNr").ToString.Trim) & strSepString
							strFileContent_2_3_I &= "Type=1" & strSepString
							strFileContent_2_3_I &= "CAcc=" & Trim(Str(iVerIndMwStOp)) & strSepString
							strFileContent_2_3_I &= "BType=1" & strSepString
							strFileContent_2_3_I &= "ValNt=-" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
							strFileContent_2_3_I &= "Text=DV_Sonstige_" & rDbrec("_USFilialeNr").ToString.Trim & "}" & vbNewLine & Space(2)
						End If
						' Kostenstelle MwSt. Satz
						' {Bk AccId=2200 Type=1 CAcc=1100 BType=2 ValNt=-0.70 ValTx=-9.30 Text=TestVerlust}
						strFileContent_2_3_I &= "{BK AccId=" & Trim(Str(iMwStOp)) & strSepString
						strFileContent_2_3_I &= "Type=1" & strSepString
						strFileContent_2_3_I &= "CAcc=" & Trim(Str(iADebitorMwStOp)) & strSepString
						strFileContent_2_3_I &= "BType=2" & strSepString
						strFileContent_2_3_I &= "ValNt=-" & Trim(Str(Format(cTotalTempBetrag_1, "f4"))) & strSepString
						strFileContent_2_3_I &= "ValTx=-" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
						strFileContent_2_3_I &= "Text=DV_Sonstige_MwSt_" & rDbrec("_USFilialeNr").ToString.Trim & "}" & vbNewLine

					End If
					cTotalTempBetrag_0_1 -= cTotalTempBetrag
					cTotalTempBetrag_1_1 -= cTotalTempBetrag_1

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.Ind Verlust: {1}", strMethodeName, ex.Message))
					MsgBox(ex.Message, MsgBoxStyle.Critical, "InsertOPRecValueToFile_10")

				End Try

				' --------------------------------------------------------------------------------------------------------------------
				Try
					strFileContent_2_3_F &= "//Fest Verlust" & strSepString & vbNewLine
					' Fest Verlust: {Bk AccId=3090 Type=1 CAcc=1100
					' TaxId=USt76 TIdx=4 CIdx=3 ValNt=-9.30 ValTx=-0.70 Text=TestVerlust}
					cTotalTempBetrag = Math.Round(CDbl(Val(rDbrec("KDVerlustF").ToString)), 4)
					cTotalTempBetrag_1 = Math.Round(cTotalTempBetrag * Me._dbMwSt / 100, 4)

					If cTotalTempBetrag <> 0 Then
						strFileContent_2_3_F &= "{BK AccId=" & Trim(Str(iVerFMwStOp)) & strSepString
						strFileContent_2_3_F &= "Type=1" & strSepString
						strFileContent_2_3_F &= "CAcc=" & Trim(Str(iADebitorMwStOp)) & strSepString

						strFileContent_2_3_F &= String.Format("TaxId=USt{0}", Format(Me._dbMwSt * 10, "00")) & strSepString
						If Not String.IsNullOrEmpty(rDbrec("_USFilialeNr").ToString) Then
							strFileContent_2_3_F &= "TIdx=4" & strSepString
							strFileContent_2_3_F &= "CIdx=3" & strSepString
						End If
						strFileContent_2_3_F &= "ValNt=-" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
						strFileContent_2_3_F &= "ValTx=-" & Trim(Str(Format(cTotalTempBetrag_1, "f4"))) & strSepString
						strFileContent_2_3_F &= "Text=DV_Fest" & "}" & vbNewLine & Space(2)

						If Not String.IsNullOrEmpty(rDbrec("_USFilialeNr").ToString) Then
							' Kostenstelle exkl. MwSt.
							' {Bk AccId=920 Type=1 CAcc=3090 BType=1 ValNt=-9.30 Text=TestVerlust}
							strFileContent_2_3_F &= "{BK AccId=" & Str(rDbrec("_USFilialeNr").ToString.Trim) & strSepString
							strFileContent_2_3_F &= "Type=1" & strSepString
							strFileContent_2_3_F &= "CAcc=" & Trim(Str(iVerFMwStOp)) & strSepString
							strFileContent_2_3_F &= "BType=1" & strSepString
							strFileContent_2_3_F &= "ValNt=-" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
							strFileContent_2_3_F &= "Text=DV_Fest_" & rDbrec("_USFilialeNr").ToString.Trim & "}" & vbNewLine & Space(2)
						End If
						' Kostenstelle MwSt. Satz
						' {Bk AccId=2200 Type=1 CAcc=1100 BType=2 ValNt=-0.70 ValTx=-9.30 Text=TestVerlust}
						strFileContent_2_3_F &= "{BK AccId=" & Trim(Str(iMwStOp)) & strSepString
						strFileContent_2_3_F &= "Type=1" & strSepString
						strFileContent_2_3_F &= "CAcc=" & Trim(Str(iADebitorMwStOp)) & strSepString
						strFileContent_2_3_F &= "BType=2" & strSepString
						strFileContent_2_3_F &= "ValNt=-" & Trim(Str(Format(cTotalTempBetrag_1, "f4"))) & strSepString
						strFileContent_2_3_F &= "ValTx=-" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
						strFileContent_2_3_F &= "Text=DV_Fest_MwSt_" & rDbrec("_USFilialeNr").ToString.Trim & "}" & vbNewLine

					End If
					cTotalTempBetrag_0_1 -= cTotalTempBetrag
					cTotalTempBetrag_1_1 -= cTotalTempBetrag_1

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.Fest Verlust: {1}", strMethodeName, ex.Message))
					MsgBox(ex.Message, MsgBoxStyle.Critical, "InsertOPRecValueToFile_11")

				End Try

				' Ende der Verlust Minderung---------------------------------------------

				' Rückvergütung Minderung
				' --------------------------------------------------------------------------------------------------------------------
				Try
					strFileContent_2_4_A &= "//Temporär Rückvergütung" & strSepString & vbNewLine
					' Temporär Rückvergütung: {Bk AccId=3090 Type=1 CAcc=1100
					' TaxId=USt76 TIdx=4 CIdx=3 ValNt=-9.30 ValTx=-0.70 Text=TestRückvergütung}
					cTotalTempBetrag = Math.Round(CDbl(Val(rDbrec("KDRuA").ToString)), 4)
					cTotalTempBetrag_1 = Math.Round(cTotalTempBetrag * Me._dbMwSt / 100, 4)

					If cTotalTempBetrag <> 0 Then
						strFileContent_2_4_A &= "{BK AccId=" & Trim(Str(iRuAMwStOp)) & strSepString
						strFileContent_2_4_A &= "Type=1" & strSepString
						strFileContent_2_4_A &= "CAcc=" & Trim(Str(iADebitorMwStOp)) & strSepString

						strFileContent_2_4_A &= String.Format("TaxId=USt{0}", Format(Me._dbMwSt * 10, "00")) & strSepString
						If Not String.IsNullOrEmpty(rDbrec("_USFilialeNr").ToString) Then
							strFileContent_2_4_A &= "TIdx=4" & strSepString
							strFileContent_2_4_A &= "CIdx=3" & strSepString
						End If
						strFileContent_2_4_A &= "ValNt=-" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
						strFileContent_2_4_A &= "ValTx=-" & Trim(Str(Format(cTotalTempBetrag_1, "f4"))) & strSepString
						strFileContent_2_4_A &= "Text=RV_Temporär" & "}" & vbNewLine & Space(2)

						If Not String.IsNullOrEmpty(rDbrec("_USFilialeNr").ToString) Then
							' Kostenstelle exkl. MwSt.
							' {Bk AccId=920 Type=1 CAcc=3090 BType=1 ValNt=-9.30 Text=TestRückvergütung}
							strFileContent_2_4_A &= "{BK AccId=" & Str(rDbrec("_USFilialeNr").ToString.Trim) & strSepString
							strFileContent_2_4_A &= "Type=1" & strSepString
							strFileContent_2_4_A &= "CAcc=" & Trim(Str(iRuAMwStOp)) & strSepString
							strFileContent_2_4_A &= "BType=1" & strSepString
							strFileContent_2_4_A &= "ValNt=-" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
							strFileContent_2_4_A &= "Text=RV_Temporär_" & rDbrec("_USFilialeNr").ToString.Trim & "}" & vbNewLine & Space(2)
						End If
						' Kostenstelle MwSt. Satz
						' {Bk AccId=2200 Type=1 CAcc=1100 BType=2 ValNt=-0.70 ValTx=-9.30 Text=TestRückvergütung}
						strFileContent_2_4_A &= "{BK AccId=" & Trim(Str(iMwStOp)) & strSepString
						strFileContent_2_4_A &= "Type=1" & strSepString
						strFileContent_2_4_A &= "CAcc=" & Trim(Str(iADebitorMwStOp)) & strSepString
						strFileContent_2_4_A &= "BType=2" & strSepString
						strFileContent_2_4_A &= "ValNt=-" & Trim(Str(Format(cTotalTempBetrag_1, "f4"))) & strSepString
						strFileContent_2_4_A &= "ValTx=-" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
						strFileContent_2_4_A &= "Text=RV_Temporär_MwSt_" & rDbrec("_USFilialeNr").ToString.Trim & "}" & vbNewLine

					End If
					cTotalTempBetrag_0_1 -= cTotalTempBetrag
					cTotalTempBetrag_1_1 -= cTotalTempBetrag_1

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.Temporär Rückvergütung: {1}", strMethodeName, ex.Message))
					MsgBox(ex.Message, MsgBoxStyle.Critical, "InsertOPRecValueToFile_12")

				End Try

				' --------------------------------------------------------------------------------------------------------------------
				Try
					strFileContent_2_4_I &= "//Ind Rückvergütung" & strSepString & vbNewLine
					' Ind Rückvergütung: {Bk AccId=3090 Type=1 CAcc=1100
					' TaxId=USt76 TIdx=4 CIdx=3 ValNt=-9.30 ValTx=-0.70 Text=TestRückvergütung}
					cTotalTempBetrag = Math.Round(CDbl(Val(rDbrec("KDRuInd").ToString)), 4)
					cTotalTempBetrag_1 = Math.Round(cTotalTempBetrag * Me._dbMwSt / 100, 4)

					If cTotalTempBetrag <> 0 Then
						strFileContent_2_4_I &= "{BK AccId=" & Trim(Str(iRuIndMwStOp)) & strSepString
						strFileContent_2_4_I &= "Type=1" & strSepString
						strFileContent_2_4_I &= "CAcc=" & Trim(Str(iADebitorMwStOp)) & strSepString

						strFileContent_2_4_I &= String.Format("TaxId=USt{0}", Format(Me._dbMwSt * 10, "00")) & strSepString
						If Not String.IsNullOrEmpty(rDbrec("_USFilialeNr").ToString) Then
							strFileContent_2_4_I &= "TIdx=4" & strSepString
							strFileContent_2_4_I &= "CIdx=3" & strSepString
						End If
						strFileContent_2_4_I &= "ValNt=-" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
						strFileContent_2_4_I &= "ValTx=-" & Trim(Str(Format(cTotalTempBetrag_1, "f4"))) & strSepString
						strFileContent_2_4_I &= "Text=RV_Sonstige" & "}" & vbNewLine & Space(2)

						If Not String.IsNullOrEmpty(rDbrec("_USFilialeNr").ToString) Then
							' Kostenstelle exkl. MwSt.
							' {Bk AccId=920 Type=1 CAcc=3090 BType=1 ValNt=-9.30 Text=TestRückvergütung}
							strFileContent_2_4_I &= "{BK AccId=" & Str(rDbrec("_USFilialeNr").ToString.Trim) & strSepString
							strFileContent_2_4_I &= "Type=1" & strSepString
							strFileContent_2_4_I &= "CAcc=" & Trim(Str(iRuIndMwStOp)) & strSepString
							strFileContent_2_4_I &= "BType=1" & strSepString
							strFileContent_2_4_I &= "ValNt=-" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
							strFileContent_2_4_I &= "Text=RV_Sonstige_" & rDbrec("_USFilialeNr").ToString.Trim & "}" & vbNewLine & Space(2)
						End If
						' Kostenstelle MwSt. Satz
						' {Bk AccId=2200 Type=1 CAcc=1100 BType=2 ValNt=-0.70 ValTx=-9.30 Text=TestRückvergütung}
						strFileContent_2_4_I &= "{BK AccId=" & Trim(Str(iMwStOp)) & strSepString
						strFileContent_2_4_I &= "Type=1" & strSepString
						strFileContent_2_4_I &= "CAcc=" & Trim(Str(iADebitorMwStOp)) & strSepString
						strFileContent_2_4_I &= "BType=2" & strSepString
						strFileContent_2_4_I &= "ValNt=-" & Trim(Str(Format(cTotalTempBetrag_1, "f4"))) & strSepString
						strFileContent_2_4_I &= "ValTx=-" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
						strFileContent_2_4_I &= "Text=RV_Sonstige_MwSt_" & rDbrec("_USFilialeNr").ToString.Trim & "}" & vbNewLine

					End If
					cTotalTempBetrag_0_1 -= cTotalTempBetrag
					cTotalTempBetrag_1_1 -= cTotalTempBetrag_1

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.Ind Rückvergütung: {1}", strMethodeName, ex.Message))
					MsgBox(ex.Message, MsgBoxStyle.Critical, "InsertOPRecValueToFile_13")

				End Try

				' --------------------------------------------------------------------------------------------------------------------
				Try
					strFileContent_2_4_F &= "//Fest Rückvergütung" & strSepString & vbNewLine
					' Fest Rückvergütung: {Bk AccId=3090 Type=1 CAcc=1100
					' TaxId=USt76 TIdx=4 CIdx=3 ValNt=-9.30 ValTx=-0.70 Text=TestRückvergütung}
					cTotalTempBetrag = Math.Round(CDbl(Val(rDbrec("KDRuF").ToString)), 4)
					cTotalTempBetrag_1 = Math.Round(cTotalTempBetrag * Me._dbMwSt / 100, 4)

					If cTotalTempBetrag <> 0 Then
						strFileContent_2_4_F &= "{BK AccId=" & Trim(Str(iRuFMwStOp)) & strSepString
						strFileContent_2_4_F &= "Type=1" & strSepString
						strFileContent_2_4_F &= "CAcc=" & Trim(Str(iADebitorMwStOp)) & strSepString

						strFileContent_2_4_F &= String.Format("TaxId=USt{0}", Format(Me._dbMwSt * 10, "00")) & strSepString
						If Not String.IsNullOrEmpty(rDbrec("_USFilialeNr").ToString) Then
							strFileContent_2_4_F &= "TIdx=4" & strSepString
							strFileContent_2_4_F &= "CIdx=3" & strSepString
						End If
						strFileContent_2_4_F &= "ValNt=-" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
						strFileContent_2_4_F &= "ValTx=-" & Trim(Str(Format(cTotalTempBetrag_1, "f4"))) & strSepString
						strFileContent_2_4_F &= "Text=RV_Fest" & "}" & vbNewLine & Space(2)

						If Not String.IsNullOrEmpty(rDbrec("_USFilialeNr").ToString) Then
							' Kostenstelle exkl. MwSt.
							' {Bk AccId=920 Type=1 CAcc=3090 BType=1 ValNt=-9.30 Text=TestRückvergütung}
							strFileContent_2_4_F &= "{BK AccId=" & Str(rDbrec("_USFilialeNr").ToString.Trim) & strSepString
							strFileContent_2_4_F &= "Type=1" & strSepString
							strFileContent_2_4_F &= "CAcc=" & Trim(Str(iRuFMwStOp)) & strSepString
							strFileContent_2_4_F &= "BType=1" & strSepString
							strFileContent_2_4_F &= "ValNt=-" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
							strFileContent_2_4_F &= "Text=RV_Fest_" & rDbrec("_USFilialeNr").ToString.Trim & "}" & vbNewLine & Space(2)
						End If
						' Kostenstelle MwSt. Satz
						' {Bk AccId=2200 Type=1 CAcc=1100 BType=2 ValNt=-0.70 ValTx=-9.30 Text=TestRückvergütung}
						strFileContent_2_4_F &= "{BK AccId=" & Trim(Str(iMwStOp)) & strSepString
						strFileContent_2_4_F &= "Type=1" & strSepString
						strFileContent_2_4_F &= "CAcc=" & Trim(Str(iADebitorMwStOp)) & strSepString
						strFileContent_2_4_F &= "BType=2" & strSepString
						strFileContent_2_4_F &= "ValNt=-" & Trim(Str(Format(cTotalTempBetrag_1, "f4"))) & strSepString
						strFileContent_2_4_F &= "ValTx=-" & Trim(Str(Format(cTotalTempBetrag, "f4"))) & strSepString
						strFileContent_2_4_F &= "Text=RV_Fest_MwSt_" & rDbrec("_USFilialeNr").ToString.Trim & "}" & vbNewLine

					End If
					cTotalTempBetrag_0_1 -= cTotalTempBetrag
					cTotalTempBetrag_1_1 -= cTotalTempBetrag_1

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.Fest Rückvergütung: {1}", strMethodeName, ex.Message))
					MsgBox(ex.Message, MsgBoxStyle.Critical, "InsertOPRecValueToFile_14")

				End Try

				' Ende der Rückvergütung Minderung---------------------------------------------

				strFileContent_0 &= strFileContent_2_A & _
														strFileContent_2_I & _
														strFileContent_2_F & _
														strFileContent_2_1_A & _
														strFileContent_2_1_I & _
														strFileContent_2_1_F & _
														strFileContent_2_2_A & _
														strFileContent_2_2_I & _
														strFileContent_2_2_F & _
														strFileContent_2_3_A & _
														strFileContent_2_3_I & _
														strFileContent_2_3_F & _
														strFileContent_2_4_A & _
														strFileContent_2_4_I & _
														strFileContent_2_4_F & vbNewLine

				strFileContent_2_A = String.Empty						' Debitoren A
				strFileContent_2_I = String.Empty						' Debitoren Ind
				strFileContent_2_F = String.Empty						' Debitoren Fest

				strFileContent_2_1_A = String.Empty					' Gutschrift A
				strFileContent_2_1_I = String.Empty					' Gutschrift Ind
				strFileContent_2_1_F = String.Empty					' Gutschrift Fest

				strFileContent_2_2_A = String.Empty					' SKonto A
				strFileContent_2_2_I = String.Empty					' SKonto Ind
				strFileContent_2_2_F = String.Empty					' SKonto Fest

				strFileContent_2_3_A = String.Empty					' Verlust A
				strFileContent_2_3_I = String.Empty					' Verlust Ind
				strFileContent_2_3_F = String.Empty					' Verlust Fest

				strFileContent_2_4_A = String.Empty					' Rückvergütung A
				strFileContent_2_4_I = String.Empty					' Rückvergütung Ind
				strFileContent_2_4_F = String.Empty					' Rückvergütung Fest


			End While
			strFileContent_0 &= "}"
			Try
				If cTotalTempBetrag_0_1 <> 0 Then
					Dim strMyBetrag_0 As String() = Str(cTotalTempBetrag_0_1 + cTotalTempBetrag_1_1).Split(CChar("."))
					Dim strTestHeader As String = String.Format("ValNt={0}{1}", _
																											cTotalTempBetrag_0_1 + cTotalTempBetrag_1_1, strSepString)

					strFileContent_0 = strFileContent_0.Replace("#ValNt=#", strTestHeader)

				Else
					Dim strMyBetrag_0 As String() = Str(cTotalTempBetrag_0_1 + cTotalTempBetrag_1_1).Split(CChar("."))
					Dim strTestHeader As String = "ValNt=0.0" & strSepString
					strFileContent_0 = strFileContent_0.Replace("#ValNt=#", strTestHeader)

				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Zusammenfassung: {1}", strMethodeName, ex.Message))
				MsgBox(ex.Message, MsgBoxStyle.Critical, "InsertOPRecValueToFile_15")

			End Try

			Return strFileContent_0
		End Function

		Function InsertLORecValueToFile() As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim sSql As String
			Dim cLABetragTotal As Double

			Dim strSqlQuery As String = Me._ExportSetting.SQL2Open
			Dim Conn As SqlConnection = New SqlConnection(Me._ExportSetting.DbConnString2Open)

			Dim strFileContent_1 As String = String.Empty
			Dim strFileContent_1_2 As String
			'Dim strTableName As String = String.Format("[_LOLFibu_{0}]", _ClsProgSetting.GetLogedUSGuid)
			Dim strTableName As String = String.Format("[{0}{1}]", m_tablename, _ClsProgSetting.GetLogedUSGuid)

			Dim i As Integer = 2
			'If clsmainsetting.IsNewVersion Then
			sSql = "[dbo].[Create Table For New_LOLFibu]"
			'strTableName = "_" & strTableName
			'Else
			'sSql = "[dbo].[Create Table For UJ_LOLFibu]"
			'strTableName = strTableName

			'End If

			Conn.Open()
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@Jahr", Me._ExportSetting.SelectedYear)
			param = cmd.Parameters.AddWithValue("@tblName", strTableName)

			Dim rDbrec As SqlDataReader = cmd.ExecuteReader


			strFileContent_1 = "//Lohndaten werden importiert..." & vbNewLine
			While rDbrec.Read()
				If rDbrec("LANR") = 8000.021 Then
					Trace.WriteLine(rDbrec("LANR"))
				End If
				' {Blg GFNr=234 Date=31.01.08 Grp=S Orig=0 MType=2
				strFileContent_1 &= "{Blg GFNr=" & i & strSepString
				strFileContent_1 &= "Date=" & Me.MDDate.ToShortDateString & strSepString
				strFileContent_1 &= "Grp=S" & strSepString
				strFileContent_1 &= "Orig=0" & strSepString
				strFileContent_1 &= "MType=2" & vbNewLine

				'If Me._strFiliale.Count > 1 Then
				'  strFileContent_1_2 = GetLOLRecValueToFile(CDbl(rDbrec("LANr").ToString))
				'  cLABetragTotal = Me._dbTotalBetrag

				'Else
				'  strFileContent_1_2 = String.Empty
				'  cLABetragTotal = CDbl(rDbrec("TotalBetrag").ToString)

				'End If

				strFileContent_1_2 = GetLOLRecValueToFile(CDbl(rDbrec("LANr").ToString))
				cLABetragTotal = Me._dbTotalBetrag

				' {Bk AccId=4000 Type=1 ValNt=1000.00 Text=Stundenlohn}
				strFileContent_1 &= "{Bk AccId=" & rDbrec("HKonto").ToString.Trim & strSepString
				strFileContent_1 &= "Type=1" & strSepString
				strFileContent_1 &= "CAcc=div" & strSepString
				strFileContent_1 &= "ValNt=" & cLABetragTotal.ToString.Trim & strSepString
				strFileContent_1 &= "Text=" & Chr(34) & rDbrec("Bezeichnung").ToString & Chr(34) & "}" & vbNewLine

				strFileContent_1 &= strFileContent_1_2 & vbNewLine

				i = i + 1
			End While

			Return strFileContent_1
		End Function

		Function GetLOLRecValueToFile(ByVal dLANr As Double) As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim sSql As String
			Dim strResult As String = String.Empty
			Dim cFilialeBetrag As Double
			Dim dTotalBetrag As Double = 0
			Dim strVorzeichen_2 As String = String.Empty
			Dim strVorzeichen_3 As String = String.Empty

			Dim strSqlQuery As String = Me._ExportSetting.SQL2Open
			Dim Conn As SqlConnection = New SqlConnection(Me._ExportSetting.DbConnString2Open)
			Dim strFileContent_1 As String = String.Empty
			Dim strFileContent_1_2 As String = Space(2)
			Dim strTableName As String = String.Format("[{0}{1}]", m_tablename, _ClsProgSetting.GetLogedUSGuid)

			sSql = "[dbo].[Get LAValues From New_LOLFibu_]"

			Conn.Open()
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@tblName", strTableName)
			param = cmd.Parameters.AddWithValue("@LANr", dLANr)

			Dim rDbrec As SqlDataReader = cmd.ExecuteReader

			If dLANr = 8100 Then
				Trace.WriteLine(dLANr)
			End If

			'If clsmainsetting.IsNewVersion Then
			While rDbrec.Read
				strVorzeichen_2 = rDbrec("Vorzeichen_2").ToString
				strVorzeichen_3 = rDbrec("Vorzeichen_3").ToString
				cFilialeBetrag = GetLAValueWithVorZeichen(Val(rDbrec("LABetrag").ToString), _
																						 strVorzeichen_2, _
																						 strVorzeichen_3)
				'        dTotalBetrag += Val(rDbrec("LABetrag").ToString)
				dTotalBetrag += Val(cFilialeBetrag)

				' {Bk AccId=2090 Type=1 CAcc=4000 TIdx=4 CIdx=3 ValNt=500 Text=Testrechung12}
				strFileContent_1_2 &= "{Bk AccId=" & rDbrec("SKonto").ToString.Trim & strSepString
				strFileContent_1_2 &= "Type=0" & strSepString
				strFileContent_1_2 &= "CAcc=" & rDbrec("HKonto").ToString.Trim & strSepString
				strFileContent_1_2 &= "ValNt=" & cFilialeBetrag.ToString.Trim & strSepString
				strFileContent_1_2 &= "Text=" & Chr(34) & rDbrec("Bezeichnung").ToString & " " & _
															rDbrec("Filiale_1").ToString.Trim & Chr(34) & "}" & vbNewLine & Space(2)

				If rDbrec("FilialeNr_1").ToString <> "0" And Not CBool(rDbrec("Reserve2")) Then
					' {Bk AccId=920 Type=1 CAcc=2090 BType=1 ValNt=500.00 Text=Testrechung12}
					strFileContent_1_2 &= "{Bk AccId=" & rDbrec("FilialeNr_1").ToString & strSepString
					strFileContent_1_2 &= "Type=0" & strSepString
					strFileContent_1_2 &= "CAcc=" & rDbrec("SKonto").ToString.Trim & strSepString
					strFileContent_1_2 &= "BType=1" & strSepString
					strFileContent_1_2 &= "ValNt=" & cFilialeBetrag.ToString.Trim & strSepString
					strFileContent_1_2 &= "Text=" & Chr(34) & rDbrec("Bezeichnung").ToString & " " & _
															rDbrec("Filiale_1").ToString.Trim & Chr(34) & "}" & vbNewLine & Space(2)
				End If

			End While

			' Der berechnete Wert darf nicht noch einmal umgewandelt werden.
			Me._dbTotalBetrag = dTotalBetrag
			strFileContent_1 &= strFileContent_1_2 & "}" & vbNewLine

			Return strFileContent_1
		End Function

		Function GetLAValueWithVorZeichen(ByVal myValue As Double, _
																			ByVal strVorzeichen_2 As String, _
																			ByVal strVorzeichen_3 As String) As Double
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim dMyValue As Double = myValue

			' Den Betrag gemäss Vorzeichen_2, Vorzeichen_3 anpassen
			If dMyValue > 0 Then
				' Vorzeichen_2
				If strVorzeichen_2 = "-" Then
					dMyValue *= -1
				End If

			ElseIf dMyValue < 0 Then
				' Vorzeichen_3
				If strVorzeichen_3 <> "-" Then
					dMyValue *= -1
				End If

			End If

			Return dMyValue
		End Function

		Function GetKDBetrag() As Double
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim dTotalUmsatz As Double = 0
			Dim Conn As SqlConnection = New SqlConnection(Me._ExportSetting.DbConnString2Open)
			Dim strResult As String = String.Empty

			Conn.Open()

			strResult = "Select sum(KDTotal) as KDTotal, sum(KDTotalInd) As KDTotalInd, sum(KDTotalFest) As KDTotalFest, "

			strResult += "sum(KDGuA) as KDGuA, sum(KDGuInd) as KDGuInd, sum(KDGuF) as KDGuF, "
			strResult += "sum(KDTotalTempSKonto) As KDTotalTempSKonto, sum(KDTotalIndSKonto) As KDTotalIndSKonto, "
			strResult += "sum(KDTotalFSKonto) As KDTotalFSKonto, "
			strResult += "sum(KDVerlustA) as KDVerlustA, sum(KDVerlustInd) as KDVerlustInd, sum(KDVerlustF) as KDVerlustF, "
			strResult += "sum(KDRuA) as KDRuA, sum(KDRuInd) as KDRuInd, sum(KDRuF) as KDRuF, "

			strResult += "sum(KDTotalFOp) as KDTotalFOp "

			strResult &= String.Format("From [UmsatzJournal_New_{0}] ", _ClsProgSetting.GetLogedUSGuid)

			Try
				Dim cmd As New SqlCommand(strResult, Conn)
				Dim rDbrec As SqlDataReader = cmd.ExecuteReader

				While rDbrec.Read
					dTotalUmsatz = Val(rDbrec("KDTotal").ToString) + _
													Val(rDbrec("KDTotalInd").ToString) + _
													Val(rDbrec("KDTotalFest").ToString)

					dTotalUmsatz -= (Val(rDbrec("KDGuA").ToString) + Val(rDbrec("KDGuInd").ToString) + Val(rDbrec("KDGuF").ToString) + _
													Val(rDbrec("KDVerlustA").ToString) + Val(rDbrec("KDVerlustInd").ToString) + _
													Val(rDbrec("KDVerlustF").ToString) + _
													Val(rDbrec("KDRuA").ToString) + Val(rDbrec("KDRuInd").ToString) + Val(rDbrec("KDRuF").ToString) + _
													Val(rDbrec("KDTotalTempSKonto").ToString) + _
													Val(rDbrec("KDTotalIndskonto").ToString) + Val(rDbrec("KDTotalFskonto").ToString)) ' + _
					'Val(rDbrec("KDTotalTemperlos").ToString) + _
					'Val(rDbrec("KDTotalInderlos").ToString) + Val(rDbrec("KDTotalFerlos").ToString) )

				End While
				dTotalUmsatz = Math.Round(dTotalUmsatz, 4)

			Catch ex As Exception
				MsgBox(ex.Message, MsgBoxStyle.Critical, "GetKDBetrag")

			Finally

			End Try

			Return dTotalUmsatz
		End Function

		Function WriteContent2File(ByVal msg As String, ByVal strFullFilename As String) As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strPathForFiles As String = String.Empty
			Dim strResult As String = "Success"

			Try
				If strFullFilename = "" Then
					strPathForFiles = String.Format("{0}Sesam_temp.txt", _ClsProgSetting.GetSpSTempPath)

				Else

					Try
						Kill(strFullFilename)

					Catch ex As Exception
						m_Logger.LogError(String.Format("{0}.Datei löschen: {1}", strMethodeName, ex.Message))

					End Try

				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Dateiname festlegen: {1}", strMethodeName, ex.Message))
				Return String.Format("Error: {0}.Dateiname festlegen: {1}", strMethodeName, ex.Message)

			End Try

			Try
				Dim MyFile As FileInfo = New FileInfo(strFullFilename)
				If Not MyFile.Directory.Exists Then MyFile.Directory.Create()

				Dim bw As BinaryWriter = New BinaryWriter(File.OpenWrite(strFullFilename))
				Dim NewContent() As Byte = Encoding.Default.GetBytes(msg)
				bw.Write(NewContent)
				bw.Close()
				strResult = strFullFilename

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
				Return String.Format("Error: {0}.{1}", strMethodeName, ex.Message)

			End Try

			Return strResult
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


End Namespace
