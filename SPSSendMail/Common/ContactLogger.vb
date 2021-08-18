Imports SPProgUtility.Mandanten
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports System.Collections.Generic
Imports SPProgUtility.CommonXmlUtility
Imports SP.DatabaseAccess.Common

Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects.ContactMng

Public Class ContactLogger

#Region "Private Consts"

	Private Const MANDANT_XML_SETTING_CUSTOMER_CONTACT_SUBJECT As String = "MD_{0}/Templates/customer-contact-subject"
	Private Const MANDANT_XML_SETTING_CUSTOMER_FAX_CONTACT_SUBJECT As String = "MD_{0}/Templates/customer-fax-contact-subject"
	Private Const MANDANT_XML_SETTING_CUSTOMER_MAIL_CONTACT_SUBJECT As String = "MD_{0}/Templates/customer-mail-contact-subject"

	Private Const MANDANT_XML_SETTING_CUSTOMER_CONTACT_BODY As String = "MD_{0}/Templates/customer-contact-body"

#End Region

	Private m_Logger As ILogger = New Logger()

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
	''' The customer database access.
	''' </summary>
	Protected m_CustomerDatabaseAccess As ICustomerDatabaseAccess

	''' <summary>
	''' The employee data access object.
	''' </summary>
	Protected m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

	Protected m_UtilityUI As UtilityUI
	''' <summary>
	''' The mandant.
	''' </summary>
	Private m_MandantData As Mandant
	Private m_MandantSettingsXml As SettingsXml

	Private m_Body As String
	Private m_MailSubject As String
	Private m_SMSSubject As String
	Private m_FaxSubject As String

#Region "Constructor"

	Public Sub New(ByVal _Setting As InitializeClass)

		m_MandantData = New Mandant

		If _Setting.MDData Is Nothing Then
			ModulConstants.MDData = ModulConstants.SelectedMDData(0)
			ModulConstants.UserData = ModulConstants.LogededUSData(ModulConstants.MDData.MDNr, 0)

			ModulConstants.ProsonalizedData = ModulConstants.ProsonalizedValues
			ModulConstants.TranslationData = ModulConstants.TranslationValues

		Else
			ModulConstants.MDData = _Setting.MDData
			ModulConstants.UserData = _Setting.UserData
			ModulConstants.ProsonalizedData = _Setting.ProsonalizedData
			ModulConstants.TranslationData = _Setting.TranslationData

		End If


		m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ModulConstants.TranslationData, ModulConstants.ProsonalizedData, ModulConstants.MDData, ModulConstants.UserData)
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

		m_CommonDatabaseAccess = New SP.DatabaseAccess.Common.CommonDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_EmployeeDatabaseAccess = New SP.DatabaseAccess.Employee.EmployeeDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_CustomerDatabaseAccess = New SP.DatabaseAccess.Customer.CustomerDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

		Try

			Try
				m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(ModulConstants.MDData.MDNr, Now.Year))
			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			End Try

			m_SMSSubject = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_CUSTOMER_CONTACT_SUBJECT, ModulConstants.MDData.MDNr))
			Try
				m_FaxSubject = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_CUSTOMER_FAX_CONTACT_SUBJECT, ModulConstants.MDData.MDNr))
			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			End Try
			Try
				m_MailSubject = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_CUSTOMER_MAIL_CONTACT_SUBJECT, ModulConstants.MDData.MDNr))
			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			End Try

			Try
				m_Body = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_CUSTOMER_CONTACT_BODY, ModulConstants.MDData.MDNr))
			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			End Try

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		If String.IsNullOrWhiteSpace(m_SMSSubject) Then
			m_SMSSubject = "SMS Versand"
		End If

		If String.IsNullOrWhiteSpace(m_FaxSubject) Then
			m_FaxSubject = "Fax Versand"
		End If
		If String.IsNullOrWhiteSpace(m_MailSubject) Then
			m_MailSubject = "Mail Versand"
		End If

		If String.IsNullOrWhiteSpace(m_Body) Then
			m_Body = "Wurde an {0} eine {1}-Nachricht gesendet. {2}"
		End If

	End Sub
#End Region

#Region "Public Methods"

	''' <summary>
	''' Adds a responsible person contact data assignment.
	''' </summary>
	Public Function NewResponsiblePersonContact(ByVal CustomerNumber As Integer?,
												ByVal address As String,
												ByVal message As String,
												ByVal ResponsiblePersonNumber As Integer?,
												ByVal ContactDate As DateTime?,
												ByVal Username As String,
												ByVal RecordNumber As Integer?,
												ByVal CreatedOn As DateTime?,
												ByVal CreatedFrom As String,
												ByVal ChangedOn As DateTime?,
												ByVal ChangedFrom As String,
												ByVal ContactType1 As String,
												ByVal ContactType2 As Short?,
												ByVal IsImportant As Boolean?,
												ByVal IsFinished As Boolean?,
												ByVal IsSMS As Boolean) As Boolean

		'Dim Contact As String = String.Format(m_Body, address, message, IIf(IsSMS, "SMS", "Fax").ToString())
		Dim Contact As String = String.Format(m_Body, address, ContactType1, message)

		Dim Period As String = IIf(IsSMS, m_SMSSubject, m_FaxSubject).ToString()
		If ContactType1.Contains("SMS") Then
			Period = m_SMSSubject
		ElseIf ContactType1.Contains("Fax") Then
			Period = m_FaxSubject
		Else
			Period = m_MailSubject
		End If


		Dim automatedBodyContent = Contact

		Dim contactData = New ResponsiblePersonAssignedContactData With {.CustomerNumber = CustomerNumber}
		contactData.ResponsiblePersonNumber = ResponsiblePersonNumber
		contactData.ContactDate = Now
		contactData.Username = m_InitializationData.UserData.UserFullName
		contactData.CreatedOn = Now
		contactData.CreatedFrom = m_InitializationData.UserData.UserFullName
		contactData.ContactType1 = ContactType1
		contactData.ContactType2 = ContactType2
		contactData.ContactPeriodString = m_Translate.GetSafeTranslationValue(Period)
		contactData.ContactImportant = IsImportant
		contactData.ContactFinished = IsFinished
		contactData.MANr = Nothing
		contactData.CreatedUserNumber = m_InitializationData.UserData.UserNr
		contactData.ProposeNr = Nothing
		contactData.VacancyNumber = Nothing
		contactData.OfNumber = Nothing
		contactData.Mail_ID = Nothing
		contactData.TaskRecNr = Nothing
		contactData.UsNr = m_InitializationData.UserData.UserNr
		contactData.ESNr = Nothing
		contactData.KontaktDocID = Nothing
		contactData.EmployeeContactRecID = Nothing
		contactData.ContactsString = String.Format(automatedBodyContent, address, contactData.ContactType1, String.Empty)

		Dim success = m_CustomerDatabaseAccess.AddResponsiblePersonContactAssignment(contactData)
		If Not success Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kontakt Daten konnten nicht gespeichert werden."))
		End If


		Return success

	End Function

	''' <summary>
	'''  Adds a employee contact.
	''' </summary>
	Public Function NewEmployeeContact(ByVal MANr As Integer, ByVal address As String, ByVal message As String,
								   ByVal ContactType1 As String, ByVal ContactType2 As Short?,
								   ByVal ContactDate As DateTime?,
								   ByVal IsImportant As Boolean?, ByVal IsFinished As Boolean?,
								   ByVal CreatedOn As DateTime?, ByVal CreatedFrom As String) As Boolean

		Dim Contact As String = String.Format(m_Body, address, ContactType1, message)
		Dim Period As String = String.Empty
		If ContactType1.Contains("SMS") Then
			Period = m_SMSSubject
		ElseIf ContactType1.Contains("Fax") Then
			Period = m_FaxSubject
		Else
			Period = m_MailSubject
		End If

		Dim automatedBodyContent = Contact

		Dim contactData = New EmployeeContactData With {.EmployeeNumber = MANr}

		contactData.CustomerNumber = Nothing
		contactData.ContactDate = Now
		contactData.ContactType1 = ContactType1
		contactData.ContactType2 = ContactType2
		contactData.ContactPeriodString = m_Translate.GetSafeTranslationValue(Period)
		contactData.ContactImportant = IsImportant
		contactData.ContactFinished = IsFinished

		contactData.ProposeNr = Nothing
		contactData.VacancyNumber = Nothing
		contactData.OfNumber = Nothing
		contactData.Mail_ID = Nothing
		contactData.TaskRecNr = Nothing
		contactData.UsNr = m_InitializationData.UserData.UserNr
		contactData.ESNr = Nothing
		contactData.CustomerContactRecId = Nothing
		contactData.KontaktDocID = Nothing
		contactData.CreatedOn = Now
		contactData.CreatedFrom = m_InitializationData.UserData.UserFullName
		contactData.CreatedUserNumber = m_InitializationData.UserData.UserNr
		contactData.ContactsString = String.Format(automatedBodyContent, address, contactData.ContactType1, String.Empty)

		Dim success = m_EmployeeDatabaseAccess.AddEmployeeContact(contactData)
		If Not success Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kontakt Daten konnten nicht gespeichert werden."))
		End If

		Return success

	End Function




	'Dim success As Boolean = True

	'	Dim sql As String = "[Create New MA_Kontakt]"

	'	' Parameters

	'	Dim listOfParams As New List(Of SqlClient.SqlParameter)
	'	listOfParams.Add(New SqlClient.SqlParameter("@MANr", ReplaceMissing(MANr, DBNull.Value)))
	'	listOfParams.Add(New SqlClient.SqlParameter("@KDNr", DBNull.Value))
	'	listOfParams.Add(New SqlClient.SqlParameter("@Kontakte", ReplaceMissing(Contact, DBNull.Value)))
	'	listOfParams.Add(New SqlClient.SqlParameter("@KontaktType1", ReplaceMissing(ContactType1, DBNull.Value)))
	'	listOfParams.Add(New SqlClient.SqlParameter("@KontaktType2", ReplaceMissing(ContactType2, DBNull.Value)))
	'	listOfParams.Add(New SqlClient.SqlParameter("@KontaktDate", ReplaceMissing(ContactDate, DBNull.Value)))
	'	listOfParams.Add(New SqlClient.SqlParameter("@KontaktDauer", ReplaceMissing(Period, DBNull.Value)))
	'	listOfParams.Add(New SqlClient.SqlParameter("@KontaktWichtig", ReplaceMissing(IsImportant, DBNull.Value)))
	'	listOfParams.Add(New SqlClient.SqlParameter("@KontaktErledigt", ReplaceMissing(IsFinished, DBNull.Value)))
	'	listOfParams.Add(New SqlClient.SqlParameter("@CreatedOn", ReplaceMissing(CreatedOn, DBNull.Value)))
	'	listOfParams.Add(New SqlClient.SqlParameter("@CreatedFrom", ReplaceMissing(CreatedFrom, DBNull.Value)))
	'	listOfParams.Add(New SqlClient.SqlParameter("@ChangedOn", ReplaceMissing(ChangedOn, DBNull.Value)))
	'	listOfParams.Add(New SqlClient.SqlParameter("@ChangedFrom", ReplaceMissing(ChangedFrom, DBNull.Value)))
	'	listOfParams.Add(New SqlClient.SqlParameter("@ProposeNr", ReplaceMissing(ProposeNr, DBNull.Value)))
	'	listOfParams.Add(New SqlClient.SqlParameter("@VakNr", ReplaceMissing(VacancyNumber, DBNull.Value)))
	'	listOfParams.Add(New SqlClient.SqlParameter("@OfNr", ReplaceMissing(OfNumber, DBNull.Value)))
	'	listOfParams.Add(New SqlClient.SqlParameter("@Mail_ID", ReplaceMissing(Mail_ID, DBNull.Value)))
	'	listOfParams.Add(New SqlClient.SqlParameter("@TaskRecNr", ReplaceMissing(TaskRecNr, DBNull.Value)))
	'	listOfParams.Add(New SqlClient.SqlParameter("@USNr", ReplaceMissing(UsNr, DBNull.Value)))
	'	listOfParams.Add(New SqlClient.SqlParameter("@ESNr", ReplaceMissing(ESNr, DBNull.Value)))
	'	listOfParams.Add(New SqlClient.SqlParameter("@KDKontaktRecID", DBNull.Value))
	'	listOfParams.Add(New SqlClient.SqlParameter("@KontaktDocID", DBNull.Value))

	'	Dim newIdParameter As New SqlClient.SqlParameter("@NewContactID", SqlDbType.Int)
	'	newIdParameter.Direction = ParameterDirection.Output
	'	listOfParams.Add(newIdParameter)

	'	Dim recNrParameter As New SqlClient.SqlParameter("@RecNr ", SqlDbType.Int)
	'	recNrParameter.Direction = ParameterDirection.Output
	'	listOfParams.Add(recNrParameter)

	'	Dim connection As SqlClient.SqlConnection = New SqlClient.SqlConnection(ModulConstants.MDData.MDDbConn)
	'	Try
	'		connection.Open()

	'		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sql, connection)
	'		cmd.CommandType = CommandType.StoredProcedure

	'		For Each param As SqlClient.SqlParameter In listOfParams
	'			cmd.Parameters.Add(param)
	'		Next

	'		cmd.ExecuteNonQuery()

	'	Catch e As Exception
	'		success = False
	'		m_Logger.LogError(e.ToString())

	'	Finally
	'		connection.Close()
	'		connection.Dispose()

	'	End Try

	'	Return success

	'End Function

	''' <summary>
	''' Replaces a missing object with another object.
	''' </summary>
	''' <param name="obj">The object.</param>
	''' <param name="replacementObject">The replacement object.</param>
	''' <returns>The object or the replacement object it the object is nothing.</returns>
	Protected Shared Function ReplaceMissing(ByVal obj As Object, ByVal replacementObject As Object) As Object

		If (obj Is Nothing) Then
			Return replacementObject
		Else
			Return obj
		End If

	End Function

#End Region

End Class
