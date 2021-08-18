Imports SP.DatabaseAccess.Employee.DataObjects.ContactMng
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects.BankMng

Namespace Employee

    Partial Public Class EmployeeDatabaseAccess
        Inherits DatabaseAccessBase
        Implements IEmployeeDatabaseAccess

    ''' <summary>
    ''' Loads employee bank data (MA_Bank).
    ''' </summary>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <param name="bankRecordNumber">Optional bank record number.</param>
    ''' <returns>Employee bank data or nothing in error case.</returns>
		Function LoadEmployeeBanks(ByVal employeeNumber As Integer, Optional ByVal bankRecordNumber As Integer? = Nothing, Optional ByVal bankforAdvancePayment As Boolean? = Nothing) As IEnumerable(Of EmployeeBankData) Implements IEmployeeDatabaseAccess.LoadEmployeeBanks

			Dim result As List(Of EmployeeBankData) = Nothing

			Dim sql As String

			sql = "SELECT MAB.*, MAL.ZahlArt FROM MA_Bank MAB Left Join MA_LOSetting MAL On MAB.MANr = MAL.MANr WHERE MAB.MANr = @maNumber AND (@bankRecordNumber IS NULL OR  MAB.RecNr = @bankRecordNumber) ORDER BY "
			If bankforAdvancePayment.HasValue AndAlso bankforAdvancePayment Then
				sql &= "MAB.BnkZG DESC, ActiveRec DESC, MAB.RecNr Asc"
			Else
				sql &= "MAB.RecNr Asc"
			End If


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@maNumber", ReplaceMissing(employeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@bankRecordNumber", ReplaceMissing(bankRecordNumber, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeBankData)

					While reader.Read()
						Dim bankData As New EmployeeBankData
						bankData.ID = SafeGetInteger(reader, "ID", 0)
						bankData.EmployeeNumber = SafeGetInteger(reader, "MANr", Nothing)
						bankData.RecordNumber = SafeGetShort(reader, "RecNr", Nothing)
						bankData.Bank = SafeGetString(reader, "Bank", Nothing)
						bankData.BankLocation = SafeGetString(reader, "BANKORT", Nothing)
						bankData.DTABCNR = SafeGetString(reader, "DTABCNR", Nothing)
						bankData.AccountNr = SafeGetString(reader, "KONTONR", Nothing)
						bankData.DTAAdr1 = SafeGetString(reader, "DTAADR1", Nothing)
						bankData.DTAAdr2 = SafeGetString(reader, "DTAADR2", Nothing)
						bankData.DTAAdr3 = SafeGetString(reader, "DTAADR3", Nothing)
						bankData.DTAAdr4 = SafeGetString(reader, "DTAADR4", Nothing)
						bankData.ActiveRec = SafeGetBoolean(reader, "ActiveRec", False)
						bankData.Result = SafeGetString(reader, "Result", Nothing)
						bankData.BankZG = SafeGetBoolean(reader, "BnkZG", False)
						bankData.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						bankData.CreatedFrom = SafeGetString(reader, "CreatedFrom", Nothing)
						bankData.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
						bankData.ChangedFrom = SafeGetString(reader, "ChangedFrom", Nothing)
						bankData.BankAU = SafeGetBoolean(reader, "BnkAU", False)
						bankData.IBANNr = SafeGetString(reader, "IBANNr", Nothing)
						bankData.Swift = SafeGetString(reader, "Swift", Nothing)
						bankData.BLZ = SafeGetString(reader, "BLZ", Nothing)
						bankData.LMLAnr = SafeGetString(reader, "LMLANr", Nothing)
						bankData.BnkLOL = SafeGetBoolean(reader, "BnkLOL", False)

						bankData.zahlart = SafeGetString(reader, "zahlart")

						result.Add(bankData)

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
    ''' Adds an employee bank.
    ''' </summary>
    ''' <param name="bankData">The employee bank data.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Function AddEmployeeBank(ByVal bankData As EmployeeBankData) As Boolean Implements IEmployeeDatabaseAccess.AddEmployeeBank

      Dim success = True

      Dim sql As String

      sql = "[Create New MA_Bank]"

      ' Parameters

      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("@MANr", ReplaceMissing(bankData.EmployeeNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Bank", ReplaceMissing(bankData.Bank, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@BankOrt", ReplaceMissing(bankData.BankLocation, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@DTABCNR", ReplaceMissing(bankData.DTABCNR, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@KONTONR", ReplaceMissing(bankData.AccountNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@DTAADR1", ReplaceMissing(bankData.DTAAdr1, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@DTAADR2", ReplaceMissing(bankData.DTAAdr2, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@DTAADR3", ReplaceMissing(bankData.DTAAdr3, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@DTAADR4", ReplaceMissing(bankData.DTAAdr4, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ActiveRec", ReplaceMissing(bankData.ActiveRec, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Result", ReplaceMissing(bankData.Result, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@BnkZG", ReplaceMissing(bankData.BankZG, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@CreatedOn", ReplaceMissing(bankData.CreatedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@CreatedFrom", ReplaceMissing(bankData.CreatedFrom, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ChangedOn", ReplaceMissing(bankData.ChangedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ChangedFrom", ReplaceMissing(bankData.ChangedFrom, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@BnkAU", ReplaceMissing(bankData.BankAU, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@IBANNr", ReplaceMissing(bankData.IBANNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Swift", ReplaceMissing(bankData.Swift, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@BLZ", ReplaceMissing(bankData.BLZ, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@LMLANr", ReplaceMissing(bankData.LMLAnr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@BnkLOL", ReplaceMissing(bankData.BnkLOL, DBNull.Value)))

      Dim newIdParameter = New SqlClient.SqlParameter("@NewBankId", SqlDbType.Int)
      newIdParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newIdParameter)

      Dim recNrParameter = New SqlClient.SqlParameter("@RecNr ", SqlDbType.Int)
      recNrParameter.Direction = ParameterDirection.Output
      listOfParams.Add(recNrParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If Not newIdParameter.Value Is Nothing AndAlso
          Not recNrParameter.Value Is Nothing Then
        bankData.ID = CType(newIdParameter.Value, Integer)
        bankData.RecordNumber = CType(recNrParameter.Value, Integer)
      Else
        success = False
      End If

      Return success

    End Function

    ''' <summary>
    ''' Update bank data of an employee.
    ''' </summary>
    ''' <param name="bankData">The bank data.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Function UpdateEmployeeBank(ByVal bankData As EmployeeBankData) As Boolean Implements IEmployeeDatabaseAccess.UpdateEmployeeBank

      Dim success = True

      Dim sql As String

      sql = "[UPDATE MA_Bank]"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)

      listOfParams.Add(New SqlClient.SqlParameter("@ID_MA_Bank", bankData.ID))
      listOfParams.Add(New SqlClient.SqlParameter("@MANr", ReplaceMissing(bankData.EmployeeNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RecNr", ReplaceMissing(bankData.RecordNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@BANK", ReplaceMissing(bankData.Bank, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@BANKORT", ReplaceMissing(bankData.BankLocation, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@DTABCNR", ReplaceMissing(bankData.DTABCNR, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@KONTONR", ReplaceMissing(bankData.AccountNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@DTAADR1", ReplaceMissing(bankData.DTAAdr1, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@DTAADR2", ReplaceMissing(bankData.DTAAdr2, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@DTAADR3", ReplaceMissing(bankData.DTAAdr3, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@DTAADR4", ReplaceMissing(bankData.DTAAdr4, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ActiveRec", ReplaceMissing(bankData.ActiveRec, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Result", ReplaceMissing(bankData.Result, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@BnkZG", ReplaceMissing(bankData.BankZG, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@CreatedOn", ReplaceMissing(bankData.CreatedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@CreatedFrom", ReplaceMissing(bankData.CreatedFrom, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ChangedOn", ReplaceMissing(bankData.ChangedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ChangedFrom", ReplaceMissing(bankData.ChangedFrom, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@BnkAU", ReplaceMissing(bankData.BankAU, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@IBANNr", ReplaceMissing(bankData.IBANNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Swift", ReplaceMissing(bankData.Swift, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@BLZ", ReplaceMissing(bankData.BLZ, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@LMLANr", ReplaceMissing(bankData.LMLAnr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@BnkLOL", ReplaceMissing(bankData.BnkLOL, DBNull.Value)))

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      Return success

    End Function

    ''' <summary>
    ''' Deletes employee bank data.
    ''' </summary>
    ''' <param name="id">The bank data id.</param>
    ''' <param name="modul">The module.</param>
    ''' <param name="username">The username.</param>
    ''' <param name="usnr">USnr number.</param>
    ''' <returns>Boolean value indicating success.</returns>
    Function DeleteEmployeeBank(ByVal id As Integer, ByVal modul As String, ByVal username As String, ByVal usnr As Integer) As DeleteEmployeeBankResult Implements IEmployeeDatabaseAccess.DeleteEmployeeBank

      Dim success = True

      Dim sql As String

      sql = "[Delete MA_Bank]"

      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("id", id))
      listOfParams.Add(New SqlClient.SqlParameter("modul", modul))
      listOfParams.Add(New SqlClient.SqlParameter("username", username))
      listOfParams.Add(New SqlClient.SqlParameter("usnr", usnr))

      Dim resultParameter = New SqlClient.SqlParameter("@Result", SqlDbType.Int)
      resultParameter.Direction = ParameterDirection.Output
      listOfParams.Add(resultParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      Dim resultEnum As DeleteEmployeeBankResult

      If Not resultParameter.Value Is Nothing Then
        Try
          resultEnum = CType(resultParameter.Value, DeleteEmployeeBankResult)
        Catch
          resultEnum = DeleteEmployeeBankResult.ErrorWhileDelete
        End Try
      Else
        resultEnum = DeleteEmployeeBankResult.ErrorWhileDelete
      End If

      Return resultEnum

    End Function

    ''' <summary>
    ''' Search for existing bank data (Bankenstamm).
    ''' </summary>
    ''' <param name="clearingNumber">The clearing number.</param>
    ''' <param name="bankName">The bank name.</param>
    ''' <param name="bankPostcode">The bank postcode.</param>
    ''' <param name="bankLocation">The bank location.</param>
    ''' <param name="swift">The bank swift.</param>
    ''' <returns>List of bank search result data or nothing in error case.</returns>
    Function SearchBankData(ByVal clearingNumber As String, ByVal bankName As String, ByVal bankPostcode As String, ByVal bankLocation As String, ByVal swift As String) As IEnumerable(Of SearchBankDataResult) Implements IEmployeeDatabaseAccess.SearchBankData

      Dim result As List(Of SearchBankDataResult) = Nothing

      Dim sql As String

      sql = "[Get Search Bank Data]"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("@ClearingNumber", ReplaceMissing(clearingNumber, String.Empty)))
      listOfParams.Add(New SqlClient.SqlParameter("@BankName", ReplaceMissing(bankName, String.Empty)))
      listOfParams.Add(New SqlClient.SqlParameter("@PLZ", ReplaceMissing(bankPostcode, String.Empty)))
      listOfParams.Add(New SqlClient.SqlParameter("@Ort", ReplaceMissing(bankLocation, String.Empty)))
      listOfParams.Add(New SqlClient.SqlParameter("@Swift", ReplaceMissing(swift, String.Empty)))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of SearchBankDataResult)

          While reader.Read()
            Dim searchData As New SearchBankDataResult
            searchData.ClearingNumber = SafeGetString(reader, "ClearingNr")
            searchData.BankName = SafeGetString(reader, "BankName")
            searchData.Postcode = SafeGetString(reader, "BankPLZ")
            searchData.Location = SafeGetString(reader, "BankOrt")
            searchData.Swift = SafeGetString(reader, "Swift")
            searchData.Telephone = SafeGetString(reader, "Telefon")
            searchData.Telefax = SafeGetString(reader, "Telefax")
            searchData.PostAccount = SafeGetString(reader, "Postkonto")

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

    End Class

End Namespace