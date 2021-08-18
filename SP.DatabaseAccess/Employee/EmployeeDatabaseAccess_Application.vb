
Imports SP.DatabaseAccess.Employee.DataObjects.AdvancedPaymentMng
Imports System.Text
Imports SP.DatabaseAccess.Applicant.DataObjects
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng

Namespace Employee

	Partial Public Class EmployeeDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IEmployeeDatabaseAccess


		''' <summary>
		''' Loads application data for assigned applicant.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		Function LoadAssignedEmployeeApplications(ByVal customer_ID As String, ByVal employeeNumber As Integer, ByVal applicationNumber As Integer?, ByVal onlyActive As Boolean?) As IEnumerable(Of ApplicationData) Implements IEmployeeDatabaseAccess.LoadAssignedEmployeeApplications

			Dim result As List(Of ApplicationData) = Nothing

			Dim sql As String = String.Empty

			sql = "SELECT [ID]"
			sql &= ",[Customer_ID]"
			sql &= ",[ApplicationLabel]"
			sql &= ",[EmployeeID]"
			sql &= ",[VacancyNumber]"
			sql &= ",IsNull((Select Top 1 KDNr From Vakanzen Where VakNr = tbl_Application.VacancyNumber), 0) CustomerNumber"
			sql &= ",IsNull((Select Top 1 Nachname From Benutzer Where Transfered_Guid = tbl_Application.Advisor), '') Lastname"
			sql &= ",IsNull((Select Top 1 Vorname From Benutzer Where Transfered_Guid = tbl_Application.Advisor), '') Firstname"
			sql &= ",[Advisor]"
			sql &= ",[BusinessBranch]"
			sql &= ",[Dismissalperiod]"
			sql &= ",[Availability]"
			sql &= ",[Comment]"
			sql &= ",[CreatedOn]"
			sql &= ",[CreatedFrom]"
			sql &= ",[CheckedOn]"
			sql &= ",[CheckedFrom]"
			sql &= ",[ApplicationLifecycle]"

			sql &= " From dbo.tbl_Application"
			sql &= " Where"
			sql &= " (isnull(@Customer_ID, '') = '' OR Customer_ID = @Customer_ID)"
			sql &= " AND EmployeeID = @employeeID"

			sql &= " AND (@applcationNumber = 0 Or ID = @applcationNumber)"
			If onlyActive Then sql &= " AND IsNull(ApplicationLifecycle, 0) Not In (1, 5, 6)"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("Customer_ID", ReplaceMissing(customer_ID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("employeeID", ReplaceMissing(employeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@applcationNumber", ReplaceMissing(applicationNumber, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If Not reader Is Nothing Then

					result = New List(Of ApplicationData)

					While reader.Read

						Dim applicationData As New ApplicationData

						applicationData.ID = SafeGetInteger(reader, "ID", 0)
						applicationData.Customer_ID = SafeGetString(reader, "Customer_ID")
						applicationData.ApplicationLabel = SafeGetString(reader, "ApplicationLabel")
						applicationData.EmployeeID = SafeGetInteger(reader, "EmployeeID", Nothing)
						applicationData.VacancyNumber = SafeGetInteger(reader, "VacancyNumber", Nothing)
						applicationData.CustomerNumber = SafeGetInteger(reader, "CustomerNumber", Nothing)
						applicationData.Advisor = SafeGetString(reader, "Advisor")
						applicationData.BusinessBranch = SafeGetString(reader, "BusinessBranch")
						applicationData.Dismissalperiod = SafeGetString(reader, "Dismissalperiod")
						applicationData.Availability = SafeGetString(reader, "Availability")
						applicationData.Comment = SafeGetString(reader, "Comment")
						applicationData.AdvisorLastname = SafeGetString(reader, "Lastname")
						applicationData.AdvisorFirstname = SafeGetString(reader, "Firstname")

						applicationData.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						applicationData.CreatedFrom = SafeGetString(reader, "createdFrom")
						applicationData.CheckedOn = SafeGetDateTime(reader, "CheckedOn", Nothing)
						applicationData.CheckedFrom = SafeGetString(reader, "CheckedFrom")
						applicationData.ApplicationLifecycle = SafeGetInteger(reader, "ApplicationLifecycle", Nothing)


						result.Add(applicationData)

					End While

				End If


			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				result = Nothing

			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		''' <summary>
		''' Updates applicant data to employee.
		''' </summary>
		''' <remarks></remarks>
		Function UpdateApplicantToEmployee(ByVal employeeMasterData As EmployeeMasterData) As Boolean Implements IEmployeeDatabaseAccess.UpdateApplicantToEmployee

			Dim success = True

			Dim sql As String

			sql = "UPDATE Mitarbeiter SET "
			sql &= "ShowAsApplicant = @ShowAsApplicant, "
			sql &= "ChangedOn = GetDate(), "
			sql &= "ChangedFrom = @ChangedFrom "
			sql &= "WHERE MANr = @MANr "

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("MANr", employeeMasterData.EmployeeNumber))
			listOfParams.Add(New SqlClient.SqlParameter("ShowAsApplicant", employeeMasterData.ShowAsApplicant))
			listOfParams.Add(New SqlClient.SqlParameter("ChangedFrom", employeeMasterData.ChangedFrom))

			success = ExecuteNonQuery(sql, listOfParams)


			Return success

		End Function

		''' <summary>
		''' Updates applicant flag data.
		''' </summary>
		''' <remarks></remarks>
		Function UpdateApplicantFlagData(ByVal employeeMasterData As EmployeeMasterData) As Boolean Implements IEmployeeDatabaseAccess.UpdateApplicantFlagData

			Dim success = True

			Dim sql As String

			sql = "UPDATE Mitarbeiter SET "
			sql &= "ApplicantLifecycle = @ApplicantLifecycle, "
			sql &= "ChangedOn = GetDate(), "
			sql &= "ChangedFrom = @ChangedFrom "
			sql &= "WHERE MANr = @MANr "

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("MANr", employeeMasterData.EmployeeNumber))
			listOfParams.Add(New SqlClient.SqlParameter("ApplicantLifecycle", employeeMasterData.ShowAsApplicant))
			listOfParams.Add(New SqlClient.SqlParameter("ChangedFrom", employeeMasterData.ChangedFrom))

			success = ExecuteNonQuery(sql, listOfParams)


			Return success

		End Function


	End Class

End Namespace

