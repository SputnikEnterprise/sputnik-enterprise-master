
Imports SP.DatabaseAccess.TableSetting.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language


Namespace TableSetting



	Partial Class TablesDatabaseAccess
		Inherits DatabaseAccessBase
		Implements ITablesDatabaseAccess



#Region "customer mahncode"

		Function LoadCustomerPaymentReminderCodeData() As IEnumerable(Of Customer.DataObjects.PaymentReminderCodeData) Implements ITablesDatabaseAccess.LoadCustomerPaymentReminderCodeData

			Dim result As List(Of Customer.DataObjects.PaymentReminderCodeData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Mahn1, Mahn2, Mahn3, Mahn4, GetFeld FROM Tab_Mahncode ORDER BY GetFeld"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of Customer.DataObjects.PaymentReminderCodeData)

					While reader.Read()
						Dim reminderCode As New Customer.DataObjects.PaymentReminderCodeData
						reminderCode.ID = SafeGetInteger(reader, "ID", 0)
						reminderCode.Reminder1 = SafeGetString(reader, "Mahn1")
						reminderCode.Reminder2 = SafeGetString(reader, "Mahn2")
						reminderCode.Reminder3 = SafeGetString(reader, "Mahn3")
						reminderCode.Reminder4 = SafeGetString(reader, "Mahn4")
						reminderCode.GetField = SafeGetString(reader, "GetFeld")

						result.Add(reminderCode)

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

		Function AddCustomerPaymentReminderCodeData(ByVal data As Customer.DataObjects.PaymentReminderCodeData) As Boolean Implements ITablesDatabaseAccess.AddCustomerPaymentReminderCodeData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.Tab_Mahncode (Mahn1, Mahn2, Mahn3, Mahn4, GetFeld) Values ("
			sql &= "@Mahn1"
			sql &= ", @Mahn2"
			sql &= ", @Mahn3"
			sql &= ", @Mahn4"
			sql &= ", @GetFeld)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Mahn1", ReplaceMissing(data.Reminder1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Mahn2", ReplaceMissing(data.Reminder2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Mahn3", ReplaceMissing(data.Reminder3, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Mahn4", ReplaceMissing(data.Reminder4, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@GetFeld", ReplaceMissing(data.GetField, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateCustomerPaymentReminderCodeData(ByVal data As Customer.DataObjects.PaymentReminderCodeData) As Boolean Implements ITablesDatabaseAccess.UpdateCustomerPaymentReminderCodeData

			Dim success = True

			Dim sql As String

			sql = "Update dbo.Tab_Mahncode Set Mahn1 = @Mahn1, "
			sql &= "Mahn2 = @Mahn2, "
			sql &= "Mahn3 = @Mahn3, "
			sql &= "Mahn4 = @Mahn4, "
			sql &= "GetFeld = @GetFeld "
			sql &= "Where ID = @ID"

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.ID, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Mahn1", ReplaceMissing(data.Reminder1, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Mahn2", ReplaceMissing(data.Reminder2, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Mahn3", ReplaceMissing(data.Reminder3, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Mahn4", ReplaceMissing(data.Reminder4, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@GetFeld", ReplaceMissing(data.GetField, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

				Return success

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			End Try

			Return success

		End Function

		Function DeleteCustomerPaymentReminderCodeData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteCustomerPaymentReminderCodeData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_Mahncode "
			sql &= "Where ID = @ID"

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(recid, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

				Return success

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			End Try

			Return success

		End Function

#End Region



#Region "customer payment conditions"

		Function LoadCustomerPaymentConditionData() As IEnumerable(Of Customer.DataObjects.PaymentConditionData) Implements ITablesDatabaseAccess.LoadCustomerPaymentConditionData

			Dim result As List(Of Customer.DataObjects.PaymentConditionData) = Nothing

			Dim sql As String

			sql = "SELECT ID, GetFeld, Description, Bez_D, Bez_I, Bez_F, Bez_E FROM Tab_ZahlKond ORDER BY GetFeld"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of Customer.DataObjects.PaymentConditionData)

					While reader.Read()
						Dim data As New Customer.DataObjects.PaymentConditionData
						data.ID = SafeGetInteger(reader, "ID", 0)
						data.GetField = SafeGetString(reader, "GetFeld")
						data.Description = SafeGetString(reader, "Description")
						data.bez_d = SafeGetString(reader, "bez_d")
						data.bez_i = SafeGetString(reader, "bez_i")
						data.bez_f = SafeGetString(reader, "bez_f")
						data.bez_e = SafeGetString(reader, "bez_e")

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

		Function AddCustomerPaymentConditionData(ByVal data As Customer.DataObjects.PaymentConditionData) As Boolean Implements ITablesDatabaseAccess.AddCustomerPaymentConditionData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.Tab_ZahlKond (GetFeld, Description, Bez_D, Bez_I, Bez_F, Bez_E) Values ("
			sql &= "@GetFeld"
			sql &= ", @Description"
			sql &= ", @Bez_D"
			sql &= ", @Bez_I"
			sql &= ", @Bez_F"
			sql &= ", @Bez_E)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@GetFeld", ReplaceMissing(data.GetField, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Description", ReplaceMissing(data.Description, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_D", ReplaceMissing(data.bez_d, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_I", ReplaceMissing(data.bez_i, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_F", ReplaceMissing(data.bez_f, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_E", ReplaceMissing(data.bez_e, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateCustomerPaymentConditionData(ByVal data As Customer.DataObjects.PaymentConditionData) As Boolean Implements ITablesDatabaseAccess.UpdateCustomerPaymentConditionData

			Dim success = True

			Dim sql As String

			sql = "Update dbo.Tab_ZahlKond Set GetFeld = @GetFeld, "
			sql &= "Description = @Description, "
			sql &= "Bez_D = @Bez_D, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID"

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("ID", ReplaceMissing(data.ID, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("GetFeld", ReplaceMissing(data.GetField, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Description", ReplaceMissing(data.Description, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Bez_d", ReplaceMissing(data.bez_d, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Bez_i", ReplaceMissing(data.bez_i, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Bez_F", ReplaceMissing(data.bez_f, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Bez_E", ReplaceMissing(data.bez_e, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

				Return success

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			End Try

			Return success

		End Function

		Function DeleteCustomerPaymentConditionData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteCustomerPaymentConditionData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_ZahlKond "
			sql &= "Where ID = @ID"

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(recid, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

				Return success

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			End Try

			Return success

		End Function

#End Region




#Region "customer invoice options"

		Function LoadCustomerInvoiceOptionData() As IEnumerable(Of Customer.DataObjects.InvoiceOptionData) Implements ITablesDatabaseAccess.LoadCustomerInvoiceOptionData

			Dim result As List(Of Customer.DataObjects.InvoiceOptionData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Bezeichnung, Bez_D, Bez_I, Bez_F, Bez_E FROM Tab_KDFOptions ORDER BY Bezeichnung"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of Customer.DataObjects.InvoiceOptionData)

					While reader.Read()
						Dim data As New Customer.DataObjects.InvoiceOptionData
						data.ID = SafeGetInteger(reader, "ID", 0)
						data.Description = SafeGetString(reader, "Bezeichnung")
						data.bez_d = SafeGetString(reader, "Bez_D")
						data.bez_i = SafeGetString(reader, "Bez_I")
						data.bez_f = SafeGetString(reader, "Bez_F")
						data.bez_e = SafeGetString(reader, "Bez_E")

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

		Function AddCustomerInvoiceOptionData(ByVal data As Customer.DataObjects.InvoiceOptionData) As Boolean Implements ITablesDatabaseAccess.AddCustomerInvoiceOptionData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.Tab_KDFOptions (Bezeichnung, bez_d, bez_i, bez_f, bez_e) Values ("
			sql &= "@Description"
			sql &= ", @bez_d"
			sql &= ", @bez_i"
			sql &= ", @bez_f"
			sql &= ", @bez_e)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Description", ReplaceMissing(data.Description, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@bez_d", ReplaceMissing(data.bez_d, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@bez_i", ReplaceMissing(data.bez_i, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@bez_f", ReplaceMissing(data.bez_f, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@bez_e", ReplaceMissing(data.bez_e, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateCustomerInvoiceOptionData(ByVal data As Customer.DataObjects.InvoiceOptionData) As Boolean Implements ITablesDatabaseAccess.UpdateCustomerInvoiceOptionData

			Dim success = True

			Dim sql As String

			sql = "Update dbo.Tab_KDFOptions Set Bezeichnung = @Description, "
			sql &= "bez_d = @bez_d, "
			sql &= "bez_i = @bez_i, "
			sql &= "bez_f = @bez_f, "
			sql &= "bez_e = @bez_e "
			sql &= "Where ID = @ID"

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.ID, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Description", ReplaceMissing(data.Description, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@bez_d", ReplaceMissing(data.bez_d, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@bez_i", ReplaceMissing(data.bez_i, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@bez_f", ReplaceMissing(data.bez_f, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@bez_e", ReplaceMissing(data.bez_e, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

				Return success

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			End Try

			Return success

		End Function

		Function DeleteCustomerInvoiceOptionData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteCustomerInvoiceOptionData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_KDFOptions "
			sql &= "Where ID = @ID"

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(recid, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

				Return success

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			End Try

			Return success

		End Function

#End Region




#Region "customer invoice type"

		Function LoadCustomerInvoiceTypeData() As IEnumerable(Of Customer.DataObjects.InvoiceTypeData) Implements ITablesDatabaseAccess.LoadCustomerInvoiceTypeData

			Dim result As List(Of Customer.DataObjects.InvoiceTypeData) = Nothing

			Dim sql As String

			sql = "SELECT ID, GetFeld, Description, Bez_D, Bez_I, Bez_F, Bez_E FROM Tab_Faktura ORDER BY GetFeld"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of Customer.DataObjects.InvoiceTypeData)

					While reader.Read()
						Dim data As New Customer.DataObjects.InvoiceTypeData
						data.ID = SafeGetInteger(reader, "ID", 0)
						data.Code = SafeGetString(reader, "GetFeld")
						data.Description = SafeGetString(reader, "Description")
						data.bez_d = SafeGetString(reader, "Bez_D")
						data.bez_i = SafeGetString(reader, "Bez_I")
						data.bez_f = SafeGetString(reader, "Bez_F")
						data.bez_e = SafeGetString(reader, "Bez_E")

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

		Function AddCustomerInvoiceTypeData(ByVal data As Customer.DataObjects.InvoiceTypeData) As Boolean Implements ITablesDatabaseAccess.AddCustomerInvoiceTypeData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.Tab_Faktura (GetFeld, Description, Bez_D, Bez_I, Bez_F, Bez_E) Values ("
			sql &= "@Code"
			sql &= ", @Description"
			sql &= ", @bez_d"
			sql &= ", @bez_i"
			sql &= ", @bez_f"
			sql &= ", @bez_e)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Code", ReplaceMissing(data.Code, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Description", ReplaceMissing(data.Description, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@bez_d", ReplaceMissing(data.bez_d, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@bez_i", ReplaceMissing(data.bez_i, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@bez_f", ReplaceMissing(data.bez_f, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@bez_e", ReplaceMissing(data.bez_e, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateCustomerInvoiceTypeData(ByVal data As Customer.DataObjects.InvoiceTypeData) As Boolean Implements ITablesDatabaseAccess.UpdateCustomerInvoiceTypeData

			Dim success = True

			Dim sql As String

			sql = "Update dbo.Tab_Faktura Set GetFeld = @Code, "
			sql &= "Description = @Description, "
			sql &= "bez_d = @bez_d, "
			sql &= "bez_i = @bez_i, "
			sql &= "bez_f = @bez_f, "
			sql &= "bez_e = @bez_e "
			sql &= "Where ID = @ID"

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.ID, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Code", ReplaceMissing(data.Code, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Description", ReplaceMissing(data.Description, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@bez_d", ReplaceMissing(data.bez_d, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@bez_i", ReplaceMissing(data.bez_i, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@bez_f", ReplaceMissing(data.bez_f, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@bez_e", ReplaceMissing(data.bez_e, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

				Return success

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			End Try

			Return success

		End Function

		Function DeleteCustomerInvoiceTypeData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteCustomerInvoiceTypeData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_Faktura "
			sql &= "Where ID = @ID"

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(recid, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

				Return success

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			End Try

			Return success

		End Function

#End Region



#Region "customer employee number"

		Function LoadCustomerNumberOfEmployeesData() As IEnumerable(Of Customer.DataObjects.NumberOfEmployeesData) Implements ITablesDatabaseAccess.LoadCustomerNumberOfEmployeesData

			Dim result As List(Of Customer.DataObjects.NumberOfEmployeesData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Bezeichnung FROM Tab_KDMAAnz ORDER BY ID"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of Customer.DataObjects.NumberOfEmployeesData)

					While reader.Read()
						Dim data As New Customer.DataObjects.NumberOfEmployeesData
						data.ID = SafeGetInteger(reader, "ID", 0)
						data.NumberOfEmployees = SafeGetString(reader, "Bezeichnung")

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

		Function AddCustomerNumberOfEmployeesData(ByVal data As Customer.DataObjects.NumberOfEmployeesData) As Boolean Implements ITablesDatabaseAccess.AddCustomerNumberOfEmployeesData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.Tab_KDMAAnz (Bezeichnung) Values ("
			sql &= "@NumberOfEmployees"
			sql &= ")"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@NumberOfEmployees", ReplaceMissing(data.NumberOfEmployees, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateCustomerNumberOfEmployeesData(ByVal data As Customer.DataObjects.NumberOfEmployeesData) As Boolean Implements ITablesDatabaseAccess.UpdateCustomerNumberOfEmployeesData

			Dim success = True

			Dim sql As String

			sql = "Update dbo.Tab_KDMAAnz Set Bezeichnung = @NumberOfEmployees "
			sql &= "Where ID = @ID"

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.ID, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@NumberOfEmployees", ReplaceMissing(data.NumberOfEmployees, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

				Return success

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			End Try

			Return success

		End Function

		Function DeleteCustomerNumberOfEmployeesData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteCustomerNumberOfEmployeesData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_KDMAAnz "
			sql &= "Where ID = @ID"

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(recid, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

				Return success

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			End Try

			Return success

		End Function

#End Region



#Region "customer OP shipment"

		Function LoadCustomerInvoiceShipment() As IEnumerable(Of Customer.DataObjects.OPShipmentData) Implements ITablesDatabaseAccess.LoadCustomerInvoiceShipment

			Dim result As List(Of Customer.DataObjects.OPShipmentData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Bezeichnung, Bez_d, Bez_I, Bez_F, Bez_E FROM dbo.Tab_OPVersand Order By Bezeichnung"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of Customer.DataObjects.OPShipmentData)

					While reader.Read

						Dim data = New Customer.DataObjects.OPShipmentData()
						data.ID = SafeGetInteger(reader, "ID", 0)
						data.Description = SafeGetString(reader, "Bezeichnung")
						data.bez_d = SafeGetString(reader, "Bez_d")
						data.bez_i = SafeGetString(reader, "Bez_I")
						data.bez_f = SafeGetString(reader, "Bez_F")
						data.bez_e = SafeGetString(reader, "Bez_e")


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

		Function AddCustomerInvoiceShipment(ByVal data As Customer.DataObjects.OPShipmentData) As Boolean Implements ITablesDatabaseAccess.AddCustomerInvoiceShipment

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.Tab_OPVersand (Bezeichnung, Bez_d, Bez_I, Bez_F, Bez_E) Values ("
			sql &= "@Description"
			sql &= ", @Bez_d"
			sql &= ", @Bez_I"
			sql &= ", @Bez_F"
			sql &= ", @Bez_E)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Description", ReplaceMissing(data.Description, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_d", ReplaceMissing(data.bez_d, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_I", ReplaceMissing(data.bez_i, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_F", ReplaceMissing(data.bez_f, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_E", ReplaceMissing(data.bez_e, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateCustomerInvoiceShipment(ByVal data As Customer.DataObjects.OPShipmentData) As Boolean Implements ITablesDatabaseAccess.UpdateCustomerInvoiceShipment

			Dim success = True

			Dim sql As String

			sql = "Update dbo.Tab_OPVersand Set Bezeichnung = @Description, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID"

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.ID, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Description", ReplaceMissing(data.Description, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Bez_d", ReplaceMissing(data.bez_d, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Bez_I", ReplaceMissing(data.bez_i, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Bez_F", ReplaceMissing(data.bez_f, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Bez_E", ReplaceMissing(data.bez_e, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

				Return success

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			End Try

			Return success

		End Function

		Function DeleteCustomerInvoiceShipment(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteCustomerInvoiceShipment

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_OPVersand "
			sql &= "Where ID = @ID"

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(recid, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

				Return success

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			End Try

			Return success

		End Function

#End Region






	End Class


End Namespace
