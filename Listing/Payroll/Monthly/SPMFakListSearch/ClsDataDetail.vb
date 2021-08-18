
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


  Public Shared Function GetAppGuidValue() As String
    Return "90AFE663-C9B2-45c3-83F5-9FB9178B071C"
  End Function



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
