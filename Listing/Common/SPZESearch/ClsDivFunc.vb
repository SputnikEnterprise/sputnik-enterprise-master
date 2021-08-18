
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions

Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports SP.Infrastructure.Logging

Public Class ClsDivFunc

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Public Property GetSelektion As String


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

	'// Parameter für die Suchmaske
	Dim _strParam As String
	Public Property Param() As String
		Get
			Return _strParam
		End Get
		Set(ByVal value As String)
			_strParam = value
		End Set
	End Property

	'// ZENr
	Dim _strZENr As String
	Public Property GetZENr() As String
		Get
			Return _strZENr
		End Get
		Set(ByVal value As String)
			_strZENr = value
		End Set
	End Property

	'// RENr
	Dim _strRENr As String
	Public Property GetRENr() As String
		Get
			Return _strRENr
		End Get
		Set(ByVal value As String)
			_strRENr = value
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

	'// Firmennamen
	Dim _strFirmennamen As String
	Public Property GetFirmennamen() As String
		Get
			Return _strFirmennamen
		End Get
		Set(ByVal value As String)
			_strFirmennamen = value
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
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)
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

	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private m_Mandant As Mandant
	Private m_Utility_SP As SPProgUtility.MainUtilities.Utilities

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		m_Utility_SP = New Utilities

		' Mandantendaten
		m_Mandant = New Mandant

		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

	End Sub

	''' <summary>
	''' listet eine Auflistung der Mandantendaten
	''' </summary>
	''' <returns></returns>
	''' <remarks></remarks>
	Function LoadMandantenData() As IEnumerable(Of MandantenData)
		Dim result As List(Of MandantenData) = Nothing

		Dim sql As String = "[Mandanten. Get All Allowed MDData]"

		Dim reader As SqlClient.SqlDataReader = m_Utility_SP.OpenReader(ClsDataDetail.m_InitialData.MDData.MDDbConn, sql, Nothing, CommandType.StoredProcedure)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of MandantenData)

				While reader.Read()
					Dim recData As New MandantenData

					recData.MDNr = CInt(m_Utility_SP.SafeGetInteger(reader, "MDNr", 0))
					recData.MDName = m_Utility_SP.SafeGetString(reader, "MDName")
					recData.MDGuid = m_Utility_SP.SafeGetString(reader, "MDGuid")
					recData.MDConnStr = m_Mandant.GetSelectedMDData(recData.MDNr).MDDbConn

					result.Add(recData)

				End While

			End If

		Catch e As Exception
			result = Nothing
			m_Logger.LogError(e.ToString())

		Finally
			m_Utility_SP.CloseReader(reader)

		End Try

		Return result

	End Function


#Region "Funktionen zur Suche nach Daten..."


	Public Function GetSortString(ByVal frmTest As frmZESearch) As String
		Dim strSort As String = " order by "
		Try
			Dim strSortBez As String = String.Empty
			Dim strName As String()
			Dim strMyName As String = String.Empty


			'0 - Zahlungsnummer
			'1 - Rechnungsnummer
			'2 - Kundennummer
			'3 - Rechnungsempfänger
			'4 - Valutadatum (Aufsteigend)
			'5 - Valutadatum (Absteigend)
			'6 - Buchungsdatum (Aufsteigend)
			'7 - Buchungsdatum (Absteigend)
			'8 - Betrag (Aufsteigend)
			'9 - Betrag (Absteigend)
			'Else - Rechnungsempfänger und Valutadatum (Aufsteigend)

			With frmTest
				strName = Regex.Split(.CboSort.Text.Trim, ",")
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					Select Case CInt(Val(Left(strName(i).ToString.Trim, 1)))

						Case 0      ' Nach Zahlungsnummer
							If Left(strName(i).ToString.Trim, 1) <> "0" Then
								strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " " & strName(i).ToString.Trim
							Else

								strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & "ZENR ASC"
								strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & m_Translate.GetSafeTranslationValue("Zahlungsnummer und Valutadatum")
							End If

						Case 1      ' Rechnungsnummer
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & "RENR ASC"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & m_Translate.GetSafeTranslationValue("Rechnungsnummer und Valutadatum")

						Case 2      ' Kundennummer
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & "KDNR ASC"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & m_Translate.GetSafeTranslationValue("Kundennummer und Valutadatum")

						Case 3      ' Rechnungsempfänger
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & "R_Name1 ASC"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & m_Translate.GetSafeTranslationValue("Rechnungsempfänger")

						Case 4      ' Valutadatum (Aufsteigend)
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & "V_Date ASC"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & m_Translate.GetSafeTranslationValue("Valutadatum (Aufsteigend)")

						Case 5      ' Valutadatum (Absteigend)
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & "V_Date DESC"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & m_Translate.GetSafeTranslationValue("Valutadatum (Absteigend)")

						Case 6      ' Buchungsdatum (Aufsteigend)
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & "B_Date ASC"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & m_Translate.GetSafeTranslationValue("Buchungsdatum (Aufsteigend)")

						Case 7      ' Buchungsdatum (Absteigend)
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & "B_Date DESC"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & m_Translate.GetSafeTranslationValue("Buchungsdatum (Absteigend)")

						Case 8      ' Betrag (Aufsteigend)
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & "Betrag ASC"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & m_Translate.GetSafeTranslationValue("Betrag (Absteigend)")

						Case 9      ' Betrag (Absteigend)
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & "Betrag DESC"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & m_Translate.GetSafeTranslationValue("Betrag (Absteigend)")

						Case Else   ' Rechnungsempfänger und Valutadatum (Aufsteigend)
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & "R_Name1, V_Date ASC"
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & m_Translate.GetSafeTranslationValue("Rechnungsempfänger und Valutadatum (Aufsteigend)")

					End Select
					'End If
				Next

			End With
			If strMyName.Trim.Length = 0 Then
				strMyName = " R_Name1, V_Date ASC"
				strSortBez = " " & m_Translate.GetSafeTranslationValue("Rechnungsempfänger und Valutadatum (Aufsteigend)")
			End If
			strSort = String.Format("{0} {1}", strSort, strMyName)
			ClsDataDetail.GetSortBez = strSortBez


		Catch ex As Exception
			MessageBox.Show(ex.Message, "GetSortString", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try

		Return strSort
	End Function

	Public Shared Sub SetJobNr4Print(ByVal frmTest As frmZESearch)

		Try
			Dim strPrintNr As String = String.Empty
			strPrintNr = "6.0" ' Im Moment gibt es nur eine Liste
			ClsDataDetail.GetModulToPrint = strPrintNr

		Catch ex As Exception
			MessageBox.Show(ex.Message, "GetJobNr4Print", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try

	End Sub

#End Region


End Class

