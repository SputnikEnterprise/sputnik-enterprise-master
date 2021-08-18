
Imports SPProgUtility.MainUtilities

Imports System.IO
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions

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

  '// Firmenname
  Dim _strKDName As String
  Public Property GetKDName() As String
    Get
      Return _strKDName
    End Get
    Set(ByVal value As String)
      _strKDName = value
    End Set
  End Property

  '// KDZhd
  Dim _iKDZhdNr As Integer
  Public Property GetKDZhdNr() As Integer
    Get
      Return _iKDZhdNr
    End Get
    Set(ByVal value As Integer)
      _iKDZhdNr = value
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

  'Dim _ClsSystem As New ClsMain_Net
  'Dim strMyDateFormat As String = _ClsSystem.GetSQLDateFormat()
  'Dim strConnString As String = _ClsSystem.GetConnString()

#Region "Funktionen für Speichern der Daten..."

  '// Kanton (= Kanton)
  Dim _QSTRecNr As Integer
  Public Property QSTRecNr() As Integer
    Get
      Return _QSTRecNr
    End Get
    Set(ByVal value As Integer)
      _QSTRecNr = value
    End Set
  End Property

  '// Kanton (= Kanton)
  Dim _QSTKanton As String
  Public Property QSTKanton() As String
    Get
      Return _QSTKanton
    End Get
    Set(ByVal value As String)
      _QSTKanton = value
    End Set
  End Property
  '// Gemeinde (= Gemeinde)
  Dim _QSTGemeinde As String
  Public Property QSTGemeinde() As String
    Get
      Return _QSTGemeinde
    End Get
    Set(ByVal value As String)
      _QSTGemeinde = value
    End Set
  End Property
  '// Adresse1 (= Adresse1)
  Dim _QSTAdresse1 As String
  Public Property QSTAderesse1() As String
    Get
      Return _QSTAdresse1
    End Get
    Set(ByVal value As String)
      _QSTAdresse1 = value
    End Set
  End Property
  '// Zusatz (= Zusatz)
  Dim _QSTZusatz As String
  Public Property QSTZusatz() As String
    Get
      Return _QSTZusatz
    End Get
    Set(ByVal value As String)
      _QSTZusatz = value
    End Set
  End Property
  '// ZHD (= ZHD)
  Dim _QSTZHD As String
  Public Property QSTZHD() As String
    Get
      Return _QSTZHD
    End Get
    Set(ByVal value As String)
      _QSTZHD = value
    End Set
  End Property
  '// Postfach (= Postfach)
  Dim _QSTPostfach As String
  Public Property QSTPostfach() As String
    Get
      Return _QSTPostfach
    End Get
    Set(ByVal value As String)
      _QSTPostfach = value
    End Set
  End Property
  '// Strasse (= Strasse)
  Dim _QSTStrasse As String
  Public Property QSTStrasse() As String
    Get
      Return _QSTStrasse
    End Get
    Set(ByVal value As String)
      _QSTStrasse = value
    End Set
  End Property
  '// Land (= Land)
  Dim _QSTLand As String
  Public Property QSTLand() As String
    Get
      Return _QSTLand
    End Get
    Set(ByVal value As String)
      _QSTLand = value
    End Set
  End Property
  '// PLZ (= PLZ)
  Dim _QSTPlz As String
  Public Property QSTPlz() As String
    Get
      Return _QSTPlz
    End Get
    Set(ByVal value As String)
      _QSTPlz = value
    End Set
  End Property
  '// Ort (= Ort)
  Dim _QSTOrt As String
  Public Property QSTOrt() As String
    Get
      Return _QSTOrt
    End Get
    Set(ByVal value As String)
      _QSTOrt = value
    End Set
  End Property
  '// StammNr (= StammNr)
  Dim _QSTStammNr As String
  Public Property QSTStammNr() As String
    Get
      Return _QSTStammNr
    End Get
    Set(ByVal value As String)
      _QSTStammNr = value
    End Set
  End Property
  '// Provision (= Provision)
  Dim _QSTProvision As Double
  Public Property QSTProvision() As Double
    Get
      Return _QSTProvision
    End Get
    Set(ByVal value As Double)
      _QSTProvision = value
    End Set
  End Property
  '// Bemerkung (= Bemerkung)
  Dim _QSTBemerkung As String
  Public Property QSTBemerkung() As String
    Get
      Return _QSTBemerkung
    End Get
    Set(ByVal value As String)
      _QSTBemerkung = value
    End Set
  End Property


#End Region


#Region "Datensatz bearbeiten..."

  Sub SaveDataToQSTDb(ByVal iRecNr As Integer)
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)
    Dim strQuery As String = String.Empty
    Dim iNewRecNr As Integer = 0
    Dim m_utiliity As New Utilities
    Dim m_common As New SPProgUtility.CommonSettings.CommonSetting

    If iRecNr <> 0 Then
      iNewRecNr = iRecNr
      strQuery = "Update MD_QstAddress Set Kanton = @Kanton, Gemeinde = @Gemeinde, Adresse1 = @Adresse1, "
      strQuery &= "Zusatz = @Zusatz, ZHD = @ZHD, Postfach = @Postfach, Strasse = @Strasse, Land = @Land, PLZ = @PLZ, "
      strQuery &= "Ort = @Ort, StammNr = @StammNr, Provision = @Provision, Bemerkung = @Bemerkung, "
      strQuery &= "ChangedOn = @ChangedOn, ChangedFrom = @ChangedFrom, MDNr = @MDNr "
      strQuery &= "Where RecNr = @RecID"

    Else
      iNewRecNr = GetNewRecNr()
      strQuery = "Insert Into MD_QstAddress (Kanton, Gemeinde, Adresse1, Zusatz, ZHD, Postfach, Strasse, "
      strQuery &= "Land, PLZ, Ort, StammNr, Provision, Bemerkung, CreatedOn, CreatedFrom, RecNr, MDNr) Values ("
      strQuery &= "@Kanton, @Gemeinde,@Adresse1, "
      strQuery &= "@Zusatz, @ZHD, @Postfach, @Strasse, @Land, @PLZ, "
      strQuery &= "@Ort, @StammNr, @Provision, @Bemerkung, "
      strQuery &= "@ChangedOn, @ChangedFrom, @RecID, @MDNr)"

    End If

    Dim i As Integer = 0
    Dim _ClsDb As New ClsDbFunc
    Me.QSTRecNr = iNewRecNr

    Try
      Conn.Open()
      Dim cmd As System.Data.SqlClient.SqlCommand
      cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
      Dim param As System.Data.SqlClient.SqlParameter

      param = cmd.Parameters.AddWithValue("@Kanton", Me.QSTKanton)
      param = cmd.Parameters.AddWithValue("@Gemeinde", Me.QSTGemeinde)
      param = cmd.Parameters.AddWithValue("@Adresse1", Me.QSTAderesse1)
      param = cmd.Parameters.AddWithValue("@Zusatz", Me.QSTZusatz)
      param = cmd.Parameters.AddWithValue("@ZHD", Me.QSTZHD)
      param = cmd.Parameters.AddWithValue("@Postfach", Me.QSTPostfach)
      param = cmd.Parameters.AddWithValue("@Strasse", Me.QSTStrasse)
      param = cmd.Parameters.AddWithValue("@Land", Me.QSTLand)
      param = cmd.Parameters.AddWithValue("@PLZ", Me.QSTPlz)
      param = cmd.Parameters.AddWithValue("@Ort", Me.QSTOrt)
      param = cmd.Parameters.AddWithValue("@StammNr", Me.QSTStammNr)
      param = cmd.Parameters.AddWithValue("@Provision", Me.QSTProvision)
      param = cmd.Parameters.AddWithValue("@Bemerkung", Me.QSTBemerkung)

      param = cmd.Parameters.AddWithValue("@ChangedOn", Format(Now.Date, "G"))
      param = cmd.Parameters.AddWithValue("@ChangedFrom", String.Format("{0}, {1}", m_common.GetLogedUserLastName, m_common.GetLogedUserFirstName))
      param = cmd.Parameters.AddWithValue("@RecID", iNewRecNr)
      param = cmd.Parameters.AddWithValue("@MDNr", ClsDataDetail.SelectedMDData.MDNr)

      cmd.ExecuteNonQuery()
      cmd.Parameters.Clear()


    Catch ex As Exception
      MsgBox(ex.Message, MsgBoxStyle.Critical, "SaveDataToQSTDb_0")

    End Try


  End Sub

  Function GetNewRecNr() As Integer
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)
    Dim iResult As Integer = 1
    Dim strQuery As String = "Select Top 1 RecNr From MD_QstAddress Order By RecNr Desc"
    Conn.Open()

    Dim cmd As System.Data.SqlClient.SqlCommand
    cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
    Dim rKDrec As SqlDataReader = cmd.ExecuteReader          ' PLZ-Datenbank


    Try
      While rKDrec.Read
        iResult += CInt(rKDrec("RecNr").ToString)
      End While
      If Not rKDrec.HasRows Then iResult = 1

    Catch ex As Exception


    End Try


    Return iResult
  End Function

  Sub DeleteSelectedRec(ByVal iRecNr As Integer)
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)

    Dim strQuery As String = "Delete MD_QstAddress Where RecNr = @RecID"

    Try
      Conn.Open()
      Dim cmd As System.Data.SqlClient.SqlCommand
      cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
      Dim param As System.Data.SqlClient.SqlParameter

      param = cmd.Parameters.AddWithValue("@RecID", iRecNr)

      cmd.ExecuteNonQuery()
      cmd.Parameters.Clear()


    Catch ex As Exception
      MsgBox(ex.Message, MsgBoxStyle.Critical, "DeleteSelectedRec_0")

    End Try


  End Sub

#End Region

#Region "Funktionen zur Suche nach Daten..."

  Function GetLocalSQLString() As String
    Dim sSql As String = String.Empty

    sSql = "Select * From MD_QstAddress "
    sSql &= "Where (RecNr = @RecNr Or @RecNr = 0) And MDNr = @MDNr "
    sSql &= "Order By Kanton, Gemeinde"

    Return sSql
  End Function

  Function GetLstItems(ByVal lst As ListBox) As String
    Dim strBerufItems As String = String.Empty

    For i = 0 To lst.Items.Count - 1
      strBerufItems += lst.Items(i).ToString & "#@"
    Next

    Return Left(strBerufItems, Len(strBerufItems) - 2)
  End Function

#End Region


End Class