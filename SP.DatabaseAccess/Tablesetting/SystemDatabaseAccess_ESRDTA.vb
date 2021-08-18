
Imports SP.DatabaseAccess.TableSetting.DataObjects

Namespace TableSetting


	Partial Class TablesDatabaseAccess
		Inherits DatabaseAccessBase
		Implements ITablesDatabaseAccess


#Region "DTA Data"

		Function LoadMandantBankData(ByVal mandantenNumber As Integer, ByVal modulNumber As BankModulEnum) As IEnumerable(Of MDBankData) Implements ITablesDatabaseAccess.LoadMandantBankData

			Dim result As List(Of MDBankData) = Nothing

			Dim sql As String

			sql = "[Load Mandant Bank Data]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", mandantenNumber))
			listOfParams.Add(New SqlClient.SqlParameter("ModulNumber", ReplaceMissing(modulNumber, BankModulEnum.ESRDATA)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of MDBankData)

					While reader.Read

						Dim data = New MDBankData()

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.MDNr = SafeGetInteger(reader, "MDNr", Nothing)
						data.Jahr = SafeGetInteger(reader, "Jahr", 0)
						data.ModulArt = SafeGetInteger(reader, "ModulArt", BankModulEnum.ESRDATA)
						data.USNr = SafeGetInteger(reader, "USNr", 0)

						data.MD_ID = SafeGetString(reader, "MD_ID")
						data.KontoESR1 = SafeGetString(reader, "KontoESR1")
						data.KontoESR2 = SafeGetString(reader, "KontoESR2")

						data.DTAClnr = SafeGetString(reader, "DTAClnr")
						data.KontoDTA = SafeGetString(reader, "KontoDTA")
						data.KontoVG = SafeGetString(reader, "KontoVG")
						data.BankName = SafeGetString(reader, "BankName")
						data.BankClnr = SafeGetString(reader, "BankClnr")
						data.BankAdresse = SafeGetString(reader, "BankAdresse")
						data.Swift = SafeGetString(reader, "Swift")
						data.ESRIBAN1 = SafeGetString(reader, "ESRIBAN1")
						data.ESRIBAN2 = SafeGetString(reader, "ESRIBAN2")
						data.ESRIBAN3 = SafeGetString(reader, "ESRIBAN3")
						data.DTAIBAN = SafeGetString(reader, "DTAIBAN")
						data.VGIBAN = SafeGetString(reader, "VGIBAN")
						data.DTAAdr1 = SafeGetString(reader, "DTAAdr1")
						data.DTAAdr2 = SafeGetString(reader, "DTAAdr2")
						data.DTAAdr3 = SafeGetString(reader, "DTAAdr3")
						data.DTAAdr4 = SafeGetString(reader, "DTAAdr4")
						data.AsStandard = SafeGetBoolean(reader, "AsStandard", False)
						data.MD_ID = SafeGetString(reader, "MD_ID")
						data.MD_ID = SafeGetString(reader, "MD_ID")
						data.MD_ID = SafeGetString(reader, "MD_ID")
						data.MD_ID = SafeGetString(reader, "MD_ID")
						data.MD_ID = SafeGetString(reader, "MD_ID")

						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						data.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
						data.ChangedFrom = SafeGetString(reader, "ChangedFrom")
						data.RecBez = SafeGetString(reader, "RecBez")

						result.Add(data)

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


		''' <summary>
		''' update assigned data.
		''' </summary>
		''' <returns>boolean</returns>
		Function UpdateAssignedMandantBankData(ByVal data As MDBankData) As Boolean Implements ITablesDatabaseAccess.UpdateAssignedMandantBankData

			Dim success As Boolean = True

			Dim sql As String

			sql = "[Update Assigned Mandant Bank Data]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MD_ID", ReplaceMissing(data.MD_ID, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("KontoESR1", ReplaceMissing(data.KontoESR1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KontoESR2", ReplaceMissing(data.KontoESR2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DTAClnr", ReplaceMissing(data.DTAClnr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KontoDTA", ReplaceMissing(data.KontoDTA, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KontoVG", ReplaceMissing(data.KontoVG, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("BankName", ReplaceMissing(data.BankName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("BankClnr", ReplaceMissing(data.BankClnr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("BankAdresse", ReplaceMissing(data.BankAdresse, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Swift", ReplaceMissing(data.Swift, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ESRIBAN1", ReplaceMissing(data.ESRIBAN1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ESRIBAN2", ReplaceMissing(data.ESRIBAN2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ESRIBAN3", ReplaceMissing(data.ESRIBAN3, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DTAIBAN", ReplaceMissing(data.DTAIBAN, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("VGIBAN", ReplaceMissing(data.VGIBAN, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DTAAdr1", ReplaceMissing(data.DTAAdr1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DTAAdr2", ReplaceMissing(data.DTAAdr2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DTAAdr3", ReplaceMissing(data.DTAAdr3, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DTAAdr4", ReplaceMissing(data.DTAAdr4, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("AsStandard", ReplaceMissing(data.AsStandard, False)))
			listOfParams.Add(New SqlClient.SqlParameter("ChangedFrom", ReplaceMissing(data.ChangedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RecBez", ReplaceMissing(data.RecBez, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USNr", ReplaceMissing(data.USNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ID", ReplaceMissing(data.ID, DBNull.Value)))


			Try
				success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			Finally

			End Try

			Return success

		End Function

		''' <summary>
		''' add data.
		''' </summary>
		''' <returns>boolean</returns>
		Function AddNewMandantBankData(ByVal data As MDBankData) As Boolean Implements ITablesDatabaseAccess.AddNewMandantBankData

			Dim success As Boolean = True

			Dim sql As String

			sql = "[Create New Mandant Bank Data]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(data.MDNr, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("ModulArt", ReplaceMissing(data.ModulArt, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MD_ID", ReplaceMissing(data.MD_ID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KontoESR1", ReplaceMissing(data.KontoESR1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KontoESR2", ReplaceMissing(data.KontoESR2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DTAClnr", ReplaceMissing(data.DTAClnr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KontoDTA", ReplaceMissing(data.KontoDTA, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KontoVG", ReplaceMissing(data.KontoVG, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("BankName", ReplaceMissing(data.BankName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("BankClnr", ReplaceMissing(data.BankClnr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("BankAdresse", ReplaceMissing(data.BankAdresse, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Swift", ReplaceMissing(data.Swift, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ESRIBAN1", ReplaceMissing(data.ESRIBAN1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ESRIBAN2", ReplaceMissing(data.ESRIBAN2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ESRIBAN3", ReplaceMissing(data.ESRIBAN3, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DTAIBAN", ReplaceMissing(data.DTAIBAN, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("VGIBAN", ReplaceMissing(data.VGIBAN, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DTAAdr1", ReplaceMissing(data.DTAAdr1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DTAAdr2", ReplaceMissing(data.DTAAdr2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DTAAdr3", ReplaceMissing(data.DTAAdr3, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DTAAdr4", ReplaceMissing(data.DTAAdr4, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("AsStandard", ReplaceMissing(data.AsStandard, False)))
			listOfParams.Add(New SqlClient.SqlParameter("CreatedFrom", ReplaceMissing(data.CreatedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RecBez", ReplaceMissing(data.RecBez, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USNr", ReplaceMissing(data.USNr, DBNull.Value)))


			Try
				' New ID 
				Dim newIdParameter = New SqlClient.SqlParameter("NewID", SqlDbType.Int)
				newIdParameter.Direction = ParameterDirection.Output
				listOfParams.Add(newIdParameter)


				success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

				If success Then
					If Not newIdParameter.Value Is Nothing Then
						data.ID = CType(newIdParameter.Value, Integer)
					End If

				Else
					success = False
				End If

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			Finally

			End Try

			Return success

		End Function

		Function DeleteAssignedMandantBankData(ByVal id As Integer, ByVal advisorNumber As Integer) As SP.DatabaseAccess.Common.DataObjects.DeleteResult Implements ITablesDatabaseAccess.DeleteAssignedMandantBankData

			Dim success As Boolean = True

			Dim sql As String
			sql = "[Delete Assigned Mandant Bank Data]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("ID", ReplaceMissing(id, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USNr", ReplaceMissing(advisorNumber, DBNull.Value)))

			Try
				Dim resultParameter = New SqlClient.SqlParameter("@Result", SqlDbType.Int)
				resultParameter.Direction = ParameterDirection.Output
				listOfParams.Add(resultParameter)

				success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

				Dim resultEnum As SP.DatabaseAccess.Common.DataObjects.DeleteResult

				If Not resultParameter.Value Is Nothing Then
					Try
						resultEnum = CType(resultParameter.Value, SP.DatabaseAccess.Common.DataObjects.DeleteResult)
					Catch
						resultEnum = SP.DatabaseAccess.Common.DataObjects.DeleteResult.ErrorWhileDelete
					End Try
				Else
					resultEnum = SP.DatabaseAccess.Common.DataObjects.DeleteResult.ErrorWhileDelete
				End If

				Return resultEnum


			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			Finally

			End Try

			Return success

		End Function


#End Region



	End Class


End Namespace

