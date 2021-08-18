
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

	Public Shared ReadOnly Property ChangeMandantData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass
		Get
			Dim m_md As New SPProgUtility.Mandanten.Mandant
			Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
			Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
			Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

			Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
			Dim translate = clsTransalation.GetTranslationInObject

			m_InitialData = New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

			Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

		End Get
	End Property

	Shared Function GetAppGuidValue() As String
		Return "942ECD22-2128-4962-8C59-2F0447BE6F6A"
	End Function


	'#Region "Datenbank Connection auswählen..."

	'  '// Query für Datensuche
	'  Shared _strConnString As String
	'  Public Shared Property GetDbConnString() As String
	'    Get
	'      If GetSelectedDbName Is Nothing Then
	'        Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	'        _strConnString = _ClsProgSetting.GetConnString()
	'      Else
	'        _strConnString = GetSelectedMDDbConnString()

	'      End If
	'      _strConnString = EnablingMarsintoConnString(_strConnString)

	'      Return (_strConnString)
	'    End Get
	'    Set(ByVal value As String)
	'      _strConnString = value
	'    End Set
	'  End Property

	'  '// Query für Datensuche
	'  Shared _strRootConnString As String
	'  Public Shared Property GetDbRootConnString() As String
	'    Get
	'      Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	'      Dim _strRootConnString As String = _ClsProgSetting.GetDbSelectConnString()

	'      Return _strRootConnString
	'    End Get
	'    Set(ByVal value As String)
	'      _strRootConnString = value
	'    End Set
	'  End Property

	'  '// Query für selektierten Mandanten
	'  Shared _strSelectedMDConnString As String
	'  Public Shared Property GetSelectedMDDbConnString() As String
	'    Get
	'      Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	'      Dim _aConnString As String() = _ClsProgSetting.GetConnString().Split(CChar(";"))
	'      For i As Integer = 0 To _aConnString.Length - 1
	'        If _aConnString(i).ToString.ToLower.Contains("initial catalog") Then
	'          _aConnString(i) = String.Format("Initial Catalog={0}", GetSelectedDbName)
	'        End If
	'      Next

	'      Dim AtS As New System.Text.StringBuilder
	'      For i As Integer = 0 To _aConnString.Length - 1
	'        AtS.Append(_aConnString(i))
	'        If i <> _aConnString.Length - 1 Then
	'          AtS.Append(";")
	'        End If
	'      Next
	'      _strSelectedMDConnString = AtS.ToString

	'      Return (AtS.ToString)
	'    End Get
	'    Set(ByVal value As String)
	'      _strSelectedMDConnString = value
	'    End Set
	'  End Property

	'  '// Datenbankname wenn anderes ausgewählt wird
	'  Shared _strSelectedDbName As String
	'  Public Shared Property GetSelectedDbName() As String
	'    Get
	'      Return _strSelectedDbName
	'    End Get
	'    Set(ByVal value As String)
	'      _strSelectedDbName = value
	'    End Set
	'  End Property


	'#End Region


	'// Monat
	Shared _sLP As Short
  Public Shared Property GetLP() As Short
    Get
      Return _sLP
    End Get
    Set(ByVal value As Short)
      _sLP = value
    End Set
  End Property

  '// Jahr
  Shared _strYear As String
  Public Shared Property GetYear() As String
    Get
      Return _strYear
    End Get
    Set(ByVal value As String)
      _strYear = value
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
