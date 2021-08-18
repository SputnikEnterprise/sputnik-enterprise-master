
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.ProgPath.ClsProgPath
Imports SPProgUtility.ProgPath
Imports SPProgUtility
Imports SPProgUtility.Mandanten


Public Class ClsMainSetting

  Public Property DbConnString2Open As String

  Public Property ModulName As String
  Public Property SQL2Open As String
  Public Property SQL4FieldShow As String
  Public Property SQLFields As String

  Public Property ExportFileName As String
  Public Property FieldSeprator As String
  Public Property FieldIn As String

  Public Shared TranslationData As Dictionary(Of String, ClsTranslationData)
  Public Shared ProsonalizedData As Dictionary(Of String, ClsProsonalizedData)
  Public Shared ProgSettingData As ClsCSVSettings
  Public Shared Property GetSelectedMDConnstring As String



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
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Shared ReadOnly Property SelectedMDData() As ClsMDData
    Get
      Dim m_md As New Mandant

      Return m_md.GetSelectedMDData(ProgSettingData.SelectedMDNr)
    End Get
  End Property






  Public Shared ReadOnly Property GetLLLicenceInfo() As String
    Get
      ' 18er Version
      Return "NgvEEQ"
    End Get
  End Property

  Public Shared ReadOnly Property GetAppGuidValue() As String
    Get
      Return "09d3110a-ad4f-4a93-99b4-ba4028206b34"
    End Get
  End Property

  ''// Query für Datensuche
  'Shared _strConnString As String
  'Public Shared Property GetDbConnString() As String
  '  Get
  '    Dim _ClsSystem As New SPProgUtility.ClsProgSettingPath
  '    Dim _strConnString As String = _ClsSystem.GetConnString()

  '    Return (_strConnString)
  '  End Get
  '  Set(ByVal value As String)
  '    _strConnString = value
  '  End Set
  'End Property

  ''// Query für Datensuche
  'Shared _strRootConnString As String
  'Public Shared Property GetDbRootConnString() As String
  '  Get
  '    Dim _ClsSystem As New SPProgUtility.ClsProgSettingPath
  '    Dim _strRootConnString As String = _ClsSystem.GetDbSelectConnString()

  '    Return _strRootConnString
  '  End Get
  '  Set(ByVal value As String)
  '    _strRootConnString = value
  '  End Set
  'End Property

End Class
