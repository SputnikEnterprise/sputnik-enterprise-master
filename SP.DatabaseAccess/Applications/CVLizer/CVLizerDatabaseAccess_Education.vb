
'Imports System.Drawing
Imports System.IO
Imports SP.DatabaseAccess.CVLizer.DataObjects


Namespace CVLizer

	Partial Class CVLizerDatabaseAccess

		Inherits DatabaseAccessBase
		Implements ICVLizerDatabaseAccess

		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As SP.Infrastructure.Utility

#Region "education methodes"


#Region "viewing data"

		Function LoadCVLEducationPhaseViewData(ByVal cvlPrifleID As Integer?, ByVal educationID As Integer) As IEnumerable(Of EducationPhaseViewData) Implements ICVLizerDatabaseAccess.LoadCVLEducationPhaseViewData
			Dim result As List(Of EducationPhaseViewData) = Nothing

			Dim sql = "[Load Assigned CVL Education Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			m_Utility = New SP.Infrastructure.Utility


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@CVLProfileID", ReplaceMissing(cvlPrifleID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@EducationID", ReplaceMissing(educationID, 0)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of EducationPhaseViewData)

					While reader.Read
						Dim data = New EducationPhaseViewData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.EducationPhaseID = SafeGetInteger(reader, "FK_EducationID", Nothing)
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

						data.IsCedCodeLable = SafeGetString(reader, "IsCedCodeLable")
						data.Completed = SafeGetBoolean(reader, "Completed", Nothing)
						data.Score = SafeGetInteger(reader, "Score", Nothing)
						data.SchooolNames = LoadAssignedCVLEducationPhaseSchoolnameViewData(data.ID)
						data.Graduations = LoadAssignedCVLEducationPhaseGraduationViewData(data.ID)
						data.EducationTypes = LoadAssignedCVLEducationPhaseEducationTypeViewData(data.ID)


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


		Private Function LoadAssignedCVLEducationPhaseSchoolnameViewData(ByVal educationPhaseID As Integer) As IEnumerable(Of CodeViewData)
			Dim result As List(Of CodeViewData) = Nothing

			Dim sql = "[Load Assigned CVL Phase Schoolname Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@EducationPhaseID", ReplaceMissing(educationPhaseID, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of CodeViewData)

					While reader.Read
						Dim data = New CodeViewData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.Lable = SafeGetString(reader, "Schoolname")


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

		Private Function LoadAssignedCVLEducationPhaseGraduationViewData(ByVal educationPhaseID As Integer) As IEnumerable(Of CodeViewData)
			Dim result As List(Of CodeViewData) = Nothing

			Dim sql = "[Load Assigned CVL Phase Graduation Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@EducationPhaseID", ReplaceMissing(educationPhaseID, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of CodeViewData)

					While reader.Read
						Dim data = New CodeViewData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.Lable = SafeGetString(reader, "Graduations")


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

		Private Function LoadAssignedCVLEducationPhaseEducationTypeViewData(ByVal educationPhaseID As Integer) As IEnumerable(Of CodeNameWeightViewData)
			Dim result As List(Of CodeNameWeightViewData) = Nothing

			Dim sql = "[Load Assigned CVL Phase EducationType Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@EducationPhaseID", ReplaceMissing(educationPhaseID, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of CodeNameWeightViewData)

					While reader.Read
						Dim data = New CodeNameWeightViewData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.Code = SafeGetString(reader, "Code")
						data.Name = SafeGetString(reader, "Name")
						data.Weight = SafeGetDecimal(reader, "Weight", Nothing)


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


#Region "adding methodes"

		Function AddCVLEducationPhaseData(ByVal educationID As Integer, ByVal phaseID As Integer, ByVal cvlEducationPhaseData As EducationPhaseData) As Boolean Implements ICVLizerDatabaseAccess.AddCVLEducationPhaseData
			Dim success As Boolean

			Dim sql = "[CreateCVLEducationPhase]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)

			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@CVLEducationID", ReplaceMissing(educationID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@PhaseID", ReplaceMissing(phaseID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@IsCedCode", ReplaceMissing(cvlEducationPhaseData.IsCed.Code, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Completed", ReplaceMissing(cvlEducationPhaseData.Completed, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Score", ReplaceMissing(cvlEducationPhaseData.Score, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@SchoolName", If(cvlEducationPhaseData.SchoolName Is Nothing, DBNull.Value, ReplaceMissing(String.Join("#", cvlEducationPhaseData.SchoolName.ToArray()), DBNull.Value))))
			listOfParams.Add(New SqlClient.SqlParameter("@Graduation", If(cvlEducationPhaseData.Graduation Is Nothing, DBNull.Value, ReplaceMissing(String.Join("#", cvlEducationPhaseData.Graduation.ToArray()), DBNull.Value))))


			' Output Parameters
			Dim newEducationPhaseIdParameter = New SqlClient.SqlParameter("@NewEducationPhaseId", SqlDbType.Int)
			newEducationPhaseIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newEducationPhaseIdParameter)


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If Not newEducationPhaseIdParameter.Value Is Nothing Then
				cvlEducationPhaseData.EducationPhaseID = CType(newEducationPhaseIdParameter.Value, Integer)
			Else
				success = False
			End If

			Return success
		End Function

		Function AddCVLEducationPhaseEducationTypeData(ByVal educationPhaseID As Integer, ByVal data As CodeNameWeightedData) As Boolean Implements ICVLizerDatabaseAccess.AddCVLEducationPhaseEducationTypeData
			Dim success As Boolean

			Dim sql = "[CreateCVLEducationPhaseEducationType]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@EducPhasesID", ReplaceMissing(educationPhaseID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Code", ReplaceMissing(data.Code, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@CodeName", ReplaceMissing(data.Name, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Weight", ReplaceMissing(data.Weight, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function


#End Region

#End Region



	End Class


End Namespace
