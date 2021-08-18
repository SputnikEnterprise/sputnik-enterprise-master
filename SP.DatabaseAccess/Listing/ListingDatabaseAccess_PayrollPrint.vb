
Imports SP.DatabaseAccess.Listing.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language


Namespace Listing


	Partial Class ListingDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IListingDatabaseAccess


		Function LoadPayrollsPrintData(ByVal searchdata As PayrollSearchData) As IEnumerable(Of PayrollPrintData) Implements IListingDatabaseAccess.LoadPayrollsPrintData
			Dim result As List(Of PayrollPrintData) = Nothing

			Dim monthBuffer As String = String.Empty
			Dim yearBuffer As String = String.Empty
			Dim manrBuffer As String = String.Empty
			Dim lonrBuffer As String = String.Empty

			If Not searchdata.monat Is Nothing AndAlso searchdata.monat.Count > 0 Then
				For Each number In searchdata.monat
					If Val(number) > 0 Then monthBuffer &= IIf(monthBuffer <> "", ", ", "") & number
				Next
			End If

			If Not searchdata.jahr Is Nothing AndAlso searchdata.jahr.Count > 0 Then
				For Each number In searchdata.jahr
					If Val(number) > 0 Then yearBuffer &= IIf(yearBuffer <> "", ", ", "") & number
				Next
			End If

			If Not searchdata.MANr Is Nothing AndAlso searchdata.MANr.Count > 0 Then
				For Each number In searchdata.MANr
					If Val(number) > 0 Then manrBuffer &= IIf(manrBuffer <> "", ", ", "") & number
				Next
			End If

			If Not searchdata.LONr Is Nothing AndAlso searchdata.LONr.Count > 0 Then
				For Each number In searchdata.LONr
					If Val(number) > 0 Then lonrBuffer &= IIf(lonrBuffer <> "", ", ", "") & number
				Next
			End If


			Dim sql As String

			sql = "SELECT LO.ID, LO.MDNr, LO.LONr, LO.MANr, "
			sql &= "Convert(Bit, (CASE "
			sql &= "WHEN ISNULL(MA.email, '') = '' THEN 0 "
			sql &= "Else ISNULL(MA.send2wos, 0) "
			sql &= "End "
			sql &= ")) SendData2WOS, "

			sql &= "Convert(Bit, (CASE "
			sql &= "WHEN ISNULL(MA.email, '') = '' THEN 0 "
			sql &= "Else ISNULL(MA.SendDataWithEMail, 0) "
			sql &= "End "
			sql &= ")) SendDataWithEMail, "

			sql &= "IsNull(MA.Sprache, 'deutsch') As Sprache, "
			sql &= "IsNull(LO.LMID, 0) As LMID, "
			sql &= "IsNull(LO.ZGNr, 0) As ZGNr, "

			sql &= "(Select Top (1) VGNr From LOL Where LOL.MDNr = LO.MDNr AND LOL.MANr = LO.MANr And LOL.LONr = LO.LONr AND LOL.LANr = 8730 AND IsNull(LOL.VGNr, 0) > 0 AND IsNull(LOL.m_btr, 0) <> 0) LpVGNr, "

			sql &= "IsNull(LO.LODoc_Guid, '') As LOGuid, "
			sql &= "IsNull(MA.Transfered_Guid, '') As MAGuid, "
			sql &= "Convert(int, LO.LP) LP, "
			sql &= "Convert(Int, LO.Jahr) Jahr, "
			sql &= "MA.Nachname, MA.Vorname, "
			sql &= "MA.Geschlecht Gender, "
			sql &= "IsNull(MA.EMail, '') EmployeeEMail, "
			sql &= "IsNull( (Select Top (1) SendAsZIP From dbo.MA_LOSetting L Where L.MANr = MA.MANr), 0) SendAsZIP,  "
			sql &= "LO.CreatedOn, "
			sql &= "LO.CreatedFrom "

			sql &= "FROM LO "
			sql &= "Left Join Mitarbeiter MA On LO.MANr = MA.MANr "

			sql &= "Where (LO.IsComplete = 1) "
			sql &= "And (@MDNr = 0 OR LO.MDNr = @MDNr) "


			If Not String.IsNullOrWhiteSpace(monthBuffer) Then sql &= String.Format("And LO.LP In ({0}) ", monthBuffer)
			If Not String.IsNullOrWhiteSpace(yearBuffer) Then sql &= String.Format("And LO.Jahr In ({0}) ", yearBuffer)
			If Not String.IsNullOrWhiteSpace(manrBuffer) Then sql &= String.Format("And LO.MANr In ({0}) ", manrBuffer)
			If Not String.IsNullOrWhiteSpace(lonrBuffer) Then sql &= String.Format("And LO.LONr In ({0}) ", lonrBuffer)
			If searchdata.mawos <> PayrollSearchData.WOSValue.All Then
				sql &= String.Format("And (MA.Send2WOS In ({0}) And IsNull(MA.EMail, '') <> '') ", ReplaceMissing(CInt(searchdata.mawos), CInt(PayrollSearchData.WOSValue.All)))
			End If

			sql &= "Order BY "
			Select Case searchdata.sortvalue
				Case 0
					sql &= "MA.Nachname ASC, MA.Vorname ASC, LO.Jahr DESC, LO.LP Desc, LO.CreatedOn Desc "
				Case 1
					sql &= "LO.MANr "

				Case 2
					sql &= "LO.CreatedOn "

				Case Else
					sql &= "MA.Nachname ASC, MA.Vorname ASC, LO.Jahr DESC, LO.LP Desc, LO.CreatedOn Desc "

			End Select


			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(searchdata.MDNr, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)


			Try

				If (Not reader Is Nothing) Then

					result = New List(Of PayrollPrintData)

					While reader.Read

						Dim data = New PayrollPrintData()

						data.recID = SafeGetInteger(reader, "ID", 0)
						data.MDNr = SafeGetInteger(reader, "MDNr", 0)
						data.jahr = SafeGetInteger(reader, "jahr", 0)
						data.monat = SafeGetInteger(reader, "LP", 0)
						data.LONr = SafeGetInteger(reader, "LONr", 0)
						data.MANr = SafeGetInteger(reader, "manr", 0)
						data.LMID = SafeGetInteger(reader, "LMID", 0)
						data.lpVGNr = SafeGetInteger(reader, "LpVGNr", 0)
						data.ZGNumber = SafeGetInteger(reader, "ZGNr", 0)

						data.employeefirstname = SafeGetString(reader, "vorname")
						data.employeelastname = SafeGetString(reader, "nachname")
						data.EmployeeEMail = SafeGetString(reader, "EmployeeEMail")
						data.SendDataWithEMail = SafeGetBoolean(reader, "SendDataWithEMail", False)
						data.SendAsZIP = SafeGetBoolean(reader, "SendAsZIP", False)

						data.Send2WOS = SafeGetBoolean(reader, "SendData2WOS", False)
						Dim lang = SafeGetString(reader, "Sprache")
						If String.IsNullOrWhiteSpace(lang) Then lang = "D" Else lang = lang.ToUpper.ToString.Substring(0, 1)
						data.employeeLanguage = lang

						data.Gender = SafeGetString(reader, "Gender")
						data.EmployeeGuid = SafeGetString(reader, "MAGuid")
						data.PayrollGuid = SafeGetString(reader, "LOGuid")

						data.createdon = SafeGetDateTime(reader, "createdon", Nothing)
						data.createdfrom = SafeGetString(reader, "CreatedFrom")

						If searchdata.GroupByEMail.GetValueOrDefault(False) Then
							If data.SendDataWithEMail.GetValueOrDefault(False) AndAlso Not String.IsNullOrWhiteSpace(data.EmployeeEMail) Then result.Add(data)
						Else
							result.Add(data)
						End If


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

		Function LoadPayrollsDetailData(ByVal searchdata As PayrollSearchData) As IEnumerable(Of PayrollDetailData) Implements IListingDatabaseAccess.LoadPayrollsDetailData
			Dim result As List(Of PayrollDetailData) = Nothing

			Dim monthBuffer As String = String.Empty
			Dim yearBuffer As String = String.Empty
			Dim manrBuffer As String = String.Empty
			Dim lonrBuffer As String = String.Empty

			For Each number In searchdata.monat
				If Val(number) > 0 Then monthBuffer &= IIf(monthBuffer <> "", ", ", "") & number
			Next
			For Each number In searchdata.jahr
				If Val(number) > 0 Then yearBuffer &= IIf(yearBuffer <> "", ", ", "") & number
			Next

			For Each number In searchdata.MANr
				If Val(number) > 0 Then manrBuffer &= IIf(manrBuffer <> "", ", ", "") & number
			Next
			For Each number In searchdata.LONr
				If Val(number) > 0 Then lonrBuffer &= IIf(lonrBuffer <> "", ", ", "") & number
			Next

			Dim sql As String

			sql = "SELECT LOL.ID, LOL.MDNr, Convert(Int, LOL.LP) LP, Convert(Int, LOL.Jahr) Jahr, LOL.LONr, LOL.MANr, "
			sql &= "LOL.m_Anz, LOL.m_Bas, LOL.m_Ans , LOL.m_btr, "
			sql &= "LOL.LANr, LOL.RPText LALoText, "
			sql &= "IsNull(MA.Send2WOS, 0) As Send2WOS, "
			sql &= "IsNull(MA.Sprache, 'deutsch') As Sprache, "
			sql &= "MA.Nachname, MA.Vorname "

			sql &= "FROM LOL "
			sql &= "Left Join Mitarbeiter MA On LOL.MANr = MA.MANr "
			sql &= "Left Join LA On LOL.LANr = LA.LANr And LA.LAJahr = LOL.Jahr AND LA.LADeactivated = 0 "

			sql &= "Where "
			sql &= "LOL.MDNr = @MDNr "
			sql &= "And (LA.Kumulativ = 0 And LA.KumulativMonth = 0 And LA.nolisting <> 1) "


			If Not String.IsNullOrWhiteSpace(monthBuffer) Then sql &= String.Format("And LOL.LP In ({0}) ", monthBuffer)
			If Not String.IsNullOrWhiteSpace(yearBuffer) Then sql &= String.Format("And LOL.Jahr In ({0}) ", yearBuffer)
			If Not String.IsNullOrWhiteSpace(manrBuffer) Then sql &= String.Format("And LOL.MANr In ({0}) ", manrBuffer)
			If Not String.IsNullOrWhiteSpace(lonrBuffer) Then sql &= String.Format("And LOL.LONr In ({0}) ", lonrBuffer)
			If searchdata.mawos <> PayrollSearchData.WOSValue.All Then sql &= String.Format("And MA.Send2WOS In ({0}) ", ReplaceMissing(CInt(searchdata.mawos), CInt(PayrollSearchData.WOSValue.All)))

			sql &= "Order By MA.Nachname ASC, MA.Vorname ASC, LOL.LONr, LOL.Jahr Desc, LOL.LP Desc, LOL.LANr ASC "


			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(searchdata.MDNr, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)


			Try

				If (Not reader Is Nothing) Then

					result = New List(Of PayrollDetailData)

					While reader.Read

						Dim data = New PayrollDetailData()

						data.recID = SafeGetInteger(reader, "ID", 0)
						data.MDNr = SafeGetInteger(reader, "MDNr", 0)
						data.jahr = SafeGetInteger(reader, "jahr", 0)
						data.monat = SafeGetInteger(reader, "LP", 0)
						data.LONr = SafeGetInteger(reader, "LONr", 0)
						data.MANr = SafeGetInteger(reader, "manr", 0)

						data.employeefirstname = SafeGetString(reader, "vorname")
						data.employeelastname = SafeGetString(reader, "nachname")

						data.Send2WOS = SafeGetBoolean(reader, "Send2WOS", False)
						data.employeeLanguage = SafeGetString(reader, "Sprache")

						data.LANr = SafeGetDecimal(reader, "lanr", Nothing)
						data.m_Anz = SafeGetDecimal(reader, "m_anz", Nothing)
						data.m_Bas = SafeGetDecimal(reader, "m_bas", Nothing)
						data.m_Ans = SafeGetDecimal(reader, "m_ans", Nothing)
						data.m_btr = SafeGetDecimal(reader, "m_btr", Nothing)
						data.LALoText = SafeGetString(reader, "LALoText")


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

		Function DeletePayroll(ByVal mdNumber As Integer, ByVal payrollNumber As Integer, ByVal modul As String, ByVal username As String, ByVal usnr As Integer) As DeletePayrollResult Implements IListingDatabaseAccess.DeletePayroll

			Dim success = True

			Dim sql As String = String.Empty

			sql = "[Delete Assigned Payroll Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdnr", mdNumber))
			listOfParams.Add(New SqlClient.SqlParameter("LONr", payrollNumber))
			listOfParams.Add(New SqlClient.SqlParameter("Username", ReplaceMissing(username, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Usnr", ReplaceMissing(usnr, DBNull.Value)))

			Dim resultParameter = New SqlClient.SqlParameter("@Result", SqlDbType.Int)
			resultParameter.Direction = ParameterDirection.Output
			listOfParams.Add(resultParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Dim resultEnum As DeletePayrollResult

			If Not resultParameter.Value Is Nothing Then
				Try
					resultEnum = CType(resultParameter.Value, DeletePayrollResult)
				Catch
					resultEnum = DeletePayrollResult.ErrorWhileDelete
				End Try
			Else
				resultEnum = DeletePayrollResult.ErrorWhileDelete
			End If

			Return resultEnum

		End Function



	End Class


End Namespace


