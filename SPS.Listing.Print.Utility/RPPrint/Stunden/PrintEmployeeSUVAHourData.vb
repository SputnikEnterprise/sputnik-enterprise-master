
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

Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects

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
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.TableSetting

Namespace ReportPrint

	Public Class PrintEmployeeSUVAHourData

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
		Private m_TablesettingDatabaseAccess As ITablesDatabaseAccess

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
		Private m_CustomerLanguage As String
		Private m_NumberOfCopies As Integer?

		Private m_pdfPrinterSettings As PrinterSettings


#End Region


#Region "Public properties"

		Public Property PrintData As SUVAHoursPrintData

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
			m_TablesettingDatabaseAccess = New TablesDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)

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


		Public Function PrintEmployeeSUVAHourData() As PrintResult
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


			Return Not (m_PrintJobData Is Nothing)

		End Function

		Private Function ShowAssignedListInDesign() As Boolean
			Dim result As Boolean = True
			Dim employeedata = PrintData.EmployeeHoursToPrintData

			Try
				If employeedata Is Nothing Then
					m_Logger.LogWarning("Keine Daten wurden gefunden.")

					Return False
				End If

				InitLL(LL)
				LL.Variables.Clear()
				LL.Fields.Clear()

				result = result AndAlso SetLLVariable(LL)
				result = result AndAlso DefineSUVATableData(LL, employeedata(0))

				If result Then
					SplashScreenManager.CloseForm(False)
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

		Function PrintAssignedList() As Boolean
			Const LL_PRNOPT_COPIES_SUPPORTED As Integer = 3
			Dim result As Boolean = True
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
				If employeedata Is Nothing Then
					m_Logger.LogWarning("Keine Daten wurden gefunden.")

					Return False
				End If

				InitLL(LL)
				LL.Variables.Clear()
				LL.Fields.Clear()

				result = result AndAlso SetLLVariable(LL)
				result = result AndAlso DefineSUVATableData(LL, employeedata(0))

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
					For Each itm In employeedata
						' pass data for current record
						result = result AndAlso DefineSUVATableData(LL, itm)

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

		Private Function SetLLVariable(ByVal LL As ListLabel) As Boolean
			Dim result As Boolean = True

			' Zusätzliche Variable einfügen
			LL.Variables.Add("AutorFName", m_InitializationData.UserData.UserFName)
			LL.Variables.Add("AutorLName", m_InitializationData.UserData.UserLName)

			Dim filterConditions As String = String.Empty
			For Each itm In PrintData.FilterData
				filterConditions &= If(Not String.IsNullOrWhiteSpace(filterConditions), vbNewLine, String.Empty) & itm
			Next
			LL.Variables.Add("FilterBez", filterConditions)
			LL.Variables.Add("AbsenceDataForCalculatingDayCount", PrintData.AbsenceDataForCalculatingDayCount)

			result = result AndAlso GetMDUSData4Print(LL)
			result = result AndAlso DefineEmployeeData(LL)
			result = result AndAlso DefineDynamicAbsenceListData(LL)

			Return result

		End Function

		Private Function DefineEmployeeData(ByVal LL As ListLabel) As Boolean
			Dim result As Boolean = True

			Dim data = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(PrintData.EmployeeHoursToPrintData(0).EmployeeNumber, False)
			If data Is Nothing Then
				m_Logger.LogError(m_Translate.GetSafeTranslationValue("Kandidaten Daten konten nicht geladen werden."))

				Return False
			End If

			LL.Variables.Add("EmployeeFirstname", data.Firstname)
			LL.Variables.Add("EmployeeLastname", data.Lastname)
			LL.Variables.Add("AHV_Nr_New", data.AHV_Nr_New)

			Return result

		End Function

		Private Function DefineDynamicAbsenceListData(ByVal LL As ListLabel) As Boolean
			Dim result As Boolean = True
			Dim i As Integer = 1

			Dim data = m_TablesettingDatabaseAccess.LoadAbsenceData()
			If data Is Nothing Then
				m_Logger.LogError(m_Translate.GetSafeTranslationValue("Kandidaten Daten konten nicht geladen werden."))

				Return False
			End If

			For i = 1 To 15
				LL.Variables.Add(String.Format("Absence_{0}", i), String.Empty)
			Next

			i = 1
			For Each itm In PrintData.DynamicAbsenceListData
				Dim searchResult = data.Where(Function(x) x.bez_value = itm).FirstOrDefault()
				If searchResult Is Nothing Then Continue For

				LL.Variables.Add(String.Format("Absence_{0}", i), String.Format("{0} = {1}", itm, searchResult.bez_d))

				i += 1
			Next


			Return result

		End Function

		Private Function DefineSUVATableData(ByVal LL As ListLabel, ByVal data As SP.DatabaseAccess.Listing.DataObjects.SuvaTableListData) As Boolean
			Dim result As Boolean = True

			Try
				LL.Core.LlDefineVariableExt("MANr", ReplaceMissing(data.EmployeeNumber, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("Monat", ReplaceMissing(data.CalendarMonth, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("Woche", ReplaceMissing(data.CalendarWeek, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("Jahr", ReplaceMissing(data.CalendarYear, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("KDNr", ReplaceMissing(data.CustomerNumber, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("ESNr", ReplaceMissing(data.EmploymentNumber, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("FR", ReplaceMissing(data.FridayDate, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("MO", ReplaceMissing(data.MondayDate, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("MODate", ReplaceMissing(data.MondayOfWeek, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("SODate", ReplaceMissing(data.SundayOfWeek, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("RPNr", ReplaceMissing(data.ReportNumber, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("SA", ReplaceMissing(data.SaturdayDate, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("SO", ReplaceMissing(data.SundayDate, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("Tag1", ReplaceMissing(data.Tag1, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Tag2", ReplaceMissing(data.Tag2, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Tag3", ReplaceMissing(data.Tag3, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Tag4", ReplaceMissing(data.Tag4, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Tag5", ReplaceMissing(data.Tag5, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Tag6", ReplaceMissing(data.Tag6, String.Empty), LlFieldType.Text)
				LL.Core.LlDefineVariableExt("Tag7", ReplaceMissing(data.Tag7, String.Empty), LlFieldType.Text)

				LL.Core.LlDefineVariableExt("DO", ReplaceMissing(data.ThursdayDate, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("DI", ReplaceMissing(data.TuesdayDate, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("MI", ReplaceMissing(data.WednesdayDate, String.Empty), LlFieldType.Date_Localized)
				LL.Core.LlDefineVariableExt("AnzTage", ReplaceMissing(data.WorkedDayCount, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineVariableExt("AnzStd", ReplaceMissing(data.WorkedHourCount, 0), LlFieldType.Numeric_Localized)


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

	End Class


	Public Class SUVAHoursPrintData

		Public Property frmhwnd As String
		Public Property EmployeeHoursToPrintData As BindingList(Of SuvaTableListData)
		Public Property ShowAsDesign As Boolean
		Public Property FilterData As List(Of String)
		Public Property PrintJobNumber As String
		Public Property AbsenceDataForCalculatingDayCount As String
		Public Property DynamicAbsenceListData As List(Of String)

	End Class


End Namespace


