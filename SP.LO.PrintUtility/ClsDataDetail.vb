
Imports System.Windows.Forms

Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.ProgPath.ClsProgPath
Imports SPProgUtility.ProgPath
Imports SPProgUtility
Imports SPProgUtility.Mandanten


Public Class ClsDataDetail

  Public Shared strKDData As String = String.Empty
  Public Shared strButtonValue As String = String.Empty

  Public Shared Get4What As String = String.Empty

  Public Shared GetSortBez As String = String.Empty
  Public Shared GetFilterBez As String = String.Empty
  Public Shared GetFilterBez2 As String = String.Empty
  Public Shared GetFilterBez3 As String = String.Empty
  Public Shared GetFilterBez4 As String = String.Empty

  Public Shared TranslationData As Dictionary(Of String, ClsTranslationData)
  Public Shared ProsonalizedData As Dictionary(Of String, ClsProsonalizedData)
  Public Shared ProgSettingData As ClsLOSetting
  Public Shared Property GetSelectedMDConnstring As String

  Public Shared MDData As New SPProgUtility.ClsMDData
  Public Shared UserData As New SPProgUtility.ClsUserData







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
        Return m.GetPersonalizedCaptionInObject(MDData.MDNr)

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
  Public Shared ReadOnly Property SelectedMDData(ByVal iMDNr As Integer) As SPProgUtility.ClsMDData
    Get
      Dim m_md As New Mandant
      MDData = m_md.GetSelectedMDData(iMDNr)
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
  Public Shared ReadOnly Property LogededUSData(ByVal iMDNr As Integer, ByVal iUserNr As Integer) As SPProgUtility.ClsUserData
    Get
      Dim m_md As New Mandant
      UserData = m_md.GetSelectedUserData(iMDNr, iUserNr) 'ProgSettingData.SelectedMDNr, ProgSettingData.LogedUSNr)
      ProgSettingData.LogedUSNr = UserData.UserNr

      Return UserData
    End Get
  End Property



  '' Alle wählbaren Art der Adresse
  'Enum AddressArt As Short
  '  ADD_ES    ' 0
  '  ADD_RP    ' 1
  '  ADD_LO    ' 2
  '  ADD_ARG   ' 3
  '  ADD_ZV    ' 4
  '  ADD_LOAusweis    ' 5

  '  ADD_STemplate    ' 6
  'End Enum

  ''// Die aktuell gewählte Art der Adresse
  'Shared _AdressArt As AddressArt
  'Public Shared Property SelectedAddressArt() As AddressArt
  '  Get
  '    Return _AdressArt
  '  End Get
  '  Set(ByVal value As AddressArt)
  '    _AdressArt = value
  '  End Set
  'End Property

  '' Art der Suche (Suche nach Land, PLZ)
  'Enum SearchModul As Short
  '  Search_PLZ      ' 0
  '  Search_Country  ' 1
  'End Enum

  ''// Die aktuell gewählte Art der Adresse
  'Shared _SearchModul As SearchModul
  'Public Shared Property SelectedSearchModul() As SearchModul
  '  Get
  '    Return _SearchModul
  '  End Get
  '  Set(ByVal value As SearchModul)
  '    _SearchModul = value
  '  End Set
  'End Property




  Public Shared Function GetAppGuidValue() As String
    Return "C89974E6-68FB-483D-8C62-3E65385B5FAF"
  End Function



  
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

  Shared _AnzKopien As Short
  Public Shared Property GetAnzahlLOKopien() As Short
    Get
      Return _AnzKopien
    End Get
    Set(ByVal value As Short)
      _AnzKopien = value
    End Set
  End Property

End Class

