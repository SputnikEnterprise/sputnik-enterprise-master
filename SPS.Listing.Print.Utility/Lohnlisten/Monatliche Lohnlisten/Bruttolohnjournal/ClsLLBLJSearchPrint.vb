﻿
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath

Imports System.Data.SqlClient
Imports System.IO
Imports combit.ListLabel25
Imports SPS.Listing.Print.Utility.MainUtilities
Imports SPS.Listing.Print.Utility.ClsMainSetting

Imports SPProgUtility.MainUtilities

Imports SP.Infrastructure.Logging
Imports DevExpress.XtraSplashScreen
Imports SP.Infrastructure.UI

Public Class ClsLLBLJSearchPrint
	Implements IDisposable

#Region "Private Consts"

	Private Const MANDANT_XML_SETTING_SPUTNIK_LL_DEBUG As String = "MD_{0}/Sonstiges/EnableLLDebug"

#End Region

	Protected disposed As Boolean = False

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As SPProgUtility.MainUtilities.Utilities

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI
	Private m_ProgPath As New ClsProgPath
	Private m_mandant As New Mandant
	Private PrintSetting As New ClsLLBLJSearchPrintSetting

	Private _ClsReg As New SPProgUtility.ClsDivReg
	Private _ClsLog As New SPProgUtility.ClsEventLog
	Friend _ClsLLFunc As New ClsLLFunc

	Private strConnString As String = String.Empty
	Private strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

	Private Property USSignFileName As String
	Private Property ExistsDocFile As Boolean
	Private Property ExtraVakFieldData As List(Of String)

	Private LL As ListLabel = New ListLabel



#Region "Constructor"

	Public Sub New(ByVal _Setting As ClsLLBLJSearchPrintSetting)

		m_Utility = New SPProgUtility.MainUtilities.Utilities
		m_UtilityUI = New UtilityUI
		m_ProgPath = New ClsProgPath
		m_mandant = New Mandant

		Me.PrintSetting = _Setting
		Me.strConnString = _Setting.DbConnString2Open
		Me.ExistsDocFile = BuildPrintJob()

	End Sub

#End Region

	Function BuildPrintJob() As Boolean
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim sSql As String = "Select * From DokPrint Where [JobNr] = @JobNr"
		Dim Conn As SqlConnection = New SqlConnection(strConnString)
		Dim bResult As Boolean = True
		Dim JobNr As String = PrintSetting.JobNr2Print

		If JobNr = String.Empty Then
			Dim strMessage As String = "Sie haben keine Vorlage ausgewählt.{0}Bitte wählen Sie aus der Liste eine Vorlage aus."
			m_Logger.LogError("File with jobnr = " & JobNr & "could not be found...")
			m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue(strMessage), vbNewLine))

			Return False
		End If
		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
		Dim param As System.Data.SqlClient.SqlParameter

		Try
			Conn.Open()

			param = cmd.Parameters.AddWithValue("@JobNr", PrintSetting.JobNr2Print)
			Dim rDocrec As SqlDataReader = cmd.ExecuteReader					' Dokumentendatenbank
			rDocrec.Read()
			If Not String.IsNullOrEmpty(rDocrec("DocName").ToString) Then
				_ClsLLFunc.LLDocName = m_mandant.GetSelectedMDDocPath(Me.PrintSetting.SelectedMDNr) & rDocrec("DocName").ToString
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
					_ClsLLFunc.LLExportedFilePath = m_ProgPath.GetSpS2DeleteHomeFolder
				Else
					_ClsLLFunc.LLExportedFilePath = _ClsReg.AddDirSep(rDocrec("TempDocPath").ToString)
				End If

				If String.IsNullOrEmpty(rDocrec("ExportedFileName").ToString) Then
					_ClsLLFunc.LLExportedFileName = Path.GetFileNameWithoutExtension(rDocrec("DocName").ToString)
				Else
					_ClsLLFunc.LLExportedFileName = Path.GetFileNameWithoutExtension(rDocrec("ExportedFileName").ToString)
				End If

				If IsDBNull(rDocrec("PrintInDiffColor")) Or (rDocrec("PrintInDiffColor").ToString = String.Empty) Then
					_ClsLLFunc.LLPrintInDiffColor = False
				Else
					_ClsLLFunc.LLPrintInDiffColor = CBool(rDocrec("PrintInDiffColor"))
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
			Dim bAllowedtoDebug As Boolean = CBool(m_ProgPath.GetXMLValueByQueryWithFilename(m_mandant.GetSelectedMDDataXMLFilename(ClsMainSetting.MDData.MDNr, Now.Year),
																																									 String.Format(MANDANT_XML_SETTING_SPUTNIK_LL_DEBUG, ClsMainSetting.MDData.MDNr), 0))
			If bAllowedtoDebug Then
				LL.Debug = LlDebug.LogToFile
				LL.Debug = LlDebug.Enabled
			End If
		Catch ex As Exception
			m_Logger.LogError(String.Format("EnablingLLDebuging.{0}: {1}", strMethodeName, ex))

		End Try

		If Not Me.ExistsDocFile Then Exit Sub

		Try
			Dim rFoundedrec As SqlDataReader = MainUtilities.OpenDb4PrintListing(PrintSetting.DbConnString2Open, PrintSetting.SQL2Open)
			If rFoundedrec Is Nothing OrElse Not rFoundedrec.HasRows Then
				m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, "Keine Daten wurden gefunden..."))
				Exit Sub
			End If

			InitLL()
			LL.Variables.Clear()
			LL.Fields.Clear()

			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				DefineData(True, rFoundedrec)
			Else
				DefineData(False, rFoundedrec)
			End If
			SetLLVariable()

			SplashScreenManager.CloseForm(False)

			LL.Core.LlDefineLayout(CType(PrintSetting.frmhwnd, IntPtr), _ClsLLFunc.LLDocLabel,
														 If(_ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper), LlProject.List, LlProject.Card),
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
		Dim strJobNr As String = Me.PrintSetting.JobNr2Print

		Dim strQuery As String = PrintSetting.SQL2Open
		Dim Conn As SqlConnection = New SqlConnection(strConnString)
		Dim i As Integer = 0

		If Not Me.ExistsDocFile Then Return strResult

		Try
			Dim rFoundedrec As SqlDataReader = MainUtilities.OpenDb4PrintListing(PrintSetting.DbConnString2Open, PrintSetting.SQL2Open)
			If rFoundedrec Is Nothing OrElse Not rFoundedrec.HasRows Then Return m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden") & "..."

			InitLL()
			LL.Variables.Clear()
			LL.Fields.Clear()

			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				DefineData(True, rFoundedrec)
			Else
				DefineData(False, rFoundedrec)
			End If
			SetLLVariable()
			LL.Core.LlPrintWithBoxStart(If(_ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper), _
																		 LlProject.List, LlProject.Card), _
																		 _ClsLLFunc.LLDocName, LlPrintMode.Export, LlBoxType.StandardAbort, _
																		 CType(PrintSetting.frmhwnd, IntPtr), _ClsLLFunc.LLDocLabel)

			LL.Core.LlPrintSetOption(LlPrintOption.Copies, _ClsLLFunc.LLCopyCount)
			LL.Core.LlPrintSetOption(LL_PRNOPT_COPIES_SUPPORTED, LL.Core.LlPrintGetOption(LlPrintOption.Copies))

			If bShowBox Then
				SplashScreenManager.CloseForm(False)
				LL.Core.LlPrintOptionsDialog(CType(PrintSetting.frmhwnd, IntPtr), String.Format("{1}: {2}{0}{3}", _
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
				LL.Core.LlPreviewDisplay(_ClsLLFunc.LLDocName, _
																 m_ProgPath.GetSpSTempFolder, CType(PrintSetting.frmhwnd, IntPtr))
				LL.Core.LlPreviewDeleteFiles(_ClsLLFunc.LLDocName, m_ProgPath.GetSpSTempFolder)
				Return "Error"
			End If


		Catch LlException As Exception
			m_Logger.LogError(String.Format("LlException.{0}: {1}", strMethodeName, LlException.Message))
			strResult = String.Format("Error: {0}", LlException.Message)

		Finally

		End Try

		Return strResult
	End Function


	Sub DefineData(ByVal AsFields As Boolean, ByVal MyDataReader As SqlDataReader)
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

	Sub GetMDUSData4Print()

		MainUtilities.GetUSData(Me.PrintSetting.DbConnString2Open, _ClsLLFunc, PrintSetting.SelectedMDNr)
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


		End With

	End Sub

	Sub InitLL()
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
		Const LL_OPTION_PRVZOOM_PERC As Integer = 25													 ' 25

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

		LL.Core.LlPreviewSetTempPath(m_ProgPath.GetSpSTempFolder)
		LL.Core.LlSetPrinterDefaultsDir(m_ProgPath.GetPrinterHomeFolder)

	End Sub

	Sub SetLLVariable()
		Dim aValue As New List(Of String)

		' Zusätzliche Variable einfügen
		LL.Variables.Add("AutorFName", ClsMainSetting.UserData.UserFName)
		LL.Variables.Add("AutorLName", ClsMainSetting.UserData.UserLName)
		LL.Variables.Add("USSignFilename", Me.PrintSetting.USSignFileName)

		LL.Variables.Add("SortBez", PrintSetting.ListSortBez)
		LL.Variables.Add("FilterBez", PrintSetting.ListFilterBez(0))
		LL.Variables.Add("FilterBez2", If(PrintSetting.ListFilterBez.Count > 1, PrintSetting.ListFilterBez(1), String.Empty))
		LL.Variables.Add("FilterBez3", If(PrintSetting.ListFilterBez.Count > 2, PrintSetting.ListFilterBez(2), String.Empty))
		LL.Variables.Add("FilterBez4", If(PrintSetting.ListFilterBez.Count > 3, PrintSetting.ListFilterBez(3), String.Empty))
		LL.Fields.Add("bLineinDiffColor", _ClsLLFunc.LLPrintInDiffColor)

		LL.Variables.Add("ShowBruttolohn", PrintSetting.ShowBruttolohn)
		LL.Variables.Add("ShowSUVABasis", PrintSetting.ShowSUVABasis)
		LL.Variables.Add("ShowAHVBasis", PrintSetting.ShowAHVBasis)

		' Mandantendaten drucken...
		GetMDUSData4Print()

	End Sub



	Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
		If Not Me.disposed Then
			If disposing Then
				SplashScreenManager.CloseForm(False)
			End If
			' Add code here to release the unmanaged resource.
			LL.Dispose()
			LL.Core.Dispose()
			' Note that this is not thread safe.
		End If
		Me.disposed = True
	End Sub

#Region " IDisposable Support"

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
