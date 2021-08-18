
Imports System.Windows.Forms
Imports System.Data.SqlClient

Public Class ClsDataDetail

  Public Shared strFremdListData As String = String.Empty
  Public Shared _strButtonValue As ButtonValue = ButtonValue.Fremdleistungen
  Public Shared _Get4What As What = What.MANR

	'Public Shared frmFremdList As Form

  Public Shared GetSortBez As String = String.Empty
  Public Shared GetFilterBez As String = String.Empty
  Public Shared GetFilterBez2 As String = String.Empty
  Public Shared GetFilterBez3 As String = String.Empty
  Public Shared GetFilterBez4 As String = String.Empty


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

			m_InitialData = New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

			Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

		End Get
	End Property

	Enum What As Integer
    MANR
  End Enum

  Enum ButtonValue As Integer
    Fremdleistungen
  End Enum

  Public Shared Function GetAppGuidValue() As String
    Return "5E86FE3A-195B-4d9d-8786-949177C8ED40"
  End Function

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
    Filiale
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
  Shared _dtFremdleistungen As DataTable
  Public Shared Property SelectedDataTable() As DataTable
    Get
      If _dtFremdleistungen Is Nothing Then
        _dtFremdleistungen = New DataTable()
      End If
      Return _dtFremdleistungen
    End Get
    Set(ByVal value As DataTable)
      _dtFremdleistungen = value
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
