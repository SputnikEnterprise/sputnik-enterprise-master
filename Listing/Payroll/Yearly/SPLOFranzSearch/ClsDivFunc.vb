
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging

Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities

Imports SPLOFranzSearch.ClsDataDetail


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
	Function GetQuerySQLString() As String
		Dim Sql As String = String.Empty

		Try

			Dim jahr As Integer = m_SearchCriteria.FirstYear
			Dim manrList As String = m_SearchCriteria.MANrList
			Dim filiale As String = m_SearchCriteria.filiale

			ClsDataDetail.SelectedContainer.Clear()
			ClsDataDetail.LLTablename = String.Format("_FranzGrenzgänger_{0}", m_InitialData.UserData.UserNr)

			Sql = "[Create New Table For FranzGrenzgänger With Mandant]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", m_InitialData.MDData.MDNr))
			listOfParams.Add(New SqlClient.SqlParameter("manrListe", ReplaceMissing(m_SearchCriteria.MANrList, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("jahr", ReplaceMissing(m_SearchCriteria.FirstYear, Now.Year)))
			listOfParams.Add(New SqlClient.SqlParameter("tblName", ReplaceMissing(ClsDataDetail.LLTablename, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("Kanton", ReplaceMissing(m_SearchCriteria.EmployeeCanton, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("S_Kanton", ReplaceMissing(m_SearchCriteria.EmployeeTaxCanton, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("filiale", ReplaceMissing(m_SearchCriteria.filiale, String.Empty)))

			Dim result As Boolean = True
			m_utility.OpenReader(m_InitialData.MDData.MDDbConn, Sql, listOfParams, CommandType.StoredProcedure)
			If result Then
				Sql = String.Format("Select * From {0} ", ClsDataDetail.LLTablename)
				Sql &= "ORDER BY Nachname, Vorname ASC"

				m_SearchCriteria.sqlsearchstring = Sql

			Else
				Throw New Exception(String.Format("{0} >>> Parameters: {1} Fehler in der Abfrage.", Sql,
																	String.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}",
																								", ", listOfParams(0).Value, listOfParams(1).Value, listOfParams(2).Value, listOfParams(3).Value, listOfParams(4).Value)))

			End If


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			Return String.Empty

		Finally

		End Try

		Return Sql

	End Function


#End Region


End Class

