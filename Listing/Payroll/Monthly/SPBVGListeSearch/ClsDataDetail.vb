
Imports System.Windows.Forms

Public Class ClsDataDetail

  Public Shared strBVGListeData As String = String.Empty
  Public Shared strButtonValue As String = String.Empty

  Public Shared Get4What As String = String.Empty
  Public Shared frmBVGListe As Form

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
			Return "A5335B0C-60E7-4165-8CE2-53A5AD2FBDC4"
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

  '// Sorted from frmBVGListeSearch_LV
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

  '// Parameter für ListView
  Shared _sqlLVParameter As ClsDivFunc.LVParameter
  Public Shared Property LVParameter() As ClsDivFunc.LVParameter
    Get
      Return _sqlLVParameter
    End Get
    Set(ByVal value As ClsDivFunc.LVParameter)
      _sqlLVParameter = value
    End Set
  End Property

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

  '// DataTable QSTListe
  Shared _dtQSTListe As DataTable
  Public Shared Property QSTListeDataTable() As DataTable
    Get
      Return _dtQSTListe
    End Get
    Set(ByVal value As DataTable)
      _dtQSTListe = value
    End Set
  End Property

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
