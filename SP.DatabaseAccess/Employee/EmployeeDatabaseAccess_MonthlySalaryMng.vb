Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects.MonthlySalary

Namespace Employee

  Partial Public Class EmployeeDatabaseAccess
    Inherits DatabaseAccessBase
    Implements IEmployeeDatabaseAccess

    ''' <summary>
    ''' Loads employee overview list for monthly salary management.
    ''' </summary>
    Public Function LoadEmployeeOverviewListForMonthlySalaryMng() As IEnumerable(Of EmployeeOverviewData) Implements IEmployeeDatabaseAccess.LoadEmployeeOverviewListForMonthlySalaryMng

      Dim result As List(Of EmployeeOverviewData) = Nothing

      Dim sql As String

			sql = "SELECT MANr, (IsNull(Nachname, '') + ', ' + IsNull(Vorname, '')) as Name, MA_Kanton, S_Kanton FROM Mitarbeiter ORDER BY Name ASC"

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of EmployeeOverviewData)

          While reader.Read()
            Dim overviewData As New EmployeeOverviewData
            overviewData.EmployeeNumber = SafeGetInteger(reader, "MANr", 0)
            overviewData.Name = SafeGetString(reader, "Name")
            overviewData.Canton = SafeGetString(reader, "MA_Kanton")
						overviewData.S_Canton = SafeGetString(reader, "S_Kanton")

            result.Add(overviewData)

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
    '''  Loads monthly salary overview list for monthly salary management.
    ''' </summary>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <param name="onlyCurrentYear">The boolean value indiciating if only current year data should be loaded..</param>
    ''' <returns>List of monthly overview data.</returns>
    Public Function LoadMonthlySalaryOverviewListForMonthlySalaryMng(ByVal employeeNumber As Integer, ByVal mdnumber As Integer, Optional ByVal onlyCurrentYear As Boolean = False) As IEnumerable(Of MonthlySalaryOverviewData) Implements IEmployeeDatabaseAccess.LoadMonthlySalaryOverviewListForMonthlySalaryMng

      Dim result As List(Of MonthlySalaryOverviewData) = Nothing

      Dim sql As String

			sql = "SELECT LM.MDNr, LM.LMNr, LM.ESNr, LM.LANR, LM.LAName "
			sql &= ", LM.LP_VON, LM.[Jahr Von], LM.LP_BIS, LM.[Jahr Bis] "
			sql &= ", LM.M_ANZ, LM.M_ANS, LM.M_BAS, LM.M_BTR, LM.LAIndBez "
			sql &= ", LM.Kanton, LM.ZGGrund, LM.BnkNr "
			sql &= ", LM.CreatedFrom, LM.CreatedOn, LM.ChangedFrom, LM.ChangedON, LA.Vorzeichen "
			sql &= ", (SELECT COUNT(*) FROM LOL WHERE DestLMNr = LM.LMNr) As NumberOfExistingLOL "
			sql &= ", (SELECT COUNT(*) FROM LM_Doc WHERE LMNr = LM.LMNR) As NumberOfExistingLMDocRecords "
			sql &= "FROM LM "
			sql &= "LEFT JOIN LA ON LM.LANr = LA.LANr "
			sql &= "And LM.[Jahr Von] = LA.LAJahr "
			sql &= "WHERE LA.LADeactivated = 0 "
			sql &= "And LM.MANr = @maNumber "

			If mdnumber > 0 Then sql &= "And LM.MDNr = @mdnr "

			If onlyCurrentYear Then
				sql = sql & " And Year(GetDate()) BETWEEN LM.[Jahr Von] And LM.[Jahr Bis] "
			End If

			sql = sql & " ORDER BY LM.[Jahr Bis] DESC, LM.LP_BIS DESC"

			Dim employeeNumberParamater As New SqlClient.SqlParameter("maNumber", employeeNumber)
			Dim mdNumberParamater As New SqlClient.SqlParameter("mdnr", mdnumber)
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(employeeNumberParamater)
			listOfParams.Add(mdNumberParamater)


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of MonthlySalaryOverviewData)

					While reader.Read()
						Dim overviewData As New MonthlySalaryOverviewData

						overviewData.MDNr = SafeGetInteger(reader, "MDNr", Nothing)
						overviewData.LMNr = SafeGetInteger(reader, "LMNr", Nothing)
						overviewData.ESNr = SafeGetInteger(reader, "ESNr", Nothing)
						overviewData.LANr = SafeGetDecimal(reader, "LANR", Nothing)
						overviewData.LAName = SafeGetString(reader, "LAName")
						overviewData.LP_From = SafeGetInteger(reader, "LP_VON", Nothing)
						overviewData.Year_From = SafeGetString(reader, "Jahr Von")
						overviewData.LP_To = SafeGetInteger(reader, "LP_BIS", Nothing)
						overviewData.Year_To = SafeGetString(reader, "Jahr Bis")
						overviewData.M_Anz = SafeGetDecimal(reader, "M_ANZ", Nothing)
						overviewData.M_Ans = SafeGetDecimal(reader, "M_ANS", Nothing)
						overviewData.M_Bas = SafeGetDecimal(reader, "M_BAS", Nothing)
						overviewData.M_BTR = SafeGetDecimal(reader, "M_BTR", Nothing)
						overviewData.LAIndBez = SafeGetString(reader, "LAIndBez")
						overviewData.Canton = SafeGetString(reader, "Kanton")
						overviewData.ZGGrund = SafeGetString(reader, "ZGGrund")
						overviewData.BankNr = SafeGetInteger(reader, "BnkNr", Nothing)
						overviewData.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						overviewData.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						overviewData.ChangedFrom = SafeGetString(reader, "ChangedFrom")
						overviewData.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
						overviewData.Sign = SafeGetString(reader, "Vorzeichen")
						overviewData.NumberOfExistingLOLRecords = SafeGetInteger(reader, "NumberOfExistingLOL", 0)
						overviewData.NumberOfExistingLMDocRecords = SafeGetInteger(reader, "NumberOfExistingLMDocRecords", 0)

						result.Add(overviewData)

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
		''' Loads es list data for monthly salary management.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <returns>List of es data or nothing in error case.</returns>
		Public Function LoadESListForMonthlySalaryMng(ByVal employeeNumber As Integer, ByVal mdnumber As Integer) As IEnumerable(Of ESData) Implements IEmployeeDatabaseAccess.LoadESListForMonthlySalaryMng

			Dim result As List(Of ESData) = Nothing

			Dim sql As String

			sql = "SELECT ES.ESNr, KD.KDNr, KD.Firma1, ES.ES_Ab, ES.ES_Ende, ES.ESKst1, ES.ESKst2, ES.ESKst, ESL.ESLohnNr, ESL.GAVNr, ESL.GAVKanton, ESL.GAVGruppe0, ESL.GAVGruppe1, ESL.GAVGruppe2, ESL.GAVGruppe3, ESL.GAVBezeichnung, "
			sql &= "ES.Einstufung, ES.ESBranche, ES.Suva, "
			sql &= "ESL.GAV_FAG, ESL.GAV_FAN, ESL.GAV_WAG, ESL.GAV_WAN, ESL.GAV_VAG, ESL.GAV_VAN, "
			sql &= "ESL.GAV_FAG_S, ESL.GAV_FAN_S, ESL.GAV_WAG_S, ESL.GAV_WAN_S, ESL.GAV_VAG_S, ESL.GAV_VAN_S, "
			sql &= "ESL.GAV_FAG_M, ESL.GAV_FAN_M, ESL.GAV_WAG_M, ESL.GAV_WAN_M, ESL.GAV_VAG_M, ESL.GAV_VAN_M ,"
			sql &= "ESL.GAV_FAG_J, ESL.GAV_FAN_J, ESL.GAV_WAG_J, ESL.GAV_WAN_J, ESL.GAV_VAG_J, ESL.GAV_VAN_J "
			sql &= "FROM ES "
			sql &= "LEFT JOIN ESLohn ESL On ES.ESNr = ESL.ESNr  "
			sql &= "LEFT JOIN Kunden KD On ES.KDNr = KD.KDNr  "
			sql &= "WHERE ES.MANr = @maNumber And ES.MDNr = @mdnr And ESL.Aktivlodaten = 1 "
			sql &= "Order By ES.ES_Ab Desc, ES.ES_Ende "

			Dim employeeNumberParamater As New SqlClient.SqlParameter("maNumber", employeeNumber)
			Dim mdNumberParamater As New SqlClient.SqlParameter("mdnr", mdnumber)
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(employeeNumberParamater)
			listOfParams.Add(mdNumberParamater)

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ESData)

					While reader.Read()
						Dim esData As New ESData
						esData.ESNr = SafeGetInteger(reader, "ESNr", Nothing)
						esData.Company1 = SafeGetString(reader, "Firma1")
						esData.ES_From = SafeGetDateTime(reader, "ES_Ab", Nothing)
						esData.ES_To = SafeGetDateTime(reader, "ES_Ende", Nothing)

						esData.ESKst1 = SafeGetString(reader, "ESKst1")
						esData.ESKst2 = SafeGetString(reader, "ESKst2")
						esData.ESKst = SafeGetString(reader, "ESKst")
						esData.CustomerNumber = SafeGetInteger(reader, "KDNr", Nothing)
						esData.ESLohnNr = SafeGetInteger(reader, "ESLohnNr", Nothing)

						esData.GAVNr = SafeGetInteger(reader, "GAVNr", Nothing)
						esData.GAVKanton = SafeGetString(reader, "GAVKanton", Nothing)
						esData.GAVGruppe0 = SafeGetString(reader, "GAVGruppe0")
						esData.GAVGruppe1 = SafeGetString(reader, "GAVGruppe1")
						esData.GAVGruppe2 = SafeGetString(reader, "GAVGruppe2")
						esData.GAVGruppe3 = SafeGetString(reader, "GAVGruppe3")
						esData.GAVBezeichnung = SafeGetString(reader, "GAVBezeichnung")
						esData.Einstufung = SafeGetString(reader, "Einstufung")
						esData.ESBranche = SafeGetString(reader, "ESBranche")
						esData.Suva = SafeGetString(reader, "Suva")

						esData.GAV_FAG = SafeGetDecimal(reader, "GAV_FAG", Nothing)
						esData.GAV_FAN = SafeGetDecimal(reader, "GAV_FAN", Nothing)
						esData.GAV_WAG = SafeGetDecimal(reader, "GAV_WAG", Nothing)
						esData.GAV_WAN = SafeGetDecimal(reader, "GAV_WAN", Nothing)
						esData.GAV_VAG = SafeGetDecimal(reader, "GAV_VAG", Nothing)
						esData.GAV_VAN = SafeGetDecimal(reader, "GAV_VAN", Nothing)

						esData.GAV_FAG_S = SafeGetDecimal(reader, "GAV_FAG_S", Nothing)
						esData.GAV_FAN_S = SafeGetDecimal(reader, "GAV_FAN_S", Nothing)
						esData.GAV_WAG_S = SafeGetDecimal(reader, "GAV_WAG_S", Nothing)
						esData.GAV_WAN_S = SafeGetDecimal(reader, "GAV_WAN_S", Nothing)
						esData.GAV_VAG_S = SafeGetDecimal(reader, "GAV_VAG_S", Nothing)
						esData.GAV_VAN_S = SafeGetDecimal(reader, "GAV_VAN_S", Nothing)

						esData.GAV_FAG_M = SafeGetDecimal(reader, "GAV_FAG_M", Nothing)
						esData.GAV_FAN_M = SafeGetDecimal(reader, "GAV_FAN_M", Nothing)
						esData.GAV_WAG_M = SafeGetDecimal(reader, "GAV_WAG_M", Nothing)
						esData.GAV_WAN_M = SafeGetDecimal(reader, "GAV_WAN_M", Nothing)
						esData.GAV_VAG_M = SafeGetDecimal(reader, "GAV_VAG_M", Nothing)
						esData.GAV_VAN_M = SafeGetDecimal(reader, "GAV_VAN_M", Nothing)

						esData.GAV_FAG_J = SafeGetDecimal(reader, "GAV_FAG_J", Nothing)
						esData.GAV_FAN_J = SafeGetDecimal(reader, "GAV_FAN_J", Nothing)
						esData.GAV_WAG_J = SafeGetDecimal(reader, "GAV_WAG_J", Nothing)
						esData.GAV_WAN_J = SafeGetDecimal(reader, "GAV_WAN_J", Nothing)
						esData.GAV_VAG_J = SafeGetDecimal(reader, "GAV_VAG_J", Nothing)
						esData.GAV_VAN_J = SafeGetDecimal(reader, "GAV_VAN_J", Nothing)

						result.Add(esData)

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
		''' Loads la list data for monthly salary management.
		''' </summary>
		''' <param name="year">The year.</param>
		''' <returns>List of la data or nothing in error case.</returns>
		Public Function LoadLAListForMonthlySalaryMng(ByVal year As Integer) As IEnumerable(Of LAData) Implements IEmployeeDatabaseAccess.LoadLAListForMonthlySalaryMng

			Dim result As List(Of LAData) = Nothing

			Dim sql As String

			sql = "SELECT LANr, LALoText, Vorzeichen, AllowedMore_Anz, AllowedMore_Bas, AllowedMore_Ans, AllowedMore_Btr, Rundung, TypeAnzahl, TypeBasis, TypeAnsatz "
			sql &= "From LA WHERE LAJahr = @year And Verwendung IN ('2', '3') And LADeactivated = 0 ORDER BY LANr "

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("year", year))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of LAData)

          While reader.Read()
            Dim laData As New LAData
            laData.LANr = SafeGetDecimal(reader, "LANr", Nothing)
            laData.LALoText = SafeGetString(reader, "LALoText")
            laData.Sign = SafeGetString(reader, "Vorzeichen")
            laData.AllowMoreAnzahl = SafeGetBoolean(reader, "AllowedMore_Anz", False)
            laData.AllowMoreBasis = SafeGetBoolean(reader, "AllowedMore_Bas", False)
            laData.AllowMoreAnsatz = SafeGetBoolean(reader, "AllowedMore_Ans", False)
            laData.AllowMoreBetrag = SafeGetBoolean(reader, "AllowedMore_Btr", False)
            laData.Rounding = SafeGetShort(reader, "Rundung", Nothing)
            laData.TypeAnzahl = SafeGetShort(reader, "TypeAnzahl", Nothing)
            laData.TypeBasis = SafeGetShort(reader, "TypeBasis", Nothing)
            laData.TypeAnsatz = SafeGetShort(reader, "TypeAnsatz", Nothing)

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
		''' Loads canton list data for monthly salary management.
		''' </summary>
		''' <returns>List of canton data or nothing in error case.</returns>
		Public Function LoadCantonListForMonthlySalaryMng(ByVal emplyeeNumber As Integer, ByVal mandantenNummer As Integer) As IEnumerable(Of CantonData) Implements IEmployeeDatabaseAccess.LoadCantonListForMonthlySalaryMng

      Dim result As List(Of CantonData) = Nothing

      Dim sql As String

			sql = "BEGIN TRY DROP TABLE #Canton END TRY BEGIN CATCH END CATCH; "
			sql &= "SELECT Fak_Kanton AS Canton INTO #Canton FROM MD_KiAU GROUP BY fak_Kanton ORDER BY fak_Kanton "
			sql &= "INSERT INTO #Canton (Canton) (SELECT S_Kanton FROM Mitarbeiter WHERE MANr = @MANr) "
			sql &= "INSERT INTO #Canton (Canton) (SELECT ISNULL(MA_Kanton, (SELECT TOP 1 MD_Kanton FROM dbo.Mandanten WHERE MDNr = @MDNr ORDER BY Jahr Desc)) AS MA_Kanton FROM Mitarbeiter WHERE MANr = @MANr) "
			sql &= "INSERT INTO #Canton (Canton) (SELECT LO.S_Kanton AS S_LOKanton FROM LO WHERE LO.MDNr = @MDNr And LO.MANr = @MANr And ISNULL(LO.S_Kanton, '') <> '' GROUP BY LO.S_Kanton ) "
			sql &= "SELECT Canton FROM #Canton WHERE (Canton <> '' AND Canton IS NOT NULL) GROUP BY Canton ORDER BY Canton "

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MANr", emplyeeNumber))
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", mandantenNummer))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CantonData)

					While reader.Read()
						Dim laData As New CantonData
						laData.Canton = SafeGetString(reader, "Canton")

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
    ''' Loads employee bank list data for monthly salary management.
    ''' </summary>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <returns>List of bank data or nothing in error case.</returns>
    Public Function LoadEmployeeBankListForMonthlySalaryMng(ByVal employeeNumber As Integer) As IEnumerable(Of BankData) Implements IEmployeeDatabaseAccess.LoadEmployeeBankListForMonthlySalaryMng

      Dim result As List(Of BankData) = Nothing

      Dim sql As String

      sql = "[Get Kreditor Bakdata 4 Selected Employee]"

      Dim employeeNumberParamater As New SqlClient.SqlParameter("maNumber", employeeNumber)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(employeeNumberParamater)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of BankData)

          While reader.Read()
            Dim laData As New BankData
            laData.RecNr = SafeGetShort(reader, "RecNr", Nothing)
            laData.BankName = SafeGetString(reader, "Bank")
            laData.BankLocation = SafeGetString(reader, "BankOrt")
            laData.BankAccountNumber = SafeGetString(reader, "KontoNr")

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
    ''' Loads the employee LO setting data.
    ''' </summary>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <returns>Employee LO setting or nothing in error case.</returns>
    Public Function LoadEmployeeLOSettingForMonthlySalaryMng(ByVal employeeNumber As Integer) As EmployeeLOSettingData Implements IEmployeeDatabaseAccess.LoadEmployeeLOSettingForMonthlySalaryMng
      Dim employeeLOSettting As EmployeeLOSettingData = Nothing

      Dim sql As String

      sql = "SELECT Currency FROM MA_LOSetting WHERE MANr = @maNr "
     

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("maNr", employeeNumber))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If Not reader Is Nothing Then

          If reader.Read Then
            employeeLOSettting = New EmployeeLOSettingData
            employeeLOSettting.Currency = SafeGetString(reader, "Currency")

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
    ''' Add as an LM record.
    ''' </summary>
    ''' <param name="lmData">The lm data.</param>
    ''' <returns>LMNr of new record or nothing in error case.</returns>
    Public Function AddLM(ByVal lmData As LMData) As Integer? Implements IEmployeeDatabaseAccess.AddLM

      Dim success = True

      Dim sql As String

      sql = "[Create New LM]"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)

      listOfParams.Add(New SqlClient.SqlParameter("MANr", ReplaceMissing(lmData.EmployeeNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("ESNr", ReplaceMissing(lmData.ESNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("Kst1", ReplaceMissing(lmData.LMKst1, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("Kst2", ReplaceMissing(lmData.LMKst2, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("Kst3", ReplaceMissing(lmData.Kst, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("LANr", ReplaceMissing(lmData.LANR, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("LAVon", ReplaceMissing(lmData.LP_VON, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("LABis", ReplaceMissing(lmData.LP_BIS, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("JahrVon", ReplaceMissing(lmData.JahrVon, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("JahrBis", ReplaceMissing(lmData.JahrBis, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("SUVA", ReplaceMissing(lmData.SUVA, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("Anzahl", ReplaceMissing(lmData.M_ANZ, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("Basis", ReplaceMissing(lmData.M_BAS, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("Ansatz", ReplaceMissing(lmData.M_ANS, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("betrag", ReplaceMissing(lmData.M_BTR, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("LABezeichnung", ReplaceMissing(lmData.LAName, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("LAIndBez", ReplaceMissing(lmData.LAIndBez, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("ForDTA", ReplaceMissing(lmData.LMWithDTA, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("ZGGrund", ReplaceMissing(lmData.ZGGrund, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("BnkNr", ReplaceMissing(lmData.BnkNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("Kanton", ReplaceMissing(lmData.Kanton, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAVNr", ReplaceMissing(lmData.GAVNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAVKanton", ReplaceMissing(lmData.GAVKanton, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAVGruppe0", ReplaceMissing(lmData.GAVGruppe0, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAVGruppe1", ReplaceMissing(lmData.GAVGruppe1, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAVGruppe2", ReplaceMissing(lmData.GAVGruppe2, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAVGruppe3", ReplaceMissing(lmData.GAVGruppe3, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAVBezeichnung", ReplaceMissing(lmData.GAVBezeichnung, DBNull.Value)))

      listOfParams.Add(New SqlClient.SqlParameter("GAV_FAG", ReplaceMissing(lmData.GAV_FAG, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_FAN", ReplaceMissing(lmData.GAV_FAN, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_WAG", ReplaceMissing(lmData.GAV_WAG, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_WAN", ReplaceMissing(lmData.GAV_WAN, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_VAG", ReplaceMissing(lmData.GAV_VAG, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_VAN", ReplaceMissing(lmData.GAV_VAN, DBNull.Value)))

      listOfParams.Add(New SqlClient.SqlParameter("GAV_FAG_S", ReplaceMissing(lmData.GAV_FAG_S, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_FAN_S", ReplaceMissing(lmData.GAV_FAN_S, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_WAG_S", ReplaceMissing(lmData.GAV_WAG_S, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_WAN_S", ReplaceMissing(lmData.GAV_WAN_S, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_VAG_S", ReplaceMissing(lmData.GAV_VAG_S, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_VAN_S", ReplaceMissing(lmData.GAV_VAN_S, DBNull.Value)))

      listOfParams.Add(New SqlClient.SqlParameter("GAV_FAG_M", ReplaceMissing(lmData.GAV_FAG_M, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_FAN_M", ReplaceMissing(lmData.GAV_FAN_M, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_WAG_M", ReplaceMissing(lmData.GAV_WAG_M, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_WAN_M", ReplaceMissing(lmData.GAV_WAN_M, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_VAG_M", ReplaceMissing(lmData.GAV_VAG_M, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_VAN_M", ReplaceMissing(lmData.GAV_VAN_M, DBNull.Value)))

      listOfParams.Add(New SqlClient.SqlParameter("GAV_FAG_J", ReplaceMissing(lmData.GAV_FAG_J, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_FAN_J", ReplaceMissing(lmData.GAV_FAN_J, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_WAG_J", ReplaceMissing(lmData.GAV_WAG_J, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_WAN_J", ReplaceMissing(lmData.GAV_WAN_J, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_VAG_J", ReplaceMissing(lmData.GAV_VAG_J, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_VAN_J", ReplaceMissing(lmData.GAV_VAN_J, DBNull.Value)))

      listOfParams.Add(New SqlClient.SqlParameter("ESEinstufung", ReplaceMissing(lmData.ESEinstufung, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("ESBranche", ReplaceMissing(lmData.ESBranche, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(lmData.KDNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("CreatedUser", ReplaceMissing(lmData.CreatedFrom, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("ESLohnNr", ReplaceMissing(lmData.ESLohnNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("USNr", ReplaceMissing(lmData.USNr, DBNull.Value)))

      Dim newIdParameter = New SqlClient.SqlParameter("@NewLMId", SqlDbType.Int)
      newIdParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newIdParameter)

      Dim lmNr = New SqlClient.SqlParameter("@LMNr ", SqlDbType.Int)
      lmNr.Direction = ParameterDirection.Output
      listOfParams.Add(lmNr)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If Not newIdParameter.Value Is Nothing AndAlso
         Not lmNr.Value Is Nothing Then
        Return lmNr.Value
      Else
        Return Nothing
      End If

    End Function

    ''' <summary>
    ''' Updades LM data.
    ''' </summary>
    ''' <param name="lmData">The lm data.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function UpdateLM(ByVal lmData As LMData) As Boolean Implements IEmployeeDatabaseAccess.UpdateLM

      Dim success = True

      Dim sql As String

      sql = "[Update LM For MLohnMng]"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)

      listOfParams.Add(New SqlClient.SqlParameter("LMNr", ReplaceMissing(lmData.LMNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("MANr", ReplaceMissing(lmData.EmployeeNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("ESNr", ReplaceMissing(lmData.ESNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("Kst1", ReplaceMissing(lmData.LMKst1, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("Kst2", ReplaceMissing(lmData.LMKst2, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("Kst3", ReplaceMissing(lmData.Kst, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("LANr", ReplaceMissing(lmData.LANR, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("LAVon", ReplaceMissing(lmData.LP_VON, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("LABis", ReplaceMissing(lmData.LP_BIS, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("JahrVon", ReplaceMissing(lmData.JahrVon, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("JahrBis", ReplaceMissing(lmData.JahrBis, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("SUVA", ReplaceMissing(lmData.SUVA, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("Anzahl", ReplaceMissing(lmData.M_ANZ, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("Basis", ReplaceMissing(lmData.M_BAS, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("Ansatz", ReplaceMissing(lmData.M_ANS, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("betrag", ReplaceMissing(lmData.M_BTR, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("LABezeichnung", ReplaceMissing(lmData.LAName, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("LAIndBez", ReplaceMissing(lmData.LAIndBez, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("ForDTA", ReplaceMissing(lmData.LMWithDTA, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("ZGGrund", ReplaceMissing(lmData.ZGGrund, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("BnkNr", ReplaceMissing(lmData.BnkNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("Kanton", ReplaceMissing(lmData.Kanton, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAVNr", ReplaceMissing(lmData.GAVNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAVKanton", ReplaceMissing(lmData.GAVKanton, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAVGruppe0", ReplaceMissing(lmData.GAVGruppe0, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAVGruppe1", ReplaceMissing(lmData.GAVGruppe1, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAVGruppe2", ReplaceMissing(lmData.GAVGruppe2, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAVGruppe3", ReplaceMissing(lmData.GAVGruppe3, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAVBezeichnung", ReplaceMissing(lmData.GAVBezeichnung, DBNull.Value)))

      listOfParams.Add(New SqlClient.SqlParameter("GAV_FAG", ReplaceMissing(lmData.GAV_FAG, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_FAN", ReplaceMissing(lmData.GAV_FAN, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_WAG", ReplaceMissing(lmData.GAV_WAG, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_WAN", ReplaceMissing(lmData.GAV_WAN, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_VAG", ReplaceMissing(lmData.GAV_VAG, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_VAN", ReplaceMissing(lmData.GAV_VAN, DBNull.Value)))

      listOfParams.Add(New SqlClient.SqlParameter("GAV_FAG_S", ReplaceMissing(lmData.GAV_FAG_S, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_FAN_S", ReplaceMissing(lmData.GAV_FAN_S, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_WAG_S", ReplaceMissing(lmData.GAV_WAG_S, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_WAN_S", ReplaceMissing(lmData.GAV_WAN_S, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_VAG_S", ReplaceMissing(lmData.GAV_VAG_S, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_VAN_S", ReplaceMissing(lmData.GAV_VAN_S, DBNull.Value)))

      listOfParams.Add(New SqlClient.SqlParameter("GAV_FAG_M", ReplaceMissing(lmData.GAV_FAG_M, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_FAN_M", ReplaceMissing(lmData.GAV_FAN_M, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_WAG_M", ReplaceMissing(lmData.GAV_WAG_M, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_WAN_M", ReplaceMissing(lmData.GAV_WAN_M, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_VAG_M", ReplaceMissing(lmData.GAV_VAG_M, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_VAN_M", ReplaceMissing(lmData.GAV_VAN_M, DBNull.Value)))

      listOfParams.Add(New SqlClient.SqlParameter("GAV_FAG_J", ReplaceMissing(lmData.GAV_FAG_J, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_FAN_J", ReplaceMissing(lmData.GAV_FAN_J, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_WAG_J", ReplaceMissing(lmData.GAV_WAG_J, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_WAN_J", ReplaceMissing(lmData.GAV_WAN_J, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_VAG_J", ReplaceMissing(lmData.GAV_VAG_J, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("GAV_VAN_J", ReplaceMissing(lmData.GAV_VAN_J, DBNull.Value)))

      listOfParams.Add(New SqlClient.SqlParameter("ESEinstufung", ReplaceMissing(lmData.ESEinstufung, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("ESBranche", ReplaceMissing(lmData.ESBranche, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(lmData.KDNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("ChangedUser", ReplaceMissing(lmData.ChangedFrom, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("ESLohnNr", ReplaceMissing(lmData.ESLohnNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("USNr", ReplaceMissing(lmData.USNr, DBNull.Value)))

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure)

      Return success

    End Function

    ''' <summary>
    ''' Loads conflicted LOL records. 
    ''' </summary>
    ''' <param name="employeeNumber">The employeenumber.</param>
    ''' <param name="lmNr">The lmNr.</param>
    ''' <param name="firstMonth">The first momth.</param>
    ''' <param name="firstYear">The first year.</param>
    ''' <param name="lastMonth">The last momth.</param>
    ''' <param name="lastYear">The last year.</param>
    ''' <returns>List of conflicted LOL records.</returns>  
		Function LoadConflictedLOLRecordsForMonthlySalaryMng(ByVal employeeNumber As Integer, ByVal esNr As Integer?, ByVal lmNr As Integer?, ByVal firstMonth As Integer, ByVal firstYear As Integer, ByVal lastMonth As Integer, ByVal lastYear As Integer, ByRef resultCode As Integer) As IEnumerable(Of ConflictedLOLData) Implements IEmployeeDatabaseAccess.LoadConflictedLOLRecordsForMonthlySalaryMng

			Dim success = True

			Dim result As List(Of ConflictedLOLData) = Nothing

			Dim sql As String
			sql = "[Get Conflicted LOL With ESNumber]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("MANr", employeeNumber))
			listOfParams.Add(New SqlClient.SqlParameter("ESNr", ReplaceMissing(esNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("LMNr", ReplaceMissing(lmNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("LPVon", firstMonth))
			listOfParams.Add(New SqlClient.SqlParameter("JahrVon", firstYear))
			listOfParams.Add(New SqlClient.SqlParameter("LPBis", lastMonth))
			listOfParams.Add(New SqlClient.SqlParameter("JahrBis", lastYear))

			Dim resultCodeParameter = New SqlClient.SqlParameter("@ResultCode", SqlDbType.Int)
			resultCodeParameter.Direction = ParameterDirection.Output
			listOfParams.Add(resultCodeParameter)


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ConflictedLOLData)

					While reader.Read()
						Dim laData As New ConflictedLOLData
						laData.LONr = SafeGetInteger(reader, "LONr", Nothing)
						laData.LP = SafeGetInteger(reader, "LP", Nothing)
						laData.Year = SafeGetString(reader, "Jahr")

						result.Add(laData)

					End While

					reader.Close()

					If Not resultCodeParameter.Value Is Nothing Then
						resultCode = resultCodeParameter.Value
					Else
						resultCode = -1
					End If

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
    ''' Checks for an existing LM in a period with LANr.
    ''' </summary>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <param name="laNANr">The LAnr.</param>
    ''' <param name="firstMonth">The first month.</param>
    ''' <param name="firstYear">The first year.</param>
    ''' <param name="lastMonth">The last month.</param>
    ''' <param name="lastYear">The last year.</param>
    ''' <param name="result">Boolean value indidicating if a record exits.</param>
    ''' <returns>Boolean flag indicating success of query.</returns>
		Public Function CheckForExistingLMInPeriodWithLANr(ByVal employeeNumber As Integer, ByVal laNANr As Decimal, ByVal firstMonth As Integer, ByVal firstYear As Integer, ByVal lastMonth As Integer, ByVal lastYear As Integer, ByRef result As Boolean) As Boolean Implements IEmployeeDatabaseAccess.CheckForExistingLMInPeriodWithLANr

			Dim success = True

			Dim sql As String

			sql = "SELECT COUNT(*) FROM LM WHERE MANr = @maNr and LANr = @LANr AND (" &
					 "dbo.DateTimeFromYearMonthDay([Jahr Von], LP_Von, 1) BETWEEN  dbo.DateTimeFromYearMonthDay(@JahrVon, @LPVon, 1) AND DATEADD(s, -1, DATEADD(m, DATEDIFF(m, 0, dbo.DateTimeFromYearMonthDay(@JahrBis, @LPBis, 1))+1, 0)) OR " &
					 "dbo.DateTimeFromYearMonthDay([Jahr Bis], LP_Bis, 1) BETWEEN  dbo.DateTimeFromYearMonthDay(@JahrVon, @LPVon, 1) AND DATEADD(s, -1, DATEADD(m, DATEDIFF(m, 0, dbo.DateTimeFromYearMonthDay(@JahrBis, @LPBis, 1))+1, 0)) OR " &
					 "dbo.DateTimeFromYearMonthDay(@JahrVon, @LPVon, 1)   BETWEEN  dbo.DateTimeFromYearMonthDay([Jahr Von],LP_Von, 1) AND DATEADD(s, -1, DATEADD(m, DATEDIFF(m, 0, dbo.DateTimeFromYearMonthDay([Jahr Bis], LP_Bis, 1 ))+1, 0)) OR " &
					 "dbo.DateTimeFromYearMonthDay(@JahrBis, @LPBis, 1)   BETWEEN  dbo.DateTimeFromYearMonthDay([Jahr Von],LP_Von, 1) AND DATEADD(s, -1, DATEADD(m, DATEDIFF(m, 0, dbo.DateTimeFromYearMonthDay([Jahr Bis], LP_Bis, 1))+1, 0)) )"
			' Parameters

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("maNr", employeeNumber))
			listOfParams.Add(New SqlClient.SqlParameter("LANr", laNANr))
			listOfParams.Add(New SqlClient.SqlParameter("JahrVon", firstYear))
			listOfParams.Add(New SqlClient.SqlParameter("LPVon", firstMonth))
			listOfParams.Add(New SqlClient.SqlParameter("JahrBis", lastYear))
			listOfParams.Add(New SqlClient.SqlParameter("LPBis", lastMonth))

			Dim countConflicing = ExecuteScalar(sql, listOfParams)

			If Not countConflicing Is Nothing Then
				result = (countConflicing > 0)
			Else
				success = False
			End If

			Return success

		End Function

		''' <summary>
		''' Deletes an LM record.
		''' </summary>
		''' <param name="lmNr">The LMnr.</param>
		''' <param name="employeeNumber">The employee number (MANr).</param>
		''' <returns>Boolean value indicating success.</returns>
		Public Function DeleteLM(ByVal lmNr As Integer, ByVal employeeNumber As Integer, ByVal username As String, ByVal usnr As Integer) As DeleteEmployeeLMResult Implements IEmployeeDatabaseAccess.DeleteLM

			Dim success = True

			Dim sql As String

			sql = "[Delete Assigned LM Data]"

			'sql = "DELETE FROM LM WHERE LMNr = @lmNr AND MANr = @maNr;"
			'sql &= " DELETE FROM LM_DOC WHERE LMNr = @lmNr; "

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("lmNr", lmNr))
			listOfParams.Add(New SqlClient.SqlParameter("maNr", employeeNumber))
			listOfParams.Add(New SqlClient.SqlParameter("username", username))
			listOfParams.Add(New SqlClient.SqlParameter("usnr", usnr))

			Dim resultParameter = New SqlClient.SqlParameter("@Result", SqlDbType.Int)
			resultParameter.Direction = ParameterDirection.Output
			listOfParams.Add(resultParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Dim resultEnum As DeleteEmployeeLMResult

			If Not resultParameter.Value Is Nothing Then
				Try
					resultEnum = CType(resultParameter.Value, DeleteEmployeeLMResult)
				Catch
					resultEnum = DeleteEmployeeLMResult.ErrorWhileDelete
				End Try
			Else
				resultEnum = DeleteEmployeeLMResult.ErrorWhileDelete
			End If


			Return resultEnum

		End Function

		''' <summary>
		''' Loads the documents for an LM data record.
		''' </summary>
		''' <param name="lmNr">The lm number.</param>
		''' <param name="recordNumber">The record number if only one record should be loaded.</param>
		''' <param name="includeFileBytes">Boolen flag indicating if the file bytes should also be loaded.</param>
		''' <returns>List of documents or nothing in error case.</returns>
		Function LoadLMDocListForLM(ByVal lmNr As Integer, Optional ByVal recordNumber As Integer? = Nothing, Optional includeFileBytes As Boolean = False) As IEnumerable(Of LMDocData) Implements IEmployeeDatabaseAccess.LoadLMDocListForLM

      Dim result As List(Of LMDocData) = Nothing

      Dim sql As String = String.Empty

      sql = "SELECT ID, RecNr, LMNr, DocDescription, CreatedOn, CreatedFrom "

      If includeFileBytes Then
        sql = sql & ", DocScan "
      End If

      sql = sql & " FROM LM_Doc WHERE LMNr = @lmNr AND (@recordNumber IS NULL OR RecNr = @recordNumber) "
      sql = sql & " ORDER BY CreatedOn Desc, DocDescription"

      ' Parameters

      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("lmNr", lmNr))
      listOfParams.Add(New SqlClient.SqlParameter("recordNumber", ReplaceMissing(recordNumber, DBNull.Value)))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of LMDocData)

          While reader.Read()
            Dim documentData As New LMDocData
            documentData.ID = SafeGetInteger(reader, "ID", 0)
            documentData.RecordNumber = SafeGetInteger(reader, "RecNr", 0)
            documentData.LMNr = SafeGetInteger(reader, "LMNr", 0)
            documentData.DocDescription = SafeGetString(reader, "DocDescription")

            If (includeFileBytes) Then
              documentData.DocScan = SafeGetByteArray(reader, "DocScan")
            End If

            documentData.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
            documentData.CreatedFrom = SafeGetString(reader, "CreatedFrom", Nothing)

            result.Add(documentData)

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
    ''' Add as a LM document.
    ''' </summary>
    ''' <param name="lmDoc">The lm dcoument.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function AddLMDoc(ByVal lmDoc As LMDocData) As Boolean Implements IEmployeeDatabaseAccess.AddLMDoc

      Dim success = True

      Dim sql As String

      sql = "[Create New LM_Doc]"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("LMNr", ReplaceMissing(lmDoc.LMNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("DocDescription", ReplaceMissing(lmDoc.DocDescription, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("DocScan", ReplaceMissing(lmDoc.DocScan, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("CreatedOn", ReplaceMissing(lmDoc.CreatedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("CreatedFrom", ReplaceMissing(lmDoc.CreatedFrom, DBNull.Value)))


      Dim newIdParameter = New SqlClient.SqlParameter("@NewDocId", SqlDbType.Int)
      newIdParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newIdParameter)

      Dim recordNumber = New SqlClient.SqlParameter("@RecNr ", SqlDbType.Int)
      recordNumber.Direction = ParameterDirection.Output
      listOfParams.Add(recordNumber)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If Not newIdParameter.Value Is Nothing AndAlso
         Not recordNumber.Value Is Nothing Then
        lmDoc.ID = newIdParameter.Value
        lmDoc.RecordNumber = recordNumber.Value
      Else
        success = False
      End If

      Return success

    End Function

    ''' <summary>
    ''' Upadates an LM document.
    ''' </summary>
    ''' <param name="lmDoc">The LM document object.</param>
    ''' <param name="updateDocScan">Boolean flag indicating if the doc scan bytes should also be updated.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function UdateLMDoc(ByVal lmDoc As LMDocData, Optional updateDocScan As Boolean = False) As Boolean Implements IEmployeeDatabaseAccess.UdateLMDoc

      Dim success = True

      Dim sql As String

      sql = "UPDATE LM_Doc SET "
      sql = sql & "LMNr = @lmNr, "
      sql = sql & "DocDescription = @docDescription, "

      If (updateDocScan) Then
        sql = sql & "DoscScan = @docScan, "
      End If

      sql = sql & "CreatedOn = @createdOn, "
      sql = sql & "CreatedFrom = @createdFrom, "
      sql = sql & "RecNr = @recNr "
      sql = sql & "WHERE ID = @id "

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("lmNr", ReplaceMissing(lmDoc.LMNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("docDescription", ReplaceMissing(lmDoc.DocDescription, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("createdOn", ReplaceMissing(lmDoc.CreatedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("createdFrom", ReplaceMissing(lmDoc.CreatedFrom, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("recNr", ReplaceMissing(lmDoc.RecordNumber, DBNull.Value)))

      If (updateDocScan) Then
        listOfParams.Add(New SqlClient.SqlParameter("docScan", ReplaceMissing(lmDoc.DocScan, DBNull.Value)))
      End If

      listOfParams.Add(New SqlClient.SqlParameter("id", lmDoc.ID))

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function

    ''' <summary>
    ''' Deletes an LM document.
    ''' </summary>
    ''' <param name="id">The id of the LM document</param>
    ''' <returns>Boolean value indicating success.</returns>
    Public Function DeleteLMDoc(ByVal id As Integer) As Boolean Implements IEmployeeDatabaseAccess.DeleteLMDoc

      Dim success = True

      Dim sql As String

      sql = "DELETE FROM LM_Doc WHERE ID = @id"

      Dim listOfParams As New List(Of SqlClient.SqlParameter)

      listOfParams.Add(New SqlClient.SqlParameter("id", id))

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function

  End Class

End Namespace