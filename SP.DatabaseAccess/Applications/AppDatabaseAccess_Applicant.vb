Imports SP.DatabaseAccess.Applicant.DataObjects
'Imports System.Transactions
Imports SP.DatabaseAccess.CVLizer.DataObjects
Imports SP.DatabaseAccess.EMailJob.DataObjects.EMailSettingData

Namespace Applicant



	Partial Class AppDatabaseAccess

		Inherits DatabaseAccessBase
		Implements IAppDatabaseAccess



		''' <summary>
		''' Loads all Document Data
		''' </summary>
		''' <returns>The user data.</returns>
		Function LoadApplicantData(ByVal customerID As String, ByVal businessBranch As String) As IEnumerable(Of ApplicantData) Implements IAppDatabaseAccess.LoadApplicantData

			Dim result As List(Of ApplicantData) = Nothing

			Dim sql As String


			sql = "SELECT A.ID"
			sql &= ",A.[Customer_ID]"
			sql &= ",A.[ApplicantNumber]"
			sql &= ",A.[Lastname]"
			sql &= ",A.[Firstname]"
			sql &= ",A.[Gender]"
			sql &= ",A.[Street]"
			sql &= ",A.[PostOfficeBox]"
			sql &= ",A.[Postcode]"
			sql &= ",A.[Location]"
			sql &= ",A.[Country]"
			sql &= ",A.[Nationality]"
			sql &= ",A.[EMail]"
			sql &= ",A.[Telephone]"
			sql &= ",A.[MobilePhone]"
			sql &= ",A.[Birthdate]"
			sql &= ",A.[Permission]"
			sql &= ",A.[Profession]"
			sql &= ",A.[Auto]"
			sql &= ",A.[Motorcycle]"
			sql &= ",A.[Bicycle]"
			sql &= ",A.[DrivingLicence1]"
			sql &= ",A.[DrivingLicence2]"
			sql &= ",A.[DrivingLicence3]"
			sql &= ",A.[CivilState]"
			sql &= ",A.[Language]"
			sql &= ",A.[LanguageLevel]"

			sql &= ",A.CreatedOn"
			sql &= ",A.CreatedFrom"
			sql &= ",A.ChangedOn"
			sql &= ",A.ChangedFrom"

			sql &= " FROM [dbo].[tbl_applicant] A "
			sql &= " Left Join [spSystemInfo].Dbo.[tbl_CustomerSetting] C On A.Customer_ID = C.Customer_ID "
			sql &= " Left Join [dbo].[tbl_application] AP On A.Customer_ID = AP.Customer_ID And A.ApplicantNumber = AP.ApplicantNumber "
			sql &= " Where (@CustomerID = '' OR A.Customer_ID = @CustomerID)"
			sql &= " And (@BusinessBranch = '' Or AP.BusinessBranch = @BusinessBranch)"
			sql &= " Order By A.ID Desc"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", customerID))
			listOfParams.Add(New SqlClient.SqlParameter("BusinessBranch", ReplaceMissing(businessBranch, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ApplicantData)

					While reader.Read

						Dim data = New ApplicantData()

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.Customer_ID = SafeGetString(reader, "Customer_ID")
						data.ApplicantNumber = SafeGetInteger(reader, "ApplicantNumber", 0)
						data.Lastname = SafeGetString(reader, "Lastname")
						data.Firstname = SafeGetString(reader, "Firstname")

						data.Gender = SafeGetString(reader, "Gender", String.Empty)
						data.Street = SafeGetString(reader, "Street")

						data.Postcode = SafeGetString(reader, "Postcode")
						data.Location = SafeGetString(reader, "Location")
						data.Country = SafeGetString(reader, "Country")
						data.Nationality = SafeGetString(reader, "Nationality")
						data.EMail = SafeGetString(reader, "EMail")
						data.Telephone = SafeGetString(reader, "Telephone")
						data.MobilePhone = SafeGetString(reader, "MobilePhone")
						data.Birthdate = SafeGetDateTime(reader, "Birthdate", Nothing)
						data.Permission = SafeGetString(reader, "Permission")
						data.Profession = SafeGetString(reader, "Profession")
						data.Auto = SafeGetBoolean(reader, "Auto", False)
						data.Motorcycle = SafeGetBoolean(reader, "Motorcycle", False)
						data.Bicycle = SafeGetBoolean(reader, "Bicycle", False)
						data.DrivingLicence1 = SafeGetString(reader, "DrivingLicence1")
						data.DrivingLicence2 = SafeGetString(reader, "DrivingLicence2")
						data.DrivingLicence3 = SafeGetString(reader, "DrivingLicence3")
						data.CivilState = SafeGetInteger(reader, "CivilState", 0)
						data.Language = SafeGetString(reader, "Language")
						data.LanguageLevel = SafeGetInteger(reader, "LanguageLevel", 0)

						data.CreatedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
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

		''' <summary>
		''' loads assigned applicant
		''' </summary>
		''' <returns>applicantdata.</returns>
		Function LoadAssignedApplicantData(ByVal customerID As String, ByVal applicantData As ApplicantData) As ApplicantData Implements IAppDatabaseAccess.LoadAssignedApplicantData

			Dim result As ApplicantData = Nothing

			Dim sql As String


			sql = "SELECT Top 1"
			sql &= " A.ID"
			sql &= ",A.[Customer_ID]"
			sql &= ",A.[EmployeeID]"
			sql &= ",A.[Lastname]"
			sql &= ",A.[Firstname]"
			sql &= ",A.[Gender]"
			sql &= ",A.[Street]"
			sql &= ",A.[PostOfficeBox]"
			sql &= ",A.[Postcode]"
			sql &= ",A.[Location]"
			sql &= ",A.[Country]"
			sql &= ",A.[Nationality]"
			sql &= ",A.[EMail]"
			sql &= ",A.[Telephone]"
			sql &= ",A.[MobilePhone]"
			sql &= ",A.[Birthdate]"
			sql &= ",A.[Permission]"
			sql &= ",A.[Profession]"
			sql &= ",A.[Auto]"
			sql &= ",A.[Motorcycle]"
			sql &= ",A.[Bicycle]"
			sql &= ",A.[DrivingLicence1]"
			sql &= ",A.[DrivingLicence2]"
			sql &= ",A.[DrivingLicence3]"
			sql &= ",A.[CivilState]"
			sql &= ",A.[Language]"
			sql &= ",A.[LanguageLevel]"

			sql &= ",A.CreatedOn"
			sql &= ",A.CreatedFrom"

			sql &= " FROM [dbo].[tbl_applicant] A "
			sql &= " Left Join [spSystemInfo].Dbo.[tbl_CustomerSetting] C On C.Customer_ID = A.Customer_ID "
			sql &= " Where (A.Customer_ID = @CustomerID)"
			sql &= " And (A.EMail = @EMail)"
			sql &= " And (A.Birthdate  = NULL OR A.Birthdate = @Birthdate)"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", customerID))
			listOfParams.Add(New SqlClient.SqlParameter("EMail", ReplaceMissing(applicantData.EMail, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Birthdate", ReplaceMissing(applicantData.Birthdate, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New ApplicantData

					While reader.Read

						Dim data = New ApplicantData()

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.Customer_ID = SafeGetString(reader, "Customer_ID")
						data.ApplicantNumber = SafeGetInteger(reader, "ID", 0)
						data.EmployeeID = SafeGetInteger(reader, "EmployeeID", 0)
						data.Lastname = SafeGetString(reader, "Lastname")
						data.Firstname = SafeGetString(reader, "Firstname")

						data.Gender = SafeGetString(reader, "Gender", String.Empty)
						data.Street = SafeGetString(reader, "Street")

						data.Postcode = SafeGetString(reader, "Postcode")
						data.Location = SafeGetString(reader, "Location")
						data.Country = SafeGetString(reader, "Country")
						data.Nationality = SafeGetString(reader, "Nationality")
						data.EMail = SafeGetString(reader, "EMail")
						data.Telephone = SafeGetString(reader, "Telephone")
						data.MobilePhone = SafeGetString(reader, "MobilePhone")
						data.Birthdate = SafeGetDateTime(reader, "Birthdate", Nothing)
						data.Permission = SafeGetString(reader, "Permission")
						data.Profession = SafeGetString(reader, "Profession")
						data.Auto = SafeGetBoolean(reader, "Auto", False)
						data.Motorcycle = SafeGetBoolean(reader, "Motorcycle", False)
						data.Bicycle = SafeGetBoolean(reader, "Bicycle", False)
						data.DrivingLicence1 = SafeGetString(reader, "DrivingLicence1")
						data.DrivingLicence2 = SafeGetString(reader, "DrivingLicence2")
						data.DrivingLicence3 = SafeGetString(reader, "DrivingLicence3")
						data.CivilState = SafeGetInteger(reader, "CivilState", 0)
						data.Language = SafeGetString(reader, "Language")
						data.LanguageLevel = SafeGetInteger(reader, "LanguageLevel", 0)

						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom")


						result = data

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


#Region "loading cv data"

		Public Function LoadCVPersonalData(ByVal customerID As String, ByVal businessBranch As String, ByVal trxmlID As Integer?) As IEnumerable(Of ApplicantCvPersonalData) Implements IAppDatabaseAccess.LoadCVPersonalData

			Dim result As List(Of ApplicantData) = Nothing

			Dim sql As String

			sql = "[List CV Personal Data]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", customerID))
			listOfParams.Add(New SqlClient.SqlParameter("BusinessBranch", ReplaceMissing(businessBranch, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("trxmlID", ReplaceMissing(trxmlID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ApplicantData)

					While reader.Read

						Dim data = New ApplicantData()

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.Customer_ID = SafeGetString(reader, "Customer_ID")
						data.ApplicantNumber = SafeGetInteger(reader, "ApplicantNumber", 0)
						data.Lastname = SafeGetString(reader, "Lastname")
						data.Firstname = SafeGetString(reader, "Firstname")

						data.Gender = SafeGetString(reader, "Gender", String.Empty)
						data.Street = SafeGetString(reader, "Street")

						data.Postcode = SafeGetString(reader, "Postcode")
						data.Location = SafeGetString(reader, "Location")
						data.Country = SafeGetString(reader, "Country")
						data.Nationality = SafeGetString(reader, "Nationality")
						data.EMail = SafeGetString(reader, "EMail")
						data.Telephone = SafeGetString(reader, "Telephone")
						data.MobilePhone = SafeGetString(reader, "MobilePhone")
						data.Birthdate = SafeGetDateTime(reader, "Birthdate", Nothing)
						data.Permission = SafeGetString(reader, "Permission")
						data.Profession = SafeGetString(reader, "Profession")
						data.Auto = SafeGetBoolean(reader, "Auto", False)
						data.Motorcycle = SafeGetBoolean(reader, "Motorcycle", False)
						data.Bicycle = SafeGetBoolean(reader, "Bicycle", False)
						data.DrivingLicence1 = SafeGetString(reader, "DrivingLicence1")
						data.DrivingLicence2 = SafeGetString(reader, "DrivingLicence2")
						data.DrivingLicence3 = SafeGetString(reader, "DrivingLicence3")
						data.CivilState = SafeGetInteger(reader, "CivilState", 0)
						data.Language = SafeGetString(reader, "Language")
						data.LanguageLevel = SafeGetInteger(reader, "LanguageLevel", 0)

						data.CreatedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
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


#End Region


#Region "updateing applicant data with cvlizer data"

		''' <summary>
		''' Adds a application with applicant data.
		''' </summary>
		Function UpdateApplicatantWithCVLData(ByVal cvlData As CVLizerXMLData, ByVal applicantID As Integer, ByVal applicationID As Integer, ByVal priorityModul As PriorityModulEnum) As Boolean Implements IAppDatabaseAccess.UpdateApplicatantWithCVLData

			Dim success = True

			Dim sql As String

			sql = "[Update Applicant With CVLData]"

			If priorityModul = PriorityModulEnum.CVL Then
				sql = "[Update Applicant Data With CVLData Priority]"
			Else
				sql = "[Update CVL Data With ApplicantData Priority]"
			End If

			' Parameters

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("Customer_ID", ReplaceMissing(cvlData.Customer_ID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ApplicantID", ReplaceMissing(applicantID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ApplicationID", ReplaceMissing(applicationID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ProfileID", ReplaceMissing(cvlData.ProfileID, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success

		End Function

		''' <summary>
		''' update a application with scan drop in data.
		''' </summary>
		Function UpdateApplicationWithScanDropInData(ByVal customerID As String, ByVal applicationID As Integer, ByVal scanData As ApplicationData) As Boolean Implements IAppDatabaseAccess.UpdateApplicationWithScanDropInData

			Dim success = True

			Dim sql As String

			sql = "[Update Application With Scan DropIn Data]"

			' Parameters

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("Customer_ID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ApplicationID", ReplaceMissing(applicationID, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("Advisor", ReplaceMissing(scanData.Advisor, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("BusinessBranch", ReplaceMissing(scanData.BusinessBranch, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success

		End Function

		''' <summary>
		''' update a application with scan drop in data.
		''' </summary>
		Function UpdateNewApplicationWithOldApplicantData(ByVal customerID As String, ByVal cvlProfileID As Integer, ByVal applicationID As Integer?, ByVal applicantID As Integer?) As Boolean Implements IAppDatabaseAccess.UpdateNewApplicationWithOldApplicantData

			Dim success = True

			Dim sql As String

			sql = "Update [applicant].dbo.tbl_application Set FK_ApplicantID = @cvlProfileID "
			sql &= "Where ID = @ApplicationID"

			' Parameters

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("Customer_ID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ApplicationID", ReplaceMissing(applicationID, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("cvlProfileID", ReplaceMissing(cvlProfileID, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)


			Return success

		End Function


#End Region

	End Class


End Namespace
