
Imports System.Windows.Forms
Imports System.Data.SqlClient

Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.ProgPath.ClsProgPath
Imports SPProgUtility.ProgPath
Imports SPProgUtility
Imports SPProgUtility.Mandanten
Imports System.IO


Public Class ClsDataDetail

  Public Shared strLohnkontiData As String = String.Empty
  Public Shared _strButtonValue As ButtonValue = ButtonValue.Fremdleistungen
  Public Shared _Get4What As What = What.MANR

  Public Shared GetSortBez As String = String.Empty
  Public Shared GetFilterBez As String = String.Empty
  Public Shared GetFilterBez2 As String = String.Empty
  Public Shared GetFilterBez3 As String = String.Empty
  Public Shared GetFilterBez4 As String = String.Empty



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

	Private Shared m_ModuleCache As SP.ModuleCaching.ModuleCache

	Public Shared Function GetModuleCach() As SP.ModuleCaching.ModuleCache

		If m_ModuleCache Is Nothing Then
			m_ModuleCache = New SP.ModuleCaching.ModuleCache()
			m_ModuleCache.MaxCustomerFormsToCache = 2
			m_ModuleCache.MaxResponsiblePersonFormsToCache = 2
			m_ModuleCache.MaxEmployeeFormsToCache = 2
		End If

		Return m_ModuleCache

	End Function



  Structure ParamVar
		Dim Jahr As Integer
		Dim MANR As String
  End Structure
  Public Shared Param As ParamVar


  Enum What As Integer
    MANR
  End Enum

  Enum ButtonValue As Integer
    Fremdleistungen
  End Enum

  Public Shared Function GetAppGuidValue() As String
    Return "9DC1BF21-9F6C-4539-90DB-C3E4912891E0"
  End Function

  '// Query für Datensuche
  Shared _strConnString As String
  Public Shared Property GetDbConnString() As String
    Get
      If GetSelectedDbName Is Nothing Then
        Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
        _strConnString = _ClsProgSetting.GetConnString()
      Else
        _strConnString = GetSelectedMDDbConnString()

      End If

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

  '// Query für selektierten Mandanten
  Shared _strSelectedMDConnString As String
  Public Shared Property GetSelectedMDDbConnString() As String
    Get
      Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
      Dim _aConnString As String() = _ClsProgSetting.GetConnString().Split(CChar(";"))
      For i As Integer = 0 To _aConnString.Length - 1
        If _aConnString(i).ToString.ToLower.Contains("initial catalog") Then
          _aConnString(i) = String.Format("Initial Catalog={0}", GetSelectedDbName)
        End If
      Next

      Dim AtS As New System.Text.StringBuilder
      For i As Integer = 0 To _aConnString.Length - 1
        AtS.Append(_aConnString(i))
        If i <> _aConnString.Length - 1 Then
          AtS.Append(";")
        End If
      Next
      _strSelectedMDConnString = AtS.ToString

      Return (AtS.ToString)
    End Get
    Set(ByVal value As String)
      _strSelectedMDConnString = value
    End Set
  End Property


  '// Datenbankname wenn anderes ausgewählt wird
  Shared _strSelectedDbName As String
  Public Shared Property GetSelectedDbName() As String
    Get
      Return _strSelectedDbName
    End Get
    Set(ByVal value As String)
      _strSelectedDbName = value
    End Set
  End Property

  Public Shared Property Get4What(Optional ByVal What As What = What.MANR) As What
    Get
      Return _Get4What
    End Get
    Set(ByVal value As What)
      _Get4What = What
    End Set
  End Property

  Public Shared Property StrButtonValue(Optional ByVal button As ButtonValue = ButtonValue.Fremdleistungen) As ButtonValue
    Get
      Return _strButtonValue
    End Get
    Set(ByVal value As ButtonValue)
      _strButtonValue = button
    End Set
  End Property

  ''' <summary>
  ''' Gibt die maximale Höhe des Panels, wenn alle Controls sichtbar sind.
  ''' </summary>
  ''' <param name="pnl"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Shared Function GetPanelHeight(ByVal pnl As Panel) As Integer
    Dim height As Integer = 0
    Dim lowestPos As Integer = 0
    For Each con As Control In pnl.Controls
      If lowestPos < con.Location.Y + con.Height Then
        lowestPos = con.Location.Y + con.Height
      End If
    Next
    Return lowestPos + 10
  End Function

  ''' <summary>
  ''' Gibt an, ob ein Panel aufgeklappt ist oder nicht.
  ''' </summary>
  ''' <param name="pnl"></param>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Shared ReadOnly Property PanelExpanded(ByVal pnl As Panel) As Boolean
    Get
      If pnl.Height = GetPanelHeight(pnl) Then
        Return True
      End If
      Return False
    End Get
  End Property


  '// Tabellennamen der zu abspeichernde Liste
  Shared _lltablename As String
  Public Shared Property LLTablename() As String
    Get
      If _lltablename Is Nothing Then
        _lltablename = ""
      End If
      Return _lltablename
    End Get
    Set(ByVal value As String)
      _lltablename = value
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

  '// Sort for LV
  Shared _strSortBez As String
  Public Shared Property GetLVSortBez() As String
    Get
      Return _strSortBez
    End Get
    Set(ByVal value As String)
      _strSortBez = value
    End Set
  End Property

#Region "Parameter für LL-Variabeln"

  Enum SuchkriterienList As Integer
    MANR
    Periode
    MonatVon
    MonatBis
    JahrVon
    JahrBis
  End Enum

  Structure SelectionItem
    Dim Bezeichnung As SuchkriterienList
    Dim Text As String
  End Structure

  '// Selektions-Container für die Anzeige der selektierten Kriterien
  Shared _SelectedContainer As ArrayList
  Public Shared ReadOnly Property SelectedContainer() As ArrayList
    Get
      If _SelectedContainer Is Nothing Then
        _SelectedContainer = New ArrayList
      End If
      Return _SelectedContainer
    End Get
  End Property

#End Region

  '// DataTable 
  Private Shared _dt As DataTable
  Public Shared Property SelectedDataTable() As DataTable
    Get
      If _dt Is Nothing Then
        _dt = New DataTable()
      End If
      Return _dt
    End Get
    Set(ByVal value As DataTable)
      _dt = value
    End Set
  End Property

  '// DataTable for LV
  Private Shared _dtLV As DataTable
  Public Shared Property SelectedDataTableLV() As DataTable
    Get
      If _dtLV Is Nothing Then
        _dtLV = New DataTable()
      End If
      Return _dtLV
    End Get
    Set(ByVal value As DataTable)
      _dtLV = value
    End Set
  End Property

  Public Shared AnzMax As Integer ' Für die Fortschrittsanzeige im LL

  ''' <summary>
  ''' Für die ProgressBar-Aufzählung ganzzahliger Prozent
  ''' </summary>
  ''' <param name="min"></param>
  ''' <param name="max"></param>
  ''' <param name="current"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Shared Function GetProzent(ByVal min As Integer, ByVal max As Integer, ByVal current As Integer) As Integer
    If current > max Then
      Return 100
    End If
    If max < 1 Then
      Return 0
    End If
    Return CInt((current / max) * 100)
  End Function
End Class
