Imports SP.DatabaseAccess.Employee.DataObjects.Salary
Imports System.Text

Namespace Employee

	Partial Public Class EmployeeDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IEmployeeDatabaseAccess

		''' <summary>
		''' Loads employee LO (salary) settings.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <returns>The LO settings or nothing in error case.</returns>
		Function LoadEmployeeLOSettings(ByVal employeeNumber As Integer) As EmployeeLOSettingsData Implements IEmployeeDatabaseAccess.LoadEmployeeLOSettings

			Dim employeeLOSettting As EmployeeLOSettingsData = Nothing

			Dim sql As String = String.Empty

			sql = sql & "SELECT [MANr]"
			sql = sql & ",[Currency]"
			sql = sql & ",[Zahlart]"
			sql = sql & ",[NoZG]"
			sql = sql & ",[NoZGWhy]"
			sql = sql & ",[NoLO]"
			sql = sql & ",[NoLOWhy]"
			sql = sql & ",[AHVCode]"
			sql = sql & ",[ALVCode]"
			sql = sql & ",[BVGCode]"
			sql = sql & ",[KTGpflicht]"
			sql = sql & " ,[KKpflicht]"
			sql = sql & ",[LORes1pflicht]"
			sql = sql & ",[LoRes2pflicht]"
			sql = sql & ",[FerienBack]"
			sql = sql & ",[FeierBack]"
			sql = sql & ",[Lohn13Back]"
			sql = sql & ",[Result]"
			sql = sql & ",[NoRPPrint]"
			sql = sql & ",[SendAsZIP]"
			sql = sql & ",[SecSuvaCode]"
			sql = sql & " ,[MAGleitzeit]"
			sql = sql & ",[KI]"
			sql = sql & ",[Max_NegativSalary]"
			sql = sql & ",[ID]"
			sql = sql & ",[AHVAnAm]"
			sql = sql & ",[WeeklyPayment]"
			sql = sql & "FROM [MA_LOSetting] "
			sql = sql & "WHERE MANr = @employeeNumber"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("employeeNumber", employeeNumber))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If Not reader Is Nothing Then

					If reader.Read Then
						employeeLOSettting = New EmployeeLOSettingsData
						employeeLOSettting.ID = SafeGetInteger(reader, "ID", 0)
						employeeLOSettting.EmployeeNumber = SafeGetInteger(reader, "MANr", Nothing)
						employeeLOSettting.Currency = SafeGetString(reader, "Currency")
						employeeLOSettting.Zahlart = SafeGetString(reader, "Zahlart")
						employeeLOSettting.NoZG = SafeGetBoolean(reader, "NoZG", Nothing)
						employeeLOSettting.NoZGWhy = SafeGetString(reader, "NoZGWhy")
						employeeLOSettting.NoLO = SafeGetBoolean(reader, "NoLO", Nothing)
						employeeLOSettting.NoLOWhy = SafeGetString(reader, "NoLOWhy")
						employeeLOSettting.AHVCode = SafeGetString(reader, "AHVCode")
						employeeLOSettting.ALVCode = SafeGetString(reader, "ALVCode")
						employeeLOSettting.BVGCode = SafeGetString(reader, "BVGCode")
						employeeLOSettting.KTGPflicht = SafeGetBoolean(reader, "KTGpflicht", Nothing)
						employeeLOSettting.KKPflicht = SafeGetBoolean(reader, "KKpflicht", Nothing)
						employeeLOSettting.LORes1Pflicht = SafeGetBoolean(reader, "LORes1pflicht", Nothing)
						employeeLOSettting.LoRes2Pflicht = SafeGetBoolean(reader, "LORes2pflicht", Nothing)
						employeeLOSettting.FerienBack = SafeGetBoolean(reader, "FerienBack", Nothing)
						employeeLOSettting.FeierBack = SafeGetBoolean(reader, "FeierBack", Nothing)
						employeeLOSettting.Lohn13Back = SafeGetBoolean(reader, "Lohn13Back", Nothing)
						employeeLOSettting.Result = SafeGetString(reader, "Result")
						employeeLOSettting.NoRPPrint = SafeGetBoolean(reader, "NoRPPrint", Nothing)
						employeeLOSettting.PayrollSendAsZip = SafeGetBoolean(reader, "SendAsZIP", Nothing)
						employeeLOSettting.SecSuvaCode = SafeGetString(reader, "SecSuvaCode")
						employeeLOSettting.MAGleitzeit = SafeGetBoolean(reader, "MAGleitzeit", Nothing)
						employeeLOSettting.KI = SafeGetBoolean(reader, "KI", Nothing)
						employeeLOSettting.Max_NegativSalary = SafeGetDecimal(reader, "Max_NegativSalary", Nothing)
						employeeLOSettting.AHVAnAm = SafeGetDateTime(reader, "AHVAnAm", Nothing)
						employeeLOSettting.WeeklyPayment = SafeGetBoolean(reader, "WeeklyPayment", False)

					End If

				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				employeeLOSettting = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return employeeLOSettting

		End Function

		''' <summary>
		''' Loads AHV data.
		''' </summary>
		''' <returns>List of AHV data.</returns>
		Public Function LoadAHVData() As IEnumerable(Of AHVData) Implements IEmployeeDatabaseAccess.LoadAHVData

			Dim result As List(Of AHVData) = Nothing

			Dim sql As String = String.Empty

			sql = String.Format("SELECT ID, GetFeld , Description, {0} TranslatedText FROM Tab_AHV ORDER BY TranslatedText", MapLanguageToColumnName(SelectedTranslationLanguage))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of AHVData)

					While reader.Read()
						Dim ahvData As New AHVData
						ahvData.ID = SafeGetInteger(reader, "ID", 0)
						ahvData.GetField = SafeGetShort(reader, "GetFeld", Nothing)
						ahvData.Description = SafeGetString(reader, "Description")
						ahvData.TranslatedAHVText = SafeGetString(reader, "TranslatedText")

						result.Add(ahvData)
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
		''' Loads ALV data.
		''' </summary>
		''' <returns>List of ALV data.</returns>
		Public Function LoadALVData() As IEnumerable(Of ALVData) Implements IEmployeeDatabaseAccess.LoadALVData

			Dim result As List(Of ALVData) = Nothing

			Dim sql As String = String.Empty

			sql = String.Format("SELECT ID, GetFeld , Description, {0} TranslatedText FROM Tab_ALV ORDER BY GetFeld", MapLanguageToColumnName(SelectedTranslationLanguage))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of ALVData)

					While reader.Read()
						Dim alvData As New ALVData
						alvData.ID = SafeGetInteger(reader, "ID", 0)
						alvData.GetField = SafeGetShort(reader, "GetFeld", Nothing)
						alvData.Description = SafeGetString(reader, "Description")
						alvData.TranslatedALVText = SafeGetString(reader, "TranslatedText")

						result.Add(alvData)
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
		''' Loads BVG data.
		''' </summary>
		''' <returns>List of BVG data.</returns>
		Public Function LoadBVGData() As IEnumerable(Of BVGData) Implements IEmployeeDatabaseAccess.LoadBVGData

			Dim result As List(Of BVGData) = Nothing

			Dim sql As String = String.Empty

			sql = String.Format("SELECT ID, GetFeld , Description, {0} TranslatedText FROM Tab_BVG ORDER BY GetFeld", MapLanguageToColumnName(SelectedTranslationLanguage))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of BVGData)

					While reader.Read()
						Dim bvgData As New BVGData
						bvgData.ID = SafeGetInteger(reader, "ID", 0)
						bvgData.GetField = SafeGetString(reader, "GetFeld", Nothing)
						bvgData.Description = SafeGetString(reader, "Description")
						bvgData.TranslatedBVGText = SafeGetString(reader, "TranslatedText")

						result.Add(bvgData)
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
		''' Loads Suva2 data.
		''' </summary>
		''' <returns>List of Suva2 data.</returns>
		Public Function LoadSuva2Data() As IEnumerable(Of Suva2Data) Implements IEmployeeDatabaseAccess.LoadSuva2Data

			Dim result As List(Of Suva2Data) = Nothing

			Dim sql As String = String.Empty

			sql = String.Format("SELECT ID, GetFeld , Bezeichnung, {0} TranslatedText FROM Tab_2Suva ORDER BY GetFeld", MapLanguageToColumnName(SelectedTranslationLanguage))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of Suva2Data)

					While reader.Read()
						Dim bvgData As New Suva2Data
						bvgData.ID = SafeGetInteger(reader, "ID", 0)
						bvgData.GetField = SafeGetString(reader, "GetFeld", Nothing)
						bvgData.Description = SafeGetString(reader, "Bezeichnung")
						bvgData.TranslatedSuva2Text = SafeGetString(reader, "TranslatedText")

						result.Add(bvgData)
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
		''' Loads currency data.
		''' </summary>
		''' <returns>List of currency data.</returns>
		Public Function LoadCurrencyData() As IEnumerable(Of CurrencyData) Implements IEmployeeDatabaseAccess.LoadCurrenyData

			Dim result As List(Of CurrencyData) = Nothing

			Dim sql As String = String.Empty

			sql = String.Format("SELECT RecValue, {0} TranslatedText FROM tbl_base_Currency ORDER BY RecValue", MapLanguageToColumnName(SelectedTranslationLanguage))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of CurrencyData)

					While reader.Read()
						Dim currencyData As New CurrencyData
						currencyData.RecValue = SafeGetString(reader, "RecValue", Nothing)
						currencyData.TranslatedCurrencyText = SafeGetString(reader, "TranslatedText")

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
		''' Loads payment method data.
		''' </summary>
		''' <returns>List of payment data.</returns>
		Public Function LoadPaymentMethodData() As IEnumerable(Of PaymentMethodData) Implements IEmployeeDatabaseAccess.LoadPaymentMethodData

			Dim result As List(Of PaymentMethodData) = Nothing

			Dim sql As String = String.Empty

			sql = String.Format("SELECT RecValue, {0} TranslatedText FROM tbl_base_Zahlart ORDER BY RecValue", MapLanguageToColumnName(SelectedTranslationLanguage))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of PaymentMethodData)

					While reader.Read()
						Dim paymentMethodData As New PaymentMethodData
						paymentMethodData.RecValue = SafeGetString(reader, "RecValue", Nothing)
						paymentMethodData.TranslatedPaymentMethod = SafeGetString(reader, "TranslatedText")

						result.Add(paymentMethodData)
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
		''' Updates employee LO (salary) Settings.
		''' </summary>
		''' <param name="employeeLOSettings">The employee LO settings.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function UpdateEmployeeLOSettings(ByVal employeeLOSettings As EmployeeLOSettingsData) As Boolean Implements IEmployeeDatabaseAccess.UpdateEmployeeLOSettings

			Dim success = True

			Dim sql As String = String.Empty

			sql = sql & "UPDATE [MA_LOSetting] "
			sql = sql & "SET [MANr] = @employeeNumber"
			sql = sql & ",[Currency] = @currency"
			sql = sql & ",[Zahlart] = @zahlart"
			sql = sql & ",[NoZG] = @noZG"
			sql = sql & ",[NoZGWhy] = @noZGwhy"
			sql = sql & ",[NoLO] = @noLO"
			sql = sql & ",[NoLOWhy] = @noLOWhy"
			sql = sql & ",[AHVCode] = @ahvCode"
			sql = sql & ",[ALVCode] = @alvCode"
			sql = sql & ",[BVGCode] = @bvgCode"
			sql = sql & ",[KTGpflicht] = @ktgPflicht"
			sql = sql & ",[KKpflicht] = @kkPflicht"
			sql = sql & ",[LORes1pflicht] = @loRes1Pflicht"
			sql = sql & ",[LoRes2pflicht] = @loRes2Pflicht"
			sql = sql & ",[FerienBack] = @ferienBack"
			sql = sql & ",[FeierBack] =  @feierBack"
			sql = sql & ",[Lohn13Back] = @lohn13Back"
			sql = sql & ",[Result] = @result"
			sql = sql & ",[NoRPPrint] = @noRPPrint"
			sql = sql & ",[SendAsZIP] = @SendAsZIP"
			sql = sql & ",[SecSuvaCode] = @secSuvaCode"
			sql = sql & ",[MAGleitzeit] = @maGleitzeit"
			sql = sql & ",[KI] = @KI"
			sql = sql & ",[Max_NegativSalary] = @max_NegativeSalary"
			sql = sql & ",[AHVAnAm] = @ahvAnAm "
			sql = sql & ",[WeeklyPayment] = @WeeklyPayment"

			sql = sql & " WHERE MANr = @employeeNumber"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("employeeNumber", ReplaceMissing(employeeLOSettings.EmployeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("currency", ReplaceMissing(employeeLOSettings.Currency, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("zahlart", ReplaceMissing(employeeLOSettings.Zahlart, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("noZG", ReplaceMissing(employeeLOSettings.NoZG, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("noZGwhy", ReplaceMissing(employeeLOSettings.NoZGWhy, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("noLO", ReplaceMissing(employeeLOSettings.NoLO, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("noLOWhy", ReplaceMissing(employeeLOSettings.NoLOWhy, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ahvCode", ReplaceMissing(employeeLOSettings.AHVCode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("alvCode", ReplaceMissing(employeeLOSettings.ALVCode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("bvgCode", ReplaceMissing(employeeLOSettings.BVGCode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ktgPflicht", ReplaceMissing(employeeLOSettings.KTGPflicht, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("kkPflicht", ReplaceMissing(employeeLOSettings.KKPflicht, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("loRes1Pflicht", ReplaceMissing(employeeLOSettings.LORes1Pflicht, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("loRes2Pflicht", ReplaceMissing(employeeLOSettings.LoRes2Pflicht, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ferienBack", ReplaceMissing(employeeLOSettings.FerienBack, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("feierBack", ReplaceMissing(employeeLOSettings.FeierBack, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("lohn13Back", ReplaceMissing(employeeLOSettings.Lohn13Back, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("result", ReplaceMissing(employeeLOSettings.Result, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("noRPPrint", ReplaceMissing(employeeLOSettings.NoRPPrint, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("SendAsZIP", ReplaceMissing(employeeLOSettings.PayrollSendAsZip, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("secSuvaCode", ReplaceMissing(employeeLOSettings.SecSuvaCode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("maGleitzeit", ReplaceMissing(employeeLOSettings.MAGleitzeit, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("max_NegativeSalary", ReplaceMissing(employeeLOSettings.Max_NegativSalary, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KI", ReplaceMissing(employeeLOSettings.KI, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ahvAnAm", ReplaceMissing(employeeLOSettings.AHVAnAm, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("WeeklyPayment", ReplaceMissing(employeeLOSettings.WeeklyPayment, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

	End Class

End Namespace
