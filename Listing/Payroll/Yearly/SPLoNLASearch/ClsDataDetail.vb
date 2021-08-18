
Imports System.Windows.Forms
Imports System.Data.SqlClient

Public Class ClsDataDetail

  Public Shared strLoNLAData As String = String.Empty
  Public Shared _strButtonValue As ButtonValue = ButtonValue.KandidatenNummer
  Public Shared _Get4What As What = What.MANR
  Public Shared txt_SQLQuery As String = ""

  Public Shared GetSortBez As String = String.Empty
  Public Shared GetFilterBez As String = String.Empty
  Public Shared GetFilterBez2 As String = String.Empty
  Public Shared GetFilterBez3 As String = String.Empty
  Public Shared GetFilterBez4 As String = String.Empty

	Public Shared NLA_2_3 As String = ""
  Public Shared NLA_3_0 As String = ""
  Public Shared NLA_4_0 As String = ""
  Public Shared NLA_7_0 As String = ""
  Public Shared NLA_13_1_2 As String = ""
  Public Shared NLA_13_2_3 As String = ""

  Public Shared NLA_Nebenleistung_1 As String = ""
  Public Shared NLA_Nebenleistung_2 As String = ""
  Public Shared NLA_Bemerkung_1 As String = ""
  Public Shared NLA_Bemerkung_2 As String = ""
  Public Shared bSentIntoToWOS As Boolean

  Public Shared IsLVLarg As Boolean


	Public Structure ParamVar
		Dim Jahr As String
		Dim MANR As String
		Dim NameVon As String
		Dim NameBis As String
		Dim Kanton As String
		Dim Land As String
	End Structure
  Public Shared Param As ParamVar


  Enum What As Integer
    MANR
    Nachname
  End Enum

  Enum ButtonValue As Integer
    KandidatenNummer
    KandidatenNamen
  End Enum


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
      Return "CD6B7230-77AE-449e-80B4-B9293CD19955"
    End Get
  End Property


	Public Shared Property Get4What() As What
    Get
      Return _Get4What
    End Get
    Set(ByVal value As What)
      _Get4What = value
    End Set
  End Property

  Public Shared Property StrButtonValue(Optional ByVal button As ButtonValue = ButtonValue.KandidatenNummer) As ButtonValue
    Get
      Return _strButtonValue
    End Get
    Set(ByVal value As ButtonValue)
      _strButtonValue = button
    End Set
  End Property


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

  '// Verzeichnis für LL-Dateien
  Shared _strPath4LLFiles As String
  Public Shared Property GetPath4LLFiles() As String
    Get
      Return _strPath4LLFiles
    End Get
    Set(ByVal value As String)
      _strPath4LLFiles = value
    End Set
  End Property

  '// Datei für Unterschrift
  Shared _strFile4USSign As String
  Public Shared Property GetFilename4USSign() As String
    Get
      Return _strFile4USSign
    End Get
    Set(ByVal value As String)
      _strFile4USSign = value
    End Set
  End Property

  '// War der letzte Druck als Export (wird benutzt für mehrere Lohnausweise auf PDF zu erstellen.)
  Shared _bWasLastPrintAsExport As Boolean
  Public Shared Property IsLastPrintjobAsExport() As Boolean
    Get
      Return _bWasLastPrintAsExport
    End Get
    Set(ByVal value As Boolean)
      _bWasLastPrintAsExport = value
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
    Jahr
    MANR
    NameVon
    NameBis
		Kanton
		Land
		Geschlecht
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

  '// DataTable für selektierte Datensätze aus der DB
  Shared _SelectedDataTable As DataTable
  Public Shared Property SelectedDataTable() As DataTable
    Get
      If _SelectedDataTable Is Nothing Then
        _SelectedDataTable = New DataTable()
      End If
      Return _SelectedDataTable
    End Get
    Set(ByVal value As DataTable)
      _SelectedDataTable = value
    End Set
  End Property

  '// DataTable für die fehlgeschlagene Datensätze aus der selektierte Datensätze
  Shared _SelectedDataTableFaild As DataTable
  Public Shared Property SelectedDataTableFailed() As DataTable
    Get
      If _SelectedDataTableFaild Is Nothing Then
        _SelectedDataTableFaild = New DataTable()
      End If
      Return _SelectedDataTableFaild
    End Get
    Set(ByVal value As DataTable)
      _SelectedDataTableFaild = value
    End Set
  End Property

  '// ListView für die Anzeige der korrekten Datensätze
  Shared _SelectedListView As ListView
  Public Shared Property SelectedListView() As ListView
    Get
      Return _SelectedListView
    End Get
    Set(ByVal value As ListView)
      _SelectedListView = value
    End Set
  End Property

  '// ListView für die Anzeige der fehlgeschlagenen Datensätze
  Shared _SelectedListViewFailed As ListView
  Public Shared Property SelectedListViewFailed() As ListView
    Get
      Return _SelectedListViewFailed
    End Get
    Set(ByVal value As ListView)
      _SelectedListViewFailed = value
    End Set
  End Property

  ''' <summary>
  ''' Druckmöglichkeiten
  ''' </summary>
  ''' <remarks></remarks>
  Enum PrintSelectionEnum As Integer
    Alles
    Korrekte
    Fehlgeschlagene
  End Enum

  Public Shared PrintSelection As PrintSelectionEnum
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

  ''' <summary>
  ''' MANR --> Mögliche Eingaben: 100- oder 9,10,14 oder 9-12 oder -40 oder 9,10,30-35,50
  ''' Wird soweit aufgeschlüsselt, dass jede MANR angegeben wird, und nur einmal "ab" oder "bis"
  ''' einer MANR gesucht werden kann. Das 'Minus'-Zeichen wird mit 'bis' ersetzt.
  ''' </summary>
  ''' <remarks></remarks>
  Public Shared Sub ConvertMANREntry()
    Dim rplEntry As String = ""
    For Each manr As String In ClsDataDetail.Param.MANR.Split(CChar(","))
      If manr.Contains("-") And Not manr.StartsWith("-") And Not manr.EndsWith("-") Then
        For i As Integer = CInt(manr.Split(CChar("-"))(0)) To CInt(manr.Split(CChar("-"))(1))
          rplEntry += String.Format("{0},", i)
          If rplEntry.Length > 4000 Then
            Exit For
          End If
        Next
      Else
        rplEntry += String.Format("{0},", manr)
      End If
    Next
    rplEntry = rplEntry.Substring(0, rplEntry.Length - 1)
    ClsDataDetail.Param.MANR = rplEntry.Replace("-", "bis") ' Konvertierung für die gespeicherte Prozedur
  End Sub

  'Shared Function GetAppGuidValue() As String
  '  Return "CD6B7230-77AE-449e-80B4-B9293CD19955"
  'End Function

End Class
