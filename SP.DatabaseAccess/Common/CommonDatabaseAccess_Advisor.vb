
Imports SP.DatabaseAccess.Common.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language


Namespace Common


	Partial Class CommonDatabaseAccess
		Inherits DatabaseAccessBase
		Implements ICommonDatabaseAccess


		''' <summary>
		''' Loads advisor data.
		''' </summary>
		''' <returns>List of advisor data.</returns>
		Function LoadAdvisorData() As IEnumerable(Of AdvisorData) Implements ICommonDatabaseAccess.LoadAdvisorData

			Dim result As List(Of AdvisorData) = Nothing

			Dim sql As String

			sql = "[dbo].[Get DispoName For Unterzeichner]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@USNachname", ReplaceMissing("%%", String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("@Filiale1", ReplaceMissing("%%", String.Empty)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of AdvisorData)

					While reader.Read()
						Dim advisorData As New AdvisorData
						advisorData.UserNumber = SafeGetInteger(reader, "USNR", 0)
						advisorData.Firstname = SafeGetString(reader, "Vorname")
						advisorData.Lastname = SafeGetString(reader, "Nachname")
						advisorData.Salutation = SafeGetString(reader, "Anrede")
						advisorData.KST = SafeGetString(reader, "KST")
						advisorData.UserGuid = SafeGetString(reader, "Transfered_Guid")

						result.Add(advisorData)

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

		Function LoadActivatedAdvisorData() As IEnumerable(Of AdvisorData) Implements ICommonDatabaseAccess.LoadActivatedAdvisorData

			Dim result As List(Of AdvisorData) = Nothing

			Dim sql As String

			sql = "[dbo].[Load Activated Advisors Data]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@USNachname", ReplaceMissing("%%", String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("@Filiale1", ReplaceMissing("%%", String.Empty)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of AdvisorData)

					While reader.Read()
						Dim advisorData As New AdvisorData
						advisorData.UserNumber = SafeGetInteger(reader, "USNR", 0)
						advisorData.Firstname = SafeGetString(reader, "Vorname")
						advisorData.Lastname = SafeGetString(reader, "Nachname")
						advisorData.Salutation = SafeGetString(reader, "Anrede")
						advisorData.KST = SafeGetString(reader, "KST")
						advisorData.UserGuid = SafeGetString(reader, "Transfered_Guid")
						advisorData.Deactivated = False

						result.Add(advisorData)

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

		Function LoadAllAdvisorsData() As IEnumerable(Of AdvisorData) Implements ICommonDatabaseAccess.LoadAllAdvisorsData

			Dim result As List(Of AdvisorData) = Nothing

			Dim sql As String

			sql = "[dbo].[Load All Advisors Data]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@USNachname", ReplaceMissing("%%", String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("@Filiale1", ReplaceMissing("%%", String.Empty)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of AdvisorData)

					While reader.Read()
						Dim advisorData As New AdvisorData
						advisorData.UserNumber = SafeGetInteger(reader, "USNR", 0)
						advisorData.Firstname = SafeGetString(reader, "Vorname")
						advisorData.Lastname = SafeGetString(reader, "Nachname")
						advisorData.Salutation = SafeGetString(reader, "Anrede")
						advisorData.KST = SafeGetString(reader, "KST")
						advisorData.UserGuid = SafeGetString(reader, "Transfered_Guid")
						advisorData.Deactivated = SafeGetBoolean(reader, "Deactivated", False)

						result.Add(advisorData)

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
		''' Loads advisor data.
		''' </summary>
		''' <returns>List of advisor data.</returns>
		Function LoadAdvisorDataforGivenAdvisor(ByVal advisorKST As String) As AdvisorData Implements ICommonDatabaseAccess.LoadAdvisorDataforGivenAdvisor

			Dim result As AdvisorData = Nothing

			Dim sql As String

			sql = "[dbo].[Get Advisordata For Given Advisor]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@AdvisorKST", advisorKST))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New AdvisorData

					While reader.Read()
						Dim advisorData As New AdvisorData

						advisorData.UserNumber = SafeGetInteger(reader, "USNR", 0)
						advisorData.Salutation = SafeGetString(reader, "USAnrede")
						advisorData.Firstname = SafeGetString(reader, "USVorname")
						advisorData.Lastname = SafeGetString(reader, "USNachname")
						advisorData.KST = SafeGetString(reader, "KST")
						advisorData.KST1 = SafeGetString(reader, "USKST1")
						advisorData.KST2 = SafeGetString(reader, "USKST2")
						advisorData.UserFTitel = SafeGetString(reader, "USTitel_1")
						advisorData.UserSTitel = SafeGetString(reader, "USTitel_2")
						advisorData.UserLanguage = SafeGetString(reader, "USLanguage")
						If String.IsNullOrWhiteSpace(result.UserLanguage) Then
							advisorData.UserLanguage = "deutsch"
						End If

						advisorData.UserMDNr = SafeGetInteger(reader, "UserMDNr", 0)
						advisorData.UserGuid = SafeGetString(reader, "USGuid")

						advisorData.UserFiliale = SafeGetString(reader, "USFiliale")
						advisorData.UserBusinessBranch = SafeGetString(reader, "UserBusinessBranch")
						advisorData.UserTelefon = SafeGetString(reader, "USTelefon")
						advisorData.UserTelefax = SafeGetString(reader, "USTelefax")
						advisorData.UserMobile = SafeGetString(reader, "USNatel")
						advisorData.UsereMail = SafeGetString(reader, "USeMail")

						advisorData.UserMDGuid = SafeGetString(reader, "UserMDGuid")
						advisorData.UserMDName = SafeGetString(reader, "MDName")
						advisorData.UserMDName2 = SafeGetString(reader, "MDName2")
						advisorData.UserMDName3 = SafeGetString(reader, "MDName3")
						advisorData.UserMDPostfach = SafeGetString(reader, "MDPostfach")
						advisorData.UserMDStrasse = SafeGetString(reader, "MDStrasse")
						advisorData.UserMDPLZ = SafeGetString(reader, "MDPLZ")
						advisorData.UserMDOrt = SafeGetString(reader, "MDOrt")
						advisorData.UserMDLand = SafeGetString(reader, "MDLand")
						advisorData.MDCanton = SafeGetString(reader, "MD_Kanton")
						advisorData.UserMDTelefon = SafeGetString(reader, "MDTelefon")
						advisorData.UserMDDTelefon = SafeGetString(reader, "MDDTelefon")
						advisorData.UserMDTelefax = SafeGetString(reader, "MDTelefax")
						advisorData.UserMDeMail = SafeGetString(reader, "MDeMail")
						advisorData.UserMDHomepage = SafeGetString(reader, "MDHomepage")




						result = advisorData

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

		Function LoadAdvisorDataforGivenNumber(ByVal mandantNumber As Integer, ByVal advisorNumber As Integer) As AdvisorData Implements ICommonDatabaseAccess.LoadAdvisorDataforGivenNumber

			Dim result As AdvisorData = Nothing

			Dim sql As String

			sql = "[dbo].[Get Advisordata For Given Advisor Number]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mandantNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("USNr", ReplaceMissing(advisorNumber, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New AdvisorData

					While reader.Read()
						Dim advisorData As New AdvisorData
						advisorData.UserNumber = SafeGetInteger(reader, "USNR", 0)
						advisorData.Salutation = SafeGetString(reader, "USAnrede")
						advisorData.Firstname = SafeGetString(reader, "USVorname")
						advisorData.Lastname = SafeGetString(reader, "USNachname")
						advisorData.KST = SafeGetString(reader, "KST")
						advisorData.KST1 = SafeGetString(reader, "USKST1")
						advisorData.KST2 = SafeGetString(reader, "USKST2")
						advisorData.UserFTitel = SafeGetString(reader, "USTitel_1")
						advisorData.UserSTitel = SafeGetString(reader, "USTitel_2")
						advisorData.UserLanguage = SafeGetString(reader, "USLanguage")
						If String.IsNullOrWhiteSpace(result.UserLanguage) Then
							advisorData.UserLanguage = "deutsch"
						End If

						advisorData.UserMDNr = SafeGetInteger(reader, "UserMDNr", 0)
						advisorData.UserGuid = SafeGetString(reader, "USGuid")

						'advisorData.UserLoginname = SafeGetString(reader, "UserLoginname")
						'advisorData.UserLoginPassword = SafeGetString(reader, "UserLoginPassword")
						advisorData.UserFiliale = SafeGetString(reader, "USFiliale")
						advisorData.UserBusinessBranch = SafeGetString(reader, "UserBusinessBranch")
						advisorData.UserTelefon = SafeGetString(reader, "USTelefon")
						advisorData.UserTelefax = SafeGetString(reader, "USTelefax")
						advisorData.UserMobile = SafeGetString(reader, "USNatel")
						advisorData.UsereMail = SafeGetString(reader, "USeMail")

						advisorData.UserMDGuid = SafeGetString(reader, "UserMDGuid")
						advisorData.UserMDName = SafeGetString(reader, "MDName")
						advisorData.UserMDName2 = SafeGetString(reader, "MDName2")
						advisorData.UserMDName3 = SafeGetString(reader, "MDName3")
						advisorData.UserMDPostfach = SafeGetString(reader, "MDPostfach")
						advisorData.UserMDStrasse = SafeGetString(reader, "MDStrasse")
						advisorData.UserMDPLZ = SafeGetString(reader, "MDPLZ")
						advisorData.UserMDOrt = SafeGetString(reader, "MDOrt")
						advisorData.UserMDLand = SafeGetString(reader, "MDLand")
						advisorData.MDCanton = SafeGetString(reader, "MD_Kanton")
						advisorData.UserMDTelefon = SafeGetString(reader, "MDTelefon")
						advisorData.UserMDDTelefon = SafeGetString(reader, "MDDTelefon")
						advisorData.UserMDTelefax = SafeGetString(reader, "MDTelefax")
						advisorData.UserMDeMail = SafeGetString(reader, "MDeMail")
						advisorData.UserMDHomepage = SafeGetString(reader, "MDHomepage")
						advisorData.Deactivated = SafeGetBoolean(reader, "Deactivated", False)


						result = advisorData

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
		''' Loads advisor data with given guid.
		''' </summary>
		''' <returns>List of advisor data.</returns>
		Function LoadAdvisorDataforGivenGuid(ByVal advisorGuid As String) As AdvisorData Implements ICommonDatabaseAccess.LoadAdvisorDataforGivenGuid

			Dim result As AdvisorData = Nothing

			Dim sql As String

			sql = "[dbo].[Get Advisordata For Given Advisor Guid]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("AdvisorGuid", advisorGuid))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New AdvisorData

					While reader.Read()
						Dim advisorData As New AdvisorData
						advisorData.UserNumber = SafeGetInteger(reader, "USNR", 0)
						advisorData.Firstname = SafeGetString(reader, "USVorname")
						advisorData.Lastname = SafeGetString(reader, "USNachname", 0)
						advisorData.Salutation = SafeGetString(reader, "USAnrede", 0)
						advisorData.KST = SafeGetString(reader, "KST", 0)
						advisorData.KST1 = SafeGetString(reader, "USKST1", 0)
						advisorData.KST2 = SafeGetString(reader, "USKST2", 0)

						advisorData.UserMDNr = SafeGetInteger(reader, "UserMDNr", 0)
						advisorData.UserGuid = SafeGetString(reader, "USGuid")
						advisorData.Deactivated = SafeGetBoolean(reader, "Deactivated", False)

						result = advisorData

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


		Function LoadKSTDataForGivenFilial(ByVal filial As String) As IEnumerable(Of AdvisorData) Implements ICommonDatabaseAccess.LoadKSTDataForGivenFilial
			Dim result As List(Of AdvisorData) = Nothing

			Dim sql As String

			sql = "[List All KST For Given Filial]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("filiale", ReplaceMissing(filial, String.Empty)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of AdvisorData)

					While reader.Read()
						Dim currencyData As New AdvisorData

						currencyData.KST = SafeGetString(reader, "KST")


						result.Add(currencyData)

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
