
Imports SP.DatabaseAccess.Applicant.DataObjects


Namespace Applicant



	Partial Class CvlDatabaseAccess

		Inherits DatabaseAccessBase
		Implements IAppCvDatabaseAccess


#Region "public methodes Loading"

		Public Function LoadAllCvProfileData() As IEnumerable(Of ApplicantCvProfileData) Implements IAppCvDatabaseAccess.LoadAllCvProfileData
			Dim result As List(Of ApplicantCvProfileData) = Nothing

			Dim sql As String


			sql = "SELECT "
			sql &= " P.ID"
			sql &= ",P.[Customer_ID]"
			sql &= ",P.[BusinessBranch]"
			sql &= ",P.[TrxmlID]"
			sql &= ",P.[FK_CvPersonal]"
			sql &= ",P.[FK_CvDocumentText]"
			sql &= ",P.[FK_CvDocumentHtml]"
			sql &= ",P.CreatedOn"
			sql &= ",P.CreatedFrom"
			sql &= ",P.ChangedOn"
			sql &= ",P.ChangedFrom"

			sql &= " FROM [dbo].[tbl_CvProfile] P "
			sql &= " Order By P.CreatedOn Desc"


			' Parameters

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ApplicantCvProfileData)

					While reader.Read

						Dim data = New ApplicantCvProfileData()

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.Customer_ID = SafeGetString(reader, "Customer_ID")
						data.BusinessBranch = SafeGetString(reader, "BusinessBranch")

						data.TrxmlID = SafeGetInteger(reader, "TrxmlID", Nothing)
						data.FK_CvDocumentText = SafeGetInteger(reader, "FK_CvDocumentText", Nothing)
						data.FK_CvPersonal = SafeGetInteger(reader, "FK_CvPersonal", Nothing)
						data.FK_CvDocumentHtml = SafeGetInteger(reader, "FK_CvDocumentHtml", Nothing)

						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						data.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
						data.ChangedFrom = SafeGetString(reader, "ChangedFrom")

						result.Add(data)

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

		Public Function LoadCvProfileData(ByVal trxmlID As Integer?) As ApplicantCvProfileData Implements IAppCvDatabaseAccess.LoadCvProfileData
			Dim result As ApplicantCvProfileData = Nothing

			Dim sql As String


			sql = "SELECT Top 1 "
			sql &= " P.ID"
			sql &= ",P.[Customer_ID]"
			sql &= ",P.[BusinessBranch]"
			sql &= ",P.[TrxmlID]"
			sql &= ",P.[FK_CvPersonal]"
			sql &= ",P.[FK_CvDocumentText]"
			sql &= ",P.[FK_CvDocumentHtml]"
			sql &= ",P.CreatedOn"
			sql &= ",P.CreatedFrom"
			sql &= ",P.ChangedOn"
			sql &= ",P.ChangedFrom"

			sql &= " FROM [dbo].[tbl_CvProfile] P "
			sql &= " Where "
			sql &= " (@trXMLID = 0 Or P.TrxmlID = @TrxmlID)"
			sql &= " Order By P.ID Desc"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("trxmlID", ReplaceMissing(trxmlID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				result = New ApplicantCvProfileData

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result.ID = SafeGetInteger(reader, "ID", 0)
					result.Customer_ID = SafeGetString(reader, "Customer_ID")
					result.BusinessBranch = SafeGetString(reader, "BusinessBranch")

					result.TrxmlID = SafeGetInteger(reader, "TrxmlID", Nothing)
					result.FK_CvDocumentText = SafeGetInteger(reader, "FK_CvDocumentText", Nothing)
					result.FK_CvPersonal = SafeGetInteger(reader, "FK_CvPersonal", Nothing)
					result.FK_CvDocumentHtml = SafeGetInteger(reader, "FK_CvDocumentHtml", Nothing)

					result.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
					result.CreatedFrom = SafeGetString(reader, "CreatedFrom")
					result.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
					result.ChangedFrom = SafeGetString(reader, "ChangedFrom")

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		Public Function LoadCvPersonalData(ByVal trxmlID As Integer?) As ApplicantCvPersonalData Implements IAppCvDatabaseAccess.LoadCvPersonalData
			Dim result As ApplicantCvPersonalData = Nothing

			Dim sql As String


			sql = "SELECT Top 1 "
			sql &= " P.ID"
			sql &= ",P.[Initials]"
			sql &= ",P.[Title]"
			sql &= ",P.[FirstName]"
			sql &= ",P.[MiddleName]"
			sql &= ",P.[LastNamePrefix]"

			sql &= ",P.[LastName]"
			sql &= ",P.[FullName]"
			sql &= ",P.[DateOfBirth]"
			sql &= ",P.[PlaceOfBirth]"
			sql &= ",P.[FK_CvNationality]"
			sql &= ",P.[FK_CvGender]"
			sql &= ",P.[FK_CvDriversLicence]"
			sql &= ",P.[FK_CvMaritalStatus]"
			sql &= ",P.[Availability]"
			sql &= ",P.[MilitaryService]"
			sql &= ",P.[FK_CvAddress]"

			sql &= ",P.CreatedOn"
			sql &= ",P.CreatedFrom"
			sql &= ",P.ChangedOn"
			sql &= ",P.ChangedFrom"

			sql &= " FROM [dbo].[tbl_CvPersonal] P "
			sql &= " Where "
			sql &= " (@trxmlID = 0 Or P.ID = IsNull((Select Top 1 FK_CvPersonal From dbo.tbl_CvProfile Where TrxmlID = @trxmlID), 0))"
			sql &= " Order By P.ID Desc"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("trxmlID", ReplaceMissing(trxmlID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				result = New ApplicantCvPersonalData

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result.ID = SafeGetInteger(reader, "ID", 0)
					result.Initials = SafeGetString(reader, "Initials")
					result.Title = SafeGetString(reader, "Title")
					result.FirstName = SafeGetString(reader, "FirstName")
					result.MiddleName = SafeGetString(reader, "MiddleName")
					result.LastNamePrefix = SafeGetString(reader, "LastNamePrefix")
					result.LastName = SafeGetString(reader, "LastName")
					result.FullName = SafeGetString(reader, "FullName")

					result.DateOfBirth = SafeGetDateTime(reader, "DateOfBirth", Nothing)
					result.PlaceOfBirth = SafeGetString(reader, "PlaceOfBirth")

					result.FK_CvNationality = SafeGetInteger(reader, "FK_CvNationality", Nothing)
					result.FK_CvGender = SafeGetInteger(reader, "FK_CvGender", Nothing)
					result.FK_CvDriversLicence = SafeGetInteger(reader, "FK_CvDriversLicence", Nothing)
					result.FK_CvMaritalStatus = SafeGetInteger(reader, "FK_CvMaritalStatus", Nothing)

					result.Availability = SafeGetString(reader, "Availability")
					result.PlaceOfBirth = SafeGetString(reader, "MilitaryService")
					result.FK_CvAddress = SafeGetInteger(reader, "FK_CvAddress", Nothing)

					result.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
					result.CreatedFrom = SafeGetString(reader, "CreatedFrom")
					result.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
					result.ChangedFrom = SafeGetString(reader, "ChangedFrom")

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		Public Function LoadCvAddressData(ByVal trxmlID As Integer?) As ApplicantCvAddressData Implements IAppCvDatabaseAccess.LoadCvAddressData
			Dim result As ApplicantCvAddressData = Nothing

			Dim sql As String


			sql = "SELECT Top 1 "
			sql &= " A.ID"
			sql &= ",A.[AddressLine]"
			sql &= ",A.[StreetName]"
			sql &= ",A.[StreetNumberBase]"
			sql &= ",A.[StreetNumberExtension]"
			sql &= ",A.[PostalCode]"
			sql &= ",A.[City]"
			sql &= ",A.[FK_CvRegion]"
			sql &= ",A.[FK_CvCountry]"

			sql &= ",A.CreatedOn"
			sql &= ",A.CreatedFrom"
			sql &= ",A.ChangedOn"
			sql &= ",A.ChangedFrom"

			sql &= " FROM [dbo].[tbl_CvAddress] A "
			sql &= " Where "
			sql &= " (@trxmlID = 0 Or A.ID = IsNull((Select Top 1 P.FK_CvAddress From dbo.tbl_CvPersonal P Where P.ID = (Select Top 1 FK_CvPersonal From tbl_CvProfile Where TrxmlID = @trxmlID) ), 0))"
			sql &= " Order By A.ID Desc"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("trxmlID", ReplaceMissing(trxmlID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				result = New ApplicantCvAddressData

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result.ID = SafeGetInteger(reader, "ID", 0)
					result.AddressLine = SafeGetString(reader, "AddressLine")
					result.StreetName = SafeGetString(reader, "StreetName")
					result.StreetNumberBase = SafeGetString(reader, "StreetNumberBase")
					result.StreetNumberExtension = SafeGetString(reader, "StreetNumberExtension")
					result.PostalCode = SafeGetString(reader, "PostalCode")
					result.City = SafeGetString(reader, "City")
					result.FK_CvRegion = SafeGetInteger(reader, "FK_CvRegion", Nothing)
					result.FK_CvCountry = SafeGetInteger(reader, "FK_CvCountry", Nothing)


					result.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
					result.CreatedFrom = SafeGetString(reader, "CreatedFrom")
					result.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
					result.ChangedFrom = SafeGetString(reader, "ChangedFrom")

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		Public Function LoadCvPhoneNumberData(ByVal trxmlID As Integer?) As IEnumerable(Of ApplicantCvPhoneNumberData) Implements IAppCvDatabaseAccess.LoadCvPhoneNumberData
			Dim result As List(Of ApplicantCvPhoneNumberData) = Nothing

			Dim sql As String


			sql = "SELECT Top 1 "
			sql &= " P.ID"
			sql &= ",P.[FK_CvPersonal]"
			sql &= ",P.[FK_CvPhoneNumberType]"
			sql &= ",P.[PhoneNumber]"

			sql &= " FROM [dbo].[tbl_CvPhoneNumber] P "
			sql &= " Where "
			sql &= " (@trxmlID = 0 Or P.FK_CvPersonal = IsNull((Select Top 1 ID From dbo.tbl_CvPersonal Where ID = (Select Top 1 FK_CvPersonal From tbl_CvProfile Where TrxmlID = @trxmlID) ), 0))"
			sql &= " Order By P.ID Desc"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("trxmlID", ReplaceMissing(trxmlID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ApplicantCvPhoneNumberData)

					While reader.Read

						Dim data = New ApplicantCvPhoneNumberData()

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.FK_CvPersonal = SafeGetInteger(reader, "FK_CvPersonal", Nothing)
						data.FK_CvPhoneNumberType = SafeGetInteger(reader, "FK_CvPhoneNumberType", Nothing)
						data.PhoneNumber = SafeGetString(reader, "PhoneNumber")


						result.Add(data)

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

		Public Function LoadCvEMailData(ByVal trxmlID As Integer?) As IEnumerable(Of ApplicantCvEmailData) Implements IAppCvDatabaseAccess.LoadCvEMailData
			Dim result As List(Of ApplicantCvEmailData) = Nothing

			Dim sql As String


			sql = "SELECT Top 1 "
			sql &= " P.ID"
			sql &= ",P.[FK_CvPersonal]"
			sql &= ",P.[FK_CvEmailType]"
			sql &= ",P.[Email]"

			sql &= " FROM [dbo].[tbl_CvEmail] P "
			sql &= " Where "
			sql &= " (@trxmlID = 0 Or P.FK_CvPersonal = IsNull((Select Top 1 ID From dbo.tbl_CvPersonal Where ID = (Select Top 1 FK_CvPersonal From tbl_CvProfile Where TrxmlID = @trxmlID) ), 0))"
			sql &= " Order By P.ID Desc"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("trxmlID", ReplaceMissing(trxmlID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ApplicantCvEmailData)

					While reader.Read

						Dim data = New ApplicantCvEmailData()

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.FK_CvPersonal = SafeGetInteger(reader, "FK_CvPersonal", Nothing)
						data.FK_CvEmailType = SafeGetInteger(reader, "FK_CvEmailType", Nothing)
						data.Email = SafeGetString(reader, "Email")


						result.Add(data)

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

		Public Function LoadCvPictureData(ByVal trxmlID As Integer?) As ApplicantCvPictureData Implements IAppCvDatabaseAccess.LoadCvPictureData
			Dim result As ApplicantCvPictureData = Nothing

			Dim sql As String


			sql = "SELECT Top 1 "
			sql &= " P.ID"
			sql &= ",P.[Content]"
			sql &= ",P.[Filename]"
			sql &= ",P.[ContentType]"

			sql &= " FROM [dbo].[tbl_CvPicture] P "
			sql &= " Where "
			sql &= " (@trxmlID = 0 Or P.ID = IsNull((Select Top 1 FK_CvProfilePicture From dbo.tbl_CvCustomArea Where FK_CvProfile = (Select Top 1 ID From tbl_CvProfile Where TrxmlID = @trxmlID) ), 0))"
			sql &= " Order By P.ID Desc"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("trxmlID", ReplaceMissing(trxmlID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				result = New ApplicantCvPictureData

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result.ID = SafeGetInteger(reader, "ID", 0)
					result.Content = SafeGetByteArray(reader, "Content")
					result.Filename = SafeGetString(reader, "Filename")
					result.ContentType = SafeGetString(reader, "ContentType")

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function


		Public Function LoadCvSkillData(ByVal trxmlID As Integer?) As IEnumerable(Of ApplicantCvLanguageSkillData) Implements IAppCvDatabaseAccess.LoadCvSkillData
			Dim result As List(Of ApplicantCvLanguageSkillData) = Nothing

			Dim sql As String


			sql = "SELECT "
			sql &= " S.ID"
			sql &= ",S.[FK_CvSkill]"
			sql &= ",S.[Text]"
			sql &= ",S.[FK_CvLanguageSkillType]"
			sql &= ",S.[FK_CvLanguageProficiency]"
			sql &= ",S.[IsNativeLanguage]"

			sql &= " FROM [dbo].[tbl_CvLanguageSkill] S "
			sql &= " Where "
			sql &= " (@trxmlID = 0 Or S.FK_CvSkill = IsNull((Select Top 1 ID From dbo.tbl_CvSkill Where FK_CvProfile = (Select Top 1 ID From tbl_CvProfile Where TrxmlID = @trxmlID) ), 0))"
			sql &= " Order By S.ID Desc"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("trxmlID", ReplaceMissing(trxmlID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ApplicantCvLanguageSkillData)

					While reader.Read

						Dim data = New ApplicantCvLanguageSkillData()

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.FK_CvSkill = SafeGetInteger(reader, "FK_CvSkill", Nothing)
						data.Text = SafeGetString(reader, "Text")
						data.FK_CvLanguageSkillType = SafeGetInteger(reader, "FK_CvLanguageSkillType", Nothing)
						data.FK_CvLanguageProficiency = SafeGetInteger(reader, "FK_CvLanguageProficiency", Nothing)
						data.IsNativeLanguage = SafeGetBoolean(reader, "IsNativeLanguage", Nothing)


						result.Add(data)

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


#End Region



	End Class


End Namespace
