
Imports System.Data.SqlClient

Public Class ClsDataDetail

  Public Shared Conn As SqlConnection
  Public Shared frmUms As Form
  Public Shared strKDData As String = String.Empty
  Public Shared strButtonValue As String = String.Empty

  Public Shared Get4What As String = String.Empty

  Public Shared GetSortBez As String = String.Empty
  Public Shared GetFilterBez As String = String.Empty
  Public Shared GetFilterBez2 As String = String.Empty
  Public Shared GetFilterBez3 As String = String.Empty
  Public Shared GetFilterBez4 As String = String.Empty

  Public Shared strAllKDNr As String = String.Empty
  Public Shared Property GetKstFullName As String

  Public Shared ReadOnly Property GetAppGuidValue() As String
    Get
      Return "09dbe069-23e5-40b4-93f1-a57b47b615bc"
    End Get
  End Property

  Public Shared Property IsFeiertagAsNetto_2 As Boolean
  Public Shared Property IsFerienAsNetto_2 As Boolean
  Public Shared Property Is13LohnAsNetto_2 As Boolean





	''' <summary>
	''' searchcriteria
	''' </summary>
	''' <remarks></remarks>
	Public Shared SelectionCriteria As SearchCriteria

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

			'clsMandant.MDDbConn = EnablingMarsintoConnString(m_InitialData.MDData.MDDbConn)
			clsMandant.MDDbConn = EnablingMarsintoConnString(clsMandant.MDDbConn)
			m_InitialData = New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

			Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

		End Get
	End Property

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



	''// Query für Datensuche
	'Shared _strConnString As String
	'Public Shared Property GetDbConnString() As String
	'  Get
	'    If GetSelectedDbName Is Nothing Then
	'      Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	'      _strConnString = _ClsProgSetting.GetConnString()
	'    Else
	'      _strConnString = GetSelectedMDDbConnString()

	'    End If
	'    _strConnString = EnablingMarsintoConnString(_strConnString)

	'    Return (_strConnString)
	'  End Get
	'  Set(ByVal value As String)
	'    _strConnString = value
	'  End Set
	'End Property

	''// Query für Datensuche
	'Shared _strRootConnString As String
	'Public Shared Property GetDbRootConnString() As String
	'  Get
	'    Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	'    Dim _strRootConnString As String = _ClsProgSetting.GetDbSelectConnString()

	'    Return _strRootConnString
	'  End Get
	'  Set(ByVal value As String)
	'    _strRootConnString = value
	'  End Set
	'End Property

	''// Query für selektierten Mandanten
	'Shared _strSelectedMDConnString As String
	'Public Shared Property GetSelectedMDDbConnString() As String
	'  Get
	'    Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	'    Dim _aConnString As String() = _ClsProgSetting.GetConnString().Split(CChar(";"))
	'    For i As Integer = 0 To _aConnString.Length - 1
	'      If _aConnString(i).ToString.ToLower.Contains("initial catalog") Then
	'        _aConnString(i) = String.Format("Initial Catalog={0}", GetSelectedDbName)
	'      End If
	'    Next

	'    Dim AtS As New System.Text.StringBuilder
	'    For i As Integer = 0 To _aConnString.Length - 1
	'      AtS.Append(_aConnString(i))
	'      If i <> _aConnString.Length - 1 Then
	'        AtS.Append(";")
	'      End If
	'    Next
	'    _strSelectedMDConnString = AtS.ToString

	'    Return (AtS.ToString)
	'  End Get
	'  Set(ByVal value As String)
	'    _strSelectedMDConnString = value
	'  End Set
	'End Property


	''// Datenbankname wenn anderes ausgewählt wird
	'Shared _strSelectedDbName As String
	'Public Shared Property GetSelectedDbName() As String
	'  Get
	'    Return _strSelectedDbName
	'  End Get
	'  Set(ByVal value As String)
	'    _strSelectedDbName = value
	'  End Set
	'End Property





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

  '// XMarge in MD
	Shared _xMarge As Decimal
	Public Shared Property GetXMarge() As Decimal
		Get
			Return _xMarge
		End Get
		Set(ByVal value As Decimal)
			_xMarge = value
		End Set
	End Property

  '// XMarge in MD
	Shared _xMarge_2 As Decimal
	Public Shared Property GetXMarge_2() As Decimal
		Get
			Return _xMarge_2
		End Get
		Set(ByVal value As Decimal)
			_xMarge_2 = value
		End Set
	End Property

  '// Temporärumsatz
  Shared _dTemp As Double
  Public Shared Property GetTotalTemp() As Double
    Get
      Return _dTemp
    End Get
    Set(ByVal value As Double)
      _dTemp = value
    End Set
  End Property

  '// Sonstigeumsatz
  Shared _dInd As Double
  Public Shared Property GetTotalInd() As Double
    Get
      Return _dInd
    End Get
    Set(ByVal value As Double)
      _dInd = value
    End Set
  End Property

  '// Festumsatz
  Shared _dFest As Double
  Public Shared Property GetTotalFest() As Double
    Get
      Return _dFest
    End Get
    Set(ByVal value As Double)
      _dFest = value
    End Set
  End Property

  '// Sorted from frmkdsearch_LV
  Shared _strSortBez As String
  Public Shared Property GetLVSortBez() As String
    Get
      Return _strSortBez
    End Get
    Set(ByVal value As String)
      _strSortBez = value
    End Set
  End Property

  '// Automatische Benutzernummer
  Shared _iUSNr As Integer
  Public Shared Property GetAutoUserNr() As Integer
    Get
      Return _iUSNr
    End Get
    Set(ByVal value As Integer)
      _iUSNr = value
    End Set
  End Property

  '// Automatische Benutzernummer
  Shared _strAutoConn As String = String.Empty
  Public Shared Property GetAutoConnString() As String
    Get
      Return _strAutoConn
    End Get
    Set(ByVal value As String)
      _strAutoConn = value
    End Set
  End Property

  '// Automatischer Start
  Shared _bAutomated As Boolean
  Public Shared Property IsAutomaatedStart() As Boolean
    Get
      Return _bAutomated
    End Get
    Set(ByVal value As Boolean)
      _bAutomated = value
    End Set
  End Property

  '// Variablen für den Form
  ' 0 = Sortierung
  ' 1 = Monat von 
  ' 2 = Monat bis
  ' 3 = Jahr von
  ' 4 = Jahr bis
  ' 5 = Berater
  ' 6 = Filiale
  ' 7 = Branche
  ' 8 = KD-Kanton
  ' 9 = KD-Ort
  ' 10 = MA-Nationalität
  ' 11 = MA-Land
  Shared _libfrmVars As New List(Of String)
  Public Shared Property GetFormVars() As List(Of String)
    Get
      Return _libfrmVars
    End Get
    Set(ByVal value As List(Of String))
      _libfrmVars = value
    End Set
  End Property

End Class


Public Class MyComparer

#Region "IComparer Members"

  Public Function Compare(x As Object, y As Object) As Integer
    ' TODO: Add MyComparer.Compare implementation
    If x Is Nothing Then
      Return -1
    End If
    If y Is Nothing Then
      Return 1
    End If
    Dim a As Integer = Integer.Parse(x.ToString())
    Dim b As Integer = Integer.Parse(y.ToString())
    Return a - b
  End Function

#End Region

End Class


Public Class ComboValue
  Private _Bez As String
  Private _Value As String

  Public Sub New(ByVal _Text2Show As String, ByVal _Value2Save As String)
    _Bez = _Text2Show
    _Value = _Value2Save
  End Sub

  Public Function ComboValue() As String
    Return _Value
  End Function

  Public Function Text() As String
    Return _Bez
  End Function

  Public Overrides Function ToString() As String
    Return _Bez
  End Function

End Class

