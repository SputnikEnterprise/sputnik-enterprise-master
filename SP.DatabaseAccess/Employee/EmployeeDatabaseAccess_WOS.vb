
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Employee.DataObjects
Imports SP.DatabaseAccess.ES.DataObjects.ESMng
Imports SP.DatabaseAccess.Report.DataObjects
Imports SP.DatabaseAccess.Employee.DataObjects.DocumentMng

Namespace Employee


	Partial Public Class EmployeeDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IEmployeeDatabaseAccess



#Region "WOS Export"

		Function LoadEmployeeDataForWOSExport(ByVal userNumber As Integer?, ByVal employeeNumber As Integer?, ByVal employmentNumber As Integer?, ByVal eslohnNumber As Integer?, ByVal reportNumber As Integer?, ByVal rplNumber As Integer?, ByVal rpDocNumber As Integer?, ByVal payrollNumber As Integer?) As EmployeeWOSData Implements IEmployeeDatabaseAccess.LoadEmployeeDataForWOSExport

			Dim success = True

			Dim result As EmployeeWOSData = Nothing

			Dim sql As String
			sql = "[Get EmployeeData For Transfer into WOS]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("LogedUSNr", ReplaceMissing(userNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("MANr", ReplaceMissing(employeeNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("ESNr", ReplaceMissing(employmentNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("ESLohnNr", ReplaceMissing(eslohnNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("LONr", ReplaceMissing(payrollNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("RPNr", ReplaceMissing(reportNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("RPLNr", ReplaceMissing(rplNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("RPDocNr", ReplaceMissing(rpDocNumber, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				result = New EmployeeWOSData

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					Dim overviewData As New EmployeeWOSData

					overviewData.EmployeeNumber = employeeNumber
					overviewData.EmploymentNumber = employmentNumber
					overviewData.EmploymentLineNumber = eslohnNumber
					overviewData.ReportNumber = reportNumber
					overviewData.ReportLlineNumber = rplNumber
					overviewData.ReportDocNumber = rpDocNumber
					overviewData.PayrollNumber = payrollNumber
					overviewData.AssignedDocumentGuid = SafeGetString(reader, "Doc_Guid")

					overviewData.MATransferedGuid = SafeGetString(reader, "MA_Guid")
					overviewData.ESDoc_Guid = SafeGetString(reader, "ESDoc_Guid")
					overviewData.RPDoc_Guid = SafeGetString(reader, "RPDoc_Guid")
					overviewData.PayrollDoc_Guid = SafeGetString(reader, "Payroll_Guid")

					overviewData.UserAnrede = SafeGetString(reader, "USAnrede")
					overviewData.UserVorname = SafeGetString(reader, "USVorname")
					overviewData.UserName = SafeGetString(reader, "USNachname")
					overviewData.UserTelefon = SafeGetString(reader, "USTelefon")
					overviewData.UserTelefax = SafeGetString(reader, "USTelefax")
					overviewData.UserMail = SafeGetString(reader, "USeMail")
					overviewData.LogedUserID = SafeGetString(reader, "LogedUser_Guid")

					overviewData.MDTelefon = SafeGetString(reader, "MDTelefon")
					overviewData.MD_DTelefon = SafeGetString(reader, "MDDTelefon")
					overviewData.MDTelefax = SafeGetString(reader, "MDTelefax")
					overviewData.MDMail = SafeGetString(reader, "MDeMail")

					overviewData.MA_Nachname = SafeGetString(reader, "MA_Nachname")
					overviewData.MA_Vorname = SafeGetString(reader, "MA_Vorname")
					overviewData.MA_Postfach = SafeGetString(reader, "Postfach")
					overviewData.MA_Strasse = SafeGetString(reader, "Strasse")
					overviewData.MA_PLZ = SafeGetString(reader, "PLZ")
					overviewData.MA_Ort = SafeGetString(reader, "Ort")
					overviewData.MA_Land = SafeGetString(reader, "Land")
					overviewData.MA_Filiale = SafeGetString(reader, "MA_Filiale")
					overviewData.MA_Berater = SafeGetString(reader, "Berater")
					overviewData.MA_Email = SafeGetString(reader, "MA_EMail")
					overviewData.MA_AGB_Wos = SafeGetString(reader, "AGB_WOS")
					overviewData.MA_Beruf = SafeGetString(reader, "MA_Beruf")
					overviewData.MA_Branche = SafeGetString(reader, "MA_Branche")
					overviewData.MA_Language = SafeGetString(reader, "MA_Language")
					overviewData.MA_GebDat = SafeGetDateTime(reader, "MA_GebDat", Nothing)
					overviewData.MA_Gender = SafeGetString(reader, "MASex")
					overviewData.MA_BriefAnrede = SafeGetString(reader, "BriefAnrede")
					overviewData.MA_Zivil = SafeGetString(reader, "Zivilstand")
					overviewData.MA_Nationality = SafeGetString(reader, "MA_Nationality")

					overviewData.MA_FSchein = SafeGetString(reader, "MAFSchein")
					overviewData.MA_Auto = SafeGetString(reader, "MAAuto")
					overviewData.MA_Kontakt = SafeGetString(reader, "MA_Kontakt")
					overviewData.MA_State1 = SafeGetString(reader, "MA_State1")
					overviewData.MA_State2 = SafeGetString(reader, "MA_State2")
					overviewData.MA_Eigenschaft = SafeGetString(reader, "MA_Eigenschaft")
					overviewData.MA_SSprache = SafeGetString(reader, "MA_SSprache")
					overviewData.MA_MSprache = SafeGetString(reader, "MA_MSprache")
					overviewData.AHV_Nr = SafeGetString(reader, "AHV_Nr_New")
					overviewData.MA_Canton = SafeGetString(reader, "MA_Kanton")

					overviewData.UserInitial = SafeGetString(reader, "User_Initial")
					overviewData.UserSex = SafeGetString(reader, "User_Sex")
					overviewData.UserFiliale = SafeGetString(reader, "User_Filiale")
					overviewData.UserSign = SafeGetByteArray(reader, "User_Sign")
					overviewData.UserPicture = SafeGetByteArray(reader, "User_Picture")

					result = overviewData

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString)

			Finally
				CloseReader(reader)

			End Try

			Return result

		End Function

		Function LoadAvailableEmployeeDataForWOSExport(ByVal employeeNumber As Integer?, ByVal logedUserNr As Integer) As AvailableEmployeeWOSData Implements IEmployeeDatabaseAccess.LoadAvailableEmployeeDataForWOSExport
			Dim result As AvailableEmployeeWOSData = Nothing

			Dim sql As String
			sql = "[Get Available Employee Data For Transfer To WS]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("MANr", ReplaceMissing(employeeNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("LogedUSNr", ReplaceMissing(logedUserNr, 0)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				result = New AvailableEmployeeWOSData

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					Dim overviewData As New AvailableEmployeeWOSData

					overviewData.EmployeeNumber = employeeNumber

					overviewData.Customer_ID = SafeGetString(reader, "AssignedCustomer_ID")
					overviewData.MATransferedGuid = SafeGetString(reader, "MA_Guid")

					overviewData.LogedUserID = SafeGetString(reader, "Advisor_ID")

					overviewData.MA_Nachname = SafeGetString(reader, "MA_Nachname")
					overviewData.MA_Vorname = SafeGetString(reader, "MA_Vorname")
					overviewData.MA_Gender = SafeGetString(reader, "Geschlecht")
					overviewData.MA_Postfach = SafeGetString(reader, "Postfach")
					overviewData.MA_Strasse = SafeGetString(reader, "Strasse")
					overviewData.MA_PLZ = SafeGetString(reader, "PLZ")
					overviewData.MA_Ort = SafeGetString(reader, "Ort")
					overviewData.MA_Land = SafeGetString(reader, "Land")
					overviewData.MA_Filiale = SafeGetString(reader, "MA_Filiale")
					overviewData.MA_Berater = SafeGetString(reader, "Berater")
					overviewData.MA_Email = SafeGetString(reader, "MA_EMail")
					overviewData.MA_AGB_Wos = SafeGetString(reader, "AGB_WOS")
					overviewData.MA_Beruf = SafeGetString(reader, "MA_Beruf")
					overviewData.MA_Branche = SafeGetString(reader, "MA_Branche")
					overviewData.MA_Language = SafeGetString(reader, "MA_Language")
					overviewData.MA_GebDat = SafeGetDateTime(reader, "MA_GebDat", Nothing)
					overviewData.MA_Gender = SafeGetString(reader, "MASex")
					overviewData.Salutation = SafeGetString(reader, "BriefAnrede")
					overviewData.MA_Zivil = SafeGetString(reader, "Zivilstand")
					overviewData.MA_Nationality = SafeGetString(reader, "MA_Nationality")

					overviewData.MA_FSchein = SafeGetString(reader, "MAFSchein")
					overviewData.MA_Auto = SafeGetString(reader, "MAAuto")
					overviewData.MA_Kontakt = SafeGetString(reader, "MA_Kontakt")
					overviewData.MA_State1 = SafeGetString(reader, "MA_State1")
					overviewData.MA_State2 = SafeGetString(reader, "MA_State2")
					overviewData.MA_Eigenschaft = SafeGetString(reader, "MA_Eigenschaft")
					overviewData.MA_SSprache = SafeGetString(reader, "MA_SSprache")
					overviewData.MA_MSprache = SafeGetString(reader, "MA_MSprache")
					overviewData.MA_Canton = SafeGetString(reader, "MA_Kanton")
					overviewData.MA_Branche = SafeGetString(reader, "MA_Branche")
					overviewData.JobProzent = SafeGetString(reader, "JobProzent")
					overviewData.Permit = SafeGetString(reader, "Bewillig")

					overviewData.MA_Res1 = SafeGetString(reader, "MA_Res1")
					overviewData.MA_Res2 = SafeGetString(reader, "MA_Res2")
					overviewData.MA_Res3 = SafeGetString(reader, "MA_Res3")
					overviewData.MA_Res4 = SafeGetString(reader, "MA_Res4")
					overviewData.MA_Res5 = SafeGetString(reader, "MA_Res5")

					overviewData.LL_Name = SafeGetString(reader, "LL_Name")
					overviewData.Reserve0 = SafeGetString(reader, "Reserve0")
					overviewData.Reserve1 = SafeGetString(reader, "Reserve1")
					overviewData.Reserve2 = SafeGetString(reader, "Reserve2")
					overviewData.Reserve3 = SafeGetString(reader, "Reserve3")
					overviewData.Reserve4 = SafeGetString(reader, "Reserve4")
					overviewData.Reserve5 = SafeGetString(reader, "Reserve5")
					overviewData.Reserve6 = SafeGetString(reader, "Reserve6")
					overviewData.Reserve7 = SafeGetString(reader, "Reserve7")
					overviewData.Reserve8 = SafeGetString(reader, "Reserve8")
					overviewData.Reserve9 = SafeGetString(reader, "Reserve9")
					overviewData.Reserve10 = SafeGetString(reader, "Reserve10")
					overviewData.Reserve11 = SafeGetString(reader, "Reserve11")
					overviewData.Reserve12 = SafeGetString(reader, "Reserve12")
					overviewData.Reserve13 = SafeGetString(reader, "Reserve13")
					overviewData.Reserve14 = SafeGetString(reader, "Reserve14")
					overviewData.Reserve15 = SafeGetString(reader, "Reserve15")
					overviewData.ReserveRtf0 = SafeGetString(reader, "_Reserve0")
					overviewData.ReserveRtf1 = SafeGetString(reader, "_Reserve1")
					overviewData.ReserveRtf2 = SafeGetString(reader, "_Reserve2")
					overviewData.ReserveRtf3 = SafeGetString(reader, "_Reserve3")
					overviewData.ReserveRtf4 = SafeGetString(reader, "_Reserve4")
					overviewData.ReserveRtf5 = SafeGetString(reader, "_Reserve5")
					overviewData.ReserveRtf6 = SafeGetString(reader, "_Reserve6")
					overviewData.ReserveRtf7 = SafeGetString(reader, "_Reserve7")
					overviewData.ReserveRtf8 = SafeGetString(reader, "_Reserve8")
					overviewData.ReserveRtf9 = SafeGetString(reader, "_Reserve9")
					overviewData.ReserveRtf10 = SafeGetString(reader, "_Reserve10")
					overviewData.ReserveRtf11 = SafeGetString(reader, "_Reserve11")
					overviewData.ReserveRtf12 = SafeGetString(reader, "_Reserve12")
					overviewData.ReserveRtf13 = SafeGetString(reader, "_Reserve13")
					overviewData.ReserveRtf14 = SafeGetString(reader, "_Reserve14")
					overviewData.ReserveRtf15 = SafeGetString(reader, "_Reserve15")

					overviewData.DesiredWagesOld = SafeGetDecimal(reader, "GehaltAlt", 0)
					overviewData.DesiredWagesNew = SafeGetDecimal(reader, "GehaltNeu", 0)
					overviewData.DesiredWagesInMonth = SafeGetDecimal(reader, "GehaltPerMonth", 0)
					overviewData.DesiredWagesInHour = SafeGetDecimal(reader, "GehaltPerStd", 0)

					result = overviewData

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString)

			Finally
				CloseReader(reader)

			End Try

			Return result

		End Function

		'Function LoadEmployeeDataForEmployeeWOSExport(ByVal userNumber As Integer?, ByVal employeeNumber As Integer?, ByVal employmentNumber As Integer?, ByVal eslohnNumber As Integer?, ByVal reportNumber As Integer?, ByVal rplNumber As Integer?, ByVal rpDocNumber As Integer?, ByVal payrollNumber As Integer?) As DataTable Implements IEmployeeDatabaseAccess.LoadEmployeeDataForEmployeeWOSExport

		'	Dim sql As String
		'	sql = "[Get EmployeeData For Transfer into WOS]"

		'	' Parameters
		'	Dim listOfParams As New List(Of SqlClient.SqlParameter)

		'	listOfParams.Add(New SqlClient.SqlParameter("LogedUSNr", ReplaceMissing(userNumber, 0)))
		'	listOfParams.Add(New SqlClient.SqlParameter("MANr", ReplaceMissing(employeeNumber, 0)))
		'	listOfParams.Add(New SqlClient.SqlParameter("ESNr", ReplaceMissing(employmentNumber, 0)))
		'	listOfParams.Add(New SqlClient.SqlParameter("ESLohnNr", ReplaceMissing(eslohnNumber, 0)))
		'	listOfParams.Add(New SqlClient.SqlParameter("LONr", ReplaceMissing(payrollNumber, 0)))
		'	listOfParams.Add(New SqlClient.SqlParameter("RPNr", ReplaceMissing(reportNumber, 0)))
		'	listOfParams.Add(New SqlClient.SqlParameter("RPLNr", ReplaceMissing(rplNumber, 0)))
		'	listOfParams.Add(New SqlClient.SqlParameter("RPDocNr", ReplaceMissing(rpDocNumber, 0)))

		'	Dim dataTable = FillDataTable(sql, listOfParams, CommandType.StoredProcedure)


		'	Return dataTable

		'End Function


#End Region


#Region "private methodes"

		'Private Function LoadEmployeeApplicationReserveData(ByVal employeeNumber As Integer) As AvailableEmployeeWOSData
		'	Dim result As AvailableEmployeeWOSData = Nothing

		'	Dim sql As String
		'	sql = "[Load Employee Application Reserve Data]"

		'	' Parameters
		'	Dim listOfParams As New List(Of SqlClient.SqlParameter)

		'	listOfParams.Add(New SqlClient.SqlParameter("MANr", ReplaceMissing(employeeNumber, 0)))

		'	Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

		'	Try
		'		result = New AvailableEmployeeWOSData

		'		If (Not reader Is Nothing AndAlso reader.Read()) Then

		'			Dim overviewData As New AvailableEmployeeWOSData

		'			overviewData.EmployeeNumber = employeeNumber

		'			overviewData.Customer_ID = SafeGetString(reader, "AssignedCustomer_ID")
		'			overviewData.MATransferedGuid = SafeGetString(reader, "MA_Guid")

		'			overviewData.LogedUserID = SafeGetString(reader, "Advisor_ID")

		'			overviewData.MA_Nachname = SafeGetString(reader, "MA_Nachname")
		'			overviewData.MA_Vorname = SafeGetString(reader, "MA_Vorname")
		'			overviewData.MA_Gender = SafeGetString(reader, "Geschlecht")
		'			overviewData.MA_Postfach = SafeGetString(reader, "Postfach")
		'			overviewData.MA_Strasse = SafeGetString(reader, "Strasse")
		'			overviewData.MA_PLZ = SafeGetString(reader, "PLZ")
		'			overviewData.MA_Ort = SafeGetString(reader, "Ort")
		'			overviewData.MA_Land = SafeGetString(reader, "Land")
		'			overviewData.MA_Filiale = SafeGetString(reader, "MA_Filiale")
		'			overviewData.MA_Berater = SafeGetString(reader, "Berater")
		'			overviewData.MA_Email = SafeGetString(reader, "MA_EMail")
		'			overviewData.MA_AGB_Wos = SafeGetString(reader, "AGB_WOS")
		'			overviewData.MA_Beruf = SafeGetString(reader, "MA_Beruf")
		'			overviewData.MA_Branche = SafeGetString(reader, "MA_Branche")
		'			overviewData.MA_Language = SafeGetString(reader, "MA_Language")
		'			overviewData.MA_GebDat = SafeGetDateTime(reader, "MA_GebDat", Nothing)
		'			overviewData.MA_Gender = SafeGetString(reader, "MASex")
		'			overviewData.Salutation = SafeGetString(reader, "BriefAnrede")
		'			overviewData.MA_Zivil = SafeGetString(reader, "Zivilstand")
		'			overviewData.MA_Nationality = SafeGetString(reader, "MA_Nationality")
		'			overviewData.NationalityLabel = SafeGetString(reader, "NationalityLabel")

		'			overviewData.MA_FSchein = SafeGetString(reader, "MAFSchein")
		'			overviewData.MA_Auto = SafeGetString(reader, "MAAuto")
		'			overviewData.MA_Kontakt = SafeGetString(reader, "MA_Kontakt")
		'			overviewData.MA_State1 = SafeGetString(reader, "MA_State1")
		'			overviewData.MA_State2 = SafeGetString(reader, "MA_State2")
		'			overviewData.MA_Eigenschaft = SafeGetString(reader, "MA_Eigenschaft")
		'			overviewData.MA_SSprache = SafeGetString(reader, "MA_SSprache")
		'			overviewData.MA_MSprache = SafeGetString(reader, "MA_MSprache")
		'			overviewData.MA_Canton = SafeGetString(reader, "MA_Kanton")
		'			overviewData.MA_Branche = SafeGetString(reader, "MA_Branche")

		'			overviewData.MA_Res1 = SafeGetString(reader, "MA_Res1")
		'			overviewData.MA_Res2 = SafeGetString(reader, "MA_Res2")
		'			overviewData.MA_Res3 = SafeGetString(reader, "MA_Res3")
		'			overviewData.MA_Res4 = SafeGetString(reader, "MA_Res4")
		'			overviewData.MA_Res5 = SafeGetString(reader, "MA_Res5")

		'			overviewData.LL_Name = SafeGetString(reader, "LL_Name")
		'			overviewData.Reserve0 = SafeGetString(reader, "Reserve0")
		'			overviewData.Reserve1 = SafeGetString(reader, "Reserve1")
		'			overviewData.Reserve2 = SafeGetString(reader, "Reserve2")
		'			overviewData.Reserve3 = SafeGetString(reader, "Reserve3")
		'			overviewData.Reserve4 = SafeGetString(reader, "Reserve4")
		'			overviewData.Reserve5 = SafeGetString(reader, "Reserve5")
		'			overviewData.Reserve6 = SafeGetString(reader, "Reserve6")
		'			overviewData.Reserve7 = SafeGetString(reader, "Reserve7")
		'			overviewData.Reserve8 = SafeGetString(reader, "Reserve8")
		'			overviewData.Reserve9 = SafeGetString(reader, "Reserve9")
		'			overviewData.Reserve10 = SafeGetString(reader, "Reserve10")
		'			overviewData.Reserve11 = SafeGetString(reader, "Reserve11")
		'			overviewData.Reserve12 = SafeGetString(reader, "Reserve12")
		'			overviewData.Reserve13 = SafeGetString(reader, "Reserve13")
		'			overviewData.Reserve14 = SafeGetString(reader, "Reserve14")
		'			overviewData.Reserve15 = SafeGetString(reader, "Reserve15")
		'			overviewData.ReserveRtf0 = SafeGetString(reader, "_Reserve0")
		'			overviewData.ReserveRtf1 = SafeGetString(reader, "_Reserve1")
		'			overviewData.ReserveRtf2 = SafeGetString(reader, "_Reserve2")
		'			overviewData.ReserveRtf3 = SafeGetString(reader, "_Reserve3")
		'			overviewData.ReserveRtf4 = SafeGetString(reader, "_Reserve4")
		'			overviewData.ReserveRtf5 = SafeGetString(reader, "_Reserve5")
		'			overviewData.ReserveRtf6 = SafeGetString(reader, "_Reserve6")
		'			overviewData.ReserveRtf7 = SafeGetString(reader, "_Reserve7")
		'			overviewData.ReserveRtf8 = SafeGetString(reader, "_Reserve8")
		'			overviewData.ReserveRtf9 = SafeGetString(reader, "_Reserve9")
		'			overviewData.ReserveRtf10 = SafeGetString(reader, "_Reserve10")
		'			overviewData.ReserveRtf11 = SafeGetString(reader, "_Reserve11")
		'			overviewData.ReserveRtf12 = SafeGetString(reader, "_Reserve12")
		'			overviewData.ReserveRtf13 = SafeGetString(reader, "_Reserve13")
		'			overviewData.ReserveRtf14 = SafeGetString(reader, "_Reserve14")
		'			overviewData.ReserveRtf15 = SafeGetString(reader, "_Reserve15")


		'			result = overviewData

		'		End If

		'	Catch e As Exception
		'		result = Nothing
		'		m_Logger.LogError(e.ToString)

		'	Finally
		'		CloseReader(reader)

		'	End Try

		'	Return result

		'End Function


#End Region


	End Class


End Namespace
