
Imports SP.DatabaseAccess.Listing.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language

Namespace Listing


  Partial Class ListingDatabaseAccess
    Inherits DatabaseAccessBase
    Implements IListingDatabaseAccess


		Function AddWeeklyReportData(ByVal data As ReportWeeklyPrintData) As Boolean Implements IListingDatabaseAccess.AddWeeklyReportData

			Dim success As Boolean = True
			Dim sql As String

			sql = "Insert Into RPPrint ("
			sql &= "MDNr, "
			sql &= "RPNr, "
			sql &= "MANr, "
			sql &= "KDNr, "
			sql &= "ESNr, "
			sql &= "Mo, "
			sql &= "Di, "
			sql &= "Mi, "
			sql &= "Do, "
			sql &= "Fr, "
			sql &= "Sa, "
			sql &= "So, "
			sql &= "Monat, "
			sql &= "Woche, "
			sql &= "Jahr, "
			sql &= "PrintedWeeks, "
			sql &= "PrintedDate, "
			sql &= "USNr "

			sql &= ") "
			sql &= "Values "
			sql &= "( "

			sql &= "@MDNr, "
			sql &= "@RPNr, "
			sql &= "@MANr, "
			sql &= "@KDNr, "
			sql &= "@ESNr, "
			sql &= "@Mo, "
			sql &= "@Di, "
			sql &= "@Mi, "
			sql &= "@Do, "
			sql &= "@Fr, "
			sql &= "@Sa, "
			sql &= "@So, "
			sql &= "@Monat, "
			sql &= "@Woche, "
			sql &= "@Jahr, "
			sql &= "@PrintedWeeks, "
			sql &= "@PrintedDate, "
			sql &= "@USNr "
			sql &= ") "


			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(data.MDNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USNr", ReplaceMissing(data.UserNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RPNr", ReplaceMissing(data.RPNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MANr", ReplaceMissing(data.MANr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(data.KDNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ESNr", ReplaceMissing(data.ESNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Mo", ReplaceMissing(data.MondayDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Di", ReplaceMissing(data.TuesdayDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Mi", ReplaceMissing(data.WednesdayDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Do", ReplaceMissing(data.ThursdayDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Fr", ReplaceMissing(data.FridayDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Sa", ReplaceMissing(data.SaturdayDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("So", ReplaceMissing(data.SundayDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Monat", ReplaceMissing(data.Month, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Woche", ReplaceMissing(data.Week, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Jahr", ReplaceMissing(data.Year, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("PrintedWeeks", ReplaceMissing(data.PrintedWeeks, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("PrintedDate", ReplaceMissing(data.PrintedDates, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)


			Return success

		End Function

		Function DeleteUserReportPrintData(ByVal data As ReportWeeklyPrintData) As Boolean Implements IListingDatabaseAccess.DeleteUserReportPrintData
			Dim success As Boolean = True

			Dim sql As String

			sql = "DELETE Dbo.[RPPrint] "
			sql &= "Where "
			sql &= "MDNr = @MDNr And "
			sql &= "USNr = @USNr"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(data.MDNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USNr", ReplaceMissing(data.UserNr, DBNull.Value)))

			success = success AndAlso ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

			Return success
		End Function

		Function LoadReportDataForPrinting(ByVal searchdata As RPSearchData) As IEnumerable(Of RPPrintData) Implements IListingDatabaseAccess.LoadReportDataForPrinting
			Dim result As List(Of RPPrintData) = Nothing

			Dim sql As String

			sql = "Select RP.* From RP "
			sql &= "Left Join ES On RP.ESNr = ES.ESNr "
			sql &= "Left Join MA_LOSetting On RP.MANr = MA_LOSetting.MANr "
			sql &= "Left Join Mitarbeiter On RP.MANr = Mitarbeiter.MANr "
			sql &= "Left Join Kunden On RP.KDNr = Kunden.KDNr "

			sql &= "Where (ES.PrintNoRP <> 1) "
			sql &= "And RP.MDNr = @MDNr "

			If searchdata.RPNr Is Nothing Then
				sql &= "And RP.Monat = @monat "
				sql &= "And RP.Jahr = @jahr "

				sql &= "And (@kdnr = 0 Or RP.KDNr In (@kdnr)) "
				sql &= "And (@manr = 0 Or RP.MANr In (@manr)) "

				sql &= "And (@kst1 = '' Or RP.RPKst1 = @kst1) "
				sql &= "And (@kst2 = '' Or RP.RPKst2 = @kst2) "
				sql &= "And (@kst3 = '' Or RP.RPKst = @kst3) "

			Else
				sql &= "And (@rpnr = 0 Or RP.RPNr In (@rpnr)) "

			End If

			sql &= "And (KD.KDFiliale + MA.MaFiliale) Like '@filiale "

			sql &= "Order By RP.RPNr ASC"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(searchdata.MDNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("RPNr", ReplaceMissing(searchdata.RPNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("monat", ReplaceMissing(searchdata.monat, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("jahr", ReplaceMissing(searchdata.jahr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("manr", ReplaceMissing(searchdata.MANr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("kdnr", ReplaceMissing(searchdata.KDNr, 0)))

			listOfParams.Add(New SqlClient.SqlParameter("kst1", ReplaceMissing(searchdata.kst1, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("kst2", ReplaceMissing(searchdata.kst2, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("kst3", ReplaceMissing(searchdata.kst3, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("Filiale", ReplaceMissing("%%", String.Empty)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of RPPrintData)

					While reader.Read

						Dim data = New RPPrintData()
						data.jahr = SafeGetInteger(reader, "jahr", 0)
						data.monat = SafeGetInteger(reader, "monat", 0)
						data.RPNr = SafeGetInteger(reader, "rpnr", 0)
						data.KDNr = SafeGetInteger(reader, "kdnr", 0)
						data.MANr = SafeGetInteger(reader, "manr", 0)

						data.von = SafeGetDateTime(reader, "von", Nothing)
						data.bis = SafeGetDateTime(reader, "bis", Nothing)

						data.printedweeks = SafeGetString(reader, "printedweek")

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
		''' Loads the complete PrintData with all necessary data for the List
		''' </summary>
		Function LoadRPPrintWeeklyData(ByVal MDNr As Integer, ByVal printYear As Integer, ByVal firstWeek As Integer, ByVal lastWeek As Integer, ByVal userNumber As Integer) As IEnumerable(Of RPPrintWeeklyData) Implements IListingDatabaseAccess.LoadRPPrintWeeklyData

			Dim result As List(Of RPPrintWeeklyData) = Nothing

			Dim sql As String

			sql = "[Load Report Data For Print Reports]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(MDNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("year", ReplaceMissing(printYear, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("FromWeek", ReplaceMissing(firstWeek, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ToWeek", ReplaceMissing(lastWeek, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USNr", ReplaceMissing(userNumber, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of RPPrintWeeklyData)

					While reader.Read

						Dim data = New RPPrintWeeklyData()

						data.RPNr = SafeGetInteger(reader, "RPNr", Nothing)
						data.MANr = SafeGetInteger(reader, "MANr", Nothing)
						data.KDNr = SafeGetInteger(reader, "KDNr", Nothing)
						data.ESNr = SafeGetInteger(reader, "ESNr", Nothing)
						data.Montag = SafeGetDateTime(reader, "Mo", Nothing)
						data.Dienstag = SafeGetDateTime(reader, "Di", Nothing)
						data.Mittwoch = SafeGetDateTime(reader, "Mi", Nothing)
						data.Donnerstag = SafeGetDateTime(reader, "Do", Nothing)
						data.Freitag = SafeGetDateTime(reader, "Fr", Nothing)
						data.Samstag = SafeGetDateTime(reader, "Sa", Nothing)
						data.Sonntag = SafeGetDateTime(reader, "So", Nothing)

						data.Monat = SafeGetInteger(reader, "Monat", Nothing)
						data.Woche = SafeGetInteger(reader, "Woche", Nothing)
						data.Jahr = SafeGetInteger(reader, "Jahr", Nothing)
						data.USNr = userNumber
						data.PrintedWeeks = String.Empty
						data.PrintedDate = String.Empty

						data.CustomerCompany = SafeGetString(reader, "Firma1")
						data.KDFirma2 = SafeGetString(reader, "Firma2")
						data.KDFirma3 = SafeGetString(reader, "Firma3")
						data.CustomerPostoffice = SafeGetString(reader, "KDPostfach")
						data.CustomerStreet = SafeGetString(reader, "KDStrasse")
						data.CustomerPostcode = SafeGetString(reader, "KDPLZ")
						data.CustomerLocation = SafeGetString(reader, "KDOrt")
						data.CustomerCountry = SafeGetString(reader, "KDLand")
						data.CustomerTelephone = SafeGetString(reader, "KDTelefon")
						data.CustomerTelefax = SafeGetString(reader, "KDTelefax")

						data.ESArbOrt = SafeGetString(reader, "ArbOrt")
						data.ESArbZeit = SafeGetString(reader, "ArbZeit")
						data.ESMelden = SafeGetString(reader, "Melden")
						data.ESAls = SafeGetString(reader, "ES_Als")
						data.ESBemerk_MA = SafeGetString(reader, "Bemerk_MA")
						data.ESBemerk_1 = SafeGetString(reader, "Bemerk_1")
						data.ESBemerk_2 = SafeGetString(reader, "Bemerk_2")
						data.ESBemerk_3 = SafeGetString(reader, "Bemerk_3")
						data.ESBemerk_LO = SafeGetString(reader, "Bemerk_LO")
						data.ESBemerk_RE = SafeGetString(reader, "Bemerk_RE")
						data.ESBemerk_P = SafeGetString(reader, "Bemerk_P")
						data.ESSUVA = SafeGetString(reader, "ESSUVA")
						data.ESUhr = SafeGetString(reader, "ES_Uhr")
						data.ESEnde = SafeGetString(reader, "Ende")

						data.ES_Ab = SafeGetDateTime(reader, "ES_Ab", Nothing)
						data.ES_Ende = SafeGetDateTime(reader, "ES_Ende", Nothing)

						data.ESLStdLohn = SafeGetDecimal(reader, "Stundenlohn", Nothing)
						data.ESLTarif = SafeGetDecimal(reader, "Tarif", Nothing)
						data.ESLMAStdSpesen = SafeGetDecimal(reader, "MAStdSpesen", Nothing)
						data.ESLMATSpesen = SafeGetDecimal(reader, "MATSpesen", Nothing)
						data.ESLKDTSpesen = SafeGetDecimal(reader, "KDTSpesen", Nothing)
						data.ESLGAVGruppe0 = SafeGetString(reader, "GAVGruppe0")

						data.EmployeeGender = SafeGetString(reader, "Geschlecht")
						data.EmployeeLastname = SafeGetString(reader, "MANachname")
						data.EmployeeFirstname = SafeGetString(reader, "MAVorname")
						data.MACo = SafeGetString(reader, "MACo")
						data.EmployeePostoffice = SafeGetString(reader, "MAPostoffice")
						data.EmployeeStreet = SafeGetString(reader, "MAStrasse")
						data.EmployeePostcode = SafeGetString(reader, "MAPLZ")
						data.EmployeeLocation = SafeGetString(reader, "MAOrt")
						data.EmployeeCountry = SafeGetString(reader, "MALand")


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				result = Nothing
				m_Logger.LogError(ex.ToString())

			Finally
				CloseReader(reader)

			End Try


			Return result
		End Function

		Function UpdateAssignedReportWithPrintData(ByVal MDNr As Integer, ByVal printData As RPPrintWeeklyData, ByVal userNumber As Integer) As Boolean Implements IListingDatabaseAccess.UpdateAssignedReportWithPrintData

			Dim success = True

			Dim sql As String

			sql = "Update RP Set "
			'If printData.PrintedWeeks.Contains(printData.Woche) Then
			'	sql &= "PrintedWeeks = PrintedWeeks + '#' + @Printweek"
			'	'Else
			sql &= "PrintedWeeks = CASE WHEN ISNULL(printedweeks, '') = '' THEN Convert(nvarchar(10), @Printweek) ELSE printedweeks + '#' + Convert(nvarchar(10), @Printweek) + ''  END"
			'End If

			'If Not String.IsNullOrWhiteSpace(printData.PrintedDate) Then
			'	sql &= ", PrintedDate = PrintedDate + '#' + @Printdate"
			'Else
			sql &= ", PrintedDate = CASE WHEN ISNULL(PrintedDate, '') = '' THEN CONVERT(NVARCHAR(10), GETDATE(), 104) ELSE PrintedDate + '#' + CONVERT(NVARCHAR(10), GETDATE(), 104) + '' END"
			'sql &= ", PrintedDate = @Printdate"
			'End If

			sql &= " Where MDNr = @MDNr"
			sql &= " And RPNr = @rpnr"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(MDNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Printweek", ReplaceMissing(printData.Woche, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("rpnr", ReplaceMissing(printData.RPNr, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)


			Return success
		End Function

	End Class


End Namespace


