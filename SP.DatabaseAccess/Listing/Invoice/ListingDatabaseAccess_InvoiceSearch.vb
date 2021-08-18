
Imports SP.DatabaseAccess.Listing.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language
Imports SP.DatabaseAccess.Customer.DataObjects

Namespace Listing


	Partial Class ListingDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IListingDatabaseAccess



		Function LoadPaymentReminderCodeInInvoicesData() As IEnumerable(Of PaymentReminderCodeData) Implements IListingDatabaseAccess.LoadPaymentReminderCodeInInvoicesData

			Dim result As List(Of PaymentReminderCodeData) = Nothing

			Dim sql As String

			sql = "[Load Payment reminder Code For Search In Invoices]"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of PaymentReminderCodeData)

					While reader.Read()
						Dim reminderCode As New PaymentReminderCodeData

						reminderCode.Reminder1 = SafeGetString(reader, "Mahn1")
						reminderCode.Reminder2 = SafeGetString(reader, "Mahn2")
						reminderCode.Reminder3 = SafeGetString(reader, "Mahn3")
						reminderCode.Reminder4 = SafeGetString(reader, "Mahn4")
						reminderCode.GetField = SafeGetString(reader, "GetFeld")

						result.Add(reminderCode)

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

		Function LoadInvoiceArtCodeInInvoicesData() As IEnumerable(Of InvoiceArtData) Implements IListingDatabaseAccess.LoadInvoiceArtCodeInInvoicesData

			Dim result As List(Of InvoiceArtData) = Nothing

			Dim sql As String

			sql = "[Load Invoice Art For Search In Invoice]"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of InvoiceArtData)

					While reader.Read()
						Dim reminderCode As New InvoiceArtData

						reminderCode.Art = SafeGetString(reader, "Art")
						reminderCode.ArtLabel = SafeGetString(reader, "ArtLabel")


						result.Add(reminderCode)

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


	End Class


End Namespace
