
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

Imports SPLOGUSearch.ClsDataDetail


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

	'// Query.GetSearchCommand
	Dim _strCommand As SqlCommand
	Public Property GetSearchCommand() As SqlCommand
		Get
			Return _strCommand
		End Get
		Set(ByVal value As SqlCommand)
			_strCommand = value
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

	' // KrediNr
	Dim _strKrediNr As String
	Public Property GetKrediNr() As String
		Get
			Return _strKrediNr
		End Get
		Set(ByVal value As String)
			_strKrediNr = value
		End Set
	End Property

	' // LONr
	Dim _strLONr As String
	Public Property GetLONr() As String
		Get
			Return _strLONr
		End Get
		Set(ByVal value As String)
			_strLONr = value
		End Set
	End Property

	' // LP
	'Dim _strLP As String
	'Public Property GetLP() As String
	'  Get
	'    Return _strLP
	'  End Get
	'  Set(ByVal value As String)
	'    _strLP = value
	'  End Set
	'End Property

	' // Jahr 
	Dim _strJahr As String
	Public Property GetJahr() As String
		Get
			Return _strJahr
		End Get
		Set(ByVal value As String)
			_strJahr = value
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

	'// Kundennamen
	Dim _strKDName As String
	Public Property GetKDName() As String
		Get
			Return _strKDName
		End Get
		Set(ByVal value As String)
			_strKDName = value
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

	'// Print.LLPrintInDiffColor
	Dim _LLPrintInDiffColor As Boolean
	Public Property LLPrintInDiffColor() As Boolean
		Get
			Return _LLPrintInDiffColor
		End Get
		Set(ByVal value As Boolean)
			_LLPrintInDiffColor = value
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

#Region "Private Fields"

	Protected Shared m_Logger As ILogger = New Logger()

	Private m_md As Mandant

	Private m_common As CommonSetting
	Protected m_utility As Utilities
	Protected m_UtilityUi As SP.Infrastructure.UI.UtilityUI

	Private m_SearchCriteria As New SearchCriteria

	Private Property SelectedMonatvon As String
	Private Property SelectedMonatbis As String
	Private Property SelectedJahrvon As String
	Private Property SelectedJahrbis As String

	Private Property SelectedMANr As String


#End Region



#Region "Contructor"

	Public Sub New()

		m_md = New Mandant
		m_utility = New Utilities
		m_UtilityUi = New UtilityUI

	End Sub

#End Region




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

		Dim reader = m_utility.OpenReader(m_InitialData.MDData.MDDbConn, sql, Nothing, CommandType.StoredProcedure)

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

	Function IsNewNetto(ByVal iYear As Integer) As Boolean?
		Dim result As Boolean? = False
		Dim m_utility As New Utilities
		m_md = New Mandant

		Dim sql As String = "Declare @test bit = 0 "
		sql &= "if Exists(Select Top 1 LANr From LA Where LAJahr = @MDYear And LANr = 8000.01 And LADeactivated = 0 And RunFuncBefore = '50)') "
		sql &= "begin "
		sql &= "set @test = 1 "
		sql &= "End "
		sql &= "select @test As IsNewNetto "

		Dim yearParameter As New SqlClient.SqlParameter("MDYear", iYear)
		Dim listOfParams As New List(Of SqlClient.SqlParameter)
		listOfParams.Add(yearParameter)

		Dim reader = m_utility.OpenReader(m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.Text)

		Try

			If (Not reader Is Nothing) Then

				While reader.Read()

					result = CBool(m_utility.SafeGetInteger(reader, "IsNewNetto", 0))

				End While

			End If

		Catch e As Exception
			m_Logger.LogError(e.ToString())
			Return False

		Finally
			m_utility.CloseReader(reader)

		End Try

		Return result

	End Function

	Public Function ExistsLastYearLAInLOL(ByVal mdnr As Integer, ByVal yearTo As Integer, ByVal monthTo As Integer) As Boolean?
		Dim result As Boolean? = False

		Dim sql As String = "[Exists Lastyear Guthaben]"

		Dim listOfParams As New List(Of SqlClient.SqlParameter)
		listOfParams.Add(New SqlClient.SqlParameter("MDNr", mdnr))
		listOfParams.Add(New SqlClient.SqlParameter("jahrBis", m_utility.ReplaceMissing(yearTo, Now.Year)))
		listOfParams.Add(New SqlClient.SqlParameter("monatBis", m_utility.ReplaceMissing(monthTo, Now.Month)))

		Dim reader = m_utility.OpenReader(m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

		Try

			If (Not reader Is Nothing AndAlso reader.Read()) Then
				result = CBool(m_utility.SafeGetInteger(reader, "ExistsLANrRows", 0))
			End If

		Catch e As Exception
			m_Logger.LogError(e.ToString())
			Return False

		Finally
			m_utility.CloseReader(reader)

		End Try

		Return result

	End Function


#Region "Funktionen zur Suche nach Daten..."

	''' <summary>
	''' Dynamische SQL-Query-Zusammenstellung der benötigten Tabellen.
	''' </summary>
	Function GetQuerySQLString(ByVal _searchCriteria As SearchCriteria) As String
		Dim sSql As String = String.Empty
		Dim sSqlLen As Integer = 0
		Dim sZusatzBez As String = String.Empty
		Dim i As Integer = 0
		Dim conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		ClsDataDetail.SelectedDataTable.Clear()
		'Dim selItem As ClsDataDetail.SelectionItem

		Try
			m_SearchCriteria = _searchCriteria

			'With frmTest

			' Default-Werte
			Dim monatBis As String = "12"
			Dim jahrBis As String = Date.Now.Year.ToString
			Dim manrList As String = ""

			ClsDataDetail.SelectedContainer.Clear()
			ClsDataDetail.LLTablename = String.Format("_DiverseGuthaben_{0}", m_InitialData.UserData.UserNr)

			' MONAT BIS
			monatBis = m_SearchCriteria.bismonat
			'' Filterbezeichnung
			'selItem = New ClsDataDetail.SelectionItem
			'selItem.Bezeichnung = m_SearchCriteria.bismonat
			'selItem.Text = monatBis
			'ClsDataDetail.SelectedContainer.Add(selItem)

			' JAHR BIS
			jahrBis = m_SearchCriteria.bisjahr
			'' Filterbezeichnung
			'selItem = New ClsDataDetail.SelectionItem
			'selItem.Bezeichnung = m_SearchCriteria.bisjahr
			'selItem.Text = jahrBis
			'ClsDataDetail.SelectedContainer.Add(selItem)

			' MANR
			manrList = m_SearchCriteria.manr
			'If m_SearchCriteria.manr.Length > 0 Then
			'	For Each manr As String In m_SearchCriteria.manr.Split(CChar(","))
			'		manrList += String.Format("{0},", manr)
			'	Next
			'	manrList = manrList.Substring(0, manrList.Length - 1)

			'	' Filterbezeichnung
			'	selItem = New ClsDataDetail.SelectionItem
			'	selItem.Bezeichnung = m_SearchCriteria.manr
			'	selItem.Text = manrList
			'	ClsDataDetail.SelectedContainer.Add(selItem)
			'End If

			Dim isNewNettoLohn As Boolean? = IsNewNetto(CInt(jahrBis))

			' DIVERSE GUTHABEN
			Dim cmd As SqlCommand = New SqlCommand(sSql, conn)
			If isNewNettoLohn Then
				cmd.CommandText = "[Create New Table For NewNetto Guthaben With Tolerance limit]"
			Else
				cmd.CommandText = "[Create New Table For DiverseGuthaben With Mandant]"

			End If

			cmd.CommandType = CommandType.StoredProcedure
			Dim pmonatBis As SqlParameter = New SqlParameter("@monatBis", SqlDbType.Int, 2)
			Dim pjahrBis As SqlParameter = New SqlParameter("@jahrBis", SqlDbType.Int, 4)
			Dim pManr As SqlParameter = New SqlParameter("@manrListe", SqlDbType.NVarChar, 4000)
			Dim ptblName As SqlParameter = New SqlParameter("@tblName", SqlDbType.NVarChar, 40)
			pmonatBis.Value = monatBis
			pjahrBis.Value = jahrBis
			pManr.Value = manrList
			ptblName.Value = ClsDataDetail.LLTablename

			cmd.Parameters.AddWithValue("@MDNr", ClsDataDetail.m_InitialData.MDData.MDNr)
			cmd.Parameters.AddWithValue("@tolerancelimit", m_SearchCriteria.ToleranceLimit)
			cmd.Parameters.Add(pmonatBis)
			cmd.Parameters.Add(pjahrBis)
			cmd.Parameters.Add(pManr)
			cmd.Parameters.Add(ptblName)

			Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
			Dim dt As DataTable = New DataTable("DIVERSEGUTHABEN")


			If da.Fill(dt) > 0 Then
				ClsDataDetail.SelectedDataTable = dt
			End If

			sSql = String.Format("Select * From {0} ORDER BY MANachname, MAVorname", ClsDataDetail.LLTablename)
			sSqlLen = Len(sSql)


			'End With

		Catch ex As ApplicationException
			' Der Thread wurde während der Erstellung der Tabelle unterbrochen --> Kein Fehler      
		Catch ex As Threading.ThreadAbortException
			' Der Thread wurde abgebrochen --> Kein Fehler
		Catch ex As Exception
			MessageBox.Show(String.Format("{0}{2}{1}", ex.Message, ex.StackTrace, vbLf), "GetQuerySQLString")
		Finally
			conn.Close()
		End Try

		Return sSql


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

	Function GetLstItems(ByVal lst As ListBox) As String
		Dim strItems As String = String.Empty

		For i = 0 To lst.Items.Count - 1
			strItems += lst.Items(i).ToString & "#@"
		Next

		Return Left(strItems, Len(strItems) - 2)
	End Function

#End Region




End Class

