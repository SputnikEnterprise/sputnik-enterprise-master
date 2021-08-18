
Imports System.IO
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports System.Runtime.CompilerServices
Imports DevExpress.XtraEditors.Controls
Imports SP.Infrastructure.Logging

Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports SPESSearch.ClsDataDetail

Imports SPS.Export.Listing.Utility.PVLGAV
Imports SPProgUtility.ProgPath

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

	'// Kandidatenname
	Dim _strMAName As String
	Public Property GetMAName() As String
		Get
			Return _strMAName
		End Get
		Set(ByVal value As String)
			_strMAName = value
		End Set
	End Property

	'// Kandidatenvorname
	Dim _strMAVorname As String
	Public Property GetMAVorname() As String
		Get
			Return _strMAVorname
		End Get
		Set(ByVal value As String)
			_strMAVorname = value
		End Set
	End Property

	'// GAV-Beruf
	Dim _strESGAVBeruf As String
	Public Property GetESGAVBeruf() As String
		Get
			Return _strESGAVBeruf
		End Get
		Set(ByVal value As String)
			_strESGAVBeruf = value
		End Set
	End Property

	'// Einsatz als
	Dim _strESEinsatzAls As String
	Public Property GetESEinsatzAls() As String
		Get
			Return _strESEinsatzAls
		End Get
		Set(ByVal value As String)
			_strESEinsatzAls = value
		End Set
	End Property

	'// Query.GetSearchQuery
	Dim _strTelNr As String
	Public Property GetTelNr() As String
		Get
			Return _strTelNr
		End Get
		Set(ByVal value As String)
			_strTelNr = value
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


#Region "private consts"

	Private Const FORM_XML_DEFAULTVALUES_KEY As String = "Forms_Normaly/Field_DefaultValues"

#End Region


	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	'Private m_xml As New ClsXML
	Private m_mandant As Mandant
	Private m_common As CommonSetting
	Private m_ProgPath As ClsProgPath

	Private m_utility As Utilities
	Private _ClsReg As New SPProgUtility.ClsDivReg

	Public Property mandantNumber As New List(Of Integer)
	Public Property KstData As New List(Of String)

	Private m_SPPVLGAVUtilServiceUrl As String
	Private m_MandantFormXMLFile As String



#Region "Constructor"

	Public Sub New()

		m_utility = New Utilities
		m_mandant = New Mandant
		m_ProgPath = New ClsProgPath

		m_MandantFormXMLFile = m_mandant.GetSelectedMDFormDataXMLFilename(ClsDataDetail.MDData.MDNr)


	End Sub


#End Region


	''' <summary>
	''' listet eine Auflistung der Mandantendaten
	''' </summary>
	''' <returns></returns>
	''' <remarks></remarks>
	Function LoadMandantenData() As IEnumerable(Of MandantenData)
		Dim m_utility As New Utilities
		Dim result As List(Of MandantenData) = Nothing
		m_mandant = New Mandant

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
					advisorData.MDConnStr = m_mandant.GetSelectedMDData(advisorData.MDNr).MDDbConn

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

	Private ReadOnly Property ESCustomerMinMarge() As Boolean
		Get
			Dim calculatecustomerrefundinmarge As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/calculatecustomerrefundinmarge", FORM_XML_DEFAULTVALUES_KEY)), False)

			Return calculatecustomerrefundinmarge
		End Get
	End Property

#Region "Funktionen zur Suche nach Daten..."

	''' <summary>
	''' Dynamische SQL-Query-Zusammenstellung der benötigten Tabellen.
	''' Resultat wird in einer temporäre Tabelle _Einsatzliste_[UserNr] geschrieben.
	''' </summary>dim cv as 
	''' <param name="frmTest"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function GetStartSQLString(ByVal frmTest As frmESSearch) As String
		Dim sSql As String = String.Empty
		Dim sSqlLen As Integer = 0
		Dim sZusatzBez As String = String.Empty
		Dim i As Integer = 0

		With frmTest
			' Die virtuelle Tabelle muss gelöscht werden, falls bereits erstellt
			sSql += String.Format("Begin Try Drop Table {0} End Try ", ClsDataDetail.SPTabNamenES)
			sSql += "Begin Catch End Catch " ' Bei Fehler soll gar nichts passieren
			sSql += String.Format("Begin Try Drop Table {0} End Try ", ClsDataDetail.SPTabNamenESKSTStatistik)
			sSql += "Begin Catch End Catch " ' Bei Fehler soll gar nichts passieren
			sSql += "Begin Try Drop Table #Einsatzliste End Try Begin Catch End Catch "

			sSql += "Select ES.ID as ESID, ES.ESNr, ES.KDNr, ES.MANr, ES.ES_Als, ES.ES_Ab, ES.ES_Ende, "
			sSql += "ES.Suva, ES.ESBranche, "
			sSql += "ES.ESKst1, ES.ESKst2, ES.ESKst, "
			sSql += "IsNull(ES.ESVerBacked, 0) As ESVerBacked, IsNull(ES.VerleihBacked, 0) As VerleihBacked, "

			sSql += "ESLohn.Stundenlohn, ESLohn.MATSpesen, "
			sSql &= "ESLohn.Tarif As Tarif_0, "

			Dim bES_KD_UmsMin As Boolean = ESCustomerMinMarge
			Dim strVerSQL As String = ""
			strVerSQL = "(case "
			strVerSQL &= "When IsNull(KD.KD_UmsMin, 0) = 0 then ESLohn.Tarif "
			strVerSQL &= "else (ESLohn.Tarif-(ESLohn.Tarif * KD.KD_UmsMin / 100)) "
			strVerSQL &= "end) as Tarif"
			sSql &= String.Format("{0}, ", If(bES_KD_UmsMin, strVerSQL, "ESLohn.Tarif As Tarif"))

			sSql += "ESLohn.KDTSpesen, ESLohn.MAStdSpesen, ESLohn.LOVon, KDZHDNr As ESKDZHDNr, VakNr, ProposeNr,  "

			sSql += "ESLohn.GAVNr GAVNumber, ESLohn.GAVGruppe0, ESLohn.GAVGruppe1, ESLohn.GAVGruppe2, "
			sSql += "ESLohn.GAVGruppe3, ESLohn.GAVBezeichnung, ESLohn.GAVKanton, "
			sSql += "ESLohn.GAVStdLohn, ESLohn.GAV_StdWeek, ESLohn.GAV_StdMonth, "
			sSql += "ESLohn.GAV_StdYear, "

			sSql += "ESLohn.BruttoMarge, ESLohn.MargeMitBVG, "
			sSql += "ESLohn.Grundlohn, ESLohn.FerienProz, ESLohn.Ferien, ESLohn.FeierProz, ESLohn.Feier, "
			sSql += "ESLohn.Lohn13Proz, ESLohn.Lohn13, ESLohn.MwStBetrag, ESLohn.AktivLODaten, ESLohn.GAVInfo_String, "
			sSql += "IsNull(ESLohn.ESDoc_Guid, '') As ESDoc_Guid, IsNull(ESLohn.VerleihDoc_Guid, '') As VerleihDoc_Guid, "

			sSql += "MA.Nachname, MA.Vorname, "
			sSql += "MA.PLZ As MAPLZ, MA.Ort As MAOrt, "
			sSql += "MA.Land As MALand, MA.Postfach As MAPostfach, "
			sSql += "MA.Strasse As MAStrasse, MA.Wohnt_bei As MACo, MA.Telefon_G, "
			sSql += "MA.Telefon_P, MA.Natel As MANatel, IsNull(MA.MA_SMS_Mailing, 0) As MA_SMS_Mailing, "
			sSql += "MA.GebDat, MA.AHV_Nr, MA.AHV_Nr_New, "
			sSql += "MA.Geschlecht, MA.Beruf As MABeruf, "
			sSql += "MA.Nationality, MA.Zivilstand, "

			sSql += "MA.Bewillig, MA.Bew_Bis, "

			sSql += "MA.S_Kanton, "
			sSql += "MA.eMail As MAeMail, "
			sSql += "(SELECT Top 1 IsNull(Notice_Employment, '') FROM dbo.tbl_MA_Notices Where tbl_MA_Notices.EmployeeNumber = MA.MANr) AS Employee_Notice_Employment, "
			sSql += "MAKK.GotDocs, MAKK.BriefAnrede As MABriefAnrede, MALo.AHVAnAm, "

			sSql += "KD.Firma1, KD.Kreditlimite, KD.Kreditlimite_2, KD.Bemerkung AS Customer_Notice_Employment, "
			sSql += "KD.KreditlimiteAb, KD.KreditlimiteBis, "
			sSql += "KD.Strasse As KDStrasse, KD.Postfach As KDPostfach, "
			sSql += "KD.PLZ As KDPLZ, KD.Ort As KDOrt, "
			sSql += "KD.Telefon As KDTelefon, "
			sSql += "KD.Telefax As KDTelefax, KD.EMail As KDEmail, "

			sSql += "KDZ.Nachname As KDZNachname, KDZ.Vorname As KDZVorname, KDZ.Anrede As KDZAnrede, KDZ.AnredeForm As KDZAnredeform, KDZ.EMail As KDZeMail, "

			sSql += "KD.Land As KDLand, KD.FProperty, IsNull(KD.KD_UmsMin, 0) As KD_UmsMin, KD.KL_RefNr As Kredit_RefNr, "
			sSql += "[Einsatzdaten] = (Convert(varchar(10), ES.ES_Ab,104) + '-' + Convert(varchar(10), ES.ES_Ende, 104)) "

			' Filialen der Berater hinzufügen
			sSql += ", IsNull((SELECT Top 1 Bezeichnung FROM US_Filiale "
			sSql += "         WHERE "
			sSql += "         USNR = (SELECT TOP 1 USNR FROM Benutzer "
			sSql += "                 WHERE "
			sSql += "                 KST Like Substring(ES.ESKst, 0, Charindex('/', ES.ESKst)) Or "
			sSql += "                 KST Like ES.ESKst) "
			sSql += "         ), '') as Filiale1 "
			sSql += ", IsNull((SELECT Top 1 Bezeichnung FROM US_Filiale "
			sSql += "         WHERE "
			sSql += "         USNR = (SELECT TOP 1 USNR FROM Benutzer "
			sSql += "                 WHERE "
			sSql += "                 KST Like Substring(ES.ESKst,Charindex('/',ES.ESKst)+1, Len(ES.ESKst)) Or "
			sSql += "                 KST Like ES.ESKst) "
			sSql += "         ), '') as Filiale2, "

			'' MABerater
			sSql += "ISNULL(( SELECT TOP 1 " &
												"(Nachname + ', '+Vorname) AS USName " &
			"FROM Benutzer " &
								 "WHERE  USNR = ( SELECT TOP 1 " &
			"USNR " &
			"FROM Benutzer " &
																 "WHERE  KST LIKE SUBSTRING(ES.ESKst, 0, " &
																													 "CHARINDEX('/', " &
																															"ES.ESKst)) " &
																				"OR KST LIKE ES.ESKst " &
															 ") " &
							 "), '') AS MABerater , "

			'' KDBerater
			sSql += "ISNULL(( SELECT TOP 1 " &
											 "(Nachname + ', '+Vorname) AS USName " &
		 "FROM Benutzer " &
								 "WHERE  USNR = ( SELECT TOP 1 " &
			"USNR " &
			"FROM Benutzer " &
																 "WHERE  KST LIKE SUBSTRING(ES.ESKst, " &
																													 "CHARINDEX('/', " &
																															"ES.ESKst) + 1, " &
																													 "LEN(ES.ESKst)) " &
																				"OR KST LIKE ES.ESKst " &
															 ") " &
							 "), '') AS KDBerater "


			sSql += ", 0 as Geteilt, 0 As FilialGeteilt "

			' Angaben für BVG-Begin aus der Lohndatenbank (tbl_LOBVG) holen...
			sSql &= ", IsNull((Select Top 1 BVG.esBegin From tbl_LOBVGData BVG Where BVG.MANr = ES.MANr " &
	"And convert(datetime, '01.' + convert(nvarchar(2), BVG.LP) + '.' + convert(nvarchar(4), BVG.Jahr), 103) <= ES.ES_Ab Order By BVG.Jahr, BVG.LP), " &
		 "DateAdd(Month, 3, ES.ES_Ab)) As LO_ESBegin, "
			sSql &= "IsNull((Select Top 1 BVG.BVGBegin From tbl_LOBVGData BVG Where BVG.MANr = ES.MANr " &
	"And convert(datetime, '01.' + convert(nvarchar(2), BVG.LP) + '.' + convert(nvarchar(4), BVG.Jahr), 103) <= ES.ES_Ab Order By BVG.Jahr, BVG.LP), " &
		 "DateAdd(Month, 3, ES.ES_Ab)) As LO_BVGBegin, "

			' Die Farbe der Hauptmaske für die Einsätze übernehmen
			' Die SQL-String ist auf der Datenbank
			Dim conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
			Dim cmdFarbe As SqlCommand = New SqlCommand("SELECT CaseString FROM TBL_COLORS WHERE Modulname='ES0'", conn)
			conn.Open()
			sSql += cmdFarbe.ExecuteScalar().ToString
			conn.Close()

			' Resultat in einer temporäre #Tabelle ablegen
			sSql += "Into #Einsatzliste "

			sSql += "From ES Left Join ESLohn On ES.ESNr = ESLohn.ESNr "
			sSql += "Left Join Mitarbeiter MA On "
			sSql += "ES.MANr = MA.MANr "
			sSql += "Left Join MAKontakt_Komm MAKK On ES.MANr = MAKK.MANr "

			sSql += "Left Join Kunden KD On "
			sSql += "ES.KDNr = KD.KDNr "
			sSql += "Left Join KD_Zustaendig KDZ On "
			sSql += "ES.KDNr = KDZ.KDNr And KDZHDNr = KDZ.RecNr "

			' Zusätzliche benötigte Tabellen -------------------------------------------
			'===========================================================================

			'' MA_LOSETTING
			'If .Cbo_MALohnSperren.SelectedIndex > 0 Or _
			'   .Cbo_MARapporteDrucken.SelectedIndex > 0 Or _
			'   .Cbo_MAVorschussSperren.SelectedIndex > 0 Then

			sSql += " Left Join MA_LOSetting MALo On ES.MANr = MALo.MANr"
			'End If

			'===========================================================================

			sSqlLen = Len(sSql)

		End With

		Return sSql
	End Function

	Function GetQuerySQLString(ByVal sSQLQuery As String, ByVal frmTest As frmESSearch) As String
		Dim sSql As String = String.Empty
		Dim sOldQuery As String = sSQLQuery

		Dim FilterBez As String = String.Empty
		Dim sSqlLen As Integer = 0
		Dim sZusatzBez As String = String.Empty
		Dim strAndString As String = String.Empty

		Dim strUSFiliale As String = ClsDataDetail.UserData.UserFiliale
		Dim iSQLLen As Integer = Len(sSQLQuery)
		Dim strFieldName As String = String.Empty
		Dim strSearchValue As String = String.Empty
		Dim cv As ComboValue


		With frmTest
			' Standard --------------------------------------------------------------------------------------------------------


			' 1. Seite === Allgemeine ============================================================================================
			' ====================================================================================================================
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString

			' Einsatznummer ESNr ------------------------------------------------------------------------
			Dim esnr1 As String = .txtESESNr_1.Text.Replace("'", "").Replace("*", "%").Trim
			Dim esnr2 As String = .txtESESNr_2.Text.Replace("'", "").Replace("*", "%").Trim

			If esnr1 = "" And esnr2 = "" Then   'do nothing
				' Suche ESNr mit Sonderzeichen
			ElseIf esnr1.Contains("%") Then
				FilterBez = String.Format("{0}Einsätze wie ({1}){2}", strAndString, esnr1, vbLf)
				sSql += String.Format("{0}ES.ESNr Like '{1}'", strAndString, esnr1)
				' Suche ESNr mit Komma getrennt
			ElseIf InStr(esnr1, ",") > 0 Then
				FilterBez = String.Format("Mitarbeiter wie ({0}){1}", esnr1, vbLf)
				sSql += String.Format("{0}ES.ESNr In (", strAndString)
				For Each esnr In esnr1.Split(CChar(","))
					sSql += String.Format("'{0}',", esnr)
				Next
				sSql = sSql.Substring(0, sSql.Length - 1)
				sSql += ")"
				' Suche genau eine ESNr
			ElseIf esnr1 = esnr2 Then
				FilterBez = String.Format("Einsatz mit Nr = {0}{1}", esnr1, vbLf)
				sSql += String.Format("{0}ES.ESNr = '{1}'", strAndString, esnr1)
				' Suche ab ESNr
			ElseIf esnr1 <> "" And esnr2 = "" Then
				FilterBez = String.Format("Einsatz ab Nr {0}{1}", esnr1, vbLf)
				sSql += String.Format("{0}ES.ESNr >= '{1}'", strAndString, esnr1)
				' Suche bis ESNr
			ElseIf esnr1 = "" And esnr2 <> "" Then
				FilterBez = String.Format("Einsatz bis Nr {0}{1}", esnr2, vbLf)
				sSql += String.Format("{0}ES.ESNr <= '{1}'", strAndString, esnr2)
				' Suche zwischen erste und zweite ESNr
			Else
				FilterBez = String.Format("Einsätze zwischen Nr {0} und {1}{2}", esnr1, esnr2, vbLf)
				sSql += String.Format("{0}ES.ESNr Between '{1}' And '{2}'", strAndString, esnr1, esnr2)
			End If

			' Mitarbeiternummer MANr --------------------------------------------------------------------
			Dim manr1 As String = .txtESMANr_1.Text.Replace("'", "").Replace("*", "%").Trim
			Dim manr2 As String = .txtESMANr_2.Text.Replace("'", "").Replace("*", "%").Trim
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString

			If manr1 = String.Empty And manr2 = String.Empty Then       ' do nothing
				' Suche MANr mit Sonderzeichen
			ElseIf manr1.Contains("%") Then
				FilterBez += String.Format("Einsatz mit MANr wie ({0}){1}", manr1, vbLf)
				sSql += String.Format("{0}ES.MANr Like '{1}'", strAndString, manr1)
				' Suche mehrere MANr getrennt durch Komma
			ElseIf InStr(manr1, ",") > 0 Then
				FilterBez += String.Format("Einsätze mit MANr wie ({0}){1}", manr1, vbLf)
				sSql += String.Format("{0} ES.MANr In (", strAndString)
				For Each manr In manr1.Split(CChar(","))
					sSql += String.Format("'{0}',", manr)
				Next
				sSql = sSql.Substring(0, sSql.Length - 1)
				sSql += ")"
				' Suche Einsätze von genau einer MANr
			ElseIf manr1 = manr2 Then
				FilterBez += String.Format("Einsatz mit MANr = {0}{1}", manr1, vbLf)
				sSql += String.Format("{0}ES.MANr = '{1}'", strAndString, manr1)
				' Suche Einsätze ab MANr
			ElseIf manr1 <> "" And manr2 = "" Then
				FilterBez += String.Format("Einsätze ab MANr {0}{1}", manr1, vbLf)
				sSql += String.Format("{0}ES.MANr >= '{1}'", strAndString, manr1)
				' Suche Einsätze bis MANr
			ElseIf manr1 = "" And manr2 <> "" Then
				FilterBez += String.Format("Einsätze bis MANr {0}{1}", manr2, vbLf)
				sSql += String.Format("{0}ES.MANr <= '{1}'", strAndString, manr2)
				' Suche Einsätze zwischen zwei MANr 
			Else
				FilterBez = String.Format("Einsätze zwischen MANr {0} und {1}{2}", manr1, manr2, vbLf)
				sSql += String.Format("{0}ES.MANr Between '{1}' And '{2}'",
					 strAndString, manr1.Trim, manr2)
			End If

			' Kundennummer -------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			Dim kdnr1 As String = .txtESKDNr_1.Text.Replace("'", "").Replace("*", "%").Trim
			Dim kdnr2 As String = .txtESKDNr_2.Text.Replace("'", "").Replace("*", "%").Trim

			If kdnr1 = "" And kdnr2 = "" Then   'do nothing
				' Suche KDNr mit Sonderzeichen 
			ElseIf kdnr1.Contains("%") Then
				FilterBez += String.Format("Einsatz mit KDNr wie ({0}){1}", kdnr1, vbLf)
				sSql += String.Format("{0}ES.KDNr Like '{1}'", strAndString, kdnr1)
				' Suche mehrere KDNr getrennt durch Komma
			ElseIf InStr(kdnr1, ",") > 0 Then
				FilterBez += String.Format("Einsätze mit KDNr wie ({0}){1}", kdnr1, vbLf)
				sSql += String.Format("{0}ES.KDNr In (", strAndString)
				For Each kdnr In kdnr1.Split(CChar(","))
					sSql += String.Format("'{0}',", kdnr)
				Next
				sSql = sSql.Substring(0, sSql.Length - 1)
				sSql += ")"
				' Suche Einsätze mit genau einer KDNr
			ElseIf kdnr1 = kdnr2 Then
				FilterBez += String.Format("Einsatz mit KDNr = {0}{1}", kdnr1, vbLf)
				sSql += String.Format("{0}ES.KDNr = '{1}'", strAndString, kdnr1)
				' Suche Einsätze ab KDNr
			ElseIf kdnr1 <> "" And kdnr2 = "" Then
				FilterBez += String.Format("Einsätze ab KDNr {0}{1}", kdnr1, vbLf)
				sSql += String.Format("{0}ES.KDNr >= '{1}'", strAndString, kdnr1)
				' Suche Einsätze bis KDNr
			ElseIf kdnr1 = "" And kdnr2 <> "" Then
				FilterBez += String.Format("Einsätze bis KDNr {0}{1}", kdnr2, vbLf)
				sSql += String.Format("{0}ES.KDNr <= '{1}'", strAndString, kdnr2)
				' Suche Einsätze zwischen zwei KDNr
			Else
				FilterBez = String.Format("Einsätze mit Kundennummer zwischen {0} und {1}{2}",
																	kdnr1, kdnr2, vbLf)
				sSql += String.Format("{0}ES.KDNr Between '{1}' And '{2}'", strAndString, kdnr1, kdnr2)
			End If

			' Aktiv zwischen -----------------------------------------------------------------------------------------
			If .deAktiv_1.Text <> "" Then If Year(CDate(.deAktiv_1.Text)) = 1 Then .deAktiv_1.Text = String.Empty
			If .deAktiv_2.Text <> "" Then If Year(CDate(.deAktiv_2.Text)) = 1 Then .deAktiv_2.Text = String.Empty
			If .deAktiv_1.Text <> "" Or .deAktiv_2.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				Dim aktivVom As String = ""
				Dim aktivBis As String = ""
				If IsDate(.deAktiv_1.Text) Then
					aktivVom = Date.Parse(.deAktiv_1.Text).ToString("dd.MM.yyyy")
				End If
				If IsDate(.deAktiv_2.Text) Then
					aktivBis = Date.Parse(.deAktiv_2.Text).ToString("dd.MM.yyyy")
				End If

				' Suche zwischen zwei Datum
				If aktivVom.Length > 0 And aktivBis.Length > 0 Then
					'sSql += String.Format("{0}(ES.ES_Ende >= Convert(DateTime, '{1} 00:00', 104)", strAndString, aktivVom)
					'sSql += String.Format(" Or ES.ES_Ende is Null) And ES.ES_Ab <= Convert(DateTime, '{0} 23:59', 104)", aktivBis)
					sSql += String.Format("{0}(ES.ES_Ende >= '{1}' Or ES.ES_Ende Is Null) And ES.ES_Ab <= '{2}'",
																strAndString, aktivVom, aktivBis)
					FilterBez += String.Format("Mitarbeiter aktiv zwischen {0} und {1}{2}", aktivVom, aktivBis, vbLf)
					' Suche ab erstes Datum
				ElseIf aktivVom.Length > 0 Then
					'sSql += String.Format("{0}ES.ES_Ab >= Convert(DateTime, '{1} 00:00', 104)", strAndString, aktivVom)
					sSql += String.Format("{0}ES.ES_Ab >= '{1}'", strAndString, aktivVom)
					FilterBez += String.Format("Mitarbeiter aktiv ab Datum {0}{1}", aktivVom, vbLf)
					' Suche bis zweites Datum
				ElseIf aktivBis.Length > 0 Then
					'sSql += String.Format("{0}ES.ES_Ende <= Convert(DateTime, '{1} 23:59', 104)", strAndString, aktivBis)
					sSql += String.Format("{0}ES.ES_Ende <= '{1}'", strAndString, aktivBis)
					FilterBez += String.Format("Mitarbeiter aktiv bis Datum {0}{1}", aktivBis, vbLf)
				End If
			End If

			' Erfasst am/bis --------------------------------------------------------------------------------------------------
			If .deCreated_1.Text <> "" Then If Year(CDate(.deCreated_1.Text)) = 1 Then .deCreated_1.Text = String.Empty
			If .deCreated_2.Text <> "" Then If Year(CDate(.deCreated_2.Text)) = 1 Then .deCreated_2.Text = String.Empty
			If .deCreated_1.Text.Length > 0 Or .deCreated_2.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				Dim erfasstAm As String = ""
				Dim erfasstBis As String = ""
				If IsDate(.deCreated_1.Text) Then
					erfasstAm = Date.Parse(.deCreated_1.Text).ToString("dd.MM.yyyy")
				End If
				If IsDate(.deCreated_2.Text) Then
					erfasstBis = Date.Parse(.deCreated_2.Text).ToString("dd.MM.yyyy")
				End If
				' Suche zwischen zwei Datum
				If erfasstAm.Length > 0 And erfasstBis.Length > 0 Then
					sSql += String.Format("{0}ES.CreatedOn Between Convert(DateTime, '{1} 00:00', 104) And Convert(DateTime, '{2} 23:59', 104)",
					 strAndString, erfasstAm, erfasstBis)
					FilterBez += String.Format("Mitarbeiter erfasst zwischen {0} und {1}{2}", erfasstAm, erfasstBis, vbLf)
					' Suche ab erstes Datum
				ElseIf erfasstAm.Length > 0 Then
					sSql += String.Format("{0}ES.CreatedOn >= Convert(DateTime,'{1} 00:00', 104)", strAndString, erfasstAm)
					FilterBez += String.Format("Mitarbeiter erfasst ab Datum {0}{1}", erfasstAm, vbLf)
					' Suche bis zweites Datum
				ElseIf erfasstBis.Length > 0 Then
					sSql += String.Format("{0}ES.CreatedOn <= Convert(DateTime,'{1} 23:59', 104)", strAndString, erfasstBis)
					FilterBez += String.Format("Mitarbeiter erfasst bis Datum {0}{1}", erfasstBis, vbLf)
				End If
			End If


			' Kundenkostenstell -------------------------------------------------------------------------------------------------------
			If Me.KstData.Count > 0 Then

				Dim kstdata As String = ConvListObject2String(Me.KstData, ", ")
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sSql += String.Format("{0}ESLohn.KSTBez In ({1})", strAndString, kstdata)

				FilterBez += String.Format(m_Translate.GetSafeTranslationValue("Einsätze mit folgenden Kunden-Kostenstellen: ({1}){0}"), vbLf, kstdata)
			End If



			' Kostenstelle 1 -----------------------------------------------------------------------------------------
			If .Cbo_ESKST1.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_ESKST1.SelectedItem, ComboValue)
				strSearchValue = cv.ComboValue.Trim.Replace("'", "").Replace("*", "%")
				If InStr(strSearchValue, ",") > 0 Then
					sSql += String.Format("{0}ES.ESKST1 In (", strAndString)
					For Each kst In strSearchValue.Split(CChar(","))
						sSql += String.Format("'{0}',", kst)
					Next
					sSql = sSql.Remove(sSql.Length - 1, 1)
					sSql += ")"
				Else
					If .Chk_ESKST1.Checked Then
						sSql += String.Format("{0}ES.ESKST1 = '{1}'", strAndString, strSearchValue)
					Else
						sSql += String.Format("{0}ES.ESKST1 Like '%{1}%'", strAndString, strSearchValue)
					End If
				End If
				FilterBez += String.Format("Einsatz mit Kostenstelle 1 = {0}{1}", strSearchValue, vbLf)
			End If

			' Kostenstelle 2 -----------------------------------------------------------------------------------------
			If .Cbo_ESKST2.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				strSearchValue = .Cbo_ESKST2.Text.Trim.Replace("'", "").Replace("*", "%")
				cv = DirectCast(.Cbo_ESKST2.SelectedItem, ComboValue)
				If InStr(strSearchValue, ",") > 0 Then
					sSql += String.Format("{0}ES.ESKST2 In (", strAndString)
					For Each kst In strSearchValue.Split(CChar(","))
						sSql += String.Format("'{0}',", strSearchValue.Trim)
					Next
					sSql = sSql.Remove(sSql.Length - 1, 1)
					sSql += ")"
				Else
					If .Chk_ESKST2.Checked Then
						sSql += String.Format("{0}ES.ESKST2 = '{1}'", strAndString, cv.ComboValue)
					Else
						sSql += String.Format("{0}ES.ESKST2 Like '%{1}%'", strAndString, cv.ComboValue)
					End If
				End If
				FilterBez += String.Format("Einsatz mit Kostenstelle 2 = {0}{1}", cv.ToString, vbLf)
			End If

			' Berater ------------------------------------------------------------------------------------------------
			If .Cbo_ESBerater.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_ESBerater.SelectedItem, ComboValue)
				strSearchValue = cv.Text.Trim.Replace("'", "''").Replace("*", "%")

				If cv.ComboValue.Contains(",") Then
					sSql += String.Format("{0}ES.ESKST In(", strAndString)
					For Each ber In cv.ComboValue.Split(CChar(","))
						sSql += String.Format("'{0}',", ber.Trim)
					Next
					sSql = sSql.Remove(sSql.Length - 1, 1)
					sSql += ")"
				Else
					If .Chk_ESBerater.Checked Then
						sSql += String.Format("{0}(ES.ESKST  = '{1}' Or ", strAndString, cv.ComboValue)
						sSql += String.Format("ES.ESKST  Like '{0}/%' Or ", cv.ComboValue)
						sSql += String.Format("ES.ESKST  Like '%/{0}')", cv.ComboValue)
					Else
						sSql += String.Format("{0}ES.ESKST Like '%{1}%'", strAndString, cv.ComboValue)
					End If
				End If
				FilterBez += String.Format("Einsatz mit Berater = {0}{1}", cv.Text, vbLf)
			End If

			' Suva ---------------------------------------------------------------------------------------------------
			If .Cbo_ESSuvaCode.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_ESSuvaCode.SelectedItem, ComboValue)
				strSearchValue = cv.Text.Substring(0, 2)
				FilterBez += String.Format("Einsätze mit Suva-Code in ({0}){1}", cv.ComboValue, vbLf)
				sSql += String.Format("{0}ES.Suva In ('{1}')", strAndString, cv.ComboValue.Replace(",", "','"))
			End If

			' Alle Disponenten-KST der Filiale  -----------------------------------------------------------------------
			If Not String.IsNullOrWhiteSpace(.Cbo_ESFiliale.Text) AndAlso String.IsNullOrWhiteSpace(.Cbo_ESBerater.Text) Then
				strSearchValue = .Cbo_ESFiliale.Text.Trim
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				strFieldName = "ES.ESKst"
				FilterBez += "Filiale wie (" & strSearchValue & ") " & vbLf

				strSearchValue = GetFilialKstData(strSearchValue)
				strSearchValue = Replace(strSearchValue, "'", "")
				Dim strName As String() = Regex.Split(strSearchValue.Trim, ",")
				Dim strMyName As String = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, " Or ", "")) & strFieldName & " = '" & strName(i) & "'"
					strMyName += CStr(IIf(strMyName.Length > 0, " Or ", "")) & strFieldName & " Like '" & strName(i) & "/%'"
					strMyName += CStr(IIf(strMyName.Length > 0, " Or ", "")) & strFieldName & " Like '%/" & strName(i) & "'"
				Next
				If strName.Length > 0 Then strSearchValue = strMyName
				If InStr(strSearchValue, ",") > 0 Then strSearchValue = Replace(strSearchValue, ",", ",")

				sSql += strAndString & " (" & strSearchValue & ")"

			End If

			' 2. Seite === Sonstiges =============================================================================================
			' ====================================================================================================================


			' Unterschrieben
			' Einsatzvertrag unterschrieben --------------------------------------------------------------------------
			If .Cbo_ESESVertragUntersch.SelectedIndex > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_ESESVertragUntersch.SelectedItem, ComboValue)
				sSql += String.Format("{0}ES.ESVerBacked = {1}", strAndString, cv.ComboValue)   ' .ToItem.Value_1)
				FilterBez += String.Format("Einsatzvertrag = {0}{1}", cv.Text, vbLf)
			End If

			' Verleihvertrag unterschrieben --------------------------------------------------------------------------
			If .Cbo_ESVerleihUntersch.Text <> String.Empty Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_ESVerleihUntersch.SelectedItem, ComboValue)
				sSql += String.Format("{0}ES.VerleihBacked = {1}", strAndString, cv.ComboValue) ' ToItem.Value_1)
				FilterBez += String.Format("Verleihvertrag = {0}{1}", cv.Text, vbLf)
			End If

			' Gedruckt
			' Einsatzvertrag gedruckt --------------------------------------------------------------------------------
			If .Cbo_ESEinsatzvertragGedruckt.Text <> String.Empty Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_ESEinsatzvertragGedruckt.SelectedItem, ComboValue)
				sSql += String.Format("{0}ES.Print_MA = {1}", strAndString, cv.ComboValue) ' ToItem.Value_1)
				FilterBez += String.Format("Einsatzvertrag gedruckt = {0}{1}", cv.Text, vbLf)
			End If
			' Verleihvertrag gedruckt --------------------------------------------------------------------------------
			If .Cbo_ESVerleihvertragGedruckt.Text <> String.Empty Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_ESVerleihvertragGedruckt.SelectedItem, ComboValue)
				sSql += String.Format("{0}ES.Print_KD = {1}", strAndString, cv.ComboValue) ' ToItem.Value_1)
				FilterBez += String.Format("Verleihvertrag gedruckt = {0}{1}", cv.Text, vbLf)
			End If

			' Einsatz als --------------------------------------------------------------------------------------------
			If .Lst_ESEinsatzAls.Items.Count > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = GetLstItems(.Lst_ESEinsatzAls).Replace("'", "''")
				sSql += String.Format("{0}ES.ES_Als In ('", strAndString)
				sSql += Replace(sZusatzBez, "#@", "','") & "')"
				FilterBez += "Berufe in (" & sZusatzBez & ")" & vbLf
			End If

			' Einstufung ---------------------------------------------------------------------------------------------
			If .Cbo_ESEinstufung.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = .Cbo_ESEinstufung.Text.Trim
				sZusatzBez = sZusatzBez.Replace("'", "''")
				FilterBez += String.Format("Einsätze mit Einstufung = {0}{1}", sZusatzBez, vbLf)
				sSql += String.Format("{0}ES.Einstufung = '{1}'", strAndString, sZusatzBez.Replace(",", "','"))
			End If

			' Unterzeichner ---------------------------------------------------------------------------------------------
			If .Cbo_ESUnterzeichner.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				Dim unterzeichner As String = .Cbo_ESUnterzeichner.Text.Trim.Replace("'", "''").Replace("*", "%")
				FilterBez += String.Format("Einsätze mit Unterzeichner = {0}{1}", unterzeichner, vbLf)
				sSql += String.Format("{0}ES.ESUnterzeichner = '{1}'", strAndString, unterzeichner)
			End If

			' Branche -------------------------------------------------------------------------------------------------
			If .Lst_ESBranche.Items.Count > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = GetLstItems(.Lst_ESBranche).Replace("'", "''")
				sSql += String.Format("{0}ES.ESBranche In ('", strAndString)
				sSql += Replace(sZusatzBez, "#@", "','") & "')"
				FilterBez += "Einsätze mit Branchen in (" & sZusatzBez & ")" & vbLf
			End If

			' Nur aktive Lohndaten anzeigen --------------------------------------------------------------------------
			If .ChkESNurAktiveLohn.Checked Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sSql += String.Format("{0}ESLohn.AktivLODaten = 1", strAndString)
				FilterBez += String.Format("Einsätze mit nur aktive Lohndaten anzeigen.{0}", vbLf)
			End If

			' Nicht in der Einsatzliste auflisten --------------------------------------------------------------------------
			If .cbo_ESAuflisten.Text <> String.Empty Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.cbo_ESAuflisten.SelectedItem, ComboValue)
				sSql += String.Format("{0}ES.NoListing = {1}", strAndString, cv.ComboValue) ' ToItem.Value_1)
				FilterBez += String.Format("In der Einsatzliste auflisten = {0}{1}", cv.Text, vbLf)
			Else
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sSql += String.Format("{0}ES.NoListing = 0", strAndString)

			End If

			' GAV-Kanton ---------------------------------------------------------------------------------------------
			If .Cbo_ESGAVKanton.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				Dim gavKanton As String = .Cbo_ESGAVKanton.Text.Trim.Replace("'", "''").Replace("*", "%")
				Dim leereFelder As Boolean = False
				sSql += String.Format("{0}(", strAndString)
				If gavKanton.Contains("Leere Felder") Then
					gavKanton = gavKanton.Replace("Leere Felder", "")
					sSql += "(ESLohn.GAVKanton Is Null Or ESLohn.GAVKanton = '')"
					If gavKanton.Trim.StartsWith(",") Then
						gavKanton = gavKanton.Trim.Remove(0, 1)
					End If
					leereFelder = True
				End If

				If gavKanton <> String.Empty Then
					If leereFelder Then
						sSql += " Or "
					End If
					sSql += "ESLohn.GAVKanton In ("
					For Each gavK As String In gavKanton.Split(CChar(","))
						sSql += String.Format("'{0}',", gavK.Trim)
					Next
					sSql = sSql.Remove(sSql.Length - 1, 1)
					sSql += ")"
				End If

				sSql += ")"
				FilterBez += String.Format("Einsätze mit GAV-Kanton des Mitarbeiters in ({0}){1}", gavKanton, vbLf)
			End If

			' GAV-Berufe ---------------------------------------------------------------------------------------------
			If .Lst_ESGAVBeruf.Items.Count > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = GetLstItems(.Lst_ESGAVBeruf).Replace("'", "''")
				sSql += String.Format("{0}ESLohn.GAVGruppe0 In ('", strAndString)
				sSql += Replace(sZusatzBez, "#@", "','") & "')"
				FilterBez += "Einsätze mit GAV-Berufe in (" & sZusatzBez & ")" & vbLf
			End If

			' 1.Gruppe -----------------------------------------------------------------------------------------------
			If .Cbo_ESGruppe1.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				Dim gruppe1 As String = .Cbo_ESGruppe1.Text.Trim.Replace("'", "''").Replace("*", "%")
				If .Cbo_ESGruppe1.SelectedIndex = 0 Then
					sSql += String.Format("{0}(ESLohn.GAVGruppe1 Is Null Or ESLohn.GAVGruppe1 = '')", strAndString)
				ElseIf .Cbo_ESGruppe1.SelectedIndex > -1 Then
					sSql += String.Format("{0}ESLohn.GAVGruppe1 = '{1}'", strAndString,
																.Cbo_ESGruppe1.Text.Trim.Replace("'", "''").Replace("*", "%"))
				ElseIf gruppe1.Contains("%") Then
					sSql += String.Format("{0}ESLohn.GAVGruppe1 Like '{1}'", strAndString, gruppe1)
				Else
					sSql += String.Format("{0}ESLohn.GAVGruppe1 In ('{1}')", strAndString, gruppe1.Replace(",", "','"))
				End If

				FilterBez += String.Format("Einsätze mit Gruppe 1 in ({0}){1}", gruppe1, vbLf)
			End If


			' 3. Seite === Kandidaten ============================================================================================
			' ====================================================================================================================

			' WOS übermitteln ---------------------------------------------------------------------------------------------
			If .Cbo_MAWOS.Text <> String.Empty Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MAWOS.SelectedItem, ComboValue)
				FilterBez += String.Format("Kandidat mit WOS Pflichtigkeit = {0}{1}", .Cbo_MAWOS.Text, vbLf)
				sSql += String.Format("{0}MA.Send2WOS in ('{1}')", strAndString, cv.ComboValue.Replace(" ", "").Replace(",", "','"))
			End If

			' Web Export ---------------------------------------------------------------------------------------------
			If .Cbo_MAWeb.Text <> String.Empty Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MAWeb.SelectedItem, ComboValue)
				FilterBez += String.Format("Kandidatendaten veröffentlichen = {0}{1}", .Cbo_MAWOS.Text, vbLf)
				sSql += String.Format("{0}MAKK.WebExport in ('{1}')", strAndString, cv.ComboValue.Replace(" ", "").Replace(",", "','"))
			End If

			' Sonstiges: Allgemein: Kommunikationsart------------------------------------------------------------------------------
			If .txt_MAKontaktArten.Text <> String.Empty Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				Dim suchEingabe As String = .txt_MAKontaktArten.Text.Replace(" ", "")
				cv = DirectCast(.cbo_MAKontaktArten.SelectedItem, ComboValue)
				Select Case cv.ComboValue
					Case "0" ' Telefon/Natel/eMail
						sSql += String.Format("{0}Replace(MA.Telefon_p, ' ', '') Like '%{1}%' Or ", strAndString, suchEingabe)
						sSql += String.Format("Replace(MA.Telefon2, ' ', '') Like '%{0}%' Or ", suchEingabe)
						sSql += String.Format("Replace(MA.Telefon3, ' ', '') Like '%{0}%' Or ", suchEingabe)
						sSql += String.Format("Replace(MA.Telefon_G, ' ', '') Like '%{0}%' Or ", suchEingabe)
						sSql += String.Format("Replace(MA.Natel, ' ', '') Like '%{0}%' ", suchEingabe)

					Case "1" ' E-Mail
						sSql += String.Format("{0}MA.EMail Like '%{1}%' ", strAndString, suchEingabe)

					Case "2" ' Homepage
						sSql += String.Format("{0}MA.Homepage Like '%{1}%' ", strAndString, suchEingabe)

				End Select
				FilterBez += String.Format("Kandidaten {0} = {1}{2}", .txt_MAKontaktArten.Text, suchEingabe, vbLf)

			End If

			' Zwischenverdienst -----------------------------------------------------------------------------------------------
			If .Cbo_MAZwischenverdienst.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MAZwischenverdienst.SelectedItem, ComboValue)

				sSql += String.Format("{0}MAKK.InZv = {1}", strAndString, cv.ComboValue)
				FilterBez += String.Format("Kandidaten mit {0} = {1}{2}", .lblZwischenverdienst.Text, cv.Text, vbLf)
			End If

			' ALK-Kassen -----------------------------------------------------------------------------------------------
			If .cboALKKasse.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.cboALKKasse.SelectedItem, ComboValue)

				sSql += String.Format("{0}MAKK.ALKNumber = {1}", strAndString, cv.ComboValue)
				FilterBez += String.Format("Kandidaten mit ALK-Nummer = {0}{1}", cv.Text, vbLf)
			End If


			' Telefon ---------------------------------------------------------------------------------------------
			If .cbo_MANotTelefon.Text <> String.Empty Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.cbo_MANotTelefon.SelectedItem, ComboValue)
				FilterBez += String.Format("Kandidaten-Telefonnummer ist leer? {0}{1}", .cbo_MANotTelefon.Text, vbLf)
				If cv.ComboValue = "1" Then
					sSql += String.Format("{0}(Not Len(MA.Telefon_p) > 5 Or MA.Telefon_p Is Null)", strAndString)
				Else
					sSql += String.Format("{0}Len(MA.Telefon_p) > 5 ", strAndString)
				End If
			End If
			' Natel ---------------------------------------------------------------------------------------------
			If .cbo_MANotNatel.Text <> String.Empty Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.cbo_MANotNatel.SelectedItem, ComboValue)
				FilterBez += String.Format("Kandidaten-Mobilenummer ist leer? {0}{1}", .cbo_MANotNatel.Text, vbLf)
				If cv.ComboValue = "1" Then
					sSql += String.Format("{0}(Not Len(MA.Natel) > 5 Or MA.Natel Is Null)", strAndString)
				Else
					sSql += String.Format("{0}Len(MA.Natel) > 5 ", strAndString)
				End If
			End If
			' EMail ---------------------------------------------------------------------------------------------
			If .cbo_MANotEMail.Text <> String.Empty Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.cbo_MANotEMail.SelectedItem, ComboValue)
				FilterBez += String.Format("Kandidaten-EMail ist leer? {0}{1}", .cbo_MANotEMail.Text, vbLf)
				If cv.ComboValue = "1" Then
					sSql += String.Format("{0}(Not Len(MA.eMail) > 5 Or MA.eMail Is Null)", strAndString)
				Else
					sSql += String.Format("{0}Len(MA.eMail) > 5 ", strAndString)
				End If
			End If




			' Geburtsdaten
			' Geburtsdatum Geboren am --------------------------------------------------------------------------------
			If .deMAGeb_1.Text <> "" Then If Year(CDate(.deMAGeb_1.Text)) = 1 Then .deMAGeb_1.Text = String.Empty
			If .deMAGeb_2.Text <> "" Then If Year(CDate(.deMAGeb_2.Text)) = 1 Then .deMAGeb_2.Text = String.Empty
			If .deMAGeb_1.Text.Length > 0 Or .deMAGeb_2.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				Dim gebDate1 As String = ""
				Dim gebDate2 As String = ""
				If IsDate(.deMAGeb_1.Text) Then
					gebDate1 = Date.Parse(.deMAGeb_1.Text).ToString("dd.MM.yyyy")
				End If
				If IsDate(.deMAGeb_2.Text) Then
					gebDate2 = Date.Parse(.deMAGeb_2.Text).ToString("dd.MM.yyyy")
				End If
				If gebDate1.Length > 0 And gebDate2.Length > 0 Then
					' Suche Geburtsdatum zwischen zwei Datum
					sSql += String.Format("{0}MA.GebDat Between Convert(DateTime, '{1} 00:00',104) And Convert(DateTime, '{2} 23:59', 104)",
											strAndString, gebDate1, gebDate2)
					FilterBez += String.Format("Mitarbeiter geboren zwischen {0} und {1}{2}", gebDate1, gebDate2, vbLf)
					' Suche ab erstes Geburtsdatum
				ElseIf gebDate1.Length > 0 Then
					sSql += String.Format("{0}MA.GebDat >= Convert(DateTime,'{1} 00:00',104)", strAndString, gebDate1)
					FilterBez += String.Format("Mitarbeiter ab Geburtsdatum {0}{1}", gebDate1, vbLf)
					' Suche bis zweiten Geburtsdatum
				ElseIf gebDate2.Length > 0 Then
					sSql += String.Format("{0}MA.GebDat <= Convert(DateTime,'{1} 23:59',104)", strAndString, gebDate2)
					FilterBez += String.Format("Mitarbeiter bis Geburtsdatum {0}{1}", .deMAGeb_2.Text, vbLf)
				End If
			End If

			' Geboren im Monat ---------------------------------------------------------------------------------------
			If .Cbo_MAGeborenMonat.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = " "
				For Each monat As String In .Cbo_MAGeborenMonat.Text.Split(CChar(","))
					monat = monat.Replace("'", "").Trim
					If monat.Length > 0 Then
						sZusatzBez += monat + ","
					End If
				Next
				sZusatzBez = sZusatzBez.Substring(0, sZusatzBez.Length - 1)
				If sZusatzBez.Length > 0 Then
					sSql += String.Format("{0}Month(MA.GebDat) in ({1})", strAndString, sZusatzBez.Trim)
					FilterBez += String.Format("Mitarbeiter geboren im Monat {0}{1}", .Cbo_MAGeborenMonat.Text, vbLf)
				End If
			End If

			' AHV-Karte vollständig -------------------------------------------------------------------------------------------
			If .Cbo_MAAHVVollstaendig.SelectedIndex > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MAAHVVollstaendig.SelectedItem, ComboValue)
				If cv.ComboValue = "0" Then
					sSql += String.Format("{0}(Not Len(MA.AHV_Nr) > 11 Or MA.AHV_Nr Is Null)", strAndString)
				Else
					sSql += String.Format("{0}Len(MA.AHV_Nr) > 11 ", strAndString)
				End If
				FilterBez += String.Format("Mitarbeiter mit {0} = {1}{2}", .lblAlteAHVNr.Text, cv.Text, vbLf)
			End If

			' Neue AHV-Karte vollständig --------------------------------------------------------------------------------------
			If .Cbo_MANewAHVVollstaendig.SelectedIndex > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MANewAHVVollstaendig.SelectedItem, ComboValue)
				If cv.ComboValue = "0" Then 'ToItem.Value_1 = "0" Then
					sSql += String.Format("{0}(Not Len(MA.AHV_Nr_New) = 16 Or MA.AHV_Nr_New Is Null)", strAndString)
				Else
					sSql += String.Format("{0}Len(MA.AHV_Nr_New) = 16", strAndString)
				End If
				FilterBez += String.Format("Mitarbeiter mit {0} = {1}{2}", .lblNeueAHVNr.Text, cv.Text, vbLf)
			End If



			' Bewilligung -----------------------------------------------------------------------------------------------------
			' ist heute nicht aktuell --------------------------------------------------------------------------------------
			If .chkMABew.Checked Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sSql += String.Format("{0}( Convert(Date, MA.Bew_Bis) < Convert(Date, GetDate()) )", strAndString)
				FilterBez += String.Format("Mitarbeiter mit keiner gültigen Bewilligung{0}", vbLf)
			End If


			' Bewilligungs-Art
			If Not .luePermissionCode.EditValue Is Nothing AndAlso Not String.IsNullOrWhiteSpace(.luePermissionCode.EditValue) Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				Dim bewCode As String = .luePermissionCode.EditValue.Trim.Replace("'", "''").Replace("*", "%")
				Dim leereFelder As Boolean = False
				sSql += String.Format("{0}(", strAndString)

				If bewCode.Contains("Leere Felder") Then
					bewCode = bewCode.Replace("Leere Felder", "")
					sSql += " (IsNull(MA.Bewillig, '') = '')"
					If bewCode.Trim.StartsWith(",") Then
						bewCode = bewCode.Trim.Remove(0, 1)
					End If
					leereFelder = True
				End If

				If bewCode <> String.Empty Then
					If leereFelder Then
						sSql += " Or "
					End If
					sSql += "MA.Bewillig in ("
					For Each bewC As String In bewCode.Split(CChar(","))
						sSql += String.Format("'{0}',", bewC.Trim)
					Next
					sSql = sSql.Remove(sSql.Length - 1, 1)
					sSql += ")"
				End If
				sSql += ")"
				FilterBez += String.Format("Einsätze mit Mitarbeiter mit Bewilligung = {0}{1}", sZusatzBez, vbLf)

			End If

			' Bewilligung gültig bis Monat
			If .Cbo_MABewBisMonat.Text <> String.Empty Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				' Explizit gültige Bewilligung im Monat
				sSql += String.Format("{0}Month(MA.Bew_Bis) = {1}", strAndString, .Cbo_MABewBisMonat.Text)
				FilterBez += String.Format("Bewilligung gültig genau im Monat {0}{1}",
				 .Cbo_MABewBisMonat.Text, vbLf)
			End If

			' Bewilligung gültig bis Jahr
			If .Cbo_MABewBisJahr.Text <> String.Empty Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				' Explizit gültige Bewilligung in den Jahren
				sSql += String.Format("{0}Year(MA.Bew_Bis) = {1}", strAndString, .Cbo_MABewBisJahr.Text)
				FilterBez += String.Format("Bewilligung gültig im Jahr {0}{1}", .Cbo_MABewBisJahr.Text, vbLf)
			End If

			' Q-Steuer -----------------------------------------------------------------------------------------------
			If Not .lueQSTCode.EditValue Is Nothing AndAlso Not String.IsNullOrWhiteSpace(.lueQSTCode.EditValue) Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				Dim qSteuer As String = .lueQSTCode.EditValue.Trim.Replace("'", "''").Replace("*", "%")

				Dim leereFelder As Boolean = False
				sSql += String.Format("{0}(", strAndString)
				If qSteuer.Contains("Leere Felder") Then
					qSteuer = qSteuer.Replace("Leere Felder", "")
					sSql += "(IsNull(MA.Q_Steuer, '') = '')"
					If qSteuer.Trim.StartsWith(",") Then
						qSteuer = qSteuer.Trim.Remove(0, 1)
					End If
					leereFelder = True
				End If

				If qSteuer <> String.Empty Then
					If leereFelder Then
						sSql += " Or "
					End If
					sSql += "MA.Q_Steuer In ("
					For Each gavK As String In qSteuer.Split(CChar(","))
						sSql += String.Format("'{0}',", gavK.Trim)
					Next
					sSql = sSql.Remove(sSql.Length - 1, 1)
					sSql += ")"
				End If

				sSql += ")"
				FilterBez += String.Format("Einsätze mit Quellensteuer des Mitarbeiters in ({0}){1}", qSteuer, vbLf)
			End If

			' S-Kanton (Steuerkanton) --------------------------------------------------------------------------------
			If Not .lueTaxCanton.EditValue Is Nothing AndAlso Not String.IsNullOrWhiteSpace(.lueTaxCanton.EditValue) Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				Dim sKanton As String = .lueTaxCanton.EditValue.Trim.Replace("'", "''").Replace("*", "%")
				Dim leereFelder As Boolean = False
				sSql += String.Format("{0}(", strAndString)
				If sKanton.Contains("Leere Felder") Then
					sKanton = sKanton.Replace("Leere Felder", "")
					sSql += "(IsNull(MA.S_Kanton, '') = '')"
					If sKanton.Trim.StartsWith(",") Then
						sKanton = sKanton.Trim.Remove(0, 1)
					End If
					leereFelder = True
				End If

				If sKanton <> String.Empty Then
					If leereFelder Then
						sSql += " Or "
					End If
					sSql += "MA.S_Kanton In ("
					For Each gavK As String In sKanton.Split(CChar(","))
						sSql += String.Format("'{0}',", gavK.Trim)
					Next
					sSql = sSql.Remove(sSql.Length - 1, 1)
					sSql += ")"
				End If

				sSql += ")"
				FilterBez += String.Format("Einsätze mit Steuerkanton des Mitarbeiters in ({0}){1}", sKanton, vbLf)
			End If

			' Nationalität -------------------------------------------------------------------------------------------
			If Not .lueNationality.EditValue Is Nothing AndAlso Not String.IsNullOrWhiteSpace(.lueNationality.EditValue) Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				Dim code As String = .lueNationality.EditValue.Trim.Replace("'", "''").Replace("*", "%")
				Dim leereFelder As Boolean = False
				sSql += String.Format("{0}(", strAndString)

				If Not String.IsNullOrWhiteSpace(code) Then
					sSql += "MA.Nationality In ("

					For Each itm In code.Split(",")
						sSql += String.Format("'{0}',", itm)
					Next
					sSql = sSql.Remove(sSql.Length - 1, 1)

					sSql += ")"

				End If

				sSql += ")"
				FilterBez += String.Format("Einsätze mit Nationalität des Mitarbeiters in ({0}){1}", code, vbLf)
			End If

			' Zivilstand ---------------------------------------------------------------------------------------------
			If Not .lueCivilstate.EditValue Is Nothing AndAlso Not String.IsNullOrWhiteSpace(.lueCivilstate.EditValue) Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				Dim code = .lueCivilstate.EditValue
				FilterBez += String.Format("Mitarbeiter mit Zivilstand = {0}{1}", .lueCivilstate.Text, vbLf)
				sSql += String.Format("{0}MA.Zivilstand in ('{1}')", strAndString, code.Replace(" ", "").Replace(",", "','"))
			End If

			' Herkunftsland des Mitarbeiter --------------------------------------------------------------------------
			If Not .lueCountry.EditValue Is Nothing AndAlso Not String.IsNullOrWhiteSpace(.lueCountry.EditValue) Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = .lueCountry.EditValue.Replace("'", "''").Trim
				Dim leereFelder As Boolean = False
				sSql += String.Format("{0}(", strAndString)

				If Not String.IsNullOrWhiteSpace(sZusatzBez) Then
					sSql += "MA.Land In ("

					For Each itm In sZusatzBez.Split(",")
						sSql += String.Format("'{0}',", itm)
					Next
					sSql = sSql.Remove(sSql.Length - 1, 1)

					sSql += ")"

				End If
				sSql += ")"

				FilterBez += String.Format("Einsätze mit Herkunftsland des Mitarbeiters in ({0}){1}",
																	 .lueCountry.EditValue, vbLf)
			End If

			' Korrespondenz-Sprache -------------------------------------------------------------------------------------------
			If .Cbo_MAKorrSprache.Text <> String.Empty Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				Dim korrSprache As String = .Cbo_MAKorrSprache.Text.Trim.Replace("'", "''").Replace("*", "%")
				Dim leereFelder As Boolean = False
				sSql += String.Format("{0}(", strAndString)
				If korrSprache.Contains("Leere Felder") Then
					korrSprache = korrSprache.Replace("Leere Felder", "")
					sSql += "(MA.Sprache Is Null Or MA.Sprache = '')"
					If korrSprache.Trim.StartsWith(",") Then
						korrSprache = korrSprache.Trim.Remove(0, 1)
					End If
					leereFelder = True
				End If

				If korrSprache <> String.Empty Then
					If leereFelder Then
						sSql += " Or "
					End If
					sSql += "MA.Sprache In ("
					For Each korrS As String In korrSprache.Split(CChar(","))
						sSql += String.Format("'{0}',", korrS.Trim)
					Next
					sSql = sSql.Remove(sSql.Length - 1, 1)
					sSql += ")"
				End If

				sSql += ")"
				FilterBez += String.Format("Einsätze mit Korrespondenz-Sprache des Mitarbeiters in ({0}){1}", korrSprache, vbLf)
			End If

			' Sperrung Lohn ---------------------------------------------------------------------------------------------------
			If .Cbo_MALohnSperren.SelectedIndex > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MALohnSperren.SelectedItem, ComboValue)
				FilterBez += String.Format("Einsätze mit Lohnsperrung = {0}{1}", cv.Text, vbLf)
				sSql += String.Format("{0}MALo.NoLO = {1}", strAndString, cv.ComboValue)
			End If

			' Sperrung Vorschuss ----------------------------------------------------------------------------------------------
			If .Cbo_MAVorschussSperren.SelectedIndex > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MAVorschussSperren.SelectedItem, ComboValue)
				FilterBez += String.Format("Einsätze mit Vorschusssperrung = {0}{1}", cv.Text, vbLf)
				sSql += String.Format("{0}MALo.NoZG = {1}", strAndString, cv.ComboValue)
			End If

			' Rapporte drucken ------------------------------------------------------------------------------------------------
			If .Cbo_MARapporteDrucken.SelectedIndex > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MARapporteDrucken.SelectedItem, ComboValue)
				If cv.ComboValue = "0" Then
					sSql += String.Format("{0}(MALo.NoRPPrint = 0 Or MALo.NoRPPrint Is Null)", strAndString)
				Else
					sSql += String.Format("{0}MALo.NoRPPrint = 1", strAndString)
				End If
				FilterBez += String.Format("Einsätze mit Kandidaten-Rapporte nicht drucken = {0}{1}", cv.Text, vbLf)
			End If

			' Unterschreitungslimite unterschritten ----------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If .chk_MAUnterschreitungslimiteUnterschritten.Checked Then

				' Alle Kandidaten, die eine Unterschreitungslimite haben, oder in der Mandantenverwaltung angegebene globale
				' Limite haben und diese überschritten ist.
				Dim connection As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
				Dim sqlKreditlimite As String = "[List MANR Unterschreitungslimite unterschritten]"
				Dim cmd As SqlCommand = New SqlCommand(sqlKreditlimite, connection)
				cmd.CommandType = CommandType.StoredProcedure
				cmd.Parameters.AddWithValue("@filiale", ClsDataDetail.UserData.UserFiliale)

				connection.Open()

				Dim reader As SqlDataReader = cmd.ExecuteReader()
				If reader.HasRows Then
					sSql += String.Format("{0} MA.MANR In (", strAndString)
					While reader.Read
						sSql += String.Format("{0},", reader("MANR"))
					End While
					sSql = sSql.Remove(sSql.Length - 1, 1)
					sSql += ") "
					FilterBez = String.Format("Unterschreitungslimite unterschritten {0}", vbLf)
				End If
				connection.Close()
			End If

			' Bankverbindung --------------------------------------------------------------------------------------------------
			If .Cbo_MABankverbindung.Text <> String.Empty Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				Dim connection As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
				' Alle Kandidaten, die Zahlungsart K und eine Bankverbindung haben oder nicht
				Dim sqlBankverbindung As String = "[List MANR Bankverbindung]"
				cv = DirectCast(.Cbo_MABankverbindung.SelectedItem, ComboValue)

				FilterBez = String.Format("Kandidaten mit Zahlungsart K und Bankverbindung = {1}{0}", vbLf, .Cbo_MABankverbindung.Text)

				Dim cmd As SqlCommand = New SqlCommand(sqlBankverbindung, connection)
				cmd.CommandType = CommandType.StoredProcedure
				cmd.Parameters.AddWithValue("@mitOhne", CInt(Val(cv.ComboValue)))
				cmd.Parameters.AddWithValue("@zahlungsart", "K")
				cmd.Parameters.AddWithValue("@filiale", ClsDataDetail.UserData.UserFiliale)

				connection.Open()

				Dim reader As SqlDataReader = cmd.ExecuteReader()
				If reader.HasRows Then
					sSql += String.Format("{0} MA.MANR In (", strAndString)
					While reader.Read
						sSql += String.Format("{0},", reader("MANR"))
					End While
					sSql = sSql.Remove(sSql.Length - 1, 1)
					sSql += ") "
				End If

				connection.Close()
			End If

			' Qualifikationsnachweis ---------------------------------------------------------------------------------------------
			If .Cbo_MAQualifikationNachweis.Text <> String.Empty Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MAQualifikationNachweis.SelectedItem, ComboValue)
				FilterBez += String.Format("Kandidaten Qualifikationsnachweis = {0}{1}", .Cbo_MAQualifikationNachweis.Text, vbLf)
				sSql += String.Format("{0}MAKK.GotDocs in ('{1}')", strAndString, cv.ComboValue.Replace(" ", "").Replace(",", "','"))
			End If

			' Kontakt ---------------------------------------------------------------------------------------------------------
			If .Cbo_MAKontakt.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = .Cbo_MAKontakt.Text.Trim.Replace("'", "''")
				Dim leereFelder As Boolean = False
				If Not leereFelder Then sSql += strAndString
				sSql += "MAKK.KontaktHow Not In ("
				For Each kontakt In sZusatzBez.Split(CChar(","))
					sSql += String.Format("'{0}',", kontakt.Trim)
				Next
				sSql = sSql.Substring(0, sSql.Length - 1)
				sSql += ") "

				FilterBez += String.Format(m_Translate.GetSafeTranslationValue("Mitarbeiter nicht mit den Kontaktarten {0}{1}"),
																	 .Cbo_MAKontakt.Text, vbLf, .lblMAKontakt.Text)
			End If

			' 1. Status -------------------------------------------------------------------------------------------------------
			If .Cbo_MAStatus1.Text <> String.Empty Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = .Cbo_MAStatus1.Text.Trim.Replace(", ", ",").Replace("'", "''")
				Dim leereFelder As Boolean = False
				If Not leereFelder Then sSql += strAndString
				sSql += "MAKK.KStat1 Not In ("
				For Each strText As String In sZusatzBez.Split(CChar(","))
					sSql += String.Format("'{0}',", strText.Trim)
				Next
				sSql = sSql.Remove(sSql.Length - 1, 1)
				sSql += ")"
				FilterBez += String.Format(m_Translate.GetSafeTranslationValue("Mitarbeiter nicht mit {2} wie {0}{1}"), .Cbo_MAStatus1.Text, vbLf, .lblMA1Status.Text)
			End If

			' 2. Status -------------------------------------------------------------------------------------------------------
			If .Cbo_MAStatus2.Text <> String.Empty Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = .Cbo_MAStatus2.Text.Trim.Replace(", ", ",").Replace("'", "''")
				Dim leereFelder As Boolean = False
				If Not leereFelder Then sSql += strAndString
				sSql += "MAKK.KStat2 Not In ("
				For Each strText As String In sZusatzBez.Split(CChar(","))
					sSql += String.Format("'{0}',", strText.Trim)
				Next
				sSql = sSql.Remove(sSql.Length - 1, 1)
				sSql += ")"

				FilterBez += String.Format(m_Translate.GetSafeTranslationValue("Mitarbeiter nicht mit {2} wie {0}{1}"), .Cbo_MAStatus2.Text, vbLf, .lblMA2Status.Text)
			End If




			' 4. Seite === Kunden ================================================================================================
			' ====================================================================================================================

			If .Cbo_KDKanton.Text <> String.Empty Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				Dim kanton As String = .Cbo_KDKanton.Text.Trim.Replace("'", "").Replace("*", "%")
				Dim leereFelder As Boolean = False
				sSql += String.Format("{0}(", strAndString)
				If kanton.Contains("Leere Felder") Then
					kanton = kanton.Replace("Leere Felder", "")
					sSql += "KD.PLZ = ''"
					If kanton.Trim.StartsWith(",") Then
						kanton = kanton.Trim.Remove(0, 1)
					End If
					leereFelder = True
				End If

				If kanton <> String.Empty Then
					If leereFelder Then
						sSql += " Or "
					End If
					sSql += "KD.PLZ in (Select Convert(varchar(10), PLZ.PLZ)"
					sSql += " From PLZ Where PLZ.Kanton In ("
					For Each kt In kanton.Split(CChar(","))
						sSql += String.Format("'{0}',", kt.Trim)
					Next
					sSql = sSql.Remove(sSql.Length - 1, 1)
					sSql += "))"
				End If
				sSql += ")"
				FilterBez += String.Format("Einsatz mit Kunden im Kanton = {0}{1}", .Cbo_KDKanton.Text, vbLf)
			End If

			' Stammdaten
			' Kanton ----------------------------------------------------------------------------------------------------------

			'For Each item As CheckedListBoxItem In .Cbo_ESGAVKanton.Properties.Items
			'  If item.CheckState = CheckState.Checked Then
			'    cv = DirectCast(item.Value, ComboValue)

			'    Dim kanton As String = cv.Text.Trim.Replace("'", "").Replace("*", "%")

			'  End If
			'Next

			' Sprache ---------------------------------------------------------------------------------------------------------
			If .Cbo_KDSprache.Text <> String.Empty Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = .Cbo_KDSprache.Text.Replace("'", "''").Trim
				Dim leereFelder As Boolean = False
				sSql += String.Format("{0}(", strAndString)
				If .Cbo_KDSprache.Text.Contains("Leere Felder") Then
					sZusatzBez = sZusatzBez.Replace("Leere Felder", "")
					sSql += "(KD.Sprache Is Null Or KD.Sprache = '')"
					If sZusatzBez.Trim.StartsWith(",") Then
						sZusatzBez = sZusatzBez.Trim.Remove(0, 1)
					End If
					leereFelder = True
				End If

				If sZusatzBez <> String.Empty Then
					If leereFelder Then
						sSql += " Or "
					End If
					sSql += "KD.Sprache in ("
					For Each land As String In sZusatzBez.Split(CChar(","))
						sSql += String.Format("'{0}',", land.Trim)
					Next
					sSql = sSql.Remove(sSql.Length - 1, 1)
					sSql += ")"
				End If
				sSql += ")"
				FilterBez += String.Format("Einsätze mit Sprache des Kunden in ({0}){1}",
																	 .Cbo_KDSprache.Text, vbLf)
			End If

			' Vermittlung und Verleih
			' Einsatz sperren -------------------------------------------------------------------------------------------------
			If .Cbo_KDEinsatzSperren.SelectedIndex > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_KDEinsatzSperren.SelectedItem, ComboValue)
				FilterBez += String.Format("Einsätze mit Kunden-Einsatz sperren = {0}{1}", cv.Text, vbLf)
				sSql += String.Format("{0}KD.NoES = {1}", strAndString, cv.ComboValue)
			End If
			' Im Web ----------------------------------------------------------------------------------------------------------
			If .Cbo_KDImWeb.SelectedIndex > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_KDImWeb.SelectedItem, ComboValue)
				If cv.ComboValue = "0" Then
					sSql += String.Format("{0}(KD.ExportIE = 0 Or KD.ExportIE Is Null)", strAndString)
				Else
					sSql += String.Format("{0}KD.ExportIE = 1", strAndString)
				End If
				FilterBez += String.Format("Einsätze mit Kunden im Web = {0}{1}", cv.Text, vbLf)
			End If

			' Kredite
			' Kreditwarnung ---------------------------------------------------------------------------------------------------
			If .Cbo_KDKreditwarnung.SelectedIndex > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_KDKreditwarnung.SelectedItem, ComboValue)
				If cv.ComboValue = "0" Then
					sSql += String.Format("{0}(KD.KreditWarnung = 0 Or KD.KreditWarnung Is Null)", strAndString)
				Else
					sSql += String.Format("{0}KD.KreditWarnung = 1", strAndString)
				End If
				FilterBez += String.Format("Einsätze mit Kunden mit Kreditwarnung = {0}{1}", cv.Text, vbLf)
			End If

			' Kreditlimite 1 --------------------------------------------------------------------------------------------------
			If .Lst_KDKreditlimite1.Items.Count > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = GetLstItems(.Lst_KDKreditlimite1).Replace("'", "''")
				sSql += String.Format("{0}KD.KreditLimite In (", strAndString)
				sSql += Replace(sZusatzBez, "#@", ",") & ")"
				FilterBez += "Einsätze mit Kunden mit 1.Kreditlimite in (" & sZusatzBez & ")" & vbLf
			End If

			' Kreditlimite 2 --------------------------------------------------------------------------------------------------
			If .Lst_KDKreditlimite2.Items.Count > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = GetLstItems(.Lst_KDKreditlimite2).Replace("'", "''")
				sSql += String.Format("{0}KD.KreditLimite_2 In (", strAndString)
				sSql += Replace(sZusatzBez, "#@", ",") & ")"
				FilterBez += "Einsätze mit Kunden mit 2.Kreditlimite in (" & sZusatzBez & ")" & vbLf
			End If

			' Kredit gültig ---------------------------------------------------------------------------------------------------
			If .deKredit_1.Text <> "" Then If Year(CDate(.deKredit_1.Text)) = 1 Then .deKredit_1.Text = String.Empty
			If .deKredit_2.Text <> "" Then If Year(CDate(.deKredit_2.Text)) = 1 Then .deKredit_2.Text = String.Empty
			If .deKredit_1.Text <> "" Or .deKredit_2.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				Dim gueltigVom As String = ""
				Dim gueltigBis As String = ""
				If IsDate(.deKredit_1.Text) Then gueltigVom = Date.Parse(.deKredit_1.Text).ToString("dd.MM.yyyy")
				If IsDate(.deKredit_2.Text) Then gueltigBis = Date.Parse(.deKredit_2.Text).ToString("dd.MM.yyyy")

				' Suche zwischen zwei Datum
				If gueltigVom.Length > 0 And gueltigBis.Length > 0 Then
					sSql += String.Format("{0}(KD.KreditlimiteBis >= '{1}' Or KD.KreditlimiteBis Is Null) And KD.KreditlimiteAb <= '{2}'",
																strAndString, gueltigVom, gueltigBis)
					FilterBez += String.Format("Kunden mit Kreditlimite gültig zwischen {0} und {1}{2}", gueltigVom, gueltigBis, vbLf)
					' Suche ab erstes Datum
				ElseIf gueltigVom.Length > 0 Then
					sSql += String.Format("{0}(KD.KreditlimiteAb >= '{1}' Or KD.KreditlimiteAb Is Null)", strAndString, gueltigVom)
					FilterBez += String.Format("Kunden mit Kreditlimite gültig ab Datum {0}{1}", gueltigVom, vbLf)
					' Suche bis zweites Datum
				ElseIf gueltigBis.Length > 0 Then
					'sSql += String.Format("{0}KD.KreditlimiteBis <= '{1}' And KD.KreditLimiteAb <= '{1}'", strAndString, gueltigBis)
					sSql += String.Format("{0}KD.KreditlimiteBis <= '{1}' ", strAndString, gueltigBis)
					FilterBez += String.Format("Kunden mit Kreditlimite gültig bis Datum {0}{1}", gueltigBis, vbLf)
				End If
			End If

			' Keine Kreditlimite eingetragen? ---------------------------------------------------------------------------------------------------
			If .chkKDKeinKreditlimite.CheckState = CheckState.Checked Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				Dim kreditWarn As String = .Cbo_KDKreditwarnung.Text ' ToItem.Value_1
				sSql += String.Format("{0}(KD.KreditLimite = 0 Or KD.KreditLimite Is Null)", strAndString)
				FilterBez += String.Format("Einsätze mit Kunden OHNE Kreditlimite{0}", vbLf)
			End If

			' Kreditlimite überschritten -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If .chkKDKreditlimiteUeberschritten.Checked Then

				' Alle Kunden die eine Kreditlimite haben und diese überschritten ist.
				Dim connection As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
				Dim sqlKreditlimite As String = "[List KDNR Kreditlimite ueberschritten]"
				Dim cmd As SqlCommand = New SqlCommand(sqlKreditlimite, connection)
				cmd.CommandType = CommandType.StoredProcedure
				cmd.Parameters.AddWithValue("@filiale", strUSFiliale)

				connection.Open()

				Dim reader As SqlDataReader = cmd.ExecuteReader()
				If reader.HasRows Then
					sSql += String.Format("{0} KD.KDNR In (", strAndString)
					While reader.Read
						sSql += String.Format("{0},", reader("Kdnr"))
					End While
					sSql = sSql.Remove(sSql.Length - 1, 1)
					sSql += ") "
					FilterBez = String.Format("Kreditlimite überschritten {0}", vbLf)

				End If

				connection.Close()
			End If


			' Rapporte drucken ------------------------------------------------------------------------------------------------
			If .Cbo_KDRapporteDrucken.SelectedIndex > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_KDRapporteDrucken.SelectedItem, ComboValue)
				If cv.ComboValue = "0" Then
					sSql += String.Format("{0}(KD.PrintNoRP = 0 Or KD.PrintNoRP Is Null)", strAndString)
				Else
					sSql += String.Format("{0}KD.PrintNoRP = 1", strAndString)
				End If
				FilterBez += String.Format("Einsätze mit Kunden mit Rapport nicht drucken = {0}{1}", cv.Text, vbLf)
			End If

			' =================================================================================================================
			' Mandantennummer -------------------------------------------------------------------------------------------------------
			If ClsDataDetail.MDData.MultiMD = 1 Then

				Dim mdnumber As String = ConvListObject2String(Me.mandantNumber, ", ")
				FilterBez += String.Format(m_Translate.GetSafeTranslationValue("Einsätze mit Mandantennummer: ({1}){0}"), vbLf, mdnumber)

				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sSql += String.Format("{0}ES.MDNr In ({1})", strAndString, mdnumber)
			End If

			' Filialen-Einschränkung: Disponent darf nur Kandidaten oder Kunden seiner Filiale sehen.
			' Der Mitarbeiter oder Kunde kann zu mehrere Filialen angehören.
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			sSql += String.Format("{0}('{1}' = '' Or MA.MAFiliale Like '%{1}%' Or KD.KDFiliale Like '%{1}%')", strAndString, strUSFiliale)
		End With
		ClsDataDetail.GetFilterBez = FilterBez

		m_Logger.LogInfo(String.Format("ESSearchWhereQuery: {0}", sSql))

		Return sSql
	End Function

	Function GetFilialKstData(ByVal strFiliale As String) As String
		Dim strKSTResult As String = ","
		Dim strFieldName As String = "KST"

		Dim strSqlQuery As String = "Select Benutzer.KST From Benutzer Left Join US_Filiale on Benutzer.USNr = US_Filiale.USNr Where "
		If strFiliale = "Leere Felder" Then
			strSqlQuery += "US_Filiale.Bezeichnung Is Null "
		Else
			strSqlQuery += "US_Filiale.Bezeichnung = '" & strFiliale & "' "
		End If

		strSqlQuery += "Group By Benutzer.KST Order By Benutzer.KST"

		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rPLZrec As SqlDataReader = cmd.ExecuteReader                    ' PLZ-Datenbank

			While rPLZrec.Read
				strKSTResult += rPLZrec(strFieldName).ToString & ","

			End While
			Console.WriteLine("strKSTResult: " & strKSTResult)
			If strKSTResult.Length > 1 Then
				strKSTResult = Mid(strKSTResult, 2, Len(strKSTResult) - 2)
				strKSTResult = Replace(strKSTResult, ",", "','")
			Else
				strKSTResult = String.Empty
			End If

		Catch e As Exception
			strKSTResult = String.Empty
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return strKSTResult
	End Function

	Function GetSortString(ByVal frmTest As frmESSearch) As String
		Dim strSort As String = " Order by "
		Dim strSortBez As String = String.Empty
		Dim strName As String()
		Dim strMyName As String = String.Empty

		'0 - Einsatznummer
		'1 - Kandidatennnummer
		'2 - Kundennummer
		'3 - Einsatzbeginn und Ende
		'4 - Kandidatennname
		'5 - Firmenname

		With frmTest
			strName = Regex.Split(.CboSort.Text.Trim, ",")
			strMyName = String.Empty
			If .CboSort.Text.Contains("-") Then
				For i As Integer = 0 To strName.Length - 1
					Select Case CInt(Val(Left(strName(i).ToString.Trim, 1))) ' Das erste Zeichen der Sortierung
						Case 0                  ' Nach Einsatznummer
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " ESNr "
							strSortBez += CStr(IIf(strSortBez.Length > 0, ",", "")) & m_Translate.GetSafeTranslationValue("Einsatznummer")
						Case 1                  ' Nach Kandidatennummer
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " MANr "
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & m_Translate.GetSafeTranslationValue("Kandidatennummer")
						Case 2                  ' Kundennummer
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " KDNr "
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & m_Translate.GetSafeTranslationValue("Kundennummer")

						Case 3                  ' Einsatzbeginn und Ende
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " ES_Ab, ES_Ende "
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & m_Translate.GetSafeTranslationValue("Einsatzbeginn und Ende")
						Case 4                  ' Kandidatenname
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " Nachname, Vorname "
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & m_Translate.GetSafeTranslationValue("Kandidatenname")
						Case 5          ' Firmenname
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " Firma1 "
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & m_Translate.GetSafeTranslationValue("Firmenname")
					End Select
				Next i
			Else
				strMyName = .CboSort.Text
				strSortBez = m_Translate.GetSafeTranslationValue("Benutzerdefiniert")
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


	Function CreatePVLTable() As Boolean
		Dim success As Boolean = True

		Dim pvlObj As PVLGAVUtility = New PVLGAVUtility(m_InitialData)
		Dim data = pvlObj.LoadPVLMetaData
		If ExistsPVLData(data) Then Return success

		For Each itm In data
			Dim sql As String
			sql = "IF NOT (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'tbl_PVLMetaData'))"
			sql &= " Begin"
			sql &= " CREATE TABLE [dbo].[tbl_PVLMetaData]("
			sql &= " [ID] [int] IDENTITY(1,1) NOT NULL,"
			sql &= " [GAVNumber] int Null,"
			sql &= " [Name_DE] [nvarchar](255) NULL,"
			sql &= " [Name_FR] [nvarchar](255) NULL,"
			sql &= " [Name_IT] [nvarchar](255) NULL,"
			sql &= " [CurrentGAVTableName] [nvarchar](255) NULL,"
			sql &= " [CreatedOn] DATETIME NULL,"
			sql &= " [CreatedFrom] [nvarchar](255) NULL,"
			sql &= " CONSTRAINT [PK_tbl_PVLMetaData_ID] PRIMARY KEY CLUSTERED "
			sql &= " ("
			sql &= " [ID] Asc"
			sql &= " )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]"
			sql &= " ) ON [PRIMARY]"

			sql &= "End;"

			sql &= " Delete tbl_PVLMetaData Where GAVNumber = @GAVNumber;"
			sql &= " Insert Into tbl_PVLMetaData (GAVNumber, Name_DE, Name_FR, Name_IT, CurrentGAVTableName, CreatedOn, CreatedFrom) Values ("
			sql &= " @GAVNumber, @Name_DE, @Name_FR, @Name_IT"
			sql &= ",@CurrentGAVTableName, GetDate(), @UserName"
			sql &= " )"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("GAVNumber", m_utility.ReplaceMissing(itm.GAVNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Name_DE", m_utility.ReplaceMissing(itm.NameDE, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Name_FR", m_utility.ReplaceMissing(itm.NameFR, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Name_IT", m_utility.ReplaceMissing(itm.NameIT, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CurrentGAVTableName", m_utility.ReplaceMissing(itm.CurruentDBName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("UserName", m_utility.ReplaceMissing(m_InitialData.UserData.UserFullName, DBNull.Value)))

			success = success AndAlso m_utility.ExecuteNonQuery(m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.Text)

		Next


		Return success

	End Function

	Private Function ExistsPVLData(ByVal data As List(Of PVLMetaData)) As Boolean
		Dim result As Boolean = True
		If data Is Nothing OrElse data.Count = 0 Then
			Return False
		End If

		For Each itm In data
			Dim dbName = itm.CurruentDBName

			Dim sql As String
			sql = "IF NOT (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'tbl_PVLMetaData'))"
			sql &= " Begin"
			sql &= " CREATE TABLE [dbo].[tbl_PVLMetaData]("
			sql &= " [ID] [int] IDENTITY(1,1) NOT NULL,"
			sql &= " [GAVNumber] int Null,"
			sql &= " [Name_DE] [nvarchar](255) NULL,"
			sql &= " [Name_FR] [nvarchar](255) NULL,"
			sql &= " [Name_IT] [nvarchar](255) NULL,"
			sql &= " [CurrentGAVTableName] [nvarchar](255) NULL,"
			sql &= " [CreatedOn] DATETIME NULL,"
			sql &= " [CreatedFrom] [nvarchar](255) NULL,"
			sql &= " CONSTRAINT [PK_tbl_PVLMetaData_ID] PRIMARY KEY CLUSTERED "
			sql &= " ("
			sql &= " [ID] Asc"
			sql &= " )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]"
			sql &= " ) ON [PRIMARY]"

			sql &= "End;"

			sql &= " Select Top 1 CurrentGAVTableName From tbl_PVLMetaData Where CurrentGAVTableName =  @CurrentGAVTableName;"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("CurrentGAVTableName", m_utility.ReplaceMissing(itm.CurruentDBName, DBNull.Value)))

			Dim reader = m_utility.OpenReader(m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.Text)
			If (Not reader Is Nothing AndAlso reader.Read()) Then
				Return True
			Else
				Return False
			End If
		Next

	End Function

	Private Function ParseToBoolean(ByVal stringvalue As String, ByVal value As Boolean?) As Boolean
		Dim result As Boolean
		If (Not Boolean.TryParse(stringvalue, result)) Then
			Return value
		End If
		Return result
	End Function

	Public Overloads Shared Function ConvListObject2String(ByVal lst As List(Of Integer), Optional ByVal Seperator As String = ", ") As String
		Dim str As New System.Text.StringBuilder
		For i As Integer = 0 To lst.Count - 1
			str.AppendFormat("{0}{1}", CInt(lst.Item(i)), If(i = lst.Count - 1, "", Seperator))
		Next
		Return str.ToString
	End Function

	Public Overloads Shared Function ConvListObject2String(ByVal lst As List(Of String), Optional ByVal Seperator As String = ", ") As String
		Dim str As New System.Text.StringBuilder
		For i As Integer = 0 To lst.Count - 1
			str.AppendFormat("'{0}'{1}", CStr(lst.Item(i)), If(i = lst.Count - 1, "", Seperator))
		Next
		Return str.ToString
	End Function

End Class

