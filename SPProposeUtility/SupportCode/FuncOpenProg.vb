
Option Strict Off

Imports System.Guid
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Reflection
Imports System.Threading

Imports SP.Infrastructure.Logging
Imports SP.KD.CPersonMng.UI
Imports SP.KD.KontaktMng

Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SP
Imports SP.MA.VorstellungMng.frmJobInterview

Imports SPProposeUtility.ClsDataDetail


Module FuncOpenProg

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private _ClsReg As New SPProgUtility.ClsDivReg
	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	Private strConnString As String = _ClsProgSetting.GetConnString()

	Private m_xml As New ClsXML
	Private Property PrintJobNr As String
	Private Property SQL4Print As String
	Private Property bPrintAsDesign As Boolean

	'Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass


#Region "Öffnen von Kontaktdatenbanken..."

	Sub RunOpenMAKontaktForm(ByVal iNr As Integer?, ByVal iMANr As Integer?, ByVal iKDNr As Integer?,
													 ByVal iVakNr As Integer?, ByVal lProposeNr As Integer?)

		'm_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ClsDataDetail.TranslationData,
		'                                                                            ClsDataDetail.ProsonalizedData, ClsDataDetail.MDData,
		'                                                                            ClsDataDetail.UserData)

		Dim kontakte = New SP.MA.KontaktMng.frmContacts(m_InitialData)
		Dim employeeNumber As Integer
		If Integer.TryParse(iMANr, employeeNumber) Then
			Dim initalData As New SP.MA.KontaktMng.InitalDataForNewContact
			Dim bKontaktOK As Boolean

			If Not iNr.HasValue Then
				initalData = New SP.MA.KontaktMng.InitalDataForNewContact With {.StartDateTime = DateTime.Now, .ContactTypeBezID = "Telefonisch",
																																				.customerProposeNumber = lProposeNr,
																																				.customerNumber = iKDNr,
																																				.customerVacancyNumber = iVakNr,
																																				.customerESNumber = Nothing}
				bKontaktOK = kontakte.ActivateNewContactDataMode(employeeNumber, If(iNr > 0, Nothing, initalData), Nothing)

			Else

				bKontaktOK = kontakte.LoadContactData(employeeNumber, iNr, Nothing)

			End If

			If bKontaktOK Then kontakte.Show()

		End If

	End Sub

	Sub RunOpenKDKontaktForm(ByVal iNr As Integer?, ByVal iKDNr As Integer?, ByVal iZHDNr As Integer?,
													 ByVal iMANr As Integer?, ByVal iVakNr As Integer?, _
													 ByVal iProposeNr As Integer?)

		'm_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(m_InitialData)
		'ClsDataDetail.TranslationData,
		'ClsDataDetail.ProsonalizedData, ClsDataDetail.MDData,
		'ClsDataDetail.UserData)

		Dim kontakte = New SP.KD.KontaktMng.frmContacts(m_InitialData) 'm_InitializationData)
		Dim customerNumber As Integer
		If Integer.TryParse(iKDNr, customerNumber) Then
			Dim initalData As New SP.KD.KontaktMng.InitalDataForNewContact
			Dim bKontaktOK As Boolean

			If Not iNr.HasValue Then
				initalData = New SP.KD.KontaktMng.InitalDataForNewContact With {.StartDateTime = DateTime.Now, .ContactTypeBezID = "Telefonisch",
																																				.customerESNumber = Nothing, .customerProposeNumber = iProposeNr,
																																				.customerVacancyNumber = iVakNr, .EmployeeCopyList = New List(Of Integer)(New Integer() {iMANr})}
				bKontaktOK = kontakte.ActivateNewContactDataMode(customerNumber, iZHDNr, initalData, Nothing)

			Else

				bKontaktOK = kontakte.LoadContactData(customerNumber, iZHDNr, iNr, Nothing)

			End If

			If bKontaktOK Then kontakte.Show()

		End If


	End Sub

	Sub RunOpenMAVorstellungForm(ByVal iNr As Integer?, ByVal iMANr As Integer?, _
															 ByVal iKDNr As Integer?, ByVal izhdNr As Integer?, ByVal iVakNr As Integer?, _
															 ByVal iPNr As Integer?,
															 ByVal strBez As String)

		Dim strTranslationProgName As String = String.Empty

		'Dim m_InitializationData As SP.Infrastructure.Initialization.InitializeClass
		'm_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ClsDataDetail.TranslationData,
		'                                                                            ClsDataDetail.ProsonalizedData,
		'                                                                            ClsDataDetail.MDData, ClsDataDetail.UserData
		'                                                                            )
		Dim frmInterview As New MA.VorstellungMng.frmJobInterview(m_InitialData)

		frmInterview.InitDataForNewInteview = New InitalDataForJobInterview With {.InterviewAs = strBez,
																																							.InteviewDate = New DateTime(Now.Year, Now.Month, Now.Day, 8, 0, 0, 0),
																																							.CustomerNumber = iKDNr,
																																							.ResponsiblePersonNumber = izhdNr,
																																							.IDState = Nothing,
																																							.Result = String.Empty,
																																							.VakNr = iVakNr,
																																							.ProposeNr = iPNr
																																						 }

		frmInterview.Show()
		frmInterview.LoadJobInterviewData(iMANr, iNr)




		'strTranslationProgName = _ClsProgSetting.GetPersonalFolder() & "SPTranslationProg" & _ClsProgSetting.GetLogedUSNr()
		'_ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSMJobTerminUtil.ClsMain")
		'_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", iNr.ToString)

		'Try
		'  _ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "Nr", iNr.ToString)

		'  oMyProg = CreateObject("SPSModulsView.ClsMain")
		'  oMyProg.TranslateProg4Net("SPSMJobTerminUtil.ClsMain", iMANr.ToString, iNr.ToString, iKDNr.ToString, _
		'                            iVakNr.ToString, iPNr.ToString)

		'Catch e As Exception
		'  MsgBox(e.Message, MsgBoxStyle.Critical, "RunOpenMAVorstellungForm")

		'End Try

	End Sub

#End Region


#Region "Öffnen von Masken..."

	Sub StartMailing()
		m_Logger.LogDebug("StartMailing...")

		m_Logger.LogDebug(String.Format("ClsDataDetail.GetProposalNr: {0} | " &
								 "ClsDataDetail.LLExportFileName: {1} | " &
								 "ClsDataDetail.GetProposalMANr: {2} | " &
								 "ClsDataDetail.GetProposalKDNr: {3} | " &
								 "ClsDataDetail.GetProposalZHDNr: {4} | " &
								 "ClsDataDetail.GetProposalVakNr: {5} | " &
								 "ClsDataDetail.UserData.UserNr: {6} | " &
								 "ClsDataDetail.MDData.MDGuid: {7} | " &
								 "ClsDataDetail.MDData.MDNr: {8} | " &
								 "ClsDataDetail.MDData.MDYear: {9} | ",
								 ClsDataDetail.GetProposalNr,
								 ClsDataDetail.LLExportFileName,
								 ClsDataDetail.GetProposalMANr,
								 ClsDataDetail.GetProposalKDNr,
								 ClsDataDetail.GetProposalZHDNr,
								 ClsDataDetail.GetProposalVakNr,
								 m_InitialData.UserData.UserNr,
								 m_InitialData.MDData.MDGuid,
								 m_InitialData.MDData.MDNr,
								 m_InitialData.MDData.MDYear))

		Try
			Dim obj As New SPS.Propose.SendMail.Utility.ClsMain_Net(New SPS.Propose.SendMail.Utility.ClsMailSetting With {.ProposeNr2Send = ClsDataDetail.GetProposalNr, _
																																																										.Doc2Send = ClsDataDetail.LLExportFileName, _
																																																										.MANr2Send = ClsDataDetail.GetProposalMANr, _
																																																										.KDNr2Send = ClsDataDetail.GetProposalKDNr, _
																																																										.KDZNr2Send = ClsDataDetail.GetProposalZHDNr, _
																																																										.VakNr2Send = ClsDataDetail.GetProposalVakNr},
																																																									New SPS.Propose.SendMail.Utility.ClsSetting With {
																																																										.LogedUSNr = m_InitialData.UserData.UserNr,
																																																										.PersonalizedItems = m_InitialData.ProsonalizedData,
																																																										.SelectedMDGuid = m_InitialData.MDData.MDGuid,
																																																										.SelectedMDNr = m_InitialData.MDData.MDNr,
																																																										.SelectedMDYear = m_InitialData.MDData.MDYear,
																																																										.TranslationItems = m_InitialData.TranslationData})
			obj.ShowfrmProposalMail()
			'obj.Dispose()


		Catch ex As Exception
			m_Logger.LogError(String.Format("StartMailing: {0}", ex.StackTrace))

		End Try

	End Sub

	Sub OpenEMailForm(ByVal iProposeNr As Integer, ByVal iMANr As Integer, _
										ByVal iKDNr As Integer, ByVal iZHDNr As Integer?, _
										ByVal strJobNr As String)
		Dim strGuid As Guid = Guid.NewGuid()
		Dim strFilename As New List(Of String)

		ClsDataDetail.LLExportFileName.Clear()

		If strJobNr <> String.Empty Then
			Dim _ClsDb As New ClsDbFunc
			SQL4Print = _ClsDb.GetSQLString4Print(0)
			Dim aJobNr As String() = strJobNr.Split(New Char() {";", ",", "#"}, StringSplitOptions.RemoveEmptyEntries)
			For i As Integer = 0 To aJobNr.Length - 1
				PrintJobNr = aJobNr(i).ToString.Trim

				m_Logger.LogDebug(String.Format("jobnr: {0} | Setting will be initialized... ", PrintJobNr))

				Dim _Setting As New SPS.Listing.Print.Utility.ClsLLProposeSearchPrintSetting With {.DbConnString2Open = m_InitialData.MDData.MDDbConn, _
																																					 .SQL2Open = SQL4Print, _
																																					 .JobNr2Print = PrintJobNr,
																																													 .SelectedMDNr = m_InitialData.MDData.MDNr}
				Dim obj As New SPS.Listing.Print.Utility.ProposeSearchListing.ClsPrintProposeSearchList(_Setting)
				Dim strFullfilename = obj.PrintProposeTpl_1(False, String.Empty, ClsDataDetail.GetProposalNr, False, True)
				If File.Exists(strFullfilename) Then
					m_Logger.LogDebug(String.Format("Createdfile to send: ({0}) >>> {1}", PrintJobNr, strFullfilename))
					ClsDataDetail.LLExportFileName.Add(strFullfilename)
				Else
					m_Logger.LogWarning(String.Format("Template could not be created: ({0}) >>> {1}", PrintJobNr, strFullfilename))
				End If

			Next

		Else
			ClsDataDetail.LLExportFileName.Add(String.Empty)

		End If
		strFilename = ClsDataDetail.LLExportFileName
		m_Logger.LogDebug(String.Format("strFilename (Anzahl): {0} | strJobNr: {1}", strFilename.Count, strJobNr))

		m_Logger.LogDebug(String.Format("ClsDataDetail.GetProposalNr: {0} | " &
						 "ClsDataDetail.LLExportFileName: {1} | " &
						 "ClsDataDetail.GetProposalMANr: {2} | " &
						 "ClsDataDetail.GetProposalKDNr: {3} | " &
						 "ClsDataDetail.GetProposalZHDNr: {4} | " &
						 "ClsDataDetail.GetProposalVakNr: {5} | " &
						 "ClsDataDetail.UserData.UserNr: {6} | " &
						 "ClsDataDetail.MDData.MDGuid: {7} | " &
						 "ClsDataDetail.MDData.MDNr: {8} | " &
						 "ClsDataDetail.MDData.MDYear: {9} | ",
						 ClsDataDetail.GetProposalNr,
						 ClsDataDetail.LLExportFileName.ToArray(),
						 ClsDataDetail.GetProposalMANr,
						 ClsDataDetail.GetProposalKDNr,
						 ClsDataDetail.GetProposalZHDNr,
						 ClsDataDetail.GetProposalVakNr,
						 m_InitialData.UserData.UserNr,
						 m_InitialData.MDData.MDGuid,
						 m_InitialData.MDData.MDNr,
						 m_InitialData.MDData.MDYear))

		m_Logger.LogDebug("StartMailing wird aufgerufen...")
		StartMailing()
		m_Logger.LogDebug("quiting from StartMailing...")

		If strFilename.Count = 0 Then
			m_Logger.LogDebug("Keine Anhänge werden von Propose gewählt...")
			'Dim strMessage As String = "Das Mailmodul kann nicht gestartet werden. Mögliche Ursachen:{0}1 - {1}{0}2 - {2}{0}"
			'strMessage = String.Format(strMessage, vbNewLine, _
			'													"Sie haben kein Dokument in der Dokumentenverwaltung eingetragen.", _
			'													"Die Ausgabedatei konnte nicht erstellt werden.")

			'MsgBox(strMessage, MsgBoxStyle.Exclamation, m_Translate.GetSafeTranslationValue("Vorschlag per EMail versenden"))
		End If

	End Sub

	Sub OpenMAForm(ByVal obj As Object, ByVal employeeNumber As Integer)
		If employeeNumber = 0 Then Return

		Dim hub = MessageService.Instance.Hub
		Dim openEmployeeMng As New OpenEmployeeMngRequest(obj, m_InitialData.UserData.UserNr, m_InitialData.MDData.MDNr, employeeNumber)
		hub.Publish(openEmployeeMng)

	End Sub

	Sub OpenKDForm(ByVal obj As Object, ByVal customerNumber As Integer)
		If customerNumber = 0 Then Return

		' Send a request to open a customerMng form.
		Dim hub = MessageService.Instance.Hub
		Dim openCustomerMng As New OpenCustomerMngRequest(obj, m_InitialData.UserData.UserNr, m_InitialData.MDData.MDNr, customerNumber)
		hub.Publish(openCustomerMng)

	End Sub

	Sub OpenKDZHDForm(ByVal iKDNr As Integer, ByVal iKDZhdNr As Integer)
		If iKDNr = 0 Or iKDZhdNr = 0 Then Return
		Try
			Dim responsiblePersonsFrom = New frmResponsiblePerson(CreateInitialData(m_InitialData.MDData.MDNr, m_InitialData.UserData.UserNr))

			If (responsiblePersonsFrom.LoadResponsiblePersonData(iKDNr, iKDZhdNr)) Then
				responsiblePersonsFrom.Show()
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

	End Sub

	Sub OpenVakForm(ByVal iVakNr As Integer)
		If iVakNr = 0 Then Return
		Try
			Dim frmVacancy = New SPKD.Vakanz.frmVakanzen(m_InitialData)
			Dim setting = New SPKD.Vakanz.ClsVakSetting With {.SelectedVakNr = iVakNr}
			frmVacancy.VacancySetting = setting
			If Not frmVacancy.LoadData Then Return

			frmVacancy.Show()
			frmVacancy.BringToFront()

			'Dim obj As New SPKD.Vakanz.ClsMain_Net(New SPKD.Vakanz.ClsVakSetting With {.SelectedVakNr = iVakNr},
			'																			 New SPKD.Vakanz.ClsSetting With {.SelectedMDNr = m_InitialData.MDData.MDNr,
			'																																				.LogedUSNr = m_InitialData.UserData.UserNr})
			'obj.ShowfrmVakanzen()

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

	End Sub

	Sub OpenESForm(ByVal iMANr As Integer, _
								 ByVal iKDNr As Integer, ByVal iZHDNr As Integer, _
								 ByVal iVakNr As Integer)

		Dim ESnumber As Integer?
		If ESnumber.HasValue AndAlso ESnumber = 0 Then ESnumber = Nothing

		If Not IsUserActionAllowed(m_InitialData.UserData.UserNr, 250, m_InitialData.MDData.MDNr) Then Exit Sub

		Try

			Dim preselection = New SP.MA.EinsatzMng.PreselectionData With {.MDNr = m_InitialData.MDData.MDNr,
																						.EmployeeNumber = iMANr,
																						.CustomerNumber = iKDNr,
																						.ResponsiblePersonNumber = iZHDNr,
																						.ESAls = Nothing,
																						.ESAb = New DateTime(Now.Year, Now.Month, Now.Day),
																						.VAKNr = iVakNr,
																						.PNR = ClsDataDetail.GetProposalNr,
																						.CustomerKST = Nothing,
																						.BeraterMA = m_InitialData.UserData.UserKST,
																						.BeraterKD = m_InitialData.UserData.UserKST
																					 }

			'm_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(m_InitialData.TranslationData,
			'																																						ClsDataDetail.ProsonalizedData,
			'																																						ClsDataDetail.MDData,
			'																																						ClsDataDetail.UserData)
			Dim frmNewEs As SP.MA.EinsatzMng.UI.frmNewES = New SP.MA.EinsatzMng.UI.frmNewES(m_InitialData, preselection)

			frmNewEs.Show()
			frmNewEs.BringToFront()





			'Dim frmEinsatz As SP.MA.EinsatzMng.UI.frmES = CType(ClsDataDetail.GetModuleCach.GetModuleForm(ClsDataDetail.MDData.MDNr, SP.ModuleCaching.ModuleName.ESMng), frmES)

			'Dim preselection = New PreselectionData With {.MDNr = ClsDataDetail.MDData.MDNr,
			'																							.EmployeeNumber = iMANr,
			'																							.CustomerNumber = iKDNr,
			'																							.ResponsiblePersonNumber = iZHDNr,
			'																							.ESAls = Nothing,
			'																							.ESAb = New DateTime(Now.Year, Now.Month, Now.Day),
			'																							.VAKNr = iVakNr,
			'																							.PNR = ClsDataDetail.GetProposalNr,
			'																							.CustomerKST = Nothing,
			'																							.BeraterMA = ClsDataDetail.UserData.UserKST,
			'																							.BeraterKD = ClsDataDetail.UserData.UserKST
			'																						 }

			'm_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ClsDataDetail.TranslationData,
			'																																						ClsDataDetail.ProsonalizedData,
			'																																						ClsDataDetail.MDData,
			'																																						ClsDataDetail.UserData)
			'Dim frmNewEs As SP.MA.EinsatzMng.UI.frmNewES = New SP.MA.EinsatzMng.UI.frmNewES(m_InitializationData, preselection)

			'frmNewEs.Show()
			'frmNewEs.BringToFront()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.StackTrace))

		End Try

	End Sub

#End Region


End Module
