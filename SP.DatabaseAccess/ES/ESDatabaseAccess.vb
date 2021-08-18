Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.ES.DataObjects.ESMng
Imports System.Text

Namespace ES

  Partial Public Class ESDatabaseAccess
    Inherits DatabaseAccessBase
    Implements IESDatabaseAccess

#Region "Constructor"

    ''' <summary>
    ''' Constructor.
    ''' </summary>
    ''' <param name="connectionString">The connection string.</param>
    Public Sub New(ByVal connectionString As String, ByVal translationLanguage As Language)
      MyBase.New(connectionString, translationLanguage)

    End Sub

    ''' <summary>
    ''' Constructor.
    ''' </summary>
    ''' <param name="connectionString">The connection string.</param>
    ''' <param name="translationLanguage">The translation language.</param>
    Public Sub New(ByVal connectionString As String, ByVal translationLanguage As String)
      MyBase.new(connectionString, translationLanguage)
    End Sub

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Loads employee data.
    ''' </summary>
    ''' <returns>List of employee data.</returns>
    Public Function LoadEmployeeData() As IEnumerable(Of EmployeeData) Implements IESDatabaseAccess.LoadEmployeeData

      Dim result As List(Of EmployeeData) = Nothing

      Dim sql As String

			sql = "SELECT MA.MANr, MA.Nachname, MA.Vorname, MA.PLZ, MA.Ort FROM Mitarbeiter MA Left Join MAKontakt_Komm MAKK On MA.MANr = MAKK.MANr "
      sql &= "Where (MAKK.NoES = 0 Or MAKK.NoES Is Null) AND IsNull(ShowAsApplicant, 0) = 0 ORDER BY MA.Nachname, MA.Vorname"

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
    ''' Lodas customer data.
    ''' </summary>
    ''' <returns>List of customer data.</returns>
		Public Function LoadCustomerData(Optional ByVal usFiliale As String = "") As IEnumerable(Of CustomerData) Implements IESDatabaseAccess.LoadCustomerData

			Dim result As List(Of CustomerData) = Nothing

			Dim sql As String

			sql = "SELECT KDNr, Firma1, Strasse, PLZ, Ort FROM Kunden Where (NoES = 0 Or NoES Is Null) "
			sql &= "AND KDFiliale Like @Filiale "
			sql &= "ORDER BY Firma1 ASC"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("Filiale", "%" & ReplaceMissing(usFiliale, String.Empty) & "%"))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CustomerData)

					While reader.Read

						Dim customerData = New CustomerData()
						customerData.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)
						customerData.Company1 = SafeGetString(reader, "Firma1")
						customerData.Street = SafeGetString(reader, "Strasse")
						customerData.Postcode = SafeGetString(reader, "PLZ")
						customerData.Location = SafeGetString(reader, "Ort")

						result.Add(customerData)

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
    ''' Loads salary calculation percentage values.
    ''' </summary>
    ''' <param name="age">The age.</param>
    ''' <returns>The calculation percentage values.</returns>
    Function LoadSalaryCalculationPercentageValues(ByVal age As Integer) As SalaryCalculationPercentageValues Implements IESDatabaseAccess.LoadSalaryCalculationPercentageValues

      Dim result As SalaryCalculationPercentageValues = Nothing

      Dim sql As String = String.Empty

      sql = sql & "Select [ID]"
      sql = sql & ",[Jahrgang]"
      sql = sql & ",[FerProzentSatz]"
      sql = sql & ",[FeierProzentSatz]"
      sql = sql & ",[13ProzentSatz]"
      sql = sql & ",[Result]"
      sql = sql & " FROM [TabFerien/Feier/13] WHERE [Jahrgang] = @age"

      ' Parameters
      Dim ageParameter As New SqlClient.SqlParameter("age", age)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(ageParameter)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If Not reader Is Nothing Then

          If reader.Read Then
            result = New SalaryCalculationPercentageValues

            result.ID = SafeGetInteger(reader, "ID", 0)
            result.Jahrgang = SafeGetInteger(reader, "Jahrgang", Nothing)
            result.FerProzentSatz = SafeGetDecimal(reader, "FerProzentSatz", Nothing)
            result.FeierProzenSatz = SafeGetDecimal(reader, "FeierProzentSatz", Nothing)
            result.ProzenzSat13Lohn = SafeGetDecimal(reader, "13ProzentSatz", Nothing)
            result.Result = SafeGetString(reader, "Result")

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
    ''' Loads ES master data.
    ''' </summary>
    ''' <param name="esNumber">The es number.</param>
    ''' <returns>Employee master data or nothing in error case.</returns>
    Function LoadESMasterData(ByVal esNumber As Integer) As ESMasterData Implements IESDatabaseAccess.LoadESMasterData

      Dim esMasterData As ESMasterData = Nothing

      Dim sql As String = String.Empty

      sql = sql & "Select [ID]"
      sql = sql & ",[ESNR]"
      sql = sql & ",[MANR]"
      sql = sql & ",[KDNR]"
      sql = sql & ",[KSTBez]"
      sql = sql & ",[ESKst]"
      sql = sql & ",[Arbzeit]"
      sql = sql & ",[Arbort]"
      sql = sql & ",[Melden]"
      sql = sql & ",[ES_Als]"
      sql = sql & ",[ES_Ab]"
      sql = sql & ",[ES_Uhr]"
      sql = sql & ",[ES_Ende]"
      sql = sql & ",[Ende]"
      sql = sql & ",[GAVText]"
      sql = sql & ",[Bemerk_MA]"
      sql = sql & ",[Bemerk_KD]"
      sql = sql & ",[Bemerk_RE]"
      sql = sql & ",[Bemerk_Lo]"
      sql = sql & ",[Bemerk_P]"

      sql = sql & ",[dismissalon]"
      sql = sql & ",[dismissalfor]"
      sql = sql & ",[dismissalkind]"
      sql = sql & ",[dismissalreason]"
      sql = sql & ",[dismissalwho]"

      sql = sql & ",[RP_Art]"
      sql = sql & ",[LeistungsDoc]"
      sql = sql & ",[MWST]"
      sql = sql & ",[SUVA]"
      sql = sql & ",[Currency]"
      sql = sql & ",[CreatedOn]"
      sql = sql & ",[CreatedFrom]"
      sql = sql & ",[CreatedKST]"
      sql = sql & ",[ChangedOn]"
      sql = sql & ",[ChangedFrom]"
      sql = sql & ",[ChangedKST]"
      sql = sql & ",[Result]"
      sql = sql & ",[KDZustaendig]"
      sql = sql & ",[ESKST1]"
      sql = sql & ",[ESKST2]"
      sql = sql & ",[ESUnterzeichner]"
      sql = sql & ",[VerleihBacked]"
      sql = sql & ",[Bemerk_1]"
      sql = sql & ",[Bemerk_2]"
      sql = sql & ",[Bemerk_3]"
      sql = sql & ",[Print_KD]"
      sql = sql & ",[Print_MA]"
      sql = sql & ",[Far-pflicht]"
      sql = sql & ",[ESVerBacked]"
      sql = sql & ",[NoListing]"
      sql = sql & ",[BVGCode]"
      sql = sql & ",[Einstufung]"
      sql = sql & ",[ESBranche]"
      sql = sql & ",[GoesLonger]"
      sql = sql & ",[ProposeNr]"
      sql = sql & ",[VakNr]"
      sql = sql & ",[PNr]"
      sql = sql & ",[KDZHDNr]"
      sql = sql & ",[MDNr]"
      sql = sql & ",[PrintNoRP]"
      sql = sql & " FROM [ES]"
      sql = sql & " WHERE ESNr = @esNr"

      ' Parameters
      Dim esNumberParameter As New SqlClient.SqlParameter("esNr", esNumber)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(esNumberParameter)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If Not reader Is Nothing Then

          If reader.Read Then
            esMasterData = New ESMasterData

            esMasterData.ID = SafeGetInteger(reader, "ID", 0)
            esMasterData.ESNR = SafeGetInteger(reader, "ESNR", Nothing)
            esMasterData.EmployeeNumber = SafeGetInteger(reader, "MANr", Nothing)
            esMasterData.CustomerNumber = SafeGetInteger(reader, "KDNR", Nothing)
            esMasterData.KSTBez = SafeGetString(reader, "KSTBez")
            esMasterData.ESKst = SafeGetString(reader, "ESKst")
            esMasterData.Arbzeit = SafeGetString(reader, "Arbzeit")
            esMasterData.Arbort = SafeGetString(reader, "Arbort")
            esMasterData.Melden = SafeGetString(reader, "Melden")
            esMasterData.ES_Als = SafeGetString(reader, "ES_Als")
            esMasterData.ES_Ab = SafeGetDateTime(reader, "ES_Ab", Nothing)
            esMasterData.ES_Uhr = SafeGetString(reader, "ES_Uhr")
            esMasterData.ES_Ende = SafeGetDateTime(reader, "Es_Ende", Nothing)
						esMasterData.Ende = SafeGetString(reader, "Ende")
						esMasterData.GAVText = SafeGetString(reader, "GAVText")
            esMasterData.Bemerk_MA = SafeGetString(reader, "Bemerk_MA")
            esMasterData.Bemerk_KD = SafeGetString(reader, "Bemerk_KD")
            esMasterData.Bemerk_RE = SafeGetString(reader, "Bemerk_RE")
            esMasterData.Bemerk_Lo = SafeGetString(reader, "Bemerk_Lo")
            esMasterData.Bemerk_P = SafeGetString(reader, "Bemerk_P")

            esMasterData.dismissalon = SafeGetDateTime(reader, "dismissalon", Nothing)
            esMasterData.dismissalfor = SafeGetDateTime(reader, "dismissalfor", Nothing)

            esMasterData.dismissalkind = SafeGetString(reader, "dismissalkind")
            esMasterData.dismissalreason = SafeGetString(reader, "dismissalreason")
            esMasterData.dismissalwho = SafeGetString(reader, "dismissalwho")

            esMasterData.RP_Art = SafeGetString(reader, "RP_Art")
            esMasterData.LeistungsDoc = SafeGetString(reader, "LeistungsDoc")
            esMasterData.MWST = SafeGetString(reader, "MWST")
            esMasterData.SUVA = SafeGetString(reader, "SUVA")
            esMasterData.Currency = SafeGetString(reader, "Currency")
            esMasterData.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
            esMasterData.CreatedFrom = SafeGetString(reader, "CreatedFrom")
            esMasterData.CreatedKST = SafeGetString(reader, "CreatedKST", )
            esMasterData.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
            esMasterData.ChangedFrom = SafeGetString(reader, "ChangedFrom", Nothing)
            esMasterData.ChangedKST = SafeGetString(reader, "ChangedKST", Nothing)
            esMasterData.Result = SafeGetString(reader, "Result")
            esMasterData.KDZustaendig = SafeGetString(reader, "KDZustaendig")
            esMasterData.ESKST1 = SafeGetString(reader, "ESKST1")
            esMasterData.ESKST2 = SafeGetString(reader, "ESKST2")
            esMasterData.ESUnterzeichner = SafeGetString(reader, "ESUnterzeichner")
            esMasterData.VerleihBacked = SafeGetBoolean(reader, "VerleihBacked", Nothing)
            esMasterData.Bemerk_1 = SafeGetString(reader, "Bemerk_1")
            esMasterData.Bemerk_2 = SafeGetString(reader, "Bemerk_2")
            esMasterData.Bemerk_3 = SafeGetString(reader, "Bemerk_3")
            esMasterData.Print_KD = SafeGetBoolean(reader, "Print_KD", Nothing)
            esMasterData.Print_MA = SafeGetBoolean(reader, "Print_MA", Nothing)
            esMasterData.FarPflichtig = SafeGetBoolean(reader, "Far-pflicht", Nothing)
            esMasterData.ESVerBacked = SafeGetBoolean(reader, "ESVerBacked", Nothing)
            esMasterData.NoListing = SafeGetBoolean(reader, "NoListing", Nothing)
            esMasterData.BVGCode = SafeGetShort(reader, "BVGCode", Nothing)
            esMasterData.Einstufung = SafeGetString(reader, "Einstufung", )
            esMasterData.ESBranche = SafeGetString(reader, "ESBranche")
            esMasterData.GoesLonger = SafeGetString(reader, "GoesLonger")
            esMasterData.ProposeNr = SafeGetInteger(reader, "ProposeNr", Nothing)
            esMasterData.VakNr = SafeGetInteger(reader, "VakNr", Nothing)
            esMasterData.PNr = SafeGetInteger(reader, "PNr", Nothing)
            esMasterData.KDZHDNr = SafeGetInteger(reader, "KDZHDNr", Nothing)
            esMasterData.MDNr = SafeGetInteger(reader, "MDNr", Nothing)
            esMasterData.PrintNoRP = SafeGetBoolean(reader, "PrintNoRP", Nothing)

          End If

        End If

      Catch ex As Exception
        m_Logger.LogError(ex.ToString())
        esMasterData = Nothing
      Finally
        CloseReader(reader)
      End Try

      Return esMasterData

    End Function

    ''' <summary>
    ''' Loads ES salary data.
    ''' </summary>
    ''' <param name="esNumber">The es nr.</param>
    ''' <returns>List of ES salary data.</returns>
    Public Function LoadESSalaryData(ByVal esNumber As Integer) As IEnumerable(Of ESSalaryData) Implements IESDatabaseAccess.LoadESSalaryData

      Dim result As List(Of ESSalaryData) = Nothing

      Dim sql As String = String.Empty

      sql = sql & "Select [ID]"
      sql = sql & ",[ESLohnNr]"
      sql = sql & ",[ESNr]"
      sql = sql & ",[MANr]"
      sql = sql & ",[KDNr]"
      sql = sql & ",[KSTNr]"
      sql = sql & ",[KSTBez]"
      sql = sql & ",[GavText]"
      sql = sql & ",[GrundLohn]"
      sql = sql & ",[StundenLohn]"
      sql = sql & ",[FerBasis]"
      sql = sql & ",[Ferien]"
      sql = sql & ",[FerienProz]"
      sql = sql & ",[Feier]"
      sql = sql & ",[FeierProz]"
      sql = sql & ",[Basis13]"
      sql = sql & ",[Lohn13]"
      sql = sql & ",[Lohn13Proz]"
      sql = sql & ",[Tarif]"
      sql = sql & ",[MAStdSpesen]"
      sql = sql & ",[MATSpesen]"
      sql = sql & ",[KDTSpesen]"
      sql = sql & ",[MATotal]"
      sql = sql & ",[KDTotal]"
      sql = sql & ",[MWSTBetrag]"
      sql = sql & ",[BruttoMarge]"
      sql = sql & ",[LOVon]"
      sql = sql & ",dbo.[GetEndDateOfESLohn](ESNr, ESLohnNr) LOBis"
      sql = sql & ",[Result]"
      sql = sql & ",[AktivLODaten]"
      sql = sql & ",[MargeMitBVG]"
      sql = sql & ",[GAVNr]"
      sql = sql & ",[GAVKanton]"
      sql = sql & ",[GAVGruppe0]"
      sql = sql & ",[GAVGruppe1]"
      sql = sql & ",[GAVGruppe2]"
      sql = sql & ",[GAVGruppe3]"
      sql = sql & ",[GAVBezeichnung]"
      sql = sql & ",[GAV_FAG]"
      sql = sql & ",[GAV_FAN]"
      sql = sql & ",[GAV_WAG]"
      sql = sql & ",[GAV_WAN]"
      sql = sql & ",[GAV_VAG]"
      sql = sql & ",[GAV_VAN]"
      sql = sql & ",[GAV_StdWeek]"
      sql = sql & ",[GAV_StdMonth]"
      sql = sql & ",[GAV_StdYear]"
      sql = sql & ",[GAVStdLohn]"
      sql = sql & ",[GAV_FAG_S]"
      sql = sql & ",[GAV_FAN_S]"
      sql = sql & ",[GAV_WAG_S]"
      sql = sql & ",[GAV_WAN_S]"
      sql = sql & ",[GAV_VAG_S]"
      sql = sql & ",[GAV_VAN_S]"
      sql = sql & ",[GAV_FAG_M]"
      sql = sql & ",[GAV_FAN_M]"
      sql = sql & ",[GAV_WAG_M]"
      sql = sql & ",[GAV_WAN_M]"
      sql = sql & ",[GAV_VAG_M]"
      sql = sql & ",[GAV_VAN_M]"
      sql = sql & ",[FerienWay]"
      sql = sql & ",[LO13Way]"
      sql = sql & ",[GAV_FAG_J]"
      sql = sql & ",[GAV_FAN_J]"
      sql = sql & ",[GAV_WAG_J]"
      sql = sql & ",[GAV_WAN_J]"
      sql = sql & ",[GAV_VAG_J]"
      sql = sql & ",[GAV_VAN_J]"
      sql = sql & ",[CreatedOn]"
      sql = sql & ",[CreatedFrom]"
      sql = sql & ",[ChangedOn]"
      sql = sql & ",[ChangedFrom]"
      sql = sql & ",[VerleihDoc_Guid]"
      sql = sql & ",[ESDoc_Guid]"
      sql = sql & ",[Transfered_User]"
      sql = sql & ",[Transfered_On]"
      sql = sql & ",[IsPVL]"
      sql = sql & ",[FeierBasis]"
      sql = sql & ",[GAVInfo_String]"
      sql = sql & ",[LOFeiertagWay]"
      sql = sql & ",[GAVDate]"
      sql = sql & ",[MargenInfo_String]"
      sql = sql & " FROM [dbo].[ESLohn] WHERE ESNr = @esNr"
      sql = sql & " ORDER BY [LOVon] DESC, [ESLohnNr] DESC"

      ' Parameters
      Dim esNumberParameter As New SqlClient.SqlParameter("esNr", esNumber)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(esNumberParameter)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of ESSalaryData)

          While reader.Read

            Dim esSalaryData As New ESSalaryData

            esSalaryData.ID = SafeGetInteger(reader, "ID", 0)
						esSalaryData.ESLohnNr = SafeGetInteger(reader, "ESLohnNr", Nothing)
            esSalaryData.ESNr = SafeGetInteger(reader, "ESNr", Nothing)
            esSalaryData.EmployeeNumber = SafeGetInteger(reader, "MANr", Nothing)
            esSalaryData.CustomerNumber = SafeGetInteger(reader, "KDNr", Nothing)
						esSalaryData.KSTNr = SafeGetInteger(reader, "KSTNr", Nothing)
            esSalaryData.KSTBez = SafeGetString(reader, "KSTBez")
            esSalaryData.GavText = SafeGetString(reader, "GavText")
            esSalaryData.GrundLohn = SafeGetDecimal(reader, "GrundLohn", Nothing)
            esSalaryData.StundenLohn = SafeGetDecimal(reader, "StundenLohn", Nothing)
            esSalaryData.FerBasis = SafeGetDecimal(reader, "FerBasis", Nothing)
            esSalaryData.Ferien = SafeGetDecimal(reader, "Ferien", Nothing)
            esSalaryData.FerienProz = SafeGetDecimal(reader, "FerienProz", Nothing)
            esSalaryData.Feier = SafeGetDecimal(reader, "Feier", Nothing)
            esSalaryData.FeierProz = SafeGetDecimal(reader, "FeierProz", Nothing)
            esSalaryData.Basis13 = SafeGetDecimal(reader, "Basis13", Nothing)
            esSalaryData.Lohn13 = SafeGetDecimal(reader, "Lohn13", Nothing)
            esSalaryData.Lohn13Proz = SafeGetDecimal(reader, "Lohn13Proz", Nothing)
            esSalaryData.Tarif = SafeGetDecimal(reader, "Tarif", Nothing)
            esSalaryData.MAStdSpesen = SafeGetDecimal(reader, "MAStdSpesen", Nothing)
            esSalaryData.MATSpesen = SafeGetDecimal(reader, "MATSpesen", Nothing)
            esSalaryData.KDTSpesen = SafeGetDecimal(reader, "KDTSpesen", Nothing)
            esSalaryData.MATotal = SafeGetDecimal(reader, "MATotal", Nothing)
            esSalaryData.KDTotal = SafeGetDecimal(reader, "KDTotal", Nothing)
            esSalaryData.MWSTBetrag = SafeGetDecimal(reader, "MWSTBetrag", Nothing)
            esSalaryData.BruttoMarge = SafeGetDecimal(reader, "BruttoMarge", Nothing)
            esSalaryData.LOVon = SafeGetDateTime(reader, "LOVon", Nothing)
            esSalaryData.LOBis = SafeGetDateTime(reader, "LOBis", Nothing)
            esSalaryData.Result = SafeGetString(reader, "Result")
            esSalaryData.AktivLODaten = SafeGetBoolean(reader, "AktivLODaten", Nothing)
            esSalaryData.MargeMitBVG = SafeGetDecimal(reader, "MargeMitBVG", Nothing)
            esSalaryData.GAVNr = SafeGetInteger(reader, "GAVNr", Nothing)
            esSalaryData.GAVKanton = SafeGetString(reader, "GAVKanton")
            esSalaryData.GAVGruppe0 = SafeGetString(reader, "GAVGruppe0")
            esSalaryData.GAVGruppe1 = SafeGetString(reader, "GAVGruppe1")
            esSalaryData.GAVGruppe2 = SafeGetString(reader, "GAVGruppe2")
            esSalaryData.GAVGruppe3 = SafeGetString(reader, "GAVGruppe3")
            esSalaryData.GAVBezeichnung = SafeGetString(reader, "GavBezeichnung")
            esSalaryData.GAV_FAG = SafeGetDecimal(reader, "GAV_FAG", Nothing)
            esSalaryData.GAV_FAN = SafeGetDecimal(reader, "GAV_FAN", Nothing)
            esSalaryData.GAV_WAG = SafeGetDecimal(reader, "GAV_WAG", Nothing)
            esSalaryData.GAV_WAN = SafeGetDecimal(reader, "GAV_WAN", Nothing)
            esSalaryData.GAV_VAG = SafeGetDecimal(reader, "GAV_VAG", Nothing)
            esSalaryData.GAV_VAN = SafeGetDecimal(reader, "GAV_VAN", Nothing)
            esSalaryData.GAV_StdWeek = SafeGetDecimal(reader, "GAV_StdWeek", Nothing)
            esSalaryData.GAV_StdMonth = SafeGetDecimal(reader, "GAV_StdMonth", Nothing)
            esSalaryData.GAV_StdYear = SafeGetDecimal(reader, "GAV_StdYear", Nothing)
            esSalaryData.GAVStdLohn = SafeGetDecimal(reader, "GAVStdLohn", Nothing)
            esSalaryData.GAV_FAG_S = SafeGetDecimal(reader, "GAV_FAG_S", Nothing)
            esSalaryData.GAV_FAN_S = SafeGetDecimal(reader, "GAV_FAN_S", Nothing)
            esSalaryData.GAV_WAG_S = SafeGetDecimal(reader, "GAV_WAG_S", Nothing)
            esSalaryData.GAV_WAN_S = SafeGetDecimal(reader, "GAV_WAN_S", Nothing)
            esSalaryData.GAV_VAG_S = SafeGetDecimal(reader, "GAV_VAG_S", Nothing)
            esSalaryData.GAV_VAN_S = SafeGetDecimal(reader, "GAV_VAN_S", Nothing)
            esSalaryData.GAV_FAG_M = SafeGetDecimal(reader, "GAV_FAG_M", Nothing)
            esSalaryData.GAV_FAN_M = SafeGetDecimal(reader, "GAV_FAN_M", Nothing)
            esSalaryData.GAV_WAG_M = SafeGetDecimal(reader, "GAV_WAG_M", Nothing)
            esSalaryData.GAV_WAN_M = SafeGetDecimal(reader, "GAV_WAN_M", Nothing)
            esSalaryData.GAV_VAG_M = SafeGetDecimal(reader, "GAV_VAG_M", Nothing)
            esSalaryData.GAV_VAN_M = SafeGetDecimal(reader, "GAV_VAN_M", Nothing)
            esSalaryData.FerienWay = SafeGetShort(reader, "FerienWay", Nothing)
            esSalaryData.LO13Way = SafeGetShort(reader, "LO13Way", Nothing)
            esSalaryData.GAV_FAG_J = SafeGetDecimal(reader, "GAV_FAG_J", Nothing)
            esSalaryData.GAV_FAN_J = SafeGetDecimal(reader, "GAV_FAN_J", Nothing)
            esSalaryData.GAV_WAG_J = SafeGetDecimal(reader, "GAV_WAG_J", Nothing)
            esSalaryData.GAV_WAN_J = SafeGetDecimal(reader, "GAV_WAN_J", Nothing)
            esSalaryData.GAV_VAG_J = SafeGetDecimal(reader, "GAV_VAG_J", Nothing)
            esSalaryData.GAV_VAN_J = SafeGetDecimal(reader, "GAV_VAN_J", Nothing)
            esSalaryData.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
            esSalaryData.CreatedFrom = SafeGetString(reader, "CreatedFrom")
            esSalaryData.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
            esSalaryData.ChangedFrom = SafeGetString(reader, "ChangedFrom")
            esSalaryData.VerleihDoc_Guid = SafeGetString(reader, "VerleihDoc_Guid")
            esSalaryData.ESDoc_Guid = SafeGetString(reader, "ESDoc_Guid")
            esSalaryData.Transfered_User = SafeGetString(reader, "Transfered_User")
            esSalaryData.Transfered_On = SafeGetDateTime(reader, "Transfered_On", Nothing)
            esSalaryData.IsPVL = SafeGetByte(reader, "IsPVL", Nothing)
            esSalaryData.FeierBasis = SafeGetDecimal(reader, "FeierBasis", Nothing)
            esSalaryData.GAVInfo_String = SafeGetString(reader, "GAVInfo_String")
            esSalaryData.LOFeiertagWay = SafeGetByte(reader, "LOFeiertagWay", Nothing)
            esSalaryData.GavDate = SafeGetDateTime(reader, "GavDate", Nothing)
            esSalaryData.MargenInfo_String = SafeGetString(reader, "MargenInfo_String")

            esSalaryData.FARPflichtig = (esSalaryData.GAV_FAG + esSalaryData.GAV_FAN) > 0

            Try

              ' Extract MargeOhneBVGProz and MergeMitBVGProz from MargenInfo_String.
              If Not String.IsNullOrWhiteSpace(esSalaryData.MargenInfo_String) Then

								Dim tokens = esSalaryData.MargenInfo_String.Split("¦")
								If tokens.Count > 1 Then
									Dim margeOhneBVGProz = tokens.ElementAt(6)
									Dim margeMitBVGProz = tokens.ElementAt(7)
									esSalaryData.MargeOhneBVGInProzent = Math.Round(Convert.ToDecimal(margeOhneBVGProz), 4)
									esSalaryData.MargeMitBVGInProzent = Math.Round(Convert.ToDecimal(margeMitBVGProz), 4)
								Else
									m_Logger.LogWarning(String.Format("ESNr: {0} | esSalaryData.MargenInfo_String: {1}", esNumber, esSalaryData.MargenInfo_String))

								End If
							End If

            Catch ex As Exception
							m_Logger.LogError(String.Format("ESNr: {0} | esSalaryData.MargenInfo_String: {1} | {2}", esNumber, esSalaryData.MargenInfo_String, ex.ToString()))
            End Try

            result.Add(esSalaryData)

          End While

        End If
      Catch ex As Exception
				m_Logger.LogError(String.Format("ESNr: {0} | {1}", esNumber, ex.ToString()))
				result = Nothing
      Finally
        CloseReader(reader)
      End Try

      Return result

    End Function

    ''' <summary>
    ''' Loads mandant suva HL data.
    ''' </summary>
    ''' <param name="year">The year.</param>
    ''' <param name="mdNr">The mandant number.</param>
    ''' <param name="success">The succcess value.</param>
    ''' <returns>Suva HL value.</returns>
    Public Function LoadMandantSuvaHLData(ByVal year As Integer, ByVal mdNr As Integer, ByRef success As Boolean) As Decimal? Implements IESDatabaseAccess.LoadMandantSuvaHLData

      success = False
      Dim suva_HL As Decimal = Nothing

      Dim sql As String
      sql = "Select IsNull(Suva_HL, 126000) As Suva_HL From Mandanten where Jahr = @year AND MDNr = @mdnr"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("year", Convert.ToString(year)))
      listOfParams.Add(New SqlClient.SqlParameter("mdnr", Convert.ToString(mdNr)))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try
        If reader IsNot Nothing Then

          If reader.Read Then

            suva_HL = SafeGetDecimal(reader, "Suva_HL", Nothing)
            success = True
          End If

        End If

      Catch e As Exception
        m_Logger.LogError(e.ToString())
      Finally
        CloseReader(reader)
      End Try

      Return suva_HL

    End Function

    ''' <summary>
    ''' Loads the ES categorization data (Tab_EsEinstufung).
    ''' </summary>
    ''' <returns>List of ES categorization data.</returns>
    Public Function LoadESCategorizationData() As IEnumerable(Of ESCategorizationData) Implements IESDatabaseAccess.LoadESCategorizationData

      Dim result As List(Of ESCategorizationData) = Nothing

      Dim sql As String

      sql = String.Format("SELECT ID, Bezeichnung, Result, {0} as TranslatedText FROM Tab_ESEinstufung ORDER BY {0} ASC", MapLanguageToColumnName(SelectedTranslationLanguage))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of ESCategorizationData)

          While reader.Read()
            Dim esCategorizationData As New ESCategorizationData
            esCategorizationData.ID = SafeGetInteger(reader, "ID", 0)
            esCategorizationData.Description = SafeGetString(reader, "Bezeichnung")
            esCategorizationData.TranslatedESCategorizationDescription = SafeGetString(reader, "TranslatedText")

            result.Add(esCategorizationData)

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
    ''' Loads ES additional salary type data.
    ''' </summary>
    ''' <param name="esNumber">The ES number.</param>
    ''' <param name="type">The type.</param>
    ''' <param name="esLANr">The optional record number filter.</param>
    ''' <param name="esLohnNumber">Optional ES Lohn number filter.</param>
    ''' <returns>List of additional salary type data.</returns>
    Public Function LoadESAdditionalSalaryTypeData(ByVal esNumber As Integer, ByVal type As ESAdditionalSalaryType, Optional ByVal esLANr As Integer? = Nothing, Optional ByVal esLohnNumber As Integer? = Nothing) As IEnumerable(Of ESEmployeeAndCustomerLAData) Implements IESDatabaseAccess.LoadESAdditionalSalaryTypeData

      Dim result As List(Of ESEmployeeAndCustomerLAData) = Nothing

      Dim table As String = String.Empty
      Select Case type
        Case ESAdditionalSalaryType.Customer
          table = "ES_KD_LA"
        Case ESAdditionalSalaryType.Employee
          table = "ES_MA_LA"
      End Select

      Dim sql As String = String.Empty

      sql = sql & "SELECT [ID]"
      sql = sql & ",[ESNr]"
      sql = sql & ",[KDNr]"
      sql = sql & ",[MANr]"
      sql = sql & ",[ESLANr]"
      sql = sql & ",[KSTNr]"
      sql = sql & ",[LANr]"
      sql = sql & ",[LABez]"
      sql = sql & ",[Betrag]"
      sql = sql & ",[Ansatz]"
      sql = sql & ",[Basis]"
      sql = sql & ",[Tag]"
      sql = sql & ",[Monat]"
      sql = sql & ",[Std]"
      sql = sql & ",[Kilometer]"
      sql = sql & ",[Woche]"
      sql = sql & ",[Vertrag]"
      sql = sql & ",[Currency]"
      sql = sql & ",[Result]"
      sql = sql & ",[ESLohnNr] "
      sql = sql & String.Format("FROM [{0}] ", table)
      sql = sql & "WHERE [ESNr] = @esNr AND (@esLANr IS NULL OR [ESLANr] = @esLANr) AND (@esLohnNr IS NULL OR [ESLohnNr] = @esLohnNr) "
      sql = sql & "ORDER BY ID DESC"

      ' Parameters
      Dim esNumberParameter As New SqlClient.SqlParameter("esNr", esNumber)
      Dim esLANumberParameter As New SqlClient.SqlParameter("esLANr", ReplaceMissing(esLANr, DBNull.Value))
      Dim esLohnNumberParameter As New SqlClient.SqlParameter("esLohnNr", ReplaceMissing(esLohnNumber, DBNull.Value))
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(esNumberParameter)
      listOfParams.Add(esLANumberParameter)
      listOfParams.Add(esLohnNumberParameter)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of ESEmployeeAndCustomerLAData)

          While reader.Read

            Dim esEmplAndCustSalaryTypedata As New ESEmployeeAndCustomerLAData

            esEmplAndCustSalaryTypedata.ID = SafeGetInteger(reader, "ID", 0)
            esEmplAndCustSalaryTypedata.ESNr = SafeGetInteger(reader, "ESNr", Nothing)
            esEmplAndCustSalaryTypedata.CustomerNumber = SafeGetInteger(reader, "KDNr", Nothing)
            esEmplAndCustSalaryTypedata.EmployeeNumber = SafeGetInteger(reader, "MANr", Nothing)
            esEmplAndCustSalaryTypedata.ESLANr = SafeGetInteger(reader, "ESLANr", Nothing)
            esEmplAndCustSalaryTypedata.KSTNr = SafeGetInteger(reader, "KSTNr", Nothing)
            esEmplAndCustSalaryTypedata.LANr = SafeGetDecimal(reader, "LANr", Nothing)
            esEmplAndCustSalaryTypedata.LABez = SafeGetString(reader, "LABez")
            esEmplAndCustSalaryTypedata.Betrag = SafeGetDecimal(reader, "Betrag", Nothing)
            esEmplAndCustSalaryTypedata.Ansatz = SafeGetDecimal(reader, "Ansatz", Nothing)
            esEmplAndCustSalaryTypedata.Basis = SafeGetDecimal(reader, "Basis", Nothing)
            esEmplAndCustSalaryTypedata.Tag = SafeGetBoolean(reader, "Tag", Nothing)
            esEmplAndCustSalaryTypedata.Monat = SafeGetBoolean(reader, "Monat", Nothing)
            esEmplAndCustSalaryTypedata.Std = SafeGetBoolean(reader, "Std", Nothing)
            esEmplAndCustSalaryTypedata.Kilometer = SafeGetBoolean(reader, "Kilometer", Nothing)
            esEmplAndCustSalaryTypedata.Week = SafeGetBoolean(reader, "Woche", Nothing)

            esEmplAndCustSalaryTypedata.Vertrag = SafeGetBoolean(reader, "Vertrag", Nothing)
            esEmplAndCustSalaryTypedata.Currency = SafeGetString(reader, "Currency")
            esEmplAndCustSalaryTypedata.Result = SafeGetString(reader, "Result")
            esEmplAndCustSalaryTypedata.ESLohnNr = SafeGetInteger(reader, "ESLohnNr", 0)

            result.Add(esEmplAndCustSalaryTypedata)

          End While

        End If
      Catch ex As Exception
        m_Logger.LogError(ex.tostring())
        result = Nothing
      Finally
        CloseReader(reader)
      End Try

      Return result

    End Function

    ''' <summary>
    ''' Loads la list data for ES management.
    ''' </summary>
    ''' <param name="year">The year.</param>
    ''' <returns>List of la data or nothing in error case.</returns>
    Public Function LoadLAListForESMng(ByVal year As Integer) As IEnumerable(Of LAData) Implements IESDatabaseAccess.LoadLAListForESMng

      Dim result As List(Of LAData) = Nothing

      Dim sql As String

			sql = "SELECT LANr, LALoText, Vorzeichen, AllowedMore_Anz, AllowedMore_Bas, AllowedMore_Ans, AllowedMore_Btr, Rundung, TypeAnzahl, Fixanzahl, TypeBasis, FixBasis, TypeAnsatz, Fixansatz, MABasVar, KDBasis, MWSTPflichtig "
			sql &= " From LA "
			sql &= "WHERE Verwendung IN ('1', '3') AND LA.LANr not In (500, 501, 600, 601, 700, 701, 5000, 5100, 8910) AND LAJahr = @year AND LADeactivated = 0 ORDER BY LANr "

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
						laData.FixAnzahl = SafeGetDecimal(reader, "FixAnzahl", 1)
						laData.TypeBasis = SafeGetShort(reader, "TypeBasis", Nothing)
						laData.FixBasis = SafeGetDecimal(reader, "FixBasis", 0)
						laData.TypeAnsatz = SafeGetShort(reader, "TypeAnsatz", Nothing)
						laData.FixAnsatz = SafeGetDecimal(reader, "FixAnsatz", 100)

						laData.MABasVar = SafeGetString(reader, "MABasVar")
						laData.KDBasis = SafeGetString(reader, "KDBasis")
						laData.MWSTPflichtig = SafeGetBoolean(reader, "MWSTPflichtig", False)

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
    ''' Gets conflicted LO records in period.
    ''' </summary>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <param name="mdNr">The mandant number.</param>
    ''' <param name="startDate">The start date of the period.</param>
    ''' <param name="endDate">The end date of the period.</param>
    ''' <param name="resultCode">The result code.</param>
    ''' <returns>Conflicting LO records between period and result code.</returns>
    Public Function LoadConflictedLORecordsInPeriod(ByVal employeeNumber As Integer, ByVal mdNr As Integer, ByVal startDate As DateTime, ByVal endDate As DateTime, ByRef resultCode As Integer) As IEnumerable(Of ConflictedLOData) Implements IESDatabaseAccess.LoadConflictedLORecordsInPeriod

      Dim success = True

      Dim result As List(Of ConflictedLOData) = Nothing

      Dim sql As String

      sql = "[Get Conflicted LO Between Period]"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)

      listOfParams.Add(New SqlClient.SqlParameter("@MANr", employeeNumber))
      listOfParams.Add(New SqlClient.SqlParameter("@MDNr", mdNr))
      listOfParams.Add(New SqlClient.SqlParameter("@StartDate", startDate))
      listOfParams.Add(New SqlClient.SqlParameter("@EndDate", endDate))

      Dim resultCodeParameter = New SqlClient.SqlParameter("@ResultCode", SqlDbType.Int)
      resultCodeParameter.Direction = ParameterDirection.Output
      listOfParams.Add(resultCodeParameter)


      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of ConflictedLOData)

          While reader.Read()
            Dim loData As New ConflictedLOData
            loData.LONr = SafeGetInteger(reader, "LONr", Nothing)
						loData.LP = SafeGetInteger(reader, "LP", Nothing)
						loData.Year = SafeGetInteger(reader, "Jahr", Nothing)

            result.Add(loData)

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
    ''' Gets conflicted RPL records in period.
    ''' </summary>
    ''' <param name="esNumber">The ES number.</param>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <param name="startDate">The start date of the period.</param>
    ''' <param name="endDate">The end date of the period.</param>
    ''' <param name="resultCode">The result code.</param>
    ''' <returns>Conflicting RPL records between period and result code.</returns>
    Public Function LoadConflictedRPLRecordsInPeriod(ByVal esNumber As Integer, ByVal employeeNumber As Integer, ByVal startDate As DateTime, ByVal endDate As DateTime, ByRef resultCode As Integer) As IEnumerable(Of ConflictedRPLData) Implements IESDatabaseAccess.LoadConflictedRPLRecordsInPeriod

      Dim success = True

      Dim result As List(Of ConflictedRPLData) = Nothing

      Dim sql As String

      sql = "[Get Conflicted RPL Between Period]"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)

      listOfParams.Add(New SqlClient.SqlParameter("@ESNr", esNumber))
      listOfParams.Add(New SqlClient.SqlParameter("@MANr", employeeNumber))
      listOfParams.Add(New SqlClient.SqlParameter("@StartDate", startDate))
      listOfParams.Add(New SqlClient.SqlParameter("@EndDate", endDate))

      Dim resultCodeParameter = New SqlClient.SqlParameter("@ResultCode", SqlDbType.Int)
      resultCodeParameter.Direction = ParameterDirection.Output
      listOfParams.Add(resultCodeParameter)


      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of ConflictedRPLData)

          While reader.Read()
            Dim rplData As New ConflictedRPLData
            rplData.RPNr = SafeGetInteger(reader, "RPNR", Nothing)
            rplData.VonDate = SafeGetDateTime(reader, "VonDate", Nothing)
            rplData.BisDate = SafeGetDateTime(reader, "BisDate", Nothing)

            result.Add(rplData)

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
    ''' Gets conflicted MonthClose records in period.
    ''' </summary>
    ''' <param name="mdNr">The mandant number.</param>
    ''' <param name="startDate">The start date of the period.</param>
    ''' <param name="endDate">The end date of the period.</param>
    ''' <param name="resultCode">The result code.</param>
    ''' <returns>Conflicting MonthClose records between period and result code.</returns>
    Public Function LoadConflictedMonthCloseRecordsInPeriod(ByVal mdNr As Integer, ByVal startDate As DateTime, ByVal endDate As DateTime, ByRef resultCode As Integer) As IEnumerable(Of ConflictedMonthCloseData) Implements IESDatabaseAccess.LoadConflictedMonthCloseRecordsInPeriod

      Dim success = True

      Dim result As List(Of ConflictedMonthCloseData) = Nothing

      Dim sql As String

      sql = "[Get Conflicted MonthCloseRecords Between Period]"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)

      listOfParams.Add(New SqlClient.SqlParameter("@MDNr", mdNr))
      listOfParams.Add(New SqlClient.SqlParameter("@StartDate", startDate))
      listOfParams.Add(New SqlClient.SqlParameter("@EndDate", endDate))

      Dim resultCodeParameter = New SqlClient.SqlParameter("@ResultCode", SqlDbType.Int)
      resultCodeParameter.Direction = ParameterDirection.Output
      listOfParams.Add(resultCodeParameter)


      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of ConflictedMonthCloseData)

          While reader.Read()
            Dim monthData As New ConflictedMonthCloseData
						monthData.Month = SafeGetInteger(reader, "Monat", Nothing)
            monthData.Year = SafeGetInteger(reader, "Jahr", Nothing)

            result.Add(monthData)

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
    ''' Loads marge boundary values for mandant.
    ''' </summary>
    ''' <param name="mdNumber">The mandant number.</param>
    ''' <returns>The mandant marge boundary values or nothing.</returns>
    Public Function LoadMargeBoundaryValuesForMandant(ByVal mdNumber As Integer, ByVal mdYear As Integer) As MandantMargeBoundaryValues Implements IESDatabaseAccess.LoadMargeBoundaryValuesForMandant

      Dim result As MandantMargeBoundaryValues = Nothing

      Dim sql As String = String.Empty

      sql = sql & "SELECT Top 1 "
      sql = sql & "[B_MARGE]"
      sql = sql & ",[B_MARGEP]"
      sql = sql & " FROM [Mandanten] WHERE [MDNr] = @mdNr And Jahr = @mdYear"

      ' Parameters
      Dim mdNumberParameter As New SqlClient.SqlParameter("mdNr", mdNumber)
      Dim mdYearParameter As New SqlClient.SqlParameter("mdYear", mdYear)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(mdNumberParameter)
      listOfParams.Add(mdYearParameter)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If Not reader Is Nothing Then

          If reader.Read Then
            result = New MandantMargeBoundaryValues

            result.B_Marge = SafeGetDecimal(reader, "B_MARGE", Nothing)
            result.B_MargeP = SafeGetDecimal(reader, "B_MARGEP", Nothing)

          Else
            m_Logger.LogWarning(String.Format("Keine Margendaten für Mandantennummer | Jahr {0} | {1} wurde gefunden.", mdNumber, mdYear))

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
    ''' Loads SUVA data.
    ''' </summary>
    ''' <returns>List of suva data.</returns>
    Public Function LoadSuvaData() As IEnumerable(Of SuvaData) Implements IESDatabaseAccess.LoadSuvaData

      Dim result As List(Of SuvaData) = Nothing

      Dim sql As String

      sql = String.Format("SELECT ID, GetFeld, Description, {0} AS TranslatedDescription FROM TAB_SUVA", MapLanguageToColumnName(SelectedTranslationLanguage))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of SuvaData)

          While reader.Read

            Dim suvaData = New SuvaData()
            suvaData.ID = SafeGetInteger(reader, "ID", 0)
            suvaData.GetField = SafeGetString(reader, "GetFeld")
            suvaData.Description = SafeGetString(reader, "Description")
            suvaData.TranslatedDescription = SafeGetString(reader, "TranslatedDescription")

            suvaData.DataforView = String.Format("{0}: {1}", SafeGetString(reader, "GetFeld"), SafeGetString(reader, "TranslatedDescription"))

            result.Add(suvaData)

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
    ''' Loads existing report number for ES.
    ''' </summary>
    ''' <param name="esNr">The ES number.</param>
    ''' <returns>List of existing report numbers or nothing in error case.</returns>
    Function LoadExistingReportNumbersForES(ByVal esNr As Integer) As IEnumerable(Of Integer) Implements IESDatabaseAccess.LoadExistingReportNumbersForES

      Dim result As List(Of Integer) = Nothing

      Dim sql As String

      sql = String.Format("SELECT RPNR FROM RP WHERE ESNR = @esNr")

      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("@ESNr", esNr))
      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of Integer)

          While reader.Read

            Dim rpNr As Integer = SafeGetInteger(reader, "RPNr", 0)

            result.Add(rpNr)

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
    ''' Checks if ES salary data can be deleted.
    ''' </summary>
    ''' <param name="esNumber">The ES number.</param>
    ''' <param name="esLohnNr">The ES salary number.</param>
    ''' <returns>Boolean value indicating if ES salary data record can be deleted.</returns>
    Public Function CheckIfESSalaryDataCanBeDeleted(ByVal esNumber As Integer, ByVal esLohnNr As Integer) As Boolean Implements IESDatabaseAccess.CheckIfESSalaryDataCanBeDeleted

      Dim canBeDeleted = False

      Dim sql As String

      sql = "SELECT COUNT(*) FROM RPL WHERE ESNr = @ESNr AND ESLohnNR = @ESLohnNr"
      ' Parameters

      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("@ESNr", esNumber))
      listOfParams.Add(New SqlClient.SqlParameter("@ESLohnNr", esLohnNr))

      Dim existingRPLRecordsCount = ExecuteScalar(sql, listOfParams)

      If Not existingRPLRecordsCount Is Nothing Then
        canBeDeleted = (existingRPLRecordsCount <= 0)
      End If

      Return canBeDeleted

    End Function

    ''' <summary>
    ''' Checks if an RP records exists for an ES.
    ''' </summary>
    ''' <param name="esNumber">The ES number.</param>
    ''' <returns>Boolean flag indicating if RP exists for ES.</returns>
    Public Function CheckIfRPExistsForES(ByVal esNumber As Integer) As Boolean? Implements IESDatabaseAccess.CheckIfRPExistsForES

      Dim doesRPExistForES = False

      Dim sql As String

      sql = "SELECT COUNT(*) FROM RP WHERE ESNr = @ESNr"
      ' Parameters

      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("@ESNr", esNumber))

      Dim existingRPLRecordsCount = ExecuteScalar(sql, listOfParams)

      If Not existingRPLRecordsCount Is Nothing Then
        doesRPExistForES = (existingRPLRecordsCount > 0)

        Return doesRPExistForES
      Else
        Return Nothing
      End If

    End Function

    ''' <summary>
    ''' Checks if the Kostenteilung can be changed.
    ''' </summary>
    ''' <param name="esNumber">The es number.</param>
    ''' <returns>Boolean flag indicating if the Kostenteilung can be changed.</returns>
    Public Function CheckIfKostenteilungCanBeChanged(ByVal esNumber As Integer) As Boolean? Implements IESDatabaseAccess.CheckIfKostenteilungCanBeChanged

      Dim result As List(Of SuvaData) = Nothing

      Dim sql As String = String.Empty

      sql = sql & "SELECT TOP 1 RP.ESNr FROM RP WHERE RP.ESNr = @esnr AND RP.LONr > 0 "
      sql = sql & "UNION "
      sql = sql & "SELECT TOP 1 RPL.ESNr FROM RPL WHERE RPL.ESNr = @esnr and RPL.REnr > 0 "
      sql = sql & "UNION "
      sql = sql & "SELECT TOP 1 LM.ESNr FROM LM WHERE LM.esnr = @ESNr "
      sql = sql & "AND EXISTS (SELECT TOP 1 Destlmnr FROM LOL Where Destlmnr = LM.LMNr And LM.LAnr = LOL.LANr And LOL.MANr = LM.MANr)"

      ' Parameters
      Dim esNumberParameter As New SqlClient.SqlParameter("esnr", esNumber)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(esNumberParameter)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          Return Not reader.HasRows

        End If

      Catch e As Exception
        result = Nothing
        m_Logger.LogError(e.ToString())

      Finally
        CloseReader(reader)
      End Try

      ' Error case
      Return Nothing

    End Function

		''' <summary>
		''' Checks if the LOVon can be set.
		''' </summary>
		''' <param name="esNumber">The es number.</param>
		''' <param name="lovonDate">The Date of ESLohn to be set.</param>
		''' <returns>Boolean flag indicating if the LOVon can be set.</returns>
		Public Function CheckIfLOVonDateCanBeSet(ByVal esNumber As Integer, ByVal esLohnNumber As Integer?, ByVal lovonDate As DateTime) As Boolean? Implements IESDatabaseAccess.CheckIfLOVonDateCanBeSet

			Dim result As Boolean = True

			Dim sql As String = String.Empty

			sql = sql & "SELECT TOP 1 RPL.ESNr FROM RPL WHERE RPL.ESNr = @esnr "
			If esLohnNumber.HasValue Then
				sql &= "AND RPL.ESLohnNr = @esLohnNumber "
				sql = sql & "AND RPL.VonDate <> @lovonDate "

			Else
				sql = sql & "AND RPL.VonDate >= @lovonDate "

			End If

			' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("ESNr", esNumber))
			If esLohnNumber.HasValue Then listOfParams.Add(New SqlClient.SqlParameter("esLohnNumber", esLohnNumber))
			listOfParams.Add(New SqlClient.SqlParameter("lovonDate", lovonDate))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					Return Not reader.HasRows

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			' Error case
			Return Nothing

		End Function

    ''' <summary>
    ''' Updates ES data.
    ''' </summary>
    ''' <param name="esData">The es data.</param>
    ''' <returns>Boolean value indicating success.</returns>
    Public Function UpdateESMasterData(ByVal esData As ESMasterData) As Boolean Implements IESDatabaseAccess.UpdateESMasterData

      Dim success = True

      Dim sql As String = String.Empty

      sql = sql & "[UPDATE ES]"
    
      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("@ESNR", ReplaceMissing(esData.ESNR, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@MANR", ReplaceMissing(esData.EmployeeNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@KDNR", ReplaceMissing(esData.CustomerNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@KSTBez", ReplaceMissing(esData.KSTBez, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ESKst", ReplaceMissing(esData.ESKst, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Arbzeit", ReplaceMissing(esData.Arbzeit, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Arbort", ReplaceMissing(esData.Arbort, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Melden", ReplaceMissing(esData.Melden, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ES_Als", ReplaceMissing(esData.ES_Als, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ES_Ab", ReplaceMissing(esData.ES_Ab, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ES_Uhr", ReplaceMissing(esData.ES_Uhr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ES_Ende", ReplaceMissing(esData.ES_Ende, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Ende", ReplaceMissing(esData.Ende, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAVText", ReplaceMissing(esData.GAVText, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Bemerk_MA", ReplaceMissing(esData.Bemerk_MA, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Bemerk_KD", ReplaceMissing(esData.Bemerk_KD, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Bemerk_RE", ReplaceMissing(esData.Bemerk_RE, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Bemerk_Lo", ReplaceMissing(esData.Bemerk_Lo, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Bemerk_P", ReplaceMissing(esData.Bemerk_P, DBNull.Value)))

      listOfParams.Add(New SqlClient.SqlParameter("@dismissalon", ReplaceMissing(esData.dismissalon, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@dismissalfor", ReplaceMissing(esData.dismissalfor, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@dismissalkind", ReplaceMissing(esData.dismissalkind, DBNull.Value)))

      listOfParams.Add(New SqlClient.SqlParameter("@dismissalreason", ReplaceMissing(esData.dismissalreason, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@dismissalwho", ReplaceMissing(esData.dismissalwho, DBNull.Value)))

      listOfParams.Add(New SqlClient.SqlParameter("@RP_Art", ReplaceMissing(esData.RP_Art, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@LeistungsDoc", ReplaceMissing(esData.LeistungsDoc, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@MWST", ReplaceMissing(esData.MWST, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@SUVA", ReplaceMissing(esData.SUVA, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Currency", ReplaceMissing(esData.Currency, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@CreatedOn", ReplaceMissing(esData.CreatedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@CreatedFrom", ReplaceMissing(esData.CreatedFrom, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@CreatedKST", ReplaceMissing(esData.CreatedKST, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ChangedOn", ReplaceMissing(esData.ChangedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ChangedFrom", ReplaceMissing(esData.ChangedFrom, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ChangedKST", ReplaceMissing(esData.ChangedKST, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Result", ReplaceMissing(esData.Result, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@KDZustaendig", ReplaceMissing(esData.KDZustaendig, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ESKST1", ReplaceMissing(esData.ESKST1, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ESKST2", ReplaceMissing(esData.ESKST2, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ESUnterzeichner", ReplaceMissing(esData.ESUnterzeichner, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@VerleihBacked", ReplaceMissing(esData.VerleihBacked, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Bemerk_1", ReplaceMissing(esData.Bemerk_1, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Bemerk_2", ReplaceMissing(esData.Bemerk_2, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Bemerk_3", ReplaceMissing(esData.Bemerk_3, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Print_KD", ReplaceMissing(esData.Print_KD, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Print_MA", ReplaceMissing(esData.Print_MA, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Farpflicht", ReplaceMissing(esData.FarPflichtig, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ESVerBacked", ReplaceMissing(esData.ESVerBacked, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@NoListing", ReplaceMissing(esData.NoListing, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@BVGCode", ReplaceMissing(esData.BVGCode, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Einstufung", ReplaceMissing(esData.Einstufung, String.Empty)))
      listOfParams.Add(New SqlClient.SqlParameter("@ESBranche", ReplaceMissing(esData.ESBranche, String.Empty)))
      listOfParams.Add(New SqlClient.SqlParameter("@GoesLonger", ReplaceMissing(esData.GoesLonger, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ProposeNr", ReplaceMissing(esData.ProposeNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@VakNr", ReplaceMissing(esData.VakNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@PNr", ReplaceMissing(esData.PNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@KDZHDNr", ReplaceMissing(esData.KDZHDNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@MDNr", ReplaceMissing(esData.MDNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@PrintNoRP", ReplaceMissing(esData.PrintNoRP, DBNull.Value)))

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      Return success

    End Function

    ''' <summary>
    '''  Updates ES salary data (ESLohn). Only the columns needed by ESMng are updated.
    ''' </summary>
    ''' <param name="esNumber">The ES number.</param>
    ''' <param name="esLohnNr">The ES salary number.</param>
    ''' <param name="loVon">The LOVon data.</param>
    ''' <returns>Boolean value indicating success.</returns>
    Public Function UpdateESSalrayDataForESMng(ByVal esNumber As Integer, ByVal esLohnNr As Integer, ByVal loVon As DateTime) As Boolean Implements IESDatabaseAccess.UpdateESSalrayDataForESMng

      Dim success = True

      Dim sql As String

      sql = "[UPDATE ESLohn for EinsatzMng]"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)

      listOfParams.Add(New SqlClient.SqlParameter("@ESNr", esNumber))
      listOfParams.Add(New SqlClient.SqlParameter("@ESLohnr", esLohnNr))
      listOfParams.Add(New SqlClient.SqlParameter("@LoVon", loVon)) ' Only column that needs to be updated in ESMng.

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      Return success

    End Function

    ''' <summary>
    ''' Updates ES additional salary type data.
    ''' </summary>
    ''' <param name="type">The type.</param>
    ''' <param name="laSalaryTypeData">The la salary type data.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function UpdateESAdditionalSalaryTypeData(ByVal type As ESAdditionalSalaryType, ByVal laSalaryTypeData As ESEmployeeAndCustomerLAData) As Boolean Implements IESDatabaseAccess.UpdateESAdditionalSalaryTypeData

      Dim success = True

      Dim sql As String

      Select Case type
        Case ESAdditionalSalaryType.Customer
          sql = "[UPDATE ES_KD_LA]"
        Case ESAdditionalSalaryType.Employee
          sql = "[UPDATE ES_MA_LA]"
        Case Else
          Return False
      End Select

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)

      listOfParams.Add(New SqlClient.SqlParameter("@ID", laSalaryTypeData.ID))
      listOfParams.Add(New SqlClient.SqlParameter("@ESNr", ReplaceMissing(laSalaryTypeData.ESNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@KDNr", ReplaceMissing(laSalaryTypeData.CustomerNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@MANr", ReplaceMissing(laSalaryTypeData.EmployeeNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ESLANr", ReplaceMissing(laSalaryTypeData.ESLANr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@KSTNr", ReplaceMissing(laSalaryTypeData.KSTNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@LANr", ReplaceMissing(laSalaryTypeData.LANr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@LABez", ReplaceMissing(laSalaryTypeData.LABez, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Betrag", ReplaceMissing(laSalaryTypeData.Betrag, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Ansatz", ReplaceMissing(laSalaryTypeData.Ansatz, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Basis", ReplaceMissing(laSalaryTypeData.Basis, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Tag", ReplaceMissing(laSalaryTypeData.Tag, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Monat", ReplaceMissing(laSalaryTypeData.Monat, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Std", ReplaceMissing(laSalaryTypeData.Std, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Kilometer", ReplaceMissing(laSalaryTypeData.Kilometer, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Week", ReplaceMissing(laSalaryTypeData.Week, DBNull.Value)))

      listOfParams.Add(New SqlClient.SqlParameter("@Vertrag", ReplaceMissing(laSalaryTypeData.Vertrag, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Currency", ReplaceMissing(laSalaryTypeData.Currency, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Result", ReplaceMissing(laSalaryTypeData.Result, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ESLohnNr", ReplaceMissing(laSalaryTypeData.ESLohnNr, 0)))

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      Return success

    End Function

    ''' <summary>
    ''' Adds a new ES additional salary type data.
    ''' </summary>
    ''' <param name="type">The type.</param>
    ''' <param name="esLAData">The es LA data.</param>
    ''' <param name="laNrOffset">The LANr offset.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Function AddNewESAdditionalSalaryTypeData(ByVal type As ESAdditionalSalaryType, ByVal esLAData As ESEmployeeAndCustomerLAData, ByVal laNrOffset As Integer) As Boolean Implements IESDatabaseAccess.AddNewESAdditionalSalaryTypeData

      Dim success = True

      Dim sql As String

      Select Case type
        Case ESAdditionalSalaryType.Customer
          sql = "[Create New ES_KD_LA]"
        Case ESAdditionalSalaryType.Employee
          sql = "[Create New ES_MA_LA]"
        Case Else
          Return False
      End Select

      ' Parameters

      Dim listOfParams As New List(Of SqlClient.SqlParameter)

      ' Data of ES La.
      listOfParams.Add(New SqlClient.SqlParameter("@ESNr", ReplaceMissing(esLAData.ESNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@KDNr", ReplaceMissing(esLAData.CustomerNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@MANr", ReplaceMissing(esLAData.EmployeeNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@LANr", ReplaceMissing(esLAData.LANr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@LABez", ReplaceMissing(esLAData.LABez, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Betrag", ReplaceMissing(esLAData.Betrag, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Ansatz", ReplaceMissing(esLAData.Ansatz, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Basis", ReplaceMissing(esLAData.Basis, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Tag", ReplaceMissing(esLAData.Tag, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Monat", ReplaceMissing(esLAData.Monat, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Std", ReplaceMissing(esLAData.Std, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Kilometer", ReplaceMissing(esLAData.Kilometer, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Week", ReplaceMissing(esLAData.Week, DBNull.Value)))

      listOfParams.Add(New SqlClient.SqlParameter("@Vertrag", ReplaceMissing(esLAData.Vertrag, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Result", ReplaceMissing(esLAData.Result, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ESLohnNr", ReplaceMissing(esLAData.ESLohnNr, 0)))
      listOfParams.Add(New SqlClient.SqlParameter("@ESLANrOffset", laNrOffset))
      ' KSTNr and Currency are determined in the stored procedure

      ' New ID of ES_MA_LA or ES_KD_LA
      Dim newIdParameter = New SqlClient.SqlParameter("@NewES_LA_Id", SqlDbType.Int)
      newIdParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newIdParameter)

      ' New ESLANr
      Dim newESLAnrParameter = New SqlClient.SqlParameter("@ESLANr", SqlDbType.Int)
      newESLAnrParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newESLAnrParameter)
      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If success AndAlso
        Not newIdParameter.Value Is Nothing AndAlso
        Not newESLAnrParameter Is Nothing Then
        esLAData.ID = CType(newIdParameter.Value, Integer)
        esLAData.ESLANr = CType(newESLAnrParameter.Value, Integer)
      Else
        success = False
      End If

      Return success

    End Function

    ''' <summary>
    ''' Adds a new ES.
    ''' </summary>
    ''' <param name="esMasterData">The es master data.</param>
    ''' <param name="esNumberOffset">The Es number offset.</param>
    ''' <returns>Boolean value indicating success.</returns>
    Public Function AddNewES(ByVal esMasterData As ESMasterData, ByVal esNumberOffset As Integer) As Boolean Implements IESDatabaseAccess.AddNewES

      Dim success = True

      Dim sql As String

      sql = "[Create New ES]"

      ' Parameters

      Dim listOfParams As New List(Of SqlClient.SqlParameter)

      ' Data of ES
      listOfParams.Add(New SqlClient.SqlParameter("@MANR", ReplaceMissing(esMasterData.EmployeeNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@KDNR", ReplaceMissing(esMasterData.CustomerNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@KSTBez", ReplaceMissing(esMasterData.KSTBez, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ESKst", ReplaceMissing(esMasterData.ESKst, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Arbzeit", ReplaceMissing(esMasterData.Arbzeit, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Arbort", ReplaceMissing(esMasterData.Arbort, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Melden", ReplaceMissing(esMasterData.Melden, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ES_Als", ReplaceMissing(esMasterData.ES_Als, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ES_Ab", ReplaceMissing(esMasterData.ES_Ab, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ES_Uhr", ReplaceMissing(esMasterData.ES_Uhr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ES_Ende", ReplaceMissing(esMasterData.ES_Ende, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Ende", ReplaceMissing(esMasterData.Ende, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAVText", ReplaceMissing(esMasterData.GAVText, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Bemerk_MA", ReplaceMissing(esMasterData.Bemerk_MA, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Bemerk_KD", ReplaceMissing(esMasterData.Bemerk_KD, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Bemerk_RE", ReplaceMissing(esMasterData.Bemerk_RE, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Bemerk_Lo", ReplaceMissing(esMasterData.Bemerk_Lo, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Bemerk_P", ReplaceMissing(esMasterData.Bemerk_P, DBNull.Value)))

      listOfParams.Add(New SqlClient.SqlParameter("@dismissalon", ReplaceMissing(esMasterData.dismissalon, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@dismissalfor", ReplaceMissing(esMasterData.dismissalfor, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@dismissalkind", ReplaceMissing(esMasterData.dismissalkind, DBNull.Value)))

      listOfParams.Add(New SqlClient.SqlParameter("@dismissalreason", ReplaceMissing(esMasterData.dismissalreason, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@dismissalwho", ReplaceMissing(esMasterData.dismissalwho, DBNull.Value)))

      listOfParams.Add(New SqlClient.SqlParameter("@RP_Art", ReplaceMissing(esMasterData.RP_Art, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@LeistungsDoc", ReplaceMissing(esMasterData.LeistungsDoc, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@MWST", ReplaceMissing(esMasterData.MWST, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@SUVA", ReplaceMissing(esMasterData.SUVA, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Currency", ReplaceMissing(esMasterData.Currency, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@CreatedOn", ReplaceMissing(esMasterData.CreatedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@CreatedFrom", ReplaceMissing(esMasterData.CreatedFrom, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@CreatedKST", ReplaceMissing(esMasterData.CreatedKST, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ChangedOn", ReplaceMissing(esMasterData.ChangedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ChangedFrom", ReplaceMissing(esMasterData.ChangedFrom, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ChangedKST", ReplaceMissing(esMasterData.ChangedKST, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Result", ReplaceMissing(esMasterData.Result, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@KDZustaendig", ReplaceMissing(esMasterData.KDZustaendig, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ESKST1", ReplaceMissing(esMasterData.ESKST1, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ESKST2", ReplaceMissing(esMasterData.ESKST2, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ESUnterzeichner", ReplaceMissing(esMasterData.ESUnterzeichner, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@VerleihBacked", ReplaceMissing(esMasterData.VerleihBacked, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Bemerk_1", ReplaceMissing(esMasterData.Bemerk_1, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Bemerk_2", ReplaceMissing(esMasterData.Bemerk_2, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Bemerk_3", ReplaceMissing(esMasterData.Bemerk_3, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Print_KD", ReplaceMissing(esMasterData.Print_KD, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Print_MA", ReplaceMissing(esMasterData.Print_MA, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Farpflicht", ReplaceMissing(esMasterData.FarPflichtig, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ESVerBacked", ReplaceMissing(esMasterData.ESVerBacked, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@NoListing", ReplaceMissing(esMasterData.NoListing, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@BVGCode", ReplaceMissing(esMasterData.BVGCode, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Einstufung", ReplaceMissing(esMasterData.Einstufung, String.Empty)))
      listOfParams.Add(New SqlClient.SqlParameter("@ESBranche", ReplaceMissing(esMasterData.ESBranche, String.Empty)))
      listOfParams.Add(New SqlClient.SqlParameter("@GoesLonger", ReplaceMissing(esMasterData.GoesLonger, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ProposeNr", ReplaceMissing(esMasterData.ProposeNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@VakNr", ReplaceMissing(esMasterData.VakNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@PNr", ReplaceMissing(esMasterData.PNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@KDZHDNr", ReplaceMissing(esMasterData.KDZHDNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@MDNr", ReplaceMissing(esMasterData.MDNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@PrintNoRP", ReplaceMissing(esMasterData.PrintNoRP, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ESNumberOffset", esNumberOffset))

      ' New ID of ES
      Dim newIdParameter = New SqlClient.SqlParameter("@NewESID", SqlDbType.Int)
      newIdParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newIdParameter)

      ' New ESNr
      Dim newESNrParameter = New SqlClient.SqlParameter("@ESNr", SqlDbType.Int)
      newESNrParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newESNrParameter)
      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If success AndAlso
        Not newIdParameter.Value Is Nothing AndAlso
        Not newESNrParameter Is Nothing Then
        esMasterData.ID = CType(newIdParameter.Value, Integer)
        esMasterData.ESNR = CType(newESNrParameter.Value, Integer)
      Else
        success = False
      End If

      Return success

    End Function

    ''' <summary>
    ''' Adds a new ES Lohn.
    ''' </summary>
    ''' <param name="esSalaryData">The es salary data.</param>
    ''' <returns>Boolean value indicating success.</returns>
    Public Function AddNewESLohn(ByVal esSalaryData As ESSalaryData) As Boolean Implements IESDatabaseAccess.AddNewESLohn

      Dim success = True

      Dim sql As String

      sql = "[Create New ESLohn]"

      ' Parameters

      Dim listOfParams As New List(Of SqlClient.SqlParameter)

      ' Data of ESLohn
      listOfParams.Add(New SqlClient.SqlParameter("@ESNr", ReplaceMissing(esSalaryData.ESNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@MANr", ReplaceMissing(esSalaryData.EmployeeNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@KDNr", ReplaceMissing(esSalaryData.CustomerNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@KSTNr", ReplaceMissing(esSalaryData.KSTNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@KSTBez", ReplaceMissing(esSalaryData.KSTBez, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GavText", ReplaceMissing(esSalaryData.GavText, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GrundLohn", ReplaceMissing(esSalaryData.GrundLohn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@StundenLohn", ReplaceMissing(esSalaryData.StundenLohn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@FerBasis", ReplaceMissing(esSalaryData.FerBasis, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Ferien", ReplaceMissing(esSalaryData.Ferien, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@FerienProz", ReplaceMissing(esSalaryData.FerienProz, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Feier", ReplaceMissing(esSalaryData.Feier, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@FeierProz", ReplaceMissing(esSalaryData.FeierProz, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Basis13", ReplaceMissing(esSalaryData.Basis13, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Lohn13", ReplaceMissing(esSalaryData.Lohn13, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Lohn13Proz", ReplaceMissing(esSalaryData.Lohn13Proz, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Tarif", ReplaceMissing(esSalaryData.Tarif, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@MAStdSpesen", ReplaceMissing(esSalaryData.MAStdSpesen, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@MATSpesen", ReplaceMissing(esSalaryData.MATSpesen, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@KDTSpesen", ReplaceMissing(esSalaryData.KDTSpesen, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@MATotal", ReplaceMissing(esSalaryData.MATotal, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@KDTotal", ReplaceMissing(esSalaryData.KDTotal, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@MWSTBetrag", ReplaceMissing(esSalaryData.MWSTBetrag, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@BruttoMarge", ReplaceMissing(esSalaryData.BruttoMarge, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@LOVon", ReplaceMissing(esSalaryData.LOVon, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Result", ReplaceMissing(esSalaryData.Result, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@AktivLODaten", ReplaceMissing(esSalaryData.AktivLODaten, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@MargeMitBVG", ReplaceMissing(esSalaryData.MargeMitBVG, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAVNr", ReplaceMissing(esSalaryData.GAVNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAVKanton", ReplaceMissing(esSalaryData.GAVKanton, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAVGruppe0", ReplaceMissing(esSalaryData.GAVGruppe0, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAVGruppe1", ReplaceMissing(esSalaryData.GAVGruppe1, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAVGruppe2", ReplaceMissing(esSalaryData.GAVGruppe2, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAVGruppe3", ReplaceMissing(esSalaryData.GAVGruppe3, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAVBezeichnung", ReplaceMissing(esSalaryData.GAVBezeichnung, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAV_FAG", ReplaceMissing(esSalaryData.GAV_FAG, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAV_FAN", ReplaceMissing(esSalaryData.GAV_FAN, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAV_WAG", ReplaceMissing(esSalaryData.GAV_WAG, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAV_WAN", ReplaceMissing(esSalaryData.GAV_WAN, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAV_VAG", ReplaceMissing(esSalaryData.GAV_VAG, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAV_VAN", ReplaceMissing(esSalaryData.GAV_VAN, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAV_StdWeek", ReplaceMissing(esSalaryData.GAV_StdWeek, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAV_StdMonth", ReplaceMissing(esSalaryData.GAV_StdMonth, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAV_StdYear", ReplaceMissing(esSalaryData.GAV_StdYear, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAVStdLohn", ReplaceMissing(esSalaryData.GAVStdLohn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAV_FAG_S", ReplaceMissing(esSalaryData.GAV_FAG_S, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAV_FAN_S", ReplaceMissing(esSalaryData.GAV_FAN_S, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAV_WAG_S", ReplaceMissing(esSalaryData.GAV_WAG_S, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAV_WAN_S", ReplaceMissing(esSalaryData.GAV_WAN_S, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAV_VAG_S", ReplaceMissing(esSalaryData.GAV_VAG_S, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAV_VAN_S", ReplaceMissing(esSalaryData.GAV_VAN_S, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAV_FAG_M", ReplaceMissing(esSalaryData.GAV_FAG_M, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAV_FAN_M", ReplaceMissing(esSalaryData.GAV_FAN_M, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAV_WAG_M", ReplaceMissing(esSalaryData.GAV_WAG_M, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAV_WAN_M", ReplaceMissing(esSalaryData.GAV_WAN_M, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAV_VAG_M", ReplaceMissing(esSalaryData.GAV_VAG_M, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAV_VAN_M", ReplaceMissing(esSalaryData.GAV_VAN_M, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@FerienWay", ReplaceMissing(esSalaryData.FerienWay, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@LO13Way", ReplaceMissing(esSalaryData.LO13Way, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAV_FAG_J", ReplaceMissing(esSalaryData.GAV_FAG_J, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAV_FAN_J", ReplaceMissing(esSalaryData.GAV_FAN_J, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAV_WAG_J", ReplaceMissing(esSalaryData.GAV_WAG_J, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAV_WAN_J", ReplaceMissing(esSalaryData.GAV_WAN_J, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAV_VAG_J", ReplaceMissing(esSalaryData.GAV_VAG_J, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAV_VAN_J", ReplaceMissing(esSalaryData.GAV_VAN_J, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@CreatedOn", ReplaceMissing(esSalaryData.CreatedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@CreatedFrom", ReplaceMissing(esSalaryData.CreatedFrom, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ChangedOn", ReplaceMissing(esSalaryData.ChangedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ChangedFrom", ReplaceMissing(esSalaryData.ChangedFrom, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@VerleihDoc_Guid", ReplaceMissing(esSalaryData.VerleihDoc_Guid, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ESDoc_Guid", ReplaceMissing(esSalaryData.ESDoc_Guid, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Transfered_User", ReplaceMissing(esSalaryData.Transfered_User, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Transfered_On", ReplaceMissing(esSalaryData.Transfered_On, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@IsPVL", ReplaceMissing(esSalaryData.IsPVL, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@FeierBasis", ReplaceMissing(esSalaryData.FeierBasis, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAVInfo_String", ReplaceMissing(esSalaryData.GAVInfo_String, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@LOFeiertagWay", ReplaceMissing(esSalaryData.LOFeiertagWay, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAVDate", ReplaceMissing(esSalaryData.GavDate, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@MargenInfo_String", ReplaceMissing(esSalaryData.MargenInfo_String, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("PVLDatabaseName", ReplaceMissing(esSalaryData.PVLDatabaseName, DBNull.Value)))


			' New ID of ESLohn
			Dim newIdParameter = New SqlClient.SqlParameter("@NewESLohnID", SqlDbType.Int)
      newIdParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newIdParameter)

      ' New ESLohnNr
      Dim newESLohnNrParameter = New SqlClient.SqlParameter("@ESLohnNr", SqlDbType.Int)
      newESLohnNrParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newESLohnNrParameter)
      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If success AndAlso
        Not newIdParameter.Value Is Nothing AndAlso
        Not newESLohnNrParameter Is Nothing Then
        esSalaryData.ID = CType(newIdParameter.Value, Integer)
        esSalaryData.ESLohnNr = CType(newESLohnNrParameter.Value, Integer)
      Else
        success = False
      End If

      Return success

    End Function

    ''' <summary>
    ''' Adds a new RP.
    ''' </summary>
    ''' <param name="rp">The RP data.</param>
    ''' <param name="rpNumberOffset">The RP number offset.</param>
    ''' <returns>Boolean value indicating success.</returns>
    Public Function AddNewRP(ByVal rp As RPData, ByVal rpNumberOffset As Integer) As Boolean Implements IESDatabaseAccess.AddNewRP

      Dim success = True

      Dim sql As String

      sql = "[Create New RP]"

      ' Parameters

      Dim listOfParams As New List(Of SqlClient.SqlParameter)

      ' Data of RP
      listOfParams.Add(New SqlClient.SqlParameter("@ESNR", ReplaceMissing(rp.ESNR, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@MANR", ReplaceMissing(rp.MANR, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@KDNR", ReplaceMissing(rp.KDNR, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@LONr", ReplaceMissing(rp.LONr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Currency", ReplaceMissing(rp.Currency, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@SUVA", ReplaceMissing(rp.SUVA, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Monat", ReplaceMissing(rp.Monat, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Jahr", ReplaceMissing(rp.Jahr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Von", ReplaceMissing(rp.Von, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Bis", ReplaceMissing(rp.Bis, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Erfasst", ReplaceMissing(rp.Erfasst, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Result", ReplaceMissing(rp.Result, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPKST", ReplaceMissing(rp.RPKST, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPKST1", ReplaceMissing(rp.RPKST1, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPKST2", ReplaceMissing(rp.RPKST2, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@PrintedWeeks", ReplaceMissing(rp.PrintedWeeks, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@PrintedDate", ReplaceMissing(rp.PrintedDate, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@FarPflicht", ReplaceMissing(rp.FarPflicht, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@BVGStd", ReplaceMissing(rp.BVGStd, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@CreatedFrom", ReplaceMissing(rp.CreatedFrom, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@CreatedOn", ReplaceMissing(rp.CreatedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@BVGCode", ReplaceMissing(rp.BVGCode, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPGAV_FAG", ReplaceMissing(rp.RPGAV_FAG, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPGAV_FAN", ReplaceMissing(rp.RPGAV_FAN, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPGAV_WAG", ReplaceMissing(rp.RPGAV_WAG, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPGAV_WAN", ReplaceMissing(rp.RPGAV_WAN, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPGAV_VAG", ReplaceMissing(rp.RPGAV_VAG, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPGAV_VAN", ReplaceMissing(rp.RPGAV_VAN, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPGAV_Nr", ReplaceMissing(rp.RPGAV_Nr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPGAV_Kanton", ReplaceMissing(rp.RPGAV_Kanton, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPGAV_Beruf", ReplaceMissing(rp.RPGAV_Beruf, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPGAV_Gruppe1", ReplaceMissing(rp.RPGAV_Gruppe1, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPGAV_Gruppe2", ReplaceMissing(rp.RPGAV_Gruppe2, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPGAV_Gruppe3", ReplaceMissing(rp.RPGAV_Gruppe3, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPGAV_Text", ReplaceMissing(rp.RPGAV_Text, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPGAV_StdWeek", ReplaceMissing(rp.RPGAV_StdWeek, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPGAV_StdMonth", ReplaceMissing(rp.RPGAV_StdMonth, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPGAV_StdYear", ReplaceMissing(rp.RPGAV_StdYear, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPGAV_FAG_M", ReplaceMissing(rp.RPGAV_FAG_M, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPGAV_FAN_M", ReplaceMissing(rp.RPGAV_FAN_M, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPGAV_VAG_M", ReplaceMissing(rp.RPGAV_VAG_M, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPGAV_VAN_M", ReplaceMissing(rp.RPGAV_VAN_M, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPGAV_WAG_M", ReplaceMissing(rp.RPGAV_WAG_M, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPGAV_WAN_M", ReplaceMissing(rp.RPGAV_WAN_M, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPGAV_FAG_S", ReplaceMissing(rp.RPGAV_FAG_S, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPGAV_FAN_S", ReplaceMissing(rp.RPGAV_FAN_S, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPGAV_VAG_S", ReplaceMissing(rp.RPGAV_VAG_S, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPGAV_VAN_S", ReplaceMissing(rp.RPGAV_VAN_S, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPGAV_WAG_S", ReplaceMissing(rp.RPGAV_WAG_S, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPGAV_WAN_S", ReplaceMissing(rp.RPGAV_WAN_S, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPGAV_FAG_J", ReplaceMissing(rp.RPGAV_FAG_J, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPGAV_FAN_J", ReplaceMissing(rp.RPGAV_FAN_J, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPGAV_VAG_J", ReplaceMissing(rp.RPGAV_VAG_J, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPGAV_VAN_J", ReplaceMissing(rp.RPGAV_VAN_J, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPGAV_WAG_J", ReplaceMissing(rp.RPGAV_WAG_J, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPGAV_WAN_J", ReplaceMissing(rp.RPGAV_WAN_J, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ES_Einstufung", ReplaceMissing(rp.ES_Einstufung, String.Empty)))
      listOfParams.Add(New SqlClient.SqlParameter("@KDBranche", ReplaceMissing(rp.KDBranche, String.Empty)))
      listOfParams.Add(New SqlClient.SqlParameter("@ProposeNr", ReplaceMissing(rp.ProposeNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPDoc_Guid", ReplaceMissing(rp.RPDoc_Guid, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@MDNr", ReplaceMissing(rp.MDNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@RPNumberOffset", rpNumberOffset))

      ' New ID of RP
      Dim newIdParameter = New SqlClient.SqlParameter("@NewRPID ", SqlDbType.Int)
      newIdParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newIdParameter)

      ' New RPNr
      Dim newRPNrParameter = New SqlClient.SqlParameter("@RPNr ", SqlDbType.Int)
      newRPNrParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newRPNrParameter)
      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If success AndAlso
        Not newIdParameter.Value Is Nothing AndAlso
        Not newRPNrParameter Is Nothing Then
        rp.ID = CType(newIdParameter.Value, Integer)
        rp.RPNR = CType(newRPNrParameter.Value, Integer)
      Else
        success = False
      End If

      Return success

    End Function

    ''' <summary>
    ''' Adds a new ES salary GAV data (ESLohn_GAVData).
    ''' </summary>
    ''' <param name="esSalaryGAVData">The ES salary GAV data.</param>
    ''' <returns>Boolean value indicating success.</returns>
    Public Function AddNewESLohnGAVData(ByVal esSalaryGAVData As ESSalaryGAVData) As Boolean Implements IESDatabaseAccess.AddNewESLohnGAVData

      Dim success = True

      Dim sql As String

      sql = "[Create New ESLohn_GAVData]"

      ' Parameters

      Dim listOfParams As New List(Of SqlClient.SqlParameter)

      ' Data of RP
      listOfParams.Add(New SqlClient.SqlParameter("@ESNR", ReplaceMissing(esSalaryGAVData.ESNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ESLohnNr", ReplaceMissing(esSalaryGAVData.ESLohnNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@MANr", ReplaceMissing(esSalaryGAVData.EmployeeNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@KDNr", ReplaceMissing(esSalaryGAVData.CustomerNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@GAVNr", ReplaceMissing(esSalaryGAVData.GAVNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Kanton", ReplaceMissing(esSalaryGAVData.Kanton, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Zusatz1", ReplaceMissing(esSalaryGAVData.Zusatz1, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Zusatz2", ReplaceMissing(esSalaryGAVData.Zusatz2, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Zusatz3", ReplaceMissing(esSalaryGAVData.Zusatz3, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Zusatz4", ReplaceMissing(esSalaryGAVData.Zusatz4, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Zusatz5", ReplaceMissing(esSalaryGAVData.Zusatz5, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Zusatz6", ReplaceMissing(esSalaryGAVData.Zusatz6, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Zusatz7", ReplaceMissing(esSalaryGAVData.Zusatz7, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Zusatz8", ReplaceMissing(esSalaryGAVData.Zusatz8, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Zusatz9", ReplaceMissing(esSalaryGAVData.Zusatz9, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Zusatz10", ReplaceMissing(esSalaryGAVData.Zusatz10, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Zusatz11", ReplaceMissing(esSalaryGAVData.Zusatz11, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Zusatz12", ReplaceMissing(esSalaryGAVData.Zusatz12, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Zusatz13", ReplaceMissing(esSalaryGAVData.Zusatz13, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Zusatz14", ReplaceMissing(esSalaryGAVData.Zusatz14, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Zusatz15", ReplaceMissing(esSalaryGAVData.Zusatz15, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Zusatz16", ReplaceMissing(esSalaryGAVData.Zusatz16, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Zusatz17", ReplaceMissing(esSalaryGAVData.Zusatz17, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Zusatz18", ReplaceMissing(esSalaryGAVData.Zusatz18, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Zusatz19", ReplaceMissing(esSalaryGAVData.Zusatz19, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Zusatz20", ReplaceMissing(esSalaryGAVData.Zusatz20, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@CreatedOn", ReplaceMissing(esSalaryGAVData.CreatedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@CreatedFrom", ReplaceMissing(esSalaryGAVData.CreatedFrom, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@IsPVL", ReplaceMissing(esSalaryGAVData.IsPVL, DBNull.Value)))

      ' New ID of ESLohn_GAVData
      Dim newIdParameter = New SqlClient.SqlParameter("@NewESLohnGAVDataID ", SqlDbType.Int)
      newIdParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newIdParameter)
      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If success AndAlso
        Not newIdParameter.Value Is Nothing Then
        esSalaryGAVData.ID = CType(newIdParameter.Value, Integer)
      Else
        success = False
      End If

      Return success

    End Function

    ''' <summary>
    ''' Add a new ES with a ES salary (ESLohn / ESLohn_GAVData) and RP.
    ''' </summary>
    ''' <param name="esMasterData">The ES data.</param>
    ''' <param name="esSalaryData">The ES salary data.</param>
    ''' <param name="esSalaryGAVData">The ES salary GAV data.</param>
    ''' <param name="rpData">The RP data.</param>
    ''' <param name="esNumberOffset">The ES number offset.</param>
    ''' <param name="rpNumberOffset">The RP number offset.</param>
    ''' <returns>Boolean value indicating success.</returns>
    Public Function AddNewESWithESLohnAndRP(ByVal esMasterData As ESMasterData,
                                            ByVal esSalaryData As ESSalaryData,
                                            ByVal esSalaryGAVData As ESSalaryGAVData,
                                            ByVal rpData As RPData,
                                            ByVal esNumberOffset As Integer,
                                            ByVal rpNumberOffset As Integer) As Boolean Implements IESDatabaseAccess.AddNewESWithESLohnAndRP

      Dim success As Boolean = True

      success = AddNewES(esMasterData, esNumberOffset)

      esSalaryData.ESNr = esMasterData.ESNR
      success = success AndAlso AddNewESLohn(esSalaryData)

      If success AndAlso Not esSalaryGAVData Is Nothing Then
        esSalaryGAVData.ESNr = esMasterData.ESNR
        esSalaryGAVData.ESLohnNr = esSalaryData.ESLohnNr
        success = success AndAlso AddNewESLohnGAVData(esSalaryGAVData)
      End If

      If success AndAlso Not rpData Is Nothing Then
        rpData.ESNR = esMasterData.ESNR
				success = success AndAlso AddNewRP(rpData, rpNumberOffset)
			End If

      If Not success AndAlso esMasterData.ESNR.HasValue Then

        ' Clean incomplete data from db.

        Dim cleanSQL As String = String.Empty

        cleanSQL = cleanSQL & "DELETE FROM ES WHERE ESNr = @ESNr;"
        cleanSQL = cleanSQL & "DELETE FROM ESLohn WHERE ESNr = @ESNr;"
        cleanSQL = cleanSQL & "DELETE FROM ESLohn_GAVData WHERE ESNr = @ESNr;"
        cleanSQL = cleanSQL & "DELETE FROM RP WHERE ESNr = @ESNr;"

        Dim listOfParams As New List(Of SqlClient.SqlParameter)
        listOfParams.Add(New SqlClient.SqlParameter("@ESNr", esMasterData.ESNR))

        ExecuteNonQuery(cleanSQL, listOfParams, CommandType.Text, False)

      End If

      Return success

    End Function

    ''' <summary>
    ''' Add a new ES salary (ESLohn / ESLohn_GAVData) and RP.
    ''' </summary>
    ''' <param name="esSalaryData">The ES salary data.</param>
    ''' <param name="esSalaryGAVData">The ES salary GAV data.</param>
    ''' <param name="rpData">The RP data.</param>
    ''' <param name="rpNumberOffset">The RP number offset.</param>
    ''' <returns>Boolean value indicating success.</returns>
    Public Function AddNewESLohnAndRP(ByVal esNr As Integer,
                                            ByVal esSalaryData As ESSalaryData,
                                            ByVal esSalaryGAVData As ESSalaryGAVData,
                                            ByVal rpData As RPData,
                                            ByVal rpNumberOffset As Integer) As Boolean Implements IESDatabaseAccess.AddNewESLohnAndRP

      Dim success As Boolean = True

      ' TODO: Delete on error

      esSalaryData.ESNr = esNr
      success = success AndAlso AddNewESLohn(esSalaryData)

      If success AndAlso Not esSalaryGAVData Is Nothing Then
        esSalaryGAVData.ESNr = esNr
        esSalaryGAVData.ESLohnNr = esSalaryData.ESLohnNr
        success = success AndAlso AddNewESLohnGAVData(esSalaryGAVData)
      End If

      If success AndAlso Not rpData Is Nothing Then
        rpData.ESNR = esNr
        success = success AndAlso AddNewRP(rpData, rpNumberOffset)
      End If

      If Not success AndAlso esSalaryData.ESLohnNr.HasValue Then

        ' Clean incomplete data from db.

        Dim cleanSQL As String = String.Empty
        Dim listOfParams As New List(Of SqlClient.SqlParameter)

        cleanSQL = cleanSQL & "DELETE FROM ESLohn WHERE ESNr = @ESNr AND ESLohnNr = @ESLohnNr;"
        cleanSQL = cleanSQL & "DELETE FROM ESLohn_GAVData WHERE ESNr = @ESNr AND ESLohnNr = @ESLohnNr;"

        listOfParams.Add(New SqlClient.SqlParameter("@ESNr", esSalaryData.ESNr))
        listOfParams.Add(New SqlClient.SqlParameter("@ESLohnNr", esSalaryData.ESLohnNr))

        If Not rpData Is Nothing AndAlso rpData.RPNR.HasValue Then
          cleanSQL = cleanSQL & "DELETE FROM RP WHERE ESNr = @ESNr AND RPNr = @RPNr;"
          listOfParams.Add(New SqlClient.SqlParameter("@RPNr", rpData.RPNR))
        End If

        ExecuteNonQuery(cleanSQL, listOfParams, CommandType.Text, False)

      End If

      Return success

    End Function

    ''' <summary>
    ''' Activates an ES salary data record.
    ''' </summary>
    ''' <param name="esNr">The ES number.</param>
    ''' <param name="esLohnNr">The ES salary number.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function ActivateESSalaryData(ByVal esNr As Integer, ByVal esLohnNr As Integer) As Boolean Implements IESDatabaseAccess.ActivateESSalaryData

      Dim success = True

      Dim sql As String

      sql = "[Activate ESLohn]"

      ' Parameters

      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("@ESNr", esNr))
      listOfParams.Add(New SqlClient.SqlParameter("@ESLohnr", esLohnNr))

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      Return success

    End Function

    ''' <summary>
    ''' Deletes an ES salary data record.
    ''' </summary>
    ''' <param name="id">The database id of ES salary data record..</param>
    ''' <param name="modul">The modul name the deletion is performed in.</param>
    ''' <param name="username">The username of the person which deletes the record.</param>
    ''' <param name="usnr">The USNr number.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function DeleteESSalaryData(ByVal id As Integer, ByVal modul As String, ByVal username As String, ByVal usnr As Integer) As DeleteESSalaryResult Implements IESDatabaseAccess.DeleteESSalaryData

      Dim success = True

      Dim sql As String

      sql = "[Delete ESLohn]"

      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("id", id))
      listOfParams.Add(New SqlClient.SqlParameter("modul", modul))
      listOfParams.Add(New SqlClient.SqlParameter("username", username))
      listOfParams.Add(New SqlClient.SqlParameter("usnr", usnr))

      Dim resultParameter = New SqlClient.SqlParameter("@result", SqlDbType.Int)
      resultParameter.Direction = ParameterDirection.Output
      listOfParams.Add(resultParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      Dim resultEnum As DeleteESSalaryResult

      If Not resultParameter.Value Is Nothing Then
        Try
          resultEnum = CType(resultParameter.Value, DeleteESSalaryResult)
        Catch
          resultEnum = DeleteESSalaryResult.ResultDeleteError
        End Try
      Else
        resultEnum = DeleteESSalaryResult.ResultDeleteError
      End If

      Return resultEnum

    End Function

    ''' <summary>
    ''' Deletes ES additionaly salary type data.
    ''' </summary>
    ''' <param name="id">The id.</param>
    ''' <param name="type">The type.</param>
    ''' <param name="modul">The modul name the deletion is performed in.</param>
    ''' <param name="username">The username of the person which deletes the record.</param>
    ''' <param name="usnr">The USNr number.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function DeleteESAdditionalSalaryTypeData(ByVal id As Integer, ByVal type As ESAdditionalSalaryType, ByVal modul As String, ByVal username As String, ByVal usnr As Integer) As DeleteESSalaryTypeResult Implements IESDatabaseAccess.DeleteESAdditionalSalaryTypeData

      Dim success = True

      Dim sql As String

      Select Case type
        Case ESAdditionalSalaryType.Customer
          sql = "[Delete ES_KD_LA]"
        Case ESAdditionalSalaryType.Employee
          sql = "[Delete ES_MA_LA]"
        Case Else
          Return False
      End Select

      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("id", id))
      listOfParams.Add(New SqlClient.SqlParameter("modul", modul))
      listOfParams.Add(New SqlClient.SqlParameter("username", username))
      listOfParams.Add(New SqlClient.SqlParameter("usnr", usnr))

      Dim resultParameter = New SqlClient.SqlParameter("@result", SqlDbType.Int)
      resultParameter.Direction = ParameterDirection.Output
      listOfParams.Add(resultParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      Dim resultEnum As DeleteESSalaryTypeResult

      If Not resultParameter.Value Is Nothing Then
        Try
          resultEnum = CType(resultParameter.Value, DeleteESSalaryTypeResult)
        Catch
          resultEnum = DeleteESSalaryTypeResult.ResultDeleteError
        End Try
      Else
        resultEnum = DeleteESSalaryTypeResult.ResultDeleteError
      End If

      Return resultEnum


    End Function


    ''' <summary>
    ''' Gets founded RP records in ES.
    ''' </summary>
    ''' <param name="esNumber">The ES number.</param>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function LoadFoundedRPInESMng(ByVal esNumber As Integer?, ByVal employeeNumber As Integer?, ByVal customerNumber As Integer?) As IEnumerable(Of FoundedReports) Implements IESDatabaseAccess.LoadFoundedRPInESMng

      Dim success = True

      Dim result As List(Of FoundedReports) = Nothing

      Dim sql As String

      sql = "Select RP.RPNr, RP.MANr, RP.KDNr, RP.Von, RP.Bis, Convert(int, RP.Monat) As Monat, Convert(Int, RP.Jahr) As Jahr, RP.Erfasst, "
      sql &= "(MA.Nachname + ', ' + MA.Vorname) As MAName, KD.Firma1, ES.ES_Als "
      sql &= "From RP "
      sql &= "Left Join Mitarbeiter MA On RP.MANr = MA.MANr "
      sql &= "Left Join Kunden KD On RP.KDNr = KD.KDNr "
      sql &= "Left Join ES On RP.ESNr = ES.ESNr "
      sql &= "Where "
      sql &= "(RP.ESNr = @ESNr Or @ESNr = 0) "
      sql &= "And (RP.MANr = @MANr Or @MANr = 0) "
      sql &= "And (RP.KDNr = @KDNr Or @KDNr = 0) "
      sql &= "Order By RP.Jahr Desc, RP.Monat Desc "

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)

      listOfParams.Add(New SqlClient.SqlParameter("@ESNr", ReplaceMissing(esNumber, 0)))
      listOfParams.Add(New SqlClient.SqlParameter("@MANr", ReplaceMissing(employeeNumber, 0)))
      listOfParams.Add(New SqlClient.SqlParameter("@KDNr", ReplaceMissing(customerNumber, 0)))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of FoundedReports)

          While reader.Read()
            Dim rplData As New FoundedReports
            rplData.RPNr = SafeGetInteger(reader, "RPNR", Nothing)
            rplData.MANr = SafeGetInteger(reader, "MANR", Nothing)
            rplData.KDNr = SafeGetInteger(reader, "KDNR", Nothing)
            rplData.VonDate = SafeGetDateTime(reader, "Von", Nothing)
            rplData.BisDate = SafeGetDateTime(reader, "Bis", Nothing)

            rplData.rpmonth = SafeGetInteger(reader, "Monat", Nothing)
            rplData.rpyear = SafeGetInteger(reader, "Jahr", Nothing)

            rplData.employeename = SafeGetString(reader, "MAName")
            rplData.customername = SafeGetString(reader, "Firma1")
            rplData.esals = SafeGetString(reader, "ES_Als")

            rplData.isfinished = SafeGetBoolean(reader, "Erfasst", Nothing)

            result.Add(rplData)

          End While

          reader.Close()

        End If

      Catch e As Exception
        result = Nothing
        m_Logger.LogError(e.StackTrace)

      Finally
        CloseReader(reader)

      End Try

      Return result

    End Function


#End Region


    ''' <summary>
    ''' Loads context menu data for print.
    ''' </summary>
    Public Function LoadContextMenu4PrintData() As IEnumerable(Of ContextMenuForPrint) Implements IESDatabaseAccess.LoadContextMenu4PrintData

      Dim result As List(Of ContextMenuForPrint) = Nothing

      Dim sql As String

      sql = "[Get List Of Documents For Print in ES]"

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing, CommandType.StoredProcedure)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of ContextMenuForPrint)

          Dim mnuItems As New ContextMenuForPrint
          mnuItems.MnuName = String.Empty
          mnuItems.MnuCaption = "Beide Verträge drucken"
          result.Add(mnuItems)

          While reader.Read()
            mnuItems = New ContextMenuForPrint
            mnuItems.MnuName = SafeGetString(reader, "jobNr", String.Empty)
            mnuItems.MnuCaption = SafeGetString(reader, "Bezeichnung", String.Empty)

            result.Add(mnuItems)

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
    ''' Loads context menu data for print (templates).
    ''' </summary>
    Public Function LoadContextMenu4PrintTemplatesData() As IEnumerable(Of ContextMenuForPrintTemplates) Implements IESDatabaseAccess.LoadContextMenu4PrintTemplatesData

      Dim result As List(Of ContextMenuForPrintTemplates) = Nothing

      Dim sql As String

      sql = "[Get List Of Templates for Print Documents in ES]"

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing, CommandType.StoredProcedure)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of ContextMenuForPrintTemplates)

          While reader.Read()
            Dim mnuItems As New ContextMenuForPrintTemplates
            mnuItems.MnuDocPath = SafeGetString(reader, "docfullname", String.Empty)
            mnuItems.MnuDocMacro = SafeGetString(reader, "makroname", String.Empty)
            mnuItems.MnuCaption = SafeGetString(reader, "menulabel", String.Empty)

            result.Add(mnuItems)

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
