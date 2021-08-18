
Imports System.Data.SqlClient
Imports System.IO
Imports combit.ListLabel25
Imports SPS.Listing.Print.Utility.MainUtilities
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath

Imports SP.Infrastructure
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SP.DatabaseAccess.TableSetting.DataObjects
Imports SP.DatabaseAccess.Listing

Namespace PrintLOListing


	Public Class LOListing_LAStamm
		Implements IDisposable


#Region "private constants"

		Private Const MODUL_TO_PRINT_TESR = "6.3"
		Private Const MODUL_TO_PRINT_BESR = "6.4"

#End Region


#Region "private fields"

		Protected disposed As Boolean = False

		Dim m_PrintSetting As New LAStammPrintSetting

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
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
		Private _ClsReg As New SPProgUtility.ClsDivReg
		Private _ClsLog As New SPProgUtility.ClsEventLog
		Friend _ClsLLFunc As New ClsLLFunc
		Private m_path As New ClsProgPath
		Private m_md As New Mandant

		Private m_ConnectionString As String
		Private m_UserLanguage As String


		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As SP.Infrastructure.Utility


		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI

		Private Property ExistsDocFile As Boolean

		Private LL As ListLabel = New ListLabel

#End Region


#Region "Constructor"

		Public Sub New(ByVal _Setting As LAStammPrintSetting)

			m_UtilityUI = New UtilityUI
			m_Utility = New SP.Infrastructure.Utility

			m_PrintSetting = _Setting
			m_InitializationData = _Setting.m_initData
			m_Translate = _Setting.m_Translate

			m_ConnectionString = m_InitializationData.MDData.MDDbConn
			m_ListingDatabaseAccess = New ListingDatabaseAccess(m_ConnectionString, m_InitializationData.UserData.UserLanguage)
			m_UserLanguage = m_InitializationData.UserData.UserLanguage


			Me.ExistsDocFile = BuildPrintJob()


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


		Private Function BuildPrintJob() As Boolean
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

			Dim sSql As String = "Select * From DokPrint Where [JobNr] = @JobNr"
			Dim Conn As SqlConnection = New SqlConnection(m_ConnectionString)
			Dim bResult As Boolean = True
			Dim JobNr As String = m_PrintSetting.JobNr2Print

			If JobNr = String.Empty Then
				Dim strMessage As String = "Sie haben keine Vorlage ausgewählt.{0}Bitte wählen Sie aus der Liste eine Vorlage aus."
				MsgBox(String.Format(TranslateMyText(strMessage), vbNewLine),
						MsgBoxStyle.Critical, TranslateMyText("Leere Vorlage"))
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
					_ClsLLFunc.LLDocName = m_md.GetSelectedMDDocPath(m_InitializationData.MDData.MDNr) & rDocrec("DocName").ToString
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
			Dim projectType As LlProject = Nothing

			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				projectType = LlProject.List
			Else
				projectType = LlProject.Card
			End If

			If Not Me.ExistsDocFile Then Exit Sub

			Try
				If m_PrintSetting.LADataList Is Nothing OrElse m_PrintSetting.LADataList.Count = 0 Then
					m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, "Keine Daten wurden gefunden..."))
					Return
				End If

				InitLL(LL)
				LL.Variables.Clear()
				LL.Fields.Clear()

				Dim data As List(Of LAStammData) = m_PrintSetting.LADataList
				LL.SetDataBinding(data, String.Empty)

				SetLLVariable(LL)

				LL.Design(_ClsLLFunc.LLDocLabel, projectType, _ClsLLFunc.LLDocName, False)


			Catch LlException As Exception
				m_Logger.LogError(String.Format("LlException.{0}: {1}", strMethodeName, LlException.Message))

			Finally

			End Try

		End Sub

		Function ShowInPrint(ByRef bShowBox As Boolean) As Boolean
			'Const LL_PRNOPT_COPIES_SUPPORTED As Integer = 3
			Dim success As Boolean = True
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim projectType As LlProject = Nothing

			Dim strJobNr As String = Me.m_PrintSetting.JobNr2Print
			Try
				Dim bAllowedtoDebug As Boolean = True
				If bAllowedtoDebug Then
					LL.Debug = LlDebug.LogToFile
					LL.Debug = LlDebug.Enabled
				End If
			Catch ex As Exception
				m_Logger.LogWarning(String.Format("EnablingLLDebuging.{0}: {1}", strMethodeName, ex))

			End Try

			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				projectType = LlProject.List
			Else
				projectType = LlProject.Card
			End If

			If Not Me.ExistsDocFile Then Return False

			Try
				If m_PrintSetting.LADataList Is Nothing OrElse m_PrintSetting.LADataList.Count = 0 Then
					m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, "Keine Daten wurden gefunden..."))
					Return False
				End If

				InitLL(LL)
				LL.Variables.Clear()
				LL.Fields.Clear()

				Dim data As List(Of LAStammData) = m_PrintSetting.LADataList
				LL.SetDataBinding(data, String.Empty)

				LL.AutoDestination = LlPrintMode.PreviewControl

				SetLLVariable(LL)
				'If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				'	DefineData(LL, True, rFoundedrec)
				'Else
				'	DefineData(LL, False, rFoundedrec)
				'End If


				LL.Print(projectType, _ClsLLFunc.LLDocName, False, LlPrintMode.Export, LlBoxType.StandardAbort, _ClsLLFunc.LLDocLabel, True, m_InitializationData.UserData.spPrinterPath) ', CType(m_PrintSetting.frmhwnd, IntPtr), _ClsLLFunc.LLDocLabel)

				'LL.Core.LlPrintWithBoxStart(projectType, _ClsLLFunc.LLDocName, LlPrintMode.Export, LlBoxType.StandardAbort, CType(m_PrintSetting.frmhwnd, IntPtr), _ClsLLFunc.LLDocLabel)

				'LL.Core.LlPrintSetOption(LlPrintOption.Copies, _ClsLLFunc.LLCopyCount)
				'LL.Core.LlPrintSetOption(LL_PRNOPT_COPIES_SUPPORTED, LL.Core.LlPrintGetOption(LlPrintOption.Copies))

				'If bShowBox Then
				'	LL.Core.LlPrintOptionsDialog(CType(m_PrintSetting.frmhwnd, IntPtr), String.Format("{1}: {2}{0}{3}",
				'																													 vbNewLine,
				'																													 _ClsLLFunc.LLDocLabel,
				'																													 strJobNr,
				'																													 _ClsLLFunc.LLDocName))
				'End If
				'Dim TargetFormat As String = LL.Core.LlPrintGetOptionString(LlPrintOptionString.Export)

				'While Not LL.Core.LlPrint()
				'End While

				''If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				''	Do
				''		' pass data for current record
				''		DefineData(LL, True, rFoundedrec)

				''		While LL.Core.LlPrintFields() = LlConstants.WrnRepeatData
				''			LL.Core.LlPrint()
				''		End While

				''	Loop While rFoundedrec.Read()

				''	While LL.Core.LlPrintFieldsEnd() = LlConstants.WrnRepeatData
				''	End While
				''End If
				'LL.Core.LlPrintEnd(0)

				'If TargetFormat = "PRV" Then
				'LL.Core.LlPreviewDisplay(_ClsLLFunc.LLDocName,
				'												 m_path.GetSpSTempFolder, CType(m_PrintSetting.frmhwnd, IntPtr))
				'	LL.Core.LlPreviewDeleteFiles(_ClsLLFunc.LLDocName, m_path.GetSpSTempFolder)
				'	Return False
				'End If

				success = True

			Catch LlException As LL_User_Aborted_Exception
				Return False
			Catch llException As ListLabelException
				Trace.WriteLine(String.Format("LlException: {0}", Err.Number))

			Catch LlException As Exception
				m_Logger.LogError(String.Format("LlException.{0}: {1}", strMethodeName, LlException.Message))
				success = False

			Finally

			End Try

			Return success

		End Function

		Sub DefineData(ByVal LL As ListLabel, ByVal AsFields As Boolean, ByVal MyDataReader As SqlDataReader)
			Dim i As Integer
			Dim iType As String
			Dim strParam As LlFieldType
			Dim strContent As String = String.Empty

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

			Const LL_OPTION_DELAYTABLEHEADER As Integer = LlOption.DelayTableHeader

			LL.LicensingInfo = ClsMainSetting.GetLL25LicenceInfo()
			LL.Language = LlLanguage.German

			LL.Core.LlSetOption(LL_OPTION_DELAYTABLEHEADER, 0)

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

		Private Function SetLLVariable(ByVal LL As ListLabel) As Boolean
			Dim result As Boolean = True

			' Zusätzliche Variable einfügen
			LL.Variables.Add("AutorFName", m_InitializationData.UserData.UserFName)
			LL.Variables.Add("AutorLName", m_InitializationData.UserData.UserLName)

			LL.Variables.Add("SortBez", m_PrintSetting.ListSortBez)
			LL.Variables.Add("FilterBez", m_PrintSetting.ListFilterBez(0))
			LL.Variables.Add("Dokbez", _ClsLLFunc.ListBez)
			LL.Variables.Add("LAStammFilter", m_PrintSetting.ListFilterBez(0))

			result = result AndAlso GetMDUSData4Print(LL)


			Return result

		End Function

		Private Function GetMDUSData4Print(ByVal LL As ListLabel) As Boolean
			Dim result As Boolean = True
			Dim strFullFilename As String = String.Format("{0}Bild_{1}_{2}.JPG", m_path.GetSpS2DeleteHomeFolder, m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr)

			Dim mdUserData = m_ListingDatabaseAccess.LoadUserAndMandantData(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserFName, m_InitializationData.UserData.UserLName)
			If mdUserData Is Nothing Then
				m_Logger.LogError(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden."))

				Return False

			End If

			'LL.SetDataBinding(mdUserData, String.Empty)

			LL.Variables.Add("MDName", mdUserData.USMDname)
			LL.Variables.Add("MDName2", mdUserData.USMDname2)
			LL.Variables.Add("MDName3", mdUserData.USMDname3)
			LL.Variables.Add("MDPostfach", mdUserData.USMDPostfach)
			LL.Variables.Add("MDStrasse", mdUserData.USMDStrasse)
			LL.Variables.Add("MDPLZ", mdUserData.USMDPlz)
			LL.Variables.Add("MDOrt", mdUserData.USMDOrt)
			LL.Variables.Add("MDLand", mdUserData.USMDLand)

			LL.Variables.Add("MDTelefax", mdUserData.USMDTelefax)
			LL.Variables.Add("MDTelefon", mdUserData.USMDTelefon)
			LL.Variables.Add("MDDTelefon", mdUserData.USMDDTelefon)
			LL.Variables.Add("MDHomepage", mdUserData.USMDHomepage)
			LL.Variables.Add("MDeMail", mdUserData.USMDeMail)

			LL.Variables.Add("USNachName", mdUserData.USNachname)
			LL.Variables.Add("USVorname", mdUserData.USVorname)

			LL.Variables.Add("USTitle1", mdUserData.USTitel_1)
			LL.Variables.Add("USTitle2", mdUserData.USTitel_2)
			LL.Variables.Add("USAbteilung", mdUserData.USAbteilung)
			If File.Exists(strFullFilename) Then
				LL.Variables.Add("USSignFilename", strFullFilename)
			Else
				Dim success = m_Utility.WriteFileBytes(strFullFilename, mdUserData.UserSign)
				LL.Variables.Add("USSignFilename", If(success, strFullFilename, String.Empty))
			End If


			Return result

		End Function




	End Class


	Public Class LAStammPrintSetting

		''' <summary>
		''' The Initialization data.
		''' </summary>
		Public Property m_initData As SP.Infrastructure.Initialization.InitializeClass

		''' <summary>
		''' The translation value helper.
		''' </summary>
		Public Property m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper
		Public Property LADataList As List(Of LAStammData)

		Public Property frmhwnd As String
		Public Property ListSortBez As String
		Public Property ListFilterBez As List(Of String)
		Public Property firstPaymentNumber As Integer()
		Public Property JobNr2Print As String
		Public Property ShowAsDesgin As Boolean
		Public Property ESRFileName As String


	End Class


End Namespace
