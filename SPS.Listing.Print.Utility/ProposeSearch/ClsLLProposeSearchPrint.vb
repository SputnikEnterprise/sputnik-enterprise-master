
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath

Imports System.Data.SqlClient
Imports System.IO
Imports combit.ListLabel25
Imports SPS.Listing.Print.Utility.MainUtilities
Imports SP.Internal.Automations.BaseTable
Imports SP.Internal.Automations

Imports SP.Infrastructure.Logging
Imports System.ComponentModel
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.Infrastructure.UI

Public Class ClsLLProposeSearchPrint

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

	Private m_connectionString As String

	''' <summary>
	''' The employee data access object.
	''' </summary>
	Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As SP.Infrastructure.Utility
	Private m_UtilityUI As UtilityUI

	Private m_path As New ClsProgPath
	Private m_md As New Mandant
	Private ProposeListPrintSetting As New ClsLLProposeSearchPrintSetting

	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	Private _ClsReg As New SPProgUtility.ClsDivReg
	Private _ClsLog As New SPProgUtility.ClsEventLog
	Friend _ClsLLFunc As New ClsLLFunc

	Private strConnString As String = String.Empty

	Private m_BaseTableUtil As SPSBaseTables
	Private m_PermissionData As BindingList(Of PermissionData)
	Private m_CountryData As BindingList(Of SP.Internal.Automations.CVLBaseTableViewData)

	Private LL As ListLabel = New ListLabel


#Region "private properties"

	Private Property USSignFileName As String
	Private Property PrintAsListing As Boolean
	Private Property ExistsDocFile As Boolean
	Private Property ExtraVakFieldData As List(Of String)

#End Region


#Region "Constructor"

	Public Sub New(ByVal _Setting As ClsLLProposeSearchPrintSetting)

		m_Utility = New SP.Infrastructure.Utility
		m_UtilityUI = New UtilityUI

		m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ClsMainSetting.TranslationData, ClsMainSetting.PerosonalizedData, ClsMainSetting.MDData, ClsMainSetting.UserData)
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

		Me.ProposeListPrintSetting = _Setting
		strConnString = _Setting.DbConnString2Open
		Me.ExistsDocFile = BuildPrintJob()

		m_connectionString = m_InitializationData.MDData.MDDbConn
		m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

		m_BaseTableUtil = New SPSBaseTables(m_InitializationData)
		m_PermissionData = m_BaseTableUtil.PerformPermissionDataOverWebService(m_InitializationData.UserData.UserLanguage)
		m_BaseTableUtil.BaseTableName = "Country"
		m_CountryData = m_BaseTableUtil.PerformCVLBaseTablelistWebserviceCall()


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
		Dim Conn As SqlConnection = New SqlConnection(strConnString)
		Dim bResult As Boolean = True
		Dim JobNr As String = ProposeListPrintSetting.JobNr2Print

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

			param = cmd.Parameters.AddWithValue("@JobNr", ProposeListPrintSetting.JobNr2Print)
			Dim rDocrec As SqlDataReader = cmd.ExecuteReader          ' Dokumentendatenbank
			rDocrec.Read()
			If Not rDocrec.HasRows Then
				m_Logger.LogWarning(String.Format("Doc-Template could not be founded: {1}", ProposeListPrintSetting.JobNr2Print))

				Return False
			End If

			If Not String.IsNullOrEmpty(rDocrec("DocName").ToString) Then
				_ClsLLFunc.LLDocName = m_md.GetSelectedMDDocPath(Me.ProposeListPrintSetting.SelectedMDNr) & rDocrec("DocName").ToString
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
		'Dim LL As New ListLabel

		If Not Me.ExistsDocFile Then Exit Sub

		Try
			Dim rFoundedrec As SqlDataReader
			If Me.ProposeListPrintSetting.IsJobAsListing Then
				rFoundedrec = MainUtilities.OpenDb4PrintListing(strConnString, Me.ProposeListPrintSetting.SQL2Open)

			Else
				rFoundedrec = MainUtilities.OpenProposeDb4PrintDoc(strConnString, Me.ProposeListPrintSetting.SQL2Open, Me.ProposeListPrintSetting,
																						   Me.ProposeListPrintSetting.ProposeNr2Print)
			End If
			If Not rFoundedrec.HasRows Then
				m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, "Keine Daten wurden gefunden..."))
				Exit Sub
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

			LL.Core.LlDefineLayout(CType(0, IntPtr), _ClsLLFunc.LLDocLabel,
								   If(_ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper), LlProject.List, LlProject.Card),
								   _ClsLLFunc.LLDocName)

		Catch LlException As Exception
			m_Logger.LogDebug(String.Format("LlException.{0}: {1}", strMethodeName, LlException.Message))


		Finally

		End Try

	End Sub

	Function ShowInPrint(ByRef bShowBox As Boolean) As String
		Const LL_PRNOPT_COPIES_SUPPORTED As Integer = 3
		Dim strResult As String = "Success"
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strJobNr As String = Me.ProposeListPrintSetting.JobNr2Print

		Try
			Dim bAllowedtoDebug As Boolean = CBool(_ClsProgSetting.GetMDProfilValue("Sonstiges", "EnableLLDebug", "0"))
			If bAllowedtoDebug Then
				LL.Debug = LlDebug.LogToFile
				LL.Debug = LlDebug.Enabled
			End If
		Catch ex As Exception
			m_Logger.LogError(String.Format("EnablingLLDebuging.{0}: {1}", strMethodeName, ex))

		End Try

		Dim strQuery As String = ProposeListPrintSetting.SQL2Open
		Dim Conn As SqlConnection = New SqlConnection(strConnString)
		Dim i As Integer = 0

		If Not Me.ExistsDocFile Then Return TranslateMyText("Keine Dokumente wurden gefunden.")

		Try
			Dim rFoundedrec As SqlDataReader
			If Me.ProposeListPrintSetting.IsJobAsListing Then
				rFoundedrec = MainUtilities.OpenDb4PrintListing(strConnString, Me.ProposeListPrintSetting.SQL2Open)

			Else
				rFoundedrec = MainUtilities.OpenProposeDb4PrintDoc(strConnString, Me.ProposeListPrintSetting.SQL2Open, Me.ProposeListPrintSetting,
																						   Me.ProposeListPrintSetting.ProposeNr2Print)
			End If
			If Not rFoundedrec.HasRows Then Return TranslateMyText("Keine Daten wurden gefunden") & "..."

			InitLL(LL)
			LL.Variables.Clear()
			LL.Fields.Clear()



			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				DefineData(LL, True, rFoundedrec)
			Else
				DefineData(LL, False, rFoundedrec)
			End If
			SetLLVariable(LL, rFoundedrec("MAnr"))

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
										 m_path.GetSpSTempFolder, CType(0, IntPtr))
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

		If strLLTemplate.ToUpper.Contains(".PDF") Then
			Dim filename As String = New FileInfo(strLLTemplate).Name
			Try
				strResult = Path.Combine(m_md.GetSelectedMDTemplatePath(ProposeListPrintSetting.SelectedMDNr), filename)
				File.Copy(Path.Combine(m_md.GetSelectedMDTemplatePath(ProposeListPrintSetting.SelectedMDNr), filename),
								Path.Combine(m_path.GetSpSOfferHomeFolder, filename), True)
				Return Path.Combine(m_path.GetSpSOfferHomeFolder, filename)

			Catch ex As Exception
				m_Logger.LogError(String.Format("Fehler beim Kopieren der Datei: {0} -> {1}",
																				Path.Combine(m_md.GetSelectedMDTemplatePath(ProposeListPrintSetting.SelectedMDNr), filename), Path.Combine(m_path.GetSpSOfferHomeFolder, filename)))
				Return String.Empty
			End Try

		End If

		'Dim LL As New ListLabel
		Dim strJobNr As String = Me.ProposeListPrintSetting.JobNr2Print

		Dim strQuery As String = ProposeListPrintSetting.SQL2Open

		Dim i As Integer = 0

		Try
			Dim rFoundedrec As SqlDataReader
			If Me.ProposeListPrintSetting.IsJobAsListing Then
				rFoundedrec = MainUtilities.OpenDb4PrintListing(strConnString, Me.ProposeListPrintSetting.SQL2Open)

			Else
				rFoundedrec = MainUtilities.OpenProposeDb4PrintDoc(strConnString, Me.ProposeListPrintSetting.SQL2Open, Me.ProposeListPrintSetting,
																						   Me.ProposeListPrintSetting.ProposeNr2Print)
			End If
			If Not rFoundedrec.HasRows Then Return TranslateMyText("Keine Daten wurden gefunden") & "..."

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

			LL.Core.LlPrintWithBoxStart(If(_ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper), LlProject.List, LlProject.Card),
										_ClsLLFunc.LLDocName, LlPrintMode.Export, LlBoxType.StandardAbort, CType(0, IntPtr),
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
										 m_path.GetSpSTempFolder,
										 CType(0, IntPtr))
				LL.Core.LlPreviewDeleteFiles(_ClsLLFunc.LLDocName,
											 m_path.GetSpSTempFolder)
			End If
			strResult = String.Format("{0}{1}", m_path.GetSpSOfferHomeFolder, _ClsLLFunc.LLExporterFileName)


		Catch LlException As Exception
			m_Logger.LogError(String.Format("LlException.{0}: {1}", strMethodeName, LlException.Message))
			strResult = False

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

					'Case "SYSTEM.BYTE[]".ToUpper
					'	strParam = LlFieldType.Drawing_hBitmap
					'	If IsDBNull(MyDataReader.Item(i)) Then
					'		strContent = Nothing
					'	Else
					'		fieldasBitmap = True
					'		'						strContent = MyDataReader.Item(i)
					'	End If

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

	Sub GetMDUSData4Print(ByVal LL As ListLabel)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		MainUtilities.GetUSData(Me.ProposeListPrintSetting.DbConnString2Open, _ClsLLFunc, 0)
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
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

		End Try

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

	Sub SetLLVariable(ByVal LL As ListLabel, ByVal employeeNumber As Integer)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim aValue As New List(Of String)

		Try

			' Zusätzliche Variable einfügen
			LL.Variables.Add("AutorFName", _ClsProgSetting.GetUserFName)
			LL.Variables.Add("AutorLName", _ClsProgSetting.GetUserLName)
			LL.Variables.Add("USSignFilename", Me.ProposeListPrintSetting.USSignFileName)
			LL.Variables.Add("USPictureFilename", String.Empty)
			LL.Variables.Add("EmployeePictureFilename", String.Empty)

			Try
				LL.Variables.Add("USPictureFilename", MainUtilities.GetUSPicture(Me.ProposeListPrintSetting.DbConnString2Open))
				LL.Variables.Add("EmployeePictureFilename", MainUtilities.GetEmployeePicture(employeeNumber))
			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
			End Try

			If Me.ProposeListPrintSetting.IsJobAsListing Then
				LL.Variables.Add("SortBez", ProposeListPrintSetting.ListSortBez)
				LL.Variables.Add("FilterBez", ProposeListPrintSetting.ListFilterBez(0))
				LL.Variables.Add("FilterBez2", ProposeListPrintSetting.ListFilterBez(1))
				LL.Variables.Add("FilterBez3", ProposeListPrintSetting.ListFilterBez(2))
				LL.Variables.Add("FilterBez4", ProposeListPrintSetting.ListFilterBez(3))
			End If
			LL.Variables.Add("Dokbez", _ClsLLFunc.ListBez)


			Dim employeeData As EmployeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(employeeNumber, False)
			If employeeData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kandidatendaten konnten nicht geladen werden."))

				Return
			End If
			Dim permissionLabel As String = employeeData.Permission
			If Not String.IsNullOrWhiteSpace(permissionLabel) AndAlso Not m_PermissionData Is Nothing AndAlso m_PermissionData.Count > 0 Then
				Dim permissionData = m_PermissionData.Where(Function(x) x.Code = permissionLabel).FirstOrDefault
				If Not permissionData Is Nothing Then
					permissionLabel = permissionData.Translated_Value
				End If
			End If
			LL.Variables.Add("BezBew", permissionLabel)
			LL.Variables.Add("PermissionLabel", permissionLabel)


			Dim nationalityLabel As String = employeeData.Nationality
			If Not String.IsNullOrWhiteSpace(nationalityLabel) AndAlso Not m_CountryData Is Nothing AndAlso m_CountryData.Count > 0 Then
				Dim nationalityData = m_CountryData.Where(Function(x) x.Code = nationalityLabel).FirstOrDefault
				If Not nationalityData Is Nothing Then
					nationalityLabel = nationalityData.Translated_Value
				End If
			End If
			LL.Variables.Add("NationalityLabel", nationalityLabel)


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

		End Try

		GetMDUSData4Print(LL)

	End Sub



End Class
