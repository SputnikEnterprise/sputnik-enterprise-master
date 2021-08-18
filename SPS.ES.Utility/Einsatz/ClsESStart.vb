

Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.ES
Imports SP.DatabaseAccess.Report
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Internal.Automations.WOSUtility.DataObjects
Imports SPProgUtility.Mandanten

Public Class ClsESStart


#Region "private fields"

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Protected m_UtilityUI As UtilityUI

	Protected m_Utility As SP.Infrastructure.Utility


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
	Protected m_EmployeeDataAccess As IEmployeeDatabaseAccess

	''' <summary>
	''' The data access object.
	''' </summary>
	Protected m_CustomerDataAccess As ICustomerDatabaseAccess

	''' <summary>
	''' The data access object.
	''' </summary>
	Protected m_ESDataAccess As IESDatabaseAccess

	''' <summary>
	''' The data access object.
	''' </summary>
	Protected m_ReportDataAccess As IReportDatabaseAccess

	''' <summary>
	''' The mandant.
	''' </summary>
	Private m_md As Mandant
	Private m_EmployeeTransferedGuid As String
	Private m_CustomerTransferedGuid As String
	Private m_EmployeeNumber As Integer
	Private m_CustomerNumber As Integer
	Private m_EmploymentNumber As Integer

#End Region


#Region "public properties"

	Public Property ESSetting As New ClsESDataSetting
	Public Property ESTemplateGuid As String
	Public Property ESVerleihTemplateGuid As String
	Public Property ReportScanGuids As New List(Of String)

#End Region

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		m_UtilityUI = New SP.Infrastructure.UI.UtilityUI
		m_Utility = New SP.Infrastructure.Utility
		m_md = New Mandant

		m_InitializationData = _setting

		Dim conStr = m_InitializationData.MDData.MDDbConn
		m_EmployeeDataAccess = New EmployeeDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_CustomerDataAccess = New CustomerDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_ESDataAccess = New ESDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_ReportDataAccess = New ReportDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)


	End Sub

	Public Function DeleteDocFrom_WS(ByVal employeeNumber As Integer, ByVal customerNumber As Integer, ByVal employmentNumber As Integer) As Boolean
		Dim result As Boolean = True

		m_EmployeeNumber = employeeNumber
		m_CustomerNumber = customerNumber
		m_EmploymentNumber = employmentNumber

		Try
			Dim employeeData = m_EmployeeDataAccess.LoadEmployeeMasterData(m_EmployeeNumber, False)
			Dim customerData = m_CustomerDataAccess.LoadCustomerMasterData(m_CustomerNumber, "%%")
			Dim reportListData = m_ESDataAccess.LoadExistingReportNumbersForES(m_EmploymentNumber)

			m_EmployeeTransferedGuid = employeeData.Transfered_Guid
			m_CustomerTransferedGuid = customerData.Transfered_Guid

			' those reports are allready deleted with deleting report!!!
			'For Each rpNumber In reportListData
			'	For Each guid In ReportScanGuids
			'		DeleteEmployeeReportDocumentFromWOS(rpNumber, guid)
			'	Next
			'Next
			If Not String.IsNullOrWhiteSpace(ESTemplateGuid) Then
				DeleteEmployeeESDocumentFromWOS(ESTemplateGuid)
			End If
			If Not String.IsNullOrWhiteSpace(ESVerleihTemplateGuid) Then
				DeleteCustomerVerleihtDocumentFromWOS(ESVerleihTemplateGuid)
			End If


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		End Try

		Return result
	End Function

	Private Function DeleteEmployeeReportDocumentFromWOS(ByVal reportNumber As Integer, ByVal scanDocGuid As String) As Boolean
		Dim result As Boolean = True
		Dim wos = New SP.Internal.Automations.WOSUtility.EmployeeExport(ModulConstants.m_InitialData)
		Dim wosSetting = New WOSSendSetting

		wosSetting.EmployeeNumber = m_EmployeeNumber
		wosSetting.EmployeeGuid = m_EmployeeTransferedGuid
		wosSetting.DocumentArtEnum = WOSSendSetting.DocumentArt.Rapport
		wosSetting.ReportDocumentNumber = reportNumber
		wosSetting.AssignedDocumentGuid = scanDocGuid
		wosSetting.DocumentInfo = String.Format("Rapport: {0}", reportNumber)

		wos.WOSSetting = wosSetting

		result = result AndAlso wos.DeleteTransferedEmployeeDocument()


		Return result

	End Function

	Private Function DeleteEmployeeESDocumentFromWOS(ByVal scanDocGuid As String) As Boolean
		Dim result As Boolean = True
		Dim wos = New SP.Internal.Automations.WOSUtility.EmployeeExport(ModulConstants.m_InitialData)
		Dim wosSetting = New WOSSendSetting

		wosSetting.EmployeeNumber = m_EmployeeNumber
		wosSetting.EmployeeGuid = m_EmployeeTransferedGuid
		wosSetting.DocumentArtEnum = WOSSendSetting.DocumentArt.Einsatzvertrag
		wosSetting.EmploymentNumber = m_EmploymentNumber
		wosSetting.AssignedDocumentGuid = scanDocGuid
		wosSetting.DocumentInfo = String.Format("Einsatzvertrag: {0}", m_EmploymentNumber)

		wos.WOSSetting = wosSetting

		result = result AndAlso wos.DeleteTransferedEmployeeDocument()


		Return result

	End Function

	Private Function DeleteCustomerReportDocumentFromWOS(ByVal reportNumber As Integer, ByVal scanDocGuid As String) As Boolean
		Dim result As Boolean = True
		Dim wos = New SP.Internal.Automations.WOSUtility.CustomerExport(ModulConstants.m_InitialData)
		Dim wosSetting = New WOSSendSetting

		wosSetting.CustomerNumber = m_CustomerNumber
		wosSetting.CustomerGuid = m_CustomerTransferedGuid
		wosSetting.DocumentArtEnum = WOSSendSetting.DocumentArt.Rapport
		wosSetting.ReportDocumentNumber = reportNumber
		wosSetting.AssignedDocumentGuid = scanDocGuid
		wosSetting.DocumentInfo = String.Format("Rapport: {0}", reportNumber)

		wos.WOSSetting = wosSetting

		result = result AndAlso wos.DeleteTransferedCustomerDocument()


		Return result

	End Function

	Private Function DeleteCustomerVerleihtDocumentFromWOS(ByVal scanDocGuid As String) As Boolean
		Dim result As Boolean = True
		Dim wos = New SP.Internal.Automations.WOSUtility.CustomerExport(ModulConstants.m_InitialData)
		Dim wosSetting = New WOSSendSetting

		wosSetting.CustomerNumber = m_CustomerNumber
		wosSetting.CustomerGuid = m_CustomerTransferedGuid
		wosSetting.DocumentArtEnum = WOSSendSetting.DocumentArt.Verleihvertrag
		wosSetting.EmploymentNumber = m_EmploymentNumber
		wosSetting.AssignedDocumentGuid = scanDocGuid
		wosSetting.DocumentInfo = String.Format("Verleihvertrag: {0}", m_EmploymentNumber)

		wos.WOSSetting = wosSetting

		result = result AndAlso wos.DeleteTransferedCustomerDocument()


		Return result

	End Function

End Class
