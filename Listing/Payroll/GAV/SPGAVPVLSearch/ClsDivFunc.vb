
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

Imports SPGAVPVLSearch.ClsDataDetail


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
		Dim sql As String = String.Empty
		Dim tabellenName As String = String.Format("_SonstigePVLBerufe_{0}", m_InitialData.UserData.UserNr)

		ClsDataDetail.LLTabellennamen = tabellenName

		Try
			'If m_SearchCriteria.FirstYear <= 2016 Then
			'	sql = "[Create New Table For PVL GAVListe With Mandant]"
			'Else
			sql = "[Create New Table For PVL GAVListe Year 2017 With Mandant]"
			'End If

			'@MDNr int = 0,
			'@jahr Int = 2012,
			'@vonMonat Int = 1,
			'@bisMonat Int = 12,
			'@gavBeruf nvarchar(255) = '',
			'@gavKanton nvarchar(2) = '',
			'@tblName nvarchar(30) = '',
			'@MANRList nvarchar(1000) = ''

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", m_InitialData.MDData.MDNr))
			listOfParams.Add(New SqlClient.SqlParameter("jahr", ReplaceMissing(m_SearchCriteria.FirstYear, Now.Year)))

			listOfParams.Add(New SqlClient.SqlParameter("vonMonat", ReplaceMissing(m_SearchCriteria.FirstMonth, Now.Month)))
			listOfParams.Add(New SqlClient.SqlParameter("bisMonat", ReplaceMissing(m_SearchCriteria.LastMonth, Now.Month)))

			listOfParams.Add(New SqlClient.SqlParameter("gavBeruf", ReplaceMissing(m_SearchCriteria.beruf, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("gavKanton", ReplaceMissing(m_SearchCriteria.kanton, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("tblName", ReplaceMissing(tabellenName, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("MANRList", ReplaceMissing(m_SearchCriteria.MANrList, String.Empty)))

			Dim result As Boolean = m_utility.ExecuteNonQuery(m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			If result Then
				sql = String.Format("Select * From {0} ORDER BY Nachname, Vorname", tabellenName)
				m_SearchCriteria.sqlsearchstring = sql
				ClsDataDetail.LLTabellennamen = tabellenName

			Else
				Throw New Exception(String.Format("{0} >>> Parameters: {1} Fehler in der Abfrage.", sql,
																					String.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}",
																												", ", listOfParams(0).Value, listOfParams(1).Value, listOfParams(2).Value, listOfParams(3).Value,
																												listOfParams(4).Value, listOfParams(5).Value, listOfParams(6).Value, listOfParams(7).Value)))

			End If

		Catch ex As Exception
			m_Logger.LogError(ex.Message)

			Return String.Empty

		End Try

		Return sql

	End Function


#End Region


End Class

