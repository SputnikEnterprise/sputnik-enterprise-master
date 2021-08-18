Imports SP.DatabaseAccess.Employee.DataObjects.LanguagesAndProfessionsMng

Namespace Employee

  Partial Public Class EmployeeDatabaseAccess
    Inherits DatabaseAccessBase
    Implements IEmployeeDatabaseAccess

    ''' <summary>
    ''' Loads job candidate language (Tab_Bew_Sprache) data.
    ''' </summary>
    ''' <returns>List of job candidate language data.</returns>
    Public Function LoadJobCandidateLanguageData() As IEnumerable(Of JobCandidateLanguageData) Implements IEmployeeDatabaseAccess.LoadJobCandidateLanguageData

      Dim result As List(Of JobCandidateLanguageData) = Nothing

      Dim sql As String = String.Empty

      sql = sql & String.Format("SELECT ID, GetFeld, {0} as TranslatedText FROM Tab_Bew_Sprache ORDER BY TranslatedText ", MapLanguageToColumnName(SelectedTranslationLanguage))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try
        If (Not reader Is Nothing) Then

          result = New List(Of JobCandidateLanguageData)

          While reader.Read()
            Dim jobCandidateLanguageData As New JobCandidateLanguageData
            jobCandidateLanguageData.ID = SafeGetInteger(reader, "ID", 0)
            jobCandidateLanguageData.GetField = SafeGetString(reader, "GetFeld")
            jobCandidateLanguageData.TranslatedLanguageText = SafeGetString(reader, "TranslatedText")

            result.Add(jobCandidateLanguageData)
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
    ''' Loads employee assigned profession data.
    ''' </summary>
    '''<param name="employeeNumber">The employee number.</param>
    '''<param name="gender">The optional gender.</param>
    ''' <returns>List of employee assigned profession data.</returns>
    Public Function LoadEmployeeAssignedProfessionData(ByVal employeeNumber As Integer, Optional ByVal gender As Char = "M") As IEnumerable(Of EmployeeAssignedProfessionData) Implements IEmployeeDatabaseAccess.LoadEmployeeAssignedProfessionData

      Dim result As List(Of EmployeeAssignedProfessionData) = Nothing

      Dim translationColumn As String = String.Empty

      Select Case SelectedTranslationLanguage
        Case DatabaseAccess.Language.German
          translationColumn = "[BerufsBezeichnung D {0}]"
        Case DatabaseAccess.Language.Italian
          translationColumn = "[BerufsBezeichnung I {0}]"
        Case DatabaseAccess.Language.French
          translationColumn = "[BerufsBezeichnung F {0}]"
        Case DatabaseAccess.Language.English
          translationColumn = "[BerufsBezeichnung E {0}]"
        Case Else
          translationColumn = "[BerufsBezeichnung D {0}]"
      End Select
      translationColumn = String.Format(translationColumn, Char.ToUpper(gender))

      Dim sql As String = String.Empty

      sql = sql & String.Format("SELECT MA_ES_Als.ID, MA_ES_Als.MANr, MA_ES_Als.BerufsText, MA_ES_Als.BerufCode, Job.{0} AS TranslatedText ", translationColumn)
      sql = sql & "FROM MA_ES_Als LEFT JOIN Job "
      sql = sql & "ON MA_ES_Als.BerufCode = Job.Code "
      sql = sql & "WHERE MA_ES_Als.MANr = @employeeNumber ORDER BY TranslatedText"

      Dim employeeNumberParameter As New SqlClient.SqlParameter("employeeNumber", employeeNumber)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(employeeNumberParameter)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try
        If (Not reader Is Nothing) Then

          result = New List(Of EmployeeAssignedProfessionData)

          While reader.Read()
            Dim employeeProfessionData As New EmployeeAssignedProfessionData
            employeeProfessionData.ID = SafeGetInteger(reader, "ID", 0)
						employeeProfessionData.EmployeeNumber = SafeGetInteger(reader, "MANr", Nothing)
						employeeProfessionData.ProfessionText = SafeGetString(reader, "BerufsText")
						employeeProfessionData.ProfessionCode = SafeGetInteger(reader, "BerufCode", Nothing)
            employeeProfessionData.TranslatedProfessionText = If(SafeGetString(reader, "TranslatedText") = String.Empty,
                                                                 SafeGetString(reader, "BerufsText") & " | (Not founded!)",
                                                                 SafeGetString(reader, "TranslatedText"))


            result.Add(employeeProfessionData)
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
    ''' Loads employee assigned sector data.
    ''' </summary>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <returns>List of sector data.</returns>
    Public Function LoadEmployeeAssignedSectorData(ByVal employeeNumber As Integer) As IEnumerable(Of EmployeeAssignedSectorData) Implements IEmployeeDatabaseAccess.LoadEmployeeAssignedSectorData

      Dim result As List(Of EmployeeAssignedSectorData) = Nothing

      Dim translationColumn As String = String.Empty

      Select Case SelectedTranslationLanguage
        Case DatabaseAccess.Language.German
          translationColumn = "[BranchenBezeichnung D]"
        Case DatabaseAccess.Language.Italian
          translationColumn = "[BranchenBezeichnung I]"
        Case DatabaseAccess.Language.French
          translationColumn = "[BranchenBezeichnung F]"
        Case DatabaseAccess.Language.English
          translationColumn = "[BranchenBezeichnung E]"
        Case Else
          translationColumn = "[BranchenBezeichnung D]"
      End Select

      Dim sql As String = String.Empty

      sql = sql & String.Format("SELECT MA_Branche.ID, MA_Branche.MANr, MA_Branche.Bezeichnung, MA_Branche.Result, MA_Branche.BranchenCode, Branchen.{0} As TranslatedText ", translationColumn)
      sql = sql & "FROM MA_Branche "
      sql = sql & "LEFT JOIN Branchen "
      sql = sql & "ON MA_Branche.BranchenCode = Branchen.Code "
      sql = sql & "WHERE MANr = @employeeNumber ORDER BY TranslatedText"

      Dim employeeNumberParameter As New SqlClient.SqlParameter("employeeNumber", employeeNumber)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(employeeNumberParameter)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try
        If (Not reader Is Nothing) Then

          result = New List(Of EmployeeAssignedSectorData)

          While reader.Read()
            Dim sectorData As New EmployeeAssignedSectorData
            sectorData.ID = SafeGetInteger(reader, "ID", 0)
            sectorData.EmployeeNumber = SafeGetInteger(reader, "MANr", 0)
            sectorData.Description = SafeGetString(reader, "Bezeichnung")
            sectorData.Result = SafeGetString(reader, "Result")
            sectorData.SectorCode = SafeGetInteger(reader, "BranchenCode", Nothing)
            sectorData.TranslatedSectorText = SafeGetString(reader, "TranslatedText")

            result.Add(sectorData)
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
    ''' Loads employee assigned verbal language data.
    ''' </summary>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <returns>List of verbal language data.</returns>
    Public Function LoadEmployeeAssignedVerbalLanguageData(ByVal employeeNumber As Integer) As IEnumerable(Of EmployeeAssignedVerbalLanguageData) Implements IEmployeeDatabaseAccess.LoadEmployeeAssignedVerbalLanguageData

      Dim result As List(Of EmployeeAssignedVerbalLanguageData) = Nothing

      Dim sql As String = String.Empty

      sql = sql & String.Format("SELECT MA_MSprachen.ID, MA_MSprachen.MANr, MA_MSprachen.Bezeichnung, MA_MSprachen.Result, Tab_Bew_Sprache.{0}  AS TranslatedText ", MapLanguageToColumnName(SelectedTranslationLanguage))
      sql = sql & "FROM MA_MSprachen LEFT JOIN Tab_Bew_Sprache "
      sql = sql & "ON MA_MSprachen.Bezeichnung = Tab_Bew_Sprache.GetFeld "
      sql = sql & "WHERE MA_MSprachen.MANr = @employeeNumber ORDER BY TranslatedText"

      Dim employeeNumberParameter As New SqlClient.SqlParameter("employeeNumber", employeeNumber)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(employeeNumberParameter)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try
        If (Not reader Is Nothing) Then

          result = New List(Of EmployeeAssignedVerbalLanguageData)

          While reader.Read()
            Dim verbalLanguageData As New EmployeeAssignedVerbalLanguageData
            verbalLanguageData.ID = SafeGetInteger(reader, "ID", 0)
            verbalLanguageData.EmployeeNumber = SafeGetInteger(reader, "MANr", 0)
            verbalLanguageData.Description = SafeGetString(reader, "Bezeichnung")
            verbalLanguageData.Result = SafeGetString(reader, "Result")
            verbalLanguageData.TranslatedDescriptionText = SafeGetString(reader, "TranslatedText")

            result.Add(verbalLanguageData)
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
    ''' Loads employee assigned written language data.
    ''' </summary>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <returns>List of written language data.</returns>
    Public Function LoadEmployeeAssignedWrittenLanguageData(ByVal employeeNumber As Integer) As IEnumerable(Of EmployeeAssignedWrittenLanguageData) Implements IEmployeeDatabaseAccess.LoadEmployeeAssignedWrittenLanguageData

      Dim result As List(Of EmployeeAssignedWrittenLanguageData) = Nothing

      Dim sql As String = String.Empty

      sql = sql & String.Format("SELECT MA_SSprachen.ID, MA_SSprachen.MANr, MA_SSprachen.Bezeichnung, MA_SSprachen.Result, Tab_Bew_Sprache.{0} as TranslatedText ", MapLanguageToColumnName(SelectedTranslationLanguage))
      sql = sql & "FROM MA_SSprachen "
      sql = sql & "LEFT JOIN Tab_Bew_Sprache "
      sql = sql & "ON MA_SSprachen.Bezeichnung = Tab_Bew_Sprache.GetFeld "
      sql = sql & "WHERE MANr = @employeeNumber ORDER BY TranslatedText"

      Dim employeeNumberParameter As New SqlClient.SqlParameter("employeeNumber", employeeNumber)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(employeeNumberParameter)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try
        If (Not reader Is Nothing) Then

          result = New List(Of EmployeeAssignedWrittenLanguageData)

          While reader.Read()
            Dim writtenLanguageData As New EmployeeAssignedWrittenLanguageData
            writtenLanguageData.ID = SafeGetInteger(reader, "ID", 0)
            writtenLanguageData.EmployeeNumber = SafeGetInteger(reader, "MANr", 0)
            writtenLanguageData.Description = SafeGetString(reader, "Bezeichnung")
            writtenLanguageData.Result = SafeGetString(reader, "Result")
            writtenLanguageData.TranslatedDescriptionText = SafeGetString(reader, "TranslatedText")

            result.Add(writtenLanguageData)
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
    ''' Adds a employee profession assignment.
    ''' </summary>
    ''' <param name="professionAssignment">The employee profession assignment.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function AddEmployeeProfessionAssignment(ByVal professionAssignment As EmployeeAssignedProfessionData) As Boolean Implements IEmployeeDatabaseAccess.AddEmployeeProfessionAssignment

      Dim success = True

      Dim sql As String

			sql = "INSERT INTO MA_ES_Als (MANr, BerufsText, BerufCode) VALUES(@employeeNumber, @description, @professionCodeInteger)"

			' Parameters
			Dim employeeNumberParameter As New SqlClient.SqlParameter("employeeNumber", ReplaceMissing(professionAssignment.EmployeeNumber, DBNull.Value))
			Dim descriptionStringParameter As New SqlClient.SqlParameter("description", ReplaceMissing(professionAssignment.ProfessionText, DBNull.Value))
      Dim professionCodeIntegerParameter As New SqlClient.SqlParameter("professionCodeInteger", ReplaceMissing(professionAssignment.ProfessionCode, DBNull.Value))
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(employeeNumberParameter)
      listOfParams.Add(descriptionStringParameter)
      listOfParams.Add(professionCodeIntegerParameter)

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function

    ''' <summary>
    ''' Adds an employee sector assignment.
    ''' </summary>
    ''' <param name="sectorAssignment">The employee sector assignment.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function AddEmployeeSectorAssignment(ByVal sectorAssignment As EmployeeAssignedSectorData) As Boolean Implements IEmployeeDatabaseAccess.AddEmployeeSectorAssignment

      Dim success = True

      Dim sql As String

      sql = "INSERT INTO MA_Branche (MANr, Bezeichnung, Result, BranchenCode) VALUES(@employeeNumber, @description, @result, @sectorCode)"

      ' Parameters
      Dim employeeNumberParameter As New SqlClient.SqlParameter("employeeNumber", ReplaceMissing(sectorAssignment.EmployeeNumber, DBNull.Value))
      Dim descriptionParameter As New SqlClient.SqlParameter("description", ReplaceMissing(sectorAssignment.Description, DBNull.Value))
      Dim resultParameter As New SqlClient.SqlParameter("result", ReplaceMissing(sectorAssignment.Result, DBNull.Value))
      Dim sectorCodeParameter As New SqlClient.SqlParameter("sectorCode", ReplaceMissing(sectorAssignment.SectorCode, DBNull.Value))
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(employeeNumberParameter)
      listOfParams.Add(descriptionParameter)
      listOfParams.Add(resultParameter)
      listOfParams.Add(sectorCodeParameter)

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function

    ''' <summary>
    ''' Adds an employee verbal language assignment.
    ''' </summary>
    ''' <param name="verbalLanguageAssignment">The employee verbal language assignment.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function AddEmployeeVerbalLanguageAssignment(ByVal verbalLanguageAssignment As EmployeeAssignedVerbalLanguageData) As Boolean Implements IEmployeeDatabaseAccess.AddEmployeeVerbalLanguageAssignment

      Dim success = True

      Dim sql As String

      sql = "INSERT INTO MA_MSprachen (MANr, Bezeichnung, Result) VALUES(@employeeNumber, @description, @result)"

      ' Parameters
      Dim employeeNumberParameter As New SqlClient.SqlParameter("employeeNumber", ReplaceMissing(verbalLanguageAssignment.EmployeeNumber, DBNull.Value))
      Dim descriptionParameter As New SqlClient.SqlParameter("description", ReplaceMissing(verbalLanguageAssignment.Description, DBNull.Value))
      Dim resultParameter As New SqlClient.SqlParameter("result", ReplaceMissing(verbalLanguageAssignment.Result, DBNull.Value))
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(employeeNumberParameter)
      listOfParams.Add(descriptionParameter)
      listOfParams.Add(resultParameter)

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function

    ''' <summary>
    '''  Adds an employee written language assignment.
    ''' </summary>
    ''' <param name="writtenLanguageAssignment">The employee written language assignment.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function AddEmployeeWrittenLanguageAssignment(ByVal writtenLanguageAssignment As EmployeeAssignedWrittenLanguageData) As Boolean Implements IEmployeeDatabaseAccess.AddEmployeeWrittenLaguageAssignment

      Dim success = True

      Dim sql As String

      sql = "INSERT INTO MA_SSprachen (MANr, Bezeichnung, Result) VALUES(@employeeNumber, @description, @result)"

      ' Parameters
      Dim employeeNumberParameter As New SqlClient.SqlParameter("employeeNumber", ReplaceMissing(writtenLanguageAssignment.EmployeeNumber, DBNull.Value))
      Dim descriptionParameter As New SqlClient.SqlParameter("description", ReplaceMissing(writtenLanguageAssignment.Description, DBNull.Value))
      Dim resultParameter As New SqlClient.SqlParameter("result", ReplaceMissing(writtenLanguageAssignment.Result, DBNull.Value))
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(employeeNumberParameter)
      listOfParams.Add(descriptionParameter)
      listOfParams.Add(resultParameter)

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function

    ''' <summary>
    ''' Deletes an employee profession data assignment.
    ''' </summary>
    ''' <param name="id">The database id of profession assigment.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function DeleteEmployeeProfessionDataAssignment(ByVal id As Integer) As Boolean Implements IEmployeeDatabaseAccess.DeleteEmployeeProfessionDataAssignment

      Dim success = True

      Dim sql As String

      sql = "DELETE FROM MA_ES_Als WHERE ID = @id"

      Dim idParameter As New SqlClient.SqlParameter("id", id)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(idParameter)

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function

    ''' <summary>
    ''' Deletes an employee sector assignment.
    ''' </summary>
    ''' <param name="id">The database id of the sector assignment.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function DeleteEmployeeSectorAssignment(ByVal id As Integer) As Boolean Implements IEmployeeDatabaseAccess.DeleteEmployeeSectorAssignment

      Dim success = True

      Dim sql As String

      sql = "DELETE FROM MA_Branche WHERE ID = @id"

      Dim idParameter As New SqlClient.SqlParameter("id", id)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(idParameter)

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function

    ''' <summary>
    ''' Deletes an employee verbal language assignment.
    ''' </summary>
    ''' <param name="id">The database id of the language assignment.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function DeleteEmployeeVerbalLanguageAssignment(ByVal id As Integer) As Boolean Implements IEmployeeDatabaseAccess.DeleteEmployeeVerbalLanguageAssignment

      Dim success = True

      Dim sql As String

      sql = "DELETE FROM MA_MSprachen WHERE ID = @id"

      Dim idParameter As New SqlClient.SqlParameter("id", id)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(idParameter)

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function

    ''' <summary>
    ''' Deletes an employee written language assignment.
    ''' </summary>
    ''' <param name="id">The database id of the written language assignment.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function DeleteEmployeeWrittenLanguageAssignment(ByVal id As Integer) As Boolean Implements IEmployeeDatabaseAccess.DeleteEmployeeWrittenLanguageAssignment

      Dim success = True

      Dim sql As String

      sql = "DELETE FROM MA_SSprachen WHERE ID = @id"

      Dim idParameter As New SqlClient.SqlParameter("id", id)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(idParameter)

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function

  End Class

End Namespace