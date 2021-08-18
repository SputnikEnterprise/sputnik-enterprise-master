
Imports System.Data.SqlClient
Imports SP.Infrastructure.Logging
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Report.DataObjects

Namespace Customer

	Partial Class CustomerDatabaseAccess
		Inherits DatabaseAccessBase
		Implements ICustomerDatabaseAccess


		Function LoadCommonPhoneNumberData(ByVal phoneNumber As String) As IEnumerable(Of CommonTelephonyData) Implements ICustomerDatabaseAccess.LoadCommonPhoneNumberData

			Dim result As List(Of CommonTelephonyData) = Nothing

			Dim sql As String

			sql = "[Load Assigned Data With Telephon Number]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("phoneNumber", ReplaceMissing(phoneNumber, String.Empty)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CommonTelephonyData)

					While reader.Read()
						Dim viewData = New CommonTelephonyData
						Dim type As String = SafeGetString(reader, "Typ")
						If type = "KS" Then
							viewData.ModulSource = TelephonyRecordSource.ResponsiblePerson
						ElseIf type = "KD" Then
							viewData.ModulSource = TelephonyRecordSource.Customer
						Else
							viewData.ModulSource = TelephonyRecordSource.Employee
						End If

						viewData.ZNumber = SafeGetInteger(reader, "ZNumber", 0)
						viewData.RecNumber = SafeGetInteger(reader, "RecNumber", 0)
						viewData.Lastname = SafeGetString(reader, "Nachname")
						viewData.Firstname = SafeGetString(reader, "Vorname")
						viewData.Company = SafeGetString(reader, "Firma")
						viewData.Telephon = SafeGetString(reader, "Telefon")


						result.Add(viewData)
					End While


				End If


			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				result = Nothing
			Finally
				CloseReader(reader)
			End Try


			Return result

		End Function

		Function AddCallHistory(ByVal mandantNr As Integer, ByVal callData As CallHistoryData) As Boolean Implements ICustomerDatabaseAccess.AddCallHistory

			Dim success = True

			Dim sql As String

			sql = "Insert Into CallHistory (EventTime, USNr, USName, CallHandle, CallID, CalledFrom, "
			sql &= "CalledTo, Incoming, KDNr, KDZHDNr, MANr, ModeNr, UserTapiID) "
			sql &= "Values ("
			sql &= "getdate()"
			sql &= ", @USNr"
			sql &= ", @USName"
			sql &= ", @CallHandle"
			sql &= ", @CallID"
			sql &= ", @CalledFrom"
			sql &= ", @CalledTo"
			sql &= ", @Incoming"
			sql &= ", @KDNr"
			sql &= ", @KDZHDNr"
			sql &= ", @MANr"
			sql &= ", @ModeNr"
			sql &= ", @UserTapiID"
			sql &= ")"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("USNr", ReplaceMissing(callData.USNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USName", ReplaceMissing(callData.Advisor, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CallHandle", ReplaceMissing(callData.CallHandle, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CallID", ReplaceMissing(callData.CallID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CalledFrom", ReplaceMissing(callData.CalledFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CalledTo", ReplaceMissing(callData.CalledTo, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Incoming", ReplaceMissing(callData.Incoming, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(callData.CustomerNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KDZHDNr", ReplaceMissing(callData.ResponslibePerson, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MANr", ReplaceMissing(callData.EmployeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ModeNr", ReplaceMissing(callData.ModeNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("UserTapiID", ReplaceMissing(callData.UserTapiID, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

			Return success

		End Function

	End Class


End Namespace
