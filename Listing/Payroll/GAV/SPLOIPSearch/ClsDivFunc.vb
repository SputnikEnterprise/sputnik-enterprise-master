
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.Utility

Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities

Imports SPLOIPSearch.ClsDataDetail


Public Class ClsDivFunc


	'#Region "Diverses"

	'	'// Get4What._strModul4What
	'	Dim _strModul4What As String
	'  Public Property Get4What() As String
	'    Get
	'      Return _strModul4What
	'    End Get
	'    Set(ByVal value As String)
	'      _strModul4What = value
	'    End Set
	'  End Property

	'  '// Query.GetSearchQuery
	'  Dim _strQuery As String
	'  Public Property GetSearchQuery() As String
	'    Get
	'      Return _strQuery
	'    End Get
	'    Set(ByVal value As String)
	'      _strQuery = value
	'    End Set
	'  End Property

	'  '// LargerLV
	'  Dim _bLargerLV As Boolean
	'  Public Property GetLargerLV() As Boolean
	'    Get
	'      Return _bLargerLV
	'    End Get
	'    Set(ByVal value As Boolean)
	'      _bLargerLV = value
	'    End Set
	'  End Property


	'#End Region

	'#Region "Funktionen für LvClick in der Suchmaske..."

	'  '// Allgemeiner Zwischenspeicher
	'  Dim _strSelektion As String
	'  Public Property GetSelektion() As String
	'    Get
	'      Return _strSelektion
	'    End Get
	'    Set(ByVal value As String)
	'      _strSelektion = value
	'    End Set
	'  End Property

	'  ' // ID
	'  Dim _strID As String
	'  Public Property GetID() As String
	'    Get
	'      Return _strID
	'    End Get
	'    Set(ByVal value As String)
	'      _strID = value
	'    End Set
	'  End Property

	'  ' // LOIPNr
	'  Dim _strLOIPNr As String
	'  Public Property GetLOIPNr() As String
	'    Get
	'      Return _strLOIPNr
	'    End Get
	'    Set(ByVal value As String)
	'      _strLOIPNr = value
	'    End Set
	'  End Property

	'  ' // KrediNr
	'  Dim _strKrediNr As String
	'  Public Property GetKrediNr() As String
	'    Get
	'      Return _strKrediNr
	'    End Get
	'    Set(ByVal value As String)
	'      _strKrediNr = value
	'    End Set
	'  End Property

	'  ' // ESNr
	'  Dim _strESNr As String
	'  Public Property GetESNr() As String
	'    Get
	'      Return _strESNr
	'    End Get
	'    Set(ByVal value As String)
	'      _strESNr = value
	'    End Set
	'  End Property

	'  '// MANr
	'  Dim _strMANr As String
	'  Public Property GetMANr() As String
	'    Get
	'      Return _strMANr
	'    End Get
	'    Set(ByVal value As String)
	'      _strMANr = value
	'    End Set
	'  End Property

	'  '// KDNr
	'  Dim _strKDNr As String
	'  Public Property GetKDNr() As String
	'    Get
	'      Return _strKDNr
	'    End Get
	'    Set(ByVal value As String)
	'      _strKDNr = value
	'    End Set
	'  End Property

	'  '// Kundennamen
	'  Dim _strKDName As String
	'  Public Property GetKDName() As String
	'    Get
	'      Return _strKDName
	'    End Get
	'    Set(ByVal value As String)
	'      _strKDName = value
	'    End Set
	'  End Property

	'  '// Kandidatenname
	'  Dim _strMAName As String
	'  Public Property GetMAName() As String
	'    Get
	'      Return _strMAName
	'    End Get
	'    Set(ByVal value As String)
	'      _strMAName = value
	'    End Set
	'  End Property

	'  '// Kandidatenvorname
	'  Dim _strMAVorname As String
	'  Public Property GetMAVorname() As String
	'    Get
	'      Return _strMAVorname
	'    End Get
	'    Set(ByVal value As String)
	'      _strMAVorname = value
	'    End Set
	'  End Property

	'  '// GAV-Beruf
	'  Dim _strESGAVBeruf As String
	'  Public Property GetESGAVBeruf() As String
	'    Get
	'      Return _strESGAVBeruf
	'    End Get
	'    Set(ByVal value As String)
	'      _strESGAVBeruf = value
	'    End Set
	'  End Property

	'  '// Einsatz als
	'  Dim _strESEinsatzAls As String
	'  Public Property GetESEinsatzAls() As String
	'    Get
	'      Return _strESEinsatzAls
	'    End Get
	'    Set(ByVal value As String)
	'      _strESEinsatzAls = value
	'    End Set
	'  End Property

	'  '// Query.GetSearchQuery
	'  Dim _strTelNr As String
	'  Public Property GetTelNr() As String
	'    Get
	'      Return _strTelNr
	'    End Get
	'    Set(ByVal value As String)
	'      _strTelNr = value
	'    End Set
	'  End Property

	'#End Region

	'#Region "LL_Properties"
	'  '// Print.LLDocName
	'  Dim _LLDocName As String
	'  Public Property LLDocName() As String
	'    Get
	'      Return _LLDocName
	'    End Get
	'    Set(ByVal value As String)
	'      _LLDocName = value
	'    End Set
	'  End Property

	'  '// Print.LLDocLabel
	'  Dim _LLDocLabel As String
	'  Public Property LLDocLabel() As String
	'    Get
	'      Return _LLDocLabel
	'    End Get
	'    Set(ByVal value As String)
	'      _LLDocLabel = value
	'    End Set
	'  End Property

	'  '// Print.LLFontDesent
	'  Dim _LLFontDesent As Integer
	'  Public Property LLFontDesent() As Integer
	'    Get
	'      Return _LLFontDesent
	'    End Get
	'    Set(ByVal value As Integer)
	'      _LLFontDesent = value
	'    End Set
	'  End Property

	'  '// Print.LLIncPrv
	'  Dim _LLIncPrv As Integer
	'  Public Property LLIncPrv() As Integer
	'    Get
	'      Return _LLIncPrv
	'    End Get
	'    Set(ByVal value As Integer)
	'      _LLIncPrv = value
	'    End Set
	'  End Property

	'  '// Print.LLParamCheck
	'  Dim _LLParamCheck As Integer
	'  Public Property LLParamCheck() As Integer
	'    Get
	'      Return _LLParamCheck
	'    End Get
	'    Set(ByVal value As Integer)
	'      _LLParamCheck = value
	'    End Set
	'  End Property

	'  '// Print.LLKonvertName
	'  Dim _LLKonvertName As Integer
	'  Public Property LLKonvertName() As Integer
	'    Get
	'      Return _LLKonvertName
	'    End Get
	'    Set(ByVal value As Integer)
	'      _LLKonvertName = value
	'    End Set
	'  End Property

	'  '// Print.LLZoomProz
	'  Dim _LLZoomProz As Integer
	'  Public Property LLZoomProz() As Integer
	'    Get
	'      Return _LLZoomProz
	'    End Get
	'    Set(ByVal value As Integer)
	'      _LLZoomProz = value
	'    End Set
	'  End Property

	'  '// Print.LLCopyCount
	'  Dim _LLCopyCount As Integer
	'  Public Property LLCopyCount() As Integer
	'    Get
	'      Return _LLCopyCount
	'    End Get
	'    Set(ByVal value As Integer)
	'      _LLCopyCount = value
	'    End Set
	'  End Property

	'  '// Print.LLExportedFilePath
	'  Dim _LLExportedFilePath As String
	'  Public Property LLExportedFilePath() As String
	'    Get
	'      Return _LLExportedFilePath
	'    End Get
	'    Set(ByVal value As String)
	'      _LLExportedFilePath = value
	'    End Set
	'  End Property

	'  '// Print.LLExportedFileName
	'  Dim _LLExportedFileName As String
	'  Public Property LLExportedFileName() As String
	'    Get
	'      Return _LLExportedFileName
	'    End Get
	'    Set(ByVal value As String)
	'      _LLExportedFileName = value
	'    End Set
	'  End Property

	'  '// Print.LLPrintInDiffColor
	'  Dim _LLPrintInDiffColor As Boolean
	'  Public Property LLPrintInDiffColor() As Boolean
	'    Get
	'      Return _LLPrintInDiffColor
	'    End Get
	'    Set(ByVal value As Boolean)
	'      _LLPrintInDiffColor = value
	'    End Set
	'  End Property

	'  '// Print.LLExportfilter
	'  Dim _LLExportfilter As String
	'  Public Property LLExportfilter() As String
	'    Get
	'      Return _LLExportfilter
	'    End Get
	'    Set(ByVal value As String)
	'      _LLExportfilter = value
	'    End Set
	'  End Property

	'  '// Print.LLExporterName
	'  Dim _LLExporterName As String
	'  Public Property LLExporterName() As String
	'    Get
	'      Return _LLExporterName
	'    End Get
	'    Set(ByVal value As String)
	'      _LLExporterName = value
	'    End Set
	'  End Property

	'  '// Print.LLExporterFileName
	'  Dim _LLExporterFileName As String
	'  Public Property LLExporterFileName() As String
	'    Get
	'      Return _LLExporterFileName
	'    End Get
	'    Set(ByVal value As String)
	'      _LLExporterFileName = value
	'    End Set
	'  End Property

	'#End Region

	'#Region "US Setting"

	'  '// USeMail (= eMail des Personalvermittlers)
	'  Dim _USeMail As String
	'  Public Property USeMail() As String
	'    Get
	'      Return _USeMail
	'    End Get
	'    Set(ByVal value As String)
	'      _USeMail = value
	'    End Set
	'  End Property

	'  '// USTelefon (= USTelefon des Personalvermittlers)
	'  Dim _USTelefon As String
	'  Public Property USTelefon() As String
	'    Get
	'      Return _USTelefon
	'    End Get
	'    Set(ByVal value As String)
	'      _USTelefon = value
	'    End Set
	'  End Property

	'  '// USTelefax (= USTelefax des Personalvermittlers)
	'  Dim _USTelefax As String
	'  Public Property USTelefax() As String
	'    Get
	'      Return _USTelefax
	'    End Get
	'    Set(ByVal value As String)
	'      _USTelefax = value
	'    End Set
	'  End Property

	'  '// USVorname (= USVorname des Personalvermittlers)
	'  Dim _USVorname As String
	'  Public Property USVorname() As String
	'    Get
	'      Return _USVorname
	'    End Get
	'    Set(ByVal value As String)
	'      _USVorname = value
	'    End Set
	'  End Property

	'  '// USAnrede (= USAnrede des Personalvermittlers)
	'  Dim _USAnrede As String
	'  Public Property USAnrede() As String
	'    Get
	'      Return _USAnrede
	'    End Get
	'    Set(ByVal value As String)
	'      _USAnrede = value
	'    End Set
	'  End Property

	'  '// USNachname (= USNachname des Personalvermittlers)
	'  Dim _USNachname As String
	'  Public Property USNachname() As String
	'    Get
	'      Return _USNachname
	'    End Get
	'    Set(ByVal value As String)
	'      _USNachname = value
	'    End Set
	'  End Property

	'  '// USMDName (= MDName des Personalvermittlers)
	'  Dim _USMDname As String
	'  Public Property USMDname() As String
	'    Get
	'      Return _USMDname
	'    End Get
	'    Set(ByVal value As String)
	'      _USMDname = value
	'    End Set
	'  End Property

	'  '// MDName2 (= MDName2 des Personalvermittlers)
	'  Dim _USMDname2 As String
	'  Public Property USMDname2() As String
	'    Get
	'      Return _USMDname2
	'    End Get
	'    Set(ByVal value As String)
	'      _USMDname2 = value
	'    End Set
	'  End Property

	'  '// MDName3 (= MDName3 des Personalvermittlers)
	'  Dim _USMDname3 As String
	'  Public Property USMDname3() As String
	'    Get
	'      Return _USMDname3
	'    End Get
	'    Set(ByVal value As String)
	'      _USMDname3 = value
	'    End Set
	'  End Property

	'  '// USMDPostfach (= MDPostfach des Personalvermittlers)
	'  Dim _USMDPostfach As String
	'  Public Property USMDPostfach() As String
	'    Get
	'      Return _USMDPostfach
	'    End Get
	'    Set(ByVal value As String)
	'      _USMDPostfach = value
	'    End Set
	'  End Property

	'  '// USMDStrasse (= MDstrasse des Personalvermittlers)
	'  Dim _USMDStrasse As String
	'  Public Property USMDStrasse() As String
	'    Get
	'      Return _USMDStrasse
	'    End Get
	'    Set(ByVal value As String)
	'      _USMDStrasse = value
	'    End Set
	'  End Property

	'  '// USMDOrt (= MDOrt des Personalvermittlers)
	'  Dim _USMDOrt As String
	'  Public Property USMDOrt() As String
	'    Get
	'      Return _USMDOrt
	'    End Get
	'    Set(ByVal value As String)
	'      _USMDOrt = value
	'    End Set
	'  End Property

	'  '// USMDPLZ (= MDPLZ des Personalvermittlers)
	'  Dim _USMDPlz As String
	'  Public Property USMDPlz() As String
	'    Get
	'      Return _USMDPlz
	'    End Get
	'    Set(ByVal value As String)
	'      _USMDPlz = value
	'    End Set
	'  End Property

	'  '// USMDLand (= MDLand des Personalvermittlers)
	'  Dim _USMDLand As String
	'  Public Property USMDLand() As String
	'    Get
	'      Return _USMDLand
	'    End Get
	'    Set(ByVal value As String)
	'      _USMDLand = value
	'    End Set
	'  End Property

	'  '// USMDTelefon (= MDTelefon des Personalvermittlers)
	'  Dim _USMDTelefon As String
	'  Public Property USMDTelefon() As String
	'    Get
	'      Return _USMDTelefon
	'    End Get
	'    Set(ByVal value As String)
	'      _USMDTelefon = value
	'    End Set
	'  End Property

	'  '// USMDTelefax (= MDTelefax des Personalvermittlers)
	'  Dim _USMDTelefax As String
	'  Public Property USMDTelefax() As String
	'    Get
	'      Return _USMDTelefax
	'    End Get
	'    Set(ByVal value As String)
	'      _USMDTelefax = value
	'    End Set
	'  End Property

	'  '// USMDeMail (= MDeMail des Personalvermittlers)
	'  Dim _USMDeMail As String
	'  Public Property USMDeMail() As String
	'    Get
	'      Return _USMDeMail
	'    End Get
	'    Set(ByVal value As String)
	'      _USMDeMail = value
	'    End Set
	'  End Property

	'  '// USMDHomepage (= MDHomepage des Personalvermittlers)
	'  Dim _USMDHomepage As String
	'  Public Property USMDHomepage() As String
	'    Get
	'      Return _USMDHomepage
	'    End Get
	'    Set(ByVal value As String)
	'      _USMDHomepage = value
	'    End Set
	'  End Property

	'#End Region

End Class

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



	Function GetStartSQLString() As String
		Dim sql As String = String.Empty
		Dim tabellenName As String = String.Format("_InkassoPool_{0}", m_InitialData.UserData.UserNr)

		ClsDataDetail.LLTabellennamen = tabellenName

		Try
			If m_SearchCriteria.FirstYear <= 2012 Then sql = "[Create New Table For InkassoPool GAVListe With Mandant]" Else sql = "[Create New Table For InkassoPool GAVListe With Mandant_2012]"

			', manr, jahr, vonMonat, bisMonat, .Cbo_LOIPGAVBeruf.Text, .Cbo_LOIPGAVKanton.Text, tabellenName, strProcName

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", m_InitialData.MDData.MDNr))
			listOfParams.Add(New SqlClient.SqlParameter("jahr", ReplaceMissing(m_SearchCriteria.FirstYear, Now.Year)))

			listOfParams.Add(New SqlClient.SqlParameter("vonMonat", ReplaceMissing(m_SearchCriteria.FirstMonth, Now.Month)))
			listOfParams.Add(New SqlClient.SqlParameter("bisMonat", ReplaceMissing(m_SearchCriteria.LastMonth, Now.Month)))

			listOfParams.Add(New SqlClient.SqlParameter("gavBeruf", ReplaceMissing(m_SearchCriteria.beruf, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("gavKanton", ReplaceMissing(m_SearchCriteria.kanton, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("tblName", ReplaceMissing(tabellenName, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("MANRList", ReplaceMissing(m_SearchCriteria.MANrList, String.Empty)))

			Dim result = m_utility.ExecuteNonQuery(m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

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


#Region "Funktionen zur Suche nach Daten..."

	'''' <summary>
	'''' Dynamische SQL-Query-Zusammenstellung der benötigten Tabellen.
	'''' </summary>
	'''' <param name="frmTest"></param>
	'''' <returns></returns>
	'''' <remarks></remarks>
	'Function GetStartSQLString(ByVal frmTest As frmLOIPSearch) As String
	'   Dim sSql As String = String.Empty
	'   Dim sSqlLen As Integer = 0
	'   Dim sZusatzBez As String = String.Empty
	'   Dim i As Integer = 0
	'   Dim _ClsReg As New SPProgUtility.ClsDivReg

	'   With frmTest
	'     ' SQL-Query der Tabelle LOL aus XML-Datei
	'     Dim strQuery As String = "//SPSLOIPSearch/frmLOIPSearch/SQLString[@ID=" & Chr(34) & ClsDataDetail.GetAppGuidValue() & Chr(34) & "]/SQL"

	'     Dim strBez As String = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetSQLDataFile(), strQuery)
	'     If strBez <> String.Empty Then
	'       sSql = strBez

	'     Else
	'       Dim manr As String = .txtLOIPMANr_1.Text
	'       Dim jahr As String = Date.Now.Year.ToString
	'       Dim vonMonat As String = "1"
	'       Dim bisMonat As String = "12"
	'       Dim tabellenName As String = String.Format("_InkassoPool_{0}", _ClsProgSetting.GetLogedUSNr)

	'       ' JAHR
	'       If .Cbo_LOIPJahr.Text.Length > 0 Then
	'         jahr = .Cbo_LOIPJahr.Text
	'       End If
	'       ' VON MONAT
	'       If .Cbo_LOIPMonatVon.Text.Length > 0 Then
	'         vonMonat = .Cbo_LOIPMonatVon.Text
	'       End If
	'       ' BIS MONAT
	'       If .Cbo_LOIPMonatBis.Text.Length > 0 Then
	'         bisMonat = .Cbo_LOIPMonatBis.Text
	'       End If

	'       ' VORJAHRESLOHNSUMME BZW. JAHRESLOHNSUMME EINES GEWERBES
	'       ' Übergabe der Parameter für die Berechnung in der FuncLL.GetJahreslohnsumme()
	'       ClsDataDetail.LLJahreslohnsummeGAVBeruf = .Cbo_LOIPGAVBeruf.Text
	'       ClsDataDetail.LLVorjahreslohnsummeJahr = CInt(jahr) - 1

	'       ' TABELLENNAMEN FÜR DEN SELECT UND LL
	'       ClsDataDetail.LLTabellennamen = tabellenName
	'       Dim strProcName As String = String.Empty
	'       strProcName = "[Create New Table For InkassoPool GAVListe]"

	'       'If .Cbo_LOIPGAVBeruf.Text.ToLower.Contains("gebäude") Then
	'       '  strProcName = "[Create New Table For InkassoPool GAVListe For Month]"
	'       'Else
	'       '  strProcName = "[Create New Table For InkassoPool GAVListe]"
	'       'End If

	'       sSql = String.Format("EXEC {7} '{0}', {1}, {2}, {3}, '{4}', '{5}', '{6}'" _
	'                            , manr, jahr, vonMonat, bisMonat, .Cbo_LOIPGAVBeruf.Text, .Cbo_LOIPGAVKanton.Text, tabellenName, _
	'                            strProcName)

	'     End If

	'     sSqlLen = Len(sSql)

	'   End With

	'   Return sSql
	' End Function


	'Function GetStartSQLString(ByVal frmTest As frmLOIPSearch) As String
	'  Dim sSql As String = String.Empty
	'  Dim sSqlLen As Integer = 0
	'  Dim sZusatzBez As String = String.Empty
	'  Dim i As Integer = 0
	'  Dim _ClsReg As New SPProgUtility.ClsDivReg

	'  With frmTest
	'    ' SQL-Query der Tabelle LOL aus XML-Datei
	'    Dim strQuery As String = "//SPSLOIPSearch/frmLOIPSearch/SQLString[@ID=" & Chr(34) & ClsDataDetail.GetAppGuidValue() & Chr(34) & "]/SQL"

	'    Dim strBez As String = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetSQLDataFile(), strQuery)
	'    If strBez <> String.Empty Then
	'      sSql = strBez

	'    Else
	'      Dim jahr As String = Date.Now.Year.ToString
	'      Dim vonMonat As String = "1"
	'      Dim bisMonat As String = "12"
	'      Dim gavBeruf As String = ""
	'      Dim gavBerufESLohn As String = ""
	'      Dim gavKanton As String = ""
	'      Dim gavKantonESLohn As String = ""
	'      Dim gavBerufRap As String = ""

	'      Dim gavKantonInkassoPoolLOL As String = ""
	'      Dim gavKantonInkassoPoolRap As String = ""
	'      Dim tabellenName As String = String.Format("_InkassoPool_{0}", _ClsProgSetting.GetLogedUSNr)

	'      ' Nur Kantone berücksichtigen, die im betreffenden GAV-Beruf im InkassoPool angeschlossen sind
	'      Dim conn As New SqlConnection(_ClsProgSetting.GetConnString)
	'      Dim sSqlTemp As String = "Select GAVKanton From TAB_InkassoPool "
	'      sSqlTemp += String.Format("Where GAVBeruf = '{0}'", .Cbo_LOIPGAVBeruf.Text)
	'      Dim sqlCom As New SqlCommand(sSqlTemp, conn)
	'      sqlCom.CommandType = CommandType.Text
	'      sqlCom.Parameters.AddWithValue("@gavBeruf", .Cbo_LOIPGAVBeruf.Text)
	'      Try
	'        conn.Open()
	'        Dim reader As SqlDataReader = sqlCom.ExecuteReader()
	'        gavKantonInkassoPoolLOL += "LOL.GAV_Kanton In ("
	'        gavKantonInkassoPoolRap += "RP.RPGAV_Kanton In ("
	'        While reader.Read()
	'          gavKantonInkassoPoolLOL += String.Format("'{0}',", reader("GAVKanton"))
	'          gavKantonInkassoPoolRap += String.Format("'{0}',", reader("GAVKanton"))
	'        End While
	'        gavKantonInkassoPoolLOL = gavKantonInkassoPoolLOL.Substring(0, gavKantonInkassoPoolLOL.Length - 1)
	'        gavKantonInkassoPoolLOL += ") And "
	'        gavKantonInkassoPoolRap = gavKantonInkassoPoolRap.Substring(0, gavKantonInkassoPoolRap.Length - 1)
	'        gavKantonInkassoPoolRap += ") And "
	'      Catch ex As Exception
	'      Finally
	'        conn.Close()
	'      End Try


	'      ' JAHR
	'      If .Cbo_LOIPJahr.Text.Length > 0 Then
	'        jahr = .Cbo_LOIPJahr.Text
	'      End If
	'      ' VON MONAT
	'      If .Cbo_LOIPMonatVon.Text.Length > 0 Then
	'        vonMonat = .Cbo_LOIPMonatVon.Text
	'      End If
	'      ' BIS MONAT
	'      If .Cbo_LOIPMonatBis.Text.Length > 0 Then
	'        bisMonat = .Cbo_LOIPMonatBis.Text
	'      End If
	'      ' GAV KANTON
	'      If .Cbo_LOIPGAVKanton.Text.Length > 0 Then
	'        If InStr(.Cbo_LOIPGAVKanton.Text, ",") > 0 Then
	'          gavKanton = "LOL.GAV_Kanton In ("
	'          gavKantonESLohn = "ESLohn.GAVKanton In ("
	'          For Each gavKt In .Cbo_LOIPGAVKanton.Text.Split(CChar(","))
	'            gavKanton += String.Format("'{0}',", gavKt)
	'            gavKantonESLohn += String.Format("'{0}',", gavKt)
	'          Next
	'          gavKanton = gavKanton.Substring(0, gavKanton.Length - 1)
	'          gavKantonESLohn = gavKantonESLohn.Substring(0, gavKantonESLohn.Length - 1)
	'          gavKanton += ") And "
	'          gavKantonESLohn += ") And "
	'        Else
	'          gavKanton = String.Format("LOL.GAV_Kanton = '{0}' And ", .Cbo_LOIPGAVKanton.Text)
	'          gavKantonESLohn = String.Format("ESLohn.GAVKanton = '{0}' And ", .Cbo_LOIPGAVKanton.Text)
	'        End If
	'      End If
	'      ' GAV BERUF
	'      gavBeruf = String.Format("LOL.GAV_Beruf = '{0}' And ", .Cbo_LOIPGAVBeruf.Text)
	'      gavBerufESLohn = String.Format("ESLohn.GAVGruppe0 = '{0}' And ", .Cbo_LOIPGAVBeruf.Text)
	'      gavBerufRap = String.Format("RP.RPGAV_Beruf = '{0}' And ", .Cbo_LOIPGAVBeruf.Text)

	'      ' TABELLE LÖSCHEN
	'      sSql += String.Format("BEGIN TRY DROP TABLE {0} END TRY BEGIN CATCH END CATCH ", tabellenName)

	'      ' SELECT
	'      sSql += "Select Mitarbeiter.Manr, Mitarbeiter.Nachname, Mitarbeiter.Vorname, "
	'      'sSql += "Mitarbeiter.Geschlecht, Mitarbeiter.AHV_Nr, Mitarbeiter.AHV_Nr_New, "
	'      sSql += "Year(Mitarbeiter.GebDat) as Jahrgang, Mitarbeiter.GebDat, "
	'      sSql += String.Format("'{0}' as VonMonat, '{1}' as BisMonat, ", vonMonat, bisMonat)
	'      sSql += String.Format("'{0}' as Jahr", jahr)

	'      ' LOHNPERIODE AB
	'      sSql += ", ("
	'      sSql += "   Select Min(LOL.LP)  From LOL "
	'      sSql += "   Where "
	'      sSql += "   LOL.LANr = 6989 and " '-- Arbeitsstunden pro Kanton & GAV
	'      sSql += String.Format("   LOL.Jahr = {0} and ", jahr)
	'      sSql += gavKantonInkassoPoolLOL
	'      If gavKanton.Length > 0 Then
	'        sSql += gavKanton
	'      End If
	'      sSql += String.Format("   LOL.LP Between {0} and {1} and ", vonMonat, bisMonat)
	'      sSql += gavBeruf
	'      sSql += "   LOL.MANr = Mitarbeiter.MANr "
	'      sSql += "  ) as abLP, "

	'      ' LOHNPERIODE BIS
	'      sSql += "("
	'      sSql += "   Select Max(LOL.LP)  From LOL "
	'      sSql += "   Where "
	'      sSql += "   LOL.LANr = 6989 and " '-- Arbeitsstunden pro Kanton & GAV
	'      sSql += String.Format("   LOL.Jahr = {0} and ", jahr)
	'      sSql += gavKantonInkassoPoolLOL
	'      If gavKanton.Length > 0 Then
	'        sSql += gavKanton
	'      End If
	'      sSql += String.Format("   LOL.LP Between {0} and {1} and ", vonMonat, bisMonat)
	'      sSql += gavBeruf
	'      sSql += "   LOL.MANr = Mitarbeiter.MANr "
	'      sSql += "  ) as bisLP, "

	'      ' SUMME STUNDEN
	'      sSql += "("
	'      sSql += "   Select Sum(LOL.M_Anz) From LOL "
	'      sSql += "   Where "
	'      sSql += "   LOL.LANr = 6989 and " '-- Arbeitsstunden pro Kanton & GAV
	'      sSql += gavKantonInkassoPoolLOL
	'      sSql += String.Format("   LOL.Jahr = {0} and ", jahr)
	'      If gavKanton.Length > 0 Then
	'        sSql += gavKanton
	'      End If
	'      sSql += String.Format("   LOL.LP Between {0} and {1} and ", vonMonat, bisMonat)
	'      sSql += gavBeruf
	'      sSql += "   LOL.MANr = Mitarbeiter.MANr "
	'      sSql += "  ) as AnzahlStd, " 'TotalStd

	'      ' SUMME BEITRAG ARBEITNEHMER
	'      sSql += "("
	'      sSql += "   Select IsNull(Sum(LOL.M_Btr),0) From LOL "
	'      sSql += "   Where " '-- Bildungsfond & Vollzugsfond Arbeitnehmer/Arbeitgeber
	'      sSql += "   LOL.LANr In (7395.2, 7395.3, 7395.4, 7395.5, 7395.6, 7395.7) And "
	'      sSql += gavKantonInkassoPoolLOL
	'      sSql += String.Format("   LOL.Jahr = {0} and ", jahr)
	'      If gavKanton.Length > 0 Then
	'        sSql += gavKanton
	'      End If
	'      sSql += String.Format("   LOL.LP Between {0} and {1} and ", vonMonat, bisMonat)
	'      sSql += gavBeruf
	'      sSql += "   LOL.MANr = Mitarbeiter.MANr "
	'      sSql += "  ) as BeitragLohnAN, " 'TotalBeitrag

	'      ' SUMME BEITRAG ARBEITGEBER
	'      sSql += "("
	'      sSql += "   Select IsNull(Sum(LOL.M_Btr),0) From LOL "
	'      sSql += "   Where " '-- Bildungsfond & Vollzugsfond Arbeitnehmer/Arbeitgeber
	'      sSql += "   LOL.LANr In (7835.2, 7835.3, 7835.4, 7835.5, 7835.6, 7835.7) And "
	'      sSql += gavKantonInkassoPoolLOL
	'      sSql += String.Format("   LOL.Jahr = {0} and ", jahr)
	'      If gavKanton.Length > 0 Then
	'        sSql += gavKanton
	'      End If
	'      sSql += String.Format("   LOL.LP Between {0} and {1} and ", vonMonat, bisMonat)
	'      sSql += gavBeruf
	'      sSql += "   LOL.MANr = Mitarbeiter.MANr "
	'      sSql += "  ) as BeitragLohnAG, " 'TotalBeitrag

	'      ' ANSATZ ARBEITGEBER STD Vollzugskosten
	'      sSql += "("
	'      sSql += "   Select Max(RPGAV_VAG_S) From RP "
	'      sSql += "   Where "
	'      sSql += "   RP.MANr = Mitarbeiter.MANr And "
	'      sSql += gavBerufRap
	'      sSql += gavKantonInkassoPoolRap
	'      sSql += String.Format("   RP.Jahr = {0} And ", jahr)
	'      sSql += String.Format("   RP.Monat Between {0} And {1} ", vonMonat, bisMonat)
	'      sSql += ") as GAV_VAG_S, "

	'      ' ANSATZ ARBEITGEBER STD Weiterbildung
	'      sSql += "("
	'      sSql += "   Select Max(RPGAV_WAG_S) From RP "
	'      sSql += "   Where "
	'      sSql += "   RP.MANr = Mitarbeiter.MANr And "
	'      sSql += gavBerufRap
	'      sSql += gavKantonInkassoPoolRap
	'      sSql += String.Format("   RP.Jahr = {0} And ", jahr)
	'      sSql += String.Format("   RP.Monat Between {0} And {1} ", vonMonat, bisMonat)
	'      sSql += ") as GAV_WAG_S, "

	'      ' ANSATZ ARBEITNEHMER PROZ Vollzugskosten
	'      sSql += "("
	'      sSql += "   Select Max(RPGAV_VAG) From RP "
	'      sSql += "   Where "
	'      sSql += "   RP.MANr = Mitarbeiter.MANr And "
	'      sSql += gavBerufRap
	'      sSql += gavKantonInkassoPoolRap
	'      sSql += String.Format("   RP.Jahr = {0} And ", jahr)
	'      sSql += String.Format("   RP.Monat Between {0} And {1} ", vonMonat, bisMonat)
	'      sSql += ") as GAV_VAG, "

	'      ' ANSATZ ARBEITNEHMER PROZ Weiterbildung
	'      sSql += "("
	'      sSql += "   Select Max(RPGAV_WAG) From RP "
	'      sSql += "   Where "
	'      sSql += "   RP.MANr = Mitarbeiter.MANr And "
	'      sSql += gavBerufRap
	'      sSql += gavKantonInkassoPoolRap
	'      sSql += String.Format("   RP.Jahr = {0} And ", jahr)
	'      sSql += String.Format("   RP.Monat Between {0} And {1} ", vonMonat, bisMonat)
	'      sSql += ") as GAV_WAG, "

	'      ' ANSATZ ARBEITNEHMER STD Vollzugskosten
	'      sSql += "("
	'      sSql += "   Select Max(RPGAV_VAN_S) From RP "
	'      sSql += "   Where "
	'      sSql += "   RP.MANr = Mitarbeiter.MANr And "
	'      sSql += gavBerufRap
	'      sSql += gavKantonInkassoPoolRap
	'      sSql += String.Format("   RP.Jahr = {0} And ", jahr)
	'      sSql += String.Format("   RP.Monat Between {0} And {1} ", vonMonat, bisMonat)
	'      sSql += ") as GAV_VAN_S, "

	'      ' ANSATZ ARBEITNEHMER STD Weiterbildung
	'      sSql += "("
	'      sSql += "   Select Max(RPGAV_WAN_S) From RP "
	'      sSql += "   Where "
	'      sSql += "   RP.MANr = Mitarbeiter.MANr And "
	'      sSql += gavBerufRap
	'      sSql += gavKantonInkassoPoolRap
	'      sSql += String.Format("   RP.Jahr = {0} And ", jahr)
	'      sSql += String.Format("   RP.Monat Between {0} And {1} ", vonMonat, bisMonat)
	'      sSql += ") as GAV_WAN_S, "

	'      ' ANSATZ ARBEITNEHMER PROZ Vollzugskosten
	'      sSql += "("
	'      sSql += "   Select Max(RPGAV_VAN) From RP "
	'      sSql += "   Where "
	'      sSql += "   RP.MANr = Mitarbeiter.MANr And "
	'      sSql += gavBerufRap
	'      sSql += gavKantonInkassoPoolRap
	'      sSql += String.Format("   RP.Jahr = {0} And ", jahr)
	'      sSql += String.Format("   RP.Monat Between {0} And {1} ", vonMonat, bisMonat)
	'      sSql += ") as GAV_VAN, "

	'      ' ANSATZ ARBEITNEHMER PROZ Weiterbildung
	'      sSql += "("
	'      sSql += "   Select Max(RPGAV_WAN) From RP "
	'      sSql += "   Where "
	'      sSql += "   RP.MANr = Mitarbeiter.MANr And "
	'      sSql += gavBerufRap
	'      sSql += gavKantonInkassoPoolRap
	'      sSql += String.Format("   RP.Jahr = {0} And ", jahr)
	'      sSql += String.Format("   RP.Monat Between {0} And {1} ", vonMonat, bisMonat)
	'      sSql += ") as GAV_WAN, "

	'      ' LOHNSUMME PRO KANDIDAT
	'      sSql += "( Select Sum(LOL.M_Btr) From LOL "
	'      sSql += "Inner Join LA On "
	'      sSql += "LOL.LANr = LA.LANr And "
	'      sSql += "LOL.Jahr = LA.LAJahr And "
	'      sSql += "LA.KKPflichtig = 1 "
	'      sSql += "Where "
	'      sSql += gavBeruf
	'      sSql += "LOL.LANR < 7000 And "
	'      sSql += String.Format("LOL.Jahr = {0} And ", jahr)
	'      sSql += String.Format("LOL.LP Between {0} And {1} And ", vonMonat, bisMonat)
	'      If gavKanton.Length > 0 Then
	'        sSql += gavKanton
	'      Else
	'        sSql += gavKantonInkassoPoolLOL
	'      End If
	'      sSql += "LOL.M_Btr <> 0 And "
	'      sSql += " LOL.MANR = Mitarbeiter.MANR "
	'      sSql += ") as Lohnsumme, "

	'      ' LOHNSUMME BERCHNET
	'      sSql += "( Select Round(IsNull(Sum(Betrag),0)*20,0)/20 From "
	'      sSql += "   (Select LOL.M_BAS, ESLohn.GAV_VAG, Betrag = LOL.M_BAS * ESLohn.GAV_VAG / 100 *-1 From LOL "
	'      sSql += "Left Join ESLohn On "
	'      sSql += gavBerufESLohn
	'      sSql += "ESLohn.MANr = LOL.MANr "
	'      sSql += "Where "
	'      sSql += String.Format("LOL.Jahr = {0} And ", jahr)
	'      sSql += gavBeruf
	'      sSql += String.Format("   LOL.LP Between {0} and {1} and ", vonMonat, bisMonat)
	'      sSql += "    LOL.LANr In (7400.10,7420.10) And "
	'      sSql += "LOL.MANr = Mitarbeiter.MANr "
	'      sSql += "Group By LOL.M_BAS, ESLohn.GAV_VAG ) as t "
	'      sSql += ") as LohnsummeCalc, "

	'      ' ARBEITGEBER-PAUSCHALE
	'      sSql += "(	Select top 1 Pauschale = ESLohn.GAV_WAG_J + ESLohn.GAV_VAG_J "
	'      sSql += "From ESLohn "
	'      sSql += String.Format("Where ESLohn.MANr=Mitarbeiter.MANr and GAVGruppe0 = '{0}') as Pauschale, ", .Cbo_LOIPGAVBeruf.Text)

	'      sSql += "MD_GAV_Adresse.GAV_Name, MD_GAV_Adresse.GAV_ZHD, MD_GAV_Adresse.GAV_Postfach,"
	'      sSql += "MD_GAV_Adresse.GAV_Strasse, MD_GAV_Adresse.GAV_PLZ, MD_GAV_Adresse.GAV_Ort, "
	'      sSql += "MD_GAV_Adresse.GAV_Bank, MD_GAV_Adresse.GAV_BankPLZOrt, MD_GAV_Adresse.GAV_Bankkonto, "
	'      sSql += "MD_GAV_Adresse.GAV_IBAN, MD_GAV_Adresse.BerufBez as GAVBeruf, MD_GAV_Adresse.Kanton as GAVKanton "

	'      ' TABELLE ANLEGEN
	'      sSql += String.Format("Into dbo.{0} ", tabellenName)

	'      sSql += "From Mitarbeiter "
	'      sSql += "Left Join MD_GAV_Adresse On "
	'      sSql += String.Format("MD_GAV_Adresse.BerufBez = '{0}'", .Cbo_LOIPGAVBeruf.Text)

	'      ' VORJAHRESLOHNSUMME
	'      ' Übergabe der Parameter für die Berechnung in der FuncLL.GetJahreslohnsumme()
	'      ClsDataDetail.LLJahreslohnsummeGAVBeruf = .Cbo_LOIPGAVBeruf.Text
	'      ClsDataDetail.LLVorjahreslohnsummeJahr = CInt(jahr) - 1

	'      ' TABELLENNAMEN FÜR DEN SELECT UND LL
	'      ClsDataDetail.LLTabellennamen = tabellenName

	'    End If

	'    ' Zusätzliche benötigte Tabellen -------------------------------------------
	'    '===========================================================================

	'    ''KUNDEN
	'    'sSql += "Left Join Kunden On "
	'    'sSql += "Kunden.KDNr = FremdOP.KDNr And "
	'    'sSql += String.Format("Kunden.KDFiliale Like '%{0}%'", _ClsProgSetting.GetUSFiliale)

	'    ''MITARBEITER
	'    'sSql += "Left Join Mitarbeiter On "
	'    'sSql += "Mitarbeiter.MANr = FremdOP.MANr "

	'    '===========================================================================

	'    sSqlLen = Len(sSql)

	'  End With

	'  Return sSql
	'End Function



#Region "helpers"

	Private Function ReplaceMissing(ByVal obj As Object, ByVal replacementObject As Object) As Object

		If (obj Is Nothing) Then
			Return replacementObject
		Else
			Return obj
		End If

	End Function

#End Region



	'Function GetQuerySQLString(ByVal sSQLQuery As String, ByVal frmTest As frmLOIPSearch) As String
	'   Dim sSql As String = String.Empty
	'   Dim sOldQuery As String = sSQLQuery

	'   Dim FilterBez As String = String.Empty
	'   Dim sSqlLen As Integer = 0
	'   Dim sZusatzBez As String = String.Empty
	'   Dim strAndString As String = String.Empty

	'   Dim strUSFiliale As String = _ClsProgSetting.GetUSFiliale()
	'   Dim iSQLLen As Integer = Len(sSQLQuery)
	'   Dim strFieldName As String = String.Empty

	'   With frmTest

	'     ' 1. Seite === Allgemein =============================================================================================
	'     ' ====================================================================================================================
	'     ' Kandidaten-Nr (MANr) -----------------------------------------------------------------------------------------------
	'     Dim manr1 As String = .txtLOIPMANr_1.Text.Replace("'", "").Replace("*", "%").Trim
	'     strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString

	'     If manr1 = "" Then 'do nothing
	'       ' Suche MANr1 mit Sonderzeichen
	'     ElseIf manr1.Contains("%") Then
	'       FilterBez = String.Format("{0}Lohnrechnungen mit MANr ({1}){2}", strAndString, manr1, vbLf)
	'       sSql += String.Format("{0}LOL.MANr Like '{1}' ", strAndString, manr1)
	'       ' Suche MANr1 mit Komma getrennt
	'     ElseIf InStr(manr1, ",") > 0 Then
	'       FilterBez = String.Format("Lohnrechnungen mit MANr ({0}){1}", manr1, vbLf)
	'       sSql += String.Format("{0}LOL.MANr In (", strAndString)
	'       For Each manr In manr1.Split(CChar(","))
	'         sSql += String.Format("{0},", manr)
	'       Next
	'       sSql = sSql.Substring(0, sSql.Length - 1)
	'       sSql += ") "
	'       ' Suche genau eine MANr
	'     Else
	'       FilterBez = String.Format("Lohnrechnungen mit MANr = {0}{1}", manr1, vbLf)
	'       sSql += String.Format("{0}LOL.MANr = {1} ", strAndString, manr1)
	'     End If

	'     ' Monat von/bis -------------------------------------------------------------------------------------------------------
	'     Dim vonMonat As String = "1"
	'     Dim bisMonat As String = "12"
	'     strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
	'     If .Cbo_LOIPMonatVon.Text.Length > 0 Then
	'       vonMonat = .Cbo_LOIPMonatVon.Text
	'     End If
	'     If .Cbo_LOIPMonatBis.Text.Length > 0 Then
	'       bisMonat = .Cbo_LOIPMonatBis.Text
	'     End If
	'     sSql += String.Format("{0}LOL.LP Between {1} and {2}", strAndString, vonMonat, bisMonat)
	'     FilterBez += String.Format("Lohnrechnungen in den Monate zwischen {0} und {1}{2}", vonMonat, bisMonat, vbLf)

	'     ' Jahr ----------------------------------------------------------------------------------------------------------------
	'     If .Cbo_LOIPJahr.Text.Length > 0 Then
	'       strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
	'       sSql += String.Format("{0}LOL.Jahr = {1}", strAndString, .Cbo_LOIPJahr.Text)
	'       FilterBez += String.Format("Lohnrechnungen im Jahr {0}{1}", .Cbo_LOIPJahr.Text, vbLf)
	'     End If

	'     ' GAV-Kanton ----------------------------------------------------------------------------------------------------------
	'     If .Cbo_LOIPGAVKanton.Text.Length > 0 Then
	'       strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
	'       If InStr(.Cbo_LOIPGAVKanton.Text, ",") > 0 Then
	'         FilterBez += String.Format("Lohnrechnungen mit GAV-Kanton ({0}){1}", .Cbo_LOIPGAVKanton.Text, vbLf)
	'         sSql += String.Format("{0}LOL.GAV_Kanton In (", strAndString)
	'         For Each gavKt In .Cbo_LOIPGAVKanton.Text.Split(CChar(","))
	'           sSql += String.Format("'{0}',", gavKt)
	'         Next
	'         sSql = sSql.Substring(0, sSql.Length - 1)
	'         sSql += ") "
	'       Else
	'         sSql += String.Format("{0}LOL.GAV_Kanton = '{1}' ", strAndString, .Cbo_LOIPGAVKanton.Text)
	'       End If
	'     Else
	'       ' Nur Kantone berücksichtigen, die im betreffenden GAV-Beruf im InkassoPool angeschlossen sind
	'       Dim conn As New SqlConnection(_ClsProgSetting.GetConnString)
	'       Dim sSqlTemp As String = "Select GAVKanton From TAB_InkassoPool "
	'       sSqlTemp += String.Format("Where GAVBeruf = '{0}'", .Cbo_LOIPGAVBeruf.Text)
	'       Dim sqlCom As New SqlCommand(sSqlTemp, conn)
	'       sqlCom.CommandType = CommandType.Text
	'       sqlCom.Parameters.AddWithValue("@gavBeruf", .Cbo_LOIPGAVBeruf.Text)
	'       Try
	'         conn.Open()
	'         Dim reader As SqlDataReader = sqlCom.ExecuteReader()
	'         sSql += String.Format("{0}LOL.GAV_Kanton In (", strAndString)
	'         While reader.Read()
	'           sSql += String.Format("'{0}',", reader("GAVKanton"))
	'         End While
	'         sSql = sSql.Substring(0, sSql.Length - 1)
	'         sSql += ") "
	'       Catch ex As Exception
	'       Finally
	'         conn.Close()
	'       End Try

	'     End If

	'     ' GAV-Beruf ----------------------------------------------------------------------------------------------------------
	'     If .Cbo_LOIPGAVBeruf.Text.Length > 0 Then
	'       strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
	'       If InStr(.Cbo_LOIPGAVBeruf.Text, ",") > 0 Then
	'         FilterBez += String.Format("Lohnrechnungen mit GAV-Beruf ({0}){1}", .Cbo_LOIPGAVBeruf.Text, vbLf)
	'         sSql += String.Format("{0}LOL.GAV_Beruf In (", strAndString)
	'         For Each gavBer In .Cbo_LOIPGAVBeruf.Text.Split(CChar(","))
	'           sSql += String.Format("'{0}',", gavBer)
	'         Next
	'         sSql = sSql.Substring(0, sSql.Length - 1)
	'         sSql += ") "
	'       Else
	'         sSql += String.Format("{0}LOL.GAV_Beruf = '{1}' ", strAndString, .Cbo_LOIPGAVBeruf.Text)
	'       End If
	'     End If

	'     ' Nur Bildungsfond-Lohnarten berücksichtigen
	'     sSql += " And LOL.LANr In (7395.2,7395.3,7395.4,7395.5,7395.6,7395.7) "

	'     ' =================================================================================================================
	'     ' Selektion abschliessen
	'     ' Nur MANr selektieren, die obengenannten Kriterien haben
	'     sSql = String.Format("Mitarbeiter.MANr In (Select LOL.MANr From LOL Left Join LO On LO.MANr = LOL.MANr Where {0})", sSql)


	'   End With

	'   ClsDataDetail.GetFilterBez = FilterBez

	'   Return sSql
	' End Function

#End Region


End Class


'Function GetLstItems(ByVal lst As ListBox) As String
'  Dim strBerufItems As String = String.Empty

'  For i = 0 To lst.Items.Count - 1
'    strBerufItems += lst.Items(i).ToString & "#@"
'  Next

'  Return Left(strBerufItems, Len(strBerufItems) - 2)
'End Function

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
