
Imports SP.DatabaseAccess.SPPublicDataJob.DataObjects


Namespace SPPublicDataJob


	Partial Class SPPublicDataJobDatabaseAccess

		Inherits DatabaseAccessBase
		Implements ISPPublicDataJobDatabaseAccess


		Function AddCantonData(ByVal cantonData As CantonData) As Boolean Implements ISPPublicDataJobDatabaseAccess.AddCantonData

			Dim success = True

			Dim sql As String

			sql = "[Create New Canton Data]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("CantonID", ReplaceMissing(cantonData.cantonId, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CantonAbbreviation", ReplaceMissing(cantonData.CantonAbbreviation, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CantonLongName", ReplaceMissing(cantonData.CantonLongName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CantonDateOfChange", ReplaceMissing(cantonData.CantonDateOfChange, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success

		End Function

		Function AddDistrictData(ByVal districtData As DistrictData) As Boolean Implements ISPPublicDataJobDatabaseAccess.AddDistrictData

			Dim success = True

			Dim sql As String

			sql = "[Create New District Data]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("districtHistId", ReplaceMissing(districtData.districtHistId, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("cantonId", ReplaceMissing(districtData.cantonId, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("districtId", ReplaceMissing(districtData.districtId, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("districtLongName", ReplaceMissing(districtData.districtLongName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("districtShortName", ReplaceMissing(districtData.districtShortName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("districtEntryMode", ReplaceMissing(districtData.districtEntryMode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("districtAdmissionNumber", ReplaceMissing(districtData.districtAdmissionNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("districtAdmissionMode", ReplaceMissing(districtData.districtAdmissionMode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("districtAdmissionDate", ReplaceMissing(districtData.districtAdmissionDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("districtAbolitionNumber", ReplaceMissing(districtData.districtAbolitionNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("districtAbolitionMode", ReplaceMissing(districtData.districtAbolitionMode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("districtAbolitionDate", ReplaceMissing(districtData.districtAbolitionDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("districtDateOfChange", ReplaceMissing(districtData.districtDateOfChange, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success

		End Function

		Function AddMunicipalityData(ByVal municipalityData As MunicipalityData) As Boolean Implements ISPPublicDataJobDatabaseAccess.AddMunicipalityData

			Dim success = True

			Dim sql As String

			sql = "[Create New Municipality Data]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("HistoryMunicipalityId", ReplaceMissing(municipalityData.HistoryMunicipalityId, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DistrictHistId", ReplaceMissing(municipalityData.DistrictHistId, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CantonAbbreviation", ReplaceMissing(municipalityData.CantonAbbreviation, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MunicipalityId", ReplaceMissing(municipalityData.MunicipalityId, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MunicipalityLongName", ReplaceMissing(municipalityData.MunicipalityLongName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MunicipalityShortName", ReplaceMissing(municipalityData.MunicipalityShortName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MunicipalityEntryMode", ReplaceMissing(municipalityData.MunicipalityEntryMode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MunicipalityStatus", ReplaceMissing(municipalityData.MunicipalityStatus, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MunicipalityAdmissionNumber", ReplaceMissing(municipalityData.MunicipalityAdmissionNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MunicipalityAdmissionMode", ReplaceMissing(municipalityData.MunicipalityAdmissionMode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MunicipalityAdmissionDate", ReplaceMissing(municipalityData.MunicipalityAdmissionDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MunicipalityAbolitionNumber", ReplaceMissing(municipalityData.MunicipalityAbolitionNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MunicipalityAbolitionMode", ReplaceMissing(municipalityData.MunicipalityAbolitionMode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MunicipalityAbolitionDate", ReplaceMissing(municipalityData.MunicipalityAbolitionDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("municipalityDateOfChange", ReplaceMissing(municipalityData.MunicipalityDateOfChange, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success

		End Function


	End Class


End Namespace
