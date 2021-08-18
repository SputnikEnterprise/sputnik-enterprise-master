Imports SP.DatabaseAccess.ES.DataObjects.ESMng
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages

Namespace UI

	''' <summary>
	''' Mediator to commuicate between user controls.
	''' </summary>
	Public Class UserControlFormMediator

#Region "Private Fields"

		Private m_ESFrm As frmES
		Private m_ucCandidateAndCustomer As ucCandidateAndCustomer
		Private m_ucEsData As ucESData
		Private m_ucSalary As ucSalaryData
		Private m_ucAdditionalInfoFields As ucAdditionalInfoFields
		Private m_ucAdditionalSalaryTypes As ucAdditionalSalaryTypes
		Private m_ucKostenteilung As ucKostenteilung

#End Region

#Region "Constructor"

		''' <summary>
		''' The constructor.
		''' </summary>
		''' <param name="frmEs">The es form.</param>
		''' <param name="candidateAndCustomer">The candidate and customer control.</param>
		Public Sub New(ByVal frmEs As frmES,
					   ByVal candidateAndCustomer As ucCandidateAndCustomer,
					   ByVal esData As ucESData,
					   ByVal salaryData As ucSalaryData,
					   ByVal additionalInfoFields As ucAdditionalInfoFields,
					   ByVal additionalSalaryTypes As ucAdditionalSalaryTypes,
					   ByVal kostenteilung As ucKostenteilung)

			Me.m_ESFrm = frmEs
			Me.m_ucCandidateAndCustomer = candidateAndCustomer
			Me.m_ucEsData = esData
			Me.m_ucSalary = salaryData
			Me.m_ucAdditionalInfoFields = additionalInfoFields
			Me.m_ucAdditionalSalaryTypes = additionalSalaryTypes
			Me.m_ucKostenteilung = kostenteilung

			m_ucCandidateAndCustomer.UCMediator = Me
			m_ucEsData.UCMediator = Me
			m_ucSalary.UCMediator = Me
			m_ucAdditionalInfoFields.UCMediator = Me
			m_ucAdditionalSalaryTypes.UCMediator = Me
			m_ucKostenteilung.UCMediator = Me

		End Sub

#End Region

#Region "Public Methods"

		''' <summary>
		''' Loads LA data.
		''' </summary>
		''' <param name="year">The year</param>
		Public Sub LoadLAData(ByVal year As Integer)
			m_ucAdditionalSalaryTypes.LoadLAData(year)
		End Sub

		''' <summary>
		''' Loads GAV detail data from ES salary data.
		''' </summary>
		''' <param name="esSalaryData">The ES salary data.</param>
		Public Sub LoadGAVDetailData(ByVal esSalaryData As ESSalaryData)
			m_ucKostenteilung.LoadGAVDetailData(esSalaryData)
		End Sub

		''' <summary>
		''' Rereshes Data after ES save.
		''' </summary>
		'''<param name="esData">The ES master data.</param>
		Public Sub RefreshDataAfterESSave(ByVal esData As ESMasterData)
			m_ucEsData.RefreshDataAfterESSave(esData)
		End Sub

		''' <param name="usNr">The The user number.</param>
		''' <param name="mdNr">The mandant number.</param>
		''' <param name="esNr">The ES number.</param>
		Public Sub SendESDataChangedNotification(ByVal usNr As Integer,
					   ByVal mdNr As Integer,
					   ByVal esNr As Integer)
			' Send a notification for changed ES data.
			Dim hub = MessageService.Instance.Hub
			Dim esDataChangedNotification As New ESDataHasChanged(Me, usNr, mdNr, esNr)
			hub.Publish(esDataChangedNotification)
		End Sub

		''' <summary>
		''' Refreshes Verleih- and Einsatzvertrag printed data.
		''' </summary>
		''' <param name="esData">The ES master data.</param>
		Public Sub RefreshVerleihAndEinsatzVertragPrintedData(ByVal esData As ESMasterData)
			m_ucEsData.RefreshVerleihAndEinsatzVertragPrintedData(esData)
		End Sub

		''' <summary>
		''' Refreshes the additional salary data()
		''' </summary>
		Public Sub RefreshAdditionalSalaryData()
			m_ucAdditionalSalaryTypes.RefreshData()
		End Sub

		''' <summary>
		''' Checks GAV validity of ES salary data.
		''' </summary>
		''' <param name="esLohn">The ES Salary data.</param>
		Public Sub CheckGAVValidityOfESSalaryData(ByVal esLohn As ESSalaryData)
			m_ESFrm.CheckGAVValidityOfESSalaryData(esLohn)
		End Sub

		''' <summary>
		'''  Checks the GAV validity of the selected ES salary data.
		''' </summary>
		Public Sub CheckGAVValidityOfSelectedESSalaryData()
			m_ESFrm.CheckGAVValidityOfSelectedESSalaryData()
		End Sub

#End Region

#Region "Properties"

		''' <summary>
		''' Gets the mandant number.
		''' </summary>
		''' <returns>The employee number.</returns>
		Public ReadOnly Property MandantNumber As Integer
			Get
				Return m_ucCandidateAndCustomer.MandantNumber
			End Get
		End Property

		''' <summary>
		''' Gets the employee number.
		''' </summary>
		''' <returns>The employee number.</returns>
		Public ReadOnly Property EmployeeNumber As Integer
			Get
				Return m_ucCandidateAndCustomer.EmployeeNumber
			End Get
		End Property

		''' <summary>
		''' Gets the customer number.
		''' </summary>
		''' <returns>The customer number.</returns>
		Public ReadOnly Property CustomerNumber As Integer
			Get
				Return m_ucCandidateAndCustomer.CustomerNumber
			End Get
		End Property

		''' <summary>
		''' Gets the customer responsible person number.
		''' </summary>
		''' <returns>The customer responsible person number.</returns>
		Public ReadOnly Property CustomerResponsibleNumber As Integer?
			Get
				Return m_ucCandidateAndCustomer.CustomerResponsibleNumber
			End Get
		End Property

		''' <summary>
		''' Gets the selected es salary data on the salary data tab.
		''' </summary>
		''' <returns>The selected es salary data.</returns>
		Public ReadOnly Property SelectedESSalaryOnSalaryDataTab As ESSalaryData
			Get
				Return m_ucSalary.SelectedESSalaryData
			End Get
		End Property

		''' <summary>
		''' Gets the selected ESEnde date.
		''' </summary>
		''' <returns>The selected ES Ende data.</returns>
		Public ReadOnly Property SelectedESEnde As DateTime?
			Get
				Return m_ucEsData.dateEditEndDate.EditValue
			End Get
		End Property

#End Region

	End Class

End Namespace