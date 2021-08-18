
Imports System.IO
Imports combit.ListLabel25

Imports DevExpress.XtraSplashScreen
Imports DevExpress.Pdf
Imports SP.Internal.Automations.WOSUtility.DataObjects
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng


Namespace EmployeePrint

	Partial Class PrintEmployeeData


#Region "Public properties"

		Public Property PrintARGBData As ARGBPrintData
		Public Property PageARGBData As ARGBPageData

#End Region


		Public Function PrintEmployeeARGBData() As PrintResult
			Dim success As Boolean = True
			Dim result As PrintResult = New PrintResult With {.Printresult = True}
			Dim i As Integer = 0

			PrintARGBData.ExportedFiles = New List(Of String)
			m_EmployeeLanguage = PrintARGBData.m_EmployeeData.Language.Substring(0, 2).ToUpper
			If String.IsNullOrWhiteSpace(PrintARGBData.ExportPath) Then PrintARGBData.ExportPath = m_path.GetSpSMAHomeFolder
			m_CurrentExportPrintInFiles = PrintARGBData.ExportPrintInFiles.GetValueOrDefault(False)
			Dim jobNumber As String = PrintARGBData.PrintJobNumber
			success = success AndAlso LoadPrintJobData(jobNumber)
			If Not success Then Return New PrintResult With {.Printresult = False, .PrintresultMessage = "Vorlage wurde nicht gefunden!"}

			If PrintARGBData.ShowAsDesign Then
				success = success AndAlso ShowAssignedARGBInDesign()

				Return result

			Else
				success = success AndAlso PrintAssignedARGBList()

				If success AndAlso Not m_CurrentExportPrintInFiles AndAlso PrintARGBData.WOSSendValueEnum <> WOSZVSENDValue.PrintOtherSendWOS Then
					m_CurrentExportPrintInFiles = True
					success = success AndAlso PrintAssignedARGBList()

					m_CurrentExportPrintInFiles = False
				End If
				If success AndAlso PrintARGBData.ExportedFiles.Count > 0 Then SaveARGBFileToMAHistoryDocDb(PrintARGBData.ExportedFiles(0))
				If Not success Then result.Printresult = False


				If Not m_CurrentExportPrintInFiles AndAlso (PrintARGBData.m_EmployeeData.Send2WOS.GetValueOrDefault(False) AndAlso Not PrintARGBData.WOSSendValueEnum = WOSZVSENDValue.PrintWithoutSending) Then
					'm_CurrentExportPrintInFiles = True
					'success = success AndAlso PrintAssignedARGBList()

					If success AndAlso PrintARGBData.ExportedFiles.Count > 0 Then
						Dim wosResult = TransferCustomerARGBDataIntoWOS(PrintARGBData.m_EmployeeData)
						result.WOSresult = wosResult.Value
					End If
				End If

			End If

			SplashScreenManager.CloseForm(False)

			Return result

		End Function


#Region "Private Methodes"

		Private Function ShowAssignedARGBInDesign() As Boolean
			Dim result As Boolean = True

			Try
				If PrintARGBData.m_EmployeeData Is Nothing Then
					m_Logger.LogWarning("Keine Daten wurden gefunden.")

					Return False
				End If

				InitLL(LL)
				LL.Variables.Clear()
				LL.Fields.Clear()

				result = result AndAlso SetLLARGBVariable(LL)
				result = result AndAlso DefineARGBPagesData(LL, PrintARGBData.m_EmployeeData)
				If Not result Then
					m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden."))

					Return False
				End If

				result = result AndAlso DefineARGBESData(LL, PrintARGBData.m_ESData(0))
				If Not result Then
					m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Es sind keine Einsatzdaten vorhanden."))

					Return False
				End If

				result = result AndAlso DefineLohnkontiData(LL, PrintARGBData.m_PayrollLohnkontidata(0))
				If Not result Then
					m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Es sind noch keine Lohndaten vorhanden."))

					Return False
				End If


				LL.Variables.Add("WOSDoc", 1)

				If result Then
					SplashScreenManager.CloseForm(False)
					LL.Core.LlDefineLayout(CType(PrintARGBData.frmhwnd, IntPtr), m_PrintJobData.LLDocLabel,
																	 If(m_PrintJobData.LLDocName.ToUpper.EndsWith(".LST".ToUpper), LlProject.List, LlProject.Card),
																	 m_PrintJobData.LLDocName)
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

		Function PrintAssignedARGBList() As Boolean
			Const LL_PRNOPT_COPIES_SUPPORTED As Integer = 3
			Dim result As Boolean = True
			Dim doExport As Boolean = m_CurrentExportPrintInFiles OrElse PrintARGBData.WOSSendValueEnum = WOSZVSENDValue.PrintOtherSendWOS
			Dim employeedata = PrintARGBData.m_EmployeeData
			Dim tempFileName = System.IO.Path.GetTempFileName()
			tempFileName = System.IO.Path.ChangeExtension(tempFileName, "PDF")

			Dim exportFileName = New FileInfo(tempFileName)
			tempFileName = exportFileName.Name

			Try
				If m_StartPrintInLLDebug Then
					LL.Debug = LlDebug.LogToFile
					LL.Debug = LlDebug.Enabled
				End If
			Catch ex As Exception
				m_Logger.LogError(String.Format("EnablingLLDebuging: {0}", ex.ToString))

			End Try

			Try
				If employeedata Is Nothing Then
					m_Logger.LogWarning("Keine Daten wurden gefunden.")

					Return False
				End If

				InitLL(LL)
				LL.Variables.Clear()
				LL.Fields.Clear()

				result = result AndAlso SetLLARGBVariable(LL)
				result = result AndAlso DefineARGBPagesData(LL, PrintARGBData.m_EmployeeData)
				If Not result Then
					m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden."))

					Return False
				End If

				result = result AndAlso DefineARGBESData(LL, PrintARGBData.m_ESData(0))
				If Not result Then
					m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Es sind keine Einsatzdaten vorhanden."))

					Return False
				End If

				result = result AndAlso DefineLohnkontiData(LL, PrintARGBData.m_PayrollLohnkontidata(0))
				If Not result Then
					m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Es sind noch keine Lohndaten vorhanden."))

					Return False
				End If

				LL.Variables.Add("WOSDoc", 0)

				SplashScreenManager.CloseForm(False)
				LL.Core.LlPrintWithBoxStart(If(m_PrintJobData.LLDocName.ToUpper.EndsWith(".LST".ToUpper),
																				 LlProject.List, LlProject.Card),
																				 m_PrintJobData.LLDocName, LlPrintMode.Export,
																				 If(doExport, LlBoxType.None, LlBoxType.StandardAbort),
																				 CType(PrintARGBData.frmhwnd, IntPtr), m_PrintJobData.LLDocLabel)

				If doExport Then
					LL.Variables.Add("WOSDoc", 1)

					LL.Core.LlPrintSetOptionString(LlPrintOptionString.Export, "PDF")
					LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.File", tempFileName)
					LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.Path", PrintARGBData.ExportPath)
					LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.Quiet", "1")

					LL.Core.LlPrintSetOption(LlPrintOption.Dialog_DestinationMask, 2)

				Else
					LL.Core.LlPrintSetOption(LlPrintOption.Copies, Math.Max(1, m_NumberOfCopies.GetValueOrDefault(1)))
					LL.Core.LlPrintSetOption(LL_PRNOPT_COPIES_SUPPORTED, LL.Core.LlPrintGetOption(LlPrintOption.Copies))

					LL.Core.LlPrintOptionsDialog(CType(PrintARGBData.frmhwnd, IntPtr), m_PrintJobData.PrintBoxHeader)
				End If



				Dim TargetFormat As String = LL.Core.LlPrintGetOptionString(LlPrintOptionString.Export)

				While Not LL.Core.LlPrint()
				End While

				If m_PrintJobData.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then

					For Each itm In PrintARGBData.m_PayrollLohnkontidata
						'pass Data for current record
						'result = result AndAlso DefineARGBESData(LL, itm)
						result = result AndAlso DefineLohnkontiData(LL, itm)
						'result = result AndAlso DefineARGBESData(LL, PrintARGBData.m_ESData(0))

						While LL.Core.LlPrintFields() = LlConstants.WrnRepeatData
							LL.Core.LlPrint()
						End While

					Next

					While LL.Core.LlPrintFieldsEnd() = LlConstants.WrnRepeatData
					End While

				End If

				LL.Core.LlPrintEnd(0)

				If TargetFormat = "PRV" Then
					SplashScreenManager.CloseForm(False)
					LL.Core.LlPreviewDisplay(m_PrintJobData.LLDocName, m_path.GetSpSTempFolder, CType(PrintARGBData.frmhwnd, IntPtr))
					LL.Core.LlPreviewDeleteFiles(m_PrintJobData.LLDocName, m_path.GetSpSTempFolder)

					Return False
				End If
				If doExport Then PrintARGBData.ExportedFiles.Add(Path.Combine(PrintARGBData.ExportPath, tempFileName))


			Catch LlException As LL_User_Aborted_Exception
				Return False

			Catch LlException As Exception
				m_Logger.LogError(String.Format("LlException ({0} >>> {1}): {2}", PrintARGBData.PrintJobNumber, m_PrintJobData.LLDocName, LlException.ToString))
				result = False

			Finally

			End Try


			Return result

		End Function

		Private Function SetLLARGBVariable(ByVal LL As ListLabel) As Boolean
			Dim result As Boolean = True

			' Zusätzliche Variable einfügen
			LL.Variables.Add("AutorFName", m_InitializationData.UserData.UserFName)
			LL.Variables.Add("AutorLName", m_InitializationData.UserData.UserLName)


			result = result AndAlso GetMDUSData4Print(LL)
			result = result AndAlso DefineEmployeeARGBData(LL)

			Return result

		End Function

		Private Function DefineEmployeeARGBData(ByVal LL As ListLabel) As Boolean
			Dim result As Boolean = True

			LL.Core.LlDefineVariableExt("SelMANr", ReplaceMissing(PrintARGBData.m_EmployeeData.EmployeeNumber, String.Empty), LlFieldType.Text)
			LL.Core.LlDefineVariableExt("MAName", ReplaceMissing(PrintARGBData.m_EmployeeAddressData.EmployeeFullname, String.Empty), LlFieldType.Text)
			LL.Core.LlDefineVariableExt("PostOfficeBox", ReplaceMissing(PrintARGBData.m_EmployeeAddressData.PostOfficeBox, String.Empty), LlFieldType.Text)
			LL.Core.LlDefineVariableExt("Strasse", ReplaceMissing(PrintARGBData.m_EmployeeAddressData.Street, String.Empty), LlFieldType.Text)
			LL.Core.LlDefineVariableExt("Adresse", ReplaceMissing(PrintARGBData.m_EmployeeAddressData.EmployeeAddress, String.Empty), LlFieldType.Text)
			LL.Core.LlDefineVariableExt("CivilStatus", ReplaceMissing(PrintARGBData.m_EmployeeData.CivilStatus, String.Empty), LlFieldType.Text)
			LL.Core.LlDefineVariableExt("AHVNr", ReplaceMissing(PrintARGBData.m_EmployeeData.AHV_Nr, String.Empty), LlFieldType.Text)
			LL.Core.LlDefineVariableExt("AHV_Nr_New", ReplaceMissing(PrintARGBData.m_EmployeeData.AHV_Nr_New, String.Empty), LlFieldType.Text)
			LL.Core.LlDefineVariableExt("GebDat", ReplaceMissing(PrintARGBData.m_EmployeeData.Birthdate, String.Empty), LlFieldType.Date_Localized)

			Return result

		End Function

		Private Function DefineARGBPagesData(ByVal LL As ListLabel, ByVal data As EmployeeMasterData) As Boolean
			Dim result As Boolean = True

			Try

				LL.Core.LlDefineVariableExt("DatumVon", ReplaceMissing(PageARGBData.DateFrom, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("DatumBis", ReplaceMissing(PageARGBData.DateTo, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("AHVKasse", ReplaceMissing(PageARGBData.Compensationfund, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("BURNr", ReplaceMissing(PageARGBData.CompanyBURNumber, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("CompanyUnfallfund", ReplaceMissing(PageARGBData.CompanyUnfallfund, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("CompanyUnfallfundNumber", ReplaceMissing(PageARGBData.CompanyUnfallfundNumber, String.Empty), LlFieldType.Text)

				LL.Core.LlDefineVariableExt("txt9_1", ReplaceMissing(PageARGBData.txt9_1, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("de9_1", ReplaceMissing(PageARGBData.de9_1, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("de9_2", ReplaceMissing(PageARGBData.de9_2, String.Empty), LlFieldType.Date_Localized)

				LL.Core.LlDefineVariableExt("op10", ReplaceMissing(PageARGBData.op10, 99), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("txt11", ReplaceMissing(PageARGBData.txt11, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("op12", ReplaceMissing(PageARGBData.op12, 2), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("txt12", ReplaceMissing(PageARGBData.txt12, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("de12_1", ReplaceMissing(PageARGBData.de12_1, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("de12_2", ReplaceMissing(PageARGBData.de12_2, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("txt13", ReplaceMissing(PageARGBData.txt13, String.Empty), LlFieldType.Text)

				LL.Core.LlDefineVariableExt("de14", ReplaceMissing(PageARGBData.de14, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("de15", ReplaceMissing(PageARGBData.de15, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("txt17", ReplaceMissing(PageARGBData.txt17, 0), LlFieldType.Numeric_Localized)

				LL.Core.LlDefineVariableExt("op19_1", ReplaceMissing(PageARGBData.op19_1, 1), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("op19_2", ReplaceMissing(PageARGBData.op19_2, 1), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("txt19", ReplaceMissing(PageARGBData.txt19, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("op20_1", ReplaceMissing(PageARGBData.op20_1, 1), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("op20_2", ReplaceMissing(PageARGBData.op20_2, 1), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("txt20_1", ReplaceMissing(PageARGBData.txt20_1, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("txt20_2", ReplaceMissing(PageARGBData.txt20_2, 0), LlFieldType.Numeric_Localized)

				LL.Core.LlDefineVariableExt("op21_1", ReplaceMissing(PageARGBData.op21_1, 1), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("op21_2", ReplaceMissing(PageARGBData.op21_2, 1), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("txt21", ReplaceMissing(PageARGBData.txt21, 0), LlFieldType.Numeric_Localized)

				LL.Core.LlDefineVariableExt("op22", ReplaceMissing(PageARGBData.op22, 1), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("txt22", ReplaceMissing(PageARGBData.txt22, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("op23", ReplaceMissing(PageARGBData.op23, 1), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("txt23", ReplaceMissing(PageARGBData.txt23, String.Empty), LlFieldType.Text)

				LL.Core.LlDefineVariableExt("de24_1", ReplaceMissing(PageARGBData.de24_1, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("de24_2", ReplaceMissing(PageARGBData.de24_2, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("de25_1", ReplaceMissing(PageARGBData.de25_1, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("de25_2", ReplaceMissing(PageARGBData.de25_2, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("de26_1", ReplaceMissing(PageARGBData.de26_1, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("de26_2", ReplaceMissing(PageARGBData.de26_2, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("de27_1", ReplaceMissing(PageARGBData.de27_1, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("de27_2", ReplaceMissing(PageARGBData.de27_2, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("de28_1", ReplaceMissing(PageARGBData.de28_1, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("de28_2", ReplaceMissing(PageARGBData.de28_2, String.Empty), LlFieldType.Date_Localized)

				LL.Core.LlDefineVariableExt("ALKNumber", ReplaceMissing(PrintARGBData.m_EmployeeContactCommData.ALKNumber, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("ALKName", ReplaceMissing(PrintARGBData.m_EmployeeContactCommData.ALKName, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("ALKPOBox", ReplaceMissing(PrintARGBData.m_EmployeeContactCommData.ALKPOBox, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("ALKStreet", ReplaceMissing(PrintARGBData.m_EmployeeContactCommData.ALKStreet, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("ALKPostcode", ReplaceMissing(PrintARGBData.m_EmployeeContactCommData.ALKPostcode, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("ALKLocation", ReplaceMissing(PrintARGBData.m_EmployeeContactCommData.ALKLocation, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("ALKTelefon", ReplaceMissing(PrintARGBData.m_EmployeeContactCommData.ALKTelephone, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("ALKTelefax", ReplaceMissing(PrintARGBData.m_EmployeeContactCommData.ALKTelefax, String.Empty), LlFieldType.Text)

				LL.Core.LlDefineVariableExt("Month_6", ReplaceMissing(PageARGBData.m_CountWorked6Month, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("Month_12", ReplaceMissing(PageARGBData.m_CountWorked12Month, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("Month_15", ReplaceMissing(PageARGBData.m_CountWorked15Month, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("Month_24", ReplaceMissing(PageARGBData.m_CountWorked24Month, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("txt16_1", ReplaceMissing(PageARGBData.txt16_1, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("txt16_2", ReplaceMissing(PageARGBData.txt16_2, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("txt16_3", ReplaceMissing(PageARGBData.txt16_3, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("txt16_4", ReplaceMissing(PageARGBData.txt16_4, 0), LlFieldType.Numeric_Localized)

				LL.Core.LlDefineVariableExt("txt18_1", ReplaceMissing(PageARGBData.txt18_1, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("txt18_2", ReplaceMissing(PageARGBData.txt18_2, 0), LlFieldType.Numeric_Localized)


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				result = False

			End Try


			Return result

		End Function

		Private Function DefineARGBESData(ByVal LL As ListLabel, ByVal data As ZVESData) As Boolean
			Dim result As Boolean = True

			Try
				Dim i As Integer = 1

				For i = 1 To 50
					LL.Core.LlDefineVariableExt(String.Format("ESNR_{0}", i), 0, LlFieldType.Numeric_Localized)
					LL.Core.LlDefineVariableExt(String.Format("ES_Ab_{0}", i), String.Empty, LlFieldType.Date_Localized)
					LL.Core.LlDefineVariableExt(String.Format("ES_Ende_{0}", i), String.Empty, LlFieldType.Date_Localized)
					LL.Core.LlDefineVariableExt(String.Format("Stundenlohn_{0}", i), 0, LlFieldType.Numeric_Localized)
					LL.Core.LlDefineVariableExt(String.Format("Lohn13Proz_{0}", i), 0, LlFieldType.Numeric_Localized)
					LL.Core.LlDefineVariableExt(String.Format("FeierProz_{0}", i), 0, LlFieldType.Numeric_Localized)
					LL.Core.LlDefineVariableExt(String.Format("FerienProz_{0}", i), 0, LlFieldType.Numeric_Localized)
					LL.Core.LlDefineVariableExt(String.Format("GrundLohn_{0}", i), 0, LlFieldType.Numeric_Localized)
					LL.Core.LlDefineVariableExt(String.Format("ES_Als_{0}", i), String.Empty, LlFieldType.Text)
					LL.Core.LlDefineVariableExt(String.Format("GAVText_{0}", i), String.Empty, LlFieldType.Text)
					LL.Core.LlDefineVariableExt(String.Format("GAVNr_{0}", i), 0, LlFieldType.Numeric_Localized)
					LL.Core.LlDefineVariableExt(String.Format("Arbzeit_{0}", i), String.Empty, LlFieldType.Text)
				Next

				i = 1
				For Each itm In PrintARGBData.m_ESData

					LL.Core.LlDefineVariableExt(String.Format("ESNR_{0}", i), ReplaceMissing(itm.ESNR, 0), LlFieldType.Numeric_Localized)
					LL.Core.LlDefineVariableExt(String.Format("ES_Ab_{0}", i), ReplaceMissing(itm.ES_Ab, String.Empty), LlFieldType.Date_Localized)
					LL.Core.LlDefineVariableExt(String.Format("ES_Ende_{0}", i), ReplaceMissing(itm.ES_Ende, String.Empty), LlFieldType.Date_Localized)
					LL.Core.LlDefineVariableExt(String.Format("Stundenlohn_{0}", i), ReplaceMissing(itm.StundenLohn, 0), LlFieldType.Numeric_Localized)
					LL.Core.LlDefineVariableExt(String.Format("Lohn13Proz_{0}", i), ReplaceMissing(itm.Lohn13Proz, 0), LlFieldType.Numeric_Localized)
					LL.Core.LlDefineVariableExt(String.Format("FeierProz_{0}", i), ReplaceMissing(itm.FeierProz, 0), LlFieldType.Numeric_Localized)
					LL.Core.LlDefineVariableExt(String.Format("FerienProz_{0}", i), ReplaceMissing(itm.FerienProz, 0), LlFieldType.Numeric_Localized)
					LL.Core.LlDefineVariableExt(String.Format("GrundLohn_{0}", i), ReplaceMissing(itm.GrundLohn, 0), LlFieldType.Numeric_Localized)
					LL.Core.LlDefineVariableExt(String.Format("ES_Als_{0}", i), ReplaceMissing(itm.ES_Als, String.Empty), LlFieldType.Text)
					LL.Core.LlDefineVariableExt(String.Format("GAVText_{0}", i), ReplaceMissing(itm.GAVGruppe0, String.Empty), LlFieldType.Text)
					LL.Core.LlDefineVariableExt(String.Format("GAVNr_{0}", i), ReplaceMissing(itm.GAVNr, 0), LlFieldType.Numeric_Localized)
					LL.Core.LlDefineVariableExt(String.Format("Arbzeit_{0}", i), ReplaceMissing(itm.Arbzeit, String.Empty), LlFieldType.Text)

					i += 1
				Next


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				result = False

			End Try


			Return result

		End Function

		Private Function DefineLohnkontiData(ByVal LL As ListLabel, ByVal data As ARGBPayrollData) As Boolean
			Dim result As Boolean = True

			Try
				LL.Core.LlDefineFieldExt("LANr", ReplaceMissing(data.LANr, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineFieldExt("RPText", ReplaceMissing(data.RPText, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineFieldExt("Month1", ReplaceMissing(data.Month1, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineFieldExt("Month2", ReplaceMissing(data.Month2, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineFieldExt("Month3", ReplaceMissing(data.Month3, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineFieldExt("Month4", ReplaceMissing(data.Month4, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineFieldExt("Month5", ReplaceMissing(data.Month5, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineFieldExt("Month6", ReplaceMissing(data.Month6, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineFieldExt("Month7", ReplaceMissing(data.Month7, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineFieldExt("Month8", ReplaceMissing(data.Month8, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineFieldExt("Month9", ReplaceMissing(data.Month9, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineFieldExt("Month10", ReplaceMissing(data.Month10, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineFieldExt("Month11", ReplaceMissing(data.Month11, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineFieldExt("Month12", ReplaceMissing(data.Month12, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineFieldExt("KumulativAmount", ReplaceMissing(data.KumulativAmount, 0), LlFieldType.Numeric_Localized)


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				result = False

			End Try


			Return result

		End Function

		'Private Function DefineARGBPayrollData(ByVal LL As ListLabel, ByVal data As ARGBPayrollData) As Boolean
		'	Dim result As Boolean = True

		'	Try

		'		LL.Core.LlDefineVariableExt("LANr", ReplaceMissing(data.LANr, 0), LlFieldType.Numeric_Localized)
		'		LL.Core.LlDefineVariableExt("Bezeichnung", ReplaceMissing(data.RPText, String.Empty), LlFieldType.Text)
		'		LL.Core.LlDefineVariableExt("Monat1", ReplaceMissing(data.Month1, 0), LlFieldType.Numeric_Localized)
		'		LL.Core.LlDefineVariableExt("Monat2", ReplaceMissing(data.Month2, 0), LlFieldType.Numeric_Localized)
		'		LL.Core.LlDefineVariableExt("Monat3", ReplaceMissing(data.Month3, 0), LlFieldType.Numeric_Localized)
		'		LL.Core.LlDefineVariableExt("Monat4", ReplaceMissing(data.Month4, 0), LlFieldType.Numeric_Localized)
		'		LL.Core.LlDefineVariableExt("Monat5", ReplaceMissing(data.Month5, 0), LlFieldType.Numeric_Localized)
		'		LL.Core.LlDefineVariableExt("Monat6", ReplaceMissing(data.Month6, 0), LlFieldType.Numeric_Localized)
		'		LL.Core.LlDefineVariableExt("Monat7", ReplaceMissing(data.Month7, 0), LlFieldType.Numeric_Localized)
		'		LL.Core.LlDefineVariableExt("Monat8", ReplaceMissing(data.Month8, 0), LlFieldType.Numeric_Localized)
		'		LL.Core.LlDefineVariableExt("Monat9", ReplaceMissing(data.Month9, 0), LlFieldType.Numeric_Localized)
		'		LL.Core.LlDefineVariableExt("Monat10", ReplaceMissing(data.Month10, 0), LlFieldType.Numeric_Localized)
		'		LL.Core.LlDefineVariableExt("Monat11", ReplaceMissing(data.Month11, 0), LlFieldType.Numeric_Localized)
		'		LL.Core.LlDefineVariableExt("Monat12", ReplaceMissing(data.Month12, 0), LlFieldType.Numeric_Localized)


		'	Catch ex As Exception
		'		m_Logger.LogError(ex.ToString)

		'		result = False

		'	End Try


		'	Return result

		'End Function


#Region "WOS-Methods"

		Function TransferCustomerARGBDataIntoWOS(ByVal employeeData As EmployeeMasterData) As WOSSendResult
			Dim result As WOSSendResult = New WOSSendResult With {.Value = False, .Message = "Initialisierung..."}
			Dim success As Boolean = True
			If employeeData.Send2WOS AndAlso Not PrintARGBData.WOSSendValueEnum = WOSZVSENDValue.PrintWithoutSending Then
				Dim wos = New SP.Internal.Automations.WOSUtility.EmployeeExport(m_InitializationData)

				Dim wosSetting = New WOSSendSetting

				wosSetting.EmployeeNumber = employeeData.EmployeeNumber
				wosSetting.EmployeeGuid = employeeData.Transfered_Guid
				wosSetting.DocumentArtEnum = WOSSendSetting.DocumentArt.Arbeitgeberbescheinigung

				Dim pdfFilename As String = PrintARGBData.ExportedFiles(0)
				If PrintARGBData.ExportedFiles.Count > 1 Then
					Dim pdfDocument As New PdfDocumentProcessor()
					pdfDocument.LoadDocument(pdfFilename)

					For i As Integer = 1 To PrintARGBData.ExportedFiles.Count - 1
						pdfDocument.AppendDocument(PrintARGBData.ExportedFiles(i))
						pdfDocument.SaveDocument(pdfFilename)
					Next
					pdfDocument.CloseDocument()
				End If

				' Arbeitgeberbescheinigung (Duplex): 01.01.2016 - 31.12.2016
				wosSetting.DocumentInfo = String.Format("Arbeitgeberbescheinigung (Duplex): {0:d} - {1:d}", PageARGBData.DateFrom, PageARGBData.DateTo)
				Dim fileByte() = m_Utility.LoadFileBytes(pdfFilename)
				wosSetting.ScanDoc = fileByte
				Dim scanfileinfo = New FileInfo(pdfFilename)
				wosSetting.ScanDocName = scanfileinfo.Name

				wos.WOSSetting = wosSetting

				If success Then result = wos.TransferEmployeeDocumentDataToWOS(True)

				PrintARGBData.ExportedFiles.Clear()
			End If


			Return result

		End Function


#End Region


#End Region


#Region "Helpers"

		Private Function SaveARGBFileToMAHistoryDocDb(ByVal strFullFilename As String) As Boolean
			Dim success As Boolean = True
			Dim strDocInfo As String = String.Format("{0:d} / {1:d}", ReplaceMissing(PageARGBData.DateFrom, String.Empty), ReplaceMissing(PageARGBData.DateTo, String.Empty))
			Dim strDocArt As String = "Arbeitgeberbescheinigung Duplex"

			Try

				Dim data = New EmployeePrintedDocProperty
				data.createdfrom = m_InitializationData.UserData.UserFullName
				data.createdon = Now
				data.docname = String.Format("{0} {1}", strDocArt, strDocInfo)
				data.manr = PrintARGBData.m_EmployeeData.EmployeeNumber
				data.scandoc = m_Utility.LoadFileBytes(strFullFilename)
				data.username = m_InitializationData.UserData.UserFullName

				success = success AndAlso m_ListingDatabaseAccess.AddAssignedPrintedDocumentInForEmployee(data)


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))

				Return False
			End Try


			Return success
		End Function


#End Region

	End Class


	Public Class ARGBPrintData

		Public Property frmhwnd As String
		Public Property m_EmployeeAddressData As EmployeeSAddressData
		Public Property m_EmployeeData As EmployeeMasterData
		Public Property m_EmployeeContactCommData As EmployeeContactComm
		Public Property m_ESData As IEnumerable(Of ZVESData)
		Public Property m_PayrollLastFourYearsAHVdata As IEnumerable(Of ARGBAHVPayrollData)
		Public Property m_PayrollLohnkontidata As IEnumerable(Of ARGBPayrollData)

		Public Property ShowAsDesign As Boolean
		Public Property PrintJobNumber As String
		Public Property ExportPath As String
		Public Property ExportPrintInFiles As Boolean?
		Public Property ExportedFiles As List(Of String)
		Public Property WOSSendValueEnum As WOSZVSENDValue

	End Class


	Public Class ARGBPageData

		Public Property DateFrom As Date
		Public Property DateTo As Date
		Public Property Compensationfund As String
		Public Property CompanyBURNumber As String
		Public Property txtBVGfund As String
		Public Property CompanyUnfallfund As String
		Public Property CompanyUnfallfundNumber As String

		Public Property txt9_1 As String
		Public Property de9_1 As Date?
		Public Property de9_2 As Date?
		Public Property op10 As Integer?
		Public Property txt11 As String
		Public Property op12 As Integer?
		Public Property txt12 As String
		Public Property de12_1 As Date?
		Public Property de12_2 As Date?
		Public Property txt13 As String
		Public Property de14 As Date?
		Public Property de15 As Date?


		Public Property m_CountWorked6Month As Integer?
		Public Property m_CountWorked12Month As Integer?
		Public Property m_CountWorked15Month As Integer?
		Public Property m_CountWorked24Month As Integer?
		Public Property txt16_1 As Decimal?
		Public Property txt16_2 As Decimal?
		Public Property txt16_3 As Decimal?
		Public Property txt16_4 As Decimal?
		Public Property txt17 As Decimal?
		Public Property txt18_1 As Decimal?
		Public Property txt18_2 As Decimal?

		Public Property op19_1 As Integer?
		Public Property op19_2 As Integer?
		Public Property txt19 As Decimal?
		Public Property op20_1 As Integer?
		Public Property op20_2 As Integer?
		Public Property txt20_1 As Decimal?
		Public Property txt20_2 As Decimal?
		Public Property op21_1 As Integer?
		Public Property op21_2 As Integer?
		Public Property txt21 As Decimal?
		Public Property op22 As Integer?
		Public Property txt22 As String
		Public Property op23 As Integer?
		Public Property txt23 As String

		Public Property de24_1 As Date?
		Public Property de24_2 As Date?
		Public Property de25_1 As Date?
		Public Property de25_2 As Date?
		Public Property de26_1 As Date?
		Public Property de26_2 As Date?
		Public Property de27_1 As Date?
		Public Property de27_2 As Date?
		Public Property de28_1 As Date?
		Public Property de28_2 As Date?


	End Class

End Namespace


