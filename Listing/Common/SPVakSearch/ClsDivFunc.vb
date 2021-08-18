
Imports System.IO
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions

Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities

Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SP.Infrastructure.Logging

Public Class ClsDivFunc

#Region "Diverses"

	'// Get4What._strModul4What
	Dim _strModul4What As String
	Public Property Get4What() As String
		Get
			Return _strModul4What
		End Get
		Set(ByVal value As String)
			_strModul4What = value
		End Set
	End Property

	'// Query.GetSearchQuery
	Dim _strQuery As String
	Public Property GetSearchQuery() As String
		Get
			Return _strQuery
		End Get
		Set(ByVal value As String)
			_strQuery = value
		End Set
	End Property

	'// LargerLV
	Dim _bLargerLV As Boolean
	Public Property GetLargerLV() As Boolean
		Get
			Return _bLargerLV
		End Get
		Set(ByVal value As Boolean)
			_bLargerLV = value
		End Set
	End Property


#End Region

#Region "Funktionen für LvClick in der Suchmaske..."

	'// Allgemeiner Zwischenspeicher
	Dim _strSelektion As String
	Public Property GetSelektion() As String
		Get
			Return _strSelektion
		End Get
		Set(ByVal value As String)
			_strSelektion = value
		End Set
	End Property

	' // ID
	Dim _strID As String
	Public Property GetID() As String
		Get
			Return _strID
		End Get
		Set(ByVal value As String)
			_strID = value
		End Set
	End Property

	' // VakNr
	Dim _strVakNr As String
	Public Property GetVakNr() As String
		Get
			Return _strVakNr
		End Get
		Set(ByVal value As String)
			_strVakNr = value
		End Set
	End Property

	' // Bezeichnung
	Dim _strBezeichnung As String
	Public Property GetBezeichnung() As String
		Get
			Return _strBezeichnung
		End Get
		Set(ByVal value As String)
			_strBezeichnung = value
		End Set
	End Property

	' // ESNr
	Dim _strESNr As String
	Public Property GetESNr() As String
		Get
			Return _strESNr
		End Get
		Set(ByVal value As String)
			_strESNr = value
		End Set
	End Property

	'// MANr
	Dim _strMANr As String
	Public Property GetMANr() As String
		Get
			Return _strMANr
		End Get
		Set(ByVal value As String)
			_strMANr = value
		End Set
	End Property

	'// KDNr
	Dim _strKDNr As String
	Public Property GetKDNr() As String
		Get
			Return _strKDNr
		End Get
		Set(ByVal value As String)
			_strKDNr = value
		End Set
	End Property

	'// Firma
	Dim _strFirma As String
	Public Property GetFirma() As String
		Get
			Return _strFirma
		End Get
		Set(ByVal value As String)
			_strFirma = value
		End Set
	End Property

	'// Zuständige Person
	Dim _strKDZHDNr As String
	Public Property GetKDZHDNr() As String
		Get
			Return _strKDZHDNr
		End Get
		Set(ByVal value As String)
			_strKDZHDNr = value
		End Set
	End Property

	'// Kunden Telefon
	Dim _strKDTelefon As String
	Public Property GetKDTelefon() As String
		Get
			Return _strKDTelefon
		End Get
		Set(ByVal value As String)
			_strKDTelefon = value
		End Set
	End Property

	'// Zuständige Person Telefon
	Dim _strKDZTelefon As String
	Public Property GetKDZTelefon() As String
		Get
			Return _strKDZTelefon
		End Get
		Set(ByVal value As String)
			_strKDZTelefon = value
		End Set
	End Property

	'// Zuständige Person Telefon
	Dim _strKDZNatel As String
	Public Property GetKDZNatel() As String
		Get
			Return _strKDZNatel
		End Get
		Set(ByVal value As String)
			_strKDZNatel = value
		End Set
	End Property

	'// Zuständige Person Name
	Dim _strKDZHDName As String
	Public Property GetKDZHDName() As String
		Get
			Return _strKDZHDName
		End Get
		Set(ByVal value As String)
			_strKDZHDName = value
		End Set
	End Property

#End Region

#Region "LL_Properties"
	'// Print.LLDocName
	Dim _LLDocName As String
	Public Property LLDocName() As String
		Get
			Return _LLDocName
		End Get
		Set(ByVal value As String)
			_LLDocName = value
		End Set
	End Property

	'// Print.LLDocLabel
	Dim _LLDocLabel As String
	Public Property LLDocLabel() As String
		Get
			Return _LLDocLabel
		End Get
		Set(ByVal value As String)
			_LLDocLabel = value
		End Set
	End Property

	'// Print.LLFontDesent
	Dim _LLFontDesent As Integer
	Public Property LLFontDesent() As Integer
		Get
			Return _LLFontDesent
		End Get
		Set(ByVal value As Integer)
			_LLFontDesent = value
		End Set
	End Property

	'// Print.LLIncPrv
	Dim _LLIncPrv As Integer
	Public Property LLIncPrv() As Integer
		Get
			Return _LLIncPrv
		End Get
		Set(ByVal value As Integer)
			_LLIncPrv = value
		End Set
	End Property

	'// Print.LLParamCheck
	Dim _LLParamCheck As Integer
	Public Property LLParamCheck() As Integer
		Get
			Return _LLParamCheck
		End Get
		Set(ByVal value As Integer)
			_LLParamCheck = value
		End Set
	End Property

	'// Print.LLKonvertName
	Dim _LLKonvertName As Integer
	Public Property LLKonvertName() As Integer
		Get
			Return _LLKonvertName
		End Get
		Set(ByVal value As Integer)
			_LLKonvertName = value
		End Set
	End Property

	'// Print.LLZoomProz
	Dim _LLZoomProz As Integer
	Public Property LLZoomProz() As Integer
		Get
			Return _LLZoomProz
		End Get
		Set(ByVal value As Integer)
			_LLZoomProz = value
		End Set
	End Property

	'// Print.LLCopyCount
	Dim _LLCopyCount As Integer
	Public Property LLCopyCount() As Integer
		Get
			Return _LLCopyCount
		End Get
		Set(ByVal value As Integer)
			_LLCopyCount = value
		End Set
	End Property

	'// Print.LLExportedFilePath
	Dim _LLExportedFilePath As String
	Public Property LLExportedFilePath() As String
		Get
			Return _LLExportedFilePath
		End Get
		Set(ByVal value As String)
			_LLExportedFilePath = value
		End Set
	End Property

	'// Print.LLExportedFileName
	Dim _LLExportedFileName As String
	Public Property LLExportedFileName() As String
		Get
			Return _LLExportedFileName
		End Get
		Set(ByVal value As String)
			_LLExportedFileName = value
		End Set
	End Property

	'// Print.LLPrintInDiffColor
	Dim _LLPrintInDiffColor As Boolean
	Public Property LLPrintInDiffColor() As Boolean
		Get
			Return _LLPrintInDiffColor
		End Get
		Set(ByVal value As Boolean)
			_LLPrintInDiffColor = value
		End Set
	End Property

	'// Print.LLExportfilter
	Dim _LLExportfilter As String
	Public Property LLExportfilter() As String
		Get
			Return _LLExportfilter
		End Get
		Set(ByVal value As String)
			_LLExportfilter = value
		End Set
	End Property

	'// Print.LLExporterName
	Dim _LLExporterName As String
	Public Property LLExporterName() As String
		Get
			Return _LLExporterName
		End Get
		Set(ByVal value As String)
			_LLExporterName = value
		End Set
	End Property

	'// Print.LLExporterFileName
	Dim _LLExporterFileName As String
	Public Property LLExporterFileName() As String
		Get
			Return _LLExporterFileName
		End Get
		Set(ByVal value As String)
			_LLExporterFileName = value
		End Set
	End Property

#End Region

#Region "US Setting"

	'// USeMail (= eMail des Personalvermittlers)
	Dim _USeMail As String
	Public Property USeMail() As String
		Get
			Return _USeMail
		End Get
		Set(ByVal value As String)
			_USeMail = value
		End Set
	End Property

	'// USTelefon (= USTelefon des Personalvermittlers)
	Dim _USTelefon As String
	Public Property USTelefon() As String
		Get
			Return _USTelefon
		End Get
		Set(ByVal value As String)
			_USTelefon = value
		End Set
	End Property

	'// USTelefax (= USTelefax des Personalvermittlers)
	Dim _USTelefax As String
	Public Property USTelefax() As String
		Get
			Return _USTelefax
		End Get
		Set(ByVal value As String)
			_USTelefax = value
		End Set
	End Property

	'// USVorname (= USVorname des Personalvermittlers)
	Dim _USVorname As String
	Public Property USVorname() As String
		Get
			Return _USVorname
		End Get
		Set(ByVal value As String)
			_USVorname = value
		End Set
	End Property

	'// USAnrede (= USAnrede des Personalvermittlers)
	Dim _USAnrede As String
	Public Property USAnrede() As String
		Get
			Return _USAnrede
		End Get
		Set(ByVal value As String)
			_USAnrede = value
		End Set
	End Property

	'// USNachname (= USNachname des Personalvermittlers)
	Dim _USNachname As String
	Public Property USNachname() As String
		Get
			Return _USNachname
		End Get
		Set(ByVal value As String)
			_USNachname = value
		End Set
	End Property

	'// USMDName (= MDName des Personalvermittlers)
	Dim _USMDname As String
	Public Property USMDname() As String
		Get
			Return _USMDname
		End Get
		Set(ByVal value As String)
			_USMDname = value
		End Set
	End Property

	'// MDName2 (= MDName2 des Personalvermittlers)
	Dim _USMDname2 As String
	Public Property USMDname2() As String
		Get
			Return _USMDname2
		End Get
		Set(ByVal value As String)
			_USMDname2 = value
		End Set
	End Property

	'// MDName3 (= MDName3 des Personalvermittlers)
	Dim _USMDname3 As String
	Public Property USMDname3() As String
		Get
			Return _USMDname3
		End Get
		Set(ByVal value As String)
			_USMDname3 = value
		End Set
	End Property

	'// USMDPostfach (= MDPostfach des Personalvermittlers)
	Dim _USMDPostfach As String
	Public Property USMDPostfach() As String
		Get
			Return _USMDPostfach
		End Get
		Set(ByVal value As String)
			_USMDPostfach = value
		End Set
	End Property

	'// USMDStrasse (= MDstrasse des Personalvermittlers)
	Dim _USMDStrasse As String
	Public Property USMDStrasse() As String
		Get
			Return _USMDStrasse
		End Get
		Set(ByVal value As String)
			_USMDStrasse = value
		End Set
	End Property

	'// USMDOrt (= MDOrt des Personalvermittlers)
	Dim _USMDOrt As String
	Public Property USMDOrt() As String
		Get
			Return _USMDOrt
		End Get
		Set(ByVal value As String)
			_USMDOrt = value
		End Set
	End Property

	'// USMDPLZ (= MDPLZ des Personalvermittlers)
	Dim _USMDPlz As String
	Public Property USMDPlz() As String
		Get
			Return _USMDPlz
		End Get
		Set(ByVal value As String)
			_USMDPlz = value
		End Set
	End Property

	'// USMDLand (= MDLand des Personalvermittlers)
	Dim _USMDLand As String
	Public Property USMDLand() As String
		Get
			Return _USMDLand
		End Get
		Set(ByVal value As String)
			_USMDLand = value
		End Set
	End Property

	'// USMDTelefon (= MDTelefon des Personalvermittlers)
	Dim _USMDTelefon As String
	Public Property USMDTelefon() As String
		Get
			Return _USMDTelefon
		End Get
		Set(ByVal value As String)
			_USMDTelefon = value
		End Set
	End Property

	'// USMDTelefax (= MDTelefax des Personalvermittlers)
	Dim _USMDTelefax As String
	Public Property USMDTelefax() As String
		Get
			Return _USMDTelefax
		End Get
		Set(ByVal value As String)
			_USMDTelefax = value
		End Set
	End Property

	'// USMDeMail (= MDeMail des Personalvermittlers)
	Dim _USMDeMail As String
	Public Property USMDeMail() As String
		Get
			Return _USMDeMail
		End Get
		Set(ByVal value As String)
			_USMDeMail = value
		End Set
	End Property

	'// USMDHomepage (= MDHomepage des Personalvermittlers)
	Dim _USMDHomepage As String
	Public Property USMDHomepage() As String
		Get
			Return _USMDHomepage
		End Get
		Set(ByVal value As String)
			_USMDHomepage = value
		End Set
	End Property

#End Region

End Class

Public Class ClsDbFunc

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private m_xml As New ClsXML

	Private m_md As Mandant
	Private m_utility As Utilities
	Private _ClsReg As New SPProgUtility.ClsDivReg
	Public Property luColor As DevExpress.XtraEditors.GridLookUpEdit

	''' <summary>
	''' listet eine Auflistung der Mandantendaten
	''' </summary>
	''' <returns></returns>
	''' <remarks></remarks>
	Function LoadMandantenData() As IEnumerable(Of MandantenData)
		Dim m_utility As New Utilities
		Dim result As List(Of MandantenData) = Nothing
		m_md = New Mandant

		Dim sql As String = "[Mandanten. Get All Allowed MDData]"

		Dim reader = m_utility.OpenReader(ClsDataDetail.GetSelectedMDConnstring, sql, Nothing, CommandType.StoredProcedure)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of MandantenData)

				While reader.Read()
					Dim advisorData As New MandantenData

					advisorData.MDNr = CInt(m_utility.SafeGetInteger(reader, "MDNr", 0))
					advisorData.MDName = m_utility.SafeGetString(reader, "MDName")
					advisorData.MDGuid = m_utility.SafeGetString(reader, "MDGuid")
					advisorData.MDConnStr = m_md.GetSelectedMDData(advisorData.MDNr).MDDbConn

					result.Add(advisorData)

				End While

			End If

		Catch e As Exception
			result = Nothing
			m_Logger.LogError(e.ToString())

		Finally
			m_utility.CloseReader(reader)

		End Try

		Return result

	End Function


	Sub GetJobNr4Print()
		Dim strModul2print As String = String.Empty

		strModul2print = "2.5"

		ClsDataDetail.GetModulToPrint = strModul2print
	End Sub

#Region "Funktionen zur Suche nach Daten..."

	''' <summary>
	''' Dynamische SQL-Query-Zusammenstellung der benötigten Tabellen.
	''' Resultat wird in einer temporäre Tabelle ##Einsatzliste geschrieben.
	''' </summary>
	''' <param name="frmTest"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function GetStartSQLString(ByVal frmTest As frmVakSearch) As String
		Dim sql As String = String.Empty
		Dim sSqlLen As Integer = 0
		Dim sZusatzBez As String = String.Empty
		Dim i As Integer = 0
		Dim _ClsReg As New SPProgUtility.ClsDivReg

		With frmTest
			' SQL-Query der Tabelle FremdOP aus XML-Datei
			sql = "Select V.ID, V.VakNr, V.Berater, V.Filiale, "
			sql &= "ISNULL( (SELECT TOP 1 Bez_D FROM dbo.tbl_base_VakKontakt WHERE RecValue = V.VakKontakt), '') VakKontakt, "
			sql += "ISNULL( (SELECT TOP 1 Bez_D FROM dbo.tbl_base_VakState WHERE RecValue = V.VakState), '') VakState, "
			sql &= "V.Bezeichnung, V.Slogan, V.Gruppe, V.KDNr, "
			sql += "V.KDZHDNr, V.ExistLink, V.VakLink, V.Beginn, "
			sql += "V.JobProzent, V.Anstellung, V.Dauer, V.MAAge, V.MASex, "
			sql += "V.MAZivil, V.MALohn, V.Jobtime, V.JobOrt, V.MAFSchein, "
			sql += "V.MAAuto, V.MANationality, V.IEExport, V.KDBeschreibung, "
			sql += "V.KDBietet, V.SBeschreibung, V.Reserve1, V.Taetigkeit, "
			sql += "V.Anforderung, V.Reserve2, V.Reserve3, V.Ausbildung, "
			sql += "V.Weiterbildung, V.SKennt, V.EDVKennt, V.CreatedOn, "
			sql += "V.CreatedFrom, V.ChangedOn, V.ChangedFrom, "
			sql += "KD.Firma1, KD.Strasse As KDStrasse, KD.PLZ As KDPLZ, KD.Ort As KDOrt, "
			sql += "KD.Land As KDLand, KD.FProperty, KD.Telefon As KDTelefon, KD.eMail As KDeMail, "
			sql += "KD.Telefon As KDTelefon, Z.Nachname As KDZNachname, "
			sql += "Z.Telefon As KDZTelefon, Z.Natel As KDZNatel, Z.EMail As KDZeMail, "
			sql += "Z.Vorname As KDZVorname, IsNull((Select (US.Nachname + ', ' + US.Vorname) As MABerater From Benutzer US Where US.Kst = V.Berater), '') As VakBeratername, "
			sql &= "JCH.IsOnline JCHOnline, OstJob.IsOnline OstJobOnline  "
			sql += "From dbo.Vakanzen V "

			sql += "Left Join dbo.Vak_JobCHData JCH On "
			sql += "JCH.VakNr = V.VakNr "

			sql += "Left Join dbo.Vak_OstJobData OstJob On "
			sql += "OstJob.VakNr = V.VakNr "

			sql += "Left Join dbo.tbl_STMPVacancySetting S On "
			sql += "S.VakNr = V.VakNr "

			'KUNDEN
			sql += "Left Join dbo.Kunden KD On "
			sql += "KD.KDNr = V.KDNr And "
			sql += String.Format("KD.KDFiliale Like '%{0}%' ", ClsDataDetail.UserData.UserFiliale)

			'KD_ZUSTAENDIG
			sql += "Left Join dbo.KD_Zustaendig Z On "
			sql += "Z.RecNr = V.KDZHDNr "


			'Vak_JobCHBranchenData
			If .Lst_VakBranchen.Items.Count > 0 Then
				sql += "Left Join dbo.Vak_JobCHBranchenData VBranche On "
				sql += "VBranche.VakNr = V.VakNr "
			End If

			'VAK_MSPRACHEN
			If .Lst_VakMSprachen.Items.Count > 0 Then
				sql += "Left Join dbo.Vak_MSprachen On "
				sql += "Vak_MSprachen.VakNr = V.VakNr "
			End If


			sSqlLen = Len(sql)

		End With

		Return sql
	End Function

	Function GetQuerySQLString(ByVal sSQLQuery As String, ByVal frmTest As frmVakSearch) As String
		Dim sSql As String = String.Empty
		Dim sOldQuery As String = sSQLQuery

		Dim FilterBez As String = String.Empty
		Dim sSqlLen As Integer = 0
		Dim sZusatzBez As String = String.Empty
		Dim strAndString As String = String.Empty

		Dim strUSFiliale As String = ClsDataDetail.UserData.UserFiliale
		Dim iSQLLen As Integer = Len(sSQLQuery)
		Dim strFieldName As String = String.Empty
		Dim cv As ComboValue

		With frmTest

			' 1. Seite === Allgemeine ============================================================================================
			' ====================================================================================================================
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString

			' Vakanzen-Nr --------------------------------------------------------------------------------------------------
			Dim vaknr1 As String = .txtVakNr_1.Text.Replace("'", "").Replace("*", "%").Trim
			Dim vaknr2 As String = .txtVakNr_2.Text.Replace("'", "").Replace("*", "%").Trim

			If vaknr1 = "" And vaknr2 = "" Then 'do nothing
				' Suche VakNr mit Sonderzeichen
			ElseIf vaknr1.Contains("%") Then
				FilterBez = String.Format(m_xml.GetSafeTranslationValue("{0}Vakanzen mit VakNr ({1}){2}"), strAndString, vaknr1, vbLf)
				sSql += String.Format("{0}V.VakNr Like '{1}'", strAndString, vaknr1)
				' Suche VakNr mit Komma getrennt
			ElseIf InStr(vaknr1, ",") > 0 Then
				FilterBez = String.Format(m_xml.GetSafeTranslationValue("Vakanzen mit VakNr ({0}){1}"), vaknr1, vbLf)
				sSql += String.Format("{0}V.VakNr In (", strAndString)
				For Each esnr In vaknr1.Split(CChar(","))
					sSql += String.Format("'{0}',", esnr)
				Next
				sSql = sSql.Substring(0, sSql.Length - 1)
				sSql += ")"
				' Suche genau eine VakNr
			ElseIf vaknr1 = vaknr2 Then
				FilterBez = String.Format(m_xml.GetSafeTranslationValue("Vakanz mit VakNr = {0}{1}"), vaknr1, vbLf)
				sSql += String.Format("{0}V.VakNr = {1}", strAndString, vaknr1)
				' Suche ab VakNr1 
			ElseIf vaknr1 <> "" And vaknr2 = "" Then
				FilterBez = String.Format(m_xml.GetSafeTranslationValue("Vakanzen ab VakNr {0}{1}"), vaknr1, vbLf)
				sSql += String.Format("{0}V.VakNr >= {1}", strAndString, vaknr1)
				' Suche bis VakNr2
			ElseIf vaknr1 = "" And vaknr2 <> "" Then
				FilterBez = String.Format(m_xml.GetSafeTranslationValue("Vakanzen bis VakNr {0}{1}"), vaknr2, vbLf)
				sSql += String.Format("{0}V.VakNr <= {1}", strAndString, vaknr2)
				' Suche zwischen erste und zweite VakNr
			Else
				FilterBez = String.Format(m_xml.GetSafeTranslationValue("Vakanzen zwischen VakNr {0} und {1}{2}"), vaknr1, vaknr2, vbLf)
				sSql += String.Format("{0}V.VakNr Between '{1}' And '{2}'", strAndString, vaknr1, vaknr2)
			End If

			' Kundennummer -----------------------------------------------------------------------------------------
			Dim kdnr1 As String = .txtVakKDNr_1.Text.Replace("'", "").Replace("*", "%").Trim
			Dim kdnr2 As String = .txtVakKDNr_2.Text.Replace("'", "").Replace("*", "%").Trim
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString

			If kdnr1 = String.Empty And kdnr2 = String.Empty Then   ' do nothing
				' Suche KDNr mit Sonderzeichen
			ElseIf kdnr1.Contains("%") Then
				FilterBez = String.Format(m_xml.GetSafeTranslationValue("Vakanzen mit KDNr wie ({0}){1}"), kdnr1, vbLf)
				sSql += String.Format("{0}V.KDNr Like '{1}'", strAndString, kdnr1)
				' Suche mehrere KDNr getrennt durch Komma
			ElseIf InStr(kdnr1, ",") > 0 Then
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Vakanzen mit KDNr wie ({0}){1}"), kdnr1, vbLf)
				sSql += String.Format("{0}V.KDNr In (", strAndString)
				For Each manr In kdnr1.Split(CChar(","))
					sSql += String.Format("'{0}',", manr)
				Next
				sSql = sSql.Substring(0, sSql.Length - 1)
				sSql += ")"
				' Suche Vakanzen von genau einem KDNr
			ElseIf kdnr1 = kdnr2 Then
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Vakanzen mit KDNr = {0}{1}"), kdnr1, vbLf)
				sSql += String.Format("{0}V.KDNr = '{1}'", strAndString, kdnr1)
				' Suche Vakanzen ab KDNr1
			ElseIf kdnr1 <> "" And kdnr2 = "" Then
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Vakanzen ab KDNr {0}{1}"), kdnr1, vbLf)
				sSql += String.Format("{0}V.KDNr >= '{1}'", strAndString, kdnr1)
				' Suche Vakanzen bis KDNr2
			ElseIf kdnr1 = "" And kdnr2 <> "" Then
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Vakanzen bis KDNr {0}{1}"), kdnr2, vbLf)
				sSql += String.Format("{0}V.KDNr <= '{1}'", strAndString, kdnr2)
				' Suche Vakanzen zwischen zwei KDNr 
			Else
				FilterBez = String.Format(m_xml.GetSafeTranslationValue("Vakanzen zwischen KDNr {0} und {1}{2}"), kdnr1, kdnr2, vbLf)
				sSql += String.Format("{0}V.KDNr Between '{1}' And '{2}'",
					 strAndString, kdnr1.Trim, kdnr2)
			End If

			' Antritt per -----------------------------------------------------------------------------------------------
			If .Cbo_VakAntrittPer.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sSql &= strAndString & String.Format("V.Beginn = '{0}' ", .Cbo_VakAntrittPer.Text)
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Vakanzen mit Beginn = {0}{1}"),
																	 .Cbo_VakAntrittPer.Text, vbLf)
			End If

			' Beschäftigung -----------------------------------------------------------------------------------------------
			If .Cbo_VakBeschaeftigung.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sSql &= strAndString & String.Format("V.JobProzent = '{0}' ", .Cbo_VakBeschaeftigung.Text)
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Vakanzen mit Beschäftigungsgrad = {0}{1}"),
																	 .Cbo_VakBeschaeftigung.Text, vbLf)
			End If

			' Anstellungsart -----------------------------------------------------------------------------------------------
			If .Cbo_VakAnstellungsart.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_VakAnstellungsart.SelectedItem, ComboValue)
				If Not cv Is Nothing Then
					sSql &= strAndString & String.Format("V.Anstellung Like '%{0}%' ", cv.ComboValue)
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Vakanzen mit Anstellungsart = {0}{1}"),
																		 cv.Text, vbLf)
				End If
			End If

			' Gruppe -----------------------------------------------------------------------------------------------
			If .Cbo_VakGruppe.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sSql &= strAndString & String.Format("V.Gruppe = '{0}' ", .Cbo_VakGruppe.Text)
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Vakanzen mit Gruppe = {0}{1}"), .Cbo_VakGruppe.Text, vbLf)
			End If

			' Dauer -----------------------------------------------------------------------------------------------
			If .Cbo_VakDauer.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sSql &= strAndString & String.Format("V.Dauer = '{0}' ", .Cbo_VakDauer.Text)
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Vakanzen mit Dauer = {0}{1}"),
																	 .Cbo_VakDauer.Text, vbLf)
			End If

			' Alter -----------------------------------------------------------------------------------------------
			If .Cbo_VakAlter.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_VakAlter.SelectedItem, ComboValue)
				If Not cv Is Nothing Then
					sSql &= strAndString & String.Format("V.MAAge Like '%{0}%' ", cv.ComboValue)
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Vakanzen mit Mitarbeiteralter = {0}{1}"),
																		 cv.Text, vbLf)
				End If
			End If

			' Geschlecht -----------------------------------------------------------------------------------------------
			If .Cbo_VakGeschlecht.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sSql &= strAndString & String.Format("V.MASex = '{0}' ", .Cbo_VakGeschlecht.Text)
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Vakanzen mit Mitarbeitergeschlecht = {0}{1}"),
																	 .Cbo_VakGeschlecht.Text, vbLf)
			End If

			' Zivilstand -----------------------------------------------------------------------------------------------
			If .Cbo_VakZivilstand.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sSql &= strAndString & String.Format("V.MAZivil = '{0}' ", .Cbo_VakZivilstand.Text)
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Vakanzen mit Mitarbeiterzivilstand = {0}{1}"),
																	 .Cbo_VakZivilstand.Text, vbLf)
			End If

			' Lohn -----------------------------------------------------------------------------------------------
			If .Cbo_VakLohn.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sSql &= strAndString & String.Format("V.MALohn = '{0}' ", .Cbo_VakLohn.Text)
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Vakanzen mit Mitarbeiterlohn = {0}{1}"),
																	 .Cbo_VakLohn.Text, vbLf)
			End If

			' Arbeitszeit -----------------------------------------------------------------------------------------------
			If .Cbo_VakArbeitszeit.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sSql &= strAndString & String.Format("V.Jobtime = '{0}' ", .Cbo_VakArbeitszeit.Text)
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Vakanzen mit Arbeitszeit = {0}{1}"),
																	 .Cbo_VakArbeitszeit.Text, vbLf)
			End If

			' Arbeitsort -----------------------------------------------------------------------------------------------
			If .Cbo_VakArbeitsort.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sSql &= strAndString & String.Format("V.JobOrt = '{0}' ", .Cbo_VakArbeitsort.Text)
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Vakanzen mit Arbeitsort = {0}{1}"),
																	 .Cbo_VakArbeitsort.Text, vbLf)
			End If

			' Kontakt -----------------------------------------------------------------------------------------------
			If .Cbo_VakKontakt.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_VakKontakt.SelectedItem, ComboValue)
				If Not cv Is Nothing Then
					sSql &= strAndString & String.Format("V.VakKontakt = '{0}' ", cv.ComboValue)
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Vakanzen mit Kontakt = {0}{1}"),
																		 cv.Text, vbLf)
				End If
			End If

			' Status -----------------------------------------------------------------------------------------------
			If .Cbo_VakStatus.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_VakStatus.SelectedItem, ComboValue)
				If Not cv Is Nothing Then
					sSql &= strAndString & String.Format("V.VakState = '{0}' ", cv.ComboValue)
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Vakanzen mit Status = {0}{1}"),
																		cv.Text, vbLf)
				End If
			End If

			' Geschäftsstelle -----------------------------------------------------------------------------------------------
			If .Cbo_VakGeschaeftsstelle.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sSql &= strAndString & String.Format("V.Filiale = '{0}' ", .Cbo_VakGeschaeftsstelle.Text)
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Vakanzen mit Filiale = {0}{1}"),
																	 .Cbo_VakGeschaeftsstelle.Text, vbLf)
			End If

			If .Cbo_VakBerater.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_VakBerater.SelectedItem, ComboValue)
				Dim strSearchValue = cv.Text.Trim.Replace("'", "''").Replace("*", "%")

				If cv.ComboValue.Contains(",") Then
					sSql += String.Format("{0}V.Berater In(", strAndString)
					For Each ber In cv.ComboValue.Split(CChar(","))
						sSql += String.Format("'{0}',", ber.Trim)
					Next
					sSql = sSql.Remove(sSql.Length - 1, 1)
					sSql += ")"
				Else
					sSql += String.Format("{0}V.Berater  = '{1}' ", strAndString, cv.ComboValue)

				End If
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Vakanzen mit Berater = {0}{1}"), .Cbo_VakBerater.Text, vbLf)
			End If


			' Im Web veröffentlicht ----------------------------------------------------------------------------------------
			If Not String.IsNullOrWhiteSpace(.Cbo_VakImWeb.Text) Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_VakImWeb.SelectedItem, ComboValue)

				sSql += String.Format("{0}IsNull(V.IEExport, 0) = {1} ", strAndString, cv.ComboValue)
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Vakanzen im Web {0}{1}"), .Cbo_VakImWeb.Text, vbLf)
			End If

			If Not String.IsNullOrWhiteSpace(.cbo_JobsCH.Text) Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.cbo_JobsCH.SelectedItem, ComboValue)

				sSql += String.Format("{0}IsNull(JCH.ISOnline, 0) = {1} ", strAndString, cv.ComboValue)
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Vakanzen auf Jobs.CH {0}{1}"), .cbo_JobsCH.EditValue, vbLf)
			End If

			If Not String.IsNullOrWhiteSpace(.cbo_Ostjob.Text) Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.cbo_Ostjob.SelectedItem, ComboValue)

				sSql += String.Format("{0}IsNull(OstJob.ISOnline, 0) = {1} ", strAndString, cv.ComboValue)
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Vakanzen auf Ostjob.CH {0}{1}"), .cbo_Ostjob.EditValue, vbLf)
			End If

			If Not String.IsNullOrWhiteSpace(.Cbo_AVAM.Text) Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_AVAM.SelectedItem, ComboValue)

				If cv.ComboValue = "1" Then
					sSql += String.Format("{0}IsNull(V.SBNNumber, 0) <> 0 And IsNull(S.JobRoomID, '') <> '' ", strAndString)

				Else
					sSql += String.Format("{0}IsNull(V.SBNNumber, 0) = 0 And IsNull(S.JobRoomID, '') = '' ", strAndString)

				End If
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Vakanz an RAV {0}{1}"), .Cbo_AVAM.Text, vbLf)

			End If

			If Not String.IsNullOrWhiteSpace(.Cbo_AVAMState.Text) Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_AVAMState.SelectedItem, ComboValue)

				sSql += String.Format("{0}IsNull(S.AVAMRecordState, '') = '{1}' ", strAndString, .Cbo_AVAMState.Text)
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Übermittlung an RAV mit Status: {0}{1}"), .Cbo_AVAMState.Text, vbLf)

			End If

			' Erfasst ab/bis -----------------------------------------------------------------------------------------------
			If .deVAKErfasstAb.Text <> "" Then If Year(CDate(.deVAKErfasstAb.Text)) = 1 Then .deVAKErfasstAb.Text = String.Empty
			If .deVAKErfasstBis.Text <> "" Then If Year(CDate(.deVAKErfasstBis.Text)) = 1 Then .deVAKErfasstBis.Text = String.Empty
			If .deVAKErfasstAb.Text <> "" Or .deVAKErfasstBis.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				Dim erfasstab As String = ""
				Dim erfasstbis As String = ""
				If IsDate(.deVAKErfasstAb.Text) Then
					erfasstab = Date.Parse(.deVAKErfasstAb.Text).ToString("dd.MM.yyyy")
				End If
				If IsDate(.deVAKErfasstBis.Text) Then
					erfasstbis = Date.Parse(.deVAKErfasstBis.Text).ToString("dd.MM.yyyy")
				End If

				' Suche zwischen zwei Datum
				If erfasstab.Length > 0 And erfasstbis.Length > 0 Then
					sSql += String.Format("{0}V.CreatedOn Between '{1} 00:00' And '{2} 23:59'",
																strAndString, erfasstab, erfasstbis)
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Vakanzen erstellt zwischen {0} und {1}{2}"),
																		 erfasstab, erfasstbis, vbLf)
					' Suche ab erstes Datum
				ElseIf erfasstab.Length > 0 Then
					sSql += String.Format("{0}V.CreatedOn >= '{1}'", strAndString, erfasstab)
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Vakanzen erstellt ab Datum {0}{1}"), erfasstab, vbLf)
					' Suche bis zweites Datum
				ElseIf erfasstbis.Length > 0 Then
					sSql += String.Format("{0}V.CreatedOn <= '{1} 23:59'", strAndString, erfasstbis)
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Vakanzen erstellt bis Datum {0}{1}"), erfasstbis, vbLf)
				End If
			End If


			' Branchen -----------------------------------------------------------------------------------------------
			If .Lst_VakBranchen.Items.Count > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = GetLstItems(.Lst_VakBranchen).Replace("'", "''")
				sSql += strAndString & "VBranche.Bezeichnung In ('"
				sSql += Replace(sZusatzBez, "#@", "','") & "')"
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Branchen in ({0}){1}"), sZusatzBez, vbLf)
			End If

			' Bezeichnung -----------------------------------------------------------------------------------------------
			If .Lst_VakBezeichnung.Items.Count > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = GetLstItems(.Lst_VakBezeichnung).Replace("'", "''")
				sSql += strAndString & "V.Bezeichnung In ('"
				sSql += Replace(sZusatzBez, "#@", "','") & "')"
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Bezeichnung in ({0}){1}"), sZusatzBez, vbLf)
			End If

			' Mündliche Sprachen -----------------------------------------------------------------------------------------------
			If .Lst_VakMSprachen.Items.Count > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = GetLstItems(.Lst_VakMSprachen).Replace("'", "''")
				sSql += strAndString & "Vak_MSprachen.Bezeichnung In ('"
				sSql += Replace(sZusatzBez, "#@", "','") & "')"
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mündliche Sprachen in ({0}){1}"), sZusatzBez, vbLf)
			End If

			'' Schriftliche Sprachen -----------------------------------------------------------------------------------------------
			'If .Lst_VakSSprachen.Items.Count > 0 Then
			'  strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			'  sZusatzBez = GetLstItems(.Lst_VakSSprachen).Replace("'", "''")
			'  sSql += strAndString & "Vak_SSprachen.Bezeichnung In ('"
			'  sSql += Replace(sZusatzBez, "#@", "','") & "')"
			'  FilterBez += String.Format(m_xml.GetSafeTranslationValue("Schriftliche Sprachen in ({0}){1}"), sZusatzBez, vbLf)
			'End If

			If ClsDataDetail.MDData.MultiMD = 1 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sSql &= String.Format("{0}V.MDNr = {1}", strAndString, ClsDataDetail.ProgSettingData.SelectedMDNr)
			End If

		End With
		ClsDataDetail.GetFilterBez = FilterBez

		m_Logger.LogInfo(String.Format("VAKSearchWhereQuery: {0}", sSql))

		Return sSql
	End Function


	Function GetSortString(ByVal frmTest As frmVakSearch) As String
		Dim strSort As String = " Order by "
		Dim strSortBez As String = String.Empty
		Dim strName As String()
		Dim strMyName As String = String.Empty

		'0 - VakNr
		'1 - Bezeichnung
		'2 - Erfassdatum
		'3 - Firma

		With frmTest
			strName = Regex.Split(.CboSort.Text.Trim, ",")
			strMyName = String.Empty
			If .CboSort.Text.Contains("-") Then
				For i As Integer = 0 To strName.Length - 1
					Select Case CInt(Val(Left(strName(i).ToString.Trim, 1))) ' Das erste Zeichen der Sortierung
						Case 0        ' Nach VakNr
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " V.VakNr"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ",", "")) & "Vakanzennummer"
						Case 1        ' Nach Bezeichnung
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " V.Bezeichnung"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Bezeichnung"
						Case 2        ' Nach Erfassdatum
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " V.CreatedOn"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Erfassdatum"
						Case 3        ' Nach Firma
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " KD.Firma1 ASC"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Firma"
					End Select
				Next i
			Else
				strMyName = .CboSort.Text
				strSortBez = "Benutzerdefiniert"
			End If

		End With

		If strMyName.Trim = "" Then
			Return ""
		Else
			strSort = strSort & strMyName
			ClsDataDetail.GetSortBez = strSortBez
			Return strSort
		End If

	End Function

	Function GetLstItems(ByVal lst As DevExpress.XtraEditors.ListBoxControl) As String
		Dim strBerufItems As String = String.Empty

		For i = 0 To lst.Items.Count - 1
			strBerufItems += lst.Items(i).ToString & "#@"
		Next

		Return Left(strBerufItems, Len(strBerufItems) - 2)
	End Function

#End Region

End Class

'Module MyComboBoxExtensions
'  <Extension()> _
'  Public Function ToItem(ByVal cbo As myCbo) As ComboBoxItem
'    If TypeOf (cbo.SelectedItem) Is ComboBoxItem And cbo.SelectedIndex > -1 Then
'      Return DirectCast(cbo.Items(cbo.SelectedIndex), ComboBoxItem)
'    ElseIf cbo.SelectedIndex > -1 Then
'      Dim item As New ComboBoxItem("", "")
'      item.Text = cbo.Items(cbo.SelectedIndex).ToString
'      item.Value = item.Text
'      Return item
'    Else
'      Dim item As New ComboBoxItem("", "")
'      item.Text = cbo.Text
'      item.Value = cbo.Text
'      Return item
'    End If
'  End Function
'End Module