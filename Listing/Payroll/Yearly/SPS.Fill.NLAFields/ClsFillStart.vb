
Imports System.IO
Imports System.Windows.Forms
Imports DevExpress.XtraSplashScreen
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging

Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Listing.DataObjects

Imports iTextSharp.text.pdf
Imports System.Security
Imports System.ComponentModel
Imports DevExpress.Pdf
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.Internal.Automations.WOSUtility.DataObjects


Namespace FillNLA

	Public Class ClsFillStart


#Region "public properties"

		Public _ClsSetting As ClsNLASetting
		Public Property GetSelectedMANr() As Integer
		Public Property DbConn2Open As New SqlClient.SqlConnection()

#End Region


#Region "privat fields"

		''' <summary>
		''' The logger.
		''' </summary>
		Private m_Logger As ILogger = New Logger()

		Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

		Private LoadingPanel As New WaitForm1
		Private FinalFilename2Print As String

		Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

		Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

		Private m_mandant As Mandant
		''' <summary>
		''' The cls prog path.
		''' </summary>
		Private m_path As ClsProgPath

		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As SP.Infrastructure.Utility

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI
		Private m_MandantDocumentPath As String

		Private m_connectionString As String
		''' <summary>
		''' The common data access object.
		''' </summary>
		Private m_CommonDatabaseAccess As ICommonDatabaseAccess
		Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess
		Private m_ListingDatabaseAccess As IListingDatabaseAccess

		Private m_ShowAdvisorData As Boolean
		Private m_EnableEditingTemplateData As Boolean
		Private m_newVersion2021 As Boolean
		Private m_DoneNLAFiles As List(Of String)
		Private m_SentNLAToWOS As List(Of String)

		Private m_CurrentDoneNLAFile As String
		Private m_NLATemplate As String
		Private m_NLAData As BindingList(Of ListingPayrollNLAData)
		Private m_CurrentNLAData As ListingPayrollNLAData
		Private m_FinalDoneFile As String

#End Region


#Region "Private Consts"

		Private Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Lohnausweis_NLA"

#End Region


#Region "Constructor"

		Sub New(ByVal _setting As ClsNLASetting, ByVal progSetting As SP.Infrastructure.Initialization.InitializeClass)
			Me._ClsSetting = _setting

			m_InitializationData = progSetting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

			m_mandant = New Mandant
			m_path = New ClsProgPath

			m_Utility = New Utility
			m_UtilityUI = New UtilityUI

			m_connectionString = m_InitializationData.MDData.MDDbConn
			m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_ListingDatabaseAccess = New ListingDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

			m_ShowAdvisorData = ShowAdvisorData
			m_EnableEditingTemplateData = EnableEditingTemplateData

			NotifyUser = True
			JustExportDataIntoFile = False
			m_SentNLAToWOS = New List(Of String)

		End Sub


		Public Sub New(ByVal _iMANr As Integer, ByVal _setting As ClsNLASetting, ByVal progSetting As SP.Infrastructure.Initialization.InitializeClass)
			Dim bDownladnewFile As Boolean = False

			m_InitializationData = progSetting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

			m_mandant = New Mandant
			m_path = New ClsProgPath

			m_Utility = New Utility
			m_UtilityUI = New UtilityUI

			Me.GetSelectedMANr = _iMANr
			Me._ClsSetting = _setting

			m_connectionString = m_InitializationData.MDData.MDDbConn
			m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_ListingDatabaseAccess = New ListingDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

			Dim sValue As Short = _ClsSetting.GetJobID(m_InitializationData.MDData.MDNr)
			m_newVersion2021 = _ClsSetting.Get2021JobID(m_InitializationData.MDData.MDNr)

			m_MandantDocumentPath = m_mandant.GetSelectedMDDocPath(m_InitializationData.MDData.MDNr)
			Dim strAdresse As String = "http://downloads.domain.com/sps_downloads/prog/forms/"
			Dim strFilename As String
			strFilename = String.Format("nla_r{0}.pdf", If(m_newVersion2021, "_2021", ""))


			If sValue = 1 Then strFilename = String.Format("nla_l{0}.pdf", If(m_newVersion2021, "_2021", ""))

			If Not File.Exists(Path.Combine(m_MandantDocumentPath, strFilename)) Then
				bDownladnewFile = True
			End If
			If bDownladnewFile Then
				Try
					My.Computer.Network.DownloadFile(String.Format("{0}{1}", strAdresse, strFilename), String.Format("{0}{1}", m_MandantDocumentPath, strFilename))

				Catch ex As Exception
					Dim strMsg As String = "Die Datei konnte nicht heruntergeladen werden."
					strMsg = m_Translate.GetSafeTranslationValue(strMsg)
					DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, m_Translate.GetSafeTranslationValue("Datei herunterladen"),
																										 MessageBoxButtons.OK, MessageBoxIcon.Error,
																										 MessageBoxDefaultButton.Button1)

				End Try
			End If
			m_NLATemplate = Path.Combine(m_MandantDocumentPath, strFilename)

			Dim xmlMandantFile As String = m_mandant.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr)
			_ClsSetting.NLA_2_3 = m_Utility.ParseToString(m_path.GetXMLNodeValue(xmlMandantFile, String.Format("{0}/nla_2_3", FORM_XML_MAIN_KEY)), String.Empty)
			_ClsSetting.NLA_3_0 = m_Utility.ParseToString(m_path.GetXMLNodeValue(xmlMandantFile, String.Format("{0}/nla_3_0", FORM_XML_MAIN_KEY)), String.Empty)
			_ClsSetting.NLA_4_0 = m_Utility.ParseToString(m_path.GetXMLNodeValue(xmlMandantFile, String.Format("{0}/nla_4_0", FORM_XML_MAIN_KEY)), String.Empty)
			_ClsSetting.NLA_7_0 = m_Utility.ParseToString(m_path.GetXMLNodeValue(xmlMandantFile, String.Format("{0}/nla_7_0", FORM_XML_MAIN_KEY)), String.Empty)
			_ClsSetting.NLA_13_1_2 = m_Utility.ParseToString(m_path.GetXMLNodeValue(xmlMandantFile, String.Format("{0}/nla_13_1_2", FORM_XML_MAIN_KEY)), String.Empty)
			_ClsSetting.NLA_13_2_3 = m_Utility.ParseToString(m_path.GetXMLNodeValue(xmlMandantFile, String.Format("{0}/nla_13_2_3", FORM_XML_MAIN_KEY)), String.Empty)

			_ClsSetting.NLA_14_1_1 = m_Utility.ParseToString(m_path.GetXMLNodeValue(xmlMandantFile, String.Format("{0}/nla_nebenleistung_1", FORM_XML_MAIN_KEY)), String.Empty)
			_ClsSetting.NLA_14_1_2 = m_Utility.ParseToString(m_path.GetXMLNodeValue(xmlMandantFile, String.Format("{0}/nla_nebenleistung_2", FORM_XML_MAIN_KEY)), String.Empty)

			_ClsSetting.NLA_15_1_1 = m_Utility.ParseToString(m_path.GetXMLNodeValue(xmlMandantFile, String.Format("{0}/nla_bemerkung_1", FORM_XML_MAIN_KEY)), String.Empty)
			_ClsSetting.NLA_15_1_2 = m_Utility.ParseToString(m_path.GetXMLNodeValue(xmlMandantFile, String.Format("{0}/nla_bemerkung_2", FORM_XML_MAIN_KEY)), String.Empty)

			m_ShowAdvisorData = ShowAdvisorData
			m_EnableEditingTemplateData = EnableEditingTemplateData

			'Me.DbConn2Open = _setting.DbConn2Open
			m_SentNLAToWOS = New List(Of String)

		End Sub


#End Region

#Region "readonly Properties"

		Public ReadOnly Property GetNLAFiles() As List(Of String)
			Get
				Return m_DoneNLAFiles
			End Get
		End Property

		Public ReadOnly Property GetDoneNLAFiles() As String
			Get
				Return m_FinalDoneFile
			End Get
		End Property
		Public ReadOnly Property GetSentNLAWOSFiles() As List(Of String)
			Get
				Return m_SentNLAToWOS
			End Get
		End Property

		ReadOnly Property GetPDFVW_O2SSerial() As String
			Get
				Return _ClsProgSetting.GetPDFVW_O2SSerial  ' "yourlicencekey"
			End Get
		End Property

		ReadOnly Property GetPDF_O2SSerial() As String
			Get
				Return _ClsProgSetting.GetPDF_O2SSerial ' "yourlicencekey"
			End Get
		End Property

		Private ReadOnly Property ShowAdvisorData() As Boolean
			Get
				Dim xmlFile = m_mandant.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr)
				Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
				Dim sValue As String = String.Empty
				Dim strMainKey As String = "//Lohnausweis_NLA"
				Dim strKeyName As String = "DisableUserName".ToLower
				Dim strQuery As String = String.Format("{0}/{1}", strMainKey, strKeyName)

				strQuery = String.Format("{0}/{1}", strMainKey, strKeyName)
				sValue = _ClsProgSetting.GetXMLValueByQuery(xmlFile, strQuery, String.Empty)

				Return Val(sValue) = 1
			End Get
		End Property

		Private ReadOnly Property EnableEditingTemplateData() As Boolean
			Get
				Dim xmlFile = m_mandant.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr)
				Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
				Dim sValue As String = String.Empty
				Dim strMainKey As String = "//Lohnausweis_NLA"
				Dim strKeyName As String = "enableediting".ToLower
				Dim strQuery As String = String.Format("{0}/{1}", strMainKey, strKeyName)

				strQuery = String.Format("{0}/{1}", strMainKey, strKeyName)
				sValue = _ClsProgSetting.GetXMLValueByQuery(xmlFile, strQuery, String.Empty)

				Return Val(sValue) = 1
			End Get
		End Property


		Public Property NotifyUser As Boolean
		Public Property JustExportDataIntoFile As Boolean


#End Region

		Function OpenNLASetting() As Boolean
			Dim frm = New frmNLASetting(m_InitializationData)
			If frm.ShowDialog() = DialogResult.OK Then
				Return True
			Else
				Return False
			End If

		End Function

		Public Function StartFillingNLA(ByVal SQL2Open As String) As Boolean
			Dim success As Boolean = True
			Dim isWOSTransfered As Boolean
			m_SentNLAToWOS.clear

			If Not File.Exists(Me.m_NLATemplate) Then
				m_UtilityUI.ShowErrorDialog(String.Format("Die Vorlage Datei existiert nicht! {0}", m_NLATemplate))

				Return False
			End If
			m_DoneNLAFiles = New List(Of String)

			Try

				Dim strFinalFilename As String = String.Format("{0}FinalNLA.pdf", _ClsProgSetting.GetSpSNLATempPath)

				If File.Exists(strFinalFilename) Then
					Try
						File.Delete(strFinalFilename)
					Catch ex As Exception
						strFinalFilename = Path.GetTempFileName
						Dim newTempFile As String = Path.Combine(_ClsProgSetting.GetSpSNLATempPath, Path.GetFileName(strFinalFilename))
						File.Move(strFinalFilename, newTempFile)
						strFinalFilename = Path.ChangeExtension(newTempFile, "pdf")

					End Try
				End If

				success = success AndAlso LoadPayrollNLAData()
				If Not success Then
					SplashScreenManager.CloseForm(False)

					Dim msg As String = m_Translate.GetSafeTranslationValue("Keine Daten für Lohnausweis gefunden.")
					m_Logger.LogError(msg)

					'm_UtilityUI.ShowErrorDialog(msg)

					Return False
				End If
				Try
					Dim i As Integer = 0
					Dim nlaCount As Integer = m_NLAData.Count

					For Each itm In m_NLAData
						i += 1

						m_CurrentNLAData = itm
						m_CurrentDoneNLAFile = String.Empty

						Dim iCurrMANr As Integer = itm.MANr
						Dim strMsg As String = m_Translate.GetSafeTranslationValue("Lohnausweis für {0} {1} {2} wird erstellt...")
						strMsg = String.Format(strMsg, m_Translate.GetSafeTranslationValue(If(itm.employeegeschlecht = "M", "Herr", "Frau")), itm.employeefirstname, itm.employeelastname)
						Dim frm As New WaitForm1
						SplashScreenManager.ShowForm(frm, GetType(WaitForm1), False, False, False)
						SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Bitte warten Sie einen Augenblick") & String.Format(" ({0} von {1})", i, nlaCount) & "...")
						SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue((strMsg)))

						Dim time_1 As Double = System.Environment.TickCount

						If m_newVersion2021 Then
							success = success AndAlso Create2021NLAPDFDoc()
						Else
							success = success AndAlso CreatedPDFDocWithDevExpressComponents()
						End If
						If JustExportDataIntoFile Then
							m_DoneNLAFiles.Add(m_CurrentDoneNLAFile)

							Continue For
						End If

						If success Then
							SaveNLAFileToMAHistoryDocDb(m_CurrentDoneNLAFile)
							If itm.employeesend2wos AndAlso Not _ClsSetting.CreateJobsForExport.GetValueOrDefault(False) AndAlso
								(_ClsSetting.Send2WOS = WOSSENDValue.PrintAndSend OrElse _ClsSetting.Send2WOS = WOSSENDValue.PrintOtherSendWOS) Then
								isWOSTransfered = True
								TransferEmployeeNLADataIntoWOS()
								m_SentNLAToWOS.Add(m_CurrentDoneNLAFile)

								If _ClsSetting.Send2WOS = WOSSENDValue.PrintAndSend Then
									m_DoneNLAFiles.Add(m_CurrentDoneNLAFile)
								End If

							Else
								m_DoneNLAFiles.Add(m_CurrentDoneNLAFile)

							End If
						Else

						End If
						m_Logger.LogInfo(String.Format("nla was created for employee {0} in {1}", iCurrMANr, m_CurrentDoneNLAFile))

					Next

				Catch ex As Exception
					m_Logger.LogError(String.Format("Fill PDF-Files: {0}", ex.ToString))

					Return False
				End Try
				SplashScreenManager.CloseForm(False)

				If JustExportDataIntoFile Then Return success
				If Not success Then Return False

				Try
					If m_DoneNLAFiles.Count > 1 Then
						Dim strMsg As String = m_Translate.GetSafeTranslationValue("Die Dokumente werden zusammengefügt...")
						LoadingPanel.SetCaption(m_Translate.GetSafeTranslationValue("Bitte warten Sie einen Augenblick..."))
						LoadingPanel.SetDescription(strMsg)
						LoadingPanel.Show()

						Using pdfDocumentProcessor As New PdfDocumentProcessor()

							pdfDocumentProcessor.CreateEmptyDocument(strFinalFilename)
							For Each itm In m_DoneNLAFiles
								If File.Exists(itm) Then
									pdfDocumentProcessor.AppendDocument(itm.ToString)
								Else
									m_Logger.LogInfo(String.Format("nla could not be founded. {0}", itm))
								End If
							Next
						End Using

					ElseIf m_DoneNLAFiles.Count <> 0 Then
						File.Copy(m_CurrentDoneNLAFile, strFinalFilename, True)

					End If
					If _ClsSetting.CreateJobsForExport.GetValueOrDefault(False) Then
						Dim strMsg As String = "Ihre Daten wurden erfolgreich in {0}{1}{0}geschrieben."
						strMsg = String.Format(m_Translate.GetSafeTranslationValue(strMsg), vbNewLine, strFinalFilename)
						LoadingPanel.Close()

						DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, m_Translate.GetSafeTranslationValue("Daten exportieren"),
																											 MessageBoxButtons.OK, MessageBoxIcon.Information,
																											 MessageBoxDefaultButton.Button1)
						Process.Start(strFinalFilename)
						Return True
					End If

				Catch ex As Exception
					m_Logger.LogError(String.Format("Merge PDF-Files: {0}", ex.ToString))

					Return False
				End Try

				LoadingPanel.Close()
				If Not _ClsSetting.CreateJobsForExport.GetValueOrDefault(False) AndAlso m_DoneNLAFiles.Count > 0 Then
					m_FinalDoneFile = strFinalFilename
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("Open&Fill PDF-Files: {0}", ex.ToString))

				success = False

			Finally
				LoadingPanel.Close()

			End Try


			Return success
		End Function

		Public Function LoadPDFFields(ByVal pdfFilename As String) As List(Of String)

			If String.IsNullOrWhiteSpace(pdfFilename) Then pdfFilename = m_NLATemplate
			Dim result = ListAllPDFFields(pdfFilename)

			Return result
		End Function

		''' <summary>
		''' Load nla data.
		''' </summary>
		Private Function LoadPayrollNLAData() As Boolean
			Dim listDataSource = New BindingList(Of ListingPayrollNLAData)

			Dim data = m_ListingDatabaseAccess.LoadAnnualNLAData(_ClsSetting.SQL2Open)

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				If NotifyUser Then m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Lohnausweis Daten konnten nicht geladen werden."))
				data = Nothing

				Return False

			End If

			For Each result In data

				Dim viewData = New ListingPayrollNLAData With {
				.MANr = result.MANr,
				.employeefirstname = result.employeefirstname,
				.employeelastname = result.employeelastname,
				.employeemastrasse = result.employeemastrasse,
				.employeemaplz = result.employeemaplz,
				.employeemaort = result.employeemaort,
				.employeemaland = result.employeemaland,
				.employeemaco = result.employeemaco,
				.employeeahv_nr = result.employeeahv_nr,
				.employeeahv_nr_new = result.employeeahv_nr_new,
				.employeebirthdate = result.employeebirthdate,
				.employeesend2wos = result.employeesend2wos,
				.employeegeschlecht = result.employeegeschlecht,
				.employeemapostfach = result.employeemapostfach,
				.employeelajahr = result.employeelajahr,
				.Z_1_0 = result.Z_1_0,
				.Z_2_1 = result.Z_2_1,
				.Z_2_2 = result.Z_2_2,
				.Z_2_3 = result.Z_2_3,
				.Z_3_0 = result.Z_3_0,
				.Z_4_0 = result.Z_4_0,
				.Z_5_0 = result.Z_5_0,
				.Z_6_0 = result.Z_6_0,
				.Z_7_0 = result.Z_7_0,
				.Z_8_0 = result.Z_8_0,
				.Z_9_0 = result.Z_9_0,
				.Z_10_1 = result.Z_10_1,
				.Z_10_2 = result.Z_10_2,
				.Z_11_0 = result.Z_11_0,
				.Z_12_0 = result.Z_12_0,
				.Z_13_1_1 = result.Z_13_1_1,
				.Z_13_1_2 = result.Z_13_1_2,
				.Z_13_2_1 = result.Z_13_2_1,
				.Z_13_2_2 = result.Z_13_2_2,
				.Z_13_2_3 = result.Z_13_2_3,
				.Z_13_3_0 = result.Z_13_3_0,
				.NLA_LoAusweis = result.NLA_LoAusweis,
				.NLA_Befoerderung = result.NLA_Befoerderung,
				.NLA_Kantine = result.NLA_Kantine,
				.NLA_2_3 = result.NLA_2_3,
				.NLA_3_0 = result.NLA_3_0,
				.NLA_4_0 = result.NLA_4_0,
				.NLA_7_0 = result.NLA_7_0,
				.NLA_Spesen_NotShow = result.NLA_LoAusweis,
				.NLA_13_1_2 = result.NLA_13_1_2,
				.NLA_13_2_3 = result.NLA_13_2_3,
				.NLA_Nebenleistung_1 = result.NLA_Nebenleistung_1,
				.NLA_Nebenleistung_2 = result.NLA_Nebenleistung_2,
				.NLA_Bemerkung_1 = result.NLA_Bemerkung_1,
				.NLA_Bemerkung_2 = result.NLA_Bemerkung_2,
				.Grund = result.Grund,
				.ES_Ab1 = result.ES_Ab1,
				.ES_Bis1 = result.ES_Bis1
				}

				listDataSource.Add(viewData)

			Next

			m_NLAData = listDataSource


			Return Not (listDataSource Is Nothing)
		End Function

#Region "WOS-Methods"

		Function TransferEmployeeNLADataIntoWOS() As WOSSendResult
			Dim result As WOSSendResult = New WOSSendResult With {.Value = False, .Message = "Initialisierung..."}
			Dim success As Boolean = True
			Try

				If m_CurrentNLAData.employeesend2wos AndAlso Not _ClsSetting.Send2WOS = WOSZVSENDValue.PrintWithoutSending Then
					Dim wos = New SP.Internal.Automations.WOSUtility.EmployeeExport(m_InitializationData)
					Dim employeeData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(m_CurrentNLAData.MANr, False)
					If employeeData Is Nothing Then
						m_Logger.LogError(m_Translate.GetSafeTranslationValue("Stammdaten konnten nicht geladen werden."))
						Return New WOSSendResult With {.Value = False, .Message = m_Translate.GetSafeTranslationValue("Stammdaten konnten nicht geladen werden.")}
					End If
					Dim wosSetting = New WOSSendSetting

					wosSetting.EmployeeNumber = employeeData.EmployeeNumber
					wosSetting.EmployeeGuid = employeeData.Transfered_Guid
					wosSetting.DocumentArtEnum = WOSSendSetting.DocumentArt.Lohnausweis

					Dim pdfFilename As String = m_CurrentDoneNLAFile

					wosSetting.DocumentInfo = String.Format("Lohnausweis: {0}", m_CurrentNLAData.employeelajahr)
					Dim fileByte() = m_Utility.LoadFileBytes(pdfFilename)
					wosSetting.ScanDoc = fileByte
					Dim scanfileinfo = New FileInfo(pdfFilename)
					wosSetting.ScanDocName = scanfileinfo.Name

					wos.WOSSetting = wosSetting

					If success Then result = wos.TransferEmployeeDocumentDataToWOS(True)
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))

			End Try


			Return result

		End Function


#End Region

		Private Function SaveNLAFileToMAHistoryDocDb(ByVal strFullFilename As String) As Boolean
			Dim success As Boolean = True
			Dim strDocInfo As String = String.Format("Lohnausweis: {0}", m_CurrentNLAData.employeelajahr)
			Dim strDocArt As String = "Lohnausweis"

			Try

				Dim data = New EmployeePrintedDocProperty
				data.createdfrom = m_InitializationData.UserData.UserFullName
				data.createdon = Now
				data.docname = String.Format("{0} {1}", strDocArt, strDocInfo)
				data.manr = m_CurrentNLAData.MANr
				data.scandoc = m_Utility.LoadFileBytes(strFullFilename)
				data.username = m_InitializationData.UserData.UserFullName

				success = success AndAlso m_ListingDatabaseAccess.AddAssignedPrintedDocumentInForEmployee(data)


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))

				Return False
			End Try


			Return success
		End Function

		'Private Function CreatedPDFDocWithMA(ByVal nlaRecord As ListingPayrollNLAData) As Boolean
		'	Dim result As Boolean = True

		'	Dim pdfReader As PdfReader = New PdfReader("C:\Path\PDF\nla_L.pdf")
		'	If pdfReader.IsEncrypted Then
		'		PdfReader.unethicalreading = True
		'	End If
		'	Dim TempFilename As String = Path.Combine(_ClsProgSetting.GetSpSNLATempPath, String.Format("{0}.pdf", nlaRecord.MANr))
		'	TempFilename = Path.ChangeExtension(TempFilename, "PDF")
		'	TempFilename = "C:\Path\PDF\Final_fw5.pdf"

		'	Dim pdfStamper = New PdfStamper(pdfReader, New System.IO.FileStream(TempFilename, System.IO.FileMode.Create), "\6c", True)




		'	Dim pdfFormFields As AcroFields = pdfStamper.AcroFields

		'	' ------ SET YOUR FORM FIELDS ------

		'	pdfFormFields.SetField("A", If(nlaRecord.NLA_LoAusweis, True, False))
		'	pdfFormFields.SetField("B", If(nlaRecord.NLA_LoAusweis, "nein", "ja"))
		'	pdfFormFields.SetField("C", nlaRecord.employeeahv_nr_new)
		'	pdfFormFields.SetField("C2", String.Format("{0:d}", nlaRecord.employeebirthdate))

		'	pdfFormFields.SetField("13-1-1-1", If(CBool(nlaRecord.Z_13_1_1), "ja", "nein"))
		'	pdfFormFields.SetField("F", If(CBool(nlaRecord.NLA_Befoerderung) OrElse nlaRecord.Z_2_2.GetValueOrDefault(0) <> 0, "ja", "nein"))
		'	pdfFormFields.SetField("G", If(nlaRecord.NLA_Kantine OrElse nlaRecord.Z_2_1.GetValueOrDefault(0) <> 0, "ja", "nein"))

		'	pdfFormFields.SetField("D", nlaRecord.employeelajahr)
		'	pdfFormFields.SetField("E-von", nlaRecord.ES_Ab1)
		'	pdfFormFields.SetField("E-bis", nlaRecord.ES_Bis1)
		'	Dim hAnrede As String = m_Translate.GetSafeTranslationValue(If(nlaRecord.employeegeschlecht = "M", "Herr", "Frau")) & If(String.IsNullOrWhiteSpace(nlaRecord.employeemaco), "", String.Format(" {0} {1}", nlaRecord.employeefirstname, nlaRecord.employeelastname))
		'	pdfFormFields.SetField("HAnrede", hAnrede)
		'	Dim hName As String = If(String.IsNullOrWhiteSpace(nlaRecord.employeemaco), String.Format("{0} {1}", nlaRecord.employeefirstname, nlaRecord.employeelastname), (nlaRecord.employeemaco))
		'	pdfFormFields.SetField("HName", hName)
		'	pdfFormFields.SetField("HAdresse", String.Format("{0}", nlaRecord.employeemastrasse))
		'	Dim hPostfach As String = If(String.IsNullOrWhiteSpace(nlaRecord.employeemapostfach), String.Format("{0}-{1} {2}", nlaRecord.employeemaland, nlaRecord.employeemaplz, nlaRecord.employeemaort), nlaRecord.employeemapostfach)
		'	pdfFormFields.SetField("HPostfach", hPostfach)
		'	Dim hWohnort As String = If(String.IsNullOrWhiteSpace(nlaRecord.employeemapostfach), String.Empty, String.Format("{0}-{1} {2}", nlaRecord.employeemaland, nlaRecord.employeemaplz, nlaRecord.employeemaort))
		'	pdfFormFields.SetField("HWohnort", hWohnort)

		'	pdfFormFields.SetField("1", If(nlaRecord.Z_1_0 = 0, String.Empty, Math.Floor(nlaRecord.Z_1_0.GetValueOrDefault(0))))

		'	pdfFormFields.SetField("2-1", If(nlaRecord.Z_2_1.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(nlaRecord.Z_2_1.GetValueOrDefault(0))))
		'	pdfFormFields.SetField("2-2", If(nlaRecord.Z_2_2.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(nlaRecord.Z_2_2.GetValueOrDefault(0))))
		'	pdfFormFields.SetField("2-3-1", If(nlaRecord.Z_2_3.GetValueOrDefault(0) <> 0, If(nlaRecord.NLA_2_3 = String.Empty, _ClsSetting.NLA_2_3, nlaRecord.NLA_2_3), String.Empty))
		'	pdfFormFields.SetField("2-3-2", If(nlaRecord.Z_2_3.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(nlaRecord.Z_2_3.GetValueOrDefault(0))))

		'	pdfFormFields.SetField("3-1", If(nlaRecord.Z_3_0.GetValueOrDefault(0) <> 0, If(nlaRecord.NLA_3_0 = String.Empty, _ClsSetting.NLA_3_0, nlaRecord.NLA_3_0), String.Empty))
		'	pdfFormFields.SetField("3-2", If(nlaRecord.Z_3_0.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(nlaRecord.Z_3_0.GetValueOrDefault(0))))
		'	pdfFormFields.SetField("4-1", If(nlaRecord.Z_4_0.GetValueOrDefault(0) <> 0, If(nlaRecord.NLA_4_0 = String.Empty, _ClsSetting.NLA_4_0, nlaRecord.NLA_4_0), String.Empty))
		'	pdfFormFields.SetField("4-2", If(nlaRecord.Z_4_0.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(nlaRecord.Z_4_0.GetValueOrDefault(0))))

		'	pdfFormFields.SetField("5", If(nlaRecord.Z_5_0.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(nlaRecord.Z_5_0.GetValueOrDefault(0))))
		'	pdfFormFields.SetField("6", If(nlaRecord.Z_6_0.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(nlaRecord.Z_6_0.GetValueOrDefault(0))))
		'	pdfFormFields.SetField("7-1", If(nlaRecord.Z_7_0.GetValueOrDefault(0) <> 0, If(nlaRecord.NLA_7_0 = String.Empty, _ClsSetting.NLA_7_0, nlaRecord.NLA_7_0), String.Empty))
		'	pdfFormFields.SetField("7-1-2", If(nlaRecord.Z_7_0.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(nlaRecord.Z_7_0.GetValueOrDefault(0))))
		'	pdfFormFields.SetField("8", If(nlaRecord.Z_8_0 = 0, String.Empty, Math.Floor(nlaRecord.Z_8_0.GetValueOrDefault(0))))

		'	pdfFormFields.SetField("9", If(nlaRecord.Z_9_0.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(nlaRecord.Z_9_0.GetValueOrDefault(0) * -1)))
		'	' If (nlaRecord.Z_9_0.GetValueOrDefault(0) < 0, nlaRecord.Z_9_0.GetValueOrDefault(0) * -1, nlaRecord.Z_9_0.GetValueOrDefault(0)) Then)))
		'	pdfFormFields.SetField("10-1", If(nlaRecord.Z_10_1.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(nlaRecord.Z_10_1.GetValueOrDefault(0) * -1)))
		'	' If (nlaRecord.Z_10_1.GetValueOrDefault(0) < 0, nlaRecord.Z_10_1.GetValueOrDefault(0) * -1, nlaRecord.Z_10_1.GetValueOrDefault(0) * -1) Then)))
		'	pdfFormFields.SetField("10-2", If(nlaRecord.Z_10_2.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(nlaRecord.Z_10_2.GetValueOrDefault(0) * -1)))
		'	' If (nlaRecord.Z_10_2.GetValueOrDefault(0) < 0, nlaRecord.Z_10_2.GetValueOrDefault(0) * -1, nlaRecord.Z_10_2.GetValueOrDefault(0) * -1) Then)))
		'	pdfFormFields.SetField("11", If(nlaRecord.Z_11_0.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(nlaRecord.Z_11_0.GetValueOrDefault(0))))

		'	pdfFormFields.SetField("12", If(nlaRecord.Z_12_0.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(nlaRecord.Z_12_0.GetValueOrDefault(0) * -1)))
		'	' If (nlaRecord.Z_12_0.GetValueOrDefault(0) < 0, nlaRecord.Z_12_0.GetValueOrDefault(0) * -1, nlaRecord.Z_12_0.GetValueOrDefault(0) * -1) Then)))
		'	pdfFormFields.SetField("13-1-1-2", If(nlaRecord.Z_13_1_1.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(nlaRecord.Z_13_1_1.GetValueOrDefault(0))))
		'	pdfFormFields.SetField("13-1-1-1", If(nlaRecord.Z_13_1_1.GetValueOrDefault(0) = 0, "nein", "ja"))

		'	pdfFormFields.SetField("13-1-2-1", If(nlaRecord.Z_13_1_2.GetValueOrDefault(0) <> 0, If(nlaRecord.NLA_13_1_2 = String.Empty, _ClsSetting.NLA_13_1_2, nlaRecord.NLA_13_1_2), String.Empty))
		'	pdfFormFields.SetField("13-1-2-2", If(nlaRecord.Z_13_1_2.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(nlaRecord.Z_13_1_2.GetValueOrDefault(0))))

		'	pdfFormFields.SetField("13-2-1-2", If(nlaRecord.Z_13_2_1.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(nlaRecord.Z_13_2_1.GetValueOrDefault(0))))
		'	pdfFormFields.SetField("13-2-2-2", If(nlaRecord.Z_13_2_2.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(nlaRecord.Z_13_2_2.GetValueOrDefault(0))))

		'	pdfFormFields.SetField("13-2-3-1", If(nlaRecord.Z_13_2_3.GetValueOrDefault(0) <> 0, If(nlaRecord.NLA_13_2_3 = String.Empty, _ClsSetting.NLA_13_2_3, nlaRecord.NLA_13_2_3), String.Empty))
		'	pdfFormFields.SetField("13-2-3-2", If(nlaRecord.Z_13_2_3.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(nlaRecord.Z_13_2_3.GetValueOrDefault(0))))

		'	pdfFormFields.SetField("13-3", If(nlaRecord.Z_13_3_0.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(nlaRecord.Z_13_3_0.GetValueOrDefault(0))))

		'	pdfFormFields.SetField("14-1", If(nlaRecord.NLA_Nebenleistung_1 = String.Empty, _ClsSetting.NLA_14_1_1, nlaRecord.NLA_Nebenleistung_1))
		'	pdfFormFields.SetField("14-2", If(nlaRecord.NLA_Nebenleistung_2 = String.Empty, _ClsSetting.NLA_14_1_2, nlaRecord.NLA_Nebenleistung_2))
		'	pdfFormFields.SetField("15-1", If(nlaRecord.NLA_Bemerkung_1 = String.Empty, _ClsSetting.NLA_15_1_1, nlaRecord.NLA_Bemerkung_1))
		'	pdfFormFields.SetField("15-2", If(nlaRecord.NLA_Bemerkung_2 = String.Empty, _ClsSetting.NLA_15_1_2, nlaRecord.NLA_Bemerkung_2))


		'	'result = result AndAlso FillMDDataIntoPDF(pdfFormFields)




		'	pdfStamper.FormFlattening = False

		'	Dim fields() As String = pdfStamper.AcroFields.Fields.[Select](Function(x) x.Key).ToArray()
		'	For key As Integer = 0 To fields.Count - 1
		'		pdfStamper.AcroFields.SetFieldProperty(fields(key), "setfflags", PdfFormField.FF_READ_ONLY, Nothing)
		'	Next

		'	' close the pdf
		'	pdfStamper.Close()
		'	' pdfReader.close() ---> DON"T EVER CLOSE READER If YOU'RE GENERATING LOTS OF PDF FILES IN LOOP

		'	'Add Watermark
		'	'AddPDFWatermark("C:\test_words_replaced.pdf", "C:\test_Watermarked_and_Replaced.pdf", Application.StartupPath & "\Anuba.jpg")


		'	Return result
		'End Function

		Private Function ListAllPDFFields(ByVal pdfFilename As String) As List(Of String)
			Dim result As New List(Of String)

			Using documentProcessor As New PdfDocumentProcessor()
				documentProcessor.LoadDocument(pdfFilename)

				' Get names of interactive form fields.
				Dim formData As PdfFormData = documentProcessor.GetFormData()
				Dim names As IList(Of String) = formData.GetFieldNames()

				' Show the field names in the rich text box.
				Dim strings(names.Count - 1) As String
				names.CopyTo(strings, 0)

				Trace.WriteLine(strings.ToList())

				result = strings.ToList()
			End Using

			Return result
		End Function

		Private Function CreatedPDFDocWithDevExpressComponents() As Boolean
			Dim result As Boolean = True
			Dim formData As PdfFormData

			m_CurrentDoneNLAFile = String.Empty
			Using documentProcessor As New PdfDocumentProcessor()

				documentProcessor.LoadDocument(m_NLATemplate)
				' Obtain interactive form data from a document.
				formData = documentProcessor.GetFormData()

				'Dim TempFilename As String = Path.GetTempFileName ' Combine(_ClsProgSetting.GetSpSNLATempPath, String.Format("{0}.pdf", m_CurrentNLAData.MANr))
				Dim TempFilename As String = Path.Combine(m_InitializationData.UserData.spTempNLAPath, String.Format("Lohnausweis_{0}_{1}.pdf", m_CurrentNLAData.MANr, m_CurrentNLAData.employeelajahr))

				If File.Exists(TempFilename) Then
					Try
						File.Delete(TempFilename)
					Catch ex As Exception
						TempFilename = Path.Combine(m_InitializationData.UserData.spTempNLAPath, String.Format("Lohnausweis_{0}_{1}_{2}.ex", m_CurrentNLAData.MANr, m_CurrentNLAData.employeelajahr, Environment.TickCount.ToString))
						TempFilename = Path.ChangeExtension(TempFilename, ".pdf")

					End Try
				End If


				'TempFilename = Path.ChangeExtension(TempFilename, "PDF")
				'TempFilename = String.Format("C:\Path\PDF\Final_fw_{0}.pdf", m_CurrentNLAData.MANr)

				Try

					Dim fieldData = LoadPDFFields(m_NLATemplate)
					For Each itm In fieldData
						Trace.WriteLine(itm.ToString)
					Next
					' Specify values for FirstName, LastName and Gender form fields.
					If m_CurrentNLAData.NLA_LoAusweis Then formData("A").Value = "Ja"
					If Not m_CurrentNLAData.NLA_LoAusweis Then formData("B").Value = "Ja"
					formData("C").Value = m_CurrentNLAData.employeeahv_nr

					formData("C2").Value = m_CurrentNLAData.employeeahv_nr_new
					formData("13-1-1-1").Value = If(CBool(m_CurrentNLAData.Z_13_1_1), "Ja", "Off")
					formData("F").Value = If(CBool(m_CurrentNLAData.NLA_Befoerderung) OrElse m_CurrentNLAData.Z_2_2.GetValueOrDefault(0) <> 0, "Ja", "Off")
					formData("G").Value = If(m_CurrentNLAData.NLA_Kantine OrElse m_CurrentNLAData.Z_2_1.GetValueOrDefault(0) <> 0, "Ja", "Off")

					formData("D").Value = String.Format("{0}", m_CurrentNLAData.employeelajahr)
					formData("E-von").Value = m_CurrentNLAData.ES_Ab1
					formData("E-bis").Value = m_CurrentNLAData.ES_Bis1
					Dim hAnrede As String = m_Translate.GetSafeTranslationValue(If(m_CurrentNLAData.employeegeschlecht = "M", "Herr", "Frau")) & If(String.IsNullOrWhiteSpace(m_CurrentNLAData.employeemaco), "", String.Format(" {0} {1}", m_CurrentNLAData.employeefirstname, m_CurrentNLAData.employeelastname))
					formData("HAnrede").Value = hAnrede
					Dim hName As String = If(String.IsNullOrWhiteSpace(m_CurrentNLAData.employeemaco), String.Format("{0} {1}", m_CurrentNLAData.employeefirstname, m_CurrentNLAData.employeelastname), (m_CurrentNLAData.employeemaco))
					formData("HName").Value = hName
					formData("HAdresse").Value = String.Format("{0}", m_CurrentNLAData.employeemastrasse)
					Dim hPostfach As String = If(String.IsNullOrWhiteSpace(m_CurrentNLAData.employeemapostfach), String.Format("{0}-{1} {2}", m_CurrentNLAData.employeemaland, m_CurrentNLAData.employeemaplz, m_CurrentNLAData.employeemaort), m_CurrentNLAData.employeemapostfach)
					formData("HPostfach").Value = hPostfach
					Dim hWohnort As String = If(String.IsNullOrWhiteSpace(m_CurrentNLAData.employeemapostfach), String.Empty, String.Format("{0}-{1} {2}", m_CurrentNLAData.employeemaland, m_CurrentNLAData.employeemaplz, m_CurrentNLAData.employeemaort))
					formData("HWohnort").Value = hWohnort

					formData("1").Value = String.Format("{0}", If(m_CurrentNLAData.Z_1_0 = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_1_0.GetValueOrDefault(0))))
					formData("2-1").Value = String.Format("{0}", If(m_CurrentNLAData.Z_2_1.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_2_1.GetValueOrDefault(0))))
					formData("2-2").Value = String.Format("{0}", If(m_CurrentNLAData.Z_2_2.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_2_2.GetValueOrDefault(0))))
					formData("2-3-1").Value = String.Format("{0}", If(m_CurrentNLAData.Z_2_3.GetValueOrDefault(0) <> 0, If(m_CurrentNLAData.NLA_2_3 = String.Empty, _ClsSetting.NLA_2_3, m_CurrentNLAData.NLA_2_3), String.Empty))
					formData("2-3-2").Value = String.Format("{0}", If(m_CurrentNLAData.Z_2_3.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_2_3.GetValueOrDefault(0))))

					formData("3-1").Value = String.Format("{0}", If(m_CurrentNLAData.Z_3_0.GetValueOrDefault(0) <> 0, If(m_CurrentNLAData.NLA_3_0 = String.Empty, _ClsSetting.NLA_3_0, m_CurrentNLAData.NLA_3_0), String.Empty))
					formData("3-2").Value = String.Format("{0}", If(m_CurrentNLAData.Z_3_0.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_3_0.GetValueOrDefault(0))))
					formData("4-1").Value = String.Format("{0}", If(m_CurrentNLAData.Z_4_0.GetValueOrDefault(0) <> 0, If(m_CurrentNLAData.NLA_4_0 = String.Empty, _ClsSetting.NLA_4_0, m_CurrentNLAData.NLA_4_0), String.Empty))
					formData("4-2").Value = String.Format("{0}", If(m_CurrentNLAData.Z_4_0.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_4_0.GetValueOrDefault(0))))

					formData("5").Value = String.Format("{0}", If(m_CurrentNLAData.Z_5_0.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_5_0.GetValueOrDefault(0))))
					formData("6").Value = String.Format("{0}", If(m_CurrentNLAData.Z_6_0.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_6_0.GetValueOrDefault(0))))
					formData("7-1").Value = String.Format("{0}", If(m_CurrentNLAData.Z_7_0.GetValueOrDefault(0) <> 0, If(m_CurrentNLAData.NLA_7_0 = String.Empty, _ClsSetting.NLA_7_0, m_CurrentNLAData.NLA_7_0), String.Empty))
					formData("7-1-2").Value = String.Format("{0}", If(m_CurrentNLAData.Z_7_0.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_7_0.GetValueOrDefault(0))))
					formData("8").Value = String.Format("{0}", If(m_CurrentNLAData.Z_8_0 = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_8_0.GetValueOrDefault(0))))

					formData("9").Value = String.Format("{0}", If(m_CurrentNLAData.Z_9_0.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_9_0.GetValueOrDefault(0) * -1)))
					formData("10-1").Value = String.Format("{0}", If(m_CurrentNLAData.Z_10_1.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_10_1.GetValueOrDefault(0) * -1)))
					formData("10-2").Value = String.Format("{0}", If(m_CurrentNLAData.Z_10_2.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_10_2.GetValueOrDefault(0) * -1)))
					formData("11").Value = String.Format("{0}", If(m_CurrentNLAData.Z_11_0.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_11_0.GetValueOrDefault(0))))

					formData("12").Value = String.Format("{0}", If(m_CurrentNLAData.Z_12_0.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_12_0.GetValueOrDefault(0) * -1)))
					formData("13-1-1-2").Value = String.Format("{0}", If(m_CurrentNLAData.Z_13_1_1.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_13_1_1.GetValueOrDefault(0))))
					formData("13-1-1-1").Value = String.Format("{0}", If(m_CurrentNLAData.Z_13_1_1.GetValueOrDefault(0) = 0, "Off", "Ja"))

					formData("13-1-2-1").Value = String.Format("{0}", If(m_CurrentNLAData.Z_13_1_2.GetValueOrDefault(0) <> 0, If(m_CurrentNLAData.NLA_13_1_2 = String.Empty, _ClsSetting.NLA_13_1_2, m_CurrentNLAData.NLA_13_1_2), String.Empty))
					formData("13-1-2-2").Value = String.Format("{0}", If(m_CurrentNLAData.Z_13_1_2.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_13_1_2.GetValueOrDefault(0))))

					formData("13-2-1-2").Value = String.Format("{0}", If(m_CurrentNLAData.Z_13_2_1.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_13_2_1.GetValueOrDefault(0))))
					formData("13-2-2-2").Value = String.Format("{0}", If(m_CurrentNLAData.Z_13_2_2.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_13_2_2.GetValueOrDefault(0))))

					formData("13-2-3-1").Value = String.Format("{0}", If(m_CurrentNLAData.Z_13_2_3.GetValueOrDefault(0) <> 0, If(m_CurrentNLAData.NLA_13_2_3 = String.Empty, _ClsSetting.NLA_13_2_3, m_CurrentNLAData.NLA_13_2_3), String.Empty))
					formData("13-2-3-2").Value = String.Format("{0}", If(m_CurrentNLAData.Z_13_2_3.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_13_2_3.GetValueOrDefault(0))))

					formData("13-3").Value = String.Format("{0}", If(m_CurrentNLAData.Z_13_3_0.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_13_3_0.GetValueOrDefault(0))))

					formData("14-1").Value = String.Format("{0}", If(m_CurrentNLAData.NLA_Nebenleistung_1 = String.Empty, _ClsSetting.NLA_14_1_1, m_CurrentNLAData.NLA_Nebenleistung_1))
					formData("14-2").Value = String.Format("{0}", If(m_CurrentNLAData.NLA_Nebenleistung_2 = String.Empty, _ClsSetting.NLA_14_1_2, m_CurrentNLAData.NLA_Nebenleistung_2))
					formData("15-1").Value = String.Format("{0}", If(m_CurrentNLAData.NLA_Bemerkung_1 = String.Empty, _ClsSetting.NLA_15_1_1, m_CurrentNLAData.NLA_Bemerkung_1))
					formData("15-2").Value = String.Format("{0}", If(m_CurrentNLAData.NLA_Bemerkung_2 = String.Empty, _ClsSetting.NLA_15_1_2, m_CurrentNLAData.NLA_Bemerkung_2))

					result = result AndAlso FillMDDataIntoPDF(formData)

				Catch ex As Exception
					m_Logger.LogError(String.Format("error during filling form. {0}", ex.ToString))

					Return False
				End Try

				If result Then
					If Not m_EnableEditingTemplateData Then
						If documentProcessor.FlattenForm() Then
							documentProcessor.SaveDocument(TempFilename)

						Else
							m_Logger.LogWarning(String.Format("for employee: {0} form could not be successfully save!", m_CurrentNLAData.MANr))
						End If

					Else
						documentProcessor.SaveDocument(TempFilename)

						m_Logger.LogWarning(String.Format("for employee: {0} form is not saved as readonly! EnableEditingTemplateData: {1}", m_CurrentNLAData.MANr, m_EnableEditingTemplateData))
					End If
					m_CurrentDoneNLAFile = TempFilename

				Else
					m_Logger.LogWarning(String.Format("for employee: {0} form could not be successfully filled!", m_CurrentNLAData.MANr))
				End If

			End Using


			Return result
		End Function

		Private Function Create2021NLAPDFDoc() As Boolean
			Dim result As Boolean = True
			Dim formData As PdfFormData

			m_CurrentDoneNLAFile = String.Empty
			Using documentProcessor As New PdfDocumentProcessor()

				documentProcessor.LoadDocument(m_NLATemplate)
				' Obtain interactive form data from a document.
				formData = documentProcessor.GetFormData()

				Dim TempFilename As String = Path.Combine(m_InitializationData.UserData.spTempNLAPath, String.Format("Lohnausweis_{0}_{1}.pdf", m_CurrentNLAData.MANr, m_CurrentNLAData.employeelajahr))

				If File.Exists(TempFilename) Then
					Try
						File.Delete(TempFilename)
					Catch ex As Exception
						TempFilename = Path.Combine(m_InitializationData.UserData.spTempNLAPath, String.Format("Lohnausweis_{0}_{1}_{2}.ex", m_CurrentNLAData.MANr, m_CurrentNLAData.employeelajahr, Environment.TickCount.ToString))
						TempFilename = Path.ChangeExtension(TempFilename, ".pdf")

					End Try
				End If

				'Dim TempFilename As String = Path.Combine(m_InitializationData.UserData.spTempNLAPath, String.Format("NLA_{0}_{1}", Path.GetFileNameWithoutExtension(Path.GetRandomFileName), m_CurrentNLAData.MANr))

				Try

					Dim fieldData = LoadPDFFields(m_NLATemplate)
					For Each itm In fieldData
						Trace.WriteLine(itm.ToString)
					Next
					' Specify values for FirstName, LastName and Gender form fields.
					If m_CurrentNLAData.NLA_LoAusweis Then
						formData("OptionKreuzOhneRahmen_A").Value = "Ja / Oui / Sì"
					Else
						formData("OptionKreuzOhneRahmen_B").Value = "Ja / Oui / Sì"
					End If
					formData("AHVLinks_C").Value = m_CurrentNLAData.employeeahv_nr_new
					formData("TextLinks_C-GebDatum").Value = Format("{0:d}", m_CurrentNLAData.employeebirthdate)

					formData("OptionKreuzOhneRahmen_13_1_1").Value = If(CBool(m_CurrentNLAData.Z_13_1_1), "Ja / Oui / Sì", "Off")
					formData("OptionKreuzOhneRahmen_F").Value = If(CBool(m_CurrentNLAData.NLA_Befoerderung) OrElse m_CurrentNLAData.Z_2_2.GetValueOrDefault(0) <> 0, "Ja / Oui / Sì", "Off")
					formData("OptionKreuzOhneRahmen_G").Value = If(m_CurrentNLAData.NLA_Kantine OrElse m_CurrentNLAData.Z_2_1.GetValueOrDefault(0) <> 0, "Ja / Oui / Sì", "Off")

					formData("TextLinks_D").Value = String.Format("{0}", m_CurrentNLAData.employeelajahr)
					formData("TextLinks_E-von").Value = m_CurrentNLAData.ES_Ab1
					formData("TextLinks_E-bis").Value = m_CurrentNLAData.ES_Bis1


					Dim hAnrede As String = m_Translate.GetSafeTranslationValue(If(m_CurrentNLAData.employeegeschlecht = "M", "Herr", "Frau")) & If(String.IsNullOrWhiteSpace(m_CurrentNLAData.employeemaco), "",
						String.Format(" {0} {1}", m_CurrentNLAData.employeefirstname, m_CurrentNLAData.employeelastname))
					Dim hName As String = If(String.IsNullOrWhiteSpace(m_CurrentNLAData.employeemaco), String.Format("{0} {1}", m_CurrentNLAData.employeefirstname, m_CurrentNLAData.employeelastname), (m_CurrentNLAData.employeemaco))
					Dim hPostfach As String = If(String.IsNullOrWhiteSpace(m_CurrentNLAData.employeemapostfach), String.Format("{0}-{1} {2}", m_CurrentNLAData.employeemaland, m_CurrentNLAData.employeemaplz, m_CurrentNLAData.employeemaort), m_CurrentNLAData.employeemapostfach)
					Dim hWohnort As String = If(String.IsNullOrWhiteSpace(m_CurrentNLAData.employeemapostfach), String.Empty, String.Format("{0}-{1} {2}", m_CurrentNLAData.employeemaland, m_CurrentNLAData.employeemaplz, m_CurrentNLAData.employeemaort))
					formData("TextMehrzeiligLinks_Empfaenger").Value = String.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}", vbNewLine, hAnrede, hName, m_CurrentNLAData.employeemastrasse, hPostfach, hWohnort)




					formData("DezZahlNull_1").Value = String.Format("{0}", If(m_CurrentNLAData.Z_1_0 = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_1_0.GetValueOrDefault(0))))
					formData("DezZahlNull_2_1").Value = String.Format("{0}", If(m_CurrentNLAData.Z_2_1.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_2_1.GetValueOrDefault(0))))
					formData("DezZahlNull_2_2").Value = String.Format("{0}", If(m_CurrentNLAData.Z_2_2.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_2_2.GetValueOrDefault(0))))
					formData("TextLinks_2_3-Art").Value = String.Format("{0}", If(m_CurrentNLAData.Z_2_3.GetValueOrDefault(0) <> 0, If(m_CurrentNLAData.NLA_2_3 = String.Empty, _ClsSetting.NLA_2_3, m_CurrentNLAData.NLA_2_3), String.Empty))
					formData("DezZahlNull_2_3").Value = String.Format("{0}", If(m_CurrentNLAData.Z_2_3.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_2_3.GetValueOrDefault(0))))

					formData("TextLinks_3-Art").Value = String.Format("{0}", If(m_CurrentNLAData.Z_3_0.GetValueOrDefault(0) <> 0, If(m_CurrentNLAData.NLA_3_0 = String.Empty, _ClsSetting.NLA_3_0, m_CurrentNLAData.NLA_3_0), String.Empty))
					formData("DezZahlNull_3").Value = String.Format("{0}", If(m_CurrentNLAData.Z_3_0.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_3_0.GetValueOrDefault(0))))
					formData("TextLinks_4-Art").Value = String.Format("{0}", If(m_CurrentNLAData.Z_4_0.GetValueOrDefault(0) <> 0, If(m_CurrentNLAData.NLA_4_0 = String.Empty, _ClsSetting.NLA_4_0, m_CurrentNLAData.NLA_4_0), String.Empty))
					formData("DezZahlNull_4").Value = String.Format("{0}", If(m_CurrentNLAData.Z_4_0.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_4_0.GetValueOrDefault(0))))

					formData("DezZahlNull_5").Value = String.Format("{0}", If(m_CurrentNLAData.Z_5_0.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_5_0.GetValueOrDefault(0))))
					formData("DezZahlNull_6").Value = String.Format("{0}", If(m_CurrentNLAData.Z_6_0.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_6_0.GetValueOrDefault(0))))
					formData("TextLinks_7-Art").Value = String.Format("{0}", If(m_CurrentNLAData.Z_7_0.GetValueOrDefault(0) <> 0, If(m_CurrentNLAData.NLA_7_0 = String.Empty, _ClsSetting.NLA_7_0, m_CurrentNLAData.NLA_7_0), String.Empty))
					formData("DezZahlNull_7").Value = String.Format("{0}", If(m_CurrentNLAData.Z_7_0.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_7_0.GetValueOrDefault(0))))
					formData("DezZahlNull_8").Value = String.Format("{0}", If(m_CurrentNLAData.Z_8_0 = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_8_0.GetValueOrDefault(0))))

					formData("DezZahlNull_9").Value = String.Format("{0}", If(m_CurrentNLAData.Z_9_0.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_9_0.GetValueOrDefault(0) * -1)))
					formData("DezZahlNull_10_1").Value = String.Format("{0}", If(m_CurrentNLAData.Z_10_1.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_10_1.GetValueOrDefault(0) * -1)))
					formData("DezZahlNull_10_2").Value = String.Format("{0}", If(m_CurrentNLAData.Z_10_2.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_10_2.GetValueOrDefault(0) * -1)))
					formData("DezZahlNull_11").Value = String.Format("{0}", If(m_CurrentNLAData.Z_11_0.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_11_0.GetValueOrDefault(0))))

					formData("DezZahlNull_12").Value = String.Format("{0}", If(m_CurrentNLAData.Z_12_0.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_12_0.GetValueOrDefault(0) * -1)))
					formData("DezZahlNull_13_1_1").Value = String.Format("{0}", If(m_CurrentNLAData.Z_13_1_1.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_13_1_1.GetValueOrDefault(0))))
					formData("OptionKreuzOhneRahmen_13_1_1").Value = String.Format("{0}", If(m_CurrentNLAData.Z_13_1_1.GetValueOrDefault(0) = 0, "Off", "Ja / Oui / Sì"))

					formData("TextLinks_13_1_2-Art").Value = String.Format("{0}", If(m_CurrentNLAData.Z_13_1_2.GetValueOrDefault(0) <> 0, If(m_CurrentNLAData.NLA_13_1_2 = String.Empty, _ClsSetting.NLA_13_1_2, m_CurrentNLAData.NLA_13_1_2), String.Empty))
					formData("DezZahlNull_13_1_2").Value = String.Format("{0}", If(m_CurrentNLAData.Z_13_1_2.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_13_1_2.GetValueOrDefault(0))))

					formData("DezZahlNull_13_2_1").Value = String.Format("{0}", If(m_CurrentNLAData.Z_13_2_1.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_13_2_1.GetValueOrDefault(0))))
					formData("DezZahlNull_13_2_2").Value = String.Format("{0}", If(m_CurrentNLAData.Z_13_2_2.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_13_2_2.GetValueOrDefault(0))))

					formData("TextLinks_13_2_3-Art").Value = String.Format("{0}", If(m_CurrentNLAData.Z_13_2_3.GetValueOrDefault(0) <> 0, If(m_CurrentNLAData.NLA_13_2_3 = String.Empty, _ClsSetting.NLA_13_2_3, m_CurrentNLAData.NLA_13_2_3), String.Empty))
					formData("DezZahlNull_13_2_3").Value = String.Format("{0}", If(m_CurrentNLAData.Z_13_2_3.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_13_2_3.GetValueOrDefault(0))))

					formData("DezZahlNull_13_3").Value = String.Format("{0}", If(m_CurrentNLAData.Z_13_3_0.GetValueOrDefault(0) = 0, String.Empty, Math.Floor(m_CurrentNLAData.Z_13_3_0.GetValueOrDefault(0))))

					formData("TextLinks_14_1").Value = String.Format("{0}", If(m_CurrentNLAData.NLA_Nebenleistung_1 = String.Empty, _ClsSetting.NLA_14_1_1, m_CurrentNLAData.NLA_Nebenleistung_1))
					formData("TextLinks_14_2").Value = String.Format("{0}", If(m_CurrentNLAData.NLA_Nebenleistung_2 = String.Empty, _ClsSetting.NLA_14_1_2, m_CurrentNLAData.NLA_Nebenleistung_2))
					formData("TextLinks_15_1").Value = String.Format("{0}", If(m_CurrentNLAData.NLA_Bemerkung_1 = String.Empty, _ClsSetting.NLA_15_1_1, m_CurrentNLAData.NLA_Bemerkung_1))
					formData("TextLinks_15_2").Value = String.Format("{0}", If(m_CurrentNLAData.NLA_Bemerkung_2 = String.Empty, _ClsSetting.NLA_15_1_2, m_CurrentNLAData.NLA_Bemerkung_2))

					result = result AndAlso FillMDDataInto2021PDF(formData)


				Catch ex As Exception
					m_Logger.LogError(String.Format("error during filling form. {0}", ex.ToString))

					Return False
				End Try

				If result Then
					If Not m_EnableEditingTemplateData Then
						If documentProcessor.FlattenForm() Then
							documentProcessor.SaveDocument(TempFilename)

						Else
							m_Logger.LogWarning(String.Format("for employee: {0} form could not be successfully save!", m_CurrentNLAData.MANr))
						End If

					Else
						documentProcessor.SaveDocument(TempFilename)

						m_Logger.LogWarning(String.Format("for employee: {0} form is not saved as readonly! EnableEditingTemplateData: {1}", m_CurrentNLAData.MANr, m_EnableEditingTemplateData))
					End If
					m_CurrentDoneNLAFile = TempFilename

				Else
					m_Logger.LogWarning(String.Format("for employee: {0} form could not be successfully filled!", m_CurrentNLAData.MANr))
				End If

			End Using


			Return result
		End Function

		Private Shared Function EncryptPassword(ByVal passwordText As String) As SecureString
			Dim password As New SecureString()
			For Each c As Char In passwordText
				password.AppendChar(c)
			Next c
			Return password
		End Function

		Function FillMDDataIntoPDF(ByVal formData As PdfFormData) As Boolean
			Dim result As Boolean = True

			Try
				formData("OrtDatum").Value = String.Format("{0}, {1}", m_InitializationData.MDData.MDCity, String.Format(Now.Date, "G"))
				If Not m_ShowAdvisorData Then
					formData("Unterschrift1.0").Value = String.Format("{0}", m_InitializationData.UserData.UserFullName)
					formData("Unterschrift1.1").Value = String.Format("{0}", m_InitializationData.MDData.MDName)

				Else
					formData("Unterschrift1.0").Value = String.Format("{0}", m_InitializationData.MDData.MDName)
					formData("Unterschrift1.1").Value = String.Format("{0}", m_InitializationData.MDData.MDName_2)
				End If

				If String.IsNullOrWhiteSpace(m_InitializationData.MDData.MDTelefon) Then
					formData("Unterschrift1.2").Value = String.Format("{0}", If(m_ShowAdvisorData, m_InitializationData.MDData.MDName_2, m_InitializationData.MDData.MDName_3))
					formData("Unterschrift1.3").Value = String.Format("{0}", m_InitializationData.MDData.MDStreet)
					formData("Unterschrift1.4").Value = String.Format("{0}-{1} {2}", m_InitializationData.MDData.MDCountry, m_InitializationData.MDData.MDPostcode, m_InitializationData.MDData.MDCity)

				Else
					formData("Unterschrift1.2").Value = String.Format("{0}", m_InitializationData.MDData.MDStreet)
					formData("Unterschrift1.3").Value = String.Format("{0}-{1} {2}", m_InitializationData.MDData.MDCountry, m_InitializationData.MDData.MDPostcode, m_InitializationData.MDData.MDCity)
					formData("Unterschrift1.4").Value = String.Format("{0}", m_InitializationData.MDData.MDTelefon)

				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("MD-Adresse füllen: {0}", ex.ToString))
				Return False

			End Try

			Return result
		End Function

		Function FillMDDataInto2021PDF(ByVal formData As PdfFormData) As Boolean
			Dim result As Boolean = True

			Try
				Dim signData As String

				formData("TextLinks_I").Value = String.Format("{0}, {1}", m_InitializationData.MDData.MDCity, String.Format(Now.Date, "G"))
				If Not m_ShowAdvisorData Then
					'formData("Unterschrift1.0").Value = String.Format("{0}", m_InitializationData.UserData.UserFullName)
					'formData("Unterschrift1.1").Value = String.Format("{0}", m_InitializationData.MDData.MDName)

					signData = String.Format("{1}{0}{2}{0}", vbNewLine, m_InitializationData.UserData.UserFullName, m_InitializationData.MDData.MDName)

				Else
					'formData("Unterschrift1.0").Value = String.Format("{0}", m_InitializationData.MDData.MDName)
					'formData("Unterschrift1.1").Value = String.Format("{0}", m_InitializationData.MDData.MDName_2)
					If String.IsNullOrWhiteSpace(m_InitializationData.MDData.MDName_2) Then
						signData = String.Format("{1}{0}", vbNewLine, m_InitializationData.MDData.MDName)
					Else
						signData = String.Format("{1}{0}{2}{0}", vbNewLine, m_InitializationData.MDData.MDName, m_InitializationData.MDData.MDName_2)
					End If

				End If


				If String.IsNullOrWhiteSpace(m_InitializationData.MDData.MDTelefon) Then
					'formData("Unterschrift1.2").Value = String.Format("{0}", If(m_ShowAdvisorData, m_InitializationData.MDData.MDName_2, m_InitializationData.MDData.MDName_3))
					'formData("Unterschrift1.3").Value = String.Format("{0}", m_InitializationData.MDData.MDStreet)
					'formData("Unterschrift1.4").Value = String.Format("{0}-{1} {2}", m_InitializationData.MDData.MDCountry, m_InitializationData.MDData.MDPostcode, m_InitializationData.MDData.MDCity)

					If String.IsNullOrWhiteSpace(m_InitializationData.MDData.MDName_2) Then
						signData &= String.Format("{1}{0}{2}", vbNewLine,
											  m_InitializationData.MDData.MDStreet,
											  String.Format("{0}-{1} {2}", m_InitializationData.MDData.MDCountry, m_InitializationData.MDData.MDPostcode, m_InitializationData.MDData.MDCity))

					Else
						signData &= String.Format("{1}{0}{2}{0}{3}", vbNewLine, If(m_ShowAdvisorData, m_InitializationData.MDData.MDName_2, m_InitializationData.MDData.MDName_3),
											  m_InitializationData.MDData.MDStreet,
											  String.Format("{0}-{1} {2}", m_InitializationData.MDData.MDCountry, m_InitializationData.MDData.MDPostcode, m_InitializationData.MDData.MDCity))

					End If

				Else
					'formData("Unterschrift1.2").Value = String.Format("{0}", m_InitializationData.MDData.MDStreet)
					'formData("Unterschrift1.3").Value = String.Format("{0}-{1} {2}", m_InitializationData.MDData.MDCountry, m_InitializationData.MDData.MDPostcode, m_InitializationData.MDData.MDCity)
					'formData("Unterschrift1.4").Value = String.Format("{0}", m_InitializationData.MDData.MDTelefon)

					signData &= String.Format("{1}{0}{2}{0}{3}", vbNewLine, m_InitializationData.MDData.MDStreet,
											  String.Format("{0}-{1} {2}", m_InitializationData.MDData.MDCountry, m_InitializationData.MDData.MDPostcode, m_InitializationData.MDData.MDCity),
											  m_InitializationData.MDData.MDTelefon)


				End If
				formData("TextMehrzeiligLinks_Bestaetigung").Value = String.Format("{0}", signData)


			Catch ex As Exception
				m_Logger.LogError(String.Format("MD-Adresse füllen: {0}", ex.ToString))
				Return False

			End Try

			Return result
		End Function


	End Class



	Class Program

		Private Shared Function FindCheckBox(ByVal fields As IEnumerable(Of PdfInteractiveFormField), ByVal partialName As String, ByVal fieldName As String) As PdfInteractiveFormField
			For Each field As PdfInteractiveFormField In fields
				If ((partialName + field.Name) _
										= fieldName) Then
					If (field.Flags.HasFlag(PdfInteractiveFormFieldFlags.Radio) _
											OrElse (field.Flags.HasFlag(PdfInteractiveFormFieldFlags.PushButton) _
											OrElse (field.Widget Is Nothing))) Then
						Return Nothing
					End If

					Return CType(field, PdfButtonFormField)
				End If

				If (Not (field.Kids) Is Nothing) Then
					Dim intermediateResult As PdfInteractiveFormField = Program.FindCheckBox(field.Kids, (partialName + ("." + field.Name)), fieldName)
					If (Not (intermediateResult) Is Nothing) Then
						Return intermediateResult
					End If

				End If

			Next
			Return Nothing
		End Function

		Private Shared Function GetCheckboxCheckedValue(ByVal processor As PdfDocumentProcessor, ByVal fieldName As String) As String
			If ((processor.Document Is Nothing) _
									OrElse (processor.Document.AcroForm Is Nothing)) Then
				Return Nothing
			End If

			Dim button As PdfInteractiveFormField = Program.FindCheckBox(processor.Document.AcroForm.Fields, "", fieldName)
			If (Not (button) Is Nothing) Then
				Dim appearance As PdfAnnotationAppearances = button.Widget.Appearance
				If (Not (appearance) Is Nothing) Then
					Dim names As IList(Of String) = appearance.Normal.Forms.Keys.ToList
					If (names.Count = 1) Then
						Return names.First
					End If

					If (names.Count > 1) Then
						Return names(1)
					End If

					'TODO: Warning!!!, inline IF is not supported ?
					names(0) = "Off"
					'names(0)
				End If

				Return Nothing
			End If

			Return Nothing
		End Function

		Private Shared Sub Main(ByVal args() As String)
			Dim processor As PdfDocumentProcessor = New PdfDocumentProcessor
			processor.LoadDocument("D:/form.pdf")
			Dim data As PdfFormData = processor.GetFormData
			data("A").Value = Program.GetCheckboxCheckedValue(processor, "A")
			processor.SaveDocument("D:/form1.pdf")
		End Sub

	End Class

End Namespace
