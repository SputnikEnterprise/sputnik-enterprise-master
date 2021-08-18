
Imports System.Windows.Forms
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath
Imports SPProgUtility


Public Class ClsDataDetail
  Inherits ClsXML

  Public Shared strKDData As String = String.Empty
  Public Shared strButtonValue As String = String.Empty

  Public Shared Get4What As String = String.Empty

  Public Shared GetSortBez As String = String.Empty
  Public Shared GetFilterBez As String = String.Empty
  Public Shared GetFilterBez2 As String = String.Empty
  Public Shared GetFilterBez3 As String = String.Empty
  Public Shared GetFilterBez4 As String = String.Empty


  Public Shared TranslationData As Dictionary(Of String, SPProgUtility.ClsTranslationData)
  Public Shared ProsonalizedData As Dictionary(Of String, SPProgUtility.ClsProsonalizedData)

  Public Shared MDData As New ClsMDData
  Public Shared UserData As New ClsUserData



  ''' <summary>
  ''' alle übersetzte Elemente in 3 Sprachen
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Shared ReadOnly Property TranslationValues() As Dictionary(Of String, ClsTranslationData)
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
  Public Shared ReadOnly Property ProsonalizedValues() As Dictionary(Of String, ClsProsonalizedData)
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
  Public Shared ReadOnly Property SelectedMDData(ByVal iMDNr As Integer) As ClsMDData
    Get
      Dim m_md As New Mandant
      MDData = m_md.GetSelectedMDData(iMDNr)

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
      UserData = m_md.GetSelectedUserData(iMDNr, iUserNr)

      Return UserData
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

    ADD_STemplate    ' 6
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
    Return "0017df27-c9c4-411e-a4a9-1e57a4c915a3"
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

