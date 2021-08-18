
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SPProgUtility.MainUtilities
Imports SP.DatabaseAccess.Applicant.DataObjects

Namespace DataBaseAccess


	Public Class MainGrid

		Private Shared m_Logger As ILogger = New Logger()

		'Function LoadUserRights(ByVal userNumber As Integer, ByVal mandantNumber As Integer) As IEnumerable(Of UserRights)
		'	Dim result As List(Of UserRights) = Nothing
		'	Dim m_utility As New Utilities

		'	Dim Sql As String = "[Get MandantListing In MainView]"

		'	Dim listOfParams As New List(Of SqlClient.SqlParameter)
		'	listOfParams.Add(New SqlClient.SqlParameter("userNumber", userNumber))
		'	listOfParams.Add(New SqlClient.SqlParameter("mandantNumber", mandantNumber))

		'	Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, Sql, listOfParams)

		'	Try

		'		If (Not reader Is Nothing) Then

		'			result = New List(Of UserRights)

		'			While reader.Read()
		'				Dim overviewData As New UserRights

		'				'overviewData.MDName = m_utility.SafeGetString(reader, "Mandantenname")
		'				'overviewData.MDNr = m_utility.SafeGetInteger(reader, "MDNr", 0)

		'				result.Add(overviewData)

		'			End While

		'		End If

		'	Catch e As Exception
		'		result = Nothing
		'		m_Logger.LogError(e.ToString())

		'	Finally
		'		m_utility.CloseReader(reader)

		'	End Try

		'	Return result
		'End Function


		Public Function IsUserActionAllowed(ByVal iUSNr As Integer, ByVal iFuncName As Integer, ByVal iMDNr As Integer) As Boolean
			Dim m_utility As New Utilities

			Dim iLogedUsnr As Integer = iUSNr
			Dim bResult As Boolean
			Dim Sql As String = "[Get User SecLevel For Selected Moduls]"
			Dim iMyFuncNr As Integer = iFuncName
			If iMyFuncNr = 0 Then
				'm_Logger.LogWarning(String.Format("{0}.Kein Modul wurde ausgewählt..."))
				Return False
			End If

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("LogedUSNr", iUSNr))
			listOfParams.Add(New SqlClient.SqlParameter("FuncSecNr", iFuncName))
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", iMDNr))
			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, Sql, listOfParams, CommandType.StoredProcedure)

			Try

				Try
					reader.Read()
					If reader.HasRows Then
						If Not IsDBNull(reader("IsAllowed")) Then
							bResult = CBool(reader("IsAllowed"))
						Else
							bResult = False
						End If
					End If
					reader.Close()

				Catch ex As Exception
					m_Logger.LogError(String.Format("SecDb lesen: {0}", ex.Message))

				Finally

				End Try

			Catch ex As Exception
				m_Logger.LogError(String.Format("SecDb lesen: {0}", ex.Message))

			Finally
				reader.Close()

			End Try
			Return bResult

		End Function

		Public Function GetLastRegisteredUpdate(ByVal logedUserName As String) As UpdateProperty
			Dim m_utility As New Utilities

			Dim result As New UpdateProperty
			Dim Sql As String = "Select Top 1 * From [Sputnik DbSelect].Dbo.tbl_UpdateViewedProtokoll Where UserName = @LogedUser Order By RecID Desc"
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("LogedUser", logedUserName))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, Sql, listOfParams, CommandType.Text)

			Try
				reader.Read()
				If reader.HasRows Then
					result.RecID = m_utility.SafeGetInteger(reader, "RecID", Nothing)
					result.UpdateFilename = m_utility.SafeGetString(reader, "UpdateFileName")
					result.UpdateFileDate = m_utility.SafeGetDateTime(reader, "UpdateFileDate", Nothing)
					result.UpdateViewed = m_utility.SafeGetDateTime(reader, "UpdateViewed", Nothing)
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("GetLastRegisteredUpdate: {0}", ex.ToString))

			Finally
				reader.Close()

			End Try
			Return result

		End Function

		Public Function UpdateViewedUpdate(ByVal updateID As Integer?) As Boolean
			Dim m_utility As New Utilities

			Dim result As Boolean
			Dim Sql As String = "Insert Into [Sputnik DbSelect].Dbo.tbl_UpdateViewedProtokoll (RecID, Username, UpdateViewed) Values (@RecID, @Username, @UpdateViewed)"
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("RecID", ReplaceMissing(updateID, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Username", ModulConstants.UserData.UserLoginname))
			listOfParams.Add(New SqlClient.SqlParameter("UpdateViewed", Now))


			Try
				result = m_utility.ExecuteNonQuery(ModulConstants.MDData.MDDbConn, Sql, listOfParams, CommandType.Text)

			Catch ex As Exception
				m_Logger.LogError(String.Format("UpdateViewedUpdate: {0}", ex.ToString))

			End Try
			Return result

		End Function

		Function LoadAktivMandanten() As IEnumerable(Of MDData)
			Dim result As List(Of MDData) = Nothing
			Dim m_utility As New Utilities

			Dim Sql As String = "[Get MandantListing In MainView]"


			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, Sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of MDData)

					While reader.Read()
						Dim overviewData As New MDData

						overviewData.MDName = m_utility.SafeGetString(reader, "Mandantenname")
						overviewData.MDNr = m_utility.SafeGetInteger(reader, "MDNr", 0)

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function

		Public Function LoadUserImageData(ByVal USNr As Integer) As UserImageData
			Dim m_utility As New Utilities

			Dim result As UserImageData = Nothing

			Dim sql As String

			sql = "SELECT USNr, USBild FROM Benutzer WHERE USNr = @USNr"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("USNr", USNr))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					If reader.Read Then
						result = New UserImageData
						result.UsrNr = m_utility.SafeGetInteger(reader, "USNr", 0)
						result.UserImage = m_utility.SafeGetByteArray(reader, "USBild")
					End If
				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)
			End Try

			Return result

		End Function

		Function GetDbTODOData4Show(ByVal sql As String, ByVal advisornumber As Integer?) As IEnumerable(Of FoundedTODOData)
			Dim result As List(Of FoundedTODOData) = Nothing
			Dim m_utility As New Utilities


			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("userNr", advisornumber))
			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedTODOData)
					'	Select Case TD.ID,
					'   TD.MANr,
					'TD.KDNr,
					'TD.KDZHDNr,
					'TD.VakNr,
					'TD.ProposeNr,
					'TD.ESNr,
					'TD.RPNr,
					'TD.LMNr,
					'TD.RENr,
					'TD.ZENr,
					'TD.Subject,
					'TD.Body,
					'TD.Important,
					'TD.Schedulebegins,
					'TD.Scheduleends,
					'TD.ScheduleRememberIn,
					'TD.scheduleRemember,
					'TD.AllUsers,
					'TD.SourceInput,
					'(SELECT TOP 1 t1.USNr FROM #td1 t1 WHERE t1.ID = TD.ID) USNr,
					'   (SELECT TOP 1 TU.Done FROM dbo.tbl_TODO_User TU WHERE TU.FK_ToDoID = TD.ID) Done


					While reader.Read()
						Dim overviewData As New FoundedTODOData

						overviewData.id = CInt(m_utility.SafeGetInteger(reader, "id", 0))
						overviewData.recnr = 0

						overviewData.manr = m_utility.SafeGetInteger(reader, "manr", 0)
						overviewData.lmnr = m_utility.SafeGetInteger(reader, "lmnr", 0)
						overviewData.kdnr = m_utility.SafeGetInteger(reader, "kdnr", 0)
						overviewData.zhdnr = m_utility.SafeGetInteger(reader, "kdzhdnr", 0)
						overviewData.vaknr = m_utility.SafeGetInteger(reader, "vaknr", 0)
						overviewData.proposenr = m_utility.SafeGetInteger(reader, "proposenr", 0)
						overviewData.esnr = m_utility.SafeGetInteger(reader, "esnr", 0)
						'overviewData.mdnr = m_utility.SafeGetInteger(reader, "mdnr", 0)
						overviewData.usnr = m_utility.SafeGetInteger(reader, "usnr", 0)
						overviewData.rpnr = m_utility.SafeGetInteger(reader, "rpnr", 0)
						overviewData.renr = m_utility.SafeGetInteger(reader, "renr", 0)
						overviewData.zenr = m_utility.SafeGetInteger(reader, "zenr", 0)

						overviewData.EmployeeLastname = m_utility.SafeGetString(reader, "EmployeeLastname")
						overviewData.EmployeeFirstname = m_utility.SafeGetString(reader, "EmployeeFirstname")
						overviewData.customername = m_utility.SafeGetString(reader, "Customername")
						overviewData.ZLastname = m_utility.SafeGetString(reader, "ZLastname")
						overviewData.ZFirstname = m_utility.SafeGetString(reader, "ZFirstname")
						overviewData.ProposeLabel = m_utility.SafeGetString(reader, "ProposeLabel")

						overviewData.done = m_utility.SafeGetBoolean(reader, "done", Nothing)
						overviewData.importand = m_utility.SafeGetBoolean(reader, "important", Nothing)

						overviewData.subject = m_utility.SafeGetString(reader, "subject")
						overviewData.body = m_utility.SafeGetString(reader, "body")

						overviewData.schedulebegins = m_utility.SafeGetDateTime(reader, "schedulebegins", Nothing)
						overviewData.scheduleends = m_utility.SafeGetDateTime(reader, "scheduleends", Nothing)
						overviewData.schedulerememberin = m_utility.SafeGetDateTime(reader, "schedulerememberin", Nothing)
						overviewData.scheduleremember = m_utility.SafeGetDateTime(reader, "scheduleremember", Nothing)

						overviewData.es_als = String.Empty ' m_utility.SafeGetString(reader, "es_als")
						overviewData.es_ab = Nothing ' m_utility.SafeGetDateTime(reader, "es_ab", Nothing)
						overviewData.es_ende = Nothing ' m_utility.SafeGetDateTime(reader, "es_ende", Nothing)

						overviewData.advisor = String.Empty ' m_utility.SafeGetString(reader, "usname")

						overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")


						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function


		Function GetDbEmployeeData4Show(ByVal sql As String, ByVal showBackupHistoryData As Boolean) As IEnumerable(Of FoundedEmployeeData)
			Dim result As List(Of FoundedEmployeeData) = Nothing
			Dim m_utility As New Utilities

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", ModulConstants.MDData.MDNr))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_2", String.Empty))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_3", 0))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_4", ModulConstants.UserData.UserFiliale))
			listOfParams.Add(New SqlClient.SqlParameter("showBackupHistoryData", ReplaceMissing(showBackupHistoryData, 0)))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedEmployeeData)

					While reader.Read()
						Dim overviewData As New FoundedEmployeeData

						overviewData.mdnr = CInt(m_utility.SafeGetInteger(reader, "MDNr", 0))
						overviewData.manr = CInt(m_utility.SafeGetInteger(reader, "MANr", 0))
						overviewData._res = m_utility.SafeGetString(reader, "0")

						overviewData.telefon_p = m_utility.SafeGetString(reader, "telefon_p")

						overviewData.natel = m_utility.SafeGetString(reader, "natel")
						overviewData.maname = m_utility.SafeGetString(reader, "maname")
						overviewData.strasse = m_utility.SafeGetString(reader, "strasse")
						overviewData.maplzort = m_utility.SafeGetString(reader, "maplzort")
						overviewData.magebdat = m_utility.SafeGetDateTime(reader, "magebdat", Nothing)
						overviewData.beruf = m_utility.SafeGetString(reader, "Beruf")
						overviewData.maalterwithdate = m_utility.SafeGetString(reader, "maalterwithdate")
						overviewData.mabewilligung = m_utility.SafeGetString(reader, "mabewilligung")
						overviewData.mastatus_1 = m_utility.SafeGetString(reader, "mastatus_1")
						overviewData.mastatus_2 = m_utility.SafeGetString(reader, "mastatus_2")

						overviewData.maqualifikation = m_utility.SafeGetString(reader, "maqualifikation")
						overviewData.maemail = m_utility.SafeGetString(reader, "maemail")
						overviewData.tempmabild = m_utility.SafeGetString(reader, "tempmabild")

						overviewData.actives = m_utility.SafeGetBoolean(reader, "actives", Nothing)
						overviewData.noes = m_utility.SafeGetBoolean(reader, "noes", Nothing)
						overviewData.webexport = m_utility.SafeGetBoolean(reader, "WebExport", Nothing)
						overviewData.zfiliale = m_utility.SafeGetString(reader, "zfiliale")
						overviewData.beraterin = m_utility.SafeGetString(reader, "beraterin")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function

		Function GetDbCustomerData4Show(ByVal sql As String) As IEnumerable(Of FoundedCustomerData)
			Dim result As List(Of FoundedCustomerData) = Nothing
			Dim m_utility As New Utilities

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", ModulConstants.MDData.MDNr))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_2", String.Empty))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_3", 0))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_4", ModulConstants.UserData.UserFiliale))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedCustomerData)

					While reader.Read()
						Dim overviewData As New FoundedCustomerData

						overviewData.kdnr = CInt(m_utility.SafeGetInteger(reader, "kdnr", 0))
						overviewData.mdnr = CInt(m_utility.SafeGetInteger(reader, "mdnr", 0))
						overviewData._res = m_utility.SafeGetString(reader, "0")

						overviewData.firma1 = m_utility.SafeGetString(reader, "firma1")

						overviewData.kdzhdnr = m_utility.SafeGetString(reader, "kdzhdnr")
						overviewData.kdzname = m_utility.SafeGetString(reader, "kdzname")
						overviewData.strasse = m_utility.SafeGetString(reader, "strasse")
						overviewData.kdplzort = m_utility.SafeGetString(reader, "kdplzort")

						overviewData.fproperty = m_utility.SafeGetInteger(reader, "fproperty", 0)

						overviewData.howkontakt = m_utility.SafeGetString(reader, "howkontakt")
						overviewData.kdstate1 = m_utility.SafeGetString(reader, "kdstate1")
						overviewData.kdstate2 = m_utility.SafeGetString(reader, "kdstate2")

						overviewData.kreditlimiteab = m_utility.SafeGetDateTime(reader, "kreditlimiteab", Nothing)
						overviewData.kreditlimitebis = m_utility.SafeGetDateTime(reader, "kreditlimitebis", Nothing)

						overviewData.kreditlimite = m_utility.SafeGetString(reader, "kreditlimite")
						overviewData.kreditlimite_2 = m_utility.SafeGetString(reader, "kreditlimite_2")
						overviewData.nachname = m_utility.SafeGetString(reader, "nachname")
						overviewData.vorname = m_utility.SafeGetString(reader, "vorname")

						overviewData.kdtelefon = m_utility.SafeGetString(reader, "kdtelefon")
						overviewData.kdtelefax = m_utility.SafeGetString(reader, "kdtelefax")
						overviewData.kdemail = m_utility.SafeGetString(reader, "kdemail")

						overviewData.ztelefon = m_utility.SafeGetString(reader, "ztelefon")
						overviewData.ztelefax = m_utility.SafeGetString(reader, "ztelefax")
						overviewData.zemail = m_utility.SafeGetString(reader, "zemail")
						overviewData.znatel = m_utility.SafeGetString(reader, "znatel")

						overviewData.kdberater = m_utility.SafeGetString(reader, "kdberater")

						overviewData.actives = m_utility.SafeGetBoolean(reader, "actives", Nothing)
						overviewData.NOES = m_utility.SafeGetBoolean(reader, "NOES", Nothing)
						overviewData.zfiliale = m_utility.SafeGetString(reader, "zfiliale")
						overviewData.beraterin = m_utility.SafeGetString(reader, "beraterin")

						overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				m_Logger.LogError(result(result.Count - 1).firma1 & ": " & e.ToString())
				result = Nothing

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function

		Function GetDbVacancyData4Show(ByVal sql As String) As IEnumerable(Of FoundedVacancyData)
			Dim result As List(Of FoundedVacancyData) = Nothing
			Dim m_utility As New Utilities

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", ModulConstants.MDData.MDNr))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_2", String.Empty))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_3", 0))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_4", ModulConstants.UserData.UserFiliale))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedVacancyData)

					While reader.Read()
						Dim overviewData As New FoundedVacancyData
						overviewData._res = m_utility.SafeGetString(reader, "0")

						overviewData.Customer_ID = m_utility.SafeGetString(reader, "Customer_ID")
						overviewData.mdnr = CInt(m_utility.SafeGetInteger(reader, "mdnr", 0))
						overviewData.vaknr = CInt(m_utility.SafeGetInteger(reader, "vaknr", 0))
						overviewData.kdnr = CInt(m_utility.SafeGetInteger(reader, "kdnr", 0))
						overviewData.kdzhdnr = m_utility.SafeGetInteger(reader, "kdzhdnr", 0)

						overviewData.firma1 = m_utility.SafeGetString(reader, "firma1")
						overviewData.bezeichnung = m_utility.SafeGetString(reader, "bezeichnung")
						overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")
						overviewData.changedon = m_utility.SafeGetDateTime(reader, "changedon", Nothing)
						overviewData.changedfrom = m_utility.SafeGetString(reader, "changedfrom")

						overviewData.kdzname = m_utility.SafeGetString(reader, "kdzname")
						overviewData.advisor = m_utility.SafeGetString(reader, "BeraterIn")

						overviewData.kdemail = m_utility.SafeGetString(reader, "kdemail")
						overviewData.zemail = m_utility.SafeGetString(reader, "zemail")

						overviewData.vakstate = m_utility.SafeGetString(reader, "Vakstate")
						overviewData.vak_kanton = m_utility.SafeGetString(reader, "Vak_kanton")

						overviewData.vaklink = m_utility.SafeGetString(reader, "VakLink")

						overviewData.vakkontakt = m_utility.SafeGetString(reader, "vakkontakt")
						overviewData.vacancygruppe = m_utility.SafeGetString(reader, "vacancygruppe")

						overviewData.vacancyplz = m_utility.SafeGetString(reader, "vacancyplz")
						overviewData.vacancyort = m_utility.SafeGetString(reader, "vacancyort")

						overviewData.titelforsearch = m_utility.SafeGetString(reader, "titelforsearch")
						overviewData.shortdescription = m_utility.SafeGetString(reader, "shortdescription")

						overviewData.kdtelefon = m_utility.SafeGetString(reader, "kdtelefon")
						overviewData.kdtelefax = m_utility.SafeGetString(reader, "kdtelefax")

						overviewData.ztelefon = m_utility.SafeGetString(reader, "ztelefon")
						overviewData.ztelefax = m_utility.SafeGetString(reader, "ztelefax")
						overviewData.znatel = m_utility.SafeGetString(reader, "znatel")

						overviewData.FirstTransferDate = m_utility.SafeGetDateTime(reader, "FirstTransferDate", Nothing)
						overviewData.ourisonline = m_utility.SafeGetBoolean(reader, "ourisonline", False)
						overviewData.jchisonline = m_utility.SafeGetBoolean(reader, "jchisonline", False)
						overviewData.ojisonline = m_utility.SafeGetBoolean(reader, "ojisonline", False)
						overviewData.JobsCHExpire = m_utility.SafeGetBoolean(reader, "JobsCHExpire", False)
						overviewData.OstJobExpire = m_utility.SafeGetBoolean(reader, "OstJobExpire", False)

						overviewData.jobchdate = m_utility.SafeGetString(reader, "jobchdate")
						overviewData.ostjobchdate = m_utility.SafeGetString(reader, "ostjobchdate")

						overviewData.AVAMRecordState = m_utility.SafeGetString(reader, "AVAMRecordState")

						overviewData.AVAMJobroomID = m_utility.SafeGetString(reader, "AVAMJobroomID")
						overviewData.AVAMFrom = m_utility.SafeGetDateTime(reader, "AVAMFrom", Nothing)
						overviewData.AVAMUntil = m_utility.SafeGetDateTime(reader, "AVAMUntil", Nothing)
						overviewData.AVAMReportingDate = m_utility.SafeGetDateTime(reader, "AVAMReportingDate", Nothing)
						overviewData.AVAMReportingObligationEndDate = m_utility.SafeGetDateTime(reader, "AVAMReportingObligationEndDate", Nothing)
						overviewData.AVAMReportingObligation = m_utility.SafeGetBoolean(reader, "AVAMReportingObligation", False)

						overviewData.zfiliale = m_utility.SafeGetString(reader, "zfiliale")

						Dim jobChannel = m_utility.SafeGetBoolean(reader, "jobchannelpriority", False)
						If jobChannel Then
							overviewData.jobchannelpriority = FoundedVacancyData.JobplattformEnum.ONLINE
						Else
							overviewData.jobchannelpriority = FoundedVacancyData.JobplattformEnum.OFFLINE
						End If


						If overviewData.jchisonline AndAlso Not overviewData.JobsCHExpire Then
							overviewData.JobsCHWillbeExpireSoon = FoundedVacancyData.JobplattformEnum.ONLINE
						ElseIf overviewData.jchisonline AndAlso overviewData.JobsCHExpire Then
							overviewData.JobsCHWillbeExpireSoon = FoundedVacancyData.JobplattformEnum.EXPIRING

						Else
							overviewData.JobsCHWillbeExpireSoon = FoundedVacancyData.JobplattformEnum.OFFLINE
						End If

						If overviewData.ojisonline AndAlso Not overviewData.OstJobExpire Then
							overviewData.OstJobWillbeExpireSoon = FoundedVacancyData.JobplattformEnum.ONLINE
						ElseIf overviewData.ojisonline AndAlso overviewData.OstJobExpire Then
							overviewData.OstJobWillbeExpireSoon = FoundedVacancyData.JobplattformEnum.EXPIRING

						Else
							overviewData.OstJobWillbeExpireSoon = FoundedVacancyData.JobplattformEnum.OFFLINE
						End If



						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function

		Function LoadJobplattformData() As IEnumerable(Of JobPlattformsInfoData)
			Dim result As List(Of JobPlattformsInfoData) = Nothing
			Dim m_utility As New Utilities

			Dim sql As String

			sql = "[Load Jobplattform Data Grouped By Mandant]"


			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of JobPlattformsInfoData)

					While reader.Read()
						Dim overviewData As New JobPlattformsInfoData

						overviewData.Customer_ID = m_utility.SafeGetString(reader, "Customer_ID")
						overviewData.MD_Name1 = m_utility.SafeGetString(reader, "MD_Name1")
						overviewData.JobplattformLabel = m_utility.SafeGetString(reader, "JobPlattformLabel")

						overviewData.TotalAllowedJobsSlot = m_utility.SafeGetInteger(reader, "Slot", 0)
						overviewData.TranferedJobs = m_utility.SafeGetInteger(reader, "ExportedJobs", 0)
						overviewData.TotalSoonExpireJobs = m_utility.SafeGetInteger(reader, "JobsExpire", 0)


						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function

		Function GetDbProposeData4Show(ByVal sql As String) As IEnumerable(Of FoundedProposeData)
			Dim result As List(Of FoundedProposeData) = Nothing
			Dim m_utility As New Utilities

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", ModulConstants.MDData.MDNr))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_2", String.Empty))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_3", 0))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_4", ModulConstants.UserData.UserFiliale))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedProposeData)

					While reader.Read()
						Dim overviewData As New FoundedProposeData
						overviewData._res = m_utility.SafeGetString(reader, "0")

						overviewData.mdnr = CInt(m_utility.SafeGetInteger(reader, "mdnr", 0))
						overviewData.pnr = CInt(m_utility.SafeGetInteger(reader, "pnr", 0))
						overviewData.manr = CInt(m_utility.SafeGetInteger(reader, "manr", 0))
						overviewData.kdnr = CInt(m_utility.SafeGetInteger(reader, "kdnr", 0))
						overviewData.zhdnr = m_utility.SafeGetInteger(reader, "kdzhdnr", 0)
						overviewData.vaknr = CInt(m_utility.SafeGetInteger(reader, "vaknr", 0))

						overviewData.honorar = CInt(m_utility.SafeGetDecimal(reader, "ab_hBetrag", 0))

						overviewData.part = m_utility.SafeGetString(reader, "p_art")
						overviewData.panstellung = m_utility.SafeGetString(reader, "p_anstellung")
						overviewData.advisor = m_utility.SafeGetString(reader, "BeraterIn")

						overviewData.firma1 = m_utility.SafeGetString(reader, "firma1")
						overviewData.zname = m_utility.SafeGetString(reader, "kdzname")

						overviewData.maname = m_utility.SafeGetString(reader, "maname")
						overviewData.bezeichnung = m_utility.SafeGetString(reader, "bezeichnung")
						overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")
						overviewData.pstate = m_utility.SafeGetString(reader, "pstate")

						overviewData.vakals = m_utility.SafeGetString(reader, "vakals")
						overviewData.vakcreatedon = m_utility.SafeGetDateTime(reader, "vakcreatedon", Nothing)

						overviewData.kdemail = m_utility.SafeGetString(reader, "kdemail")
						overviewData.zemail = m_utility.SafeGetString(reader, "zemail")

						overviewData.ourisonline = m_utility.SafeGetBoolean(reader, "ourisonline", Nothing)
						overviewData.jchisonline = m_utility.SafeGetBoolean(reader, "jchisonline", Nothing)
						overviewData.ojisonline = m_utility.SafeGetBoolean(reader, "ojisonline", Nothing)

						overviewData.kdtelefon = m_utility.SafeGetString(reader, "kdtelefon")
						overviewData.kdtelefax = m_utility.SafeGetString(reader, "kdtelefax")

						overviewData.ztelefon = m_utility.SafeGetString(reader, "ztelefon")
						overviewData.ztelefax = m_utility.SafeGetString(reader, "ztelefax")
						overviewData.znatel = m_utility.SafeGetString(reader, "znatel")

						overviewData.zfiliale = m_utility.SafeGetString(reader, "zfiliale")


						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function

		Function GetDbOfferData4Show(ByVal sql As String) As IEnumerable(Of FoundedOfferData)
			Dim result As List(Of FoundedOfferData) = Nothing
			Dim m_utility As New Utilities

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", ModulConstants.MDData.MDNr))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_2", String.Empty))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_3", 0))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_4", ModulConstants.UserData.UserFiliale))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedOfferData)

					While reader.Read()
						Dim overviewData As New FoundedOfferData

						overviewData.mdnr = m_utility.SafeGetInteger(reader, "mdnr", 0)
						overviewData._res = m_utility.SafeGetString(reader, "0")

						overviewData.ofnr = m_utility.SafeGetInteger(reader, "ofnr", 0)

						overviewData.kdnr = m_utility.SafeGetInteger(reader, "kdnr", Nothing)
						overviewData.zhdnr = m_utility.SafeGetInteger(reader, "kdzhdnr", Nothing)

						overviewData.manr = m_utility.SafeGetInteger(reader, "manr", Nothing)
						overviewData.employeename = m_utility.SafeGetString(reader, "maname")
						overviewData.bezeichnung = m_utility.SafeGetString(reader, "bezeichnung")

						overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")
						overviewData.offerstate = m_utility.SafeGetString(reader, "offerstate")

						overviewData.customername = m_utility.SafeGetString(reader, "firma1")
						overviewData.customerstreet = m_utility.SafeGetString(reader, "kdstrasse")
						overviewData.customeraddress = m_utility.SafeGetString(reader, "kdadresse")
						overviewData.customeremail = m_utility.SafeGetString(reader, "kdemail")
						overviewData.customertelefon = m_utility.SafeGetString(reader, "kdTelefon")
						overviewData.customertelefax = m_utility.SafeGetString(reader, "kdTelefax")

						overviewData.zname = m_utility.SafeGetString(reader, "kdzname")
						overviewData.ztelefon = m_utility.SafeGetString(reader, "zhdTelefon")
						overviewData.zmobile = m_utility.SafeGetString(reader, "zhdNatel")
						overviewData.zemail = m_utility.SafeGetString(reader, "zhdemail")

						overviewData.advisor = m_utility.SafeGetString(reader, "Beraterin")

						overviewData.zfiliale = m_utility.SafeGetString(reader, "zfiliale")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function

		Function LoadApplicationData(ByVal sql As String) As IEnumerable(Of MainViewApplicationData)
			Dim result As List(Of MainViewApplicationData) = Nothing
			Dim m_utility As New Utilities

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", ModulConstants.MDData.MDNr))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_2", String.Empty))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_3", 0))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_4", ModulConstants.UserData.UserFiliale))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of MainViewApplicationData)

					While reader.Read()
						Dim overviewData As New MainViewApplicationData

						overviewData.MDNr = m_utility.SafeGetInteger(reader, "MDNr", 0)
						overviewData.ID = m_utility.SafeGetInteger(reader, "id", 0)
						overviewData.BusinessBranch = m_utility.SafeGetString(reader, "BusinessBranch")
						overviewData.Advisor = m_utility.SafeGetString(reader, "Advisor")

						overviewData.EmployeeID = m_utility.SafeGetInteger(reader, "EmployeeID", 0)
						overviewData.VacancyNumber = m_utility.SafeGetInteger(reader, "Vacancynumber", 0)
						overviewData.Customernumber = m_utility.SafeGetInteger(reader, "Customernumber", 0)
						overviewData.ApplicationAdvisorLastName = m_utility.SafeGetString(reader, "ApplicationAdvisorLastName")
						overviewData.ApplicationAdvisorFirstName = m_utility.SafeGetString(reader, "ApplicationAdvisorFirstName")

						overviewData.ApplicantCountry = m_utility.SafeGetString(reader, "ApplicantCountry")

						overviewData.ApplicantFirstname = m_utility.SafeGetString(reader, "ApplicantFirstname")
						overviewData.ApplicantLastname = m_utility.SafeGetString(reader, "ApplicantLastname")
						overviewData.ApplicantLocation = m_utility.SafeGetString(reader, "ApplicantLocation")
						overviewData.ApplicantPostcode = m_utility.SafeGetString(reader, "ApplicantPostcode")
						overviewData.ApplicantStreet = m_utility.SafeGetString(reader, "ApplicantStreet")
						overviewData.Birthdate = m_utility.SafeGetDateTime(reader, "Birthday", Nothing)

						overviewData.ApplicationLabel = m_utility.SafeGetString(reader, "ApplicationLabel")
						overviewData.ApplicationLifecycle = m_utility.SafeGetInteger(reader, "ApplicationLifecycle", 0)
						overviewData.Availability = m_utility.SafeGetString(reader, "Availability")
						overviewData.zfiliale = m_utility.SafeGetString(reader, "BusinessBranch")
						overviewData.Comment = m_utility.SafeGetString(reader, "Comment")

						overviewData.CheckedFrom = m_utility.SafeGetString(reader, "Checkedfrom")
						overviewData.CheckedOn = m_utility.SafeGetDateTime(reader, "Checkedon", Nothing)
						overviewData.CreatedFrom = m_utility.SafeGetString(reader, "Createdfrom")
						overviewData.CreatedOn = m_utility.SafeGetDateTime(reader, "Createdon", Nothing)

						overviewData.CustomerCountry = m_utility.SafeGetString(reader, "CustomerCountry")
						overviewData.CustomerLocation = m_utility.SafeGetString(reader, "CustomerLocation")
						overviewData.Customername = m_utility.SafeGetString(reader, "Customername")

						overviewData.CustomerPostcode = m_utility.SafeGetString(reader, "CustomerPostcode")
						overviewData.CustomerStreet = m_utility.SafeGetString(reader, "CustomerStreet")
						overviewData.Dismissalperiod = m_utility.SafeGetString(reader, "Dismissalperiod")
						overviewData.zfiliale = m_utility.SafeGetString(reader, "zfiliale")
						overviewData.ShowAsApplicant = m_utility.SafeGetInteger(reader, "ShowAsApplicant", Nothing)

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function

		Function GetDbESData4Show(ByVal sql As String) As IEnumerable(Of FoundedESData)
			Dim result As List(Of FoundedESData) = Nothing
			Dim m_utility As New Utilities

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", ModulConstants.MDData.MDNr))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_2", String.Empty))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_3", 0))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_4", ModulConstants.UserData.UserFiliale))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedESData)

					While reader.Read()
						Dim overviewData As New FoundedESData

						overviewData.mdnr = CInt(m_utility.SafeGetInteger(reader, "mdnr", 0))
						overviewData.gavnumber = CInt(m_utility.SafeGetInteger(reader, "gavnumber", 0))
						overviewData._res = m_utility.SafeGetString(reader, "0")
						overviewData.esnr = CInt(m_utility.SafeGetInteger(reader, "esnr", 0))
						overviewData.manr = CInt(m_utility.SafeGetInteger(reader, "manr", 0))
						overviewData.kdnr = CInt(m_utility.SafeGetInteger(reader, "kdnr", 0))
						overviewData.zhdnr = CInt(m_utility.SafeGetInteger(reader, "kdzhdnr", 0))
						overviewData.vaknr = CInt(m_utility.SafeGetInteger(reader, "vaknr", 0))
						overviewData.proposenr = CInt(m_utility.SafeGetInteger(reader, "proposenr", 0))

						overviewData.eskst1 = m_utility.SafeGetString(reader, "eskst1")
						overviewData.eskst2 = m_utility.SafeGetString(reader, "eskst2")
						overviewData.eskst = m_utility.SafeGetString(reader, "eskst")
						overviewData.employeeadvisor = m_utility.SafeGetString(reader, "employeeadvisor")
						overviewData.employeebusinessbranch = m_utility.SafeGetString(reader, "mafiliale")
						overviewData.customeradvisor = m_utility.SafeGetString(reader, "customeradvisor")
						overviewData.customerbusinessbranch = m_utility.SafeGetString(reader, "kdfiliale")
						If overviewData.employeeadvisor = overviewData.customeradvisor Then
							overviewData.esadvisor = String.Format("{0}", overviewData.employeeadvisor)
						Else
							overviewData.esadvisor = String.Format("{0} - {1}", overviewData.employeeadvisor, overviewData.customeradvisor)
						End If

						overviewData.es_als = m_utility.SafeGetString(reader, "es_als")
						overviewData.es_ab = m_utility.SafeGetDateTime(reader, "es_ab", Nothing)
						overviewData.es_ende = m_utility.SafeGetDateTime(reader, "es_ende", Nothing)
						overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")

						overviewData.lobis = m_utility.SafeGetDateTime(reader, "lobis", Nothing)
						overviewData.lovon = m_utility.SafeGetDateTime(reader, "lovon", Nothing)
						overviewData.gavstate = True

						overviewData.tarif = m_utility.SafeGetDecimal(reader, "tarif", Nothing)
						overviewData.grundlohn = m_utility.SafeGetDecimal(reader, "grundlohn", Nothing)
						overviewData.stundenlohn = m_utility.SafeGetDecimal(reader, "stundenlohn", Nothing)

						overviewData.ferienproz = m_utility.SafeGetDecimal(reader, "ferienproz", Nothing)
						overviewData.feierproz = m_utility.SafeGetDecimal(reader, "feierproz", Nothing)
						overviewData.lohn13proz = m_utility.SafeGetDecimal(reader, "lohn13proz", Nothing)
						overviewData.mastdspesen = m_utility.SafeGetDecimal(reader, "mastdspesen", Nothing)
						overviewData.matspesen = m_utility.SafeGetDecimal(reader, "matspesen", Nothing)
						overviewData.kdtspesen = m_utility.SafeGetDecimal(reader, "kdtspesen", Nothing)
						overviewData.bruttomarge = m_utility.SafeGetDecimal(reader, "bruttomarge", Nothing)

						overviewData.gavkanton = m_utility.SafeGetString(reader, "gavkanton")
						overviewData.gavbezeichnung = m_utility.SafeGetString(reader, "gavbezeichnung")
						overviewData.gavfar = m_utility.SafeGetString(reader, "gavfar")
						overviewData.actives = m_utility.SafeGetBoolean(reader, "actives", False)


						overviewData.customername = m_utility.SafeGetString(reader, "firma1")
						overviewData.customerstreet = m_utility.SafeGetString(reader, "kdstrasse")
						overviewData.customeraddress = m_utility.SafeGetString(reader, "kdadresse")
						overviewData.customeremail = m_utility.SafeGetString(reader, "kdemail")
						overviewData.customertelefon = m_utility.SafeGetString(reader, "kdTelefon")

						overviewData.kreditlimite = m_utility.SafeGetDecimal(reader, "KreditLimite", Nothing)
						overviewData.kreditlimite_2 = m_utility.SafeGetDecimal(reader, "Kreditlimite_2", Nothing)
						overviewData.kreditlimiteab = m_utility.SafeGetDateTime(reader, "KreditlimiteAb", Nothing)
						overviewData.kreditlimitebis = m_utility.SafeGetDateTime(reader, "KreditlimiteBis", Nothing)
						overviewData.kreditwarnung = m_utility.SafeGetBoolean(reader, "KreditWarnung", False)
						overviewData.PrintNoRP = m_utility.SafeGetBoolean(reader, "PrintNoRP", False)

						overviewData.zname = m_utility.SafeGetString(reader, "kdzname")
						overviewData.ztelefon = m_utility.SafeGetString(reader, "zhdTelefon")
						overviewData.zmobile = m_utility.SafeGetString(reader, "zhdNatel")
						overviewData.zemail = m_utility.SafeGetString(reader, "zhdemail")


						overviewData.employeename = m_utility.SafeGetString(reader, "maname")
						overviewData.employeestreet = m_utility.SafeGetString(reader, "mastrasse")
						overviewData.employeeaddress = m_utility.SafeGetString(reader, "maadresse")

						overviewData.employeetelfon = m_utility.SafeGetString(reader, "matelefon")

						overviewData.employeemobile = m_utility.SafeGetString(reader, "manatel")
						overviewData.employeeemail = m_utility.SafeGetString(reader, "maemail")

						overviewData.proposeals = m_utility.SafeGetString(reader, "pals")
						overviewData.proposecreatedon = m_utility.SafeGetDateTime(reader, "pcreatedon", Nothing)
						overviewData.proposestatus = m_utility.SafeGetString(reader, "pstatus")

						overviewData.zfiliale = m_utility.SafeGetString(reader, "zfiliale")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function


		Function GetDbRepportData4Show(ByVal sql As String) As IEnumerable(Of FoundedReportData)
			Dim result As List(Of FoundedReportData) = Nothing
			Dim m_utility As New Utilities

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", ModulConstants.MDData.MDNr))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_2", String.Empty))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_3", 0))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_4", ModulConstants.UserData.UserFiliale))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedReportData)

					While reader.Read()
						Dim overviewData As New FoundedReportData

						overviewData.mdnr = m_utility.SafeGetInteger(reader, "mdnr", 0)
						overviewData.employeeMDNr = m_utility.SafeGetInteger(reader, "employeemdnr", 0)
						overviewData.customerMDNr = m_utility.SafeGetInteger(reader, "customermdnr", 0)

						overviewData.esMDNr = m_utility.SafeGetInteger(reader, "esmdnr", 0)
						overviewData.loMDNr = m_utility.SafeGetInteger(reader, "lomdnr", 0)

						overviewData._res = m_utility.SafeGetString(reader, "0")
						overviewData.rpnr = CInt(m_utility.SafeGetInteger(reader, "rpnr", 0))
						overviewData.lonr = CInt(m_utility.SafeGetInteger(reader, "lonr", Nothing))

						overviewData.esnr = CInt(m_utility.SafeGetInteger(reader, "esnr", 0))
						overviewData.manr = CInt(m_utility.SafeGetInteger(reader, "manr", 0))
						overviewData.kdnr = CInt(m_utility.SafeGetInteger(reader, "kdnr", 0))

						overviewData.rpmonat = CInt(m_utility.SafeGetInteger(reader, "rpmonat", 0))
						overviewData.rpjahr = CInt(m_utility.SafeGetInteger(reader, "rpjahr", 0))

						overviewData.rpperiode = m_utility.SafeGetString(reader, "rpperiode")
						overviewData.es_als = m_utility.SafeGetString(reader, "es_als")
						overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")
						overviewData.zhdnr = CInt(m_utility.SafeGetInteger(reader, "kdzhdnr", Nothing))
						overviewData.vaknr = CInt(m_utility.SafeGetInteger(reader, "vaknr", Nothing))

						overviewData.customername = m_utility.SafeGetString(reader, "firma1")
						overviewData.customerstreet = m_utility.SafeGetString(reader, "kdstrasse")
						overviewData.customeraddress = m_utility.SafeGetString(reader, "kdadresse")
						overviewData.customeremail = m_utility.SafeGetString(reader, "kdemail")
						overviewData.customertelefon = m_utility.SafeGetString(reader, "kdTelefon")
						overviewData.customertelefax = m_utility.SafeGetString(reader, "kdTelefax")

						overviewData.zname = m_utility.SafeGetString(reader, "zname")
						overviewData.zemail = m_utility.SafeGetString(reader, "zemail")
						overviewData.ztelefon = m_utility.SafeGetString(reader, "zTelefon")
						overviewData.zmobile = m_utility.SafeGetString(reader, "zNatel")

						overviewData.gavkanton = m_utility.SafeGetString(reader, "gavkanton")
						overviewData.rpgav_beruf = m_utility.SafeGetString(reader, "rpgav_beruf")
						overviewData.gavbezeichnung = m_utility.SafeGetString(reader, "gavbezeichnung")
						overviewData.gavfar = m_utility.SafeGetString(reader, "gavfar")
						overviewData.gavparifond = m_utility.SafeGetString(reader, "gavparifond")

						overviewData.PrintNoRP = m_utility.SafeGetBoolean(reader, "PrintNoRP", False)
						overviewData.rpdone = m_utility.SafeGetBoolean(reader, "erfasst", False)

						overviewData.employeename = m_utility.SafeGetString(reader, "maname")
						overviewData.employeestreet = m_utility.SafeGetString(reader, "mastrasse")
						overviewData.employeeaddress = m_utility.SafeGetString(reader, "maadresse")
						overviewData.employeetelfon = m_utility.SafeGetString(reader, "matelefon")
						overviewData.employeemobile = m_utility.SafeGetString(reader, "manatel")
						overviewData.employeeemail = m_utility.SafeGetString(reader, "maemail")

						overviewData.rpkst1 = m_utility.SafeGetString(reader, "rpkst1")
						overviewData.rpkst2 = m_utility.SafeGetString(reader, "rpkst2")
						overviewData.rpkst = m_utility.SafeGetString(reader, "rpkst")

						overviewData.employeeadvisor = m_utility.SafeGetString(reader, "employeeadvisor")
						overviewData.customeradvisor = m_utility.SafeGetString(reader, "customeradvisor")
						If overviewData.employeeadvisor = overviewData.customeradvisor Then
							overviewData.rpadvisor = String.Format("{0}", overviewData.employeeadvisor)
						Else
							overviewData.rpadvisor = String.Format("{0} - {1}", overviewData.employeeadvisor, overviewData.customeradvisor)
						End If

						overviewData.es_ab = m_utility.SafeGetDateTime(reader, "es_ab", Nothing)
						overviewData.es_ende = m_utility.SafeGetDateTime(reader, "es_ende", Nothing)

						overviewData.gavnumber = m_utility.SafeGetInteger(reader, "gavnumber", 0)
						overviewData.von = m_utility.SafeGetDateTime(reader, "von", Nothing)
						overviewData.bis = m_utility.SafeGetDateTime(reader, "Bis", Nothing)
						overviewData.lovon = m_utility.SafeGetDateTime(reader, "LOVon", Nothing)
						overviewData.gavstate = True

						overviewData.zfiliale = m_utility.SafeGetString(reader, "zfiliale")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function


#Region "Employee Propierties"

		Function GetDbEmployeeESDataForProperties(ByVal employeenumber As Integer?) As IEnumerable(Of FoundedEmployeeESDetailData)
			Dim result As List(Of FoundedEmployeeESDetailData) = Nothing
			Dim m_utility As New Utilities

			Dim sql As String = "[Get New Top ESData 4 Selected MA In MainView]"
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MANr", employeenumber))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedEmployeeESDetailData)

					While reader.Read()
						Dim overviewData As New FoundedEmployeeESDetailData

						overviewData.mdnr = CInt(m_utility.SafeGetInteger(reader, "mdnr", 0))
						overviewData.esnr = CInt(m_utility.SafeGetInteger(reader, "esnr", 0))
						overviewData.kdnr = CInt(m_utility.SafeGetInteger(reader, "kdnr", 0))
						overviewData.zhdnr = CInt(m_utility.SafeGetInteger(reader, "kdzhdnr", 0))

						overviewData.esals = m_utility.SafeGetString(reader, "es_als")
						overviewData.periode = m_utility.SafeGetString(reader, "periode")

						overviewData.customername = m_utility.SafeGetString(reader, "Firma1")
						overviewData.zfiliale = m_utility.SafeGetString(reader, "zfiliale")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function


		Function GetDbEmployeeProposalDataForProperties(ByVal employeenumber As Integer?) As IEnumerable(Of FoundedEmployeeProposalDetailData)
			Dim result As List(Of FoundedEmployeeProposalDetailData) = Nothing
			Dim m_utility As New Utilities

			Dim sql As String = "[Get New Top ProposeData 4 Selected MA In MainView]"
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MANr", employeenumber))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedEmployeeProposalDetailData)

					While reader.Read()
						Dim overviewData As New FoundedEmployeeProposalDetailData

						overviewData.pnr = m_utility.SafeGetInteger(reader, "ProposeNr", 0)
						overviewData.kdnr = m_utility.SafeGetInteger(reader, "kdnr", Nothing)
						overviewData.zhdnr = m_utility.SafeGetInteger(reader, "zhdnr", Nothing)
						overviewData.manr = m_utility.SafeGetString(reader, "manr", Nothing)

						overviewData.bezeichnung = m_utility.SafeGetString(reader, "Bezeichnung")
						overviewData.customername = m_utility.SafeGetString(reader, "firma1")
						overviewData.employeename = m_utility.SafeGetString(reader, "maname")
						overviewData.zhdname = m_utility.SafeGetString(reader, "zname")
						overviewData.p_art = m_utility.SafeGetString(reader, "p_art")
						overviewData.p_state = m_utility.SafeGetString(reader, "p_state")

						overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")
						overviewData.zfiliale = m_utility.SafeGetString(reader, "zfiliale")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function


		'Function GetDbEmployeeContactDataForProperties(ByVal EmployeeNumber As Integer?) As IEnumerable(Of FoundedEmployeeContactDetailData)
		'	Dim result As List(Of FoundedEmployeeContactDetailData) = Nothing
		'	Dim m_utility As New Utilities

		'	Dim sql As String = "[Get New Top KontaktData 4 Selected MA In MainView]"
		'	Dim listOfParams As New List(Of SqlClient.SqlParameter)
		'	listOfParams.Add(New SqlClient.SqlParameter("@MANr", EmployeeNumber))

		'	Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

		'	Try

		'		If (Not reader Is Nothing) Then

		'			result = New List(Of FoundedEmployeeContactDetailData)

		'			While reader.Read()
		'				Dim overviewData As New FoundedEmployeeContactDetailData

		'				overviewData.contactnr = m_utility.SafeGetInteger(reader, "RecNr", 0)
		'				overviewData.manr = m_utility.SafeGetInteger(reader, "manr", Nothing)
		'				overviewData.bezeichnung = m_utility.SafeGetString(reader, "bezeichnung")
		'				overviewData.beschreibung = m_utility.SafeGetString(reader, "Beschreibung")
		'				overviewData.datum = m_utility.SafeGetDateTime(reader, "datum", Nothing)
		'				overviewData.art = m_utility.SafeGetString(reader, "KontaktType1")

		'				overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
		'				overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")

		'				result.Add(overviewData)

		'			End While

		'		End If

		'	Catch e As Exception
		'		result = Nothing
		'		m_Logger.LogError(e.ToString())

		'	Finally
		'		m_utility.CloseReader(reader)

		'	End Try

		'	Return result
		'End Function


		Function GetDbEmployeeSalaryDataForProperties(ByVal employeenumber As Integer?) As IEnumerable(Of FoundedEmployeeSalaryDetailData)
			Dim result As List(Of FoundedEmployeeSalaryDetailData) = Nothing
			Dim m_utility As New Utilities

			Dim sql As String = "[Get New Top NewLOData 4 Selected MA In MainView]"
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MANr", employeenumber))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedEmployeeSalaryDetailData)

					While reader.Read()
						Dim overviewData As New FoundedEmployeeSalaryDetailData

						overviewData.mdnr = m_utility.SafeGetInteger(reader, "mdnr", 0)
						overviewData.lonr = m_utility.SafeGetInteger(reader, "lonr", 0)

						overviewData.periode = m_utility.SafeGetString(reader, "periode")

						overviewData.zfiliale = m_utility.SafeGetString(reader, "zFiliale")
						overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function


		Function GetDbEmployeeZGDataForProperties(ByVal employeenumber As Integer?) As IEnumerable(Of FoundedEmployeeZGDetailData)
			Dim result As List(Of FoundedEmployeeZGDetailData) = Nothing
			Dim m_utility As New Utilities

			Dim sql As String = "[Get New Top OpenZGData 4 Selected MA In MainView]"
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MANr", employeenumber))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedEmployeeZGDetailData)

					While reader.Read()
						Dim overviewData As New FoundedEmployeeZGDetailData

						overviewData.mdnr = m_utility.SafeGetInteger(reader, "mdnr", 0)
						overviewData.zgnr = m_utility.SafeGetInteger(reader, "zgnr", 0)

						overviewData.betrag = m_utility.SafeGetDecimal(reader, "Betrag", 0)
						overviewData.zgperiode = m_utility.SafeGetString(reader, "zgperiode")

						overviewData.zfiliale = m_utility.SafeGetString(reader, "zFiliale")
						overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function

		Function GetDbEmployeeReportDataForProperties(ByVal employeenumber As Integer?) As IEnumerable(Of FoundedEmployeeReportDetailData)
			Dim result As List(Of FoundedEmployeeReportDetailData) = Nothing
			Dim m_utility As New Utilities

			Dim sql As String = "[Get New Top OpenRPData 4 Selected MA In MainView]"
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MANr", employeenumber))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedEmployeeReportDetailData)

					While reader.Read()
						Dim overviewData As New FoundedEmployeeReportDetailData

						overviewData.mdnr = m_utility.SafeGetInteger(reader, "mdnr", 0)
						overviewData.rpnr = m_utility.SafeGetInteger(reader, "rpnr", 0)

						overviewData.periode = m_utility.SafeGetString(reader, "periode")
						overviewData.customername = m_utility.SafeGetString(reader, "Firma1")

						overviewData.zfiliale = m_utility.SafeGetString(reader, "zFiliale")
						overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function

#End Region



#Region "Employee Details"

		Function GetDbEmployeeESDataForDetails(ByVal employeenumber As Integer?) As IEnumerable(Of FoundedEmployeeESDetailData)
			Dim result As List(Of FoundedEmployeeESDetailData) = Nothing
			Dim m_utility As New Utilities

			Dim sql As String
			If employeenumber.HasValue Then
				sql = "[Get ESData 4 Selected MA In MainView]"
			Else
				sql = "[Get ESData 4 All MA In MainView]"
			End If

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			If employeenumber.HasValue Then listOfParams.Add(New SqlClient.SqlParameter("@MANr", employeenumber))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedEmployeeESDetailData)

					While reader.Read()
						Dim overviewData As New FoundedEmployeeESDetailData

						overviewData.mdnr = m_utility.SafeGetInteger(reader, "mdnr", 0)
						overviewData.esnr = m_utility.SafeGetInteger(reader, "esnr", 0)
						overviewData.kdnr = m_utility.SafeGetInteger(reader, "kdnr", 0)
						overviewData.zhdnr = m_utility.SafeGetInteger(reader, "kdzhdnr", 0)
						overviewData.manr = m_utility.SafeGetInteger(reader, "manr", 0)

						overviewData.esals = m_utility.SafeGetString(reader, "es_als")
						overviewData.periode = m_utility.SafeGetString(reader, "periode")

						overviewData.customername = m_utility.SafeGetString(reader, "Firma1")
						overviewData.employeename = m_utility.SafeGetString(reader, "maname")

						overviewData.tarif = m_utility.SafeGetDecimal(reader, "tarif", Nothing)
						overviewData.stundenlohn = m_utility.SafeGetDecimal(reader, "stundenlohn", Nothing)
						overviewData.margemitbvg = m_utility.SafeGetDecimal(reader, "MargeMitBVG", Nothing)
						overviewData.margeohnebvg = m_utility.SafeGetDecimal(reader, "bruttomarge", Nothing)

						overviewData.actives = m_utility.SafeGetBoolean(reader, "actives", False)

						overviewData.zfiliale = m_utility.SafeGetString(reader, "zFiliale")
						overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function

		Function GetDbEmployeeProposalDataForDetails(ByVal employeenumber As Integer?) As IEnumerable(Of FoundedEmployeeProposalDetailData)
			Dim result As List(Of FoundedEmployeeProposalDetailData) = Nothing
			Dim m_utility As New Utilities

			Dim sql As String
			If employeenumber.HasValue Then
				sql = "[Get ProposeData 4 Selected MA In MainView]"
			Else
				sql = "[Get ProposeData 4 All MA In MainView]"
			End If

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			If employeenumber.HasValue Then listOfParams.Add(New SqlClient.SqlParameter("@MANr", employeenumber))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedEmployeeProposalDetailData)

					While reader.Read()
						Dim overviewData As New FoundedEmployeeProposalDetailData

						overviewData.mdnr = m_utility.SafeGetInteger(reader, "mdnr", 0)
						overviewData.pnr = m_utility.SafeGetInteger(reader, "ProposeNr", 0)
						overviewData.kdnr = m_utility.SafeGetInteger(reader, "kdnr", Nothing)
						overviewData.zhdnr = m_utility.SafeGetInteger(reader, "zhdnr", Nothing)
						overviewData.manr = m_utility.SafeGetString(reader, "manr", Nothing)

						overviewData.bezeichnung = m_utility.SafeGetString(reader, "Bezeichnung")
						overviewData.customername = m_utility.SafeGetString(reader, "firma1")
						overviewData.employeename = m_utility.SafeGetString(reader, "maname")
						overviewData.zhdname = m_utility.SafeGetString(reader, "zname")
						overviewData.p_art = m_utility.SafeGetString(reader, "p_art")
						overviewData.p_state = m_utility.SafeGetString(reader, "p_state")

						overviewData.advisor = m_utility.SafeGetString(reader, "berater")
						overviewData.zfiliale = m_utility.SafeGetString(reader, "zFiliale")
						overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function

		'Function GetDbEmployeeContactDataForDetails(ByVal EmployeeNumber As Integer?) As IEnumerable(Of FoundedEmployeeContactDetailData)
		'	Dim result As List(Of FoundedEmployeeContactDetailData) = Nothing
		'	Dim m_utility As New Utilities

		'	Dim sql As String
		'	If EmployeeNumber.HasValue Then
		'		sql = "[Get KontaktData 4 Selected MA In MainView]"
		'	Else
		'		sql = "[Get KontaktData 4 All MA In MainView]"
		'	End If

		'	Dim listOfParams As New List(Of SqlClient.SqlParameter)
		'	If EmployeeNumber.HasValue Then listOfParams.Add(New SqlClient.SqlParameter("@MANr", EmployeeNumber))

		'	Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

		'	Try

		'		If (Not reader Is Nothing) Then

		'			result = New List(Of FoundedEmployeeContactDetailData)

		'			While reader.Read()
		'				Dim overviewData As New FoundedEmployeeContactDetailData

		'				overviewData.contactnr = m_utility.SafeGetInteger(reader, "RecNr", 0)
		'				overviewData.manr = m_utility.SafeGetInteger(reader, "manr", Nothing)
		'				overviewData.kdnr = m_utility.SafeGetInteger(reader, "kdnr", Nothing)

		'				overviewData.monat = m_utility.SafeGetInteger(reader, "contactmonth", 0)
		'				overviewData.jahr = m_utility.SafeGetInteger(reader, "contactyear", 0)

		'				overviewData.customername = m_utility.SafeGetString(reader, "firma1")
		'				overviewData.employeename = m_utility.SafeGetString(reader, "maname")

		'				overviewData.bezeichnung = m_utility.SafeGetString(reader, "Bezeichnung")
		'				overviewData.beschreibung = m_utility.SafeGetString(reader, "Beschreibung")
		'				overviewData.datum = m_utility.SafeGetDateTime(reader, "datum", Nothing)
		'				overviewData.art = m_utility.SafeGetString(reader, "kontakttype1")

		'				overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
		'				overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")

		'				result.Add(overviewData)

		'			End While

		'		End If

		'	Catch e As Exception
		'		result = Nothing
		'		m_Logger.LogError(e.ToString())

		'	Finally
		'		m_utility.CloseReader(reader)

		'	End Try

		'	Return result
		'End Function

		Function GetDbEmployeeSalaryDataForDetail(ByVal employeenumber As Integer?) As IEnumerable(Of FoundedEmployeeSalaryDetailData)
			Dim result As List(Of FoundedEmployeeSalaryDetailData) = Nothing
			Dim m_utility As New Utilities

			Dim sql As String
			If employeenumber.HasValue Then
				sql = "[Get All LOData 4 Selected MA In MainView]"
			Else
				sql = "[Get New Top NewLOData 4 All MA In MainView]"
			End If


			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			If employeenumber.HasValue Then listOfParams.Add(New SqlClient.SqlParameter("@MANr", employeenumber))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedEmployeeSalaryDetailData)

					While reader.Read()
						Dim overviewData As New FoundedEmployeeSalaryDetailData

						overviewData.mdnr = m_utility.SafeGetInteger(reader, "mdnr", 0)
						overviewData.lonr = m_utility.SafeGetInteger(reader, "lonr", 0)
						overviewData.manr = m_utility.SafeGetInteger(reader, "manr", 0)

						overviewData.monat = m_utility.SafeGetInteger(reader, "lp", 0)
						overviewData.jahr = m_utility.SafeGetInteger(reader, "jahr", 0)

						overviewData.periode = m_utility.SafeGetString(reader, "periode")

						overviewData.employeename = m_utility.SafeGetString(reader, "maname")

						overviewData.zfiliale = m_utility.SafeGetString(reader, "zFiliale")
						overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function

		Function GetDbEmployeeReportDataForDetails(ByVal employeenumber As Integer?) As IEnumerable(Of FoundedEmployeeReportDetailData)
			Dim result As List(Of FoundedEmployeeReportDetailData) = Nothing
			Dim m_utility As New Utilities

			Dim sql As String
			If employeenumber.HasValue Then
				sql = "[Get RPData 4 Selected MA In MainView]"
			Else
				sql = "[Get RPData 4 All MA In MainView]"
			End If
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			If employeenumber.HasValue Then listOfParams.Add(New SqlClient.SqlParameter("@MANr", employeenumber))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedEmployeeReportDetailData)

					While reader.Read()
						Dim overviewData As New FoundedEmployeeReportDetailData

						overviewData.mdnr = m_utility.SafeGetInteger(reader, "mdnr", 0)
						overviewData.rpnr = m_utility.SafeGetInteger(reader, "rpnr", Nothing)

						overviewData.manr = m_utility.SafeGetInteger(reader, "manr", Nothing)
						overviewData.kdnr = m_utility.SafeGetInteger(reader, "kdnr", Nothing)
						overviewData.esnr = m_utility.SafeGetInteger(reader, "esnr", Nothing)
						overviewData.lonr = m_utility.SafeGetInteger(reader, "lonr", Nothing)

						overviewData.monat = m_utility.SafeGetInteger(reader, "monat", Nothing)
						overviewData.jahr = m_utility.SafeGetInteger(reader, "jahr", Nothing)
						overviewData.rpdone = m_utility.SafeGetInteger(reader, "erfasst", Nothing)

						overviewData.employeename = m_utility.SafeGetString(reader, "maname")
						overviewData.customername = m_utility.SafeGetString(reader, "firma1")
						overviewData.rpgav_beruf = m_utility.SafeGetString(reader, "RPGAV_Beruf")

						overviewData.periode = m_utility.SafeGetString(reader, "periode")

						overviewData.zfiliale = m_utility.SafeGetString(reader, "zFiliale")
						overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function

		Function GetDbEmployeeZGDataForDetails(ByVal employeenumber As Integer?) As IEnumerable(Of FoundedEmployeeZGDetailData)
			Dim result As List(Of FoundedEmployeeZGDetailData) = Nothing
			Dim m_utility As New Utilities

			Dim sql As String
			If employeenumber.HasValue Then
				sql = "[Get ZGData 4 Selected MA In MainView]"
			Else
				sql = "[Get ZGData 4 All MA In MainView]"
			End If
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			If employeenumber.HasValue Then listOfParams.Add(New SqlClient.SqlParameter("@MANr", employeenumber))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedEmployeeZGDetailData)

					While reader.Read()
						Dim overviewData As New FoundedEmployeeZGDetailData

						overviewData.mdnr = CInt(m_utility.SafeGetInteger(reader, "MDNr", 0))
						overviewData.employeeMDNr = m_utility.SafeGetInteger(reader, "employeemdnr", 0)
						overviewData.zgnr = CInt(m_utility.SafeGetInteger(reader, "ZGNr", 0))
						overviewData.rpnr = CInt(m_utility.SafeGetInteger(reader, "rpnr", Nothing))
						overviewData.manr = CInt(m_utility.SafeGetInteger(reader, "MAnr", 0))
						overviewData.lonr = CInt(m_utility.SafeGetInteger(reader, "lonr", Nothing))
						overviewData.vgnr = CInt(m_utility.SafeGetInteger(reader, "vgnr", Nothing))

						overviewData.monat = CInt(m_utility.SafeGetInteger(reader, "monat", 0))
						overviewData.jahr = CInt(m_utility.SafeGetInteger(reader, "jahr", 0))

						overviewData.zgperiode = m_utility.SafeGetString(reader, "zgperiode")

						overviewData.betrag = m_utility.SafeGetDecimal(reader, "betrag", Nothing)

						overviewData.aus_dat = m_utility.SafeGetDateTime(reader, "aus_dat", Nothing)

						overviewData.employeename = m_utility.SafeGetString(reader, "maname")

						overviewData.lanr = m_utility.SafeGetString(reader, "lanr", 0)
						overviewData.laname = m_utility.SafeGetString(reader, "laname")

						overviewData.zfiliale = m_utility.SafeGetString(reader, "zfiliale")
						overviewData.createdon = m_utility.SafeGetDateTime(reader, "CreatedOn", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "Createdfrom")

						Dim bIsOut As Boolean = m_utility.SafeGetInteger(reader, "vgnr", 0) >= 1
						Dim bIsAsLO As Boolean = m_utility.SafeGetInteger(reader, "lonr", 0) >= 1

						overviewData.isout = CBool(bIsOut)
						overviewData.isaslo = CBool(bIsAsLO)

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function


#End Region


#Region "Customer Propierties"

		Function GetDbCustomerESDataForProperties(ByVal customerNumber As Integer?) As IEnumerable(Of FoundedCustomerESDetailData)
			Dim result As List(Of FoundedCustomerESDetailData) = Nothing
			Dim m_utility As New Utilities

			Dim sql As String = "[Get New Top ESData 4 Selected KD In MainView]"
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@KDNr", customerNumber))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedCustomerESDetailData)

					While reader.Read()
						Dim overviewData As New FoundedCustomerESDetailData

						overviewData.mdnr = CInt(m_utility.SafeGetInteger(reader, "mdnr", 0))
						overviewData.esnr = CInt(m_utility.SafeGetInteger(reader, "esnr", 0))
						overviewData.manr = CInt(m_utility.SafeGetInteger(reader, "manr", 0))
						overviewData.zhdnr = CInt(m_utility.SafeGetInteger(reader, "kdzhdnr", 0))

						overviewData.esals = m_utility.SafeGetString(reader, "es_als")
						overviewData.periode = m_utility.SafeGetString(reader, "periode")

						overviewData.employeename = m_utility.SafeGetString(reader, "maname")
						overviewData.zfiliale = m_utility.SafeGetString(reader, "zfiliale")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function

		Function GetDbCustomerReportDataForProperties(ByVal customerNumber As Integer?) As IEnumerable(Of FoundedCustomerReportDetailData)
			Dim result As List(Of FoundedCustomerReportDetailData) = Nothing
			Dim m_utility As New Utilities

			Dim sql As String = "[Get New Top OpenRPData 4 Selected KD In MainView]"
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@KDNr", customerNumber))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedCustomerReportDetailData)

					While reader.Read()
						Dim overviewData As New FoundedCustomerReportDetailData

						overviewData.mdnr = m_utility.SafeGetInteger(reader, "mdnr", 0)
						overviewData.rpnr = m_utility.SafeGetInteger(reader, "rpnr", 0)
						overviewData.employeename = m_utility.SafeGetString(reader, "maname")

						overviewData.periode = m_utility.SafeGetString(reader, "periode")

						overviewData.zfiliale = m_utility.SafeGetString(reader, "zFiliale")
						overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function


		'Function GetDbCustomerContactDataForProperties(ByVal customerNumber As Integer?) As IEnumerable(Of FoundedCustomerContactDetailData)
		'	Dim result As List(Of FoundedCustomerContactDetailData) = Nothing
		'	Dim m_utility As New Utilities

		'	Dim sql As String = "[Get New Top KontaktData 4 Selected KD In MainView]"
		'	Dim listOfParams As New List(Of SqlClient.SqlParameter)
		'	listOfParams.Add(New SqlClient.SqlParameter("@KDNr", customerNumber))

		'	Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

		'	Try

		'		If (Not reader Is Nothing) Then

		'			result = New List(Of FoundedCustomerContactDetailData)

		'			While reader.Read()
		'				Dim overviewData As New FoundedCustomerContactDetailData

		'				overviewData.contactnr = m_utility.SafeGetInteger(reader, "RecNr", 0)
		'				overviewData.kdnr = m_utility.SafeGetInteger(reader, "kdnr", Nothing)
		'				overviewData.zhdnr = m_utility.SafeGetInteger(reader, "zhdnr", Nothing)
		'				overviewData.zhdname = m_utility.SafeGetString(reader, "zname")

		'				overviewData.employeename = m_utility.SafeGetString(reader, "maname")
		'				overviewData.customername = m_utility.SafeGetString(reader, "firma1")
		'				overviewData.zhdname = m_utility.SafeGetString(reader, "zname")
		'				overviewData.datum = m_utility.SafeGetDateTime(reader, "datum", Nothing)

		'				overviewData.bezeichnung = m_utility.SafeGetString(reader, "Bezeichnung")
		'				overviewData.beschreibung = m_utility.SafeGetString(reader, "Beschreibung")
		'				overviewData.kst = m_utility.SafeGetString(reader, "kst")
		'				overviewData.art = m_utility.SafeGetString(reader, "KontaktType1")

		'				overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
		'				overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")

		'				result.Add(overviewData)

		'			End While

		'		End If

		'	Catch e As Exception
		'		result = Nothing
		'		m_Logger.LogError(e.ToString())

		'	Finally
		'		m_utility.CloseReader(reader)

		'	End Try

		'	Return result
		'End Function

		Function GetDbCustomerVacancyDataForProperties(ByVal customerNumber As Integer?, ByVal showOpenVacancy As Boolean?) As IEnumerable(Of CustomerVacanciesProperty)
			Dim success = True
			Dim m_utility As New Utilities

			Dim result As List(Of CustomerVacanciesProperty) = Nothing

			Dim sql As String
			If customerNumber.HasValue Then
				If showOpenVacancy.GetValueOrDefault(False) Then
					sql = "[Get Open VacanciesData 4 Selected Customer In MainView]"
				Else
					sql = "[Get VacanciesData 4 Selected Customer In MainView]"
				End If

			Else
				sql = "[Get VacanciesData 4 All Customer In MainView]"
			End If

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			If customerNumber.HasValue Then listOfParams.Add(New SqlClient.SqlParameter("@KDNr", ReplaceMissing(customerNumber, 0)))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CustomerVacanciesProperty)

					While reader.Read()
						Dim overviewData As New CustomerVacanciesProperty

						overviewData.mdnr = m_utility.SafeGetInteger(reader, "mdnr", 0)
						overviewData.vaknr = m_utility.SafeGetInteger(reader, "vaknr", 0)
						overviewData.kdnr = m_utility.SafeGetInteger(reader, "kdnr", 0)
						overviewData.kdzhdnr = m_utility.SafeGetInteger(reader, "kdzhdnr", 0)

						overviewData.firma1 = m_utility.SafeGetString(reader, "firma1")
						overviewData.bezeichnung = m_utility.SafeGetString(reader, "bezeichnung")
						overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")

						overviewData.kdzname = m_utility.SafeGetString(reader, "kdzname")
						overviewData.advisor = m_utility.SafeGetString(reader, "BeraterIn")

						overviewData.kdemail = m_utility.SafeGetString(reader, "kdemail")
						overviewData.zemail = m_utility.SafeGetString(reader, "zemail")

						overviewData.vakstate = m_utility.SafeGetString(reader, "Vakstate")
						overviewData.vak_kanton = m_utility.SafeGetString(reader, "Vak_kanton")

						overviewData.vaklink = m_utility.SafeGetString(reader, "VakLink")

						overviewData.vakkontakt = m_utility.SafeGetString(reader, "vakkontakt")
						overviewData.vacancygruppe = m_utility.SafeGetString(reader, "vacancygruppe")

						overviewData.vacancyplz = m_utility.SafeGetString(reader, "vacancyplz")
						overviewData.vacancyort = m_utility.SafeGetString(reader, "vacancyort")

						overviewData.titelforsearch = m_utility.SafeGetString(reader, "titelforsearch")
						overviewData.shortdescription = m_utility.SafeGetString(reader, "shortdescription")

						overviewData.kdtelefon = m_utility.SafeGetString(reader, "kdtelefon")
						overviewData.kdtelefax = m_utility.SafeGetString(reader, "kdtelefax")

						overviewData.ztelefon = m_utility.SafeGetString(reader, "ztelefon")
						overviewData.ztelefax = m_utility.SafeGetString(reader, "ztelefax")
						overviewData.znatel = m_utility.SafeGetString(reader, "znatel")

						overviewData.ourisonline = m_utility.SafeGetBoolean(reader, "ourisonline", Nothing)
						overviewData.jchisonline = m_utility.SafeGetBoolean(reader, "jchisonline", Nothing)
						overviewData.ojisonline = m_utility.SafeGetBoolean(reader, "ojisonline", Nothing)

						overviewData.jobchdate = m_utility.SafeGetString(reader, "jobchdate")
						overviewData.ostjobchdate = m_utility.SafeGetString(reader, "ostjobchdate")

						overviewData.zfiliale = m_utility.SafeGetString(reader, "zfiliale")


						result.Add(overviewData)

					End While

					reader.Close()

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally

			End Try

			Return result

		End Function


		Function GetDbCustomerProposalDataForProperties(ByVal customerNumber As Integer?) As IEnumerable(Of FoundedCustomerProposalDetailData)
			Dim result As List(Of FoundedCustomerProposalDetailData) = Nothing
			Dim m_utility As New Utilities

			Dim sql As String = "[Get New Top ProposeData 4 Selected KD In MainView]"
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@KDNr", customerNumber))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedCustomerProposalDetailData)

					While reader.Read()
						Dim overviewData As New FoundedCustomerProposalDetailData

						overviewData.mdnr = m_utility.SafeGetInteger(reader, "mdnr", 0)

						overviewData.pnr = m_utility.SafeGetInteger(reader, "ProposeNr", 0)
						overviewData.kdnr = m_utility.SafeGetInteger(reader, "kdnr", Nothing)
						overviewData.zhdnr = m_utility.SafeGetInteger(reader, "zhdnr", Nothing)
						overviewData.manr = m_utility.SafeGetString(reader, "manr", Nothing)

						overviewData.bezeichnung = m_utility.SafeGetString(reader, "Bezeichnung")
						overviewData.customername = m_utility.SafeGetString(reader, "firma1")
						overviewData.employeename = m_utility.SafeGetString(reader, "maname")
						overviewData.zhdname = m_utility.SafeGetString(reader, "zname")
						overviewData.p_art = m_utility.SafeGetString(reader, "p_art")
						overviewData.p_state = m_utility.SafeGetString(reader, "p_state")

						overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")
						overviewData.zfiliale = m_utility.SafeGetString(reader, "zfiliale")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function

		Function GetDbCustomerInvoiceDataForProperties(ByVal customerNumber As Integer?) As IEnumerable(Of FoundedCustomerInvoiceDetailData)
			Dim result As List(Of FoundedCustomerInvoiceDetailData) = Nothing
			Dim m_utility As New Utilities

			Dim sql As String = "[Get New Top OpenREData 4 Selected KD In MainView]"
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@KDNr", customerNumber))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedCustomerInvoiceDetailData)

					While reader.Read()
						Dim overviewData As New FoundedCustomerInvoiceDetailData

						overviewData.mdnr = m_utility.SafeGetString(reader, "mdnr", 0)
						overviewData.customerMDNr = m_utility.SafeGetString(reader, "customerMDNr", 0)

						overviewData.renr = CInt(m_utility.SafeGetInteger(reader, "renr", 0))
						overviewData.kdnr = CInt(m_utility.SafeGetInteger(reader, "kdnr", 0))

						overviewData.firma1 = m_utility.SafeGetString(reader, "firma1")

						overviewData.zhd = m_utility.SafeGetString(reader, "zhd")
						overviewData.plzort = String.Format("{0} {1}", m_utility.SafeGetString(reader, "plz"), m_utility.SafeGetString(reader, "ort"))

						overviewData.fbmonth = CInt(m_utility.SafeGetInteger(reader, "fakmonth", 0))

						overviewData.fakdate = m_utility.SafeGetDateTime(reader, "fakdate", Nothing)
						overviewData.faelligdate = m_utility.SafeGetDateTime(reader, "faelligdate", Nothing)

						overviewData.einstufung = m_utility.SafeGetString(reader, "einstufung")
						overviewData.branche = m_utility.SafeGetString(reader, "branche")

						overviewData.betragink = m_utility.SafeGetDecimal(reader, "betragink", 0)
						overviewData.betragex = m_utility.SafeGetDecimal(reader, "betragex", 0)
						overviewData.bezahlt = m_utility.SafeGetDecimal(reader, "betragbezahlt", 0)
						overviewData.mwstproz = m_utility.SafeGetDecimal(reader, "mwstproz", 0)
						overviewData.betragmwst = m_utility.SafeGetDecimal(reader, "betragmwst", 0)
						overviewData.betragopen = m_utility.SafeGetDecimal(reader, "betragink", 0) - m_utility.SafeGetDecimal(reader, "betragbezahlt", 0)

						overviewData.isopen = overviewData.betragopen <> 0

						overviewData.rekst1 = m_utility.SafeGetString(reader, "rekst1")
						overviewData.rekst2 = m_utility.SafeGetString(reader, "rekst2")
						overviewData.rekst = m_utility.SafeGetString(reader, "rekst")

						overviewData.reart1 = m_utility.SafeGetString(reader, "reart1")
						overviewData.reart2 = m_utility.SafeGetString(reader, "reart2")
						overviewData.zahlkond = m_utility.SafeGetString(reader, "zahlungskondition")

						overviewData.employeeadvisor = m_utility.SafeGetString(reader, "employeeadvisor")
						overviewData.customeradvisor = m_utility.SafeGetString(reader, "customeradvisor")

						overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")
						overviewData.zfiliale = m_utility.SafeGetString(reader, "zfiliale")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function

		Function GetDbCustomerRecipientOfPaymentsDataForProperties(ByVal customerNumber As Integer?) As IEnumerable(Of FoundedCustomerROPDetailData)
			Dim result As List(Of FoundedCustomerROPDetailData) = Nothing
			Dim m_utility As New Utilities

			Dim sql As String = "[Get New Top ZEData 4 Selected KD In MainView]"
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@KDNr", customerNumber))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedCustomerROPDetailData)

					While reader.Read()
						Dim overviewData As New FoundedCustomerROPDetailData

						overviewData.customerMDNr = m_utility.SafeGetString(reader, "customermdnr", 0)
						overviewData.mdnr = m_utility.SafeGetString(reader, "mdnr", 0)
						overviewData.zenr = m_utility.SafeGetInteger(reader, "zenr", 0)
						overviewData.renr = m_utility.SafeGetInteger(reader, "renr", 0)
						overviewData.kdnr = m_utility.SafeGetInteger(reader, "kdnr", 0)

						overviewData.firma1 = m_utility.SafeGetString(reader, "firma1")
						overviewData.firma2 = m_utility.SafeGetString(reader, "firma2")
						overviewData.firma3 = m_utility.SafeGetString(reader, "firma3")
						overviewData.abteilung = m_utility.SafeGetString(reader, "abteilung")

						overviewData.zhd = m_utility.SafeGetString(reader, "zhd")
						overviewData.postfach = m_utility.SafeGetString(reader, "postfach")
						overviewData.strasse = m_utility.SafeGetString(reader, "strasse")
						overviewData.plz = m_utility.SafeGetString(reader, "ort")
						overviewData.ort = m_utility.SafeGetString(reader, "plz")
						overviewData.plzort = String.Format("{0} {1}", m_utility.SafeGetString(reader, "plz"), m_utility.SafeGetString(reader, "ort"))


						overviewData.valutadate = m_utility.SafeGetDateTime(reader, "valutadate", Nothing)
						overviewData.buchungdate = m_utility.SafeGetDateTime(reader, "buchungsdate", Nothing)
						overviewData.fakdate = m_utility.SafeGetDateTime(reader, "fakdate", Nothing)
						overviewData.faelligdate = m_utility.SafeGetDateTime(reader, "faelligdate", Nothing)

						overviewData.einstufung = m_utility.SafeGetString(reader, "einstufung")
						overviewData.branche = m_utility.SafeGetString(reader, "branche")

						overviewData.betragink = m_utility.SafeGetDecimal(reader, "betragink", 0)
						overviewData.zebetrag = m_utility.SafeGetDecimal(reader, "zebetrag", 0)

						overviewData.mwstproz = m_utility.SafeGetDecimal(reader, "mwstproz", 0)
						overviewData.betragopen = m_utility.SafeGetDecimal(reader, "betragink", 0) - m_utility.SafeGetDecimal(reader, "betragbezahlt", 0)

						overviewData.rekst1 = m_utility.SafeGetString(reader, "rekst1")
						overviewData.rekst2 = m_utility.SafeGetString(reader, "rekst2")
						overviewData.rekst = m_utility.SafeGetString(reader, "rekst")

						overviewData.reart1 = m_utility.SafeGetString(reader, "reart1")
						overviewData.reart2 = m_utility.SafeGetString(reader, "reart2")


						overviewData.kdtelefon = m_utility.SafeGetString(reader, "kdtelefon")
						overviewData.kdtelefax = m_utility.SafeGetString(reader, "kdtelefax")
						overviewData.kdemail = m_utility.SafeGetString(reader, "kdemail")

						overviewData.employeeadvisor = m_utility.SafeGetString(reader, "employeeadvisor")
						overviewData.customeradvisor = m_utility.SafeGetString(reader, "customeradvisor")

						overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")
						overviewData.zfiliale = m_utility.SafeGetString(reader, "zfiliale")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function


#End Region


#Region "Customer Details"

		Function GetDbCustomerESDataForDetails(ByVal customerNumber As Integer?) As IEnumerable(Of FoundedCustomerESDetailData)
			Dim result As List(Of FoundedCustomerESDetailData) = Nothing
			Dim m_utility As New Utilities

			Dim sql As String
			If customerNumber.HasValue Then
				sql = "[Get ESData 4 Selected KD In MainView]"
			Else
				sql = "[Get ESData 4 All KD In MainView]"
			End If

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			If customerNumber.HasValue Then listOfParams.Add(New SqlClient.SqlParameter("@KDNr", customerNumber))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedCustomerESDetailData)

					While reader.Read()
						Dim overviewData As New FoundedCustomerESDetailData

						overviewData.employeeMDNr = m_utility.SafeGetInteger(reader, "employeemdnr", 0)
						overviewData.customerMDNr = m_utility.SafeGetInteger(reader, "customermdnr", 0)

						overviewData.mdnr = m_utility.SafeGetInteger(reader, "mdnr", 0)
						overviewData.esnr = m_utility.SafeGetInteger(reader, "esnr", 0)
						overviewData.kdnr = m_utility.SafeGetInteger(reader, "kdnr", 0)
						overviewData.zhdnr = m_utility.SafeGetInteger(reader, "kdzhdnr", 0)
						overviewData.manr = m_utility.SafeGetInteger(reader, "manr", 0)

						overviewData.esals = m_utility.SafeGetString(reader, "es_als")
						overviewData.periode = m_utility.SafeGetString(reader, "periode")

						overviewData.customername = m_utility.SafeGetString(reader, "Firma1")
						overviewData.employeename = m_utility.SafeGetString(reader, "maname")

						overviewData.tarif = m_utility.SafeGetDecimal(reader, "tarif", Nothing)
						overviewData.stundenlohn = m_utility.SafeGetDecimal(reader, "stundenlohn", Nothing)
						overviewData.margemitbvg = m_utility.SafeGetDecimal(reader, "MargeMitBVG", Nothing)
						overviewData.margeohnebvg = m_utility.SafeGetDecimal(reader, "bruttomarge", Nothing)

						overviewData.actives = m_utility.SafeGetBoolean(reader, "actives", False)

						overviewData.zfiliale = m_utility.SafeGetString(reader, "zFiliale")
						overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function

		Function GetDbCustomerProposalDataForDetails(ByVal customerNumber As Integer?) As IEnumerable(Of FoundedCustomerProposalDetailData)
			Dim result As List(Of FoundedCustomerProposalDetailData) = Nothing
			Dim m_utility As New Utilities

			Dim sql As String
			If customerNumber.HasValue Then
				sql = "[Get ProposeData 4 Selected KD In MainView]"
			Else
				sql = "[Get ProposeData 4 All KD In MainView]"
			End If

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			If customerNumber.HasValue Then listOfParams.Add(New SqlClient.SqlParameter("@KDNr", customerNumber))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedCustomerProposalDetailData)

					While reader.Read()
						Dim overviewData As New FoundedCustomerProposalDetailData

						overviewData.mdnr = m_utility.SafeGetInteger(reader, "mdnr", 0)
						overviewData.pnr = m_utility.SafeGetInteger(reader, "ProposeNr", 0)
						overviewData.kdnr = m_utility.SafeGetInteger(reader, "kdnr", Nothing)
						overviewData.zhdnr = m_utility.SafeGetInteger(reader, "zhdnr", Nothing)
						overviewData.manr = m_utility.SafeGetString(reader, "manr", Nothing)

						overviewData.bezeichnung = m_utility.SafeGetString(reader, "Bezeichnung")
						overviewData.customername = m_utility.SafeGetString(reader, "firma1")
						overviewData.employeename = m_utility.SafeGetString(reader, "maname")
						overviewData.zhdname = m_utility.SafeGetString(reader, "zname")
						overviewData.p_art = m_utility.SafeGetString(reader, "p_art")
						overviewData.p_state = m_utility.SafeGetString(reader, "p_state")

						overviewData.advisor = m_utility.SafeGetString(reader, "berater")
						overviewData.zfiliale = m_utility.SafeGetString(reader, "zFiliale")
						overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function

		'Function GetDbCustomerContactDataForDetails(ByVal customerNumber As Integer?) As IEnumerable(Of FoundedCustomerContactDetailData)
		'	Dim result As List(Of FoundedCustomerContactDetailData) = Nothing
		'	Dim m_utility As New Utilities

		'	Dim sql As String
		'	If customerNumber.HasValue Then
		'		sql = "[Get KontaktData 4 Selected KD In MainView]"
		'	Else
		'		sql = "[Get KontaktData 4 All KD In MainView]"
		'	End If

		'	Dim listOfParams As New List(Of SqlClient.SqlParameter)
		'	If customerNumber.HasValue Then listOfParams.Add(New SqlClient.SqlParameter("@KDNr", customerNumber))

		'	Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

		'	Try

		'		If (Not reader Is Nothing) Then

		'			result = New List(Of FoundedCustomerContactDetailData)

		'			While reader.Read()
		'				Dim overviewData As New FoundedCustomerContactDetailData

		'				overviewData.contactnr = m_utility.SafeGetInteger(reader, "RecNr", 0)
		'				overviewData.manr = m_utility.SafeGetInteger(reader, "manr", Nothing)
		'				overviewData.kdnr = m_utility.SafeGetInteger(reader, "kdnr", Nothing)

		'				overviewData.monat = m_utility.SafeGetInteger(reader, "contactmonth", 0)
		'				overviewData.jahr = m_utility.SafeGetInteger(reader, "contactyear", 0)

		'				overviewData.customername = m_utility.SafeGetString(reader, "firma1")
		'				overviewData.zhdname = m_utility.SafeGetString(reader, "zname")

		'				overviewData.EmployeeNumbers = m_utility.SafeGetString(reader, "EmployeeNumbers")
		'				overviewData.EmployeeNames = m_utility.SafeGetString(reader, "EmployeeNames")
		'				overviewData.MoreEmployeesContacted = m_utility.SafeGetBoolean(reader, "MoreEmployees", False)

		'				overviewData.bezeichnung = m_utility.SafeGetString(reader, "Bezeichnung")
		'				overviewData.beschreibung = m_utility.SafeGetString(reader, "Beschreibung")
		'				overviewData.datum = m_utility.SafeGetDateTime(reader, "datum", Nothing)
		'				overviewData.art = m_utility.SafeGetString(reader, "kontakttype1")

		'				overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
		'				overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")

		'				result.Add(overviewData)

		'			End While

		'		End If

		'	Catch e As Exception
		'		result = Nothing
		'		m_Logger.LogError(e.ToString())

		'	Finally
		'		m_utility.CloseReader(reader)

		'	End Try

		'	Return result
		'End Function

		Function GetDbCustomerReportDataForDetails(ByVal customerNumber As Integer?) As IEnumerable(Of FoundedCustomerReportDetailData)
			Dim result As List(Of FoundedCustomerReportDetailData) = Nothing
			Dim m_utility As New Utilities

			Dim sql As String
			If customerNumber.HasValue Then
				sql = "[Get RPData 4 Selected KD In MainView]"
			Else
				sql = "[Get RPData 4 All KD In MainView]"
			End If
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			If customerNumber.HasValue Then listOfParams.Add(New SqlClient.SqlParameter("@KDNr", customerNumber))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedCustomerReportDetailData)

					While reader.Read()
						Dim overviewData As New FoundedCustomerReportDetailData

						overviewData.employeeMDNr = m_utility.SafeGetInteger(reader, "employeemdnr", 0)
						overviewData.customerMDNr = m_utility.SafeGetInteger(reader, "customermdnr", 0)

						overviewData.mdnr = m_utility.SafeGetInteger(reader, "mdnr", 0)
						overviewData.rpnr = m_utility.SafeGetInteger(reader, "rpnr", Nothing)

						overviewData.manr = m_utility.SafeGetInteger(reader, "manr", Nothing)
						overviewData.kdnr = m_utility.SafeGetInteger(reader, "kdnr", Nothing)
						overviewData.esnr = m_utility.SafeGetInteger(reader, "esnr", Nothing)
						overviewData.lonr = m_utility.SafeGetInteger(reader, "lonr", Nothing)

						overviewData.monat = m_utility.SafeGetInteger(reader, "monat", Nothing)
						overviewData.jahr = m_utility.SafeGetInteger(reader, "jahr", Nothing)
						overviewData.rpdone = m_utility.SafeGetBoolean(reader, "erfasst", Nothing)

						overviewData.employeename = m_utility.SafeGetString(reader, "maname")
						overviewData.customername = m_utility.SafeGetString(reader, "firma1")
						overviewData.rpgav_beruf = m_utility.SafeGetString(reader, "RPGAV_Beruf")

						overviewData.periode = m_utility.SafeGetString(reader, "periode")

						overviewData.zfiliale = m_utility.SafeGetString(reader, "zFiliale")
						overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function

		Function GetDbCustomerInvoiceDataForDetails(ByVal customerNumber As Integer?) As IEnumerable(Of FoundedCustomerInvoiceDetailData)
			Dim result As List(Of FoundedCustomerInvoiceDetailData) = Nothing
			Dim m_utility As New Utilities

			Dim sql As String
			If customerNumber.HasValue Then
				sql = "[Get REData 4 Selected KD In MainView]"
			Else
				sql = "[Get REData 4 All Kd In MainView]"
			End If

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			If customerNumber.HasValue Then listOfParams.Add(New SqlClient.SqlParameter("@KDNr", customerNumber))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedCustomerInvoiceDetailData)

					While reader.Read()
						Dim overviewData As New FoundedCustomerInvoiceDetailData

						overviewData.customerMDNr = m_utility.SafeGetString(reader, "customerMDNr", 0)
						overviewData.mdnr = m_utility.SafeGetString(reader, "mdnr", 0)

						overviewData.renr = CInt(m_utility.SafeGetInteger(reader, "renr", 0))
						overviewData.kdnr = CInt(m_utility.SafeGetInteger(reader, "kdnr", 0))

						overviewData.firma1 = m_utility.SafeGetString(reader, "firma1")

						overviewData.zhd = m_utility.SafeGetString(reader, "zhd")
						overviewData.plzort = String.Format("{0} {1}", m_utility.SafeGetString(reader, "plz"), m_utility.SafeGetString(reader, "ort"))

						overviewData.fbmonth = CInt(m_utility.SafeGetInteger(reader, "fakmonth", 0))

						overviewData.fakdate = m_utility.SafeGetDateTime(reader, "fakdate", Nothing)
						overviewData.faelligdate = m_utility.SafeGetDateTime(reader, "faelligdate", Nothing)

						overviewData.einstufung = m_utility.SafeGetString(reader, "einstufung")
						overviewData.branche = m_utility.SafeGetString(reader, "branche")

						overviewData.betragink = m_utility.SafeGetDecimal(reader, "betragink", 0)
						overviewData.betragex = m_utility.SafeGetDecimal(reader, "betragex", 0)
						overviewData.bezahlt = m_utility.SafeGetDecimal(reader, "betragbezahlt", 0)
						overviewData.mwstproz = m_utility.SafeGetDecimal(reader, "mwstproz", 0)
						overviewData.betragmwst = m_utility.SafeGetDecimal(reader, "betragmwst", 0)
						overviewData.betragopen = m_utility.SafeGetDecimal(reader, "betragink", 0) - m_utility.SafeGetDecimal(reader, "betragbezahlt", 0)
						overviewData.isopen = overviewData.betragopen <> 0

						overviewData.rekst1 = m_utility.SafeGetString(reader, "rekst1")
						overviewData.rekst2 = m_utility.SafeGetString(reader, "rekst2")
						overviewData.rekst = m_utility.SafeGetString(reader, "rekst")

						overviewData.reart1 = m_utility.SafeGetString(reader, "reart1")
						overviewData.reart2 = m_utility.SafeGetString(reader, "reart2")
						overviewData.zahlkond = m_utility.SafeGetString(reader, "zahlungskondition")

						overviewData.employeeadvisor = m_utility.SafeGetString(reader, "employeeadvisor")
						overviewData.customeradvisor = m_utility.SafeGetString(reader, "customeradvisor")

						overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")
						overviewData.zfiliale = m_utility.SafeGetString(reader, "zfiliale")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function

		Function GetDbCustomerRecipientOfPaymentsDataForDetails(ByVal customerNumber As Integer?) As IEnumerable(Of FoundedCustomerROPDetailData)
			Dim result As List(Of FoundedCustomerROPDetailData) = Nothing
			Dim m_utility As New Utilities

			Dim sql As String
			If customerNumber.HasValue Then
				sql = "[Get ZEData 4 Selected KD In MainView]"
			Else
				sql = "[Get ZEData 4 All KD In MainView]"
			End If

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			If customerNumber.HasValue Then listOfParams.Add(New SqlClient.SqlParameter("@KDNr", customerNumber))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedCustomerROPDetailData)

					While reader.Read()
						Dim overviewData As New FoundedCustomerROPDetailData

						overviewData.customerMDNr = m_utility.SafeGetInteger(reader, "customermdnr", 0)
						overviewData.mdnr = m_utility.SafeGetInteger(reader, "mdnr", 0)

						overviewData.zenr = m_utility.SafeGetInteger(reader, "zenr", 0)
						overviewData.renr = m_utility.SafeGetInteger(reader, "renr", 0)
						overviewData.kdnr = m_utility.SafeGetInteger(reader, "kdnr", 0)

						overviewData.firma1 = m_utility.SafeGetString(reader, "firma1")
						overviewData.firma2 = m_utility.SafeGetString(reader, "firma2")
						overviewData.firma3 = m_utility.SafeGetString(reader, "firma3")
						overviewData.abteilung = m_utility.SafeGetString(reader, "abteilung")

						overviewData.zhd = m_utility.SafeGetString(reader, "zhd")
						overviewData.postfach = m_utility.SafeGetString(reader, "postfach")
						overviewData.strasse = m_utility.SafeGetString(reader, "strasse")
						overviewData.plz = m_utility.SafeGetString(reader, "ort")
						overviewData.ort = m_utility.SafeGetString(reader, "plz")
						overviewData.plzort = String.Format("{0} {1}", m_utility.SafeGetString(reader, "plz"), m_utility.SafeGetString(reader, "ort"))


						overviewData.valutadate = m_utility.SafeGetDateTime(reader, "valutadate", Nothing)
						overviewData.buchungdate = m_utility.SafeGetDateTime(reader, "buchungsdate", Nothing)
						overviewData.fakdate = m_utility.SafeGetDateTime(reader, "fakdate", Nothing)
						overviewData.faelligdate = m_utility.SafeGetDateTime(reader, "faelligdate", Nothing)

						overviewData.einstufung = m_utility.SafeGetString(reader, "einstufung")
						overviewData.branche = m_utility.SafeGetString(reader, "branche")

						overviewData.betragink = m_utility.SafeGetDecimal(reader, "betragink", 0)
						overviewData.zebetrag = m_utility.SafeGetDecimal(reader, "zebetrag", 0)

						overviewData.mwstproz = m_utility.SafeGetDecimal(reader, "mwstproz", 0)
						overviewData.betragopen = m_utility.SafeGetDecimal(reader, "betragink", 0) - m_utility.SafeGetDecimal(reader, "betragbezahlt", 0)

						overviewData.rekst1 = m_utility.SafeGetString(reader, "rekst1")
						overviewData.rekst2 = m_utility.SafeGetString(reader, "rekst2")
						overviewData.rekst = m_utility.SafeGetString(reader, "rekst")

						overviewData.reart1 = m_utility.SafeGetString(reader, "reart1")
						overviewData.reart2 = m_utility.SafeGetString(reader, "reart2")


						overviewData.kdtelefon = m_utility.SafeGetString(reader, "kdtelefon")
						overviewData.kdtelefax = m_utility.SafeGetString(reader, "kdtelefax")
						overviewData.kdemail = m_utility.SafeGetString(reader, "kdemail")

						overviewData.employeeadvisor = m_utility.SafeGetString(reader, "employeeadvisor")
						overviewData.customeradvisor = m_utility.SafeGetString(reader, "customeradvisor")

						overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")
						overviewData.zfiliale = m_utility.SafeGetString(reader, "zfiliale")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function


#End Region




#Region "Einsatz Properties"

		Function GetDbEinsatzReportDataForProperties(ByVal ESNumber As Integer?) As IEnumerable(Of FoundedESReportDetailData)
			Dim result As List(Of FoundedESReportDetailData) = Nothing
			Dim m_utility As New Utilities

			Dim sql As String = "[Get New Top OpenRPData 4 Selected ES In MainView]"
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@ESNr", ESNumber))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedESReportDetailData)

					While reader.Read()
						Dim overviewData As New FoundedESReportDetailData

						overviewData.mdnr = m_utility.SafeGetInteger(reader, "mdnr", 0)
						overviewData.employeeMDNr = m_utility.SafeGetInteger(reader, "employeemdnr", 0)
						overviewData.customerMDNr = m_utility.SafeGetInteger(reader, "customerMDNr", 0)

						overviewData.rpnr = m_utility.SafeGetInteger(reader, "rpnr", 0)

						overviewData.periode = m_utility.SafeGetString(reader, "periode")

						overviewData.zfiliale = m_utility.SafeGetString(reader, "zFiliale")
						overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function

#End Region


#Region "Einsatz Details"

		Function GetDbEinsatzReportDataForDetails(ByVal esNumber As Integer?) As IEnumerable(Of FoundedESReportDetailData)
			Dim result As List(Of FoundedESReportDetailData) = Nothing
			Dim m_utility As New Utilities

			Dim sql As String
			If esNumber.HasValue Then
				sql = "[Get RPData 4 Selected ES In MainView]"
			Else
				sql = "[Get RPData 4 All ES In MainView]"
			End If
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			If esNumber.HasValue Then listOfParams.Add(New SqlClient.SqlParameter("@ESNr", esNumber))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedESReportDetailData)

					While reader.Read()
						Dim overviewData As New FoundedESReportDetailData

						overviewData.mdnr = m_utility.SafeGetInteger(reader, "mdnr", 0)
						overviewData.rpnr = m_utility.SafeGetInteger(reader, "rpnr", Nothing)

						overviewData.manr = m_utility.SafeGetInteger(reader, "manr", Nothing)
						overviewData.kdnr = m_utility.SafeGetInteger(reader, "kdnr", Nothing)
						overviewData.esnr = m_utility.SafeGetInteger(reader, "esnr", Nothing)
						overviewData.lonr = m_utility.SafeGetInteger(reader, "lonr", Nothing)

						overviewData.monat = m_utility.SafeGetInteger(reader, "monat", Nothing)
						overviewData.jahr = m_utility.SafeGetInteger(reader, "jahr", Nothing)
						overviewData.rpdone = m_utility.SafeGetBoolean(reader, "erfasst", Nothing)

						overviewData.employeename = m_utility.SafeGetString(reader, "maname")
						overviewData.customername = m_utility.SafeGetString(reader, "firma1")
						overviewData.rpgav_beruf = m_utility.SafeGetString(reader, "RPGAV_Beruf")

						overviewData.periode = m_utility.SafeGetString(reader, "periode")

						overviewData.zfiliale = m_utility.SafeGetString(reader, "zFiliale")
						overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function

#End Region




#Region "Vacancy Details"

		Function GetDbVacanciesProposalDataForProperties(ByVal vacancysnr As Integer?) As IEnumerable(Of FoundedVacanciesProposalDetailData)
			Dim result As List(Of FoundedVacanciesProposalDetailData) = Nothing
			Dim m_utility As New Utilities

			Dim sql As String = "[Get New Top ProposeData 4 Selected Vacancy In MainView]"
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@VakNr", vacancysnr))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedVacanciesProposalDetailData)

					While reader.Read()
						Dim overviewData As New FoundedVacanciesProposalDetailData

						overviewData.employeeMDNr = m_utility.SafeGetInteger(reader, "employeemdnr", 0)
						overviewData.customerMDNr = m_utility.SafeGetInteger(reader, "customerMDNr", 0)

						overviewData.mdnr = m_utility.SafeGetInteger(reader, "mdnr", 0)

						overviewData.pnr = m_utility.SafeGetInteger(reader, "ProposeNr", 0)
						overviewData.kdnr = m_utility.SafeGetInteger(reader, "kdnr", Nothing)
						overviewData.zhdnr = m_utility.SafeGetInteger(reader, "zhdnr", Nothing)
						overviewData.manr = m_utility.SafeGetString(reader, "manr", Nothing)

						overviewData.bezeichnung = m_utility.SafeGetString(reader, "Bezeichnung")
						overviewData.customername = m_utility.SafeGetString(reader, "firma1")
						overviewData.employeename = m_utility.SafeGetString(reader, "maname")
						overviewData.zhdname = m_utility.SafeGetString(reader, "zname")
						overviewData.p_art = m_utility.SafeGetString(reader, "p_art")
						overviewData.p_state = m_utility.SafeGetString(reader, "p_state")

						overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")
						overviewData.zfiliale = m_utility.SafeGetString(reader, "zfiliale")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function

		Function GetDbVacanciesProposalDataForDetails(ByVal vacancyNumber As Integer?) As IEnumerable(Of FoundedVacanciesProposalDetailData)
			Dim result As List(Of FoundedVacanciesProposalDetailData) = Nothing
			Dim m_utility As New Utilities

			Dim sql As String
			If vacancyNumber.HasValue Then
				sql = "[Get ProposeData 4 Selected Vacancy In MainView]"
			Else
				sql = "[Get ProposeData 4 All Vacancies In MainView]"
			End If

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			If vacancyNumber.HasValue Then listOfParams.Add(New SqlClient.SqlParameter("@VakNr", vacancyNumber))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedVacanciesProposalDetailData)

					While reader.Read()
						Dim overviewData As New FoundedVacanciesProposalDetailData

						overviewData.employeeMDNr = m_utility.SafeGetInteger(reader, "employeemdnr", 0)
						overviewData.customerMDNr = m_utility.SafeGetInteger(reader, "customerMDNr", 0)

						overviewData.mdnr = m_utility.SafeGetInteger(reader, "mdnr", 0)

						overviewData.pnr = m_utility.SafeGetInteger(reader, "ProposeNr", 0)
						overviewData.kdnr = m_utility.SafeGetInteger(reader, "kdnr", Nothing)
						overviewData.zhdnr = m_utility.SafeGetInteger(reader, "zhdnr", Nothing)
						overviewData.manr = m_utility.SafeGetString(reader, "manr", Nothing)

						overviewData.bezeichnung = m_utility.SafeGetString(reader, "Bezeichnung")
						overviewData.customername = m_utility.SafeGetString(reader, "firma1")
						overviewData.employeename = m_utility.SafeGetString(reader, "maname")
						overviewData.zhdname = m_utility.SafeGetString(reader, "zname")
						overviewData.p_art = m_utility.SafeGetString(reader, "p_art")
						overviewData.p_state = m_utility.SafeGetString(reader, "p_state")
						overviewData.advisor = m_utility.SafeGetString(reader, "Berater")

						overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")
						overviewData.zfiliale = m_utility.SafeGetString(reader, "zfiliale")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function


		Function GetDbVacanciesESDataForProperties(ByVal vacancyNumber As Integer?) As IEnumerable(Of FoundedVacancyESDetailData)
			Dim result As List(Of FoundedVacancyESDetailData) = Nothing
			Dim m_utility As New Utilities

			Dim sql As String = "[Get New Top ESData 4 Selected Vacancy In MainView]"
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@VakNr", vacancyNumber))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedVacancyESDetailData)

					While reader.Read()
						Dim overviewData As New FoundedVacancyESDetailData

						overviewData.employeeMDNr = m_utility.SafeGetInteger(reader, "employeemdnr", 0)
						overviewData.customerMDNr = m_utility.SafeGetInteger(reader, "customerMDNr", 0)

						overviewData.mdnr = CInt(m_utility.SafeGetInteger(reader, "mdnr", 0))
						overviewData.esnr = CInt(m_utility.SafeGetInteger(reader, "esnr", 0))
						overviewData.manr = CInt(m_utility.SafeGetInteger(reader, "manr", 0))
						overviewData.zhdnr = CInt(m_utility.SafeGetInteger(reader, "kdzhdnr", 0))

						overviewData.esals = m_utility.SafeGetString(reader, "es_als")
						overviewData.periode = m_utility.SafeGetString(reader, "periode")

						overviewData.employeename = m_utility.SafeGetString(reader, "maname")
						overviewData.employeename = m_utility.SafeGetString(reader, "maname")
						overviewData.zfiliale = m_utility.SafeGetString(reader, "zfiliale")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function

		Function GetDbVacanciesESDataForDetails(ByVal vacancyNumber As Integer?) As IEnumerable(Of FoundedVacancyESDetailData)
			Dim result As List(Of FoundedVacancyESDetailData) = Nothing
			Dim m_utility As New Utilities

			Dim sql As String
			If vacancyNumber.HasValue Then
				sql = "[Get ESData 4 Selected Vacancy In MainView]"
			Else
				sql = "[Get ESData 4 All Vacancies In MainView]"
			End If

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			If vacancyNumber.HasValue Then listOfParams.Add(New SqlClient.SqlParameter("@VakNr", vacancyNumber))


			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedVacancyESDetailData)

					While reader.Read()
						Dim overviewData As New FoundedVacancyESDetailData

						overviewData.employeeMDNr = m_utility.SafeGetInteger(reader, "employeemdnr", 0)
						overviewData.customerMDNr = m_utility.SafeGetInteger(reader, "customerMDNr", 0)

						overviewData.mdnr = m_utility.SafeGetInteger(reader, "mdnr", 0)
						overviewData.esnr = m_utility.SafeGetInteger(reader, "esnr", 0)
						overviewData.kdnr = m_utility.SafeGetInteger(reader, "kdnr", 0)
						overviewData.zhdnr = m_utility.SafeGetInteger(reader, "kdzhdnr", 0)
						overviewData.manr = m_utility.SafeGetInteger(reader, "manr", 0)

						overviewData.esals = m_utility.SafeGetString(reader, "es_als")
						overviewData.periode = m_utility.SafeGetString(reader, "periode")

						overviewData.customername = m_utility.SafeGetString(reader, "Firma1")
						overviewData.employeename = m_utility.SafeGetString(reader, "maname")

						overviewData.tarif = m_utility.SafeGetDecimal(reader, "tarif", Nothing)
						overviewData.stundenlohn = m_utility.SafeGetDecimal(reader, "stundenlohn", Nothing)
						overviewData.margemitbvg = m_utility.SafeGetDecimal(reader, "MargeMitBVG", Nothing)
						overviewData.margeohnebvg = m_utility.SafeGetDecimal(reader, "bruttomarge", Nothing)

						overviewData.actives = m_utility.SafeGetBoolean(reader, "actives", False)

						overviewData.zfiliale = m_utility.SafeGetString(reader, "zFiliale")
						overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function

#End Region












		Function GetDbProposalESDataForProperties(ByVal proposalnr As Integer?) As IEnumerable(Of FoundedProposalESDetailData)
			Dim result As List(Of FoundedProposalESDetailData) = Nothing
			Dim m_utility As New Utilities

			Dim sql As String
			If Not proposalnr.HasValue Then
				sql = "[Get ESData 4 All Propose In MainView]"
			Else
				sql = "[Get ESData 4 Selected Propose In MainView]"
			End If

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			If proposalnr.HasValue Then listOfParams.Add(New SqlClient.SqlParameter("@ProposeNr", proposalnr))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedProposalESDetailData)

					While reader.Read()
						Dim overviewData As New FoundedProposalESDetailData

						overviewData.employeeMDNr = m_utility.SafeGetInteger(reader, "employeemdnr", 0)
						overviewData.customerMDNr = m_utility.SafeGetInteger(reader, "customerMDNr", 0)

						overviewData.mdnr = m_utility.SafeGetInteger(reader, "mdnr", 0)
						overviewData.esnr = m_utility.SafeGetInteger(reader, "esnr", 0)
						overviewData.kdnr = m_utility.SafeGetInteger(reader, "kdnr", 0)
						overviewData.zhdnr = m_utility.SafeGetInteger(reader, "kdzhdnr", 0)
						overviewData.manr = m_utility.SafeGetInteger(reader, "manr", 0)

						overviewData.esals = m_utility.SafeGetString(reader, "es_als")
						overviewData.periode = m_utility.SafeGetString(reader, "periode")

						overviewData.customername = m_utility.SafeGetString(reader, "Firma1")
						overviewData.zhdname = m_utility.SafeGetString(reader, "zname")
						overviewData.employeename = m_utility.SafeGetString(reader, "maname")

						overviewData.tarif = m_utility.SafeGetDecimal(reader, "tarif", Nothing)
						overviewData.stundenlohn = m_utility.SafeGetDecimal(reader, "stundenlohn", Nothing)
						overviewData.margemitbvg = m_utility.SafeGetDecimal(reader, "MargeMitBVG", Nothing)
						overviewData.margeohnebvg = m_utility.SafeGetDecimal(reader, "bruttomarge", Nothing)

						overviewData.actives = m_utility.SafeGetBoolean(reader, "actives", False)

						overviewData.zfiliale = m_utility.SafeGetString(reader, "zFiliale")
						overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function

		Function GetDbProposalInterviewDataForProperties(ByVal proposalnr As Integer?) As IEnumerable(Of FoundedProposalInterviewDetailData)
			Dim result As List(Of FoundedProposalInterviewDetailData) = Nothing
			Dim m_utility As New Utilities

			Dim sql As String
			If Not proposalnr.HasValue Then
				sql = "[Get InterviewData 4 All Propose In MainView]"
			Else
				sql = "[Get InterivewData 4 Selected Propose In MainView]"
			End If

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			If proposalnr.HasValue Then listOfParams.Add(New SqlClient.SqlParameter("@ProposeNr", proposalnr))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedProposalInterviewDetailData)

					While reader.Read()
						Dim overviewData As New FoundedProposalInterviewDetailData

						overviewData.employeeMDNr = m_utility.SafeGetInteger(reader, "employeemdnr", 0)
						overviewData.customerMDNr = m_utility.SafeGetInteger(reader, "customerMDNr", 0)

						overviewData.recid = m_utility.SafeGetInteger(reader, "id", 0)
						overviewData.recnr = m_utility.SafeGetInteger(reader, "recNr", 0)

						overviewData.kdnr = m_utility.SafeGetInteger(reader, "kdnr", Nothing)
						overviewData.zhdnr = m_utility.SafeGetInteger(reader, "KDZHDNr", Nothing)
						overviewData.employeenumber = m_utility.SafeGetString(reader, "manr", Nothing)

						overviewData.datum = m_utility.SafeGetDateTime(reader, "termindate", Nothing)
						overviewData.jobtitel = m_utility.SafeGetString(reader, "jobtitel")
						overviewData.employeename = m_utility.SafeGetString(reader, "Kandidat")
						overviewData.zhdname = m_utility.SafeGetString(reader, "zname")
						overviewData.customername = m_utility.SafeGetString(reader, "firma1")
						overviewData.jstate = m_utility.SafeGetString(reader, "jstate")

						overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function

		'Function GetDbProposalContactDataForProperties(ByVal Proposalnr As Integer?, ByVal Top10 As Boolean?) As IEnumerable(Of FoundedProposalContactDetailData)
		'	Dim result As List(Of FoundedProposalContactDetailData) = Nothing
		'	Dim m_utility As New Utilities

		'	Dim sql As String

		'	If Not Top10.HasValue Then
		'		If Not Proposalnr.HasValue Then
		'			sql = "[Get KontaktData 4 All Propose In MainView]"
		'		Else
		'			sql = "[Get KontaktData 4 Selected Propose In MainView]"
		'		End If

		'	Else
		'		sql = "[Get New Top KontaktData 4 Selected Propose In MainView]"

		'	End If

		'	Dim listOfParams As New List(Of SqlClient.SqlParameter)
		'	If Proposalnr.HasValue Then listOfParams.Add(New SqlClient.SqlParameter("@ProposeNr", Proposalnr))

		'	Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

		'	Try

		'		If (Not reader Is Nothing) Then

		'			result = New List(Of FoundedProposalContactDetailData)

		'			While reader.Read()
		'				Dim overviewData As New FoundedProposalContactDetailData

		'				overviewData.employeeMDNr = m_utility.SafeGetInteger(reader, "employeemdnr", 0)
		'				overviewData.customerMDNr = m_utility.SafeGetInteger(reader, "customerMDNr", 0)

		'				overviewData.contactnr = m_utility.SafeGetInteger(reader, "RecNr", 0)
		'				overviewData.manr = m_utility.SafeGetInteger(reader, "manr", Nothing)
		'				overviewData.kdnr = m_utility.SafeGetInteger(reader, "kdnr", Nothing)

		'				overviewData.monat = m_utility.SafeGetInteger(reader, "contactmonth", 0)
		'				overviewData.jahr = m_utility.SafeGetInteger(reader, "contactyear", 0)

		'				overviewData.customername = m_utility.SafeGetString(reader, "firma1")
		'				overviewData.zhdname = m_utility.SafeGetString(reader, "zname")
		'				overviewData.employeename = m_utility.SafeGetString(reader, "maname")

		'				overviewData.bezeichnung = m_utility.SafeGetString(reader, "Bezeichnung")
		'				overviewData.beschreibung = m_utility.SafeGetString(reader, "Beschreibung")
		'				overviewData.datum = m_utility.SafeGetDateTime(reader, "datum", Nothing)
		'				overviewData.art = m_utility.SafeGetString(reader, "kontakttype1")

		'				overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
		'				overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")

		'				result.Add(overviewData)

		'			End While

		'		End If

		'	Catch e As Exception
		'		result = Nothing
		'		m_Logger.LogError(e.ToString())

		'	Finally
		'		m_utility.CloseReader(reader)

		'	End Try

		'	Return result
		'End Function



#Region "Report Properties"

		Function GetDbReportZGDataForProperties(ByVal reportNumber As Integer?) As IEnumerable(Of FoundedReportZGDetailData)
			Dim result As List(Of FoundedReportZGDetailData) = Nothing
			Dim m_utility As New Utilities

			Dim sql As String = "[Get New Top OpenZGData 4 Selected RP In MainView]" ' "[Get New Top OpenZGData 4 Selected MA In MainView]"
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@RPNr", reportNumber))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedReportZGDetailData)

					While reader.Read()
						Dim overviewData As New FoundedReportZGDetailData

						overviewData.mdnr = m_utility.SafeGetInteger(reader, "mdnr", 0)
						overviewData.employeeMDNr = m_utility.SafeGetInteger(reader, "employeemdnr", 0)

						overviewData.zgnr = m_utility.SafeGetInteger(reader, "zgnr", 0)

						overviewData.betrag = m_utility.SafeGetDecimal(reader, "Betrag", 0)
						overviewData.zgperiode = m_utility.SafeGetString(reader, "zgperiode")

						overviewData.zfiliale = m_utility.SafeGetString(reader, "zFiliale")
						overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function

		Function GetDbReportZGDataForDetails(ByVal reportNumber As Integer?) As IEnumerable(Of FoundedReportZGDetailData)
			Dim result As List(Of FoundedReportZGDetailData) = Nothing
			Dim m_utility As New Utilities

			Dim sql As String
			If reportNumber.HasValue Then
				sql = "[Get ZGData 4 Selected RP In MainView]"
			Else
				sql = "[Get ZGData 4 All RP In MainView]"
			End If

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			If reportNumber.HasValue Then listOfParams.Add(New SqlClient.SqlParameter("@RPNr", reportNumber))


			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedReportZGDetailData)

					While reader.Read()
						Dim overviewData As New FoundedReportZGDetailData

						overviewData.mdnr = m_utility.SafeGetInteger(reader, "mdnr", 0)
						overviewData.employeeMDNr = m_utility.SafeGetInteger(reader, "employeemdnr", 0)

						overviewData.zgnr = CInt(m_utility.SafeGetInteger(reader, "ZGNr", 0))
						overviewData.rpnr = CInt(m_utility.SafeGetInteger(reader, "rpnr", Nothing))
						overviewData.manr = CInt(m_utility.SafeGetInteger(reader, "MANr", 0))
						overviewData.lonr = CInt(m_utility.SafeGetInteger(reader, "lonr", Nothing))
						overviewData.vgnr = CInt(m_utility.SafeGetInteger(reader, "vgnr", Nothing))

						overviewData.monat = CInt(m_utility.SafeGetInteger(reader, "monat", 0))
						overviewData.jahr = CInt(m_utility.SafeGetInteger(reader, "jahr", 0))

						overviewData.zgperiode = m_utility.SafeGetString(reader, "zgperiode")

						overviewData.betrag = m_utility.SafeGetDecimal(reader, "betrag", Nothing)

						overviewData.aus_dat = m_utility.SafeGetDateTime(reader, "aus_dat", Nothing)

						overviewData.employeename = m_utility.SafeGetString(reader, "maname")

						overviewData.lanr = m_utility.SafeGetString(reader, "lanr", 0)
						overviewData.laname = m_utility.SafeGetString(reader, "laname")

						overviewData.zfiliale = m_utility.SafeGetString(reader, "zfiliale")
						overviewData.createdon = m_utility.SafeGetDateTime(reader, "CreatedOn", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "Createdfrom")

						Dim bIsOut As Boolean = m_utility.SafeGetInteger(reader, "vgnr", 0) >= 1
						Dim bIsAsLO As Boolean = m_utility.SafeGetInteger(reader, "lonr", 0) >= 1

						overviewData.isout = CBool(bIsOut)
						overviewData.isaslo = CBool(bIsAsLO)

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function


		Function GetDbReportInvoiceDataForProperties(ByVal reportNumber As Integer?) As IEnumerable(Of FoundedReportInvoiceDetailData)
			Dim result As List(Of FoundedReportInvoiceDetailData) = Nothing
			Dim m_utility As New Utilities

			Dim sql As String = "[Get New Top OpenREData 4 Selected RP In MainView]"
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@RPNr", reportNumber))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedReportInvoiceDetailData)

					While reader.Read()
						Dim overviewData As New FoundedReportInvoiceDetailData

						overviewData.mdnr = m_utility.SafeGetString(reader, "mdnr", 0)
						overviewData.customerMDNr = m_utility.SafeGetInteger(reader, "customermdnr", 0)

						overviewData.renr = CInt(m_utility.SafeGetInteger(reader, "renr", 0))
						overviewData.kdnr = CInt(m_utility.SafeGetInteger(reader, "kdnr", 0))

						overviewData.firma1 = m_utility.SafeGetString(reader, "firma1")

						overviewData.zhd = m_utility.SafeGetString(reader, "zhd")
						overviewData.plzort = String.Format("{0} {1}", m_utility.SafeGetString(reader, "plz"), m_utility.SafeGetString(reader, "ort"))

						overviewData.fbmonth = CInt(m_utility.SafeGetInteger(reader, "fakmonth", 0))

						overviewData.fakdate = m_utility.SafeGetDateTime(reader, "fakdate", Nothing)
						overviewData.faelligdate = m_utility.SafeGetDateTime(reader, "faelligdate", Nothing)

						overviewData.einstufung = m_utility.SafeGetString(reader, "einstufung")
						overviewData.branche = m_utility.SafeGetString(reader, "branche")

						overviewData.betragink = m_utility.SafeGetDecimal(reader, "betragink", 0)
						overviewData.betragex = m_utility.SafeGetDecimal(reader, "betragex", 0)
						overviewData.bezahlt = m_utility.SafeGetDecimal(reader, "betragbezahlt", 0)
						overviewData.mwstproz = m_utility.SafeGetDecimal(reader, "mwstproz", 0)
						overviewData.betragmwst = m_utility.SafeGetDecimal(reader, "betragmwst", 0)
						overviewData.betragopen = m_utility.SafeGetDecimal(reader, "betragink", 0) - m_utility.SafeGetDecimal(reader, "betragbezahlt", 0)

						overviewData.rekst1 = m_utility.SafeGetString(reader, "rekst1")
						overviewData.rekst2 = m_utility.SafeGetString(reader, "rekst2")
						overviewData.rekst = m_utility.SafeGetString(reader, "rekst")

						overviewData.reart1 = m_utility.SafeGetString(reader, "reart1")
						overviewData.reart2 = m_utility.SafeGetString(reader, "reart2")
						overviewData.zahlkond = m_utility.SafeGetString(reader, "zahlungskondition")

						overviewData.employeeadvisor = m_utility.SafeGetString(reader, "employeeadvisor")
						overviewData.customeradvisor = m_utility.SafeGetString(reader, "customeradvisor")

						overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")
						overviewData.zfiliale = m_utility.SafeGetString(reader, "zfiliale")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function

		Function GetDbReportInvoiceDataForDetails(ByVal reportNumber As Integer?) As IEnumerable(Of FoundedReportInvoiceDetailData)
			Dim result As List(Of FoundedReportInvoiceDetailData) = Nothing
			Dim m_utility As New Utilities

			Dim sql As String
			If reportNumber.HasValue Then
				sql = "[Get REData 4 Selected RP In MainView]"
			Else
				sql = "[Get REData 4 All RP In MainView]"
			End If

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			If reportNumber.HasValue Then listOfParams.Add(New SqlClient.SqlParameter("@RPNr", reportNumber))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedReportInvoiceDetailData)

					While reader.Read()
						Dim overviewData As New FoundedReportInvoiceDetailData

						overviewData.mdnr = m_utility.SafeGetString(reader, "mdnr", 0)
						overviewData.renr = CInt(m_utility.SafeGetInteger(reader, "renr", 0))
						overviewData.kdnr = CInt(m_utility.SafeGetInteger(reader, "kdnr", 0))

						overviewData.firma1 = m_utility.SafeGetString(reader, "firma1")

						overviewData.zhd = m_utility.SafeGetString(reader, "zhd")
						overviewData.plzort = String.Format("{0} {1}", m_utility.SafeGetString(reader, "plz"), m_utility.SafeGetString(reader, "ort"))

						overviewData.fbmonth = CInt(m_utility.SafeGetInteger(reader, "fakmonth", 0))

						overviewData.fakdate = m_utility.SafeGetDateTime(reader, "fakdate", Nothing)
						overviewData.faelligdate = m_utility.SafeGetDateTime(reader, "faelligdate", Nothing)

						overviewData.einstufung = m_utility.SafeGetString(reader, "einstufung")
						overviewData.branche = m_utility.SafeGetString(reader, "branche")

						overviewData.betragink = m_utility.SafeGetDecimal(reader, "betragink", 0)
						overviewData.betragex = m_utility.SafeGetDecimal(reader, "betragex", 0)
						overviewData.bezahlt = m_utility.SafeGetDecimal(reader, "betragbezahlt", 0)
						overviewData.mwstproz = m_utility.SafeGetDecimal(reader, "mwstproz", 0)
						overviewData.betragmwst = m_utility.SafeGetDecimal(reader, "betragmwst", 0)
						overviewData.betragopen = m_utility.SafeGetDecimal(reader, "betragink", 0) - m_utility.SafeGetDecimal(reader, "betragbezahlt", 0)

						overviewData.isopen = overviewData.betragopen <> 0

						overviewData.rekst1 = m_utility.SafeGetString(reader, "rekst1")
						overviewData.rekst2 = m_utility.SafeGetString(reader, "rekst2")
						overviewData.rekst = m_utility.SafeGetString(reader, "rekst")

						overviewData.reart1 = m_utility.SafeGetString(reader, "reart1")
						overviewData.reart2 = m_utility.SafeGetString(reader, "reart2")
						overviewData.zahlkond = m_utility.SafeGetString(reader, "zahlungskondition")

						overviewData.employeeadvisor = m_utility.SafeGetString(reader, "employeeadvisor")
						overviewData.customeradvisor = m_utility.SafeGetString(reader, "customeradvisor")

						overviewData.createdon = m_utility.SafeGetDateTime(reader, "CreatedOn", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "CreatedFrom")
						overviewData.zfiliale = m_utility.SafeGetString(reader, "zfiliale")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function


#End Region




		Function GetDbZGData4Show(ByVal sql As String) As IEnumerable(Of FoundedZGData)
			Dim result As List(Of FoundedZGData) = Nothing
			Dim m_utility As New Utilities

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", ModulConstants.MDData.MDNr))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_2", String.Empty))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_3", 0))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_4", ModulConstants.UserData.UserFiliale))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedZGData)

					While reader.Read()
						Dim overviewData As New FoundedZGData

						overviewData._res = m_utility.SafeGetString(reader, "0")
						overviewData.mdnr = CInt(m_utility.SafeGetInteger(reader, "MDNr", 0))
						overviewData.zgnr = CInt(m_utility.SafeGetInteger(reader, "ZGNr", 0))

						overviewData.rpnr = CInt(m_utility.SafeGetInteger(reader, "rpnr", 0))
						overviewData.manr = CInt(m_utility.SafeGetInteger(reader, "MAnr", 0))
						overviewData.vgnr = CInt(m_utility.SafeGetInteger(reader, "vgnr", 0))
						overviewData.monat = CInt(m_utility.SafeGetInteger(reader, "monat", 0))
						overviewData.jahr = CInt(m_utility.SafeGetInteger(reader, "jahr", 0))
						overviewData.lonr = CInt(m_utility.SafeGetInteger(reader, "lonr", 0))

						overviewData.zgperiode = m_utility.SafeGetString(reader, "zgperiode")

						overviewData.createdon = m_utility.SafeGetDateTime(reader, "CreatedOn", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "Createdfrom")
						overviewData.lanr = m_utility.SafeGetString(reader, "lanr", 0)
						overviewData.betrag = m_utility.SafeGetDecimal(reader, "betrag", Nothing)

						overviewData.aus_dat = m_utility.SafeGetDateTime(reader, "aus_dat", Nothing)

            overviewData.EmployeeFirstname = m_utility.SafeGetString(reader, "Firstname")
            overviewData.EmployeeLastname = m_utility.SafeGetString(reader, "Lastname")
						overviewData.employeename = String.Format("{1}, {0}", m_utility.SafeGetString(reader, "Firstname"), m_utility.SafeGetString(reader, "Lastname"))

						overviewData.Employeestreet = m_utility.SafeGetString(reader, "Employeestreet")
            overviewData.Employeepostcode = m_utility.SafeGetString(reader, "Employeepostcode")
            overviewData.Employeelocation = m_utility.SafeGetString(reader, "Employeelocation")

            overviewData.employeetelefon = m_utility.SafeGetString(reader, "matelefon")
            overviewData.employeemobile = m_utility.SafeGetString(reader, "manatel")
						overviewData.employeeemail = m_utility.SafeGetString(reader, "maemail")

						overviewData.laname = m_utility.SafeGetString(reader, "laname")
						overviewData.zfiliale = m_utility.SafeGetString(reader, "zfiliale")

						Dim bIsOut As Boolean = m_utility.SafeGetInteger(reader, "vgnr", 0) >= 1
						Dim bIsAsLO As Boolean = m_utility.SafeGetInteger(reader, "lonr", 0) >= 1

						overviewData.isout = CBool(bIsOut)
						overviewData.isaslo = CBool(bIsAsLO)


						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function

		Function GetDbSalaryData4Show(ByVal sql As String) As IEnumerable(Of FoundedSalaryData)
			Dim result As List(Of FoundedSalaryData) = Nothing
			Dim m_utility As New Utilities

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", ModulConstants.MDData.MDNr))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_2", String.Empty))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_3", 0))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_4", ModulConstants.UserData.UserFiliale))
			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)


			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedSalaryData)

					While reader.Read()
						Dim overviewData As New FoundedSalaryData

						overviewData._res = m_utility.SafeGetString(reader, "0")
						overviewData.mdnr = m_utility.SafeGetInteger(reader, "mdnr", 0)

						overviewData.lonr = m_utility.SafeGetInteger(reader, "lonr", 0)
						overviewData.manr = m_utility.SafeGetInteger(reader, "manr", 0)
						overviewData.zgnr = m_utility.SafeGetInteger(reader, "zgnr", 0)
						overviewData.lmid = m_utility.SafeGetInteger(reader, "lmid", 0)
						overviewData.vgnr = m_utility.SafeGetInteger(reader, "vgnr", 0)
						overviewData.IsComplete = m_utility.SafeGetBoolean(reader, "IsComplete", False)

						overviewData.monat = m_utility.SafeGetInteger(reader, "monat", 0)
						overviewData.jahr = m_utility.SafeGetInteger(reader, "jahr", 0)
						overviewData.loperiode = m_utility.SafeGetString(reader, "loperiode")

						overviewData.createdon = m_utility.SafeGetDateTime(reader, "erstellt am", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "erstellt durch")

						overviewData.bruttobetrag = m_utility.SafeGetDecimal(reader, "bruttolohn", Nothing)
						overviewData.zgbetrag = m_utility.SafeGetDecimal(reader, "zgbetrag", Nothing)
						overviewData.lmbetrag = m_utility.SafeGetDecimal(reader, "lmbetrag", Nothing)

						overviewData.lobetrag = m_utility.SafeGetDecimal(reader, "zgbetrag", Nothing)
						If Not overviewData.lobetrag.HasValue Then
							overviewData.lobetrag = m_utility.SafeGetDecimal(reader, "lmbetrag", Nothing)
							If Not overviewData.lobetrag.HasValue Then

							End If
						End If
						overviewData.employeename = m_utility.SafeGetString(reader, "maname")
						overviewData.employeestreet = m_utility.SafeGetString(reader, "mastrasse")
						overviewData.employeeaddress = m_utility.SafeGetString(reader, "maplzort")
						overviewData.maaddress = m_utility.SafeGetString(reader, "maaddress")
						overviewData.employeetelefon = m_utility.SafeGetString(reader, "matelefon")
						overviewData.employeemobile = m_utility.SafeGetString(reader, "manatel")
						overviewData.employeeemail = m_utility.SafeGetString(reader, "maemail")

						'overviewData.magebdat = m_utility.SafeGetDateTime(reader, "magebdat", Nothing)
						overviewData.magebdat = m_utility.SafeGetString(reader, "magebdat")
						overviewData.maalterwithdate = m_utility.SafeGetString(reader, "maalterwithdate")

						overviewData.mabewilligung = m_utility.SafeGetString(reader, "mabewilligung")
						overviewData.maqualifikation = m_utility.SafeGetString(reader, "maqualifikation")
						overviewData.tempmabild = m_utility.SafeGetString(reader, "tempmabild")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function


		Function GetDbSalaryDetailData4ShowInNavigation(ByVal iLONr As Integer) As IEnumerable(Of FoundedSalaryDetailData)
			Dim result As List(Of FoundedSalaryDetailData) = Nothing
			Dim m_utility As New Utilities
			Dim Sql As String = "[Get LOLData 4 Selected LO In MainView]"
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@LONr", iLONr))
			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, Sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedSalaryDetailData)

					While reader.Read()
						Dim overviewData As New FoundedSalaryDetailData

						overviewData.MDNr = CInt(m_utility.SafeGetInteger(reader, "MDNr", 0))

						overviewData.modulname = m_utility.SafeGetString(reader, "modulname")
						overviewData.destrpnr = CInt(m_utility.SafeGetInteger(reader, "destrpnr", 0))
						overviewData.destlmnr = CInt(m_utility.SafeGetInteger(reader, "destlmnr", 0))
						overviewData.destzgnr = CInt(m_utility.SafeGetInteger(reader, "destzgnr", 0))
						overviewData.destesnr = m_utility.SafeGetInteger(reader, "destesnr", 0)

						overviewData.lonr = CInt(m_utility.SafeGetInteger(reader, "lonr", 0))
						overviewData.lanr = m_utility.SafeGetDecimal(reader, "lanr", Nothing)
						overviewData.bezeichnung = m_utility.SafeGetString(reader, "bezeichnung")

						overviewData.betrag = m_utility.SafeGetDecimal(reader, "betrag", Nothing)


						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function

		Function LoadEmployeeDataForPrintPayroll(ByVal employeeNumber As Integer) As PayrollPrintData
			Dim result As PayrollPrintData = Nothing
			Dim m_utility As New Utilities

			Dim Sql As String = "Select SUBSTRING(Sprache, 1, 1) As Language From Mitarbeiter Where MANr = @MANr"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MANr", employeeNumber))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, Sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New PayrollPrintData

					While reader.Read()

						result.language = UCase(m_utility.SafeGetString(reader, "Language"))

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function


		Function GetDbCustomerBillsData4Show(ByVal sql As String) As IEnumerable(Of FoundedCustomerBillData)
			Dim result As List(Of FoundedCustomerBillData) = Nothing
			Dim m_utility As New Utilities

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", ModulConstants.MDData.MDNr))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_2", String.Empty))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_3", 0))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_4", ModulConstants.UserData.UserFiliale))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedCustomerBillData)

					While reader.Read()
						Dim overviewData As New FoundedCustomerBillData

						overviewData._res = m_utility.SafeGetString(reader, "0")
						overviewData.mdnr = CInt(m_utility.SafeGetInteger(reader, "ReMDNr", 0))
						overviewData.customermdnr = CInt(m_utility.SafeGetInteger(reader, "customerMDNr", 0))

						overviewData.renr = CInt(m_utility.SafeGetInteger(reader, "renr", 0))
						overviewData.kdnr = CInt(m_utility.SafeGetInteger(reader, "kdnr", 0))

						overviewData.firma1 = m_utility.SafeGetString(reader, "firma1")
						overviewData.firma2 = m_utility.SafeGetString(reader, "firma2")
						overviewData.firma3 = m_utility.SafeGetString(reader, "firma3")
						overviewData.abteilung = m_utility.SafeGetString(reader, "abteilung")

						overviewData.zhd = m_utility.SafeGetString(reader, "zhd")
						overviewData.postfach = m_utility.SafeGetString(reader, "postfach")
						overviewData.strasse = m_utility.SafeGetString(reader, "strasse")
						overviewData.plz = m_utility.SafeGetString(reader, "ort")
						overviewData.ort = m_utility.SafeGetString(reader, "plz")
						overviewData.plzort = String.Format("{0} {1}", m_utility.SafeGetString(reader, "plz"), m_utility.SafeGetString(reader, "ort"))

						overviewData.fbmonth = CInt(m_utility.SafeGetInteger(reader, "fakmonth", 0))

						overviewData.fakdate = m_utility.SafeGetDateTime(reader, "fakdate", Nothing)
						overviewData.printdate = m_utility.SafeGetDateTime(reader, "printdate", Nothing)
						overviewData.faelligdate = m_utility.SafeGetDateTime(reader, "faelligdate", Nothing)

						overviewData.einstufung = m_utility.SafeGetString(reader, "einstufung")
						overviewData.branche = m_utility.SafeGetString(reader, "branche")

						overviewData.betragink = m_utility.SafeGetDecimal(reader, "betragink", 0)
						overviewData.betragex = m_utility.SafeGetDecimal(reader, "betragex", 0)
						overviewData.bezahlt = m_utility.SafeGetDecimal(reader, "betragbezahlt", 0)
						overviewData.mwstproz = m_utility.SafeGetDecimal(reader, "mwstproz", 0)
						overviewData.betragmwst = m_utility.SafeGetDecimal(reader, "betragmwst", 0)
						overviewData.betragopen = m_utility.SafeGetDecimal(reader, "betragink", 0) - m_utility.SafeGetDecimal(reader, "betragbezahlt", 0)

						overviewData.rekst1 = m_utility.SafeGetString(reader, "rekst1")
						overviewData.rekst2 = m_utility.SafeGetString(reader, "rekst2")
						overviewData.rekst = m_utility.SafeGetString(reader, "rekst")

						overviewData.reart1 = m_utility.SafeGetString(reader, "reart1")
						overviewData.reart2 = m_utility.SafeGetString(reader, "reart2")
						overviewData.zahlkond = m_utility.SafeGetString(reader, "zahlungskondition")


						overviewData.kdtelefon = m_utility.SafeGetString(reader, "kdtelefon")
						overviewData.kdtelefax = m_utility.SafeGetString(reader, "kdtelefax")
						overviewData.kdemail = m_utility.SafeGetString(reader, "kdemail")

						overviewData.employeeadvisor = m_utility.SafeGetString(reader, "employeeadvisor")
						overviewData.customeradvisor = m_utility.SafeGetString(reader, "customeradvisor")

						overviewData.zfiliale = m_utility.SafeGetString(reader, "zfiliale")

						overviewData.createdon = m_utility.SafeGetDateTime(reader, "erstellt am", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "erstellt durch")


						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function


#Region "Invoice: Report Properties"

		Function GetDbInvoiceReportDataForProperties(ByVal invoiceNumber As Integer?) As IEnumerable(Of FoundedInvoiceReportDetailData)
			Dim result As List(Of FoundedInvoiceReportDetailData) = Nothing
			Dim m_utility As New Utilities

			Dim sql As String = "[Get Top RPData 4 Selected RE In MainView]"
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@RENr", invoiceNumber))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedInvoiceReportDetailData)

					While reader.Read()
						Dim overviewData As New FoundedInvoiceReportDetailData

						overviewData.mdnr = m_utility.SafeGetInteger(reader, "mdnr", 0)
						overviewData.employeeMDNr = m_utility.SafeGetInteger(reader, "employeemdnr", 0)
						overviewData.customerMDNr = m_utility.SafeGetInteger(reader, "customerMDNr", 0)

						overviewData.rpnr = m_utility.SafeGetInteger(reader, "rpnr", Nothing)
						overviewData.kdnr = m_utility.SafeGetInteger(reader, "kdnr", Nothing)
						overviewData.manr = m_utility.SafeGetInteger(reader, "manr", Nothing)
						overviewData.lonr = m_utility.SafeGetInteger(reader, "lonr", Nothing)

						overviewData.employeename = m_utility.SafeGetString(reader, "maname")
						overviewData.customername = m_utility.SafeGetString(reader, "firma1")

						overviewData.periode = m_utility.SafeGetString(reader, "repportperiode")
						overviewData.rpgav_beruf = m_utility.SafeGetString(reader, "RPGAV_Beruf")
						overviewData.rpdone = m_utility.SafeGetBoolean(reader, "erfasst", Nothing)

						overviewData.customeramount = m_utility.SafeGetDecimal(reader, "k_betrag", Nothing)

						overviewData.zfiliale = m_utility.SafeGetString(reader, "zFiliale")
						overviewData.createdon = m_utility.SafeGetDateTime(reader, "Changedon", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "ChangedFrom")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function

#End Region


#Region "Invoice Details"

		Function GetDbInvoiceReportDataForDetails(ByVal invoiceNumber As Integer?) As IEnumerable(Of FoundedInvoiceReportDetailData)
			Dim result As List(Of FoundedInvoiceReportDetailData) = Nothing
			Dim m_utility As New Utilities

			Dim sql As String
			If invoiceNumber.HasValue Then
				sql = "[Get RPData 4 Selected RE In MainView]"
			Else
				sql = "[Get RPData 4 All RE In MainView]"
			End If
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			If invoiceNumber.HasValue Then listOfParams.Add(New SqlClient.SqlParameter("@RENr", invoiceNumber))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedInvoiceReportDetailData)

					While reader.Read()
						Dim overviewData As New FoundedInvoiceReportDetailData

						overviewData.mdnr = m_utility.SafeGetInteger(reader, "mdnr", 0)
						overviewData.employeeMDNr = m_utility.SafeGetInteger(reader, "employeemdnr", 0)
						overviewData.customerMDNr = m_utility.SafeGetInteger(reader, "customerMDNr", 0)

						overviewData.rpnr = m_utility.SafeGetInteger(reader, "rpnr", Nothing)
						overviewData.kdnr = m_utility.SafeGetInteger(reader, "kdnr", Nothing)
						overviewData.manr = m_utility.SafeGetInteger(reader, "manr", Nothing)
						overviewData.lonr = m_utility.SafeGetInteger(reader, "lonr", Nothing)

						overviewData.monat = m_utility.SafeGetInteger(reader, "monat", Nothing)
						overviewData.jahr = m_utility.SafeGetInteger(reader, "jahr", Nothing)

						overviewData.employeename = m_utility.SafeGetString(reader, "maname")
						overviewData.customername = m_utility.SafeGetString(reader, "firma1")

						overviewData.periode = m_utility.SafeGetString(reader, "repportperiode")
						overviewData.rpgav_beruf = m_utility.SafeGetString(reader, "RPGAV_Beruf")
						overviewData.rpdone = m_utility.SafeGetBoolean(reader, "erfasst", Nothing)

						overviewData.customeramount = m_utility.SafeGetDecimal(reader, "k_betrag", Nothing)

						overviewData.zfiliale = m_utility.SafeGetString(reader, "zFiliale")
						overviewData.createdon = m_utility.SafeGetDateTime(reader, "Changedon", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "ChangedFrom")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function

		Function GetDbInvoiceRecipientOfPaymentsDataForProperties(ByVal invoiceNumber As Integer?) As IEnumerable(Of FoundedCustomerROPDetailData)
			Dim result As List(Of FoundedCustomerROPDetailData) = Nothing
			Dim m_utility As New Utilities

			Dim sql As String = "[Get Top ZEData 4 Selected RE In MainView]"
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@RENr", invoiceNumber))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedCustomerROPDetailData)

					While reader.Read()
						Dim overviewData As New FoundedCustomerROPDetailData

						overviewData.customerMDNr = m_utility.SafeGetString(reader, "customermdnr", 0)
						overviewData.mdnr = m_utility.SafeGetString(reader, "mdnr", 0)
						overviewData.zenr = m_utility.SafeGetInteger(reader, "zenr", 0)
						overviewData.renr = m_utility.SafeGetInteger(reader, "renr", 0)
						overviewData.kdnr = m_utility.SafeGetInteger(reader, "kdnr", 0)

						overviewData.firma1 = m_utility.SafeGetString(reader, "firma1")
						overviewData.firma2 = m_utility.SafeGetString(reader, "firma2")
						overviewData.firma3 = m_utility.SafeGetString(reader, "firma3")
						overviewData.abteilung = m_utility.SafeGetString(reader, "abteilung")

						overviewData.zhd = m_utility.SafeGetString(reader, "zhd")
						overviewData.postfach = m_utility.SafeGetString(reader, "postfach")
						overviewData.strasse = m_utility.SafeGetString(reader, "strasse")
						overviewData.plz = m_utility.SafeGetString(reader, "ort")
						overviewData.ort = m_utility.SafeGetString(reader, "plz")
						overviewData.plzort = String.Format("{0} {1}", m_utility.SafeGetString(reader, "plz"), m_utility.SafeGetString(reader, "ort"))


						overviewData.valutadate = m_utility.SafeGetDateTime(reader, "valutadate", Nothing)
						overviewData.buchungdate = m_utility.SafeGetDateTime(reader, "buchungsdate", Nothing)
						overviewData.fakdate = m_utility.SafeGetDateTime(reader, "fakdate", Nothing)
						overviewData.faelligdate = m_utility.SafeGetDateTime(reader, "faelligdate", Nothing)

						overviewData.einstufung = m_utility.SafeGetString(reader, "einstufung")
						overviewData.branche = m_utility.SafeGetString(reader, "branche")

						overviewData.betragink = m_utility.SafeGetDecimal(reader, "betragink", 0)
						overviewData.zebetrag = m_utility.SafeGetDecimal(reader, "zebetrag", 0)

						overviewData.mwstproz = m_utility.SafeGetDecimal(reader, "mwstproz", 0)
						overviewData.betragopen = m_utility.SafeGetDecimal(reader, "betragink", 0) - m_utility.SafeGetDecimal(reader, "betragbezahlt", 0)

						overviewData.rekst1 = m_utility.SafeGetString(reader, "rekst1")
						overviewData.rekst2 = m_utility.SafeGetString(reader, "rekst2")
						overviewData.rekst = m_utility.SafeGetString(reader, "rekst")

						overviewData.reart1 = m_utility.SafeGetString(reader, "reart1")
						overviewData.reart2 = m_utility.SafeGetString(reader, "reart2")


						overviewData.kdtelefon = m_utility.SafeGetString(reader, "kdtelefon")
						overviewData.kdtelefax = m_utility.SafeGetString(reader, "kdtelefax")
						overviewData.kdemail = m_utility.SafeGetString(reader, "kdemail")

						overviewData.employeeadvisor = m_utility.SafeGetString(reader, "employeeadvisor")
						overviewData.customeradvisor = m_utility.SafeGetString(reader, "customeradvisor")

						overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")
						overviewData.zfiliale = m_utility.SafeGetString(reader, "zfiliale")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function


		Function GetDbInvoiceRecipientOfPaymentsDataForDetails(ByVal invoiceNumber As Integer?) As IEnumerable(Of FoundedCustomerROPDetailData)
			Dim result As List(Of FoundedCustomerROPDetailData) = Nothing
			Dim m_utility As New Utilities

			Dim sql As String
			If invoiceNumber.HasValue Then
				sql = "[Get ZEData 4 Selected RE In MainView]"
			Else
				sql = "[Get ZEData 4 All RE In MainView]"
			End If
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			If invoiceNumber.HasValue Then listOfParams.Add(New SqlClient.SqlParameter("@RENr", invoiceNumber))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedCustomerROPDetailData)

					While reader.Read()
						Dim overviewData As New FoundedCustomerROPDetailData

						overviewData.customerMDNr = m_utility.SafeGetInteger(reader, "customermdnr", 0)
						overviewData.mdnr = m_utility.SafeGetInteger(reader, "mdnr", 0)
						overviewData.zenr = m_utility.SafeGetInteger(reader, "zenr", 0)
						overviewData.renr = m_utility.SafeGetInteger(reader, "renr", 0)
						overviewData.kdnr = m_utility.SafeGetInteger(reader, "kdnr", 0)

						overviewData.firma1 = m_utility.SafeGetString(reader, "firma1")
						overviewData.firma2 = m_utility.SafeGetString(reader, "firma2")
						overviewData.firma3 = m_utility.SafeGetString(reader, "firma3")
						overviewData.abteilung = m_utility.SafeGetString(reader, "abteilung")

						overviewData.zhd = m_utility.SafeGetString(reader, "zhd")
						overviewData.postfach = m_utility.SafeGetString(reader, "postfach")
						overviewData.strasse = m_utility.SafeGetString(reader, "strasse")
						overviewData.plz = m_utility.SafeGetString(reader, "ort")
						overviewData.ort = m_utility.SafeGetString(reader, "plz")
						overviewData.plzort = String.Format("{0} {1}", m_utility.SafeGetString(reader, "plz"), m_utility.SafeGetString(reader, "ort"))


						overviewData.valutadate = m_utility.SafeGetDateTime(reader, "valutadate", Nothing)
						overviewData.buchungdate = m_utility.SafeGetDateTime(reader, "buchungsdate", Nothing)
						overviewData.fakdate = m_utility.SafeGetDateTime(reader, "fakdate", Nothing)
						overviewData.faelligdate = m_utility.SafeGetDateTime(reader, "faelligdate", Nothing)

						overviewData.einstufung = m_utility.SafeGetString(reader, "einstufung")
						overviewData.branche = m_utility.SafeGetString(reader, "branche")

						overviewData.betragink = m_utility.SafeGetDecimal(reader, "betragink", 0)
						overviewData.zebetrag = m_utility.SafeGetDecimal(reader, "zebetrag", 0)

						overviewData.mwstproz = m_utility.SafeGetDecimal(reader, "mwstproz", 0)
						overviewData.betragopen = m_utility.SafeGetDecimal(reader, "betragink", 0) - m_utility.SafeGetDecimal(reader, "betragbezahlt", 0)

						overviewData.rekst1 = m_utility.SafeGetString(reader, "rekst1")
						overviewData.rekst2 = m_utility.SafeGetString(reader, "rekst2")
						overviewData.rekst = m_utility.SafeGetString(reader, "rekst")

						overviewData.reart1 = m_utility.SafeGetString(reader, "reart1")
						overviewData.reart2 = m_utility.SafeGetString(reader, "reart2")


						overviewData.kdtelefon = m_utility.SafeGetString(reader, "kdtelefon")
						overviewData.kdtelefax = m_utility.SafeGetString(reader, "kdtelefax")
						overviewData.kdemail = m_utility.SafeGetString(reader, "kdemail")

						overviewData.employeeadvisor = m_utility.SafeGetString(reader, "employeeadvisor")
						overviewData.customeradvisor = m_utility.SafeGetString(reader, "customeradvisor")

						overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "createdfrom")
						overviewData.zfiliale = m_utility.SafeGetString(reader, "zfiliale")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function

#End Region








		Function GetDbCustomerRecipientOfPaymentsData4Show(ByVal sql As String) As IEnumerable(Of FoundedCustomerROPData)
			Dim result As List(Of FoundedCustomerROPData) = Nothing
			Dim m_utility As New Utilities

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", ModulConstants.MDData.MDNr))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_2", String.Empty))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_3", 0))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_4", ModulConstants.UserData.UserFiliale))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedCustomerROPData)

					While reader.Read()
						Dim overviewData As New FoundedCustomerROPData

						overviewData._res = m_utility.SafeGetString(reader, "0")
						overviewData.mdnr = CInt(m_utility.SafeGetInteger(reader, "remdnr", 0))
						overviewData.customermdnr = CInt(m_utility.SafeGetInteger(reader, "customermdnr", 0))

						overviewData.zenr = CInt(m_utility.SafeGetInteger(reader, "zenr", 0))
						overviewData.renr = CInt(m_utility.SafeGetInteger(reader, "renr", 0))
						overviewData.kdnr = CInt(m_utility.SafeGetInteger(reader, "kdnr", 0))

						overviewData.firma1 = m_utility.SafeGetString(reader, "firma1")
						overviewData.firma2 = m_utility.SafeGetString(reader, "firma2")
						overviewData.firma3 = m_utility.SafeGetString(reader, "firma3")
						overviewData.abteilung = m_utility.SafeGetString(reader, "abteilung")

						overviewData.zhd = m_utility.SafeGetString(reader, "zhd")
						overviewData.postfach = m_utility.SafeGetString(reader, "postfach")
						overviewData.strasse = m_utility.SafeGetString(reader, "strasse")
						overviewData.plz = m_utility.SafeGetString(reader, "ort")
						overviewData.ort = m_utility.SafeGetString(reader, "plz")
						overviewData.plzort = String.Format("{0} {1}", m_utility.SafeGetString(reader, "plz"), m_utility.SafeGetString(reader, "ort"))


						overviewData.valutadate = m_utility.SafeGetDateTime(reader, "valutadate", Nothing)
						overviewData.buchungdate = m_utility.SafeGetDateTime(reader, "buchungsdate", Nothing)
						overviewData.fakdate = m_utility.SafeGetDateTime(reader, "fakdate", Nothing)
						overviewData.faelligdate = m_utility.SafeGetDateTime(reader, "faelligdate", Nothing)

						overviewData.einstufung = m_utility.SafeGetString(reader, "einstufung")
						overviewData.branche = m_utility.SafeGetString(reader, "branche")

						overviewData.betragink = m_utility.SafeGetDecimal(reader, "betragink", 0)
						overviewData.zebetrag = m_utility.SafeGetDecimal(reader, "zebetrag", 0)

						overviewData.mwstproz = m_utility.SafeGetDecimal(reader, "mwstproz", 0)
						overviewData.betragopen = m_utility.SafeGetDecimal(reader, "betragink", 0) - m_utility.SafeGetDecimal(reader, "betragbezahlt", 0)


						overviewData.rekst1 = m_utility.SafeGetString(reader, "rekst1")
						overviewData.rekst2 = m_utility.SafeGetString(reader, "rekst2")
						overviewData.rekst = m_utility.SafeGetString(reader, "rekst")

						overviewData.reart1 = m_utility.SafeGetString(reader, "reart1")
						overviewData.reart2 = m_utility.SafeGetString(reader, "reart2")


						overviewData.kdtelefon = m_utility.SafeGetString(reader, "kdtelefon")
						overviewData.kdtelefax = m_utility.SafeGetString(reader, "kdtelefax")
						overviewData.kdemail = m_utility.SafeGetString(reader, "kdemail")

						overviewData.employeeadvisor = m_utility.SafeGetString(reader, "employeeadvisor")
						overviewData.customeradvisor = m_utility.SafeGetString(reader, "customeradvisor")

						overviewData.zfiliale = m_utility.SafeGetString(reader, "zfiliale")

						overviewData.createdon = m_utility.SafeGetDateTime(reader, "erstellt am", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "erstellt durch")


						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function


		Function GetDbCustomerCreditBillsData4Show(ByVal sql As String) As IEnumerable(Of FoundedCustomerCreditBillData)
			Dim result As List(Of FoundedCustomerCreditBillData) = Nothing
			Dim m_utility As New Utilities

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", ModulConstants.MDData.MDNr))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_2", String.Empty))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_3", 0))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_4", ModulConstants.UserData.UserFiliale))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedCustomerCreditBillData)

					While reader.Read()
						Dim overviewData As New FoundedCustomerCreditBillData

						overviewData._res = m_utility.SafeGetString(reader, "0")
						overviewData.renr = CInt(m_utility.SafeGetInteger(reader, "renr", 0))
						overviewData.kdnr = CInt(m_utility.SafeGetInteger(reader, "kdnr", 0))

						overviewData.firma1 = m_utility.SafeGetString(reader, "firma1")
						overviewData.firma2 = m_utility.SafeGetString(reader, "firma2")
						overviewData.firma3 = m_utility.SafeGetString(reader, "firma3")
						overviewData.abteilung = m_utility.SafeGetString(reader, "abteilung")

						overviewData.zhd = m_utility.SafeGetString(reader, "zhd")
						overviewData.postfach = m_utility.SafeGetString(reader, "postfach")
						overviewData.strasse = m_utility.SafeGetString(reader, "strasse")
						overviewData.plz = m_utility.SafeGetString(reader, "ort")
						overviewData.ort = m_utility.SafeGetString(reader, "plz")
						overviewData.plzort = String.Format("{0} {1}", m_utility.SafeGetString(reader, "plz"), m_utility.SafeGetString(reader, "ort"))

						overviewData.fbmonth = CInt(m_utility.SafeGetInteger(reader, "fakmonth", 0))

						overviewData.fakdate = m_utility.SafeGetDateTime(reader, "fakdate", Nothing)
						overviewData.printdate = m_utility.SafeGetDateTime(reader, "printdate", Nothing)
						overviewData.faelligdate = m_utility.SafeGetDateTime(reader, "faelligdate", Nothing)
						overviewData.buchungdate = m_utility.SafeGetDateTime(reader, "Gebuchtam", Nothing)

						overviewData.einstufung = m_utility.SafeGetString(reader, "einstufung")
						overviewData.branche = m_utility.SafeGetString(reader, "branche")

						overviewData.betragink = m_utility.SafeGetDecimal(reader, "betragink", 0)
						overviewData.betragex = m_utility.SafeGetDecimal(reader, "betragex", 0)
						overviewData.bezahlt = m_utility.SafeGetDecimal(reader, "betragbezahlt", 0)
						overviewData.mwstproz = m_utility.SafeGetDecimal(reader, "mwstproz", 0)
						overviewData.betragmwst = m_utility.SafeGetDecimal(reader, "betragmwst", 0)
						overviewData.betragopen = m_utility.SafeGetDecimal(reader, "betragink", 0) - m_utility.SafeGetDecimal(reader, "betragbezahlt", 0)
						overviewData.isdown = Not overviewData.buchungdate Is Nothing

						overviewData.rekst1 = m_utility.SafeGetString(reader, "rekst1")
						overviewData.rekst2 = m_utility.SafeGetString(reader, "rekst2")
						overviewData.rekst = m_utility.SafeGetString(reader, "rekst")

						overviewData.reart1 = m_utility.SafeGetString(reader, "reart1")
						overviewData.reart2 = m_utility.SafeGetString(reader, "reart2")
						overviewData.zahlkond = m_utility.SafeGetString(reader, "zahlungskondition")


						overviewData.kdtelefon = m_utility.SafeGetString(reader, "kdtelefon")
						overviewData.kdtelefax = m_utility.SafeGetString(reader, "kdtelefax")
						overviewData.kdemail = m_utility.SafeGetString(reader, "kdemail")

						overviewData.employeeadvisor = m_utility.SafeGetString(reader, "employeeadvisor")
						overviewData.customeradvisor = m_utility.SafeGetString(reader, "customeradvisor")

						overviewData.zfiliale = m_utility.SafeGetString(reader, "zfiliale")

						overviewData.createdon = m_utility.SafeGetDateTime(reader, "erstellt am", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "erstellt durch")


						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function

		Function GetDbCustomerreminderData4Show(ByVal sql As String) As IEnumerable(Of FoundedCustomerreminderData)
			Dim result As List(Of FoundedCustomerreminderData) = Nothing
			Dim m_utility As New Utilities

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", ModulConstants.MDData.MDNr))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_2", String.Empty))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_3", 0))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_4", ModulConstants.UserData.UserFiliale))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedCustomerreminderData)

					While reader.Read()
						Dim overviewData As New FoundedCustomerreminderData

						overviewData._res = m_utility.SafeGetString(reader, "0")
						overviewData.renr = CInt(m_utility.SafeGetInteger(reader, "renr", 0))
						overviewData.kdnr = CInt(m_utility.SafeGetInteger(reader, "kdnr", 0))

						overviewData.firma1 = m_utility.SafeGetString(reader, "firma1")
						overviewData.firma2 = m_utility.SafeGetString(reader, "firma2")
						overviewData.firma3 = m_utility.SafeGetString(reader, "firma3")
						overviewData.abteilung = m_utility.SafeGetString(reader, "abteilung")

						overviewData.zhd = m_utility.SafeGetString(reader, "zhd")
						overviewData.postfach = m_utility.SafeGetString(reader, "postfach")
						overviewData.strasse = m_utility.SafeGetString(reader, "strasse")
						overviewData.plz = m_utility.SafeGetString(reader, "ort")
						overviewData.ort = m_utility.SafeGetString(reader, "plz")
						overviewData.plzort = String.Format("{0} {1}", m_utility.SafeGetString(reader, "plz"), m_utility.SafeGetString(reader, "ort"))

						overviewData.fbmonth = CInt(m_utility.SafeGetInteger(reader, "fakmonth", 0))

						overviewData.fakdate = m_utility.SafeGetDateTime(reader, "fakdate", Nothing)
						overviewData.printdate = m_utility.SafeGetDateTime(reader, "printdate", Nothing)
						overviewData.faelligdate = m_utility.SafeGetDateTime(reader, "faelligdate", Nothing)

						overviewData.reminder_0date = m_utility.SafeGetDateTime(reader, "reminder_0", Nothing)
						overviewData.reminder_1date = m_utility.SafeGetDateTime(reader, "reminder_1", Nothing)
						overviewData.reminder_2date = m_utility.SafeGetDateTime(reader, "reminder_2", Nothing)
						overviewData.reminder_3date = m_utility.SafeGetDateTime(reader, "reminder_3", Nothing)

						overviewData.einstufung = m_utility.SafeGetString(reader, "einstufung")
						overviewData.branche = m_utility.SafeGetString(reader, "branche")

						overviewData.betragink = m_utility.SafeGetDecimal(reader, "betragink", 0)
						overviewData.betragex = m_utility.SafeGetDecimal(reader, "betragex", 0)
						overviewData.bezahlt = m_utility.SafeGetDecimal(reader, "betragbezahlt", 0)
						overviewData.mwstproz = m_utility.SafeGetDecimal(reader, "mwstproz", 0)
						overviewData.betragmwst = m_utility.SafeGetDecimal(reader, "betragmwst", 0)
						overviewData.betragopen = m_utility.SafeGetDecimal(reader, "betragink", 0) - m_utility.SafeGetDecimal(reader, "betragbezahlt", 0)

						overviewData.rekst1 = m_utility.SafeGetString(reader, "rekst1")
						overviewData.rekst2 = m_utility.SafeGetString(reader, "rekst2")
						overviewData.rekst = m_utility.SafeGetString(reader, "rekst")

						overviewData.reart1 = m_utility.SafeGetString(reader, "reart1")
						overviewData.reart2 = m_utility.SafeGetString(reader, "reart2")
						overviewData.zahlkond = m_utility.SafeGetString(reader, "zahlungskondition")


						overviewData.kdtelefon = m_utility.SafeGetString(reader, "kdtelefon")
						overviewData.kdtelefax = m_utility.SafeGetString(reader, "kdtelefax")
						overviewData.kdemail = m_utility.SafeGetString(reader, "kdemail")

						overviewData.employeeadvisor = m_utility.SafeGetString(reader, "employeeadvisor")
						overviewData.customeradvisor = m_utility.SafeGetString(reader, "customeradvisor")

						overviewData.zfiliale = m_utility.SafeGetString(reader, "zfiliale")

						overviewData.createdon = m_utility.SafeGetDateTime(reader, "erstellt am", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "erstellt durch")


						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function


		Function GetDbCustomerFOPData4Show(ByVal sql As String) As IEnumerable(Of FoundedFOPData)
			Dim result As List(Of FoundedFOPData) = Nothing
			Dim m_utility As New Utilities

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", ModulConstants.MDData.MDNr))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_2", String.Empty))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_3", 0))
			listOfParams.Add(New SqlClient.SqlParameter("@Param_4", ModulConstants.UserData.UserFiliale))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FoundedFOPData)

					While reader.Read()
						Dim overviewData As New FoundedFOPData

						overviewData._res = m_utility.SafeGetString(reader, "0")
						overviewData.fopnr = CInt(m_utility.SafeGetInteger(reader, "fopnr", 0))
						overviewData.kdnr = CInt(m_utility.SafeGetInteger(reader, "kdnr", 0))
						overviewData.manr = CInt(m_utility.SafeGetInteger(reader, "manr", 0))
						overviewData.esnr = CInt(m_utility.SafeGetInteger(reader, "esnr", 0))

						overviewData.firma1 = m_utility.SafeGetString(reader, "firma1")
						overviewData.maname = m_utility.SafeGetString(reader, "maname")

						overviewData.kdbranche = m_utility.SafeGetString(reader, "kdbranche")
						overviewData.kreditnr = CInt(m_utility.SafeGetInteger(reader, "kreditnr", 0))
						overviewData.kreditname = m_utility.SafeGetString(reader, "kreditname")

						overviewData.krediton = m_utility.SafeGetDateTime(reader, "krediton", Nothing)
						overviewData.paidon = m_utility.SafeGetDateTime(reader, "paidon", Nothing)

						overviewData.esals = m_utility.SafeGetString(reader, "esals")
						overviewData.esab = m_utility.SafeGetDateTime(reader, "es_ab", Nothing)
						overviewData.esende = m_utility.SafeGetDateTime(reader, "es_ende", Nothing)

						overviewData.betragex = m_utility.SafeGetDecimal(reader, "betragex", 0)
						overviewData.betragmwst = m_utility.SafeGetDecimal(reader, "betragmwst", 0)
						overviewData.betragtotal = m_utility.SafeGetDecimal(reader, "betragtotal", 0)

						overviewData.kst3 = m_utility.SafeGetString(reader, "kst3")
						overviewData.bemerkung = m_utility.SafeGetString(reader, "bemerkung")

						overviewData.employeeadvisor = m_utility.SafeGetString(reader, "employeeadvisor")
						overviewData.customeradvisor = m_utility.SafeGetString(reader, "customeradvisor")

						overviewData.zfiliale = m_utility.SafeGetString(reader, "zfiliale")

						overviewData.createdon = m_utility.SafeGetDateTime(reader, "erstellt am", Nothing)
						overviewData.createdfrom = m_utility.SafeGetString(reader, "erstellt durch")

						result.Add(overviewData)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result
		End Function



		''' <summary>
		''' Loads context menu data for print.
		''' </summary>
		Public Function LoadContextMenu4PrintCustomerData() As IEnumerable(Of ContextMenuForPrint)

			Dim result As List(Of ContextMenuForPrint) = Nothing
			Dim m_utility As New Utilities
			Dim sql As String

			sql = "[Get ContexMenuItems 4 Print In MainView]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@ModulName", "KDPrint"))
			listOfParams.Add(New SqlClient.SqlParameter("@lang", ModulConstants.UserData.UserLanguage))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ContextMenuForPrint)

					Dim mnuBez As String = String.Empty

					While reader.Read()
						Dim mnuItems As New ContextMenuForPrint
						mnuItems.MnuName = m_utility.SafeGetString(reader, "MnuName")
						mnuBez = m_utility.SafeGetString(reader, "TranslatedValue")
						If mnuBez.StartsWith("_") Then mnuItems.MnuCaption = mnuBez.Replace("_", "") Else mnuItems.MnuCaption = mnuBez

						mnuItems.MnuGrouped = mnuBez.StartsWith("_")

						result.Add(mnuItems)

					End While
				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result

		End Function

		''' <summary>
		''' Loads context menu data for print.
		''' </summary>
		Public Function LoadContextMenu4NewCustomerData() As IEnumerable(Of ContextMenuForNew)

			Dim result As List(Of ContextMenuForNew) = Nothing
			Dim m_utility As New Utilities
			Dim sql As String

			sql = "[Get ContexMenuItems 4 Print In MainView]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@ModulName", "KDNew"))
			listOfParams.Add(New SqlClient.SqlParameter("@lang", ModulConstants.UserData.UserLanguage))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ContextMenuForNew)
					Dim mnuBez As String = String.Empty

					While reader.Read()
						Dim mnuItems As New ContextMenuForNew
						mnuItems.MnuName = m_utility.SafeGetString(reader, "MnuName")
						mnuBez = m_utility.SafeGetString(reader, "TranslatedValue")
						If mnuBez.StartsWith("_") Then mnuItems.MnuCaption = mnuBez.Replace("_", "") Else mnuItems.MnuCaption = mnuBez

						mnuItems.MnuGrouped = mnuBez.StartsWith("_")


						result.Add(mnuItems)

					End While
				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result

		End Function

		Public Function LoadContextMenu4PrintEmploymentData() As IEnumerable(Of ContextMenuForPrint)

			Dim result As List(Of ContextMenuForPrint) = Nothing
			Dim m_utility As New Utilities
			Dim sql As String

			sql = "[Get ContexMenuItems 4 Print In MainView]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@ModulName", "ESPrint"))
			listOfParams.Add(New SqlClient.SqlParameter("@lang", ModulConstants.UserData.UserLanguage))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ContextMenuForPrint)

					Dim mnuBez As String = String.Empty

					While reader.Read()
						Dim mnuItems As New ContextMenuForPrint
						mnuItems.MnuName = m_utility.SafeGetString(reader, "MnuName")
						mnuBez = m_utility.SafeGetString(reader, "TranslatedValue")
						If mnuBez.StartsWith("_") Then mnuItems.MnuCaption = mnuBez.Replace("_", "") Else mnuItems.MnuCaption = mnuBez

						mnuItems.MnuGrouped = mnuBez.StartsWith("_")

						result.Add(mnuItems)

					End While
				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result

		End Function

		Public Function LoadContextMenu4NewEmploymentData() As IEnumerable(Of ContextMenuForNew)

			Dim result As List(Of ContextMenuForNew) = Nothing
			Dim m_utility As New Utilities
			Dim sql As String

			sql = "[Get ContexMenuItems 4 NewRec In MainView]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@ModulName", "ESNew"))
			listOfParams.Add(New SqlClient.SqlParameter("@lang", ModulConstants.UserData.UserLanguage))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ContextMenuForNew)
					Dim mnuBez As String = String.Empty

					While reader.Read()
						Dim mnuItems As New ContextMenuForNew
						mnuItems.MnuName = m_utility.SafeGetString(reader, "MnuName")
						mnuBez = m_utility.SafeGetString(reader, "TranslatedValue")
						If mnuBez.StartsWith("_") Then mnuItems.MnuCaption = mnuBez.Replace("_", "") Else mnuItems.MnuCaption = mnuBez

						mnuItems.MnuGrouped = mnuBez.StartsWith("_")


						result.Add(mnuItems)

					End While
				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result

		End Function

		Public Function LoadContextMenu4PrintReportData() As IEnumerable(Of ContextMenuForPrint)

			Dim result As List(Of ContextMenuForPrint) = Nothing
			Dim m_utility As New Utilities
			Dim sql As String

			sql = "[Get ContexMenuItems 4 Print In MainView]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@ModulName", "RPPrint"))
			listOfParams.Add(New SqlClient.SqlParameter("@lang", ModulConstants.UserData.UserLanguage))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ContextMenuForPrint)

					Dim mnuBez As String = String.Empty

					While reader.Read()
						Dim mnuItems As New ContextMenuForPrint
						mnuItems.MnuName = m_utility.SafeGetString(reader, "MnuName")
						mnuBez = m_utility.SafeGetString(reader, "TranslatedValue")
						mnuItems.MnuGrouped = mnuBez.StartsWith("_") OrElse mnuBez.StartsWith("-")
						If mnuBez.StartsWith("_") OrElse mnuBez.StartsWith("-") Then mnuItems.MnuCaption = mnuBez.Remove(0, 1) Else mnuItems.MnuCaption = mnuBez

						result.Add(mnuItems)

					End While
				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result

		End Function

		Public Function LoadContextMenu4NewReportData() As IEnumerable(Of ContextMenuForNew)

			Dim result As List(Of ContextMenuForNew) = Nothing
			Dim m_utility As New Utilities
			Dim sql As String

			sql = "[Get ContexMenuItems 4 NewRec In MainView]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@ModulName", "RPNew"))
			listOfParams.Add(New SqlClient.SqlParameter("@lang", ModulConstants.UserData.UserLanguage))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ContextMenuForNew)
					Dim mnuBez As String = String.Empty

					While reader.Read()
						Dim mnuItems As New ContextMenuForNew
						mnuItems.MnuName = m_utility.SafeGetString(reader, "MnuName")
						mnuBez = m_utility.SafeGetString(reader, "TranslatedValue")
						If mnuBez.StartsWith("_") Then mnuItems.MnuCaption = mnuBez.Replace("_", "") Else mnuItems.MnuCaption = mnuBez

						mnuItems.MnuGrouped = mnuBez.StartsWith("_")


						result.Add(mnuItems)

					End While
				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				m_utility.CloseReader(reader)

			End Try

			Return result

		End Function


#Region "Helpers"

		Protected Shared Function ReplaceMissing(ByVal obj As Object, ByVal replacementObject As Object) As Object

			If (obj Is Nothing) Then
				Return replacementObject
			Else
				Return obj
			End If

		End Function

#End Region


	End Class




End Namespace

