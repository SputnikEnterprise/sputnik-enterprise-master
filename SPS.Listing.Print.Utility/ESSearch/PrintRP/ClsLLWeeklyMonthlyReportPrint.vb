
Imports SPProgUtility.Mandanten
Imports System.IO
Imports combit.ListLabel25
Imports SPS.Listing.Print.Utility.MainUtilities
Imports SP.Infrastructure.Logging
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Common
Imports SP.Infrastructure.UI
Imports SP.DatabaseAccess.Listing.DataObjects
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Employee
Imports DevExpress.XtraSplashScreen
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.ES
Imports SP.Infrastructure

Namespace PrintReport


	Public Class ClsLLWeeklyMonthlyReportPrint

		Implements IDisposable
		Protected disposed As Boolean = False

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		''' <summary>
		''' The Initialization data.
		''' </summary>
		Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

		''' <summary>
		''' The translation value helper.
		''' </summary>
		Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

		''' <summary>
		''' The common database access.
		''' </summary>
		Protected m_CommonDatabaseAccess As ICommonDatabaseAccess

		''' <summary>
		''' The employee database access.
		''' </summary>
		Protected m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess
		Protected m_CustomerDatabaseAccess As ICustomerDatabaseAccess
		Protected m_EinsatzDatabaseAccess As IESDatabaseAccess

		Private m_path As SPProgUtility.ProgPath.ClsProgPath
		Private m_ConnectionString As String

		Private m_ShowPrintOptionDialg As Boolean

		''' <summary>
		''' The mandant.
		''' </summary>
		Private m_MandantData As Mandant

		''' <summary>
		''' The Listing data access object.
		''' </summary>
		Private m_ListingDatabaseAccess As IListingDatabaseAccess

		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As SP.Infrastructure.Utility

		Private m_EmployeeAddressData As EmployeeSAddressData
		Private m_EmployeeData As EmployeeMasterData
		Private m_CurrentReportData As RPPrintWeeklyData

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI

		Private m_PrintJobData As PrintJobData

		Friend _ClsLLFunc As New ClsLLFunc

		Private m_PrintYear As Integer
		Private m_FirstWeek As Integer
		Private m_LastWeek As Integer


		Private m_StartPrintInLLDebug As Boolean
		Private m_CurrentExportPrintInFiles As Boolean
		Private Property ExistsDocFile As Boolean
		Private m_EmployeeLanguage As String
		Private m_NumberOfCopies As Integer?
		Private m_PDFUtility As PDFUtilities.Utilities

		Private LL As ListLabel = New ListLabel



#Region "public properties"

		Public Property PrintData As New ReportPrintData

#End Region

#Region "Constructor"

		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			m_InitializationData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
			m_MandantData = New Mandant
			m_path = New SPProgUtility.ProgPath.ClsProgPath
			m_UtilityUI = New UtilityUI
			m_Utility = New SP.Infrastructure.Utility
			m_PDFUtility = New PDFUtilities.Utilities

			m_CommonDatabaseAccess = New CommonDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)
			m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)
			m_CustomerDatabaseAccess = New CustomerDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)
			m_EinsatzDatabaseAccess = New ESDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)
			m_ListingDatabaseAccess = New ListingDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)

			LL = New ListLabel
			m_StartPrintInLLDebug = CBool(m_MandantData.GetSelectedMDProfilValue(m_InitializationData.MDData.MDNr, Now.Year, "Sonstiges", "EnableLLDebug", "0"))


		End Sub

		Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
			If Not Me.disposed Then
				If disposing Then

				End If
				' Add code here to release the unmanaged resource.
				LL.Dispose()
				LL.Core.Dispose()
				' Note that this is not thread safe.
			End If
			Me.disposed = True
		End Sub

		' Do not change or add Overridable to these methods.
		' Put cleanup code in Dispose(ByVal disposing As Boolean).
		Public Overloads Sub Dispose() Implements IDisposable.Dispose
			Dispose(True)
			GC.SuppressFinalize(Me)
		End Sub

		Protected Overrides Sub Finalize()
			Dispose(False)
			MyBase.Finalize()
		End Sub


#End Region


		Public Function PrintEmployeeRepportData() As PrintResult
			Dim success As Boolean = True
			Dim result As PrintResult = New PrintResult With {.Printresult = True}
			Dim i As Integer = 0
			Dim countOfPrintedReport As Integer = 0
			Dim verifyTestPrint As Boolean = True

			m_PrintYear = PrintData.PrintYear
			m_FirstWeek = PrintData.FirstWeek
			m_LastWeek = PrintData.LastWeek

			m_ShowPrintOptionDialg = True

			PrintData.ListOfExportedFiles = New List(Of String)

			If String.IsNullOrWhiteSpace(PrintData.ExportPath) Then PrintData.ExportPath = m_InitializationData.UserData.spTempRepportPath ' .GetSpSMAHomeFolder
			m_CurrentExportPrintInFiles = PrintData.ExportPrintInFiles.GetValueOrDefault(False)

			Dim jobNumber As String = PrintData.PrintJobNumber
			success = success AndAlso LoadPrintJobData(jobNumber)
			If Not success Then Return New PrintResult With {.Printresult = False, .PrintresultMessage = "Vorlage wurde nicht gefunden!"}
			success = success AndAlso LoadReportData()
			If Not success Then Return New PrintResult With {.Printresult = False, .PrintresultMessage = "Keine Rapportdaten wurden gefunden!"}

			If PrintData.ShowAsDesign Then
				success = success AndAlso ShowAssignedListInDesign()

				Return result
			End If

			SplashScreenManager.CloseForm(False)
			SplashScreenManager.ShowForm(GetType(WaitForm1), True, False)
			SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Rapporte drucken") & Space(20))
			SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Die Rapporte werden zusammengestellt") & "...")

			countOfPrintedReport = 0
			For Each Report In PrintData.m_Repportdata
				m_CurrentReportData = Report

				success = success AndAlso PrintAssignedList()
				If Not m_CurrentExportPrintInFiles Then success = success AndAlso m_ListingDatabaseAccess.UpdateAssignedReportWithPrintData(m_InitializationData.MDData.MDNr, m_CurrentReportData, m_InitializationData.UserData.UserNr)

				countOfPrintedReport += 1
				If verifyTestPrint AndAlso countOfPrintedReport = PrintData.CountOfTestPrint Then
					verifyTestPrint = m_UtilityUI.ShowYesNoDialog("Möchten Sie weitere Test-Rapporte drucken?", "Test-Rapport drucken")
					If verifyTestPrint Then countOfPrintedReport = 0
				End If

				m_ShowPrintOptionDialg = False
			Next

			If Not success Then Return New PrintResult With {.Printresult = False}

			Try
				If m_CurrentExportPrintInFiles Then

					Dim strExportPfad As String = m_InitializationData.UserData.spTempRepportPath
					If Not Directory.Exists(strExportPfad) Then Directory.CreateDirectory(strExportPfad)

					Dim fileName As String = "Rapport_"
					PrintData.ExportFinalFilename = String.Format("{0}_{1}.pdf", fileName, m_InitializationData.MDData.MDName)

					If File.Exists(Path.Combine(strExportPfad, PrintData.ExportFinalFilename)) Then
						Try
							File.Delete(Path.Combine(strExportPfad, PrintData.ExportFinalFilename))
						Catch ex As Exception
							PrintData.ExportFinalFilename = String.Format("{0}{1}_{2}.PDF", fileName, m_InitializationData.MDData.MDName, Environment.TickCount)
						End Try
					End If

					Dim strFinalFilename As String = Path.Combine(strExportPfad, PrintData.ExportFinalFilename)

					If PrintData.ListOfExportedFiles.Count = 1 Then
						Try
							File.Copy(PrintData.ListOfExportedFiles(0), strFinalFilename, True)

						Catch ex As Exception
							m_Logger.LogError(String.Format("Datei kopieren: {0}", ex.ToString))

						End Try

					ElseIf PrintData.ListOfExportedFiles.Count > 1 Then

						Try
							Dim mergePDF = m_PDFUtility.MergePdfFiles(PrintData.ListOfExportedFiles.ToArray, strFinalFilename)

							If Not mergePDF Then
								m_Logger.LogError("merging pdf file was not successfull!")
								strFinalFilename = String.Empty
							End If

						Catch ex As Exception
							m_Logger.LogError(String.Format("{0}", ex.ToString))
							strFinalFilename = String.Empty

						End Try

					End If

				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("MergePDF: {0}", ex.ToString))

			End Try

			SplashScreenManager.CloseForm(False)

			Return result

		End Function


#Region "Private Methodes"

		Private Function LoadPrintJobData(ByVal jobNumberToPrint As String) As Boolean

			m_PrintJobData = m_ListingDatabaseAccess.LoadAssignedPrintJobData(m_InitializationData.MDData.MDNr, m_EmployeeLanguage, jobNumberToPrint)


			Return Not (m_PrintJobData Is Nothing)

		End Function

		Private Function LoadReportData() As Boolean

			Dim data = m_ListingDatabaseAccess.LoadRPPrintWeeklyData(m_InitializationData.MDData.MDNr, m_PrintYear, m_FirstWeek, m_LastWeek, m_InitializationData.UserData.UserNr)

			If data Is Nothing Then
				m_Logger.LogError(m_Translate.GetSafeTranslationValue("Rapportdaten konnten nicht geladen werden."))

				Return False
			End If

			PrintData.m_Repportdata = data


			Return Not (m_PrintJobData Is Nothing)

		End Function

		Private Function ShowAssignedListInDesign() As Boolean
			Dim result As Boolean = True

			Try
				'If m_EmployeeData Is Nothing Then
				'	m_Logger.LogWarning("Keine Daten wurden gefunden.")

				'	Return False
				'End If

				InitLL(LL)
				LL.Variables.Clear()
				LL.Fields.Clear()

				result = result AndAlso SetLLVariable(LL)
				For Each Report In PrintData.m_Repportdata
					result = result AndAlso LoadAssignedEmployeeData(LL, Report.MANr)
					result = result AndAlso LoadAssignedCustomerData(LL, Report.KDNr)
					result = result AndAlso LoadAssignedEmploymentData(LL, Report.ESNr)

					result = result AndAlso GetRPWeeklyData4Print(LL, Report)

					Exit For
				Next
				If Not result Then Return result

				LL.Variables.Add("WOSDoc", 1)

				If result Then
					SplashScreenManager.CloseForm(False)

					LL.Core.LlDefineLayout(CType(PrintData.frmhwnd, IntPtr), m_PrintJobData.LLDocLabel, If(m_PrintJobData.LLDocName.ToUpper.EndsWith(".LST".ToUpper), LlProject.List, LlProject.Card), m_PrintJobData.LLDocName)
					LL.Core.LlPrintEnd()
				End If

			Catch LlException As Exception
				m_Logger.LogError(String.Format("LlException: {0}", LlException.ToString))
				LL.Dispose()
				result = False

			Finally

			End Try


			Return result

		End Function

		Function PrintAssignedList() As Boolean
			Const LL_PRNOPT_COPIES_SUPPORTED As Integer = 3
			Dim result As Boolean = True
			Dim doExport As Boolean = m_CurrentExportPrintInFiles OrElse PrintData.WOSSendValueEnum = WOSZVSENDValue.PrintOtherSendWOS

			Try
				If m_StartPrintInLLDebug Then
					LL.Debug = LlDebug.LogToFile
					LL.Debug = LlDebug.Enabled
				End If
			Catch ex As Exception
				m_Logger.LogError(String.Format("EnablingLLDebuging: {0}", ex.ToString))

			End Try

			Try

				InitLL(LL)
				LL.Variables.Clear()
				LL.Fields.Clear()

				result = result AndAlso SetLLVariable(LL)

				result = result AndAlso LoadAssignedEmployeeData(LL, m_CurrentReportData.MANr)
				result = result AndAlso LoadAssignedCustomerData(LL, m_CurrentReportData.KDNr)
				result = result AndAlso LoadAssignedEmploymentData(LL, m_CurrentReportData.ESNr)

				result = result AndAlso GetRPWeeklyData4Print(LL, m_CurrentReportData)
				If Not result Then Return result

				LL.Variables.Add("WOSDoc", 0)

				SplashScreenManager.CloseForm(False)
				LL.Core.LlPrintWithBoxStart(If(m_PrintJobData.LLDocName.ToUpper.EndsWith(".LST".ToUpper),
																				 LlProject.List, LlProject.Card),
																				 m_PrintJobData.LLDocName, LlPrintMode.Export,
																				 If(doExport, LlBoxType.None, LlBoxType.StandardAbort),
																				 CType(PrintData.frmhwnd, IntPtr), m_PrintJobData.LLDocLabel)

				Dim strExportPfad As String = m_InitializationData.UserData.spTempRepportPath
				If Not Directory.Exists(strExportPfad) Then Directory.CreateDirectory(strExportPfad)

				Dim initialFilename As String = "Rapport_"
				Dim strExportFilename = String.Format("{0}_{1}_{2}_{3}.pdf", initialFilename, m_CurrentReportData.MANr, m_CurrentReportData.ESNr, Environment.TickCount)

				If doExport Then

					LL.Variables.Add("WOSDoc", 1)

					LL.Core.LlPrintSetOptionString(LlPrintOptionString.Export, "PDF")
					LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.File", strExportFilename)
					LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.Path", strExportPfad)
					LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.Quiet", "1")

					LL.Core.LlPrintSetOption(LlPrintOption.Dialog_DestinationMask, 2)

				Else
					LL.Core.LlPrintSetOption(LlPrintOption.Copies, Math.Max(1, m_NumberOfCopies.GetValueOrDefault(1)))
					LL.Core.LlPrintSetOption(LL_PRNOPT_COPIES_SUPPORTED, LL.Core.LlPrintGetOption(LlPrintOption.Copies))

					If m_ShowPrintOptionDialg Then LL.Core.LlPrintOptionsDialog(CType(PrintData.frmhwnd, IntPtr), m_PrintJobData.PrintBoxHeader)
				End If

				Dim TargetFormat As String = LL.Core.LlPrintGetOptionString(LlPrintOptionString.Export)

				While Not LL.Core.LlPrint()
					Trace.WriteLine("while schleife!")
				End While


				LL.Core.LlPrintEnd(0)

				If TargetFormat = "PRV" Then
					SplashScreenManager.CloseForm(False)
					LL.Core.LlPreviewDisplay(m_PrintJobData.LLDocName, m_path.GetSpSTempFolder, CType(PrintData.frmhwnd, IntPtr))
					LL.Core.LlPreviewDeleteFiles(m_PrintJobData.LLDocName, m_path.GetSpSTempFolder)

					Return False
				End If

				If doExport Then PrintData.ListOfExportedFiles.Add(Path.Combine(strExportPfad, strExportFilename))


			Catch LlException As LL_User_Aborted_Exception
				Return False

			Catch LlException As Exception
				m_Logger.LogError(String.Format("LlException ({0} >>> {1}): {2}", PrintData.PrintJobNumber, m_PrintJobData.LLDocName, LlException.ToString))
				result = False

			Finally

			End Try


			Return result

		End Function

		Function ExportLLDoc(ByVal strJobNr As String, ByVal iExportMode As Short,
											 ByVal iProposeNr As Integer) As Boolean

			Dim strLLTemplate As String = _ClsLLFunc.LLDocName
			Dim bResult As Boolean = True
			If Not Me.ExistsDocFile Then Return bResult

			'Dim strQuery As String = PrintData.SQL2Open
			'Dim Conn As SqlConnection = New SqlConnection(m_ConnectionString)
			'Dim i As Integer = 0

			Try
				'Conn.Open()
				'Dim cmd As System.Data.SqlClient.SqlCommand
				'cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
				'cmd.CommandType = CommandType.StoredProcedure
				'Dim param As System.Data.SqlClient.SqlParameter
				'param = cmd.Parameters.AddWithValue("@iProposeNr", iProposeNr)

				'Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader
				'rFoundedrec.Read()
				'If IsNothing(rFoundedrec) Then
				'	Dim strMessage As String = "Die gesuchten Daten sind nicht vorhanden. Bitte kontrollieren sie Ihre Angaben."
				'	MsgBox(TranslateMyText(strMessage),
				'			 MsgBoxStyle.Critical, TranslateMyText("Daten suchen"))
				'	Return bResult
				'End If

				InitLL(LL)
				LL.Variables.Clear()
				LL.Fields.Clear()

				'If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				'	DefineData(True, rFoundedrec)
				'Else
				'	DefineData(False, rFoundedrec)
				'End If
				SetLLVariable(LL)

				LL.ExportOptions.Clear()
				SetExportSetting(iExportMode)

				LL.Core.LlPrintWithBoxStart(If(_ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper), LlProject.List, LlProject.Card),
																	_ClsLLFunc.LLDocName, LlPrintMode.Export, LlBoxType.StandardAbort, CType(PrintData.frmhwnd, IntPtr),
																	_ClsLLFunc.LLDocLabel)

				LL.Core.LlPrintSetOptionString(LlPrintOptionString.Export, _ClsLLFunc.LLExporterName)
				LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.File", _ClsLLFunc.LLExporterFileName)
				LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.Path", m_path.GetSpSOfferHomeFolder)
				LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.Quiet", "1")

				LL.Core.LlPrintSetOption(LlPrintOption.Dialog_DestinationMask, 2)
				Dim TargetFormat As String = LL.Core.LlPrintGetOptionString(LlPrintOptionString.Export)

				While Not LL.Core.LlPrint()
				End While

				'If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				'	Do
				'		' pass data for current record
				'		DefineData(True, rFoundedrec)

				'		While LL.Core.LlPrintFields() = LlConstants.WrnRepeatData ' LlConst.LL_WRN_REPEAT_DATA
				'			LL.Core.LlPrint()
				'		End While

				'	Loop While rFoundedrec.Read()

				'	While LL.Core.LlPrintFieldsEnd() = LlConstants.WrnRepeatData ' LlConst.LL_WRN_REPEAT_DATA
				'	End While
				'End If
				LL.Core.LlPrintEnd(0)

				If TargetFormat = "PRV" Then
					LL.Core.LlPreviewDisplay(_ClsLLFunc.LLDocName,
																 m_path.GetSpSTempFolder,
																 CType(PrintData.frmhwnd, IntPtr))
					LL.Core.LlPreviewDeleteFiles(_ClsLLFunc.LLDocName,
																		 m_path.GetSpSTempFolder)
					Return False
				End If
				'      _ClsLLFunc.LLExportFileName = String.Format("{0}{1}", m_path.GetSpSOfferHomeFolder, _ClsLLFunc.LLExporterFileName)

			Catch LlException As Exception
				m_Logger.LogError(String.Format("LlException.Message:{0}This information was generated by a List & Label custom exception.", vbNewLine))

				'If Err.Number <> (LlError.LL_ERR_USER_ABORTED) And Err.Number <> 5 Then
				'  MessageBox.Show(LlException.Message + vbCrLf, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
				'End If
				bResult = False

			Finally

			End Try

			Return bResult
		End Function

		Sub SetExportSetting(ByVal iIndex As Short)
			Select Case iIndex
				Case 0
					_ClsLLFunc.LLExportfilter = "PDF Files|*.PDF"
					_ClsLLFunc.LLExporterName = "PDF"
					_ClsLLFunc.LLExporterFileName = _ClsLLFunc.LLExportedFileName + ".pdf"

				Case 1
					_ClsLLFunc.LLExportfilter = "MHTML Files|*.mht"
					_ClsLLFunc.LLExporterName = "MHTML"
					_ClsLLFunc.LLExporterFileName = _ClsLLFunc.LLExportedFileName + ".mht"

				Case 2
					_ClsLLFunc.LLExportfilter = "HTML Files|*.HTM"
					_ClsLLFunc.LLExporterName = "HTML"
					_ClsLLFunc.LLExporterFileName = _ClsLLFunc.LLExportedFileName + ".htm"

				Case 3
					_ClsLLFunc.LLExportfilter = "RTF Files|*.RTF"
					_ClsLLFunc.LLExporterName = "RTF"
					_ClsLLFunc.LLExporterFileName = _ClsLLFunc.LLExportedFileName + ".rtf"

				Case 4
					_ClsLLFunc.LLExportfilter = "XML Files|*.XML"
					_ClsLLFunc.LLExporterName = "XML"
					_ClsLLFunc.LLExporterFileName = _ClsLLFunc.LLExportedFileName + ".xml"

				Case 5
					_ClsLLFunc.LLExportfilter = "Tiff Files|*.TIF"
					_ClsLLFunc.LLExporterName = "PICTURE_MULTITIFF"
					_ClsLLFunc.LLExporterFileName = _ClsLLFunc.LLExportedFileName + ".tif"

				Case 6
					_ClsLLFunc.LLExportfilter = "Text Files|*.TXT"
					_ClsLLFunc.LLExporterName = "TXT"
					_ClsLLFunc.LLExporterFileName = _ClsLLFunc.LLExportedFileName + ".txt"

				Case 7
					_ClsLLFunc.LLExportfilter = "Excel Files|*.XLS"
					_ClsLLFunc.LLExporterName = "XLS"
					_ClsLLFunc.LLExporterFileName = _ClsLLFunc.LLExportedFileName + ".xls"
			End Select

		End Sub

		Private Function LoadAssignedEmployeeData(ByVal LL As ListLabel, ByVal employeeNumber As Integer) As Boolean

			Dim success As Boolean = True

			Dim data = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(employeeNumber, False)

			If data Is Nothing Then
				m_Logger.LogError(m_Translate.GetSafeTranslationValue("Kandidatendaten konnten nicht geladen werden."))

				Return False
			End If

			m_EmployeeAddressData = m_EmployeeDatabaseAccess.LoadEmployeeReportAddressData(employeeNumber)

			If (m_EmployeeAddressData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kandidaten Adressdaten konnten nicht geladen werden."))
			End If


			Try
				LL.Core.LlDefineVariableExt("MANr", ReplaceMissing(data.EmployeeNumber, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("MANachname", ReplaceMissing(m_EmployeeAddressData.Lastname, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("MAVorname", ReplaceMissing(m_EmployeeAddressData.Firstname, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("MAName", ReplaceMissing(m_EmployeeAddressData.EmployeeFullnameWithComma, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("MACo", ReplaceMissing(m_EmployeeAddressData.StaysAt, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Geschlecht", ReplaceMissing(m_EmployeeAddressData.Gender, String.Empty), LlFieldType.Text)

				LL.Core.LlDefineVariableExt("MAPlzOrt", ReplaceMissing(m_EmployeeAddressData.EmployeePostcodeLocation, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("MAPst", ReplaceMissing(m_EmployeeAddressData.PostOfficeBox, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("MAStr", ReplaceMissing(m_EmployeeAddressData.Street, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("MALnd", ReplaceMissing(m_EmployeeAddressData.Country, String.Empty), LlFieldType.Text)

				LL.Core.LlDefineVariableExt("Telefon_G", ReplaceMissing(data.Telephone_G, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Telefon_P", ReplaceMissing(data.Telephone_P, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Telefon_2", ReplaceMissing(data.Telephone2, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Telefon_3", ReplaceMissing(data.Telephone3, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Natel", ReplaceMissing(data.MobilePhone, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Natel2", ReplaceMissing(data.MobilePhone2, String.Empty), LlFieldType.Text)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				Dim msg = m_Translate.GetSafeTranslationValue("Fehler während Auswertung der Kandidaten-Felder für Druckausgabe der Rapporte.")
				m_UtilityUI.ShowErrorDialog(String.Format("{1}{0}{2}", vbNewLine, msg, ex.ToString))

				Return False
			End Try


			Return success

		End Function

		Private Function LoadAssignedCustomerData(ByVal LL As ListLabel, ByVal customerNumber As Integer) As Boolean

			Dim success As Boolean = True

			Dim data = m_CustomerDatabaseAccess.LoadCustomerMasterData(customerNumber, "%%")

			If data Is Nothing Then
				m_Logger.LogError(m_Translate.GetSafeTranslationValue("Kundendaten konnten nicht geladen werden."))

				Return False
			End If


			Try
				LL.Core.LlDefineVariableExt("KDNr", ReplaceMissing(data.CustomerNumber, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("Firma1", ReplaceMissing(data.Company1, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Firma2", ReplaceMissing(data.Company2, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Firma3", ReplaceMissing(data.Company3, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("KDFax", ReplaceMissing(data.Telefax, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("KDLnd", ReplaceMissing(data.CountryCode, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("KDPLZOrt", ReplaceMissing(data.CustomerPostcodeLocation, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("KDPst", ReplaceMissing(data.PostOfficeBox, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("KDStr", ReplaceMissing(data.Street, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("KDTelefon", ReplaceMissing(data.Telephone, String.Empty), LlFieldType.Text)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				Dim msg = m_Translate.GetSafeTranslationValue("Fehler während Auswertung der Kunden-Felder für Druckausgabe der Rapporte.")
				m_UtilityUI.ShowErrorDialog(String.Format("{1}{0}{2}", vbNewLine, msg, ex.ToString))

				Return False
			End Try


			Return success

		End Function

		Private Function LoadAssignedEmploymentData(ByVal LL As ListLabel, ByVal employmentNumber As Integer) As Boolean

			Dim success As Boolean = True

			Dim data = m_EinsatzDatabaseAccess.LoadESMasterData(employmentNumber)

			If data Is Nothing Then
				m_Logger.LogError(m_Translate.GetSafeTranslationValue("Einsatzdaten konnten nicht geladen werden."))

				Return False
			End If

			Dim esSalaryData = m_EinsatzDatabaseAccess.LoadESSalaryData(employmentNumber)

			If esSalaryData Is Nothing Then
				m_Logger.LogError(m_Translate.GetSafeTranslationValue("Einsatz-Lohn Daten konnten nicht geladen werden."))

				Return False
			End If


			Try
				'LL.Core.LlDefineVariableExt("ESLABetrag1", ReplaceMissing(reportData., String.Empty), LlFieldType.Text)
				'LL.Core.LlDefineVariableExt("ESLABetrag2", ReplaceMissing(reportData.Montag, String.Empty), LlFieldType.Text)
				'LL.Core.LlDefineVariableExt("ESLABetrag3", ReplaceMissing(reportData.Montag, String.Empty), LlFieldType.Text)
				'LL.Core.LlDefineVariableExt("ESLABetrag4", ReplaceMissing(reportData.Montag, String.Empty), LlFieldType.Text)
				'LL.Core.LlDefineVariableExt("ESLABetrag5", ReplaceMissing(reportData.Montag, String.Empty), LlFieldType.Text)
				'LL.Core.LlDefineVariableExt("ESLABez1", ReplaceMissing(reportData.Montag, String.Empty), LlFieldType.Text)
				'LL.Core.LlDefineVariableExt("ESLABez2", ReplaceMissing(reportData.Montag, String.Empty), LlFieldType.Text)
				'LL.Core.LlDefineVariableExt("ESLABez3", ReplaceMissing(reportData.Montag, String.Empty), LlFieldType.Text)
				'LL.Core.LlDefineVariableExt("ESLABez4", ReplaceMissing(reportData.Montag, String.Empty), LlFieldType.Text)
				'LL.Core.LlDefineVariableExt("ESLABez5", ReplaceMissing(reportData.Montag, String.Empty), LlFieldType.Text)
				'LL.Core.LlDefineVariableExt("ESLANr1", ReplaceMissing(reportData.Montag, String.Empty), LlFieldType.Text)
				'LL.Core.LlDefineVariableExt("ESLANr2", ReplaceMissing(reportData.Montag, String.Empty), LlFieldType.Text)
				'LL.Core.LlDefineVariableExt("ESLANr3", ReplaceMissing(reportData.Montag, String.Empty), LlFieldType.Text)
				'LL.Core.LlDefineVariableExt("ESLANr4", ReplaceMissing(reportData.Montag, String.Empty), LlFieldType.Text)
				'LL.Core.LlDefineVariableExt("ESLANr5", ReplaceMissing(reportData.Montag, String.Empty), LlFieldType.Text)


				LL.Core.LlDefineVariableExt("ESNr", ReplaceMissing(data.ESNR, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("ESSUVA", ReplaceMissing(data.SUVA, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("GAVGruppe0", ReplaceMissing(esSalaryData(0).GAVGruppe0, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("MAStdSpesen", ReplaceMissing(esSalaryData(0).MAStdSpesen, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("MATSpesen", ReplaceMissing(esSalaryData(0).MATSpesen, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("Melden", ReplaceMissing(data.Melden, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Stundenlohn", ReplaceMissing(esSalaryData(0).StundenLohn, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("Tarif", ReplaceMissing(esSalaryData(0).Tarif, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("KDTSpesen", ReplaceMissing(esSalaryData(0).KDTSpesen, 0), LlFieldType.Numeric_Localized)

				LL.Core.LlDefineVariableExt("ArbOrt", ReplaceMissing(data.Arbort, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Arbzeit", ReplaceMissing(data.Arbzeit, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Bemerk_1", ReplaceMissing(data.Bemerk_1, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Bemerk_2", ReplaceMissing(data.Bemerk_2, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Bemerk_3", ReplaceMissing(data.Bemerk_3, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Bemerk_LO", ReplaceMissing(data.Bemerk_Lo, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Bemerk_MA", ReplaceMissing(data.Bemerk_MA, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Bemerk_P", ReplaceMissing(data.Bemerk_P, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Bemerk_RE", ReplaceMissing(data.Bemerk_RE, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Ende", ReplaceMissing(data.Ende, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("ES_Als", ReplaceMissing(data.ES_Als, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("ES_Uhr", ReplaceMissing(data.ES_Uhr, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("ES_Ab", ReplaceMissing(data.ES_Ab, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("ES_Ende", ReplaceMissing(data.ES_Ende, String.Empty), LlFieldType.Date_Localized)


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				Dim msg = m_Translate.GetSafeTranslationValue("Fehler während Auswertung der Einsatz-Felder für Druckausgabe der Rapporte.")
				m_UtilityUI.ShowErrorDialog(String.Format("{1}{0}{2}", vbNewLine, msg, ex.ToString))

				Return False
			End Try


			Return success

		End Function

		Private Function GetRPWeeklyData4Print(ByVal LL As ListLabel, ByVal reportData As RPPrintWeeklyData) As Boolean

			Dim success As Boolean = True

			Try

				LL.Core.LlDefineVariableExt("RPNr", ReplaceMissing(reportData.RPNr, 0), LlFieldType.Numeric)
				LL.Core.LlDefineVariableExt("Mo", ReplaceMissing(reportData.Montag, CDate("1.1.1900")), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("Di", ReplaceMissing(reportData.Dienstag, CDate("1.1.1900")), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("Mi", ReplaceMissing(reportData.Mittwoch, CDate("1.1.1900")), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("Do", ReplaceMissing(reportData.Donnerstag, CDate("1.1.1900")), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("Fr", ReplaceMissing(reportData.Freitag, CDate("1.1.1900")), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("Sa", ReplaceMissing(reportData.Samstag, CDate("1.1.1900")), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("So", ReplaceMissing(reportData.Sonntag, CDate("1.1.1900")), LlFieldType.Date_Localized)

				If reportData.Montag.HasValue Then
					LL.Core.LlDefineVariableExt("FirstDay", ReplaceMissing(reportData.Montag, String.Empty), LlFieldType.Date_Localized)
				ElseIf reportData.Dienstag.HasValue Then
					LL.Core.LlDefineVariableExt("FirstDay", ReplaceMissing(reportData.Dienstag, String.Empty), LlFieldType.Date_Localized)
				ElseIf reportData.Mittwoch.HasValue Then
					LL.Core.LlDefineVariableExt("FirstDay", ReplaceMissing(reportData.Mittwoch, String.Empty), LlFieldType.Date_Localized)
				ElseIf reportData.Donnerstag.HasValue Then
					LL.Core.LlDefineVariableExt("FirstDay", ReplaceMissing(reportData.Donnerstag, String.Empty), LlFieldType.Date_Localized)
				ElseIf reportData.Freitag.HasValue Then
					LL.Core.LlDefineVariableExt("FirstDay", ReplaceMissing(reportData.Freitag, String.Empty), LlFieldType.Date_Localized)
				ElseIf reportData.Samstag.HasValue Then
					LL.Core.LlDefineVariableExt("FirstDay", ReplaceMissing(reportData.Samstag, String.Empty), LlFieldType.Date_Localized)
				ElseIf reportData.Sonntag.HasValue Then
					LL.Core.LlDefineVariableExt("FirstDay", ReplaceMissing(reportData.Sonntag, String.Empty), LlFieldType.Date_Localized)

				Else
					LL.Core.LlDefineVariableExt("FirstDay", String.Empty, LlFieldType.Date_Localized)

				End If

				If reportData.Sonntag.HasValue Then
					LL.Core.LlDefineVariableExt("LastDay", ReplaceMissing(reportData.Sonntag, String.Empty), LlFieldType.Date_Localized)
				ElseIf reportData.Samstag.HasValue Then
					LL.Core.LlDefineVariableExt("LastDay", ReplaceMissing(reportData.Samstag, String.Empty), LlFieldType.Date_Localized)
				ElseIf reportData.Freitag.HasValue Then
					LL.Core.LlDefineVariableExt("LastDay", ReplaceMissing(reportData.Freitag, String.Empty), LlFieldType.Date_Localized)
				ElseIf reportData.Donnerstag.HasValue Then
					LL.Core.LlDefineVariableExt("LastDay", ReplaceMissing(reportData.Donnerstag, String.Empty), LlFieldType.Date_Localized)
				ElseIf reportData.Mittwoch.HasValue Then
					LL.Core.LlDefineVariableExt("LastDay", ReplaceMissing(reportData.Mittwoch, String.Empty), LlFieldType.Date_Localized)
				ElseIf reportData.Dienstag.HasValue Then
					LL.Core.LlDefineVariableExt("LastDay", ReplaceMissing(reportData.Dienstag, String.Empty), LlFieldType.Date_Localized)
				ElseIf reportData.Montag.HasValue Then
					LL.Core.LlDefineVariableExt("LastDay", ReplaceMissing(reportData.Montag, String.Empty), LlFieldType.Date_Localized)

				Else
					LL.Core.LlDefineVariableExt("LastDay", String.Empty, LlFieldType.Date_Localized)

				End If


				LL.Core.LlDefineVariableExt("Jahr", ReplaceMissing(reportData.Jahr, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Woche", ReplaceMissing(reportData.Woche, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("Monat", ReplaceMissing(reportData.Monat, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("PrintedDate", ReplaceMissing(reportData.PrintedDate, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("PrintedWeeks", ReplaceMissing(reportData.PrintedWeeks, String.Empty), LlFieldType.Text)


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))

				Dim msg = m_Translate.GetSafeTranslationValue("Fehler während Auswertung der Rapport-Felder für Druckausgabe der Rapporte.")
				m_UtilityUI.ShowErrorDialog(String.Format("{1}{0}{2}", vbNewLine, msg, ex.ToString))

				success = False

			Finally

			End Try

			Return success

		End Function

#End Region


#Region "Helpers"

		Sub InitLL(ByVal LL As ListLabel)
			Const LL_DLGBOXMODE_3DBUTTONS As Integer = 32768                       ' 0x8000
			Const LL_DLGBOXMODE_ALT10 As Integer = 11                              ' 0x000B
			Const LL_OPTION_INCLUDEFONTDESCENT As Integer = 79                     ' 79
			Const LL_OPTION_INCREMENTAL_PREVIEW As Integer = 135                   ' 135

			Const LL_OPTION_VARSCASESENSITIVE As Integer = 46                      ' 46
			Const LL_OPTION_IMMEDIATELASTPAGE As Integer = 64                      ' 64
			Const LL_OPTION_CONVERTCRLF As Integer = 21                            ' 21

			Const LL_OPTION_NOPARAMETERCHECK As Integer = 32                       ' 32
			Const LL_OPTION_XLATVARNAMES As Integer = 51                           ' 51

			Const LL_OPTION_ESC_CLOSES_PREVIEW As Integer = 102                    ' 102
			Const LL_OPTION_SUPERVISOR As Integer = 3                              ' 3
			Const LL_OPTION_UISTYLE As Integer = 99                                ' 99
			Const LL_OPTION_UISTYLE_OFFICE2003 As Integer = 2                      ' 2
			Const LL_OPTION_AUTOMULTIPAGE As Integer = 42                          ' 42
			Const LL_OPTION_SUPPORTPAGEBREAK As Integer = 10                       ' 10
			Const LL_OPTION_PRVZOOM_PERC As Integer = 25                           ' 25

			Try

				LL.LicensingInfo = ClsMainSetting.GetLL25LicenceInfo()
				LL.Language = LlLanguage.German

				LlCore.LlSetDlgboxMode(LL_DLGBOXMODE_3DBUTTONS + LL_DLGBOXMODE_ALT10)

				' beim LL13 muss ich es so machen...
				LL.Core.LlSetOption(LL_OPTION_INCLUDEFONTDESCENT, 0)
				LL.Core.LlSetOption(LL_OPTION_INCREMENTAL_PREVIEW, 0)

				' beim LL13 muss ich es so machen...
				LL.Core.LlSetOption(LL_OPTION_INCLUDEFONTDESCENT, m_PrintJobData.LLFontDesent)
				LL.Core.LlSetOption(LL_OPTION_INCREMENTAL_PREVIEW, m_PrintJobData.LLIncPrv)

				LL.Core.LlSetOption(LL_OPTION_VARSCASESENSITIVE, 0)

				LL.Core.LlSetOption(LL_OPTION_IMMEDIATELASTPAGE, 1)       ' Lastpage
				LL.Core.LlSetOption(LL_OPTION_CONVERTCRLF, 1)             ' Doppelte Zeilenumbruch

				LL.Core.LlSetOption(LL_OPTION_NOPARAMETERCHECK, m_PrintJobData.LLParamCheck)
				LL.Core.LlSetOption(LL_OPTION_XLATVARNAMES, m_PrintJobData.LLKonvertName)

				LL.Core.LlSetOption(LL_OPTION_ESC_CLOSES_PREVIEW, 1)
				LL.Core.LlSetOption(LL_OPTION_SUPERVISOR, 1)
				LL.Core.LlSetOption(LL_OPTION_UISTYLE, LL_OPTION_UISTYLE_OFFICE2003)
				LL.Core.LlSetOption(LL_OPTION_AUTOMULTIPAGE, 1)

				LL.Core.LlSetOption(LL_OPTION_SUPPORTPAGEBREAK, 1)
				LL.Core.LlSetOption(LL_OPTION_PRVZOOM_PERC, m_PrintJobData.LLZoomProz)

				LL.Core.LlPreviewSetTempPath(m_path.GetSpSTempFolder)
				LL.Core.LlSetPrinterDefaultsDir(m_path.GetPrinterHomeFolder)

			Catch ex As Exception
				m_Logger.LogError(String.Format("InitLL: {0}", ex.ToString))
			End Try

		End Sub

		Private Function SetLLVariable(ByVal LL As ListLabel) As Boolean
			Dim result As Boolean = True

			Try

				' Zusätzliche Variable einfügen
				LL.Variables.Add("AutorFName", m_InitializationData.UserData.UserFName)
				LL.Variables.Add("AutorLName", m_InitializationData.UserData.UserLName)


				result = result AndAlso GetMDUSData4Print(LL)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			End Try

			Return result

		End Function

		Private Function GetMDUSData4Print(ByVal LL As ListLabel) As Boolean
			Dim result As Boolean = True
			Dim strFullFilename As String = String.Format("{0}Bild_{1}_{2}.JPG", m_path.GetSpS2DeleteHomeFolder, m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr)

			Dim data = m_ListingDatabaseAccess.LoadUserAndMandantData(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserFName, m_InitializationData.UserData.UserLName)
			If data Is Nothing Then
				m_Logger.LogError(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden."))

				Return False

			End If
			LL.Variables.Add("MDName", data.USMDname)
			LL.Variables.Add("MDName2", data.USMDname2)
			LL.Variables.Add("MDName3", data.USMDname3)
			LL.Variables.Add("MDPostfach", data.USMDPostfach)
			LL.Variables.Add("MDStrasse", data.USMDStrasse)
			LL.Variables.Add("MDPLZ", data.USMDPlz)
			LL.Variables.Add("MDOrt", data.USMDOrt)
			LL.Variables.Add("MDLand", data.USMDLand)

			LL.Variables.Add("MDTelefax", data.USMDTelefax)
			LL.Variables.Add("MDTelefon", data.USMDTelefon)
			LL.Variables.Add("MDDTelefon", data.USMDDTelefon)
			LL.Variables.Add("MDHomepage", data.USMDHomepage)
			LL.Variables.Add("MDeMail", data.USMDeMail)

			LL.Variables.Add("USNachName", data.USNachname)
			LL.Variables.Add("USVorname", data.USVorname)

			LL.Variables.Add("USTitle1", data.USTitel_1)
			LL.Variables.Add("USTitle2", data.USTitel_2)
			LL.Variables.Add("USAbteilung", data.USAbteilung)
			If File.Exists(strFullFilename) Then
				LL.Variables.Add("USSignFilename", strFullFilename)
			Else
				Dim success = Not data.UserSign Is Nothing AndAlso m_Utility.WriteFileBytes(strFullFilename, data.UserSign)
				LL.Variables.Add("USSignFilename", If(success, strFullFilename, String.Empty))
			End If


			Return result

		End Function

#End Region


	End Class


	Public Class ReportPrintData

		Public Property m_Repportdata As IEnumerable(Of RPPrintWeeklyData)

		Public Property frmhwnd As String
		Public Property LastWeek As Integer
		Public Property FirstWeek As Integer
		Public Property PrintYear As Integer
		Public Property USSignFileName As String

		Public Property ListSortBez As String
		Public Property ListFilterBez As List(Of String)

		Public Property ShowAsDesign As Boolean
		Public Property PrintJobNumber As String
		Public Property ExportPath As String
		Public Property ExportPrintInFiles As Boolean?
		Public Property ListOfExportedFiles As List(Of String)
		Public Property ExportFinalFilename As String
		Public Property WOSSendValueEnum As WOSZVSENDValue

		Public Property CountOfTestPrint As Integer

	End Class


End Namespace