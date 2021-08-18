
Imports System.IO
Imports System.Drawing
Imports System.Windows.Forms
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Listing.DataObjects
Imports combit.ListLabel25

Imports DevExpress.Pdf
Imports System.Drawing.Printing

Imports System.Drawing.Imaging

Imports O2S.Components.PDF4NET.PDFFile
Imports O2S.Components.PDFRender4NET
Imports SP.DatabaseAccess.WOS
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Internal.Automations.WOSUtility.DataObjects
Imports SP.Infrastructure.PDFUtilities.Utilities
Imports iTextSharp.text.pdf
Imports SP.Infrastructure

Namespace InvoicePrint

	Public Class ClsPrintInvoice
		Implements IDisposable


#Region "private consts"

		Private Const MANDANT_XML_MAIN_KEY As String = "MD_{0}/"
		Private Const MANDANT_XML_SETTING_SPUTNIK_INVOICE_SETTING As String = "MD_{0}/Debitoren"
		Private Const MANDANT_XML_SETTING_SPUTNIK_SONSTIGES_SETTING As String = "MD_{0}/Sonstiges"
		Private Const MANDANT_XML_SETTING_SPUTNIK_AHV_SETTING As String = "MD_{0}/AHV-Daten"
		Private Const MANDANT_XML_SETTING_SPUTNIK_SUVA_SETTING As String = "MD_{0}/SUVA-Daten"
		Private Const MANDANT_XML_SETTING_SPUTNIK_FAK_SETTING As String = "MD_{0}/Fak-Daten"

#End Region


#Region "private fields"

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
		''' The Listing data access object.
		''' </summary>
		Private m_ListingDatabaseAccess As IListingDatabaseAccess

		''' <summary>
		''' The wos data access object.
		''' </summary>
		Private m_WOSDatabaseAccess As WOSDatabaseAccess

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

		Private m_invoiceRefNumber As String
		Private m_CurrentinvoiceNumber As Integer?
		Private m_CurrentCustomerNumber As Integer?
		Private m_CustomerLanguage As String
		Private m_NumberOfCopies As Integer?
		Private m_invoiceYear As Integer?
		Private m_reportScanFile As String
		Private m_firstCallReport As Boolean

		Private m_pdfPrinterSettings As PrinterSettings
		Private m_ReferenceNumbersTo10Setting As Boolean
		Private m_PDFUtility As PDFUtilities.Utilities


#End Region


#Region "Public properties"

		Public Property PrintData As InvoicePrintData
		Public Property PrintAsFirstJob As Boolean?

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
			m_ListingDatabaseAccess = New ListingDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)
			m_WOSDatabaseAccess = New WOSDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)

			m_SonstigesSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_SONSTIGES_SETTING, m_InitializationData.MDData.MDNr)
			m_InvoiceSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_INVOICE_SETTING, m_InitializationData.MDData.MDNr)
			m_SuvaSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_SUVA_SETTING, m_InitializationData.MDData.MDNr)
			m_AHVSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_AHV_SETTING, m_InitializationData.MDData.MDNr)
			m_FAKSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_FAK_SETTING, m_InitializationData.MDData.MDNr)
			m_MandantSetting = String.Format(MANDANT_XML_MAIN_KEY, m_InitializationData.MDData.MDNr)
			m_ReferenceNumbersTo10Setting = ReferenceNumbersTo10Setting


			LL = New ListLabel
			m_StartPrintInLLDebug = CBool(m_MandantData.GetSelectedMDProfilValue(m_InitializationData.MDData.MDNr, Now.Year, "Sonstiges", "EnableLLDebug", "0"))


		End Sub

		Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
			If Not Me.disposed Then
				If disposing Then

				End If
				' Add code here to release the unmanaged resource.
				If Not LL Is Nothing Then
					LL.Dispose()
					LL.Core.Dispose()
				End If
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


#Region "private properties"

		''' <summary>
		''' Gets the reference number to 10 setting.
		''' </summary>
		Private ReadOnly Property ReferenceNumbersTo10Setting As Boolean
			Get

				Dim mdNumber = m_InitializationData.MDData.MDNr

				Dim invoiceYear = Now.Year
				Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}/Debitoren", mdNumber)
				Dim ref10forfactoring As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_MandantData.GetSelectedMDDataXMLFilename(mdNumber, invoiceYear), String.Format("{0}/ref10forfactoring", FORM_XML_MAIN_KEY)), False)

				Return ref10forfactoring.HasValue AndAlso ref10forfactoring

			End Get

		End Property

		ReadOnly Property GetPDFVW_O2SSerial() As String
			Get
				Return "yourlicencekey"
			End Get
		End Property

		Public ReadOnly Property GetPDF4Net_O2SSerial() As String
			Get
				Return "yourlicencekey"
			End Get
		End Property

		Private ReadOnly Property PrintEZOnSepratedPage() As Boolean
			Get
				Dim ezonsepratedpage As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_MandantData.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, m_invoiceYear),
																								   String.Format("MD_{0}/Debitoren/ezonsepratedpage", m_InitializationData.MDData.MDNr)), False)

				Return ezonsepratedpage.HasValue AndAlso ezonsepratedpage

			End Get
		End Property

#End Region

		Public Function PrintInvoice() As PrintResult
			Dim success As Boolean = True
			Dim result As PrintResult = New PrintResult With {.Printresult = True}
			Dim firstCallAutomatedInvoice As Boolean = True
			Dim firstCallCustomInvoice As Boolean = True
			Dim firstCallCreditCustomInvoice As Boolean = True
			Dim firstCallAutomatedCreditCustomInvoice As Boolean = True
			Dim i As Integer = 0

			Dim settings = New PrinterSettings()

			result.JobResultInvoiceData = New List(Of PrintedInvoiceData)

			If settings.PrinterName Is Nothing OrElse String.IsNullOrWhiteSpace(settings.PrinterName) Then
				m_Logger.LogError("no default printer is defined.")
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Sie haben keinen Standard-Drucker definiert."))

				Return (New PrintResult With {.Printresult = False})
			Else
				m_Logger.LogInfo(String.Format("default printer: {0}", settings.PrinterName))
			End If

			If String.IsNullOrWhiteSpace(PrintData.ExportPath) Then PrintData.ExportPath = m_path.GetSpSREHomeFolder
			m_pdfPrinterSettings = New PrinterSettings
			m_pdfPrinterSettings = Nothing
			m_firstCallReport = True

			success = success AndAlso LoadInvoiceListData()
			If Not success Then result.PrintresultMessage = m_Translate.GetSafeTranslationValue("Keine Rechnungen wurden gefunden.")
			If Not success Then Return result
			PrintData.ExportedFiles = New List(Of String)

			For Each invoice In m_invoiceData
				Dim JobResultData As New PrintedInvoiceData
				Dim jobNumber As String = GetPrintJobNumber(invoice.Art, String.Empty, invoice.CreditInvoiceAutomated, PrintData.WhatToPrintValueEnum = WhatToPrintValue.EZ)

				m_CurrentinvoiceNumber = invoice.RENr
				m_CurrentCustomerNumber = invoice.KDNr
				m_invoiceRefNumber = invoice.RefNr
				m_CustomerLanguage = invoice.Language
				m_NumberOfCopies = If(PrintData.PrintInvoiceAsCopy.GetValueOrDefault(False), 1, invoice.NumberOfCopies)

				m_invoiceYear = If(invoice.InvoiceDate.HasValue, Now.Year, DateValue(invoice.InvoiceDate).Year)
				m_CurrentExportPrintInFiles = PrintData.ExportPrintInFiles.GetValueOrDefault(False)
				If PrintData.ExportPrintInFiles.GetValueOrDefault(False) Then invoice.Send2WOS = False

				success = success AndAlso LoadPrintJobData(jobNumber)

				If PrintData.ShowAsDesign Then
					success = success AndAlso ShowAssignedInvoiceInDesign()

					Return result

				Else
					Dim printjobAsFirstjob As Boolean = True

					Select Case True
						Case invoice.Art = "A"
							printjobAsFirstjob = firstCallAutomatedInvoice

						Case invoice.Art = "I", invoice.Art = "F"
							printjobAsFirstjob = firstCallCustomInvoice

						Case invoice.Art = "G", invoice.Art = "R"

							If invoice.CreditInvoiceAutomated Then
								printjobAsFirstjob = firstCallAutomatedCreditCustomInvoice

							Else
								printjobAsFirstjob = firstCallCreditCustomInvoice
							End If

					End Select

					If invoice.Send2WOS AndAlso PrintData.WOSSendValueEnum = InvoicePrintData.WOSValue.PrintOtherSendWOS Then
						m_CurrentExportPrintInFiles = True

					ElseIf invoice.Send2WOS AndAlso PrintData.WOSSendValueEnum = InvoicePrintData.WOSValue.PrintAndSend Then
						m_CurrentExportPrintInFiles = False
						success = success AndAlso FindAndSendJobToPrinter(printjobAsFirstjob)
						m_CurrentExportPrintInFiles = True

					End If
					success = success AndAlso FindAndSendJobToPrinter(printjobAsFirstjob)

					If Not m_CurrentExportPrintInFiles Then
						firstCallAutomatedInvoice = If(invoice.Art = "A", False, firstCallAutomatedInvoice)
						firstCallCustomInvoice = If(invoice.Art = "I", False, firstCallCustomInvoice)
						firstCallCreditCustomInvoice = If(invoice.Art = "G" AndAlso Not invoice.CreditInvoiceAutomated, False, firstCallCreditCustomInvoice)
						firstCallAutomatedCreditCustomInvoice = If(invoice.Art = "G" AndAlso invoice.CreditInvoiceAutomated, False, firstCallAutomatedCreditCustomInvoice)
					End If
				End If

				If Not success Then Exit For

				JobResultData.InvoiceNumber = m_CurrentinvoiceNumber
				JobResultData.CustomerNumber = m_CurrentCustomerNumber
				JobResultData.Companyname = invoice.CustomerName
				JobResultData.REEMail = invoice.REEmail
				JobResultData.SendAsZip = invoice.SendAsZip.GetValueOrDefault(False)
				JobResultData.OneInvoicePerMail = invoice.OneInvoicePerMail.GetValueOrDefault(False)
				JobResultData.InvoiceDate = invoice.InvoiceDate
				JobResultData.MandantName = m_InitializationData.MDData.MDName
				JobResultData.MandantLocation = m_InitializationData.MDData.MDCity

				If PrintData.ExportedFiles.Count > 1 Then

					Dim mergePDF As Boolean = True
					Dim newMergedFilename As String = Path.Combine(m_InitializationData.UserData.spTempInvoicePath, String.Format("Rechnung_{0}_{1}.ex", m_CurrentinvoiceNumber, m_CurrentCustomerNumber))
					newMergedFilename = Path.ChangeExtension(newMergedFilename, ".pdf")
					If File.Exists(newMergedFilename) Then
						Try
							File.Delete(newMergedFilename)
						Catch ex As Exception
							newMergedFilename = Path.Combine(m_InitializationData.UserData.spTempInvoicePath, String.Format("Rechnung_{0}_{1}_{2}.ex", m_CurrentinvoiceNumber, m_CurrentCustomerNumber, Environment.TickCount.ToString))
							newMergedFilename = Path.ChangeExtension(newMergedFilename, ".pdf")

						End Try
					End If
					mergePDF = mergePDF AndAlso m_PDFUtility.MergePdfFiles(PrintData.ExportedFiles.ToArray, newMergedFilename)

					If mergePDF Then
						JobResultData.ExportedFileName = newMergedFilename
						Try
							For Each itm In PrintData.ExportedFiles
								System.IO.File.Open(itm, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
								FileClose(1)

								File.Delete(itm)
							Next
						Catch ex As Exception

						End Try

						PrintData.ExportedFiles.Clear()
					End If

				ElseIf PrintData.ExportedFiles.Count = 1 Then
					JobResultData.ExportedFileName = PrintData.ExportedFiles(0)
					PrintData.ExportedFiles.Clear()

				End If

				result.Printresult = True
				result.PrintresultMessage = String.Empty

				result.JobResultInvoiceData.Add(JobResultData)

				i += 1
			Next


			Return result

		End Function


#Region "Private Methodes"

		Private Function FindAndSendJobToPrinter(ByVal printjobAsFirstjob As Boolean) As Boolean
			Dim result As Boolean = True
			Dim currentInoviceData = m_invoiceData.Where(Function(data) data.RENr = m_CurrentinvoiceNumber).FirstOrDefault()
			result = result AndAlso Not currentInoviceData Is Nothing

			Dim jobNumber As String

			If PrintData.WhatToPrintValueEnum <> WhatToPrintValue.EZ Then
				jobNumber = GetPrintJobNumber(currentInoviceData.Art, String.Empty, currentInoviceData.CreditInvoiceAutomated, False)
				result = result AndAlso LoadPrintJobData(jobNumber)
			End If

			If PrintAsFirstJob.HasValue Then
				printjobAsFirstjob = PrintAsFirstJob
				PrintAsFirstJob = Nothing
			End If

			If PrintData.WhatToPrintValueEnum <> WhatToPrintValue.EZ Then result = result AndAlso PrintAssignedInvoice(printjobAsFirstjob, False)
			If result AndAlso PrintEZOnSepratedPage AndAlso PrintData.WhatToPrintValueEnum <> WhatToPrintValue.Detail AndAlso Not PrintData.PrintInvoiceAsCopy.GetValueOrDefault(False) AndAlso
								(currentInoviceData.Art = "A" OrElse currentInoviceData.Art = "I" OrElse currentInoviceData.Art = "F") Then
				jobNumber = GetPrintJobNumber(currentInoviceData.Art, String.Empty, currentInoviceData.CreditInvoiceAutomated, True)
				result = result AndAlso LoadPrintJobData(jobNumber)

				' set m_NumberOfCopies to 1 
				m_NumberOfCopies = 1
				result = result AndAlso PrintAssignedInvoice(printjobAsFirstjob, True)
			End If
			If Not m_CurrentExportPrintInFiles Then
				result = result AndAlso m_ListingDatabaseAccess.UpdateInvoiceDatabaseWithPrintDate(currentInoviceData.MDNr, m_CurrentinvoiceNumber)
			End If

			If result AndAlso PrintData.WhatToPrintValueEnum = WhatToPrintValue.DetailAndEZ AndAlso Not PrintData.PrintInvoiceAsCopy.GetValueOrDefault(False) AndAlso currentInoviceData.Art = "A" AndAlso currentInoviceData.PrintWithReport.GetValueOrDefault(0) Then
				' print reports
				' O2Solution
				FindAndSendReportScanToPrinter()

				'' Devexpress, slow!!!
				'result = result AndAlso PrintAssignedReportForInvoice()

			End If

			If PrintData.WhatToPrintValueEnum = WhatToPrintValue.DetailAndEZ AndAlso Not PrintData.PrintInvoiceAsCopy.GetValueOrDefault(False) AndAlso
								currentInoviceData.Send2WOS AndAlso Not (PrintData.WOSSendValueEnum = InvoicePrintData.WOSValue.PrintWithoutSending) AndAlso PrintData.ExportedFiles.Count > 0 Then
				Dim wosResult = TransferCustomerInvoiceIntoWOS(currentInoviceData)
				result = wosResult.Value
			End If

			'LL = New ListLabel

			Return result

		End Function

		Private Function FindAndSendReportScanToPrinter() As Boolean
			Dim result As Boolean = True
			Dim reportFiles As New List(Of String)

			Dim jobNumber As String = "10.4.1"

			'If m_CurrentExportPrintInFiles Then Return True
			m_reportScanFile = String.Empty

			result = result AndAlso LoadPrintJobData(jobNumber)
			result = result AndAlso CreateAssignedReportWithO2SAndSendToPrinter()

			m_firstCallReport = If(m_firstCallReport AndAlso Not result, True, False)
			'LL = New ListLabel


			Return result

		End Function

		Private Function LoadPrintJobData(ByVal jobNumberToPrint As String) As Boolean

			m_PrintJobData = m_ListingDatabaseAccess.LoadAssignedPrintJobData(m_InitializationData.MDData.MDNr, m_CustomerLanguage, jobNumberToPrint)


			Return Not (m_PrintJobData Is Nothing)

		End Function

		Private Function LoadInvoiceListData() As Boolean

			Dim searchConditions As New InvoicePrintSearchConditionData With {.MDNr = m_InitializationData.MDData.MDNr}
			searchConditions.GroupByEMail = False
			searchConditions.WOSValueEnum = WOSSearchValue.SearchAllCustomer
			searchConditions.OrderByEnum = PrintData.OrderByEnum


			searchConditions.InvoiceNumbers = PrintData.InvoiceNumbers
			searchConditions.CustomerNumbers = New List(Of Integer)

			m_invoiceData = m_ListingDatabaseAccess.LoadInvoiceData(searchConditions)

			If m_invoiceData Is Nothing Then
				m_Logger.LogError(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden."))

				Return False
			End If


			Return Not (m_invoiceData Is Nothing)

		End Function



		Private Function ShowAssignedInvoiceInDesign() As Boolean
			Dim result As Boolean = True
			Dim reportNumber As Integer?
			Dim employeeNumber As Integer?
			Dim reportlineNumber As Integer?
			Dim invoiceArt As String

			Try
				Dim invoiceData As IEnumerable(Of SP.DatabaseAccess.Listing.DataObjects.InvoicePrintData)

				Dim currentInoviceData = m_invoiceData.Where(Function(data) data.RENr = m_CurrentinvoiceNumber).FirstOrDefault()
				If currentInoviceData Is Nothing Then Return False
				invoiceArt = currentInoviceData.Art.ToUpper

				Select Case True
					Case invoiceArt = "A"
						invoiceData = m_ListingDatabaseAccess.LoadAssignedAutomatedInvoicePrintData(m_InitializationData.MDData.MDNr, m_CurrentinvoiceNumber, m_CustomerLanguage)

					Case invoiceArt = "I", invoiceArt = "F"
						invoiceData = m_ListingDatabaseAccess.LoadAssignedCustomInvoicePrintData(m_InitializationData.MDData.MDNr, m_CurrentinvoiceNumber, m_CustomerLanguage)

					Case invoiceArt = "G"
						invoiceData = m_ListingDatabaseAccess.LoadAssignedCreditInvoicePrintData(m_InitializationData.MDData.MDNr, m_CurrentinvoiceNumber, m_CustomerLanguage)

					Case invoiceArt = "R"
						invoiceData = m_ListingDatabaseAccess.LoadAssignedRefundInvoicePrintData(m_InitializationData.MDData.MDNr, m_CurrentinvoiceNumber, m_CustomerLanguage)

					Case Else
						Return False

				End Select
				If invoiceData Is Nothing OrElse invoiceData.Count = 0 Then
					m_Logger.LogError(String.Format("Rechnung {0} wurde nicht gefunden.", m_CurrentinvoiceNumber))

					Return False
				End If

				reportNumber = invoiceData(0).RPNr
				employeeNumber = invoiceData(0).MANr
				reportlineNumber = invoiceData(0).RPLNr

				InitLL(LL)
				LL.Variables.Clear()
				LL.Fields.Clear()

				result = result AndAlso SetLLVariable(LL)
				result = result AndAlso DefineData(LL, invoiceData(0))

				LL.Variables.Add("WOSDoc", 1)

				If result Then
					LL.Core.LlDefineLayout(CType(PrintData.frmhwnd, IntPtr), m_PrintJobData.LLDocLabel,
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

		Function PrintAssignedInvoice(ByVal showPrinterBox As Boolean, ByVal jobAsEZ As Boolean) As Boolean
			Const LL_PRNOPT_COPIES_SUPPORTED As Integer = 3
			Dim result As Boolean = True
			Dim reportNumber As Integer?
			Dim employeeNumber As Integer?
			Dim reportlineNumber As Integer?
			Dim invoiceArt As String
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
				Dim invoiceData As IEnumerable(Of SP.DatabaseAccess.Listing.DataObjects.InvoicePrintData)

				Dim currentInoviceData = m_invoiceData.Where(Function(data) data.RENr = m_CurrentinvoiceNumber).FirstOrDefault()
				If currentInoviceData Is Nothing Then Return False
				invoiceArt = currentInoviceData.Art.ToUpper

				Select Case True
					Case invoiceArt = "A"
						invoiceData = m_ListingDatabaseAccess.LoadAssignedAutomatedInvoicePrintData(m_InitializationData.MDData.MDNr, m_CurrentinvoiceNumber, m_CustomerLanguage)

					Case invoiceArt = "I", invoiceArt = "F"
						invoiceData = m_ListingDatabaseAccess.LoadAssignedCustomInvoicePrintData(m_InitializationData.MDData.MDNr, m_CurrentinvoiceNumber, m_CustomerLanguage)

					Case invoiceArt = "G"
						invoiceData = m_ListingDatabaseAccess.LoadAssignedCreditInvoicePrintData(m_InitializationData.MDData.MDNr, m_CurrentinvoiceNumber, m_CustomerLanguage)

					Case invoiceArt = "R"
						invoiceData = m_ListingDatabaseAccess.LoadAssignedRefundInvoicePrintData(m_InitializationData.MDData.MDNr, m_CurrentinvoiceNumber, m_CustomerLanguage)

					Case Else
						Return False

				End Select

				If invoiceData Is Nothing OrElse invoiceData.Count = 0 Then
					m_Logger.LogError(String.Format("Rechnung {0} wurde nicht gefunden.", m_CurrentinvoiceNumber))

					Return False
				End If
				Dim printResult As Integer = 0

				reportNumber = invoiceData(0).RPNr.GetValueOrDefault(0)
				employeeNumber = invoiceData(0).MANr.GetValueOrDefault(0)
				reportlineNumber = invoiceData(0).RPLNr.GetValueOrDefault(0)

				InitLL(LL)
				LL.Variables.Clear()
				LL.Fields.Clear()

				result = result AndAlso DefineData(LL, invoiceData(0))
				result = result AndAlso SetLLVariable(LL)
				LL.Variables.Add("WOSDoc", 0)

				LL.Core.LlPrintWithBoxStart(If(m_PrintJobData.LLDocName.ToUpper.EndsWith(".LST".ToUpper), LlProject.List, LlProject.Card), m_PrintJobData.LLDocName, LlPrintMode.Export,
											If(m_CurrentExportPrintInFiles, LlBoxType.None, LlBoxType.StandardAbort), CType(PrintData.frmhwnd, IntPtr), m_PrintJobData.LLDocLabel)

				If m_CurrentExportPrintInFiles Then

					Dim strExportPfad As String = m_InitializationData.UserData.spTempInvoicePath
					If Not Directory.Exists(strExportPfad) Then Directory.CreateDirectory(m_InitializationData.UserData.spTempInvoicePath)
					PrintData.ExportPath = strExportPfad

					Dim initialFilename As String = String.Format("Rechnung{0}", If(jobAsEZ, "_EZ", ""))
					tempFileName = String.Format("{0}_{1}.pdf", initialFilename, m_CurrentinvoiceNumber)

					If File.Exists(Path.Combine(strExportPfad, tempFileName)) Then
						Try
							'tempFileName = String.Format("{0}{1}_{2}.PDF", initialFilename, m_CurrentinvoiceNumber, Environment.TickCount)
							File.Delete(Path.Combine(strExportPfad, tempFileName))
						Catch ex As Exception
							tempFileName = String.Format("{0}{1}_{2}.PDF", initialFilename, m_CurrentinvoiceNumber, Environment.TickCount)
						End Try
					End If


					LL.Variables.Add("WOSDoc", 1)
					LL.Core.LlPrintSetOptionString(LlPrintOptionString.Export, "PDF")
					LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.File", tempFileName)
					LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.Path", strExportPfad)
					LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.Quiet", "1")

					LL.Core.LlPrintSetOption(LlPrintOption.Dialog_DestinationMask, 2)

				Else
					LL.Core.LlPrintSetOption(LlPrintOption.Copies, Math.Max(1, m_NumberOfCopies.GetValueOrDefault(1))) ' m_PrintJobData.LLCopyCount)
					LL.Core.LlPrintSetOption(LL_PRNOPT_COPIES_SUPPORTED, LL.Core.LlPrintGetOption(LlPrintOption.Copies))

					If showPrinterBox Then LL.Core.LlPrintOptionsDialog(CType(PrintData.frmhwnd, IntPtr), m_PrintJobData.PrintBoxHeader)

				End If

				Dim TargetFormat As String = LL.Core.LlPrintGetOptionString(LlPrintOptionString.Export)
				While Not LL.Core.LlPrint()
				End While

				If m_PrintJobData.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
					For Each itm In invoiceData
						' pass data for current record
						result = result AndAlso DefineData(LL, itm)

						While LL.Core.LlPrintFields() = LlConstants.WrnRepeatData
							LL.Core.LlPrint()
						End While

					Next

					While LL.Core.LlPrintFieldsEnd() = LlConstants.WrnRepeatData
					End While
				End If
				LL.Core.LlPrintEnd(0)

				If TargetFormat = "PRV" Then
					LL.Core.LlPreviewDisplay(m_PrintJobData.LLDocName, m_path.GetSpSTempFolder, CType(PrintData.frmhwnd, IntPtr))
					LL.Core.LlPreviewDeleteFiles(m_PrintJobData.LLDocName, m_path.GetSpSTempFolder)

					Return False
				End If
				If m_CurrentExportPrintInFiles Then
					PrintData.ExportedFiles.Add(Path.Combine(PrintData.ExportPath, tempFileName))
					Try
						'System.IO.File.Open(Path.Combine(PrintData.ExportPath, tempFileName), IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
						'FileClose(1)

					Catch ex As Exception

					End Try


				End If


			Catch LlException As LL_User_Aborted_Exception
				Return False

			Catch LlException As Exception
				m_Logger.LogError(String.Format("LlException: {0}", LlException.ToString))
				result = False

			Finally

			End Try


			Return result

		End Function

		Function PrintAssignedReportScan(ByVal showPrinterBox As Boolean, ByVal data As ReportForPrintInvoiceData) As Boolean
			Const LL_PRNOPT_COPIES_SUPPORTED As Integer = 3
			Dim result As Boolean = True

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

				result = result AndAlso DefineReportData(LL, data)
				result = result AndAlso SetLLVariable(LL)
				LL.Variables.Add("WOSDoc", If(m_CurrentExportPrintInFiles, 1, 0))

				LL.Core.LlPrintWithBoxStart(If(m_PrintJobData.LLDocName.ToUpper.EndsWith(".LST".ToUpper),
																																						 LlProject.List, LlProject.Card),
																																						 m_PrintJobData.LLDocName, LlPrintMode.Export,
																																						 If(m_CurrentExportPrintInFiles, LlBoxType.None, LlBoxType.StandardAbort),
																																						 CType(PrintData.frmhwnd, IntPtr), m_PrintJobData.LLDocLabel)

				LL.Core.LlPrintSetOption(LlPrintOption.Copies, m_PrintJobData.LLCopyCount)
				LL.Core.LlPrintSetOption(LL_PRNOPT_COPIES_SUPPORTED, LL.Core.LlPrintGetOption(LlPrintOption.Copies))

				If showPrinterBox Then LL.Core.LlPrintOptionsDialog(CType(PrintData.frmhwnd, IntPtr), m_PrintJobData.PrintBoxHeader)

				Dim TargetFormat As String = LL.Core.LlPrintGetOptionString(LlPrintOptionString.Export)
				While Not LL.Core.LlPrint()
				End While

				LL.Core.LlPrintEnd(0)

				If TargetFormat = "PRV" Then
					LL.Core.LlPreviewDisplay(m_PrintJobData.LLDocName, m_path.GetSpSTempFolder, CType(PrintData.frmhwnd, IntPtr))
					LL.Core.LlPreviewDeleteFiles(m_PrintJobData.LLDocName, m_path.GetSpSTempFolder)

					Return False
				End If


			Catch LlException As LL_User_Aborted_Exception
				Return False

			Catch LlException As Exception
				m_Logger.LogError(String.Format("LlException: {0}", LlException.ToString))
				result = False

			Finally

			End Try


			Return result

		End Function

		Function PrintAssignedReportForInvoice() As Boolean
			Dim result As Boolean = True
			'Dim reportNumber As Integer
			'Dim reportlineNumber As Integer
			Dim rpDocGuid As String = String.Empty

			Try
				If m_StartPrintInLLDebug Then
					LL.Debug = LlDebug.LogToFile
					LL.Debug = LlDebug.Enabled
				End If
			Catch ex As Exception
				m_Logger.LogError(String.Format("EnablingLLDebuging: {0}", ex.ToString))

			End Try

			Try
				Dim currentInoviceData = m_invoiceData.Where(Function(data) data.RENr = m_CurrentinvoiceNumber).FirstOrDefault()

				Dim reportData = m_ListingDatabaseAccess.LoadReportDataForPrintedInvoice(m_InitializationData.MDData.MDNr, m_CurrentinvoiceNumber)
				If reportData Is Nothing Then
					m_Logger.LogError(String.Format("Rapport-Scan für die Rechnung {0} wurde nicht gefunden.", m_CurrentinvoiceNumber))

					Return True
				End If
				If reportData.Count = 0 Then Return True

				Dim PDFFiles As New List(Of String)
				Dim CreateReportFile As Boolean = True

				For Each report In reportData
					'reportNumber = report.RPNr
					'reportlineNumber = report.RPLNr
					If rpDocGuid.Contains(report.RPDoc_Guid) Then
						CreateReportFile = False
					Else
						CreateReportFile = True
						rpDocGuid &= String.Format(" | {0}", report.RPDoc_Guid)
					End If

					If CreateReportFile Then

						Dim bytes() = report.DocScan
						Dim tempFileName = System.IO.Path.GetTempFileName()
						Dim fileExtension = report.Extension

						Dim tempFileFinal = System.IO.Path.ChangeExtension(tempFileName, fileExtension)
						If bytes Is Nothing OrElse Not m_Utility.WriteFileBytes(tempFileFinal, bytes) Then Return True

						PDFFiles.Add(tempFileFinal)

						If m_CurrentExportPrintInFiles Then
							PrintData.ExportedFiles.Add(tempFileFinal)
						End If

					End If

				Next


				If Not m_CurrentExportPrintInFiles AndAlso m_pdfPrinterSettings Is Nothing Then
					Dim dlgPrinter As New PrintDialog

					dlgPrinter.ShowNetwork = True

					If dlgPrinter.ShowDialog() = DialogResult.OK Then
						m_pdfPrinterSettings = dlgPrinter.PrinterSettings
					Else
						Return False

					End If

				End If
				If Not m_CurrentExportPrintInFiles Then
					Dim pdfDocument As New PdfDocumentProcessor()
					Dim pdfPrinterSettings As New PdfPrinterSettings(m_pdfPrinterSettings)
					pdfDocument.LoadDocument(PDFFiles(0).ToString)

					For i As Integer = 1 To PDFFiles.Count - 1
						pdfDocument.AppendDocument(PDFFiles(i))
						pdfDocument.SaveDocument(PDFFiles(0).ToString)
					Next

					pdfDocument.Print(pdfPrinterSettings)
				End If


			Catch LlException As LL_User_Aborted_Exception
				Return False

			Catch LlException As Exception
				m_Logger.LogError(String.Format("LlException: {0}", LlException.ToString))
				result = False

			Finally

			End Try


			Return result

		End Function

		Function CreateAssignedReportWithO2SAndSendToPrinter() As Boolean
			Dim result As Boolean = True
			Dim rpDocGuid As String = String.Empty
			'Dim reportNumber As Integer
			'Dim reportlineNumber As Integer

			Try
				Dim currentInoviceData = m_invoiceData.Where(Function(data) data.RENr = m_CurrentinvoiceNumber).FirstOrDefault()

				Dim reportData = m_ListingDatabaseAccess.LoadReportDataForPrintedInvoice(m_InitializationData.MDData.MDNr, m_CurrentinvoiceNumber)
				If reportData Is Nothing Then
					m_Logger.LogError(String.Format("Rapport-Scan für die Rechnung {0} wurde nicht gefunden.", m_CurrentinvoiceNumber))

					Return Nothing
				End If
				If reportData.Count = 0 Then Return Nothing

				Dim PDFFiles As New List(Of String)
				Dim CreateReportFile As Boolean = True

				For Each report In reportData
					'reportNumber = report.RPNr
					'reportlineNumber = report.RPLNr
					If rpDocGuid.Contains(report.RPDoc_Guid) Then
						CreateReportFile = False
						m_Logger.LogDebug(String.Format("report was allready printed: {0} !", report.RPDoc_Guid))
					Else
						CreateReportFile = True
						rpDocGuid &= String.Format(" | {0}", report.RPDoc_Guid)
						m_Logger.LogDebug(String.Format("report will be printed: {0}", report.RPDoc_Guid))
					End If

					If CreateReportFile Then

						Dim bytes() = report.DocScan
						Dim tempFileName = System.IO.Path.GetTempFileName()
						Dim fileExtension = report.Extension

						Dim tempFileFinal = System.IO.Path.ChangeExtension(tempFileName, fileExtension)
						If bytes Is Nothing OrElse Not m_Utility.WriteFileBytes(tempFileFinal, bytes) Then Return Nothing

						PDFFiles.Add(tempFileFinal)

						If Not m_CurrentExportPrintInFiles Then
							Dim splitedFiles = SplitPDF(tempFileFinal, Path.Combine(m_path.GetSpSREHomeFolder, "_"))
							m_Logger.LogDebug(String.Format("count of report pages: {0}", splitedFiles.Count))

							For Each pdffile In splitedFiles

								Dim tempSourceFileName = System.IO.Path.GetTempFileName()
								Dim tempReportFinalFile = System.IO.Path.ChangeExtension(tempSourceFileName, ".JPG")
								Dim reportJPGFile = ConvertPDFToGrafikAsArray(pdffile, System.Drawing.Imaging.ImageFormat.Jpeg)
								'Dim reportJPGFile = ConvertPDFToGrafikAsArray(tempFileFinal, System.Drawing.Imaging.ImageFormat.Jpeg)

								m_reportScanFile = reportJPGFile
								result = result AndAlso PrintAssignedReportScan(m_firstCallReport, report)

								m_firstCallReport = False

							Next
						End If

					End If

				Next
				If m_CurrentExportPrintInFiles AndAlso PDFFiles.Count > 0 Then
					For Each itm In PDFFiles
						PrintData.ExportedFiles.Add(itm)
					Next
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))
				result = Nothing

			Finally

			End Try


			Return result

		End Function

		Private Function SetLLVariable(ByVal LL As ListLabel) As Boolean
			Dim result As Boolean = True
			Dim aValue As New List(Of String)
			Dim i As Integer = 0

			' Zusätzliche Variable einfügen
			LL.Variables.Add("AutorFName", m_InitializationData.UserData.UserFName)
			LL.Variables.Add("AutorLName", m_InitializationData.UserData.UserLName)

			LL.Variables.Add("strOPCopy", If(PrintData.PrintInvoiceAsCopy.GetValueOrDefault(False), m_Translate.GetSafeTranslationValue("Kopie"), String.Empty))

			LL.Variables.Add("FormatedRefNr", GetFormatedRefData(m_invoiceRefNumber))

			For i = 0 To 10
				LL.Variables.Add(String.Format("RPNr_{0}", i), 0)
				LL.Variables.Add(String.Format("KSTBez_{0}", i), String.Empty)
				LL.Variables.Add(String.Format("LABez_{0}", i), String.Empty)
				LL.Variables.Add(String.Format("Anzahl_{0}", i), 0)
				LL.Variables.Add(String.Format("Betrag_{0}", i), 0)
			Next

			Dim rpData = m_ListingDatabaseAccess.LoadInvoiceReportData(m_InitializationData.MDData.MDNr, m_CurrentinvoiceNumber)
			If rpData Is Nothing Then
				m_Logger.LogError(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden."))

				Return False
			End If

			i = 0
			For Each rp In rpData

				LL.Variables.Add(String.Format("RPNr_{0}", i), rp.RPNr)
				LL.Variables.Add(String.Format("KSTBez_{0}", i), rp.KSTBez)
				LL.Variables.Add(String.Format("LABez_{0}", i), rp.LABez)
				LL.Variables.Add(String.Format("Anzahl_{0}", i), rp.Anzahl)
				LL.Variables.Add(String.Format("Betrag_{0}", i), rp.Betrag)

				i += 1
			Next
			result = result AndAlso GetInvoiceRPLGroupData4Print(LL)
			result = result AndAlso GetMDUSData4Print(LL)


			Return result

		End Function

		Private Function GetInvoiceRPLGroupData4Print(ByVal LL As ListLabel) As Boolean
			Dim result As Boolean = True
			Dim i As Integer = 0

			For i = 0 To 20
				LL.Variables.Add(String.Format("GroupedLABez_{0}", i), String.Empty)
				LL.Variables.Add(String.Format("GroupedLAAnzahl_{0}", i), 0)
				LL.Variables.Add(String.Format("GroupedLABetrag_{0}", i), 0)
			Next

			For i = 0 To 20
				LL.Variables.Add(String.Format("KSTGroupedName_{0}", i), String.Empty)
				LL.Variables.Add(String.Format("KSTGroupedLABez_{0}", i), String.Empty)
				LL.Variables.Add(String.Format("KSTGroupedLAAnzahl_{0}", i), 0)
				LL.Variables.Add(String.Format("KSTGroupedLABetrag_{0}", i), 0)
			Next


			Try
				Dim laGroupedData = m_ListingDatabaseAccess.LoadInvoiceReportLinesGroupedData(m_InitializationData.MDData.MDNr, m_CurrentinvoiceNumber)
				If laGroupedData Is Nothing Then
					m_Logger.LogError(m_Translate.GetSafeTranslationValue("Keine Rapportzeile für Groupierung wurde gefunden."))

					Return False
				End If

				i = 0
				For Each reportline In laGroupedData
					If i > 20 Then Exit For

					LL.Variables.Add(String.Format("GroupedLABez_{0}", i), reportline.LABez)
					LL.Variables.Add(String.Format("GroupedLAAnzahl_{0}", i), reportline.Anzahl)
					LL.Variables.Add(String.Format("GroupedLABetrag_{0}", i), reportline.Betrag)

					i += 1
				Next

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

			Try
				Dim laKSTGroupedData = m_ListingDatabaseAccess.LoadInvoiceReportLinesGroupedByKSTData(m_InitializationData.MDData.MDNr, m_CurrentinvoiceNumber)
				If laKSTGroupedData Is Nothing Then
					m_Logger.LogError(m_Translate.GetSafeTranslationValue("Keine Rapportzeile für Groupierung der Kostenstellen wurde gefunden."))

					Return False
				End If

				i = 0
				For Each reportline In laKSTGroupedData
					If i > 20 Then Exit For

					LL.Variables.Add(String.Format("KSTGroupedName_{0}", i), reportline.KSTBez)
					LL.Variables.Add(String.Format("KSTGroupedLABez_{0}", i), reportline.LABez)
					LL.Variables.Add(String.Format("KSTGroupedLAAnzahl_{0}", i), reportline.Anzahl)
					LL.Variables.Add(String.Format("KSTGroupedLABetrag_{0}", i), reportline.Betrag)

					i += 1
				Next

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try


			Return result

		End Function

		Private Function GetMDUSData4Print(ByVal LL As ListLabel) As Boolean
			Dim result As Boolean = True

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

			' additional mandantdata
			Dim MandantData = LoadAdditionalMandantData()
			If MandantData Is Nothing Then
				m_Logger.LogError(m_Translate.GetSafeTranslationValue("Zusätzliche Mandanten Daten konnten nicht geladen werden."))

				Return False

			End If
			LL.Variables.Add("BewName", MandantData.BewName)
			LL.Variables.Add("BewStr", MandantData.BewStrasse)
			LL.Variables.Add("BewOrt", MandantData.BewPLZOrt)
			LL.Variables.Add("BewNameAus", MandantData.BewSeco)
			LL.Variables.Add("MwStProzent", MandantData.mwstProzent)
			LL.Variables.Add("MwStNr", MandantData.mwstNumber)

			LL.Variables.Add("BURNumber", MandantData.BURNumber)
			LL.Variables.Add("UIDNumber", MandantData.UIDNumber)
			LL.Variables.Add("bvgname", MandantData.bvgname)
			LL.Variables.Add("bvgagnr", MandantData.bvgagnr)
			LL.Variables.Add("farNr", MandantData.farNr)


			Return result

		End Function

		Private Function DefineData(ByVal LL As ListLabel, ByVal data As SP.DatabaseAccess.Listing.DataObjects.InvoicePrintData) As Boolean
			Dim result As Boolean = True

			Try
				Dim printEmployeeAsAnonym As Boolean = PrintData.PrintEmployeeAsAnonym.GetValueOrDefault(False)
				LL.Core.LlDefineVariableExt("PrintEmployeeAsAnonym", ReplaceMissing(printEmployeeAsAnonym, False), LlFieldType.Boolean)

				LL.Core.LlDefineVariableExt("RENr", ReplaceMissing(data.RENr, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("Bemerk_1", ReplaceMissing(data.Bemerk_1, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Bemerk_2", ReplaceMissing(data.Bemerk_2, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Bemerk_3", ReplaceMissing(data.Bemerk_3, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Bemerk_LO", ReplaceMissing(data.Bemerk_LO, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Bemerk_P", ReplaceMissing(data.Bemerk_P, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Bemerk_RE", ReplaceMissing(data.Bemerk_RE, String.Empty), LlFieldType.Text)

				LL.Core.LlDefineVariableExt("BetragEx", ReplaceMissing(data.BetragEx, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("BetragInk", ReplaceMissing(data.BetragInk, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("Bezahlt", ReplaceMissing(data.Bezahlt, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("BetragOhne", ReplaceMissing(data.BetragOhne, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("BisDate", ReplaceMissing(data.BisDate, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("btrFr", ReplaceMissing(data.btrFr, 0), LlFieldType.Numeric_Localized)
				If String.Format("{0:F0}", data.BtrRP).Length = 1 Then
					LL.Core.LlDefineVariableExt("BtrRP", "0" & ReplaceMissing(data.BtrRP, String.Empty), LlFieldType.Text)
				Else
					LL.Core.LlDefineVariableExt("BtrRP", ReplaceMissing(data.BtrRP, String.Empty), LlFieldType.Text)
				End If
				LL.Core.LlDefineVariableExt("CreatedFrom", ReplaceMissing(data.CreatedFrom, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("CreatedOn", ReplaceMissing(data.CreatedOn, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("Currency", ReplaceMissing(data.Currency, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("DismissalFor", ReplaceMissing(data.DismissalFor, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("DismissalKind", ReplaceMissing(data.DismissalKind, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("DismissalOn", ReplaceMissing(data.DismissalOn, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("DismissalWho", ReplaceMissing(data.DismissalWho, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("DTAKontoNr", ReplaceMissing(data.DTAKontoNr, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Ende", ReplaceMissing(data.Ende, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("ESArt", ReplaceMissing(data.ESArt, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("ESNr", ReplaceMissing(data.ESNr, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("ESR_BCNr", ReplaceMissing(data.ESR_BCNr, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("ESR_IBAN1", ReplaceMissing(data.ESR_IBAN1, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("ESR_IBAN2", ReplaceMissing(data.ESR_IBAN2, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("ESR_IBAN3", ReplaceMissing(data.ESR_IBAN3, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("ESR_Swift", ReplaceMissing(data.ESR_Swift, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("ESRBankAdresse", ReplaceMissing(data.ESRBankAdresse, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("ESRBankName", ReplaceMissing(data.ESRBankName, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("ESRID", ReplaceMissing(data.ESRID, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("ESRKonto", ReplaceMissing(data.ESRKonto, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Faellig", ReplaceMissing(data.Faellig, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("Fak_Dat", ReplaceMissing(data.Fak_Dat, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("ValutaDate", ReplaceMissing(data.ValutaData, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("GAV_FAG", ReplaceMissing(data.GAV_FAG, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("GAV_VAG", ReplaceMissing(data.GAV_VAG, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("GAV_WAG", ReplaceMissing(data.GAV_WAG, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("GAVGruppe0", ReplaceMissing(data.GAVGruppe0, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("IBANDTA", ReplaceMissing(data.IBANDTA, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("K_Ansatz", ReplaceMissing(data.K_Ansatz, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("K_Anzahl", ReplaceMissing(data.K_Anzahl, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("K_Basis", ReplaceMissing(data.K_Basis, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("K_Betrag", ReplaceMissing(data.K_Betrag, 0), LlFieldType.Numeric_Localized)

				LL.Core.LlDefineVariableExt("KDNr", ReplaceMissing(data.KDNr, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("KDFirma1", ReplaceMissing(data.KDFirma1, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("KDFirma2", ReplaceMissing(data.KDFirma2, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("KDFirma3", ReplaceMissing(data.KDFirma3, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("KDPostfach", ReplaceMissing(data.KDPostfach, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("KDStrasse", ReplaceMissing(data.KDStrasse, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("KDPLZ", ReplaceMissing(data.KDPLZ, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("KDOrt", ReplaceMissing(data.KDOrt, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("KDLand", ReplaceMissing(data.KDLand, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("KDZHDData", ReplaceMissing(data.KDZHDData, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("KDZHDNr", ReplaceMissing(data.KDZHDNr, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("KDMwStNr", ReplaceMissing(data.KDMwStNr, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("KontoNr", ReplaceMissing(data.KontoNr, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("KST", ReplaceMissing(data.KST, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("KST_Ort_PLZ", ReplaceMissing(data.KST_Ort_PLZ, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("KST_PK_PLZ", ReplaceMissing(data.KST_PK_PLZ, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("KST_Res_Info_1", ReplaceMissing(data.KST_Res_Info_1, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("KST_Res_Info_2", ReplaceMissing(data.KST_Res_Info_2, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("KstBez", ReplaceMissing(data.KstBez, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("KSTNr", ReplaceMissing(data.KSTNr, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("LANr", ReplaceMissing(data.LANr, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("LAOPText", ReplaceMissing(data.LAOPText, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("MAName", If(printEmployeeAsAnonym, String.Empty, ReplaceMissing(data.MAName, String.Empty)), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("MANr", ReplaceMissing(data.MANr, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("MDEMail", ReplaceMissing(data.MDEMail, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("MDHomepage", ReplaceMissing(data.MDHomepage, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("MwSt", ReplaceMissing(data.MwSt, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("MwSt1", ReplaceMissing(data.MwSt1, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("MwStProz", ReplaceMissing(data.MwStProz, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("OPVersand", ReplaceMissing(data.OPVersand, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("PLZOrt", ReplaceMissing(data.PLZOrt, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("R_Abteilung", ReplaceMissing(data.R_Abteilung, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("R_Land", ReplaceMissing(data.R_Land, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("R_Name1", ReplaceMissing(data.R_Name1, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("R_Name2", ReplaceMissing(data.R_Name2, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("R_Name3", ReplaceMissing(data.R_Name3, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("R_Postfach", ReplaceMissing(data.R_Postfach, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("R_Strasse", ReplaceMissing(data.R_Strasse, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("R_ZHD", ReplaceMissing(data.R_ZHD, String.Empty), LlFieldType.Text)

				LL.Core.LlDefineVariableExt("RefFootNr", ReplaceMissing(data.RefFootNr, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("RefNr", ReplaceMissing(data.RefNr, String.Empty), LlFieldType.Text)

				Dim printOpenAmount As Boolean = PrintData.PrintOpenAmount.GetValueOrDefault(False) AndAlso (data.Art <> "G" AndAlso data.Art <> "R") AndAlso data.Bezahlt.GetValueOrDefault(0) > 0
				LL.Core.LlDefineVariableExt("PrintOpenAmount", ReplaceMissing(printOpenAmount, False), LlFieldType.Boolean)

				If printOpenAmount AndAlso data.BetragInk.GetValueOrDefault(0) > data.Bezahlt.GetValueOrDefault(0) Then
					Dim amountOpen = data.BetragInk.GetValueOrDefault(0) - data.Bezahlt.GetValueOrDefault(0)
					Dim referenceUtility As New SP.Infrastructure.ReferenceNumberUtility
					Dim refNumbers = referenceUtility.FormatReferenceNumbers(data.ESRID, data.KDNr, data.RENr, amountOpen, data.ESRKonto, data.ESRKonto, m_ReferenceNumbersTo10Setting)

					LL.Core.LlDefineVariableExt("RefFootNr", ReplaceMissing(refNumbers.Item2, String.Empty), LlFieldType.Text)
					LL.Core.LlDefineVariableExt("RefNr", ReplaceMissing(refNumbers.Item1, String.Empty), LlFieldType.Text)

					Dim frAmountOpen = Math.Truncate(amountOpen)
					Dim rpAmountOpen = Split(String.Format("{0:F2}", amountOpen), ".")(1)

					LL.Core.LlDefineVariableExt("btrFr", ReplaceMissing(frAmountOpen, 0), LlFieldType.Numeric_Localized)
					If String.Format("{0:F0}", rpAmountOpen).Length = 1 Then
						LL.Core.LlDefineVariableExt("BtrRP", "0" & ReplaceMissing(rpAmountOpen, String.Empty), LlFieldType.Text)
					Else
						LL.Core.LlDefineVariableExt("BtrRP", ReplaceMissing(rpAmountOpen, String.Empty), LlFieldType.Text)
					End If

				End If

				LL.Core.LlDefineVariableExt("RENr", ReplaceMissing(data.RENr, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("RPBis", ReplaceMissing(data.RPBis, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("RPLNr", ReplaceMissing(data.RPLNr, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("RPNr", ReplaceMissing(data.RPNr, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("RPText", ReplaceMissing(data.RPText, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("RPVon", ReplaceMissing(data.RPVon, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("RPZusatztext", ReplaceMissing(data.RPZusatztext, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Send2WOS", ReplaceMissing(data.Send2WOS, False), LlFieldType.Boolean)
				LL.Core.LlDefineVariableExt("VonDate", ReplaceMissing(data.VonDate, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("ZahlKond", ReplaceMissing(data.ZahlKond, String.Empty), LlFieldType.Text)


				' custom invoice
				LL.Core.LlDefineVariableExt("IndMWST", ReplaceMissing(data.IndMWST, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("IndBetragEx", ReplaceMissing(data.IndBetragEx, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("BetragTotal", ReplaceMissing(data.BetragTotal, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("RE_HeadText", ReplaceMissing(data.RE_HeadText, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("RE_Text", ReplaceMissing(data.RE_Text, String.Empty), LlFieldType.Text)



			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				result = False

			End Try


			Return result

		End Function

		Private Function DefineReportData(ByVal LL As ListLabel, ByVal data As ReportForPrintInvoiceData) As Boolean
			Dim result As Boolean = True

			Try
				LL.Core.Variables.Add("RENr", ReplaceMissing(data.RENr, 0), LlFieldType.Numeric_Localized)
				LL.Core.Variables.Add("ESNr", ReplaceMissing(data.ESNr, 0), LlFieldType.Numeric_Localized)
				LL.Core.Variables.Add("RPLNr", ReplaceMissing(data.RPLNr, 0), LlFieldType.Numeric_Localized)
				LL.Core.Variables.Add("RPNr", ReplaceMissing(data.RPNr, 0), LlFieldType.Numeric_Localized)

				LL.Core.Variables.Add("FullFilename", m_reportScanFile, LlFieldType.Text)


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				result = False

			End Try


			Return result

		End Function

		Private Function DefineDataForReport(ByVal LL As ListLabel, ByVal data As SP.DatabaseAccess.Listing.DataObjects.ReportForPrintInvoiceData) As Boolean
			Dim result As Boolean = True

			Try
				LL.Core.LlDefineVariableExt("ID", data.ID, LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("RENr", data.RENr, LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("RPNr", data.RPNr, LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("RPLNr", data.RPLNr, LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("ESNr", data.ESNr, LlFieldType.Numeric_Localized)

				LL.Core.LlDefineVariableExt("ChangedOn", data.ChangedOn, LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("ChangedFrom", data.ChangedFrom, LlFieldType.Text)

				LL.Core.LlDefineVariableExt("Beschreibung", data.Beschreibung, LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Extension", data.Extension, LlFieldType.Text)


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				result = False

			End Try


			Return result

		End Function


#End Region


#Region "Helpers"

		Private Function LoadAdditionalMandantData() As AdditionalMandantData

			Dim m_MandantXMLFile = m_MandantData.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, m_invoiceYear)
			m_MandantSettingsXml = New SPProgUtility.CommonXmlUtility.SettingsXml(m_MandantXMLFile)

			Dim data = New AdditionalMandantData

			data.BewName = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/BewName", m_SonstigesSetting))
			data.BewPostfach = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/BewPostfach", m_SonstigesSetting))
			data.BewStrasse = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/BewStrasse", m_SonstigesSetting))
			data.BewPLZOrt = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/BewPLZOrt", m_SonstigesSetting))
			data.BewSeco = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/BewSeco", m_SonstigesSetting))

			data.mwstProzent = Val(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/mwstsatz", m_InvoiceSetting)))
			data.mwstNumber = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/mwstnr", m_InvoiceSetting))

			data.BURNumber = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/BURNumber", m_SonstigesSetting))
			data.UIDNumber = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/UIDNumber", m_SonstigesSetting))
			data.bvgname = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/bvgname", m_SonstigesSetting))
			data.bvgagnr = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/bvgagnr", m_SonstigesSetting))
			data.farNr = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/FAR.-MitgliedNr", m_SonstigesSetting))


			Return data

		End Function

		Private Function GetFormatedRefData(ByVal refData As String) As String
			Dim result As String = refData

			Dim ref(7) As String
			refData = refData.Replace(" ", "")

			ref(0) = Mid(refData, 1, Len(refData) Mod 5)
			ref(1) = Mid(refData, Len(refData) Mod 5 + 1, 5)
			ref(2) = Mid(refData, Len(refData) Mod 5 + 6, 5)
			ref(3) = Mid(refData, Len(refData) Mod 5 + 11, 5)
			ref(4) = Mid(refData, Len(refData) Mod 5 + 16, 5)
			ref(5) = Mid(refData, Len(refData) Mod 5 + 21, 5)
			ref(6) = Mid(refData, Len(refData) Mod 5 + 26, 5)

			result = String.Join(" ", refData(0), refData(1), refData(2), refData(3), refData(4), refData(5), refData(6))


			Return result

		End Function

		Private Function GetPrintJobNumber(ByVal art As String, ByVal language As String, ByVal creditInvoiceAutomated As Boolean, ByVal JobForEZ As Boolean)
			Dim result As String
			Dim ezJobNr As String = If(JobForEZ, ".1", String.Empty)

			Select Case True
				Case language = "FR", language = "F"
					language = ".F"

				Case language = "IT", language = "I"
					language = ".I"


				Case Else
					language = ""
			End Select

			Select Case True
				Case art = "A"
					result = String.Format("5.6{0}{1}", ezJobNr, language)

				Case art = "I", art = "F"
					result = String.Format("5.5{0}{1}", ezJobNr, language)

				Case art = "G"
					result = String.Format("5.7{0}{1}", language, If(creditInvoiceAutomated, ".A", String.Empty))

				Case art = "R"
					result = String.Format("5.7{0}", language)

				Case Else
					result = String.Format("5.6{0}", language)


			End Select

			Return result
		End Function

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


#Region "WOS-Methods"

		Private Function TransferCustomerInvoiceIntoWOS(ByVal currentInoviceData As InvoiceData) As WOSSendResult
			Dim result As WOSSendResult = New WOSSendResult With {.Value = False, .Message = "Initialisierung..."}
			If currentInoviceData.Send2WOS AndAlso Not PrintData.WOSSendValueEnum = InvoicePrintData.WOSValue.PrintWithoutSending Then
				Dim wos = New SP.Internal.Automations.WOSUtility.CustomerExport(m_InitializationData)

				Dim wosSetting = New WOSSendSetting

				wosSetting.InvoiceNumber = m_CurrentinvoiceNumber
				wosSetting.CustomerNumber = currentInoviceData.KDNr
				wosSetting.CustomerGuid = currentInoviceData.CustomerGuid
				wosSetting.DocumentArtEnum = WOSSendSetting.DocumentArt.Rechnung

				Dim pdfFilename As String = PrintData.ExportedFiles(0).ToString
				If PrintData.ExportedFiles.Count > 1 Then
					Dim pdfDocument As New PdfDocumentProcessor()
					pdfDocument.LoadDocument(pdfFilename)

					For i As Integer = 1 To PrintData.ExportedFiles.Count - 1
						pdfDocument.AppendDocument(PrintData.ExportedFiles(i))
						pdfDocument.SaveDocument(pdfFilename)
					Next
					pdfDocument.CloseDocument()
				End If

				wosSetting.DocumentInfo = String.Format(m_Translate.GetSafeTranslationValue("Rechnung: {0} ({1:d})"),
																																																m_CurrentinvoiceNumber, currentInoviceData.InvoiceDate)
				Dim fileByte() = m_Utility.LoadFileBytes(pdfFilename) 'fileitem)
				wosSetting.ScanDoc = fileByte
				Dim scanfileinfo = New FileInfo(pdfFilename)
				wosSetting.ScanDocName = scanfileinfo.Name

				Dim docGuid As String = currentInoviceData.DocGuid
				If String.IsNullOrWhiteSpace(docGuid) Then
					Dim newGuid As String = Guid.NewGuid.ToString
					Dim success As Boolean = True
					success = success AndAlso m_WOSDatabaseAccess.UpdateInvoiceGuidData(m_CurrentinvoiceNumber, newGuid)

					If success Then docGuid = newGuid
					If Not success Then Return New WOSSendResult With {.Value = False, .Message = "Rechnung konnte nicht aktualisiert werden (DocGuid)!"}
				End If
				currentInoviceData.DocGuid = docGuid

				wosSetting.AssignedDocumentGuid = docGuid

				wos.WOSSetting = wosSetting
				result = wos.TransferCustomerDocumentDataToWOS(True)

				PrintData.ExportedFiles.Clear()
			End If


			Return result

		End Function

		''' <summary>
		''' delete customer document from WOS
		''' </summary>
		Public Function DeleteCustomerInvoiceFromWOS(ByVal currentInoviceData As InvoiceData) As WOSSendResult
			Dim result As WOSSendResult = New WOSSendResult With {.Value = True}

			If currentInoviceData Is Nothing Then Return New WOSSendResult With {.Value = False}
			Dim wos = New SP.Internal.Automations.WOSUtility.CustomerExport(m_InitializationData)

			Dim wosSetting = New WOSSendSetting

			wosSetting.InvoiceNumber = currentInoviceData.RENr
			wosSetting.CustomerNumber = currentInoviceData.KDNr
			'wosSetting.CustomerGuid = currentInoviceData.CustomerGuid
			wosSetting.DocumentArtEnum = WOSSendSetting.DocumentArt.Rechnung
			wosSetting.CustomerDocumentGuid = currentInoviceData.DocGuid

			wos.WOSSetting = wosSetting
			result.Value = wos.DeleteTransferedCustomerDocument()
			If Not result.Value Then m_Logger.LogWarning(result.Message)

			Return result

		End Function

#End Region


#Region "Helper methodes"

		Private Function ConvertPDFToGrafikAsArray(ByVal strSourceFullFileName As String, ByVal imgEx As ImageFormat) As String
			Dim tempFileFinal = String.Empty

			If File.Exists(strSourceFullFileName) Then
				Try
					Dim strExtension As String = "JPG"
					Dim strFile = O2S.Components.PDFRender4NET.PDFFile.Open(strSourceFullFileName)
					strFile.SerialNumber = Me.GetPDFVW_O2SSerial
					Try

						If imgEx Is ImageFormat.Bmp Then
							strExtension = "bmp"
						ElseIf imgEx Is ImageFormat.Emf Then
							strExtension = "emf"
						ElseIf imgEx Is ImageFormat.Exif Then
							strExtension = "exif"
						ElseIf imgEx Is ImageFormat.Gif Then
							strExtension = "gif"
						ElseIf imgEx Is ImageFormat.Icon Then
							strExtension = "ico"
						ElseIf imgEx Is ImageFormat.Jpeg Then
							strExtension = "jpg"
						ElseIf imgEx Is ImageFormat.MemoryBmp Then
							strExtension = "bmp"
						ElseIf imgEx Is ImageFormat.Png Then
							strExtension = "png"
						ElseIf imgEx Is ImageFormat.Tiff Then
							strExtension = "tiff"
						ElseIf imgEx Is ImageFormat.Wmf Then
							strExtension = "wmf"
						End If

						Dim tempSourceFileName = System.IO.Path.GetTempFileName()
						tempFileFinal = System.IO.Path.ChangeExtension(tempSourceFileName, String.Format(".{0}", strExtension))

						Dim pageImage As Bitmap = strFile.GetPageImage(0, 96)

						pageImage.Save(tempFileFinal, imgEx)


						Return tempFileFinal


					Catch ex As Exception
						m_Logger.LogError(ex.ToString)
						tempFileFinal = String.Empty

					Finally
						strFile.Dispose()

					End Try

				Catch ex As Exception
					m_Logger.LogError(ex.ToString)
					tempFileFinal = String.Empty

				End Try

			End If


			Return tempFileFinal

		End Function

		''' <summary>
		''' Splits a pdf file into multiple pdf files.
		''' </summary>
		''' <param name="pdfFileFullPath">The full path of the pdf file.</param>
		''' <returns>A list of PDFObjects with complete PDFFileName property</returns>
		Private Function SplitPDF(ByVal pdfFileFullPath As String, ByVal splittedPDFsFolderFullPath As String) As List(Of String)
			Dim result As List(Of String) = New List(Of String)

			O2S.Components.PDF4NET.PDFFile.PDFFile.SerialNumber = Me.GetPDF4Net_O2SSerial
			Try
				Dim pdfFileInfo As FileInfo = New FileInfo(pdfFileFullPath)

				' The name template for the splitted pdf files.
				Dim splittedPDFPathTemplate As String = Path.Combine(splittedPDFsFolderFullPath, pdfFileInfo.Name)

				Dim pdfObjectList = New List(Of String)
				Dim pdffilename = pdfFileInfo.Name.ToLower
				If Directory.Exists(splittedPDFsFolderFullPath) Then
					If Not DeleteSplittedFolder(splittedPDFsFolderFullPath) Then
						result.Add(pdfFileFullPath)
						Return result
					End If
				End If
				Directory.CreateDirectory(splittedPDFsFolderFullPath)
				' Split the pdf
				O2S.Components.PDF4NET.PDFFile.PDFFile.SplitFile(pdfFileFullPath, splittedPDFPathTemplate)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				Return Nothing
			End Try

			Dim directoryInfo As DirectoryInfo = New DirectoryInfo(splittedPDFsFolderFullPath)
			For Each fileInfo In directoryInfo.GetFiles()
				result.Add(fileInfo.FullName)
			Next

			Return result

		End Function

		Private Function DeleteSplittedFolder(ByVal workingFolderPath As String) As Boolean
			Dim success As Boolean = True
			Dim directoryInfo As DirectoryInfo = New DirectoryInfo(workingFolderPath)

			Try
				System.IO.Directory.Delete(workingFolderPath, True)
			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				success = False
			End Try


			Return success

		End Function


#End Region



		Private Class AdditionalMandantData

			Public Property BewName As String
			Public Property BewPostfach As String
			Public Property BewStrasse As String
			Public Property BewPLZOrt As String
			Public Property BewSeco As String
			Public Property mwstProzent As Decimal
			Public Property mwstNumber As String
			Public Property BURNumber As String
			Public Property UIDNumber As String
			Public Property bvgname As String
			Public Property bvgagnr As String
			Public Property farNr As String

		End Class


	End Class




	Public Class InvoicePrintData

		Public Enum OrderByValue

			OrderByInvoiceNumber
			OrderByCustomerName
			OrderByInvoiceDate

		End Enum

		Public Enum WOSValue
			PrintWithoutSending
			PrintOtherSendWOS
			PrintAndSend
		End Enum

		Public Property frmhwnd As String

		Public Property ShowAsDesign As Boolean

		Public Property InvoiceNumbers As List(Of Integer)
		Public Property OrderByEnum As OrderByValue
		Public Property WOSSendValueEnum As WOSValue

		Public Property PrintInvoiceAsCopy As Boolean?
		Public Property WhatToPrintValueEnum As WhatToPrintValue

		Public Property ExportPrintInFiles As Boolean?

		Public Property ExportPath As String
		Public Property ExportedFiles As List(Of String)

		Public Property PrintOpenAmount As Boolean?
		Public Property PrintEmployeeAsAnonym As Boolean?

	End Class


End Namespace


