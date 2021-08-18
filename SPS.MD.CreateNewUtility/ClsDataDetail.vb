
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.ProgPath.ClsProgPath
Imports SPProgUtility.ProgPath
Imports SPProgUtility
Imports SPProgUtility.Mandanten

Public Class ClsDataDetail

	'Public Shared TranslationData As Dictionary(Of String, ClsTranslationData)
	'Public Shared ProsonalizedData As Dictionary(Of String, ClsProsonalizedData)
	'Public Shared ProgSettingData As ClsSetting


	Public Shared ReadOnly Property GetAppGuidValue() As String
    Get
      Return "160fb05f-3e43-4c36-90e8-6ec502acab37"
    End Get
  End Property


	'Public Shared ReadOnly Property ProsonalizedName() As Dictionary(Of String, ClsProsonalizedData)
	'  Get
	'    Try
	'      Dim m As New Mandant
	'      Return m.GetPersonalizedCaptionInObject(ProgSettingData.SelectedMDNr)

	'    Catch ex As Exception
	'      Return Nothing
	'    End Try
	'  End Get
	'End Property


	'Public Shared ReadOnly Property Translation() As Dictionary(Of String, ClsTranslationData)
	'  Get
	'    Try
	'      Dim m_Translation As New SPProgUtility.SPTranslation.ClsTranslation
	'      Return m_Translation.GetTranslationInObject

	'    Catch ex As Exception
	'      Return Nothing
	'    End Try
	'  End Get
	'End Property

	'Public Shared ReadOnly Property SelectedDbSelectData() As ClsDbSelectData
	'  Get
	'    Dim m_Progpath As New ClsProgPath

	'    Return m_Progpath.GetDbSelectData()
	'  End Get
	'End Property

	'Public Shared ReadOnly Property SelectedMDData() As ClsMDData
	'  Get
	'    Dim m_md As New Mandant

	'    Return m_md.GetSelectedMDData(ProgSettingData.SelectedMDNr)
	'  End Get
	'End Property

	''// Query für Datensuche
	'Shared _strConnString As String
	'Public Shared Property GetDbConnString() As String
	'  Get
	'    Dim _strConnString As String = SelectedMDData.MDDbConn

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
	'    Dim _strRootConnString As String = SelectedDbSelectData.MDDbConn

	'    Return _strRootConnString
	'  End Get
	'  Set(ByVal value As String)
	'    _strRootConnString = value
	'  End Set
	'End Property

End Class
