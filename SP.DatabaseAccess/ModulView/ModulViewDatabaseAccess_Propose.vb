
Imports SP.DatabaseAccess.ModulView.DataObjects

Namespace ModulView


	Partial Class ModulViewDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IModulViewDatabaseAccess

		Function LoadAssignedProposeContactData(ByVal mdNr As Integer, ByVal proposeNumber As Integer?, ByVal recID As Integer?, ByVal showLatestEntries As Boolean?) As IEnumerable(Of ModulViewProposeContactData) Implements IModulViewDatabaseAccess.LoadAssignedProposeContactData

			Dim result As List(Of ModulViewProposeContactData) = Nothing

			Dim sql As String

			sql = "[Load Assigned Propose Contact Data in ModulView]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("proposeNr", ReplaceMissing(proposeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("showLatestEntries", ReplaceMissing(showLatestEntries, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("recID", ReplaceMissing(recID, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If Not reader Is Nothing Then
					result = New List(Of ModulViewProposeContactData)
					Dim i As Integer = 0

					While reader.Read
						Dim overviewData = New ModulViewProposeContactData

						overviewData.employeeMDNr = SafeGetInteger(reader, "employeemdnr", 0)
						overviewData.customerMDNr = SafeGetInteger(reader, "customerMDNr", 0)
						overviewData.contactnr = SafeGetInteger(reader, "RecNr", 0)
						overviewData.manr = SafeGetInteger(reader, "manr", Nothing)
						overviewData.kdnr = SafeGetInteger(reader, "kdnr", Nothing)
						overviewData.monat = SafeGetInteger(reader, "contactmonth", 0)
						overviewData.jahr = SafeGetInteger(reader, "contactyear", 0)
						overviewData.customername = SafeGetString(reader, "firma1")
						overviewData.zhdname = SafeGetString(reader, "zname")
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