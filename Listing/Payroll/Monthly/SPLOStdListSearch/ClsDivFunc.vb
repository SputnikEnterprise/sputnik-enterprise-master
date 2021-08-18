
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

Imports SPLOStdListSearch.ClsDataDetail



'Public Class ClsDivFunc

'#Region "Diverses"

'	'// Get4What._strModul4What
'	Dim _strModul4What As String
'	Public Property Get4What() As String
'		Get
'			Return _strModul4What
'		End Get
'		Set(ByVal value As String)
'			_strModul4What = value
'		End Set
'	End Property

'	'// Query.GetSearchQuery
'	Dim _strQuery As String
'	Public Property GetSearchQuery() As String
'		Get
'			Return _strQuery
'		End Get
'		Set(ByVal value As String)
'			_strQuery = value
'		End Set
'	End Property

'	'// Query.GetSearchCommand
'	Dim _strCommand As SqlCommand
'	Public Property GetSearchCommand() As SqlCommand
'		Get
'			Return _strCommand
'		End Get
'		Set(ByVal value As SqlCommand)
'			_strCommand = value
'		End Set
'	End Property

'	'// LargerLV
'	Dim _bLargerLV As Boolean
'	Public Property GetLargerLV() As Boolean
'		Get
'			Return _bLargerLV
'		End Get
'		Set(ByVal value As Boolean)
'			_bLargerLV = value
'		End Set
'	End Property

'#End Region

'#Region "Funktionen für LvClick in der Suchmaske..."

'	'// Allgemeiner Zwischenspeicher
'	Dim _strSelektion As String
'	Public Property GetSelektion() As String
'		Get
'			Return _strSelektion
'		End Get
'		Set(ByVal value As String)
'			_strSelektion = value
'		End Set
'	End Property

'	' // ID
'	Dim _strID As String
'	Public Property GetID() As String
'		Get
'			Return _strID
'		End Get
'		Set(ByVal value As String)
'			_strID = value
'		End Set
'	End Property

'	' // KrediNr
'	Dim _strKrediNr As String
'	Public Property GetKrediNr() As String
'		Get
'			Return _strKrediNr
'		End Get
'		Set(ByVal value As String)
'			_strKrediNr = value
'		End Set
'	End Property

'	' // LONr
'	Dim _strLONr As String
'	Public Property GetLONr() As String
'		Get
'			Return _strLONr
'		End Get
'		Set(ByVal value As String)
'			_strLONr = value
'		End Set
'	End Property

'	' // LP
'	Dim _strLP As String
'	Public Property GetLP() As String
'		Get
'			Return _strLP
'		End Get
'		Set(ByVal value As String)
'			_strLP = value
'		End Set
'	End Property

'	' // Jahr 
'	Dim _strJahr As String
'	Public Property GetJahr() As String
'		Get
'			Return _strJahr
'		End Get
'		Set(ByVal value As String)
'			_strJahr = value
'		End Set
'	End Property

'	'// MANr
'	Dim _strMANr As String
'	Public Property GetMANr() As String
'		Get
'			Return _strMANr
'		End Get
'		Set(ByVal value As String)
'			_strMANr = value
'		End Set
'	End Property

'	'// KDNr
'	Dim _strKDNr As String
'	Public Property GetKDNr() As String
'		Get
'			Return _strKDNr
'		End Get
'		Set(ByVal value As String)
'			_strKDNr = value
'		End Set
'	End Property

'	'// Kundennamen
'	Dim _strKDName As String
'	Public Property GetKDName() As String
'		Get
'			Return _strKDName
'		End Get
'		Set(ByVal value As String)
'			_strKDName = value
'		End Set
'	End Property

'	'// Kandidatenname
'	Dim _strMAName As String
'	Public Property GetMAName() As String
'		Get
'			Return _strMAName
'		End Get
'		Set(ByVal value As String)
'			_strMAName = value
'		End Set
'	End Property

'	'// Kandidatenvorname
'	Dim _strMAVorname As String
'	Public Property GetMAVorname() As String
'		Get
'			Return _strMAVorname
'		End Get
'		Set(ByVal value As String)
'			_strMAVorname = value
'		End Set
'	End Property

'	'// GAV-Beruf
'	Dim _strESGAVBeruf As String
'	Public Property GetESGAVBeruf() As String
'		Get
'			Return _strESGAVBeruf
'		End Get
'		Set(ByVal value As String)
'			_strESGAVBeruf = value
'		End Set
'	End Property

'	'// Einsatz als
'	Dim _strESEinsatzAls As String
'	Public Property GetESEinsatzAls() As String
'		Get
'			Return _strESEinsatzAls
'		End Get
'		Set(ByVal value As String)
'			_strESEinsatzAls = value
'		End Set
'	End Property

'	'// Query.GetSearchQuery
'	Dim _strTelNr As String
'	Public Property GetTelNr() As String
'		Get
'			Return _strTelNr
'		End Get
'		Set(ByVal value As String)
'			_strTelNr = value
'		End Set
'	End Property

'#End Region

'#Region "LL_Properties"
'	'// Print.LLDocName
'	Dim _LLDocName As String
'	Public Property LLDocName() As String
'		Get
'			Return _LLDocName
'		End Get
'		Set(ByVal value As String)
'			_LLDocName = value
'		End Set
'	End Property

'	'// Print.LLDocLabel
'	Dim _LLDocLabel As String
'	Public Property LLDocLabel() As String
'		Get
'			Return _LLDocLabel
'		End Get
'		Set(ByVal value As String)
'			_LLDocLabel = value
'		End Set
'	End Property

'	'// Print.LLFontDesent
'	Dim _LLFontDesent As Integer
'	Public Property LLFontDesent() As Integer
'		Get
'			Return _LLFontDesent
'		End Get
'		Set(ByVal value As Integer)
'			_LLFontDesent = value
'		End Set
'	End Property

'	'// Print.LLIncPrv
'	Dim _LLIncPrv As Integer
'	Public Property LLIncPrv() As Integer
'		Get
'			Return _LLIncPrv
'		End Get
'		Set(ByVal value As Integer)
'			_LLIncPrv = value
'		End Set
'	End Property

'	'// Print.LLParamCheck
'	Dim _LLParamCheck As Integer
'	Public Property LLParamCheck() As Integer
'		Get
'			Return _LLParamCheck
'		End Get
'		Set(ByVal value As Integer)
'			_LLParamCheck = value
'		End Set
'	End Property

'	'// Print.LLKonvertName
'	Dim _LLKonvertName As Integer
'	Public Property LLKonvertName() As Integer
'		Get
'			Return _LLKonvertName
'		End Get
'		Set(ByVal value As Integer)
'			_LLKonvertName = value
'		End Set
'	End Property

'	'// Print.LLZoomProz
'	Dim _LLZoomProz As Integer
'	Public Property LLZoomProz() As Integer
'		Get
'			Return _LLZoomProz
'		End Get
'		Set(ByVal value As Integer)
'			_LLZoomProz = value
'		End Set
'	End Property

'	'// Print.LLCopyCount
'	Dim _LLCopyCount As Integer
'	Public Property LLCopyCount() As Integer
'		Get
'			Return _LLCopyCount
'		End Get
'		Set(ByVal value As Integer)
'			_LLCopyCount = value
'		End Set
'	End Property

'	'// Print.LLExportedFilePath
'	Dim _LLExportedFilePath As String
'	Public Property LLExportedFilePath() As String
'		Get
'			Return _LLExportedFilePath
'		End Get
'		Set(ByVal value As String)
'			_LLExportedFilePath = value
'		End Set
'	End Property

'	'// Print.LLExportedFileName
'	Dim _LLExportedFileName As String
'	Public Property LLExportedFileName() As String
'		Get
'			Return _LLExportedFileName
'		End Get
'		Set(ByVal value As String)
'			_LLExportedFileName = value
'		End Set
'	End Property

'	'// Print.LLPrintInDiffColor
'	Dim _LLPrintInDiffColor As Boolean
'	Public Property LLPrintInDiffColor() As Boolean
'		Get
'			Return _LLPrintInDiffColor
'		End Get
'		Set(ByVal value As Boolean)
'			_LLPrintInDiffColor = value
'		End Set
'	End Property

'	'// Print.LLExportfilter
'	Dim _LLExportfilter As String
'	Public Property LLExportfilter() As String
'		Get
'			Return _LLExportfilter
'		End Get
'		Set(ByVal value As String)
'			_LLExportfilter = value
'		End Set
'	End Property

'	'// Print.LLExporterName
'	Dim _LLExporterName As String
'	Public Property LLExporterName() As String
'		Get
'			Return _LLExporterName
'		End Get
'		Set(ByVal value As String)
'			_LLExporterName = value
'		End Set
'	End Property

'	'// Print.LLExporterFileName
'	Dim _LLExporterFileName As String
'	Public Property LLExporterFileName() As String
'		Get
'			Return _LLExporterFileName
'		End Get
'		Set(ByVal value As String)
'			_LLExporterFileName = value
'		End Set
'	End Property

'#End Region

'#Region "US Setting"

'	'// USeMail (= eMail des Personalvermittlers)
'	Dim _USeMail As String
'	Public Property USeMail() As String
'		Get
'			Return _USeMail
'		End Get
'		Set(ByVal value As String)
'			_USeMail = value
'		End Set
'	End Property

'	'// USTelefon (= USTelefon des Personalvermittlers)
'	Dim _USTelefon As String
'	Public Property USTelefon() As String
'		Get
'			Return _USTelefon
'		End Get
'		Set(ByVal value As String)
'			_USTelefon = value
'		End Set
'	End Property

'	'// USTelefax (= USTelefax des Personalvermittlers)
'	Dim _USTelefax As String
'	Public Property USTelefax() As String
'		Get
'			Return _USTelefax
'		End Get
'		Set(ByVal value As String)
'			_USTelefax = value
'		End Set
'	End Property

'	'// USVorname (= USVorname des Personalvermittlers)
'	Dim _USVorname As String
'	Public Property USVorname() As String
'		Get
'			Return _USVorname
'		End Get
'		Set(ByVal value As String)
'			_USVorname = value
'		End Set
'	End Property

'	'// USAnrede (= USAnrede des Personalvermittlers)
'	Dim _USAnrede As String
'	Public Property USAnrede() As String
'		Get
'			Return _USAnrede
'		End Get
'		Set(ByVal value As String)
'			_USAnrede = value
'		End Set
'	End Property

'	'// USNachname (= USNachname des Personalvermittlers)
'	Dim _USNachname As String
'	Public Property USNachname() As String
'		Get
'			Return _USNachname
'		End Get
'		Set(ByVal value As String)
'			_USNachname = value
'		End Set
'	End Property

'	'// USMDName (= MDName des Personalvermittlers)
'	Dim _USMDname As String
'	Public Property USMDname() As String
'		Get
'			Return _USMDname
'		End Get
'		Set(ByVal value As String)
'			_USMDname = value
'		End Set
'	End Property

'	'// MDName2 (= MDName2 des Personalvermittlers)
'	Dim _USMDname2 As String
'	Public Property USMDname2() As String
'		Get
'			Return _USMDname2
'		End Get
'		Set(ByVal value As String)
'			_USMDname2 = value
'		End Set
'	End Property

'	'// MDName3 (= MDName3 des Personalvermittlers)
'	Dim _USMDname3 As String
'	Public Property USMDname3() As String
'		Get
'			Return _USMDname3
'		End Get
'		Set(ByVal value As String)
'			_USMDname3 = value
'		End Set
'	End Property

'	'// USMDPostfach (= MDPostfach des Personalvermittlers)
'	Dim _USMDPostfach As String
'	Public Property USMDPostfach() As String
'		Get
'			Return _USMDPostfach
'		End Get
'		Set(ByVal value As String)
'			_USMDPostfach = value
'		End Set
'	End Property

'	'// USMDStrasse (= MDstrasse des Personalvermittlers)
'	Dim _USMDStrasse As String
'	Public Property USMDStrasse() As String
'		Get
'			Return _USMDStrasse
'		End Get
'		Set(ByVal value As String)
'			_USMDStrasse = value
'		End Set
'	End Property

'	'// USMDOrt (= MDOrt des Personalvermittlers)
'	Dim _USMDOrt As String
'	Public Property USMDOrt() As String
'		Get
'			Return _USMDOrt
'		End Get
'		Set(ByVal value As String)
'			_USMDOrt = value
'		End Set
'	End Property

'	'// USMDPLZ (= MDPLZ des Personalvermittlers)
'	Dim _USMDPlz As String
'	Public Property USMDPlz() As String
'		Get
'			Return _USMDPlz
'		End Get
'		Set(ByVal value As String)
'			_USMDPlz = value
'		End Set
'	End Property

'	'// USMDLand (= MDLand des Personalvermittlers)
'	Dim _USMDLand As String
'	Public Property USMDLand() As String
'		Get
'			Return _USMDLand
'		End Get
'		Set(ByVal value As String)
'			_USMDLand = value
'		End Set
'	End Property

'	'// USMDTelefon (= MDTelefon des Personalvermittlers)
'	Dim _USMDTelefon As String
'	Public Property USMDTelefon() As String
'		Get
'			Return _USMDTelefon
'		End Get
'		Set(ByVal value As String)
'			_USMDTelefon = value
'		End Set
'	End Property

'	'// USMDTelefax (= MDTelefax des Personalvermittlers)
'	Dim _USMDTelefax As String
'	Public Property USMDTelefax() As String
'		Get
'			Return _USMDTelefax
'		End Get
'		Set(ByVal value As String)
'			_USMDTelefax = value
'		End Set
'	End Property

'	'// USMDeMail (= MDeMail des Personalvermittlers)
'	Dim _USMDeMail As String
'	Public Property USMDeMail() As String
'		Get
'			Return _USMDeMail
'		End Get
'		Set(ByVal value As String)
'			_USMDeMail = value
'		End Set
'	End Property

'	'// USMDHomepage (= MDHomepage des Personalvermittlers)
'	Dim _USMDHomepage As String
'	Public Property USMDHomepage() As String
'		Get
'			Return _USMDHomepage
'		End Get
'		Set(ByVal value As String)
'			_USMDHomepage = value
'		End Set
'	End Property

'#End Region

'End Class

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
		Dim sSqlLen As Integer = 0
		Dim sZusatzBez As String = String.Empty
		Dim i As Integer = 0
		Dim conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		ClsDataDetail.SelectedDataTable.Clear()
		Dim selItem As ClsDataDetail.SelectionItem


		' Default-Werte
		Dim manrList As String = ""
		Dim jahrVon As Integer = m_SearchCriteria.FirstYear
		Dim jahrBis As Integer = m_SearchCriteria.LastYear
		Dim monatVon As Integer = m_SearchCriteria.FirstMonth
		Dim monatBis As Integer = m_SearchCriteria.LastMonth
		Dim lohnartenBez As String = ""
		Dim kanton As String = m_SearchCriteria.Kanton
		Dim beruf As String = m_SearchCriteria.Gavberuf
		Dim betragNull As Integer = 1
		ClsDataDetail.SelectedContainer.Clear()
		ClsDataDetail.LLTablename = String.Format("_Arbeitsstunden_{0}", m_InitialData.UserData.UserNr)
		Dim filiale As String = m_SearchCriteria.filiale

		' MANR
		If Not m_SearchCriteria.EmployeeNumbers Is Nothing Then
			For Each manr As String In m_SearchCriteria.EmployeeNumbers.Split(CChar(","))
				manrList += String.Format("{0},", manr)
			Next
			manrList = manrList.Substring(0, manrList.Length - 1)
		End If

		' Filterbezeichnung
		selItem = New ClsDataDetail.SelectionItem
		selItem.Bezeichnung = ClsDataDetail.SuchkriterienList.MANR
		selItem.Text = manrList
		ClsDataDetail.SelectedContainer.Add(selItem)

		Try

			Sql = String.Format("BEGIN TRY DROP TABLE {0} END TRY BEGIN CATCH END CATCH; ", ClsDataDetail.LLTablename)
			Sql &= "SELECT LOL.MDNr, LOL.LONr, LOL.LANR, LOL.MANR, LOL.LP, LOL.M_Btr, Convert(int, LOL.Jahr) As Jahr, MA.Nachname, MA.Vorname, MA.Geschlecht, MA.Nationality, "
			Sql &= "LOL.GAV_Kanton, LOL.GAV_Beruf "
			Sql &= String.Format("Into {0} ", ClsDataDetail.LLTablename)
			Sql += "FROM LOL "
			Sql += "LEFT JOIN Mitarbeiter MA ON "
			Sql += "LOL.MANR = MA.MANR "
			Sql += "WHERE LOL.MDNr = @MDNr And "
			Sql += "(@filiale = '' Or MA.KST In ("
			Sql += " SELECT KST FROM Benutzer WHERE "
			Sql += "  (@filiale = '' Or USFiliale = @filiale) "
			Sql += ")) And "
			If manrList.Length > 0 Then
				Sql += String.Format("LOL.MANR In ({0}) And ", manrList)
			End If
			Sql += "((LOL.Jahr = @jahrVon And LOL.LP >= @monatVon And (@jahrVon <> @jahrBis Or (LOL.LP >= @monatVon "
			Sql += "And LOL.LP <= @monatBis))) Or "
			Sql += "(LOL.Jahr > @jahrVon And LOL.Jahr < @jahrBis) Or "
			Sql += "(LOL.Jahr = @jahrBis And LOL.LP <= @monatBis And (@jahrVon <> @jahrBis Or (LOL.LP >= @monatVon "
			Sql += "And LOL.LP <= @monatBis)))) And "

			Sql += "(@kanton = '' Or LOL.GAV_Kanton = @kanton) And "
			Sql += "(@beruf = '' Or LOL.GAV_Beruf = @beruf) And "

			If Not kanton Is Nothing Or Not beruf Is Nothing Then
				Sql += "( LOL.LANr IN (6989) ) "
			Else
				Sql += "( LOL.LANr IN (6990) ) "
			End If
			Sql += "ORDER BY MA.Nachname ASC, MA.Vorname ASC, LOL.Jahr ASC, LOL.LP ASC, LOL.LANR ASC "


			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", m_InitialData.MDData.MDNr))
			listOfParams.Add(New SqlClient.SqlParameter("jahrvon", ReplaceMissing(jahrVon, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("jahrBis", ReplaceMissing(jahrBis, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("monatVon", ReplaceMissing(monatVon, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("monatBis", ReplaceMissing(monatBis, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("kanton", ReplaceMissing(kanton, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("beruf", ReplaceMissing(beruf, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("filiale", ReplaceMissing(filiale, String.Empty)))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(m_InitialData.MDData.MDDbConn, Sql, listOfParams)

			Sql = String.Format("Select * From {0} ", ClsDataDetail.LLTablename)
			Sql &= "ORDER BY Nachname ASC, Vorname ASC, Jahr ASC, LP ASC, LANR ASC "
			m_SearchCriteria.sqlsearchstring = Sql

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			Return String.Empty

		End Try

		Return Sql

	End Function


	'Function GetLstItems(ByVal lst As ListBox) As String
	'  Dim strBerufItems As String = String.Empty

	'  For i = 0 To lst.Items.Count - 1
	'    strBerufItems += lst.Items(i).ToString & "#@"
	'  Next

	'  Return Left(strBerufItems, Len(strBerufItems) - 2)
	'End Function

	' Private Sub DropMyTableInDb()
	'	Dim conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

	'	Try

	'		conn.Open()
	'		' Eine bestehende Tabelle auf der Datenbank löschen
	'		Dim cmdCreateTable As SqlCommand = New SqlCommand(String.Format("BEGIN TRY DROP TABLE {0} END TRY BEGIN CATCH END CATCH ", _
	'																																		ClsDataDetail.LLTablename), _
	'																																		conn)
	'		cmdCreateTable.ExecuteNonQuery()

	'	Catch ex As Exception

	'	End Try

	'End Sub

	'Private Sub InsertDataTableToDatabase(ByVal TableToInsert As DataTable)
	'	Dim conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

	'	Try
	'		conn.Open()
	'		' Eine bestehende Tabelle auf der Datenbank löschen
	'		'Dim cmdCreateTable As SqlCommand = _
	'		'  New SqlCommand(String.Format("BEGIN TRY DROP TABLE {0} END TRY BEGIN CATCH END CATCH ", ClsDataDetail.LLTablename), conn)
	'		'cmdCreateTable.ExecuteNonQuery()

	'		' Die erstellte Tabelle auf die Datenbank erzeugen
	'		Dim cmdCreateTable As New SqlCommand()
	'		cmdCreateTable.CommandText = String.Format("CREATE TABLE {0} (", ClsDataDetail.LLTablename)
	'		For Each col As DataColumn In TableToInsert.Columns
	'			If col.DataType.Name = "Decimal" Then
	'				cmdCreateTable.CommandText += String.Format(" {0} {1}(18,2),", col.ColumnName, col.DataType.Name)
	'			Else
	'				' Konvertierung String zu nvarchar muss die Anzahl Zeichen genau bestimmt sein.
	'				If col.DataType.Name = "String" Then
	'					Dim max As Integer = 1
	'					For Each row As DataRow In TableToInsert.Rows
	'						If Not IsDBNull(row(col.ColumnName)) Then
	'							If max < row(col.ColumnName).ToString.Length Then
	'								max = row(col.ColumnName).ToString.Length
	'							End If
	'						End If
	'					Next
	'					cmdCreateTable.CommandText += String.Format(" {0} nvarchar ({1}),", col.ColumnName, max)
	'				Else
	'					cmdCreateTable.CommandText += String.Format(" {0} {1},", col.ColumnName, col.DataType.Name)
	'				End If
	'			End If
	'		Next
	'		cmdCreateTable.CommandText = cmdCreateTable.CommandText.Remove(cmdCreateTable.CommandText.Length - 1, 1) ' letztes Komma entfernen
	'		cmdCreateTable.CommandText += " )"
	'		cmdCreateTable.CommandText = cmdCreateTable.CommandText.Replace("Int32", "Int").Replace("Int16", "Int")
	'		cmdCreateTable.Connection = conn
	'		cmdCreateTable.ExecuteNonQuery()

	'		' Die erzeugte Tabelle mit der erstellten Tabelle füllen
	'		cmdCreateTable.CommandText = String.Format("INSERT INTO {0} VALUES (", ClsDataDetail.LLTablename)
	'		For Each col As DataColumn In TableToInsert.Columns
	'			Dim typeObj As Object = SqlDbType.Int
	'			Select Case col.DataType.Name.ToUpper
	'				Case "String".ToUpper
	'					typeObj = SqlDbType.NVarChar
	'				Case "DateTime".ToUpper
	'					typeObj = SqlDbType.DateTime
	'				Case "Decimal".ToUpper
	'					typeObj = SqlDbType.Decimal
	'			End Select
	'			' CommandText ergänzen
	'			cmdCreateTable.CommandText += String.Format("@{0}, ", col.ColumnName)
	'			'Parameter hinzufügen
	'			Dim p As SqlParameter = New SqlParameter(String.Format("@{0}", col.ColumnName), DirectCast(typeObj, SqlDbType))
	'			cmdCreateTable.Parameters.Add(p)

	'		Next
	'		cmdCreateTable.CommandText = cmdCreateTable.CommandText.Remove(cmdCreateTable.CommandText.Length - 2, 2) ' letztes Komma entfernen
	'		cmdCreateTable.CommandText += ")"

	'		' Jeden Datensatz auf der Datenbank übertragen
	'		For Each rowToInsert As DataRow In TableToInsert.Rows
	'			' Parameter füllen
	'			For Each p As SqlParameter In cmdCreateTable.Parameters
	'				If p.SqlDbType.ToString.ToUpper = "NVARCHAR" Then
	'					p.Value = rowToInsert(p.ParameterName.Replace("@", "")).ToString
	'				Else
	'					p.Value = rowToInsert(p.ParameterName.Replace("@", ""))
	'				End If
	'			Next
	'			' Zeile schreiben
	'			cmdCreateTable.ExecuteNonQuery()
	'		Next

	'	Catch ex As SqlException
	'		Dim msg As String = ""
	'		For Each Err As SqlError In ex.Errors
	'			msg += String.Format("{0}: {1}{2}", Err.Number, Err.Message, vbLf)
	'		Next
	'		MessageBox.Show(String.Format("Die Daten konnten nicht ordnungsgemäss auf der Datenbank gespeichert werden.{1}{0}{1}", _
	'																	msg, vbLf), "InsertDataTableToDatabase: SQLException", _
	'																	MessageBoxButtons.OK, MessageBoxIcon.Error)
	'		Throw ex
	'	Catch ex As Exception
	'		MessageBox.Show(String.Format("Folgender Fehler kann nicht behandelt werden: {0}{1}", ex.Message, vbLf), _
	'										"InsertDataTableToDatabase: Exception", _
	'										MessageBoxButtons.OK, MessageBoxIcon.Error)
	'		Throw ex

	'	Finally
	'		conn.Close()
	'	End Try
	'End Sub

#End Region


End Class

'Module MyComboBoxExtensions
'  <Extension()> _
'  Public Function ToItem(ByVal cbo As myCbo) As ComboBoxItem
'    If TypeOf (cbo.SelectedItem) Is ComboBoxItem And cbo.SelectedIndex > -1 Then
'      Return DirectCast(cbo.Items(cbo.SelectedIndex), ComboBoxItem)
'    ElseIf cbo.SelectedIndex > -1 Then
'      Dim item As New ComboBoxItem("", "")
'      item.Text = cbo.Items(cbo.SelectedIndex).ToString
'      item.Value = item.Text
'      Return item
'    Else
'      Dim item As New ComboBoxItem("", "")
'      item.Text = cbo.Text
'      item.Value = cbo.Text
'      Return item
'    End If
'  End Function
'End Module