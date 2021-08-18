
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Applicant.DataObjects
Imports System.Text
'Imports System.Transactions


Namespace Applicant



	Partial Class AppDatabaseAccess

		Inherits DatabaseAccessBase
		Implements IAppDatabaseAccess


		''' <summary>
		''' Adds a application with applicant data.
		''' </summary>
		Function AddApplicationWithApplicant(ByVal application As ApplicationData, ByVal applicant As ApplicantData) As Boolean Implements IAppDatabaseAccess.AddApplicationWithApplicant

			Dim success = True

			Dim sql As String

			sql = "[Create New Application With Applicant]"

			' Parameters

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("Customer_ID", ReplaceMissing(application.Customer_ID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("VacancyNumber", ReplaceMissing(application.VacancyNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ApplicationLabel", ReplaceMissing(application.ApplicationLabel, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("BusinessBranch", ReplaceMissing(application.BusinessBranch, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Advisor", ReplaceMissing(application.Advisor, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Dismissalperiod", ReplaceMissing(application.Dismissalperiod, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Availability", ReplaceMissing(application.Availability, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Comment", ReplaceMissing(application.Comment, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("Lastname", ReplaceMissing(applicant.Lastname, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Firstname", ReplaceMissing(applicant.Firstname, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Gender", ReplaceMissing(applicant.Gender, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Street", ReplaceMissing(applicant.Street, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("PostOfficeBox", ReplaceMissing(applicant.PostOfficeBox, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Postcode", ReplaceMissing(applicant.Postcode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Location", ReplaceMissing(applicant.Location, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Country", ReplaceMissing(applicant.Country, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Nationality", ReplaceMissing(applicant.Nationality, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("EMail", ReplaceMissing(applicant.EMail, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Telephone", ReplaceMissing(applicant.Telephone, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MobilePhone", ReplaceMissing(applicant.MobilePhone, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Birthdate", ReplaceMissing(applicant.Birthdate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Permission", ReplaceMissing(applicant.Permission, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Profession", ReplaceMissing(applicant.Profession, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Auto", ReplaceMissing(applicant.Auto, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Motorcycle", ReplaceMissing(applicant.Motorcycle, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Bicycle", ReplaceMissing(applicant.Bicycle, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DrivingLicence1", ReplaceMissing(applicant.DrivingLicence1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DrivingLicence2", ReplaceMissing(applicant.DrivingLicence2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DrivingLicence3", ReplaceMissing(applicant.DrivingLicence3, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CivilState", ReplaceMissing(applicant.CivilState, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Language", ReplaceMissing(applicant.Language, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("LanguageLevel", ReplaceMissing(applicant.LanguageLevel, DBNull.Value)))


			Dim newApplicationIdParameter = New SqlClient.SqlParameter("@NewApplicationId", SqlDbType.Int)
			newApplicationIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newApplicationIdParameter)

			Dim applicantIDParameter = New SqlClient.SqlParameter("@NewApplicantId", SqlDbType.Int)
			applicantIDParameter.Direction = ParameterDirection.Output
			listOfParams.Add(applicantIDParameter)


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If Not newApplicationIdParameter.Value Is Nothing AndAlso Not applicantIDParameter.Value Is Nothing Then
				application.ID = CType(newApplicationIdParameter.Value, Integer)
				application.FK_ApplicantID = CType(applicantIDParameter.Value, Integer)
				applicant.ID = CType(applicantIDParameter.Value, Integer)
			Else
				success = False
			End If

			Return success

		End Function

		''' <summary>
		''' Adds a application data.
		''' </summary>
		Function AddApplication(ByVal application As ApplicationData, ByVal applicant As ApplicantData) As Boolean Implements IAppDatabaseAccess.AddApplication

			Dim success = True

			Dim sql As String

			sql = "[Create New Application]"

			' Parameters

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("Customer_ID", ReplaceMissing(application.Customer_ID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("FK_ApplicantID", ReplaceMissing(applicant.ID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("EmployeeID", ReplaceMissing(applicant.EmployeeID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("VacancyNumber", ReplaceMissing(application.VacancyNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ApplicationLabel", ReplaceMissing(application.ApplicationLabel, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("BusinessBranch", ReplaceMissing(application.BusinessBranch, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Advisor", ReplaceMissing(application.Advisor, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Dismissalperiod", ReplaceMissing(application.Dismissalperiod, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Availability", ReplaceMissing(application.Availability, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Comment", ReplaceMissing(application.Comment, DBNull.Value)))

			Dim newIdParameter = New SqlClient.SqlParameter("@NewId", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If Not newIdParameter.Value Is Nothing Then
				application.ID = CType(newIdParameter.Value, Integer)
			Else
				success = False
			End If

			Return success

		End Function

		Function AddProfileDataForNotValidatedCVLData(ByVal applicantID As Integer, ByVal applicationID As Integer) As Boolean Implements IAppDatabaseAccess.AddProfileDataForNotValidatedCVLData

			Dim success = True

			Dim sql As String

			sql = "[Create New CVLProfile For Not Validated CVL Data]"

			' Parameters

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("applicationID", ReplaceMissing(applicationID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("applicantID", ReplaceMissing(applicantID, DBNull.Value)))

			Dim newIdParameter = New SqlClient.SqlParameter("@NewCVLId", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If Not newIdParameter.Value Is Nothing Then
				'application.ID = CType(newIdParameter.Value, Integer)
			Else
				success = False
			End If

			Return success

		End Function

		''' <summary>
		''' Adds a document data.
		''' </summary>
		Function AddApplicantDocument(ByVal document As ApplicantDocumentData) As Boolean Implements IAppDatabaseAccess.AddApplicantDocument

			Dim success = True

			Dim sql As String

			sql = "[Create New Applicant Document]"

			' Parameters

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("FK_ApplicantID", ReplaceMissing(document.FK_ApplicantID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Type", ReplaceMissing(document.Type, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Flag", ReplaceMissing(document.Flag, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Title", ReplaceMissing(document.Title, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("FileExtension", ReplaceMissing(document.FileExtension, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Content", ReplaceMissing(document.Content, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("HashValue", ReplaceMissing(document.HashValue, DBNull.Value)))


			Dim newIdParameter = New SqlClient.SqlParameter("@NewId", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If Not newIdParameter.Value Is Nothing Then
				document.ID = CType(newIdParameter.Value, Integer)
			Else
				success = False
			End If

			Return success

		End Function

		''' <summary>
		''' Loads all application Data
		''' </summary>
		''' <returns>The user data.</returns>
		Function LoadApplicationData(ByVal customerID As String, ByVal businessBranch As String) As IEnumerable(Of ApplicationData) Implements IAppDatabaseAccess.LoadApplicationData

			Dim result As List(Of ApplicationData) = Nothing

			Dim sql As String


			sql = "SELECT A.ID"
			sql &= ",A.[Customer_ID]"
			sql &= ",A.[FK_ApplicantID]"
			sql &= ",A.[VacancyNumber]"
			sql &= ",A.[BusinessBranch]"
			sql &= ",A.[Advisor]"
			sql &= ",A.[Dismissalperiod]"
			sql &= ",A.[Availability]"
			sql &= ",A.[Comment]"
			sql &= ",A.CreatedOn"
			sql &= ",A.CreatedFrom"
			sql &= ",A.[CheckedOn]"
			sql &= ",A.[CheckedFrom]"
			sql &= ",A.[ApplicationLifecycle]"

			sql &= " FROM [dbo].[tbl_application] A "
			sql &= " Left Join [spSystemInfo].dbo.[tbl_CustomerSetting] C On A.Customer_ID = C.Customer_ID "
			sql &= " Where (A.Customer_ID = @CustomerID)"
			sql &= " And (@BusinessBranch = '' Or A.BusinessBranch = @BusinessBranch)"
			sql &= " Order By A.ID Desc"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("BusinessBranch", ReplaceMissing(businessBranch, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ApplicationData)

					While reader.Read

						Dim data = New ApplicationData()

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.Customer_ID = SafeGetString(reader, "Customer_ID")
						data.FK_ApplicantID = SafeGetInteger(reader, "FK_ApplicantID", 0)
						data.VacancyNumber = SafeGetInteger(reader, "VacancyNumber", 0)
						data.BusinessBranch = SafeGetString(reader, "BusinessBranch")
						data.Advisor = SafeGetString(reader, "Advisor")
						data.Dismissalperiod = SafeGetString(reader, "Dismissalperiod")

						data.Availability = SafeGetString(reader, "Availability")
						data.Comment = SafeGetString(reader, "Comment")

						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						data.CheckedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.CheckedFrom = SafeGetString(reader, "CreatedFrom")
						data.ApplicationLifecycle = SafeGetInteger(reader, "ApplicationLifecycle", 0)


						result.Add(data)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		''' <summary>
		''' Loads all assigned applicant applications Data
		''' </summary>
		''' <returns>The user data.</returns>
		Function LoadAssignedApplicantApplications(ByVal customerID As String, ByVal applicantID As Integer) As IEnumerable(Of ApplicationData) Implements IAppDatabaseAccess.LoadAssignedApplicantApplications

			Dim result As List(Of ApplicationData) = Nothing

			Dim sql As String


			sql = "SELECT A.ID"
			sql &= ",A.[Customer_ID]"
			sql &= ",A.[FK_ApplicantID]"
			sql &= ",A.[VacancyNumber]"
			sql &= ",A.[ApplicationLabel]"
			sql &= ",A.[BusinessBranch]"
			sql &= ",b.Vorname AdvisorFirstName"
			sql &= ",b.Nachname AdvisorLastName"
			sql &= ",A.[Dismissalperiod]"
			sql &= ",A.[Availability]"
			sql &= ",A.[Comment]"
			sql &= ",A.CreatedOn"
			sql &= ",A.CreatedFrom"
			sql &= ",A.[ApplicationLifecycle]"

			sql &= " FROM [dbo].[tbl_application] A "
			sql &= " Left Join Benutzer b On A.Advisor = b.Transfered_Guid "
			sql &= " Where (A.Customer_ID = @CustomerID)"
			sql &= " And (A.FK_ApplicantID = @applicantID)"
			sql &= " Order By A.CreatedOn Desc"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("applicantID", ReplaceMissing(applicantID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ApplicationData)

					While reader.Read

						Dim data = New ApplicationData()
						Dim AdvisorLastname As String
						Dim AdvisorFirstname As String

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.Customer_ID = SafeGetString(reader, "Customer_ID")
						data.FK_ApplicantID = SafeGetInteger(reader, "FK_ApplicantID", 0)
						data.ApplicationLabel = SafeGetString(reader, "ApplicationLabel")
						data.VacancyNumber = SafeGetInteger(reader, "VacancyNumber", 0)
						data.BusinessBranch = SafeGetString(reader, "BusinessBranch")

						AdvisorFirstname = SafeGetString(reader, "AdvisorFirstName")
						AdvisorLastname = SafeGetString(reader, "AdvisorLastName")
						data.Advisor = String.Format("{0} {1}", AdvisorFirstname, AdvisorLastname)

						data.Dismissalperiod = SafeGetString(reader, "Dismissalperiod")

						data.Availability = SafeGetString(reader, "Availability")
						data.Comment = SafeGetString(reader, "Comment")

						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						data.ApplicationLifecycle = SafeGetInteger(reader, "ApplicationLifecycle", 0)


						result.Add(data)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		Function UpdateNewApplicationWithExistingApplicantData(ByVal customerID As String, ByVal applicationID As Integer, ByVal applicantID As Integer, ByVal hashValues As String) As Boolean Implements IAppDatabaseAccess.UpdateNewApplicationWithExistingApplicantData

			Dim success = True

			Dim sql As String
			' TODO: use SQL function
			sql = "Update [tbl_application] Set "
			sql &= " FK_ApplicantID = @fk_ApplicantID "
			sql &= " Where customer_ID = @customerID And ID = @applicantID"
			sql &= "Update tbl_ApplicantDocument Set FK_ApplicantID = @FK_ApplicantID"
			sql &= "And FileHash in (; "


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("fk_ApplicantID", ReplaceMissing(applicationID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("applicantID", ReplaceMissing(applicantID, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)


			Return success

		End Function

		Function DeleteAssignedApplication(ByVal customerID As String, ByVal applicationID As Integer) As Boolean Implements IAppDatabaseAccess.DeleteAssignedApplication

			Dim success = True

			Dim sql As String


			sql = "DELETE [tbl_application]"
			sql &= " Where Customer_ID = @CustomerID"
			sql &= " AND ID = @ID ; "


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", customerID))
			listOfParams.Add(New SqlClient.SqlParameter("ID", ReplaceMissing(applicationID, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)


			Return success

		End Function

		Function DeleteAllrelatedApplicantData(ByVal customerID As String, ByVal applicantID As Integer) As Boolean Implements IAppDatabaseAccess.DeleteAllrelatedApplicantData

			Dim success = True

			Dim sql As String


			sql = "DELETE [tbl_application]"
			sql &= " Where Customer_ID = @CustomerID"
			sql &= " AND FK_ApplicantID = @applicantID ; "

			sql &= "DELETE [tbl_applicant]"
			sql &= " Where Customer_ID = @CustomerID"
			sql &= " AND ID = @applicantID;"

			sql &= "DELETE [tbl_applicant_Document]"
			sql &= " Where Customer_ID = @CustomerID"
			sql &= " AND FK_ApplicantID = @applicantID"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", customerID))
			listOfParams.Add(New SqlClient.SqlParameter("applicantID", ReplaceMissing(applicantID, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)


			Return success

		End Function


#Region "Mainview"

		Function LoadAssignedApplicationDataForMainView(ByVal recID As Integer) As MainViewApplicationData Implements IAppDatabaseAccess.LoadAssignedApplicationDataForMainView
			Dim result As MainViewApplicationData = Nothing

			Dim sql As String

			sql = "[List Assigned Application Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("ID", ReplaceMissing(recID, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New MainViewApplicationData

					While reader.Read()
						Dim overviewData As New MainViewApplicationData

						overviewData.ID = SafeGetInteger(reader, "ID", 0)
						overviewData.ApplicationID = SafeGetInteger(reader, "ApplicationID", 0)
						overviewData.Customer_ID = SafeGetString(reader, "Customer_ID")
						overviewData.ApplicationLabel = SafeGetString(reader, "ApplicationLabel")
						overviewData.Advisor = SafeGetString(reader, "Advisor")
						overviewData.BusinessBranch = SafeGetString(reader, "BusinessBranch")

						overviewData.Dismissalperiod = SafeGetString(reader, "Dismissalperiod")

						overviewData.Availability = SafeGetString(reader, "Availability")
						overviewData.Comment = SafeGetString(reader, "Comment")
						overviewData.ApplicantLastname = SafeGetString(reader, "ApplicantLastname")
						overviewData.ApplicantFirstname = SafeGetString(reader, "ApplicantFirstname")
						overviewData.Birthdate = SafeGetDateTime(reader, "Birthday", Nothing)
						overviewData.ApplicantStreet = SafeGetString(reader, "ApplicantStreet")
						overviewData.ApplicantPostcode = SafeGetString(reader, "ApplicantPostcode")
						overviewData.ApplicantLocation = SafeGetString(reader, "ApplicantLocation")
						overviewData.ApplicantCountry = SafeGetString(reader, "ApplicantCountry")
						'overviewData.CVLProfileID = SafeGetInteger(reader, "CVLProfileID", 0)
						overviewData.Customername = SafeGetString(reader, "Customername")
						overviewData.CustomerStreet = SafeGetString(reader, "CustomerStreet")
						overviewData.CustomerPostcode = SafeGetString(reader, "CustomerPostcode")
						overviewData.CustomerLocation = SafeGetString(reader, "CustomerLocation")
						overviewData.CustomerCountry = SafeGetString(reader, "CustomerCountry")
						overviewData.VacancyLable = SafeGetString(reader, "VacancyLabel")
						overviewData.ApplicationAdvisorLastName = SafeGetString(reader, "ApplicationAdvisorLastName")
						overviewData.ApplicationAdvisorFirstName = SafeGetString(reader, "ApplicationAdvisorFirstName")
						overviewData.ApplicationMandantName = SafeGetString(reader, "ApplicationMandantName")

						overviewData.MDNr = SafeGetInteger(reader, "MDNr", 0)
						overviewData.EmployeeID = SafeGetInteger(reader, "EmployeeID", 0)
						overviewData.VacancyNumber = SafeGetInteger(reader, "VacancyNumber", 0)
						overviewData.Customernumber = SafeGetInteger(reader, "Customernumber", 0)

						overviewData.ApplicationLifecycle = SafeGetInteger(reader, "ApplicationLifecycle", 0)
						overviewData.ApplicantLifecycle = SafeGetInteger(reader, "ApplicantLifecycle", 0)

						overviewData.CheckedFrom = SafeGetString(reader, "Checkedfrom")
						overviewData.CheckedOn = SafeGetDateTime(reader, "Checkedon", Nothing)
						overviewData.CreatedFrom = SafeGetString(reader, "Createdfrom")
						overviewData.CreatedOn = SafeGetDateTime(reader, "Createdon", Nothing)

						overviewData.zfiliale = SafeGetString(reader, "zfiliale")


						result = overviewData

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)

			End Try

			Return result

		End Function

		Function UpdateApplicationWithAdvisorData(ByVal data As ApplicationData) As Boolean Implements IAppDatabaseAccess.UpdateApplicationWithAdvisorData

			Dim success = True

			Dim sql As String

			sql = "Update [tbl_application] Set "
			sql &= " ApplicationLabel =  @ApplicationLabel, "
			sql &= " Advisor =  @advisor, "
			sql &= " Businessbranch =  @businessbranch, "
			sql &= " CheckedOn = GetDate(), "
			sql &= " CheckedFrom = @checkedfrom, "
			sql &= " ApplicationLifecycle = @lifecycle "

			sql &= " Where ID = @ID "



			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("ApplicationLabel", ReplaceMissing(data.ApplicationLabel, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("advisor", ReplaceMissing(data.Advisor, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("businessbranch", ReplaceMissing(data.BusinessBranch, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("checkedfrom", ReplaceMissing(data.CheckedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("lifecycle", ReplaceMissing(data.ApplicationLifecycle, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ID", ReplaceMissing(data.ID, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)


			Return success

		End Function

		Function UpdateMainViewApplicationWithAdvisorData(ByVal data As MainViewApplicationData) As Boolean Implements IAppDatabaseAccess.UpdateMainViewApplicationWithAdvisorData

			Dim success = True

			Dim sql As String

			sql = "Update [tbl_application] Set "
			sql &= " ApplicationLabel =  @ApplicationLabel, "
			sql &= " Advisor =  @advisor, "
			sql &= " Businessbranch =  @businessbranch, "
			sql &= " CheckedOn = GetDate(), "
			sql &= " CheckedFrom = @checkedfrom, "
			sql &= " ApplicationLifecycle = @lifecycle "

			sql &= " Where ID = @ID "



			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("ApplicationLabel", ReplaceMissing(data.ApplicationLabel, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("advisor", ReplaceMissing(data.Advisor, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("businessbranch", ReplaceMissing(data.BusinessBranch, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("checkedfrom", ReplaceMissing(data.CheckedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("lifecycle", ReplaceMissing(data.ApplicationLifecycle, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ID", ReplaceMissing(data.ID, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)


			Return success

		End Function

		Function UpdateAllApplicantApplicationFlagData(ByVal data As MainViewApplicationData) As Boolean Implements IAppDatabaseAccess.UpdateAllApplicantApplicationFlagData

			Dim success = True

			Dim sql As String

			sql = "Update [tbl_Application] Set "
			sql &= " CheckedOn = GetDate(), "
			sql &= " CheckedFrom = @checkedfrom, "
			sql &= " ApplicationLifecycle = @lifecycle "

			sql &= " Where EmployeeID = @EmployeeID "
			sql &= " And ApplicationLifecycle In (0, 2, 3, 4) "


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("checkedfrom", ReplaceMissing(data.CheckedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("lifecycle", ReplaceMissing(data.ApplicationLifecycle, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("EmployeeID", ReplaceMissing(data.EmployeeID, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)


			Return success

		End Function

		Function UpdateApplicationLabelData(ByVal data As MainViewApplicationData) As Boolean Implements IAppDatabaseAccess.UpdateApplicationLabelData

			Dim success = True

			Dim sql As String

			sql = "Update [tbl_application] Set "
			sql &= " ApplicationLabel =  @ApplicationLabel "

			sql &= " Where ID = @ID "



			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("ApplicationLabel", ReplaceMissing(data.ApplicationLabel, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("advisor", ReplaceMissing(data.Advisor, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("businessbranch", ReplaceMissing(data.BusinessBranch, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("checkedfrom", ReplaceMissing(data.CheckedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("lifecycle", ReplaceMissing(data.ApplicationLifecycle, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ID", ReplaceMissing(data.ID, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)


			Return success

		End Function

#End Region


	End Class

End Namespace
