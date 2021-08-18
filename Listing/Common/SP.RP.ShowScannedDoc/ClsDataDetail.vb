

Imports SPProgUtility
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.CommonXmlUtility

Imports System.Windows.Forms

Public Class ClsDataDetail


	''' <summary>
	''' The translation value helper.
	''' </summary>
	Public Shared m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper
	Public Shared m_InitialData As SP.Infrastructure.Initialization.InitializeClass

	'Public Shared TranslationData As Dictionary(Of String, ClsTranslationData)
	'Public Shared ProsonalizedData As Dictionary(Of String, ClsProsonalizedData)
	'Public Shared MDData As New SPProgUtility.ClsMDData
	'Public Shared UserData As New SPProgUtility.ClsUserData

	'Private Shared m_ModuleCache As SP.ModuleCaching.ModuleCache

	'Public Shared Function GetModuleCach() As SP.ModuleCaching.ModuleCache

	'	If m_ModuleCache Is Nothing Then
	'		m_ModuleCache = New SP.ModuleCaching.ModuleCache()
	'		m_ModuleCache.MaxCustomerFormsToCache = 2
	'		m_ModuleCache.MaxResponsiblePersonFormsToCache = 2
	'		m_ModuleCache.MaxEmployeeFormsToCache = 2
	'	End If

	'	Return m_ModuleCache

	'End Function


	'Public Shared Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

	'	Dim m_md As New SPProgUtility.Mandanten.Mandant
	'	Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
	'	Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
	'	Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

	'	Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
	'	Dim translate = clsTransalation.GetTranslationInObject

	'	Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

	'End Function


  Public Shared ReadOnly Property GetAppGuidValue() As String
    Get
      Return "fa4b54ea-f486-492a-b65b-e99ddadff521"
    End Get
  End Property

  '// Query für Datensuche [Sputnik Scanjobs]
  Shared _strScanConnString As String
  Public Shared ReadOnly Property GetScanDbConnString() As String
    Get
      Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
      Dim m_path As New SPProgUtility.ProgPath.ClsProgPath
      Dim _strScanConnString As String = m_path.GetScanDbData.ScanDbConn  '_ClsProgSetting.GetConnString4ScanJobs


      If _strScanConnString Is Nothing Then Return String.Empty
      If _strScanConnString.Length < 10 Then
        Dim _ClsReg As New SPProgUtility.ClsDivReg
        Dim _strNormalDbConnString As String = String.Empty
        _strNormalDbConnString = _ClsReg.GetINIString(_ClsProgSetting.GetInitIniFile, "ScanDb", "ConnStr_Net", String.Empty)
        _strScanConnString = _strNormalDbConnString

        _ClsReg.SetINIString(_ClsProgSetting.GetInitIniFile, "ScanDb", "ConnStr_Net", _strScanConnString)
        _ClsReg.SetRegKeyValue("Software\ajande.com\Sputnik Suite\Options\DbSelections", _
                                  "Connection String ScanJobs.Net", _strScanConnString)
      End If

      Return (_strScanConnString)
    End Get
  End Property

  '// Query für Datensuche [Sputnik ...]
  Shared _strConnString As String
  Public Shared ReadOnly Property GetDbConnString() As String
    Get
      Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
      Dim _strConnString As String = _ClsProgSetting.GetConnString()

      Return (_strConnString)
    End Get
  End Property

  '// Query für [Sputnik DbSelect]
  Shared _strRootConnString As String
  Public Shared ReadOnly Property GetDbRootConnString() As String
    Get
      Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
      Dim _strRootConnString As String = _ClsProgSetting.GetDbSelectConnString()

      Return _strRootConnString
    End Get
  End Property

  '// SQL-Query
  Shared _SQLString As String
  Public Shared Property SQLQuery() As String
    Get
      Return _SQLString
    End Get
    Set(ByVal value As String)
      _SQLString = value
    End Set
  End Property

  '// SelectedNumbers
  Shared _SelectedNumbers As String = ""
  Public Shared Property GetSelectedNumbers() As String
    Get
      Return _SelectedNumbers
    End Get
    Set(ByVal value As String)
      _SelectedNumbers = value
    End Set
  End Property

  '// SelectedBez
  Shared _SelectedBez As String = ""
  Public Shared Property GetSelectedBez() As String
    Get
      Return _SelectedBez
    End Get
    Set(ByVal value As String)
      _SelectedBez = value
    End Set
  End Property

  '// Darf geändert werden?
  Shared _bAllowedChange As Boolean
  Public Shared Property bAllowedToChange() As Boolean
    Get
      Return _bAllowedChange
    End Get
    Set(ByVal value As Boolean)
      _bAllowedChange = value
    End Set
  End Property

  '// Darf KST geändert werden?
  Shared _bAllowedChangeKST As Boolean
  Public Shared Property bAllowedTochangeKST() As Boolean
    Get
      Return _bAllowedChangeKST
    End Get
    Set(ByVal value As Boolean)
      _bAllowedChangeKST = value
    End Set
  End Property

  '// Ist der Satz New?
  Shared _bAsNew As Boolean
  Public Shared Property bAsNew() As Boolean
    Get
      Return _bAsNew
    End Get
    Set(ByVal value As Boolean)
      _bAsNew = value
    End Set
  End Property

  '// Spaltenbreite von MAVorstellung
  Shared _bAllowedWriteMAVorColWidth As Boolean
  Public Shared Property bAllowedWriteMAVorColWidth() As Boolean
    Get
      Return _bAllowedWriteMAVorColWidth
    End Get
    Set(ByVal value As Boolean)
      _bAllowedWriteMAVorColWidth = value
    End Set
  End Property

  '// Spaltenbreite von MAKontakt
  Shared _bAllowedWriteMAKontaktColWidth As Boolean
  Public Shared Property bAllowedWriteMAKontaktColWidth() As Boolean
    Get
      Return _bAllowedWriteMAKontaktColWidth
    End Get
    Set(ByVal value As Boolean)
      _bAllowedWriteMAKontaktColWidth = value
    End Set
  End Property

  '// Spaltenbreite von KDKontakt
  Shared _bAllowedWriteKDKontaktColWidth As Boolean
  Public Shared Property bAllowedWriteKDKontaktColWidth() As Boolean
    Get
      Return _bAllowedWriteKDKontaktColWidth
    End Get
    Set(ByVal value As Boolean)
      _bAllowedWriteKDKontaktColWidth = value
    End Set
  End Property

  '// Vakanzen-Nummer
  Shared _iVNr As Integer
  Public Shared Property GetVakanzNr() As Integer
    Get
      Return _iVNr
    End Get
    Set(ByVal value As Integer)
      _iVNr = value
    End Set
  End Property

  '// Kandidatennummer
  Shared _iMANr As Integer
  Public Shared Property GetProposalMANr() As Integer
    Get
      Return _iMANr
    End Get
    Set(ByVal value As Integer)
      _iMANr = value
    End Set
  End Property

  '// Kundenennummer
  Shared _iKDNr As Integer
  Public Shared Property GetProposalKDNr() As Integer
    Get
      Return _iKDNr
    End Get
    Set(ByVal value As Integer)
      _iKDNr = value
    End Set
  End Property

  '// ZHD-Nummer
  Shared _iZHDNr As Integer
  Public Shared Property GetProposalZHDNr() As Integer
    Get
      Return _iZHDNr
    End Get
    Set(ByVal value As Integer)
      _iZHDNr = value
    End Set
  End Property


  ' Helps extracting a column value form a data reader.
  Public Shared Function GetColumnTextStr(ByVal dr As SqlClient.SqlDataReader, _
                                          ByVal columnName As String, ByVal replacementOnNull As String) As String

    If Not dr.IsDBNull(dr.GetOrdinal(columnName)) Then
      If String.IsNullOrEmpty(CStr(dr(columnName))) Then
        Return replacementOnNull
      End If
      Return CStr(dr(columnName))
    End If

    Return replacementOnNull
  End Function

End Class
