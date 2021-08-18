
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.ProgPath.ClsProgPath
Imports SPProgUtility.ProgPath
Imports SPProgUtility
Imports SPProgUtility.Mandanten
Imports System.Windows.Forms

Public Class ClsDataDetail

  Public Shared strKDData As String = String.Empty
  Public Shared strButtonValue As String = String.Empty
  Public Shared strUSSignFilename As String = String.Empty

  Public Shared Get4What As String = String.Empty

  Public Shared GetSortBez As String = String.Empty
  Public Shared Property LLExportFileName As New List(Of String)
  Public Shared GetFilterBez2 As String = String.Empty
  Public Shared GetFilterBez3 As String = String.Empty
  Public Shared GetFilterBez4 As String = String.Empty

  Public Shared Property bBodyAsHtml As Boolean


  Public Shared TranslationData As Dictionary(Of String, ClsTranslationData)
  Public Shared ProsonalizedData As Dictionary(Of String, ClsProsonalizedData)
  Public Shared ProgSettingData As ClsSetting
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




  Public Shared ReadOnly Property GetAppGuidValue() As String
    Get
      Return "f6789065-9689-4c52-8128-7d686149e2c2"
    End Get
  End Property


  '// SQL-Query
  Shared _SQLString As String
  Public Shared Property SQLQuery() As String
    Get
      Return _SQLString
    End Get
    Set(ByVal value As String)
      _SQLString = value
    End Set
  End Property

  '// SelectedNumbers
  Shared _SelectedNumbers As String = ""
  Public Shared Property GetSelectedNumbers() As String
    Get
      Return _SelectedNumbers
    End Get
    Set(ByVal value As String)
      _SelectedNumbers = value
    End Set
  End Property

  '// SelectedBez
  Shared _SelectedBez As String = ""
  Public Shared Property GetSelectedBez() As String
    Get
      Return _SelectedBez
    End Get
    Set(ByVal value As String)
      _SelectedBez = value
    End Set
  End Property

  '// Darf geändert werden?
  Shared _bAllowedChange As Boolean
  Public Shared Property bAllowedToChange() As Boolean
    Get
      Return _bAllowedChange
    End Get
    Set(ByVal value As Boolean)
      _bAllowedChange = value
    End Set
  End Property

  '// Darf KST geändert werden?
  Shared _bAllowedChangeKST As Boolean
  Public Shared Property bAllowedTochangeKST() As Boolean
    Get
      Return _bAllowedChangeKST
    End Get
    Set(ByVal value As Boolean)
      _bAllowedChangeKST = value
    End Set
  End Property

  '// Ist der Satz New?
  Shared _bAsNew As Boolean
  Public Shared Property bAsNew() As Boolean
    Get
      Return _bAsNew
    End Get
    Set(ByVal value As Boolean)
      _bAsNew = value
    End Set
  End Property

  '// Spaltenbreite von MAVorstellung
  Shared _bAllowedWriteMAVorColWidth As Boolean
  Public Shared Property bAllowedWriteMAVorColWidth() As Boolean
    Get
      Return _bAllowedWriteMAVorColWidth
    End Get
    Set(ByVal value As Boolean)
      _bAllowedWriteMAVorColWidth = value
    End Set
  End Property

  '// Spaltenbreite von MAKontakt
  Shared _bAllowedWriteMAKontaktColWidth As Boolean
  Public Shared Property bAllowedWriteMAKontaktColWidth() As Boolean
    Get
      Return _bAllowedWriteMAKontaktColWidth
    End Get
    Set(ByVal value As Boolean)
      _bAllowedWriteMAKontaktColWidth = value
    End Set
  End Property

  '// Spaltenbreite von KDKontakt
  Shared _bAllowedWriteKDKontaktColWidth As Boolean
  Public Shared Property bAllowedWriteKDKontaktColWidth() As Boolean
    Get
      Return _bAllowedWriteKDKontaktColWidth
    End Get
    Set(ByVal value As Boolean)
      _bAllowedWriteKDKontaktColWidth = value
    End Set
  End Property

  '// Vorschlagsnummer
  Public Shared Property GetProposalNr As Integer

  '// Kandidatennummer
  Public Shared Property GetProposalMANr As Integer

  '// Kundenennummer
  Public Shared Property GetProposalKDNr As Integer

  '// ZHD-Nummer
  Public Shared Property GetProposalZHDNr As Integer?

  '// Vakanzen-Nummer
  Public Shared Property GetProposalVakNr As Integer?

  ' Helps extracting a column value form a data reader.
  Public Shared Function GetColumnTextStr(ByVal dr As SqlClient.SqlDataReader, _
                                          ByVal columnName As String, ByVal replacementOnNull As String) As String

    If Not dr.IsDBNull(dr.GetOrdinal(columnName)) Then
      If String.IsNullOrEmpty(CStr(dr(columnName))) Then
        Return replacementOnNull
      End If
      Return CStr(dr(columnName))
    End If

    Return replacementOnNull
  End Function

End Class
