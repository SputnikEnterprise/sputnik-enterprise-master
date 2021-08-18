
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.ProgPath.ClsProgPath
Imports SPProgUtility.ProgPath
Imports SPProgUtility
Imports SPProgUtility.Mandanten
Imports System.IO


Public Class ClsDataDetail

  Public Shared strValueData As String
  Public Shared strButtonValue As String

  Public Shared Get4What As String

  Public Shared GetSortBez As String = String.Empty
  Public Shared GetFilterBez As String = String.Empty
  Public Shared GetFilterBez2 As String = String.Empty
  Public Shared GetFilterBez3 As String = String.Empty
  Public Shared GetFilterBez4 As String = String.Empty


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
      Return "b0ff184d-9750-4aec-b79a-930163b36ab2"
    End Get
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

  '// ZG. Betrag
  Shared _bIsPositiv As Boolean
  Public Shared Property ShowBetragAsPositiv() As Boolean
    Get
      Return _bIsPositiv
    End Get
    Set(ByVal value As Boolean)
      _bIsPositiv = value
    End Set
  End Property

  '// Totalbetrag
  Shared _dTotalBetrag As Double
  Public Shared Property GetTotalBetrag() As Double
    Get
      Return _dTotalBetrag
    End Get
    Set(ByVal value As Double)
      If value < 0 Then
        _dTotalBetrag = value * CInt(IIf(CBool(ShowBetragAsPositiv), -1, 1))
      Else
        _dTotalBetrag = value
      End If

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


Class ComboBoxItem

  Public Property DisplayValue As String
  Public Property ValueMember As String

  Public Overrides Function ToString() As String
    Return DisplayValue
  End Function


End Class
