
Imports System.IO
Imports System.Data.SqlClient

Imports System.Text.RegularExpressions
Imports System.Runtime.CompilerServices

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.Utility

Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities

Imports SPFARListeSearch.ClsDataDetail


Public Class ClsDbFunc

#Region "Private Fields"

	Protected Shared m_Logger As ILogger = New Logger()

	Private m_md As Mandant

	Private m_common As CommonSetting
	Protected m_utility As Utilities
	Protected m_UtilityUi As SP.Infrastructure.UI.UtilityUI

	Private m_SearchCriteria As SearchCriteria

#End Region


#Region "Contructor"

	Public Sub New(ByVal _setting As SearchCriteria)

		m_md = New Mandant
		m_utility = New Utilities
		m_UtilityUi = New UtilityUI

		m_SearchCriteria = _setting

	End Sub

#End Region


#Region "Funktionen zur Suche nach Daten..."

	''' <summary>
	''' Dynamische SQL-Query-Zusammenstellung der benötigten Tabellen.
	''' </summary>
	''' <returns></returns>
	''' <remarks></remarks>
	Function GetStartSQLString() As String
		Dim Sql As String = String.Empty

		Dim manr As String = m_SearchCriteria.MANrList
		Dim jahr As Integer = m_SearchCriteria.FirstYear
		Dim vonMonat As Integer = m_SearchCriteria.FirstMonth
		Dim bisMonat As Integer = m_SearchCriteria.LastMonth
		Dim tabellenName As String = String.Format("_FARListe_{0}", m_InitialData.UserData.UserNr)
		tabellenName = "_FARPVLListe"

		Try

			'Sql = "[Create New Table For FARListe With Mandant]"
			Sql = "[Load FAR Data For FAR-Bescheinigung]"

			'@MDNr int = 0,
			'@MANRList nvarchar(1000),
			'@jahr Int = 2009,
			'@vonMonat Int = 1,
			'@bisMonat Int = 12,
			'@gavBeruf nvarchar(255) = '',
			'@tblName nvarchar(255) = ''

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", m_InitialData.MDData.MDNr))
			listOfParams.Add(New SqlClient.SqlParameter("MANRList", ReplaceMissing(m_SearchCriteria.MANrList, String.Empty)))

			listOfParams.Add(New SqlClient.SqlParameter("jahr", ReplaceMissing(m_SearchCriteria.FirstYear, Now.Year)))
			listOfParams.Add(New SqlClient.SqlParameter("vonMonat", ReplaceMissing(m_SearchCriteria.FirstMonth, Now.Month)))
			listOfParams.Add(New SqlClient.SqlParameter("bisMonat", ReplaceMissing(m_SearchCriteria.LastMonth, Now.Month)))

			listOfParams.Add(New SqlClient.SqlParameter("gavBeruf", ReplaceMissing(m_SearchCriteria.beruf, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("tblName", DBNull.Value)) 'ReplaceMissing(tabellenName, String.Empty)))

			Dim result As Boolean = m_utility.ExecuteNonQuery(m_InitialData.MDData.MDDbConn, Sql, listOfParams, CommandType.StoredProcedure, False)

			If result Then
				Sql = "Select MANr, Nachname, Vorname, GebDat, AHV_Nr, AHV_Nr_New "
				Sql &= ",ES_Ab, ES_Ende, FirstESNr, LastESNr, FirstESAs, LastESAs, FirstGAVName, LastGAVName "

				Sql &= ",AbLP, BisLP, VonMonat, BisMonat, Jahr, "
				Sql &= "SUM(AnzahlStd) AnzahlStd, SUM(Lohnsumme) Lohnsumme, SUM(Beitrag) Beitrag, "
				Sql &= "GAV_Fag, GAV_Fan, GAV_VAG_S, GAV_WAG_S, GAV_VAG, GAV_WAG, GAV_VAN_S, GAV_WAN_S, GAV_VAN, GAV_WAN, "
				Sql &= "GAV_Name, GAV_ZHD, GAV_Postfach, GAV_Strasse, GAV_PLZ, GAV_Ort, GAV_Bank, GAV_BankPLZOrt, GAV_Bankkonto, GAV_IBAN, GAVBEruf "
				Sql &= "From {0} FAR "

				If m_SearchCriteria.resor Then
					Dim berufBez As String = String.Empty

					Dim data = LoadLAData(m_SearchCriteria.FirstYear, m_SearchCriteria.FirstMonth, m_SearchCriteria.LastMonth, m_SearchCriteria.resor)
					If Not data Is Nothing AndAlso data.Count > 0 Then
						Sql &= "Where GAV_Beruf In ("
						For Each itm In data
							berufBez &= If(String.IsNullOrWhiteSpace(berufBez), "", ", ") & "'" & itm.gavberuf & "'"
						Next
						Sql &= berufBez & ") "
					Else
						Sql &= "Where GAV_Beruf = '' "
					End If
				End If

				'Else
				'Sql = "Select MANr, Nachname, Vorname, GebDat, AHV_Nr, AHV_Nr_New, AbLP, BisLP, VonMonat, BisMonat, Jahr, SUM(AnzahlStd) AnzahlStd, SUM(Lohnsumme) Lohnsumme, SUM(Beitrag) Beitrag, "
				'Sql &= "GAV_Fag, GAV_Fan, GAV_VAG_S, GAV_WAG_S, GAV_VAG, GAV_WAG, GAV_VAN_S, GAV_WAN_S, GAV_VAN, GAV_WAN, "
				'Sql &= "GAV_Name, GAV_ZHD, GAV_Postfach, GAV_Strasse, GAV_PLZ, GAV_Ort, GAV_Bank, GAV_BankPLZOrt, GAV_Bankkonto, GAV_IBAN, GAVBEruf "
				'Sql &= "From {0} "

				'Sql &= "GROUP BY MANr, Nachname, Vorname, GebDat, AHV_Nr, AHV_Nr_New, AbLP, BisLP, VonMonat, BisMonat, Jahr, "
				'Sql &= "GAV_Fag, GAV_Fan, GAV_VAG_S, GAV_WAG_S, GAV_VAG, GAV_WAG, GAV_VAN_S, GAV_WAN_S, GAV_VAN, GAV_WAN, "
				'Sql &= "GAV_Name, GAV_ZHD, GAV_Postfach, GAV_Strasse, GAV_PLZ, GAV_Ort, GAV_Bank, GAV_BankPLZOrt, GAV_Bankkonto, GAV_IBAN, GAVBEruf "

				'Sql &= "ORDER BY Nachname, Vorname, VonMonat, BisMonat"

				'End If
				Sql &= " GROUP BY "
				Sql &= "MANr, Nachname, Vorname, GebDat, AHV_Nr, AHV_Nr_New"
				Sql &= ",ES_Ab, ES_Ende, FirstESNr, LastESNr, FirstESAs, LastESAs, FirstGAVName, LastGAVName "
				Sql &= ", AbLP, BisLP, VonMonat, BisMonat, Jahr, "
				Sql &= "GAV_Fag, GAV_Fan, GAV_VAG_S, GAV_WAG_S, GAV_VAG, GAV_WAG, GAV_VAN_S, GAV_WAN_S, GAV_VAN, GAV_WAN, "
				Sql &= "GAV_Name, GAV_ZHD, GAV_Postfach, GAV_Strasse, GAV_PLZ, GAV_Ort, GAV_Bank, GAV_BankPLZOrt, GAV_Bankkonto, GAV_IBAN, GAVBEruf "
				Sql &= "ORDER BY Nachname, Vorname, VonMonat, BisMonat"


				Sql = String.Format(Sql, tabellenName)

				m_SearchCriteria.sqlsearchstring = Sql

				ClsDataDetail.LLTabellennamen = tabellenName

			Else
				Throw New Exception(String.Format("{0} >>> Parameters: {1} Fehler in der Abfrage.", Sql,
																					String.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}",
																												", ", listOfParams(0).Value, listOfParams(1).Value, listOfParams(2).Value, listOfParams(3).Value,
																												listOfParams(4).Value, listOfParams(5).Value, listOfParams(6).Value, m_InitialData.MDData.MDDbConn)))
			End If

			'Sql = String.Format("EXEC [Create New Table For FARListe With Mandant] '{0}', {1}, {2}, {3}, '{4}', '{5}'" _
			'										 , manr, jahr, vonMonat, bisMonat, .Cbo_FARListeBeruf.Text, tabellenName)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

			Return String.Empty

		End Try

		Return Sql

	End Function


#End Region


End Class

