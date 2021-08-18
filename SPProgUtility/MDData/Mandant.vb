
Imports NLog
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.ProgPath
Imports System.IO
Imports DevExpress.XtraGrid.Columns
Imports SPProgUtility.CommonXmlUtility

Namespace Mandanten

	Public Class Mandant


#Region "private consts"

		Private Const MANDANT_XML_SETTING_SPUTNIK_AUTOFILTERCONDITION_NUMBER As String = "MD_{0}/Sonstiges/autofilterconditionnr"
		Private Const MANDANT_XML_SETTING_SPUTNIK_AUTOFILTERCONDITION_DATE As String = "MD_{0}/Sonstiges/autofilterconditiondate"
		Private Const MANDANT_XML_SETTING_SPUTNIK_MAILING_SETTING As String = "MD_{0}/Mailing"

#End Region

#Region "private fields"

		Private Shared logger As Logger = LogManager.GetCurrentClassLogger()

		''' <summary>
		''' Settings xml.
		''' </summary>
		Private m_MandantSettingsXml As SettingsXml

		Private m_Progpath As ClsProgPath
		Private m_DatabaseAccess As DataBaseAccess
		Private m_Reg As ClsDivReg
		Private m_utilitiy As New Utilities


#End Region


#Region "public properties..."

		Public ReadOnly Property GetDefaultMDNr() As Integer
			Get
				m_Reg = New ClsDivReg
				Return m_Reg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "MDNr")
			End Get
		End Property

		Public ReadOnly Property GetDefaultUSNr() As Integer
			Get
				m_Reg = New ClsDivReg
				Return CInt(m_Reg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "UserNr").ToString)
			End Get
		End Property

#End Region


#Region "SelectedMD Infos..."


		''' <summary>
		''' Die MD-Data aus der \\Server\spenterprise$\Bin\Programm.xml
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GetSelectedMDData(ByVal iMDNr As Integer) As ClsMDData 'Implements IMandant.GetSelectedMDData
			Dim result As New ClsMDData
			If iMDNr = 0 Then iMDNr = Me.GetDefaultMDNr
			m_Progpath = New ClsProgPath
			Dim settingQuery = Nothing
			'logger.Debug(String.Format("MDNr: {0} | GetFileServerXMLFullFilename: {1}", iMDNr, m_Progpath.GetFileServerXMLFullFilename))

			Try
				Dim rootXMLFilename = m_Progpath.GetFileServerXMLFullFilename
				If Not File.Exists(rootXMLFilename) Then
					Throw New Exception(String.Format("Einstellungsdatei für Mandant {0} >>> {1} wurde nicht gefunden.", iMDNr, m_Progpath.GetFileServerXMLFullFilename))
				End If
				Dim xDoc As XDocument = XDocument.Load(rootXMLFilename)
				Dim ConfigQuery = (From exportSetting In xDoc.Root.Elements("databaseconnections").Elements(String.Format("md_{0}", iMDNr))
								   Select New With {
										   .mdconnstr = m_utilitiy.GetSafeStringFromXElement(exportSetting.Element("connstr")),
										   .mddbname = m_utilitiy.GetSafeStringFromXElement(exportSetting.Element("dbname")),
										   .mddbserver = m_utilitiy.GetSafeStringFromXElement(exportSetting.Element("dbserver")),
										   .mdmainpath = m_utilitiy.GetSafeStringFromXElement(exportSetting.Element("mdmainpath")),
										   .mdguid = m_utilitiy.GetSafeStringFromXElement(exportSetting.Element("mdguid")),
										   .multimd = m_utilitiy.GetSafeStringFromXElement(exportSetting.Element("multimd")),
										   .closedmd = m_utilitiy.GetSafeStringFromXElement(exportSetting.Element("closedmd"))
											   }).FirstOrDefault()
				Try

					settingQuery = (From exportSetting In xDoc.Root.Elements("programcommonsettings")
									Select New With {.webservicedomain = m_utilitiy.GetSafeStringFromXElement(exportSetting.Element("webservicedomain"))}).FirstOrDefault()
				Catch ex As Exception

				End Try

				result.MDNr = iMDNr

				If Not ConfigQuery Is Nothing Then

					result.MDDbConn = m_utilitiy.FromBase64(ConfigQuery.mdconnstr)
					result.MDDbName = ConfigQuery.mddbname
					result.MDDbServer = ConfigQuery.mddbserver
					result.MDMainPath = ConfigQuery.mdmainpath
					result.MDGuid = ConfigQuery.mdguid
					result.MultiMD = ConfigQuery.multimd
					result.ClosedMD = Val(ConfigQuery.closedmd)
					If Not settingQuery Is Nothing AndAlso Not settingQuery.webservicedomain Is Nothing Then

					End If
					If settingQuery Is Nothing OrElse settingQuery.webservicedomain Is Nothing OrElse String.IsNullOrWhiteSpace(settingQuery.webservicedomain) Then result.WebserviceDomain = "http://asmx.domain.com" Else result.WebserviceDomain = settingQuery.webservicedomain


					Dim dbacsess As New DataBaseAccess
					Dim resultMDData As New ClsMDData
					resultMDData = dbacsess.OpenSelectedMDDatabase(iMDNr, result.MDDbConn)

					If resultMDData Is Nothing OrElse resultMDData.MDYear = 0 Then
						Throw New Exception(String.Format("database {0} could not be founded! Connection: {1}", result.MDDbName, result.MDDbConn))
					End If
					result.MDName = resultMDData.MDName
					result.MDName_2 = resultMDData.MDName_2
					result.MDName_3 = resultMDData.MDName_3
					result.MDStreet = resultMDData.MDStreet
					result.MDPostcode = resultMDData.MDPostcode
					result.MDCity = resultMDData.MDCity
					result.MDCountry = resultMDData.MDCountry
					result.MDCanton = resultMDData.MDCanton
					result.MDTelefon = resultMDData.MDTelefon
					result.MDTelefax = resultMDData.MDTelefax
					result.MDeMail = resultMDData.MDeMail
					result.MDHomepage = resultMDData.MDHomepage
					result.MDGuid = resultMDData.MDGuid
					result.MDYear = resultMDData.MDYear
					result.MDRootPath = resultMDData.MDRootPath

					Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}/Sonstiges", iMDNr)
					Dim mandantXMLYearfilename = String.Format("{0}Programm.XML", String.Format("{0}{1}\", result.MDMainPath, result.MDYear))
					If Not File.Exists(mandantXMLYearfilename) Then
						Throw New Exception(String.Format("Der eingetragene Einstellungsdatei in {0} für Mandant {1} ({2}) ist falsch!", rootXMLFilename, iMDNr, mandantXMLYearfilename))
					End If

					Dim colorname As String = m_Progpath.GetXMLNodeValue(mandantXMLYearfilename, String.Format("{0}/mandantcolor", FORM_XML_MAIN_KEY))

					m_MandantSettingsXml = New SettingsXml(mandantXMLYearfilename)
					result.MandantColorName = colorname
					result.AutoFilterConditionNumber = AutoFilterConditionNumber(iMDNr)
					result.AutoFilterConditionDate = AutoFilterConditionDate(iMDNr)

				End If


			Catch ex As Exception
				logger.Error(String.Format("MDNr: {0} | GetFileServerXMLFullFilename: {1} | {2}", iMDNr, m_Progpath.GetFileServerXMLFullFilename, ex.ToString))
				result = Nothing
			End Try

			Return result
		End Function


		Private Function AutoFilterConditionDate(ByVal mdNr As Integer) As AutoFilterCondition
			Dim value As String = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_AUTOFILTERCONDITION_DATE, mdNr))
			Dim result As AutoFilterCondition
			If String.IsNullOrWhiteSpace(value) Then
				result = AutoFilterCondition.Contains
			Else
				result = CType(value, AutoFilterCondition)
			End If

			Return result

		End Function

		Private Function AutoFilterConditionNumber(ByVal mdNr As Integer) As AutoFilterCondition
			Dim value As String = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_AUTOFILTERCONDITION_NUMBER, mdNr))
			Dim result As AutoFilterCondition
			If String.IsNullOrWhiteSpace(value) Then
				result = AutoFilterCondition.Contains
			Else
				result = CType(value, AutoFilterCondition)
			End If

			Return result

		End Function


		''' <summary>
		''' Server - Mandanten-Jahresverzeichnis: \\Server\spenterprise$\MDxx\Jahr\
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GetSelectedMDYearPath(ByVal iMDNr As Integer, ByVal iYear As Integer) As String 'Implements IMandant.GetSelectedMDYearPath
			Dim strValue As String = String.Empty
			Try
				If iMDNr = 0 Then iMDNr = Me.GetDefaultMDNr
				If iYear = 0 Then iYear = Now.Year

				Dim mdData = GetSelectedMDData(iMDNr)
				If mdData Is Nothing Then Return String.Empty
				strValue = String.Format("{0}{1}\", mdData.MDMainPath, iYear)

			Catch ex As Exception
				logger.Error(ex.ToString)
			End Try

			Return strValue
		End Function

		''' <summary>
		''' Server - Dokumentenverzeichnis von Mandanten : \\Server\spenterprise$\MDxx\Documents\
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GetSelectedMDDocPath(ByVal iMDNr As Integer) As String 'Implements IMandant.GetSelectedMDDocPath
			Dim strValue As String = String.Empty
			Try
				If iMDNr = 0 Then iMDNr = Me.GetDefaultMDNr

				Dim mdData = GetSelectedMDData(iMDNr)
				If mdData Is Nothing Then Return String.Empty

				strValue = String.Format("{0}Documents\", mdData.MDMainPath)

			Catch ex As Exception
				logger.Error(ex.ToString)
			End Try

			Return strValue
		End Function

		''' <summary>
		''' Mandanten: \\Server\spenterprise$\MDxx\Templates\
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GetSelectedMDTemplatePath(ByVal iMDNr As Integer) As String 'Implements IMandant.GetSelectedMDTemplatePath
			Dim mdData = GetSelectedMDData(iMDNr)
			If mdData Is Nothing Then Return String.Empty

			Return String.Format("{0}Templates\", mdData.MDMainPath)
		End Function

		''' <summary>
		''' Mandanten: \\Server\spenterprise$\MDxx\Skins\
		''' </summary>
		''' <param name="iMDNr"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GetSelectedMDSkinPath(ByVal iMDNr As Integer) As String 'Implements IMandant.GetSelectedMDSkinPath
			Dim strValue As String = String.Empty
			Try
				If iMDNr = 0 Then iMDNr = Me.GetDefaultMDNr

				strValue = String.Format("{0}Skins\", GetSelectedMDTemplatePath(iMDNr))

				Try
					If Not File.Exists(String.Format("{0}MainView.XML", strValue)) AndAlso File.Exists(String.Format("{0}MainView.XML", GetSelectedMDTemplatePath(iMDNr))) Then
						File.Copy(String.Format("{0}MainView.XML", GetSelectedMDTemplatePath(iMDNr)), String.Format("{0}MainView.XML", strValue))

						Try
							File.Delete(String.Format("{0}MainView.XML", GetSelectedMDTemplatePath(iMDNr)))

						Catch ex As Exception

						End Try

					End If

				Catch ex As Exception
					logger.Error(String.Format("GetSelectedMDSkinPath: {0}", ex.StackTrace))
				End Try


			Catch ex As Exception
				logger.Error(ex.ToString)
			End Try

			Return strValue
		End Function

		''' <summary>
		''' Mandanten: \\Server\spenterprise$\MDxx\Images\
		''' </summary>
		''' <param name="iMDNr"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GetSelectedMDImagePath(ByVal iMDNr As Integer) As String 'Implements IMandant.GetSelectedMDImagePath
			Dim strValue As String = String.Empty
			Try
				If iMDNr = 0 Then iMDNr = Me.GetDefaultMDNr

				Dim mdData = GetSelectedMDData(iMDNr)
				If mdData Is Nothing Then Return String.Empty

				strValue = String.Format("{0}Images\", mdData.MDMainPath)

			Catch ex As Exception
				logger.Error(ex.ToString)
			End Try

			Return strValue
		End Function

		''' <summary>
		''' Mandanten: \\Server\spenterprise$\MDxx\Skins\FormData.XML
		''' </summary>
		''' <param name="iMDNr"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GetSelectedMDFormDataXMLFilename(ByVal iMDNr As Integer) As String
			Dim strValue As String = String.Empty
			Try
				If iMDNr = 0 Then iMDNr = Me.GetDefaultMDNr

				strValue = String.Format("{0}FormData.XML", Me.GetSelectedMDSkinPath(iMDNr))

			Catch ex As Exception
				logger.Error(ex.ToString)
			End Try

			Return strValue
		End Function

		''' <summary>
		''' Mandanten: \\Server\spenterprise$\MDxx\Profiles\GridSetting\
		''' </summary>
		''' <param name="iMDNr"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GetGridSettingPath(ByVal iMDNr As Integer) As String

			If iMDNr = 0 Then iMDNr = Me.GetDefaultMDNr
			Dim strPath As String = String.Format("{0}GridSetting\", GetSelectedMDUserProfilePath(iMDNr))

			Try
				If Not Directory.Exists(strPath) Then Directory.CreateDirectory(strPath)

				Try

					If Not File.Exists(String.Format("{0}UserGridSetting.XML", strPath)) AndAlso File.Exists(String.Format("{0}UserGridSetting.XML", GetSelectedMDTemplatePath(iMDNr))) Then
						File.Copy(String.Format("{0}UserGridSetting.XML", GetSelectedMDTemplatePath(iMDNr)), String.Format("{0}UserGridSetting.XML", strPath))

						Try
							File.Delete(String.Format("{0}UserGridSetting.XML", GetSelectedMDTemplatePath(iMDNr)))
						Catch ex As Exception

						End Try

					End If

				Catch ex As Exception
					logger.Error(String.Format("GetGridSettingPath: {0}", ex.StackTrace))
				End Try


			Catch ex As Exception
				logger.Error(ex.StackTrace)
			End Try

			Return strPath

		End Function

		''' <summary>
		''' Mandanten: \\Server\spenterprise$\MDxx\Profiles\
		''' </summary>
		''' <param name="iMDNr"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GetSelectedMDUserProfilePath(ByVal iMDNr As Integer) As String
			Dim strValue As String = String.Empty
			Try
				If iMDNr = 0 Then iMDNr = Me.GetDefaultMDNr

				Dim mdData = GetSelectedMDData(iMDNr)
				If mdData Is Nothing Then Return String.Empty
				strValue = String.Format("{0}Profiles\", mdData.MDMainPath)

			Catch ex As Exception
				logger.Error(ex.ToString)
			End Try

			Return strValue
		End Function


		''' <summary>
		''' Mandanten: \\Server\spenterprise$\MDxx\Skins\SelectData.XML
		''' </summary>
		''' <param name="iMDNr"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GetSelectedMDSQLDataXMLFilename(ByVal iMDNr As Integer) As String
			Dim strValue As String = String.Empty
			Try
				If iMDNr = 0 Then iMDNr = Me.GetDefaultMDNr

				strValue = String.Format("{0}SelectData.XML", Me.GetSelectedMDSkinPath(iMDNr))

			Catch ex As Exception
				logger.Error(ex.ToString)
			End Try

			Return strValue
		End Function

		''' <summary>
		''' Mandanten: \\Server\spenterprise$\MDxx\Skins\MainView.XML
		''' </summary>
		''' <param name="iMDNr"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GetSelectedMDMainViewXMLFilename(ByVal iMDNr As Integer) As String 'Implements IMandant.GetSelectedMDMainViewXMLFilename
			Dim strValue As String = String.Empty
			If iMDNr = 0 Then iMDNr = Me.GetDefaultMDNr

			strValue = String.Format("{0}MainView.XML", Me.GetSelectedMDSkinPath(iMDNr))

			Return strValue
		End Function

		''' <summary>
		''' Mandanten: \\Server\spenterprise$\MDxx\Jahr\Programm.XML
		''' </summary>
		''' <param name="iMDNr"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GetSelectedMDDataXMLFilename(ByVal iMDNr As Integer, ByVal iYear As Integer) As String 'Implements IMandant.GetSelectedMDDataXMLFilename
			Dim strValue As String = String.Empty

			Try
				If iMDNr = 0 Then iMDNr = Me.GetDefaultMDNr
				strValue = String.Format("{0}Programm.XML", Me.GetSelectedMDYearPath(iMDNr, iYear))

			Catch ex As Exception
				logger.Error(ex.ToString)
			End Try

			Return strValue
		End Function

		''' <summary>
		''' Mandanten: \\Server\spenterprise$\MDxx\Profiles\UserProfile{UserNr}.XML
		''' </summary>
		''' <param name="iMDNr"> 0 for standard from registry</param>
		''' <param name="iUSNr"> 0 for standard from registry</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GetSelectedMDUserProfileXMLFilename(ByVal iMDNr As Integer, ByVal iUSNr As Integer) As String 'Implements IMandant.GetSelectedMDUserProfileXMLFilename
			Dim strValue As String = String.Empty
			Try

				If iMDNr = 0 Then iMDNr = Me.GetDefaultMDNr
				If iUSNr = 0 Then iUSNr = Me.GetDefaultUSNr

				strValue = String.Format("{0}UserProfile{1}.XML", Me.GetSelectedMDUserProfilePath(iMDNr), iUSNr)

			Catch ex As Exception
				logger.Error(ex.ToString)
			End Try

			Return strValue
		End Function

		''' <summary>
		''' Mandanten: \\Server\spenterprise$\MDxx\Profiles\GridSetting\UserGridSetting{UserNr}.XML
		''' </summary>
		''' <param name="iMDNr"> 0 for standard from registry</param>
		''' <param name="iUSNr"> 0 for standard from registry</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GetUserGridSettingXMLFilename(ByVal iMDNr As Integer, ByVal iUSNr As Integer) As String
			Dim strValue As String = String.Empty
			Try

				If iMDNr = 0 Then iMDNr = Me.GetDefaultMDNr
				If iUSNr = 0 Then iUSNr = Me.GetDefaultUSNr

				strValue = String.Format("{0}UserGridSetting{1}.XML", Me.GetGridSettingPath(iMDNr), iUSNr)

			Catch ex As Exception
				logger.Error(ex.StackTrace)
			End Try

			Return strValue
		End Function

		''' <summary>
		''' Mandanten: \\Server\spenterprise$\MDxx\Profiles\GridSetting\UserGridSetting.XML
		''' </summary>
		''' <param name="iMDNr"> 0 for standard from registry</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GetAllUserGridSettingXMLFilename(ByVal iMDNr As Integer) As String
			Dim strValue As String = String.Empty
			Try

				If iMDNr = 0 Then iMDNr = Me.GetDefaultMDNr

				strValue = String.Format("{0}UserGridSetting.XML", Me.GetGridSettingPath(iMDNr))

			Catch ex As Exception
				logger.Error(ex.StackTrace)
			End Try

			Return strValue
		End Function


#End Region


		''' <summary>
		''' MDData in List form
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Function GetSelectedMDDataInList(ByVal iMDNr As Integer) As List(Of String)
			Dim result As New List(Of String)
			m_DatabaseAccess = New DataBaseAccess
			Dim MDData = m_DatabaseAccess.OpenSelectedMDDatabase(iMDNr, Me.GetSelectedMDData(iMDNr).MDDbConn)

			Try
				result.Add(MDData.MDGuid)
				result.Add(MDData.MDName)
				result.Add(MDData.MDName_2)
				result.Add(MDData.MDStreet)
				result.Add(MDData.MDPostcode)
				result.Add(MDData.MDCity)
				result.Add(MDData.MDCountry)
				result.Add(MDData.MDTelefon)
				result.Add(MDData.MDTelefax)
				result.Add(MDData.MDeMail)
				result.Add(MDData.MDGuid)
				result.Add(MDData.MDYear)

			Catch ex As Exception
				logger.Error(ex.ToString)

			End Try

			Return result
		End Function

		''' <summary>
		''' MDData in MDObject
		''' </summary>
		''' <param name="iMDNr"></param>
		''' <param name="iYear"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function GetMDData4SelectedMD(ByVal iMDNr As Integer, ByVal iYear As Integer) As ClsMDData
			m_DatabaseAccess = New DataBaseAccess
			Return m_DatabaseAccess.OpenSelectedMDDatabase(iMDNr, Me.GetSelectedMDData(iMDNr).MDDbConn)
		End Function

		''' <summary>
		''' gets Userdata into UserObject With UserNr
		''' </summary>
		''' <param name="iMDNr"></param>
		''' <param name="iUserNr"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function GetSelectedUserData(ByVal iMDNr As Integer, ByVal iUserNr As Integer) As ClsUserData
			m_DatabaseAccess = New DataBaseAccess
			Return m_DatabaseAccess.OpenSelectedUserDatabase(Me.GetSelectedMDData(iMDNr).MDDbConn, iUserNr)
		End Function

		''' <summary>
		''' gets Userdata into UserObject with KST
		''' </summary>
		''' <param name="iMDNr"></param>
		''' <param name="UserKst"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function GetSelectedUserDataWithKST(ByVal iMDNr As Integer, ByVal UserKst As String) As ClsUserData
			m_DatabaseAccess = New DataBaseAccess
			Return m_DatabaseAccess.OpenSelectedUserDatabaseWithKST(Me.GetSelectedMDData(iMDNr).MDDbConn, UserKst)
		End Function

		''' <summary>
		''' gets Userdata into UserObject with userlast and firstname
		''' </summary>
		''' <param name="iMDNr"></param>
		''' <param name="UserLastname"></param>
		''' <param name="UserFirstname"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function GetSelectedUserDataWithUserName(ByVal iMDNr As Integer, ByVal UserLastname As String, ByVal UserFirstname As String) As ClsUserData
			m_DatabaseAccess = New DataBaseAccess
			Return m_DatabaseAccess.OpenSelectedUserDatabaseWithUserName(Me.GetSelectedMDData(iMDNr).MDDbConn, UserLastname, UserFirstname)
		End Function


		''' <summary>
		''' Gibt den selected Mandanten-Guid 
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Function GetMDGuid(ByVal iMDNr As Integer) As String
			Return GetMDData4SelectedMD(iMDNr, Now.Year).MDGuid
		End Function

		''' <summary>
		''' Gibt den selected User-Guid 
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Function GetUserGuid(ByVal iMDNr As Integer, ByVal iUserNr As Integer) As String
			Return GetSelectedUserData(iMDNr, iUserNr).UserGuid
		End Function

		''' <summary>
		''' gets layoutname for devexpress layout which defined in setup
		''' </summary>
		''' <param name="iMDNr"> 0 for standard from registry</param>
		''' <param name="iUSNr"> 0 for standard from registry</param>
		''' <param name="strValuebyNull"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function GetSelectedUILayoutName(ByVal iMDNr As Integer, ByVal iUSNr As Integer, ByVal strValuebyNull As String) As String
			Dim strQuery As String = "//Layouts/Form_DevEx/FormStyle"
			Dim strStyleName As String = m_utilitiy.GetXMLValueByQueryWithFilename(GetSelectedMDUserProfileXMLFilename(iMDNr, iUSNr), strQuery, strValuebyNull)

			Return strStyleName
		End Function

		''' <summary>
		''' gets settingvalues from program.xml in MDxx\year\-Folder
		''' </summary>
		''' <param name="iMDNr"> 0 for standard from registry</param>
		''' <param name="iYear"></param>
		''' <param name="strSectionName"></param>
		''' <param name="strFieldName"></param>
		''' <param name="strValuebyNull"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function GetSelectedMDProfilValue(ByVal iMDNr As Integer, ByVal iYear As Integer, ByVal strSectionName As String, ByVal strFieldName As String, ByVal strValuebyNull As String) As String
			Dim bResult As String = String.Empty
			Dim strMDProfileName As String = GetSelectedMDDataXMLFilename(iMDNr, iYear)
			Dim strQuery As String = "//{0}/{1}"
			Dim strBez As String = String.Empty

			Try
				strQuery = String.Format(strQuery, strSectionName, strFieldName)
				strBez = m_utilitiy.GetXMLNodeValue(strMDProfileName, strQuery)
				If strBez = String.Empty Then strBez = strValuebyNull

			Catch ex As Exception
				logger.Error(ex.ToString)
			End Try

			Return strBez
		End Function

		''' <summary>
		''' sucht nach angegebenen Wert in der Benutzer.XML-Datei.
		''' </summary>
		''' <param name="strFieldName"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function GetLogedUSProfilValue(ByVal iMDNr As Integer, ByVal strFieldName As String, ByVal iUSNr As Integer) As String
			Dim bResult As String = String.Empty
			Dim strUserProfileName As String = GetSelectedMDUserProfileXMLFilename(iMDNr, iUSNr) ' .GetUserProfileFile()
			'Dim strQuery As String = String.Format("//User_" & iUSNr & "/USSetting/SettingName[@ID=" & Chr(34) & strFieldName & Chr(34) & "]/USValue"
			Dim strQuery As String = String.Format("//User_{1}/USSetting/SettingName[@ID={0}{2}{0}]/USValue", Chr(34), iUSNr, strFieldName)
			Dim strBez As String = m_utilitiy.GetXMLNodeValue(strUserProfileName, strQuery)

			Return strBez
		End Function

		Function GetSelectedCtlNameValue(ByVal iMDNr As Integer, ByVal strCtlName As String, ByVal strValuebyNull As String, ByVal strUSLang As String) As String
			Dim bResult As String = String.Empty
			Dim strMDProfileName As String = GetSelectedMDFormDataXMLFilename(iMDNr) ' GetFormDataFile()
			'    Dim strQuery As String = "//Control[@Name=" & Chr(34) & strCtlName & Chr(34) & "]/CtlLabel" & Me.GetUSLanguage  
			Dim strQuery As String = "//Control[@Name={0}{1}{0}]/CtlLabel{2}" '& Me.GetUSLanguage

			strQuery = String.Format(strQuery, Chr(34), strCtlName, strUSLang)
			Dim strBez As String = m_utilitiy.GetXMLNodeValue(strMDProfileName, strQuery)
			If strBez = String.Empty Then strBez = strValuebyNull

			Return strBez
		End Function

		Function GetSelectedMDMainViewXMLValue(ByVal iMDNr As Integer, ByVal strQuery As String, ByVal strValuebyNull As String) As String
			Dim bResult As String = String.Empty
			Dim strMDProfileName As String = GetSelectedMDMainViewXMLFilename(iMDNr) 'GetMainViewSettingFile()
			'Dim strQuery As String = "//Modul[@ID={0}{1}{0}]/CtlLabel{2}"

			'strQuery = String.Format(strQuery, Chr(34), strCtlName, Me.GetUSLanguage)
			Dim strBez As String = m_utilitiy.GetXMLNodeValue(strMDProfileName, strQuery)
			If strBez = String.Empty Then strBez = strValuebyNull

			Return strBez
		End Function



#Region "Pfad für Templates..."

		''' <summary>
		''' Dateinamen für diversen Templates:
		''' - Zeugnisdeckblatt
		''' - AGBDeckblatt
		''' - AGB4Temp
		''' - AGB4Fest
		''' </summary>
		''' <param name="iMDNr"></param>
		''' <param name="iYear"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function GetPath4Templates(ByVal iMDNr As Integer, ByVal iYear As Integer) As TemplateData
			Return New TemplateData With {.Path4ZeugnisDeckblatt = GetSelectedMDProfilValue(iMDNr, iYear, "Templates", "ZeugnisDeckblatt", ""),
												 .Path4AGBDeckblatt = GetSelectedMDProfilValue(iMDNr, iYear, "Templates", "AGBDeckblatt", ""),
												 .Path4AGBTemp = GetSelectedMDProfilValue(iMDNr, iYear, "Templates", "AGB4Temp", ""),
												 .Path4AGBFest = GetSelectedMDProfilValue(iMDNr, iYear, "Templates", "AGB4Fest", "")
												}
		End Function


#End Region


		''' <summary>
		''' Functions for WOS-access
		''' </summary>
		''' <param name="iMDNr"></param>
		''' <param name="iYear"></param>
		''' <returns></returns>
		''' <remarks></remarks>
#Region "WOS-Lizenzen..."

		Function GetWOSGuid(ByVal iMDNr As Integer, ByVal iYear As Integer) As WOSData
			Return New WOSData With {.WOSEmployeeGuid = GetSelectedMDProfilValue(iMDNr, iYear, "Export", "MA_SPUser_ID", ""),
															 .WOSCustomerGuid = GetSelectedMDProfilValue(iMDNr, iYear, "Export", "KD_SPUser_ID", ""),
															 .WOSVacancyGuid = GetSelectedMDProfilValue(iMDNr, iYear, "Export", "Vak_SPUser_ID", "")
															}
		End Function

		Function AllowedExportEmployee2WOS(ByVal iMDNr As Integer, ByVal iYear As Integer) As Boolean
			Return Me.GetWOSGuid(iMDNr, iYear).WOSEmployeeGuid.Length > 25
		End Function

		Function AllowedExportCustomer2WOS(ByVal iMDNr As Integer, ByVal iYear As Integer) As Boolean
			Return Me.GetWOSGuid(iMDNr, iYear).WOSCustomerGuid.Length > 25
		End Function

		Function AllowedExportVacancy2WOS(ByVal iMDNr As Integer, ByVal iYear As Integer) As Boolean
			Return Me.GetWOSGuid(iMDNr, iYear).WOSVacancyGuid.Length > 25
		End Function

#End Region

		Private ReadOnly Property EMailEnableSSL(ByVal iMDNr As Integer, ByVal iYear As Integer) As Boolean
			Get
				Dim m_MandantSettingsXml As SettingsXml
				m_MandantSettingsXml = New SettingsXml(GetSelectedMDDataXMLFilename(iMDNr, iYear))
				Dim m_MailingSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_MAILING_SETTING, iMDNr)

				Dim value As Boolean = ParseToBoolean(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/smtp-enablessl", m_MailingSetting)), False)

				Return value
			End Get
		End Property

		''' <summary>
		''' Daten für:
		''' - SMTP-Server
		''' - SMTP-Port
		''' - Davidserver
		''' - Fax-Server
		''' - Fax-Extension
		''' - Fax-Forwarder
		''' </summary>
		''' <param name="iMDNr"></param>
		''' <param name="iYear"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function GetMailingData(ByVal iMDNr As Integer, ByVal iYear As Integer) As ClsMailingData 'Implements iPath.ICommonSetting.GetMailingData
			Dim result As New ClsMailingData

			Try
				Dim xDoc As XDocument = XDocument.Load(GetSelectedMDDataXMLFilename(iMDNr, iYear))
				Dim ConfigQuery = (From exportSetting In xDoc.Root.Elements("Mailing")
													 Select New With {
															 .SmtpServer = m_utilitiy.GetSafeStringFromXElement(exportSetting.Element("SMTP-Server")),
															 .Smtpport = m_utilitiy.GetSafeStringFromXElement(exportSetting.Element("SMTP-Port")),
															 .faxDavidserver = m_utilitiy.GetSafeStringFromXElement(exportSetting.Element("faxdavidserver")),
															 .faxserver = m_utilitiy.GetSafeStringFromXElement(exportSetting.Element("fax-server")),
															 .faxextension = m_utilitiy.GetSafeStringFromXElement(exportSetting.Element("fax-extension")),
															 .faxforwarder = m_utilitiy.GetSafeStringFromXElement(exportSetting.Element("fax-forwarder"))
																 }).FirstOrDefault()

				result.SMTPServer = ConfigQuery.SmtpServer
				result.SMTPPort = CInt(Val(ConfigQuery.Smtpport))
				result.EnableSSL = EMailEnableSSL(iMDNr, iYear)

				result.FaxDavidServer = ConfigQuery.faxDavidserver

				result.FaxServer = ConfigQuery.faxserver
				result.FaxExtension = ConfigQuery.faxextension
				result.FaxForwarder = ConfigQuery.faxforwarder

			Catch ex As Exception
				logger.Error(ex.ToString)
				result.EnableSSL = False

			End Try

			Return result
		End Function

		''' <summary>
		''' Rechte für Moduls werden registriert. 
		''' </summary>
		''' <param name="strFieldname"></param>
		''' <returns>Boolean (True | False)</returns>
		''' <example>IsModulLicenceOK("sesam")</example>
		''' <remarks>
		''' Veraltet! Bitte SPUserSec.ClsUserSec.IsModulLicenceOK(strfieldname) benutzen.
		''' </remarks>
		Function IsModulLicenseOK(ByVal iMDNr As Integer, ByVal iYear As Integer, ByVal strFieldname As String) As Boolean 'Implements iPath.ICommonSetting.IsModulLicenceOK
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim result As New LicenseData
			Dim bresult As New Boolean

			Try
				Dim xDoc As XDocument = XDocument.Load(GetSelectedMDDataXMLFilename(iMDNr, iYear))
				Dim ConfigQuery = (From exportSetting In xDoc.Root.Elements("Licencing")
								   Select New With {
															 .sesam = m_utilitiy.GetSafeStringFromXElement(exportSetting.Element("sesam")),
															 .cresus = m_utilitiy.GetSafeStringFromXElement(exportSetting.Element("cresus")),
															 .abacus = m_utilitiy.GetSafeStringFromXElement(exportSetting.Element("abacus")),
															 .swifac = m_utilitiy.GetSafeStringFromXElement(exportSetting.Element("swifac")),
															 .comatic = m_utilitiy.GetSafeStringFromXElement(exportSetting.Element("comatic")),
															 .kmufactoring = m_utilitiy.GetSafeStringFromXElement(exportSetting.Element("kmufactoring")),
															 .csoplist = m_utilitiy.GetSafeStringFromXElement(exportSetting.Element("csoplist")),
															 .parifond = m_utilitiy.GetSafeStringFromXElement(exportSetting.Element("parifond"))
																 }).FirstOrDefault()

				result.sesam = ConfigQuery.sesam
				result.cresus = ConfigQuery.cresus
				result.abacus = ConfigQuery.abacus
				result.swifac = ConfigQuery.swifac

				result.comatic = ConfigQuery.comatic
				result.kmufactoring = ConfigQuery.kmufactoring
				result.csoplist = ConfigQuery.csoplist
				result.parifond = ConfigQuery.parifond

			Catch ex As Exception

			End Try


			Try

				Select Case strFieldname.ToLower
					Case "sesam"
						bresult = result.sesam = "+{172195F3-8C5A-41df-9018-6D3527CFD807}+"

					Case "cresus"
						bresult = result.cresus = "+{8BCF3299-F2C9-4EA6-873C-CD2E9B7317A4}+"

					Case "abacus"
						bresult = result.abacus = "+{B2705597-5217-4778-96F6-2998CCEF0598}+"

					Case "swifac"
						bresult = result.swifac = "+{AC1200CE-AE49-4f40-A28E-EA5891449595}+"

					Case "comatic"
						bresult = result.comatic = "+{4401D7E1-D512-420d-8822-2106456B33C0}+"

					Case "kmufactoring"
						bresult = result.kmufactoring = "+{799327A2-E69D-4021-AF83-072CA0468AAE}+"

					Case "csoplist"
						bresult = result.csoplist = "+{982E690D-39E4-4D8C-9028-01F2714E3A49}+"

					Case "parifond"
						bresult = result.parifond = "+{9E9ACBD2-37FE-4632-902A-F7252B04DCE8}+"

					Case Else
						Return False

				End Select

			Catch ex As Exception
				logger.Error(String.Format("{0}.{1}", strMethodeName, ex.Message))

			End Try

			Return bresult
		End Function

		''' <summary>
		''' Rechte für Moduls werden registriert. 
		''' </summary>
		''' <param name="strFieldname"></param>
		''' <returns>Boolean (True | False)</returns>
		''' <example>IsModulLicenceOK("sesam")</example>
		''' <remarks>
		''' Veraltet! Bitte SPUserSec.ClsUserSec.IsModulLicenceOK(strfieldname) benutzen.
		''' </remarks>
		Function ModulLicenseKeys(ByVal iMDNr As Integer, ByVal iYear As Integer, ByVal strFieldname As String) As LicenseData
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim result As New LicenseData
			Dim bresult As New Boolean

			Try
				Dim xDoc As XDocument = XDocument.Load(GetSelectedMDDataXMLFilename(iMDNr, iYear))
				Dim ConfigQuery = (From exportSetting In xDoc.Root.Elements("Licencing")
								   Select New With {
																							.sesam = m_utilitiy.GetSafeStringFromXElement(exportSetting.Element("sesam")),
																							.cresus = m_utilitiy.GetSafeStringFromXElement(exportSetting.Element("cresus")),
																							.abacus = m_utilitiy.GetSafeStringFromXElement(exportSetting.Element("abacus")),
																							.swifac = m_utilitiy.GetSafeStringFromXElement(exportSetting.Element("swifac")),
																							.comatic = m_utilitiy.GetSafeStringFromXElement(exportSetting.Element("comatic")),
																							.kmufactoring = m_utilitiy.GetSafeStringFromXElement(exportSetting.Element("kmufactoring")),
																							.csoplist = m_utilitiy.GetSafeStringFromXElement(exportSetting.Element("csoplist")),
																							.parifond = m_utilitiy.GetSafeStringFromXElement(exportSetting.Element("parifond")),
				.dvrefnr = m_utilitiy.GetSafeStringFromXElement(exportSetting.Element("customermng_deltavistaSolvencyCheckReferenceNumber")),
				.dvusername = m_utilitiy.GetSafeStringFromXElement(exportSetting.Element("customermng_deltavistaWebServiceUserName")),
				.dvuserpw = m_utilitiy.GetSafeStringFromXElement(exportSetting.Element("customermng_deltavistaWebServicePassword")),
				.dvurl = m_utilitiy.GetSafeStringFromXElement(exportSetting.Element("customermng_deltavistaWebServiceUrl")),
				.scandropin = m_utilitiy.GetSafeStringFromXElement(exportSetting.Element("scandropin")),
				.cvdropin = m_utilitiy.GetSafeStringFromXElement(exportSetting.Element("cvdropin")),
														 				.pmsearch = m_utilitiy.GetSafeStringFromXElement(exportSetting.Element("pmsearch"))
																								}).FirstOrDefault()

				result.sesam = ConfigQuery.sesam
				result.cresus = ConfigQuery.cresus
				result.abacus = ConfigQuery.abacus
				result.swifac = ConfigQuery.swifac

				result.comatic = ConfigQuery.comatic
				result.kmufactoring = ConfigQuery.kmufactoring
				result.csoplist = ConfigQuery.csoplist
				result.parifond = ConfigQuery.parifond

				result.dvrefnr = m_utilitiy.FromBase64(ConfigQuery.dvrefnr)
				result.dvusername = m_utilitiy.FromBase64(ConfigQuery.dvusername)
				result.dvuserpw = m_utilitiy.FromBase64(ConfigQuery.dvuserpw)
				result.dvurl = m_utilitiy.FromBase64(ConfigQuery.dvurl)

				result.ScanDropIN = m_utilitiy.ParseToBoolean(ConfigQuery.scandropin, False)
				result.CVDropIN = m_utilitiy.ParseToBoolean(ConfigQuery.cvdropin, False)
				result.PMSearch = m_utilitiy.ParseToBoolean(ConfigQuery.pmsearch, False)


			Catch ex As Exception
				logger.Error(ex.ToString)

			End Try


			Return result
		End Function

		Public Function GetPersonalizedCaptionInObject(ByVal iMDNr As Integer) As Dictionary(Of String, ClsProsonalizedData)
			Dim m_Progpath As New ClsProgPath
			Dim translationLookup As New Dictionary(Of String, ClsProsonalizedData)
			Dim xDoc As XDocument
			Dim xmlDoc As String = Me.GetSelectedMDFormDataXMLFilename(iMDNr)
			'Dim time1 As New Stopwatch
			'time1.Start()

			Try

				xDoc = XDocument.Load(xmlDoc)
				Dim lisOfControl = (From control In xDoc.Root.Elements("Control").ToList())

				For Each ctrl In lisOfControl

					Try
						Dim ctrolObject As New ClsProsonalizedData
						ctrolObject.CaptionKey = ctrl.Attribute("Name").Value
						ctrolObject.CaptionValue = ctrl.Element("CtlLabel").Value

						translationLookup.Add(ctrolObject.CaptionKey, ctrolObject)


					Catch ex As Exception
						Dim msg As String = String.Format("for each: {0} | Document: {1} | {2} -> {3}", iMDNr, xmlDoc, ctrl.Attribute("Name").Value, ctrl.Element("CtlLabel").Value)
						logger.Error(String.Format("{0} - {1}", msg, ex.ToString))
					End Try

				Next


				Return translationLookup

			Catch ex As Exception
				Dim msg As String = String.Format("loading document: {0}", xmlDoc)
				logger.Error(String.Format("{0} - {1}", msg, ex.ToString))
				Return Nothing

			Finally
				'time1.Stop()
				'Dim ts As TimeSpan = time1.Elapsed

				'' Format and display the TimeSpan value.
				'Dim elapsedTime As String = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10)
				'Console.WriteLine("RunTime " + elapsedTime)

			End Try

		End Function


		Private Function ParseToBoolean(ByVal stringvalue As String, ByVal value As Boolean?) As Boolean
			Dim result As Boolean
			If (Not Boolean.TryParse(stringvalue, result)) Then
				Return value
			End If
			Return result
		End Function


	End Class


End Namespace
