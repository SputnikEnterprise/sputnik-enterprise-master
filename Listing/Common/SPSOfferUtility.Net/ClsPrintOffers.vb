

Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath

Imports System.Data.SqlClient

Imports SP.Infrastructure.Initialization
Imports SP.Infrastructure.Logging

Public Class OfferData

	Public Property OfferNumber As Integer
	Public Property CustomerNumber As Integer
	Public Property CResponsibleNumber As Integer
	Public Property EmployeeNumber As Integer

End Class

Public Class CustomerData
	Public Property CustomerNumber As Integer
	Public Property CResponsibleNumber As Integer

End Class


Public Class ClsPrintOffers

	Implements IDisposable
	Protected disposed As Boolean = False

	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	'Private _ClsLLFunc As ClsLLFunc

	Private m_path As New ClsProgPath
	Private m_md As New Mandant
	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private m_utility As SPProgUtility.MainUtilities.Utilities

	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	'Private LL As ListLabel = New ListLabel

	Private Property OfferNr As Integer?
	Private Property MANr As Integer?
	Private Property KDNr As Integer?
	Private Property ZHDNr As Integer?

	Private Property ForExport As Boolean?
	Private Property Modultoprint As String



	Public Sub New(ByVal _Setting As InitializeClass,
								 ByVal iSelectedOfNr As Integer?, ByVal iMANr As Integer?,
														ByVal iKDNr As Integer?, ByVal iZHDNr As Integer?,
														ByVal bForExport As Boolean?, ByVal strModulToPrint As String)

		m_InitializationData = _Setting

		m_utility = New SPProgUtility.MainUtilities.Utilities
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_Setting.TranslationData, _Setting.ProsonalizedData)
		'_ClsLLFunc = New ClsLLFunc

		Try
			OfferNr = iSelectedOfNr
			MANr = iMANr
			KDNr = iKDNr
			ZHDNr = iZHDNr

			ForExport = bForExport
			Modultoprint = strModulToPrint


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:Öffnen der Daten...", ex.ToString))

		End Try

	End Sub


	'Public Shared ReadOnly Property GetLLLicenceInfo() As String
	'	Get
	'		Return "t/lJEQ"
	'	End Get
	'End Property

	Public Shared ReadOnly Property GetAppGuidValue() As String
		Get
			Return "09d3110a-ad4f-4a93-99b4-ba4028206b34"
		End Get
	End Property


	'Private Function BuildPrintJob(ByVal JobNr As String) As Boolean
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim sSql As String = "Select * From DokPrint Where [JobNr] = @JobNr"
	'	Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)
	'	Dim bResult As Boolean = True

	'	If JobNr = String.Empty Then
	'		MsgBox("Sie haben keine Vorlage ausgewählt." & vbCrLf & _
	'					"Bitte wählen Sie aus der Liste eine Vorlage aus.", _
	'					MsgBoxStyle.Critical, "Leere Vorlage")
	'		Return False
	'	End If
	'	Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
	'	Dim param As System.Data.SqlClient.SqlParameter

	'	Try
	'		Conn.Open()

	'		param = cmd.Parameters.AddWithValue("@JobNr", JobNr)
	'		Dim rDocrec As SqlDataReader = cmd.ExecuteReader					' Offertendatenbank
	'		rDocrec.Read()
	'		If rDocrec.HasRows Then

	'			If Not IsDBNull(rDocrec("DocName")) Or (rDocrec("DocName").ToString = String.Empty) Then
	'				_ClsLLFunc.LLDocName = _ClsProgSetting.GetMDDocPath() & rDocrec("DocName").ToString
	'				_ClsLLFunc.LLDocLabel = rDocrec("Bezeichnung").ToString
	'				If IsDBNull(rDocrec("ParamCheck")) Or (rDocrec("ParamCheck").ToString = String.Empty) Then
	'					_ClsLLFunc.LLParamCheck = 0
	'				Else
	'					_ClsLLFunc.LLParamCheck = CInt(IIf(CBool(rDocrec("ParamCheck")), 1, 0))
	'				End If

	'				If IsDBNull(rDocrec("KonvertName")) Or (rDocrec("KonvertName").ToString = String.Empty) Then
	'					_ClsLLFunc.LLKonvertName = 0
	'				Else
	'					_ClsLLFunc.LLKonvertName = CInt(IIf(CBool(rDocrec("KonvertName")), 1, 0))
	'				End If

	'				If IsDBNull(rDocrec("ZoomProz")) Or (rDocrec("ZoomProz").ToString = String.Empty) Then
	'					_ClsLLFunc.LLParamCheck = 100
	'				Else
	'					_ClsLLFunc.LLZoomProz = CInt(IIf(CInt(rDocrec("ZoomProz")) = 0, 150, CInt(rDocrec("ZoomProz"))))
	'				End If

	'				If IsDBNull(rDocrec("Anzahlkopien")) Or (rDocrec("Anzahlkopien").ToString = String.Empty) Then
	'					_ClsLLFunc.LLCopyCount = 1
	'				Else
	'					_ClsLLFunc.LLCopyCount = CInt(IIf(CInt(rDocrec("Anzahlkopien")) = 0, 1, CInt(rDocrec("Anzahlkopien"))))
	'				End If

	'				If IsDBNull(rDocrec("TempDocPath")) Or (rDocrec("TempDocPath").ToString = String.Empty) Then
	'					_ClsLLFunc.LLExportedFilePath = _ClsProgSetting.GetSpSFiles2DeletePath
	'				Else
	'					_ClsLLFunc.LLExportedFilePath = rDocrec("TempDocPath").ToString
	'					If Not _ClsLLFunc.LLExportedFilePath.EndsWith("\") AndAlso Directory.Exists(_ClsLLFunc.LLExportedFilePath) Then _ClsLLFunc.LLExportedFilePath &= "\"
	'				End If

	'				If IsDBNull(rDocrec("ExportedFileName")) Or (rDocrec("ExportedFileName").ToString = String.Empty) Then
	'					_ClsLLFunc.LLExportedFileName = Path.GetFileNameWithoutExtension(rDocrec("DocName").ToString)
	'				Else
	'					_ClsLLFunc.LLExportedFileName = Path.GetFileNameWithoutExtension(rDocrec("ExportedFileName").ToString)
	'				End If


	'			Else
	'				_ClsLLFunc.LLDocName = String.Empty
	'				_ClsLLFunc.LLDocLabel = String.Empty
	'				_ClsLLFunc.LLParamCheck = 0
	'				_ClsLLFunc.LLKonvertName = 0

	'				_ClsLLFunc.LLParamCheck = 100
	'				_ClsLLFunc.LLCopyCount = 1
	'				_ClsLLFunc.LLExportedFilePath = _ClsProgSetting.GetSpSFiles2DeletePath
	'				_ClsLLFunc.LLExportedFileName = String.Empty
	'				bResult = False

	'			End If

	'		Else
	'			MsgBox(String.Format("Das gewünschte Dokument konnte nicht gefunden werden. {0}", _
	'													 JobNr), MsgBoxStyle.Exclamation, "Dokument öffnen")
	'			bResult = False
	'		End If
	'		rDocrec.Close()

	'	Catch ex As Exception
	'		MsgBox(String.Format("***Fehler_1 (BuildPrintJob): Fehler bei der Suche nach Dokument ({0}) {1}", _
	'												 JobNr, ex.Message), _
	'												 MsgBoxStyle.Critical, "BuildPrintJob")

	'		bResult = False

	'	Finally
	'		cmd.Dispose()
	'		Conn.Close()

	'	End Try

	'	Return bResult
	'End Function

	'Sub ShowInDesign(ByVal LL As ListLabel, ByVal strJobNr As String, ByVal iOfferNr As Integer, ByVal iKDNr As Integer, ByVal iKDZNr As Integer)
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

	'	Try
	'		If Not BuildPrintJob(Modultoprint) Then Return

	'		Dim rOffrec As SqlDataReader = GetDbInfo()
	'		If IsNothing(rOffrec) Then
	'			MsgBox("Die Offerte ist nicht vollständig ausgefüllt. Bitte kontrollieren sie Ihre Angaben.", _
	'						 MsgBoxStyle.Critical, "GetDbInfo_ShowInPrint")
	'			Exit Sub
	'		End If

	'		InitLL(LL)
	'		LL.Variables.Clear()
	'		LL.Fields.Clear()

	'		DefineData(LL, False, rOffrec)
	'		SetLLVariable(LL, CInt(rOffrec("MANr").ToString))

	'		'LL.Core.LlPrintSetOptionString(LlPrintOptionString.Export, "Tobit FaxWare")
	'		LL.Core.LlDefineLayout(CType(0, IntPtr), _ClsLLFunc.LLDocLabel, LlProject.List, _ClsLLFunc.LLDocName)

	'	Catch LlException As Exception
	'		m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, LlException.Message))

	'		' Catch Exceptions
	'		MessageBox.Show(LlException.Message + vbCrLf + _
	'		"This information was generated by a List & Label custom exception.", _
	'		"Information", MessageBoxButtons.OK, MessageBoxIcon.Information)

	'	Finally

	'	End Try

	'End Sub

	'Function ShowInPrint(ByRef bShowBox As Boolean) As Boolean
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Const LL_PRNOPT_COPIES_SUPPORTED As Integer = 3
	'	Dim bResult As Boolean = True

	'	If Not BuildPrintJob(Modultoprint) Then Return False

	'	Try

	'		Dim rOffrec As SqlDataReader = GetDbInfo()
	'		If IsNothing(rOffrec) Then
	'			MsgBox("Die Offerte ist nicht vollständig ausgefüllt. Bitte kontrollieren sie Ihre Angaben.", _
	'						 MsgBoxStyle.Critical, "GetDbInfo_ShowInPrint")
	'			Return False
	'		End If

	'		InitLL(LL)
	'		LL.Variables.Clear()
	'		LL.Fields.Clear()

	'		DefineData(LL, False, rOffrec)
	'		SetLLVariable(LL, CInt(rOffrec("MANr").ToString))

	'		LL.Core.LlPrintWithBoxStart(LlProject.List, _ClsLLFunc.LLDocName, LlPrintMode.Export, _
	'																LlBoxType.StandardAbort, CType(0, IntPtr), _ClsLLFunc.LLDocLabel)

	'		LL.Core.LlPrintSetOption(LL_PRNOPT_COPIES_SUPPORTED, _ClsLLFunc.LLCopyCount)
	'		LL.Core.LlPrintSetOption(LL_PRNOPT_COPIES_SUPPORTED, LL.Core.LlPrintGetOption(LlPrintOption.Copies))

	'		If bShowBox Then
	'			LL.Core.LlPrintOptionsDialog(CType(0, IntPtr), _ClsLLFunc.LLDocLabel & vbCrLf & _ClsLLFunc.LLDocName)
	'		End If
	'		Dim TargetFormat As String = LL.Core.LlPrintGetOptionString(LlPrintOptionString.Export)
	'		'LL.Core.LlPrintSetOptionString(LlPrintOptionString.Export, "MSFAX")

	'		While Not LL.Core.LlPrint()
	'		End While

	'		Do
	'			' pass data for current record
	'			DefineData(LL, True, rOffrec)

	'			GetMASBerufe(CInt(rOffrec("MANr").ToString))
	'			LL.Variables.Add("MAsBerufe", _ClsLLFunc.SelectedMAsBerufe)

	'			' Print table line, check return value and abort printing or wrap pages
	'			' if neccessary
	'			While LL.Core.LlPrintFields() = LlConstants.WrnRepeatData
	'				LL.Core.LlPrint()
	'			End While

	'			' Skip to next record}
	'		Loop While rOffrec.Read()

	'		' Finish printing the table, print linked objects}
	'		While LL.Core.LlPrintFieldsEnd() = LlConstants.WrnRepeatData
	'		End While

	'		LL.Core.LlPrintEnd(0)

	'		If TargetFormat = "PRV" Then
	'			LL.Core.LlPreviewDisplay(_ClsLLFunc.LLDocName, _ClsProgSetting.GetSpSTempPath, CType(0, IntPtr))
	'			LL.Core.LlPreviewDeleteFiles(_ClsLLFunc.LLDocName, _ClsProgSetting.GetSpSTempPath)
	'			bResult = False
	'		End If


	'	Catch LlException As Exception
	'		m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, LlException.Message))

	'		If Err.Number <> (LlError.LL_ERR_USER_ABORTED) And Err.Number <> 5 Then
	'			MessageBox.Show(LlException.Message + vbCrLf, _
	'			"Errorcorde: " & Err.Number, MessageBoxButtons.OK, MessageBoxIcon.Information)
	'		End If
	'		bResult = False

	'	Finally

	'	End Try

	'	Return bResult
	'End Function

	'Function ExportLLDoc() As String
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

	'	'If Not BuildPrintJob(strJobNr) Then Return False
	'	Dim strLLTemplate As String = _ClsLLFunc.LLDocName
	'	Dim i As Integer = 0
	'	Dim bResult As String = "Success"

	'	Try
	'		If Not BuildPrintJob(Modultoprint) Then Return String.Empty

	'		InitLL(LL)
	'		Dim rOffrec As SqlDataReader = GetDbInfo()

	'		If rOffrec Is Nothing Then Return String.Empty
	'		LL.Variables.Clear()
	'		LL.Fields.Clear()

	'		DefineData(LL, False, rOffrec)
	'		SetLLVariable(LL, CInt(rOffrec("MANr").ToString))

	'		LL.ExportOptions.Clear()
	'		SetExportSetting(0)

	'		LL.Core.LlPrintWithBoxStart(LlProject.List, _ClsLLFunc.LLDocName, LlPrintMode.Export, _
	'																LlBoxType.StandardAbort, CType(0, IntPtr), _ClsLLFunc.LLDocLabel)

	'		' neue ab 13.9.2011
	'		Dim strDestPath As String = _ClsProgSetting.GetSpSFiles2DeletePath
	'		Dim strTransferValue As String = String.Empty

	'		Dim strfilename As String = _ClsLLFunc.LLExportedFileName & "." & _ClsLLFunc.LLExporterName
	'		If strfilename = String.Empty Then strfilename = String.Format("Offerblatt_{0}.PDF", OfferNr)
	'		If strfilename.Contains("{0}") Then strfilename = String.Format(strfilename, OfferNr)

	'		Dim strDestFullfilename As String = strDestPath & strfilename
	'		If File.Exists(strDestFullfilename) Then File.Delete(strDestFullfilename)

	'		LL.Core.LlPrintSetOptionString(LlPrintOptionString.Export, _ClsLLFunc.LLExporterName)
	'		LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.File", strfilename)
	'		LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.Path", strDestPath)
	'		LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.Quiet", "1")

	'		LL.Core.LlPrintSetOption(LlPrintOption.Dialog_DestinationMask, 2)
	'		Dim TargetFormat As String = LL.Core.LlPrintGetOptionString(LlPrintOptionString.Export)

	'		While Not LL.Core.LlPrint()
	'		End While

	'		Do
	'			' pass data for current record
	'			DefineData(LL, True, rOffrec)
	'			GetMASBerufe(CInt(rOffrec("MANr").ToString))
	'			LL.Variables.Add("MAsBerufe", _ClsLLFunc.SelectedMAsBerufe)

	'			' Print table line, check return value and abort printing or wrap pages
	'			' if neccessary
	'			While LL.Core.LlPrintFields() = LlConstants.WrnRepeatData
	'				LL.Core.LlPrint()
	'			End While

	'			' Skip to next record}
	'		Loop While rOffrec.Read()

	'		' Finish printing the table, print linked objects}
	'		While LL.Core.LlPrintFieldsEnd() = LlConstants.WrnRepeatData
	'		End While

	'		LL.Core.LlPrintEnd(0)

	'		If TargetFormat = "PRV" Then
	'			LL.Core.LlPreviewDisplay(_ClsLLFunc.LLDocName, _ClsProgSetting.GetSpSTempPath, CType(0, IntPtr))
	'			LL.Core.LlPreviewDeleteFiles(_ClsLLFunc.LLDocName, _ClsProgSetting.GetSpSTempPath)
	'		End If
	'		ClsOfDetails.GetExportedFileName = strDestFullfilename
	'		bResult = ClsOfDetails.GetExportedFileName

	'	Catch LlException As Exception
	'		m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, LlException.Message))

	'		If Err.Number <> (LlError.LL_ERR_USER_ABORTED) And Err.Number <> 5 Then
	'			MessageBox.Show(LlException.Message + vbCrLf, _
	'											"Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
	'		End If
	'		bResult = String.Format("Error: {0}", LlException.Message)

	'	Finally

	'	End Try

	'	Return bResult
	'End Function



#Region "Helpers"

	'Sub InitLL(ByVal LL As ListLabel)
	'	Const LL_DLGBOXMODE_3DBUTTONS As Integer = 32768											 ' 0x8000
	'	Const LL_DLGBOXMODE_ALT10 As Integer = 11															 ' 0x000B
	'	Const LL_OPTION_INCLUDEFONTDESCENT As Integer = 79										 ' 79
	'	Const LL_OPTION_INCREMENTAL_PREVIEW As Integer = 135									 ' 135

	'	Const LL_OPTION_VARSCASESENSITIVE As Integer = 46											 ' 46
	'	Const LL_OPTION_IMMEDIATELASTPAGE As Integer = 64											 ' 64
	'	Const LL_OPTION_CONVERTCRLF As Integer = 21														 ' 21

	'	Const LL_OPTION_NOPARAMETERCHECK As Integer = 32											 ' 32
	'	Const LL_OPTION_XLATVARNAMES As Integer = 51													 ' 51

	'	Const LL_OPTION_ESC_CLOSES_PREVIEW As Integer = 102										 ' 102
	'	Const LL_OPTION_SUPERVISOR As Integer = 3															 ' 3
	'	Const LL_OPTION_UISTYLE As Integer = 99																 ' 99
	'	Const LL_OPTION_UISTYLE_OFFICE2003 As Integer = 2											 ' 2
	'	Const LL_OPTION_AUTOMULTIPAGE As Integer = 42													 ' 42
	'	Const LL_OPTION_SUPPORTPAGEBREAK As Integer = 10											 ' 10
	'	Const LL_OPTION_PRVZOOM_PERC As Integer = 25													 ' 25

	'	LL.LicensingInfo = GetLLLicenceInfo()

	'	LlCore.LlSetDlgboxMode(LL_DLGBOXMODE_3DBUTTONS + LL_DLGBOXMODE_ALT10)

	'	' beim LL13 muss ich es so machen...
	'	LL.Core.LlSetOption(LL_OPTION_INCLUDEFONTDESCENT, 0)
	'	LL.Core.LlSetOption(LL_OPTION_INCREMENTAL_PREVIEW, 0)

	'	' beim LL13 muss ich es so machen...
	'	LL.Core.LlSetOption(LL_OPTION_INCLUDEFONTDESCENT, _ClsLLFunc.LLFontDesent)
	'	LL.Core.LlSetOption(LL_OPTION_INCREMENTAL_PREVIEW, _ClsLLFunc.LLIncPrv)

	'	LL.Core.LlSetOption(LL_OPTION_VARSCASESENSITIVE, 0)

	'	LL.Core.LlSetOption(LL_OPTION_IMMEDIATELASTPAGE, 1)				' Lastpage
	'	LL.Core.LlSetOption(LL_OPTION_CONVERTCRLF, 1)							' Doppelte Zeilenumbruch

	'	LL.Core.LlSetOption(LL_OPTION_NOPARAMETERCHECK, _ClsLLFunc.LLParamCheck)
	'	LL.Core.LlSetOption(LL_OPTION_XLATVARNAMES, _ClsLLFunc.LLKonvertName)

	'	LL.Core.LlSetOption(LL_OPTION_ESC_CLOSES_PREVIEW, 1)
	'	LL.Core.LlSetOption(LL_OPTION_SUPERVISOR, 1)
	'	LL.Core.LlSetOption(LL_OPTION_UISTYLE, LL_OPTION_UISTYLE_OFFICE2003)
	'	LL.Core.LlSetOption(LL_OPTION_AUTOMULTIPAGE, 1)

	'	LL.Core.LlSetOption(LL_OPTION_SUPPORTPAGEBREAK, 1)
	'	LL.Core.LlSetOption(LL_OPTION_PRVZOOM_PERC, _ClsLLFunc.LLZoomProz)

	'	LL.Core.LlPreviewSetTempPath(m_path.GetSpSTempFolder)
	'	LL.Core.LlSetPrinterDefaultsDir(m_path.GetPrinterHomeFolder)

	'End Sub


	'Private Sub SetExportSetting(ByVal iIndex As Short)
	'	Select Case iIndex
	'		Case 0
	'			_ClsLLFunc.LLExportfilter = "PDF Files|*.PDF"
	'			_ClsLLFunc.LLExporterName = "PDF"
	'			_ClsLLFunc.LLExporterFileName = _ClsLLFunc.LLExportedFileName + ".pdf"

	'		Case 1
	'			_ClsLLFunc.LLExportfilter = "MHTML Files|*.mht"
	'			_ClsLLFunc.LLExporterName = "MHTML"
	'			_ClsLLFunc.LLExporterFileName = _ClsLLFunc.LLExportedFileName + ".mht"

	'		Case 2
	'			_ClsLLFunc.LLExportfilter = "HTML Files|*.HTM"
	'			_ClsLLFunc.LLExporterName = "HTML"
	'			_ClsLLFunc.LLExporterFileName = _ClsLLFunc.LLExportedFileName + ".htm"

	'		Case 3
	'			_ClsLLFunc.LLExportfilter = "RTF Files|*.RTF"
	'			_ClsLLFunc.LLExporterName = "RTF"
	'			_ClsLLFunc.LLExporterFileName = _ClsLLFunc.LLExportedFileName + ".rtf"

	'		Case 4
	'			_ClsLLFunc.LLExportfilter = "XML Files|*.XML"
	'			_ClsLLFunc.LLExporterName = "XML"
	'			_ClsLLFunc.LLExporterFileName = _ClsLLFunc.LLExportedFileName + ".xml"

	'		Case 5
	'			_ClsLLFunc.LLExportfilter = "Tiff Files|*.TIF"
	'			_ClsLLFunc.LLExporterName = "PICTURE_MULTITIFF"
	'			_ClsLLFunc.LLExporterFileName = _ClsLLFunc.LLExportedFileName + ".tif"

	'		Case 6
	'			_ClsLLFunc.LLExportfilter = "Text Files|*.TXT"
	'			_ClsLLFunc.LLExporterName = "TXT"
	'			_ClsLLFunc.LLExporterFileName = _ClsLLFunc.LLExportedFileName + ".txt"

	'		Case 7
	'			_ClsLLFunc.LLExportfilter = "Excel Files|*.XLS"
	'			_ClsLLFunc.LLExporterName = "XLS"
	'			_ClsLLFunc.LLExporterFileName = _ClsLLFunc.LLExportedFileName + ".xls"
	'	End Select

	'End Sub

	Private Function GetDbInfo() As SqlDataReader
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim sSql As String = String.Empty
		Dim SelectedOffNumber As Integer? = OfferNr
		Dim bWithoutMA As Boolean

		Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)
		Dim i As Integer = 0
		Dim cmd As SqlCommand
		Dim rOffrec As SqlDataReader

		Try
			Conn.Open()

			sSql = "Select MANr From OFF_MASelection Where OfNr = " & OfferNr

			cmd = New System.Data.SqlClient.SqlCommand(sSql, Conn)
			rOffrec = cmd.ExecuteReader
			rOffrec.Read()
			If rOffrec.HasRows Then
				sSql = "[Get OfferData For Print In MailMerge] "
				bWithoutMA = False

			Else
				sSql = "[Get OfferData For Print In MailMerge Without MA] "
				bWithoutMA = True

			End If
			If ZHDNr.HasValue Then
				sSql += OfferNr & ", " & KDNr & ", " & ZHDNr
			Else
				sSql += OfferNr & ", " & KDNr & ", 0"
			End If
			rOffrec.Close()
			cmd = New System.Data.SqlClient.SqlCommand(sSql, Conn)
			rOffrec = cmd.ExecuteReader
			rOffrec.Read()
			If Not rOffrec.HasRows Then
				rOffrec.Close()
				Conn.Close()

				Return Nothing
			End If
			Return rOffrec

			'End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			MsgBox(String.Format("***Fehler_1 (GetDbInfo): Fehler bei der Suche nach Daten. JobNr: {0}{1}",
													 Modultoprint, vbNewLine & ex.Message),
													 MsgBoxStyle.Critical, "GetDbInfo")

		Finally
			'    Conn.Close()

		End Try

		Return Nothing

	End Function

#End Region

	Protected Overridable Overloads Sub Dispose(
	 ByVal disposing As Boolean)
		If Not Me.disposed Then
			If disposing Then

			End If
			' Add code here to release the unmanaged resource.
			'LL.Dispose()
			'LL.Core.Dispose()
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

End Class
