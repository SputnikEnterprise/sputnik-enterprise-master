
Imports SP.DatabaseAccess.Listing.DataObjects

Namespace Listing



	Partial Class ListingDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IListingDatabaseAccess


		Function LoadOfferDataToSendEmail(ByVal eMailType As String) As IEnumerable(Of EMailOfferData) Implements IListingDatabaseAccess.LoadOfferDataToSendEmail
			Dim result As List(Of EMailOfferData) = Nothing

			Dim SQL As String

			SQL = "[List OfferData For Search OfferSendig]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			Dim reader As SqlClient.SqlDataReader = OpenReader(SQL, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EMailOfferData)

					While reader.Read()
						Dim overviewData As New EMailOfferData

						overviewData.OfferNumber = SafeGetInteger(reader, "Nummer", 0)
						overviewData.OfferLabel = SafeGetString(reader, "Bezeichnung")


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



	End Class


End Namespace
