Imports SP.DatabaseAccess
Imports System.Text
'Imports System.Transactions
Imports SP.DatabaseAccess.Report.DataObjects
Imports SP.Infrastructure.DateAndTimeCalculation

Namespace Report

	Partial Public Class ReportDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IReportDatabaseAccess

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
			MyBase.New(connectionString, translationLanguage)
		End Sub

#End Region

#Region "Public Methods"

		''' <summary>
		''' Loads RP master data.
		''' </summary>
		''' <param name="rpNr">The RP number.</param>
		''' <returns>Report master data or nothing in error case.</returns>
		Public Function LoadRPMasterData(ByVal rpNr As Integer) As RPMasterData Implements IReportDatabaseAccess.LoadRPMasterData

			Dim rpMasterData As RPMasterData = Nothing

			Dim sql As String

			sql = "Select [ID]"
			sql &= ",[RPNR]"
			sql &= ",[ESNR]"
			sql &= ",[MANR]"
			sql &= ",[KDNR]"
			sql &= ",[LONr]"
			sql &= ",[Currency]"
			sql &= ",[SUVA]"
			sql &= ",[Monat]"
			sql &= ",[Jahr]"
			sql &= ",[Von]"
			sql &= ",[Bis]"
			sql &= ",[Erfasst]"
			sql &= ",[Result]"
			sql &= ",[RPKST]"
			sql &= ",[RPKST1]"
			sql &= ",[RPKST2]"
			sql &= ",[PrintedWeeks]"
			sql &= ",[PrintedDate]"
			sql &= ",[Far-pflicht]"
			sql &= ",[BVGStd]"
			sql &= ",[CreatedFrom]"
			sql &= ",[CreatedOn]"
			sql &= ",[BVGCode]"
			sql &= ",[RPGAV_FAG]"
			sql &= ",[RPGAV_FAN]"
			sql &= ",[RPGAV_WAG]"
			sql &= ",[RPGAV_WAN]"
			sql &= ",[RPGAV_VAG]"
			sql &= ",[RPGAV_VAN]"
			sql &= ",[RPGAV_Nr]"
			sql &= ",[RPGAV_Kanton]"
			sql &= ",[RPGAV_Beruf]"
			sql &= ",[RPGAV_Gruppe1]"
			sql &= ",[RPGAV_Gruppe2]"
			sql &= ",[RPGAV_Gruppe3]"
			sql &= ",[RPGAV_Text]"
			sql &= ",[RPGAV_StdWeek]"
			sql &= ",[RPGAV_StdMonth]"
			sql &= ",[RPGAV_StdYear]"
			sql &= ",[RPGAV_FAG_M]"
			sql &= ",[RPGAV_FAN_M]"
			sql &= ",[RPGAV_VAG_M]"
			sql &= ",[RPGAV_VAN_M]"
			sql &= ",[RPGAV_WAG_M]"
			sql &= ",[RPGAV_WAN_M]"
			sql &= ",[RPGAV_FAG_S]"
			sql &= ",[RPGAV_FAN_S]"
			sql &= ",[RPGAV_VAG_S]"
			sql &= ",[RPGAV_VAN_S]"
			sql &= ",[RPGAV_WAG_S]"
			sql &= ",[RPGAV_WAN_S]"
			sql &= ",[RPGAV_FAG_J]"
			sql &= ",[RPGAV_FAN_J]"
			sql &= ",[RPGAV_VAG_J]"
			sql &= ",[RPGAV_VAN_J]"
			sql &= ",[RPGAV_WAG_J]"
			sql &= ",[RPGAV_WAN_J]"
			sql &= ",[ES_Einstufung]"
			sql &= ",[KDBranche]"
			sql &= ",[ProposeNr]"
			sql &= ",[RPDoc_Guid]"
			sql &= ",[MDNr]"
			sql &= ",(SELECT COUNT(*) FROM MonthClose WHERE Monat = [RP].[Monat] AND Jahr = CAST([RP].[Jahr] as int) And MDNr = [RP].[MDNr]) AS IsMonthClosed "
			sql &= ",(SELECT TOP 1 ISNULL(ES.KDZHDNr, 0) FROM ES WHERE ES.ESNr = (SELECT TOP 1 RP.ESNr FROM RP WHERE RP.RPNr = @rpNr)) AS ResponsiblePersonNumber "
			sql &= " FROM [RP]"
			sql &= " WHERE [RPNR] = @rpNr"

			' Parameters
			Dim rpNumberParameter As New SqlClient.SqlParameter("rpNr", rpNr)
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(rpNumberParameter)

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If Not reader Is Nothing Then

					If reader.Read Then
						rpMasterData = New RPMasterData

						rpMasterData.ID = SafeGetInteger(reader, "ID", 0)
						rpMasterData.RPNR = SafeGetInteger(reader, "RPNR", Nothing)
						rpMasterData.ESNR = SafeGetInteger(reader, "ESNR", Nothing)
						rpMasterData.EmployeeNumber = SafeGetInteger(reader, "MANR", Nothing)
						rpMasterData.CustomerNumber = SafeGetInteger(reader, "KDNR", Nothing)
						rpMasterData.ResponsiblePersonNumber = SafeGetInteger(reader, "ResponsiblePersonNumber", Nothing)
						rpMasterData.LONr = SafeGetInteger(reader, "LONr", Nothing)
						rpMasterData.Currency = SafeGetString(reader, "Currency")
						rpMasterData.SUVA = SafeGetString(reader, "SUVA")
						rpMasterData.Monat = SafeGetByte(reader, "Monat", Nothing)
						rpMasterData.Jahr = SafeGetString(reader, "Jahr")
						rpMasterData.Von = SafeGetDateTime(reader, "Von", Nothing)
						rpMasterData.Bis = SafeGetDateTime(reader, "Bis", Nothing)
						rpMasterData.Erfasst = SafeGetBoolean(reader, "Erfasst", Nothing)
						rpMasterData.Result = SafeGetString(reader, "Result")
						rpMasterData.RPKST = SafeGetString(reader, "RPKST")
						rpMasterData.RPKST1 = SafeGetString(reader, "RPKST1")
						rpMasterData.RPKST2 = SafeGetString(reader, "RPKST2")
						rpMasterData.PrintedWeeks = SafeGetString(reader, "PrintedWeeks")
						rpMasterData.PrintedDate = SafeGetString(reader, "PrintedDate")
						rpMasterData.Farpflicht = SafeGetBoolean(reader, "Far-pflicht", Nothing)
						rpMasterData.BVGStd = SafeGetDecimal(reader, "BVGStd", Nothing)
						rpMasterData.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						rpMasterData.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						rpMasterData.BVGCode = SafeGetShort(reader, "BVGCode", Nothing)
						rpMasterData.RPGAV_FAG = SafeGetDecimal(reader, "RPGAV_FAG", Nothing)
						rpMasterData.RPGAV_FAN = SafeGetDecimal(reader, "RPGAV_FAN", Nothing)
						rpMasterData.RPGAV_WAG = SafeGetDecimal(reader, "RPGAV_WAG", Nothing)
						rpMasterData.RPGAV_WAN = SafeGetDecimal(reader, "RPGAV_WAN", Nothing)
						rpMasterData.RPGAV_VAG = SafeGetDecimal(reader, "RPGAV_VAG", Nothing)
						rpMasterData.RPGAV_VAN = SafeGetDecimal(reader, "RPGAV_VAN", Nothing)
						rpMasterData.RPGAV_Nr = SafeGetInteger(reader, "RPGAV_Nr", Nothing)
						rpMasterData.RPGAV_Kanton = SafeGetString(reader, "RPGAV_Kanton")
						rpMasterData.RPGAV_Beruf = SafeGetString(reader, "RPGAV_Beruf")
						rpMasterData.RPGAV_Gruppe1 = SafeGetString(reader, "RPGAV_Gruppe1")
						rpMasterData.RPGAV_Gruppe2 = SafeGetString(reader, "RPGAV_Gruppe2")
						rpMasterData.RPGAV_Gruppe3 = SafeGetString(reader, "RPGAV_Gruppe3")
						rpMasterData.RPGAV_Text = SafeGetString(reader, "RPGAV_Text")
						rpMasterData.RPGAV_StdWeek = SafeGetDecimal(reader, "RPGAV_StdWeek", Nothing)
						rpMasterData.RPGAV_StdMonth = SafeGetDecimal(reader, "RPGAV_StdMonth", Nothing)
						rpMasterData.RPGAV_StdYear = SafeGetDecimal(reader, "RPGAV_StdYear", Nothing)
						rpMasterData.RPGAV_FAG_M = SafeGetDecimal(reader, "RPGAV_FAG_M", Nothing)
						rpMasterData.RPGAV_FAN_M = SafeGetDecimal(reader, "RPGAV_FAN_M", Nothing)
						rpMasterData.RPGAV_VAG_M = SafeGetDecimal(reader, "RPGAV_VAG_M", Nothing)
						rpMasterData.RPGAV_VAN_M = SafeGetDecimal(reader, "RPGAV_VAN_M", Nothing)
						rpMasterData.RPGAV_WAG_M = SafeGetDecimal(reader, "RPGAV_WAG_M", Nothing)
						rpMasterData.RPGAV_WAN_M = SafeGetDecimal(reader, "RPGAV_WAN_M", Nothing)
						rpMasterData.RPGAV_FAG_S = SafeGetDecimal(reader, "RPGAV_FAG_S", Nothing)
						rpMasterData.RPGAV_FAN_S = SafeGetDecimal(reader, "RPGAV_FAN_S", Nothing)
						rpMasterData.RPGAV_VAG_S = SafeGetDecimal(reader, "RPGAV_VAG_S", Nothing)
						rpMasterData.RPGAV_VAN_S = SafeGetDecimal(reader, "RPGAV_VAN_S", Nothing)
						rpMasterData.RPGAV_WAG_S = SafeGetDecimal(reader, "RPGAV_WAG_S", Nothing)
						rpMasterData.RPGAV_WAN_S = SafeGetDecimal(reader, "RPGAV_WAN_S", Nothing)
						rpMasterData.RPGAV_FAG_J = SafeGetDecimal(reader, "RPGAV_FAG_J", Nothing)
						rpMasterData.RPGAV_FAN_J = SafeGetDecimal(reader, "RPGAV_FAN_J", Nothing)
						rpMasterData.RPGAV_VAG_J = SafeGetDecimal(reader, "RPGAV_VAG_J", Nothing)
						rpMasterData.RPGAV_VAN_J = SafeGetDecimal(reader, "RPGAV_VAN_J", Nothing)
						rpMasterData.RPGAV_WAG_J = SafeGetDecimal(reader, "RPGAV_WAG_J", Nothing)
						rpMasterData.RPGAV_WAN_J = SafeGetDecimal(reader, "RPGAV_WAN_J", Nothing)
						rpMasterData.ES_Einstufung = SafeGetString(reader, "ES_Einstufung")
						rpMasterData.KDBranche = SafeGetString(reader, "KDBranche")
						rpMasterData.ProposeNr = SafeGetInteger(reader, "ProposeNr", Nothing)
						rpMasterData.RPDoc_Guid = SafeGetString(reader, "RPDoc_Guid")
						rpMasterData.MDNr = SafeGetInteger(reader, "MDNr", Nothing)
						rpMasterData.IsMonthClosed = (SafeGetInteger(reader, "IsMonthClosed", 0) > 0)

					End If

				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				rpMasterData = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return rpMasterData

		End Function

		''' <summary>
		''' Loads RP absence days data.
		''' </summary>
		''' <param name="rpNr">The RP number.</param>
		''' <returns>Report absence day data or nothing in error case.</returns>
		Public Function LoadRPAbsenceDaysData(ByVal rpNr As Integer) As RPAbsenceDaysData Implements IReportDatabaseAccess.LoadRPAbsenceDaysData

			Dim result As RPAbsenceDaysData = Nothing

			Dim sql As String

			sql = "[Get Netto Absence Days in Report]"
			'sql = "SELECT"
			'sql = sql & " [RP_Fehltage].[ID]"
			'sql = sql & ",[RP_Fehltage].[RPNr]"
			'sql = sql & ",[RP_Fehltage].[Fehltag1]"
			'sql = sql & ",[RP_Fehltage].[Fehltag2]"
			'sql = sql & ",[RP_Fehltage].[Fehltag3]"
			'sql = sql & ",[RP_Fehltage].[Fehltag4]"
			'sql = sql & ",[RP_Fehltage].[Fehltag5]"
			'sql = sql & ",[RP_Fehltage].[Fehltag6]"
			'sql = sql & ",[RP_Fehltage].[Fehltag7]"
			'sql = sql & ",[RP_Fehltage].[Fehltag8]"
			'sql = sql & ",[RP_Fehltage].[Fehltag9]"
			'sql = sql & ",[RP_Fehltage].[Fehltag10]"
			'sql = sql & ",[RP_Fehltage].[Fehltag11]"
			'sql = sql & ",[RP_Fehltage].[Fehltag12]"
			'sql = sql & ",[RP_Fehltage].[Fehltag13]"
			'sql = sql & ",[RP_Fehltage].[Fehltag14]"
			'sql = sql & ",[RP_Fehltage].[Fehltag15]"
			'sql = sql & ",[RP_Fehltage].[Fehltag16]"
			'sql = sql & ",[RP_Fehltage].[Fehltag17]"
			'sql = sql & ",[RP_Fehltage].[Fehltag18]"
			'sql = sql & ",[RP_Fehltage].[Fehltag19]"
			'sql = sql & ",[RP_Fehltage].[Fehltag20]"
			'sql = sql & ",[RP_Fehltage].[Fehltag21]"
			'sql = sql & ",[RP_Fehltage].[Fehltag22]"
			'sql = sql & ",[RP_Fehltage].[Fehltag23]"
			'sql = sql & ",[RP_Fehltage].[Fehltag24]"
			'sql = sql & ",[RP_Fehltage].[Fehltag25]"
			'sql = sql & ",[RP_Fehltage].[Fehltag26]"
			'sql = sql & ",[RP_Fehltage].[Fehltag27]"
			'sql = sql & ",[RP_Fehltage].[Fehltag28]"
			'sql = sql & ",[RP_Fehltage].[Fehltag29]"
			'sql = sql & ",[RP_Fehltage].[Fehltag30]"
			'sql = sql & ",[RP_Fehltage].[Fehltag31]"
			'sql = sql & ",[RP_Fehltage].[RPNr2]"
			'sql = sql & ",Convert(int, [RP].[Monat]) Monat"
			'sql = sql & ",Convert(int, [RP].[Jahr]) Jahr"
			'sql = sql & " FROM [RP_Fehltage]"
			'sql = sql & " INNER JOIN [RP] ON [RP_Fehltage].RPNr = [RP].RPNr"
			'sql = sql & " WHERE [RP_Fehltage].[RPNr] = @rpNr"


			' Parameters
			Dim rpNrParameter As New SqlClient.SqlParameter("rpNr", rpNr)
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(rpNrParameter)

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			'Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If Not reader Is Nothing Then

					If reader.Read Then
						result = New RPAbsenceDaysData

						result.ID = SafeGetInteger(reader, "ID", 0)
						result.RPNr = SafeGetInteger(reader, "RPNr", Nothing)
						result.RPMonat = SafeGetInteger(reader, "Monat", Nothing)
						result.RPJahr = SafeGetInteger(reader, "Jahr", Nothing)

						result.Fehltag1 = SafeGetString(reader, "Fehltag1")
						result.Fehltag2 = SafeGetString(reader, "Fehltag2")
						result.Fehltag3 = SafeGetString(reader, "Fehltag3")
						result.Fehltag4 = SafeGetString(reader, "Fehltag4")
						result.Fehltag5 = SafeGetString(reader, "Fehltag5")
						result.Fehltag6 = SafeGetString(reader, "Fehltag6")
						result.Fehltag7 = SafeGetString(reader, "Fehltag7")
						result.Fehltag8 = SafeGetString(reader, "Fehltag8")
						result.Fehltag9 = SafeGetString(reader, "Fehltag9")
						result.Fehltag10 = SafeGetString(reader, "Fehltag10")
						result.Fehltag11 = SafeGetString(reader, "Fehltag11")
						result.Fehltag12 = SafeGetString(reader, "Fehltag12")
						result.Fehltag13 = SafeGetString(reader, "Fehltag13")
						result.Fehltag14 = SafeGetString(reader, "Fehltag14")
						result.Fehltag15 = SafeGetString(reader, "Fehltag15")
						result.Fehltag16 = SafeGetString(reader, "Fehltag16")
						result.Fehltag17 = SafeGetString(reader, "Fehltag17")
						result.Fehltag18 = SafeGetString(reader, "Fehltag18")
						result.Fehltag19 = SafeGetString(reader, "Fehltag19")
						result.Fehltag20 = SafeGetString(reader, "Fehltag20")
						result.Fehltag21 = SafeGetString(reader, "Fehltag21")
						result.Fehltag22 = SafeGetString(reader, "Fehltag22")
						result.Fehltag23 = SafeGetString(reader, "Fehltag23")
						result.Fehltag24 = SafeGetString(reader, "Fehltag24")
						result.Fehltag25 = SafeGetString(reader, "Fehltag25")
						result.Fehltag26 = SafeGetString(reader, "Fehltag26")
						result.Fehltag27 = SafeGetString(reader, "Fehltag27")
						result.Fehltag28 = SafeGetString(reader, "Fehltag28")
						result.Fehltag29 = SafeGetString(reader, "Fehltag29")
						result.Fehltag30 = SafeGetString(reader, "Fehltag30")
						result.Fehltag31 = SafeGetString(reader, "Fehltag31")
						result.RPNr2 = SafeGetInteger(reader, "RPNr2", Nothing)

					End If

				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("Test: {0}{1}", vbNewLine, ex.ToString()))
				result = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		''' <summary>
		''' Loads RP overview list data.
		''' </summary>
		''' <returns>List of RP overview data or nothing in error case.</returns>
		Public Function LoadRPOverviewListData() As IEnumerable(Of RPOverviewData) Implements IReportDatabaseAccess.LoadRPOverviewListData

			Dim result As List(Of RPOverviewData) = Nothing

			Dim sql As String

			sql = "SELECT RP.ID, RP.RPNR, RP.ESNR, RP.Von, RP.Bis, Convert(int, RP.Monat) Monat, Convert(Int, RP.Jahr) Jahr, RP.Erfasst, RP.CreatedOn, RP.CreatedFrom, "
			sql &= "MA.Nachname ,MA.Vorname, KD.Firma1 "
			sql &= "FROM RP LEFT JOIN Mitarbeiter MA ON RP.MANr = MA.MANr "
			sql &= "LEFT JOIN Kunden KD ON RP.KDNr = KD.KDNr "
			sql &= "Where RP.ESNr Is Not Null "
			sql &= "ORDER BY RP.Jahr DESC, RP.Monat DESC, MA.Nachname ASC, MA.Vorname ASC"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of RPOverviewData)

					While reader.Read

						Dim rpOverviewData = New RPOverviewData()
						rpOverviewData.ID = SafeGetInteger(reader, "ID", 0)
						rpOverviewData.RPNr = SafeGetInteger(reader, "RPNr", 0)
						rpOverviewData.ESNr = SafeGetInteger(reader, "ESNr", 0)
						rpOverviewData.ReportFrom = SafeGetDateTime(reader, "Von", Nothing)
						rpOverviewData.ReportTo = SafeGetDateTime(reader, "Bis", Nothing)
						rpOverviewData.ReportMonth = SafeGetInteger(reader, "Monat", 0)
						rpOverviewData.ReportYear = SafeGetInteger(reader, "Jahr", 0)
						rpOverviewData.Erfasst = SafeGetBoolean(reader, "Erfasst", Nothing)
						rpOverviewData.EmployeeLastname = SafeGetString(reader, "Nachname")
						rpOverviewData.EmployeeFirstname = SafeGetString(reader, "Vorname")
						rpOverviewData.Customer1 = SafeGetString(reader, "Firma1")

						rpOverviewData.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						rpOverviewData.CreatedFrom = SafeGetString(reader, "CreatedFrom")

						result.Add(rpOverviewData)

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
		''' Loads report detail data.
		''' </summary>
		''' <param name="rpNr">The report number.</param>
		''' <returns>The report detail data or nothing in error case.</returns>
		Public Function LoadRPDetailData(ByVal rpNr As Integer) As RPDetailData Implements IReportDatabaseAccess.LoadRPDetailData

			Dim result As RPDetailData = Nothing

			Dim sql As String

			sql = "SELECT RP.Monat, RP.Jahr, RP.ESNr, RP.MANr, RP.KDNr, RP.Von, RP.Bis, RP.LONr, MA.Kinder, ES.ES_Als, ES.ES_Ab, ES.ES_Ende "
			sql &= ",(SELECT TOP 1 ISNULL(ES.KDZHDNr, 0) FROM ES WHERE ES.ESNr = (SELECT TOP 1 RP.ESNr FROM RP WHERE RP.RPNr = @rpNr)) AS ResponsiblePersonNumber "
			sql &= "FROM RP "
			sql &= "LEFT JOIN Mitarbeiter MA ON RP.MANr = MA.MANr "
			sql &= "LEFT JOIN ES ON RP.ESNr = ES.ESNr "
			sql &= "WHERE RP.RPNr = @rpNr"

			' Parameters
			Dim rpNrParameter As New SqlClient.SqlParameter("rpNr", rpNr)
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(rpNrParameter)

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If Not reader Is Nothing Then

					If reader.Read Then
						result = New RPDetailData

						result.Month = SafeGetByte(reader, "Monat", Nothing)
						result.Year = SafeGetString(reader, "Jahr")
						result.EmployeeNumber = SafeGetInteger(reader, "MANr", 0)
						result.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)
						result.ResponsiblePersonNumber = SafeGetInteger(reader, "ResponsiblePersonNumber", 0)
						result.ESNr = SafeGetInteger(reader, "ESNr", 0)
						result.RPFromDate = SafeGetDateTime(reader, "Von", Nothing)
						result.RPToDate = SafeGetDateTime(reader, "Bis", Nothing)
						result.LONr = SafeGetInteger(reader, "LONr", Nothing)
						result.ChildsCount = SafeGetShort(reader, "Kinder", 0)
						result.ESAls = SafeGetString(reader, "ES_Als")
						result.ESFromDate = SafeGetDateTime(reader, "ES_Ab", Nothing)
						result.ESToDate = SafeGetDateTime(reader, "ES_Ende", Nothing)

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
		''' looks for duplicate LANr in RPL for same time
		''' </summary>
		''' <param name="rpNr"></param>
		''' <param name="lanr"></param>
		''' <param name="vonDate"></param>
		''' <param name="bisDate"></param>
		''' <param name="isKD"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function ExistsRPLLADataForPeriode(ByVal rpNr As Integer, ByVal lanr As Decimal, ByVal vonDate As DateTime, ByVal bisDate As DateTime,
																							ByVal isKD As Boolean?) As Boolean Implements IReportDatabaseAccess.ExistsRPLLADataForPeriode
			Dim result As Boolean = False
			Dim sql As String = String.Empty

			sql &= "SELECT Top 1 RPL.RPNR, RPL.RPLNr, RPL.KSTNr, RPL.LANR, RPL.ESLohnNr, RPL.RENr, RPL.RPZusatzText, "
			sql &= "RPL.M_Anzahl, RPL.M_Basis, RPL.M_Ansatz, RPL.M_Betrag, "
			sql &= "RPL.K_Anzahl, RPL.K_Basis, RPL.K_Ansatz, RPL.K_Betrag, "
			sql &= "RPL.MWST, RPL.VonDate, RPL.BisDate, RPL.ChangedOn, RPL.ChangedFrom "

			sql &= "FROM RPL "

			sql &= "WHERE RPL.rpNr = @rpNr And RPL.ShowinList = 1 And RPL.KD = @isKD "
			sql &= "And RPL.VonDate = @VonDate And RPL.BisDate = @BisDate And RPL.LANr = @LANr "
			sql &= "ORDER BY RPL.RPLNr DESC "

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			' Data
			listOfParams.Add(New SqlClient.SqlParameter("@RPNr", ReplaceMissing(rpNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@isKD", ReplaceMissing(isKD, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("@VonDate", ReplaceMissing(vonDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@BisDate", ReplaceMissing(bisDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LANr", ReplaceMissing(lanr, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)
			Try

				If (Not reader Is Nothing) Then

					result = reader.HasRows

				End If

			Catch e As Exception
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function


		''' <summary>
		''' Loads RPL list data.
		''' </summary>
		''' <param name="rpNr">Thre report number.</param>
		''' <param name="lang">The language.</param>
		''' <param name="rplDataType">The RPL data type.</param>
		''' <param name="esLohnNr">Optional esLohnNr.</param>
		''' <returns>List of RPL list data or nothing in error case.</returns>
		Public Function LoadRPLListData(ByVal rpNr As Integer, ByVal lang As String, ByVal rplDataType As RPLType, Optional ByVal esLohnNr As Integer? = Nothing) As IEnumerable(Of RPLListData) Implements IReportDatabaseAccess.LoadRPLListData

			Dim result As List(Of RPLListData) = Nothing

			Dim translatedLAColumn As String = String.Empty
			Dim m_DateUtility As New DateAndTimeUtily

			Select Case rplDataType

				Case RPLType.Employee

					Select Case lang.ToLower()
						Case "deutsch"
							translatedLAColumn = "LALoText"
						Case "französisch"
							translatedLAColumn = "ISNull(Name_LO_F, LALoText)"
						Case "italienisch"
							translatedLAColumn = "IsNull(Name_LO_I, LALoText)"
						Case "englisch"
							translatedLAColumn = "IsNull(Name_LO_E, LALoText)"
						Case Else
							translatedLAColumn = "LALoText"
					End Select

				Case RPLType.Customer
					Select Case lang.ToLower()
						Case "deutsch"
							translatedLAColumn = "LAOpText"
						Case "französisch"
							translatedLAColumn = "IsNull(Name_OP_F, LAOpText)"
						Case "italienisch"
							translatedLAColumn = "IsNull(Name_OP_I, LAOpText)"
						Case "englisch"
							translatedLAColumn = "IsNull(Name_OP_E, LAOpText)"
						Case Else
							translatedLAColumn = "LAOpText"
					End Select
				Case Else
					m_Logger.LogError(String.Format("Invalid RPLDataType: {0}", rplDataType.ToString()))
					Return Nothing
			End Select

			Dim sql As String = String.Empty

			sql = sql & "SELECT RPL.ID, RPL.RPNR, RPL.RPLNr, RPL.KSTNr, RPL.LANR, RPL.ESLohnNr, RPL.RENr, RPL.RPZusatzText, "
			sql = sql & "RPL.M_Anzahl, RPL.M_Basis, RPL.M_Ansatz, RPL.M_Betrag, "
			sql = sql & "RPL.K_Anzahl, RPL.K_Basis, RPL.K_Ansatz, RPL.K_Betrag, "
			sql = sql & "RPL.MWST, RPL.VonDate, RPL.BisDate, RPL.ChangedOn, RPL.ChangedFrom, "
			sql = sql & "(SELECT COUNT(ID) FROM RP_ScanDoc WHERE RPNR = @rpNr And RPLNr =  RPL.RPLNr) As HasDocument, "
			sql = sql & String.Format("{0} as TranslatedLAColumn, ", translatedLAColumn)
			sql = sql & "LA.Vorzeichen "
			sql = sql & ",ISNULL( (SELECT TOP 1 KST.Bezeichnung KSTBez FROM KD_KST KST WHERE KST.KDNR = RPL.KDNr And KST.RecNr = RPL.KSTNr), '') AS KSTBez "
			sql = sql & "FROM RPL "
			sql = sql & "LEFT JOIN LA_Translated ON RPL.LANR = LA_Translated.LANR "
			sql = sql & "LEFT JOIN LA ON RPL.LANR = LA.LANr AND Year(RPL.VonDate) = LA.LAJahr AND LA.LADeactivated = 0 "

			sql = sql & "WHERE RPL.rpNr = @rpNr And RPL.ShowinList = 1 AND RPL.KD = @isKD AND (@esLohnNr IS NULL OR RPL.ESLohnnr = @esLohnNr) And RPL.VonDate Is Not Null "
			sql = sql & "ORDER BY RPL.VonDate DESC, RPL.LANr ASC "

			'sql = sql & "ORDER BY RPL.ID DESC "

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("rpNr", rpNr))
			listOfParams.Add(New SqlClient.SqlParameter("isKD", (rplDataType.Equals(RPLType.Customer))))
			listOfParams.Add(New SqlClient.SqlParameter("esLohnNr", ReplaceMissing(esLohnNr, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of RPLListData)

					While reader.Read

						Dim rplListData = New RPLListData()
						rplListData.ID = SafeGetInteger(reader, "ID", 0)
						rplListData.RPNr = SafeGetInteger(reader, "RPNR", Nothing)
						rplListData.RPLNr = SafeGetInteger(reader, "RPLNr", Nothing)
						rplListData.KSTNr = SafeGetInteger(reader, "KSTNr", Nothing)
						rplListData.kstname = SafeGetString(reader, "KSTBez")
						rplListData.LANr = SafeGetDecimal(reader, "LANr", Nothing)
						rplListData.ESLohnNr = SafeGetInteger(reader, "ESLohnNr", Nothing)
						rplListData.RENr = SafeGetInteger(reader, "RENr", Nothing)
						rplListData.RPZusatzText = SafeGetString(reader, "RPZusatzText")

						Select Case rplDataType
							Case RPLType.Employee
								rplListData.Anzahl = SafeGetDecimal(reader, "M_Anzahl", Nothing)
								rplListData.Basis = SafeGetDecimal(reader, "M_Basis", Nothing)
								rplListData.Ansatz = SafeGetDecimal(reader, "M_Ansatz", Nothing)
								rplListData.Betrag = SafeGetDecimal(reader, "M_Betrag", Nothing)
							Case RPLType.Customer
								rplListData.Anzahl = SafeGetDecimal(reader, "K_Anzahl", Nothing)
								rplListData.Basis = SafeGetDecimal(reader, "K_Basis", Nothing)
								rplListData.Ansatz = SafeGetDecimal(reader, "K_Ansatz", Nothing)
								rplListData.Betrag = SafeGetDecimal(reader, "K_Betrag", Nothing)
							Case Else
								' Do nothing
						End Select

						rplListData.MWST = SafeGetDecimal(reader, "MWST", Nothing)
						rplListData.VonDate = SafeGetDateTime(reader, "VonDate", Nothing)
						rplListData.BisDate = SafeGetDateTime(reader, "BisDate", Nothing)
						rplListData.rpltime = String.Format("{0:d} - {1:d}", rplListData.VonDate, rplListData.BisDate)


						Dim kwdata As Integer() = m_DateUtility.GetCalendarWeeksBetweenDates(rplListData.VonDate, rplListData.BisDate)
						rplListData.rplkwvon = Format(kwdata(0), "n0")
						rplListData.rplkwbis = Format(kwdata(kwdata.Length - 1), "n0")

						rplListData.rplkw = String.Format("{0}{1}{2}", rplListData.rplkwvon, If(rplListData.rplkwvon = rplListData.rplkwbis, "", " - "),
																							If(rplListData.rplkwvon = rplListData.rplkwbis, "", rplListData.rplkwbis))

						rplListData.Sign = SafeGetString(reader, "Vorzeichen", String.Empty)
						rplListData.TranslatedLAText = SafeGetString(reader, "TranslatedLAColumn")
						rplListData.HasDocument = (SafeGetInteger(reader, "HasDocument", 0) > 0)
						rplListData.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
						rplListData.ChangedFrom = SafeGetString(reader, "ChangedFrom", Nothing)
						rplListData.Type = rplDataType

						result.Add(rplListData)

					End While

				End If

			Catch e As Exception
				m_Logger.LogError(String.Format("Result: {1}{0}{2}", vbNewLine, result.Count, e.ToString()))
				result = Nothing

			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		''' <summary>
		''' Loads LA list data.
		''' </summary>
		''' <param name="year">The year.</param>
		''' <param name="usLanguage">The user language.</param>
		''' <param name="rplType">The RPL type.</param>
		''' <param name="laNr">Optional LANr to select.</param>
		''' <returns>List of la data or nothing in error case.</returns>
		Public Function LoadLAListData(ByVal year As Integer, ByVal usLanguage As String, ByVal rplType As RPLType, Optional laNr As Decimal? = Nothing) As IEnumerable(Of LAData) Implements IReportDatabaseAccess.LoadLAListData

			Dim result As List(Of LAData) = Nothing

			Dim sql As String

			sql = "[Get LAData For RP]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MDYear ", year))
			listOfParams.Add(New SqlClient.SqlParameter("@USLanguage ", usLanguage))

			If Not laNr Is Nothing Then
				listOfParams.Add(New SqlClient.SqlParameter("@LANrToSelect ", laNr))
			End If

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of LAData)

					While reader.Read()
						Dim laData As New LAData
						laData.LANr = SafeGetDecimal(reader, "LANr", Nothing)
						laData.LALoText = SafeGetString(reader, "LALoText")
						laData.LAOPText = SafeGetString(reader, "LAOPText")
						laData.Sign = SafeGetString(reader, "Vorzeichen")
						laData.AllowMoreAnzahl = SafeGetBoolean(reader, "AllowedMore_Anz", False)
						laData.AllowMoreBasis = SafeGetBoolean(reader, "AllowedMore_Bas", False)
						laData.AllowMoreAnsatz = SafeGetBoolean(reader, "AllowedMore_Ans", False)
						laData.AllowMoreBetrag = SafeGetBoolean(reader, "AllowedMore_Btr", False)
						laData.Rounding = SafeGetShort(reader, "Rundung", Nothing)
						laData.TypeAnzahl = SafeGetShort(reader, "TypeAnzahl", Nothing)
						laData.TypeBasis = SafeGetShort(reader, "TypeBasis", Nothing)
						laData.TypeAnsatz = SafeGetShort(reader, "TypeAnsatz", Nothing)

						laData.MABasVar = SafeGetString(reader, "MABasVar")
						laData.FixBasis = SafeGetDecimal(reader, "FixBasis", Nothing)
						laData.MAAnsVar = SafeGetString(reader, "MAAnsVar")
						laData.MAAnzVar = SafeGetString(reader, "MAAnzVar")
						laData.KDAnzahl = SafeGetString(reader, "KDAnzahl")
						laData.KDBasis = SafeGetString(reader, "KDBasis")
						laData.KDAnsatz = SafeGetString(reader, "KDAnsatz")
						laData.FeierInklusiv = SafeGetBoolean(reader, "FeierInklusiv", Nothing)
						laData.FerienInklusiv = SafeGetBoolean(reader, "FerienInklusiv", Nothing)
						laData.Inklusiv13 = SafeGetBoolean(reader, "13Inklusiv", Nothing)
						laData.FixAnsatz = SafeGetDecimal(reader, "FixAnsatz", Nothing)
						laData.DuppinKD = SafeGetBoolean(reader, "DuppinKD", Nothing)
						laData.TagesSpesen = SafeGetBoolean(reader, "TagesSpesen", Nothing)
						laData.StdSpesen = SafeGetBoolean(reader, "StdSpesen", Nothing)
						laData.ProTag = SafeGetBoolean(reader, "ProTag", Nothing)
						laData.GleitTime = SafeGetBoolean(reader, "GleitTime", Nothing)
						laData.MoreProz4Fer = SafeGetBoolean(reader, "MoreProz4Fer", Nothing)
						laData.MoreProz4Feier = SafeGetBoolean(reader, "MoreProz4Feier", Nothing)
						laData.MoreProz413 = SafeGetBoolean(reader, "MoreProz413", Nothing)

						laData.MoreProz4FerAmount = SafeGetDecimal(reader, "MoreProz4FerAmount", 0)
						laData.MoreProz4FeierAmount = SafeGetDecimal(reader, "MoreProz4FeierAmount", 0)
						laData.MoreProz413Amount = SafeGetDecimal(reader, "MoreProz413Amount", 0)

						laData.MWSTPflichtig = SafeGetBoolean(reader, "MWSTPflichtig", Nothing)

						laData.RPLTypeForTranslation = rplType

						result.Add(laData)

					End While

				End If

			Catch e As Exception
				m_Logger.LogError(String.Format("Result: {1}{0}{2}", vbNewLine, result.Count, e.ToString()))
				result = Nothing

			Finally
				CloseReader(reader)

			End Try

			Return result

		End Function

		''' <summary>
		''' Loads ES salary data.
		''' </summary>
		''' <param name="esNumber">The es nr.</param>
		''' <returns>List of ES salary data.</returns>
		Public Function LoadESSalaryData(ByVal esNumber As Integer) As IEnumerable(Of ESSalaryData) Implements IReportDatabaseAccess.LoadESSalaryData

			Dim result As List(Of ESSalaryData) = Nothing

			Dim sql As String

			sql = "Select ESL.[ID]"
			sql &= ",ESL.[ESLohnNr]"
			sql &= ",ESL.[KSTNr]"
			sql &= ",ESL.[GrundLohn]"
			sql &= ",ESL.[StundenLohn]"
			sql &= ",ESL.[Tarif]"
			sql &= ",ESL.[MWSTBetrag]"
			sql &= ",ESL.[LOVon]"
			sql &= ",dbo.[GetEndDateOfESLohn](ESL.ESNr, ESL.ESLohnNr) LOBis"
			sql &= ",ESL.[AktivLODaten]"
			sql &= ",ESL.[FeierProz]"
			sql &= ",ESL.[FerienProz]"
			sql &= ",ESL.[Lohn13Proz]"
			sql &= ",ESL.[GAVText]"
			sql &= ",ESL.[GAVNr]"
			sql &= ",ESL.[LOFeiertagWay]"
			sql &= ",ESL.[FerienWay]"
			sql &= ",ESL.[LO13Way]"
			sql &= ",ESL.[MAStdSpesen]"
			sql &= ",ESL.[MATSpesen]"
			sql &= ",ESL.[KDTSpesen]"
			sql &= ",ESL.[IsPVL]"
			sql &= ",ESL.[Createdon]"
			sql &= ",IsNull(KD.MWSt, 0) MWSt"
			sql &= ",ESL.GAVInfo_String"
			sql &= " FROM [dbo].[ESLohn] ESL "
			sql &= " Left Join [dbo].[Kunden] KD On KD.KDNr = ESL.KDNr "
			sql &= "WHERE ESL.ESNr = @esNr"
			sql &= " ORDER BY ESL.[LOVon] DESC, ESL.[ESLohnNr] DESC"

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
						esSalaryData.KSTNr = SafeGetInteger(reader, "KSTNr", Nothing)
						esSalaryData.GrundLohn = SafeGetDecimal(reader, "GrundLohn", Nothing)
						esSalaryData.StundenLohn = SafeGetDecimal(reader, "StundenLohn", Nothing)
						esSalaryData.Tarif = SafeGetDecimal(reader, "Tarif", Nothing)
						esSalaryData.MWStBetrag = SafeGetDecimal(reader, "MWStBetrag", Nothing)
						esSalaryData.LOVon = SafeGetDateTime(reader, "LOVon", Nothing)
						esSalaryData.LOBis = SafeGetDateTime(reader, "LOBis", Nothing)
						esSalaryData.AktivLODaten = SafeGetBoolean(reader, "AktivLODaten", Nothing)
						esSalaryData.FeierProz = SafeGetDecimal(reader, "FeierProz", Nothing)
						esSalaryData.FerienProz = SafeGetDecimal(reader, "FerienProz", Nothing)
						esSalaryData.Lohn13Proz = SafeGetDecimal(reader, "Lohn13Proz", Nothing)
						esSalaryData.GAVText = SafeGetString(reader, "GAVText")
						esSalaryData.GAVNr = SafeGetInteger(reader, "GAVNr", Nothing)
						esSalaryData.LOFeiertagWay = SafeGetByte(reader, "LOFeiertagWay", Nothing)
						esSalaryData.FerienWay = SafeGetShort(reader, "FerienWay", Nothing)
						esSalaryData.LO13Way = SafeGetShort(reader, "LO13Way", Nothing)
						esSalaryData.MAStdSpesen = SafeGetDecimal(reader, "MAStdSpesen", Nothing)
						esSalaryData.MATSpesen = SafeGetDecimal(reader, "MATSpesen", Nothing)
						esSalaryData.KDTSpesen = SafeGetDecimal(reader, "KDTSpesen", Nothing)
						esSalaryData.IsPVL = SafeGetByte(reader, "IsPVL", Nothing)
						esSalaryData.Createdon = SafeGetDateTime(reader, "CreatedOn", Nothing)
						esSalaryData.CustomerMwStPflicht = SafeGetBoolean(reader, "MWSt", False)
						esSalaryData.GAVInfo_String = SafeGetString(reader, "GAVInfo_String")

						result.Add(esSalaryData)

					End While

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
		''' Loads ES data for createing reports.
		''' </summary>
		''' <param name="mandantNumber">The mandant nr.</param>
		''' <returns>List of ES data.</returns>
		Public Function LoadESDataForCreateingReport(ByVal mandantNumber As Integer, ByVal jahr As Integer, ByVal monat As Integer) As IEnumerable(Of ESData) Implements IReportDatabaseAccess.LoadESDataForCreateingReport

			Dim result As List(Of ESData) = Nothing

			Dim sql As String

			sql = "Select ES.[ID]"
			sql &= ",ES.[ESNr]"
			sql &= ",ES.[MANr]"
			sql &= ",ES.[KDNr]"
			sql &= ",ES.[ES_Ab]"
			sql &= ",ES.[ES_Ende]"
			sql &= ",ES.[Suva]"
			sql &= ",ES.[ESKST1]"
			sql &= ",ES.[ESKST2]"
			sql &= ",ES.[ESKST]"
			sql &= ",ES.[ESBranche]"

			sql &= ",ESL.[GrundLohn]"
			sql &= ",ESL.[StundenLohn]"
			sql &= ",ESL.[Tarif]"
			sql &= ",ESL.[MWSTBetrag]"
			sql &= ",ESL.[LOVon]"
			sql &= ",dbo.[GetEndDateOfESLohn](ES.ESNr, ESL.ESLohnNr) LOBis"
			sql &= ",ESL.[AktivLODaten]"
			sql &= ",ESL.[FeierProz]"
			sql &= ",ESL.[FerienProz]"
			sql &= ",ESL.[Lohn13Proz]"
			sql &= ",ESL.[GAVText]"
			sql &= ",ESL.[GAVNr]"
			sql &= ",ESL.[LOFeiertagWay]"
			sql &= ",ESL.[FerienWay]"
			sql &= ",ESL.[LO13Way]"
			sql &= ",ESL.[MAStdSpesen]"
			sql &= ",ESL.[MATSpesen]"
			sql &= ",ESL.[KDTSpesen]"
			sql &= ",ESL.[IsPVL]"
			sql &= ",ES.[Createdon]"

			sql &= " FROM [dbo].[ES]"
			sql &= " Left Join ESLohn ESL On ES.ESNr = ESL.ESNr"

			sql &= " WHERE"
			sql &= " ES.MDNr = @MDNr"
			sql &= " And ES.ES_Ab <= dbo.[lastDayInMonth](dbo.[EndOfMonth] (@jahr, @monat))"
			sql &= " And (ES.ES_Ende >= dbo.[FirstDayInMonth](dbo.[EndOfMonth] (@jahr, @monat)) Or ES.ES_Ende Is Null)"

			sql &= " And ESL.AktivLODaten = 1"
			sql &= " And (ISNULL(ESL.MATotal, 0) + ISNULL(ESL.KDTotal, 0)) > 0"
			sql &= " And ES.ESNr Not In (Select ESNr From RP Where MDNr = @MDNr And Monat = @monat And Jahr = @jahr)"

			sql &= " ORDER BY ES.ESNr"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mandantNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("jahr", ReplaceMissing(jahr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("monat", ReplaceMissing(monat, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ESData)

					While reader.Read

						Dim esData As New ESData

						esData.ID = SafeGetInteger(reader, "ID", 0)
						esData.ESNr = SafeGetInteger(reader, "ESNr", Nothing)
						esData.MANr = SafeGetInteger(reader, "MANr", Nothing)
						esData.KDNr = SafeGetInteger(reader, "KDNr", Nothing)

						esData.esAb = SafeGetDateTime(reader, "ES_Ab", Nothing)
						esData.esEnde = SafeGetDateTime(reader, "ES_Ende", Nothing)

						esData.eskst1 = SafeGetString(reader, "ESKst1")
						esData.eskst2 = SafeGetString(reader, "ESKst2")
						esData.eskst = SafeGetString(reader, "ESKst")
						esData.suva = SafeGetString(reader, "suva")
						esData.esbranche = SafeGetString(reader, "ESBranche")

						result.Add(esData)

					End While

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
		''' Loads created RP data from es data.
		''' </summary>
		''' <param name="mandantNumber">The mandant nr.</param>
		''' <returns>List of ES data.</returns>
		Public Function LoadCreatedRPOverviewListData(ByVal mandantNumber As Integer, ByVal rpnumbers As Integer()) As IEnumerable(Of RPOverviewData) Implements IReportDatabaseAccess.LoadCreatedRPOverviewListData

			Dim result As List(Of RPOverviewData) = Nothing

			Dim sql As String
			Dim rpNumbersBuffer As String = String.Empty

			For Each number In rpnumbers

				rpNumbersBuffer &= IIf(rpNumbersBuffer <> "", ", ", "") & number

			Next

			sql = "SELECT RP.ID"
			sql &= ",RP.RPNR"
			sql &= ",RP.MANR"
			sql &= ",RP.KDNR"
			sql &= ",RP.ESNR"
			sql &= ",RP.Von"
			sql &= ",RP.Bis"
			sql &= ",Convert(Int, RP.Monat) Monat"
			sql &= ",Convert(Int, RP.Jahr) Jahr"
			sql &= ",RP.Erfasst"
			sql &= ",RP.CreatedOn"
			sql &= ",RP.CreatedFrom"
			sql &= ",MA.Nachname"
			sql &= ",MA.Vorname"
			sql &= ",KD.Firma1"

			sql &= " FROM RP"
			sql &= " LEFT JOIN Mitarbeiter MA ON RP.MANr = MA.MANr"
			sql &= " LEFT JOIN Kunden KD ON RP.KDNr = KD.KDNr"

			sql &= " Where RP.MDNr = @mandantNumber"
			sql &= String.Format(" And RP.RPNr In ({0})", rpNumbersBuffer)

			sql &= " ORDER BY RP.Jahr DESC, RP.Monat DESC, MA.Nachname ASC, MA.Vorname ASC"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mandantNumber", ReplaceMissing(mandantNumber, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of RPOverviewData)

					While reader.Read

						Dim rpOverviewData = New RPOverviewData()
						rpOverviewData.ID = SafeGetInteger(reader, "ID", 0)
						rpOverviewData.RPNr = SafeGetInteger(reader, "RPNr", 0)
						rpOverviewData.MANr = SafeGetInteger(reader, "MANr", 0)
						rpOverviewData.KDNr = SafeGetInteger(reader, "KDNr", 0)
						rpOverviewData.ESNr = SafeGetInteger(reader, "ESNr", 0)
						rpOverviewData.ReportFrom = SafeGetDateTime(reader, "Von", Nothing)
						rpOverviewData.ReportTo = SafeGetDateTime(reader, "Bis", Nothing)
						rpOverviewData.ReportMonth = SafeGetInteger(reader, "Monat", 0)
						rpOverviewData.ReportYear = SafeGetInteger(reader, "Jahr", 0)
						rpOverviewData.Erfasst = SafeGetBoolean(reader, "Erfasst", Nothing)
						rpOverviewData.EmployeeLastname = SafeGetString(reader, "Nachname")
						rpOverviewData.EmployeeFirstname = SafeGetString(reader, "Vorname")
						rpOverviewData.Customer1 = SafeGetString(reader, "Firma1")

						rpOverviewData.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						rpOverviewData.CreatedFrom = SafeGetString(reader, "CreatedFrom")

						result.Add(rpOverviewData)

					End While

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
		''' Loads additonal fee list of an employee RPL.
		''' </summary>
		''' <param name="rpNr">The RPNr.</param>
		''' <param name="rplNr">The RPLNr.</param>
		''' <returns>List of additional RPL fees or nothing in error case.</returns>
		Function LoadAdditionalFeeListOfEmployeeRPL(ByVal rpNr As Integer, ByVal rplNr As Integer) As IEnumerable(Of RPLAdditionalFee) Implements IReportDatabaseAccess.LoadAdditionalFeeListOfEmployeeRPL


			Dim result As List(Of RPLAdditionalFee) = Nothing

			Dim sql As String

			sql = "[Get Zuschleage For RPLLine]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@RPNr ", rpNr))
			listOfParams.Add(New SqlClient.SqlParameter("@RPLNr ", rplNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of RPLAdditionalFee)

					While reader.Read()
						Dim rplAdditionalFeeData As New RPLAdditionalFee
						rplAdditionalFeeData.LANr = SafeGetDecimal(reader, "LANr", Nothing)
						rplAdditionalFeeData.M_Ansatz = SafeGetDecimal(reader, "M_Ansatz", Nothing)
						rplAdditionalFeeData.M_Basis = SafeGetDecimal(reader, "M_Basis", Nothing)
						rplAdditionalFeeData.M_Betrag = SafeGetDecimal(reader, "M_Betrag", Nothing)

						result.Add(rplAdditionalFeeData)

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
		''' Loads flexibletime list of a RPL.
		''' </summary>
		''' <param name="rpNr">The RPNr.</param>
		''' <param name="rplNr">The RPLNr.</param>
		''' <returns>List of RPL felxible time  or nothing in error case.</returns>
		Function LoadFlexibleTimeListOfRPL(ByVal rpNr As Integer, ByVal rplNr As Integer) As IEnumerable(Of RPLFlexibleTimeData) Implements IReportDatabaseAccess.LoadFlexibleTimeListOfRPL

			Dim result As List(Of RPLFlexibleTimeData) = Nothing

			Dim sql As String

			sql = "[Get Gleitzeit For RPLLine]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@RPNr ", rpNr))
			listOfParams.Add(New SqlClient.SqlParameter("@RPLNr ", rplNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of RPLFlexibleTimeData)

					While reader.Read()
						Dim laData As New RPLFlexibleTimeData
						laData.KompBetrag = SafeGetDecimal(reader, "KompBetrag", Nothing)
						laData.KompStd = SafeGetDecimal(reader, "KompStd", Nothing)

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
		''' Loads mandant TSP LMV Spesen hour value.
		''' </summary>
		''' <param name="gavNumber">The GAV number.</param>
		''' <returns>Spesen value.</returns>
		Public Function LoadMandantTSPLMVSpesenHourValue(ByVal gavNumber As Integer, ByVal year As Integer) As Decimal? Implements IReportDatabaseAccess.LoadMandantTSPLMVSpesenHourValue
			Dim spesenValueHours As Decimal? = Nothing

			Dim sql As String = "SELECT TOP 1 TSpesen FROM MD_TSP_LMV WHERE GAVNumber = @gavNumber AND MDYear = @year ORDER BY ID DESC"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@gavNumber", gavNumber))
			listOfParams.Add(New SqlClient.SqlParameter("@year", Convert.ToString(year)))

			Dim result As Object = ExecuteScalar(sql, listOfParams)

			If result Is Nothing OrElse IsDBNull(result) Then
				Return Nothing
			Else
				Return result
			End If

		End Function

		''' <summary>
		''' Loads mandant TSP LMV working hours per week.
		''' </summary>
		''' <param name="gavNumber">The GAV number.</param>
		''' <param name="year">The year.</param>
		''' <returns>The working hours per week.</returns>
		Public Function LoadManantTSPLMVWorkingHoursPerWeek(ByVal gavNumber As Integer, ByVal year As Integer) As Decimal? Implements IReportDatabaseAccess.LoadManantTSPLMVWorkingHoursPerWeek
			Dim workinghours As Decimal? = Nothing

			Dim sql As String = "SELECT TOP 1 TWochenstunden FROM MD_TSP_LMV WHERE GAVNumber = @gavNumber AND MDYear = @year ORDER BY ID DESC"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@gavNumber", gavNumber))
			listOfParams.Add(New SqlClient.SqlParameter("@year", Convert.ToString(year)))

			Dim result As Object = ExecuteScalar(sql, listOfParams)

			If result Is Nothing OrElse IsDBNull(result) Then
				Return Nothing
			Else
				Return result
			End If

		End Function


		''' <summary>
		''' Loads month close data by month and year.
		''' </summary>
		''' <param name="month">The month.</param>
		''' <param name="year">The year.</param>
		''' <returns>Month close data or nothing in error case.</returns>
		Public Function LoadMonthCloseData(ByVal mandantnumber As Integer, ByVal month As Integer, ByVal year As Integer) As MonthCloseData Implements IReportDatabaseAccess.LoadMonthCloseData

			Dim rpMasterData As MonthCloseData = Nothing

			Dim sql As String = String.Empty

			sql = sql & "Select [ID]"
			sql = sql & ",[Monat]"
			sql = sql & ",[Jahr]"
			sql = sql & ",[UserName]"
			sql = sql & ",[CreatedOn]"
			sql = sql & ",[MDNr]"
			sql = sql & " FROM [MonthClose]"
			sql = sql & " WHERE [Monat] = @month AND [Jahr] = @year And MDNr = @MDNr"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("month", month))
			listOfParams.Add(New SqlClient.SqlParameter("year", year))
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", mandantnumber))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If Not reader Is Nothing Then

					If reader.Read Then
						rpMasterData = New MonthCloseData

						rpMasterData.ID = SafeGetInteger(reader, "ID", 0)
						rpMasterData.Monat = SafeGetInteger(reader, "Monat", Nothing)
						rpMasterData.Jahr = SafeGetInteger(reader, "Jahr", Nothing)
						rpMasterData.UserName = SafeGetString(reader, "UserName")
						rpMasterData.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						rpMasterData.MDNr = SafeGetInteger(reader, "MDNr", Nothing)

					End If

				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				rpMasterData = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return rpMasterData

		End Function

		''' <summary>
		''' Loads the report finished ('Erfasst') flag.
		''' </summary>
		''' <param name="rpNr">The report number.</param>
		''' <param name="isReportFinished">Boolean flag indicating if report is finished.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function LoadRPFinishedFlag(ByVal rpNr As Integer, ByRef isReportFinished As Boolean) As Boolean Implements IReportDatabaseAccess.LoadRPFinishedFlag

			Dim success As Boolean = False
			isReportFinished = False

			Dim sql As String = String.Empty

			sql = sql & "Select [Erfasst]"
			sql = sql & " FROM [RP]"
			sql = sql & " WHERE [RPNr] = @rpNr"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("rpNr", rpNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If Not reader Is Nothing Then

					If reader.Read Then
						isReportFinished = SafeGetBoolean(reader, "Erfasst", False)
						success = True
					End If

				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				success = False
			Finally
				CloseReader(reader)
			End Try

			Return success

		End Function

		''' <summary>
		''' Loads rpl additional texts.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <returns>List of additional texts.</returns>

		Function LoadRPLAdditionalTexts(ByVal customerNumber As Integer) As IEnumerable(Of RPLAdditionalTextData) Implements IReportDatabaseAccess.LoadRPLAdditionalTexts


			Dim result As List(Of RPLAdditionalTextData) = Nothing

			Dim sql As String = String.Empty

			sql = sql & "(SELECT DISTINCT RPZusatzText FROM RPL WHERE KDNR = @customerNumber AND RPZusatzText IS NOT NULL AND RPZusatzText != '' "
			sql = sql & "UNION "
			sql = sql & "SELECT '') "
			sql = sql & "ORDER BY RPZusatzText ASC"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("customerNumber", customerNumber))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of RPLAdditionalTextData)

					While reader.Read()
						Dim rplAdditionalText As New RPLAdditionalTextData
						rplAdditionalText.AdditionalText = SafeGetString(reader, "RPZusatzText")

						result.Add(rplAdditionalText)

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
		''' Gets the next free RPL number.
		''' </summary>
		''' <param name="rpNr">The report number.</param>
		''' <returns>Next free RPL number of report.</returns>
		Public Function GetNextFeeRPLNumber(ByVal rpNr As Integer) As Integer? Implements IReportDatabaseAccess.GetNextFeeRPLNumber

			Dim nextFreeRPLNr As Integer? = 0

			Dim sql As String

			sql = "SELECT dbo.[Get Free RPLNr] (@RPNr)"

			' Parameters

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			' Data
			listOfParams.Add(New SqlClient.SqlParameter("@RPNr", rpNr))

			nextFreeRPLNr = ExecuteScalar(sql, listOfParams)

			Return nextFreeRPLNr

		End Function

		''' <summary>
		''' Finds a RPNr by ESNr, month and year.
		''' </summary>
		''' <param name="esNr">The ESNr.</param>
		''' <param name="month">The month.</param>
		''' <param name="year">The year.</param>
		''' <param name="foundRPNr">The found RPNr or nothing if the report could not be found.</param>
		''' <param name="foundRPId">The found RPId of nothing if the report could not be found.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function FindRPNrByESNrMonthAndYear(ByVal esNr As Integer, ByVal month As Byte, ByVal year As Integer, ByRef foundRPNr As Integer?, ByRef foundRPId As Integer?) As Boolean Implements IReportDatabaseAccess.FindRPNrByESNrMonthAndYear

			foundRPNr = Nothing
			foundRPId = Nothing

			Dim success As Boolean = True

			Dim sql As String

			sql = "SELECT TOP 1 ID, RPNR FROM RP WHERE ESNR = @esNr AND Monat = @month AND Jahr = @year"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@esNr", esNr))
			listOfParams.Add(New SqlClient.SqlParameter("@month", month))
			listOfParams.Add(New SqlClient.SqlParameter("@year", Convert.ToString(year)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If Not reader Is Nothing Then
					If reader.Read Then
						foundRPId = SafeGetInteger(reader, "ID", Nothing)
						foundRPNr = SafeGetInteger(reader, "RPNR", Nothing)
					End If
				Else
					' Reader count not be open -> success = false
					success = False
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				success = False
				foundRPNr = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return success
		End Function

		''' <summary>
		''' Determines if day data for an RPL exists.
		''' </summary>
		''' <param name="rpNr">The RPNr.</param>
		''' <param name="rplNr">The RPLNr.</param>
		''' <param name="rplDataType">The rpl type.</param>
		''' <returns>Boolean flag indicating existence or nothing in error case.</returns>
		Public Function ExistsRPLDayDataForRPL(ByVal rpNr As Integer, ByVal rplNr As Integer, ByVal rplDataType As RPLType) As Boolean? Implements IReportDatabaseAccess.ExistsRPLDayDataForRPL

			Dim doesRPDayDataExistsForRPL = False

			Dim sql As String
			Dim table As String = String.Empty

			Select Case rplDataType
				Case RPLType.Employee
					table = "RPL_MA_Day"
				Case RPLType.Customer
					table = "RPL_KD_Day"
				Case Else
					m_Logger.LogError(String.Format("Invalid RPL type: {0}", rplDataType.ToString()))
					Return Nothing
			End Select

			sql = String.Format("SELECT COUNT(*) FROM {0} WHERE RPNr = @rpNr AND RPLNr = @rplNr", table)

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@rpNr", rpNr))
			listOfParams.Add(New SqlClient.SqlParameter("@rplNr", rplNr))

			Dim existingDayDataCount = ExecuteScalar(sql, listOfParams)

			If Not existingDayDataCount Is Nothing Then
				doesRPDayDataExistsForRPL = (existingDayDataCount > 0)

				Return doesRPDayDataExistsForRPL
			Else
				Return Nothing
			End If

		End Function

		''' <summary>
		''' Determines if RP absence days data exists for rP.
		''' </summary>
		''' <param name="rpNr">The RPNr.</param>
		''' <returns>Boolean flag indicating existence or nothing in error case.</returns>
		Public Function ExistsRPAbsenceDaysDataForRP(ByVal rpNr As Integer) As Boolean? Implements IReportDatabaseAccess.ExistsRPAbsenceDaysDataForRP

			Dim doesRPAbsenceDaysDataExistsForRP = False

			Dim sql As String

			sql = "SELECT COUNT(*) FROM RP_Fehltage WHERE RPNr = @rpNr"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@rpNr", rpNr))

			Dim existingAbsenceDaysDataCount = ExecuteScalar(sql, listOfParams)

			If Not existingAbsenceDaysDataCount Is Nothing Then
				doesRPAbsenceDaysDataExistsForRP = (existingAbsenceDaysDataCount > 0)

				Return doesRPAbsenceDaysDataExistsForRP
			Else
				Return Nothing
			End If

		End Function

		''' <summary>
		''' Loads list of RPL day data.
		''' </summary>
		''' <param name="rpNr">The RPNr.</param>
		''' <param name="rplDataType">The RPL type.</param>
		''' <param name="rplNr">The optional RPLNr.</param>
		''' <returns>The list of RPL day data or nothing in error case.</returns>
		Public Function LoadRPLDayData(ByVal rpNr As Integer, ByVal rplDataType As RPLType, Optional ByVal rplNr As Integer? = Nothing) As IEnumerable(Of RPLDayData) Implements IReportDatabaseAccess.LoadRPLDayData

			Dim result As List(Of RPLDayData) = Nothing

			Dim table As String = String.Empty

			Select Case rplDataType
				Case RPLType.Employee
					table = "RPL_MA_Day"
				Case RPLType.Customer
					table = "RPL_KD_Day"
				Case Else
					m_Logger.LogError(String.Format("Invalid RPL type: {0}", rplDataType.ToString()))
					Return Nothing
			End Select

			Dim sql As String = String.Empty

			sql = sql & "Select [ID]"
			sql = sql & ",[RPNr]"
			sql = sql & ",[RPLNr]"
			Select Case rplDataType
				Case RPLType.Employee
					sql = sql & ",[MANr]"
				Case RPLType.Customer
					sql = sql & ",[KDNr]"
				Case Else
					' do  nothig
			End Select

			sql = sql & ",[ESNr]"
			sql = sql & ",[Monat]"
			sql = sql & ",[Jahr]"
			sql = sql & ",[Tag1]"
			sql = sql & ",[Tag2]"
			sql = sql & ",[Tag3]"
			sql = sql & ",[Tag4]"
			sql = sql & ",[Tag5]"
			sql = sql & ",[Tag6]"
			sql = sql & ",[Tag7]"
			sql = sql & ",[Tag8]"
			sql = sql & ",[Tag9]"
			sql = sql & ",[Tag10]"
			sql = sql & ",[Tag11]"
			sql = sql & ",[Tag12]"
			sql = sql & ",[Tag13]"
			sql = sql & ",[Tag14]"
			sql = sql & ",[Tag15]"
			sql = sql & ",[Tag16]"
			sql = sql & ",[Tag17]"
			sql = sql & ",[Tag18]"
			sql = sql & ",[Tag19]"
			sql = sql & ",[Tag20]"
			sql = sql & ",[Tag21]"
			sql = sql & ",[Tag22]"
			sql = sql & ",[Tag23]"
			sql = sql & ",[Tag24]"
			sql = sql & ",[Tag25]"
			sql = sql & ",[Tag26]"
			sql = sql & ",[Tag27]"
			sql = sql & ",[Tag28]"
			sql = sql & ",[Tag29]"
			sql = sql & ",[Tag30]"
			sql = sql & ",[Tag31]"
			sql = sql & ",[KumulativStd]"
			sql = sql & ",[KstNr]"
			sql = sql & ",[KstBez]"
			sql = sql & ",[ESLohnNr]"
			sql = sql & ",[IsDecimal]"
			sql = sql & String.Format(" FROM [{0}]", table)
			sql = sql & " WHERE [RPNr] = @rpNr AND ((@rpLNr IS NULL) OR ([RPLNr] = @rpLNr))"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("rpNr", rpNr))
			listOfParams.Add(New SqlClient.SqlParameter("rpLNr", ReplaceMissing(rplNr, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of RPLDayData)

					While reader.Read()
						Dim rplDayData As New RPLDayData

						rplDayData.ID = SafeGetInteger(reader, "ID", 0)
						rplDayData.RPNr = SafeGetInteger(reader, "RPNr", Nothing)
						rplDayData.RPLNr = SafeGetInteger(reader, "RPLNr", Nothing)

						Select Case rplDataType
							Case RPLType.Employee
								rplDayData.EmployeeNumber = SafeGetInteger(reader, "MANr", Nothing)
							Case RPLType.Customer
								rplDayData.CustomerNumber = SafeGetInteger(reader, "KDNr", Nothing)
							Case Else
								m_Logger.LogError(String.Format("Invalid RPL type: {0}", rplDataType.ToString()))
								Return Nothing
						End Select

						rplDayData.ESNr = SafeGetInteger(reader, "ESNr", Nothing)
						rplDayData.Monat = SafeGetByte(reader, "Monat", Nothing)
						rplDayData.Jahr = SafeGetString(reader, "Jahr")
						rplDayData.Tag1 = SafeGetDecimal(reader, "Tag1", Nothing)
						rplDayData.Tag2 = SafeGetDecimal(reader, "Tag2", Nothing)
						rplDayData.Tag3 = SafeGetDecimal(reader, "Tag3", Nothing)
						rplDayData.Tag4 = SafeGetDecimal(reader, "Tag4", Nothing)
						rplDayData.Tag5 = SafeGetDecimal(reader, "Tag5", Nothing)
						rplDayData.Tag6 = SafeGetDecimal(reader, "Tag6", Nothing)
						rplDayData.Tag7 = SafeGetDecimal(reader, "Tag7", Nothing)
						rplDayData.Tag8 = SafeGetDecimal(reader, "Tag8", Nothing)
						rplDayData.Tag9 = SafeGetDecimal(reader, "Tag9", Nothing)
						rplDayData.Tag10 = SafeGetDecimal(reader, "Tag10", Nothing)
						rplDayData.Tag11 = SafeGetDecimal(reader, "Tag11", Nothing)
						rplDayData.Tag12 = SafeGetDecimal(reader, "Tag12", Nothing)
						rplDayData.Tag13 = SafeGetDecimal(reader, "Tag13", Nothing)
						rplDayData.Tag14 = SafeGetDecimal(reader, "Tag14", Nothing)
						rplDayData.Tag15 = SafeGetDecimal(reader, "Tag15", Nothing)
						rplDayData.Tag16 = SafeGetDecimal(reader, "Tag16", Nothing)
						rplDayData.Tag17 = SafeGetDecimal(reader, "Tag17", Nothing)
						rplDayData.Tag18 = SafeGetDecimal(reader, "Tag18", Nothing)
						rplDayData.Tag19 = SafeGetDecimal(reader, "Tag19", Nothing)
						rplDayData.Tag20 = SafeGetDecimal(reader, "Tag20", Nothing)
						rplDayData.Tag21 = SafeGetDecimal(reader, "Tag21", Nothing)
						rplDayData.Tag22 = SafeGetDecimal(reader, "Tag22", Nothing)
						rplDayData.Tag23 = SafeGetDecimal(reader, "Tag23", Nothing)
						rplDayData.Tag24 = SafeGetDecimal(reader, "Tag24", Nothing)
						rplDayData.Tag25 = SafeGetDecimal(reader, "Tag25", Nothing)
						rplDayData.Tag26 = SafeGetDecimal(reader, "Tag26", Nothing)
						rplDayData.Tag27 = SafeGetDecimal(reader, "Tag27", Nothing)
						rplDayData.Tag28 = SafeGetDecimal(reader, "Tag28", Nothing)
						rplDayData.Tag29 = SafeGetDecimal(reader, "Tag29", Nothing)
						rplDayData.Tag30 = SafeGetDecimal(reader, "Tag30", Nothing)
						rplDayData.Tag31 = SafeGetDecimal(reader, "Tag31", Nothing)
						rplDayData.KumulativStd = SafeGetDecimal(reader, "KumulativStd", Nothing)
						rplDayData.KstNr = SafeGetInteger(reader, "KstNr", Nothing)
						rplDayData.KstBez = SafeGetString(reader, "KstBez", Nothing)
						rplDayData.ESLohnNr = SafeGetInteger(reader, "ESLohnNr", Nothing)
						rplDayData.isdecimal = SafeGetBoolean(reader, "isdecimal", False)

						rplDayData.Type = rplDataType

						result.Add(rplDayData)

					End While

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
		''' Adds new employee RPL data.
		''' </summary>
		''' <param name="initData">The init data.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function AddNewEmployeeRPLData(ByVal initData As NewEmployeeRPLInitData) As Boolean Implements IReportDatabaseAccess.AddNewEmployeeRPLData

			Dim success = True

			Dim sql As String

			sql = "[Create New MA_RPLData]"

			' Parameters

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			' Data
			listOfParams.Add(New SqlClient.SqlParameter("@RPNr", ReplaceMissing(initData.RPNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@RPLNr", ReplaceMissing(initData.RPLNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KDNr ", ReplaceMissing(initData.KDNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MANr", ReplaceMissing(initData.MANr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ESNr", ReplaceMissing(initData.ESNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KSTNR", ReplaceMissing(initData.KSTNR, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KstBez", ReplaceMissing(initData.KstBez, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@GAVText", ReplaceMissing(initData.GAVText, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Currency", ReplaceMissing(initData.Currency, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LANr", ReplaceMissing(initData.LANr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@M_Anzahl", ReplaceMissing(initData.M_Anzahl, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@M_Basis", ReplaceMissing(initData.M_Basis, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@M_Ansatz", ReplaceMissing(initData.M_Ansatz, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@SUVA", ReplaceMissing(initData.SUVA, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@M_Ferien", ReplaceMissing(initData.M_Ferien, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@M_Feier", ReplaceMissing(initData.M_Feier, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@M_13", ReplaceMissing(initData.M_13, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@VonDate", ReplaceMissing(initData.VonDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@BisDate", ReplaceMissing(initData.BisDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@FerBas", ReplaceMissing(initData.FerBas, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@13Basis", ReplaceMissing(initData.Basis13, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ESLohnNr", ReplaceMissing(initData.ESLohnNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LOSpesenBas", ReplaceMissing(initData.LOSpesenBas, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LOSpesen", ReplaceMissing(initData.LOSpesen, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@TAnzahl", ReplaceMissing(initData.TAnzahl, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MATSpesenBas", ReplaceMissing(initData.MATSpesenBas, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MATSpesen", ReplaceMissing(initData.MATSpesen, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@StdTotal", ReplaceMissing(initData.StdTotal, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@FeierTotal", ReplaceMissing(initData.FeierTotal, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@FerTotal", ReplaceMissing(initData.FerTotal, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@13Total", ReplaceMissing(initData.Total13, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KompStd", ReplaceMissing(initData.KompStd, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KompBetrag", ReplaceMissing(initData.KompBetrag, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@UserName", ReplaceMissing(initData.UserName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@IsPVL", ReplaceMissing(initData.IsPVL, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@RPZusatzText", ReplaceMissing(initData.RPZusatzText, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Return success

		End Function

		''' <summary>
		''' Adds new customer RPL data.
		''' </summary>
		''' <param name="initData">The init data.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function AddNewCustomerRPLData(ByVal initData As NewCustomerRPLInitData) As Boolean Implements IReportDatabaseAccess.AddNewCustomerRPLData

			Dim success = True

			Dim sql As String

			sql = "[Create New KD_RPLData]"

			' Parameters

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			' Data
			listOfParams.Add(New SqlClient.SqlParameter("@RPNr", ReplaceMissing(initData.RPNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@RPLNr", ReplaceMissing(initData.RPLNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KDNr", ReplaceMissing(initData.KDNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MANr", ReplaceMissing(initData.MANr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ESNr", ReplaceMissing(initData.ESNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KSTNR", ReplaceMissing(initData.KSTNR, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KstBez", ReplaceMissing(initData.KstBez, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@GAVText", ReplaceMissing(initData.GAVText, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Currency", ReplaceMissing(initData.Currency, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LANr", ReplaceMissing(initData.LANr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@K_Anzahl", ReplaceMissing(initData.K_Anzahl, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@K_Basis", ReplaceMissing(initData.K_Basis, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@K_Ansatz", ReplaceMissing(initData.K_Ansatz, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MWST", ReplaceMissing(initData.MWST, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@SUVA", ReplaceMissing(initData.SUVA, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@VonDate", ReplaceMissing(initData.VonDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@BisDate", ReplaceMissing(initData.BisDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ESLohnNr", ReplaceMissing(initData.ESLohnNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@TAnzahl", ReplaceMissing(initData.TAnzahl, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KDTSpesenBas", ReplaceMissing(initData.KDTSpesenBas, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KDTSpesen", ReplaceMissing(initData.KDTSpesen, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KDBetrag", ReplaceMissing(initData.KDBetrag, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@UserName", ReplaceMissing(initData.UserName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@RPZusatzText", ReplaceMissing(initData.RPZusatzText, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@IsCreatedWithEmployee", ReplaceMissing(initData.IsCreatedWithEmployee, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Return success

		End Function

		''' <summary>
		''' Adds new employee RPL day data.
		''' </summary>
		''' <param name="rplDayData">The RPL day data.</param>
		''' <returns>Boolean flag indicating succes.</returns>
		Public Function AddNewEmployeeRPLDayData(ByVal rplDayData As RPLDayData) As Boolean Implements IReportDatabaseAccess.AddNewEmployeeRPLDayData

			Dim success = True

			Dim sql As String

			sql = "[Create New RPL_MA_Day]"

			' Parameters

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			' Data
			listOfParams.Add(New SqlClient.SqlParameter("@RPNr", ReplaceMissing(rplDayData.RPNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@RPLNr", ReplaceMissing(rplDayData.RPLNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MANr", ReplaceMissing(rplDayData.EmployeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ESNr", ReplaceMissing(rplDayData.ESNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Monat", ReplaceMissing(rplDayData.Monat, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Jahr", ReplaceMissing(rplDayData.Jahr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag1", ReplaceMissing(rplDayData.Tag1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag2", ReplaceMissing(rplDayData.Tag2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag3", ReplaceMissing(rplDayData.Tag3, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag4", ReplaceMissing(rplDayData.Tag4, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag5", ReplaceMissing(rplDayData.Tag5, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag6", ReplaceMissing(rplDayData.Tag6, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag7", ReplaceMissing(rplDayData.Tag7, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag8", ReplaceMissing(rplDayData.Tag8, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag9", ReplaceMissing(rplDayData.Tag9, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag10", ReplaceMissing(rplDayData.Tag10, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag11", ReplaceMissing(rplDayData.Tag11, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag12", ReplaceMissing(rplDayData.Tag12, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag13", ReplaceMissing(rplDayData.Tag13, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag14", ReplaceMissing(rplDayData.Tag14, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag15", ReplaceMissing(rplDayData.Tag15, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag16", ReplaceMissing(rplDayData.Tag16, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag17", ReplaceMissing(rplDayData.Tag17, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag18", ReplaceMissing(rplDayData.Tag18, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag19", ReplaceMissing(rplDayData.Tag19, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag20", ReplaceMissing(rplDayData.Tag20, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag21", ReplaceMissing(rplDayData.Tag21, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag22", ReplaceMissing(rplDayData.Tag22, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag23", ReplaceMissing(rplDayData.Tag23, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag24", ReplaceMissing(rplDayData.Tag24, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag25", ReplaceMissing(rplDayData.Tag25, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag26", ReplaceMissing(rplDayData.Tag26, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag27", ReplaceMissing(rplDayData.Tag27, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag28", ReplaceMissing(rplDayData.Tag28, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag29", ReplaceMissing(rplDayData.Tag29, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag30", ReplaceMissing(rplDayData.Tag30, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag31", ReplaceMissing(rplDayData.Tag31, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KumulativStd", ReplaceMissing(rplDayData.KumulativStd, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KstNr", ReplaceMissing(rplDayData.KstNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KstBez", ReplaceMissing(rplDayData.KstBez, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ESLohnNr", ReplaceMissing(rplDayData.ESLohnNr, DBNull.Value)))

			Dim newIdParameter = New SqlClient.SqlParameter("@IdNewNewRPLDayData", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If success AndAlso
				Not newIdParameter.Value Is Nothing AndAlso
				Not IsDBNull(newIdParameter.Value) Then
				rplDayData.ID = CType(newIdParameter.Value, Integer)
			Else
				success = False
			End If

			Return success

		End Function

		''' <summary>
		''' Adds new customer RPL day data.
		''' </summary>
		''' <param name="rplDayData">The RPL day data.</param>
		''' <returns>Boolean flag indicating succes.</returns>
		Public Function AddNewCustomerRPLDayData(ByVal rplDayData As RPLDayData) As Boolean Implements IReportDatabaseAccess.AddNewCustomerRPLDayData

			Dim success = True

			Dim sql As String

			sql = "[Create New RPL_KD_Day]"

			' Parameters

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			' Data
			listOfParams.Add(New SqlClient.SqlParameter("@RPNr", ReplaceMissing(rplDayData.RPNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@RPLNr", ReplaceMissing(rplDayData.RPLNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KDNr", ReplaceMissing(rplDayData.CustomerNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ESNr", ReplaceMissing(rplDayData.ESNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Monat", ReplaceMissing(rplDayData.Monat, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Jahr", ReplaceMissing(rplDayData.Jahr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag1", ReplaceMissing(rplDayData.Tag1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag2", ReplaceMissing(rplDayData.Tag2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag3", ReplaceMissing(rplDayData.Tag3, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag4", ReplaceMissing(rplDayData.Tag4, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag5", ReplaceMissing(rplDayData.Tag5, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag6", ReplaceMissing(rplDayData.Tag6, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag7", ReplaceMissing(rplDayData.Tag7, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag8", ReplaceMissing(rplDayData.Tag8, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag9", ReplaceMissing(rplDayData.Tag9, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag10", ReplaceMissing(rplDayData.Tag10, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag11", ReplaceMissing(rplDayData.Tag11, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag12", ReplaceMissing(rplDayData.Tag12, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag13", ReplaceMissing(rplDayData.Tag13, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag14", ReplaceMissing(rplDayData.Tag14, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag15", ReplaceMissing(rplDayData.Tag15, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag16", ReplaceMissing(rplDayData.Tag16, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag17", ReplaceMissing(rplDayData.Tag17, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag18", ReplaceMissing(rplDayData.Tag18, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag19", ReplaceMissing(rplDayData.Tag19, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag20", ReplaceMissing(rplDayData.Tag20, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag21", ReplaceMissing(rplDayData.Tag21, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag22", ReplaceMissing(rplDayData.Tag22, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag23", ReplaceMissing(rplDayData.Tag23, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag24", ReplaceMissing(rplDayData.Tag24, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag25", ReplaceMissing(rplDayData.Tag25, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag26", ReplaceMissing(rplDayData.Tag26, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag27", ReplaceMissing(rplDayData.Tag27, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag28", ReplaceMissing(rplDayData.Tag28, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag29", ReplaceMissing(rplDayData.Tag29, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag30", ReplaceMissing(rplDayData.Tag30, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag31", ReplaceMissing(rplDayData.Tag31, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KumulativStd", ReplaceMissing(rplDayData.KumulativStd, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KstNr", ReplaceMissing(rplDayData.KstNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KstBez", ReplaceMissing(rplDayData.KstBez, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ESLohnNr", ReplaceMissing(rplDayData.ESLohnNr, DBNull.Value)))

			Dim newIdParameter = New SqlClient.SqlParameter("@IdNewNewRPLDayData", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If success AndAlso
				Not newIdParameter.Value Is Nothing AndAlso
				Not IsDBNull(newIdParameter.Value) Then
				rplDayData.ID = CType(newIdParameter.Value, Integer)
			Else
				success = False
			End If

			Return success

		End Function

		''' <summary>
		''' Adds new absence day data.
		''' </summary>
		''' <param name="absenceDayData">The absence day data.</param>
		''' <returns>Boolean flag indicating succes.</returns>
		Public Function AddNewAbsenceDayData(ByVal absenceDayData As RPAbsenceDaysData) As Boolean Implements IReportDatabaseAccess.AddNewAbsenceDayData

			Dim success = True

			Dim sql As String

			sql = "[Create New RP_Fehltage]"

			' Parameters

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			' Data
			listOfParams.Add(New SqlClient.SqlParameter("@RPNr", ReplaceMissing(absenceDayData.RPNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag1", ReplaceMissing(absenceDayData.Fehltag1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag2", ReplaceMissing(absenceDayData.Fehltag2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag3", ReplaceMissing(absenceDayData.Fehltag3, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag4", ReplaceMissing(absenceDayData.Fehltag4, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag5", ReplaceMissing(absenceDayData.Fehltag5, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag6", ReplaceMissing(absenceDayData.Fehltag6, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag7", ReplaceMissing(absenceDayData.Fehltag7, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag8", ReplaceMissing(absenceDayData.Fehltag8, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag9", ReplaceMissing(absenceDayData.Fehltag9, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag10", ReplaceMissing(absenceDayData.Fehltag10, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag11", ReplaceMissing(absenceDayData.Fehltag11, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag12", ReplaceMissing(absenceDayData.Fehltag12, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag13", ReplaceMissing(absenceDayData.Fehltag13, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag14", ReplaceMissing(absenceDayData.Fehltag14, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag15", ReplaceMissing(absenceDayData.Fehltag15, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag16", ReplaceMissing(absenceDayData.Fehltag16, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag17", ReplaceMissing(absenceDayData.Fehltag17, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag18", ReplaceMissing(absenceDayData.Fehltag18, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag19", ReplaceMissing(absenceDayData.Fehltag19, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag20", ReplaceMissing(absenceDayData.Fehltag20, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag21", ReplaceMissing(absenceDayData.Fehltag21, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag22", ReplaceMissing(absenceDayData.Fehltag22, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag23", ReplaceMissing(absenceDayData.Fehltag23, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag24", ReplaceMissing(absenceDayData.Fehltag24, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag25", ReplaceMissing(absenceDayData.Fehltag25, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag26", ReplaceMissing(absenceDayData.Fehltag26, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag27", ReplaceMissing(absenceDayData.Fehltag27, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag28", ReplaceMissing(absenceDayData.Fehltag28, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag29", ReplaceMissing(absenceDayData.Fehltag29, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag30", ReplaceMissing(absenceDayData.Fehltag30, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag31", ReplaceMissing(absenceDayData.Fehltag31, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@RPNr2", ReplaceMissing(absenceDayData.RPNr2, DBNull.Value)))

			Dim newIdParameter = New SqlClient.SqlParameter("@IDNewRPFehltage", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If success AndAlso
				Not newIdParameter.Value Is Nothing AndAlso
				Not IsDBNull(newIdParameter.Value) Then
				absenceDayData.ID = CType(newIdParameter.Value, Integer)
			Else
				success = False
			End If

			Return success

		End Function

		''' <summary>
		''' Adds a new report for an existing ES.
		''' </summary>
		''' <param name="initData">The init data.</param>
		''' <returns>Boolean flag indicating succes.</returns>
		Public Function AddNewRPForExistingES(ByVal initData As NewRPForExistingESData) As Boolean Implements IReportDatabaseAccess.AddNewRPForExistingES

			Dim success = True

			Dim sql As String

			sql = "[Create New RP For Existing ES]"

			' Parameters

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			' Data
			listOfParams.Add(New SqlClient.SqlParameter("@ESNr", ReplaceMissing(initData.ESNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@RPMonth", ReplaceMissing(initData.RPMonth, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@RPYear", ReplaceMissing(initData.RPYear, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@RPVon", ReplaceMissing(initData.RPVon, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@RPBis", ReplaceMissing(initData.RPBis, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@RPSuva", ReplaceMissing(initData.RPSuva, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@RPKst", ReplaceMissing(initData.RPKst, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@RPKst1", ReplaceMissing(initData.RPKst1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@RPKst2", ReplaceMissing(initData.RPKst2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@RPKDBranche", ReplaceMissing(initData.RPKDBranche, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", ReplaceMissing(initData.MDNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@RPNumberOffset", ReplaceMissing(initData.RPNumberOffset, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@CreatedFrom", ReplaceMissing(initData.CreatedFrom, DBNull.Value)))

			Dim newRPNrParameter = New SqlClient.SqlParameter("@NewRPNr", SqlDbType.Int)
			newRPNrParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newRPNrParameter)

			Dim newIdParameter = New SqlClient.SqlParameter("@NewId", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If success AndAlso
				Not newRPNrParameter.Value Is Nothing AndAlso
				Not IsDBNull(newRPNrParameter.Value) Then
				initData.NewRPNrOutput = CType(newRPNrParameter.Value, Integer)
			Else
				success = False
			End If

			If success AndAlso
				Not newIdParameter.Value Is Nothing AndAlso
				Not IsDBNull(newIdParameter.Value) Then
				initData.NewIdRPOutput = CType(newIdParameter.Value, Integer)
			Else
				success = False
			End If

			Return success

		End Function

		''' <summary>
		''' Updates employee RPL data.
		''' </summary>
		''' <param name="updateData">The update data.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function UpdateEmployeeRPLData(ByVal updateData As UpdateEmployeeRPLData) As Boolean Implements IReportDatabaseAccess.UpdateEmployeeRPLData

			Dim success = True

			Dim sql As String = String.Empty

			sql = sql & "[UPDATE MA_RPLData]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@RPNr", ReplaceMissing(updateData.RPNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@RPLNr", ReplaceMissing(updateData.RPLNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LANr", ReplaceMissing(updateData.LANr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@M_Anzahl", ReplaceMissing(updateData.M_Anzahl, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@M_Basis", ReplaceMissing(updateData.M_Basis, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@M_Ansatz", ReplaceMissing(updateData.M_Ansatz, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@M_Ferien", ReplaceMissing(updateData.M_Ferien, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@M_Feier", ReplaceMissing(updateData.M_Feier, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@M_13", ReplaceMissing(updateData.M_13, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@VonDate", ReplaceMissing(updateData.VonDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@BisDate", ReplaceMissing(updateData.BisDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ESLohnNr", ReplaceMissing(updateData.ESLohnNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LOSpesenBas", ReplaceMissing(updateData.LOSpesenBas, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LOSpesen", ReplaceMissing(updateData.LOSpesen, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@StdTotal", ReplaceMissing(updateData.StdTotal, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@FeierTotal", ReplaceMissing(updateData.FeierTotal, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@FerTotal", ReplaceMissing(updateData.FerTotal, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@13Total", ReplaceMissing(updateData.Total13, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@FerBas", ReplaceMissing(updateData.FerBas, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@13Basis", ReplaceMissing(updateData.Basis13, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@TAnzahl", ReplaceMissing(updateData.TAnzahl, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MATSpesenBas ", ReplaceMissing(updateData.MATSpesenBas, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MATSpesen", ReplaceMissing(updateData.MATSpesen, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KstNr ", ReplaceMissing(updateData.KstNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KstBez", ReplaceMissing(updateData.KstBez, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KompStd", ReplaceMissing(updateData.KompStd, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KompBetrag", ReplaceMissing(updateData.KompBetrag, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@UserName", ReplaceMissing(updateData.UserName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@IsPVL", ReplaceMissing(updateData.IsPVL, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@RPZusatzText", ReplaceMissing(updateData.RPZusatzText, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Return success

		End Function

		''' <summary>
		''' Updates customer RPL data.
		''' </summary>
		''' <param name="updateData">The update data.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function UpdateCustomerRPLData(ByVal updateData As UpdateCustomerRPLData) As Boolean Implements IReportDatabaseAccess.UpdateCustomerRPLData
			Dim success = True

			Dim sql As String = String.Empty

			sql = sql & "[UPDATE KD_RPLData]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@RPNr", ReplaceMissing(updateData.RPNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@RPLNr", ReplaceMissing(updateData.RPLNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LANr", ReplaceMissing(updateData.LANr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@K_Anzahl", ReplaceMissing(updateData.K_Anzahl, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@K_Basis", ReplaceMissing(updateData.K_Basis, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@K_Ansatz", ReplaceMissing(updateData.K_Ansatz, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MWST", ReplaceMissing(updateData.MWST, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@VonDate", ReplaceMissing(updateData.VonDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@BisDate", ReplaceMissing(updateData.BisDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ESLohnNr", ReplaceMissing(updateData.ESLohnNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KDBetrag", ReplaceMissing(updateData.KDBetrag, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@TAnzahl", ReplaceMissing(updateData.TAnzahl, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MATSpesenBas", ReplaceMissing(updateData.MATSpesenBas, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MATSpesen", ReplaceMissing(updateData.MATSpesen, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KstNr", ReplaceMissing(updateData.KstNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KstBez", ReplaceMissing(updateData.KstBez, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@UserName", ReplaceMissing(updateData.UserName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@RPZusatzText", ReplaceMissing(updateData.RPZusatzText, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Return success

		End Function

		''' <summary>
		''' Updates employee RPL day data.
		''' </summary>
		''' <param name="rplDayData">The rpl Day data.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function UpdateEmployeeRPLDayData(ByVal rplDayData As RPLDayData) As Boolean Implements IReportDatabaseAccess.UpdateEmployeeRPLDayData

			Dim success = True

			Dim sql As String = String.Empty

			sql = sql & "[UPDATE RPL_MA_Day]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(rplDayData.ID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@RPNr ", ReplaceMissing(rplDayData.RPNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@RPLNr ", ReplaceMissing(rplDayData.RPLNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MANr ", ReplaceMissing(rplDayData.EmployeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ESNr ", ReplaceMissing(rplDayData.ESNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Monat ", ReplaceMissing(rplDayData.Monat, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Jahr ", ReplaceMissing(rplDayData.Jahr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag1", ReplaceMissing(rplDayData.Tag1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag2", ReplaceMissing(rplDayData.Tag2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag3", ReplaceMissing(rplDayData.Tag3, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag4", ReplaceMissing(rplDayData.Tag4, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag5", ReplaceMissing(rplDayData.Tag5, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag6", ReplaceMissing(rplDayData.Tag6, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag7", ReplaceMissing(rplDayData.Tag7, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag8", ReplaceMissing(rplDayData.Tag8, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag9", ReplaceMissing(rplDayData.Tag9, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag10", ReplaceMissing(rplDayData.Tag10, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag11", ReplaceMissing(rplDayData.Tag11, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag12", ReplaceMissing(rplDayData.Tag12, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag13", ReplaceMissing(rplDayData.Tag13, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag14", ReplaceMissing(rplDayData.Tag14, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag15", ReplaceMissing(rplDayData.Tag15, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag16", ReplaceMissing(rplDayData.Tag16, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag17", ReplaceMissing(rplDayData.Tag17, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag18", ReplaceMissing(rplDayData.Tag18, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag19", ReplaceMissing(rplDayData.Tag19, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag20", ReplaceMissing(rplDayData.Tag20, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag21", ReplaceMissing(rplDayData.Tag21, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag22", ReplaceMissing(rplDayData.Tag22, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag23", ReplaceMissing(rplDayData.Tag23, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag24", ReplaceMissing(rplDayData.Tag24, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag25", ReplaceMissing(rplDayData.Tag25, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag26", ReplaceMissing(rplDayData.Tag26, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag27", ReplaceMissing(rplDayData.Tag27, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag28", ReplaceMissing(rplDayData.Tag28, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag29", ReplaceMissing(rplDayData.Tag29, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag30", ReplaceMissing(rplDayData.Tag30, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag31", ReplaceMissing(rplDayData.Tag31, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KumulativStd ", ReplaceMissing(rplDayData.KumulativStd, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KstNr ", ReplaceMissing(rplDayData.KstNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KstBez ", ReplaceMissing(rplDayData.KstBez, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ESLohnNr ", ReplaceMissing(rplDayData.ESLohnNr, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Return success

		End Function

		''' <summary>
		''' Updates customer RPL day data.
		''' </summary>
		''' <param name="rplDayData">The rpl Day data.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function UpdateCustomerRPLDayData(ByVal rplDayData As RPLDayData) As Boolean Implements IReportDatabaseAccess.UpdateCustomerRPLDayData

			Dim success = True

			Dim sql As String = String.Empty

			sql = sql & "[UPDATE RPL_KD_Day]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(rplDayData.ID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@RPNr ", ReplaceMissing(rplDayData.RPNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@RPLNr ", ReplaceMissing(rplDayData.RPLNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KDNr ", ReplaceMissing(rplDayData.CustomerNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ESNr ", ReplaceMissing(rplDayData.ESNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Monat ", ReplaceMissing(rplDayData.Monat, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Jahr ", ReplaceMissing(rplDayData.Jahr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag1", ReplaceMissing(rplDayData.Tag1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag2", ReplaceMissing(rplDayData.Tag2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag3", ReplaceMissing(rplDayData.Tag3, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag4", ReplaceMissing(rplDayData.Tag4, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag5", ReplaceMissing(rplDayData.Tag5, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag6", ReplaceMissing(rplDayData.Tag6, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag7", ReplaceMissing(rplDayData.Tag7, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag8", ReplaceMissing(rplDayData.Tag8, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag9", ReplaceMissing(rplDayData.Tag9, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag10", ReplaceMissing(rplDayData.Tag10, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag11", ReplaceMissing(rplDayData.Tag11, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag12", ReplaceMissing(rplDayData.Tag12, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag13", ReplaceMissing(rplDayData.Tag13, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag14", ReplaceMissing(rplDayData.Tag14, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag15", ReplaceMissing(rplDayData.Tag15, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag16", ReplaceMissing(rplDayData.Tag16, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag17", ReplaceMissing(rplDayData.Tag17, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag18", ReplaceMissing(rplDayData.Tag18, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag19", ReplaceMissing(rplDayData.Tag19, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag20", ReplaceMissing(rplDayData.Tag20, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag21", ReplaceMissing(rplDayData.Tag21, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag22", ReplaceMissing(rplDayData.Tag22, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag23", ReplaceMissing(rplDayData.Tag23, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag24", ReplaceMissing(rplDayData.Tag24, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag25", ReplaceMissing(rplDayData.Tag25, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag26", ReplaceMissing(rplDayData.Tag26, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag27", ReplaceMissing(rplDayData.Tag27, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag28", ReplaceMissing(rplDayData.Tag28, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag29", ReplaceMissing(rplDayData.Tag29, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag30", ReplaceMissing(rplDayData.Tag30, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tag31", ReplaceMissing(rplDayData.Tag31, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KumulativStd ", ReplaceMissing(rplDayData.KumulativStd, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KstNr ", ReplaceMissing(rplDayData.KstNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KstBez ", ReplaceMissing(rplDayData.KstBez, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ESLohnNr ", ReplaceMissing(rplDayData.ESLohnNr, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Return success

		End Function

		''' <summary>
		''' Updates RP absence day data.
		''' </summary>
		''' <param name="RPAbsenceDaysData">The RP absence days data.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function UpdateRPAbsenceDaysData(ByVal rpAbsenceDaysData As RPAbsenceDaysData) As Boolean Implements IReportDatabaseAccess.UpdateRPAbsenceDaysData

			Dim success = True

			Dim sql As String = String.Empty

			sql = sql & "[UPDATE RP_Fehltage]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(rpAbsenceDaysData.ID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@RPNr", ReplaceMissing(rpAbsenceDaysData.RPNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag1", ReplaceMissing(rpAbsenceDaysData.Fehltag1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag2", ReplaceMissing(rpAbsenceDaysData.Fehltag2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag3", ReplaceMissing(rpAbsenceDaysData.Fehltag3, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag4", ReplaceMissing(rpAbsenceDaysData.Fehltag4, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag5", ReplaceMissing(rpAbsenceDaysData.Fehltag5, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag6", ReplaceMissing(rpAbsenceDaysData.Fehltag6, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag7", ReplaceMissing(rpAbsenceDaysData.Fehltag7, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag8", ReplaceMissing(rpAbsenceDaysData.Fehltag8, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag9", ReplaceMissing(rpAbsenceDaysData.Fehltag9, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag10", ReplaceMissing(rpAbsenceDaysData.Fehltag10, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag11", ReplaceMissing(rpAbsenceDaysData.Fehltag11, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag12", ReplaceMissing(rpAbsenceDaysData.Fehltag12, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag13", ReplaceMissing(rpAbsenceDaysData.Fehltag13, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag14", ReplaceMissing(rpAbsenceDaysData.Fehltag14, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag15", ReplaceMissing(rpAbsenceDaysData.Fehltag15, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag16", ReplaceMissing(rpAbsenceDaysData.Fehltag16, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag17", ReplaceMissing(rpAbsenceDaysData.Fehltag17, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag18", ReplaceMissing(rpAbsenceDaysData.Fehltag18, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag19", ReplaceMissing(rpAbsenceDaysData.Fehltag19, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag20", ReplaceMissing(rpAbsenceDaysData.Fehltag20, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag21", ReplaceMissing(rpAbsenceDaysData.Fehltag21, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag22", ReplaceMissing(rpAbsenceDaysData.Fehltag22, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag23", ReplaceMissing(rpAbsenceDaysData.Fehltag23, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag24", ReplaceMissing(rpAbsenceDaysData.Fehltag24, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag25", ReplaceMissing(rpAbsenceDaysData.Fehltag25, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag26", ReplaceMissing(rpAbsenceDaysData.Fehltag26, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag27", ReplaceMissing(rpAbsenceDaysData.Fehltag27, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag28", ReplaceMissing(rpAbsenceDaysData.Fehltag28, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag29", ReplaceMissing(rpAbsenceDaysData.Fehltag29, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag30", ReplaceMissing(rpAbsenceDaysData.Fehltag30, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fehltag31", ReplaceMissing(rpAbsenceDaysData.Fehltag31, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@RPNr2", ReplaceMissing(rpAbsenceDaysData.RPNr2, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Return success

		End Function

		''' <summary>
		''' Updates the RP finsihed ('Erfasst') flag.
		''' </summary>
		''' <param name="rpNr">The report number.</param>
		''' <param name="finished">The finsihed flag.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function UpdateRPFinishedFlag(ByVal rpNr As Integer, ByVal finished As Boolean) As Boolean Implements IReportDatabaseAccess.UpdateRPFinishedFlag

			Dim success = True

			Dim sql As String

			sql = "UPDATE dbo.RP SET "
			sql = sql & "Erfasst = @erfasst "
			sql = sql & "WHERE RPNR = @rpNr "

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("rpNr", rpNr))
			listOfParams.Add(New SqlClient.SqlParameter("erfasst", finished))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

    End Function

    ''' <summary>
    ''' Update employee RPL TSpesen data.
    ''' </summary>
    ''' <param name="rpNr">The report number.</param>
    ''' <param name="anzTSpesen">The number of spesen days.</param>
    ''' <param name="esLohnNr">The ESLohnNr.</param>
    ''' <returns>Boolen flag indicating success.</returns>
    Public Function UpdateEmployeeRPLTSpesenData(ByVal rpNr As Integer, ByVal anzTSpesen As Integer, ByVal esLohnNr As Integer) As Boolean Implements IReportDatabaseAccess.UpdateEmployeeRPLTSpesenData

      Dim success = True

      Dim sql As String = String.Empty

      sql = sql & "[Update MA_RPLTSpesenData]"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("@RPNr", ReplaceMissing(rpNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@AnzTSpesen", ReplaceMissing(anzTSpesen, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ESLohnNr ", ReplaceMissing(esLohnNr, DBNull.Value)))

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      Return success

    End Function

    ''' <summary>
    ''' Correct RP absence days data after delete of RPL.
    ''' </summary>
    ''' <param name="rpNr">The RPNr.</param>
    ''' <param name="rpYear">The RP year.</param>
    ''' <param name="rpMonth">The RP month.</param>
    ''' <returns>Boolean truth value indicating success.</returns>
    Public Function CorrectRPAbsenceDaysDataAfterDeleteOfRPL(ByVal rpNr As Integer, ByVal rpYear As Integer, ByVal rpMonth As Integer) As Boolean Implements IReportDatabaseAccess.CorrectRPAbsenceDaysDataAfterDeleteOfRPL

      Dim success As Boolean = True

      Dim minRPLDate As DateTime?
      Dim maxRPLDate As DateTime?

      Dim sql As String = "[Get Min And MaxDate for RPAbsenceDay Correction]"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("rpNr", rpNr))
      listOfParams.Add(New SqlClient.SqlParameter("year", rpYear))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

      Try

        If Not reader Is Nothing Then

          If reader.Read Then

            minRPLDate = SafeGetDateTime(reader, "MinDate", Nothing)
            maxRPLDate = SafeGetDateTime(reader, "MaxDate", Nothing)

          End If

        End If

      Catch ex As Exception
        m_Logger.LogError(ex.ToString())
        success = False
        Return success
      Finally
        CloseReader(reader)
      End Try

      Dim absenceAbsenceDayData = LoadRPAbsenceDaysData(rpNr)

      If Not absenceAbsenceDayData Is Nothing Then

        If minRPLDate.HasValue Then
          ' Reset (Init) days before minimal RPL date
          absenceAbsenceDayData.InitAbsenceDayDataBeforeDate(rpYear, rpMonth, minRPLDate.Value)

        End If

        If maxRPLDate.HasValue Then
          ' Reset (Init) days after maximal RPL date
          absenceAbsenceDayData.InitAbsenceDayDataAfterDate(rpYear, rpMonth, maxRPLDate.Value)
        End If

        success = UpdateRPAbsenceDaysData(absenceAbsenceDayData)

      End If

      Return success

    End Function


    ''' <summary>
    ''' Gets RPL day hours total data.
    ''' </summary>
    ''' <param name="rpNr">The report number.</param>
    ''' <param name="rplDataType">The RPL type.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function GetRPLDayHoursTotal(ByVal rpNr As Integer, ByVal rplDataType As RPLType) As RPLDayHoursTotal Implements IReportDatabaseAccess.GetRPLDayHoursTotal

      Dim totalData As RPLDayHoursTotal = Nothing

      Dim sql As String = String.Empty

      Select Case rplDataType
        Case RPLType.Employee
          sql = "[Get RPL_MA_Day Tagessummen]"
        Case RPLType.Customer
          sql = "[Get RPL_KD_Day Tagessummen]"
        Case Else
          m_Logger.LogError(String.Format("Invalid RPL type: {0}", rplDataType.ToString()))
          Return Nothing
      End Select

      ' Parameters
      Dim rpNumberParameter As New SqlClient.SqlParameter("rpNr", rpNr)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(rpNumberParameter)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

      Try

        If Not reader Is Nothing Then
          totalData = New RPLDayHoursTotal

          If reader.Read Then
            totalData.RPNr = rpNr

            totalData.Tag1 = SafeGetDecimal(reader, "SumTag1", 0)
            totalData.Tag2 = SafeGetDecimal(reader, "SumTag2", 0)
            totalData.Tag3 = SafeGetDecimal(reader, "SumTag3", 0)
            totalData.Tag4 = SafeGetDecimal(reader, "SumTag4", 0)
            totalData.Tag5 = SafeGetDecimal(reader, "SumTag5", 0)
            totalData.Tag6 = SafeGetDecimal(reader, "SumTag6", 0)
            totalData.Tag7 = SafeGetDecimal(reader, "SumTag7", 0)
            totalData.Tag8 = SafeGetDecimal(reader, "SumTag8", 0)
            totalData.Tag9 = SafeGetDecimal(reader, "SumTag9", 0)
            totalData.Tag10 = SafeGetDecimal(reader, "SumTag10", 0)
            totalData.Tag11 = SafeGetDecimal(reader, "SumTag11", 0)
            totalData.Tag12 = SafeGetDecimal(reader, "SumTag12", 0)
            totalData.Tag13 = SafeGetDecimal(reader, "SumTag13", 0)
            totalData.Tag14 = SafeGetDecimal(reader, "SumTag14", 0)
            totalData.Tag15 = SafeGetDecimal(reader, "SumTag15", 0)
            totalData.Tag16 = SafeGetDecimal(reader, "SumTag16", 0)
            totalData.Tag17 = SafeGetDecimal(reader, "SumTag17", 0)
            totalData.Tag18 = SafeGetDecimal(reader, "SumTag18", 0)
            totalData.Tag19 = SafeGetDecimal(reader, "SumTag19", 0)
            totalData.Tag20 = SafeGetDecimal(reader, "SumTag20", 0)
            totalData.Tag21 = SafeGetDecimal(reader, "SumTag21", 0)
            totalData.Tag22 = SafeGetDecimal(reader, "SumTag22", 0)
            totalData.Tag23 = SafeGetDecimal(reader, "SumTag23", 0)
            totalData.Tag24 = SafeGetDecimal(reader, "SumTag24", 0)
            totalData.Tag25 = SafeGetDecimal(reader, "SumTag25", 0)
            totalData.Tag26 = SafeGetDecimal(reader, "SumTag26", 0)
            totalData.Tag27 = SafeGetDecimal(reader, "SumTag27", 0)
            totalData.Tag28 = SafeGetDecimal(reader, "SumTag28", 0)
            totalData.Tag29 = SafeGetDecimal(reader, "SumTag29", 0)
            totalData.Tag30 = SafeGetDecimal(reader, "SumTag30", 0)
            totalData.Tag31 = SafeGetDecimal(reader, "SumTag31", 0)

            totalData.isdecimal = SafeGetBoolean(reader, "IsDecimal", False)

            totalData.Type = rplDataType
          End If

        End If

      Catch ex As Exception
        m_Logger.LogError(ex.ToString())
        totalData = Nothing
      Finally
        CloseReader(reader)
      End Try

      Return totalData

    End Function

    ''' <summary>
		''' get RPL scan doc guid for wos deleting.
    ''' </summary>
    ''' <param name="rpNr">The report number.</param>
    ''' <param name="rplNr">The RPL number.</param>
    ''' <returns>Boolean flag indicating success.</returns>
		Public Function GetRPLScanDocGuid(ByVal rpNr As Integer, ByVal rplNr As Integer) As String Implements IReportDatabaseAccess.GetRPLScanDocGuid

			Dim result As String = String.Empty
			Dim sql As String

			sql = "Select RPDoc_Guid "
			sql &= "From RP_ScanDoc "
			sql &= "Where RPNr = @RPNr And RPLNr = @RPLNr"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("RPNr", rpNr))
			listOfParams.Add(New SqlClient.SqlParameter("RPLNr", rplNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			If Not reader Is Nothing Then
				If reader.Read Then
					result = SafeGetString(reader, "RPDoc_Guid")

				End If
			End If


			Return result

		End Function

		''' <summary>
		''' Deletes employee RPL data.
		''' </summary>
		''' <param name="rpNr">The report number.</param>
		''' <param name="rplNr">The RPL number.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function DeleteEmployeeRPLData(ByVal rpNr As Integer, ByVal rplNr As Integer) As DeleteMARPLDataResult Implements IReportDatabaseAccess.DeleteEmployeeRPLData

			Dim success As Boolean = True
			Dim sql As String

			sql = "[Delete MA_RPLData]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("RPNr", rpNr))
			listOfParams.Add(New SqlClient.SqlParameter("RPLNr", rplNr))

			Dim resultParameter = New SqlClient.SqlParameter("Result", SqlDbType.Int)
			resultParameter.Direction = ParameterDirection.Output
			listOfParams.Add(resultParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Dim resultEnum As DeleteMARPLDataResult

			If success AndAlso
				Not resultParameter.Value Is Nothing AndAlso
				Not IsDBNull(resultParameter.Value) Then
				Try
					resultEnum = CType(resultParameter.Value, DeleteMARPLDataResult)
				Catch
					resultEnum = DeleteMARPLDataResult.ResultDeleteError
				End Try
			Else
				resultEnum = DeleteMARPLDataResult.ResultDeleteError
			End If

			Return resultEnum

		End Function

    ''' <summary>
    ''' Deletes customer RPL data.
    ''' </summary>
    ''' <param name="rpNr">The report number.</param>
    ''' <param name="rplNr">The RPL number.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function DeleteCustomerRPLData(ByVal rpNr As Integer, ByVal rplNr As Integer) As DeleteKDRPLDataResult Implements IReportDatabaseAccess.DeleteCustomerRPLData
      Dim success = True

      Dim sql As String

      sql = "[Delete KD_RPLData]"

      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("RPNr", rpNr))
      listOfParams.Add(New SqlClient.SqlParameter("RPLNr", rplNr))

      Dim resultParameter = New SqlClient.SqlParameter("Result", SqlDbType.Int)
      resultParameter.Direction = ParameterDirection.Output
      listOfParams.Add(resultParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      Dim resultEnum As DeleteKDRPLDataResult

      If success AndAlso
        Not resultParameter.Value Is Nothing AndAlso
        Not IsDBNull(resultParameter.Value) Then
        Try
          resultEnum = CType(resultParameter.Value, DeleteKDRPLDataResult)
        Catch
          resultEnum = DeleteKDRPLDataResult.ResultDeleteError
        End Try
      Else
        resultEnum = DeleteKDRPLDataResult.ResultDeleteError
      End If

      Return resultEnum

    End Function

    ''' <summary>
    ''' Clears the flexible time of an RPL.
    ''' </summary>
    ''' <param name="rpNr">The report number.</param>
    ''' <param name="rplNr">The RPL number.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function ClearFlexibleTimeOfRPL(ByVal rpNr As Integer, ByVal rplNr As Integer) As Boolean? Implements IReportDatabaseAccess.ClearFlexibleTimeOfRPL

      Dim success = True

      Dim sql As String

      sql = "UPDATE RPL SET "
      sql = sql & "KompStd = 0, "
      sql = sql & "KompBetrag = 0 "
      sql = sql & "WHERE RPNR = @rpNr AND RPLNr = @rplNr"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)

      listOfParams.Add(New SqlClient.SqlParameter("rpNr", rpNr))
      listOfParams.Add(New SqlClient.SqlParameter("rplNr", rplNr))

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function

#End Region

  End Class

End Namespace
