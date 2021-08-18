Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Common
Imports SP.Infrastructure
Imports SP.Infrastructure.UI
Imports SPProgUtility.Mandanten
Imports SPProgUtility.CommonSettings
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Common.DataObjects
Imports System.ComponentModel
Imports SPProgUtility.ProgPath
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages

Namespace UI

  Public Class frmNewEmployee

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
    ''' The data access object.
    ''' </summary>
    Private m_EmployeeDataAccess As IEmployeeDatabaseAccess

    ''' <summary>
    ''' The common database access.
    ''' </summary>
    Protected m_CommonDatabaseAccess As ICommonDatabaseAccess

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
    ''' The SPProgUtility object.
    ''' </summary>
    Private m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath

    ''' <summary>
    ''' The mandant.
    ''' </summary>
    Private m_Mandant As Mandant

    ''' <summary>
    ''' The path.
    ''' </summary>
    Private m_path As ClsProgPath

    ''' <summary>
    ''' Communication support between controls.
    ''' </summary>
    Protected m_UCMediator As NewEmployeeUserControlFormMediator

    ''' <summary>
    ''' The common settings.
    ''' </summary>
    Private m_Common As CommonSetting

    ''' <summary>
    ''' List of tab controls.
    ''' </summary>
    Private m_ListOfPageControls As New List(Of ucWizardPageBaseControl)

    ''' <summary>
    ''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
    ''' </summary>
    Private m_SuppressUIEvents As Boolean = False

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

      ' Dieser Aufruf ist für den Designer erforderlich.
      DevExpress.UserSkins.BonusSkins.Register()
      DevExpress.Skins.SkinManager.EnableFormSkins()

      m_SuppressUIEvents = True
      InitializeComponent()
      m_SuppressUIEvents = False

      ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

      m_ListOfPageControls.Add(ucWelcomePage)
			'm_ListOfPageControls.Add(ucEmployeeBasicDataPage)
			m_ListOfPageControls.Add(ucEmployeeAdditionalData1Page)
      m_ListOfPageControls.Add(ucEmployeeAdditionalData2Page)
      m_ListOfPageControls.Add(ucCreateEmployeePage)

      ' Init sub controls with configuration information
      For Each ctrl In m_ListOfPageControls
        ctrl.InitWithConfigurationData(m_InitializationData, m_Translate)
        ctrl.PreselectionData = preselectionData
      Next

      'Create the user control mediator
      m_UCMediator = New NewEmployeeUserControlFormMediator(Me,
                                                            ucWelcomePage,
                                                            ucEmployeeBasicDataPage,
                                                            ucEmployeeAdditionalData1Page,
                                                            ucEmployeeAdditionalData2Page,
                                                            ucCreateEmployeePage,
                                                            m_Translate)

      ChangeMandant(m_InitializationData.MDData.MDNr)

      m_UtilityUI = New UtilityUI
      m_Utility = New Utility

      ' Translate controls.
      TranslateControls()

      Reset()

      ucWelcomePage.ActivatePage()

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
    ''' Gets the employee db access object.
    ''' </summary>
    Public ReadOnly Property EmployeeDbAccess As IEmployeeDatabaseAccess
      Get
        Return m_EmployeeDataAccess
      End Get
    End Property


    ''' <summary>
    ''' Gets the selected mandant number.
    ''' </summary>
    ''' <returns>The selected MDNr.</returns>
    Public ReadOnly Property SelectedMDNr As Integer?
      Get
        Dim mandantData = ucWelcomePage.SelectedMandantAndAdvisorData.MandantData

        If mandantData Is Nothing Then
          Return Nothing
        Else
          Return mandantData.MandantNumber
        End If
      End Get
    End Property

    ''' <summary>
    ''' Employee number of newly added employee.
    ''' </summary>
    Public Property NewlyAddedEmployeeNumber As Integer?

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
        m_EmployeeDataAccess = New DatabaseAccess.Employee.EmployeeDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)

      End If

    End Sub

    Public Function ActivateBasicDataPage() As Boolean

      If ucWelcomePage.ValidateData Then
        wizardCtrl.SelectedPageIndex = 1
        Return True
      Else
        Return False
      End If

    End Function

#End Region

#Region "Private Methods"

    ''' <summary>
    '''  Trannslate the controls.
    ''' </summary>
    Private Sub TranslateControls()

      Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
			wizardCtrl.Text = m_Translate.GetSafeTranslationValue(wizardCtrl.Text)
			wizardCtrl.NextText = String.Format("{0} >", m_Translate.GetSafeTranslationValue("Weiter"))
			wizardCtrl.PreviousText = String.Format("< {0}", m_Translate.GetSafeTranslationValue("Zurück"))
			wizardCtrl.CancelText = m_Translate.GetSafeTranslationValue("Abbrechen")

			wizardCtrl.FinishText = m_Translate.GetSafeTranslationValue("Kandidat erstellen")

			pageWelcomePage.Text = String.Format("1) {0}", m_Translate.GetSafeTranslationValue("Mandant und Berater eingeben"))
			pageWelcomePage.DescriptionText = m_Translate.GetSafeTranslationValue("Bitte wählen Sie den Mandant und den Berater")

			pageBasicDataPage.Text = String.Format("2) {0}", m_Translate.GetSafeTranslationValue("Kandidatendaten eingeben"))
      pageBasicDataPage.DescriptionText = m_Translate.GetSafeTranslationValue("Bitte geben Sie die Daten des neuen Kandidaten ein")

			pageAdditionalData1.Text = String.Format("3) {0}", m_Translate.GetSafeTranslationValue("Kandidatendaten eingeben (forts.)"))
      pageAdditionalData1.DescriptionText = m_Translate.GetSafeTranslationValue("Bitte geben Sie Eigenschaft und Merkmale sowie die Qualifikation ein")

			pageAdditionalData2.Text = String.Format("4) {0}", m_Translate.GetSafeTranslationValue("Kandidatendaten eingeben (forts.)"))
      pageAdditionalData2.DescriptionText = m_Translate.GetSafeTranslationValue("Bitte geben Sie die Lohndaten ein")

      pageCreateEmployee.Text = m_Translate.GetSafeTranslationValue("Zusammenfassung")
      pageCreateEmployee.DescriptionText = m_Translate.GetSafeTranslationValue("Prüfen Sie nochmals die Daten und erstellen Sie anschliessend den Kandidaten")
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



    ''' <summary>
    ''' Validates all data.
    ''' </summary>
    Private Function ValidateData() As Boolean

      Dim valid As Boolean = True

      For Each ctrl In m_ListOfPageControls
        valid = valid AndAlso ctrl.ValidateData()
      Next

      Return valid
    End Function


    ''' <summary>
    ''' Handles page changing event of wizard.
    ''' </summary>
    Private Sub OnSelectedPageChanging(sender As System.Object, e As DevExpress.XtraWizard.WizardPageChangingEventArgs) Handles wizardCtrl.SelectedPageChanging

      If e.Direction = DevExpress.XtraWizard.Direction.Forward Then

        Dim allowForward As Boolean = True

        If e.PrevPage.Name = pageWelcomePage.Name Then
          allowForward = ucWelcomePage.ValidateData()
					'ElseIf e.PrevPage.Name = pageBasicDataPage.Name Then
					'  allowForward = ucEmployeeBasicDataPage.ValidateData()
				ElseIf e.PrevPage.Name = pageAdditionalData1.Name Then
          allowForward = ucEmployeeAdditionalData1Page.ValidateData()
        ElseIf e.PrevPage.Name = pageAdditionalData2.Name Then
          allowForward = ucEmployeeAdditionalData2Page.ValidateData()
        End If

        If Not allowForward Then
          e.Cancel = True
          Return
        End If

      End If

			If e.Page.Name = pageWelcomePage.Name Then
				ucWelcomePage.ActivatePage()
				'ElseIf e.Page.Name = pageBasicDataPage.Name Then
				'  ucEmployeeBasicDataPage.ActivatePage()
			ElseIf e.Page.Name = pageAdditionalData1.Name Then
				ucEmployeeAdditionalData1Page.ActivatePage()
			ElseIf e.Page.Name = pageAdditionalData2.Name Then
				ucEmployeeAdditionalData2Page.ActivatePage()
			ElseIf e.Page.Name = pageCreateEmployee.Name Then
				ucCreateEmployeePage.ActivatePage()
			End If

		End Sub

    ''' <summary>
    ''' Handles click on finish button.
    ''' </summary>
    Private Sub OnWizardCtrl_FinishClick(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles wizardCtrl.FinishClick

			'If (ValidateData()) Then

			'  Dim FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"

			'  If ValidateData() Then

			'    Dim mdnr = SelectedMDNr.Value

			'    Dim currencyvalue As String = m_Utility.ParseToString(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mdnr),
			'                                                                                 String.Format("{0}/currencyvalue", FORM_XML_MAIN_KEY)), "CHF")
			'    Dim mainlanguagevalue As String = m_Utility.ParseToString(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mdnr),
			'                                                                                     String.Format("{0}/mainlanguagevalue", FORM_XML_MAIN_KEY)), "deutsch")

			'    Dim employeezahlart As String = m_Utility.ParseToString(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mdnr),
			'                                                           String.Format("{0}/employeezahlart", FORM_XML_MAIN_KEY)), "K")
			'    Dim employeebvgcode As String = m_Utility.ParseToString(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mdnr),
			'                                                           String.Format("{0}/employeebvgcode", FORM_XML_MAIN_KEY)), "9")
			'    Dim employeerahmenarbeitsvertrag As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mdnr),
			'                                                                                                   String.Format("{0}/employeerahmenarbeitsvertrag", FORM_XML_MAIN_KEY)), True)
			'    Dim employeeferienback As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mdnr),
			'                                                                                         String.Format("{0}/employeeferienback", FORM_XML_MAIN_KEY)), False)
			'    Dim employeefeiertagback As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mdnr),
			'                                                                                           String.Format("{0}/employeefeiertagback", FORM_XML_MAIN_KEY)), False)
			'    Dim employee13lohnback As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mdnr),
			'                                                                                         String.Format("{0}/employee13lohnback", FORM_XML_MAIN_KEY)), False)

			'    Dim employeenolo As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mdnr),
			'                                                                                   String.Format("{0}/employeenolo", FORM_XML_MAIN_KEY)), False)
			'    Dim employeenozg As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mdnr),
			'                                                                                   String.Format("{0}/employeenozg", FORM_XML_MAIN_KEY)), False)

			'    Dim employeesecsuvacode As String = m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mdnr), String.Format("{0}/employeesecsuvacode", FORM_XML_MAIN_KEY))

			'    Dim employeeNumberOffsetFromSettings As Integer = ReadEmployeeOffsetFromSettings()

			'    Dim mandantAndAdvisorData = m_UCMediator.SelectedMandantAndAdvisorData
			'    Dim basicData = m_UCMediator.SelectedBasiscData
			'    Dim additionalData1 = m_UCMediator.SelectedAdditionalData1
			'    Dim additionalData2 = m_UCMediator.selectedAdditionalData2

			'    Dim newEmployeeInitData As New NewEmployeeInitData With {
			'      .KST = If(mandantAndAdvisorData.AdvisorData Is Nothing, String.Empty, mandantAndAdvisorData.AdvisorData.KST),
			'      .Lastname = basicData.Lastname,
			'      .Firstname = basicData.Firstname,
			'      .Street = basicData.Street,
			'      .CountryCode = basicData.CountryCode,
			'      .Postcode = basicData.PostCode,
			'      .Location = basicData.Location,
			'      .Gender = basicData.Gender,
			'      .Nationality = basicData.Nationality,
			'      .Civilstate = basicData.CivilState,
			'      .Birthdate = basicData.Birthdate,
			'      .Language = basicData.Language,
			'      .DStellen = additionalData1.DStellen,
			'      .NoES = additionalData1.NoES,
			'      .Stat1 = additionalData1.KStat1,
			'      .Stat2 = additionalData1.KStat2,
			'      .Contact = additionalData1.KontaktHow,
			'      .ProfessionCode = additionalData1.ProfessionCode,
			'      .Profession = additionalData1.Profession,
			'      .QLand = additionalData1.QLand,
			'      .Permission = additionalData2.Permission,
			'      .PermissionToDate = additionalData2.PermissionToDate,
			'      .BirthPlace = additionalData2.BirthPlace,
			'      .S_Canton = additionalData2.S_Canton,
			'      .Residence = additionalData2.Residence,
			'      .ANS_QST_Bis = additionalData2.ANS_QST_Bis,
			'      .Q_Steuer = additionalData2.Q_Steuer,
			'      .ChurchTax = additionalData2.ChurchTax,
			'      .ChildsCount = additionalData2.ChildsCount,
			'      .QSTCommunity = additionalData2.QSTCommunity,
			'      .RahmenCheck = employeerahmenarbeitsvertrag,
			'      .NoZG = employeenozg,
			'      .NoLO = employeenolo,
			'      .Currency = currencyvalue,
			'      .Zahlart = employeezahlart,
			'      .BVGCode = employeebvgcode,
			'      .SecSuvaCode = employeesecsuvacode,
			'      .FerienBack = employeeferienback,
			'      .FeiertagBack = employeefeiertagback,
			'      .L13Back = employee13lohnback,
			'      .EmployeeNumberOffset = employeeNumberOffsetFromSettings,
			'      .MDNr = mandantAndAdvisorData.MandantData.MandantNumber,
			'      .UserKST = m_InitializationData.UserData.UserKST,
			'    .CreatedFrom = m_Common.GetLogedUserNameWithComma
			'      }

			'    Dim success As Boolean = m_EmployeeDataAccess.AddNewEmployee(newEmployeeInitData)

			'    If success Then
			'      NewlyAddedEmployeeNumber = newEmployeeInitData.IdNewEmployee
			'      DialogResult = DialogResult.OK

			'      ' Send a request to open a employeeMng form.
			'      Dim hub = MessageService.Instance.Hub
			'      Dim openCustomerMng As New OpenEmployeeMngRequest(Me, m_InitializationData.UserData.UserNr, SelectedMDNr, NewlyAddedEmployeeNumber)
			'      hub.Publish(openCustomerMng)

			'    Else
			'      NewlyAddedEmployeeNumber = Nothing
			'      DialogResult = DialogResult.Cancel
			'      m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Der Kandidat konnte nicht angelegt werden."))

			'    End If

			'    Close()

			'  End If

			'End If

		End Sub

    ''' <summary>
    ''' Reads the employee offset from the settings.
    ''' </summary>
    ''' <returns>Employee offset or zero if it could not be read.</returns>
    Private Function ReadEmployeeOffsetFromSettings() As Integer

      Dim strQuery As String = "//StartNr/Mitarbeiter"
      Dim r = m_ClsProgSetting.GetUserProfileFile
      Dim employeeNumberStartNumberSetting As String = m_ClsProgSetting.GetXMLValueByQuery(m_ClsProgSetting.GetMDData_XMLFile, strQuery, "0")
      Dim intVal As Integer

      If Integer.TryParse(employeeNumberStartNumberSetting, intVal) Then
        Return intVal
      Else
        Return 0
      End If

    End Function

    ''' <summary>
    ''' Handles click on cancel button.
    ''' </summary>
    Private Sub OnWizardCtrl_CancelClick(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles wizardCtrl.CancelClick
      DialogResult = DialogResult.Cancel
      Close()
    End Sub

#End Region

  End Class

End Namespace