
Imports System.Threading

Imports SP.MA.EmployeeMng.UI
Imports SP.MA.ApplicantMng.UI
Imports SP.KD.CPersonMng.UI
Imports SP.KD.CustomerMng.UI
Imports SP.KD.InvoiceMng.UI
Imports SP.MA.ReportMng.UI

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SP.MA.EinsatzMng.UI
Imports SP.MA.EinsatzMng

Imports SP.MA.AdvancePaymentMng
Imports SP.MA.AdvancePaymentMng.UI
Imports SP.MA.PayrollMng.UI
Imports SPS.DTAUtility.UI
Imports SPS.ESRUtility.UI

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages

Imports SPS.SYS.TableSettingMng
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports System.Data.SqlClient
Imports SPS.MainView.DataBaseAccess
Imports SPS.Listing.Print.Utility.InvoicePrint
Imports SPS.Listing.Print.Utility
Imports SPRPListSearch
Imports SP.DatabaseAccess.PayrollMng

Public Class ClsOpenModul
	Private Shared m_Logger As ILogger = New Logger()

	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	Private Property _ClsSetting As New ClsSetting

	Private m_ModuleCache As SP.ModuleCaching.ModuleCache

	Protected m_UtilityUI As UtilityUI
	Protected m_Translate As TranslateValues

	' Constractors
	Public Sub New(ByVal _setting As ClsSetting)

		Me._ClsSetting = _setting

		m_UtilityUI = New UtilityUI
		m_Translate = New TranslateValues

	End Sub


#Region "Kandidatenmaske öffnen..."

	''' <summary>
	''' startet das Threading zum öffnen der Kandidatenverwaltung
	''' </summary>
	''' <remarks></remarks>
	Sub OpenSelectedEmployee(ByVal mandantNumber As Integer, ByVal userNumber As Integer)
		Try
			Dim Employeenumber As Integer? = Me._ClsSetting.SelectedMANr
			If Employeenumber.HasValue AndAlso Employeenumber = 0 Then Employeenumber = Nothing

			If Not IsUserActionAllowed(userNumber, 101, mandantNumber) Then Return

			Dim frm As frmEmployees = CType(ModulConstants.GetModuleCach.GetModuleForm(mandantNumber, userNumber, SP.ModuleCaching.ModuleName.EmployeeMng), frmEmployees)

			If Employeenumber.HasValue Then
				frm.LoadEmployeeData(Employeenumber)

				If frm.IsEmployeeDataLoaded Then
					frm.Show()
					frm.BringToFront()
				End If

			Else

				CreateNewEmployee(mandantNumber, userNumber)

			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
		End Try

	End Sub

	Sub CreateNewEmployee(ByVal mandantNumber As Integer, ByVal userNumber As Integer)
		Dim preselection = New SP.MA.EmployeeMng.PreselectionData With
											 {
												 .MDNr = mandantNumber,
												 .Advisor = ModulConstants.UserData.UserKST
											 }
		m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ModulConstants.TranslationData,
																																								ModulConstants.ProsonalizedData,
																																								ModulConstants.MDData,
																																								ModulConstants.UserData)

		Dim frmNewEmployee As SP.MA.EmployeeMng.UI.frmNewEmployee = New SP.MA.EmployeeMng.UI.frmNewEmployee(m_InitializationData, preselection)
		frmNewEmployee.Show()
		frmNewEmployee.BringToFront()

	End Sub

	Sub OpenSelectedApplicant(ByVal mandantNumber As Integer, ByVal userNumber As Integer)
		Try
			Dim Employeenumber As Integer? = Me._ClsSetting.SelectedMANr
			If Employeenumber.HasValue AndAlso Employeenumber = 0 Then Employeenumber = Nothing

			If Not IsUserActionAllowed(userNumber, 101, mandantNumber) Then Exit Sub

			Dim frm As frmApplicant = CType(ModulConstants.GetModuleCach.GetModuleForm(mandantNumber, userNumber, SP.ModuleCaching.ModuleName.ApplicantMng), frmApplicant)

			If Employeenumber.HasValue Then
				frm.LoadEmployeeData(Employeenumber)

				If frm.IsEmployeeDataLoaded Then
					frm.Show()
					frm.BringToFront()
				End If

			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
		End Try

	End Sub

#End Region


#Region "Kundennmaske öffnen..."

	''' <summary>
	''' öffnen der Kandidatenverwaltung
	''' </summary>
	''' <remarks></remarks>
	Sub OpenSelectedCustomer(ByVal mandantNumber As Integer, ByVal userNumber As Integer)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		If Not IsUserActionAllowed(userNumber, 201, mandantNumber) Then Exit Sub
		m_Logger.LogDebug(String.Format("mandantnumber: {0} ,usernumber: {1}, _clssetting.SelectedMDNr: {2}, _clssetting.SelectedKDNr: {3}", mandantNumber, userNumber, _ClsSetting.SelectedMDNr, _ClsSetting.SelectedKDNr))

		Try
			Dim frm As frmCustomers = CType(ModulConstants.GetModuleCach.GetModuleForm(mandantNumber, userNumber, SP.ModuleCaching.ModuleName.CustomerMng), frmCustomers)

			If Me._ClsSetting.SelectedKDNr > 0 Then
				frm.LoadCustomerData(Me._ClsSetting.SelectedKDNr)
			Else
				frm.LoadCustomerData(Nothing)
			End If
			If frm.IsCustomerDataLoaded Then
				frm.Show()
				frm.BringToFront()
			End If


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.ToString))
			ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	''' <summary>
	''' öffnet die Zuständige Person
	''' </summary>
	''' <remarks></remarks>
	Sub OpenSelectedCPerson() 'ByVal mandantNumber As Integer, ByVal userNumber As Integer)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		'If userNumber = 0 Then userNumber = ModulConstants.UserData.UserNr
		'If mandantNumber = 0 Then mandantNumber = ModulConstants.MDData.MDNr

		If Not IsUserActionAllowed(ModulConstants.UserData.UserNr, 201) Then Exit Sub

		Try
			Dim responsiblePersonsFrom = New frmResponsiblePerson(CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr))
			Dim customerNumber As Integer
			If Integer.TryParse(Me._ClsSetting.SelectedKDNr, customerNumber) Then
				If (responsiblePersonsFrom.LoadResponsiblePersonData(customerNumber, Me._ClsSetting.SelectedZHDNr)) Then
					responsiblePersonsFrom.Show()
				End If
			End If


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.ToString))
			ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.ToString))

		End Try

	End Sub


#End Region

	Sub OpenSelectedVacancyTiny(ByVal mandantNumber As Integer, ByVal userNumber As Integer)
		'Sub OpenSelectedVacancy(ByVal mandantNumber As Integer, ByVal userNumber As Integer)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = "Success..."
		Dim liLONr As New List(Of Integer)

		If userNumber = 0 Then userNumber = ModulConstants.UserData.UserNr
		If mandantNumber = 0 Then mandantNumber = _ClsSetting.SelectedMDNr
		If Not IsUserActionAllowed(userNumber, 701) Then m_Logger.LogWarning("No rights...") : Exit Sub

		Try
			Dim init = CreateInitialData(mandantNumber, userNumber)
			Dim frmVacancy = New SPKD.Vakanz.frmVakanzen(init)
			Dim setting = New SPKD.Vakanz.ClsVakSetting With {.SelectedVakNr = Me._ClsSetting.SelectedVakNr,
				.SelectedKDNr = Me._ClsSetting.SelectedKDNr,
				.SelectedZHDNr = Me._ClsSetting.SelectedZHDNr}
			frmVacancy.VacancySetting = setting
			If Not frmVacancy.LoadData Then Return

			frmVacancy.Show()
			frmVacancy.BringToFront()


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.ToString))
			ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	''' <summary>
	''' startet Vakanzenverwaltung
	''' </summary>
	''' <remarks></remarks>
	Sub OpenSelectedVacancy()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = "Success..."
		Dim liLONr As New List(Of Integer)

		If Not IsUserActionAllowed(ModulConstants.UserData.UserNr, 701) Then m_Logger.LogWarning("No rights...") : Exit Sub

		Try
			Dim init = CreateInitialData(_ClsSetting.SelectedMDNr, ModulConstants.UserData.UserNr)
			Dim frmVacancy = New SPKD.Vakanz.frmVakanzen(init)
			Dim setting = New SPKD.Vakanz.ClsVakSetting With {.SelectedVakNr = Me._ClsSetting.SelectedVakNr, .SelectedKDNr = Me._ClsSetting.SelectedKDNr, .SelectedZHDNr = Me._ClsSetting.SelectedZHDNr}
			frmVacancy.VacancySetting = setting
			If Not frmVacancy.LoadData Then Return

			frmVacancy.Show()
			frmVacancy.BringToFront()


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.ToString))
			ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	''' <summary>
	''' startet Vorschlagverwaltung
	''' </summary>
	''' <remarks></remarks>
	Sub OpenSelectedProposeTiny(ByVal mandantNumber As Integer, ByVal userNumber As Integer)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = "Success..."

		If mandantNumber = 0 Then mandantNumber = ModulConstants.MDData.MDNr
		If userNumber = 0 Then userNumber = ModulConstants.UserData.UserNr
		Dim init = CreateInitialData(mandantNumber, userNumber)

		If Not IsUserActionAllowed(userNumber, 801) Then m_Logger.LogWarning("No rights...") : Exit Sub

		Try
			Dim obj As New SPProposeUtility.ClsMain_Net(init)
			If Me._ClsSetting.SelectedProposeNr.GetValueOrDefault(0) = 0 Then

				obj.ShowfrmProposal(Me._ClsSetting.SelectedMANr,
														Me._ClsSetting.SelectedKDNr,
														Me._ClsSetting.SelectedZHDNr,
														Me._ClsSetting.SelectedVakNr)
			Else
				obj.ShowfrmProposal(Me._ClsSetting.SelectedProposeNr)
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.ToString))
			ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Sub OpenSelectedLO()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = "Success..."
		Dim liLONr As New List(Of Integer)


		If Not IsUserActionAllowed(ModulConstants.UserData.UserNr, 553) And Not IsUserActionAllowed(ModulConstants.UserData.UserNr, 554) Then m_Logger.LogWarning("No rights...") : Exit Sub

		Try
			Dim init = CreateInitialData(_ClsSetting.SelectedMDNr, ModulConstants.UserData.UserNr)

			Dim _settring As New SP.LO.PrintUtility.ClsLOSetting With
				{.SelectedMDNr = Me._ClsSetting.SelectedMDNr,
				 .SelectedMANr = New List(Of Integer)(New Integer() {Me._ClsSetting.SelectedMANr}),
				 .SelectedLONr = New List(Of Integer)(New Integer() {Me._ClsSetting.SelectedLONr}),
				 .SelectedMonth = New List(Of Integer)(New Integer() {If(Me._ClsSetting.SelectedMonth.HasValue, Me._ClsSetting.SelectedMonth, 0)}),
				 .SelectedYear = New List(Of Integer)(New Integer() {If(Me._ClsSetting.SelectedYear.HasValue, Me._ClsSetting.SelectedYear, 0)}), .SearchAutomatic = True}

			'Dim obj As New SP.LO.PrintUtility.ClsMain_Net(init, _settring)
			'obj.ShowfrmLO4Details()

			Dim obj As New SP.LO.PrintUtility.frmLOPrint(init)
			obj.LOSetting = _settring

			obj.Show()
			obj.BringToFront()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.ToString))
			ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Sub DeleteAssignedInvalidPayroll(ByVal payrollNumber As Integer)
		Dim success As Boolean = True
		Dim msg As String

		msg = m_Translate.GetSafeTranslationValue("Die Lohnabrechnung scheint fehlerhaft zu sein. Ich werde die Abrechnung für Sie löschen!")
		m_UtilityUI.ShowOKDialog(msg, m_Translate.GetSafeTranslationValue("Fehlerhaft Abrechnung löschen"), MessageBoxIcon.Information)

		m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ModulConstants.TranslationData, ModulConstants.ProsonalizedData, ModulConstants.MDData, ModulConstants.UserData)
		Dim payrollDatabaseAccess = New PayrollDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

		success = success AndAlso payrollDatabaseAccess.DeleteAssignedInvalidPayroll(payrollNumber)

		If success Then
			msg = m_Translate.GetSafeTranslationValue("Fehlerhaft Lohnabrechnung wurde erfolgreich gelöscht. Sie dürfen jetzt die Lohnabrechnung neu erstellen.")
			m_UtilityUI.ShowOKDialog(msg, m_Translate.GetSafeTranslationValue("Fehlerhaft Abrechnung löschen"), MessageBoxIcon.Information)

		Else
			msg = m_Translate.GetSafeTranslationValue("Fehlerhaft Lohnabrechnung konnte <b>nicht</b> gelöscht werden! Sie dürfen jetzt die Lohnabrechnung neu erstellen.")
			m_UtilityUI.ShowOKDialog(msg, m_Translate.GetSafeTranslationValue("Fehlerhaft Abrechnung löschen"), MessageBoxIcon.Asterisk)

		End If

	End Sub


#Region "Offertenverwaltung öffnen..."

	''' <summary>
	''' startet das Threading zum öffnen der Offertenverwaltung
	''' </summary>
	''' <remarks></remarks>
	Sub OpenSelectedOffer()

		If Not IsUserActionAllowed(ModulConstants.UserData.UserNr, 801) Then Exit Sub

		Dim t As Thread = New Thread(AddressOf OpenOfferWithThreading)

		t.IsBackground = True
		t.Name = "OpenOfferWithThreading"
		t.Start()

	End Sub

	''' <summary>
	''' öffnet die Offermaske über Hauptübersicht
	''' </summary>
	''' <remarks></remarks>
	Private Sub OpenOfferWithThreading()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		' Dim oMyProg As Object
		Dim strTranslationProgName As String = String.Empty
		Dim _ClsReg As New SPProgUtility.ClsDivReg

		Try
			ModulConstants.vb6Object.TranslateProg4Net("OfferUtility.ClsMain", CInt(Me._ClsSetting.SelectedOfferNr))


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.ToString))
			ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Sub SendOffertWithMail()

		Dim iOfferNr As Integer? = Me._ClsSetting.SelectedOfferNr ' 2
		Dim iKDTempNr As Integer? = Me._ClsSetting.SelectedKDNr ' 19642
		Dim iKDZTempNr As Integer? = Me._ClsSetting.SelectedZHDNr ' 104621
		Dim bSendAsTest As Boolean = True
		Dim streMailField As String = "KDeMail"
		Dim strGuid As String = String.Empty
		Dim strJobNr As String = "15.1.1"
		Dim strFileToSend As String = ""

		If Not iOfferNr.HasValue OrElse Not iKDTempNr.HasValue OrElse Not iKDZTempNr.HasValue Then
			m_Logger.LogWarning("Empty Customer, Responsible Person or Offernumber!")
			Return
		End If

		Try
			Dim o2Open_1 As New SPSSendMail.ClsMain_Net
			o2Open_1.SendOfferWithMail(iOfferNr, 0, iKDTempNr, iKDZTempNr, False, True, True, True, streMailField,
																 System.Guid.NewGuid.ToString, "", "", strFileToSend) 'strOfferblattFileName)

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		End Try

	End Sub

#End Region


#Region "Einsatzmaske öffnen / Neue..."

	Public Sub OpenSelectedES(ByVal mandantNumber As Integer, ByVal userNumber As Integer)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim ESnumber As Integer? = Me._ClsSetting.SelectedESNr
		If ESnumber.HasValue AndAlso ESnumber = 0 Then ESnumber = Nothing

		If Not IsUserActionAllowed(userNumber, 250, mandantNumber) Then Exit Sub
		If Not _ClsSetting.SelectedMDNr.HasValue Then _ClsSetting.SelectedMDNr = ModulConstants.MDData.MDNr

		Try
			Dim frmEinsatz As SP.MA.EinsatzMng.UI.frmES = CType(ModulConstants.GetModuleCach.GetModuleForm(mandantNumber, userNumber, SP.ModuleCaching.ModuleName.ESMng), frmES)

			If ESnumber.HasValue Then
				frmEinsatz.LoadESData(ESnumber)
			Else
				CreateNewES(mandantNumber, userNumber)
				Exit Sub
			End If
			If frmEinsatz.IsESDataLoaded Then
				frmEinsatz.Show()
				frmEinsatz.BringToFront()
			End If


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.ToString))
			ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Public Sub CreateNewES(ByVal mandantNumber As Integer, ByVal userNumber As Integer)

		Dim preselection = New SP.MA.EinsatzMng.PreselectionData With {.MDNr = mandantNumber,
																									.EmployeeNumber = Me._ClsSetting.SelectedMANr,
																									.CustomerNumber = Me._ClsSetting.SelectedKDNr,
																									.ResponsiblePersonNumber = Me._ClsSetting.SelectedZHDNr,
																									.ESAls = Nothing,
																									.ESAb = New DateTime(Now.Year, Now.Month, Now.Day),
																									.VAKNr = Me._ClsSetting.SelectedVakNr,
																									.PNR = Me._ClsSetting.SelectedProposeNr,
																									.CustomerKST = Nothing,
																									.BeraterMA = ModulConstants.UserData.UserKST,
																									.BeraterKD = ModulConstants.UserData.UserKST
																								 }

		m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ModulConstants.TranslationData,
																																								ModulConstants.ProsonalizedData,
																																								ModulConstants.MDData,
																																								ModulConstants.UserData)
		Dim frmNewEs As SP.MA.EinsatzMng.UI.frmNewES = New SP.MA.EinsatzMng.UI.frmNewES(m_InitializationData, preselection)

		frmNewEs.Show()
		frmNewEs.BringToFront()

	End Sub

	''' <summary>
	''' startet das Threading zum öffnen der Einsatzverwaltung
	''' </summary>
	''' <remarks></remarks>
	Public Sub OpenNewESForm(ByVal mandantNumber As Integer, ByVal userNumber As Integer)

		If Not IsUserActionAllowed(userNumber, 251, mandantNumber) Then Exit Sub

		Dim preselection = New SP.MA.EinsatzMng.PreselectionData With {.MDNr = mandantNumber,
																									.EmployeeNumber = _ClsSetting.SelectedMANr,
																									.CustomerNumber = _ClsSetting.SelectedKDNr,
																									.ResponsiblePersonNumber = Nothing,
																									.ESAls = Nothing,
																									.ESAb = New DateTime(Now.Year, Now.Month, Now.Day),
																									.VAKNr = Nothing,
																									.PNR = Nothing,
																									.CustomerKST = Nothing,
																									.BeraterMA = Nothing,
																									.BeraterKD = Nothing
																								 }

		m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ModulConstants.TranslationData,
																																								ModulConstants.ProsonalizedData,
																																								ModulConstants.MDData,
																																								ModulConstants.UserData)
		Dim frmNewEs As SP.MA.EinsatzMng.UI.frmNewES = New SP.MA.EinsatzMng.UI.frmNewES(m_InitializationData, preselection)

		frmNewEs.Show()
		frmNewEs.BringToFront()

	End Sub

#End Region


#Region "Rapporte öffnen..."

	''' <summary>
	''' startet das Threading zum öffnen der Rapportverwaltung
	''' </summary>
	''' <remarks></remarks>
	Sub OpenSelectedReport(ByVal mandantNumber As Integer, ByVal userNumber As Integer)

		If Not IsUserActionAllowed(userNumber, 300, mandantNumber) Then Exit Sub

		Dim frmReportMng As SP.MA.ReportMng.UI.frmReportMng = CType(ModulConstants.GetModuleCach.GetModuleForm(mandantNumber, userNumber, SP.ModuleCaching.ModuleName.ReportMng), frmReportMng)

		Dim rpNumber As Integer
		If Integer.TryParse(Me._ClsSetting.SelectedRPNr, rpNumber) Then

			frmReportMng.LoadReportData(rpNumber)
			frmReportMng.Show()
			frmReportMng.BringToFront()

		End If

	End Sub

	''' <summary>
	''' öffnet die Rapportverwaltung über Hauptübersicht
	''' </summary>
	''' <remarks></remarks>
	Sub OpenReportFormForNewRP()

		If Me._ClsSetting.SelectedMDNr <> ModulConstants.MDData.MDNr OrElse m_InitializationData Is Nothing Then
			m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ModulConstants.TranslationData,
																																						ModulConstants.ProsonalizedData,
																																						ModulConstants.MDData,
																																						ModulConstants.UserData)
		End If

		Dim frmNewReportMng As SP.MA.ReportMng.UI.frmNewReport = New SP.MA.ReportMng.UI.frmNewReport(m_InitializationData)
		frmNewReportMng.LoadPayrollData()
		frmNewReportMng.Show()
		frmNewReportMng.BringToFront()

	End Sub

#End Region


#Region "Vorschuss öffnen..."


	Sub OpenSelectedAdvancePayment(ByVal mandantNumber As Integer, ByVal userNumber As Integer)
		Dim m_translate As New TranslateValues

		If Me._ClsSetting.SelectedMDNr <> ModulConstants.MDData.MDNr OrElse m_InitializationData Is Nothing Then
			m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ModulConstants.TranslationData, ModulConstants.ProsonalizedData, ModulConstants.MDData, ModulConstants.UserData)
		End If

		Dim frm As frmAdvancePayments = CType(ModulConstants.GetModuleCach.GetModuleForm(mandantNumber, userNumber, SP.ModuleCaching.ModuleName.AdvancePaymentMng), frmAdvancePayments)

		Dim bAskForNewRecord As Boolean = Me._ClsSetting.SelectedZGNr.HasValue
		Dim bOpenwithWizard As MsgBoxResult

		If Me._ClsSetting.SelectedZGNr.HasValue AndAlso (frm.LoadAdvancePaymentData(Me._ClsSetting.SelectedZGNr)) Then
			frm.Show()
			frm.BringToFront()

		Else
			If bAskForNewRecord Then
				Dim msg As String = String.Format(m_translate.GetSafeTranslationValue("Der Vorschuss konnte nicht geladen werden.{0}Möglicherweise wurde der Vorschuss breits gelöscht.{0}{0}Möchten Sie einen neuen Vorschuss anlegen?"), vbNewLine)
				bOpenwithWizard = m_UtilityUI.ShowYesNoDialog(msg, m_translate.GetSafeTranslationValue("Neuen Vorschuss anlegen?"), MessageBoxDefaultButton.Button2)
			Else
				bOpenwithWizard = MsgBoxResult.Yes
			End If

			If bOpenwithWizard = MsgBoxResult.Yes Then
				If Not ModulConstants.UserSecValue(350) Then Return
				Dim preselection = New SP.MA.AdvancePaymentMng.PreselectionData With {.MDNr = mandantNumber,
																																							.EmployeeNumber = Me._ClsSetting.SelectedMANr,
																																							.Advisor = ModulConstants.UserData.UserKST,
																																							.LANr = Nothing,
																																							.RPNr = Nothing
																																						 }

				Dim frmNewAdvancePayment As SP.MA.AdvancePaymentMng.UI.frmNewAdvancePayment = New SP.MA.AdvancePaymentMng.UI.frmNewAdvancePayment(m_InitializationData, preselection)
				frmNewAdvancePayment.Show()
				frmNewAdvancePayment.BringToFront()

			End If

		End If

	End Sub

#End Region


#Region "Lohnabrechnung erstellen"

	''' <summary>
	''' startet Lohnbuchhaltung
	''' </summary>
	''' <remarks></remarks>
	Sub OpenPayrollForm(ByVal mandantNumber As Integer, ByVal userNumber As Integer)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		If Not IsUserActionAllowed(ModulConstants.UserData.UserNr, 550, mandantNumber) Then Exit Sub

		Try
			Dim frm As frmPayroll = CType(ModulConstants.GetModuleCach.GetModuleForm(mandantNumber, userNumber, SP.ModuleCaching.ModuleName.PayrollMng), frmPayroll)

			frm.LoadPayrollData()
			frm.Show()
			frm.BringToFront()


		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
		End Try

	End Sub

#End Region

#Region "Monat abschliessen"

	''' <summary>
	''' einen Monat abschliessen
	''' </summary>
	''' <remarks></remarks>
	Sub openCloseMonth()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strTranslationProgName As String = String.Empty

		If Not IsUserActionAllowed(ModulConstants.UserData.UserNr, 669) Then Exit Sub

		Try
			m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ModulConstants.TranslationData,
																																			ModulConstants.ProsonalizedData,
																																			ModulConstants.MDData,
																																			ModulConstants.UserData)

			Dim frmMonthClose As New SPS.SYS.TableSettingMng.frmCloseMonth(m_InitializationData)
			frmMonthClose.LoadData()

			frmMonthClose.Show()
			frmMonthClose.BringToFront()


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.ToString))
			ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.ToString))

		End Try

	End Sub

#End Region


#Region "DTA öffnen..."

	''' <summary>
	''' öffnet die DTA-Verwaltung über Hauptübersicht
	''' </summary>
	''' <remarks></remarks>
	Sub OpenNewDTAForm(ByVal mandantNumber As Integer, ByVal userNumber As Integer)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		If Not IsUserActionAllowed(ModulConstants.UserData.UserNr, 562, mandantNumber) Then Exit Sub

		Try
			m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ModulConstants.TranslationData,
																																ModulConstants.ProsonalizedData,
																																ModulConstants.MDData,
																																ModulConstants.UserData)
			Dim frm = frmDTA.ShowForm(m_InitializationData, showModal:=False)

			frm.Show()
			frm.BringToFront()


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.ToString))
			ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.ToString))

		End Try

	End Sub

#End Region


#Region "Zahlungseingang öffnen..."

	''' <summary>
	''' startet das Threading zum öffnen der Zahlungseingang
	''' </summary>
	''' <remarks></remarks>
	Sub OpenSelectedPayment()

		If Not ModulConstants.UserSecValue(15) Then Return
		If Me._ClsSetting.SelectedMDNr <> ModulConstants.MDData.MDNr OrElse m_InitializationData Is Nothing Then
			m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ModulConstants.TranslationData,
																																						ModulConstants.ProsonalizedData,
																																						ModulConstants.MDData,
																																						ModulConstants.UserData)
		End If

		Dim frmPaymentMng = New SP.KD.InvoiceMng.UI.frmZEedit(m_InitializationData)

		frmPaymentMng.CurrentPaymentNumber = CInt(Me._ClsSetting.SelectedZENr)
		If frmPaymentMng.LoadData() Then
			frmPaymentMng.Show()
			frmPaymentMng.BringToFront()
		End If


	End Sub


#End Region


#Region "RE-Verwaltung öffnen..."

	''' <summary>
	''' startet das Threading zum öffnen der Fakturaverwaltung
	''' </summary>
	''' <remarks></remarks>
	Sub OpenSelectedInvoice(ByVal mandantNumber As Integer, ByVal userNumber As Integer)

		Try

			If Not ModulConstants.UserSecValue(14) Then Return
			Dim frm As frmInvoices = CType(ModulConstants.GetModuleCach.GetModuleForm(mandantNumber, userNumber, SP.ModuleCaching.ModuleName.InvoiceMng), frmInvoices)
			frm.LoadInvoiceData(Me._ClsSetting.SelectedRENr)
			If frm.IsInvoiceDataLoaded Then
				frm.Show()
				frm.BringToFront()
			End If


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			ShowErrDetail(String.Format("{0}", ex.ToString))

		End Try

	End Sub

	Sub OpenNewInvoice(ByVal mandantNumber As Integer, ByVal userNumber As Integer)

		If Not IsUserActionAllowed(userNumber, 401, mandantNumber) Then Exit Sub
		Try
			m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(
					ModulConstants.TranslationData,
					ModulConstants.ProsonalizedData,
					ModulConstants.MDData,
					ModulConstants.UserData)

			Dim preselected = New SP.KD.InvoiceMng.PreselectionData With {
					.MDNr = mandantNumber,
					.BeraterKD = ModulConstants.UserData.UserKST,
					.BeraterMA = ModulConstants.UserData.UserKST,
					.CustomerNumber = Me._ClsSetting.SelectedKDNr
			}

			Dim frmNewInvoice As frmNewInvoice = New frmNewInvoice(m_InitializationData, preselected)
			frmNewInvoice.Show()
			frmNewInvoice.BringToFront()


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			ShowErrDetail(String.Format("{0}", ex.ToString))

		End Try

	End Sub

	''' <summary>
	''' creates new automated invoice 
	''' </summary>
	''' <remarks></remarks>
	Sub OpenNewREAutoForm()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		If Not IsUserActionAllowed(ModulConstants.UserData.UserNr, 402) Then m_Logger.LogWarning("No rights...") : Return

		Try
			m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ModulConstants.TranslationData,
																																						ModulConstants.ProsonalizedData,
																																						ModulConstants.MDData,
																																						ModulConstants.UserData)

			Dim frmNewautomatedInvoice As SP.KD.InvoiceMng.UI.frmNewAutomatedInvoice = New SP.KD.InvoiceMng.UI.frmNewAutomatedInvoice(m_InitializationData, Nothing)
			frmNewautomatedInvoice.LoadData()

			frmNewautomatedInvoice.Show()
			frmNewautomatedInvoice.BringToFront()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.ToString))
			ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Sub OpenNewPaymentForm(ByVal mandantNumber As Integer, ByVal userNumber As Integer)

		If Not IsUserActionAllowed(userNumber, 501, mandantNumber) Then Exit Sub
		Try
			m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(
								ModulConstants.TranslationData,
								ModulConstants.ProsonalizedData,
								ModulConstants.MDData,
								ModulConstants.UserData)

			Dim preselected = New SP.KD.InvoiceMng.PreselectionPaymentData With {
								.MDNr = mandantNumber,
								.InvoiceNumber = _ClsSetting.SelectedRENr
						}

			Dim frmNewPayment As frmNewZahlungsEingang = New frmNewZahlungsEingang(m_InitializationData, preselected)
			frmNewPayment.Show()
			frmNewPayment.BringToFront()


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			ShowErrDetail(String.Format("{0}", ex.ToString))

		End Try

	End Sub

	Sub OpenNewMahnForm()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		If Not IsUserActionAllowed(ModulConstants.UserData.UserNr, 480) Then m_Logger.LogWarning("No rights...") : Exit Sub

		Try
			Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

			Dim preselection As SP.KD.InvoiceMng.PreselectionData = New SP.KD.InvoiceMng.PreselectionData With {.MDNr = ModulConstants.MDData.MDNr}
			Dim frmNewDunning As SP.KD.InvoiceMng.UI.frmNewMahnung = New SP.KD.InvoiceMng.UI.frmNewMahnung(init, preselection)
			frmNewDunning.PreselectionData = preselection
			frmNewDunning.LoadData()

			frmNewDunning.Show()
			frmNewDunning.BringToFront()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.ToString))
			ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	''' <summary>
	''' creates new automated invoice 
	''' </summary>
	''' <remarks></remarks>
	Sub OpenNewZahlungsEingangForm()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strTranslationProgName As String = String.Empty
		Dim _ClsReg As New SPProgUtility.ClsDivReg

		If Not IsUserActionAllowed(ModulConstants.UserData.UserNr, 501) Then m_Logger.LogWarning("No rights...") : Exit Sub

		Try
			m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(
					ModulConstants.TranslationData,
					ModulConstants.ProsonalizedData,
					ModulConstants.MDData,
					ModulConstants.UserData)

			Dim preselected = New SP.KD.InvoiceMng.PreselectionData With {
					.MDNr = Me._ClsSetting.SelectedMDNr,
					.BeraterKD = ModulConstants.UserData.UserKST,
					.BeraterMA = ModulConstants.UserData.UserKST,
					.CustomerNumber = Me._ClsSetting.SelectedKDNr
			}

			Dim frmNewInvoice As SP.KD.InvoiceMng.UI.frmNewInvoice = New SP.KD.InvoiceMng.UI.frmNewInvoice(
					m_InitializationData,
					preselected)

			frmNewInvoice.Show()
			frmNewInvoice.BringToFront()


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.ToString))
			ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.ToString))

		End Try

	End Sub


#End Region



#Region "Employee Contacts"

	Sub OpenSelectedEmployeeContact()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = "Success..."

		Try
			Dim kontakte = New SP.MA.KontaktMng.frmContacts(CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr))
			Dim employeeNumber As Integer
			If Integer.TryParse(Me._ClsSetting.SelectedMANr, employeeNumber) Then

				If (kontakte.ActivateNewContactDataMode(employeeNumber, Nothing, Nothing)) Then
					kontakte.LoadContactData(employeeNumber, Me._ClsSetting.ContactRecordNumber, Nothing)
					kontakte.Show()
				End If

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.ToString))
			ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Sub OpenSelectedEmployeeContactForNewEntry()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = "Success..."

		Try
			Dim kontakte = New SP.MA.KontaktMng.frmContacts(CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr))
			Dim employeeNumber As Integer
			If Integer.TryParse(Me._ClsSetting.SelectedMANr, employeeNumber) Then

				Dim initalData As New SP.MA.KontaktMng.InitalDataForNewContact With {.StartDateTime = DateTime.Now, .ContactTypeBezID = "Telefonisch"}
				If (kontakte.ActivateNewContactDataMode(employeeNumber, initalData, Nothing)) Then
					kontakte.Show()
				End If

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.ToString))
			ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.ToString))

		End Try

	End Sub



#End Region


#Region "Customer Contacts"

	Sub OpenSelectedCustomerContact()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = "Success..."

		Try
			Dim kontakte = New SP.KD.KontaktMng.frmContacts(CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr))
			Dim customerNumber As Integer
			If Integer.TryParse(Me._ClsSetting.SelectedKDNr, customerNumber) Then
				If Me._ClsSetting.SelectedZHDNr = 0 Then Me._ClsSetting.SelectedZHDNr = Nothing
				If (kontakte.ActivateNewContactDataMode(customerNumber, Me._ClsSetting.SelectedZHDNr, Nothing, Nothing)) Then
					kontakte.LoadContactData(customerNumber, Me._ClsSetting.SelectedZHDNr, Me._ClsSetting.ContactRecordNumber, Nothing)
					kontakte.Show()
				End If

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.ToString))
			ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.ToString))

		End Try

	End Sub

#End Region



#Region "Employee interview"

	Sub OpenSelectedEmployeeInterview()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = "Success..."


		Try
			Dim frmInterview As New SP.MA.VorstellungMng.frmJobInterview(CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr))

			frmInterview.LoadJobInterviewData(Me._ClsSetting.SelectedMANr, If(Me._ClsSetting.InternviewRecNr = 0, Nothing, Me._ClsSetting.InternviewRecNr))
			frmInterview.Show()


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.ToString))
			strResult = String.Format("Error: {0}", ex.ToString)

		End Try

	End Sub

	Sub OpenSelectedCustomerContactForNewEntry()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = "Success..."

		Try
			Dim kontakte = New SP.KD.KontaktMng.frmContacts(CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr))
			Dim customerNumber As Integer
			If Integer.TryParse(Me._ClsSetting.SelectedKDNr, customerNumber) Then

				Dim initalData As New SP.KD.KontaktMng.InitalDataForNewContact With {.StartDateTime = DateTime.Now, .ContactTypeBezID = "Telefonisch"}
				If (kontakte.ActivateNewContactDataMode(customerNumber, Me._ClsSetting.SelectedZHDNr, initalData, Nothing)) Then
					kontakte.Show()
				End If

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.ToString))
			ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.ToString))

		End Try

	End Sub


#End Region


	Sub OpenSelectedEmployeeMSalary()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = "Success..."

		Try
			Dim lohnAngaben = New SP.MA.MLohnMng.frmMSalary(CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr))
			Dim employeeNumber As Integer
			If Integer.TryParse(Me._ClsSetting.SelectedMANr, employeeNumber) Then

				lohnAngaben.Show()
				lohnAngaben.LoadData(employeeNumber, False)
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.ToString))
			ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.ToString))

		End Try

	End Sub


#Region "Ausdrücke für Kandidaten..."

	''' <summary>
	''' startet das Threading zum öffnen der Einsatzverwaltung
	''' </summary>
	''' <remarks></remarks>
	Sub PrintMAStammblatt()
		Dim ShowDesign As Boolean = IsUserActionAllowed(ModulConstants.UserData.UserNr, 105, ModulConstants.MDData.MDNr) AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)
		Dim _setting As New SPS.Listing.Print.Utility.ClsLLMASearchPrintSetting With {.DbConnString2Open = _ClsProgSetting.GetConnString, .SelectedMDNr = _ClsSetting.SelectedMDNr,
																																									.liMANr2Print = New List(Of Integer)(New Integer() {Me._ClsSetting.SelectedMANr}),
																																									.ShowAsDesign = ShowDesign,
																																									.JobNr2Print = "1.0"}
		Dim obj As New SPS.Listing.Print.Utility.MAStammblatt.ClsPrintMAStammblatt(_setting)
		Try

			obj.PrintMAStammBlatt()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			ShowErrDetail(String.Format("{0}", ex.ToString))

		End Try

	End Sub

	Sub AddContentToPDF()
		Dim m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ModulConstants.TranslationData,
																																ModulConstants.ProsonalizedData,
																																ModulConstants.MDData,
																																ModulConstants.UserData)

		Dim _Setting As New SPS.Listing.Print.Utility.ClsLLMATemplateSetting With {.TemplateName = "C:\Users\username\Documents\Document.pdf", .EmployeeNumbers2Print = New List(Of Integer)(New Integer() {_ClsSetting.SelectedMANr})}

		Dim obj As New SPS.Listing.Print.Utility.MATemplates.ClsPrintMATemplates(m_InitializationData)

		Try


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			ShowErrDetail(String.Format("{0}", ex.ToString))

		End Try

	End Sub

	''' <summary>
	''' startet den Ausdruck von Lohnabrechnungen eines Kandidaten
	''' </summary>
	''' <remarks></remarks>
	Sub PrintMALO()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = "Success..."
		Dim liLONr As New List(Of Integer)
		Dim mdnr As Integer = ModulConstants.MDData.MDNr

		If _ClsSetting.SelectedMDNr.HasValue Then mdnr = _ClsSetting.SelectedMDNr

		Try
			Dim init = CreateInitialData(mdnr, ModulConstants.UserData.UserNr)

			Dim _settring As New SP.LO.PrintUtility.ClsLOSetting With {.SelectedMANr = New List(Of Integer)(New Integer() {Me._ClsSetting.SelectedMANr}),
																																 .SelectedLONr = New List(Of Integer)(New Integer() {Me._ClsSetting.SelectedLONr}),
																																 .SelectedMonth = New List(Of Integer)(New Integer() {0}),
																																 .SelectedYear = New List(Of Integer)(New Integer() {0}),
																																 .SearchAutomatic = True,
																																 .SelectedMDNr = mdnr,
																																 .LogedUSNr = ModulConstants.UserData.UserNr}
			'Dim obj As New SP.LO.PrintUtility.ClsMain_Net(init, _settring)
			'obj.ShowfrmLO4Print()

			Dim obj As New SP.LO.PrintUtility.frmLOPrint(init)
			obj.LOSetting = _settring

			obj.Show()
			obj.BringToFront()


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.ToString))
			ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	''' <summary>
	''' startet den Ausdruck von ausgewählten Lohnabrechnungen eines Kandidaten
	''' </summary>
	''' <remarks></remarks>
	Sub PrinSelectedtMAPayroll()
		Dim liLONr As New List(Of Integer)
		Dim mdnr As Integer = ModulConstants.MDData.MDNr
		If _ClsSetting.SelectedMDNr.HasValue Then mdnr = _ClsSetting.SelectedMDNr
		Dim bPrintAsDesign = IsUserActionAllowed(ModulConstants.UserData.UserNr, 558, mdnr) AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)
		Dim bSendPrintJob2WOS As Boolean = False
		Dim bSend_And_PrintJob2WOS As Boolean = False

		Dim m_DataAccess As New MainGrid
		Dim employeelanguage = m_DataAccess.LoadEmployeeDataForPrintPayroll(Me._ClsSetting.SelectedMANr.GetValueOrDefault(0))
		Try
			Dim m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ModulConstants.TranslationData,
																																ModulConstants.ProsonalizedData,
																																ModulConstants.MDData,
																																ModulConstants.UserData)
			Dim _PSetting As New SPS.Listing.Print.Utility.ClsLLLOSearchPrintSetting
			_PSetting = New SPS.Listing.Print.Utility.ClsLLLOSearchPrintSetting With {.DbConnString2Open = ModulConstants.MDData.MDDbConn,
																																							 .SQL2Open = String.Empty,
																																							 .JobNr2Print = "9.1",
																																							 .Is4Export = False,
																																							 .SendData2WOS = bSendPrintJob2WOS,
																																							 .SendAndPrintData2WOS = bSend_And_PrintJob2WOS,
																																							 .liLONr2Print = New List(Of Integer)(New Integer() {Me._ClsSetting.SelectedLONr.GetValueOrDefault(0)}),
																																							 .liMANr2Print = New List(Of Integer)(New Integer() {Me._ClsSetting.SelectedMANr.GetValueOrDefault(0)}),
																																							 .liLOSend2WOS = New List(Of Boolean)(New Boolean() {False}),
																																							 .LiMALang = New List(Of String)(New String() {employeelanguage.language}),
																																							 .SelectedLONr2Print = 0,
																																							 .SelectedMANr2Print = 0,
																											.SelectedMDNr = ModulConstants.MDData.MDNr,
																											.LogedUSNr = ModulConstants.UserData.UserNr,
																											.PerosonalizedData = ModulConstants.ProsonalizedData,
																											.TranslationData = ModulConstants.TranslationData
																																							 }
			Dim obj As New LOSearchListing.ClsPrintLOSearchList(m_InitializationData)
			obj.PrintData = _PSetting
			Dim result As PrintResult = obj.PrintLOSearchList(bPrintAsDesign)


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			ShowErrDetail(String.Format("{0}", ex.ToString))

		End Try

	End Sub

	''' <summary>
	''' employee suva hour listing
	''' </summary>
	''' <remarks></remarks>
	Sub PrintSuvaStdListe4SelectedEmployee()
		Dim m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ModulConstants.TranslationData,
																																ModulConstants.ProsonalizedData,
																																ModulConstants.MDData,
																																ModulConstants.UserData)

		Dim frm As New SP.Employee.SuvaSTDSearch.frmSUVAStd(m_InitializationData)
		Dim employeeNumbers As New List(Of Integer?) '(New Integer() {1352655})
		employeeNumbers.Add(_ClsSetting.SelectedMANr)

		Dim preselectionSetting As New SP.Employee.SuvaSTDSearch.PreselectionData With {.MDNr = m_InitializationData.MDData.MDNr, .ListYear = Now.Year, .EmployeeNumbers = employeeNumbers}
		frm.PreselectionData = preselectionSetting
		frm.PreselectData()

		frm.Show()
		frm.BringToFront()

	End Sub

	''' <summary>
	''' startet das Threading zum öffnen der Zwischenverdienstverwaltung
	''' </summary>
	''' <remarks></remarks>
	Sub PrintZV4SelectedEmployee()
		Dim ShowDesign As Boolean = (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)
		'If ModulConstants.UserData.UserNr = 1 Then
		m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ModulConstants.TranslationData,
																																ModulConstants.ProsonalizedData,
																																ModulConstants.MDData,
																																ModulConstants.UserData)

		Dim frmZV = New SPS.MA.Guthaben.frmZV(m_InitializationData)

		Dim preselectionSetting As New SPS.MA.Guthaben.PreselectionZVData With {.MDNr = m_InitializationData.MDData.MDNr, .EmployeeNumber = _ClsSetting.SelectedMANr}
		frmZV.PreselectionData = preselectionSetting

		frmZV.LoadData()
		frmZV.DisplayEmployeeData()

		frmZV.Show()
		frmZV.BringToFront()

	End Sub

	''' <summary>
	''' startet das Threading zum öffnen der Arbeitgeberbescheinigung 
	''' </summary>
	''' <remarks></remarks>
	Sub PrintARG4SelectedEmployee()
		Dim ShowDesign As Boolean = (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)
		m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ModulConstants.TranslationData,
																																ModulConstants.ProsonalizedData,
																																ModulConstants.MDData,
																																ModulConstants.UserData)

		Dim frmARGB = New SPS.MA.Guthaben.frmARGB(m_InitializationData)

		Dim preselectionSetting As New SPS.MA.Guthaben.PreselectionARGBData With {.MDNr = m_InitializationData.MDData.MDNr, .EmployeeNumber = _ClsSetting.SelectedMANr}
		frmARGB.PreselectionData = preselectionSetting

		frmARGB.LoadData()
		frmARGB.DisplayEmployeeData()

		frmARGB.Show()
		frmARGB.BringToFront()

	End Sub

	Sub PrintEmployeeForgottenZVARGB()

		m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ModulConstants.TranslationData, ModulConstants.ProsonalizedData, ModulConstants.MDData, ModulConstants.UserData)

		Dim frmZV = New SP.MA.PayrollMng.UI.frmForgottenZVARGB(m_InitializationData)

		frmZV.LoadData()
		frmZV.Show()
		frmZV.BringToFront()

	End Sub

#End Region


	Sub PrintAssignedReport()
		Dim m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ModulConstants.TranslationData,
																																ModulConstants.ProsonalizedData,
																																ModulConstants.MDData,
																																ModulConstants.UserData)

		Try
			Dim assignedMontn As Integer?
			Dim assignedYear As Integer?

			assignedMontn = _ClsSetting.SelectedMonth
			assignedYear = _ClsSetting.SelectedYear

			Dim frm = New frmRPListSearch(m_InitializationData)

			frm.AssignedMonth = assignedMontn
			frm.AssignedYear = assignedYear
			frm.EmployeeNumber = _ClsSetting.SelectedMANr
			frm.CustomerNumber = _ClsSetting.SelectedKDNr
			frm.EmploymentNumber = _ClsSetting.SelectedESNr
			frm.ReportNumber = _ClsSetting.SelectedRPNr

			frm.LoadData()

			frm.Show()
			frm.BringToFront()

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)
			m_Logger.LogError(ex.ToString)

			Return
		End Try

	End Sub

	Sub PrintRPCompareData()
		Dim t As Thread = New Thread(AddressOf OpenForm4PrintRPCompareDataWithThreading)

		t.IsBackground = True
		t.Name = "OpenForm4PrintRPCompareDataWithThreading"
		t.Start()

	End Sub

	Private Sub OpenForm4PrintRPCompareDataWithThreading()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strTranslationProgName As String = String.Empty
		Dim _ClsReg As New SPProgUtility.ClsDivReg

		Try
			ModulConstants.vb6Object.TranslateProg4Net("SPSUmsatzUtil.ClsMain")


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.ToString))
			ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Sub PrintRPStdData()

		Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)
		If Not IsUserActionAllowed(ModulConstants.UserData.UserNr, 640, ModulConstants.MDData.MDNr) Then Return

		Try
			Dim frm As New SP.RP.EmployeeCustomerHourSearch.frmHourSearch(init)

			Dim preselectionSetting As New SP.RP.EmployeeCustomerHourSearch.PreselectionData With {.MDNr = init.MDData.MDNr}
			frm.PreselectionData = preselectionSetting
			frm.PreselectData()

			frm.Show()
			frm.BringToFront()


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			ShowErrDetail(String.Format("{0}", ex.ToString))

		End Try

	End Sub

	Sub PrintStdData()

		'If My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown Then
		'	PrintKAEListingData()

		'	Return
		'End If

		Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)
		If Not IsUserActionAllowed(ModulConstants.UserData.UserNr, 640, ModulConstants.MDData.MDNr) Then Return

		Try
			Dim frm As New SP.RP.EmployeeCustomerHourSearch.UI.frmKDKSTSearch(init)
			Dim invoiceNumbers As New List(Of Integer?)
			invoiceNumbers.Add(CType(_ClsSetting.SelectedKDNr, Integer))

			Dim preselectionSetting As New SP.RP.EmployeeCustomerHourSearch.UI.PreselectionData With {.MDNr = init.MDData.MDNr,
				.InvoiceNumbers = invoiceNumbers}
			frm.PreselectionData = preselectionSetting
			frm.PreselectData()

			frm.Show()
			frm.BringToFront()


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			ShowErrDetail(String.Format("{0}", ex.ToString))

		End Try

	End Sub

	Sub PrintKAEListingData()

		Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)
		If Not IsUserActionAllowed(ModulConstants.UserData.UserNr, 640, ModulConstants.MDData.MDNr) Then Return

		Try
			Dim frm As New SP.RP.EmployeeCustomerHourSearch.UI.frmKAEListing(init)
			Dim customerNumbers As New List(Of Integer?)
			customerNumbers.Add(CType(_ClsSetting.SelectedKDNr, Integer))

			Dim preselectionSetting As New SP.RP.EmployeeCustomerHourSearch.UI.PreselectionKAEData With {.MDNr = init.MDData.MDNr,
				.CustomerNumbers = customerNumbers}
			frm.PreselectionData = preselectionSetting
			frm.PreselectData()

			frm.Show()
			frm.BringToFront()


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			ShowErrDetail(String.Format("{0}", ex.ToString))

		End Try

	End Sub

	''' <summary>
	''' öffnet die BESR-Verwaltung über Hauptübersicht
	''' </summary>
	''' <remarks></remarks>
	Sub OpenNewBESRForm(ByVal mandantNumber As Integer, ByVal userNumber As Integer)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		If Not IsUserActionAllowed(ModulConstants.UserData.UserNr, 503, mandantNumber) Then Exit Sub

		Try

			m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ModulConstants.TranslationData,
																																ModulConstants.ProsonalizedData,
																																ModulConstants.MDData,
																																ModulConstants.UserData)
			Dim frm = frmBesr.ShowForm(m_InitializationData, showModal:=False)

			frm.Show()
			frm.BringToFront()


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.ToString))
			ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	''' <summary>
	''' startet das Threading zum öffnen der Einsatzverwaltung
	''' </summary>
	''' <remarks></remarks>
	Sub PrintKDStammblatt()
		Dim ShowDesign As Boolean = IsUserActionAllowed(ModulConstants.UserData.UserNr, 205, ModulConstants.MDData.MDNr) AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)
		Dim _setting As New SPS.Listing.Print.Utility.ClsLLKDSearchPrintSetting With {.DbConnString2Open = _ClsProgSetting.GetConnString,
																																									.liKDNr2Print = New List(Of Integer)(New Integer() {Me._ClsSetting.SelectedKDNr}),
																																									.ShowAsDesign = ShowDesign,
																																									.JobNr2Print = "2.0"}
		Dim obj As New SPS.Listing.Print.Utility.KDStammblatt.ClsPrintKDStammblatt(_setting)
		Try
			obj.PrintKDStammBlatt()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			ShowErrDetail(String.Format("{0}", ex.ToString))

		End Try

	End Sub

	Sub PrintProposeStammblatt()
		Dim ShowDesign As Boolean = IsUserActionAllowed(ModulConstants.UserData.UserNr, 805, ModulConstants.MDData.MDNr) AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)

		Dim sSql As String = "[Get Propose Data 4 Print]"
		Dim _Setting As New SPS.Listing.Print.Utility.ClsLLProposeSearchPrintSetting With
			{.DbConnString2Open = ModulConstants.MDData.MDDbConn,
			 .SQL2Open = sSql,
			 .JobNr2Print = "18.0",
			 .ProposeNr2Print = Me._ClsSetting.SelectedProposeNr,
			 .SelectedMDNr = ModulConstants.MDData.MDNr, .LogedUSNr = ModulConstants.UserData.UserNr}

		Dim obj As New SPS.Listing.Print.Utility.ProposeSearchListing.ClsPrintProposeSearchList(_Setting)
		obj.PrintProposeTpl_1(ShowDesign, String.Empty, _Setting.ProposeNr2Print, False, False)

	End Sub

	Sub PrintProposeHonorarblatt()
		Dim ShowDesign As Boolean = IsUserActionAllowed(ModulConstants.UserData.UserNr, 805, ModulConstants.MDData.MDNr) AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)
		Dim sSql As String = "[Get Propose Data 4 Print]"
		Dim _Setting As New SPS.Listing.Print.Utility.ClsLLProposeSearchPrintSetting With {.DbConnString2Open = ModulConstants.MDData.MDDbConn,
																																	 .SQL2Open = sSql,
																																	 .JobNr2Print = "18.0.1",
																																	 .ProposeNr2Print = Me._ClsSetting.SelectedProposeNr,
																																	 .SelectedMDNr = ModulConstants.MDData.MDNr, .LogedUSNr = ModulConstants.UserData.UserNr}

		Dim obj As New SPS.Listing.Print.Utility.ProposeSearchListing.ClsPrintProposeSearchList(_Setting)
		obj.PrintProposeTpl_1(ShowDesign, String.Empty, _Setting.ProposeNr2Print, False, False)

	End Sub

	Sub Printofferblatt()
		Dim ShowDesign As Boolean = IsUserActionAllowed(ModulConstants.UserData.UserNr, 805, ModulConstants.MDData.MDNr) AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)

		Try
			Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)
			Dim success As Boolean = True

			If _ClsSetting.SelectedKDNr.GetValueOrDefault(0) = 0 Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Keine Kundendaten wurden gefunden!"))

				Return
			End If

			Dim _setting As New SPS.Listing.Print.Utility.ClsLLOfferSearchPrintSetting With {.m_initData = init, .offerNumber = _ClsSetting.SelectedOfferNr,
				.JobNr2Print = "15.0", .ShowAsExport = False, .customerNumber = _ClsSetting.SelectedKDNr.GetValueOrDefault(0), .cresponsibleNumber = _ClsSetting.SelectedZHDNr.GetValueOrDefault(0),
				.ShowAsDesgin = False}
			Dim printTemplate As New SPS.Listing.Print.Utility.OfferSearchListing.ClsPrintOfferSearchList(_setting)
			Dim strOfferblattFileName As String = printTemplate.PrintOfferTemplate()


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			ShowErrDetail(String.Format("{0}", ex.ToString))

		End Try

	End Sub

	Sub SendOfferWithMail()
		Dim ShowDesign As Boolean = IsUserActionAllowed(ModulConstants.UserData.UserNr, 805, ModulConstants.MDData.MDNr) AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)
		Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)
		Dim o2Open As New SPSSendMail.ClsMain_Net(init)

		Try
			o2Open.SendOfferWithMail(_ClsSetting.SelectedOfferNr, 0, _ClsSetting.SelectedKDNr, _ClsSetting.SelectedZHDNr, False, True, True, True, "KDeMail", System.Guid.NewGuid.ToString, "", "", "")



		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			ShowErrDetail(String.Format("{0}", ex.ToString))

		End Try

	End Sub


	''' <summary>
	''' druckt den ausgewählten Einsatzvertrag aus
	''' </summary>
	''' <param name="sWOS">0 = NUR Drucken | 1 = Drucken und Senden | 2 = NUR Senden</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function PrintESVertrag_(ByVal _bAsVerleih As Boolean, ByVal _bPrintWithVerleih As Boolean, ByVal sWOS As Short) As Boolean
		Dim result As Boolean = True
		Dim allowedESVerDesign = IsUserActionAllowed(ModulConstants.UserData.UserNr, 254, ModulConstants.MDData.MDNr)
		Dim allowedVerleihVerDesign = IsUserActionAllowed(ModulConstants.UserData.UserNr, 256, ModulConstants.MDData.MDNr)

		Dim liESNr As List(Of Integer) = New List(Of Integer)(New Integer() {_ClsSetting.SelectedESNr})
		Dim liMANr As New List(Of Integer)
		Dim ShowDesign As Boolean = False
		If Not _bPrintWithVerleih Then
			If _bAsVerleih Then
				ShowDesign = allowedVerleihVerDesign AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)
			Else
				ShowDesign = allowedESVerDesign AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)
			End If
		End If

		Try
			Dim _settring As New SP.ES.PrintUtility.ClsESSetting With {.SelectedESNr = liESNr,
																																 .SearchAutomatic = False,
																																 .ShowDesign = ShowDesign,
																																 .MDData = ModulConstants.MDData,
																																 .UserData = ModulConstants.UserData,
																																 .PerosonalizedData = ModulConstants.ProsonalizedData,
																																 .TranslationData = ModulConstants.TranslationData}
			Dim obj As New SP.ES.PrintUtility.ClsMain_Net(_settring)
			obj.PrintSelectedES(_bAsVerleih, _bPrintWithVerleih, sWOS)
			m_Logger.LogDebug(String.Format("ESNr: {0} | _bAsVerleih:({1}) | Wos:({2})", liESNr(0), _bAsVerleih, sWOS))


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			result = False
		End Try


		Return result

	End Function



	Sub PrintNotFinshedReportList()
		Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		Try
			Dim obj As New SP.RP.NotDoneList.frmRPNotDoneSearch(init)

			obj.Show()
			obj.BringToFront()


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			ShowErrDetail(String.Format("{0}", ex.ToString))

		End Try

	End Sub

	Sub PrintRPDataIntoPDF()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = "Success..."

		Try
			Dim _settring As New SP.RPContent.PrintUtility.ClsRPCSetting With {.SelectedMonth = New List(Of Short)(New Short() {CShort(Now.Month)}),
																																	.SelectedYear = New List(Of Integer)(New Integer() {Now.Year}),
																																	.SelectedRPNr = New List(Of Integer)(New Integer() {0}),
																																	.SelectedPVLBez = String.Empty,
																																	.SelectedMDNr = ModulConstants.MDData.MDNr,
																																																			.SelectedMDYear = Now.Year,
																																																			.SelectedMDGuid = String.Empty,
																																																			.LogedUSNr = ModulConstants.UserData.UserNr}
			Dim obj As New SP.RPContent.PrintUtility.ClsMain_Net(_settring)
			obj.ShowfrmRPContent4Print()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
			ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.ToString))

		End Try

	End Sub


	Sub PrintSelectedOPRecord()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim ShowDesign As Boolean = (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)

		If Not IsUserActionAllowed(ModulConstants.UserData.UserNr, 406) Then Exit Sub
		ShowDesign = ShowDesign AndAlso IsUserActionAllowed(ModulConstants.UserData.UserNr, 407, ModulConstants.MDData.MDNr)

		Try
			Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)
			Dim invoiceNumbers As New List(Of Integer)

			invoiceNumbers.Add(CType(_ClsSetting.SelectedRENr, Integer))
			Dim _setting As New InvoicePrintData With {.InvoiceNumbers = invoiceNumbers,
																									.PrintInvoiceAsCopy = False,
																									.ShowAsDesign = ShowDesign,
																									.PrintOpenAmount = _ClsSetting.PrintOpenAmount}
			Dim printUtil = New ClsPrintInvoice(init)
			printUtil.PrintData = _setting
			Dim strResult = printUtil.PrintInvoice()

			printUtil.Dispose()


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.ToString))
			ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Sub PrintMoreOPRecords()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		If Not IsUserActionAllowed(ModulConstants.UserData.UserNr, 406) Then Exit Sub

		Try

			Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

			Dim frm = New SP.Invoice.PrintUtility.frmInvoicePrint(init)
			Dim preselectionSetting As New SP.Invoice.PrintUtility.PreselectionData With {.MDNr = ModulConstants.MDData.MDNr}
			frm.PreselectionData = preselectionSetting
			frm.PreselectData()

			frm.Show()
			frm.BringToFront()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.ToString))
			ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Sub PrintMahnRecords()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		If Not IsUserActionAllowed(ModulConstants.UserData.UserNr, 482) Then Exit Sub
		Try
			Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

			Dim frm = New SP.Invoice.PrintUtility.frmDunningPrint(init)
			Dim preselectionSetting As New SP.Invoice.PrintUtility.PreselectionDunningData With {.MDNr = init.MDData.MDNr, .DunningLevel = 0}
			frm.PreselectionData = preselectionSetting
			frm.LoadData()

			frm.Show()
			frm.BringToFront()


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.ToString))
			ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Private Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

		Dim m_md As New SPProgUtility.Mandanten.Mandant
		Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
		Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
		Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

		Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
		Dim translate = clsTransalation.GetTranslationInObject

		Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

	End Function




#Region "Extras"

	Sub ShowExtrasUpdateprotokoll()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name


		Try
			Process.Start("http://downloads.domain.com/sps_downloads/PDF/anleitungen/Updateprotokoll/update_details.pdf")


		Catch ex As Exception
			ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Sub ShowExtrasFernwartung()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name


		Try
			Process.Start("http://downloads.domain.com/sps_downloads/exe/teamviewer/TeamViewerQS_de.exe")


		Catch ex As Exception
			ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Sub ShowExtrasBenutzerhandbuch()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name


		Try
			Process.Start("http://downloads.domain.com/sps_downloads/PDF/anleitungen/Updateprotokoll/Handbuch_Gesamt.pdf")


		Catch ex As Exception
			ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Sub ShowUpdateNews()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name


		Try
			Dim url As String = "http://downloads.domain.com/sps_downloads/PDF/anleitungen/Updateprotokoll/update_details.pdf"

			Process.Start(url)


		Catch ex As Exception
			ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Sub ShowExtrasGAVNews()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name


		Try
			Process.Start("http://downloads.domain.com/sps_downloads/PDF/infos/gav_news.pdf")


		Catch ex As Exception
			ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Sub ShowExtrasDeleteProtokoll()
		Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		Try
			Dim frmProtocol As New SPS.SYS.TableSettingMng.frmDeletedrecs(init)
			frmProtocol.Show()
			frmProtocol.BringToFront()


		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

	End Sub


	Sub OpenAutoRPScan()
		Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)


		Try
			Dim frm As New SP.RP.ShowScannedDoc.frmRPDocScan(init)
			frm.Show()
			frm.BringToFront()


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try


	End Sub

#End Region


#Region "Verwaltung"

	Sub ShowVMandanten()
		Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		Try
			Dim frm As New SPS.SYS.TableSettingMng.frmMandant(init)
			frm.Show()
			frm.BringToFront()

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

	End Sub

	Sub ShowVLohnartenstamm()
		Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Dim frm As New SPS.SYS.TableSettingMng.frmLAStamm(init)
			frm.LoadLAStammData()
			frm.Show()
			frm.BringToFront()


		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

	End Sub

	Sub ShowVEinstellung()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)
			Dim frm As New SPSSetting.frmTemplateDetail(init)
			frm.Show()
			frm.BringToFront()


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Sub ShowVTabellen()

		Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		Try
			Dim frm As New SPS.SYS.TableSettingMng.frmTables(init)
			frm.Show()
			frm.BringToFront()

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

	End Sub

	Sub ShowVBenutzer()
		Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		Try
			Dim frm As New SPS.SYS.TableSettingMng.frmUsers(init)
			frm.LoadUserData()
			frm.Show()
			frm.BringToFront()

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

	End Sub

	Sub ShowVDokument()
		Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		Try
			Dim frmDocUtility As New SPS.SYS.DocUtility.frmDocUtility(init)
			frmDocUtility.LoadData()
			frmDocUtility.Show()
			frmDocUtility.BringToFront()

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

	End Sub

	Sub ShowCockpit()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Process.Start("SPMy.Cockpit.exe")

		Catch ex As Exception
			ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.ToString))

		End Try

	End Sub

#End Region


#Region "TAPI"

	Sub TelefonCallToCustomer(ByVal strNumber As String)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim iTest As Integer = 0

		Try
			Dim m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ModulConstants.TranslationData,
																																										ModulConstants.ProsonalizedData,
																																										ModulConstants.MDData,
																																										ModulConstants.UserData
																																										)
			Dim oMyProg As New SPSTapi.UI.frmCaller(m_InitializationData)

			oMyProg.LoadData(strNumber)
			oMyProg.Show()
			oMyProg.BringToFront()

		Catch e As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
			ShowErrDetail(e.Message)

		End Try

	End Sub

	Sub SendEMailToCustomer(ByVal strEMail As String)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim iTest As Integer = 0

		If Not String.IsNullOrWhiteSpace(strEMail) Then
			m_UtilityUI.OpenEmail(strEMail)
			Dim result As Boolean

			Dim obj As New SPSSendMail.ContactLogger(New SPSSendMail.InitializeClass With {.MDData = ModulConstants.MDData,
																																						 .ProsonalizedData = ModulConstants.ProsonalizedData,
																																						 .TranslationData = ModulConstants.TranslationData,
																																						 .UserData = ModulConstants.UserData})

			result = obj.NewResponsiblePersonContact(_ClsSetting.SelectedKDNr, strEMail,
														 String.Empty, Nothing,
														 Now, ModulConstants.UserData.UserFullName, Nothing,
														 Now, ModulConstants.UserData.UserFullName, Nothing, Nothing, "Einzelmail", 1, False, True,
														 False)

		End If

	End Sub

	Sub SendEMailTocResponsible(ByVal strEMail As String)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim iTest As Integer = 0

		If Not String.IsNullOrWhiteSpace(strEMail) Then
			m_UtilityUI.OpenEmail(strEMail)
			Dim result As Boolean

			Dim obj As New SPSSendMail.ContactLogger(New SPSSendMail.InitializeClass With {.MDData = ModulConstants.MDData,
																																						 .ProsonalizedData = ModulConstants.ProsonalizedData,
																																						 .TranslationData = ModulConstants.TranslationData,
																																						 .UserData = ModulConstants.UserData})

			result = obj.NewResponsiblePersonContact(_ClsSetting.SelectedKDNr, strEMail,
														 String.Empty, _ClsSetting.SelectedZHDNr,
														 Now, ModulConstants.UserData.UserFullName, Nothing,
														 Now, ModulConstants.UserData.UserFullName, Nothing, Nothing, "Einzelmail", 1, False, True,
														 False)

		End If

	End Sub

	Sub TelefonCallToEmployee(ByVal strNumber As String)
		Dim iTest As Integer = 0

		Try
			Dim m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ModulConstants.TranslationData,
																																										ModulConstants.ProsonalizedData,
																																										ModulConstants.MDData,
																																										ModulConstants.UserData
																																										)

			Dim oMyProg As New SPSTapi.UI.frmCaller(m_InitializationData)
			oMyProg.LoadData(strNumber)
			oMyProg.Show()
			oMyProg.BringToFront()


		Catch e As Exception
			m_Logger.LogError(e.ToString)
			ShowErrDetail(e.ToString)

		End Try

	End Sub

	Sub SendEMailToEmployee(ByVal strEMail As String)
		If Not String.IsNullOrWhiteSpace(strEMail) Then
			m_UtilityUI.OpenEmail(strEMail)
			Dim result As Boolean

			Dim obj As New SPSSendMail.ContactLogger(New SPSSendMail.InitializeClass With {.MDData = ModulConstants.MDData,
																																										 .ProsonalizedData = ModulConstants.ProsonalizedData,
																																										 .TranslationData = ModulConstants.TranslationData,
																																										 .UserData = ModulConstants.UserData})

			result = obj.NewEmployeeContact(_ClsSetting.SelectedMANr, strEMail,
														 String.Empty,
														 "Einzelmail", 1, Now.Date, False, True,
														 Now, ModulConstants.UserData.UserFullName)
		End If

	End Sub


#End Region


#Region "Liste für GAV..."

	' InkassoPool
	Sub OpenGAVIPoolSearchForm()

		Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)
		Dim frm As New SPLOIPSearch.frmGAVInkassoSearch(init)

		frm.Show()
		frm.BringToFront()


		'Dim o2Open As New SPLOIPSearch.ClsMain_Net

		'Try
		'	o2Open.ShowfrmLOIPSearch()

		'Catch ex As Exception
		'	m_UtilityUI.ShowErrorDialog(ex.ToString)

		'End Try

	End Sub

	'' GEFAK-List für Baselland (BL)
	'Sub OpenGAVGEFakSearchForm()
	'	' TODO: is to old module
	'	Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)
	'	Dim frm As New SPGAVBLSearch.frmGAVGEFAKSearch(init)

	'	frm.Show()
	'	frm.BringToFront()

	'End Sub

	'' Diverse GAV-Liste
	'Sub OpenGAVDivSearchForm()
	'	Dim o2Open As New SPGAVDivSearch.ClsMain_Net

	'	Try
	'		o2Open.ShowfrmGAVDivSearch()

	'	Catch ex As Exception
	'		m_UtilityUI.ShowErrorDialog(ex.ToString)

	'	End Try

	'End Sub

	' Schreinergewerbe
	Sub OpenGAVSchSearchForm()

		' TODO: is to old module
		'Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)
		'Dim frm As New SPGAVSchreinerSearch.frmGAVSearch(init)

		'frm.Show()
		'frm.BringToFront()

	End Sub

	' PVL-Lohndaten
	Sub OpenPVLLohnSearchForm()
		Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		Try
			Dim o2Open As New SPGAVPVLSearch.ClsMain_Net(init)
			o2Open.ShowfrmGAVDivSearch()

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

	End Sub

	' FAR-Lohnbescheinigung
	Sub OpenFARLohnSearchForm()
		Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		Try
			Dim o2Open As New SPFARListeSearch.ClsMain_Net(init)
			o2Open.ShowfrmFARListeSearch()

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

	End Sub

#End Region


#Region "Monatliche Lohnlisten..."

	' Monatliche BVG-Liste...
	Sub OpenMBVGSearchForm()
		Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		Try
			Dim o2Open As New SPBVGListeSearch.ClsMain_Net(init)
			o2Open.ShowfrmBVGListeSearch()

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

	End Sub

	' Monatliche Quellensteuer...
	Sub OpenMQSTSearchForm()
		Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		Try
			Dim obj As New SPQSTListeSearch.frmQSTListeSearch(init)

			obj.LoadData()

			obj.Show()
			obj.BringToFront()


		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

	End Sub

	' Monatliche Fibu...
	Sub OpenMFibuSearchForm()
		Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		Try
			Dim o2Open As New SPFibuSearch.ClsMain_Net(init)
			o2Open.ShowSPFibuSearch()

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

	End Sub

	' Monatliche Lohnbelege für Fibu...
	Sub OpenMLohnbelegFibuSearchForm()
		Dim o2Open As Object

		Try
			o2Open = CreateObject("LOFibuList.ClsMain")
			o2Open.OpenfrmLists("12.8")

			o2Open = Nothing

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

	End Sub

	' Monatliche FAK-Liste...
	Sub OpenMFakSearchForm()
		Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		Try
			Dim o2Open As New SPMFakListSearch.ClsMain_Net(init)
			o2Open.ShowfrmMFakListSearch()

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

	End Sub

	' Monatliche B-Journal-Liste...
	Sub OpenMBJournalSearchForm()
		Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		Try
			Dim o2Open As New SPBruttolohnjournal.ClsMain_Net(init)
			o2Open.ShowfrmBruttolohnjournal()

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

	End Sub

	' Monatliche Liste (Lohnartenrekapitulation Arbeitnehmer + Liste der Mitarbeiterlohnart)...
	Sub OpenMMARekapLOANSearchForm(ByVal sListNr As Short)
		Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		Try
			Dim o2Open As New SPLOANSearch.ClsMain_Net(init)
			o2Open.ShowfrmLOAN(sListNr)

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

	End Sub

	' Monatliche Liste (Arbeitsstundenliste)...
	Sub OpenMLOStdSearchForm()
		Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		Try
			Dim o2Open As New SPLOStdListSearch.ClsMain_Net(init)
			o2Open.ShowfrmLOStdList()

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

	End Sub

	' Monatliche Liste (Fremdleistungen)...
	Sub OpenMFremdleistungSearchForm()
		Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		Try
			Dim o2Open As New SPFremdListSearch.ClsMain_Net(init)
			o2Open.ShowfrmFremdList()

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

	End Sub

	' Monatliche Liste (Arbeitgeberlohnarten + Rekapitulation)...
	Sub OpenMFRekapLOAGSearchForm(ByVal sListNr As Short)
		Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		Try
			Dim o2Open As New SPLOAGSearch.ClsMain_Net(init)
			o2Open.ShowfrmLOAG(sListNr)

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

	End Sub



#End Region



#Region "Jährliche Lohnlisten..."


	Sub OpenYLohnkontiSearchForm()
		Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		Try
			Dim o2Open As New SPLOLKontiSearch.frmLOLKontiSearch(init)
			o2Open.LoadData()

			o2Open.Show()
			o2Open.BringToFront()

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

	End Sub

	Sub OpenYLohnRekapSearchForm()
		Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		Try
			Dim o2Open As New SPLOLRJSearch.ClsMain_Net(init)
			o2Open.ShowfrmYLohnRekap()

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

	End Sub

	Sub OpenYKigaSearchForm()
		Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		Try
			Dim o2Open As New SPLOKigaYSearch.ClsMain_Net(init)
			o2Open.ShowfrmLOKigaY()

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

	End Sub

	Sub OpenYFGrenzGSearchForm()
		Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		Try
			Dim o2Open As New SPLOFranzSearch.ClsMain_Net(init)
			o2Open.ShowfrmLOFranz()

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

	End Sub

	Sub OpenYGuthabenSearchForm()
		Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		Try
			Dim o2Open As New SPLOGUSearch.ClsMain_Net(init)
			o2Open.ShowfrmLOGU()

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

	End Sub

	Sub OpenYFAKSearchForm()
		Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		Try
			Dim o2Open As New SPYFakListSearch.ClsMain_Net(init)
			o2Open.ShowfrmYFakListSearch()

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

	End Sub

	Sub OpenYNLASearchForm()
		Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		Try
			Dim o2Open As New SPLoNLASearch.ClsMain_Net(init)
			o2Open.ShowfrmLoNLA()

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

	End Sub

	Sub OpenUVGSearchForm()
		Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		Try
			Dim o2Open As New SPUVGListeSearch.ClsMain_Net(init)
			o2Open.ShowfrmUVGListeSearch()

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

	End Sub

	Sub OpenYAHVSearchForm()
		Dim init = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		Try
			Dim o2Open As New SPYAHVListSearch.ClsMain_Net(init)
			o2Open.ShowfrmYAHVListSearch()

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

	End Sub


#End Region


	Sub OpenTODOList()
		Dim m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ModulConstants.TranslationData,
																																										ModulConstants.ProsonalizedData,
																																										ModulConstants.MDData,
																																										ModulConstants.UserData
																																										)


		Dim frmTodo As New SP.TodoMng.frmTodo(m_InitializationData)
		frmTodo.ToDoIDNumber = Me._ClsSetting.SelectedTODONr
		frmTodo.LoadData()

		frmTodo.Show()
		frmTodo.BringToFront()

	End Sub

	Sub ShowTarifCalculator()
		Dim o2Open As New SPTarifCalculator.ClsMain_Net

		Try
			o2Open.ShowfrmTarifCalculator()

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try


	End Sub

	''' <summary>
	''' lists external documents from database
	''' </summary>
	''' <param name="sender"></param>
	''' <remarks></remarks>
	Sub ShowContextMenu4ExternDocuments(sender As DevExpress.XtraEditors.DropDownButton)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
		Dim strQuery As String = "[Get ContexMenuItems 4 Print In MainView]"
		Dim popupMenu1 As New DevExpress.XtraBars.PopupMenu
		Dim mgr As New DevExpress.XtraBars.BarManager

		popupMenu1.Manager = mgr
		sender.DropDownControl = popupMenu1

		Try
			Conn.Open()
			Dim cmd As System.Data.SqlClient.SqlCommand
			cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
			Dim param As System.Data.SqlClient.SqlParameter
			cmd.CommandType = CommandType.StoredProcedure
			param = cmd.Parameters.AddWithValue("@ModulName", "ExternDoc")

			Dim rFrec As SqlDataReader = cmd.ExecuteReader
			sender.Visible = False

			Try
				While rFrec.Read
					Dim strmnuBez As String = String.Empty
					Dim strmnuName As String = String.Empty
					Dim strmnuTooltip As String = String.Empty
					Dim strAccessibleName As String = String.Empty

					strmnuBez = String.Format("{0}", rFrec("TranslatedValue"))
					strmnuName = String.Format("{0}", rFrec("mnuName"))
					strAccessibleName = String.Format("{0}", rFrec("PrintJobNr"))

					Dim itm As New DevExpress.XtraBars.BarButtonItem
					itm = New DevExpress.XtraBars.BarButtonItem

					itm.Name = strmnuName
					itm.Caption = strmnuBez
					itm.AccessibleName = strAccessibleName


					If strmnuBez.StartsWith("_") Then
						itm.Caption = strmnuBez.Remove(0, 1)
						popupMenu1.AddItem(itm).BeginGroup = True
					Else
						popupMenu1.AddItem(itm)
					End If
					AddHandler itm.ItemClick, AddressOf GetMnuItem4ExternalDocuments

					sender.Visible = True
				End While

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Menüs aufbauen: {1}", strMethodeName, ex.ToString))
				ShowErrDetail(String.Format("{0}.Menüs aufbauen: {1}", strMethodeName, ex.ToString))

			End Try


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

	End Sub

	''' <summary>
	''' opens an external documents which is in some path
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Sub GetMnuItem4ExternalDocuments(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
		Dim strMnuName As String = e.Item.Name.ToLower
		Dim AccessibleName As String = e.Item.AccessibleName.ToLower

		Select Case strMnuName
			Case "Inhaltsverzeichnis".ToLower
				Process.Start(AccessibleName)

			Case "OrgHandBook".ToLower
				Process.Start(AccessibleName)


			Case Else
				Exit Sub

		End Select

	End Sub

	' String nach Base64 codieren
	Function ToBase64(ByVal sText As String) As String
		' String zunächst in ein Byte-Array umwandeln
		Dim nBytes() As Byte = System.Text.Encoding.Default.GetBytes(sText)

		' jetzt das Byte-Array nach Base64 codieren
		Return System.Convert.ToBase64String(nBytes)
	End Function

	' Base64-String in lesbaren String umwandeln
	Function FromBase64(ByVal sText As String) As String
		' Base64-String zunächst in ByteArray konvertieren
		Dim nBytes() As Byte = System.Convert.FromBase64String(sText)

		' ByteArray in String umwandeln
		Return System.Text.Encoding.Default.GetString(nBytes)
	End Function





End Class

