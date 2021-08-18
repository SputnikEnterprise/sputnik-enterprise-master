Imports SP.DatabaseAccess.Employee.DataObjects.MediationMng
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects

Namespace Employee

  Partial Public Class EmployeeDatabaseAccess
    Inherits DatabaseAccessBase
    Implements IEmployeeDatabaseAccess

    ''' <summary>
    ''' Loads employee employment type data.
    ''' </summary>
    ''' <returns>List of employee employement type data.</returns>
    Public Function LoadEmployeeEmployementTypeData() As IEnumerable(Of EmployeeEmployementType) Implements IEmployeeDatabaseAccess.LoadEmployeeEmployementTypeData

      Dim result As List(Of EmployeeEmployementType) = Nothing

      Dim sql As String = String.Empty

      sql = String.Format("SELECT ID, Bezeichnung , {0} TranslatedText FROM Tab_MAAnstellung ORDER BY Bezeichnung", MapLanguageToColumnName(SelectedTranslationLanguage))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try
        If (Not reader Is Nothing) Then

          result = New List(Of EmployeeEmployementType)

          While reader.Read()
            Dim employeeEmploymentTypeData As New EmployeeEmployementType
            employeeEmploymentTypeData.ID = SafeGetInteger(reader, "ID", 0)
            employeeEmploymentTypeData.Description = SafeGetString(reader, "Bezeichnung")
            employeeEmploymentTypeData.TranslatedEmploymentType = SafeGetString(reader, "TranslatedText")

            result.Add(employeeEmploymentTypeData)
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
    ''' Loads employee assigned employment type (MA_Anstellung) data.
    ''' </summary>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <returns>List of employee assigned employment type data.</returns>
    Public Function LoadEmployeeAssignedEmploymentTypeData(ByVal employeeNumber As Integer) As IEnumerable(Of EmployeeAssignedEmploymentTypeData) Implements IEmployeeDatabaseAccess.LoadEmployeeAssignedEmploymentTypeData

      Dim result As List(Of EmployeeAssignedEmploymentTypeData) = Nothing

      Dim sql As String = String.Empty

      sql = sql & String.Format("SELECT MA_Anstellung.ID, MA_Anstellung.MANr, MA_Anstellung.Bezeichnung, Tab_MAAnstellung.{0} as TranslatedText ", MapLanguageToColumnName(SelectedTranslationLanguage))
      sql = sql & "FROM MA_Anstellung LEFT JOIN Tab_MAAnstellung ON MA_Anstellung.Bezeichnung = Tab_MAAnstellung.Bezeichnung "
      sql = sql & "WHERE MA_Anstellung.MANr = @employeeNumber "
      sql = sql & "ORDER BY TranslatedText"

      Dim employeeNumberParameter As New SqlClient.SqlParameter("employeeNumber", employeeNumber)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(employeeNumberParameter)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try
        If (Not reader Is Nothing) Then

          result = New List(Of EmployeeAssignedEmploymentTypeData)

          While reader.Read()
            Dim assignedEmploymentTypeData As New EmployeeAssignedEmploymentTypeData
            assignedEmploymentTypeData.ID = SafeGetInteger(reader, "ID", 0)
            assignedEmploymentTypeData.EmployeeNumber = SafeGetInteger(reader, "MANr", 0)
            assignedEmploymentTypeData.Description = SafeGetString(reader, "Bezeichnung")
            assignedEmploymentTypeData.TranslatedEmploymentType = SafeGetString(reader, "TranslatedText")

            result.Add(assignedEmploymentTypeData)
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
    ''' Loads employee communication data.
    ''' </summary>
    ''' <returns>List of employee communication data.</returns>
    Public Function LoadEmployeeCommunicationData() As IEnumerable(Of EmployeeCommunicationData) Implements IEmployeeDatabaseAccess.LoadEmployeeCommunicationData

      Dim result As List(Of EmployeeCommunicationData) = Nothing

      Dim sql As String = String.Empty

      sql = String.Format("SELECT ID, Bezeichnung, {0} TranslatedText FROM Tab_MAKommunikation ORDER BY Bezeichnung", MapLanguageToColumnName(SelectedTranslationLanguage))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try
        If (Not reader Is Nothing) Then

          result = New List(Of EmployeeCommunicationData)

          While reader.Read()
            Dim employeeCommunicationData As New EmployeeCommunicationData
            employeeCommunicationData.ID = SafeGetInteger(reader, "ID", 0)
            employeeCommunicationData.Description = SafeGetString(reader, "Bezeichnung")
            employeeCommunicationData.TranslatedCommunicationText = SafeGetString(reader, "TranslatedText")

            result.Add(employeeCommunicationData)
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
    ''' Loads employee assigned communication data.
    ''' </summary>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <returns>List of employee assigned communication data.</returns>
    Public Function LoadEmployeeAssignedCommunicationData(ByVal employeeNumber As Integer) As IEnumerable(Of EmployeeAssignedCommunicationData) Implements IEmployeeDatabaseAccess.LoadEmployeeAssignedCommunicationData

      Dim result As List(Of EmployeeAssignedCommunicationData) = Nothing

      Dim sql As String = String.Empty

      sql = sql & String.Format("SELECT MA_Kommunikation.ID, MA_Kommunikation.MANr, MA_Kommunikation.Bezeichnung, Tab_MAKommunikation.{0} as TranslatedText ", MapLanguageToColumnName(SelectedTranslationLanguage))
      sql = sql & "FROM MA_Kommunikation LEFT JOIN Tab_MAKommunikation ON MA_Kommunikation.Bezeichnung = Tab_MAKommunikation.Bezeichnung "
      sql = sql & "WHERE MA_Kommunikation.MANr = @employeeNumber "
      sql = sql & "ORDER BY TranslatedText"

      Dim employeeNumberParameter As New SqlClient.SqlParameter("employeeNumber", employeeNumber)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(employeeNumberParameter)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try
        If (Not reader Is Nothing) Then

          result = New List(Of EmployeeAssignedCommunicationData)

          While reader.Read()
            Dim employeeCommunicationData As New EmployeeAssignedCommunicationData
            employeeCommunicationData.ID = SafeGetInteger(reader, "ID", 0)
            employeeCommunicationData.EmployeeNumber = SafeGetInteger(reader, "MANr", Nothing)
            employeeCommunicationData.Description = SafeGetString(reader, "Bezeichnung")
            employeeCommunicationData.TranslatedCommunicationText = SafeGetString(reader, "TranslatedText")

            result.Add(employeeCommunicationData)
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
    ''' Loads assessment data.
    ''' </summary>
    ''' <returns>List of assessment data.</returns>
    Public Function LoadAssessmentData() As IEnumerable(Of AssessmentData) Implements IEmployeeDatabaseAccess.LoadAssessmentData

      Dim result As List(Of AssessmentData) = Nothing

      Dim sql As String = String.Empty

      sql = String.Format("SELECT ID, Bezeichnung, Result, {0} as TranslatedText FROM Tab_Beurteilung ORDER BY {0}", MapLanguageToColumnName(SelectedTranslationLanguage))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of AssessmentData)

          While reader.Read()
            Dim assessmentData As New AssessmentData
            assessmentData.ID = SafeGetInteger(reader, "ID", 0)
            assessmentData.Description = SafeGetString(reader, "Bezeichnung")
            assessmentData.Result = SafeGetString(reader, "Result")
            assessmentData.TranslatedAssessmentText = SafeGetString(reader, "TranslatedText")

            result.Add(assessmentData)
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
    ''' Loads employee assigned assessment data.
    ''' </summary>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <returns>List of employee assigned assessment data.</returns>
    Function LoadEmployeeAssignedAssessmentData(ByVal employeeNumber As Integer) As IEnumerable(Of EmployeeAssignedAssessmentData) Implements IEmployeeDatabaseAccess.LoadEmployeeAssignedAssessmentData

      Dim result As List(Of EmployeeAssignedAssessmentData) = Nothing

      Dim sql As String = String.Empty

      sql = sql & String.Format("SELECT MA_Beurteilung.ID, MA_Beurteilung.MANr, MA_Beurteilung.Bezeichnung,MA_Beurteilung.Result, Tab_Beurteilung.{0} as TranslatedText ", MapLanguageToColumnName(SelectedTranslationLanguage))
      sql = sql & "FROM MA_Beurteilung LEFT JOIN Tab_Beurteilung ON MA_Beurteilung.Bezeichnung = Tab_Beurteilung.Bezeichnung "
      sql = sql & "WHERE MA_Beurteilung.MANr = @employeeNumber "
      sql = sql & "ORDER BY TranslatedText"

      Dim employeeNumberParameter As New SqlClient.SqlParameter("employeeNumber", employeeNumber)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(employeeNumberParameter)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try
        If (Not reader Is Nothing) Then

          result = New List(Of EmployeeAssignedAssessmentData)

          While reader.Read()
            Dim employeeCommunicationData As New EmployeeAssignedAssessmentData
            employeeCommunicationData.ID = SafeGetInteger(reader, "ID", 0)
            employeeCommunicationData.EmployeeNumber = SafeGetInteger(reader, "MANr", Nothing)
            employeeCommunicationData.Description = SafeGetString(reader, "Bezeichnung")
            employeeCommunicationData.TranslatedDescription = SafeGetString(reader, "TranslatedText")

            result.Add(employeeCommunicationData)
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
    ''' Loads driving licence data.
    ''' </summary>
    ''' <returns>List of driving licence data.</returns>
    Public Function LoadDrivingLicenceData() As IEnumerable(Of DrivingLicenceData) Implements IEmployeeDatabaseAccess.LoadDrivingLicenceData

      Dim result As List(Of DrivingLicenceData) = Nothing

      Dim sql As String = String.Empty

      sql = String.Format("SELECT RecValue, {0} as TranslatedText FROM tbl_base_F_Schein ORDER BY RecValue", MapLanguageToColumnName(SelectedTranslationLanguage))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of DrivingLicenceData)

          While reader.Read()
            Dim drivingLicenceData As New DrivingLicenceData
            drivingLicenceData.RecValue = SafeGetString(reader, "RecValue")
            drivingLicenceData.TranslatedDrivingLicenceText = SafeGetString(reader, "TranslatedText")
            drivingLicenceData.ValueToShow = String.Format("{0}: {1}", SafeGetString(reader, "RecValue"), SafeGetString(reader, "TranslatedText"))

            result.Add(drivingLicenceData)
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
    ''' Loads vehicle data.
    ''' </summary>
    ''' <returns>List of vehicle data.</returns>
    Public Function LoadVehicleData() As IEnumerable(Of VehicleData) Implements IEmployeeDatabaseAccess.LoadVehicleData

      Dim result As List(Of VehicleData) = Nothing

      Dim sql As String = String.Empty

      sql = String.Format("SELECT RecValue, {0} as TranslatedText FROM tbl_base_Fahrzeug ORDER BY RecValue", MapLanguageToColumnName(SelectedTranslationLanguage))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of VehicleData)

          While reader.Read()
            Dim vehicleData As New VehicleData
            vehicleData.RecValue = SafeGetString(reader, "RecValue")
            vehicleData.TranslatedVehicleText = SafeGetString(reader, "TranslatedText")

            result.Add(vehicleData)
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
    ''' Loads car reserve data.
    ''' </summary>
    ''' <returns>List of car reserve data.</returns>
    Public Function LoadCarReserveData() As IEnumerable(Of CarReserveData) Implements IEmployeeDatabaseAccess.LoadCarReserveData

      Dim result As List(Of CarReserveData) = Nothing

      Dim sql As String = String.Empty

      sql = String.Format("SELECT ID, Bezeichnung, Result, {0} as TranslatedText FROM Tab_AutoRes ORDER BY ID", MapLanguageToColumnName(SelectedTranslationLanguage))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of CarReserveData)

          While reader.Read()
            Dim carReserveData As New CarReserveData
            carReserveData.ID = SafeGetInteger(reader, "ID", 0)
            carReserveData.Description = SafeGetString(reader, "Bezeichnung")
            carReserveData.Result = SafeGetString(reader, "Result")
            carReserveData.TranslatedReserveText = SafeGetString(reader, "TranslatedText")
            result.Add(carReserveData)

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
    ''' Loads contact reserve data.
    ''' </summary>
    ''' <param name="contactReserveType">The contact reserve type.</param>
    ''' <returns>List of employee contact reserve data.</returns>
    Public Function LoadContactReserveData(ByVal contactReserveType As EmployeeContactReserveType) As IEnumerable(Of ContactReserveData) Implements IEmployeeDatabaseAccess.LoadContactReserveData

      Dim result As List(Of ContactReserveData) = Nothing

      Dim sql As String = String.Empty

      sql = String.Format("SELECT ID, Bezeichnung, Result, {0} as TranslatedText FROM Tab_KontaktRes{1}", MapLanguageToColumnName(SelectedTranslationLanguage), CType(contactReserveType, Integer))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of ContactReserveData)

          While reader.Read()
            Dim contactReserveData As New ContactReserveData
            contactReserveData.ID = SafeGetInteger(reader, "ID", 0)
            contactReserveData.Description = SafeGetString(reader, "Bezeichnung")
            contactReserveData.Result = SafeGetString(reader, "Result")
            contactReserveData.TranslatedReserveText = SafeGetString(reader, "TranslatedText")
            result.Add(contactReserveData)

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
    ''' Loads the employee communication reserve 5 data.
    ''' </summary>
    ''' <returns>List of employee reserve 5 data.</returns>
    Public Function LoadEmployeeContactCommReserve5Data() As IEnumerable(Of ContactReserve5Data) Implements IEmployeeDatabaseAccess.LoadEmployeeContactCommReserve5Data

      Dim result As List(Of ContactReserve5Data) = Nothing

      Dim sql As String

      sql = "SELECT Res5 FROM MAKontakt_Komm WHERE NOT (Res5 IS NULL Or Res5 = '') GROUP BY Res5 Order By Res5 Asc"

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of ContactReserve5Data)

          While reader.Read()
            Dim reserveData As New ContactReserve5Data
            reserveData.Reserve5 = SafeGetString(reader, "Res5")

            result.Add(reserveData)

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
    ''' Loads dead line (Tab_Kundfristen) data
    ''' </summary>
    ''' <returns>List of deadline data.</returns>
    Public Function LoadDeadLineData() As IEnumerable(Of DeadlineData) Implements IEmployeeDatabaseAccess.LoadDeadLineData

      Dim result As List(Of DeadlineData) = Nothing

      Dim sql As String

      sql = String.Format("SELECT ID, GetFeld, {0} as TranslatedText FROM Tab_Kundfristen ORDER BY ID", MapLanguageToColumnName(SelectedTranslationLanguage))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of DeadlineData)

          While reader.Read()
            Dim deadlineData As New DeadlineData
            deadlineData.ID = SafeGetInteger(reader, "ID", 0)
            deadlineData.GetField = SafeGetString(reader, "GetFeld")
            deadlineData.TranslatedDeadlineText = SafeGetString(reader, "TranslatedText")

            result.Add(deadlineData)

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
    ''' Loads work pensum data.
    ''' </summary>
    ''' <returns>List of work pensum data.</returns>
    Public Function LoadWorkPensumData() As IEnumerable(Of WorkPensumData) Implements IEmployeeDatabaseAccess.LoadWorkPensumData

      Dim result As List(Of WorkPensumData) = Nothing

      Dim sql As String

      sql = "SELECT ID, GetFeld FROM Tab_ArbPensum ORDER BY ID"

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of WorkPensumData)

          While reader.Read()
            Dim deadlineData As New WorkPensumData
            deadlineData.ID = SafeGetInteger(reader, "ID", 0)
            deadlineData.GetField = SafeGetString(reader, "GetFeld")

            result.Add(deadlineData)

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
    ''' Adds a employee employment type assignment.
    ''' </summary>
    ''' <param name="employeeEmploymentTypeAssignment">The employee employment type assignment.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function AddEmployeeEmploymentTypeAssignment(ByVal employeeEmploymentTypeAssignment As EmployeeAssignedEmploymentTypeData) As Boolean Implements IEmployeeDatabaseAccess.AddEmployeeEmploymentTypeAssignment

      Dim success = True

      Dim sql As String

      sql = "INSERT INTO MA_Anstellung (MANr, Bezeichnung) VALUES(@employeeNumber, @description)"

      ' Parameters
      Dim employeeNumberParameter As New SqlClient.SqlParameter("employeeNumber", ReplaceMissing(employeeEmploymentTypeAssignment.EmployeeNumber, DBNull.Value))
      Dim descriptionParameter As New SqlClient.SqlParameter("description", ReplaceMissing(employeeEmploymentTypeAssignment.Description, DBNull.Value))
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(employeeNumberParameter)
      listOfParams.Add(descriptionParameter)

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function

    ''' <summary>
    ''' Adds a employee communication data assignment.
    ''' </summary>
    ''' <param name="coummunicationAssignment">The communication data.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function AddEmployeeCommunicationAssignment(ByVal coummunicationAssignment As EmployeeAssignedCommunicationData) As Boolean Implements IEmployeeDatabaseAccess.AddEmployeeCommunicationAssignment

      Dim success = True

      Dim sql As String

      sql = "INSERT INTO MA_Kommunikation (MANr, Bezeichnung) VALUES(@employeeNumber, @description); SELECT @@IDENTITY"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("employeeNumber", ReplaceMissing(coummunicationAssignment.EmployeeNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("description", ReplaceMissing(coummunicationAssignment.Description, DBNull.Value)))

      Dim id = ExecuteScalar(sql, listOfParams)

      If Not id Is Nothing Then
        coummunicationAssignment.ID = CType(id, Integer)
      Else
        success = False
      End If

      Return success

    End Function

    ''' <summary>
    ''' Adds an employee assessment assignment.
    ''' </summary>
    ''' <param name="assessmentAssignment"></param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function AddEmployeeAssessmentAssignment(ByVal assessmentAssignment As EmployeeAssignedAssessmentData) As Boolean Implements IEmployeeDatabaseAccess.AddEmployeeAssessmentAssignment

      Dim success = True

      Dim sql As String

      sql = "INSERT INTO MA_Beurteilung(MANr, Bezeichnung) VALUES(@employeeNumber, @description); SELECT @@IDENTITY"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("employeeNumber", ReplaceMissing(assessmentAssignment.EmployeeNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("description", ReplaceMissing(assessmentAssignment.Description, DBNull.Value)))

      Dim id = ExecuteScalar(sql, listOfParams)

      If Not id Is Nothing Then
        assessmentAssignment.ID = CType(id, Integer)
      Else
        success = False
      End If

      Return success

    End Function

    ''' <summary>
    ''' Deletes an employee employment type assignment.
    ''' </summary>
    ''' <param name="id">The database id of the employment type assigment.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function DeleteEmployeeEmploymentTypeAssignment(ByVal id As Integer) As Boolean Implements IEmployeeDatabaseAccess.DeleteEmployeeEmploymentTypeAssignment

      Dim success = True

      Dim sql As String

      sql = "DELETE FROM MA_Anstellung WHERE ID = @id"

      Dim idParameter As New SqlClient.SqlParameter("id", id)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(idParameter)

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function

    ''' <summary>
    ''' Deletes a employee communication data assignment.
    ''' </summary>
    ''' <param name="id">The database id of the communication data assigment.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Function DeleteEmployeeCommunicationAssignment(ByVal id As Integer) As Boolean Implements IEmployeeDatabaseAccess.DeleteEmployeeCommunicationAssignment

      Dim success = True

      Dim sql As String

      sql = "DELETE FROM MA_Kommunikation WHERE ID = @id"

      Dim idParameter As New SqlClient.SqlParameter("id", id)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(idParameter)

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function

    ''' <summary>
    ''' Deletes an employee assessment data assignment.
    ''' </summary>
    ''' <param name="id">The database id of assessment assigment.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Function DeleteEmployeeAssessmentAssignment(ByVal id As Integer) As Boolean Implements IEmployeeDatabaseAccess.DeleteEmployeeAssessmentAssignment

      Dim success = True

      Dim sql As String

      sql = "DELETE FROM MA_Beurteilung WHERE ID = @id"

      Dim idParameter As New SqlClient.SqlParameter("id", id)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(idParameter)

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function

  End Class

End Namespace
