Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects
Imports SP.DatabaseAccess.Employee.DataObjects.Salary
Imports System.Text

Namespace Employee

	Partial Public Class EmployeeDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IEmployeeDatabaseAccess

		''' <summary>
		''' Loads employee master data (Mitarbeiter).
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <param name="includeImageData">Optional flag indicating if image data should also be loaded.</param>
		''' <returns>Employee master data or nothing in error case.</returns>
		Function LoadEmployeeMasterData(ByVal employeeNumber As Integer, Optional includeImageData As Boolean = False) As EmployeeMasterData Implements IEmployeeDatabaseAccess.LoadEmployeeMasterData

			Dim employeeMasterData As EmployeeMasterData = Nothing

			Dim sql As String

			If includeImageData Then
				sql = "[Load Assigned Employee Master Data]"
			Else
				sql = "[Load Assigned Employee Master Data Without Picture]"
			End If

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", 0))
			listOfParams.Add(New SqlClient.SqlParameter("employeeNumber", ReplaceMissing(employeeNumber, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If Not reader Is Nothing Then

					If reader.Read Then
						employeeMasterData = New EmployeeMasterData

						employeeMasterData.ID = SafeGetInteger(reader, "ID", 0)
						employeeMasterData.EmployeeNumber = SafeGetInteger(reader, "MANr", 0)
						employeeMasterData.Lastname = SafeGetString(reader, "Nachname")
						employeeMasterData.Firstname = SafeGetString(reader, "Vorname")
						employeeMasterData.PostOfficeBox = SafeGetString(reader, "Postfach")
						employeeMasterData.Latitude = SafeGetDouble(reader, "Latitude", 0)
						employeeMasterData.Longitude = SafeGetDouble(reader, "Longitude", 0)

						employeeMasterData.Street = SafeGetString(reader, "Strasse")
						employeeMasterData.Postcode = SafeGetString(reader, "PLZ")
						employeeMasterData.Location = SafeGetString(reader, "Ort")
						employeeMasterData.Country = SafeGetString(reader, "Land")
						employeeMasterData.Language = SafeGetString(reader, "Sprache")
						employeeMasterData.Birthdate = SafeGetDateTime(reader, "GebDat", Nothing)
						employeeMasterData.Gender = SafeGetString(reader, "Geschlecht")
						employeeMasterData.AHV_Nr = SafeGetString(reader, "AHV_Nr")
						employeeMasterData.Nationality = SafeGetString(reader, "Nationality")
						employeeMasterData.CivilStatus = SafeGetString(reader, "Zivilstand")
						employeeMasterData.Telephone_P = SafeGetString(reader, "Telefon_P")
						employeeMasterData.Telephone2 = SafeGetString(reader, "Telefon2")
						employeeMasterData.Telephone3 = SafeGetString(reader, "Telefon3")
						employeeMasterData.Telephone_G = SafeGetString(reader, "Telefon_G")
						employeeMasterData.MobilePhone = SafeGetString(reader, "Natel")
						employeeMasterData.Homepage = SafeGetString(reader, "Homepage")
						employeeMasterData.Email = SafeGetString(reader, "eMail")
						employeeMasterData.Permission = SafeGetString(reader, "Bewillig")
						employeeMasterData.PermissionToDate = SafeGetDateTime(reader, "Bew_Bis", Nothing)
						employeeMasterData.BirthPlace = SafeGetString(reader, "GebOrt")
						employeeMasterData.CHPartner = SafeGetBoolean(reader, "CHPartner", False)
						employeeMasterData.ValidatePermissionWithTax = SafeGetBoolean(reader, "ValidatePermissionWithTax", False)
						employeeMasterData.NoSpecialTax = SafeGetBoolean(reader, "NoSpecialTax", False)
						employeeMasterData.Q_Steuer = SafeGetString(reader, "Q_Steuer")
						employeeMasterData.S_Canton = SafeGetString(reader, "S_Kanton")
						employeeMasterData.ChurchTax = SafeGetString(reader, "Kirchensteuer")
						employeeMasterData.Residence = SafeGetBoolean(reader, "Ansaessigkeit", Nothing)
						employeeMasterData.ChildsCount = SafeGetShort(reader, "Kinder", Nothing)
						employeeMasterData.Profession = SafeGetString(reader, "Beruf")
						employeeMasterData.StaysAt = SafeGetString(reader, "Wohnt_bei")

						employeeMasterData.V_Hint = SafeGetString(reader, "V_Hinweis")
						employeeMasterData.Notice_Employment = SafeGetString(reader, "Notice_Employment")
						employeeMasterData.Notice_Report = SafeGetString(reader, "Notice_Report")
						employeeMasterData.Notice_AdvancedPayment = SafeGetString(reader, "Notice_AdvancedPayment")
						employeeMasterData.Notice_Payroll = SafeGetString(reader, "Notice_Payroll")

						employeeMasterData.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						employeeMasterData.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
						employeeMasterData.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						employeeMasterData.ChangedFrom = SafeGetString(reader, "ChangedFrom")
						employeeMasterData.HasImage = SafeGetBoolean(reader, "Bild", False)
						If (includeImageData) Then
							employeeMasterData.MABild = SafeGetByteArray(reader, "MABild")
						End If
						employeeMasterData.Result = SafeGetString(reader, "Result")
						employeeMasterData.KST = SafeGetString(reader, "KST")
						employeeMasterData.FirstContact = SafeGetDateTime(reader, "ErstKontakt", Nothing)
						employeeMasterData.LastContact = SafeGetDateTime(reader, "LetztKontakt", Nothing)
						employeeMasterData.QSTCommunity = SafeGetString(reader, "QSTGemeinde")

						employeeMasterData.TaxCommunityLabel = SafeGetString(reader, "TaxCommunityLabel")
						employeeMasterData.TaxCommunityCode = SafeGetInteger(reader, "TaxCommunityCode", Nothing)

						employeeMasterData.BusinessBranch = SafeGetString(reader, "Filiale")
						employeeMasterData.GAVBez = SafeGetString(reader, "GAVBez")
						employeeMasterData.CivilState2 = SafeGetString(reader, "Zivilstand2")
						employeeMasterData.QLand = SafeGetString(reader, "QLand")
						employeeMasterData.MABusinessBranch = SafeGetString(reader, "MAFiliale")
						employeeMasterData.AHV_Nr_New = SafeGetString(reader, "AHV_Nr_New")
						employeeMasterData.MA_Canton = SafeGetString(reader, "MA_Kanton")
						employeeMasterData.ANS_OST_Bis = SafeGetDateTime(reader, "Ans_QST_Bis", Nothing)
						employeeMasterData.Transfered_Guid = SafeGetString(reader, "Transfered_Guid")
						employeeMasterData.Transfered_On = SafeGetDateTime(reader, "Transfered_On", Nothing)
						employeeMasterData.Send2WOS = SafeGetBoolean(reader, "Send2WOS", Nothing)
						employeeMasterData.SendDataWithEMail = SafeGetBoolean(reader, "SendDataWithEMail", Nothing)
						employeeMasterData.MA_SMS_Mailing = SafeGetBoolean(reader, "MA_SMS_Mailing", Nothing)
						employeeMasterData.MA_EMail_Mailing = SafeGetBoolean(reader, "MA_EMail_Mailing", Nothing)
						employeeMasterData.ProfessionCode = SafeGetInteger(reader, "BerufCode", Nothing)
						employeeMasterData.WOSGuid = SafeGetString(reader, "Transfered_Guid")
						employeeMasterData.MDNr = SafeGetInteger(reader, "MDnr", Nothing)
						employeeMasterData.Facebook = SafeGetString(reader, "facebook")
						employeeMasterData.LinkedIn = SafeGetString(reader, "LinkedIn")
						employeeMasterData.Xing = SafeGetString(reader, "xing")
						employeeMasterData.MobilePhone2 = SafeGetString(reader, "Natel2")
						employeeMasterData.ShowAsApplicant = SafeGetBoolean(reader, "ShowAsApplicant", False)
						employeeMasterData.ApplicantID = SafeGetInteger(reader, "ApplicantID", 0)
						employeeMasterData.ApplicantLifecycle = SafeGetInteger(reader, "ApplicantLifecycle", 0)
						employeeMasterData.CVLProfileID = SafeGetInteger(reader, "CVLProfileID", 0)
						employeeMasterData.ForeignCategory = SafeGetString(reader, "ForeignCategory")
						employeeMasterData.ZEMISNumber = SafeGetString(reader, "ZEMISNumber")
						employeeMasterData.EmploymentType = SafeGetString(reader, "EmploymentType")
						employeeMasterData.OtherEmploymentType = SafeGetString(reader, "OtherEmploymentType")
						employeeMasterData.TypeOfStay = SafeGetString(reader, "TypeOfStay")
						employeeMasterData.EmployeePartnerRecID = SafeGetInteger(reader, "EmployeePartnerRecID", 0)
						employeeMasterData.EmployeeLOHistoryRecID = SafeGetInteger(reader, "EmployeeLOHistoryRecID", 0)
						employeeMasterData.ExistsHistoryData = SafeGetBoolean(reader, "ExistsHistoryData", False)
						employeeMasterData.ExistsOldBackupData = SafeGetBoolean(reader, "ExistsOldBackupData", Nothing)

						employeeMasterData.CreatedUserNumber = SafeGetInteger(reader, "CreatedUserNumber", 0)
						employeeMasterData.ChangedUserNumber = SafeGetInteger(reader, "ChangedUserNumber", 0)

					End If

				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				employeeMasterData = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return employeeMasterData

			Return Nothing
		End Function

		''' <summary>
		''' Loads existing employees data by search criteria.
		''' </summary>
		''' <param name="lastname">The lastname.</param>
		''' <param name="firstname">The firstname.</param>
		''' <param name="street">The street.</param>
		''' <param name="postcode">The postcode.</param>
		''' <param name="location">The location.</param>
		''' <param name="countryCode">The country code.</param>
		''' <returns>List of existing customer data.</returns>
		Function LoadExistingEmployeesBySearchCriteria(ByVal lastname As String, ByVal firstname As String, ByVal street As String, ByVal postcode As String, ByVal location As String, ByVal countryCode As String) As IEnumerable(Of ExistingEmployeeSearchData) Implements IEmployeeDatabaseAccess.LoadExistingEmployeesBySearchCriteria

			Dim result As List(Of ExistingEmployeeSearchData) = Nothing

			Dim sql As String

			sql = "[Get Search Existing Employee For New Employee]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("Nachname", ReplaceMissing(lastname, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("Vorname", ReplaceMissing(firstname, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("Strasse", ReplaceMissing(street, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("PLZ", ReplaceMissing(postcode, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("Ort", ReplaceMissing(location, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("Land", ReplaceMissing(countryCode, String.Empty)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ExistingEmployeeSearchData)

					While reader.Read()
						Dim searchData As New ExistingEmployeeSearchData

						searchData.MDNr = SafeGetInteger(reader, "mdnr", 0)
						searchData.EmployeeNumber = SafeGetInteger(reader, "MANr", 0)
						searchData.Lastname = SafeGetString(reader, "Nachname")
						searchData.Firstname = SafeGetString(reader, "Vorname")
						searchData.Street = SafeGetString(reader, "Strasse")
						searchData.Postcode = SafeGetString(reader, "PLZ")
						searchData.Location = SafeGetString(reader, "Ort")
						searchData.CountryCode = SafeGetString(reader, "Land")
						searchData.employeeKST = SafeGetString(reader, "KST")
						searchData.employeeAdvisor = SafeGetString(reader, "Advisor")

						result.Add(searchData)

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

		Function LoadAssignedEmployeesBySearchCriteria(ByVal mySearchData As ExistingEmployeeSearchData) As IEnumerable(Of ExistingEmployeeSearchData) Implements IEmployeeDatabaseAccess.LoadAssignedEmployeesBySearchCriteria

			Dim result As List(Of ExistingEmployeeSearchData) = Nothing

			Dim sql As String

			sql = "[Load Existing Employees By Search Criteria For Notification]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("Nachname", ReplaceMissing(mySearchData.Lastname, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("Vorname", ReplaceMissing(mySearchData.Firstname, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("Strasse", ReplaceMissing(mySearchData.Street, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("PLZ", ReplaceMissing(mySearchData.Postcode, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("Ort", ReplaceMissing(mySearchData.Location, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("Land", ReplaceMissing(mySearchData.CountryCode, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("eMail", ReplaceMissing(mySearchData.Email, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("GebDat", ReplaceMissing(mySearchData.Birthdate, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ExistingEmployeeSearchData)

					While reader.Read()
						Dim searchData As New ExistingEmployeeSearchData

						searchData.MDNr = SafeGetInteger(reader, "MDNr", 0)
						searchData.EmployeeNumber = SafeGetInteger(reader, "MANr", 0)
						searchData.Lastname = SafeGetString(reader, "Nachname")
						searchData.Firstname = SafeGetString(reader, "Vorname")
						searchData.Street = SafeGetString(reader, "Strasse")
						searchData.Postcode = SafeGetString(reader, "PLZ")
						searchData.Location = SafeGetString(reader, "Ort")
						searchData.CountryCode = SafeGetString(reader, "Land")
						searchData.employeeKST = SafeGetString(reader, "KST")
						searchData.employeeAdvisor = SafeGetString(reader, "Advisor")
						searchData.ShowAsApplicant = SafeGetBoolean(reader, "ShowAsApplicant", False)
						searchData.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						searchData.CreatedFrom = SafeGetString(reader, "CreatedFrom")


						result.Add(searchData)

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


		''' <summary>
		''' Loads the employee QST (Quellensteuer) communities (Gemeinden).
		''' </summary>
		''' <returns>List of QST communities.</returns>
		Function LoadEmployeeBeforeEQuestBackup(ByVal employeeNumber As Integer?) As IEnumerable(Of EmployeeBackupBeforeEQuestData) Implements IEmployeeDatabaseAccess.LoadEmployeeBeforeEQuestBackup

			Dim result As List(Of EmployeeBackupBeforeEQuestData) = Nothing

			Dim sql As String

			sql = "SELECT MANR"
			sql &= ",BEWILLIG"
			sql &= ",CHPartner"
			sql &= ",NoSpecialTax"
			sql &= ",S_KANTON"
			sql &= ",QSTGemeinde"
			sql &= ",Q_STEUER"
			sql &= ",Kirchensteuer"
			sql &= ",Kinder"
			sql &= ",Ansaessigkeit"
			sql &= ",Ans_QST_Bis "
			sql &= ",CheckedOn "
			sql &= ",dbo.[uf_Get Advisor First And Lastname](CheckedUserNumber) CheckedFrom "
			sql &= ",CheckedUserNumber "
			sql &= "From dbo.tbl_TempEmployee_before_EQuest "
			sql &= "WHERE MANr = @EmployeeNumber OR (@EmployeeNumber = 0) "
			sql &= "Order By MANr"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("EmployeeNumber", ReplaceMissing(employeeNumber, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeBackupBeforeEQuestData)

					While reader.Read()
						Dim interviewData As New EmployeeBackupBeforeEQuestData

						interviewData.EmployeeNumber = SafeGetInteger(reader, "MANR", 0)
						interviewData.Permission = SafeGetString(reader, "BEWILLIG")
						interviewData.CHPartner = SafeGetBoolean(reader, "CHPartner", False)
						interviewData.NoSpecialTax = SafeGetBoolean(reader, "NoSpecialTax", False)
						interviewData.S_Canton = SafeGetString(reader, "S_KANTON")
						interviewData.QSTCommunity = SafeGetString(reader, "QSTGemeinde")
						interviewData.Q_Steuer = SafeGetString(reader, "Q_STEUER")
						interviewData.ChurchTax = SafeGetString(reader, "Kirchensteuer")
						interviewData.ChildsCount = SafeGetInteger(reader, "Kinder", 0)
						interviewData.Residence = SafeGetBoolean(reader, "Ansaessigkeit", False)
						interviewData.ANS_OST_Bis = SafeGetDateTime(reader, "Ans_QST_Bis", Nothing)
						interviewData.CheckedOn = SafeGetDateTime(reader, "CheckedOn", Nothing)
						interviewData.CheckedFrom = SafeGetString(reader, "CheckedFrom")
						interviewData.CheckedUserNumber = SafeGetInteger(reader, "CheckedUserNumber", Nothing)


						result.Add(interviewData)

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

		Function UpdateEmployeeBeforeEQuestBackup(ByVal employeeNumber As Integer?, ByVal checkedUserNumber As Integer) As Boolean Implements IEmployeeDatabaseAccess.UpdateEmployeeBeforeEQuestBackup

			Dim success As Boolean = True

			Dim sql As String

			sql = "Update dbo.tbl_TempEmployee_before_EQuest "
			sql &= "Set "
			sql &= "CheckedOn = GetDate()"
			sql &= ",CheckedUserNumber = @UserNumber "
			sql &= ",CheckedFrom = dbo.[uf_Get Advisor First And Lastname](@UserNumber) "
			sql &= "WHERE MANr = @EmployeeNumber "

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("EmployeeNumber", ReplaceMissing(employeeNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("UserNumber", ReplaceMissing(checkedUserNumber, 0)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

			Return success
		End Function

		''' <summary>
		''' Loads the employee communities (Gemeinden) for selected canton.
		''' </summary>
		''' <returns>List of QST communities.</returns>
		Public Function LoadEmployeeQSTCommunitiesWithCanton(ByVal canton As String) As IEnumerable(Of EmployeeQSTCommunityData) Implements IEmployeeDatabaseAccess.LoadEmployeeQSTCommunitiesWithCanton

			Dim result As List(Of EmployeeQSTCommunityData) = Nothing

			Dim sql As String

			sql = "IF OBJECT_ID('tbl_Gemeinde', 'U') IS NOT NULL "

			sql &= "begin "
			sql &= "Select (Convert(nvarchar(10), [BFS-Gemeindenummer]) + ' ' + Gemeindename) As QSTGemeinde From tbl_Gemeinde "
			If Not canton Is Nothing AndAlso canton <> String.Empty Then sql &= String.Format("Where kanton = '{0}' ", canton)
			sql &= "Order By Gemeindename Asc "
			sql &= "End "

			sql &= "ELSE "

			sql &= "begin "
			sql &= "SELECT QSTGemeinde, S_Kanton FROM Mitarbeiter WHERE NOT (QSTGemeinde IS NULL Or QSTGemeinde = '')  "
			If Not canton Is Nothing AndAlso canton <> String.Empty Then sql &= String.Format("And S_Kanton = '{0}' ", canton)
			sql &= "GROUP BY QSTGemeinde, S_Kanton "
			sql &= "Order By QSTGemeinde Asc "
			sql &= "End "

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeQSTCommunityData)

					While reader.Read()
						Dim interviewData As New EmployeeQSTCommunityData
						interviewData.CommunityName = SafeGetString(reader, "QSTGemeinde")

						result.Add(interviewData)

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


		''' <summary>
		''' Loads employee state data1 (TAB_MAStat).
		''' </summary>
		''' <returns>List of employee state data (1).</returns>
		Public Function LoadEmployeeStateData1() As IEnumerable(Of EmployeeStateData) Implements IEmployeeDatabaseAccess.LoadEmployeeStateData1

			Dim result As List(Of EmployeeStateData) = Nothing

			Dim sql As String

			sql = String.Format("SELECT ID, GetFeld, Description, {0} as TranslatedText FROM TAB_MAStat ORDER BY {0} ASC", MapLanguageToColumnName(SelectedTranslationLanguage))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeStateData)

					While reader.Read()
						Dim employeeStateData As New EmployeeStateData
						employeeStateData.ID = SafeGetInteger(reader, "ID", 0)
						employeeStateData.GetField = SafeGetString(reader, "GetFeld")
						employeeStateData.Description = SafeGetString(reader, "Description")
						employeeStateData.TranslatedState = SafeGetString(reader, "TranslatedText")


						result.Add(employeeStateData)

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

		''' <summary>
		''' Loads employee state data2 (TAB_MAStat2).
		''' </summary>
		''' <returns>List of employee state data (2).</returns>
		Public Function LoadEmployeeStateData2() As IEnumerable(Of EmployeeStateData) Implements IEmployeeDatabaseAccess.LoadEmployeeStateData2

			Dim result As List(Of EmployeeStateData) = Nothing

			Dim sql As String

			sql = String.Format("SELECT ID, Bezeichnung, Result, {0} as TranslatedText FROM TAB_MAStat2 ORDER BY {0} ASC", MapLanguageToColumnName(SelectedTranslationLanguage))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeStateData)

					While reader.Read()
						Dim employeeStateData As New EmployeeStateData
						employeeStateData.ID = SafeGetInteger(reader, "ID", 0)
						employeeStateData.Description = SafeGetString(reader, "Bezeichnung")
						employeeStateData.Result = SafeGetString(reader, "Result")
						employeeStateData.TranslatedState = SafeGetString(reader, "TranslatedText")

						result.Add(employeeStateData)

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

		''' <summary>
		''' Loads business branch data of an employee (MA_Filiale)
		''' </summary>
		''' <returns>List of business branches data.</returns>
		Public Function LoadEmployeeBusinessBranches(ByVal employeeNumber As Integer) As IEnumerable(Of EmployeeBusinessBranchData) Implements IEmployeeDatabaseAccess.LoadEmployeeBusinessBranches

			Dim result As List(Of EmployeeBusinessBranchData) = Nothing

			Dim sql As String

			sql = "SELECT ID, MANr, Bezeichnung, MDNr FROM MA_Filiale WHERE MANr = @employeeNumber Order By Bezeichnung ASC"

			' Parameters
			Dim employeeNumberParameter As New SqlClient.SqlParameter("employeeNumber", employeeNumber)
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(employeeNumberParameter)

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeBusinessBranchData)

					While reader.Read()
						Dim customerBusinessBranchData As New EmployeeBusinessBranchData
						customerBusinessBranchData.ID = SafeGetInteger(reader, "ID", 0)
						customerBusinessBranchData.EmployeeNumber = SafeGetInteger(reader, "MANr", 0)
						customerBusinessBranchData.Description = SafeGetString(reader, "Bezeichnung")
						customerBusinessBranchData.MDNr = SafeGetInteger(reader, "MDNr", 0)
						result.Add(customerBusinessBranchData)

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

		''' <summary>
		''' Loads employee contact info data (TAB_MAKontakt).
		''' </summary>
		''' <returns>List of employee contact info data.</returns>
		Public Function LoadEmployeeContactsInfo() As IEnumerable(Of EmployeeContactInfoData) Implements IEmployeeDatabaseAccess.LoadEmployeeContactsInfo

			Dim result As List(Of EmployeeContactInfoData) = Nothing

			Dim sql As String
			sql = String.Format("SELECT ID, GetFeld, Description, {0} as TranslatedText FROM TAB_MAKontakt ORDER BY {0} ASC", MapLanguageToColumnName(SelectedTranslationLanguage))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeContactInfoData)

					While reader.Read()
						Dim contactInfoData As New EmployeeContactInfoData
						contactInfoData.ID = SafeGetInteger(reader, "ID", 0)
						contactInfoData.GetFeld = SafeGetString(reader, "GetFeld")
						contactInfoData.Description = SafeGetString(reader, "Description")
						contactInfoData.TranslatedContactInfoText = SafeGetString(reader, "TranslatedText")

						result.Add(contactInfoData)

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

		''' <summary>
		''' Loads employee contact comm data(MAKontakt_Komm).
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <returns>List ofemployee contact comm data.</returns>
		Public Function LoadEmployeeContactCommData(ByVal employeeNumber As Integer) As EmployeeContactComm Implements IEmployeeDatabaseAccess.LoadEmployeeContactCommData

			Dim result As EmployeeContactComm = Nothing

			Dim sql As String
			sql = "SELECT * FROM MAKontakt_Komm WHERE MANr = @employeeNumber ORDER BY ID ASC"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("@employeeNumber", ReplaceMissing(employeeNumber, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If Not reader Is Nothing Then

					If reader.Read Then
						result = New EmployeeContactComm
						result.ID = SafeGetInteger(reader, "ID", 0)
						result.EmployeeNumber = SafeGetInteger(reader, "MANr", 0)
						result.AnredeForm = SafeGetString(reader, "AnredeForm")
						result.BriefAnrede = SafeGetString(reader, "BriefAnrede")
						result.KontaktHow = SafeGetString(reader, "KontaktHow")
						result.KStat1 = SafeGetString(reader, "KStat1")
						result.KStat2 = SafeGetString(reader, "KStat2")
						result.WebExport = SafeGetBoolean(reader, "WebExport", Nothing)
						result.ESAb = SafeGetDateTime(reader, "ESAb", Nothing)
						result.ESEnde = SafeGetDateTime(reader, "ESEnde", Nothing)
						result.Absenzen = SafeGetString(reader, "Absenzen")
						result.NoWorkAS = SafeGetString(reader, "NoWorkAS")
						result.InLandSeit = SafeGetString(reader, "InLandSeit")
						result.GetAHVKarte = SafeGetBoolean(reader, "GetAHVKarte", Nothing)
						result.GetAHVKarteBez = SafeGetString(reader, "GetAHVKarteBez")
						result.AHVKarteBacked = SafeGetBoolean(reader, "AHVKarteBacked", Nothing)
						result.AHVKateBackedBez = SafeGetString(reader, "AHVKarteBackedBez")
						result.InZV = SafeGetBoolean(reader, "InZV", Nothing)
						result.InZVBez = SafeGetString(reader, "InZVBez")
						result.RahmenArbeit = SafeGetBoolean(reader, "RahmenArbeit", Nothing)
						result.RahemArbeitBez = SafeGetString(reader, "RahmenArbeitBez")
						result.Res1 = SafeGetString(reader, "Res1")
						result.Res2 = SafeGetString(reader, "Res2")
						result.Res3 = SafeGetString(reader, "Res3")
						result.Res4 = SafeGetString(reader, "Res4")
						result.KundFristen = SafeGetString(reader, "KundFristen")
						result.KundGrund = SafeGetString(reader, "KundGrund")
						result.Arbeitspensum = SafeGetString(reader, "Arbeitspensum")
						result.GehaltAlt = SafeGetDecimal(reader, "GehaltAlt", Nothing)
						result.GehaltNeu = SafeGetDecimal(reader, "GehaltNeu", Nothing)
						result.GotDocs = SafeGetBoolean(reader, "GotDocs", Nothing)
						result.Result = SafeGetString(reader, "Result")
						result.GehaltPerMonth = SafeGetDecimal(reader, "GehaltPerMonth", Nothing)
						result.GehaltPerStd = SafeGetDecimal(reader, "GehaltPerStd", Nothing)
						result.DStellen = SafeGetBoolean(reader, "DStellen", Nothing)
						result.NoES = SafeGetBoolean(reader, "NoES", Nothing)
						result.Res5 = SafeGetString(reader, "Res5")
						result.AGB_WOS = SafeGetString(reader, "AGB_WOS")

						result.ZVeMail = SafeGetString(reader, "ZVeMail")
						result.ZVVersand = SafeGetString(reader, "ZVVersand")
						result.ALKNumber = SafeGetInteger(reader, "ALKNumber", Nothing)
						result.ALKName = SafeGetString(reader, "ALKName")
						result.ALKPOBox = SafeGetString(reader, "ALKPOBox")
						result.ALKStreet = SafeGetString(reader, "ALKStreet")
						result.ALKPostcode = SafeGetInteger(reader, "ALKPostcode", Nothing)
						result.ALKLocation = SafeGetString(reader, "ALKLocation")
						result.ALKTelephone = SafeGetString(reader, "ALKTelephone")
						result.ALKTelefax = SafeGetString(reader, "ALKTelefax")

					End If

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)

			End Try

			Return result

		End Function

		''' <summary>
		''' Loads employee other data (MASonstiges)
		''' </summary>
		''' <param name="employeeNumber">The employeenumber.</param>
		''' <returns>List of employee other data.</returns>
		Public Function LoadEmployeeOtherData(ByVal employeeNumber As Integer) As EmployeeOtherData Implements IEmployeeDatabaseAccess.LoadEmployeeOtherData

			Dim result As EmployeeOtherData = Nothing

			Dim sql As String = String.Empty

			sql = sql & "Select [MANr]"
			sql = sql & ",[ArbVol]"
			sql = sql & ",[Mailing]"
			sql = sql & ",[MSprache1]"
			sql = sql & ",[MSprache2]"
			sql = sql & ",[MSprache3]"
			sql = sql & ",[SSprache1]"
			sql = sql & ",[SSprache2]"
			sql = sql & ",[SSprache3]"
			sql = sql & ",[MSprache1Level]"
			sql = sql & ",[MSprache2Level]"
			sql = sql & ",[MSprache3Level]"
			sql = sql & ",[SSprache1Level]"
			sql = sql & ",[SSprache2Level]"
			sql = sql & ",[SSprache3Level]"
			sql = sql & ",[F_Schein1]"
			sql = sql & ",[F_Schein2]"
			sql = sql & ",[F_schein3]"
			sql = sql & ",[Fahrzeug]"
			sql = sql & ",[MSprache4]"
			sql = sql & ",[MSprache5]"
			sql = sql & ",[MSprache6]"
			sql = sql & ",[SSprache4]"
			sql = sql & ",[SSprache5]"
			sql = sql & ",[SSprache6]"
			sql = sql & ",[KundGrund]"
			sql = sql & ",[BVG]"
			sql = sql & ",[AutoReserv]"
			sql = sql & ",[ID]"
			sql = sql & " FROM [MASonstiges]"
			sql = sql & "  WHERE [MANr] = @employeeNumber"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("@employeeNumber", ReplaceMissing(employeeNumber, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If Not reader Is Nothing Then

					If reader.Read Then
						result = New EmployeeOtherData
						result.ID = SafeGetInteger(reader, "ID", 0)
						result.EmployeeNumber = SafeGetInteger(reader, "MANr", 0)
						result.ArbVol = SafeGetDecimal(reader, "ArbVol", Nothing)
						result.Mailing = SafeGetBoolean(reader, "Mailing", Nothing)
						result.MLanguage1 = SafeGetString(reader, "MSprache1")
						result.MLanguage2 = SafeGetString(reader, "MSprache2")
						result.MLanguage3 = SafeGetString(reader, "MSprache3")
						result.SLanguage1 = SafeGetString(reader, "SSprache1")
						result.SLanguage2 = SafeGetString(reader, "SSprache2")
						result.SLanguage3 = SafeGetString(reader, "SSprache3")
						result.MLanguage1Level = SafeGetShort(reader, "MSprache1Level", Nothing)
						result.MLanguage2Level = SafeGetShort(reader, "MSprache2Level", Nothing)
						result.MLanguage3Level = SafeGetShort(reader, "MSprache3Level", Nothing)
						result.SLanguage1Level = SafeGetShort(reader, "SSprache1Level", Nothing)
						result.SLanguage2Level = SafeGetShort(reader, "SSprache2Level", Nothing)
						result.SLanguage3Level = SafeGetShort(reader, "SSprache3Level", Nothing)
						result.DrivingLicence1 = SafeGetString(reader, "F_Schein1")
						result.DrivingLicence2 = SafeGetString(reader, "F_Schein2")
						result.DrivingLicence3 = SafeGetString(reader, "F_Schein3")
						result.Vehicle = SafeGetString(reader, "Fahrzeug")
						result.MLanguage4 = SafeGetString(reader, "MSprache4")
						result.MLanguage5 = SafeGetString(reader, "MSprache5")
						result.MLanguage6 = SafeGetString(reader, "MSprache6")
						result.SLanguage4 = SafeGetString(reader, "SSprache4")
						result.SLanguage5 = SafeGetString(reader, "SSprache5")
						result.SLanguage6 = SafeGetString(reader, "SSprache6")
						result.KundGrund = SafeGetString(reader, "KundGrund")
						result.BVG = SafeGetString(reader, "BVG")
						result.AutoReserv = SafeGetString(reader, "AutoReserv")
					End If

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)

			End Try

			Return result

		End Function

		''' <summary>
		''' Loads employee ES state data.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <returns>Employee ES state data.</returns>
		Public Function LoadEmployeeESStateData(ByVal employeeNumber As Integer) As EmployeeESStateData Implements IEmployeeDatabaseAccess.LoadEmployeeESStateData

			Dim success = True

			Dim sql As String = String.Empty

			sql = "[Get Einsatz State Of Employee]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MANr", employeeNumber))

			Dim resultESNumberParameter = New SqlClient.SqlParameter("@ESNr", SqlDbType.Int)
			resultESNumberParameter.Direction = ParameterDirection.Output
			listOfParams.Add(resultESNumberParameter)

			Dim resultStateParameter = New SqlClient.SqlParameter("@State", SqlDbType.Int)
			resultStateParameter.Direction = ParameterDirection.Output
			listOfParams.Add(resultStateParameter)

			Dim resultLastEsAbParameter = New SqlClient.SqlParameter("@Last_Es_Ab", SqlDbType.DateTime)
			resultLastEsAbParameter.Direction = ParameterDirection.Output
			listOfParams.Add(resultLastEsAbParameter)

			Dim resultLastEsEndeParameter = New SqlClient.SqlParameter("@Last_Es_Ende", SqlDbType.DateTime)
			resultLastEsEndeParameter.Direction = ParameterDirection.Output
			listOfParams.Add(resultLastEsEndeParameter)

			Dim resultLastEsAls = New SqlClient.SqlParameter("@Last_Es_Als", SqlDbType.NVarChar, 1000)
			resultLastEsAls.Direction = ParameterDirection.Output
			listOfParams.Add(resultLastEsAls)

			Dim customerParameter = New SqlClient.SqlParameter("@Customer", SqlDbType.NVarChar, 70)
			customerParameter.Direction = ParameterDirection.Output
			listOfParams.Add(customerParameter)


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Dim result As EmployeeESStateData
			Try

				result = New EmployeeESStateData

				result.ESNumber = resultESNumberParameter.Value
				result.State = resultStateParameter.Value
				result.Last_Es_Ab = If(IsDBNull(resultLastEsAbParameter.Value), Nothing, resultLastEsAbParameter.Value)
				result.Last_Es_Ende = If(IsDBNull(resultLastEsEndeParameter.Value), Nothing, resultLastEsEndeParameter.Value)
				result.Last_Es_Als = If(IsDBNull(resultLastEsAls.Value), Nothing, resultLastEsAls.Value)
				result.Customer = If(IsDBNull(customerParameter.Value), Nothing, customerParameter.Value)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				result = Nothing
			End Try

			Return result

		End Function

		''' <summary>
		''' Loads employee propose state data.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <returns>Employee propose state data.</returns>
		Public Function LoadEmployeeProposeStateData(ByVal employeeNumber As Integer) As EmployeeProposeStateData Implements IEmployeeDatabaseAccess.LoadEmployeeProposeStateData

			Dim success = True

			Dim sql As String = String.Empty

			sql = "[Get Propose State Of Employee]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MANr", employeeNumber))

			Dim resultProposeNumberParameter = New SqlClient.SqlParameter("@ProposeNr", SqlDbType.Int)
			resultProposeNumberParameter.Direction = ParameterDirection.Output
			listOfParams.Add(resultProposeNumberParameter)

			Dim resultStateParameter = New SqlClient.SqlParameter("@State", SqlDbType.Int)
			resultStateParameter.Direction = ParameterDirection.Output
			listOfParams.Add(resultStateParameter)

			Dim proposeCreatedOnParameter = New SqlClient.SqlParameter("@ProposeCreatedOn", SqlDbType.DateTime)
			proposeCreatedOnParameter.Direction = ParameterDirection.Output
			listOfParams.Add(proposeCreatedOnParameter)

			Dim proposeDescriptionParameter = New SqlClient.SqlParameter("@ProposeDescription", SqlDbType.NVarChar, 255)
			proposeDescriptionParameter.Direction = ParameterDirection.Output
			listOfParams.Add(proposeDescriptionParameter)

			Dim proposeCustomerParameter = New SqlClient.SqlParameter("@ProposeCustomer", SqlDbType.NVarChar, 70)
			proposeCustomerParameter.Direction = ParameterDirection.Output
			listOfParams.Add(proposeCustomerParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Dim result As EmployeeProposeStateData
			Try

				result = New EmployeeProposeStateData

				result.ProposeNumber = resultProposeNumberParameter.Value
				result.State = resultStateParameter.Value
				result.ProposeCreatedOn = If(IsDBNull(proposeCreatedOnParameter.Value), Nothing, proposeCreatedOnParameter.Value)
				result.ProposeDescription = If(IsDBNull(proposeDescriptionParameter.Value), Nothing, proposeDescriptionParameter.Value)
				result.Customer = If(IsDBNull(proposeCustomerParameter.Value), Nothing, proposeCustomerParameter.Value)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				result = Nothing
			End Try

			Return result

		End Function

		''' <summary>
		''' Loads QST (Quellenstensteuer) data.
		''' </summary>
		''' <returns>List of QST data.</returns>
		Public Function LoadQSTData() As IEnumerable(Of QSTData) Implements IEmployeeDatabaseAccess.LoadQSTData

			Dim result As List(Of QSTData) = Nothing

			Dim sql As String
			sql = String.Format("SELECT ID, GetFeld, Description, {0} as TranslatedText FROM Tab_Quell ORDER BY {0} ", MapLanguageToColumnName(SelectedTranslationLanguage))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of QSTData)

					While reader.Read()
						Dim contactInfoData As New QSTData
						contactInfoData.ID = SafeGetShort(reader, "ID", 0)
						contactInfoData.GetField = SafeGetString(reader, "GetFeld")
						contactInfoData.Description = SafeGetString(reader, "Description")
						contactInfoData.TranslatedQSTCodeText = SafeGetString(reader, "TranslatedText")

						result.Add(contactInfoData)

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

		''' <summary>
		''' Loads church tax code data.
		''' </summary>
		''' <returns>List of church tax code data.</returns>
		Function LoadChurchTaxCodeData() As IEnumerable(Of ChurchTaxCodeData) Implements IEmployeeDatabaseAccess.LoadChurchTaxCodeData

			Dim result As List(Of ChurchTaxCodeData) = Nothing

			Dim sql As String
			sql = String.Format("SELECT ID, Code, Bezeichnung, {0} as TranslatedText FROM Tab_Kirchen ORDER BY {0} ", MapLanguageToColumnName(SelectedTranslationLanguage))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ChurchTaxCodeData)

					While reader.Read()
						Dim contactInfoData As New ChurchTaxCodeData
						contactInfoData.ID = SafeGetInteger(reader, "ID", 0)
						contactInfoData.Code = SafeGetString(reader, "Code")
						contactInfoData.Description = SafeGetString(reader, "Bezeichnung")
						contactInfoData.TranslateChurchCodeText = SafeGetString(reader, "TranslatedText")

						result.Add(contactInfoData)

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

		''' <summary>
		''' Loads context menu data for print.
		''' </summary>
		Public Function LoadContextMenu4PrintData() As IEnumerable(Of ContextMenuForPrint) Implements IEmployeeDatabaseAccess.LoadContextMenu4PrintData

			Dim result As List(Of ContextMenuForPrint) = Nothing

			Dim sql As String

			sql = "[Get List Of Documents For Print in Employee]"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ContextMenuForPrint)

					While reader.Read()
						Dim mnuItems As New ContextMenuForPrint
						mnuItems.MnuName = SafeGetString(reader, "jobNr", String.Empty)
						mnuItems.MnuCaption = SafeGetString(reader, "Bezeichnung", String.Empty)

						result.Add(mnuItems)

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

		''' <summary>
		''' Loads context menu data for print (templates).
		''' </summary>
		Public Function LoadContextMenu4PrintTemplatesData() As IEnumerable(Of ContextMenuForPrintTemplates) Implements IEmployeeDatabaseAccess.LoadContextMenu4PrintTemplatesData

			Dim result As List(Of ContextMenuForPrintTemplates) = Nothing

			Dim sql As String

			sql = "[Get List Of Templates for Print Documents in Employee]"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ContextMenuForPrintTemplates)

					While reader.Read()
						Dim mnuItems As New ContextMenuForPrintTemplates
						mnuItems.MnuDocPath = SafeGetString(reader, "docfullname", String.Empty)
						mnuItems.MnuDocMacro = SafeGetString(reader, "makroname", String.Empty)
						mnuItems.MnuCaption = SafeGetString(reader, "menulabel", String.Empty)

						result.Add(mnuItems)

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

		''' <summary>
		''' Adds a new employee.
		''' </summary>
		''' <param name="initData">The inital data.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function AddNewEmployee(ByVal initData As NewEmployeeInitData) As Boolean Implements IEmployeeDatabaseAccess.AddNewEmployee

			Dim success = True

			Dim sql As String

			sql = "[Create New Mitarbeiter]"

			' Parameters

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			' Data of Mitarbeiter
			listOfParams.Add(New SqlClient.SqlParameter("@KST", ReplaceMissing(initData.KST, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Lastname", ReplaceMissing(initData.Lastname, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Firstname", ReplaceMissing(initData.Firstname, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Street", ReplaceMissing(initData.Street, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Countrycode", ReplaceMissing(initData.CountryCode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Postcode", ReplaceMissing(initData.Postcode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Location", ReplaceMissing(initData.Location, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Gender", ReplaceMissing(initData.Gender, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Nationality", ReplaceMissing(initData.Nationality, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@CivilState", ReplaceMissing(initData.Civilstate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Birthdate", ReplaceMissing(initData.Birthdate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Language", ReplaceMissing(initData.Language, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@BerufCode", ReplaceMissing(initData.ProfessionCode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Beruf", ReplaceMissing(initData.Profession, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@QLand", ReplaceMissing(initData.QLand, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bewillig", ReplaceMissing(initData.Permission, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bew_Bis", ReplaceMissing(initData.PermissionToDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@GebOrt", ReplaceMissing(initData.BirthPlace, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CHPartner", ReplaceMissing(initData.CHPartner, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ValidatePermissionWithTax", ReplaceMissing(initData.ValidatePermissionWithTax, True)))
			listOfParams.Add(New SqlClient.SqlParameter("NoSpecialTax", ReplaceMissing(initData.NoSpecialTax, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@S_Kanton", ReplaceMissing(initData.S_Canton, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Ansaessigkeit", ReplaceMissing(initData.Residence, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Ans_QST_Bis", ReplaceMissing(initData.ANS_QST_Bis, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Q_Steuer", ReplaceMissing(initData.Q_Steuer, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("@Kirchensteuer", ReplaceMissing(initData.ChurchTax, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("@Kinder", ReplaceMissing(initData.ChildsCount, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("@QSTGemeinde", ReplaceMissing(initData.TaxCommunityCode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@TaxCommunityLabel", ReplaceMissing(initData.TaxCommunityLabel, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@TaxCommunityCode", ReplaceMissing(initData.TaxCommunityCode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@TaxCommunity", ReplaceMissing(initData.TaxCommunity, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ForeignCategory", ReplaceMissing(initData.ForeignCategory, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ZemisNumber", ReplaceMissing(initData.ZEMISNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("EmploymentType", ReplaceMissing(initData.EmploymentType, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("OtherEmploymentType", ReplaceMissing(initData.OtherEmploymentType, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("TypeofStay", ReplaceMissing(initData.TypeOfStay, DBNull.Value)))

			' Data of MAKontakt
			listOfParams.Add(New SqlClient.SqlParameter("@DStellen", ReplaceMissing(initData.DStellen, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@NoES", ReplaceMissing(initData.NoES, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Stat1", ReplaceMissing(initData.Stat1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Stat2", ReplaceMissing(initData.Stat2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Kontakt", ReplaceMissing(initData.Contact, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@RahmCheck", ReplaceMissing(initData.RahmenCheck, DBNull.Value)))

			' Data of MA_LOSetting
			listOfParams.Add(New SqlClient.SqlParameter("@NoZG", ReplaceMissing(initData.NoZG, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@NoLO", ReplaceMissing(initData.NoLO, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Currency", ReplaceMissing(initData.Currency, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Zahlart", ReplaceMissing(initData.Zahlart, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@BVGCode", ReplaceMissing(initData.BVGCode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@SecSuvaCode", ReplaceMissing(initData.SecSuvaCode, DBNull.Value)))

			' Data of MA_LOSetting
			listOfParams.Add(New SqlClient.SqlParameter("@FerienBack", ReplaceMissing(initData.FerienBack, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@FeiertagBack", ReplaceMissing(initData.FeiertagBack, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@L13Back", ReplaceMissing(initData.L13Back, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ShowAsApplicant", ReplaceMissing(initData.ShowAsApplicant, DBNull.Value)))

			' Common data
			listOfParams.Add(New SqlClient.SqlParameter("@EmployeeNumberOffset", ReplaceMissing(initData.EmployeeNumberOffset, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Mdnr", ReplaceMissing(initData.MDNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("@Userkst", ReplaceMissing(initData.UserKST, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("@CreatedFrom", ReplaceMissing(initData.CreatedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@CreatedUserNumber", ReplaceMissing(initData.CreatedUserNumber, DBNull.Value)))

			Dim newIdParameter = New SqlClient.SqlParameter("@IdNewEmployee", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If success AndAlso Not newIdParameter.Value Is Nothing Then
				initData.IdNewEmployee = CType(newIdParameter.Value, Integer)
			Else
				success = False
			End If


			Return success
		End Function

		''' <summary>
		''' Adds a employee bussiness branch assignment.
		''' </summary>
		''' <param name="employeeBusinessBranch">The employee business branch object.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function AddEmployeeBussinessBranch(ByVal employeeBusinessBranch As EmployeeBusinessBranchData) As Boolean Implements IEmployeeDatabaseAccess.AddEmployeeBussinessBranch

			Dim success = True

			Dim sql As String

			sql = "INSERT INTO MA_Filiale (MANr, Bezeichnung, MDNr) VALUES(@employeeNumber, @description, @mdNumber)"

			' Parameters
			Dim customerNumberParameter As New SqlClient.SqlParameter("employeeNumber", ReplaceMissing(employeeBusinessBranch.EmployeeNumber, DBNull.Value))
			Dim nameParameter As New SqlClient.SqlParameter("description", ReplaceMissing(employeeBusinessBranch.Description, DBNull.Value))
			Dim mdNumberParameter As New SqlClient.SqlParameter("mdNumber", ReplaceMissing(employeeBusinessBranch.MDNr, DBNull.Value))
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(customerNumberParameter)
			listOfParams.Add(nameParameter)
			listOfParams.Add(mdNumberParameter)

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		''' <summary>
		''' Upadates employee master data.
		''' </summary>
		''' <param name="employeeMasterData">The employee master data.</param>
		''' <returns>Boolean flag indicating succeess.</returns>
		Public Function UpdateEmployeeMasterData(ByVal employeeMasterData As EmployeeMasterData) As Boolean Implements IEmployeeDatabaseAccess.UpdateEmployeeMasterData

			Dim success = True

			Dim sql As String


			sql = "[Update Assigned Employee Data]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("employeeNumber", ReplaceMissing(employeeMasterData.EmployeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("lastname", ReplaceMissing(employeeMasterData.Lastname, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("firstname", ReplaceMissing(employeeMasterData.Firstname, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("postofficebox", ReplaceMissing(employeeMasterData.PostOfficeBox, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("street", ReplaceMissing(employeeMasterData.Street, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("postcode", ReplaceMissing(employeeMasterData.Postcode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Latitude", ReplaceMissing(employeeMasterData.Latitude, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Longitude", ReplaceMissing(employeeMasterData.Longitude, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("location", ReplaceMissing(employeeMasterData.Location, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("country", ReplaceMissing(employeeMasterData.Country, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("language", ReplaceMissing(employeeMasterData.Language, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("birthdate", ReplaceMissing(employeeMasterData.Birthdate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("gender", ReplaceMissing(employeeMasterData.Gender, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("aHV_Nr", ReplaceMissing(employeeMasterData.AHV_Nr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("nationality", ReplaceMissing(employeeMasterData.Nationality, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("civilstate", ReplaceMissing(employeeMasterData.CivilStatus, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("telephone_p", ReplaceMissing(employeeMasterData.Telephone_P, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("telephone_2", ReplaceMissing(employeeMasterData.Telephone2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("telephone_3", ReplaceMissing(employeeMasterData.Telephone3, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("telephone_G", ReplaceMissing(employeeMasterData.Telephone_G, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("mobilePhone", ReplaceMissing(employeeMasterData.MobilePhone, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("homepage", ReplaceMissing(employeeMasterData.Homepage, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("email", ReplaceMissing(employeeMasterData.Email, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("permission", ReplaceMissing(employeeMasterData.Permission, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("permissionToDate", ReplaceMissing(employeeMasterData.PermissionToDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("birthLocation", ReplaceMissing(employeeMasterData.BirthPlace, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CHPartner", ReplaceMissing(employeeMasterData.CHPartner, False)))
			listOfParams.Add(New SqlClient.SqlParameter("ValidatePermissionWithTax", ReplaceMissing(employeeMasterData.ValidatePermissionWithTax, True)))
			listOfParams.Add(New SqlClient.SqlParameter("NoSpecialTax", ReplaceMissing(employeeMasterData.NoSpecialTax, False)))
			listOfParams.Add(New SqlClient.SqlParameter("q_steuer", ReplaceMissing(employeeMasterData.Q_Steuer, "0")))
			listOfParams.Add(New SqlClient.SqlParameter("s_canton", ReplaceMissing(employeeMasterData.S_Canton, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("churchtax", ReplaceMissing(employeeMasterData.ChurchTax, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("residence", ReplaceMissing(employeeMasterData.Residence, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("childsCount", ReplaceMissing(employeeMasterData.ChildsCount, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("profession", ReplaceMissing(employeeMasterData.Profession, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("staysAt", ReplaceMissing(employeeMasterData.StaysAt, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("createdOn", ReplaceMissing(employeeMasterData.CreatedOn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("changedOn", ReplaceMissing(employeeMasterData.ChangedOn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("createdFrom", ReplaceMissing(employeeMasterData.CreatedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("changedFrom", ReplaceMissing(employeeMasterData.ChangedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("hasImage", ReplaceMissing(employeeMasterData.HasImage, DBNull.Value)))
			'listOfParams.Add(New SqlClient.SqlParameter("result", ReplaceMissing(employeeMasterData.Result, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("kst", ReplaceMissing(employeeMasterData.KST, DBNull.Value)))
			'listOfParams.Add(New SqlClient.SqlParameter("firstContact", ReplaceMissing(employeeMasterData.FirstContact, DBNull.Value)))
			'listOfParams.Add(New SqlClient.SqlParameter("lastContact", ReplaceMissing(employeeMasterData.LastContact, DBNull.Value)))
			'listOfParams.Add(New SqlClient.SqlParameter("qstCommunity", ReplaceMissing(employeeMasterData.TaxCommunityCode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("TaxCommunity", ReplaceMissing(employeeMasterData.TaxCommunity, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("TaxCommunityLabel", ReplaceMissing(employeeMasterData.TaxCommunityLabel, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("TaxCommunityCode", ReplaceMissing(employeeMasterData.TaxCommunityCode, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("businessbranch", ReplaceMissing(employeeMasterData.BusinessBranch, DBNull.Value)))
			'listOfParams.Add(New SqlClient.SqlParameter("gavbez", ReplaceMissing(employeeMasterData.GAVBez, DBNull.Value)))
			'listOfParams.Add(New SqlClient.SqlParameter("civilState2", ReplaceMissing(employeeMasterData.CivilState2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("qland", ReplaceMissing(employeeMasterData.QLand, DBNull.Value)))
			'listOfParams.Add(New SqlClient.SqlParameter("maBusinessBranch", ReplaceMissing(employeeMasterData.MABusinessBranch, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ahv_Nr_New", ReplaceMissing(employeeMasterData.AHV_Nr_New, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ma_Canton", ReplaceMissing(employeeMasterData.MA_Canton, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ans_QST_Bis", ReplaceMissing(employeeMasterData.ANS_OST_Bis, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("transfered_Guid", ReplaceMissing(employeeMasterData.Transfered_Guid, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("transfered_User", ReplaceMissing(employeeMasterData.Transfered_User, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("transfered_on", ReplaceMissing(employeeMasterData.Transfered_On, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("send2WOS", ReplaceMissing(employeeMasterData.Send2WOS, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("SendDataWithEMail", ReplaceMissing(employeeMasterData.SendDataWithEMail, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ma_sms_mailing", ReplaceMissing(employeeMasterData.MA_SMS_Mailing, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ma_email_mailing", ReplaceMissing(employeeMasterData.MA_EMail_Mailing, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("professionCode", ReplaceMissing(employeeMasterData.ProfessionCode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", ReplaceMissing(employeeMasterData.MDNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("mobilePhone2", ReplaceMissing(employeeMasterData.MobilePhone2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("facebook", ReplaceMissing(employeeMasterData.Facebook, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("LinkedIn", ReplaceMissing(employeeMasterData.LinkedIn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("xing", ReplaceMissing(employeeMasterData.Xing, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ApplicantID", ReplaceMissing(employeeMasterData.ApplicantID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ApplicantLifecycle", ReplaceMissing(employeeMasterData.ApplicantLifecycle, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CVLProfileID", ReplaceMissing(employeeMasterData.CVLProfileID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("v_Hint", ReplaceMissing(employeeMasterData.V_Hint, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Notice_Employment", ReplaceMissing(employeeMasterData.Notice_Employment, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Notice_Report", ReplaceMissing(employeeMasterData.Notice_Report, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Notice_AdvancedPayment", ReplaceMissing(employeeMasterData.Notice_AdvancedPayment, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Notice_Payroll", ReplaceMissing(employeeMasterData.Notice_Payroll, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ForeignCategory", ReplaceMissing(employeeMasterData.ForeignCategory, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ZemisNumber", ReplaceMissing(employeeMasterData.ZEMISNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("EmploymentType", ReplaceMissing(employeeMasterData.EmploymentType, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("OtherEmploymentType", ReplaceMissing(employeeMasterData.OtherEmploymentType, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("TypeofStay", ReplaceMissing(employeeMasterData.TypeOfStay, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("ChangedUserNumber", ReplaceMissing(employeeMasterData.ChangedUserNumber, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Return success

		End Function

		Function UpdateEmployeeGeoData(ByVal employee As EmployeeMasterData) As Boolean Implements IEmployeeDatabaseAccess.UpdateEmployeeGeoData

			Dim success = True

			Dim sql As String

			sql = "Update [dbo].[Mitarbeiter] Set "
			sql &= "[Latitude] = @Latitude"
			sql &= ",[Longitude] = @Longitude"

			sql = sql & " WHERE MANr = @employeeNumber; "

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("employeeNumber", ReplaceMissing(employee.EmployeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Latitude", ReplaceMissing(employee.Latitude, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Longitude", ReplaceMissing(employee.Longitude, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)


			Return success

		End Function



		''' <summary>
		''' Updates employee document byte data.
		''' </summary>
		''' <returns>Boolean value indicating success.</returns>
		''' <remarks></remarks>
		Function UpdateEmployeePictureByteData(ByVal employeeNumber As Integer, ByVal filebytes() As Byte) As Boolean Implements IEmployeeDatabaseAccess.UpdateEmployeePictureByteData

			Dim success = True

			Dim sql As String

			sql = "UPDATE Mitarbeiter SET "
			sql = sql & "MABild = @fileBytes, "
			sql = sql & "Bild = 1 "
			sql = sql & "WHERE MANr = @MANr "

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("fileBytes", ReplaceMissing(filebytes, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MANr", employeeNumber))

			success = ExecuteNonQuery(sql, listOfParams)


			Return success

		End Function

		''' <summary>
		''' Updates employee contact comm data.
		''' </summary>
		''' <param name="employeeContactCommData">The employee contact comm data.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function UpdateEmployeeConactCommData(ByVal employeeContactCommData As EmployeeContactComm) As Boolean Implements IEmployeeDatabaseAccess.UpdateEmployeeConactCommData

			Dim success = True

			Dim sql As String = String.Empty

			sql = sql & "UPDATE MAKontakt_Komm SET "
			sql = sql & "[MANr] = @employeeNumber"
			sql = sql & ",[AnredeForm] = @anredeForm"
			sql = sql & ",[BriefAnrede] = @briefAnrede"
			sql = sql & ",[KontaktHow] = @kontakHow"
			sql = sql & ",[KStat1] = @kstat1"
			sql = sql & ",[KStat2] = @kstat2"
			sql = sql & ",[WebExport] = @webExport"
			sql = sql & ",[ESAb] = @esAb"
			sql = sql & ",[ESEnde] = @esEnde"
			sql = sql & ",[Absenzen] = @absenzen"
			sql = sql & ",[NoWorkAS] = @noWorkAS"
			sql = sql & ",[InLandSeit] =@inLandSeit"
			sql = sql & ",[GetAHVKarte] = @getAHVKarte"
			sql = sql & ",[GetAHVKarteBez] = @getAHVKarteBez"
			sql = sql & ",[AHVKarteBacked] = @ahvKarteBacked"
			sql = sql & ",[AHVKarteBackedBez] = @ahvKarteBackedBez"
			sql = sql & ",[InZV] = @inZV"
			sql = sql & ",[InZVBez] = @inZVBez"
			sql = sql & ",[RahmenArbeit] = @rahmenArbeit"
			sql = sql & ",[RahmenArbeitBez] = @rahmenArbeitBez"
			sql = sql & ",[Res1] = @res1"
			sql = sql & ",[Res2] = @res2"
			sql = sql & ",[Res3] = @res3"
			sql = sql & ",[Res4] = @res4"
			sql = sql & ",[KundFristen] = @kundenFristen"
			sql = sql & ",[KundGrund] = @kundenGrund"
			sql = sql & ",[Arbeitspensum] = @arbeitspensum"
			sql = sql & ",[GehaltAlt] = @gehaltAlt"
			sql = sql & ",[GehaltNeu] = @gehaltNeu"
			sql = sql & ",[GotDocs] = @gotDocs"
			sql = sql & ",[Result] = @result"
			sql = sql & ",[GehaltPerMonth] = @gehaltPerMonth"
			sql = sql & ",[GehaltPerStd] = @gehaltPerStd"
			sql = sql & ",[DStellen] = @dstellen"
			sql = sql & ",[NoES] = @noES"
			sql = sql & ",[Res5] = @res5"
			sql = sql & ",[AGB_WOS] = @agb_WOS"
			sql = sql & ",[ZVeMail] = @zVeMail"
			sql = sql & ",[ZVVersand] = @zVVersand"

			sql = sql & ",[ALKNumber] = @ALKNumber"
			sql = sql & ",[ALKName] = @ALKName"
			sql = sql & ",[ALKPOBox] = @ALKPOBox"
			sql = sql & ",[ALKStreet] = @ALKStreet"
			sql = sql & ",[ALKPostcode] = @ALKPostcode"
			sql = sql & ",[ALKLocation] = @ALKLocation"
			sql = sql & ",[ALKTelephone] = @ALKTelephone"
			sql = sql & ",[ALKTelefax] = @ALKTelefax"

			sql = sql & " WHERE MANr = @employeeNumber"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("employeeNumber", ReplaceMissing(employeeContactCommData.EmployeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("anredeForm", ReplaceMissing(employeeContactCommData.AnredeForm, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("briefAnrede", ReplaceMissing(employeeContactCommData.BriefAnrede, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("kontakHow", ReplaceMissing(employeeContactCommData.KontaktHow, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("kstat1", ReplaceMissing(employeeContactCommData.KStat1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("kstat2", ReplaceMissing(employeeContactCommData.KStat2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("webExport", ReplaceMissing(employeeContactCommData.WebExport, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("esAb", ReplaceMissing(employeeContactCommData.ESAb, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("esEnde", ReplaceMissing(employeeContactCommData.ESEnde, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("absenzen", ReplaceMissing(employeeContactCommData.Absenzen, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("noWorkAS", ReplaceMissing(employeeContactCommData.NoWorkAS, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("inLandSeit", ReplaceMissing(employeeContactCommData.InLandSeit, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("getAHVKarte", ReplaceMissing(employeeContactCommData.GetAHVKarte, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("getAHVKarteBez", ReplaceMissing(employeeContactCommData.GetAHVKarteBez, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ahvKarteBacked", ReplaceMissing(employeeContactCommData.AHVKarteBacked, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ahvKarteBackedBez", ReplaceMissing(employeeContactCommData.AHVKateBackedBez, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("inZV", ReplaceMissing(employeeContactCommData.InZV, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("inZVBez", ReplaceMissing(employeeContactCommData.InZVBez, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("rahmenArbeit", ReplaceMissing(employeeContactCommData.RahmenArbeit, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("rahmenArbeitBez", ReplaceMissing(employeeContactCommData.RahemArbeitBez, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("res1", ReplaceMissing(employeeContactCommData.Res1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("res2", ReplaceMissing(employeeContactCommData.Res2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("res3", ReplaceMissing(employeeContactCommData.Res3, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("res4", ReplaceMissing(employeeContactCommData.Res4, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("kundenFristen", ReplaceMissing(employeeContactCommData.KundFristen, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("kundenGrund", ReplaceMissing(employeeContactCommData.KundGrund, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("arbeitspensum", ReplaceMissing(employeeContactCommData.Arbeitspensum, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("gehaltAlt", ReplaceMissing(employeeContactCommData.GehaltAlt, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("gehaltNeu", ReplaceMissing(employeeContactCommData.GehaltNeu, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("gotDocs", ReplaceMissing(employeeContactCommData.GotDocs, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("result", ReplaceMissing(employeeContactCommData.Result, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("gehaltPerMonth", ReplaceMissing(employeeContactCommData.GehaltPerMonth, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("gehaltPerStd", ReplaceMissing(employeeContactCommData.GehaltPerStd, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("dstellen", ReplaceMissing(employeeContactCommData.DStellen, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("noES", ReplaceMissing(employeeContactCommData.NoES, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("res5", ReplaceMissing(employeeContactCommData.Res5, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("agb_WOS", ReplaceMissing(employeeContactCommData.AGB_WOS, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("zVeMail", ReplaceMissing(employeeContactCommData.ZVeMail, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("zVVersand", ReplaceMissing(employeeContactCommData.ZVVersand, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("ALKNumber", ReplaceMissing(employeeContactCommData.ALKNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ALKName", ReplaceMissing(employeeContactCommData.ALKName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ALKPOBox", ReplaceMissing(employeeContactCommData.ALKPOBox, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ALKStreet", ReplaceMissing(employeeContactCommData.ALKStreet, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ALKPostcode", ReplaceMissing(employeeContactCommData.ALKPostcode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ALKLocation", ReplaceMissing(employeeContactCommData.ALKLocation, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ALKTelephone", ReplaceMissing(employeeContactCommData.ALKTelephone, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ALKTelefax", ReplaceMissing(employeeContactCommData.ALKTelefax, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		''' <summary>
		''' Updates employee other data (MASonstiges.)
		''' </summary>
		''' <param name="emplyoeeOtherData">The employee other data.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function UpdateEmployeeOtherData(ByVal emplyoeeOtherData As EmployeeOtherData) As Boolean Implements IEmployeeDatabaseAccess.UpdateEmployeeOtherData

			Dim success = True

			Dim sql As String = String.Empty

			sql = sql & ""

			sql = sql & "UPDATE [MASonstiges] "
			sql = sql & "SET [MANr] = @employeeNumber"
			sql = sql & ",[ArbVol] = @arbVol"
			sql = sql & ",[Mailing] = @mailing"
			sql = sql & ",[MSprache1] = @mLanguage1"
			sql = sql & ",[MSprache2] = @mLanguage2"
			sql = sql & ",[MSprache3] = @mLanguage3"
			sql = sql & ",[SSprache1] = @sLanguage1"
			sql = sql & ",[SSprache2] = @sLanguage2"
			sql = sql & ",[SSprache3] = @sLanguage3"
			sql = sql & ",[MSprache1Level] = @mLanguage1Level"
			sql = sql & ",[MSprache2Level] = @mLanguage2Level"
			sql = sql & ",[MSprache3Level] = @mLanguage3Level"
			sql = sql & ",[SSprache1Level] = @sLanguage1Level"
			sql = sql & ",[SSprache2Level] = @sLanguage2Level"
			sql = sql & ",[SSprache3Level] = @sLanguage3Level"
			sql = sql & ",[F_Schein1] = @drivingLicence1"
			sql = sql & ",[F_Schein2] = @drivingLicence2"
			sql = sql & ",[F_schein3] = @drivingLicence3"
			sql = sql & ",[Fahrzeug] = @vehicle"
			sql = sql & ",[MSprache4] = @mLanguage4"
			sql = sql & ",[MSprache5] = @mLanguage5"
			sql = sql & ",[MSprache6] = @mLanguage6"
			sql = sql & ",[SSprache4] = @sLanguage4"
			sql = sql & ",[SSprache5] = @sLanguage5"
			sql = sql & ",[SSprache6] = @sLanguage6"
			sql = sql & ",[KundGrund] = @kundGrund"
			sql = sql & ",[BVG] = @bvg"
			sql = sql & ",[AutoReserv] = @autoReserv"
			sql = sql & " WHERE [MANr] = @employeeNumber"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("employeeNumber", ReplaceMissing(emplyoeeOtherData.EmployeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("arbVol", ReplaceMissing(emplyoeeOtherData.ArbVol, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("mailing", ReplaceMissing(emplyoeeOtherData.Mailing, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("mLanguage1", ReplaceMissing(emplyoeeOtherData.MLanguage1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("mLanguage2", ReplaceMissing(emplyoeeOtherData.MLanguage2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("mLanguage3", ReplaceMissing(emplyoeeOtherData.MLanguage3, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("sLanguage1", ReplaceMissing(emplyoeeOtherData.SLanguage1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("sLanguage2", ReplaceMissing(emplyoeeOtherData.SLanguage2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("sLanguage3", ReplaceMissing(emplyoeeOtherData.SLanguage3, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("mLanguage1Level", ReplaceMissing(emplyoeeOtherData.MLanguage1Level, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("mLanguage2Level", ReplaceMissing(emplyoeeOtherData.MLanguage2Level, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("mLanguage3Level", ReplaceMissing(emplyoeeOtherData.MLanguage3Level, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("sLanguage1Level", ReplaceMissing(emplyoeeOtherData.SLanguage1Level, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("sLanguage2Level", ReplaceMissing(emplyoeeOtherData.SLanguage2Level, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("sLanguage3Level", ReplaceMissing(emplyoeeOtherData.SLanguage3Level, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("drivingLicence1", ReplaceMissing(emplyoeeOtherData.DrivingLicence1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("drivingLicence2", ReplaceMissing(emplyoeeOtherData.DrivingLicence2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("drivingLicence3", ReplaceMissing(emplyoeeOtherData.DrivingLicence3, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("vehicle", ReplaceMissing(emplyoeeOtherData.Vehicle, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("mLanguage4", ReplaceMissing(emplyoeeOtherData.MLanguage4, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("mLanguage5", ReplaceMissing(emplyoeeOtherData.MLanguage5, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("mLanguage6", ReplaceMissing(emplyoeeOtherData.MLanguage6, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("sLanguage4", ReplaceMissing(emplyoeeOtherData.SLanguage4, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("sLanguage5", ReplaceMissing(emplyoeeOtherData.SLanguage5, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("sLanguage6", ReplaceMissing(emplyoeeOtherData.SLanguage6, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("kundGrund", ReplaceMissing(emplyoeeOtherData.KundGrund, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("bvg", ReplaceMissing(emplyoeeOtherData.BVG, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("autoReserv", ReplaceMissing(emplyoeeOtherData.AutoReserv, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateEmployeeBackupHistory(ByVal previousEmployeeMasterData As EmployeeMasterData, ByVal previousEmployeeLOSettingData As EmployeeLOSettingsData,
											 ByVal NewemployeeMasterData As EmployeeMasterData, ByVal NewemployeeLOSettingData As EmployeeLOSettingsData,
											 ByVal createdUserNumber As Integer) As Boolean Implements IEmployeeDatabaseAccess.UpdateEmployeeBackupHistory

			Dim success = True
			Dim createNewRecord As Boolean = False
			'Return True


			Dim sql As String

			createNewRecord = Not ExistsEmployeePayrollRelevantHistoryData(NewemployeeMasterData.EmployeeNumber)
			If previousEmployeeMasterData.Permission <> NewemployeeMasterData.Permission OrElse previousEmployeeMasterData.PermissionToDate <> NewemployeeMasterData.PermissionToDate OrElse
					previousEmployeeMasterData.Permission <> NewemployeeMasterData.ForeignCategory OrElse previousEmployeeMasterData.BirthPlace <> NewemployeeMasterData.BirthPlace OrElse
					previousEmployeeMasterData.CHPartner <> NewemployeeMasterData.CHPartner OrElse previousEmployeeMasterData.NoSpecialTax <> NewemployeeMasterData.NoSpecialTax OrElse
					previousEmployeeMasterData.ZEMISNumber <> NewemployeeMasterData.ZEMISNumber OrElse previousEmployeeMasterData.S_Canton <> NewemployeeMasterData.S_Canton OrElse
					previousEmployeeMasterData.Q_Steuer <> NewemployeeMasterData.Q_Steuer OrElse previousEmployeeMasterData.TaxCommunity <> NewemployeeMasterData.TaxCommunity OrElse
					previousEmployeeMasterData.ChurchTax <> NewemployeeMasterData.ChurchTax OrElse previousEmployeeMasterData.ChildsCount <> NewemployeeMasterData.ChildsCount OrElse
					previousEmployeeMasterData.EmploymentType <> NewemployeeMasterData.EmploymentType OrElse previousEmployeeMasterData.OtherEmploymentType <> NewemployeeMasterData.OtherEmploymentType OrElse
					previousEmployeeMasterData.TypeOfStay <> NewemployeeMasterData.TypeOfStay OrElse previousEmployeeMasterData.Residence <> NewemployeeMasterData.Residence OrElse
					previousEmployeeMasterData.ANS_OST_Bis <> NewemployeeMasterData.ANS_OST_Bis OrElse previousEmployeeMasterData.AHV_Nr_New <> NewemployeeMasterData.AHV_Nr_New OrElse
					previousEmployeeMasterData.Birthdate <> NewemployeeMasterData.Birthdate OrElse previousEmployeeMasterData.CivilStatus <> NewemployeeMasterData.CivilStatus OrElse
					previousEmployeeMasterData.TaxCommunityCode <> NewemployeeMasterData.TaxCommunityCode OrElse previousEmployeeMasterData.TaxCommunityLabel <> NewemployeeMasterData.TaxCommunityLabel OrElse
					previousEmployeeMasterData.Nationality <> NewemployeeMasterData.Nationality OrElse
					previousEmployeeLOSettingData.AHVCode <> NewemployeeLOSettingData.AHVCode OrElse
					previousEmployeeLOSettingData.AHVAnAm <> NewemployeeLOSettingData.AHVAnAm OrElse
					previousEmployeeLOSettingData.BVGCode <> NewemployeeLOSettingData.BVGCode OrElse
					 previousEmployeeLOSettingData.KTGPflicht <> NewemployeeLOSettingData.KTGPflicht OrElse
					previousEmployeeLOSettingData.KKPflicht <> NewemployeeLOSettingData.KKPflicht OrElse
					 previousEmployeeLOSettingData.FerienBack <> NewemployeeLOSettingData.FerienBack OrElse
					previousEmployeeLOSettingData.FeierBack <> NewemployeeLOSettingData.FeierBack OrElse
					 previousEmployeeLOSettingData.Lohn13Back <> NewemployeeLOSettingData.Lohn13Back OrElse
					 previousEmployeeLOSettingData.MAGleitzeit <> NewemployeeLOSettingData.MAGleitzeit Then

				createNewRecord = True

				Dim objectData As New List(Of String)
				objectData.Add(String.Format("previousEmployeeMasterData.Permission: {0}", previousEmployeeMasterData.Permission))
				objectData.Add(String.Format("NewemployeeMasterData.Permission: {0}", NewemployeeMasterData.Permission))
				objectData.Add(String.Format("previousEmployeeMasterData.PermissionToDate: {0}", previousEmployeeMasterData.PermissionToDate))
				objectData.Add(String.Format("NewemployeeMasterData.PermissionToDate: {0}", NewemployeeMasterData.PermissionToDate))
				objectData.Add(String.Format("previousEmployeeMasterData.Permission: {0}", previousEmployeeMasterData.Permission))
				objectData.Add(String.Format("NewemployeeMasterData.ForeignCategory: {0}", NewemployeeMasterData.ForeignCategory))
				objectData.Add(String.Format("previousEmployeeMasterData.BirthPlace: {0}", previousEmployeeMasterData.BirthPlace))
				objectData.Add(String.Format("NewemployeeMasterData.BirthPlace: {0}", NewemployeeMasterData.BirthPlace))
				objectData.Add(String.Format("previousEmployeeMasterData.CHPartner: {0}", previousEmployeeMasterData.CHPartner))
				objectData.Add(String.Format("NewemployeeMasterData.CHPartner: {0}", NewemployeeMasterData.CHPartner))
				objectData.Add(String.Format("previousEmployeeMasterData.NoSpecialTax: {0}", previousEmployeeMasterData.NoSpecialTax))
				objectData.Add(String.Format("NewemployeeMasterData.NoSpecialTax: {0}", NewemployeeMasterData.NoSpecialTax))
				objectData.Add(String.Format("previousEmployeeMasterData.ZEMISNumber: {0}", previousEmployeeMasterData.ZEMISNumber))
				objectData.Add(String.Format("NewemployeeMasterData.ZEMISNumber: {0}", NewemployeeMasterData.ZEMISNumber))
				objectData.Add(String.Format("previousEmployeeMasterData.S_Canton: {0}", previousEmployeeMasterData.S_Canton))
				objectData.Add(String.Format("NewemployeeMasterData.S_Canton: {0}", NewemployeeMasterData.S_Canton))
				objectData.Add(String.Format("previousEmployeeMasterData.Q_Steuer: {0}", previousEmployeeMasterData.Q_Steuer))
				objectData.Add(String.Format("NewemployeeMasterData.Q_Steuer: {0}", NewemployeeMasterData.Q_Steuer))
				objectData.Add(String.Format("previousEmployeeMasterData.TaxCommunity: {0}", previousEmployeeMasterData.TaxCommunity))
				objectData.Add(String.Format("NewemployeeMasterData.TaxCommunity: {0}", NewemployeeMasterData.TaxCommunity))
				objectData.Add(String.Format("previousEmployeeMasterData.TaxCommunityCode: {0}", previousEmployeeMasterData.TaxCommunityCode))
				objectData.Add(String.Format("NewemployeeMasterData.TaxCommunityCode: {0}", NewemployeeMasterData.TaxCommunityCode))
				objectData.Add(String.Format("previousEmployeeMasterData.TaxCommunityLabel: {0}", previousEmployeeMasterData.TaxCommunityLabel))
				objectData.Add(String.Format("NewemployeeMasterData.TaxCommunityLabel: {0}", NewemployeeMasterData.TaxCommunityLabel))
				objectData.Add(String.Format("previousEmployeeMasterData.ChurchTax: {0}", previousEmployeeMasterData.ChurchTax))
				objectData.Add(String.Format("NewemployeeMasterData.ChurchTax: {0}", NewemployeeMasterData.ChurchTax))
				objectData.Add(String.Format("previousEmployeeMasterData.ChildsCount: {0}", previousEmployeeMasterData.ChildsCount))
				objectData.Add(String.Format("NewemployeeMasterData.ChildsCount: {0}", NewemployeeMasterData.ChildsCount))
				objectData.Add(String.Format("previousEmployeeMasterData.EmploymentType: {0}", previousEmployeeMasterData.EmploymentType))
				objectData.Add(String.Format("NewemployeeMasterData.EmploymentType: {0}", NewemployeeMasterData.EmploymentType))
				objectData.Add(String.Format("previousEmployeeMasterData.OtherEmploymentType: {0}", previousEmployeeMasterData.OtherEmploymentType))
				objectData.Add(String.Format("NewemployeeMasterData.OtherEmploymentType: {0}", NewemployeeMasterData.OtherEmploymentType))
				objectData.Add(String.Format("previousEmployeeMasterData.TypeOfStay: {0}", previousEmployeeMasterData.TypeOfStay))
				objectData.Add(String.Format("NewemployeeMasterData.TypeOfStay: {0}", NewemployeeMasterData.TypeOfStay))
				objectData.Add(String.Format("previousEmployeeMasterData.Residence: {0}", previousEmployeeMasterData.Residence))
				objectData.Add(String.Format("NewemployeeMasterData.Residence: {0}", NewemployeeMasterData.Residence))
				objectData.Add(String.Format("previousEmployeeMasterData.ANS_OST_Bis: {0}", previousEmployeeMasterData.ANS_OST_Bis))
				objectData.Add(String.Format("NewemployeeMasterData.ANS_OST_Bis: {0}", NewemployeeMasterData.ANS_OST_Bis))
				objectData.Add(String.Format("previousEmployeeMasterData.AHV_Nr_New: {0}", previousEmployeeMasterData.AHV_Nr_New))
				objectData.Add(String.Format("NewemployeeMasterData.AHV_Nr_New: {0}", NewemployeeMasterData.AHV_Nr_New))
				objectData.Add(String.Format("previousEmployeeMasterData.Birthdate: {0}", previousEmployeeMasterData.Birthdate))
				objectData.Add(String.Format("NewemployeeMasterData.Birthdate: {0}", NewemployeeMasterData.Birthdate))
				objectData.Add(String.Format("previousEmployeeMasterData.CivilStatus: {0}", previousEmployeeMasterData.CivilStatus))
				objectData.Add(String.Format("NewemployeeMasterData.CivilStatus: {0}", NewemployeeMasterData.CivilStatus))
				objectData.Add(String.Format("previousEmployeeMasterData.Nationality: {0}", previousEmployeeMasterData.Nationality))
				objectData.Add(String.Format("NewemployeeMasterData.Nationality: {0}", NewemployeeMasterData.Nationality))
				objectData.Add(String.Format("previousEmployeeLOSettingData.AHVCode: {0}", previousEmployeeLOSettingData.AHVCode))
				objectData.Add(String.Format("NewemployeeLOSettingData.AHVCode: {0}", NewemployeeLOSettingData.AHVCode))
				objectData.Add(String.Format("previousEmployeeLOSettingData.AHVAnAm: {0}", previousEmployeeLOSettingData.AHVAnAm))
				objectData.Add(String.Format("NewemployeeLOSettingData.AHVAnAm: {0}", NewemployeeLOSettingData.AHVAnAm))
				objectData.Add(String.Format("previousEmployeeLOSettingData.BVGCode: {0}", previousEmployeeLOSettingData.BVGCode))
				objectData.Add(String.Format("NewemployeeLOSettingData.BVGCode: {0}", NewemployeeLOSettingData.BVGCode))
				objectData.Add(String.Format("previousEmployeeLOSettingData.KTGPflicht: {0}", previousEmployeeLOSettingData.KTGPflicht))
				objectData.Add(String.Format("NewemployeeLOSettingData.KTGPflicht: {0}", NewemployeeLOSettingData.KTGPflicht))
				objectData.Add(String.Format("previousEmployeeLOSettingData.KKPflicht: {0}", previousEmployeeLOSettingData.KKPflicht))
				objectData.Add(String.Format("NewemployeeLOSettingData.KKPflicht: {0}", NewemployeeLOSettingData.KKPflicht))
				objectData.Add(String.Format("previousEmployeeLOSettingData.FerienBack: {0}", previousEmployeeLOSettingData.FerienBack))
				objectData.Add(String.Format("NewemployeeLOSettingData.FerienBack: {0}", NewemployeeLOSettingData.FerienBack))
				objectData.Add(String.Format("previousEmployeeLOSettingData.FeierBack: {0}", previousEmployeeLOSettingData.FeierBack))
				objectData.Add(String.Format("NewemployeeLOSettingData.FeierBack: {0}", NewemployeeLOSettingData.FeierBack))
				objectData.Add(String.Format("previousEmployeeLOSettingData.Lohn13Back: {0}", previousEmployeeLOSettingData.Lohn13Back))
				objectData.Add(String.Format("NewemployeeLOSettingData.Lohn13Back: {0}", NewemployeeLOSettingData.Lohn13Back))
				objectData.Add(String.Format("previousEmployeeLOSettingData.MAGleitzeit: {0}", previousEmployeeLOSettingData.MAGleitzeit))
				objectData.Add(String.Format("NewemployeeLOSettingData.MAGleitzeit: {0}", NewemployeeLOSettingData.MAGleitzeit))

				m_Logger.LogWarning(String.Format("chagnes are founded. createNewRecord: {0}", createNewRecord)) ' String.Join(vbNewLine, objectData)))
			End If

			If Not createNewRecord Then Return True

			Dim employeeExistingBackupData As New EmployeeBackupHistoryData With {.EmployeeNumber = NewemployeeMasterData.EmployeeNumber}
			employeeExistingBackupData.CivilState = NewemployeeMasterData.CivilStatus
			employeeExistingBackupData.SocialInsuranceNumber = NewemployeeMasterData.AHV_Nr_New
			employeeExistingBackupData.Nationality = NewemployeeMasterData.Nationality

			employeeExistingBackupData.PermissionCode = NewemployeeMasterData.Permission
			employeeExistingBackupData.PermissionValidTo = NewemployeeMasterData.PermissionToDate
			employeeExistingBackupData.ForeignCategory = NewemployeeMasterData.ForeignCategory
			employeeExistingBackupData.BirthPlace = NewemployeeMasterData.BirthPlace
			employeeExistingBackupData.CHPartner = NewemployeeMasterData.CHPartner
			employeeExistingBackupData.NoSpecialTax = NewemployeeMasterData.NoSpecialTax
			employeeExistingBackupData.ZemisNumber = NewemployeeMasterData.ZEMISNumber
			employeeExistingBackupData.TaxCanton = NewemployeeMasterData.S_Canton
			employeeExistingBackupData.TaxCode = NewemployeeMasterData.Q_Steuer
			employeeExistingBackupData.TaxCommunityLabel = NewemployeeMasterData.TaxCommunityLabel
			employeeExistingBackupData.TaxCommunityCode = NewemployeeMasterData.TaxCommunityCode
			employeeExistingBackupData.TaxChurchCode = NewemployeeMasterData.ChurchTax
			employeeExistingBackupData.NumberOfChildren = NewemployeeMasterData.ChildsCount
			employeeExistingBackupData.EmploymentType = NewemployeeMasterData.EmploymentType
			employeeExistingBackupData.OtherEmploymentType = NewemployeeMasterData.OtherEmploymentType
			employeeExistingBackupData.TypeofStay = NewemployeeMasterData.TypeOfStay
			employeeExistingBackupData.CertificateForResidence = NewemployeeMasterData.Residence
			employeeExistingBackupData.CertificateForResidenceValidTo = NewemployeeMasterData.ANS_OST_Bis
			employeeExistingBackupData.Birthdate = NewemployeeMasterData.Birthdate

			employeeExistingBackupData.AHVCode = NewemployeeLOSettingData.AHVCode
			employeeExistingBackupData.ALVCode = NewemployeeLOSettingData.ALVCode
			employeeExistingBackupData.BVGCode = NewemployeeLOSettingData.BVGCode
			employeeExistingBackupData.KTGPflicht = NewemployeeLOSettingData.KTGPflicht
			employeeExistingBackupData.KKPflicht = NewemployeeLOSettingData.KKPflicht
			employeeExistingBackupData.FerienBack = NewemployeeLOSettingData.FerienBack
			employeeExistingBackupData.FeierBack = NewemployeeLOSettingData.FeierBack
			employeeExistingBackupData.Lohn13Back = NewemployeeLOSettingData.Lohn13Back
			employeeExistingBackupData.MAGleitzeit = NewemployeeLOSettingData.MAGleitzeit

			employeeExistingBackupData.CreatedOn = Now
			employeeExistingBackupData.CreatedUserNumber = createdUserNumber



			sql = "[Create New Emplyee Backup History For Assigned Employee Data]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("employeeNumber", ReplaceMissing(employeeExistingBackupData.EmployeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("civilstate", ReplaceMissing(employeeExistingBackupData.CivilState, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("birthdate", ReplaceMissing(employeeExistingBackupData.Birthdate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("SocialInsuranceNumber", ReplaceMissing(employeeExistingBackupData.SocialInsuranceNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("nationality", ReplaceMissing(employeeExistingBackupData.Nationality, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("permission", ReplaceMissing(employeeExistingBackupData.PermissionCode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("permissionToDate", ReplaceMissing(employeeExistingBackupData.PermissionValidTo, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ForeignCategory", ReplaceMissing(employeeExistingBackupData.ForeignCategory, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("birthLocation", ReplaceMissing(employeeExistingBackupData.BirthPlace, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CHPartner", ReplaceMissing(employeeExistingBackupData.CHPartner, False)))
			listOfParams.Add(New SqlClient.SqlParameter("NoSpecialTax", ReplaceMissing(employeeExistingBackupData.NoSpecialTax, False)))
			listOfParams.Add(New SqlClient.SqlParameter("ZemisNumber", ReplaceMissing(employeeExistingBackupData.ZemisNumber, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("TaxCanton", ReplaceMissing(employeeExistingBackupData.TaxCanton, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("TaxCommunityCode", ReplaceMissing(employeeExistingBackupData.TaxCommunityCode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("TaxCommunityLabel", ReplaceMissing(employeeExistingBackupData.TaxCommunityLabel, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("TaxCode", ReplaceMissing(employeeExistingBackupData.TaxCode, "0")))
			listOfParams.Add(New SqlClient.SqlParameter("TaxChurchCode", ReplaceMissing(employeeExistingBackupData.TaxChurchCode, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("NumberOfChildren", ReplaceMissing(employeeExistingBackupData.NumberOfChildren, 0)))

			listOfParams.Add(New SqlClient.SqlParameter("residence", ReplaceMissing(employeeExistingBackupData.CertificateForResidence, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ans_QST_Bis", ReplaceMissing(employeeExistingBackupData.CertificateForResidenceValidTo, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("EmploymentType", ReplaceMissing(employeeExistingBackupData.EmploymentType, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("OtherEmploymentType", ReplaceMissing(employeeExistingBackupData.OtherEmploymentType, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("TypeofStay", ReplaceMissing(employeeExistingBackupData.TypeofStay, DBNull.Value)))


			listOfParams.Add(New SqlClient.SqlParameter("AHVCode", ReplaceMissing(employeeExistingBackupData.AHVCode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ALVCode", ReplaceMissing(employeeExistingBackupData.ALVCode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("BVGCode", ReplaceMissing(employeeExistingBackupData.BVGCode, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("KTGPflicht", ReplaceMissing(employeeExistingBackupData.KTGPflicht, False)))
			listOfParams.Add(New SqlClient.SqlParameter("FerienBack", ReplaceMissing(employeeExistingBackupData.FerienBack, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("FeiertagBack", ReplaceMissing(employeeExistingBackupData.FeierBack, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("L13Back", ReplaceMissing(employeeExistingBackupData.Lohn13Back, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("AllowedGleitzeit", ReplaceMissing(employeeExistingBackupData.MAGleitzeit, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("UserNumber", ReplaceMissing(createdUserNumber, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Return success

		End Function

		Private Function ExistsEmployeePayrollRelevantHistoryData(ByVal employeeNumber As Integer) As Boolean

			Dim result As Boolean = False

			Dim sql As String

			sql = "[Load Assigned Employee Payroll relevant History Data]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("employeeNumber", ReplaceMissing(employeeNumber, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = reader.HasRows

				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result
		End Function




		Function ChangeEployeeDataWithApplicantData(ByVal existingEmployeeNumber As Integer, ByVal applicantEmployeeNumber As Integer) As Boolean Implements IEmployeeDatabaseAccess.ChangeEployeeDataWithApplicantData

			Dim success = True

			Dim sql As String

			sql = "UPDATE MA_Kontakte SET "
			sql &= "MANr = @existingEmployeeNumber "
			sql &= "WHERE MANr = @applicantEmployeeNumber; "

			sql &= "UPDATE MA_LLDoc SET "
			sql &= "MANr = @existingEmployeeNumber "
			sql &= "WHERE MANr = @applicantEmployeeNumber; "

			sql &= "UPDATE tbl_application SET "
			sql &= "EmployeeID = @existingEmployeeNumber "
			sql &= "WHERE EmployeeID = @applicantEmployeeNumber; "

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("applicantEmployeeNumber", ReplaceMissing(applicantEmployeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("existingEmployeeNumber", ReplaceMissing(existingEmployeeNumber, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		''' <summary>
		''' Deletes an employee business branch.
		''' </summary>
		''' <param name="id">The database id of the business branch.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function DeleteEmployeeBusinessBranch(ByVal id As Integer) As Boolean Implements IEmployeeDatabaseAccess.DeleteEmployeeBusinessBranch

			Dim success = True

			Dim sql As String

			sql = "DELETE FROM MA_Filiale WHERE ID = @id"

			Dim idParameter As New SqlClient.SqlParameter("id", id)
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(idParameter)

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		''' <summary>
		''' Deletes an employee.
		''' </summary>
		''' <param name="employeenumber">The employee number.</param>
		''' <param name="modul">The modul</param>
		''' <param name="username">The username</param>
		''' <param name="usnr">The usnr.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Function DeleteEmployee(ByVal employeenumber As Integer, ByVal modul As String, ByVal username As String, ByVal usnr As Integer) As DeleteEmployeeResult Implements IEmployeeDatabaseAccess.DeleteEmployee

			Dim success = True

			Dim sql As String

			sql = "[Delete Selected Employee]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MANr", employeenumber))
			listOfParams.Add(New SqlClient.SqlParameter("Modul", ReplaceMissing(modul, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Username", ReplaceMissing(username, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Usnr", ReplaceMissing(usnr, DBNull.Value)))

			Dim resultParameter = New SqlClient.SqlParameter("@Result", SqlDbType.Int)
			resultParameter.Direction = ParameterDirection.Output
			listOfParams.Add(resultParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Dim resultEnum As DeleteEmployeeResult

			If Not resultParameter.Value Is Nothing Then
				Try
					resultEnum = CType(resultParameter.Value, DeleteEmployeeResult)
				Catch
					resultEnum = DeleteEmployeeResult.ErrorWhileDelete
				End Try
			Else
				resultEnum = DeleteEmployeeResult.ErrorWhileDelete
			End If

			Return resultEnum

		End Function


#Region "employee properties"

		''' <summary>
		''' Gets founded Propose records for selected employee.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function LoadFoundedProposeForEmployeeMng(ByVal employeeNumber As Integer?) As IEnumerable(Of EmployeeProposeProperty) Implements IEmployeeDatabaseAccess.LoadFoundedProposeForEmployeeMng

			Dim success = True

			Dim result As List(Of EmployeeProposeProperty) = Nothing

			Dim sql As String
			If employeeNumber.HasValue Then
				sql = "[Get ProposeData 4 Selected MA In MainView]"
			Else
				sql = "[Get ProposeData 4 All MA In MainView]"
			End If

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MANr", ReplaceMissing(employeeNumber, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeProposeProperty)

					While reader.Read()
						Dim overviewData As New EmployeeProposeProperty

						overviewData.mdnr = SafeGetInteger(reader, "mdnr", 0)
						overviewData.pnr = SafeGetInteger(reader, "ProposeNr", 0)
						overviewData.kdnr = SafeGetInteger(reader, "kdnr", Nothing)
						overviewData.zhdnr = SafeGetInteger(reader, "zhdnr", Nothing)
						overviewData.manr = SafeGetInteger(reader, "manr", Nothing)

						overviewData.bezeichnung = SafeGetString(reader, "Bezeichnung")
						overviewData.customername = SafeGetString(reader, "firma1")
						overviewData.employeename = SafeGetString(reader, "maname")
						overviewData.zhdname = SafeGetString(reader, "zname")
						overviewData.p_art = SafeGetString(reader, "p_art")
						overviewData.p_state = SafeGetString(reader, "p_state")

						overviewData.advisor = SafeGetString(reader, "berater")
						overviewData.zfiliale = SafeGetString(reader, "zFiliale")
						overviewData.createdon = SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = SafeGetString(reader, "createdfrom")
						result.Add(overviewData)

					End While

					reader.Close()

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally
				CloseReader(reader)

			End Try

			Return result

		End Function

		''' <summary>
		''' Gets founded Offer records for selected employee.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function LoadFoundedOfferForEmployeeMng(ByVal employeeNumber As Integer?) As IEnumerable(Of EmployeeOfferProperty) Implements IEmployeeDatabaseAccess.LoadFoundedOfferForEmployeeMng

			Dim success = True

			Dim result As List(Of EmployeeOfferProperty) = Nothing

			Dim sql As String
			If employeeNumber.HasValue Then
				sql = "[Get OfferData 4 Selected Employee In MainView]"
			Else
				sql = "[Get OfferData 4 All Employee In MainView]"
			End If

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MANr", ReplaceMissing(employeeNumber, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeOfferProperty)

					While reader.Read()
						Dim overviewData As New EmployeeOfferProperty

						overviewData.mdnr = SafeGetInteger(reader, "mdnr", 0)

						overviewData.ofnr = SafeGetInteger(reader, "ofnr", 0)

						overviewData.kdnr = SafeGetInteger(reader, "kdnr", Nothing)
						overviewData.zhdnr = SafeGetInteger(reader, "kdzhdnr", Nothing)

						overviewData.manr = SafeGetInteger(reader, "manr", Nothing)
						overviewData.employeename = SafeGetString(reader, "maname")
						overviewData.bezeichnung = SafeGetString(reader, "bezeichnung")

						overviewData.createdon = SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = SafeGetString(reader, "createdfrom")
						overviewData.offerstate = SafeGetString(reader, "offerstate")

						overviewData.customername = SafeGetString(reader, "firma1")
						overviewData.customerstreet = SafeGetString(reader, "kdstrasse")
						overviewData.customeraddress = SafeGetString(reader, "kdadresse")
						overviewData.customeremail = SafeGetString(reader, "kdemail")
						overviewData.customertelefon = SafeGetString(reader, "kdTelefon")
						overviewData.customertelefax = SafeGetString(reader, "kdTelefax")

						overviewData.zname = SafeGetString(reader, "kdzname")
						overviewData.ztelefon = SafeGetString(reader, "zhdTelefon")
						overviewData.zmobile = SafeGetString(reader, "zhdNatel")
						overviewData.zemail = SafeGetString(reader, "zhdemail")

						overviewData.advisor = SafeGetString(reader, "Beraterin")

						overviewData.zfiliale = SafeGetString(reader, "zfiliale")

						result.Add(overviewData)

					End While

					reader.Close()

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally
				CloseReader(reader)

			End Try

			Return result

		End Function

		''' <summary>
		''' Gets founded ES records for selected employee.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function LoadFoundedESForEmployeeMng(ByVal employeeNumber As Integer?) As IEnumerable(Of EmployeeESProperty) Implements IEmployeeDatabaseAccess.LoadFoundedESForEmployeeMng

			Dim success = True

			Dim result As List(Of EmployeeESProperty) = Nothing

			Dim sql As String
			If employeeNumber.HasValue Then
				sql = "[Get ESData 4 Selected MA In MainView]"
			Else
				sql = "[Get ESData 4 All MA In MainView]"
			End If

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MANr", ReplaceMissing(employeeNumber, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeESProperty)

					While reader.Read()
						Dim overviewData As New EmployeeESProperty
						overviewData.mdnr = SafeGetInteger(reader, "mdnr", 0)
						overviewData.esnr = SafeGetInteger(reader, "esnr", 0)
						overviewData.kdnr = SafeGetInteger(reader, "kdnr", 0)
						overviewData.zhdnr = SafeGetInteger(reader, "kdzhdnr", 0)
						overviewData.manr = SafeGetInteger(reader, "manr", 0)

						overviewData.esals = SafeGetString(reader, "es_als")
						overviewData.periode = SafeGetString(reader, "periode")

						overviewData.customername = SafeGetString(reader, "Firma1")
						overviewData.employeename = SafeGetString(reader, "maname")

						overviewData.stundenlohn = SafeGetDecimal(reader, "stundenlohn", Nothing)
						overviewData.EmployeeStundenSpesen = SafeGetDecimal(reader, "MAStdSpesen", Nothing)
						overviewData.EmployeeTagesSpesen = SafeGetDecimal(reader, "MATSpesen", Nothing)

						overviewData.tarif = SafeGetDecimal(reader, "tarif", Nothing)
						overviewData.CustomerTagesSpesen = SafeGetDecimal(reader, "KDTSpesen", Nothing)
						overviewData.margemitbvg = SafeGetDecimal(reader, "MargeMitBVG", Nothing)
						overviewData.margeohnebvg = SafeGetDecimal(reader, "bruttomarge", Nothing)


						overviewData.actives = SafeGetBoolean(reader, "actives", False)

						overviewData.zfiliale = SafeGetString(reader, "zFiliale")
						overviewData.createdon = SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = SafeGetString(reader, "createdfrom")

						result.Add(overviewData)

					End While

					reader.Close()

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally
				CloseReader(reader)

			End Try

			Return result

		End Function

		''' <summary>
		''' Gets founded AdvancePayment records for selected employee.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function LoadFoundedRPForEmployeeMng(ByVal employeeNumber As Integer?) As IEnumerable(Of EmployeeReportsProperty) Implements IEmployeeDatabaseAccess.LoadFoundedRPForEmployeeMng

			Dim success = True

			Dim result As List(Of EmployeeReportsProperty) = Nothing

			Dim sql As String
			If employeeNumber.HasValue Then
				sql = "[Get RPData 4 Selected MA In MainView]"
			Else
				sql = "[Get RPData 4 All MA In MainView]"
			End If

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("@MANr", ReplaceMissing(employeeNumber, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeReportsProperty)

					While reader.Read()
						Dim overviewData As New EmployeeReportsProperty
						overviewData.mdnr = SafeGetInteger(reader, "mdnr", 0)
						overviewData.rpnr = SafeGetInteger(reader, "rpnr", Nothing)

						overviewData.manr = SafeGetInteger(reader, "manr", Nothing)
						overviewData.kdnr = SafeGetInteger(reader, "kdnr", Nothing)
						overviewData.esnr = SafeGetInteger(reader, "esnr", Nothing)
						overviewData.lonr = SafeGetInteger(reader, "lonr", Nothing)

						overviewData.monat = SafeGetInteger(reader, "monat", Nothing)
						overviewData.jahr = SafeGetInteger(reader, "jahr", Nothing)
						overviewData.rpdone = SafeGetBoolean(reader, "erfasst", Nothing)

						overviewData.employeename = SafeGetString(reader, "maname")
						overviewData.customername = SafeGetString(reader, "firma1")
						overviewData.rpgav_beruf = SafeGetString(reader, "RPGAV_Beruf")

						overviewData.periode = SafeGetString(reader, "periode")

						overviewData.zfiliale = SafeGetString(reader, "zFiliale")
						overviewData.createdon = SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = SafeGetString(reader, "createdfrom")

						result.Add(overviewData)

					End While

					reader.Close()

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally
				CloseReader(reader)

			End Try

			Return result

		End Function


		''' <summary>
		''' Gets founded AdvancePayment records for selected employee.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function LoadFoundedAdvancePaymentForEmployeeMng(ByVal employeeNumber As Integer?) As IEnumerable(Of EmployeeAdvancePaymentProperty) Implements IEmployeeDatabaseAccess.LoadFoundedAdvancePaymentForEmployeeMng

			Dim success = True

			Dim result As List(Of EmployeeAdvancePaymentProperty) = Nothing

			Dim sql As String
			If employeeNumber.HasValue Then
				sql = "[Get ZGData 4 Selected MA In MainView]"
			Else
				sql = "[Get ZGData 4 All MA In MainView]"
			End If

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("@MANr", ReplaceMissing(employeeNumber, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeAdvancePaymentProperty)

					While reader.Read()
						Dim overviewData As New EmployeeAdvancePaymentProperty

						overviewData.mdnr = CInt(SafeGetInteger(reader, "MDNr", 0))
						overviewData.employeeMDNr = SafeGetInteger(reader, "employeemdnr", 0)
						overviewData.zgnr = CInt(SafeGetInteger(reader, "ZGNr", 0))
						overviewData.rpnr = CInt(SafeGetInteger(reader, "rpnr", Nothing))
						overviewData.manr = CInt(SafeGetInteger(reader, "MAnr", 0))
						overviewData.lonr = CInt(SafeGetInteger(reader, "lonr", Nothing))
						overviewData.vgnr = CInt(SafeGetInteger(reader, "vgnr", Nothing))

						overviewData.monat = CInt(SafeGetInteger(reader, "monat", 0))
						overviewData.jahr = CInt(SafeGetInteger(reader, "jahr", 0))

						overviewData.zgperiode = SafeGetString(reader, "zgperiode")

						overviewData.betrag = SafeGetDecimal(reader, "betrag", Nothing)

						overviewData.aus_dat = SafeGetDateTime(reader, "aus_dat", Nothing)

						overviewData.employeename = SafeGetString(reader, "maname")

						overviewData.lanr = SafeGetDecimal(reader, "lanr", 0)
						overviewData.laname = SafeGetString(reader, "laname")

						overviewData.zfiliale = SafeGetString(reader, "zfiliale")
						overviewData.createdon = SafeGetDateTime(reader, "CreatedOn", Nothing)
						overviewData.createdfrom = SafeGetString(reader, "Createdfrom")

						Dim bIsOut As Boolean = SafeGetInteger(reader, "vgnr", 0) > 1
						Dim bIsAsLO As Boolean = SafeGetInteger(reader, "lonr", 0) > 1

						overviewData.isout = CBool(bIsOut)
						overviewData.isaslo = CBool(bIsAsLO)

						result.Add(overviewData)

					End While

					reader.Close()

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally
				CloseReader(reader)

			End Try

			Return result

		End Function



		''' <summary>
		''' Gets founded Payroll records for selected employee.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function LoadFoundedPayrollForEmployeeMng(ByVal employeeNumber As Integer?) As IEnumerable(Of EmployeePayrollProperty) Implements IEmployeeDatabaseAccess.LoadFoundedPayrollForEmployeeMng

			Dim success = True

			Dim result As List(Of EmployeePayrollProperty) = Nothing

			Dim sql As String
			If employeeNumber.HasValue Then
				sql = "[Get All LOData 4 Selected MA In MainView]"
			Else
				sql = "[Get New Top NewLOData 4 All MA In MainView]"
			End If

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("@MANr", ReplaceMissing(employeeNumber, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmployeePayrollProperty)

					While reader.Read()
						Dim overviewData As New EmployeePayrollProperty

						overviewData.mdnr = SafeGetInteger(reader, "mdnr", 0)
						overviewData.lonr = SafeGetInteger(reader, "lonr", 0)
						overviewData.manr = SafeGetInteger(reader, "manr", 0)

						overviewData.monat = SafeGetInteger(reader, "lp", 0)
						overviewData.jahr = SafeGetInteger(reader, "jahr", 0)

						overviewData.periode = SafeGetString(reader, "periode")

						overviewData.employeename = SafeGetString(reader, "maname")

						overviewData.zfiliale = SafeGetString(reader, "zFiliale")
						overviewData.createdon = SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = SafeGetString(reader, "createdfrom")

						overviewData.IsComplete = SafeGetBoolean(reader, "IsComplete", False)

						result.Add(overviewData)

					End While

					reader.Close()

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally
				CloseReader(reader)

			End Try

			Return result

		End Function

		''' <summary>
		''' Gets founded printed documents records for selected employee.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function LoadPrintedDocumentsForEmployeeMng(ByVal employeeNumber As Integer?, ByVal getARG As Boolean, ByVal getZV As Boolean, ByVal getPayroll As Boolean, ByVal getNLA As Boolean, ByVal getThisYear As Boolean) As IEnumerable(Of EmployeePrintedDocProperty) Implements IEmployeeDatabaseAccess.LoadPrintedDocumentsForEmployeeMng

			Dim success = True

			Dim result As List(Of EmployeePrintedDocProperty) = Nothing

			Dim sql As String

			sql = "Select MAD.* "
			sql &= "From MA_Printed_Docs MAD "
			sql &= "Where MAD.MANr = @MANr "

			If Not getARG Then sql &= String.Format("And MAD.DocName Not Like '{0}%'", "Arbeitgeber")
			If Not getZV Then sql &= String.Format("And MAD.DocName Not Like ''{0}%'", "Zwi%chenverdienst")
			If Not getPayroll Then sql &= String.Format("And MAD.DocName Not Like ''{0}%'", "Lohnabrechnung")
			If Not getNLA Then sql &= String.Format("And MAD.DocName Not Like ''{0}%'", "Lohnausweis")

			If getThisYear Then sql &= String.Format("And MAD.DocName Like '%{0}'", Now.Year)

			sql &= "Order By MAD.CreatedOn DESC"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("@MANr", ReplaceMissing(employeeNumber, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmployeePrintedDocProperty)

					While reader.Read()
						Dim overviewData As New EmployeePrintedDocProperty

						overviewData.recID = SafeGetInteger(reader, "ID", 0)
						overviewData.manr = SafeGetInteger(reader, "MANr", 0)
						overviewData.manr = SafeGetInteger(reader, "manr", 0)

						overviewData.docname = SafeGetString(reader, "DocName")

						overviewData.scandoc = SafeGetByteArray(reader, "scandoc")
						overviewData.createdon = SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.username = SafeGetString(reader, "UserName")

						result.Add(overviewData)

					End While

					reader.Close()

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally
				CloseReader(reader)

			End Try

			Return result

		End Function

		''' <summary>
		''' delete selected documents rec
		''' </summary>
		''' <param name="employeeNumber"></param>
		''' <param name="recID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function DeletePrintedDocumentsForEmployeeMng(ByVal employeeNumber As Integer?, ByVal recID As Integer) As Boolean Implements IEmployeeDatabaseAccess.DeletePrintedDocumentsForEmployeeMng

			Dim success = True
			Dim sql As String

			sql = "Delete MA_Printed_Docs Where MANr = @MANr And ID = @recID "

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("@MANr", ReplaceMissing(employeeNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("@recID", ReplaceMissing(recID, 0)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function LoadEmployeePayrollHistoryDataForEmployeeMng(ByVal employeeNumber As Integer?) As IEnumerable(Of EmployeeBackupHistoryData) Implements IEmployeeDatabaseAccess.LoadEmployeePayrollHistoryDataForEmployeeMng

			Dim success = True

			Dim result As List(Of EmployeeBackupHistoryData) = Nothing

			Dim sql As String

			sql = "[Load Assigned Employee All Payroll relevant History Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("employeeNumber", ReplaceMissing(employeeNumber, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeBackupHistoryData)

					While reader.Read()
						Dim overviewData As New EmployeeBackupHistoryData

						overviewData.EmployeeNumber = SafeGetInteger(reader, "EmployeeNumber", Nothing)

						overviewData.CivilState = SafeGetString(reader, "CivilState")
						overviewData.SocialInsuranceNumber = SafeGetString(reader, "SocialInsuranceNumber")
						overviewData.Nationality = SafeGetString(reader, "Nationality")
						overviewData.PermissionCode = SafeGetString(reader, "PermissionCode")
						overviewData.PermissionValidTo = SafeGetDateTime(reader, "PermissionValidTo", Nothing)
						overviewData.ForeignCategory = SafeGetString(reader, "ForeignCategory")
						overviewData.BirthPlace = SafeGetString(reader, "Hometown")
						overviewData.CHPartner = SafeGetBoolean(reader, "CHPartner", False)
						overviewData.NoSpecialTax = SafeGetBoolean(reader, "NoSpecialTax", False)

						overviewData.ZemisNumber = SafeGetString(reader, "ZemisNumber")
						overviewData.TaxCanton = SafeGetString(reader, "TaxCanton")
						overviewData.TaxCode = SafeGetString(reader, "TaxCode")
						overviewData.TaxCommunityLabel = SafeGetString(reader, "TaxCommunityLabel")

						overviewData.TaxCommunityCode = SafeGetInteger(reader, "TaxCommunityCode", Nothing)
						overviewData.TaxChurchCode = SafeGetString(reader, "TaxChurchCode")
						overviewData.NumberOfChildren = SafeGetInteger(reader, "NumberOfChildren", 0)
						overviewData.EmploymentType = SafeGetString(reader, "EmploymentType")
						overviewData.OtherEmploymentType = SafeGetString(reader, "OtherEmploymentType")
						overviewData.TypeofStay = SafeGetString(reader, "TypeofStay")
						overviewData.CertificateForResidence = SafeGetBoolean(reader, "CertificateForResidence", False)
						overviewData.CertificateForResidenceValidTo = SafeGetDateTime(reader, "CertificateForResidenceValidTo", Nothing)
						overviewData.Birthdate = SafeGetDateTime(reader, "Birthdate", Nothing)

						overviewData.AHVCode = SafeGetString(reader, "AHVCode")
						overviewData.ALVCode = SafeGetString(reader, "ALVCode")
						overviewData.BVGCode = SafeGetString(reader, "BVGCode")
						overviewData.KTGPflicht = SafeGetBoolean(reader, "AllowedKTG", False)
						overviewData.FerienBack = SafeGetBoolean(reader, "BackedFerien", False)
						overviewData.FeierBack = SafeGetBoolean(reader, "BackedFeiertag", False)
						overviewData.Lohn13Back = SafeGetBoolean(reader, "Backed13Lohn", False)
						overviewData.MAGleitzeit = SafeGetBoolean(reader, "AllowedGleitzeit", False)

						overviewData.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						overviewData.CreatedFrom = SafeGetString(reader, "createdfrom")
						overviewData.CreatedUserNumber = SafeGetInteger(reader, "CreatedUserNumber", Nothing)


						result.Add(overviewData)

					End While

					reader.Close()

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally
				CloseReader(reader)

			End Try

			Return result

		End Function

#End Region



	End Class



End Namespace