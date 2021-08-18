
'Imports SPProgUtility.CommonSettings
'Imports SPProgUtility.Mandanten
'Imports SPProgUtility.MainUtilities
'Imports SPS.MD.ESRDTAUtility.ClsDataDetail


'Public Class ClsDivFunc

'#Region "Diverses"

'  '// Get4What._strModul4What
'  Dim _strModul4What As String
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

'  '// Firmenname
'  Dim _strKDName As String
'  Public Property GetKDName() As String
'    Get
'      Return _strKDName
'    End Get
'    Set(ByVal value As String)
'      _strKDName = value
'    End Set
'  End Property

'  '// KDZhd
'  Dim _iKDZhdNr As Integer
'  Public Property GetKDZhdNr() As Integer
'    Get
'      Return _iKDZhdNr
'    End Get
'    Set(ByVal value As Integer)
'      _iKDZhdNr = value
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


'End Class

'Public Class ClsDbFunc

'	Private m_Common As New CommonSetting

'	Private m_md As Mandant
'  Private m_utility As Utilities


'  '// RecNr(= Datensatznr)
'  Dim _RecNr As Integer
'  Public Property RecNr() As Integer
'    Get
'      Return _RecNr
'    End Get
'    Set(ByVal value As Integer)
'      _RecNr = value
'    End Set
'  End Property


'#Region "Constructor"

'  Sub New()

'    m_utility = New Utilities
'    m_md = New Mandant

'  End Sub


'#End Region



'#Region "Funktionen für Speichern der ESR-Daten..."

'  '// RecBez
'  Dim _RecBez As String
'  Public Property RecBez() As String
'    Get
'      Return _RecBez
'    End Get
'    Set(ByVal value As String)
'      _RecBez = value
'    End Set
'  End Property

'  '// MD-Identifikaiton 
'  Dim _MDID As String
'  Public Property MDID() As String
'    Get
'      Return _MDID
'    End Get
'    Set(ByVal value As String)
'      _MDID = value
'    End Set
'  End Property

'  '// ESRKonto1
'  Dim _ESRKonto1 As String
'  Public Property ESRKonto1() As String
'    Get
'      Return _ESRKonto1
'    End Get
'    Set(ByVal value As String)
'      _ESRKonto1 = value
'    End Set
'  End Property
'  '// ESRKonto2 
'  Dim _ESRKonto2 As String
'  Public Property ESRKonto2() As String
'    Get
'      Return _ESRKonto2
'    End Get
'    Set(ByVal value As String)
'      _ESRKonto2 = value
'    End Set
'  End Property

'  '// Dateiname(= Dateiname)
'  Dim _ESRFilename As String
'  Public Property ESRFileName() As String
'    Get
'      Return _ESRFilename
'    End Get
'    Set(ByVal value As String)
'      _ESRFilename = value
'    End Set
'  End Property

'  '// Bankname
'  Dim _ESRBankname As String
'  Public Property ESRBankname() As String
'    Get
'      Return _ESRBankname
'    End Get
'    Set(ByVal value As String)
'      _ESRBankname = value
'    End Set
'  End Property

'  '// Clearingnummer
'  Dim _ESRClNr As String
'  Public Property ESRCLNr() As String
'    Get
'      Return _ESRCLNr
'    End Get
'    Set(ByVal value As String)
'      _ESRClNr = value
'    End Set
'  End Property

'  '// BankAdresse
'  Dim _ESRBankAdresse As String
'  Public Property ESRBankAdresse() As String
'    Get
'      Return _ESRBankAdresse
'    End Get
'    Set(ByVal value As String)
'      _ESRBankAdresse = value
'    End Set
'  End Property

'  '// Bankswift
'  Dim _ESRSwift As String
'  Public Property ESRSwift() As String
'    Get
'      Return _ESRSwift
'    End Get
'    Set(ByVal value As String)
'      _ESRSwift = value
'    End Set
'  End Property

'  '// IBAN1
'  Dim _ESRIBAN1 As String
'  Public Property ESRIBAN1() As String
'    Get
'      Return _ESRIBAN1
'    End Get
'    Set(ByVal value As String)
'      _ESRIBAN1 = value
'    End Set
'  End Property

'  '// IBAN2
'  Dim _ESRIBAN2 As String
'  Public Property ESRIBAN2() As String
'    Get
'      Return _ESRIBAN2
'    End Get
'    Set(ByVal value As String)
'      _ESRIBAN2 = value
'    End Set
'  End Property

'  '// IBAN3
'  Dim _ESRIBAN3 As String
'  Public Property ESRIBAN3() As String
'    Get
'      Return _ESRIBAN3
'    End Get
'    Set(ByVal value As String)
'      _ESRIBAN3 = value
'    End Set
'  End Property

'#End Region

'#Region "DTA-Daten..."

'  '// DTAClearingnr
'  Dim _DTAClNr As String
'  Public Property DTAClNr() As String
'    Get
'      Return _DTAClNr
'    End Get
'    Set(ByVal value As String)
'      _DTAClNr = value
'    End Set
'  End Property

'  '// DTAKonto
'  Dim _DTAKontoNr As String
'  Public Property DTAKontoNr() As String
'    Get
'      Return _DTAKontoNr
'    End Get
'    Set(ByVal value As String)
'      _DTAKontoNr = value
'    End Set
'  End Property

'  '// VGKonto
'  Dim _VGKontoNr As String
'  Public Property VGKontoNr() As String
'    Get
'      Return _VGKontoNr
'    End Get
'    Set(ByVal value As String)
'      _VGKontoNr = value
'    End Set
'  End Property

'  '// DTAIBAN
'  Dim _DTAIBAN As String
'  Public Property DTAIBAN() As String
'    Get
'      Return _DTAIBAN
'    End Get
'    Set(ByVal value As String)
'      _DTAIBAN = value
'    End Set
'  End Property

'  '// VGIBAN
'  Dim _VGIBAN As String
'  Public Property VGIBAN() As String
'    Get
'      Return _VGIBAN
'    End Get
'    Set(ByVal value As String)
'      _VGIBAN = value
'    End Set
'  End Property

'  '// DTAAdr1
'  Dim _DTAAdr1 As String
'  Public Property DTAAdr1() As String
'    Get
'      Return _DTAAdr1
'    End Get
'    Set(ByVal value As String)
'      _DTAAdr1 = value
'    End Set
'  End Property

'  '// DTAAdr2
'  Dim _DTAAdr2 As String
'  Public Property DTAAdr2() As String
'    Get
'      Return _DTAAdr2
'    End Get
'    Set(ByVal value As String)
'      _DTAAdr2 = value
'    End Set
'  End Property

'  '// DTAAdr3
'  Dim _DTAAdr3 As String
'  Public Property DTAAdr3() As String
'    Get
'      Return _DTAAdr3
'    End Get
'    Set(ByVal value As String)
'      _DTAAdr3 = value
'    End Set
'  End Property

'  '// DTAAdr4
'  Dim _DTAAdr4 As String
'  Public Property DTAAdr4() As String
'    Get
'      Return _DTAAdr4
'    End Get
'    Set(ByVal value As String)
'      _DTAAdr4 = value
'    End Set
'  End Property

'  '// Als Standard
'  Dim _AsStandard As Boolean
'  Public Property bAsStandard() As Boolean
'    Get
'      Return _AsStandard
'    End Get
'    Set(ByVal value As Boolean)
'      _AsStandard = value
'    End Set
'  End Property

'  '// für DTA
'  Dim _AsDTA As Boolean
'  Public Property bAsDTA() As Boolean
'    Get
'      Return _AsDTA
'    End Get
'    Set(ByVal value As Boolean)
'      _AsDTA = value
'    End Set
'  End Property

'#End Region


'#Region "Datensatz bearbeiten..."

'  Function GetSQLString4DTA(ByVal bAsNew As Boolean) As String
'    Dim strSQL As String = String.Empty

'    If Not bAsNew Then
'      strSQL = "Update MD_ESRDTA Set RecBez = @RecBez, MDNr = @MDNr, Jahr = @Jahr, MD_ID = @MD_ID, DTACLNr = @DTACLNr, "
'      strSQL &= "KontoDTA = @KontoDTA, KontoVG = @KontoVG, "
'      strSQL &= "DTAIBAN = @DTAIBAN, VGIBAN = @VGIBAN, DTAAdr1 = @DTAAdr1, DTAAdr2 = @DTAAdr2, DTAAdr3 = @DTAAdr3, "
'      strSQL &= "DTAAdr4 = @DTAAdr4, USNr = @USNr, "
'      strSQL &= "ChangedOn = @ChangedOn, ChangedFrom = @ChangedFrom, AsStandard = @AsStandard "
'      strSQL &= "Where RecNr = @RecNr And ModulArt = 0 And MDNr = @MDNr "

'    Else
'      ' neuer Datensatz
'      strSQL = "Insert Into MD_ESRDTA (RecNr, RecBez, MDNr, Jahr, MD_ID, DTAClNr, KontoDTA, KontoVG, "
'      strSQL &= "DTAIBAN, VGIBAN, DTAAdr1, DTAAdr2, DTAAdr3, DTAAdr4, USNr, "
'      strSQL &= "CreatedOn, CreatedFrom, AsStandard, ModulArt) "
'      strSQL &= "Values ("

'      strSQL &= "@RecNr, @RecBez, @MDNr, @Jahr, @MD_ID, @DTAClNr, @KontoDTA, @KontoVG, "
'      strSQL &= "@DTAIBAN, @VGIBAN, @DTAAdr1, @DTAAdr2, @DTAAdr3, @DTAAdr4, @USNr, "
'      strSQL &= "@CreatedOn, @CreatedFrom, @AsStandard, 0) "

'    End If
'    If Me.bAsStandard Then strSQL &= "Update MD_ESRDTA Set AsStandard = 0 Where RecNr <> @RecNr And ModulArt = 0 And MDNr = @MDNr "
'    ClsDataDetail.SQLQuery = strSQL

'    Return strSQL
'  End Function

'  Function GetSQLString4ESR(ByVal bAsNew As Boolean) As String
'    Dim strSQL As String = String.Empty

'    If Not bAsNew Then
'      strSQL = "Update MD_ESRDTA Set RecBez = @RecBez, MDNr = @MDNr, Jahr = @Jahr, "
'      strSQL &= "MD_ID = @MD_ID, KontoESR1 = @KontoESR1, KontoESR2 = @KontoESR2, "
'      strSQL &= "ESRFilename = @ESRFilename, BankName = @Bankname, BankClnr = @BankClNr, "
'      strSQL &= "BankAdresse = @BankAdresse, Swift = @Swift, ESRIBAN1 = @ESRIBAN1, ESRIBAN2 = @ESRIBAN2, "
'      strSQL &= "ESRIBAN3 = @ESRIBAN3, USNr = @USNr, "
'      strSQL &= "ChangedOn = @ChangedOn, ChangedFrom = @ChangedFrom, AsStandard = @AsStandard "
'      strSQL &= "Where RecNr = @RecNr And ModulArt = 1 And MDNr = @MDNr "

'    Else
'      strSQL = "Insert Into MD_ESRDTA (RecNr, RecBez, MDNr, Jahr, MD_ID, KontoESR1, KontoESR2, ESRFilename, BankName, "
'      strSQL &= "BankClnr, BankAdresse, "
'      strSQL &= "Swift, ESRIBAN1, ESRIBAN2, ESRIBAN3, USNr, CreatedOn, CreatedFrom, AsStandard, ModulArt) Values ("
'      strSQL &= "@RecNr, @RecBez, @MDNr, @Jahr, @MD_ID, @KontoESR1, @KontoESR2, "
'      strSQL &= "@ESRFilename, @Bankname, @BankClNr, "
'      strSQL &= "@BankAdresse, @Swift, @ESRIBAN1, @ESRIBAN2, @ESRIBAN3, @USNr, "
'      strSQL &= "@CreatedOn, @CreatedFrom, @AsStandard, 1) "

'    End If
'    If Me.bAsStandard Then strSQL &= "Update MD_ESRDTA Set AsStandard = 0 Where RecNr <> @RecNr And ModulArt = 1 And MDNr = @MDNr "
'    ClsDataDetail.SQLQuery = strSQL

'    Return strSQL
'  End Function

'	Function SaveDataToDTADb(ByVal iRecNr As Integer) As Boolean
'		Dim m_Utilities As New Utilities
'		Dim strQuery As String = GetSQLString4DTA(iRecNr = 0)
'		Dim iNewRecNr As Integer = 0
'		Dim bAsNewrec As Boolean = iRecNr = 0
'		Dim bResult As Boolean

'		If Not bAsNewrec Then
'			iNewRecNr = iRecNr

'		Else
'			iNewRecNr = GetNewRecNr()

'		End If
'		Dim listOfParams As New List(Of SqlClient.SqlParameter)

'		Dim i As Integer = 0
'		Dim _ClsDb As New ClsDbFunc
'		Me.RecNr = iNewRecNr

'		Try
'			'If Me.bAsDTA Then
'			listOfParams.Add(New SqlClient.SqlParameter("@RecNr", Me.RecNr))

'			listOfParams.Add(New SqlClient.SqlParameter("@RecBez", RecBez))
'			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", m_InitialData.MDData.mdnr))
'			listOfParams.Add(New SqlClient.SqlParameter("@Jahr", m_InitialData.MDData.MDYear))

'			listOfParams.Add(New SqlClient.SqlParameter("@MD_ID", MDID))
'			listOfParams.Add(New SqlClient.SqlParameter("@DTACLNr", DTAClNr))
'			listOfParams.Add(New SqlClient.SqlParameter("@KontoDTA", DTAKontoNr))
'			listOfParams.Add(New SqlClient.SqlParameter("@KontoVG", VGKontoNr))
'			listOfParams.Add(New SqlClient.SqlParameter("@DTAIBAN", DTAIBAN))
'			listOfParams.Add(New SqlClient.SqlParameter("@VGIBAN", VGIBAN))
'			listOfParams.Add(New SqlClient.SqlParameter("@DTAAdr1", DTAAdr1))
'			listOfParams.Add(New SqlClient.SqlParameter("@DTAAdr2", DTAAdr2))
'			listOfParams.Add(New SqlClient.SqlParameter("@DTAAdr3", DTAAdr3))
'			listOfParams.Add(New SqlClient.SqlParameter("@DTAAdr4", DTAAdr4))
'			listOfParams.Add(New SqlClient.SqlParameter("@USNr", m_InitialData.USerData.usernr))

'			listOfParams.Add(New SqlClient.SqlParameter("@AsStandard", Me.bAsStandard))

'			If Not bAsNewrec Then
'				listOfParams.Add(New SqlClient.SqlParameter("@ChangedOn", Format(Now.Date, "G")))
'				listOfParams.Add(New SqlClient.SqlParameter("@ChangedFrom", m_Common.GetLogedUserName))

'			Else
'				listOfParams.Add(New SqlClient.SqlParameter("@CreatedOn", Format(Now.Date, "G")))
'				listOfParams.Add(New SqlClient.SqlParameter("@CreatedFrom", m_Common.GetLogedUserName))

'			End If
'			Dim reader = m_Utilities.ExecuteNonQuery(m_InitialData.MDData.MDDbConn, strQuery, listOfParams, CommandType.Text)

'			bResult = True

'		Catch ex As Exception
'			MsgBox(ex.Message, MsgBoxStyle.Critical, "SaveDataToQSTDb_0")
'			Me.RecNr = iNewRecNr
'			bResult = False

'		End Try


'		Return bResult
'	End Function

'	Function SaveDataToESRDb(ByVal iRecNr As Integer) As Boolean
'		'Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)
'		Dim m_Utilities As New Utilities
'		Dim strQuery As String = GetSQLString4ESR(iRecNr = 0)
'		Dim iNewRecNr As Integer = 0
'		Dim bAsNewrec As Boolean = iRecNr = 0
'		Dim bResult As Boolean

'		If Not bAsNewrec Then
'			iNewRecNr = iRecNr

'		Else
'			iNewRecNr = GetNewRecNr()

'		End If
'		Dim listOfParams As New List(Of SqlClient.SqlParameter)

'		Dim i As Integer = 0
'		Dim _ClsDb As New ClsDbFunc
'		Me.RecNr = iNewRecNr

'		Try
'			'Conn.Open()
'			'Dim cmd As System.Data.SqlClient.SqlCommand
'			'cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
'			'Dim param As System.Data.SqlClient.SqlParameter
'			'm_InitialData.MDData.mdnr

'			listOfParams.Add(New SqlClient.SqlParameter("@RecNr", Me.RecNr))

'			listOfParams.Add(New SqlClient.SqlParameter("@RecBez", Me.RecBez))
'			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", m_InitialData.MDData.MDNr))
'			listOfParams.Add(New SqlClient.SqlParameter("@Jahr", m_InitialData.MDData.MDYear))

'			listOfParams.Add(New SqlClient.SqlParameter("@MD_ID", Me.MDID))
'			listOfParams.Add(New SqlClient.SqlParameter("@KontoESR1", Me.ESRKonto1))
'			listOfParams.Add(New SqlClient.SqlParameter("@KontoESR2", Me.ESRKonto2))
'			listOfParams.Add(New SqlClient.SqlParameter("@ESRFilename", Me.ESRFileName))
'			listOfParams.Add(New SqlClient.SqlParameter("@Bankname", Me.ESRBankname))
'			listOfParams.Add(New SqlClient.SqlParameter("@BankClNr", Me.ESRCLNr))
'			listOfParams.Add(New SqlClient.SqlParameter("@BankAdresse", Me.ESRBankAdresse))
'			listOfParams.Add(New SqlClient.SqlParameter("@Swift", Me.ESRSwift))
'			listOfParams.Add(New SqlClient.SqlParameter("@ESRIBAN1", Me.ESRIBAN1))
'			listOfParams.Add(New SqlClient.SqlParameter("@ESRIBAN2", Me.ESRIBAN2))
'			listOfParams.Add(New SqlClient.SqlParameter("@ESRIBAN3", Me.ESRIBAN3))
'			listOfParams.Add(New SqlClient.SqlParameter("@USNr", m_InitialData.USerData.usernr))

'			listOfParams.Add(New SqlClient.SqlParameter("@AsStandard", Me.bAsStandard))

'			If Not bAsNewrec Then
'				listOfParams.Add(New SqlClient.SqlParameter("@ChangedOn", Format(Now.Date, "G")))
'				listOfParams.Add(New SqlClient.SqlParameter("@ChangedFrom", m_Common.GetLogedUserName))

'			Else
'				listOfParams.Add(New SqlClient.SqlParameter("@CreatedOn", Format(Now.Date, "G")))
'				listOfParams.Add(New SqlClient.SqlParameter("@CreatedFrom", m_Common.GetLogedUserName))

'			End If
'			Dim reader = m_Utilities.ExecuteNonQuery(m_InitialData.MDData.MDDbConn, strQuery, listOfParams, CommandType.Text)
'			bResult = reader

'		Catch ex As Exception
'			MsgBox(ex.Message, MsgBoxStyle.Critical, "SaveDataToQSTDb_0")
'			Me.RecNr = iNewRecNr
'			bResult = False

'		End Try


'		Return bResult
'	End Function


'  Function GetNewRecNr() As Integer
'    Dim m_Utilities As New utilities
'    Dim iResult As Integer = 1
'    Dim strQuery As String = "Select Top 1 RECNr From MD_ESRDTA Order By RecNr Desc"


'    Try
'			Dim reader = m_Utilities.OpenReader(m_InitialData.MDData.MDDbConn, strQuery, Nothing, CommandType.Text)
'      If (Not reader Is Nothing) Then
'        If reader.Read Then
'          iResult += CInt(reader("RecNr").ToString)
'        End If
'      End If
'      m_Utilities.CloseReader(reader)

'    Catch ex As Exception

'    End Try

'    Return iResult
'  End Function

'  Sub DeleteSelectedRec(ByVal iRecNr As Integer)
'    Dim m_Utilities As New Utilities
'    Dim strQuery As String = "Delete MD_ESRDTA Where RecNr = @RecID"
'    ' Parameters
'    Dim listOfParams As New List(Of SqlClient.SqlParameter)
'    listOfParams.Add(New SqlClient.SqlParameter("@RecID", iRecNr))

'    Try
'			Dim reader = m_Utilities.ExecuteNonQuery(m_InitialData.MDData.MDDbConn, strQuery, listOfParams, CommandType.Text)

'    Catch ex As Exception
'      MsgBox(ex.Message, MsgBoxStyle.Critical, "DeleteSelectedRec_0")

'    End Try


'  End Sub

'#End Region

'#Region "Funktionen zur Suche nach Daten..."

'  Function GetLocalSQLString(ByVal iRecID As Integer, _
'                             ByVal bSelectAsDTA As Boolean) As String
'    Dim sSql As String = String.Empty
'    Dim sSqlLen As Integer = 0
'    Dim sZusatzBez As String = String.Empty
'    Dim i As Integer = 0

'    sSql = "Select * From MD_ESRDTA Where "
'    If iRecID > 0 Then sSql &= "RecNr = @iRecID And "
'    sSql &= String.Format("ModulArt = {0} And ", If(bSelectAsDTA, 0, 1))
'    sSql &= "MDNr = @MDNr Order By RecNr"

'    Return sSql
'  End Function

'  Function GetRemoteSQLString(ByVal frmTest As frmESRDTA) As String
'    Dim sSql As String = String.Empty
'    Dim sSqlLen As Integer = 0
'    Dim sZusatzBez As String = String.Empty
'    Dim i As Integer = 0

'    sSql = "Select * From MD_ESRDTA Order By RecNr"

'    Return sSql
'  End Function

'  Function GetLstItems(ByVal lst As ListBox) As String
'    Dim strBerufItems As String = String.Empty

'    For i = 0 To lst.Items.Count - 1
'      strBerufItems += lst.Items(i).ToString & "#@"
'    Next

'    Return Left(strBerufItems, Len(strBerufItems) - 2)
'  End Function

'#End Region




'End Class