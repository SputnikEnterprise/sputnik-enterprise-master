
Imports SP.DatabaseAccess.CVLizer.DataObjects


Namespace CVLizer

	Partial Class CVLizerDatabaseAccess

		Inherits DatabaseAccessBase
		Implements ICVLizerDatabaseAccess


#Region "statistic methodes"

		Function AddCVLStatisticData(ByVal cvlProfileID As Integer, ByVal data As CVCodeSummaryData) As Boolean Implements ICVLizerDatabaseAccess.AddCVLStatisticData
			Dim success As Boolean

			Dim sql = "[CreateCVLStatistic]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@CVLProfileID", ReplaceMissing(cvlProfileID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Code", ReplaceMissing(data.Code, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Name", ReplaceMissing(data.Name, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Weight", ReplaceMissing(data.Weight, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Duration", ReplaceMissing(data.Duration, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Domain", ReplaceMissing(data.Domain, DBNull.Value)))


			' Output Parameters
			Dim newStatisticIdParameter = New SqlClient.SqlParameter("@NewStatisticId", SqlDbType.Int)
			newStatisticIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newStatisticIdParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If Not newStatisticIdParameter.Value Is Nothing Then
				data.ID = CType(newStatisticIdParameter.Value, Integer)
			Else
				success = False
			End If

			Return success
		End Function








#End Region


	End Class


End Namespace
