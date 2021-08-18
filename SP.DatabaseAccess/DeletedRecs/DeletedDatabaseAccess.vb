Imports System.Data.SqlClient
Imports SP.Infrastructure.Logging
Imports SP.DatabaseAccess.Deleted.DataObjects

Imports SPProgUtility.Mandanten

Namespace Deleted

	''' <summary>
	''' deleted database access class.
	''' </summary>
	Public Class DeletedDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IDeletedDatabaseAccess

#Region "Constructor"

		''' <summary>
		''' Constructor.
		''' </summary>
		''' <param name="connectionString">The connection string.</param>
		''' <param name="translationLanguage">The translation language.</param>
		''' <remarks></remarks>
		Public Sub New(ByVal connectionString As String, ByVal translationLanguage As Language)
			MyBase.New(connectionString, translationLanguage)

		End Sub

		''' <summary>
		''' Constructor.
		''' </summary>
		''' <param name="connectionString">The connection string.</param>
		''' <param name="translationLanguage">The translation language.</param>
		''' <remarks></remarks>
		Public Sub New(ByVal connectionString As String, ByVal translationLanguage As String)
			MyBase.New(connectionString, translationLanguage)
		End Sub

#End Region

		''' <summary>
		''' Loads deleted data.
		''' </summary>
		''' <returns>List of deleted data.</returns>
		Function LoadDeletedRecsForSelectedModules(ByVal modulname As String,
																							 ByVal thisyear As Boolean?,
																							 ByVal thismonth As Boolean?) As IEnumerable(Of DeletedData) Implements IDeletedDatabaseAccess.LoadDeletedRecsForSelectedModules

			Dim result As List(Of DeletedData) = Nothing

			Dim sql As String

			sql = "Select id, DeletedModul, convert(Datetime, DeletedDate, 104) As DeletedDate, UserName, DeletedNr, RecInfo, DeletedDoc From DeleteInfo "
			sql &= "Where DeletedModul = @modulname "
			If thisyear.HasValue AndAlso thisyear Then
				sql &= "And Year(DeletedDate) = Year(Getdate()) "
			End If
			If thismonth.HasValue AndAlso thismonth Then
				sql &= "And Month(DeletedDate) = Month(GetDate()) And Year(DeletedDate) = Year(Getdate()) "
			End If
			sql &= "Order By ID DESC"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("@modulname", ReplaceMissing(modulname, Nothing)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of DeletedData)

					While reader.Read

						Dim deleteddata = New DeletedData()
						deleteddata.recid = SafeGetInteger(reader, "ID", 0)
						deleteddata.deletedmodul = SafeGetString(reader, "Deletedmodul")

						deleteddata.deletenumber = SafeGetInteger(reader, "DeletedNr", Nothing)
						deleteddata.deleteinfo = SafeGetString(reader, "RecInfo")

						deleteddata.createdfrom = SafeGetString(reader, "UserName")
						deleteddata.createdon = SafeGetDateTime(reader, "DeletedDate", Nothing)

						deleteddata.deletemodul = SafeGetString(reader, "DeletedModul")
						deleteddata.scandoc = SafeGetByteArray(reader, "DeletedDoc")


						result.Add(deleteddata)

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

		Function AddDeleteRecInfo(ByVal deleteData As DeletedData) As Boolean Implements IDeletedDatabaseAccess.AddDeleteRecInfo

			Dim success = True

			Dim sql As String

			sql = "[Add Deletedrec Info Data]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("DeletedModul", ReplaceMissing(deleteData.deletedmodul, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("UserName", ReplaceMissing(deleteData.createdfrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DeletedNr", ReplaceMissing(deleteData.deletenumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RecInfo", ReplaceMissing(deleteData.deleteinfo, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DeletedDoc", ReplaceMissing(deleteData.scandoc, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Return success

		End Function


		Function UpdateDeleteRecInfo(ByVal deleteData As DeletedData) As Boolean Implements IDeletedDatabaseAccess.UpdateDeleteRecInfo

			Dim success = True

			Dim sql As String

			sql = "Update DeleteInfo "
			sql &= "Set DeletedModul = @DeletedModul , "
			sql &= "DeletedDate = @DeletedDate, "
			sql &= "UserName = @UserName, "
			sql &= "DeletedNr = @DeletedNr, "
			sql &= "RecInfo = @RecInfo, "
			sql &= "DeletedDoc = @DeletedDoc "
			sql &= "Where DeletedModul = @DeletedModul "
			sql &= "And ID = (Select Top 1 [ID] From DeleteInfo Order By ID DESC) "

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("DeletedModul", ReplaceMissing(deleteData.deletedmodul, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DeletedDate", ReplaceMissing(Now, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("UserName", ReplaceMissing(deleteData.createdfrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DeletedNr", ReplaceMissing(deleteData.deletenumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RecInfo", ReplaceMissing(deleteData.deleteinfo, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DeletedDoc", ReplaceMissing(deleteData.scandoc, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.Text)

			Return success

		End Function


	End Class

End Namespace