
Imports SP.DatabaseAccess.CVLizer.DataObjects


Namespace CVLizer

	Partial Class CVLizerDatabaseAccess

		Inherits DatabaseAccessBase
		Implements ICVLizerDatabaseAccess


#Region "public methodes Loading"


#Region "viewing data"

		Function LoadAssignedCVLWorkPhaseAddressViewData(ByVal phaseID As Integer) As IEnumerable(Of AddressViewData) Implements ICVLizerDatabaseAccess.LoadAssignedCVLWorkPhaseAddressViewData
			Dim result As List(Of AddressViewData) = Nothing

			Dim sql = "[Load Assigned CVL Phase Location Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@PhaseID", ReplaceMissing(phaseID, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of AddressViewData)

					While reader.Read
						Dim data = New AddressViewData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.Street = SafeGetString(reader, "Street")
						data.Postcode = SafeGetString(reader, "Postcode")
						data.City = SafeGetString(reader, "City")
						data.Country = SafeGetString(reader, "FK_CountryCode")
						data.CountryLable = SafeGetString(reader, "CountryLable")
						data.State = SafeGetString(reader, "State")


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

		Function LoadAssignedCVLWorkPhaseSkillViewData(ByVal phaseID As Integer) As IEnumerable(Of CodeNameWeightViewData) Implements ICVLizerDatabaseAccess.LoadAssignedCVLWorkPhaseSkillViewData
			Dim result As List(Of CodeNameWeightViewData) = Nothing

			Dim sql = "[Load Assigned CVL Phase Skill Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@PhaseID", ReplaceMissing(phaseID, DBNull.Value)))


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

		Function LoadAssignedCVLWorkPhaseSoftSkillViewData(ByVal phaseID As Integer) As IEnumerable(Of CodeNameWeightViewData) Implements ICVLizerDatabaseAccess.LoadAssignedCVLWorkPhaseSoftSkillViewData
			Dim result As List(Of CodeNameWeightViewData) = Nothing

			Dim sql = "[Load Assigned CVL Phase SoftSkill Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@PhaseID", ReplaceMissing(phaseID, DBNull.Value)))


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

		Function LoadAssignedCVLWorkPhaseOperationAreaViewData(ByVal phaseID As Integer) As IEnumerable(Of CodeNameWeightViewData) Implements ICVLizerDatabaseAccess.LoadAssignedCVLWorkPhaseOperationAreaViewData
			Dim result As List(Of CodeNameWeightViewData) = Nothing

			Dim sql = "[Load Assigned CVL Phase OperationArea Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@PhaseID", ReplaceMissing(phaseID, DBNull.Value)))


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

		Function LoadAssignedCVLWorkPhaseIndustryViewData(ByVal phaseID As Integer) As IEnumerable(Of CodeNameWeightViewData) Implements ICVLizerDatabaseAccess.LoadAssignedCVLWorkPhaseIndustryViewData
			Dim result As List(Of CodeNameWeightViewData) = Nothing

			Dim sql = "[Load Assigned CVL Phase Industry Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@PhaseID", ReplaceMissing(phaseID, DBNull.Value)))


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

		Function LoadAssignedCVLWorkPhaseCustomCodeViewData(ByVal phaseID As Integer) As IEnumerable(Of CodeNameWeightViewData) Implements ICVLizerDatabaseAccess.LoadAssignedCVLWorkPhaseCustomCodeViewData
			Dim result As List(Of CodeNameWeightViewData) = Nothing

			Dim sql = "[Load Assigned CVL Phase CustomCode Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@PhaseID", ReplaceMissing(phaseID, DBNull.Value)))


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

		Function LoadAssignedCVLWorkPhaseTopicViewData(ByVal phaseID As Integer) As IEnumerable(Of CodeViewData) Implements ICVLizerDatabaseAccess.LoadAssignedCVLWorkPhaseTopicViewData
			Dim result As List(Of CodeViewData) = Nothing

			Dim sql = "[Load Assigned CVL Phase Topic Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@PhaseID", ReplaceMissing(phaseID, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of CodeViewData)

					While reader.Read
						Dim data = New CodeViewData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.Lable = SafeGetString(reader, "Name")


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

		Function LoadAssignedCVLWorkPhaseInternetResourceViewData(ByVal phaseID As Integer) As IEnumerable(Of InternetResourceViewData) Implements ICVLizerDatabaseAccess.LoadAssignedCVLWorkPhaseInternetResourceViewData
			Dim result As List(Of InternetResourceViewData) = Nothing

			Dim sql = "[Load Assigned CVL Phase InternetResource Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@PhaseID", ReplaceMissing(phaseID, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of InternetResourceViewData)

					While reader.Read
						Dim data = New InternetResourceViewData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.URL = SafeGetString(reader, "URL")
						data.Title = SafeGetString(reader, "Title")
						data.Source = SafeGetString(reader, "Source")
						data.Snippet = SafeGetString(reader, "Snippet")


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

		Function LoadAssignedCVLWorkPhaseDocumentIDViewData(ByVal phaseID As Integer) As IEnumerable(Of IDiewData) Implements ICVLizerDatabaseAccess.LoadAssignedCVLWorkPhaseDocumentIDViewData
			Dim result As List(Of IDiewData) = Nothing

			Dim sql = "[Load Assigned CVL Phase DocumentID Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@PhaseID", ReplaceMissing(phaseID, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of IDiewData)

					While reader.Read
						Dim data = New IDiewData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.CodeNumber = SafeGetInteger(reader, "Code", Nothing)


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

		Function AddCVLPhaseData(ByVal profileID As Integer, ByVal cvlPhaseData As Phase) As Boolean Implements ICVLizerDatabaseAccess.AddCVLPhaseData
			Dim success As Boolean

			Dim sql = "[CreateCVLPhase]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@DateFrom", ReplaceMissing(cvlPhaseData.DateFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@DateTo", ReplaceMissing(cvlPhaseData.DateTo, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@DateFromFuzzy", ReplaceMissing(cvlPhaseData.DateFromFuzzy, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@DateToFuzzy", ReplaceMissing(cvlPhaseData.DateToFuzzy, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Duration", ReplaceMissing(cvlPhaseData.Duration, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@IsCurrent", ReplaceMissing(cvlPhaseData.Current, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@SubPhase", ReplaceMissing(cvlPhaseData.SubPhase, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Comments", ReplaceMissing(cvlPhaseData.Comments, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@PlainText", ReplaceMissing(cvlPhaseData.PlainText, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Topics", If(cvlPhaseData.Topic Is Nothing, DBNull.Value, ReplaceMissing(String.Join("#", cvlPhaseData.Topic.ToArray()), DBNull.Value))))
			listOfParams.Add(New SqlClient.SqlParameter("@DocumentID", If(cvlPhaseData.DocumentID Is Nothing, DBNull.Value, ReplaceMissing(String.Join("#", cvlPhaseData.DocumentID.ToArray()), DBNull.Value))))

			' Output Parameters
			Dim newIdParameter = New SqlClient.SqlParameter("@NewPhaseId", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If Not newIdParameter.Value Is Nothing Then
				cvlPhaseData.PhaseID = CType(newIdParameter.Value, Integer)
			Else
				success = False
			End If

			Return success
		End Function

		Function AddCVLPhaseLocationData(ByVal phaseID As Integer, ByVal cvlLocationData As AddressData) As Boolean Implements ICVLizerDatabaseAccess.AddCVLPhaseLocationData
			Dim success As Boolean

			Dim sql = "[CreateCVLPhaseLocation]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@PhasesID", ReplaceMissing(phaseID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Street", ReplaceMissing(cvlLocationData.Street, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@PostCode", ReplaceMissing(cvlLocationData.Postcode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@City", ReplaceMissing(cvlLocationData.City, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@CountryCode", ReplaceMissing(cvlLocationData.Country.Code, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@AddressState", ReplaceMissing(cvlLocationData.State, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function

		Function AddCVLPhaseSkillData(ByVal phaseID As Integer, ByVal data As CodeNameWeightedData) As Boolean Implements ICVLizerDatabaseAccess.AddCVLPhaseSkillData
			Dim success As Boolean

			Dim sql = "[CreateCVLPhaseSkill]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@PhasesID", ReplaceMissing(phaseID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Code", ReplaceMissing(data.Code, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@CodeName", ReplaceMissing(data.Name, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Weight", ReplaceMissing(data.Weight, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function

		Function AddCVLPhaseSoftSkillData(ByVal phaseID As Integer, ByVal data As CodeNameWeightedData) As Boolean Implements ICVLizerDatabaseAccess.AddCVLPhaseSoftSkillData
			Dim success As Boolean

			Dim sql = "[CreateCVLPhaseSoftSkill]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@PhasesID", ReplaceMissing(phaseID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Code", ReplaceMissing(data.Code, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@CodeName", ReplaceMissing(data.Name, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Weight", ReplaceMissing(data.Weight, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function

		Function AddCVLPhaseOperationAreaData(ByVal phaseID As Integer, ByVal data As CodeNameWeightedData) As Boolean Implements ICVLizerDatabaseAccess.AddCVLPhaseOperationAreaData
			Dim success As Boolean

			Dim sql = "[CreateCVLPhaseOperationArea]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@PhasesID", ReplaceMissing(phaseID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Code", ReplaceMissing(data.Code, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@CodeName", ReplaceMissing(data.Name, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Weight", ReplaceMissing(data.Weight, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function

		Function AddCVLPhaseIndustryData(ByVal phaseID As Integer, ByVal data As CodeNameWeightedData) As Boolean Implements ICVLizerDatabaseAccess.AddCVLPhaseIndustryData
			Dim success As Boolean

			Dim sql = "[CreateCVLPhaseIndustry]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@PhasesID", ReplaceMissing(phaseID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Code", ReplaceMissing(data.Code, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@CodeName", ReplaceMissing(data.Name, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Weight", ReplaceMissing(data.Weight, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function

		Function AddCVLPhaseCustomCodeData(ByVal phaseID As Integer, ByVal data As CodeNameWeightedData) As Boolean Implements ICVLizerDatabaseAccess.AddCVLPhaseCustomCodeData
			Dim success As Boolean

			Dim sql = "[CreateCVLPhaseCustomCode]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@PhasesID", ReplaceMissing(phaseID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Code", ReplaceMissing(data.Code, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@CodeName", ReplaceMissing(data.Name, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Weight", ReplaceMissing(data.Weight, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function

		Function AddCVLPhaseInternetResourceData(ByVal phaseID As Integer, ByVal data As InternetResource) As Boolean Implements ICVLizerDatabaseAccess.AddCVLPhaseInternetResourceData
			Dim success As Boolean

			Dim sql = "[CreateCVLPhaseInternetResource]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@PhasesID", ReplaceMissing(phaseID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Url", ReplaceMissing(data.URL, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Title", ReplaceMissing(data.Title, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Source", ReplaceMissing(data.Source, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Snippet", ReplaceMissing(data.Snippet, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function



#Region "Search Expiriences"

		Function AddCVLExperiencesData(ByVal profileID As Integer, ByVal exData As CVLExperiencesData) As Boolean Implements ICVLizerDatabaseAccess.AddCVLExperiencesData
			Dim success As Boolean

			Dim sql = "[CreateCVLExperiences]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@CustomerID", ReplaceMissing(exData.CustomerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ProfileID", ReplaceMissing(exData.ProfileID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@DateFrom", ReplaceMissing(exData.DateFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@DateTo", ReplaceMissing(exData.DateTo, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Duration", ReplaceMissing(exData.Duration, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Code", ReplaceMissing(exData.Code, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Skills", ReplaceMissing(exData.ExperiencesKind = ExperiencesEnum.SKILLS, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("OpAreas", ReplaceMissing(exData.ExperiencesKind = ExperiencesEnum.OPERATIONAREA, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("JobTitel", ReplaceMissing(exData.ExperiencesKind = ExperiencesEnum.JOBFUNCTIONS, DBNull.Value)))

			' Output Parameters
			Dim newIdParameter = New SqlClient.SqlParameter("@NewId", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If Not newIdParameter.Value Is Nothing Then
				exData.ID = CType(newIdParameter.Value, Integer)
			Else
				success = False
			End If

			Return success
		End Function

#End Region


#End Region


#End Region


	End Class


End Namespace
