
Imports SP.DatabaseAccess.CVLizer.DataObjects


Namespace CVLizer

	Partial Class CVLizerDatabaseAccess

		Inherits DatabaseAccessBase
		Implements ICVLizerDatabaseAccess


#Region "additional information methodes"


#Region "viewing data"

		Function LoadCVLAdditionalInfoViewData(ByVal cvlPrifleID As Integer?, ByVal addID As Integer) As AdditionalInfoViewData Implements ICVLizerDatabaseAccess.LoadCVLAdditionalInfoViewData
			Dim result As AdditionalInfoViewData = Nothing

			Dim sql = "[Load Assigned CVL Additional Information Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			m_Utility = New SP.Infrastructure.Utility


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@CVLProfileID", ReplaceMissing(cvlPrifleID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@AddID", ReplaceMissing(addID, 0)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New AdditionalInfoViewData

					While reader.Read

						result.ID = SafeGetInteger(reader, "ID", Nothing)

						result.MilitaryService = SafeGetBoolean(reader, "MilitaryService", Nothing)
						result.Competences = SafeGetString(reader, "Competences")
						result.Additionals = SafeGetString(reader, "Additionals")
						result.Interests = SafeGetString(reader, "Interests")

						result.Languages = LoadAssignedCVLAddtioinalLanguageData(addID)
						result.DrivingLicences = LoadAssignedCVLAdditionalDrivingLicenceViewData(addID)
						result.UndatedSkills = LoadAssignedCVLAddtioinalUndatedSkillViewData(addID)
						result.UndatedOperationArea = LoadAssignedCVLAddtioinalUndatedOperationAreaViewData(addID)
						result.UndatedIndustries = LoadAssignedCVLAddtioinalUndatedIndustryViewData(addID)

						result.InternetResources = LoadAssignedCVLAddtioinalInternetResourceViewData(addID)


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



		Function LoadAssignedCVLAdditionalDrivingLicenceViewData(ByVal addID As Integer) As IEnumerable(Of CodeViewData) Implements ICVLizerDatabaseAccess.LoadAssignedCVLAdditionalDrivingLicenceViewData
			Dim result As List(Of CodeViewData) = Nothing

			Dim sql = "[Load Assigned CVL Additional DrivingLicence Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@AddID", ReplaceMissing(addID, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of CodeViewData)

					While reader.Read
						Dim data = New CodeViewData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.Lable = SafeGetString(reader, "DrivingLicence")


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

		Function LoadAssignedCVLAddtioinalUndatedSkillViewData(ByVal addID As Integer) As IEnumerable(Of CodeNameWeightViewData) Implements ICVLizerDatabaseAccess.LoadAssignedCVLAddtioinalUndatedSkillViewData
			Dim result As List(Of CodeNameWeightViewData) = Nothing

			Dim sql = "[Load Assigned CVL Additional Undated Skill Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@AddID", ReplaceMissing(addID, DBNull.Value)))


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

		Function LoadAssignedCVLAddtioinalUndatedOperationAreaViewData(ByVal addID As Integer) As IEnumerable(Of CodeNameWeightViewData) Implements ICVLizerDatabaseAccess.LoadAssignedCVLAddtioinalUndatedOperationAreaViewData
			Dim result As List(Of CodeNameWeightViewData) = Nothing

			Dim sql = "[Load Assigned CVL Additional Undated OperationArea Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@AddID", ReplaceMissing(addID, DBNull.Value)))


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

		Function LoadAssignedCVLAddtioinalUndatedIndustryViewData(ByVal addID As Integer) As IEnumerable(Of CodeNameWeightViewData) Implements ICVLizerDatabaseAccess.LoadAssignedCVLAddtioinalUndatedIndustryViewData
			Dim result As List(Of CodeNameWeightViewData) = Nothing

			Dim sql = "[Load Assigned CVL Additional Undated Industry Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@AddID", ReplaceMissing(addID, DBNull.Value)))


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

		Function LoadAssignedCVLAddtioinalInternetResourceViewData(ByVal addID As Integer) As IEnumerable(Of InternetResourceViewData) Implements ICVLizerDatabaseAccess.LoadAssignedCVLAddtioinalInternetResourceViewData
			Dim result As List(Of InternetResourceViewData) = Nothing

			Dim sql = "[Load Assigned CVL Additional InternetResource Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@AddID", ReplaceMissing(addID, DBNull.Value)))


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

		Function LoadAssignedCVLAddtioinalLanguageData(ByVal addID As Integer) As IEnumerable(Of LanguageData) Implements ICVLizerDatabaseAccess.LoadAssignedCVLAddtioinalLanguageData
			Dim result As List(Of LanguageData) = Nothing

			Dim sql = "[Load Assigned CVL Additional Language Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@AddID", ReplaceMissing(addID, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of LanguageData)

					While reader.Read
						Dim data = New LanguageData

						data.Code = SafeGetString(reader, "FK_LanguageCode")
						data.CodeName = SafeGetString(reader, "LanguageLable")

						Dim levelCode As String = String.Empty
						Dim levelName As String = String.Empty

						levelCode = SafeGetString(reader, "FK_LanguageLevelCode")
						levelName = SafeGetString(reader, "LanguageLevelLable")
						data.Level = New CodeNameData With {.Code = levelCode, .CodeName = levelName}


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

		Function AddCVLAdditionalInformationData(ByVal cvlProfileID As Integer, ByVal cvAddData As OtherInformationData) As Boolean Implements ICVLizerDatabaseAccess.AddCVLAdditionalInformationData
			Dim success As Boolean

			Dim sql = "[CreateCVLAdditionalInformation]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@CVLProfileID", ReplaceMissing(cvlProfileID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MilitaryService", ReplaceMissing(cvAddData.MilitaryService, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Competences", ReplaceMissing(cvAddData.Competences, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Additionals", ReplaceMissing(cvAddData.Additionals, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Interests", ReplaceMissing(cvAddData.Interests, DBNull.Value)))


			' Output Parameters
			Dim newAdditioalInfoIdParameter = New SqlClient.SqlParameter("@NewAdditioalInfoId", SqlDbType.Int)
			newAdditioalInfoIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newAdditioalInfoIdParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If Not newAdditioalInfoIdParameter.Value Is Nothing Then
				cvAddData.ID = CType(newAdditioalInfoIdParameter.Value, Integer)
			Else
				success = False
			End If

			Return success
		End Function

		Function AddCVLAdditionalLanguageData(ByVal publicationPhaseID As Integer, ByVal lang As LanguageData) As Boolean Implements ICVLizerDatabaseAccess.AddCVLAdditionalLanguageData
			Dim success As Boolean

			Dim sql = "[CreateCVLAdditionalLanguage]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@CVLAdditionalID", ReplaceMissing(publicationPhaseID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LanguageCode", ReplaceMissing(lang.Code, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LanguageLevelCode", ReplaceMissing(lang.Level.Code, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function

		Function AddCVLAdditionalDriverLicenceData(ByVal publicationPhaseID As Integer, ByVal dLicence As String) As Boolean Implements ICVLizerDatabaseAccess.AddCVLAdditionalDriverLicenceData
			Dim success As Boolean

			Dim sql = "[CreateCVLAdditionalDriverLicence]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@CVLAdditionalID", ReplaceMissing(publicationPhaseID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@DrivingLicence", ReplaceMissing(dLicence, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function

		Function AddCVLAdditionalUndateSkillData(ByVal publicationPhaseID As Integer, ByVal data As CodeNameWeightedData) As Boolean Implements ICVLizerDatabaseAccess.AddCVLAdditionalUndateSkillData
			Dim success As Boolean

			Dim sql = "[CreateCVLAdditionalUSkill]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@CVLAdditionalID", ReplaceMissing(publicationPhaseID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Code", ReplaceMissing(data.Code, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@CodeName", ReplaceMissing(data.Name, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Weight", ReplaceMissing(data.Weight, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function

		Function AddCVLAdditionalUndatedOperationAreaData(ByVal publicationPhaseID As Integer, ByVal data As CodeNameWeightedData) As Boolean Implements ICVLizerDatabaseAccess.AddCVLAdditionalUndatedOperationAreaData
			Dim success As Boolean

			Dim sql = "[CreateCVLAdditionalUOperationArea]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@CVLAdditionalID", ReplaceMissing(publicationPhaseID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Code", ReplaceMissing(data.Code, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@CodeName", ReplaceMissing(data.Name, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Weight", ReplaceMissing(data.Weight, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function

		Function AddCVLAdditionalUndatedIndustryData(ByVal publicationPhaseID As Integer, ByVal data As CodeNameWeightedData) As Boolean Implements ICVLizerDatabaseAccess.AddCVLAdditionalUndatedIndustryData
			Dim success As Boolean

			Dim sql = "[CreateCVLAdditionalUIndustry]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@CVLAdditionalID", ReplaceMissing(publicationPhaseID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Code", ReplaceMissing(data.Code, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@CodeName", ReplaceMissing(data.Name, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Weight", ReplaceMissing(data.Weight, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function

		Function AddCVLAdditionalInternetResourceData(ByVal publicationPhaseID As Integer, ByVal data As InternetResource) As Boolean Implements ICVLizerDatabaseAccess.AddCVLAdditionalInternetResourceData
			Dim success As Boolean

			Dim sql = "[CreateCVLAdditionalInternetResource]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@CVLAdditionalID", ReplaceMissing(publicationPhaseID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Url", ReplaceMissing(data.URL, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Title", ReplaceMissing(data.Title, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Source", ReplaceMissing(data.Source, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Snippet", ReplaceMissing(data.Snippet, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function



#End Region


#End Region


	End Class


End Namespace
