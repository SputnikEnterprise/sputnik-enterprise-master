Imports SP.DatabaseAccess.SalaryValueCalculation.DataObjects

Namespace SalaryValueCalculation

  ''' <summary>
  ''' Salary value calculation database access.
  ''' </summary>
  Public Class SalaryValueCalculationDatabaseAccess
    Inherits DatabaseAccessBase
    Implements ISalaryValueCalculationDatabaseAccess

#Region "Constructor"

    ''' <summary>
    ''' Constructor.
    ''' </summary>
    ''' <param name="connectionString">The connection string.</param>
    ''' <param name="translationLanguage">The translation language.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal connectionString As String, ByVal translationLanguage As Language)
      MyBase.New(connectionString, translationLanguage)

    End Sub

    ''' <summary>
    ''' Constructor.
    ''' </summary>
    ''' <param name="connectionString">The connection string.</param>
    ''' <param name="translationLanguage">The translation language.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal connectionString As String, ByVal translationLanguage As String)
      MyBase.new(connectionString, translationLanguage)
    End Sub

#End Region

    ''' <summary>
    ''' Loads the employee data.
    ''' </summary>
    ''' <param name="maNr">The employee number.</param>
    ''' <returns>Employee data or nothing.</returns>
    Function LoadEmployeeData(ByVal maNr As Integer) As EmployeeData Implements ISalaryValueCalculationDatabaseAccess.LoadEmployeeData

      Dim employeeData As EmployeeData = Nothing

      Dim sql As String

      sql = "SELECT MANr, Zivilstand, MA_Kanton, Geschlecht, GebDat FROM Mitarbeiter WHERE MANr = @maNr"

      ' Parameters
      Dim employeeNumberParameter As New SqlClient.SqlParameter("maNr", maNr)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(employeeNumberParameter)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If Not reader Is Nothing Then

          If reader.Read Then
            employeeData = New EmployeeData
            employeeData.EmployeeNumber = SafeGetInteger(reader, "MANr", Nothing)
            employeeData.CivilStatus = SafeGetString(reader, "Zivilstand")
            employeeData.Canton = SafeGetString(reader, "MA_Kanton")
            employeeData.Gender = SafeGetString(reader, "Geschlecht")
            employeeData.Birthdate = SafeGetDateTime(reader, "GebDat", Nothing)

          End If

        End If

      Catch ex As Exception
        m_Logger.LogError(ex.ToString())
        employeeData = Nothing
      Finally
        CloseReader(reader)
      End Try

      Return employeeData

    End Function

    ''' <summary>
    ''' Loads LA data.
    ''' </summary>
    ''' <param name="laNr">The la number.</param>
    ''' <param name="laYear">The la year.</param>
    ''' <returns>LA data or nothing in error case.</returns>
    Public Function LoadLAData(ByVal laNr As Decimal, ByVal laYear As Integer) As LAData Implements ISalaryValueCalculationDatabaseAccess.LoadLAData

      Dim laData As LAData = Nothing

      Dim sql As String

			sql = "SELECT LANr, MABasVar, MAAnsVar, TypeBasis, TypeAnzahl, TypeAnsatz, FixBasis, FixAnzahl, FixAnsatz, FixAnsatz, SeeKanton FROM LA WHERE LANr = @laNr AND LAJahr = @laYear And LADeactivated = 0"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("laNr", laNr))
      listOfParams.Add(New SqlClient.SqlParameter("laYear", laYear))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If Not reader Is Nothing Then

          If reader.Read Then
            laData = New LAData
            laData.LANR = SafeGetDecimal(reader, "LANr", Nothing)
            laData.MABasVar = SafeGetString(reader, "MABasVar")
            laData.MAAnsVar = SafeGetString(reader, "MAAnsVar")
            laData.TypeBasis = SafeGetShort(reader, "TypeBasis", Nothing)
            laData.TypeAnzahl = SafeGetShort(reader, "TypeAnzahl", Nothing)
            laData.TypeAnsatz = SafeGetShort(reader, "TypeAnsatz", Nothing)
            laData.MABasVar = SafeGetString(reader, "MABasVar")
            laData.FixBasis = SafeGetDecimal(reader, "FixBasis", Nothing)
            laData.FixAnzahl = SafeGetDecimal(reader, "FixAnzahl", Nothing)
            laData.FixAnsatz = SafeGetDecimal(reader, "FixAnsatz", Nothing)
            laData.SeeKanton = SafeGetBoolean(reader, "SeeKanton", False)
          End If

        End If

      Catch ex As Exception
        m_Logger.LogError(ex.ToString())
        laData = Nothing
      Finally
        CloseReader(reader)
      End Try

      Return laData

    End Function

    ''' <summary>
    ''' Loads fix basis from MK_KiAu.
    ''' </summary>
    ''' <param name="maBasVar">The maBasVar.</param>
    ''' <param name="fakCanton">The canton.</param>
    ''' <param name="year">The year.</param>
    ''' <returns>The fix basis value or nothing in error case.</returns>
    Function LoadFixBasisFromMDKiAu(ByVal maBasVar As String, ByVal fakCanton As String, ByVal year As Integer) As MDKiAuData Implements ISalaryValueCalculationDatabaseAccess.LoadFixBasisFromMDKiAu

      Dim mdKiAuData As MDKiAuData = Nothing

      If String.IsNullOrEmpty(maBasVar) Then
        Return Nothing
      End If

      maBasVar = maBasVar.ToLower()

      Dim sql As String

      sql = "SELECT TOP 1 * FROM MD_KiAu WHERE Fak_Kanton = @canton AND MDYear = @year"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("canton", ReplaceMissing(fakCanton, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("year", year))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If Not reader Is Nothing Then

          If reader.Read Then

            For i As Integer = 0 To reader.FieldCount - 1

              Dim name As String = reader.GetName(i).ToLower()
              If name = maBasVar Then

                mdKiAuData = New MDKiAuData
                mdKiAuData.FixBasis = SafeGetDecimal(reader, maBasVar, Nothing)

                Exit For

              End If
            Next

          End If
        End If
      Catch ex As Exception
        m_Logger.LogError(ex.ToString())
        mdKiAuData = Nothing
      Finally
        CloseReader(reader)
      End Try

      Return mdKiAuData
    End Function


		Function LoadFeierTagGuthaben(ByVal mdNr As Integer, ByVal employeeNumber As Integer, ByVal esNumber As Integer) As Decimal Implements ISalaryValueCalculationDatabaseAccess.LoadFeierTagGuthaben
			Dim dRueckstellung As Decimal = 0
			Dim dBezahlt As Decimal = 0
			Dim result As Decimal = 0

			Dim sql As String

			sql = "[Get Feiertag Guthaben With Netto 1 And 2]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MANr", ReplaceMissing(employeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ESNr", ReplaceMissing(esNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNr, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then
					dRueckstellung = SafeGetDecimal(reader, "BackedBetrag", 0)
					dBezahlt = SafeGetDecimal(reader, "PayedBetrag", 0)

					' Rückstellung - Auszahlung = Das Guthaben von Ferien
					result = (-1 * dRueckstellung) - dBezahlt

				End If


			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				result = Nothing

			Finally
				CloseReader(reader)
			End Try


			Return result

		End Function

		Function LoadFerienGuthaben(ByVal mdNr As Integer, ByVal employeeNumber As Integer, ByVal esNumber As Integer) As Decimal Implements ISalaryValueCalculationDatabaseAccess.LoadFerienGuthaben
			Dim dRueckstellung As Decimal = 0
			Dim dBezahlt As Decimal = 0
			Dim result As Decimal = 0

			Dim sql As String

			sql = "[Get Ferientag Guthaben With Netto 1 And 2]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MANr", ReplaceMissing(employeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ESNr", ReplaceMissing(esNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNr, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then
					dRueckstellung = SafeGetDecimal(reader, "BackedBetrag", 0)
					dBezahlt = SafeGetDecimal(reader, "PayedBetrag", 0)

					' Rückstellung - Auszahlung = Das Guthaben von Ferien
					result = (-1 * dRueckstellung) - dBezahlt

				End If


			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				result = Nothing

			Finally
				CloseReader(reader)
			End Try


			Return result

		End Function

		Function Load13LohnGuthaben(ByVal mdNr As Integer, ByVal employeeNumber As Integer, ByVal esNumber As Integer) As Decimal Implements ISalaryValueCalculationDatabaseAccess.Load13LohnGuthaben
			Dim dRueckstellung As Decimal = 0
			Dim dBezahlt As Decimal = 0
			Dim result As Decimal = 0

			Dim sql As String

			sql = "[Get 13Lohn Guthaben With Netto 1 And 2]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MANr", ReplaceMissing(employeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ESNr", ReplaceMissing(esNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNr, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then
					dRueckstellung = SafeGetDecimal(reader, "BackedBetrag", 0)
					dBezahlt = SafeGetDecimal(reader, "PayedBetrag", 0)

					' Rückstellung - Auszahlung = Das Guthaben von Ferien
					result = (-1 * dRueckstellung) - dBezahlt

				End If


			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				result = Nothing

			Finally
				CloseReader(reader)
			End Try


			Return result

		End Function

		Function LoadDarlehenGuthaben(ByVal mdNr As Integer, ByVal employeeNumber As Integer) As Decimal Implements ISalaryValueCalculationDatabaseAccess.LoadDarlehenGuthaben
			Dim dRueckstellung As Decimal = 0
			Dim dBezahlt As Decimal = 0
			Dim result As Decimal = 0

			Dim sql As String

			sql = "[Get Darlehen Guthaben]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MANr", ReplaceMissing(employeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNr, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then
					dRueckstellung = SafeGetDecimal(reader, "BackedBetrag", 0)
					dBezahlt = SafeGetDecimal(reader, "PayedBetrag", 0)

					' Rückstellung - Auszahlung = Das Guthaben von Ferien
					result = dBezahlt + dRueckstellung

				End If


			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				result = Nothing

			Finally
				CloseReader(reader)
			End Try


			Return result

		End Function



		Function LoadAmountOfNightInReport(ByVal mdNr As Integer, ByVal employeeNumber As Integer, ByVal esNumber As Integer) As NightHourData Implements ISalaryValueCalculationDatabaseAccess.LoadAmountOfNightInReport
			Dim result As NightHourData = Nothing

			'Dim dRueckstellungStd As Decimal = 0
			'Dim dRueckstellungBetrag As Decimal = 0
			'Dim dBezahltStd As Decimal = 0
			'Dim dBezahltBetrag As Decimal = 0

			'Dim dRestStd As Decimal = 0
			'Dim dRestBetrag As Decimal = 0

			'Dim dBasis As Decimal = 0



			'Dim dRueckstellung As Decimal = 0
			'Dim dBezahlt As Decimal = 0

			Dim sql As String

			sql = "[Get NightStd For Calculating Amounts in Report with ES Number]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MANr", ReplaceMissing(employeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ESNr", ReplaceMissing(esNumber, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				result = New NightHourData

				If (Not reader Is Nothing AndAlso reader.Read()) Then
					result.BackedAmount = SafeGetDecimal(reader, "BackedBetrag", 0)
					result.BackedHours = SafeGetDecimal(reader, "BackedStd", 0)

					result.PayedAmount = SafeGetDecimal(reader, "PayedBetrag", 0)
					result.PayedHours = SafeGetDecimal(reader, "PayedStd", 0)

					If result.BackedAmount = 0 OrElse result.BackedHours = 0 Then result = (New NightHourData With {.BackedHours = 0, .PayedHours = 0})

				End If


			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				result = Nothing

			Finally
				CloseReader(reader)
			End Try


			Return result

		End Function


	End Class

End Namespace