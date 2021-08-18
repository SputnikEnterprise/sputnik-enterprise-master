
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects

Namespace Employee


	Partial Public Class EmployeeDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IEmployeeDatabaseAccess

		Function LoadExistingEmployeesANDApplicantData(ByVal mdNr As Integer) As IEnumerable(Of ExistingEmployeeSearchData) Implements IEmployeeDatabaseAccess.LoadExistingEmployeesANDApplicantData

			Dim result As List(Of ExistingEmployeeSearchData) = Nothing

			Dim sql As String

			sql = "[Get Existing Employee And Applicant Data For Quick Search]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", ReplaceMissing(mdNr, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ExistingEmployeeSearchData)

					While reader.Read()
						Dim searchData As New ExistingEmployeeSearchData

						searchData.MDNr = SafeGetInteger(reader, "MDNr", 0)
						searchData.EmployeeNumber = SafeGetInteger(reader, "MANr", 0)
						searchData.Lastname = SafeGetString(reader, "Nachname")
						searchData.Firstname = SafeGetString(reader, "Vorname")
						searchData.Street = SafeGetString(reader, "Strasse")
						searchData.Postcode = SafeGetString(reader, "PLZ")
						searchData.Location = SafeGetString(reader, "Ort")
						searchData.CountryCode = SafeGetString(reader, "Land")
						searchData.Gender = SafeGetString(reader, "Geschlecht")
						searchData.Birthdate = SafeGetDateTime(reader, "GebDat", Nothing)
						searchData.employeeKST = SafeGetString(reader, "KST")
						searchData.MABusinessBranch = SafeGetString(reader, "MAFiliale")
						searchData.Telephone_P = SafeGetString(reader, "Telefon_P")
						searchData.MobilePhone = SafeGetString(reader, "Natel")
						searchData.Email = SafeGetString(reader, "eMail")
						searchData.ShowAsApplicant = SafeGetBoolean(reader, "ShowAsApplicant", False)
						searchData.ApplicantLifecycle = SafeGetInteger(reader, "ApplicantLifecycle", 0)
						searchData.ApplicantID = SafeGetInteger(reader, "ApplicantID", 0)
						searchData.BriefAnrede = SafeGetString(reader, "BriefAnrede")
						searchData.SMS_Mailing = SafeGetBoolean(reader, "MA_SMS_Mailing", False)

						searchData.employeeAdvisor = SafeGetString(reader, "Advisor")

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

		Function LoadQuickSearchDataWithStoredProcedure(ByVal mdNr As Integer, ByVal storedProcedureName As String) As IEnumerable(Of ExistingEmployeeSearchData) Implements IEmployeeDatabaseAccess.LoadQuickSearchDataWithStoredProcedure

			Dim result As List(Of ExistingEmployeeSearchData) = Nothing

			Dim sql As String = String.Format("{0}", storedProcedureName)

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", ReplaceMissing(mdNr, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ExistingEmployeeSearchData)

					While reader.Read()
						Dim searchData As New ExistingEmployeeSearchData

						searchData.MDNr = SafeGetInteger(reader, "MDNr", 0)
						searchData.EmployeeNumber = SafeGetInteger(reader, "MANr", 0)
						searchData.Lastname = SafeGetString(reader, "Nachname")
						searchData.Firstname = SafeGetString(reader, "Vorname")
						searchData.Street = SafeGetString(reader, "Strasse")
						searchData.Postcode = SafeGetString(reader, "PLZ")
						searchData.Location = SafeGetString(reader, "Ort")
						searchData.CountryCode = SafeGetString(reader, "Land")
						searchData.Gender = SafeGetString(reader, "Geschlecht")
						searchData.Birthdate = SafeGetDateTime(reader, "GebDat", Nothing)
						searchData.employeeKST = SafeGetString(reader, "KST")
						searchData.MABusinessBranch = SafeGetString(reader, "MAFiliale")
						searchData.Telephone_P = SafeGetString(reader, "Telefon_P")
						searchData.MobilePhone = SafeGetString(reader, "Natel")
						searchData.Email = SafeGetString(reader, "eMail")
						searchData.ShowAsApplicant = SafeGetBoolean(reader, "ShowAsApplicant", False)
						searchData.ApplicantLifecycle = SafeGetInteger(reader, "ApplicantLifecycle", 0)
						searchData.ApplicantID = SafeGetInteger(reader, "ApplicantID", 0)
						searchData.BriefAnrede = SafeGetString(reader, "BriefAnrede")
						searchData.SMS_Mailing = SafeGetBoolean(reader, "MA_SMS_Mailing", False)

						searchData.employeeAdvisor = SafeGetString(reader, "Advisor")

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

	End Class

End Namespace
