Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Report.DataObjects

Namespace Listing


	Partial Class ListingDatabaseAccess

		Inherits DatabaseAccessBase
		Implements IListingDatabaseAccess


		Function LoadAllCustomerMasterData() As IEnumerable(Of CustomerMasterData) Implements IListingDatabaseAccess.LoadAllCustomerMasterData

			Dim result As List(Of CustomerMasterData) = Nothing

			Dim sql As String

			sql = "[Load All Customers Data]"


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CustomerMasterData)

					While reader.Read

						Dim data = New CustomerMasterData

						data.CustomerMandantNumber = SafeGetInteger(reader, "MDNr", 0)
						data.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)
						data.WOSGuid = SafeGetString(reader, "Transfered_Guid", String.Empty)

						' TODO: Anhand Int-Wert das Impage für btnSolvency auswählen...
						'data.SolvencyDecisionID = SafeGetInteger(reader, "DecisionID", 0)
						'data.SolvencyInfo = SafeGetString(reader, "SolvencyData")

						data.Company1 = SafeGetString(reader, "Firma1")
						data.Company2 = SafeGetString(reader, "Firma2")
						data.Company3 = SafeGetString(reader, "Firma3")
						data.Street = SafeGetString(reader, "Strasse")
						data.CountryCode = SafeGetString(reader, "Land")
						data.Postcode = SafeGetString(reader, "PLZ")
						data.Latitude = SafeGetDouble(reader, "Latitude", 0)
						data.Longitude = SafeGetDouble(reader, "Longitude", 0)

						data.PostOfficeBox = SafeGetString(reader, "Postfach")
						data.Location = SafeGetString(reader, "Ort")
						data.Telephone = SafeGetString(reader, "Telefon")
						data.Telefax = SafeGetString(reader, "Telefax")
						data.Telefax_Mailing = SafeGetBoolean(reader, "KD_Telefax_Mailing", False)
						data.EMail = SafeGetString(reader, "EMail")
						data.Email_Mailing = SafeGetBoolean(reader, "KD_Mail_Mailing", False)
						data.Hompage = SafeGetString(reader, "Homepage")
						data.facebook = SafeGetString(reader, "facebook")
						data.xing = SafeGetString(reader, "xing")
						data.KST = SafeGetString(reader, "KST")
						data.FirstProperty = SafeGetDecimal(reader, "FProperty", Nothing)
						data.Language = SafeGetString(reader, "Sprache")
						data.HowContact = SafeGetString(reader, "HowKontakt")
						data.CustomerState1 = SafeGetString(reader, "KDState1")
						data.CustomerState2 = SafeGetString(reader, "KDState2")
						data.NoUse = SafeGetBoolean(reader, "NoES", False)
						data.NoUseComment = SafeGetString(reader, "NOESBez")
						data.SalaryPerMonth = SafeGetDecimal(reader, "GehaltPerMonth", Nothing)
						data.SalaryPerHour = SafeGetDecimal(reader, "GehaltPerStd", Nothing)
						data.Reserve1 = SafeGetString(reader, "KDRes1")
						data.Reserve2 = SafeGetString(reader, "KDRes2")
						data.Reserve3 = SafeGetString(reader, "KDRes3")
						data.Reserve4 = SafeGetString(reader, "KDRes4")
						data.CreditLimit1 = SafeGetDecimal(reader, "KreditLimite", 0)
						data.CreditLimit2 = SafeGetDecimal(reader, "Kreditlimite_2", 0)
						data.CreditLimitsFromDate = SafeGetDateTime(reader, "KreditlimiteAb", Nothing)
						data.CreditLimitsToDate = SafeGetDateTime(reader, "KreditlimiteBis", Nothing)
						'data.OpenInvoiceAmount = SafeGetDecimal(reader, "OpenInvoiceAmount", 0)
						data.ReferenceNumber = SafeGetString(reader, "KL_RefNr")
						data.KD_UmsMin = SafeGetDecimal(reader, "KD_UmsMin", 0)
						data.mwstpflicht = SafeGetBoolean(reader, "mwst", 1)
						data.NumberOfCopies = SafeGetShort(reader, "AnzKopien", Nothing)
						data.ValueAddedTaxNumber = SafeGetString(reader, "MwStNr", Nothing)
						data.CreditWarning = SafeGetBoolean(reader, "KreditWarnung", False)
						data.OPShipment = SafeGetString(reader, "OPVersand", "")
						data.NotPrintReports = SafeGetBoolean(reader, "PrintNoRP", False)
						data.TermsAndConditions_WOS = SafeGetString(reader, "AGB_WOS")
						data.sendToWOS = SafeGetBoolean(reader, "Send2WOS", False)
						data.DoNotShowContractInWOS = SafeGetBoolean(reader, "DoNotShowContractInWOS", False)
						data.CurrencyCode = SafeGetString(reader, "Currency")
						data.BillTypeCode = SafeGetString(reader, "Faktura")
						data.NumberOfEmployees = SafeGetString(reader, "MAAnzahl")
						data.CanteenAvailable = SafeGetBoolean(reader, "Kantine", False)
						data.TransportationOptions = SafeGetBoolean(reader, "Transport", False)
						data.InvoiceOption = SafeGetString(reader, "FakturaOption")
						data.ShowHoursInNormal = SafeGetBoolean(reader, "ShowHoursInNormal", False)

						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						data.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
						data.ChangedFrom = SafeGetString(reader, "ChangedFrom")
						data.Transfered_Guid = SafeGetString(reader, "Transfered_Guid")

						data.Comment = SafeGetString(reader, "Notice_Common")
						data.Notice_Employment = SafeGetString(reader, "Notice_Employment")
						data.Notice_Report = SafeGetString(reader, "Notice_Report")
						data.Notice_Invoice = SafeGetString(reader, "Notice_Invoice")
						data.Notice_Payment = SafeGetString(reader, "Notice_Payment")

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

		Function LoadAllCustomerCountryCodeData() As IEnumerable(Of CustomerMasterData) Implements IListingDatabaseAccess.LoadAllCustomerCountryCodeData

			Dim result As List(Of CustomerMasterData) = Nothing

			Dim sql As String

			sql = "Select "
			sql &= "[Land] "

			sql &= "FROM Kunden "
			sql &= "WHERE ISNULL(land, '') <> '' "
			sql &= "GROUP BY Land "
			sql &= "ORDER BY Land"


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing, CommandType.Text)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CustomerMasterData)

					While reader.Read

						Dim data = New CustomerMasterData

						data.CountryCode = SafeGetString(reader, "Land")


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

		Function UpdateCustomerCountryData(ByVal oldCountryCode As String, ByVal newCountryCode As String) As Boolean Implements IListingDatabaseAccess.UpdateCustomerCountryData

			Dim success = True

			Dim sql As String

			sql = "Update [dbo].[Kunden] Set "
			sql &= "[Land] = @newCode "

			sql &= " WHERE Land = @oldCode; "

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("oldCode", ReplaceMissing(oldCountryCode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("newCode", ReplaceMissing(newCountryCode, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)


			Return success

		End Function

	End Class


End Namespace
