
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

Imports SP.KD.KSTSearch.ClsDataDetail


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


	Function LoadKSTData() As IEnumerable(Of KSTData)
		Dim result As List(Of KSTData) = Nothing

		Dim sql As String

		Dim kdnr As String = m_SearchCriteria.KDNrList
		Dim manr As String = m_SearchCriteria.MANrList
		Dim esnr As String = m_SearchCriteria.ESNrList
		Dim jahr As Integer = m_SearchCriteria.FirstYear
		Dim vonMonat As Integer = m_SearchCriteria.FirstMonth
		Dim bisMonat As Integer = m_SearchCriteria.LastMonth
		Dim tabellenName As String = String.Format("_KDKSTListe_{0}", m_InitialData.UserData.UserNr)

		Try

			Sql = "[Create New Table For KDKstListe With Mandant]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", m_InitialData.MDData.MDNr))

			listOfParams.Add(New SqlClient.SqlParameter("KDNRList", ReplaceMissing(m_SearchCriteria.KDNrList, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("MANRList", ReplaceMissing(m_SearchCriteria.MANrList, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("ESNRList", ReplaceMissing(m_SearchCriteria.ESNrList, String.Empty)))

			listOfParams.Add(New SqlClient.SqlParameter("jahr", ReplaceMissing(m_SearchCriteria.FirstYear, Now.Year)))
			listOfParams.Add(New SqlClient.SqlParameter("vonMonat", ReplaceMissing(m_SearchCriteria.FirstMonth, Now.Month)))
			listOfParams.Add(New SqlClient.SqlParameter("bisMonat", ReplaceMissing(m_SearchCriteria.LastMonth, Now.Month)))

			listOfParams.Add(New SqlClient.SqlParameter("gavBeruf", ReplaceMissing(m_SearchCriteria.beruf, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("tblName", ReplaceMissing(tabellenName, String.Empty)))

			Dim reader As SqlDataReader = m_utility.ExecuteReader

			'Dim reader = m_utility.OpenReader(m_InitialData.MDData.MDDbConn, Sql, listOfParams, CommandType.StoredProcedure)

			If (Not reader Is Nothing) Then

				result = New List(Of KSTData)

				While reader.Read

					Dim KDKstData = New KSTData()
					KDKstData.kstnr = m_utility.SafeGetInteger(reader, "kstnr", 0)
					KDKstData.kstbez = m_utility.SafeGetString(reader, "kstbez")
					KDKstData.esnr = m_utility.SafeGetInteger(reader, "esnr", 0)
					KDKstData.kdnr = m_utility.SafeGetInteger(reader, "kdnr", 0)
					KDKstData.rpnr = m_utility.SafeGetInteger(reader, "rpnr", 0)
					KDKstData.totalstd = m_utility.SafeGetDecimal(reader, "totalstd", 0)

					result.Add(KDKstData)

				End While

			End If


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			result = Nothing

		End Try

		Return result

	End Function


	''' <summary>
	''' Dynamische SQL-Query-Zusammenstellung der benötigten Tabellen.
	''' </summary>
	''' <returns></returns>
	''' <remarks></remarks>
	Function GetStartSQLString() As String
		Dim Sql As String = String.Empty

		Dim kdnr As String = m_SearchCriteria.KDNrList
		Dim manr As String = m_SearchCriteria.MANrList
		Dim esnr As String = m_SearchCriteria.ESNrList
		Dim jahr As Integer = m_SearchCriteria.FirstYear
		Dim vonMonat As Integer = m_SearchCriteria.FirstMonth
		Dim bisMonat As Integer = m_SearchCriteria.LastMonth
		Dim tabellenName As String = String.Format("_KDKSTListe_{0}", m_InitialData.UserData.UserNr)

		Try

			Dim KDkstData = LoadKSTData()
			If KDkstData Is Nothing Then Return String.Empty

			For Each itm In KDkstData


			Next

			Sql = "[Create New Table For KDKstListe With Mandant]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", m_InitialData.MDData.MDNr))

			listOfParams.Add(New SqlClient.SqlParameter("KDNRList", ReplaceMissing(m_SearchCriteria.KDNrList, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("MANRList", ReplaceMissing(m_SearchCriteria.MANrList, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("ESNRList", ReplaceMissing(m_SearchCriteria.ESNrList, String.Empty)))

			listOfParams.Add(New SqlClient.SqlParameter("jahr", ReplaceMissing(m_SearchCriteria.FirstYear, Now.Year)))
			listOfParams.Add(New SqlClient.SqlParameter("vonMonat", ReplaceMissing(m_SearchCriteria.FirstMonth, Now.Month)))
			listOfParams.Add(New SqlClient.SqlParameter("bisMonat", ReplaceMissing(m_SearchCriteria.LastMonth, Now.Month)))

			listOfParams.Add(New SqlClient.SqlParameter("gavBeruf", ReplaceMissing(m_SearchCriteria.beruf, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("tblName", ReplaceMissing(tabellenName, String.Empty)))

			Dim reader As SqlDataReader = m_utility.ExecuteReader

			Dim reader = m_utility.OpenReader(m_InitialData.MDData.MDDbConn, Sql, listOfParams, CommandType.StoredProcedure)

			If (Not reader Is Nothing) Then

				While reader.Read



				End While

			End If

			ClsDataDetail.LLTabellennamen = tabellenName
			m_SearchCriteria.sqlsearchstring = Sql

			'Sql = String.Format("EXEC [Create New Table For FARListe With Mandant] '{0}', {1}, {2}, {3}, '{4}', '{5}'" _
			'										 , manr, jahr, vonMonat, bisMonat, .Cbo_FARListeBeruf.Text, tabellenName)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

			Return String.Empty

		End Try

		Return Sql

	End Function


	Function InsertDataIntoKSTData(ByVal kstdata As List(Of KSTData)) As Boolean
		Dim result As Boolean = True

		Dim kdnr As String = m_SearchCriteria.KDNrList
		Dim manr As String = m_SearchCriteria.MANrList
		Dim esnr As String = m_SearchCriteria.ESNrList
		Dim jahr As Integer = m_SearchCriteria.FirstYear
		Dim vonMonat As Integer = m_SearchCriteria.FirstMonth
		Dim bisMonat As Integer = m_SearchCriteria.LastMonth
		Dim tabellenName As String = String.Format("_KDKSTListe_{0}", m_InitialData.UserData.UserNr)

		Try

			If kstdata Is Nothing Then Return False

			Sql = "[Create New Table For KDKstListe With Mandant]"


			Sql = "[Create New Table For KDKstListe With Mandant]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", m_InitialData.MDData.MDNr))

			listOfParams.Add(New SqlClient.SqlParameter("KDNRList", ReplaceMissing(m_SearchCriteria.KDNrList, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("MANRList", ReplaceMissing(m_SearchCriteria.MANrList, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("ESNRList", ReplaceMissing(m_SearchCriteria.ESNrList, String.Empty)))

			listOfParams.Add(New SqlClient.SqlParameter("jahr", ReplaceMissing(m_SearchCriteria.FirstYear, Now.Year)))
			listOfParams.Add(New SqlClient.SqlParameter("vonMonat", ReplaceMissing(m_SearchCriteria.FirstMonth, Now.Month)))
			listOfParams.Add(New SqlClient.SqlParameter("bisMonat", ReplaceMissing(m_SearchCriteria.LastMonth, Now.Month)))

			listOfParams.Add(New SqlClient.SqlParameter("gavBeruf", ReplaceMissing(m_SearchCriteria.beruf, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("tblName", ReplaceMissing(tabellenName, String.Empty)))

			Dim reader As SqlDataReader = m_utility.ExecuteReader

			Dim reader = m_utility.OpenReader(m_InitialData.MDData.MDDbConn, Sql, listOfParams, CommandType.StoredProcedure)

			If (Not reader Is Nothing) Then

				While reader.Read



				End While

			End If


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			result = False

		End Try

		Return result

	End Function


#End Region


End Class

