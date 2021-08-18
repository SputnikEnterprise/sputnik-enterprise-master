
Imports SPProgUtility.Mandanten
Imports SPProgUtility


Public Class ClsMDData

  Public MDNr As Integer
  Public MDGroupNr As Integer
  Public MDYear As Integer

  Public MDName As String
  Public MDGuid As String

  Public MDMainPath As String
  Public MDDbConn As String
  Public MDDbName As String
  Public MDDbServer As String

  Public MDName_2 As String
  Public MDName_3 As String
  Public MDStreet As String
  Public MDPostcode As String
  Public MDCountry As String
  Public MDCity As String

  Public MDTelefon As String
  Public MDTelefax As String
  Public MDeMail As String
  Public MDHomepage As String

End Class


Public Class ClsSetting

  Public Property SelectedMDNr As Integer
  Public Property SelectedMDYear As Integer
  Public Property SelectedMDGuid As String
  Public Property LogedUSNr As Integer

  Public PerosonalizedData As Dictionary(Of String, ClsProsonalizedData)
  Public TranslationItems As Dictionary(Of String, ClsTranslationData)

End Class


Public Class ClsMainSetting

	' ''' <summary>
	' ''' The translation value helper.
	' ''' </summary>
	'Public Shared m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper


  Public Shared ProgSettingData As ClsSetting

  Public Shared TranslationData As Dictionary(Of String, SPProgUtility.ClsTranslationData)
  Public Shared PerosonalizedData As Dictionary(Of String, SPProgUtility.ClsProsonalizedData)

	Public Shared Property MDData() As SPProgUtility.ClsMDData
	Public Shared Property UserData() As SPProgUtility.ClsUserData



	'Public Shared MDData As New ClsMDData
	'Public Shared UserData As New ClsUserData



	''' <summary>
	''' The translation value helper.
	''' </summary>
	Public Shared m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper
	Public Shared m_InitialData As SP.Infrastructure.Initialization.InitializeClass


	Public Shared ReadOnly Property GetLL24LicenceInfo() As String
		Get
			Return "yourlicencekey"

		End Get
	End Property

	Public Shared ReadOnly Property GetLL25LicenceInfo() As String
		Get
			Return "yourlicencekey"

		End Get
	End Property

	Public Shared ReadOnly Property GetAppGuidValue() As String
		Get
			Return "09d3110a-ad4f-4a93-99b4-ba4028206b34"
		End Get
	End Property



End Class


Public Class AssamlyInfo

	Public Property Filename As String
	Public Property Filelocation As String
	Public Property FileVersion As String

End Class