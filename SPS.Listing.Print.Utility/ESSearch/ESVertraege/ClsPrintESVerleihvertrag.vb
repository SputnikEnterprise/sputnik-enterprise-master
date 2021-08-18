
Imports System.IO

Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.DatabaseAccess.ES
Imports SP.DatabaseAccess.ES.DataObjects.ESMng

Imports SP.Infrastructure.Logging
Imports System.Reflection
Imports SPS.Listing.Print.Utility.ClsMainSetting
Imports SP.Internal.Automations.WOSUtility.DataObjects
Imports SP.Infrastructure

Imports SPProgUtility.CommonXmlUtility
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess.WOS

Namespace ESVerleih

	Public Class ClsPrintESVerleihvertrag


#Region "private consts"

		Private Const MANDANT_XML_SETTING_WOS_CUSTOMER_GUID As String = "MD_{0}/Export/KD_SPUser_ID"

#End Region


#Region "private fields"

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
		Private _ClsSetting As New ClsLLESVertragSetting

		''' <summary>
		''' The customer data access object.
		''' </summary>
		Private m_CustomerDatabaseAccess As ICustomerDatabaseAccess

		''' <summary>
		''' The employment data access object.
		''' </summary>
		Private m_ESDatabaseAccess As IESDatabaseAccess

		''' <summary>
		''' The wos data access object.
		''' </summary>
		Private m_WOSDatabaseAccess As WOSDatabaseAccess


		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As SP.Infrastructure.Utility

		''' <summary>
		''' Settings xml.
		''' </summary>
		Private m_MandantSettingsXml As SettingsXml

		''' <summary>
		''' The mandant.
		''' </summary>
		Private m_MandantData As Mandant
		Private m_PDFUtility As PDFUtilities.Utilities

		Private m_CustomerWOSID As String
		Private m_connectionString As String

		Private m_EmployeeGuid As String
		Private m_CustomerGuid As String
		Private m_ReportGuid As String
		Private m_EmployeeEmploymentGuid As String
		Private m_CustomerEmploymentGuid As String
		Private m_DocumentGuid As String
		Private m_CurrentEmployeeNumber As Integer?
		Private m_CurrentCustomerNumber As Integer?
		Private m_CurrentESNumber As Integer?
		Private m_CurrentReportNumber As Integer?


#End Region


#Region "Constructor"

		Public Sub New(ByVal _Setting As ClsLLESVertragSetting)
			Dim currentDomain As AppDomain = AppDomain.CurrentDomain

			m_Utility = New SP.Infrastructure.Utility
			m_MandantData = New Mandant

			_ClsSetting = _Setting
			AddHandler currentDomain.AssemblyResolve, AddressOf MyResolveEventHandler

			Dim m_init = CreateInitialData(_ClsSetting.SelectedMDNr, _ClsSetting.LogedUSNr)
			m_InitialData = m_init
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitialData.TranslationData, m_InitialData.ProsonalizedData)
			m_PDFUtility = New PDFUtilities.Utilities

			m_connectionString = m_InitialData.MDData.MDDbConn
			m_CustomerDatabaseAccess = New CustomerDatabaseAccess(m_connectionString, m_InitialData.UserData.UserLanguage)
			m_ESDatabaseAccess = New ESDatabaseAccess(m_connectionString, m_InitialData.UserData.UserLanguage)
			m_WOSDatabaseAccess = New WOSDatabaseAccess(m_connectionString, m_InitialData.UserData.UserLanguage)

			ClsMainSetting.MDData = m_init.MDData
			ClsMainSetting.UserData = m_init.UserData
			ClsMainSetting.ProgSettingData = New ClsSetting
			ClsMainSetting.ProgSettingData = New ClsSetting

			If String.IsNullOrWhiteSpace(_ClsSetting.ExportPath) Then
				_ClsSetting.ExportPath = _ClsSetting.GetExportPfad
			End If
			m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(m_InitialData.MDData.MDNr, Now.Year))
			m_CustomerWOSID = CustomerWOSID

			If String.IsNullOrWhiteSpace(_ClsSetting.ExportFinalFilename) Then
				_ClsSetting.ExportFinalFilename = _ClsSetting.GetExportFinalFilename(_ClsSetting.IsPrintAsVerleih)
			End If
			Me._ClsSetting.USSignFileName = MainUtilities.GetUSSign(Me._ClsSetting.DbConnString2Open)

		End Sub

#End Region


#Region "private properties"

		Private ReadOnly Property CustomerWOSID() As String
			Get
				Dim value = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_WOS_CUSTOMER_GUID, m_InitialData.MDData.MDNr))

				Return value
			End Get
		End Property

#End Region

		Function PrintESVerleih(ByVal bAsDesign As Boolean) As String
			Dim strResult As String = String.Empty
			Dim strWOSResult As WOSSendResult = New WOSSendResult With {.Value = False, .Message = "Initialisierung..."}
			Dim liESNr As New List(Of Integer)
			Dim liMANr As New List(Of Integer)
			Dim liKDNr As New List(Of Integer)
			Dim liKDZHDNr As New List(Of Integer)
			Dim liMAWOS As New List(Of Boolean)
			Dim liKDWOS As New List(Of Boolean)
			Dim liMALang As New List(Of String)
			Dim liKDLang As New List(Of String)

			_ClsSetting.ListOfExportedFilesVerleih.Clear()
			Try
				liESNr = _ClsSetting.liESNr2Print
				liMANr = _ClsSetting.liMANr2Print
				liKDNr = _ClsSetting.liKDNr2Print
				liKDZHDNr = _ClsSetting.liKDZHDNr2Print
				liMAWOS = _ClsSetting.liSendESMAData2WOS
				liKDWOS = _ClsSetting.liSendESKDData2WOS
				liMALang = _ClsSetting.LiMALang
				liKDLang = _ClsSetting.LiKDLang
				For i As Integer = 0 To liESNr.Count - 1
					_ClsSetting.SelectedESNr2Print = liESNr(i)
					_ClsSetting.SelectedMANr2Print = liMANr(i)
					_ClsSetting.SelectedKDNr2Print = liKDNr(i)
					_ClsSetting.SelectedKDZHDNr2Print = liKDZHDNr(i)
					_ClsSetting.SendMAData2WOS = liMAWOS(i)   ' Der Kandidat ist WOS-pflichtig
					_ClsSetting.SendKDData2WOS = If(Not String.IsNullOrWhiteSpace(m_CustomerWOSID), liKDWOS(i), False)   ' Der Kunde ist WOS-pflichtig
					_ClsSetting.SelectedMALang = If(liMALang(i) = "D", String.Empty, liMALang(i))
					_ClsSetting.SelectedKDLang = If(liKDLang(i) = "D", String.Empty, liKDLang(i))

					_ClsSetting.SelectedLang = _ClsSetting.SelectedKDLang
					Dim _clsPrintVerleih As New ClsLLVerleihPrint(_ClsSetting)
					If bAsDesign Then
						_clsPrintVerleih.ShowInDesign()
						_clsPrintVerleih.Dispose()

						Return String.Empty
					End If

					If _ClsSetting.SendAndPrintData2WOS OrElse Not _ClsSetting.SendKDData2WOS Then strResult = _clsPrintVerleih.ShowInPrint(i = 0)
					_clsPrintVerleih.Dispose()
					If strResult.ToLower.Contains("error".ToLower) Then Return strResult

					If _ClsSetting.SendKDData2WOS AndAlso Not String.IsNullOrWhiteSpace(m_CustomerWOSID) Then
						m_Logger.LogDebug(String.Format("Wird exportiert und anschliessend ins WOS gestellt..."))

						Dim _clsESWOSExport As New ClsLLVerleihPrint(_ClsSetting)
						Dim exportResult As Boolean = _clsESWOSExport.ExportLLDoc()
						_clsESWOSExport.Dispose()
						Dim strMsg As String = String.Format(MainUtilities.TranslateMyText("WOS-Error: Ihre Dokumente konnten nicht übermittelt werden.{0}{1}"), vbNewLine, strWOSResult)
						If Not exportResult OrElse _ClsSetting.ListOfExportedFilesVerleih.Count = 0 Then Throw New Exception(strMsg)
						strWOSResult = TransferCustomerVerleihIntoWOS() ' SendSelectedKDDoc2WOS()

					End If

				Next
				'If _ClsProgSetting.bAllowedKDDocTransferTo_WS And strWOSResult.ToLower.Contains("wos-success".ToLower) Then strWOSResult = SendFinalSelectedDoc2WOS()
				If strWOSResult.Value Then strResult = "WOS-Success"

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))
				strResult = ex.ToString

			End Try

			Return strResult
		End Function



		Function ExportESVerleihvertrag() As String
			Dim strResult As String = String.Empty
			Dim liESNr As New List(Of Integer)
			Dim liMANr As New List(Of Integer)
			Dim liKDNr As New List(Of Integer)
			Dim liKDZHDNr As New List(Of Integer)
			Dim liMAWOS As New List(Of Boolean)
			Dim liKDWOS As New List(Of Boolean)
			Dim liMALang As New List(Of String)
			Dim liKDLang As New List(Of String)
			Dim success As Boolean = True

			_ClsSetting.ListOfExportedFilesVerleih.Clear()
			Try

				liESNr = _ClsSetting.liESNr2Print
				liMANr = _ClsSetting.liMANr2Print
				liKDNr = _ClsSetting.liKDNr2Print
				liKDZHDNr = _ClsSetting.liKDZHDNr2Print
				liMAWOS = _ClsSetting.liSendESMAData2WOS
				liKDWOS = _ClsSetting.liSendESKDData2WOS
				liMALang = _ClsSetting.LiMALang
				liKDLang = _ClsSetting.LiKDLang

				For i As Integer = 0 To liESNr.Count - 1
					_ClsSetting.SelectedESNr2Print = liESNr(i)
					_ClsSetting.SelectedMANr2Print = liMANr(i)
					_ClsSetting.SelectedKDNr2Print = liKDNr(i)
					_ClsSetting.SelectedKDZHDNr2Print = liKDZHDNr(i)
					_ClsSetting.SendMAData2WOS = liMAWOS(i)     ' Der Kandidat ist WOS-pflichtig
					_ClsSetting.SendKDData2WOS = liKDWOS(i)     ' Der Kunde ist WOS-pflichtig
					_ClsSetting.SelectedMALang = If(liMALang(i) = "D", String.Empty, liMALang(i))
					_ClsSetting.SelectedKDLang = If(liKDLang(i) = "D", String.Empty, liKDLang(i))

					_ClsSetting.SelectedLang = _ClsSetting.SelectedKDLang
					Dim _clsESPrint As New ClsLLVerleihPrint(_ClsSetting)
					success = success AndAlso _clsESPrint.ExportLLDoc()

					_clsESPrint.Dispose()
					If Not success Then
						strResult = m_Translate.GetSafeTranslationValue("Export war nicht erfolgreich!")
						Exit For
					End If
				Next
				If Not success Then Return strResult

				Try
					If _ClsSetting.ListOfExportedFilesVerleih.Count > 0 Then
						'_ClsSetting.ExportFinalFilename = String.Format(_ClsSetting.ExportFinalFilename, _ClsSetting.SelectedMonth, _ClsSetting.SelectedYear)
						'Dim strExportPfad As String = _ClsSetting.GetExportPfad
						'If Not Directory.Exists(strExportPfad) Then strExportPfad = _ClsProgSetting.GetSpSESTempPath
						'Dim strFinalFilename As String = String.Format("{0}{1}", strExportPfad, _ClsSetting.ExportFinalFilename)

						Dim strExportPfad As String = m_InitialData.UserData.spTempEmplymentPath
						If Not Directory.Exists(strExportPfad) Then Directory.CreateDirectory(m_InitialData.UserData.spTempEmplymentPath)

						Dim initialFilename As String = "Einsatzvertrag_"

						If _ClsSetting.IsPrintAsVerleih Then initialFilename = "Verleihvertrag_"

						Dim fileName = String.Format("{0}{1}_{2}_{3}.pdf", initialFilename, m_InitialData.MDData.MDName, _ClsSetting.SelectedMonth, _ClsSetting.SelectedYear)

						If File.Exists(Path.Combine(strExportPfad, fileName)) Then
							Try
								File.Delete(Path.Combine(strExportPfad, fileName))
							Catch ex As Exception
								fileName = String.Format("{0}{1}_{2}_{3}_{4}.pdf", initialFilename, m_InitialData.MDData.MDName, Environment.TickCount, _ClsSetting.SelectedMonth, _ClsSetting.SelectedYear)
							End Try
						End If
						Dim strFinalFilename As String = Path.Combine(strExportPfad, fileName)

						strResult = String.Empty
						If _ClsSetting.ListOfExportedFilesVerleih.Count = 1 Then
							Try
								File.Copy(_ClsSetting.ListOfExportedFilesVerleih(0), strFinalFilename, True)
								strResult = strFinalFilename

							Catch ex As Exception
								strResult = ("Fehler beim Kopieren der Datei!")
								m_Logger.LogError(String.Format("Datei kopieren: {0}", ex.ToString))
								Return strResult
							End Try

						Else
							'Dim obj As New SP.PDFO2S.ClsPDF4Net
							'strResult = obj.Merg2PDFFiles(strFinalFilename, _ClsSetting.ListOfExportedFilesVerleih.ToArray)
							'strResult = strFinalFilename

							Dim mergePDF = m_PDFUtility.MergePdfFiles(_ClsSetting.ListOfExportedFilesVerleih.ToArray, strFinalFilename)
							If mergePDF Then
								strResult = strFinalFilename
							Else
								m_Logger.LogError("merging pdf file was not successfull!")
								strResult = String.Empty
							End If

						End If

					End If
					_ClsSetting.ExportFinalFilename = strResult


				Catch ex As Exception
					strResult = ("Fehler beim zusammenstellen der Dateien!")
					m_Logger.LogError(String.Format("MergePDF: {0}", ex.ToString))
					Return strResult
				End Try

			Catch ex As Exception
				strResult = String.Format("Fehler: {0}", ex.ToString)
				m_Logger.LogError(String.Format("{0}", ex.ToString))
				Return strResult
			End Try

			Return strResult
		End Function

		Private Function TransferCustomerVerleihIntoWOS() As WOSSendResult
			Dim result As WOSSendResult = New WOSSendResult With {.Value = False, .Message = "Initialisierung..."}
			Dim pdfFileName As String = _ClsSetting.ListOfExportedFilesVerleih(0)

			Dim fileByte() = m_Utility.LoadFileBytes(pdfFileName)
			result.Value = TransferCustomerEmploymentDataToWOS(_ClsSetting.SelectedESNr2Print, fileByte)
			If result.Value Then result.Message = "Success"


			Return result

		End Function



#Region "Helpers"

		Private Function TransferCustomerEmploymentDataToWOS(ByVal esNumber As Integer, ByVal file2Transfer As Byte()) As Boolean
			Dim result As Boolean = True
			Dim cResponsibleNumber As Integer = 0
			Dim responsiblePersonData As ResponsiblePersonMasterData = Nothing
			Dim responblePersonGuid As String = String.Empty

			If String.IsNullOrWhiteSpace(m_CustomerWOSID) Then Return False

			Dim esData = m_ESDatabaseAccess.LoadESMasterData(esNumber)
			Dim esSalaryDataList As List(Of ESSalaryData) = m_ESDatabaseAccess.LoadESSalaryData(esNumber)
			If esData Is Nothing OrElse esSalaryDataList Is Nothing Then
				m_Logger.LogDebug(String.Format("Einsatz mit der Nummer {0} wurde nicht gefunden.", esNumber))

				Return False
			End If

			Dim salaryDta As ESSalaryData = esSalaryDataList.Where(Function(x) x.AktivLODaten = True).FirstOrDefault
			If salaryDta Is Nothing Then
				m_Logger.LogError("keine Einsatzlohn Daten wurden gefunden.")

				Return False
			End If

			m_CurrentEmployeeNumber = salaryDta.EmployeeNumber
			m_CurrentCustomerNumber = salaryDta.CustomerNumber
			m_CurrentESNumber = esNumber
			cResponsibleNumber = esData.KDZHDNr.GetValueOrDefault(0)
			m_DocumentGuid = salaryDta.VerleihDoc_Guid

			Dim customerData As CustomerMasterData = m_CustomerDatabaseAccess.LoadCustomerMasterData(m_CurrentCustomerNumber, m_InitialData.UserData.UserFiliale)
			Dim customereMailData As List(Of CustomerAssignedEmailData) = m_CustomerDatabaseAccess.LoadAssignedEmailsOfCustomer(m_CurrentCustomerNumber)
			If cResponsibleNumber > 0 Then
				responsiblePersonData = m_CustomerDatabaseAccess.LoadResponsiblePersonMasterData(m_CurrentCustomerNumber, cResponsibleNumber)
			End If

			If customerData Is Nothing OrElse customereMailData Is Nothing Then
				m_Logger.LogDebug(String.Format("Kundendaten {0} mit Einsatznummer {1} wurde nicht gefunden.", m_CurrentCustomerNumber, m_CurrentESNumber))

				Return False
			End If
			If Not customerData.sendToWOS OrElse customereMailData.Count = 0 Then
				m_Logger.LogDebug(String.Format("Der Kunde {0} ist nicht WOS-pflichtig!", m_CurrentCustomerNumber))

				Return False
			End If

			m_CustomerGuid = customerData.Transfered_Guid
			If String.IsNullOrWhiteSpace(m_CustomerGuid) Then
				Dim newGuid As String = Guid.NewGuid.ToString
				result = result AndAlso m_WOSDatabaseAccess.UpdateCustomerGuidData(m_CurrentCustomerNumber, newGuid)

				If result Then m_CustomerGuid = newGuid
			End If

			If Not responsiblePersonData Is Nothing AndAlso cResponsibleNumber > 0 Then
				responblePersonGuid = responsiblePersonData.TransferedGuid
				If String.IsNullOrWhiteSpace(responblePersonGuid) Then
					Dim newGuid As String = Guid.NewGuid.ToString
					result = result AndAlso m_WOSDatabaseAccess.UpdateCustomerResponsibleGuidData(m_CurrentCustomerNumber, cResponsibleNumber, newGuid)

					If result Then responblePersonGuid = newGuid
				End If
			End If

			If String.IsNullOrWhiteSpace(m_DocumentGuid) Then
				Dim newGuid As String = Guid.NewGuid.ToString
				result = result AndAlso m_WOSDatabaseAccess.UpdateCustomerEmploymentGuidData(esNumber, newGuid)

				If result Then m_DocumentGuid = newGuid
			End If

			Dim _setting As New SP.Internal.Automations.WOSUtility.DataObjects.WOSSendSetting
			_setting.DocumentArtEnum = WOSSendSetting.DocumentArt.Verleihvertrag
			_setting.DocumentInfo = String.Format("Verleihvertrag: {0}", esNumber)
			_setting.EmployeeGuid = String.Empty
			_setting.CustomerGuid = m_CustomerGuid
			_setting.CresponsibleGuid = responblePersonGuid

			_setting.EmployeeNumber = m_CurrentEmployeeNumber
			_setting.CustomerNumber = m_CurrentCustomerNumber
			_setting.CresponsibleNumber = cResponsibleNumber
			_setting.EmploymentNumber = m_CurrentESNumber

			_setting.AssignedDocumentGuid = m_DocumentGuid
			_setting.SignTransferedDocument = False
			_setting.ScanDoc = file2Transfer
			_setting.ScanDocName = String.Empty


			Dim obj As New SP.Internal.Automations.WOSUtility.CustomerExport(m_InitialData)
			obj.WOSSetting = _setting
			Dim sendResult = obj.TransferCustomerDocumentDataToWOS(True)

			result = sendResult.Value

			Return result
		End Function

		Function MyResolveEventHandler(sender As Object, args As ResolveEventArgs) As Assembly
			'This handler is called only when the common language runtime tries to bind to the assembly and fails.        
			m_Logger = New Logger

			'Retrieve the list of referenced assemblies in an array of AssemblyName.
			Dim objExecutingAssemblies As [Assembly]
			objExecutingAssemblies = [Assembly].GetExecutingAssembly()
			Dim arrReferencedAssmbNames() As AssemblyName
			arrReferencedAssmbNames = objExecutingAssemblies.GetReferencedAssemblies()

			'Loop through the array of referenced assembly names.
			Dim strAssmbName As AssemblyName
			For Each strAssmbName In arrReferencedAssmbNames

				'Look for the assembly names that have raised the "AssemblyResolve" event.
				If (strAssmbName.FullName.Substring(0, strAssmbName.FullName.IndexOf(",")) = args.Name.Substring(0, args.Name.IndexOf(","))) Then

					'Build the path of the assembly from where it has to be loaded.
					Dim strTempAssmbPath As String = String.Empty
					strTempAssmbPath = IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase), args.Name.Substring(0, args.Name.IndexOf(",")) & ".dll")
					If IO.File.Exists(strTempAssmbPath) Then
						Dim msg = String.Format("loading Assembly: {0}", strTempAssmbPath)
						m_Logger.LogWarning(msg)
						Trace.WriteLine(String.Format("loading Assembly: ", strTempAssmbPath))
						Dim MyAssembly As [Assembly]

						'Load the assembly from the specified path. 
						MyAssembly = [Assembly].LoadFrom(strTempAssmbPath)

						'Return the loaded assembly.
						Return MyAssembly
					Else
						Dim msg = String.Format("Assembly could not be found: {0}", strTempAssmbPath)
						m_Logger.LogWarning(msg)
						Trace.WriteLine(msg)
						'Return Nothing
					End If

				End If
			Next


			Return Nothing

		End Function

		Private Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

			Dim m_md As New SPProgUtility.Mandanten.Mandant
			Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
			Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
			Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

			Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
			Dim translate = clsTransalation.GetTranslationInObject

			Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

		End Function


#End Region

	End Class

End Namespace



