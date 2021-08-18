
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Listing.DataObjects


Namespace Listing


	Partial Class ListingDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IListingDatabaseAccess

		Function LoadAllEmployeeEmploymentTypeData() As IEnumerable(Of EmploymentTypeData) Implements IListingDatabaseAccess.LoadAllEmployeeEmploymentTypeData

			Dim result As List(Of EmploymentTypeData) = Nothing

			Dim sql As String

			sql = "SELECT "
			sql &= "EmploymentType "
			sql &= "FROM Mitarbeiter MA "
			sql &= "WHERE ISNULL(EmploymentType, '') <> '' "
			sql &= "GROUP BY EmploymentType "
			sql &= "ORDER BY EmploymentType"


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmploymentTypeData)

					While reader.Read

						Dim data = New EmploymentTypeData

						data.Rec_Value = SafeGetString(reader, "EmploymentType")


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

		Function LoadAllEmployeeOtherEmploymentTypeData() As IEnumerable(Of EmploymentTypeData) Implements IListingDatabaseAccess.LoadAllEmployeeOtherEmploymentTypeData

			Dim result As List(Of EmploymentTypeData) = Nothing

			Dim sql As String

			sql = "SELECT "
			sql &= "OtherEmploymentType "
			sql &= "FROM Mitarbeiter MA "
			sql &= "WHERE ISNULL(OtherEmploymentType, '') <> '' "
			sql &= "GROUP BY OtherEmploymentType "
			sql &= "ORDER BY OtherEmploymentType"


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmploymentTypeData)

					While reader.Read

						Dim data = New EmploymentTypeData

						data.Rec_Value = SafeGetString(reader, "OtherEmploymentType")


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

		Function LoadAllEmployeeTypeOfStayData() As IEnumerable(Of TypeOfStayData) Implements IListingDatabaseAccess.LoadAllEmployeeTypeOfStayData

			Dim result As List(Of TypeOfStayData) = Nothing

			Dim sql As String

			sql = "SELECT "
			sql &= "TypeofStay "
			sql &= "FROM Mitarbeiter MA "
			sql &= "WHERE ISNULL(TypeofStay, '') <> '' "
			sql &= "GROUP BY TypeofStay "
			sql &= "ORDER BY TypeofStay"


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of TypeOfStayData)

					While reader.Read

						Dim data = New TypeOfStayData

						data.Rec_Value = SafeGetString(reader, "TypeofStay")


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

		Function LoadAllEmployeeForeignCategoryData() As IEnumerable(Of PermissionData) Implements IListingDatabaseAccess.LoadAllEmployeeForeignCategoryData

			Dim result As List(Of PermissionData) = Nothing

			Dim sql As String

			sql = "SELECT "
			sql &= "ForeignCategory "
			sql &= "FROM Mitarbeiter MA "
			sql &= "WHERE ISNULL(ForeignCategory, '') <> '' "
			sql &= "GROUP BY ForeignCategory "
			sql &= "ORDER BY ForeignCategory"


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of PermissionData)

					While reader.Read

						Dim data = New PermissionData

						data.Rec_Value = SafeGetString(reader, "ForeignCategory")


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

		Function LoadAllEmployeeTaxCantonData() As IEnumerable(Of CantonData) Implements IListingDatabaseAccess.LoadAllEmployeeTaxCantonData

			Dim result As List(Of CantonData) = Nothing

			Dim sql As String

			sql = "SELECT "
			sql &= "S_Kanton "
			sql &= "FROM Mitarbeiter MA "
			sql &= "WHERE ISNULL(S_Kanton, '') <> '' "
			sql &= "GROUP BY S_Kanton "
			sql &= "ORDER BY S_Kanton"


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CantonData)

					While reader.Read

						Dim data = New CantonData

						data.Canton = SafeGetString(reader, "S_Kanton")


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

		Function LoadAllEmployeeBirthPlaceData() As IEnumerable(Of AnyStringValueData) Implements IListingDatabaseAccess.LoadAllEmployeeBirthPlaceData

			Dim result As List(Of AnyStringValueData) = Nothing

			Dim sql As String

			sql = "SELECT "
			sql &= "GebOrt "
			sql &= "FROM Mitarbeiter MA "
			sql &= "WHERE ISNULL(GebOrt, '') <> '' "
			sql &= "GROUP BY GebOrt "
			sql &= "ORDER BY GebOrt"


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of AnyStringValueData)

					While reader.Read

						Dim data = New AnyStringValueData

						data.FieldValue = SafeGetString(reader, "GebOrt")


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


		Function LoadPermissionForEmployeeSearchData(ByVal mdNr As Integer) As IEnumerable(Of Common.DataObjects.PermissionData) Implements IListingDatabaseAccess.LoadPermissionForEmployeeSearchData

			Dim result As List(Of Common.DataObjects.PermissionData) = Nothing

			Dim sql As String

			sql = "[Show Permission Data For Search In Employee Listing]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of Common.DataObjects.PermissionData)

					While reader.Read

						Dim data = New Common.DataObjects.PermissionData
						data.RecValue = SafeGetString(reader, "PermissionCode")
						data.TranslatedPermission = SafeGetString(reader, "CodeLabel")


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

		Function LoadTaxForEmployeeSearchData(ByVal mdNr As Integer) As IEnumerable(Of Listing.DataObjects.QSTCodeData) Implements IListingDatabaseAccess.LoadTaxForEmployeeSearchData

			Dim result As List(Of Listing.DataObjects.QSTCodeData) = Nothing

			Dim sql As String

			sql = "[Show Tax Data For Search In Employee Listing]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of Listing.DataObjects.QSTCodeData)

					While reader.Read

						Dim data = New Listing.DataObjects.QSTCodeData
						data.QSTCode = SafeGetString(reader, "TaxCode")
						data.QSTCodeLabel = SafeGetString(reader, "CodeLabel")


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

		Function LoadCommunityForEmployeeSearchData(ByVal mdNr As Integer) As IEnumerable(Of QSTCommunityData) Implements IListingDatabaseAccess.LoadCommunityForEmployeeSearchData

			Dim result As List(Of QSTCommunityData) = Nothing

			Dim sql As String
			Dim laNumbersBuffer As String = String.Empty

			sql = "[Show Community Data For Search In Employee Listing]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNr, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of QSTCommunityData)

					While reader.Read

						Dim data As New QSTCommunityData
						data.CommunityCode = SafeGetInteger(reader, "TaxCommunityCode", 0)
						data.CommunityName = SafeGetString(reader, "TaxCommunityLabel")

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

		Function LoadCountryForEmployeeSearchData(ByVal mdNr As Integer) As IEnumerable(Of Common.DataObjects.CountryData) Implements IListingDatabaseAccess.LoadCountryForEmployeeSearchData

			Dim result As List(Of Common.DataObjects.CountryData) = Nothing

			Dim sql As String

			sql = "[Show Country Data For Search In Employee Listing]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of Common.DataObjects.CountryData)

					While reader.Read

						Dim data = New Common.DataObjects.CountryData
						data.Code = SafeGetString(reader, "Code")
						data.Name = SafeGetString(reader, "LandName")


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

		Function LoadNationalityForEmployeeSearchData(ByVal mdNr As Integer) As IEnumerable(Of Common.DataObjects.CountryData) Implements IListingDatabaseAccess.LoadNationalityForEmployeeSearchData

			Dim result As List(Of Common.DataObjects.CountryData) = Nothing

			Dim sql As String

			sql = "[Show Nationality Data For Search In Employee Listing]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of Common.DataObjects.CountryData)

					While reader.Read

						Dim data = New Common.DataObjects.CountryData
						data.Code = SafeGetString(reader, "Code")
						data.Name = SafeGetString(reader, "LandName")


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

		Function LoadCivilstateForEmployeeSearchData(ByVal mdNr As Integer) As IEnumerable(Of Common.DataObjects.CivilStateData) Implements IListingDatabaseAccess.LoadCivilstateForEmployeeSearchData

			Dim result As List(Of Common.DataObjects.CivilStateData) = Nothing

			Dim sql As String

			sql = "[Show Civilstate Data For Search In Employee Listing]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of Common.DataObjects.CivilStateData)

					While reader.Read

						Dim data = New Common.DataObjects.CivilStateData
						data.GetField = SafeGetString(reader, "Zivilstand")
						data.TranslatedCivilState = SafeGetString(reader, "CodeLabel")


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
