
Imports SP.DatabaseAccess.Listing.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language
Imports SP.DatabaseAccess.PayrollMng.DataObjects
Imports SP.DatabaseAccess.Report.DataObjects
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.ES.DataObjects.ESMng

Namespace Listing


	Partial Class ListingDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IListingDatabaseAccess



		Function LoadPermissionForEmploymentData(ByVal mdNr As Integer) As IEnumerable(Of Common.DataObjects.PermissionData) Implements IListingDatabaseAccess.LoadPermissionForEmploymentData

			Dim result As List(Of Common.DataObjects.PermissionData) = Nothing

			Dim sql As String

			sql = "[Show Permission Data For Search In Employment Listing]"


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

		Function LoadTaxForEmploymentData(ByVal mdNr As Integer) As IEnumerable(Of Listing.DataObjects.QSTCodeData) Implements IListingDatabaseAccess.LoadTaxForEmploymentData

			Dim result As List(Of Listing.DataObjects.QSTCodeData) = Nothing

			Dim sql As String

			sql = "[Show Tax Data For Search In Employment Listing]"


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

		Function LoadCountryForEmploymentData(ByVal mdNr As Integer) As IEnumerable(Of CountryData) Implements IListingDatabaseAccess.LoadCountryForEmploymentData

			Dim result As List(Of CountryData) = Nothing

			Dim sql As String

			sql = "[Show Country Data For Search In Employment Listing]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of CountryData)

					While reader.Read

						Dim data = New CountryData
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

		Function LoadNationalityForEmploymentData(ByVal mdNr As Integer) As IEnumerable(Of CountryData) Implements IListingDatabaseAccess.LoadNationalityForEmploymentData

			Dim result As List(Of CountryData) = Nothing

			Dim sql As String

			sql = "[Show Nationality Data For Search In Employment Listing]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of CountryData)

					While reader.Read

						Dim data = New CountryData
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

		Function LoadCivilstateForEmploymentData(ByVal mdNr As Integer) As IEnumerable(Of Common.DataObjects.CivilStateData) Implements IListingDatabaseAccess.LoadCivilstateForEmploymentData

			Dim result As List(Of Common.DataObjects.CivilStateData) = Nothing

			Dim sql As String

			sql = "[Show Civilstate Data For Search In Employment Listing]"


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

		Function LoadTaxCantonForEmploymentData(ByVal mdNr As Integer) As IEnumerable(Of Common.DataObjects.CantonData) Implements IListingDatabaseAccess.LoadTaxCantonForEmploymentData

			Dim result As List(Of Common.DataObjects.CantonData) = Nothing

			Dim sql As String

			sql = "[Show Taxcanton Data For Search In Employment Listing]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of Common.DataObjects.CantonData)

					While reader.Read

						Dim data = New Common.DataObjects.CantonData
						data.GetField = SafeGetString(reader, "S_Kanton")
						data.Description = SafeGetString(reader, "CodeLabel")


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


		Function LoadCustomerKSTForEmploymentData(ByVal mdNr As Integer) As IEnumerable(Of EmploymentCustomerCostcenterData) Implements IListingDatabaseAccess.LoadCustomerKSTForEmploymentData

			Dim result As List(Of EmploymentCustomerCostcenterData) = Nothing

			Dim sql As String

			sql = "[Show Customer Costcenter Data For Search In Employment Listing]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of EmploymentCustomerCostcenterData)

					While reader.Read

						Dim data = New EmploymentCustomerCostcenterData
						data.CostcenterLabel = SafeGetString(reader, "Bezeichnung")


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

		''' <summary>
		''' Loads ES data for active time.
		''' </summary>
		Function LoadActiveReportData(ByVal mdNr As Integer, ByVal startOfMonth As DateTime, ByVal endOfMonth As DateTime, ByVal userKST As String) As IEnumerable(Of RPMasterData) Implements IListingDatabaseAccess.LoadActiveReportData

			Dim result As List(Of RPMasterData) = Nothing

			Dim sql As String = String.Empty

			sql = "[List Report Data For Finishing Flag]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("@startOfMonth", startOfMonth))
			listOfParams.Add(New SqlClient.SqlParameter("@endOfMonth", endOfMonth))
			listOfParams.Add(New SqlClient.SqlParameter("@KST", userKST))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of RPMasterData)

					While reader.Read

						Dim data = New RPMasterData
						data.RPNR = SafeGetInteger(reader, "RPNr", 0)
						data.ESNR = SafeGetInteger(reader, "ESNr", 0)
						data.RPKST = SafeGetString(reader, "RPKst")
						data.Erfasst = SafeGetBoolean(reader, "Erfasst", Nothing)


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

		Function AddReportFinishingFlagCheck(ByVal mdNr As Integer, ByVal esNumber As Integer, ByVal rpNumber As Integer, ByVal userInfo As String) As Boolean Implements IListingDatabaseAccess.AddReportFinishingFlagCheck

			Dim success As Boolean = True

			Dim sql As String = String.Empty

			sql = "Insert Into [tbl_Report_FinishingFlagCheck] (MDNr, ESNr, RPNr, CheckedOn, CheckedFrom) Values ("
			sql &= "@MDNr, @ESNr, @RPNr, GetDate(), @CheckedFrom"
			sql &= ")"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("ESNr", ReplaceMissing(esNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RPNr", ReplaceMissing(rpNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CheckedFrom", ReplaceMissing(userInfo, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)


			Return success

		End Function


		Function LoadAssignedEmploymentsData(ByVal mdNr As Integer, ByVal esNumbers As List(Of Integer)) As IEnumerable(Of ESMasterData) Implements IListingDatabaseAccess.LoadAssignedEmploymentsData

			Dim result As List(Of ESMasterData) = Nothing
			Dim manrBuffer As String = String.Empty

			Dim sql As String

			Dim esNumbersBuffer As String = String.Empty

			For Each number In esNumbers

				esNumbersBuffer = esNumbersBuffer & IIf(esNumbersBuffer <> "", ", ", "") & number

			Next

			sql = "Select ES.ESNr, ES.MANr, ES.KDNr From ES "
			sql &= String.Format("WHERE ES.ESNr In ({0}) ", esNumbersBuffer)
			sql &= "Order By ESNr"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of ESMasterData)

					While reader.Read
						Dim data = New ESMasterData

						data.ESNR = SafeGetInteger(reader, "ESNr", 0)
						data.EmployeeNumber = SafeGetInteger(reader, "MANr", 0)
						data.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)


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

		Function LoadAssignedEmploymentDependentData(ByVal mdNr As Integer, ByVal esNumber As Integer) As IEnumerable(Of EmploymentDependentData) Implements IListingDatabaseAccess.LoadAssignedEmploymentDependentData

			Dim result As List(Of EmploymentDependentData) = Nothing

			Dim sql As String = String.Empty

			sql = "[Get ESData 4 Delete Selected ES]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			'listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("ESNr", ReplaceMissing(esNumber, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of EmploymentDependentData)

					While reader.Read
						Dim data = New EmploymentDependentData

						data.LONr = SafeGetInteger(reader, "RPNr", 0)
						data.LANr = SafeGetDecimal(reader, "LANr", 0)
						data.RENr = SafeGetInteger(reader, "RENr", 0)

						data.ESDoc_Guid = SafeGetString(reader, "ESDoc_Guid")
						data.VerleihDoc_Guid = SafeGetString(reader, "VerleihDoc_Guid")
						data.RPDoc_Guid = SafeGetString(reader, "RPDocGuid")


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

