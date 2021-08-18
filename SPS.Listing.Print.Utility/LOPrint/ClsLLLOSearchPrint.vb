
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath

Imports System.Text.RegularExpressions
Imports System.Data.SqlClient
Imports System.IO
Imports combit.ListLabel25
Imports SPS.Listing.Print.Utility.ClsMainSetting
Imports SPS.Listing.Print.Utility.MainUtilities
Imports SPS.Listing.Print.Utility.LODbDatabases.ClsLODb4Print

Imports SPS.MA.Lohn.Utility.SPSLohnUtility.ClsLOFunktionality
Imports SPS.MA.Lohn.Utility.SPSLohnUtility.ClsGuthabenIndividuell
Imports SPS.MA.Lohn.Utility.SPSLohnUtility.ClsGuthabenProLohnabrechnung
Imports SPS.MA.Lohn.Utility.SPSLohnUtility.ClsGuthaben

Imports SP.Infrastructure.Logging
Imports SP.DatabaseAccess.PayrollMng
Imports SP.DatabaseAccess.PayrollMng.DataObjects
Imports SP.DatabaseAccess.Listing

Public Class ClsLLLOSearchPrint
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


	Private m_ProgPath As ClsProgPath
	Private m_mandant As Mandant
	Private mSP_utility As SPProgUtility.MainUtilities.Utilities
	Private m_utilityUI As SP.Infrastructure.UI.UtilityUI
	Private m_utility As SP.Infrastructure.Utility

	'Private LOPrintSetting As New ClsLLLOSearchPrintSetting
	Private LOPrintSetting As ClsLLLOSearchPrintSetting

	Private _ClsReg As SPProgUtility.ClsDivReg
	Private _ClsLLFunc As ClsLLFunc

	Private m_connectionString As String = String.Empty

	Private Property USSignFileName As String
	Private Property ExistsDocFile As Boolean

	Private LL As ListLabel '= New ListLabel
	Private m_PayrollDatabaseAccess As IPayrollDatabaseAccess
	Private m_ListingDatabaseAccess As IListingDatabaseAccess
	Private m_PayrollData As LOMasterData
	Private m_CurrentPayrollNumber As Integer
	Private m_CurrentEmployeeNumber As Integer

	Private m_sortkwlanrinpayslip As Boolean
	Private m_MandantFormXMLFile As String
	Private Const FORM_XML_DEFAULTVALUES_KEY As String = "Forms_Normaly/Field_DefaultValues"
	Private Const FORM_XML_PRINT_PAYROLL_KEY As String = "Forms_Normaly/Lohnbuchhaltung"




#Region "Constructor"

	Public Sub New(ByVal _Setting As ClsLLLOSearchPrintSetting)

		m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ClsMainSetting.TranslationData, ClsMainSetting.PerosonalizedData, ClsMainSetting.MDData, ClsMainSetting.UserData)
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

		m_ProgPath = New ClsProgPath
		m_mandant = New Mandant
		mSP_utility = New SPProgUtility.MainUtilities.Utilities
		m_utility = New SP.Infrastructure.Utility
		m_utilityUI = New SP.Infrastructure.UI.UtilityUI
		LL = New ListLabel
		_ClsReg = New SPProgUtility.ClsDivReg
		_ClsLLFunc = New ClsLLFunc


		Me.LOPrintSetting = _Setting
		Me.m_connectionString = _Setting.DbConnString2Open

		m_PayrollDatabaseAccess = New PayrollDatabaseAccess(m_connectionString, ClsMainSetting.UserData.UserLanguage)
		m_ListingDatabaseAccess = New ListingDatabaseAccess(m_connectionString, ClsMainSetting.UserData.UserLanguage)

		Dim recordMandantNumber As Integer = LOPrintSetting.SelectedMDNr
		LoadPayrollData(LOPrintSetting.SelectedLONr2Print)
		If Not m_PayrollData Is Nothing Then
			recordMandantNumber = m_PayrollData.MDNr
		End If
		'If Not (recordMandantNumber = LOPrintSetting.SelectedMDNr OrElse ClsMainSetting.MDData.MultiMD = 0) Then
		LOPrintSetting.SelectedMDNr = recordMandantNumber
		'End If

		m_MandantFormXMLFile = m_mandant.GetSelectedMDFormDataXMLFilename(LOPrintSetting.SelectedMDNr)

		Me.ExistsDocFile = BuildPrintJob()

		Dim sortkwlanrinpayslip As Boolean? = Nothing
		If Not String.IsNullOrWhiteSpace(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/sortkwlanrinpayslip", FORM_XML_DEFAULTVALUES_KEY))) Then
			sortkwlanrinpayslip = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/sortkwlanrinpayslip", FORM_XML_DEFAULTVALUES_KEY)), False)
		Else
			'Dim _ClsReg As New SPProgUtility.ClsDivReg
			Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
			If Val(_ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, "Lohnbuchhaltung", "SortKW+LANr", "0")) <> 0 Then
				sortkwlanrinpayslip = True
			Else
				sortkwlanrinpayslip = False
			End If
		End If
		m_sortkwlanrinpayslip = sortkwlanrinpayslip

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


	Public ReadOnly Property GetNumberOfCopy As Integer
		Get
			If _ClsLLFunc Is Nothing Then Return 1

			Return _ClsLLFunc.LLCopyCount

		End Get
	End Property

	Function BuildPrintJob() As Boolean
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim sSql As String = "Select * From DokPrint Where [JobNr] = @JobNr"
		Dim Conn As SqlConnection = New SqlConnection(m_connectionString)
		Dim bResult As Boolean = True
		Dim JobNr As String = LOPrintSetting.JobNr2Print

		If JobNr = String.Empty Then
			Dim strMessage As String = "Sie haben keine Vorlage ausgewählt.{0}Bitte wählen Sie aus der Liste eine Vorlage aus."
			MsgBox(String.Format(TranslateMyText(strMessage), vbNewLine), MsgBoxStyle.Critical, TranslateMyText("Leere Vorlage"))
			m_Logger.LogError(String.Format("{0} {1}", strMessage, LOPrintSetting.JobNr2Print))

			Return False
		End If
		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
		Dim param As System.Data.SqlClient.SqlParameter

		Try
			Conn.Open()

			param = cmd.Parameters.AddWithValue("@JobNr", LOPrintSetting.JobNr2Print)
			Dim rDocrec As SqlDataReader = cmd.ExecuteReader          ' Dokumentendatenbank
			rDocrec.Read()
			If Not String.IsNullOrEmpty(rDocrec("DocName").ToString) Then
				_ClsLLFunc.LLDocName = String.Format("{0}{1}{2}", m_mandant.GetSelectedMDDocPath(Me.LOPrintSetting.SelectedMDNr), If(LOPrintSetting.SelectedMALang <> String.Empty, LOPrintSetting.SelectedMALang & "\", ""), rDocrec("DocName").ToString)
				If Not File.Exists(_ClsLLFunc.LLDocName) Then
					_ClsLLFunc.LLDocName = String.Format("{0}{1}", m_mandant.GetSelectedMDDocPath(Me.LOPrintSetting.SelectedMDNr), rDocrec("DocName").ToString)
				End If
				_ClsLLFunc.LLDocLabel = MainUtilities.TranslateMyText(rDocrec("Bezeichnung").ToString)

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

				If LOPrintSetting.NumberOfCopy Is Nothing Then

					If String.IsNullOrEmpty(rDocrec("Anzahlkopien").ToString) Then
						_ClsLLFunc.LLCopyCount = 1
					Else
						_ClsLLFunc.LLCopyCount = If(LOPrintSetting.AnzahlCopies = 0, Math.Max(CByte(rDocrec("Anzahlkopien")), 1), LOPrintSetting.AnzahlCopies)
					End If

				Else
					_ClsLLFunc.LLCopyCount = LOPrintSetting.NumberOfCopy

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

				m_Logger.LogError(String.Format("doc name was not founded! {0}", LOPrintSetting.JobNr2Print))

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
			Dim bAllowedtoDebug As Boolean = CBool(m_mandant.GetSelectedMDProfilValue(LOPrintSetting.SelectedMDNr, Now.Year, "Sonstiges", "EnableLLDebug", "0"))
			If bAllowedtoDebug Then
				LL.Debug = LlDebug.LogToFile
				LL.Debug = LlDebug.Enabled
			End If
		Catch ex As Exception
			m_Logger.LogError(String.Format("EnablingLLDebuging.{0}: {1}", strMethodeName, ex))

		End Try

		If Not Me.ExistsDocFile Then Return

		Try
			Dim rFoundedrec As SqlDataReader = OpenDb4LOPrint(LOPrintSetting, m_sortkwlanrinpayslip)

			'Dim rFoundedrec As SqlDataReader = MainUtilities.OpenDb4PrintListing(strConnString, Me.LOPrintSetting.SQL2Open)
			If Not rFoundedrec.HasRows Then
				m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, "Keine Daten wurden gefunden..."))

				Return
			End If

			InitLL()
			LL.Variables.Clear()
			LL.Fields.Clear()
			Me.LOPrintSetting.SelectedMonth = rFoundedrec("LP")
			Me.LOPrintSetting.SelectedYear = rFoundedrec("Jahr")
			Me.LOPrintSetting.LoCreatedFrom = rFoundedrec("LOCreatedFrom")
			Me.LOPrintSetting.LoCreatedOn = rFoundedrec("LOCreatedOn")

			LL.Variables.Add("WOSDoc", 1)

			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				DefineData(True, rFoundedrec)
			Else
				DefineData(False, rFoundedrec)
			End If
			SetLLVariable()

			LL.Core.LlDefineLayout(CType(0, IntPtr), _ClsLLFunc.LLDocLabel,
														 If(_ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper), LlProject.List, LlProject.Card),
														 _ClsLLFunc.LLDocName)

		Catch LlException As Exception
			m_Logger.LogError(String.Format("LlException.{0}: {1}", strMethodeName, LlException.Message))

		Finally

		End Try

	End Sub

	Function ShowInPrint(ByRef bShowBox As Boolean) As Boolean
		Const LL_PRNOPT_COPIES_SUPPORTED As Integer = 3
		Dim result As Boolean = True
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strJobNr As String = Me.LOPrintSetting.JobNr2Print

		Dim i As Integer = 0

		If Not Me.ExistsDocFile Then Return False

		Try
			Dim rFoundedrec As SqlDataReader = OpenDb4LOPrint(LOPrintSetting, m_sortkwlanrinpayslip)
			If Not rFoundedrec.HasRows Then
				m_Logger.LogError("Keine Daten wurden gefunden.")

				Return False
			End If

			Try
				Dim bAllowedtoDebug As Boolean = CBool(m_mandant.GetSelectedMDProfilValue(LOPrintSetting.SelectedMDNr, Now.Year, "Sonstiges", "EnableLLDebug", "0"))
				If bAllowedtoDebug Then
					LL.Debug = LlDebug.LogToFile
					LL.Debug = LlDebug.Enabled
				End If
			Catch ex As Exception
				m_Logger.LogError(String.Format("EnablingLLDebuging.{0}: {1}", strMethodeName, ex))

			End Try

			InitLL()
			LL.Variables.Clear()
			LL.Fields.Clear()
			Me.LOPrintSetting.SelectedMonth = rFoundedrec("LP")
			Me.LOPrintSetting.SelectedYear = rFoundedrec("Jahr")
			Me.LOPrintSetting.LoCreatedFrom = rFoundedrec("LOCreatedFrom")
			Me.LOPrintSetting.LoCreatedOn = rFoundedrec("LOCreatedOn")

			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				DefineData(True, rFoundedrec)
			Else
				DefineData(False, rFoundedrec)
			End If
			SetLLVariable()
			LL.Variables.Add("WOSDoc", 0)

			LL.Core.LlPrintWithBoxStart(If(_ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper),
																		 LlProject.List, LlProject.Card),
																		 _ClsLLFunc.LLDocName, LlPrintMode.Export, LlBoxType.StandardAbort,
																		 CType(0, IntPtr), _ClsLLFunc.LLDocLabel)

			LL.Core.LlPrintSetOption(LlPrintOption.Copies, _ClsLLFunc.LLCopyCount)
			LL.Core.LlPrintSetOption(LL_PRNOPT_COPIES_SUPPORTED, LL.Core.LlPrintGetOption(LlPrintOption.Copies))

			If bShowBox AndAlso Not Me.LOPrintSetting.Is4Export Then
				LL.Core.LlPrintOptionsDialog(CType(0, IntPtr), String.Format("{1}: {2}{0}{3}", vbNewLine, _ClsLLFunc.LLDocLabel, strJobNr, _ClsLLFunc.LLDocName))
				Dim newCopyCount As Integer = Val(LL.Core.LlPrintGetOption(LlPrintOption.Copies))
				If newCopyCount = 0 Then newCopyCount = 1
				_ClsLLFunc.LLCopyCount = newCopyCount
			End If
			Dim TargetFormat As String = LL.Core.LlPrintGetOptionString(LlPrintOptionString.Export)

			While Not LL.Core.LlPrint()
			End While

			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				Do
					' pass data for current record
					DefineData(True, rFoundedrec)

					While LL.Core.LlPrintFields() = LlConstants.WrnRepeatData
						LL.Core.LlPrint()
					End While

				Loop While rFoundedrec.Read()

				While LL.Core.LlPrintFieldsEnd() = LlConstants.WrnRepeatData
				End While
			End If
			LL.Core.LlPrintEnd(0)

			If TargetFormat = "PRV" Then
				LL.Core.LlPreviewDisplay(_ClsLLFunc.LLDocName, m_ProgPath.GetSpSTempFolder, CType(0, IntPtr))
				LL.Core.LlPreviewDeleteFiles(_ClsLLFunc.LLDocName, m_ProgPath.GetSpSTempFolder)

				result = False

			End If


		Catch LlException As LL_User_Aborted_Exception
			Return False

		Catch LlException As Exception
			m_Logger.LogError(String.Format("LlException.{0}: {1}", strMethodeName, LlException.Message))
			result = False

		Finally

		End Try

		Return result
	End Function

	Function ExportLLDoc() As String
		Dim strResult As String = String.Empty
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strJobNr As String = Me.LOPrintSetting.JobNr2Print

		Dim i As Integer = 0

		If Not Me.ExistsDocFile Then Return "doc file was not founded!"

		Try
			Dim rFoundedrec As SqlDataReader = OpenDb4LOPrint(LOPrintSetting, m_sortkwlanrinpayslip)
			If Not rFoundedrec.HasRows Then Return strResult

			InitLL()
			LL.Variables.Clear()
			LL.Fields.Clear()
			Me.LOPrintSetting.SelectedMonth = mSP_utility.SafeGetShort(rFoundedrec, "LP", 0)
			Me.LOPrintSetting.SelectedYear = mSP_utility.SafeGetShort(rFoundedrec, "Jahr", 0)
			m_CurrentPayrollNumber = mSP_utility.SafeGetInteger(rFoundedrec, "LONr", 0)
			m_CurrentEmployeeNumber = mSP_utility.SafeGetInteger(rFoundedrec, "MANr", 0)

			Me.LOPrintSetting.LoCreatedFrom = rFoundedrec("LOCreatedFrom")
			Me.LOPrintSetting.LoCreatedOn = rFoundedrec("LOCreatedOn")

			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				DefineData(True, rFoundedrec)
			Else
				DefineData(False, rFoundedrec)
			End If
			SetLLVariable()
			LL.Variables.Add("WOSDoc", 1)

			LL.ExportOptions.Clear()
			SetExportSetting(0)

			LL.Core.LlPrintWithBoxStart(If(_ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper), LlProject.List, LlProject.Card),
																	_ClsLLFunc.LLDocName, LlPrintMode.Export, LlBoxType.StandardAbort, CType(0, IntPtr),
																	_ClsLLFunc.LLDocLabel)

			Dim strExportPfad As String = m_InitializationData.UserData.spTempPayrollPath
			If Not Directory.Exists(strExportPfad) Then Directory.CreateDirectory(m_InitializationData.UserData.spTempPayrollPath)

			Dim initialFilename As String = "Lohnabrechnung_"
			Dim strExportFilename = String.Format("{0}{1}_{2}.pdf", initialFilename, Me.LOPrintSetting.SelectedLONr2Print, Me.LOPrintSetting.SelectedMANr2Print)

			If File.Exists(Path.Combine(strExportPfad, strExportFilename)) Then
				Try
					File.Delete(Path.Combine(strExportPfad, strExportFilename))
				Catch ex As Exception
					strExportFilename = String.Format("{0}{1}_{2}_{3}.PDF", initialFilename, Me.LOPrintSetting.SelectedLONr2Print, Me.LOPrintSetting.SelectedMANr2Print, Environment.TickCount)
				End Try
			End If

			LL.Core.LlPrintSetOptionString(LlPrintOptionString.Export, _ClsLLFunc.LLExporterName)
			LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.File", strExportFilename)
			LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.Path", strExportPfad)
			LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.Quiet", "1")

			LL.Core.LlPrintSetOption(LlPrintOption.Dialog_DestinationMask, 2)
			Dim TargetFormat As String = LL.Core.LlPrintGetOptionString(LlPrintOptionString.Export)

			While Not LL.Core.LlPrint()
			End While

			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				Do
					' pass data for current record
					DefineData(True, rFoundedrec)

					While LL.Core.LlPrintFields() = LlConstants.WrnRepeatData
						LL.Core.LlPrint()
					End While

				Loop While rFoundedrec.Read()

				While LL.Core.LlPrintFieldsEnd() = LlConstants.WrnRepeatData
				End While
			End If
			LL.Core.LlPrintEnd(0)

			Dim exportData As New ExportPayrollData
			exportData.LONr = m_CurrentPayrollNumber
			exportData.MANr = m_CurrentEmployeeNumber
			exportData.PayrollMonth = LOPrintSetting.SelectedMonth
			exportData.PayrollYear = LOPrintSetting.SelectedYear
			exportData.ExportFilename = Path.Combine(strExportPfad, strExportFilename)
			exportData.FileContent = m_utility.LoadFileBytes(Path.Combine(strExportPfad, strExportFilename))

			LOPrintSetting.ListOfExportedFiles.Add(exportData)

			If TargetFormat = "PRV" Then
				LL.Core.LlPreviewDisplay(_ClsLLFunc.LLDocName, m_ProgPath.GetSpSTempFolder, CType(0, IntPtr))
				LL.Core.LlPreviewDeleteFiles(_ClsLLFunc.LLDocName, m_ProgPath.GetSpSTempFolder)
			End If

			Try
				LL.Dispose()
				LL.Core.Dispose()

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.LL Disposing...:{1}", strMethodeName, ex.ToString))

			End Try


		Catch LlException As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, LlException))
			strResult = String.Format("Error: {0}", LlException.Message)

		Finally

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

	Sub DefineData(ByVal AsFields As Boolean, ByVal MyDataReader As SqlDataReader)
		Dim i As Integer
		Dim iType As String
		Dim strParam As LlFieldType
		Dim strContent As String

		For i = 0 To MyDataReader.FieldCount - 1
			iType = MyDataReader.GetFieldType(i).ToString.ToUpper
			Select Case iType.ToUpper

				Case "System.Int16".ToUpper, "System.Int32".ToUpper, "System.Int64".ToUpper, "System.Decimal".ToUpper, "System.Double".ToUpper
					strParam = LlFieldType.Numeric
					strContent = If(IsDBNull(MyDataReader.Item(i)), "", MyDataReader.Item(i).ToString & "")

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


	Sub GetMDUSData4Print()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim recordMandantNumber As Integer = LOPrintSetting.SelectedMDNr

		If Not m_PayrollData Is Nothing Then
			recordMandantNumber = m_PayrollData.MDNr
		End If

		Utilities.GetUSData(LOPrintSetting.DbConnString2Open, _ClsLLFunc, recordMandantNumber)
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
				LL.Variables.Add("BewName", m_mandant.GetSelectedMDProfilValue(LOPrintSetting.SelectedMDNr, Now.Year, "Sonstiges", "BewName", ""))
				LL.Variables.Add("BewStr", m_mandant.GetSelectedMDProfilValue(LOPrintSetting.SelectedMDNr, Now.Year, "Sonstiges", "BewStrasse", ""))
				LL.Variables.Add("BewOrt", m_mandant.GetSelectedMDProfilValue(LOPrintSetting.SelectedMDNr, Now.Year, "Sonstiges", "BewPLZOrt", ""))
				LL.Variables.Add("BewNameAus", m_mandant.GetSelectedMDProfilValue(LOPrintSetting.SelectedMDNr, Now.Year, "Sonstiges", "BewSeco", ""))
				LL.Variables.Add("MwStProzent", m_mandant.GetSelectedMDProfilValue(LOPrintSetting.SelectedMDNr, Now.Year, "Debitoren", "MWST-Satz", ""))

			End With

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Private Function LoadPayrollData(ByVal payrollNumber As Integer) As Boolean
		Dim result As Boolean = True

		m_PayrollData = m_PayrollDatabaseAccess.LoadPayrollMasterData(payrollNumber)

		If m_PayrollData Is Nothing Then
			m_utilityUI.ShowErrorDialog("Lohn-Daten für Mandant-Spezifikationen konnten nicht geladen werden.")
		End If


		Return (Not m_PayrollData Is Nothing)

	End Function

	Sub InitLL()
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

	Sub SetLLVariable()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim aValue As New List(Of String)

		Dim printfeiertaginpayslip As Boolean? = Nothing
		If Not String.IsNullOrWhiteSpace(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/printfeiertaginpayslip", FORM_XML_DEFAULTVALUES_KEY))) Then
			printfeiertaginpayslip = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/printfeiertaginpayslip", FORM_XML_DEFAULTVALUES_KEY)), True)
		End If
		Dim printFerienInpayslip As Boolean = True
		Dim print13LohnInpayslip As Boolean = True
		Dim printDarleheninpayslip As Boolean = True
		Dim printGleitStdinpayslip As Boolean = True
		Dim printNightStdinpayslip As Boolean = True
		Dim sortkwlanrinpayslip As Boolean = False

		Dim minAmountfeiertaginpayslip As Decimal = 0
		Dim minAmountFerienInpayslip As Decimal = 0
		Dim minAmount13LohnInpayslip As Decimal = 0
		Dim minAmountDarleheninpayslip As Decimal = 0
		Dim minAmountGleitStdinpayslip As Decimal = 0
		Dim minAmountNightStdinpayslip As Decimal = 0

		If printfeiertaginpayslip.HasValue Then
			printfeiertaginpayslip = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/printfeiertaginpayslip", FORM_XML_DEFAULTVALUES_KEY)), True)
			minAmountfeiertaginpayslip = ParseToDec(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/minAmountfeiertaginpayslip", FORM_XML_DEFAULTVALUES_KEY)), 0)

			printFerienInpayslip = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/printferieninpayslip", FORM_XML_DEFAULTVALUES_KEY)), True)
			minAmountFerienInpayslip = ParseToDec(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/minAmountFerienInpayslip", FORM_XML_DEFAULTVALUES_KEY)), 0)

			print13LohnInpayslip = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/print13lohninpayslip", FORM_XML_DEFAULTVALUES_KEY)), True)
			minAmount13LohnInpayslip = ParseToDec(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/minAmount13LohnInpayslip", FORM_XML_DEFAULTVALUES_KEY)), 0)

			printDarleheninpayslip = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/printdarleheninpayslip", FORM_XML_DEFAULTVALUES_KEY)), True)
			minAmountDarleheninpayslip = ParseToDec(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/minAmountDarleheninpayslip", FORM_XML_DEFAULTVALUES_KEY)), 0)

			printGleitStdinpayslip = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/printgleitstdinpayslip", FORM_XML_DEFAULTVALUES_KEY)), True)
			minAmountGleitStdinpayslip = ParseToDec(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/minAmountGleitStdinpayslip", FORM_XML_DEFAULTVALUES_KEY)), 0)

			printNightStdinpayslip = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/printnightstdinpayslip", FORM_XML_DEFAULTVALUES_KEY)), True)
			minAmountNightStdinpayslip = ParseToDec(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/minAmountNightStdinpayslip", FORM_XML_DEFAULTVALUES_KEY)), 0)

		Else
			printfeiertaginpayslip = CBool(mSP_utility.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, String.Format("{0}/printfeiertaginlo".ToLower, FORM_XML_PRINT_PAYROLL_KEY), "0"))
			printFerienInpayslip = CBool(mSP_utility.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, String.Format("{0}/printferieninlo".ToLower, FORM_XML_PRINT_PAYROLL_KEY), "0"))
			print13LohnInpayslip = CBool(mSP_utility.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, String.Format("{0}/print13lohninlo".ToLower, FORM_XML_PRINT_PAYROLL_KEY), "0"))
			printDarleheninpayslip = CBool(mSP_utility.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, String.Format("{0}/printdarleheninlo".ToLower, FORM_XML_PRINT_PAYROLL_KEY), "0"))
			printGleitStdinpayslip = CBool(mSP_utility.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, String.Format("{0}/printgleitstdinlo".ToLower, FORM_XML_PRINT_PAYROLL_KEY), "0"))
			printNightStdinpayslip = CBool(mSP_utility.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, String.Format("{0}/printnightstdinlo".ToLower, FORM_XML_PRINT_PAYROLL_KEY), "0"))

		End If

		LL.Variables.Add("DTABCNr", String.Empty)
		LL.Variables.Add("KONTONR", String.Empty)
		LL.Variables.Add("IBAN_MA", String.Empty)
		LL.Variables.Add("Swift_MA", String.Empty)
		LL.Variables.Add("DTAADR1", String.Empty)
		LL.Variables.Add("BANKORT", String.Empty)
		LL.Variables.Add("BANK", String.Empty)

		' Zusätzliche Variable einfügen
		LL.Variables.Add("AutorFName", ClsMainSetting.UserData.UserFName)
		LL.Variables.Add("AutorLName", ClsMainSetting.UserData.UserLName)
		LL.Variables.Add("USSignFilename", _ClsLLFunc.strUSSignFilename)

		LL.Variables.Add("SelLONr", LOPrintSetting.SelectedLONr2Print)
		LL.Variables.Add("SelMonat", LOPrintSetting.SelectedMonth)
		LL.Variables.Add("SelJahr", LOPrintSetting.SelectedYear)
		LL.Variables.Add("SelMANr", LOPrintSetting.SelectedMANr2Print)
		LL.Variables.Add("Currency", "CHF")
		LL.Variables.Add("LOCreatedOn", LOPrintSetting.LoCreatedOn)
		LL.Variables.Add("LOCreatedFrom", LOPrintSetting.LoCreatedFrom)

		LL.Variables.Add("Feier_Guthaben", 0)
		LL.Variables.Add("Feier_Guthaben_1", 0)
		LL.Variables.Add("Fer_Guthaben", 0)
		LL.Variables.Add("Fer_Guthaben_1", 0)
		LL.Variables.Add("Guthaben_13Lohn", 0)
		LL.Variables.Add("Guthaben_13Lohn_1", 0)
		LL.Variables.Add("Guthaben_Darlehen", 0)
		LL.Variables.Add("Guthaben_GStd", 0)
		LL.Variables.Add("Guthaben_GBtrag", 0)
		LL.Variables.Add("Guthaben_NStd", 0)
		LL.Variables.Add("Guthaben_NBtrag", 0)


		Try
			Dim Conn As New SqlConnection(m_connectionString)
			Conn.Open()
			Dim sSql As String = "[Get BnkData For Selected MA In LO]"
			Dim cmd As SqlCommand = New SqlCommand(sSql, Conn)
			cmd.CommandType = CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@LONr", LOPrintSetting.SelectedLONr2Print)
			param = cmd.Parameters.AddWithValue("@MANr", LOPrintSetting.SelectedMANr2Print)
			Dim rTemprec As SqlDataReader = cmd.ExecuteReader

			Try
				While rTemprec.Read
					Dim strValue As String = GetColumnTextStr(rTemprec, "BnkAu", "")
					If CBool(strValue) Then strValue = GetColumnTextStr(rTemprec, "BLZ", "") & " " Else strValue = ""
					LL.Variables.Add("DTABCNr", String.Format("{0}{1}", strValue,
																											 GetColumnTextStr(rTemprec, "DtaBCNr", "")))
					LL.Variables.Add("KONTONR", String.Format("{0}", GetColumnTextStr(rTemprec, "KontoNr", "")))
					LL.Variables.Add("IBAN_MA", String.Format("{0}", GetColumnTextStr(rTemprec, "IBANNr", "")))
					LL.Variables.Add("Swift_MA", String.Format("{0}", GetColumnTextStr(rTemprec, "Swift", "")))
					LL.Variables.Add("DTAADR1", String.Format("{0}", GetColumnTextStr(rTemprec, "DTAADR1", "")))
					LL.Variables.Add("BANKORT", String.Format("{0}", GetColumnTextStr(rTemprec, "BANKORT", "")))
					LL.Variables.Add("BANK", String.Format("{0}", GetColumnTextStr(rTemprec, "BANK", "")))

				End While

			Catch ex As Exception
				m_Logger.LogError(String.Format("Lesen der Kandidaten Bankdaten.{0}: {1}", strMethodeName, ex.ToString))

			End Try
			rTemprec.Close()

		Catch ex As Exception
			m_Logger.LogError(String.Format("Öffnen der Kandidaten Bankdaten.{0}: {1}", strMethodeName, ex.ToString))

		End Try

		Try
			Dim dGuthaben As Decimal = 0
			Dim sAnzahl As Single = 0
			Dim dBetrag As Decimal = 0

			If printfeiertaginpayslip Then
				Try
					dGuthaben = GetFeierGuthabenProLO(LOPrintSetting.SelectedMDNr, LOPrintSetting.SelectedMANr2Print, LOPrintSetting.SelectedMonth, LOPrintSetting.SelectedYear)
					LL.Variables.Add("Feier_Guthaben", If(Math.Abs(dGuthaben) > minAmountfeiertaginpayslip, dGuthaben, 0))

					dGuthaben = GetLOFeierGuthabenIndividuell(LOPrintSetting.SelectedMANr2Print, LOPrintSetting.SelectedMonth, LOPrintSetting.SelectedYear)
					LL.Variables.Add("Feier_Guthaben_1", If(Math.Abs(dGuthaben) > minAmountfeiertaginpayslip, dGuthaben, 0))

				Catch ex As Exception
					m_Logger.LogError(String.Format("Ausgabe von Guthaben der Feiertagsentschädigung auf Lohnabrechnung.{0}: {1}", strMethodeName, ex.ToString))

				End Try

			End If
			dGuthaben = 0
			If printFerienInpayslip Then
				Try
					dGuthaben = GetFerGuthabenProLO(LOPrintSetting.SelectedMDNr, LOPrintSetting.SelectedMANr2Print, LOPrintSetting.SelectedMonth, LOPrintSetting.SelectedYear)
					LL.Variables.Add("Fer_Guthaben", If(Math.Abs(dGuthaben) > minAmountFerienInpayslip, dGuthaben, 0))

					dGuthaben = GetLOFerGuthabenIndividuell(LOPrintSetting.SelectedMANr2Print, LOPrintSetting.SelectedMonth, LOPrintSetting.SelectedYear)
					LL.Variables.Add("Fer_Guthaben_1", If(Math.Abs(dGuthaben) > minAmountFerienInpayslip, dGuthaben, 0))

				Catch ex As Exception
					m_Logger.LogError(String.Format("Ausgabe von Guthaben der Ferienentschädigung auf Lohnabrechnung.{0}: {1}", strMethodeName, ex.ToString))

				End Try

			End If
			dGuthaben = 0
			If print13LohnInpayslip Then
				Try
					dGuthaben = Get13LohnGuthabenProLO(LOPrintSetting.SelectedMDNr, LOPrintSetting.SelectedMANr2Print, LOPrintSetting.SelectedMonth, LOPrintSetting.SelectedYear)
					LL.Variables.Add("Guthaben_13Lohn", If(Math.Abs(dGuthaben) > minAmount13LohnInpayslip, dGuthaben, 0))

					dGuthaben = GetLO13GuthabenIndividuell(LOPrintSetting.SelectedMANr2Print, LOPrintSetting.SelectedMonth, LOPrintSetting.SelectedYear)
					LL.Variables.Add("Guthaben_13Lohn_1", If(Math.Abs(dGuthaben) > minAmount13LohnInpayslip, dGuthaben, 0))

				Catch ex As Exception
					m_Logger.LogError(String.Format("Ausgabe von Guthaben der 13. Lohn auf Lohnabrechnung.{0}: {1}", strMethodeName, ex.ToString))

				End Try

			End If
			dGuthaben = 0
			If printDarleheninpayslip Then
				Try
					dGuthaben = GetDarlehenGuthabenProLO(LOPrintSetting.SelectedMDNr, LOPrintSetting.SelectedMANr2Print, LOPrintSetting.SelectedMonth, LOPrintSetting.SelectedYear)
					LL.Variables.Add("Guthaben_Darlehen", If(Math.Abs(dGuthaben) > minAmountDarleheninpayslip, dGuthaben, 0))

				Catch ex As Exception
					m_Logger.LogError(String.Format("Ausgabe von Guthaben der Darlehn auf Lohnabrechnung.{0}: {1}", strMethodeName, ex.ToString))

				End Try

			End If
			dGuthaben = 0
			If printGleitStdinpayslip Then
				Try
					sAnzahl = 0
					dBetrag = 0

					'dGuthaben = GetAnzGStdProLO(LOPrintSetting.SelectedMDNr, LOPrintSetting.SelectedMANr2Print, LOPrintSetting.SelectedMonth, LOPrintSetting.SelectedYear, sAnzahl, dBetrag)
					Dim data = m_ListingDatabaseAccess.LoadFlexibleWorkingHoursForPayrollData(LOPrintSetting.SelectedMDNr, LOPrintSetting.SelectedMANr2Print, LOPrintSetting.SelectedYear, LOPrintSetting.SelectedMonth) ', sAnzahl, dBetrag)
					If Not data Is Nothing Then
						sAnzahl = data.CreditHours
						dBetrag = data.CreditAmount
					End If
					LL.Variables.Add("Guthaben_GStd", If(Math.Abs(sAnzahl) > minAmountGleitStdinpayslip, sAnzahl, 0))
					LL.Variables.Add("Guthaben_GBtrag", dBetrag)

				Catch ex As Exception
					m_Logger.LogError(String.Format("Ausgabe von Guthaben der Gleitzeit auf Lohnabrechnung.{0}: {1}", strMethodeName, ex.ToString))

				End Try

			End If
			dGuthaben = 0
			sAnzahl = 0
			dBetrag = 0
			If printNightStdinpayslip Then
				Try
					dGuthaben = GetNightGStdProLO(LOPrintSetting.SelectedMDNr, LOPrintSetting.SelectedMANr2Print, LOPrintSetting.SelectedMonth, LOPrintSetting.SelectedYear, sAnzahl, dBetrag)
					LL.Variables.Add("Guthaben_NStd", If(Math.Abs(sAnzahl) > minAmountNightStdinpayslip, sAnzahl, 0))
					LL.Variables.Add("Guthaben_NBtrag", dBetrag)

				Catch ex As Exception
					m_Logger.LogError(String.Format("Ausgabe von Guthaben der Nachtzulage auf Lohnabrechnung.{0}: {1}", strMethodeName, ex.ToString))

				End Try

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:Guthaben auswerten.{1}", strMethodeName, ex.ToString))

		End Try

		' Mandantendaten drucken...
		GetMDUSData4Print()

	End Sub



#Region "Helpers"


	Private Function ParseToBoolean(ByVal stringvalue As String, ByVal value As Boolean?) As Boolean
		Dim result As Boolean
		If (Not Boolean.TryParse(stringvalue, result)) Then
			Return value
		End If
		Return result
	End Function

	Private Function ParseToInteger(ByVal stringvalue As String, ByVal value As Integer?) As Integer
		Dim result As Integer
		If (Not Integer.TryParse(stringvalue, result)) Then
			Return value
		End If
		Return result
	End Function

	Private Function ParseToDec(ByVal stringvalue As String, ByVal value As Decimal?) As Decimal
		Dim result As Decimal
		If (Not Decimal.TryParse(stringvalue, result)) Then
			Return value
		End If
		Return result
	End Function

	Private Function StrToBool(ByVal str As String) As Boolean

		Dim result As Boolean = False

		If String.IsNullOrWhiteSpace(str) Then
			Return False
		End If

		Boolean.TryParse(str, result)

		Return result
	End Function


#End Region


End Class

