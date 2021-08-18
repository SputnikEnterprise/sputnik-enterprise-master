Imports SP.DatabaseAccess.Employee.DataObjects.ContactMng
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects.BankMng
Imports SP.DatabaseAccess.Employee.DataObjects.JobInterviewMng

Namespace Employee

  Partial Public Class EmployeeDatabaseAccess
    Inherits DatabaseAccessBase
    Implements IEmployeeDatabaseAccess

    ''' <summary>
    ''' Loads employee job interview data (MA_JobTermin).
    ''' </summary>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <param name="interviewRecorNumber">Optional job interview record number.</param>
    ''' <returns>Employee job interview data or nothing in error case.</returns>
    Function LoadEmployeeJobInterviews(ByVal employeeNumber As Integer, Optional ByVal interviewRecorNumber As Integer? = Nothing) As IEnumerable(Of EmployeeJobAppointmentData) Implements IEmployeeDatabaseAccess.LoadEmployeeJobInterviews
      Dim result As List(Of EmployeeJobAppointmentData) = Nothing

      Dim sql As String

      sql = "SELECT * FROM MA_JobTermin WHERE MANr = @maNumber AND (@interviewRecordNumber IS NULL OR  RecNr = @interviewRecordNumber) ORDER BY TerminDate Desc"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("@maNumber", employeeNumber))
      listOfParams.Add(New SqlClient.SqlParameter("@interviewRecordNumber", ReplaceMissing(interviewRecorNumber, DBNull.Value)))


      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of EmployeeJobAppointmentData)

          While reader.Read()
            Dim interviewData As New EmployeeJobAppointmentData
            interviewData.ID = SafeGetInteger(reader, "ID", 0)
            interviewData.RecordNumber = SafeGetInteger(reader, "RecNr", Nothing)
            interviewData.JobTitle = SafeGetString(reader, "JobTitel", Nothing)
            interviewData.EmployeeNumber = SafeGetInteger(reader, "MANr", Nothing)
            interviewData.AppointmentDate = SafeGetDateTime(reader, "TerminDate", Nothing)
            interviewData.CustomerNumber = SafeGetInteger(reader, "KDNr", Nothing)
            interviewData.Company = SafeGetString(reader, "Firma", Nothing)
            interviewData.AppointmentWith = SafeGetString(reader, "TerminWith", Nothing)
            interviewData.Location = SafeGetString(reader, "Ort", Nothing)
            interviewData.Telephone = SafeGetString(reader, "Telefon", Nothing)
            interviewData.Telefax = SafeGetString(reader, "Telefax", Nothing)
            interviewData.Homepage = SafeGetString(reader, "Homepage", Nothing)
            interviewData.eMail = SafeGetString(reader, "eMail", Nothing)
            interviewData.Outcome = SafeGetString(reader, "Ergebnis", Nothing)
            interviewData.JobAppointmentState = SafeGetString(reader, "JobTerminStatus", Nothing)
            interviewData.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
            interviewData.CreatedFrom = SafeGetString(reader, "CreatedFrom", Nothing)
            interviewData.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
            interviewData.ChangedFrom = SafeGetString(reader, "ChangedFrom", Nothing)
            interviewData.Result = SafeGetString(reader, "Result", Nothing)
            interviewData.ProposeNr = SafeGetInteger(reader, "ProposeNr", Nothing)
            interviewData.VakNr = SafeGetInteger(reader, "VakNr", Nothing)
            interviewData.OfNr = SafeGetInteger(reader, "OfNr", Nothing)
            interviewData.ResponsiblePersonNumber = SafeGetInteger(reader, "KDZHDNr", Nothing)

            result.Add(interviewData)

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
    ''' Load customer data for job interview management.
    ''' </summary>
    ''' <returns>List of customer data or nothing in error case.</returns>
    Function LoadCustomerDataForJobInterviewMng() As IEnumerable(Of Employee.DataObjects.JobInterviewMng.CustomerData) Implements IEmployeeDatabaseAccess.LoadCustomerDataForJobInterviewMng
      Dim result As List(Of Employee.DataObjects.JobInterviewMng.CustomerData) = Nothing

      Dim sql As String

      sql = "SELECT KDNr, Firma1, PLZ, Ort FROM Kunden ORDER BY Firma1 Asc"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of Employee.DataObjects.JobInterviewMng.CustomerData)

          While reader.Read()
            Dim customerData As New Employee.DataObjects.JobInterviewMng.CustomerData
            customerData.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)
            customerData.Company = SafeGetString(reader, "Firma1", Nothing)
            customerData.Postcode = SafeGetString(reader, "PLZ", Nothing)
            customerData.Location = SafeGetString(reader, "Ort", Nothing)

            result.Add(customerData)

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
    ''' Loads responsible person data for job interview management.
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <returns>List of responsible person data or nothing in error case.</returns>
    Function LoadResponsiblePersonDataForJobInterviewMng(ByVal customerNumber As Integer) As IEnumerable(Of Employee.DataObjects.JobInterviewMng.ResponsiblePersonData) Implements IEmployeeDatabaseAccess.LoadResponsiblePersonDataForJobInterviewMng

      Dim result As List(Of Employee.DataObjects.JobInterviewMng.ResponsiblePersonData) = Nothing

      Dim sql As String
			Dim lang = MapLanguageToShortLanguageCode(SelectedTranslationLanguage)

			sql = "SELECT ZHD.*, "
			sql &= String.Format("IsNull((SELECT TOP 1 Bez_{0} FROM Tab_ZHDState1 t WHERE t.Bezeichnung = ZHD.KDZState1), '') AS TranslatedState1, ", lang)
			sql &= String.Format("IsNull((SELECT TOP 1 Bez_{0} FROM Tab_ZHDState2 t WHERE t.Bezeichnung = ZHD.KDZState2), '') AS TranslatedState2, ", lang)
			sql &= String.Format("IsNull((SELECT TOP 1 Bez_{0} FROM Tab_ZHDKontakt t WHERE t.Bezeichnung = ZHD.KDZHowKontakt), '') AS TranslatedHowKontakt, ", lang)
			sql &= String.Format("IsNull((SELECT TOP 1 Anrede_{0} FROM Anrede ANR WHERE ANR.Anrede = ZHD.Anrede), '') AS TranslatedAnrede, ", lang)

			sql &= " ZHD.Nachname, ZHD.Vorname, ZHD.PLZ, ZHD.Ort, ZHD.Telefon, ZHD.Telefax, ZHD.Email, KD.Homepage "
			sql &= " FROM KD_Zustaendig ZHD LEFT JOIN Kunden KD ON ZHD.KDNr = KD.KDNr "
			sql &= " WHERE ZHD.KDNr = @customerNumber "
			sql &= " ORDER BY ZHD.Nachname Asc, ZHD.Vorname Asc"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("@customerNumber", customerNumber))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of Employee.DataObjects.JobInterviewMng.ResponsiblePersonData)

          While reader.Read()
            Dim responsiblePersonData As New Employee.DataObjects.JobInterviewMng.ResponsiblePersonData
            responsiblePersonData.ID = SafeGetInteger(reader, "ID", 0)
            responsiblePersonData.RecordNumber = SafeGetInteger(reader, "RecNr", Nothing)
            responsiblePersonData.CustomerNumber = SafeGetInteger(reader, "KDNr", Nothing)
            responsiblePersonData.TranslatedAnrede = SafeGetString(reader, "TranslatedAnrede", Nothing)
            responsiblePersonData.Lastname = SafeGetString(reader, "Nachname", Nothing)
            responsiblePersonData.Firstname = SafeGetString(reader, "Vorname", Nothing)
            responsiblePersonData.Postcode = SafeGetString(reader, "PLZ", Nothing)
            responsiblePersonData.Location = SafeGetString(reader, "Ort", Nothing)
            responsiblePersonData.Telephone = SafeGetString(reader, "Telefon", Nothing)
            responsiblePersonData.Telefax = SafeGetString(reader, "Telefax", Nothing)
            responsiblePersonData.EMail = SafeGetString(reader, "Email", Nothing)
            responsiblePersonData.Homepage = SafeGetString(reader, "Homepage", Nothing)

						responsiblePersonData.TranslatedZState1 = SafeGetString(reader, "TranslatedState1")
						responsiblePersonData.TranslatedZState2 = SafeGetString(reader, "TranslatedState2")
						responsiblePersonData.TranslatedZHowKontakt = SafeGetString(reader, "TranslatedHowKontakt")

						responsiblePersonData.ZState1 = SafeGetString(reader, "KDZState1")
						responsiblePersonData.ZState2 = SafeGetString(reader, "KDZState2")

						responsiblePersonData.TranslatedSalutation = SafeGetString(reader, "TranslatedAnrede")

            result.Add(responsiblePersonData)

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
    ''' Loads job appointment state date for job intervie management.
    ''' </summary>
    ''' <returns>List of job interview state data or nothing in error case.</returns>
    Function LoadJobAppointmentStateDataeForJobInterviewMng() As IEnumerable(Of Employee.DataObjects.JobInterviewMng.JobAppointmentStateData) Implements IEmployeeDatabaseAccess.LoadJobAppointmentStateDataeForJobInterviewMng

      Dim result As List(Of Employee.DataObjects.JobInterviewMng.JobAppointmentStateData) = Nothing

      Dim sql As String

      sql = "SELECT ID, Bezeichnung, Result FROM Tab_JobTerminStatus ORDER BY Bezeichnung Asc"

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of Employee.DataObjects.JobInterviewMng.JobAppointmentStateData)

          While reader.Read()
            Dim appointmentStateData As New Employee.DataObjects.JobInterviewMng.JobAppointmentStateData
            appointmentStateData.ID = SafeGetInteger(reader, "ID", 0)
            appointmentStateData.Description = SafeGetString(reader, "Bezeichnung", Nothing)
            appointmentStateData.Result = SafeGetString(reader, "Result", Nothing)

            result.Add(appointmentStateData)

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
    ''' Loads vacancy data.
    ''' </summary>
    ''' <returns>List of vacancy data.</returns>
    Function LoadVacancyDataForJobInterviewMng(ByVal customerNumber As Integer) As IEnumerable(Of Employee.DataObjects.JobInterviewMng.VacancyData) Implements IEmployeeDatabaseAccess.LoadVacancyDataForJobInterviewMng

      Dim result As List(Of Employee.DataObjects.JobInterviewMng.VacancyData) = Nothing

      Dim sql As String

			sql = "SELECT ID, KDNr, KDZHDNr, VakNr, Bezeichnung, CreatedOn, CreatedFrom, "
			sql &= "("
			sql &= "CASE  "
			sql &= " When isnumeric(V.VakState) = 1 then (Select Top 1 bez_d From tbl_base_VakState where RecValue = V.VakState) "
			sql &= " ELSE VakState "
			sql &= " End "
			sql &= ") as Vakstate "
			sql &= " FROM Vakanzen V WHERE KDNr = @customerNumber ORDER BY VakNr ASC"

      Dim customerNumberParameter As New SqlClient.SqlParameter("customerNumber", customerNumber)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(customerNumberParameter)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of Employee.DataObjects.JobInterviewMng.VacancyData)

          While reader.Read

            Dim vacancyData = New Employee.DataObjects.JobInterviewMng.VacancyData()
            vacancyData.ID = SafeGetInteger(reader, "ID", 0)
            vacancyData.VacancyNumber = SafeGetInteger(reader, "VakNr", Nothing)
            vacancyData.Description = SafeGetString(reader, "Bezeichnung")

						vacancyData.VakState = SafeGetString(reader, "VakState")

						vacancyData.CreatedOn = SafeGetDateTime(reader, "createdon", Nothing)
						vacancyData.CreatedFrom = SafeGetString(reader, "CreatedFrom")

            result.Add(vacancyData)

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
    ''' Loads propose data.
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <returns>List of propose data.</returns>
    Function LoadProposeDataForJobInterviewMng(ByVal customerNumber As Integer, ByVal employeeNumber As Integer) As IEnumerable(Of Employee.DataObjects.JobInterviewMng.ProposeData) Implements IEmployeeDatabaseAccess.LoadProposeDataForJobInterviewMng

      Dim result As List(Of Employee.DataObjects.JobInterviewMng.ProposeData) = Nothing

      Dim sql As String

			sql = "SELECT ID, ProposeNr, MANr, P.P_State, P.CreatedOn, P.CreatedFrom, Bezeichnung "
			sql &= " FROM Propose P WHERE KDNr = @customerNumber AND MANr = @maNumber ORDER BY ProposeNr ASC"

      Dim customerNumberParameter As New SqlClient.SqlParameter("customerNumber", customerNumber)
      Dim employeeNumberParameter As New SqlClient.SqlParameter("maNumber", employeeNumber)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(customerNumberParameter)
      listOfParams.Add(employeeNumberParameter)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of Employee.DataObjects.JobInterviewMng.ProposeData)

          While reader.Read

            Dim proposeData = New Employee.DataObjects.JobInterviewMng.ProposeData()
            proposeData.ID = SafeGetInteger(reader, "ID", 0)
            proposeData.ProposeNumber = SafeGetInteger(reader, "ProposeNr", Nothing)
            proposeData.EmployeeNumber = SafeGetInteger(reader, "MANr", Nothing)
						proposeData.Description = SafeGetString(reader, "Bezeichnung")

						proposeData.CreatedOn = SafeGetDateTime(reader, "createdon", Nothing)
						proposeData.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						proposeData.P_State = SafeGetString(reader, "P_State")

            result.Add(proposeData)

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
    ''' Adds a employee job interview.
    ''' </summary>
    ''' <param name="interviewData">The interview data.</param>
    ''' <returns>Boolean value indicating success.</returns>
    Function AddEmployeeJobInterview(ByVal interviewData As EmployeeJobAppointmentData) As Boolean Implements IEmployeeDatabaseAccess.AddEmployeeJobInterview

      Dim success = True

      Dim sql As String

      sql = "[Create New MA_JobTermin]"

      ' Parameters

      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("@MANr", ReplaceMissing(interviewData.EmployeeNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@JobTitel", ReplaceMissing(interviewData.JobTitle, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@TerminDate", ReplaceMissing(interviewData.AppointmentDate, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@KDNr", ReplaceMissing(interviewData.CustomerNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Firma", ReplaceMissing(interviewData.Company, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@TerminWith", ReplaceMissing(interviewData.AppointmentWith, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Ort", ReplaceMissing(interviewData.Location, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Telefon", ReplaceMissing(interviewData.Telephone, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Telefax", ReplaceMissing(interviewData.Telefax, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Homepage", ReplaceMissing(interviewData.Homepage, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@eMail", ReplaceMissing(interviewData.eMail, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Ergebnis", ReplaceMissing(interviewData.Outcome, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@JobTerminStatus", ReplaceMissing(interviewData.JobAppointmentState, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@CreatedOn", ReplaceMissing(interviewData.CreatedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@CreatedFrom", ReplaceMissing(interviewData.CreatedFrom, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ChangedOn", ReplaceMissing(interviewData.ChangedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ChangedFrom", ReplaceMissing(interviewData.ChangedFrom, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Result", ReplaceMissing(interviewData.Result, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ProposeNr", ReplaceMissing(interviewData.ProposeNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@VakNr", ReplaceMissing(interviewData.VakNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@OfNr", ReplaceMissing(interviewData.OfNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@KDZHDNr", ReplaceMissing(interviewData.ResponsiblePersonNumber, DBNull.Value)))

      Dim newIdParameter = New SqlClient.SqlParameter("@NewJobTerminId", SqlDbType.Int)
      newIdParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newIdParameter)

      Dim recNrParameter = New SqlClient.SqlParameter("@RecNr ", SqlDbType.Int)
      recNrParameter.Direction = ParameterDirection.Output
      listOfParams.Add(recNrParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If Not newIdParameter.Value Is Nothing AndAlso
          Not recNrParameter.Value Is Nothing Then
        interviewData.ID = CType(newIdParameter.Value, Integer)
        interviewData.RecordNumber = CType(recNrParameter.Value, Integer)
      Else
        success = False
      End If

      Return success

    End Function

    ''' <summary>
    ''' Upadates employee job interview data.
    ''' </summary>
    ''' <param name="interviewData">The job interview data.</param>
    ''' <returns>Boolean value indicating success.</returns>
    Function UpdateEmployeeJobInterview(ByVal interviewData As EmployeeJobAppointmentData) As Boolean Implements IEmployeeDatabaseAccess.UpdateEmployeeJobInterview

      Dim success = True

      Dim sql As String

      sql = "[UPDATE MA_JobTermin]"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)

      listOfParams.Add(New SqlClient.SqlParameter("@ID_MA_JobTermin", interviewData.ID))
      listOfParams.Add(New SqlClient.SqlParameter("@RecNr", ReplaceMissing(interviewData.RecordNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@JobTitel", ReplaceMissing(interviewData.JobTitle, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@MANr", ReplaceMissing(interviewData.EmployeeNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@TerminDate", ReplaceMissing(interviewData.AppointmentDate, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@KDNr", ReplaceMissing(interviewData.CustomerNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Firma", ReplaceMissing(interviewData.Company, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@TerminWith", ReplaceMissing(interviewData.AppointmentWith, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Ort", ReplaceMissing(interviewData.Location, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Telefon", ReplaceMissing(interviewData.Telephone, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Telefax", ReplaceMissing(interviewData.Telephone, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Homepage", ReplaceMissing(interviewData.Homepage, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@eMail", ReplaceMissing(interviewData.eMail, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Ergebnis", ReplaceMissing(interviewData.Outcome, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@JobTerminStatus", ReplaceMissing(interviewData.JobAppointmentState, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@CreatedOn", ReplaceMissing(interviewData.CreatedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@CreatedFrom", ReplaceMissing(interviewData.CreatedFrom, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ChangedOn", ReplaceMissing(interviewData.ChangedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ChangedFrom", ReplaceMissing(interviewData.ChangedFrom, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Result", ReplaceMissing(interviewData.Result, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ProposeNr", ReplaceMissing(interviewData.ProposeNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@VakNr", ReplaceMissing(interviewData.VakNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@OfNr", ReplaceMissing(interviewData.OfNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@KDZHDNr", ReplaceMissing(interviewData.ResponsiblePersonNumber, DBNull.Value)))

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      Return success

    End Function

    ''' <summary>
    ''' Deletes an employee job interview.
    ''' </summary>
    ''' <param name="id">The id of the job interview.</param>
    ''' <returns>Boolean value indicating success.</returns>
    Function DeleteEmployeeJobInterview(ByVal id As Integer) As DeleteEmployeeJobAppointmentResult Implements IEmployeeDatabaseAccess.DeleteEmployeeJobInterview

      Dim success = True

      Dim sql As String

      sql = "[Delete MA_JobTermin]"

      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("id", id))

      Dim resultParameter = New SqlClient.SqlParameter("@Result", SqlDbType.Int)
      resultParameter.Direction = ParameterDirection.Output
      listOfParams.Add(resultParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      Dim resultEnum As DeleteEmployeeJobAppointmentResult

      If Not resultParameter.Value Is Nothing Then
        Try
          resultEnum = CType(resultParameter.Value, DeleteEmployeeJobAppointmentResult)
        Catch
          resultEnum = DeleteEmployeeJobAppointmentResult.ErrorWhileDelete
        End Try
      Else
        resultEnum = DeleteEmployeeJobAppointmentResult.ErrorWhileDelete
      End If

      Return resultEnum
    End Function



		''' <summary>
		''' Loads employee job interview data (MA_JobTermin) for propose.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <param name="interviewRecorNumber">Optional job interview record number.</param>
		''' <returns>Employee job interview data or nothing in error case.</returns>
		Function LoadEmployeeJobInterviewsForPropose(ByVal employeeNumber As Integer, ByVal proposeNumber As Integer, Optional ByVal interviewRecorNumber As Integer? = Nothing) As IEnumerable(Of EmployeeJobAppointmentData) Implements IEmployeeDatabaseAccess.LoadEmployeeJobInterviewsForPropose
			Dim result As List(Of EmployeeJobAppointmentData) = Nothing

			Dim sql As String

			sql = "SELECT * FROM MA_JobTermin WHERE MANr = @maNumber AND ProposeNr = @proposeNumber And (@interviewRecordNumber IS NULL OR  RecNr = @interviewRecordNumber) ORDER BY TerminDate Desc"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@maNumber", employeeNumber))
			listOfParams.Add(New SqlClient.SqlParameter("@proposeNumber", proposeNumber))
			listOfParams.Add(New SqlClient.SqlParameter("@interviewRecordNumber", ReplaceMissing(interviewRecorNumber, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeJobAppointmentData)

					While reader.Read()
						Dim interviewData As New EmployeeJobAppointmentData
						interviewData.ID = SafeGetInteger(reader, "ID", 0)
						interviewData.RecordNumber = SafeGetInteger(reader, "RecNr", Nothing)
						interviewData.JobTitle = SafeGetString(reader, "JobTitel", Nothing)
						interviewData.EmployeeNumber = SafeGetInteger(reader, "MANr", Nothing)
						interviewData.AppointmentDate = SafeGetDateTime(reader, "TerminDate", Nothing)
						interviewData.CustomerNumber = SafeGetInteger(reader, "KDNr", Nothing)
						interviewData.Company = SafeGetString(reader, "Firma", Nothing)
						interviewData.AppointmentWith = SafeGetString(reader, "TerminWith", Nothing)
						interviewData.Location = SafeGetString(reader, "Ort", Nothing)
						interviewData.Telephone = SafeGetString(reader, "Telefon", Nothing)
						interviewData.Telefax = SafeGetString(reader, "Telefax", Nothing)
						interviewData.Homepage = SafeGetString(reader, "Homepage", Nothing)
						interviewData.eMail = SafeGetString(reader, "eMail", Nothing)
						interviewData.Outcome = SafeGetString(reader, "Ergebnis", Nothing)
						interviewData.JobAppointmentState = SafeGetString(reader, "JobTerminStatus", Nothing)
						interviewData.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						interviewData.CreatedFrom = SafeGetString(reader, "CreatedFrom", Nothing)
						interviewData.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
						interviewData.ChangedFrom = SafeGetString(reader, "ChangedFrom", Nothing)
						interviewData.Result = SafeGetString(reader, "Result", Nothing)
						interviewData.ProposeNr = SafeGetInteger(reader, "ProposeNr", Nothing)
						interviewData.VakNr = SafeGetInteger(reader, "VakNr", Nothing)
						interviewData.OfNr = SafeGetInteger(reader, "OfNr", Nothing)
						interviewData.ResponsiblePersonNumber = SafeGetInteger(reader, "KDZHDNr", Nothing)

						result.Add(interviewData)

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


  End Class

End Namespace