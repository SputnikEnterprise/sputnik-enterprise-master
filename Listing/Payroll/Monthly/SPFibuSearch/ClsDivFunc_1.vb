
Imports SP.Infrastructure.Logging
Imports System.Data.SqlClient

Imports SPFibuSearch.ClsDataDetail


Public Class ClsDivFunc_2

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

  '// Print.LLExporterFileName
  Dim _LLPrintInDiffColor As Boolean
  Public Property LLPrintDiffColor() As Boolean
    Get
      Return _LLPrintInDiffColor
    End Get
    Set(ByVal value As Boolean)
      _LLPrintInDiffColor = value
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

#Region "Funktionen..."

  Sub GetBetragSign()
    Dim bResult As Boolean
    Dim _ClsSystem As New SPProgUtility.ClsProgSettingPath
    Dim _ClsReg As New SPProgUtility.ClsDivReg
    Dim strQuery As String = "//SPSZGSearch/ZGSearch/SQLString[@ID=" & Chr(34) & ClsDataDetail.GetAppGuidValue() & Chr(34) & "]/BetragSign"

    Dim strBez As String = _ClsReg.GetXMLNodeValue(_ClsSystem.GetSQLDataFile(), strQuery)
    If strBez <> String.Empty Then
      If strBez = CStr(1) Then bResult = True
    End If

  End Sub

#End Region

End Class

Public Class ClsDbFunc_1

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	Dim _ClsFunc As New ClsDivFunc
  Dim _ClsReg As New SPProgUtility.ClsDivReg
  Dim _ClsSystem As New SPProgUtility.ClsProgSettingPath

  Dim LioFilialen As New List(Of String)

#Region "Methoden..."

  Dim _strKst As String
  Public Property GetKst() As String
    Get
      Return _strKst
    End Get
    Set(ByVal value As String)
      _strKst = value
    End Set
  End Property

  Dim _iLONr As Integer
  Public Property GetLONr() As Integer
    Get
      Return _iLONr
    End Get
    Set(ByVal value As Integer)
      _iLONr = value
    End Set
  End Property

  Dim _sMonth As Short
  Public Property GetMonth() As Short
    Get
      Return _sMonth
    End Get
    Set(ByVal value As Short)
      _sMonth = value
    End Set
  End Property

  Dim _strYear As String
  Public Property GetMDYear() As String
    Get
      Return _strYear
    End Get
    Set(ByVal value As String)
      _strYear = value
    End Set
  End Property

  Dim _LioLOLData As List(Of Double)
  Property GetLOLData() As List(Of Double)

    '_LioLOLData(0) = Val(rLOrec("AHVTotal").ToString)
    '_LioLOLData(1) = Val(rLOrec("AHVKSTBasis").ToString)
    '_LioLOLData(2) = Val(rLOrec("AHVProz").ToString)
    Get
      Return _LioLOLData
    End Get
    Set(ByVal value As List(Of Double))
      _LioLOLData = value
    End Set

  End Property

  Dim _LioLOLName As List(Of String)
  Property GetLOLString() As List(Of String)

    '_LioLOLData(0) = Val(rLOrec("AHVTotal").ToString)
    '_LioLOLData(1) = Val(rLOrec("AHVKSTBasis").ToString)
    '_LioLOLData(2) = Val(rLOrec("AHVProz").ToString)
    Get
      Return _LioLOLName
    End Get
    Set(ByVal value As List(Of String))
      _LioLOLName = value
    End Set

  End Property

#End Region

  Dim _cDbMyConn As SqlClient.SqlConnection
  Sub New(ByVal cDbMyConnection As SqlClient.SqlConnection, ByVal sMonth As Short, ByVal strYear As String)
    _cDbMyConn = cDbMyConnection
    Me._sMonth = sMonth
    Me._strYear = strYear
  End Sub

  Sub CreateTable4LOAHVBas_0(ByVal frmTest As frmFibuSearch, _
                           ByVal ifMonth As Integer, _
                           ByVal strYear As String, _
                           ByVal strFiliale As String)

    'Try
    '  If IsNothing(ClsDataDetail.Conn) Then Exit Sub
    '  Dim sSql As String = "[dbo].[Create New Table For LOLFibu_Basis]"
    '  Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, ClsDataDetail.Conn)
    '  cmd.CommandType = Data.CommandType.StoredProcedure
    '  Dim param As System.Data.SqlClient.SqlParameter
    '  param = cmd.Parameters.AddWithValue("@LP", ifMonth)
    '  param = cmd.Parameters.AddWithValue("@Jahr", strYear)
    '  param = cmd.Parameters.AddWithValue("@tblName", String.Format("_LOLFibu_{0} ", _ClsSystem.GetLogedUSGuid))

    '  Dim rLOrec As SqlDataReader = cmd.ExecuteReader


    '  Dim LioData As New List(Of Double)
    '  LioData = FillMyLOiBetrag(5)

    '  While rLOrec.Read()
    '    Me._iLONr = CInt(rLOrec("LONr").ToString)
    '    Me._strKst = rLOrec("KST").ToString

    '    If Me._iLONr = 5963 Or Me._iLONr = 6010 Then
    '      Trace.WriteLine(Me._iLONr)
    '    End If

    '    LioData(0) = Val(rLOrec("AHVTotal").ToString)
    '    LioData(1) = Val(rLOrec("AHVKSTBasis").ToString)
    '    If Val(rLOrec("AnzLO").ToString) = 1 And Val(rLOrec("AHVProz").ToString) > 1 Then
    '      LioData(2) = 1
    '    Else
    '      LioData(2) = Val(rLOrec("AHVProz").ToString)
    '    End If

    '    LioData(3) = Val(rLOrec("FilialeNr_1").ToString)
    '    LioData(4) = Val(rLOrec("FilialeNr_2").ToString)

    '    Me._LioLOLData = LioData

    '    If CDbl(rLOrec("FilialeNr_1").ToString) = 920 Or CDbl(rLOrec("FilialeNr_2").ToString) = 920 Then
    '      'Trace.WriteLine(String.Format("Lohn-Nr.: ", Me._iLONr))
    '    End If

    '    'If (Me._iLONr <> 5476 And Me._iLONr <> 5575) Then
    '    If LioData(3) <> LioData(4) Then
    '      For i As Integer = 0 To 1
    '        GetLOLDataFromLONr(LioData(2), CInt(LioData(3 + i)), _
    '                           rLOrec("Filiale_" & CStr(i + 1)).ToString, CShort(rLOrec("AnzLO").ToString))
    '      Next

    '    Else
    '      GetLOLDataFromLONr(LioData(2), CInt(LioData(3)), If(String.IsNullOrEmpty(rLOrec("Filiale_1").ToString), "???", _
    '                                                          rLOrec("Filiale_1").ToString), CShort(rLOrec("AnzLO").ToString))

    '    End If
    '    'End If

    '  End While

    '  InesrtDataToFinalDb(frmTest)

    'Catch e As SqlException
    '  MsgBox(e.StackTrace & vbNewLine & e.Message, MsgBoxStyle.Critical, "CreateTable4LOAHVBas (SqlException):")
    '  Trace.WriteLine(e.Message, "SqlException: CreateTable4LOAHVBas")

    'Catch e As Exception
    '  MsgBox(e.StackTrace & vbNewLine & e.Message, MsgBoxStyle.Critical, "CreateTable4LOAHVBas")
    '  Trace.WriteLine(e.Message, "Exception: CreateTable4LOAHVBas")

    'Finally

    'End Try

  End Sub

  Sub CreateTable4LOAHVBas(ByVal frmTest As frmFibuSearch, _
                         ByVal ifMonth As Integer, _
                         ByVal strYear As String, _
                         ByVal strFiliale As String)

    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
    Try
      Dim sCalledMethode As Short = 1
      If IsNothing(ClsDataDetail.Conn) Then Exit Sub
			Dim sSql As String = "[dbo].[Create New Table For LOLFibu_Basis With Mandant]"
      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, ClsDataDetail.Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@MDNr", m_InitialData.MDData.MDNr)
			param = cmd.Parameters.AddWithValue("@Jahr", strYear)
      param = cmd.Parameters.AddWithValue("@LP", ifMonth)
			param = cmd.Parameters.AddWithValue("@tblName", String.Format("[_LOLFibu_{0}] ", m_InitialData.UserData.UserGuid))

      Dim rLOrec As SqlDataReader = cmd.ExecuteReader
      Dim strlonr As String = String.Empty


      Dim LioData As New List(Of Double)
      Dim LioName As New List(Of String)
      LioData = FillMyLOiBetrag(5)
      LioName = FillMyLOiString(2)

      While rLOrec.Read()
        Me._iLONr = CInt(rLOrec("LONr").ToString)
        Me._strKst = rLOrec("KST").ToString
        If Me._iLONr = 15197 Then
          Trace.WriteLine(Me._iLONr)
        End If

        LioData(0) = Val(rLOrec("AHVTotal").ToString)
        LioData(1) = Val(rLOrec("AHVKSTBasis").ToString)
        If Val(rLOrec("AnzLO").ToString) = 1 And Val(rLOrec("AHVProz").ToString) > 1 Then
          LioData(2) = 1
        Else
          LioData(2) = Val(rLOrec("AHVProz").ToString)
        End If

        LioData(3) = Val(rLOrec("FilialeNr_1").ToString)
        LioData(4) = Val(rLOrec("FilialeNr_2").ToString)

        LioName(0) = rLOrec("Filiale_1").ToString
        LioName(1) = rLOrec("Filiale_2").ToString

        Me._LioLOLData = LioData
        Me._LioLOLName = LioName

        If CDbl(rLOrec("FilialeNr_1").ToString) = 920 Or CDbl(rLOrec("FilialeNr_2").ToString) = 920 Then
          '          Trace.WriteLine(String.Format("Lohn-Nr.: ", Me._iLONr))
        End If
        'If Me._iLONr = 1581 Or Me._iLONr = 1582 Or Me._iLONr = 1610 Or Me._iLONr = 1627 Or Me._iLONr = 1669 Or Me._iLONr = 1756 Or Me._iLONr = 1758 Or Me._iLONr = 1759 Or Me._iLONr = 1761 Or Me._iLONr = 1762 Or Me._iLONr = 1764 Or Me._iLONr = 1769 Or Me._iLONr = 1770 Or Me._iLONr = 1772 Or Me._iLONr = 1773 Or Me._iLONr = 1774 Or Me._iLONr = 1783 Or Me._iLONr = 1784 Then
        '  Trace.WriteLine(String.Format("Lohn-Nr.: {0}", Me._iLONr))
        'Else
        '  Trace.WriteLine(String.Format("Lohn-Nr.: {0}", Me._iLONr))

        'End If


        If Me._iLONr = 5958 Then
          Trace.WriteLine(String.Format("Lohn-Nr.: {0}", Me._iLONr))
        Else
          'Trace.WriteLine(String.Format("Lohn-Nr.: {0}", Me._iLONr))

        End If

        If strlonr.ToLower.Contains(CStr(Me._iLONr)) Then
          sCalledMethode = CShort(sCalledMethode + 1)
        Else
          sCalledMethode = 1
        End If


        strlonr &= vbNewLine
        If LioData(3) <> LioData(4) Then
          strlonr &= Me._iLONr & " (Geteilter KST): " & LioData(3) & " / " & LioData(4)
          For i As Integer = 0 To 1
            GetLOLDataFromLONr(LioData(2), CInt(LioData(3 + i)), _
                               rLOrec("Filiale_" & CStr(i + 1)).ToString, CShort(rLOrec("AnzLO").ToString), _
                               sCalledMethode)
          Next

        Else
          strlonr &= Me._iLONr & " (Einzelne KST): " & LioData(3)
          GetLOLDataFromLONr(LioData(2), CInt(LioData(3)), If(String.IsNullOrEmpty(rLOrec("Filiale_1").ToString), "???", _
                                                              rLOrec("Filiale_1").ToString), CShort(rLOrec("AnzLO").ToString), _
                                                              sCalledMethode)




          ' --------------------------------------------------------------------------------------------------------
          If LioData(2) = -999 Then
            If CShort(rLOrec("AnzLO").ToString) <> 1 Then
              If sCalledMethode = CShort(rLOrec("AnzLO").ToString) Then
                LioData(2) = 1
                GetLOLDataFromLONr(LioData(2), CInt(LioData(3)), _
                                   If(String.IsNullOrEmpty(rLOrec("Filiale_1").ToString), "???", _
                                      rLOrec("Filiale_1").ToString), CShort(rLOrec("AnzLO").ToString), _
                                      sCalledMethode)

              End If

            End If

          End If
          ' --------------------------------------------------------------------------------------------------------



        End If

      End While

      InesrtDataToFinalDb(frmTest)

      Trace.WriteLine(strlonr)


    Catch e As SqlException
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
			MsgBox(e.StackTrace & vbNewLine & e.Message, MsgBoxStyle.Critical, "CreateTable4LOAHVBas (SqlException):")
			Trace.WriteLine(e.Message, "SqlException: CreateTable4LOAHVBas")

		Catch e As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
			MsgBox(e.StackTrace & vbNewLine & e.Message, MsgBoxStyle.Critical, "CreateTable4LOAHVBas")
			Trace.WriteLine(e.Message, "Exception: CreateTable4LOAHVBas")

		Finally

		End Try

	End Sub

	Sub GetLOLDataFromLONr(ByVal dblKSTProz As Double, ByVal iFilialNr As Integer,
												 ByVal strFilialName As String, ByVal iRecCount As Short,
												 ByVal sCalledMethode As Short)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim bInsertKST As Boolean = True
		Dim strLOLFibuTable As String = String.Format("#_LOLData_{0}", CStr(iFilialNr))
		Dim strLOLFilialTable As String = String.Format("##_LOLFilial_{0}", CStr(iFilialNr))
		Dim sSql As String = String.Format("BEGIN TRY DROP TABLE {0} END TRY BEGIN CATCH END CATCH ", strLOLFibuTable)

		'If Me._LioLOLData(3) = Me._LioLOLData(4) Then Exit Sub


		For j As Integer = 0 To LioFilialen.Count - 1
			If LioFilialen(j) = iFilialNr.ToString Then
				bInsertKST = False
				Exit For
			End If
		Next
		If bInsertKST Then
			sSql &= String.Format("BEGIN TRY DROP TABLE {0} END TRY BEGIN CATCH END CATCH ", strLOLFilialTable)

			LioFilialen.Add("")
			LioFilialen(LioFilialen.Count - 1) = iFilialNr.ToString
		End If

		sSql &= "Select 0 As 'LONr', 0 As 'Proz', LOL.LANr, "
		sSql &= "LOL.RPText, "
		sSql &= "LOL.m_btr, "

		sSql &= "(		"
		sSql &= "	case  "
		sSql &= "		When LA.BruttoPflichtig = 1 then 1 "
		sSql &= "		Else 0 "
		sSql &= "End "
		sSql &= "	) As BPflicht, "

		sSql &= "(		"
		sSql &= "	case  "
		sSql &= "		When LA.AHVPflichtig = 1 then 1 "
		sSql &= "		Else 0 "
		sSql &= "End "
		sSql &= "	) As AHVPflicht, "


		sSql &= String.Format("	Convert(nvarchar(70), '{0}') As Filiale_1 ", strFilialName)

		sSql &= ",(		"
		sSql &= "	case  "
		sSql &= "		When LOL.LANR > 6999 then '' "
		sSql &= "		When charindex('/', LOL.KST, 0) > 1 then      "

		sSql &= "					(Select Top 1 Benutzer.USFiliale "
		sSql &= "    From Benutzer "
		'sSql &= "						Left Join Benutzer on US_Filiale.USNr = Benutzer.USNr "
		sSql &= "						Where Benutzer.KST In (SUBSTRING(LOL.KST,charindex('/', LOL.KST, 0)+1,50) ) "
		sSql &= "						And Benutzer.USFiliale Is not Null "
		'sSql &= "						Group By US_Filiale.Bezeichnung "
		sSql &= "				   ) "

		sSql &= "		else 		"
		sSql &= "					(Select Top 1 Benutzer.USFiliale "
		sSql &= "    From Benutzer "
		'sSql &= "				Left Join Benutzer on US_Filiale.USNr = Benutzer.USNr "
		sSql &= "    Where (Benutzer.KST = LOL.KST) "
		sSql &= "				And Benutzer.USFiliale Is not Null "
		'sSql &= "				Group By Benutzer.USFiliale "
		sSql &= "			   ) "

		sSql &= "End "
		sSql &= "	) As Filiale_2 "

		sSql &= ", LOL.M_BTR As 'Betrag_1', LOL.M_BTR As 'Betrag_2', Convert(nvarchar(70), KST) As 'FilialeNr_1', Convert(nvarchar(70), KST) As 'FilialeNr_2' "

		sSql &= String.Format("Into {0} ", strLOLFibuTable)
		sSql &= "From LOL "
		sSql &= "Left Join LA On LOL.LANR = LA.LANr And LOL.Jahr = LA.LAJahr "
		sSql &= "Where LOL.MDNr = @MDNr And "

		sSql &= String.Format("LOL.LONr = {0} ", Me._iLONr)
		sSql &= "And RPText is not null and LOL.M_BTR <> 0 "
		sSql &= "And LA.SKonto + LA.HKonto <> 0 "

		If dblKSTProz = -990 Then
			sSql &= "And LA.BruttoPflichtig = 0 And LA.AHVPflichtig = 0 " ' neue Zeile 16.10.2012
			'sSql &= "And LOL.LANr > 7000 And LA.BruttoPflichtig = 0 And LA.AHVPflichtig = 0 "

		ElseIf dblKSTProz = -999 Then
			If iRecCount = 1 Then
				'        sSql &= String.Format("And LOL.LANr < 7000 And LOL.KST = '{0}' ", Me._strKst)

			Else
				'If sCalledMethode = iRecCount Then
				'  'sSql &= String.Format("And LOL.KST <> '{0}' ", Me._strKst)
				'  sSql &= String.Format("And LOL.LANr > 7000 ", Me._strKst)

				'Else
				'  sSql &= String.Format("And LOL.LANr < 7000 And LOL.KST = '{0}' ", Me._strKst)

				'End If
				sSql &= String.Format("And LOL.LANr < 7000 And LOL.KST = '{0}' ", Me._strKst)

			End If

		Else
			sSql &= "And LOL.LANr > 7000 "

		End If

		sSql &= "Order By LOL.LONr, LOL.LANr "

		sSql &= String.Format("Update {0} Set {0}.FilialeNr_1 = {1}, ", strLOLFibuTable, iFilialNr)
		sSql &= String.Format("{0}.LONr = {1}, {0}.Proz = {2} ", strLOLFibuTable, Me._iLONr, dblKSTProz)

		'If Me._iLONr = 1726 Then
		'  '  Trace.WriteLine(Me._iLONr)
		'End If

		If dblKSTProz = -990 Then   ' Achtung: diese Lohnarten sind weder Brutto noch AHV-pflichtig!!!
			If Me._LioLOLData(3) <> Me._LioLOLData(4) Then
				sSql &= String.Format("Update {0} Set {0}.Betrag_2 = {0}.m_btr / 2 ", strLOLFibuTable)
			Else
				sSql &= String.Format("Update {0} Set {0}.Betrag_2 = {0}.m_btr ", strLOLFibuTable)
			End If

		ElseIf dblKSTProz = -999 Then   ' Achtung: diese Lohnarten sind alle BRUTTO-pflichtig!!!
			If Me._LioLOLData(3) <> Me._LioLOLData(4) Then
				sSql &= String.Format("Update {0} Set {0}.Betrag_2 = {0}.m_btr / 2 ", strLOLFibuTable)
			Else
				sSql &= String.Format("Update {0} Set {0}.Betrag_2 = {0}.m_btr ", strLOLFibuTable)
			End If
			sSql &= String.Format("Where {0}.Filiale_1 = '{1}' ", strLOLFibuTable, strFilialName)
			'sSql &= String.Format("Where {0}.Filiale_1 = '{1}' And {0}.Filiale_2 = '{2}' ", strLOLFibuTable, strFilialName, Me._LioLOLName(1))

		Else
			If Me._LioLOLData(3) <> Me._LioLOLData(4) Then
				sSql &= String.Format("Update {0} Set {0}.Betrag_2 = {0}.m_btr * {1} / 2 ", strLOLFibuTable, (Me._LioLOLData(1) / Me._LioLOLData(0)))

			Else
				If dblKSTProz = 1 Then
					sSql &= String.Format("Update {0} Set {0}.Betrag_2 = {0}.m_btr ", strLOLFibuTable)
				Else
					sSql &= String.Format("Update {0} Set {0}.Betrag_2 = {0}.m_btr * {1} ", strLOLFibuTable, (Me._LioLOLData(1) / Me._LioLOLData(0)))
				End If
			End If

		End If

		If bInsertKST Then
			sSql &= String.Format("   Select * Into {0} From {1} Order By {1}.LANr ", strLOLFilialTable, strLOLFibuTable)

		Else
			sSql &= String.Format("   Insert Into {0} Select * From {1} ", strLOLFilialTable, strLOLFibuTable)

		End If

		sSql = sSql.Replace("@_LOLData_", strLOLFibuTable).Replace("@_LOLFilial_", strLOLFilialTable)
		Try
			If IsNothing(ClsDataDetail.Conn) Then Exit Sub
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, ClsDataDetail.Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@MDNr", m_InitialData.MDData.MDNr)
			param = cmd.Parameters.AddWithValue("@LP", Me._sMonth)
			param = cmd.Parameters.AddWithValue("@Jahr", Me._strYear)
			param = cmd.Parameters.AddWithValue("@LONr", Me._iLONr)
			param = cmd.Parameters.AddWithValue("@MyFilialNr", iFilialNr)

			'Trace.WriteLine(String.Format("Lohnabrechnung: {0}, Tabelle: {1}", Me._iLONr, strLOLFilialTable))

			cmd.ExecuteNonQuery()
			'CreateLOLDataFromLONr()

		Catch e As SqlException
			m_Logger.LogError(String.Format("{0}.{1}", sSql, e.ToString))
			MsgBox(e.Message, MsgBoxStyle.Critical, "(GetLOLDataFromLONr) Fehler_2")
			Trace.WriteLine(e.Message, "SqlException: GetLOLDataFromLONr")

		Catch e As Exception
			m_Logger.LogError(String.Format("{0}.{1}", sSql, e.ToString))
			MsgBox(e.Message, MsgBoxStyle.Critical, "(GetLOLDataFromLONr) Fehler_0")
			Trace.WriteLine(e.Message, "Exception: GetLOLDataFromLONr")

		Finally

		End Try

	End Sub

	Sub InesrtDataToFinalDb(ByVal frmSource As frmFibuSearch)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim sSql As String = String.Empty
		Dim strTempTable As String = "##_LOLFilial_"
		Dim i As Integer = 0

		frmSource.txt_SQL_2.Text = String.Empty

		i = 0
		sSql &= String.Format("BEGIN TRY DROP TABLE [_LOLFibu_Total_{0}] ", _ClsSystem.GetLogedUSGuid)
		sSql &= "END TRY BEGIN CATCH END CATCH "

		If LioFilialen.Count > 0 Then
			sSql &= String.Format("BEGIN TRY DROP TABLE [_LOLFibu_{0}] ", _ClsSystem.GetLogedUSGuid)
			sSql &= "END TRY BEGIN CATCH END CATCH "

			sSql &= String.Format("Select @_LOLFilial_{0}.LONr, @_LOLFilial_{0}.Proz, ", LioFilialen(i).ToString)
			sSql &= String.Format("@_LOLFilial_{0}.LANr, @_LOLFilial_{0}.RPText, ", LioFilialen(i).ToString)
			sSql &= String.Format("@_LOLFilial_{0}.m_btr, @_LOLFilial_{0}.Betrag_2, ", LioFilialen(i).ToString)

			sSql &= String.Format("@_LOLFilial_{0}.Filiale_1 + Space(50) As Filiale_1, ", LioFilialen(i).ToString)
			sSql &= String.Format("@_LOLFilial_{0}.FilialeNr_1, {1} As Jahr, ", LioFilialen(i).ToString, ClsDataDetail.GetYear)
			sSql &= String.Format("La.Vorzeichen_2, La.Vorzeichen_3, La.HKonto, La.SKonto, ", LioFilialen(i).ToString)
			sSql &= String.Format("LA.LALOText As Bezeichnung, LA.Reserve2 ", LioFilialen(i).ToString)

			sSql &= String.Format("Into [_LOLFibu_{0}] From @_LOLFilial_{1} ", _ClsSystem.GetLogedUSGuid, LioFilialen(i).ToString)
			sSql &= String.Format("Left Join LA On @_LOLFilial_{0}.LANr = LA.LANr  ", LioFilialen(i).ToString)
			sSql &= String.Format("Where LA.LAJahr = {0} ", ClsDataDetail.GetYear)
			sSql &= String.Format("Order By @_LOLFilial_{0}.LANr ", LioFilialen(i).ToString)


			For i = 1 To LioFilialen.Count - 1
				sSql &= String.Format("   Insert Into [_LOLFibu_{0}] ", _ClsSystem.GetLogedUSGuid)

				sSql &= String.Format("Select @_LOLFilial_{0}.LONr, @_LOLFilial_{0}.Proz, ", LioFilialen(i).ToString)
				sSql &= String.Format("@_LOLFilial_{0}.LANr, @_LOLFilial_{0}.RPText, ", LioFilialen(i).ToString)
				sSql &= String.Format("@_LOLFilial_{0}.m_btr, @_LOLFilial_{0}.Betrag_2, ", LioFilialen(i).ToString)

				sSql &= String.Format("@_LOLFilial_{0}.Filiale_1, ", LioFilialen(i).ToString)
				sSql &= String.Format("@_LOLFilial_{0}.FilialeNr_1, {1} As Jahr, ", LioFilialen(i).ToString, ClsDataDetail.GetYear)
				sSql &= String.Format("La.Vorzeichen_2, La.Vorzeichen_3, La.HKonto, La.SKonto, ", LioFilialen(i).ToString)
				sSql &= String.Format("LA.LALOText As Bezeichnung, LA.Reserve2 ", LioFilialen(i).ToString)

				sSql &= String.Format("From @_LOLFilial_{0} ", LioFilialen(i).ToString)

				sSql &= String.Format("Left Join LA On @_LOLFilial_{0}.LANr = LA.LANr  ", LioFilialen(i).ToString)
				sSql &= String.Format("Where LA.LAJahr = {0} ", ClsDataDetail.GetYear)

				sSql &= String.Format("Order By @_LOLFilial_{0}.LANr ", LioFilialen(i).ToString)

			Next
		End If
		sSql = sSql.Replace("@_LOLFilial_", strTempTable)


		Try
			If IsNothing(ClsDataDetail.Conn) Then Exit Sub
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, ClsDataDetail.Conn)
			cmd.CommandType = Data.CommandType.Text

			cmd.ExecuteNonQuery()


		Catch e As SqlException
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
			frmSource.txt_SQL_2.Text = sSql
			MsgBox(e.StackTrace & vbNewLine & e.Message, MsgBoxStyle.Critical, "InesrtDataToFinalDb (SqlException)")
			Trace.WriteLine(e.Message, "SqlException: InesrtDataToFinalDb")

		Catch e As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
			MsgBox(e.StackTrace & vbNewLine & e.Message, MsgBoxStyle.Critical, "InesrtDataToFinalDb")
      Trace.WriteLine(e.Message, "Exception: InesrtDataToFinalDb")

    Finally

    End Try

  End Sub

  Function FillMyLOiBetrag(ByVal iCount As Integer) As List(Of Double)
    Dim loiBetrag As New List(Of Double)

    If iCount = 0 Then iCount = 30
    For i As Integer = 0 To iCount - 1
      loiBetrag.Add(0)
      loiBetrag(i) = 0
    Next

    Return loiBetrag
  End Function

  Function FillMyLOiString(ByVal iCount As Integer) As List(Of String)
    Dim loiBetrag As New List(Of String)

    If iCount = 0 Then iCount = 30
    For i As Integer = 0 To iCount - 1
      loiBetrag.Add("")
      loiBetrag(i) = ""
    Next

    Return loiBetrag
  End Function

#Region "Funktionen zur Suche nach Daten..."

  Function GetStartSQLString(ByVal frmTest As frmFibuSearch, _
                             ByVal ifMonth As Short, _
                             ByVal strYear As String, _
                             ByVal strFiliale As String, _
                             ByVal bWithUmsJDb As Boolean) As String

    Dim sSql As String = String.Empty
    Dim sSqlLen As Integer = 0
    Dim sZusatzBez As String = String.Empty
    Dim i As Integer = 0
    Dim _ClsReg As New SPProgUtility.ClsDivReg

		ClsDataDetail.Conn = New SqlConnection(EnablingMarsintoConnString(m_InitialData.MDData.MDDbConn))
    ClsDataDetail.Conn.Open()
		m_Logger.LogDebug(String.Format("m_InitialData.MDName: {0} | m_InitialData.MDNr: {1} | m_InitialData.MDDbConn: {2}", m_InitialData.MDData.MDName, m_InitialData.MDData.MDNr, m_InitialData.MDData.MDDbConn))

		Me.CreateTable4LOAHVBas(frmTest, ifMonth, strYear, "")

    Dim FilterBez As String = frmTest.Cbo_Month.Text & " / " & frmTest.Cbo_Year.Text
    ClsDataDetail.GetFilterBez = FilterBez

    Return sSql
  End Function

#End Region

#Region "Final-Query..."

  Function GetFinalQueryForOutput(ByVal frmSource As frmFibuSearch) As String
    Dim strResult As String = String.Empty
    strResult &= "Select lFibu.LANr, LA.LALoText As Bezeichnung, "
    strResult &= "LA.HKonto, LA.SKonto, LA.Vorzeichen_2, La.Vorzeichen_3, "
    strResult &= "Sum(LFibu.Betrag_2) As TotalBetrag "

    strResult &= String.Format("From [_LOLFibu_{0}] LFibu ", _ClsSystem.GetLogedUSGuid)
    strResult &= "Left Join LA on lFibu.LANr = LA.LANr "
    strResult &= String.Format("Where LA.LAJahr = {0} ", frmSource.Cbo_Year.Text)
    strResult &= "Group By lFibu.LANr, LA.LALoText, "
    strResult &= "LA.HKonto, LA.SKonto, LA.Vorzeichen_2, La.Vorzeichen_3 "

    strResult &= "Order By lFibu.LANr, LA.LALoText "

    Return strResult
  End Function

  Function GetOutputQueryForFilial(ByVal frmSource As frmFibuSearch, ByVal strFilialName As String) As String
    Dim strResult As String = "Select lFibu.LANr, LA.LALoText As Bezeichnung, "
    strResult &= "LA.HKonto, LA.SKonto, LA.Vorzeichen_2, La.Vorzeichen_3, "
    strResult &= "Sum(lFibu.Betrag_2) As TotalBetrag "
    strResult &= String.Format("From [_LOLFibu_{0}] lFibu ", _ClsSystem.GetLogedUSGuid)
    strResult &= "Left Join LA on lFibu.LANr = LA.LANr "
    strResult &= String.Format("Where LA.LAJahr = {0} ", frmSource.Cbo_Year.Text)
    If Not strFilialName.ToLower = "alle" AndAlso strFilialName <> String.Empty Then
      strResult &= String.Format("And lFibu.Filiale_1 = '{0}' ", strFilialName)
    End If
    strResult &= "Group By lFibu.LANr, LA.LALoText, "
    strResult &= "LA.HKonto, LA.SKonto, LA.Vorzeichen_2, La.Vorzeichen_3 "

    strResult &= "Order By lFibu.LANr, LA.LALoText "

    Return strResult
  End Function

#End Region


#Region "Final-Query Gruppiert nach H- und SKonto..."

  Function GetFinalQueryForOutput_G(ByVal frmSource As frmFibuSearch) As String
    Dim strResult As String = String.Empty

    strResult &= "begin Try Drop Table #Fibu_ End Try Begin Catch End Catch "
    strResult &= "begin Try Drop Table #FibuKum_ End Try Begin Catch End Catch "

    strResult &= "Select lFibu.LANr, LA.LALoText As Bezeichnung, "
    strResult &= "LA.HKonto, LA.SKonto, LA.Vorzeichen_2, La.Vorzeichen_3, "
    strResult &= "Sum(LFibu.Betrag_2) As TotalBetrag Into #Fibu_ "

    strResult &= String.Format("From [_LOLFibu_{0}] LFibu ", _ClsSystem.GetLogedUSGuid)
    strResult &= "Left Join LA on lFibu.LANr = LA.LANr "
    strResult &= String.Format("Where LA.LAJahr = {0} ", frmSource.Cbo_Year.Text)
    strResult &= "Group By lFibu.LANr, LA.LALoText, "
    strResult &= "LA.HKonto, LA.SKonto, LA.Vorzeichen_2, La.Vorzeichen_3 "

    strResult &= "Select sum(LFibu.TotalBetrag) As BetragKumulativ, "
    strResult &= "LA.HKonto, LA.SKonto "
    strResult &= "Into #FibuKum_ From #Fibu_ LFibu "
    strResult &= "Left Join LA on lFibu.LANr = LA.LANr "
    strResult &= String.Format("Where LA.LAJahr = {0} ", frmSource.Cbo_Year.Text)
    strResult &= "Group By LA.HKonto, LA.SKonto "

    strResult &= "Select fbKum.*, "
    strResult &= "IsNull((Select fbk.KontoName From FBK Where fbk.KontoNr = fbKum.HKonto), '')  As Habenkonto, "
    strResult &= "IsNull((Select fbk.KontoName From FBK Where fbk.KontoNr = fbKum.SKonto), '') As Sollkonto "
    strResult &= "From #FibuKum_ fbKum "
    strResult &= "Order By Convert(int, fbKum.SKonto) "

    Return strResult
  End Function

  Function GetOutputQueryForFilial_G(ByVal frmSource As frmFibuSearch, ByVal strFilialName As String) As String
    Dim strResult As String = String.Empty

    strResult &= "begin Try Drop Table #Fibu_ End Try Begin Catch End Catch "
    strResult &= "begin Try Drop Table #FibuKum_ End Try Begin Catch End Catch "
    strResult &= "Select lFibu.LANr, LA.LALoText As Bezeichnung, "

    strResult &= "LA.HKonto, LA.SKonto, LA.Vorzeichen_2, La.Vorzeichen_3, "
    strResult &= "Sum(lFibu.Betrag_2) As TotalBetrag Into #Fibu_ "
    strResult &= String.Format("From [_LOLFibu_{0}] lFibu ", _ClsSystem.GetLogedUSGuid)
    strResult &= "Left Join LA on lFibu.LANr = LA.LANr "
    strResult &= String.Format("Where LA.LAJahr = {0} ", frmSource.Cbo_Year.Text)
    If Not strFilialName.ToLower = "alle" AndAlso strFilialName <> String.Empty Then
      strResult &= String.Format("And lFibu.Filiale_1 = '{0}' ", strFilialName)
    End If
    strResult &= "Group By lFibu.LANr, LA.LALoText, "
    strResult &= "LA.HKonto, LA.SKonto, LA.Vorzeichen_2, La.Vorzeichen_3 "

    strResult &= "Select sum(LFibu.TotalBetrag) As BetragKumulativ, "
    strResult &= "LA.HKonto, LA.SKonto "
    strResult &= "Into #FibuKum_ From #Fibu_ LFibu "
    strResult &= "Left Join LA on lFibu.LANr = LA.LANr "
    strResult &= String.Format("Where LA.LAJahr = {0} ", frmSource.Cbo_Year.Text)
    strResult &= "Group By LA.HKonto, LA.SKonto "

    strResult &= "Select fbKum.*, "
    strResult &= "IsNull((Select fbk.KontoName From FBK Where fbk.KontoNr = fbKum.HKonto), '')  As Habenkonto, "
    strResult &= "IsNull((Select fbk.KontoName From FBK Where fbk.KontoNr = fbKum.SKonto), '') As Sollkonto "
    strResult &= "From #FibuKum_ fbKum "
    strResult &= "Order By Convert(int, fbKum.SKonto) "

    Return strResult
  End Function

#End Region

End Class


