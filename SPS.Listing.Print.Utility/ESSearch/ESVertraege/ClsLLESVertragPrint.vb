
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath

Imports System.Data.SqlClient
Imports System.IO
Imports combit.ListLabel25

Imports SPS.Listing.Print.Utility.MainUtilities.Utilities
Imports SPS.Listing.Print.Utility.ESDbDatabases.ClsESDb4Print
Imports SPProgUtility.MainUtilities
Imports SPS.Listing.Print.Utility.ClsMainSetting

Imports SP.Infrastructure.Logging
Imports SP.DatabaseAccess.ES
Imports SP.DatabaseAccess.ES.DataObjects.ESMng
Imports SP.Infrastructure.UI


Public Class ClsLLESVertragPrint
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

	Private m_mandant As Mandant

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As Utilities

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	Private _ESVertragSetting As New ClsLLESVertragSetting

	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	Private _ClsReg As New SPProgUtility.ClsDivReg
	Private m_ProgPath As ClsProgPath
	Friend _ClsLLFunc As New ClsLLFunc

	Private m_connectionString As String = String.Empty

	Private m_MandantSettingsXml As SPProgUtility.CommonXmlUtility.SettingsXml
	Private m_MandantXMLFile As String
	Private m_MandantSetting As String
	Private m_SonstigesSetting As String
	Private m_InvoiceSetting As String
	Private m_MandantFormXMLFile As String
	Private m_ESUnterzeichner As Boolean

	Private m_ESDatabaseAccess As IESDatabaseAccess
	Private m_ESData As ESMasterData

	Private Property USSignFileName As String
	Private Property ExistsDocFile As Boolean

	Private LL As ListLabel = New ListLabel


#Region "private consts"

	Private Const MANDANT_XML_MAIN_KEY As String = "MD_{0}/"

	Private Const MANDANT_XML_SETTING_SPUTNIK_INVOICE_SETTING As String = "MD_{0}/Debitoren"
	Private Const MANDANT_XML_SETTING_SPUTNIK_SONSTIGES_SETTING As String = "MD_{0}/Sonstiges"

	Private Const MANDANT_XML_SETTING_SPUTNIK_AHV_SETTING As String = "MD_{0}/AHV-Daten"
	Private Const MANDANT_XML_SETTING_SPUTNIK_SUVA_SETTING As String = "MD_{0}/SUVA-Daten"
	Private Const MANDANT_XML_SETTING_SPUTNIK_FAK_SETTING As String = "MD_{0}/Fak-Daten"
	Private Const FORM_XML_DEFAULTVALUES_KEY As String = "Forms_Normaly/Field_DefaultValues"

#End Region


#Region "Private Properties"

	Private ReadOnly Property GetYearMwSt(ByVal iYear As Integer) As Decimal
		Get
			'Dim mwstsatz As Decimal = ParseToDec(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/mwstsatz", m_InvoiceSetting)), 8)
			Dim mdNumber = m_InitialData.MDData.MDNr
			If iYear = 0 Then iYear = Now.Year
			Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}/Debitoren", mdNumber)
			Dim mwstsatz As String = m_ProgPath.GetXMLNodeValue(m_mandant.GetSelectedMDDataXMLFilename(mdNumber, iYear), String.Format("{0}/mwstsatz", FORM_XML_MAIN_KEY))

			If String.IsNullOrWhiteSpace(mwstsatz) Then
				Return 8
			End If
			Return mwstsatz
		End Get
	End Property

	Private ReadOnly Property GetESEndeByNull() As String
		Get
			Dim esendebynull As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/esendebynull", FORM_XML_DEFAULTVALUES_KEY))
			Return If(String.IsNullOrWhiteSpace(esendebynull), "unbestimmt", esendebynull)
		End Get
	End Property

	Private ReadOnly Property GetESUnterzeichner() As Boolean
		Get
			Dim sValue As Boolean = True

			Dim strQuery As String = String.Format("//ExportSetting[@Name={0}SP.ES.PrintUtility{0}]/esunterzeichner_esvertrag", Chr(34))
			Dim strBez As String = _ClsProgSetting.GetXMLNodeValue(m_mandant.GetSelectedMDUserProfileXMLFilename(_ESVertragSetting.SelectedMDNr, _ESVertragSetting.LogedUSNr), strQuery)
			sValue = StrToBool(strBez)

			Return sValue
		End Get

	End Property

#End Region


#Region "Constructor"

	Public Sub New(ByVal _setting As ClsLLESVertragSetting)

		m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ClsMainSetting.TranslationData, ClsMainSetting.PerosonalizedData, ClsMainSetting.MDData, ClsMainSetting.UserData)
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

		m_Utility = New Utilities
		m_UtilityUI = New UtilityUI
		m_ProgPath = New ClsProgPath
		m_mandant = New Mandant


		Try
			Me._ESVertragSetting = _setting
			Me.m_connectionString = _ESVertragSetting.DbConnString2Open

			m_ESDatabaseAccess = New ESDatabaseAccess(m_connectionString, m_InitialData.UserData.UserLanguage)

			Dim recordMandantNumber As Integer = m_InitialData.MDData.MDNr
			LoadEmploymentData(_ESVertragSetting.SelectedESNr2Print)
			If Not m_ESData Is Nothing Then
				recordMandantNumber = m_ESData.MDNr
			End If
			'If Not (recordMandantNumber = m_InitialData.MDData.MDNr OrElse m_InitialData.MDData.MultiMD = 0) Then
			_ESVertragSetting.SelectedMDNr = recordMandantNumber
			'End If

			Me.ExistsDocFile = BuildPrintJob()
			m_MandantSetting = String.Format(MANDANT_XML_MAIN_KEY, _ESVertragSetting.SelectedMDNr)
			m_SonstigesSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_SONSTIGES_SETTING, _ESVertragSetting.SelectedMDNr)
			m_InvoiceSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_INVOICE_SETTING, _ESVertragSetting.SelectedMDNr)
			m_MandantFormXMLFile = m_mandant.GetSelectedMDFormDataXMLFilename(_ESVertragSetting.SelectedMDNr)

			m_MandantXMLFile = m_mandant.GetSelectedMDDataXMLFilename(_ESVertragSetting.SelectedMDNr, _ESVertragSetting.SelectedYear)
			If Not System.IO.File.Exists(m_MandantXMLFile) Then
				m_UtilityUI.ShowErrorDialog(String.Format("Die Einstellung-Datei wurde nicht gefunden.{0}{1}", vbNewLine, m_MandantXMLFile))

			Else
				m_MandantSettingsXml = New SPProgUtility.CommonXmlUtility.SettingsXml(m_MandantXMLFile)
			End If

			m_ESUnterzeichner = GetESUnterzeichner


		Catch ex As Exception
			m_Logger.LogError(String.Format("Öffnen der Daten von Einsatzlohndaten: {0}", ex.ToString))

		End Try

	End Sub

	Protected Overridable Overloads Sub Dispose(
		 ByVal disposing As Boolean)
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

	Function BuildPrintJob() As Boolean
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim sSql As String = "Select * From DokPrint Where [JobNr] = @JobNr"
		Dim Conn As SqlConnection = New SqlConnection(m_connectionString)
		Dim bResult As Boolean = True
		Dim JobNr As String = _ESVertragSetting.JobNr2Print

		If JobNr = String.Empty Then
			Dim strMessage As String = "Sie haben für JobNr {1} keine Vorlage ausgewählt.{0}Bitte wählen Sie aus der Liste eine Vorlage aus."
			m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, String.Format(strMessage, vbNewLine, JobNr)))
			MsgBox(String.Format(TranslateMyText(strMessage), vbNewLine, JobNr),
						MsgBoxStyle.Critical, TranslateMyText("Leere Vorlage"))
			Return False
		End If
		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
		Dim param As System.Data.SqlClient.SqlParameter

		Try
			Conn.Open()

			param = cmd.Parameters.AddWithValue("@JobNr", _ESVertragSetting.JobNr2Print)
			Dim rDocrec As SqlDataReader = cmd.ExecuteReader          ' Dokumentendatenbank
			rDocrec.Read()
			If Not String.IsNullOrEmpty(rDocrec("DocName").ToString) Then
				_ClsLLFunc.LLDocName = String.Format("{0}{1}{2}",
																						 m_mandant.GetSelectedMDDocPath(_ESVertragSetting.SelectedMDNr),
																						 If(_ESVertragSetting.SelectedLang <> String.Empty, _ESVertragSetting.SelectedLang & "\", ""),
																						 rDocrec("DocName").ToString)
				If Not File.Exists(_ClsLLFunc.LLDocName) Then
					_ClsLLFunc.LLDocName = String.Format("{0}{1}",
																							 m_mandant.GetSelectedMDDocPath(_ESVertragSetting.SelectedMDNr),
																							 rDocrec("DocName").ToString)
				End If
				_ClsLLFunc.LLDocLabel = rDocrec("Bezeichnung").ToString

				If String.IsNullOrEmpty(rDocrec("ParamCheck").ToString) Then
					_ClsLLFunc.LLParamCheck = 0
				Else
					_ClsLLFunc.LLParamCheck = CInt(IIf(CBool(rDocrec("ParamCheck")), 1, 0))
				End If

				If String.IsNullOrEmpty(rDocrec("KonvertName").ToString) Then
					_ClsLLFunc.LLKonvertName = 0
				Else
					_ClsLLFunc.LLKonvertName = CInt(IIf(CBool(rDocrec("KonvertName")), 1, 0))
				End If

				If String.IsNullOrEmpty(rDocrec("ZoomProz").ToString) Then
					_ClsLLFunc.LLParamCheck = 100
				Else
					_ClsLLFunc.LLZoomProz = CInt(IIf(CInt(rDocrec("ZoomProz")) = 0, 150, CInt(rDocrec("ZoomProz"))))
				End If

				If String.IsNullOrEmpty(rDocrec("Anzahlkopien").ToString) Then
					_ClsLLFunc.LLCopyCount = 1
				Else
					_ClsLLFunc.LLCopyCount = If(_ESVertragSetting.AnzahlCopies = 0, Math.Max(CByte(rDocrec("Anzahlkopien")), 1), _ESVertragSetting.AnzahlCopies)
				End If

				If String.IsNullOrEmpty((rDocrec("TempDocPath").ToString)) Then
					_ClsLLFunc.LLExportedFilePath = m_ProgPath.GetSpS2DeleteHomeFolder
				Else
					_ClsLLFunc.LLExportedFilePath = _ClsReg.AddDirSep(rDocrec("TempDocPath").ToString)
				End If

				If String.IsNullOrEmpty(rDocrec("ExportedFileName").ToString) Then
					_ClsLLFunc.LLExportedFileName = Path.GetFileNameWithoutExtension(rDocrec("DocName").ToString)
				Else
					_ClsLLFunc.LLExportedFileName = Path.GetFileNameWithoutExtension(rDocrec("ExportedFileName").ToString)
				End If


			Else
				_ClsLLFunc.LLDocName = String.Empty
				_ClsLLFunc.LLDocLabel = String.Empty
				_ClsLLFunc.LLParamCheck = 0
				_ClsLLFunc.LLKonvertName = 0

				_ClsLLFunc.LLParamCheck = 100
				_ClsLLFunc.LLCopyCount = 1
				_ClsLLFunc.LLExportedFilePath = m_ProgPath.GetPrinterHomeFolder
				_ClsLLFunc.LLExportedFileName = String.Empty
				bResult = False

			End If
			rDocrec.Close()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, ex.ToString))
			bResult = False

		Finally
			cmd.Dispose()
			Conn.Close()

		End Try

		Return bResult
	End Function

	Sub ShowInDesign()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Dim bAllowedtoDebug As Boolean = CBool(_ClsProgSetting.GetMDProfilValue("Sonstiges", "EnableLLDebug", "0"))
			If bAllowedtoDebug Then
				LL.Debug = LlDebug.LogToFile
				LL.Debug = LlDebug.Enabled
			End If
		Catch ex As Exception
			m_Logger.LogError(String.Format("EnablingLLDebuging.{0}: {1}", strMethodeName, ex))

		End Try

		If Not Me.ExistsDocFile Then Exit Sub

		Try
			Dim rFoundedrec As SqlDataReader = OpenDb4PrintESVertrag(_ESVertragSetting)

			If Not rFoundedrec.HasRows Then
				m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, "Keine Daten wurden gefunden..."))
				Exit Sub
			End If

			InitLL(LL)
			LL.Variables.Clear()
			LL.Fields.Clear()
			Me._ESVertragSetting.SelectedMonth = CDate(rFoundedrec("ES_Ab")).Month
			Me._ESVertragSetting.SelectedYear = CDate(rFoundedrec("ES_Ab")).Year
			SetLLVariable(rFoundedrec)
			ExcludeGAVString(rFoundedrec("GAVInfo_String"))
			LL.Variables.Add("WOSDoc", 1)

			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				DefineData(LL, True, rFoundedrec)
			Else
				DefineData(LL, False, rFoundedrec)
			End If

			LL.Core.LlDefineLayout(CType(0, IntPtr), _ClsLLFunc.LLDocLabel,
														 If(_ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper), LlProject.List, LlProject.Card),
														 _ClsLLFunc.LLDocName)

		Catch LlException As Exception
			m_Logger.LogError(String.Format("LlException. ESNr: {0} | MANr: {1}: {2}:{3}",
																 Me._ESVertragSetting.SelectedESNr2Print,
																 Me._ESVertragSetting.SelectedMANr2Print,
																 strMethodeName, LlException.Message))

		Finally

		End Try

	End Sub

	Function ShowInPrint(ByRef bShowBox As Boolean) As String
		Const LL_PRNOPT_COPIES_SUPPORTED As Integer = 3
		Dim strResult As String = "Success"
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim strJobNr As String = Me._ESVertragSetting.JobNr2Print

		Dim strQuery As String = _ESVertragSetting.SQL2Open
		Dim Conn As SqlConnection = New SqlConnection(m_connectionString)
		Dim i As Integer = 0

		If Not Me.ExistsDocFile Then Return strResult

		Try
			Dim bAllowedtoDebug As Boolean = CBool(_ClsProgSetting.GetMDProfilValue("Sonstiges", "EnableLLDebug", "0"))
			If bAllowedtoDebug Then
				LL.Debug = LlDebug.LogToFile
				LL.Debug = LlDebug.Enabled
			End If
		Catch ex As Exception
			m_Logger.LogError(String.Format("EnablingLLDebuging.{0}: {1}", strMethodeName, ex))

		End Try

		Try
			Dim rFoundedrec As SqlDataReader = OpenDb4PrintESVertrag(_ESVertragSetting)
			If Not rFoundedrec.HasRows Then Return TranslateMyText("Keine Daten wurden gefunden") & "..."

			InitLL(LL)
			LL.Variables.Clear()
			LL.Fields.Clear()
			Me._ESVertragSetting.SelectedMonth = CDate(rFoundedrec("ES_Ab")).Month
			Me._ESVertragSetting.SelectedYear = CDate(rFoundedrec("ES_Ab")).Year

			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				DefineData(LL, True, rFoundedrec)
			Else
				DefineData(LL, False, rFoundedrec)
			End If
			SetLLVariable(rFoundedrec)
			ExcludeGAVString(rFoundedrec("GAVInfo_String"))
			LL.Variables.Add("WOSDoc", 0)

			LL.Core.LlPrintWithBoxStart(If(_ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper),
																		 LlProject.List, LlProject.Card),
																		 _ClsLLFunc.LLDocName, LlPrintMode.Export, LlBoxType.StandardAbort,
																		 CType(0, IntPtr), _ClsLLFunc.LLDocLabel)

			LL.Core.LlPrintSetOption(LlPrintOption.Copies, _ClsLLFunc.LLCopyCount)
			LL.Core.LlPrintSetOption(LL_PRNOPT_COPIES_SUPPORTED, LL.Core.LlPrintGetOption(LlPrintOption.Copies))

			If bShowBox AndAlso Not Me._ESVertragSetting.Is4Export Then
				LL.Core.LlPrintOptionsDialog(CType(0, IntPtr), String.Format("{1}: {2}{0}{3}",
																																	 vbNewLine,
																																	 _ClsLLFunc.LLDocLabel,
																																	 strJobNr,
																																	 _ClsLLFunc.LLDocName))
			End If
			Dim TargetFormat As String = LL.Core.LlPrintGetOptionString(LlPrintOptionString.Export)

			While Not LL.Core.LlPrint()
			End While

			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				Do
					' pass data for current record
					DefineData(LL, True, rFoundedrec)

					While LL.Core.LlPrintFields() = LlConstants.WrnRepeatData
						LL.Core.LlPrint()
					End While

				Loop While rFoundedrec.Read()

				While LL.Core.LlPrintFieldsEnd() = LlConstants.WrnRepeatData
				End While
			End If
			Try
				LL.Core.LlPrintEnd(0)
			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.LlPrintEnd...:{1}", strMethodeName, ex.ToString))
			End Try


			If TargetFormat = "PRV" Then
				LL.Core.LlPreviewDisplay(_ClsLLFunc.LLDocName, m_ProgPath.GetSpSTempFolder, CType(0, IntPtr))
				LL.Core.LlPreviewDeleteFiles(_ClsLLFunc.LLDocName, m_ProgPath.GetSpSTempFolder)

				Return "Error"
			End If

			Try
				CType(LL, IDisposable).Dispose()

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.LL Disposing...:{1}", strMethodeName, ex.ToString))

			End Try


		Catch LlException As LL_User_Aborted_Exception
			Return "Error"

		Catch LlException As Exception
			m_Logger.LogError(String.Format("LlException. ESNr: {0} | MANr: {1}: {2}:{3}",
																 Me._ESVertragSetting.SelectedESNr2Print,
																 Me._ESVertragSetting.SelectedMANr2Print,
																 strMethodeName, LlException.Message))
			strResult = String.Format("Error. ESNr: {0} | MANr: {1}: {2}:{3}",
																 Me._ESVertragSetting.SelectedESNr2Print,
																 Me._ESVertragSetting.SelectedMANr2Print,
																 strMethodeName, LlException.Message)

		Finally
			CType(LL, IDisposable).Dispose()

		End Try

		Return strResult
	End Function

	Function ExportLLDoc() As Boolean
		Dim strResult As Boolean = True
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim strJobNr As String = Me._ESVertragSetting.JobNr2Print

		Dim strQuery As String = _ESVertragSetting.SQL2Open
		Dim Conn As SqlConnection = New SqlConnection(m_connectionString)
		Dim i As Integer = 0

		If Not Me.ExistsDocFile Then Return False

		Try
			Dim bAllowedtoDebug As Boolean = CBool(_ClsProgSetting.GetMDProfilValue("Sonstiges", "EnableLLDebug", "0"))
			If bAllowedtoDebug Then
				LL.Debug = LlDebug.LogToFile
				LL.Debug = LlDebug.Enabled
			End If
		Catch ex As Exception
			m_Logger.LogError(String.Format("EnablingLLDebuging.{0}: {1}", strMethodeName, ex))

		End Try

		Try
			Dim rFoundedrec As SqlDataReader = OpenDb4PrintESVertrag(_ESVertragSetting)
			If Not rFoundedrec.HasRows Then Return TranslateMyText("Keine Daten wurden gefunden") & "..."

			InitLL(LL)
			LL.Variables.Clear()
			LL.Fields.Clear()
			Me._ESVertragSetting.SelectedMonth = CDate(rFoundedrec("ES_Ab")).Month
			Me._ESVertragSetting.SelectedYear = CDate(rFoundedrec("ES_Ab")).Year

			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				DefineData(LL, True, rFoundedrec)
			Else
				DefineData(LL, False, rFoundedrec)
			End If
			SetLLVariable(rFoundedrec)
			ExcludeGAVString(rFoundedrec("GAVInfo_String"))
			LL.Variables.Add("WOSDoc", 1)

			LL.ExportOptions.Clear()
			SetExportSetting(0)

			LL.Core.LlPrintWithBoxStart(If(_ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper), LlProject.List, LlProject.Card),
																	_ClsLLFunc.LLDocName, LlPrintMode.Export, LlBoxType.StandardAbort, CType(0, IntPtr),
																	_ClsLLFunc.LLDocLabel)

			'Dim strExportPfad As String = Me._ESVertragSetting.ExportPath
			'If Not Directory.Exists(strExportPfad) Then strExportPfad = _ClsProgSetting.GetSpSESTempPath

			'Dim strExportFilename As String = Me._ESVertragSetting.GetExportFilename(Me._ESVertragSetting.IsPrintAsVerleih)
			'strExportFilename = String.Format(strExportFilename, Me._ESVertragSetting.SelectedESNr2Print, Me._ESVertragSetting.SelectedMANr2Print)



			Dim strExportPfad As String = m_InitializationData.UserData.spTempEmplymentPath
			If Not Directory.Exists(strExportPfad) Then Directory.CreateDirectory(m_InitializationData.UserData.spTempEmplymentPath)

			Dim strExportFilename As String
			Dim initialFilename As String = "Einsatzvertrag_"

			If _ESVertragSetting.IsPrintAsVerleih Then initialFilename = "Verleihvertrag_"

			strExportFilename = String.Format("{0}{1}.PDF", initialFilename, _ESVertragSetting.SelectedESNr2Print)

			If File.Exists(Path.Combine(strExportPfad, strExportFilename)) Then
				Try
					File.Delete(Path.Combine(strExportPfad, strExportFilename))
				Catch ex As Exception
					strExportFilename = String.Format("{0}{1}_{2}.PDF", initialFilename, _ESVertragSetting.SelectedESNr2Print, Environment.TickCount)
				End Try
			End If

			LL.Core.LlPrintSetOptionString(LlPrintOptionString.Export, _ClsLLFunc.LLExporterName)
			LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.File", strExportFilename)
			LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.Path", strExportPfad)
			LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.Quiet", "1")

			LL.Core.LlPrintSetOption(LlPrintOption.Dialog_DestinationMask, 2)
			Dim TargetFormat As String = LL.Core.LlPrintGetOptionString(LlPrintOptionString.Export)

			'LL_Test.Core.LlPrint()

			While Not LL.Core.LlPrint()
			End While

			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				Do
					' pass data for current record
					DefineData(Me.LL, True, rFoundedrec)

					While LL.Core.LlPrintFields() = LlConstants.WrnRepeatData
						LL.Core.LlPrint()
					End While

				Loop While rFoundedrec.Read()

				While LL.Core.LlPrintFieldsEnd() = LlConstants.WrnRepeatData
				End While
			End If

			Try
				LL.Core.LlPrintEnd(0)

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.LLPrintEnd(0)...:{1}", strMethodeName, ex.ToString))

				Return False
			End Try

			Me._ESVertragSetting.ListOfExportedFilesESVertrag.Add(Path.Combine(strExportPfad, strExportFilename))

			If TargetFormat = "PRV" Then
				LL.Core.LlPreviewDisplay(_ClsLLFunc.LLDocName, m_ProgPath.GetSpSTempFolder, CType(0, IntPtr))
				LL.Core.LlPreviewDeleteFiles(_ClsLLFunc.LLDocName, m_ProgPath.GetSpSTempFolder)

				Return False
			End If

			Try
				CType(LL, IDisposable).Dispose()

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.LL Disposing...:{1}", strMethodeName, ex.ToString))
				Return False
			End Try

		Catch LlException As Exception
			m_Logger.LogError(String.Format("LlException. ESNr: {0} | MANr: {1}: {2}:{3}",
																 Me._ESVertragSetting.SelectedESNr2Print,
																 Me._ESVertragSetting.SelectedMANr2Print,
																 strMethodeName, LlException.Message))
			Return False

		Finally
			CType(LL, IDisposable).Dispose()

		End Try

		Return strResult
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

	Sub DefineData(ByVal LL As ListLabel, ByVal AsFields As Boolean, ByVal MyDataReader As SqlDataReader)
		Dim i As Integer

		Dim iType As String
		Dim strParam As LlFieldType
		Dim strContent As String

		For i = 0 To MyDataReader.FieldCount - 1
			iType = MyDataReader.GetFieldType(i).ToString.ToUpper

			Select Case iType.ToUpper

				Case "System.Int16".ToUpper, "System.Int32".ToUpper, "System.Int64".ToUpper, "System.Decimal".ToUpper, "System.Double".ToUpper
					strParam = LlFieldType.Numeric
					strContent = MyDataReader.Item(i).ToString & ""

				Case "System.DateTime".ToUpper
					strParam = LlFieldType.Date_OLE

					If IsDBNull(MyDataReader.Item(i)) Then
						strContent = CDate("01.01.1900").ToOADate().ToString() ' , "G")
					Else
						strContent = MyDataReader.GetDateTime(i).ToOADate().ToString()
					End If

				Case "System.Boolean".ToUpper
					strParam = LlFieldType.Boolean
					If IsDBNull(MyDataReader.Item(i)) Then
						strContent = "0"
					Else
						strContent = CStr(IIf(CBool(MyDataReader.Item(i)), 1, 0))
					End If

				Case Else
					strParam = LlFieldType.Text
					If IsDBNull(MyDataReader.Item(i)) Then
						strContent = String.Empty
					Else
						strContent = MyDataReader.Item(i).ToString
					End If

			End Select

			LL.Core.LlDefineFieldExt(MyDataReader.GetName(i), strContent, strParam)
			LL.Core.LlDefineVariableExt(MyDataReader.GetName(i), strContent, strParam)
		Next

	End Sub

	Sub GetMDUSData4Print(ByVal LL As ListLabel)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		LoadEmploymentData(_ESVertragSetting.DbConnString2Open, _ClsLLFunc, _ESVertragSetting.SelectedESNr2Print)
		Try
			With _ClsLLFunc
				LL.Variables.Add("MDName", .USMDname)
				LL.Variables.Add("MDName2", .USMDname2)
				LL.Variables.Add("MDName3", .USMDname3)
				LL.Variables.Add("MDPostfach", .USMDPostfach)
				LL.Variables.Add("MDStrasse", .USMDStrasse)
				LL.Variables.Add("MDPLZ", .USMDPlz)
				LL.Variables.Add("MDOrt", .USMDOrt)
				LL.Variables.Add("MDLand", .USMDLand)

				LL.Variables.Add("USOrt", .USOrt)
				LL.Variables.Add("USPLZ", .USPLZ)
				LL.Variables.Add("USStrasse", .USStrasse)


				LL.Variables.Add("MDTelefax", .USMDTelefax)
				LL.Variables.Add("MDTelefon", .USMDTelefon)
				LL.Variables.Add("MDDTelefon", .USMDDTelefon)
				LL.Variables.Add("MDHomepage", .USMDHomepage)
				LL.Variables.Add("MDeMail", .USMDeMail)

				LL.Variables.Add("USNachName", .USNachname)
				LL.Variables.Add("USVorname", .USVorname)

				LL.Variables.Add("USTitle1", .USTitel_1)
				LL.Variables.Add("USTitle2", .USTitel_2)
				LL.Variables.Add("USAbteilung", .USAbteilung)

				' Bewilligungsbehörden
				Dim BewName As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/BewName", m_SonstigesSetting))
				Dim BewPostfach As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/BewPostfach", m_SonstigesSetting))
				Dim BewStrasse As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/BewStrasse", m_SonstigesSetting))
				Dim BewPLZOrt As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/BewPLZOrt", m_SonstigesSetting))
				Dim BewSeco As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/BewSeco", m_SonstigesSetting))

				Dim mwstnr As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/mwstnr", m_InvoiceSetting))
				Dim mwstsatz As Decimal = ParseToDec(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/mwstsatz", m_InvoiceSetting)), 8)


				LL.Variables.Add("BewName", BewName) ' _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, "Sonstiges", "BewName"))
				LL.Variables.Add("BewStr", BewStrasse) ' _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, "Sonstiges", "BewStrasse"))
				LL.Variables.Add("BewOrt", BewPLZOrt) ' _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, "Sonstiges", "BewPLZOrt"))
				LL.Variables.Add("BewNameAus", BewSeco) ' _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, "Sonstiges", "BewSeco"))
				LL.Variables.Add("MwStProzent", mwstsatz)

			End With

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Private Sub LoadEmploymentData(ByVal strConnString As String, ByVal _ClsLLFunc As ClsLLFunc, ByVal recordNumber As Integer)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim m_path As New SPProgUtility.ProgPath.ClsProgPath
		Dim strUSKst As String = String.Empty
		Dim strUSNachname As String = String.Empty
		Dim iUSNr As Integer = 0
		Dim recordMandantNumber As Integer = m_InitialData.MDData.MDNr
		Dim Conn As New SqlConnection(strConnString)
		Conn.Open()

		If Not m_ESData Is Nothing Then
			recordMandantNumber = m_ESData.MDNr
		End If

		Dim sSql As String = "[Get USData 4 Templates With MDNumber And USNumber]"
		Dim cmd As SqlCommand = New SqlCommand(sSql, Conn)
		cmd.CommandType = CommandType.StoredProcedure
		Dim param As System.Data.SqlClient.SqlParameter
		param = cmd.Parameters.AddWithValue("@USNr", ClsMainSetting.UserData.UserNr)
		'param = cmd.Parameters.AddWithValue("@USVorname", ClsMainSetting.UserData.UserFName)
		param = cmd.Parameters.AddWithValue("@MDNr", recordMandantNumber)

		Dim rTemprec As SqlDataReader = cmd.ExecuteReader

		Try
			rTemprec.Read()
			With _ClsLLFunc
				.iSelectedUSNr = m_path.SafeGetInteger(rTemprec, "USNr", 0)

				.USAnrede = m_path.SafeGetString(rTemprec, "USAnrede")
				.USeMail = m_path.SafeGetString(rTemprec, "USeMail")
				.USNachname = m_path.SafeGetString(rTemprec, "USNachname")
				.USVorname = m_path.SafeGetString(rTemprec, "USVorname")
				.USTelefon = m_path.SafeGetString(rTemprec, "USTelefon")
				.USTelefax = m_path.SafeGetString(rTemprec, "USTelefax")
				.USNatel = m_path.SafeGetString(rTemprec, "USNatel")

				.USTitel_1 = m_path.SafeGetString(rTemprec, "USTitel_1")
				.USTitel_2 = m_path.SafeGetString(rTemprec, "USTitel_2")

				.USAbteilung = m_path.SafeGetString(rTemprec, "USAbteilung")
				.USPostfach = m_path.SafeGetString(rTemprec, "USPostfach")
				.USStrasse = m_path.SafeGetString(rTemprec, "USStrasse")
				.USPLZ = m_path.SafeGetString(rTemprec, "USPLZ")
				.USLand = m_path.SafeGetString(rTemprec, "USLand")
				.USOrt = m_path.SafeGetString(rTemprec, "USOrt")

				.Exchange_USName = m_path.SafeGetString(rTemprec, "EMail_UserName")
				.Exchange_USPW = m_path.SafeGetString(rTemprec, "EMail_UserPW")

				.USMDname = m_path.SafeGetString(rTemprec, "MDName")
				.USMDname2 = m_path.SafeGetString(rTemprec, "MDName2")
				.USMDname3 = m_path.SafeGetString(rTemprec, "MDName3")
				.USMDPostfach = m_path.SafeGetString(rTemprec, "MDPostfach")
				.USMDStrasse = m_path.SafeGetString(rTemprec, "MDStrasse")
				.USMDPlz = m_path.SafeGetString(rTemprec, "MDPLZ")
				.USMDOrt = m_path.SafeGetString(rTemprec, "MDOrt")
				.USMDLand = m_path.SafeGetString(rTemprec, "MDLand")

				.USMDTelefon = m_path.SafeGetString(rTemprec, "MDTelefon")
				.USMDDTelefon = m_path.SafeGetString(rTemprec, "MDDTelefon")
				.USMDTelefax = m_path.SafeGetString(rTemprec, "MDTelefax")
				.USMDeMail = m_path.SafeGetString(rTemprec, "MDeMail")
				.USMDHomepage = m_path.SafeGetString(rTemprec, "MDHomepage")

			End With
			rTemprec.Close()


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

		Finally
			rTemprec.Close()
			Conn.Close()

		End Try

	End Sub

	Private Function LoadEmploymentData(ByVal esNumber As Integer) As Boolean
		Dim result As Boolean = True

		m_ESData = m_ESDatabaseAccess.LoadESMasterData(esNumber)

		If m_ESData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Einsatz-Daten für Mandant-Spezifikationen konnten nicht geladen werden."))
		End If


		Return (Not m_ESData Is Nothing)

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
		LL.Core.LlSetOption(LL_OPTION_INCLUDEFONTDESCENT, _ClsLLFunc.LLFontDesent)
		LL.Core.LlSetOption(LL_OPTION_INCREMENTAL_PREVIEW, _ClsLLFunc.LLIncPrv)

		LL.Core.LlSetOption(LL_OPTION_VARSCASESENSITIVE, 0)

		LL.Core.LlSetOption(LL_OPTION_IMMEDIATELASTPAGE, 1)       ' Lastpage
		LL.Core.LlSetOption(LL_OPTION_CONVERTCRLF, 1)             ' Doppelte Zeilenumbruch

		LL.Core.LlSetOption(LL_OPTION_NOPARAMETERCHECK, _ClsLLFunc.LLParamCheck)
		LL.Core.LlSetOption(LL_OPTION_XLATVARNAMES, _ClsLLFunc.LLKonvertName)

		LL.Core.LlSetOption(LL_OPTION_ESC_CLOSES_PREVIEW, 1)
		LL.Core.LlSetOption(LL_OPTION_SUPERVISOR, 1)
		LL.Core.LlSetOption(LL_OPTION_UISTYLE, LL_OPTION_UISTYLE_OFFICE2003)
		LL.Core.LlSetOption(LL_OPTION_AUTOMULTIPAGE, 1)

		LL.Core.LlSetOption(LL_OPTION_SUPPORTPAGEBREAK, 1)
		LL.Core.LlSetOption(LL_OPTION_PRVZOOM_PERC, _ClsLLFunc.LLZoomProz)

		LL.Core.LlPreviewSetTempPath(m_ProgPath.GetSpSTempFolder)
		LL.Core.LlSetPrinterDefaultsDir(m_ProgPath.GetPrinterHomeFolder)

	End Sub

	Sub SetLLVariable(ByVal rFrec As SqlDataReader)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim aValue As New List(Of String)
		Dim dESLoVon As Date

		Try
			' Mandantendaten drucken...
			GetMDUSData4Print(LL)
			' Zusätzliche Variable einfügen
			LL.Variables.Add("AutorFName", _ClsProgSetting.GetUserFName)
			LL.Variables.Add("AutorLName", _ClsProgSetting.GetUserLName)
			LL.Variables.Add("USSignFilename", Me._ESVertragSetting.USSignFileName)


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}. US-Variable auflisten. {1}", strMethodeName, ex.ToString))

		End Try

		With rFrec
			Try
				Dim iKDZustaendigNr As Integer = m_Utility.SafeGetInteger(rFrec, "KDZHDNr", 0)
				Dim KDPostfach As String = m_Utility.SafeGetString(rFrec, "KDPostfach")
				Dim KDStrasse As String = m_Utility.SafeGetString(rFrec, "KDStrasse")
				Dim KDPLZOrt As String = m_Utility.SafeGetString(rFrec, "KDPLZOrt")

				Dim aKst As String() = m_Utility.SafeGetString(rFrec, "ESKST").ToString.Split(CChar("/"))
				Dim strKst_1 As String = aKst(0)
				Dim strKst_2 As String = aKst(0)
				If aKst.Length > 1 Then strKst_2 = aKst(1).Trim

				Dim KDzAnredeForm As String = m_Utility.SafeGetString(rFrec, "KDzAnredeForm")
				Dim KDzAnrede As String = m_Utility.SafeGetString(rFrec, "KDzAnrede")
				Dim KDzAnrede_Prog As String = m_Utility.SafeGetString(rFrec, "KDzAnrede")
				Dim KDZFName As String = m_Utility.SafeGetString(rFrec, "KDzVorname")
				Dim KDZLName As String = m_Utility.SafeGetString(rFrec, "KDzNachname")

				Dim KDzPostfach = m_Utility.SafeGetString(rFrec, "KDzPostfach")
				Dim KDzStrasse As String = m_Utility.SafeGetString(rFrec, "KDzStrasse")
				Dim KDzPLZOrt As String = String.Format("{0} {1}", m_Utility.SafeGetString(rFrec, "KDzPLZ"), m_Utility.SafeGetString(rFrec, "KDzOrt"))
				Dim strES_EndeDate As String
				Dim esEndeDateWithGoeslonger As String = String.Empty
				Dim isESEndeNull As Boolean
				Dim employeeLanguage = m_Utility.SafeGetString(rFrec, "MASprache")
				If String.IsNullOrWhiteSpace(employeeLanguage) Then employeeLanguage = "D"

				If m_Utility.SafeGetString(rFrec, "ES_Ende") = String.Empty Then
					strES_EndeDate = GetESEndeByNull
					esEndeDateWithGoeslonger = m_Utility.SafeGetString(rFrec, "GoesLonger")
					strES_EndeDate = GetSafeTranslationValue(strES_EndeDate, employeeLanguage.Substring(0, 1).ToUpper)
					isESEndeNull = True

				Else
					strES_EndeDate = String.Format(rFrec("ES_Ende"), "d")
					isESEndeNull = False

				End If
				If String.IsNullOrWhiteSpace(esEndeDateWithGoeslonger) Then esEndeDateWithGoeslonger = strES_EndeDate

				LL.Variables.Add("KDPostfach", KDPostfach)
				LL.Variables.Add("KDStrasse", KDStrasse)
				LL.Variables.Add("KDPLZOrt", KDPLZOrt)

				LL.Variables.Add("ESCreatedOn", m_Utility.SafeGetDateTime(rFrec, "CreatedOn", Nothing))
				LL.Variables.Add("ESCreatedFrom", m_Utility.SafeGetString(rFrec, "Createdfrom"))
				LL.Variables.Add("ESChangedOn", m_Utility.SafeGetDateTime(rFrec, "CreatedOn", Nothing))
				LL.Variables.Add("ESChangedFrom", m_Utility.SafeGetString(rFrec, "Changedfrom"))

				LL.Variables.Add("KDzFName", KDZFName)

				LL.Variables.Add("KDZNr", iKDZustaendigNr)
				If KDzAnrede_Prog = String.Empty Then
					KDzAnrede_Prog = "Sehr geehrter Kunde"
				Else

					KDzAnrede_Prog = String.Format("Sehr geehrte{0}", If(KDzAnrede = "Herr", "r", ""))
					'KDzAnrede_Prog = String.Format(m_Translate.GetSafeTranslationValue(String.Format("Sehr geehrte{0}", If(KDzAnrede = "Herr", "r", "")))) '& " {0}", KDZLName)

				End If

				LL.Variables.Add("KDzAnrede", KDzAnrede)
				LL.Variables.Add("KDzAnrede_Prog", GetSafeTranslationValue(KDzAnrede_Prog, employeeLanguage.Substring(0, 1).ToUpper))
				LL.Variables.Add("KDzAnredeForm", KDzAnredeForm)
				LL.Variables.Add("KDzFName", KDZFName)
				LL.Variables.Add("KDzLName", KDZLName)
				LL.Variables.Add("KDzPostfach", KDzPostfach)
				LL.Variables.Add("KDzStrasse", KDzStrasse)
				LL.Variables.Add("KDzPLZOrt", KDzPLZOrt)

				LL.Variables.Add("ES_EndeDate", strES_EndeDate)   ' "unbestimmt"
				LL.Variables.Add("esEndeDateWithGoeslonger", esEndeDateWithGoeslonger)
				LL.Variables.Add("isESEndeNull", isESEndeNull, LlFieldType.Boolean)

				Dim dESBegin As Date = m_Utility.SafeGetDateTime(rFrec, "ES_Ab", Nothing)
				dESLoVon = m_Utility.SafeGetDateTime(rFrec, "LOVon", Nothing)

				LL.Variables.Add("MwSt_Satz", GetYearMwSt(Year(dESLoVon)))

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}. Normale Variable auflisten. {1}", strMethodeName, ex.ToString))

			End Try

			' Zusätzliche Lohnarten in Einsatzverwaltung
			GetESLohnData4Print()

			' Zusätzliche Lohnarten in ES_ZLohn
			GetES_ZLohnDataArt0_4Print()

			' Zusätzliche Lohnarten in ES_ZLohn
			GetES_ZLohnDataArt1_4Print()

			' Zusätzliche Lohnarten in ES_ZLohn
			GetES_ZLohnDataArt2_4Print()

			' Benutzerdaten wegen angemeldeten Benutzer und Unterzeichner
			GetUSData4ESVertrag()

			' Benutzerunterschrift wegen angemeldeten Benutzer oder Unterzeichner
			Test_GetUSSign4ESVertrag()

		End With

		Try

			Dim rFoundedrec As SqlDataReader = OpenMDDb4PrintESVertrag(_ESVertragSetting, dESLoVon.Year)
			If Not rFoundedrec.HasRows Then
				Throw New Exception("Keine Mandanteninformationen wurden gefunden.")

			End If
			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				DefineData(LL, True, rFoundedrec)
			Else
				DefineData(LL, False, rFoundedrec)
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}. Mandanteninformationen auflisten. {1}", strMethodeName, ex.ToString))

		End Try

	End Sub


	Sub GetES_ZLohnDataArt0_4Print()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try

			For i As Short = 1 To 4
				LL.Variables.Add(String.Format("S_ESZLANr{0}", i), 0)
				LL.Variables.Add(String.Format("S_ESZLABez{0}", i), "")

				LL.Variables.Add(String.Format("S_Art_{0}", i), 0)
				LL.Variables.Add(String.Format("S_ESZLABetrag_G_{0}", i), 0)
				LL.Variables.Add(String.Format("S_ESZLABetrag_StdLohn_{0}", i), 0)

				LL.Variables.Add(String.Format("S_ESZLABetrag_Feier_{0}", i), 0)
				LL.Variables.Add(String.Format("S_ESZLABetrag_FeierProz_{0}", i), 0)

				LL.Variables.Add(String.Format("S_ESZLABetrag_Fer_{0}", i), 0)
				LL.Variables.Add(String.Format("S_ESZLABetrag_FerProz_{0}", i), 0)

				LL.Variables.Add(String.Format("S_ESZLABetrag_13_{0}", i), 0)
				LL.Variables.Add(String.Format("S_ESZLABetrag_13Proz_{0}", i), 0)

			Next i

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Daten vorbelegen.{1}", strMethodeName, ex.ToString))

		End Try

		Try
			Dim Conn As New SqlConnection(m_connectionString)
			Dim sSql As String = "[Get ESZLohnData For Print Vertrag Art]"
			Conn.Open()

			Dim cmd As SqlCommand = New SqlCommand(sSql, Conn)
			cmd.CommandType = CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@ESNr", _ESVertragSetting.SelectedESNr2Print)
			param = cmd.Parameters.AddWithValue("@Art", 0)
			param = cmd.Parameters.AddWithValue("@Year", Now.Year)
			Dim rTemprec As SqlDataReader = cmd.ExecuteReader

			Dim i As Integer = 1
			Try
				While rTemprec.Read
					LL.Variables.Add(String.Format("S_ESZLANr{0}", i), rTemprec("LANr"))
					LL.Variables.Add(String.Format("S_ESZLABez{0}", i), rTemprec("LABez"))

					LL.Variables.Add(String.Format("S_Art_{0}", i), rTemprec("Art"))
					LL.Variables.Add(String.Format("S_ESZLABetrag_G_{0}", i), rTemprec("Grundlohn"))
					LL.Variables.Add(String.Format("S_ESZLABetrag_StdLohn_{0}", i), rTemprec("Stundenlohn"))

					LL.Variables.Add(String.Format("S_ESZLABetrag_Feier_{0}", i), rTemprec("Feier"))
					LL.Variables.Add(String.Format("S_ESZLABetrag_FeierProz_{0}", i), rTemprec("FeierProz"))

					LL.Variables.Add(String.Format("S_ESZLABetrag_Fer_{0}", i), rTemprec("Ferien"))
					LL.Variables.Add(String.Format("S_ESZLABetrag_FerProz_{0}", i), rTemprec("FerienProz"))

					LL.Variables.Add(String.Format("S_ESZLABetrag_13_{0}", i), rTemprec("Lohn13"))
					LL.Variables.Add(String.Format("S_ESZLABetrag_13Proz_{0}", i), rTemprec("Lohn13Proz"))

					i += 1
				End While

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Daten lesen.{1}", strMethodeName, ex.ToString))

			End Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Sub GetES_ZLohnDataArt1_4Print()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			For i As Short = 5 To 8
				LL.Variables.Add(String.Format("W_ESZLANr{0}", i), 0)
				LL.Variables.Add(String.Format("W_ESZLABez{0}", i), "")

				LL.Variables.Add(String.Format("W_Art_{0}", i), 0)
				LL.Variables.Add(String.Format("W_ESZLABetrag_G_{0}", i), 0)
				LL.Variables.Add(String.Format("W_ESZLABetrag_StdLohn_{0}", i), 0)

				LL.Variables.Add(String.Format("W_ESZLABetrag_Feier_{0}", i), 0)
				LL.Variables.Add(String.Format("W_ESZLABetrag_FeierProz_{0}", i), 0)

				LL.Variables.Add(String.Format("W_ESZLABetrag_Fer_{0}", i), 0)
				LL.Variables.Add(String.Format("W_ESZLABetrag_FerProz_{0}", i), 0)

				LL.Variables.Add(String.Format("W_ESZLABetrag_13_{0}", i), 0)
				LL.Variables.Add(String.Format("W_ESZLABetrag_13Proz_{0}", i), 0)

			Next i

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Daten vorbelegen.{1}", strMethodeName, ex.ToString))

		End Try

		Try
			Dim Conn As New SqlConnection(m_connectionString)
			Dim sSql As String = "[Get ESZLohnData For Print Vertrag Art]"
			Conn.Open()

			Dim cmd As SqlCommand = New SqlCommand(sSql, Conn)
			cmd.CommandType = CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@ESNr", _ESVertragSetting.SelectedESNr2Print)
			param = cmd.Parameters.AddWithValue("@Art", 1)
			param = cmd.Parameters.AddWithValue("@Year", Now.Year)
			Dim rTemprec As SqlDataReader = cmd.ExecuteReader

			Dim i As Integer = 5
			Try
				While rTemprec.Read
					LL.Variables.Add(String.Format("W_ESZLANr{0}", i), rTemprec("LANr"))
					LL.Variables.Add(String.Format("W_ESZLABez{0}", i), rTemprec("LABez"))

					LL.Variables.Add(String.Format("W_Art_{0}", i), rTemprec("Art"))
					LL.Variables.Add(String.Format("W_ESZLABetrag_G_{0}", i), rTemprec("Grundlohn"))
					LL.Variables.Add(String.Format("W_ESZLABetrag_StdLohn_{0}", i), rTemprec("Stundenlohn"))

					LL.Variables.Add(String.Format("W_ESZLABetrag_Feier_{0}", i), rTemprec("Feier"))
					LL.Variables.Add(String.Format("W_ESZLABetrag_FeierProz_{0}", i), rTemprec("FeierProz"))

					LL.Variables.Add(String.Format("W_ESZLABetrag_Fer_{0}", i), rTemprec("Ferien"))
					LL.Variables.Add(String.Format("W_ESZLABetrag_FerProz_{0}", i), rTemprec("FerienProz"))

					LL.Variables.Add(String.Format("W_ESZLABetrag_13_{0}", i), rTemprec("Lohn13"))
					LL.Variables.Add(String.Format("W_ESZLABetrag_13Proz_{0}", i), rTemprec("Lohn13Proz"))

					i += 1
				End While

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Daten lesen.{1}", strMethodeName, ex.ToString))

			End Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Sub GetES_ZLohnDataArt2_4Print()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			For i As Short = 9 To 12
				LL.Variables.Add(String.Format("M_ESZLANr{0}", i), 0)
				LL.Variables.Add(String.Format("M_ESZLABez{0}", i), "")

				LL.Variables.Add(String.Format("M_Art_{0}", i), 0)
				LL.Variables.Add(String.Format("M_ESZLABetrag_G_{0}", i), 0)
				LL.Variables.Add(String.Format("M_ESZLABetrag_StdLohn_{0}", i), 0)

				LL.Variables.Add(String.Format("M_ESZLABetrag_Feier_{0}", i), 0)
				LL.Variables.Add(String.Format("M_ESZLABetrag_FeierProz_{0}", i), 0)

				LL.Variables.Add(String.Format("M_ESZLABetrag_Fer_{0}", i), 0)
				LL.Variables.Add(String.Format("M_ESZLABetrag_FerProz_{0}", i), 0)

				LL.Variables.Add(String.Format("M_ESZLABetrag_13_{0}", i), 0)
				LL.Variables.Add(String.Format("M_ESZLABetrag_13Proz_{0}", i), 0)

			Next i

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Daten vorbelegen.{1}", strMethodeName, ex.ToString))

		End Try

		Try
			Dim Conn As New SqlConnection(m_connectionString)
			Dim sSql As String = "[Get ESZLohnData For Print Vertrag Art]"
			Conn.Open()

			Dim cmd As SqlCommand = New SqlCommand(sSql, Conn)
			cmd.CommandType = CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@ESNr", _ESVertragSetting.SelectedESNr2Print)
			param = cmd.Parameters.AddWithValue("@Art", 2)
			param = cmd.Parameters.AddWithValue("@Year", Now.Year)
			Dim rTemprec As SqlDataReader = cmd.ExecuteReader

			Dim i As Integer = 9
			Try
				While rTemprec.Read
					LL.Variables.Add(String.Format("M_ESZLANr{0}", i), rTemprec("LANr"))
					LL.Variables.Add(String.Format("M_ESZLABez{0}", i), rTemprec("LABez"))

					LL.Variables.Add(String.Format("M_Art_{0}", i), rTemprec("Art"))
					LL.Variables.Add(String.Format("M_ESZLABetrag_G_{0}", i), rTemprec("Grundlohn"))
					LL.Variables.Add(String.Format("M_ESZLABetrag_StdLohn_{0}", i), rTemprec("Stundenlohn"))

					LL.Variables.Add(String.Format("M_ESZLABetrag_Feier_{0}", i), rTemprec("Feier"))
					LL.Variables.Add(String.Format("M_ESZLABetrag_FeierProz_{0}", i), rTemprec("FeierProz"))

					LL.Variables.Add(String.Format("M_ESZLABetrag_Fer_{0}", i), rTemprec("Ferien"))
					LL.Variables.Add(String.Format("M_ESZLABetrag_FerProz_{0}", i), rTemprec("FerienProz"))

					LL.Variables.Add(String.Format("M_ESZLABetrag_13_{0}", i), rTemprec("Lohn13"))
					LL.Variables.Add(String.Format("M_ESZLABetrag_13Proz_{0}", i), rTemprec("Lohn13Proz"))

					i += 1
				End While

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Daten lesen.{1}", strMethodeName, ex.ToString))

			End Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Sub GetESLohnData4Print()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Dim Conn As New SqlConnection(m_connectionString)
			Conn.Open()
			Dim sSql As String = String.Empty

			If Me._ESVertragSetting.IsPrintAsVerleih Then
				sSql = "[Get ESLAData For Print In Verleih]"
			Else
				sSql = "[Get ESLAData For Print In Einsatzvertrag]"
			End If

			Dim cmd As SqlCommand = New SqlCommand(sSql, Conn)
			cmd.CommandType = CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@ESNr", _ESVertragSetting.SelectedESNr2Print)
			param = cmd.Parameters.AddWithValue("@ESLohnNr", _ESVertragSetting.SelectedESLohnNr2Print)
			Dim rTemprec As SqlDataReader = cmd.ExecuteReader
			Dim i As Integer = 1

			Try
				For i = 1 To 20
					LL.Variables.Add(String.Format("ESLANr{0}", i), 0)
					LL.Variables.Add(String.Format("ESLABez{0}", i), String.Empty)
					LL.Variables.Add(String.Format("ESLABetrag{0}", i), 0)
					LL.Variables.Add(String.Format("ESLAArt{0}", i), 0)
				Next

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Daten vorbelegen.{1}", strMethodeName, ex.ToString))
			End Try

			i = 1
			Try
				'Tag, Monat, Std, kilometer, Woche
				While rTemprec.Read
					LL.Variables.Add(String.Format("ESLANr{0}", i), rTemprec("LANr"))
					LL.Variables.Add(String.Format("ESLABez{0}", i), rTemprec("LABez"))
					LL.Variables.Add(String.Format("ESLABetrag{0}", i), rTemprec("Betrag"))

					If m_Utility.SafeGetBoolean(rTemprec, "Tag", False) Then
						LL.Variables.Add(String.Format("ESLAArt{0}", i), 1)
					ElseIf m_Utility.SafeGetBoolean(rTemprec, "Monat", False) Then
						LL.Variables.Add(String.Format("ESLAArt{0}", i), 2)
					ElseIf m_Utility.SafeGetBoolean(rTemprec, "Std", False) Then
						LL.Variables.Add(String.Format("ESLAArt{0}", i), 3)

					ElseIf m_Utility.SafeGetBoolean(rTemprec, "kilometer", False) Then
						LL.Variables.Add(String.Format("ESLAArt{0}", i), 4)
					ElseIf m_Utility.SafeGetBoolean(rTemprec, "Woche", False) Then
						LL.Variables.Add(String.Format("ESLAArt{0}", i), 5)

					End If

					i += i
				End While

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}:Daten von Einsatzlohndaten lesen:{1}", strMethodeName, ex.ToString))

			End Try
			rTemprec.Close()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:Öffnen der Daten von Einsatzlohndaten:{1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Sub GetUSData4ESVertrag()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Conn As New SqlConnection(m_connectionString)
		Dim strFullFilename As String = String.Empty
		Dim strFiles As String = String.Empty
		Dim BA As Byte() = Nothing
		Dim bSignFromUser As Boolean = m_ESUnterzeichner
		Dim strAllESKst As String = String.Empty

		LL.Variables.Add("US_Anrede", String.Empty)
		LL.Variables.Add("ESUS_Anrede", String.Empty)
		LL.Variables.Add("KstFullName_Anrede", String.Empty)
		LL.Variables.Add("KstFullName", String.Empty)
		LL.Variables.Add("USTitel_1", String.Empty)
		LL.Variables.Add("USTitel_2", String.Empty)
		LL.Variables.Add("USAbteilung", String.Empty)

		Dim sUSSql As String = String.Empty

		If bSignFromUser Then
			sUSSql = "[Get USData 4 ESPrint With ESUnterzeichner]"
		Else
			sUSSql = "[Get USData 4 ESPrint With ESKSTData]"
		End If

		Conn.Open()
		Dim cmd As SqlCommand = New SqlCommand(sUSSql, Conn)
		cmd.CommandType = CommandType.StoredProcedure
		Dim param As System.Data.SqlClient.SqlParameter
		If bSignFromUser Then
			param = cmd.Parameters.AddWithValue("@ESNr", Me._ESVertragSetting.SelectedESNr2Print)

		Else
			param = cmd.Parameters.AddWithValue("@ESNr", Me._ESVertragSetting.SelectedESNr2Print)
			param = cmd.Parameters.AddWithValue("@AsVerleih", If(Me._ESVertragSetting.IsPrintAsVerleih, 1, 0))

		End If
		Dim rTemprec As SqlDataReader = cmd.ExecuteReader


		Try
			rTemprec.Read()
			If rTemprec.HasRows Then
				Dim strESUSAnrede As String = rTemprec("Anrede") '"ESUS_Anrede")
				Dim strUSrede As String = rTemprec("Anrede")

				LL.Variables.Add("ESUS_Anrede", strESUSAnrede)
				LL.Variables.Add("US_Anrede", strUSrede)

				LL.Variables.Add("KstFullName_Anrede", rTemprec("Anrede"))
				LL.Variables.Add("KstFullName", String.Format("{0} {1}", rTemprec("Vorname"), rTemprec("Nachname")))
				LL.Variables.Add("USTitel_1", rTemprec("USTitel_1"))
				LL.Variables.Add("USTitel_2", rTemprec("USTitel_2"))
				LL.Variables.Add("USAbteilung", rTemprec("USAbteilung"))

				'strAllESKst = rTemprec("AllESKst")
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:Datenbanken lesen: {1}", strMethodeName, ex.ToString))
			Return

		End Try

	End Sub

	Sub Test_GetUSSign4ESVertrag()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Conn As New SqlConnection(m_connectionString)
		Dim strFullFilename As String = String.Empty
		Dim strFiles As String = String.Empty
		Dim BA As Byte() = Nothing
		Dim bSignFromUser As Boolean = m_ESUnterzeichner ' If(Me._ESVertragSetting.GetESUnterzeichner = 1, True, False)
		LL.Variables.Add("USSignFilename", String.Empty)

		Dim sUSSql As String = String.Empty
		If bSignFromUser Then
			sUSSql = "[Get USSign 4 ESPrint With ESUnterzeichner]"
		Else
			sUSSql = "[Get USSign 4 ESPrint With ESKSTData]"
		End If

		Conn.Open()
		Dim SQLCmd_1 As SqlCommand = New SqlCommand(sUSSql, Conn)
		SQLCmd_1.CommandType = CommandType.StoredProcedure
		Dim param As System.Data.SqlClient.SqlParameter
		If bSignFromUser Then
			param = SQLCmd_1.Parameters.AddWithValue("@ESNr", Me._ESVertragSetting.SelectedESNr2Print)

		Else
			param = SQLCmd_1.Parameters.AddWithValue("@ESNr", Me._ESVertragSetting.SelectedESNr2Print)
			param = SQLCmd_1.Parameters.AddWithValue("@AsVerleih", If(Me._ESVertragSetting.IsPrintAsVerleih, 1, 0))

		End If

		Try
			strFullFilename = String.Format("{0}Bild_{1}.JPG", m_ProgPath.GetSpS2DeleteHomeFolder, System.Guid.NewGuid.ToString())

			Try
				Try
					If IsDBNull(SQLCmd_1.ExecuteScalar) Then
						Return
					Else
						BA = CType(SQLCmd_1.ExecuteScalar, Byte())
					End If
					If BA Is Nothing Then Return

				Catch ex As Exception
					Return

				End Try
				Dim ArraySize As New Integer
				ArraySize = BA.GetUpperBound(0)

				If File.Exists(strFullFilename) Then File.Delete(strFullFilename)
				Dim fs As New FileStream(strFullFilename, FileMode.CreateNew)
				fs.Write(BA, 0, ArraySize + 1)
				fs.Close()
				fs.Dispose()
				LL.Variables.Add("USSignFilename", strFullFilename)
				m_Logger.LogInfo(String.Format("USSignFilename: ({0})", strFullFilename))

			Catch ex As Exception
				m_Logger.LogError(String.Format("***Fehler ({0}): {1}", strMethodeName, ex.ToString))

			End Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("***Fehler ({0}): {1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Private Function OpenMDDb4PrintESVertrag(ByVal _setting As ClsLLESVertragSetting, ByVal jahr As Integer) As SqlDataReader
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strUSKst As String = String.Empty
		Dim strUSNachname As String = String.Empty
		Dim iUSNr As Integer = 0
		Dim Conn As New SqlConnection(_setting.DbConnString2Open)
		Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
		Dim rFoundedrec As SqlDataReader = Nothing

		Try
			Conn.Open()

			Dim sSql As String = "[Get MDData For Print ESVertrag]"
			Dim cmd As SqlCommand = New SqlCommand(sSql, Conn)
			cmd.CommandType = CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@Year", jahr)
			param = cmd.Parameters.AddWithValue("@USNr", _ClsProgSetting.GetLogedUSNr)
			param = cmd.Parameters.AddWithValue("@MDNr", _ESVertragSetting.SelectedMDNr)
			rFoundedrec = cmd.ExecuteReader

			rFoundedrec.Read()

			If IsNothing(rFoundedrec) Or Not rFoundedrec.HasRows Then
				Dim strMessage As String = MainUtilities.TranslateMyText("Die gesuchten Mandanteninformationen sind nicht vorhanden. Bitte kontrollieren sie Ihre Angaben.{0}Jahr: {1} | Benutzer: {2}")
				strMessage = String.Format(strMessage, vbNewLine, jahr, _ClsProgSetting.GetLogedUSNr)

				m_Logger.LogError(String.Format("{0}.rFoundedrec: {1}", strMethodeName, strMessage))
				MsgBox(MainUtilities.TranslateMyText(strMessage),
							 MsgBoxStyle.Critical, strMethodeName)
				Return Nothing

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, ex.ToString))

		End Try

		Return rFoundedrec
	End Function

	Private Sub ExcludeGAVString(ByVal strGAVString As String)
		Dim paresedGAVStirngData As New GAVStringData()
		Try

			paresedGAVStirngData.FillFromString(strGAVString)

			LL.Variables.Add(String.Format("{0}", "Res_15"), String.Empty)
			LL.Variables.Add(String.Format("{0}", "Res_16"), String.Empty)
			LL.Variables.Add(String.Format("{0}", "Res_17"), String.Empty)
			LL.Variables.Add(String.Format("{0}", "Res_18"), String.Empty)
			LL.Variables.Add(String.Format("{0}", "Res_D"), String.Empty)
			LL.Variables.Add(String.Format("{0}", "Res_E"), String.Empty)
			LL.Variables.Add(String.Format("{0}", "Res_F"), String.Empty)

			If Not paresedGAVStirngData.Gruppe0 Is Nothing Then
				LL.Variables.Add(String.Format("{0}", "Res_15"), paresedGAVStirngData.Res_15)
				LL.Variables.Add(String.Format("{0}", "Res_16"), paresedGAVStirngData.Res_16)
				LL.Variables.Add(String.Format("{0}", "Res_17"), paresedGAVStirngData.Res_17)
				LL.Variables.Add(String.Format("{0}", "Res_18"), paresedGAVStirngData.Res_18)
				LL.Variables.Add(String.Format("{0}", "Res_D"), paresedGAVStirngData.Res_D)
				LL.Variables.Add(String.Format("{0}", "Res_E"), paresedGAVStirngData.Res_E)
				LL.Variables.Add(String.Format("{0}", "Res_F"), paresedGAVStirngData.Res_F)
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

	End Sub

	Private Function GetSafeTranslationValue(ByVal dicKey As String, ByVal strDestLanguage As String) As String
		Dim strPersonalizedItem As String = dicKey

		Try
			If m_InitialData.TranslationData.ContainsKey(strPersonalizedItem) Then

				If strDestLanguage = "I" Then
					Return m_InitialData.TranslationData.Item(strPersonalizedItem).Translation_IT
				ElseIf strDestLanguage = "F" Then
					Return m_InitialData.TranslationData.Item(strPersonalizedItem).Translation_FR
				Else
					Return m_InitialData.TranslationData.Item(strPersonalizedItem).LogedUserLanguage
				End If

			Else
				Return strPersonalizedItem

			End If

		Catch ex As Exception
			Return strPersonalizedItem
		End Try

	End Function


#Region "helpers for exporting data"

	Public Function ExportDocWithLLUtil() As Boolean
		Dim strResult As Boolean = True
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim LL_Test As ListLabel = New ListLabel

		Dim strJobNr As String = Me._ESVertragSetting.JobNr2Print

		Dim strQuery As String = _ESVertragSetting.SQL2Open
		Dim Conn As SqlConnection = New SqlConnection(m_connectionString)
		Dim i As Integer = 0

		If Not Me.ExistsDocFile Then Return False

		Try
			Dim bAllowedtoDebug As Boolean = CBool(_ClsProgSetting.GetMDProfilValue("Sonstiges", "EnableLLDebug", "0"))
			If bAllowedtoDebug Then
				LL_Test.Debug = LlDebug.LogToFile
				LL_Test.Debug = LlDebug.Enabled
			End If
		Catch ex As Exception
			m_Logger.LogError(String.Format("EnablingLLDebuging.{0}: {1}", strMethodeName, ex))

		End Try

		Try
			Dim rFoundedrec As SqlDataReader = OpenDb4PrintESVertrag(_ESVertragSetting)
			If Not rFoundedrec.HasRows Then Return TranslateMyText("Keine Daten wurden gefunden") & "..."

			InitLLUtilLL(LL_Test)
			LL_Test.Variables.Clear()
			LL_Test.Fields.Clear()
			Me._ESVertragSetting.SelectedMonth = CDate(rFoundedrec("ES_Ab")).Month
			Me._ESVertragSetting.SelectedYear = CDate(rFoundedrec("ES_Ab")).Year

			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				DefineLLUtilData(LL_Test, True, rFoundedrec)
			Else
				DefineLLUtilData(LL_Test, False, rFoundedrec)
			End If
			SetLLUtilVariable(LL_Test, rFoundedrec)
			ExcludeGAVString(rFoundedrec("GAVInfo_String"))
			LL_Test.Variables.Add("WOSDoc", 1)

			LL_Test.ExportOptions.Clear()
			SetExportSetting(0)

			LL_Test.Core.LlPrintWithBoxStart(If(_ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper), LlProject.List, LlProject.Card),
																	_ClsLLFunc.LLDocName, LlPrintMode.Export, LlBoxType.StandardAbort, CType(0, IntPtr),
																	_ClsLLFunc.LLDocLabel)

			Dim strExportPfad As String = Me._ESVertragSetting.ExportPath
			If Not Directory.Exists(strExportPfad) Then strExportPfad = _ClsProgSetting.GetSpSESTempPath

			Dim strExportFilename As String = Me._ESVertragSetting.GetExportFilename(Me._ESVertragSetting.IsPrintAsVerleih)
			strExportFilename = String.Format(strExportFilename,
																				Me._ESVertragSetting.SelectedESNr2Print,
																				Me._ESVertragSetting.SelectedMANr2Print)

			LL_Test.Core.LlPrintSetOptionString(LlPrintOptionString.Export, _ClsLLFunc.LLExporterName)
			LL_Test.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.File", strExportFilename)
			LL_Test.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.Path", strExportPfad)
			LL_Test.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.Quiet", "1")

			LL_Test.Core.LlPrintSetOption(LlPrintOption.Dialog_DestinationMask, 2)
			Dim TargetFormat As String = LL_Test.Core.LlPrintGetOptionString(LlPrintOptionString.Export)

			While Not LL_Test.Core.LlPrint()
			End While

			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				Do
					' pass data for current record
					DefineLLUtilData(LL_Test, True, rFoundedrec)

					While LL_Test.Core.LlPrintFields() = LlConstants.WrnRepeatData
						LL_Test.Core.LlPrint()
					End While

				Loop While rFoundedrec.Read()

				While LL_Test.Core.LlPrintFieldsEnd() = LlConstants.WrnRepeatData
				End While
			End If

			Try
				LL_Test.Core.LlPrintEnd(0)

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.LLPrintEnd(0)...:{1}", strMethodeName, ex.ToString))

				Return False
			End Try

			Me._ESVertragSetting.ListOfExportedFilesESVertrag.Add(String.Format("{0}{1}", strExportPfad, strExportFilename))

			If TargetFormat = "PRV" Then
				LL_Test.Core.LlPreviewDisplay(_ClsLLFunc.LLDocName, m_ProgPath.GetSpSTempFolder, CType(0, IntPtr))
				LL_Test.Core.LlPreviewDeleteFiles(_ClsLLFunc.LLDocName, m_ProgPath.GetSpSTempFolder)

				Return False
			End If

			Try
				CType(LL_Test, IDisposable).Dispose()

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.LL Disposing...:{1}", strMethodeName, ex.ToString))
				Return False
			End Try

		Catch LlException As Exception
			m_Logger.LogError(String.Format("LlException. ESNr: {0} | MANr: {1}: {2}:{3}",
																 Me._ESVertragSetting.SelectedESNr2Print,
																 Me._ESVertragSetting.SelectedMANr2Print,
																 strMethodeName, LlException.Message))
			Return False

		Finally
			CType(LL_Test, IDisposable).Dispose()

		End Try

		Return strResult
	End Function

	Sub InitLLUtilLL(ByVal llUtil As ListLabel)
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

		llUtil.LicensingInfo = ClsMainSetting.GetLL25LicenceInfo()

		LlCore.LlSetDlgboxMode(LL_DLGBOXMODE_3DBUTTONS + LL_DLGBOXMODE_ALT10)

		' beim LL13 muss ich es so machen...
		llUtil.Core.LlSetOption(LL_OPTION_INCLUDEFONTDESCENT, 0)
		llUtil.Core.LlSetOption(LL_OPTION_INCREMENTAL_PREVIEW, 0)

		' beim LL13 muss ich es so machen...
		llUtil.Core.LlSetOption(LL_OPTION_INCLUDEFONTDESCENT, _ClsLLFunc.LLFontDesent)
		llUtil.Core.LlSetOption(LL_OPTION_INCREMENTAL_PREVIEW, _ClsLLFunc.LLIncPrv)

		llUtil.Core.LlSetOption(LL_OPTION_VARSCASESENSITIVE, 0)

		llUtil.Core.LlSetOption(LL_OPTION_IMMEDIATELASTPAGE, 1)       ' Lastpage
		llUtil.Core.LlSetOption(LL_OPTION_CONVERTCRLF, 1)             ' Doppelte Zeilenumbruch

		llUtil.Core.LlSetOption(LL_OPTION_NOPARAMETERCHECK, _ClsLLFunc.LLParamCheck)
		llUtil.Core.LlSetOption(LL_OPTION_XLATVARNAMES, _ClsLLFunc.LLKonvertName)

		llUtil.Core.LlSetOption(LL_OPTION_ESC_CLOSES_PREVIEW, 1)
		llUtil.Core.LlSetOption(LL_OPTION_SUPERVISOR, 1)
		llUtil.Core.LlSetOption(LL_OPTION_UISTYLE, LL_OPTION_UISTYLE_OFFICE2003)
		llUtil.Core.LlSetOption(LL_OPTION_AUTOMULTIPAGE, 1)

		llUtil.Core.LlSetOption(LL_OPTION_SUPPORTPAGEBREAK, 1)
		llUtil.Core.LlSetOption(LL_OPTION_PRVZOOM_PERC, _ClsLLFunc.LLZoomProz)

		llUtil.Core.LlPreviewSetTempPath(m_ProgPath.GetSpSTempFolder)
		llUtil.Core.LlSetPrinterDefaultsDir(m_ProgPath.GetPrinterHomeFolder)

	End Sub

	Sub SetLLUtilVariable(ByVal llUtil As ListLabel, ByVal rFrec As SqlDataReader)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim aValue As New List(Of String)
		Dim dESLoVon As Date

		Try
			' Mandantendaten drucken...
			GetLLUtilMDUSData4Print(llUtil)
			' Zusätzliche Variable einfügen
			llUtil.Variables.Add("AutorFName", _ClsProgSetting.GetUserFName)
			llUtil.Variables.Add("AutorLName", _ClsProgSetting.GetUserLName)
			llUtil.Variables.Add("USSignFilename", Me._ESVertragSetting.USSignFileName)


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}. US-Variable auflisten. {1}", strMethodeName, ex.ToString))

		End Try

		With rFrec
			Try
				Dim iKDZustaendigNr As Integer = m_Utility.SafeGetInteger(rFrec, "KDZHDNr", 0)
				Dim KDPostfach As String = m_Utility.SafeGetString(rFrec, "KDPostfach")
				Dim KDStrasse As String = m_Utility.SafeGetString(rFrec, "KDStrasse")
				Dim KDPLZOrt As String = m_Utility.SafeGetString(rFrec, "KDPLZOrt")

				Dim aKst As String() = m_Utility.SafeGetString(rFrec, "ESKST").ToString.Split(CChar("/"))
				Dim strKst_1 As String = aKst(0)
				Dim strKst_2 As String = aKst(0)
				If aKst.Length > 1 Then strKst_2 = aKst(1).Trim

				Dim KDzAnredeForm As String = m_Utility.SafeGetString(rFrec, "KDzAnredeForm")
				Dim KDzAnrede As String = m_Utility.SafeGetString(rFrec, "KDzAnrede")
				Dim KDzAnrede_Prog As String = m_Utility.SafeGetString(rFrec, "KDzAnrede")
				Dim KDZFName As String = m_Utility.SafeGetString(rFrec, "KDzVorname")
				Dim KDZLName As String = m_Utility.SafeGetString(rFrec, "KDzNachname")

				Dim KDzPostfach = m_Utility.SafeGetString(rFrec, "KDzPostfach")
				Dim KDzStrasse As String = m_Utility.SafeGetString(rFrec, "KDzStrasse")
				Dim KDzPLZOrt As String = String.Format("{0} {1}", m_Utility.SafeGetString(rFrec, "KDzPLZ"), m_Utility.SafeGetString(rFrec, "KDzOrt"))
				Dim strES_EndeDate As String
				Dim esEndeDateWithGoeslonger As String = String.Empty
				Dim isESEndeNull As Boolean
				Dim employeeLanguage = m_Utility.SafeGetString(rFrec, "MASprache")
				If String.IsNullOrWhiteSpace(employeeLanguage) Then employeeLanguage = "D"

				If m_Utility.SafeGetString(rFrec, "ES_Ende") = String.Empty Then
					strES_EndeDate = GetESEndeByNull
					esEndeDateWithGoeslonger = m_Utility.SafeGetString(rFrec, "GoesLonger")
					strES_EndeDate = GetSafeTranslationValue(strES_EndeDate, employeeLanguage.Substring(0, 1).ToUpper)
					isESEndeNull = True

				Else
					strES_EndeDate = String.Format(rFrec("ES_Ende"), "d")
					isESEndeNull = False

				End If
				If String.IsNullOrWhiteSpace(esEndeDateWithGoeslonger) Then esEndeDateWithGoeslonger = strES_EndeDate

				llUtil.Variables.Add("KDPostfach", KDPostfach)
				llUtil.Variables.Add("KDStrasse", KDStrasse)
				llUtil.Variables.Add("KDPLZOrt", KDPLZOrt)

				llUtil.Variables.Add("KDZNr", iKDZustaendigNr)
				If KDzAnrede_Prog = String.Empty Then
					KDzAnrede_Prog = "Sehr geehrter Kunde"
				Else

					KDzAnrede_Prog = String.Format("Sehr geehrte{0}", If(KDzAnrede = "Herr", "r", ""))
					'KDzAnrede_Prog = String.Format(m_Translate.GetSafeTranslationValue(String.Format("Sehr geehrte{0}", If(KDzAnrede = "Herr", "r", "")))) '& " {0}", KDZLName)

				End If

				llUtil.Variables.Add("KDzAnrede", KDzAnrede)
				llUtil.Variables.Add("KDzAnrede_Prog", GetSafeTranslationValue(KDzAnrede_Prog, employeeLanguage.Substring(0, 1).ToUpper))
				llUtil.Variables.Add("KDzAnredeForm", KDzAnredeForm)
				llUtil.Variables.Add("KDzFName", KDZFName)
				llUtil.Variables.Add("KDzLName", KDZLName)
				llUtil.Variables.Add("KDzPostfach", KDzPostfach)
				llUtil.Variables.Add("KDzStrasse", KDzStrasse)
				llUtil.Variables.Add("KDzPLZOrt", KDzPLZOrt)

				llUtil.Variables.Add("ES_EndeDate", strES_EndeDate)   ' "unbestimmt"
				llUtil.Variables.Add("esEndeDateWithGoeslonger", esEndeDateWithGoeslonger)
				llUtil.Variables.Add("isESEndeNull", isESEndeNull, LlFieldType.Boolean)

				Dim dESBegin As Date = m_Utility.SafeGetDateTime(rFrec, "ES_Ab", Nothing)
				dESLoVon = m_Utility.SafeGetDateTime(rFrec, "LOVon", Nothing)

				llUtil.Variables.Add("MwSt_Satz", GetYearMwSt(Year(dESLoVon)))

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}. Normale Variable auflisten. {1}", strMethodeName, ex.ToString))

			End Try

			' Zusätzliche Lohnarten in Einsatzverwaltung
			GetLLUtilESLohnData4Print(llUtil)

			' Zusätzliche Lohnarten in ES_ZLohn
			GetLLUtilES_ZLohnDataArt0_4Print(llUtil)

			' Zusätzliche Lohnarten in ES_ZLohn
			GetLLUtilES_ZLohnDataArt1_4Print(llUtil)

			' Zusätzliche Lohnarten in ES_ZLohn
			GetLLUtilES_ZLohnDataArt2_4Print(llUtil)

			' Benutzerdaten wegen angemeldeten Benutzer und Unterzeichner
			GetLLUtilUSData4ESVertrag(llUtil)

			' Benutzerunterschrift wegen angemeldeten Benutzer oder Unterzeichner
			Test_GetLLUtilUSSign4ESVertrag(llUtil)

		End With

		Try

			Dim rFoundedrec As SqlDataReader = OpenMDDb4PrintESVertrag(_ESVertragSetting, dESLoVon.Year)
			If Not rFoundedrec.HasRows Then
				Throw New Exception("Keine Mandanteninformationen wurden gefunden.")

			End If
			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				DefineLLUtilData(llUtil, True, rFoundedrec)
			Else
				DefineLLUtilData(llUtil, False, rFoundedrec)
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}. Mandanteninformationen auflisten. {1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Sub GetLLUtilMDUSData4Print(ByVal llUtil As ListLabel)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		LoadEmploymentData(_ESVertragSetting.DbConnString2Open, _ClsLLFunc, _ESVertragSetting.SelectedESNr2Print)
		Try
			With _ClsLLFunc
				llUtil.Variables.Add("MDName", .USMDname)
				llUtil.Variables.Add("MDName2", .USMDname2)
				llUtil.Variables.Add("MDName3", .USMDname3)
				llUtil.Variables.Add("MDPostfach", .USMDPostfach)
				llUtil.Variables.Add("MDStrasse", .USMDStrasse)
				llUtil.Variables.Add("MDPLZ", .USMDPlz)
				llUtil.Variables.Add("MDOrt", .USMDOrt)
				llUtil.Variables.Add("MDLand", .USMDLand)

				llUtil.Variables.Add("MDTelefax", .USMDTelefax)
				llUtil.Variables.Add("MDTelefon", .USMDTelefon)
				llUtil.Variables.Add("MDDTelefon", .USMDDTelefon)
				llUtil.Variables.Add("MDHomepage", .USMDHomepage)
				llUtil.Variables.Add("MDeMail", .USMDeMail)

				llUtil.Variables.Add("USNachName", .USNachname)
				llUtil.Variables.Add("USVorname", .USVorname)

				llUtil.Variables.Add("USTitle1", .USTitel_1)
				llUtil.Variables.Add("USTitle2", .USTitel_2)
				llUtil.Variables.Add("USAbteilung", .USAbteilung)

				' Bewilligungsbehörden
				Dim BewName As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/BewName", m_SonstigesSetting))
				Dim BewPostfach As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/BewPostfach", m_SonstigesSetting))
				Dim BewStrasse As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/BewStrasse", m_SonstigesSetting))
				Dim BewPLZOrt As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/BewPLZOrt", m_SonstigesSetting))
				Dim BewSeco As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/BewSeco", m_SonstigesSetting))

				Dim mwstnr As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/mwstnr", m_InvoiceSetting))
				Dim mwstsatz As Decimal = ParseToDec(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/mwstsatz", m_InvoiceSetting)), 8)


				llUtil.Variables.Add("BewName", BewName)
				llUtil.Variables.Add("BewStr", BewStrasse)
				llUtil.Variables.Add("BewOrt", BewPLZOrt)
				llUtil.Variables.Add("BewNameAus", BewSeco)
				llUtil.Variables.Add("MwStProzent", mwstsatz)

			End With

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Sub GetLLUtilESLohnData4Print(ByVal llUtil As ListLabel)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Dim Conn As New SqlConnection(m_connectionString)
			Conn.Open()
			Dim sSql As String = String.Empty

			If Me._ESVertragSetting.IsPrintAsVerleih Then
				sSql = "[Get ESLAData For Print In Verleih]"
			Else
				sSql = "[Get ESLAData For Print In Einsatzvertrag]"
			End If

			Dim cmd As SqlCommand = New SqlCommand(sSql, Conn)
			cmd.CommandType = CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@ESNr", _ESVertragSetting.SelectedESNr2Print)
			param = cmd.Parameters.AddWithValue("@ESLohnNr", _ESVertragSetting.SelectedESLohnNr2Print)
			Dim rTemprec As SqlDataReader = cmd.ExecuteReader
			Dim i As Integer = 1

			Try
				For i = 1 To 20
					llUtil.Variables.Add(String.Format("ESLANr{0}", i), 0)
					llUtil.Variables.Add(String.Format("ESLABez{0}", i), String.Empty)
					llUtil.Variables.Add(String.Format("ESLABetrag{0}", i), 0)
					llUtil.Variables.Add(String.Format("ESLAArt{0}", i), 0)
				Next

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Daten vorbelegen.{1}", strMethodeName, ex.ToString))
			End Try

			i = 1
			Try
				'Tag, Monat, Std, kilometer, Woche
				While rTemprec.Read
					llUtil.Variables.Add(String.Format("ESLANr{0}", i), rTemprec("LANr"))
					llUtil.Variables.Add(String.Format("ESLABez{0}", i), rTemprec("LABez"))
					llUtil.Variables.Add(String.Format("ESLABetrag{0}", i), rTemprec("Betrag"))

					If m_Utility.SafeGetBoolean(rTemprec, "Tag", False) Then
						llUtil.Variables.Add(String.Format("ESLAArt{0}", i), 1)
					ElseIf m_Utility.SafeGetBoolean(rTemprec, "Monat", False) Then
						llUtil.Variables.Add(String.Format("ESLAArt{0}", i), 2)
					ElseIf m_Utility.SafeGetBoolean(rTemprec, "Std", False) Then
						llUtil.Variables.Add(String.Format("ESLAArt{0}", i), 3)

					ElseIf m_Utility.SafeGetBoolean(rTemprec, "kilometer", False) Then
						llUtil.Variables.Add(String.Format("ESLAArt{0}", i), 4)
					ElseIf m_Utility.SafeGetBoolean(rTemprec, "Woche", False) Then
						llUtil.Variables.Add(String.Format("ESLAArt{0}", i), 5)

					End If

					i += i
				End While

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}:Daten von Einsatzlohndaten lesen:{1}", strMethodeName, ex.ToString))

			End Try
			rTemprec.Close()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:Öffnen der Daten von Einsatzlohndaten:{1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Sub GetLLUtilUSData4ESVertrag(ByVal llUtil As ListLabel)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Conn As New SqlConnection(m_connectionString)
		Dim strFullFilename As String = String.Empty
		Dim strFiles As String = String.Empty
		Dim BA As Byte() = Nothing
		Dim bSignFromUser As Boolean = m_ESUnterzeichner
		Dim strAllESKst As String = String.Empty

		llUtil.Variables.Add("US_Anrede", String.Empty)
		llUtil.Variables.Add("ESUS_Anrede", String.Empty)
		llUtil.Variables.Add("KstFullName_Anrede", String.Empty)
		llUtil.Variables.Add("KstFullName", String.Empty)
		llUtil.Variables.Add("USTitel_1", String.Empty)
		llUtil.Variables.Add("USTitel_2", String.Empty)
		llUtil.Variables.Add("USAbteilung", String.Empty)

		Dim sUSSql As String = String.Empty

		If bSignFromUser Then
			sUSSql = "[Get USData 4 ESPrint With ESUnterzeichner]"
		Else
			sUSSql = "[Get USData 4 ESPrint With ESKSTData]"
		End If

		Conn.Open()
		Dim cmd As SqlCommand = New SqlCommand(sUSSql, Conn)
		cmd.CommandType = CommandType.StoredProcedure
		Dim param As System.Data.SqlClient.SqlParameter
		If bSignFromUser Then
			param = cmd.Parameters.AddWithValue("@ESNr", Me._ESVertragSetting.SelectedESNr2Print)

		Else
			param = cmd.Parameters.AddWithValue("@ESNr", Me._ESVertragSetting.SelectedESNr2Print)
			param = cmd.Parameters.AddWithValue("@AsVerleih", If(Me._ESVertragSetting.IsPrintAsVerleih, 1, 0))

		End If
		Dim rTemprec As SqlDataReader = cmd.ExecuteReader


		Try
			rTemprec.Read()
			If rTemprec.HasRows Then
				Dim strESUSAnrede As String = rTemprec("Anrede") '"ESUS_Anrede")
				Dim strUSrede As String = rTemprec("Anrede")

				llUtil.Variables.Add("ESUS_Anrede", strESUSAnrede)
				llUtil.Variables.Add("US_Anrede", strUSrede)

				llUtil.Variables.Add("KstFullName_Anrede", rTemprec("Anrede"))
				llUtil.Variables.Add("KstFullName", String.Format("{0} {1}", rTemprec("Vorname"), rTemprec("Nachname")))
				llUtil.Variables.Add("USTitel_1", rTemprec("USTitel_1"))
				llUtil.Variables.Add("USTitel_2", rTemprec("USTitel_2"))
				llUtil.Variables.Add("USAbteilung", rTemprec("USAbteilung"))

				'strAllESKst = rTemprec("AllESKst")
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:Datenbanken lesen: {1}", strMethodeName, ex.ToString))
			Return

		End Try

	End Sub

	Sub GetLLUtilES_ZLohnDataArt0_4Print(ByVal llUtil As ListLabel)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try

			For i As Short = 1 To 4
				llUtil.Variables.Add(String.Format("S_ESZLANr{0}", i), 0)
				llUtil.Variables.Add(String.Format("S_ESZLABez{0}", i), "")

				llUtil.Variables.Add(String.Format("S_Art_{0}", i), 0)
				llUtil.Variables.Add(String.Format("S_ESZLABetrag_G_{0}", i), 0)
				llUtil.Variables.Add(String.Format("S_ESZLABetrag_StdLohn_{0}", i), 0)

				llUtil.Variables.Add(String.Format("S_ESZLABetrag_Feier_{0}", i), 0)
				llUtil.Variables.Add(String.Format("S_ESZLABetrag_FeierProz_{0}", i), 0)

				llUtil.Variables.Add(String.Format("S_ESZLABetrag_Fer_{0}", i), 0)
				llUtil.Variables.Add(String.Format("S_ESZLABetrag_FerProz_{0}", i), 0)

				llUtil.Variables.Add(String.Format("S_ESZLABetrag_13_{0}", i), 0)
				llUtil.Variables.Add(String.Format("S_ESZLABetrag_13Proz_{0}", i), 0)

			Next i

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Daten vorbelegen.{1}", strMethodeName, ex.ToString))

		End Try

		Try
			Dim Conn As New SqlConnection(m_connectionString)
			Dim sSql As String = "[Get ESZLohnData For Print Vertrag Art]"
			Conn.Open()

			Dim cmd As SqlCommand = New SqlCommand(sSql, Conn)
			cmd.CommandType = CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@ESNr", _ESVertragSetting.SelectedESNr2Print)
			param = cmd.Parameters.AddWithValue("@Art", 0)
			param = cmd.Parameters.AddWithValue("@Year", Now.Year)
			Dim rTemprec As SqlDataReader = cmd.ExecuteReader

			Dim i As Integer = 1
			Try
				While rTemprec.Read
					llUtil.Variables.Add(String.Format("S_ESZLANr{0}", i), rTemprec("LANr"))
					llUtil.Variables.Add(String.Format("S_ESZLABez{0}", i), rTemprec("LABez"))

					llUtil.Variables.Add(String.Format("S_Art_{0}", i), rTemprec("Art"))
					llUtil.Variables.Add(String.Format("S_ESZLABetrag_G_{0}", i), rTemprec("Grundlohn"))
					llUtil.Variables.Add(String.Format("S_ESZLABetrag_StdLohn_{0}", i), rTemprec("Stundenlohn"))

					llUtil.Variables.Add(String.Format("S_ESZLABetrag_Feier_{0}", i), rTemprec("Feier"))
					llUtil.Variables.Add(String.Format("S_ESZLABetrag_FeierProz_{0}", i), rTemprec("FeierProz"))

					llUtil.Variables.Add(String.Format("S_ESZLABetrag_Fer_{0}", i), rTemprec("Ferien"))
					llUtil.Variables.Add(String.Format("S_ESZLABetrag_FerProz_{0}", i), rTemprec("FerienProz"))

					llUtil.Variables.Add(String.Format("S_ESZLABetrag_13_{0}", i), rTemprec("Lohn13"))
					llUtil.Variables.Add(String.Format("S_ESZLABetrag_13Proz_{0}", i), rTemprec("Lohn13Proz"))

					i += 1
				End While

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Daten lesen.{1}", strMethodeName, ex.ToString))

			End Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Sub GetLLUtilES_ZLohnDataArt1_4Print(ByVal llUtil As ListLabel)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			For i As Short = 5 To 8
				llUtil.Variables.Add(String.Format("W_ESZLANr{0}", i), 0)
				llUtil.Variables.Add(String.Format("W_ESZLABez{0}", i), "")

				llUtil.Variables.Add(String.Format("W_Art_{0}", i), 0)
				llUtil.Variables.Add(String.Format("W_ESZLABetrag_G_{0}", i), 0)
				llUtil.Variables.Add(String.Format("W_ESZLABetrag_StdLohn_{0}", i), 0)

				llUtil.Variables.Add(String.Format("W_ESZLABetrag_Feier_{0}", i), 0)
				llUtil.Variables.Add(String.Format("W_ESZLABetrag_FeierProz_{0}", i), 0)

				llUtil.Variables.Add(String.Format("W_ESZLABetrag_Fer_{0}", i), 0)
				llUtil.Variables.Add(String.Format("W_ESZLABetrag_FerProz_{0}", i), 0)

				llUtil.Variables.Add(String.Format("W_ESZLABetrag_13_{0}", i), 0)
				llUtil.Variables.Add(String.Format("W_ESZLABetrag_13Proz_{0}", i), 0)

			Next i

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Daten vorbelegen.{1}", strMethodeName, ex.ToString))

		End Try

		Try
			Dim Conn As New SqlConnection(m_connectionString)
			Dim sSql As String = "[Get ESZLohnData For Print Vertrag Art]"
			Conn.Open()

			Dim cmd As SqlCommand = New SqlCommand(sSql, Conn)
			cmd.CommandType = CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@ESNr", _ESVertragSetting.SelectedESNr2Print)
			param = cmd.Parameters.AddWithValue("@Art", 1)
			param = cmd.Parameters.AddWithValue("@Year", Now.Year)
			Dim rTemprec As SqlDataReader = cmd.ExecuteReader

			Dim i As Integer = 5
			Try
				While rTemprec.Read
					llUtil.Variables.Add(String.Format("W_ESZLANr{0}", i), rTemprec("LANr"))
					llUtil.Variables.Add(String.Format("W_ESZLABez{0}", i), rTemprec("LABez"))

					llUtil.Variables.Add(String.Format("W_Art_{0}", i), rTemprec("Art"))
					llUtil.Variables.Add(String.Format("W_ESZLABetrag_G_{0}", i), rTemprec("Grundlohn"))
					llUtil.Variables.Add(String.Format("W_ESZLABetrag_StdLohn_{0}", i), rTemprec("Stundenlohn"))

					llUtil.Variables.Add(String.Format("W_ESZLABetrag_Feier_{0}", i), rTemprec("Feier"))
					llUtil.Variables.Add(String.Format("W_ESZLABetrag_FeierProz_{0}", i), rTemprec("FeierProz"))

					llUtil.Variables.Add(String.Format("W_ESZLABetrag_Fer_{0}", i), rTemprec("Ferien"))
					llUtil.Variables.Add(String.Format("W_ESZLABetrag_FerProz_{0}", i), rTemprec("FerienProz"))

					llUtil.Variables.Add(String.Format("W_ESZLABetrag_13_{0}", i), rTemprec("Lohn13"))
					llUtil.Variables.Add(String.Format("W_ESZLABetrag_13Proz_{0}", i), rTemprec("Lohn13Proz"))

					i += 1
				End While

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Daten lesen.{1}", strMethodeName, ex.ToString))

			End Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Sub GetLLUtilES_ZLohnDataArt2_4Print(ByVal llUtil As ListLabel)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			For i As Short = 9 To 12
				llUtil.Variables.Add(String.Format("M_ESZLANr{0}", i), 0)
				llUtil.Variables.Add(String.Format("M_ESZLABez{0}", i), "")

				llUtil.Variables.Add(String.Format("M_Art_{0}", i), 0)
				llUtil.Variables.Add(String.Format("M_ESZLABetrag_G_{0}", i), 0)
				llUtil.Variables.Add(String.Format("M_ESZLABetrag_StdLohn_{0}", i), 0)

				llUtil.Variables.Add(String.Format("M_ESZLABetrag_Feier_{0}", i), 0)
				llUtil.Variables.Add(String.Format("M_ESZLABetrag_FeierProz_{0}", i), 0)

				llUtil.Variables.Add(String.Format("M_ESZLABetrag_Fer_{0}", i), 0)
				llUtil.Variables.Add(String.Format("M_ESZLABetrag_FerProz_{0}", i), 0)

				llUtil.Variables.Add(String.Format("M_ESZLABetrag_13_{0}", i), 0)
				llUtil.Variables.Add(String.Format("M_ESZLABetrag_13Proz_{0}", i), 0)

			Next i

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Daten vorbelegen.{1}", strMethodeName, ex.ToString))

		End Try

		Try
			Dim Conn As New SqlConnection(m_connectionString)
			Dim sSql As String = "[Get ESZLohnData For Print Vertrag Art]"
			Conn.Open()

			Dim cmd As SqlCommand = New SqlCommand(sSql, Conn)
			cmd.CommandType = CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@ESNr", _ESVertragSetting.SelectedESNr2Print)
			param = cmd.Parameters.AddWithValue("@Art", 2)
			param = cmd.Parameters.AddWithValue("@Year", Now.Year)
			Dim rTemprec As SqlDataReader = cmd.ExecuteReader

			Dim i As Integer = 9
			Try
				While rTemprec.Read
					llUtil.Variables.Add(String.Format("M_ESZLANr{0}", i), rTemprec("LANr"))
					llUtil.Variables.Add(String.Format("M_ESZLABez{0}", i), rTemprec("LABez"))

					llUtil.Variables.Add(String.Format("M_Art_{0}", i), rTemprec("Art"))
					llUtil.Variables.Add(String.Format("M_ESZLABetrag_G_{0}", i), rTemprec("Grundlohn"))
					llUtil.Variables.Add(String.Format("M_ESZLABetrag_StdLohn_{0}", i), rTemprec("Stundenlohn"))

					llUtil.Variables.Add(String.Format("M_ESZLABetrag_Feier_{0}", i), rTemprec("Feier"))
					llUtil.Variables.Add(String.Format("M_ESZLABetrag_FeierProz_{0}", i), rTemprec("FeierProz"))

					llUtil.Variables.Add(String.Format("M_ESZLABetrag_Fer_{0}", i), rTemprec("Ferien"))
					llUtil.Variables.Add(String.Format("M_ESZLABetrag_FerProz_{0}", i), rTemprec("FerienProz"))

					llUtil.Variables.Add(String.Format("M_ESZLABetrag_13_{0}", i), rTemprec("Lohn13"))
					llUtil.Variables.Add(String.Format("M_ESZLABetrag_13Proz_{0}", i), rTemprec("Lohn13Proz"))

					i += 1
				End While

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Daten lesen.{1}", strMethodeName, ex.ToString))

			End Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Sub Test_GetLLUtilUSSign4ESVertrag(ByVal llUtil As ListLabel)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Conn As New SqlConnection(m_connectionString)
		Dim strFullFilename As String = String.Empty
		Dim strFiles As String = String.Empty
		Dim BA As Byte() = Nothing
		Dim bSignFromUser As Boolean = m_ESUnterzeichner ' If(Me._ESVertragSetting.GetESUnterzeichner = 1, True, False)
		llUtil.Variables.Add("USSignFilename", String.Empty)

		Dim sUSSql As String = String.Empty
		If bSignFromUser Then
			sUSSql = "[Get USSign 4 ESPrint With ESUnterzeichner]"
		Else
			sUSSql = "[Get USSign 4 ESPrint With ESKSTData]"
		End If

		Conn.Open()
		Dim SQLCmd_1 As SqlCommand = New SqlCommand(sUSSql, Conn)
		SQLCmd_1.CommandType = CommandType.StoredProcedure
		Dim param As System.Data.SqlClient.SqlParameter
		If bSignFromUser Then
			param = SQLCmd_1.Parameters.AddWithValue("@ESNr", Me._ESVertragSetting.SelectedESNr2Print)

		Else
			param = SQLCmd_1.Parameters.AddWithValue("@ESNr", Me._ESVertragSetting.SelectedESNr2Print)
			param = SQLCmd_1.Parameters.AddWithValue("@AsVerleih", If(Me._ESVertragSetting.IsPrintAsVerleih, 1, 0))

		End If

		Try
			strFullFilename = String.Format("{0}Bild_{1}.JPG", m_ProgPath.GetSpS2DeleteHomeFolder, System.Guid.NewGuid.ToString())

			Try
				Try
					If IsDBNull(SQLCmd_1.ExecuteScalar) Then
						Return
					Else
						BA = CType(SQLCmd_1.ExecuteScalar, Byte())
					End If
					If BA Is Nothing Then Return

				Catch ex As Exception
					Return

				End Try
				Dim ArraySize As New Integer
				ArraySize = BA.GetUpperBound(0)

				If File.Exists(strFullFilename) Then File.Delete(strFullFilename)
				Dim fs As New FileStream(strFullFilename, FileMode.CreateNew)
				fs.Write(BA, 0, ArraySize + 1)
				fs.Close()
				fs.Dispose()
				llUtil.Variables.Add("USSignFilename", strFullFilename)

			Catch ex As Exception
				m_Logger.LogError(String.Format("***Fehler ({0}): {1}", strMethodeName, ex.ToString))

			End Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("***Fehler ({0}): {1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Sub DefineLLUtilData(ByVal llUtil As ListLabel, ByVal AsFields As Boolean, ByVal MyDataReader As SqlDataReader)
		Dim i As Integer

		Dim iType As String
		Dim strParam As LlFieldType
		Dim strContent As String

		For i = 0 To MyDataReader.FieldCount - 1
			iType = MyDataReader.GetFieldType(i).ToString.ToUpper

			Select Case iType.ToUpper

				Case "System.Int16".ToUpper, "System.Int32".ToUpper, "System.Int64".ToUpper, "System.Decimal".ToUpper, "System.Double".ToUpper
					strParam = LlFieldType.Numeric
					strContent = MyDataReader.Item(i).ToString & ""

				Case "System.DateTime".ToUpper
					strParam = LlFieldType.Date_OLE

					If IsDBNull(MyDataReader.Item(i)) Then
						strContent = CDate("01.01.1900").ToOADate().ToString() ' , "G")
					Else
						strContent = MyDataReader.GetDateTime(i).ToOADate().ToString()
					End If

				Case "System.Boolean".ToUpper
					strParam = LlFieldType.Boolean
					If IsDBNull(MyDataReader.Item(i)) Then
						strContent = "0"
					Else
						strContent = CStr(IIf(CBool(MyDataReader.Item(i)), 1, 0))
					End If

				Case Else
					strParam = LlFieldType.Text
					If IsDBNull(MyDataReader.Item(i)) Then
						strContent = String.Empty
					Else
						strContent = MyDataReader.Item(i).ToString
					End If

			End Select

			llUtil.Core.LlDefineFieldExt(MyDataReader.GetName(i), strContent, strParam)
			llUtil.Core.LlDefineVariableExt(MyDataReader.GetName(i), strContent, strParam)
		Next

	End Sub


#End Region


End Class

