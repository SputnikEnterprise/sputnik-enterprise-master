
Imports System.Data.SqlClient
Imports SP.DatabaseAccess.Propose.DataObjects


Namespace Propose

	Partial Class ProposeDatabaseAccess

		Inherits DatabaseAccessBase
		Implements IProposeDatabaseAccess


		''' <summary>
		''' Loads propose master data.
		''' </summary>
		Function LoadProposeMasterData(ByVal proposeNumber As Integer) As ProposeMasterData Implements IProposeDatabaseAccess.LoadProposeMasterData
			Dim result As ProposeMasterData = Nothing

			Dim sql As String

			sql = "SELECT P.ID "
			sql &= ",P.ProposeNr"
			sql &= ",P.MANr"
			sql &= ",P.KDNr"
			sql &= ",P.KDZHDNr"
			sql &= ",P.VakNr"
			sql &= ",P.ApplicationNumber"
			sql &= ",P.KST"
			sql &= ",P.Berater"
			sql &= ",P.KD_Kst"
			sql &= ",P.MA_Kst"

			sql &= ",IsNull( (Select Top (1) USNr From Benutzer US Where US.KST = P.MA_Kst), 0) MA_USNr"
			sql &= ",IsNull( (Select Top (1) USNr From Benutzer US Where US.KST = P.KD_Kst), 0) KD_USNr"

			sql &= ",P.Bezeichnung"
			sql &= ",P.P_State"
			sql &= ",P.P_Art"
			sql &= ",P.KD_Tarif"
			sql &= ",P.MA_ESAls"
			sql &= ",P.P_Anstellung"
			sql &= ",P.P_ArbBegin"
			sql &= ",P.P_Zusatz1"
			sql &= ",P.P_Zusatz2"
			sql &= ",P.P_Zusatz3"
			sql &= ",P.P_Zusatz4"
			sql &= ",P.CreatedOn"
			sql &= ",P.CreatedFrom"
			sql &= ",P.ChangedOn"
			sql &= ",P.ChangedFrom"
			sql &= ",P.Ab_AnstellungAls"
			sql &= ",P.Ab_AntrittPer"
			sql &= ",P.Ab_LohnBas"
			sql &= ",convert(Int, P.Ab_LohnAnz) Ab_LohnAnz"
			sql &= ",P.Ab_LohnBetrag"
			sql &= ",P.Ab_HBas"
			sql &= ",P.Ab_HAns"
			sql &= ",P.Ab_HBetrag"
			sql &= ",P.Ab_REPer"
			sql &= ",P.Ab_Bemerkung"
			sql &= ",P.Ab_RePer_Date"
			sql &= ",P.P_Zusatz5"
			sql &= ",P.P_ArbZeit"
			sql &= ",P.P_Spesen"
			sql &= ",P.P_ArbBegin_Date"
			sql &= ",P._P_Zusatz1"
			sql &= ",P._P_Zusatz2"
			sql &= ",P._P_Zusatz3"
			sql &= ",P._P_Zusatz4"
			sql &= ",P._P_Zusatz5"
			sql &= ",P.Doc_Guid"
			sql &= ",P.MDNr "

			sql &= " FROM Propose P "
			sql &= "WHERE P.ProposeNr = @proposeNumber "


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("proposeNumber", ReplaceMissing(proposeNumber, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then
					result = New ProposeMasterData

					Dim data = New ProposeMasterData()

					data.ID = SafeGetInteger(reader, "ID", 0)
					data.ProposeNr = SafeGetInteger(reader, "ProposeNr", 0)
					data.MANr = SafeGetInteger(reader, "MANr", 0)
					data.KDNr = SafeGetInteger(reader, "KDNr", 0)
					data.KDZHDNr = SafeGetInteger(reader, "KDZHDNr", 0)
					data.VakNr = SafeGetInteger(reader, "VakNr", 0)
					data.ApplicationNumber = SafeGetInteger(reader, "ApplicationNumber", 0)

					data.KST = SafeGetString(reader, "KST")
					data.Berater = SafeGetString(reader, "Berater")
					data.KD_Kst = SafeGetString(reader, "KD_Kst")
					data.MA_Kst = SafeGetString(reader, "MA_Kst")
					data.Employee_UserNumber = SafeGetInteger(reader, "MA_USNr", 0)
					data.Customer_UserNumber = SafeGetInteger(reader, "KD_USNr", 0)

					data.Bezeichnung = SafeGetString(reader, "Bezeichnung")
					data.P_State = SafeGetString(reader, "P_State")
					data.P_Art = SafeGetString(reader, "P_Art")
					data.KD_Tarif = SafeGetString(reader, "KD_Tarif")
					data.MA_ESAls = SafeGetString(reader, "MA_ESAls")
					data.P_Anstellung = SafeGetString(reader, "P_Anstellung")
					data.P_ArbBegin = SafeGetString(reader, "P_ArbBegin")
					data.P_Zusatz1 = SafeGetString(reader, "P_Zusatz1")
					data.P_Zusatz2 = SafeGetString(reader, "P_Zusatz2")
					data.P_Zusatz3 = SafeGetString(reader, "P_Zusatz3")
					data.P_Zusatz4 = SafeGetString(reader, "P_Zusatz4")
					data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
					data.CreatedFrom = SafeGetString(reader, "CreatedFrom")
					data.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
					data.ChangedFrom = SafeGetString(reader, "ChangedFrom")
					data.Ab_AnstellungAls = SafeGetString(reader, "Ab_AnstellungAls")
					data.Ab_AntrittPer = SafeGetString(reader, "Ab_AntrittPer")
					data.Ab_LohnBas = SafeGetDecimal(reader, "Ab_LohnBas", 0)
					data.Ab_LohnAnz = SafeGetInteger(reader, "Ab_LohnAnz", 0)
					data.Ab_LohnBetrag = SafeGetDecimal(reader, "Ab_LohnBetrag", 0)
					data.Ab_HBas = SafeGetDecimal(reader, "Ab_HBas", 0)
					data.Ab_HAns = SafeGetDecimal(reader, "Ab_HAns", 0)
					data.Ab_HBetrag = SafeGetDecimal(reader, "Ab_HBetrag", 0)
					data.Ab_REPer = SafeGetString(reader, "Ab_REPer")
					data.Ab_Bemerkung = SafeGetString(reader, "Ab_Bemerkung")
					data.Ab_RePer_Date = SafeGetDateTime(reader, "Ab_RePer_Date", Nothing)
					data.P_Zusatz5 = SafeGetString(reader, "P_Zusatz5")
					data.P_ArbZeit = SafeGetString(reader, "P_ArbZeit")
					data.P_Spesen = SafeGetString(reader, "P_Spesen")
					data.P_ArbBegin_Date = SafeGetDateTime(reader, "P_ArbBegin_Date", Nothing)
					data._P_Zusatz1_Html = SafeGetString(reader, "_P_Zusatz1")
					data._P_Zusatz2_Html = SafeGetString(reader, "_P_Zusatz2")
					data._P_Zusatz3_Html = SafeGetString(reader, "_P_Zusatz3")
					data._P_Zusatz4_Html = SafeGetString(reader, "_P_Zusatz4")
					data._P_Zusatz5_Html = SafeGetString(reader, "_P_Zusatz5")
					data.Doc_Guid = SafeGetString(reader, "Doc_Guid")
					data.MDNr = SafeGetInteger(reader, "MDNr", 0)


					result = data

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result
		End Function

		Function DuplicateProposeData(ByVal mdNumber As Integer, ByVal oldProposeNumber As Integer, ByVal proposeMasterData As ProposeMasterData) As Boolean Implements IProposeDatabaseAccess.DuplicateProposeData
			Dim success As Boolean = True

			Try

				Dim sql As String

				sql = "[Duplicate Propose With Exisiting Propose Data]"


				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNumber, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("OldProposeNumber", ReplaceMissing(oldProposeNumber, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("ProposeNumberOffset", ReplaceMissing(proposeMasterData.ProposeNumberOffset, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("MANr", ReplaceMissing(proposeMasterData.MANr, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(proposeMasterData.KDNr, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("KDZHDNr", ReplaceMissing(proposeMasterData.KDZHDNr, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("VakNr", ReplaceMissing(proposeMasterData.VakNr, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("CreatedFrom", ReplaceMissing(proposeMasterData.CreatedFrom, DBNull.Value)))


				Dim newIdParameter = New SqlClient.SqlParameter("IdNewPropose", SqlDbType.Int)
				newIdParameter.Direction = ParameterDirection.Output
				listOfParams.Add(newIdParameter)

				success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

				If success AndAlso Not newIdParameter.Value Is Nothing Then
					proposeMasterData.ProposeNr = CType(newIdParameter.Value, Integer)
				Else
					success = False
				End If


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				success = False

			End Try

			Return success

		End Function

		Function DeleteProposeData(ByVal proposeNumber As Integer, ByVal userNumber As Integer) As Boolean Implements IProposeDatabaseAccess.DeleteProposeData
			Dim success As Boolean = True

			Try

				Dim sql As String

				sql = "[Delete Selected Propose]"


				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("PNumber", ReplaceMissing(proposeNumber, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("USNr", ReplaceMissing(userNumber, DBNull.Value)))

				Dim resultParameter = New SqlClient.SqlParameter("@Result", SqlDbType.Int)
				resultParameter.Direction = ParameterDirection.Output
				listOfParams.Add(resultParameter)

				success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)
				Dim resultEnum As DeleteProposeResult

				If Not resultParameter.Value Is Nothing Then
					Try
						resultEnum = CType(resultParameter.Value, DeleteProposeResult)
					Catch
						resultEnum = DeleteProposeResult.ErrorWhileDelete
					End Try
				Else
					resultEnum = DeleteProposeResult.ErrorWhileDelete
				End If
				success = DeleteProposeResult.Deleted


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				success = False

			End Try

			Return success
		End Function

		''' <summary>
		''' Loads context menu data for print.
		''' </summary>
		Function LoadContextMenu4PrintData() As IEnumerable(Of ContextMenuForPrint) Implements IProposeDatabaseAccess.LoadContextMenu4PrintData

			Dim result As List(Of ContextMenuForPrint) = Nothing

			Dim sql As String

			sql = "[Get List Of Documents For Print in Propose]"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ContextMenuForPrint)

					Dim mnuItems As New ContextMenuForPrint

					While reader.Read()
						mnuItems = New ContextMenuForPrint
						mnuItems.MnuName = SafeGetString(reader, "jobNr", String.Empty)
						mnuItems.MnuCaption = SafeGetString(reader, "Bezeichnung", String.Empty)

						result.Add(mnuItems)

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

	End Class

End Namespace
