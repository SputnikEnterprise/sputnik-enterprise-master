
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Report.DataObjects

Namespace Listing

	Partial Class ListingDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IListingDatabaseAccess

		Function LoadAllEmployeeMasterData(Optional includeImageData As Boolean = False) As IEnumerable(Of EmployeeMasterData) Implements IListingDatabaseAccess.LoadAllEmployeeMasterData

			Dim result As List(Of EmployeeMasterData) = Nothing

			Dim sql As String

			If includeImageData Then
				sql = "[Load Assigned Employee Master Data]"
			Else
				sql = "[Load Assigned Employee Master Data Without Picture]"
			End If


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", ReplaceMissing(0, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("employeeNumber", DBNull.Value))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeMasterData)

					While reader.Read

						Dim data = New EmployeeMasterData

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.EmployeeNumber = SafeGetInteger(reader, "MANr", 0)
						data.Lastname = SafeGetString(reader, "Nachname")
						data.Firstname = SafeGetString(reader, "Vorname")
						data.PostOfficeBox = SafeGetString(reader, "Postfach")
						data.Latitude = SafeGetDouble(reader, "Latitude", 0)
						data.Longitude = SafeGetDouble(reader, "Longitude", 0)

						data.Street = SafeGetString(reader, "Strasse")
						data.Postcode = SafeGetString(reader, "PLZ")
						data.Location = SafeGetString(reader, "Ort")
						data.Country = SafeGetString(reader, "Land")
						data.Language = SafeGetString(reader, "Sprache")
						data.Birthdate = SafeGetDateTime(reader, "GebDat", Nothing)
						data.Gender = SafeGetString(reader, "Geschlecht")
						data.AHV_Nr = SafeGetString(reader, "AHV_Nr")
						data.Nationality = SafeGetString(reader, "Nationality")
						data.CivilStatus = SafeGetString(reader, "Zivilstand")
						data.Telephone_P = SafeGetString(reader, "Telefon_P")
						data.Telephone2 = SafeGetString(reader, "Telefon2")
						data.Telephone3 = SafeGetString(reader, "Telefon3")
						data.Telephone_G = SafeGetString(reader, "Telefon_G")
						data.MobilePhone = SafeGetString(reader, "Natel")
						data.Homepage = SafeGetString(reader, "Homepage")
						data.Email = SafeGetString(reader, "eMail")
						data.Permission = SafeGetString(reader, "Bewillig")
						data.PermissionToDate = SafeGetDateTime(reader, "Bew_Bis", Nothing)
						data.BirthPlace = SafeGetString(reader, "GebOrt")
						data.CHPartner = SafeGetBoolean(reader, "CHPartner", False)
						data.ValidatePermissionWithTax = SafeGetBoolean(reader, "ValidatePermissionWithTax", False)
						data.NoSpecialTax = SafeGetBoolean(reader, "NoSpecialTax", False)
						data.Q_Steuer = SafeGetString(reader, "Q_Steuer")
						data.S_Canton = SafeGetString(reader, "S_Kanton")
						data.ChurchTax = SafeGetString(reader, "Kirchensteuer")
						data.Residence = SafeGetBoolean(reader, "Ansaessigkeit", Nothing)
						data.ChildsCount = SafeGetShort(reader, "Kinder", Nothing)
						data.Profession = SafeGetString(reader, "Beruf")
						data.StaysAt = SafeGetString(reader, "Wohnt_bei")

						data.V_Hint = SafeGetString(reader, "V_Hinweis")
						data.Notice_Employment = SafeGetString(reader, "Notice_Employment")
						data.Notice_Report = SafeGetString(reader, "Notice_Report")
						data.Notice_AdvancedPayment = SafeGetString(reader, "Notice_AdvancedPayment")
						data.Notice_Payroll = SafeGetString(reader, "Notice_Payroll")

						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						data.ChangedFrom = SafeGetString(reader, "ChangedFrom")
						data.HasImage = SafeGetBoolean(reader, "Bild", False)
						If (includeImageData) Then
							data.MABild = SafeGetByteArray(reader, "MABild")
						End If
						data.Result = SafeGetString(reader, "Result")
						data.KST = SafeGetString(reader, "KST")

						data.InZV = SafeGetBoolean(reader, "InZV", False)
						data.KStat1 = SafeGetString(reader, "KStat1")
						data.KStat2 = SafeGetString(reader, "KStat2")
						data.KontaktHow = SafeGetString(reader, "KontaktHow")

						data.FirstContact = SafeGetDateTime(reader, "ErstKontakt", Nothing)
						data.LastContact = SafeGetDateTime(reader, "LetztKontakt", Nothing)
						data.QSTCommunity = SafeGetString(reader, "QSTGemeinde")

						data.TaxCommunityLabel = SafeGetString(reader, "TaxCommunityLabel")
						data.TaxCommunityCode = SafeGetInteger(reader, "TaxCommunityCode", 0)

						data.BusinessBranch = SafeGetString(reader, "Filiale")
						data.GAVBez = SafeGetString(reader, "GAVBez")
						data.CivilState2 = SafeGetString(reader, "Zivilstand2")
						data.QLand = SafeGetString(reader, "QLand")
						data.MABusinessBranch = SafeGetString(reader, "MAFiliale")
						data.AHV_Nr_New = SafeGetString(reader, "AHV_Nr_New")
						data.MA_Canton = SafeGetString(reader, "MA_Kanton")
						data.ANS_OST_Bis = SafeGetDateTime(reader, "Ans_QST_Bis", Nothing)
						data.Transfered_Guid = SafeGetString(reader, "Transfered_Guid")
						data.Transfered_On = SafeGetDateTime(reader, "Transfered_On", Nothing)
						data.Send2WOS = SafeGetBoolean(reader, "Send2WOS", Nothing)
						data.SendDataWithEMail = SafeGetBoolean(reader, "SendDataWithEMail", Nothing)
						data.MA_SMS_Mailing = SafeGetBoolean(reader, "MA_SMS_Mailing", Nothing)
						data.MA_EMail_Mailing = SafeGetBoolean(reader, "MA_EMail_Mailing", Nothing)
						data.ProfessionCode = SafeGetInteger(reader, "BerufCode", Nothing)
						data.WOSGuid = SafeGetString(reader, "Transfered_Guid")
						data.MDNr = SafeGetInteger(reader, "MDnr", Nothing)
						data.Facebook = SafeGetString(reader, "facebook")
						data.LinkedIn = SafeGetString(reader, "linkedIn")
						data.Xing = SafeGetString(reader, "xing")
						data.MobilePhone2 = SafeGetString(reader, "Natel2")
						data.ShowAsApplicant = SafeGetBoolean(reader, "ShowAsApplicant", False)
						data.ApplicantID = SafeGetInteger(reader, "ApplicantID", 0)
						data.ApplicantLifecycle = SafeGetInteger(reader, "ApplicantLifecycle", 0)
						data.CVLProfileID = SafeGetInteger(reader, "CVLProfileID", 0)
						data.ForeignCategory = SafeGetString(reader, "ForeignCategory")
						data.ZEMISNumber = SafeGetString(reader, "ZEMISNumber")
						data.EmploymentType = SafeGetString(reader, "EmploymentType")
						data.OtherEmploymentType = SafeGetString(reader, "OtherEmploymentType")
						data.TypeOfStay = SafeGetString(reader, "TypeOfStay")

						data.EmployeePartnerRecID = SafeGetInteger(reader, "EmployeePartnerRecID", 0)


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

		Function LoadAllEmployeeCountryCodeData() As IEnumerable(Of EmployeeMasterData) Implements IListingDatabaseAccess.LoadAllEmployeeCountryCodeData

			Dim result As List(Of EmployeeMasterData) = Nothing

			Dim sql As String

			sql = "SELECT "
			sql &= "[Land] "
			sql &= "FROM Mitarbeiter MA "
			sql &= "WHERE ISNULL(land, '') <> '' "
			sql &= "GROUP BY Land "
			sql &= "ORDER BY Land"


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeMasterData)

					While reader.Read

						Dim data = New EmployeeMasterData

						data.Country = SafeGetString(reader, "Land")


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

		Function LoadAllEmployeeCommunityCodeData() As IEnumerable(Of EmployeeMasterData) Implements IListingDatabaseAccess.LoadAllEmployeeCommunityCodeData

			Dim result As List(Of EmployeeMasterData) = Nothing

			Dim sql As String

			sql = "SELECT "
			'sql &= "MAP.TaxCanton "
			sql &= "MAP.TaxCommunityCode "
			sql &= "FROM dbo.tbl_Changed_Employee_PayrollData MAP "
			sql &= "WHERE "
			sql &= "ISNULL(MAP.TaxCommunityLabel, '') = '' "
			sql &= "And ISNULL(MAP.TaxCanton, '') <> '' "
			sql &= "AND MAP.TaxCode <> '0' "
			sql &= "AND IsNull(MAP.TaxCommunityCode, 0) <> 0 "
			sql &= "GROUP BY "
			'sql &= "MAP.TaxCanton "
			sql &= "MAP.TaxCommunityCode "
			sql &= "ORDER BY " 'MAP.TaxCanton"
			sql &= "MAP.TaxCommunityCode"


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeMasterData)

					While reader.Read

						Dim data = New EmployeeMasterData

						'data.S_Canton = SafeGetString(reader, "TaxCanton")
						data.TaxCommunityCode = SafeGetInteger(reader, "TaxCommunityCode", 0)


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

		Function UpdateEmployeeCommunityData(ByVal employeeData As EmployeeMasterData) As Boolean Implements IListingDatabaseAccess.UpdateEmployeeCommunityData

			Dim success = True

			Dim sql As String

			sql = "Update dbo.tbl_Changed_Employee_PayrollData Set "
			sql &= "[TaxCommunityLabel] = @TaxCommunityLabel "
			sql &= ",TaxCanton = @TaxCanton "

			sql &= "WHERE TaxCommunityCode = @TaxCommunityCode "
			sql &= "AND IsNull(TaxCommunityLabel, '') = ''; "
			'sql &= "AND TaxCanton = @TaxCanton; "


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("TaxCommunityCode", ReplaceMissing(employeeData.TaxCommunityCode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("TaxCommunityLabel", ReplaceMissing(employeeData.TaxCommunityLabel, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("TaxCanton", ReplaceMissing(employeeData.S_Canton, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams)


			Return success

		End Function

		Function LoadAllUnDefinedEmployeeCommunityLabelData() As IEnumerable(Of EmployeeMasterData) Implements IListingDatabaseAccess.LoadAllUnDefinedEmployeeCommunityLabelData

			Dim result As List(Of EmployeeMasterData) = Nothing

			Dim sql As String

			sql = "SELECT "
			'sql &= "MAP.TaxCanton "
			sql &= "MAP.TaxCommunityLabel "
			sql &= "FROM dbo.tbl_Changed_Employee_PayrollData MAP "
			sql &= "WHERE "
			sql &= "MAP.TaxCode <> '0' "
			sql &= "And ISNULL(MAP.TaxCanton, '') <> '' "
			sql &= "AND ISNULL(MAP.TaxCommunityLabel, '') <> '' "
			sql &= "AND IsNull(MAP.TaxCommunityCode, 0) = 0 "
			sql &= "GROUP BY "
			'sql &= "MAP.TaxCanton "
			sql &= "MAP.TaxCommunityLabel "
			sql &= "ORDER BY " 'MAP.TaxCanton"
			sql &= "MAP.TaxCommunityLabel"


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeMasterData)

					While reader.Read

						Dim data = New EmployeeMasterData

						'data.S_Canton = SafeGetString(reader, "TaxCanton")
						data.TaxCommunityLabel = SafeGetString(reader, "TaxCommunityLabel")


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

		Function UpdateUnDefinedEmployeeCommunityData(ByVal employeeData As EmployeeMasterData) As Boolean Implements IListingDatabaseAccess.UpdateUnDefinedEmployeeCommunityData

			Dim success = True

			Dim sql As String

			sql = "Update dbo.tbl_Changed_Employee_PayrollData Set "
			sql &= "TaxCommunityCode = @TaxCommunityCode "
			sql &= ",TaxCanton = @TaxCanton "

			sql &= "WHERE "
			sql &= "[TaxCommunityLabel] = @TaxCommunityLabel "
			sql &= "And IsNull(TaxCommunityCode, 0) = 0; "
			'sql &= "And TaxCanton = @TaxCanton; "


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("TaxCommunityCode", ReplaceMissing(employeeData.TaxCommunityCode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("TaxCommunityLabel", ReplaceMissing(employeeData.TaxCommunityLabel, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("TaxCanton", ReplaceMissing(employeeData.S_Canton, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams)


			Return success

		End Function


		Function LoadAllEmployeeNationalityCodeData() As IEnumerable(Of EmployeeMasterData) Implements IListingDatabaseAccess.LoadAllEmployeeNationalityCodeData

			Dim result As List(Of EmployeeMasterData) = Nothing

			Dim sql As String

			sql = "SELECT "
			sql &= "[Nationality] "

			sql &= "FROM Mitarbeiter MA "
			sql &= "WHERE "
			sql &= "ISNULL(Nationality, '') <> '' "
			sql &= "GROUP BY Nationality "
			sql &= "ORDER BY Nationality"


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeMasterData)

					While reader.Read

						Dim data = New EmployeeMasterData

						data.Nationality = SafeGetString(reader, "Nationality")


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

		Function UpdateEmployeeCountryData(ByVal oldCountryCode As String, ByVal newCountryCode As String) As Boolean Implements IListingDatabaseAccess.UpdateEmployeeCountryData

			Dim success = True

			Dim sql As String

			sql = "Update [dbo].[Mitarbeiter] Set "
			sql &= "[Land] = @newCode "

			sql &= " WHERE Land = @oldCode; "

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("oldCode", ReplaceMissing(oldCountryCode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("newCode", ReplaceMissing(newCountryCode, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)


			Return success

		End Function

		Function UpdateEmployeeNationalityData(ByVal oldCountryCode As String, ByVal newCountryCode As String) As Boolean Implements IListingDatabaseAccess.UpdateEmployeeNationalityData

			Dim success = True

			Dim sql As String

			sql = "Update [dbo].[Mitarbeiter] Set "
			sql &= "[Nationality] = @newCode "

			sql &= " WHERE Nationality = @oldCode; "

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("oldCode", ReplaceMissing(oldCountryCode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("newCode", ReplaceMissing(newCountryCode, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)


			Return success

		End Function





	End Class

End Namespace
