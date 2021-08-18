
Imports System.Data.SqlClient
Imports SP.Infrastructure.Logging
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Report.DataObjects

Namespace Customer


	Partial Class CustomerDatabaseAccess
		Inherits DatabaseAccessBase
		Implements ICustomerDatabaseAccess



		Function GetCustomerMonthHoursAndAbsenceData(ByVal mdNr As Integer, ByVal customerNumber As Integer, ByVal reportNumber As Integer, ByVal year As Integer, ByVal month As Integer) As ZVHourAbsenceData Implements ICustomerDatabaseAccess.GetCustomerMonthHoursAndAbsenceData

			Dim result As ZVHourAbsenceData = Nothing

			Dim hourData = GetRPLDayHoursMontlyTotal(mdNr, customerNumber, reportNumber, year, month)
			Dim absenceData = LoadRPAbsenceDaysMontlyData(mdNr, customerNumber, reportNumber, year, month)

			result = New ZVHourAbsenceData
			If hourData Is Nothing OrElse absenceData Is Nothing Then Return result

			result.Tag1 = hourData.Tag1
			result.Tag2 = hourData.Tag2
			result.Tag3 = hourData.Tag3
			result.Tag4 = hourData.Tag4
			result.Tag5 = hourData.Tag5
			result.Tag6 = hourData.Tag6
			result.Tag7 = hourData.Tag7
			result.Tag8 = hourData.Tag8
			result.Tag9 = hourData.Tag9
			result.Tag10 = hourData.Tag10
			result.Tag11 = hourData.Tag11
			result.Tag12 = hourData.Tag12
			result.Tag13 = hourData.Tag13
			result.Tag14 = hourData.Tag14
			result.Tag15 = hourData.Tag15
			result.Tag16 = hourData.Tag16
			result.Tag17 = hourData.Tag17
			result.Tag18 = hourData.Tag18
			result.Tag19 = hourData.Tag19
			result.Tag20 = hourData.Tag20
			result.Tag21 = hourData.Tag21
			result.Tag22 = hourData.Tag22
			result.Tag23 = hourData.Tag23
			result.Tag24 = hourData.Tag24
			result.Tag25 = hourData.Tag25
			result.Tag26 = hourData.Tag26
			result.Tag27 = hourData.Tag27
			result.Tag28 = hourData.Tag28
			result.Tag29 = hourData.Tag29
			result.Tag30 = hourData.Tag30
			result.Tag31 = hourData.Tag31

			result.Fehltag1 = absenceData.Fehltag1
			result.Fehltag2 = absenceData.Fehltag2
			result.Fehltag3 = absenceData.Fehltag3
			result.Fehltag4 = absenceData.Fehltag4
			result.Fehltag5 = absenceData.Fehltag5
			result.Fehltag6 = absenceData.Fehltag6
			result.Fehltag7 = absenceData.Fehltag7
			result.Fehltag8 = absenceData.Fehltag8
			result.Fehltag9 = absenceData.Fehltag9
			result.Fehltag10 = absenceData.Fehltag10
			result.Fehltag11 = absenceData.Fehltag11
			result.Fehltag12 = absenceData.Fehltag12
			result.Fehltag13 = absenceData.Fehltag13
			result.Fehltag14 = absenceData.Fehltag14
			result.Fehltag15 = absenceData.Fehltag15
			result.Fehltag16 = absenceData.Fehltag16
			result.Fehltag17 = absenceData.Fehltag17
			result.Fehltag18 = absenceData.Fehltag18
			result.Fehltag19 = absenceData.Fehltag19
			result.Fehltag20 = absenceData.Fehltag20
			result.Fehltag21 = absenceData.Fehltag21
			result.Fehltag22 = absenceData.Fehltag22
			result.Fehltag23 = absenceData.Fehltag23
			result.Fehltag24 = absenceData.Fehltag24
			result.Fehltag25 = absenceData.Fehltag25
			result.Fehltag26 = absenceData.Fehltag26
			result.Fehltag27 = absenceData.Fehltag27
			result.Fehltag28 = absenceData.Fehltag28
			result.Fehltag29 = absenceData.Fehltag29
			result.Fehltag30 = absenceData.Fehltag30
			result.Fehltag31 = absenceData.Fehltag31


			Return result

		End Function


		''' <summary>
		''' Loads customer report hours data of month.
		''' </summary>
		Private Function GetRPLDayHoursMontlyTotal(ByVal mdNr As Integer, ByVal customerNumber As Integer, ByVal reportNumber As Integer, ByVal year As Integer, ByVal month As Integer) As RPLDayHoursTotal

			Dim totalData As RPLDayHoursTotal = Nothing

			Dim sql As String

			sql = "[Get Customer Worked Days of Month]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("kdnr", ReplaceMissing(customerNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("rpnr", ReplaceMissing(reportNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("year", ReplaceMissing(year, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("month", ReplaceMissing(month, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				totalData = New RPLDayHoursTotal
				If (Not reader Is Nothing AndAlso reader.Read()) Then

					totalData.Tag1 = SafeGetDecimal(reader, "SumTag1", 0)
					totalData.Tag2 = SafeGetDecimal(reader, "SumTag2", 0)
					totalData.Tag3 = SafeGetDecimal(reader, "SumTag3", 0)
					totalData.Tag4 = SafeGetDecimal(reader, "SumTag4", 0)
					totalData.Tag5 = SafeGetDecimal(reader, "SumTag5", 0)
					totalData.Tag6 = SafeGetDecimal(reader, "SumTag6", 0)
					totalData.Tag7 = SafeGetDecimal(reader, "SumTag7", 0)
					totalData.Tag8 = SafeGetDecimal(reader, "SumTag8", 0)
					totalData.Tag9 = SafeGetDecimal(reader, "SumTag9", 0)
					totalData.Tag10 = SafeGetDecimal(reader, "SumTag10", 0)
					totalData.Tag11 = SafeGetDecimal(reader, "SumTag11", 0)
					totalData.Tag12 = SafeGetDecimal(reader, "SumTag12", 0)
					totalData.Tag13 = SafeGetDecimal(reader, "SumTag13", 0)
					totalData.Tag14 = SafeGetDecimal(reader, "SumTag14", 0)
					totalData.Tag15 = SafeGetDecimal(reader, "SumTag15", 0)
					totalData.Tag16 = SafeGetDecimal(reader, "SumTag16", 0)
					totalData.Tag17 = SafeGetDecimal(reader, "SumTag17", 0)
					totalData.Tag18 = SafeGetDecimal(reader, "SumTag18", 0)
					totalData.Tag19 = SafeGetDecimal(reader, "SumTag19", 0)
					totalData.Tag20 = SafeGetDecimal(reader, "SumTag20", 0)
					totalData.Tag21 = SafeGetDecimal(reader, "SumTag21", 0)
					totalData.Tag22 = SafeGetDecimal(reader, "SumTag22", 0)
					totalData.Tag23 = SafeGetDecimal(reader, "SumTag23", 0)
					totalData.Tag24 = SafeGetDecimal(reader, "SumTag24", 0)
					totalData.Tag25 = SafeGetDecimal(reader, "SumTag25", 0)
					totalData.Tag26 = SafeGetDecimal(reader, "SumTag26", 0)
					totalData.Tag27 = SafeGetDecimal(reader, "SumTag27", 0)
					totalData.Tag28 = SafeGetDecimal(reader, "SumTag28", 0)
					totalData.Tag29 = SafeGetDecimal(reader, "SumTag29", 0)
					totalData.Tag30 = SafeGetDecimal(reader, "SumTag30", 0)
					totalData.Tag31 = SafeGetDecimal(reader, "SumTag31", 0)

					totalData.Type = Report.RPLType.Customer

				End If


			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				totalData = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return totalData

		End Function

		''' <summary>
		''' Loads absence days of month data.
		''' </summary>
		Private Function LoadRPAbsenceDaysMontlyData(ByVal mdNr As Integer, ByVal customerNumber As Integer, ByVal reportNumber As Integer, ByVal year As Integer, ByVal month As Integer) As RPAbsenceDaysData

			Dim result As RPAbsenceDaysData = Nothing

			Dim sql As String

			sql = "[Get Customer Netto Absence Days of month]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("kdnr", ReplaceMissing(customerNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("rpnr", ReplaceMissing(reportNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("year", ReplaceMissing(year, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("month", ReplaceMissing(month, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If Not reader Is Nothing Then

					If reader.Read Then
						result = New RPAbsenceDaysData

						result.Fehltag1 = SafeGetString(reader, "Fehltag1")
						result.Fehltag2 = SafeGetString(reader, "Fehltag2")
						result.Fehltag3 = SafeGetString(reader, "Fehltag3")
						result.Fehltag4 = SafeGetString(reader, "Fehltag4")
						result.Fehltag5 = SafeGetString(reader, "Fehltag5")
						result.Fehltag6 = SafeGetString(reader, "Fehltag6")
						result.Fehltag7 = SafeGetString(reader, "Fehltag7")
						result.Fehltag8 = SafeGetString(reader, "Fehltag8")
						result.Fehltag9 = SafeGetString(reader, "Fehltag9")
						result.Fehltag10 = SafeGetString(reader, "Fehltag10")
						result.Fehltag11 = SafeGetString(reader, "Fehltag11")
						result.Fehltag12 = SafeGetString(reader, "Fehltag12")
						result.Fehltag13 = SafeGetString(reader, "Fehltag13")
						result.Fehltag14 = SafeGetString(reader, "Fehltag14")
						result.Fehltag15 = SafeGetString(reader, "Fehltag15")
						result.Fehltag16 = SafeGetString(reader, "Fehltag16")
						result.Fehltag17 = SafeGetString(reader, "Fehltag17")
						result.Fehltag18 = SafeGetString(reader, "Fehltag18")
						result.Fehltag19 = SafeGetString(reader, "Fehltag19")
						result.Fehltag20 = SafeGetString(reader, "Fehltag20")
						result.Fehltag21 = SafeGetString(reader, "Fehltag21")
						result.Fehltag22 = SafeGetString(reader, "Fehltag22")
						result.Fehltag23 = SafeGetString(reader, "Fehltag23")
						result.Fehltag24 = SafeGetString(reader, "Fehltag24")
						result.Fehltag25 = SafeGetString(reader, "Fehltag25")
						result.Fehltag26 = SafeGetString(reader, "Fehltag26")
						result.Fehltag27 = SafeGetString(reader, "Fehltag27")
						result.Fehltag28 = SafeGetString(reader, "Fehltag28")
						result.Fehltag29 = SafeGetString(reader, "Fehltag29")
						result.Fehltag30 = SafeGetString(reader, "Fehltag30")
						result.Fehltag31 = SafeGetString(reader, "Fehltag31")

					End If

				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString()))
				result = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		''' <summary>
		''' Loads customer report hours data grouped by kstnr.
		''' </summary>
		Function GetCustomerMonthHoursGroupedByKSTData(ByVal mdNr As Integer, ByVal customerNumber As Integer, ByVal reportNumber As Integer, ByVal year As Integer, ByVal month As Integer) As IEnumerable(Of WorkingHourGroupedWithKSTNrData) Implements ICustomerDatabaseAccess.GetCustomerMonthHoursGroupedByKSTData

			Dim result As List(Of WorkingHourGroupedWithKSTNrData) = Nothing

			Dim sql As String

			sql = "[Get Customer Worked Days of month Grouped By KSTNr]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("kdnr", ReplaceMissing(customerNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("rpnr", ReplaceMissing(reportNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("year", ReplaceMissing(year, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("month", ReplaceMissing(month, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of WorkingHourGroupedWithKSTNrData)

					While reader.Read()
						Dim viewData = New WorkingHourGroupedWithKSTNrData

						viewData.RPNr = SafeGetInteger(reader, "RPNr", 0)
						viewData.KSTNr = SafeGetInteger(reader, "KSTNr", 0)
						viewData.KSTBez = SafeGetString(reader, "KSTBez")

						viewData.Tag1 = SafeGetDecimal(reader, "SumTag1", 0)
						viewData.Tag2 = SafeGetDecimal(reader, "SumTag2", 0)
						viewData.Tag3 = SafeGetDecimal(reader, "SumTag3", 0)
						viewData.Tag4 = SafeGetDecimal(reader, "SumTag4", 0)
						viewData.Tag5 = SafeGetDecimal(reader, "SumTag5", 0)
						viewData.Tag6 = SafeGetDecimal(reader, "SumTag6", 0)
						viewData.Tag7 = SafeGetDecimal(reader, "SumTag7", 0)
						viewData.Tag8 = SafeGetDecimal(reader, "SumTag8", 0)
						viewData.Tag9 = SafeGetDecimal(reader, "SumTag9", 0)
						viewData.Tag10 = SafeGetDecimal(reader, "SumTag10", 0)
						viewData.Tag11 = SafeGetDecimal(reader, "SumTag11", 0)
						viewData.Tag12 = SafeGetDecimal(reader, "SumTag12", 0)
						viewData.Tag13 = SafeGetDecimal(reader, "SumTag13", 0)
						viewData.Tag14 = SafeGetDecimal(reader, "SumTag14", 0)
						viewData.Tag15 = SafeGetDecimal(reader, "SumTag15", 0)
						viewData.Tag16 = SafeGetDecimal(reader, "SumTag16", 0)
						viewData.Tag17 = SafeGetDecimal(reader, "SumTag17", 0)
						viewData.Tag18 = SafeGetDecimal(reader, "SumTag18", 0)
						viewData.Tag19 = SafeGetDecimal(reader, "SumTag19", 0)
						viewData.Tag20 = SafeGetDecimal(reader, "SumTag20", 0)
						viewData.Tag21 = SafeGetDecimal(reader, "SumTag21", 0)
						viewData.Tag22 = SafeGetDecimal(reader, "SumTag22", 0)
						viewData.Tag23 = SafeGetDecimal(reader, "SumTag23", 0)
						viewData.Tag24 = SafeGetDecimal(reader, "SumTag24", 0)
						viewData.Tag25 = SafeGetDecimal(reader, "SumTag25", 0)
						viewData.Tag26 = SafeGetDecimal(reader, "SumTag26", 0)
						viewData.Tag27 = SafeGetDecimal(reader, "SumTag27", 0)
						viewData.Tag28 = SafeGetDecimal(reader, "SumTag28", 0)
						viewData.Tag29 = SafeGetDecimal(reader, "SumTag29", 0)
						viewData.Tag30 = SafeGetDecimal(reader, "SumTag30", 0)
						viewData.Tag31 = SafeGetDecimal(reader, "SumTag31", 0)


						result.Add(viewData)
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
