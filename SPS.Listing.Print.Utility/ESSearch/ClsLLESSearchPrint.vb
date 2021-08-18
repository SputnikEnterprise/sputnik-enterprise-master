
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath

Imports System.Data.SqlClient
Imports System.IO
Imports combit.ListLabel25
Imports SPS.Listing.Print.Utility.MainUtilities
Imports SPS.Listing.Print.Utility.ClsMainSetting

Imports SP.Infrastructure.Logging

Public Class ClsLLESSearchPrint
	Implements IDisposable

#Region "private Consts"

	Private Const FORM_XML_DEFAULTVALUES_KEY As String = "Forms_Normaly/Field_DefaultValues"

#End Region

	Protected disposed As Boolean = False

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Protected m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	Private m_ProgPath As ClsProgPath
	Private m_mandant As Mandant
	Private m_utility As SPProgUtility.MainUtilities.Utilities

	Private ESListPrintSetting As New ClsLLESSearchPrintSetting

	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	Private _ClsReg As New SPProgUtility.ClsDivReg
	Private _ClsLog As New SPProgUtility.ClsEventLog
	Friend _ClsLLFunc As New ClsLLFunc
	Private m_MandantFormXMLFile As String

	Private strConnString As String = String.Empty
	Private strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

	Private Property USSignFileName As String
	Private Property ExistsDocFile As Boolean


	Private LL As ListLabel = New ListLabel
	Private m_CustomerMinMarge As Boolean


#Region "Constructor"

	Public Sub New(ByVal _Setting As ClsLLESSearchPrintSetting)

		m_InitializationData = m_InitialData

		m_ProgPath = New ClsProgPath
		m_mandant = New Mandant
		m_utility = New SPProgUtility.MainUtilities.Utilities

		Me.ESListPrintSetting = _Setting
		Me.strConnString = _Setting.DbConnString2Open
		Me.ExistsDocFile = BuildPrintJob()

		m_MandantFormXMLFile = m_mandant.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr)

		m_CustomerMinMarge = ESCustomerMinMarge

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


	Private ReadOnly Property GetYearMwSt(ByVal iYear As Integer) As Single
		Get
			Dim mdNumber = ESListPrintSetting.SelectedMDNr
			If iYear = 0 Then iYear = Now.Year
			Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}/Debitoren", mdNumber)
			Dim mwstsatz As String = m_ProgPath.GetXMLNodeValue(m_mandant.GetSelectedMDDataXMLFilename(mdNumber, iYear), String.Format("{0}/mwstsatz", FORM_XML_MAIN_KEY))

			If String.IsNullOrWhiteSpace(mwstsatz) Then
				Return 8
			End If

			Return mwstsatz

		End Get
	End Property

	Private ReadOnly Property ESCustomerMinMarge() As Boolean
		Get
			Dim calculatecustomerrefundinmarge As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/calculatecustomerrefundinmarge", FORM_XML_DEFAULTVALUES_KEY)), False)

			Return calculatecustomerrefundinmarge
		End Get
	End Property

	Function BuildPrintJob() As Boolean
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim sSql As String = "Select * From DokPrint Where [JobNr] = @JobNr"
		Dim Conn As SqlConnection = New SqlConnection(strConnString)
		Dim bResult As Boolean = True
		Dim JobNr As String = ESListPrintSetting.JobNr2Print

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

			param = cmd.Parameters.AddWithValue("@JobNr", ESListPrintSetting.JobNr2Print)
			Dim rDocrec As SqlDataReader = cmd.ExecuteReader          ' Dokumentendatenbank
			rDocrec.Read()
			If Not String.IsNullOrEmpty(rDocrec("DocName").ToString) Then
				_ClsLLFunc.LLDocName = m_mandant.GetSelectedMDDocPath(Me.ESListPrintSetting.SelectedMDNr) & rDocrec("DocName").ToString
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

			'MsgBox(String.Format(TranslateMyText("Fehler: {1}{0}JobNr: {2}"), vbNewLine, Err.Description, JobNr),
			'       MsgBoxStyle.Critical, strMethodeName)

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
			Dim rFoundedrec As SqlDataReader = OpenDb4PrintListing(strConnString, ESListPrintSetting.SQL2Open)
			If Not rFoundedrec.HasRows Then
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
			ExcludeGAVString(rFoundedrec("GAVInfo_String"))


			LL.Core.LlDefineLayout(CType(0, IntPtr), _ClsLLFunc.LLDocLabel,
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
		Dim strJobNr As String = Me.ESListPrintSetting.JobNr2Print
		Try
			Dim bAllowedtoDebug As Boolean = CBool(_ClsProgSetting.GetMDProfilValue("Sonstiges", "EnableLLDebug", "0"))
			If bAllowedtoDebug Then
				LL.Debug = LlDebug.LogToFile
				LL.Debug = LlDebug.Enabled
			End If
		Catch ex As Exception
			m_Logger.LogError(String.Format("EnablingLLDebuging.{0}: {1}", strMethodeName, ex))

		End Try

		Dim strQuery As String = ESListPrintSetting.SQL2Open
		Dim Conn As SqlConnection = New SqlConnection(strConnString)
		Dim i As Integer = 0

		If Not Me.ExistsDocFile Then Return strResult

		Try
			Dim rFoundedrec As SqlDataReader = Nothing
			If Me.ESListPrintSetting.SQL2Open = String.Empty Then
				rFoundedrec = MainUtilities.OpenDb4PrintESTemplate(strConnString, Me.ESListPrintSetting.ESNr2Print)

			Else
				rFoundedrec = MainUtilities.OpenDb4PrintListing(strConnString, Me.ESListPrintSetting.SQL2Open)

			End If

			If (rFoundedrec Is Nothing) OrElse Not rFoundedrec.HasRows Then Return TranslateMyText("Keine Daten wurden gefunden") & "..."
			InitLL()
			LL.Variables.Clear()
			LL.Fields.Clear()

			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				DefineData(True, rFoundedrec)
			Else
				DefineData(False, rFoundedrec)
			End If
			SetLLVariable()
			ExcludeGAVString(rFoundedrec("GAVInfo_String"))

			LL.Core.LlPrintWithBoxStart(If(_ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper),
																		 LlProject.List, LlProject.Card),
																		 _ClsLLFunc.LLDocName, LlPrintMode.Export, LlBoxType.StandardAbort,
																		 CType(0, IntPtr), _ClsLLFunc.LLDocLabel)

			LL.Core.LlPrintSetOption(LlPrintOption.Copies, _ClsLLFunc.LLCopyCount)
			LL.Core.LlPrintSetOption(LL_PRNOPT_COPIES_SUPPORTED, LL.Core.LlPrintGetOption(LlPrintOption.Copies))

			If bShowBox Then
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
					DefineData(True, rFoundedrec)
					ExcludeGAVString(rFoundedrec("GAVInfo_String"))

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
																 m_ProgPath.GetSpSTempFolder, CType(0, IntPtr))
				LL.Core.LlPreviewDeleteFiles(_ClsLLFunc.LLDocName, m_ProgPath.GetSpSTempFolder)
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


	Sub LoadDesingerForGAVStatistiken()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		If Not Me.ExistsDocFile Then Return

		Try
			Dim rFoundedrec As SqlDataReader = Nothing
			rFoundedrec = MainUtilities.OpenDb4ESGAVStatistiken(ESListPrintSetting)

			If (rFoundedrec Is Nothing) OrElse Not rFoundedrec.HasRows Then
				m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, "Keine Daten wurden gefunden..."))
				Return
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

			LL.Core.LlDefineLayout(CType(0, IntPtr), _ClsLLFunc.LLDocLabel,
														 If(_ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper), LlProject.List, LlProject.Card),
														 _ClsLLFunc.LLDocName)

		Catch LlException As Exception
			m_Logger.LogError(String.Format("LlException.{0}: {1}", strMethodeName, LlException.Message))

		Finally

		End Try

	End Sub

	Function PrintGAVStatistiken() As PrintResult
		Const LL_PRNOPT_COPIES_SUPPORTED As Integer = 3
		Dim result As PrintResult = New PrintResult With {.Printresult = True}
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strJobNr As String = Me.ESListPrintSetting.JobNr2Print

		If Not Me.ExistsDocFile Then Return New PrintResult With {.Printresult = False,
			.PrintresultMessage = String.Format("{0}: Vorlage wurde nicht gefunden.", ESListPrintSetting.JobNr2Print)}

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
			Dim rFoundedrec As SqlDataReader = Nothing
			rFoundedrec = MainUtilities.OpenDb4ESGAVStatistiken(ESListPrintSetting)

			If (rFoundedrec Is Nothing) OrElse Not rFoundedrec.HasRows Then Return New PrintResult With {.Printresult = False,
			.PrintresultMessage = String.Format("Keine Daten wurden gefunden.")}

			InitLL()
			LL.Variables.Clear()
			LL.Fields.Clear()

			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				DefineData(True, rFoundedrec)
			Else
				DefineData(False, rFoundedrec)
			End If
			SetLLVariable()

			LL.Core.LlPrintWithBoxStart(If(_ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper),
																		 LlProject.List, LlProject.Card),
																		 _ClsLLFunc.LLDocName, LlPrintMode.Export, LlBoxType.StandardAbort,
																		 CType(0, IntPtr), _ClsLLFunc.LLDocLabel)

			LL.Core.LlPrintSetOption(LlPrintOption.Copies, _ClsLLFunc.LLCopyCount)
			LL.Core.LlPrintSetOption(LL_PRNOPT_COPIES_SUPPORTED, LL.Core.LlPrintGetOption(LlPrintOption.Copies))

			LL.Core.LlPrintOptionsDialog(CType(0, IntPtr), String.Format("{1}: {2}{0}{3}",
																																	 vbNewLine,
																																	 _ClsLLFunc.LLDocLabel,
																																	 strJobNr,
																																	 _ClsLLFunc.LLDocName))
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

				Return New PrintResult With {.Printresult = False}

			End If


		Catch LlException As LL_User_Aborted_Exception
			Return New PrintResult With {.Printresult = False,
			.PrintresultMessage = String.Format("{0}", LlException.ToString)}

		Catch LlException As Exception
			m_Logger.LogError(String.Format("LlException.{0}: {1}", strMethodeName, LlException.Message))
			Return New PrintResult With {.Printresult = False,
			.PrintresultMessage = String.Format("{0}", LlException.ToString)}

		Finally

		End Try


		Return result

	End Function


#Region "employees (AT)"

	Sub LoadDesingerForEmployeesATStatistiken()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		If Not Me.ExistsDocFile Then Return

		Try
			Dim rFoundedrec As SqlDataReader = Nothing
			rFoundedrec = MainUtilities.OpenDb4EmployeesATStatistiken(ESListPrintSetting)

			If (rFoundedrec Is Nothing) OrElse Not rFoundedrec.HasRows Then
				m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, "Keine Daten wurden gefunden..."))
				Return
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

			LL.Core.LlDefineLayout(CType(0, IntPtr), _ClsLLFunc.LLDocLabel,
														 If(_ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper), LlProject.List, LlProject.Card),
														 _ClsLLFunc.LLDocName)

		Catch LlException As Exception
			m_Logger.LogError(String.Format("LlException.{0}: {1}", strMethodeName, LlException.Message))

		Finally

		End Try

	End Sub

	Function PrintEmployeesATStatistiken() As PrintResult
		Const LL_PRNOPT_COPIES_SUPPORTED As Integer = 3
		Dim result As PrintResult = New PrintResult With {.Printresult = True}
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strJobNr As String = Me.ESListPrintSetting.JobNr2Print

		If Not Me.ExistsDocFile Then Return New PrintResult With {.Printresult = False,
			.PrintresultMessage = String.Format("{0}: Vorlage wurde nicht gefunden.", ESListPrintSetting.JobNr2Print)}

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
			Dim rFoundedrec As SqlDataReader = Nothing
			rFoundedrec = MainUtilities.OpenDb4EmployeesATStatistiken(ESListPrintSetting)

			If (rFoundedrec Is Nothing) OrElse Not rFoundedrec.HasRows Then Return New PrintResult With {.Printresult = False,
			.PrintresultMessage = String.Format("Keine Daten wurden gefunden.")}

			InitLL()
			LL.Variables.Clear()
			LL.Fields.Clear()

			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				DefineData(True, rFoundedrec)
			Else
				DefineData(False, rFoundedrec)
			End If
			SetLLVariable()

			LL.Core.LlPrintWithBoxStart(If(_ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper),
																		 LlProject.List, LlProject.Card),
																		 _ClsLLFunc.LLDocName, LlPrintMode.Export, LlBoxType.StandardAbort,
																		 CType(0, IntPtr), _ClsLLFunc.LLDocLabel)

			LL.Core.LlPrintSetOption(LlPrintOption.Copies, _ClsLLFunc.LLCopyCount)
			LL.Core.LlPrintSetOption(LL_PRNOPT_COPIES_SUPPORTED, LL.Core.LlPrintGetOption(LlPrintOption.Copies))

			LL.Core.LlPrintOptionsDialog(CType(0, IntPtr), String.Format("{1}: {2}{0}{3}",
																																	 vbNewLine,
																																	 _ClsLLFunc.LLDocLabel,
																																	 strJobNr,
																																	 _ClsLLFunc.LLDocName))
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

				Return New PrintResult With {.Printresult = False}

			End If


		Catch LlException As LL_User_Aborted_Exception
			Return New PrintResult With {.Printresult = False,
			.PrintresultMessage = String.Format("{0}", LlException.ToString)}

		Catch LlException As Exception
			m_Logger.LogError(String.Format("LlException.{0}: {1}", strMethodeName, LlException.Message))
			Return New PrintResult With {.Printresult = False,
			.PrintresultMessage = String.Format("{0}", LlException.ToString)}

		Finally

		End Try


		Return result

	End Function


#End Region


	Function ExportLLDoc(ByVal strJobNr As String, ByVal iExportMode As Short,
											 ByVal iProposeNr As Integer) As Boolean

		Dim strLLTemplate As String = _ClsLLFunc.LLDocName
		Dim bResult As Boolean = True
		If Not Me.ExistsDocFile Then Return bResult

		Dim strQuery As String = ESListPrintSetting.SQL2Open
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
							 MsgBoxStyle.Critical, TranslateMyText("Daten suchen"))
				Return bResult
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

			LL.ExportOptions.Clear()
			SetExportSetting(iExportMode)

			LL.Core.LlPrintWithBoxStart(If(_ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper), LlProject.List, LlProject.Card),
																	_ClsLLFunc.LLDocName, LlPrintMode.Export, LlBoxType.StandardAbort, CType(0, IntPtr),
																	_ClsLLFunc.LLDocLabel)

			LL.Core.LlPrintSetOptionString(LlPrintOptionString.Export, _ClsLLFunc.LLExporterName)
			LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.File", _ClsLLFunc.LLExporterFileName)
			LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.Path", m_ProgPath.GetSpSOfferHomeFolder)
			LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.Quiet", "1")

			LL.Core.LlPrintSetOption(LlPrintOption.Dialog_DestinationMask, 2)
			Dim TargetFormat As String = LL.Core.LlPrintGetOptionString(LlPrintOptionString.Export)

			While Not LL.Core.LlPrint()
			End While

			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				Do
					' pass data for current record
					DefineData(True, rFoundedrec)

					While LL.Core.LlPrintFields() = LlConstants.WrnRepeatData ' LlConst.LL_WRN_REPEAT_DATA
						LL.Core.LlPrint()
					End While

				Loop While rFoundedrec.Read()

				While LL.Core.LlPrintFieldsEnd() = LlConstants.WrnRepeatData ' LlConst.LL_WRN_REPEAT_DATA
				End While
			End If
			LL.Core.LlPrintEnd(0)

			If TargetFormat = "PRV" Then
				LL.Core.LlPreviewDisplay(_ClsLLFunc.LLDocName,
																 m_ProgPath.GetSpSTempFolder,
																 CType(0, IntPtr))
				LL.Core.LlPreviewDeleteFiles(_ClsLLFunc.LLDocName,
																		 m_ProgPath.GetSpSTempFolder)
				Return False
			End If
			'      _ClsLLFunc.LLExportFileName = String.Format("{0}{1}", m_path.GetSpSOfferHomeFolder,ClsLLFunc.LLExporterFileName)

		Catch LlException As Exception
			m_Logger.LogError(String.Format("LlException.Message:{0}This information was generated by a List & Label custom exception.", vbNewLine))

			'If Err.Number <> (LlError.LL_ERR_USER_ABORTED) And Err.Number <> 5 Then
			'  MessageBox.Show(LlException.Message + vbCrLf, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
			'End If
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

	Sub DefineData(ByVal AsFields As Boolean, ByVal MyDataReader As SqlDataReader)
		Dim i As Integer
		Dim iType As String
		Dim strParam As LlFieldType
		Dim strContent As String

		' Define data
		For i = 0 To MyDataReader.FieldCount - 1
			iType = MyDataReader.GetFieldType(i).ToString.ToUpper

			Select Case iType.ToUpper

				'GR: Numerisches Feld
				Case "System.Int16".ToUpper, "System.Int32".ToUpper, "System.Int64".ToUpper, "System.Decimal".ToUpper, "System.Double".ToUpper
					'DbType.Currency, DbType.Decimal, DbType.Double, DbType.Int16, DbType.Int32, DbType.Int64
					'TypeCode.Decimal, TypeCode.Double, TypeCode.Int16, TypeCode.Int32, TypeCode.Int64, TypeCode.UInt16, TypeCode.UInt32, TypeCode.Int64
					'SqlDbType.Money, SqlDbType.Decimal, SqlDbType.Float, SqlDbType.Int, SqlDbType.SmallInt, SqlDbType.SmallMoney, SqlDbType.TinyInt
					strParam = LlFieldType.Numeric
					strContent = MyDataReader.Item(i).ToString & ""

					'GR: Falls der Datentyp "Datum" ist, Umwandlun in einen numerischen Datumswert
				Case "System.DateTime".ToUpper
					''SqlDbType.Date, SqlDbType.DateTime, SqlDbType.DateTime2, SqlDbType.SmallDateTime, SqlDbType.Time
					'strParam = LlFieldType.Date_Localized
					'If IsDBNull(MyDataReader.Item(i)) Then
					'  strContent = CDate("1.1.1900").ToShortDateString
					'Else
					'  strContent = CDate(MyDataReader.Item(i)).ToShortDateString
					'End If

					strParam = LlFieldType.Date_OLE

					If IsDBNull(MyDataReader.Item(i)) Then
						strContent = CDate("01.01.1900").ToOADate().ToString() ' , "G")
					Else
						strContent = MyDataReader.GetDateTime(i).ToOADate().ToString()
					End If

					'GR: Entscheidungsfeld (Ja/Nein), Boolean
				Case "System.Boolean".ToUpper
					strParam = LlFieldType.Boolean
					If IsDBNull(MyDataReader.Item(i)) Then
						strContent = "0"
					Else
						strContent = CStr(IIf(CBool(MyDataReader.Item(i)), 1, 0))
					End If

					'GR: Zeichenformat = Text
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
			'      LL.Core.LlDefineFieldExt(MyDataReader.GetName(i), MyDataReader.GetValue(i),LlFieldType.Numeric 
		Next

	End Sub

	Sub GetMDUSData4Print()

		MainUtilities.GetUSData(Me.ESListPrintSetting.DbConnString2Open, _ClsLLFunc, ESListPrintSetting.SelectedMDNr)
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
			LL.Variables.Add("MwStProzent", GetYearMwSt(ESListPrintSetting.SelectedMDYear))

		End With

	End Sub

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

		LL.Core.LlSetOption(LlOption.RibbonDefaultEnabledState, 1)

		LL.Core.LlPreviewSetTempPath(m_ProgPath.GetSpSTempFolder)
		LL.Core.LlSetPrinterDefaultsDir(m_ProgPath.GetPrinterHomeFolder)

	End Sub

	Sub SetLLVariable()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim aValue As New List(Of String)

		' Zusätzliche Variable einfügen
		LL.Variables.Add("AutorFName", m_InitialData.UserData.UserFName)
		LL.Variables.Add("AutorLName", m_InitialData.UserData.UserLName)
		LL.Variables.Add("USSignFilename", Me.ESListPrintSetting.USSignFileName)


		LL.Variables.Add("SortBez", ESListPrintSetting.ListSortBez)
		LL.Variables.Add("FilterBez", ESListPrintSetting.ListFilterBez(0))
		LL.Variables.Add("FilterBez2", ESListPrintSetting.ListFilterBez(1))
		LL.Variables.Add("FilterBez3", ESListPrintSetting.ListFilterBez(2))
		LL.Variables.Add("FilterBez4", ESListPrintSetting.ListFilterBez(3))

		LL.Fields.Add("bLineinDiffColor", _clsllfunc.LLPrintInDiffColor)

		LL.Fields.Add("ES_KD_UmsMin", If(m_CustomerMinMarge, "1", "0"))

		' Mandantendaten drucken...
		GetMDUSData4Print()
		Dim permissionData = ESListPrintSetting.PermissionData

		' Zusatzstatistik am Ende der Einsatzliste anzeigen
		If Me.ESListPrintSetting.ShowBewilligStatistik Then
			Dim conn As SqlConnection = New SqlConnection(strConnString)
			Dim cmd As SqlCommand = New SqlCommand("[List MA Stat Bew]", conn)
			cmd.CommandType = CommandType.StoredProcedure
			cmd.Parameters.AddWithValue("@tableToReturn", 1)
			cmd.Parameters.AddWithValue("@tblNameSource", Me.ESListPrintSetting.DbTblName)
			cmd.Parameters.AddWithValue("@filiale", m_InitialData.UserData.UserFiliale)

			Try
				conn.Open()
				Dim reader As SqlDataReader = cmd.ExecuteReader()

				Dim bew As String = ""
				Dim frau As Integer = 0
				Dim mann As Integer = 0
				Dim rowCount As Integer
				While reader.Read()
					rowCount += 1
					bew = reader("Bewillig").ToString
					If Not String.IsNullOrWhiteSpace(bew) AndAlso Not permissionData Is Nothing AndAlso permissionData.Count > 0 Then
						Dim bewData = permissionData.Where(Function(x) x.Code = bew).FirstOrDefault()
						If Not bewData Is Nothing AndAlso Not String.IsNullOrWhiteSpace(bewData.Translated_Value) Then bew = String.Format("{0} - {1}", bewData.Code, bewData.Translated_Value)
					End If

					If bew = "S" OrElse String.IsNullOrWhiteSpace(bew) Then
						bew = "Schweizer/in"
					End If

					frau = Int32.Parse(reader("Frau").ToString)
					mann = Int32.Parse(reader("Mann").ToString)
					LL.Variables.Add(String.Format("Bew_{0}", rowCount), bew)
					LL.Variables.Add(String.Format("Frau_{0}", rowCount), frau)
					LL.Variables.Add(String.Format("Mann_{0}", rowCount), mann)
				End While

				' Noch nicht aufgeführte Bewilligungen anhängen (inkl. Reservefelder bis 15)
				For z As Integer = rowCount + 1 To 15
					LL.Variables.Add(String.Format("Bew_{0}", z), "")
					LL.Variables.Add(String.Format("Frau_{0}", z), 0)
					LL.Variables.Add(String.Format("Mann_{0}", z), 0)
				Next

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, ex.ToString))

			Finally
				conn.Close()
			End Try
		End If

	End Sub

	Private Sub ExcludeGAVString(ByVal strGAVString As String)
		Dim paresedGAVStirngData As New GAVStringData()
		Try

			paresedGAVStirngData.FillFromString(strGAVString)

			LL.Variables.Add(String.Format("{0}", "FARAG"), 0)
			LL.Variables.Add(String.Format("{0}", "FARAN"), 0)
			LL.Variables.Add(String.Format("{0}", "VAG_Value"), 0)
			LL.Variables.Add(String.Format("{0}", "VAN_Value"), 0)
			LL.Variables.Add(String.Format("{0}", "Kanton"), String.Empty)
			LL.Variables.Add(String.Format("{0}", "Gruppe0"), String.Empty)
			LL.Variables.Add(String.Format("{0}", "Gruppe1"), String.Empty)
			LL.Variables.Add(String.Format("{0}", "Gruppe2"), String.Empty)
			LL.Variables.Add(String.Format("{0}", "Gruppe3"), String.Empty)
			LL.Variables.Add(String.Format("{0}", "BasisLohn"), 0)
			LL.Variables.Add(String.Format("{0}", "FerienProz"), 0)
			LL.Variables.Add(String.Format("{0}", "FeierProz"), 0)
			LL.Variables.Add(String.Format("{0}", "Proz_Lohn13"), 0)

			If Not paresedGAVStirngData.Gruppe0 Is Nothing Then
				LL.Variables.Add(String.Format("{0}", "FARAG"), paresedGAVStirngData.FARAG)
				LL.Variables.Add(String.Format("{0}", "FARAN"), paresedGAVStirngData.FARAN)
				LL.Variables.Add(String.Format("{0}", "VAG_Value"), paresedGAVStirngData.VAG_Value)
				LL.Variables.Add(String.Format("{0}", "VAN_Value"), paresedGAVStirngData.VAN_Value)
				LL.Variables.Add(String.Format("{0}", "Kanton"), paresedGAVStirngData.Kanton)
				LL.Variables.Add(String.Format("{0}", "Gruppe0"), paresedGAVStirngData.Gruppe0)
				LL.Variables.Add(String.Format("{0}", "Gruppe1"), paresedGAVStirngData.Gruppe1)
				LL.Variables.Add(String.Format("{0}", "Gruppe2"), paresedGAVStirngData.Gruppe2)
				LL.Variables.Add(String.Format("{0}", "Gruppe3"), paresedGAVStirngData.Gruppe3)
				LL.Variables.Add(String.Format("{0}", "BasisLohn"), paresedGAVStirngData.BasisLohn)
				LL.Variables.Add(String.Format("{0}", "FerienProz"), paresedGAVStirngData.FerienProz * 100)
				LL.Variables.Add(String.Format("{0}", "FeierProz"), paresedGAVStirngData.FeierProz * 100)
				LL.Variables.Add(String.Format("{0}", "Proz_Lohn13"), paresedGAVStirngData.Proz_Lohn13 * 100)
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

	End Sub


End Class
