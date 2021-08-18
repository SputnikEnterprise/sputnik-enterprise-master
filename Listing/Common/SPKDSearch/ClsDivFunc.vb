
Imports System.IO
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions

Imports SPProgUtility.CommonSettings

Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
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

	'// Firmenname
	Dim _strKDName As String
	Public Property GetKDName() As String
		Get
			Return _strKDName
		End Get
		Set(ByVal value As String)
			_strKDName = value
		End Set
	End Property

	'// KDZhd
	Dim _iKDZhdNr As Integer
	Public Property GetKDZhdNr() As Integer
		Get
			Return _iKDZhdNr
		End Get
		Set(ByVal value As Integer)
			_iKDZhdNr = value
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
	Public Property luColor As DevExpress.XtraEditors.GridLookUpEdit

	Public Property mandantNumber As New List(Of Integer)


	Sub GetJobNr4Print(ByVal frmSource As frmKDSearch, ByVal sListArt As Short)
		Dim strModul2print As String = String.Empty

		'Bruttoumsatzliste Rapporte Total
		With frmSource
			If sListArt = 0 Then strModul2print = "2.3" ' Druck für Kundenliste
			If sListArt = 1 Then strModul2print = "2.4" ' Druck für Kundenliste
			If sListArt = 2 Then strModul2print = "2.3.3" ' Kunden-Kontaktliste (02.08.2010)
			If sListArt = 3 Then strModul2print = "2.3.4" ' Kunden-Kreditlimiteliste (02.08.2010)
		End With

		ClsDataDetail.GetModulToPrint = strModul2print
	End Sub

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


#Region "Funktionen zur Suche nach Daten..."

	Function GetStartSQLString(ByVal frmTest As frmKDSearch) As String
		Dim sSql As String = String.Empty
		Dim sSqlLen As Integer = 0
		Dim sZusatzBez As String = String.Empty
		Dim i As Integer = 0
		'Dim m_common As New CommonSetting

		With frmTest
			' DIE TABELLE WIRD AUF DER DB GESPEICHERT
			ClsDataDetail.LLTablename = String.Format("_Kundenliste_{0}", ClsDataDetail.ProgSettingData.LogedUSNr)
			' ALTE TABELLE LÖSCHEN
			sSql = String.Format("BEGIN TRY DROP TABLE {0} END TRY BEGIN CATCH END CATCH ", ClsDataDetail.LLTablename)

			If .de_NoKontaktseit.Text <> String.Empty And (.de_ZKontakt_1.Text + .de_ZKontakt_2.Text +
										.txt_ZKontaktBez.Text +
										.Cbo_ZKontaktTyp.Text +
										.Cbo_ZKontaktDown.Text +
										.Cbo_ZKontaktFrom.Text <> "") Then
				Dim strMsg As String = "Sie versuchen Liste der nicht kontaktierte Kunden zu suchen mit ungeeigenten Suchverfahren. Ich werde die Suche optimieren."
				DevExpress.XtraEditors.XtraMessageBox.Show(m_xml.GetSafeTranslationValue(strMsg),
																									 m_xml.GetSafeTranslationValue("Suche nach Kontaktdaten"),
																									 MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
				.de_ZKontakt_1.Text = String.Empty
				.de_ZKontakt_1.Properties.NullValuePromptShowForEmptyValue = True

				.de_ZKontakt_2.Text = String.Empty
				.de_ZKontakt_2.Properties.NullValuePromptShowForEmptyValue = True

				.txt_ZKontaktBez.Text = String.Empty
				.Cbo_ZKontaktTyp.Text = String.Empty
				.Cbo_ZKontaktDown.Text = String.Empty
				.Cbo_ZKontaktFrom.Text = String.Empty

			End If

			If .de_NoKontaktseit.Text <> String.Empty Then
				sSql &= "BEGIN TRY DROP TABLE #KDMax END TRY BEGIN CATCH END CATCH "
				sSql &= "Select KD.KDNR Into #KDMax From Kunden KD "
				sSql &= "Where  Convert(datetime, (Select Max(Kontaktdate) As Datum From KD_KontaktTotal KDK "
				sSql &= "Where KDK.KDNr = KD.KDNR And KD.KDNr > 0 "

				If UCase(.cbo_KDArtHide.Text) <> String.Empty Then
					sZusatzBez = .cbo_KDArtHide.Text
					sZusatzBez = sZusatzBez.Replace(",", "','").Replace(" ", "")
					sSql += String.Format("And KDK.KontaktType1 Not In ('{0}') ", sZusatzBez)
				End If

				sSql &= "), 104) < 	"
				sSql &= String.Format("Convert(Datetime, '{0}', 104) ", Format(CDate(.de_NoKontaktseit.Text), "d"))
				sSql &= "order By KDNr "
			End If

			sSql += "Select "
			sSql += "Distinct(KD.KDNr), KD.FProperty, KD.Firma1, KD.Firma2 As KDFirma2, KD.Firma3 As KDFirma3, KD.PLZ As KDPLZ, "
			sSql += "KD.Ort As KDOrt, KD.Postfach As KDPostfach, KD.Land As KDLand, "
			sSql += "IsNull(KD.Telefon, '') As KDTelefon, IsNull(KD.Telefax, '') As KDTelefax, "
			sSql += "KD.Strasse As KDStrasse, KD.KDState1, KD.KDState2, "
			sSql += "KD.HowKontakt As KDKontakt, KD.eMail As KDeMail, "
			sSql += "KD.Currency As KDCurrency, convert(nvarchar(4000), KD.Bemerkung) As KDBemerkung, "
			sSql += "KD.Kreditlimite As KDKreditlimite, "
			sSql += "KD.KreditlimiteAb As KDKreditLimiteAb, "
			sSql += "KD.KreditlimiteBis As KDKreditLimiteBis, "

			sSql += "KD.Kreditlimite_2 As KDKreditlimite_2, "
			sSql += "KD.KL_RefNr As Kredit_RefNr, "
			sSql += "KD.KreditWarnung, "
			sSql += "KD.KDFiliale As KDAllFiliale, IsNull((Select Top 1 (US.Nachname + ', ' + US.Vorname) As MABerater From Benutzer US Where US.Kst = KD.KST), '') As KDBeraterFullname, "

			sSql += "KD.Faktura As KDFaktura, "
			sSql += "IsNull((Select Top 1 ZahlKond From KD_RE_Address Where KD_RE_Address.KDNr = KD.KDNR), '') As KDZahlKond, "
			sSql += "IsNull((Select Top 1 MahnCode From KD_RE_Address Where KD_RE_Address.KDNr = KD.KDNR), '') As KDMahnCode, "

			sSql += "KD.MwStNr As KDMwStNr, KD.MwSt As KDMwSt, KD.KST, "

			sSql += "IsNull(KD.KD_Telefax_Mailing, 0) As KD_Telefax_Mailing, IsNull(KD.KD_Mail_Mailing, 0) As KD_Mail_Mailing, IsNull(KD.KD_UmsMin, 0) As KD_UmsMin, "

			sSql += "IsNull(KD_Zustaendig.RecNr, 0) As ZHDRecNr, "
			sSql += "KD_Zustaendig.Anrede, KD_Zustaendig.Nachname, "
			sSql += "KD_Zustaendig.Vorname, KD_Zustaendig.AnredeForm, "
			sSql += "IsNull(KD_Zustaendig.Telefon, '') As ZHDTelefon, "
			sSql += "IsNull(KD_Zustaendig.Telefax, '') As ZHDTelefax, "
			sSql += "IsNull(KD_Zustaendig.Natel, '') As ZHDNatel, "
			sSql += "IsNull(KD_Zustaendig.eMail, '') As ZHDeMail, "

			sSql += "IsNull(KD_Zustaendig.ZHD_Telefax_Mailing, 0) As ZHD_Telefax_Mailing, IsNull(KD_Zustaendig.ZHD_SMS_Mailing, 0) As ZHD_SMS_Mailing, "
			sSql &= "IsNull(KD_Zustaendig.ZHD_Mail_Mailing, 0) As ZHD_Mail_Mailing, "

			sSql += "KD_Zustaendig.Postfach As ZHDPostfach, "
			sSql += "KD_Zustaendig.Strasse As ZHDStrasse, "
			sSql += "KD_Zustaendig.PLZ As ZHDPLZ, "
			sSql += "KD_Zustaendig.Ort As ZHDOrt, "
			sSql += "KD_Zustaendig.Abteilung As ZHDAbt, "
			sSql += "KD_Zustaendig.Position As ZHDPos, "
			sSql += "KD_Zustaendig.Interessen As ZHDInteressen, "
			sSql += "KD_Zustaendig.Bemerkung As ZHDBemerkung, "
			sSql += "KD_Zustaendig.Geb_Dat As ZHDGebdat, "
			sSql += "KD_Zustaendig.KDZHowKontakt, "
			sSql += "KD_Zustaendig.KDZState1, "
			sSql += "KD_Zustaendig.KDZState2, "
			sSql += "KD_Zustaendig.Berater As ZHDBerater, "
			sSql += "IsNull((Select Top 1 (US.Nachname + ', ' + US.Vorname) As MABerater From Benutzer US Where US.Kst = KD_Zustaendig.berater), '') As ZHDBeraterFullname, "

			If .de_ZKontakt_1.Text <> "" Then If Year(CDate(.de_ZKontakt_1.Text)) = 1 Then .de_ZKontakt_1.Text = String.Empty
			If .de_ZKontakt_2.Text <> "" Then If Year(CDate(.de_ZKontakt_2.Text)) = 1 Then .de_ZKontakt_2.Text = String.Empty

			If .de_ZKontakt_1.EditValue Is Nothing And .de_ZKontakt_2.EditValue Is Nothing AndAlso
										String.IsNullOrWhiteSpace(.txt_ZKontaktBez.Text & .Cbo_ZKontaktTyp.Text & .Cbo_ZKontaktDown.Text & .Cbo_ZKontaktFrom.Text) Then
				sSql += "convert(datetime, '01.01.1900') As ZHDKontaktDate, "
				' Kontaktliste
				sSql += "Convert(nvarchar(10), Convert(DateTime, '01.01.1900'),104) As ZHDKontaktDateString, "
				sSql += "Convert(nvarchar(5),Convert(DateTime, '01.01.1900 00:00:00',104),108) as ZHDKontaktTimeString, "
				sSql += "'' As ZHDKontaktRecNr, "
				sSql += "'' As ZHDKontaktType1, "
				sSql += "'' As ZHDKontaktDauer, "
				sSql += "'' As ZHDKontakte, '' As ZHDKontakteFrom, "

			Else
				'  sSql += "KDK.KDZNr, "
				sSql += "KDK.KontaktDate As ZHDKontaktDate, "
				' Kontaktliste
				sSql += "Convert(nvarchar(10), Convert(DateTime, KDK.KontaktDate),104) As ZHDKontaktDateString, "
				sSql += "Convert(nvarchar(5),Convert(DateTime, KDK.KontaktDate,104),108) as ZHDKontaktTimeString, "
				sSql += "KDK.RecNr As ZHDKontaktRecNr, "
				sSql += "KDK.KontaktType1 As ZHDKontaktType1, "
				sSql += "Convert(nvarchar(500), KDK.KontaktDauer) As ZHDKontaktDauer, "
				sSql += "Convert(nvarchar(500), KDK.Kontakte) As ZHDKontakte, KDK.CreatedFrom As ZHDKontakteFrom, "

			End If

			' Die Farbe der Hauptmaske für die Kandidaten übernehmen
			' Die SQL-String ist auf der Datenbank
			'Dim conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring) 'ClsDataDetail.GetSelectedConnstring)
			'Dim cmdFarbe As SqlCommand = New SqlCommand("SELECT CaseString FROM TBL_COLORS WHERE Modulname='KD0' ", conn)
			'conn.Open()
			'sSql += cmdFarbe.ExecuteScalar().ToString
			'conn.Close()

			sSql += "0 As '0' "

			' TABELLE AUF DER DATENBANK SPEICHERN
			sSql += String.Format("INTO {0} ", ClsDataDetail.LLTablename)

			sSql += "From Kunden KD "
			sSql += "Left Join KD_Zustaendig On KD.KDNr = KD_Zustaendig.KDNr "
			'sSql += "Left Join KD_RE_Address KDRE On KD.KDNr = KDRE.KDNr "


			' KD_KontaktTotal von Zuständige Personen
			If .de_ZKontakt_1.Text + .de_ZKontakt_2.Text +
							.txt_ZKontaktBez.Text +
							.Cbo_ZKontaktTyp.Text +
							.Cbo_ZKontaktDown.Text +
							.Cbo_ZKontaktFrom.Text + .cbo_KDArtHide.Text <> "" Then
				sSql += "Left Join KD_KontaktTotal KDK On " 'KD_Zustaendig.RecNr = KDK.KDZNr And "
				sSql += "KD.KDNr = KDK.KDNr "

			End If


			'' wegen Exists und Not Exists
			'If .Lst_KDBerufe.Items.Count > 0 Then
			'  sSql += "Left Join KD_Berufe On KD.KDNr = KD_Berufe.KDNr "

			'End If
			'If .Lst_KDBranche.Items.Count > 0 Then
			'  sSql += "Left Join KD_Branche On KD.KDNr = KD_Branche.KDNr "

			'End If

			'If .Lst_KDStichwort.Items.Count > 0 Then
			'  sSql += "Left Join KD_Stichwort On KD.KDNr = KD_Stichwort.KDNr "

			'End If
			'If .Lst_KDFiliale.Items.Count > 0 Then
			'  sSql += "Left Join KD_Filiale On KD.KDNr = KD_Filiale.KDNr "

			'End If
			'If .Lst_KDAnstellung.Items.Count > 0 Then
			'  sSql += "Left Join KD_Anstellung On KD.KDNr = KD_Anstellung.KDNr "

			'End If
			'If .Lst_KDGAV.Items.Count > 0 Then
			'  sSql += "Left Join KD_GAVGruppe On KD.KDNr = KD_GAVGruppe.KDNr "

			'End If

			'' Zuständige Personen
			'' Kommunikation
			'If .Lst_ZKom.Items.Count > 0 Then
			'  sSql += "Left Join KD_ZKomm On KD_Zustaendig.RecNr = KD_ZKomm.KDZNr "
			'  sSql += "And KD.KDNr = KD_ZKomm.KDNr "

			'End If

			'' Kommunikationart von Zuständige Personen
			'If .Lst_ZVersand.Items.Count > 0 Then
			'  sSql += "Left Join KD_ZKontaktArt On KD_Zustaendig.RecNr = KD_ZKontaktArt.KDZNr "
			'  sSql += "And KD.KDNr = KD_ZKontaktArt.KDNr "

			'End If

			' -----------------------------------------------------------------------------------------------
			'' Berufe von Zuständige Personen
			'   If .Lst_ZBerufe.Items.Count > 0 Then
			'     sSql += "Left Join KD_ZBerufe On KD_Zustaendig.RecNr = KD_ZBerufe.KDZNr "
			'     sSql += "And KD.KDNr = KD_ZBerufe.KDNr "

			'   End If
			'   ' Branchen von Zuständige Personen
			'   If .Lst_ZBranche.Items.Count > 0 Then
			'     sSql += "Left Join KD_ZBranche On KD_Zustaendig.RecNr = KD_ZBranche.KDZNr "
			'     sSql += "And KD.KDNr = KD_ZBranche.KDNr "

			'   End If
			'   ' 1. Reserve von Zuständige Personen
			'   If .Lst_Z1Res.Items.Count > 0 Then
			'     sSql += "Left Join KD_ZRes1 On KD_Zustaendig.RecNr = KD_ZRes1.KDZNr "
			'     sSql += "And KD.KDNr = KD_ZRes1.KDNr "

			'   End If
			'   ' 2. Reserve von Zuständige Personen
			'   If .Lst_Z2Res.Items.Count > 0 Then
			'     sSql += "Left Join KD_ZRes2 On KD_Zustaendig.RecNr = KD_ZRes2.KDZNr "
			'     sSql += "And KD.KDNr = KD_ZRes2.KDNr "

			'   End If
			'   ' 3. Reserve von Zuständige Personen
			'   If .Lst_Z3Res.Items.Count > 0 Then
			'     sSql += "Left Join KD_ZRes3 On KD_Zustaendig.RecNr = KD_ZRes3.KDZNr "
			'     sSql += "And KD.KDNr = KD_ZRes3.KDNr "

			'   End If
			'   ' 4. Reserve von Zuständige Personen
			'   If .Lst_Z4Res.Items.Count > 0 Then
			'     sSql += "Left Join KD_ZRes4 On KD_Zustaendig.RecNr = KD_ZRes4.KDZNr "
			'     sSql += "And KD.KDNr = KD_ZRes4.KDNr "

			'   End If

			sSqlLen = Len(sSql)

		End With

		Return sSql
	End Function

	Function GetQuerySQLString(ByVal sSQLQuery As String, ByVal frmTest As frmKDSearch) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim sSql As String = String.Empty
		Dim sOldQuery As String = sSQLQuery
		Dim m_common As New CommonSetting

		Dim FilterBez As String = String.Empty
		Dim sSqlLen As Integer = 0
		Dim sZusatzBez As String = String.Empty
		Dim strAndString As String = String.Empty

		Dim strUSFiliale As String = m_common.GetLogedUserFiliale
		Dim iSQLLen As Integer = Len(sSQLQuery)
		Dim strFieldName As String = String.Empty
		Dim strName As String()
		Dim strMyName As String = String.Empty
		Dim strMyDateFormat As String = "d"
		Dim cv As ComboBoxItem
		Dim ExcludeValue As Boolean = False

		With frmTest
			If .txtKDNr_1.Text = String.Empty And .txtKDNr_2.Text = String.Empty Then
			ElseIf .txtKDNr_1.Text.Contains("*") Or .txtKDNr_1.Text.Contains("%") Then
				FilterBez = "Kundennummer wie (" & .txtKDNr_1.Text & ") " & vbLf
				sSql += " KD.KDNr Like " & Replace(.txtKDNr_1.Text, "*", "%")

			ElseIf InStr(.txtKDNr_1.Text, ",") > 0 Then
				FilterBez = "Kundennummer wie (" & .txtKDNr_1.Text & ") " & vbLf
				sSql += " KD.KDNr In (" & .txtKDNr_1.Text & ")"

			ElseIf .txtKDNr_1.Text = .txtKDNr_2.Text Then
				FilterBez = "Kundennummer = " & .txtKDNr_1.Text & " " & vbLf
				sSql += " KD.KDNr = " & CInt(.txtKDNr_1.Text)

			ElseIf .txtKDNr_1.Text <> "" And .txtKDNr_2.Text = "" Then
				FilterBez = "Kundennummer ab " & .txtKDNr_1.Text & " " & vbLf
				sSql += " KD.KDNr >= " & CInt(.txtKDNr_1.Text)

			ElseIf .txtKDNr_1.Text = "" And .txtKDNr_2.Text <> "" Then
				FilterBez = "Kundennummer bis " & .txtKDNr_2.Text & " " & vbLf
				sSql += " KD.KDNr <= " & CInt(.txtKDNr_2.Text)

			Else
				FilterBez = "Kundennummer zwischen " & .txtKDNr_1.Text & " und " &
												.txtKDNr_2.Text & " " & vbLf
				sSql += " KD.KDNr Between " & CInt(.txtKDNr_1.Text) &
												" And " & CInt(.txtKDNr_2.Text)
			End If

			' Firma1 -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If .txtKDName_1.Text = "" And .txtKDName_2.Text = "" Then
			ElseIf .txtKDName_1.Text.Contains("*") Or .txtKDName_1.Text.Contains("%") Then
				FilterBez += "Kundenname wie (" & .txtKDName_1.Text & ") " & vbLf
				sSql += " KD.Firma1 Like '" & Replace(.txtKDName_1.Text, "*", "%") & "'"

			ElseIf InStr(.txtKDName_1.Text, ",") > 0 Then
				FilterBez += "Kundenname wie (" & .txtKDName_1.Text & ") " & vbLf
				sZusatzBez = .txtKDName_1.Text
				sZusatzBez = Replace(sZusatzBez, ", ", ",")
				sSql += " KD.Firma1 In ('" & Replace(sZusatzBez, ",", "','") & "')"

			ElseIf UCase(.txtKDName_1.Text) = UCase(.txtKDName_2.Text) Then
				FilterBez += "Kundenname = " & .txtKDName_2.Text & vbLf
				sSql += strAndString & "KD.Firma1 like '" &
									.txtKDName_1.Text & "'"

			ElseIf .txtKDName_1.Text <> "" And .txtKDName_2.Text = "" Then
				FilterBez += "Kundenname ab " & .txtKDName_1.Text & vbLf
				sSql += strAndString & "KD.Firma1 >= '" &
									.txtKDName_1.Text & "'"

			ElseIf .txtKDName_1.Text = "" And .txtKDName_2.Text <> "" Then
				FilterBez += "Kundenname bis " & .txtKDName_2.Text & vbLf
				sSql += strAndString & "KD.Firma1 <= '" &
									.txtKDName_2.Text & "'"

			Else
				FilterBez += "Kundenname zwischen " & .txtKDName_1.Text & " und " &
										.txtKDName_2.Text & vbLf
				sSql += strAndString & "KD.Firma1 Between '" &
							.txtKDName_1.Text & "' And '" & .txtKDName_2.Text & "'"
			End If

			' PLZ -------------------------------------------------------------------------------------------------------
			sZusatzBez = .txtKDPLZ_1.Text.Replace("'", "''").Replace("*", "%").Trim
			' Suche nach einer oder mehrere PLZ
			If sZusatzBez <> String.Empty Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				If sZusatzBez.IndexOf(CChar("-")) > 0 Then
					Dim plzVon As String = sZusatzBez.Split(CChar("-"))(0)
					Dim plzBis As String = sZusatzBez.Split(CChar("-"))(1)
					sSql += String.Format("{0}KD.PLZ Between '{1}' And '{2}'", strAndString, plzVon, plzBis)
					FilterBez += String.Format("Kunden mit PLZ zwischen {0} und {1}{2}", plzVon, plzBis, vbLf)
				Else
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Kunden mit PLZ ({0}){1}"), sZusatzBez, vbLf)
					sSql += String.Format("{0}(", strAndString)
					Dim strOrString As String = ""
					For Each plz As String In sZusatzBez.Split(CChar(","))
						sSql += String.Format("{0}KD.PLZ like '{1}'", strOrString, plz.Trim)
						strOrString = " Or "
					Next
					sSql += ")"
				End If
			End If

			' Ort -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.txtKDOrt_1.Text) <> String.Empty Then
				sZusatzBez = .txtKDOrt_1.Text.Trim
				sZusatzBez = Replace(sZusatzBez, ", ", ",")
				FilterBez += "Ort wie (" & sZusatzBez & ") " & vbLf
				If InStr(.txtKDPLZ_1.Text, ",") > 0 Then
					sZusatzBez = Replace(sZusatzBez, ",", "','")

				End If
				If sZusatzBez.Contains("*") Or sZusatzBez.Contains("%") Then
					sSql += strAndString & " KD.Ort Like '" & Replace(sZusatzBez, "*", "%") & "'"
				Else
					sSql += strAndString & "KD.Ort In ('" & sZusatzBez & "')"
				End If

			End If

			' Land -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.txtKDLand_1.Text) <> String.Empty Then
				sZusatzBez = .txtKDLand_1.Text.Trim
				sZusatzBez = Replace(sZusatzBez, ", ", ",")
				FilterBez += "Land wie (" & sZusatzBez & ") " & vbLf

				If InStr(.txtKDLand_1.Text, ",") > 0 Then
					sZusatzBez = Replace(sZusatzBez, ",", "','")
				End If
				If sZusatzBez.Contains("*") Or sZusatzBez.Contains("%") Then
					sSql += strAndString & " KD.Land Like '" & Replace(sZusatzBez, "*", "%") & "'"
				Else
					sSql += strAndString & "KD.Land In ('" & sZusatzBez & "')"
				End If

			End If

			' Kanton -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_KDKanton.Text) <> String.Empty Then
				sZusatzBez = .Cbo_KDKanton.Text.Trim

				strName = Regex.Split(sZusatzBez.Trim, ",")
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
				Next
				If strName.Length > 0 Then sZusatzBez = strMyName
				FilterBez += "Kanton wie (" & sZusatzBez & ") " & vbLf

				If InStr(sZusatzBez.Trim, ",") > 0 Then
					sZusatzBez = Replace(sZusatzBez.Trim, ",", "','")
				End If
				sSql += strAndString & "KD.PLZ In ('" & GetKantonPLZ(sZusatzBez.Trim) & "')"

			End If

			' Berater -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_KDBerater.Text) <> String.Empty Then
				cv = DirectCast(.Cbo_KDBerater.SelectedItem, ComboBoxItem)
				sZusatzBez = cv.Value ' .Cbo_KDBerater.Text.Trim

				strName = Regex.Split(cv.Value.Trim, ",")
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
				Next
				If strName.Length > 0 Then sZusatzBez = strMyName
				FilterBez += "Berater wie (" & sZusatzBez & ") " & vbLf

				If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
					sSql += strAndString & " (KD.KST = '' Or KD.KST Is Null) "
				Else
					If InStr(sZusatzBez, ",") > 0 Then
						sZusatzBez = Replace(sZusatzBez, ",", "','")

					End If
					sSql += strAndString & "KD.KST In('" & sZusatzBez & "')"
				End If

			End If

			' 1. Eigenschaft -----------------------------------------------------------------------------------------------------

			Try
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				If CStr(Me.luColor.EditValue) <> String.Empty Then
					sZusatzBez = CStr(CInt(Me.luColor.EditValue)).Trim
					FilterBez += .lblFProperty.Text & " wie (" & Me.luColor.Text & ") " & vbLf

					'sZusatzBez = Mid(CStr(sZusatzBez).ToString, 1, InStr(CStr(sZusatzBez).ToString, vbTab) - 1)
					sSql += strAndString & "KD.FProperty In (" & sZusatzBez.Replace("16777215", "0") & ")"

				End If

				'If UCase(.Cbo_KDFProperty.Text) <> String.Empty Then
				'  sZusatzBez = .Cbo_KDFProperty.Text.Trim
				'  FilterBez += .LblChange_1.Text & " wie (" & sZusatzBez & ") " & vbLf

				'  sZusatzBez = Mid(CStr(sZusatzBez).ToString, 1, InStr(CStr(sZusatzBez).ToString, vbTab) - 1)
				'  sSql += strAndString & "KD.FProperty In (" & sZusatzBez.Replace("16777215", "0") & ")"

				'End If
			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.FProperty: {1} s.", strMethodeName, ex.Message))

			End Try

			' Kontakt -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_KDKontakt.Text) <> String.Empty Then
				ExcludeValue = .ExcludeSelectedCheckedComboboxValue(.Cbo_KDKontakt)
				sZusatzBez = .Cbo_KDKontakt.Text.Trim
				'FilterBez += .lblKontakt.Text & " wie (" & sZusatzBez & ") " & vbLf

				strName = Regex.Split(sZusatzBez.Trim, .Cbo_KDKontakt.Properties.SeparatorChar)
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, .Cbo_KDKontakt.Properties.SeparatorChar, "")) & strName(i).ToString.Trim
				Next
				If strName.Length > 0 Then sZusatzBez = strMyName

				If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
					sSql += strAndString & " (KD.HowKontakt = '' Or KD.HowKontakt Is Null) "
				Else
					'If InStr(sZusatzBez, ",") > 0 Then sZusatzBez = Replace(sZusatzBez, ",", "','")

					sSql += String.Format("{0}IsNull(KD.HowKontakt, '') {1} ('{2}')", strAndString, If(ExcludeValue, "Not In", "In"), sZusatzBez.Trim.Replace(.Cbo_KDKontakt.Properties.SeparatorChar, "','"))
					'sSql += strAndString & "KD.HowKontakt In ('" & sZusatzBez & "')"

				End If
				FilterBez += String.Format("{1} {2} ({3}){0}", vbNewLine, .lblKontakt.Text, If(ExcludeValue, "NICHT in", "in"), sZusatzBez)

			End If

			' 1. Status -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_KD1Stat.Text) <> String.Empty Then
				ExcludeValue = .ExcludeSelectedComboboxValue(.Cbo_KD1Stat)
				sZusatzBez = .Cbo_KD1Stat.Text.Trim

				strName = Regex.Split(sZusatzBez.Trim, ",")
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
				Next
				If strName.Length > 0 Then sZusatzBez = strMyName
				'FilterBez += .lbl1Status.Text & " wie (" & sZusatzBez & ") " & vbLf

				If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
					sSql += strAndString & " (KD.KDState1 = '' Or KD.KDState1 Is Null) "
				Else
					'If InStr(sZusatzBez, ",") > 0 Then
					'  sZusatzBez = Replace(sZusatzBez, ",", "','")
					'End If

					sSql += String.Format("{0}IsNull(KD.KDState1, '') {1} ('{2}')", strAndString, If(ExcludeValue, "Not In", "In"), sZusatzBez.Trim.Replace(",", "','"))
					'sSql += strAndString & "KD.KDState1 In ('" & sZusatzBez & "')"
				End If
				FilterBez += String.Format("{1} {2} ({3}){0}", vbNewLine, .lbl2Status.Text, If(ExcludeValue, "NICHT in", "in"), sZusatzBez)

			End If

			' 2. Status -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_KD2Stat.Text) <> String.Empty Then
				ExcludeValue = .ExcludeSelectedComboboxValue(.Cbo_KD2Stat)
				sZusatzBez = .Cbo_KD2Stat.Text.Trim

				strName = Regex.Split(sZusatzBez.Trim, ",")
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
				Next
				If strName.Length > 0 Then sZusatzBez = strMyName
				'FilterBez += .lbl2Status.Text & " wie (" & sZusatzBez & ") " & vbLf

				If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
					sSql += strAndString & " (KD.KDState2 = '' Or KD.KDState2 Is Null) "
				Else
					'If InStr(sZusatzBez, ",") > 0 Then
					'  sZusatzBez = Replace(sZusatzBez, ",", "','")
					'End If

					sSql += String.Format("{0}IsNull(KD.KDState2, '') {1} ('{2}')", strAndString, If(ExcludeValue, "Not In", "In"), sZusatzBez.Trim.Replace(",", "','"))
					'sSql += strAndString & "KD.KDState2 In ('" & sZusatzBez & "')"
				End If
				FilterBez += String.Format("{1} {2} ({3}){0}", vbNewLine, .lbl2Status.Text, If(ExcludeValue, "NICHT in", "in"), sZusatzBez)

			End If

			' Einsatz gesperrt ---------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_ESStop.Text) <> String.Empty Then
				sZusatzBez = .Cbo_ESStop.Text
				If InStr(1, UCase(sZusatzBez), UCase("nicht")) > 0 Then
					FilterBez += "Einsatz nicht gesperrt " & vbLf
					sZusatzBez = .Cbo_ESStop.Text.Trim
					sSql += strAndString & " (KD.NoES = 0 Or KD.NoES Is Null) "

				Else
					FilterBez += "Einsatz gesperrt " & vbLf
					sSql += strAndString & "KD.NoES = 1 "
				End If

			End If

			' Daten fürs WWW -----------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_WWW.Text) <> String.Empty Then
				sZusatzBez = .Cbo_WWW.Text
				If InStr(1, UCase(sZusatzBez), UCase("nicht")) > 0 Then
					FilterBez += "Einsatz nicht exportieren " & vbLf
					sSql += strAndString & " (KD.ExportIE = 0 Or KD.ExportIE Is Null) "

				Else
					FilterBez += "Daten exportieren " & vbLf
					sSql += strAndString & "KD.ExportIE = 1 "
				End If

			End If

			' 1. Res -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_KD1Res.Text) <> String.Empty Then
				ExcludeValue = .ExcludeSelectedCheckedComboboxValue(.Cbo_KD1Res)
				sZusatzBez = .Cbo_KD1Res.Text.Trim

				strName = Regex.Split(sZusatzBez.Trim, .Cbo_KD1Res.Properties.SeparatorChar)
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, .Cbo_KD1Res.Properties.SeparatorChar, "")) & strName(i).ToString.Trim
				Next
				If strName.Length > 0 Then sZusatzBez = strMyName
				'FilterBez += .lbl1Res.Text & " wie (" & sZusatzBez & ") " & vbLf

				If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
					sSql += strAndString & "(KD.KDRes1 = '' Or KD.KDRes1 Is Null) "

				Else
					sSql += String.Format("{0}IsNull(KD.KDRes1, '') {1} ('{2}')", strAndString, If(ExcludeValue, "Not In", "In"), sZusatzBez.Trim.Replace(.Cbo_KD1Res.Properties.SeparatorChar, "','"))

				End If
				FilterBez += String.Format("{1} {2} ({3}){0}", vbNewLine, .lbl1Res.Text, If(ExcludeValue, "NICHT in", "in"), sZusatzBez)

			End If

			' 2. Res -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_KD2Res.Text) <> String.Empty Then
				ExcludeValue = .ExcludeSelectedCheckedComboboxValue(.Cbo_KD2Res)
				sZusatzBez = .Cbo_KD2Res.Text.Trim

				strName = Regex.Split(sZusatzBez.Trim, .Cbo_KD2Res.Properties.SeparatorChar)
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, .Cbo_KD2Res.Properties.SeparatorChar, "")) & strName(i).ToString.Trim
				Next
				If strName.Length > 0 Then sZusatzBez = strMyName
				'FilterBez += .lbl2Res.Text & " wie (" & sZusatzBez & ") " & vbLf

				If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
					sSql += strAndString & "(KD.KDRes2 = '' Or KD.KDRes2 Is Null) "

				Else
					sSql += String.Format("{0}IsNull(KD.KDRes2, '') {1} ('{2}')", strAndString, If(ExcludeValue, "Not In", "In"), sZusatzBez.Trim.Replace(.Cbo_KD2Res.Properties.SeparatorChar, "','"))

				End If
				FilterBez += String.Format("{1} {2} ({3}){0}", vbNewLine, .lbl2Res.Text, If(ExcludeValue, "NICHT in", "in"), sZusatzBez)

			End If

			' 3. Res -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_KD3Res.Text) <> String.Empty Then
				ExcludeValue = .ExcludeSelectedCheckedComboboxValue(.Cbo_KD3Res)
				sZusatzBez = .Cbo_KD3Res.Text.Trim

				strName = Regex.Split(sZusatzBez.Trim, .Cbo_KD3Res.Properties.SeparatorChar)
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, .Cbo_KD3Res.Properties.SeparatorChar, "")) & strName(i).ToString.Trim
				Next
				If strName.Length > 0 Then sZusatzBez = strMyName
				'FilterBez += .lbl3Res.Text & " wie (" & sZusatzBez & ") " & vbLf

				If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
					sSql += strAndString & "(KD.KDRes3 = '' Or KD.KDRes3 Is Null) "

				Else
					sSql += String.Format("{0}IsNull(KD.KDRes3, '') {1} ('{2}')", strAndString, If(ExcludeValue, "Not In", "In"), sZusatzBez.Trim.Replace(.Cbo_KD3Res.Properties.SeparatorChar, "','"))

				End If
				FilterBez += String.Format("{1} {2} ({3}){0}", vbNewLine, .lbl3Res.Text, If(ExcludeValue, "NICHT in", "in"), sZusatzBez)

			End If

			' 4. Res -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_KD4Res.Text) <> String.Empty Then
				ExcludeValue = .ExcludeSelectedCheckedComboboxValue(.Cbo_KD4Res)
				sZusatzBez = .Cbo_KD4Res.Text.Trim

				strName = Regex.Split(sZusatzBez.Trim, .Cbo_KD4Res.Properties.SeparatorChar)
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, .Cbo_KD4Res.Properties.SeparatorChar, "")) & strName(i).ToString.Trim
				Next
				If strName.Length > 0 Then sZusatzBez = strMyName
				'FilterBez += .lbl4Res.Text & " wie (" & sZusatzBez & ") " & vbLf

				If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
					sSql += strAndString & "(KD.KDRes4 = '' Or KD.KDRes4 Is Null) "

				Else
					sSql += String.Format("{0}IsNull(KD.KDRes4, '') {1} ('{2}')", strAndString, If(ExcludeValue, "Not In", "In"), sZusatzBez.Trim.Replace(.Cbo_KD4Res.Properties.SeparatorChar, "','"))

				End If
				FilterBez += String.Format("{1} {2} ({3}){0}", vbNewLine, .lbl4Res.Text, If(ExcludeValue, "NICHT in", "in"), sZusatzBez)

			End If

			' Zahlungskondition --------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			ExcludeValue = False
			If UCase(.Cbo_KDZKond.Text) <> String.Empty Then
				sZusatzBez = .Cbo_KDZKond.Text.Trim

				strName = Regex.Split(sZusatzBez.Trim, ",")
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
				Next
				If strName.Length > 0 Then sZusatzBez = strMyName

				'If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
				'  sSql += strAndString & "(KDRE.ZahlKond = '' Or KDRE.ZahlKond Is Null) "

				'Else
				If InStr(sZusatzBez, ",") > 0 Then
					sZusatzBez = Replace(sZusatzBez, ",", "','")
				End If

				sSql &= String.Format("{0}{1} EXISTS(SELECT TOP 1 KDNr FROM KD_RE_Address WHERE ZahlKond In ({2}) AND KDNr = KD.KDNR)", strAndString, If(ExcludeValue, "NOT", ""), sZusatzBez)
				'sSql += strAndString & "KDRE.ZahlKond In ('" & sZusatzBez & "')"

				'End If

				FilterBez += "Zahlungskondition wie (" & sZusatzBez & ") " & vbLf
			End If

			' Betriebsgrösse -----------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_KDBCount.Text) <> String.Empty Then
				sZusatzBez = .Cbo_KDBCount.Text.Trim

				strName = Regex.Split(sZusatzBez.Trim, ",")
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
				Next
				If strName.Length > 0 Then sZusatzBez = strMyName
				FilterBez += "Mitarbeiteranzahl wie (" & sZusatzBez & ") " & vbLf

				'If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
				'	sSql += strAndString & "(KD.MAAnzahl = '' Or KD.MAAnzahl Is Null) "

				'Else
				If InStr(sZusatzBez, ",") > 0 Then
					sZusatzBez = Replace(sZusatzBez, ",", "','")
				End If
				sSql += strAndString & "IsNull(KD.MAAnzahl, '') In ('" & sZusatzBez & "')"

				'End If
			End If

			' Transportmöglichkeit -----------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_Transport.Text) <> String.Empty Then
				sZusatzBez = .Cbo_Transport.Text.Trim
				If InStr(1, UCase(sZusatzBez), UCase("nicht")) > 0 Then
					FilterBez += "Transportmöglichkeit nicht vorhanden " & vbLf
					sSql += strAndString & " (KD.Transport = 0 Or KD.Transport Is Null) "

				Else
					FilterBez += "Transportmöglichkeit vorhanden " & vbLf
					sSql += strAndString & "KD.Transport = 1 "
				End If

			End If

			' Kantine -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_Kantine.Text) <> String.Empty Then
				sZusatzBez = .Cbo_Kantine.Text.Trim
				If InStr(1, UCase(sZusatzBez), UCase("nicht")) > 0 Then
					FilterBez += "Kantine nicht vorhanden " & vbLf
					sSql += strAndString & " (KD.Kantine = 0 Or KD.Kantine Is Null) "

				Else
					FilterBez += "Kantine vorhanden " & vbLf
					sSql += strAndString & "KD.Kantine = 1 "
				End If

			End If


			' 2. Seite ===========================================================================================================
			' ====================================================================================================================

			' Währung -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_KDCurrency.Text) <> String.Empty Then
				ExcludeValue = .ExcludeSelectedComboboxValue(.Cbo_KDCurrency)
				sZusatzBez = .Cbo_KDCurrency.Text.Trim

				strName = Regex.Split(sZusatzBez.Trim, ",")
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
				Next
				If strName.Length > 0 Then sZusatzBez = strMyName
				FilterBez += "Währung wie (" & sZusatzBez & ") " & vbLf

				'If InStr(sZusatzBez, ",") > 0 Then
				'	sZusatzBez = Replace(sZusatzBez, ",", "','")
				'End If

				sSql += String.Format("{0}KD.Currency {1} ('{2}')", strAndString, If(ExcludeValue, "Not In", "In"), sZusatzBez.Trim.Replace(",", "','"))
				'sSql += strAndString & "KD.Currency In ('" & sZusatzBez & "')"
				FilterBez += String.Format("{1} {2} ({3}){0}", vbNewLine, "Währung", If(ExcludeValue, "NICHT in", "in"), sZusatzBez)

			End If

			' Mahncode -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_KDMahn.Text) <> String.Empty Then
				ExcludeValue = .ExcludeSelectedComboboxValue(.Cbo_KDMahn)
				sZusatzBez = .Cbo_KDMahn.Text.Trim

				strName = Regex.Split(sZusatzBez.Trim, ",")
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
				Next
				If strName.Length > 0 Then sZusatzBez = strMyName
				'FilterBez += "Mahncode wie (" & sZusatzBez & ") " & vbLf

				'If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
				'	sSql += strAndString & " (KDRE.Mahncode = '' Or KDRE.Mahncode Is Null) "

				'Else
				If InStr(sZusatzBez, ",") > 0 Then
					sZusatzBez = Replace(sZusatzBez, ",", "','")
				End If
				sSql &= String.Format("{0}{1} EXISTS(SELECT TOP 1 KDNr FROM KD_RE_Address WHERE Mahncode In ('{2}') AND KDNr = KD.KDNR)", strAndString, If(ExcludeValue, "NOT", ""), sZusatzBez)
				'sSql += strAndString & "KDRE.Mahncode In ('" & sZusatzBez & "')"

				'End If
				FilterBez += String.Format("{1} {2} ({3}){0}", vbNewLine, "Mahncode", If(ExcludeValue, "NICHT in", "in"), sZusatzBez)

			End If

			' Fakturacode -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_KDFaktura.Text) <> String.Empty Then
				ExcludeValue = .ExcludeSelectedComboboxValue(.Cbo_KDFaktura)
				sZusatzBez = .Cbo_KDFaktura.Text.Trim

				strName = Regex.Split(sZusatzBez.Trim, ",")
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
				Next
				If strName.Length > 0 Then sZusatzBez = strMyName
				'FilterBez += "Fakturacode wie (" & sZusatzBez & ") " & vbLf

				If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
					sSql += strAndString & " (KD.Faktura = '' Or KD.Faktura Is Null) "

				Else
					'If InStr(sZusatzBez, ",") > 0 Then
					'	sZusatzBez = Replace(sZusatzBez, ",", "','")
					'End If
					sSql += String.Format("{0}KD.Faktura {1} ('{2}')", strAndString, If(ExcludeValue, "Not In", "In"), sZusatzBez.Trim.Replace(",", "','"))
					'sSql += strAndString & "KD.Faktura In ('" & sZusatzBez & "')"

				End If
				FilterBez += String.Format("{1} {2} ({3}){0}", vbNewLine, "Fakturacode", If(ExcludeValue, "NICHT in", "in"), sZusatzBez)

			End If

			' MwSt.-pflicht -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_MwSt.Text) <> String.Empty Then
				sZusatzBez = .Cbo_MwSt.Text.Trim
				If InStr(UCase(sZusatzBez), UCase("nicht")) > 0 Then
					FilterBez += "MwSt. nicht pflichtig" & vbLf
					sSql += strAndString & " (KD.MwSt = 0 Or KD.MwSt Is Null) "

				Else
					FilterBez += "MwSt. pflichtig " & vbLf
					sSql += strAndString & "KD.MwSt = 1 "
				End If

			End If

			' Rapporte drucken ---------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_RPPrint.Text) <> String.Empty Then
				sZusatzBez = .Cbo_RPPrint.Text.Trim
				If InStr(UCase(sZusatzBez), UCase("nicht")) > 0 Then
					FilterBez += "Rapporte nicht drucken" & vbLf
					sSql += strAndString & " (KD.PrintNoRP = 0 Or KD.PrintNoRP Is Null) "

				Else
					FilterBez += "Rapporte drucken" & vbLf
					sSql += strAndString & "KD.PrintNoRP = 1 "

				End If

			End If

			' Mit OP senden -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_KDWithOP.Text) <> String.Empty Then
				sZusatzBez = .Cbo_KDWithOP.Text.Trim

				strName = Regex.Split(sZusatzBez.Trim, ",")
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
				Next
				If strName.Length > 0 Then sZusatzBez = strMyName
				FilterBez += "Rechnungsversand wie (" & sZusatzBez & ") " & vbLf

				If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
					FilterBez += "Rechnungsversand nicht vorhanden " & vbLf
					sSql += strAndString & " (KD.OPVersand = '' Or KD.OPVersand Is Null) "
				Else
					If InStr(sZusatzBez, ",") > 0 Then
						sZusatzBez = Replace(sZusatzBez, ",", "','")
					End If
					sSql += strAndString & "KD.OPVersand In ('" & sZusatzBez & "')"

				End If
			End If

			' 1. Kreditwarnung ---------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_1Kredit.Text) <> String.Empty Then
				sZusatzBez = .Cbo_1Kredit.Text
				If InStr(UCase(sZusatzBez), UCase("nicht")) > 0 Then
					FilterBez += "1. Kreditüberschreitung nicht warnen" & vbLf
					sSql += strAndString & " (KD.KreditWarnung = 0 Or KD.KreditWarnung Is Null) "

				Else
					FilterBez += "1. Kreditüberschreitung warnen" & vbLf
					sSql += strAndString & "KD.KreditWarnung = 1 "
				End If

			End If

			' 1. Kredithöhe -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_KD1KLimite.Text) <> String.Empty Then
				sZusatzBez = .Cbo_KD1KLimite.Text

				strName = Regex.Split(sZusatzBez.Trim, ",")
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
				Next
				If strName.Length > 0 Then sZusatzBez = strMyName
				FilterBez += "1. Kreditlimite wie (" & sZusatzBez & ") " & vbLf

				If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
					sSql += strAndString & "(KD.Kreditlimite = 0 Or KD.Kreditlimite Is Null)"
				Else
					sSql += strAndString & "KD.Kreditlimite In (" & sZusatzBez & ")"
				End If

			End If

			' 2. Kredithöhe -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_KD2KLimite.Text) <> String.Empty Then
				sZusatzBez = .Cbo_KD2KLimite.Text

				strName = Regex.Split(sZusatzBez.Trim, ",")
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
				Next
				If strName.Length > 0 Then sZusatzBez = strMyName
				FilterBez += "2. Kreditlimite wie (" & sZusatzBez & ") " & vbLf

				If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
					sSql += strAndString & "(KD.Kreditlimite_2 = 0 Or KD.Kreditlimite_2 Is Null)"
				Else
					sSql += strAndString & "KD.Kreditlimite_2 In (" & sZusatzBez & ")"
				End If

			End If

			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.cbo_KDVerguetung.Text) <> String.Empty Then
				sZusatzBez = .cbo_KDVerguetung.Text

				strName = Regex.Split(sZusatzBez.Trim, ",")
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
				Next
				If strName.Length > 0 Then sZusatzBez = strMyName
				FilterBez += "Kunden-Vergütung wie (" & sZusatzBez & ") " & vbLf

				If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
					sSql += strAndString & "(KD.KD_UmsMin = 0 Or KD.KD_UmsMin Is Null)"
				Else
					sSql += strAndString & "KD.KD_UmsMin In (" & sZusatzBez & ")"
				End If

			End If


			' 1. Kredithöhe ist gültig zwischen -----------------------------------------------------------------------------------
			If .de_KDKredit_1.Text <> "" Then If Year(CDate(.de_KDKredit_1.Text)) = 1 Then .de_KDKredit_1.Text = String.Empty
			If .de_KDKredit_2.Text <> "" Then If Year(CDate(.de_KDKredit_2.Text)) = 1 Then .de_KDKredit_2.Text = String.Empty
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If .de_KDKredit_1.Text = String.Empty And .de_KDKredit_2.Text = String.Empty Then
			ElseIf .de_KDKredit_1.Text = .de_KDKredit_2.Text Then
				FilterBez += "1. Kredit ist gültig am = " & .de_KDKredit_1.Text & vbLf
				sSql += strAndString & "KD.KreditlimiteAb = '" &
										Format(CDate(.de_KDKredit_1.Text), strMyDateFormat) & "'"

			ElseIf .de_KDKredit_1.Text <> "" And .de_KDKredit_2.Text = "" Then
				FilterBez += "1. Kredit ist gültig ab " & .de_KDKredit_1.Text & vbLf
				sSql += strAndString & "KD.KreditlimiteAb >= '" &
										Format(CDate(.de_KDKredit_1.Text), strMyDateFormat) & "'"

			ElseIf .de_KDKredit_1.Text = "" And .de_KDKredit_2.Text <> "" Then
				FilterBez += "1. Kredit ist gültig bis " & .de_KDKredit_2.Text & vbLf
				sSql += strAndString & "KD.KreditlimiteAb <= '" &
										Format(CDate(.de_KDKredit_2.Text), strMyDateFormat) & "'"

			Else
				FilterBez += "1. Kredit ist gültig zwischen " & .de_KDKredit_1.Text &
										" und " & .de_KDKredit_2.Text & vbLf
				sSql += strAndString & "(KD.KreditlimiteBis >= '" & Format(CDate(.de_KDKredit_1.Text), strMyDateFormat) & "' Or " &
							"KD.KreditlimiteBis is Null)" &
							" And KD.KreditlimiteAb <= '" & Format(CDate(.de_KDKredit_2.Text), strMyDateFormat) & "'"

			End If

			' 2. Kredithöhe -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.txt_KL_Ref.Text) <> String.Empty Then
				sZusatzBez = .txt_KL_Ref.Text

				sZusatzBez = Replace(sZusatzBez, ", ", ",")
				FilterBez += "Kreditreferenz wie (" & sZusatzBez & ") " & vbLf
				If InStr(sZusatzBez, ",") > 0 Then
					sZusatzBez = Replace(sZusatzBez, ",", "','")
				End If
				sSql += strAndString & "IsNull(KD.KL_RefNr, '') In ('" & sZusatzBez & "')"

			End If

			'' KL_RefNr -------------------------------------------------------------------------------------------------------
			'strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			'If UCase(.txt_KL_Ref.Text) <> String.Empty Then
			'  sZusatzBez = .txt_KL_Ref.Text

			'  strName = Regex.Split(sZusatzBez.Trim, ",")
			'  strMyName = String.Empty
			'  For i As Integer = 0 To strName.Length - 1
			'    strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
			'  Next
			'  If strName.Length > 0 Then sZusatzBez = strMyName
			'  FilterBez += "Kreditreferenz wie (" & sZusatzBez & ") " & vbLf

			'  If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
			'    sSql += strAndString & "(KD.KL_RefNr = 0 Or KD.Kreditlimite_2 Is Null)"
			'  Else
			'    sSql += strAndString & "KD.KL_RefNr In (" & sZusatzBez & ")"
			'  End If

			'End If

			' Kreditlimite überschritten ----------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If .chkKDKreditlimiteUeberschritten.Checked Then

				' Alle Kunden die eine Kreditlimite haben und diese überschritten ist.
				Dim connection As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring) 'ClsDataDetail.GetSelectedConnstring)
				Dim sqlKreditlimite As String = "[List KDNR Kreditlimite ueberschritten]"
				Dim cmd As SqlCommand = New SqlCommand(sqlKreditlimite, connection)
				cmd.CommandType = CommandType.StoredProcedure
				cmd.Parameters.AddWithValue("@filiale", "")

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

			' KD Erfasst -------------------------------------------------------------------------------------------------------
			If .de_KDErfasst_1.Text <> "" Then If Year(CDate(.de_KDErfasst_1.Text)) = 1 Then .de_KDErfasst_1.Text = String.Empty
			If .de_KDErfasst_2.Text <> "" Then If Year(CDate(.de_KDErfasst_2.Text)) = 1 Then .de_KDErfasst_2.Text = String.Empty
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If .de_KDErfasst_1.Text = String.Empty And .de_KDErfasst_2.Text = String.Empty Then
			ElseIf .de_KDErfasst_1.Text = .de_KDErfasst_2.Text Then
				FilterBez += "Kunden erfasst am = " & .de_KDErfasst_1.Text & vbLf
				sSql += strAndString & "KD.CreatedOn Between '" &
										Format(CDate(.de_KDErfasst_1.Text), strMyDateFormat) & "' And '" &
										Format(DateAdd(DateInterval.Day, 1, CDate(.de_KDErfasst_2.Text)), strMyDateFormat) & "'"

			ElseIf .de_KDErfasst_1.Text <> "" And .de_KDErfasst_2.Text = "" Then
				FilterBez += "Kunden erfasst ab " & .de_KDErfasst_1.Text & vbLf
				sSql += strAndString & "KD.CreatedOn >= '" &
										Format(CDate(.de_KDErfasst_1.Text), strMyDateFormat) & "'"

			ElseIf .de_KDErfasst_1.Text = "" And .de_KDErfasst_2.Text <> "" Then
				FilterBez += "Kunden erfasst bis " & .de_KDErfasst_2.Text & vbLf
				sSql += strAndString & "KD.CreatedOn <= '" &
										Format(CDate(.de_KDErfasst_2.Text), strMyDateFormat) & "'"

			Else
				FilterBez += "Kunden erfasst zwischen " & .de_KDErfasst_1.Text &
										" und " & .de_KDErfasst_2.Text & vbLf
				sSql += strAndString & "KD.CreatedOn Between '" &
										Format(CDate(.de_KDErfasst_1.Text), strMyDateFormat) & "' And '" &
										Format(DateAdd(DateInterval.Day, 1, CDate(.de_KDErfasst_2.Text)), strMyDateFormat) & "'"
			End If

			' KD geändert -------------------------------------------------------------------------------------------------------
			If .de_KDChanged_1.Text <> "" Then If Year(CDate(.de_KDChanged_1.Text)) = 1 Then .de_KDChanged_1.Text = String.Empty
			If .de_KDChanged_2.Text <> "" Then If Year(CDate(.de_KDChanged_2.Text)) = 1 Then .de_KDChanged_2.Text = String.Empty
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If .de_KDChanged_1.Text = String.Empty And .de_KDChanged_2.Text = String.Empty Then
			ElseIf .de_KDChanged_1.Text = .de_KDChanged_2.Text Then
				FilterBez += "Kunden geändert am = " & .de_KDChanged_1.Text & vbLf
				sSql += strAndString & "KD.ChangedOn Between '" &
										Format(CDate(.de_KDChanged_1.Text), strMyDateFormat) & "' And '" &
										Format(DateAdd(DateInterval.Day, 1, CDate(.de_KDChanged_2.Text)), strMyDateFormat) & "'"

			ElseIf .de_KDChanged_1.Text <> "" And .de_KDChanged_2.Text = "" Then
				FilterBez += "Kunden geändert ab " & .de_KDChanged_1.Text & vbLf
				sSql += strAndString & "KD.ChangedOn >= '" &
										Format(CDate(.de_KDChanged_1.Text), strMyDateFormat) & "'"

			ElseIf .de_KDChanged_1.Text = "" And .de_KDChanged_2.Text <> "" Then
				FilterBez += "Kunden geändert bis " & .de_KDChanged_2.Text & vbLf
				sSql += strAndString & "KD.ChangedOn <= '" &
										Format(CDate(.de_KDChanged_2.Text), strMyDateFormat) & "'"

			Else
				FilterBez += "Kunden geändert zwischen " & .de_KDChanged_1.Text &
										" und " & .de_KDChanged_2.Text & vbLf
				sSql += strAndString & "KD.ChangedOn Between '" &
										Format(CDate(.de_KDChanged_1.Text), strMyDateFormat) & "' And '" &
										Format(DateAdd(DateInterval.Day, 1, CDate(.de_KDChanged_2.Text)), strMyDateFormat) & "'"
			End If


			' Im Einsatz -------------------------------------------------------------------------------------------------------
			If .Cbo_ES.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = .Cbo_ES.Text.Trim
				sSql += strAndString & "(KD.KDNr "

				If InStr(UCase(sZusatzBez), UCase("nicht")) > 0 Then
					FilterBez += "Nicht im Einsatz " & vbLf
					sSql += "Not "
				Else
					FilterBez += "Im Einsatz " & vbLf

				End If
				sSql += "In (Select ES.KDNr From ES)) "

			End If

			' Telefax Mailing ---------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_KDFax_NoMailing.Text) <> String.Empty Then
				strFieldName = "KD.KD_Telefax_Mailing"
				sZusatzBez = .Cbo_KDFax_NoMailing.Text.Trim
				If InStr(UCase(sZusatzBez), UCase("nicht")) > 0 Then
					FilterBez += m_xml.GetSafeTranslationValue("Faxmailing darf gesendet werden") & vbLf
					sSql += String.Format("{0}({1} = 0 Or {1} Is Null)", strAndString, strFieldName)

				Else
					FilterBez += m_xml.GetSafeTranslationValue("Faxmailing darf nicht gesendet werden") & vbLf
					sSql += String.Format("{0}{1} = 1", strAndString, strFieldName)

				End If

			End If

			' Mailing Mailing ---------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_KDMail_NoMailing.Text) <> String.Empty Then
				strFieldName = "KD.KD_Mail_Mailing"
				sZusatzBez = .Cbo_KDMail_NoMailing.Text.Trim
				If InStr(UCase(sZusatzBez), UCase("nicht")) > 0 Then
					FilterBez += m_xml.GetSafeTranslationValue("E-Mailmailing darf gesendet werden") & vbLf
					sSql += String.Format("{0}({1} = 0 Or {1} Is Null)", strAndString, strFieldName)

				Else
					FilterBez += m_xml.GetSafeTranslationValue("E-Mailmailing darf nicht gesendet werden") & vbLf
					sSql += String.Format("{0}{1} = 1", strAndString, strFieldName)

				End If
			End If

			' Sonstiges: Allgemein: Kommunikationsart------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If .txt_KDKontaktArten.Text <> String.Empty Then
				Dim suchEingabe As String = .txt_KDKontaktArten.Text.Replace(" ", "")
				Select Case DirectCast(.cbo_KDKontaktArten.SelectedItem, ComboBoxItem).Value
					Case "0" ' Telefon/Telefax/Natel
						sSql += String.Format("{0}Replace(KD.Telefon, ' ', '') Like '%{1}%' Or ", strAndString, suchEingabe)
						sSql += String.Format("Replace(KD.Telefax, ' ', '') Like '%{0}%' Or ", suchEingabe)
						sSql += String.Format("Replace(KD_Zustaendig.Telefon, ' ', '') Like '%{0}%' Or ", suchEingabe)
						sSql += String.Format("Replace(KD_Zustaendig.Telefax, ' ', '') Like '%{0}%' Or ", suchEingabe)
						sSql += String.Format("Replace(KD_Zustaendig.Natel, ' ', '') Like '%{0}%'", suchEingabe)
					Case "1" ' E-Mail
						sSql += String.Format("{0}KD.EMail Like '%{1}%' Or ", strAndString, suchEingabe)
						sSql += String.Format("KD_Zustaendig.EMail Like '%{0}%'", suchEingabe)
					Case "2" ' Homepage
						sSql += String.Format("{0}KD.Homepage Like '%{1}%'", strAndString, suchEingabe)
				End Select
			End If

			' Kundenberufe -------------------------------------------------------------------------------------------------------
			If .Lst_KDBerufe.Items.Count > 0 Then
				ExcludeValue = .ExcludeSelectedLSTValue(.btnHideKDBerufe)
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = GetLstItems(.Lst_KDBerufe) '.Replace("#@", "','")
				'sZusatzBez = Replace(sZusatzBez, "'", "''")

				'sSql += strAndString & "KD_Berufe.Bezeichnung In ('"
				'sSql += Replace(sZusatzBez, "#@", "','") & "')"

				sSql &= String.Format("{0}{1} EXISTS(SELECT TOP 1 KDNr FROM KD_Berufe WHERE Bezeichnung In ({2}) AND KDNr = KD.KDNR)", strAndString, If(ExcludeValue, "NOT", ""), sZusatzBez)

				FilterBez += String.Format("Kundenberufe {2} ({1}){0}", vbNewLine, sZusatzBez, If(ExcludeValue, "NICHT in", "in"))
			End If

			' Kundenbranchen -----------------------------------------------------------------------------------------------------
			If .Lst_KDBranche.Items.Count > 0 Then
				ExcludeValue = .ExcludeSelectedLSTValue(.btnHideKDBranchen)
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = GetLstItems(.Lst_KDBranche) '.Replace("#@", "','")
				'sZusatzBez = Replace(sZusatzBez, "'", "''")

				'sSql += strAndString & "KD_Branche.Bezeichnung In ('"
				'sSql += Replace(sZusatzBez, "#@", "','") & "')"

				sSql &= String.Format("{0}{1} EXISTS(SELECT TOP 1 KDNr FROM KD_Branche WHERE Bezeichnung In ({2}) AND KDNr = KD.KDNR)", strAndString, If(ExcludeValue, "NOT", ""), sZusatzBez)

				FilterBez += String.Format("Kundenbranche {2} ({1}){0}", vbNewLine, sZusatzBez, If(ExcludeValue, "NICHT in", "in"))
				'FilterBez += "Kundenbranche in (" & sZusatzBez & ")" & vbLf
			End If

			' Kundenstichwort ----------------------------------------------------------------------------------------------------
			If .Lst_KDStichwort.Items.Count > 0 Then
				ExcludeValue = .ExcludeSelectedLSTValue(.btnHideKDStichwort)
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = GetLstItems(.Lst_KDStichwort) '.Replace("#@", "','")
				'sZusatzBez = Replace(sZusatzBez, "'", "''")

				'sSql += strAndString & "KD_Stichwort.Bezeichnung In ('"
				'sSql += Replace(sZusatzBez, "#@", "','") & "')"

				sSql &= String.Format("{0}{1} EXISTS(SELECT TOP 1 KDNr FROM KD_Stichwort WHERE IsNull(Bezeichnung, '') In ({2}) AND KDNr = KD.KDNR)", strAndString, If(ExcludeValue, "NOT", ""), sZusatzBez)

				FilterBez += String.Format("Kundenstichwort {2} ({1}){0}", vbNewLine, sZusatzBez, If(ExcludeValue, "NICHT in", "in"))
				'FilterBez += "Kundenstichwort in (" & sZusatzBez & ")" & vbLf
			End If

			' Kundenfiliale ------------------------------------------------------------------------------------------------------
			If .Lst_KDFiliale.Items.Count > 0 Then
				ExcludeValue = .ExcludeSelectedLSTValue(.btnHideKDFiliale)
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = GetLstItems(.Lst_KDFiliale) '.Replace("#@", "','")
				'sZusatzBez = Replace(sZusatzBez, "'", "''")

				'sSql += strAndString & "KD_Filiale.Bezeichnung In ('"
				'sSql += Replace(sZusatzBez, "#@", "','") & "')"

				sSql &= String.Format("{0}{1} EXISTS(SELECT TOP 1 KDNr FROM KD_Filiale WHERE IsNull(Bezeichnung, '') In ({2}) AND KDNr = KD.KDNR)", strAndString, If(ExcludeValue, "NOT", ""), sZusatzBez)

				FilterBez += String.Format("Kundenfiliale {2} ({1}){0}", vbNewLine, sZusatzBez, If(ExcludeValue, "NICHT in", "in"))
				'FilterBez += "Kundenfiliale in (" & sZusatzBez & ")" & vbLf
			End If

			' Kundenanstellungsart -----------------------------------------------------------------------------------------------
			If .Lst_KDAnstellung.Items.Count > 0 Then
				ExcludeValue = .ExcludeSelectedLSTValue(.btnHideKDAnstellung)
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = GetLstItems(.Lst_KDAnstellung) '.Replace("#@", "','")
				'sZusatzBez = Replace(sZusatzBez, "'", "''")

				'sSql += strAndString & "KD_Anstellung.Bezeichnung In ('"
				'sSql += Replace(sZusatzBez, "#@", "','") & "')"

				sSql &= String.Format("{0}{1} EXISTS(SELECT TOP 1 KDNr FROM KD_Anstellung WHERE IsNull(Bezeichnung, '') In ({2}) AND KDNr = KD.KDNR)", strAndString, If(ExcludeValue, "NOT", ""), sZusatzBez)

				FilterBez += String.Format("Kundenanstellungsart {2} ({1}){0}", vbNewLine, sZusatzBez, If(ExcludeValue, "NICHT in", "in"))
				'FilterBez += "Kundenanstellungsart in (" & sZusatzBez & ")" & vbLf
			End If

			' KD-GAV -------------------------------------------------------------------------------------------------------
			If .Lst_KDGAV.Items.Count > 0 Then
				ExcludeValue = .ExcludeSelectedLSTValue(.btnHideKDGAV)
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = GetLstItems(.Lst_KDGAV) '.Replace("#@", "','")
				'sZusatzBez = Replace(sZusatzBez, "'", "''")
				'sZusatzBez = Replace(sZusatzBez, "'", "''")

				'sSql += strAndString & "KD_GAVGruppe.Bezeichnung In ('"
				'sSql += Replace(sZusatzBez, "#@", "','") & "')"

				sSql &= String.Format("{0}{1} EXISTS(SELECT TOP 1 KDNr FROM KD_GAVGruppe WHERE IsNull(Bezeichnung, '') In ({2}) AND KDNr = KD.KDNR)", strAndString, If(ExcludeValue, "NOT", ""), sZusatzBez)

				FilterBez += String.Format("Kunden-GAVberufe {2} ({1}){0}", vbNewLine, sZusatzBez, If(ExcludeValue, "NICHT in", "in"))
				'FilterBez += "Kunden-GAVberufe in (" & sZusatzBez & ")" & vbLf
			End If


			' 3. Seite ===========================================================================================================
			' ====================================================================================================================

			' Z_Nachname -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.txtZName.Text.Trim) <> String.Empty Then
				sZusatzBez = .txtZName.Text.Trim

				strName = Regex.Split(sZusatzBez.Trim, ",")
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
				Next
				If strName.Length > 0 Then sZusatzBez = strMyName
				FilterBez += "Nachname wie (" & sZusatzBez & ") " & vbLf

				If InStr(sZusatzBez, ",") > 0 Then
					sZusatzBez = Replace(sZusatzBez, ",", "','")
				End If
				If sZusatzBez.Contains("*") Or sZusatzBez.Contains("%") Then
					sSql += strAndString & "KD_Zustaendig.Nachname Like '" & Replace(sZusatzBez, "*", "%") & "'"
				Else
					sSql += strAndString & "KD_Zustaendig.Nachname In ('" & sZusatzBez & "')"
				End If

			End If

			' Z_Vorname -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.txtZVorname.Text.Trim) <> String.Empty Then
				sZusatzBez = .txtZVorname.Text.Trim

				strName = Regex.Split(sZusatzBez.Trim, ",")
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
				Next
				If strName.Length > 0 Then sZusatzBez = strMyName
				FilterBez += "Vorname wie (" & sZusatzBez & ") " & vbLf

				If InStr(sZusatzBez, ",") > 0 Then
					sZusatzBez = Replace(sZusatzBez, ",", "','")
				End If
				If sZusatzBez.Contains("*") Or sZusatzBez.Contains("%") Then
					sSql += strAndString & "KD_Zustaendig.Vorname Like '" & Replace(sZusatzBez, "*", "%") & "'"
				Else
					sSql += strAndString & "KD_Zustaendig.Vorname In ('" & sZusatzBez & "')"
				End If

			End If

			' Z_Berater -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_ZBerater.Text.Trim) <> String.Empty Then
				cv = DirectCast(.Cbo_ZBerater.SelectedItem, ComboBoxItem)
				sZusatzBez = cv.Value

				strName = Regex.Split(sZusatzBez.Trim, ",")
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
				Next
				If strName.Length > 0 Then sZusatzBez = strMyName
				FilterBez += "BeraterIn wie (" & sZusatzBez & ") " & vbLf

				If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
					sSql += strAndString & " (KD_Zustaendig.Berater = '' Or KD_Zustaendig.Berater Is Null) "

				Else
					If InStr(sZusatzBez, ",") > 0 Then
						sZusatzBez = Replace(sZusatzBez, ",", "','")
					End If
					sSql += strAndString & "KD_Zustaendig.Berater In ('" & sZusatzBez & "')"

				End If
			End If

			' Z_Abteilung -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_ZAbteilung.Text.Trim) <> String.Empty Then
				sZusatzBez = .Cbo_ZAbteilung.Text.Trim

				strName = Regex.Split(sZusatzBez.Trim, ",")
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
				Next
				If strName.Length > 0 Then sZusatzBez = strMyName
				FilterBez += "Abteilung wie (" & sZusatzBez & ") " & vbLf

				If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
					sSql += strAndString & " (KD_Zustaendig.Abteilung = '' Or KD_Zustaendig.Abteilung Is Null) "

				Else
					If InStr(sZusatzBez, ",") > 0 Then
						sZusatzBez = Replace(sZusatzBez, ",", "','")
					End If
					sSql += strAndString & "KD_Zustaendig.Abteilung In ('" & sZusatzBez & "')"

				End If
			End If

			' Z_Position -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_ZPosition.Text) <> String.Empty Then
				sZusatzBez = .Cbo_ZPosition.Text

				strName = Regex.Split(sZusatzBez.Trim, ",")
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
				Next
				If strName.Length > 0 Then sZusatzBez = strMyName
				FilterBez += "Position wie (" & sZusatzBez & ") " & vbLf

				If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
					sSql += strAndString & " (KD_Zustaendig.Position = '' Or KD_Zustaendig.Position Is Null) "

				Else
					If InStr(sZusatzBez, ",") > 0 Then
						sZusatzBez = Replace(sZusatzBez, ",", "','")
					End If
					sSql += strAndString & "KD_Zustaendig.Position In ('" & sZusatzBez & "')"
				End If

			End If

			' Z_Kommunikation -----------------------------------------------------------------------------------------------
			If .Lst_ZKom.Items.Count > 0 Then
				ExcludeValue = .ExcludeSelectedLSTValue(.btnHideZKom)
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = GetLstItems(.Lst_ZKom)
				'sZusatzBez = Replace(sZusatzBez, "'", "''")

				sSql &= String.Format("{0}{1} EXISTS(SELECT TOP 1 KDZNr FROM KD_ZKomm WHERE Bezeichnung In ({2}) AND KDNr = KD.KDNR AND KDZNr = dbo.KD_Zustaendig.RecNr)", strAndString, If(ExcludeValue, "NOT", ""), sZusatzBez)
				'sSql += String.Format("KD_ZKomm.Bezeichnung In ({1}) ", strAndString, sZusatzBez)
				'sSql += Replace(sZusatzBez, "#@", "','") & "')"

				FilterBez += .LibZKom.Text & " in (" & sZusatzBez & ")" & vbLf
			End If

			' Z_Versandart -----------------------------------------------------------------------------------------------
			If .Lst_ZVersand.Items.Count > 0 Then
				ExcludeValue = .ExcludeSelectedLSTValue(.btnHideZVersand)
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = GetLstItems(.Lst_ZVersand)
				'sZusatzBez = Replace(sZusatzBez, "'", "''")

				sSql &= String.Format("{0}{1} EXISTS(SELECT TOP 1 KDZNr FROM KD_ZKontaktArt WHERE Bezeichnung In ({2}) AND KDNr = KD.KDNR AND KDZNr = dbo.KD_Zustaendig.RecNr)", strAndString, If(ExcludeValue, "NOT", ""), sZusatzBez)
				'sSql += String.Format("KD_ZKontaktArt.Bezeichnung In ({1}) ", strAndString, sZusatzBez)

				'sSql += strAndString & "KD_ZKontaktArt.Bezeichnung In ('"
				'    sSql += Replace(sZusatzBez, "#@", "','") & "')"

				FilterBez += .LibZVersand.Text & " in (" & sZusatzBez & ")" & vbLf
			End If

			' Z_Kontakt -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_ZKontakt.Text.Trim) <> String.Empty Then
				ExcludeValue = .ExcludeSelectedComboboxValue(.Cbo_ZKontakt)
				sZusatzBez = .Cbo_ZKontakt.Text.Trim

				strName = Regex.Split(sZusatzBez.Trim, ",")
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
				Next
				If strName.Length > 0 Then sZusatzBez = strMyName
				'FilterBez += .LblChange_9.Text & " wie (" & sZusatzBez & ") " & vbLf

				If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
					sSql += strAndString & " (KD_Zustaendig.KDZHowKontakt = '' Or KD_Zustaendig.KDZHowKontakt Is Null) "

				Else
					'If InStr(sZusatzBez, ",") > 0 Then
					'	sZusatzBez = Replace(sZusatzBez, ",", "','")
					'End If

					sSql += String.Format("{0}KD_Zustaendig.KDZHowKontakt {1} ('{2}')", strAndString, If(ExcludeValue, "Not In", "In"), sZusatzBez.Trim.Replace(",", "','"))
					'sSql += strAndString & "KD_Zustaendig.KDZHowKontakt In ('" & sZusatzBez & "')"
				End If
				FilterBez += String.Format("{1} {2} ({3}){0}", vbNewLine, .LblChange_9.Text, If(ExcludeValue, "NICHT in", "in"), sZusatzBez)

			End If

			' Z_Stat 1 -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_Z1Stat.Text) <> String.Empty Then
				ExcludeValue = .ExcludeSelectedComboboxValue(.Cbo_Z1Stat)
				sZusatzBez = .Cbo_Z1Stat.Text.Trim

				strName = Regex.Split(sZusatzBez.Trim, ",")
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
				Next
				If strName.Length > 0 Then sZusatzBez = strMyName
				'FilterBez += .LblChange_10.Text & " wie (" & sZusatzBez & ") " & vbLf

				If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
					sSql += strAndString & " (KD_Zustaendig.KDZState1 = '' Or KD_Zustaendig.KDZState1 Is Null) "

				Else
					'If InStr(sZusatzBez, ",") > 0 Then
					'	sZusatzBez = Replace(sZusatzBez, ",", "','")
					'End If
					sSql += String.Format("{0}KD_Zustaendig.KDZState1 {1} ('{2}')", strAndString, If(ExcludeValue, "Not In", "In"), sZusatzBez.Trim.Replace(",", "','"))
					'sSql += strAndString & "KD_Zustaendig.KDZState1 In ('" & sZusatzBez & "')"
				End If
				FilterBez += String.Format("{1} {2} ({3}){0}", vbNewLine, .LblChange_10.Text, If(ExcludeValue, "NICHT in", "in"), sZusatzBez)

			End If

			' Z_Stat 2 -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_Z2Stat.Text) <> String.Empty Then
				ExcludeValue = .ExcludeSelectedComboboxValue(.Cbo_Z2Stat)
				sZusatzBez = .Cbo_Z2Stat.Text.Trim

				strName = Regex.Split(sZusatzBez.Trim, ",")
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
				Next
				If strName.Length > 0 Then sZusatzBez = strMyName
				'FilterBez += .LblChange_11.Text & " wie (" & sZusatzBez & ") " & vbLf

				If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
					sSql += strAndString & " (KD_Zustaendig.KDZState2 = '' Or KD_Zustaendig.KDZState2 Is Null) "

				Else
					'If InStr(sZusatzBez, ",") > 0 Then
					'	sZusatzBez = Replace(sZusatzBez, ",", "','")
					'End If
					sSql += String.Format("{0}KD_Zustaendig.KDZState2 {1} ('{2}')", strAndString, If(ExcludeValue, "Not In", "In"), sZusatzBez.Trim.Replace(",", "','"))
					'sSql += strAndString & "KD_Zustaendig.KDZState2 In ('" & sZusatzBez & "')"

				End If
				FilterBez += String.Format("{1} {2} ({3}){0}", vbNewLine, .LblChange_11.Text, If(ExcludeValue, "NICHT in", "in"), sZusatzBez)

			End If

			' Z-Geb.-Dat ---------------------------------------------------------------------------------------------------------
			If .de_ZGeb_1.Text <> "" Then If Year(CDate(.de_ZGeb_1.Text)) = 1 Then .de_ZGeb_1.Text = String.Empty
			If .de_ZGeb_2.Text <> "" Then If Year(CDate(.de_ZGeb_2.Text)) = 1 Then .de_ZGeb_2.Text = String.Empty
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If .de_ZGeb_1.Text.Trim = String.Empty And .de_ZGeb_2.Text.Trim = String.Empty Then
			ElseIf .de_ZGeb_1.Text = .de_ZGeb_2.Text Then
				FilterBez += "Geboren am = " & .de_ZGeb_1.Text & vbLf
				sSql += strAndString & "KD_Zustaendig.Geb_dat = '" &
										Format(CDate(.de_ZGeb_1.Text), strMyDateFormat) & "'"

			ElseIf .de_ZGeb_1.Text <> "" And .de_ZGeb_2.Text = "" Then
				FilterBez += "Gebortsdatum ab " & .de_ZGeb_1.Text & vbLf
				sSql += strAndString & "KD_Zustaendig.Geb_dat >= '" &
										Format(CDate(.de_ZGeb_1.Text), strMyDateFormat) & "'"

			ElseIf .de_ZGeb_1.Text = "" And .de_ZGeb_2.Text <> "" Then
				FilterBez += "Gebortsdatum bis " & .de_ZGeb_2.Text & vbLf
				sSql += strAndString & "KD_Zustaendig.Geb_dat <= '" &
										Format(CDate(.de_ZGeb_2.Text), strMyDateFormat) & "'"

			Else
				FilterBez += "Gebortsdatum zwischen " & .de_ZGeb_1.Text &
										" und " & .de_ZGeb_2.Text & vbLf
				sSql += strAndString & "KD_Zustaendig.Geb_dat Between '" &
										Format(CDate(.de_ZGeb_1.Text), strMyDateFormat) & "' And '" &
										Format(CDate(.de_ZGeb_2.Text), strMyDateFormat) & "'"
			End If

			' Geburtsmonat -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If .Cbo_ZMGeb.Text <> "" Then
				sZusatzBez = .Cbo_ZMGeb.Text.Trim
				FilterBez += "Geburtsmonat = " & CInt(sZusatzBez) & vbLf

				If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
					sSql += strAndString & " (KD_Zustaendig.Geb_Dat = '' Or KD_Zustaendig.Geb_Dat Is Null) "

				Else
					sSql += strAndString & "Month(KD_Zustaendig.Geb_Dat) = " & CInt(sZusatzBez) & ""
				End If
			End If

			' Z-Kontakt ----------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If .de_ZKontakt_1.Text = String.Empty And .de_ZKontakt_2.Text = String.Empty Then
			ElseIf .de_ZKontakt_1.Text = .de_ZKontakt_2.Text Then
				FilterBez += "Kontakt erfasst am = " & .de_ZKontakt_1.Text & vbLf
				sSql += strAndString & "KDK.KontaktDate Between '" &
										Format(CDate(.de_ZKontakt_1.Text), strMyDateFormat) & "' And '" &
										Format(DateAdd(DateInterval.Day, 1, CDate(.de_ZKontakt_2.Text)), strMyDateFormat) & "'"

			ElseIf .de_ZKontakt_1.Text <> "" And .de_ZKontakt_2.Text = "" Then
				FilterBez += "Kontakt erfasst ab " & .de_ZKontakt_1.Text & vbLf
				sSql += strAndString & "KDK.KontaktDate >= '" &
										Format(CDate(.de_ZKontakt_1.Text), strMyDateFormat) & "'"

			ElseIf .de_ZKontakt_1.Text = "" And .de_ZKontakt_2.Text <> "" Then
				FilterBez += "Kunden erfasst bis " & .de_ZKontakt_2.Text & vbLf
				sSql += strAndString & "KDK.KontaktDate <= '" &
										Format(CDate(.de_ZKontakt_2.Text), strMyDateFormat) & "'"

			Else
				FilterBez += "Kontakt erfasst zwischen " & .de_ZKontakt_1.Text &
										" und " & .de_ZKontakt_2.Text & vbLf
				sSql += strAndString & "KDK.KontaktDate Between '" &
										Format(CDate(.de_ZKontakt_1.Text), strMyDateFormat) & "' And '" &
										Format(CDate(.de_ZKontakt_2.Text), strMyDateFormat) & "'"
			End If

			' Text für Kontaktbeschreibung
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If .txt_ZKontaktBez.Text <> "" Then
				sZusatzBez = .txt_ZKontaktBez.Text.Trim
				FilterBez += "Beschreibung wie: " & sZusatzBez & vbLf
				sSql += strAndString & "KDK.Kontakte Like '%" & sZusatzBez & "%'"
			End If

			' Kontakttyp -------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_ZKontaktTyp.Text) <> String.Empty Then
				sZusatzBez = .Cbo_ZKontaktTyp.Text
				FilterBez += "Kontakttyp wie: " & sZusatzBez & vbLf
				sSql += strAndString & "KDK.KontaktType1 = '" & sZusatzBez & "'"

			End If

			' Erledigte Kontakte -------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_ZKontaktDown.Text) <> String.Empty Then
				sZusatzBez = .Cbo_ZKontaktDown.Text
				If InStr(UCase(sZusatzBez), UCase("nicht")) > 0 Then
					FilterBez += "Nicht erledigte Kontakte" & vbLf
					sSql += strAndString & "(KDK.KontaktErledigt = 0 Or KDK.KontaktErledigt Is Null) "

				Else
					FilterBez += "Erledigte Kontakte" & vbLf
					sSql += strAndString & "KDK.KontaktErledigt = 1 "
				End If

			End If

			' NoKontaktseit ----------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If .de_NoKontaktseit.Text <> "" Then
				FilterBez += "Keine Kundenkontakte seit " & .de_NoKontaktseit.Text & vbLf
				sSql += String.Format("{0} KD.KDNr In (Select KDNr From #KDMax Where #KDMax.KDNr = KD.KDNR And KD.KDNr > 0) ", strAndString, CDate(.de_NoKontaktseit.Text))
			End If

			' Kontaktart ausblenden -------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If Not String.IsNullOrWhiteSpace(.cbo_KDArtHide.Text) AndAlso String.IsNullOrWhiteSpace(.de_NoKontaktseit.Text) Then
				sZusatzBez = .cbo_KDArtHide.Text
				FilterBez += "Kontaktart ausblenden wie: " & sZusatzBez & vbLf
				'sZusatzBez = sZusatzBez.Replace(.cbo_KDArtHide.Properties.SeparatorChar, "','")

				strName = Regex.Split(sZusatzBez.Trim, .cbo_KDArtHide.Properties.SeparatorChar)
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, .cbo_KDArtHide.Properties.SeparatorChar, "")) & strName(i).ToString.Trim
				Next
				If strName.Length > 0 Then sZusatzBez = strMyName


				sSql += String.Format("{0}KDK.KontaktType1 {1} ('{2}')", strAndString, "Not In", strMyName.Trim.Replace(.cbo_KDArtHide.Properties.SeparatorChar, "','"))

				'sSql += String.Format("{0}KDK.KontaktType1 Not In ('{1}')", strAndString, sZusatzBez)
			End If

			' (ZHD) Telefax Mailing ---------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_ZFax_NoMailing.Text) <> String.Empty Then
				strFieldName = "KD_Zustaendig.ZHD_Telefax_Mailing"
				sZusatzBez = .Cbo_ZFax_NoMailing.Text.Trim
				If InStr(UCase(sZusatzBez), UCase("nicht")) > 0 Then
					FilterBez += m_xml.GetSafeTranslationValue("Faxmailing darf gesendet werden") & vbLf
					sSql += String.Format("{0}({1} = 0 Or {1} Is Null)", strAndString, strFieldName)

				Else
					FilterBez += m_xml.GetSafeTranslationValue("Faxmailing darf nicht gesendet werden") & vbLf
					sSql += String.Format("{0}{1} = 1 ", strAndString, strFieldName)

				End If
			End If

			' (ZHD) SMS Mailing ---------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_ZSMS_NoMailing.Text) <> String.Empty Then
				strFieldName = "KD_Zustaendig.ZHD_SMS_Mailing"
				sZusatzBez = .Cbo_ZSMS_NoMailing.Text.Trim
				If InStr(UCase(sZusatzBez), UCase("nicht")) > 0 Then
					FilterBez += m_xml.GetSafeTranslationValue("SMS-Mailing darf gesendet werden") & vbLf
					sSql += String.Format("{0}({1} = 0 Or {1} Is Null)", strAndString, strFieldName)

				Else
					FilterBez += m_xml.GetSafeTranslationValue("SMS-Mailing darf nicht gesendet werden") & vbLf
					sSql += String.Format("{0}{1} = 1", strAndString, strFieldName)

				End If
			End If

			' (ZHD) Mailing Mailing ---------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_ZMail_NoMailing.Text) <> String.Empty Then
				strFieldName = "KD_Zustaendig.ZHD_Mail_Mailing"
				sZusatzBez = .Cbo_ZMail_NoMailing.Text.Trim
				If InStr(UCase(sZusatzBez), UCase("nicht")) > 0 Then
					FilterBez += m_xml.GetSafeTranslationValue("E-Mailmailing darf gesendet werden") & vbLf
					sSql += String.Format("{0}({1} = 0 Or {1} Is Null)", strAndString, strFieldName)

				Else
					FilterBez += m_xml.GetSafeTranslationValue("E-Mailmailing darf nicht gesendet werden") & vbLf
					sSql += String.Format("{0}{1} = 1", strAndString, strFieldName)

				End If
			End If


			' ZKontaktFrom  -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			strFieldName = "KDK.CreatedFrom"
			If UCase(.Cbo_ZKontaktFrom.Text) <> String.Empty Then
				sZusatzBez = .Cbo_ZKontaktFrom.Text.Trim
				FilterBez += "Kontakt erfasst durch(" & sZusatzBez & ") " & vbLf

				sZusatzBez = Replace(sZusatzBez, "'", "")
				strName = Regex.Split(sZusatzBez.Trim, ",")
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, " Or ", "")) & strFieldName & " Like '%" & strName(i).Trim & "%'"
				Next
				If strName.Length > 0 Then sZusatzBez = strMyName

				If InStr(sZusatzBez, ",") > 0 Then sZusatzBez = Replace(sZusatzBez, ",", ",")

				sSql += strAndString & " (" & sZusatzBez & ")"

			End If


			' 4. Seite ===========================================================================================================
			' ====================================================================================================================

			' Z_Berufe -----------------------------------------------------------------------------------------------
			If .Lst_ZBerufe.Items.Count > 0 Then
				ExcludeValue = .ExcludeSelectedLSTValue(.btnHideZBerufe)
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = GetLstItems(.Lst_ZBerufe) '.Replace("#@", "','")
				'sZusatzBez = Replace(sZusatzBez, "'", "''")

				'sSql += strAndString & "KD_ZBerufe.Bezeichnung In ('"
				'sSql += Replace(sZusatzBez, "#@", "','") & "')"

				sSql &= String.Format("{0}{1} EXISTS(SELECT TOP 1 KDZNr FROM KD_ZBerufe WHERE Bezeichnung In ({2}) AND KDNr = KD.KDNR AND KDZNr = dbo.KD_Zustaendig.RecNr)", strAndString, If(ExcludeValue, "NOT", ""), sZusatzBez)

				FilterBez += String.Format("Z.-Berufe {2} ({1}){0}", vbNewLine, sZusatzBez, If(ExcludeValue, "NICHT in", "in"))
				'FilterBez += "Z.-Berufe in (" & sZusatzBez & ")" & vbLf
			End If

			' Z_Branche -----------------------------------------------------------------------------------------------
			If .Lst_ZBranche.Items.Count > 0 Then
				ExcludeValue = .ExcludeSelectedLSTValue(.btnHideZBranche)
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = GetLstItems(.Lst_ZBranche) '.Replace("#@", "','")
				'sZusatzBez = Replace(sZusatzBez, "'", "''")

				'sSql += strAndString & "KD_ZBranche.Bezeichnung In ('"
				'sSql += Replace(sZusatzBez, "#@", "','") & "')"

				sSql &= String.Format("{0}{1} EXISTS(SELECT TOP 1 KDZNr FROM KD_ZBranche WHERE Bezeichnung In ({2}) AND KDNr = KD.KDNR AND KDZNr = dbo.KD_Zustaendig.RecNr)", strAndString, If(ExcludeValue, "NOT", ""), sZusatzBez)

				FilterBez += String.Format("Z.-Branche {2} ({1}){0}", vbNewLine, sZusatzBez, If(ExcludeValue, "NICHT in", "in"))
				'FilterBez += "Z.-Branche in (" & sZusatzBez & ")" & vbLf
			End If

			' Z_Res1 -----------------------------------------------------------------------------------------------
			If .Lst_Z1Res.Items.Count > 0 Then
				ExcludeValue = .ExcludeSelectedLSTValue(.btnHideZRes1)
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = GetLstItems(.Lst_Z1Res) '.Replace("#@", "','")
				'sZusatzBez = Replace(sZusatzBez, "'", "''")

				'sSql += strAndString & "KD_ZRes1.Bezeichnung In ('"
				'sSql += Replace(sZusatzBez, "#@", "','") & "')"

				sSql &= String.Format("{0}{1} EXISTS(SELECT TOP 1 KDZNr FROM KD_ZRes1 WHERE Bezeichnung In ({2}) AND KDNr = KD.KDNR AND KDZNr = dbo.KD_Zustaendig.RecNr)", strAndString, If(ExcludeValue, "NOT", ""), sZusatzBez)

				FilterBez += String.Format("{1} {2} ({3}){0}", vbNewLine, .lblzhdres1.Text, If(ExcludeValue, "NICHT in", "in"), sZusatzBez)
				'FilterBez += .lblzhdres1.Text & " in (" & sZusatzBez & ")" & vbLf
			End If

			' Z_Res2 -----------------------------------------------------------------------------------------------
			If .Lst_Z2Res.Items.Count > 0 Then
				ExcludeValue = .ExcludeSelectedLSTValue(.btnHideZRes2)
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = GetLstItems(.Lst_Z2Res) '.Replace("#@", "','")
				'sZusatzBez = Replace(sZusatzBez, "'", "''")

				'sSql += strAndString & "KD_ZRes2.Bezeichnung In ('"
				'sSql += Replace(sZusatzBez, "#@", "','") & "')"

				sSql &= String.Format("{0}{1} EXISTS(SELECT TOP 1 KDZNr FROM KD_ZRes2 WHERE Bezeichnung In ({2}) AND KDNr = KD.KDNR AND KDZNr = dbo.KD_Zustaendig.RecNr)", strAndString, If(ExcludeValue, "NOT", ""), sZusatzBez)

				FilterBez += String.Format("{1} {2} ({3}){0}", vbNewLine, .lblzhdres2.Text, If(ExcludeValue, "NICHT in", "in"), sZusatzBez)
			End If

			' Z_Res3 -----------------------------------------------------------------------------------------------
			If .Lst_Z3Res.Items.Count > 0 Then
				ExcludeValue = .ExcludeSelectedLSTValue(.btnHideZRes3)
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = GetLstItems(.Lst_Z3Res) '.Replace("#@", "','")
				'sZusatzBez = Replace(sZusatzBez, "'", "''")

				'sSql += strAndString & "KD_ZRes3.Bezeichnung In ('"
				'sSql += Replace(sZusatzBez, "#@", "','") & "')"
				sSql &= String.Format("{0}{1} EXISTS(SELECT TOP 1 KDZNr FROM KD_ZRes3 WHERE Bezeichnung In ({2}) AND KDNr = KD.KDNR AND KDZNr = dbo.KD_Zustaendig.RecNr)", strAndString, If(ExcludeValue, "NOT", ""), sZusatzBez)

				FilterBez += String.Format("{1} {2} ({3}){0}", vbNewLine, .lblzhdres3.Text, If(ExcludeValue, "NICHT in", "in"), sZusatzBez)
			End If

			' Z_Res4 -----------------------------------------------------------------------------------------------
			If .Lst_Z4Res.Items.Count > 0 Then
				ExcludeValue = .ExcludeSelectedLSTValue(.btnHideZRes4)
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = GetLstItems(.Lst_Z4Res) '.Replace("#@", "','")
				'sZusatzBez = Replace(sZusatzBez, "'", "''")

				'sSql += strAndString & "KD_ZRes4.Bezeichnung In ('"
				'sSql += Replace(sZusatzBez, "#@", "','") & "')"
				sSql &= String.Format("{0}{1} EXISTS(SELECT TOP 1 KDZNr FROM KD_ZRes4 WHERE Bezeichnung In ({2}) AND KDNr = KD.KDNR AND KDZNr = dbo.KD_Zustaendig.RecNr)", strAndString, If(ExcludeValue, "NOT", ""), sZusatzBez)

				FilterBez += String.Format("{1} {2} ({3}){0}", vbNewLine, .lblzhdres4.Text, If(ExcludeValue, "NICHT in", "in"), sZusatzBez)
			End If

			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			' Filialen Teilung...
			If strUSFiliale <> "" Then
				FilterBez += "Kundenfiliale wie (" & sZusatzBez & ")" & vbLf
				sSql += strAndString & "KD.KDFiliale Like '%" & strUSFiliale & "%' "
			End If

			If ClsDataDetail.MDData.MultiMD = 1 Then

				Dim mdnumber As String = ConvListObject2String(Me.mandantNumber, ", ")
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Kunden mit Mandantennummer: ({1}){0}"), vbLf, mdnumber)

				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sSql += String.Format("{0}KD.MDNr In ({1})", strAndString, mdnumber)
			End If

		End With
		ClsDataDetail.GetFilterBez = FilterBez

		m_Logger.LogInfo(String.Format("KDSearchWhereQuery: {0}", sSql))


		Return sSql
	End Function

	Function GetSortString(ByVal frmTest As frmKDSearch) As String
		Dim strSort As String = " Order by "
		Dim strSortBez As String = String.Empty
		Dim strName As String()
		Dim strMyName As String = String.Empty

		'0 - Kundennummer
		'1 - Kundenname
		'2 - Kundenplz + Kundenort
		'3 - Ort
		'4 - Kreditlimite
		'5 - Telefon
		'6 - Telefax
		'7 - Nachname, Vorname
		With frmTest
			strName = Regex.Split(.CboSort.Text.Trim, ",")
			strMyName = String.Empty
			If strName(0).ToString = String.Empty Then
				strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " Firma1"
				strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Kundenname"
				ClsDataDetail.LLOrderByStringKontaktliste = " Kundenliste.Firma1"
				ClsDataDetail.GetSortForPrint = " Firma1"

			Else
				For i As Integer = 0 To strName.Length - 1
					Select Case CInt(Val(Left(strName(i).ToString.Trim, 1)))
						Case 0      ' Nach Kundennummer
							If Left(strName(i).ToString.Trim, 1) <> "0" Then
								strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " " & strName(i).ToString.Trim
								strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & strName(i).ToString.Trim
								ClsDataDetail.LLOrderByStringKontaktliste = " Kundenliste.Firma1"

							Else
								strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " KDNr"
								strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Kundennummer"
								ClsDataDetail.LLOrderByStringKontaktliste = " Kundenliste.KDNR"
								ClsDataDetail.GetSortForPrint = " KDNr"

							End If

						Case 1      ' Kundenname
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " Firma1"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Kundenname"
							ClsDataDetail.LLOrderByStringKontaktliste = " Kundenliste.Firma1"
							ClsDataDetail.GetSortForPrint = " Firma1"

						Case 2      ' Kundenort
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " KDPLZ, KDOrt"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Kundenplz, Kundenort"
							ClsDataDetail.LLOrderByStringKontaktliste = " Kundenliste.KDPLZ, Kundenliste.KDOrt"
							ClsDataDetail.GetSortForPrint = " KDPLZ, KDOrt"

						Case 3      ' Ort
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " KDOrt"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Kunden-Ortschaft"
							ClsDataDetail.LLOrderByStringKontaktliste = " Kundenliste.KDOrt"
							ClsDataDetail.GetSortForPrint = " KDOrt"

						Case 4      ' Kreditlimite
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " KDKreditlimite"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Kunden-Kreditlimite"
							ClsDataDetail.LLOrderByStringKontaktliste = " Kundenliste.Firma1" ' Keine Kreditlimite auf der Kontaktliste
							ClsDataDetail.GetSortForPrint = " KDKreditlimite"

						Case 5      ' Telefon
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " KDTelefon"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Kunden-Telefon"
							ClsDataDetail.LLOrderByStringKontaktliste = " Kundenliste.Firma1" ' Keine Telefon auf der Kontaktliste
							ClsDataDetail.GetSortForPrint = " KDTelefon"

						Case 6      ' Telefax
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " KDTelefax"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Kunden-Telefax"
							ClsDataDetail.LLOrderByStringKontaktliste = " Kundenliste.Firma1" ' Keine Telefax auf der Kontaktliste
							ClsDataDetail.GetSortForPrint = " KDTelefax"

						Case 7      ' Nach und Vorname
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " Nachname, Vorname"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Nach- / Vorname"
							ClsDataDetail.LLOrderByStringKontaktliste = " #tblKDZustaendig.Nachname, #tblKDZustaendig.Vorname"
							ClsDataDetail.GetSortForPrint = " Nachname, Vorname"


						Case Else   ' Kundenname
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " Firma1, KDOrt"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Kundenname und Kundenort"
							ClsDataDetail.LLOrderByStringKontaktliste = " Kundenliste.Firma1 Kundenliste.KDOrt"
							ClsDataDetail.GetSortForPrint = " Firma1, KDOrt"

					End Select
				Next i
			End If

		End With
		strSort = strSort & strMyName
		ClsDataDetail.GetSortBez = strSortBez


		Return strSort
	End Function

	Function GetLstItems(ByVal lst As DevExpress.XtraEditors.ListBoxControl) As String
		Dim LSTContent As String = String.Empty

		For i = 0 To lst.Items.Count - 1
			LSTContent &= If(LSTContent.Trim.Length > 0, ",", "") & String.Format("'{0}'", lst.Items(i).ToString.Replace("'", "''"))
		Next

		Return LSTContent

	End Function

	Public Overloads Shared Function ConvListObject2String(ByVal lst As List(Of Integer), Optional ByVal Seperator As String = ", ") As String
		Dim str As New System.Text.StringBuilder
		For i As Integer = 0 To lst.Count - 1
			str.AppendFormat("{0}{1}", CInt(lst.Item(i)), If(i = lst.Count - 1, "", Seperator))
		Next
		Return str.ToString
	End Function

#End Region


End Class
