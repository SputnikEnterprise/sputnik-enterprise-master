
Imports System.Data.SqlClient
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.DatabaseAccess.Listing


Namespace Listing


	Partial Class ListingDatabaseAccess

		Inherits DatabaseAccessBase
		Implements IListingDatabaseAccess


		Function LoadAllTwixCustomerMasterData(ByVal mdNr As Integer) As IEnumerable(Of CustomerMasterData) Implements IListingDatabaseAccess.LoadAllTwixCustomerMasterData

			Dim result As List(Of CustomerMasterData) = Nothing

			Dim sql As String

			sql = "Select KDNr"
			sql &= ", ISNULL([Name], '') Firma1"
			sql &= ", ISNULL(Strasse, '') Strasse"
			sql &= ", CONVERT(NVARCHAR(10), ISNULL(PLZ, '')) PLZ"
			sql &= ", ISNULL(Ort, '') Ort"
			sql &= ", ISNULL(Kanton, '') Kanton"
			sql &= ", ISNULL(Telefon, '') Telefon"
			sql &= ", ISNULL(EMail, '') EMail"
			sql &= ", ISNULL(Homepage, '') Homepage"
			sql &= ", ISNULL(Branche, '') Branche"
			sql &= ", ISNULL(Berufe, '') Berufe "
			sql &= "From tblKunde Where [Name] Is Not Null Order By ISNULL([Name], '')"


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing, CommandType.Text)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CustomerMasterData)

					While reader.Read

						Dim data = New CustomerMasterData

						data.CustomerMandantNumber = mdNr
						data.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)

						data.Company1 = SafeGetString(reader, "Firma1")
						data.Street = SafeGetString(reader, "Strasse")
						data.CountryCode = "CH"
						data.Postcode = SafeGetString(reader, "PLZ")

						data.Location = SafeGetString(reader, "Ort")
						data.Telephone = SafeGetString(reader, "Telefon")
						data.EMail = SafeGetString(reader, "EMail")
						data.Hompage = SafeGetString(reader, "Homepage")

						data.Language = "de"
						data.KDBusinessBranch = SafeGetString(reader, "Branche")
						data.facebook = SafeGetString(reader, "Berufe")

						data.CreatedOn = Now
						data.CreatedFrom = "System"
						data.ChangedOn = Nothing


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
