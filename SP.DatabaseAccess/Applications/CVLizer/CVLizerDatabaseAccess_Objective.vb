
Imports SP.DatabaseAccess.CVLizer.DataObjects


Namespace CVLizer

	Partial Class CVLizerDatabaseAccess

		Inherits DatabaseAccessBase
		Implements ICVLizerDatabaseAccess


#Region "objective methodes"

		Function AddCVLObjectiveData(ByVal cvlProfileID As Integer, ByVal data As ObjectiveData) As Boolean Implements ICVLizerDatabaseAccess.AddCVLObjectiveData
			Dim success As Boolean

			Dim sql = "[CreateCVLObjective]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@CVLProfileID", ReplaceMissing(cvlProfileID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@AvailabilityDate", ReplaceMissing(data.AvailabilityDate, DBNull.Value)))


			' Output Parameters
			Dim newObjectiveIdParameter = New SqlClient.SqlParameter("@NewObjectiveId", SqlDbType.Int)
			newObjectiveIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newObjectiveIdParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If Not newObjectiveIdParameter.Value Is Nothing Then
				data.ID = CType(newObjectiveIdParameter.Value, Integer)
			Else
				success = False
			End If

			Return success
		End Function

		Function AddCVLObjectiveWorkPhaseData(ByVal objID As Integer, ByVal phaseID As Integer, ByVal cvlWorkPhaseData As WorkPhaseData) As Boolean Implements ICVLizerDatabaseAccess.AddCVLObjectiveWorkPhaseData
			Dim success As Boolean

			Dim sql = "[CreateCVLObjectivePhase]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)

			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@CVLObjID", ReplaceMissing(objID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@PhaseID", ReplaceMissing(phaseID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Project", ReplaceMissing(cvlWorkPhaseData.Project, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Companies", If(cvlWorkPhaseData.Company Is Nothing, DBNull.Value, ReplaceMissing(String.Join("#", cvlWorkPhaseData.Company.ToArray()), DBNull.Value))))
			listOfParams.Add(New SqlClient.SqlParameter("@Functions", If(cvlWorkPhaseData.Functions Is Nothing, DBNull.Value, ReplaceMissing(String.Join("#", cvlWorkPhaseData.Functions.ToArray()), DBNull.Value))))


			' Output Parameters
			Dim newObjPhaseIdParameter = New SqlClient.SqlParameter("@NewObjPhaseId", SqlDbType.Int)
			newObjPhaseIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newObjPhaseIdParameter)


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If Not newObjPhaseIdParameter.Value Is Nothing Then
				cvlWorkPhaseData.WorkPhaseID = CType(newObjPhaseIdParameter.Value, Integer)
			Else
				success = False
			End If

			Return success
		End Function

		Function AddCVLObjectivePhasePositionData(ByVal phaseID As Integer, ByVal data As CodeNameData) As Boolean Implements ICVLizerDatabaseAccess.AddCVLObjectivePhasePositionData
			Dim success As Boolean

			Dim sql = "[CreateCVLObjPhasePosition]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@ObjPhasesID", ReplaceMissing(phaseID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Code", ReplaceMissing(data.Code, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@CodeName", ReplaceMissing(data.CodeName, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function

		Function AddCVLObjectivePhaseEmploymentData(ByVal phaseID As Integer, ByVal data As CodeNameData) As Boolean Implements ICVLizerDatabaseAccess.AddCVLObjectivePhaseEmploymentData
			Dim success As Boolean

			Dim sql = "[CreateCVLObjPhaseEmployment]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@ObjPhasesID", ReplaceMissing(phaseID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Code", ReplaceMissing(data.Code, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@CodeName", ReplaceMissing(data.CodeName, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function

		Function AddCVLObjectivePhaseWorktimeData(ByVal phaseID As Integer, ByVal data As CodeNameData) As Boolean Implements ICVLizerDatabaseAccess.AddCVLObjectivePhaseWorktimeData
			Dim success As Boolean

			Dim sql = "[CreateCVLObjPhaseWorktime]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@ObjPhasesID", ReplaceMissing(phaseID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Code", ReplaceMissing(data.Code, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@CodeName", ReplaceMissing(data.CodeName, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function

		Function AddCVLObjectiveSalaryData(ByVal objID As Integer, ByVal salary As String) As Boolean Implements ICVLizerDatabaseAccess.AddCVLObjectiveSalaryData
			Dim success As Boolean

			Dim sql = "[CreateCVLObjectiveSalary]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@CVLObjID", ReplaceMissing(objID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@salary", ReplaceMissing(salary, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function






#End Region


	End Class


End Namespace
