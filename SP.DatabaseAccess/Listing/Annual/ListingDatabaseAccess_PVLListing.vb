
Imports SP.DatabaseAccess.Listing.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language


Namespace Listing



	Partial Class ListingDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IListingDatabaseAccess



		''' <summary>
		''' Loas inkassopool pauschal.
		''' </summary>
		Public Function LoadInkassopoolPauschale(ByVal mdNr As Integer, ByVal gavBeruf As String, ByVal gavcanton As String, ByVal firstmonat As Integer, ByVal lastmonat As Integer, ByVal year As Integer, ByVal employeenumbers As String) As Decimal? Implements IListingDatabaseAccess.LoadInkassopoolPauschale

			Dim pauschale As Decimal? = Nothing

			Dim sql As String

			If gavBeruf.ToLower.Contains("gebäudetechnik") Then
				sql = "[Get InkassoPool Pauschale For Month With Mandant]"
			Else
				sql = "[Get InkassoPool Pauschale With Mandant]"
			End If

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("@jahr", ReplaceMissing(year, Now.Year)))
			listOfParams.Add(New SqlClient.SqlParameter("@vonMonat", ReplaceMissing(firstmonat, Now.Month)))
			listOfParams.Add(New SqlClient.SqlParameter("@bisMonat", ReplaceMissing(lastmonat, ReplaceMissing(firstmonat, Now.Month))))
			listOfParams.Add(New SqlClient.SqlParameter("@gavKanton", ReplaceMissing(gavcanton, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("@gavBeruf", ReplaceMissing(gavBeruf, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("@MANRList", ReplaceMissing(employeenumbers, String.Empty)))


			pauschale = ExecuteScalar(sql, listOfParams, CommandType.StoredProcedure)

			Return pauschale
		End Function


		''' <summary>
		''' Loas GEFAK pauschal.
		''' </summary>
		Public Function LoadGEFAKPauschale(ByVal mdNr As Integer, ByVal gavBeruf As String, ByVal firstmonat As Integer, ByVal lastmonat As Integer, ByVal year As Integer, ByVal employeenumbers As String) As Decimal? Implements IListingDatabaseAccess.LoadGEFAKPauschale

			Dim pauschale As Decimal? = Nothing

			Dim sql As String

			sql = "[Get GEFAK Pauschale With Mandant]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("@jahr", ReplaceMissing(year, Now.Year)))
			listOfParams.Add(New SqlClient.SqlParameter("@vonMonat", ReplaceMissing(firstmonat, Now.Month)))
			listOfParams.Add(New SqlClient.SqlParameter("@bisMonat", ReplaceMissing(lastmonat, ReplaceMissing(firstmonat, Now.Month))))
			listOfParams.Add(New SqlClient.SqlParameter("@gavBeruf", ReplaceMissing(gavBeruf, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("@MANRList", ReplaceMissing(employeenumbers, String.Empty)))


			pauschale = ExecuteScalar(sql, listOfParams, CommandType.StoredProcedure)

			Return pauschale
		End Function


	End Class

End Namespace
