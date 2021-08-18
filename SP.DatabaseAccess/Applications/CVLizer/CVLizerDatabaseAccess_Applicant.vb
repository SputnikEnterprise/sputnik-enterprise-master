
Imports SP.DatabaseAccess.CVLizer.DataObjects


Namespace CVLizer

	Partial Class CVLizerDatabaseAccess

		Inherits DatabaseAccessBase
		Implements ICVLizerDatabaseAccess


#Region "public methodes Loading"

		Function ExistsCVLFile(ByVal customerID As String, ByVal cvHashvalue As String) As Boolean Implements ICVLizerDatabaseAccess.ExistsCVLFile
			Dim result As Boolean?

			Dim sql = "[Load CVL File Hashvalues]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)

			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@CustomerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@cvHashvalue", ReplaceMissing(cvHashvalue, DBNull.Value)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					Return reader.HasRows

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			' Error case
			Return Nothing
		End Function

		Function AddCvPersonalInformationData(ByVal customerID As String, ByVal cvlXMLData As CVLizerXMLData, ByVal cvPersonalinformationData As PersonalInformationData) As Boolean Implements ICVLizerDatabaseAccess.AddCvPersonalInformationData
			Dim success As Boolean

			Dim sql = "[CreateCVLProfile]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)

			Dim nationalityBuffer As String = String.Empty
			For Each nationality In cvPersonalinformationData.Nationality
				'nationalityBuffer = nationalityBuffer & IIf(nationality.Code <> "", ", ", "") & nationality.Code
				nationalityBuffer = nationalityBuffer & IIf(String.IsNullOrWhiteSpace(nationalityBuffer), "", ", ") & nationality.Code
			Next

			Dim civilStateBuffer As String = String.Empty
			For Each civilState In cvPersonalinformationData.CivilStatus
				'civilStateBuffer = civilStateBuffer & IIf(civilState.Code <> "", ", ", "") & civilState.Code
				civilStateBuffer = civilStateBuffer & IIf(String.IsNullOrWhiteSpace(civilStateBuffer), "", ", ") & civilState.Code
			Next

			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@CustomerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@FirstName", ReplaceMissing(cvPersonalinformationData.FirstName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LastName", ReplaceMissing(cvPersonalinformationData.LastName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@GenderCode", ReplaceMissing(cvPersonalinformationData.Gender.Code, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@IsCedCode", ReplaceMissing(cvPersonalinformationData.IsCed.Code, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@DateOfBirth", ReplaceMissing(cvPersonalinformationData.DateOfBirth, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@PlaceOfBirth", ReplaceMissing(cvPersonalinformationData.DateOfBirthPlace, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@NationalityCode", ReplaceMissing(nationalityBuffer, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@CivilStatusCode", ReplaceMissing(civilStateBuffer, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("@Titles", If(cvPersonalinformationData.Title Is Nothing, DBNull.Value, ReplaceMissing(String.Join(",", cvPersonalinformationData.Title.ToArray()), DBNull.Value))))
			listOfParams.Add(New SqlClient.SqlParameter("@PhoneNumbers", If(cvPersonalinformationData.PhoneNumbers Is Nothing, DBNull.Value, ReplaceMissing(String.Join(",", cvPersonalinformationData.PhoneNumbers.ToArray()), DBNull.Value))))
			listOfParams.Add(New SqlClient.SqlParameter("@TelefaxNumbers", If(cvPersonalinformationData.TelefaxNumber Is Nothing, DBNull.Value, ReplaceMissing(String.Join(",", cvPersonalinformationData.TelefaxNumber.ToArray()), DBNull.Value))))
			listOfParams.Add(New SqlClient.SqlParameter("@EMails", If(cvPersonalinformationData.Email Is Nothing, DBNull.Value, ReplaceMissing(String.Join(",", cvPersonalinformationData.Email.ToArray()), DBNull.Value))))
			listOfParams.Add(New SqlClient.SqlParameter("@Homepages", If(cvPersonalinformationData.Homepage Is Nothing, DBNull.Value, ReplaceMissing(String.Join(",", cvPersonalinformationData.Homepage.ToArray()), DBNull.Value))))

			listOfParams.Add(New SqlClient.SqlParameter("@Street", ReplaceMissing(cvPersonalinformationData.Address.Street, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@PostCode", ReplaceMissing(cvPersonalinformationData.Address.Postcode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@City", ReplaceMissing(cvPersonalinformationData.Address.City, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@CountryCode", ReplaceMissing(cvPersonalinformationData.Address.Country.Code, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@AddressState", ReplaceMissing(cvPersonalinformationData.Address.State, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@WorkAdditionalText", ReplaceMissing(cvlXMLData.Work.AdditionalText, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@EducationAdditionalText", ReplaceMissing(cvlXMLData.Education.AdditionalText, DBNull.Value)))


			' Output Parameters
			Dim newIdParameter = New SqlClient.SqlParameter("@NewCVLId", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)

			Dim NewPersonalIdParameter = New SqlClient.SqlParameter("@NewPersonalId", SqlDbType.Int)
			NewPersonalIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(NewPersonalIdParameter)

			Dim newWorkIdParameter = New SqlClient.SqlParameter("@NewWorkId", SqlDbType.Int)
			newWorkIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newWorkIdParameter)

			Dim newEducationIdParameter = New SqlClient.SqlParameter("@NewEducationId", SqlDbType.Int)
			newEducationIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newEducationIdParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If Not newIdParameter.Value Is Nothing AndAlso Not NewPersonalIdParameter.Value Is Nothing AndAlso Not newWorkIdParameter.Value Is Nothing AndAlso Not newEducationIdParameter.Value Is Nothing Then
				cvlXMLData.ProfileID = CType(newIdParameter.Value, Integer)
				cvPersonalinformationData.PersonalID = CType(NewPersonalIdParameter.Value, Integer)
				cvlXMLData.Work.ID = CType(newWorkIdParameter.Value, Integer)
				cvlXMLData.Education.ID = CType(newEducationIdParameter.Value, Integer)
			Else
				success = False
			End If

			Return success
		End Function

		Function LoadCVLProfileViewData(ByVal customerID As String, ByVal cvlPrifleID As Integer?) As IEnumerable(Of CVLizerProfileViewData) Implements ICVLizerDatabaseAccess.LoadCVLProfileViewData
			Dim result As List(Of CVLizerProfileViewData) = Nothing

			Dim sql = "[Load CVL Profile Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@CustomerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@CVLProfileID", ReplaceMissing(cvlPrifleID, 0)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of CVLizerProfileViewData)

					While reader.Read
						Dim data = New CVLizerProfileViewData

						data.ProfileID = SafeGetInteger(reader, "ID", Nothing)
						data.Customer_ID = SafeGetString(reader, "Customer_ID")
						data.PersonalID = SafeGetInteger(reader, "PersonalID", Nothing)
						data.WorkID = SafeGetInteger(reader, "WorkID", Nothing)
						data.EducationID = SafeGetInteger(reader, "EducationID", Nothing)
						data.AdditionalID = SafeGetInteger(reader, "AdditionalID", Nothing)
						data.ObjectiveID = SafeGetInteger(reader, "ObjectiveID", Nothing)
						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						data.FirstName = SafeGetString(reader, "FirstName")
						data.LastName = SafeGetString(reader, "LastName")


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

		Function LoadAssignedCVLProfileViewData(ByVal customerID As String, ByVal cvlPrifleID As Integer?) As CVLizerProfileViewData Implements ICVLizerDatabaseAccess.LoadAssignedCVLProfileViewData
			Dim result As CVLizerProfileViewData = Nothing

			Dim sql = "[Load CVL Profile Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@CustomerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@CVLProfileID", ReplaceMissing(cvlPrifleID, 0)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New CVLizerProfileViewData

					While reader.Read

						result.ProfileID = SafeGetInteger(reader, "ID", Nothing)
						result.Customer_ID = SafeGetString(reader, "Customer_ID")
						result.PersonalID = SafeGetInteger(reader, "PersonalID", Nothing)
						result.WorkID = SafeGetInteger(reader, "WorkID", Nothing)
						result.EducationID = SafeGetInteger(reader, "EducationID", Nothing)
						result.AdditionalID = SafeGetInteger(reader, "AdditionalID", Nothing)
						result.ObjectiveID = SafeGetInteger(reader, "ObjectiveID", Nothing)
						result.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						result.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						result.FirstName = SafeGetString(reader, "FirstName")
						result.LastName = SafeGetString(reader, "LastName")


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

		Function LoadAssignedCVLPersonalViewData(ByVal cvlPrifleID As Integer, ByVal cvlPersonalID As Integer) As PersonalViewData Implements ICVLizerDatabaseAccess.LoadAssignedCVLPersonalViewData
			Dim result As PersonalViewData = Nothing

			Dim sql = "[Load Assigned CVL Personal Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@CVLProfileID", ReplaceMissing(cvlPrifleID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@PersonalID", ReplaceMissing(cvlPersonalID, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New PersonalViewData

					While reader.Read

						result.PersonalID = SafeGetInteger(reader, "ID", Nothing)
						result.DateOfBirth = SafeGetDateTime(reader, "DateOfBirth", Nothing)
						result.DateOfBirthPlace = SafeGetString(reader, "PlaceOfBirth")
						result.FirstName = SafeGetString(reader, "FirstName")
						result.Gender = SafeGetString(reader, "FK_GenderCode")
						result.GenderLabel = SafeGetString(reader, "GenderLabel")

						result.IsCed = SafeGetString(reader, "FK_IsCedCode")
						result.IsCedLable = SafeGetString(reader, "FK_IsCedCode")
						result.LastName = SafeGetString(reader, "LastName")

						Dim code As String = String.Empty
						Dim Lable As String = String.Empty

						Dim nationality = LoadAssignedCVLPersonalNationalityViewData(cvlPersonalID)
						If Not Nationality Is Nothing AndAlso Nationality.Count > 0 Then
							For Each itm In nationality
								code &= If(String.IsNullOrWhiteSpace(code), "", ", ") & itm.Code
								Lable &= If(String.IsNullOrWhiteSpace(Lable), "", ", ") & itm.CodeName
							Next
							result.Nationality = code
							result.NationalityLable = Lable
						End If

						code = String.Empty
						Lable = String.Empty

						Dim civilstate = LoadAssignedCVLPersonalCivilstateViewData(cvlPersonalID)
						If Not civilstate Is Nothing AndAlso civilstate.Count > 0 Then
							For Each itm In civilstate
								code &= If(String.IsNullOrWhiteSpace(code), "", ", ") & itm.Code
								Lable &= If(String.IsNullOrWhiteSpace(Lable), "", ", ") & itm.CodeName
							Next
							result.CivilState = code
							result.CivilStateLable = Lable
						End If


						Dim photoData = LoadAssignedCVLPersonalPhotoViewData(cvlPrifleID)
						result.PersonalPhoto = photoData.DocBinary

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

		Private Function LoadAssignedCVLPersonalNationalityViewData(ByVal cvlPersonalID As Integer) As IEnumerable(Of CodeNameData)
			Dim result As List(Of CodeNameData) = Nothing

			Dim sql = "[Load Assigned CVL Personal Nationality Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@PersonalID", ReplaceMissing(cvlPersonalID, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of CodeNameData)

					While reader.Read
						Dim data = New CodeNameData

						data.Code = SafeGetString(reader, "FK_NationalityCode")
						data.CodeName = SafeGetString(reader, "NationalityCodeLable")


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

		Private Function LoadAssignedCVLPersonalCivilstateViewData(ByVal cvlPersonalID As Integer) As IEnumerable(Of CodeNameData)
			Dim result As List(Of CodeNameData) = Nothing

			Dim sql = "[Load Assigned CVL Personal CivilState Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@PersonalID", ReplaceMissing(cvlPersonalID, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of CodeNameData)

					While reader.Read
						Dim data = New CodeNameData

						data.Code = SafeGetString(reader, "FK_CivilStateCode")
						data.CodeName = SafeGetString(reader, "CivilStateCodeLable")


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

		Function LoadAssignedCVLPersonalAddressViewData(ByVal cvlPrifleID As Integer, ByVal cvlPersonalID As Integer) As AddressViewData Implements ICVLizerDatabaseAccess.LoadAssignedCVLPersonalAddressViewData
			Dim result As AddressViewData = Nothing

			Dim sql = "[Load Assigned CVL Personal Address Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@PersonalID", ReplaceMissing(cvlPersonalID, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New AddressViewData

					While reader.Read

						result.ID = SafeGetInteger(reader, "ID", Nothing)
						result.Street = SafeGetString(reader, "Street")
						result.Postcode = SafeGetString(reader, "Postcode")
						result.City = SafeGetString(reader, "City")
						result.Country = SafeGetString(reader, "FK_CountryCode")
						result.CountryLable = SafeGetString(reader, "CountryLable")
						result.State = SafeGetString(reader, "State")

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

		Function LoadAssignedCVLPersonalTitleViewData(ByVal cvlPrifleID As Integer, ByVal cvlPersonalID As Integer) As IEnumerable(Of PersonalListViewData) Implements ICVLizerDatabaseAccess.LoadAssignedCVLPersonalTitleViewData
			Dim result As List(Of PersonalListViewData) = Nothing

			Dim sql = "[Load Assigned CVL Personal Title Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@PersonalID", ReplaceMissing(cvlPersonalID, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of PersonalListViewData)

					While reader.Read
						Dim data = New PersonalListViewData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.PersonalID = SafeGetInteger(reader, "FK_PersonalID", Nothing)
						data.Lable = SafeGetString(reader, "Title")


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

		Function LoadAssignedCVLPersonalEMailViewData(ByVal cvlPrifleID As Integer, ByVal cvlPersonalID As Integer) As IEnumerable(Of PersonalListViewData) Implements ICVLizerDatabaseAccess.LoadAssignedCVLPersonalEMailViewData
			Dim result As List(Of PersonalListViewData) = Nothing

			Dim sql = "[Load Assigned CVL Personal EMail Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@PersonalID", ReplaceMissing(cvlPersonalID, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of PersonalListViewData)

					While reader.Read
						Dim data = New PersonalListViewData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.PersonalID = SafeGetInteger(reader, "FK_PersonalID", Nothing)
						data.Lable = SafeGetString(reader, "EMailAddress")


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

		Function LoadAssignedCVLPersonalHomepageViewData(ByVal cvlPrifleID As Integer, ByVal cvlPersonalID As Integer) As IEnumerable(Of PersonalListViewData) Implements ICVLizerDatabaseAccess.LoadAssignedCVLPersonalHomepageViewData
			Dim result As List(Of PersonalListViewData) = Nothing

			Dim sql = "[Load Assigned CVL Personal Homepage Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@PersonalID", ReplaceMissing(cvlPersonalID, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of PersonalListViewData)

					While reader.Read
						Dim data = New PersonalListViewData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.PersonalID = SafeGetInteger(reader, "FK_PersonalID", Nothing)
						data.Lable = SafeGetString(reader, "Homepage")


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

		Function LoadAssignedCVLPersonalTelefonNumberViewData(ByVal cvlPrifleID As Integer, ByVal cvlPersonalID As Integer) As IEnumerable(Of PersonalListViewData) Implements ICVLizerDatabaseAccess.LoadAssignedCVLPersonalTelefonNumberViewData
			Dim result As List(Of PersonalListViewData) = Nothing

			Dim sql = "[Load Assigned CVL Personal Telefonnumber Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@PersonalID", ReplaceMissing(cvlPersonalID, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of PersonalListViewData)

					While reader.Read
						Dim data = New PersonalListViewData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.PersonalID = SafeGetInteger(reader, "FK_PersonalID", Nothing)
						data.Lable = SafeGetString(reader, "PhoneNumber")


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

		Function LoadAssignedCVLPersonalTelefaxNumberViewData(ByVal cvlPrifleID As Integer, ByVal cvlPersonalID As Integer) As IEnumerable(Of PersonalListViewData) Implements ICVLizerDatabaseAccess.LoadAssignedCVLPersonalTelefaxNumberViewData
			Dim result As List(Of PersonalListViewData) = Nothing

			Dim sql = "[Load Assigned CVL Personal Telefaxnumber Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@PersonalID", ReplaceMissing(cvlPersonalID, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of PersonalListViewData)

					While reader.Read
						Dim data = New PersonalListViewData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.PersonalID = SafeGetInteger(reader, "FK_PersonalID", Nothing)
						data.Lable = SafeGetString(reader, "TelefaxNumber")


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

		Function AddCustomerPayableServiceUsage(ByVal customerID As String, ByVal userData As CustomerPayableUserData) As Boolean Implements ICVLizerDatabaseAccess.AddCustomerPayableServiceUsage
			Dim success As Boolean = True

			Try

				Dim sql As String
				sql = "INSERT INTO [spSystemInfo].Dbo.[tblCustomerPayableServices] ("
				sql &= "[Customer_Guid]"
				sql &= ",[User_Guid]"
				sql &= ",[ServiceName]"
				sql &= ",[JobID]"
				sql &= ",[Servicedate]"
				sql &= ",[CreatedOn]"
				sql &= ",[CreatedFrom]"
				sql &= ",[t2]"
				sql &= ",[Validated]"
				sql &= ",[validatedon]) "
				sql &= "VALUES ("
				sql &= "@Customer_Guid"
				sql &= ",@User_Guid"
				sql &= ",@ServiceName"
				sql &= ",@ServiceArt"
				sql &= ",GetDate()"
				sql &= ",GetDate()"
				sql &= ",@CreatedFrom"
				sql &= ",1"
				sql &= ",1"
				sql &= ",GetDate()"
				sql &= " )"


				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("Customer_Guid", ReplaceMissing(customerID, userData.CustomerID)))
				listOfParams.Add(New SqlClient.SqlParameter("User_Guid", ReplaceMissing(userData.AdvisorID, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("ServiceName", ReplaceMissing(userData.ServiceName, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("ServiceArt", ReplaceMissing(userData.JobID, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("CreatedFrom", ReplaceMissing(userData.Advisorname, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				Return False
			End Try


			Return success

		End Function


#End Region


	End Class


End Namespace
