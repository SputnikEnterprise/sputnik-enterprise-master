
Imports System.Windows.Forms

Public Class ClsDataDetail

  Public Shared strUVGListeData As String = String.Empty
  Public Shared strButtonValue As String = String.Empty

  Public Shared Get4What As String = String.Empty
  Public Shared frmUVGListe As Form

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



  Public Shared ReadOnly Property GetAppGuidValue() As String
    Get
      Return "5BD40577-5956-4887-8E1E-3492933338A0"
    End Get
  End Property


  '// Tabellennamen
  Shared _LLTabellennamen As String
  Public Shared Property LLTabellennamen() As String
    Get
      Return _LLTabellennamen
    End Get
    Set(ByVal value As String)
      _LLTabellennamen = value
    End Set
  End Property


  '// Vorjahreslohnsumme für GetJahreslohnsumme()
  Shared _LLSelektionText As String
  Public Shared Property LLSelektionText() As String
    Get
      Return _LLSelektionText
    End Get
    Set(ByVal value As String)
      _LLSelektionText = value
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

  '// Sorted from frmUVGListeSearch_LV
  Shared _strSortBez As String
  Public Shared Property GetLVSortBez() As String
    Get
      Return _strSortBez
    End Get
    Set(ByVal value As String)
      _strSortBez = value
    End Set
  End Property

  Shared _arbeitgeberPauschale As Boolean
  Public Shared Property CheckArbeitgeberPauschale() As Boolean
    Get
      Return _arbeitgeberPauschale
    End Get
    Set(ByVal value As Boolean)
      _arbeitgeberPauschale = value
    End Set
  End Property

	''// Parameter für ListView
	'Shared _sqlLVParameter As ClsDivFunc.LVParameter
	'Public Shared Property LVParameter() As ClsDivFunc.LVParameter
	'  Get
	'    Return _sqlLVParameter
	'  End Get
	'  Set(ByVal value As ClsDivFunc.LVParameter)
	'    _sqlLVParameter = value
	'  End Set
	'End Property

  '// Parameter für ListView
  Shared _sqlCommand As SqlClient.SqlCommand
  Public Shared Property SQLCommand() As SqlClient.SqlCommand
    Get
      Return _sqlCommand
    End Get
    Set(ByVal value As SqlClient.SqlCommand)
      _sqlCommand = value
    End Set
  End Property

  '// DataTable UVGListe
  Shared _dtUVGListe As DataTable
  Public Shared Property UVGListeDataTable() As DataTable
    Get
      Return _dtUVGListe
    End Get
    Set(ByVal value As DataTable)
      _dtUVGListe = value
    End Set
  End Property

  '// LL TotalMA12
  Shared _lltotalMA12 As Decimal
  Public Shared Property LLTotalMA12() As Decimal
    Get
      Return _lltotalMA12
    End Get
    Set(ByVal value As Decimal)
      _lltotalMA12 = value
    End Set
  End Property

  '// LL TotalMA3
  Shared _lltotalMA3 As Decimal
  Public Shared Property LLTotalMA3() As Decimal
    Get
      Return _lltotalMA3
    End Get
    Set(ByVal value As Decimal)
      _lltotalMA3 = value
    End Set
  End Property

  '// LL TotalFA12
  Shared _lltotalFA12 As Decimal
  Public Shared Property LLTotalFA12() As Decimal
    Get
      Return _lltotalFA12
    End Get
    Set(ByVal value As Decimal)
      _lltotalFA12 = value
    End Set
  End Property

  '// LL TotalFA3
  Shared _lltotalFA3 As Decimal
  Public Shared Property LLTotalFA3() As Decimal
    Get
      Return _lltotalFA3
    End Get
    Set(ByVal value As Decimal)
      _lltotalFA3 = value
    End Set
  End Property

  '// LL TotalMZ12
  Shared _lltotalMZ12 As Decimal
  Public Shared Property LLTotalMZ12() As Decimal
    Get
      Return _lltotalMZ12
    End Get
    Set(ByVal value As Decimal)
      _lltotalMZ12 = value
    End Set
  End Property

  '// LL TotalMZ3
  Shared _lltotalMZ3 As Decimal
  Public Shared Property LLTotalMZ3() As Decimal
    Get
      Return _lltotalMZ3
    End Get
    Set(ByVal value As Decimal)
      _lltotalMZ3 = value
    End Set
  End Property

  '// LL TotalFZ12
  Shared _lltotalFZ12 As Decimal
  Public Shared Property LLTotalFZ12() As Decimal
    Get
      Return _lltotalFZ12
    End Get
    Set(ByVal value As Decimal)
      _lltotalFZ12 = value
    End Set
  End Property

  '// LL TotalFZ3
  Shared _lltotalFZ3 As Decimal
  Public Shared Property LLTotalFZ3() As Decimal
    Get
      Return _lltotalFZ3
    End Get
    Set(ByVal value As Decimal)
      _lltotalFZ3 = value
    End Set
  End Property

  '// Suva-HL-Jahr
  Shared _llSuvaHLJahr As Integer
  Public Shared Property LLSuvaHLJahr() As Integer
    Get
      Return _llSuvaHLJahr
    End Get
    Set(ByVal value As Integer)
      _llSuvaHLJahr = value
    End Set
  End Property

  '// Für die Anzahl angestellten Mitarbeiter Ende September: Parameter für gespeicherte Prozedur
  Shared _llESAnzJahr As Integer
  Public Shared Property LLESAnzJahr() As Integer
    Get
      Return _llESAnzJahr
    End Get
    Set(ByVal value As Integer)
      _llESAnzJahr = value
    End Set
  End Property

  '// Parameter LLJahr anzeigen oder nicht.
  Shared _llShowESAnzJahr As Boolean
  Public Shared Property LLShowESAnzJahr() As Boolean
    Get
      Return _llShowESAnzJahr
    End Get
    Set(ByVal value As Boolean)
      _llShowESAnzJahr = value
    End Set
  End Property

  '// Liste der Summen von SecSuvaCode
  Shared _llSecSuvaItems As ArrayList
  Public Shared ReadOnly Property LLSecSuvaItems() As ArrayList
    Get
      If _llSecSuvaItems Is Nothing Then
        _llSecSuvaItems = New ArrayList()
      End If
      Return _llSecSuvaItems
    End Get
  End Property

  '// Liste der Item von SecSuvaCode
  Structure SecSuvaItem
    Dim Bezeichnung As String
    Dim M As Decimal
    Dim F As Decimal
  End Structure

	Shared _llJahrVon As Integer
	Public Shared Property LLJahrVon() As Integer
		Get
			Return _llJahrVon
		End Get
		Set(ByVal value As Integer)
			_llJahrVon = value
		End Set
	End Property

  '// Selektierte Zeitperiode: JahrBis
  Shared _llJahrBis As String
  Public Shared Property LLJahrBis() As String
    Get
      Return _llJahrBis
    End Get
    Set(ByVal value As String)
      _llJahrBis = value
    End Set
  End Property

  '// Selektierte Zeitperiode: MonatVon
  Shared _llMonatVon As String
  Public Shared Property LLMonatVon() As String
    Get
      Return _llMonatVon
    End Get
    Set(ByVal value As String)
      _llMonatVon = value
    End Set
  End Property

  '// Selektierte Zeitperiode: MonatBis
  Shared _llMonatBis As String
  Public Shared Property LLMonatBis() As String
    Get
      Return _llMonatBis
    End Get
    Set(ByVal value As String)
      _llMonatBis = value
    End Set
  End Property
End Class
