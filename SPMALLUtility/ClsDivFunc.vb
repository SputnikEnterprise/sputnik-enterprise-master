
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

  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
  Dim _ClsReg As New SPProgUtility.ClsDivReg
  Dim _ClsSystem As New ClsMain_Net

  Dim strMyDateFormat As String = _ClsProgSetting.GetSQLDateFormat()
  Dim strConnString As String = _ClsProgSetting.GetConnString()


#Region "Funktionen für Speichern der Daten..."

  '// RecNr
  Dim _P_RecNr As Integer
  Public Property GetRecNr() As Integer
    Get
      Return _P_RecNr
    End Get
    Set(ByVal value As Integer)
      _P_RecNr = value
    End Set
  End Property

  '// MANr
  Dim _P_MANr As Integer
  Public Property GetMANr() As Integer
    Get
      Return _P_MANr
    End Get
    Set(ByVal value As Integer)
      _P_MANr = value
    End Set
  End Property

  '// KDNr
  Dim _P_KDNr As Integer
  Public Property GetKDNr() As Integer
    Get
      Return _P_KDNr
    End Get
    Set(ByVal value As Integer)
      _P_KDNr = value
    End Set
  End Property

  '// KDZHDNr
  Dim _P_KDZHDNr As Integer
  Public Property GetKDZHDNr() As Integer
    Get
      Return _P_KDZHDNr
    End Get
    Set(ByVal value As Integer)
      _P_KDZHDNr = value
    End Set
  End Property

  '// VakNr
  Dim _P_VakNr As Integer
  Public Property GetVakNr() As Integer
    Get
      Return _P_VakNr
    End Get
    Set(ByVal value As Integer)
      _P_VakNr = value
    End Set
  End Property

  '// Bezeichnung
  Dim _Bez As String
  Public Property GetBezeichnung() As String
    Get
      Return _Bez
    End Get
    Set(ByVal value As String)
      _Bez = value
    End Set
  End Property

  '// KST
  Dim _PKst_1 As String
  Public Property GetP_KST_1() As String
    Get
      Return _PKst_1
    End Get
    Set(ByVal value As String)
      _PKst_1 = value
    End Set
  End Property

  '// Berater 
  Dim _Berater_1 As String
  Public Property GetBerater_1() As String
    Get
      Return _Berater_1
    End Get
    Set(ByVal value As String)
      _Berater_1 = value
    End Set
  End Property

  '// KST
  Dim _PKst_2 As String
  Public Property GetP_KST_2() As String
    Get
      Return _PKst_2
    End Get
    Set(ByVal value As String)
      _PKst_2 = value
    End Set
  End Property

  '// Berater 
  Dim _Berater_2 As String
  Public Property GetBerater_2() As String
    Get
      Return _Berater_2
    End Get
    Set(ByVal value As String)
      _Berater_2 = value
    End Set
  End Property

  '// Status
  Dim _PState As String
  Public Property GetP_State() As String
    Get
      Return _PState
    End Get
    Set(ByVal value As String)
      _PState = value
    End Set
  End Property

  '// Art
  Dim _PArt As String
  Public Property GetP_Art() As String
    Get
      Return _PArt
    End Get
    Set(ByVal value As String)
      _PArt = value
    End Set
  End Property

  '// Tarif
  Dim _KDTarif As String
  Public Property GetKD_Tarif() As String
    Get
      Return _KDTarif
    End Get
    Set(ByVal value As String)
      _KDTarif = value
    End Set
  End Property

  '// Lohn
  Dim _MALohn As String
  Public Property GetMA_Lohn() As String
    Get
      Return _MALohn
    End Get
    Set(ByVal value As String)
      _MALohn = value
    End Set
  End Property

  '// Anstellung
  Dim _Anstellung As String
  Public Property GetAnstellung() As String
    Get
      Return _Anstellung
    End Get
    Set(ByVal value As String)
      _Anstellung = value
    End Set
  End Property

  '// ArbBegin
  Dim _ArbBegin As String
  Public Property GetArbBegin() As String
    Get
      Return _ArbBegin
    End Get
    Set(ByVal value As String)
      _ArbBegin = value
    End Set
  End Property

  '// Zusatz1
  Dim _Zusatz1 As String
  Public Property GetZusatz1() As String
    Get
      Return _Zusatz1
    End Get
    Set(ByVal value As String)
      _Zusatz1 = value
    End Set
  End Property

  '// Zusatz2
  Dim _Zusatz2 As String
  Public Property GetZusatz2() As String
    Get
      Return _Zusatz2
    End Get
    Set(ByVal value As String)
      _Zusatz2 = value
    End Set
  End Property

  '// Zusatz3
  Dim _Zusatz3 As String
  Public Property GetZusatz3() As String
    Get
      Return _Zusatz3
    End Get
    Set(ByVal value As String)
      _Zusatz3 = value
    End Set
  End Property

  '// Zusatz4
  Dim _Zusatz4 As String
  Public Property GetZusatz4() As String
    Get
      Return _Zusatz4
    End Get
    Set(ByVal value As String)
      _Zusatz4 = value
    End Set
  End Property

  '// Anstellung als
  Dim _Ab_AnstellungAls As String
  Public Property GetAb_AnstellungAls() As String
    Get
      Return _Ab_AnstellungAls
    End Get
    Set(ByVal value As String)
      _Ab_AnstellungAls = value
    End Set
  End Property

  '// Antritt per
  Dim _Ab_AntrittPer As String
  Public Property GetAb_AntrittPer() As String
    Get
      Return _Ab_AntrittPer
    End Get
    Set(ByVal value As String)
      _Ab_AntrittPer = value
    End Set
  End Property

  '// LohnBasis
  Dim _Ab_LohnBasis As Double
  Public Property GetAb_LohnBasis() As Double
    Get
      Return _Ab_LohnBasis
    End Get
    Set(ByVal value As Double)
      _Ab_LohnBasis = value
    End Set
  End Property

  '// LohnAnzahl
  Dim _Ab_LohnAnzahl As Double
  Public Property GetAb_LohnAnzahl() As Double
    Get
      Return _Ab_LohnAnzahl
    End Get
    Set(ByVal value As Double)
      _Ab_LohnAnzahl = value
    End Set
  End Property

  '// LohnBetrag
  Dim _Ab_LohnBetrag As Double
  Public Property GetAb_LohnBetrag() As Double
    Get
      Return _Ab_LohnBetrag
    End Get
    Set(ByVal value As Double)
      _Ab_LohnBetrag = value
    End Set
  End Property

  '// HBasis
  Dim _Ab_HBasis As Double
  Public Property GetAb_HBasis() As Double
    Get
      Return _Ab_HBasis
    End Get
    Set(ByVal value As Double)
      _Ab_HBasis = value
    End Set
  End Property

  '// HAnsatz
  Dim _Ab_HAnsatz As Double
  Public Property GetAb_HAnsatz() As Double
    Get
      Return _Ab_HAnsatz
    End Get
    Set(ByVal value As Double)
      _Ab_HAnsatz = value
    End Set
  End Property

  '// HBetrag
  Dim _Ab_HBetrag As Double
  Public Property GetAb_HBetrag() As Double
    Get
      Return _Ab_HBetrag
    End Get
    Set(ByVal value As Double)
      _Ab_HBetrag = value
    End Set
  End Property

  '// Verrechnung Per
  Dim _Ab_RePer As String
  Public Property GetAb_RePer() As String
    Get
      Return _Ab_RePer
    End Get
    Set(ByVal value As String)
      _Ab_RePer = value
    End Set
  End Property

  '// Abschluss Bemerkung
  Dim _Ab_Bemerkung As String
  Public Property GetAb_Bemerkung() As String
    Get
      Return _Ab_Bemerkung
    End Get
    Set(ByVal value As String)
      _Ab_Bemerkung = value
    End Set
  End Property

#End Region

  Function GetJobNr4Print(ByVal strTemplateBez As String) As String
    Dim strModul2print As String = String.Empty

    'Bruttoumsatzliste Rapporte Total
    Dim strQuery As String = "//SPMALLUtility/frmLL/DiffSetting[@ID=" & Chr(34) & _
                                ClsDataDetail.GetAppGuidValue() & Chr(34) & "]/DocJobNr_LL_" & strTemplateBez.ToString

    Dim strBez As String = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetSQLDataFile(), strQuery)
    If strBez <> String.Empty Then
      strModul2print = strBez

    Else
      strModul2print = "1.2.0"
    End If

    Return strModul2print
  End Function


#Region "Datensatz bearbeiten..."

  Function GetSQLString(ByVal iRecNr As Integer) As String
    Dim strSQL As String = String.Empty

    'If iRecNr <> 0 Then
    '  strSQL = "Update Propose Set Bezeichnung = @Bez, VakNr = @VakNr, "
    '  strSQL &= "P_State = @P_State, P_Art = @P_Art, "
    '  strSQL &= "KD_Tarif = @KD_Tarif, MA_Lohn = @MA_Lohn, "
    '  strSQL &= "P_Anstellung = @P_Anstellung, P_ArbBegin = @P_ArbBegin, "
    '  strSQL &= "P_Zusatz1 = @P_Zusatz1, P_Zusatz2 = @P_Zusatz2, P_Zusatz3 = @P_Zusatz3, P_Zusatz4 = @P_Zusatz4, "
    '  If ClsDataDetail.bAllowedToChange Then
    '    strSQL &= "KST = @KST, Berater = @Berater, MANr = @MANr, KDNr = @KDNr, KDZHDNr = @KDZHDNr, "
    '  End If
    '  strSQL &= "Ab_AnstellungAls = @Ab_AnstellungAls, Ab_AntrittPer = @Ab_AntrittPer, "
    '  strSQL &= "Ab_LohnBas = @Ab_LohnBasis, Ab_LohnAnz = @Ab_LohnAnzahl, Ab_LohnBetrag = @Ab_LohnBetrag, "
    '  strSQL &= "Ab_HBas = @Ab_HBasis, Ab_HAns = @Ab_HAnsatz, Ab_HBetrag = @Ab_HBetrag, "
    '  strSQL &= "Ab_RePer = @Ab_RePer, Ab_Bemerkung = @Ab_Bemerkung, "

    '  strSQL &= "ChangedOn = @ChangedOn, ChangedFrom = @ChangedFrom "
    '  strSQL &= "Where ProposeNr = @RecNr "

    'Else
    '  strSQL = "Insert Into Propose (ProposeNr, MANr, KDNr, KDZHDNr, VakNr, "
    '  strSQL &= "KST, Berater, Bezeichnung, P_State, "
    '  strSQL &= "P_Art, KD_Tarif, MA_Lohn, P_Anstellung, P_ArbBegin, "
    '  strSQL &= "P_Zusatz1, P_Zusatz2, P_Zusatz3, P_Zusatz4, "

    '  strSQL &= "Ab_AnstellungAls, Ab_AntrittPer, "
    '  strSQL &= "Ab_LohnBas, Ab_LohnAnz, Ab_LohnBetrag, "
    '  strSQL &= "Ab_HBas, Ab_HAns, Ab_HBetrag, "
    '  strSQL &= "Ab_RePer, Ab_Bemerkung, "

    '  strSQL &= "CreatedOn, CreatedFrom) Values ("

    '  strSQL &= "@RecNr, @MANr, @KDNr, @KDZHDNr, @VakNr, "
    '  strSQL &= "@KST, @Berater, @Bezeichnung, @P_State, "
    '  strSQL &= "@P_Art, @KD_Tarif, @MA_Lohn, @P_Anstellung, @P_ArbBegin, "
    '  strSQL &= "@P_Zusatz1, @P_Zusatz2, @P_Zusatz3, @P_Zusatz4, "

    '  strSQL &= "@Ab_AnstellungAls, @Ab_AntrittPer, "
    '  strSQL &= "@Ab_LohnBasis, @Ab_LohnAnzahl, @Ab_LohnBetrag, "
    '  strSQL &= "@Ab_HBasis, @Ab_HAnsatz, @Ab_HBetrag, "
    '  strSQL &= "@Ab_RePer, @Ab_Bemerkung, "

    '  strSQL &= "@CreatedOn, @CreatedFrom)"

    'End If

    'ClsDataDetail.SQLQuery = strSQL

    Return strSQL
  End Function

  Function GetSQLString4KontaktDb(ByVal b4KD As Boolean) As String
    Dim strSQL As String = String.Empty

    If b4KD Then
      strSQL = "Insert Into KD_KontaktTotal (KDNr, KDZNr, KontaktDate,Kontakte, RecNr, "
      strSQL &= "KontaktType1, KontaktType2, "
      strSQL &= "KontaktDauer, KontaktWichtig, KontaktErledigt, MANr, ProposeNr, VakNr, OfNr, "
      strSQL &= "CreatedOn, CreatedFrom) Values "

      strSQL &= "(@KDNr, @KDZNr, @KontaktDate, @Kontakte, @RecNr, "
      strSQL &= "@KontaktType1, @KontaktType2, "
      strSQL &= "@KontaktDauer, @KontaktWichtig, @KontaktErledigt, @MANr, @ProposeNr, @VakNr, @OfNr, "
      strSQL &= "@Createdon, @CreatedFrom)"

    Else
      strSQL = "Insert Into MA_Kontakte (MANr, KontaktDate,Kontakte, RecNr, "
      strSQL &= "KontaktType1, KontaktType2, "
      strSQL &= "KontaktDauer, KontaktWichtig, KontaktErledigt, ProposeNr, VakNr, OfNr, "
      strSQL &= "CreatedOn, CreatedFrom) Values "

      strSQL &= "(@MANr, @KontaktDate, @Kontakte, @RecNr, "
      strSQL &= "@KontaktType1, @KontaktType2, "
      strSQL &= "@KontaktDauer, @KontaktWichtig, @KontaktErledigt, @ProposeNr, @VakNr, @OfNr, "
      strSQL &= "@Createdon, @CreatedFrom)"

    End If

    Return strSQL
  End Function

  Function GetNewRecNr() As Integer
    Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
    Dim iResult As Integer = 1
    Dim strQuery As String = "Select Top 1 ProposeNr From Propose Order By ProposeNr Desc"
    Conn.Open()

    Dim cmd As System.Data.SqlClient.SqlCommand
    cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
    Dim rKDrec As SqlDataReader = cmd.ExecuteReader          ' Datenbank


    Try
      While rKDrec.Read
        iResult += CInt(rKDrec("ProposeNr").ToString)
      End While
      If Not rKDrec.HasRows Then iResult = 1

    Catch ex As Exception


    End Try


    Return iResult
  End Function

  Function GetNewKontaktRecNr(ByVal b4KD As Boolean) As Integer
    Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
    Dim iResult As Integer = 1
    Dim strQuery As String = String.Format("Select Top 1 RecNr From {0} Order By RecNr Desc", _
                                          If(b4KD, "KD_KontaktTotal", "MA_Kontakte"))
    Conn.Open()

    Dim cmd As System.Data.SqlClient.SqlCommand
    cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
    Dim rKDrec As SqlDataReader = cmd.ExecuteReader          ' Datenbank


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
    Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)

    Dim strQuery As String = "[Delete Selected Proposerec]"

    Try
      Conn.Open()
      Dim cmd As System.Data.SqlClient.SqlCommand
      cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
      cmd.CommandType = CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter

      param = cmd.Parameters.AddWithValue("@PNumber", iRecNr)
      param = cmd.Parameters.AddWithValue("@USInfo", _ClsProgSetting.GetUserLName)

      cmd.ExecuteNonQuery()
      cmd.Parameters.Clear()


    Catch ex As Exception
      MsgBox(ex.Message, MsgBoxStyle.Critical, "DeleteSelectedRec_0")

    End Try


  End Sub

#End Region

#Region "Funktionen zur Suche nach Daten..."

  
  

  Function GetVorstellungSQLString(ByVal iRecNr As Integer) As String
    Dim sSql As String = String.Empty
    Dim sSqlLen As Integer = 0
    Dim sZusatzBez As String = String.Empty
    Dim i As Integer = 0

    Dim strQuery As String = "//SPMALLUtility/SPMALLUtility/SQLString[@ID=" & Chr(34) & _
                                ClsDataDetail.GetAppGuidValue() & Chr(34) & "]/SQL_1"

    Dim strBez As String = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetSQLDataFile(), strQuery)
    If strBez <> String.Empty Then
      sSql = strBez

    Else
      sSql = "Select Top 20 MAJt.* From MA_JobTermin MAJt "
      sSql &= "Where MAJt.ProposeNr = @iRecNr "
      sSql &= "Order By MAJt.TerminDate Desc"

    End If
    sSqlLen = Len(sSql)

    Return sSql
  End Function

  Function GetLstItems(ByVal lst As ListBox) As String
    Dim strBerufItems As String = String.Empty

    For i As Integer = 0 To lst.Items.Count - 1
      strBerufItems += lst.Items(i).ToString & "#@"
    Next

    Return Left(strBerufItems, Len(strBerufItems) - 2)
  End Function

#End Region


End Class