
Imports System.Data.SqlClient
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.ProgPath.ClsProgPath
Imports SPProgUtility.ProgPath
Imports SPProgUtility
Imports SPProgUtility.Mandanten



Public Class ClsDataDetail

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

  Public Shared TranslationData As Dictionary(Of String, ClsTranslationData)
  Public Shared ProsonalizedData As Dictionary(Of String, ClsProsonalizedData)
  Public Shared ProgSettingData As ClsSetting
  Public Shared Property GetSelectedMDConnstring As String

  Public Shared MDData As New ClsMDData
  Public Shared UserData As New ClsUserData




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
		Return "34e02b48-59a7-4c5d-8b13-6bba72c98f33"
  End Function




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

  '// Sorted from Search_LV
  Shared _strSortBez As String
  Public Shared Property GetLVSortBez() As String
    Get
      Return _strSortBez
    End Get
    Set(ByVal value As String)
      _strSortBez = value
    End Set
  End Property

  '// Sorted from Search_LV
  Shared _bListAsGrouped As Boolean
  Public Shared Property ListAsGrouped() As Boolean
    Get
      Return _bListAsGrouped
    End Get
    Set(ByVal value As Boolean)
      _bListAsGrouped = value
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
