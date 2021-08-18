
Imports System.Data.SqlClient

Public Class ClsDataDetail

  Public Shared Conn As SqlConnection
  Public Shared strValueData As String
  Public Shared strButtonValue As String

  Public Shared Get4What As String

  Public Shared GetSortBez As String
  Public Shared GetFilterBez As String
  Public Shared GetFilterBez2 As String
  Public Shared GetFilterBez3 As String
  Public Shared GetFilterBez4 As String

  Public Shared IsLVLarg As Boolean


	''' <summary>
	''' The translation value helper.
	''' </summary>
	Public Shared m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper
	Public Shared m_InitialData As SP.Infrastructure.Initialization.InitializeClass


  Public Shared Function GetXMLValueByQuery(ByVal strFilename As String, _
                          ByVal strQuery As String, _
                          ByVal strValuebyNull As String) As String
    Dim _ClsReg As New SPProgUtility.ClsDivReg
    Dim bResult As String = String.Empty
    Dim strBez As String = _ClsReg.GetXMLNodeValue(strFilename, strQuery)

    If strBez = String.Empty Then strBez = strValuebyNull

    Return strBez
  End Function

  Public Shared ReadOnly Property GetMailDbName() As String
    Get
      Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
      Dim strDatabaseName As String = "Sputnik_MailDb"
      Dim strQuery As String = String.Format("//Mailing/Mail-Database")
      strDatabaseName = ClsDataDetail.GetXMLValueByQuery(_ClsProgSetting.GetMDData_XMLFile, _
                                                                               strQuery, strDatabaseName)

      Return strDatabaseName

    End Get
  End Property

  Public Shared ReadOnly Property GetAppGuidValue() As String
    Get
      Return "09dbe069-23e5-40b4-93f1-a57b47b615bc"
    End Get
  End Property

  '// Query für Datensuche
  Shared _strConnString As String
  Public Shared Property GetDbConnString() As String
    Get
      Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
      Dim _strConnString As String = _ClsProgSetting.GetConnString()

      Return (_strConnString)
    End Get
    Set(ByVal value As String)
      _strConnString = value
    End Set
  End Property

  '// Query für Datensuche
  Shared _strRootConnString As String
  Public Shared Property GetDbRootConnString() As String
    Get
      Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
      Dim _strRootConnString As String = _ClsProgSetting.GetDbSelectConnString()

      Return _strRootConnString
    End Get
    Set(ByVal value As String)
      _strRootConnString = value
    End Set
  End Property

  '// Offertennummer
  Shared _iOffNr As Integer
  Public Shared Property GetOffNr() As Integer
    Get
      Return _iOffNr
    End Get
    Set(ByVal value As Integer)
      _iOffNr = value
    End Set
  End Property

  '// Kundennummer
  Shared _iKDNr As Integer
  Public Shared Property GetKDNr() As Integer
    Get
      Return _iKDNr
    End Get
    Set(ByVal value As Integer)
      _iKDNr = value
    End Set
  End Property

  '// ZHD-Nummer
  Shared _iZHDNr As Integer
  Public Shared Property GetZHDNr() As Integer
    Get
      Return _iZHDNr
    End Get
    Set(ByVal value As Integer)
      _iZHDNr = value
    End Set
  End Property

  '// Kandidatennummer
  Shared _iMANr As Integer
  Public Shared Property GetMANr() As Integer
    Get
      Return _iMANr
    End Get
    Set(ByVal value As Integer)
      _iMANr = value
    End Set
  End Property

  '// Versandfeld
  Shared _streMailField As String
  Public Shared Property GeteMailFieldToSend() As String
    Get
      Return _streMailField
    End Get
    Set(ByVal value As String)
      _streMailField = value
    End Set
  End Property

  '// From-Adresse
  Shared _streMailFrom As String
  Public Shared Property GeteMailFrom() As String
    Get
      Return _streMailFrom
    End Get
    Set(ByVal value As String)
      _streMailFrom = value
    End Set
  End Property

  '// Temporäre LOG-Datei
  Shared _strTempLogFile As String
  Public Shared Property GetTempLogFile() As String
    Get
      Return _strTempLogFile
    End Get
    Set(ByVal value As String)
      _strTempLogFile = value
    End Set
  End Property

  '// Dokname
  Shared _strAttachmentFile As String()
  Public Shared Property GetAttachmentFile() As String()
    Get
      Return _strAttachmentFile
    End Get
    Set(ByVal value As String())
      _strAttachmentFile = value
    End Set
  End Property

  '// ob das Mail bereits versendet wurde?
  Shared _bCheckSentMail As Boolean
  Public Shared Property CheckForMailSent() As Boolean
    Get
      Return _bCheckSentMail
    End Get
    Set(ByVal value As Boolean)
      _bCheckSentMail = value
    End Set
  End Property

  '// Ist es ein selbstangehängte Attachment?
  Shared _bIndAttachmentFile As Boolean
  Public Shared Property IsAttachedFileInd() As Boolean
    Get
      Return _bIndAttachmentFile
    End Get
    Set(ByVal value As Boolean)
      _bIndAttachmentFile = value
    End Set
  End Property

  '// Message_ID
  Shared _strMessageGuid As String
  Public Shared Property GetMessageGuid() As String
    Get
      Return _strMessageGuid
    End Get
    Set(ByVal value As String)
      _strMessageGuid = value
    End Set
  End Property

  '// Mail als Html senden?
  Shared _bSendAsHtml As Boolean
  Public Shared Property SendAsHtml() As Boolean
    Get
      Return _bSendAsHtml
    End Get
    Set(ByVal value As Boolean)
      _bSendAsHtml = value
    End Set
  End Property

  '// MA-Doks auch mitsenden?
  Shared _bWithMADoks As Boolean
  Public Shared Property SendWithMADoks() As Boolean
    Get
      Return _bWithMADoks
    End Get
    Set(ByVal value As Boolean)
      _bWithMADoks = value
    End Set
  End Property

  '// Presentation-Doks auch mitsenden?
  Shared _bWithPDoks As Boolean
  Public Shared Property SendWithPDoks() As Boolean
    Get
      Return _bWithPDoks
    End Get
    Set(ByVal value As Boolean)
      _bWithPDoks = value
    End Set
  End Property

  '// Query für Datensuche
  Shared _strSQLString As String
  Public Shared Property GetSQLQuery() As String
    Get
      Return _strSQLString
    End Get
    Set(ByVal value As String)
      _strSQLString = value
    End Set
  End Property

  '// Query für Datensuche
  Shared _strSQLString_1 As String
  Public Shared Property GetSQLQuery4Print() As String
    Get
      Return _strSQLString_1
    End Get
    Set(ByVal value As String)
      _strSQLString_1 = value
    End Set
  End Property

  '// Query für NUR Sortierung der Daten
  Shared _strFilialBez As String
  Public Shared Property GetFilialBez4Print() As String
    Get
      Return _strFilialBez
    End Get
    Set(ByVal value As String)
      _strFilialBez = value
    End Set
  End Property

  '// Query für NUR Sortierung der Daten
  Shared _strSortString As String
  Public Shared Property GetSQLSortString() As String
    Get
      Return _strSortString
    End Get
    Set(ByVal value As String)
      _strSortString = value
    End Set
  End Property

  '// ModulToPrint für Drucken
  Shared _strModulToprint As String
  Public Shared Property GetModulToPrint() As String
    Get
      Return _strModulToprint
    End Get
    Set(ByVal value As String)
      _strModulToprint = value
    End Set
  End Property

  '// Vorlage für eMail
  Shared _strTemplateFilename As String
  Public Shared Property GetMailTemplateFilename() As String
    Get
      Return _strTemplateFilename
    End Get
    Set(ByVal value As String)
      _strTemplateFilename = value
    End Set
  End Property

  '// Tapi_Called
  Shared _bFirstCall As Boolean
  Public Shared Property IsFirstTapiCall() As Boolean
    Get
      Return _bFirstCall
    End Get
    Set(ByVal value As Boolean)
      _bFirstCall = value
    End Set
  End Property

  '// Neue Version von Suchen...
  Shared _bIsNewVersion As Boolean
  Public Shared Property IsNewVersion() As Boolean
    Get
      Return _bIsNewVersion
    End Get
    Set(ByVal value As Boolean)
      _bIsNewVersion = value
    End Set
  End Property

End Class
