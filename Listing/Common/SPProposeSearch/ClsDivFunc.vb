
Imports System.Text.RegularExpressions
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports SPProposeSearch.ClsDataDetail
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

		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ClsDataDetail.GetSelectedMDConnstring, sql, Nothing, CommandType.StoredProcedure)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of MandantenData)

				While reader.Read()
					Dim recData As New MandantenData

					recData.MDNr = CInt(m_utility.SafeGetInteger(reader, "MDNr", 0))
					recData.MDName = m_utility.SafeGetString(reader, "MDName")
					recData.MDGuid = m_utility.SafeGetString(reader, "MDGuid")
					recData.MDConnStr = m_md.GetSelectedMDData(recData.MDNr).MDDbConn

					result.Add(recData)

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




#Region "Funktionen zur Suche nach Daten..."

	''' <summary>
	''' Dynamische SQL-Query-Zusammenstellung der benötigten Tabellen.
	''' Resultat wird in einer temporäre Tabelle ##Einsatzliste geschrieben.
	''' </summary>
	''' <returns></returns>
	''' <remarks></remarks>
	Function GetStartSQLString() As String
		Dim sSql As String = String.Empty

		sSql += "Select P.*, "
		sSql += "MA.Nachname As MANachname, MA.Vorname As MAVorname, MA.PLZ As MAPLZ, MA.Ort As MAOrt, "
		sSql += "MA.Telefon_P As MATelefon, MA.Telefon_G As MATelefon, MA.Natel As MANatel, "

		sSql += "KD.Firma1, KD.Strasse As KDStrasse, KD.PLZ As KDPLZ, KD.Ort As KDOrt, "
		sSql += "KD.Land As KDLand, KD.FProperty, KD.Telefon As KDTelefon, KD.eMail As KDeMail, KD.KL_RefNr As Kredit_RefNr, "
		sSql += "KD.Telefon As KDTelefon, KDZ.Nachname As KDZNachname, "
		sSql += "KDZ.Vorname As KDZVorname, "
		sSql += "KDZ.Telefon As KDZTelefon, KDZ.Natel As KDZNatel "

		sSql += "From Propose P "
		sSql &= "Left Join Mitarbeiter MA On P.MANr = MA.MANr "
		sSql &= "Left Join Kunden KD On P.KDNr = KD.KDNr "
		sSql &= "Left Join KD_Zustaendig KDZ On P.KDZHDNr = KDZ.RecNr And KD.KDNr = KDZ.KDNr "

		Return sSql
	End Function

	Function GetQuerySQLString(ByVal sSQLQuery As String, ByVal frmTest As frmProposeSearch) As String
		Dim sSql As String = String.Empty
		Dim sOldQuery As String = sSQLQuery

		Dim FilterBez As String = String.Empty
		Dim sSqlLen As Integer = 0
		Dim sZusatzBez As String = String.Empty
		Dim strAndString As String = String.Empty

		Dim iSQLLen As Integer = Len(sSQLQuery)
		Dim strFieldName As String = String.Empty

		With frmTest
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString

			' Propose-Nr --------------------------------------------------------------------------------------------------
			Dim strValue1 As String = .txt_PNr_1.Text.Replace("'", "").Replace("*", "%").Trim
			Dim strValue2 As String = .txt_PNr_2.Text.Replace("'", "").Replace("*", "%").Trim
			strFieldName = "P.ProposeNr"
			If strValue1 = "" And strValue2 = "" Then 'do nothing
				' Suche ProposeNr mit Sonderzeichen
			ElseIf strValue1.Contains("%") Then
				FilterBez = String.Format("{0}Vorschlag mit Nummer ({1}){2}", strAndString, strValue1, vbLf)
				sSql += String.Format("{0}{1} Like '{2}'", strAndString, strFieldName, strValue1)
				' Suche PNr mit Komma getrennt
			ElseIf InStr(strValue1, ",") > 0 Then
				FilterBez = String.Format("Vorschlag mit Nummer ({0}){1}", strValue1, vbLf)
				sSql += String.Format("{0}{1} In (", strAndString, strFieldName)
				For Each esnr In strValue1.Split(CChar(","))
					sSql += String.Format("'{0}',", esnr)
				Next
				sSql = sSql.Substring(0, sSql.Length - 1)
				sSql += ")"

				' Suche PNr mit Bindestrich für bis (-)
			ElseIf InStr(strValue1, "-") > 0 Then
				Dim aValue1 As String() = strValue1.Split(CChar("-"))
				FilterBez = String.Format("Vorschlagnummer zwischen {0} und {1}{2}", aValue1(0), aValue1(1), vbLf)
				sSql += String.Format("{0}{1} Between '{2}' And '{3}'", strAndString, strFieldName, aValue1(0), aValue1(1))

				' Suche genau eine PNr
			ElseIf (strValue1 = strValue2 And .txt_PNr_2.Visible) Or (strValue1 <> "" And Not .txt_PNr_2.Visible) Then
				FilterBez = String.Format("Vorschlag mit Nummer = {0}{1}", strValue1, vbLf)
				sSql += String.Format("{0}{1} = {2}", strAndString, strFieldName, strValue1)
				' Suche ab PNr1 
			ElseIf strValue1 <> "" And strValue2 = "" And .txt_PNr_2.Visible Then
				FilterBez = String.Format("Vorschlagnummer ab {0}{1}", strValue1, vbLf)
				sSql += String.Format("{0}{1} >= {2}", strAndString, strFieldName, strValue1)
				' Suche bis PNr2
			ElseIf strValue1 = "" And strValue2 <> "" And .txt_PNr_2.Visible Then
				FilterBez = String.Format("Vorschlagnummer bis {0}{1}", strValue2, vbLf)
				sSql += String.Format("{0}{1} <= {2}", strAndString, strFieldName, strValue2)
				' Suche zwischen erste und zweite PNr
			ElseIf .txt_PNr_2.Visible Then
				FilterBez = String.Format("Vorschlagnummer zwischen {0} und {1}{2}", strValue1, strValue2, vbLf)
				sSql += String.Format("{0}{1} Between '{2}' And '{3}'", strAndString, strFieldName, strValue1, strValue2)
			End If

			' Kandidatennummer -----------------------------------------------------------------------------------------
			strValue1 = .txt_MANr_1.Text.Replace("'", "").Replace("*", "%").Trim
			strValue2 = .txt_MANr_2.Text.Replace("'", "").Replace("*", "%").Trim
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			strFieldName = "P.MANr"

			If strValue1 = String.Empty And strValue2 = String.Empty Then   ' do nothing
				' Suche MANr mit Sonderzeichen
			ElseIf strValue1.Contains("%") Then
				FilterBez = String.Format("Vorschlag mit MANr wie ({0}){1}", strValue1, vbLf)
				sSql += String.Format("{0}{1} Like '{2}'", strAndString, strFieldName, strValue1)
				' Suche mehrere MANr getrennt durch Komma
			ElseIf InStr(strValue1, ",") > 0 Then
				FilterBez += String.Format("Vorschlag mit MANr wie ({0}){1}", strValue1, vbLf)
				sSql += String.Format("{0}{1} In (", strAndString, strFieldName)
				For Each manr In strValue1.Split(CChar(","))
					sSql += String.Format("'{0}',", manr)
				Next
				sSql = sSql.Substring(0, sSql.Length - 1)
				sSql += ")"

				' Suche MANr mit Bindestrich für bis (-)
			ElseIf InStr(strValue1, "-") > 0 Then
				Dim aValue1 As String() = strValue1.Split(CChar("-"))
				FilterBez = String.Format("Vorschlag mit MANr zwischen {0} und {1}{2}", aValue1(0), aValue1(1), vbLf)
				sSql += String.Format("{0}{1} Between '{2}' And '{3}'", strAndString, strFieldName, aValue1(0), aValue1(1))

				' Suche Vorschlag von genau einem MANr
			ElseIf (strValue1 = strValue2 And .txt_MANr_2.Visible) Or (strValue1 <> "" And Not .txt_MANr_2.Visible) Then
				FilterBez += String.Format("Vorschlag mit MANr = {0}{1}", strValue1, vbLf)
				sSql += String.Format("{0}{1} = '{2}'", strAndString, strFieldName, strValue1)
				' Suche Vorschlag ab MANr
			ElseIf strValue1 <> "" And strValue2 = "" And .txt_MANr_2.Visible Then
				FilterBez += String.Format("Vorschlag ab MANr {0}{1}", strValue1, vbLf)
				sSql += String.Format("{0}{1} >= '{2}'", strAndString, strFieldName, strValue1)
				' Suche Vorschlag bis MANr
			ElseIf strValue1 = "" And strValue2 <> "" And .txt_MANr_2.Visible Then
				FilterBez += String.Format("Vorschlag bis MANr {0}{1}", strValue2, vbLf)
				sSql += String.Format("{0}{1} <= '{2}'", strAndString, strFieldName, strValue2)
				' Suche Vorschlag zwischen zwei MANr 
			ElseIf .txt_MANr_2.Visible Then
				FilterBez = String.Format("Vorschlag mit MANr zwischen {0} und {1}{2}", strValue1, strValue2, vbLf)
				sSql += String.Format("{0}{1} Between '{2}' And '{3}'", strAndString, strFieldName, strValue1.Trim, strValue2)
			End If

			' Kundennummer -----------------------------------------------------------------------------------------
			strValue1 = .txt_KDNr_1.Text.Replace("'", "").Replace("*", "%").Trim
			strValue2 = .txt_KDNr_2.Text.Replace("'", "").Replace("*", "%").Trim
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			strFieldName = "P.KDNr"

			If strValue1 = String.Empty And strValue2 = String.Empty Then   ' do nothing
				' Suche KDNr mit Sonderzeichen
			ElseIf strValue1.Contains("%") Then
				FilterBez = String.Format("Vorschlag mit KDNr wie ({0}){1}", strValue1, vbLf)
				sSql += String.Format("{0}{1} Like '{2}'", strAndString, strFieldName, strValue1)
				' Suche mehrere KDNr getrennt durch Komma
			ElseIf InStr(strValue1, ",") > 0 Then
				FilterBez += String.Format("Vorschlag mit KDNr wie ({0}){1}", strValue1, vbLf)
				sSql += String.Format("{0}{1} In (", strAndString, strFieldName)
				For Each manr In strValue1.Split(CChar(","))
					sSql += String.Format("'{0}',", manr)
				Next
				sSql = sSql.Substring(0, sSql.Length - 1)
				sSql += ")"

				' Suche MANr mit Bindestrich für bis (-)
			ElseIf InStr(strValue1, "-") > 0 Then
				Dim aValue1 As String() = strValue1.Split(CChar("-"))
				FilterBez = String.Format("Vorschlag mit MANr zwischen {0} und {1}{2}", aValue1(0), aValue1(1), vbLf)
				sSql += String.Format("{0}{1} Between '{2}' And '{3}'", strAndString, strFieldName, aValue1(0), aValue1(1))

				' Suche Vorschlag von genau einem KDNr
			ElseIf (strValue1 = strValue2 And .txt_KDNr_2.Visible) Or (strValue1 <> "" And Not .txt_KDNr_2.Visible) Then
				FilterBez += String.Format("Vorschlag mit KDNr = {0}{1}", strValue1, vbLf)
				sSql += String.Format("{0}{1} = '{2}'", strAndString, strFieldName, strValue1)
				' Suche Vorschlag ab KDNr1
			ElseIf strValue1 <> "" And strValue2 = "" And .txt_KDNr_2.Visible Then
				FilterBez += String.Format("Vorschlag ab KDNr {0}{1}", strValue1, vbLf)
				sSql += String.Format("{0}{1} >= '{2}'", strAndString, strFieldName, strValue1)
				' Suche Vorschlag bis KDNr2
			ElseIf strValue1 = "" And strValue2 <> "" And .txt_KDNr_2.Visible Then
				FilterBez += String.Format("Vorschlag bis KDNr {0}{1}", strValue2, vbLf)
				sSql += String.Format("{0}{1} <= '{2}'", strAndString, strFieldName, strValue2)
				' Suche Vorschlag zwischen zwei KDNr 
			ElseIf .txt_KDNr_2.Visible Then
				FilterBez = String.Format("Vorschlag zwischen KDNr {0} und {1}{2}", strValue1, strValue2, vbLf)
				sSql += String.Format("{0}{1} Between '{2}' And '{3}'", strAndString, strFieldName, strValue1.Trim, strValue2)
			End If

			' Vakanzennummer -----------------------------------------------------------------------------------------
			strValue1 = .txt_VakNr_1.Text.Replace("'", "").Replace("*", "%").Trim
			strValue2 = .txt_VakNr_2.Text.Replace("'", "").Replace("*", "%").Trim
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			strFieldName = "P.VakNr"

			If strValue1 = String.Empty And strValue2 = String.Empty Then   ' do nothing
				' Suche VakNr mit Sonderzeichen
			ElseIf strValue1.Contains("%") Then
				FilterBez = String.Format("Vorschlag mit VakNr wie ({0}){1}", strValue1, vbLf)
				sSql += String.Format("{0}{1} Like '{2}'", strAndString, strFieldName, strValue1)
				' Suche mehrere VakNr getrennt durch Komma
			ElseIf InStr(strValue1, ",") > 0 Then
				FilterBez += String.Format("Vorschlag mit VakNr wie ({0}){1}", strValue1, vbLf)
				sSql += String.Format("{0}{1} In (", strAndString, strFieldName)
				For Each manr In strValue1.Split(CChar(","))
					sSql += String.Format("'{0}',", manr)
				Next
				sSql = sSql.Substring(0, sSql.Length - 1)
				sSql += ")"

				' Suche VakNr mit Bindestrich für bis (-)
			ElseIf InStr(strValue1, "-") > 0 Then
				Dim aValue1 As String() = strValue1.Split(CChar("-"))
				FilterBez = String.Format("Vorschlag mit MANr zwischen {0} und {1}{2}", aValue1(0), aValue1(1), vbLf)
				sSql += String.Format("{0}{1} Between '{2}' And '{3}'", strAndString, strFieldName, aValue1(0), aValue1(1))

				' Suche Vorschlag von genau einem VakNr
			ElseIf (strValue1 = strValue2 And .txt_VakNr_2.Visible) Or (strValue1 <> "" And Not .txt_VakNr_2.Visible) Then
				FilterBez += String.Format("Vorschlag mit VakNr = {0}{1}", strValue1, vbLf)
				sSql += String.Format("{0}{1} = '{2}'", strAndString, strFieldName, strValue1)

				' Suche Vorschlag ab VakNr
			ElseIf strValue1 <> "" And strValue2 = "" And .txt_VakNr_2.Visible Then
				FilterBez += String.Format("Vorschlag ab VakNr {0}{1}", strValue1, vbLf)
				sSql += String.Format("{0}{1} >= '{2}'", strAndString, strFieldName, strValue1)
				' Suche Vorschlag bis VakNr
			ElseIf strValue1 = "" And strValue2 <> "" And .txt_VakNr_2.Visible Then
				FilterBez += String.Format("Vorschlag bis VakNr {0}{1}", strValue2, vbLf)
				sSql += String.Format("{0}{1} <= '{2}'", strAndString, strFieldName, strValue2)
				' Suche Vorschlag zwischen zwei VakNr 
			ElseIf .txt_VakNr_2.Visible Then
				FilterBez = String.Format("Vorschlag zwischen VakNr {0} und {1}{2}", strValue1, strValue2, vbLf)
				sSql += String.Format("{0}{1} Between '{2}' And '{3}'", strAndString, strFieldName, strValue1.Trim, strValue2)
			End If


			' Bezeichnung -----------------------------------------------------------------------------------------------
			If .Cbo_Bez.Text <> "" Then
				strValue1 = .Cbo_Bez.Text
				strFieldName = "P.Bezeichnung"
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sSql += String.Format("{0}{1} Like '%{2}%'", strAndString, strFieldName, strValue1)
				FilterBez += String.Format("Vorschläge mit Status = {0}{1}", strValue1, vbLf)
			End If

			' Status -----------------------------------------------------------------------------------------------
			If .Cbo_State.Text <> "" Then
				strValue1 = .Cbo_State.Text.Trim
				strFieldName = "P.P_State"
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				Dim aValue1 As String() = strValue1.Split(CChar(", "))
				Dim strMyString2Search As String = String.Empty
				For i As Integer = 0 To aValue1.Length - 1
					strMyString2Search &= If(strMyString2Search = String.Empty, "", ",") & aValue1(i).ToString.Trim
				Next
				sSql += String.Format("{0}{1} in ('{2}')", strAndString, strFieldName,
											strMyString2Search.Trim.Replace("Leere Felder",
																														 "").Replace("'", "''").Replace(",", "','"))

				'sSql += String.Format("{0}{1} In ('{2}')", strAndString, strFieldName, strValue1)
				FilterBez += String.Format("Vorschläge mit Status = {0}{1}", strValue1, vbLf)
			End If

			' Art -----------------------------------------------------------------------------------------------
			If .Cbo_Art.Text <> "" Then
				strValue1 = .Cbo_Art.Text
				strFieldName = "P.P_Art"
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sSql += String.Format("{0}{1} = '{2}'", strAndString, strFieldName, strValue1)
				FilterBez += String.Format("Vorschläge mit Art = {0}{1}", strValue1, vbLf)
			End If

			' Anstellungsart -----------------------------------------------------------------------------------------------
			If .Cbo_Anstellung.Text <> "" Then
				strValue1 = .Cbo_Anstellung.Text
				strFieldName = "P.P_Anstellung"
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sSql += String.Format("{0}{1} = '{2}'", strAndString, strFieldName, strValue1)
				FilterBez += String.Format("Vorschläge mit Anstellungsart = {0}{1}", strValue1, vbLf)
			End If


			' Arbbeginn -----------------------------------------------------------------------------------------------
			If .Cbo_Arbbegin.Text <> "" Then
				strValue1 = .Cbo_Arbbegin.Text
				strFieldName = "P.P_ArbBegin"
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sSql += String.Format("{0}{1} = '{2}'", strAndString, strFieldName, strValue1)
				FilterBez += String.Format("Vorschläge mit Arbeitsbeginn = {0}{1}", strValue1, vbLf)
			End If

			' Lohn -----------------------------------------------------------------------------------------------
			If .Cbo_Lohn.Text <> "" Then
				strValue1 = .Cbo_Lohn.Text
				strFieldName = "P.Ab_Lohnbetrag"
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sSql += String.Format("{0}{1} Like '%{2}%'", strAndString, strFieldName, Val(strValue1))
				FilterBez += String.Format("Vorschläge mit Kandidatenlohn wie {0}{1}", strValue1, vbLf)
			End If

			' Tarif -----------------------------------------------------------------------------------------------
			If .Cbo_Tarif.Text <> "" Then
				strValue1 = .Cbo_Tarif.Text
				strFieldName = "P.KD_Tarif"
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sSql += String.Format("{0}{1} Like '%{2}%'", strAndString, strFieldName, strValue1)
				FilterBez += String.Format("Vorschläge mit Kundentarif wie {0}{1}", strValue1, vbLf)
			End If


			' Berater -----------------------------------------------------------------------------------------------
			If .Cbo_KST.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				strValue1 = .Cbo_KST.Text
				strFieldName = "P.KST"
				Dim berater As String = strValue1.Trim.Replace("'", "''").Replace("*", "%")
				If .Cbo_KST.SelectedIndex > -1 Then
					Dim item As ComboValue = CType(.Cbo_KST.SelectedItem, ComboValue)
					berater = item.ComboValue
				End If
				If berater.Contains(",") Then
					sSql += String.Format("{0}{1} In(", strAndString, strFieldName)
					For Each ber In berater.Split(CChar(","))
						sSql += String.Format("'{0}',", ber.Trim)
					Next
					sSql = sSql.Remove(sSql.Length - 1, 1)
					sSql += ")"
				Else
					sSql += String.Format("{0}({1} = '{2}' Or ", strAndString, strFieldName, berater)
					sSql += String.Format("{0} Like '{1}/%' Or ", strFieldName, berater)
					sSql += String.Format("{0} Like '%/{1}')", strFieldName, berater)
				End If
				FilterBez += String.Format("Vorschläge mit Berater wie {0}{1}", strValue1, vbLf)
			End If

			' Alle Disponenten-KST der Filiale  -----------------------------------------------------------------------
			If .Cbo_Filiale.Text <> String.Empty Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				strValue1 = .Cbo_Filiale.Text.Trim
				strFieldName = "P.KST"
				FilterBez += "Filiale wie (" & strValue1 & ") " & vbLf

				sZusatzBez = GetFilialKstData(strValue1)
				sZusatzBez = Replace(sZusatzBez, "'", "")
				Dim strName As String() = Regex.Split(sZusatzBez.Trim, ",")
				Dim strMyName As String = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, " Or ", "")) & strFieldName & " = '" & strName(i) & "'"
					strMyName += CStr(IIf(strMyName.Length > 0, " Or ", "")) & strFieldName & " Like '" & strName(i) & "/%'"
					strMyName += CStr(IIf(strMyName.Length > 0, " Or ", "")) & strFieldName & " Like '%/" & strName(i) & "'"
				Next
				If strName.Length > 0 Then sZusatzBez = strMyName
				If InStr(sZusatzBez, ",") > 0 Then sZusatzBez = Replace(sZusatzBez, ",", ",")
				sSql += strAndString & " (" & sZusatzBez & ")"
			End If

			' Erfasst ab/bis -----------------------------------------------------------------------------------------------
			If .deErfasst_1.Text <> "" Then If Year(CDate(.deErfasst_1.Text)) = 1 Then .deErfasst_1.Text = String.Empty
			If .deErfasst_2.Text <> "" Then If Year(CDate(.deErfasst_2.Text)) = 1 Then .deErfasst_2.Text = String.Empty
			If .deErfasst_1.Text <> "" Or .deErfasst_2.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				strFieldName = "P.CreatedOn"

				Dim erfasstab As String = String.Empty
				Dim erfasstbis As String = String.Empty
				If IsDate(.deErfasst_1.Text) Then
					erfasstab = Date.Parse(.deErfasst_1.Text).ToString("dd.MM.yyyy")
				End If
				If IsDate(.deErfasst_2.Text) Then
					erfasstbis = Date.Parse(.deErfasst_2.Text).ToString("dd.MM.yyyy")
				End If

				' Suche zwischen zwei Datum
				If erfasstab.Length > 0 And erfasstbis.Length > 0 Then
					sSql += String.Format("{0}{1} Between '{2} 00:00' And '{3} 23:59'",
																strAndString, strFieldName, erfasstab, erfasstbis)
					FilterBez += String.Format("Vorschlag erstellt zwischen {0} und {1}{2}", erfasstab, erfasstbis, vbLf)
					' Suche ab erstes Datum
				ElseIf erfasstab.Length > 0 Then
					sSql += String.Format("{0}{1} >= '{2}'", strAndString, strFieldName, erfasstab)
					FilterBez += String.Format("Vorschlag erstellt ab Datum {0}{1}", erfasstab, vbLf)
					' Suche bis zweites Datum
				ElseIf erfasstbis.Length > 0 Then
					sSql += String.Format("{0}{1} <= '{2} 23:59'", strAndString, strFieldName, erfasstbis)
					FilterBez += String.Format("Vorschlag erstellt bis Datum {0}{1}", erfasstbis, vbLf)
				End If
			End If

		End With

		'KUNDEN
		If Not String.IsNullOrWhiteSpace(m_InitialData.UserData.UserFiliale) Then
			Dim usFiliale = m_InitialData.UserData.UserFiliale
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			sSql += String.Format("{0}('{1}' = '' Or MA.MAFiliale Like '%{1}%' Or KD.KDFiliale Like '%{1}%')", strAndString, usFiliale)
			FilterBez += String.Format("Filiale wie ({0}){1}", usFiliale, vbLf)
		End If

		' Mandantennummer -------------------------------------------------------------------------------------------------------
		If ClsDataDetail.MDData.MultiMD = 1 Then
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			sSql += String.Format("{0}P.MDNr = {1}", strAndString, ClsDataDetail.ProgSettingData.SelectedMDNr)
		End If
		ClsDataDetail.GetFilterBez = FilterBez

		m_Logger.LogInfo(String.Format("ProposeSearchWhereQuery: {0}", sSql))

		Return sSql
	End Function

	Function GetSortString(ByVal frmTest As frmProposeSearch) As String
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
						Case 0        ' Nach PNr
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " P.ProposeNr"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ",", "")) & "Vorschlagnummer"
						Case 1        ' Nach Bezeichnung
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " P.Bezeichnung"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ",", "")) & "Bezeichnung"
						Case 2        ' Nach MAName
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " MANachname, MAVorname"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Kandidatenname"
						Case 3        ' Nach Firmenname
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " Firma1"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Firmenname"
						Case 4        ' Nach Vaknr
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " P.VakNr"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Vakanzennummer"
						Case 5        ' Nach Erfassdatum
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " P.CreatedOn"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Erfassdatum"

						Case 6        ' Nach Status
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " P.P_State"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Status"

						Case Else        ' Nach Bezeichnung
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " P.Bezeichnung"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ",", "")) & "Bezeichnung"

					End Select
				Next i
			Else
				strMyName = .CboSort.Text
				strSortBez = String.Format("{0}: {1}", m_xml.GetSafeTranslationValue("Benutzerdefiniert..."), strMyName)
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

	Function GetLstItems(ByVal lst As ListBox) As String
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
'  Public Function ToItem(ByVal cbo As SPProposeSearch.myCbo) As ComboValue
'    If TypeOf (cbo.SelectedItem) Is ComboValue And cbo.SelectedIndex > -1 Then
'      Return DirectCast(cbo.Items(cbo.SelectedIndex), ComboValue)
'    ElseIf cbo.SelectedIndex > -1 Then
'      Dim item As New ComboValue("", "")
'      item.Text = cbo.Items(cbo.SelectedIndex).tostring
'      item.Value = item.Text
'      Return item
'    Else
'      Dim item As New ComboValue("", "")
'      item.Text = cbo.Text
'      item.Value = cbo.Text
'      Return item
'    End If
'  End Function
'End Module