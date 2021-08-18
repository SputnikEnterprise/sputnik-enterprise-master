
Imports System.Windows.Forms

Imports SPProgUtility
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities

Imports SPProgUtility.ProgPath
Imports System.IO

Public Class ClsDataDetail

  Public Shared strESData As String = String.Empty
  Public Shared strButtonValue As String = String.Empty

  Public Shared Get4What As String = String.Empty
  Public Shared frmES As Form

  Public Shared GetSortBez As String = String.Empty
  Public Shared GetFilterBez As String = String.Empty
  Public Shared GetFilterBez2 As String = String.Empty
  Public Shared GetFilterBez3 As String = String.Empty
  Public Shared GetFilterBez4 As String = String.Empty


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


  ''' <summary>
  ''' alle übersetzte Elemente in 3 Sprachen
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Shared ReadOnly Property Translation() As Dictionary(Of String, ClsTranslationData)
    Get
      Try
        Dim m_Translation As New SPProgUtility.SPTranslation.ClsTranslation
        Return m_Translation.GetTranslationInObject

      Catch ex As Exception
        Return Nothing
      End Try
    End Get
  End Property

  ''' <summary>
  ''' Individuelle Bezeichnungen für Labels
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Shared ReadOnly Property ProsonalizedName() As Dictionary(Of String, ClsProsonalizedData)
    Get
      Try
        Dim m As New Mandant
        Return m.GetPersonalizedCaptionInObject(ProgSettingData.SelectedMDNr)

      Catch ex As Exception
        Return Nothing
      End Try
    End Get
  End Property

  ''' <summary>
  ''' Datenbankeinstellungen für [Sputnik DBSelect]
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Shared ReadOnly Property SelectedDbSelectData() As ClsDbSelectData
    Get
      Dim m_Progpath As New ClsProgPath

      Return m_Progpath.GetDbSelectData()
    End Get
  End Property

  ''' <summary>
  ''' Datenbankeinstellungen aus der \\Server\spenterprise$\bin\Programm.XML. gemäss Mandantennummer
  ''' </summary>
  ''' <param name="iMDNr"></param>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Shared ReadOnly Property SelectedMDData(ByVal iMDNr As Integer) As ClsMDData
    Get
      Dim m_md As New Mandant
      MDData = m_md.GetSelectedMDData(iMDNr) 'ProgSettingData.SelectedMDNr)
      ProgSettingData.SelectedMDNr = MDData.MDNr
      GetSelectedMDConnstring = MDData.MDDbConn

      Return MDData
    End Get
  End Property

  ''' <summary>
  ''' Benutzerdaten aus der Datenbank
  ''' </summary>
  ''' <param name="iMDNr"></param>
  ''' <param name="iUserNr"></param>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Shared ReadOnly Property LogededUSData(ByVal iMDNr As Integer, ByVal iUserNr As Integer) As ClsUserData
    Get
      Dim m_md As New Mandant
      UserData = m_md.GetSelectedUserData(iMDNr, iUserNr) 'ProgSettingData.SelectedMDNr, ProgSettingData.LogedUSNr)
      ProgSettingData.LogedUSNr = UserData.UserNr

      Return UserData
    End Get
  End Property

  ''' <summary>
  ''' Benutzerdaten aus der Datenbank
  ''' </summary>
  ''' <param name="iMDNr"></param>
  ''' <param name="strLastname"></param>
  ''' <param name="strFirstname"></param>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Shared ReadOnly Property LogededUSData(ByVal iMDNr As Integer, ByVal strLastname As String, ByVal strFirstname As String) As ClsUserData
    Get
      Dim m_md As New Mandant
      UserData = m_md.GetSelectedUserDataWithUserName(iMDNr, strLastname, strFirstname)
      ProgSettingData.LogedUSNr = UserData.UserNr

      Return UserData
    End Get
  End Property



  Public Shared ReadOnly Property GetAppGuidValue() As String
    Get
      Return "E7AC7BF2-F177-461b-AE4C-723856928E72"
    End Get
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

  '// Sorted from frmESsearch_LV
  Shared _strSortBez As String
  Public Shared Property GetLVSortBez() As String
    Get
      Return _strSortBez
    End Get
    Set(ByVal value As String)
      _strSortBez = value
    End Set
  End Property

  '// Tabellennamen der Einsatzliste auf der DB
  Shared _SPTabNamenES As String
  Public Shared Property SPTabNamenES() As String
    Get
      Return _SPTabNamenES
    End Get
    Set(ByVal value As String)
      _SPTabNamenES = value
    End Set
  End Property

  '// Tabellennamen der EinsatzKSTStatistikListe auf der DB
  Shared _SPTabNamenESKSTStatistik As String
  Public Shared Property SPTabNamenESKSTStatistik() As String
    Get
      Return _SPTabNamenESKSTStatistik
    End Get
    Set(ByVal value As String)
      _SPTabNamenESKSTStatistik = value
    End Set
  End Property

  '// Statistik nur Anzeigen, wenn die Einsatzliste gedruckt wird.
  Shared _showBewilligStatistik As Boolean
  Public Shared Property ShowBewilligStatistik() As Boolean
    Get
      Return _showBewilligStatistik
    End Get
    Set(ByVal value As Boolean)
      _showBewilligStatistik = value
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
