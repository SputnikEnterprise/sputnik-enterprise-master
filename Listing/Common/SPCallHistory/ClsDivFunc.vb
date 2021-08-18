
Imports System.IO
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports System.Runtime.CompilerServices

Imports SPProgUtility.CommonSettings
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities

Imports SP.Infrastructure.Logging

Public Class ClsDivFunc

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()


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

	' // KDzNr
	Dim _strKDZNr As String
	Public Property GetKDZNr() As String
		Get
			Return _strKDZNr
		End Get
		Set(ByVal value As String)
			_strKDZNr = value
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

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private _ClsSystem As New SPProgUtility.ClsProgSettingPath
	Private m_md As Mandant


	Public Property MandantNumber As New List(Of Integer)


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

#Region "Suche nach Normale CallHistory..."

	Function GetStartSQLString() As String
		Dim sSql As String = String.Empty

		' Die virtuelle Tabelle muss gelöscht werden, falls bereits erstellt
		sSql += "Begin Try Drop Table ##CallDb End Try Begin Catch End Catch; "

		sSql &= "Select CH.UsName As BeraterIn, "
		sSql &= "CH.UsName, (Case "
		sSql &= "	When Charindex(' ', CH.UsName, 1) > 0 Then SubString(ch.usname, 1, charindex(' ', ch.usname) -1) "

		sSql &= "    End "
		sSql &= ") As USVorname, "
		sSql &= "(Case "
		sSql &= "	When Charindex(' ', CH.UsName, 1) > 0 Then SubString(ch.usname, charindex(' ',ch.usname) +1, 100) "

		sSql &= "    End "
		sSql &= ") As USNachname, "
		sSql &= "CH.CalledTo, "
		sSql &= "CH.CallInfo As Info, CH.EventTime As Zeitpunkt, "
		sSql &= "CH.KDNr, CH.KDZHDNr As ZHDNr, CH.MANr As MANr, "
		sSql &= "(MA.Nachname + ', ' + Ma.Vorname) As [Kandidatenname], "
		sSql &= "KD.Firma1 As Firmenname, KD.FProperty, (KDZ.Nachname + ', ' + KDZ.Vorname) As ZuständigePerson "

		' Resultat in einer virtuellen Tabelle ablegen
		sSql += "Into ##CallDb "
		sSql &= "From CallHistory CH "
		sSql &= "Left Join Mitarbeiter MA On CH.MANr = MA.MANr "
		sSql &= "Left Join Kunden KD On CH.KDNr = KD.KDNr "
		sSql &= "Left Join KD_Zustaendig KDZ On CH.KDZHDNr = KDZ.RecNr And CH.KDNr = KDZ.KDNr "
		sSql &= "Left Join Benutzer US On CH.USNr = US.USNr "

		Return sSql
	End Function

	Function GetQuerySQLString(ByVal frmTest As frmCallHistory) As String
		Dim sSql As String = String.Empty
		Dim FilterBez As String = String.Empty
		Dim strAndString As String = String.Empty
		Dim cv As ComboValue

		With frmTest
			' EventTime -----------------------------------------------------------------------------------------
			If .deCallAt_1.Text <> "" Or .deCallAt_2.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				Dim erfasstAm As String = ""
				Dim erfasstBis As String = ""
				If IsDate(.deCallAt_1.Text) Then
					erfasstAm = Date.Parse(.deCallAt_1.Text).ToString("d")
				End If
				If IsDate(.deCallAt_2.Text) Then
					erfasstBis = Date.Parse(.deCallAt_2.Text).ToString("d")
				End If
				' Suche zwischen zwei Datum
				If erfasstAm.Length > 0 And erfasstBis.Length > 0 Then
					sSql += String.Format("{0}CH.EventTime Between Convert(DateTime, '{1} 00:00', 104) And Convert(DateTime, '{2} 23:59', 104)",
					 strAndString, erfasstAm, erfasstBis)
					FilterBez += String.Format("Eintrag erfasst zwischen {0} und {1}{2}", erfasstAm, erfasstBis, vbLf)
					' Suche ab erstes Datum
				ElseIf erfasstAm.Length > 0 Then
					sSql += String.Format("{0}CH.EventTime >= Convert(DateTime,'{1} 00:00', 104)", strAndString, erfasstAm)
					FilterBez += String.Format("Eintrag erfasst ab Datum {0}{1}", erfasstAm, vbLf)
					' Suche bis zweites Datum
				ElseIf erfasstBis.Length > 0 Then
					sSql += String.Format("{0}CH.EventTime <= Convert(DateTime,'{1} 23:59', 104)", strAndString, erfasstBis)
					FilterBez += String.Format("Eintrag erfasst bis Datum {0}{1}", erfasstBis, vbLf)
				End If
			End If

			If .Cbo_Berater.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_Berater.SelectedItem, ComboValue)

				Dim strBerater As String = cv.ComboValue ' .Cbo_Berater.ToItem.Value
				FilterBez += String.Format("BeraterIn: ({0}){1}", strBerater, vbLf)
				sSql += String.Format("{0}CH.USName In ('{1}')", strAndString, strBerater.Replace(",", "','"))
			End If

			If .Cbo_LstArt.Text.ToUpper.Contains("1") Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				FilterBez += String.Format("{0}{1}", .Cbo_LstArt.Text, vbLf)
				sSql += String.Format("{0}(CH.KDNr > 0 Or CH.KDZHDNr > 0) ", strAndString)

			ElseIf .Cbo_LstArt.Text.ToUpper.Contains("2") Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				FilterBez += String.Format("{0}{1}", .Cbo_LstArt.Text, vbLf)
				sSql += String.Format("{0}CH.MANr > 0 ", strAndString)

			End If

		End With

		ClsDataDetail.GetFilterBez = FilterBez

		Return sSql
	End Function

	Function GetSortString(ByVal frmTest As frmCallHistory) As String
		Dim strSort As String = " Order by "
		Dim strSortBez As String = String.Empty
		Dim strName As String()
		Dim strMyName As String = String.Empty

		'0 - BeraterIn
		'1 - Datum (Aufsteigend)

		With frmTest
			strName = Regex.Split(.CboSort.Text.Trim, ",")
			strMyName = String.Empty
			If .CboSort.Text.Contains("-") Then
				For i As Integer = 0 To strName.Length - 1
					Select Case CInt(Val(Left(strName(i).ToString.Trim, 1))) ' Das erste Zeichen der Sortierung
						Case 0          ' Nach BeraterIn
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " USNachname, USVorname, Zeitpunkt DESC"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ",", "")) & "BeraterIn und Erstellungsdatum"

						Case 1          ' Datum (Absteigend)
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " Zeitpunkt DESC"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Erstellungsdatum Absteigend"

						Case Else          ' Nach BeraterIn
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " USNachname, USVorname, Zeitpunkt DESC"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ",", "")) & "BeraterIn und Erstellungsdatum"

					End Select
				Next i
			Else
				strMyName = .CboSort.Text
				strSortBez = "Benutzerdefiniert"
			End If

		End With

		strSort = strSort & strMyName
		ClsDataDetail.GetSortBez = strSortBez

		Return strSort
	End Function


#End Region


#Region "Kandidatenkontakte..."

	Function GetStartSQLString4MA_Kontakt() As String
		Dim sSql As String = String.Empty

		' Die virtuelle Tabelle muss gelöscht werden, falls bereits erstellt
		sSql += "Begin Try Drop Table ##MAKontaktDb End Try "
		sSql += "Begin Catch End Catch "

		'sSql &= "Select MAK.ID, MAK.MANr, MAK.Kontakte, MAK.RecNr, MAK.KontaktDate, MAK.KontaktType1, "
		'sSql &= "MA.Nachname, MA.Vorname, m.Betreff, m.Beschreibung "

		sSql &= "Select MAK.RecNr, MAK.MANr, MAK.Kontakte As [Kontaktbeschreibung], MAK.KontaktDate As [Datum], MAK.KontaktType1 As [Kontakttype], "
		sSql &= "(MA.Nachname+', '+ MA.Vorname) As [Kandidatenname], MAK.KontaktDauer As [Aufgabenbetreff] "  'm.Beschreibung As [Aufgabenbeschreibung] "
		' Resultat in einer virtuellen Tabelle ablegen
		sSql += "Into ##MAKontaktDb "
		sSql &= "From MA_Kontakte MAK Left Join Mitarbeiter MA On MAK.MANr = MA.MANr "

		'sSql &= "Left Join [Sputnik DbSelect].[dbo].MyTasks m On m.KontaktRecNr = MAK.RecNr "

		Return sSql
	End Function


	Function GetQuerySQLString4MA_Kontakt(ByVal frmTest As frmCallHistory) As String
		Dim sSql As String = String.Empty
		Dim FilterBez As String = String.Empty
		Dim strAndString As String = String.Empty
		Dim cv As ComboValue

		With frmTest
			' EventTime -----------------------------------------------------------------------------------------
			If .deMAKontakt_1.Text <> "" Or .deMAKontakt_2.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				Dim erfasstAm As String = ""
				Dim erfasstBis As String = ""
				If IsDate(.deMAKontakt_1.Text) Then
					erfasstAm = Date.Parse(.deMAKontakt_1.Text).ToString("d")
				End If
				If IsDate(.deMAKontakt_2.Text) Then
					erfasstBis = Date.Parse(.deMAKontakt_2.Text).ToString("d")
				End If
				' Suche zwischen zwei Datum
				If erfasstAm.Length > 0 And erfasstBis.Length > 0 Then
					sSql += String.Format("{0}MAK.KontaktDate Between Convert(DateTime, '{1} 00:00', 104) And Convert(DateTime, '{2} 23:59', 104)",
					 strAndString, erfasstAm, erfasstBis)
					FilterBez += String.Format("Eintrag erfasst zwischen {0} und {1}{2}", erfasstAm, erfasstBis, vbLf)
					' Suche ab erstes Datum
				ElseIf erfasstAm.Length > 0 Then
					sSql += String.Format("{0}MAK.KontaktDate >= Convert(DateTime,'{1} 00:00', 104)", strAndString, erfasstAm)
					FilterBez += String.Format("Eintrag erfasst ab Datum {0}{1}", erfasstAm, vbLf)
					' Suche bis zweites Datum
				ElseIf erfasstBis.Length > 0 Then
					sSql += String.Format("{0}MAK.KontaktDate <= Convert(DateTime,'{1} 23:59', 104)", strAndString, erfasstBis)
					FilterBez += String.Format("Eintrag erfasst bis Datum {0}{1}", erfasstBis, vbLf)
				End If
			End If

			If .Cbo_MA_KST.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MA_KST.SelectedItem, ComboValue)

				Dim strBerater As String = cv.ComboValue ' .Cbo_Berater.ToItem.Value
				FilterBez += String.Format("BeraterIn: ({0}){1}", strBerater, vbLf)
				sSql += String.Format("{0}replace(MAK.CreatedFrom, ',', '') In ('{1}')", strAndString, strBerater.Replace(",", "','"))
			End If

			If .Cbo_MA_Art.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MA_Art.SelectedItem, ComboValue)

				Dim strMAArt As String = cv.ComboValue ' .Cbo_MA_Art.ToItem.Value
				FilterBez += String.Format("Kontaktart: ({0}){1}", strMAArt, vbLf)
				sSql += String.Format("{0}MAK.KontaktType1 In ('{1}')", strAndString, strMAArt.Replace(",", "','"))
			End If

			If .Cbo_MA_Kontakt.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				Dim strMAKBez As String = .Cbo_MA_Kontakt.Text
				FilterBez += String.Format("Beschreibung: ({0}){1}", strMAKBez, vbLf)
				sSql += String.Format("{0}MAK.Kontakte Like '%{1}%'", strAndString, strMAKBez.Replace("'", "''"))
			End If

			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			sSql += String.Format("{0}MAK.KontaktDate Is not Null ", strAndString)

			If ClsDataDetail.MDData.MultiMD = 1 Then
				Dim mdnumber As String = ConvListObject2String(Me.MandantNumber, ", ")
				FilterBez += String.Format("Kandidaten mit Mandantennummer: ({1}){0}", vbLf, mdnumber)

				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sSql += String.Format("{0}MA.MDNr In ({1})", strAndString, mdnumber)
			End If

		End With
		ClsDataDetail.GetFilterBez = FilterBez

		Return sSql
	End Function

	Function GetSortString4MA_Kontakt(ByVal frmTest As frmCallHistory) As String
		Dim strSort As String = String.Empty
		Dim strSortBez As String = String.Empty
		Dim strName As String()
		Dim strMyName As String = String.Empty

		'0 - Firmenname
		'1 - Kontaktdatum

		With frmTest
			strName = Regex.Split(.cboContactSort.Text.Trim, ",")
			strMyName = String.Empty
			If .cboContactSort.Text.Contains("-") Then
				For i As Integer = 0 To strName.Length - 1
					Select Case CInt(Val(Left(strName(i).ToString.Trim, 1))) ' Das erste Zeichen der Sortierung

						Case 0          ' Nach Kandidaten
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " Kandidatenname, Datum ASC"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ",", "")) & "Kandidaten und Datum aufsteigend"

						Case 1          ' Nach Kandidaten
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " Kandidatenname, Datum Desc"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ",", "")) & "Kandidaten und Datum absteigend"

						Case 2          ' Datum 
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " Datum ASC"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Datum aufsteigend"

						Case 3          ' Datum 
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " Datum Desc"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Datum absteigend"


						Case Else          ' Nach Kandidaten
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " Kandidatenname, Datum ASC"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ",", "")) & "Kandidaten und Datum aufsteigend"

					End Select
				Next i
			Else
				strMyName = .cboContactSort.Text
				strSortBez = "Benutzerdefiniert"
			End If

		End With
		If Not String.IsNullOrWhiteSpace(strMyName) Then
			strSort = " Order by "
			strSort &= strMyName
		End If

		ClsDataDetail.GetSortBez = strSortBez

		Return strSort
	End Function


#End Region


#Region "Kundenkontakte..."

	Function GetStartSQLString4KD_Kontakt() As String
		Dim sSql As String = String.Empty

		' Die virtuelle Tabelle muss gelöscht werden, falls bereits erstellt
		sSql += "Begin Try Drop Table ##KDKontaktDb End Try "
		sSql += "Begin Catch End Catch "

		sSql &= "Select K.RecNr, K.KDNr, K.KDZNr As ZHDNr, K.Kontakte As [Kontaktbeschreibung], K.KontaktDate As [Datum], K.KontaktType1 As [Kontakttype], "
		sSql &= "KD.Firma1 As [Firmenname], KD.FProperty, K.KontaktDauer As [Aufgabenbetreff], "
		sSql &= "(KDz.Nachname+', '+KDz.Vorname) As [ZuständigePerson] "

		' Resultat in einer virtuellen Tabelle ablegen
		sSql += "Into ##KDKontaktDb "
		sSql &= "From KD_KontaktTotal K "
		sSql &= "Left Join Kunden KD On K.KDNr = KD.KDNr "
		sSql &= "	Left Join KD_Zustaendig KDz On K.KDZNr = KDz.RecNr And KD.KDNr = KDz.KDNr "

		'sSql &= "Left Join [Sputnik DbSelect].[dbo].MyTasks m On m.KontaktRecNr = K.RecNr "

		Return sSql
	End Function

	Function GetQuerySQLString4KD_Kontakt(ByVal frmTest As frmCallHistory) As String
		Dim sSql As String = String.Empty
		Dim FilterBez As String = String.Empty
		Dim strAndString As String = String.Empty
		Dim cv As ComboValue

		With frmTest
			' EventTime -----------------------------------------------------------------------------------------
			If .deMAKontakt_1.Text <> "" Or .deMAKontakt_2.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				Dim erfasstAm As String = ""
				Dim erfasstBis As String = ""
				If IsDate(.deMAKontakt_1.Text) Then
					erfasstAm = Date.Parse(.deMAKontakt_1.Text).ToString("d")
				End If
				If IsDate(.deMAKontakt_2.Text) Then
					erfasstBis = Date.Parse(.deMAKontakt_2.Text).ToString("d")
				End If
				' Suche zwischen zwei Datum
				If erfasstAm.Length > 0 And erfasstBis.Length > 0 Then
					sSql += String.Format("{0}K.KontaktDate Between Convert(DateTime, '{1} 00:00', 104) And Convert(DateTime, '{2} 23:59', 104)",
					 strAndString, erfasstAm, erfasstBis)
					FilterBez += String.Format("Eintrag erfasst zwischen {0} und {1}{2}", erfasstAm, erfasstBis, vbLf)
					' Suche ab erstes Datum
				ElseIf erfasstAm.Length > 0 Then
					sSql += String.Format("{0}K.KontaktDate >= Convert(DateTime,'{1} 00:00', 104)", strAndString, erfasstAm)
					FilterBez += String.Format("Eintrag erfasst ab Datum {0}{1}", erfasstAm, vbLf)
					' Suche bis zweites Datum
				ElseIf erfasstBis.Length > 0 Then
					sSql += String.Format("{0}K.KontaktDate <= Convert(DateTime,'{1} 23:59', 104)", strAndString, erfasstBis)
					FilterBez += String.Format("Eintrag erfasst bis Datum {0}{1}", erfasstBis, vbLf)
				End If
			End If

			If .Cbo_MA_KST.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MA_KST.SelectedItem, ComboValue)

				Dim strBerater As String = cv.ComboValue ' .Cbo_Berater.ToItem.Value
				FilterBez += String.Format("BeraterIn: ({0}){1}", strBerater, vbLf)
				sSql += String.Format("{0}replace(K.CreatedFrom, ',', '') In ('{1}')", strAndString, strBerater.Replace(",", "','"))
			End If

			If .Cbo_MA_Art.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MA_Art.SelectedItem, ComboValue)

				Dim strMAArt As String = cv.ComboValue ' .Cbo_MA_Art.ToItem.Value
				FilterBez += String.Format("Kontaktart: ({0}){1}", strMAArt, vbLf)
				sSql += String.Format("{0}K.KontaktType1 In ('{1}')", strAndString, strMAArt.Replace(",", "','"))
			End If

			If .Cbo_MA_Kontakt.Text <> "" Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				Dim strMAKBez As String = .Cbo_MA_Kontakt.Text

				FilterBez += String.Format("Beschreibung: ({0}){1}", strMAKBez, vbLf)
				sSql += String.Format("{0}K.Kontakte Like '%{1}%'", strAndString, strMAKBez.Replace("'", "''"))
			End If

			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			sSql += String.Format("{0}K.KontaktDate Is not Null ", strAndString)

			If ClsDataDetail.MDData.MultiMD = 1 Then
				Dim mdnumber As String = ConvListObject2String(Me.mandantNumber, ", ")
				FilterBez += String.Format("Kunden mit Mandantennummer: ({1}){0}", vbLf, mdnumber)

				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sSql += String.Format("{0}KD.MDNr In ({1})", strAndString, mdnumber)


				'strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				'sSql &= String.Format("{0}KD.MDNr = {1}", strAndString, ClsDataDetail.ProgSettingData.SelectedMDNr)
			End If


		End With
		ClsDataDetail.GetFilterBez = FilterBez

		Return sSql
	End Function

	Function GetSortString4KD_Kontakt(ByVal frmTest As frmCallHistory) As String
		Dim strSort As String = String.Empty
		Dim strSortBez As String = String.Empty
		Dim strName As String()
		Dim strMyName As String = String.Empty

		'0 - Firmenname
		'1 - Kontaktdatum

		With frmTest
			strName = Regex.Split(.cboContactSort.Text.Trim, ",")
			strMyName = String.Empty
			If .cboContactSort.Text.Contains("-") Then
				For i As Integer = 0 To strName.Length - 1
					Select Case CInt(Val(Left(strName(i).ToString.Trim, 1))) ' Das erste Zeichen der Sortierung
						Case 0          ' Nach Firmenname, Datum Aufsteigend
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " Firmenname ASC, Datum ASC"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ",", "")) & "Firmenname und Datum aufsteigend"

						Case 1          ' Nach Firmenname, Datum Absteigend
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " Firmenname ASC, Datum DESC"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ",", "")) & "Firmenname und Datum absteigend"


						Case 2          ' Datum aufsteigend
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " Datum ASC"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Datum aufsteigend"

						Case 3          ' Datum absteigend
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " Datum DESC"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Datum absteigend"


						Case Else          ' Nach Firmenname
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " Firmenname ASC, Datum ASC"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ",", "")) & "Firmenname und Datum"

					End Select
				Next i
			Else
				strMyName = .cboContactSort.Text
				strSortBez = "Benutzerdefiniert"
			End If

		End With

		If Not String.IsNullOrWhiteSpace(strMyName) Then
			strSort = " Order by "
			strSort &= strMyName
		End If

		ClsDataDetail.GetSortBez = strSortBez

		Return strSort
	End Function

#End Region


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


	' Finale Datenbanken für Exportieren --------------------------------------------------------------------------------------
	Function GetFinalDb4Export(ByVal frmTest As frmCallHistory) As String
		' Query umstellen für die korrekte Interpretation des Selects von den Komponenten. 
		Dim strSqlQuery As String = frmTest.txt_SQLQuery.Text.Replace("##CallDb", "CallDb_" & _ClsSystem.GetLogedUSNr)
		Dim i As Integer = 0

		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			cmd.ExecuteNonQuery()

			strSqlQuery = strSqlQuery.Substring(strSqlQuery.IndexOf("Select * "))


		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return strSqlQuery
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

