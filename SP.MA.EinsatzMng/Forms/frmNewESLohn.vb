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

Public Class frmNewESLohn
  Implements INewESUserControlFormMediator

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
  ''' The common settings.
  ''' </summary>
  Private m_Common As CommonSetting

  ''' <summary>
  ''' The SPProgUtility object.
  ''' </summary>
  Protected m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath

  Private m_md As Mandant
  Private m_path As ClsProgPath

  ''' <summary>
  ''' The es number.
  ''' </summary>
  Private m_ESNumber As Integer

  ''' <summary>
  ''' The number of new newly created ESLohn.
  ''' </summary>
  Private m_NewEsLohnNr As Integer

  ''' <summary>
  ''' Candidate and customer data.
  ''' </summary>
  Private m_CandidateAndCustomerData As InitCandidateAndCustomerData

  ''' <summary>
  ''' ES Data.
  ''' </summary>
  Private m_SelectedESData As InitESData

  ''' <summary>
  ''' Boolan flag indicating if the form has been initialized.
  ''' </summary>
  Private m_IsInitialized = False


#End Region

#Region "Constructor"

  Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass,
                 ByVal esNumber As Integer,
                 ByVal candidateAndCustomerData As InitCandidateAndCustomerData,
                 ByVal selectedESData As InitESData)

    ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
    Try
      ' Mandantendaten
      m_md = New Mandant
      m_path = New ClsProgPath
      m_Common = New CommonSetting

      m_InitializationData = _setting
      m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

      m_ESNumber = esNumber
      m_CandidateAndCustomerData = candidateAndCustomerData
      m_SelectedESData = selectedESData

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

    m_ListOfPageControls.Add(ucESSalaryData)

    Dim conStr = m_InitializationData.MDData.MDDbConn

    m_ESDataAccess = New DatabaseAccess.ES.ESDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
    m_CommonDatabaseAccess = New DatabaseAccess.Common.CommonDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
    m_EmployeeDatabaseAccess = New DatabaseAccess.Employee.EmployeeDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
    m_CustomerDatabaseAccess = New DatabaseAccess.Customer.CustomerDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)

    ' Init sub controls with configuration information
    For Each ctrl In m_ListOfPageControls
      ctrl.InitWithConfigurationData(m_InitializationData, m_Translate)
    Next

    ucESSalaryData.UCMediator = Me

    m_UtilityUI = New UtilityUI
    m_Utility = New Utility

    ' Translate controls.
    TranslateControls()

    Reset()

    ucESSalaryData.ESNr = esNumber
    ucESSalaryData.ActivatePage()

  End Sub

#End Region

#Region "Public Properties"

  ''' <summary>
  ''' Gets the number of the newly created ESLohn.
  ''' </summary>
  Public ReadOnly Property NewESLohnNr As Integer?
    Get
      Return m_NewEsLohnNr
    End Get
  End Property

  ''' <summary>
  ''' Gets the selected candidate and customer data.
  ''' </summary>
  ''' <returns>Selected candidate and customer data.</returns>
  Public ReadOnly Property SelectedCandidateAndCustomerData As InitCandidateAndCustomerData Implements INewESUserControlFormMediator.SelectedCandidateAndCustomerData
    Get
      Return m_CandidateAndCustomerData
    End Get
  End Property

  ''' <summary>
  ''' Gets the selected ES data.
  ''' </summary>
  ''' <returns>Selected ES data.</returns>
  Public ReadOnly Property SelectedESData As InitESData Implements INewESUserControlFormMediator.SelectedESData
    Get
      Return m_SelectedESData
    End Get
  End Property

  ''' <summary>
  ''' Gets the ES salary data data.
  ''' </summary>
  ''' <returns>Selected ES salary data.</returns>
  Public ReadOnly Property SelectedSalaryData As InitESSalaryData Implements INewESUserControlFormMediator.SelectedSalaryData
    Get
      Return ucESSalaryData.SelectedESSalaryData
    End Get
  End Property

  ''' <summary>
  ''' Gets the common db access object.
  ''' </summary>
  Public ReadOnly Property CommonDbAccess As ICommonDatabaseAccess Implements INewESUserControlFormMediator.CommonDbAccess
    Get
      Return m_CommonDatabaseAccess
    End Get
  End Property

  ''' <summary>
  ''' Gets the ES db access object.
  ''' </summary>
  Public ReadOnly Property ESDbAccess As IESDatabaseAccess Implements INewESUserControlFormMediator.ESDbAccess
    Get
      Return m_ESDataAccess
    End Get
  End Property

  ''' <summary>
  ''' Gets the employee db access object.
  ''' </summary>
  Public ReadOnly Property EmployeeDbAccess As IEmployeeDatabaseAccess Implements INewESUserControlFormMediator.EmployeeDbAccess
    Get
      Return m_EmployeeDatabaseAccess
    End Get
  End Property

  ''' <summary>
  ''' Gets the customer db access object.
  ''' </summary> 
  Public ReadOnly Property CustomerDbAccess As ICustomerDatabaseAccess Implements INewESUserControlFormMediator.CustomerDbAccess
    Get
      Return m_CustomerDatabaseAccess
    End Get
  End Property

#End Region

#Region "Public Methods"

  ''' <summary>
  ''' Handles change of mandant, employee or customer.
  ''' </summary>
  Public Sub HandleChangeMandantEmployeeOrCustomer() Implements INewESUserControlFormMediator.HandleChangeMandantEmployeeOrCustomer
    ' Is not possible here.
  End Sub

  ''' <summary>
  ''' Handle finish click
  ''' </summary>
  Public Function HandleFinishClick() As Boolean Implements INewESUserControlFormMediator.HandleFinishClick
    ' Nothing to do here
    Return True
  End Function

  ''' <summary>
  ''' Handles change of mandant.
  ''' </summary>
  ''' <param name="mdNumber">The mandant number.</param>
  Public Sub HandleChageOfMandant(ByVal mdNumber As Integer) Implements INewESUserControlFormMediator.HandleChangeOfMandant
    ' Is not possible here.
  End Sub

  ''' <summary>
  ''' Validates all data.
  ''' </summary>
  Public Function ValidateData() As Boolean Implements INewESUserControlFormMediator.ValidateData
    Return ucESSalaryData.ValidateData()
  End Function

#End Region

#Region "Private Methods"

  ''' <summary>
  ''' Translates the controls
  ''' </summary>
  Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
		Me.btnAddESSalaryData.Text = m_Translate.GetSafeTranslationValue(Me.btnAddESSalaryData.Text)
		Me.btnCancel.Text = m_Translate.GetSafeTranslationValue(Me.btnCancel.Text)

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
  ''' Handles click on add slary data.
  ''' </summary>
  Private Sub OnBtnAddESSalaryData_Click(sender As System.Object, e As System.EventArgs) Handles btnAddESSalaryData.Click
    Dim success As Boolean = True

    If Not Me.ucESSalaryData.dateEditSalaryDataFrom.EditValue Is Nothing Then
      success = m_ESDataAccess.CheckIfLOVonDateCanBeSet(m_ESNumber, Nothing, Me.ucESSalaryData.dateEditSalaryDataFrom.EditValue)
      If Not success Then
				Dim result = m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Achtung, für diese Zeitperiode bestehen bereits erfasste Rapporte."),
																								 m_Translate.GetSafeTranslationValue("Einsatzlohn erfassen"), MessageBoxDefaultButton.Button2)
				If result = False Then Return Else success = True
      End If
    End If

    If success AndAlso ValidateData() Then

      Dim candidateAndCustomerData = SelectedCandidateAndCustomerData
      Dim esData = SelectedESData
      Dim salaryData = SelectedSalaryData

      Dim esDataSupport As New ESCreateService(candidateAndCustomerData.MandantData.MandantNumber, m_InitializationData)


      Dim newESLohnNr As Integer? = esDataSupport.CreateESLohn(m_ESNumber, candidateAndCustomerData, esData, salaryData)

      If Not newESLohnNr.HasValue Then
        DialogResult = DialogResult.Cancel
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Lohndaten konnten nicht angelegt werden."))
      Else
        DialogResult = DialogResult.OK
        m_NewEsLohnNr = newESLohnNr.Value

        ' Check if user whants to override tagespesen.
        If salaryData.DidUserConfirmOverrideOfTagespesen Then
          DeleteESSalaryRecordsThatDoNotMatchWithTagespesenOfNewlyEnteredESSalary()
        End If

				Close()
			End If


		End If

  End Sub

  ''' <summary>
  ''' Deletes ES salary records that do not match with tagespesen of newly enterd ES salary (if possible).
  ''' </summary>
  Private Sub DeleteESSalaryRecordsThatDoNotMatchWithTagespesenOfNewlyEnteredESSalary()
    Dim esSalaryData = m_ESDataAccess.LoadESSalaryData(m_ESNumber)

    If Not esSalaryData Is Nothing Then

      Dim newlyEnteredESSalary = esSalaryData.Where(Function(data) data.ESLohnNr = NewESLohnNr).FirstOrDefault

      For Each esSalary In esSalaryData

        ' Skip newly entered ESLohn
        If esSalary.ESLohnNr = NewESLohnNr Then
          Continue For
        End If

        If Not (esSalary.MATSpesen = newlyEnteredESSalary.MATSpesen And
                esSalary.KDTSpesen = newlyEnteredESSalary.KDTSpesen) Then

          ' Tagespen do not match with newly entered ESSalary -> try to delete this ESSalary if possible
          m_ESDataAccess.DeleteESSalaryData(esSalary.ID, ConstantValues.ModulName, m_ClsProgSetting.GetUserName(), m_InitializationData.UserData.UserNr)

        End If

      Next

    End If

  End Sub

  ''' <summary>
  ''' Handles click on cancel button.
  ''' </summary>
  Private Sub OnBtnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
    DialogResult = DialogResult.Cancel
    Close()
  End Sub

#End Region

End Class