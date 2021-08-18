
Imports System.IO
Imports combit.ListLabel25
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Listing.DataObjects

Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects
Imports DevExpress.XtraSplashScreen
Imports DevExpress.Pdf
Imports System.Drawing.Printing

Imports SP.Internal.Automations.WOSUtility.DataObjects
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng


Namespace EmployeePrint

	Public Class PrintEmployeeData

		Implements IDisposable
		Protected disposed As Boolean = False


#Region "private fields"

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

		''' <summary>
		''' The Listing data access object.
		''' </summary>
		Private m_ListingDatabaseAccess As IListingDatabaseAccess

		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As SP.Infrastructure.Utility


		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI

		''' <summary>
		''' The mandant.
		''' </summary>
		Private m_MandantData As Mandant

		Private m_path As SPProgUtility.ProgPath.ClsProgPath
		Private m_MandantSettingsXml As SPProgUtility.CommonXmlUtility.SettingsXml

		Private m_MandantSetting As String
		Private m_SonstigesSetting As String
		Private m_InvoiceSetting As String
		Private m_SuvaSetting As String
		Private m_AHVSetting As String
		Private m_FAKSetting As String

		Private m_StartPrintInLLDebug As Boolean
		Private m_CurrentExportPrintInFiles As Boolean

		Private LL As ListLabel

		Private m_PrintJobData As PrintJobData
		Private m_invoiceData As List(Of InvoiceData)
		Private m_currentEmployeeData As EmployeeMasterData

		Private m_CurrentCustomerNumber As Integer?
		Private m_EmployeeLanguage As String
		Private m_NumberOfCopies As Integer?

		Private m_pdfPrinterSettings As PrinterSettings


#End Region


#Region "Public properties"

		Public Property PrintData As ZVPrintData
		Public Property PageData As ZVPageData

#End Region


#Region "private consts"

		Private Const MANDANT_XML_MAIN_KEY As String = "MD_{0}/"
		Private Const MANDANT_XML_SETTING_SPUTNIK_INVOICE_SETTING As String = "MD_{0}/Debitoren"
		Private Const MANDANT_XML_SETTING_SPUTNIK_SONSTIGES_SETTING As String = "MD_{0}/Sonstiges"
		Private Const MANDANT_XML_SETTING_SPUTNIK_AHV_SETTING As String = "MD_{0}/AHV-Daten"
		Private Const MANDANT_XML_SETTING_SPUTNIK_SUVA_SETTING As String = "MD_{0}/SUVA-Daten"
		Private Const MANDANT_XML_SETTING_SPUTNIK_FAK_SETTING As String = "MD_{0}/Fak-Daten"

#End Region


#Region "Constructor"

		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			m_InitializationData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
			m_MandantData = New Mandant
			m_path = New SPProgUtility.ProgPath.ClsProgPath
			m_UtilityUI = New UtilityUI
			m_Utility = New SP.Infrastructure.Utility

			m_CommonDatabaseAccess = New CommonDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)
			m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)
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


		Public Function PrintEmployeeZVData() As PrintResult
			Dim success As Boolean = True
			Dim result As PrintResult = New PrintResult With {.Printresult = True}
			Dim i As Integer = 0

			PrintData.ExportedFiles = New List(Of String)
			m_EmployeeLanguage = PrintData.m_EmployeeData.Language.Substring(0, 2).ToUpper
			If String.IsNullOrWhiteSpace(PrintData.ExportPath) Then PrintData.ExportPath = m_path.GetSpSMAHomeFolder
			m_CurrentExportPrintInFiles = PrintData.ExportPrintInFiles.GetValueOrDefault(False)

			Dim jobNumber As String = PrintData.PrintJobNumber
			success = success AndAlso LoadPrintJobData(jobNumber)
			If Not success Then Return New PrintResult With {.Printresult = False, .PrintresultMessage = "Vorlage wurde nicht gefunden!"}

			If PrintData.ShowAsDesign Then
				success = success AndAlso ShowAssignedListInDesign()

				Return result

			Else
				'If PrintData.WOSSendValueEnum <> ZVPrintData.WOSValue.PrintOtherSendWOS Then success = success AndAlso PrintAssignedList()
				success = success AndAlso PrintAssignedList()

				If success AndAlso Not m_CurrentExportPrintInFiles AndAlso PrintData.WOSSendValueEnum <> WOSZVSENDValue.PrintOtherSendWOS Then
					m_CurrentExportPrintInFiles = True
					success = success AndAlso PrintAssignedList()

					m_CurrentExportPrintInFiles = False
				End If
				If success AndAlso PrintData.ExportedFiles.Count > 0 Then SaveZVFileToMAHistoryDocDb(PrintData.ExportedFiles(0))
				If Not success Then result.Printresult = False

				If success AndAlso Not m_CurrentExportPrintInFiles AndAlso (PrintData.m_EmployeeData.Send2WOS.GetValueOrDefault(False) AndAlso Not PrintData.WOSSendValueEnum = WOSZVSENDValue.PrintWithoutSending) Then
					'm_CurrentExportPrintInFiles = True
					'success = success AndAlso PrintAssignedList()

					If success AndAlso PrintData.ExportedFiles.Count > 0 Then
						Dim wosResult = TransferCustomerZVDataIntoWOS(PrintData.m_EmployeeData)
						result.WOSresult = wosResult.Value
					End If

				Else


				End If

			End If

			SplashScreenManager.CloseForm(False)

			Return result

		End Function


#Region "Private Methodes"

		Private Function LoadPrintJobData(ByVal jobNumberToPrint As String) As Boolean

			m_PrintJobData = m_ListingDatabaseAccess.LoadAssignedPrintJobData(m_InitializationData.MDData.MDNr, m_EmployeeLanguage, jobNumberToPrint)


			Return Not (m_PrintJobData Is Nothing)

		End Function

		Private Function ShowAssignedListInDesign() As Boolean
			Dim result As Boolean = True

			Try
				If PrintData.m_EmployeeData Is Nothing Then
					m_Logger.LogWarning("Keine Daten wurden gefunden.")

					Return False
				End If

				InitLL(LL)
				LL.Variables.Clear()
				LL.Fields.Clear()

				result = result AndAlso SetLLVariable(LL)
				result = result AndAlso DefineZVPagesData(LL, PrintData.m_EmployeeData)
				result = result AndAlso DefineZVESData(LL, PrintData.m_ESData(0))
				result = result AndAlso DefineZVHourAbsenceData(LL, PrintData.m_HourAbsencedata)

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
			Dim employeedata = PrintData.m_EmployeeData
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

				result = result AndAlso SetLLVariable(LL)
				result = result AndAlso DefineZVPagesData(LL, PrintData.m_EmployeeData)
				result = result AndAlso DefineZVESData(LL, PrintData.m_ESData(0))
				result = result AndAlso DefineZVHourAbsenceData(LL, PrintData.m_HourAbsencedata)
				LL.Variables.Add("WOSDoc", 0)

				SplashScreenManager.CloseForm(False)
				LL.Core.LlPrintWithBoxStart(If(m_PrintJobData.LLDocName.ToUpper.EndsWith(".LST".ToUpper),
																				 LlProject.List, LlProject.Card),
																				 m_PrintJobData.LLDocName, LlPrintMode.Export,
																				 If(doExport, LlBoxType.None, LlBoxType.StandardAbort),
																				 CType(PrintData.frmhwnd, IntPtr), m_PrintJobData.LLDocLabel)

				If doExport Then
					LL.Variables.Add("WOSDoc", 1)

					LL.Core.LlPrintSetOptionString(LlPrintOptionString.Export, "PDF")
					LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.File", tempFileName)
					LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.Path", PrintData.ExportPath)
					LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.Quiet", "1")

					LL.Core.LlPrintSetOption(LlPrintOption.Dialog_DestinationMask, 2)

				Else
					LL.Core.LlPrintSetOption(LlPrintOption.Copies, Math.Max(1, m_NumberOfCopies.GetValueOrDefault(1)))
					LL.Core.LlPrintSetOption(LL_PRNOPT_COPIES_SUPPORTED, LL.Core.LlPrintGetOption(LlPrintOption.Copies))

					LL.Core.LlPrintOptionsDialog(CType(PrintData.frmhwnd, IntPtr), m_PrintJobData.PrintBoxHeader)
				End If


				Dim TargetFormat As String = LL.Core.LlPrintGetOptionString(LlPrintOptionString.Export)
				While Not LL.Core.LlPrint()
				End While

				If m_PrintJobData.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
					'For Each itm In PrintData.m_ESData
					'	' pass data for current record
					'	'result = result AndAlso DefineZVESData(LL, itm)

					While LL.Core.LlPrintFields() = LlConstants.WrnRepeatData
						LL.Core.LlPrint()
					End While

					'Next

					While LL.Core.LlPrintFieldsEnd() = LlConstants.WrnRepeatData
					End While
				End If
				LL.Core.LlPrintEnd(0)

				If TargetFormat = "PRV" Then
					SplashScreenManager.CloseForm(False)
					LL.Core.LlPreviewDisplay(m_PrintJobData.LLDocName, m_path.GetSpSTempFolder, CType(PrintData.frmhwnd, IntPtr))
					LL.Core.LlPreviewDeleteFiles(m_PrintJobData.LLDocName, m_path.GetSpSTempFolder)

					Return False
				End If
				If doExport Then PrintData.ExportedFiles.Add(Path.Combine(PrintData.ExportPath, tempFileName))


			Catch LlException As LL_User_Aborted_Exception
				Return False

			Catch LlException As Exception
				m_Logger.LogError(String.Format("LlException ({0} >>> {1}): {2}", PrintData.PrintJobNumber, m_PrintJobData.LLDocName, LlException.ToString))
				result = False

			Finally

			End Try


			Return result

		End Function

		Private Function SetLLVariable(ByVal LL As ListLabel) As Boolean
			Dim result As Boolean = True

			Try

				' Zusätzliche Variable einfügen
				LL.Variables.Add("AutorFName", m_InitializationData.UserData.UserFName)
				LL.Variables.Add("AutorLName", m_InitializationData.UserData.UserLName)


				result = result AndAlso GetMDUSData4Print(LL)
				result = result AndAlso DefineEmployeeData(LL)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			End Try

			Return result

		End Function

		Private Function DefineEmployeeData(ByVal LL As ListLabel) As Boolean
			Dim result As Boolean = True

			Try

				LL.Core.LlDefineVariableExt("SelMANr", ReplaceMissing(PrintData.m_EmployeeData.EmployeeNumber, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("MAName", ReplaceMissing(PrintData.m_EmployeeAddressData.EmployeeFullname, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("PostOfficeBox", ReplaceMissing(PrintData.m_EmployeeAddressData.PostOfficeBox, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Strasse", ReplaceMissing(PrintData.m_EmployeeAddressData.Street, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Adresse", ReplaceMissing(PrintData.m_EmployeeAddressData.EmployeeAddress, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("CivilStatus", ReplaceMissing(PrintData.m_EmployeeData.CivilStatus, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("AHVNr", ReplaceMissing(PrintData.m_EmployeeData.AHV_Nr, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("AHV_Nr_New", ReplaceMissing(PrintData.m_EmployeeData.AHV_Nr_New, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("GebDat", ReplaceMissing(PrintData.m_EmployeeData.Birthdate, String.Empty), LlFieldType.Date_Localized)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

			Return result

		End Function

		Private Function DefineZVPagesData(ByVal LL As ListLabel, ByVal data As EmployeeMasterData) As Boolean
			Dim result As Boolean = True

			Try

				LL.Core.LlDefineVariableExt("Jahr", ReplaceMissing(PageData.Jahr, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Monat", ReplaceMissing(PageData.Monat, String.Empty), LlFieldType.Text)

				LL.Core.LlDefineVariableExt("TotalHoursAmount", ReplaceMissing(PrintData.m_HourAbsencedata.TotalAmountOfHours, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("Nachtzeitzulage", ReplaceMissing(PageData.lblNachtzeitValue, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("TimeAus", ReplaceMissing(PageData.lblAuszahlungGleitValue, 0), LlFieldType.Numeric_Localized)

				LL.Core.LlDefineVariableExt("op6", ReplaceMissing(PageData.op6, 1), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("txt6_1", ReplaceMissing(PageData.txt6_1, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("txt6_2", ReplaceMissing(PageData.txt6_2, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("txt6_3", ReplaceMissing(PageData.txt6_3, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("txt7", ReplaceMissing(PageData.txt7, String.Empty), LlFieldType.Text)

				LL.Core.LlDefineVariableExt("txt8_1", ReplaceMissing(PageData.txt8_1, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("txt8_2", ReplaceMissing(PageData.txt8_2, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("txt9_1", ReplaceMissing(PageData.txt9, 0), LlFieldType.Numeric_Localized)

				LL.Core.LlDefineVariableExt("txt10_1", ReplaceMissing(PageData.txt10_1, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("m_Percent_500", ReplaceMissing(PageData.m_Percent_500, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("txt10_2", ReplaceMissing(PageData.txt10_2, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("m_Percent_600", ReplaceMissing(PageData.m_Percent_600, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("txt10_3", ReplaceMissing(PageData.txt10_3, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("m_Percent_700", ReplaceMissing(PageData.m_Percent_700, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("txt10_4", ReplaceMissing(PageData.txt10_4, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("txt10_5", ReplaceMissing(PageData.txt10_5, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("txt10_6", ReplaceMissing(PageData.txt10_6, 0), LlFieldType.Numeric_Localized)

				LL.Core.LlDefineVariableExt("chk11_1", ReplaceMissing(PageData.chk11_1, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("txt11_1", ReplaceMissing(PageData.txt11_1, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("txt11_2", ReplaceMissing(PageData.txt11_2, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("chk11_2", ReplaceMissing(PageData.chk11_2, 0), LlFieldType.Numeric_Localized)

				LL.Core.LlDefineVariableExt("txt14_1", ReplaceMissing(PageData.txt14_1, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("txt14_2", ReplaceMissing(PageData.txt14_2, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("txt14_3", ReplaceMissing(PageData.txt14_3, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("txt14_4", ReplaceMissing(PageData.txt14_4, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("txt14_5", ReplaceMissing(PageData.txt14_5, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("txt14_6", ReplaceMissing(PageData.txt14_6, String.Empty), LlFieldType.Text)

				LL.Core.LlDefineVariableExt("op15", ReplaceMissing(PageData.op15, 99), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("txt15_1", ReplaceMissing(PageData.txt15_1, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("chk15", ReplaceMissing(PageData.chk15, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("txt15_2", ReplaceMissing(PageData.txt15_2, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("txt15_3", ReplaceMissing(PageData.txt15_3, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("txt15_4", ReplaceMissing(PageData.txt15_4, String.Empty), LlFieldType.Text)

				LL.Core.LlDefineVariableExt("txt16", ReplaceMissing(PageData.txt16, String.Empty), LlFieldType.Text)

				LL.Core.LlDefineVariableExt("op17", ReplaceMissing(PageData.op17, 1), LlFieldType.Numeric_Localized)

				LL.Core.LlDefineVariableExt("txt18_1", ReplaceMissing(PageData.txt18_1, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("txt18_2", ReplaceMissing(PageData.txt18_2, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("txt18_3", ReplaceMissing(PageData.txt18_3, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("txt18_4", ReplaceMissing(PageData.txt18_4, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("txt18_5", ReplaceMissing(PageData.txt18_5, 0), LlFieldType.Numeric_Localized)

				LL.Core.LlDefineVariableExt("op12", ReplaceMissing(PageData.op12, 2), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("txt12", ReplaceMissing(PageData.txt12, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Compensationfund", ReplaceMissing(PageData.Compensationfund, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("CompanyBURNumber", ReplaceMissing(PageData.CompanyBURNumber, String.Empty), LlFieldType.Text)

				LL.Core.LlDefineVariableExt("ALKNumber", ReplaceMissing(PrintData.m_EmployeeContactCommData.ALKNumber, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("ALKName", ReplaceMissing(PrintData.m_EmployeeContactCommData.ALKName, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("ALKPOBox", ReplaceMissing(PrintData.m_EmployeeContactCommData.ALKPOBox, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("ALKStreet", ReplaceMissing(PrintData.m_EmployeeContactCommData.ALKStreet, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("ALKPostcode", ReplaceMissing(PrintData.m_EmployeeContactCommData.ALKPostcode, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("ALKLocation", ReplaceMissing(PrintData.m_EmployeeContactCommData.ALKLocation, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("ALKTelefon", ReplaceMissing(PrintData.m_EmployeeContactCommData.ALKTelephone, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("ALKTelefax", ReplaceMissing(PrintData.m_EmployeeContactCommData.ALKTelefax, String.Empty), LlFieldType.Text)


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				result = False

			End Try


			Return result

		End Function

		Private Function DefineZVESData(ByVal LL As ListLabel, ByVal data As ZVESData) As Boolean
			Dim result As Boolean = True

			Try
				Dim i As Integer = 1

				For i = 1 To 10
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
				For Each itm In PrintData.m_ESData

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

		Private Function DefineZVHourAbsenceData(ByVal LL As ListLabel, ByVal data As ZVHourAbsenceData) As Boolean
			Dim result As Boolean = True

			Try
				For i As Integer = 1 To 31
					LL.Core.LlDefineVariableExt(String.Format("Tag{0}", i), ReplaceMissing(data.GetWorkingHoursOfDay(i), String.Empty), LlFieldType.Text)
					LL.Core.LlDefineVariableExt(String.Format("_Fehl{0}", i), ReplaceMissing(data.GetAbsenceDayCodeOfDay(i), String.Empty), LlFieldType.Text)
				Next

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				result = False

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


		Private Function SaveZVFileToMAHistoryDocDb(ByVal strFullFilename As String) As Boolean
			Dim success As Boolean = True
			Dim strDocInfo As String = String.Format("{0} / {1}", ReplaceMissing(PageData.Monat, String.Empty), ReplaceMissing(PageData.Jahr, String.Empty))
			Dim strDocArt As String = "Zwischenverdienstformular Duplex"

			Try

				Dim data = New EmployeePrintedDocProperty
				data.createdfrom = m_InitializationData.UserData.UserFullName
				data.createdon = Now
				data.docname = String.Format("{0} {1}", strDocArt, strDocInfo)
				data.manr = PrintData.m_EmployeeData.EmployeeNumber
				data.scandoc = m_Utility.LoadFileBytes(strFullFilename)
				data.username = m_InitializationData.UserData.UserFullName

				success = success AndAlso m_ListingDatabaseAccess.AddAssignedPrintedDocumentInForEmployee(data)


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))

				Return False
			End Try


			Return success
		End Function



#Region "WOS-Methods"

		Function TransferCustomerZVDataIntoWOS(ByVal employeeData As EmployeeMasterData) As WOSSendResult
			Dim result As WOSSendResult = New WOSSendResult With {.Value = False, .Message = "Initialisierung..."}
			Dim success As Boolean = True
			If employeeData.Send2WOS AndAlso Not PrintData.WOSSendValueEnum = WOSZVSENDValue.PrintWithoutSending Then
				Dim wos = New SP.Internal.Automations.WOSUtility.EmployeeExport(m_InitializationData)

				Dim wosSetting = New WOSSendSetting

				wosSetting.EmployeeNumber = employeeData.EmployeeNumber
				wosSetting.EmployeeGuid = employeeData.Transfered_Guid
				wosSetting.DocumentArtEnum = WOSSendSetting.DocumentArt.Zwischenverdienstformular

				Dim pdfFilename As String = PrintData.ExportedFiles(0)
				If PrintData.ExportedFiles.Count > 1 Then
					Dim pdfDocument As New PdfDocumentProcessor()
					pdfDocument.LoadDocument(pdfFilename)

					For i As Integer = 1 To PrintData.ExportedFiles.Count - 1
						pdfDocument.AppendDocument(PrintData.ExportedFiles(i))
						pdfDocument.SaveDocument(pdfFilename)
					Next
					pdfDocument.CloseDocument()
				End If

				wosSetting.DocumentInfo = String.Format("Zwischenverdienst (Duplex): {0} / {1}", PageData.Monat, PageData.Jahr)
				Dim fileByte() = m_Utility.LoadFileBytes(pdfFilename)
				wosSetting.ScanDoc = fileByte
				Dim scanfileinfo = New FileInfo(pdfFilename)
				wosSetting.ScanDocName = scanfileinfo.Name

				wos.WOSSetting = wosSetting

				If success Then result = wos.TransferEmployeeDocumentDataToWOS(True)

				PrintData.ExportedFiles.Clear()
			End If


			Return result

		End Function


#End Region


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

		''' <summary>
		''' Replaces a missing object with another object.
		''' </summary>
		''' <param name="obj">The object.</param>
		''' <param name="replacementObject">The replacement object.</param>
		''' <returns>The object or the replacement object it the object is nothing.</returns>
		Private Function ReplaceMissing(ByVal obj As Object, ByVal replacementObject As Object) As Object

			If (obj Is Nothing) Then
				Return replacementObject
			Else
				Return obj
			End If

		End Function



#End Region

	End Class


	Public Class ZVPrintData

		Public Property frmhwnd As String
		Public Property m_EmployeeAddressData As EmployeeSAddressData
		Public Property m_EmployeeData As EmployeeMasterData
		Public Property m_EmployeeContactCommData As EmployeeContactComm
		Public Property m_ESData As IEnumerable(Of ZVESData)

		Public Property m_HourAbsencedata As ZVHourAbsenceData
		Public Property m_Payrolldata As IEnumerable(Of ZVPayrollData)

		Public Property ShowAsDesign As Boolean
		Public Property PrintJobNumber As String
		Public Property ExportPath As String
		Public Property ExportPrintInFiles As Boolean?
		Public Property ExportedFiles As List(Of String)
		Public Property WOSSendValueEnum As WOSZVSENDValue

	End Class


	Public Class ZVPageData

		Public Property Monat As String
		Public Property Jahr As String
		Public Property Compensationfund As String
		Public Property CompanyBURNumber As String
		Public Property op12 As Integer?
		Public Property op15 As Integer?
		Public Property txt15_1 As String
		Public Property chk15 As Integer?
		Public Property txt15_2 As String
		Public Property txt15_3 As String
		Public Property txt15_4 As String
		Public Property txt16 As String
		Public Property op17 As Integer?
		Public Property lblAuszahlungGleitValue As Decimal?
		Public Property lblAuszahlungNachtzulageValue As Decimal?
		Public Property lblNachtzeitValue As Decimal?

		Public Property txt8_1 As Decimal?
		Public Property txt8_2 As Decimal?
		Public Property txt9 As Decimal?
		Public Property txt10_1 As Decimal?
		Public Property m_Percent_500 As Decimal?
		Public Property txt10_2 As Decimal?
		Public Property m_Percent_600 As Decimal?
		Public Property txt10_3 As Decimal?
		Public Property m_Percent_700 As Decimal?
		Public Property txt10_4 As Decimal?
		Public Property txt10_5 As String
		Public Property txt10_6 As Decimal?

		Public Property txt12 As String
		Public Property txt14_1 As Decimal?
		Public Property txt14_2 As Decimal?
		Public Property txt14_3 As Decimal?
		Public Property txt14_4 As Decimal?
		Public Property txt14_5 As Decimal?
		Public Property txt14_6 As String


		Public Property op6 As Integer?
		Public Property txt6_1 As Decimal?
		Public Property txt6_2 As Decimal?
		Public Property txt6_3 As Decimal?
		Public Property txt7 As String

		Public Property chk11_1 As Integer?
		Public Property chk11_2 As Integer?
		Public Property txt11_1 As String
		Public Property txt11_2 As Decimal?

		Public Property txt18_1 As Decimal?
		Public Property txt18_2 As Decimal?
		Public Property txt18_3 As Decimal?
		Public Property txt18_4 As Decimal?
		Public Property txt18_5 As Decimal?

	End Class

End Namespace


