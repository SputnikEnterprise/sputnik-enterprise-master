
Imports System.IO

Imports SP.Infrastructure.Logging
Imports SPS.Listing.Print.Utility.ClsMainSetting
Imports SPS.Listing.Print.Utility.MainUtilities.Utilities
Imports SP.Internal.Automations.WOSUtility.DataObjects
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Listing.DataObjects
Imports SP.Infrastructure.UI
Imports SP.DatabaseAccess.WOS
Imports System.Drawing.Printing
Imports DevExpress.Pdf
Imports SP.Infrastructure

'Imports O2S.Components.PDF4NET.PDFFile

Namespace LOSearchListing

	Public Class ClsPrintLOSearchList


#Region "Private fields"

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI

		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As SP.Infrastructure.Utility

		Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
		Private _ClsSetting As New ClsLLLOSearchPrintSetting

		Private m_ConnectionString As String

		''' <summary>
		''' The common data access object.
		''' </summary>
		Private m_CommonDatabaseAccess As ICommonDatabaseAccess
		Private m_ListingDatabaseAccess As IListingDatabaseAccess

		''' <summary>
		''' The wos data access object.
		''' </summary>
		Private m_WOSDatabaseAccess As IWOSDatabaseAccess

		Private m_CurrentPayrollData As PayrollPrintData

		Private m_CurrentEmployeeNumber As Integer
		Private m_CurrentPayrollNumber As Integer
		Private m_CurrentLanguage As String
		Private m_CurrentWOS As Boolean
		Private m_CurrentPayrollGuid As String
		Private m_CurrentEmployeeGuid As String
		Private m_PDFUtility As PDFUtilities.Utilities


#End Region


#Region "public properties"
		Public Property PrintData As ClsLLLOSearchPrintSetting

#End Region



#Region "Constructor"

		Public Sub New(ByVal _Setting As ClsLLLOSearchPrintSetting)

			_ClsSetting = _Setting
			PrintData = _Setting

			Dim m_init = CreateInitialData(_ClsSetting.SelectedMDNr, _ClsSetting.LogedUSNr)
			m_InitialData = m_init
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitialData.TranslationData, m_InitialData.ProsonalizedData)
			m_UtilityUI = New UtilityUI
			m_PDFUtility = New PDFUtilities.Utilities

			ClsMainSetting.MDData = m_init.MDData
			ClsMainSetting.UserData = m_init.UserData
			ClsMainSetting.TranslationData = m_InitialData.TranslationData
			ClsMainSetting.PerosonalizedData = m_InitialData.ProsonalizedData

			ClsMainSetting.ProgSettingData = New ClsSetting
			If String.IsNullOrWhiteSpace(_ClsSetting.ExportPath) Then
				_ClsSetting.ExportPath = _ClsSetting.GetExportPfad
			End If

			If String.IsNullOrWhiteSpace(_ClsSetting.ExportFinalFilename) Then
				_ClsSetting.ExportFinalFilename = _ClsSetting.GetExportFinalFilename
			End If
			Me._ClsSetting.USSignFileName = MainUtilities.GetUSSign(Me._ClsSetting.DbConnString2Open)

		End Sub

		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			m_InitialData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitialData.TranslationData, m_InitialData.ProsonalizedData)
			m_Utility = New SP.Infrastructure.Utility
			m_UtilityUI = New UtilityUI

			m_PDFUtility = New PDFUtilities.Utilities

			m_ConnectionString = m_InitialData.MDData.MDDbConn
			m_CommonDatabaseAccess = New CommonDatabaseAccess(m_ConnectionString, m_InitialData.UserData.UserLanguage)
			m_ListingDatabaseAccess = New ListingDatabaseAccess(m_ConnectionString, m_InitialData.UserData.UserLanguage)
			m_WOSDatabaseAccess = New WOSDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)

			ClsMainSetting.MDData = m_InitialData.MDData
			ClsMainSetting.UserData = m_InitialData.UserData
			ClsMainSetting.TranslationData = m_InitialData.TranslationData
			ClsMainSetting.PerosonalizedData = m_InitialData.ProsonalizedData

			ClsMainSetting.ProgSettingData = New ClsSetting
			Me._ClsSetting.USSignFileName = GetUSSign(m_InitialData.MDData.MDDbConn)

		End Sub

#End Region


		Function PrintLOSearchList(ByVal bAsDesign As Boolean) As PrintResult
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim result As PrintResult = New PrintResult With {.ExportFilename = String.Empty, .Printresult = False}
			Dim strResult As Boolean = True
			Dim wosResult As WOSSendResult
			Dim liLONr As New List(Of Integer)
			Dim liMANr As New List(Of Integer)
			Dim liWOS As New List(Of Boolean)
			Dim liMALang As New List(Of String)

			Dim settings = New PrinterSettings()
			If settings.PrinterName Is Nothing OrElse String.IsNullOrWhiteSpace(settings.PrinterName) Then
				m_Logger.LogError("no default printer is defined.")
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Sie haben keinen Standard-Drucker definiert."))

				Return result
			Else
				m_Logger.LogInfo(String.Format("default printer: {0}", settings.PrinterName))
			End If

			_ClsSetting = PrintData
			_ClsSetting.ListOfExportedFiles.Clear()
			Try
				liLONr = _ClsSetting.liLONr2Print
				liMANr = _ClsSetting.liMANr2Print
				liWOS = _ClsSetting.liLOSend2WOS
				liMALang = _ClsSetting.LiMALang


				Dim loNumbers As New List(Of Integer)
				For Each lo In PrintData.liLONr2Print
					loNumbers.Add(lo)
				Next
				Dim payrollData = m_ListingDatabaseAccess.LoadPayrollsPrintData(New PayrollSearchData With {.MDNr = m_InitialData.MDData.MDNr, .LONr = loNumbers, .sortvalue = PrintData.SortValue, .mawos = PayrollSearchData.WOSValue.All})
				If payrollData Is Nothing Then
					Return New PrintResult With {.ExportFilename = m_Translate.GetSafeTranslationValue("Keine Lohnabrechnungsdaten wurden gefunden!"), .Printresult = False}
				End If

				Dim firstCall As Boolean = True
				'For i As Integer = 0 To liLONr.Count - 1
				For Each lo In payrollData
					m_CurrentPayrollData = lo

					_ClsSetting.SelectedLONr2Print = lo.LONr ' liLONr(i)
					_ClsSetting.SelectedMANr2Print = lo.MANr ' liMANr(i)
					_ClsSetting.SendData2WOS = lo.Send2WOS  ' liWOS(i)
					_ClsSetting.SelectedMALang = lo.employeeLanguage  '  If(liMALang(i) = "D", String.Empty, liMALang(i))

					m_CurrentPayrollNumber = lo.LONr
					m_CurrentEmployeeNumber = lo.MANr
					m_CurrentWOS = lo.Send2WOS
					m_CurrentLanguage = lo.employeeLanguage
					m_CurrentPayrollGuid = lo.PayrollGuid
					m_CurrentEmployeeGuid = lo.EmployeeGuid

					If PrintData.WOSSendValueEnum = WOSSENDValue.PrintWithoutSending Then m_CurrentWOS = False
					_ClsSetting.SendData2WOS = m_CurrentWOS

					Dim _clsLOPrint As New ClsLLLOSearchPrint(_ClsSetting)
					If bAsDesign Then
						_clsLOPrint.ShowInDesign()
						_clsLOPrint.Dispose()

						Return result

					End If
					If _ClsSetting.SendAndPrintData2WOS OrElse Not m_CurrentWOS Then

						strResult = _clsLOPrint.ShowInPrint(firstCall)
						If firstCall Then
							_ClsSetting.NumberOfCopy = _clsLOPrint.GetNumberOfcopy

						End If
						_clsLOPrint.Dispose()
					End If
					If Not strResult Then Return result

					' in PDF erstellen...
					Try
						Dim _clsLOExport As New ClsLLLOSearchPrint(_ClsSetting)
						Dim exportResult = _clsLOExport.ExportLLDoc()
						_clsLOExport.Dispose()

						Dim exportData = _ClsSetting.ListOfExportedFiles.Find(Function(p) p.LONr = _ClsSetting.SelectedLONr2Print And p.MANr = _ClsSetting.SelectedMANr2Print)
						If exportData Is Nothing Then
							m_Logger.LogError(String.Format("could not be founded: payrollnumber: {0} | employeenumber: {1}", _ClsSetting.SelectedLONr2Print, _ClsSetting.SelectedMANr2Print))
						Else

							SaveFileToMAHistoryDocDb(exportData)
						End If

					Catch ex As Exception
						m_Logger.LogError(String.Format("{0}.Daten in History aufnehmen. {1}", strMethodeName, ex.ToString))
					End Try

					If m_CurrentWOS AndAlso _ClsSetting.ListOfExportedFiles.Count > 0 Then
						wosResult = TransferCustomerPayrollDataIntoWOS()

						result.WOSresult = wosResult.Value
					End If

					_clsLOPrint.Dispose()
					firstCall = False
				Next


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

			End Try


			Return result

		End Function

		Function ExportPayrollsData() As PrintResult
			Dim success As Boolean = True
			Dim result As PrintResult = New PrintResult With {.Printresult = True}

			Dim liLONr As New List(Of Integer)
			Dim liMANr As New List(Of Integer)
			Dim liMALang As New List(Of String)

			Dim settings = New PrinterSettings()
			If settings.PrinterName Is Nothing OrElse String.IsNullOrWhiteSpace(settings.PrinterName) Then
				m_Logger.LogError("no default printer is defined.")
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Sie haben keinen Standard-Drucker definiert."))

				Return (New PrintResult With {.Printresult = False, .PrintresultMessage = "Sie haben keinen Standard-Drucker definiert."})
			Else
				m_Logger.LogInfo(String.Format("default printer: {0}", settings.PrinterName))
			End If

			_ClsSetting = PrintData
			_ClsSetting.ListOfExportedFiles.Clear()
			result.JobResultPayrollData = New List(Of PrintedPayrollData)

			Try

				liLONr = _ClsSetting.liLONr2Print
				liMANr = _ClsSetting.liMANr2Print
				liMALang = _ClsSetting.LiMALang

				Dim loNumbers As New List(Of Integer)
				For Each lo In PrintData.liLONr2Print
					loNumbers.Add(lo)
				Next
				Dim payrollData = m_ListingDatabaseAccess.LoadPayrollsPrintData(New PayrollSearchData With {.MDNr = m_InitialData.MDData.MDNr, .LONr = loNumbers, .sortvalue = PrintData.SortValue, .mawos = PayrollSearchData.WOSValue.All})
				If payrollData Is Nothing OrElse payrollData.Count = 0 Then
					m_Logger.LogWarning("keine Lohndaten wurden gefunden.")
					Return (New PrintResult With {.Printresult = False, .PrintresultMessage = "keine Lohndaten wurden gefunden."})
				End If

				For Each lo In payrollData
					Dim JobResultData As New PrintedPayrollData

					_ClsSetting.SelectedLONr2Print = lo.LONr
					_ClsSetting.SelectedMANr2Print = lo.MANr
					_ClsSetting.SendData2WOS = False
					_ClsSetting.SelectedMALang = lo.employeeLanguage

					m_CurrentPayrollNumber = lo.LONr
					m_CurrentEmployeeNumber = lo.MANr
					m_CurrentWOS = False
					m_CurrentLanguage = lo.employeeLanguage

					Dim _clsLOPrint As New ClsLLLOSearchPrint(_ClsSetting)
					_clsLOPrint.ExportLLDoc()
					_clsLOPrint.Dispose()

					If _ClsSetting.ListOfExportedFiles.Count = 0 Then Exit For

					JobResultData.PayrollNumber = lo.LONr
					JobResultData.EmployeeNumber = lo.MANr
					JobResultData.Firstname = lo.employeefirstname
					JobResultData.Lastname = lo.employeelastname
					JobResultData.EmployeeEMail = lo.EmployeeEMail
					JobResultData.SendAsZip = lo.SendAsZIP
					JobResultData.PayrollDate = lo.createdon
					JobResultData.MandantName = m_InitialData.MDData.MDName
					JobResultData.MandantLocation = m_InitialData.MDData.MDCity

					JobResultData.ExportedFileName = _ClsSetting.ListOfExportedFiles(0).ExportFilename
					_ClsSetting.ListOfExportedFiles.Clear()


					result.JobResultPayrollData.Add(JobResultData)

				Next


			Catch ex As Exception
				result = (New PrintResult With {.Printresult = False, .PrintresultMessage = "Keine Datei wurde exportiert."})

			End Try


			Return result
		End Function

		'Function ExportEndPayrollData() As PrintResult
		'	Dim success As Boolean = True
		'	Dim result As PrintResult = New PrintResult With {.Printresult = True}

		'	Dim liLONr As New List(Of Integer)
		'	Dim liMANr As New List(Of Integer)
		'	Dim liMALang As New List(Of String)

		'	Dim settings = New PrinterSettings()
		'	If settings.PrinterName Is Nothing OrElse String.IsNullOrWhiteSpace(settings.PrinterName) Then
		'		m_Logger.LogError("no default printer is defined.")
		'		m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Sie haben keinen Standard-Drucker definiert."))

		'		Return (New PrintResult With {.Printresult = False, .PrintresultMessage = "Sie haben keinen Standard-Drucker definiert."})
		'	Else
		'		m_Logger.LogInfo(String.Format("default printer: {0}", settings.PrinterName))
		'	End If

		'	_ClsSetting = PrintData
		'	_ClsSetting.ListOfExportedFiles.Clear()
		'	result.JobResultPayrollData = New List(Of PrintedPayrollData)

		'	Try

		'		liLONr = _ClsSetting.liLONr2Print
		'		liMANr = _ClsSetting.liMANr2Print
		'		liMALang = _ClsSetting.LiMALang

		'		Dim loNumbers As New List(Of Integer)
		'		For Each lo In PrintData.liLONr2Print
		'			loNumbers.Add(lo)
		'		Next
		'		Dim payrollData = m_ListingDatabaseAccess.LoadPayrollsPrintData(New PayrollSearchData With {.MDNr = 0, .LONr = loNumbers, .sortvalue = PrintData.SortValue, .mawos = PayrollSearchData.WOSValue.All})
		'		If payrollData Is Nothing OrElse payrollData.Count = 0 Then
		'			m_Logger.LogWarning("keine Lohndaten wurden gefunden.")
		'			Return (New PrintResult With {.Printresult = False, .PrintresultMessage = "keine Lohndaten wurden gefunden."})
		'		End If

		'		For Each lo In payrollData
		'			Dim JobResultData As New PrintedPayrollData

		'			_ClsSetting.SelectedLONr2Print = lo.LONr
		'			_ClsSetting.SelectedMANr2Print = lo.MANr
		'			_ClsSetting.SendData2WOS = False
		'			_ClsSetting.SelectedMALang = lo.employeeLanguage

		'			m_CurrentPayrollNumber = lo.LONr
		'			m_CurrentEmployeeNumber = lo.MANr
		'			m_CurrentWOS = False
		'			m_CurrentLanguage = lo.employeeLanguage

		'			Dim _clsLOPrint As New ClsLLLOSearchPrint(_ClsSetting)
		'			_clsLOPrint.ExportLLDoc()
		'			_clsLOPrint.Dispose()

		'			If _ClsSetting.ListOfExportedFiles.Count = 0 Then Exit For

		'			JobResultData.PayrollNumber = lo.LONr
		'			JobResultData.EmployeeNumber = lo.MANr
		'			JobResultData.Firstname = lo.employeefirstname
		'			JobResultData.Lastname = lo.employeelastname
		'			JobResultData.EmployeeEMail = lo.EmployeeEMail
		'			JobResultData.SendAsZip = lo.SendAsZIP
		'			JobResultData.PayrollDate = lo.createdon
		'			JobResultData.MandantName = m_InitialData.MDData.MDName
		'			JobResultData.MandantLocation = m_InitialData.MDData.MDCity

		'			JobResultData.ExportedFileName = _ClsSetting.ListOfExportedFiles(0).ExportFilename
		'			_ClsSetting.ListOfExportedFiles.Clear()


		'			result.JobResultPayrollData.Add(JobResultData)

		'		Next
		'		'If _ClsSetting.ListOfExportedFiles.Count = 0 Then Return (New PrintResult With {.Printresult = False, .PrintresultMessage = "Keine Datei wurde exportiert."})

		'	Catch ex As Exception
		'		result = (New PrintResult With {.Printresult = False, .PrintresultMessage = "Keine Datei wurde exportiert."})

		'	End Try


		'	Return result
		'End Function

		Function ExportLOSearchList() As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strResult As String = String.Empty
			Dim liLONr As New List(Of Integer)
			Dim liMANr As New List(Of Integer)
			Dim liMALang As New List(Of String)

			Dim settings = New PrinterSettings()
			If settings.PrinterName Is Nothing OrElse String.IsNullOrWhiteSpace(settings.PrinterName) Then
				m_Logger.LogError("no default printer is defined.")
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Sie haben keinen Standard-Drucker definiert."))

				Return String.Empty
			Else
				m_Logger.LogInfo(String.Format("default printer: {0}", settings.PrinterName))
			End If

			_ClsSetting = PrintData
			_ClsSetting.ListOfExportedFiles.Clear()
			Try

				liLONr = _ClsSetting.liLONr2Print
				liMANr = _ClsSetting.liMANr2Print
				liMALang = _ClsSetting.LiMALang

				Dim loNumbers As New List(Of Integer)
				For Each lo In PrintData.liLONr2Print
					loNumbers.Add(lo)
				Next
				Dim payrollData = m_ListingDatabaseAccess.LoadPayrollsPrintData(New PayrollSearchData With {.MDNr = m_InitialData.MDData.MDNr, .LONr = loNumbers, .sortvalue = PrintData.SortValue, .mawos = PayrollSearchData.WOSValue.All})
				If payrollData Is Nothing OrElse payrollData.Count = 0 Then
					m_Logger.LogWarning("keine Lohndaten wurden gefunden.")
					Return String.Empty
				End If

				For Each lo In payrollData

					_ClsSetting.SelectedLONr2Print = lo.LONr
					_ClsSetting.SelectedMANr2Print = lo.MANr
					_ClsSetting.SendData2WOS = False
					_ClsSetting.SelectedMALang = lo.employeeLanguage

					m_CurrentPayrollNumber = lo.LONr
					m_CurrentEmployeeNumber = lo.MANr
					m_CurrentWOS = False
					m_CurrentLanguage = lo.employeeLanguage

					Dim _clsLOPrint As New ClsLLLOSearchPrint(_ClsSetting)
					strResult = _clsLOPrint.ExportLLDoc()
					_clsLOPrint.Dispose()

					If _ClsSetting.ListOfExportedFiles.Count = 0 Then Exit For
				Next
				If _ClsSetting.ListOfExportedFiles.Count = 0 Then Return String.Empty

				Try
					If _ClsSetting.ListOfExportedFiles.Count > 0 Then

						Dim strExportPfad As String = m_InitialData.UserData.spTempPayrollPath
						If Not Directory.Exists(strExportPfad) Then Directory.CreateDirectory(m_InitialData.UserData.spTempPayrollPath)

						Dim fileName As String = "Lohnabrechnung_"
						_ClsSetting.ExportFinalFilename = String.Format("{0}{1}_{2}_{3}.pdf", fileName, m_InitialData.MDData.MDName, _ClsSetting.SelectedMonth, _ClsSetting.SelectedYear)

						If File.Exists(Path.Combine(strExportPfad, _ClsSetting.ExportFinalFilename)) Then
							Try
								File.Delete(Path.Combine(strExportPfad, _ClsSetting.ExportFinalFilename))
							Catch ex As Exception
								_ClsSetting.ExportFinalFilename = String.Format("{0}{1}_{2}_{3}_{4}.PDF", fileName, m_InitialData.MDData.MDName, _ClsSetting.SelectedMonth, _ClsSetting.SelectedYear, Environment.TickCount)
							End Try
						End If

						Dim strFinalFilename As String = Path.Combine(strExportPfad, _ClsSetting.ExportFinalFilename)

						If _ClsSetting.ListOfExportedFiles.Count = 1 Then
							Try
								File.Copy(_ClsSetting.ListOfExportedFiles(0).ExportFilename, strFinalFilename, True)
								strResult = strFinalFilename

							Catch ex As Exception
								m_Logger.LogError(String.Format("{0}.Datei kopieren: {1}", strMethodeName, ex.ToString))

							End Try

						ElseIf _ClsSetting.ListOfExportedFiles.Count > 1 Then

							Try
								Dim fileList As New List(Of String)
								For Each itm In _ClsSetting.ListOfExportedFiles
									fileList.Add(itm.ExportFilename)
								Next
								Dim mergePDF = m_PDFUtility.MergePdfFiles(fileList.ToArray, strFinalFilename)

								If Not mergePDF Then
									m_Logger.LogError("merging pdf file was not successfull!")
									strFinalFilename = String.Empty
								End If

							Catch ex As Exception
								m_Logger.LogError(String.Format("{0}", ex.ToString))
								strFinalFilename = String.Empty

							End Try
							strResult = strFinalFilename

						Else
							Return String.Empty

						End If

						For Each fileitem In _ClsSetting.ListOfExportedFiles
							File.Delete(fileitem.ExportFilename)
						Next


					End If

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.MergePDF: {1}", strMethodeName, ex.ToString))

				End Try

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

			End Try

			Return strResult
		End Function

		Private Function SaveFileToMAHistoryDocDb(ByVal exportFile As ExportPayrollData) As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strDocInfo As String = String.Format("{0} / {1}", exportFile.PayrollMonth, exportFile.PayrollYear)
			Dim strDocArt As String = "Lohnabrechnung"
			Dim strResult As String = "error"

			Try
				strResult = MainUtilities.SavePrintedDocToDb(exportFile.MANr, strDocArt, strDocInfo, exportFile.ExportFilename, "MA_Printed_Docs")

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

			End Try

			Return strResult
		End Function


#Region "WOS-Methods"


		Private Function TransferCustomerPayrollDataIntoWOS() As WOSSendResult
			Dim result As WOSSendResult = New WOSSendResult With {.Value = False, .Message = "Initialisierung..."}
			Dim success As Boolean = True

			Dim wos = New SP.Internal.Automations.WOSUtility.EmployeeExport(m_InitialData)

			Dim wosSetting = New WOSSendSetting

			wosSetting.EmployeeNumber = m_CurrentEmployeeNumber
			wosSetting.PayrollNumber = m_CurrentPayrollNumber
			wosSetting.EmployeeGuid = m_CurrentEmployeeGuid
			wosSetting.DocumentArtEnum = WOSSendSetting.DocumentArt.Lohnabrechnung

			Dim docGuid As String = m_CurrentPayrollGuid
			If String.IsNullOrWhiteSpace(docGuid) Then
				Dim newGuid As String = Guid.NewGuid.ToString
				Dim successGuid As Boolean = True
				successGuid = successGuid AndAlso m_WOSDatabaseAccess.UpdatePayrollGuidData(m_CurrentPayrollNumber, newGuid)

				If successGuid Then docGuid = newGuid
				If Not successGuid Then Return New WOSSendResult With {.Value = False, .Message = "Rechnung konnte nicht aktualisiert werden (DocGuid)!"}
			End If

			m_CurrentPayrollGuid = docGuid
			wosSetting.AssignedDocumentGuid = m_CurrentPayrollGuid

			' Lohnabrechnung: 5 / 2017
			Dim docInfo As String = String.Format("Lohnabrechnung: {0} / {1}", m_CurrentPayrollData.monat, m_CurrentPayrollData.jahr)
			wosSetting.DocumentInfo = docInfo

			Dim exportData = _ClsSetting.ListOfExportedFiles.Find(Function(p) p.LONr = m_CurrentPayrollNumber And p.MANr = m_CurrentEmployeeNumber)
			If exportData Is Nothing Then
				m_Logger.LogError(String.Format("could not be founded: payrollnumber: {0} | employeenumber: {1}", m_CurrentPayrollNumber, m_CurrentEmployeeNumber))
				Return New WOSSendResult With {.Value = False, .Message = "no payroll file was founded!"}
			End If
			Dim pdfFilename As String = exportData.ExportFilename

			Dim fileByte() = m_Utility.LoadFileBytes(pdfFilename)
			wosSetting.ScanDoc = fileByte
			Dim scanfileinfo = New FileInfo(pdfFilename)
			wosSetting.ScanDocName = scanfileinfo.Name

			wos.WOSSetting = wosSetting

			If success Then result = wos.TransferEmployeeDocumentDataToWOS(True)


			Return result

		End Function

#End Region




#Region "Helpers"

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


