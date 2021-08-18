Imports SP.DatabaseAccess.Listing.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language


Namespace Listing


	Partial Class ListingDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IListingDatabaseAccess

		Function LoadQSTCantonMasterData(ByVal searchdata As PayrollListingSearchData) As QSTCantonMasterData Implements IListingDatabaseAccess.LoadQSTCantonMasterData
			Dim result As QSTCantonMasterData = Nothing

			Dim sql As String

			sql = "Select Top(1) * "
			sql &= "FROM dbo.MD_QstAddress "
			sql &= "Where "
			sql &= "MDNr = @MDNr "
			sql &= "AND (@Canton = '' OR Kanton = @Canton) "


			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(searchdata.MDNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Canton", ReplaceMissing(searchdata.Canton, String.Empty)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)


			Try

				result = New QSTCantonMasterData

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result.ID = SafeGetInteger(reader, "ID", 0)
					result.RecNr = SafeGetInteger(reader, "RecNr", 0)
					result.MDNr = SafeGetInteger(reader, "MDNr", 0)
					result.Address1 = SafeGetString(reader, "Adresse1")
					result.Canton = SafeGetString(reader, "Kanton")
					result.Address2 = SafeGetString(reader, "Zusatz")
					result.ZHD = SafeGetString(reader, "ZHD")
					result.PostOfficeBox = SafeGetString(reader, "Postfach")
					result.Street = SafeGetString(reader, "Strasse")
					result.Country = SafeGetString(reader, "Land")
					result.Postcode = SafeGetString(reader, "PLZ")
					result.City = SafeGetString(reader, "Ort")
					result.AccountNumber = SafeGetString(reader, "StammNr")
					result.Provision = SafeGetDecimal(reader, "Provision", Nothing)
					result.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
					result.CreatedFrom = SafeGetString(reader, "CreatedFrom")
					result.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
					result.ChangedFrom = SafeGetString(reader, "ChangedFrom")
					result.Communikty = SafeGetString(reader, "Gemeinde")
					result.Comments = SafeGetString(reader, "Bemerkung")

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		Function LoadQSTYearData(ByVal mdNr As Integer) As IEnumerable(Of Integer) Implements IListingDatabaseAccess.LoadQSTYearData
			Dim result As List(Of Integer) = Nothing

			Dim sql As String

			sql = "[Load Payroll Year For Search In QST Listing]"


			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNr, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)


			Try

				If (Not reader Is Nothing) Then
					result = New List(Of Integer)

					While reader.Read

						Dim jahr As Integer = SafeGetInteger(reader, "Jahr", 0)

						result.Add(jahr)

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

		Function LoadQSTMonthData(ByVal mdNr As Integer, ByVal year As Integer) As IEnumerable(Of Integer) Implements IListingDatabaseAccess.LoadQSTMonthData
			Dim result As List(Of Integer) = Nothing

			Dim sql As String

			sql = "[Load Payroll Month For Search In QST Listing]"


			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("year", ReplaceMissing(year, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)


			Try

				If (Not reader Is Nothing) Then
					result = New List(Of Integer)

					While reader.Read

						Dim jahr As Integer = SafeGetInteger(reader, "LP", 0)

						result.Add(jahr)

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

		Function LoadQSTLAData(ByVal mdNr As Integer, ByVal year As Integer, ByVal monthFrom As Integer, ByVal monthTo As Integer?) As IEnumerable(Of QSTLAData) Implements IListingDatabaseAccess.LoadQSTLAData

			Dim result As List(Of QSTLAData) = Nothing
			'Dim i As Integer = 0
			Dim sql As String

			sql = "[Show LA Data For Search In QST Listing]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("year", ReplaceMissing(year, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MonatVon", ReplaceMissing(monthFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MonatBis", ReplaceMissing(monthTo, monthFrom)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of QSTLAData)

					While reader.Read

						'If i = 0 Then
						'	Dim firstData = New QSTLAData
						'	firstData.LANr = 0
						'	firstData.LALoText = "Alle"
						'	firstData.QSteuer = False

						'	result.Add(firstData)

						'End If

						Dim data = New QSTLAData
						data.LANr = SafeGetDecimal(reader, "LANr", Nothing)
						data.LALoText = SafeGetString(reader, "LALoText")
						data.QSteuer = SafeGetBoolean(reader, "Q_Steuer", False)

						result.Add(data)

						'i += 1

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

		Function LoadQSTCantonData(ByVal mdNr As Integer, ByVal year As Integer, ByVal monthFrom As Integer, ByVal monthTo As Integer?, ByVal laNumbers As String) As IEnumerable(Of CantonData) Implements IListingDatabaseAccess.LoadQSTCantonData

			Dim result As List(Of CantonData) = Nothing

			Dim sql As String
			Dim laNumbersBuffer As String = String.Empty

			For Each number In laNumbers.Split(New Char() {",", ";", "#"}, StringSplitOptions.RemoveEmptyEntries)

				laNumbersBuffer = laNumbersBuffer & IIf(laNumbersBuffer <> "", ",", "") & number

			Next

			sql = "[Show Canton Data For Search In QST Listing]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("year", ReplaceMissing(year, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MonatVon", ReplaceMissing(monthFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MonatBis", ReplaceMissing(monthTo, monthFrom)))
			listOfParams.Add(New SqlClient.SqlParameter("LANumbers", ReplaceMissing(laNumbersBuffer, String.Empty)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of CantonData)

					While reader.Read

						Dim data As New CantonData
						data.Canton = SafeGetString(reader, "S_Kanton")

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

		Function LoadQSTCommunityData(ByVal mdNr As Integer, ByVal year As Integer, ByVal monthFrom As Integer, ByVal monthTo As Integer?, ByVal laNumbers As String, ByVal canton As String) As IEnumerable(Of QSTCommunityData) Implements IListingDatabaseAccess.LoadQSTCommunityData

			Dim result As List(Of QSTCommunityData) = Nothing

			Dim sql As String
			Dim laNumbersBuffer As String = String.Empty

			For Each number In laNumbers.Split(New Char() {",", ";", "#"}, StringSplitOptions.RemoveEmptyEntries)

				laNumbersBuffer = laNumbersBuffer & IIf(laNumbersBuffer <> "", ", ", "") & number

			Next

			sql = "[Show Community Data For Search In QST Listing]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("year", ReplaceMissing(year, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MonatVon", ReplaceMissing(monthFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MonatBis", ReplaceMissing(monthTo, monthFrom)))
			listOfParams.Add(New SqlClient.SqlParameter("LANumbers", ReplaceMissing(laNumbersBuffer, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("Canton", ReplaceMissing(canton, String.Empty)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of QSTCommunityData)

					While reader.Read

						Dim data As New QSTCommunityData
						data.CommunityCode = SafeGetString(reader, "QSTGemeinde")
						data.CommunityName = SafeGetString(reader, "QSTGemeinde")

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

		Function LoadTaxCountryCodeData(ByVal mdNr As Integer, ByVal year As Integer, ByVal monthFrom As Integer, ByVal monthTo As Integer?, ByVal canton As String) As IEnumerable(Of Common.DataObjects.CountryData) Implements IListingDatabaseAccess.LoadTaxCountryCodeData

			Dim result As List(Of Common.DataObjects.CountryData) = Nothing

			Dim sql As String

			sql = "[Show Country Code Data For Search In QST Listing]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("year", ReplaceMissing(year, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MonatVon", ReplaceMissing(monthFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MonatBis", ReplaceMissing(monthTo, monthFrom)))
			listOfParams.Add(New SqlClient.SqlParameter("Canton", ReplaceMissing(canton, String.Empty)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of Common.DataObjects.CountryData)

					While reader.Read

						Dim data As New Common.DataObjects.CountryData
						data.Code = SafeGetString(reader, "Code")
						data.Name = SafeGetString(reader, "CountryLabel")


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

		Function LoadTaxNationalityCodeData(ByVal mdNr As Integer, ByVal year As Integer, ByVal monthFrom As Integer, ByVal monthTo As Integer?, ByVal canton As String) As IEnumerable(Of Common.DataObjects.CountryData) Implements IListingDatabaseAccess.LoadTaxNationalityCodeData

			Dim result As List(Of Common.DataObjects.CountryData) = Nothing

			Dim sql As String

			sql = "[Show Nationality Code Data For Search In QST Listing]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("year", ReplaceMissing(year, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MonatVon", ReplaceMissing(monthFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MonatBis", ReplaceMissing(monthTo, monthFrom)))
			listOfParams.Add(New SqlClient.SqlParameter("Canton", ReplaceMissing(canton, String.Empty)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of Common.DataObjects.CountryData)

					While reader.Read

						Dim data As New Common.DataObjects.CountryData
						data.Code = SafeGetString(reader, "Nationality")
						data.Name = SafeGetString(reader, "NationalityLabel")


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

		Function LoadQSTCodeData(ByVal mdNr As Integer, ByVal year As Integer, ByVal monthFrom As Integer, ByVal monthTo As Integer?, ByVal canton As String) As IEnumerable(Of QSTCodeData) Implements IListingDatabaseAccess.LoadQSTCodeData

			Dim result As List(Of QSTCodeData) = Nothing

			Dim sql As String

			sql = "[Show QST Code Data For Search In QST Listing]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("year", ReplaceMissing(year, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MonatVon", ReplaceMissing(monthFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MonatBis", ReplaceMissing(monthTo, monthFrom)))
			listOfParams.Add(New SqlClient.SqlParameter("Canton", ReplaceMissing(canton, String.Empty)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of QSTCodeData)

					While reader.Read

						Dim data As New QSTCodeData
						data.QSTCode = SafeGetString(reader, "Q_Steuer")
						data.QSTCodeLabel = SafeGetString(reader, "CodeLabel")


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

		Function LoadQSTPermissionData(ByVal mdNr As Integer, ByVal year As Integer, ByVal monthFrom As Integer, ByVal monthTo As Integer?, ByVal canton As String, ByVal qstCode As String) As IEnumerable(Of QSTPermissionData) Implements IListingDatabaseAccess.LoadQSTPermissionData

			Dim result As List(Of QSTPermissionData) = Nothing

			Dim sql As String

			sql = "[Show Permission Data For Search In QST Listing]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("year", ReplaceMissing(year, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MonatVon", ReplaceMissing(monthFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MonatBis", ReplaceMissing(monthTo, monthFrom)))
			listOfParams.Add(New SqlClient.SqlParameter("Canton", ReplaceMissing(canton, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("QSTCode", ReplaceMissing(qstCode, String.Empty)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of QSTPermissionData)

					While reader.Read

						Dim data As New QSTPermissionData
						'data.PermissioinCode = SafeGetString(reader, "Permission")
						data.PermissionCode = SafeGetString(reader, "Permission")
						data.PermissionCodeLabel = SafeGetString(reader, "CodeLabel")


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

		Function LoadSearchResultOfTaxData(ByVal mdNr As Integer, ByVal userNr As Integer) As IEnumerable(Of SearchRestulOfTaxData) Implements IListingDatabaseAccess.LoadSearchResultOfTaxData

			Dim result As List(Of SearchRestulOfTaxData) = Nothing

			Dim sql As String

			sql = "[Load Montly Tax Data For Search In TAX Listing]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USNr", ReplaceMissing(userNr, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of SearchRestulOfTaxData)

					While reader.Read
						Dim data As New SearchRestulOfTaxData

						data.MANr = SafeGetInteger(reader, "MANr", Nothing)
						data.EmployeeFirstname = SafeGetString(reader, "Vorname")
						data.EmployeeLastname = SafeGetString(reader, "Nachname")
						data.employeename = String.Format("{1}, {0}", data.EmployeeFirstname, data.EmployeeLastname)

						data.vonmonat = SafeGetInteger(reader, "vonmonat", 0)
						data.bismonat = SafeGetInteger(reader, "bismonat", 0)
						data.jahr = SafeGetInteger(reader, "jahr", 0)

						data.gebdat = SafeGetDateTime(reader, "gebdat", Nothing)
						data.ahv_nr = SafeGetString(reader, "ahv_nr")
						data.ahv_nr_new = SafeGetString(reader, "ahv_nr_new")
						data.s_kanton = SafeGetString(reader, "s_kanton")
						data.qstgemeinde = SafeGetString(reader, "S_Gemeinde")

						data.TaxCommunityLabel = SafeGetString(reader, "TaxCommunityLabel")
						data.TaxCommunityCode = SafeGetInteger(reader, "TaxCommunityCode", 0)

						data.EmploymentType = SafeGetString(reader, "EmploymentType")
						data.OtherEmploymentType = SafeGetString(reader, "OtherEmploymentType")
						data.TypeofStay = SafeGetString(reader, "TypeofStay")
						data.ForeignCategory = SafeGetString(reader, "ForeignCategory")
						data.SocialInsuranceNumber = SafeGetString(reader, "SocialInsuranceNumber")
						data.CivilState = SafeGetString(reader, "CivilState")
						data.NumberOfChildren = SafeGetInteger(reader, "NumberOfChildren", 0)
						data.TaxChurchCode = SafeGetString(reader, "TaxChurchCode")
						data.PartnerLastName = SafeGetString(reader, "PartnerLastName")
						data.PartnerFirstname = SafeGetString(reader, "PartnerFirstname")
						data.PartnerStreet = SafeGetString(reader, "PartnerStreet")
						data.PartnerPostcode = SafeGetString(reader, "PartnerPostcode")
						data.PartnerCity = SafeGetString(reader, "PartnerCity")
						data.PartnerCountry = SafeGetString(reader, "PartnerCountry")
						data.InEmployment = SafeGetBoolean(reader, "InEmployment", False)

						data.EmploymentLocation = SafeGetString(reader, "ESOrt")
						data.EmploymentPostcode = SafeGetString(reader, "ESPLZ")
						data.EmploymentStreet = SafeGetString(reader, "ESStrasse")

						data.geschlecht = SafeGetString(reader, "geschlecht")

						data.employeestreet = SafeGetString(reader, "mastrasse")
						data.employeepostcode = SafeGetString(reader, "maplz")
						data.employeecity = SafeGetString(reader, "maort")
						data.employeecountry = SafeGetString(reader, "maland")

						data.monat = SafeGetInteger(reader, "monat", Nothing)
						data.kinder = SafeGetInteger(reader, "kinder", Nothing)
						data.employeelanguage = SafeGetString(reader, "sprache")

						data.m_anz = SafeGetDecimal(reader, "m_anz", Nothing)
						data.m_bas = SafeGetDecimal(reader, "m_bas", Nothing)
						data.m_ans = SafeGetDecimal(reader, "m_ans", Nothing)
						data.m_btr = SafeGetDecimal(reader, "m_btr", Nothing)
						data.Bruttolohn = SafeGetDecimal(reader, "bruttolohn", Nothing)
						data.qstbasis = SafeGetDecimal(reader, "qstbasis", Nothing)

						data.Exception_KTGBetrag_Amount = SafeGetDecimal(reader, "Exception_KTGBetrag_Amount", 0)
						data.Exception_SuvaBetrag_Amount = SafeGetDecimal(reader, "Exception_SuvaBetrag_Amount", 0)
						data.Exception_KiAuBetrag_Amount = SafeGetDecimal(reader, "Exception_KiAuBetrag_Amount", 0)
						data.Exception_OtherServices_Amount = SafeGetDecimal(reader, "Exception_OtherServices_Amount", 0)
						data.Exception_OtherNotDefined_Amount = SafeGetDecimal(reader, "Exception_OtherNotDefined_Amount", 0)
						data.Exception_OtherServicesLALabel = SafeGetString(reader, "Exception_OtherServicesLALabel")
						data.Exception_OtherNotDefinedLALabel = SafeGetString(reader, "Exception_OtherNotDefinedLALabel")
						data.Exception_SPayed_Amount = SafeGetDecimal(reader, "Exception_SPayed_Amount", 0)
						data.Exception_SBacked_Amount = SafeGetDecimal(reader, "Exception_SBacked_Amount", 0)


						data.stdanz = SafeGetDecimal(reader, "stdanz", Nothing)

						data.tarifcode = SafeGetString(reader, "tarifcode")
						data.workeddays = SafeGetInteger(reader, "workeddays", Nothing)

						data.WorkingHoursMonth = SafeGetDecimal(reader, "RPGAVStdMonth", 0)
						data.WorkingHoursWeek = SafeGetDecimal(reader, "RPGAVStdWeek", 0)
						data.EmploymentNumber = SafeGetInteger(reader, "AssignedESNr", 0)
						data.ESLohnNumber = SafeGetInteger(reader, "AssignedESLohnNr", 0)
						data.ReportNumber = SafeGetInteger(reader, "AssignedRPNr", 0)
						data.WorkingPensum = SafeGetString(reader, "Arbeitspensum")
						data.GAVStringInfo = SafeGetString(reader, "GAVInfo")
						data.Dismissalreason = SafeGetString(reader, "Dismissalreason")

						data.EmployeePartnerRecID = SafeGetInteger(reader, "EmployeePartnerRecID", 0)
						data.EmployeeLOHistoryID = SafeGetInteger(reader, "EmployeeLOHistoryID", 0)

						data.esab = SafeGetDateTime(reader, "esab", Nothing)
						data.esende = SafeGetDateTime(reader, "esende", Nothing)


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

		Function DeleteAllUserTaxData(ByVal mdNr As Integer, ByVal userNumber As Integer) As Boolean Implements IListingDatabaseAccess.DeleteAllUserTaxData

			Dim result As Boolean = True

			Dim sql As String

			sql = "[Delete Montly Tax Data For Search In TAX Listing]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USNr", ReplaceMissing(userNumber, DBNull.Value)))

			result = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return result

		End Function

		Function AddTaxListDataToTaxData(ByVal listData As TaxListTableData) As Boolean Implements IListingDatabaseAccess.AddTaxListDataToTaxData
			Dim success As Boolean = True


			Dim sql As String

			sql = "INSERT INTO dbo.[tbl_TaxData] "
			sql &= "("
			sql &= "MDNr"
			sql &= ",USNR"
			sql &= ",MANR"
			sql &= ",LANR"
			sql &= ",Monat"
			sql &= ",LONR"
			sql &= ",LALOText"
			sql &= ",S_Kanton"
			sql &= ",S_Gemeinde"
			sql &= ",ESAb"
			sql &= ",ESEnde"
			sql &= ",Nachname"
			sql &= ",Vorname"
			sql &= ",VonMonat"
			sql &= ",BisMonat"
			sql &= ",Jahr"
			sql &= ",GebDat"
			sql &= ",AHV_Nr"
			sql &= ",AHV_Nr_New"
			sql &= ",Geschlecht"
			sql &= ",MAStrasse"
			sql &= ",MAPLZ"
			sql &= ",MAOrt"
			sql &= ",MAPLZOrt"
			sql &= ",MALand"
			sql &= ",Zivilstand"
			sql &= ",Sprache"
			sql &= ",Kinder"
			sql &= ",Bewillig"
			sql &= ",SelectedKanton"
			sql &= ",SelectedGemeinde"
			sql &= ",M_Anz"
			sql &= ",M_Bas"
			sql &= ",M_Ans"
			sql &= ",M_Btr"
			sql &= ",Bruttolohn"
			sql &= ",QSTBasis"
			sql &= ",StdAnz"
			sql &= ",TarifCode"
			sql &= ",WorkedDays"
			sql &= ",ShowStdAnz"
			sql &= ",ESStrasse"
			sql &= ",ESPLZ"
			sql &= ",ESOrt"
			sql &= ",ESKanton"
			sql &= ",AssignedESNr"
			sql &= ",AssignedESLohnNr"
			sql &= ",AssignedRPNr"
			sql &= ",GAVInfo"
			sql &= ",RPGAVStdWeek"
			sql &= ",RPGAVStdMonth"
			sql &= ",Dismissalreason"
			sql &= ",EmployeePartnerRecID"
			sql &= ",EmployeeLOHistoryID"
			sql &= ",Arbeitspensum"

			sql &= ",Exception_KTGBetrag_Amount"
			sql &= ",Exception_SuvaBetrag_Amount"
			sql &= ",Exception_KiAuBetrag_Amount"
			sql &= ",Exception_OtherServices_Amount"
			sql &= ",Exception_OtherNotDefined_Amount"
			sql &= ",Exception_SPayed_Amount"
			sql &= ",Exception_SBacked_Amount"
			sql &= ",Exception_OtherServicesLALabel"
			sql &= ",Exception_OtherNotDefinedLALabel"

			sql &= ")"

			sql &= "Values "

			sql &= "("
			sql &= "@MDNr"
			sql &= ",@USNr"
			sql &= ",@MANr"
			sql &= ",@LANr"
			sql &= ",@Monat"
			sql &= ",@LONR"
			sql &= ",@LALOText"
			sql &= ",@S_Kanton"
			sql &= ",@S_Gemeinde"
			sql &= ",@ESAb"
			sql &= ",@ESEnde"
			sql &= ",@Nachname"
			sql &= ",@Vorname"
			sql &= ",@VonMonat"
			sql &= ",@BisMonat"
			sql &= ",@Jahr"
			sql &= ",@GebDat"
			sql &= ",@AHV_Nr"
			sql &= ",@AHV_Nr_New"
			sql &= ",@Geschlecht"
			sql &= ",@MAStrasse"
			sql &= ",@MAPLZ"
			sql &= ",@MAOrt"
			sql &= ",@MAPLZOrt"
			sql &= ",@MALand"
			sql &= ",@Zivilstand"
			sql &= ",@Sprache"
			sql &= ",@Kinder"
			sql &= ",@Bewillig"
			sql &= ",@SelectedKanton"
			sql &= ",@SelectedGemeinde"
			sql &= ",@M_Anz"
			sql &= ",@M_Bas"
			sql &= ",@M_Ans"
			sql &= ",@M_Btr"
			sql &= ",@Bruttolohn"
			sql &= ",@QSTBasis"
			sql &= ",@StdAnz"
			sql &= ",@TarifCode"
			sql &= ",@WorkedDays"
			sql &= ",@ShowStdAnz"
			sql &= ",@ESStrasse"
			sql &= ",@ESPLZ"
			sql &= ",@ESOrt"
			sql &= ",@ESKanton"
			sql &= ",@AssignedESNr"
			sql &= ",@AssignedESLohnNr"
			sql &= ",@AssignedRPNr"
			sql &= ",@GAVInfo"
			sql &= ",@RPGAVStdWeek"
			sql &= ",@RPGAVStdMonth"
			sql &= ",@Dismissalreason"
			sql &= ",@EmployeePartnerRecID"
			sql &= ",@EmployeeLOHistoryID"
			sql &= ",@Arbeitspensum"

			sql &= ",@Exception_KTGBetrag_Amount"
			sql &= ",@Exception_SuvaBetrag_Amount"
			sql &= ",@Exception_KiAuBetrag_Amount"
			sql &= ",@Exception_OtherServices_Amount"
			sql &= ",@Exception_OtherNotDefined_Amount"
			sql &= ",@Exception_SPayed_Amount"
			sql &= ",@Exception_SBacked_Amount"
			sql &= ",@Exception_OtherServicesLALabel"
			sql &= ",@Exception_OtherNotDefinedLALabel"

			sql &= ")"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			' Data of parameters
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(listData.MDNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USNr", ReplaceMissing(listData.USNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MANr", ReplaceMissing(listData.MANR, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Monat", ReplaceMissing(listData.Monat, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("LANR", ReplaceMissing(listData.LANR, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("LONr", ReplaceMissing(listData.LONR, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("LALOText", ReplaceMissing(listData.LALOText, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("S_Kanton", ReplaceMissing(listData.S_Kanton, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("S_Gemeinde", ReplaceMissing(listData.S_Gemeinde, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ESAb", ReplaceMissing(listData.ESAb, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ESEnde", ReplaceMissing(listData.ESEnde, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Nachname", ReplaceMissing(listData.Nachname, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Vorname", ReplaceMissing(listData.Vorname, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("VonMonat", ReplaceMissing(listData.VonMonat, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("BisMonat", ReplaceMissing(listData.BisMonat, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Jahr", ReplaceMissing(listData.Jahr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("GebDat", ReplaceMissing(listData.GebDat, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("AHV_Nr", ReplaceMissing(listData.AHV_Nr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("AHV_Nr_New", ReplaceMissing(listData.AHV_Nr_New, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Geschlecht", ReplaceMissing(listData.Geschlecht, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MAStrasse", ReplaceMissing(listData.MAStrasse, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MAPLZ", ReplaceMissing(listData.MAPLZ, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MAOrt", ReplaceMissing(listData.MAOrt, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MAPLZOrt", ReplaceMissing(listData.MAPLZOrt, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MALand", ReplaceMissing(listData.MALand, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Zivilstand", ReplaceMissing(listData.Zivilstand, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Sprache", ReplaceMissing(listData.Sprache, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Kinder", ReplaceMissing(listData.Kinder, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Bewillig", ReplaceMissing(listData.Bewillig, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("SelectedKanton", ReplaceMissing(listData.SelectedKanton, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("SelectedGemeinde", ReplaceMissing(listData.SelectedGemeinde, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("M_Anz", ReplaceMissing(listData.M_Anz, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("M_Bas", ReplaceMissing(listData.M_Bas, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("M_Ans", ReplaceMissing(listData.M_Ans, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("M_Btr", ReplaceMissing(listData.M_Btr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Bruttolohn", ReplaceMissing(listData.Bruttolohn, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("QSTBasis", ReplaceMissing(listData.QSTBasis, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("StdAnz", ReplaceMissing(listData.StdAnz, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("TarifCode", ReplaceMissing(listData.TarifCode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("WorkedDays", ReplaceMissing(listData.WorkedDays, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ShowStdAnz", ReplaceMissing(listData.ShowStdAnz, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ESStrasse", ReplaceMissing(listData.ESStrasse, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ESPLZ", ReplaceMissing(listData.ESPLZ, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ESOrt", ReplaceMissing(listData.ESOrt, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ESKanton", ReplaceMissing(listData.ESKanton, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("AssignedESNr", ReplaceMissing(listData.AssignedESNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("AssignedESLohnNr", ReplaceMissing(listData.AssignedESLohnNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("AssignedRPNr", ReplaceMissing(listData.AssignedRPNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("GAVInfo", ReplaceMissing(listData.GAVInfo, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RPGAVStdWeek", ReplaceMissing(listData.RPGAVStdWeek, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RPGAVStdMonth", ReplaceMissing(listData.RPGAVStdMonth, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Dismissalreason", ReplaceMissing(listData.Dismissalreason, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("EmployeePartnerRecID", ReplaceMissing(listData.EmployeePartnerRecID, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("EmployeeLOHistoryID", ReplaceMissing(listData.EmployeeLOHistoryID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Arbeitspensum", ReplaceMissing(listData.Arbeitspensum, DBNull.Value)))


			Dim taxExceptionData = LoadTaxExceptionData(listData.MDNr, listData.MANR, listData.Monat.GetValueOrDefault(0), listData.Jahr.GetValueOrDefault(0))
			If taxExceptionData Is Nothing Then
				taxExceptionData.Exception_KTGBetrag_Amount = 0
				taxExceptionData.Exception_SuvaBetrag_Amount = 0
				taxExceptionData.Exception_KiAuBetrag_Amount = 0
				taxExceptionData.Exception_OtherServices_Amount = 0
				taxExceptionData.Exception_OtherNotDefined_Amount = 0
				taxExceptionData.Exception_OtherServicesLALabel = String.Empty
				taxExceptionData.Exception_OtherNotDefinedLALabel = String.Empty
				taxExceptionData.Exception_SPayed_Amount = 0
				taxExceptionData.Exception_SBacked_Amount = 0
			End If

			listOfParams.Add(New SqlClient.SqlParameter("Exception_KTGBetrag_Amount", ReplaceMissing(taxExceptionData.Exception_KTGBetrag_Amount, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Exception_SuvaBetrag_Amount", ReplaceMissing(taxExceptionData.Exception_SuvaBetrag_Amount, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Exception_KiAuBetrag_Amount", ReplaceMissing(taxExceptionData.Exception_KiAuBetrag_Amount, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Exception_OtherServices_Amount", ReplaceMissing(taxExceptionData.Exception_OtherServices_Amount, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Exception_OtherNotDefined_Amount", ReplaceMissing(taxExceptionData.Exception_OtherNotDefined_Amount, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Exception_OtherServicesLALabel", ReplaceMissing(taxExceptionData.Exception_OtherServicesLALabel, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Exception_OtherNotDefinedLALabel", ReplaceMissing(taxExceptionData.Exception_OtherNotDefinedLALabel, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Exception_SPayed_Amount", ReplaceMissing(taxExceptionData.Exception_SPayed_Amount, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Exception_SBacked_Amount", ReplaceMissing(taxExceptionData.Exception_SBacked_Amount, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)


			Return success

		End Function



		Private Function LoadTaxExceptionData(ByVal mdNr As Integer, ByVal maNr As Integer, ByVal month As Integer, ByVal year As Integer) As SearchRestulOfTaxData
			Dim result As SearchRestulOfTaxData

			Dim sql As String
			sql = "[Load Tax Exception Data For Assigned Employee]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("@iMANr", maNr))
			listOfParams.Add(New SqlClient.SqlParameter("@siMonth", month))
			listOfParams.Add(New SqlClient.SqlParameter("@nYear", year))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)


			result = New SearchRestulOfTaxData
			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then
					Dim data = New SearchRestulOfTaxData

					data.Exception_KTGBetrag_Amount = SafeGetDecimal(reader, "KTGBetrag", 0)
					data.Exception_SuvaBetrag_Amount = SafeGetDecimal(reader, "SuvaBetrag", 0)
					data.Exception_KiAuBetrag_Amount = SafeGetDecimal(reader, "KiAuBetrag", 0)
					data.Exception_OtherServices_Amount = SafeGetDecimal(reader, "OtherServicesAmounts", 0)
					data.Exception_OtherNotDefined_Amount = SafeGetDecimal(reader, "OtherNotDefinedAmounts", 0)
					data.Exception_OtherServicesLALabel = SafeGetString(reader, "OtherServicesAmountsLALabel")
					data.Exception_OtherNotDefinedLALabel = SafeGetString(reader, "OtherNotDefinedAmountsLALabel")
					data.Exception_SPayed_Amount = SafeGetDecimal(reader, "SPayed", 0)
					data.Exception_SBacked_Amount = SafeGetDecimal(reader, "SBacked", 0)


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


	End Class

End Namespace
