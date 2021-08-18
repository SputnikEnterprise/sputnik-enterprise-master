Imports SP.DatabaseAccess.Listing.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language
Imports SP.DatabaseAccess.Customer.DataObjects

Namespace Listing



	Partial Class ListingDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IListingDatabaseAccess


		Function LoadDowntimeCustomerData(ByVal mdNr As Integer, ByVal month As Integer, ByVal year As Integer) As IEnumerable(Of CustomerMasterData) Implements IListingDatabaseAccess.LoadDowntimeCustomerData

			Dim result As List(Of CustomerMasterData) = Nothing

			Dim sql As String

			sql = "Select LOL.DestKDNr KDNr, KD.Firma1, KD.Strasse, KD.PLZ, KD.Ort "
			sql &= "From dbo.LOL LOL LEFT Join dbo.Kunden KD On KD.KDNr = LOL.DestKDNr "
			sql &= "Where LOL.MDNr = @MDNr "
			sql &= "AND (@month = 0 OR Convert(Int, LOL.LP) = @month) "
			sql &= "AND (@year = 0 OR Convert(Int, LOL.Jahr) = @year) "
			sql &= "AND EXISTS(SELECT L.DestKDNr FROM LOL L WHERE L.MDNr = @MDNr AND L.LANr IN (103.01, 1000.01) AND L.DestKDNr = LOL.DestKDNr ) "
			sql &= "AND ISNULL(LOL.DestKDNr, 0) <> 0 "
			sql &= "AND LOL.LANr IN (103.01, 1000.01) "
			sql &= "GROUP BY LOL.DestKDNr, KD.Firma1, KD.Strasse, KD.PLZ, KD.Ort "
			sql &= "ORDER BY KD.Firma1 ASC"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("month", ReplaceMissing(month, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("year", ReplaceMissing(year, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CustomerMasterData)

					While reader.Read

						Dim customerData = New CustomerMasterData()
						customerData.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)
						customerData.Company1 = SafeGetString(reader, "Firma1")
						customerData.Street = SafeGetString(reader, "Strasse")
						customerData.Postcode = SafeGetString(reader, "PLZ")
						customerData.Location = SafeGetString(reader, "Ort")

						result.Add(customerData)

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

		Function LoadDowntimeDataData(ByVal mdNr As Integer, ByVal customerNumber As Integer?, ByVal month As Integer, ByVal year As Integer) As IEnumerable(Of DowntimeData) Implements IListingDatabaseAccess.LoadDowntimeDataData

			Dim result As List(Of DowntimeData) = Nothing

			Dim sql As String

			sql = "[Load Short Working Time Data For Listing]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("month", ReplaceMissing(month, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("year", ReplaceMissing(year, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(customerNumber, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of DowntimeData)

					While reader.Read
						Dim data = New DowntimeData

						data.MDNr = SafeGetInteger(reader, "MDNr", Nothing)
						data.EmployeeNumber = SafeGetInteger(reader, "MANr", Nothing)
						data.CustomerNumber = SafeGetInteger(reader, "KDNr", Nothing)
						data.PayrollNumber = SafeGetInteger(reader, "LONr", Nothing)
						data.PayrollMonth = month
						data.PayrollYear = year

						data.EmployeeLastname = SafeGetString(reader, "EmployeeLastname")
						data.EmployeeFirstname = SafeGetString(reader, "EmployeeFirstname")
						data.EmployeeSocialSecurityNumber = SafeGetString(reader, "EmployeeSocialSecurityNumber")
						data.EmployeeBirthdate = SafeGetDateTime(reader, "EmployeeBirthDate", Nothing)

						data.LALabel = SafeGetString(reader, "LALabel")
						data.Customername = SafeGetString(reader, "Customername")
						data.NeedAttention = SafeGetBoolean(reader, "NeedAttention", False)

						data.TargetHours = SafeGetDecimal(reader, "TargetHours", Nothing)
						data.DowntimeHours = SafeGetDecimal(reader, "DowntimeHours", Nothing)
						data.DowntimeProcentage = SafeGetDecimal(reader, "DowntimeProcentage", Nothing)
						data.AHVBasis = SafeGetDecimal(reader, "AHVBasis", Nothing)
						data.DowntimeAHVBasis = SafeGetDecimal(reader, "DowntimeAHVBasis", Nothing)
						data.DowntimeCompensation = SafeGetDecimal(reader, "DowntimeCompensation", Nothing)
						data.DowntimeCompensationAG = SafeGetDecimal(reader, "DowntimeCompensationAG", Nothing)
						data.TotalCompensation = SafeGetDecimal(reader, "TotalCompensation", Nothing)


						result.Add(data)

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


	End Class

End Namespace
