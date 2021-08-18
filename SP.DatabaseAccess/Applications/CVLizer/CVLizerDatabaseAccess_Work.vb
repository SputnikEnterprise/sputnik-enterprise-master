
Imports SP.DatabaseAccess.CVLizer.DataObjects


Namespace CVLizer

	Partial Class CVLizerDatabaseAccess

		Inherits DatabaseAccessBase
		Implements ICVLizerDatabaseAccess



#Region "work methodes"


#Region "viewing data"

		Function LoadCVLWorkPhaseViewData(ByVal cvlPrifleID As Integer?, ByVal workID As Integer) As IEnumerable(Of WorkPhaseViewData) Implements ICVLizerDatabaseAccess.LoadCVLWorkPhaseViewData
			Dim result As List(Of WorkPhaseViewData) = Nothing

			Dim sql = "[Load Assigned CVL Work Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@CVLProfileID", ReplaceMissing(cvlPrifleID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@WorkID", ReplaceMissing(workID, 0)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of WorkPhaseViewData)

					While reader.Read
						Dim data = New WorkPhaseViewData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.WorkID = SafeGetInteger(reader, "FK_WorkID", Nothing)
						data.PhaseID = SafeGetInteger(reader, "FK_PhasesID", Nothing)

						data.DateFromFuzzy = SafeGetString(reader, "DateFromFuzzy")
						data.DateToFuzzy = SafeGetString(reader, "DateToFuzzy")

						data.Duration = SafeGetInteger(reader, "Duration", Nothing)
						data.Current = SafeGetBoolean(reader, "Current", Nothing)
						data.SubPhase = SafeGetBoolean(reader, "SubPhase", Nothing)
						data.Comments = SafeGetString(reader, "Comments")
						data.PlainText = SafeGetString(reader, "PlainText")

						data.DateFrom = SafeGetDateTime(reader, "DateFrom", Nothing)
						data.DateTo = SafeGetDateTime(reader, "DateTo", Nothing)

						data.Locations = LoadAssignedCVLWorkPhaseAddressViewData(data.PhaseID)
						data.Skills = LoadAssignedCVLWorkPhaseSkillViewData(data.PhaseID)
						data.SoftSkills = LoadAssignedCVLWorkPhaseSoftSkillViewData(data.PhaseID)
						data.OperationAreas = LoadAssignedCVLWorkPhaseOperationAreaViewData(data.PhaseID)
						data.Industries = LoadAssignedCVLWorkPhaseIndustryViewData(data.PhaseID)
						data.CustomCodes = LoadAssignedCVLWorkPhaseCustomCodeViewData(data.PhaseID)
						data.Topic = LoadAssignedCVLWorkPhaseTopicViewData(data.PhaseID)
						data.InternetResources = LoadAssignedCVLWorkPhaseInternetResourceViewData(data.PhaseID)
						data.DocumentID = LoadAssignedCVLWorkPhaseDocumentIDViewData(data.PhaseID)

						data.Companies = LoadAssignedCVLWorkPhaseCompanyViewData(data.ID)
						data.Functions = LoadAssignedCVLWorkPhaseFunctionViewData(data.ID)
						data.Positions = LoadAssignedCVLWorkPhasePositionViewData(data.ID)
						data.Project = SafeGetBoolean(reader, "Project", Nothing)
						data.Employments = LoadAssignedCVLWorkPhaseEmploymentViewData(data.ID)
						data.WorkTimes = LoadAssignedCVLWorkPhaseWorktimeViewData(data.ID)


						result.Add(data)

					End While

				End If

			Catch e As Exception
				m_Logger.LogError(e.ToString())
				result = Nothing
			Finally
				CloseReader(reader)
			End Try


			Return result
		End Function


		Function LoadAssignedCVLWorkPhaseCompanyViewData(ByVal workPhaseID As Integer) As IEnumerable(Of CodeViewData) Implements ICVLizerDatabaseAccess.LoadAssignedCVLWorkPhaseCompanyViewData
			Dim result As List(Of CodeViewData) = Nothing

			Dim sql = "[Load Assigned CVL Work Company Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@WorkPhaseID", ReplaceMissing(workPhaseID, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of CodeViewData)

					While reader.Read
						Dim data = New CodeViewData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.Lable = SafeGetString(reader, "Company")


						result.Add(data)

					End While

				End If

			Catch e As Exception
				m_Logger.LogError(e.ToString())
				result = Nothing
			Finally
				CloseReader(reader)
			End Try


			Return result
		End Function

		Function LoadAssignedCVLWorkPhaseFunctionViewData(ByVal workPhaseID As Integer) As IEnumerable(Of CodeViewData) Implements ICVLizerDatabaseAccess.LoadAssignedCVLWorkPhaseFunctionViewData
			Dim result As List(Of CodeViewData) = Nothing

			Dim sql = "[Load Assigned CVL Work Function Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@WorkPhaseID", ReplaceMissing(workPhaseID, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of CodeViewData)

					While reader.Read
						Dim data = New CodeViewData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.Lable = SafeGetString(reader, "Function")


						result.Add(data)

					End While

				End If

			Catch e As Exception
				m_Logger.LogError(e.ToString())
				result = Nothing
			Finally
				CloseReader(reader)
			End Try


			Return result
		End Function

		Function LoadAssignedCVLWorkPhasePositionViewData(ByVal workPhaseID As Integer) As IEnumerable(Of CodeNameViewData) Implements ICVLizerDatabaseAccess.LoadAssignedCVLWorkPhasePositionViewData
			Dim result As List(Of CodeNameViewData) = Nothing

			Dim sql = "[Load Assigned CVL Work Position Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@WorkPhaseID", ReplaceMissing(workPhaseID, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of CodeNameViewData)

					While reader.Read
						Dim data = New CodeNameViewData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.Code = SafeGetString(reader, "Code")
						data.Name = SafeGetString(reader, "Name")


						result.Add(data)

					End While

				End If

			Catch e As Exception
				m_Logger.LogError(e.ToString())
				result = Nothing
			Finally
				CloseReader(reader)
			End Try


			Return result
		End Function

		Function LoadAssignedCVLWorkPhaseEmploymentViewData(ByVal workPhaseID As Integer) As IEnumerable(Of CodeNameViewData) Implements ICVLizerDatabaseAccess.LoadAssignedCVLWorkPhaseEmploymentViewData
			Dim result As List(Of CodeNameViewData) = Nothing

			Dim sql = "[Load Assigned CVL Work Employment Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@WorkPhaseID", ReplaceMissing(workPhaseID, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of CodeNameViewData)

					While reader.Read
						Dim data = New CodeNameViewData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.Code = SafeGetString(reader, "Code")
						data.Name = SafeGetString(reader, "Name")


						result.Add(data)

					End While

				End If

			Catch e As Exception
				m_Logger.LogError(e.ToString())
				result = Nothing
			Finally
				CloseReader(reader)
			End Try


			Return result
		End Function

		Function LoadAssignedCVLWorkPhaseWorktimeViewData(ByVal workPhaseID As Integer) As IEnumerable(Of CodeNameViewData) Implements ICVLizerDatabaseAccess.LoadAssignedCVLWorkPhaseWorktimeViewData
			Dim result As List(Of CodeNameViewData) = Nothing

			Dim sql = "[Load Assigned CVL Work Worktime Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@WorkPhaseID", ReplaceMissing(workPhaseID, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of CodeNameViewData)

					While reader.Read
						Dim data = New CodeNameViewData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.Code = SafeGetString(reader, "Code")
						data.Name = SafeGetString(reader, "Name")


						result.Add(data)

					End While

				End If

			Catch e As Exception
				m_Logger.LogError(e.ToString())
				result = Nothing
			Finally
				CloseReader(reader)
			End Try


			Return result
		End Function



#End Region


#Region "adding methodes"

		Function AddCVLWorkPhaseData(ByVal workID As Integer, ByVal phaseID As Integer, ByVal cvlWorkPhaseData As WorkPhaseData) As Boolean Implements ICVLizerDatabaseAccess.AddCVLWorkPhaseData
			Dim success As Boolean

			Dim sql = "[CreateCVLWorkPhase]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)

			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@CVLWorkID", ReplaceMissing(workID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@PhaseID", ReplaceMissing(phaseID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Project", ReplaceMissing(cvlWorkPhaseData.Project, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Companies", If(cvlWorkPhaseData.Company Is Nothing, DBNull.Value, ReplaceMissing(String.Join("#", cvlWorkPhaseData.Company.ToArray()), DBNull.Value))))
			listOfParams.Add(New SqlClient.SqlParameter("@Functions", If(cvlWorkPhaseData.Functions Is Nothing, DBNull.Value, ReplaceMissing(String.Join("#", cvlWorkPhaseData.Functions.ToArray()), DBNull.Value))))


			' Output Parameters
			Dim newWorkPhaseIdParameter = New SqlClient.SqlParameter("@NewWorkPhaseId", SqlDbType.Int)
			newWorkPhaseIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newWorkPhaseIdParameter)


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If Not newWorkPhaseIdParameter.Value Is Nothing Then
				cvlWorkPhaseData.WorkPhaseID = CType(newWorkPhaseIdParameter.Value, Integer)
			Else
				success = False
			End If

			Return success
		End Function

		Function AddCVLWorkPhasePositionData(ByVal WorkPhaseID As Integer, ByVal data As CodeNameData) As Boolean Implements ICVLizerDatabaseAccess.AddCVLWorkPhasePositionData
			Dim success As Boolean

			Dim sql = "[CreateCVLWorkPhasePosition]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@WorkPhasesID", ReplaceMissing(WorkPhaseID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Code", ReplaceMissing(data.Code, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@CodeName", ReplaceMissing(data.CodeName, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function

		Function AddCVLWorkPhaseEmploymentData(ByVal WorkPhaseID As Integer, ByVal data As CodeNameData) As Boolean Implements ICVLizerDatabaseAccess.AddCVLWorkPhaseEmploymentData
			Dim success As Boolean

			Dim sql = "[CreateCVLWorkPhaseEmployment]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@WorkPhasesID", ReplaceMissing(WorkPhaseID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Code", ReplaceMissing(data.Code, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@CodeName", ReplaceMissing(data.CodeName, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function

		Function AddCVLWorkPhaseWorktimeData(ByVal WorkPhaseID As Integer, ByVal data As CodeNameData) As Boolean Implements ICVLizerDatabaseAccess.AddCVLWorkPhaseWorktimeData
			Dim success As Boolean

			Dim sql = "[CreateCVLWorkPhaseWorktime]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@WorkPhasesID", ReplaceMissing(WorkPhaseID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Code", ReplaceMissing(data.Code, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@CodeName", ReplaceMissing(data.CodeName, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function

#End Region


#End Region



	End Class


End Namespace
