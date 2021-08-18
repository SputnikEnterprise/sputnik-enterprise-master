Imports SP.DatabaseAccess
Imports System.Text
Imports SP.Infrastructure
Imports SP.DatabaseAccess.DTAUtility.DataObjects

Namespace DTAUtility

	Partial Public Class DTAUtilityDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IDTAUtilityDatabaseAccess

#Region "Public Enums"

		Public Enum ZgTypeEnum
			CreatePain001FileSwiss = 4
			''' <summary>
			''' DTA File Schweiz.
			''' </summary>
			''' <remarks></remarks>
			CreateDtaFileSwiss = 1
			''' <summary>
			''' DTA File Ausland.
			''' </summary>
			''' <remarks></remarks>
			CreateDtaFileForeign = 2
			''' <summary>
			''' Vergütungsauftrag.
			''' </summary>
			''' <remarks></remarks>
			CreateVg = 3
		End Enum

		Public Enum DtaJobTypeEnum
			''' <summary>
			''' DTA Aufträge.
			''' </summary>
			''' <remarks></remarks>
			Dta
			''' <summary>
			''' DTA Kreditoren Aufträge.
			''' </summary>
			''' <remarks></remarks>
			DtaLol
		End Enum

#End Region ' Public Enums


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
		''' Loads Bank data from mandant.
		''' </summary>
		Function LoadBankData(ByVal mdNr As Integer) As List(Of DataObjects.BankData) Implements IDTAUtilityDatabaseAccess.LoadBankData
			Dim bankDatas As List(Of DataObjects.BankData) = Nothing

			Dim sql As String
			sql = "SELECT ID, RecNr, KontoESR2, BankName, RecBez"
			sql &= ", IsNull(Swift, '') Swift"
			sql &= ", IsNull(DTAIBAN, '') DTAIBAN "
			sql &= ", IsNull(AsStandard, 0) AsStandard "
			sql &= " From dbo.MD_ESRDTA "
			sql &= " Where ModulArt = 0 And MDNr = @mdNr "
			sql &= " Order By AsStandard Desc"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try
				If reader IsNot Nothing Then

					bankDatas = New List(Of DataObjects.BankData)

					While reader.Read
						Dim bankData = New DataObjects.BankData With {
								.ID = SafeGetInteger(reader, "ID", Nothing),
								.RecNr = SafeGetInteger(reader, "RecNr", Nothing),
								.KontoESR2 = SafeGetString(reader, "KontoESR2"),
								.BankName = SafeGetString(reader, "BankName"),
								.RecBez = SafeGetString(reader, "RecBez"),
								.Swift = SafeGetString(reader, "Swift"),
								.IBANNr = SafeGetString(reader, "DTAIBAN"),
							 .AsStandard = SafeGetBoolean(reader, "AsStandard", False)
							}
						bankDatas.Add(bankData)
					End While
				End If
			Catch e As Exception
				m_Logger.LogError(e.ToString())
				bankDatas = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return bankDatas
		End Function

		Public Function LoadJobNumberData(ByVal dtaJobType As DtaJobTypeEnum, ByVal mdNr As Integer) As List(Of JobNumberData) Implements IDTAUtilityDatabaseAccess.LoadJobNumberData
			' Stored Procedure
			Dim sql As String = String.Empty
			Select Case dtaJobType
				Case DTAUtilityDatabaseAccess.DtaJobTypeEnum.Dta
					sql = "[Show DTANr For Find DTADetail]"
				Case DTAUtilityDatabaseAccess.DtaJobTypeEnum.DtaLol
					sql = "[Show DTANr For Find DTALOLDetail]"
				Case Else
					Return Nothing
			End Select

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			Dim jobNumberData As List(Of DataObjects.JobNumberData) = Nothing
			Try
				If reader IsNot Nothing Then

					jobNumberData = New List(Of DataObjects.JobNumberData)

					While reader.Read
						Dim data = New DataObjects.JobNumberData
						data.VGNr = SafeGetInteger(reader, "VGNr", 0)
						data.DTADate = SafeGetDateTime(reader, "DTADate", Nothing)
						data.TotalBetrag = SafeGetDecimal(reader, "TotalBetrag", 0)
						data.RecAnzahl = SafeGetInteger(reader, "RecAnzahl", 0)

						jobNumberData.Add(data)
					End While
				End If
			Catch e As Exception
				m_Logger.LogError(e.ToString())
				jobNumberData = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return jobNumberData

		End Function

		Public Function LoadZGADataForDTAFileCreation(ByVal zgType As ZgTypeEnum, ByVal mdNr As Integer) As List(Of ZgData) Implements IDTAUtilityDatabaseAccess.LoadZGADataForDTAFileCreation
			' Stored Procedure
			Dim sql As String = String.Empty
			Select Case zgType
				Case ZgTypeEnum.CreatePain001FileSwiss, ZgTypeEnum.CreateDtaFileSwiss
					sql = "[Get AdvancePaymentData For Create DTAFile For Swiss]"
				Case ZgTypeEnum.CreateDtaFileForeign
					sql = "[Get AdvancePaymentData For Create DTAFile For Foreign]"
				Case ZgTypeEnum.CreateVg
					sql = "[Get AdvancePaymentData For Create VG For All]"
				Case Else
					Return Nothing
			End Select

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			Dim result As List(Of DataObjects.ZgData) = Nothing
			Try
				If reader IsNot Nothing Then

					result = New List(Of DataObjects.ZgData)

					While reader.Read
						Dim data = New DataObjects.ZgData

						data.ZGNr = SafeGetInteger(reader, "ZGNr", 0)
						data.MANr = SafeGetInteger(reader, "MANr", 0)
						data.Betrag = SafeGetDecimal(reader, "Betrag", Nothing)
						data.Currency = SafeGetString(reader, "Currency")
						data.ZGGrund = SafeGetString(reader, "ZGGrund")
						data.ClearingNr = SafeGetString(reader, "ClearingNr")
						data.KontoNr = SafeGetString(reader, "KontoNr")
						data.DTAAdr1 = SafeGetString(reader, "DTAAdr1")
						data.DTAAdr2 = SafeGetString(reader, "DTAAdr2")
						data.DTAAdr3 = SafeGetString(reader, "DTAAdr3")
						data.DTAAdr4 = SafeGetString(reader, "DTAAdr4")
						data.LONr = SafeGetInteger(reader, "LONr", 0)
						data.LP = SafeGetInteger(reader, "LP", 0)
						data.Jahr = SafeGetInteger(reader, "Jahr", 0)
						data.Bank = SafeGetString(reader, "Bank")
						data.BankOrt = SafeGetString(reader, "Bankort")
						data.Swift = SafeGetString(reader, "Swift")
						data.IBANNr = SafeGetString(reader, "IBANNr")
						data.EmployeeCountry = SafeGetString(reader, "EmployeeCountry")

						result.Add(data)
					End While
				End If
			Catch e As Exception
				m_Logger.LogError(e.ToString())
				result = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return result
		End Function

		Public Function LoadLolDataForDtaFileCreation(ByVal mdNr As Integer) As List(Of LolDataForDtaFileCreation) Implements IDTAUtilityDatabaseAccess.LoadLolDataForDtaFileCreation
			' Stored Procedure
			Dim sql As String = "[Get CreditorData For Create DTAFile For Swiss]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			Dim dataList As List(Of DataObjects.LolDataForDtaFileCreation) = Nothing
			Try
				If reader IsNot Nothing Then
					dataList = New List(Of DataObjects.LolDataForDtaFileCreation)

					While reader.Read
						Dim data = New DataObjects.LolDataForDtaFileCreation
						data.LOLID = SafeGetInteger(reader, "LOLID", 0)
						data.MANr = SafeGetInteger(reader, "MANr", Nothing)
						data.LANr = SafeGetDecimal(reader, "LANr", Nothing)
						data.RPText = SafeGetString(reader, "RPText")
						data.LONr = SafeGetInteger(reader, "LONr", Nothing)
						data.ZGGrund = SafeGetString(reader, "ZGGrund")
						data.LP = SafeGetInteger(reader, "LP", Nothing)
						data.Jahr = SafeGetInteger(reader, "Jahr", Nothing)
						data.Betrag = SafeGetDecimal(reader, "M_Btr", Nothing)
						data.Nachname = SafeGetString(reader, "Nachname")
						data.Vorname = SafeGetString(reader, "Vorname")
						data.BANK = SafeGetString(reader, "BANK")
						data.Swift = SafeGetString(reader, "Swift")
						data.BankOrt = SafeGetString(reader, "Bankort")
						data.DTABCNR = SafeGetString(reader, "DTABCNR")
						data.KONTONR = SafeGetString(reader, "KONTONR")
						data.DTAADR1 = SafeGetString(reader, "DTAADR1")
						data.DTAADR2 = SafeGetString(reader, "DTAADR2")
						data.DTAADR3 = SafeGetString(reader, "DTAADR3")
						data.DTAADR4 = SafeGetString(reader, "DTAADR4")
						data.IBANNr = SafeGetString(reader, "IBANNr")
						data.EmployeeCountry = SafeGetString(reader, "EmployeeCountry")

						dataList.Add(data)

					End While
				End If


			Catch e As Exception
				m_Logger.LogError(e.ToString())
				dataList = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return dataList
		End Function

		''' <summary>
		''' Load mandant and bank data for DTA file creation.
		''' </summary>
		''' <param name="mdNr">The mandant number.</param>
		''' <param name="year">The year.</param>
		''' <param name="bankID">The bank record number.</param>
		''' <returns>Mandant and bank data for DTA file creation.</returns>
		Public Function LoadMandantAndBankDataForDTAFileCreation(ByVal mdNr As Integer, ByVal year As Integer, ByVal bankID As Integer) As MandantAndBankDataForDTAFileCreation Implements IDTAUtilityDatabaseAccess.LoadMandantAndBankDataForDTAFileCreation

			Dim result As MandantAndBankDataForDTAFileCreation = Nothing

			Dim sql As String = String.Empty

			sql = sql & "SELECT MD.MDNr, MD.MD_Name1 As MDName, MD.Strasse As MDStrasse, (MD.PLZ + ' ' + MD.Ort) As MDPLZOrt, "
			sql = sql & "MD.DTANummer, MD.[Currency] As MDCurrency, "
			sql = sql & "MDBank.MD_ID As [DTA ID], MDBank.DTAClnr As [DTA BCNr], MDBank.KontoDTA As [DTA Konto], "
			sql = sql & "MDBank.KontoVG As [VG Konto], "
			sql = sql & "MDBank.DTAIBAN As [IBANDTA], MDBank.VGIBAN As [IBANVG], "
			sql = sql & "MDBank.DTAAdr1 As [DTA Name], MDBank.DTAAdr2 As [DTA Postfach], "
			sql = sql & "MDBank.DTAAdr3 As [DTA Strasse], MDBank.DTAAdr4 AS [DTA PLZOrt], "
			sql = sql & "MDBank.Swift As IID "
			sql = sql & "FROM Mandanten MD, MD_ESRDTA MDBank "
			sql = sql & "WHERE MD.Jahr = @MDYear AND MD.MDNr = @MDNR and MDBank.ModulArt = 0 And MDBank.ID = @ID "

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MDYear", year))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNR", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("@ID", bankID))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try
				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = New MandantAndBankDataForDTAFileCreation


					result.MDNr = SafeGetInteger(reader, "MDNr", 0)
					result.MDName = SafeGetString(reader, "MDName", String.Empty)
					result.MDStrasse = SafeGetString(reader, "MDStrasse", String.Empty)
					result.MDPLZOrt = SafeGetString(reader, "MDPLZOrt", String.Empty)
					result.DTANummer = SafeGetInteger(reader, "DTANummer", 0)
					result.MDCurrency = SafeGetString(reader, "MDCurrency", String.Empty)
					result.DTA_ID = SafeGetString(reader, "DTA ID", String.Empty)
					result.DTA_BCNr = SafeGetString(reader, "DTA BCNr", String.Empty)
					result.DTA_Konto = SafeGetString(reader, "DTA Konto", String.Empty)
					result.VG_Konto = SafeGetString(reader, "VG Konto", String.Empty)
					result.IBANDTA = SafeGetString(reader, "IBANDTA", String.Empty)
					result.IBANVG = SafeGetString(reader, "IBANVG", String.Empty)
					result.DTA_Name = SafeGetString(reader, "DTA Name", String.Empty)
					result.IID = SafeGetString(reader, "IID", String.Empty)
					result.DTA_Strasse = SafeGetString(reader, "DTA Strasse", String.Empty)
					result.DTA_PLZOrt = SafeGetString(reader, "DTA PLZOrt", String.Empty)
					result.DTA_Postfach = SafeGetString(reader, "DTA Postfach", String.Empty)

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
		''' Load ind ZG data.
		''' </summary>
		''' <param name="mdNr">The mandant number.</param>
		''' <param name="InlandBank">The inland bank number.</param>
		''' <returns>List of data or nothing in error case.</returns>
		Public Function LoadIndZGData(ByVal mdNr As Integer, ByVal InlandBank As Integer, ByVal zgNumbers As Integer()) As IEnumerable(Of ZgData) Implements IDTAUtilityDatabaseAccess.LoadIndZGData

			Dim result As List(Of DataObjects.ZgData) = Nothing

			Dim zgNumbersBuffer As String = String.Empty

			For Each number In zgNumbers

				zgNumbersBuffer = zgNumbersBuffer & IIf(zgNumbersBuffer <> "", ", ", "") & number

			Next

			Dim sql As String = String.Empty

			sql = sql & "Select ZG.ZGNr, ZG.MANr, ZG.LONr, ZG.Betrag, ZG.Currency, Convert(Int, ZG.LP) As LP, Convert(Int, ZG.Jahr) As Jahr, "
			sql = sql & "ZG.ZGGrund, ZG.ClearingNr, ZG.KontoNr, ZG.IBANNr, ZG.Bank, ZG.BankOrt, "
			sql = sql & "ZG.BLZ, ZG.Swift, ZG.DTAAdr1, ZG.DTAAdr2, ZG.DTAAdr3, ZG.DTAAdr4, "
			sql = sql & "MA.Nachname, MA.Vorname, MAL.NoZG, MAL.NoLO "
			sql = sql & "From ZG "
			sql = sql & "Left Join Mitarbeiter MA On ZG.MANr = MA.MANr "
			sql = sql & "Left Join MA_LOSetting MAL On ZG.MANr = MAL.MANr "
			sql = sql & "Where ZG.LANr = 8920 And ZG.MDNr = @mdNr "
			sql = sql & "AND (MAL.NoLO = 0 AND MAL.NoZG = 0) "

			If InlandBank = 1 Then
				sql = sql & "And (ZG.BnkAU = 0 Or ZG.BnkAU Is Null) "

			ElseIf InlandBank = 2 Then
				sql = sql & "And ZG.BnkAU = 1 And (ZG.Blz <> '' Or ZG.Blz Is Not Null) "
			End If
			sql = sql & "And ZG.VGNr = 0 And ZG.ZGNr In (" & zgNumbersBuffer & ") "
			sql = sql & "Order By ZG.ZGNr"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try
				If reader IsNot Nothing Then

					result = New List(Of DataObjects.ZgData)

					While reader.Read
						Dim data = New DataObjects.ZgData
						data.ZGNr = SafeGetInteger(reader, "ZGNr", 0)
						data.MANr = SafeGetInteger(reader, "MANr", 0)
						data.LONr = SafeGetInteger(reader, "LONr", 0)
						data.Betrag = SafeGetDecimal(reader, "Betrag", 0)
						data.Currency = SafeGetString(reader, "Currency")
						data.LP = SafeGetInteger(reader, "LP", 0)

						data.Jahr = SafeGetInteger(reader, "Jahr", 0)
						data.ZGGrund = SafeGetString(reader, "ZGGrund")
						data.ClearingNr = SafeGetInteger(reader, "ClearingNr", 0)
						data.KontoNr = SafeGetString(reader, "KontoNr")
						data.IBANNr = SafeGetString(reader, "IBANNr")
						data.Bank = SafeGetString(reader, "Bank")
						data.BankOrt = SafeGetString(reader, "BankOrt")
						data.Swift = SafeGetString(reader, "Swift")
						data.DTAAdr1 = SafeGetString(reader, "DTAAdr1")
						data.DTAAdr2 = SafeGetString(reader, "DTAAdr2")
						data.DTAAdr3 = SafeGetString(reader, "DTAAdr3")
						data.DTAAdr4 = SafeGetString(reader, "DTAAdr4")
						data.Nachname = SafeGetString(reader, "Nachname")
						data.Vorname = SafeGetString(reader, "Vorname")
						data.NoZG = SafeGetBoolean(reader, "NoZG", False)
						data.NoLO = SafeGetBoolean(reader, "NoLO", False)

						result.Add(data)
					End While
				End If
			Catch e As Exception
				m_Logger.LogError(e.ToString())
				result = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		Public Function LoadZGADataForDTAList(ByVal mdNr As Integer, ByVal dtaNumbers As Integer()) As List(Of DtaDataForListing) Implements IDTAUtilityDatabaseAccess.LoadZGADataForDTAList

			Dim result As List(Of DataObjects.DtaDataForListing) = Nothing

			Dim sql As String = String.Empty

			Dim dtaNumbersBuffer As String = String.Empty

			For Each number In dtaNumbers

				dtaNumbersBuffer = dtaNumbersBuffer & IIf(dtaNumbersBuffer <> "", ", ", "") & number

			Next

			sql &= "Select ZG.ZGNr, ZG.MANr, ZG.LONr, ZG.Betrag, ZG.Currency, Convert(Int, ZG.LP) As LP, Convert(Int, ZG.Jahr) As Jahr, "
			sql &= "ZG.ZGGrund, ZG.ClearingNr, ZG.KontoNr, ZG.IBANNr, ZG.Bank, ZG.BankOrt, "
			sql &= "ZG.BLZ, ZG.Swift, ZG.DTAAdr1, ZG.DTAAdr2, ZG.DTAAdr3, ZG.DTAAdr4, "
			sql &= "MA.Nachname, MA.Vorname, MA.Strasse, MA.PLZ, MA.Ort, MA.Land "
			sql &= "From ZG "
			sql &= "Left Join Mitarbeiter MA On ZG.MANr = MA.MANr "
			sql &= "Where ZG.MDNr = @mdNr And ZG.VGNr In (" & dtaNumbersBuffer & ") "
			sql &= "Order By MA.Nachname, MA.Vorname, VGNr"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try
				If reader IsNot Nothing Then

					result = New List(Of DataObjects.DtaDataForListing)

					While reader.Read
						Dim data = New DataObjects.DtaDataForListing

						data.ZGNr = SafeGetInteger(reader, "ZGNr", 0)
						data.MANr = SafeGetInteger(reader, "MANr", 0)
						data.LONr = SafeGetInteger(reader, "LONr", 0)
						data.Betrag = SafeGetDecimal(reader, "Betrag", 0)
						data.Currency = SafeGetString(reader, "Currency")
						data.LP = SafeGetInteger(reader, "LP", 0)
						data.Jahr = SafeGetInteger(reader, "Jahr", 0)
						data.ZGGrund = SafeGetString(reader, "ZGGrund")
						data.ClearingNr = SafeGetInteger(reader, "ClearingNr", 0)
						data.KontoNr = SafeGetString(reader, "KontoNr")
						data.IBANNr = SafeGetString(reader, "IBANNr")
						data.Bank = SafeGetString(reader, "Bank")
						data.BankOrt = SafeGetString(reader, "BankOrt")
						data.Swift = SafeGetString(reader, "Swift")

						data.DTAAdr1 = SafeGetString(reader, "DTAAdr1")
						data.DTAAdr2 = SafeGetString(reader, "DTAAdr2")
						data.DTAAdr3 = SafeGetString(reader, "DTAAdr3")
						data.DTAAdr4 = SafeGetString(reader, "DTAAdr4")

						data.Nachname = SafeGetString(reader, "Nachname")
						data.Vorname = SafeGetString(reader, "Vorname")
						data.Strasse = SafeGetString(reader, "Strasse")
						data.PLZ = SafeGetString(reader, "PLZ")
						data.Ort = SafeGetString(reader, "Ort")
						data.Land = SafeGetString(reader, "Land")
						data.EmployeeCountry = SafeGetString(reader, "Land")

						result.Add(data)

					End While

				End If

			Catch e As Exception
				m_Logger.LogError(e.ToString())
				result = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function


		''' <summary>
		''' Gets a new DtaNr.
		''' </summary>
		''' <param name="offset">The offset.</param>
		''' <returns>The data number.</returns>
		Function GetNewDtaNr(ByVal offset As Integer) As Integer? Implements IDTAUtilityDatabaseAccess.GetNewDtaNr

			Dim result As Integer? = Nothing

			Dim sSql As String

			' Lohnabrechnungsnummer in der Mandantenverwaltung...

			sSql = "SELECT ISNULL((SELECT MAX(zg.vgnr) FROM dbo.ZG), 0) AS ZGVGNr, "
			sSql &= "ISNULL((SELECT MAX(LOL.vgnr) FROM dbo.LOL), 0) AS LOLVGNr "

			Dim reader As SqlClient.SqlDataReader = OpenReader(sSql, Nothing)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					Dim ZGVGNr As Integer = SafeGetInteger(reader, "ZGVGNr", 0)
					Dim LOLVGNr As Integer = SafeGetInteger(reader, "LOLVGNr", 0)

					result = Math.Max(ZGVGNr, LOLVGNr) + 1
					result = Math.Max(offset + 1, result.Value)
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
		''' Updates the ZG record for DTA file.
		''' </summary>
		''' <param name="zgNr">The zgNr.</param>
		''' <param name="dtaNr">The dtaNr.</param>
		''' <param name="dtadate">The dtadate.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Function UpdateZGrecForDTAFile(ByVal zgNr As Integer, ByVal dtaNr As Integer?, ByVal dtadate As DateTime) As Boolean Implements IDTAUtilityDatabaseAccess.UpdateZGrecForDTAFile

			Dim success = True

			Dim sql As String = String.Empty

			sql = sql & "[Update AdvancePaymentrec For DTAFile]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("@ZGNr", ReplaceMissing(zgNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@DTANR  ", ReplaceMissing(dtaNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@DTADate", ReplaceMissing(dtadate, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Return success

		End Function

		''' <summary>
		''' Updates the LOL record for DTA file.
		''' </summary>
		''' <param name="RecID">The ID in LOL.</param>
		''' <param name="dtaNr">The dtaNr.</param>
		''' <param name="dtadate">The dtadate.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Function UpdateLOLrecForDTAFile(ByVal recID As Integer, ByVal dtaNr As Integer?, ByVal dtadate As DateTime) As Boolean Implements IDTAUtilityDatabaseAccess.UpdateLOLrecForDTAFile

			Dim success = True

			Dim sql As String = String.Empty

			sql = sql & "[Update Creditorrec For DTAFile]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("@RecID", ReplaceMissing(recID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@DTANR  ", ReplaceMissing(dtaNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@DTADate", ReplaceMissing(dtadate, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Return success

		End Function


		Public Function SetZgOrderDeleted(ByVal vgNoArray As Integer()) As Boolean Implements IDTAUtilityDatabaseAccess.SetZgOrderDeleted

			Dim success As Boolean = False

			Dim vgNoString As String = String.Join(",", (
			From vgNo In vgNoArray
			Select vgNo.ToString("0")
			))
			Dim sql As String = "UPDATE ZG SET VGNr = 0 WHERE VGNr IN (" + vgNoString + ")"
			Try
				success = ExecuteNonQuery(sql, parameters:=Nothing, commandType:=CommandType.Text)
			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
			End Try
			Return success

		End Function

		Public Function SetLolOrderDeleted(ByVal vgNoArray As Integer()) As Boolean Implements IDTAUtilityDatabaseAccess.SetLolOrderDeleted

			Dim success As Boolean = False

			Dim vgNoString As String = String.Join(",", (
			From vgNo In vgNoArray
			Select vgNo.ToString("0")
			))
			Dim sql As String = "UPDATE LOL SET VGNr = 0, DTADate = Null WHERE VGNr IN (" + vgNoString + ")"
			Try
				success = ExecuteNonQuery(sql, parameters:=Nothing, commandType:=CommandType.Text)
			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
			End Try
			Return success

		End Function

#End Region

	End Class

End Namespace
