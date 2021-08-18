
Imports SPProgUtility
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings

Public Class InitializeClass

  Public TranslationData As Dictionary(Of String, ClsTranslationData)
  Public ProsonalizedData As Dictionary(Of String, ClsProsonalizedData)
  Public MDData As New SPProgUtility.ClsMDData
  Public UserData As New SPProgUtility.ClsUserData


End Class

''' <summary>
''' Constants.
''' </summary>
Public Class ModulConstants

  Public Shared TranslationData As Dictionary(Of String, ClsTranslationData)
  Public Shared ProsonalizedData As Dictionary(Of String, ClsProsonalizedData)
  Public Shared MDData As New SPProgUtility.ClsMDData
  Public Shared UserData As New SPProgUtility.ClsUserData


  Public Shared Function ParseToIntOrNothing(ByVal number As String) As Integer?
    Dim result As Integer
    If (Not Integer.TryParse(number, result)) Then
      Return Nothing
    End If
    Return result
  End Function


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

End Class



''' <summary>
''' Translate given or founded text
''' </summary>
''' <remarks></remarks>
Public Class TranslateValues

  Function GetSafeTranslationValue(ByVal dicKey As String) As String
    Dim strPersonalizedItem As String = dicKey

    Try
      If ModulConstants.TranslationData.ContainsKey(strPersonalizedItem) Then
        Return ModulConstants.TranslationData.Item(strPersonalizedItem).LogedUserLanguage

      Else
        Return strPersonalizedItem

      End If

    Catch ex As Exception
      Return strPersonalizedItem
    End Try

  End Function

  Function GetSafeTranslationValue(ByVal dicKey As String, ByVal bCheckPersonalizedItem As Boolean) As String
    Dim strPersonalizedItem As String = dicKey

    Try
      If bCheckPersonalizedItem Then
        If ModulConstants.ProsonalizedData.ContainsKey(dicKey) Then
          strPersonalizedItem = ModulConstants.ProsonalizedData.Item(dicKey).CaptionValue

        Else
          strPersonalizedItem = strPersonalizedItem

        End If
      End If

      If ModulConstants.TranslationData.ContainsKey(strPersonalizedItem) Then
        Return ModulConstants.TranslationData.Item(strPersonalizedItem).LogedUserLanguage

      Else
        Return strPersonalizedItem

      End If

    Catch ex As Exception
      Return strPersonalizedItem
    End Try

  End Function

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
