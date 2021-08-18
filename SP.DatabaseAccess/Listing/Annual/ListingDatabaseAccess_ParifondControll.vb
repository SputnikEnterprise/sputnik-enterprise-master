
Imports SP.DatabaseAccess.Listing.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language
'Imports System.Text
'Imports System.IO



Namespace Listing


	Partial Class ListingDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IListingDatabaseAccess


		Function LoadESListData(ByVal tableName As String, ByVal reportNumbers As List(Of Integer)) As IEnumerable(Of ListingESListData) Implements IListingDatabaseAccess.LoadESListData
			Dim result As List(Of ListingESListData) = Nothing

			Dim SQL As String
			Dim numbersBuffer As String = String.Empty

			For Each number In reportNumbers
				numbersBuffer = numbersBuffer & IIf(numbersBuffer <> "", ", ", "") & number
			Next

			SQL = "SELECT "
			SQL &= "ES.ESNr "
			SQL &= ",ES.KDNr"
			SQL &= ",ES.MANr"
			SQL &= ",es.AHV_Nr_New"
			SQL &= ",ES.Nachname"
			SQL &= ",es.vorname"
			SQL &= ",ES.GebDat"
			SQL &= ",es.MABeruf"
			SQL &= ",es.Filiale1"
			SQL &= ",es.Firma1"
			SQL &= ",'' UID"
			SQL &= ",'' NogaCode"
			SQL &= ",(SELECT TOP 1 Ort FROM Kunden K WHERE K.KDNr = ES.KDNr) KDOrt  "
			SQL &= ",(SELECT TOP 1 Arbort FROM ES E WHERE E.ESNr = ES.ESNr) ESOrt  "
			SQL &= ",ES.ES_Ab"
			SQL &= ",es.ES_Ende"
			SQL &= ",ES.GAVNumber"
			SQL &= ",ES.GAVGruppe0"
			SQL &= ",es.ES_Als"
			SQL &= ",(SELECT TOP 1 Einstufung FROM ES E WHERE E.ESNr = ES.ESNr) Einstufung"
			SQL &= ",es.GAVBezeichnung"
			SQL &= ",es.GAVInfo_String"
			SQL &= ",es.Grundlohn"
			SQL &= ",es.Ferien"
			SQL &= ",es.FerienProz"
			SQL &= ",es.Feier"
			SQL &= ",es.FeierProz"
			SQL &= ",es.Lohn13"
			SQL &= ",es.Lohn13Proz"
			SQL &= ",(ISNULL(es.Stundenlohn, 0) + ISNULL(es.MAStdSpesen, 0)) Bruttolohn"
			SQL &= ",ISNULL((SELECT SUM(M_Betrag) FROM dbo.RPL LEFT JOIN LA ON LA.LANr = RPL.LANr AND LA.LAJahr = YEAR(RPL.VonDate) AND LA.DefineAsSpesenInParifondList = 1 AND LA.LADeactivated = 0 "
			SQL &= " WHERE LA.DefineAsSpesenInParifondList = 1"
			SQL &= String.Format(" AND RPL.ESNr = ES.ESNr  And RPL.RPNr In ({0}) AND KD = 0), 0) TotalBetragSpesen", numbersBuffer)
			SQL &= String.Format(",ISNULL((SELECT SUM(R.KumulativStd) FROM dbo.RPL_MA_Day R WHERE R.ESNr = ES.ESNr And R.RPNr In ({0}) ), 0) KumulierteStundenEinsatz", numbersBuffer)
			SQL &= String.Format(" FROM dbo.{0} ES", tableName)

			SQL &= " ORDER BY es.Nachname, es.vorname"

			Dim reader As SqlClient.SqlDataReader = OpenReader(SQL, Nothing, CommandType.Text)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ListingESListData)

					While reader.Read()
						Dim overviewData As New ListingESListData

						overviewData.ESNr = SafeGetInteger(reader, "ESNr", 0)
						overviewData.KDNr = SafeGetInteger(reader, "KDNr", 0)
						overviewData.MANr = SafeGetInteger(reader, "MANr", 0)

						overviewData.ahv_nr = SafeGetString(reader, "AHV_Nr_New")
						overviewData.employeelastname = SafeGetString(reader, "Nachname")
						overviewData.employeefirstname = SafeGetString(reader, "Vorname")

						overviewData.customername = SafeGetString(reader, "Firma1")
						overviewData.GebDat = SafeGetDateTime(reader, "GebDat", Nothing)
						overviewData.ES_Ab = SafeGetDateTime(reader, "ES_Ab", Nothing)
						overviewData.ES_Ende = SafeGetDateTime(reader, "ES_Ende", Nothing)

						overviewData.MABeruf = SafeGetString(reader, "MABeruf")
						overviewData.Filiale = SafeGetString(reader, "Filiale1")
						overviewData.UID = SafeGetString(reader, "UID")
						overviewData.NogaCode = SafeGetString(reader, "NogaCode")
						overviewData.KDOrt = SafeGetString(reader, "KDOrt")

						overviewData.ESOrt = SafeGetString(reader, "ESOrt")
						overviewData.GAVNumber = SafeGetInteger(reader, "GAVNumber", 0)
						overviewData.GAVGruppe0 = SafeGetString(reader, "GAVGruppe0")
						overviewData.ES_Als = SafeGetString(reader, "ES_Als")
						overviewData.Einstufung = SafeGetString(reader, "Einstufung")
						overviewData.GAVBezeichnung = SafeGetString(reader, "GAVBezeichnung")
						overviewData.GAVInfo_String = SafeGetString(reader, "GAVInfo_String")

						overviewData.Grundlohn = SafeGetDecimal(reader, "Grundlohn", 0)
						overviewData.Ferien = SafeGetDecimal(reader, "Ferien", 0)
						overviewData.FerienProz = SafeGetDecimal(reader, "FerienProz", 0)
						overviewData.Feier = SafeGetDecimal(reader, "Feier", 0)
						overviewData.FeierProz = SafeGetDecimal(reader, "FeierProz", 0)
						overviewData.Lohn13 = SafeGetDecimal(reader, "Lohn13", 0)
						overviewData.Lohn13Proz = SafeGetDecimal(reader, "Lohn13Proz", 0)

						overviewData.Bruttolohn = SafeGetDecimal(reader, "Bruttolohn", 0)
						overviewData.ESSpesen = SafeGetDecimal(reader, "TotalBetragSpesen", 0)
						overviewData.ESStunden = SafeGetDecimal(reader, "KumulierteStundenEinsatz", 0)


						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)

			End Try

			Return result
		End Function



	End Class


End Namespace
