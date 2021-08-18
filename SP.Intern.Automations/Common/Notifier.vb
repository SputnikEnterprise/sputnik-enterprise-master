
Imports System.ComponentModel
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Common

Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SPProgUtility.CommonXmlUtility
Imports SPProgUtility.Mandanten
Imports SP.Infrastructure.Initialization
Imports System.Threading.Tasks
Imports SPProgUtility.ProgPath

Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Applicant
Imports SP.DatabaseAccess.Applicant.DataObjects
Imports SP.DatabaseAccess.Employee.DataObjects.LanguagesAndProfessionsMng
Imports SP.DatabaseAccess.Employee.DataObjects.ContactMng
Imports SP.DatabaseAccess.EMailJob.DataObjects
Imports SP.Internal.Automations
Imports SP.DatabaseAccess.Employee.DataObjects.TodoMng
Imports SP.DatabaseAccess.Propose.DataObjects

Namespace Notifying

	Public Class Notifier

#Region "Private fields"

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
		''' The employee data access object.
		''' </summary>
		Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI

		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As Infrastructure.Utility

		''' <summary>
		''' The path.
		''' </summary>
		Private m_path As ClsProgPath

		''' <summary>
		''' The common database access.
		''' </summary>
		Protected m_CommonDatabaseAccess As ICommonDatabaseAccess

		''' <summary>
		''' Settings xml.
		''' </summary>
		Private m_MandantSettingsXml As SettingsXml

		''' <summary>
		''' The mandant.
		''' </summary>
		Private m_MandantData As Mandant
		Private m_connectionString As String

		Private m_ImportedApplicantData As BindingList(Of ApplicantData)



#End Region


#Region "public property"

		Public Property CustomerID As String

		Public ReadOnly Property ImportedApplicantData As BindingList(Of ApplicantData)
			Get
				Return m_ImportedApplicantData
			End Get
		End Property


#End Region


#Region "Constructor"

		Sub New(ByVal _setting As InitializeClass)

			m_InitializationData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

			m_MandantData = New Mandant
			m_UtilityUI = New UtilityUI
			m_Utility = New Infrastructure.Utility
			m_path = New ClsProgPath

			m_connectionString = m_InitializationData.MDData.MDDbConn
			m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

			m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))

		End Sub

#End Region


#Region "Public Methodes"

		Public Function AddNewNotifierForSystemMessages(ByVal customerID As String, ByVal data As NotifyData) As Boolean
			Dim success As Boolean = True
			Dim todoData = New TodoData With {.UserNumber = m_InitializationData.UserData.UserNr}

			Select Case data.NotifyArt
				Case NotifyData.NotifyEnum.SYSTEMUPDATE
					todoData.TODOSourceEnum = TODOEnum.SYSTEM_UPDATE
				Case NotifyData.NotifyEnum.SCANFILEINFO
					todoData.TODOSourceEnum = TODOEnum.SYSTEM_SCANFILEINFO
				Case NotifyData.NotifyEnum.SCANERROR
					todoData.TODOSourceEnum = TODOEnum.SYSTEM_SCANERROR

				Case Else
					todoData.TODOSourceEnum = TODOEnum.SYSTEM_UPDATE
			End Select

			todoData.Subject = String.Format("{0}", data.NotifyHeader)
			todoData.Body = String.Format("Meldung: {1}{0}{2}", vbNewLine, data.ID, data.NotifyComments)
			todoData.AllUsers = True

			todoData.IsImportant = True
			todoData.IsCompleted = False
			todoData.Schedulebegins = data.CreatedOn
			todoData.Scheduleends = data.CreatedOn
			todoData.AllUsers = True

			todoData.CreatedOn = data.CreatedOn
			todoData.CreatedFrom = "System"

			success = success AndAlso AddNewNotifierForSystemNotifications(customerID, todoData, data.ID)


			Return success

		End Function

		Public Function AddNewNotifierForVacancyChangedOnlineState(ByVal customerID As String, ByVal vacancyNumber As Integer?, ByVal userNumber As Integer?) As Boolean
			Dim success As Boolean = True
			Dim todoData = New TodoData With {.UserNumber = userNumber}

			todoData.VacancyNumber = vacancyNumber

			todoData.TODOSourceEnum = TODOEnum.VACANCY_ONLINE_STATE_CHANGE
			todoData.Subject = "Änderung der Vakanz-Online Status"
			todoData.Body = String.Format("Vakanz mit der Nummer {0} wurde Online-Status geändert.", vacancyNumber)
			todoData.IsImportant = True
			todoData.IsCompleted = False
			todoData.Schedulebegins = DateTime.Now
			todoData.Scheduleends = DateTime.Now

			If (todoData Is Nothing) Then
				todoData.CreatedOn = DateTime.Now
				todoData.CreatedFrom = "System"
			Else
				todoData.ChangedOn = DateTime.Now
				todoData.ChangedFrom = "System"
			End If

			success = success AndAlso m_EmployeeDatabaseAccess.InsertTodoData(customerID, todoData)


			Return success

		End Function

		Public Function AddNewNotifierForProposeResult(ByVal customerID As String, ByVal proposeData As ProposeMasterData, ByVal body As String, ByVal userNumbers As List(Of Integer)) As Boolean
			Dim success As Boolean = True
			Dim todoData = New TodoData
			Dim todoUserData = New TodoUserData
			Dim createdNewToDo As Boolean = True

			Try

				userNumbers = userNumbers.GroupBy(Function(m) m).Where(Function(g) g.Count() >= 1).Select(Function(g) g.Key).ToList

				'Dim existsTODOData = m_EmployeeDatabaseAccess.LoadTodoDataForAutoCreatedNotify(customerID, userNumbers(0), 2, proposeData.ProposeNr, body)
				'If Not existsTODOData Is Nothing AndAlso existsTODOData.Count > 0 Then

				'	For Each todo In existsTODOData
				'		Dim existsTodoUserData = m_EmployeeDatabaseAccess.LoadTodoUserData(todo.ID)
				'		If existsTodoUserData Is Nothing Then Exit For

				'		For Each usnr In userNumbers
				'			Dim data = existsTodoUserData.Where(Function(x) x.UserNumber = usnr).FirstOrDefault
				'			If data Is Nothing Then
				'				todoUserData = New TodoUserData

				'				todoUserData.UserNumber = usnr
				'				todoUserData.Customer_ID = customerID
				'				todoUserData.FK_ToDoID = todo.ID
				'				todoUserData.CreatedFrom = "System"

				'				success = success AndAlso m_EmployeeDatabaseAccess.InsertTodoUserData(customerID, todoUserData)
				'			End If

				'			Return True
				'		Next

				'	Next

				'End If

				todoData.UserNumber = userNumbers(0)
				todoData.ProposeNumber = proposeData.ProposeNr
				todoData.EmployeeNumber = proposeData.MANr
				todoData.CustomerNumber = proposeData.KDNr


				todoData.TODOSourceEnum = TODOEnum.PROPOSE_UPDATED
				todoData.Subject = String.Format("Vorschlag: ({0})", proposeData.Bezeichnung)
				'todoData.Body = String.Format("Vorschlag mit der Nummer {1} wurde Online geändert.{0}{2}", vbNewLine, proposeData.ProposeNr, body)
				todoData.Body = String.Format("{0}", body)
				todoData.IsImportant = True
				todoData.IsCompleted = False
				todoData.Schedulebegins = DateTime.Now
				todoData.Scheduleends = DateTime.Now
				todoData.AllUsers = False

				'If (todoData Is Nothing) Then
				todoData.CreatedOn = DateTime.Now
				todoData.CreatedFrom = "System"
				'Else
				'	todoData.ChangedOn = DateTime.Now
				'	todoData.ChangedFrom = "System"
				'End If

				success = success AndAlso m_EmployeeDatabaseAccess.InsertTodoData(customerID, todoData)
				Dim i As Integer = 1

				If userNumbers.Count > 1 Then
					If userNumbers(i) <> userNumbers(0) Then
						todoUserData.UserNumber = userNumbers(i)
						todoUserData.Customer_ID = customerID
						todoUserData.FK_ToDoID = todoData.ID
						todoUserData.CreatedFrom = "System"

						success = success AndAlso m_EmployeeDatabaseAccess.InsertTodoUserData(customerID, todoUserData)
					End If

					i += 1
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))

				Return False
			End Try


			Return success

		End Function

#End Region


#Region "private methodes"

		Private Function AddNewNotifierForSystemNotifications(ByVal customerID As String, ByVal data As TodoData, ByVal notifyID As Integer) As Boolean
			Dim success As Boolean = True
			Dim todoData = New TodoData
			Dim todoUserData = New TodoUserData
			Dim createdNewToDo As Boolean = True
			Dim bodyToSearch As String = String.Format("Meldung: {0}", notifyID)

			Dim existsTODOData = m_EmployeeDatabaseAccess.LoadTodoDataForAutoCreatedNotify(customerID, String.Empty, 0, 0, bodyToSearch)
			If Not existsTODOData Is Nothing AndAlso existsTODOData.Count > 0 Then Return True

			todoData = data
			todoData.UserNumber = 1

			success = success AndAlso m_EmployeeDatabaseAccess.InsertTodoData(customerID, todoData)


			Return success

		End Function

#End Region


	End Class


End Namespace
