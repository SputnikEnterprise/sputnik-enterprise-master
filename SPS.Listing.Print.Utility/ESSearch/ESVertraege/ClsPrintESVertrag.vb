
Imports System.IO
Imports System.IO.File
Imports SPProgUtility.ClsProgSettingPath

Imports SP.Infrastructure.Logging
Imports System.Reflection
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.DatabaseAccess.ES
Imports SP.DatabaseAccess.ES.DataObjects.ESMng

Imports SPS.Listing.Print.Utility.ClsMainSetting
Imports SPS.Listing.Print.Utility.MainUtilities.Utilities
Imports SP.Internal.Automations.WOSUtility.DataObjects
Imports SP.DatabaseAccess.WOS
Imports SPProgUtility.Mandanten
Imports SPProgUtility.CommonXmlUtility
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports System.Drawing.Printing
Imports SP.Infrastructure.UI
Imports SP.Infrastructure

Namespace ESVertrag



	''' <summary>
	''' ist für Druck und Exprot der Einsatzverträge zuständig
	''' </summary>
	''' <remarks></remarks>
	Public Class ClsPrintESVertrag


#Region "private consts"

		Private Const MANDANT_XML_SETTING_WOS_EMPLOYEE_GUID As String = "MD_{0}/Export/MA_SPUser_ID"

#End Region


#Region "privae fields"

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
		''' The employee data access object.
		''' </summary>
		Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

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
		Private m_UtilityUI As UtilityUI

		''' <summary>
		''' The mandant.
		''' </summary>
		Private m_MandantData As Mandant

		Private m_EmployeeWOSID As String
		Private m_connectionString As String
		Private m_PDFUtility As PDFUtilities.Utilities

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

			_ClsSetting = _Setting
			AddHandler currentDomain.AssemblyResolve, AddressOf MyResolveEventHandler

			Dim m_init = CreateInitialData(_ClsSetting.SelectedMDNr, _ClsSetting.LogedUSNr)
			m_InitialData = m_init
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitialData.TranslationData, m_InitialData.ProsonalizedData)
			m_Utility = New SP.Infrastructure.Utility
			m_UtilityUI = New UtilityUI
			m_MandantData = New Mandant
			m_PDFUtility = New PDFUtilities.Utilities

			m_connectionString = m_InitialData.MDData.MDDbConn
			m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(m_connectionString, m_InitialData.UserData.UserLanguage)
			m_CustomerDatabaseAccess = New CustomerDatabaseAccess(m_connectionString, m_InitialData.UserData.UserLanguage)
			m_ESDatabaseAccess = New ESDatabaseAccess(m_connectionString, m_InitialData.UserData.UserLanguage)
			m_WOSDatabaseAccess = New WOSDatabaseAccess(m_connectionString, m_InitialData.UserData.UserLanguage)

			ClsMainSetting.MDData = m_InitialData.MDData
			ClsMainSetting.UserData = m_InitialData.UserData
			ClsMainSetting.TranslationData = m_InitialData.TranslationData
			ClsMainSetting.PerosonalizedData = m_InitialData.ProsonalizedData

			ClsMainSetting.ProgSettingData = New ClsSetting

			If String.IsNullOrWhiteSpace(_ClsSetting.ExportPath) Then
				_ClsSetting.ExportPath = _ClsSetting.GetExportPfad
			End If

			If String.IsNullOrWhiteSpace(_ClsSetting.ExportFinalFilename) Then
				_ClsSetting.ExportFinalFilename = _ClsSetting.GetExportFinalFilename(_ClsSetting.IsPrintAsVerleih)
			End If
			Me._ClsSetting.USSignFileName = GetUSSign(Me._ClsSetting.DbConnString2Open)

			m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(m_InitialData.MDData.MDNr, Now.Year))
			m_EmployeeWOSID = EmployeeWOSID

		End Sub

		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass, ByVal _Set As ClsLLESVertragSetting)
			Dim currentDomain As AppDomain = AppDomain.CurrentDomain

			AddHandler currentDomain.AssemblyResolve, AddressOf MyResolveEventHandler

			_ClsSetting = _Set
			m_InitialData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitialData.TranslationData, m_InitialData.ProsonalizedData)
			m_Utility = New SP.Infrastructure.Utility
			m_UtilityUI = New UtilityUI
			m_MandantData = New Mandant
			m_PDFUtility = New PDFUtilities.Utilities

			ClsMainSetting.MDData = m_InitialData.MDData
			ClsMainSetting.UserData = m_InitialData.UserData
			ClsMainSetting.TranslationData = m_InitialData.TranslationData
			ClsMainSetting.PerosonalizedData = m_InitialData.ProsonalizedData

			ClsMainSetting.ProgSettingData = New ClsSetting
			If String.IsNullOrWhiteSpace(_ClsSetting.ExportPath) Then
				_ClsSetting.ExportPath = _ClsSetting.GetExportPfad
			End If

			If String.IsNullOrWhiteSpace(_ClsSetting.ExportFinalFilename) Then
				_ClsSetting.ExportFinalFilename = _ClsSetting.GetExportFinalFilename(_ClsSetting.IsPrintAsVerleih)
			End If
			Me._ClsSetting.USSignFileName = GetUSSign(Me._ClsSetting.DbConnString2Open)

			m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(m_InitialData.MDData.MDNr, Now.Year))
			m_EmployeeWOSID = EmployeeWOSID

		End Sub


#End Region

#Region "private properties"

		Private ReadOnly Property EmployeeWOSID() As String
			Get
				Dim value = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_WOS_EMPLOYEE_GUID, m_InitialData.MDData.MDNr))

				Return value
			End Get
		End Property

#End Region


		''' <summary>
		''' Druckt und exportiert die Einsatzverträge
		''' </summary>
		''' <param name="bAsDesign"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function PrintESVertrag(ByVal bAsDesign As Boolean) As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strResult As String = String.Empty
			Dim strWOSResult As WOSSendResult = New WOSSendResult With {.Value = False, .Message = "Initialisierung..."}
			Dim liESNr As New List(Of Integer)
			Dim liMANr As New List(Of Integer)
			Dim liKDNr As New List(Of Integer)
			Dim liMAWOS As New List(Of Boolean)
			Dim liKDWOS As New List(Of Boolean)
			Dim liMALang As New List(Of String)
			Dim liKDLang As New List(Of String)

			Dim settings = New PrinterSettings()
			If settings.PrinterName Is Nothing OrElse String.IsNullOrWhiteSpace(settings.PrinterName) Then
				m_Logger.LogError("no default printer is defined.")
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Sie haben keinen Standard-Drucker definiert."))

				Return String.Empty
			Else
				m_Logger.LogInfo(String.Format("default printer: {0}", settings.PrinterName))
			End If

			_ClsSetting.ListOfExportedFilesESVertrag.Clear()
			Try
				liESNr = _ClsSetting.liESNr2Print
				liMANr = _ClsSetting.liMANr2Print
				liKDNr = _ClsSetting.liKDNr2Print
				liMAWOS = _ClsSetting.liSendESMAData2WOS
				liKDWOS = _ClsSetting.liSendESKDData2WOS
				liMALang = _ClsSetting.LiMALang
				liKDLang = _ClsSetting.LiKDLang
				For i As Integer = 0 To liESNr.Count - 1
					_ClsSetting.SelectedESNr2Print = liESNr(i)
					_ClsSetting.SelectedMANr2Print = liMANr(i)
					_ClsSetting.SelectedKDNr2Print = liKDNr(i)
					_ClsSetting.SendMAData2WOS = If(Not String.IsNullOrWhiteSpace(m_EmployeeWOSID), liMAWOS(i), False)   ' Der Kandidat ist WOS-pflichtig
					_ClsSetting.SendKDData2WOS = liKDWOS(i)   ' Der Kunde ist WOS-pflichtig
					_ClsSetting.SelectedMALang = If(liMALang(i) = "D", String.Empty, liMALang(i))
					_ClsSetting.SelectedKDLang = If(liKDLang(i) = "D", String.Empty, liKDLang(i))

					_ClsSetting.SelectedLang = _ClsSetting.SelectedMALang
					Dim _clsESPrint As New ClsLLESVertragPrint(_ClsSetting)
					If bAsDesign Then
						_clsESPrint.ShowInDesign()
						_clsESPrint.Dispose()

						Return "success..."
					End If

					If _ClsSetting.SendAndPrintData2WOS Or Not _ClsSetting.SendMAData2WOS Then strResult = _clsESPrint.ShowInPrint(i = 0)
					_clsESPrint.Dispose()
					If strResult.ToLower.Contains("error".ToLower) Then Return strResult

					If _ClsSetting.SendMAData2WOS AndAlso Not String.IsNullOrWhiteSpace(m_EmployeeWOSID) Then
						Dim _clsESWOSExport As New ClsLLESVertragPrint(_ClsSetting)
						Dim exportResult = _clsESWOSExport.ExportLLDoc()
						_clsESWOSExport.Dispose()

						Dim strMsg As String = String.Format(MainUtilities.TranslateMyText("WOS-Error: Ihre Dokumente konnten nicht übermittelt werden.{0}{1}"), vbNewLine, strWOSResult)
						If Not exportResult OrElse _ClsSetting.ListOfExportedFilesESVertrag.Count = 0 Then Throw New Exception(strMsg)
						strWOSResult = TransferCustomerVerleihIntoWOS() ' SendSelectedKDDoc2WOS()

					End If

				Next
				If strWOSResult.Value Then strResult = "WOS-Success"
				'If _ClsProgSetting.bAllowedMADocTransferTo_WS And strWOSResult.ToLower.Contains("wos-success") Then strWOSResult = SendFinalSelectedDoc2WOS()
				'If strWOSResult.ToLower.Contains("success".ToLower) Then strResult = "WOS-Success"


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				strResult = ex.ToString

			End Try

			Return strResult
		End Function

		Function ExportESVertrag() As String
			Dim strResult As String = String.Empty
			Dim liESNr As New List(Of Integer)
			Dim liMANr As New List(Of Integer)
			Dim liKDNr As New List(Of Integer)
			Dim liMAWOS As New List(Of Boolean)
			Dim liKDWOS As New List(Of Boolean)
			Dim liMALang As New List(Of String)
			Dim liKDLang As New List(Of String)
			Dim success As Boolean = True

			Dim settings = New PrinterSettings()
			If settings.PrinterName Is Nothing OrElse String.IsNullOrWhiteSpace(settings.PrinterName) Then
				m_Logger.LogError("no default printer is defined.")
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Sie haben keinen Standard-Drucker definiert."))

				Return String.Empty
			Else
				m_Logger.LogInfo(String.Format("default printer: {0}", settings.PrinterName))
			End If

			_ClsSetting.ListOfExportedFilesESVertrag.Clear()
			Try

				liESNr = _ClsSetting.liESNr2Print
				liMANr = _ClsSetting.liMANr2Print
				liKDNr = _ClsSetting.liKDNr2Print
				liMAWOS = _ClsSetting.liSendESMAData2WOS
				liKDWOS = _ClsSetting.liSendESKDData2WOS
				liMALang = _ClsSetting.LiMALang
				liKDLang = _ClsSetting.LiKDLang
				For i As Integer = 0 To liESNr.Count - 1
					_ClsSetting.SelectedESNr2Print = liESNr(i)
					_ClsSetting.SelectedMANr2Print = liMANr(i)
					_ClsSetting.SelectedKDNr2Print = liKDNr(i)
					_ClsSetting.SendMAData2WOS = liMAWOS(i)   ' Der Kandidat ist WOS-pflichtig
					_ClsSetting.SendKDData2WOS = liKDWOS(i)   ' Der Kunde ist WOS-pflichtig
					_ClsSetting.SelectedMALang = If(liMALang(i) = "D", String.Empty, liMALang(i))
					_ClsSetting.SelectedKDLang = If(liKDLang(i) = "D", String.Empty, liKDLang(i))

					_ClsSetting.SelectedLang = _ClsSetting.SelectedMALang

					Dim _clsESPrint As New ClsLLESVertragPrint(_ClsSetting)
					success = success AndAlso _clsESPrint.ExportLLDoc()
					_clsESPrint.Dispose()

					If Not success Then
						strResult = m_Translate.GetSafeTranslationValue("Export war nicht erfolgreich!")
						Exit For
					End If
				Next
				If Not success Then Return strResult

				Try
					If _ClsSetting.ListOfExportedFilesESVertrag.Count > 0 Then

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
						If _ClsSetting.ListOfExportedFilesESVertrag.Count = 1 Then
							Try
								File.Copy(_ClsSetting.ListOfExportedFilesESVertrag(0), strFinalFilename, True)
								strResult = strFinalFilename

							Catch ex As Exception
								strResult = ("Fehler beim Kopieren der Datei!")
								m_Logger.LogError(String.Format("Datei kopieren: {0}", ex.ToString))
								Return strResult
							End Try

						Else
							Dim mergePDF = m_PDFUtility.MergePdfFiles(_ClsSetting.ListOfExportedFilesESVertrag.ToArray, strFinalFilename)
							If mergePDF Then
								strResult = strFinalFilename
							Else
								m_Logger.LogError("merging pdf file was not successfull!")
								strResult = String.Empty
							End If

						End If

						'For Each fileitem In _ClsSetting.ListOfExportedFilesESVertrag
						'	Try
						'		'	File.Delete(fileitem)
						'	Catch ex As Exception

						'	End Try
						'Next
						_ClsSetting.ExportFinalFilename = strResult

					End If


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
			Dim pdfFileName As String = _ClsSetting.ListOfExportedFilesESVertrag(0)

			Dim fileByte() = m_Utility.LoadFileBytes(pdfFileName)
			result.Value = TransferEmployeeEmploymentDataToWOS(_ClsSetting.SelectedESNr2Print, fileByte)
			If result.Value Then result.Message = "Success"


			Return result

		End Function


#Region "Helpers"

		Public Function TransferEmployeeEmploymentDataToWOS(ByVal esNumber As Integer, ByVal file2Transfer As Byte()) As Boolean
			Dim result As Boolean = True
			If String.IsNullOrWhiteSpace(m_EmployeeWOSID) Then Return False

			Dim esSalaryDataList As List(Of ESSalaryData) = m_ESDatabaseAccess.LoadESSalaryData(esNumber)
			If esSalaryDataList Is Nothing Then
				m_Logger.LogDebug(String.Format("Einsatz mit der Nummer {0} wurde nicht gefunden.", esNumber))

				Return False
			End If

			Dim salaryDta As ESSalaryData = esSalaryDataList.Where(Function(x) x.AktivLODaten = True).FirstOrDefault
			If salaryDta Is Nothing Then
				m_Logger.LogError("keine Einsatzlohn Daten wurden gefunden.")

				Return False
			End If

			m_CurrentEmployeeNumber = salaryDta.EmployeeNumber
			m_CurrentESNumber = esNumber
			m_DocumentGuid = salaryDta.ESDoc_Guid
			Dim employeeData As EmployeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(m_CurrentEmployeeNumber, False)

			If Not employeeData.Send2WOS OrElse String.IsNullOrWhiteSpace(employeeData.Email) Then Return False

			m_EmployeeGuid = employeeData.Transfered_Guid

			If String.IsNullOrWhiteSpace(m_EmployeeGuid) Then
				Dim newGuid As String = Guid.NewGuid.ToString
				result = result AndAlso m_WOSDatabaseAccess.UpdateEmployeeGuidData(m_CurrentEmployeeNumber, newGuid)

				If result Then m_EmployeeGuid = newGuid
			End If

			If String.IsNullOrWhiteSpace(m_DocumentGuid) Then
				Dim newGuid As String = Guid.NewGuid.ToString
				result = result AndAlso m_WOSDatabaseAccess.UpdateEmploymentGuidData(esNumber, newGuid)

				If result Then m_DocumentGuid = newGuid
			End If

			Dim _setting As New SP.Internal.Automations.WOSUtility.DataObjects.WOSSendSetting
			_setting.DocumentArtEnum = WOSSendSetting.DocumentArt.Einsatzvertrag
			_setting.DocumentInfo = String.Format("Einsatzvertrag: {0}", esNumber)
			_setting.EmployeeGuid = m_EmployeeGuid
			_setting.EmployeeNumber = m_CurrentEmployeeNumber
			_setting.EmploymentNumber = esNumber

			_setting.AssignedDocumentGuid = m_DocumentGuid
			_setting.SignTransferedDocument = False
			_setting.ScanDoc = file2Transfer
			_setting.ScanDocName = String.Empty


			Dim obj As New SP.Internal.Automations.WOSUtility.EmployeeExport(m_InitialData)
			obj.WOSSetting = _setting
			Dim sendResult = obj.TransferEmployeeDocumentDataToWOS(True)

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


