
Imports System.Windows.Forms

Public Class ClsDataDetail

  Public Shared strKDData As String = String.Empty
  Public Shared strButtonValue As String = String.Empty

  Public Shared Get4What As String = String.Empty

  Public Shared GetSortBez As String = String.Empty
  Public Shared GetFilterBez As String = String.Empty
  Public Shared GetFilterBez2 As String = String.Empty
  Public Shared GetFilterBez3 As String = String.Empty
  Public Shared GetFilterBez4 As String = String.Empty

  Public Shared IsLVLarg As Boolean


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





  ' Alle wählbaren Art der Adresse
  Enum AddressArt As Short
    ADD_ES    ' 0
    ADD_RP    ' 1
    ADD_LO    ' 2
    ADD_ARG   ' 3
    ADD_ZV    ' 4
    ADD_LOAusweis    ' 5
		ADD_STemplate		 ' 6
	End Enum

  '// Die aktuell gewählte Art der Adresse
  Shared _AdressArt As AddressArt
  Public Shared Property SelectedAddressArt() As AddressArt
    Get
      Return _AdressArt
    End Get
    Set(ByVal value As AddressArt)
      _AdressArt = value
    End Set
  End Property

  ' Art der Suche (Suche nach Land, PLZ)
  Enum SearchModul As Short
    Search_PLZ      ' 0
    Search_Country  ' 1
  End Enum

  Public Shared Function GetAppGuidValue() As String
    Return "88104AA4-D604-42dd-9D9E-719C7A2327FF"
  End Function

  '// Die aktuell gewählte Art der Adresse
  Shared _SearchModul As SearchModul
  Public Shared Property SelectedSearchModul() As SearchModul
    Get
      Return _SearchModul
    End Get
    Set(ByVal value As SearchModul)
      _SearchModul = value
    End Set
  End Property

  '// Query für Datensuche
  Shared _strConnString As String
  Public Shared Property GetDbConnString() As String
    Get
      Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
      Dim _strConnString As String = _ClsProgSetting.GetConnString()

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

  '// Selectierte Kandidaten
  Shared _MANr As Integer
  Public Shared Property GetSelectedMANr() As Integer
    Get
      Return _MANr
    End Get
    Set(ByVal value As Integer)
      _MANr = value
    End Set
  End Property

End Class


Public Class ClsComboBoxItem
  Public Text As String
  Public Value As String

  Public Sub New(ByVal text As String, ByVal val As String)
    Me.Text = text
    Me.Value = val
  End Sub
  Public Overrides Function ToString() As String
    Return String.Format("{1}{0}{2}", vbTab, Text, Value)
  End Function
End Class
