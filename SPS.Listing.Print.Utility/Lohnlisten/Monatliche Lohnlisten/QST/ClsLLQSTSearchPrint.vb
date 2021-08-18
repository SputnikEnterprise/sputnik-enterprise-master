
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath

Imports System.Data.SqlClient
Imports System.IO
Imports combit.ListLabel25
Imports SPS.Listing.Print.Utility.MainUtilities

Imports SP.Infrastructure.Logging

Public Class ClsLLQSTSearchPrint
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

	Private m_path As New ClsProgPath
	Private m_md As New Mandant
	Private _QstListPrintSetting As New ClsLLQSTSearchPrintSetting

	Private strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	Private _ClsReg As New SPProgUtility.ClsDivReg
	Private _ClsLog As New SPProgUtility.ClsEventLog
	Friend _ClsLLFunc As New ClsLLFunc

	Private strConnString As String = String.Empty

	Private Property USSignFileName As String
	Private Property ExistsDocFile As Boolean

	Private LL As New ListLabel

#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass, ByVal printSetting As ClsLLQSTSearchPrintSetting)

		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

		Me._QstListPrintSetting = printSetting
		Me.strConnString = printSetting.DbConnString2Open
		Me.ExistsDocFile = MainUtilities.BuildPrintJob(printSetting.SelectedMDNr, printSetting.DbConnString2Open, printSetting.JobNr2Print, _ClsLLFunc)

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


	Sub ShowInDesign()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		If Not Me.ExistsDocFile Then Return

		Try
			Dim rFoundedrec As SqlDataReader = MainUtilities.OpenDb4PrintMonthlyTaxListing(strConnString, m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr)
			If Not rFoundedrec.HasRows Then
				m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, "Keine Daten wurden gefunden..."))

				Return
			End If

			InitLL(LL)
			LL.Variables.Clear()
			LL.Fields.Clear()

			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				DefineData(LL, True, rFoundedrec)
			Else
				DefineData(LL, False, rFoundedrec)
			End If
			SetLLVariable(LL)

			LL.Core.LlDefineLayout(CType(_QstListPrintSetting.frmhwnd, IntPtr), _ClsLLFunc.LLDocLabel, If(_ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper), LlProject.List, LlProject.Card), _ClsLLFunc.LLDocName)
			rFoundedrec.Close()

		Catch LlException As Exception
			m_Logger.LogError(String.Format("LlException.{0}: {1}", strMethodeName, LlException.Message))

		Finally

		End Try

	End Sub

	Function ShowInPrint(ByRef bShowBox As Boolean) As String
		Const LL_PRNOPT_COPIES_SUPPORTED As Integer = 3
		Dim strResult As String = "Success"
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

		Dim strJobNr As String = Me._QstListPrintSetting.JobNr2Print

		Dim strQuery As String = _QstListPrintSetting.SQL2Open
		Dim Conn As SqlConnection = New SqlConnection(strConnString)
		Dim i As Integer = 0

		If Not Me.ExistsDocFile Then Return strResult

		Try
			Dim rFoundedrec As SqlDataReader = MainUtilities.OpenDb4PrintMonthlyTaxListing(strConnString, m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr)
			If Not rFoundedrec.HasRows Then Return m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden") & "..."

			InitLL(LL)
			LL.Variables.Clear()
			LL.Fields.Clear()

			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				DefineData(LL, True, rFoundedrec)
			Else
				DefineData(LL, False, rFoundedrec)
			End If
			SetLLVariable(LL)
			LL.Core.LlPrintWithBoxStart(If(_ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper),
																		 LlProject.List, LlProject.Card),
																		 _ClsLLFunc.LLDocName, LlPrintMode.Export, LlBoxType.StandardAbort,
																		 CType(_QstListPrintSetting.frmhwnd, IntPtr), _ClsLLFunc.LLDocLabel)

			LL.Core.LlPrintSetOption(LlPrintOption.Copies, _ClsLLFunc.LLCopyCount)
			LL.Core.LlPrintSetOption(LL_PRNOPT_COPIES_SUPPORTED, LL.Core.LlPrintGetOption(LlPrintOption.Copies))

			If bShowBox Then
				LL.Core.LlPrintOptionsDialog(CType(_QstListPrintSetting.frmhwnd, IntPtr), String.Format("{1}: {2}{0}{3}", vbNewLine, _ClsLLFunc.LLDocLabel, strJobNr, _ClsLLFunc.LLDocName))
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
			LL.Core.LlPrintEnd(0)

			If TargetFormat = "PRV" Then
				LL.Core.LlPreviewDisplay(_ClsLLFunc.LLDocName,
																 m_path.GetSpSTempFolder, CType(_QstListPrintSetting.frmhwnd, IntPtr))
				LL.Core.LlPreviewDeleteFiles(_ClsLLFunc.LLDocName, m_path.GetSpSTempFolder)
				Return "Error"
			End If
			rFoundedrec.Close()

		Catch LlException As LL_User_Aborted_Exception
			Return "Error"

		Catch LlException As Exception
			m_Logger.LogError(String.Format("LlException.{0}: {1}", strMethodeName, LlException.Message))
			strResult = String.Format("Error: {0}", LlException.Message)

		Finally

		End Try

		Return strResult
	End Function

	Function ExportLLDoc(ByVal LL As ListLabel, ByVal strJobNr As String, ByVal iExportMode As Short,
					   ByVal iProposeNr As Integer) As Boolean

		Dim strLLTemplate As String = _ClsLLFunc.LLDocName
		Dim bResult As Boolean = True
		If Not Me.ExistsDocFile Then Return bResult

		Dim strQuery As String = _QstListPrintSetting.SQL2Open
		Dim Conn As SqlConnection = New SqlConnection(strConnString)
		Dim i As Integer = 0

		Try
			Conn.Open()
			Dim cmd As System.Data.SqlClient.SqlCommand
			cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
			cmd.CommandType = CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@iProposeNr", iProposeNr)

			Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader
			rFoundedrec.Read()
			If IsNothing(rFoundedrec) Then
				Dim strMessage As String = "Die gesuchten Daten sind nicht vorhanden. Bitte kontrollieren sie Ihre Angaben."
				MsgBox(TranslateMyText(strMessage),
			   MsgBoxStyle.Critical, m_Translate.GetSafeTranslationValue("Daten suchen"))
				Return bResult
			End If

			InitLL(LL)
			LL.Variables.Clear()
			LL.Fields.Clear()

			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				DefineData(LL, True, rFoundedrec)
			Else
				DefineData(LL, False, rFoundedrec)
			End If
			SetLLVariable(LL)

			LL.ExportOptions.Clear()
			SetExportSetting(iExportMode)

			LL.Core.LlPrintWithBoxStart(If(_ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper), LlProject.List, LlProject.Card), _ClsLLFunc.LLDocName, LlPrintMode.Export, LlBoxType.StandardAbort, CType(_QstListPrintSetting.frmhwnd, IntPtr), _ClsLLFunc.LLDocLabel)

			LL.Core.LlPrintSetOptionString(LlPrintOptionString.Export, _ClsLLFunc.LLExporterName)
			LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.File", _ClsLLFunc.LLExporterFileName)
			LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.Path", m_path.GetSpSOfferHomeFolder)
			LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.Quiet", "1")

			LL.Core.LlPrintSetOption(LlPrintOption.Dialog_DestinationMask, 2)
			Dim TargetFormat As String = LL.Core.LlPrintGetOptionString(LlPrintOptionString.Export)

			While Not LL.Core.LlPrint()
			End While

			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				Do
					' pass data for current record
					DefineData(LL, True, rFoundedrec)

					While LL.Core.LlPrintFields() = LlConstants.WrnRepeatData ' LlConst.LL_WRN_REPEAT_DATA
						LL.Core.LlPrint()
					End While

				Loop While rFoundedrec.Read()

				While LL.Core.LlPrintFieldsEnd() = LlConstants.WrnRepeatData ' LlConst.LL_WRN_REPEAT_DATA
				End While
			End If
			LL.Core.LlPrintEnd(0)

			If TargetFormat = "PRV" Then
				LL.Core.LlPreviewDisplay(_ClsLLFunc.LLDocName, m_path.GetSpSTempFolder, CType(_QstListPrintSetting.frmhwnd, IntPtr))
				LL.Core.LlPreviewDeleteFiles(_ClsLLFunc.LLDocName, m_path.GetSpSTempFolder)
			End If
			'      _ClsLLFunc.LLExportFileName = String.Format("{0}{1}", m_path.GetSpSOfferHomeFolder, _ClsLLFunc.LLExporterFileName)

		Catch LlException As Exception
			m_Logger.LogError(String.Format("LlException.Message:{0}This information was generated by a List & Label custom exception.", vbNewLine))

			bResult = False

		Finally

		End Try

		Return bResult
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

		' Define data
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

		MainUtilities.GetUSData(Me._QstListPrintSetting.DbConnString2Open, _ClsLLFunc, _QstListPrintSetting.SelectedMDNr)
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
			LL.Variables.Add("BewName", _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, "Sonstiges", "BewName"))
			LL.Variables.Add("BewStr", _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, "Sonstiges", "BewStrasse"))
			LL.Variables.Add("BewOrt", _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, "Sonstiges", "BewPLZOrt"))
			LL.Variables.Add("BewNameAus", _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, "Sonstiges", "BewSeco"))
			LL.Variables.Add("MwStProzent", _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, "Debitoren", "MWST-Satz"))

		End With

	End Sub

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

		LL.Core.LlPreviewSetTempPath(m_path.GetSpSTempFolder)
		LL.Core.LlSetPrinterDefaultsDir(m_path.GetPrinterHomeFolder)

	End Sub

	Sub SetLLVariable(ByVal LL As ListLabel)
		Dim aValue As New List(Of String)

		' Zusätzliche Variable einfügen
		LL.Variables.Add("AutorFName", _ClsProgSetting.GetUserFName)
		LL.Variables.Add("AutorLName", _ClsProgSetting.GetUserLName)
		LL.Variables.Add("USSignFilename", Me._QstListPrintSetting.USSignFileName)

		LL.Variables.Add("SortBez", _QstListPrintSetting.ListSortBez)

		Dim filterLabel As String = String.Empty
		For Each itm In _QstListPrintSetting.ListFilterBez
			If Not String.IsNullOrWhiteSpace(itm) Then filterLabel &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(filterLabel), "", vbNewLine), itm)
		Next
		LL.Variables.Add("FilterBez", filterLabel)

		LL.Variables.Add("ListBez2Print", _QstListPrintSetting.ListBez2Print)
		LL.Variables.Add("LLPrintDiffColor", _ClsLLFunc.LLPrintInDiffColor())

		LL.Variables.Add("SetEmptyGemeindeWithCity", _QstListPrintSetting.SetEmptyGemeindeWithCity)
		LL.Variables.Add("HideBruttolohnColumn", _QstListPrintSetting.HideBruttolohnColumn)
		LL.Variables.Add("HideQSTBasisColumn", _QstListPrintSetting.HideQSTBasisColumn)
		LL.Variables.Add("HideQSTBasis2Column", _QstListPrintSetting.HideQSTBasis2Column)

		' Mandantendaten drucken...
		GetMDUSData4Print(LL)
		GetQSTAddress(LL)

	End Sub

	''' <summary>
	''' Get QST-Address from Database
	''' </summary>
	''' <param name="ll"></param>
	''' <remarks></remarks>
	Sub GetQSTAddress(ByVal ll As ListLabel)
		Dim conn As SqlConnection = New SqlConnection(_ClsProgSetting.GetConnString)
		Dim sql As String = "" '"Declare @Kanton nvarchar(2); "

		sql &= "SELECT TOP 1 Gemeinde As QSTGemeinde, Adresse1 As QSTAdresse, Zusatz As QSTZusatz, "
		sql += "ZHD As QSTZHD, Postfach As QSTPostfach, Strasse As QSTStrasse, Land As QSTLand, Ort As QSTOrt, PLZ As QSTPLZ, "
		sql += "StammNr As QSTStammNr, Provision As QSTProvision, Kanton As QSTKanton "
		sql += "FROM MD_QstAddress WHERE MDNr = @MDNr And "
		sql += "((@kanton = '' And MD_QstAddress.Kanton = (Select Top 1 MD_Kanton From Mandanten Order By Jahr Desc)) "
		sql += "   Or MD_QstAddress.Kanton = @kanton) And "
		sql += "(@gemeinde = '' Or MD_QstAddress.Gemeinde = @gemeinde)"

		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sql, conn)
		conn.Open()

		Dim param As System.Data.SqlClient.SqlParameter
		param = cmd.Parameters.AddWithValue("@MDNr", Me._QstListPrintSetting.SelectedMDNr)
		param = cmd.Parameters.AddWithValue("@kanton", Me._QstListPrintSetting.SelectedCanton)
		param = cmd.Parameters.AddWithValue("@gemeinde", Me._QstListPrintSetting.SelectedCommunity)

		Dim reader As SqlDataReader = cmd.ExecuteReader()
		reader.Read()
		If reader.HasRows Then
			If Not IsDBNull(reader("QSTGemeinde")) Then
				Me._QstListPrintSetting.Gemeinde = reader("QSTGemeinde").ToString
			End If
			If Not IsDBNull(reader("QSTAdresse")) Then
				Me._QstListPrintSetting.Adresse = reader("QSTAdresse").ToString
			End If
			If Not IsDBNull(reader("QSTZusatz")) Then
				Me._QstListPrintSetting.Zusatz = reader("QSTZusatz").ToString
			End If
			If Not IsDBNull(reader("QSTZHD")) Then
				Me._QstListPrintSetting.ZHD = reader("QSTZHD").ToString
			End If
			If Not IsDBNull(reader("QSTPostfach")) Then
				Me._QstListPrintSetting.Postfach = reader("QSTPostfach").ToString
			End If
			If Not IsDBNull(reader("QSTStrasse")) Then
				Me._QstListPrintSetting.Strasse = reader("QSTStrasse").ToString
			End If
			If Not IsDBNull(reader("QSTLand")) Then
				Me._QstListPrintSetting.Land = reader("QSTLand").ToString
			End If
			If Not IsDBNull(reader("QSTOrt")) Then
				Me._QstListPrintSetting.Ort = reader("QSTOrt").ToString
			End If
			If Not IsDBNull(reader("QSTPLZ")) Then
				Me._QstListPrintSetting.PLZ = reader("QSTPLZ").ToString
			End If
			If Not IsDBNull(reader("QSTStammNr")) Then
				Me._QstListPrintSetting.StammNr = reader("QSTStammNr").ToString
			End If
			If Not IsDBNull(reader("QSTProvision")) Then
				Me._QstListPrintSetting.Provision = CDec(reader("QSTProvision").ToString)
			End If
			If Not IsDBNull(reader("QSTKanton")) Then
				Me._QstListPrintSetting.Kanton = reader("QSTKanton").ToString
			End If
		End If

		ll.Variables.Add("QSTGemeinde", Me._QstListPrintSetting.Gemeinde)
		ll.Variables.Add("QSTAdresse", Me._QstListPrintSetting.Adresse)
		ll.Variables.Add("QSTZusatz", Me._QstListPrintSetting.Zusatz)
		ll.Variables.Add("QSTZHD", Me._QstListPrintSetting.ZHD)
		ll.Variables.Add("QSTPostfach", Me._QstListPrintSetting.Postfach)
		ll.Variables.Add("QSTStrasse", Me._QstListPrintSetting.Strasse)
		ll.Variables.Add("QSTLand", Me._QstListPrintSetting.Land)
		ll.Variables.Add("QSTOrt", Me._QstListPrintSetting.Ort)
		ll.Variables.Add("QSTPLZ", Me._QstListPrintSetting.PLZ)
		ll.Variables.Add("QSTStammNr", Me._QstListPrintSetting.StammNr)
		ll.Variables.Add("QSTProvision", Me._QstListPrintSetting.Provision)
		ll.Variables.Add("QSTKanton", Me._QstListPrintSetting.Kanton)

		conn.Close()

	End Sub


End Class



