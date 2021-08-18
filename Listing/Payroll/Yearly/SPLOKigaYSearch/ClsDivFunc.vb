

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

Imports SPLOKigaYSearch.ClsDataDetail


Public Class ClsDbFunc

#Region "Private Fields"

	Protected Shared m_Logger As ILogger = New Logger()

	Private m_md As Mandant

	Private m_common As CommonSetting
	Protected m_utility As Utilities
	Protected m_UtilityUi As SP.Infrastructure.UI.UtilityUI

	Private m_SearchCriteria As SearchCriteria

	Private Property SelectedMonatvon As String
	Private Property SelectedMonatbis As String
	Private Property SelectedJahrvon As String
	Private Property SelectedJahrbis As String

	Private Property SelectedMANr As String


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
		'Dim sSqlLen As Integer = 0
		'Dim sZusatzBez As String = String.Empty
		'Dim i As Integer = 0
		'Dim conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		'ClsDataDetail.SelectedDataTable.Clear()
		'Dim selItem As ClsDataDetail.SelectionItem

		Try

			Dim jahr As Integer = m_SearchCriteria.FirstYear
			Dim filiale As String = m_SearchCriteria.filiale

			'ClsDataDetail.SelectedContainer.Clear()
			ClsDataDetail.LLTablename = String.Format("_KIGAStatistiken_{0}", m_InitialData.UserData.UserNr)

			' JAHR
			'If ClsDataDetail.Param.Jahr.Length > 0 Then
			'	jahr = ClsDataDetail.Param.Jahr
			'	' Filterbezeichnung
			'	selItem = New ClsDataDetail.SelectionItem
			'	selItem.Bezeichnung = ClsDataDetail.SuchkriterienList.Jahr
			'	selItem.Text = jahr
			'	ClsDataDetail.SelectedContainer.Add(selItem)
			'End If

			'' FILIALE
			'If ClsDataDetail.Param.Filiale.Length > 0 Then
			'	filiale = ClsDataDetail.Param.Filiale
			'	' Filterbezeichnung
			'	selItem = New ClsDataDetail.SelectionItem
			'	selItem.Bezeichnung = ClsDataDetail.SuchkriterienList.Filiale
			'	selItem.Text = filiale
			'	ClsDataDetail.SelectedContainer.Add(selItem)
			'End If

			' KIGA STATISTIKEN

			Sql = "[Create New Table For KIGAStatistiken With Mandant]"

			'@MDNr int = 0,
			'@jahr int = 2010,
			'@tblName nvarchar(40) = '',
			'@filiale nvarchar(100) = ''

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", m_InitialData.MDData.MDNr))
			listOfParams.Add(New SqlClient.SqlParameter("jahr", ReplaceMissing(m_SearchCriteria.FirstYear, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("tblName", ReplaceMissing(ClsDataDetail.LLTablename, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("filiale", ReplaceMissing(m_SearchCriteria.filiale, String.Empty)))

			'Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(m_InitialData.MDData.MDDbConn, Sql, listOfParams, CommandType.StoredProcedure)
			Dim result As Boolean = m_utility.ExecuteNonQuery(m_InitialData.MDData.MDDbConn, Sql, listOfParams, CommandType.StoredProcedure)

			If result Then
				Sql = String.Format("Select * From {0} ", ClsDataDetail.LLTablename)
				Sql &= "ORDER BY Jahr ASC"

				m_SearchCriteria.sqlsearchstring = Sql

			Else
				Throw New Exception(String.Format("{0} >>> Parameters: {1} Fehler in der Abfrage.", Sql,
																	String.Format("{1}{0}{2}{0}{3}{0}{4}",
																								", ", listOfParams(0).Value, listOfParams(1).Value, listOfParams(2).Value, listOfParams(3).Value)))

			End If


			'Dim pfiliale As SqlParameter = New SqlParameter("@filiale", SqlDbType.NVarChar, 100)
			'pjahr.Value = jahr
			'ptblName.Value = ClsDataDetail.LLTablename
			'pfiliale.Value = filiale
			'cmd.Parameters.Add(pjahr)
			'cmd.Parameters.Add(ptblName)
			'cmd.Parameters.Add(pfiliale)


			'Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
			'Dim dt As DataTable = New DataTable("KIGASTATISTIKEN")
			'AddHandler dt.RowChanged, New DataRowChangeEventHandler(AddressOf Row_Changed)


			'If da.Fill(dt) > 0 Then
			'	ClsDataDetail.SelectedDataTable = dt

			'End If

			'' Für die Anzeige in der SQL-Abfrage-Reiter 
			'sSql = String.Format("EXEC [Create New Table For KIGAStatistiken] @jahr={0}, @tblName={1}, @filiale='{2}'", _
			'										 jahr, ClsDataDetail.LLTablename, filiale)

			'sSqlLen = Len(sSql)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			Return String.Empty

		Finally

		End Try

		Return Sql

	End Function


	'Private Sub Row_Changed(ByVal sender As Object, ByVal e As DataRowChangeEventArgs)
	'  Try
	'    ' Die Übertragung mit der Datenbank ist abgeschlossen. Jetzt wird das DataTable gefüllt und kann unterbrochen werden.
	'    NotifyMainAllowAbort(True)

	'    _counter += 1
	'    If _counter > _counterMax Then
	'      _counterMax = _counterMax * 2
	'      _counter = 1
	'    End If
	'    NotifyMainProgressBar("Daten werden aufbereitet...", ClsDataDetail.GetProzent(1, _counterMax, _counter))
	'  Catch ex As Threading.ThreadAbortException
	'    ' Der Thread wurde abgebrochen --> Kein Fehler
	'    NotifyMainProgressBar("Thread abgebrochen", 1)
	'  Catch ex As Exception
	'    MessageBox.Show(String.Format("{0}{2}{1}", ex.Message, ex.StackTrace, vbLf), "Row_Changed")
	'  End Try


	'End Sub

	'Function GetLstItems(ByVal lst As ListBox) As String
	'  Dim strItems As String = String.Empty

	'  For i = 0 To lst.Items.Count - 1
	'    strItems += lst.Items(i).ToString & "#@"
	'  Next

	'  Return Left(strItems, Len(strItems) - 2)
	'End Function

#End Region

End Class

