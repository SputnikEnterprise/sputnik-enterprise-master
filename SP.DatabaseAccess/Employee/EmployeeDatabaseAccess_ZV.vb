
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Employee.DataObjects
Imports SP.DatabaseAccess.ES.DataObjects.ESMng
Imports SP.DatabaseAccess.Report.DataObjects
Imports SP.DatabaseAccess.Employee.DataObjects.DocumentMng
Imports SP.DatabaseAccess.Listing.DataObjects

Namespace Employee


	Partial Public Class EmployeeDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IEmployeeDatabaseAccess




		''' <summary>
		''' Loads ES data for zv Form.
		''' </summary>
		''' <param name="mdNr">The MDNr.</param>
		''' <param name="maNr">The MANr.</param>
		''' <returns>List of ES data or nothing in error case.</returns>
		Public Function LoadESData2ForZVForm(ByVal mdNr As Integer, ByVal maNr As Integer, ByVal startOfMonth As DateTime, ByVal endOfMonth As DateTime) As IEnumerable(Of ZVESData) Implements IEmployeeDatabaseAccess.LoadESData2ForZVForm

			Dim result As List(Of ZVESData) = Nothing

			Dim sql As String

			sql = "Select ES.ESNr"
			sql &= ",ES.[ESNR]"
			sql &= ",ES.[MANR]"
			sql &= ",ES.[KDNR]"
			sql &= ",ES.[KDZHDNr]"
			sql &= ",ES.[MDNr]"
			sql &= ",ES.ES_Ab"
			sql &= ",ES.ES_Ende"
			sql &= ",ES.[ES_Als]"
			sql &= ",ES.[Bemerk_MA]"
			sql &= ",ES.[Bemerk_KD]"
			sql &= ",ES.[Bemerk_RE]"
			sql &= ",ES.[Bemerk_Lo]"
			sql &= ",ES.[Bemerk_P]"
			sql &= ",ES.[dismissalon]"
			sql &= ",ES.[dismissalfor]"
			sql &= ",ES.[dismissalkind]"
			sql &= ",ES.[dismissalreason]"
			sql &= ",ES.[dismissalwho]"
			sql &= ",ES.[Bemerk_1]"
			sql &= ",ES.[Bemerk_2]"
			sql &= ",ES.[Bemerk_3]"

			sql &= ",L.[GrundLohn]"
			sql &= ",L.[StundenLohn]"
			sql &= ",L.[FerienProz]"
			sql &= ",L.[FeierProz]"
			sql &= ",L.[Lohn13Proz]"

			sql &= ",L.[Feier]"
			sql &= ",L.[Ferien]"
			sql &= ",L.[Lohn13]"

			sql &= ",L.[GAVNr]"
			sql &= ",L.[GAVKanton]"
			sql &= ",L.[GAVGruppe0]"
			sql &= ",KD.[Firma1]"

			sql &= " From ES "
			sql &= " Left Join ESLohn L On ES.ESNr = L.ESNr "
			sql &= " Left Join Kunden KD On ES.KDNr = KD.KDNr "

			sql &= " Where ES.MANr = @MANr and "
			sql &= " (ES.ES_Ende >=  @startOfMonth "
			sql &= " Or ES.ES_Ende is Null) "
			sql &= " And ES.ES_Ab <= @endOfMonth "
			sql &= " And L.AktivLODaten = 1 "
			sql &= " And ES.MDNr = @mdNr "
			sql &= " Order By ES.ES_Ab ASC, ES.ES_Ende ASC"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("@maNr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@startOfMonth", startOfMonth))
			listOfParams.Add(New SqlClient.SqlParameter("@endOfMonth", endOfMonth))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of ZVESData)

					While reader.Read

						Dim data = New ZVESData

						data.ESNR = SafeGetInteger(reader, "ESNR", Nothing)
						data.EmployeeNumber = SafeGetInteger(reader, "MANr", Nothing)
						data.CustomerNumber = SafeGetInteger(reader, "KDNR", Nothing)
						data.ES_Ab = SafeGetDateTime(reader, "ES_Ab", Nothing)
						data.ES_Ende = SafeGetDateTime(reader, "ES_Ende", Nothing) 'New DateTime(3999, 12, 31)) 'Nothing)
						data.CustomerName = SafeGetString(reader, "Firma1")

						data.Bemerk_MA = SafeGetString(reader, "Bemerk_MA")
						data.Bemerk_KD = SafeGetString(reader, "Bemerk_KD")
						data.Bemerk_RE = SafeGetString(reader, "Bemerk_RE")
						data.Bemerk_Lo = SafeGetString(reader, "Bemerk_Lo")
						data.Bemerk_P = SafeGetString(reader, "Bemerk_P")
						data.ES_Als = SafeGetString(reader, "ES_Als")

						data.dismissalon = SafeGetDateTime(reader, "dismissalon", Nothing)
						data.dismissalfor = SafeGetDateTime(reader, "dismissalfor", Nothing)
						data.dismissalkind = SafeGetString(reader, "dismissalkind")
						data.dismissalreason = SafeGetString(reader, "dismissalreason")
						data.dismissalwho = SafeGetString(reader, "dismissalwho")
						data.Bemerk_1 = SafeGetString(reader, "Bemerk_1")
						data.Bemerk_2 = SafeGetString(reader, "Bemerk_2")
						data.Bemerk_3 = SafeGetString(reader, "Bemerk_3")
						data.KDZHDNr = SafeGetInteger(reader, "KDZHDNr", Nothing)
						data.MDNr = SafeGetInteger(reader, "MDNr", Nothing)
						data.GrundLohn = SafeGetDecimal(reader, "GrundLohn", Nothing)
						data.StundenLohn = SafeGetDecimal(reader, "StundenLohn", Nothing)
						data.FerienProz = SafeGetDecimal(reader, "FerienProz", Nothing)
						data.FeierProz = SafeGetDecimal(reader, "FeierProz", Nothing)
						data.Lohn13Proz = SafeGetDecimal(reader, "Lohn13Proz", Nothing)

						data.Ferien = SafeGetDecimal(reader, "Ferien", Nothing)
						data.Feier = SafeGetDecimal(reader, "Feier", Nothing)
						data.Lohn13 = SafeGetDecimal(reader, "Lohn13", Nothing)

						data.GAVNr = SafeGetInteger(reader, "GAVNr", Nothing)
						data.GAVKanton = SafeGetString(reader, "GAVKanton")
						data.GAVGruppe0 = SafeGetString(reader, "GAVGruppe0")


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
		''' Loads employee zv data.
		''' </summary>
		''' <returns>List of employee div-address data.</returns>
		Public Function LoadEmployeeZvAddressData(ByVal employeeNumber As Integer) As EmployeeSAddressData Implements IEmployeeDatabaseAccess.LoadEmployeeZvAddressData

			Dim result As EmployeeSAddressData = Nothing

			Dim sql As String

			sql = "[Get MAAdressData For Selected MA In ZV]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MANummer", employeeNumber))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			result = New EmployeeSAddressData
			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result.Gender = SafeGetString(reader, "Geschlecht")
					result.Lastname = SafeGetString(reader, "MANachname")
					result.Firstname = SafeGetString(reader, "MAVorname")
					result.StaysAt = SafeGetString(reader, "MACo")
					result.PostOfficeBox = SafeGetString(reader, "MAPostfach")
					result.Street = SafeGetString(reader, "MAStrasse")
					result.Postcode = SafeGetString(reader, "MAPLZ")
					result.Location = SafeGetString(reader, "MAOrt")
					result.Country = SafeGetString(reader, "MALand")
					result.Add_Bemerkung = SafeGetString(reader, "Add_Bemerkung")
					result.Add_Res1 = SafeGetString(reader, "Add_Res1")
					result.Add_Res2 = SafeGetString(reader, "Add_Res2")
					result.Add_Res3 = SafeGetString(reader, "Add_Res3")


				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)

			End Try

			Return result
		End Function

		Function GetEmployeeMonthHoursAndAbsenceData(ByVal mdNr As Integer, ByVal employeeNumber As Integer, ByVal reportNumber As Integer, ByVal year As Integer, ByVal month As Integer) As ZVHourAbsenceData Implements IEmployeeDatabaseAccess.GetEmployeeMonthHoursAndAbsenceData

			Dim result As ZVHourAbsenceData = Nothing

			Dim hourData = GetRPLDayHoursMontlyTotal(mdNr, employeeNumber, reportNumber, year, month)
			Dim absenceData = LoadRPAbsenceDaysMontlyData(mdNr, employeeNumber, reportNumber, year, month)

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
		''' Loads report hours data of month.
		''' </summary>
		Private Function GetRPLDayHoursMontlyTotal(ByVal mdNr As Integer, ByVal employeeNumber As Integer, ByVal reportNumber As Integer, ByVal year As Integer, ByVal month As Integer) As RPLDayHoursTotal

			Dim totalData As RPLDayHoursTotal = Nothing

			Dim sql As String

			sql = "[Get Employee Worked Days of month]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("manr", ReplaceMissing(employeeNumber, 0)))
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

					totalData.Type = Report.RPLType.Employee

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
		Private Function LoadRPAbsenceDaysMontlyData(ByVal mdNr As Integer, ByVal employeeNumber As Integer, ByVal reportNumber As Integer, ByVal year As Integer, ByVal month As Integer) As RPAbsenceDaysData

			Dim result As RPAbsenceDaysData = Nothing

			Dim sql As String

			sql = "[Get Employee Netto Absence Days of month]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("manr", ReplaceMissing(employeeNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("rpnr", ReplaceMissing(reportNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("year", ReplaceMissing(year, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("month", ReplaceMissing(month, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				Dim resultList = New List(Of RPAbsenceDaysData)
				result = New RPAbsenceDaysData
				If (Not reader Is Nothing) Then

					While reader.Read()
						Dim viewData = New RPAbsenceDaysData

						viewData.Fehltag1 = SafeGetString(reader, "Fehltag1")
						viewData.Fehltag2 = SafeGetString(reader, "Fehltag2")
						viewData.Fehltag3 = SafeGetString(reader, "Fehltag3")
						viewData.Fehltag4 = SafeGetString(reader, "Fehltag4")
						viewData.Fehltag5 = SafeGetString(reader, "Fehltag5")
						viewData.Fehltag6 = SafeGetString(reader, "Fehltag6")
						viewData.Fehltag7 = SafeGetString(reader, "Fehltag7")
						viewData.Fehltag8 = SafeGetString(reader, "Fehltag8")
						viewData.Fehltag9 = SafeGetString(reader, "Fehltag9")
						viewData.Fehltag10 = SafeGetString(reader, "Fehltag10")
						viewData.Fehltag11 = SafeGetString(reader, "Fehltag11")
						viewData.Fehltag12 = SafeGetString(reader, "Fehltag12")
						viewData.Fehltag13 = SafeGetString(reader, "Fehltag13")
						viewData.Fehltag14 = SafeGetString(reader, "Fehltag14")
						viewData.Fehltag15 = SafeGetString(reader, "Fehltag15")
						viewData.Fehltag16 = SafeGetString(reader, "Fehltag16")
						viewData.Fehltag17 = SafeGetString(reader, "Fehltag17")
						viewData.Fehltag18 = SafeGetString(reader, "Fehltag18")
						viewData.Fehltag19 = SafeGetString(reader, "Fehltag19")
						viewData.Fehltag20 = SafeGetString(reader, "Fehltag20")
						viewData.Fehltag21 = SafeGetString(reader, "Fehltag21")
						viewData.Fehltag22 = SafeGetString(reader, "Fehltag22")
						viewData.Fehltag23 = SafeGetString(reader, "Fehltag23")
						viewData.Fehltag24 = SafeGetString(reader, "Fehltag24")
						viewData.Fehltag25 = SafeGetString(reader, "Fehltag25")
						viewData.Fehltag26 = SafeGetString(reader, "Fehltag26")
						viewData.Fehltag27 = SafeGetString(reader, "Fehltag27")
						viewData.Fehltag28 = SafeGetString(reader, "Fehltag28")
						viewData.Fehltag29 = SafeGetString(reader, "Fehltag29")
						viewData.Fehltag30 = SafeGetString(reader, "Fehltag30")
						viewData.Fehltag31 = SafeGetString(reader, "Fehltag31")

						resultList.Add(viewData)

					End While

					For Each itm In resultList
						Dim value = itm.Fehltag1
						Dim fehlCode = resultList.Where(Function(s) Not String.IsNullOrWhiteSpace(s.Fehltag1)).FirstOrDefault
						If Not fehlCode Is Nothing Then
							result.Fehltag1 = fehlCode.Fehltag1
						Else
							result.Fehltag1 = value
						End If

						value = itm.Fehltag2
						fehlCode = resultList.Where(Function(s) Not String.IsNullOrWhiteSpace(s.Fehltag2)).FirstOrDefault
						If Not fehlCode Is Nothing Then
							result.Fehltag2 = fehlCode.Fehltag2
						Else
							result.Fehltag2 = value
						End If

						value = itm.Fehltag3
						fehlCode = resultList.Where(Function(s) Not String.IsNullOrWhiteSpace(s.Fehltag3)).FirstOrDefault
						If Not fehlCode Is Nothing Then
							result.Fehltag3 = fehlCode.Fehltag3
						Else
							result.Fehltag3 = value
						End If

						value = itm.Fehltag4
						fehlCode = resultList.Where(Function(s) Not String.IsNullOrWhiteSpace(s.Fehltag4)).FirstOrDefault
						If Not fehlCode Is Nothing Then
							result.Fehltag4 = fehlCode.Fehltag4
						Else
							result.Fehltag4 = value
						End If

						value = itm.Fehltag5
						fehlCode = resultList.Where(Function(s) Not String.IsNullOrWhiteSpace(s.Fehltag5)).FirstOrDefault
						If Not fehlCode Is Nothing Then
							result.Fehltag5 = fehlCode.Fehltag5
						Else
							result.Fehltag5 = value
						End If
						value = itm.Fehltag6
						fehlCode = resultList.Where(Function(s) Not String.IsNullOrWhiteSpace(s.Fehltag6)).FirstOrDefault
						If Not fehlCode Is Nothing Then
							result.Fehltag6 = fehlCode.Fehltag6
						Else
							result.Fehltag6 = value
						End If
						value = itm.Fehltag7
						fehlCode = resultList.Where(Function(s) Not String.IsNullOrWhiteSpace(s.Fehltag7)).FirstOrDefault
						If Not fehlCode Is Nothing Then
							result.Fehltag7 = fehlCode.Fehltag7
						Else
							result.Fehltag7 = value
						End If
						value = itm.Fehltag8
						fehlCode = resultList.Where(Function(s) Not String.IsNullOrWhiteSpace(s.Fehltag8)).FirstOrDefault
						If Not fehlCode Is Nothing Then
							result.Fehltag8 = fehlCode.Fehltag8
						Else
							result.Fehltag8 = value
						End If
						value = itm.Fehltag9
						fehlCode = resultList.Where(Function(s) Not String.IsNullOrWhiteSpace(s.Fehltag9)).FirstOrDefault
						If Not fehlCode Is Nothing Then
							result.Fehltag9 = fehlCode.Fehltag9
						Else
							result.Fehltag9 = value
						End If
						value = itm.Fehltag10
						fehlCode = resultList.Where(Function(s) Not String.IsNullOrWhiteSpace(s.Fehltag10)).FirstOrDefault
						If Not fehlCode Is Nothing Then
							result.Fehltag10 = fehlCode.Fehltag10
						Else
							result.Fehltag10 = value
						End If
						value = itm.Fehltag11
						fehlCode = resultList.Where(Function(s) Not String.IsNullOrWhiteSpace(s.Fehltag11)).FirstOrDefault
						If Not fehlCode Is Nothing Then
							result.Fehltag11 = fehlCode.Fehltag11
						Else
							result.Fehltag11 = value
						End If
						value = itm.Fehltag12
						fehlCode = resultList.Where(Function(s) Not String.IsNullOrWhiteSpace(s.Fehltag12)).FirstOrDefault
						If Not fehlCode Is Nothing Then
							result.Fehltag12 = fehlCode.Fehltag12
						Else
							result.Fehltag12 = value
						End If
						value = itm.Fehltag13
						fehlCode = resultList.Where(Function(s) Not String.IsNullOrWhiteSpace(s.Fehltag13)).FirstOrDefault
						If Not fehlCode Is Nothing Then
							result.Fehltag13 = fehlCode.Fehltag13
						Else
							result.Fehltag13 = value
						End If
						value = itm.Fehltag14
						fehlCode = resultList.Where(Function(s) Not String.IsNullOrWhiteSpace(s.Fehltag14)).FirstOrDefault
						If Not fehlCode Is Nothing Then
							result.Fehltag14 = fehlCode.Fehltag14
						Else
							result.Fehltag14 = value
						End If
						value = itm.Fehltag15
						fehlCode = resultList.Where(Function(s) Not String.IsNullOrWhiteSpace(s.Fehltag15)).FirstOrDefault
						If Not fehlCode Is Nothing Then
							result.Fehltag15 = fehlCode.Fehltag15
						Else
							result.Fehltag15 = value
						End If
						value = itm.Fehltag16
						fehlCode = resultList.Where(Function(s) Not String.IsNullOrWhiteSpace(s.Fehltag16)).FirstOrDefault
						If Not fehlCode Is Nothing Then
							result.Fehltag16 = fehlCode.Fehltag16
						Else
							result.Fehltag16 = value
						End If
						value = itm.Fehltag17
						fehlCode = resultList.Where(Function(s) Not String.IsNullOrWhiteSpace(s.Fehltag17)).FirstOrDefault
						If Not fehlCode Is Nothing Then
							result.Fehltag17 = fehlCode.Fehltag17
						Else
							result.Fehltag17 = value
						End If
						value = itm.Fehltag18
						fehlCode = resultList.Where(Function(s) Not String.IsNullOrWhiteSpace(s.Fehltag18)).FirstOrDefault
						If Not fehlCode Is Nothing Then
							result.Fehltag18 = fehlCode.Fehltag18
						Else
							result.Fehltag18 = value
						End If
						value = itm.Fehltag19
						fehlCode = resultList.Where(Function(s) Not String.IsNullOrWhiteSpace(s.Fehltag19)).FirstOrDefault
						If Not fehlCode Is Nothing Then
							result.Fehltag19 = fehlCode.Fehltag19
						Else
							result.Fehltag19 = value
						End If
						value = itm.Fehltag20
						fehlCode = resultList.Where(Function(s) Not String.IsNullOrWhiteSpace(s.Fehltag20)).FirstOrDefault
						If Not fehlCode Is Nothing Then
							result.Fehltag20 = fehlCode.Fehltag20
						Else
							result.Fehltag20 = value
						End If


						value = itm.Fehltag21
						fehlCode = resultList.Where(Function(s) Not String.IsNullOrWhiteSpace(s.Fehltag21)).FirstOrDefault
						If Not fehlCode Is Nothing Then
							result.Fehltag21 = fehlCode.Fehltag21
						Else
							result.Fehltag21 = value
						End If

						value = itm.Fehltag22
						fehlCode = resultList.Where(Function(s) Not String.IsNullOrWhiteSpace(s.Fehltag22)).FirstOrDefault
						If Not fehlCode Is Nothing Then
							result.Fehltag22 = fehlCode.Fehltag22
						Else
							result.Fehltag22 = value
						End If

						value = itm.Fehltag23
						fehlCode = resultList.Where(Function(s) Not String.IsNullOrWhiteSpace(s.Fehltag23)).FirstOrDefault
						If Not fehlCode Is Nothing Then
							result.Fehltag23 = fehlCode.Fehltag23
						Else
							result.Fehltag23 = value
						End If

						value = itm.Fehltag24
						fehlCode = resultList.Where(Function(s) Not String.IsNullOrWhiteSpace(s.Fehltag24)).FirstOrDefault
						If Not fehlCode Is Nothing Then
							result.Fehltag24 = fehlCode.Fehltag24
						Else
							result.Fehltag24 = value
						End If

						value = itm.Fehltag25
						fehlCode = resultList.Where(Function(s) Not String.IsNullOrWhiteSpace(s.Fehltag25)).FirstOrDefault
						If Not fehlCode Is Nothing Then
							result.Fehltag25 = fehlCode.Fehltag25
						Else
							result.Fehltag25 = value
						End If
						value = itm.Fehltag26
						fehlCode = resultList.Where(Function(s) Not String.IsNullOrWhiteSpace(s.Fehltag26)).FirstOrDefault
						If Not fehlCode Is Nothing Then
							result.Fehltag26 = fehlCode.Fehltag26
						Else
							result.Fehltag26 = value
						End If
						value = itm.Fehltag27
						fehlCode = resultList.Where(Function(s) Not String.IsNullOrWhiteSpace(s.Fehltag27)).FirstOrDefault
						If Not fehlCode Is Nothing Then
							result.Fehltag27 = fehlCode.Fehltag27
						Else
							result.Fehltag27 = value
						End If
						value = itm.Fehltag28
						fehlCode = resultList.Where(Function(s) Not String.IsNullOrWhiteSpace(s.Fehltag28)).FirstOrDefault
						If Not fehlCode Is Nothing Then
							result.Fehltag28 = fehlCode.Fehltag28
						Else
							result.Fehltag28 = value
						End If
						value = itm.Fehltag29
						fehlCode = resultList.Where(Function(s) Not String.IsNullOrWhiteSpace(s.Fehltag29)).FirstOrDefault
						If Not fehlCode Is Nothing Then
							result.Fehltag29 = fehlCode.Fehltag29
						Else
							result.Fehltag29 = value
						End If
						value = itm.Fehltag30
						fehlCode = resultList.Where(Function(s) Not String.IsNullOrWhiteSpace(s.Fehltag30)).FirstOrDefault
						If Not fehlCode Is Nothing Then
							result.Fehltag30 = fehlCode.Fehltag30
						Else
							result.Fehltag30 = value
						End If
						value = itm.Fehltag31
						fehlCode = resultList.Where(Function(s) Not String.IsNullOrWhiteSpace(s.Fehltag31)).FirstOrDefault
						If Not fehlCode Is Nothing Then
							result.Fehltag31 = fehlCode.Fehltag31
						Else
							result.Fehltag31 = value
						End If

					Next

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
		''' Loads report hours data grouped by kstnr.
		''' </summary>
		Function GetEmployeeMonthHoursGroupedByKSTData(ByVal mdNr As Integer, ByVal employeeNumber As Integer, ByVal reportNumber As Integer, ByVal year As Integer, ByVal month As Integer) As IEnumerable(Of WorkingHourGroupedWithKSTNrData) Implements IEmployeeDatabaseAccess.GetEmployeeMonthHoursGroupedByKSTData

			Dim result As List(Of WorkingHourGroupedWithKSTNrData) = Nothing

			Dim sql As String

			sql = "[Get Employee Worked Days of month Grouped By KSTNr]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("manr", ReplaceMissing(employeeNumber, 0)))
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


		''' <summary>
		''' Loads absence days of month data.
		''' </summary>
		Public Function LoadEmployeePayrollData(ByVal mdNr As Integer, ByVal employeeNumber As Integer, ByVal year As Integer, ByVal month As Integer) As IEnumerable(Of ZVPayrollData) Implements IEmployeeDatabaseAccess.LoadEmployeePayrollData

			Dim result As List(Of ZVPayrollData) = Nothing

			Dim sql As String

			sql = "[List Employee Payroll Data For Search ZV]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("manr", ReplaceMissing(employeeNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("year", ReplaceMissing(year, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("month", ReplaceMissing(month, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ZVPayrollData)

					While reader.Read

						Dim data = New ZVPayrollData

						data.LONr = SafeGetInteger(reader, "LONr", 0)
						data.LANr = SafeGetDecimal(reader, "LANr", 0)
						data.TotalAnzahl = SafeGetDecimal(reader, "TotalAnzahl", 0)
						data.TotalBasis = SafeGetDecimal(reader, "TotalBasis", 0)
						data.TotalBetrag = SafeGetDecimal(reader, "TotalBetrag", 0)

						data.RPText = SafeGetString(reader, "RPText")
						data.Bruttopflichtig = SafeGetBoolean(reader, "Bruttopflichtig", False)
						data.AHVpflichtig = SafeGetBoolean(reader, "AHVpflichtig", False)


						result.Add(data)

					End While

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
		''' Loads employee ESVertrag from mall_doc table.
		''' </summary>
		Function LoadEmployeeDocumentForZVData(ByVal employeeNumber As Integer, ByVal recNumber As Integer, ByVal categoryNumber As Integer) As EmployeeDocumentData Implements IEmployeeDatabaseAccess.LoadEmployeeDocumentForZVData

			Dim result As EmployeeDocumentData = Nothing

			Dim sql As String
			Dim bezeichnungValue As String = String.Empty
			Dim beschreibungValue As String = String.Empty

			If categoryNumber = 211 Then
				bezeichnungValue = "Vertrag"
				beschreibungValue = "employment_"
			ElseIf categoryNumber = 212 Then
				bezeichnungValue = "Lohnabrechnung"
				beschreibungValue = "payroll_"
			End If

			sql = "Select Top 1 ID, RecNr, Bezeichnung, Beschreibung, DocPath, ScanExtension, Categorie_Nr From MA_LLDoc "
			sql &= " WHERE "
			sql &= " MANr = @MANr "
			sql &= " AND Categorie_Nr = @Categorie_Nr "
			sql &= String.Format(" AND (Bezeichnung LIKE '{0} {1}:%'", bezeichnungValue, recNumber)
			sql &= String.Format(" OR Beschreibung LIKE '{0}{1}_{2}%')", beschreibungValue, recNumber, categoryNumber)
			sql &= " Order By ID Desc"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("manr", ReplaceMissing(employeeNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Categorie_Nr", ReplaceMissing(categoryNumber, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try

				result = New EmployeeDocumentData

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result.ID = SafeGetInteger(reader, "ID", 0)
					result.DocumentRecordNumber = SafeGetInteger(reader, "RecNr", Nothing)
					result.Name = SafeGetString(reader, "Bezeichnung")
					result.Description = SafeGetString(reader, "Beschreibung")
					result.ScanExtension = SafeGetString(reader, "ScanExtension")
					result.CategorieNumber = SafeGetInteger(reader, "Categorie_Nr", 0)
					result.FileFullPath = SafeGetString(reader, "DocPath")


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
		''' Loads employee ESVertrag from ma_printed_docs table.
		''' </summary>
		Function LoadEmployeePrintedDocumentForZVData(ByVal employeeNumber As Integer, ByVal month As Integer, ByVal year As Integer, ByVal categoryNumber As Integer) As EmployeeDocumentData Implements IEmployeeDatabaseAccess.LoadEmployeePrintedDocumentForZVData

			Dim result As EmployeeDocumentData = Nothing

			Dim sql As String
			Dim bezeichnungValue As String = String.Empty
			Dim beschreibungValue As String = String.Empty

			If categoryNumber = 211 Then
				bezeichnungValue = "Vertrag"
				beschreibungValue = "employment_"
			ElseIf categoryNumber = 212 Then
				bezeichnungValue = "Lohnabrechnung"
				beschreibungValue = "payroll_"
			End If

			sql = "Select Top 1 "
			sql &= "ID ,"
			sql &= "DocName ,"
			sql &= "ScanDoc "

			sql &= " From MA_Printed_Docs "
			sql &= " WHERE "
			sql &= " MANr = @MANr "
			sql &= String.Format(" And (DocName Like '{0} {1} / {2}')", bezeichnungValue, month, year)
			sql &= " Order By ID Desc"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("manr", ReplaceMissing(employeeNumber, 0)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try

				result = New EmployeeDocumentData

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result.ID = SafeGetInteger(reader, "ID", 0)
					result.Name = SafeGetString(reader, "DocName")
					result.Description = SafeGetString(reader, "DocName")
					result.ScanExtension = ".PDF"


				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString()))
				result = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function


#Region "ARGB"


		''' <summary>
		''' Loads employee argb data.
		''' </summary>
		''' <returns>List of employee div-address data.</returns>
		Function LoadEmployeeARGBAddressData(ByVal employeeNumber As Integer) As EmployeeSAddressData Implements IEmployeeDatabaseAccess.LoadEmployeeARGBAddressData

			Dim result As EmployeeSAddressData = Nothing

			Dim sql As String

			sql = "[Get MAAdressData For Selected MA In ARG]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MANummer", employeeNumber))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			result = New EmployeeSAddressData
			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result.Gender = SafeGetString(reader, "Geschlecht")
					result.Lastname = SafeGetString(reader, "MANachname")
					result.Firstname = SafeGetString(reader, "MAVorname")
					result.StaysAt = SafeGetString(reader, "MACo")
					result.PostOfficeBox = SafeGetString(reader, "MAPostfach")
					result.Street = SafeGetString(reader, "MAStrasse")
					result.Postcode = SafeGetString(reader, "MAPLZ")
					result.Location = SafeGetString(reader, "MAOrt")
					result.Country = SafeGetString(reader, "MALand")
					result.Add_Bemerkung = SafeGetString(reader, "Add_Bemerkung")
					result.Add_Res1 = SafeGetString(reader, "Add_Res1")
					result.Add_Res2 = SafeGetString(reader, "Add_Res2")
					result.Add_Res3 = SafeGetString(reader, "Add_Res3")


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
		''' Loads payroll data for ARGB.
		''' </summary>
		Function LoadEmployeePayrollForARGBData(ByVal mdNr As Integer, ByVal employeeNumber As Integer, ByVal dateFrom As Date, ByVal dateTo As Date) As IEnumerable(Of ARGBPayrollData) Implements IEmployeeDatabaseAccess.LoadEmployeePayrollForARGBData

			Dim result As List(Of ARGBPayrollData) = Nothing

			Dim sql As String

			sql = "[List Employee Payroll Data For Search ARGB]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("manr", ReplaceMissing(employeeNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("dateFrom", ReplaceMissing(dateFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("dateTo", ReplaceMissing(dateTo, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ARGBPayrollData)

					While reader.Read

						Dim data = New ARGBPayrollData

						data.LANr = SafeGetDecimal(reader, "LANr", 0)
						data.RPText = SafeGetString(reader, "Bezeichnung")
						data.Year = SafeGetInteger(reader, "Jahr", 0)
						data.Month1 = SafeGetDecimal(reader, "Monat1", 0)
						data.Month2 = SafeGetDecimal(reader, "Monat2", 0)
						data.Month3 = SafeGetDecimal(reader, "Monat3", 0)
						data.Month4 = SafeGetDecimal(reader, "Monat4", 0)
						data.Month5 = SafeGetDecimal(reader, "Monat5", 0)
						data.Month6 = SafeGetDecimal(reader, "Monat6", 0)
						data.Month7 = SafeGetDecimal(reader, "Monat7", 0)
						data.Month8 = SafeGetDecimal(reader, "Monat8", 0)
						data.Month9 = SafeGetDecimal(reader, "Monat9", 0)
						data.Month10 = SafeGetDecimal(reader, "Monat10", 0)
						data.Month11 = SafeGetDecimal(reader, "Monat11", 0)
						data.Month12 = SafeGetDecimal(reader, "Monat12", 0)

						data.Bruttopflichtig = SafeGetBoolean(reader, "Bruttopflichtig", False)
						data.AHVpflichtig = SafeGetBoolean(reader, "AHVpflichtig", False)
						data.ARGB_Verdienst_Unterkunft = SafeGetBoolean(reader, "ARGB_Verdienst_Unterkunft", False)
						data.ARGB_Verdienst_Mahlzeit = SafeGetBoolean(reader, "ARGB_Verdienst_Mahlzeit", False)


						result.Add(data)

					End While

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
		''' Loads payroll data for lohnkonti.
		''' </summary>
		Function LoadEmployeePayrollForLohnkontiData(ByVal mdNr As Integer, ByVal employeeNumber As Integer, ByVal dateFrom As Date, ByVal dateTo As Date) As IEnumerable(Of ARGBPayrollData) Implements IEmployeeDatabaseAccess.LoadEmployeePayrollForLohnkontiData

			Dim result As List(Of ARGBPayrollData) = Nothing

			Dim sql As String

			sql = "[List Employee Payroll Data For Search Lohnkonti]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("manr", ReplaceMissing(employeeNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("dateFrom", ReplaceMissing(dateFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("dateTo", ReplaceMissing(dateTo, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ARGBPayrollData)

					While reader.Read

						Dim data = New ARGBPayrollData

						data.LANr = SafeGetDecimal(reader, "LANr", 0)
						data.RPText = SafeGetString(reader, "Bezeichnung")
						data.Year = SafeGetInteger(reader, "Jahr", 0)
						data.Month1 = SafeGetDecimal(reader, "Monat1", 0)
						data.Month2 = SafeGetDecimal(reader, "Monat2", 0)
						data.Month3 = SafeGetDecimal(reader, "Monat3", 0)
						data.Month4 = SafeGetDecimal(reader, "Monat4", 0)
						data.Month5 = SafeGetDecimal(reader, "Monat5", 0)
						data.Month6 = SafeGetDecimal(reader, "Monat6", 0)
						data.Month7 = SafeGetDecimal(reader, "Monat7", 0)
						data.Month8 = SafeGetDecimal(reader, "Monat8", 0)
						data.Month9 = SafeGetDecimal(reader, "Monat9", 0)
						data.Month10 = SafeGetDecimal(reader, "Monat10", 0)
						data.Month11 = SafeGetDecimal(reader, "Monat11", 0)
						data.Month12 = SafeGetDecimal(reader, "Monat12", 0)


						result.Add(data)

					End While

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
		''' Loads ahv data for last years.
		''' </summary>
		Function LoadEmployeeAHVPayrollForARGBLastMonthData(ByVal mdNr As Integer, ByVal employeeNumber As Integer, ByVal dateFrom As Date) As IEnumerable(Of ARGBAHVPayrollData) Implements IEmployeeDatabaseAccess.LoadEmployeeAHVPayrollForARGBLastMonthData

			Dim result As List(Of ARGBAHVPayrollData) = Nothing

			Dim sql As String

			sql = "[List Employee Payroll AHV Data Last 6_12_15_24 Month For Search ARGB]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("manr", ReplaceMissing(employeeNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("dateFrom", ReplaceMissing(dateFrom, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ARGBAHVPayrollData)

					While reader.Read

						Dim data = New ARGBAHVPayrollData

						data.MonthBefore = SafeGetInteger(reader, "MonthBefore", 0)
						data.AHVAmount = SafeGetDecimal(reader, "AHVAmount", 0)


						result.Add(data)

					End While

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
		''' Loads payroll numbers for print with argb.
		''' </summary>
		Function LoadEmployeePayrollForPrintWithARGBData(ByVal mdNr As Integer, ByVal employeeNumber As Integer, ByVal dateFrom As Date, ByVal dateTo As Date) As IEnumerable(Of PayrollPrintData) Implements IEmployeeDatabaseAccess.LoadEmployeePayrollForPrintWithARGBData

			Dim result As List(Of PayrollPrintData) = Nothing

			Dim sql As String

			sql = "[List Employee Payroll Data For Print Payrolls With ARGB]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("manr", ReplaceMissing(employeeNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("dateFrom", ReplaceMissing(dateFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("dateTo", ReplaceMissing(dateTo, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of PayrollPrintData)

					While reader.Read
						Dim data = New PayrollPrintData

						data.LONr = SafeGetInteger(reader, "LONr", 0)
						data.jahr = SafeGetInteger(reader, "Jahr", 0)
						data.monat = SafeGetInteger(reader, "LP", 0)


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString()))
				result = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function


#End Region


	End Class


End Namespace
