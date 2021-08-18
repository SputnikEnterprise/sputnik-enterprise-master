
Imports SPProgUtility

Public Class ClsOfDetails
	Public Shared iOfNr As Integer

	Public Shared strLLFaxNumber As String
	Public Shared strLLFaxRecp As String

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Public Shared m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper
	Public Shared m_InitialData As SP.Infrastructure.Initialization.InitializeClass

	Public Shared ReadOnly Property ChangeMandantData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass
		Get
			Dim m_md As New SPProgUtility.Mandanten.Mandant
			Dim clsMandant As ClsMDData = m_md.GetSelectedMDData(iMDNr)
			Dim logedUserData As ClsUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
			Dim personalizedData As Dictionary(Of String, ClsProsonalizedData) = m_md.GetPersonalizedCaptionInObject(iMDNr)

			Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
			Dim translate As Dictionary(Of String, ClsTranslationData) = clsTransalation.GetTranslationInObject

			m_InitialData = New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

			Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

		End Get
	End Property


	Public Shared ReadOnly Property GetLLLicenceInfo() As String
		Get
			Return "t/lJEQ"
		End Get
	End Property

	Public Shared ReadOnly Property GetMailDbName() As String
		Get
			Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
			Dim strDatabaseName As String = "Sputnik_MailDb"
			Dim strQuery As String = String.Format("//Mailing/Mail-Database")
			strDatabaseName = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetMDData_XMLFile, _
																																							 strQuery, strDatabaseName)

			Return strDatabaseName

		End Get
	End Property

	''// Message_ID
	'Shared _strMessageGuid As String
	'Public Shared Property GetMessageGuid() As String
	'	Get
	'		Return _strMessageGuid
	'	End Get
	'	Set(ByVal value As String)
	'		_strMessageGuid = value
	'	End Set
	'End Property

	''// Exportierte Datei (.PDF) für Mailversand von Offerte
	'Shared _strExportedFileName As String
	'Public Shared Property GetExportedFileName() As String
	'	Get
	'		Return _strExportedFileName
	'	End Get
	'	Set(ByVal value As String)
	'		_strExportedFileName = value
	'	End Set
	'End Property

	'// Temporäre LOG-Datei
	Shared _strTempLogFile As String
	Public Shared Property GetTempLogFile() As String
		Get
			Return _strTempLogFile
		End Get
		Set(ByVal value As String)
			_strTempLogFile = value
		End Set
	End Property

	'// Original-Query, die dem Programm übergeben wurde...
	Shared _strOrtQueryFile As String
	'Public Shared Property GetOrgProgQuery() As String
	'	Get
	'		Return _strOrtQueryFile
	'	End Get
	'	Set(ByVal value As String)
	'		_strOrtQueryFile = value
	'	End Set
	'End Property

	'// OFNr
	Shared _iOfNr As Integer
	Public Shared Property GetSelectedOFNr() As Integer
		Get
			Return _iOfNr
		End Get
		Set(ByVal value As Integer)
			_iOfNr = value
		End Set
	End Property

	'// KDNr
	Shared _iKDNr As Integer
	Public Shared Property GetSelectedKDNr() As Integer
		Get
			Return _iKDNr
		End Get
		Set(ByVal value As Integer)
			_iKDNr = value
		End Set
	End Property

	'// KDZNr
	Shared _iKDZNr As Integer
	Public Shared Property GetSelectedKDZNr() As Integer
		Get
			Return _iKDZNr
		End Get
		Set(ByVal value As Integer)
			_iKDZNr = value
		End Set
	End Property

	'// MANr
	Shared _iMANr As Integer
	Public Shared Property GetSelectedMANr() As Integer
		Get
			Return _iMANr
		End Get
		Set(ByVal value As Integer)
			_iMANr = value
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
