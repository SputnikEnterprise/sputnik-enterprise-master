
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions

Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities

Imports SPOPSearch.ClsDataDetail
Imports SP.Infrastructure.Logging

Public Class ClsDivFunc

#Region "Diverse Properties"

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

	'// Is LV in Virtualmode
	Dim _bLVAsVirtual As Boolean
	Public Property IsLVInVirtualMode() As Boolean
		Get
			Return _bLVAsVirtual
		End Get
		Set(ByVal value As Boolean)
			_bLVAsVirtual = value
		End Set
	End Property


#End Region

#Region "Properties für LvClick in der Suchmaske..."

	'// OPNr
	Dim _strOPNr As String
	Public Property GetOPNr() As String
		Get
			Return _strOPNr
		End Get
		Set(ByVal value As String)
			_strOPNr = value
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

#End Region

#Region "LL_Properties"
	'// Print.LLDocName
	Dim _LLDocName As String
	Public Property LLFullDocPath() As String
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

#Region "Allgemeine Funktionen"


	''' <summary>
	''' Dynamischer Aufbau des Drucken-KontextMenu-Bezeichnung.
	''' </summary>
	''' <param name="JobNr">Die JobNr, wie auf der Tabelle DokPrint vorhanden ist.</param>
	''' <returns>Gibt den Namen des Dokuments oder die Bezeichnung zurück.</returns>
	''' <remarks></remarks>
	Public Function GetPrintContextMenuText(ByVal JobNr As String) As String
		Dim text As String = ""
		Dim sSql As String = "Select DokNameToShow, Bezeichnung From DokPrint Where [JobNr] = @JobNr"
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim cmd As SqlCommand = New SqlCommand(sSql, Conn)

		Try
			Conn.Open()
			cmd.CommandType = CommandType.Text
			cmd.Parameters.AddWithValue("@JobNr", JobNr)
			Dim reader As SqlDataReader = cmd.ExecuteReader()
			reader.Read()

			If reader.HasRows Then
				If Not IsDBNull(reader("DokNameToShow")) AndAlso reader("DokNameToShow").ToString.Length > 0 Then
					text = reader("DokNameToShow").ToString
				ElseIf Not IsDBNull(reader("Bezeichnung")) Then
					text = reader("Bezeichnung").ToString
				End If
			End If

			Return text

		Catch sqlExc As SqlException
			Dim fehlerMeldung As String = ""
			For Each errorText As SqlError In sqlExc.Errors
				fehlerMeldung += String.Format("{0}: {1}", errorText.Number, errorText.Message)
			Next

			MessageBox.Show(fehlerMeldung, "Datenbank oder Verbindung zum Server", MessageBoxButtons.OK, MessageBoxIcon.Error)
		Catch e As Exception
			MessageBox.Show(e.Message, "Aufbau Drucken-Kontextmenu fehlgeschlagen...", MessageBoxButtons.OK, MessageBoxIcon.Error)
		Finally
			Conn.Close()
		End Try

		Return ""
	End Function

#End Region

End Class

Public Class ClsDbFunc

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	Dim _ClsReg As New SPProgUtility.ClsDivReg

	'Private strMDIniFile As String = _ClsProgSetting.GetMDIniFile()

	'Dim strMDProgFile As String = _ClsProgSetting.GetMDIniFile()
	'Dim strInitProgFile As String = _ClsProgSetting.GetInitIniFile()
	Private m_md As Mandant
	Private m_utility As Utilities


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


#Region "Funktionen zur Suche nach Daten..."




	Sub GetJobNr4Print(ByVal frmTest As frmOPSearch)

		Try
			Dim strPrintNr As String = String.Empty

			'0 - Alle Rechnungen
			'1 - Offene/Teil-Offene Rechnungen
			'2 - Gebuchte (bezahlte) Rechnungen
			'3 - Gemahnte Rechnungen
			'4 - Gemahnte Rechnungen aber offene Debitoren
			'5 - MwSt.-pflichtige Liste aller Rechnungen
			'6 - MwSt.-freie Liste aller Rechnungen

			With frmTest
				Select Case CInt(Left(.Cbo_ListingArt.Text, 1))
					Case 0      ' Alle Rechnungen
						strPrintNr = "7.0"
						ClsDataDetail.ListBez = "Liste aller Rechnungen"

					Case 1      ' Offene/Teil-Offene Rechnungen
						strPrintNr = "7.5"                ' Liste aller offene/teiloffenen Rechnungen
						ClsDataDetail.ListBez = "Liste aller offenen und teiloffenen Rechnungen"

					Case 2      ' Gebuchte (bezahlte) Rechnungen
						strPrintNr = "7.6"                ' Liste aller bezahlten Rechnungen
						ClsDataDetail.ListBez = "Liste aller bezahlten Rechnungen"

					Case 3       ' Gemahnte Rechnungen
						strPrintNr = "7.7"                ' Liste aller aktuell gemahnten Rechnungen
						ClsDataDetail.ListBez = "Liste aller gemahnten Rechnungen"

					Case 4       ' Gemahnte Rechnungen offene Debitoren
						strPrintNr = "7.7"                ' Liste aller aktuell gemahnten Rechnungen
						ClsDataDetail.ListBez = "Liste aller gemahnten Rechnungen, offene Debitoren"

					Case 5      ' MwSt.-pflichtige Liste aller Rechnungen
						strPrintNr = "7.9"                ' MWST-Liste aller Rechnungen
						ClsDataDetail.ListBez = "MwSt.-pflichtige Liste aller Rechnungen"

					Case 6      ' MwSt.-freie Liste aller Rechnungen
						strPrintNr = "7.13"                ' MWST-Liste aller Rechnungen
						ClsDataDetail.ListBez = "MwSt.-freie Liste aller Rechnungen"

				End Select
			End With
			ClsDataDetail.GetModulToPrint = strPrintNr


		Catch ex As Exception
			MessageBox.Show(ex.Message, "GetJobNr4Print", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try

	End Sub

#Region "Methoden für Offene Debitorenliste..."

	'''' <summary>
	'''' Diese Funktion wird nur ausgeführt wenn die Liste für eine bestimmte Zeitspanne sein soll. Sie ist nicht für Betragsuche.
	'''' </summary>
	'''' <param name="strOpQuery">die SQL-Query</param>
	'''' <param name="frmTest">Das Forumular. (frmOPSearch)</param>
	'''' <returns></returns>
	'''' <remarks></remarks>
	'Function GetQueryForOpenRE(ByVal strOpQuery As String, ByVal frmTest As frmOPSearch) As String
	'	Dim sSql As String = String.Empty
	'	Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
	'	Dim iOPNr As Integer = 0
	'	Dim strAndString As String = String.Empty
	'	Dim strFieldName As String = String.Empty
	'	Dim strOpRENr As String = String.Empty

	'	Try
	'		Conn.Open()

	'		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strOpQuery, Conn)
	'		Dim rOPrec As SqlDataReader = cmd.ExecuteReader          ' RE-Datenbank

	'		' wurde das Datum gewählt...
	'		GetZEBetrag(rOPrec, "", frmTest)
	'		sSql = Me.GetStartSQLString(frmTest)
	'		sSql += " Where RE.RENr In (" & ClsDataDetail.GetstrOPNr4Date() & ") "
	'		sSql += String.Format("Select * From {0}", ClsDataDetail.SPTabNamenDBL) ' 04.02.2010
	'		sSql += Me.GetSortString(frmTest)

	'	Catch e As Exception
	'		MsgBox(e.Message)
	'		MessageBox.Show(e.Message, "GetQueryForOpenRE", MessageBoxButtons.OK, MessageBoxIcon.Error)
	'	Finally
	'		Conn.Close()
	'		Conn.Dispose()

	'	End Try


	'	Return sSql
	'End Function

	'Function GetSortString_2(ByVal frmTest As frmOPSearch) As String
	'	Dim strSort As String = String.Empty
	'	Try
	'		Dim strSortBez As String = String.Empty

	'		With frmTest
	'			strSort = " Order by RENr, Fak_Dat"

	'		End With
	'		ClsDataDetail.GetSortValue_2 = strSortBez
	'	Catch ex As Exception
	'		MessageBox.Show(ex.Message, "GetSortString_2", MessageBoxButtons.OK, MessageBoxIcon.Error)
	'	End Try


	'	Return strSort
	'End Function



#End Region

	'Function GetLstItems(ByVal lst As ListBox) As String
	'  Dim strBerufItems As String = String.Empty
	'  Try
	'    For i = 0 To lst.Items.Count - 1
	'      strBerufItems += lst.Items(i).ToString & ","
	'    Next
	'  Catch ex As Exception
	'    MessageBox.Show(ex.Message, "GetLstItems", MessageBoxButtons.OK, MessageBoxIcon.Error)
	'  End Try


	'  Return Left(strBerufItems, Len(strBerufItems) - 1)
	'End Function


#End Region


End Class

''' <summary>
''' Klasse für die ComboBox, um Text und Wert zu haben.
''' Das Item wird mit den Parameter Text für die Anzeige und
''' Value für den Wert zur ComboBox hinzugefügt.
''' </summary>
''' <remarks></remarks>
Class ComboBoxItem
	Public Text As String
	Public Value As String
	Public Sub New(ByVal text As String, ByVal val As String)
		Me.Text = text
		Me.Value = val
	End Sub
	Public Overrides Function ToString() As String
		Return Text
	End Function
End Class

'Module MyComboBoxExtensions
'  <Extension()> _
' _
'   Public Function ToItem(ByVal cbo As myCbo) As ComboBoxItem
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