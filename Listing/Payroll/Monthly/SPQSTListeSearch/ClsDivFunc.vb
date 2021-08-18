
Imports System.Data.SqlClient
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports SP.Infrastructure.Logging
Imports SP.DatabaseAccess.Listing.DataObjects
Imports SP.DatabaseAccess.Listing

Public Class ClsDbFunc


#Region "private consts"

	Private Const TAX_TABLE_NAME = "_QSTListe_{0}"

#End Region


#Region "private fields"

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private m_ConnectionString As String

	''' <summary>
	''' The Listing data access object.
	''' </summary>
	Private m_ListingDatabaseAccess As IListingDatabaseAccess

	Private m_UtilityUi As SP.Infrastructure.UI.UtilityUI
	Private m_md As Mandant
	Private m_utility As New Utilities
	Private m_QSTTablename As String

#End Region


#Region "public properties"

	Public Property mandantNumber As New List(Of Integer)
	Public Property SearchCriteriums As QSTListingSearchData

#End Region


#Region "constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

		m_UtilityUi = New SP.Infrastructure.UI.UtilityUI
		m_utility = New Utilities
		m_md = New Mandant

		m_ListingDatabaseAccess = New ListingDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)

		m_QSTTablename = String.Format(TAX_TABLE_NAME, m_InitializationData.UserData.UserNr)

	End Sub

#End Region


	Function LoadQSTCode(ByVal jahr As Integer, ByVal mvon As Integer, ByVal mbis As Integer, ByVal canton As String) As IEnumerable(Of CodeData)
		Dim result As List(Of CodeData) = Nothing
		Dim sql As String

		sql = "Select LO.Q_Steuer, ISNull( (Select Top 1 TQ.[Description] From Tab_Quell TQ Where TQ.GetFeld = LO.Q_Steuer ), '') CodeLabel "
		sql &= "From LO "
		sql += String.Format("Where LO.MDNr = {0} And LO.Jahr = {1} And ", m_InitializationData.MDData.MDNr, jahr)
		sql += String.Format("LO.LP Between {0} And {1} And ", mvon, mbis)
		sql += "(IsNull(@skanton, '') = '' OR LO.S_Kanton = @skanton) And "
		sql += "IsNull(LO.Q_Steuer, '') <> '' "
		sql += "Group By LO.Q_Steuer Order By LO.Q_Steuer"


		Dim listOfParams As New List(Of SqlClient.SqlParameter)
		listOfParams.Add(New SqlClient.SqlParameter("skanton", ReplaceMissing(canton, DBNull.Value)))


		Dim reader = m_utility.OpenReader(m_InitializationData.MDData.MDDbConn, sql, listOfParams, CommandType.Text)

		result = New List(Of CodeData)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of CodeData)

				While reader.Read()
					Dim data = New CodeData
					data.Code = m_utility.SafeGetString(reader, "Q_Steuer")
					data.CodeLabel = m_utility.SafeGetString(reader, "CodeLabel")

					result.Add(data)

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

	Function LoadPermissionCode(ByVal jahr As Integer, ByVal mvon As Integer, ByVal mbis As Integer, ByVal canton As String, ByVal qstCodes As String) As IEnumerable(Of CodeData)
		Dim result As List(Of CodeData) = Nothing
		Dim sql As String

		sql = "Select LO.Permission, ISNull( (Select Top 1 TB.[Bez_D] From tbl_base_Bewilligung TB Where TB.RecValue = LO.Permission ), '') CodeLabel "
		sql &= "From LO "
		sql &= "Where "
		sql &= "LO.MDNr = @MDNr "
		sql &= "And LO.Jahr = @Year "
		sql &= "And (LO.LP Between @MonthFrom And @MonthTo) "
		sql &= "And (IsNull(@skanton, '') = '' OR LO.S_Kanton = @skanton) "
		sql &= String.Format("AND ISNULL(LO.Q_Steuer, '') IN ({0}) ", qstCodes)
		sql &= "AND IsNull(LO.Permission, '') <> '' "
		sql &= "Group By LO.Permission Order By LO.Permission"


		Dim listOfParams As New List(Of SqlClient.SqlParameter)
		listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(m_InitializationData.MDData.MDNr, DBNull.Value)))
		listOfParams.Add(New SqlClient.SqlParameter("Year", ReplaceMissing(jahr, m_InitializationData.MDData.MDYear)))
		listOfParams.Add(New SqlClient.SqlParameter("MonthFrom", ReplaceMissing(mvon, Now.Month)))
		listOfParams.Add(New SqlClient.SqlParameter("MonthTo", ReplaceMissing(mbis, mvon)))

		listOfParams.Add(New SqlClient.SqlParameter("skanton", ReplaceMissing(canton, DBNull.Value)))
		'listOfParams.Add(New SqlClient.SqlParameter("qstCode", ReplaceMissing(qstCodes, String.Empty)))


		Dim reader = m_utility.OpenReader(m_InitializationData.MDData.MDDbConn, sql, listOfParams, CommandType.Text)

		result = New List(Of CodeData)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of CodeData)

				While reader.Read()
					Dim data = New CodeData
					data.Code = m_utility.SafeGetString(reader, "Permission")
					data.CodeLabel = m_utility.SafeGetString(reader, "CodeLabel")

					result.Add(data)

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

	Private Function DropQSTTable() As Boolean
		Dim conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)

		Try
			conn.Open()

			' Eine bestehende Tabelle auf der Datenbank löschen
			Dim cmdCreateTable As SqlCommand = New SqlCommand(String.Format("BEGIN TRY DROP TABLE {0} END TRY BEGIN CATCH END CATCH ", ClsDataDetail.LLTabellennamen), conn)
			cmdCreateTable.ExecuteNonQuery()

			Return True

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			Return False

		End Try

	End Function

	Function GetStartSQLString() As String
		Dim result As String = String.Empty
		Dim sZusatzBez As String = String.Empty
		Dim conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)

		'Filterbedingungen zurücksetzen
		ClsDataDetail.GetFilterBez = ""

		Try
			conn.Open()


			'With frmTest
			Dim manrListe As String = ""
			Dim jahr As String = Date.Now.Year.ToString
			Dim vonMonat As String = "1"
			Dim bisMonat As String = "12"
			'Dim tabellenName As String = String.Format("_QSTListe_{0}", m_InitializationData.UserData.UserNr)

			Dim LPMin As Integer = 1
			Dim LPMax As Integer = 1
			Dim datumVon As DateTime
			Dim datumBis As DateTime
			Dim datumBisPlus As DateTime


			Dim loarten As String = ""
			If Not SearchCriteriums.LANr Is Nothing OrElse SearchCriteriums.LANr.Count = 0 Then
				loarten = String.Join(",", SearchCriteriums.LANr)
			End If
			m_Logger.LogDebug(String.Format("loarten: {0}", loarten))

			Dim kanton As String
			If String.IsNullOrWhiteSpace(SearchCriteriums.Canton) Then
				kanton = String.Empty
			Else
				kanton = SearchCriteriums.Canton
			End If

			Dim gemeinde As String
			If String.IsNullOrWhiteSpace(SearchCriteriums.Community) Then
				gemeinde = String.Empty
			Else
				gemeinde = SearchCriteriums.Community
			End If

			Dim betragNull As Integer = Convert.ToInt32(SearchCriteriums.HideZeroAmount.GetValueOrDefault(False))
			Dim nurErstenES As Integer = Convert.ToInt32(SearchCriteriums.FirstEmployment.GetValueOrDefault(False))
			Dim bWithFranc As Boolean = Not SearchCriteriums.HideFranz.GetValueOrDefault(False)
			Dim bWithJustFranc As Boolean = Not SearchCriteriums.ShowFranz.GetValueOrDefault(False)
			If SearchCriteriums.FirstEmployment.GetValueOrDefault(False) Then ClsDataDetail.GetFilterBez += String.Format(TranslateMyText("Nur den ersten Einsatz als Eintritt{0}"), vbLf)


			' Performance-Test
			Dim tHaupt As Double
			Dim tESBegin As Double
			Dim tESEnd As Double
			Dim tQSTVor As Double
			Dim tQSTNach As Double
			Dim tBrutto As Double
			Dim tStd As Double
			Dim stoppUhr As Stopwatch = New Stopwatch()
			stoppUhr.Reset()
			Dim zHaupt As Integer
			Dim zESBegin As Integer
			Dim zESEnd As Integer
			Dim zQSTVor As Integer
			Dim zQSTNach As Integer
			Dim zBrutto As Integer
			Dim zStd As Integer

			If Not SearchCriteriums.EmployeeNumbers Is Nothing AndAlso SearchCriteriums.EmployeeNumbers.Count > 0 Then manrListe = String.Join(",", SearchCriteriums.EmployeeNumbers)
			jahr = SearchCriteriums.Year
			vonMonat = SearchCriteriums.MonthFrom
			bisMonat = SearchCriteriums.MonthTo

			datumVon = DateTime.Parse(String.Format("01.{0}.{1}", vonMonat, jahr))
			datumBis = DateTime.Parse(String.Format("01.{0}.{1}", bisMonat, jahr))



			'' SELEKTIERTE ZEITPERIODE
			'Dim selVonDt As DateTime = DateTime.Parse(String.Format("01.{0}.{1}", vonMonat, jahr))
			'Dim selBisDt As DateTime = DateTime.Parse(String.Format("01.{0}.{1}", bisMonat, jahr)).AddMonths(1).AddDays(-1)
			'ClsDataDetail.SelPeriodeVon = selVonDt
			'ClsDataDetail.SelPeriodeBis = selBisDt

			' TABELLENNAMEN FÜR DEN SELECT UND LL
			ClsDataDetail.LLTabellennamen = m_QSTTablename

			If Not DropQSTTable() Then
				m_UtilityUi.ShowErrorDialog("Die alten Daten konnten nicht gelöscht werden.")

				Return String.Empty
			End If

			' Wegen Perfomancegründen programmatische Lösung, statt Datenbank.
			'Dim ProcedureName As String = "[Create New Table For QSTListe Deutschland With Mandant]"
			'If Not (CInt(loarten) = 7620 And CInt(jahr) >= 2014) Then
			'	ProcedureName = "[Create New Table For QSTListe With Mandant]"
			'	sSql = String.Format("EXEC {0} {1}, {2}, {3}, '{4}', '{5}', '{6}', '{7}', {8}, {9}, {10}" _
			'											 , ProcedureName, m_InitializationData.MDData.MDNr, jahr, vonMonat, bisMonat, tabellenName, loarten, kanton, gemeinde, betragNull, nurErstenES)

			'Else
			'	sSql = String.Format("EXEC {0} {1}, {2}, {3}, '{4}', '{5}', '{6}', '{7}', {8}, {9}" _
			'											 , ProcedureName, m_InitializationData.MDData.MDNr, jahr, vonMonat, bisMonat, tabellenName, kanton, gemeinde, betragNull, nurErstenES)

			'End If

			' Haupttabelle mit Rohdaten aus der DB holen
			' SELECT
			Dim cmd As SqlCommand = New SqlCommand("", conn)
			cmd.CommandType = CommandType.Text
			Dim cmdTextHaupt As System.Text.StringBuilder = New System.Text.StringBuilder()
			cmdTextHaupt.Append("Select LOL.MANR, LOL.LANR, LA.LALOText, LOL.LP As Monat, LOL.LONR, ")
			cmdTextHaupt.Append("LO.S_Kanton, LOL.QSTGemeinde As S_Gemeinde, ")
			cmdTextHaupt.Append("Convert(datetime, Null, 104) As ESAb, ")
			cmdTextHaupt.Append("Convert(datetime, Null, 104) As ESEnde, ")

			' es address
			cmdTextHaupt.Append("IsNull( (Select TOP 1 KD.Strasse From ES Left Join Kunden KD On ES.KDNr = KD.KDNr Where ")
			cmdTextHaupt.Append("ES.MDNR = @MDNr And ")
			cmdTextHaupt.Append("ES.MANR = LOL.MANr And ")
			cmdTextHaupt.Append("((ES.ES_Ab <= @datumBis And (ES.ES_Ende Is Null Or ES.ES_Ende >= @datumVon)) Or ")
			cmdTextHaupt.Append(" ((Year(ES.ES_Ab) = Year(@datumVon) Or Year(ES.ES_Ende) = Year(@datumVon)))) ")
			cmdTextHaupt.Append("Order By ES.ES_Ab Desc), '') As ESStrasse, ")

			cmdTextHaupt.Append("IsNull( (Select TOP 1 KD.PLZ From ES Left Join Kunden KD On ES.KDNr = KD.KDNr Where ")
			cmdTextHaupt.Append("ES.MDNR = @MDNr And ")
			cmdTextHaupt.Append("ES.MANR = LOL.MANr And ")
			cmdTextHaupt.Append("((ES.ES_Ab <= @datumBis And (ES.ES_Ende Is Null Or ES.ES_Ende >= @datumVon)) Or ")
			cmdTextHaupt.Append(" ((Year(ES.ES_Ab) = Year(@datumVon) Or Year(ES.ES_Ende) = Year(@datumVon)))) ")
			cmdTextHaupt.Append("Order By ES.ES_Ab Desc), '') As ESPLZ, ")

			cmdTextHaupt.Append("IsNull( (Select TOP 1 KD.Ort From ES Left Join Kunden KD On ES.KDNr = KD.KDNr Where ")
			cmdTextHaupt.Append("ES.MDNR = @MDNr And ")
			cmdTextHaupt.Append("ES.MANR = LOL.MANr And ")
			cmdTextHaupt.Append("((ES.ES_Ab <= @datumBis And (ES.ES_Ende Is Null Or ES.ES_Ende >= @datumVon)) Or ")
			cmdTextHaupt.Append(" ((Year(ES.ES_Ab) = Year(@datumVon) Or Year(ES.ES_Ende) = Year(@datumVon)))) ")
			cmdTextHaupt.Append("Order By ES.ES_Ab Desc), '') As ESOrt, ")

			cmdTextHaupt.Append("IsNull( (Select TOP 1 P.Kanton From ES Left Join Kunden KD On ES.KDNr = KD.KDNr ")
			cmdTextHaupt.Append("LEFT JOIN PLZ P ON Replace(Replace(KD.PLZ, ' ', ''), '-', '') = Convert(nvarchar(10), P.PLZ) AND P.Land = 'CH' ")
			cmdTextHaupt.Append("Where ")
			cmdTextHaupt.Append("ES.MDNR = @MDNr And ")
			cmdTextHaupt.Append("ES.MANR = LOL.MANr And ")
			cmdTextHaupt.Append("((ES.ES_Ab <= @datumBis And (ES.ES_Ende Is Null Or ES.ES_Ende >= @datumVon)) Or ")
			cmdTextHaupt.Append(" ((Year(ES.ES_Ab) = Year(@datumVon) Or Year(ES.ES_Ende) = Year(@datumVon)))) ")
			cmdTextHaupt.Append("Order By ES.ES_Ab Desc), '') As ESKanton, ")

			cmdTextHaupt.Append("Convert(INT, IsNull( (Select TOP (1) ES.ESNr From ES Where ")
			cmdTextHaupt.Append("ES.MDNR = @MDNr And ")
			cmdTextHaupt.Append("ES.MANR = LOL.MANr And ")
			cmdTextHaupt.Append("((ES.ES_Ab <= @datumBis And (ES.ES_Ende Is Null Or ES.ES_Ende >= @datumVon)) Or ")
			cmdTextHaupt.Append(" ((Year(ES.ES_Ab) = Year(@datumVon) Or Year(ES.ES_Ende) = Year(@datumVon)))) ")
			cmdTextHaupt.Append("Order By ES.ES_Ab ASC), 0)) As AssignedESNr, ")

			cmdTextHaupt.Append("IsNull( (Select TOP (1) ESL.GAVInfo_String From ESLohn ESL Left Join ES On ESL.ESNr = ES.ESNr Where ")
			cmdTextHaupt.Append("ES.MDNR = @MDNr And ")
			cmdTextHaupt.Append("ES.MANR = LOL.MANr And ")
			cmdTextHaupt.Append("((ES.ES_Ab <= @datumBis And (ES.ES_Ende Is Null Or ES.ES_Ende >= @datumVon)) Or ")
			cmdTextHaupt.Append(" ((Year(ES.ES_Ab) = Year(@datumVon) Or Year(ES.ES_Ende) = Year(@datumVon)))) ")
			cmdTextHaupt.Append("And (ESL.LOVon <= @datumBis OR ESL.LOVon >= @datumVon) ")
			cmdTextHaupt.Append("Order By ESL.ID Desc), '') As GAVInfo, ")

			cmdTextHaupt.Append("Convert(INT, IsNull( (Select TOP (1) ESL.ESLohnNr From ESLohn ESL Left Join ES On ESL.ESNr = ES.ESNr Where ")
			cmdTextHaupt.Append("ES.MDNR = @MDNr And ")
			cmdTextHaupt.Append("ES.MANR = LOL.MANr And ")
			cmdTextHaupt.Append("((ES.ES_Ab <= @datumBis And (ES.ES_Ende Is Null Or ES.ES_Ende >= @datumVon)) Or ")
			cmdTextHaupt.Append(" ((Year(ES.ES_Ab) = Year(@datumVon) Or Year(ES.ES_Ende) = Year(@datumVon)))) ")
			cmdTextHaupt.Append("And (ESL.LOVon <= @datumBis OR ESL.LOVon >= @datumVon) ")
			cmdTextHaupt.Append("Order By ESL.ID Desc), 0)) As AssignedESLohnNr, ")

			cmdTextHaupt.Append("Convert(INT, IsNull( (Select TOP (1) RP.RPNr From RP Where ")
			cmdTextHaupt.Append("RP.MDNR = @MDNr And ")
			cmdTextHaupt.Append("RP.MANR = LOL.MANr And ")
			cmdTextHaupt.Append("((RP.Von <= @datumBis And (RP.Bis >= @datumVon)) Or ")
			cmdTextHaupt.Append(" ((Year(RP.Von) = Year(@datumVon) Or Year(RP.Bis) = Year(@datumVon)))) ")
			cmdTextHaupt.Append("Order By RP.Von Desc), 0)) As AssignedRPNr, ")

			cmdTextHaupt.Append("Convert(DECIMAL, IsNull( (Select TOP (1) RP.RPGAV_StdWeek From RP Where ")
			cmdTextHaupt.Append("RP.MDNR = @MDNr And ")
			cmdTextHaupt.Append("RP.MANR = LOL.MANr And ")
			cmdTextHaupt.Append("((RP.Von <= @datumBis And (RP.Bis >= @datumVon)) Or ")
			cmdTextHaupt.Append(" ((Year(RP.Von) = Year(@datumVon) Or Year(RP.Bis) = Year(@datumVon)))) ")
			cmdTextHaupt.Append("Order By RP.Von Desc), 0)) As RPGAVStdWeek, ")

			cmdTextHaupt.Append("Convert(DECIMAL, IsNull( (Select TOP (1) RP.RPGAV_StdMonth From RP Where ")
			cmdTextHaupt.Append("RP.MDNR = @MDNr And ")
			cmdTextHaupt.Append("RP.MANR = LOL.MANr And ")
			cmdTextHaupt.Append("((RP.Von <= @datumBis And (RP.Bis >= @datumVon)) Or ")
			cmdTextHaupt.Append(" ((Year(RP.Von) = Year(@datumVon) Or Year(RP.Bis) = Year(@datumVon)))) ")
			cmdTextHaupt.Append("Order By RP.Von Desc), 0)) As RPGAVStdMonth, ")

			cmdTextHaupt.Append("IsNull( (Select TOP (1) ES.Dismissalreason From ES Where ")
			cmdTextHaupt.Append("ES.MDNR = @MDNr And ")
			cmdTextHaupt.Append("ES.MANR = LOL.MANr And ")
			cmdTextHaupt.Append("((ES.ES_Ab <= @datumBis And (ES.ES_Ende Is Null Or ES.ES_Ende >= @datumVon)) Or ")
			cmdTextHaupt.Append(" ((Year(ES.ES_Ab) = Year(@datumVon) Or Year(ES.ES_Ende) = Year(@datumVon)))) ")
			cmdTextHaupt.Append("Order By ES.ES_Ab ASC), '') As Dismissalreason, ")


			' employee data
			cmdTextHaupt.Append("MA.Nachname, ")
			cmdTextHaupt.Append("MA.Vorname, ")
			cmdTextHaupt.Append("@vonMonat As VonMonat, ")
			cmdTextHaupt.Append("@bisMonat As BisMonat, ")
			cmdTextHaupt.Append("@jahr As Jahr, ")
			cmdTextHaupt.Append("MA.GebDat, ")
			cmdTextHaupt.Append("MA.AHV_Nr, ")
			cmdTextHaupt.Append("MA.AHV_Nr_New, ")
			cmdTextHaupt.Append("MA.Geschlecht, ")
			cmdTextHaupt.Append("MA.Strasse As MAStrasse, ")
			cmdTextHaupt.Append("MA.PLZ As MAPLZ, ")
			cmdTextHaupt.Append("MA.Ort As MAOrt, ")
			cmdTextHaupt.Append("MA.PLZ + ' ' + MA.Ort As MAPLZOrt, ")
			cmdTextHaupt.Append("MA.Land As MALand, ")
			cmdTextHaupt.Append("MA.Sprache, ")

			cmdTextHaupt.Append("LO.EmployeePartnerRecID, ")
			cmdTextHaupt.Append("LO.EmployeeLOHistoryID, ")
			cmdTextHaupt.Append("LO.Zivilstand, ")
			cmdTextHaupt.Append("LO.AnzahlKinder Kinder, ")
			cmdTextHaupt.Append("LO.Permission Bewillig, ")
			cmdTextHaupt.Append("MAKK.Arbeitspensum, ")
			cmdTextHaupt.Append("@kanton As SelectedKanton, ")
			cmdTextHaupt.Append("@gemeinde As SelectedGemeinde, ")
			cmdTextHaupt.Append("LOL.M_Anz, ")
			cmdTextHaupt.Append("LOL.M_Bas,")
			cmdTextHaupt.Append("LOL.M_Ans,")
			cmdTextHaupt.Append("LOL.M_Btr,")
			cmdTextHaupt.Append("0 As Bruttolohn, ")
			cmdTextHaupt.Append("CASE WHEN LOL.LANR = 7600 OR LOL.LANR = 7620 THEN LO.QSTBasis ELSE 0 END As QSTBasis, ")
			cmdTextHaupt.Append("0 As StdAnz, ")

			cmdTextHaupt.Append("SUBSTRING(LO.QSTTarif, 1, 3) As TarifCode, ")
			cmdTextHaupt.Append("CASE WHEN LOL.LANR = 7600 OR LOL.LANR = 7620 THEN LO.WorkedDays ELSE 0 END As WorkedDays,")
			cmdTextHaupt.Append("CASE WHEN IsNull(TAB_QSTInfo.MonthStd,180) > 30 THEN 1 ELSE 0 END As ShowStdAnz ")

			' FROM
			cmdTextHaupt.Append("FROM LOL ")
			cmdTextHaupt.Append("LEFT JOIN Mitarbeiter MA ON ")
			cmdTextHaupt.Append("MA.MANR = LOL.MANR ")
			cmdTextHaupt.Append("LEFT JOIN MAKontakt_Komm MAKK ON ")
			cmdTextHaupt.Append("MAKK.MANR = LOL.MANR AND MAKK.MANR = MA.MANr ")
			cmdTextHaupt.Append("INNER JOIN LO ON ")
			cmdTextHaupt.Append("LO.MANR = LOL.MANR AND ")
			cmdTextHaupt.Append("LO.LONR = LOL.LONR ")
			If kanton.Length > 0 Then cmdTextHaupt.Append("AND LO.S_Kanton = @kanton ")

			cmdTextHaupt.Append("LEFT JOIN TAB_QSTInfo ON ")
			cmdTextHaupt.Append("TAB_QSTInfo.SKanton = LO.S_Kanton ")
			cmdTextHaupt.Append("LEFT JOIN LA ON ")
			cmdTextHaupt.Append("LA.LANR = LOL.LANR And ")
			cmdTextHaupt.Append("LA.LAJahr = LOL.Jahr ")

			cmdTextHaupt.Append("LEFT JOIN dbo.tbl_Changed_Employee_PayrollData MP ON ")
			cmdTextHaupt.Append("MP.ID = LO.EmployeeLOHistoryID ")

			' WHERE
			cmdTextHaupt.Append("WHERE ")
			cmdTextHaupt.Append("LOL.MDNr = @MDNr ")


			If Not String.IsNullOrWhiteSpace(manrListe) Then cmdTextHaupt.Append(String.Format("And LOL.MANR In ({0}) ", manrListe))

			If betragNull = 1 Then cmdTextHaupt.Append("And LOL.M_BTR <> 0 ")

			If Not bWithFranc Then cmdTextHaupt.Append("And MA.Land Not In ('FR', 'F') ")
			If Not bWithJustFranc Then cmdTextHaupt.Append("And MA.Land In ('FR', 'F') ")

			If Not SearchCriteriums.CountryList Is Nothing AndAlso SearchCriteriums.CountryList.Count > 0 Then
				Dim countryCodes As String = String.Empty
				For Each itm In SearchCriteriums.CountryList
					countryCodes &= If(String.IsNullOrWhiteSpace(countryCodes), "'", ",'") & itm.ToString.TrimStart & "'"
				Next
				If Not String.IsNullOrWhiteSpace(countryCodes) Then
					cmdTextHaupt.Append(String.Format("And LO.Land In ({0}) ", countryCodes))
				End If
			End If

			If Not SearchCriteriums.NationaliyList Is Nothing AndAlso SearchCriteriums.NationaliyList.Count > 0 Then
				Dim nationalityCodes As String = String.Empty
				For Each itm In SearchCriteriums.CountryList
					nationalityCodes &= If(String.IsNullOrWhiteSpace(nationalityCodes), "'", ",'") & itm.ToString.TrimStart & "'"
				Next
				If Not String.IsNullOrWhiteSpace(nationalityCodes) Then
					cmdTextHaupt.Append(String.Format("And MP.Nationality In ({0}) ", nationalityCodes))
				End If
			End If


			If Not SearchCriteriums.QSTCode Is Nothing AndAlso SearchCriteriums.QSTCode.Count > 0 Then
				Dim qstCodes As String = String.Empty
				For Each itm In SearchCriteriums.QSTCode
					qstCodes &= If(String.IsNullOrWhiteSpace(qstCodes), "'", ",'") & itm.ToString.TrimStart & "'"
				Next
				If Not String.IsNullOrWhiteSpace(qstCodes) Then
					cmdTextHaupt.Append(String.Format("And LO.Q_Steuer In ({0}) ", qstCodes))
				End If
			End If
			If Not SearchCriteriums.Permission Is Nothing AndAlso SearchCriteriums.Permission.Count > 0 Then
				Dim permissionCodes As String = String.Empty ' String.Join(",", SearchCriteriums.Permission)
				For Each itm In SearchCriteriums.Permission
					permissionCodes &= If(String.IsNullOrWhiteSpace(permissionCodes), "'", ",'") & itm.ToString.TrimStart & "'"
				Next
				If Not String.IsNullOrWhiteSpace(permissionCodes) Then
					cmdTextHaupt.Append(String.Format("And LO.Permission In ({0}) ", permissionCodes))
				End If
			End If


			Dim lanr As Integer = If(loarten = "7620", 7620, 0)
			Dim isDeQST As Boolean = (lanr = 7620 AndAlso CInt(jahr) >= 2014)
			' nur ab 2014 sind die QST-Lohnart 7620 gibt nicht mehr normalerweise! für 7620 gibt dann 7600 mit Kombination des Codes.
			If isDeQST Then
				cmdTextHaupt.Append(String.Format("And (LOL.LANR IN (7600, 7620) And LO.Q_Steuer In ('L', 'M', 'N', 'O', 'P', 'G') ) "))
			Else
				cmdTextHaupt.Append(String.Format("And LOL.LANR IN ({0}) ", loarten))
			End If

			If Not String.IsNullOrWhiteSpace(gemeinde) Then cmdTextHaupt.Append("And LOL.QSTGemeinde = @gemeinde ")

			cmdTextHaupt.Append("And LOL.LP Between @vonMonat And @bisMonat ")
			cmdTextHaupt.Append("And LOL.Jahr = @jahr ")

			cmdTextHaupt.Append("ORDER BY Nachname, Vorname, LOL.MANR, Jahr, LOL.LP, LOL.LANR ")

			m_Logger.LogDebug(String.Format("cmdTextHaupt: {0}", cmdTextHaupt.ToString))

			cmd.CommandText = cmdTextHaupt.ToString
			cmd.Parameters.AddWithValue("@MDNr", SearchCriteriums.MDNr)
			cmd.Parameters.AddWithValue("@datumVon", datumVon)
			cmd.Parameters.AddWithValue("@datumBis", datumBis)
			cmd.Parameters.AddWithValue("@vonMonat", Int32.Parse(vonMonat))
			cmd.Parameters.AddWithValue("@bisMonat", Int32.Parse(bisMonat))
			cmd.Parameters.AddWithValue("@jahr", Int32.Parse(jahr))
			cmd.Parameters.AddWithValue("@kanton", kanton)
			cmd.Parameters.AddWithValue("@gemeinde", gemeinde)
			Dim daHaupt As SqlDataAdapter = New SqlDataAdapter(cmd)
			Dim dt As dsQSTListe.HaupttabelleDataTable = New dsQSTListe.HaupttabelleDataTable()

			' Eintritts- und Austrittsdatum (Einsatz-Beginn und -Ende)
			Dim cmdES As SqlCommand = New SqlCommand("", conn)
			Dim cmdTextESAb As String = "Select TOP 1 ES.ES_Ab From ES Where "
			cmdTextESAb += "ES.MDNR = @MDNr And "
			cmdTextESAb += "ES.MANR = @manr And "
			cmdTextESAb += "((ES.ES_Ab <= @datumBis And (ES.ES_Ende Is Null Or ES.ES_Ende >= @datumVon)) Or "
			cmdTextESAb += " (@nurErstenEinsatz = 1 And (Year(ES.ES_Ab) = Year(@datumVon) Or Year(ES.ES_Ende) = Year(@datumVon)))) "
			cmdTextESAb += "Order By manr,ES.ES_Ab ASC "

			m_Logger.LogDebug(String.Format("cmdTextESAb: {0}", cmdTextESAb.ToString))

			' Einsatz-Ende müssen um 1 Monat nach der gesuchten Zeitperiode gesucht werden.
			' Wenn der Einsatz unbefristet ist, so muss das Datum 01.01.9999 gesetzt werden, da ein Null-Wert 
			' der kleinste Wert hat und somit sich am falschen Ende der Liste befindet. (Sprich: ganz unten)
			' [Nicht 31.12.9999, da ein Monat addiert werden muss]
			Dim cmdTextESEnde As String = "Select Top 1 ES_Ende From ( "
			cmdTextESEnde += "Select IsNull(ES.ES_Ende, Convert(DateTime,'01.01.9999',104)) as ES_Ende From ES Where "
			cmdTextESEnde += "ES.MDNR = @MDNr And "
			cmdTextESEnde += "ES.MANR = @manr And "
			cmdTextESEnde += "ES.ES_Ab <= @datumBisPlus And (ES.ES_Ende Is Null Or ES.ES_Ende >= @datumVon) "
			cmdTextESEnde += ") as t "
			cmdTextESEnde += "Order By ES_Ende DESC "

			m_Logger.LogDebug(String.Format("cmdTextESEnde: {0}", cmdTextESEnde.ToString))

			'Dim cmdESArbeitsort As SqlCommand = New SqlCommand("", conn)
			'Dim cmdTextESArbeitsort As String = "Select TOP 1 ES.Arbort From ES Where "
			'cmdTextESArbeitsort += "ES.MDNR = @MDNr And "
			'cmdTextESArbeitsort += "ES.MANR = @manr And "
			'cmdTextESArbeitsort += "((ES.ES_Ab <= @datumBis And (ES.ES_Ende Is Null Or ES.ES_Ende >= @datumVon)) Or "
			'cmdTextESArbeitsort += " (@nurErstenEinsatz = 1 And (Year(ES.ES_Ab) = Year(@datumVon) Or Year(ES.ES_Ende) = Year(@datumVon)))) "
			'cmdTextESArbeitsort += "Order By ES.ES_Ab Desc "

			'm_Logger.LogDebug(String.Format("cmdTextESArbeitsort: {0}", cmdTextESArbeitsort.ToString))


			Dim pESdtMDNr As SqlParameter = New SqlParameter("@MDNr", SqlDbType.Int)
			Dim pESmanrHaupt As SqlParameter = New SqlParameter("@manr", SqlDbType.Int)
			Dim pESdtVon As SqlParameter = New SqlParameter("@datumVon", SqlDbType.DateTime)
			Dim pESdtBis As SqlParameter = New SqlParameter("@datumBis", SqlDbType.DateTime)
			Dim pESdtBisPlus As SqlParameter = New SqlParameter("@datumBisPlus", SqlDbType.DateTime)
			Dim pNurErstenES As SqlParameter = New SqlParameter("@nurErstenEinsatz", SqlDbType.Bit)

			cmdES.Parameters.Add(pESdtMDNr)
			cmdES.Parameters.Add(pNurErstenES)
			cmdES.Parameters.Add(pESmanrHaupt)
			cmdES.Parameters.Add(pESdtVon)
			cmdES.Parameters.Add(pESdtBis)
			cmdES.Parameters.Add(pESdtBisPlus)

			Dim gefunden As Boolean = False
			Dim neuerKandidat As Boolean = True
			Dim zCounter As Integer = 0
			pESdtMDNr.Value = m_InitializationData.MDData.MDNr
			pNurErstenES.Value = nurErstenES

			' Quellensteuer-Abzug des Vormonats abchecken für Ermittlung des Einsatz-Beginns
			' Ausser wenn Datum-Von-Monat bereits Januar ist.
			Dim cmdTextQSTVormonat As String = "SELECT "
			cmdTextQSTVormonat += "(SELECT Count(*) FROM LOL WHERE LOL.MDNr = @MDNr And "
			cmdTextQSTVormonat += "LOL.MANR=LO.MANR And "
			cmdTextQSTVormonat += "LOL.Jahr = LO.Jahr And "
			cmdTextQSTVormonat += "LOL.LP = Month(DateAdd(Month,-1,@datumVon)) And "
			cmdTextQSTVormonat += "LOL.LANR In (7600, 7620) And "
			cmdTextQSTVormonat += "LOL.M_Btr <> 0) As QSTAbzug "
			cmdTextQSTVormonat += "FROM LO "
			cmdTextQSTVormonat += "WHERE LO.MDNr = @MDNr And "
			cmdTextQSTVormonat += "LO.MANR = @manr And "
			cmdTextQSTVormonat += "LO.Jahr= Year(@datumVon) And "
			cmdTextQSTVormonat += "Month(@datumVon) > Month(DateAdd(Month,-1,@datumVon)) And "
			cmdTextQSTVormonat += "LO.LP = Month(DateAdd(Month,-1,@datumVon)) "

			m_Logger.LogDebug(String.Format("cmdTextQSTVormonat: {0}", cmdTextQSTVormonat.ToString))

			Dim cmdQSTVormonat As SqlCommand = New SqlCommand(cmdTextQSTVormonat, conn)

			Dim pQSTVormonatMDNr As SqlParameter = New SqlParameter("@MDNr", SqlDbType.Int)
			Dim pQSTVormonatManr As SqlParameter = New SqlParameter("@manr", SqlDbType.Int)
			Dim pQSTVormonatDtVon As SqlParameter = New SqlParameter("@datumVon", SqlDbType.DateTime)

			cmdQSTVormonat.Parameters.Add(pQSTVormonatMDNr)
			cmdQSTVormonat.Parameters.Add(pQSTVormonatManr)
			cmdQSTVormonat.Parameters.Add(pQSTVormonatDtVon)
			pQSTVormonatMDNr.Value = m_InitializationData.MDData.MDNr

			' Quellensteuer-Abzug des Nachmonats abchecken für Ermittlung des Einsatz-Endes
			' Ausser wenn Datum-Bis-Monat bereits Dezember ist. (LP=12)
			Dim cmdTextQSTNachmonat As String = "SELECT "
			cmdTextQSTNachmonat += "(SELECT Count(*) FROM LOL WHERE LOL.MDNr = @MDNr And "
			cmdTextQSTNachmonat += "LOL.MANR=LO.MANR And "
			cmdTextQSTNachmonat += "LOL.Jahr = LO.Jahr And "
			cmdTextQSTNachmonat += "LOL.LP = Month(DateAdd(Month,+1,@datumBis)) And "
			cmdTextQSTNachmonat += "LOL.LANR In (7600, 7620) And "
			cmdTextQSTNachmonat += "LOL.M_Btr <> 0) As QSTAbzug "
			cmdTextQSTNachmonat += "FROM LO "
			cmdTextQSTNachmonat += "WHERE LO.MDNr = @MDNr And "
			cmdTextQSTNachmonat += "LO.MANR = @manr And "
			cmdTextQSTNachmonat += "LO.Jahr= Year(@datumBis) And "
			cmdTextQSTNachmonat += "Month(@datumBis) < Month(DateAdd(Month,+1,@datumBis)) And "
			cmdTextQSTNachmonat += "LO.LP = Month(DateAdd(Month,+1,@datumBis)) "

			m_Logger.LogDebug(String.Format("cmdTextQSTNachmonat: {0}", cmdTextQSTNachmonat.ToString))

			Dim cmdQSTNachmonat As SqlCommand = New SqlCommand(cmdTextQSTNachmonat, conn)
			Dim pQSTNachmonatMDNr As SqlParameter = New SqlParameter("@MDNr", SqlDbType.Int)
			Dim pQSTNachmonatManr As SqlParameter = New SqlParameter("@manr", SqlDbType.Int)
			Dim pQSTNachmonatDtBis As SqlParameter = New SqlParameter("@datumBis", SqlDbType.DateTime)

			cmdQSTNachmonat.Parameters.Add(pQSTNachmonatMDNr)
			cmdQSTNachmonat.Parameters.Add(pQSTNachmonatManr)
			cmdQSTNachmonat.Parameters.Add(pQSTNachmonatDtBis)
			pQSTNachmonatMDNr.Value = m_InitializationData.MDData.MDNr

			' Bruttolohn
			Dim cmdTextBruttolohn As String = "SELECT IsNull(LOL.M_Btr,0) As BruttoLohn "
			cmdTextBruttolohn += "FROM LOL "
			cmdTextBruttolohn += "WHERE LOL.MDNr = @MDNr And "
			cmdTextBruttolohn += "LOL.LANr = 7000 And "
			cmdTextBruttolohn += "LOL.LP = @lp And "
			cmdTextBruttolohn += "LOL.Jahr = @jahr And "
			cmdTextBruttolohn += "LOL.MANR = @manr "

			m_Logger.LogDebug(String.Format("cmdTextBruttolohn: {0}", cmdTextBruttolohn.ToString))

			Dim cmdBruttolohn As SqlCommand = New SqlCommand(cmdTextBruttolohn, conn)
			cmdBruttolohn.Parameters.AddWithValue("@MDNr", m_InitializationData.MDData.MDNr)
			cmdBruttolohn.Parameters.AddWithValue("@jahr", jahr)

			Dim pLPBrutto As SqlParameter = New SqlParameter("@lp", SqlDbType.Int)
			Dim pManrBrutto As SqlParameter = New SqlParameter("@manr", SqlDbType.Int)

			cmdBruttolohn.Parameters.Add(pLPBrutto)
			cmdBruttolohn.Parameters.Add(pManrBrutto)
			Dim daBrutto As SqlDataAdapter = New SqlDataAdapter(cmdBruttolohn)

			' Stundenanzahl
			Dim cmdTextStundenanzahl As String = "SELECT IsNull(LOL.M_Btr,0) As StdAnz "
			cmdTextStundenanzahl += "FROM LOL "
			cmdTextStundenanzahl += "WHERE LOL.MDNr = @MDNr And "
			cmdTextStundenanzahl += "LOL.LANr = 6990 And "
			cmdTextStundenanzahl += "LOL.LP = @lp And "
			cmdTextStundenanzahl += "LOL.Jahr = @jahr And "
			cmdTextStundenanzahl += "LOL.MANR = @manr "

			m_Logger.LogDebug(String.Format("cmdTextStundenanzahl: {0}", cmdTextStundenanzahl.ToString))

			Dim cmdStundenanzahl As SqlCommand = New SqlCommand(cmdTextStundenanzahl, conn)

			cmdStundenanzahl.Parameters.AddWithValue("@MDNr", m_InitializationData.MDData.MDNr)
			cmdStundenanzahl.Parameters.AddWithValue("@jahr", jahr)

			Dim pLPStdAnz As SqlParameter = New SqlParameter("@lp", SqlDbType.Int)
			Dim pManrStdAnz As SqlParameter = New SqlParameter("@manr", SqlDbType.Int)

			cmdStundenanzahl.Parameters.Add(pLPStdAnz)
			cmdStundenanzahl.Parameters.Add(pManrStdAnz)
			Dim daStdAnz As SqlDataAdapter = New SqlDataAdapter(cmdStundenanzahl)

			' Quellensteuer-Adresse für LL angeben
			ClsDataDetail.LLQSTAdresseKanton = kanton
			ClsDataDetail.LLQSTAdresseGemeinde = gemeinde

			' Haupttabelle füllen
			Dim manr As String = ""
			Dim manrTemp As String = "" ' für nurErstenEinsatz-Routine
			LPMin = 1
			LPMax = 1
			Dim row As DataRow
			Dim absolutZeile As Integer = 0

			stoppUhr.Start()
			If daHaupt.Fill(dt) > 0 Then
				stoppUhr.Stop()
				tHaupt = stoppUhr.ElapsedMilliseconds
				stoppUhr.Reset()
				zHaupt += 1
				For i As Integer = 0 To dt.Rows.Count - 1
					row = dt.Rows(i)
					gefunden = False
					manr = row("MANR").ToString ' Kandidat-Manr festhalten

					' Wenn nur den ersten Einsatz als Eintritt markiert, pro Kandidat zurückstellen
					If manr <> manrTemp Then
						manrTemp = manr
						pNurErstenES.Value = nurErstenES
					End If


					LPMin = Int32.Parse(row("Monat").ToString) ' Erster Monat ist Min
					' Max herausfinden
					LPMax = Int32.Parse(row("Monat").ToString) ' Erster Monat könnte auch der letzte sein

					' Suche nach vorne: Nächster Datensatz überprüfen.
					zCounter = 0
					absolutZeile = 0
					While Not gefunden

						zCounter += 1
						If i + zCounter = dt.Rows.Count Then ' Es gibt keine Datensätze mehr
							zCounter -= 1 ' Pointer um eins zurück
							gefunden = True
						ElseIf manr <> dt.Rows(i + zCounter)("MANR").ToString Then ' Nächster Datensatz hat neuen Kandidat --> neuerKandidat
							zCounter -= 1 ' Pointer um eins zurück
							gefunden = True
							neuerKandidat = True
						ElseIf Int32.Parse(dt.Rows(i + zCounter)("Monat").ToString) > LPMax + 1 Then ' Ein Monat wird übersprungen --> Min/Max gefunden
							zCounter -= 1 ' Pointer um eins zurück
							gefunden = True
						Else
							' Max vom nächsten Datensatz zuweisen
							LPMax = Int32.Parse(dt.Rows(i + zCounter)("Monat").ToString)
						End If

						' Bruttolohn
						' Nur hinzufügen, wenn die Lohnart 7600 oder 7620 ist, andernfalls leer lassen
						If CDec(dt.Rows(i + absolutZeile)("LANR").ToString) = 7600 OrElse CDec(dt.Rows(i + absolutZeile)("LANR").ToString) = 7620 Then
							pLPBrutto.Value = dt.Rows(i + absolutZeile)("Monat")
							pManrBrutto.Value = manr
							If conn.State = ConnectionState.Closed Then conn.Open()
							stoppUhr.Start()
							dt.Rows(i + absolutZeile)("Bruttolohn") = cmdBruttolohn.ExecuteScalar()
							stoppUhr.Stop()
							tBrutto += stoppUhr.ElapsedMilliseconds
							stoppUhr.Reset()
							zBrutto += 1
						End If

						' Stundenanzahl
						' Nur hinzufügen, wenn die Lohnart 7600 oder 7620 ist, andernfalls leer lassen
						If CDec(dt.Rows(i + absolutZeile)("LANR").ToString) = 7600 OrElse CDec(dt.Rows(i + absolutZeile)("LANR").ToString) = 7620 Then
							pLPStdAnz.Value = dt.Rows(i + absolutZeile)("Monat")
							pManrStdAnz.Value = manr
							If conn.State = ConnectionState.Closed Then conn.Open()
							stoppUhr.Start()
							Dim obj As Object = cmdStundenanzahl.ExecuteScalar()
							stoppUhr.Stop()
							tStd += stoppUhr.ElapsedMilliseconds
							stoppUhr.Reset()
							zStd += 1
							If Not obj Is Nothing Then
								dt.Rows(i + absolutZeile)("StdAnz") = obj
							End If
						End If

						absolutZeile += 1

					End While

					' EINSATZ-BEGINN / EINSATZ-ENDE ========================================================================
					' Wenn Min und Max eines Kunden gefunden, so die Einsatzdaten holen
					datumVon = DateTime.Parse(String.Format("01.{0}.{1}", LPMin, jahr))
					datumBis = DateTime.Parse(String.Format("01.{0}.{1}", LPMax, jahr)).AddMonths(1).AddDays(-1)

					' Das DatumPlus (+ 2 Monate) ausser für Dezember
					Dim monatPlus As Integer = 1
					If LPMax < 12 Then
						monatPlus = 2
					End If
					datumBisPlus = DateTime.Parse(String.Format("01.{0}.{1}", LPMax, jahr)).AddMonths(monatPlus).AddDays(-1)

					pESmanrHaupt.Value = manr
					pESdtVon.Value = datumVon
					pESdtBis.Value = datumBis
					pESdtBisPlus.Value = datumBisPlus
					If conn.State = ConnectionState.Closed Then conn.Open()

					' EINTRITTSDATUM (EINSATZ-BEGINN) -----------------------------
					cmdES.CommandText = cmdTextESAb
					stoppUhr.Start()
					Dim esBeginObj As Object = cmdES.ExecuteScalar() ' Einsatzbeginn der gesuchten Zeitperiode
					stoppUhr.Stop()
					tESBegin += stoppUhr.ElapsedMilliseconds
					stoppUhr.Reset()
					zESBegin += 1

					' Lohnabrechnung ohne Einsatz während gesuchten Monats --> Das Von-Datum als Eintrittsdatum setzen
					If esBeginObj Is Nothing Then
						esBeginObj = datumVon
					End If

					' Eintrittsdatum initialisieren
					Dim eintrittDt As DateTime = DirectCast(esBeginObj, DateTime)

					' Da nach Zeitperiode gesucht wird, so kann es sein, dass der erste Monat kein Einsatz hat, aber
					' die folgenden Monate doch. Der Einsatz beginnt in anderen Worte später als die Lohnabrechnungen.
					If Year(eintrittDt) = Int32.Parse(jahr) And Month(eintrittDt) > LPMin Then
						' Das Von-Datum als Eintrittsdatum setzen
						eintrittDt = datumVon
					End If

					' QST-Abzug im Vormonat prüfen
					pQSTVormonatManr.Value = manr
					pQSTVormonatDtVon.Value = datumVon
					stoppUhr.Start()
					esBeginObj = cmdQSTVormonat.ExecuteScalar()
					stoppUhr.Stop()
					tQSTVor += stoppUhr.ElapsedMilliseconds
					stoppUhr.Reset()
					zQSTVor += 1
					' Wenn der erste Einsatz im Jahr markiert
					If nurErstenES = 1 Then
						' Wenn der erste Einsatz nach Lohnabrechnungen anfängt, so ersten Monat der Zeitperiode
						If Year(eintrittDt).ToString = jahr And Month(eintrittDt) > LPMin Then
							eintrittDt = DateTime.Parse(String.Format("01.{0}.{1}", LPMin, jahr))
						End If
						' ACHTUNG: Nur im ersten Lauf Checkbox "Nur der erste Einsatz im Jahr" berücksichtigen.
						pNurErstenES.Value = 0

					Else
						' Es gibt im Vormonat eine Lohnabrechnung
						If Not esBeginObj Is Nothing Then
							' ...mit Quellensteuerabzug
							If CInt(esBeginObj) = 1 Then
								eintrittDt = DateTime.Parse("01.01.1900") ' kein Eintrittsdatum
							Else ' ...ohne Quellensteuerabzug
								' Wenn kein Einsatz vorhanden, oder früher oder später anfängt, so Monats-Beginn-Datum (LPMin)
								If Year(eintrittDt).ToString <> jahr Or Month(eintrittDt) <> LPMin Then
									eintrittDt = DateTime.Parse(String.Format("01.{0}.{1}", LPMin, jahr))
								End If
							End If
						Else ' Es gibt im Vormonat keine Lohnabrechnung
							' Eintrittsdatum angeben
							' Wenn kein Einsatz vorhanden, oder früher oder später anfängt, so Monats-Beginn-Datum (LPMin)
							If Year(eintrittDt).ToString <> jahr Or Month(eintrittDt) <> LPMin Then
								eintrittDt = DateTime.Parse(String.Format("01.{0}.{1}", LPMin, jahr))
							End If
						End If
					End If

					If Year(eintrittDt) > 1900 Then
						' sollte Datetime sein!
						dt.Rows(i)("ESAb") = eintrittDt ' eintrittDt.ToShortDateString
					End If

					' AUSTRITTSDATUM (EINSATZ-ENDE) -----------------------
					'  unbefristeter Einsatz = 01.01.9999 (statt DbNull) [Nicht 31.12.9999 nehmen, da ein Monat addiert wird.]
					cmdES.CommandText = cmdTextESEnde
					stoppUhr.Start()
					Dim esEndeObj As Object = cmdES.ExecuteScalar() ' Einsatz-Ende der gesuchten Zeitperiode
					stoppUhr.Stop()
					tESEnd += stoppUhr.ElapsedMilliseconds
					stoppUhr.Reset()
					zESEnd += 1
					' Gibt es im Nachmonat eine Lohnabrechnung 
					'     mit Quellensteuer-Abzug so kein Austrittsdatum
					'     ohne Quellensteuer-Abzug so Austrittsdatum
					' Gibt es im Nachmonat keine Lohnabrechnung aber der Einsatz geht weiter 
					'     mit Einsatz so kein Austrittsdatum
					'     ohne Einsatz so Austrittsdatum
					' AUSNAHME bildet der Monat 12: Hier wird stets ein Austritt angegeben. Entweder
					'   das Einsatz-Ende-Datum, falls ein Einsatz dann endet, oder Monatsende, falls 
					'   weitere Einsätze im nächsten Jahr vorhanden sind.
					pQSTNachmonatManr.Value = manr
					pQSTNachmonatDtBis.Value = datumBis
					stoppUhr.Start()
					Dim qstNachMonatObj As Object = cmdQSTNachmonat.ExecuteScalar()
					stoppUhr.Stop()
					tQSTNach += stoppUhr.ElapsedMilliseconds
					stoppUhr.Reset()
					zQSTNach += 1

					' Austrittsdatum mit letzen Monat aller Lohnabrechnungen mit QST-Abzug initialisieren
					Dim austrittDt As DateTime = DateTime.Parse(String.Format("01.{0}.{1}", LPMax, jahr)).AddMonths(1).AddDays(-1)

					' Wenn Einsatz-Ende-Datum vorhanden (befristeter Einsatz)
					If Not esEndeObj Is Nothing Then
						Dim einsatzEndeDt As DateTime = DirectCast(esEndeObj, DateTime)
						' Das Einsatz-Ende-Datum nur nehmen, wenn im gleichen Monat wie die letzte quellensteuerpflichtige Lohnabrechnung endet.
						' Oder der Einsatz unbefristet ist (dann ist das Jahr = 9999)
						If einsatzEndeDt.Year = austrittDt.Year And einsatzEndeDt.Month = austrittDt.Month Or einsatzEndeDt.Year = 9999 Then
							austrittDt = DirectCast(esEndeObj, DateTime)
						End If
						' Das gesuchte Zeitperiode-Ende trifft sich genau mit dem Einsatz-Ende
						If ClsDataDetail.SelPeriodeBis.Year = einsatzEndeDt.Year And ClsDataDetail.SelPeriodeBis.Month = einsatzEndeDt.Month Then
							austrittDt = DirectCast(esEndeObj, DateTime)
						End If

					End If

					' Lohnabrechnung im Nachmonat vorhanden
					If Not qstNachMonatObj Is Nothing Then
						' Mit Quellensteuer-Abzug
						If CInt(qstNachMonatObj) = 1 Then
							austrittDt = DateTime.Parse("01.01.9999") ' Kein Austrittsdatum (gleich wie unbefristeter Einsatz)
						Else ' Ohne Quellensteuer-Abzug
							' Wenn der Einsatz-Ende nicht im gleichen Monat endet, so Monatsende von LPMax nehmen
							If Not (austrittDt.Year.ToString = jahr And austrittDt.Month = LPMax) Then
								austrittDt = DateTime.Parse(String.Format("01.{0}.{1}", LPMax, jahr)).AddMonths(1).AddDays(-1)
							End If
						End If
					Else ' Keine Lohnabrechnung
						' Mit Einsatz
						If Not esEndeObj Is Nothing Then
							Dim einsatzEndeDt As DateTime = DirectCast(esEndeObj, DateTime)
							' (Ausnahme)
							' Wenn Zeitperiode LPMax bis Dezember geht und der Einsatz endet nicht im Dezember und weitergeht,
							' so trotzdem Austritt Ende Jahr
							If LPMax = 12 And austrittDt.Year.ToString > jahr Then
								' 15.01.2014: geändert da gewisse Ämter diese nicht richtig fanden!
								'	austrittDt = DateTime.Parse(String.Format("31.12.{0}", jahr))	' Austritt Ende Jahr

								' Wenn der Einsatz-Ende nicht im gleichen Monat endet, sondern vorher, so Monatsende von LPMax nehmen
							ElseIf austrittDt.Year <> 9999 And Not austrittDt.Year.ToString = jahr And austrittDt.Month = LPMax Then
								austrittDt = DateTime.Parse(String.Format("01.{0}.{1}", LPMax, jahr)).AddMonths(1).AddDays(-1)

								' Wenn der Einsatz im nächsten Monat weitergeht, so kein Austrittsdatum
							ElseIf einsatzEndeDt.Year = austrittDt.Year And einsatzEndeDt.Month > austrittDt.Month Or einsatzEndeDt.Year > austrittDt.Year Then
								austrittDt = DateTime.Parse("01.01.9999") ' Kein Austrittsdatum (gleich wie unbefristeter Einsatz)

							End If
						End If
					End If

					If austrittDt.Year < 3000 Then
						' sollte Datetime sein!
						dt.Rows(i + zCounter)("ESEnde") = austrittDt '.ToShortDateString
					End If

					' Um so viele Datensätze vorrücken, wie vorher Anzahl Male nach vorne gesucht wurde
					i += zCounter

				Next

			End If
			ClsDataDetail.QSTListeDataTable = dt
			'Dim test = dt.CreateDataReader

			'Dim cmdCreateTable As SqlCommand = New SqlCommand("", conn)

			'' Die erstellte Tabelle auf die Datenbank erzeugen
			'cmdCreateTable.CommandText = String.Format("Begin Try Drop Table {0} End Try Begin Catch End Catch; SELECT * Into {0} From Dbo.tbl_TaxData", ClsDataDetail.LLTabellennamen)
			''For Each col As DataColumn In dt.Columns
			''	cmdCreateTable.CommandText += String.Format(" {0} {1},", col.ColumnName, col.DataType.Name)
			''Next
			''cmdCreateTable.CommandText = cmdCreateTable.CommandText.Remove(cmdCreateTable.CommandText.Length - 1, 1) ' letztes Komma entfernen
			''cmdCreateTable.CommandText += " )"
			''cmdCreateTable.CommandText = cmdCreateTable.CommandText.Replace("Int32", "Int").Replace("Int16", "Int").Replace("String", "nvarchar(255)").Replace("Decimal", "money")
			'cmdCreateTable.ExecuteNonQuery()
			'conn.Close()

			If DeleteAllCreatedData() Then
				CreateNewTaxData(dt)
			End If

			'' Die erzeugte Tabelle mit der erstellten Tabelle füllen
			'cmdCreateTable.CommandText = String.Format("INSERT INTO {0} VALUES (", ClsDataDetail.LLTabellennamen)
			'For Each col As DataColumn In dt.Columns
			'	Dim typeObj As Object = SqlDbType.Int
			'	Select Case col.DataType.Name.ToUpper
			'		Case "String".ToUpper
			'			typeObj = SqlDbType.NVarChar

			'		Case "DateTime".ToUpper
			'			typeObj = SqlDbType.DateTime

			'		Case "Decimal".ToUpper
			'			typeObj = SqlDbType.Decimal

			'		Case "money".ToUpper
			'			typeObj = SqlDbType.Money

			'	End Select
			'	' CommandText ergänzen
			'	cmdCreateTable.CommandText += String.Format("@{0}, ", col.ColumnName)
			'	'Parameter hinzufügen
			'	Dim p As SqlParameter = New SqlParameter(String.Format("@{0}", col.ColumnName), DirectCast(typeObj, SqlDbType))
			'	cmdCreateTable.Parameters.Add(p)

			'Next
			'cmdCreateTable.CommandText = cmdCreateTable.CommandText.Remove(cmdCreateTable.CommandText.Length - 2, 2) ' letztes Komma entfernen
			'cmdCreateTable.CommandText += ")"

			'' Jeden Datensatz auf der Datenbank übertragen
			'For Each rowToInsert As DataRow In dt.Rows
			'	' Parameter füllen
			'	For Each p As SqlParameter In cmdCreateTable.Parameters
			'		If p.SqlDbType.ToString.ToUpper = "NVARCHAR" Then
			'			p.Value = rowToInsert(p.ParameterName.Replace("@", "")).ToString
			'		Else
			'			p.Value = rowToInsert(p.ParameterName.Replace("@", ""))
			'		End If
			'	Next
			'	' Zeile schreiben
			'	cmdCreateTable.ExecuteNonQuery()
			'Next

			'conn.Close()


			result = LoadTaxQueryString()


		Catch ex As Exception
			m_UtilityUi.ShowErrorDialog(ex.ToString)

			Return String.Empty

		Finally

		End Try

		Return result
	End Function

	Private Function DeleteAllCreatedData() As Boolean
		Dim result As Boolean = True

		result = result AndAlso m_ListingDatabaseAccess.DeleteAllUserTaxData(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr)


		Return result
	End Function

	Private Function CreateNewTaxData(ByVal data As DataTable) As Boolean
		Dim result As Boolean = True

		For Each itm In data.Rows
			Dim itmRowData = itm
			Dim taxData As New TaxListTableData With {.MANR = ParseNullableInteger(GetValueFromdataRow(itmRowData, "MANr"))}

			taxData.UserTableName = String.Format("{0}", m_QSTTablename)

			taxData.Monat = ParseNullableInteger(GetValueFromdataRow(itmRowData, "Monat"))
			taxData.LANR = ParseNullableDecimal(GetValueFromdataRow(itmRowData, "LANR"))
			taxData.LONR = ParseNullableInteger(GetValueFromdataRow(itmRowData, "LONR"))
			taxData.S_Kanton = GetValueFromdataRow(itmRowData, "S_Kanton")
			taxData.S_Gemeinde = GetValueFromdataRow(itmRowData, "S_Gemeinde")

			Dim esAb As Date?
			Dim esBis As Date?
			If Not String.IsNullOrWhiteSpace(GetValueFromdataRow(itmRowData, "ESAb")) Then
				esAb = Convert.ToDateTime(GetValueFromdataRow(itmRowData, "ESAb"))
			Else
				esAb = Nothing
			End If
			If Not String.IsNullOrWhiteSpace(GetValueFromdataRow(itmRowData, "ESEnde")) Then
				esBis = Convert.ToDateTime(GetValueFromdataRow(itmRowData, "ESEnde"))
			Else
				esBis = Nothing
			End If

			taxData.ESAb = esAb ' If(String.IsNullOrWhiteSpace(GetValueFromdataRow(itmRowData, "ESAb")), Nothing, Convert.ToDateTime(GetValueFromdataRow(itmRowData, "ESAb")))
			taxData.ESEnde = esBis ' If(String.IsNullOrWhiteSpace(GetValueFromdataRow(itmRowData, "ESEnde")), Nothing, Convert.ToDateTime(GetValueFromdataRow(itmRowData, "ESEnde")))
			taxData.Nachname = GetValueFromdataRow(itmRowData, "Nachname")
			taxData.Vorname = GetValueFromdataRow(itmRowData, "Vorname")
			taxData.VonMonat = ParseNullableInteger(GetValueFromdataRow(itmRowData, "VonMonat"))
			taxData.BisMonat = ParseNullableInteger(GetValueFromdataRow(itmRowData, "BisMonat"))
			taxData.Jahr = ParseNullableInteger(GetValueFromdataRow(itmRowData, "Jahr"))
			taxData.GebDat = GetValueFromdataRow(itmRowData, "GebDat")
			taxData.AHV_Nr = GetValueFromdataRow(itmRowData, "AHV_Nr")
			taxData.AHV_Nr_New = GetValueFromdataRow(itmRowData, "AHV_Nr_New")
			taxData.Geschlecht = GetValueFromdataRow(itmRowData, "Geschlecht")
			taxData.MAStrasse = GetValueFromdataRow(itmRowData, "MAStrasse")
			taxData.MAPLZ = GetValueFromdataRow(itmRowData, "MAPLZ")
			taxData.MAOrt = GetValueFromdataRow(itmRowData, "MAOrt")
			taxData.MAPLZOrt = GetValueFromdataRow(itmRowData, "MAPLZOrt")
			taxData.MALand = GetValueFromdataRow(itmRowData, "MALand")
			taxData.Zivilstand = GetValueFromdataRow(itmRowData, "Zivilstand")
			taxData.Sprache = GetValueFromdataRow(itmRowData, "Sprache")
			taxData.Kinder = ParseNullableInteger(GetValueFromdataRow(itmRowData, "Kinder"))
			taxData.Bewillig = GetValueFromdataRow(itmRowData, "Bewillig")
			taxData.SelectedKanton = GetValueFromdataRow(itmRowData, "SelectedKanton")
			taxData.SelectedGemeinde = GetValueFromdataRow(itmRowData, "SelectedGemeinde")
			taxData.M_Anz = ParseNullableDecimal(GetValueFromdataRow(itmRowData, "M_Anz"))
			taxData.M_Bas = ParseNullableDecimal(GetValueFromdataRow(itmRowData, "M_Bas"))
			taxData.M_Ans = ParseNullableDecimal(GetValueFromdataRow(itmRowData, "M_Ans"))
			taxData.M_Btr = ParseNullableDecimal(GetValueFromdataRow(itmRowData, "M_Btr"))
			taxData.Bruttolohn = ParseNullableDecimal(GetValueFromdataRow(itmRowData, "Bruttolohn"))
			taxData.QSTBasis = ParseNullableDecimal(GetValueFromdataRow(itmRowData, "QSTBasis"))
			taxData.StdAnz = ParseNullableDecimal(GetValueFromdataRow(itmRowData, "StdAnz"))
			taxData.TarifCode = GetValueFromdataRow(itmRowData, "TarifCode")
			taxData.WorkedDays = ParseNullableInteger(GetValueFromdataRow(itmRowData, "WorkedDays"))
			'If GetValueFromdataRow(itmRowData, "ShowStdAnz") = 1 Then
			'	taxData.ShowStdAnz = True
			'Else
			'	taxData.ShowStdAnz = False
			'End If
			taxData.ShowStdAnz = ParseNullableBoolean(GetValueFromdataRow(itmRowData, "ShowStdAnz"))

			taxData.ESStrasse = GetValueFromdataRow(itmRowData, "ESStrasse")
			taxData.ESPLZ = GetValueFromdataRow(itmRowData, "ESPLZ")
			taxData.ESOrt = GetValueFromdataRow(itmRowData, "ESOrt")
			taxData.ESKanton = GetValueFromdataRow(itmRowData, "ESKanton")
			taxData.AssignedESNr = ParseNullableInteger(GetValueFromdataRow(itmRowData, "AssignedESNr"))
			taxData.AssignedESLohnNr = ParseNullableInteger(GetValueFromdataRow(itmRowData, "AssignedESLohnNr"))
			taxData.AssignedRPNr = ParseNullableInteger(GetValueFromdataRow(itmRowData, "AssignedRPNr"))
			taxData.GAVInfo = GetValueFromdataRow(itmRowData, "GAVInfo")
			taxData.RPGAVStdWeek = Convert.ToDecimal(Val(GetValueFromdataRow(itmRowData, "RPGAVStdWeek")))
			taxData.RPGAVStdMonth = Convert.ToDecimal(Val(GetValueFromdataRow(itmRowData, "RPGAVStdMonth")))
			taxData.Dismissalreason = GetValueFromdataRow(itmRowData, "Dismissalreason")
			taxData.EmployeePartnerRecID = ParseNullableInteger(GetValueFromdataRow(itmRowData, "EmployeePartnerRecID"))
			taxData.EmployeeLOHistoryID = ParseNullableInteger(GetValueFromdataRow(itmRowData, "EmployeeLOHistoryID"))
			taxData.Arbeitspensum = GetValueFromdataRow(itmRowData, "Arbeitspensum")
			taxData.USNr = m_InitializationData.UserData.UserNr
			taxData.MDNr = m_InitializationData.MDData.MDNr

			result = result AndAlso m_ListingDatabaseAccess.AddTaxListDataToTaxData(taxData)


			If Not result Then Exit For

		Next

		Return result
	End Function

	Private Function LoadTaxQueryString() As String
		Dim result As String

		result = "[Load Montly Tax Data For Search In TAX Listing]"


		Return result

	End Function

	'result = "SELECT MANR,"
	'result &= "CONVERT(DECIMAL, LANR) LANR,"
	'result &= "LALOText,"
	'result &= "Monat,"
	'result &= "LONR,"
	'result &= "IsNull( (Select Top (1) MP.TaxCanton From dbo.tbl_Changed_Employee_PayrollData MP Where ID = tblQST.EmployeeLOHistoryID), '') S_Kanton,"
	'result &= "IsNull( (Select Top (1) MP.TaxCommunityLabel From dbo.tbl_Changed_Employee_PayrollData MP Where ID = tblQST.EmployeeLOHistoryID), '') S_Gemeinde,"
	'result &= "IsNull( (Select Top (1) MP.TaxCommunityCode From dbo.tbl_Changed_Employee_PayrollData MP Where ID = tblQST.EmployeeLOHistoryID), 0) TaxCommunityCode,"
	'result &= "IsNull( (Select Top (1) MP.TaxCommunityLabel From dbo.tbl_Changed_Employee_PayrollData MP Where ID = tblQST.EmployeeLOHistoryID), '') TaxCommunityLabel,"
	'result &= "IsNull( (Select Top (1) MP.EmploymentType From dbo.tbl_Changed_Employee_PayrollData MP Where ID = tblQST.EmployeeLOHistoryID), '') EmploymentType,"
	'result &= "IsNull( (Select Top (1) MP.OtherEmploymentType From dbo.tbl_Changed_Employee_PayrollData MP Where ID = tblQST.EmployeeLOHistoryID), '') OtherEmploymentType,"
	'result &= "IsNull( (Select Top (1) MP.TypeofStay From dbo.tbl_Changed_Employee_PayrollData MP Where ID = tblQST.EmployeeLOHistoryID), '') TypeofStay,"
	'result &= "IsNull( (Select Top (1) MP.ForeignCategory From dbo.tbl_Changed_Employee_PayrollData MP Where ID = tblQST.EmployeeLOHistoryID), '') ForeignCategory,"
	'result &= "IsNull( (Select Top (1) MP.SocialInsuranceNumber From dbo.tbl_Changed_Employee_PayrollData MP Where ID = tblQST.EmployeeLOHistoryID), '') SocialInsuranceNumber,"
	'result &= "IsNull( (Select Top (1) MP.CivilState From dbo.tbl_Changed_Employee_PayrollData MP Where ID = tblQST.EmployeeLOHistoryID), '') CivilState,"
	'result &= "Convert(INT, IsNull( (Select Top (1) MP.NumberOfChildren From dbo.tbl_Changed_Employee_PayrollData MP Where ID = tblQST.EmployeeLOHistoryID), 0)) NumberOfChildren,"
	'result &= "IsNull( (Select Top (1) MP.TaxChurchCode From dbo.tbl_Changed_Employee_PayrollData MP Where ID = tblQST.EmployeeLOHistoryID), 'N') TaxChurchCode,"

	'result &= "IsNull( (Select Top (1) P.LastName From dbo.tbl_Employee_Partnership P Where P.ID = tblQST.EmployeePartnerRecID), '') PartnerLastName,"
	'result &= "IsNull( (Select Top (1) P.Firstname From dbo.tbl_Employee_Partnership P Where P.ID = tblQST.EmployeePartnerRecID), '') PartnerFirstname,"
	'result &= "IsNull( (Select Top (1) P.Street From dbo.tbl_Employee_Partnership P Where P.ID = tblQST.EmployeePartnerRecID), '') PartnerStreet,"
	'result &= "IsNull( (Select Top (1) P.Postcode From dbo.tbl_Employee_Partnership P Where P.ID = tblQST.EmployeePartnerRecID), '') PartnerPostcode,"
	'result &= "IsNull( (Select Top (1) P.City From dbo.tbl_Employee_Partnership P Where P.ID = tblQST.EmployeePartnerRecID), '') PartnerCity,"
	'result &= "IsNull( (Select Top (1) P.Country From dbo.tbl_Employee_Partnership P Where P.ID = tblQST.EmployeePartnerRecID), '') PartnerCountry,"
	'result &= "Convert(BIT, IsNull( (Select Top (1) P.InEmployment From dbo.tbl_Employee_Partnership P Where P.ID = tblQST.EmployeePartnerRecID), 0)) InEmployment,"


	'result &= "CONVERT(DATETIME, ESAb) ESAb,"
	'result &= "CONVERT(DATETIME, ESEnde) ESEnde,"
	'result &= "Nachname,"
	'result &= "Vorname,"
	'result &= "VonMonat,"
	'result &= "BisMonat,"
	'result &= "Jahr,"
	'result &= "GebDat,"
	'result &= "AHV_Nr,"
	'result &= "AHV_Nr_New,"
	'result &= "Geschlecht,"
	'result &= "MAStrasse,"
	'result &= "MAPLZ,"
	'result &= "MAOrt,"
	'result &= "MAPLZOrt,"
	'result &= "MALand,"
	'result &= "Zivilstand,"
	'result &= "Sprache,"
	'result &= "Kinder,"
	'result &= "Bewillig,"
	'result &= "SelectedKanton,"
	'result &= "SelectedGemeinde,"
	'result &= "M_Anz,"
	'result &= "M_Bas,"
	'result &= "M_Ans,"
	'result &= "M_Btr,"
	'result &= "Bruttolohn,"
	'result &= "QSTBasis,"
	'result &= "StdAnz,"
	'result &= "TarifCode,"
	'result &= "WorkedDays,"
	'result &= "ShowStdAnz"

	'result &= ",ESStrasse"
	'result &= ",ESPLZ"
	'result &= ",ESOrt"
	'result &= ",ESKanton"
	'result &= ",AssignedESNr"
	'result &= ",AssignedESLohnNr"
	'result &= ",AssignedRPNr"
	'result &= ",RPGAVStdWeek"
	'result &= ",RPGAVStdMonth"
	'result &= ",Arbeitspensum"
	'result &= ",GAVInfo"
	'result &= ",Dismissalreason"

	'result &= ",EmployeePartnerRecID "
	'result &= ",EmployeeLOHistoryID "

	'result &= "FROM {0} tblQST "
	'result &= "Order By Nachname, Vorname, Monat"

	'result = String.Format(result, m_QSTTablename)

	Private Function GetValueFromdataRow(ByVal dataRow As DataRow, ByVal column As String) As Object

		If Not dataRow.IsNull(column) Then
			Dim value As Object = dataRow(column)
			Return value
		End If

		Return Nothing
	End Function

	Private Function ParseNullableInteger(ByVal str As String) As Integer?

		If String.IsNullOrEmpty(str) Then
			Return Nothing
		End If

		Return Integer.Parse(str)

	End Function

	Private Function ParseNullableDecimal(ByVal str As String) As Decimal?

		If String.IsNullOrEmpty(str) Then
			Return Nothing
		End If

		Return Decimal.Parse(str)

	End Function

	Private Function ParseNullableBoolean(ByVal str As String) As Boolean?

		If String.IsNullOrEmpty(str) Then
			Return Nothing
		End If
		If Val(str) = 1 OrElse str.ToLower = "true" Then Return True Else Return False

		'Return Boolean.Parse(str)

	End Function

	'Sub UpdateMitarbeiterRow(ByRef actualRow As DataRow, ByVal rowWithUpdates As DataRow)
	'	actualRow("Nachname") = rowWithUpdates("Nachname")
	'	actualRow("Vorname") = rowWithUpdates("Vorname")
	'	actualRow("VonMonat") = rowWithUpdates("VonMonat")
	'	actualRow("BisMonat") = rowWithUpdates("BisMonat")
	'	actualRow("Jahr") = rowWithUpdates("Jahr")
	'	actualRow("GebDat") = rowWithUpdates("GebDat")
	'	actualRow("AHV_Nr") = rowWithUpdates("AHV_Nr")
	'	actualRow("AHV_Nr_New") = rowWithUpdates("AHV_Nr_New")
	'	actualRow("MAStrasse") = rowWithUpdates("Strasse")
	'	actualRow("MAPLZ") = rowWithUpdates("PLZ")
	'	actualRow("MAOrt") = rowWithUpdates("Ort")
	'	actualRow("MALand") = rowWithUpdates("Land")
	'	actualRow("Geschlecht") = rowWithUpdates("Geschlecht")
	'	actualRow("Zivilstand") = rowWithUpdates("Zivilstand")
	'	actualRow("Sprache") = rowWithUpdates("Sprache")
	'	actualRow("Kinder") = rowWithUpdates("Kinder")
	'	actualRow("Bewillig") = rowWithUpdates("Bewillig")
	'End Sub


	'Function GetLstItems(ByVal lst As ListBox) As String
	'	Dim strBerufItems As String = String.Empty

	'	For i = 0 To lst.Items.Count - 1
	'		strBerufItems += lst.Items(i).ToString & "#@"
	'	Next

	'	Return Left(strBerufItems, Len(strBerufItems) - 2)
	'End Function

#End Region

End Class
