
Imports SP.DatabaseAccess.Common.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language

Namespace Common


	Partial Class CommonDatabaseAccess
		Inherits DatabaseAccessBase
		Implements ICommonDatabaseAccess



		''' <summary>
		''' Loads contact type1 data.
		''' </summary>
		''' <returns>List of contact type1 data.</returns>
		Function LoadContactTypeData1() As IEnumerable(Of ContactType1Data) Implements ICommonDatabaseAccess.LoadContactTypeData1

			Dim result As List(Of ContactType1Data) = Nothing

			Dim sql As String

			sql = "SELECT ID, Bez_ID, IconIndex, Result, RecNr, Bez_DE, Bez_FR, Bez_IT, Bez_EN FROM Tab_KontaktType1 ORDER BY RecNr ASC"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ContactType1Data)

					While reader.Read

						Dim contactType1Data = New ContactType1Data()
						contactType1Data.ID = SafeGetInteger(reader, "ID", 0)
						contactType1Data.Bez_ID = SafeGetString(reader, "Bez_ID")
						contactType1Data.IconIndex = SafeGetInteger(reader, "IconIndex", Nothing)
						contactType1Data.Result = SafeGetString(reader, "Result")
						contactType1Data.RecNr = SafeGetInteger(reader, "RecNr", Nothing)
						contactType1Data.Caption_DE = SafeGetString(reader, "Bez_DE")
						contactType1Data.Caption_FR = SafeGetString(reader, "Bez_FR")
						contactType1Data.Caption_IT = SafeGetString(reader, "Bez_IT")
						contactType1Data.Caption_EN = SafeGetString(reader, "Bez_EN")

						result.Add(contactType1Data)

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
