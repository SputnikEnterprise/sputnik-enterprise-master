
Imports System.IO
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.Utility

Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities

Imports SPYFakListSearch.ClsDataDetail


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

#End Region

#Region "Funktionen für LvClick in der Suchmaske..."

  '// RPNr
  Dim _strRPNr As String
  Public Property GetRPNr() As String
    Get
      Return _strRPNr
    End Get
    Set(ByVal value As String)
      _strRPNr = value
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

  '// ESNr
  Dim _strESNr As String
  Public Property GetESNr() As String
    Get
      Return _strESNr
    End Get
    Set(ByVal value As String)
      _strESNr = value
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

	Private _ClsFunc As New ClsDivFunc
	Private _ClsDataSaver As New DbDataSaver

	Private _ClsReg As New SPProgUtility.ClsDivReg
	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath


	Protected Shared m_Logger As ILogger = New Logger()

	Private m_md As Mandant

	Private m_common As CommonSetting
	Protected m_utility As Utilities
	Protected m_UtilityUi As SP.Infrastructure.UI.UtilityUI

	Private m_SearchCriteria As New SearchCriteria

#End Region


#Region "Contructor"

	Public Sub New(ByVal _search As SearchCriteria)

		m_md = New Mandant
		m_utility = New Utilities
		m_UtilityUi = New UtilityUI

		m_SearchCriteria = _search

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


#Region "Funktionen zur Suche nach Daten..."

	Function GetStartSQLString() As String
		Dim sSql As String = String.Empty
		Dim sSqlLen As Integer = 0
		Dim sZusatzBez As String = String.Empty
		Dim i As Integer = 0

		DeleteRecFromDb(m_SearchCriteria.jahr)
		sSql = "Select LOL.MANr, LOL.Jahr, LOL.LANr, Sum(LOL.m_btr) As Betrag From LOL Left Join Mitarbeiter MA On LOL.MANr = MA.MANr "

		Return sSql

	End Function

  Function GetQuerySQLString(ByVal sSQLQuery As String, ByVal frmTest As frmYFakListSearch) As String
    Dim sSql As String = String.Empty
    Dim sOldQuery As String = sSQLQuery
    Dim strFieldName As String = String.Empty

    Dim FilterBez As String = String.Empty
    Dim sSqlLen As Integer = 0
    Dim sZusatzBez As String = String.Empty
    Dim strAndString As String = String.Empty

		Dim strUSFiliale As String = m_InitialData.UserData.UserFiliale
    Dim iSQLLen As Integer = Len(sSQLQuery)

    Dim strName As String()
    Dim strMyName As String = String.Empty

    With frmTest
      ' Lohnartennummer ----------------------------------------------------------------------------------------------
      strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
      strFieldName = "LOL.Lanr"
      sZusatzBez = "3600, 3602, 3650, 3700, 3750, 3800, 3850, 3900, 3900.1, 3901, 3901.1"
      strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
      sSql += strAndString & strFieldName & " In (" & sZusatzBez & ")"


      ' Jahr -------------------------------------------------------------------------------------------------------
      strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
      strFieldName = "LOL.Jahr"
      If UCase(.Cbo_Year.Text) <> String.Empty Then
        sZusatzBez = .Cbo_Year.Text.Trim
        FilterBez += "Jahr wie (" & sZusatzBez & ") " & vbLf

        strName = Regex.Split(sZusatzBez.Trim, ",")
        strMyName = String.Empty
        For i As Integer = 0 To strName.Length - 1
          strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
        Next
        If strName.Length > 0 Then sZusatzBez = strMyName

        sSql += strAndString & strFieldName & " In (" & sZusatzBez & ")"
      End If

      ' Kanton -------------------------------------------------------------------------------------------------------
      strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
      strFieldName = "LOL.S_Kanton"
      If UCase(.Cbo_Kanton.Text) <> String.Empty Then
        sZusatzBez = .Cbo_Kanton.Text.Trim
        FilterBez += "Kanton wie (" & sZusatzBez & ") " & vbLf

        If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
          sSql += strAndString & " (" & strFieldName & " = '' Or " & strFieldName & " Is Null) "
        Else

          strName = Regex.Split(sZusatzBez.Trim, ",")
          strMyName = String.Empty
          For i As Integer = 0 To strName.Length - 1
            strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
          Next
          If strName.Length > 0 Then sZusatzBez = strMyName

          If InStr(sZusatzBez.Trim, ",") > 0 Then
            sZusatzBez = Replace(sZusatzBez.Trim, ",", "','")
          End If

          sSql += strAndString & strFieldName & " In ('" & sZusatzBez.Trim & "')"
        End If

      End If

      ' Filiale -------------------------------------------------------------------------------------------------------
      strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
      strFieldName = "MA.Kst"
      If UCase(.Cbo_Filiale.Text) <> String.Empty Then
        sZusatzBez = .Cbo_Filiale.Text.Trim
        FilterBez += "Filiale wie (" & sZusatzBez & ") " & vbLf

        If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
          sSql += strAndString & " (" & strFieldName & " = '' Or " & strFieldName & " Is Null) "
        Else

          sZusatzBez = GetFilialKstData(sZusatzBez)
          sZusatzBez = Replace(sZusatzBez, "'", "")
          strName = Regex.Split(sZusatzBez.Trim, ",")
          strMyName = String.Empty
          For i As Integer = 0 To strName.Length - 1
            If strName(i).Trim <> String.Empty Then
              strMyName += CStr(IIf(strMyName.Length > 0, " Or ", "")) & strFieldName & " = '" & strName(i).Trim & "'"
            End If
          Next
          If strName.Length > 0 Then sZusatzBez = strMyName

          sSql += strAndString & " (" & sZusatzBez & ")"
        End If

      End If

      ' Filialen Teilung...
      strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
      If strUSFiliale <> "" Then
        strFieldName = "MA.Kst"
        If UCase(strUSFiliale) <> String.Empty Then
          sZusatzBez = strUSFiliale
          FilterBez += "Filiale wie (" & sZusatzBez & ") " & vbLf

          sZusatzBez = GetFilialKstData(sZusatzBez)
          sZusatzBez = Replace(sZusatzBez, "'", "")
          strName = Regex.Split(sZusatzBez.Trim, ",")
          strMyName = String.Empty
          For i As Integer = 0 To strName.Length - 1
            strMyName += CStr(IIf(strMyName.Length > 0, " Or ", "")) & strFieldName & " Like '%" & strName(i).Trim & "%'"
          Next
          If strName.Length > 0 Then sZusatzBez = strMyName

          sSql += strAndString & " (" & sZusatzBez & ")"

        End If
      End If


			' Mandant -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			strFieldName = "LOL.MDNr"
			If Not .lueMandant.EditValue Is Nothing Then
				sZusatzBez = .lueMandant.Text.Trim
				FilterBez += "Mandant wie (" & sZusatzBez & ") " & vbLf

				sSql += String.Format("{0}{1} = {2} ", strAndString, strFieldName, m_InitialData.MDData.MDNr)
			End If


      ' Gruppierung der Datensätze
      sSql += " Group By LOL.MANr, LOL.Jahr, LOL.LANr "


    End With
    ClsDataDetail.GetFilterBez = FilterBez

    Return sSql
  End Function

	Function GetStartSQLString_2() As String
		Dim Sql As String = String.Empty

		Sql &= "Select Db.MANr, Db.Jahr, Db.USNr, SUM(Db._3600) AS _3600, SUM(Db._3602) AS _3602, "
		Sql &= "SUM(Db._3650) AS _3650, SUM(Db._3700) AS _3700, SUM(Db._3750) AS _3750, SUM(Db._3800) AS _3800, SUM(Db._3850) AS _3850, SUM(Db._3900) AS _3900, SUM(Db._3900_1) AS _3900_1, "
		Sql &= "SUM(Db._3901) AS _3901, SUM(Db._3901_1) AS _3901_1, "
		Sql &= "DB.MDNr, MA.Nachname As MANachname, MA.Vorname As MAVorname, MA.Zivilstand, MA.GebDat, MA.AHV_Nr, MA.AHV_Nr_New "
		Sql &= String.Format("From _KiAuZulage_{0} Db ", m_InitialData.UserData.UserNr)
		Sql &= "Left Join Mitarbeiter MA On Db.MANr = MA.MANr "

		Sql &= "Where DB.MDNr = " & m_InitialData.MDData.MDNr & " And Db.USNr = " & m_InitialData.UserData.UserNr & " And Db.Jahr = " & m_SearchCriteria.jahr & " "
		Sql &= "GROUP BY Db.MANr, Db.Jahr, Db.USNr,	DB.MDNr, MA.Nachname, MA.Vorname, MA.Zivilstand, MA.GebDat, MA.AHV_Nr, MA.AHV_Nr_New "

		Return Sql

	End Function

	Function GetSortString() As String
		Dim strSort As String = " Order By "
		Dim strSortBez As String = String.Empty
		Dim strMyName As String = String.Empty

		strMyName = "LOL.MANr, LOL.Jahr, LOL.LANr"
		strSortBez = CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Mitarbeiternummer, Jahr, Lohnarten"

		strSort = strSort & strMyName
		ClsDataDetail.GetSortBez = strSortBez

		Return strSort
	End Function

	Function GetSortString_2() As String
		Dim strSort As String = " Order By "
		Dim strSortBez As String = String.Empty
		Dim strMyName As String = String.Empty

		strMyName = "MA.Nachname, MA.Vorname"
		strSortBez = CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Mitarbeitername"

		strSort = strSort & strMyName
		ClsDataDetail.GetSortBez = strSortBez

		Return strSort
	End Function


#End Region

#Region "Sonstige Funktionen für Datenbank..."


	Function BuildYFakDb(ByVal strQuery As String) As Boolean
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim success As Boolean = True
		Dim iMANr As Integer? = 0
		Dim iOldMANr As Integer? = 0
		Dim strYear As Integer? = 0
		Dim clanr As Decimal? = 0
		Dim cBetrag As Decimal? = 0

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand
			cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
			Dim rYFakrec As SqlDataReader = cmd.ExecuteReader

			If rYFakrec.HasRows() Then
				'While True

				While rYFakrec.Read

					_ClsDataSaver.GetLA_3600 = 0
					_ClsDataSaver.GetLA_3602 = 0
					_ClsDataSaver.GetLA_3650 = 0
					_ClsDataSaver.GetLA_3700 = 0
					_ClsDataSaver.GetLA_3750 = 0
					_ClsDataSaver.GetLA_3800 = 0
					_ClsDataSaver.GetLA_3850 = 0
					_ClsDataSaver.GetLA_3900 = 0
					_ClsDataSaver.GetLA_3900_1 = 0
					_ClsDataSaver.GetLA_3901 = 0
					_ClsDataSaver.GetLA_3901_1 = 0

					iMANr = m_utility.SafeGetInteger(rYFakrec, "MANr", 0)
					clanr = m_utility.SafeGetDecimal(rYFakrec, "LANr", 0)
					cBetrag = m_utility.SafeGetDecimal(rYFakrec, "Betrag", 0)

					If iMANr = 15021 Or iMANr = 15310 Or iMANr = 9785 Then
						Trace.WriteLine(iMANr)
					End If

					'If iMANr <> iOldMANr AndAlso iOldMANr > 0 Then

					Select Case clanr
						Case 3600
							_ClsDataSaver.GetLA_3600 = cBetrag

						Case 3602
							_ClsDataSaver.GetLA_3602 = cBetrag

						Case 3650
							_ClsDataSaver.GetLA_3650 = cBetrag

						Case 3700
							_ClsDataSaver.GetLA_3700 = cBetrag

						Case 3750
							_ClsDataSaver.GetLA_3750 = cBetrag

						Case 3800
							_ClsDataSaver.GetLA_3800 = cBetrag

						Case 3850
							_ClsDataSaver.GetLA_3850 = cBetrag

						Case 3900
							_ClsDataSaver.GetLA_3900 = cBetrag

						Case 3900.1
							_ClsDataSaver.GetLA_3900_1 = cBetrag

						Case 3901
							_ClsDataSaver.GetLA_3901 = cBetrag

						Case 3901.1
							_ClsDataSaver.GetLA_3901_1 = cBetrag


					End Select

					success = GetInsertString(iMANr)


					If Not success Then Return False
					'iOldMANr = m_utility.SafeGetInteger(rYFakrec, "MANr", 0)

					'End If


					'If iMANr = iOldMANr OrElse iOldMANr = 0 Then
					'	iOldMANr = m_utility.SafeGetInteger(rYFakrec, "MANr", 0)

					'	Select Case clanr
					'		Case 3600
					'			_ClsDataSaver.GetLA_3600 = cBetrag

					'		Case 3602
					'			_ClsDataSaver.GetLA_3602 = cBetrag

					'		Case 3650
					'			_ClsDataSaver.GetLA_3650 = cBetrag

					'		Case 3700
					'			_ClsDataSaver.GetLA_3700 = cBetrag

					'		Case 3750
					'			_ClsDataSaver.GetLA_3750 = cBetrag

					'		Case 3800
					'			_ClsDataSaver.GetLA_3800 = cBetrag

					'		Case 3850
					'			_ClsDataSaver.GetLA_3850 = cBetrag

					'		Case 3900
					'			_ClsDataSaver.GetLA_3900 = cBetrag

					'		Case 3900.1
					'			_ClsDataSaver.GetLA_3900_1 = cBetrag

					'		Case 3901
					'			_ClsDataSaver.GetLA_3901 = cBetrag

					'		Case 3901.1
					'			_ClsDataSaver.GetLA_3901_1 = cBetrag


					'	End Select

					'	'If iMANr <> m_utility.SafeGetInteger(rYFakrec, "MANr", 0) Then Exit While
					'	'rYFakrec.Read()

					'End If

				End While


				'' Datensatz hinzufügen...
				'If iMANr = 15567 Or iMANr = 15559 Then
				'	Trace.WriteLine(iMANr)
				'End If
				'If _ClsDataSaver.GetLA_3600.GetValueOrDefault(0) + _ClsDataSaver.GetLA_3602.GetValueOrDefault(0) + _ClsDataSaver.GetLA_3650.GetValueOrDefault(0) +
				'	_ClsDataSaver.GetLA_3700.GetValueOrDefault(0) + _ClsDataSaver.GetLA_3750.GetValueOrDefault(0) +
				'	_ClsDataSaver.GetLA_3800.GetValueOrDefault(0) + _ClsDataSaver.GetLA_3850.GetValueOrDefault(0) +
				'	_ClsDataSaver.GetLA_3900.GetValueOrDefault(0) + _ClsDataSaver.GetLA_3900_1.GetValueOrDefault(0) + _ClsDataSaver.GetLA_3901.GetValueOrDefault(0) +
				'	_ClsDataSaver.GetLA_3901_1.GetValueOrDefault(0) <> 0 Then success = GetInsertString(iMANr)

				'_ClsDataSaver.GetLA_3600 = 0
				'_ClsDataSaver.GetLA_3602 = 0
				'_ClsDataSaver.GetLA_3650 = 0

				'_ClsDataSaver.GetLA_3700 = 0
				'_ClsDataSaver.GetLA_3750 = 0
				'_ClsDataSaver.GetLA_3800 = 0
				'_ClsDataSaver.GetLA_3850 = 0
				'_ClsDataSaver.GetLA_3900 = 0
				'_ClsDataSaver.GetLA_3900_1 = 0
				'_ClsDataSaver.GetLA_3901 = 0
				'_ClsDataSaver.GetLA_3901_1 = 0

				'If Not success Then Exit Function

				'Try
				'	'If IsDBNull(rYFakrec("MANr")) Then Exit While

				'Catch ex As Exception
				'	'MsgBox(ex.Message)
				'	Exit While
				'End Try

				'Exit While

				'End While
			End If


		Catch e As Exception
			m_UtilityUi.ShowErrorDialog(e.ToString)

			Return False

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return True

	End Function


	'Function BuildYFakDb(ByVal employeeNumber As Integer) As Boolean
	'	Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
	'	Dim success As Boolean = True
	'	Dim iMANr As Integer? = 0
	'	Dim strYear As String = String.Empty
	'	Dim clanr As Decimal? = 0
	'	Dim cBetrag As Decimal = 0

	'	Try
	'		Conn.Open()

	'		Dim cmd As System.Data.SqlClient.SqlCommand
	'		cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
	'		Dim param As System.Data.SqlClient.SqlParameter
	'		param = cmd.Parameters.AddWithValue("@MANr", employeeNumber)

	'		Dim rYFakrec As SqlDataReader = cmd.ExecuteReader

	'		If rYFakrec.HasRows() Then
	'			While True

	'				If Not iMANr.HasValue OrElse iMANr = 0 Then rYFakrec.Read()
	'				iMANr = m_utility.SafeGetInteger(rYFakrec, "MANr", 0)

	'				strYear = CStr(rYFakrec("Jahr").ToString)
	'				clanr = m_utility.SafeGetDecimal(rYFakrec, "LANr", 0)	' CDec(rYFakrec("LANr").ToString)
	'				cBetrag = CDec(rYFakrec("Betrag").ToString)

	'				While rYFakrec.Read

	'					If iMANr = m_utility.SafeGetInteger(rYFakrec, "MANr", 0) Then
	'						Select Case clanr	 ' m_utility.SafeGetDecimal(rYFakrec, "LANr", 0)
	'							Case 3600
	'								_ClsDataSaver.GetLA_3600 = m_utility.SafeGetDecimal(rYFakrec, "Betrag", 0)

	'							Case 3602
	'								_ClsDataSaver.GetLA_3602 = m_utility.SafeGetDecimal(rYFakrec, "Betrag", 0)

	'							Case 3650
	'								_ClsDataSaver.GetLA_3650 = m_utility.SafeGetDecimal(rYFakrec, "Betrag", 0)

	'							Case 3700
	'								_ClsDataSaver.GetLA_3700 = m_utility.SafeGetDecimal(rYFakrec, "Betrag", 0)

	'							Case 3750
	'								_ClsDataSaver.GetLA_3750 = m_utility.SafeGetDecimal(rYFakrec, "Betrag", 0)

	'							Case 3800
	'								_ClsDataSaver.GetLA_3800 = m_utility.SafeGetDecimal(rYFakrec, "Betrag", 0)

	'							Case 3850
	'								_ClsDataSaver.GetLA_3850 = m_utility.SafeGetDecimal(rYFakrec, "Betrag", 0)

	'							Case 3900
	'								_ClsDataSaver.GetLA_3900 = m_utility.SafeGetDecimal(rYFakrec, "Betrag", 0)

	'							Case 3900.1
	'								_ClsDataSaver.GetLA_3900_1 = m_utility.SafeGetDecimal(rYFakrec, "Betrag", 0)

	'							Case 3901
	'								_ClsDataSaver.GetLA_3901 = m_utility.SafeGetDecimal(rYFakrec, "Betrag", 0)

	'							Case 3901.1
	'								_ClsDataSaver.GetLA_3901_1 = m_utility.SafeGetDecimal(rYFakrec, "Betrag", 0)


	'						End Select

	'						'If iMANr <> m_utility.SafeGetInteger(rYFakrec, "MANr", 0) Then Exit While
	'						rYFakrec.Read()

	'						Try
	'							'	If Not m_utility.SafeGetInteger(rYFakrec, "MANr", Nothing).HasValue Then Exit While

	'						Catch ex As Exception
	'							Exit While
	'						End Try

	'					Else
	'						Exit While
	'					End If

	'				End While


	'				' Datensatz hinzufügen...
	'				If iMANr = 15567 Or iMANr = 15559 Then
	'					Trace.WriteLine(iMANr)
	'				End If
	'				success = GetInsertString(iMANr, clanr, cBetrag)

	'				_ClsDataSaver.GetLA_3600 = 0
	'				_ClsDataSaver.GetLA_3602 = 0
	'				_ClsDataSaver.GetLA_3650 = 0
	'				_ClsDataSaver.GetLA_3700 = 0
	'				_ClsDataSaver.GetLA_3750 = 0
	'				_ClsDataSaver.GetLA_3800 = 0
	'				_ClsDataSaver.GetLA_3850 = 0
	'				_ClsDataSaver.GetLA_3900 = 0
	'				_ClsDataSaver.GetLA_3900_1 = 0
	'				_ClsDataSaver.GetLA_3901 = 0
	'				_ClsDataSaver.GetLA_3901_1 = 0

	'				If Not success Then Exit Function

	'				Try
	'					If IsDBNull(rYFakrec("MANr")) Then Exit While

	'				Catch ex As Exception
	'					'MsgBox(ex.Message)
	'					Exit While
	'				End Try

	'			End While
	'		End If


	'	Catch e As Exception
	'		m_UtilityUi.ShowErrorDialog(e.ToString)

	'		Return False

	'	Finally
	'		Conn.Close()
	'		Conn.Dispose()

	'	End Try

	'	Return True

	'End Function






	Function GetInsertString(ByVal iMANr As Integer?) As Boolean
		Dim result As Boolean = True
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim i As Integer = 0

		' KiAuZulage_Year
		Dim strResult As String = "Insert Into _KiAuZulage_{0} (MANr, Jahr, USNr, _3600, _3602, _3650, "
		strResult += "_3700, _3750, "
		strResult += "_3800, _3850, _3900, _3900_1, _3901, _3901_1, CreatedFrom, CreatedOn, MDNr) "
		strResult += "Values (@MANr, @Jahr, @USNr, "
		strResult += "@_3600, @_3602, @_3650, "
		strResult += "@_3700, @_3750, "
		strResult += "@_3800, @_3850, @_3900, @_3900_1, @_3901, @_3901_1, "
		strResult += "@CreatedFrom, @CreatedOn, @MDNr)"
		strResult = String.Format(strResult, m_InitialData.UserData.UserNr)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand
			cmd = New System.Data.SqlClient.SqlCommand(strResult, Conn)

			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@MANr", iMANr)
			param = cmd.Parameters.AddWithValue("@Jahr", m_SearchCriteria.jahr)
			param = cmd.Parameters.AddWithValue("@USNr", m_InitialData.UserData.UserNr)

			param = cmd.Parameters.AddWithValue("@_3600", m_utility.ReplaceMissing(_ClsDataSaver.GetLA_3600, 0))
			param = cmd.Parameters.AddWithValue("@_3602", m_utility.ReplaceMissing(_ClsDataSaver.GetLA_3602, 0))
			param = cmd.Parameters.AddWithValue("@_3650", m_utility.ReplaceMissing(_ClsDataSaver.GetLA_3650, 0))
			param = cmd.Parameters.AddWithValue("@_3700", m_utility.ReplaceMissing(_ClsDataSaver.GetLA_3700, 0))
			param = cmd.Parameters.AddWithValue("@_3750", m_utility.ReplaceMissing(_ClsDataSaver.GetLA_3750, 0))
			param = cmd.Parameters.AddWithValue("@_3800", m_utility.ReplaceMissing(_ClsDataSaver.GetLA_3800, 0))
			param = cmd.Parameters.AddWithValue("@_3850", m_utility.ReplaceMissing(_ClsDataSaver.GetLA_3850, 0))
			param = cmd.Parameters.AddWithValue("@_3900", m_utility.ReplaceMissing(_ClsDataSaver.GetLA_3900, 0))
			param = cmd.Parameters.AddWithValue("@_3900_1", m_utility.ReplaceMissing(_ClsDataSaver.GetLA_3900_1, 0))
			param = cmd.Parameters.AddWithValue("@_3901", m_utility.ReplaceMissing(_ClsDataSaver.GetLA_3901, 0))
			param = cmd.Parameters.AddWithValue("@_3901_1", m_utility.ReplaceMissing(_ClsDataSaver.GetLA_3901_1, 0))

			param = cmd.Parameters.AddWithValue("@CreatedFrom", m_InitialData.UserData.UserFullNameWithComma)
			param = cmd.Parameters.AddWithValue("@CreatedOn", Now.Date)
			param = cmd.Parameters.AddWithValue("@MDNr", m_InitialData.MDData.MDNr)


			cmd.ExecuteNonQuery()			' Datensatz hinzufügen...
			result = True


		Catch e As Exception
			m_UtilityUi.ShowErrorDialog(e.ToString)
			Return False

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return result

	End Function

#End Region


  Function GetLstItems(ByVal lst As ListBox) As String
    Dim strBerufItems As String = String.Empty

    For i = 0 To lst.Items.Count - 1
      strBerufItems += lst.Items(i).ToString & ","
    Next

    Return Left(strBerufItems, Len(strBerufItems) - 1)
  End Function


End Class

Class DbDataSaver

	'Public Sub New()

	'   For i As Integer = 0 To 8
	'     Me.GetLAValues(i) = 0
	'   Next

	' End Sub

	''Dim _LANr(8) As Decimal
	'Property GetLAValues() As Decimal
	''	Get
	''		Return _LANr
	''	End Get
	''	Set(ByVal value As Decimal?)
	''		_LANr = value
	''	End Set
	''End Property

	Property GetLA_3600() As Decimal?
	Property GetLA_3602() As Decimal?
	Property GetLA_3650() As Decimal?
	Property GetLA_3700() As Decimal?
	Property GetLA_3750() As Decimal?
	Property GetLA_3800() As Decimal?
	Property GetLA_3850() As Decimal?
	Property GetLA_3900() As Decimal?
	Property GetLA_3900_1() As Decimal?
	Property GetLA_3901() As Decimal?
	Property GetLA_3901_1() As Decimal?

End Class