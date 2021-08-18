Imports SP.DatabaseAccess
Imports System.Text
Imports SP.Infrastructure
Imports SP.DatabaseAccess.Invoice.DataObjects
Imports SP.DatabaseAccess.AdvancePaymentMng.DataObjects

Namespace AdvancePaymentMng

  Partial Public Class AdvancePaymentDatabaseAccess
    Inherits DatabaseAccessBase
    Implements IAdvancePaymentDatabaseAccess

#Region "Constructor"

    ''' <summary>
    ''' Constructor.
    ''' </summary>
    ''' <param name="connectionString">The connection string.</param>
    ''' <param name="translationLanguage">The translation language.</param>
    Public Sub New(ByVal connectionString As String, ByVal translationLanguage As Language)
      MyBase.New(connectionString, translationLanguage)

    End Sub

    ''' <summary>
    ''' Constructor.
    ''' </summary>
    ''' <param name="connectionString">The connection string.</param>
    ''' <param name="translationLanguage">The translation language.</param>
    Public Sub New(ByVal connectionString As String, ByVal translationLanguage As String)
      MyBase.New(connectionString, translationLanguage)
    End Sub

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Loads the ZG master data.
    ''' </summary>
    ''' <param name="zgNr">The ZG number.</param>
    ''' <returns>The ZG master data or nothing in error case.</returns>
    Public Function LoadZGMasterData(ByVal zgNr As Integer) As ZGMasterData Implements IAdvancePaymentDatabaseAccess.LoadZGMasterData

      Dim zgMasterData As ZGMasterData = Nothing

      Dim sql As String = String.Empty

      sql = sql & "SELECT "
      sql = sql & "[ID]"
      sql = sql & ",[RPNR]"
      sql = sql & ",[ZGNr]"
      sql = sql & ",[MANR]"
      sql = sql & ",[LANR]"
      sql = sql & ",[LONR]"
      sql = sql & ",[VGNR]"
      sql = sql & ",[ZGGRUND]"
      sql = sql & ",[Betrag]"
      sql = sql & ",[Anzahl]"
      sql = sql & ",[Ansatz]"
      sql = sql & ",[Basis]"
      sql = sql & ",[Currency]"
      sql = sql & ",[LP]"
      sql = sql & ",[JAHR]"
      sql = sql & ",[Aus_Dat]"
      sql = sql & ",[ClearingNr]"
      sql = sql & ",[Bank]"
      sql = sql & ",[KontoNr]"
      sql = sql & ",[BankOrt]"
      sql = sql & ",[DTAAdr1]"
      sql = sql & ",[DTAAdr2]"
      sql = sql & ",[DTAAdr3]"
      sql = sql & ",[DTAAdr4]"
      sql = sql & ",[N2Char]"
      sql = sql & ",[1000000]"
      sql = sql & ",[100000]"
      sql = sql & ",[10000]"
      sql = sql & ",[1000]"
      sql = sql & ",[100]"
      sql = sql & ",[10]"
      sql = sql & ",[1]"
      sql = sql & ",[USName]"
      sql = sql & ",[Result]"
      sql = sql & ",[CheckNumber]"
      sql = sql & ",[GebAbzug]"
      sql = sql & ",[CreatedOn]"
      sql = sql & ",[CreatedFrom]"
      sql = sql & ",[ChangedOn]"
      sql = sql & ",[ChangedFrom]"
      sql = sql & ",[DTA_Dat]"
      sql = sql & ",[BnkAU]"
      sql = sql & ",[DTADate]"
      sql = sql & ",[IBANNr]"
      sql = sql & ",[Swift]"
      sql = sql & ",[BLZ]"
      sql = sql & ",[Printed_Dat]"
      sql = sql & ",[MDNr] "
      sql = sql & ",[IsCreatedWithLO]"
      sql = sql & ", (SELECT COUNT(*) FROM MonthClose WHERE Monat = [ZG].[LP] AND Jahr = CAST([ZG].[Jahr] as int) And MDNr = [ZG].[MDNr]) AS IsMonthClosed "
      sql = sql & "FROM [dbo].[ZG] "
      sql = sql & "WHERE ZGNr = @zgNr"

      ' Parameters
      Dim zgNumberParameter As New SqlClient.SqlParameter("zgNr", zgNr)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(zgNumberParameter)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If Not reader Is Nothing Then

          If reader.Read Then
            zgMasterData = New ZGMasterData

            zgMasterData.ID = SafeGetInteger(reader, "ID", 0)
            zgMasterData.RPNR = SafeGetInteger(reader, "RPNR", Nothing)
            zgMasterData.ZGNr = SafeGetInteger(reader, "ZGNr", Nothing)
            zgMasterData.MANR = SafeGetInteger(reader, "MANR", Nothing)
            zgMasterData.LANR = SafeGetInteger(reader, "LANR", Nothing)
            zgMasterData.LONR = SafeGetInteger(reader, "LONR", Nothing)
            zgMasterData.VGNR = SafeGetInteger(reader, "VGNR", Nothing)
						zgMasterData.ZGGRUND = SafeGetString(reader, "ZGGRUND", "")
            zgMasterData.Betrag = SafeGetDecimal(reader, "Betrag", Nothing)
            zgMasterData.Anzahl = SafeGetInteger(reader, "Anzahl", Nothing)
            zgMasterData.Ansatz = SafeGetInteger(reader, "Ansatz", Nothing)
            zgMasterData.Basis = SafeGetDecimal(reader, "Basis", Nothing)
            zgMasterData.Currency = SafeGetString(reader, "Currency")
						zgMasterData.LP = SafeGetInteger(reader, "LP", Nothing)
            zgMasterData.JAHR = SafeGetString(reader, "JAHR", Nothing)
            zgMasterData.Aus_Dat = SafeGetDateTime(reader, "Aus_Dat", Nothing)
            zgMasterData.ClearingNr = SafeGetInteger(reader, "ClearingNr", Nothing)
						zgMasterData.Bank = SafeGetString(reader, "Bank", "")
						zgMasterData.KontoNr = SafeGetString(reader, "KontoNr", "")
						zgMasterData.BankOrt = SafeGetString(reader, "BankOrt", "")
						zgMasterData.DTAAdr1 = SafeGetString(reader, "DTAAdr1", "")
						zgMasterData.DTAAdr2 = SafeGetString(reader, "DTAAdr2", "")
						zgMasterData.DTAAdr3 = SafeGetString(reader, "DTAAdr3", "")
						zgMasterData.DTAAdr4 = SafeGetString(reader, "DTAAdr4", "")
						zgMasterData.N2Char = SafeGetString(reader, "N2Char", "")
            zgMasterData._1000000 = SafeGetString(reader, "1000000")
            zgMasterData._100000 = SafeGetString(reader, "100000")
            zgMasterData._10000 = SafeGetString(reader, "10000")
            zgMasterData._1000 = SafeGetString(reader, "1000")
            zgMasterData._100 = SafeGetString(reader, "100")
            zgMasterData._10 = SafeGetString(reader, "10")
            zgMasterData._1 = SafeGetString(reader, "1")
            zgMasterData.USName = SafeGetString(reader, "USName")
            zgMasterData.Result = SafeGetString(reader, "Result")
						zgMasterData.CheckNumber = SafeGetString(reader, "CheckNumber", "")
            zgMasterData.GebAbzug = SafeGetBoolean(reader, "GebAbzug", Nothing)
            zgMasterData.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
            zgMasterData.CreatedFrom = SafeGetString(reader, "CreatedFrom", Nothing)
            zgMasterData.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
            zgMasterData.ChangedFrom = SafeGetString(reader, "ChangedFrom", Nothing)
            zgMasterData.DTA_Dat = SafeGetDateTime(reader, "DTA_Dat", Nothing)
						zgMasterData.BnkAU = SafeGetBoolean(reader, "BnkAU", 0)
            zgMasterData.DTADate = SafeGetDateTime(reader, "DTADate", Nothing)
						zgMasterData.IBANNr = SafeGetString(reader, "IBANNr", "")
						zgMasterData.Swift = SafeGetString(reader, "Swift", "")
						zgMasterData.BLZ = SafeGetString(reader, "BLZ", "")
            zgMasterData.Printed_Dat = SafeGetDateTime(reader, "Printed_Dat", Nothing)
            zgMasterData.MDNr = SafeGetInteger(reader, "MDNr", Nothing)

          End If

        End If

      Catch ex As Exception
        m_Logger.LogError(ex.tostring())
        zgMasterData = Nothing
      Finally
        CloseReader(reader)
      End Try

      Return zgMasterData

    End Function

		Function LoadAssignedZGMasterData(ByVal zgNumbers As List(Of Integer)) As IEnumerable(Of ZGMasterData) Implements IAdvancePaymentDatabaseAccess.LoadAssignedZGMasterData

			Dim zgMasterData As List(Of ZGMasterData) = Nothing

			Dim sql As String
			Dim zgNumbersBuffer As String = String.Empty

			For Each number In zgNumbers

				zgNumbersBuffer = zgNumbersBuffer & IIf(zgNumbersBuffer <> "", ", ", "") & number

			Next

			sql = "SELECT "
			sql &= "ZG.[ID]"
			sql &= ",ZG.[RPNR]"
			sql &= ",ZG.[ZGNr]"
			sql &= ",ZG.[MANR]"
			sql &= ",ZG.[LANR]"
			sql &= ",ZG.[LONR]"
			sql &= ",ZG.[VGNR]"
			sql &= ",ZG.[ZGGRUND]"
			sql &= ",ZG.[Betrag]"
			sql &= ",ZG.[Anzahl]"
			sql &= ",ZG.[Ansatz]"
			sql &= ",ZG.[Basis]"
			sql &= ",ZG.[Currency]"
			sql &= ",ZG.[LP]"
			sql &= ",ZG.[JAHR]"
			sql &= ",ZG.[Aus_Dat]"
			sql &= ",ZG.[ClearingNr]"
			sql &= ",ZG.[Bank]"
			sql &= ",ZG.[KontoNr]"
			sql &= ",ZG.[BankOrt]"
			sql &= ",ZG.[DTAAdr1]"
			sql &= ",ZG.[DTAAdr2]"
			sql &= ",ZG.[DTAAdr3]"
			sql &= ",ZG.[DTAAdr4]"
			sql &= ",ZG.[N2Char]"
			sql &= ",ZG.[1000000]"
			sql &= ",ZG.[100000]"
			sql &= ",ZG.[10000]"
			sql &= ",ZG.[1000]"
			sql &= ",ZG.[100]"
			sql &= ",ZG.[10]"
			sql &= ",ZG.[1]"
			sql &= ",ZG.[USName]"
			sql &= ",ZG.[Result]"
			sql &= ",ZG.[CheckNumber]"
			sql &= ",ZG.[GebAbzug]"
			sql &= ",ZG.[CreatedOn]"
			sql &= ",ZG.[CreatedFrom]"
			sql &= ",ZG.[ChangedOn]"
			sql &= ",ZG.[ChangedFrom]"
			sql &= ",ZG.[DTA_Dat]"
			sql &= ",ZG.[BnkAU]"
			sql &= ",ZG.[DTADate]"
			sql &= ",ZG.[IBANNr]"
			sql &= ",ZG.[Swift]"
			sql &= ",ZG.[BLZ]"
			sql &= ",ZG.[Printed_Dat]"
			sql &= ",ZG.[MDNr] "
			sql &= ",ZG.[IsCreatedWithLO]"
			sql &= ", (SELECT COUNT(*) FROM MonthClose WHERE Monat = [ZG].[LP] AND Jahr = CAST([ZG].[Jahr] as int) And MDNr = [ZG].[MDNr]) AS IsMonthClosed "
			sql &= ", MA.Nachname "
			sql &= ", MA.Vorname "

			sql &= "FROM [dbo].[ZG] "
			sql &= "Left Join Mitarbeiter MA On MA.MANr = ZG.MANr "
			sql &= String.Format("WHERE ZG.ZGNr In ({0}) ", zgNumbersBuffer)
			sql &= "Order By ZG.Aus_Dat DESC"


			' Parameters

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If Not reader Is Nothing Then
					zgMasterData = New List(Of ZGMasterData)

					While reader.Read
						Dim viewData = New ZGMasterData

						viewData.ID = SafeGetInteger(reader, "ID", 0)
						viewData.RPNR = SafeGetInteger(reader, "RPNR", Nothing)
						viewData.ZGNr = SafeGetInteger(reader, "ZGNr", Nothing)
						viewData.MANR = SafeGetInteger(reader, "MANR", Nothing)
						viewData.LANR = SafeGetInteger(reader, "LANR", Nothing)
						viewData.LONR = SafeGetInteger(reader, "LONR", Nothing)
						viewData.VGNR = SafeGetInteger(reader, "VGNR", Nothing)
						viewData.ZGGRUND = SafeGetString(reader, "ZGGRUND", "")
						viewData.Betrag = SafeGetDecimal(reader, "Betrag", Nothing)
						viewData.Anzahl = SafeGetInteger(reader, "Anzahl", Nothing)
						viewData.Ansatz = SafeGetInteger(reader, "Ansatz", Nothing)
						viewData.Basis = SafeGetDecimal(reader, "Basis", Nothing)
						viewData.Currency = SafeGetString(reader, "Currency")
						viewData.LP = SafeGetInteger(reader, "LP", Nothing)
						viewData.JAHR = SafeGetString(reader, "JAHR", Nothing)
						viewData.Aus_Dat = SafeGetDateTime(reader, "Aus_Dat", Nothing)
						viewData.ClearingNr = SafeGetInteger(reader, "ClearingNr", Nothing)
						viewData.Bank = SafeGetString(reader, "Bank", "")
						viewData.KontoNr = SafeGetString(reader, "KontoNr", "")
						viewData.BankOrt = SafeGetString(reader, "BankOrt", "")
						viewData.DTAAdr1 = SafeGetString(reader, "DTAAdr1", "")
						viewData.DTAAdr2 = SafeGetString(reader, "DTAAdr2", "")
						viewData.DTAAdr3 = SafeGetString(reader, "DTAAdr3", "")
						viewData.DTAAdr4 = SafeGetString(reader, "DTAAdr4", "")
						viewData.N2Char = SafeGetString(reader, "N2Char", "")
						viewData._1000000 = SafeGetString(reader, "1000000")
						viewData._100000 = SafeGetString(reader, "100000")
						viewData._10000 = SafeGetString(reader, "10000")
						viewData._1000 = SafeGetString(reader, "1000")
						viewData._100 = SafeGetString(reader, "100")
						viewData._10 = SafeGetString(reader, "10")
						viewData._1 = SafeGetString(reader, "1")
						viewData.USName = SafeGetString(reader, "USName")
						viewData.Result = SafeGetString(reader, "Result")
						viewData.CheckNumber = SafeGetString(reader, "CheckNumber", "")
						viewData.GebAbzug = SafeGetBoolean(reader, "GebAbzug", Nothing)
						viewData.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						viewData.CreatedFrom = SafeGetString(reader, "CreatedFrom", Nothing)
						viewData.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
						viewData.ChangedFrom = SafeGetString(reader, "ChangedFrom", Nothing)
						viewData.DTA_Dat = SafeGetDateTime(reader, "DTA_Dat", Nothing)
						viewData.BnkAU = SafeGetBoolean(reader, "BnkAU", 0)
						viewData.DTADate = SafeGetDateTime(reader, "DTADate", Nothing)
						viewData.IBANNr = SafeGetString(reader, "IBANNr", "")
						viewData.Swift = SafeGetString(reader, "Swift", "")
						viewData.BLZ = SafeGetString(reader, "BLZ", "")
						viewData.Printed_Dat = SafeGetDateTime(reader, "Printed_Dat", Nothing)
						viewData.MDNr = SafeGetInteger(reader, "MDNr", Nothing)
						viewData.EmployeeLastname = SafeGetString(reader, "Nachname")
						viewData.EmployeeFirstname = SafeGetString(reader, "Vorname")


						zgMasterData.Add(viewData)

					End While

				End If

			Catch ex As Exception
				m_Logger.LogError(ex.tostring())
				zgMasterData = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return zgMasterData

		End Function

		''' <summary>
		''' Loads employee data.
		''' </summary>
		''' <returns>List of employee data.</returns>
		Public Function LoadEmployeeData() As IEnumerable(Of EmployeeData) Implements IAdvancePaymentDatabaseAccess.LoadEmployeeData

			Dim result As List(Of EmployeeData) = Nothing

			Dim sql As String

      sql = "SELECT MA.MANr, MA.Nachname, MA.Vorname, MA.PLZ, MA.Ort FROM Mitarbeiter MA Left Join MA_LoSetting MALO On MA.MANr = MALO.MANr "
      sql &= "Where IsNull(MALO.NoZG, 0) = 0 And IsNull(MA.ShowAsApplicant, 0) = 0 ORDER BY MA.Nachname, MA.Vorname"


      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeData)

					While reader.Read

						Dim employeeData = New EmployeeData()
						employeeData.EmployeeNumber = SafeGetInteger(reader, "MANr", 0)
						employeeData.LastName = SafeGetString(reader, "Nachname")
						employeeData.Firstname = SafeGetString(reader, "Vorname")
						employeeData.Postcode = SafeGetString(reader, "PLZ")
						employeeData.Location = SafeGetString(reader, "Ort")

						result.Add(employeeData)

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
		''' Loads Guthaben values for advance payment.
		''' </summary>
		''' <param name="mdNumber">The mandant number.</param>
		''' <param name="employeeNumber">The employee number.</param>
		''' <param name="month">The month.</param>
		''' <param name="year">The year.</param>
		''' <returns>The Guthaben values.</returns>
		Function LoadGuthabenValuesForAdvancePayment(ByVal mdNumber As Integer, ByVal employeeNumber As Integer, ByVal month As Integer, ByVal year As Integer) As BalanceValues Implements IAdvancePaymentDatabaseAccess.LoadGuthabenValuesForAdvancePayment

			Dim result As BalanceValues = Nothing

			Dim sql As String = "[Get Guthaben Values For AdvancePayment]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNumber))
			listOfParams.Add(New SqlClient.SqlParameter("maNr", employeeNumber))
			listOfParams.Add(New SqlClient.SqlParameter("month", month))
			listOfParams.Add(New SqlClient.SqlParameter("year", year))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If Not reader Is Nothing Then

					If reader.Read Then
						result = New BalanceValues

						result.ReportHours = SafeGetDecimal(reader, "Rapportstunden", 0)
						result.ReportTotal = SafeGetDecimal(reader, "RapportTotal", 0)
						result.Q_ZHLG = SafeGetDecimal(reader, "Q_ZHLG", 100)
						result.N_ZHLG = SafeGetDecimal(reader, "N_ZHLG", 100)
						result.Total = SafeGetDecimal(reader, "Total", 0)

					End If

				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				result = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		''' <summary>
		''' Loads the LA data.
		''' </summary>
		''' <returns>List of LA data or nothing in error case.</returns>
		Public Function LoadLAData() As IEnumerable(Of LAData) Implements IAdvancePaymentDatabaseAccess.LoadLAData

			Dim result As List(Of LAData) = Nothing

			Dim sql As String = String.Empty

			sql = sql & "Select DISTINCT LANr, LAText FROM LA WHERE Verwendung = 4 And LADeactivated = 0 ORDER BY LAText"


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of LAData)

					While reader.Read

						Dim laData = New LAData
						laData.LANr = SafeGetDecimal(reader, "LANr", 0)
						laData.LAText = SafeGetString(reader, "LAText")

						result.Add(laData)

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
		''' Loads invalid month for advance payment.
		''' </summary>
		''' <param name="year">The year.</param>
		''' <param name="employeeNumber">The employee number.</param>
		''' <param name="mdNumber">The mandant number.</param>
		''' <returns>List of invalid month or nothing in error case.</returns>
		Public Function LoadInvalidMonthForAdvancePayment(ByVal year As Integer, ByVal employeeNumber As Integer, ByVal mdNumber As Integer) As IEnumerable(Of Integer) Implements IAdvancePaymentDatabaseAccess.LoadInvalidMonthForAdvancePayment

			Dim result As List(Of Integer) = Nothing

			Dim sql As String = String.Empty

			sql = sql & "[Get Invalid Month for AdvancePayment]"


			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Year", year))
			listOfParams.Add(New SqlClient.SqlParameter("@MANr", employeeNumber))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", mdNumber))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of Integer)

					While reader.Read

						Dim month = SafeGetInteger(reader, "Month", "0")

						result.Add(Convert.ToInt32(month))

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
		''' Load number of ES of Employee for a month.
		''' </summary> 
		''' <param name="year">The year.</param>
		''' <param name="month">The month.</param>
		''' <param name="employeeNumber">The employee number.</param>
		''' <param name="mdNumber">The mandant number.</param>
		''' <returns>Number of ES.</returns>
		Public Function LoadNumberOfESOfEmployeeForMonth(ByVal year As Integer, ByVal month As Integer, ByVal employeeNumber As Integer, ByVal mdNumber As Integer) As Integer? Implements IAdvancePaymentDatabaseAccess.LoadNumberOfESOfEmployeeForMonth

			Dim canBeDeleted = False

			Dim sql As String

			sql = "[Get Number Of ES of Employee for Month]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Year", year))
			listOfParams.Add(New SqlClient.SqlParameter("@Month", month))
			listOfParams.Add(New SqlClient.SqlParameter("@MANr", employeeNumber))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", mdNumber))

			Dim numberOfES = ExecuteScalar(sql, listOfParams, CommandType.StoredProcedure)

			Return numberOfES

		End Function

		''' <summary>
		''' Loads negative LM data.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <param name="month">The month.</param>
		''' <param name="year">The year.</param>
		''' <returns>List of negative LM data or nothing in error case.</returns>
		Public Function LoadNegativeLMData(ByVal employeeNumber As Integer, ByVal month As Integer, ByVal year As Integer) As IEnumerable(Of NegativeLMData) Implements IAdvancePaymentDatabaseAccess.LoadNegativeLMData

			Dim result As List(Of NegativeLMData) = Nothing

			Dim sql As String = String.Empty

			sql = sql & "[Get AbzugLMData For ZG]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MANr", employeeNumber))
			listOfParams.Add(New SqlClient.SqlParameter("@Monat", month))
			listOfParams.Add(New SqlClient.SqlParameter("@TempYear", year))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of NegativeLMData)

					While reader.Read

						Dim negativeLMData = New NegativeLMData

						negativeLMData.LMNR = SafeGetInteger(reader, "LMNR", Nothing)
						negativeLMData.MANR = SafeGetInteger(reader, "MANR", Nothing)
						negativeLMData.KST = SafeGetString(reader, "KST")
						negativeLMData.Jahr_Bis = SafeGetString(reader, "Jahr Bis")
						negativeLMData.Jahr_Von = SafeGetString(reader, "Jahr Von")
						negativeLMData.LANr = SafeGetDecimal(reader, "LANr", Nothing)
						negativeLMData.LP_Von = SafeGetInteger(reader, "LP_Von", Nothing)
						negativeLMData.LP_Bis = SafeGetInteger(reader, "LP_Bis", Nothing)
						negativeLMData.M_Btr = SafeGetDecimal(reader, "M_Btr", Nothing)
						negativeLMData.Suva = SafeGetString(reader, "Suva")
						negativeLMData.LAName = SafeGetString(reader, "LAName")
						negativeLMData.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						negativeLMData.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						negativeLMData.ChangedFrom = SafeGetString(reader, "ChangedFrom")
						negativeLMData.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)

						result.Add(negativeLMData)

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
		''' Loads payment reason texts.
		''' </summary>
		''' <param name="employeeNumber">The employee number number.</param>
		''' <param name="mdNumber">The mandant number.</param>
		''' <returns>List of payment reason texts.</returns>
		Public Function LoadPaymentReasonTexts(ByVal employeeNumber As Integer, ByVal mdNumber As Integer) As IEnumerable(Of PaymentReasonData) Implements IAdvancePaymentDatabaseAccess.LoadPaymentReasonTexts

			Dim result As List(Of PaymentReasonData) = Nothing

			Dim sql As String = String.Empty

			sql = sql & "(SELECT DISTINCT ZGGRUND FROM ZG WHERE MANR = @employeeNumber And MDNr = @mdNr And ZGGRUND Is Not NULL And ZGGRUND != '' "
			sql = sql & "UNION "
      sql = sql & "SELECT '') "
      sql = sql & "ORDER BY ZGGRUND ASC"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("employeeNumber", employeeNumber))
      listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNumber))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of PaymentReasonData)

          While reader.Read()
            Dim paymentReasonText As New PaymentReasonData
            paymentReasonText.ReasonText = SafeGetString(reader, "ZGGRUND")

            result.Add(paymentReasonText)

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
    ''' Adds a new ZG data.
    ''' </summary>
    ''' <param name="zgData">The ZG data.</param>
    ''' <param name="zgNumberOffset">The ZG number offset.</param>
    ''' <returns>Boolean truth value indicating success.</returns>
    Public Function AddNewZGData(ByVal zgData As ZGMasterData, ByVal zgNumberOffset As Integer) As Boolean Implements IAdvancePaymentDatabaseAccess.AddNewZGData

      Dim success = True

      Dim sql As String

      sql = "[Create New ZG]"

      ' Parameters

      Dim listOfParams As New List(Of SqlClient.SqlParameter)

      '' Data of ES
      listOfParams.Add(New SqlClient.SqlParameter("@RPNR", ReplaceMissing(zgData.RPNR, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@MANR", ReplaceMissing(zgData.MANR, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@LANR", ReplaceMissing(zgData.LANR, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@LONR", ReplaceMissing(zgData.LONR, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@VGNR", ReplaceMissing(zgData.VGNR, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ZGGRUND", ReplaceMissing(zgData.ZGGRUND, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Betrag", ReplaceMissing(zgData.Betrag, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Anzahl", ReplaceMissing(zgData.Anzahl, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Ansatz", ReplaceMissing(zgData.Ansatz, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Basis", ReplaceMissing(zgData.Basis, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Currency", ReplaceMissing(zgData.Currency, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@LP", ReplaceMissing(zgData.LP, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@JAHR", ReplaceMissing(zgData.JAHR, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Aus_Dat", ReplaceMissing(zgData.Aus_Dat, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ClearingNr", ReplaceMissing(zgData.ClearingNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Bank", ReplaceMissing(zgData.Bank, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@KontoNr", ReplaceMissing(zgData.KontoNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@BankOrt", ReplaceMissing(zgData.BankOrt, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@DTAAdr1", ReplaceMissing(zgData.DTAAdr1, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@DTAAdr2", ReplaceMissing(zgData.DTAAdr2, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@DTAAdr3", ReplaceMissing(zgData.DTAAdr3, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@DTAAdr4", ReplaceMissing(zgData.DTAAdr4, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@N2Char", ReplaceMissing(zgData.N2Char, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@_1000000", ReplaceMissing(zgData._1000000, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@_100000", ReplaceMissing(zgData._100000, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@_10000", ReplaceMissing(zgData._10000, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@_1000", ReplaceMissing(zgData._1000, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@_100", ReplaceMissing(zgData._100, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@_10", ReplaceMissing(zgData._10, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@_1", ReplaceMissing(zgData._1, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@USName", ReplaceMissing(zgData.USName, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Result", ReplaceMissing(zgData.Result, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@CheckNumber", ReplaceMissing(zgData.CheckNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@GebAbzug", ReplaceMissing(zgData.GebAbzug, 0)))
      listOfParams.Add(New SqlClient.SqlParameter("@CreatedOn", ReplaceMissing(zgData.CreatedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@CreatedFrom", ReplaceMissing(zgData.CreatedFrom, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ChangedOn", ReplaceMissing(zgData.ChangedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ChangedFrom", ReplaceMissing(zgData.ChangedFrom, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@DTA_Dat", ReplaceMissing(zgData.DTA_Dat, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@BnkAU", ReplaceMissing(zgData.BnkAU, 0)))
      listOfParams.Add(New SqlClient.SqlParameter("@DTADate", ReplaceMissing(zgData.DTADate, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@IBANNr", ReplaceMissing(zgData.IBANNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Swift", ReplaceMissing(zgData.Swift, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@BLZ", ReplaceMissing(zgData.BLZ, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Printed_Dat", ReplaceMissing(zgData.Printed_Dat, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@MDNr", ReplaceMissing(zgData.MDNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@IsCreatedWithLO", ReplaceMissing(zgData.IsCreatedWithLO, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ZGNumberOffset", ReplaceMissing(zgNumberOffset, DBNull.Value)))

      ' New ID of ZG
      Dim newIdParameter = New SqlClient.SqlParameter("@NewZGID ", SqlDbType.Int)
      newIdParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newIdParameter)

      ' New ZGNr
      Dim newZGNrParameter = New SqlClient.SqlParameter("@ZGNr ", SqlDbType.Int)
      newZGNrParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newZGNrParameter)
      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If success AndAlso
        Not newIdParameter.Value Is Nothing AndAlso
        Not newZGNrParameter Is Nothing Then
        zgData.ID = CType(newIdParameter.Value, Integer)
        zgData.ZGNr = CType(newZGNrParameter.Value, Integer)
      Else
        success = False
      End If

      Return success

    End Function

    ''' <summary>
    ''' Updates the ZG data.
    ''' </summary>
    ''' <param name="zgData">The ZG data.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function UpdateZGData(ByVal zgData As ZGMasterData) As Boolean Implements IAdvancePaymentDatabaseAccess.UpdateZGData

      Dim success = True

      Dim sql As String = String.Empty

      sql = sql & "[UPDATE ZG]"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)

      listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(zgData.ID, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPNR  ", ReplaceMissing(zgData.RPNR, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ZGNr", ReplaceMissing(zgData.ZGNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@MANR", ReplaceMissing(zgData.MANR, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@LANR", ReplaceMissing(zgData.LANR, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@LONR", ReplaceMissing(zgData.LONR, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@VGNR", ReplaceMissing(zgData.VGNR, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ZGGRUND", ReplaceMissing(zgData.ZGGRUND, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Betrag", ReplaceMissing(zgData.Betrag, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Anzahl", ReplaceMissing(zgData.Anzahl, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Ansatz", ReplaceMissing(zgData.Ansatz, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Basis", ReplaceMissing(zgData.Basis, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Currency", ReplaceMissing(zgData.Currency, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@LP", ReplaceMissing(zgData.LP, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@JAHR", ReplaceMissing(zgData.JAHR, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Aus_Dat", ReplaceMissing(zgData.Aus_Dat, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ClearingNr", ReplaceMissing(zgData.ClearingNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Bank", ReplaceMissing(zgData.Bank, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@KontoNr", ReplaceMissing(zgData.KontoNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@BankOrt", ReplaceMissing(zgData.BankOrt, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@DTAAdr1", ReplaceMissing(zgData.DTAAdr1, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@DTAAdr2", ReplaceMissing(zgData.DTAAdr2, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@DTAAdr3", ReplaceMissing(zgData.DTAAdr3, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@DTAAdr4", ReplaceMissing(zgData.DTAAdr4, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@N2Char", ReplaceMissing(zgData.N2Char, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@_1000000", ReplaceMissing(zgData._1000000, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@_100000", ReplaceMissing(zgData._100000, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@_10000", ReplaceMissing(zgData._10000, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@_1000", ReplaceMissing(zgData._1000, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@_100", ReplaceMissing(zgData._100, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@_10", ReplaceMissing(zgData._10, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@_1", ReplaceMissing(zgData._1, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@USName", ReplaceMissing(zgData.USName, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Result", ReplaceMissing(zgData.Result, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@CheckNumber", ReplaceMissing(zgData.CheckNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@GebAbzug", ReplaceMissing(zgData.GebAbzug, 0)))
      listOfParams.Add(New SqlClient.SqlParameter("@CreatedOn", ReplaceMissing(zgData.CreatedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@CreatedFrom", ReplaceMissing(zgData.CreatedFrom, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ChangedOn", ReplaceMissing(zgData.ChangedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ChangedFrom", ReplaceMissing(zgData.ChangedFrom, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@DTA_Dat", ReplaceMissing(zgData.DTA_Dat, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@BnkAU", ReplaceMissing(zgData.BnkAU, 0)))
      listOfParams.Add(New SqlClient.SqlParameter("@DTADate", ReplaceMissing(zgData.DTADate, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@IBANNr", ReplaceMissing(zgData.IBANNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Swift", ReplaceMissing(zgData.Swift, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@BLZ", ReplaceMissing(zgData.BLZ, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Printed_Dat", ReplaceMissing(zgData.Printed_Dat, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@MDNr", ReplaceMissing(zgData.MDNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@IsCreatedWithLO", ReplaceMissing(zgData.IsCreatedWithLO, DBNull.Value)))

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      Return success

    End Function

    ''' <summary>
    ''' Deletes ZG data.
    ''' </summary>
    ''' <param name="id">The id.</param>
    ''' <param name="modul">The modul name the deletion is performed in.</param>
    ''' <param name="username">The username of the person which deletes the record.</param>
    ''' <param name="usnr">The USNr number.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function DeleteZGData(ByVal id As Integer, ByVal modul As String, ByVal username As String, ByVal usnr As Integer) As DeleteZGResult Implements IAdvancePaymentDatabaseAccess.DeleteZGData

      Dim success = True

      Dim sql As String = "[Delete ZG]"

      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("id", id))
      listOfParams.Add(New SqlClient.SqlParameter("modul", modul))
      listOfParams.Add(New SqlClient.SqlParameter("username", username))
      listOfParams.Add(New SqlClient.SqlParameter("usnr", usnr))

      Dim resultParameter = New SqlClient.SqlParameter("@result", SqlDbType.Int)
      resultParameter.Direction = ParameterDirection.Output
      listOfParams.Add(resultParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      Dim resultEnum As DeleteZGResult

      If Not resultParameter.Value Is Nothing Then
        Try
          resultEnum = CType(resultParameter.Value, DeleteZGResult)
        Catch
          resultEnum = DeleteZGResult.ResultDeleteError
        End Try
      Else
        resultEnum = DeleteZGResult.ResultDeleteError
      End If

      Return resultEnum

    End Function

#End Region

  End Class

End Namespace
