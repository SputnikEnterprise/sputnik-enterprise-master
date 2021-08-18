
Public Class ClsDataDetail

  Public Shared strValueData As String
  Public Shared strButtonValue As String

  Public Shared Get4What As String

  Public Shared GetSortBez As String
  Public Shared GetFilterBez As String
  Public Shared GetFilterBez2 As String
  Public Shared GetFilterBez3 As String
  Public Shared GetFilterBez4 As String




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





  Public Shared ReadOnly Property GetAppGuidValue() As String
    Get
      Return "69040e06-6f21-4638-85e0-74063c7098d0"
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

  '// Query für NUR Sortierung der Ausgabe
  Shared _strSortString_2 As String
  Public Shared Property GetSQLSortString_2() As String
    Get
      Return _strSortString_2
    End Get
    Set(ByVal value As String)
      _strSortString_2 = value
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

End Class
