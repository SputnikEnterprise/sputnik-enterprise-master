

'Imports System.Data.SqlClient
Imports System.IO
Imports combit.ListLabel25
Imports SPS.Listing.Print.Utility.MainUtilities
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath

Imports SP.Infrastructure
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Listing.DataObjects

Public Class ClsLLESRPrint
	Implements IDisposable


#Region "private constants"

	Private Const MODUL_TO_PRINT_TESR = "6.3"
	Private Const MODUL_TO_PRINT_BESR = "6.4"

#End Region


#Region "private fields"

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	Protected disposed As Boolean = False

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' The Listing data access object.
	''' </summary>
	Private m_ListingDatabaseAccess As IListingDatabaseAccess

	Private m_StartPrintInLLDebug As Boolean
	Private m_PrintJobData As PrintJobData
	Private m_ESRData As List(Of ESRListPrintData)
	Private m_ESRPaymentData As List(Of ESRPaymentListPrintData)


	Friend _ClsLLFunc As New ClsLLFunc
	Private m_path As New ClsProgPath
	Private m_MandantData As New Mandant


	Private LL As ListLabel = New ListLabel

#End Region


#Region "Private Properties"

	Public Property PrintSetting As ClsLLESRPrintSetting

#End Region


#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		m_MandantData = New Mandant

		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
		m_MandantData = New Mandant

		m_ListingDatabaseAccess = New ListingDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

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

#Region " IDisposable Support "
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


#End Region


#Region "Public Methodes"

	Public Function PrintESRList() As PrintResult
		Dim success As Boolean = True
		Dim result As PrintResult = New PrintResult With {.Printresult = True}

		PrintSetting.JobNr2Print = "6.3"
		success = success AndAlso LoadPrintJobData(PrintSetting.JobNr2Print)
		If Not success Then Return New PrintResult With {.Printresult = False, .PrintresultMessage = "Keine Vorlage wurde gefunden."}

		If PrintSetting.ShowAsDesgin Then
			success = success AndAlso ShowInDesign()

			Return result

		Else
			success = success AndAlso ShowInPrint(True)

		End If

		Return result
	End Function

	Public Function PrintESRPaymentList() As PrintResult
		Dim success As Boolean = True
		Dim result As PrintResult = New PrintResult With {.Printresult = True}

		PrintSetting.JobNr2Print = "6.4"
		success = success AndAlso LoadPrintJobData(PrintSetting.JobNr2Print)
		If Not success Then Return New PrintResult With {.Printresult = False, .PrintresultMessage = "Keine Vorlage wurde gefunden."}

		If PrintSetting.ShowAsDesgin Then
			success = success AndAlso ShowInDesign()

			Return result

		Else
			success = success AndAlso ShowInPrint(True)

		End If

		Return result
	End Function

#End Region

	Private Function ShowInDesign() As Boolean
		Dim result As Boolean = True

		Try
			If Me.PrintSetting.JobNr2Print = "6.3" Then
				result = result AndAlso LoadESRListData()

			Else
				result = result AndAlso LoadESRPaymentListData()

			End If
			If Not result Then Return False

			InitLL(LL)
			LL.Variables.Clear()
			LL.Fields.Clear()

			If Me.PrintSetting.JobNr2Print = "6.3" Then
				result = result AndAlso DefineESRData(LL, m_ESRData(0))
			Else
				result = result AndAlso DefineESRPaymentData(LL, m_ESRPaymentData(0))
			End If
			result = result AndAlso SetLLVariable(LL)

			LL.Core.LlDefineLayout(CType(PrintSetting.frmhwnd, IntPtr), m_PrintJobData.LLDocLabel,
								   If(m_PrintJobData.LLDocName.ToUpper.EndsWith(".LST".ToUpper), LlProject.List, LlProject.Card),
								   m_PrintJobData.LLDocName)

		Catch LlException As Exception
			m_Logger.LogError(String.Format("LlException.{0}", LlException.Message))

			result = False

		Finally

		End Try

		Return result
	End Function

	Function ShowInPrint(ByRef bShowBox As Boolean) As Boolean
		Const LL_PRNOPT_COPIES_SUPPORTED As Integer = 3
		Dim success As Boolean = True
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim strJobNr As String = Me.PrintSetting.JobNr2Print

		Try
			If m_StartPrintInLLDebug Then
				LL.Debug = LlDebug.LogToFile
				LL.Debug = LlDebug.Enabled
			End If
		Catch ex As Exception
			m_Logger.LogError(String.Format("EnablingLLDebuging: {0}", ex.ToString))

		End Try

		Try
			If Me.PrintSetting.JobNr2Print = "6.3" Then
				success = success AndAlso LoadESRListData()

			Else
				success = success AndAlso LoadESRPaymentListData()

			End If
			If Not success Then Return False

			InitLL(LL)
			LL.Variables.Clear()
			LL.Fields.Clear()

			If Me.PrintSetting.JobNr2Print = "6.3" Then
				success = success AndAlso DefineESRData(LL, m_ESRData(0))
			Else
				success = success AndAlso DefineESRPaymentData(LL, m_ESRPaymentData(0))
			End If
			success = success AndAlso SetLLVariable(LL)
			LL.Core.LlPrintWithBoxStart(If(m_PrintJobData.LLDocName.ToUpper.EndsWith(".LST".ToUpper), LlProject.List, LlProject.Card), m_PrintJobData.LLDocName, LlPrintMode.Export, LlBoxType.StandardAbort, CType(PrintSetting.frmhwnd, IntPtr), m_PrintJobData.LLDocLabel)

			LL.Core.LlPrintSetOption(LlPrintOption.Copies, m_PrintJobData.LLCopyCount)
			LL.Core.LlPrintSetOption(LL_PRNOPT_COPIES_SUPPORTED, LL.Core.LlPrintGetOption(LlPrintOption.Copies))

			If bShowBox Then
				LL.Core.LlPrintOptionsDialog(CType(PrintSetting.frmhwnd, IntPtr), String.Format("{1}: {2}{0}{3}", vbNewLine, m_PrintJobData.LLDocLabel, strJobNr, m_PrintJobData.LLDocName))
			End If
			Dim TargetFormat As String = LL.Core.LlPrintGetOptionString(LlPrintOptionString.Export)

			While Not LL.Core.LlPrint()
			End While

			If Me.PrintSetting.JobNr2Print = "6.3" Then

				For Each itm In m_ESRData

					' pass data for current record
					DefineESRData(LL, itm)

					While LL.Core.LlPrintFields() = LlConstants.WrnRepeatData
						LL.Core.LlPrint()
					End While

				Next

			Else
				For Each itm In m_ESRPaymentData

					' pass data for current record
					DefineESRPaymentData(LL, itm)

					While LL.Core.LlPrintFields() = LlConstants.WrnRepeatData
						LL.Core.LlPrint()
					End While

				Next

			End If

			While LL.Core.LlPrintFieldsEnd() = LlConstants.WrnRepeatData
			End While

			LL.Core.LlPrintEnd(0)

			If TargetFormat = "PRV" Then
				LL.Core.LlPreviewDisplay(m_PrintJobData.LLDocName, m_path.GetSpSTempFolder, CType(PrintSetting.frmhwnd, IntPtr))
				LL.Core.LlPreviewDeleteFiles(m_PrintJobData.LLDocName, m_path.GetSpSTempFolder)

				Return False
			End If
			success = True


		Catch LlException As LL_User_Aborted_Exception
			Return False

		Catch LlException As Exception
			m_Logger.LogError(String.Format("LlException.{0}: {1}", strMethodeName, LlException.Message))
			success = False

		Finally

		End Try

		Return success

	End Function

	Private Function LoadPrintJobData(ByVal jobNumberToPrint As String) As Boolean

		m_PrintJobData = m_ListingDatabaseAccess.LoadAssignedPrintJobData(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserLanguage, jobNumberToPrint)

		Return Not (m_PrintJobData Is Nothing)
	End Function

	Private Function LoadESRListData() As Boolean
		Dim result As Boolean = True

		m_ESRData = m_ListingDatabaseAccess.LoadESRDataForPrintList(m_InitializationData.MDData.MDNr, PrintSetting.DiskIdentity)

		Return result
	End Function

	Private Function LoadESRPaymentListData() As Boolean
		Dim result As Boolean = True
		'Dim paymentNumbers = New List(Of Integer)
		'paymentNumbers = m_PrintSetting.firstPaymentNumber.Select(Function(x) x.ToString.ToList())

		m_ESRPaymentData = m_ListingDatabaseAccess.LoadPaymentDataForESRPrintList(m_InitializationData.MDData.MDNr, PrintSetting.firstPaymentNumber.ToList)

		Return result
	End Function

	Private Sub InitLL(ByVal LL As ListLabel)
		Const LL_DLGBOXMODE_3DBUTTONS As Integer = 32768                       ' 0x8000
		Const LL_DLGBOXMODE_ALT10 As Integer = 11                              ' 0x000B
		Const LL_OPTION_INCLUDEFONTDESCENT As Integer = 79                                       ' 79
		Const LL_OPTION_INCREMENTAL_PREVIEW As Integer = 135                                     ' 135

		Const LL_OPTION_VARSCASESENSITIVE As Integer = 46                                            ' 46
		Const LL_OPTION_IMMEDIATELASTPAGE As Integer = 64                                            ' 64
		Const LL_OPTION_CONVERTCRLF As Integer = 21                                                      ' 21

		Const LL_OPTION_NOPARAMETERCHECK As Integer = 32                                             ' 32
		Const LL_OPTION_XLATVARNAMES As Integer = 51                                                     ' 51

		Const LL_OPTION_ESC_CLOSES_PREVIEW As Integer = 102                                      ' 102
		Const LL_OPTION_SUPERVISOR As Integer = 3                                                            ' 3
		Const LL_OPTION_UISTYLE As Integer = 99                                                              ' 99
		Const LL_OPTION_UISTYLE_OFFICE2003 As Integer = 2                                            ' 2
		Const LL_OPTION_AUTOMULTIPAGE As Integer = 42                                                    ' 42
		Const LL_OPTION_SUPPORTPAGEBREAK As Integer = 10                                             ' 10
		Const LL_OPTION_PRVZOOM_PERC As Integer = 25                                                     ' 25

		Const LL_OPTION_DELAYTABLEHEADER As Integer = LlOption.DelayTableHeader

		LL.LicensingInfo = ClsMainSetting.GetLL25LicenceInfo()
		LL.Language = LlLanguage.German

		LL.Core.LlSetOption(LL_OPTION_DELAYTABLEHEADER, 0)

		LlCore.LlSetDlgboxMode(LL_DLGBOXMODE_3DBUTTONS + LL_DLGBOXMODE_ALT10)

		' beim LL13 muss ich es so machen...
		LL.Core.LlSetOption(LL_OPTION_INCLUDEFONTDESCENT, 0)
		LL.Core.LlSetOption(LL_OPTION_INCREMENTAL_PREVIEW, 0)

		' beim LL13 muss ich es so machen...
		LL.Core.LlSetOption(LL_OPTION_INCLUDEFONTDESCENT, _ClsLLFunc.LLFontDesent)
		LL.Core.LlSetOption(LL_OPTION_INCREMENTAL_PREVIEW, _ClsLLFunc.LLIncPrv)

		LL.Core.LlSetOption(LL_OPTION_VARSCASESENSITIVE, 0)

		LL.Core.LlSetOption(LL_OPTION_IMMEDIATELASTPAGE, 1)             ' Lastpage
		LL.Core.LlSetOption(LL_OPTION_CONVERTCRLF, 1)                           ' Doppelte Zeilenumbruch

		LL.Core.LlSetOption(LL_OPTION_NOPARAMETERCHECK, _ClsLLFunc.LLParamCheck)
		LL.Core.LlSetOption(LL_OPTION_XLATVARNAMES, _ClsLLFunc.LLKonvertName)

		LL.Core.LlSetOption(LL_OPTION_ESC_CLOSES_PREVIEW, 1)
		LL.Core.LlSetOption(LL_OPTION_SUPERVISOR, 1)
		LL.Core.LlSetOption(LL_OPTION_UISTYLE, LL_OPTION_UISTYLE_OFFICE2003)
		LL.Core.LlSetOption(LL_OPTION_AUTOMULTIPAGE, 1)

		LL.Core.LlSetOption(LL_OPTION_SUPPORTPAGEBREAK, 1)
		LL.Core.LlSetOption(LL_OPTION_PRVZOOM_PERC, _ClsLLFunc.LLZoomProz)

		LL.Core.LlPreviewSetTempPath(m_path.GetSpSTempFolder)
		LL.Core.LlSetPrinterDefaultsDir(m_path.GetPrinterHomeFolder)

	End Sub

	Private Function SetLLVariable(ByVal LL As ListLabel) As Boolean
		Dim result As Boolean = True
		Dim aValue As New List(Of String)
		Dim i As Integer = 0
		Dim zeNumbersBuffer As String = String.Empty

		' Zusätzliche Variable einfügen
		LL.Variables.Add("AutorFName", m_InitializationData.UserData.UserFName)
		LL.Variables.Add("AutorLName", m_InitializationData.UserData.UserLName)

		LL.Variables.Add("SortBez", PrintSetting.ListSortBez)
		LL.Variables.Add("FilterBez", PrintSetting.ListFilterBez(0))

		LL.Variables.Add("Dokbez", _ClsLLFunc.ListBez)

		For Each number In Me.PrintSetting.firstPaymentNumber
			zeNumbersBuffer = zeNumbersBuffer & IIf(zeNumbersBuffer <> "", ", ", "") & number
		Next
		LL.Variables.Add("paymentNumber", zeNumbersBuffer)

		LL.Variables.Add("MDESRKontoNr", Me.PrintSetting.ESRKontoNumber)

		GetMDUSData4Print(LL)

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


		Dim MandantenData = m_ListingDatabaseAccess.LoadMandantDataForPrintDTAESRListData(m_InitializationData.MDData.MDNr, PrintSetting.MandantBankIDNumber)
		If Not MandantenData Is Nothing Then
			LL.Variables.Add("DTAClnr", MandantenData.DTAClnr)
			LL.Variables.Add("KontoDTA", MandantenData.KontoDTA)
			LL.Variables.Add("KontoVG", MandantenData.KontoVG)
			LL.Variables.Add("VGIBAN", MandantenData.VGIBAN)

			LL.Variables.Add("ESRfiledate", Me.PrintSetting.ESRfiledate)
			LL.Variables.Add("ESRFileName", Me.PrintSetting.ESRFileName)
		End If

		Return result
	End Function

	Private Function DefineESRData(ByVal LL As ListLabel, ByVal data As ESRListPrintData) As Boolean
		Dim result As Boolean = True

		Try
			LL.Core.LlDefineVariableExt("Betrag", ReplaceMissing(data.ESRBookedAmount, 0), LlFieldType.Numeric_Localized)
			LL.Core.LlDefineVariableExt("CreatedFrom", ReplaceMissing(data.Createdfrom, String.Empty), LlFieldType.Text)
			LL.Core.LlDefineVariableExt("CreatedOn", ReplaceMissing(data.CreatedOn, String.Empty), LlFieldType.Date_Localized)

			LL.Core.LlDefineVariableExt("DiskInfo", ReplaceMissing(data.DiskInfo, String.Empty), LlFieldType.Text)
			LL.Core.LlDefineVariableExt("ID", ReplaceMissing(data.ID, 0), LlFieldType.Numeric_Localized)
			LL.Core.LlDefineVariableExt("RENr", ReplaceMissing(data.InvoiceNumber, 0), LlFieldType.Numeric_Localized)
			LL.Core.LlDefineVariableExt("KDNr", ReplaceMissing(data.CustomerNumber, 0), LlFieldType.Numeric_Localized)
			LL.Core.LlDefineVariableExt("MDNr", ReplaceMissing(data.MDNr, 0), LlFieldType.Numeric_Localized)
			LL.Core.LlDefineVariableExt("OK", ReplaceMissing(data.AmountDecision, String.Empty), LlFieldType.Text)

			LL.Core.LlDefineVariableExt("PayedAmount", ReplaceMissing(data.ESRAmount, 0), LlFieldType.Numeric_Localized)
			LL.Core.LlDefineVariableExt("Rec", ReplaceMissing(data.Rec, String.Empty), LlFieldType.Text)

			LL.Core.LlDefineVariableExt("VD", ReplaceMissing(data.VD, String.Empty), LlFieldType.Date_Localized)
			LL.Core.LlDefineVariableExt("VT", ReplaceMissing(data.VT, String.Empty), LlFieldType.Text)

			LL.Core.LlDefineVariableExt("InvoiceAmount", ReplaceMissing(data.InvoiceAmount, 0), LlFieldType.Numeric_Localized)
			LL.Core.LlDefineVariableExt("InvoicePayed", ReplaceMissing(data.InvoicePayed, 0), LlFieldType.Numeric_Localized)
			LL.Core.LlDefineVariableExt("InvoiceOpenAmount", ReplaceMissing(data.InvoiceOpenAmount, 0), LlFieldType.Numeric_Localized)

			LL.Core.LlDefineVariableExt("R_Name1", ReplaceMissing(data.Company, String.Empty), LlFieldType.Text)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

			result = False

		End Try

		Return result
	End Function

	Private Function DefineESRPaymentData(ByVal LL As ListLabel, ByVal data As ESRPaymentListPrintData) As Boolean
		Dim result As Boolean = True

		Try
			LL.Core.LlDefineVariableExt("ZENr", ReplaceMissing(data.ZENr, 0), LlFieldType.Numeric_Localized)
			LL.Core.LlDefineVariableExt("KDNr", ReplaceMissing(data.CustomerNumber, 0), LlFieldType.Numeric_Localized)
			LL.Core.LlDefineVariableExt("MDNr", ReplaceMissing(data.MDNr, 0), LlFieldType.Numeric_Localized)
			LL.Core.LlDefineVariableExt("RENr", ReplaceMissing(data.InvoiceNumber, 0), LlFieldType.Numeric_Localized)

			LL.Core.LlDefineVariableExt("CreatedFrom", ReplaceMissing(data.Createdfrom, String.Empty), LlFieldType.Text)
			LL.Core.LlDefineVariableExt("CreatedOn", ReplaceMissing(data.CreatedOn, String.Empty), LlFieldType.Date_Localized)
			LL.Core.LlDefineVariableExt("B_Date", ReplaceMissing(data.BookedOn, String.Empty), LlFieldType.Date_Localized)
			LL.Core.LlDefineVariableExt("Fak_Dat", ReplaceMissing(data.Fak_Date, String.Empty), LlFieldType.Date_Localized)
			LL.Core.LlDefineVariableExt("V_Date", ReplaceMissing(data.ValutaOn, String.Empty), LlFieldType.Date_Localized)

			LL.Core.LlDefineVariableExt("Betrag", ReplaceMissing(data.Amount, 0), LlFieldType.Numeric_Localized)
			LL.Core.LlDefineVariableExt("InvoiceAmount", ReplaceMissing(data.InvoiceAmount, 0), LlFieldType.Numeric_Localized)
			LL.Core.LlDefineVariableExt("InvoicePayed", ReplaceMissing(data.InvoicePayed, 0), LlFieldType.Numeric_Localized)
			LL.Core.LlDefineVariableExt("InvoiceOpenAmount", ReplaceMissing(data.InvoiceOpenAmount, 0), LlFieldType.Numeric_Localized)

			LL.Core.LlDefineVariableExt("R_Name1", ReplaceMissing(data.Company, String.Empty), LlFieldType.Text)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

			result = False

		End Try

		Return result
	End Function


End Class
