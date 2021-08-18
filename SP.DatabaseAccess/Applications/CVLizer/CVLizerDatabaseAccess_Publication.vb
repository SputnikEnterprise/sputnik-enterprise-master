
Imports SP.DatabaseAccess.CVLizer.DataObjects


Namespace CVLizer

	Partial Class CVLizerDatabaseAccess

		Inherits DatabaseAccessBase
		Implements ICVLizerDatabaseAccess


#Region "publication methodes"


#Region "viewing data"

		Function LoadCVLPublicationViewData(ByVal cvlPrifleID As Integer?) As IEnumerable(Of PublicationViewData) Implements ICVLizerDatabaseAccess.LoadCVLPublicationViewData
			Dim result As List(Of PublicationViewData) = Nothing

			Dim sql = "[Load Assigned CVL Publication Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			m_Utility = New SP.Infrastructure.Utility


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@CVLProfileID", ReplaceMissing(cvlPrifleID, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of PublicationViewData)

					While reader.Read
						Dim data = New PublicationViewData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
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

						data.Proceedings = SafeGetString(reader, "Proceedings")
						data.Institute = SafeGetString(reader, "Institute")

						data.Author = LoadAssignedCVLPublicationAuthorsViewData(data.ID)


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


		Private Function LoadAssignedCVLPublicationAuthorsViewData(ByVal publicationID As Integer) As IEnumerable(Of CodeViewData)
			Dim result As List(Of CodeViewData) = Nothing

			Dim sql = "[Load Assigned CVL Publication Authors Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@publicationID", ReplaceMissing(publicationID, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of CodeViewData)

					While reader.Read
						Dim data = New CodeViewData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.Lable = SafeGetString(reader, "Authors")


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

		Function AddCVLPublicationPhaseData(ByVal cvlProfileID As Integer, ByVal phaseID As Integer, ByVal cvPublicationData As PublicationData) As Boolean Implements ICVLizerDatabaseAccess.AddCVLPublicationPhaseData
			Dim success As Boolean

			Dim sql = "[CreateCVLPublicationPhase]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@CVLProfileID", ReplaceMissing(cvlProfileID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@PhaseID", ReplaceMissing(phaseID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Proceedings", ReplaceMissing(cvPublicationData.Proceedings, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Institute", ReplaceMissing(cvPublicationData.Institute, DBNull.Value)))


			' Output Parameters
			Dim newPublicationPhaseIdParameter = New SqlClient.SqlParameter("@NewPublicationPhaseId", SqlDbType.Int)
			newPublicationPhaseIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newPublicationPhaseIdParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If Not newPublicationPhaseIdParameter.Value Is Nothing Then
				cvPublicationData.PublicationPhaseID = CType(newPublicationPhaseIdParameter.Value, Integer)
			Else
				success = False
			End If

			Return success
		End Function

		Function AddCVLPublicationPhaseAutorData(ByVal publicationPhaseID As Integer, ByVal autor As String) As Boolean Implements ICVLizerDatabaseAccess.AddCVLPublicationPhaseAutorData
			Dim success As Boolean

			Dim sql = "[CreateCVLPublicationPhaseAutor]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@CVLPubPhaseID", ReplaceMissing(publicationPhaseID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Autor", ReplaceMissing(autor, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function


#End Region

#End Region


	End Class


End Namespace
