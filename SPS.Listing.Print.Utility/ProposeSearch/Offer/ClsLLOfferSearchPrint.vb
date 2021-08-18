
Imports System.Data.SqlClient
Imports System.IO
Imports combit.ListLabel25
Imports SPS.Listing.Print.Utility.MainUtilities
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath

Imports SP.Infrastructure
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging


Public Class ClsLLOfferSearchPrint

	Implements IDisposable
	Protected disposed As Boolean = False

	Dim m_PrintSetting As New ClsLLOfferSearchPrintSetting

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_initData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	Private _ClsReg As New SPProgUtility.ClsDivReg
	Friend _ClsLLFunc As New ClsLLFunc
	Private m_path As ClsProgPath
	Private m_md As Mandant
	Private m_UtilityUI As UtilityUI

	Private strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

	Private Property USSignFileName As String
	Private Property PrintAsListing As Boolean
	Private Property ExistsDocFile As Boolean
	Private Property ExtraVakFieldData As List(Of String)

	Private LL As ListLabel = New ListLabel



	Function BuildPrintJob() As Boolean
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim sSql As String = "Select * From DokPrint Where [JobNr] = @JobNr"
		Dim Conn As SqlConnection = New SqlConnection(m_PrintSetting.m_initData.MDData.MDDbConn)
		Dim bResult As Boolean = True
		Dim JobNr As String = m_PrintSetting.JobNr2Print

		If JobNr = String.Empty Then
			Dim strMessage As String = "Sie haben keine Vorlage ausgewählt.{0}Bitte wählen Sie aus der Liste eine Vorlage aus."
			m_UtilityUI.ShowErrorDialog(String.Format(TranslateMyText(strMessage), vbNewLine))

			Return False
		End If
		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
		Dim param As System.Data.SqlClient.SqlParameter

		Try
			Conn.Open()

			param = cmd.Parameters.AddWithValue("@JobNr", m_PrintSetting.JobNr2Print)
			Dim rDocrec As SqlDataReader = cmd.ExecuteReader
			rDocrec.Read()
			If Not String.IsNullOrEmpty(rDocrec("DocName").ToString) Then
				_ClsLLFunc.LLDocName = m_md.GetSelectedMDDocPath(m_PrintSetting.m_initData.MDData.MDNr) & rDocrec("DocName").ToString
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
					_ClsLLFunc.LLCopyCount = CInt(IIf(CInt(rDocrec("Anzahlkopien")) = 0, 1, CInt(rDocrec("Anzahlkopien"))))
				End If

				If String.IsNullOrEmpty((rDocrec("TempDocPath").ToString)) Then
					_ClsLLFunc.LLExportedFilePath = m_path.GetSpS2DeleteHomeFolder
				Else
					_ClsLLFunc.LLExportedFilePath = _ClsReg.AddDirSep(rDocrec("TempDocPath").ToString)
				End If

				If String.IsNullOrEmpty(rDocrec("ExportedFileName").ToString) Then
					_ClsLLFunc.LLExportedFileName = Path.GetFileNameWithoutExtension(rDocrec("DocName").ToString)
				Else
					_ClsLLFunc.LLExportedFileName = Path.GetFileNameWithoutExtension(rDocrec("ExportedFileName").ToString)
				End If

				If String.IsNullOrEmpty(rDocrec("DokNameToShow").ToString) Or String.IsNullOrEmpty(rDocrec("DokNameToShow").ToString) Then
					_ClsLLFunc.ListBez = rDocrec("Bezeichnung").ToString
				Else
					_ClsLLFunc.ListBez = rDocrec("DokNameToShow").ToString
				End If

			Else
				_ClsLLFunc.LLDocName = String.Empty
				_ClsLLFunc.LLDocLabel = String.Empty
				_ClsLLFunc.LLParamCheck = 0
				_ClsLLFunc.LLKonvertName = 0

				_ClsLLFunc.LLParamCheck = 100
				_ClsLLFunc.LLCopyCount = 1
				_ClsLLFunc.LLExportedFilePath = m_path.GetPrinterHomeFolder
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
		If Not Me.ExistsDocFile Then Exit Sub

		Try
			Dim rFoundedrec = MainUtilities.LoadOfferDataForTemplate(m_PrintSetting)

			If rFoundedrec Is Nothing Then
				m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, "Keine Daten wurden gefunden..."))
				Return
			Else
				rFoundedrec.Read()
			End If

			InitLL(LL)
			LL.Variables.Clear()
			LL.Fields.Clear()

			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				DefineData(LL, True, rFoundedrec)
			Else
				DefineData(LL, False, rFoundedrec)
			End If
			SetLLVariable(LL, rFoundedrec("MANr"))

			LL.Core.LlDefineLayout(CType(0, IntPtr), _ClsLLFunc.LLDocLabel, _
														 If(_ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper), LlProject.List, LlProject.Card), _
														 _ClsLLFunc.LLDocName)

		Catch LlException As Exception
			m_Logger.LogError(String.Format("LlException.{0}: {1}", strMethodeName, LlException.Message))

		Finally

		End Try

	End Sub

	Function ShowInPrint(ByRef bShowBox As Boolean) As String
		Const LL_PRNOPT_COPIES_SUPPORTED As Integer = 3
		Dim strResult As String = "Success"
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim strJobNr As String = Me.m_PrintSetting.JobNr2Print
		Try
			Dim bAllowedtoDebug As Boolean = CBool(_ClsProgSetting.GetMDProfilValue("Sonstiges", "EnableLLDebug", "0"))
			If bAllowedtoDebug Then
				LL.Debug = LlDebug.LogToFile
				LL.Debug = LlDebug.Enabled
			End If
		Catch ex As Exception
			m_Logger.LogError(String.Format("EnablingLLDebuging.{0}: {1}", strMethodeName, ex.ToString))

		End Try

		Dim i As Integer = 0

		If Not Me.ExistsDocFile Then Return TranslateMyText("Keine Dokumente wurden gefunden.")

		Try
			Dim rFoundedrec = MainUtilities.LoadOfferDataForTemplate(m_PrintSetting)
			If rFoundedrec Is Nothing Then
				m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, "Keine Daten wurden gefunden..."))
				Return "Keine Daten wurden gefunden."
			Else
				rFoundedrec.Read()
			End If

			InitLL(LL)
			LL.Variables.Clear()
			LL.Fields.Clear()


			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				DefineData(LL, True, rFoundedrec)
			Else
				DefineData(LL, False, rFoundedrec)
			End If
			SetLLVariable(LL, rFoundedrec("MAnr"))

			LL.Core.LlPrintWithBoxStart(If(_ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper), _
																		 LlProject.List, LlProject.Card), _
																		 _ClsLLFunc.LLDocName, LlPrintMode.Export, LlBoxType.StandardAbort, _
																		 CType(0, IntPtr), _ClsLLFunc.LLDocLabel)

			LL.Core.LlPrintSetOption(LlPrintOption.Copies, _ClsLLFunc.LLCopyCount)
			LL.Core.LlPrintSetOption(LL_PRNOPT_COPIES_SUPPORTED, LL.Core.LlPrintGetOption(LlPrintOption.Copies))

			If bShowBox Then
				LL.Core.LlPrintOptionsDialog(CType(0, IntPtr), String.Format("{1}: {2}{0}{3}", _
																																	 vbNewLine, _
																																	 _ClsLLFunc.LLDocLabel, _
																																	 strJobNr, _
																																	 _ClsLLFunc.LLDocName))
			End If
			Dim TargetFormat As String = LL.Core.LlPrintGetOptionString(LlPrintOptionString.Export)

			While Not LL.Core.LlPrint()
			End While

			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				Do
					' pass data for current record
					DefineData(LL, True, rFoundedrec)

					Dim jobDescription = GetEmployeeSecJob(rFoundedrec("MANr"), Me.m_PrintSetting)
					LL.Variables.Add("MAsBerufe", jobDescription)

					While LL.Core.LlPrintFields() = LlConstants.WrnRepeatData
						LL.Core.LlPrint()
					End While

				Loop While rFoundedrec.Read()

				While LL.Core.LlPrintFieldsEnd() = LlConstants.WrnRepeatData
				End While
			End If
			LL.Core.LlPrintEnd(0)

			If TargetFormat = "PRV" Then
				LL.Core.LlPreviewDisplay(_ClsLLFunc.LLDocName, m_path.GetSpSTempFolder, CType(0, IntPtr))
				LL.Core.LlPreviewDeleteFiles(_ClsLLFunc.LLDocName, m_path.GetSpSTempFolder)

				Return "Error"
			End If


		Catch LlException As LL_User_Aborted_Exception
			Return "Error"

		Catch LlException As Exception
			m_Logger.LogError(String.Format("LlException.{0}: {1}", strMethodeName, LlException.Message))
			strResult = String.Format("Error: {0}", LlException.Message)

		Finally

		End Try

		Return strResult
	End Function

	Function ExportLLDoc() As String
		If Not Me.ExistsDocFile Then Return TranslateMyText("Keine Dokumente wurden gefunden.")
		Dim strLLTemplate As String = _ClsLLFunc.LLDocName
		Dim strResult As String = "Success..."

		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Dim rFoundedrec As SqlDataReader
			rFoundedrec = MainUtilities.LoadOfferDataForTemplate(m_PrintSetting)
			If rFoundedrec Is Nothing Then
				m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, "Keine Daten wurden gefunden..."))
				Return "Keine Daten wurden gefunden."
			Else
				rFoundedrec.Read()
			End If

			InitLL(LL)
			LL.Variables.Clear()
			LL.Fields.Clear()

			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				DefineData(LL, True, rFoundedrec)
			Else
				DefineData(LL, False, rFoundedrec)
			End If
			SetLLVariable(LL, rFoundedrec("MANr"))

			LL.ExportOptions.Clear()
			SetExportSetting(0)

			LL.Core.LlPrintWithBoxStart(If(_ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper), LlProject.List, LlProject.Card), _
																	_ClsLLFunc.LLDocName, LlPrintMode.Export, LlBoxType.StandardAbort, CType(0, IntPtr), _
																	_ClsLLFunc.LLDocLabel)

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

					Dim jobDescription = GetEmployeeSecJob(rFoundedrec("MANr"), Me.m_PrintSetting)
					LL.Variables.Add("MAsBerufe", jobDescription)

					While LL.Core.LlPrintFields() = LlConstants.WrnRepeatData
						LL.Core.LlPrint()
					End While

				Loop While rFoundedrec.Read()

				While LL.Core.LlPrintFieldsEnd() = LlConstants.WrnRepeatData
				End While
			End If
			LL.Core.LlPrintEnd(0)

			If TargetFormat = "PRV" Then
				LL.Core.LlPreviewDisplay(_ClsLLFunc.LLDocName, m_path.GetSpSTempFolder, CType(0, IntPtr))
				LL.Core.LlPreviewDeleteFiles(_ClsLLFunc.LLDocName, m_path.GetSpSTempFolder)
			End If
			strResult = Path.Combine(m_path.GetSpSOfferHomeFolder, _ClsLLFunc.LLExporterFileName)


		Catch LlException As Exception
			m_Logger.LogError(String.Format("LlException.{0}: {1}", strMethodeName, LlException.Message))
			strResult = String.Empty

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

	Sub DefineData(ByVal LL As ListLabel, ByVal AsFields As Boolean, ByVal MyDataReader As SqlDataReader)
		Dim i As Integer

		Dim iType As String
		Dim strParam As LlFieldType
		Dim strContent As String = String.Empty
		Dim fieldasBitmap As Boolean = False

		' Define data
		For i = 0 To MyDataReader.FieldCount - 1
			iType = MyDataReader.GetFieldType(i).ToString.ToUpper

			fieldasBitmap = False
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

		MainUtilities.GetUSData(m_PrintSetting.m_initData.MDData.MDDbConn, _ClsLLFunc, 0)
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
				LL.Variables.Add("USNatel", .USNatel)

				' Bewilligungsbehörden
				LL.Variables.Add("BewName", _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, "Sonstiges", "BewName"))
				LL.Variables.Add("BewStr", _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, "Sonstiges", "BewStrasse"))
				LL.Variables.Add("BewOrt", _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, "Sonstiges", "BewPLZOrt"))
				LL.Variables.Add("BewNameAus", _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, "Sonstiges", "BewSeco"))
				LL.Variables.Add("MwStProzent", _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, "Debitoren", "MWST-Satz"))

			End With

		Catch ex As Exception
			m_Logger.LogError(String.Format("LlException.{0}: {1}", strMethodeName, ex.ToString))

		End Try

	End Sub

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
		LL.Core.LlSetOption(LL_OPTION_INCLUDEFONTDESCENT, _ClsLLFunc.LLFontDesent)
		LL.Core.LlSetOption(LL_OPTION_INCREMENTAL_PREVIEW, _ClsLLFunc.LLIncPrv)

		LL.Core.LlSetOption(LL_OPTION_VARSCASESENSITIVE, 0)

		LL.Core.LlSetOption(LL_OPTION_IMMEDIATELASTPAGE, 1)				' Lastpage
		LL.Core.LlSetOption(LL_OPTION_CONVERTCRLF, 1)							' Doppelte Zeilenumbruch

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

	Sub SetLLVariable(ByVal LL As ListLabel, ByVal employeeNumber As Integer)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim aValue As New List(Of String)
		Dim strMaskLbl As String

		Try
			' Zusätzliche Variable einfügen
			LL.Variables.Add("AutorFName", _ClsProgSetting.GetUserFName)
			LL.Variables.Add("AutorLName", _ClsProgSetting.GetUserLName)
			LL.Variables.Add("USSignFilename", Me.m_PrintSetting.USSignFileName)
			LL.Variables.Add("USPictureFilename", String.Empty)
			LL.Variables.Add("EmployeePictureFilename", String.Empty)

			Try
				LL.Variables.Add("USPictureFilename", MainUtilities.GetUSPicture(m_PrintSetting.m_initData.MDData.MDDbConn))
				LL.Variables.Add("EmployeePictureFilename", MainUtilities.GetEmployeePicture(employeeNumber))
			Catch ex As Exception
				m_Logger.LogError(String.Format("LlException.{0}: {1}", strMethodeName, ex.ToString))
			End Try

			LL.Variables.Add("Dokbez", _ClsLLFunc.ListBez)


			Dim jobDescription = GetEmployeeSecJob(employeeNumber, Me.m_PrintSetting)
			LL.Variables.Add("MAsBerufe", jobDescription)

			' Die Feld-Bezeichnungen gemäss Einstellung der Maske...
			LL.Variables.Add("OFStatusbez", "Status")
			LL.Variables.Add("OFGruppebez", "Gruppe")
			LL.Variables.Add("OFKontaktbez", "Kontakt")
			LL.Variables.Add("OF1Bemerkbez", "1. Bemerkung")
			LL.Variables.Add("OF2Bemerkbez", "2. Bemerkung")
			LL.Variables.Add("OF3Bemerkbez", "3. Bemerkung")
			LL.Variables.Add("OF4Bemerkbez", "4. Bemerkung")
			LL.Variables.Add("OF5Bemerkbez", "5. Bemerkung")

			LL.Variables.Add("OF1Resbez", "1. Reserve")
			LL.Variables.Add("OF2Resbez", "2. Reserve")
			LL.Variables.Add("OF3Resbez", "3. Reserve")
			LL.Variables.Add("OF4Resbez", "4. Reserve")
			LL.Variables.Add("OF5Resbez", "5. Reserve")

			LL.Variables.Add("OfFileName", _ClsLLFunc.LLExportedFilePath & "\" & _ClsLLFunc.LLExportedFileName)

			' Daten für die Bezeichnung...
			Const Ini_263 As String = "Sonstiges"
			Const Ini_588 As String = "OffertenstatusBez"
			Const Ini_589 As String = "OffertenKontakteBez"
			Const Ini_590 As String = "OffertenRes1Bez"
			Const Ini_591 As String = "OffertenRes2Bez"
			Const Ini_592 As String = "OffertenRes3Bez"
			Const Ini_593 As String = "OffertenRes4Bez"
			Const Ini_594 As String = "OffertenRes5Bez"
			Const Ini_595 As String = "OffertenBem1Bez"
			Const Ini_596 As String = "OffertenBem2Bez"
			Const Ini_597 As String = "OffertenBem3Bez"
			Const Ini_598 As String = "OffertenBem4Bez"
			Const Ini_599 As String = "OffertenBem5Bez"
			Const Ini_601 As String = "OffertenGruppeBez"

			strMaskLbl = _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, Ini_263, Ini_588)
			If strMaskLbl <> "" Then LL.Variables.Add("OFStatusbez", strMaskLbl)
			' Gruppe
			strMaskLbl = _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, Ini_263, Ini_601)
			If strMaskLbl <> "" Then LL.Variables.Add("OFGruppebez", strMaskLbl)
			' Kontakt
			strMaskLbl = _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, Ini_263, Ini_589)
			If strMaskLbl <> "" Then LL.Variables.Add("OFKontaktbez", strMaskLbl)

			' 1. Bemerkung
			strMaskLbl = _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, Ini_263, Ini_595)
			If strMaskLbl <> "" Then LL.Variables.Add("OF1Bemerkbez", strMaskLbl)
			' 2. Bemerkung
			strMaskLbl = _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, Ini_263, Ini_596)
			If strMaskLbl <> "" Then LL.Variables.Add("OF2Bemerkbez", strMaskLbl)
			' 3. Bemerkung
			strMaskLbl = _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, Ini_263, Ini_597)
			If strMaskLbl <> "" Then LL.Variables.Add("OF3Bemerkbez", strMaskLbl)
			' 4. Bemerkung
			strMaskLbl = _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, Ini_263, Ini_598)
			If strMaskLbl <> "" Then LL.Variables.Add("OF4Bemerkbez", strMaskLbl)
			' 5. Bemerkung
			strMaskLbl = _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, Ini_263, Ini_599)
			If strMaskLbl <> "" Then LL.Variables.Add("OF5Bemerkbez", strMaskLbl)

			' 3. Seite...
			' 1. Reserve
			strMaskLbl = _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, Ini_263, Ini_590)
			If strMaskLbl <> "" Then LL.Variables.Add("OF1Resbez", strMaskLbl)
			' 2. Reserve
			strMaskLbl = _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, Ini_263, Ini_591)
			If strMaskLbl <> "" Then LL.Variables.Add("OF2Resbez", strMaskLbl)
			' 3. Reserve
			strMaskLbl = _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, Ini_263, Ini_592)
			If strMaskLbl <> "" Then LL.Variables.Add("OF3Resbez", strMaskLbl)
			' 4. Reserve
			strMaskLbl = _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, Ini_263, Ini_593)
			If strMaskLbl <> "" Then LL.Variables.Add("OF4Resbez", strMaskLbl)
			' 5. Reserve
			strMaskLbl = _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, Ini_263, Ini_594)
			If strMaskLbl <> "" Then LL.Variables.Add("OF5Resbez", strMaskLbl)

			LL.Variables.Add("RepNrToSend", "")
			LL.Variables.Add("RepNameToSend", "")




		Catch ex As Exception
			m_Logger.LogError(String.Format("LlException.{0}: {1}", strMethodeName, ex.ToString))

		End Try

		GetMDUSData4Print(LL)

	End Sub


	Public Sub New(ByVal _Setting As ClsLLOfferSearchPrintSetting)

		m_path = New ClsProgPath
		m_md = New Mandant
		m_UtilityUI = New UtilityUI

		Me.m_PrintSetting = _Setting
		Me.ExistsDocFile = BuildPrintJob()

	End Sub

	Protected Overridable Overloads Sub Dispose( _
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


End Class

