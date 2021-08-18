
Imports SP.DatabaseAccess.Common.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language


Namespace Common

	Partial Class CommonDatabaseAccess
		Inherits DatabaseAccessBase
		Implements ICommonDatabaseAccess


		''' <summary>
		''' Loads country data.
		''' </summary>
		''' <returns>The country data.</returns>
		Function LoadCountryData() As IEnumerable(Of CountryData) Implements ICommonDatabaseAccess.LoadCountryData

			Dim result As List(Of CountryData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Land, Code FROM LND ORDER BY Land ASC"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If Not reader Is Nothing Then

					result = New List(Of CountryData)

					While reader.Read()
						Dim countryData As New CountryData
						countryData.ID = SafeGetInteger(reader, "ID", 0)
						countryData.Name = SafeGetString(reader, "Land")
						countryData.Code = SafeGetString(reader, "Code")

						result.Add(countryData)

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
		''' Loads post code data.
		''' </summary>
		''' <returns>List of post code data.</returns>
		Function LoadPostcodeData() As IEnumerable(Of PostCodeData) Implements ICommonDatabaseAccess.LoadPostcodeData

			Dim result As List(Of PostCodeData) = Nothing

			Dim sql As String

			sql = "SELECT ID, PLZ, Ort, KANTON, Land FROM PLZ ORDER BY PLZ"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of PostCodeData)

					While reader.Read()
						Dim postcodeData As New PostCodeData
						postcodeData.ID = SafeGetInteger(reader, "ID", 0)
						postcodeData.Postcode = Convert.ToString(SafeGetInteger(reader, "PLZ", 0))
						postcodeData.Location = SafeGetString(reader, "Ort")
						postcodeData.Canton = SafeGetString(reader, "Kanton")
						postcodeData.Country = SafeGetString(reader, "Land")


						result.Add(postcodeData)

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
		''' Loads canton data.
		''' </summary>
		''' <returns>List of canton data.</returns>
		Function LoadCantonData() As IEnumerable(Of CantonData) Implements ICommonDatabaseAccess.LoadCantonData

			Dim result As List(Of CantonData) = Nothing

			Dim sql As String

			sql = "SELECT GetFeld, Description FROM Tab_Kanton ORDER BY Description Asc"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CantonData)

					While reader.Read()
						Dim cantonData As New CantonData

						cantonData.GetField = SafeGetString(reader, "GetFeld")
						cantonData.Description = SafeGetString(reader, "Description")

						result.Add(cantonData)

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
		''' Loads canton by postcode.
		''' </summary>
		''' <param name="postCode">The post code.</param>
		''' <returns>Postcode string or nothing in error case.</returns>
		Function LoadCantonByPostCode(ByVal postCode As String) As String Implements ICommonDatabaseAccess.LoadCantonByPostCode

			Dim result As String = Nothing

			Dim sql As String

			sql = "SELECT TOP 1 KANTON FROM PLZ WHERE PLZ = @postcode"

			' Parameters
			Dim postCodeParameter As New SqlClient.SqlParameter("postcode", postCode)
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(postCodeParameter)

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = SafeGetString(reader, "Kanton")

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
		''' Loads language data.
		''' </summary>
		''' <returns>List of language data.</returns>
		Function LoadLanguageData() As IEnumerable(Of LanguageData) Implements ICommonDatabaseAccess.LoadLanguageData

			Dim result As List(Of LanguageData) = Nothing

			Dim sql As String

			sql = String.Format("SELECT ID, GetFeld, Description, {0} as TranslatedText FROM TAB_Sprache ORDER BY {0} ASC", MapLanguageToColumnName(SelectedTranslationLanguage))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of LanguageData)

					While reader.Read()
						Dim languageData As New LanguageData
						languageData.ID = SafeGetInteger(reader, "ID", 0)
						languageData.GetField = SafeGetString(reader, "GetFeld")
						languageData.Description = SafeGetString(reader, "Description")
						languageData.TranslatedDescription = SafeGetString(reader, "TranslatedText")

						result.Add(languageData)

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
		''' Loads gender data.
		''' </summary>
		''' <returns>List of gender data.</returns>
		Function LoadGenderData() As IEnumerable(Of GenderData) Implements ICommonDatabaseAccess.LoadGenderData

			Dim result As List(Of GenderData) = Nothing

			Dim sql As String
			sql = String.Format("SELECT RecValue, {0} as TranslatedText FROM tbl_base_Geschlecht ORDER BY {0} ASC", MapLanguageToColumnName(SelectedTranslationLanguage))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of GenderData)

					While reader.Read()
						Dim genderData As New GenderData
						genderData.RecValue = SafeGetString(reader, "RecValue")
						genderData.TranslatedGender = SafeGetString(reader, "TranslatedText")

						result.Add(genderData)

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
		''' Loads civil state data.
		''' </summary>
		''' <returns>List of civil state data.</returns>
		Public Function LoadCivilStateData() As IEnumerable(Of CivilStateData) Implements ICommonDatabaseAccess.LoadCivilStateData

			Dim result As List(Of CivilStateData) = Nothing

			Dim sql As String
			sql = String.Format("SELECT GetFeld, Description, {0} as TranslatedText FROM TAB_Zivilstand ORDER BY {0} ASC", MapLanguageToColumnName(SelectedTranslationLanguage))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CivilStateData)

					While reader.Read()
						Dim civilStateData As New CivilStateData
						civilStateData.GetField = SafeGetString(reader, "GetFeld")
						civilStateData.Description = SafeGetString(reader, "Description")
						civilStateData.TranslatedCivilState = SafeGetString(reader, "TranslatedText")


						result.Add(civilStateData)

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
		''' Loads permission data.
		''' </summary>
		''' <returns>List of permission data.</returns>
		Public Function LoadPermissionData() As IEnumerable(Of PermissionData) Implements ICommonDatabaseAccess.LoadPermissionData

			Dim result As List(Of PermissionData) = Nothing

			Dim sql As String
			sql = String.Format("SELECT RecValue, {0} as TranslatedText FROM tbl_base_Bewilligung ORDER BY {0} ASC", MapLanguageToColumnName(SelectedTranslationLanguage))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of PermissionData)

					While reader.Read()
						Dim permissionData As New PermissionData
						permissionData.RecValue = SafeGetString(reader, "RecValue")
						permissionData.TranslatedPermission = SafeGetString(reader, "TranslatedText")

						result.Add(permissionData)

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
		''' Translate the permission code.
		''' </summary>
		''' <param name="permissionCode">The permission code.</param>
		''' <param name="language">The language.</param>
		''' <returns>Translated permission code or nothing if not possible</returns>
		Public Function TranslatePermissionCode(ByVal permissionCode As String, ByVal language As String) As String Implements ICommonDatabaseAccess.TranslatePermissionCode

			Dim result As MandantData = Nothing
			If String.IsNullOrWhiteSpace(permissionCode) Then Return String.Empty

			Dim sql As String

			sql = "SELECT [dbo].TranslatePermissionCode(@permissionCode, @language)"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("permissionCode", permissionCode))
			listOfParams.Add(New SqlClient.SqlParameter("language", language))

			Dim translation = ExecuteScalar(sql, listOfParams)

			If IsDBNull(translation) Then
				translation = String.Empty
			End If

			Return translation

		End Function

		''' <summary>
		''' Loads business branches data (Filialen)
		''' </summary>
		''' <returns>List of business branches data.</returns>
		Public Function LoadBusinessBranchsData() As IEnumerable(Of AvilableBusinessBranchData) Implements ICommonDatabaseAccess.LoadBusinessBranchsData

			Dim result As List(Of AvilableBusinessBranchData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Bezeichnung, Code_1 FROM Filialen Order By Bezeichnung ASC"


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of AvilableBusinessBranchData)

					While reader.Read()
						Dim availableBusinessBranchData As New AvilableBusinessBranchData
						availableBusinessBranchData.ID = SafeGetInteger(reader, "ID", 0)
						availableBusinessBranchData.Name = SafeGetString(reader, "Bezeichnung")
						availableBusinessBranchData.Code_1 = SafeGetString(reader, "Code_1")
						result.Add(availableBusinessBranchData)

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
		''' Loads all branch data (Branchen)
		''' </summary>
		Function LoadBranchData() As IEnumerable(Of BranchData) Implements ICommonDatabaseAccess.LoadBranchData

			Dim result As List(Of BranchData) = Nothing

			Dim translationColumn As String = String.Empty

			Select Case SelectedTranslationLanguage
				Case DatabaseAccess.Language.German
					translationColumn = "[BranchenBezeichnung D]"
				Case DatabaseAccess.Language.Italian
					translationColumn = "[BranchenBezeichnung I]"
				Case DatabaseAccess.Language.French
					translationColumn = "[BranchenBezeichnung F]"
				Case DatabaseAccess.Language.English
					translationColumn = "[BranchenBezeichnung E]"
				Case Else
					translationColumn = "[BranchenBezeichnung D]"
			End Select

			Dim sql As String = String.Format("SELECT  Code, Branche, {0} as TranslatedBranche FROM Branchen ORDER BY {0}", translationColumn)
			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then
					result = New List(Of BranchData)

					While reader.Read

						Dim branch = New BranchData With {
			  .Code = SafeGetInteger(reader, "Code", 0),
			  .Branche = SafeGetString(reader, "Branche"),
			  .TranslatedBrancheText = SafeGetString(reader, "TranslatedBranche")
			}
						result.Add(branch)

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
		''' Loads salutation data.
		''' </summary>
		''' <returns>List of salutation data.</returns>
		Function LoadSalutationData() As IEnumerable(Of SalutationData) Implements ICommonDatabaseAccess.LoadSalutationData

			Dim result As List(Of SalutationData) = Nothing

			Dim sql As String

			sql = String.Format("SELECT ID, Anrede, BriefForm, Anrede_{0} AS TranslatedAnrede, BriefForm_{0} AS TranslatedBriefFrom FROM Anrede ORDER BY ID ASC", MapLanguageToShortLanguageCode(SelectedTranslationLanguage))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of SalutationData)

					While reader.Read

						Dim salutationData = New SalutationData()
						salutationData.Id = SafeGetInteger(reader, "ID", 0)
						salutationData.Salutation = SafeGetString(reader, "Anrede")
						salutationData.LetterForm = SafeGetString(reader, "BriefForm")
						salutationData.TranslatedSalutation = SafeGetString(reader, "TranslatedAnrede")
						salutationData.TranslatedLetterForm = SafeGetString(reader, "TranslatedBriefFrom")

						result.Add(salutationData)

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
		''' Loads terms and conditions data (Tab_AGB)
		''' </summary>
		''' <returns>List of terms and conditions data.</returns>
		Public Function LoadTermsAndConditionsData() As IEnumerable(Of TermsAndConditionsData) Implements ICommonDatabaseAccess.LoadTermsAndConditionsData
			Dim result As List(Of TermsAndConditionsData) = Nothing

			Dim sql As String

			sql = String.Format("SELECT ID, Bezeichnung, {0} as TranslatedText FROM Tab_AGB ORDER BY {0} ASC", MapLanguageToColumnName(SelectedTranslationLanguage))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of TermsAndConditionsData)

					While reader.Read()
						Dim termsAndConditionsData As New TermsAndConditionsData
						termsAndConditionsData.ID = SafeGetInteger(reader, "ID", 0)
						termsAndConditionsData.Description = SafeGetString(reader, "Bezeichnung")
						termsAndConditionsData.TranslatedTermsAndConditions = SafeGetString(reader, "TranslatedText")


						result.Add(termsAndConditionsData)

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
		''' Loads currency data (Tab_Currency)
		''' </summary>
		''' <returns>List of currency data.</returns>
		Public Function LoadCurrencyData() As IEnumerable(Of CurrencyData) Implements ICommonDatabaseAccess.LoadCurrencyData
			Dim result As List(Of CurrencyData) = Nothing

			Dim sql As String

			sql = "SELECT GetFeld, Description FROM Tab_Currency ORDER BY GetFeld ASC"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CurrencyData)

					While reader.Read()
						Dim currencyData As New CurrencyData
						currencyData.Code = SafeGetString(reader, "GetFeld")
						currencyData.Description = SafeGetString(reader, "Description")

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

		''' <summary>
		''' Loads absence data.
		''' </summary>
		''' <returns>List of absence data.</returns>
		Public Function LoadAbsenceData() As IEnumerable(Of AbsenceData) Implements ICommonDatabaseAccess.LoadAbsenceData

			Dim result As List(Of AbsenceData) = Nothing

			Dim sql As String
			sql = String.Format("SELECT GetFeld, {0} as TranslatedText FROM Tab_Fehlzeit ORDER BY {0} ASC", MapLanguageToColumnName(SelectedTranslationLanguage))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of AbsenceData)

					While reader.Read()
						Dim genderData As New AbsenceData
						genderData.GetFeld = SafeGetString(reader, "GetFeld")
						genderData.TranslatedDescription = SafeGetString(reader, "TranslatedText")

						result.Add(genderData)

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
		''' Loads the cost centers 1 and 2
		''' </summary>
		Public Function LoadDefaultCostCenters(ByVal usNr As Integer) As DataObjects.DefaultCostCenters Implements ICommonDatabaseAccess.LoadDefaultCostCenters

			Dim costCenters As DataObjects.DefaultCostCenters = Nothing

			Dim sql As String
			sql = "SELECT USKst1, USKst2 FROM Benutzer WHERE USNr = @usNr"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("usNr", usNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try
				If reader IsNot Nothing Then
					If reader.Read Then
						costCenters = New DataObjects.DefaultCostCenters With {
			  .CostCenter1 = SafeGetString(reader, "USKst1"),
			  .CostCenter2 = SafeGetString(reader, "USKst2")
			}
					End If
				End If

			Catch e As Exception
				m_Logger.LogError(e.ToString())
			Finally
				CloseReader(reader)
			End Try

			Return costCenters
		End Function

		''' <summary>
		''' Loads the cost centers 1 and 2
		''' </summary>
		Public Function LoadCostCenters() As DataObjects.CostCenters Implements ICommonDatabaseAccess.LoadCostCenters

			Dim costCenters As DataObjects.CostCenters = Nothing

			Dim sql As String
			sql = "Select 'Kst1' as Kst , KSTName, KSTBezeichnung, '' as KSTName1 from Tab_Kst1" +
			" Union All " +
			"SELECT 'Kst2' as Kst, KSTName, KSTBezeichnung, KSTName1  from Tab_Kst2"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			'listOfParams.Add(New SqlClient.SqlParameter("invoiceNumber", invoiceNumber))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try
				If reader IsNot Nothing Then
					costCenters = New DataObjects.CostCenters()

					While reader.Read

						Dim kst As String = SafeGetString(reader, "Kst")
						If kst.Equals("Kst1") Then
							Dim costCenter = New DataObjects.CostCenter With {
				.KSTBezeichnung = SafeGetString(reader, "KSTBezeichnung"),
				.KSTName = SafeGetString(reader, "KSTName")
			  }
							costCenters.AddCostCenter1(costCenter)
						Else
							Dim costCenter = New DataObjects.CostCenter2 With {
				.KSTBezeichnung = SafeGetString(reader, "KSTBezeichnung"),
				.KSTName = SafeGetString(reader, "KSTName"),
				.KSTName1 = SafeGetString(reader, "KSTName1")
			  }
							costCenters.AddCostCenter2(costCenter)
						End If
					End While
				End If

			Catch e As Exception
				m_Logger.LogError(e.ToString())
			Finally
				CloseReader(reader)
			End Try

			Return costCenters
		End Function

		''' <summary>
		''' Loads the mandant year data.
		''' </summary>
		''' <param name="mdNumber">The mandant number</param>
		''' <returns>List of years or nothing in error case.</returns>
		Public Function LoadMandantYears(ByVal mdNumber As Integer) As IEnumerable(Of Integer) Implements ICommonDatabaseAccess.LoadMandantYears

			Dim result As List(Of Integer) = Nothing

			Dim sql As String = String.Empty

			sql = sql & "SELECT DISTINCT Jahr FROM Mandanten WHERE MDNR = @mdNr ORDER BY Jahr DESC "

			Dim mdNumberParameter As New SqlClient.SqlParameter("mdNr", mdNumber)
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(mdNumberParameter)

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of Integer)

					While reader.Read

						Dim year = SafeGetString(reader, "Jahr", "0")

						result.Add(Convert.ToInt32(year))

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
		''' Loads employee exists payroll month for a year.
		''' </summary>
		''' <param name="year">The year.</param>
		''' <param name="mdNumber">The mandant number.</param>
		''' <returns>List of closed month or nothing in error case.</returns>
		Public Function LoadEmployeeExistsPayrollMonthOfYear(ByVal mdNumber As Integer, ByVal employeeNr As Integer, ByVal year As Integer) As IEnumerable(Of Integer) Implements ICommonDatabaseAccess.LoadEmployeeExistsPayrollMonthOfYear

			Dim result As List(Of Integer) = Nothing

			Dim sql As String = String.Empty

			sql = sql & "SELECT LP AS [Month] FROM LO WHERE MDNr = @MDNr And MANr = @MANr And Jahr=@Year Order By LP DESC"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", mdNumber))
			listOfParams.Add(New SqlClient.SqlParameter("@MANr", employeeNr))
			listOfParams.Add(New SqlClient.SqlParameter("@Year", year))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of Integer)

					While reader.Read

						Dim month = SafeGetInteger(reader, "Month", "0")

						result.Add(month)

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
		''' Loads employee exists payroll year.
		''' </summary>
		''' <param name="mdNumber">The mandant number.</param>
		''' <returns>List of closed month or nothing in error case.</returns>
		Public Function LoadEmployeeExistsPayrollYear(ByVal mdNumber As Integer, ByVal employeeNr As Integer) As IEnumerable(Of Integer) Implements ICommonDatabaseAccess.LoadEmployeeExistsPayrollYear

			Dim result As List(Of Integer) = Nothing

			Dim sql As String = String.Empty

			sql = sql & "SELECT Jahr AS [YEAR] FROM LO WHERE MDNr = @MDNr And MANr = @MANr Group By Jahr Order By Jahr DESC"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", mdNumber))
			listOfParams.Add(New SqlClient.SqlParameter("@MANr", employeeNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of Integer)

					While reader.Read

						Dim year = SafeGetInteger(reader, "YEAR", "0")

						result.Add(year)

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
		''' Loads the month for a year in LO table.
		''' </summary>
		''' <param name="year">The year.</param>
		''' <param name="mdNumber">The mandant number.</param>
		''' <returns>List of closed month or nothing in error case.</returns>
		Function LoadPayrollMonthOfYear(ByVal year As Integer, ByVal mdNumber As Integer) As IEnumerable(Of Integer) Implements ICommonDatabaseAccess.LoadPayrollMonthOfYear

			Dim result As List(Of Integer) = Nothing

			Dim sql As String

			sql = "SELECT Convert(Int, LP) AS [Month] "
			sql &= "FROM LO "
			sql &= "WHERE MDNr = @MDNr "
			sql &= "And Convert(Int, Jahr) = @Year "
			sql &= "Group By Convert(Int, LP) "
			sql &= "Order By Convert(Int, LP) Desc "

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Year", year))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", mdNumber))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of Integer)

					While reader.Read

						Dim month = SafeGetInteger(reader, "Month", "0")

						result.Add(month)

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

		Public Function LoadClosedMonthOfYear(ByVal year As Integer, ByVal mdNumber As Integer) As IEnumerable(Of Integer) Implements ICommonDatabaseAccess.LoadClosedMonthOfYear

			Dim result As List(Of Integer) = Nothing

			Dim sql As String = String.Empty

			sql = sql & "SELECT Monat AS [Month] FROM MonthClose WHERE Jahr=@Year And MDNr = @MDNr"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Year", year))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", mdNumber))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of Integer)

					While reader.Read

						Dim month = SafeGetInteger(reader, "Month", "0")

						result.Add(month)

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
		''' Loads translated LA bez.
		''' </summary>
		''' <param name="laNr">The LAnr.</param>
		''' <param name="language">The language.</param>
		''' <param name="defaultText">The default text (german).</param>
		''' <returns>Translated text.</returns>
		Function LoadTranslatedLABez(ByVal laNr As Decimal, ByVal language As String, ByVal defaultText As String) As String Implements ICommonDatabaseAccess.LoadTranslatedLABez

			Dim result As MandantData = Nothing

			Dim sql As String

			sql = "SELECT [dbo].[Translate LA Name](@laNr, @language, @defaultText)"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("laNr", laNr))
			listOfParams.Add(New SqlClient.SqlParameter("language", language))
			listOfParams.Add(New SqlClient.SqlParameter("defaultText", defaultText))

			Dim translation = ExecuteScalar(sql, listOfParams)

			Return translation


		End Function


		''' <summary>
		''' Loads the closed month for a year.
		''' </summary>
		''' <param name="mdNumber">The mandant number.</param>
		''' <param name="year">The year.</param>
		''' <param name="month">The month.</param>
		''' <returns>List of closed month or nothing in error case.</returns>
		Public Function LoadAllMonthCloseData(ByVal mdNumber As Integer, ByVal year As Integer?, ByVal month As Integer?) As IEnumerable(Of MonthCloseData) Implements ICommonDatabaseAccess.LoadAllMonthCloseData

			Dim result As List(Of MonthCloseData) = Nothing

			Dim sql As String = String.Empty

			sql = sql & "SELECT ID, Convert(Int, Monat) As Monat, Convert(Int, Jahr) As Jahr, UserName, MDNr, (Select TOP 1 MDName From [Mandant.AllowedMDList] Where MDNr = @MDNr) As MDName, "
			sql &= "Convert(Datetime, CreatedOn) As CreatedOn "
			sql &= "FROM MonthClose WHERE (@Jahr = 0 Or Jahr = @Jahr) And (@Monat = 0 Or Monat = @Monat) And MDNr = @MDNr "
			sql &= "Order By Convert(Int, Jahr) Desc, Convert(Int, Monat) DESC"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Jahr", ReplaceMissing(year, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("@Monat", ReplaceMissing(month, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", mdNumber))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of MonthCloseData)

					While reader.Read

						Dim myData = New MonthCloseData()

						myData.ID = SafeGetInteger(reader, "ID", 0)
						myData.monat = SafeGetInteger(reader, "monat", 0)
						myData.jahr = SafeGetInteger(reader, "jahr", 0)
						myData.UserName = SafeGetString(reader, "username")
						myData.CreatedOn = SafeGetDateTime(reader, "createdon", Nothing)
						myData.MDName = SafeGetString(reader, "mdname")
						myData.MandantNumber = SafeGetInteger(reader, "MDNr", 0)

						result.Add(myData)

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
		''' Save CloseMonth data.
		''' </summary>
		''' <param name="CloseMonth">The year.</param>
		''' <returns>List of closed month or nothing in error case.</returns>
		Public Function SaveMonthCloseData(ByVal CloseMonth As MonthCloseData) As Boolean Implements ICommonDatabaseAccess.SaveMonthCloseData

			Dim success As Boolean

			Dim sql As String = String.Empty

			sql = sql & "Insert Into MonthClose (Monat, Jahr, UserName, CreatedOn, MDNr) Values (@Monat, @Jahr, @UserName, @CreatedOn, @MDNr)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Jahr", ReplaceMissing(CloseMonth.jahr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("@Monat", ReplaceMissing(CloseMonth.monat, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("@UserName", ReplaceMissing(CloseMonth.UserName, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("@CreatedOn", ReplaceMissing(CloseMonth.CreatedOn, Nothing)))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", CloseMonth.MandantNumber))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

			Return success

		End Function

		''' <summary>
		''' delete CloseMonth data.
		''' </summary>
		''' <param name="CloseMonth">The year.</param>
		''' <returns>List of closed month or nothing in error case.</returns>
		Public Function DeleteMonthCloseData(ByVal CloseMonth As MonthCloseData) As Boolean Implements ICommonDatabaseAccess.DeleteMonthCloseData

			Dim success As Boolean

			Dim sql As String = String.Empty

			sql = sql & "Delete MonthClose Where ID = @ID"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(CloseMonth.ID, 0)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

			Return success

		End Function


		''' <summary>
		''' Loads the closed month for a year.
		''' </summary>
		''' <param name="year">The year.</param>
		''' <param name="mdNumber">The mandant number.</param>
		''' <returns>List of closed month or nothing in error case.</returns>
		Public Function LoadNotInvalidDataForClosingMonth(ByVal mdNumber As Integer, ByVal year As Integer?, ByVal month As Integer?) As IEnumerable(Of NotValidatedData) Implements ICommonDatabaseAccess.LoadNotInvalidDataForClosingMonth

			Dim result As List(Of NotValidatedData) = Nothing

			Dim sql As String = String.Empty
			sql = "[List Conflicted Data For Closing Month]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Jahr", ReplaceMissing(year, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("@Monat", ReplaceMissing(month, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", mdNumber))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of NotValidatedData)

					While reader.Read

						Dim myData = New NotValidatedData()

						myData.modulname = SafeGetString(reader, "modulname")
						myData.modulnr = SafeGetInteger(reader, "modulnr", 0)
						myData.manr = SafeGetInteger(reader, "manr", 0)
						myData.startdate = SafeGetDateTime(reader, "von", Nothing)
						myData.maname = SafeGetString(reader, "maname")

						result.Add(myData)

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

		Function LoadSearchQueryTemplateData(ByVal mdNumber As Integer, ByVal showMenuIn As String) As IEnumerable(Of SearchQueryTemplateData) Implements ICommonDatabaseAccess.LoadSearchQueryTemplateData
			Dim result As List(Of SearchQueryTemplateData) = Nothing

			Dim sql As String

			sql = "Select [ID], [MenuLabel], [QueryString], [QueryType], [ShowMenuIn], "
			sql &= String.Format("{0} as TranslatedText ", MapLanguageToColumnName(SelectedTranslationLanguage))
			sql &= "From [tbl_Query_Template] "
			sql &= "Where [ShowMenuIn] = @QueryshowIn "
			sql &= "Order By ID"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("QueryshowIn", ReplaceMissing(showMenuIn, String.Empty)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of SearchQueryTemplateData)

					While reader.Read

						Dim myData = New SearchQueryTemplateData()

						myData.ID = SafeGetInteger(reader, "ID", 0)
						myData.MenuLabel = SafeGetString(reader, "MenuLabel")
						myData.TranslatedLabel = SafeGetString(reader, "TranslatedText")
						myData.QueryString = SafeGetString(reader, "QueryString")
						myData.QueryType = SafeGetInteger(reader, "QueryType", 0)
						myData.ShowMenuIn = SafeGetString(reader, "ShowMenuIn")


						result.Add(myData)

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

		Function LoadMandantBytesData(ByVal mdNumber As Integer, ByVal logoModul As String) As Byte() Implements ICommonDatabaseAccess.LoadMandantBytesData
			Dim result As Byte() = Nothing

			Dim sql As String
			sql = "Select FileContent From MD_Documents "
			sql &= "Where MDNr = @mdNumber "
			sql &= "AND (@logoModul = '' OR FileBez = @logoModul) "

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNumber", ReplaceMissing(mdNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("logoModul", ReplaceMissing(logoModul, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = SafeGetByteArray(reader, "FileContent")

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
