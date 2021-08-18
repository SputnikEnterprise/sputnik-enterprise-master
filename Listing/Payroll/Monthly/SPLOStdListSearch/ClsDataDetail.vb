
Imports System.Windows.Forms
Imports System.Data.SqlClient

Public Class ClsDataDetail

  Public Shared strLOStdListData As String = String.Empty
  Private Shared _strButtonValue As ButtonValue = ButtonValue.Arbeitsstunden

  Private Shared _Get4What As What = What.MANR
  Public Shared frmLOStdList As Form

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



  Enum What As Integer
    MANR
  End Enum

  Enum ButtonValue As Integer
    Arbeitsstunden
  End Enum

  Public Shared Property Get4What(Optional ByVal What As What = What.MANR) As What
    Get
      Return _Get4What
    End Get
    Set(ByVal value As What)
      _Get4What = What
    End Set
  End Property

  Public Shared Function GetAppGuidValue() As String
    Return "67A21D41-D743-4afb-8C84-64CE514445D9"
  End Function


  Public Shared Property StrButtonValue(Optional ByVal button As ButtonValue = ButtonValue.Arbeitsstunden) As ButtonValue
    Get
      Return _strButtonValue
    End Get
    Set(ByVal value As ButtonValue)
      _strButtonValue = button
    End Set
  End Property

	'// Tabellennamen der zu abspeichernde Liste
  Shared _lltablename As String
  Public Shared Property LLTablename() As String
    Get
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
    Kanton
    Beruf
    Kategorie
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



  '// DataTable Liste der Arbeitsstunden
  Shared _dtSelectedDataTable As DataTable
  Public Shared Property SelectedDataTable() As DataTable
    Get
      If _dtSelectedDataTable Is Nothing Then
        _dtSelectedDataTable = New DataTable()
      End If
      Return _dtSelectedDataTable
    End Get
    Set(ByVal value As DataTable)
      _dtSelectedDataTable = value
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
