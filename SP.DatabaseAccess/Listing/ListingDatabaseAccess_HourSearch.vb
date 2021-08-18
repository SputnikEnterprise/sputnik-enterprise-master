
Imports SP.DatabaseAccess.Listing.DataObjects
Imports SP.DatabaseAccess.TableSetting.DataObjects
Imports SP.DatabaseAccess.Report.DataObjects

Namespace Listing



	Partial Class ListingDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IListingDatabaseAccess


#Region "customer"

		''' <summary>
		''' Loads the Existing first property data.
		''' </summary>
		''' <returns>List of first property data.</returns>
		Public Function LoadCustomerExistingFirstPropertyData(ByVal mdNr As Integer) As IEnumerable(Of Customer.DataObjects.FirstPropertyData) Implements IListingDatabaseAccess.LoadCustomerExistingFirstPropertyData

			Dim result As List(Of Customer.DataObjects.FirstPropertyData) = Nothing

			Dim sql As String

			sql = "SELECT KD.FProperty, T.Bez_{0} Bezeichnung FROM Kunden KD "
			sql &= "Left Join Tab_KDFProperty T ON KD.FProperty = T.Bez_Value "
			sql &= "WHERE KD.FProperty <> 0 "
			sql &= "GROUP BY KD.FProperty, T.Bez_{0} "
			sql &= "ORDER BY Bez_{0} ASC"
			sql = String.Format(sql, MapLanguageToShortLanguageCode(SelectedTranslationLanguage))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of Customer.DataObjects.FirstPropertyData)

					While reader.Read()
						Dim fPropertyData As New Customer.DataObjects.FirstPropertyData
						fPropertyData.FPropertyValue = SafeGetDecimal(reader, "FProperty", Nothing)
						fPropertyData.Description = SafeGetString(reader, "Bezeichnung")

						result.Add(fPropertyData)

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
		''' Loads Existing customer contact data1.
		''' </summary>
		''' <returns>List of customer contact info data.</returns>
		Public Function LoadCustomerExistingContactInfo(ByVal mdNr As Integer) As IEnumerable(Of CustomerContactData) Implements IListingDatabaseAccess.LoadCustomerExistingContactInfo

			Dim result As List(Of CustomerContactData) = Nothing

			Dim sql As String

			sql = "SELECT KD.HowKontakt, IsNull(T.Bez_{0}, KD.HowKontakt) Bezeichnung FROM Kunden KD "
			sql &= "Left Join TAB_KDKontakt T ON KD.HowKontakt = T.Bez_Value "
			sql &= "WHERE KD.HowKontakt <> '' "
			sql &= "GROUP BY KD.HowKontakt, T.Bez_{0} "
			sql &= "ORDER BY Bez_{0} ASC"

			sql = String.Format(sql, MapLanguageToShortLanguageCode(SelectedTranslationLanguage))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CustomerContactData)

					While reader.Read()
						Dim contactInfoData As New CustomerContactData
						contactInfoData.bez_value = SafeGetString(reader, "HowKontakt")
						contactInfoData.bez_d = SafeGetString(reader, "Bezeichnung")

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
		''' Loads Existing customer state data1.
		''' </summary>
		''' <returns>List of customer state data (1).</returns>
		Public Function LoadCustomerExistingStateData1(ByVal mdNr As Integer) As IEnumerable(Of CustomerStateData) Implements IListingDatabaseAccess.LoadCustomerExistingStateData1

			Dim result As List(Of CustomerStateData) = Nothing

			Dim sql As String

			sql = "SELECT KD.KDState1, IsNull(T.Bez_{0}, KD.KDState1) Bezeichnung FROM Kunden KD "
			sql &= "Left Join TAB_KDStat T ON KD.KDState1 = T.Bez_Value "
			sql &= "WHERE KD.KDState1 <> '' "
			sql &= "GROUP BY KD.KDState1, T.Bez_{0} "
			sql &= "ORDER BY Bez_{0} ASC"

			sql = String.Format(sql, MapLanguageToShortLanguageCode(SelectedTranslationLanguage))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CustomerStateData)

					While reader.Read()
						Dim customerStateData As New CustomerStateData
						customerStateData.bez_value = SafeGetString(reader, "KDState1")
						customerStateData.bez_d = SafeGetString(reader, "Bezeichnung")

						result.Add(customerStateData)

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
		''' Loads Existing customer state data2.
		''' </summary>
		''' <returns>List of customer state data (2).</returns>
		Public Function LoadCustomerExistingStateData2(ByVal mdNr As Integer) As IEnumerable(Of CustomerStateData) Implements IListingDatabaseAccess.LoadCustomerExistingStateData2

			Dim result As List(Of CustomerStateData) = Nothing

			Dim sql As String

			sql = "SELECT KD.KDState2, IsNull(T.Bez_{0}, KD.KDState2) Bezeichnung FROM Kunden KD "
			sql &= "Left Join TAB_KDStat T ON KD.KDState2 = T.Bez_Value "
			sql &= "WHERE KD.KDState2 <> '' "
			sql &= "GROUP BY KD.KDState2, T.Bez_{0} "
			sql &= "ORDER BY Bez_{0} ASC"

			sql = String.Format(sql, MapLanguageToShortLanguageCode(SelectedTranslationLanguage))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CustomerStateData)

					While reader.Read()
						Dim customerStateData As New CustomerStateData
						customerStateData.bez_value = SafeGetString(reader, "KDState2")
						customerStateData.bez_d = SafeGetString(reader, "Bezeichnung")

						result.Add(customerStateData)

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


#End Region


#Region "Employee"

		''' <summary>
		''' Loads employee contact info data.
		''' </summary>
		''' <returns>List of employee contact info data.</returns>
		Public Function LoadEmployeeExistingContactsInfo(ByVal mdNr As Integer) As IEnumerable(Of EmployeeContactData) Implements IListingDatabaseAccess.LoadEmployeeExistingContactsInfo

			Dim result As List(Of EmployeeContactData) = Nothing

			Dim sql As String
			sql = "SELECT MA.KontaktHow, IsNull(T.Bez_{0}, MA.KontaktHow) Bezeichnung FROM MAKontakt_Komm MA "
			sql &= "Left Join TAB_MAKontakt T ON MA.KontaktHow = T.Description "
			sql &= "WHERE MA.KontaktHow <> '' "
			sql &= "GROUP BY MA.KontaktHow, T.Bez_{0} "
			sql &= "ORDER BY Bez_{0} ASC"

			sql = String.Format(sql, MapLanguageToShortLanguageCode(SelectedTranslationLanguage))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeContactData)

					While reader.Read()
						Dim contactInfoData As New EmployeeContactData
						contactInfoData.bez_value = SafeGetString(reader, "KontaktHow")
						contactInfoData.bez_d = SafeGetString(reader, "Bezeichnung")

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
		''' Loads Existing employee state data1.
		''' </summary>
		''' <returns>List of employee state data (1).</returns>
		Public Function LoadEmployeeExistingStateData1(ByVal mdNr As Integer) As IEnumerable(Of EmployeeStateData) Implements IListingDatabaseAccess.LoadEmployeeExistingStateData1

			Dim result As List(Of EmployeeStateData) = Nothing

			Dim sql As String

			sql = "SELECT MA.KStat1, IsNull(T.Bez_{0}, MA.KStat1) Bezeichnung FROM MAKontakt_Komm MA "
			sql &= "Left Join TAB_MAStat T ON MA.KStat1 = T.Description "
			sql &= "WHERE MA.KStat1 <> '' "
			sql &= "GROUP BY MA.KStat1, T.Bez_{0} "
			sql &= "ORDER BY Bez_{0} ASC"

			sql = String.Format(sql, MapLanguageToShortLanguageCode(SelectedTranslationLanguage))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeStateData)

					While reader.Read()
						Dim customerStateData As New EmployeeStateData
						customerStateData.bez_value = SafeGetString(reader, "KStat1")
						customerStateData.bez_d = SafeGetString(reader, "Bezeichnung")

						result.Add(customerStateData)

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
		''' Loads Existing employee state data2.
		''' </summary>
		''' <returns>List of employee state data (2).</returns>
		Public Function LoadEmployeeExistingStateData2(ByVal mdNr As Integer) As IEnumerable(Of EmployeeStateData) Implements IListingDatabaseAccess.LoadEmployeeExistingStateData2

			Dim result As List(Of EmployeeStateData) = Nothing

			Dim sql As String

			sql = "SELECT MA.KStat2, IsNull(T.Bez_{0}, MA.KStat2) Bezeichnung FROM MAKontakt_Komm MA "
			sql &= "Left Join TAB_MAStat2 T ON MA.KStat2 = T.Bezeichnung "
			sql &= "WHERE MA.KStat2 <> '' "
			sql &= "GROUP BY MA.KStat2, T.Bez_{0} "
			sql &= "ORDER BY Bez_{0} ASC"

			sql = String.Format(sql, MapLanguageToShortLanguageCode(SelectedTranslationLanguage))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeStateData)

					While reader.Read()
						Dim customerStateData As New EmployeeStateData
						customerStateData.bez_value = SafeGetString(reader, "KStat2")
						customerStateData.bez_d = SafeGetString(reader, "Bezeichnung")

						result.Add(customerStateData)

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
		''' Loads contact reserve data.
		''' </summary>
		''' <param name="contactReserveType">The contact reserve type.</param>
		''' <returns>List of employee contact reserve data.</returns>
		Public Function LoadEmployeeExistingContactReserveData(ByVal mdNr As Integer, ByVal contactReserveType As ContactReserveType) As IEnumerable(Of ContactReserveData) Implements IListingDatabaseAccess.LoadEmployeeExistingContactReserveData

			Dim result As List(Of ContactReserveData) = Nothing

			Dim sql As String = String.Empty

			sql = "SELECT MA.Res{1} ResValue, IsNull(T.Bez_{0}, MA.Res{1}) Bezeichnung FROM MAKontakt_Komm MA "
			sql &= "Left Join Tab_KontaktRes{1} T ON MA.Res{1} = T.Bezeichnung "
			sql &= "WHERE MA.Res{1} <> '' "
			sql &= "GROUP BY MA.Res{1}, T.Bez_{0} "
			sql &= "ORDER BY Bez_{0} ASC"

			sql = String.Format(sql, MapLanguageToShortLanguageCode(SelectedTranslationLanguage), CType(contactReserveType, Integer))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ContactReserveData)

					While reader.Read()
						Dim contactReserveData As New ContactReserveData
						contactReserveData.bez_value = SafeGetString(reader, "ResValue")
						contactReserveData.bez_d = SafeGetString(reader, "Bezeichnung")
						result.Add(contactReserveData)

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

#End Region


#Region "employment"

		''' <summary>
		''' Loads Existing employment kst1 data.
		''' </summary>
		''' <returns>List of employment kst1 data</returns>
		Public Function LoadEmploymentExistingCostCenter1Data(ByVal mdNr As Integer) As IEnumerable(Of CostCenter1Data) Implements IListingDatabaseAccess.LoadEmploymentExistingCostCenter1Data

			Dim result As List(Of CostCenter1Data) = Nothing

			Dim sql As String

			sql = "SELECT ES.ESKST1, IsNull(T.KSTBezeichnung, ES.ESKST1) Bezeichnung FROM ES "
			sql &= "Left Join Tab_Kst1 T ON ES.ESKST1 = T.KSTName "
			sql &= "WHERE ES.MDNr = @MDNr "
			sql &= "And ES.ESKST1 <> '' "
			sql &= "GROUP BY ES.ESKST1, T.KSTBezeichnung "
			sql &= "ORDER BY T.KSTBezeichnung ASC"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CostCenter1Data)

					While reader.Read()
						Dim customerStateData As New CostCenter1Data
						customerStateData.kstname = SafeGetString(reader, "ESKST1")
						customerStateData.kstbezeichnung = SafeGetString(reader, "Bezeichnung")

						result.Add(customerStateData)

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
		''' Loads Existing employment kst2 data.
		''' </summary>
		''' <returns>List of employment kst2 data</returns>
		Public Function LoadEmploymentExistingCostCenter2Data(ByVal mdNr As Integer) As IEnumerable(Of CostCenter2Data) Implements IListingDatabaseAccess.LoadEmploymentExistingCostCenter2Data

			Dim result As List(Of CostCenter2Data) = Nothing

			Dim sql As String

			sql = "SELECT ES.ESKST2, IsNull(T.KSTBezeichnung, ES.ESKST2) Bezeichnung FROM ES "
			sql &= "Left Join Tab_Kst2 T ON ES.ESKST2 = T.KSTName "
			sql &= "WHERE ES.MDNr = @MDNr "
			sql &= "And ES.ESKST2 <> '' "
			sql &= "GROUP BY ES.ESKST2, T.KSTBezeichnung "
			sql &= "ORDER BY T.KSTBezeichnung ASC"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CostCenter2Data)

					While reader.Read()
						Dim customerStateData As New CostCenter2Data
						customerStateData.kstname = SafeGetString(reader, "ESKST2")
						customerStateData.kstbezeichnung = SafeGetString(reader, "Bezeichnung")

						result.Add(customerStateData)

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
		''' Loads Existing employment advisor data.
		''' </summary>
		''' <returns>List of employment advisor data</returns>
		Public Function LoadEmploymentExistingAdvisorData(ByVal mdNr As Integer) As IEnumerable(Of Common.DataObjects.AdvisorData) Implements IListingDatabaseAccess.LoadEmploymentExistingAdvisorData

			Dim result As List(Of Common.DataObjects.AdvisorData) = Nothing

			Dim sql As String

			sql = "[List Employment Berater Data]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of Common.DataObjects.AdvisorData)

					While reader.Read()
						Dim customerStateData As New Common.DataObjects.AdvisorData
						Dim userData As String = SafeGetString(reader, "USName")
						Dim userName = userData.Split(New [Char]() {CChar("("), CChar(")")})(0).Trim

						Dim userLastName As String = userName.Split(New [Char]() {CChar(",")})(0).Trim
						Dim userFirstName As String = userName.Split(New [Char]() {CChar(",")})(1).Trim
						Dim userKST As String = userData.Split(New [Char]() {CChar("("), CChar(")")})(1).Trim

						customerStateData.KST = userKST
						customerStateData.Lastname = userLastName
						customerStateData.Firstname = userFirstName

						result.Add(customerStateData)

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
		''' Loads Existing employment eseinstufung data.
		''' </summary>
		''' <returns>List of customer state data (1).</returns>
		Public Function LoadEmploymentExistingCategorizationData(ByVal mdNr As Integer) As IEnumerable(Of ES.DataObjects.ESMng.ESCategorizationData) Implements IListingDatabaseAccess.LoadEmploymentExistingCategorizationData

			Dim result As List(Of ES.DataObjects.ESMng.ESCategorizationData) = Nothing

			Dim sql As String

			sql = "SELECT ES.Einstufung, IsNull(T.Bez_{0}, ES.Einstufung) Bezeichnung FROM ES "
			sql &= "Left Join Tab_ESEinstufung T ON ES.Einstufung = T.Bezeichnung "
			sql &= "WHERE ES.MDNr = @MDNr "
			sql &= "And ES.Einstufung <> '' "
			sql &= "GROUP BY ES.Einstufung, T.Bez_{0} "
			sql &= "ORDER BY T.Bez_{0} ASC"

			sql = String.Format(sql, MapLanguageToShortLanguageCode(SelectedTranslationLanguage))

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ES.DataObjects.ESMng.ESCategorizationData)

					While reader.Read()
						Dim customerStateData As New ES.DataObjects.ESMng.ESCategorizationData
						customerStateData.Description = SafeGetString(reader, "Einstufung")
						customerStateData.bez_d = SafeGetString(reader, "Bezeichnung")

						result.Add(customerStateData)

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
		''' Loads Existing employment eseinstufung data.
		''' </summary>
		''' <returns>List of customer state data (1).</returns>
		Public Function LoadEmploymentExistingSectorData(ByVal mdNr As Integer) As IEnumerable(Of SectorData) Implements IListingDatabaseAccess.LoadEmploymentExistingSectorData

			Dim result As List(Of SectorData) = Nothing

			Dim sql As String

			sql = "SELECT ES.ESBranche, IsNull(T.[BranchenBezeichnung {0}], ES.ESBranche) Bezeichnung FROM ES "
			sql &= "Left Join Branchen T ON ES.ESBranche = T.Branche "
			sql &= "WHERE ES.MDNr = @MDNr "
			sql &= "And ES.ESBranche <> '' "
			sql &= "GROUP BY ES.ESBranche, T.[BranchenBezeichnung {0}] "
			sql &= "ORDER BY T.[BranchenBezeichnung {0}] ASC"

			sql = String.Format(sql, MapLanguageToShortLanguageCode(SelectedTranslationLanguage))

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of SectorData)

					While reader.Read()
						Dim customerStateData As New SectorData
						customerStateData.branche = SafeGetString(reader, "ESBranche")
						customerStateData.bez_d = SafeGetString(reader, "Bezeichnung")

						result.Add(customerStateData)

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
		''' Loads Existing employment PVL data.
		''' </summary>
		''' <returns>List of customer state data (1).</returns>
		Public Function LoadEmploymentExistingPVLData(ByVal mdNr As Integer) As IEnumerable(Of ES.DataObjects.ESMng.ESSalaryData) Implements IListingDatabaseAccess.LoadEmploymentExistingPVLData

			Dim result As List(Of ES.DataObjects.ESMng.ESSalaryData) = Nothing

			Dim sql As String

			sql = "SELECT ESL.GAVNr, ESL.GAVGruppe0 FROM ESLohn ESL "
			sql &= "Left Join ES ON ESL.ESNr = ES.ESNr "
			sql &= "WHERE ES.MDNr = @MDNr "
			sql &= "And ESL.GAVNr > 10000 "
			sql &= "And ESL.AktivLODaten = 0 "
			sql &= "And ES.NoListing = 0 "
			sql &= "GROUP BY ESL.GAVNr, ESL.GAVGruppe0 "
			sql &= "ORDER BY ESL.GAVGruppe0 ASC"

			sql = String.Format(sql, MapLanguageToShortLanguageCode(SelectedTranslationLanguage))

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ES.DataObjects.ESMng.ESSalaryData)

					While reader.Read()
						Dim customerStateData As New ES.DataObjects.ESMng.ESSalaryData
						customerStateData.GAVNr = SafeGetInteger(reader, "GAVNr", Nothing)
						customerStateData.GAVGruppe0 = SafeGetString(reader, "GAVGruppe0")

						result.Add(customerStateData)

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

#End Region


#Region "searching customer hour data"

		''' <summary>
		''' search customer report hour data.
		''' </summary>
		Function SearchCustomerHoursReportlineData(ByVal mdNr As Integer, ByVal searchType As HourSearchTypeEnum, ByVal usFiliale As String, ByVal searchData As HourSearchData) As IEnumerable(Of CustomertReportHoursData) Implements IListingDatabaseAccess.SearchCustomerHoursReportlineData

			Dim result As List(Of CustomertReportHoursData) = Nothing

			Dim sql As String
			Dim numbersBuffer As String = String.Empty
			If Not searchData.Numbers Is Nothing Then
				For Each number In searchData.Numbers
					numbersBuffer = numbersBuffer & IIf(numbersBuffer <> "", ", ", "") & number
				Next
			End If

			sql = "[List Report Hours For Customer]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", ReplaceMissing(mdNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Numbers", ReplaceMissing(numbersBuffer, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("monat", ReplaceMissing(searchData.Monat, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("jahr", ReplaceMissing(searchData.Jahr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("CustomerFProperty", ReplaceMissing(searchData.CustomerFProperty, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("CustomerContact", ReplaceMissing(searchData.CustomerContact, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("CustomerState1", ReplaceMissing(searchData.CustomerState1, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("CustomerState2", ReplaceMissing(searchData.CustomerState2, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("EmployeeContact", ReplaceMissing(searchData.EmployeeContact, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("EmployeeState1", ReplaceMissing(searchData.EmployeeState1, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("EmployeeState2", ReplaceMissing(searchData.EmployeeState2, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("EmployeeReserve1", ReplaceMissing(searchData.EmployeeReserve1, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("EmployeeReserve2", ReplaceMissing(searchData.EmployeeReserve2, String.Empty)))

			listOfParams.Add(New SqlClient.SqlParameter("EmploymentKst1", ReplaceMissing(searchData.EmploymentKst1, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("EmploymentKst2", ReplaceMissing(searchData.EmploymentKst2, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("EmploymentAdvisor", ReplaceMissing(searchData.EmploymentAdvisor, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("employmentESCatetorization", ReplaceMissing(searchData.EmploymentESCategorize, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("employmentSector", ReplaceMissing(searchData.EmploymentBranch, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("employmentPVL", ReplaceMissing(searchData.EmploymentPVL, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("Filiale", "%" & ReplaceMissing(usFiliale, String.Empty) & "%"))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CustomertReportHoursData)

					While reader.Read()
						Dim data As New CustomertReportHoursData
						data.MDNr = mdNr
						data.FirstProperty = SafeGetDecimal(reader, "FProperty", Nothing)
						data.CustomerNumber = SafeGetInteger(reader, "Number", Nothing)
						data.Company1 = SafeGetString(reader, "Firma1")
						data.Company2 = SafeGetString(reader, "Firma2")
						data.Company3 = SafeGetString(reader, "Firma3")
						data.PostOfficeBox = SafeGetString(reader, "Postfach")
						data.CountryCode = SafeGetString(reader, "Land")
						data.Street = SafeGetString(reader, "Strasse")
						data.Postcode = SafeGetString(reader, "PLZ")
						data.Location = SafeGetString(reader, "Ort")
						data.Telephone = SafeGetString(reader, "Telefon")
						data.Telefax = SafeGetString(reader, "Telefax")
						data.EMail = SafeGetString(reader, "EMail")
						data.TotalHours = SafeGetDecimal(reader, "TotalAnzahl", Nothing)

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

		''' <summary>
		''' search customer report line hour data.
		''' </summary>
		Function SearchAssignedCustomerHoursReportlineData(ByVal mdNr As Integer, ByVal customerNumber As Integer, ByVal usFiliale As String, ByVal searchData As HourSearchData) As IEnumerable(Of ReportlineHoursData) Implements IListingDatabaseAccess.SearchAssignedCustomerHoursReportlineData

			Dim result As List(Of ReportlineHoursData) = Nothing

			Dim sql As String

			sql = "[List ReportLine Hours For Assigned Customer]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", ReplaceMissing(mdNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(customerNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("monat", ReplaceMissing(searchData.Monat, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("jahr", ReplaceMissing(searchData.Jahr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("CustomerFProperty", ReplaceMissing(searchData.CustomerFProperty, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("CustomerContact", ReplaceMissing(searchData.CustomerContact, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("CustomerState1", ReplaceMissing(searchData.CustomerState1, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("CustomerState2", ReplaceMissing(searchData.CustomerState2, String.Empty)))

			listOfParams.Add(New SqlClient.SqlParameter("EmploymentKst1", ReplaceMissing(searchData.EmploymentKst1, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("EmploymentKst2", ReplaceMissing(searchData.EmploymentKst2, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("EmploymentAdvisor", ReplaceMissing(searchData.EmploymentAdvisor, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("employmentESCatetorization", ReplaceMissing(searchData.EmploymentESCategorize, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("employmentSector", ReplaceMissing(searchData.EmploymentBranch, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("employmentPVL", ReplaceMissing(searchData.EmploymentPVL, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("Filiale", "%" & ReplaceMissing(usFiliale, String.Empty) & "%"))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ReportlineHoursData)

					While reader.Read()
						Dim data As New ReportlineHoursData
						data.MDNr = mdNr
						data.ReportNumber = SafeGetInteger(reader, "RPNr", Nothing)
						data.Monat = SafeGetInteger(reader, "Monat", Nothing)
						data.Jahr = SafeGetInteger(reader, "Jahr", Nothing)
						data.LANr = SafeGetDecimal(reader, "LANr", Nothing)
						data.LALOText = SafeGetString(reader, "LALoText")
						data.VonDate = SafeGetDateTime(reader, "VonDate", Nothing)
						data.BisDate = SafeGetDateTime(reader, "BisDate", Nothing)
						data.CountHour = SafeGetDecimal(reader, "K_Anzahl", Nothing)

						data.RPKst1 = SafeGetString(reader, "RPKST1")
						data.RPKst2 = SafeGetString(reader, "RPKST2")
						data.RPKst = SafeGetString(reader, "RPKST")
						data.ES_Als = SafeGetString(reader, "ES_Als")
						data.Einstufung = SafeGetString(reader, "Einstufung")
						data.ESBranche = SafeGetString(reader, "ESBranche")

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

#End Region


#Region "KDKST Data"

		Function SearchCustomerHoursReportlineEachCostcenterData(ByVal mdNr As Integer, ByVal searchType As HourSearchTypeEnum, ByVal usFiliale As String, ByVal searchData As HourSearchData) As IEnumerable(Of CustomertReportHoursData) Implements IListingDatabaseAccess.SearchCustomerHoursReportlineEachCostcenterData

			Dim result As List(Of CustomertReportHoursData) = Nothing

			Dim sql As String
			Dim numbersBuffer As String = String.Empty
			If Not searchData.Numbers Is Nothing Then
				For Each number In searchData.Numbers
					numbersBuffer = numbersBuffer & IIf(numbersBuffer <> "", ", ", "") & number
				Next
			End If
			If searchdata.CalculationType = CalculationTypeEnum.HourCalculation Then
				sql = "[List Report Hours For Customer Each Costcenter]"
			Else
				sql = "[List Report Data For Customer Each Costcenter]"
			End If

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", ReplaceMissing(mdNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Numbers", ReplaceMissing(numbersBuffer, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("monat", ReplaceMissing(searchData.Monat, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("jahr", ReplaceMissing(searchData.Jahr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("CustomerFProperty", ReplaceMissing(searchData.CustomerFProperty, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("CustomerContact", ReplaceMissing(searchData.CustomerContact, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("CustomerState1", ReplaceMissing(searchData.CustomerState1, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("CustomerState2", ReplaceMissing(searchData.CustomerState2, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("EmployeeContact", ReplaceMissing(searchData.EmployeeContact, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("EmployeeState1", ReplaceMissing(searchData.EmployeeState1, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("EmployeeState2", ReplaceMissing(searchData.EmployeeState2, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("EmployeeReserve1", ReplaceMissing(searchData.EmployeeReserve1, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("EmployeeReserve2", ReplaceMissing(searchData.EmployeeReserve2, String.Empty)))

			listOfParams.Add(New SqlClient.SqlParameter("EmploymentKst1", ReplaceMissing(searchData.EmploymentKst1, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("EmploymentKst2", ReplaceMissing(searchData.EmploymentKst2, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("EmploymentAdvisor", ReplaceMissing(searchData.EmploymentAdvisor, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("employmentESCatetorization", ReplaceMissing(searchData.EmploymentESCategorize, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("employmentSector", ReplaceMissing(searchData.EmploymentBranch, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("employmentPVL", ReplaceMissing(searchData.EmploymentPVL, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("Filiale", "%" & ReplaceMissing(usFiliale, String.Empty) & "%"))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CustomertReportHoursData)

					While reader.Read()
						Dim data As New CustomertReportHoursData
						data.MDNr = mdNr
						data.FirstProperty = SafeGetDecimal(reader, "FProperty", Nothing)
						data.CustomerNumber = SafeGetInteger(reader, "Number", Nothing)
						data.CustomerCostcenter = SafeGetInteger(reader, "KSTNr", Nothing)
						data.RPKST = SafeGetString(reader, "RPKST")
						data.Company1 = SafeGetString(reader, "Firma1")
						data.Company2 = SafeGetString(reader, "Firma2")
						data.Company3 = SafeGetString(reader, "Firma3")
						data.CostcenterName = SafeGetString(reader, "KSTBez")
						data.PostOfficeBox = SafeGetString(reader, "Postfach")
						data.CountryCode = SafeGetString(reader, "Land")
						data.Street = SafeGetString(reader, "Strasse")
						data.Postcode = SafeGetString(reader, "PLZ")
						data.Location = SafeGetString(reader, "Ort")
						data.Telephone = SafeGetString(reader, "Telefon")
						data.Telefax = SafeGetString(reader, "Telefax")
						data.EMail = SafeGetString(reader, "EMail")
						data.TotalHours = SafeGetDecimal(reader, "TotalAnzahl", Nothing)
						data.Amount = SafeGetDecimal(reader, "TotalBetrag", Nothing)
						data.LANr = SafeGetDecimal(reader, "LANr", Nothing)

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

		Function SearchAssignedCustomerHoursReportlineEachCostcenterData(ByVal mdNr As Integer, ByVal customerNumber As Integer, ByVal CustomerCostcenter As Integer,
																																		 ByVal reportKST As String, ByVal laNumber As Decimal?,
																																		 ByVal usFiliale As String, ByVal searchData As HourSearchData) As IEnumerable(Of ReportlineHoursData) Implements IListingDatabaseAccess.SearchAssignedCustomerHoursReportlineEachCostcenterData

			Dim result As List(Of ReportlineHoursData) = Nothing

			Dim sql As String

			If searchData.CalculationType = CalculationTypeEnum.HourCalculation Then
				sql = "[List ReportLine Hours For Assigned Customer Each Costcenter]"
			Else
				sql = "[List ReportLine Data For Assigned Customer Each Costcenter]"
			End If

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", ReplaceMissing(mdNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(customerNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("monat", ReplaceMissing(searchData.Monat, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("jahr", ReplaceMissing(searchData.Jahr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KSTNr", ReplaceMissing(CustomerCostcenter, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("RPKST", ReplaceMissing(reportKST, String.Empty)))
			If searchData.CalculationType = CalculationTypeEnum.AllCalculation Then
				listOfParams.Add(New SqlClient.SqlParameter("LANr", ReplaceMissing(laNumber, 0)))
			End If
			listOfParams.Add(New SqlClient.SqlParameter("CustomerFProperty", ReplaceMissing(searchData.CustomerFProperty, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("CustomerContact", ReplaceMissing(searchData.CustomerContact, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("CustomerState1", ReplaceMissing(searchData.CustomerState1, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("CustomerState2", ReplaceMissing(searchData.CustomerState2, String.Empty)))

			listOfParams.Add(New SqlClient.SqlParameter("EmploymentKst1", ReplaceMissing(searchData.EmploymentKst1, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("EmploymentKst2", ReplaceMissing(searchData.EmploymentKst2, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("EmploymentAdvisor", ReplaceMissing(searchData.EmploymentAdvisor, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("employmentESCatetorization", ReplaceMissing(searchData.EmploymentESCategorize, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("employmentSector", ReplaceMissing(searchData.EmploymentBranch, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("employmentPVL", ReplaceMissing(searchData.EmploymentPVL, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("Filiale", "%" & ReplaceMissing(usFiliale, String.Empty) & "%"))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ReportlineHoursData)

					While reader.Read()
						Dim data As New ReportlineHoursData
						data.MDNr = mdNr
						data.CustomerCostcenter = SafeGetInteger(reader, "KSTNr", Nothing)
						data.CostcenterName = SafeGetString(reader, "KSTBez")
						data.ReportNumber = SafeGetInteger(reader, "RPNr", Nothing)
						data.Monat = SafeGetInteger(reader, "Monat", Nothing)
						data.Jahr = SafeGetInteger(reader, "Jahr", Nothing)
						data.LANr = SafeGetDecimal(reader, "LANr", Nothing)
						data.LALOText = SafeGetString(reader, "LALoText")
						data.VonDate = SafeGetDateTime(reader, "VonDate", Nothing)
						data.BisDate = SafeGetDateTime(reader, "BisDate", Nothing)
						data.CountHour = SafeGetDecimal(reader, "K_Anzahl", Nothing)
						data.Amount = SafeGetDecimal(reader, "K_Betrag", Nothing)

						data.RPKst1 = SafeGetString(reader, "RPKST1")
						data.RPKst2 = SafeGetString(reader, "RPKST2")
						data.RPKst = SafeGetString(reader, "RPKST")
						data.ES_Als = SafeGetString(reader, "ES_Als")
						data.Einstufung = SafeGetString(reader, "Einstufung")
						data.ESBranche = SafeGetString(reader, "ESBranche")

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

#End Region


#Region "searching employee hour data"


		''' <summary>
		''' search employee report hour data.
		''' </summary>
		Public Function SearchEmployeeHoursReportlineData(ByVal mdNr As Integer, ByVal searchType As HourSearchTypeEnum, ByVal usFiliale As String, ByVal searchData As HourSearchData) As IEnumerable(Of EmployeeReportHoursData) Implements IListingDatabaseAccess.SearchEmployeeHoursReportlineData

			Dim result As List(Of EmployeeReportHoursData) = Nothing

			Dim sql As String
			Dim numbersBuffer As String = String.Empty
			If Not searchData.Numbers Is Nothing Then
				For Each number In searchData.Numbers
					numbersBuffer = numbersBuffer & IIf(numbersBuffer <> "", ", ", "") & number
				Next
			End If

			sql = "[List Report Hours For Employee]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", ReplaceMissing(mdNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Numbers", ReplaceMissing(numbersBuffer, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("monat", ReplaceMissing(searchData.Monat, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("jahr", ReplaceMissing(searchData.Jahr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("CustomerFProperty", ReplaceMissing(searchData.CustomerFProperty, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("CustomerContact", ReplaceMissing(searchData.CustomerContact, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("CustomerState1", ReplaceMissing(searchData.CustomerState1, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("CustomerState2", ReplaceMissing(searchData.CustomerState2, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("EmployeeContact", ReplaceMissing(searchData.EmployeeContact, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("EmployeeState1", ReplaceMissing(searchData.EmployeeState1, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("EmployeeState2", ReplaceMissing(searchData.EmployeeState2, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("EmployeeReserve1", ReplaceMissing(searchData.EmployeeReserve1, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("EmployeeReserve2", ReplaceMissing(searchData.EmployeeReserve2, String.Empty)))

			listOfParams.Add(New SqlClient.SqlParameter("EmploymentKst1", ReplaceMissing(searchData.EmploymentKst1, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("EmploymentKst2", ReplaceMissing(searchData.EmploymentKst2, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("EmploymentAdvisor", ReplaceMissing(searchData.EmploymentAdvisor, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("employmentESCatetorization", ReplaceMissing(searchData.EmploymentESCategorize, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("employmentSector", ReplaceMissing(searchData.EmploymentBranch, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("employmentPVL", ReplaceMissing(searchData.EmploymentPVL, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("Filiale", "%" & ReplaceMissing(usFiliale, String.Empty) & "%"))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeReportHoursData)

					While reader.Read()
						Dim data As New EmployeeReportHoursData
						data.MDNr = mdNr
						data.Employeenumber = SafeGetInteger(reader, "Number", Nothing)
						data.Firstname = SafeGetString(reader, "Vorname")
						data.Lastname = SafeGetString(reader, "Nachname")
						data.StayAs = SafeGetString(reader, "Wohnt_Bei")
						data.PostOfficeBox = SafeGetString(reader, "Postfach")
						data.CountryCode = SafeGetString(reader, "Land")
						data.Street = SafeGetString(reader, "Strasse")
						data.Postcode = SafeGetString(reader, "PLZ")
						data.Location = SafeGetString(reader, "Ort")
						data.Telephone_P = SafeGetString(reader, "Telefon_P")
						data.Telephone_2 = SafeGetString(reader, "Telefon2")
						data.Telephone_3 = SafeGetString(reader, "Telefon3")
						data.Mobile = SafeGetString(reader, "Natel")
						data.EMail = SafeGetString(reader, "EMail")
						data.TotalHours = SafeGetDecimal(reader, "TotalAnzahl", Nothing)

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

		''' <summary>
		''' search employee report line hour data.
		''' </summary>
		Public Function SearchAssignedEmployeeHoursReportlineData(ByVal mdNr As Integer, ByVal employeeNumber As Integer, ByVal usFiliale As String, ByVal searchData As HourSearchData) As IEnumerable(Of ReportlineHoursData) Implements IListingDatabaseAccess.SearchAssignedEmployeeHoursReportlineData

			Dim result As List(Of ReportlineHoursData) = Nothing

			Dim sql As String

			sql = "[List ReportLine Hours For Assigned Employee]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", ReplaceMissing(mdNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("MANr", ReplaceMissing(employeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("monat", ReplaceMissing(searchData.Monat, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("jahr", ReplaceMissing(searchData.Jahr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("EmployeeContact", ReplaceMissing(searchData.EmployeeContact, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("EmployeeState1", ReplaceMissing(searchData.EmployeeState1, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("EmployeeState2", ReplaceMissing(searchData.EmployeeState2, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("EmployeeReserve1", ReplaceMissing(searchData.EmployeeReserve1, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("EmployeeReserve2", ReplaceMissing(searchData.EmployeeReserve2, String.Empty)))

			listOfParams.Add(New SqlClient.SqlParameter("EmploymentKst1", ReplaceMissing(searchData.EmploymentKst1, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("EmploymentKst2", ReplaceMissing(searchData.EmploymentKst2, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("EmploymentAdvisor", ReplaceMissing(searchData.EmploymentAdvisor, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("employmentESCatetorization", ReplaceMissing(searchData.EmploymentESCategorize, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("employmentSector", ReplaceMissing(searchData.EmploymentBranch, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("employmentPVL", ReplaceMissing(searchData.EmploymentPVL, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("Filiale", "%" & ReplaceMissing(usFiliale, String.Empty) & "%"))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ReportlineHoursData)

					While reader.Read()
						Dim data As New ReportlineHoursData
						data.MDNr = mdNr
						data.ReportNumber = SafeGetInteger(reader, "RPNr", Nothing)
						data.Monat = SafeGetInteger(reader, "Monat", Nothing)
						data.Jahr = SafeGetInteger(reader, "Jahr", Nothing)
						data.LANr = SafeGetDecimal(reader, "LANr", Nothing)
						data.LALOText = SafeGetString(reader, "LALoText")
						data.VonDate = SafeGetDateTime(reader, "VonDate", Nothing)
						data.BisDate = SafeGetDateTime(reader, "BisDate", Nothing)
						data.CountHour = SafeGetDecimal(reader, "M_Anzahl", Nothing)

						data.RPKst1 = SafeGetString(reader, "RPKST1")
						data.RPKst2 = SafeGetString(reader, "RPKST2")
						data.RPKst = SafeGetString(reader, "RPKST")
						data.ES_Als = SafeGetString(reader, "ES_Als")
						data.Einstufung = SafeGetString(reader, "Einstufung")
						data.ESBranche = SafeGetString(reader, "ESBranche")

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

#End Region



#Region "search employee reports"

		''' <summary>
		''' load employee data for selection.
		''' </summary>
		Public Function LoadEmployeesForSelectionData(ByVal searchData As EmployeeSuvaSearchData) As IEnumerable(Of Employee.DataObjects.MasterdataMng.EmployeeMasterData) Implements IListingDatabaseAccess.LoadEmployeesForSelectionData

			Dim result As List(Of Employee.DataObjects.MasterdataMng.EmployeeMasterData) = Nothing

			Dim sql As String

			sql = "SELECT RPL.MANr, MA.Vorname, MA.Nachname FROM RPL "
			sql &= " LEFT JOIN RP ON RP.RPNr = RPL.RPNr "
			sql &= " LEFT JOIN Mitarbeiter MA ON MA.MANr = RPL.MANr AND MA.MANr = RP.MANr"
			sql &= " Where"
			sql &= " RP.MDNr = @MDNr "
			sql &= " AND (@Jahr = 0 OR RP.Jahr = @Jahr)"
			sql &= " AND (@Monat = 0 OR RP.Monat = @Monat)"

			sql &= " GROUP BY RPL.MANr, MA.Vorname, MA.Nachname"
			sql &= " ORDER BY MA.Nachname, MA.Vorname"



			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", ReplaceMissing(searchData.MDNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("jahr", ReplaceMissing(searchData.Jahr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Monat", ReplaceMissing(searchData.Monat, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of Employee.DataObjects.MasterdataMng.EmployeeMasterData)

					While reader.Read()
						Dim data As New Employee.DataObjects.MasterdataMng.EmployeeMasterData

						data.EmployeeNumber = SafeGetInteger(reader, "MANR", Nothing)

						data.Firstname = SafeGetString(reader, "Vorname")
						data.Lastname = SafeGetString(reader, "Nachname")


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

		''' <summary>
		''' search all employee reports data.
		''' </summary>
		Public Function LoadEmployeeAllReportData(ByVal searchData As EmployeeSuvaSearchData, ByVal usFiliale As String) As IEnumerable(Of RPMasterData) Implements IListingDatabaseAccess.LoadEmployeeAllReportData

			Dim result As List(Of RPMasterData) = Nothing

			Dim sql As String
			Dim numbersBuffer As String = String.Empty
			If Not searchData.EmployeeNumbers Is Nothing Then
				For Each number In searchData.EmployeeNumbers
					numbersBuffer = numbersBuffer & IIf(numbersBuffer <> "", ", ", "") & number
				Next
			End If

			sql = String.Format("Delete RPSuvaTage Where MDNr = @MDNr And MANr In ({0}) And Jahr = @Jahr;", numbersBuffer)
			sql &= "Select RP.[ID] "
			sql &= ",RP.[RPNR]"
			sql &= ",RP.[ESNR]"
			sql &= ",RP.[MANR]"
			sql &= ",RP.[KDNR]"
			sql &= ",RP.[LONr]"
			sql &= ",RP.[Currency]"
			sql &= ",RP.[SUVA]"
			sql &= ",RP.[Monat]"
			sql &= ",RP.[Jahr]"
			sql &= ",RP.[Von]"
			sql &= ",RP.[Bis]"
			sql &= ",RP.[Erfasst]"
			sql &= ",RP.[RPKST]"
			sql &= ",RP.[RPKST1]"
			sql &= ",RP.[RPKST2]"
			sql &= ",RP.[PrintedWeeks]"
			sql &= ",RP.[PrintedDate]"
			sql &= ",RP.[Far-pflicht]"
			sql &= ",RP.[CreatedFrom]"
			sql &= ",RP.[CreatedOn]"
			sql &= ",RP.[BVGCode]"
			sql &= ",RP.[RPGAV_FAG]"
			sql &= ",RP.[RPGAV_FAN]"
			sql &= ",RP.[RPGAV_WAG]"
			sql &= ",RP.[RPGAV_WAN]"
			sql &= ",RP.[RPGAV_VAG]"
			sql &= ",RP.[RPGAV_VAN]"
			sql &= ",RP.[RPGAV_Nr]"
			sql &= ",RP.[RPGAV_Kanton]"
			sql &= ",RP.[RPGAV_Beruf]"
			sql &= ",RP.[RPGAV_Gruppe1]"
			sql &= ",RP.[RPGAV_Gruppe2]"
			sql &= ",RP.[RPGAV_Gruppe3]"
			sql &= ",RP.[RPGAV_Text]"
			sql &= ",RP.[RPGAV_StdWeek]"
			sql &= ",RP.[RPGAV_StdMonth]"
			sql &= ",RP.[RPGAV_StdYear]"
			sql &= ",RP.[RPGAV_FAG_M]"
			sql &= ",RP.[RPGAV_FAN_M]"
			sql &= ",RP.[RPGAV_VAG_M]"
			sql &= ",RP.[RPGAV_VAN_M]"
			sql &= ",RP.[RPGAV_WAG_M]"
			sql &= ",RP.[RPGAV_WAN_M]"
			sql &= ",RP.[RPGAV_FAG_S]"
			sql &= ",RP.[RPGAV_FAN_S]"
			sql &= ",RP.[RPGAV_VAG_S]"
			sql &= ",RP.[RPGAV_VAN_S]"
			sql &= ",RP.[RPGAV_WAG_S]"
			sql &= ",RP.[RPGAV_WAN_S]"
			sql &= ",RP.[RPGAV_FAG_J]"
			sql &= ",RP.[RPGAV_FAN_J]"
			sql &= ",RP.[RPGAV_VAG_J]"
			sql &= ",RP.[RPGAV_VAN_J]"
			sql &= ",RP.[RPGAV_WAG_J]"
			sql &= ",RP.[RPGAV_WAN_J]"
			sql &= ",RP.[ES_Einstufung]"
			sql &= ",RP.[KDBranche]"
			sql &= ",RP.[ProposeNr]"
			sql &= ",RP.[RPDoc_Guid]"
			sql &= ",RP.[MDNr]"
			sql &= ",(SELECT COUNT(*) FROM MonthClose WHERE Monat = [RP].[Monat] AND Jahr = CAST([RP].[Jahr] as int) And MDNr = [RP].[MDNr]) AS IsMonthClosed "
			sql &= " From RP "
			sql &= " Left Join Mitarbeiter MA On MA.MANr = RP.MANr"
			sql &= " Left Join Kunden KD On KD.KDNr = RP.KDNr"
			sql &= " Where"
			sql &= " RP.MDNr = @MDNr"
			sql &= String.Format(" AND RP.MANr In ({0})", numbersBuffer)
			sql &= " AND RP.Jahr = @Jahr"
			sql &= " AND (@Monat = 0 Or RP.Monat = @Monat)"
			sql &= " AND (@employmentPVL = '' Or RP.RPGAV_Beruf = @employmentPVL)"
			sql &= " And (@Filiale = '' OR (KD.KDFiliale + MA.MAFiliale) Like @Filiale)"
			sql &= " ORDER BY RP.Von, RP.Bis"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", ReplaceMissing(searchData.MDNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("monat", ReplaceMissing(searchData.Monat, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("jahr", ReplaceMissing(searchData.Jahr, 0)))

			listOfParams.Add(New SqlClient.SqlParameter("employmentPVL", ReplaceMissing(searchData.EmploymentPVL, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("Filiale", "%" & ReplaceMissing(usFiliale, String.Empty) & "%"))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of RPMasterData)

					While reader.Read()
						Dim data As New RPMasterData
						data.MDNr = searchData.MDNr
						data.ID = SafeGetInteger(reader, "ID", 0)
						data.RPNR = SafeGetInteger(reader, "RPNR", Nothing)
						data.ESNR = SafeGetInteger(reader, "ESNR", Nothing)
						data.EmployeeNumber = SafeGetInteger(reader, "MANR", Nothing)
						data.CustomerNumber = SafeGetInteger(reader, "KDNR", Nothing)
						data.LONr = SafeGetInteger(reader, "LONr", Nothing)
						data.Currency = SafeGetString(reader, "Currency")
						data.SUVA = SafeGetString(reader, "SUVA")
						data.Monat = SafeGetByte(reader, "Monat", Nothing)
						data.Jahr = SafeGetString(reader, "Jahr")
						data.Von = SafeGetDateTime(reader, "Von", Nothing)
						data.Bis = SafeGetDateTime(reader, "Bis", Nothing)
						data.Erfasst = SafeGetBoolean(reader, "Erfasst", Nothing)
						data.RPKST = SafeGetString(reader, "RPKST")
						data.RPKST1 = SafeGetString(reader, "RPKST1")
						data.RPKST2 = SafeGetString(reader, "RPKST2")
						data.PrintedWeeks = SafeGetString(reader, "PrintedWeeks")
						data.PrintedDate = SafeGetString(reader, "PrintedDate")
						data.Farpflicht = SafeGetBoolean(reader, "Far-pflicht", Nothing)
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.RPGAV_FAG = SafeGetDecimal(reader, "RPGAV_FAG", Nothing)
						data.RPGAV_FAN = SafeGetDecimal(reader, "RPGAV_FAN", Nothing)
						data.RPGAV_WAG = SafeGetDecimal(reader, "RPGAV_WAG", Nothing)
						data.RPGAV_WAN = SafeGetDecimal(reader, "RPGAV_WAN", Nothing)
						data.RPGAV_VAG = SafeGetDecimal(reader, "RPGAV_VAG", Nothing)
						data.RPGAV_VAN = SafeGetDecimal(reader, "RPGAV_VAN", Nothing)
						data.RPGAV_Nr = SafeGetInteger(reader, "RPGAV_Nr", Nothing)
						data.RPGAV_Kanton = SafeGetString(reader, "RPGAV_Kanton")
						data.RPGAV_Beruf = SafeGetString(reader, "RPGAV_Beruf")
						data.RPGAV_Gruppe1 = SafeGetString(reader, "RPGAV_Gruppe1")
						data.RPGAV_Gruppe2 = SafeGetString(reader, "RPGAV_Gruppe2")
						data.RPGAV_Gruppe3 = SafeGetString(reader, "RPGAV_Gruppe3")
						data.RPGAV_Text = SafeGetString(reader, "RPGAV_Text")
						data.RPGAV_StdWeek = SafeGetDecimal(reader, "RPGAV_StdWeek", Nothing)
						data.RPGAV_StdMonth = SafeGetDecimal(reader, "RPGAV_StdMonth", Nothing)
						data.RPGAV_StdYear = SafeGetDecimal(reader, "RPGAV_StdYear", Nothing)
						data.RPGAV_FAG_M = SafeGetDecimal(reader, "RPGAV_FAG_M", Nothing)
						data.RPGAV_FAN_M = SafeGetDecimal(reader, "RPGAV_FAN_M", Nothing)
						data.RPGAV_VAG_M = SafeGetDecimal(reader, "RPGAV_VAG_M", Nothing)
						data.RPGAV_VAN_M = SafeGetDecimal(reader, "RPGAV_VAN_M", Nothing)
						data.RPGAV_WAG_M = SafeGetDecimal(reader, "RPGAV_WAG_M", Nothing)
						data.RPGAV_WAN_M = SafeGetDecimal(reader, "RPGAV_WAN_M", Nothing)
						data.RPGAV_FAG_S = SafeGetDecimal(reader, "RPGAV_FAG_S", Nothing)
						data.RPGAV_FAN_S = SafeGetDecimal(reader, "RPGAV_FAN_S", Nothing)
						data.RPGAV_VAG_S = SafeGetDecimal(reader, "RPGAV_VAG_S", Nothing)
						data.RPGAV_VAN_S = SafeGetDecimal(reader, "RPGAV_VAN_S", Nothing)
						data.RPGAV_WAG_S = SafeGetDecimal(reader, "RPGAV_WAG_S", Nothing)
						data.RPGAV_WAN_S = SafeGetDecimal(reader, "RPGAV_WAN_S", Nothing)
						data.RPGAV_FAG_J = SafeGetDecimal(reader, "RPGAV_FAG_J", Nothing)
						data.RPGAV_FAN_J = SafeGetDecimal(reader, "RPGAV_FAN_J", Nothing)
						data.RPGAV_VAG_J = SafeGetDecimal(reader, "RPGAV_VAG_J", Nothing)
						data.RPGAV_VAN_J = SafeGetDecimal(reader, "RPGAV_VAN_J", Nothing)
						data.RPGAV_WAG_J = SafeGetDecimal(reader, "RPGAV_WAG_J", Nothing)
						data.RPGAV_WAN_J = SafeGetDecimal(reader, "RPGAV_WAN_J", Nothing)
						data.ES_Einstufung = SafeGetString(reader, "ES_Einstufung")
						data.KDBranche = SafeGetString(reader, "KDBranche")
						data.ProposeNr = SafeGetInteger(reader, "ProposeNr", Nothing)
						data.RPDoc_Guid = SafeGetString(reader, "RPDoc_Guid")
						data.IsMonthClosed = (SafeGetInteger(reader, "IsMonthClosed", 0) > 0)

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


		''' <summary>
		''' add weekly data to RPSuvaTage.
		''' </summary>
		Public Function AddEmployeeSUVAWeekDaysData(ByVal mdnr As Integer, ByVal data As SuvaWeekData) As Boolean Implements IListingDatabaseAccess.AddEmployeeSUVAWeekDaysData

			Dim success = True

			Dim sql As String

			sql = " Insert Into RPSuvaTage (MDNr, RPNr, MANr, ESNr, KDNr, Mo, Di,"
			sql &= " Mi, Do, Fr, Sa, So, Woche, Jahr, Monat, "
			sql &= " Tag1, Tag2, Tag3, Tag4, Tag5, Tag6, Tag7, AnzTage, AnzStd, "
			sql &= " MoDate, SoDate) "
			sql &= " Values ("
			sql &= " @MDNr, @RPNr, @MANr, @ESNr, @KDNr, @Mo, @Di, "
			sql &= " @Mi, @Do, @Fr, @Sa, @So, @Woche, @Jahr, @Monat, "
			sql &= " @Tag1, @Tag2, @Tag3, @Tag4, @Tag5, @Tag6, @Tag7, @AnzTage, @AnzStd, "
			sql &= " @MoDate, @SoDate)"

			' Parameters

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			' Data of parameters
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", mdnr))
			listOfParams.Add(New SqlClient.SqlParameter("RPNr", ReplaceMissing(data.ReportNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MANr", ReplaceMissing(data.EmployeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ESNr", ReplaceMissing(data.EmploymentNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(data.CustomerNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Mo", ReplaceMissing(data.MondayDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Di", ReplaceMissing(data.TuesdayDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Mi", ReplaceMissing(data.WednesdayDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Do", ReplaceMissing(data.ThursdayDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Fr", ReplaceMissing(data.FridayDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Sa", ReplaceMissing(data.SaturdayDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("So", ReplaceMissing(data.SundayDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Woche", ReplaceMissing(data.CalendarWeek, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Jahr", ReplaceMissing(data.CalendarYear, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Monat", ReplaceMissing(data.CalendarMonth, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("Tag1", ReplaceMissing(data.GetDayValueOfDay(DayOfWeek.Monday), DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Tag2", ReplaceMissing(data.GetDayValueOfDay(DayOfWeek.Tuesday), DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Tag3", ReplaceMissing(data.GetDayValueOfDay(DayOfWeek.Wednesday), DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Tag4", ReplaceMissing(data.GetDayValueOfDay(DayOfWeek.Thursday), DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Tag5", ReplaceMissing(data.GetDayValueOfDay(DayOfWeek.Friday), DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Tag6", ReplaceMissing(data.GetDayValueOfDay(DayOfWeek.Saturday), DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Tag7", ReplaceMissing(data.GetDayValueOfDay(DayOfWeek.Sunday), DBNull.Value)))

			'listOfParams.Add(New SqlClient.SqlParameter("AnzTage", ReplaceMissing(data.GetDayCountValueOfWeek, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("AnzTage", ReplaceMissing(data.WorkedDaysInWeek, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("AnzStd", ReplaceMissing(data.GetHourValueOfWeek, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("MoDate", ReplaceMissing(data.GetFirstDayOfWeek, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("SoDate", ReplaceMissing(data.GetFirstDayOfWeek.AddDays(6), DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

			Return success


		End Function

		''' <summary>
		''' load employee suva hour data.
		''' </summary>
		Public Function LoadEmployeeSUVAHourData(ByVal searchData As EmployeeSuvaSearchData) As IEnumerable(Of SuvaTableListData) Implements IListingDatabaseAccess.LoadEmployeeSUVAHourData

			Dim result As List(Of SuvaTableListData) = Nothing

			Dim sql As String
			Dim numbersBuffer As String = String.Empty
			If Not searchData.EmployeeNumbers Is Nothing Then
				For Each number In searchData.EmployeeNumbers
					numbersBuffer = numbersBuffer & IIf(numbersBuffer <> "", ", ", "") & number
				Next
			End If

			sql = "Select MDNr, RPNr, MANr, ESNr, KDNr "
			sql &= " ,Convert(Int, IsNull(Woche, 0)) Woche, Convert(Int, IsNull(Jahr, 0)) Jahr, Convert(Int, IsNull(Monat, 0)) Monat "
			sql &= " ,Convert(DECIMAL(10,2), IsNull(AnzStd, 0)) AnzStd, Convert(int, IsNull(AnzTage, 0)) AnzTage "
			sql &= " ,MO, DI, MI, DO, FR, SA, SO "
			sql &= " ,Tag1, Tag2, Tag3, Tag4, Tag5, Tag6, Tag7 "
			sql &= " ,MODate, SODate "

			sql &= " From RPSuvaTage "
			sql &= " Where"
			sql &= " MDNr = @MDNr AND"
			sql &= " Jahr = @Jahr"
			sql &= String.Format(" AND MANr in ({0})", numbersBuffer)
			sql &= " ORDER BY MANr, ID"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", ReplaceMissing(searchData.MDNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("jahr", ReplaceMissing(searchData.Jahr, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of SuvaTableListData)

					While reader.Read()
						Dim data As New SuvaTableListData

						data.MDNr = SafeGetInteger(reader, "MDNr", 0)
						data.ReportNumber = SafeGetInteger(reader, "RPNR", Nothing)
						data.EmploymentNumber = SafeGetInteger(reader, "ESNR", Nothing)
						data.EmployeeNumber = SafeGetInteger(reader, "MANR", Nothing)
						data.CustomerNumber = SafeGetInteger(reader, "KDNR", Nothing)

						data.MondayDate = SafeGetDateTime(reader, "MO", Nothing)
						data.TuesdayDate = SafeGetDateTime(reader, "DI", Nothing)
						data.WednesdayDate = SafeGetDateTime(reader, "MI", Nothing)
						data.ThursdayDate = SafeGetDateTime(reader, "DO", Nothing)
						data.FridayDate = SafeGetDateTime(reader, "FR", Nothing)
						data.SaturdayDate = SafeGetDateTime(reader, "SA", Nothing)
						data.SundayDate = SafeGetDateTime(reader, "SO", Nothing)

						data.Tag1 = SafeGetString(reader, "Tag1")
						data.Tag2 = SafeGetString(reader, "Tag2")
						data.Tag3 = SafeGetString(reader, "Tag3")
						data.Tag4 = SafeGetString(reader, "Tag4")
						data.Tag5 = SafeGetString(reader, "Tag5")
						data.Tag6 = SafeGetString(reader, "Tag6")
						data.Tag7 = SafeGetString(reader, "Tag7")

						data.MondayOfWeek = SafeGetDateTime(reader, "MoDate", Nothing)
						data.SundayOfWeek = SafeGetDateTime(reader, "SoDate", Nothing)

						data.WorkedDayCount = SafeGetInteger(reader, "AnzTage", 0)
						data.WorkedHourCount = SafeGetDecimal(reader, "AnzStd", 0)

						data.CalendarMonth = SafeGetInteger(reader, "Monat", Nothing)
						data.CalendarWeek = SafeGetInteger(reader, "Woche", Nothing)
						data.CalendarYear = SafeGetInteger(reader, "Jahr", Nothing)


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

#End Region


	End Class


End Namespace
