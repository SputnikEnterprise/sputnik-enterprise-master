Imports SPProgUtility.CommonSettings
Imports SPProgUtility.ProgPath
Imports SPProgUtility.Mandanten
Imports SP.MA.EinsatzMng.UI
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure
Imports SP.Infrastructure.UI
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.ES
Imports SP.DatabaseAccess.Common
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SP.Infrastructure.Settings
Imports SP.MA.EinsatzMng.Settings


Namespace UI

	Public Class frmNewES

#Region "Private Fields"

		''' <summary>
		''' The Initialization data.
		''' </summary>
		Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

		''' <summary>
		''' The translation value helper.
		''' </summary>
		Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

		''' <summary>
		''' The common database access.
		''' </summary>
		Protected m_CommonDatabaseAccess As ICommonDatabaseAccess

		''' <summary>
		''' The data access object.
		''' </summary>
		Protected m_ESDataAccess As IESDatabaseAccess

		''' <summary>
		''' The employee data access object.
		''' </summary>
		Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

		''' <summary>
		''' The customer database access.
		''' </summary>
		Private m_CustomerDatabaseAccess As ICustomerDatabaseAccess

		''' <summary>
		''' Contains the employee number of the loaded employee data.
		''' </summary>
		Private m_EmployeeNumber As Integer?

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI

		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As Utility

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		''' <summary>
		''' The settings manager.
		''' </summary>
		Private m_SettingsManager As ISettingsManager

		''' <summary>
		''' Boolean flag indicating if initial data has been loaded.
		''' </summary>
		Private m_IsInitialDataLoaded As Boolean = False

		''' <summary>
		''' List of tab controls.
		''' </summary>
		Private m_ListOfPageControls As New List(Of ucWizardPageBaseControl)

		''' <summary>
		''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
		''' </summary>
		Private m_SuppressUIEvents As Boolean = False

		''' <summary>
		''' Communication support between controls.
		''' </summary>
		Protected m_UCMediator As NewESUserControlFormMediator

		''' <summary>
		''' The common settings.
		''' </summary>
		Private m_Common As CommonSetting

		''' <summary>
		''' The SPProgUtility object.
		''' </summary>
		Protected m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath

		''' <summary>
		''' The mandant.
		''' </summary>
		''' <remarks></remarks>
		Private m_Mandant As Mandant

		Private m_path As ClsProgPath

		''' <summary>
		''' The current connection string.
		''' </summary>
		Private m_CurrentConnectionString = String.Empty

		''' <summary>
		''' The preselection data.
		''' </summary>
		Private m_PreselectionData As PreselectionData

#End Region

#Region "Constructor"

		''' <summary>
		''' The constructor.
		''' </summary>
		''' <param name="_setting">The settings.</param>
		''' <param name="preselectionData">The preselection data.</param>
		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass, ByVal preselectionData As PreselectionData)

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
			Try
				' Mandantendaten
				m_Mandant = New Mandant
				m_path = New ClsProgPath
				m_Common = New CommonSetting

				m_SettingsManager = New SettingsManager
				m_InitializationData = _setting
				m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
				m_PreselectionData = preselectionData
			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

			' Dieser Aufruf ist für den Designer erforderlich.
			DevExpress.UserSkins.BonusSkins.Register()
			DevExpress.Skins.SkinManager.EnableFormSkins()

			m_SuppressUIEvents = True
			InitializeComponent()
			m_SuppressUIEvents = False

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

			m_ListOfPageControls.Add(ucCandidateAndCustomer)
			m_ListOfPageControls.Add(ucESData)
			m_ListOfPageControls.Add(ucSalaryData)
			m_ListOfPageControls.Add(ucCreateES)

			' Init sub controls with configuration information
			For Each ctrl In m_ListOfPageControls
				ctrl.InitWithConfigurationData(m_InitializationData, m_Translate)
				ctrl.PreselectionData = preselectionData
			Next

			' Create the user control mediator
			m_UCMediator = New NewESUserControlFormMediator(Me,
													  ucCandidateAndCustomer,
													  ucESData,
													  ucSalaryData,
													  ucCreateES,
													  m_Translate)

			ChangeMandant(m_InitializationData.MDData.MDNr)

			m_UtilityUI = New UtilityUI
			m_Utility = New Utility

			' Translate controls.
			TranslateControls()

			Reset()

			ucCandidateAndCustomer.ActivatePage()

		End Sub

#End Region

#Region "Public Properties"

		''' <summary>
		''' Gets the common db access object.
		''' </summary>
		Public ReadOnly Property CommonDbAccess As ICommonDatabaseAccess
			Get
				Return m_CommonDatabaseAccess
			End Get
		End Property

		''' <summary>
		''' Gets the ES db access object.
		''' </summary>
		Public ReadOnly Property ESDbAccess As IESDatabaseAccess
			Get
				Return m_ESDataAccess
			End Get
		End Property

		''' <summary>
		''' Gets the employee db access object.
		''' </summary>
		Public ReadOnly Property EmployeeDbAccess As IEmployeeDatabaseAccess
			Get
				Return m_EmployeeDatabaseAccess
			End Get
		End Property

		''' <summary>
		''' Gets the customer db access object.
		''' </summary>
		Public ReadOnly Property CustomerDbAccess As ICustomerDatabaseAccess
			Get
				Return m_CustomerDatabaseAccess
			End Get
		End Property

		''' <summary>
		''' Gets the selected manant number.
		''' </summary>
		''' <returns>The selected MDNr.</returns>
		Public ReadOnly Property SelectedMDNr As Integer?
			Get
				Dim mandantData = ucCandidateAndCustomer.SelectedCandidateAndCustomerData.MandantData

				If mandantData Is Nothing Then
					Return Nothing
				Else
					Return mandantData.MandantNumber
				End If
			End Get
		End Property

		''' <summary>
		''' Gets the ES number of the newly created ES.
		''' </summary>
		''' <returns>The ES number of the newly created ES.</returns>
		Public ReadOnly Property ESNrOfNewlyCreatedES As Integer?
			Get
				Return ucCreateES.ESNrOfNewlyCreatedES
			End Get
		End Property

#End Region

#Region "Public Methods"

		''' <summary>
		''' Changes the mandant nr.
		''' </summary>
		''' <param name="mdNr">The mandant number.</param>
		Public Sub ChangeMandant(ByVal mdNr As Integer)

			Dim conStr = m_Mandant.GetSelectedMDData(mdNr).MDDbConn

			If Not m_CurrentConnectionString = conStr Then

				m_CurrentConnectionString = conStr

				m_ESDataAccess = New DatabaseAccess.ES.ESDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
				m_CommonDatabaseAccess = New DatabaseAccess.Common.CommonDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
				m_EmployeeDatabaseAccess = New DatabaseAccess.Employee.EmployeeDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
				m_CustomerDatabaseAccess = New DatabaseAccess.Customer.CustomerDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
			End If

		End Sub

		''' <summary>
		''' Validates all data.
		''' </summary>
		Public Function ValidateData() As Boolean

			Dim valid As Boolean = True

			For Each ctrl In m_ListOfPageControls
				valid = valid AndAlso ctrl.ValidateData()
			Next

			Return valid
		End Function

#End Region

#Region "Private Methods"

		''' <summary>
		''' Translates the controls
		''' </summary>
		Private Sub TranslateControls()

			wizardCtrl.Text = m_Translate.GetSafeTranslationValue(wizardCtrl.Text)
			wizardCtrl.NextText = String.Format("{0} >", m_Translate.GetSafeTranslationValue("Weiter"))
			wizardCtrl.PreviousText = String.Format("< {0}", m_Translate.GetSafeTranslationValue("Zurück"))
			wizardCtrl.CancelText = m_Translate.GetSafeTranslationValue("Abbrechen")
			wizardCtrl.FinishText = m_Translate.GetSafeTranslationValue("Einsatz erstellen")

			pageCandidateAndCustomer.Text = String.Format("1) {0}", m_Translate.GetSafeTranslationValue("Mandant, Mitarbeiter und Firma eingeben"))
			pageCandidateAndCustomer.DescriptionText = m_Translate.GetSafeTranslationValue("Bitte wählen Sie den Mandant, den Mitarbeiter sowie die Firma")

			pageESData.Text = String.Format("2) {0}", m_Translate.GetSafeTranslationValue("Einsatzdaten eingeben"))
			pageESData.DescriptionText = m_Translate.GetSafeTranslationValue("Bitte geben Sie die Einsatzdaten ein")

			pageSalaryData.Text = String.Format("3) {0}", m_Translate.GetSafeTranslationValue("Lohndaten eingeben"))
			pageSalaryData.DescriptionText = m_Translate.GetSafeTranslationValue("Bitte geben Sie die Lohndaten ein")

			pageCreateES.Text = m_Translate.GetSafeTranslationValue("Zusammenfassung")
			pageCreateES.DescriptionText = m_Translate.GetSafeTranslationValue("Prüfen Sie nochmals die Daten und erstellen Sie anschliessend den Einsatz")

		End Sub

		''' <summary>
		''' Resets the from.
		''' </summary>
		Private Sub Reset()

			' Reset all the child controls
			For Each ctrl In m_ListOfPageControls
				ctrl.Reset()
			Next

		End Sub

		Private Sub OnfrmNewES_VisibleChanged(sender As System.Object, e As System.EventArgs) Handles MyBase.VisibleChanged

			If Visible Then
				LoadFormSettings()
			End If

		End Sub

		Private Sub LoadFormSettings()

			Try
				Dim setting_form_height = m_SettingsManager.ReadInteger(SettingKeys.SETTING_NEW_FORM_HEIGHT)
				Dim setting_form_width = m_SettingsManager.ReadInteger(SettingKeys.SETTING_NEW_FORM_WIDTH)
				Dim setting_form_location = m_SettingsManager.ReadString(SettingKeys.SETTING_NEW_FORM_LOCATION)

				If setting_form_height > 0 Then Me.Height = Math.Max(Me.Height, setting_form_height)
				If setting_form_width > 0 Then Me.Width = Math.Max(Me.Width, setting_form_width)

				If Not String.IsNullOrEmpty(setting_form_location) Then
					Dim aLoc As String() = setting_form_location.Split(CChar(";"))
					If Screen.AllScreens.Length = 1 Then
						If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
					End If
					Me.Location = New System.Drawing.Point(Val(aLoc(0)), Val(aLoc(1)))
				End If


			Catch ex As Exception
				m_Logger.LogError(ex.ToString())

			End Try

		End Sub

		Private Sub SaveFromSettings()

			' Save form location, width and height in setttings
			Try
				If Not Me.WindowState = FormWindowState.Minimized Then
					m_SettingsManager.WriteString(SettingKeys.SETTING_NEW_FORM_LOCATION, String.Format("{0};{1}", Me.Location.X, Me.Location.Y))
					m_SettingsManager.WriteInteger(SettingKeys.SETTING_NEW_FORM_WIDTH, Me.Width)
					m_SettingsManager.WriteInteger(SettingKeys.SETTING_NEW_FORM_HEIGHT, Me.Height)

					m_SettingsManager.SaveSettings()
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())

			End Try

		End Sub

		''' <summary>
		''' Handles page changing event of wizard.
		''' </summary>
		Private Sub OnSelectedPageChanging(sender As System.Object, e As DevExpress.XtraWizard.WizardPageChangingEventArgs) Handles wizardCtrl.SelectedPageChanging

			If e.Direction = DevExpress.XtraWizard.Direction.Forward Then

				Dim allowForward As Boolean = True

				If e.PrevPage.Name = pageCandidateAndCustomer.Name Then
					allowForward = ucCandidateAndCustomer.ValidateData()
				ElseIf e.PrevPage.Name = pageESData.Name Then
					allowForward = ucESData.ValidateData()
				ElseIf e.PrevPage.Name = pageSalaryData.Name Then
					allowForward = ucSalaryData.ValidateData()
				End If

				If Not allowForward Then
					e.Cancel = True
					Return
				End If

			End If

			If e.Page.Name = pageCandidateAndCustomer.Name Then
				ucCandidateAndCustomer.ActivatePage()
			ElseIf e.Page.Name = pageESData.Name Then
				ucESData.ActivatePage()
			ElseIf e.Page.Name = pageSalaryData.Name Then
				ucSalaryData.ActivatePage()
			ElseIf e.Page.Name = pageCreateES.Name Then
				ucCreateES.ActivatePage()
			End If
		End Sub

		''' <summary>
		''' Handles click on finish button.
		''' </summary>
		Private Sub OnWizardCtrl_FinishClick(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles wizardCtrl.FinishClick

			If (m_UCMediator.HandleFinishClick()) Then

				If Not ucCreateES.ESNrOfNewlyCreatedES.HasValue Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Einsatz konnte nicht angelegt werden."))
					DialogResult = DialogResult.Cancel
				Else
					DialogResult = DialogResult.OK

					' Send a request to open a einsatzMng form.
					Dim hub = MessageService.Instance.Hub
					Dim openCustomerMng As New OpenEinsatzMngRequest(Me, m_InitializationData.UserData.UserNr, SelectedMDNr, ucCreateES.ESNrOfNewlyCreatedES)
					hub.Publish(openCustomerMng)

					Close()
				End If


			End If

		End Sub

		Private Sub Onform_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
			SaveFromSettings()
		End Sub

		''' <summary>
		''' Handles click on cancel button.
		''' </summary>
		Private Sub OnWizardCtrl_CancelClick(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles wizardCtrl.CancelClick
			DialogResult = DialogResult.Cancel
			Close()
		End Sub

#End Region


		Sub Test()
			Dim FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"

			Dim MANDANT_XML_SETTING_SPUTNIK_PAYROLL_SETTING As String = "MD_{0}/Lohnbuchhaltung"
			Dim m_PayrollSetting As String = String.Format(MANDANT_XML_SETTING_SPUTNIK_PAYROLL_SETTING, m_InitializationData.MDData.MDNr)

			Dim payrollcheckfee As String = m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(m_Mandant.GetDefaultMDNr), String.Format("{0}/payrollcheckfee", m_PayrollSetting))
			Dim advancepaymentcheckfee As String = m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(m_Mandant.GetDefaultMDNr), String.Format("{0}/advancepaymentcheckfee", m_PayrollSetting))
			Dim advancepaymenttransferfee As String = m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(m_Mandant.GetDefaultMDNr), String.Format("{0}/advancepaymenttransferfee", m_PayrollSetting))
			Dim advancepaymentcashfee As String = m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(m_Mandant.GetDefaultMDNr), String.Format("{0}/advancepaymentcashfee", m_PayrollSetting))
			Dim advancepaymenttransferinternationalfee As String = m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(m_Mandant.GetDefaultMDNr), String.Format("{0}/advancepaymenttransferinternationalfee", m_PayrollSetting))
			Dim payrolltransferinternationalfee As String = m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(m_Mandant.GetDefaultMDNr), String.Format("{0}/payrolltransferinternationalfee", m_PayrollSetting))
			Dim bvgafter As String = m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(m_Mandant.GetDefaultMDNr), String.Format("{0}/bvgafter", m_PayrollSetting))
			Dim bvginterval As String = m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(m_Mandant.GetDefaultMDNr), String.Format("{0}/bvginterval", m_PayrollSetting))
			Dim bvgintervaladd As String = m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(m_Mandant.GetDefaultMDNr), String.Format("{0}/bvgintervaladd", m_PayrollSetting))
			Dim calculatebvgwithesdays As Boolean = m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(m_Mandant.GetDefaultMDNr), String.Format("{0}/calculatebvgwithesdays", m_PayrollSetting))



			Dim esendebynull As String = m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(m_Mandant.GetDefaultMDNr), String.Format("{0}/esendebynull", FORM_XML_MAIN_KEY))


			'Dim ask4transferverleihtowos As Boolean? = ParseToBoolean(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(m_Mandant.GetDefaultMDNr), String.Format("{0}/ask4transferverleihtowos", FORM_XML_MAIN_KEY)), False)
			'Dim warnbynocustomercreditlimit As Boolean? = ParseToBoolean(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(m_Mandant.GetDefaultMDNr), String.Format("{0}/warnbynocustomercreditlimit", FORM_XML_MAIN_KEY)), False)
			'Dim companyallowednopvl As Boolean? = ParseToBoolean(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(m_Mandant.GetDefaultMDNr), String.Format("{0}/companyallowednopvl", FORM_XML_MAIN_KEY)), False)
			'' if true, userdata.userkst else makst and kdkst
			'Dim selectadvisorkst As Boolean? = ParseToBoolean(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(m_Mandant.GetDefaultMDNr), String.Format("{0}/selectadvisorkst", FORM_XML_MAIN_KEY)), False)
			'Dim calculatecustomerrefundinmarge As Boolean? = ParseToBoolean(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(m_Mandant.GetDefaultMDNr), String.Format("{0}/calculatecustomerrefundinmarge", FORM_XML_MAIN_KEY)), False)


		End Sub


	End Class

End Namespace