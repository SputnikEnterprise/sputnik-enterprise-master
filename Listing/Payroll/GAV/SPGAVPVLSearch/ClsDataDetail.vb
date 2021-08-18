
Imports System.Windows.Forms

Public Class ClsDataDetail

  Public Shared strGAVDivData As String = String.Empty
  Public Shared strButtonValue As String = String.Empty

  Public Shared Get4What As String = String.Empty
  Public Shared frmGAVDiv As Form

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
      Return "B3C1F7E4-A242-4C52-A696-D70637C185E0"
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
  Shared _LLVorjahreslohnsummeJahr As Integer
  Public Shared Property LLVorjahreslohnsummeJahr() As Integer
    Get
      Return _LLVorjahreslohnsummeJahr
    End Get
    Set(ByVal value As Integer)
      _LLVorjahreslohnsummeJahr = value
    End Set
  End Property

  '// GAV-Beruf für GetJahreslohnsumme()
  Shared _LLJahreslohnsummeGAVBeruf As String
  Public Shared Property LLJahreslohnsummeGAVBeruf() As String
    Get
      Return _LLJahreslohnsummeGAVBeruf
    End Get
    Set(ByVal value As String)
      _LLJahreslohnsummeGAVBeruf = value
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

  '// Sorted from frmSearch_LV
  Shared _strSortBez As String
  Public Shared Property GetLVSortBez() As String
    Get
      Return _strSortBez
    End Get
    Set(ByVal value As String)
      _strSortBez = value
    End Set
  End Property

End Class
