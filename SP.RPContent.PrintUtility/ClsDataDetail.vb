
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
  Public Shared ProgSettingData As ClsRPCSetting
  Public Shared Property GetSelectedMDConnstring As String

  Public Shared MDData As New ClsMDData
  Public Shared UserData As New ClsUserData


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



  Public Shared Function GetAppGuidValue() As String
    Return "d5b76e8a-b769-4030-aaf1-e4e84f19c3e4"
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

