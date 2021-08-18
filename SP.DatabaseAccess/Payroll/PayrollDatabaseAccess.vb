Imports SP.DatabaseAccess
Imports System.Text
Imports SP.Infrastructure
Imports SP.DatabaseAccess.PayrollMng.DataObjects
Imports SP.Infrastructure.DateAndTimeCalculation

Namespace PayrollMng

	Partial Public Class PayrollDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IPayrollDatabaseAccess

#Region "Constructor"

		''' <summary>
		''' Constructor.
		''' </summary>
		''' <param name="connectionString">The connection string.</param>
		''' <param name="translationLanguage">The translation language.</param>
		Public Sub New(ByVal connectionString As String, ByVal translationLanguage As Language)
			MyBase.New(connectionString, translationLanguage)

		End Sub

		''' <summary>
		''' Constructor.
		''' </summary>
		''' <param name="connectionString">The connection string.</param>
		''' <param name="translationLanguage">The translation language.</param>
		Public Sub New(ByVal connectionString As String, ByVal translationLanguage As String)
			MyBase.New(connectionString, translationLanguage)
		End Sub

#End Region

#Region "Private Methods"

		Private Function NumberRound(varZahl As Object, Optional ByVal Digit As Integer = 0) As Decimal
			On Error Resume Next

			If Digit = 0 Then Digit = 4
			'Auf 0,05 Rappen runden
			NumberRound = Convert.ToDecimal(Format(CLng(varZahl / 0.05) * 0.05, "0." & New String("0", Digit)))


		End Function

#End Region

#Region "Public Methods"

		''' <summary>
		''' Loads LABez data for payroll.
		''' </summary>
		''' <returns>List of LA Bez data or nothing in error case.</returns>
		Function LoadPayrollMasterData(ByVal payrollNumber As Integer) As LOMasterData Implements IPayrollDatabaseAccess.LoadPayrollMasterData

			Dim result As LOMasterData = Nothing

			Dim sql As String
			sql = "SELECT ID"
			sql &= ", MDNr"
			sql &= ", LONr"
			sql &= ", MANr"
			sql &= ", Convert(Int, LP) LP"
			sql &= ", Convert(Int, Jahr) Jahr"
			sql &= ", Convert(Int, Anzahlkinder) Anzahlkinder"
			sql &= ", LODoc_Guid "
			sql &= "From LO Where LONr = @LONr"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@LONr", payrollNumber))
			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try
				If (Not reader Is Nothing) Then

					result = New LOMasterData

					While reader.Read

						result.ID = SafeGetInteger(reader, "ID", Nothing)
						result.LONR = SafeGetInteger(reader, "LONR", Nothing)
						result.MDNr = SafeGetInteger(reader, "MDNr", Nothing)
						result.MANR = SafeGetInteger(reader, "MANR", Nothing)
						result.LP = SafeGetInteger(reader, "LP", Nothing)
						result.Jahr = SafeGetInteger(reader, "Jahr", Nothing)
						result.AnzahlKinder = SafeGetInteger(reader, "AnzahlKinder", Nothing)
						result.LODoc_Guid = SafeGetString(reader, "LODoc_Guid")

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
		''' Loads LABez data for payroll.
		''' </summary>
		''' <returns>List of LA Bez data or nothing in error case.</returns>
		Function LoadLABezDataForPayroll() As IEnumerable(Of LA_BezData) Implements IPayrollDatabaseAccess.LoadLABezDataForPayroll

			Dim result As List(Of LA_BezData) = Nothing

			Dim sql As String
			sql = "SELECT LANr, IsNull(NAME_I, LAText) NAME_I, IsNull(NAME_E, LAText) NAME_E, IsNull(NAME_F, LAText) NAME_F "
			sql &= "FROM LA_Translated Where LANr Is Not Null "
			sql &= "Group By LANr, LAText, IsNull(NAME_I, LAText), IsNull(NAME_E, LAText), IsNull(NAME_F, LAText) "
			sql &= "Order By LANr"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing, CommandType.Text)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of LA_BezData)

					While reader.Read

						Dim data = New LA_BezData

						data.LANr = SafeGetDecimal(reader, "LANr", Nothing)
						data.Name_I = SafeGetString(reader, "Name_I")
						data.Name_E = SafeGetString(reader, "Name_E")
						data.Name_F = SafeGetString(reader, "Name_F")

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
		''' Loads available employees for payroll.
		''' </summary>
		''' <param name="mandatNumber">The mandant number.</param>
		''' <param name="month">The month.</param>
		''' <param name="year">The year.</param>
		''' <returns>List of available employees for payroll or nothing in error case.</returns>
		Function LoadAvailableEmployeesForPayroll(ByVal mandatNumber As Integer, ByVal month As Integer, ByVal year As Integer) As IEnumerable(Of EmployeeDataForPayroll) Implements IPayrollDatabaseAccess.LoadAvailableEmployeesForPayroll

			Dim result As List(Of EmployeeDataForPayroll) = Nothing

			Dim sql As String = "[Get Available Employees for Payroll]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", mandatNumber))
			listOfParams.Add(New SqlClient.SqlParameter("@Month", month))
			listOfParams.Add(New SqlClient.SqlParameter("@Year", year))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeDataForPayroll)

					While reader.Read

						Dim data = New EmployeeDataForPayroll

						data.MANr = SafeGetInteger(reader, "MANr", 0)
						data.Nachname = SafeGetString(reader, "Nachname", String.Empty)
						data.Vorname = SafeGetString(reader, "Vorname", String.Empty)
						data.Q_Steuer = SafeGetString(reader, "Q_Steuer", String.Empty)
						data.Kinder = SafeGetShort(reader, "Kinder", 0)
						data.S_Kanton = SafeGetString(reader, "S_Kanton", String.Empty)
						data.Kirchensteuer = SafeGetString(reader, "Kirchensteuer", String.Empty)
						data.Bewillig = SafeGetString(reader, "Bewillig", String.Empty)
						data.Zahlart = SafeGetString(reader, "Zahlart", String.Empty)
						data.NoLO = SafeGetBoolean(reader, "NoLO", False)
						data.FerienBack = SafeGetBoolean(reader, "FerienBack", False)
						data.FeierBack = SafeGetBoolean(reader, "FeierBack", False)
						data.Lohn13Back = SafeGetBoolean(reader, "Lohn13Back", False)
						data.MAGleitzeit = SafeGetBoolean(reader, "MAGleitzeit", False)
						data.InZV = SafeGetBoolean(reader, "InZV", False)
						data.Is_Current_LO_Existing = SafeGetInteger(reader, "Is_Current_LO_Existing", 0) = 1
						data.Is_NonComplete_RP_Existing = SafeGetInteger(reader, "Is_NonComplete_RP_Existing", 0) = 1
						data.Is_PreviousMonth_ES_With_No_LO_Existing = SafeGetInteger(reader, "Is_PreviousMonth_ES_With_No_LO_Existing", 0) = 1
						data.EmployeeLOProcessState = EmployeeLOProcessState.Unprocessed

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
		''' Loads employee detail data for payroll.
		''' </summary>
		''' <param name="mandantNumber">The mandant number.</param>
		''' <param name="month">The month.</param>
		''' <param name="year">The year.</param>
		''' <param name="language">The language.</param>
		''' <returns>List of employee detail data for payroll or nothing in error case.</returns>
		Function LoadEmplyoeeDetailDataForPayroll(ByVal maNr As Integer, ByVal mandantNumber As Integer, ByVal month As Integer, ByVal year As Integer, ByVal language As String) As IEnumerable(Of EmployeeDetailDataForPayroll) Implements IPayrollDatabaseAccess.LoadEmplyoeeDetailDataForPayroll

			Dim result As List(Of EmployeeDetailDataForPayroll) = Nothing

			Dim sql As String = "[Get Employeedetail for Payroll]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MANr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", mandantNumber))
			listOfParams.Add(New SqlClient.SqlParameter("@Month", month))
			listOfParams.Add(New SqlClient.SqlParameter("@Year", year))
			listOfParams.Add(New SqlClient.SqlParameter("@Language", language))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeDetailDataForPayroll)

					While reader.Read

						Dim data = New EmployeeDetailDataForPayroll

						data.MANr = SafeGetInteger(reader, "MANr", 0)
						data.LANR = SafeGetDecimal(reader, "LANR", 0)
						data.LALoText = SafeGetString(reader, "LALoText", String.Empty)
						data.M_Anzahl = SafeGetDecimal(reader, "M_Anzahl", 0)
						data.M_Ansatz = SafeGetDecimal(reader, "M_Ansatz", 0)
						data.M_Basis = SafeGetDecimal(reader, "M_Basis", 0)
						data.M_Betrag = SafeGetDecimal(reader, "M_Betrag", 0)
						data.ModulNr = SafeGetInteger(reader, "RPNR", 0)
						data.ModulName = SafeGetString(reader, "Modul", String.Empty)
						data.Company1 = SafeGetString(reader, "Firma1")
						data.Notice_Payroll = SafeGetString(reader, "Notice_Payroll")

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
		''' Loads invalid record number for payroll.
		''' </summary>
		''' <param name="mandantNumber">The mandant number.</param>
		''' <param name="maNr">The employee number.</param>
		''' <param name="month">The month.</param>
		''' <param name="year">The year.</param>
		''' <returns>List of invalid record number or nothing in error case.</returns>
		Function LoadInvalidRecordNumbersForPayroll(ByVal mandantNumber As Integer, ByVal maNr As Integer, ByVal month As Integer, ByVal year As Integer) As IEnumerable(Of ModuleRecordNumber) Implements IPayrollDatabaseAccess.LoadInvalidRecordNumbersForPayroll

			Dim result As List(Of ModuleRecordNumber) = Nothing

			Dim sql As String = "[Get InvalidRecordNumbers for Payroll]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", mandantNumber))
			listOfParams.Add(New SqlClient.SqlParameter("@MANr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@Month", month))
			listOfParams.Add(New SqlClient.SqlParameter("@Year", year))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of ModuleRecordNumber)

					While reader.Read

						Dim data = New ModuleRecordNumber

						data.RecordNumber = SafeGetInteger(reader, "RecordNumber", 0)
						data.ModuleName = SafeGetString(reader, "ModuleName", String.Empty)

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
		''' Loads the work days for a month.
		''' </summary>
		''' <param name="maNr">Th employee number.</param>
		''' <param name="mdNr">The mandant number.</param>
		''' <param name="year">The year.</param>
		''' <param name="month">The month.</param>
		''' <returns>The number of work days or nothing in error case.</returns>
		Function LoadESWorkDaysForAMonth(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal year As Integer, ByVal month As Integer) As Integer? Implements IPayrollDatabaseAccess.LoadESWorkDaysForAMonth

			Dim workDays As Integer? = Nothing

			Dim sql As String = String.Empty

			sql = sql & "SELECT ES_Ab, ES_Ende FROM ES "
			sql = sql & "WHERE MANr = @maNr And MDNr = @mdNr And "
			' Overlap of two time periods = a.start <= b.end && b.start <= a.end;
			sql = sql & "ES_Ab <= @endOfMonth And (ES_Ende Is NULL Or (@startOfMonth <= ES_Ende))"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("maNr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))

			Dim startOfMonth As New DateTime(year, month, 1)
			Dim endOfMonth = startOfMonth.AddMonths(1).AddDays(-1)

			listOfParams.Add(New SqlClient.SqlParameter("startOfMonth", startOfMonth))
			listOfParams.Add(New SqlClient.SqlParameter("endOfMonth", endOfMonth))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					workDays = 0

					Dim daysWorkedInMonth(31) As Boolean

					While reader.Read

						Dim startDate As DateTime = SafeGetDateTime(reader, "ES_Ab", Nothing)
						Dim endDate As DateTime = SafeGetDateTime(reader, "ES_Ende", endOfMonth)

						If startDate > endDate Then
							' Skip corrupted data
							Continue While
						End If

						' Start date must not be befor start of month
						startDate = If(startDate < startOfMonth, startOfMonth, startDate)

						' End date must not be after end of month
						endDate = If(endDate > endOfMonth, endOfMonth, endDate)

						If startDate = endDate Then

							' Only one worked day
							daysWorkedInMonth(startDate.Day - 1) = True
						Else

							' Make sure enddate does not exceed the 31.th of a month
							If endDate.Day = 31 Then
								endDate = endDate.AddDays(-1)
							End If

							Dim currday = startDate

							' Loop trough all days of ES
							While currday <= endDate

								Dim day = currday.Day

								' Mark the day as worked
								daysWorkedInMonth(day - 1) = True

								currday = currday.AddDays(1)
							End While

						End If

					End While

					' Count number or worked days
					workDays = daysWorkedInMonth.Where(Function(data) data = True).Count()

					' Special case for February
					If month = 2 Then
						' Correct 28 or 29 wored days in februar to 30 days.
						workDays = If(workDays = 28 Or workDays = 29, 30, workDays)

					End If

				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				workDays = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return workDays

		End Function

		''' <summary>
		''' Load ES data for ES day in year calculation.
		''' </summary>
		''' <param name="maNr">The MANr.</param>
		''' <param name="mdNr">The MDNr.</param>
		''' <param name="startOfMonth">The start of month.</param>
		''' <param name="endOfMonth">The end of month.</param>
		''' <returns>List of ES data or nothing in error case.</returns>
		Function LoadESDataForESDayInYearCalculation(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal startOfMonth As DateTime, ByVal endOfMonth As DateTime) As List(Of ESDataForESDayInYearCalculation) Implements IPayrollDatabaseAccess.LoadESDataForESDayInYearCalculation

			Dim result As List(Of ESDataForESDayInYearCalculation) = Nothing

			Dim sSql As String = String.Empty

			sSql = sSql & "Select ES_Ab, ES_Ende From ES Where "
			sSql = sSql & "MANr = @maNr And ES_Ab <= @endOfMonth And (ES_Ende Is Null Or ES_Ende >= @startOfMonth) "
			sSql = sSql & "And ES.MDNr = @mdNr "
			sSql = sSql & "Order By ES_Ab"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@maNr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("@startOfMonth", startOfMonth))
			listOfParams.Add(New SqlClient.SqlParameter("@endOfMonth", endOfMonth))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sSql, listOfParams, CommandType.Text)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of ESDataForESDayInYearCalculation)

					While reader.Read

						Dim data = New ESDataForESDayInYearCalculation
						data.ES_Ab = SafeGetDateTime(reader, "ES_Ab", Nothing)
						data.ES_Ende = SafeGetDateTime(reader, "ES_Ende", Nothing)

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
		''' Loads mandant data.
		''' </summary>
		''' <param name="year">The year.</param>
		''' <param name="mdNr">The mandant number.</param>
		''' <returns>The mandant data.</returns>
		Function LoadMandantData(ByVal year As Integer, ByVal mdNr As Integer) As MandantData Implements IPayrollDatabaseAccess.LoadMandantData

			Dim result As MandantData = Nothing

			Dim sql As String = String.Empty

			sql = sql & "SELECT KK_An_MA, KK_AG_MA, KK_An_MZ, KK_AG_MZ, KK_An_WA , KK_AG_WA, KK_An_WZ, KK_AG_WZ, Suva_HL, ALV1_HL, ALV2_HL, RentAlter_M, RentAlter_W, "
			sql = sql & "BVG_Koordination_Jahr, BVG_Std, BVG_Max_Jahr, BVG_Min_Jahr, BVG_Aus1Woche, BVG_Aus2Woche, MindestAlter, "
			sql &= "BVG_List, BVG_List_Grouped "
			sql = sql & "FROM Mandanten "
			sql = sql & "WHERE Jahr = @year And MDNr = @mdNr "

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("year", year))
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = New MandantData

					result.KK_An_MA = SafeGetDecimal(reader, "KK_An_MA", 0)
					result.KK_AG_MA = SafeGetDecimal(reader, "KK_AG_MA", 0)
					result.KK_An_MZ = SafeGetDecimal(reader, "KK_An_MZ", 0)
					result.KK_AG_MZ = SafeGetDecimal(reader, "KK_AG_MZ", 0)
					result.KK_An_WA = SafeGetDecimal(reader, "KK_An_WA", 0)
					result.KK_AG_WA = SafeGetDecimal(reader, "KK_AG_WA", 0)
					result.KK_An_WZ = SafeGetDecimal(reader, "KK_An_WZ", 0)
					result.KK_AG_WZ = SafeGetDecimal(reader, "KK_AG_WZ", 0)
					result.Suva_HL = SafeGetDecimal(reader, "Suva_HL", 0)
					result.ALV1_HL = SafeGetDecimal(reader, "ALV1_HL", 0)
					result.ALV2_HL = SafeGetDecimal(reader, "ALV2_HL", 0)
					result.RentAlter_M = SafeGetShort(reader, "RentAlter_M", 0)
					result.RentAlter_W = SafeGetShort(reader, "RentAlter_W", 0)

					result.BVG_Koordination_Jahr = SafeGetDecimal(reader, "BVG_Koordination_Jahr", 0)
					result.BVG_Std = SafeGetInteger(reader, "BVG_Std", 0)
					result.BVG_Max_Jahr = SafeGetDecimal(reader, "BVG_Max_Jahr", 0)
					result.BVG_Min_Jahr = SafeGetDecimal(reader, "BVG_Min_Jahr", 0)
					result.BVG_Aus1Woche = SafeGetDecimal(reader, "BVG_Aus1Woche", 0)
					result.BVG_Aus2Woche = SafeGetDecimal(reader, "BVG_Aus2Woche", 0)

					result.MindestAlter = SafeGetShort(reader, "MindestAlter", 0)

					result.BVG_List = SafeGetString(reader, "BVG_List")
					result.BVG_List_Grouped = SafeGetString(reader, "BVG_List_Grouped", 13)

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
		''' Loads mandant Ansatz data.
		''' </summary>
		''' <param name="mdNr">The MDNr.</param>
		''' <param name="year">The year.</param>
		''' <returns>The Mandant Ansatz data or nothing error case.</returns>
		''' <remarks></remarks>
		Function LoadMandantAnsatzData(ByVal mdNr As Integer, ByVal year As Integer) As MandantAnsatzData Implements IPayrollDatabaseAccess.LoadMandantAnsatzData

			Dim result As MandantAnsatzData = Nothing

			Dim sql As String

			sql = "SELECT TOP 1 "
			sql &= "RentFrei_Monat, RentFrei_Jahr, NBUV_WStd, [AHV_AN], [AHV_2_AN], "
			sql &= "([ALV1_HL] / 12) As ALV1_HL_, ([ALV2_HL] / 12) As ALV2_HL_, "
			sql &= "[ALV_AN], [ALV2_An], "
			sql &= "([SUVA_HL] / 12 ) As SUVA_HL_, "
			sql &= "NBUV_M, NBUV_M_Z, NBUV_W, NBUV_W_Z, "
			sql &= "KK_An_MA, KK_An_MZ, KK_An_WA, KK_An_WZ, "
			sql &= "[AHV_AG], [AHV_2_AG], "
			sql &= "[ALV_AG], [ALV2_AG], "
			sql &= "Suva_A, Suva_Z, "
			sql &= "UVGZ_A, UVGZ_B, "
			sql &= "UVGZ2_A, UVGZ2_B, "
			sql &= "KK_AG_MA, KK_AG_MZ, KK_AG_WA, KK_AG_WZ, "
			sql &= "Fak_Proz "
			sql &= "From Mandanten WHERE MDNr = @mdNr And Jahr = @year"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("year", year))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = New MandantAnsatzData

					result.RentFrei_Monat = SafeGetDecimal(reader, "RentFrei_Monat", 0)
					result.RentFrei_Jahr = SafeGetDecimal(reader, "RentFrei_Jahr", 0)
					result.NBUV_WStd = SafeGetDecimal(reader, "NBUV_WStd", 0)
					result.AHV_AN = SafeGetDecimal(reader, "AHV_AN", 0)
					result.AHV_2_AN = SafeGetDecimal(reader, "AHV_2_AN", 0)
					result.ALV1_HL_ = SafeGetDecimal(reader, "ALV1_HL_", 0)
					result.ALV2_HL_ = SafeGetDecimal(reader, "ALV2_HL_", 0)
					result.ALV_AN = SafeGetDecimal(reader, "ALV_AN", 0)
					result.ALV2_An = SafeGetDecimal(reader, "ALV2_An", 0)
					result.SUVA_HL_ = SafeGetDecimal(reader, "SUVA_HL_", 0)
					result.NBUV_M = SafeGetDecimal(reader, "NBUV_M", 0)
					result.NBUV_M_Z = SafeGetDecimal(reader, "NBUV_M_Z", 0)
					result.NBUV_W = SafeGetDecimal(reader, "NBUV_W", 0)
					result.NBUV_W_Z = SafeGetDecimal(reader, "NBUV_W_Z", 0)
					result.KK_An_MA = SafeGetDecimal(reader, "KK_An_MA", 0)
					result.KK_An_MZ = SafeGetDecimal(reader, "KK_An_MZ", 0)
					result.KK_An_WA = SafeGetDecimal(reader, "KK_An_WA", 0)
					result.KK_An_WZ = SafeGetDecimal(reader, "KK_An_WZ", 0)
					result.AHV_AG = SafeGetDecimal(reader, "AHV_AG", 0)
					result.AHV_2_AG = SafeGetDecimal(reader, "AHV_2_AG", 0)
					result.ALV_AG = SafeGetDecimal(reader, "ALV_AG", 0)
					result.ALV2_AG = SafeGetDecimal(reader, "ALV2_AG", 0)
					result.Suva_A = SafeGetDecimal(reader, "Suva_A", 0)
					result.Suva_Z = SafeGetDecimal(reader, "Suva_Z", 0)
					result.UVGZ_A = SafeGetDecimal(reader, "UVGZ_A", 0)
					result.UVGZ_B = SafeGetDecimal(reader, "uvgz_b", 0)
					result.UVGZ2_A = SafeGetDecimal(reader, "UVGZ2_A", 0)
					result.UVGZ2_B = SafeGetDecimal(reader, "uvgz2_b", 0)

					result.KK_AG_MA = SafeGetDecimal(reader, "KK_AG_MA", 0)
					result.KK_AG_MZ = SafeGetDecimal(reader, "KK_AG_MZ", 0)
					result.KK_AG_WA = SafeGetDecimal(reader, "KK_AG_WA", 0)
					result.KK_AG_WZ = SafeGetDecimal(reader, "KK_AG_WZ", 0)
					result.Fak_Proz = SafeGetDecimal(reader, "Fak_Proz", 0)
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
		''' Loads existing Lo for payroll.
		''' </summary>
		''' <param name="maNr">The MANr.</param>
		''' <param name="mdNr">The MDNr.</param>
		''' <param name="month">The Month.</param>
		''' <param name="year">The year.</param>
		''' <param name="err">The error flag.</param>
		''' <returns>Existing LONr.</returns>
		Function LoadExistLoForPayroll(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal month As Integer, ByVal year As Integer, ByRef err As Boolean) As Integer? Implements IPayrollDatabaseAccess.LoadExistLoForPayroll

			Dim result As Integer? = Nothing

			Dim sql As String

			sql = "[Exist Lo For Payroll]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MANummer", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@MDLp", month))
			listOfParams.Add(New SqlClient.SqlParameter("@MDYear", year))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = SafeGetInteger(reader, "LONr", 0)

				End If

			Catch e As Exception
				err = True
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		''' <summary>
		''' Finds a new LONr.
		''' </summary>
		''' <param name="offset">The offset.</param>
		''' <returns>New LONr or nothing  in error case.</returns>
		Function FindNewLONr(ByVal offset As Integer) As Integer? Implements IPayrollDatabaseAccess.FindNewLONr

			Dim result As Integer? = Nothing

			Dim sSql As String

			' Lohnabrechnungsnummer in der Mandantenverwaltung...

			sSql = "Select ISNULL((Select Top 1 [LONr] From LO Order By [LONr] DESC), 0) As LOLONr, "
			sSql = sSql & "ISNULL((Select Top 1 [LONr] From LOL Order By [LONr] DESC), 0) As LOLLonr "

			Dim reader As SqlClient.SqlDataReader = OpenReader(sSql, Nothing)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					Dim LOLONr As Integer = SafeGetInteger(reader, "LOLONr", 0)
					Dim LOLLonr As Integer = SafeGetInteger(reader, "LOLLonr", 0)

					result = Math.Max(LOLONr, LOLLonr) + 1
					result = Math.Max(offset + 1, result.Value)
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
		''' Finds a new LMNr.
		''' </summary>
		''' <returns>New LONr or nothing  in error case.</returns>
		Function FindNewLMNr() As Integer? Implements IPayrollDatabaseAccess.FindNewLMNr

			Dim sql As String

			sql = "Select Top 1 [LMNr] From LM Order By [LMNr] DESC"

			Dim lmNr As Integer? = ExecuteScalar(sql, Nothing)

			If lmNr.HasValue Then
				lmNr = lmNr + 1
			Else
				lmNr = 1
			End If

			Return lmNr
		End Function

		''' <summary>
		''' Loads MD_KK_LMV data.
		''' </summary>
		''' <param name="year">The year.</param>
		''' <param name="gavNr">The gav number.</param>
		''' <param name="err">The error flag.</param>
		''' <returns>The MD_KK_LMV or nothing if not found. If an error happens the error parameter will be true.</returns>
		Function LoadMD_KK_LMV_Data(ByVal mdnr As Integer, ByVal year As Integer, ByVal gavNr As Integer, ByRef err As Boolean) As MD_KK_LMV_Data Implements IPayrollDatabaseAccess.LoadMD_KK_LMV_Data

			err = False
			Dim result As MD_KK_LMV_Data = Nothing

			Dim sql As String = String.Empty

			sql = sql & "Select "
			sql = sql & "KK_AN_MA_Proz,"
			sql = sql & "KK_AG_MA_Proz,"
			sql = sql & "KK_AN_MZ_Proz,"
			sql = sql & "KK_AG_MZ_Proz,"
			sql = sql & "KK_AN_WA_Proz,"
			sql = sql & "KK_AG_WA_Proz,"
			sql = sql & "KK_AN_WZ_Proz,"
			sql = sql & "KK_AG_WZ_Proz,"
			sql = sql & "KK_AN_MA_Proz_72,"
			sql = sql & "KK_AG_MA_Proz_72,"
			sql = sql & "KK_AN_MZ_Proz_72,"
			sql = sql & "KK_AG_MZ_Proz_72,"
			sql = sql & "KK_AN_WA_Proz_72,"
			sql = sql & "KK_AG_WA_Proz_72,"
			sql = sql & "KK_AN_WZ_Proz_72,"
			sql = sql & "KK_AG_WZ_Proz_72 "
			sql = sql & "FROM MD_KK_LMV "
			sql = sql & "WHERE MDYear = @year And "
			sql = sql & "GAVNumber = @gavNr And "
			sql = sql & "MDNr = @mdNr And "
			sql = sql & "(KK_AN_MA_Proz + KK_AG_MA_Proz + KK_AN_WA_Proz + KK_AG_WA_Proz + "
			sql = sql & "KK_AN_MA_Proz_72 + KK_AG_MA_Proz_72 + KK_AN_WA_Proz_72 + KK_AG_WA_Proz_72 + "
			sql = sql & "KK_AN_MZ_Proz + KK_AG_MZ_Proz + KK_AN_WZ_Proz + KK_AG_WZ_Proz + "
			sql = sql & "KK_AN_MZ_Proz_72 + KK_AG_MZ_Proz_72 + KK_AN_WZ_Proz_72 + KK_AG_WZ_Proz_72"
			sql = sql & ") <> 0 "

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdnr))
			listOfParams.Add(New SqlClient.SqlParameter("year", year))
			listOfParams.Add(New SqlClient.SqlParameter("gavNr", gavNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = New MD_KK_LMV_Data

					result.KK_AN_MA_Proz = SafeGetDecimal(reader, "KK_AN_MA_Proz", 0)
					result.KK_AG_MA_Proz = SafeGetDecimal(reader, "KK_AG_MA_Proz", 0)
					result.KK_AN_MZ_Proz = SafeGetDecimal(reader, "KK_AN_MZ_Proz", 0)
					result.KK_AG_MZ_Proz = SafeGetDecimal(reader, "KK_AG_MZ_Proz", 0)
					result.KK_AN_WA_Proz = SafeGetDecimal(reader, "KK_AN_WA_Proz", 0)
					result.KK_AG_WA_Proz = SafeGetDecimal(reader, "KK_AG_WA_Proz", 0)
					result.KK_AN_WZ_Proz = SafeGetDecimal(reader, "KK_AN_WZ_Proz", 0)
					result.KK_AG_WZ_Proz = SafeGetDecimal(reader, "KK_AG_WZ_Proz", 0)
					result.KK_AN_MA_Proz_72 = SafeGetDecimal(reader, "KK_AN_MA_Proz_72", 0)
					result.KK_AG_MA_Proz_72 = SafeGetDecimal(reader, "KK_AG_MA_Proz_72", 0)
					result.KK_AN_MZ_Proz_72 = SafeGetDecimal(reader, "KK_AN_MZ_Proz_72", 0)
					result.KK_AG_MZ_Proz_72 = SafeGetDecimal(reader, "KK_AG_MZ_Proz_72", 0)
					result.KK_AN_WA_Proz_72 = SafeGetDecimal(reader, "KK_AN_WA_Proz_72", 0)
					result.KK_AG_WA_Proz_72 = SafeGetDecimal(reader, "KK_AG_WA_Proz_72", 0)
					result.KK_AN_WZ_Proz_72 = SafeGetDecimal(reader, "KK_AN_WZ_Proz_72", 0)
					result.KK_AG_WZ_Proz_72 = SafeGetDecimal(reader, "KK_AG_WZ_Proz_72", 0)

				End If

			Catch e As Exception
				err = True
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		''' <summary>
		''' Loads employee RPL records for LO creation.
		''' </summary>
		''' <returns>List of employee RPL data for LO creation or nothing in error case.</returns>
		Function LoadEmployeeRPLDataForLOCreation(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal month As Integer, ByVal year As Integer) As IEnumerable(Of EmployeeRPLDataForLOCreation) Implements IPayrollDatabaseAccess.LoadEmployeeRPLDataForLOCreation

			Dim result As List(Of EmployeeRPLDataForLOCreation) = Nothing

			Dim sql As String = "[Get Employee RPL Data For LO Creation]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MDYear", year))
			listOfParams.Add(New SqlClient.SqlParameter("@MDLp", month))
			listOfParams.Add(New SqlClient.SqlParameter("@iMANr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeRPLDataForLOCreation)

					While reader.Read

						Dim data = New EmployeeRPLDataForLOCreation
						data.MANr = SafeGetInteger(reader, "MANr", 0)
						data.Name = SafeGetString(reader, "MAName", String.Empty)
						data.Q_Steuer = SafeGetString(reader, "Q_Steuer", String.Empty)
						data.Kinder = SafeGetShort(reader, "Kinder", 0)
						data.Zahlart = SafeGetString(reader, "Zahlart", String.Empty)
						data.NOLO = SafeGetBoolean(reader, "NOLO", False)
						data.ESNr = SafeGetInteger(reader, "ESNr", 0)
						data.KDNr = SafeGetInteger(reader, "KDNr", 0)
						data.RPNR = SafeGetInteger(reader, "RPNR", 0)
						data.RPLNR = SafeGetInteger(reader, "RPLNR", 0)
						data.LANR = SafeGetDecimal(reader, "LANR", 0)
						data.KompBetrag = SafeGetDecimal(reader, "KompBetrag", 0)
						data.KompStd = SafeGetDecimal(reader, "KompStd", 0)
						data.M_Anzahl = SafeGetDecimal(reader, "M_Anzahl", 0)
						data.M_Basis = SafeGetDecimal(reader, "M_Basis", 0)
						data.M_Ansatz = SafeGetDecimal(reader, "M_Ansatz", 0)
						data.M_Betrag = SafeGetDecimal(reader, "M_Betrag", 0)
						data.SUVA = SafeGetString(reader, "SUVA", String.Empty)
						data.BVGStd = SafeGetDouble(reader, "BVGStd", 0)
						data.VonDate = SafeGetDateTime(reader, "VonDate", Nothing)
						data.BisDate = SafeGetDateTime(reader, "BisDate", Nothing)
						data.RPZusatzText = SafeGetString(reader, "RPZusatzText", String.Empty)
						data.LONr = SafeGetInteger(reader, "LONr", 0)
						data.RPKst1 = SafeGetString(reader, "RPKst1", String.Empty)
						data.RPKst2 = SafeGetString(reader, "RPKst2", String.Empty)
						data.RPKst = SafeGetString(reader, "RPKst", String.Empty)
						data.Far_pflicht = SafeGetBoolean(reader, "Far-pflicht", False)
						data.ES_Einstufung = SafeGetString(reader, "ES_Einstufung", String.Empty)
						data.KDBranche = SafeGetString(reader, "KDBranche", String.Empty)
						data.RPGAV_Nr = SafeGetInteger(reader, "RPGAV_Nr", 0)
						data.RPGAV_Kanton = SafeGetString(reader, "RPGAV_Kanton", String.Empty)
						data.RPGAV_Beruf = SafeGetString(reader, "RPGAV_Beruf", String.Empty)
						data.RPGAV_Gruppe1 = SafeGetString(reader, "RPGAV_Gruppe1", String.Empty)
						data.RPGAV_Gruppe2 = SafeGetString(reader, "RPGAV_Gruppe2", String.Empty)
						data.RPGAV_Gruppe3 = SafeGetString(reader, "RPGAV_Gruppe3", String.Empty)
						data.RPGAV_Text = SafeGetString(reader, "RPGAV_Text", String.Empty)
						data.RPGAV_FAN = SafeGetDecimal(reader, "RPGAV_FAN", 0)
						data.RPGAV_FAG = SafeGetDecimal(reader, "RPGAV_FAG", 0)
						data.RPGAV_WAN = SafeGetDecimal(reader, "RPGAV_WAN", 0)
						data.RPGAV_WAG = SafeGetDecimal(reader, "RPGAV_WAG", 0)
						data.RPGAV_VAN = SafeGetDecimal(reader, "RPGAV_VAN", 0)
						data.RPGAV_VAG = SafeGetDecimal(reader, "RPGAV_VAG", 0)
						data.RPGAV_WAN_S = SafeGetDecimal(reader, "RPGAV_WAN_S", 0)
						data.RPGAV_WAG_S = SafeGetDecimal(reader, "RPGAV_WAG_S", 0)
						data.RPGAV_VAN_S = SafeGetDecimal(reader, "RPGAV_VAN_S", 0)
						data.RPGAV_VAG_S = SafeGetDecimal(reader, "RPGAV_VAG_S", 0)
						data.RPGAV_WAN_M = SafeGetDecimal(reader, "RPGAV_WAN_M", 0)
						data.RPGAV_WAG_M = SafeGetDecimal(reader, "RPGAV_WAG_M", 0)
						data.RPGAV_VAN_M = SafeGetDecimal(reader, "RPGAV_VAN_M", 0)
						data.RPGAV_VAG_M = SafeGetDecimal(reader, "RPGAV_VAG_M", 0)
						data.RPGAV_StdMonth = SafeGetDecimal(reader, "RPGAV_StdMonth", 0)
						data.RPGAV_StdYear = SafeGetDecimal(reader, "RPGAV_StdYear", 0)
						data.RPText = SafeGetString(reader, "RPText", String.Empty)

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
		''' Loads employee LM records for LO creation.
		''' </summary>
		''' <returns>List of employee LM data for LO creation or nothing in error case.</returns>
		Function LoadEmployeeLMDataForLOCreation(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal month As Integer, ByVal year As Integer) As IEnumerable(Of EmployeeLMDataForLOCreation) Implements IPayrollDatabaseAccess.LoadEmployeeLMDataForLOCreation

			Dim result As List(Of EmployeeLMDataForLOCreation) = Nothing

			Dim sql As String = "[Get Employee LM Data For LO Creation]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MDYear", year))
			listOfParams.Add(New SqlClient.SqlParameter("@MDLp", month))
			listOfParams.Add(New SqlClient.SqlParameter("@iMANr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeLMDataForLOCreation)

					While reader.Read

						Dim data = New EmployeeLMDataForLOCreation

						data.MANr = SafeGetInteger(reader, "MANr", 0)
						data.MAName = SafeGetString(reader, "MAName", String.Empty)
						data.Strasse = SafeGetString(reader, "Strasse", String.Empty)
						data.Geschlecht = SafeGetString(reader, "Geschlecht", String.Empty)
						data.AHV_Nr = SafeGetString(reader, "AHV_Nr", String.Empty)
						data.S_Kanton = SafeGetString(reader, "S_Kanton", String.Empty)
						data.Q_Steuer = SafeGetString(reader, "Q_Steuer", String.Empty)
						data.Kirchensteuer = SafeGetString(reader, "Kirchensteuer", String.Empty)
						data.Ansaessigkeit = SafeGetBoolean(reader, "Ansaessigkeit", False)
						data.Kinder = SafeGetShort(reader, "Kinder", 0)
						data.Zahlart = SafeGetString(reader, "Zahlart", String.Empty)
						data.AHVCode = SafeGetString(reader, "AHVCode", String.Empty)
						data.ALVCode = SafeGetString(reader, "ALVCode", String.Empty)
						data.KTGPflicht = SafeGetBoolean(reader, "KTGPflicht", False)
						data.NOLO = SafeGetBoolean(reader, "NOLO", False)
						data.LMNr = SafeGetInteger(reader, "LMNr", 0)
						data.ESNr = SafeGetInteger(reader, "ESNr", 0)
						data.KDNr = SafeGetInteger(reader, "KDNr", 0)
						data.LAName = SafeGetString(reader, "LAName", String.Empty)
						data.LMID = SafeGetInteger(reader, "LMID", 0)
						data.LANR = SafeGetDecimal(reader, "LANR", 0)
						data.M_Anz = SafeGetDecimal(reader, "M_Anz", 0)
						data.M_Bas = SafeGetDecimal(reader, "M_Bas", 0)
						data.M_Ans = SafeGetDecimal(reader, "M_Ans", 0)
						data.M_Btr = SafeGetDecimal(reader, "M_Btr", 0)
						data.SUVA = SafeGetString(reader, "SUVA", String.Empty)
						data.LMKst1 = SafeGetString(reader, "LMKst1", String.Empty)
						data.LMKst2 = SafeGetString(reader, "LMKst2", String.Empty)
						data.Kst = SafeGetString(reader, "Kst", String.Empty)
						data.LMWithDTA = SafeGetBoolean(reader, "LMWithDTA", False)
						data.ZGGrund = SafeGetString(reader, "ZGGrund", String.Empty)
						data.BnkNr = SafeGetInteger(reader, "BnkNr", 0)
						data.Far_pflicht = SafeGetBoolean(reader, "Far-pflicht", False)
						data.LAIndBez = SafeGetString(reader, "LAIndBez", String.Empty)
						data.Kanton = SafeGetString(reader, "Kanton", String.Empty)
						data.ESEinstufung = SafeGetString(reader, "ESEinstufung", String.Empty)
						data.ESBranche = SafeGetString(reader, "ESBranche", String.Empty)
						data.GAVNr = SafeGetInteger(reader, "GAVNr", 0)
						data.GAVKanton = SafeGetString(reader, "GAVKanton", String.Empty)
						data.GAVGruppe0 = SafeGetString(reader, "GAVGruppe0", String.Empty)
						data.GAVGruppe1 = SafeGetString(reader, "GAVGruppe1", String.Empty)
						data.GAVGruppe2 = SafeGetString(reader, "GAVGruppe2", String.Empty)
						data.GAVGruppe3 = SafeGetString(reader, "GAVGruppe3", String.Empty)
						data.GAVBezeichnung = SafeGetString(reader, "GAVBezeichnung", String.Empty)
						data.GAV_FAG = SafeGetDecimal(reader, "GAV_FAG", 0)
						data.GAV_FAN = SafeGetDecimal(reader, "GAV_FAN", 0)
						data.GAV_WAG = SafeGetDecimal(reader, "GAV_WAG", 0)
						data.GAV_WAN = SafeGetDecimal(reader, "GAV_WAN", 0)
						data.GAV_VAG = SafeGetDecimal(reader, "GAV_VAG", 0)
						data.GAV_VAN = SafeGetDecimal(reader, "GAV_VAN", 0)
						data.GAV_FAG_S = SafeGetDecimal(reader, "GAV_FAG_S", 0)
						data.GAV_FAN_S = SafeGetDecimal(reader, "GAV_FAN_S", 0)
						data.GAV_WAG_S = SafeGetDecimal(reader, "GAV_WAG_S", 0)
						data.GAV_WAN_S = SafeGetDecimal(reader, "GAV_WAN_S", 0)
						data.GAV_VAG_S = SafeGetDecimal(reader, "GAV_VAG_S", 0)
						data.GAV_VAN_S = SafeGetDecimal(reader, "GAV_VAN_S", 0)
						data.GAV_FAG_M = SafeGetDecimal(reader, "GAV_FAG_M", 0)
						data.GAV_FAN_M = SafeGetDecimal(reader, "GAV_FAN_M", 0)
						data.GAV_WAG_M = SafeGetDecimal(reader, "GAV_WAG_M", 0)
						data.GAV_WAN_M = SafeGetDecimal(reader, "GAV_WAN_M", 0)
						data.GAV_VAG_M = SafeGetDecimal(reader, "GAV_VAG_M", 0)
						data.GAV_VAN_M = SafeGetDecimal(reader, "GAV_VAN_M", 0)

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
		''' Loads employee LM records for LO creation.
		''' </summary>
		''' <returns>List of employee LM data for LO creation or nothing in error case.</returns>
		Function LoadEmployeeSozialleistungpflichtigLMDataForLOCreation(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal month As Integer, ByVal year As Integer) As IEnumerable(Of EmployeeLMDataForLOCreation) Implements IPayrollDatabaseAccess.LoadEmployeeSozialleistungpflichtigLMDataForLOCreation

			Dim result As List(Of EmployeeLMDataForLOCreation) = Nothing

			Dim sql As String = "[Get Employee LM Data Sozialleistungspflichtig For LO Creation]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MDYear", year))
			listOfParams.Add(New SqlClient.SqlParameter("@MDLp", month))
			listOfParams.Add(New SqlClient.SqlParameter("@iMANr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeLMDataForLOCreation)

					While reader.Read

						Dim data = New EmployeeLMDataForLOCreation

						data.MANr = SafeGetInteger(reader, "MANr", 0)
						data.MAName = SafeGetString(reader, "MAName", String.Empty)
						data.Strasse = SafeGetString(reader, "Strasse", String.Empty)
						data.Geschlecht = SafeGetString(reader, "Geschlecht", String.Empty)
						data.AHV_Nr = SafeGetString(reader, "AHV_Nr", String.Empty)
						data.S_Kanton = SafeGetString(reader, "S_Kanton", String.Empty)
						data.Q_Steuer = SafeGetString(reader, "Q_Steuer", String.Empty)
						data.Kirchensteuer = SafeGetString(reader, "Kirchensteuer", String.Empty)
						data.Ansaessigkeit = SafeGetBoolean(reader, "Ansaessigkeit", False)
						data.Kinder = SafeGetShort(reader, "Kinder", 0)
						data.Zahlart = SafeGetString(reader, "Zahlart", String.Empty)
						data.AHVCode = SafeGetString(reader, "AHVCode", String.Empty)
						data.ALVCode = SafeGetString(reader, "ALVCode", String.Empty)
						data.KTGPflicht = SafeGetBoolean(reader, "KTGPflicht", False)
						data.NOLO = SafeGetBoolean(reader, "NOLO", False)
						data.LMNr = SafeGetInteger(reader, "LMNr", 0)
						data.ESNr = SafeGetInteger(reader, "ESNr", 0)
						data.KDNr = SafeGetInteger(reader, "KDNr", 0)
						data.LAName = SafeGetString(reader, "LAName", String.Empty)
						data.LMID = SafeGetInteger(reader, "LMID", 0)
						data.LANR = SafeGetDecimal(reader, "LANR", 0)
						data.M_Anz = SafeGetDecimal(reader, "M_Anz", 0)
						data.M_Bas = SafeGetDecimal(reader, "M_Bas", 0)
						data.M_Ans = SafeGetDecimal(reader, "M_Ans", 0)
						data.M_Btr = SafeGetDecimal(reader, "M_Btr", 0)
						data.SUVA = SafeGetString(reader, "SUVA", String.Empty)
						data.LMKst1 = SafeGetString(reader, "LMKst1", String.Empty)
						data.LMKst2 = SafeGetString(reader, "LMKst2", String.Empty)
						data.Kst = SafeGetString(reader, "Kst", String.Empty)
						data.LMWithDTA = SafeGetBoolean(reader, "LMWithDTA", False)
						data.ZGGrund = SafeGetString(reader, "ZGGrund", String.Empty)
						data.BnkNr = SafeGetInteger(reader, "BnkNr", 0)
						data.Far_pflicht = SafeGetBoolean(reader, "Far-pflicht", False)
						data.LAIndBez = SafeGetString(reader, "LAIndBez", String.Empty)
						data.Kanton = SafeGetString(reader, "Kanton", String.Empty)
						data.ESEinstufung = SafeGetString(reader, "ESEinstufung", String.Empty)
						data.ESBranche = SafeGetString(reader, "ESBranche", String.Empty)
						data.GAVNr = SafeGetInteger(reader, "GAVNr", 0)
						data.GAVKanton = SafeGetString(reader, "GAVKanton", String.Empty)
						data.GAVGruppe0 = SafeGetString(reader, "GAVGruppe0", String.Empty)
						data.GAVGruppe1 = SafeGetString(reader, "GAVGruppe1", String.Empty)
						data.GAVGruppe2 = SafeGetString(reader, "GAVGruppe2", String.Empty)
						data.GAVGruppe3 = SafeGetString(reader, "GAVGruppe3", String.Empty)
						data.GAVBezeichnung = SafeGetString(reader, "GAVBezeichnung", String.Empty)
						data.GAV_FAG = SafeGetDecimal(reader, "GAV_FAG", 0)
						data.GAV_FAN = SafeGetDecimal(reader, "GAV_FAN", 0)
						data.GAV_WAG = SafeGetDecimal(reader, "GAV_WAG", 0)
						data.GAV_WAN = SafeGetDecimal(reader, "GAV_WAN", 0)
						data.GAV_VAG = SafeGetDecimal(reader, "GAV_VAG", 0)
						data.GAV_VAN = SafeGetDecimal(reader, "GAV_VAN", 0)
						data.GAV_FAG_S = SafeGetDecimal(reader, "GAV_FAG_S", 0)
						data.GAV_FAN_S = SafeGetDecimal(reader, "GAV_FAN_S", 0)
						data.GAV_WAG_S = SafeGetDecimal(reader, "GAV_WAG_S", 0)
						data.GAV_WAN_S = SafeGetDecimal(reader, "GAV_WAN_S", 0)
						data.GAV_VAG_S = SafeGetDecimal(reader, "GAV_VAG_S", 0)
						data.GAV_VAN_S = SafeGetDecimal(reader, "GAV_VAN_S", 0)
						data.GAV_FAG_M = SafeGetDecimal(reader, "GAV_FAG_M", 0)
						data.GAV_FAN_M = SafeGetDecimal(reader, "GAV_FAN_M", 0)
						data.GAV_WAG_M = SafeGetDecimal(reader, "GAV_WAG_M", 0)
						data.GAV_WAN_M = SafeGetDecimal(reader, "GAV_WAN_M", 0)
						data.GAV_VAG_M = SafeGetDecimal(reader, "GAV_VAG_M", 0)
						data.GAV_VAN_M = SafeGetDecimal(reader, "GAV_VAN_M", 0)

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
		''' Loads employee ZG records for LO creation.
		''' </summary>
		''' <returns>List of employee ZG data for LO creation or nothing in error case.</returns>
		Function LoadEmployeeZGDataForLOCreation(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal month As Integer, ByVal year As Integer) As IEnumerable(Of EmployeeZGDataForPayroll) Implements IPayrollDatabaseAccess.LoadEmployeeZGDataForLOCreation

			Dim result As List(Of EmployeeZGDataForPayroll) = Nothing

			Dim sql As String = "[Get Employee ZG Data For LO Creation]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MDYear", year))
			listOfParams.Add(New SqlClient.SqlParameter("@MDLp", month))
			listOfParams.Add(New SqlClient.SqlParameter("@iMANr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeZGDataForPayroll)

					While reader.Read

						Dim data = New EmployeeZGDataForPayroll

						data.MANr = SafeGetInteger(reader, "MANr", 0)
						data.MAName = SafeGetString(reader, "MAName", String.Empty)
						data.Strasse = SafeGetString(reader, "Strasse", String.Empty)
						data.Geschlecht = SafeGetString(reader, "Geschlecht", String.Empty)
						data.AHV_Nr = SafeGetString(reader, "AHV_Nr", String.Empty)
						data.Gebdat = SafeGetDateTime(reader, "Gebdat", Nothing)
						data.S_Kanton = SafeGetString(reader, "S_Kanton", String.Empty)
						data.Q_Steuer = SafeGetString(reader, "Q_Steuer", String.Empty)
						data.Kirchensteuer = SafeGetString(reader, "Kirchensteuer", String.Empty)
						data.Ansaessigkeit = SafeGetBoolean(reader, "Ansaessigkeit", False)
						data.Kinder = SafeGetShort(reader, "Kinder", 0)
						data.Zahlart = SafeGetString(reader, "Zahlart", String.Empty)
						data.NOLO = SafeGetBoolean(reader, "NOLO", False)
						data.AHVCode = SafeGetString(reader, "AHVCode", String.Empty)
						data.ALVCode = SafeGetString(reader, "ALVCode", String.Empty)
						data.KTGPflicht = SafeGetBoolean(reader, "KTGPflicht", False)
						data.RPNR = SafeGetInteger(reader, "RPNR", 0)
						data.ZGNr = SafeGetInteger(reader, "ZGNr", 0)
						data.LANR = SafeGetInteger(reader, "LANR", 0)
						data.Betrag = SafeGetDecimal(reader, "Betrag", 0)
						data.LONr = SafeGetInteger(reader, "LONr", 0)
						data.Jahr = SafeGetString(reader, "Jahr", String.Empty)
						data.ZGGRUND = SafeGetString(reader, "ZGGRUND", String.Empty)
						data.Basis = SafeGetDecimal(reader, "Basis", 0)
						data.Ansatz = SafeGetInteger(reader, "Ansatz", 0)
						data.Anzahl = SafeGetInteger(reader, "Anzahl", 0)
						data.GebAbzug = SafeGetBoolean(reader, "GebAbzug", False)
						data.Aus_Dat = SafeGetDateTime(reader, "Aus_Dat", Nothing)
						data.BnkAu = SafeGetBoolean(reader, "BnkAu", False)
						data.CheckNumber = SafeGetString(reader, "CheckNumber", String.Empty)

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
		''' Loads LA data.
		''' </summary>
		''' <param name="year">The year.</param>
		''' <param name="verwendung">Verwendung.</param>
		''' <param name="laNr">Optional filter LANr.</param>
		''' <returns>List of LA data or nothing in error case.</returns>
		Function LoadLAData(ByVal year As Integer, ByVal verwendung() As Integer, Optional laNr As Decimal? = Nothing) As IEnumerable(Of LAData) Implements IPayrollDatabaseAccess.LoadLAData

			Dim result As List(Of LAData) = Nothing

			Dim sql As String = String.Empty

			sql = "Select [LANr], [LALoText], [Bedingung], [Vorzeichen], [Verwendung], "
			sql = sql & "[Sum0Anzahl], [Sum1Anzahl], [Sum0Basis], [Sum1Basis], [Sum2Basis], "
			sql = sql & "[SumAnsatz], [Sum0Betrag], [Sum1Betrag], [Sum2Betrag], [Sum3Betrag], "
			sql = sql & "[BruttoPflichtig], [AHVPflichtig], [ALVPflichtig], [NBUVPflichtig], "
			sql = sql & "[UVPflichtig], [BVGPflichtig], [KKPflichtig], [QSTPflichtig], "
			sql = sql & "[Reserve1], [Reserve2], [Reserve3], [Reserve4], [Reserve5], "
			sql = sql & "[FerienInklusiv], [FeierInklusiv], [13Inklusiv], [ProTag], [RunFuncBefore], [GleitTime], "
			sql = sql & "[KumLANr], [AGLA], [Kumulativ], [KumulativMonth], [TypeAnzahl], [FixAnzahl], [MAAnzVar], "
			sql = sql & "[TypeBasis], [FixBasis], [MABasVar], [TypeAnsatz], [FixAnsatz], [MAAnsVar], "
			sql = sql & "[Rundung], [WarningByZero], [ByNullCreate], [GroupKey] "
			sql = sql & "From LA Where "
			sql = sql & "LAJahr = @year "
			sql = sql & "And LADeactivated = 0 "

			If Not verwendung Is Nothing AndAlso
			 Not verwendung.Count = 0 Then
				sql = sql & "And [Verwendung] In ("

				For i As Integer = 0 To verwendung.Length - 1
					sql = sql & String.Format("'{0}'", verwendung(i))

					If Not i = verwendung.Length - 1 Then
						sql = sql & ", "
					End If

				Next
				sql = sql & ") "
			End If

			If laNr.HasValue Then
				sql = sql & "And LANr = @laNr "
			End If

			sql = sql & "Order By [LANr] ASC"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@year", Convert.ToString(year)))
			If laNr.HasValue Then
				listOfParams.Add(New SqlClient.SqlParameter("@laNr", laNr.Value))
			End If

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of LAData)

					While reader.Read

						Dim data = New LAData
						data.LANr = SafeGetDecimal(reader, "LANr", 0)
						data.LALoText = SafeGetString(reader, "LALoText", String.Empty)
						data.Bedingung = SafeGetString(reader, "Bedingung", String.Empty)
						data.Vorzeichen = SafeGetString(reader, "Vorzeichen", String.Empty)
						data.Verwendung = SafeGetString(reader, "Verwendung", String.Empty)
						data.Sum0Anzahl = SafeGetShort(reader, "Sum0Anzahl", 0)
						data.Sum1Anzahl = SafeGetShort(reader, "Sum1Anzahl", 0)
						data.Sum0Basis = SafeGetShort(reader, "Sum0Basis", 0)
						data.Sum1Basis = SafeGetShort(reader, "Sum1Basis", 0)
						data.Sum2Basis = SafeGetShort(reader, "Sum2Basis", 0)
						data.SumAnsatz = SafeGetShort(reader, "SumAnsatz", 0)
						data.Sum0Betrag = SafeGetShort(reader, "Sum0Betrag", 0)
						data.Sum1Betrag = SafeGetShort(reader, "Sum1Betrag", 0)
						data.Sum2Betrag = SafeGetShort(reader, "Sum2Betrag", 0)
						data.Sum3Betrag = SafeGetShort(reader, "Sum3Betrag", 0)
						data.BruttoPflichtig = SafeGetBoolean(reader, "BruttoPflichtig", False)
						data.AHVPflichtig = SafeGetBoolean(reader, "AHVPflichtig", False)
						data.ALVPflichtig = SafeGetBoolean(reader, "ALVPflichtig", False)
						data.NBUVPflichtig = SafeGetBoolean(reader, "NBUVPflichtig", False)
						data.UVPflichtig = SafeGetBoolean(reader, "UVPflichtig", False)
						data.BVGPflichtig = SafeGetBoolean(reader, "BVGPflichtig", False)
						data.KKPflichtig = SafeGetBoolean(reader, "KKPflichtig", False)
						data.QSTPflichtig = SafeGetBoolean(reader, "QSTPflichtig", False)
						data.Reserve1 = SafeGetBoolean(reader, "Reserve1", False)
						data.Reserve2 = SafeGetBoolean(reader, "Reserve2", False)
						data.Reserve3 = SafeGetBoolean(reader, "Reserve3", False)
						data.Reserve4 = SafeGetBoolean(reader, "Reserve4", False)
						data.Reserve5 = SafeGetBoolean(reader, "Reserve5", False)
						data.FerienInklusiv = SafeGetBoolean(reader, "FerienInklusiv", False)
						data.FeierInklusiv = SafeGetBoolean(reader, "FeierInklusiv", False)
						data._13Inklusiv = SafeGetBoolean(reader, "13Inklusiv", False)
						data.ProTag = SafeGetBoolean(reader, "ProTag", False)
						data.RunFuncBefore = SafeGetString(reader, "RunFuncBefore", String.Empty)
						data.GleitTime = SafeGetBoolean(reader, "GleitTime", False)
						data.KumLANr = SafeGetInteger(reader, "KumLANr", 0)
						data.AGLA = SafeGetBoolean(reader, "AGLA", False)
						data.Kumulativ = SafeGetBoolean(reader, "Kumulativ", False)
						data.KumulativMonth = SafeGetBoolean(reader, "KumulativMonth", False)
						data.TypeAnzahl = SafeGetShort(reader, "TypeAnzahl", 0)
						data.FixAnzahl = SafeGetDecimal(reader, "FixAnzahl", 0)
						data.MAAnzVar = SafeGetString(reader, "MAAnzVar", String.Empty)
						data.TypeBasis = SafeGetShort(reader, "TypeBasis", 0)
						data.FixBasis = SafeGetDecimal(reader, "FixBasis", 0)
						data.MABasVar = SafeGetString(reader, "MABasVar", String.Empty)
						data.TypeAnsatz = SafeGetShort(reader, "TypeAnsatz", 0)
						data.FixAnsatz = SafeGetDecimal(reader, "FixAnsatz", 0)
						data.MAAnsVar = SafeGetString(reader, "MAAnsVar", String.Empty)
						data.Rundung = SafeGetShort(reader, "Rundung", 0)
						data.WarningByZero = SafeGetBoolean(reader, "WarningByZero", False)
						data.ByNullCreate = SafeGetBoolean(reader, "ByNullCreate", False)
						data.GroupKey = SafeGetDecimal(reader, "GroupKey", 0)

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
		''' Loads MD_KiAu data.
		''' </summary>
		''' <param name="canton">The canton.</param>
		''' <param name="year">The year.</param>
		''' <param name="err">The error flag.</param>
		''' <returns>The MD_KiAu or nothing if not found. If an error happens the error parameter will be true.</returns>
		Function LoadMD_KiAuDataData(ByVal mdNr As Integer, ByVal canton As String, ByVal year As Integer, ByRef err As Boolean) As MD_KiAuData Implements IPayrollDatabaseAccess.LoadMD_KiAuDataData

			err = False
			Dim result As MD_KiAuData = Nothing

			Dim sql As String = String.Empty

			sql = sql & "Select [ID] "
			sql = sql & ",[RecNr]"
			sql = sql & ",[MDNr]"
			sql = sql & ",[MDYear]"
			sql = sql & ",[Fak_Kanton]"
			sql = sql & ",[Ki1_FakMax]"
			sql = sql & ",[Ki2_FakMax]"
			sql = sql & ",[Ki1_Std]"
			sql = sql & ",[Ki2_Std]"
			sql = sql & ",[Ki1_Day]"
			sql = sql & ",[Ki2_Day]"
			sql = sql & ",[Ki1_Month]"
			sql = sql & ",[Ki2_Month]"
			sql = sql & ",[ChangeKiIn]"
			sql = sql & ",[Au1_Std]"
			sql = sql & ",[Au2_Std]"
			sql = sql & ",[Au1_Day]"
			sql = sql & ",[Au2_Day]"
			sql = sql & ",[Au1_Month]"
			sql = sql & ",[ChangeAuIn]"
			sql = sql & ",[CreatedOn]"
			sql = sql & ",[CreatedFrom]"
			sql = sql & ",[ChangedOn]"
			sql = sql & ",[ChangedFrom]"
			sql = sql & ",[Fak_Name]"
			sql = sql & ",[Fak_ZHD]"
			sql = sql & ",[Fak_Postfach]"
			sql = sql & ",[Fak_Strasse]"
			sql = sql & ",[Fak_PLZOrt]"
			sql = sql & ",[Fak_MNr]"
			sql = sql & ",[Fak_KNr]"
			sql = sql & ",[ChangeAuIn_2]"
			sql = sql & ",[ChangeKiIn_2]"
			sql = sql & ",[YMinLohn]"
			sql = sql & ",[Bemerkung_1]"
			sql = sql & ",[Bemerkung_2]"
			sql = sql & ",[Bemerkung_3]"
			sql = sql & ",[Bemerkung_4]"
			sql = sql & ",[Geb_Zulage]"
			sql = sql & ",[Ado_Zulage]"
			sql = sql & ",[Fak_Proz]"
			sql = sql & ",[USNr] "
			sql = sql & ",[AtEndBeginES]"
			sql = sql & ",[SeeAHVLohnForYear]"
			sql = sql & "FROM [dbo].[MD_KiAu] "
			sql = sql & "WHERE Fak_Kanton = @canton AND "
			sql = sql & "MDNr = @mdNr AND "
			sql = sql & "MDYear = @mdYear "

			sql &= " Order By ID Desc"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("canton", canton))
			listOfParams.Add(New SqlClient.SqlParameter("mdYear", year))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = New MD_KiAuData

					result.ID = SafeGetInteger(reader, "ID", 0)
					result.RecNr = SafeGetInteger(reader, "RecNr", 0)
					result.MDNr = SafeGetInteger(reader, "MDNr", 0)
					result.MDYear = SafeGetString(reader, "MDYear", String.Empty)
					result.Fak_Kanton = SafeGetString(reader, "Fak_Kanton", String.Empty)
					result.Ki1_FakMax = SafeGetDecimal(reader, "Ki1_FakMax", 0)
					result.Ki2_FakMax = SafeGetDecimal(reader, "Ki2_FakMax", 0)
					result.Ki1_Std = SafeGetDecimal(reader, "Ki1_Std", 0)
					result.Ki2_Std = SafeGetDecimal(reader, "Ki2_Std", 0)
					result.Ki1_Day = SafeGetDecimal(reader, "Ki1_Day", 0)
					result.Ki2_Day = SafeGetDecimal(reader, "Ki2_Day", 0)
					result.Ki1_Month = SafeGetDecimal(reader, "Ki1_Month", 0)
					result.Ki2_Month = SafeGetDecimal(reader, "Ki2_Month", 0)
					result.ChangeKiIn = SafeGetString(reader, "ChangeKiIn", String.Empty)
					result.Au1_Std = SafeGetDecimal(reader, "Au1_Std", 0)
					result.Au2_Std = SafeGetDecimal(reader, "Au2_Std", 0)
					result.Au1_Day = SafeGetDecimal(reader, "Au1_Day", 0)
					result.Au2_Day = SafeGetDecimal(reader, "Au2_Day", 0)
					result.Au1_Month = SafeGetDecimal(reader, "Au1_Month", 0)
					result.ChangeAuIn = SafeGetString(reader, "ChangeAuIn", String.Empty)
					result.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
					result.CreatedFrom = SafeGetString(reader, "CreatedFrom", String.Empty)
					result.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
					result.ChangedFrom = SafeGetString(reader, "ChangedFrom", String.Empty)
					result.Fak_Name = SafeGetString(reader, "Fak_Name", String.Empty)
					result.Fak_ZHD = SafeGetString(reader, "Fak_ZHD", String.Empty)
					result.Fak_Postfach = SafeGetString(reader, "Fak_Postfach", String.Empty)
					result.Fak_Strasse = SafeGetString(reader, "Fak_Strasse", String.Empty)
					result.Fak_PLZOrt = SafeGetString(reader, "Fak_PLZOrt", String.Empty)
					result.Fak_MNr = SafeGetString(reader, "Fak_MNr", String.Empty)
					result.Fak_KNr = SafeGetString(reader, "Fak_KNr", String.Empty)
					result.ChangeAuIn_2 = SafeGetString(reader, "ChangeAuIn_2", String.Empty)
					result.ChangeKiIn_2 = SafeGetString(reader, "ChangeKiIn_2", String.Empty)
					result.YMinLohn = SafeGetDecimal(reader, "YMinLohn", 0)
					result.Bemerkung_1 = SafeGetString(reader, "Bemerkung_1", String.Empty)
					result.Bemerkung_2 = SafeGetString(reader, "Bemerkung_2", String.Empty)
					result.Bemerkung_3 = SafeGetString(reader, "Bemerkung_3", String.Empty)
					result.Bemerkung_4 = SafeGetString(reader, "Bemerkung_4", String.Empty)
					result.Geb_Zulage = SafeGetDecimal(reader, "Geb_Zulage", 0)
					result.Ado_Zulage = SafeGetDecimal(reader, "Ado_Zulage", 0)
					result.Fak_Proz = SafeGetDecimal(reader, "Fak_Proz", 0)
					result.AtEndBeginES = SafeGetBoolean(reader, "AtEndBeginES", False)
					result.SeeAHVLohnForYear = SafeGetBoolean(reader, "SeeAHVLohnForYear", False)

				End If

			Catch e As Exception
				err = True
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result


		End Function

		''' <summary>
		''' Checks if a LOL exists.
		''' </summary>
		''' <param name="laNrs">The LANrs.</param>
		''' <param name="maNr">The MANr.</param>
		''' <param name="loNr">The LoNr.</param>
		''' <param name="mdNr">The MDNr.</param>
		''' <returns>Boolean flag indicating if LOL exits or nothing in error case.</returns>
		Function CheckIfLOLExists(ByVal laNrs As String, maNr As Integer, ByVal loNr As Integer, ByVal mdNr As Integer) As Boolean? Implements IPayrollDatabaseAccess.CheckIfLOLExists

			Dim doesExist = False

			Dim sql As String = String.Empty

			sql = sql & "SELECT COUNT(*) FROM LOL WHERE LANr In ({0}) AND MANr = @maNr AND LONr = @loNr AND mdNr = @mdNr"
			sql = String.Format(sql, laNrs)

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@maNr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@loNr", loNr))
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))

			Dim existingLOLRecordsCount = ExecuteScalar(sql, listOfParams)

			If Not existingLOLRecordsCount Is Nothing Then
				doesExist = (existingLOLRecordsCount > 0)

				Return doesExist
			Else
				Return Nothing
			End If

		End Function

		''' <summary>
		''' Loads suva data1 for LO creation.
		''' </summary>
		''' <param name="month">The month.</param>
		''' <param name="year">The year.</param>
		''' <param name="maNr">The emplyoee number.</param>
		''' <param name="mdNr">The mandant number.</param>
		''' <param name="err">The error flag.</param>
		''' <returns>Suva1 data.</returns>
		Function LoadSuvaData1ForLOCreation(ByVal month As Integer, ByVal year As String, ByVal maNr As Integer, ByVal mdNr As Integer, ByRef err As Boolean) As SuvaData1ForPayroll Implements IPayrollDatabaseAccess.LoadSuvaData1ForPayroll

			Dim result As SuvaData1ForPayroll = Nothing

			Dim sql As String = "[Get Suva Data1 For Payroll]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@LP", month))
			listOfParams.Add(New SqlClient.SqlParameter("@MANr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@MDYear", year))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = New SuvaData1ForPayroll

					result.SLK = SafeGetDecimal(reader, "SLK", 0)
					result.SKBZ = SafeGetDecimal(reader, "SKBZ", 0)
					result.S7835 = SafeGetDecimal(reader, "S7835", 0)
					result.SKBA = SafeGetDecimal(reader, "SKBA", 0)
					result.S7320 = SafeGetDecimal(reader, "S7320", 0)
					result.S7321 = SafeGetDecimal(reader, "S7321", 0)
					result.S7322 = SafeGetDecimal(reader, "S7322", 0)
					result.S7323 = SafeGetDecimal(reader, "S7323", 0)
					result.S7324 = SafeGetDecimal(reader, "S7324", 0)
					result.S7325 = SafeGetDecimal(reader, "S7325", 0)
					result.S7326 = SafeGetDecimal(reader, "S7326", 0)
					result.S7327 = SafeGetDecimal(reader, "S7327", 0)
					result.S7830 = SafeGetDecimal(reader, "S7830", 0)
					result.S7390 = SafeGetDecimal(reader, "S7390", 0)
					result.S7395 = SafeGetDecimal(reader, "S7395", 0)
					result.S7820 = SafeGetDecimal(reader, "S7820", 0)
					result.S7825 = SafeGetDecimal(reader, "S7825", 0)

				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				err = True
				result = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		''' <summary>
		''' Loads suva data2 for payroll.
		''' </summary>
		''' <param name="month">The month.</param>
		''' <param name="year">The year.</param>
		''' <param name="maNr">The emplyoee number.</param>
		''' <param name="mdNr">The mandant number.</param>
		''' <param name="err">The error flag.</param>
		''' <returns>Suva2 data.</returns>
		Function LoadSuvaData2ForPayroll(ByVal month As Integer, ByVal year As String, ByVal maNr As Integer, ByVal mdNr As Integer, ByRef err As Boolean) As SuvaData2ForPayroll Implements IPayrollDatabaseAccess.LoadSuvaData2ForPayroll

			Dim result As SuvaData2ForPayroll = Nothing

			Dim sql As String = "[Get Suva Data2 For Payroll]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@LP", month))
			listOfParams.Add(New SqlClient.SqlParameter("@MANr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@MDYear", year))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = New SuvaData2ForPayroll

					result.SLK = SafeGetDecimal(reader, "SLK", 0)
					result.SKBZ = SafeGetDecimal(reader, "SKBZ", 0)
					result.S7835 = SafeGetDecimal(reader, "S7835", 0)
					result.SKBA = SafeGetDecimal(reader, "SKBA", 0)
					result.S7320 = SafeGetDecimal(reader, "S7320", 0)
					result.S7321 = SafeGetDecimal(reader, "S7321", 0)
					result.S7322 = SafeGetDecimal(reader, "S7322", 0)
					result.S7323 = SafeGetDecimal(reader, "S7323", 0)
					result.S7324 = SafeGetDecimal(reader, "S7324", 0)
					result.S7325 = SafeGetDecimal(reader, "S7325", 0)
					result.S7326 = SafeGetDecimal(reader, "S7326", 0)
					result.S7327 = SafeGetDecimal(reader, "S7327", 0)
					result.S7830 = SafeGetDecimal(reader, "S7830", 0)
					result.S7389 = SafeGetDecimal(reader, "S7389", 0)
					result.S7390 = SafeGetDecimal(reader, "S7390", 0)
					result.S7394 = SafeGetDecimal(reader, "S7394", 0)
					result.S7395 = SafeGetDecimal(reader, "S7395", 0)
					result.S7819 = SafeGetDecimal(reader, "S7819", 0)
					result.S7820 = SafeGetDecimal(reader, "S7820", 0)
					result.S7824 = SafeGetDecimal(reader, "S7824", 0)
					result.S7825 = SafeGetDecimal(reader, "S7825", 0)

					result.S103_01A = SafeGetDecimal(reader, "S103_01A", 0)
					result.S103_01Z = SafeGetDecimal(reader, "S103_01Z", 0)

					result.X10 = SafeGetDecimal(reader, "X10", 0)
					result.X11 = SafeGetDecimal(reader, "X11", 0)
					result.X17 = SafeGetDecimal(reader, "X17", 0)
					result.X18 = SafeGetDecimal(reader, "X18", 0)
					result.X23 = SafeGetDecimal(reader, "X23", 0)
					result.X25 = SafeGetDecimal(reader, "X25", 0)

				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				err = True
				result = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		''' <summary>
		''' Loads worked days in LP for payroll.
		''' </summary>
		''' <param name="maNr">The maNr.</param>
		''' <param name="startOfMonth">The start of month.</param>
		''' <param name="endofMonth">The end of month.</param>
		''' <param name="endOfYear">The end of year.</param>
		''' <param name="mdNr">The mandant number.</param>
		''' <returns>List of worked days in LP data or nothing in error case.</returns>
		Function LoadWorkedDaysInLpForPayroll(ByVal maNr As Integer, ByVal startOfMonth As DateTime, ByVal endofMonth As DateTime, ByVal endOfYear As DateTime, ByVal mdNr As Integer) As IEnumerable(Of WorkedDaysInLP) Implements IPayrollDatabaseAccess.LoadWorkedDaysInLpForPayroll

			Dim result As List(Of WorkedDaysInLP) = Nothing

			Dim sql As String = "[Get WorkedDays in Lp for Payroll]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MANumber", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@StartOfMonth", startOfMonth))
			listOfParams.Add(New SqlClient.SqlParameter("@EndOfMonth", endofMonth))
			listOfParams.Add(New SqlClient.SqlParameter("@EndOfYear", endOfYear))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of WorkedDaysInLP)

					While reader.Read

						Dim data = New WorkedDaysInLP

						data.ESNr = SafeGetInteger(reader, "esnr", 0)
						data.MANr = SafeGetInteger(reader, "manr", 0)
						data.ES_Ab = SafeGetDateTime(reader, "es_ab", Nothing)
						data.ES_Ende = SafeGetDateTime(reader, "es_ende", Nothing)

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
		''' Loads ahv amount in year.
		''' </summary>
		''' <param name="maNr">The maNr.</param>
		''' <param name="mdNr">The mdNr.</param>
		''' <param name="month">The month.</param>
		''' <param name="year">The year.</param>
		''' <returns>The ahv amount value in year.</returns>
		Function LoadAHVLohnInYear(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal month As Integer, ByVal year As Integer) As Decimal? Implements IPayrollDatabaseAccess.LoadAHVLohnInYear

			Dim sSql As String = String.Empty

			sSql = sSql & "Select ISNULL(Sum(LOL.M_Btr),0) As TotalYearStd From LOL Where LP <= @month "
			sSql = sSql & "And Jahr = @year And MANr = @maNr "
			sSql = sSql & "And LOL.LANr = 7100 "
			sSql = sSql & "And LOL.MDNr = @mdNr"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@maNr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("@month", month))
			listOfParams.Add(New SqlClient.SqlParameter("@year", year))

			Dim ahvlohnInYear As Decimal? = ExecuteScalar(sSql, listOfParams)

			Return ahvlohnInYear

		End Function

		''' <summary>
		''' Loads AHV Freibetrag for payroll.
		''' </summary>
		''' <param name="maNr">The maNr.</param>
		''' <param name="month">The month.</param>
		''' <param name="year">The year.</param>
		''' <param name="monthNumber">The month number.</param>
		''' <param name="mdNr">The mdNr.</param>
		''' <param name="err">Error flag.</param>
		''' <returns>Freibetrag data.</returns>
		Function LoadAHVFreibetragForPayroll(ByVal maNr As Integer, ByVal month As Integer, ByVal year As Integer, ByVal monthNumber As Integer, ByVal mdNr As Integer, ByVal err As Boolean) As AHVFreibetragData Implements IPayrollDatabaseAccess.LoadAHVFreibetragForPayroll

			Dim result As AHVFreibetragData = Nothing

			Dim sql As String = "[Get AHV Freibetrag for Payroll]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MANummer", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@LoLP", month))
			listOfParams.Add(New SqlClient.SqlParameter("@MDYear", year.ToString()))
			listOfParams.Add(New SqlClient.SqlParameter("@MonatNr", monthNumber))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = New AHVFreibetragData

					result.AHVLohnAbRentner = SafeGetDecimal(reader, "AHVLohnAbRentner", 0)
					result.AHV_Freigrenze = SafeGetDecimal(reader, "AHV_Freigrenze", 0)
					result.Sum_AHV_Freibetrag = SafeGetDecimal(reader, "Sum_AHV_Freibetrag", 0)

				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				err = True
				result = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		''' <summary>
		''' Loads AHV Rentner Betrag for payroll.
		''' </summary>
		''' <param name="maNr">The maNr.</param>
		''' <param name="month">The month.</param>
		''' <param name="year">The year.</param>
		''' <param name="mdNr">The mdNr.</param>
		''' <param name="err">Error flag.</param>
		''' <returns>Renter Betrag data.</returns>
		Function LoadAHVRentnerBetragForPayroll(ByVal maNr As Integer, ByVal month As Integer, ByVal year As Integer, ByVal mdNr As Integer, ByVal err As Boolean) As Decimal Implements IPayrollDatabaseAccess.LoadAHVRentnerBetragForPayroll

			Dim result As Decimal = 0

			Dim sql As String = "[Get AHV RentnerBetrag for Payroll]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MANummer", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@LoLP", month))
			listOfParams.Add(New SqlClient.SqlParameter("@MDYear", year.ToString()))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = SafeGetDecimal(reader, "AHV-Lohn", 0)

				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				err = True
				result = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		''' <summary>
		''' Loads ALV1 Lohn.
		''' </summary>
		''' <param name="maNr">The MANr.</param>
		''' <param name="mdNr">The MDnr.</param>
		''' <param name="month">The month.</param>
		''' <param name="year">The year.</param>
		''' <param name="ALV1_HL">The ALV1_HL.</param>
		''' <param name="ESYearTage">The ESYearTage.</param>
		''' <returns>ALV1 Lohn.</returns>
		Function LoadALV1Lohn(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal month As Integer, ByVal year As Integer, ByVal ALV1_HL As Decimal, ByVal ESYearTage As Decimal) As Decimal Implements IPayrollDatabaseAccess.LoadALV1Lohn

			Dim sSql As String
			Dim Fullmonth As Integer
			Dim HLTEs As Decimal
			Dim ALV1LK As Decimal
			Dim X As Decimal
			Dim Y As Decimal

			sSql = "Select ISNULL(SUM(M_Btr), 0) from LOL Where MANr = @maNr "
			sSql = sSql & " And LP < @month And Jahr = @year "
			sSql = sSql & " And LANr = 7220 "
			sSql = sSql & "And LOL.MDNr = @mdNr "

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@maNr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("@month", month))
			listOfParams.Add(New SqlClient.SqlParameter("@year", year))

			' Die Summe aller ALV1-Löhne.
			ALV1LK = ExecuteScalar(sSql, listOfParams)

			Fullmonth = Int(ESYearTage / 30)
			HLTEs = ALV1_HL / 12 * Fullmonth
			HLTEs = HLTEs + NumberRound(ALV1_HL / 360 * (ESYearTage - 30 * Fullmonth), 2)

			sSql = "Select ISNULL(SUM(M_Btr), 0) as [ALVBK] from LOL Where MANr = @maNr "
			sSql = sSql & " And LP <= @month And Jahr = @year "
			sSql = sSql & " And LANr = 7200 "
			sSql = sSql & "And LOL.MDNr = @mdNr "

			listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@maNr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("@month", month))
			listOfParams.Add(New SqlClient.SqlParameter("@year", year))

			' Die Summe aller ALV-Basis. Es kann nicht NULL-Wert geben!!!
			Dim ALVBK = ExecuteScalar(sSql, listOfParams)

			X = HLTEs - ALV1LK              ' Maximum dass man abziehen könnte.
			Y = ALVBK - ALV1LK       ' ALV-Betrag ohne Abzüge.

			Dim result = Math.Min(X, Y)

			Return result
		End Function

		''' <summary>
		''' Loads ALV^2 Lohn.
		''' </summary>
		''' <param name="maNr">The MANr.</param>
		''' <param name="mdNr">The MDnr.</param>
		''' <param name="month">The month.</param>
		''' <param name="year">The year.</param>
		''' <param name="ALV2_HL">The ALV2_HL.</param>
		''' <param name="ESYearTage">The ESYearTage.</param>
		''' <returns>ALV2 Lohn.</returns>
		Function LoadALV2Lohn(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal month As Integer, ByVal year As Integer, ByVal ALV2_HL As Decimal, ByVal ESYearTage As Decimal) As Decimal Implements IPayrollDatabaseAccess.LoadALV2Lohn

			Dim sSql As String
			Dim Fullmonth As Integer
			Dim HLTEs As Decimal
			Dim ALV1LK As Decimal
			Dim ALV2LK As Decimal
			Dim X As Decimal
			Dim Y As Decimal

			sSql = "Select ISNULL(SUM(M_Btr), 0) as [ALV2K] from LOL Where MANr = @maNr "
			sSql = sSql & " And LP < @month And Jahr = @year "
			sSql = sSql & " And LANr = 7240 "
			sSql = sSql & "And LOL.MDNr = @mdNr "

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@maNr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("@month", month))
			listOfParams.Add(New SqlClient.SqlParameter("@year", year))

			' Die Summe aller ALV2-Löhne.
			ALV2LK = ExecuteScalar(sSql, listOfParams)

			Fullmonth = Int(ESYearTage / 30)
			HLTEs = ALV2_HL / 12 * Fullmonth
			HLTEs = HLTEs + NumberRound(ALV2_HL / 360 * (ESYearTage - 30 * Fullmonth), 2)

			sSql = "Select ISNULL(SUM(M_Btr), 0) as [ALV1K] from LOL Where MANr = @maNr "
			sSql = sSql & " And LP <= @month And Jahr = @year "
			sSql = sSql & " And LANr = 7220 "
			sSql = sSql & "And LOL.MDNr = @mdNr "

			listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@maNr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("@month", month))
			listOfParams.Add(New SqlClient.SqlParameter("@year", year))

			' Die Summe aller ALV1-Löhne.
			'ALV1-Lohn Kumulativ bis und mit akt. Monat
			ALV1LK = ExecuteScalar(sSql, listOfParams)

			' Die Summe aller ALV-Basis. Es kann nicht NULL-Wert geben!!!
			sSql = "Select ISNULL(SUM(M_Btr), 0) as [ALVBK] from LOL Where MANr = @maNr "
			sSql = sSql & " And LP <= @month And Jahr = @year "
			sSql = sSql & " And LANr = 7200 "
			sSql = sSql & "And LOL.MDNr = @mdNr "

			listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@maNr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("@month", month))
			listOfParams.Add(New SqlClient.SqlParameter("@year", year))

			Dim ALVBK = ExecuteScalar(sSql, listOfParams)

			X = HLTEs - ALV2LK - ALV1LK             ' Maximum dass man abziehen könnte.
			Y = ALVBK - ALV2LK - ALV1LK      ' ALV-Betrag ohne Abzüge.

			Dim result = Math.Min(X, Y)

			Return result
		End Function

		''' <summary>
		''' Loads ES data for BVG days in month calculation.
		''' </summary>
		''' <param name="maNr">The MANr.</param>
		''' <param name="mdNr">The MDNr.</param>
		''' <param name="startOfMonth">The start of month.</param>
		''' <param name="endOfMonth">The end of month.</param>
		''' <returns>List of data or nothing in error case.</returns>
		Function LoadESDataForBVGDaysInMonthCalculation(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal startOfMonth As DateTime, ByVal endOfMonth As DateTime) As IEnumerable(Of ESDataForBVGDaysInMonthCalculation) Implements IPayrollDatabaseAccess.LoadESDataForBVGDaysInMonthCalculation

			Dim result As List(Of ESDataForBVGDaysInMonthCalculation) = Nothing

			Dim sql As String

			' Die BVG-pflichtige Einsätze durch gehen, die noch aktiv sind...
			sql = "Select ES.ESNr, ES.ES_Ab, ES.ES_Ende, MAL.BVGCode From ES "
			sql &= "Left Join MA_LoSetting MAL On MAL.MANr = ES.MANr "
			sql &= "Where ES.MANr = @maNr And "
			sql &= "(ES.ES_Ab <= @endOfMonth And (ES.ES_Ende is Null Or ES.ES_Ende >= @startOfMonth)) "
			sql &= "And MAL.BVGCode <> 0 "
			sql &= "And ES.MDNr = @mdNr "
			sql &= "Order By ES.ES_Ab ASC"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@maNr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@startOfMonth", startOfMonth))
			listOfParams.Add(New SqlClient.SqlParameter("@endOfMonth", endOfMonth))
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of ESDataForBVGDaysInMonthCalculation)

					While reader.Read

						Dim data = New ESDataForBVGDaysInMonthCalculation
						data.ESNr = SafeGetInteger(reader, "ESNr", 0)
						data.ES_Ab = SafeGetDateTime(reader, "ES_Ab", Nothing)
						data.ES_Ende = SafeGetDateTime(reader, "ES_Ende", Nothing)
						data.BVGCode = SafeGetString(reader, "BVGCode", "0")

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
		''' Loas BVG AnsM for payroll.
		''' </summary>
		''' <param name="age">The age.</param>
		''' <param name="year">The year.</param>
		''' <param name="mdNr">The mdnr</param>
		''' <returns>BVG AnsM or nothing if not found.</returns>
		Function LoadBVGAnsMForPayroll(ByVal age As Integer, ByVal year As Integer, ByVal mdNr As Integer) As Decimal? Implements IPayrollDatabaseAccess.LoadBVGAnsMForPayroll

			Dim bvgAns As Decimal? = Nothing

			Dim sql As String

			sql = "[Get BVG AnsM for Payroll]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Alter", age))
			listOfParams.Add(New SqlClient.SqlParameter("@MDYear", year))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", mdNr))

			bvgAns = ExecuteScalar(sql, listOfParams, CommandType.StoredProcedure)

			Return bvgAns
		End Function

		''' <summary>
		''' Loas BVG AnsW for payroll.
		''' </summary>
		''' <param name="age">The age.</param>
		''' <param name="year">The year.</param>
		''' <param name="mdNr">The mdnr</param>
		''' <returns>BVG AnsM or nothing if not found.</returns>
		Function LoadBVGAnsWForPayroll(ByVal age As Integer, ByVal year As Integer, ByVal mdNr As Integer) As Decimal? Implements IPayrollDatabaseAccess.LoadBVGAnsWForPayroll

			Dim bvgAns As Decimal? = Nothing

			Dim sql As String

			sql = "[Get BVG AnsW for Payroll]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Alter", age))
			listOfParams.Add(New SqlClient.SqlParameter("@MDYear", year))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", mdNr))

			bvgAns = ExecuteScalar(sql, listOfParams, CommandType.StoredProcedure)

			Return bvgAns
		End Function

		''' <summary>
		''' Checks if exits LA In LO.
		''' </summary>
		''' <param name="loNr">The LONr.</param>
		''' <param name="mdNr">The MDNr.</param>
		''' <returns>Boolean flag indicating if exists LA in LO exits or nothing in error case.</returns>
		Function CheckIfLAInLOExists(ByVal loNr As Integer, ByVal mdNr As Integer) As Boolean? Implements IPayrollDatabaseAccess.CheckIfLAInLOExists

			Dim doesExist = False

			Dim sql As String = String.Empty

			sql = sql & "Select COUNT(*) From LOL Where LANr In "
			sql = sql & "(Select substring(LA.FuncBeforePrint,charindex('(',LA.FuncBeforePrint,0),LEN(LA.FuncBeforePrint)) "
			sql = sql & "From LA Where LA.FuncBeforePrint <> '' And LA.LANr = 6990 And LA.LADeactivated = 0) "
			sql = sql & "And LONr = @loNr "
			sql = sql & "And LOL.MDNr = @mdNr "


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@loNr", loNr))
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))

			Dim existingRecordsCount = ExecuteScalar(sql, listOfParams)

			If Not existingRecordsCount Is Nothing Then
				doesExist = (existingRecordsCount > 0)

				Return doesExist
			Else
				Return Nothing
			End If

		End Function

		Function VerifyUnusualPayrollData(ByVal mdNr As Integer, ByVal MANr As Integer, ByVal loNr As Integer, ByVal monat As Integer, ByVal jahr As Integer, ByVal createdUserNumber As Integer, ByVal bvgStartDate As Date?, ByVal bvgToDate As Date?) As IEnumerable(Of PayrollUnusualData) Implements IPayrollDatabaseAccess.VerifyUnusualPayrollData

			Dim result As List(Of PayrollUnusualData) = Nothing

			Dim sql As String

			sql = "[Load Unusual Data For Created Payroll]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("maNr", MANr))
			listOfParams.Add(New SqlClient.SqlParameter("loNr", loNr))
			listOfParams.Add(New SqlClient.SqlParameter("Month", monat))
			listOfParams.Add(New SqlClient.SqlParameter("year", jahr))
			listOfParams.Add(New SqlClient.SqlParameter("bvgStartDate", ReplaceMissing(bvgStartDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("bvgToDate", ReplaceMissing(bvgToDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("createdUserNumber", createdUserNumber))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of PayrollUnusualData)

					While reader.Read
						Dim data = New PayrollUnusualData

						data.MANr = SafeGetInteger(reader, "EmployeeNumber", Nothing)
						data.LastName = SafeGetString(reader, "Lastname")
						data.FirstName = SafeGetString(reader, "Firstname")
						data.LANr = SafeGetDecimal(reader, "LANr", Nothing)
						data.LALabel = SafeGetString(reader, "LALabel")
						data.Reason = SafeGetString(reader, "Reason")

						data.CreatedOn = SafeGetDateTime(reader, "Createdon", Nothing)
						data.CreatedUserNumber = SafeGetInteger(reader, "CreatedUserNumber", Nothing)


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
		''' Loads BVG Std basis.
		''' </summary>
		''' <param name="maNr">The MANr.</param>
		''' <param name="mdNr">The MDNr.</param>
		''' <param name="month">The month.</param>
		''' <param name="year">The year.</param>
		''' <returns>BGV Std Basis or nothing in error case.</returns>
		Function LoadBVGStdBasis(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal month As Integer, ByVal year As Integer) As Decimal? Implements IPayrollDatabaseAccess.LoadBVGStdBasis

			Dim result As Decimal? = Nothing

			Dim sSql As String
			Dim StartofMonth As Date
			Dim EndofMonth As Date
			Dim EndofYear As Date
			Dim StdLohnTotal As Decimal
			Dim i As Integer

			StartofMonth = CDate("01." & month & "." & year)
			EndofMonth = CDate(DateAdd("m", 1, StartofMonth.AddDays(-StartofMonth.Day + 1))).AddDays(-1)
			EndofYear = CDate("31.12." & year)

			sSql = "Select ESLohn.Stundenlohn From ESLohn Left Join ES On "
			sSql = sSql & "ESLohn.ESNr = ES.ESNr Where ES.MANr = @maNr And "
			sSql = sSql & "(ES.ES_Ab <= @endofMonth And (ES.ES_Ende is Null Or ES.ES_Ende >= @startofMonth)) "
			sSql = sSql & "And ESLohn.AktivLODaten = 1 "
			sSql = sSql & "And ES.MDNr = @mdNr "
			sSql = sSql & "Order By ES.ES_Ab"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@maNr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("@startofMonth", StartofMonth))
			listOfParams.Add(New SqlClient.SqlParameter("@endofMonth", EndofMonth))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sSql, listOfParams, CommandType.Text)

			Try
				If (Not reader Is Nothing) Then

					result = 0

					While reader.Read

						i = i + 1
						Dim stdLohn = SafeGetDecimal(reader, "Stundenlohn", 0)
						StdLohnTotal = StdLohnTotal + stdLohn

					End While

					If (i > 0) Then
						result = NumberRound((StdLohnTotal / i), 2)
					End If

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
		''' Loads std in year for BVG.
		''' </summary>
		''' <param name="maNr">The maNr.</param>
		''' <param name="mdNr">The mdNr.</param>
		''' <param name="month">The month.</param>
		''' <param name="year">The year.</param>
		''' <returns>The std in year for BVG value.</returns>
		Function LoadStdInYearForBVG(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal month As Integer, ByVal year As Integer) As Decimal? Implements IPayrollDatabaseAccess.LoadStdInYearForBVG

			Dim sSql As String = String.Empty

			sSql = sSql & "Select ISNULL(Sum(LOL.M_Btr),0) As TotalYearStd From LOL Where LP <= @month "
			sSql = sSql & "And Jahr = @year And MANr = @maNr "
			sSql = sSql & "And LOL.LANr = 6990 "
			sSql = sSql & "And LOL.MDNr = @mdNr"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@maNr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("@month", month))
			listOfParams.Add(New SqlClient.SqlParameter("@year", year))

			Dim stdInYearForBVG As Decimal? = ExecuteScalar(sSql, listOfParams)

			Return stdInYearForBVG

		End Function

		''' <summary>
		''' Loads count of zg with internationalbank transfer.
		''' </summary>
		''' <param name="maNr">The MANr.</param>
		''' <param name="mdNr">The MDNr.</param>
		''' <param name="loNr">The LONr.</param>
		''' <returns>AnzZGABank value</returns>
		Function LoadAnzZGABank(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal loNr As Integer) As Integer? Implements IPayrollDatabaseAccess.LoadAnzZGABank

			Dim sSql As String = String.Empty

			sSql = "Select Count(*) As AnzRec From ZG "
			sSql &= "LEFT JOIN LOL ON ZG.ZGNr = LOL.DestZGNr And ZG.MANr = LOL.MANr And ZG.MDNr = LOL.MDNr "
			sSql &= "Where LOL.LONr = @loNr And "
			sSql &= "ZG.MANr = @maNr And ZG.GebAbzug = 1 And ZG.BnkAu = 1 And ZG.LANr = 8920 "
			sSql &= "And ZG.MDNr = @mdNr"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@maNr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("@loNr", loNr))

			Dim anzRec As Integer? = ExecuteScalar(sSql, listOfParams)

			Return anzRec

		End Function

		''' <summary>
		''' Loads aktiv MABankData for payroll.
		''' </summary>
		''' <param name="maNr">The MaNr.</param>
		''' <param name="err">Error flag.</param>
		''' <returns>Aktiv MABankData or nothing of not found.</returns>
		Function LoadAktivMABankDataForPayroll(ByVal maNr As Integer, ByRef err As Boolean) As AktivMABankDataForPayroll Implements IPayrollDatabaseAccess.LoadAktivMABankDataForPayroll

			Dim result As AktivMABankDataForPayroll = Nothing

			Dim sql As String = "[Get Aktiv MABankData for Payroll]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MANummer", maNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = New AktivMABankDataForPayroll

					result.BnkAu = SafeGetBoolean(reader, "BnkAu", False)

				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				err = True
				result = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		''' <summary>
		''' Loads AnzLO data.
		''' </summary>
		''' <param name="maNr">The MANr.</param>
		''' <param name="mdNr">The MDNr.</param>
		''' <returns>AnzLO data.</returns>
		Function LoadAnzLO(ByVal maNr As Integer, ByVal mdNr As Integer) As Integer? Implements IPayrollDatabaseAccess.LoadAnzLO

			Dim sSql As String = String.Empty

			sSql = "Select Count(*) As AnzLO From LO Where MANr = @maNr And LO.MDNr = @mdNr"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@maNr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))

			Dim anzRec As Integer? = ExecuteScalar(sSql, listOfParams)

			Return anzRec

		End Function

		''' <summary>
		''' Loads RP Data for ES_Std_Total_New0 calculation.
		''' </summary>
		''' <param name="maNr">The MANr.</param>
		''' <param name="mdNr">The MDNr.</param>
		''' <param name="date1">The filter date1.</param>
		''' <param name="date2">The filter date2.</param>
		''' <returns>List of RP Data for calculation.</returns>
		Function LoadRPDataForESStdTotalNew0Calculation(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal date1 As DateTime, ByVal date2 As DateTime) As List(Of RPDataForESStdTotalNew0Calculation) Implements IPayrollDatabaseAccess.LoadRPDataForESStdTotalNew0Calculation

			Dim result As List(Of RPDataForESStdTotalNew0Calculation) = Nothing

			Dim sSql As String = String.Empty

			' iCounter_0 = GetTickCount
			sSql = sSql & "Select RP.RPNr, RP.Von, RP.Bis, RP.Monat, RP.Jahr, DATEDIFF(D, RP.Von, RP.Bis)+1 As ESRPTage From RP "
			sSql = sSql & "Where RP.MANr = @maNr And "
			sSql = sSql & "RP.Von >= @date1 "
			sSql = sSql & "And RP.Bis <= @date2 "
			sSql = sSql & "And RP.MDNr = @mdNr "
			sSql = sSql & "Order By RP.Von ASC, RP.Monat ASC, RP.RPNr ASC"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@maNr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("@date1", date1))
			listOfParams.Add(New SqlClient.SqlParameter("@date2", date2))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sSql, listOfParams, CommandType.Text)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of RPDataForESStdTotalNew0Calculation)

					While reader.Read

						Dim data = New RPDataForESStdTotalNew0Calculation
						data.RPNr = SafeGetInteger(reader, "RPNr", 0)
						data.Von = SafeGetDateTime(reader, "Von", Nothing)
						data.Bis = SafeGetDateTime(reader, "Bis", Nothing)
						data.Monat = SafeGetByte(reader, "Monat", 0)
						data.Jahr = SafeGetString(reader, "Jahr", String.Empty)
						data.ESRPTage = SafeGetInteger(reader, "ESRPTage", 0)

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
		''' Loads RP Data for ES_Std_Total_New1 calculation.
		''' </summary>
		''' <param name="maNr">The MANr.</param>
		''' <param name="mdNr">The MDNr.</param>
		''' <param name="date1">The filter date1.</param>
		''' <param name="date2">The filter date2.</param>
		''' <returns>List of RP Data for calculation.</returns>
		Function LoadRPDataForESStdTotalNew1Calculation(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal date1 As DateTime, ByVal date2 As DateTime) As List(Of RPDataForESStdTotalNew1Calculation) Implements IPayrollDatabaseAccess.LoadRPDataForESStdTotalNew1Calculation

			Dim result As List(Of RPDataForESStdTotalNew1Calculation) = Nothing

			Dim sSql As String = String.Empty

			' iCounter_0 = GetTickCount
			sSql = "Select RP.RPNr, RP.Von, RP.Bis, RP.Monat, RP.Jahr, DATEDIFF(D, RP.Von, RP.Bis)+1 As ESRPTage From RP "
			sSql = sSql & "Where RP.MANr = @maNr And "
			sSql = sSql & "RP.Von >= @date1 "
			sSql = sSql & "And RP.Bis <= @date2 "
			sSql = sSql & "And RP.MDNr = @mdNr "
			sSql = sSql & "Order By RP.Von ASC"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@maNr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("@date1", date1))
			listOfParams.Add(New SqlClient.SqlParameter("@date2", date2))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sSql, listOfParams, CommandType.Text)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of RPDataForESStdTotalNew1Calculation)

					While reader.Read

						Dim data = New RPDataForESStdTotalNew1Calculation
						data.RPNr = SafeGetInteger(reader, "RPNr", 0)
						data.Von = SafeGetDateTime(reader, "Von", Nothing)
						data.Bis = SafeGetDateTime(reader, "Bis", Nothing)
						data.Monat = SafeGetByte(reader, "Monat", 0)
						data.Jahr = SafeGetString(reader, "Jahr", String.Empty)
						data.ESRPTage = SafeGetInteger(reader, "ESRPTage", 0)

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
		''' Load std total for BVG std for RPL0 calculation.
		''' </summary>
		''' <param name="strFieldBez">The field bez.</param>
		''' <param name="rpNr">The report number.</param>
		''' <returns>The sum.</returns>
		Function LoadStdTotalForBVGStdFromRPL0Calculation(ByVal strFieldBez As String, ByVal rpNr As Integer) As Decimal? Implements IPayrollDatabaseAccess.LoadStdTotalForBVGStdFromRPL0Calculation

			Dim sSql = "Select IsNull( Sum(" & strFieldBez & "), 0) As StdTotal From RPL_MA_Day Where RPNr = @rpNr"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@rpNr", rpNr))

			Dim sum As Decimal? = ExecuteScalar(sSql, listOfParams)

			Return sum

		End Function

		''' <summary>
		''' Load std total for BVG std for RPL1 calculation.
		''' </summary>
		''' <param name="strFieldBez">The field bez.</param>
		''' <param name="rpNr">The report number.</param>
		''' <returns>The sum.</returns>
		Function LoadStdTotalForBVGStdFromRPL1Calculation(ByVal strFieldBez As String, ByVal rpNr As Integer) As Decimal? Implements IPayrollDatabaseAccess.LoadStdTotalForBVGStdFromRPL1Calculation

			Dim sql = "Select IsNull( Sum(" & strFieldBez & "), 0) As StdTotal From RPL_MA_Day Where RPNr = @rpNr"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@rpNr", rpNr))

			Dim sum As Decimal? = ExecuteScalar(sql, listOfParams)

			Return sum

		End Function

		Function LoadStdTotalForBVGStdFromRPLShorttime(ByVal mdnr As Integer, ByVal maNr As Integer, ByVal monat As Integer, ByVal jahr As Integer) As Decimal? Implements IPayrollDatabaseAccess.LoadStdTotalForBVGStdFromRPLShorttime

			Dim sql As String


			sql = "Select IsNull( Sum(M_Anzahl), 0) As ShorttimeTotal "
			sql &= "From RPL "
			sql &= "Left Join RP On RP.RPNr = RPL.RPNr AND RP.MANr = RPL.MANr "
			sql &= "Where RP.MDNr = @mdNr "
			sql &= "And RP.Jahr = @jahr "
			sql &= "And RP.Monat = @monat "
			sql &= "And RPL.MANr = @maNr "
			sql &= "And RPL.KD = 0 "
			sql &= "And RPL.LANr = 103.01 "

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdnr", mdnr))
			listOfParams.Add(New SqlClient.SqlParameter("maNr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("monat", monat))
			listOfParams.Add(New SqlClient.SqlParameter("jahr", jahr))

			Dim sum As Decimal? = ExecuteScalar(sql, listOfParams)

			Return sum

		End Function

		''' <summary>
		''' Load ES data for ES Std Total calculation.
		''' </summary>
		''' <param name="maNr">The MANr.</param>
		''' <param name="mdNr">THE MDNr.</param>
		''' <param name="startOfMonth">The start of month.</param>
		''' <param name="endOfMonth">The end of month.</param>
		''' <returns>List of ES data or nonthing in error case.</returns>
		Function LoadESDataForESStdTotalCalculation(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal startOfMonth As DateTime, ByVal endOfMonth As DateTime) As List(Of ESDataForESStdTotalCalculation) Implements IPayrollDatabaseAccess.LoadESDataForESStdTotalCalculation

			Dim result As List(Of ESDataForESStdTotalCalculation) = Nothing

			Dim sSql As String = String.Empty

			sSql = sSql & "Select ESTable.ESNr, MAL.BVGCode, "

			' Totalgeleistete Stunden von Einsatz errechnen...
			sSql = sSql & "(Select ISNULL(Sum(m_Anzahl), 0) From RPL Left Join LA "
			sSql = sSql & "On RPL.LANr = LA.LANr And Year(RPL.VonDate) = LA.LAJahr And LA.LADeactivated = 0 "
			sSql &= "Left Join MA_LOSetting MAL On MAL.MANr = ESTable.MANr "

			sSql = sSql & "Where LA.Sum0Anzahl = '2' "
			sSql = sSql & "And LA.BVGPflichtig = 1 "
			sSql = sSql & "And RPL.KD = 0 "
			sSql = sSql & "And RPL.ESNr = ESTable.ESNr "
			sSql = sSql & "And RPL.VonDate <= @endOfMonth "
			sSql = sSql & "And m_Betrag <> 0) As TotalESStd "

			sSql = sSql & "From ES ESTable Where ESTable.MANr = @maNr And "
			sSql = sSql & "(ESTable.ES_Ab <= @endOfMonth And (ESTable.ES_Ende is Null Or ESTable.ES_Ende >= @startOfMonth)) "
			sSql = sSql & "And MAL.BVGCode <> 0 "
			sSql = sSql & "And ESTable.MDNr = @mdNr "
			sSql = sSql & "Order By ESTable.ESNr ASC"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@maNr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("@startOfMonth", startOfMonth))
			listOfParams.Add(New SqlClient.SqlParameter("@endOfMonth", endOfMonth))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sSql, listOfParams, CommandType.Text)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of ESDataForESStdTotalCalculation)

					While reader.Read

						Dim data = New ESDataForESStdTotalCalculation
						data.ESNr = SafeGetInteger(reader, "ESNr", 0)
						data.BVGCode = SafeGetString(reader, "BVGCode", "0")
						data.TotalESStd = SafeGetDecimal(reader, "TotalESStd", 0)

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
		''' Load ES data for RP Std Total.
		''' </summary>
		''' <param name="maNr">The MANr.</param>
		''' <param name="mdNr">THE MDNr.</param>
		''' <param name="monat">The month.</param>
		''' <param name="jahr">The year.</param>
		''' <returns>total worked time in month.</returns>
		Function LoadESDataForRPStdTotal(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal monat As Integer, ByVal jahr As Integer) As Decimal? Implements IPayrollDatabaseAccess.LoadESDataForRPStdTotal

			Dim result As Decimal

			Dim sql As String

			' Totalgeleistete Stunden von Monat errechnen...
			sql = "Select ISNULL(Sum(m_Anzahl), 0) TotalMonthStd From RPL Left Join LA "
			sql &= "On RPL.LANr = LA.LANr And Year(RPL.VonDate) = LA.LAJahr AND LA.LADeactivated = 0 "
			sql &= "LEFT JOIN RP ON RP.RPNr = RPL.RPNr "
			sql &= "Where RP.MDNr = @MDNr "
			sql &= "And LA.Sum0Anzahl = '2' "
			sql &= "And LA.BVGPflichtig = 1 "
			sql &= "And RPL.KD = 0 "
			sql &= "And RPL.MANr = @manr "
			sql &= "AND RPL.VonDate >= dbo.DateTimeFromYearMonthDay(@year,@month, 1) AND RPL.BisDate <= (dbo.DateTimeFromYearMonthDay(@year, @month + 1, 1) - 1) "
			sql &= "And m_Betrag <> 0 "

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@maNr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("@month", monat))
			listOfParams.Add(New SqlClient.SqlParameter("@year", jahr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try
				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = SafeGetDecimal(reader, "TotalMonthStd", 0)

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
		''' Load LOL Data for ES Std Total calculation.
		''' </summary>
		''' <param name="maNr">The MANr.</param>
		''' <param name="mdNr">The MDNr.</param>
		''' <param name="month">The month.</param>
		''' <param name="year">The year.</param>
		''' <returns>LOL data or nothing in error case.</returns>
		Function LoadLOLDataForESStdTotalCalculation(ByVal maNr As Integer, mdNr As Integer, ByVal month As Integer, ByVal year As Integer) As LOLDataForESStdTotalCalculation Implements IPayrollDatabaseAccess.LoadLOLDataForESStdTotalCalculation

			Dim result As LOLDataForESStdTotalCalculation = Nothing

			Dim sSql As String = String.Empty

			sSql = sSql & "Select"
			sSql = sSql & "(Select Sum(m_Btr) As TotalStdBefore From LOL Where "
			sSql = sSql & "LOL.LANr In (7590) And LOL.MANr = @maNr "
			sSql = sSql & "And LOL.LP < @month And Jahr = @year And m_Btr <> 0 "
			sSql = sSql & "And LOL.MDNr = @mdNr) as Sum1,"


			sSql = sSql & "(Select Sum(m_Btr) As TotalStdBefore From LOL Where "
			sSql = sSql & "LOL.LANr = 7520 And LOL.MANr = @maNr "
			sSql = sSql & "And LOL.LP < @month And Jahr = @year And m_Btr <> 0 "
			sSql = sSql & "And LOL.MDNr = @mdNr) as Sum2"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@maNr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("@month", month))
			listOfParams.Add(New SqlClient.SqlParameter("@year", year))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sSql, listOfParams, CommandType.Text)

			Try
				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = New LOLDataForESStdTotalCalculation

					result.Sum1 = SafeGetDecimal(reader, "Sum1", 0)
					result.Sum2 = SafeGetDecimal(reader, "Sum2", 0)

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
		''' Checks if a ES exists for check for BVG.
		''' </summary>
		''' <param name="maNr">The MANr.</param>
		''' <param name="mdNr">The MDNr.</param>
		''' <param name="startOfMonth">The startOfMonth.</param>
		''' <param name="endOfMonth">The endOfMonth.</param>
		''' <returns>Boolean flag indicating if ES exits or nothing in error case.</returns>
		Function CheckIfESExistsForCheckForBVG(maNr As Integer, ByVal mdNr As Integer, ByVal startOfMonth As DateTime, ByVal endOfMonth As DateTime) As Boolean? Implements IPayrollDatabaseAccess.CheckIfESExistsForCheckForBVG

			Dim doesExist = False

			Dim sql As String = String.Empty

			sql = sql & "Select Count(*) FROM ES Where ES.MANr = @maNr And "
			sql = sql & "(ES.ES_Ende >=  @dStartofMonth_BVG Or ES.ES_Ende is Null) And ES.ES_Ab <= @dEndofMonth "
			sql = sql & "And ES.MDNr = @mdNr "

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@maNr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("@dStartofMonth_BVG", startOfMonth))
			listOfParams.Add(New SqlClient.SqlParameter("@dEndofMonth", endOfMonth))

			Dim existingESRecordsCount = ExecuteScalar(sql, listOfParams)

			If Not existingESRecordsCount Is Nothing Then
				doesExist = (existingESRecordsCount > 0)

				Return doesExist
			Else
				Return Nothing
			End If

		End Function

		''' <summary>
		''' Check if exits diff Beitrag.
		''' </summary>
		''' <param name="maNr">The MANr.</param>
		''' <param name="mdNr">The MDNr.</param>
		''' <param name="loNr">The LONr.</param>
		''' <param name="mode">The Mode.</param>
		''' <returns>Boolean flag indicating if exists or nothing in error case.</returns>
		Function CheckIfExistDiffBeitrag(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal loNr As Integer, ByVal mode As Integer) As Boolean? Implements IPayrollDatabaseAccess.CheckIfExistDiffBeitrag

			Dim result As Boolean? = Nothing

			Dim sSql As String = "[Get ExistDiffBeitrag For Payroll]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@maNr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("@loNr", loNr))
			listOfParams.Add(New SqlClient.SqlParameter("@iMode", mode))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sSql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = SafeGetBoolean(reader, "ExistsBeitrag", Nothing)

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
		''' Loads RP Data for ES_Std_4_New_KTG_0 calculation.
		''' </summary>
		''' <param name="maNr">The MANr.</param>
		''' <param name="mdNr">The MDNr.</param>
		''' <param name="date1">The date1.</param>
		''' <param name="date2">The date2.</param>
		''' <returns>List of RP data for calculation or nothing in error case.</returns>
		Function LoadRPDataForESStd4NewKTG0Calculation(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal date1 As DateTime, ByVal date2 As DateTime) As List(Of RPDataForESStd4NewKTG0Calculation) Implements IPayrollDatabaseAccess.LoadRPDataForESStd4NewKTG0Calculation


			Dim result As List(Of RPDataForESStd4NewKTG0Calculation) = Nothing

			Dim sSql As String = String.Empty

			' iCounter_0 = GetTickCount
			sSql = "Select RP.RPNr, RP.Von, RP.Bis, RP.Monat, RP.Jahr From RP "
			sSql = sSql & "Where RP.MANr = @maNr And "
			sSql = sSql & "RP.Von >= @date1 "
			sSql = sSql & "And RP.Bis < @date2 "
			'    sSql = sSql & "And RP.RPGAV_Nr In " & Anhang1GAV & ") "
			sSql = sSql & "And RP.MDNr = @mdNr "
			sSql = sSql & "Order By RP.Von ASC, RP.Monat ASC, RP.RPNr ASC"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@maNr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("@date1", date1))
			listOfParams.Add(New SqlClient.SqlParameter("@date2", date2))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sSql, listOfParams, CommandType.Text)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of RPDataForESStd4NewKTG0Calculation)

					While reader.Read

						Dim data = New RPDataForESStd4NewKTG0Calculation
						data.RPNr = SafeGetInteger(reader, "RPNr", 0)
						data.Von = SafeGetDateTime(reader, "Von", Nothing)
						data.Bis = SafeGetDateTime(reader, "Bis", Nothing)
						data.Monat = SafeGetByte(reader, "Monat", 0)
						data.Jahr = SafeGetString(reader, "Jahr", String.Empty)

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
		''' Loads RP data for ES_Std_4_New_KTG_1 calculation.
		''' </summary>
		''' <param name="maNr">The MANr.</param>
		''' <param name="mdNr">The MDNr.</param>
		''' <param name="date1">The date1.</param>
		''' <param name="date2">The date2.</param>
		''' <returns>List of RP data for calculation or nothing in error case.</returns>
		Function LoadRPDataForESStd4NewKTG1Calculation(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal date1 As DateTime, ByVal date2 As DateTime) As List(Of RPDataForESStd4NewKTG1Calculation) Implements IPayrollDatabaseAccess.LoadRPDataForESStd4NewKTG1Calculation

			Dim result As List(Of RPDataForESStd4NewKTG1Calculation) = Nothing

			Dim sSql As String = String.Empty

			' iCounter_0 = GetTickCount
			sSql = "Select RP.RPNr, RP.Von, RP.Bis, RP.Monat, RP.Jahr, DATEDIFF(D, RP.Von, RP.Bis)+1 As ESRPTage From RP "
			sSql = sSql & "Where RP.MANr = @maNr And "
			sSql = sSql & "RP.Von >=  @date1 "
			sSql = sSql & "And RP.Bis < @date2 "
			'    sSql = sSql & "And RP.RPGAV_Nr In " & Anhang1GAV & ") "
			sSql = sSql & "And RP.MDNr = @mdNr "
			sSql = sSql & "Order By RP.Von ASC"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@maNr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("@date1", date1))
			listOfParams.Add(New SqlClient.SqlParameter("@date2", date2))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sSql, listOfParams, CommandType.Text)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of RPDataForESStd4NewKTG1Calculation)

					While reader.Read

						Dim data = New RPDataForESStd4NewKTG1Calculation
						data.RPNr = SafeGetInteger(reader, "RPNr", 0)
						data.Von = SafeGetDateTime(reader, "Von", Nothing)
						data.Bis = SafeGetDateTime(reader, "Bis", Nothing)
						data.Monat = SafeGetByte(reader, "Monat", 0)
						data.Jahr = SafeGetString(reader, "Jahr", String.Empty)
						data.ESRPTage = SafeGetInteger(reader, "ESRPTage", 0)

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

		Function LoadCurrentMonthShorttimeWorkAmount(ByVal mdNr As Integer, ByVal maNr As Integer, ByVal lonr As Integer) As Decimal? Implements IPayrollDatabaseAccess.LoadCurrentMonthShorttimeWorkAmount

			Dim result As Decimal

			Dim sql As String

			' Totalgeleistete Stunden von Monat errechnen...
			sql = "Select ISNULL(Sum(m_Btr), 0) TotalAmount From dbo.LOL "
			sql &= "Where LOL.MDNr = @MDNr "
			sql &= "And LOL.LANr = 103.01 "
			sql &= "And LOL.MANr = @manr "
			sql &= "AND LOL.LONr = @lonr "
			sql &= "And ISNull(m_Btr, 0) <> 0 "

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("maNr", ReplaceMissing(maNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", ReplaceMissing(mdNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("lonr", ReplaceMissing(lonr, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try
				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = SafeGetDecimal(reader, "TotalAmount", 0)

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
		''' Checks if a filter condition matches.
		''' </summary>
		''' <param name="storedProcedureName">The stored procedure name.</param>
		''' <param name="maNr">The MANr.</param>
		''' <returns>Boolean flag indicating success or nothing in error case.</returns>
		Function DoesFilterConditionMatch(ByVal storedProcedureName As String, ByVal maNr As Integer) As Boolean? Implements IPayrollDatabaseAccess.DoesFilterConditionMatch

			Dim result As Boolean? = False

			Dim sql As String = String.Format("[{0}]", storedProcedureName)

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MANummer", maNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = True

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
		''' Checks if a filter condition matches.
		''' </summary>
		''' <param name="storedProcedureName">The stored procedure name.</param>
		''' <param name="maNr">The MANr.</param>
		''' <returns>Boolean flag indicating success or nothing in error case.</returns>
		Function DoesFilterConditionMatch(ByVal storedProcedureName As String, ByVal maNr As Integer, ByVal sVar As Decimal) As Boolean? Implements IPayrollDatabaseAccess.DoesFilterConditionMatch

			Dim result As Boolean? = False

			Dim sql As String = String.Format("[{0}]", storedProcedureName)

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MANummer", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("SVar", sVar))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = True

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
		''' Checks if a filter condition matches.
		''' </summary>
		''' <param name="storedProcedureName">The stored procedure name.</param>
		''' <param name="maNr">The MANr.</param>
		''' <returns>Boolean flag indicating success or nothing in error case.</returns>
		Function DoesFilterConditionMatch(ByVal storedProcedureName As String, ByVal maNr As Integer, ByVal sVar1 As Decimal, ByVal sVar2 As Decimal) As Boolean? Implements IPayrollDatabaseAccess.DoesFilterConditionMatch

			Dim result As Boolean? = False

			Dim sql As String = String.Format("[{0}]", storedProcedureName)

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MANummer", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("SVar1", sVar1))
			listOfParams.Add(New SqlClient.SqlParameter("SVar2", sVar2))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = True

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
		''' Checks if a condition matches.
		''' </summary>
		''' <param name="storedProcedureName">The stored procedure name.</param>
		''' <param name="maNr">The MANr.</param>
		''' <returns>Boolean flag indicating success or nothing in error case.</returns>
		Function LoadFilterConditionDataForCase4(ByVal storedProcedureName As String, ByVal maNr As Integer, ByVal lONewNr As Integer, ByVal err As Boolean) As Decimal? Implements IPayrollDatabaseAccess.LoadFilterConditionDataForCase4

			Dim result As Decimal? = False

			Dim sql As String = String.Format("[{0}]", storedProcedureName)

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MANummer", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("LONR", lONewNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = SafeGetDecimal(reader, "Beitrag", Nothing)

				End If

			Catch e As Exception
				result = Nothing
				err = True
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		Function LoadValueAmountDataForCase(ByVal functionNumber As Decimal, ByVal maNr As Integer, ByVal lONewNr As Integer, ByVal err As Boolean) As Decimal? Implements IPayrollDatabaseAccess.LoadValueAmountDataForCase

			Dim result As Decimal?

			Dim sql As String = LoadProcedureNameDataForCase(functionNumber, err)
			If String.IsNullOrWhiteSpace(sql) Then Return 0

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MANummer", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("LONR", lONewNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = SafeGetDecimal(reader, "Betrag", Nothing)

				End If

			Catch e As Exception
				result = Nothing
				err = True
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		Private Function LoadProcedureNameDataForCase(ByVal functionNumber As Decimal, ByVal err As Boolean) As String

			Dim result As String = String.Empty

			Dim sql As String
			sql = "Select TOP (1) * From dbo.tbl_LA_Function_Procedures "
			sql &= "Where FuncNumber = @functionNumber "
			sql &= "Order By ID Desc"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("functionNumber", functionNumber))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then
					result = SafeGetString(reader, "ProcName")
				End If

			Catch e As Exception
				result = Nothing
				err = True
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		''' <summary>
		''' Loads LOL data for LO Back.
		''' </summary>
		''' <param name="maNr">The MANr.</param>
		''' <param name="mdNr">The MDNr.</param>
		''' <param name="loNr">The LONr.</param>
		''' <param name="laNr">The LANr.</param>
		''' <returns>List of LA data.</returns>
		Function LoadLOLDataForRepeatLA4LOBack(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal loNr As Integer, ByVal laNr As Integer) As IEnumerable(Of LOLDataFoRepeatLA4LOBack) Implements IPayrollDatabaseAccess.LoadLOLDataForRepeatLA4LOBack

			Dim result As List(Of LOLDataFoRepeatLA4LOBack) = Nothing

			Dim sSql As String = String.Empty

			sSql = "Select * From LOL Where LONr = @loNr And MANr = @maNr And "
			sSql = sSql & "LANr = @laNr "
			sSql = sSql & "And MDNr = @mdNr "
			sSql = sSql & " Order By ID ASC"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@maNr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("@loNr", loNr))
			listOfParams.Add(New SqlClient.SqlParameter("@laNr", laNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sSql, listOfParams, CommandType.Text)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of LOLDataFoRepeatLA4LOBack)

					While reader.Read

						Dim data = New LOLDataFoRepeatLA4LOBack

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
						data.GAVNr = SafeGetInteger(reader, "GAVNr", 0)
						data.GAV_Kanton = SafeGetString(reader, "GAV_Kanton", String.Empty)
						data.GAV_Beruf = SafeGetString(reader, "GAV_Beruf", String.Empty)
						data.GAV_Gruppe1 = SafeGetString(reader, "GAV_Gruppe1", String.Empty)
						data.GAV_Gruppe2 = SafeGetString(reader, "GAV_Gruppe2", String.Empty)
						data.GAV_Gruppe3 = SafeGetString(reader, "GAV_Gruppe3", String.Empty)
						data.GAV_Text = SafeGetString(reader, "GAV_Text", String.Empty)
						data.ESEinstufung = SafeGetString(reader, "ESEinstufung", String.Empty)
						data.ESBranche = SafeGetString(reader, "ESBranche", String.Empty)

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
		''' Loads LOL data for GetLMDTAStatus.
		''' </summary>
		''' <param name="manr">The MANr.</param>
		''' <param name="loNr">The LONr.</param>
		''' <param name="err">The error flag.</param>
		''' <returns>The LOL data.</returns>
		Function LoadLOLDataForGetLMDTAStatus(ByVal manr As Integer, ByVal loNr As Integer, ByRef err As Boolean) As LOLDataForGetLMDTAStatus Implements IPayrollDatabaseAccess.LoadLOLDataForGetLMDTAStatus

			Dim result As LOLDataForGetLMDTAStatus = Nothing

			Dim sql As String = String.Empty

			sql = sql & "Select Top 1 LMWithDTA, ZGGrund, BnkNr From LOL Where LONr = @loNr And MANr = @maNr "
			sql = sql & "And LANr = 8720 And LMWithDTA = 1 "
			'sSql = sSql & "And LOL.MDNr = " & MDNr & " "

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("maNr", manr))
			listOfParams.Add(New SqlClient.SqlParameter("loNr", loNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = New LOLDataForGetLMDTAStatus

					result.LMWithDTA = SafeGetBoolean(reader, "LMWithDTA", False)
					result.ZGGrund = SafeGetString(reader, "ZGGrund", String.Empty)
					result.BnkNr = SafeGetInteger(reader, "BnkNr", 0)

				End If

			Catch e As Exception
				result = Nothing
				err = True
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		''' <summary>
		''' Loads highest ZGNr for save final data.
		''' </summary>
		''' <returns>Highest ZGNr.</returns>
		Function LoadHighestZGNrForSaveFinalData() As Integer Implements IPayrollDatabaseAccess.LoadHighestZGNrForSaveFinalData

			Dim sql As String

			sql = "Select Top 1 [ZGNr] From ZG Order By [ZGNr] DESC"

			Dim zgNr As Integer? = ExecuteScalar(sql, Nothing)

			If zgNr.HasValue Then
				zgNr = zgNr + 1
			Else
				zgNr = 1
			End If

			Return zgNr.Value
		End Function

		''' <summary>
		''' Loads highest LMID for save final data.
		''' </summary>
		''' <returns>Highest ZGNr.</returns>
		Function LoadHighestLMIDForSaveFinalData() As Integer Implements IPayrollDatabaseAccess.LoadHighestLMIDForSaveFinalData

			Dim sql As String

			sql = "Select Top 1 [ID] From LM Order By [ID] DESC"

			Dim lmID As Integer? = ExecuteScalar(sql, Nothing)

			If lmID.HasValue Then
				lmID = lmID
			Else
				lmID = 1
			End If

			Return lmID.Value
		End Function

		''' <summary>
		''' Loads LALoText for save final data.
		''' </summary>
		''' <param name="year">The year.</param>
		''' <returns>The LALoText.</returns>
		Function LoadLALoTextForSaveFinalData(ByVal year As Integer) As String Implements IPayrollDatabaseAccess.LoadLALoTextForSaveFinalData

			Dim result As String = Nothing

			Dim sSql As String

			sSql = "Select TOP 1 LALoText From LA Where LANr = 8100 And LAJahr = @year And LADeactivated = 0"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@year", year))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sSql, listOfParams)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = SafeGetString(reader, "LALoText", String.Empty)

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
		''' Loads LOL data for save final data.
		''' </summary>
		''' <param name="maNr">The MANr.</param>
		''' <param name="mdNr">The MDNr.</param>
		''' <param name="loNr">The LONr.</param>
		''' <param name="month">The month.</param>
		''' <param name="year">The year.</param>
		''' <returns>List of LANr data or nothing in error case.</returns>
		Function LoadLolLANrDataForSaveFinalData(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal loNr As Integer, ByVal month As Integer, ByVal year As Integer) As List(Of Integer) Implements IPayrollDatabaseAccess.LoadLolLANrDataForSaveFinalData

			Dim result As List(Of Integer) = Nothing

			Dim sSql As String = String.Empty

			sSql = "Select LANr From LOL Where LONr = @loNr "
			sSql = sSql & "And LP = @month And Jahr = @year "
			sSql = sSql & "And MANr = @maNr "
			sSql = sSql & "And LANr In (9300, 9200, 9100) "
			sSql = sSql & "And LOL.MDNr = @mdNr "
			sSql = sSql & "Order By LANr Desc"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@loNr", loNr))
			listOfParams.Add(New SqlClient.SqlParameter("@month", month))
			listOfParams.Add(New SqlClient.SqlParameter("@year", year))
			listOfParams.Add(New SqlClient.SqlParameter("@maNr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sSql, listOfParams, CommandType.Text)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of Integer)

					While reader.Read

						Dim laNr As Decimal = SafeGetDecimal(reader, "LANr", 0)

						result.Add(laNr)

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
		''' Loads employee Bank data for payroll.
		''' </summary>
		''' <param name="maNr">The MANr.</param>
		''' <param name="loNr">The LONr.</param>
		''' <returns>Employee bank data or nothing in error case.</returns>
		Function LoadEmployeeBnkDataForPayroll(ByVal maNr As Integer, ByVal loNr As Integer) As EmployeeBnkDataForPayroll Implements IPayrollDatabaseAccess.LoadEmployeeBnkDataForPayroll

			Dim result As EmployeeBnkDataForPayroll = Nothing

			Dim sql As String = "[Get Employee BnkData For Payroll]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@maNr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@loNr", loNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = New EmployeeBnkDataForPayroll

					result.BnkAu = SafeGetBoolean(reader, "BnkAu", False)
					result.DTABCNR = SafeGetInteger(reader, "DTABCNR", Nothing)
					result.BLZ = SafeGetString(reader, "BLZ", String.Empty)
					result.Bank = SafeGetString(reader, "Bank", String.Empty)
					result.BankOrt = SafeGetString(reader, "BankOrt", String.Empty)
					result.Swift = SafeGetString(reader, "Swift", String.Empty)
					result.KontoNr = SafeGetString(reader, "KontoNr", String.Empty)
					result.IBANNr = SafeGetString(reader, "IBANNr", String.Empty)
					result.DTAAdr1 = SafeGetString(reader, "DTAAdr1", String.Empty)
					result.DTAAdr2 = SafeGetString(reader, "DTAAdr2", String.Empty)
					result.DTAAdr3 = SafeGetString(reader, "DTAAdr3", String.Empty)
					result.DTAAdr4 = SafeGetString(reader, "DTAAdr4", String.Empty)

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
		''' Load Tag Geld Betrag for Month.
		''' </summary>
		''' <param name="maNr">The MANr.</param>
		''' <param name="mdNr">The MDNr.</param>
		''' <param name="month">The month.</param>
		''' <param name="year">The year.</param>
		''' <param name="err">Error flag.</param>
		''' <returns>TagGeldBetrag for month.</returns>
		Function LoadTagGeldBetragForMonth(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal month As Integer, ByVal year As Integer, ByVal s_canton As String, ByRef err As Boolean) As TagGeldBetragForMonth Implements IPayrollDatabaseAccess.LoadTagGeldBetragForMonth

			Dim result As TagGeldBetragForMonth = Nothing

			Dim sql As String
			sql = LoadProcedureNameDataForCase(7600.01, err)
			If String.IsNullOrWhiteSpace(sql) Then
				sql = "[Get TagGeldBetrag For Month]"
			Else
				sql = String.Format(sql, s_canton)
			End If
			sql = "[Get TagGeldBetrag For Month]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@iMANr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("@siMonth", month))
			listOfParams.Add(New SqlClient.SqlParameter("@nYear", year))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = New TagGeldBetragForMonth

					result.KTGBetrag = SafeGetDecimal(reader, "KTGBetrag", 0)
					result.SuvaBetrag = SafeGetDecimal(reader, "SuvaBetrag", 0)
					result.KiAuBetrag = SafeGetDecimal(reader, "KiAuBetrag", 0)
					result.OtherServicesAmounts = SafeGetDecimal(reader, "OtherServicesAmounts", 0)
					result.OtherNotDefinedAmounts = SafeGetDecimal(reader, "OtherNotDefinedAmounts", 0)
					result.OtherServicesAmountsLAData = SafeGetString(reader, "OtherServicesAmountsLAData")
					result.OtherNotDefinedAmountsLAData = SafeGetString(reader, "OtherNotDefinedAmountsLAData")

					result.SPayed = SafeGetDecimal(reader, "SPayed", 0)
					result.SBacked = SafeGetDecimal(reader, "SBacked", 0)

				End If

			Catch e As Exception
				result = Nothing
				err = True
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		''' <summary>
		''' Loads QST info.
		''' </summary>
		''' <param name="sCanton">The canton.</param>
		''' <param name="err">The error flag.</param>
		''' <returns>QSt info.</returns>
		''' <remarks></remarks>
		Function LoadQSTInfo(ByVal sCanton As String, ByRef err As Boolean) As TabQSTInfoData Implements IPayrollDatabaseAccess.LoadQSTInfo

			Dim result As TabQSTInfoData = Nothing

			Dim sql As String = "SELECT StdUp, StdDown, DeSameAsCH, JustAtEndBegin, MonthStd, WithFLeistung, CalendarDay, HandleAsAutomation, StdDownAtEndBegin FROM Tab_QSTInfo Where SKanton = @sCanton"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@sCanton", sCanton))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = New TabQSTInfoData

					result.StdUp = SafeGetBoolean(reader, "StdUp", False)
					result.StdDown = SafeGetBoolean(reader, "StdDown", False)
					result.DeSameAsCH = SafeGetBoolean(reader, "DeSameAsCH", False)
					result.JustAtEndBegin = SafeGetBoolean(reader, "JustAtEndBegin", False)
					result.MonthStd = SafeGetDecimal(reader, "MonthStd", 0)
					result.WithFLeistung = SafeGetBoolean(reader, "WithFLeistung", False)
					result.CalendarDay = SafeGetBoolean(reader, "CalendarDay", False)
					result.HandleAsAutomation = SafeGetBoolean(reader, "HandleAsAutomation", True)
					result.StdDownAtEndBegin = SafeGetBoolean(reader, "StdDownAtEndBegin", True)

				End If

			Catch e As Exception
				result = Nothing
				err = True
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		''' <summary>
		''' Loads LOL data for QST canton UR:
		''' </summary>
		''' <param name="loNr">The LONr.</param>
		''' <param name="mdNr">The MDNr.</param>
		''' <returns>LOLData for qst payroll or nothing in error case.</returns>
		Function LoadLOLDataForQSTCantonUR(ByVal loNr As Integer, ByVal mdNr As Integer) As LOLDataForQSTCantonUR Implements IPayrollDatabaseAccess.LoadLOLDataForQSTCantonUR

			Dim result As LOLDataForQSTCantonUR = Nothing

			Dim sql As String = "[Get LOLData For QST Canton UR]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@LONr", loNr))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = New LOLDataForQSTCantonUR

					result.Basis_Brutto = SafeGetDecimal(reader, "Basis_Brutto", 0)
					result.Basis_Rest = SafeGetDecimal(reader, "Basis_Rest", 0)
					result.KiAu_Beitrag = SafeGetDecimal(reader, "KiAu_Beitrag", 0)

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
		''' Loads ES data 1 for QST Form.
		''' </summary>
		''' <param name="maNr">The MANr.</param>
		''' <param name="mdNr">The MDNr.</param>
		''' <returns>List of ES data or nothing in error case.</returns>
		Function LoadESData1ForQSTDataForm(ByVal maNr As Integer, ByVal mdNr As Integer) As IEnumerable(Of ESData1ForQSTDataForm) Implements IPayrollDatabaseAccess.LoadESData1ForQSTDataForm

			Dim result As List(Of ESData1ForQSTDataForm) = Nothing

			Dim sSql As String = String.Empty


			sSql = "Select ES.ESNr, ES.ES_Ab, ES.ES_Ende, ES.ES_Als, Kunden.Firma1 "
			sSql = sSql & "From ES Left Join Kunden On ES.KDNr = Kunden.KDNr "
			sSql = sSql & "Where ES.MANr = @maNr And ES.MDNr = @mdNr "
			sSql = sSql & "Order By ES.ES_Ab Desc"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@maNr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sSql, listOfParams, CommandType.Text)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of ESData1ForQSTDataForm)

					While reader.Read

						Dim data = New ESData1ForQSTDataForm

						data.ESNr = SafeGetInteger(reader, "ESNr", 0)
						data.ES_Ab = SafeGetDateTime(reader, "ES_Ab", Nothing)
						data.ES_Ende = SafeGetDateTime(reader, "ES_Ende", Nothing)
						data.ES_Als = SafeGetString(reader, "ES_Als", String.Empty)
						data.Firma1 = SafeGetString(reader, "Firma1", String.Empty)

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
		''' Loads ES data 2 for QST Form.
		''' </summary>
		''' <param name="maNr">The MANr.</param>
		''' <param name="mdNr">The MDNr.</param>
		''' <returns>List of ES data or nothing in error case.</returns>
		Function LoadESData2ForQSTDataForm(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal startOfMonth As DateTime, ByVal endOfMonth As DateTime) As IEnumerable(Of ESData2ForQSTDataForm) Implements IPayrollDatabaseAccess.LoadESData2ForQSTDataForm

			Dim result As List(Of ESData2ForQSTDataForm) = Nothing

			Dim sSql As String = String.Empty

			sSql = "Select ES.ESNr, ES.ES_Ab, ES.ES_Ende, ESL.Grundlohn From ES "
			sSql = sSql & "Left Join ESLohn ESL On ES.ESNr = ESL.ESNr "
			sSql = sSql & "Where ES.MANr = @MANr and "
			sSql = sSql & "(ES.ES_Ende >=  @startOfMonth "
			sSql = sSql & "Or ES.ES_Ende is Null) "
			sSql = sSql & "And ES.ES_Ab <= @endOfMonth "
			sSql = sSql & "And ESL.AktivLODaten = 1 "
			sSql = sSql & "And ES.MDNr = @mdNr "
			sSql = sSql & "Order By ES.ES_Ab ASC, ES.ES_Ende ASC"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@maNr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("@startOfMonth", startOfMonth))
			listOfParams.Add(New SqlClient.SqlParameter("@endOfMonth", endOfMonth))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sSql, listOfParams, CommandType.Text)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of ESData2ForQSTDataForm)

					While reader.Read

						Dim data = New ESData2ForQSTDataForm

						data.ESNr = SafeGetInteger(reader, "ESNr", 0)
						data.ES_Ab = SafeGetDateTime(reader, "ES_Ab", Nothing)
						data.ES_Ende = SafeGetDateTime(reader, "ES_Ende", Nothing)
						data.Grundlohn = SafeGetDecimal(reader, "Grundlohn", 0)

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
		''' Loads Feiertag Guthaben.
		''' </summary>
		''' <param name="maNr">The MANr.</param>
		''' <param name="esNr">The ESNr.</param>
		''' <returns>The Feiertag Guthaben or nothing in error case.</returns>
		Function LoadFeiertagGuthaben(ByVal maNr As Integer, Optional ByVal esNr As Integer = 0) As GuthabenData Implements IPayrollDatabaseAccess.LoadFeiertagGuthaben

			Dim result As GuthabenData = Nothing

			Dim sql As String = "[Get Feiertag Guthaben]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MANr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@ESNr", esNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = New GuthabenData

					result.BackedBetrag = SafeGetDecimal(reader, "BackedBetrag", 0)
					result.PayedBetrag = SafeGetDecimal(reader, "PayedBetrag", 0)

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
		''' Loads Feiertag Guthaben1.
		''' </summary>
		''' <param name="maNr">The MANr.</param>
		''' <returns>The Feiertag Guthaben1 or nothing in error case.</returns>
		Function LoadFeiertagGuthaben1(ByVal maNr As Integer) As GuthabenData Implements IPayrollDatabaseAccess.LoadFeiertagGuthaben1

			Dim result As GuthabenData = Nothing

			Dim sql As String = "[Get Feiertag Guthaben_1]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MANr", maNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = New GuthabenData

					result.BackedBetrag = SafeGetDecimal(reader, "BackedBetrag", 0)
					result.PayedBetrag = SafeGetDecimal(reader, "PayedBetrag", 0)

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
		''' Loads Ferien Guthaben.
		''' </summary>
		''' <param name="maNr">The MANr.</param>
		''' <param name="esNr">The ESNr.</param>
		''' <returns>The Ferien Guthaben or nothing in error case.</returns>
		Function LoadFerienGuthaben(ByVal maNr As Integer, Optional ByVal esNr As Integer = 0) As GuthabenData Implements IPayrollDatabaseAccess.LoadFerienGuthaben

			Dim result As GuthabenData = Nothing

			Dim sql As String = "[Get Ferien Guthaben]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MANr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@ESNr", esNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = New GuthabenData

					result.BackedBetrag = SafeGetDecimal(reader, "BackedBetrag", 0)
					result.PayedBetrag = SafeGetDecimal(reader, "PayedBetrag", 0)

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
		''' Loads Ferien Guthaben1.
		''' </summary>
		''' <param name="maNr">The MANr.</param>
		''' <returns>The Ferien Guthaben1 or nothing in error case.</returns>
		Function LoadFerienGuthaben1(ByVal maNr As Integer) As GuthabenData Implements IPayrollDatabaseAccess.LoadFerienGuthaben1

			Dim result As GuthabenData = Nothing

			Dim sql As String = "[Get Ferien Guthaben_1]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MANr", maNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = New GuthabenData

					result.BackedBetrag = SafeGetDecimal(reader, "BackedBetrag", 0)
					result.PayedBetrag = SafeGetDecimal(reader, "PayedBetrag", 0)

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
		''' Loads Lohn 13 Guthaben.
		''' </summary>
		''' <param name="maNr">The MANr.</param>
		''' <param name="esNr">The ESNr.</param>
		''' <returns>The Lohn 13 Guthaben or nothing in error case.</returns>
		Function Load13LohnGuthaben(ByVal maNr As Integer, Optional ByVal esNr As Integer = 0) As GuthabenData Implements IPayrollDatabaseAccess.Load13LohnGuthaben

			Dim result As GuthabenData = Nothing

			Dim sql As String = "[Get 13Lohn Guthaben]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MANr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@ESNr", esNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = New GuthabenData

					result.BackedBetrag = SafeGetDecimal(reader, "BackedBetrag", 0)
					result.PayedBetrag = SafeGetDecimal(reader, "PayedBetrag", 0)

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
		''' Loads Lohn 13 Guthaben.
		''' </summary>
		''' <param name="maNr">The MANr.</param>
		''' <returns>The Lohn 13 Guthaben or nothing in error case.</returns>
		Function Load13LohnGuthaben1(ByVal maNr As Integer) As GuthabenData Implements IPayrollDatabaseAccess.Load13LohnGuthaben1

			Dim result As GuthabenData = Nothing

			Dim sql As String = "[Get 13Lohn Guthaben_1]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MANr", maNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = New GuthabenData

					result.BackedBetrag = SafeGetDecimal(reader, "BackedBetrag", 0)
					result.PayedBetrag = SafeGetDecimal(reader, "PayedBetrag", 0)

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
		''' Loads list of all candidates for not created ZV.
		''' </summary>
		'''<param name="username">The user name.</param>
		'''<param name="mdNr">The mandant number.</param>
		''' <returns>List of employee data or nothing in error case.</returns>
		Function LoadListOfAllKandidaten4NotCreatedZV(ByVal username As String, ByVal mdNr As Integer) As IEnumerable(Of EmployeeDataForZV) Implements IPayrollDatabaseAccess.LoadListOfAllKandidaten4NotCreatedZV

			Dim result As List(Of EmployeeDataForZV) = Nothing

			Dim sql As String = "[List All Kandidaten 4 Not Created ZV]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@UserName", username))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeDataForZV)

					While reader.Read

						Dim data = New EmployeeDataForZV

						data.MANr = SafeGetInteger(reader, "MANr", 0)
						data.LastName = SafeGetString(reader, "Nachname")
						data.FirstName = SafeGetString(reader, "Vorname")

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

		Function LoadEmployeesForForgottenZV(ByVal mdNr As Integer, ByVal month As Integer, ByVal year As Integer, ByVal username As String) As IEnumerable(Of EmployeeDataForZV) Implements IPayrollDatabaseAccess.LoadEmployeesForForgottenZV

			Dim result As List(Of EmployeeDataForZV) = Nothing

			Dim sql As String = "[Load Employees For Forgotten ZV-ARGB Print]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("MDNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("year", ReplaceMissing(year, Now.Year)))
			listOfParams.Add(New SqlClient.SqlParameter("month", ReplaceMissing(month, Now.Month)))
			listOfParams.Add(New SqlClient.SqlParameter("UserName", ReplaceMissing(username, String.Empty)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeDataForZV)

					While reader.Read

						Dim data = New EmployeeDataForZV

						data.MANr = SafeGetInteger(reader, "MANr", 0)
						data.LastName = SafeGetString(reader, "Nachname")
						data.FirstName = SafeGetString(reader, "Vorname")
						data.ZVPrinted = SafeGetBoolean(reader, "ZVPrinted", False)
						data.ARGBPrinted = SafeGetBoolean(reader, "ARGBPrinted", False)


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
		''' Loads list of all candidates for contolling payroll.
		''' </summary>
		''' <returns>List of employee data or nothing in error case.</returns>
		Function LoadSuspectPayrollsAfterCreate(ByVal mdNr As Integer, ByVal loNr As Integer()) As IEnumerable(Of SuspectPayrollData) Implements IPayrollDatabaseAccess.LoadSuspectPayrollsAfterCreate

			Dim result As List(Of SuspectPayrollData) = Nothing

			Dim loNrBuffer As New StringBuilder

			For i As Integer = 0 To loNr.Count - 1
				loNrBuffer.Append(loNr(i))

				If i < loNr.Count - 1 Then
					loNrBuffer.Append(",")
				End If
			Next

			Dim sql As String = "[List All Suspected Candidates After Created Payrolls]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("@LONumbers", loNrBuffer.ToString))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				result = New List(Of SuspectPayrollData)
				If (Not reader Is Nothing) Then

					While reader.Read

						Dim data = New SuspectPayrollData

						data.LONr = SafeGetInteger(reader, "LONr", 0)
						data.MANr = SafeGetInteger(reader, "MANr", 0)
						data.LANr = SafeGetDecimal(reader, "LANr", 0)
						data.M_Ans = SafeGetDecimal(reader, "M_ANS", 0)
						data.LastName = SafeGetString(reader, "Nachname")
						data.FirstName = SafeGetString(reader, "Vorname")

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
		''' Loads list of all candidates for printing check and cash.
		''' </summary>
		''' <returns>List of employee data or nothing in error case.</returns>
		Function LoadEmployeesForPrintCheckCashAfterPayroll(ByVal mdNr As Integer, ByVal loNr As Integer()) As IEnumerable(Of PayrollCheckCashData) Implements IPayrollDatabaseAccess.LoadEmployeesForPrintCheckCashAfterPayroll

			Dim result As List(Of PayrollCheckCashData) = Nothing

			Dim loNrBuffer As New StringBuilder

			For i As Integer = 0 To loNr.Count - 1
				loNrBuffer.Append(loNr(i))

				If i < loNr.Count - 1 Then
					loNrBuffer.Append(",")
				End If
			Next

			Dim sql As String = "[List Employees For Print Check And Cash After Created Payrolls]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("@LONumbers", loNrBuffer.ToString))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				result = New List(Of PayrollCheckCashData)

				If (Not reader Is Nothing) Then

					While reader.Read

						Dim data = New PayrollCheckCashData

						data.LONr = SafeGetInteger(reader, "LONr", 0)
						data.MANr = SafeGetInteger(reader, "MANr", 0)
						data.ZGNr = SafeGetInteger(reader, "ZGNr", 0)
						data.LANr = SafeGetDecimal(reader, "LANr", 0)
						data.LALabel = SafeGetString(reader, "LALOText")
						data.Betrag = SafeGetDecimal(reader, "Betrag", 0)
						data.LastName = SafeGetString(reader, "Nachname")
						data.FirstName = SafeGetString(reader, "Vorname")

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
		''' Loads employee data for not created ZV.
		''' </summary>
		'''<param name="employeeNumbers">The employee numbers.</param>
		''' <returns>List of employee data or nothing in error case.</returns>
		Function LoadEmployeeData4NotCreatedZV(ByVal employeeNumbers As Integer()) As IEnumerable(Of EmployeeDataForZV) Implements IPayrollDatabaseAccess.LoadEmployeeData4NotCreatedZV

			Dim result As List(Of EmployeeDataForZV) = Nothing
			Dim maNrBuffer As New StringBuilder

			For i As Integer = 0 To employeeNumbers.Count - 1
				maNrBuffer.Append(employeeNumbers(i))

				If i < employeeNumbers.Count - 1 Then
					maNrBuffer.Append(",")
				End If
			Next

			Dim sSql As String = String.Empty
			sSql = sSql & "Select MA.MANr, MA.Nachname, MA.Vorname "
			sSql = sSql & "From Mitarbeiter MA Left Join MAKontakt_Komm On "
			sSql = sSql & "MA.MANr = MAKontakt_Komm.MANr "
			sSql = sSql & "Where MA.MANr In (" & maNrBuffer.ToString() & ") And "
			sSql = sSql & "MAKontakt_Komm.InZv = 1 "
			sSql = sSql & "Order By MA.Nachname ASC, MA.Vorname ASC"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sSql, Nothing, CommandType.Text)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeDataForZV)

					While reader.Read

						Dim data = New EmployeeDataForZV

						data.MANr = SafeGetInteger(reader, "MANr", 0)
						data.LastName = SafeGetString(reader, "Nachname")
						data.FirstName = SafeGetString(reader, "Vorname")

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
		''' Checks if an LO is finished.
		''' </summary>
		''' <param name="loNr">The LONr.</param>
		''' <param name="mdNr">The MDNr.</param>
		''' <returns>Boolean flag indicating if LO is finished.</returns>
		Function IsLOFinished(ByVal loNr As Integer, ByVal mdNr As Integer) As Boolean? Implements IPayrollDatabaseAccess.IsLOFinished

			Dim doesExist = False

			Dim sSql As String = String.Empty

			sSql = sSql & "Select Count(LANr) From LOL Where LANr In (9100, 9200, 9300, 9500, 9600) "
			sSql = sSql & "And LONr = @LONr "
			sSql = sSql & "And LOL.MDNr = @MDNr "

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@LONr", loNr))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", mdNr))

			Dim existingLOLRecordsCount = ExecuteScalar(sSql, listOfParams)

			If Not existingLOLRecordsCount Is Nothing Then
				doesExist = (existingLOLRecordsCount > 0)

				Return doesExist
			Else
				Return Nothing
			End If

		End Function

		''' <summary>
		''' Cleans UP LO.
		''' </summary>
		''' <param name="maNr">The MANr.</param>
		''' <param name="mdNr">The MDNr.</param>
		''' <param name="loNr">The LONr.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Function CleanupLO(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal loNr As Integer) As Boolean Implements IPayrollDatabaseAccess.CleanupLO

			Dim success = True

			Dim sql As String = "[CleanUP LO]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MANR", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNR", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("@LONR", loNr))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Return success


		End Function

		''' <summary>
		''' CleanUp all invalid LO.
		''' </summary>
		Function CleanupAllInvalidLO(ByVal mandantNumber As Integer, ByVal year As Integer, ByVal month As Integer) As Boolean Implements IPayrollDatabaseAccess.CleanupAllInvalidLO

			Dim success = True

			Dim sql As String = "[CleanUP All Invalid LO]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", mandantNumber))
			listOfParams.Add(New SqlClient.SqlParameter("@Year", year))
			listOfParams.Add(New SqlClient.SqlParameter("@Month", month))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Return success

		End Function

		''' <summary>
		''' Adds a new LO.
		''' </summary>
		''' <param name="loMasterData">The LO master data.</param>
		''' <returns>Boolean flag indicating succes.</returns>
		Function AddNewLO(ByVal loMasterData As LOMasterData) As Boolean Implements IPayrollDatabaseAccess.AddNewLO

			Dim success = True

			Dim sql As String

			sql = "[Create New LO]"

			' Parameters

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			' Data of LO
			listOfParams.Add(New SqlClient.SqlParameter("@LONR", ReplaceMissing(loMasterData.LONR, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MANR", ReplaceMissing(loMasterData.MANR, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MAName", ReplaceMissing(loMasterData.MAName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LP", ReplaceMissing(loMasterData.LP, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Jahr", ReplaceMissing(loMasterData.Jahr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KST", ReplaceMissing(loMasterData.KST, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@S_Kanton", ReplaceMissing(loMasterData.S_Kanton, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Zivilstand", ReplaceMissing(loMasterData.Zivilstand, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Kirchensteuer", ReplaceMissing(loMasterData.Kirchensteuer, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Q_Steuer", ReplaceMissing(loMasterData.Q_Steuer, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@AnzahlKinder", ReplaceMissing(loMasterData.AnzahlKinder, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Wohnort", ReplaceMissing(loMasterData.Wohnort, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Land", ReplaceMissing(loMasterData.Land, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@WorkedDays", ReplaceMissing(loMasterData.WorkedDays, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bruttolohn", ReplaceMissing(loMasterData.Bruttolohn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@AHV_Basis", ReplaceMissing(loMasterData.AHV_Basis, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@AHV_Lohn", ReplaceMissing(loMasterData.AHV_Lohn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@AHV_Freibetrag", ReplaceMissing(loMasterData.AHV_Freibetrag, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Nicht_AHV_pflichtig", ReplaceMissing(loMasterData.Nicht_AHV_pflichtig, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ALV1_Lohn", ReplaceMissing(loMasterData.ALV1_Lohn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ALV2_Lohn", ReplaceMissing(loMasterData.ALV2_Lohn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@SUVA_Basis", ReplaceMissing(loMasterData.SUVA_Basis, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Result", ReplaceMissing(loMasterData.Result, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LOKst1", ReplaceMissing(loMasterData.LOKst1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LOKst2", ReplaceMissing(loMasterData.LOKst2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ZGNr", ReplaceMissing(loMasterData.ZGNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LMID", ReplaceMissing(loMasterData.LMID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@CreatedOn", ReplaceMissing(loMasterData.CreatedOn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@CreatedFrom", ReplaceMissing(loMasterData.CreatedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@QSTBasis", ReplaceMissing(loMasterData.QSTBasis, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@QSTTarif", ReplaceMissing(loMasterData.QSTTarif, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@WorkedDate", ReplaceMissing(loMasterData.WorkedDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@DTADate", ReplaceMissing(loMasterData.DTADate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ESData", ReplaceMissing(loMasterData.ESData, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@BVGBeginn", ReplaceMissing(loMasterData.BVGBeginn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@BVGEnde", ReplaceMissing(loMasterData.BVGEnde, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MData", ReplaceMissing(loMasterData.MData, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LODoc_Guid", ReplaceMissing(loMasterData.LODoc_Guid, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Transfered_User", ReplaceMissing(loMasterData.Transfered_User, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Transfered_On", ReplaceMissing(loMasterData.Transfered_On, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@IsComplete", ReplaceMissing(loMasterData.IsComplete, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@BVGBegin", ReplaceMissing(loMasterData.BVGBegin, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@BVGEnd", ReplaceMissing(loMasterData.BVGEnd, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", ReplaceMissing(loMasterData.MDNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CreatedUserNumber", ReplaceMissing(loMasterData.CreatedUserNumber, DBNull.Value)))

			' New ID of LO
			Dim newIdParameter = New SqlClient.SqlParameter("@NewLOID", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If success AndAlso
			Not newIdParameter.Value Is Nothing Then
				loMasterData.ID = CType(newIdParameter.Value, Integer)
			Else
				success = False
			End If

			Return success

		End Function

		''' <summary>
		''' Adds the end data to the LO record.
		''' </summary>
		''' <param name="data">The data.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Function AddEndDataToLOrec(ByVal data As EndDataForLO) As Boolean Implements IPayrollDatabaseAccess.AddEndDataToLOrec

			Dim success = True

			Dim sql As String

			sql = "[Add EndData To LOrec For Payroll]"

			' Parameters

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			' Data of parameters
			listOfParams.Add(New SqlClient.SqlParameter("@LONewNr", ReplaceMissing(data.LONewNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MANummer", ReplaceMissing(data.MANummer, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Kst1", ReplaceMissing(data.Kst1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Kst2", ReplaceMissing(data.Kst2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LP", ReplaceMissing(data.LP, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Jahr", ReplaceMissing(data.Jahr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@S_Kanton", ReplaceMissing(data.S_Kanton, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@QSTTarif", ReplaceMissing(data.QSTTarif, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Zivilstand", ReplaceMissing(data.Zivilstand, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Kirchensteuer", ReplaceMissing(data.Kirchensteuer, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Q_Steuer", ReplaceMissing(data.Q_Steuer, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Kinder", ReplaceMissing(data.Kinder, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@QSTBasis", ReplaceMissing(data.QSTBasis, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@strESData", ReplaceMissing(data.strESData, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Wohnort", ReplaceMissing(data.Wohnort, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@CHPartner", ReplaceMissing(data.CHPartner, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@NoSpecialTax", ReplaceMissing(data.NoSpecialTax, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Permission", ReplaceMissing(data.Permission, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@PermissionToDate", ReplaceMissing(data.PermissionToDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@EmployeePartnerRecID", ReplaceMissing(data.EmployeePartnerRecID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("EmployeeLOHistoryID", ReplaceMissing(data.EmployeeLOHistoryRecID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@WorkedDay", ReplaceMissing(data.WorkedDay, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Land", ReplaceMissing(data.Land, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Brutto", ReplaceMissing(data.Brutto, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@AHVBas", ReplaceMissing(data.AHVBas, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@AHVLohn", ReplaceMissing(data.AHVLohn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@AHVFrei", ReplaceMissing(data.AHVFrei, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@NAHVPf", ReplaceMissing(data.NAHVPf, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ALV1Lohn", ReplaceMissing(data.ALV1Lohn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ALV2Lohn", ReplaceMissing(data.ALV2Lohn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@SUVABas", ReplaceMissing(data.SUVABas, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MAName", ReplaceMissing(data.MAName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@BVGBeginn", ReplaceMissing(data.BVGBeginn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@BVGEnde", ReplaceMissing(data.BVGEnde, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MData", ReplaceMissing(data.MData, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ZGNumber", ReplaceMissing(data.ZGNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@BVGBeginNew", ReplaceMissing(data.BVGBegin, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@BVGEndNew", ReplaceMissing(data.BVGEnd, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@BVGDateData", ReplaceMissing(data.BVGDateData, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", ReplaceMissing(data.MDNr, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Return success

		End Function

		''' <summary>
		''' Adds LM data for save final data.
		''' </summary>
		''' <param name="data">The data.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Function AddLMDataForSaveFinalData(ByVal data As LMDataForSaveFinalData) As Boolean Implements IPayrollDatabaseAccess.AddLMDataForSaveFinalData

			Dim success = True

			Dim sql As String

			sql = "Insert Into LM (LMNr, MANr, ESNr, LMKst1, LMKst2, KST, LANr, LP_Von, LP_Bis, "
			sql = sql & "[Jahr von], [Jahr Bis], Suva, M_Anz, M_Bas, M_Ans, M_Btr, LAName, "
			sql = sql & "[Far-pflicht], MDNr, CreatedFrom, CreatedOn, ChangedFrom, ChangedOn) "
			sql = sql & "Values (@LMNr, @MANr, @ESNr, @LMKst1, @LMKst2, @KST, @LANr, @LP_Von, @LP_Bis, "
			sql = sql & "@Jahr_von, @Jahr_bis, @Suva, @M_Anz, @M_Bas, @M_Ans, @M_Btr, @LAName, @Farpflicht, "
			sql = sql & "@MDNr, @CreatedFrom, @CreatedOn, @ChangedFrom, @ChangedOn)"

			' Parameters

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			' Data of parameters
			listOfParams.Add(New SqlClient.SqlParameter("@LMNr", ReplaceMissing(data.LMNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MANr", ReplaceMissing(data.MANr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ESNr", ReplaceMissing(data.ESNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LMKst1", ReplaceMissing(data.LMKst1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LMKst2", ReplaceMissing(data.LMKst2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KST", ReplaceMissing(data.KST, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LANr", ReplaceMissing(data.LANr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LP_Von", ReplaceMissing(data.LP_Von, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LP_Bis", ReplaceMissing(data.LP_Bis, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Jahr_von", ReplaceMissing(data.Jahr_von, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Jahr_Bis", ReplaceMissing(data.Jahr_Bis, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Suva", ReplaceMissing(data.Suva, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@M_Anz", ReplaceMissing(data.M_Anz, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@M_Bas", ReplaceMissing(data.M_Bas, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@M_Ans", ReplaceMissing(data.m_Ans, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@M_Btr", ReplaceMissing(data.M_Btr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LAName", ReplaceMissing(data.LAName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Farpflicht", ReplaceMissing(data.FarPflicht, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", ReplaceMissing(data.MDNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@CreatedFrom", ReplaceMissing(data.CreatedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@CreatedOn", ReplaceMissing(data.CreatedOn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ChangedFrom", ReplaceMissing(data.ChangedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ChangedOn", ReplaceMissing(data.ChangedOn, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

			Return success


		End Function

		''' <summary>
		''' Adds a new LOL record.
		''' </summary>
		''' <param name="lolMasterData">The LOL master data.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Function AddNewLOL(ByVal lolMasterData As LOLMasterData) As Boolean Implements IPayrollDatabaseAccess.AddNewLOL

			Dim success = True

			Dim sql As String

			sql = "[Create New LOL]"

			' Parameters

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			' Data of LOL
			listOfParams.Add(New SqlClient.SqlParameter("@LONR", ReplaceMissing(lolMasterData.LONR, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MANR", ReplaceMissing(lolMasterData.MANR, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LANR", ReplaceMissing(lolMasterData.LANR, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LP", ReplaceMissing(lolMasterData.LP, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Jahr", ReplaceMissing(lolMasterData.Jahr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ModulName", ReplaceMissing(lolMasterData.ModulName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Currency", ReplaceMissing(lolMasterData.Currency, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@M_ANZ", ReplaceMissing(lolMasterData.M_ANZ, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@M_BAS", ReplaceMissing(lolMasterData.M_BAS, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@M_ANS", ReplaceMissing(lolMasterData.M_ANS, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@M_BTR", ReplaceMissing(lolMasterData.M_BTR, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@SUVA", ReplaceMissing(lolMasterData.SUVA, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KST", ReplaceMissing(lolMasterData.KST, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@RPText", ReplaceMissing(lolMasterData.RPText, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@AGLA", ReplaceMissing(lolMasterData.AGLA, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@S_Kanton", ReplaceMissing(lolMasterData.S_Kanton, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Result", ReplaceMissing(lolMasterData.Result, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KW", ReplaceMissing(lolMasterData.KW, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LOLKst1", ReplaceMissing(lolMasterData.LOLKst1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LOLKst2", ReplaceMissing(lolMasterData.LOLKst2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@DestRPNr", ReplaceMissing(lolMasterData.DestRPNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@DestZGNr", ReplaceMissing(lolMasterData.DestZGNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@DestLMNr", ReplaceMissing(lolMasterData.DestLMNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KW2", ReplaceMissing(lolMasterData.KW2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ZGAusDate", ReplaceMissing(lolMasterData.ZGAusDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LMWithDTA", ReplaceMissing(lolMasterData.LMWithDTA, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ZGGrund", ReplaceMissing(lolMasterData.ZGGrund, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@BnkNr", ReplaceMissing(lolMasterData.BnkNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@VGNr", ReplaceMissing(lolMasterData.VGNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@DTADate", ReplaceMissing(lolMasterData.DTADate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@GAVNr", ReplaceMissing(lolMasterData.GAVNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@GAV_Kanton", ReplaceMissing(TrimNonNull(lolMasterData.GAV_Kanton), DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@GAV_Beruf", ReplaceMissing(TrimNonNull(lolMasterData.GAV_Beruf), DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@GAV_Gruppe1", ReplaceMissing(TrimNonNull(lolMasterData.GAV_Gruppe1), DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@GAV_Gruppe2", ReplaceMissing(TrimNonNull(lolMasterData.GAV_Gruppe2), DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@GAV_Gruppe3", ReplaceMissing(TrimNonNull(lolMasterData.GAV_Gruppe3), DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@GAV_Text", ReplaceMissing(TrimNonNull(lolMasterData.GAV_Text), DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@DestESNr", ReplaceMissing(lolMasterData.DestESNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@DateOfLO", ReplaceMissing(lolMasterData.DateOfLO, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@QSTGemeinde", ReplaceMissing(lolMasterData.QSTGemeinde, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@DestKDNr", ReplaceMissing(lolMasterData.DestKDNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ESBranche", ReplaceMissing(lolMasterData.ESBranche, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ESEinstufung", ReplaceMissing(lolMasterData.ESEinstufung, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", ReplaceMissing(lolMasterData.MDNr, DBNull.Value)))

			' New ID of LOL
			Dim newIdParameter = New SqlClient.SqlParameter("@NewLOLID", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)
			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If success AndAlso
			Not newIdParameter.Value Is Nothing Then
				lolMasterData.ID = CType(newIdParameter.Value, Integer)
			Else
				success = False
			End If

			Return success
		End Function

		''' <summary>
		''' Adds year cumulative data.
		''' </summary>
		''' <param name="yearCumulative">The year cumulative data.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Function AddYearCumulativeData(ByVal yearCumulative As YearCumulativeData) As Boolean Implements IPayrollDatabaseAccess.AddYearCumulativeData

			Dim success = True

			Dim sql As String

			sql = "[Add YearKumulativ For Payroll]"

			' Parameters

			Dim listOfParams As New List(Of SqlClient.SqlParameter)


			' Data of parameters
			listOfParams.Add(New SqlClient.SqlParameter("@MANummer", ReplaceMissing(yearCumulative.MANummer, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LONewNr", ReplaceMissing(yearCumulative.LONewNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LANr", ReplaceMissing(yearCumulative.LANr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KumLANr", ReplaceMissing(yearCumulative.KumLANr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Kst1", ReplaceMissing(yearCumulative.Kst1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Kst2", ReplaceMissing(yearCumulative.Kst2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LP", ReplaceMissing(yearCumulative.LP, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MDYear", ReplaceMissing(yearCumulative.MDYear, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ModulName", ReplaceMissing(yearCumulative.ModulName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@AGLA", ReplaceMissing(yearCumulative.AGLA, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@DateOfLO ", ReplaceMissing(yearCumulative.DateOfLO, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", ReplaceMissing(yearCumulative.MDNr, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Return success

		End Function

		''' <summary>
		''' Adds month cumulative data.
		''' </summary>
		''' <param name="monthCumulative">The month cumulative data.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Function AddMonthCumulativeData(ByVal monthCumulative As MonthCumulativeData) As Boolean Implements IPayrollDatabaseAccess.AddMonthCumulativeData

			Dim success = True

			Dim sql As String

			sql = "[Add MonthKumulativ For Payroll]"

			' Parameters

			Dim listOfParams As New List(Of SqlClient.SqlParameter)


			' Data of parameters
			listOfParams.Add(New SqlClient.SqlParameter("@MANummer", ReplaceMissing(monthCumulative.MANummer, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LONewNr", ReplaceMissing(monthCumulative.LONewNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LANr", ReplaceMissing(monthCumulative.LANr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KumLANr", ReplaceMissing(monthCumulative.KumLANr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Kst1", ReplaceMissing(monthCumulative.Kst1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Kst2", ReplaceMissing(monthCumulative.Kst2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LP", ReplaceMissing(monthCumulative.LP, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MDYear", ReplaceMissing(monthCumulative.MDYear, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ModulName", ReplaceMissing(monthCumulative.ModulName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@AGLA", ReplaceMissing(monthCumulative.AGLA, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LAName", ReplaceMissing(monthCumulative.LAName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@DateOfLO", ReplaceMissing(monthCumulative.DateOfLO, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", ReplaceMissing(monthCumulative.MDNr, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Return success

		End Function

		Function LoadLATotalBasisInMonth(ByVal laData As MonthCumulativeData) As Decimal? Implements IPayrollDatabaseAccess.LoadLATotalBasisInMonth

			Dim sSql As String

			sSql = "Select ISNULL(Sum(LOL.M_Bas),0) As TotalLaAmount From LOL "
			sSql &= "Where "
			sSql &= "LOL.MDNr = @mdNr "
			sSql &= "And LONr = @LONr "
			sSql &= "And MANr = @maNr "
			sSql &= "And LOL.LANr = @LANr "

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", laData.MDNr))
			listOfParams.Add(New SqlClient.SqlParameter("@LONr", laData.LONewNr))
			listOfParams.Add(New SqlClient.SqlParameter("@maNr", laData.MANummer))
			listOfParams.Add(New SqlClient.SqlParameter("@lanr", laData.LANr))

			Dim laAmount As Decimal? = ExecuteScalar(sSql, listOfParams)

			Return laAmount

		End Function


		''' <summary>
		''' Loads GAV Gruppe0 data.
		''' </summary>
		''' <param name="month">The month.</param>
		''' <param name="year">The year.</param>
		''' <param name="maNr">The employee number.</param>
		''' <param name="mdNr">The mandant number.</param>
		''' <returns>Array of Gruppe0 data.</returns>
		Function LoadGAVGruppe0Data(ByVal month As Integer, ByVal year As Integer, ByVal maNr As Integer, ByVal mdNr As Integer) As List(Of GAVNumberLabelData) Implements IPayrollDatabaseAccess.LoadGAVGruppe0Data

			Dim result As New List(Of GAVNumberLabelData)

			'Dim result(50) As String

			'For i As Integer = 0 To result.Count - 1
			'	result(i) = String.Empty
			'Next

			'Dim tempList As New List(Of String)
			Dim gavData As New List(Of GAVNumberLabelData)

			Dim sSql As String

			sSql = "Select RPGAV_Nr, RPGAV_Beruf From RP Where RPGAV_Beruf <> '' And "
			sSql = sSql & "Monat = @month And Jahr = @year And "
			sSql = sSql & "MANr = @maNr "
			sSql = sSql & "And RP.MDNr = @mdNr "
			sSql = sSql & "Group By RPGAV_Nr, RPGAV_Beruf Order By RPGAV_Nr"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@month", Convert.ToString(month)))
			listOfParams.Add(New SqlClient.SqlParameter("@year", Convert.ToString(year)))
			listOfParams.Add(New SqlClient.SqlParameter("@maNr", Convert.ToString(maNr)))
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", Convert.ToString(mdNr)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sSql, listOfParams, CommandType.Text)

			If (reader Is Nothing) Then
				Return Nothing
			End If

			Try
				While reader.Read
					Dim tmpData As New GAVNumberLabelData

					tmpData.GAVGruppe0 = SafeGetString(reader, "RPGAV_Beruf", String.Empty)
					tmpData.GAVNumber = SafeGetInteger(reader, "RPGAV_Nr", 0)

					gavData.Add(tmpData)

				End While
			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				result = Nothing
				Return result
			Finally
				CloseReader(reader)
			End Try

			sSql = "SELECT IsNull(LM.GAVNr, 0) GAVNr, LM.GAVGruppe0 FROM LM "
			sSql = sSql & "WHERE LM.MANr = @maNr And "
			sSql = sSql & "(LM.[Jahr Von] <= @year And LM.[Jahr Bis] >= @year) And "
			sSql = sSql & "((LM.[LP_Von] <= @month Or LM.[Jahr Von] < @year) "
			sSql = sSql & "And (LM.[LP_Bis] >= @month or LM.[Jahr Bis] > @year)) "
			sSql = sSql & "And Not Exists(Select LO.MANr From LO where LO.MANr = @maNr "
			sSql = sSql & "And LO.LP = @month And LO.Jahr = @year And LO.MDNr = @mdNr) "
			sSql = sSql & "And LM.GAVGruppe0 <> '' "
			sSql = sSql & "And LM.MDNr = @mdNr "
			sSql = sSql & "Group By LM.GAVNr, LM.GAVGruppe0 "
			sSql = sSql & "Order By IsNull(LM.GAVNr, 0) ASC"

			' Parameters
			listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@month", Convert.ToString(month)))
			listOfParams.Add(New SqlClient.SqlParameter("@year", Convert.ToString(year)))
			listOfParams.Add(New SqlClient.SqlParameter("@maNr", Convert.ToString(maNr)))
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", Convert.ToString(mdNr)))

			reader = OpenReader(sSql, listOfParams, CommandType.Text)

			If (reader Is Nothing) Then
				Return Nothing
			End If

			Try
				While reader.Read
					Dim tmpData As New GAVNumberLabelData

					tmpData.GAVGruppe0 = SafeGetString(reader, "GAVGruppe0", String.Empty)
					tmpData.GAVNumber = SafeGetInteger(reader, "GAVNr", 0)

					Dim foundedData = gavData.Find(Function(c) c.GAVNumber = tmpData.GAVNumber AndAlso c.GAVGruppe0 = tmpData.GAVGruppe0)
					If foundedData Is Nothing Then gavData.Add(tmpData)

				End While
			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				result = Nothing
				Return result
			Finally
				CloseReader(reader)
			End Try

			'   Dim stringArray = tempList.Distinct().OrderBy(Function(str) str).ToArray()

			'   For i As Integer = 0 To stringArray.Length - 1

			'     If i >= result.Length Then Exit For

			'	result(i + 1) = stringArray(i)
			'Next

			'For i As Integer = 0 To tempList.Count - 1

			'	If i >= result.Length Then Exit For

			'	result(i + 1) = tempList.Values(i)
			'Next

			result = gavData


			Return result

		End Function

		''' <summary>
		''' Loads GAV Gruppe1 data.
		''' </summary>
		''' <param name="month">The month.</param>
		''' <param name="year">The year.</param>
		''' <param name="maNr">The employee number.</param>
		''' <param name="mdNr">The mandant number.</param>
		''' <returns>Array of Gruppe1 data.</returns>
		Function LoadGAVGruppe1Data(ByVal month As Integer, ByVal year As Integer, ByVal maNr As Integer, ByVal mdNr As Integer) As String() Implements IPayrollDatabaseAccess.LoadGAVGruppe1Data

			Dim result(26) As String

			For i As Integer = 0 To result.Count - 1
				result(i) = String.Empty
			Next

			Dim tempList As New List(Of String)
			'Dim tempList As New Dictionary(Of Integer, String)

			Dim sSql As String

			sSql = "Select RPGAV_Gruppe1 From RP Where RPGAV_Gruppe1 <> '' And "
			sSql = sSql & "Monat = @month And Jahr = @year And "
			sSql = sSql & "MANr = @maNr "
			sSql = sSql & "And RP.MDNr = @mdNr "
			sSql = sSql & "Group By RPGAV_Gruppe1 Order By RPGAV_Gruppe1"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@month", Convert.ToString(month)))
			listOfParams.Add(New SqlClient.SqlParameter("@year", Convert.ToString(year)))
			listOfParams.Add(New SqlClient.SqlParameter("@maNr", Convert.ToString(maNr)))
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", Convert.ToString(mdNr)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sSql, listOfParams, CommandType.Text)

			If (reader Is Nothing) Then
				Return Nothing
			End If

			Try

				While reader.Read
					Dim gavGruppe1 As String = SafeGetString(reader, "RPGAV_Gruppe1", String.Empty)
					tempList.Add(gavGruppe1)
					'tempList.Add(gavGruppe1)
				End While
			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				result = Nothing
				Return result
			Finally
				CloseReader(reader)
			End Try

			sSql = "SELECT LM.GAVGruppe1 FROM LM "
			sSql = sSql & "WHERE LM.MANr = @maNr And "
			sSql = sSql & "(LM.[Jahr Von] <= @year And LM.[Jahr Bis] >= @year) And "
			sSql = sSql & "((LM.[LP_Von] <= @month Or LM.[Jahr Von] < @year) "
			sSql = sSql & "And (LM.[LP_Bis] >= @month or LM.[Jahr Bis] > @year)) "
			sSql = sSql & "And Not Exists(Select LO.MANr From LO where LO.MANr = @maNr And LO.LP = @month "
			sSql = sSql & "And LO.Jahr = @year And LO.MDNr = @mdNr) "
			sSql = sSql & "And LM.GAVGruppe1 <> '' "
			sSql = sSql & "And LM.MDNr = @mdNr "
			sSql = sSql & "Group By LM.GAVGruppe1 "
			sSql = sSql & "Order By LM.GAVGruppe1 ASC"

			' Parameters
			listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@month", Convert.ToString(month)))
			listOfParams.Add(New SqlClient.SqlParameter("@year", Convert.ToString(year)))
			listOfParams.Add(New SqlClient.SqlParameter("@maNr", Convert.ToString(maNr)))
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", Convert.ToString(mdNr)))

			reader = OpenReader(sSql, listOfParams, CommandType.Text)

			If (reader Is Nothing) Then
				Return Nothing
			End If

			Try
				While reader.Read
					Dim gavGruppe1 = SafeGetString(reader, "GAVGruppe1", String.Empty)
					tempList.Add(gavGruppe1)
				End While
			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				result = Nothing
				Return result
			Finally
				CloseReader(reader)
			End Try

			Dim stringArray = tempList.Distinct().OrderBy(Function(str) str).ToArray()

			For i As Integer = 0 To stringArray.Length - 1

				If i >= result.Length Then Exit For

				result(i + 1) = stringArray(i)
			Next

			Return result

		End Function

		''' <summary>
		''' Update ZG Data with LONr for payroll.
		''' </summary>
		''' <param name="loNewNr">The LONr.</param>
		''' <param name="zgNr">The ZGNr.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Function UpdateZGDataWithLONrForPayroll(ByVal loNewNr As Integer, ByVal zgNr As Integer) As Boolean Implements IPayrollDatabaseAccess.UpdateZGDataWithLONrForPayroll

			Dim success = True

			Dim sql As String = String.Empty

			sql = sql & "Update ZG Set LONr = @LONewNr Where ZGNr = @ZGNr"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@LONewNr", ReplaceMissing(loNewNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ZGNr", ReplaceMissing(zgNr, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

			Return success

		End Function

		''' <summary>
		''' Update RP Data with LONr for payroll.
		''' </summary>
		''' <param name="loNewNr">The LONr.</param>
		''' <param name="rpNr">The RPNr.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Function UpdateRPDataWithLONrForPayroll(ByVal loNewNr As Integer, ByVal rpNr As Integer) As Boolean Implements IPayrollDatabaseAccess.UpdateRPDataWithLONrForPayroll

			Dim success = True

			Dim sql As String = String.Empty

			sql = sql & "Update RP Set LONr = @LONewNr Where RPNr = @RPNr"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@LONewNr", ReplaceMissing(loNewNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@RPNr", ReplaceMissing(rpNr, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

			Return success

		End Function

		''' <summary>
		''' Updates LOL data for GetLMDTAStatus.
		''' </summary>
		''' <param name="maNr">The MANr.</param>
		''' <param name="mdNr">The MDNr.</param>
		''' <param name="loNr">The LOLNr.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Function UpdateLOLDataForGetLMDTAStatus(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal loNr As Integer) As Boolean Implements IPayrollDatabaseAccess.UpdateLOLDataForGetLMDTAStatus

			Dim success = True

			Dim sql As String = String.Empty

			sql = sql & "Update LOL Set LMWithDTA = 0, ZGGrund = '', BnkNr = 0 "
			sql = sql & "Where LONr = @loNr And MANr = @maNr "
			sql = sql & "And LANr = 8720 And LMWithDTA = 1 And MDNr = @mdNr"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@maNr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("@loNr", loNr))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

			Return success

		End Function

		''' <summary>
		''' Updates LO for save final data.
		''' </summary>
		''' <param name="lmId">The LMId.</param>
		''' <param name="loNr">The LONr.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Function UpdateLOForSaveFinalData(ByVal lmId As Integer, ByVal loNr As Integer) As Boolean Implements IPayrollDatabaseAccess.UpdateLOForSaveFinalData

			Dim success = True

			Dim sql As String = String.Empty

			sql = sql & "Update LO Set LMID = @lmID Where LONr = @loNr"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@lmId", lmId))
			listOfParams.Add(New SqlClient.SqlParameter("@loNr", loNr))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

			Return success
		End Function

		''' <summary>
		''' Updates LOL for save final data.
		''' </summary>
		''' <param name="maNr">The MANr.</param>
		''' <param name="loNr">The LONr.</param>
		''' <param name="rpText">The RPText.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Function UpdateLOLForSaveFinalData(ByVal maNr As Integer, ByVal loNr As Integer, ByVal rpText As String) As Boolean Implements IPayrollDatabaseAccess.UpdateLOLForSaveFinalData

			Dim success = True

			Dim sql As String = String.Empty

			sql = sql & "Update LOL Set LANr = 9200, RPText = @rpText "
			sql = sql & "Where LANr = 9300 And LONr = @loNr "
			sql = sql & "And MANr = @maNr"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@maNr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@loNr", loNr))
			listOfParams.Add(New SqlClient.SqlParameter("@rpText", rpText))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

			Return success

		End Function

		''' <summary>
		''' Sets is complete flag on an LO.
		''' </summary>
		''' <param name="loNr">The LONr.</param>
		''' <param name="maNr">The MANr.</param>
		''' <param name="mdNr">The MDNr.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Function SetLOIsCompleteFlag(ByVal loNr As Integer, ByVal maNr As Integer, ByVal mdNr As Integer) As Boolean Implements IPayrollDatabaseAccess.SetLOIsCompleteFlag

			Dim success = True

			Dim sql As String = String.Empty

			sql &= "Update LO Set IsComplete = 1, Ansaessigkeit = (Select Top 1 Ansaessigkeit From Mitarbeiter Where MANr = @maNr), "
			sql &= "Ans_QST_Bis = (Select Top 1 Ans_QST_Bis From Mitarbeiter Where MANr = @maNr) "
			sql &= "Where LONr = @loNr And MANr = @maNr "
			sql &= "And MDNr = @mdNr"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@loNr", loNr))
			listOfParams.Add(New SqlClient.SqlParameter("@maNr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@mdNr", mdNr))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

			Return success
		End Function

		''' <summary>
		''' Deletes from LOL.
		''' </summary>
		''' <param name="loNr">The LONr.</param>
		''' <returns>Boolen flag indicating success.</returns>
		Function DeleteFromLOL(ByVal loNr As Integer) As Boolean Implements IPayrollDatabaseAccess.DeleteFromLOL

			Dim success = True

			Dim sql As String = "Delete From LOL Where LONr = @loNr"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("loNr", loNr))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

			Return success

		End Function

		Function DeleteAssignedInvalidPayroll(ByVal loNr As Integer) As Boolean Implements IPayrollDatabaseAccess.DeleteAssignedInvalidPayroll

			Dim success = True

			Dim sql As String = "[Delete Assigned Invalid Payroll Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("loNr", loNr))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success

		End Function


#End Region



#Region "Private Methods"

		''' <summary>
		''' Trims non null strings.
		''' </summary>
		Private Function TrimNonNull(ByVal str As String) As String

			If str Is Nothing Then
				Return str
			End If

			Return str.Trim()

		End Function

#End Region

	End Class

End Namespace
