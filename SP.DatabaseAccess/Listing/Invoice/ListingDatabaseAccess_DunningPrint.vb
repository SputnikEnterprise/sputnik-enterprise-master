
Imports SP.DatabaseAccess.Listing.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language

Namespace Listing


	Partial Class ListingDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IListingDatabaseAccess


		Public Function LoadDunningData(ByVal mdNr As Integer, ByVal dunningLevel As Integer, ByVal dunningDate As Date,
																		ByVal orderbyEnum As OrderByValue) As IEnumerable(Of DunningPrintData) Implements IListingDatabaseAccess.LoadDunningData

			Dim result As List(Of DunningPrintData) = Nothing

			Dim sql As String

			sql = "Select RE.MDNr"
			sql &= ",RE.KDNr"
			sql &= ",Sum(RE.BetragInk) BetragTotal"
			sql &= ",RE.R_Name1"
			sql &= ",RE.R_PLZ"
			sql &= ",RE.R_Ort"
			sql &= ",RE.R_Strasse"
			sql &= ",ISNULL( (SELECT TOP 1 S.SPNr FROM dbo.RE_Sp S WHERE S.KDNR = RE.KDNr AND S.SP_Dat = @dunningDate), 0) SPNr"
			sql &= ",IsNull(KD.Sprache, 'D') Sprache"

			sql &= " From RE"
			sql &= " Left Join Kunden KD On RE.KDNr = KD.KDNr"

			sql &= " Where RE.MDNr = @mdNr"
			sql &= " And RE.BetragInk <> 0"
			sql &= String.Format(" And RE.ma{0} = @dunningDate", dunningLevel)
			sql &= " And RE.BetragInk > RE.Bezahlt"
			sql &= " Group By"
			sql &= " RE.MDNr"
			sql &= ",RE.KDNr"
			sql &= ",RE.R_Name1"
			sql &= ",RE.R_PLZ"
			sql &= ",RE.R_Ort"
			sql &= ",RE.R_Strasse"
			sql &= ",KD.Sprache"
			sql &= " Order By RE.R_Name1"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("dunningDate", ReplaceMissing(dunningDate, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try
				If reader IsNot Nothing Then

					result = New List(Of DunningPrintData)

					While reader.Read
						Dim data = New DunningPrintData

						data.MDNr = SafeGetInteger(reader, "MDNr", 0)
						data.KDNr = SafeGetInteger(reader, "KDNr", 0)
						data.RName1 = SafeGetString(reader, "R_Name1")
						data.rStrasse = SafeGetString(reader, "R_Strasse")
						data.rplz = SafeGetString(reader, "R_PLZ")
						data.rort = SafeGetString(reader, "R_Ort")
						data.BetragTotal = SafeGetDecimal(reader, "BetragTotal", 0)
						data.SPNr = SafeGetInteger(reader, "SPNr", 0)
						data.VerNr = 0

						data.CustomerLanguage = Mid(SafeGetString(reader, "Sprache"), 1, 1).ToUpper


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

		Public Function LoadAssignedDunningDetailData(ByVal mdNr As Integer, ByVal customerNumber As Integer, ByVal dunningLevel As Integer, ByVal rName1 As String, ByVal dunningDate As Date) As IEnumerable(Of InvoiceDunningPrintData) Implements IListingDatabaseAccess.LoadAssignedDunningDetailData

			Dim result As List(Of InvoiceDunningPrintData) = Nothing

			Dim sql As String

			sql = "Select RE.*"
			sql &= ",KD.Sprache"
			sql &= " From RE"
			sql &= " Left Join Kunden KD On RE.KDNr = KD.KDNr"
			sql &= " Where RE.MDNr = @mdNr"

			sql &= " And RE.KDNr = @kdNr"
			sql &= " And RE.R_Name1 = @RName1"
			sql &= " And RE.BetragInk <> 0"
			sql &= String.Format(" And RE.ma{0} = @dunningDate", dunningLevel)
			sql &= " And RE.BetragInk > RE.Bezahlt"
			sql &= " Order By RE.Fak_dat Desc"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("kdNr", ReplaceMissing(customerNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("RName1", ReplaceMissing(rName1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("dunningDate", ReplaceMissing(dunningDate, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try
				If reader IsNot Nothing Then

					result = New List(Of InvoiceDunningPrintData)

					While reader.Read
						Dim data = New InvoiceDunningPrintData

						data.ReNr = SafeGetInteger(reader, "RENR", Nothing)
						data.KdNr = SafeGetInteger(reader, "KDNR", Nothing)
						data.Art = SafeGetString(reader, "ART")
						data.KST = SafeGetString(reader, "KST")
						data.Lp = SafeGetInteger(reader, "LP", Nothing)
						data.FakDat = SafeGetDateTime(reader, "FAK_DAT", Nothing)
						data.Currency = SafeGetString(reader, "Currency")
						data.BetragOhne = SafeGetDecimal(reader, "BetragOhne", 0)
						data.BetragEx = SafeGetDecimal(reader, "BetragEx", 0)
						data.BetragInk = SafeGetDecimal(reader, "BetragInk", 0)
						data.MWST1 = SafeGetDecimal(reader, "MWST1", 0)
						data.MWSTProz = SafeGetDecimal(reader, "MWSTProz", Nothing)
						data.Bezahlt = SafeGetDecimal(reader, "Bezahlt", 0)
						data.SKonto = SafeGetDecimal(reader, "SKonto", Nothing)
						data.Verlust = SafeGetDecimal(reader, "Verlust", Nothing)
						data.FSKonto = SafeGetDecimal(reader, "FSKonto", Nothing)
						data.FVerlust = SafeGetDecimal(reader, "FVerlust", Nothing)
						data.Faellig = SafeGetDateTime(reader, "Faellig", Nothing)
						data.Mahncode = SafeGetString(reader, "Mahncode")
						data.SPNr = SafeGetInteger(reader, "SPNr", Nothing)
						data.VerNr = SafeGetInteger(reader, "VerNr", Nothing)
						data.MA0 = SafeGetDateTime(reader, "MA0", Nothing)
						data.MA1 = SafeGetDateTime(reader, "MA1", Nothing)
						data.MA2 = SafeGetDateTime(reader, "MA2", Nothing)
						data.MA3 = SafeGetDateTime(reader, "MA3", Nothing)
						data.Storno = SafeGetBoolean(reader, "Storno", False)
						data.Gebucht = SafeGetBoolean(reader, "Gebucht", False)
						data.FBMonat = SafeGetShort(reader, "FBMonat", Nothing)
						data.FBDat = SafeGetDateTime(reader, "FBDat", Nothing)
						data.FKSoll = SafeGetInteger(reader, "FKSoll", Nothing)
						data.FKHaben0 = SafeGetInteger(reader, "FKHaben0", Nothing)
						data.FKHaben1 = SafeGetInteger(reader, "FKHaben1", Nothing)
						data.RName1 = SafeGetString(reader, "R_Name1")
						data.RName2 = SafeGetString(reader, "R_Name2")
						data.RName3 = SafeGetString(reader, "R_Name3")
						data.CustomerLanguage = SafeGetString(reader, "Sprache")
						data.RZHD = SafeGetString(reader, "R_ZHD")
						data.RPostfach = SafeGetString(reader, "R_Postfach")
						data.RStrasse = SafeGetString(reader, "R_Strasse")
						data.RLand = SafeGetString(reader, "R_Land")
						data.RPLZ = SafeGetString(reader, "R_PLZ")
						data.ROrt = SafeGetString(reader, "R_Ort")
						data.Zahlkond = SafeGetString(reader, "Zahlkond")
						data.Result = SafeGetString(reader, "Result")
						data.RefNr = SafeGetString(reader, "RefNr")
						data.RefFootNr = SafeGetString(reader, "RefFootNr")
						data.ESRArt = SafeGetString(reader, "ESRArt")
						data.ESRID = SafeGetString(reader, "ESRID")
						data.ESRKonto = SafeGetString(reader, "ESRKonto")
						data.MWSTNr = SafeGetString(reader, "MWSTNr")
						data.KontoNr = SafeGetString(reader, "KontoNr")
						data.BtrFr = SafeGetInteger(reader, "BtrFr", Nothing)
						data.btrRp = SafeGetString(reader, "btrRp")
						data.REKST1 = SafeGetString(reader, "REKST1")
						data.REKST2 = SafeGetString(reader, "REKST2")
						data.PrintedDate = SafeGetString(reader, "PrintedDate")
						data.GebuchtAm = SafeGetDateTime(reader, "GebuchtAm", Nothing)
						data.ZEInfo = SafeGetString(reader, "ZEInfo")
						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						data.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
						data.ChangedFrom = SafeGetString(reader, "ChangedFrom")
						data.RAbteilung = SafeGetString(reader, "R_Abteilung")
						data.Ma3RepeatNr = SafeGetInteger(reader, "Ma3_RepeatNr", Nothing)
						data.EsEinstufung = SafeGetString(reader, "ES_Einstufung")
						data.KDBranche = SafeGetString(reader, "KDBranche")
						data.DTAName = SafeGetString(reader, "DTA Name")
						data.DTAPLZOrt = SafeGetString(reader, "DTA PLZOrt")
						data.ESRBankName = SafeGetString(reader, "ESR BankName")
						data.ESRBankAdresse = SafeGetString(reader, "ESR BankAdresse")
						data.DTAKonto = SafeGetString(reader, "DTA Konto")
						data.IBANDTA = SafeGetString(reader, "IBANDTA")
						data.IBANVG = SafeGetString(reader, "IBANVG")
						data.EsrSwift = SafeGetString(reader, "ESR_Swift")
						data.EsrIBAN1 = SafeGetString(reader, "ESR_IBAN1")
						data.EsrIBAN2 = SafeGetString(reader, "ESR_IBAN2")
						data.EsrIBAN3 = SafeGetString(reader, "ESR_IBAN3")
						data.EsrBcNr = SafeGetString(reader, "ESR_BcNr")
						data.ProposeNr = SafeGetInteger(reader, "ProposeNr", Nothing)
						data.Art2 = SafeGetString(reader, "Art_2", "")
						data.MahnStopUntil = SafeGetDateTime(reader, "MahnStopUntil", Nothing)
						data.REDoc_Guid = SafeGetString(reader, "REDoc_Guid")
						data.Transfered_User = SafeGetString(reader, "Transfered_User")
						data.Transfered_On = SafeGetString(reader, "Transfered_On")
						data.ZEBis0 = SafeGetDateTime(reader, "ZEBis_0", Nothing)
						data.ZEBis1 = SafeGetDateTime(reader, "ZEBis_1", Nothing)
						data.ZEBis2 = SafeGetDateTime(reader, "ZEBis_2", Nothing)
						data.ZEBis3 = SafeGetDateTime(reader, "ZEBis_3", Nothing)
						data.MDNr = SafeGetInteger(reader, "MDNr", Nothing)


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

		Public Function LoadAssignedCustomerDunningPrintData(ByVal mdNr As Integer, ByVal dunningData As DunningPrintData, ByVal printNotDoneCredits As Boolean?, ByVal dunningLevel As Integer, ByVal dunningDate As Date
																				) As IEnumerable(Of InvoiceDunningPrintData) Implements IListingDatabaseAccess.LoadAssignedCustomerDunningPrintData

			Dim result As List(Of InvoiceDunningPrintData) = Nothing

			Dim sql As String

			sql = "Select RE.* "
			sql &= ",KD.Sprache "

			sql &= " From RE"
			sql &= " Left Join Kunden KD On RE.KDNr = KD.KDNr"
			sql &= " Where RE.MDNr = @mdNr"
			sql &= " And RE.KDNr = @kdNr"
			sql &= " And RE.R_Name1 = @RName1"
			sql &= " And RE.BetragInk <> 0 "
			sql &= String.Format(" And RE.ma{0} = @dunningDate ", dunningLevel)
			sql &= " And RE.BetragInk > RE.Bezahlt "
			If Not printNotDoneCredits Then
				sql &= " And RE.Art <> 'G'"
			End If
			sql &= " Order By RE.RENr"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("kdNr", ReplaceMissing(dunningData.KDNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("RName1", ReplaceMissing(dunningData.RName1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("dunningDate", ReplaceMissing(dunningDate, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try
				If reader IsNot Nothing Then

					result = New List(Of InvoiceDunningPrintData)

					While reader.Read
						Dim data = New InvoiceDunningPrintData

						data.ReNr = SafeGetInteger(reader, "RENR", Nothing)
						data.KdNr = SafeGetInteger(reader, "KDNR", Nothing)
						data.Art = SafeGetString(reader, "ART")
						data.KST = SafeGetString(reader, "KST")
						data.Lp = SafeGetInteger(reader, "LP", Nothing)
						data.FakDat = SafeGetDateTime(reader, "FAK_DAT", Nothing)
						data.Currency = SafeGetString(reader, "Currency")
						data.BetragOhne = SafeGetDecimal(reader, "BetragOhne", 0)
						data.BetragEx = SafeGetDecimal(reader, "BetragEx", 0)
						data.BetragInk = SafeGetDecimal(reader, "BetragInk", 0)
						data.MWST1 = SafeGetDecimal(reader, "MWST1", 0)
						data.MWSTProz = SafeGetDecimal(reader, "MWSTProz", Nothing)
						data.Bezahlt = SafeGetDecimal(reader, "Bezahlt", 0)
						data.SKonto = SafeGetDecimal(reader, "SKonto", Nothing)
						data.Verlust = SafeGetDecimal(reader, "Verlust", Nothing)
						data.FSKonto = SafeGetDecimal(reader, "FSKonto", Nothing)
						data.FVerlust = SafeGetDecimal(reader, "FVerlust", Nothing)
						data.Faellig = SafeGetDateTime(reader, "Faellig", Nothing)
						data.Mahncode = SafeGetString(reader, "Mahncode")
						data.SPNr = SafeGetInteger(reader, "SPNr", Nothing)
						data.VerNr = SafeGetInteger(reader, "VerNr", Nothing)
						data.MA0 = SafeGetDateTime(reader, "MA0", Nothing)
						data.MA1 = SafeGetDateTime(reader, "MA1", Nothing)
						data.MA2 = SafeGetDateTime(reader, "MA2", Nothing)
						data.MA3 = SafeGetDateTime(reader, "MA3", Nothing)
						data.Storno = SafeGetBoolean(reader, "Storno", False)
						data.Gebucht = SafeGetBoolean(reader, "Gebucht", False)
						data.FBMonat = SafeGetShort(reader, "FBMonat", Nothing)
						data.FBDat = SafeGetDateTime(reader, "FBDat", Nothing)
						data.FKSoll = SafeGetInteger(reader, "FKSoll", Nothing)
						data.FKHaben0 = SafeGetInteger(reader, "FKHaben0", Nothing)
						data.FKHaben1 = SafeGetInteger(reader, "FKHaben1", Nothing)
						data.RName1 = SafeGetString(reader, "R_Name1")
						data.RName2 = SafeGetString(reader, "R_Name2")
						data.RName3 = SafeGetString(reader, "R_Name3")
						data.CustomerLanguage = SafeGetString(reader, "Sprache")
						data.RZHD = SafeGetString(reader, "R_ZHD")
						data.RPostfach = SafeGetString(reader, "R_Postfach")
						data.RStrasse = SafeGetString(reader, "R_Strasse")
						data.RLand = SafeGetString(reader, "R_Land")
						data.RPLZ = SafeGetString(reader, "R_PLZ")
						data.ROrt = SafeGetString(reader, "R_Ort")
						data.Zahlkond = SafeGetString(reader, "Zahlkond")
						data.Result = SafeGetString(reader, "Result")
						data.RefNr = SafeGetString(reader, "RefNr")
						data.RefFootNr = SafeGetString(reader, "RefFootNr")
						data.ESRArt = SafeGetString(reader, "ESRArt")
						data.ESRID = SafeGetString(reader, "ESRID")
						data.ESRKonto = SafeGetString(reader, "ESRKonto")
						data.MWSTNr = SafeGetString(reader, "MWSTNr")
						data.KontoNr = SafeGetString(reader, "KontoNr")
						data.BtrFr = SafeGetInteger(reader, "BtrFr", Nothing)
						data.btrRp = SafeGetString(reader, "btrRp")
						data.REKST1 = SafeGetString(reader, "REKST1")
						data.REKST2 = SafeGetString(reader, "REKST2")
						data.PrintedDate = SafeGetString(reader, "PrintedDate")
						data.GebuchtAm = SafeGetDateTime(reader, "GebuchtAm", Nothing)
						data.ZEInfo = SafeGetString(reader, "ZEInfo")
						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						data.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
						data.ChangedFrom = SafeGetString(reader, "ChangedFrom")
						data.RAbteilung = SafeGetString(reader, "R_Abteilung")
						data.Ma3RepeatNr = SafeGetInteger(reader, "Ma3_RepeatNr", Nothing)
						data.EsEinstufung = SafeGetString(reader, "ES_Einstufung")
						data.KDBranche = SafeGetString(reader, "KDBranche")
						data.DTAName = SafeGetString(reader, "DTA Name")
						data.DTAPLZOrt = SafeGetString(reader, "DTA PLZOrt")
						data.ESRBankName = SafeGetString(reader, "ESR BankName")
						data.ESRBankAdresse = SafeGetString(reader, "ESR BankAdresse")
						data.DTAKonto = SafeGetString(reader, "DTA Konto")
						data.IBANDTA = SafeGetString(reader, "IBANDTA")
						data.IBANVG = SafeGetString(reader, "IBANVG")
						data.EsrSwift = SafeGetString(reader, "ESR_Swift")
						data.EsrIBAN1 = SafeGetString(reader, "ESR_IBAN1")
						data.EsrIBAN2 = SafeGetString(reader, "ESR_IBAN2")
						data.EsrIBAN3 = SafeGetString(reader, "ESR_IBAN3")
						data.EsrBcNr = SafeGetString(reader, "ESR_BcNr")
						data.ProposeNr = SafeGetInteger(reader, "ProposeNr", Nothing)
						data.Art2 = SafeGetString(reader, "Art_2", "")
						data.MahnStopUntil = SafeGetDateTime(reader, "MahnStopUntil", Nothing)
						data.REDoc_Guid = SafeGetString(reader, "REDoc_Guid")
						data.Transfered_User = SafeGetString(reader, "Transfered_User")
						data.Transfered_On = SafeGetString(reader, "Transfered_On")
						data.ZEBis0 = SafeGetDateTime(reader, "ZEBis_0", Nothing)
						data.ZEBis1 = SafeGetDateTime(reader, "ZEBis_1", Nothing)
						data.ZEBis2 = SafeGetDateTime(reader, "ZEBis_2", Nothing)
						data.ZEBis3 = SafeGetDateTime(reader, "ZEBis_3", Nothing)
						data.MDNr = SafeGetInteger(reader, "MDNr", Nothing)


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

		Public Function LoadAssignedCustomerDunningWithFeesPrintData(ByVal mdNr As Integer, ByVal dunningData As DunningPrintData, ByVal printNotDoneCredits As Boolean?, ByVal dunningLevel As Integer, ByVal dunningDate As Date
																				) As IEnumerable(Of InvoiceDunningPrintData) Implements IListingDatabaseAccess.LoadAssignedCustomerDunningWithFeesPrintData

			Dim result As List(Of InvoiceDunningPrintData) = Nothing

			Dim sql As String

			sql = "Select"
			sql &= " S.SP_Text"
			sql &= ",S.OP_BetragEx"
			sql &= ",S.OP_BetragInk"
			sql &= ",S.SP_Betrag"
			sql &= ",S.MWSTProz"
			sql &= ",S.SP_BetragTotal"
			sql &= ",S.SP_Bezahlt"
			sql &= ",S.RefNr"
			sql &= ",S.RefFootNr"
			sql &= ",S.ESRID"
			sql &= ",S.ESRKonto"
			sql &= ",S.MWSTNr"
			sql &= ",S.KontoNr"
			sql &= ",S.BtrFr"
			sql &= ",Convert(Int, S.btrRp) btrRp"

			sql &= ",RE.MDNr"
			sql &= ",RE.RENr"
			sql &= ",RE.KDNr"
			sql &= ",RE.Fak_Dat"
			sql &= ",RE.Faellig"
			sql &= ",RE.R_Name1"
			sql &= ",RE.R_Name2"
			sql &= ",RE.R_Name3"
			sql &= ",RE.R_Postfach"
			sql &= ",RE.R_Strasse"
			sql &= ",RE.R_ZHD"
			sql &= ",RE.R_Abteilung"
			sql &= ",RE.R_PLZ"
			sql &= ",RE.R_Ort"
			sql &= ",RE.R_Land"
			sql &= ",RE.ZahlKond"
			sql &= ",RE.Bezahlt"
			sql &= ",RE.MA0"
			sql &= ",RE.MA1"
			sql &= ",RE.MA2"
			sql &= ",RE.MA3"
			sql &= ",RE.BetragInk"
			sql &= ",RE.BetragEx"
			sql &= ",RE.Bezahlt"
			sql &= ",RE.ZEBis_0"
			sql &= ",RE.ZEBis_1"
			sql &= ",RE.ZEBis_2"
			sql &= ",RE.ZEBis_3"
			sql &= ",RE.[DTA PLZOrt]"
			sql &= ",RE.[DTA Name]"
			sql &= ",RE.[ESR BankName]"
			sql &= ",RE.[ESR BankAdresse]"
			sql &= ",RE.[DTA Konto]"
			sql &= ",RE.IBANDTA"
			sql &= ",RE.IBANVG"
			sql &= ",RE.ESR_Swift"
			sql &= ",RE.ESR_IBAN1"
			sql &= ",RE.ESR_IBAN2"
			sql &= ",RE.ESR_IBAN3"
			sql &= ",RE.ESR_BcNr"

			sql &= ",KD.Sprache"

			sql &= " From RE_SP S"
			sql &= " Left Join RE On S.RENr = RE.RENr And S.KDNr = RE.KDNr"
			sql &= " Left Join Kunden KD On RE.KDNr = KD.KDNr"
			sql &= " Where RE.MDNr = @mdNr"
			sql &= " And RE.KDNr = @kdNr"
			sql &= " And RE.R_Name1 = @RName1"
			sql &= " And RE.BetragInk > RE.Bezahlt"
			sql &= " And RE.BetragInk <> 0"
			sql &= String.Format(" And RE.ma{0} = @dunningDate", dunningLevel)
			sql &= " And S.SP_Dat = @dunningDate"
			If Not printNotDoneCredits Then
				sql &= " And RE.Art <> 'G'"
			End If
			sql &= " Order By RE.RENr, S.SPNr, S.SP_Dat"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("kdNr", ReplaceMissing(dunningData.KDNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("RName1", ReplaceMissing(dunningData.RName1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("dunningDate", ReplaceMissing(dunningDate, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try
				If reader IsNot Nothing Then

					result = New List(Of InvoiceDunningPrintData)

					While reader.Read
						Dim data = New InvoiceDunningPrintData

						data.ReNr = SafeGetInteger(reader, "RENR", Nothing)
						data.KdNr = SafeGetInteger(reader, "KDNR", Nothing)
						data.FakDat = SafeGetDateTime(reader, "FAK_DAT", Nothing)
						data.Faellig = SafeGetDateTime(reader, "Faellig", Nothing)
						data.BetragEx = SafeGetDecimal(reader, "BetragEx", 0)
						data.BetragInk = SafeGetDecimal(reader, "BetragInk", 0)
						data.Bezahlt = SafeGetDecimal(reader, "Bezahlt", 0)
						data.MA0 = SafeGetDateTime(reader, "MA0", Nothing)
						data.MA1 = SafeGetDateTime(reader, "MA1", Nothing)
						data.MA2 = SafeGetDateTime(reader, "MA2", Nothing)
						data.MA3 = SafeGetDateTime(reader, "MA3", Nothing)
						data.RName1 = SafeGetString(reader, "R_Name1")
						data.RName2 = SafeGetString(reader, "R_Name2")
						data.RName3 = SafeGetString(reader, "R_Name3")
						data.CustomerLanguage = SafeGetString(reader, "Sprache")
						data.RZHD = SafeGetString(reader, "R_ZHD")
						data.RPostfach = SafeGetString(reader, "R_Postfach")
						data.RStrasse = SafeGetString(reader, "R_Strasse")
						data.RLand = SafeGetString(reader, "R_Land")
						data.RPLZ = SafeGetString(reader, "R_PLZ")
						data.ROrt = SafeGetString(reader, "R_Ort")
						data.Zahlkond = SafeGetString(reader, "Zahlkond")
						data.RAbteilung = SafeGetString(reader, "R_Abteilung")
						data.ZEBis0 = SafeGetDateTime(reader, "ZEBis_0", Nothing)
						data.ZEBis1 = SafeGetDateTime(reader, "ZEBis_1", Nothing)
						data.ZEBis2 = SafeGetDateTime(reader, "ZEBis_2", Nothing)
						data.ZEBis3 = SafeGetDateTime(reader, "ZEBis_3", Nothing)
						data.MDNr = SafeGetInteger(reader, "MDNr", Nothing)

						data.DTAName = SafeGetString(reader, "DTA Name")
						data.DTAPLZOrt = SafeGetString(reader, "DTA PLZOrt")
						data.ESRBankName = SafeGetString(reader, "ESR BankName")
						data.ESRBankAdresse = SafeGetString(reader, "ESR BankAdresse")
						data.DTAKonto = SafeGetString(reader, "DTA Konto")
						data.IBANDTA = SafeGetString(reader, "IBANDTA")
						data.IBANVG = SafeGetString(reader, "IBANVG")
						data.EsrSwift = SafeGetString(reader, "ESR_Swift")
						data.EsrIBAN1 = SafeGetString(reader, "ESR_IBAN1")
						data.EsrIBAN2 = SafeGetString(reader, "ESR_IBAN2")
						data.EsrIBAN3 = SafeGetString(reader, "ESR_IBAN3")
						data.EsrBcNr = SafeGetString(reader, "ESR_BcNr")



						data.SPText = SafeGetString(reader, "SP_Text")
						data.OPBetragEx = SafeGetDecimal(reader, "OP_BetragEx", 0)
						data.OPBetragInk = SafeGetDecimal(reader, "OP_BetragInk", 0)
						data.SPBetrag = SafeGetDecimal(reader, "SP_Betrag", 0)
						data.SPMwStProz = SafeGetDecimal(reader, "MWSTProz", 0)
						data.SPBetragTotal = SafeGetDecimal(reader, "SP_BetragTotal", 0)
						data.SPBezahlt = SafeGetBoolean(reader, "SP_Bezahlt", False)
						data.SPRefNr = SafeGetString(reader, "RefNr")
						data.SPRefFootNr = SafeGetString(reader, "RefFootNr")
						data.SPESRID = SafeGetString(reader, "ESRID")
						data.SPESRKonto = SafeGetString(reader, "ESRKonto")
						data.SPMwStNr = SafeGetString(reader, "MWSTNr")
						data.SPKontoNr = SafeGetString(reader, "KontoNr")
						data.SPBtrFr = SafeGetInteger(reader, "BtrFr", 0)
						data.SPBtrRr = SafeGetInteger(reader, "btrRp", 0)


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

		Public Function LoadInvoiceForAssignedDunningData(ByVal mdNr As Integer, ByVal dunningData As DunningPrintData, ByVal dunningLevel As Integer, ByVal dunningDate As Date) As IEnumerable(Of InvoiceData) Implements IListingDatabaseAccess.LoadInvoiceForAssignedDunningData

			Dim result As List(Of InvoiceData) = Nothing

			Dim sql As String

			sql = "Select RE.MDNr"
			sql &= ",RE.ID"
			sql &= ",RE.Art"
			sql &= ",RE.RENr"
			sql &= ",RE.KDNr"
			sql &= ",RE.RefNr"
			sql &= ",RE.Fak_Dat"
			sql &= ",RE.BetragInk"
			sql &= ",RE.Bezahlt"
			sql &= ",RE.R_Name1"
			sql &= ",RE.CreatedOn"
			sql &= ",RE.CreatedFrom"
			sql &= ",IsNull(RE.REDoc_Guid, '') DocGuid"

			sql &= ",IsNull(KD.Sprache, 'D') Sprache"
			sql &= ",IsNull(KD.Send2WOS, 0) As Send2WOS"
			sql &= ",IsNull(KD.Transfered_Guid, '') CustomerGuid"
			sql &= ",CONVERT(BIT,("
			sql &= " CASE"
			sql &= " WHEN ISNULL(kd.OPVersand, '') LIKE '99%' THEN 1"
			sql &= " ELSE 0"
			sql &= " END"
			sql &= "), 0) PrintWithReport"

			sql &= ",CONVERT(BIT,("
			sql &= " CASE"
			sql &= " WHEN ISNULL(RE.Art, '') = 'G' THEN"
			sql &= " ( CASE"
			sql &= " WHEN EXISTS (Select Top 1 I.ID From RE_Ind I Where I.RENr = RE.RENr) THEN 0"
			sql &= " ELSE 1"
			sql &= " END)"

			sql &= " ELSE 0"
			sql &= " END"
			sql &= "), 0) CreditInvoiceAutomated"

			sql &= " From RE "
			sql &= " Left Join Kunden KD On RE.KDNr = KD.KDNr "

			sql &= " Where RE.MDNr = @mdNr"
			sql &= " And RE.BetragInk <> 0"
			sql &= " And RE.BetragInk > Bezahlt"
			sql &= String.Format(" And RE.ma{0} = @dunningDate", dunningLevel)
			sql &= " And RE.R_Name1 = @RName1"

			sql &= " Order By RE.Fak_Dat"



			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("dunningDate", ReplaceMissing(dunningDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RName1", ReplaceMissing(dunningData.RName1, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try
				If reader IsNot Nothing Then

					result = New List(Of InvoiceData)

					While reader.Read
						Dim data = New InvoiceData

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.MDNr = SafeGetInteger(reader, "MDNr", 0)
						data.RENr = SafeGetInteger(reader, "RENr", 0)
						data.KDNr = SafeGetInteger(reader, "KDNr", 0)
						data.InvoiceDate = SafeGetDateTime(reader, "Fak_Dat", Nothing)
						data.Art = SafeGetString(reader, "Art")
						data.DocGuid = SafeGetString(reader, "DocGuid")
						data.CustomerGuid = SafeGetString(reader, "CustomerGuid")
						data.CustomerName = SafeGetString(reader, "R_Name1")
						data.BetragInk = SafeGetDecimal(reader, "BetragInk", 0)
						data.Bezahlt = SafeGetDecimal(reader, "Bezahlt", 0)

						data.Language = Mid(SafeGetString(reader, "Sprache"), 1, 1).ToUpper
						data.Send2WOS = SafeGetBoolean(reader, "Send2WOS", False)
						data.PrintWithReport = SafeGetBoolean(reader, "PrintWithReport", False)
						data.CreditInvoiceAutomated = SafeGetBoolean(reader, "CreditInvoiceAutomated", False)
						data.RefNr = SafeGetString(reader, "RefNr")

						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom")


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


	End Class


End Namespace

