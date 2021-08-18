
Imports SP.DatabaseAccess.Listing.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language


Namespace Listing



	Partial Class ListingDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IListingDatabaseAccess



		''' <summary>
		''' Loads Lohnkonti data.
		''' </summary>
		Function LoadAnnualLohnkontiData(ByVal mdNr As Integer, ByVal year As Integer, ByVal monthFrom As Integer?, ByVal monthTo As Integer?, ByVal maNrList As String) As IEnumerable(Of ListingPayrollLohnkontiData) Implements IListingDatabaseAccess.LoadAnnualLohnkontiData
			Dim result As List(Of ListingPayrollLohnkontiData) = Nothing

			Dim sql As String

			sql = "[Load Payroll Data For LohnKonti]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("firstYear", ReplaceMissing(year, Now.Year)))
			listOfParams.Add(New SqlClient.SqlParameter("monthFrom", ReplaceMissing(monthFrom, 1)))
			listOfParams.Add(New SqlClient.SqlParameter("monthTo", ReplaceMissing(monthTo, 12)))
			listOfParams.Add(New SqlClient.SqlParameter("manrListe", ReplaceMissing(maNrList, String.Empty)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ListingPayrollLohnkontiData)

					While reader.Read()
						Dim overviewData As New ListingPayrollLohnkontiData

						overviewData.MANr = SafeGetInteger(reader, "MANr", 0)

						overviewData.AHVNumber = SafeGetString(reader, "AHV_Nr_New")
						overviewData.EmployeeLastname = SafeGetString(reader, "Nachname")
						overviewData.EmployeeFirstname = SafeGetString(reader, "Vorname")
						overviewData.GebDat = SafeGetDateTime(reader, "GebDat", Nothing)
						overviewData.SatrtOfEmployment = SafeGetDateTime(reader, "ESBegin", Nothing)
						overviewData.EndOfEmployment = SafeGetDateTime(reader, "ESEnde", Nothing)

						overviewData.Gender = SafeGetString(reader, "Geschlecht")
						overviewData.Nationality = SafeGetString(reader, "Nationality")

						overviewData.Januar = SafeGetDecimal(reader, "Januar", 0)
						overviewData.Februar = SafeGetDecimal(reader, "Februar", 0)
						overviewData.Maerz = SafeGetDecimal(reader, "März", 0)
						overviewData.April = SafeGetDecimal(reader, "April", 0)
						overviewData.Mai = SafeGetDecimal(reader, "Mai", 0)
						overviewData.Juni = SafeGetDecimal(reader, "Juni", 0)
						overviewData.Juli = SafeGetDecimal(reader, "Juli", 0)
						overviewData.August = SafeGetDecimal(reader, "August", 0)
						overviewData.September = SafeGetDecimal(reader, "September", 0)
						overviewData.Oktober = SafeGetDecimal(reader, "Oktober", 0)
						overviewData.November = SafeGetDecimal(reader, "November", 0)
						overviewData.Dezember = SafeGetDecimal(reader, "Dezember", 0)
						overviewData.LANr = SafeGetDecimal(reader, "lanr", 0)
						overviewData.LALabel = SafeGetString(reader, "Bezeichnung")

						overviewData.Kumulativ = SafeGetDecimal(reader, "Kumulativ", 0)


						result.Add(overviewData)

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


#Region "NLA data"

		Function LoadAnnualNLAData(ByVal sql As String) As IEnumerable(Of ListingPayrollNLAData) Implements IListingDatabaseAccess.LoadAnnualNLAData
			Dim result As List(Of ListingPayrollNLAData) = Nothing

			If String.IsNullOrWhiteSpace(sql) Then Return Nothing

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing, CommandType.Text)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ListingPayrollNLAData)

					While reader.Read()
						Dim overviewData As New ListingPayrollNLAData

						overviewData.MANr = SafeGetInteger(reader, "MANr", Nothing)

						overviewData.employeefirstname = SafeGetString(reader, "Vorname")
						overviewData.employeelastname = SafeGetString(reader, "Nachname")
						overviewData.employeemastrasse = SafeGetString(reader, "MAStrasse")
						overviewData.employeemaplz = SafeGetString(reader, "MAPLZ")
						overviewData.employeemaort = SafeGetString(reader, "MAOrt")

						overviewData.employeemaland = SafeGetString(reader, "MALand")
						overviewData.employeemaco = SafeGetString(reader, "MACo")

						overviewData.employeeahv_nr = SafeGetString(reader, "AHV_Nr")
						overviewData.employeeahv_nr_new = SafeGetString(reader, "AHV_Nr_New")
						overviewData.employeebirthdate = SafeGetDateTime(reader, "GebDat", Nothing)

						overviewData.employeesend2wos = SafeGetBoolean(reader, "Send2WOS", Nothing)
						overviewData.employeegeschlecht = SafeGetString(reader, "Geschlecht")
						overviewData.employeemapostfach = SafeGetString(reader, "MAPostfach")
						overviewData.employeelajahr = SafeGetInteger(reader, "LAJahr", Nothing)

						overviewData.Z_1_0 = SafeGetDecimal(reader, "Z_1_0", Nothing)
						overviewData.Z_2_1 = SafeGetDecimal(reader, "Z_2_1", Nothing)
						overviewData.Z_2_2 = SafeGetDecimal(reader, "Z_2_2", Nothing)
						overviewData.Z_2_3 = SafeGetDecimal(reader, "Z_2_3", Nothing)
						overviewData.Z_3_0 = SafeGetDecimal(reader, "Z_3_0", Nothing)
						overviewData.Z_4_0 = SafeGetDecimal(reader, "Z_4_0", Nothing)
						overviewData.Z_5_0 = SafeGetDecimal(reader, "Z_5_0", Nothing)
						overviewData.Z_6_0 = SafeGetDecimal(reader, "Z_6_0", Nothing)
						overviewData.Z_7_0 = SafeGetDecimal(reader, "Z_7_0", Nothing)
						overviewData.Z_8_0 = SafeGetDecimal(reader, "Z_8_0", Nothing)
						overviewData.Z_9_0 = SafeGetDecimal(reader, "Z_9_0", Nothing)
						overviewData.Z_10_1 = SafeGetDecimal(reader, "Z_10_1", Nothing)
						overviewData.Z_10_2 = SafeGetDecimal(reader, "Z_10_2", Nothing)
						overviewData.Z_11_0 = SafeGetDecimal(reader, "Z_11_0", Nothing)
						overviewData.Z_12_0 = SafeGetDecimal(reader, "Z_12_0", Nothing)
						overviewData.Z_13_1_1 = SafeGetDecimal(reader, "Z_13_1_1", Nothing)
						overviewData.Z_13_1_2 = SafeGetDecimal(reader, "Z_13_1_2", Nothing)
						overviewData.Z_13_2_1 = SafeGetDecimal(reader, "Z_13_2_1", Nothing)
						overviewData.Z_13_2_2 = SafeGetDecimal(reader, "Z_13_2_2", Nothing)
						overviewData.Z_13_2_3 = SafeGetDecimal(reader, "Z_13_2_3", Nothing)
						overviewData.Z_13_3_0 = SafeGetDecimal(reader, "Z_13_3_0", Nothing)

						overviewData.NLA_LoAusweis = SafeGetBoolean(reader, "NLA_LoAusweis", Nothing)
						overviewData.NLA_Befoerderung = SafeGetBoolean(reader, "NLA_Befoerderung", Nothing)
						overviewData.NLA_Kantine = SafeGetBoolean(reader, "NLA_Kantine", Nothing)

						overviewData.NLA_2_3 = SafeGetString(reader, "NLA_2_3")
						overviewData.NLA_3_0 = SafeGetString(reader, "NLA_3_0")
						overviewData.NLA_4_0 = SafeGetString(reader, "NLA_4_0")
						overviewData.NLA_7_0 = SafeGetString(reader, "NLA_7_0")

						overviewData.NLA_Spesen_NotShow = SafeGetBoolean(reader, "NLA_LoAusweis", Nothing)
						overviewData.NLA_13_1_2 = SafeGetString(reader, "NLA_13_1_2")
						overviewData.NLA_13_2_3 = SafeGetString(reader, "NLA_13_2_3")

						overviewData.NLA_Nebenleistung_1 = SafeGetString(reader, "NLA_Nebenleistung_1")
						overviewData.NLA_Nebenleistung_2 = SafeGetString(reader, "NLA_Nebenleistung_2")
						overviewData.NLA_Bemerkung_1 = SafeGetString(reader, "NLA_Bemerkung_1")
						overviewData.NLA_Bemerkung_2 = SafeGetString(reader, "NLA_Bemerkung_2")
						overviewData.Grund = SafeGetString(reader, "Grund")

						overviewData.ES_Ab1 = SafeGetDateTime(reader, "ES_Ab1", Nothing)
						overviewData.ES_Bis1 = SafeGetDateTime(reader, "ES_Bis1", Nothing)


						result.Add(overviewData)

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

#End Region



	End Class

End Namespace
