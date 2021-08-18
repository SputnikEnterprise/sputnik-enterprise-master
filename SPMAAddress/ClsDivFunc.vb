
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
  
  Dim strMyDateFormat As String = _ClsProgSetting.GetSQLDateFormat()
  Dim strConnString As String = _ClsProgSetting.GetConnString()


	Public Property ForEmployment As Boolean?
	Public Property ForReport As Boolean?
	Public Property ForPayroll As Boolean?
	Public Property ForAGB As Boolean?
	Public Property ForZV As Boolean?
	Public Property ForNLA As Boolean?
	Public Property ForDivers As Boolean?



#Region "Funktionen für Speichern der Daten..."

  '// RecNr
  Dim _AddressRecNr As Integer
  Public Property AddressRecNr() As Integer
    Get
      Return _AddressRecNr
    End Get
    Set(ByVal value As Integer)
      _AddressRecNr = value
    End Set
  End Property

  '// Modulnr 
  Dim _ModulNr As Short
  Public Property Add_ModulNr() As Short
    Get
      Return _ModulNr
    End Get
    Set(ByVal value As Short)
      _ModulNr = value
    End Set
  End Property

  '// MANr
  Dim _MANr As Integer
  Public Property Add_MANr() As Integer
    Get
      Return _MANr
    End Get
    Set(ByVal value As Integer)
      _MANr = value
    End Set
  End Property

  '// Geschlecht
  Dim _Geschlecht As String
  Public Property Add_Geschlecht() As String
    Get
      Return _Geschlecht
    End Get
    Set(ByVal value As String)
      _Geschlecht = value
    End Set
  End Property
  '// Nachname
  Dim _Nachname As String
  Public Property Add_Nachname() As String
    Get
      Return _Nachname
    End Get
    Set(ByVal value As String)
      _Nachname = value
    End Set
  End Property
  '// Vorname 
  Dim _Vorname As String
  Public Property Add_Vorname() As String
    Get
      Return _Vorname
    End Get
    Set(ByVal value As String)
      _Vorname = value
    End Set
  End Property
  '// Zusatz
  Dim _Zusatz As String
  Public Property Add_Zusatz() As String
    Get
      Return _Zusatz
    End Get
    Set(ByVal value As String)
      _Zusatz = value
    End Set
  End Property

  '// Postfach (= Postfach)
  Dim _Postfach As String
  Public Property Add_Postfach() As String
    Get
      Return _Postfach
    End Get
    Set(ByVal value As String)
      _Postfach = value
    End Set
  End Property
  '// Strasse (= Strasse)
  Dim _Strasse As String
  Public Property Add_Strasse() As String
    Get
      Return _Strasse
    End Get
    Set(ByVal value As String)
      _Strasse = value
    End Set
  End Property
  '// Land (= Land)
  Dim _Land As String
  Public Property Add_Land() As String
    Get
      Return _Land
    End Get
    Set(ByVal value As String)
      _Land = value
    End Set
  End Property
  '// PLZ (= PLZ)
  Dim _Plz As String
  Public Property Add_Plz() As String
    Get
      Return _Plz
    End Get
    Set(ByVal value As String)
      _Plz = value
    End Set
  End Property
  '// Ort (= Ort)
  Dim _Ort As String
  Public Property Add_Ort() As String
    Get
      Return _Ort
    End Get
    Set(ByVal value As String)
      _Ort = value
    End Set
  End Property

  '// Bemerkung 
  Dim _Bemerkung As String
  Public Property Add_Bemerkung() As String
    Get
      Return _Bemerkung
    End Get
    Set(ByVal value As String)
      _Bemerkung = value
    End Set
  End Property
  '// Add_Res1 
  Dim _Add_Res1 As String
  Public Property Add_Res1() As String
    Get
      Return _Add_Res1
    End Get
    Set(ByVal value As String)
      _Add_Res1 = value
    End Set
  End Property
  '// Add_Res2 
  Dim _Add_Res2 As String
  Public Property Add_Res2() As String
    Get
      Return _Add_Res2
    End Get
    Set(ByVal value As String)
      _Add_Res2 = value
    End Set
  End Property
  '// Add_Res3 
  Dim _Add_Res3 As String
  Public Property Add_Res3() As String
    Get
      Return _Add_Res3
    End Get
    Set(ByVal value As String)
      _Add_Res3 = value
    End Set
  End Property

  '// ActivRec
  Dim _ActivRecNr As Boolean
  Public Property Add_ActivRec() As Boolean
    Get
      Return _ActivRecNr
    End Get
    Set(ByVal value As Boolean)
      _ActivRecNr = value
    End Set
  End Property

#End Region

#Region "Datensatz bearbeiten..."

  Function SaveDataToAddressDb(ByVal iRecNr As Integer) As Boolean
    Dim bResult As Boolean
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)
    Dim strQuery As String = String.Empty
    Dim iNewRecNr As Integer = 0

    If iRecNr <> 0 Then
      iNewRecNr = iRecNr
      strQuery = "Update MA_SAddress Set ModulNr = @ModulNr, MANr = @MANr, Geschlecht = @Geschlecht, Nachname = @Nachname, "
      strQuery &= "Vorname = @Vorname, Wohnt_bei = @Zusatz, Postfach = @Postfach, Strasse = @Strasse, "
      strQuery &= "Land = @Land, PLZ = @PLZ, Ort = @Ort, "
      strQuery &= "Add_Bemerkung = @Add_Bemerkung, Add_Res1 = @Add_Res1, Add_Res2 = @Add_Res2, Add_Res3 = @Add_Res3, "
			strQuery &= "ChangedOn = GetDate(), ChangedFrom = @ChangedFrom, ActiveRec = @ActiveRec "
      strQuery &= "Where RecNr = @RecID"

    Else
      iNewRecNr = GetNewRecNr()
      strQuery = "Insert Into MA_SAddress (ModulNr, MANr, Geschlecht, Nachname, "
      strQuery &= "Vorname, Wohnt_bei, Postfach, Strasse, Land, PLZ, Ort, "
      strQuery &= "Add_Bemerkung, Add_Res1, Add_Res2, Add_Res3, "
      strQuery &= "CreatedOn, CreatedFrom, RecNr, ActiveRec) Values ("

      strQuery &= "@ModulNr, @MANr, @Geschlecht, @Nachname, "
      strQuery &= "@Vorname, @Zusatz, @Postfach, @Strasse, @Land, @PLZ, "
      strQuery &= "@Ort, @Add_Bemerkung, @Add_Res1, @Add_Res2, @Add_Res3, "
			strQuery &= "GetDate(), @ChangedFrom, @RecID, @ActiveRec)"

    End If

    Dim i As Integer = 0
    'Dim _ClsDb As New ClsDbFunc
    Me.AddressRecNr = iNewRecNr

    Try
      Conn.Open()
      Dim cmd As System.Data.SqlClient.SqlCommand
      cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
      Dim param As System.Data.SqlClient.SqlParameter

      param = cmd.Parameters.AddWithValue("@ModulNr", ClsDataDetail.SelectedAddressArt)
      param = cmd.Parameters.AddWithValue("@MANr", ClsDataDetail.GetSelectedMANr)
      param = cmd.Parameters.AddWithValue("@Geschlecht", Me.Add_Geschlecht)
      param = cmd.Parameters.AddWithValue("@Nachname", Me.Add_Nachname)
      param = cmd.Parameters.AddWithValue("@Vorname", Me.Add_Vorname)

      param = cmd.Parameters.AddWithValue("@Zusatz", Me.Add_Zusatz)
      param = cmd.Parameters.AddWithValue("@Postfach", Me.Add_Postfach)
      param = cmd.Parameters.AddWithValue("@Strasse", Me.Add_Strasse)
      param = cmd.Parameters.AddWithValue("@Land", Me.Add_Land)
      param = cmd.Parameters.AddWithValue("@PLZ", Me.Add_Plz)
      param = cmd.Parameters.AddWithValue("@Ort", Me.Add_Ort)

      param = cmd.Parameters.AddWithValue("@Add_Bemerkung", Me.Add_Bemerkung)
      param = cmd.Parameters.AddWithValue("@Add_Res1", Me.Add_Res1)
      param = cmd.Parameters.AddWithValue("@Add_Res2", Me.Add_Res2)
      param = cmd.Parameters.AddWithValue("@Add_Res3", Me.Add_Res3)

			param = cmd.Parameters.AddWithValue("@ChangedFrom", _ClsProgSetting.GetUserName)
      param = cmd.Parameters.AddWithValue("@RecID", iNewRecNr)
      param = cmd.Parameters.AddWithValue("@ActiveRec", Me.Add_ActivRec)

      cmd.ExecuteNonQuery()
      cmd.Parameters.Clear()
      If Me.Add_ActivRec Then UpdateRec4ActivRec(iNewRecNr)

      bResult = True


    Catch ex As Exception
      MsgBox(ex.Message, MsgBoxStyle.Critical, "SaveDataToAddressDb_0")
      bResult = False

    End Try

    Return bResult
  End Function

  Sub UpdateRec4ActivRec(ByVal iRecNr As Integer)
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)
    Dim iResult As Integer = 1
    Dim strQuery As String = "[Update MAAdressData For ActivRec]"
    Conn.Open()

    Dim cmd As System.Data.SqlClient.SqlCommand
    cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
    cmd.CommandType = CommandType.StoredProcedure
    Dim param As System.Data.SqlClient.SqlParameter
    param = cmd.Parameters.AddWithValue("@RecNr", iRecNr)
    param = cmd.Parameters.AddWithValue("@ModulNr", ClsDataDetail.SelectedAddressArt)
    param = cmd.Parameters.AddWithValue("@MANr", ClsDataDetail.GetSelectedMANr)

    Try
      cmd.ExecuteNonQuery()
      cmd.Parameters.Clear()

    Catch ex As Exception


    End Try

  End Sub

  Function GetNewRecNr() As Integer
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)
    Dim iResult As Integer = 1
    Dim strQuery As String = "Select Top 1 RecNr From MA_SAddress Order By RecNr Desc"
    Conn.Open()

    Dim cmd As System.Data.SqlClient.SqlCommand
    cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
    Dim rAddressrec As SqlDataReader = cmd.ExecuteReader

    Try
      While rAddressrec.Read
        iResult += CInt(rAddressrec("RecNr").ToString)
      End While
      If Not rAddressrec.HasRows Then iResult = 1

    Catch ex As Exception


    End Try

    Return iResult
  End Function

  Function GetRecCounts() As List(Of Integer)
    Dim liRecCount As New List(Of Integer)

    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)
    Dim iResult As Integer = 1
    Dim strQuery As String = "Select Count(*) As RecCount, ModulNr From MA_SAddress "
    strQuery &= "Where MANr = @MANr Group By ModulNr Order By ModulNr"
    Conn.Open()
    For i As Integer = 0 To 6
      liRecCount.Add(0)
    Next

    Dim cmd As System.Data.SqlClient.SqlCommand
    cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
    Dim param As System.Data.SqlClient.SqlParameter
    param = cmd.Parameters.AddWithValue("@MANr", ClsDataDetail.GetSelectedMANr)

    Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader

    Try
      While rFoundedrec.Read
        liRecCount.Item(CInt(rFoundedrec("ModulNr").ToString)) = CInt(rFoundedrec("RecCount").ToString)
      End While


    Catch ex As Exception

    End Try

    Return liRecCount
  End Function

  Sub DeleteSelectedRec(ByVal iRecNr As Integer)
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)
    Dim strQuery As String = "Delete MA_SAddress Where RecNr = @RecID"

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

	Function GetLocalSQLString(ByVal iRecID As Integer) As String
		Dim sql As String

		sql = "Select * From MA_SAddress Where "
		sql &= "MANr = @MANr " 'And ModulNr = @ModulNr "
		If iRecID > 0 Then sql &= "And (@iRecID = 0 Or ID = @iRecID) "
		sql &= "Order By RecNr"


		Return sql
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