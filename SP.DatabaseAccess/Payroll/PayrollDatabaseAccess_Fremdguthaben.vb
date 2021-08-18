Imports SP.DatabaseAccess
Imports System.Text
Imports SP.Infrastructure
Imports SP.DatabaseAccess.PayrollMng.DataObjects
Imports SP.Infrastructure.DateAndTimeCalculation

Namespace PayrollMng



	Partial Public Class PayrollDatabaseAccess

		Inherits DatabaseAccessBase
		Implements IPayrollDatabaseAccess

		''' <summary>
		''' Loads fremd guthaben data form lol.
		''' </summary>
		''' <returns>List of LA Bez data or nothing in error case.</returns>
		Function LoadFremdGuthabenData(ByVal mandantNumber As Integer, ByVal employeeNumber As Integer) As IEnumerable(Of LOLDataFoRepeatLA4LOBack) Implements IPayrollDatabaseAccess.LoadFremdGuthabenData

			Dim result As List(Of LOLDataFoRepeatLA4LOBack) = Nothing

			Dim sql As String

			sql = "SELECT LOL.ID "
			sql &= ", LOL.LONR "
			sql &= ", LOL.MANR "
			sql &= ", LOL.LANR "
			sql &= ", Convert(Int, LOL.LP) LP "
			sql &= ", Convert(Int, LOL.Jahr) Jahr "
			sql &= ", LOL.ModulName "
			sql &= ", LOL.Currency "
			sql &= ", LOL.M_ANZ "
			sql &= ", LOL.M_BAS "
			sql &= ", LOL.M_ANS "
			sql &= ", LOL.M_BTR "
			sql &= ", LOL.SUVA "
			sql &= ", LOL.KST "
			sql &= ", LOL.RPText "
			sql &= ", LOL.AGLA "
			sql &= ", LOL.S_Kanton "
			sql &= ", LOL.Result "
			sql &= ", LOL.KW "
			sql &= ", LOL.LOLKst1 "
			sql &= ", LOL.LOLKst2 "
			sql &= ", LOL.DestRPNr "
			sql &= ", LOL.DestZGNr "
			sql &= ", LOL.DestLMNr "
			sql &= ", LOL.KW2 "
			sql &= ", LOL.ZGAusDate "
			sql &= ", LOL.LMWithDTA "
			sql &= ", LOL.ZGGrund "
			sql &= ", LOL.BnkNr "
			sql &= ", LOL.VGNr "
			sql &= ", LOL.DTADate "
			sql &= ", LOL.GAV_Kanton "
			sql &= ", LOL.GAV_Beruf "
			sql &= ", LOL.GAV_Gruppe1 "
			sql &= ", LOL.GAV_Gruppe2 "
			sql &= ", LOL.GAV_Gruppe3 "
			sql &= ", LOL.GAV_Text "
			sql &= ", LOL.DestESNr "
			sql &= ", LOL.DateOfLO "
			sql &= ", LOL.QSTGemeinde "
			sql &= ", LOL.DestKDNr "
			sql &= ", LOL.ESBranche "
			sql &= ", LOL.ESEinstufung "
			sql &= ", LOL.MDNr "
			sql &= ", LOL.GAVNr "
			sql &= ", LA.LALOText "
			sql &= "From LOL "
			sql &= "Left Join LA On LA.LANr = LOL.LANr And LA.LAJahr = LOL.Jahr AND LA.LADeactivated = 0 "

			sql &= "Where LOL.MDNr = @mdnr "
			sql &= "And LOL.MANr = @maNr "
			sql &= "And LOL.LANr In ( "
			sql &= "530, 560 "
			sql &= ",630, 660 "
			sql &= ",730, 760 "
			sql &= ",529.10, 559.10 "
			sql &= ",629.10, 659.10 "
			sql &= ",729.10, 759.10 "
			sql &= ") "

			sql &= "Order By LOL.LANr"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@mdnr", mandantNumber))
			listOfParams.Add(New SqlClient.SqlParameter("@maNr", employeeNumber))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of LOLDataFoRepeatLA4LOBack)

					While reader.Read

						Dim data = New LOLDataFoRepeatLA4LOBack

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.MDNr = SafeGetInteger(reader, "MDNr", 0)
						data.LONr = SafeGetInteger(reader, "LONr", 0)
						data.MANr = SafeGetInteger(reader, "MANr", 0)
						data.LP = SafeGetInteger(reader, "LP", 0)
						data.Jahr = SafeGetInteger(reader, "Jahr", 0)
						data.LANr = SafeGetDecimal(reader, "LANr", Nothing)

						data.RPText = SafeGetString(reader, "RPText", String.Empty)
						data.ModulName = SafeGetString(reader, "ModulName", String.Empty)
						data.LOLKst1 = SafeGetString(reader, "LOLKst1", String.Empty)
						data.LOLKst2 = SafeGetString(reader, "LOLKst2", String.Empty)
						data.Kst = SafeGetString(reader, "Kst", String.Empty)

						data.m_Anz = SafeGetDecimal(reader, "m_Anz", 0)
						data.m_Bas = SafeGetDecimal(reader, "m_Bas", 0)
						data.m_Ans = SafeGetDecimal(reader, "m_Ans", 0)
						data.m_Btr = SafeGetDecimal(reader, "m_Btr", 0)
						data.Suva = SafeGetString(reader, "Suva", String.Empty)
						data.KW = SafeGetInteger(reader, "KW", 0)
						data.KW2 = SafeGetShort(reader, "KW2", 0)

						data.DestRPNr = SafeGetInteger(reader, "DestRPNr", 0)
						data.DestESNr = SafeGetInteger(reader, "DestESNr", 0)
						data.DestKDNr = SafeGetInteger(reader, "DestkdNr", 0)

						data.GAVNr = SafeGetInteger(reader, "GAVNr", 0)
						data.GAV_Kanton = SafeGetString(reader, "GAV_Kanton", String.Empty)
						data.GAV_Beruf = SafeGetString(reader, "GAV_Beruf", String.Empty)
						data.GAV_Gruppe1 = SafeGetString(reader, "GAV_Gruppe1", String.Empty)
						data.GAV_Gruppe2 = SafeGetString(reader, "GAV_Gruppe2", String.Empty)
						data.GAV_Gruppe3 = SafeGetString(reader, "GAV_Gruppe3", String.Empty)
						data.GAV_Text = SafeGetString(reader, "GAV_Text", String.Empty)
						data.ESEinstufung = SafeGetString(reader, "ESEinstufung", String.Empty)
						data.ESBranche = SafeGetString(reader, "ESBranche", String.Empty)
						data.DateOfLO = SafeGetDateTime(reader, "DateOfLO", Nothing)

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
		''' insert frmd guthaben into lol.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Function AddFremdGuthabenIntoLOL(ByVal data As LOLDataFoRepeatLA4LOBack) As Boolean Implements IPayrollDatabaseAccess.AddFremdGuthabenIntoLOL

			Dim success = True

			Dim sql As String = String.Empty

			sql &= "[Add Fremd Guthaben To LOL]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@loNr", ReplaceMissing(data.LONr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@maNr", ReplaceMissing(data.MANr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", ReplaceMissing(data.MDNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@lanr", ReplaceMissing(data.LANr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@m_anzahl", ReplaceMissing(data.m_Anz, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@m_basis", ReplaceMissing(data.m_Bas, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@M_Ansatz", ReplaceMissing(data.m_Ans, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@m_betrag", ReplaceMissing(data.m_Btr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@suva", ReplaceMissing(data.Suva, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@kst1", ReplaceMissing(data.LOLKst1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@kst2", ReplaceMissing(data.LOLKst2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@kst3", ReplaceMissing(data.Kst, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@lp", ReplaceMissing(data.LP, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@jahr", ReplaceMissing(data.Jahr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@rptext", ReplaceMissing(data.RPText, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@modulname", ReplaceMissing(data.ModulName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@kwnr", ReplaceMissing(data.KW, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@kw2nr", ReplaceMissing(data.KW2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@temprpnr", ReplaceMissing(data.DestRPNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@tempesnr", ReplaceMissing(data.DestESNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@tempkdnr", ReplaceMissing(data.DestKDNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@rpgav_kanton", ReplaceMissing(data.GAV_Kanton, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@rpgav_beruf", ReplaceMissing(data.GAV_Beruf, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@rpgav_Gruppe1", ReplaceMissing(data.GAV_Gruppe1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@rpgav_gruppe2", ReplaceMissing(data.GAV_Gruppe2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@rpgav_gruppe3", ReplaceMissing(data.GAV_Gruppe3, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@rpgav_text", ReplaceMissing(data.GAV_Text, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@eseinstufung", ReplaceMissing(data.ESEinstufung, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@esbranche", ReplaceMissing(data.ESBranche, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@DateofLO", ReplaceMissing(data.DateOfLO, Now)))

			' New ID of LOL
			Dim newIdParameter = New SqlClient.SqlParameter("@NewLOLID", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If success AndAlso Not newIdParameter.Value Is Nothing Then
				data.ID = CType(newIdParameter.Value, Integer)
			Else
				success = False
			End If


			Return success
		End Function

		''' <summary>
		''' update frmd guthaben into lol.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Function UpdateFremdGuthabenIntoLOL(ByVal data As LOLDataFoRepeatLA4LOBack) As Boolean Implements IPayrollDatabaseAccess.UpdateFremdGuthabenIntoLOL

			Dim success = True

			Dim sql As String

			sql = "Update LOL Set "
			sql &= "LANr = @LANr"
			sql &= ", m_Anz = @m_Anz "
			sql &= ", m_Ans = @m_Ans "
			sql &= ", m_Bas = @m_Bas "
			sql &= ", m_Btr = @m_Btr "
			sql &= ", RPText = @rpText "

			sql &= "Where ID = @ID "

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.ID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LANr", ReplaceMissing(data.LANr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@m_Anz", ReplaceMissing(data.m_Anz, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@m_Bas", ReplaceMissing(data.m_Bas, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@m_Ans", ReplaceMissing(data.m_Ans, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@m_Btr", ReplaceMissing(data.m_Btr, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("@rpText", ReplaceMissing(data.RPText, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)


			Return success
		End Function

		''' <summary>
		''' Deletes fremd guthaben from LOL.
		''' </summary>
		''' <returns>Boolen flag indicating success.</returns>
		Function DeleteAssignedFremdGuthaben(ByVal id As Integer, ByVal modul As String, ByVal username As String, ByVal usnr As Integer) As DeleteLOLForCorrectionAssignmentResult Implements IPayrollDatabaseAccess.DeleteAssignedFremdGuthaben

			Dim success = True

			Dim sql As String
			sql = "[Delete LOL Data For Correction]"


			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("ID", id))
			listOfParams.Add(New SqlClient.SqlParameter("modul", modul))
			listOfParams.Add(New SqlClient.SqlParameter("username", username))
			listOfParams.Add(New SqlClient.SqlParameter("usnr", usnr))

			Dim resultParameter = New SqlClient.SqlParameter("@Result", SqlDbType.Int)
			resultParameter.Direction = ParameterDirection.Output
			listOfParams.Add(resultParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Dim resultEnum As DeleteLOLForCorrectionAssignmentResult

			If Not resultParameter.Value Is Nothing Then
				Try
					resultEnum = CType(resultParameter.Value, DeleteLOLForCorrectionAssignmentResult)
				Catch
					resultEnum = DeleteLOLForCorrectionAssignmentResult.ResultDeleteError
				End Try
			Else
				resultEnum = DeleteLOLForCorrectionAssignmentResult.ResultDeleteError
			End If


			Return resultEnum

		End Function



	End Class

End Namespace
