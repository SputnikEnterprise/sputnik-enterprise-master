

Imports SPProgUtility
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.CommonXmlUtility
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SP.Infrastructure


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

	Public Shared SecLevelData As Dictionary(Of Integer, UserSecurityData)

	''' <summary>
	''' if Payroll is in creating process
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Property IsPayrollInProcessing As Boolean

	Public Shared Property GridSettingPath As String

	Public Shared UserGridSettingsXml As SettingsXml

	Public Shared vb6Object As Object

	Private Shared m_ModuleCache As SP.ModuleCaching.ModuleCache
	Public Shared m_RPSuppressUIEvents As Boolean
	Private Shared m_UserSecurityData As IEnumerable(Of ClsUserSecData)

	Public Shared Property ListOfMandantData As List(Of MDData)

#Region "Private Consts"
	Private Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"
#End Region

	Public Shared Function GetModuleCach() As SP.ModuleCaching.ModuleCache

		If m_ModuleCache Is Nothing Then

			Dim m_Utility As New Utility
			Dim m_Mandant As New Mandant
			Dim mandantNumber = ModulConstants.MDData.MDNr
			Dim m_path As New SPProgUtility.ProgPath.ClsProgPath
			Dim mdFormXMLFilename = m_Mandant.GetSelectedMDFormDataXMLFilename(mandantNumber)
			Dim openemployeeformmorethanonce As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(mdFormXMLFilename, String.Format("{0}/openemployeeformmorethanonce", FORM_XML_MAIN_KEY)), True)
			Dim opencustomerformmorethanonce As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(mdFormXMLFilename, String.Format("{0}/opencustomerformmorethanonce", FORM_XML_MAIN_KEY)), True)
			Dim openeinsatzformmorethanonce As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(mdFormXMLFilename, String.Format("{0}/openeinsatzformmorethanonce", FORM_XML_MAIN_KEY)), True)
			Dim openreportsformmorethanonce As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(mdFormXMLFilename, String.Format("{0}/openreportsformmorethanonce", FORM_XML_MAIN_KEY)), True)
			Dim openadvancedpaymentformmorethanonce As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(mdFormXMLFilename, String.Format("{0}/openadvancedpaymentformmorethanonce", FORM_XML_MAIN_KEY)), True)
			Dim openinvoiceformmorethanonce As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(mdFormXMLFilename, String.Format("{0}/openinvoiceformmorethanonce", FORM_XML_MAIN_KEY)), True)

			m_ModuleCache = New SP.ModuleCaching.ModuleCache()

			m_ModuleCache.MaxEmployeeFormsToCache = If(openemployeeformmorethanonce, 2, 1)
			m_ModuleCache.MaxCustomerFormsToCache = If(opencustomerformmorethanonce, 2, 1)
			m_ModuleCache.MaxResponsiblePersonFormsToCache = If(opencustomerformmorethanonce, 2, 1)

			m_ModuleCache.MaxESFormsToCache = If(openeinsatzformmorethanonce, 2, 1)
			m_ModuleCache.MaxReportFormsToCache = If(openreportsformmorethanonce, 2, 1)
			m_ModuleCache.MaxAdvancePaymentFormsToCache = If(openadvancedpaymentformmorethanonce, 2, 1)

			m_ModuleCache.MaxPayrollFormsToCache = 1

			m_ModuleCache.MaxInvoiceFormsToCache = If(openinvoiceformmorethanonce, 2, 1)

		End If

		Return m_ModuleCache

	End Function


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
			If iUserNr = 0 Then
				iUserNr = m_md.GetDefaultUSNr
			End If
			UserData = m_md.GetSelectedUserData(iMDNr, iUserNr)

			Return UserData
		End Get
	End Property

	Public Shared ReadOnly Property LogedUSSecData(ByVal iMDNr As Integer, ByVal iUserNr As Integer) As Dictionary(Of Integer, UserSecurityData)
		Get
			If iUserNr = 0 Then
				Dim m_md As New Mandant
				iUserNr = m_md.GetDefaultUSNr
			End If
			Dim SecLevelData = GetUserSecLevelInObject(iMDNr, iUserNr)

			Return SecLevelData
		End Get
	End Property

	Public Shared ReadOnly Property UserSecValue(ByVal funcNumber As Integer?) As Boolean
		Get
			Dim SecData As Boolean = False

			If SecLevelData.ContainsKey(665) Then If Not SecLevelData(665).Autorized Then Return True
			If SecLevelData.ContainsKey(funcNumber) Then
				SecData = SecLevelData(funcNumber).Autorized
			Else

			End If

			Return SecData
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
