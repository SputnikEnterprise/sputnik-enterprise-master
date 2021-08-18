
Imports System.IO
Imports System.IO.File
Imports combit.ListLabel25
Imports DevExpress.XtraSplashScreen
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Listing.DataObjects
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SPProgUtility.ClsProgSettingPath
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath
Imports SPS.Listing.Print.Utility.ClsMainSetting
Imports SPS.Listing.Print.Utility.MainUtilities.Utilities


Namespace LohnKontiSearchListing

	Public Class ClsPrintLohnKontiSearchList

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

		Private m_path As ClsProgPath

		Private m_ConnectionString As String
		Private m_StartPrintInLLDebug As Boolean
		Private m_UserLanguage As String
		Private m_NumberOfCopies As Integer?

		Private m_PrintJobData As PrintJobData
		Private m_CurrentExportPrintInFiles As Boolean
		Private m_PDFUtility As PDFUtilities.Utilities

		Private LL As ListLabel



#End Region



#Region "public properties"

		Public Property PrintData As ClsLLLohnKontiSearchPrintSetting

#End Region


#Region "Constructur"

		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			m_InitializationData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

			m_MandantData = New Mandant
			m_path = New SPProgUtility.ProgPath.ClsProgPath
			m_UtilityUI = New UtilityUI
			m_Utility = New SP.Infrastructure.Utility
			m_PDFUtility = New PDFUtilities.Utilities

			m_ConnectionString = m_InitializationData.MDData.MDDbConn
			m_ListingDatabaseAccess = New ListingDatabaseAccess(m_ConnectionString, m_InitializationData.UserData.UserLanguage)
			m_UserLanguage = m_InitializationData.UserData.UserLanguage

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


		Public Function PrintLohnKontiSearchList() As PrintResult
			Dim success As Boolean = True
			Dim result As PrintResult = New PrintResult With {.Printresult = True}

			Try

				Dim jobNumber As String = PrintData.PrintJobNumber
				success = success AndAlso LoadPrintJobData(jobNumber)
				If Not success Then Return New PrintResult With {.Printresult = False, .PrintresultMessage = "Vorlage wurde nicht gefunden!"}

				PrintData.ExportedFiles = New List(Of String)
				m_CurrentExportPrintInFiles = PrintData.ExportData.GetValueOrDefault(False)

				If PrintData.ShowAsDesign Then
					success = success AndAlso ShowAssignedListInDesign()

					Return result
				End If

				success = success AndAlso PrintAssignedARGBList()
				If Not success Then result.Printresult = False

				Dim newMergedFilename As String = String.Empty
				newMergedFilename = Path.Combine(m_InitializationData.UserData.spTempPayrollPath, String.Format("Lohnkonti_{0}_{1}", PrintData.EmployeeNumber, PrintData.SelectedYear))
				newMergedFilename = Path.ChangeExtension(newMergedFilename, ".pdf")
				If File.Exists(newMergedFilename) Then
					Try
						File.Delete(newMergedFilename)
					Catch ex As Exception
						newMergedFilename = Path.Combine(m_InitializationData.UserData.spTempPayrollPath, String.Format("Lohnkonti_{0}_{1}_{2}", PrintData.EmployeeNumber, PrintData.SelectedYear, Environment.TickCount))
						newMergedFilename = Path.ChangeExtension(newMergedFilename, ".pdf")

					End Try
				End If
				If success AndAlso PrintData.ExportedFiles.Count > 1 Then

					Dim mergePDF As Boolean = True
					mergePDF = mergePDF AndAlso m_PDFUtility.MergePdfFiles(PrintData.ExportedFiles.ToArray, newMergedFilename)

					If mergePDF Then
						PrintData.ExportedFileName = newMergedFilename
						Try
							For Each itm In PrintData.ExportedFiles
								System.IO.File.Open(itm, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
								FileClose(1)

								File.Delete(itm)
							Next
						Catch ex As Exception

						End Try

						'PrintData.ExportedFiles.Clear()
					End If

				ElseIf PrintData.ExportedFiles.Count = 1 Then

					File.Copy(PrintData.ExportedFiles(0), newMergedFilename)

					PrintData.ExportedFileName = newMergedFilename ' PrintData.ExportedFiles(0)
					'PrintData.ExportedFiles.Clear()

				End If
				If Not String.IsNullOrWhiteSpace(PrintData.ExportedFileName) Then
					PrintData.ExportedFiles.Add(PrintData.ExportedFileName)
					result.ExportFilename = PrintData.ExportedFileName
				End If


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			Finally
				SplashScreenManager.CloseForm(False)
			End Try

			Return result
		End Function




		Private Function LoadPrintJobData(ByVal jobNumberToPrint As String) As Boolean

			m_PrintJobData = m_ListingDatabaseAccess.LoadAssignedPrintJobData(m_InitializationData.MDData.MDNr, m_UserLanguage, jobNumberToPrint)


			Return Not (m_PrintJobData Is Nothing)

		End Function

		Private Function ShowAssignedListInDesign() As Boolean
			Dim result As Boolean = True

			Try
				If PrintData.LohnKontiData Is Nothing Then
					m_Logger.LogWarning("Keine Daten wurden gefunden.")

					Return False
				End If

				InitLL(LL)
				LL.Variables.Clear()
				LL.Fields.Clear()

				result = result AndAlso SetLLVariable(LL)
				result = result AndAlso DefineLohnkontiData(LL, PrintData.LohnKontiData(0))

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

		Function PrintAssignedARGBList() As Boolean
			Const LL_PRNOPT_COPIES_SUPPORTED As Integer = 3
			Dim result As Boolean = True
			Dim kontiData = PrintData.LohnKontiData
			Dim tempFileName As String = String.Empty

			Try
				If m_StartPrintInLLDebug Then
					LL.Debug = LlDebug.LogToFile
					LL.Debug = LlDebug.Enabled
				End If
			Catch ex As Exception
				m_Logger.LogError(String.Format("EnablingLLDebuging: {0}", ex.ToString))

			End Try

			Try
				If kontiData Is Nothing OrElse kontiData.Count = 0 Then
					m_Logger.LogWarning("Keine Daten wurden gefunden.")

					Return False
				End If

				InitLL(LL)
				LL.Variables.Clear()
				LL.Fields.Clear()

				result = result AndAlso SetLLVariable(LL)
				result = result AndAlso DefineLohnkontiData(LL, PrintData.LohnKontiData(0))

				SplashScreenManager.CloseForm(False)
				LL.Core.LlPrintWithBoxStart(If(m_PrintJobData.LLDocName.ToUpper.EndsWith(".LST".ToUpper),
																				 LlProject.List, LlProject.Card),
																				 m_PrintJobData.LLDocName, LlPrintMode.Export,
																				 LlBoxType.StandardAbort,
																				 CType(PrintData.frmhwnd, IntPtr), m_PrintJobData.LLDocLabel)

				If m_CurrentExportPrintInFiles Then
					If Not Directory.Exists(m_InitializationData.UserData.spTempPayrollPath) Then Directory.CreateDirectory(m_InitializationData.UserData.spTempPayrollPath)

					If PrintData.EmployeeNumber.GetValueOrDefault(0) <> 0 Then

						tempFileName = String.Format("Lohnkonti_{0}.PDF", kontiData(0).MANr)
						If File.Exists(Path.Combine(m_InitializationData.UserData.spTempPayrollPath, tempFileName)) Then
							Try
								tempFileName = String.Format("Lohnkonti_{0}_{1}.PDF", kontiData(0).MANr, Environment.TickCount)
							Catch ex As Exception

							End Try

						End If
					Else
						tempFileName = String.Format("Lohnkonti_{0}.PDF", Environment.TickCount)
					End If

					LL.Variables.Add("WOSDoc", 1)
					LL.Core.LlPrintSetOptionString(LlPrintOptionString.Export, "PDF")
					LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.File", tempFileName)
					LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.Path", m_InitializationData.UserData.spTempPayrollPath)
					LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.Quiet", "1")

						LL.Core.LlPrintSetOption(LlPrintOption.Dialog_DestinationMask, 2)

					Else
						LL.Core.LlPrintSetOption(LlPrintOption.Copies, Math.Max(1, m_NumberOfCopies.GetValueOrDefault(1))) ' m_PrintJobData.LLCopyCount)
					LL.Core.LlPrintSetOption(LL_PRNOPT_COPIES_SUPPORTED, LL.Core.LlPrintGetOption(LlPrintOption.Copies))

					LL.Core.LlPrintOptionsDialog(CType(PrintData.frmhwnd, IntPtr), m_PrintJobData.PrintBoxHeader)

				End If

				Dim TargetFormat As String = LL.Core.LlPrintGetOptionString(LlPrintOptionString.Export)

				While Not LL.Core.LlPrint()
				End While

				If m_PrintJobData.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then

					For Each itm In PrintData.LohnKontiData
						result = result AndAlso DefineLohnkontiData(LL, itm)

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
				If m_CurrentExportPrintInFiles AndAlso Not String.IsNullOrWhiteSpace(tempFileName) Then PrintData.ExportedFiles.Add(Path.Combine(m_InitializationData.UserData.spTempPayrollPath, tempFileName))


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

				LL.Variables.Add("FilterBez", PrintData.ListFilterBez(0))
				LL.Variables.Add("FilterBez2", If(PrintData.ListFilterBez.Count > 1, PrintData.ListFilterBez(1), String.Empty))
				LL.Variables.Add("FilterBez3", If(PrintData.ListFilterBez.Count > 2, PrintData.ListFilterBez(2), String.Empty))
				LL.Variables.Add("FilterBez4", If(PrintData.ListFilterBez.Count > 3, PrintData.ListFilterBez(3), String.Empty))

				LL.Variables.Add("ListBez2Print", PrintData.ListBez2Print)

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


		Private Function DefineLohnkontiData(ByVal LL As ListLabel, ByVal data As ListingPayrollLohnkontiData) As Boolean
			Dim result As Boolean = True

			LL.Core.LlDefineVariableExt("Jahr", ReplaceMissing(PrintData.SelectedYear, 0), LlFieldType.Numeric_Localized)
			LL.Core.LlDefineVariableExt("Monat", ReplaceMissing(PrintData.SelectedMonth, 0), LlFieldType.Numeric_Localized)
			LL.Core.LlDefineVariableExt("MANr", ReplaceMissing(data.MANr, 0), LlFieldType.Numeric_Localized)
			LL.Core.LlDefineVariableExt("Nachname", ReplaceMissing(data.EmployeeLastname, String.Empty), LlFieldType.Text)
			LL.Core.LlDefineVariableExt("Vorname", ReplaceMissing(data.EmployeeFirstname, String.Empty), LlFieldType.Text)
			LL.Core.LlDefineVariableExt("GebDat", ReplaceMissing(data.GebDat, String.Empty), LlFieldType.Date_Localized)
			LL.Core.LlDefineVariableExt("AHV_Nr_New", ReplaceMissing(data.AHVNumber, String.Empty), LlFieldType.Text)
			LL.Core.LlDefineVariableExt("AHV_Nr", ReplaceMissing(data.AHVNumber, String.Empty), LlFieldType.Text)

			LL.Core.LlDefineVariableExt("ESBegin", ReplaceMissing(data.SatrtOfEmployment, String.Empty), LlFieldType.Date_Localized)
			LL.Core.LlDefineVariableExt("ESEnde", ReplaceMissing(data.EndOfEmployment, String.Empty), LlFieldType.Date_Localized)
			LL.Core.LlDefineVariableExt("Geschlecht", ReplaceMissing(data.Gender, String.Empty), LlFieldType.Text)
			LL.Core.LlDefineVariableExt("Nationality", ReplaceMissing(data.Nationality, String.Empty), LlFieldType.Text)


			Try
				LL.Core.LlDefineFieldExt("LANr", ReplaceMissing(data.LANr, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineFieldExt("Bezeichnung", ReplaceMissing(data.LALabel, String.Empty), LlFieldType.Text)

				LL.Core.LlDefineFieldExt("Januar", ReplaceMissing(data.Januar, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineFieldExt("Februar", ReplaceMissing(data.Februar, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineFieldExt("März", ReplaceMissing(data.Maerz, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineFieldExt("April", ReplaceMissing(data.April, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineFieldExt("Mai", ReplaceMissing(data.Mai, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineFieldExt("Juni", ReplaceMissing(data.Juni, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineFieldExt("Juli", ReplaceMissing(data.Juli, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineFieldExt("August", ReplaceMissing(data.August, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineFieldExt("September", ReplaceMissing(data.September, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineFieldExt("Oktober", ReplaceMissing(data.Oktober, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineFieldExt("November", ReplaceMissing(data.November, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineFieldExt("Dezember", ReplaceMissing(data.Dezember, 0), LlFieldType.Numeric_Localized)
				LL.Core.LlDefineFieldExt("KumulativAmount", ReplaceMissing(data.Kumulativ, 0), LlFieldType.Numeric_Localized)


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				result = False

			End Try


			Return result

		End Function





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

End Namespace


