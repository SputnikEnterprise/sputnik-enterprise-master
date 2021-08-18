
'Imports System.Windows.Forms
'Imports SPProgUtility.SPTranslation.ClsTranslation
'Imports SPProgUtility.ProgPath.ClsProgPath
'Imports SPProgUtility.ProgPath
'Imports SPProgUtility
'Imports SPProgUtility.Mandanten

'Public Class ClsDataDetail

'	'Public Shared TranslationData As Dictionary(Of String, ClsTranslationData)
'	'Public Shared ProsonalizedData As Dictionary(Of String, ClsProsonalizedData)
'	'Public Shared ProgSettingData As ClsSetting
'	'Public Shared Property GetSelectedMDConnstring As String


'	'''' <summary>
'	'''' The translation value helper.
'	'''' </summary>
'	'Public Shared m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper
'	'Public Shared m_InitialData As SP.Infrastructure.Initialization.InitializeClass

'	'Public Shared ReadOnly Property ChangeMandantData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass
'	'	Get
'	'		Dim m_md As New SPProgUtility.Mandanten.Mandant
'	'		Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
'	'		Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
'	'		Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

'	'		Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
'	'		Dim translate = clsTransalation.GetTranslationInObject

'	'		m_InitialData = New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

'	'		Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

'	'	End Get
'	'End Property




'	'Public Shared MDData As New ClsMDData
'	'Public Shared UserData As New ClsUserData


'	' ''' <summary>
'	' ''' alle übersetzte Elemente in 3 Sprachen
'	' ''' </summary>
'	' ''' <value></value>
'	' ''' <returns></returns>
'	' ''' <remarks></remarks>
'	'Public Shared ReadOnly Property Translation() As Dictionary(Of String, ClsTranslationData)
'	'  Get
'	'    Try
'	'      Dim m_Translation As New SPProgUtility.SPTranslation.ClsTranslation
'	'      Return m_Translation.GetTranslationInObject

'	'    Catch ex As Exception
'	'      Return Nothing
'	'    End Try
'	'  End Get
'	'End Property

'	' ''' <summary>
'	' ''' Individuelle Bezeichnungen für Labels
'	' ''' </summary>
'	' ''' <value></value>
'	' ''' <returns></returns>
'	' ''' <remarks></remarks>
'	'Public Shared ReadOnly Property ProsonalizedName() As Dictionary(Of String, ClsProsonalizedData)
'	'  Get
'	'    Try
'	'      Dim m As New Mandant
'	'      Return m.GetPersonalizedCaptionInObject(ProgSettingData.SelectedMDNr)

'	'    Catch ex As Exception
'	'      Return Nothing
'	'    End Try
'	'  End Get
'	'End Property

'	' ''' <summary>
'	' ''' Datenbankeinstellungen für [Sputnik DBSelect]
'	' ''' </summary>
'	' ''' <value></value>
'	' ''' <returns></returns>
'	' ''' <remarks></remarks>
'	'Public Shared ReadOnly Property SelectedDbSelectData() As ClsDbSelectData
'	'  Get
'	'    Dim m_Progpath As New ClsProgPath

'	'    Return m_Progpath.GetDbSelectData()
'	'  End Get
'	'End Property

'	' ''' <summary>
'	' ''' Datenbankeinstellungen aus der \\Server\spenterprise$\bin\Programm.XML. gemäss Mandantennummer
'	' ''' </summary>
'	' ''' <param name="iMDNr"></param>
'	' ''' <value></value>
'	' ''' <returns></returns>
'	' ''' <remarks></remarks>
'	'Public Shared ReadOnly Property SelectedMDData(ByVal iMDNr As Integer) As ClsMDData
'	'  Get
'	'    Dim m_md As New Mandant
'	'    MDData = m_md.GetSelectedMDData(iMDNr) 'ProgSettingData.SelectedMDNr)
'	'    ProgSettingData.SelectedMDNr = MDData.MDNr
'	'    GetSelectedMDConnstring = MDData.MDDbConn

'	'    Return MDData
'	'  End Get
'	'End Property

'	' ''' <summary>
'	' ''' Benutzerdaten aus der Datenbank
'	' ''' </summary>
'	' ''' <param name="iMDNr"></param>
'	' ''' <param name="iUserNr"></param>
'	' ''' <value></value>
'	' ''' <returns></returns>
'	' ''' <remarks></remarks>
'	'Public Shared ReadOnly Property LogededUSData(ByVal iMDNr As Integer, ByVal iUserNr As Integer) As ClsUserData
'	'  Get
'	'    Dim m_md As New Mandant
'	'    UserData = m_md.GetSelectedUserData(iMDNr, iUserNr) 'ProgSettingData.SelectedMDNr, ProgSettingData.LogedUSNr)
'	'    ProgSettingData.LogedUSNr = UserData.UserNr

'	'    Return UserData
'	'  End Get
'	'End Property

'	' ''' <summary>
'	' ''' Benutzerdaten aus der Datenbank
'	' ''' </summary>
'	' ''' <param name="iMDNr"></param>
'	' ''' <param name="strLastname"></param>
'	' ''' <param name="strFirstname"></param>
'	' ''' <value></value>
'	' ''' <returns></returns>
'	' ''' <remarks></remarks>
'	'Public Shared ReadOnly Property LogededUSData(ByVal iMDNr As Integer, ByVal strLastname As String, ByVal strFirstname As String) As ClsUserData
'	'  Get
'	'    Dim m_md As New Mandant
'	'    UserData = m_md.GetSelectedUserDataWithUserName(iMDNr, strLastname, strFirstname)
'	'    ProgSettingData.LogedUSNr = UserData.UserNr

'	'    Return UserData
'	'  End Get
'	'End Property





'	Public Shared ReadOnly Property GetAppGuidValue() As String
'    Get
'      Return "D3F3B380-0526-4507-8D4E-4453AF4C67D4"
'    End Get
'  End Property

'	'''// Query für Datensuche
'	''Shared _strConnString As String
'	''Public Shared Property GetDbConnString() As String
'	''  Get
'	''    Dim _strConnString As String = SelectedMDData.MDDbConn

'	''    Return (_strConnString)
'	''  End Get
'	''  Set(ByVal value As String)
'	''    _strConnString = value
'	''  End Set
'	''End Property

'	'''// Query für Datensuche
'	''Shared _strRootConnString As String
'	''Public Shared Property GetDbRootConnString() As String
'	''  Get
'	''    Dim _strRootConnString As String = SelectedDbSelectData.MDDbConn

'	''    Return _strRootConnString
'	''  End Get
'	''  Set(ByVal value As String)
'	''    _strRootConnString = value
'	''  End Set
'	''End Property

'	''// SQL-Query
'	'Shared _SQLString As String
'	'Public Shared Property SQLQuery() As String
'	'  Get
'	'    Return _SQLString
'	'  End Get
'	'  Set(ByVal value As String)
'	'    _SQLString = value
'	'  End Set
'	'End Property

'	''// Ist als DTA?
'	'Shared _bAsDTA As Boolean
'	'Public Shared Property bAsDTA() As Boolean
'	'  Get
'	'    Return _bAsDTA
'	'  End Get
'	'  Set(ByVal value As Boolean)
'	'    _bAsDTA = value
'	'  End Set
'	'End Property

'	''// Sorted from frmkdsearch_LV
'	'Shared _strSortBez As String
'	'Public Shared Property GetLVSortBez() As String
'	'  Get
'	'    Return _strSortBez
'	'  End Get
'	'  Set(ByVal value As String)
'	'    _strSortBez = value
'	'  End Set
'	'End Property

'End Class
