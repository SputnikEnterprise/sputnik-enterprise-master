Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Listing.DataObjects

Namespace Listing


	Partial Class ListingDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IListingDatabaseAccess

		Function LoadEmploymentsEmployeeData() As IEnumerable(Of EmployeeMasterData) Implements IListingDatabaseAccess.LoadEmploymentsEmployeeData

			Dim result As List(Of EmployeeMasterData) = Nothing

			Dim sql As String

			sql = "Select "
#If DEBUG Then
			sql &= "Top 10 "
#End If
			sql &= "MANr "
			sql &= "From dbo.ES "
			sql &= "Group By ES.MANr "
			sql &= "Order By ES.MANr"


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing, CommandType.Text)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeMasterData)

					While reader.Read

						Dim data = New EmployeeMasterData

						data.EmployeeNumber = SafeGetInteger(reader, "MANr", 0)

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

		Function LoadEmployeePayrollData(ByVal mdnr As Integer, ByVal year As Integer?) As IEnumerable(Of PayrollPrintData) Implements IListingDatabaseAccess.LoadEmployeePayrollData

			Dim result As List(Of PayrollPrintData) = Nothing

			Dim sql As String

			sql = "SELECT "
#If DEBUG Then
			sql &= "Top 10 "
#End If
			sql &= "LO.ID, LO.MDNr, LO.LONr, LO.MANr, "
			sql &= "Convert(Bit, (CASE "
			sql &= "WHEN ISNULL(MA.email, '') = '' THEN 0 "
			sql &= "Else ISNULL(MA.send2wos, 0) "
			sql &= "End "
			sql &= ")) SendData2WOS, "

			sql &= "Convert(Bit, (CASE "
			sql &= "WHEN ISNULL(MA.email, '') = '' THEN 0 "
			sql &= "Else ISNULL(MA.SendDataWithEMail, 0) "
			sql &= "End "
			sql &= ")) SendDataWithEMail, "

			sql &= "IsNull(MA.Sprache, 'deutsch') As Sprache, "
			sql &= "IsNull(LO.LMID, 0) As LMID, "
			sql &= "IsNull(LO.ZGNr, 0) As ZGNr, "

			sql &= "(Select Top (1) VGNr From dbo.LOL Where LOL.MDNr = LO.MDNr AND LOL.MANr = LO.MANr And LOL.LONr = LO.LONr AND LOL.LANr = 8730 AND IsNull(LOL.VGNr, 0) > 0 AND IsNull(LOL.m_btr, 0) <> 0) LpVGNr, "

			sql &= "IsNull(LO.LODoc_Guid, '') As LOGuid, "
			sql &= "IsNull(MA.Transfered_Guid, '') As MAGuid, "
			sql &= "Convert(int, LO.LP) LP, "
			sql &= "Convert(Int, LO.Jahr) Jahr, "
			sql &= "MA.Nachname, MA.Vorname, "
			sql &= "MA.Geschlecht Gender, "
			sql &= "IsNull(MA.EMail, '') EmployeeEMail, "
			sql &= "IsNull( (Select Top (1) SendAsZIP From dbo.MA_LOSetting L Where L.MANr = MA.MANr), 0) SendAsZIP,  "
			sql &= "LO.CreatedOn, "
			sql &= "LO.CreatedFrom "

			sql &= "FROM dbo.LO "
			sql &= "Left Join dbo.Mitarbeiter MA On LO.MANr = MA.MANr "

			sql &= "Where (LO.IsComplete = 1) "
			sql &= "And (@mdnr = 0 OR LO.MDNr = @mdnr) "
			sql &= "And (@year = 0 OR Convert(Int, LO.Jahr) = @year) "

			sql &= "Order BY LO.LONr "


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdnr", ReplaceMissing(mdnr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("year", ReplaceMissing(year, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of PayrollPrintData)

					While reader.Read

						Dim data = New PayrollPrintData()

						data.recID = SafeGetInteger(reader, "ID", 0)
						data.MDNr = SafeGetInteger(reader, "MDNr", 0)
						data.jahr = SafeGetInteger(reader, "jahr", 0)
						data.monat = SafeGetInteger(reader, "LP", 0)
						data.LONr = SafeGetInteger(reader, "LONr", 0)
						data.MANr = SafeGetInteger(reader, "manr", 0)
						data.LMID = SafeGetInteger(reader, "LMID", 0)
						data.lpVGNr = SafeGetInteger(reader, "LpVGNr", 0)
						data.ZGNumber = SafeGetInteger(reader, "ZGNr", 0)

						data.employeefirstname = SafeGetString(reader, "vorname")
						data.employeelastname = SafeGetString(reader, "nachname")
						data.EmployeeEMail = SafeGetString(reader, "EmployeeEMail")
						data.SendDataWithEMail = SafeGetBoolean(reader, "SendDataWithEMail", False)
						data.SendAsZIP = SafeGetBoolean(reader, "SendAsZIP", False)

						data.Send2WOS = SafeGetBoolean(reader, "SendData2WOS", False)
						Dim lang = SafeGetString(reader, "Sprache")
						If String.IsNullOrWhiteSpace(lang) Then lang = "D" Else lang = lang.ToUpper.ToString.Substring(0, 1)
						data.employeeLanguage = lang

						data.Gender = SafeGetString(reader, "Gender")
						data.EmployeeGuid = SafeGetString(reader, "MAGuid")
						data.PayrollGuid = SafeGetString(reader, "LOGuid")

						data.createdon = SafeGetDateTime(reader, "createdon", Nothing)
						data.createdfrom = SafeGetString(reader, "CreatedFrom")


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

		Function LoadEmployeesZVGroupByMonthYearData(ByVal mdnr As Integer, ByVal year As Integer?) As IEnumerable(Of PayrollPrintData) Implements IListingDatabaseAccess.LoadEmployeesZVGroupByMonthYearData

			Dim result As List(Of PayrollPrintData) = Nothing

			Dim sql As String

			sql = "SELECT "
#If DEBUG Then
			sql &= "Top 10 "
#End If
			sql &= "LO.MDNr, LO.MANr, "

			sql &= "Convert(int, LO.LP) LP, "
			sql &= "Convert(Int, LO.Jahr) Jahr, "
			sql &= "MA.Nachname, MA.Vorname, "
			sql &= "MA.Geschlecht Gender "

			sql &= "FROM dbo.LO "
			sql &= "Left Join dbo.Mitarbeiter MA On LO.MANr = MA.MANr "
			sql &= "LEFT Join dbo.MAKontakt_Komm MAKomm On MAKomm.MANr = MA.MANr "

			sql &= "Where (LO.IsComplete = 1) "
			sql &= "And (@mdnr = 0 OR LO.MDNr = @mdnr) "
			sql &= "And (@year = 0 OR Convert(Int, LO.Jahr) = @year) "
			sql &= "AND ISNULL(MAKomm.InZV, 0) = 1 "

			sql &= "Group By LO.MDNr, LO.MANr, Convert(int, LO.LP), Convert(Int, LO.Jahr), "
			sql &= "MA.Nachname, MA.Vorname, MA.Geschlecht "

			sql &= "Order BY LO.MANr, Convert(Int, LO.LP), Convert(Int, LO.Jahr) "


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdnr", ReplaceMissing(mdnr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("year", ReplaceMissing(year, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of PayrollPrintData)

					While reader.Read

						Dim data = New PayrollPrintData()

						data.MDNr = SafeGetInteger(reader, "MDNr", 0)
						data.MANr = SafeGetInteger(reader, "manr", 0)
						data.jahr = SafeGetInteger(reader, "jahr", 0)
						data.monat = SafeGetInteger(reader, "LP", 0)

						data.Gender = SafeGetString(reader, "Gender")
						data.employeefirstname = SafeGetString(reader, "vorname")
						data.employeelastname = SafeGetString(reader, "nachname")

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

		Function LoadEmployeesArgbGroupByYearData(ByVal mdnr As Integer, ByVal year As Integer?) As IEnumerable(Of PayrollPrintData) Implements IListingDatabaseAccess.LoadEmployeesArgbGroupByYearData

			Dim result As List(Of PayrollPrintData) = Nothing

			Dim sql As String

			sql = "SELECT "
#If DEBUG Then
			sql &= "Top 10 "
#End If
			sql &= "LO.MDNr, LO.MANr, "

			sql &= "Convert(Int, LO.Jahr) Jahr, "
			sql &= "MA.Nachname, MA.Vorname, "
			sql &= "MA.Geschlecht Gender "

			sql &= "FROM LO "
			sql &= "Left Join Mitarbeiter MA On LO.MANr = MA.MANr "
			sql &= "LEFT Join dbo.MAKontakt_Komm MAKomm On MAKomm.MANr = MA.MANr "

			sql &= "Where (LO.IsComplete = 1) "
			sql &= "And (@mdnr = 0 OR LO.MDNr = @mdnr) "
			sql &= "And (@year = 0 OR Convert(Int, LO.Jahr) = @year) "
			sql &= "AND ISNULL(MAKomm.InZV, 0) = 1 "

			sql &= "Group By LO.MDNr, LO.MANr, Convert(Int, LO.Jahr), "
			sql &= "MA.Nachname, MA.Vorname, MA.Geschlecht "

			sql &= "Order BY LO.MANr, Convert(Int, LO.Jahr) "


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdnr", ReplaceMissing(mdnr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("year", ReplaceMissing(year, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of PayrollPrintData)

					While reader.Read

						Dim data = New PayrollPrintData()

						data.MDNr = SafeGetInteger(reader, "MDNr", 0)
						data.MANr = SafeGetInteger(reader, "manr", 0)
						data.jahr = SafeGetInteger(reader, "jahr", 0)

						data.Gender = SafeGetString(reader, "Gender")
						data.employeefirstname = SafeGetString(reader, "vorname")
						data.employeelastname = SafeGetString(reader, "nachname")

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

		Function LoadEmployeesNLAGroupByYearData(ByVal mdnr As Integer, ByVal year As Integer?, ByVal manrListe As String) As IEnumerable(Of PayrollNLAData) Implements IListingDatabaseAccess.LoadEmployeesNLAGroupByYearData

			Dim result As List(Of PayrollNLAData) = Nothing

			Dim sql As String

			sql = "[Create New Table For LoNLA With Mandant]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdnr", ReplaceMissing(mdnr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("jahr", ReplaceMissing(year, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("manrListe", ReplaceMissing(manrListe, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("nameVon", String.Empty))
			listOfParams.Add(New SqlClient.SqlParameter("nameBis", String.Empty))
			listOfParams.Add(New SqlClient.SqlParameter("tblName", String.Empty))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of PayrollNLAData)

					While reader.Read

						Dim overviewData = New PayrollNLAData()

						overviewData.MANr = SafeGetInteger(reader, "MANr", Nothing)
						overviewData.employeefirstname = String.Format("{0}", SafeGetString(reader, "Vorname"))
						overviewData.employeelastname = String.Format("{0}", SafeGetString(reader, "Nachname"))
						overviewData.employeemastrasse = SafeGetString(reader, "MAStrasse")
						overviewData.employeemaplz = SafeGetString(reader, "MAPLZ")
						overviewData.employeemaort = SafeGetString(reader, "MAOrt")

						overviewData.employeemaland = SafeGetString(reader, "MALand")
						overviewData.employeemaco = SafeGetString(reader, "MACo")

						overviewData.employeeahv_nr = SafeGetString(reader, "AHV_Nr")
						overviewData.employeeahv_nr_new = SafeGetString(reader, "AHV_Nr_New")

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

		Function LoadPayrollEvaluationData(ByVal mdnr As Integer, ByVal year As Integer?) As IEnumerable(Of PayrollEvaluationListData) Implements IListingDatabaseAccess.LoadPayrollEvaluationData

			Dim result As List(Of PayrollEvaluationListData) = Nothing

			Dim sql As String

			sql = "[Load Payroll For Export Payroll Evaluation List]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdnr", ReplaceMissing(mdnr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("year", ReplaceMissing(year, 0)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of PayrollEvaluationListData)

					While reader.Read

						Dim data As New PayrollEvaluationListData

						data.LONr = SafeGetInteger(reader, "LONr", 0)
						data.MANr = SafeGetInteger(reader, "MANr", 0)
						data.Monat = SafeGetInteger(reader, "Monat", 0)
						data.Jahr = SafeGetInteger(reader, "Jahr", 0)
						data.WohnOrt = SafeGetString(reader, "WohnOrt", "")
						data.Employeename = SafeGetString(reader, "Employeename", "")
						data.Country = SafeGetString(reader, "Country", "")
						data.S_Kanton = SafeGetString(reader, "S_Kanton", "")
						data.Zivilstand = SafeGetString(reader, "Zivilstand", "")
						data.Kirchensteuer = SafeGetString(reader, "Kirchensteuer", "")
						data.Q_Steuer = SafeGetString(reader, "Q_Steuer", "")
						data.AnzahlKinder = SafeGetInteger(reader, "AnzahlKinder", 0)
						data.WorkedDay = SafeGetInteger(reader, "WorkedDays", 0)
						data.Bruttolohn = SafeGetDecimal(reader, "Bruttolohn", 0)
						data.AHVBasis = SafeGetDecimal(reader, "AHVBasis", 0)
						data.AHVLohn = SafeGetDecimal(reader, "AHVLohn", 0)
						data.AHVFreibetrag = SafeGetDecimal(reader, "AHVFreibetrag", 0)
						data.NichtAHV = SafeGetDecimal(reader, "NichtAHV", 0)
						data.ALV1Lohn = SafeGetDecimal(reader, "ALV1Lohn", 0)
						data.ALV2Lohn = SafeGetDecimal(reader, "ALV2Lohn", 0)
						data.SUVABasis = SafeGetDecimal(reader, "SUVABasis", 0)
						data.QSTBasis = SafeGetDecimal(reader, "QSTBasis", 0)
						data.QSTTarif = SafeGetString(reader, "QSTTarif", "")
						data.ESData = SafeGetString(reader, "ESData", "")
						data.BVGBegin = SafeGetDateTime(reader, "BVGBegin", Nothing)
						data.BVGEnd = SafeGetDateTime(reader, "BVGEnd", Nothing)
						data.Ansaessigkeit = SafeGetBoolean(reader, "Ansaessigkeit", False)
						data.BVGBeginEnd = SafeGetString(reader, "BVGBeginEnd", "")
						data.CHPartner = SafeGetBoolean(reader, "CHPartner", False)
						data.NoSpecialTax = SafeGetBoolean(reader, "NoSpecialTax", False)
						data.Permission = SafeGetString(reader, "Permission", "")
						data.PermissionToDate = SafeGetDateTime(reader, "PermissionToDate", Nothing)
						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom", "")
						data.WorkedHour = SafeGetDecimal(reader, "WorkedHour", 0)
						data.AHVBeitrag = SafeGetDecimal(reader, "AHVBeitrag", 0)
						data.ALVBeitrag = SafeGetDecimal(reader, "ALVBeitrag", 0)
						data.ALV2Beitrag = SafeGetDecimal(reader, "ALV2Beitrag", 0)
						data.FARTotal = SafeGetDecimal(reader, "FARTotal", 0)
						data.ParifondTotal = SafeGetDecimal(reader, "ParifondTotal", 0)
						data.NBUVBeitrag = SafeGetDecimal(reader, "NBUVBeitrag", 0)
						data.KTGBeitrag = SafeGetDecimal(reader, "KTGBeitrag", 0)
						data.BVGTotal = SafeGetDecimal(reader, "BVGTotal", 0)
						data.QSTTotal = SafeGetDecimal(reader, "QSTTotal", 0)
						data.MinusLohn = SafeGetDecimal(reader, "MinusLohn", 0)
						data._8700 = SafeGetDecimal(reader, "_8700", 0)
						data._8720 = SafeGetDecimal(reader, "_8720", 0)
						data._8730 = SafeGetDecimal(reader, "_8730", 0)
						data.ZGTotal = SafeGetDecimal(reader, "ZGTotal", 0)
						data.ZGTotalFees = SafeGetDecimal(reader, "ZGTotalFees", 0)
						data.AuszahlungTotal = SafeGetDecimal(reader, "AuszahlungTotal", 0)
						data.NegativLohn = SafeGetDecimal(reader, "NegativLohn", 0)


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

		Function LoadAdvancedpaymentEvaluationData(ByVal mdnr As Integer, ByVal year As Integer?) As IEnumerable(Of AdvancedpaymentEvaluationListData) Implements IListingDatabaseAccess.LoadAdvancedpaymentEvaluationData

			Dim result As List(Of AdvancedpaymentEvaluationListData) = Nothing

			Dim sql As String

			sql = "[Load Advancedpayment For Export Evaluation List]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdnr", ReplaceMissing(mdnr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("year", ReplaceMissing(year, 0)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of AdvancedpaymentEvaluationListData)

					While reader.Read

						Dim data As New AdvancedpaymentEvaluationListData

						data.MDNr = SafeGetInteger(reader, "MDNr", 0)
						data.ZGNr = SafeGetInteger(reader, "ZGNr", 0)
						data.RPNr = SafeGetInteger(reader, "RPNr", 0)
						data.LONr = SafeGetInteger(reader, "LONr", 0)
						data.MANr = SafeGetInteger(reader, "MANr", 0)
						data.VGNr = SafeGetInteger(reader, "VGNr", 0)
						data.Monat = SafeGetInteger(reader, "Monat", 0)
						data.Jahr = SafeGetInteger(reader, "Jahr", 0)

						data.Grund = SafeGetString(reader, "ZGGRUND", "")
						data.Amount = SafeGetDecimal(reader, "Betrag", 0)
						data.Aus_Date = SafeGetDateTime(reader, "Aus_Dat", Nothing)
						data.GebAbzug = SafeGetBoolean(reader, "GebAbzug", False)
						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom", "")
						data.BankAU = SafeGetBoolean(reader, "BnkAU", False)
						data.DTADate = SafeGetDateTime(reader, "DTADate", Nothing)

						data.ClearingNr = SafeGetInteger(reader, "ClearingNr", 0)
						data.Bankname = SafeGetString(reader, "Bank", "")
						data.KontoNr = SafeGetString(reader, "KontoNr", "")
						data.BankOrt = SafeGetString(reader, "BankOrt", "")
						data.Addressline_1 = SafeGetString(reader, "DTAAdr1", "")
						data.Addressline_2 = SafeGetString(reader, "DTAAdr2", "")
						data.Addressline_3 = SafeGetString(reader, "DTAAdr3", "")
						data.Addressline_4 = SafeGetString(reader, "DTAAdr4", "")

						data.IBANNr = SafeGetString(reader, "IBANNr", "")

						data.Swift = SafeGetString(reader, "Swift", "")
						data.BLZNr = SafeGetString(reader, "BLZ", "")
						data.PrintedOn = SafeGetDateTime(reader, "Printed_Dat", Nothing)
						data.IsCreatedWithLO = SafeGetBoolean(reader, "IsCreatedWithLO", False)

						data.Lastname = SafeGetString(reader, "Nachname", "")
						data.Firstname = SafeGetString(reader, "Vorname", "")


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


	End Class


End Namespace
