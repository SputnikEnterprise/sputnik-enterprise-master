
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath


Imports System.Data.SqlClient
Imports System.IO
Imports combit.ListLabel25
Imports SPS.Listing.Print.Utility.MADbDatabases.ClsMAStammDb4Print

Imports SP.Infrastructure.Logging
Imports SP.Internal.Automations.BaseTable
Imports SP.Internal.Automations
Imports System.ComponentModel
Imports SP.Infrastructure.UI

Public Class ClsLLMAStammblattPrint
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

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As SP.Infrastructure.Utility


	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI
	Private m_UtilitySP As SPProgUtility.MainUtilities.Utilities

	Private m_path As New ClsProgPath
	Private m_mandant As New Mandant
	Private m_ProgPath As ClsProgPath
	Private MAListPrintSetting As New ClsLLMASearchPrintSetting

	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	Private _ClsReg As New SPProgUtility.ClsDivReg
	Dim _ClsLog As New SPProgUtility.ClsEventLog
	Friend _ClsLLFunc As New ClsLLFunc

	Private strConnString As String = String.Empty
	Private strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

	Private Property USSignFileName As String
	Private Property MAPhotoFileName As String
	Private Property ExistsDocFile As Boolean
	Private Property ExtraVakFieldData As List(Of String)
	Private m_BaseTableUtil As SPSBaseTables

	Private m_PermissionData As BindingList(Of PermissionData)
	Private m_CommunityData As BindingList(Of CommunityData)
	Private m_EmploymentTypeData As BindingList(Of EmploymentTypeData)
	Private m_OtherEmploymentTypeData As BindingList(Of EmploymentTypeData)
	Private m_TypeofStayData As BindingList(Of TypeOfStayData)
	Private m_ForeignCategoryData As BindingList(Of PermissionData)


	Private LL As ListLabel


#Region "Constructor"

	Public Sub New(ByVal _Setting As ClsLLMASearchPrintSetting)

		m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ClsMainSetting.TranslationData, ClsMainSetting.PerosonalizedData, ClsMainSetting.MDData, ClsMainSetting.UserData)
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

		m_UtilityUI = New UtilityUI
		m_Utility = New SP.Infrastructure.Utility
		m_UtilitySP = New SPProgUtility.MainUtilities.Utilities

		m_BaseTableUtil = New SPSBaseTables(m_InitializationData)

		m_PermissionData = m_BaseTableUtil.PerformPermissionDataOverWebService(m_InitializationData.UserData.UserLanguage)
		m_CommunityData = m_BaseTableUtil.PerformCommunityDataOverWebService(String.Empty, m_InitializationData.UserData.UserLanguage)
		m_EmploymentTypeData = m_BaseTableUtil.PerformEmploymentTypeDataOverWebService(m_InitializationData.UserData.UserLanguage)
		m_OtherEmploymentTypeData = m_BaseTableUtil.PerformOtherEmploymentTypeDataOverWebService(m_InitializationData.UserData.UserLanguage)
		m_TypeofStayData = m_BaseTableUtil.PerformTypeOfStayDataOverWebService(m_InitializationData.UserData.UserLanguage)
		m_ForeignCategoryData = m_BaseTableUtil.PerformForeignCategoryDataOverWebService(String.Empty, m_InitializationData.UserData.UserLanguage)


		m_mandant = New Mandant
		m_ProgPath = New ClsProgPath
		LL = New ListLabel

		Me.MAListPrintSetting = _Setting
		Me.strConnString = _Setting.DbConnString2Open
		Me.ExistsDocFile = BuildPrintJob()

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
		Dim JobNr As String = MAListPrintSetting.JobNr2Print

		If JobNr = String.Empty Then
			Dim strMessage As String = "Sie haben für JobNr {1} keine Vorlage ausgewählt.{0}Bitte wählen Sie aus der Liste eine Vorlage aus."
			m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, String.Format(strMessage, vbNewLine, JobNr)))
			MsgBox(String.Format(TranslateMyText(strMessage), vbNewLine, JobNr),
							MsgBoxStyle.Critical, TranslateMyText("Fehlende Vorlage"))
			Return False
		End If
		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
		Dim param As System.Data.SqlClient.SqlParameter

		Try
			Conn.Open()

			param = cmd.Parameters.AddWithValue("@JobNr", MAListPrintSetting.JobNr2Print)
			Dim rDocrec As SqlDataReader = cmd.ExecuteReader                    ' Dokumentendatenbank
			rDocrec.Read()
			If Not String.IsNullOrEmpty(rDocrec("DocName").ToString) Then
				_ClsLLFunc.LLDocName = String.Format("{0}{1}{2}",
																							 m_mandant.GetSelectedMDDocPath(Me.MAListPrintSetting.SelectedMDNr),
																							 If(MAListPrintSetting.SelectedLang <> String.Empty, MAListPrintSetting.SelectedLang & "\", ""),
																							 rDocrec("DocName").ToString)
				If Not File.Exists(_ClsLLFunc.LLDocName) Then
					_ClsLLFunc.LLDocName = String.Format("{0}{1}",
																								 m_mandant.GetSelectedMDDocPath(Me.MAListPrintSetting.SelectedMDNr),
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
					_ClsLLFunc.LLCopyCount = If(MAListPrintSetting.AnzahlCopies = 0, Math.Max(CByte(rDocrec("Anzahlkopien")), 1), MAListPrintSetting.AnzahlCopies)
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
			Dim rFoundedrec As SqlDataReader = OpenDb4MAStammPrint(MAListPrintSetting)
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
			LL.Variables.Add("WOSDoc", 1)

			Dim bewillig = m_UtilitySP.SafeGetString(rFoundedrec, "Bewillig")
			Dim EmploymentType = m_UtilitySP.SafeGetString(rFoundedrec, "EmploymentType")
			Dim OtherEmploymentType = m_UtilitySP.SafeGetString(rFoundedrec, "OtherEmploymentType")
			Dim TypeofStay = m_UtilitySP.SafeGetString(rFoundedrec, "TypeofStay")
			Dim ForeignCategory = m_UtilitySP.SafeGetString(rFoundedrec, "ForeignCategory")

			Dim permissionData = m_PermissionData.Where(Function(x) x.Code = bewillig).FirstOrDefault
			If Not permissionData Is Nothing Then
				LL.Variables.Add("BezBew", permissionData.Translated_Value)
			Else
				LL.Variables.Add("BezBew", bewillig)
			End If

			Dim employmentTypeData = m_EmploymentTypeData.Where(Function(x) x.Rec_Value = EmploymentType).FirstOrDefault
			If Not employmentTypeData Is Nothing Then
				LL.Variables.Add("EmploymentTypeBez", employmentTypeData.Translated_Value)
			Else
				LL.Variables.Add("EmploymentTypeBez", EmploymentType)
			End If
			Dim otherEmploymentTypeData = m_OtherEmploymentTypeData.Where(Function(x) x.Rec_Value = OtherEmploymentType).FirstOrDefault
			If Not otherEmploymentTypeData Is Nothing Then
				LL.Variables.Add("OtherEmploymentTypeBez", otherEmploymentTypeData.Translated_Value)
			Else
				LL.Variables.Add("OtherEmploymentTypeBez", OtherEmploymentType)
			End If
			Dim typeofStayData = m_TypeofStayData.Where(Function(x) x.Rec_Value = TypeofStay).FirstOrDefault
			If Not typeofStayData Is Nothing Then
				LL.Variables.Add("TypeofStayBez", typeofStayData.Translated_Value)
			Else
				LL.Variables.Add("TypeofStayBez", TypeofStay)
			End If
			Dim foreignCategoryData = m_ForeignCategoryData.Where(Function(x) x.Rec_Value = ForeignCategory).FirstOrDefault
			If Not foreignCategoryData Is Nothing Then
				LL.Variables.Add("ForeignCategoryBez", foreignCategoryData.Translated_Value)
			Else
				LL.Variables.Add("ForeignCategoryBez", ForeignCategory)
			End If


			LL.Core.LlDefineLayout(CType(MAListPrintSetting.frmhwnd, IntPtr), _ClsLLFunc.LLDocLabel,
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

		Dim strJobNr As String = Me.MAListPrintSetting.JobNr2Print

		Dim strQuery As String = MAListPrintSetting.SQL2Open
		Dim Conn As SqlConnection = New SqlConnection(strConnString)
		Dim i As Integer = 0

		If Not Me.ExistsDocFile Then Return strResult

		Try
			Dim rFoundedrec As SqlDataReader = OpenDb4MAStammPrint(Me.MAListPrintSetting)
			If Not rFoundedrec.HasRows Then Return TranslateMyText("Keine Daten wurden gefunden...")

			InitLL()
			LL.Variables.Clear()
			LL.Fields.Clear()

			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				DefineData(True, rFoundedrec)
			Else
				DefineData(False, rFoundedrec)
			End If
			SetLLVariable()

			Dim bewillig = m_UtilitySP.SafeGetString(rFoundedrec, "Bewillig")
			Dim EmploymentType = m_UtilitySP.SafeGetString(rFoundedrec, "EmploymentType")
			Dim OtherEmploymentType = m_UtilitySP.SafeGetString(rFoundedrec, "OtherEmploymentType")
			Dim TypeofStay = m_UtilitySP.SafeGetString(rFoundedrec, "TypeofStay")
			Dim ForeignCategory = m_UtilitySP.SafeGetString(rFoundedrec, "ForeignCategory")

			Dim permissionData = m_PermissionData.Where(Function(x) x.Code = bewillig).FirstOrDefault
			If Not permissionData Is Nothing Then
				LL.Variables.Add("BezBew", permissionData.Translated_Value)
			Else
				LL.Variables.Add("BezBew", bewillig)
			End If

			Dim employmentTypeData = m_EmploymentTypeData.Where(Function(x) x.Rec_Value = EmploymentType).FirstOrDefault
			If Not employmentTypeData Is Nothing Then
				LL.Variables.Add("EmploymentTypeBez", employmentTypeData.Translated_Value)
			Else
				LL.Variables.Add("EmploymentTypeBez", EmploymentType)
			End If
			Dim otherEmploymentTypeData = m_OtherEmploymentTypeData.Where(Function(x) x.Rec_Value = OtherEmploymentType).FirstOrDefault
			If Not otherEmploymentTypeData Is Nothing Then
				LL.Variables.Add("OtherEmploymentTypeBez", otherEmploymentTypeData.Translated_Value)
			Else
				LL.Variables.Add("OtherEmploymentTypeBez", OtherEmploymentType)
			End If
			Dim typeofStayData = m_TypeofStayData.Where(Function(x) x.Rec_Value = TypeofStay).FirstOrDefault
			If Not typeofStayData Is Nothing Then
				LL.Variables.Add("TypeofStayBez", typeofStayData.Translated_Value)
			Else
				LL.Variables.Add("TypeofStayBez", TypeofStay)
			End If
			Dim foreignCategoryData = m_ForeignCategoryData.Where(Function(x) x.Rec_Value = ForeignCategory).FirstOrDefault
			If Not foreignCategoryData Is Nothing Then
				LL.Variables.Add("ForeignCategoryBez", foreignCategoryData.Translated_Value)
			Else
				LL.Variables.Add("ForeignCategoryBez", ForeignCategory)
			End If


			LL.Core.LlPrintWithBoxStart(If(_ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper),
																			 LlProject.List, LlProject.Card),
																			 _ClsLLFunc.LLDocName, LlPrintMode.Export, LlBoxType.StandardAbort,
																			 CType(MAListPrintSetting.frmhwnd, IntPtr), _ClsLLFunc.LLDocLabel)

			LL.Core.LlPrintSetOption(LlPrintOption.Copies, _ClsLLFunc.LLCopyCount)
			LL.Core.LlPrintSetOption(LL_PRNOPT_COPIES_SUPPORTED, LL.Core.LlPrintGetOption(LlPrintOption.Copies))

			If bShowBox Then
				LL.Core.LlPrintOptionsDialog(CType(MAListPrintSetting.frmhwnd, IntPtr), String.Format("{1}: {2}{0}{3}",
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
																	 m_path.GetSpSTempFolder, CType(MAListPrintSetting.frmhwnd, IntPtr))
				LL.Core.LlPreviewDeleteFiles(_ClsLLFunc.LLDocName, m_path.GetSpSTempFolder)
				Return "Error"
			End If


		Catch LlException As LL_User_Aborted_Exception
			Return "Error"

		Catch LlException As Exception
			m_Logger.LogError(String.Format("LlException.{0}:Kandidaten:{1}: {2}", strMethodeName, Me.MAListPrintSetting.MANr2Print, LlException.Message))
			strResult = String.Format("Error: {0}:Kandidaten:{1}: {2}", strMethodeName, Me.MAListPrintSetting.MANr2Print, LlException.Message)

		Finally

		End Try

		Return strResult
	End Function

	Function ExportLLDoc() As String
		Dim strResult As String = "Success"
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

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
			Dim rFoundedrec As SqlDataReader = OpenDb4MAStammPrint(Me.MAListPrintSetting)
			If Not rFoundedrec.HasRows Then Return TranslateMyText("Keine Daten wurden gefunden") & "..."

			InitLL()
			LL.Variables.Clear()
			LL.Fields.Clear()

			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				DefineData(True, rFoundedrec)
			Else
				DefineData(False, rFoundedrec)
			End If
			SetLLVariable()

			Dim bewillig = m_UtilitySP.SafeGetString(rFoundedrec, "Bewillig")
			Dim EmploymentType = m_UtilitySP.SafeGetString(rFoundedrec, "EmploymentType")
			Dim OtherEmploymentType = m_UtilitySP.SafeGetString(rFoundedrec, "OtherEmploymentType")
			Dim TypeofStay = m_UtilitySP.SafeGetString(rFoundedrec, "TypeofStay")
			Dim ForeignCategory = m_UtilitySP.SafeGetString(rFoundedrec, "ForeignCategory")

			Dim permissionData = m_PermissionData.Where(Function(x) x.Code = bewillig).FirstOrDefault
			If Not permissionData Is Nothing Then
				LL.Variables.Add("BezBew", permissionData.Translated_Value)
			Else
				LL.Variables.Add("BezBew", bewillig)
			End If

			Dim employmentTypeData = m_EmploymentTypeData.Where(Function(x) x.Rec_Value = EmploymentType).FirstOrDefault
			If Not employmentTypeData Is Nothing Then
				LL.Variables.Add("EmploymentTypeBez", employmentTypeData.Translated_Value)
			Else
				LL.Variables.Add("EmploymentTypeBez", EmploymentType)
			End If
			Dim otherEmploymentTypeData = m_OtherEmploymentTypeData.Where(Function(x) x.Rec_Value = OtherEmploymentType).FirstOrDefault
			If Not otherEmploymentTypeData Is Nothing Then
				LL.Variables.Add("OtherEmploymentTypeBez", otherEmploymentTypeData.Translated_Value)
			Else
				LL.Variables.Add("OtherEmploymentTypeBez", OtherEmploymentType)
			End If
			Dim typeofStayData = m_TypeofStayData.Where(Function(x) x.Rec_Value = TypeofStay).FirstOrDefault
			If Not typeofStayData Is Nothing Then
				LL.Variables.Add("TypeofStayBez", typeofStayData.Translated_Value)
			Else
				LL.Variables.Add("TypeofStayBez", TypeofStay)
			End If
			Dim foreignCategoryData = m_ForeignCategoryData.Where(Function(x) x.Rec_Value = ForeignCategory).FirstOrDefault
			If Not foreignCategoryData Is Nothing Then
				LL.Variables.Add("ForeignCategoryBez", foreignCategoryData.Translated_Value)
			Else
				LL.Variables.Add("ForeignCategoryBez", ForeignCategory)
			End If

			LL.Variables.Add("WOSDoc", 1)

			LL.ExportOptions.Clear()
			SetExportSetting(0)

			LL.Core.LlPrintWithBoxStart(If(_ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper), LlProject.List, LlProject.Card),
																	_ClsLLFunc.LLDocName, LlPrintMode.Export, LlBoxType.StandardAbort, CType(0, IntPtr),
																	_ClsLLFunc.LLDocLabel)

			Dim strExportPfad As String = m_InitializationData.UserData.spTempEmployeePath
			If Not Directory.Exists(strExportPfad) Then Directory.CreateDirectory(m_InitializationData.UserData.spTempEmployeePath)

			Dim strExportFilename As String = String.Format("Kandidatenstamm_{0}.PDF", MAListPrintSetting.MANr2Print)
			If File.Exists(Path.Combine(strExportPfad, strExportFilename)) Then
				Try
					File.Delete(Path.Combine(strExportPfad, strExportFilename))
				Catch ex As Exception
					strExportFilename = String.Format("Kandidatenstamm_{0}_{1}.PDF", MAListPrintSetting.MANr2Print, Environment.TickCount)
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
			Me.MAListPrintSetting.ListOfExportedFilesMAStamm.Add(Path.Combine(strExportPfad, strExportFilename))

			If TargetFormat = "PRV" Then
				LL.Core.LlPreviewDisplay(_ClsLLFunc.LLDocName, m_path.GetSpSTempFolder, CType(MAListPrintSetting.frmhwnd, IntPtr))
				LL.Core.LlPreviewDeleteFiles(_ClsLLFunc.LLDocName, m_path.GetSpSTempFolder)
			End If

			Try
				LL.Dispose()
				LL.Core.Dispose()

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.LL Disposing...:{1}", strMethodeName, ex.ToString))

			End Try

		Catch LlException As Exception
			m_Logger.LogError(String.Format("LlException.{0}:Kandidaten:{1}: {2}", strMethodeName, Me.MAListPrintSetting.MANr2Print, LlException.Message))
			strResult = String.Format("Error: {0}:Kandidaten:{1}: {2}", strMethodeName, Me.MAListPrintSetting.MANr2Print, LlException.Message)

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

		MainUtilities.GetUSData(Me.MAListPrintSetting.DbConnString2Open, _ClsLLFunc, MAListPrintSetting.SelectedMDNr)
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

	Sub InitLL()
		Const LL_DLGBOXMODE_3DBUTTONS As Integer = 32768                       ' 0x8000
		Const LL_DLGBOXMODE_ALT10 As Integer = 11                              ' 0x000B
		Const LL_OPTION_INCLUDEFONTDESCENT As Integer = 79                                       ' 79
		Const LL_OPTION_INCREMENTAL_PREVIEW As Integer = 135                                     ' 135

		Const LL_OPTION_VARSCASESENSITIVE As Integer = 46                                            ' 46
		Const LL_OPTION_IMMEDIATELASTPAGE As Integer = 64                                            ' 64
		Const LL_OPTION_CONVERTCRLF As Integer = 21                                                      ' 21

		Const LL_OPTION_NOPARAMETERCHECK As Integer = 32                                             ' 32
		Const LL_OPTION_XLATVARNAMES As Integer = 51                                                     ' 51

		Const LL_OPTION_ESC_CLOSES_PREVIEW As Integer = 102                                      ' 102
		Const LL_OPTION_SUPERVISOR As Integer = 3                                                            ' 3
		Const LL_OPTION_UISTYLE As Integer = 99                                ' 99
		Const LL_OPTION_UISTYLE_STANDARD As Integer = 0                      ' 2
		'Const LL_OPTION_UISTYLE_OFFICE2003 As Integer = 2                      ' 2
		Const LL_OPTION_AUTOMULTIPAGE As Integer = 42                          ' 42
		Const LL_OPTION_SUPPORTPAGEBREAK As Integer = 10                                             ' 10
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

		LL.Core.LlSetOption(LL_OPTION_IMMEDIATELASTPAGE, 1)             ' Lastpage
		LL.Core.LlSetOption(LL_OPTION_CONVERTCRLF, 1)                           ' Doppelte Zeilenumbruch

		LL.Core.LlSetOption(LL_OPTION_NOPARAMETERCHECK, _ClsLLFunc.LLParamCheck)
		LL.Core.LlSetOption(LL_OPTION_XLATVARNAMES, _ClsLLFunc.LLKonvertName)

		LL.Core.LlSetOption(LL_OPTION_ESC_CLOSES_PREVIEW, 1)
		LL.Core.LlSetOption(LL_OPTION_SUPERVISOR, 1)
		LL.Core.LlSetOption(LL_OPTION_UISTYLE, LL_OPTION_UISTYLE_STANDARD) ' LL_OPTION_UISTYLE_OFFICE2003)

		LL.Core.LlSetOption(LL_OPTION_AUTOMULTIPAGE, 1)

		LL.Core.LlSetOption(LL_OPTION_SUPPORTPAGEBREAK, 1)
		LL.Core.LlSetOption(LL_OPTION_PRVZOOM_PERC, _ClsLLFunc.LLZoomProz)

		LL.Core.LlPreviewSetTempPath(m_path.GetSpSTempFolder)
		LL.Core.LlSetPrinterDefaultsDir(m_path.GetPrinterHomeFolder)

	End Sub

	Sub SetLLVariable()
		Dim aValue As New List(Of String)
		Dim mandantFormXMLFile As String

		mandantFormXMLFile = m_mandant.GetSelectedMDFormDataXMLFilename(MAListPrintSetting.SelectedMDNr)

		' Mandantendaten drucken...
		GetMDUSData4Print()

		' Zusätzliche Variable einfügen
		LL.Variables.Add("AutorFName", _ClsProgSetting.GetUserFName)
		LL.Variables.Add("AutorLName", _ClsProgSetting.GetUserLName)
		LL.Variables.Add("USSignFilename", Me.MAListPrintSetting.USSignFileName)

		' MABild auslesen...
		Me.MAPhotoFileName = GetMAPhoto()
		LL.Variables.Add("PhotoFilename", Me.MAPhotoFileName)

		' Bankdaten zusammenstellen...
		GetMA_BankData_4Print()

		' Bezeichnungen definieren...
		' 1. Reserve
		Dim strMaskLbl As String = "Res1"
		Dim strNodeBez As String = "MA1Res"
		Dim strQuery As String = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
		strMaskLbl = m_ProgPath.GetXMLValueByQueryWithFilename(mandantFormXMLFile, strQuery, "")
		LL.Variables.Add("BezRes_1", strMaskLbl)

		strNodeBez = "MA2Res"
		strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
		strMaskLbl = m_ProgPath.GetXMLValueByQueryWithFilename(mandantFormXMLFile, strQuery, "")
		LL.Variables.Add("BezRes_2", strMaskLbl)

		strNodeBez = "MA3Res"
		strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
		strMaskLbl = m_ProgPath.GetXMLValueByQueryWithFilename(mandantFormXMLFile, strQuery, "")
		LL.Variables.Add("BezRes_3", strMaskLbl)

		strNodeBez = "MA4Res"
		strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
		strMaskLbl = m_ProgPath.GetXMLValueByQueryWithFilename(mandantFormXMLFile, strQuery, "")
		LL.Variables.Add("BezRes_4", strMaskLbl)

		strNodeBez = "MA5Res"
		strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
		strMaskLbl = m_ProgPath.GetXMLValueByQueryWithFilename(mandantFormXMLFile, strQuery, "")
		LL.Variables.Add("BezRes_5", strMaskLbl)

	End Sub

	Sub GetMA_BankData_4Print()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try

			For i As Short = 0 To 3
				LL.Variables.Add(String.Format("DTABCNr{0}", i), "")
				LL.Variables.Add(String.Format("BLZ{0}", i), "")

				LL.Variables.Add(String.Format("Bank{0}", i), "")
				LL.Variables.Add(String.Format("BankOrt{0}", i), "")
				LL.Variables.Add(String.Format("Swift{0}", i), "")

				LL.Variables.Add(String.Format("KontoNr{0}", i), "")
				LL.Variables.Add(String.Format("IBAN{0}", i), "")

				LL.Variables.Add(String.Format("DTAAdr1{0}", i), "")
				LL.Variables.Add(String.Format("DTAAdr2{0}", i), "")

				LL.Variables.Add(String.Format("DTAAdr3{0}", i), "")
				LL.Variables.Add(String.Format("DTAAdr4{0}", i), "")


				LL.Variables.Add(String.Format("BankStd{0}", i), "")
				LL.Variables.Add(String.Format("BankZG{0}", i), "")
				LL.Variables.Add(String.Format("BankAU{0}", i), "")

			Next i

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Daten für Kandidat {1} vorbelegen.{2}", strMethodeName, MAListPrintSetting.MANr2Print, ex.ToString))

		End Try

		Try
			Dim Conn As New SqlConnection(strConnString)
			Dim sSql As String = "[Get MABankData For Print In Stammblatt]"
			Conn.Open()

			Dim cmd As SqlCommand = New SqlCommand(sSql, Conn)
			cmd.CommandType = CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@MANr", MAListPrintSetting.MANr2Print)
			Dim rTemprec As SqlDataReader = cmd.ExecuteReader

			Dim i As Integer = 0
			Try
				While rTemprec.Read
					LL.Variables.Add(String.Format("DTABCNr{0}", i), MainUtilities.GetColumnTextStr(rTemprec, "DTABCNr", ""))
					LL.Variables.Add(String.Format("BLZ{0}", i), MainUtilities.GetColumnTextStr(rTemprec, "BLZ", ""))

					LL.Variables.Add(String.Format("Bank{0}", i), MainUtilities.GetColumnTextStr(rTemprec, "Bank", ""))
					LL.Variables.Add(String.Format("BankOrt{0}", i), MainUtilities.GetColumnTextStr(rTemprec, "BankOrt", ""))
					LL.Variables.Add(String.Format("Swift{0}", i), MainUtilities.GetColumnTextStr(rTemprec, "Swift", ""))

					LL.Variables.Add(String.Format("KontoNr{0}", i), MainUtilities.GetColumnTextStr(rTemprec, "KontoNr", ""))
					LL.Variables.Add(String.Format("IBAN{0}", i), MainUtilities.GetColumnTextStr(rTemprec, "IBANNr", ""))

					LL.Variables.Add(String.Format("DTAAdr1{0}", i), MainUtilities.GetColumnTextStr(rTemprec, "DTAAdr1", ""))
					LL.Variables.Add(String.Format("DTAAdr2{0}", i), MainUtilities.GetColumnTextStr(rTemprec, "DTAAdr2", ""))

					LL.Variables.Add(String.Format("DTAAdr3{0}", i), MainUtilities.GetColumnTextStr(rTemprec, "DTAAdr3", ""))
					LL.Variables.Add(String.Format("DTAAdr4{0}", i), MainUtilities.GetColumnTextStr(rTemprec, "DTAAdr4", ""))

					LL.Variables.Add(String.Format("BankStd{0}", i),
														 If(Not CBool(MainUtilities.GetColumnTextStr(rTemprec, "ActiveRec", 0)), "",
																"Standard Bank"))
					LL.Variables.Add(String.Format("BankZG{0}", i),
														 If(Not CBool(MainUtilities.GetColumnTextStr(rTemprec, "BnkZG", 0)), "",
																"Für Vorschusszahlung"))
					LL.Variables.Add(String.Format("BankAU{0}", i),
														 If(Not CBool(MainUtilities.GetColumnTextStr(rTemprec, "BnkAU", 0)), "",
																"Auslandbank"))

					i += 1
				End While

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Bankdaten für Kandidat {1} lesen.{2}", strMethodeName, MAListPrintSetting.MANr2Print, ex.ToString))

			End Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Kandidat {1}. {2}", strMethodeName, MAListPrintSetting.MANr2Print, ex.ToString))

		End Try

	End Sub

	Function GetMAPhoto() As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Conn As New SqlConnection(strConnString)
		Dim strFullFilename As String = String.Empty
		Dim strFiles As String = String.Empty
		Dim BA As Byte()
		Dim sUSSql As String = "Select MABild, MANr From Mitarbeiter MA Where "
		sUSSql &= String.Format("MANr = {0} And MABild Is Not Null", Me.MAListPrintSetting.MANr2Print)

		Dim i As Integer = 0

		Conn.Open()
		'Dim SQLCmd As SqlCommand = New SqlCommand(sUSSql, Conn)
		Dim SQLCmd_1 As SqlCommand = New SqlCommand(sUSSql, Conn)

		Try

			strFullFilename = String.Format("{0}Bild_{1}.JPG", m_path.GetSpS2DeleteHomeFolder,
																				 System.Guid.NewGuid.ToString())

			Try
				Try
					BA = CType(SQLCmd_1.ExecuteScalar, Byte())
					If BA Is Nothing Then Return String.Empty

				Catch ex As Exception
					Return String.Empty

				End Try

				Dim ArraySize As New Integer
				ArraySize = BA.GetUpperBound(0)

				If File.Exists(strFullFilename) Then File.Delete(strFullFilename)
				Dim fs As New FileStream(strFullFilename, FileMode.CreateNew)
				fs.Write(BA, 0, ArraySize + 1)
				fs.Close()
				fs.Dispose()
				Me.MAPhotoFileName = strFullFilename

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Photo für Kandidat {1} auslsen.{2}", strMethodeName, MAListPrintSetting.MANr2Print, ex.ToString))
			End Try


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Kandidat {1}.{2}", strMethodeName, MAListPrintSetting.MANr2Print, ex.ToString))

		End Try

		Return strFullFilename
	End Function

End Class

