

Imports System.ComponentModel
Imports System.IO
Imports combit.ListLabel25
Imports DevExpress.XtraSplashScreen
Imports SP.DatabaseAccess.Employee.DataObjects.DocumentMng
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.Internal.Automations
Imports SP.Internal.Automations.BaseTable

Partial Class ClsLLMATemplatesPrint

	Private m_AvailableEmployeeData As AvailableEmployeeWOSData
	Private m_AvailableEmployeeContactCommData As EmployeeContactComm

	Private m_BaseTableUtil As SPSBaseTables
	Private m_PermissionData As BindingList(Of PermissionData)
	Private m_CountryData As BindingList(Of SP.Internal.Automations.CVLBaseTableViewData)


	Private LL As ListLabel = New ListLabel


	Public Function PrintAssignedEmployeeWOSTemplateData() As Boolean
		Dim result As Boolean = True

		m_CurrentEmployeeNumber = PrintSetting.EmployeeNumbers2Print(0)
		result = result AndAlso LoadEmployeeData()
		result = result AndAlso LoadEmployeeContactCommData()

		m_EmployeeLanguage = m_EmployeeData.Language
		result = result AndAlso LoadPrintJobData()


		If PrintSetting.ShowInDesign Then
			result = result AndAlso ShowAssignedListInDesign()

		ElseIf PrintSetting.CreateExportFile Then
			result = result AndAlso ExportAssignedTemplate()
		End If

		Return result
	End Function


	Private Function LoadPrintJobData() As Boolean

		m_PrintJobData = m_ListingDatabaseAccess.LoadAssignedPrintJobData(m_InitializationData.MDData.MDNr, m_EmployeeLanguage, PrintSetting.TemplateJobNumber)
		If m_PrintJobData Is Nothing Then
			m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Druckdaten konnten nicht geladen werden."))

			Return False
		End If


		Return Not (m_PrintJobData Is Nothing)

	End Function

	Private Function LoadEmployeeData() As Boolean

		m_EmployeeData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(m_CurrentEmployeeNumber, True)
		m_AvailableEmployeeData = m_EmployeeDatabaseAccess.LoadAvailableEmployeeDataForWOSExport(m_CurrentEmployeeNumber, m_InitializationData.UserData.UserNr)

		If m_AvailableEmployeeData Is Nothing Then
			m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kandidatenstammdaten konnten nicht geladen werden."))

			Return False
		End If

		Return Not (m_AvailableEmployeeData Is Nothing)

	End Function

	Private Function LoadEmployeeContactCommData() As Boolean

		m_AvailableEmployeeContactCommData = m_EmployeeDatabaseAccess.LoadEmployeeContactCommData(m_CurrentEmployeeNumber)

		If (m_AvailableEmployeeContactCommData Is Nothing) Then
			m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mitarbeiter-Kontaktdaten konnten nicht geladen werden."))
			Return False
		End If

		Return Not (m_AvailableEmployeeContactCommData Is Nothing)

	End Function

	Private Function ShowAssignedListInDesign() As Boolean
		Dim result As Boolean = True

		Try
			If m_EmployeeData Is Nothing Then
				m_Logger.LogWarning("Keine Daten wurden gefunden.")

				Return False
			End If

			InitLL(LL)
			LL.Variables.Clear()
			LL.Fields.Clear()

			result = result AndAlso SetLLVariable(LL)
			result = result AndAlso DefineAvailableEmployeeData(LL)

			If result Then
				SplashScreenManager.CloseForm(False)
				LL.Core.LlDefineLayout(CType(PrintSetting.FrmHwnd, IntPtr), m_PrintJobData.LLDocLabel,
																 If(m_PrintJobData.LLDocName.ToUpper.EndsWith(".LST".ToUpper), LlProject.List, LlProject.Card),
																 m_PrintJobData.LLDocName)
				LL.Core.LlPrintEnd()
			End If

		Catch LlException As Exception
			m_Logger.LogError(String.Format("LlException: {0}", LlException.ToString))
			LL.Dispose()
			result = False

		Finally

		End Try


		Return result

	End Function

	Private Function ExportAssignedTemplate() As Boolean
		Dim result As Boolean = True

		Try
			If m_EmployeeData Is Nothing Then
				m_Logger.LogWarning("Keine Daten wurden gefunden.")

				Return False
			End If

			Dim tempFileName = Path.Combine(m_path.GetSpSMAHomeFolder, Path.GetRandomFileName())
			tempFileName = System.IO.Path.ChangeExtension(tempFileName, "PDF")

			InitLL(LL)
			LL.Variables.Clear()
			LL.Fields.Clear()

			result = result AndAlso SetLLVariable(LL)
			result = result AndAlso DefineAvailableEmployeeData(LL)

			LL.Core.LlPrintWithBoxStart(If(m_PrintJobData.LLDocName.ToUpper.EndsWith(".LST".ToUpper),
																				 LlProject.List, LlProject.Card),
																				 m_PrintJobData.LLDocName, LlPrintMode.Export,
																				 LlBoxType.None,
																				 CType(PrintSetting.FrmHwnd, IntPtr), m_PrintJobData.LLDocLabel)

			LL.Variables.Add("WOSDoc", 1)

			LL.Core.LlPrintSetOptionString(LlPrintOptionString.Export, "PDF")
			LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.File", Path.GetFileName(tempFileName))
			LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.Path", Path.GetDirectoryName(tempFileName))
			LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.Quiet", "1")

			LL.Core.LlPrintSetOption(LlPrintOption.Dialog_DestinationMask, 2)


			Dim TargetFormat As String = LL.Core.LlPrintGetOptionString(LlPrintOptionString.Export)
			While Not LL.Core.LlPrint()
			End While

			If m_PrintJobData.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				While LL.Core.LlPrintFields() = LlConstants.WrnRepeatData
					LL.Core.LlPrint()
				End While

				While LL.Core.LlPrintFieldsEnd() = LlConstants.WrnRepeatData
				End While
			End If
			LL.Core.LlPrintEnd(0)

			PrintSetting.ExportedFiles = New List(Of String)
			PrintSetting.ExportedFiles.Add(tempFileName)


		Catch LlException As Exception
			m_Logger.LogError(String.Format("LlException: {0}", LlException.ToString))
			LL.Dispose()
			result = False

		Finally

		End Try


		Return result

	End Function

	Private Function SetLLVariable(ByVal LL As ListLabel) As Boolean
		Dim result As Boolean = True

		' Zusätzliche Variable einfügen
		LL.Variables.Add("AutorFName", m_InitializationData.UserData.UserFName)
		LL.Variables.Add("AutorLName", m_InitializationData.UserData.UserLName)
		Dim sortBez As String = String.Empty

		result = result AndAlso GetMDUSData4Print(LL)


		Return result

	End Function

	Private Function GetMDUSData4Print(ByVal LL As ListLabel) As Boolean
		Dim result As Boolean = True

		Dim data = m_ListingDatabaseAccess.LoadUserAndMandantData(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserFName, m_InitializationData.UserData.UserLName)
		If data Is Nothing Then
			m_Logger.LogError(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden."))

			Return False

		End If
		LL.Variables.Add("MDName", data.USMDname)
		LL.Variables.Add("MDName2", data.USMDname2)
		LL.Variables.Add("MDName3", data.USMDname3)
		LL.Variables.Add("MDPostfach", data.USMDPostfach)
		LL.Variables.Add("MDStrasse", data.USMDStrasse)
		LL.Variables.Add("MDPLZ", data.USMDPlz)
		LL.Variables.Add("MDOrt", data.USMDOrt)
		LL.Variables.Add("MDLand", data.USMDLand)

		LL.Variables.Add("MDTelefax", data.USMDTelefax)
		LL.Variables.Add("MDTelefon", data.USMDTelefon)
		LL.Variables.Add("MDDTelefon", data.USMDDTelefon)
		LL.Variables.Add("MDHomepage", data.USMDHomepage)
		LL.Variables.Add("MDeMail", data.USMDeMail)

		LL.Variables.Add("USNachName", data.USNachname)
		LL.Variables.Add("USVorname", data.USVorname)

		LL.Variables.Add("USTitle1", data.USTitel_1)
		LL.Variables.Add("USTitle2", data.USTitel_2)
		LL.Variables.Add("USAbteilung", data.USAbteilung)

		Return result

	End Function

	Private Function DefineAvailableEmployeeData(ByVal LL As ListLabel) As Boolean
		Dim result As Boolean = True


		LL.Core.LlDefineVariableExt("MANachname", ReplaceMissing(m_AvailableEmployeeData.MA_Nachname, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("MAVorname", ReplaceMissing(m_AvailableEmployeeData.MA_Vorname, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("MAgebdat", ReplaceMissing(m_AvailableEmployeeData.MA_GebDat, String.Empty), LlFieldType.Date_Localized)
		LL.Core.LlDefineVariableExt("MAZivil", ReplaceMissing(m_AvailableEmployeeData.MA_Zivil, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("MABeruf", ReplaceMissing(m_EmployeeData.Profession, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("MABeruf", ReplaceMissing(m_EmployeeData.Profession, String.Empty), LlFieldType.Text)
		Dim tmpFilename As String = IO.Path.GetTempFileName
		tmpFilename = IO.Path.ChangeExtension(tmpFilename, "JPG")
		If m_Utility.WriteFileBytes(tmpFilename, m_EmployeeData.MABild) Then
			LL.Core.LlDefineVariableExt("EmployeePictureFilename", ReplaceMissing(tmpFilename, String.Empty), LlFieldType.Text)
		Else
			LL.Core.LlDefineVariableExt("EmployeePictureFilename", String.Empty, LlFieldType.Text)
		End If

		'LL.Variables.Add("EmployeePictureFilename", MainUtilities.GetEmployeePicture(m_EmployeeData.EmployeeNumber))


		LL.Core.LlDefineVariableExt("EmployeeNumber", ReplaceMissing(m_AvailableEmployeeData.EmployeeNumber, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("MA_Nachname", ReplaceMissing(m_AvailableEmployeeData.MA_Nachname, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("MA_Vorname", ReplaceMissing(m_AvailableEmployeeData.MA_Vorname, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("MA_Postfach", ReplaceMissing(m_AvailableEmployeeData.MA_Postfach, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("MA_Strasse", ReplaceMissing(m_AvailableEmployeeData.MA_Strasse, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("MA_PLZ", ReplaceMissing(m_AvailableEmployeeData.MA_PLZ, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("MA_Ort", ReplaceMissing(m_AvailableEmployeeData.MA_Ort, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("MA_Land", ReplaceMissing(m_AvailableEmployeeData.MA_Land, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("MA_Filiale", ReplaceMissing(m_AvailableEmployeeData.MA_Filiale, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("MA_Berater", ReplaceMissing(m_AvailableEmployeeData.MA_Berater, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("MA_Email", ReplaceMissing(m_AvailableEmployeeData.MA_Email, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("MA_AGB_Wos", ReplaceMissing(m_AvailableEmployeeData.MA_AGB_Wos, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("MA_Beruf", ReplaceMissing(m_AvailableEmployeeData.MA_Beruf, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("MA_Branche", ReplaceMissing(m_AvailableEmployeeData.MA_Branche, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("MA_Language", ReplaceMissing(m_AvailableEmployeeData.MA_Language, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("MA_GebDat", ReplaceMissing(m_AvailableEmployeeData.MA_GebDat, String.Empty), LlFieldType.Date_Localized)
		LL.Core.LlDefineVariableExt("MA_Gender", ReplaceMissing(m_AvailableEmployeeData.MA_Gender, String.Empty), LlFieldType.Text)

		LL.Core.LlDefineVariableExt("ArbBegin_Date", ReplaceMissing(m_AvailableEmployeeContactCommData.ESAb, String.Empty), LlFieldType.Date_Localized)
		LL.Core.LlDefineVariableExt("ArbEnd_Date", ReplaceMissing(m_AvailableEmployeeContactCommData.ESEnde, String.Empty), LlFieldType.Date_Localized)
		LL.Core.LlDefineVariableExt("Absenzen", ReplaceMissing(m_AvailableEmployeeContactCommData.Absenzen, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("NoWorkAS", ReplaceMissing(m_AvailableEmployeeContactCommData.NoWorkAS, String.Empty), LlFieldType.Text)

		LL.Core.LlDefineVariableExt("Salutation", ReplaceMissing(m_AvailableEmployeeData.Salutation, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("MA_Zivil", ReplaceMissing(m_AvailableEmployeeData.MA_Zivil, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("MA_Nationality", ReplaceMissing(m_AvailableEmployeeData.MA_Nationality, String.Empty), LlFieldType.Text)

		LL.Core.LlDefineVariableExt("MA_FSchein", ReplaceMissing(m_AvailableEmployeeData.MA_FSchein, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("MA_Auto", ReplaceMissing(m_AvailableEmployeeData.MA_Auto, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("MA_Kontakt", ReplaceMissing(m_AvailableEmployeeData.MA_Kontakt, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("MA_State1", ReplaceMissing(m_AvailableEmployeeData.MA_State1, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("MA_State2", ReplaceMissing(m_AvailableEmployeeData.MA_State2, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("MA_Eigenschaft", ReplaceMissing(m_AvailableEmployeeData.MA_Eigenschaft, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("MA_SSprache", ReplaceMissing(m_AvailableEmployeeData.MA_SSprache, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("MA_MSprache", ReplaceMissing(m_AvailableEmployeeData.MA_MSprache, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("MA_Canton", ReplaceMissing(m_AvailableEmployeeData.MA_Canton, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("MA_Branche", ReplaceMissing(m_AvailableEmployeeData.MA_Branche, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("JobProzent", ReplaceMissing(m_AvailableEmployeeData.JobProzent, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("Permit", ReplaceMissing(m_AvailableEmployeeData.Permit, String.Empty), LlFieldType.Text)

		Dim permissionLabel As String = m_AvailableEmployeeData.Permit
		If Not String.IsNullOrWhiteSpace(permissionLabel) AndAlso Not m_PermissionData Is Nothing AndAlso m_PermissionData.Count > 0 Then
			Dim permissionData = m_PermissionData.Where(Function(x) x.Code = permissionLabel).FirstOrDefault
			If Not permissionData Is Nothing Then
				permissionLabel = permissionData.Translated_Value
			End If
		End If
		LL.Variables.Add("PermissionLabel", permissionLabel)


		Dim nationalityLabel As String = m_AvailableEmployeeData.MA_Nationality
		If Not String.IsNullOrWhiteSpace(nationalityLabel) AndAlso Not m_CountryData Is Nothing AndAlso m_CountryData.Count > 0 Then
			Dim nationalityData = m_CountryData.Where(Function(x) x.Code = nationalityLabel).FirstOrDefault
			If Not nationalityData Is Nothing Then
				nationalityLabel = nationalityData.Translated_Value
			End If
		End If
		LL.Variables.Add("NationalityLabel", nationalityLabel)

		LL.Core.LlDefineVariableExt("MA_Res1", ReplaceMissing(m_AvailableEmployeeData.MA_Res1, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("MA_Res2", ReplaceMissing(m_AvailableEmployeeData.MA_Res2, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("MA_Res3", ReplaceMissing(m_AvailableEmployeeData.MA_Res3, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("MA_Res4", ReplaceMissing(m_AvailableEmployeeData.MA_Res4, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("MA_Res5", ReplaceMissing(m_AvailableEmployeeData.MA_Res5, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("LL_Name", ReplaceMissing(m_AvailableEmployeeData.LL_Name, String.Empty), LlFieldType.Text)

		LL.Core.LlDefineVariableExt("Reserve0", ReplaceMissing(m_AvailableEmployeeData.Reserve0, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("Reserve1", ReplaceMissing(m_AvailableEmployeeData.Reserve1, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("Reserve2", ReplaceMissing(m_AvailableEmployeeData.Reserve2, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("Reserve3", ReplaceMissing(m_AvailableEmployeeData.Reserve3, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("Reserve4", ReplaceMissing(m_AvailableEmployeeData.Reserve4, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("Reserve5", ReplaceMissing(m_AvailableEmployeeData.Reserve5, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("Reserve6", ReplaceMissing(m_AvailableEmployeeData.Reserve6, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("Reserve7", ReplaceMissing(m_AvailableEmployeeData.Reserve7, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("Reserve8", ReplaceMissing(m_AvailableEmployeeData.Reserve8, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("Reserve9", ReplaceMissing(m_AvailableEmployeeData.Reserve9, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("Reserve10", ReplaceMissing(m_AvailableEmployeeData.Reserve10, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("Reserve11", ReplaceMissing(m_AvailableEmployeeData.Reserve11, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("Reserve12", ReplaceMissing(m_AvailableEmployeeData.Reserve12, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("Reserve13", ReplaceMissing(m_AvailableEmployeeData.Reserve13, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("Reserve14", ReplaceMissing(m_AvailableEmployeeData.Reserve14, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("Reserve15", ReplaceMissing(m_AvailableEmployeeData.Reserve15, String.Empty), LlFieldType.Text)

		LL.Core.LlDefineVariableExt("ReserveRtf0", ReplaceMissing(m_AvailableEmployeeData.ReserveRtf0, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("ReserveRtf1", ReplaceMissing(m_AvailableEmployeeData.ReserveRtf1, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("ReserveRtf2", ReplaceMissing(m_AvailableEmployeeData.ReserveRtf2, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("ReserveRtf3", ReplaceMissing(m_AvailableEmployeeData.ReserveRtf3, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("ReserveRtf4", ReplaceMissing(m_AvailableEmployeeData.ReserveRtf4, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("ReserveRtf5", ReplaceMissing(m_AvailableEmployeeData.ReserveRtf5, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("ReserveRtf6", ReplaceMissing(m_AvailableEmployeeData.ReserveRtf6, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("ReserveRtf7", ReplaceMissing(m_AvailableEmployeeData.ReserveRtf7, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("ReserveRtf8", ReplaceMissing(m_AvailableEmployeeData.ReserveRtf8, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("ReserveRtf9", ReplaceMissing(m_AvailableEmployeeData.ReserveRtf9, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("ReserveRtf10", ReplaceMissing(m_AvailableEmployeeData.ReserveRtf10, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("ReserveRtf11", ReplaceMissing(m_AvailableEmployeeData.ReserveRtf11, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("ReserveRtf12", ReplaceMissing(m_AvailableEmployeeData.ReserveRtf12, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("ReserveRtf13", ReplaceMissing(m_AvailableEmployeeData.ReserveRtf13, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("ReserveRtf14", ReplaceMissing(m_AvailableEmployeeData.ReserveRtf14, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("ReserveRtf15", ReplaceMissing(m_AvailableEmployeeData.ReserveRtf15, String.Empty), LlFieldType.Text)


		LL.Core.LlDefineVariableExt("_Reserve0", ReplaceMissing(m_AvailableEmployeeData.ReserveRtf0, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("_Reserve1", ReplaceMissing(m_AvailableEmployeeData.ReserveRtf1, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("_Reserve2", ReplaceMissing(m_AvailableEmployeeData.ReserveRtf2, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("_Reserve3", ReplaceMissing(m_AvailableEmployeeData.ReserveRtf3, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("_Reserve4", ReplaceMissing(m_AvailableEmployeeData.ReserveRtf4, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("_Reserve5", ReplaceMissing(m_AvailableEmployeeData.ReserveRtf5, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("_Reserve6", ReplaceMissing(m_AvailableEmployeeData.ReserveRtf6, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("_Reserve7", ReplaceMissing(m_AvailableEmployeeData.ReserveRtf7, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("_Reserve8", ReplaceMissing(m_AvailableEmployeeData.ReserveRtf8, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("_Reserve9", ReplaceMissing(m_AvailableEmployeeData.ReserveRtf9, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("_Reserve10", ReplaceMissing(m_AvailableEmployeeData.ReserveRtf10, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("_Reserve11", ReplaceMissing(m_AvailableEmployeeData.ReserveRtf11, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("_Reserve12", ReplaceMissing(m_AvailableEmployeeData.ReserveRtf12, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("_Reserve13", ReplaceMissing(m_AvailableEmployeeData.ReserveRtf13, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("_Reserve14", ReplaceMissing(m_AvailableEmployeeData.ReserveRtf14, String.Empty), LlFieldType.Text)
		LL.Core.LlDefineVariableExt("_Reserve15", ReplaceMissing(m_AvailableEmployeeData.ReserveRtf15, String.Empty), LlFieldType.Text)



		Return result

	End Function


#Region "Helpers"


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
		LL.Core.LlSetOption(LL_OPTION_INCLUDEFONTDESCENT, m_PrintJobData.LLFontDesent)
		LL.Core.LlSetOption(LL_OPTION_INCREMENTAL_PREVIEW, m_PrintJobData.LLIncPrv)

		LL.Core.LlSetOption(LL_OPTION_VARSCASESENSITIVE, 0)

		LL.Core.LlSetOption(LL_OPTION_IMMEDIATELASTPAGE, 1)       ' Lastpage
		LL.Core.LlSetOption(LL_OPTION_CONVERTCRLF, 1)             ' Doppelte Zeilenumbruch

		LL.Core.LlSetOption(LL_OPTION_NOPARAMETERCHECK, m_PrintJobData.LLParamCheck)
		LL.Core.LlSetOption(LL_OPTION_XLATVARNAMES, m_PrintJobData.LLKonvertName)

		LL.Core.LlSetOption(LL_OPTION_ESC_CLOSES_PREVIEW, 1)
		LL.Core.LlSetOption(LL_OPTION_SUPERVISOR, 1)
		LL.Core.LlSetOption(LL_OPTION_UISTYLE, LL_OPTION_UISTYLE_OFFICE2003)
		LL.Core.LlSetOption(LL_OPTION_AUTOMULTIPAGE, 1)

		LL.Core.LlSetOption(LL_OPTION_SUPPORTPAGEBREAK, 1)
		LL.Core.LlSetOption(LL_OPTION_PRVZOOM_PERC, m_PrintJobData.LLZoomProz)

		LL.Core.LlPreviewSetTempPath(m_path.GetSpSTempFolder)
		LL.Core.LlSetPrinterDefaultsDir(m_path.GetPrinterHomeFolder)

	End Sub

	''' <summary>
	''' Replaces a missing object with another object.
	''' </summary>
	''' <param name="obj">The object.</param>
	''' <param name="replacementObject">The replacement object.</param>
	''' <returns>The object or the replacement object it the object is nothing.</returns>
	Private Function ReplaceMissing(ByVal obj As Object, ByVal replacementObject As Object) As Object

		If (obj Is Nothing) Then
			Return replacementObject
		Else
			Return obj
		End If

	End Function

#End Region


End Class
