
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Listing.DataObjects
Imports combit.ListLabel25

Imports DevExpress.Pdf
Imports DevExpress.XtraPdfViewer
Imports System.Drawing.Printing

Imports SP.Internal.Automations.WOSUtility
Imports SP.Internal.Automations.WOSUtility.DataObjects
Imports System.Windows.Forms
Imports System.IO
Imports System.ComponentModel
Imports SP.DatabaseAccess.Invoice.DataObjects
Imports SP.DatabaseAccess.Invoice



Namespace InvoicePrint

	Public Class ClsPrintDunning

		Implements IDisposable
		Protected disposed As Boolean = False


#Region "private fields"

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		''' <summary>
		''' The invoice data access object.
		''' </summary>
		Private m_DunningPrintDatabaseAccess As IInvoiceDatabaseAccess

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
		Private m_PrintCreditOnDunningPage As Boolean

		Private m_CurrentinvoiceNumber As Integer?
		Private m_CurrentCustomerNumber As Integer?
		Private m_CustomerLanguage As String
		Private m_CurrentSPNumber As Integer?
		Private m_CurrentVerNumber As Integer?


		Private m_pdfPrinterSettings As PrinterSettings


#End Region


#Region "Public properties"

		Public Property PreselectionData As PreselectedDunningPrintData
		Public Property SelectedDunningData As BindingList(Of SP.DatabaseAccess.Listing.DataObjects.DunningPrintData)

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

			Dim connectionString = _setting.MDData.MDDbConn
			m_CommonDatabaseAccess = New CommonDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)
			m_ListingDatabaseAccess = New ListingDatabaseAccess(connectionString, _setting.UserData.UserLanguage)
			m_DunningPrintDatabaseAccess = New InvoiceDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)

			m_SonstigesSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_SONSTIGES_SETTING, m_InitializationData.MDData.MDNr)
			m_InvoiceSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_INVOICE_SETTING, m_InitializationData.MDData.MDNr)
			m_SuvaSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_SUVA_SETTING, m_InitializationData.MDData.MDNr)
			m_AHVSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_AHV_SETTING, m_InitializationData.MDData.MDNr)
			m_FAKSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_FAK_SETTING, m_InitializationData.MDData.MDNr)
			m_MandantSetting = String.Format(MANDANT_XML_MAIN_KEY, m_InitializationData.MDData.MDNr)

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


		Public Function PrintDunning() As PrintResult
			Dim success As Boolean = True
			Dim result As PrintResult = New PrintResult With {.Printresult = True}
			Dim firstCallAutomatedInvoice As Boolean = True
			Dim firstCallCustomInvoice As Boolean = True
			Dim firstCallCreditCustomInvoice As Boolean = True
			Dim firstCallAutomatedCreditCustomInvoice As Boolean = True
			Dim i As Integer = 0
			Dim printjobAsFirstjob As Boolean = True

			m_PrintCreditOnDunningPage = PrintNotDoneCreditOnDunningPage

			If String.IsNullOrWhiteSpace(PreselectionData.ExportPath) Then PreselectionData.ExportPath = m_path.GetSpSREHomeFolder
			m_pdfPrinterSettings = New PrinterSettings
			m_pdfPrinterSettings = Nothing

			PreselectionData.ExportedFiles = New List(Of String)

			For Each itm In SelectedDunningData
				m_CurrentCustomerNumber = itm.KDNr
				m_CurrentSPNumber = itm.SPNr
				m_CurrentVerNumber = itm.VerNr
				m_CustomerLanguage = itm.CustomerLanguage
				m_CurrentExportPrintInFiles = PreselectionData.ExportPrintInFiles.GetValueOrDefault(False)
				Dim jobNumber As String = GetPrintJobNumber(itm.SPNr + itm.VerNr)
				success = success AndAlso LoadPrintJobData(jobNumber)

				If PreselectionData.ShowAsDesign Then
					success = success AndAlso ShowAssignedInvoiceInDesign(itm)

					Return result

				Else

					success = success AndAlso FindAndSendJobToPrinter(printjobAsFirstjob, itm)

					If Not m_CurrentExportPrintInFiles Then
						printjobAsFirstjob = False

					End If
				End If

				If Not success Then Exit For
				i += 1
			Next


			Return result

		End Function


#Region "Private Methodes"

		Private Function FindAndSendJobToPrinter(ByVal printjobAsFirstjob As Boolean, recData As SP.DatabaseAccess.Listing.DataObjects.DunningPrintData) As Boolean
			Dim result As Boolean = True

			result = result AndAlso PrintAssignedDunning(printjobAsFirstjob, recData)
			If PreselectionData.PrintAssignedInvoices.GetValueOrDefault(False) Then result = result AndAlso StartAssignedInovicePrinting(printjobAsFirstjob, recData)

			'LL = New ListLabel

			Return result

		End Function

		Private Function LoadPrintJobData(ByVal jobNumberToPrint As String) As Boolean

			m_PrintJobData = m_ListingDatabaseAccess.LoadAssignedPrintJobData(m_InitializationData.MDData.MDNr, m_CustomerLanguage, jobNumberToPrint)


			Return Not (m_PrintJobData Is Nothing)

		End Function

		Private Function ShowAssignedInvoiceInDesign(recData As SP.DatabaseAccess.Listing.DataObjects.DunningPrintData) As Boolean
			Dim result As Boolean = True

			Try
				Dim invoiceData As List(Of InvoiceDunningPrintData)

				Select Case True
					Case m_CurrentSPNumber > 0 OrElse m_CurrentVerNumber > 0
						invoiceData = m_ListingDatabaseAccess.LoadAssignedCustomerDunningWithFeesPrintData(m_InitializationData.MDData.MDNr, recData, m_PrintCreditOnDunningPage, PreselectionData.DunningLevel, PreselectionData.DunningDate)

					Case Else
						invoiceData = m_ListingDatabaseAccess.LoadAssignedCustomerDunningPrintData(m_InitializationData.MDData.MDNr, recData, m_PrintCreditOnDunningPage, PreselectionData.DunningLevel, PreselectionData.DunningDate)

				End Select
				If invoiceData Is Nothing OrElse invoiceData.Count = 0 Then
					m_Logger.LogError(String.Format("Rechnung {0} wurde nicht gefunden.", m_CurrentinvoiceNumber))

					Return False
				End If

				InitLL(LL)
				LL.Variables.Clear()
				LL.Fields.Clear()

				result = result AndAlso SetLLVariable(LL)
				result = result AndAlso DefineData(LL, invoiceData(0))

				If result Then
					LL.Core.LlDefineLayout(CType(PreselectionData.frmhwnd, IntPtr), m_PrintJobData.LLDocLabel,
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

		Function PrintAssignedDunning(ByVal showPrinterBox As Boolean, recData As SP.DatabaseAccess.Listing.DataObjects.DunningPrintData) As Boolean
			Const LL_PRNOPT_COPIES_SUPPORTED As Integer = 3
			Dim result As Boolean = True
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
				Dim invoiceData As List(Of InvoiceDunningPrintData)

				Select Case True
					Case m_CurrentSPNumber > 0 OrElse m_CurrentVerNumber > 0
						invoiceData = m_ListingDatabaseAccess.LoadAssignedCustomerDunningWithFeesPrintData(m_InitializationData.MDData.MDNr, recData, m_PrintCreditOnDunningPage, PreselectionData.DunningLevel, PreselectionData.DunningDate)

					Case Else
						invoiceData = m_ListingDatabaseAccess.LoadAssignedCustomerDunningPrintData(m_InitializationData.MDData.MDNr, recData, m_PrintCreditOnDunningPage, PreselectionData.DunningLevel, PreselectionData.DunningDate)

				End Select
				If invoiceData Is Nothing Then
					m_Logger.LogError(String.Format("Rechnung {0} wurde nicht gefunden.", m_CurrentinvoiceNumber))

					Return False
				End If


				InitLL(LL)
				LL.Variables.Clear()
				LL.Fields.Clear()

				result = result AndAlso DefineData(LL, invoiceData(0))
				result = result AndAlso SetLLVariable(LL)

				LL.Core.LlPrintWithBoxStart(If(m_PrintJobData.LLDocName.ToUpper.EndsWith(".LST".ToUpper),
																			 LlProject.List, LlProject.Card),
																			 m_PrintJobData.LLDocName, LlPrintMode.Export,
																			 If(m_CurrentExportPrintInFiles, LlBoxType.None, LlBoxType.StandardAbort),
																			 CType(PreselectionData.frmhwnd, IntPtr), m_PrintJobData.LLDocLabel)

				If m_CurrentExportPrintInFiles Then
					LL.Core.LlPrintSetOptionString(LlPrintOptionString.Export, "PDF")
					LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.File", tempFileName)
					LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.Path", PreselectionData.ExportPath)
					LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.Quiet", "1")

					LL.Core.LlPrintSetOption(LlPrintOption.Dialog_DestinationMask, 2)

				Else
					LL.Core.LlPrintSetOption(LlPrintOption.Copies, m_PrintJobData.LLCopyCount)
					LL.Core.LlPrintSetOption(LL_PRNOPT_COPIES_SUPPORTED, LL.Core.LlPrintGetOption(LlPrintOption.Copies))

					If showPrinterBox Then LL.Core.LlPrintOptionsDialog(CType(PreselectionData.frmhwnd, IntPtr), m_PrintJobData.PrintBoxHeader)

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
					LL.Core.LlPreviewDisplay(m_PrintJobData.LLDocName, m_path.GetSpSTempFolder, CType(PreselectionData.frmhwnd, IntPtr))
					LL.Core.LlPreviewDeleteFiles(m_PrintJobData.LLDocName, m_path.GetSpSTempFolder)

					Return False
				End If
				If m_CurrentExportPrintInFiles Then PreselectionData.ExportedFiles.Add(Path.Combine(PreselectionData.ExportPath, tempFileName))


			Catch LlException As LL_User_Aborted_Exception
				Return False

			Catch LlException As Exception
				m_Logger.LogError(String.Format("LlException: {0}", LlException.ToString))
				result = False

			Finally

			End Try


			Return result

		End Function


		Private Function StartAssignedInovicePrinting(ByVal printjobAsFirstjob As Boolean, ByVal selectedData As DunningPrintData) As Boolean
			Dim ShowDesign As Boolean = False
			Dim result As PrintResult

			Dim invoiceData = m_ListingDatabaseAccess.LoadInvoiceForAssignedDunningData(m_InitializationData.MDData.MDNr, selectedData, PreselectionData.DunningLevel, PreselectionData.DunningDate)

			Dim _setting As New SPS.Listing.Print.Utility.InvoicePrint.InvoicePrintData With {.frmhwnd = PreselectionData.frmhwnd,
																																												.PrintInvoiceAsCopy = False,
																																												.ShowAsDesign = False,
																																												.ExportPrintInFiles = PreselectionData.ExportPrintInFiles,
																																												.WOSSendValueEnum = InvoicePrintData.WOSValue.PrintWithoutSending}
			Dim printUtil = New SPS.Listing.Print.Utility.InvoicePrint.ClsPrintInvoice(m_InitializationData)
			printUtil.PrintData = _setting
			Dim invoiceNumbers As New List(Of Integer)
			For Each itm In invoiceData
				invoiceNumbers.Add(itm.RENr)
			Next
			_setting.InvoiceNumbers = invoiceNumbers

			printUtil.PrintAsFirstJob = printjobAsFirstjob
			result = printUtil.PrintInvoice()

			printUtil.Dispose()
			If result.Printresult AndAlso _setting.ExportedFiles.Count > 0 Then
				For Each f In _setting.ExportedFiles
					PreselectionData.ExportedFiles.Add(f)
				Next
			End If
			If Not result.Printresult Then Return False



			'For Each itm In invoiceData
			'	_setting.InvoiceNumbers = New List(Of Integer)(New Integer() {itm.RENr})
			'	printUtil.PrintData = _setting

			'	printUtil.PrintAsFirstJob = printjobAsFirstjob
			'	result = printUtil.PrintInvoice()

			'	printUtil.Dispose()

			'	If result.Printresult AndAlso _setting.ExportedFiles.Count > 0 Then
			'		For Each f In _setting.ExportedFiles
			'			PreselectionData.ExportedFiles.Add(f)
			'		Next
			'	End If
			'	printjobAsFirstjob = False
			'	If Not result.Printresult Then Return False
			'Next


			Return True

		End Function


		Private Function SetLLVariable(ByVal LL As ListLabel) As Boolean
			Dim result As Boolean = True
			Dim aValue As New List(Of String)
			Dim i As Integer = 0

			' Zusätzliche Variable einfügen
			LL.Variables.Add("AutorFName", m_InitializationData.UserData.UserFName)
			LL.Variables.Add("AutorLName", m_InitializationData.UserData.UserLName)

			result = result AndAlso GetMDUSData4Print(LL)


			Return result

		End Function

		Private Function DefineData(ByVal LL As ListLabel, ByVal data As InvoiceDunningPrintData) As Boolean
			Dim result As Boolean = True

			Try
				LL.Core.LlDefineVariableExt("RENr", ReplaceMissing(data.ReNr, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("BetragEx", ReplaceMissing(data.BetragEx, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("BetragInk", ReplaceMissing(data.BetragInk, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("BetragOhne", ReplaceMissing(data.BetragOhne, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("Bezahlt", ReplaceMissing(data.Bezahlt, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("CreatedFrom", ReplaceMissing(data.CreatedFrom, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("CreatedOn", ReplaceMissing(data.CreatedOn, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("Currency", ReplaceMissing(data.Currency, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("ESRBankAdresse", ReplaceMissing(data.ESRBankAdresse, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("ESRBankName", ReplaceMissing(data.ESRBankName, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Faellig", ReplaceMissing(data.Faellig, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("Fak_Dat", ReplaceMissing(data.FakDat, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("MA0", ReplaceMissing(data.MA0, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("MA1", ReplaceMissing(data.MA1, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("MA2", ReplaceMissing(data.MA2, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("MA3", ReplaceMissing(data.MA3, String.Empty), LlFieldType.Date_Localized)

				LL.Core.LlDefineVariableExt("IBANDTA", ReplaceMissing(data.IBANDTA, String.Empty), LlFieldType.Text)

				LL.Core.LlDefineVariableExt("DTAPLZOrt", ReplaceMissing(data.DTAPLZOrt, String.Empty), LlFieldType.Text)

				LL.Core.LlDefineVariableExt("KDNr", ReplaceMissing(data.KdNr, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("KST", ReplaceMissing(data.KST, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("MwSt1", ReplaceMissing(data.MWST1, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("MwStProz", ReplaceMissing(data.MWSTProz, 0), LlFieldType.Numeric_Localized)

				LL.Core.LlDefineVariableExt("R_Name1", ReplaceMissing(data.RName1, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("R_Name2", ReplaceMissing(data.RName2, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("R_Name3", ReplaceMissing(data.RName3, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("R_Abteilung", ReplaceMissing(data.RAbteilung, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("R_Postfach", ReplaceMissing(data.RPostfach, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("R_Strasse", ReplaceMissing(data.RStrasse, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("R_ZHD", ReplaceMissing(data.RZHD, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("R_PLZ", ReplaceMissing(data.RPLZ, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("R_Ort", ReplaceMissing(data.ROrt, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("R_Land", ReplaceMissing(data.RLand, String.Empty), LlFieldType.Text)

				LL.Core.LlDefineVariableExt("RENr", ReplaceMissing(data.ReNr, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("ZahlKond", ReplaceMissing(data.Zahlkond, String.Empty), LlFieldType.Text)

				LL.Core.LlDefineVariableExt("ZEBis", ReplaceMissing(PreselectionData.DunningDate, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("ZEBis0", ReplaceMissing(data.ZEBis0, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("ZEBis1", ReplaceMissing(data.ZEBis1, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("ZEBis2", ReplaceMissing(data.ZEBis2, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("ZEBis3", ReplaceMissing(data.ZEBis3, String.Empty), LlFieldType.Date_Localized)

				LL.Core.LlDefineVariableExt("SP_Text", ReplaceMissing(data.SPText, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("btrFr", ReplaceMissing(data.SPBtrFr, 0), LlFieldType.Numeric_Localized)

				If String.Format("{0:F0}", data.SPBtrRr).Length = 1 Then
					LL.Core.LlDefineVariableExt("BtrRP", "0" & ReplaceMissing(data.SPBtrRr, String.Empty), LlFieldType.Text)
				Else
					LL.Core.LlDefineVariableExt("BtrRP", ReplaceMissing(data.SPBtrRr, String.Empty), LlFieldType.Text)
				End If

				LL.Core.LlDefineVariableExt("OP_BetragEx", ReplaceMissing(data.OPBetragEx, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("OP_BetragInk", ReplaceMissing(data.OPBetragInk, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("SP_Betrag", ReplaceMissing(data.SPBetrag, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("SP_BetragTotal", ReplaceMissing(data.SPBetragTotal, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("RefFootNr", ReplaceMissing(data.SPRefFootNr, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("RefNr", ReplaceMissing(data.SPRefNr, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("KontoNr", ReplaceMissing(data.SPKontoNr, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("ESRKonto", ReplaceMissing(data.SPESRKonto, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("ESRID", ReplaceMissing(data.SPESRID, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("EsrBcNr", ReplaceMissing(data.EsrBcNr, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("EsrIBAN1", ReplaceMissing(data.EsrIBAN1, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("EsrIBAN2", ReplaceMissing(data.EsrIBAN2, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("EsrIBAN3", ReplaceMissing(data.EsrIBAN3, String.Empty), LlFieldType.Text)

				result = result AndAlso GetInvoiceRPLGroupData4Print(LL, data)


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				result = False

			End Try


			Return result

		End Function

		Private Function GetInvoiceRPLGroupData4Print(ByVal LL As ListLabel, ByVal invoiceData As InvoiceDunningPrintData) As Boolean
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
				Dim laGroupedData = m_ListingDatabaseAccess.LoadInvoiceReportLinesGroupedData(invoiceData.MDNr, invoiceData.ReNr)
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
				Dim laKSTGroupedData = m_ListingDatabaseAccess.LoadInvoiceReportLinesGroupedByKSTData(invoiceData.MDNr, invoiceData.ReNr)
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


#End Region


#Region "Helpers"

		Private Function LoadAdditionalMandantData() As AdditionalMandantData

			Dim m_MandantXMLFile = m_MandantData.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Year(PreselectionData.DunningDate))
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

		Private Function GetPrintJobNumber(ByVal spVerNumber As Integer)
			Dim result As String

			Select Case PreselectionData.DunningLevel
				Case 0
					result = "7.1"

				Case 1
					result = String.Format("7.2{0}", If(spVerNumber > 0, ".1", String.Empty))

				Case 2
					result = String.Format("7.3{0}", If(spVerNumber > 0, ".1", String.Empty))

				Case 3
					result = "7.4"


				Case Else
					result = String.Empty

			End Select


			Return result
		End Function

		Sub InitLL(ByVal LL As ListLabel)
			Const LL_DLGBOXMODE_3DBUTTONS As Integer = 32768											 ' 0x8000
			Const LL_DLGBOXMODE_ALT10 As Integer = 11															 ' 0x000B
			Const LL_OPTION_INCLUDEFONTDESCENT As Integer = 79										 ' 79
			Const LL_OPTION_INCREMENTAL_PREVIEW As Integer = 135									 ' 135

			Const LL_OPTION_VARSCASESENSITIVE As Integer = 46											 ' 46
			Const LL_OPTION_IMMEDIATELASTPAGE As Integer = 64											 ' 64
			Const LL_OPTION_CONVERTCRLF As Integer = 21														 ' 21

			Const LL_OPTION_NOPARAMETERCHECK As Integer = 32											 ' 32
			Const LL_OPTION_XLATVARNAMES As Integer = 51													 ' 51

			Const LL_OPTION_ESC_CLOSES_PREVIEW As Integer = 102										 ' 102
			Const LL_OPTION_SUPERVISOR As Integer = 3															 ' 3
			Const LL_OPTION_UISTYLE As Integer = 99																 ' 99
			Const LL_OPTION_UISTYLE_OFFICE2003 As Integer = 2											 ' 2
			Const LL_OPTION_AUTOMULTIPAGE As Integer = 42													 ' 42
			Const LL_OPTION_SUPPORTPAGEBREAK As Integer = 10											 ' 10
			Const LL_OPTION_PRVZOOM_PERC As Integer = 25                                                     ' 25

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

			LL.Core.LlSetOption(LL_OPTION_IMMEDIATELASTPAGE, 1)				' Lastpage
			LL.Core.LlSetOption(LL_OPTION_CONVERTCRLF, 1)							' Doppelte Zeilenumbruch

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


		Private ReadOnly Property PrintNotDoneCreditOnDunningPage() As Boolean
			Get
				Dim ezonsepratedpage As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_MandantData.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr,
																																																																			 Year(PreselectionData.DunningDate)),
																																																																		 String.Format("MD_{0}/Debitoren/printguonmahnung", m_InitializationData.MDData.MDNr)), False)

				Return ezonsepratedpage.HasValue AndAlso ezonsepratedpage

			End Get
		End Property

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




	Public Class PreselectedDunningPrintData

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

		Public Property OrderByEnum As OrderByValue
		Public Property WOSSendValueEnum As WOSValue

		Public Property ExportPrintInFiles As Boolean?
		Public Property PrintAssignedInvoices As Boolean?

		Public Property ExportPath As String
		Public Property ExportedFiles As List(Of String)

		Public Property InvoiceNumbers As List(Of Integer)
		Public Property InvoiceCustomerNames As List(Of String)
		Public Property DunningLevel As Integer?
		Public Property DunningDate As Date?
		Public Property SPNumber As Integer?
		Public Property VerNumber As Integer?


	End Class



End Namespace
