
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

Imports SPLOLRJSearch.ClsDataDetail


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

	'Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	'Dim _ClsReg As New SPProgUtility.ClsDivReg

	'Dim strMyDateFormat As String = _ClsProgSetting.GetSQLDateFormat()

	'Public _SynchronizingThread As Threading.Thread
	'Public _SynchronizingMain As System.ComponentModel.ISynchronizeInvoke

	'Public _NotifyMainProgressDelegate As NotifyMainProgressDel
	'Public Delegate Sub NotifyMainProgressDel(ByVal Message As String, ByVal PercentComplete As Integer)
	'Public _NotifyMainAllowAbortDelegate As NotifyMainAllowAbortDel
	'Public Delegate Sub NotifyMainAllowAbortDel(ByVal Abort As Boolean)


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

	'Private Sub NotifyMainProgressBar(ByVal Message As String, ByVal Value As Integer)
	'  Try
	'    If Not _NotifyMainProgressDelegate Is Nothing Then
	'      Dim args(1) As Object
	'      args(0) = Message
	'      args(1) = Value
	'      _SynchronizingMain.Invoke(_NotifyMainProgressDelegate, args)
	'    End If

	'  Catch ex As ObjectDisposedException
	'    ' Das Objekt wurde zerstört --> keine Fehlermeldung
	'  Catch ex As InvalidOperationException
	'    ' Das Delegate wurde nicht erstellt --> keine Fehlermeldung
	'  Catch ex As Threading.ThreadAbortException
	'    ' Der Thread wurde abgebrochen --> Kein Fehler
	'  Catch ex As Exception
	'    MessageBox.Show(String.Format("{0}{2}{1}", ex.Message, ex.StackTrace, vbLf), "NotifyMainProgressBar")
	'  End Try
	'End Sub

	'Private Sub NotifyMainAllowAbort(ByVal Abort As Boolean)
	'  Try
	'    If Not _NotifyMainAllowAbortDelegate Is Nothing Then
	'      Dim args(0) As Object
	'      args(0) = Abort
	'      _SynchronizingMain.Invoke(_NotifyMainAllowAbortDelegate, args)
	'    End If
	'  Catch ex As ObjectDisposedException
	'    ' Das Objekt wurde zerstört --> keine Fehlermeldung
	'  Catch ex As InvalidOperationException
	'    ' Das Delegate wurde nicht erstellt --> keine Fehlermeldung
	'  Catch ex As Threading.ThreadAbortException
	'    ' Der Thread wurde abgebrochen --> Kein Fehler
	'  Catch ex As Exception
	'    MessageBox.Show(String.Format("{0}{2}{1}", ex.Message, ex.StackTrace, vbLf), "NotifyMainProgressBar")

	'  End Try
	'End Sub

	'Dim _counter As Integer
	'Dim _counterMax As Integer = 100

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
		'Dim _ClsReg As New SPProgUtility.ClsDivReg
		'Dim conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		'ClsDataDetail.SelectedDataTable.Clear()
		'Dim selItem As ClsDataDetail.SelectionItem

		Try

			'With frmTest
			'' SQL-Query aus XML-Datei
			'Dim strQuery As String = "//SPLohnkontiSearch/frmLohnkontiSearch/SQLString[@ID=" & Chr(34) & ClsDataDetail.GetAppGuidValue() & Chr(34) & "]/SQL"

			'Dim strBez As String = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetSQLDataFile(), strQuery)
			'If strBez <> String.Empty Then
			'  sSql = strBez

			'Else
			' Default-Werte
			'Dim jahrVon As Integer = m_SearchCriteria.FirstYear

			'ClsDataDetail.SelectedContainer.Clear()
			ClsDataDetail.LLTablename = String.Format("_LohnartenRekap_{0}", m_InitialData.UserData.UserNr)


			'' JAHR VON
			'If m_SearchCriteria.FirstYear Then
			'	jahrVon = m_SearchCriteria.FirstYear
			'	' Filterbezeichnung
			'	selItem = New ClsDataDetail.SelectionItem
			'	selItem.Bezeichnung = ClsDataDetail.SuchkriterienList.JahrVon
			'	selItem.Text = jahrVon
			'	ClsDataDetail.SelectedContainer.Add(selItem)
			'End If

			' LOHNKONTI
			'Dim cmd As SqlCommand = New SqlCommand(sSql, conn)
			Sql = "[Create New Table For LohnartenRekap With Mandant]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", m_InitialData.MDData.MDNr))
			listOfParams.Add(New SqlClient.SqlParameter("jahrvon", ReplaceMissing(m_SearchCriteria.FirstYear, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("tblName", ReplaceMissing(ClsDataDetail.LLTablename, DBNull.Value)))

			'Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(m_InitialData.MDData.MDDbConn, Sql, listOfParams, CommandType.StoredProcedure)
			Dim result As Boolean = m_utility.ExecuteNonQuery(m_InitialData.MDData.MDDbConn, Sql, listOfParams, CommandType.StoredProcedure)

			If result Then
				Sql = String.Format("Select * From {0} ", ClsDataDetail.LLTablename)
				Sql &= "ORDER BY LANr ASC"

				m_SearchCriteria.sqlsearchstring = Sql

			Else
				Throw New Exception(String.Format("{0} >>> Parameters: {1} Fehler in der Abfrage.", Sql,
													String.Format("{1}{0}{2}{0}{3}{0}{4}",
																				", ", listOfParams(0).Value, listOfParams(1).Value, listOfParams(2).Value)))

			End If





			'cmd.CommandType = CommandType.StoredProcedure
			'Dim pjahrVon As SqlParameter = New SqlParameter("@jahrVon", SqlDbType.Int, 4)
			'Dim ptblName As SqlParameter = New SqlParameter("@tblName", SqlDbType.NVarChar, 40)
			'pjahrVon.Value = m_SearchCriteria.FirstYear
			'ptblName.Value = ClsDataDetail.LLTablename
			'cmd.Parameters.Add(pjahrVon)
			'cmd.Parameters.Add(ptblName)

			''NotifyMainProgressBar("Hole Daten...", 20)
			'Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
			'Dim dt As DataTable = New DataTable("LOHNARTENREKAP")
			''AddHandler dt.RowChanged, New DataRowChangeEventHandler(AddressOf Row_Changed)

			'' Es muss während der Übermittlung zur und von der Datenbank, dass der Benutzer die Suche unterbricht.
			''NotifyMainAllowAbort(False)
			'If da.Fill(dt) > 0 Then
			'	ClsDataDetail.SelectedDataTable = dt
			'	'NotifyMainProgressBar("Daten laden abgeschlossen", 40)
			'End If

			' Für die Anzeige in der SQL-Abfrage-Reiter 
			'sSql = String.Format("EXEC [Create New Table For LohnartenRekap With Mandant] @jahrVon={0}, @tblName={1}", m_SearchCriteria.FirstYear, ClsDataDetail.LLTablename)

			'sSqlLen = Len(sSql)
			'NotifyMainProgressBar("Daten laden abgeschlossen", 60)
			'     End If

			'End With

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			Return String.Empty

		Finally
			'conn.Close()
		End Try

		Return Sql

	End Function


	'Private Sub Row_Changed(ByVal sender As Object, ByVal e As DataRowChangeEventArgs)
	'  Try
	'    ' Die Übertragung mit der Datenbank ist abgeschlossen. Jetzt wird das DataTable gefüllt und kann unterbrochen werden.
	'	'NotifyMainAllowAbort(True)

	'    _counter += 1
	'    If _counter > _counterMax Then
	'      _counterMax = _counterMax * 2
	'      _counter = 1
	'    End If
	'	'NotifyMainProgressBar("Daten werden aufbereitet...", ClsDataDetail.GetProzent(1, _counterMax, _counter))
	'  Catch ex As Threading.ThreadAbortException
	'    ' Der Thread wurde abgebrochen --> Kein Fehler
	'	'NotifyMainProgressBar("Thread abgebrochen", 1)
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