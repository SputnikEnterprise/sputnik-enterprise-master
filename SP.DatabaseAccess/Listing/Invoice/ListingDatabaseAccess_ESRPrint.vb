
Imports SP.DatabaseAccess.Listing.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language

Namespace Listing


	Partial Class ListingDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IListingDatabaseAccess

		Function LoadESRDataForPrintList(ByVal mdNr As Integer, ByVal diskIdentity As String) As IEnumerable(Of ESRListPrintData) Implements IListingDatabaseAccess.LoadESRDataForPrintList

			Dim result As List(Of ESRListPrintData) = Nothing

			Dim sql As String = String.Empty

			sql = sql & "[Load ESR Data For Print In Listing]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("diskIdentity", ReplaceMissing(diskIdentity, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of ESRListPrintData)

					While reader.Read
						Dim data = New ESRListPrintData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.MDNr = SafeGetInteger(reader, "MDNr", Nothing)
						data.CustomerNumber = SafeGetInteger(reader, "KDNr", Nothing)
						data.invoiceNumber = SafeGetInteger(reader, "RENr", Nothing)
						data.Rec = SafeGetString(reader, "Rec")
						data.VD = SafeGetDateTime(reader, "VD", Nothing)
						data.VT = SafeGetString(reader, "VT")
						data.DiskInfo = SafeGetString(reader, "DiskInfo")
						data.Company = SafeGetString(reader, "R_Name1")

						data.ESRAmount = SafeGetDecimal(reader, "PayedAmount", Nothing)
						data.ESRBookedAmount = SafeGetDecimal(reader, "Betrag", Nothing)
						data.InvoiceAmount = SafeGetDecimal(reader, "BetragInk", Nothing)
						data.InvoicePayed = SafeGetDecimal(reader, "Bezahlt", Nothing)
						data.AmountDecision = SafeGetString(reader, "OK")

						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom", String.Empty)


						result.Add(data)

					End While
				End If

			Catch e As Exception
				m_Logger.LogError(e.ToString())
				result = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		Function LoadPaymentDataForESRPrintList(ByVal mdNr As Integer, ByVal firstPaymentNumber As List(Of Integer)) As IEnumerable(Of ESRPaymentListPrintData) Implements IListingDatabaseAccess.LoadPaymentDataForESRPrintList

			Dim result As List(Of ESRPaymentListPrintData) = Nothing

			Dim sql As String

			'Dim paymentNumbersBuffer As String = String.Empty

			'For Each number In firstPaymentNumber
			'	paymentNumbersBuffer = paymentNumbersBuffer & IIf(paymentNumbersBuffer <> "", ", ", "") & number
			'Next


			sql = "Select ZE.ZENr,"
			sql &= "ZE.MDNr,"
			sql &= "ZE.RENR,"
			sql &= "ZE.KDNR,"
			sql &= "ZE.Fak_Dat,"
			sql &= "ZE.V_Date,"
			sql &= "ZE.B_Date,"
			sql &= "ZE.BETRAG,"
			sql &= "ZE.CreatedOn,"
			sql &= "ZE.CreatedFrom,"
			sql &= "RE.R_Name1 ,"
			sql &= "RE.BetragInk,"
			sql &= "RE.Bezahlt "
			sql &= "FROM dbo.ZE ZE "
			sql &= "LEFT Join dbo.RE RE ON ZE.RENr = RE.RENr "

			sql &= "WHERE ZE.MDNr = @MDNr "
			sql &= "AND ZE.Storniert = 0 "
			sql &= "AND (ZE.ZENr >= "
			sql &= String.Format(" {0} ) ", firstPaymentNumber(0))
			sql &= "ORDER BY RE.R_Name1, RE.RENR"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNr, 0)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try
				If reader IsNot Nothing Then

					result = New List(Of ESRPaymentListPrintData)

					While reader.Read
						Dim data = New ESRPaymentListPrintData

						data.ZENr = SafeGetInteger(reader, "ZENr", Nothing)
						data.MDNr = SafeGetInteger(reader, "MDNr", Nothing)
						data.CustomerNumber = SafeGetInteger(reader, "KDNr", Nothing)
						data.InvoiceNumber = SafeGetInteger(reader, "RENr", Nothing)

						data.Fak_Date = SafeGetDateTime(reader, "Fak_Dat", Nothing)
						data.ValutaOn = SafeGetDateTime(reader, "V_Date", Nothing)
						data.BookedOn = SafeGetDateTime(reader, "B_Date", Nothing)

						data.Company = SafeGetString(reader, "R_Name1")
						data.Amount = SafeGetDecimal(reader, "Betrag", Nothing)
						data.InvoiceAmount = SafeGetDecimal(reader, "BetragInk", Nothing)
						data.InvoicePayed = SafeGetDecimal(reader, "Bezahlt", Nothing)
						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.Createdfrom = SafeGetString(reader, "CreatedFrom", String.Empty)


						result.Add(data)

					End While
				End If

			Catch e As Exception
				m_Logger.LogError(e.ToString())
				result = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		Function LoadMandantDataForPrintDTAESRListData(ByVal mdNr As Integer, ByVal bankIDNumber As Integer?) As MandantDataForPrintDTAESRListing Implements IListingDatabaseAccess.LoadMandantDataForPrintDTAESRListData

			Dim result As MandantDataForPrintDTAESRListing = Nothing

			Dim sql As String

			sql = "[Get MDData for DTAList]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("BankIDNumber", bankIDNumber.GetValueOrDefault(0)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If reader IsNot Nothing Then

					result = New MandantDataForPrintDTAESRListing

					While reader.Read

						Dim data = New MandantDataForPrintDTAESRListing

						data.BankAdresse = SafeGetString(reader, "BankAdresse")
						data.BankClnr = SafeGetString(reader, "BankClnr")
						data.BankName = SafeGetString(reader, "BankName")

						data.ChangedFrom = SafeGetString(reader, "ChangedFrom")
						data.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)

						data.DTAAdr1 = SafeGetString(reader, "DTAAdr1")
						data.DTAAdr2 = SafeGetString(reader, "DTAAdr2")
						data.DTAAdr3 = SafeGetString(reader, "DTAAdr3")
						data.DTAAdr4 = SafeGetString(reader, "DTAAdr4")
						data.DTAClnr = SafeGetString(reader, "DTAClnr")
						data.DTAIBAN = SafeGetString(reader, "DTAIBAN")
						data.ESRFileName = SafeGetString(reader, "ESRFileName")

						data.ESRIBAN1 = SafeGetString(reader, "ESRIBAN1")
						data.ESRIBAN2 = SafeGetString(reader, "ESRIBAN2")
						data.ESRIBAN3 = SafeGetString(reader, "ESRIBAN3")
						data.KontoDTA = SafeGetString(reader, "KontoDTA")

						data.KontoESR1 = SafeGetString(reader, "KontoESR1")
						data.KontoESR2 = SafeGetString(reader, "KontoESR2")
						data.KontoVG = SafeGetString(reader, "KontoVG")
						data.MD_ID = SafeGetString(reader, "MD_ID")
						data.RecBez = SafeGetString(reader, "RecBez")
						data.RecNr = SafeGetInteger(reader, "RecNr", 0)

						data.Swift = SafeGetString(reader, "Swift")
						data.VGIBAN = SafeGetString(reader, "VGIBAN")


						result = data

					End While

				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))
				Return Nothing

			End Try

			Return result
		End Function


	End Class

End Namespace
