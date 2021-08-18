
Imports SPProgUtility.ProgPath
Imports SPProgUtility.Mandanten
Imports SPProgUtility.CommonSettings
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Settings
Imports SP.MA.AdvancePaymentMng.Settings
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Customer
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SP.DatabaseAccess.AdvancePaymentMng
Imports SP.DatabaseAccess.Employee

Imports SPS.Listing.Print.Utility


Namespace UI

  Public Class frmNewAdvancePayment

#Region "Private Consts"

		Private Const LANR_CHECK As Integer = 8900
		Private Const LANR_BANK_TRANSFER As Integer = 8920
		Private Const LANR_BAR As Integer = 8930

#End Region


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
    ''' The advance payment data access object.
    ''' </summary>
    Private m_AdvancePaymentDatabaseAccess As IAdvancePaymentDatabaseAccess

    ''' <summary>
    ''' The customer database access.
    ''' </summary>
    Private m_CustomerDatabaseAccess As ICustomerDatabaseAccess

    ''' <summary>
    ''' The employee database access.
    ''' </summary>
    Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

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
    Protected m_UCMediator As NewAdvancePaymentUserControlFormMediator

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

        m_InitializationData = _setting
        m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
        m_PreselectionData = preselectionData
      Catch ex As Exception
        m_Logger.LogError(ex.ToString)

      End Try
			m_SettingsManager = New SettingsManager

			' Dieser Aufruf ist für den Designer erforderlich.
			DevExpress.UserSkins.BonusSkins.Register()
      DevExpress.Skins.SkinManager.EnableFormSkins()

      m_SuppressUIEvents = True
      InitializeComponent()
      m_SuppressUIEvents = False

      ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

      m_ListOfPageControls.Add(ucPageSelectMandant)
			m_ListOfPageControls.Add(ucPageSelectAmountOfPayment)
      m_ListOfPageControls.Add(ucPageCreateZG)

      ' Init sub controls with configuration information
      For Each ctrl In m_ListOfPageControls
        ctrl.InitWithConfigurationData(m_InitializationData, m_Translate)
        ctrl.PreselectionData = preselectionData
      Next

      ' Create the user control mediator

      m_UCMediator = New NewAdvancePaymentUserControlFormMediator(m_Translate, Me,
                                                                  ucPageSelectMandant,
                                                                  ucPageSelectAmountOfPayment,
                                                                  ucPageCreateZG)

      ChangeMandant(m_InitializationData.MDData.MDNr)

      m_UtilityUI = New UtilityUI
      m_Utility = New Utility

      ' Translate controls.
      TranslateControls()

      Reset()

      ucPageSelectMandant.ActivatePage()

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
    ''' Gets the advance db access object.
    ''' </summary>
    Public ReadOnly Property AdvancePaymentDbAccess As IAdvancePaymentDatabaseAccess
      Get
        Return m_AdvancePaymentDatabaseAccess
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
    ''' Gets the employee db access object.
    ''' </summary>
    Public ReadOnly Property EmployeeDbAccess As IEmployeeDatabaseAccess
      Get
        Return m_EmployeeDatabaseAccess
      End Get
    End Property

    ''' <summary>
    ''' Gets the selected manant number.
    ''' </summary>
    ''' <returns>The selected MDNr.</returns>
    Public ReadOnly Property SelectedMDNr As Integer?
      Get

        Dim mandantData = ucPageSelectMandant.SelectedCandidateAndAdvisorData.MandantData

        If mandantData Is Nothing Then
          Return Nothing
        Else
          Return mandantData.MandantNumber
        End If
      End Get
    End Property

		Public ReadOnly Property NewAdvancePaymentNumber() As Integer?
			Get
				Return ucPageCreateZG.ZGNrOfNewlyCreatedAdvancePayment
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

        m_CommonDatabaseAccess = New DatabaseAccess.Common.CommonDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
        m_AdvancePaymentDatabaseAccess = New DatabaseAccess.AdvancePaymentMng.AdvancePaymentDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
        m_CustomerDatabaseAccess = New DatabaseAccess.Customer.CustomerDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
        m_EmployeeDatabaseAccess = New DatabaseAccess.Employee.EmployeeDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
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

      Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

			wizardCtrl.Text = m_Translate.GetSafeTranslationValue(wizardCtrl.Text)
			wizardCtrl.NextText = String.Format("{0} >", m_Translate.GetSafeTranslationValue("Weiter"))
			wizardCtrl.PreviousText = String.Format("< {0}", m_Translate.GetSafeTranslationValue("Zurück"))
			wizardCtrl.CancelText = m_Translate.GetSafeTranslationValue("Abbrechen")
			wizardCtrl.FinishText = m_Translate.GetSafeTranslationValue("Vorschuss erstellen")

			pageMandantData.Text = String.Format("1) {0}", m_Translate.GetSafeTranslationValue("Mandant, BeraterIn und Kandidat eingeben"))
      pageMandantData.DescriptionText = m_Translate.GetSafeTranslationValue("Bitte wählen Sie den Mandant, den Berater/In, sowie den Kandidat")

			pageAmountOfPayment.Text = String.Format("2) {0}", m_Translate.GetSafeTranslationValue("Auszahlung festlegen"))
      pageAmountOfPayment.DescriptionText = m_Translate.GetSafeTranslationValue("Bitte legen Sie die Höhe der Auszahlung fest")

			pageCreateAdvancePayment.Text = m_Translate.GetSafeTranslationValue("Zusammenfassung")
      pageCreateAdvancePayment.DescriptionText = m_Translate.GetSafeTranslationValue("Prüfen Sie nochmals die Daten und erstellen Sie anschliessend den Vorschuss")

    End Sub

		Private Sub OnfrmNewAdvancePayment_VisibleChanged(sender As System.Object, e As System.EventArgs) Handles MyBase.VisibleChanged

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

        If e.PrevPage.Name = pageMandantData.Name Then
          allowForward = ucPageSelectMandant.ValidateData()
				ElseIf e.PrevPage.Name = pageAmountOfPayment.Name Then
					allowForward = ucPageSelectAmountOfPayment.ValidateData()

				End If

				If Not allowForward Then
					e.Cancel = True
					Return
				End If

				End If

			If e.Page.Name = pageMandantData.Name Then
				ucPageSelectMandant.ActivatePage()
			ElseIf e.Page.Name = pageAmountOfPayment.Name Then
				ucPageSelectAmountOfPayment.ActivatePage()

				'ElseIf e.Page.Name = pageBankData.Name Then

				'	Dim PaymentType As Integer = m_UCMediator.SelectedPaymentData.PaymentType
				'	e.Cancel = True
				'	wizardCtrl.SelectedPage = pageCreateAdvancePayment
				'	ucPageCreateZG.ActivatePage()
				'	'If e.Direction = DevExpress.XtraWizard.Direction.Forward Then
				'	'	e.Cancel = True
				'	'	wizardCtrl.SelectedPage = pageCreateAdvancePayment
				'	'	ucPageCreateZG.ActivatePage()
				'	'Else
				'	'	ucPageSelectBank.ActivatePage()
				'	'End If

				'	'ucPageSelectBank.ActivatePage()

			ElseIf e.Page.Name = pageCreateAdvancePayment.Name Then
				ucPageCreateZG.ActivatePage()
			End If


		End Sub

    ''' <summary>
    ''' Handles click on finish button.
    ''' </summary>
    Private Sub OnWizardCtrl_FinishClick(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles wizardCtrl.FinishClick
			wizardCtrl.Enabled = False

      If (m_UCMediator.HandleFinishClick()) Then

        If Not ucPageCreateZG.ZGNrOfNewlyCreatedAdvancePayment.HasValue Then
					wizardCtrl.Enabled = True
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Vorschuss konnte nicht angelegt werden."))
          DialogResult = DialogResult.Cancel

				Else
					DialogResult = DialogResult.OK

					If ucPageCreateZG.OpenAdvancePaymentForm.HasValue AndAlso ucPageCreateZG.OpenAdvancePaymentForm Then

						' Send a request to open a einsatzMng form.
						Dim hub = MessageService.Instance.Hub
						Dim openCustomerMng As New OpenAdvancePaymentMngRequest(Me, m_InitializationData.UserData.UserNr, SelectedMDNr, ucPageCreateZG.ZGNrOfNewlyCreatedAdvancePayment)
						hub.Publish(openCustomerMng)
					End If

					wizardCtrl.Enabled = True
					If ucPageCreateZG.PrintAdvancePayment.HasValue AndAlso ucPageCreateZG.PrintAdvancePayment Then
						PrintAdvancePaymentData()
					End If

				End If

					Close()

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


    ''' <summary>
    ''' Resets the from.
    ''' </summary>
    Private Sub Reset()

      ' Reset all the child controls
      For Each ctrl In m_ListOfPageControls
        ctrl.Reset()
      Next

    End Sub


		Private Function PrintAdvancePaymentData() As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strResult As String = "success..."

			Try
				Dim mandantNumber As Integer = m_UCMediator.SelectedCandidateAndAdvisorData.MandantData.MandantNumber
				Dim employeeNumber As Integer = m_UCMediator.SelectedCandidateAndAdvisorData.EmployeeData.EmployeeNumber
				Dim jahr As Integer = m_UCMediator.SelectedPaymentData.Year
				Dim lp As Integer = m_UCMediator.SelectedPaymentData.Month
				Dim PaymentType As Integer = m_UCMediator.SelectedPaymentData.PaymentType

				'0: Check
				'1: Quittung
				'2: Überweisung

				Dim docart As Integer = If(PaymentType = LANR_CHECK, 0, If(PaymentType = LANR_BANK_TRANSFER, 2, 1))

				Dim _settring As New ClsLLAdvancePaymentPrintSetting With {.ZGNr = ucPageCreateZG.ZGNrOfNewlyCreatedAdvancePayment,
																																	 .DbConnString2Open = m_InitializationData.MDData.MDDbConn,
																																	 .LogedUSNr = m_InitializationData.UserData.UserNr,
																																	 .SelectedMDNr = m_InitializationData.MDData.MDNr,
																																	 .docart = docart,
																																	 .frmhwnd = Me.Handle,
																																	 .PerosonalizedData = m_InitializationData.ProsonalizedData,
																																	 .TranslationItems = m_InitializationData.TranslationData,
																																	 .ListFilterBez = New List(Of String)(New String() {String.Format("{0}", "")}),
																																	 .ShowAsDesign = If(m_InitializationData.UserData.UserNr = 1, True, False)}

				Dim obj As New AdvancePaymentData.ClsPrintAdvancePaymentData(_settring)
				strResult = obj.PrintAdvancePaymentDocument()

				'If Not strResult.ToLower.Contains("error") Then m_ZGData = m_AdvancePaymentDatabaseAccess.LoadZGMasterData(ZGData.ZGNr)

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}:ZGNr: {1}:{2}", strMethodeName, ucPageCreateZG.ZGNrOfNewlyCreatedAdvancePayment, ex.ToString))
				strResult = "Error: " & String.Format("{0}:ZGNr: {1}:{2}", strMethodeName, ucPageCreateZG.ZGNrOfNewlyCreatedAdvancePayment, ex.ToString)

			End Try

			Return strResult

		End Function


#End Region

  End Class

End Namespace
