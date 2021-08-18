
Imports SP.DatabaseAccess.Employee.DataObjects.TodoMng
Imports SP.DatabaseAccess.Employee

Namespace Employee

  Partial Public Class EmployeeDatabaseAccess
    Inherits DatabaseAccessBase
    Implements IEmployeeDatabaseAccess


		''' <summary>
		''' Loads todo list data by search criteria.
		''' </summary>
		''' <returns>List of todo data or nothing in error case.</returns>
		Function LoadTodoListDataBySearchCriteria(ByVal customerID As String, ByVal UserNrs As String, ByVal callerUserNumber As Integer) As IEnumerable(Of TodoListData) Implements IEmployeeDatabaseAccess.LoadTodoListDataBySearchCriteria

			Dim result As List(Of TodoListData) = Nothing

			Dim sql As String

			sql = "[Load TODOList Data For Assigned Users]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("Customer_ID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USNRListe", ReplaceMissing(UserNrs, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of TodoListData)

					While reader.Read()
						Dim searchData As New TodoListData

						searchData.ID = SafeGetInteger(reader, "ID", 0)
						searchData.UserNumber = SafeGetInteger(reader, "USNr", Nothing)
						searchData.EmployeeNumber = SafeGetInteger(reader, "MANr", Nothing)
						searchData.CustomerNumber = SafeGetInteger(reader, "KDNr", Nothing)
						searchData.Subject = SafeGetString(reader, "Subject")
						searchData.IsImportant = SafeGetBoolean(reader, "Important", False)
						searchData.IsCompleted = SafeGetBoolean(reader, "Done", False)
						searchData.Schedulebegins = SafeGetDateTime(reader, "Schedulebegins", Nothing)
						searchData.Scheduleends = SafeGetDateTime(reader, "Scheduleends", Nothing)
						searchData.TODOSourceEnum = SafeGetInteger(reader, "SourceInput", 0)
						searchData.AllUsers = SafeGetBoolean(reader, "AllUsers", False)


						result.Add(searchData)

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

		Function LoadTodoDataForAutoCreatedNotify(ByVal customerID As String, ByVal UserNrs As String, ByVal inputSource As Integer, ByVal modulNumber As Integer, ByVal body As String) As IEnumerable(Of TodoData) Implements IEmployeeDatabaseAccess.LoadTodoDataForAutoCreatedNotify

			Dim result As List(Of TodoData) = Nothing

			Dim sql As String
			Dim dbFieldName As String = String.Empty

			Select Case inputSource
				Case 1
					dbFieldName = "VakNr"
				Case 2
					dbFieldName = "ProposeNr"
				Case 3

			End Select
			sql = String.Format("Select Top (1) ID, USNr, MANr, KDNr, Important, convert(Bit, 0) Done, Schedulebegins, Scheduleends, AllUsers, SourceInput, Subject, Body From dbo.tblTodoList Where Body Like '%{0}%' ", body)
			If Not String.IsNullOrWhiteSpace(dbFieldName) Then sql &= String.Format("And {0} = @ModulNumber", dbFieldName)

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			If Not String.IsNullOrWhiteSpace(dbFieldName) Then listOfParams.Add(New SqlClient.SqlParameter("ModulNumber", ReplaceMissing(modulNumber, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, If(Not String.IsNullOrWhiteSpace(dbFieldName), listOfParams, Nothing), CommandType.Text)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of TodoData)

					While reader.Read()
						Dim searchData As New TodoData

						searchData.ID = SafeGetInteger(reader, "ID", 0)
						searchData.UserNumber = SafeGetInteger(reader, "USNr", Nothing)
						searchData.EmployeeNumber = SafeGetInteger(reader, "MANr", Nothing)
						searchData.CustomerNumber = SafeGetInteger(reader, "KDNr", Nothing)
						searchData.Subject = SafeGetString(reader, "Subject")
						searchData.Body = SafeGetString(reader, "Body")
						searchData.IsImportant = SafeGetBoolean(reader, "Important", False)
						searchData.IsCompleted = SafeGetBoolean(reader, "Done", False)
						searchData.Schedulebegins = SafeGetDateTime(reader, "Schedulebegins", Nothing)
						searchData.Scheduleends = SafeGetDateTime(reader, "Scheduleends", Nothing)
						searchData.TODOSourceEnum = SafeGetInteger(reader, "SourceInput", 0)
						searchData.AllUsers = SafeGetBoolean(reader, "AllUsers", False)


						result.Add(searchData)

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

		Public Function LoadTodoData(ByVal customerID As String, ByVal recID As Integer?, ByVal callerUserNumber As Integer) As TodoData Implements IEmployeeDatabaseAccess.LoadTodoData

			Dim result As TodoData = Nothing

			Dim sql As String

			sql = "[Load Assigned TODOList Data]"

			' Parameters

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("Customer_ID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("TU_USNr", ReplaceMissing(callerUserNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RecID", ReplaceMissing(recID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If Not reader Is Nothing Then

					If reader.Read Then
						result = New TodoData
						result.ID = SafeGetInteger(reader, "ID", 0)
						result.UserNumber = SafeGetInteger(reader, "USNr", Nothing)
						result.TU_UserNumber = callerUserNumber
						result.EmployeeNumber = SafeGetInteger(reader, "MANr", Nothing)
						result.CustomerNumber = SafeGetInteger(reader, "KDNr", Nothing)
						result.ResponsiblePersonRecordNumber = SafeGetInteger(reader, "KDZHDNr", Nothing)
						result.VacancyNumber = SafeGetInteger(reader, "VakNr", Nothing)
						result.ProposeNumber = SafeGetInteger(reader, "ProposeNr", Nothing)
						result.ESNumber = SafeGetInteger(reader, "ESNr", Nothing)
						result.RPNumber = SafeGetInteger(reader, "RPNr", Nothing)
						result.LMNumber = SafeGetInteger(reader, "LMNr", Nothing)
						result.RENumber = SafeGetInteger(reader, "RENr", Nothing)
						result.ZENumber = SafeGetInteger(reader, "ZENr", Nothing)
						result.Subject = SafeGetString(reader, "Subject")
						result.Body = SafeGetString(reader, "Body")
						result.IsImportant = SafeGetBoolean(reader, "Important", False)
						result.IsCompleted = SafeGetBoolean(reader, "Done", False)
						result.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						result.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						result.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
						result.ChangedFrom = SafeGetString(reader, "ChangedFrom")
						result.Schedulebegins = SafeGetDateTime(reader, "Schedulebegins", Nothing)
						result.Scheduleends = SafeGetDateTime(reader, "Scheduleends", Nothing)
						result.ScheduleRememberIn = SafeGetInteger(reader, "ScheduleRememberIn", Nothing)
						result.ScheduleRemember = SafeGetDateTime(reader, "scheduleRemember", Nothing)
						result.TODOSourceEnum = SafeGetInteger(reader, "SourceInput", 0)
						result.AllUsers = SafeGetBoolean(reader, "AllUsers", False)

					End If

				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				result = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return result
		End Function

		Function InsertTodoData(ByVal customerID As String, ByVal todoData As TodoData) As Boolean Implements IEmployeeDatabaseAccess.InsertTodoData

			Dim success = True
			Dim sql As String

			sql = "[Create New Todo]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("Customer_ID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USNr", ReplaceMissing(todoData.UserNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MANr", ReplaceMissing(todoData.EmployeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(todoData.CustomerNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KDZHDNr", ReplaceMissing(todoData.ResponsiblePersonRecordNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("VakNr", ReplaceMissing(todoData.VacancyNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ProposeNr", ReplaceMissing(todoData.ProposeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ESNr", ReplaceMissing(todoData.ESNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RPNr", ReplaceMissing(todoData.RPNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("LMNr", ReplaceMissing(todoData.LMNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RENr", ReplaceMissing(todoData.RENumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ZENr", ReplaceMissing(todoData.ZENumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Subject", ReplaceMissing(todoData.Subject, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Body", ReplaceMissing(todoData.Body, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Important", ReplaceMissing(todoData.IsImportant, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Done", ReplaceMissing(todoData.IsCompleted, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CreatedOn", ReplaceMissing(todoData.CreatedOn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CreatedFrom", ReplaceMissing(todoData.CreatedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ChangedOn", ReplaceMissing(todoData.ChangedOn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ChangedFrom", ReplaceMissing(todoData.ChangedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Schedulebegins", ReplaceMissing(todoData.Schedulebegins, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Scheduleends", ReplaceMissing(todoData.Scheduleends, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ScheduleRememberIn", ReplaceMissing(todoData.ScheduleRememberIn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ScheduleRemember", ReplaceMissing(todoData.ScheduleRemember, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("SourceInput", ReplaceMissing(todoData.TODOSourceEnum, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("AllUsers", ReplaceMissing(todoData.AllUsers, DBNull.Value)))

			Dim recNrParameter = New SqlClient.SqlParameter("@RecNr ", SqlDbType.Int)
			recNrParameter.Direction = ParameterDirection.Output
			listOfParams.Add(recNrParameter)

			Dim newIdParameter = New SqlClient.SqlParameter("@NewTodoId", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)

			Dim id = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If Not newIdParameter.Value Is Nothing AndAlso
					Not recNrParameter.Value Is Nothing Then
				todoData.ID = CType(newIdParameter.Value, Integer)
			Else
				success = False
			End If

			Return success
		End Function

		Function UpdateTodoData(ByVal customerID As String, ByVal todoData As TodoData) As Boolean Implements IEmployeeDatabaseAccess.UpdateTodoData
			Dim success = True
			Dim sql As String

			sql = "[Update Assigned Todo Data]"

			'sql = "UPDATE tblTODOList SET "
			'sql &= " USNr = @USNr, "
			'sql &= " USNr = @USNr, "
			'sql &= " MANr = @MANr, "
			'   sql &= " KDNr = @KDNr, "
			'   sql &= " KDZHDNr = @KDZHDNr, "
			'   sql &= " VakNr = @VakNr, "
			'   sql &= " ProposeNr = @ProposeNr, "
			'   sql &= " ESNr = @ESNr, "
			'   sql &= " RPNr = @RPNr, "
			'   sql &= " LMNr = @LMNr, "
			'   sql &= " RENr = @RENr, "
			'   sql &= " ZENr = @ZENr, "
			'   sql &= " Subject = @Subject, "
			'   sql &= " Body = @Body, "
			'   sql &= " Important = @Important, "
			'   sql &= " Done = @Done, "
			'   sql &= " ChangedOn = @ChangedOn, "
			'   sql &= " ChangedFrom = @ChangedFrom, "
			'   sql &= " Schedulebegins = @Schedulebegins, "
			'   sql &= " Scheduleends = @Scheduleends, "
			'   sql &= " ScheduleRememberIn = @ScheduleRememberIn, "
			'sql &= " ScheduleRemember = @ScheduleRemember, "
			'sql &= " AllUsers = @AllUsers "

			'sql &= "WHERE RecNr = @RecNr "

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("Customer_ID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USNr", ReplaceMissing(todoData.UserNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("TU_USNr", ReplaceMissing(todoData.TU_UserNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MANr", ReplaceMissing(todoData.EmployeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(todoData.CustomerNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KDZHDNr", ReplaceMissing(todoData.ResponsiblePersonRecordNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("VakNr", ReplaceMissing(todoData.VacancyNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ProposeNr", ReplaceMissing(todoData.ProposeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ESNr", ReplaceMissing(todoData.ESNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RPNr", ReplaceMissing(todoData.RPNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("LMNr", ReplaceMissing(todoData.LMNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RENr", ReplaceMissing(todoData.RENumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ZENr", ReplaceMissing(todoData.ZENumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Subject", ReplaceMissing(todoData.Subject, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Body", ReplaceMissing(todoData.Body, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Important", ReplaceMissing(todoData.IsImportant, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Done", ReplaceMissing(todoData.IsCompleted, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ChangedFrom", ReplaceMissing(todoData.ChangedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Schedulebegins", ReplaceMissing(todoData.Schedulebegins, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Scheduleends", ReplaceMissing(todoData.Scheduleends, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ScheduleRememberIn", ReplaceMissing(todoData.ScheduleRememberIn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ScheduleRemember", ReplaceMissing(todoData.ScheduleRemember, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("SourceInput", ReplaceMissing(todoData.TODOSourceEnum, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("AllUsers", ReplaceMissing(todoData.AllUsers, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@RecID", ReplaceMissing(todoData.ID, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Return success
		End Function

		Function InsertTodoUserData(ByVal customerID As String, ByVal todoUserData As TodoUserData) As Boolean Implements IEmployeeDatabaseAccess.InsertTodoUserData

			Dim success = True
			Dim sql As String

			sql = "[Create New TodoUsers]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("Customer_ID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USNr", ReplaceMissing(todoUserData.UserNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ToDoID", ReplaceMissing(todoUserData.FK_ToDoID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Done", False))
			listOfParams.Add(New SqlClient.SqlParameter("CreatedFrom", ReplaceMissing(todoUserData.CreatedFrom, DBNull.Value)))


			Dim recIDParameter = New SqlClient.SqlParameter("NewTODOUserID", SqlDbType.Int)
			recIDParameter.Direction = ParameterDirection.Output
			listOfParams.Add(recIDParameter)

			Dim id = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If Not recIDParameter.Value Is Nothing Then
				todoUserData.ID = CType(recIDParameter.Value, Integer)
			Else
				success = False
			End If

			Return success
		End Function

		''' <summary>
		''' Deletes todo data.
		''' </summary>
		''' <param name="RecID">The todo data RecNr.</param>
		''' <returns>Boolean value indicating success.</returns>
		Function DeleteTodo(ByVal RecID As Integer) As Boolean Implements IEmployeeDatabaseAccess.DeleteTodo

			Dim success = True

			Dim sql As String

			sql = "DELETE FROM tblTODOList WHERE  ID = @RecID; "
			sql = "DELETE FROM tbl_TODO_User WHERE FK_ToDoID = @RecID; "

			Dim idParameter As New SqlClient.SqlParameter("RecID", RecID)
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(idParameter)

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function DeleteTodoUserData(ByVal recID As Integer, ByVal username As String, ByVal usnr As Integer) As Boolean Implements IEmployeeDatabaseAccess.DeleteTodoUserData

			Dim success = True

			Dim sql As String

			sql = "DELETE FROM tbl_TODO_User WHERE ID = @ID"

			Dim idParameter As New SqlClient.SqlParameter("ID", recID)
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(idParameter)

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		''' <summary>
		''' Loads customer name data.
		''' </summary>
		''' <returns>List of user data.</returns>
		Public Function LoadCustomerNameData() As IEnumerable(Of CustomerNameData) Implements IEmployeeDatabaseAccess.LoadCustomerNameData

      Dim result As List(Of CustomerNameData) = Nothing

      Dim sql As String

      sql = "SELECT KDNr, Firma1, Firma2, Firma3, Postfach, Strasse, PLZ, Ort, Land FROM Kunden ORDER BY Firma1, Firma2"

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of CustomerNameData)

          While reader.Read()
            Dim kundenNameData As New CustomerNameData
            kundenNameData.KDNr = SafeGetInteger(reader, "KDNr", 0)
            kundenNameData.Firma1 = SafeGetString(reader, "Firma1")
            kundenNameData.Firma2 = SafeGetString(reader, "Firma2")
            kundenNameData.Firma2 = SafeGetString(reader, "Firma2")

            kundenNameData.Postfach = SafeGetString(reader, "Postfach")
            kundenNameData.Strasse = SafeGetString(reader, "Strasse")
            kundenNameData.PLZ = SafeGetString(reader, "PLZ")
            kundenNameData.Ort = SafeGetString(reader, "Ort")
            kundenNameData.Land = SafeGetString(reader, "Land")

            result.Add(kundenNameData)

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
    ''' Loads ZHD name data.
    ''' </summary>
    ''' <returns>List of user data.</returns>
    Public Function LoadZHDNameData(ByVal KDNr As Integer) As IEnumerable(Of ZHDNameData) Implements IEmployeeDatabaseAccess.LoadZHDNameData

      Dim result As List(Of ZHDNameData) = Nothing

      Dim sql As String

      sql = "SELECT RecNr, KDNr, Nachname, Vorname FROM KD_Zustaendig WHERE KDNr = @KDNr ORDER BY Nachname, Vorname"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("KDNr", KDNr))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of ZHDNameData)

          While reader.Read()
            Dim zhdNameData As New ZHDNameData
            zhdNameData.RecNr = SafeGetInteger(reader, "RecNr", 0)
            zhdNameData.KDNr = SafeGetInteger(reader, "KDNr", 0)
            zhdNameData.Lastname = SafeGetString(reader, "Nachname")
            zhdNameData.Firstname = SafeGetString(reader, "Vorname")

            result.Add(zhdNameData)

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
    ''' Loads user image data.
    ''' </summary>
    ''' <returns>User image data.</returns>
    Public Function LoadUserImageData(ByVal USNr As Integer) As UserImageData Implements IEmployeeDatabaseAccess.LoadUserImageData

      Dim result As UserImageData = Nothing

      Dim sql As String

      sql = "SELECT USNr, USBild FROM Benutzer WHERE USNr = @USNr"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("USNr", USNr))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          If reader.Read Then
            result = New UserImageData
            result.UsrNr = SafeGetInteger(reader, "USNr", 0)
            result.UserImage = SafeGetByteArray(reader, "USBild")
          End If
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
		''' Loads todo user data.
		''' </summary>
		''' <returns>Todo user data.</returns>
		Function LoadTodoUserData(ByVal todoID As Integer?) As IEnumerable(Of TodoUserData) Implements IEmployeeDatabaseAccess.LoadTodoUserData

			Dim result As List(Of TodoUserData) = Nothing

			Dim sql As String

			sql = "SELECT TU.ID"
			sql &= ", TU.FK_TODOID"
			sql &= ", TU.USNr"
			sql &= ", TU.Done"
			sql &= ", TU.CreatedOn"
			sql &= ", TU.CreatedFrom"
			sql &= ", TU.ChangedOn"
			sql &= ", TU.ChangedFrom"

			sql &= ", TU.USNr"
			sql &= ", US.Nachname"
			sql &= ", US.Vorname "
			sql &= "FROM tbl_TODO_User TU "
			sql &= "Left Join Benutzer US "
			sql &= "On US.USNr = TU.USNr "
			sql &= "Where (IsNull(@ToDoID, 0) = 0 Or TU.FK_TODOID = @ToDoID) "
			sql &= "Order By US.Nachname ASC, TU.CreatedOn Desc "

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("ToDoID", ReplaceMissing(todoID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of TodoUserData)

					While reader.Read()

						Dim user As New TodoUserData

						user.ID = SafeGetInteger(reader, "ID", 0)
						user.FK_ToDoID = SafeGetInteger(reader, "FK_ToDoID", 0)
						user.UserNumber = SafeGetInteger(reader, "USNr", 0)
						user.LastName = SafeGetString(reader, "Nachname")
						user.FirstName = SafeGetString(reader, "Vorname")
						user.Done = SafeGetBoolean(reader, "Done", False)

						user.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						user.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						user.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
						user.ChangedFrom = SafeGetString(reader, "ChangedFrom")


						result.Add(user)

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
		''' Loads todo user data.
		''' </summary>
		''' <returns>Todo user data.</returns>
		Function LoadUserDataFromTODOList(ByVal customerID As String) As IEnumerable(Of TodoUserData) Implements IEmployeeDatabaseAccess.LoadUserDataFromTODOList

			Dim result As List(Of TodoUserData) = Nothing

			Dim sql As String

			sql = "[Load User Data From TODOList]"


			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("Customer_ID", ReplaceMissing(customerID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of TodoUserData)

					While reader.Read()

						Dim user As New TodoUserData

						user.UserNumber = SafeGetInteger(reader, "USNr", 0)
						user.LastName = SafeGetString(reader, "Nachname")
						user.FirstName = SafeGetString(reader, "Vorname")


						result.Add(user)

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