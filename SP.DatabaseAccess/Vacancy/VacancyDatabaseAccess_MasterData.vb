
Imports System.Data.SqlClient
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.Vacancy.DataObjects


Namespace Vacancy

	Partial Class VacancyDatabaseAccess

		Inherits DatabaseAccessBase
		Implements IVacancyDatabaseAccess


		''' <summary>
		''' Loads vacancy master data.
		''' </summary>
		Function LoadVacancyMasterData(ByVal mdNr As Integer, ByVal VacancyNumber As Integer) As VacancyMasterData Implements IVacancyDatabaseAccess.LoadVacancyMasterData
			Dim result As VacancyMasterData = Nothing

			Dim sql As String

			sql = "[Load Assigned Vacancy Master Data]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", ReplaceMissing(mdNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("vacancyNumber", ReplaceMissing(VacancyNumber, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then
					result = New VacancyMasterData

					Dim data = New VacancyMasterData()

					data.ID = SafeGetInteger(reader, "ID", 0)
					data.VakNr = SafeGetInteger(reader, "VakNr", 0)
					data.Berater = SafeGetString(reader, "Berater")
					data.AdvisorNumber = SafeGetInteger(reader, "AdvisorNumber", 0)
					data.Filiale = SafeGetString(reader, "Filiale")
					data.VakKontakt = SafeGetString(reader, "VakKontakt")
					data.VakState = SafeGetString(reader, "VakState")
					data.VakKontakt_Value = SafeGetInteger(reader, "VakKontakt_Value", 0)
					data.VakState_Value = SafeGetInteger(reader, "VakState_Value", 0)
					data.Bezeichnung = SafeGetString(reader, "Bezeichnung")
					data.Slogan = SafeGetString(reader, "Slogan")
					data.Gruppe = SafeGetString(reader, "Gruppe")
					data.SubGroup = SafeGetString(reader, "SubGroup")
					data.KDNr = SafeGetInteger(reader, "KDNr", 0)

					data.SBNNumber = SafeGetInteger(reader, "SBNNumber", 0)
					data.SBNPublicationState = SafeGetInteger(reader, "SBNPublicationState", 0)
					data.SBNPublicationDate = SafeGetDateTime(reader, "SBNPublicationDate", Nothing)
					data.SBNPublicationFrom = SafeGetString(reader, "SBNPublicationFrom")

					data.KDZHDNr = SafeGetInteger(reader, "KDZHDNr", 0)
					data.ExistLink = SafeGetBoolean(reader, "ExistLink", False)
					data.VakLink = SafeGetString(reader, "VakLink")
					data.Beginn = SafeGetString(reader, "Beginn")
					data.JobProzent = SafeGetString(reader, "JobProzent")
					data.Anstellung = SafeGetString(reader, "Anstellung")
					data.Dauer = SafeGetString(reader, "Dauer")
					data.MAAge = SafeGetString(reader, "MAAge")
					data.MASex = SafeGetString(reader, "MASex")
					data.MAZivil = SafeGetString(reader, "MAZivil")
					data.MALohn = SafeGetString(reader, "MALohn")
					data.Jobtime = SafeGetString(reader, "Jobtime")
					data.JobOrt = SafeGetString(reader, "JobOrt")
					data.MAFSchein = SafeGetString(reader, "MAFSchein")
					data.MAAuto = SafeGetString(reader, "MAAuto")
					data.MANationality = SafeGetString(reader, "MANationality")

					data.IEExport = SafeGetBoolean(reader, "IEExport", False)
					data.IsJobsCHOnline = SafeGetBoolean(reader, "IsJobsCHOnline", False)
					data.IsOstJobOnline = SafeGetBoolean(reader, "IsOstJobOnline", False)

					data.KDBeschreibung = SafeGetString(reader, "KDBeschreibung")
					data.KDBietet = SafeGetString(reader, "KDBietet")
					data.SBeschreibung = SafeGetString(reader, "SBeschreibung")
					data.Reserve1 = SafeGetString(reader, "Reserve1")
					data.Taetigkeit = SafeGetString(reader, "Taetigkeit")
					data.Anforderung = SafeGetString(reader, "Anforderung")
					data.Reserve2 = SafeGetString(reader, "Reserve2")
					data.Reserve3 = SafeGetString(reader, "Reserve3")
					data.Ausbildung = SafeGetString(reader, "Ausbildung")
					data.Weiterbildung = SafeGetString(reader, "Weiterbildung")
					data.SKennt = SafeGetString(reader, "SKennt")
					data.EDVKennt = SafeGetString(reader, "EDVKennt")
					data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
					data.CreatedFrom = SafeGetString(reader, "CreatedFrom")
					data.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
					data.ChangedFrom = SafeGetString(reader, "ChangedFrom")
					data.Result = SafeGetString(reader, "Result")
					data.Vak_Region = SafeGetString(reader, "Vak_Region")
					data.Transfered_User = SafeGetString(reader, "Transfered_User")
					data.FirstTransferDate = SafeGetDateTime(reader, "FirstTransferDate", Nothing)
					data.Transfered_On = SafeGetDateTime(reader, "Transfered_On", Nothing)
					data.Transfered_Guid = SafeGetString(reader, "Transfered_Guid")
					data.Vak_Kanton = SafeGetString(reader, "Vak_Kanton")
					data.Customer_Guid = SafeGetString(reader, "Customer_Guid")
					data.Bemerkung = SafeGetString(reader, "Bemerkung")
					data.MDNr = SafeGetInteger(reader, "MDNr", 0)
					data.JobPLZ = SafeGetString(reader, "JobPLZ")
					data.UserKontakt = SafeGetString(reader, "UserKontakt")
					data.UserEMail = SafeGetString(reader, "UserEMail")
					data.TitelForSearch = SafeGetString(reader, "TitelForSearch")
					data.ShortDescription = SafeGetString(reader, "ShortDescription")
					data.CreatedUserNumber = SafeGetInteger(reader, "CreatedUserNumber", Nothing)
					data.ChangedUserNumber = SafeGetInteger(reader, "ChangedUserNumber", Nothing)


					result = data

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result
		End Function

		Function LoadJobCHMasterData(ByVal customerID As String, ByVal userNumber As Integer, ByVal VacancyNumber As Integer) As VacancyJobCHMasterData Implements IVacancyDatabaseAccess.LoadJobCHMasterData
			Dim result As VacancyJobCHMasterData = Nothing

			Dim sql As String

			sql = "[Load JobCH Vacancy Master Data]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USNr", ReplaceMissing(userNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("iVakNr", ReplaceMissing(VacancyNumber, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				result = New VacancyJobCHMasterData
				If (Not reader Is Nothing AndAlso reader.Read()) Then

					Dim data = New VacancyJobCHMasterData()

					data.ID = SafeGetInteger(reader, "JobCHID", 0)
					data.VakNr = SafeGetInteger(reader, "VakNr", 0)

					data.UserKontakt = SafeGetString(reader, "UserKontakt")
					data.UserEMail = SafeGetString(reader, "UserEMail")
					data.Organisation_ID = SafeGetInteger(reader, "Organisation_ID", 0)
					data.Organisation_SubID = SafeGetInteger(reader, "Organisation_SubID", 0)
					data.Our_URL = SafeGetString(reader, "Our_URL")
					data.Direkt_URL = SafeGetString(reader, "Direkt_URL")
					data.Layout_ID = SafeGetInteger(reader, "Layout_ID", 0)
					data.Logo_ID = SafeGetInteger(reader, "Logo_ID", 0)
					data.Bewerben_URL = SafeGetString(reader, "Bewerben_URL")
					data.Angebot_Value = SafeGetInteger(reader, "Angebot_Value", 0)
					data.Xing_Poster_URL = SafeGetString(reader, "Xing_Poster_URL")
					data.Xing_Company_Profile_URL = SafeGetString(reader, "Xing_Company_Profile_URL")
					data.Xing_Company_Is_Poc = SafeGetBoolean(reader, "Xing_Company_Is_Poc", False)
					data.StartDate = SafeGetDateTime(reader, "StartDate", Nothing)
					data.EndDate = SafeGetDateTime(reader, "EndDate", Nothing)

					data.TitelForSearch = SafeGetString(reader, "TitelForSearch")
					data.ShortDescription = SafeGetString(reader, "ShortDescription")
					data.Beginn = SafeGetString(reader, "Beginn")
					data.JobProzent = SafeGetString(reader, "JobProzent")
					data.Anstellung = SafeGetString(reader, "Anstellung")
					data.Dauer = SafeGetString(reader, "Dauer")
					data.MAAge = SafeGetString(reader, "MAAge")
					data.MASex = SafeGetString(reader, "MASex")

					data.Vak_Kanton = SafeGetString(reader, "Vak_Kanton")
					data.MAAge = SafeGetString(reader, "MAAge")

					data.IEExport = SafeGetBoolean(reader, "IEExport", False)
					data.JobChannelPriority = SafeGetBoolean(reader, "JobChannelPriority", False)
					data.Firma1 = SafeGetString(reader, "Firma1")
					data.KDzNachname = SafeGetString(reader, "KDzNachname")
					data.KDzVorname = SafeGetString(reader, "KDzVorname")
					data.USVorname = SafeGetString(reader, "USVorname")
					data.USNachname = SafeGetString(reader, "USNachname")
					data.BranchenValue = SafeGetInteger(reader, "BranchenValue", 0)
					data.BranchenBez = SafeGetString(reader, "BranchenBez")
					data.Position_Value = SafeGetInteger(reader, "Position_Value", 0)
					data.Position = SafeGetString(reader, "Position")
					data.Inserat_ID = SafeGetString(reader, "Inserat_ID")
					data.Branche = SafeGetString(reader, "Branche")
					data.Vak_Sprache = SafeGetString(reader, "Vak_Sprache")
					data.IsOnline = SafeGetBoolean(reader, "IsOnline", False)
					data.FirstJobsCHTransferDate = SafeGetDateTime(reader, "FirstTransferDate", Nothing)
					data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
					data.CreatedFrom = SafeGetString(reader, "CreatedFrom")
					data.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
					data.ChangedFrom = SafeGetString(reader, "ChangedFrom")
					data.USJCHSub_ID = SafeGetInteger(reader, "USJCHSub_ID", 0)
					data.USJCHLayout_ID = SafeGetInteger(reader, "USJCHLayout_ID", 0)
					data.USJCHLogo_ID = SafeGetInteger(reader, "USJCHLogo_ID", 0)
					data.USJCHOur_URL = SafeGetString(reader, "USJCHOur_URL")
					data.DaysToAdd = SafeGetInteger(reader, "DaysToAdd", 0)
					data.USJCHDirekt_URL = SafeGetString(reader, "USJCHDirekt_URL")
					data.USJCHXing_Poster_URL = SafeGetString(reader, "USJCHXing_Poster_URL")
					data.USJCHXing_Company_Profile_URL = SafeGetString(reader, "USJCHXing_Company_Profile_URL")
					data.USJCHXing_Company_Is_Poc = SafeGetBoolean(reader, "USJCHXing_Company_Is_Poc", False)
					data.CreatedUserNumber = SafeGetInteger(reader, "CreatedUserNumber", Nothing)
					data.ChangedUserNumber = SafeGetInteger(reader, "ChangedUserNumber", Nothing)


					result = data

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result
		End Function

		Function LoadOstJobMasterData(ByVal customerID As String, ByVal userName As String, ByVal userNumber As Integer, ByVal VacancyNumber As Integer) As VacancyOstJobMasterData Implements IVacancyDatabaseAccess.LoadOstJobMasterData
			Dim result As VacancyOstJobMasterData = Nothing

			Dim sql As String

			sql = "[Load Ostjob Vacancy Master Data]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USNr", ReplaceMissing(userNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("iVakNr", ReplaceMissing(VacancyNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("createdFrom", ReplaceMissing(userName, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then
					result = New VacancyOstJobMasterData

					Dim data = New VacancyOstJobMasterData()
					data.id = SafeGetInteger(reader, "ID", 0)
					data.VakNr = SafeGetInteger(reader, "VakNr", 0)

					data.interneid = SafeGetString(reader, "interneid")
					data.keywords = SafeGetString(reader, "keywords")
					data.linkiframe = SafeGetString(reader, "linkiframe")
					data.USOSJDirekt_URL = SafeGetString(reader, "USOSJDirekt_URL")
					data.bewerberlink = SafeGetString(reader, "bewerberlink")

					data.startdate = SafeGetDateTime(reader, "startdate", Nothing)
					data.enddate = SafeGetDateTime(reader, "enddate", Nothing)
					data.isonline = SafeGetBoolean(reader, "isonline", False)

					data.ostjob = SafeGetBoolean(reader, "ostjob", False)
					data.zentraljob = SafeGetBoolean(reader, "zentraljob", False)
					data.minisite = SafeGetBoolean(reader, "minisite", False)
					data.nicejob = SafeGetBoolean(reader, "nicejob", False)
					data.westjob = SafeGetBoolean(reader, "westjob", False)

					data.companyhomepage = SafeGetBoolean(reader, "companyhomepage", False)
					data.lehrstelle = SafeGetBoolean(reader, "lehrstelle", False)

					data.layoutid = SafeGetInteger(reader, "layout_id", 0)
					data.FirstOstJobTransferDate = SafeGetDateTime(reader, "FirstTransferDate", Nothing)
					data.createdon = SafeGetDateTime(reader, "createdon", Nothing)
					data.createdfrom = SafeGetString(reader, "createdfrom")
					data.CreatedUserNumber = SafeGetInteger(reader, "CreatedUserNumber", Nothing)
					data.ChangedUserNumber = SafeGetInteger(reader, "ChangedUserNumber", Nothing)


					result = data
				End If

			Catch e As Exception
				m_Logger.LogError(e.ToString())
				result = Nothing

			Finally
				CloseReader(reader)

			End Try

			Return result
		End Function

		Function LoadStmpSettingData(ByVal customerID As String, ByVal userName As String, ByVal userNumber As Integer, ByVal VacancyNumber As Integer) As VacancyStmpSettingData Implements IVacancyDatabaseAccess.LoadStmpSettingData
			Dim result As VacancyStmpSettingData = Nothing

			Dim sql As String
			sql = "[Load STMP Vacancy Setting Data]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("iVakNr", ReplaceMissing(VacancyNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USNr", ReplaceMissing(userNumber, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				result = New VacancyStmpSettingData
				If (Not reader Is Nothing AndAlso reader.Read()) Then

					Dim data = New VacancyStmpSettingData()
					data.id = SafeGetInteger(reader, "ID", 0)
					data.VakNr = SafeGetInteger(reader, "VakNr", 0)
					data.EducationCode = SafeGetInteger(reader, "EducationCode", 0)

					data.PublicationStartDate = SafeGetDateTime(reader, "startdate", Nothing)
					data.PublicationEndDate = SafeGetDateTime(reader, "enddate", Nothing)
					data.NumberOfJobs = SafeGetInteger(reader, "NumberOfJobs", 1)

					data.Less_One_Year = SafeGetBoolean(reader, "Less_One_Year", False)
					data.More_One_Year = SafeGetBoolean(reader, "More_One_Year", False)
					data.More_Three_Years = SafeGetBoolean(reader, "More_Three_Years", False)
					data.Sunday_and_Holidays = SafeGetBoolean(reader, "Sunday_and_Holidays", Nothing)
					data.Shift_Work = SafeGetBoolean(reader, "Shift_Work", Nothing)
					data.Night_Work = SafeGetBoolean(reader, "Night_Work", Nothing)
					data.Home_Work = SafeGetBoolean(reader, "Home_Work", Nothing)
					data.ReportToAvam = SafeGetBoolean(reader, "ReportToAvam", False)
					data.ShortEmployment = SafeGetBoolean(reader, "ShortEmployment", False)
					data.Immediately = SafeGetBoolean(reader, "Immediately", False)
					data.Surrogate = SafeGetBoolean(reader, "Surrogate", False)
					data.Permanent = SafeGetBoolean(reader, "Permanent", False)
					data.EuresDisplay = SafeGetBoolean(reader, "EuresDisplay", False)
					data.PublicDisplay = SafeGetBoolean(reader, "PublicDisplay", False)

					data.AVAMRecordState = SafeGetString(reader, "AVAMRecordState")
					data.JobroomID = SafeGetString(reader, "JobRoomID")
					data.stellennummerEgov = SafeGetString(reader, "stellennummerEgov")
					data.ReportingObligation = SafeGetBoolean(reader, "ReportingObligation", False)
					data.ReportingObligationEndDate = SafeGetDateTime(reader, "ReportingObligationEndDate", Nothing)
					data.ReportingDate = SafeGetDateTime(reader, "ReportingDate", Nothing)
					data.ReportingFrom = SafeGetString(reader, "ReportingFrom")
					data.SyncDate = SafeGetDateTime(reader, "SyncDate", Nothing)
					data.SyncFrom = SafeGetString(reader, "SyncFrom")

					result = data
				End If

			Catch e As Exception
				m_Logger.LogError(e.ToString())
				result = Nothing

			Finally
				CloseReader(reader)

			End Try

			Return result
		End Function

		Function UpdateVacancyStmpSettingData(ByVal customerID As String, ByVal userNumber As Integer, ByVal stmpData As VacancyStmpSettingData) As Boolean Implements IVacancyDatabaseAccess.UpdateVacancyStmpSettingData

			Dim success = True

			Dim sql As String

			sql = "UPDATE Dbo.[tbl_STMPVacancySetting] SET "
			sql = sql & "VakNr = @VakNr "
			sql = sql & ",startdate = @startdate "
			sql = sql & ",enddate = @enddate "
			sql = sql & ",NumberOfJobs = @numberOfJobs "
			sql = sql & ",Less_One_Year = @Less_One_Year "
			sql = sql & ",More_One_Year = @More_One_Year "
			sql = sql & ",More_Three_Years = @More_Three_Years "
			sql = sql & ",Sunday_and_Holidays = @Sunday_and_Holidays "
			sql = sql & ",Shift_Work = @Shift_Work "
			sql = sql & ",Night_Work = @Night_Work "
			sql = sql & ",Home_Work = @Home_Work "
			sql = sql & ",ReportToAvam = @ReportToAvam "
			sql = sql & ",ShortEmployment = @ShortEmployment "
			sql = sql & ",Immediately = @Immediately "
			sql = sql & ",Surrogate = @Surrogate "
			sql = sql & ",Permanent = @Permanent "
			sql = sql & ",EuresDisplay = @EuresDisplay "
			sql = sql & ",PublicDisplay = @PublicDisplay "

			sql = sql & ",AVAMRecordState = @AVAMRecordState "
			sql = sql & ",JobroomID = @JobroomID "
			sql = sql & ",StellennummerEgov = @stellennummerEgov "
			sql = sql & ",ReportingObligation = @ReportingObligation "
			sql = sql & ",ReportingObligationEndDate = @ReportingObligationEndDate "
			sql = sql & ",ReportingDate = @ReportingDate "
			sql = sql & ",ReportingFrom = @ReportingFrom "
			sql = sql & ",SyncDate = @SyncDate "
			sql = sql & ",SyncFrom = @SyncFrom "
			sql = sql & ",EducationCode = @EducationCode "

			sql = sql & "WHERE Vaknr = @VakNr "

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("VakNr", ReplaceMissing(stmpData.VakNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("startdate", ReplaceMissing(stmpData.PublicationStartDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("enddate", ReplaceMissing(stmpData.PublicationEndDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("numberOfJobs", ReplaceMissing(stmpData.NumberOfJobs, 1)))
			listOfParams.Add(New SqlClient.SqlParameter("Less_One_Year", ReplaceMissing(stmpData.Less_One_Year, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("More_One_Year", ReplaceMissing(stmpData.More_One_Year, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("More_Three_Years", ReplaceMissing(stmpData.More_Three_Years, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Sunday_and_Holidays", ReplaceMissing(stmpData.Sunday_and_Holidays, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Shift_Work", ReplaceMissing(stmpData.Shift_Work, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Night_Work", ReplaceMissing(stmpData.Night_Work, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Home_Work", ReplaceMissing(stmpData.Home_Work, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ReportToAvam", ReplaceMissing(stmpData.ReportToAvam, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ShortEmployment", ReplaceMissing(stmpData.ShortEmployment, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Immediately", ReplaceMissing(stmpData.Immediately, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Surrogate", ReplaceMissing(stmpData.Surrogate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Permanent", ReplaceMissing(stmpData.Permanent, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("EuresDisplay", ReplaceMissing(stmpData.EuresDisplay, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("PublicDisplay", ReplaceMissing(stmpData.PublicDisplay, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("AVAMRecordState", ReplaceMissing(stmpData.AVAMRecordState, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("JobroomID", ReplaceMissing(stmpData.JobroomID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("stellennummerEgov", ReplaceMissing(stmpData.stellennummerEgov, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ReportingObligation", ReplaceMissing(stmpData.ReportingObligation, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ReportingObligationEndDate", ReplaceMissing(stmpData.ReportingObligationEndDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ReportingDate", ReplaceMissing(stmpData.ReportingDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ReportingFrom", ReplaceMissing(stmpData.ReportingFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("SyncDate", ReplaceMissing(stmpData.SyncDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("SyncFrom", ReplaceMissing(stmpData.SyncFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("EducationCode", ReplaceMissing(stmpData.EducationCode, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.Text)


			Return success

		End Function

		Function UpdateVacancyOstJobMasterData(ByVal customerID As String, ByVal userNumber As Integer, ByVal ojdata As VacancyOstJobMasterData) As Boolean Implements IVacancyDatabaseAccess.UpdateVacancyOstJobMasterData

			Dim success = True

			Dim sql As String

			sql = "[Update Vacancy OstJob Master Data]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("vacancyNumber", ReplaceMissing(ojdata.VakNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("usernr", ReplaceMissing(userNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("interneid", ReplaceMissing(ojdata.interneid, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("keywords", ReplaceMissing(ojdata.keywords, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("linkiframe", ReplaceMissing(ojdata.linkiframe, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("bewerberlink", ReplaceMissing(ojdata.bewerberlink, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("isonline", ReplaceMissing(ojdata.isonline, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("ostjob", ReplaceMissing(ojdata.ostjob, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("zentraljob", ReplaceMissing(ojdata.zentraljob, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("minisite", ReplaceMissing(ojdata.minisite, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("nicejob", ReplaceMissing(ojdata.nicejob, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("westjob", ReplaceMissing(ojdata.westjob, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("companyhomepage", ReplaceMissing(ojdata.companyhomepage, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("lehrstelle", ReplaceMissing(ojdata.lehrstelle, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("startdate", ReplaceMissing(ojdata.startdate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("enddate", ReplaceMissing(ojdata.enddate, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("layoutid", ReplaceMissing(ojdata.layoutid, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("changedFrom", ReplaceMissing(ojdata.changedfrom, DBNull.Value)))

			If ojdata.isonline.GetValueOrDefault(False) Then
				If Not ojdata.FirstOstJobTransferDate.HasValue Then ojdata.FirstOstJobTransferDate = Now
			End If
			listOfParams.Add(New SqlClient.SqlParameter("FirstOstJobTransferDate", ReplaceMissing(ojdata.FirstOstJobTransferDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("changedUserNumber", ReplaceMissing(ojdata.ChangedUserNumber, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success

		End Function

		Function AddNewVacancy(ByVal vacancyMasterData As VacancyMasterData) As Boolean Implements IVacancyDatabaseAccess.AddNewVacancy

			Dim success = True

			Dim sql As String

			sql = "[Create New Vacancy]"


			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			' Data of vacancy

			listOfParams.Add(New SqlClient.SqlParameter("Berater", ReplaceMissing(vacancyMasterData.Berater, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Filiale", ReplaceMissing(vacancyMasterData.Filiale, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("VakKontakt", ReplaceMissing(vacancyMasterData.VakKontakt, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("VakState", ReplaceMissing(vacancyMasterData.VakState, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("SBNNumber", ReplaceMissing(vacancyMasterData.SBNNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Bezeichnung", ReplaceMissing(vacancyMasterData.Bezeichnung, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Slogan", ReplaceMissing(vacancyMasterData.Slogan, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Gruppe", ReplaceMissing(vacancyMasterData.Gruppe, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("SubGroup", ReplaceMissing(vacancyMasterData.SubGroup, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(vacancyMasterData.KDNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KDZHDNr", ReplaceMissing(vacancyMasterData.KDZHDNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ExistLink", ReplaceMissing(vacancyMasterData.ExistLink, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("VakLink", ReplaceMissing(vacancyMasterData.VakLink, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MALohn", ReplaceMissing(vacancyMasterData.MALohn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Jobtime", ReplaceMissing(vacancyMasterData.Jobtime, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("JobPLZ", ReplaceMissing(vacancyMasterData.JobPLZ, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("JobOrt", ReplaceMissing(vacancyMasterData.JobOrt, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Bemerkung", ReplaceMissing(vacancyMasterData.Bemerkung, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MAAuto", ReplaceMissing(vacancyMasterData.MAAuto, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Customer_Guid", ReplaceMissing(vacancyMasterData.Customer_Guid, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("UserKontakt", ReplaceMissing(vacancyMasterData.UserKontakt, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("UserEMail", ReplaceMissing(vacancyMasterData.UserEMail, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("CreatedFrom", ReplaceMissing(vacancyMasterData.CreatedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("VacancyNumberOffset", ReplaceMissing(vacancyMasterData.VacancyNumberOffset, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(vacancyMasterData.MDNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CreatedUserNumber", ReplaceMissing(vacancyMasterData.CreatedUserNumber, DBNull.Value)))

			Dim newIdParameter = New SqlClient.SqlParameter("IdNewVacancy", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If success AndAlso Not newIdParameter.Value Is Nothing Then
				vacancyMasterData.VakNr = CType(newIdParameter.Value, Integer)
			Else
				success = False
			End If


			Return success

		End Function

		Function UpdateVacancyMasterData(ByVal vacancyMasterData As VacancyMasterData) As Boolean Implements IVacancyDatabaseAccess.UpdateVacancyMasterData

			Dim success = True

			Dim sql As String

			sql = "[Update Vacancy Data]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("VakNr", ReplaceMissing(vacancyMasterData.VakNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Berater", ReplaceMissing(vacancyMasterData.Berater, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Filiale", ReplaceMissing(vacancyMasterData.Filiale, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("VakKontakt", ReplaceMissing(vacancyMasterData.VakKontakt, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("VakState", ReplaceMissing(vacancyMasterData.VakState, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("SBNNumber", ReplaceMissing(vacancyMasterData.SBNNumber.GetValueOrDefault(0), 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Bezeichnung", ReplaceMissing(vacancyMasterData.Bezeichnung, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Slogan", ReplaceMissing(vacancyMasterData.Slogan, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Gruppe", ReplaceMissing(vacancyMasterData.Gruppe, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("SubGroup", ReplaceMissing(vacancyMasterData.SubGroup, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(vacancyMasterData.KDNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KDZHDNr", ReplaceMissing(vacancyMasterData.KDZHDNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ExistLink", ReplaceMissing(vacancyMasterData.ExistLink, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("VakLink", ReplaceMissing(vacancyMasterData.VakLink, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MALohn", ReplaceMissing(vacancyMasterData.MALohn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Jobtime", ReplaceMissing(vacancyMasterData.Jobtime, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("JobPLZ", ReplaceMissing(vacancyMasterData.JobPLZ, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("JobOrt", ReplaceMissing(vacancyMasterData.JobOrt, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Bemerkung", ReplaceMissing(vacancyMasterData.Bemerkung, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MAAuto", ReplaceMissing(vacancyMasterData.MAAuto, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Customer_Guid", ReplaceMissing(vacancyMasterData.Customer_Guid, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("UserKontakt", ReplaceMissing(vacancyMasterData.UserKontakt, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("UserEMail", ReplaceMissing(vacancyMasterData.UserEMail, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("ChangedFrom", ReplaceMissing(vacancyMasterData.ChangedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(vacancyMasterData.MDNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ChangedUserNumber", ReplaceMissing(vacancyMasterData.ChangedUserNumber, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Return success

		End Function

		Function UpdateVacancyOnlineData(ByVal vacancyNumber As Integer, ByVal customerNumber As Integer, ByVal ownerOnline As Boolean, ByVal JobChannelPriority As Boolean?, ByVal jobCHOnline As Boolean, ByVal ostJobOnline As Boolean) As Boolean Implements IVacancyDatabaseAccess.UpdateVacancyOnlineData

			Dim success = True

			Dim sql As String

			sql = "Update Vakanzen Set "
			sql &= "IEExport = @ownerOnline "
			sql &= ",JobChannelPriority = @JobChannelPriority "
			sql &= "Where VakNr = @vacancyNumber "
			sql &= "AND KDNr = @KDNr; "

			sql &= "Update Vak_JobCHData Set "
			sql &= "IsOnline = @jobCHOnline "
			sql &= "Where VakNr = @vacancyNumber; "

			sql &= "Update Vak_OstJobData Set "
			sql &= "IsOnline = @ostJobOnline "
			sql &= "Where VakNr = @vacancyNumber; "


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("vacancyNumber", ReplaceMissing(vacancyNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(customerNumber, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("ownerOnline", ReplaceMissing(ownerOnline, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("JobChannelPriority", ReplaceMissing(JobChannelPriority, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("jobCHOnline", ReplaceMissing(jobCHOnline, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("ostJobOnline", ReplaceMissing(ostJobOnline, 0)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

			Return success

		End Function

		Function UpdateOtherVacanciesAsOffline(ByVal vacancyNumbers As List(Of Integer)) As Boolean Implements IVacancyDatabaseAccess.UpdateOtherVacanciesAsOffline

			Dim success = True

			Dim sql As String
			Dim vakNumbersBuffer As String = String.Empty

			For Each number In vacancyNumbers

				vakNumbersBuffer = vakNumbersBuffer & IIf(vakNumbersBuffer <> "", ", ", "") & number

			Next

			sql = "Update Vakanzen Set "
			sql &= "IEExport = @ownerOnline "
			sql &= "Where VakNr NOT In ({0}); "

			sql &= "Update Vak_JobCHData Set "
			sql &= "IsOnline = @jobCHOnline "
			sql &= "Where VakNr NOT In ({0}); "

			sql &= "Update Vak_OstJobData Set "
			sql &= "IsOnline = @ostJobOnline "
			sql &= "Where VakNr NOT In ({0}); "

			sql = String.Format(sql, vakNumbersBuffer)


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("ownerOnline", False))
			listOfParams.Add(New SqlClient.SqlParameter("jobCHOnline", False))
			listOfParams.Add(New SqlClient.SqlParameter("ostJobOnline", False))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

			Return success

		End Function

		Function UpdateVacancyJobCHMasterData(ByVal JobCHMasterData As VacancyJobCHMasterData) As Boolean Implements IVacancyDatabaseAccess.UpdateVacancyJobCHMasterData

			Dim success = True

			Dim sql As String

			'sql = "Update dbo.Vak_JobCHData Set "
			'sql &= "Vak_Sprache = @Vak_Sprache "
			'sql &= ",Position_Value = @Position_Value"
			'sql &= ",Position = @Position "
			'sql &= ",IsOnline = @IsOnline "
			'sql &= ",ChangedOn = getDate()"
			'sql &= ",ChangedFrom = @ChangedFrom "
			'sql &= ",FirstTransferDate = @FirstJobsCHTransferDate "

			'sql &= "Where VakNr = @vacancyNumber; "

			'sql &= "Declare @FirstTransferDate Datetime "
			'sql &= "Declare @IsIEOnline Bit "
			'sql &= "Select Top (1) @FirstTransferDate = FirstTransferDate, @IsIEOnline = IEExport From Vakanzen Where Vaknr = @vacancyNumber "
			'sql &= "IF @FirstTransferDate Is Null "
			'sql &= "Begin "
			'sql &= "IF @IEExport = 1 "
			'sql &= "Begin "
			'sql &= "Set @FirstTransferDate = GetDate() "
			'sql &= "End "
			'sql &= "End "

			'sql &= "Update dbo.Vakanzen Set "
			'sql &= "TitelForSearch = @TitelForSearch"
			'sql &= ",ShortDescription = @ShortDescription "
			'sql &= ",Beginn = @Beginn "
			'sql &= ",JobProzent = @JobProzent"
			'sql &= ",Anstellung = @Anstellung"
			'sql &= ",Dauer = @Dauer "
			'sql &= ",MAAge = @MAAge"
			'sql &= ",MASex = @MASex "
			'sql &= ",IEExport = @IEExport "
			'sql &= ",JobChannelPriority = @JobChannelPriority "
			'sql &= ",Vak_Kanton = @Vak_Kanton "
			'sql &= ",FirstTransferDate = @FirstTransferDate "

			'sql &= "Where VakNr = @vacancyNumber; "

			sql = "[Update Vacancy JobsCH Master Data]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("vacancyNumber", ReplaceMissing(JobCHMasterData.VakNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Vak_Sprache", ReplaceMissing(JobCHMasterData.Vak_Sprache, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Position_Value", ReplaceMissing(JobCHMasterData.Position_Value, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Position", ReplaceMissing(JobCHMasterData.Position, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("IsOnline", ReplaceMissing(JobCHMasterData.IsOnline, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("ChangedFrom", ReplaceMissing(JobCHMasterData.ChangedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("TitelForSearch", ReplaceMissing(JobCHMasterData.TitelForSearch, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ShortDescription", ReplaceMissing(JobCHMasterData.ShortDescription, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Beginn", ReplaceMissing(JobCHMasterData.Beginn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("JobProzent", ReplaceMissing(JobCHMasterData.JobProzent, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Anstellung", ReplaceMissing(JobCHMasterData.Anstellung, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Dauer", ReplaceMissing(JobCHMasterData.Dauer, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MAAge", ReplaceMissing(JobCHMasterData.MAAge, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MASex", ReplaceMissing(JobCHMasterData.MASex, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("IEExport", ReplaceMissing(JobCHMasterData.IEExport, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("JobChannelPriority", ReplaceMissing(JobCHMasterData.JobChannelPriority, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Vak_Kanton", ReplaceMissing(JobCHMasterData.Vak_Kanton, DBNull.Value)))

			If JobCHMasterData.IsOnline.GetValueOrDefault(False) Then
				If Not JobCHMasterData.FirstJobsCHTransferDate.HasValue Then JobCHMasterData.FirstJobsCHTransferDate = Now
			End If
			listOfParams.Add(New SqlClient.SqlParameter("FirstJobsCHTransferDate", ReplaceMissing(JobCHMasterData.FirstJobsCHTransferDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ChangedUserNumber", ReplaceMissing(JobCHMasterData.ChangedUserNumber, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Return success

		End Function

		Function UpdateJobCHCustomerData(ByVal JobCHMasterData As VacancyJobCHMasterData) As Boolean Implements IVacancyDatabaseAccess.UpdateJobCHCustomerData

			Dim success = True

			Dim sql As String

			'sql = "Update Vak_JobCHData Set "
			'sql &= "Organisation_ID = @Organisation_ID"
			'sql &= ",Organisation_SubID = @Organisation_SubID"
			'sql &= ",Inserat_ID = @Inserat_ID "
			'sql &= ",Our_URL = @Our_URL"
			'sql &= ",Direkt_URL = @Direkt_URL"
			'sql &= ",Layout_ID = @Layout_ID"
			'sql &= ",Logo_ID = @Logo_ID"
			'sql &= ",Bewerben_URL = @Bewerben_URL "
			'sql &= ",Angebot_Value = @Angebot_Value"
			'sql &= ", Xing_Poster_URL = @Xing_Poster_URL "
			'sql &= ",Xing_Company_Profile_URL = @Xing_Company_Profile_URL "
			'sql &= ",Xing_Company_Is_Poc = @Xing_Company_Is_Poc "
			'sql &= ",StartDate = @StartDate"
			'sql &= ",EndDate = @EndDate "
			'sql &= ",Vak_Sprache = @Vak_Sprache "

			'sql &= "Where VakNr = @vacancyNumber; "

			'sql &= "Update Vakanzen Set "
			'sql &= "UserKontakt = @UserKontakt"
			'sql &= ",UserEMail = @UserEMail "

			'sql &= "Where VakNr = @vacancyNumber; "

			sql = "[Update Vacancy JobsCH Customer Data]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("vacancyNumber", ReplaceMissing(JobCHMasterData.VakNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Organisation_ID", ReplaceMissing(JobCHMasterData.Organisation_ID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Organisation_SubID", ReplaceMissing(JobCHMasterData.Organisation_SubID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Inserat_ID", ReplaceMissing(JobCHMasterData.Inserat_ID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Our_URL", ReplaceMissing(JobCHMasterData.Our_URL, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Direkt_URL", ReplaceMissing(JobCHMasterData.Direkt_URL, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Layout_ID", ReplaceMissing(JobCHMasterData.Layout_ID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Logo_ID", ReplaceMissing(JobCHMasterData.Logo_ID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Bewerben_URL", ReplaceMissing(JobCHMasterData.Bewerben_URL, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Angebot_Value", ReplaceMissing(JobCHMasterData.Angebot_Value, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Xing_Poster_URL", ReplaceMissing(JobCHMasterData.Xing_Poster_URL, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Xing_Company_Profile_URL", ReplaceMissing(JobCHMasterData.Xing_Company_Profile_URL, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Xing_Company_Is_Poc", ReplaceMissing(JobCHMasterData.Xing_Company_Is_Poc, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Vak_Sprache", ReplaceMissing(JobCHMasterData.Vak_Sprache, "de")))
			listOfParams.Add(New SqlClient.SqlParameter("StartDate", ReplaceMissing(JobCHMasterData.StartDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("EndDate", ReplaceMissing(JobCHMasterData.EndDate, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("UserKontakt", ReplaceMissing(JobCHMasterData.UserKontakt, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("UserEMail", ReplaceMissing(JobCHMasterData.UserEMail, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Return success

		End Function


		Function AddDefaultDataIntoJobCHDb(ByVal customerID As String, ByVal createdUserNumber As Integer, ByVal vacancyNumber As Integer) As Boolean Implements IVacancyDatabaseAccess.AddDefaultDataIntoJobCHDb
			Dim success As Boolean = True

			Try

				Dim sql As String

				'sql = "If Not Exists(Select ID From Vak_JobCHData Where VakNr = @vacancyNumber) "
				'sql &= "Insert Into Vak_JobCHData (VakNr, USNr, Vak_Sprache, Organisation_ID, Organisation_SubID, Layout_ID, Logo_ID) "
				'sql &= "Values (@vacancyNumber, @userNumber, 'de', "
				'sql &= "IsNull((Select Top 1 Organisation_ID FROM tblJobAccount Where Organisation_SubID = (IsNull((Select Top 1 JCH_SubID FROM Benutzer Where USNr = @userNumber), 0)) And Customer_Guid = @customerID ), 0), "
				'sql &= "IsNull((Select Top 1 JCH_SubID FROM Benutzer Where USNr = @userNumber), 0), "
				'sql &= "IsNull((Select Top 1 JCH_LayoutID FROM Benutzer Where USNr = @userNumber), 0), IsNull((Select Top 1 JCH_LogoID FROM Benutzer Where USNr = @userNumber), 0)) "

				sql = "[Create Vacancy Default JobsCH Customer Data]"

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("vacancyNumber", ReplaceMissing(vacancyNumber, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("userNumber", ReplaceMissing(createdUserNumber, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("createdUserNumber", ReplaceMissing(createdUserNumber, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				success = False

			End Try

			Return success
		End Function

		Function UpdateJobCHDbFieldValue(ByVal myDbFieldName As String, ByVal formatedText As String, ByVal plainText As String, ByVal vacancyNumber As Integer) As Boolean Implements IVacancyDatabaseAccess.UpdateJobCHDbFieldValue
			Dim success As Boolean = True

			Try
				Dim sql As String
				sql = "Update Vak_JobCHData Set _{0} = @formatedText, {0} = @plainText Where VakNr = @vacancyNumber"
				sql = String.Format(sql, myDbFieldName)


				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("formatedText", ReplaceMissing(formatedText, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("plainText", ReplaceMissing(plainText, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("vacancyNumber", ReplaceMissing(vacancyNumber, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				success = False

			End Try

			Return success
		End Function

		Function UpdateVacancyZusatzDbFieldValue(ByVal myDbFieldName As String, ByVal formatedText As String, ByVal plainText As String, ByVal vacancyNumber As Integer) As Boolean Implements IVacancyDatabaseAccess.UpdateVacancyZusatzDbFieldValue
			Dim success As Boolean = True

			Try
				Dim sql As String
				sql = "Update Vak_ZusatzData Set _{0} = @formatedText, {0} = @plainText Where VakNr = @vacancyNumber"
				sql = String.Format(sql, myDbFieldName)


				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("formatedText", ReplaceMissing(formatedText, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("plainText", ReplaceMissing(plainText, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("vacancyNumber", ReplaceMissing(vacancyNumber, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				success = False

			End Try

			Return success
		End Function

		Function UpdateJobCHOccupationData(ByVal firstOccupation_Value As Integer?, secondOccupation_Value As Integer?,
																			 ByVal firstExperience_Value As Integer?, secondExperience_Value As Integer?,
																			 ByVal firstPosition_Value As Integer?, secondPosition_Value As Integer?,
																			 ByVal firstOccupation_Label As String, ByVal secondOccupation_Label As String,
																			 ByVal firstExperience_Label As String, ByVal secondExperience_Label As String,
																			 ByVal firstPosition_Label As String, ByVal secondPosition_Label As String,
																			 ByVal vacancyNumber As Integer) As Boolean Implements IVacancyDatabaseAccess.UpdateJobCHOccupationData
			Dim success As Boolean = True

			Try

				Dim sql As String

				sql = "Delete Vak_JobCHBerufgruppeData Where VakNr = @vacancyNumber; "

				If firstOccupation_Value.GetValueOrDefault(0) > 0 Then
					sql &= "Insert Into Vak_JobCHBerufgruppeData ("
					sql &= "VakNr"
					sql &= ",BerufGruppe_Value"
					sql &= ",BerufGruppe"
					sql &= ",Fachrichtung_Value"
					sql &= ",Fachrichtung"
					sql &= ",Position_Value"
					sql &= ",Position"
					sql &= ",ForExperience"

					sql &= ") Values ("
					sql &= "@vacancyNumber "
					sql &= ",@firstOccupation_Value"
					sql &= ",@firstOccupation_Label"
					sql &= ",@firstExperience_Value"
					sql &= ",@firstExperience_Label"
					sql &= ",@firstPosition_Value"
					sql &= ",@firstPosition_Label"
					sql &= ",1"
					sql &= "); "
				End If

				If secondOccupation_Value.GetValueOrDefault(0) > 0 Then
					sql &= "Insert Into Vak_JobCHBerufgruppeData ("
					sql &= "VakNr"
					sql &= ",BerufGruppe_Value"
					sql &= ",BerufGruppe"
					sql &= ",Fachrichtung_Value"
					sql &= ",Fachrichtung"
					sql &= ",Position_Value"
					sql &= ",Position"
					sql &= ",ForExperience"

					sql &= ") Values ("
					sql &= "@vacancyNumber "
					sql &= ",@secondOccupation_Value"
					sql &= ",@secondOccupation_Label"
					sql &= ",@secondExperience_Value"
					sql &= ",@secondExperience_Label"
					sql &= ",@secondPosition_Value"
					sql &= ",@secondPosition_Label"
					sql &= ",1"
					sql &= ")"
				End If

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("vacancyNumber", ReplaceMissing(vacancyNumber, DBNull.Value)))

				If firstOccupation_Value.GetValueOrDefault(0) > 0 Then
					listOfParams.Add(New SqlClient.SqlParameter("firstOccupation_Value", ReplaceMissing(firstOccupation_Value, DBNull.Value)))
					listOfParams.Add(New SqlClient.SqlParameter("firstOccupation_Label", ReplaceMissing(firstOccupation_Label, DBNull.Value)))
					listOfParams.Add(New SqlClient.SqlParameter("firstExperience_Value", ReplaceMissing(firstExperience_Value, DBNull.Value)))
					listOfParams.Add(New SqlClient.SqlParameter("firstExperience_Label", ReplaceMissing(firstExperience_Label, DBNull.Value)))
					listOfParams.Add(New SqlClient.SqlParameter("firstPosition_Value", ReplaceMissing(firstPosition_Value, DBNull.Value)))
					listOfParams.Add(New SqlClient.SqlParameter("firstPosition_Label", ReplaceMissing(firstPosition_Label, DBNull.Value)))
				End If
				If secondOccupation_Value.GetValueOrDefault(0) > 0 Then
					listOfParams.Add(New SqlClient.SqlParameter("secondOccupation_Value", ReplaceMissing(secondOccupation_Value, DBNull.Value)))
					listOfParams.Add(New SqlClient.SqlParameter("secondOccupation_Label", ReplaceMissing(secondOccupation_Label, DBNull.Value)))
					listOfParams.Add(New SqlClient.SqlParameter("secondExperience_Value", ReplaceMissing(secondExperience_Value, DBNull.Value)))
					listOfParams.Add(New SqlClient.SqlParameter("secondExperience_Label", ReplaceMissing(secondExperience_Label, DBNull.Value)))
					listOfParams.Add(New SqlClient.SqlParameter("secondPosition_Value", ReplaceMissing(secondPosition_Value, DBNull.Value)))
					listOfParams.Add(New SqlClient.SqlParameter("secondPosition_Label", ReplaceMissing(secondPosition_Label, DBNull.Value)))
				End If


				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				success = False

			End Try

			Return success
		End Function

		Function UpdateJobCHBildungsniveauData(ByVal firstBildung_Value As Integer?, secondBildung_Value As Integer?, thirdBildung_Value As Integer?,
																					 ByVal firstBildung_Label As String, ByVal secondBildung_Label As String, ByVal thirdBildung_Label As String,
																					 ByVal vacancyNumber As Integer) As Boolean Implements IVacancyDatabaseAccess.UpdateJobCHBildungsniveauData
			Dim success As Boolean = True

			Try

				Dim sql As String

				sql = "Delete Vak_JobCHBildungsniveauData Where VakNr = @vacancyNumber; "

				If firstBildung_Value.GetValueOrDefault(0) > 0 Then
					sql &= "Insert Into Vak_JobCHBildungsniveauData (VakNr, "
					sql &= "Bez_Value, Bezeichnung"
					sql &= ") Values ("
					sql &= "@vacancyNumber, @firstBildung_Value, @firstBildung_Label); "
				End If
				If secondBildung_Value.GetValueOrDefault(0) > 0 Then
					sql &= "Insert Into Vak_JobCHBildungsniveauData (VakNr, "
					sql &= "Bez_Value, Bezeichnung"
					sql &= ") Values ("
					sql &= "@vacancyNumber, @secondBildung_Value, @secondBildung_Label) "
				End If
				If thirdBildung_Value.GetValueOrDefault(0) > 0 Then
					sql &= "Insert Into Vak_JobCHBildungsniveauData (VakNr, "
					sql &= "Bez_Value, Bezeichnung"
					sql &= ") Values ("
					sql &= "@vacancyNumber, @thirdBildung_Value, @thirdBildung_Label) "
				End If


				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("vacancyNumber", ReplaceMissing(vacancyNumber, DBNull.Value)))
				If firstBildung_Value.GetValueOrDefault(0) > 0 Then
					listOfParams.Add(New SqlClient.SqlParameter("firstBildung_Value", ReplaceMissing(firstBildung_Value, DBNull.Value)))
					listOfParams.Add(New SqlClient.SqlParameter("firstBildung_Label", ReplaceMissing(firstBildung_Label, DBNull.Value)))
				End If
				If secondBildung_Value.GetValueOrDefault(0) > 0 Then
					listOfParams.Add(New SqlClient.SqlParameter("secondBildung_Value", ReplaceMissing(secondBildung_Value, DBNull.Value)))
					listOfParams.Add(New SqlClient.SqlParameter("secondBildung_Label", ReplaceMissing(secondBildung_Label, DBNull.Value)))
				End If
				If thirdBildung_Value.GetValueOrDefault(0) > 0 Then
					listOfParams.Add(New SqlClient.SqlParameter("thirdBildung_Value", ReplaceMissing(thirdBildung_Value, DBNull.Value)))
					listOfParams.Add(New SqlClient.SqlParameter("thirdBildung_Label", ReplaceMissing(thirdBildung_Label, DBNull.Value)))
				End If


				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				success = False

			End Try

			Return success
		End Function

		Function UpdateJobCHBrunchesData(ByVal bez_Value As Integer?, ByVal bez_Label As String, ByVal vacancyNumber As Integer) As Boolean Implements IVacancyDatabaseAccess.UpdateJobCHBrunchesData
			Dim success As Boolean = True

			Try

				Dim sql As String

				sql = "Delete Vak_JobCHBranchenData Where VakNr = @vacancyNumber; "

				If bez_Value.GetValueOrDefault(0) > 0 Then
					sql &= "Insert Into Vak_JobCHBranchenData (VakNr, "
					sql &= "Bez_Value, Bezeichnung"
					sql &= ") Values ("
					sql &= "@vacancyNumber, @Bez_Value, @Bezeichnung) "
				End If


				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("vacancyNumber", ReplaceMissing(vacancyNumber, DBNull.Value)))
				If bez_Value.GetValueOrDefault(0) > 0 Then
					listOfParams.Add(New SqlClient.SqlParameter("Bez_Value", ReplaceMissing(bez_Value, DBNull.Value)))
					listOfParams.Add(New SqlClient.SqlParameter("Bezeichnung", ReplaceMissing(bez_Label, DBNull.Value)))
				End If

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				success = False

			End Try

			Return success
		End Function

		Function UpdateJobCHRegionData(ByVal firstRegion_Value As Integer?, secondRegion_Value As Integer?,
																					 ByVal firstRegion_Label As String, ByVal secondRegion_Label As String,
																					 ByVal vacancyNumber As Integer) As Boolean Implements IVacancyDatabaseAccess.UpdateJobCHRegionData
			Dim success As Boolean = True

			Try

				Dim sql As String

				sql = "Delete Vak_JobCHRegionData Where VakNr = @vacancyNumber; "

				If firstRegion_Value.GetValueOrDefault(0) > 0 Then
					sql &= "Insert Into Vak_JobCHRegionData (VakNr, "
					sql &= "Bez_Value, Bezeichnung"
					sql &= ") Values ("
					sql &= "@vacancyNumber, @firstRegion_Value, @firstRegion_Label); "
				End If
				If secondRegion_Value.GetValueOrDefault(0) > 0 Then
					sql &= "Insert Into Vak_JobCHRegionData (VakNr, "
					sql &= "Bez_Value, Bezeichnung"
					sql &= ") Values ("
					sql &= "@vacancyNumber, @secondRegion_Value, @secondRegion_Label) "
				End If


				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("vacancyNumber", ReplaceMissing(vacancyNumber, DBNull.Value)))
				If firstRegion_Value.GetValueOrDefault(0) > 0 Then
					listOfParams.Add(New SqlClient.SqlParameter("firstRegion_Value", ReplaceMissing(firstRegion_Value, DBNull.Value)))
					listOfParams.Add(New SqlClient.SqlParameter("firstRegion_Label", ReplaceMissing(firstRegion_Label, DBNull.Value)))
				End If
				If secondRegion_Value.GetValueOrDefault(0) > 0 Then
					listOfParams.Add(New SqlClient.SqlParameter("secondRegion_Value", ReplaceMissing(secondRegion_Value, DBNull.Value)))
					listOfParams.Add(New SqlClient.SqlParameter("secondRegion_Label", ReplaceMissing(secondRegion_Label, DBNull.Value)))
				End If

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				success = False

			End Try

			Return success
		End Function

		Function UpdateVacancyLanguageData(ByVal languageData As VacancyJobCHLanguageData, ByVal jobPlattform As ExternalPlattforms) As Boolean Implements IVacancyDatabaseAccess.UpdateVacancyLanguageData
			Dim success As Boolean = True

			Try

				Dim sql As String

				If languageData.Bezeichnung_Value.GetValueOrDefault(0) = 0 Then Return success

				sql = "Delete Vak_MSprachen Where "
				sql &= "VakNr = @vacancyNumber "
				sql &= "AND Bezeichnung_Value = @Bezeichnung_Value "
				sql &= "AND LanguageNiveau_Value = @LanguageNiveau_Value "
				sql &= "AND AVAM = @AVAM; "

				sql &= "INSERT INTO dbo.Vak_MSprachen "
				sql &= "("
				sql &= "VakNr"
				sql &= ",Bezeichnung"
				sql &= ",LanguageNiveau"
				sql &= ",LanguageNiveau_Value"
				sql &= ",Bezeichnung_Value"
				sql &= ",AVAM"
				sql &= ") "
				sql &= "VALUES "
				sql &= "(@vacancyNumber"
				sql &= ",@Bezeichnung"
				sql &= ",@LanguageNiveau"
				sql &= ",@LanguageNiveau_Value"
				sql &= ",@Bezeichnung_Value"
				sql &= ",@AVAM"
				sql &= ")"


				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("vacancyNumber", ReplaceMissing(languageData.VakNr, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Bezeichnung", ReplaceMissing(languageData.Bezeichnung, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("LanguageNiveau", ReplaceMissing(languageData.LanguageNiveau, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("LanguageNiveau_Value", ReplaceMissing(languageData.LanguageNiveau_Value, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Bezeichnung_Value", ReplaceMissing(languageData.Bezeichnung_Value, DBNull.Value)))
				If jobPlattform = ExternalPlattforms.AVAM Then
					listOfParams.Add(New SqlClient.SqlParameter("AVAM", 1))
				Else
					listOfParams.Add(New SqlClient.SqlParameter("AVAM", DBNull.Value))
				End If

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)



			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				success = False

			End Try

			Return success
		End Function

		Function DuplicateVacancyData(ByVal mdNumber As Integer, ByVal oldVacancyNumber As Integer, ByVal vacancyMasterData As VacancyMasterData) As Boolean Implements IVacancyDatabaseAccess.DuplicateVacancyData
			Dim success As Boolean = True

			Try

				Dim sql As String

				sql = "[Duplicate Vacancy With Exisiting Vacancy Data]"


				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNumber, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("OldVacancyNumber", ReplaceMissing(oldVacancyNumber, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("VacancyNumberOffset", ReplaceMissing(vacancyMasterData.VacancyNumberOffset, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("CreatedFrom", ReplaceMissing(vacancyMasterData.CreatedFrom, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("CreatedUserNumber", ReplaceMissing(vacancyMasterData.CreatedUserNumber, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("NewSBNNumber", ReplaceMissing(vacancyMasterData.SBNNumber, DBNull.Value)))


				Dim newIdParameter = New SqlClient.SqlParameter("IdNewVacancy", SqlDbType.Int)
				newIdParameter.Direction = ParameterDirection.Output
				listOfParams.Add(newIdParameter)

				success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

				If success AndAlso Not newIdParameter.Value Is Nothing Then
					vacancyMasterData.VakNr = CType(newIdParameter.Value, Integer)
				Else
					success = False
				End If


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				success = False

			End Try

			Return success

		End Function

		Function LoadVacancyInseratData(ByVal vacancyNumber As Integer) As VacancyInseratData Implements IVacancyDatabaseAccess.LoadVacancyInseratData
			Dim result As VacancyInseratData = Nothing

			Dim sql As String
			sql = "If Not Exists(Select ID From Vak_ZusatzData Where VakNr = @vacancyNumber) "
			sql &= "Insert Into Vak_ZusatzData (VakNr, V_Zusatz_KDBeschreibung, V_Zusatz_KDBietet, "
			sql &= "V_Zusatz_SBeschreibung, V_Zusatz_Reserve1, V_Zusatz_Taetigkeit, V_Zusatz_Anforderung, "
			sql &= "V_Zusatz_Ausbildung, V_Zusatz_Weiterbildung, V_Zusatz_SKennt, V_Zusatz_EDVKennt) "
			sql &= "Select VakNr, KDBeschreibung, KDBietet, "
			sql &= "SBeschreibung, Reserve1, Taetigkeit, Anforderung, "
			sql &= "Ausbildung, Weiterbildung, SKennt, EDVKennt From Vakanzen Where Vaknr = @vacancyNumber "

			sql &= "Select "
			sql &= "_V_Zusatz_KDBeschreibung "
			sql &= ",_V_Zusatz_KDBietet "
			sql &= ",_V_Zusatz_SBeschreibung "
			sql &= ",_V_Zusatz_Reserve1 "
			sql &= ",_V_Zusatz_Taetigkeit "
			sql &= ",_V_Zusatz_Anforderung "
			sql &= ",_V_Zusatz_Reserve2 "
			sql &= ",_V_Zusatz_Reserve3 "
			sql &= ",_V_Zusatz_Ausbildung "
			sql &= ",_V_Zusatz_Weiterbildung "
			sql &= ",_V_Zusatz_SKennt "
			sql &= ",_V_Zusatz_EDVKennt "
			sql &= "From Vak_ZusatzData "

			sql &= " Where VakNr = @vacancyNumber"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("vacancyNumber", ReplaceMissing(vacancyNumber, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try
				result = New VacancyInseratData
				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result.KDBeschreibung = SafeGetString(reader, "_V_Zusatz_KDBeschreibung")
					result.KDBietet = SafeGetString(reader, "_V_Zusatz_KDBietet")
					result.SBeschreibung = SafeGetString(reader, "_V_Zusatz_SBeschreibung")
					result.Reserve1 = SafeGetString(reader, "_V_Zusatz_Reserve1")
					result.Taetigkeit = SafeGetString(reader, "_V_Zusatz_Taetigkeit")
					result.Anforderung = SafeGetString(reader, "_V_Zusatz_Anforderung")
					result.Reserve2 = SafeGetString(reader, "_V_Zusatz_Reserve2")
					result.Reserve3 = SafeGetString(reader, "_V_Zusatz_Reserve3")
					result.Ausbildung = SafeGetString(reader, "_V_Zusatz_Ausbildung")
					result.Weiterbildung = SafeGetString(reader, "_V_Zusatz_Weiterbildung")
					result.SKennt = SafeGetString(reader, "_V_Zusatz_SKennt")
					result.EDVKennt = SafeGetString(reader, "_V_Zusatz_EDVKennt")

				End If


			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result
		End Function

		Function LoadJobCHBerufData(ByVal vacancyNumber As Integer) As IEnumerable(Of VacancyJobCHBerufData) Implements IVacancyDatabaseAccess.LoadJobCHBerufData
			Dim result As List(Of VacancyJobCHBerufData) = Nothing

			Dim sql As String

			sql = "Select * From [Vak_JobCHBerufgruppeData] "
			sql &= "Where ForExperience = 1 "
			sql &= "And VakNr = @vacancyNumber "
			sql &= "Order By ID "

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("vacancyNumber", ReplaceMissing(vacancyNumber, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of VacancyJobCHBerufData)

					While reader.Read

						Dim data = New VacancyJobCHBerufData

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.VakNr = SafeGetInteger(reader, "VakNr", 0)
						data.BerufGruppe_Value = SafeGetInteger(reader, "BerufGruppe_Value", 0)
						data.BerufGruppe = SafeGetString(reader, "BerufGruppe")
						data.Fachrichtung_Value = SafeGetInteger(reader, "Fachrichtung_Value", 0)
						data.Fachrichtung = SafeGetString(reader, "Fachrichtung")
						data.Position_Value = SafeGetInteger(reader, "Position_Value", 0)
						data.Position = SafeGetString(reader, "Position")
						data.ForExperience = SafeGetBoolean(reader, "ForExperience", False)

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

		Function LoadJobCHBildungsNiveauData(ByVal vacancyNumber As Integer) As IEnumerable(Of VacancyJobCHPeripheryData) Implements IVacancyDatabaseAccess.LoadJobCHBildungsNiveauData
			Dim result As List(Of VacancyJobCHPeripheryData) = Nothing

			Dim sql As String

			sql = "Select * From [Vak_JobCHBildungsNiveauData] "
			sql &= "Where VakNr = @vacancyNumber "
			sql &= "Order By ID "

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("vacancyNumber", ReplaceMissing(vacancyNumber, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of VacancyJobCHPeripheryData)

					While reader.Read

						Dim data = New VacancyJobCHPeripheryData

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.VakNr = SafeGetInteger(reader, "VakNr", 0)
						data.Bez_Value = SafeGetInteger(reader, "Bez_Value", 0)
						data.Bezeichnung = SafeGetString(reader, "Bezeichnung")

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

		Function LoadJobCHRegionData(ByVal vacancyNumber As Integer) As IEnumerable(Of VacancyJobCHPeripheryData) Implements IVacancyDatabaseAccess.LoadJobCHRegionData
			Dim result As List(Of VacancyJobCHPeripheryData) = Nothing

			Dim sql As String

			sql = "Select * From [Vak_JobCHRegionData] "
			sql &= "Where VakNr = @vacancyNumber "
			sql &= "Order By ID "

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("vacancyNumber", ReplaceMissing(vacancyNumber, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of VacancyJobCHPeripheryData)

					While reader.Read

						Dim data = New VacancyJobCHPeripheryData

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.VakNr = SafeGetInteger(reader, "VakNr", 0)
						data.Bez_Value = SafeGetInteger(reader, "Bez_Value", 0)
						data.Bezeichnung = SafeGetString(reader, "Bezeichnung")

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

		Function LoadVacancyLanguageData(ByVal vacancyNumber As Integer, ByVal jobPlattform As ExternalPlattforms) As IEnumerable(Of VacancyJobCHLanguageData) Implements IVacancyDatabaseAccess.LoadVacancyLanguageData
			Dim result As List(Of VacancyJobCHLanguageData) = Nothing

			Dim sql As String

			sql = "Select "
			sql &= "ID"
			sql &= ",VakNr"
			sql &= ",Bezeichnung"
			sql &= ",LanguageNiveau"
			sql &= ",Convert(Int, LanguageNiveau_Value) LanguageNiveau_Value"
			sql &= ",Convert(Int, Bezeichnung_Value) Bezeichnung_Value "
			sql &= ",AVAM "

			sql &= "From Vak_MSprachen "
			sql &= "Where VakNr = @vacancyNumber "
			If jobPlattform = ExternalPlattforms.AVAM Then
				sql &= "AND AVAM = @AVAM "
			Else
				sql &= "AND AVAM Is Null "
			End If

			sql &= "Order By Bezeichnung"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("vacancyNumber", ReplaceMissing(vacancyNumber, DBNull.Value)))
			If jobPlattform = ExternalPlattforms.AVAM Then
				listOfParams.Add(New SqlClient.SqlParameter("AVAM", 1))
			End If


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of VacancyJobCHLanguageData)

					While reader.Read

						Dim data = New VacancyJobCHLanguageData

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.VakNr = SafeGetInteger(reader, "VakNr", 0)
						data.Bezeichnung = SafeGetString(reader, "Bezeichnung")
						data.LanguageNiveau = SafeGetString(reader, "LanguageNiveau")
						data.LanguageNiveau_Value = SafeGetInteger(reader, "LanguageNiveau_Value", 0)
						data.Bezeichnung_Value = SafeGetInteger(reader, "Bezeichnung_Value", 0)

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

		Function LoadJobCHInseratData(ByVal vacancyNumber As Integer) As VacancyInseratJobCHData Implements IVacancyDatabaseAccess.LoadJobCHInseratData
			Dim result As VacancyInseratJobCHData = Nothing

			Dim sql As String

			sql = "Select _J_Zusatz_Jobs_Vorspann, _J_Zusatz_Jobs_Aufgabe, _J_Zusatz_Jobs_Anforderung, _J_Zusatz_Jobs_Wirbieten, Vak.Bezeichnung "
			sql &= "From Vak_JobCHData VJ "
			sql &= "Left Join Vakanzen Vak On VJ.VakNr = Vak.VakNr "
			sql &= "Where VJ.VakNr = @vacancyNumber "


			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("vacancyNumber", ReplaceMissing(vacancyNumber, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try
				result = New VacancyInseratJobCHData
				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result.Bezeichnung = SafeGetString(reader, "Bezeichnung")
					result.Vorspann = SafeGetString(reader, "_J_Zusatz_Jobs_Vorspann")
					result.Aufgabe = SafeGetString(reader, "_J_Zusatz_Jobs_Aufgabe")
					result.Anforderung = SafeGetString(reader, "_J_Zusatz_Jobs_Anforderung")
					result.Wirbieten = SafeGetString(reader, "_J_Zusatz_Jobs_Wirbieten")

				End If


			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result
		End Function


		Function DeleteVacancyData(ByVal vacancyNumber As Integer, ByVal userNumber As Integer) As Boolean Implements IVacancyDatabaseAccess.DeleteVacancyData
			Dim success As Boolean = True

			Try

				Dim sql As String

				sql = "[Delete Selected Vacancy]"


				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("vacancyNumber", ReplaceMissing(vacancyNumber, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("USNr", ReplaceMissing(userNumber, DBNull.Value)))

				Dim resultParameter = New SqlClient.SqlParameter("@Result", SqlDbType.Int)
				resultParameter.Direction = ParameterDirection.Output
				listOfParams.Add(resultParameter)

				success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)
				Dim resultEnum As DeleteVacancyResult

				If Not resultParameter.Value Is Nothing Then
					Try
						resultEnum = CType(resultParameter.Value, DeleteVacancyResult)
					Catch
						resultEnum = DeleteVacancyResult.ErrorWhileDelete
					End Try
				Else
					resultEnum = DeleteVacancyResult.ErrorWhileDelete
				End If
				success = DeleteVacancyResult.Deleted


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				success = False

			End Try

			Return success
		End Function

		Function DeleteJobCHLanguageData(ByVal languageData As VacancyJobCHLanguageData) As Boolean Implements IVacancyDatabaseAccess.DeleteJobCHLanguageData
			Dim success As Boolean = True

			Try

				Dim sql As String

				If languageData.Bezeichnung_Value.GetValueOrDefault(0) = 0 Then Return success
				sql = "DELETE dbo.Vak_MSprachen "
				sql &= "Where VakNr = @vacancyNumber "
				sql &= "And ID = @ID"


				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("vacancyNumber", ReplaceMissing(languageData.VakNr, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("ID", ReplaceMissing(languageData.ID, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)



			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				success = False

			End Try

			Return success
		End Function

		Function AllowedForJobCHTransfer(ByVal customerID As String, ByVal userNumber As Integer) As Boolean Implements IVacancyDatabaseAccess.AllowedForJobCHTransfer
			Dim Result As Boolean = False

			Dim sql As String

			sql = "[Allowed Advisor For VacancyTranfer To JobsCH]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USNr", ReplaceMissing(userNumber, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			Try

				If (Not reader Is Nothing) Then

					Result = reader.HasRows

				End If

			Catch e As Exception
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return Result

		End Function

		Function AllowedForOstJobTransfer(ByVal customerID As String, ByVal userNumber As Integer) As Boolean Implements IVacancyDatabaseAccess.AllowedForOstJobTransfer
			Dim Result As Boolean = False

			Dim sql As String

			sql = "[Allowed Advisor For VacancyTranfer To Ostjob]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USNr", ReplaceMissing(userNumber, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			Try

				If (Not reader Is Nothing) Then

					Result = reader.HasRows

				End If

			Catch e As Exception
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return Result

		End Function

		Function GetJobCHExportedCounterData(ByVal customerID As String, ByVal iVakNr As Integer, ByVal UserKST As String) As JobCHCounterData Implements IVacancyDatabaseAccess.GetJobCHExportedCounterData
			Dim result As New JobCHCounterData
			Dim success = True

			Dim sql As String
			sql = "[Get Count of Exported Vacancy to Jobs_CH]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("Customer_Guid", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@VakNr", iVakNr))
			listOfParams.Add(New SqlClient.SqlParameter("@UserKST", UserKST))


			Dim resultTotalQuantity = New SqlClient.SqlParameter("@TotalRec", SqlDbType.Int)
			resultTotalQuantity.Direction = ParameterDirection.Output
			listOfParams.Add(resultTotalQuantity)

			Dim resultExportedQuantity = New SqlClient.SqlParameter("@AnzExportedRec", SqlDbType.Int)
			resultExportedQuantity.Direction = ParameterDirection.Output
			listOfParams.Add(resultExportedQuantity)

			Dim resultExpireSoonQuantity = New SqlClient.SqlParameter("@ExpireSoon", SqlDbType.Int)
			resultExpireSoonQuantity.Direction = ParameterDirection.Output
			listOfParams.Add(resultExpireSoonQuantity)

			Dim resultCounter = New SqlClient.SqlParameter("@Result", SqlDbType.Int)
			resultCounter.Direction = ParameterDirection.Output
			listOfParams.Add(resultCounter)


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If success Then
				If Not resultTotalQuantity.Value Is Nothing Then
					result.AllowedJobQuantity = resultTotalQuantity.Value
				End If
				If Not resultExportedQuantity.Value Is Nothing Then
					result.ExportedJobQuantity = resultExportedQuantity.Value
				End If
				If Not resultExpireSoonQuantity.Value Is Nothing Then
					result.ExpireSoonJobQuantity = resultExpireSoonQuantity.Value
				End If

				If resultCounter.Value > 0 Then
					result.IsCounterOK = False
				Else
					result.IsCounterOK = True
				End If

			Else
				success = False
			End If


			Return result
		End Function

		Function GetOstJobExportedCounterData(ByVal customerID As String, ByVal iVakNr As Integer, ByVal UserKST As String) As OstJobCounterData Implements IVacancyDatabaseAccess.GetOstJobExportedCounterData
			Dim result As New OstJobCounterData
			Dim success = True

			Dim sql As String

			sql = "[Get Count of Exported Vacancy to OstJob]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("Customer_Guid", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@VakNr", ReplaceMissing(iVakNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("@UserKST", ReplaceMissing(UserKST, 0)))


			Dim resultTotalQuantity = New SqlClient.SqlParameter("@TotalRec", SqlDbType.Int)
			resultTotalQuantity.Direction = ParameterDirection.Output
			listOfParams.Add(resultTotalQuantity)

			Dim resultExportedQuantity = New SqlClient.SqlParameter("@AnzExportedRec", SqlDbType.Int)
			resultExportedQuantity.Direction = ParameterDirection.Output
			listOfParams.Add(resultExportedQuantity)

			Dim resultExpireSoonQuantity = New SqlClient.SqlParameter("@ExpireSoon", SqlDbType.Int)
			resultExpireSoonQuantity.Direction = ParameterDirection.Output
			listOfParams.Add(resultExpireSoonQuantity)

			Dim resultCounter = New SqlClient.SqlParameter("@Result", SqlDbType.Int)
			resultCounter.Direction = ParameterDirection.Output
			listOfParams.Add(resultCounter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If success Then

				If Not resultTotalQuantity.Value Is Nothing Then
					result.AllowedJobQuantity = resultTotalQuantity.Value
				End If
				If Not resultExportedQuantity.Value Is Nothing Then
					result.ExportedJobQuantity = resultExportedQuantity.Value
				End If
				If Not resultExpireSoonQuantity.Value Is Nothing Then
					result.ExpireSoonJobQuantity = resultExpireSoonQuantity.Value 
				End If

				If resultCounter.Value > 0 Then
					result.IsCounterOK = False
				Else
					result.IsCounterOK = True

				End If

			End If


			Return result
		End Function

		Function GetCountOfExportedInternVacancies(ByVal customerID As String) As Integer Implements IVacancyDatabaseAccess.GetCountOfExportedInternVacancies
			Dim result As Integer = 0

			Dim sql As String

			sql = "[Get Count of Exported Vacancy to own Homepage]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("Customer_Guid", ReplaceMissing(customerID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try
				If (Not reader Is Nothing AndAlso reader.Read()) Then
					result = SafeGetInteger(reader, "AnzExportedRec", 0)

				End If


			Catch e As Exception
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		Function LoadJobCHCustomerData(ByVal customerID As String, ByVal userNumber As Integer, ByVal vacancyNumber As Integer, ByVal plattformEnum As ExternalPlattforms) As VacancyJobCHPlattformCustomerData Implements IVacancyDatabaseAccess.LoadJobCHCustomerData
			Dim result As VacancyJobCHPlattformCustomerData = Nothing

			Dim sql As String

			'sql = "If Not Exists(Select Top 1 ID "
			'sql &= "From US_JobPlattforms "
			'sql &= "Where Customer_Guid = @customerID "
			'sql &= "And Jobplattform_Art = @Jobplattform_Art) "
			'sql &= "Insert Into US_JobPlattforms (Customer_Guid, Xing_Company_Is_Poc, Jobplattform_Art) Values "
			'sql &= "(@customerID, 0, @Jobplattform_Art); "

			'sql &= "Select Top 1 * "
			'sql &= "From US_JobPlattforms "
			'sql &= "Where Customer_Guid = @customerID "
			'sql &= "And Jobplattform_Art = @Jobplattform_Art"

			sql = "[Load JobCH Vacancy Customer Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("iVakNr", ReplaceMissing(vacancyNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USNr", ReplaceMissing(userNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Jobplattform_Art", ReplaceMissing(plattformEnum, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				result = New VacancyJobCHPlattformCustomerData
				If (Not reader Is Nothing AndAlso reader.Read()) Then

					Dim data = New VacancyJobCHPlattformCustomerData

					data.ID = SafeGetInteger(reader, "ID", 0)
					data.customerID = SafeGetString(reader, "Customer_Guid")
					data.Jobplattform_Art = SafeGetInteger(reader, "Jobplattform_Art", 0)
					data.Organisation_ID = SafeGetInteger(reader, "Organisation_ID", 0)
					data.Organisation_Kontingent = SafeGetInteger(reader, "Organisation_Kontingent", 0)
					data.DaysToAdd = SafeGetInteger(reader, "DaysToAdd", 0)
					data.Logo_ID = SafeGetInteger(reader, "Logo_ID", 0)
					data.Layout_ID = SafeGetInteger(reader, "Layout_ID", 0)
					data.Vak_Sprache = SafeGetString(reader, "vak_Sprache")

					data.Our_URL = SafeGetString(reader, "Our_URL")
					data.Direkt_URL = SafeGetString(reader, "Direkt_URL")
					data.Xing_Poster_URL = SafeGetString(reader, "Xing_Poster_URL")
					data.Xing_Company_Profile_URL = SafeGetString(reader, "Xing_Company_Profile_URL")
					data.Xing_Company_Is_Poc = SafeGetBoolean(reader, "Xing_Company_Is_Poc", False)
					data.Dirctlink_iframe = SafeGetString(reader, "dirctlink_iframe")
					data.Bewerberform = SafeGetString(reader, "Bewerberform")
					data.Bewerben_URL = SafeGetString(reader, "Bewerben_URL")
					data.StartDate = SafeGetDateTime(reader, "StartDate", Nothing)
					data.EndDate = SafeGetDateTime(reader, "EndDate", Nothing)


					result = data

				End If


			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		Function LoadOstJobCustomerData(ByVal customerID As String, ByVal userNumber As Integer, ByVal vacancyNumber As Integer, ByVal plattformEnum As ExternalPlattforms) As VacancyOstJobPlattformCustomerData Implements IVacancyDatabaseAccess.LoadOstJobCustomerData
			Dim result As VacancyOstJobPlattformCustomerData = Nothing

			Dim sql As String

			'sql = "If Not Exists(Select Top 1 ID "
			'sql &= "From US_JobPlattforms "
			'sql &= "Where Customer_Guid = @customerID "
			'sql &= "And Jobplattform_Art = @Jobplattform_Art) "
			'sql &= "Insert Into US_JobPlattforms (Customer_Guid, Xing_Company_Is_Poc, Jobplattform_Art) Values "
			'sql &= "(@customerID, 0, @Jobplattform_Art); "

			'sql &= "Select Top 1 * "
			'sql &= "From US_JobPlattforms "
			'sql &= "Where Customer_Guid = @customerID "
			'sql &= "And Jobplattform_Art = @Jobplattform_Art"

			sql = "[Load OstJob Vacancy Customer Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("iVakNr", ReplaceMissing(vacancyNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USNr", ReplaceMissing(userNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Jobplattform_Art", ReplaceMissing(plattformEnum, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				result = New VacancyOstJobPlattformCustomerData
				If (Not reader Is Nothing AndAlso reader.Read()) Then

					Dim data = New VacancyOstJobPlattformCustomerData

					data.ID = SafeGetInteger(reader, "ID", 0)
					data.customerID = SafeGetString(reader, "Customer_Guid")
					data.layoutid = SafeGetInteger(reader, "layoutid", 0)
					data.companyhomepage = SafeGetBoolean(reader, "companyhomepage", False)
					data.lehrstelle = SafeGetBoolean(reader, "lehrstelle", False)
					data.DaysToAdd = SafeGetInteger(reader, "DaysToAdd", 14)
					data.ostjob = SafeGetBoolean(reader, "ostjob", True)
					data.zentraljob = SafeGetBoolean(reader, "zentraljob", False)
					data.minisite = SafeGetBoolean(reader, "minisite", False)
					data.nicejob = SafeGetBoolean(reader, "nicejob", False)
					data.westjob = SafeGetBoolean(reader, "westjob", False)
					data.StartDate = SafeGetDateTime(reader, "StartDate", Nothing)
					data.EndDate = SafeGetDateTime(reader, "EndDate", Nothing)


					result = data

				End If


			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		Function UpdateVacancyJobCHAdvisorData(ByVal mdNr As Integer, ByVal vacancyNumber As Integer, ByVal userNumber As Integer, ByVal userData As AdvisorData, ByVal templateData As String) As Boolean Implements IVacancyDatabaseAccess.UpdateVacancyJobCHAdvisorData
			Dim success = True

			Dim sql As String

			Dim strContact As String = templateData
			If String.IsNullOrWhiteSpace(templateData) Then
				strContact = userData.UserFullname

				strContact &= String.Format("{0}{1}<br />", "<br />", userData.UserMDName)
				strContact &= String.Format("{0}{1}<br />", "", userData.UserMDStrasse)
				strContact &= String.Format("{0}{1} {2}<br />", "", userData.UserMDPLZ, userData.UserMDOrt)
				If Not String.IsNullOrWhiteSpace(userData.UserMDTelefon) Then strContact &= String.Format("{0}{1}<br />", "", userData.UserMDTelefon)
				If Not String.IsNullOrWhiteSpace(userData.UserMDTelefax) Then strContact &= String.Format("{0}{1}<br />", "", userData.UserMDTelefax)
				If Not String.IsNullOrWhiteSpace(userData.UserMDeMail) Then strContact &= String.Format("{0}{1}<br />", "", userData.UserMDeMail)
				If Not String.IsNullOrWhiteSpace(userData.UserMDHomepage) Then strContact &= String.Format("{0}{1}", "", userData.UserMDHomepage)
			End If

			sql = "[Update Assinged Vacancy Data With Chagned User Data]"
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("VakNr", ReplaceMissing(vacancyNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USNr", ReplaceMissing(userNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KST", ReplaceMissing(userData.KST, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("EMail", ReplaceMissing(userData.UsereMail, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("UserKontakt", ReplaceMissing(strContact, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success

		End Function

		Function LoadVacancyZusatzMenuInfoData(ByVal customerID As String, ByVal plattformEnum As ExternalPlattforms) As IEnumerable(Of VacancyZusatzMenuData) Implements IVacancyDatabaseAccess.LoadVacancyZusatzMenuInfoData
			Dim result As List(Of VacancyZusatzMenuData) = Nothing


			Dim sql As String
			sql = "Select "
			sql &= "ID"
			sql &= ",RecNr"
			sql &= ",Convert(Int, GroupNr) GroupNr"
			sql &= ",Bezeichnung"
			sql &= ",DBFieldName"
			sql &= ",ShowInMAVersand"
			sql &= ",ShowInProposeNavBar"
			sql &= ",ModulName "
			sql &= "From dbo.tab_LLZusatzFields "
			sql &= "Where ShowinProposeNavBar = 0 "
			sql &= "And ShowInMAVersand = 0 "
			sql &= "And Modulname Like @Modulname "
			sql &= "Order By RecNr, Bezeichnung"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			Select Case plattformEnum
				Case 0
					listOfParams.Add(New SqlClient.SqlParameter("Modulname", "Vak_ZusatzData"))

				Case 1
					listOfParams.Add(New SqlClient.SqlParameter("Modulname", "Vak_Jobs.ch"))

				Case 3
					listOfParams.Add(New SqlClient.SqlParameter("Modulname", "Vak_JobWinner.ch"))

			End Select


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of VacancyZusatzMenuData)

					While reader.Read

						Dim data = New VacancyZusatzMenuData

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.RecNr = SafeGetInteger(reader, "RecNr", 0)
						data.GroupNr = SafeGetInteger(reader, "GroupNr", 0)
						data.Bezeichnung = SafeGetString(reader, "Bezeichnung")
						data.DBFieldName = SafeGetString(reader, "DBFieldName")
						data.ShowInMAVersand = SafeGetBoolean(reader, "ShowInMAVersand", False)
						data.ShowInProposeNavBar = SafeGetBoolean(reader, "ShowInProposeNavBar", False)
						data.ModulName = SafeGetString(reader, "ModulName")

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


	End Class


End Namespace
