
Imports SP.DatabaseAccess.ModulView.DataObjects

Namespace ModulView


	Partial Class ModulViewDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IModulViewDatabaseAccess

		Function LoadAssignedEmployeeContactData(ByVal mdNr As Integer, ByVal employeeNumber As Integer?, ByVal year As Integer?, ByVal month As Integer?,
												 ByVal recID As Integer?, ByVal showLatestEntries As Boolean?, ByVal contactPlainText As String) As IEnumerable(Of ModulViewEmployeeContactData) Implements IModulViewDatabaseAccess.LoadAssignedEmployeeContactData

			Dim result As List(Of ModulViewEmployeeContactData) = Nothing

			Dim sql As String

			sql = "[Load Assigned Employee Contact Data in ModulView]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("employeeNumber", ReplaceMissing(employeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("showLatestEntries", ReplaceMissing(showLatestEntries, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("recID", ReplaceMissing(recID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@contactPlainText", ReplaceMissing(contactPlainText, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("month", ReplaceMissing(month, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("year", ReplaceMissing(year, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If Not reader Is Nothing Then
					result = New List(Of ModulViewEmployeeContactData)
					Dim i As Integer = 0

					While reader.Read
						Dim overviewData = New ModulViewEmployeeContactData

						overviewData.contactnr = SafeGetInteger(reader, "RecNr", 0)
						overviewData.manr = SafeGetInteger(reader, "manr", Nothing)
						overviewData.kdnr = SafeGetInteger(reader, "kdnr", Nothing)
						overviewData.monat = SafeGetInteger(reader, "contactmonth", 0)
						overviewData.jahr = SafeGetInteger(reader, "contactyear", 0)
						overviewData.customername = SafeGetString(reader, "firma1")
						overviewData.employeename = SafeGetString(reader, "maname")
						overviewData.bezeichnung = SafeGetString(reader, "Bezeichnung")
						overviewData.beschreibung = SafeGetString(reader, "Beschreibung")
						overviewData.datum = SafeGetDateTime(reader, "datum", Nothing)
						overviewData.art = SafeGetString(reader, "kontakttype1")
						overviewData.createdon = SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = SafeGetString(reader, "createdfrom")


						result.Add(overviewData)

						If showLatestEntries AndAlso i > 10 Then Exit While
						i += 1

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