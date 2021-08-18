
Imports System.IO
Imports System.IO.File
Imports System.Drawing
Imports System.Windows.Forms
Imports SPProgUtility.ClsProgSettingPath
Imports SPProgUtility.ProgPath
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Listing.DataObjects
Imports combit.ListLabel25

Imports DevExpress.XtraSplashScreen
Imports DevExpress.Pdf
Imports DevExpress.XtraPdfViewer
Imports System.Drawing.Printing

Imports SP.Internal.Automations.WOSUtility
Imports SP.Internal.Automations.WOSUtility.DataObjects
Imports System.Drawing.Imaging

Imports System.ComponentModel
Imports System.Text



Namespace ReportPrint

	Public Class CLSPrintingReportHours_KDKST

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

		Private m_invoiceRefNumber As String
		Private m_CurrentinvoiceNumber As Integer?
		Private m_CurrentCustomerNumber As Integer?
		Private m_CustomerLanguage As String
		Private m_NumberOfCopies As Integer?
		Private m_invoiceYear As Integer?
		Private m_reportScanFile As String
		Private m_firstCallReport As Boolean

		Private m_pdfPrinterSettings As PrinterSettings
		Private m_Provider As DataProviders.ObjectDataProvider
		Private m_ProjectType As LlProject

#End Region


#Region "Public properties"

		Public Property PrintData As HoursPrintData

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
			m_ListingDatabaseAccess = New ListingDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)

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


		Public Function PrintCustomerHours() As PrintResult
			Dim success As Boolean = True
			Dim result As PrintResult = New PrintResult With {.Printresult = True}
			Dim i As Integer = 0

			Dim jobNumber As String = PrintData.PrintJobNumber
			success = success AndAlso LoadPrintJobData(jobNumber)

			If PrintData.ShowAsDesign Then
				success = success AndAlso ShowAssignedListInDesign()

				Return result

			Else
				success = success AndAlso PrintAssignedList()

			End If
			SplashScreenManager.CloseForm(False)

			Return result

		End Function


#Region "Private Methodes"

		Private Function LoadPrintJobData(ByVal jobNumberToPrint As String) As Boolean

			m_PrintJobData = m_ListingDatabaseAccess.LoadAssignedPrintJobData(m_InitializationData.MDData.MDNr, m_CustomerLanguage, jobNumberToPrint)

			If m_PrintJobData.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				m_ProjectType = LlProject.List
			Else
				m_ProjectType = LlProject.Card
			End If


			Return Not (m_PrintJobData Is Nothing)

		End Function

		Private Function ShowAssignedListInDesign() As Boolean
			Dim result As Boolean = True
			Dim customerdata = PrintData.CustomerHoursToPrintData
			Dim employeedata = PrintData.EmployeeHoursToPrintData


			Try
				If customerdata Is Nothing AndAlso employeedata Is Nothing Then
					m_Logger.LogWarning("Keine Daten wurden gefunden.")

					Return False
				End If


				InitLL(LL)
				LL.Variables.Clear()
				LL.Fields.Clear()

				result = result AndAlso SetLLVariable(LL)

				SetupDataObject()
				LL.DataSource = m_Provider

				'If PrintData.SearchKindEnum = HourSearchTypeEnum.Customer Then
				'	result = result AndAlso DefineCustomerData(LL, customerdata(0))
				'Else
				'	result = result AndAlso DefineEmplyeeData(LL, employeedata(0))
				'End If

				If result Then
					SplashScreenManager.CloseForm(False)

					LL.Design(m_PrintJobData.LLDocLabel, m_ProjectType, m_PrintJobData.LLDocName, False)

					'LL.Core.LlDefineLayout(CType(PrintData.frmhwnd, IntPtr), m_PrintJobData.LLDocLabel,
					'											 If(m_PrintJobData.LLDocName.ToUpper.EndsWith(".LST".ToUpper), LlProject.List, LlProject.Card),
					'											 m_PrintJobData.LLDocName)
					'LL.Core.LlPrintEnd()
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
			Dim customerdata = PrintData.CustomerHoursToPrintData
			Dim employeedata = PrintData.EmployeeHoursToPrintData

			Try
				If m_StartPrintInLLDebug Then
					LL.Debug = LlDebug.LogToFile
					LL.Debug = LlDebug.Enabled
				End If
			Catch ex As Exception
				m_Logger.LogError(String.Format("EnablingLLDebuging: {0}", ex.ToString))

			End Try

			Try
				If customerdata Is Nothing AndAlso employeedata Is Nothing Then
					m_Logger.LogWarning("Keine Daten wurden gefunden.")

					Return False
				End If

				InitLL(LL)
				LL.Variables.Clear()
				LL.Fields.Clear()

				result = result AndAlso SetLLVariable(LL)
				SetupDataObject()

				If PrintData.SearchKindEnum = HourSearchTypeEnum.Customer Then
					result = result AndAlso DefineCustomerData(LL, customerdata(0))
				Else
					result = result AndAlso DefineEmplyeeData(LL, employeedata(0))
				End If

				SplashScreenManager.CloseForm(False)
				LL.Core.LlPrintWithBoxStart(If(m_PrintJobData.LLDocName.ToUpper.EndsWith(".LST".ToUpper),
																			 LlProject.List, LlProject.Card),
																			 m_PrintJobData.LLDocName, LlPrintMode.Export,
																			 If(m_CurrentExportPrintInFiles, LlBoxType.None, LlBoxType.StandardAbort),
																			 CType(PrintData.frmhwnd, IntPtr), m_PrintJobData.LLDocLabel)

				LL.Core.LlPrintSetOption(LlPrintOption.Copies, Math.Max(1, m_NumberOfCopies.GetValueOrDefault(1)))
				LL.Core.LlPrintSetOption(LL_PRNOPT_COPIES_SUPPORTED, LL.Core.LlPrintGetOption(LlPrintOption.Copies))

				LL.Core.LlPrintOptionsDialog(CType(PrintData.frmhwnd, IntPtr), m_PrintJobData.PrintBoxHeader)


				Dim TargetFormat As String = LL.Core.LlPrintGetOptionString(LlPrintOptionString.Export)
				While Not LL.Core.LlPrint()
				End While

				If m_PrintJobData.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
					If PrintData.SearchKindEnum = HourSearchTypeEnum.Customer Then
						For Each itm In customerdata
							' pass data for current record
							result = result AndAlso DefineCustomerData(LL, itm)

							While LL.Core.LlPrintFields() = LlConstants.WrnRepeatData
								LL.Core.LlPrint()
							End While

						Next
					Else
						For Each itm In employeedata
							' pass data for current record
							result = result AndAlso DefineEmplyeeData(LL, itm)

							While LL.Core.LlPrintFields() = LlConstants.WrnRepeatData
								LL.Core.LlPrint()
							End While

						Next
					End If

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


			Catch LlException As LL_User_Aborted_Exception
				Return False

			Catch LlException As Exception
				m_Logger.LogError(String.Format("LlException: {0}", LlException.ToString))
				result = False

			Finally

			End Try


			Return result

		End Function

		Private Sub SetupDataObject()
			Dim customerdata = PrintData.CustomerHoursToPrintData

			Dim customerList As IEnumerable(Of CustomertReportHoursData) = customerdata
			m_Provider = New DataProviders.ObjectDataProvider(customerList)

			m_Provider.RootTableName = "Customer"

		End Sub

		Private Function SetLLVariable(ByVal LL As ListLabel) As Boolean
			Dim result As Boolean = True

			' Zusätzliche Variable einfügen
			LL.Variables.Add("AutorFName", m_InitializationData.UserData.UserFName)
			LL.Variables.Add("AutorLName", m_InitializationData.UserData.UserLName)
			Dim sortBez As String = String.Empty
			If PrintData.SearchKindEnum = HourSearchTypeEnum.Customer Then
				If PrintData.OrderByEnum = HoursPrintData.OrderByValue.OrderByCustomerName Then sortBez = "Kundenname"
			Else
				If PrintData.OrderByEnum = HoursPrintData.OrderByValue.OrderByCustomerName Then sortBez = "Kandidatenname"
			End If
			LL.Variables.Add("SortBez", String.Format(m_Translate.GetSafeTranslationValue("Sortiert nach {0}"), m_Translate.GetSafeTranslationValue(sortBez)))

			Dim filterConditions As String = String.Empty
			For Each itm In PrintData.FilterData
				filterConditions &= If(Not String.IsNullOrWhiteSpace(filterConditions), vbNewLine, String.Empty) & itm
			Next
			LL.Variables.Add("FilterBez", filterConditions) ' PrintData.FilterData.ToString)

			result = result AndAlso GetMDUSData4Print(LL)


			Return result

		End Function

		Private Function DefineCustomerData(ByVal LL As ListLabel, ByVal data As SP.DatabaseAccess.Listing.DataObjects.CustomertReportHoursData) As Boolean
			Dim result As Boolean = True

			Try
				LL.Core.LlDefineVariableExt("KDNr", ReplaceMissing(data.CustomerNumber, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("KSTNr", ReplaceMissing(data.CustomerCostcenter, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("CostcenterName", ReplaceMissing(data.CostcenterName, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Company1", ReplaceMissing(data.Company1, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Company2", ReplaceMissing(data.Company2, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Company3", ReplaceMissing(data.Company3, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("CountryCode", ReplaceMissing(data.CountryCode, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("CustomerPostcodeLocation", ReplaceMissing(data.CustomerPostcodeLocation, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("EMail", ReplaceMissing(data.EMail, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Location", ReplaceMissing(data.Location, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Postcode", ReplaceMissing(data.Postcode, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("PostOfficeBox", ReplaceMissing(data.PostOfficeBox, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Street", ReplaceMissing(data.Street, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Telefax", ReplaceMissing(data.Telefax, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Telephone", ReplaceMissing(data.Telephone, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("TotalHours", ReplaceMissing(data.TotalHours, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("Totalbetrag", ReplaceMissing(data.Amount, 0), LlFieldType.Numeric_Localized)


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				result = False

			End Try


			Return result

		End Function

		Private Function DefineEmplyeeData(ByVal LL As ListLabel, ByVal data As SP.DatabaseAccess.Listing.DataObjects.EmployeeReportHoursData) As Boolean
			Dim result As Boolean = True

			Try
				LL.Core.LlDefineVariableExt("MANr", ReplaceMissing(data.Employeenumber, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("CountryCode", ReplaceMissing(data.CountryCode, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Mobile", ReplaceMissing(data.Mobile, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("EmployeeLastnameFirstname", ReplaceMissing(data.EmployeeLastnameFirstname, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Firstname", ReplaceMissing(data.Firstname, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Lastname", ReplaceMissing(data.Lastname, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("EMail", ReplaceMissing(data.EMail, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Location", ReplaceMissing(data.Location, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Postcode", ReplaceMissing(data.Postcode, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("PostOfficeBox", ReplaceMissing(data.PostOfficeBox, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Street", ReplaceMissing(data.Street, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("StayAs", ReplaceMissing(data.StayAs, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("EmployeePostcodeLocation", ReplaceMissing(data.EmployeePostcodeLocation, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Telephone_P", ReplaceMissing(data.Telephone_P, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Telephone_2", ReplaceMissing(data.Telephone_2, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Telephone_3", ReplaceMissing(data.Telephone_3, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Telephone_G", ReplaceMissing(data.Telephone_G, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("TotalHours", ReplaceMissing(data.TotalHours, 0), LlFieldType.Numeric_Localized)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				result = False

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

			Return result

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


	Public Class CustomerHourViewData
		Inherits HoursPrintData

		Public Property CustomerHourRepportLineData As BindingList(Of EmployeeReportHoursData)

	End Class

End Namespace


