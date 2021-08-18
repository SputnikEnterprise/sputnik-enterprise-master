Imports SP.DatabaseAccess
'Imports SP.DatabaseAccess.ES.DataObjects.ESMng
Imports System.Text
Imports SP.Infrastructure
Imports SP.DatabaseAccess.Invoice.DataObjects

Namespace Invoice

	Partial Public Class InvoiceDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IInvoiceDatabaseAccess

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
			MyBase.New(connectionString, translationLanguage)
		End Sub

#End Region

#Region "Public Methods"

		''' <summary>
		''' Loads a invoice with the given invoiceNumber
		''' </summary>
		Function LoadInvoice(ByVal invoiceNumber As Integer) As DataObjects.Invoice Implements IInvoiceDatabaseAccess.LoadInvoice
			Dim invoice As DataObjects.Invoice = Nothing

			Dim sql As String = "SELECT RE.*, KD.Sprache FROM RE Left Join Kunden KD On RE.KDNr = KD.KDNr WHERE RE.RENR = @invoiceNumber"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("invoiceNumber", invoiceNumber))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try
				If reader IsNot Nothing Then
					If reader.Read Then
						invoice = New DataObjects.Invoice() 'With {
						invoice.Id = SafeGetInteger(reader, "ID", Nothing)
						invoice.ReNr = SafeGetInteger(reader, "RENR", Nothing)
						invoice.KdNr = SafeGetInteger(reader, "KDNR", Nothing)
						invoice.Art = SafeGetString(reader, "ART")
						invoice.KST = SafeGetString(reader, "KST")
						invoice.Lp = SafeGetInteger(reader, "LP", Nothing)
						invoice.FakDat = SafeGetDateTime(reader, "FAK_DAT", Nothing)
						invoice.Currency = SafeGetString(reader, "Currency")
						invoice.BetragOhne = SafeGetDecimal(reader, "BetragOhne", 0)
						invoice.BetragEx = SafeGetDecimal(reader, "BetragEx", 0)
						invoice.BetragInk = SafeGetDecimal(reader, "BetragInk", 0)
						invoice.MWST1 = SafeGetDecimal(reader, "MWST1", 0)
						invoice.MWSTProz = SafeGetDecimal(reader, "MWSTProz", Nothing)
						invoice.Bezahlt = SafeGetDecimal(reader, "Bezahlt", 0)
						invoice.SKonto = SafeGetDecimal(reader, "SKonto", Nothing)
						invoice.Verlust = SafeGetDecimal(reader, "Verlust", Nothing)
						invoice.FSKonto = SafeGetDecimal(reader, "FSKonto", Nothing)
						invoice.FVerlust = SafeGetDecimal(reader, "FVerlust", Nothing)
						invoice.Faellig = SafeGetDateTime(reader, "Faellig", Nothing)
						invoice.Mahncode = SafeGetString(reader, "Mahncode")
						invoice.SPNr = SafeGetInteger(reader, "SPNr", Nothing)
						invoice.VerNr = SafeGetInteger(reader, "VerNr", Nothing)
						invoice.MA0 = SafeGetDateTime(reader, "MA0", Nothing)
						invoice.MA1 = SafeGetDateTime(reader, "MA1", Nothing)
						invoice.MA2 = SafeGetDateTime(reader, "MA2", Nothing)
						invoice.MA3 = SafeGetDateTime(reader, "MA3", Nothing)
						invoice.Storno = SafeGetBoolean(reader, "Storno", False)
						invoice.Gebucht = SafeGetBoolean(reader, "Gebucht", False)
						invoice.FBMonat = SafeGetShort(reader, "FBMonat", Nothing)
						invoice.FBDat = SafeGetDateTime(reader, "FBDat", Nothing)
						invoice.FKSoll = SafeGetInteger(reader, "FKSoll", Nothing)
						invoice.FKHaben0 = SafeGetInteger(reader, "FKHaben0", Nothing)
						invoice.FKHaben1 = SafeGetInteger(reader, "FKHaben1", Nothing)
						invoice.RName1 = SafeGetString(reader, "R_Name1")
						invoice.RName2 = SafeGetString(reader, "R_Name2")
						invoice.RName3 = SafeGetString(reader, "R_Name3")
						invoice.CustomerLanguage = SafeGetString(reader, "Sprache")
						invoice.RZHD = SafeGetString(reader, "R_ZHD")
						invoice.RPostfach = SafeGetString(reader, "R_Postfach")
						invoice.RStrasse = SafeGetString(reader, "R_Strasse")
						invoice.RLand = SafeGetString(reader, "R_Land")
						invoice.RPLZ = SafeGetString(reader, "R_PLZ")
						invoice.ROrt = SafeGetString(reader, "R_Ort")
						invoice.ReMail = SafeGetString(reader, "REEMail")
						invoice.SendAsZip = SafeGetBoolean(reader, "SendAsZip", False)
						invoice.Zahlkond = SafeGetString(reader, "Zahlkond")
						invoice.Result = SafeGetString(reader, "Result")
						invoice.RefNr = SafeGetString(reader, "RefNr")
						invoice.RefFootNr = SafeGetString(reader, "RefFootNr")
						invoice.ESRArt = SafeGetString(reader, "ESRArt")
						invoice.ESRID = SafeGetString(reader, "ESRID")
						invoice.ESRKonto = SafeGetString(reader, "ESRKonto")
						invoice.MWSTNr = SafeGetString(reader, "MWSTNr")
						invoice.KontoNr = SafeGetString(reader, "KontoNr")
						invoice.BtrFr = SafeGetInteger(reader, "BtrFr", Nothing)
						invoice.btrRp = SafeGetString(reader, "btrRp")
						invoice.REKST1 = SafeGetString(reader, "REKST1")
						invoice.REKST2 = SafeGetString(reader, "REKST2")
						invoice.PrintedDate = SafeGetString(reader, "PrintedDate")
						invoice.GebuchtAm = SafeGetDateTime(reader, "GebuchtAm", Nothing)
						invoice.ZEInfo = SafeGetString(reader, "ZEInfo")
						invoice.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						invoice.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						invoice.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
						invoice.ChangedFrom = SafeGetString(reader, "ChangedFrom")
						invoice.RAbteilung = SafeGetString(reader, "R_Abteilung")
						invoice.Ma3RepeatNr = SafeGetInteger(reader, "Ma3_RepeatNr", Nothing)
						invoice.EsEinstufung = SafeGetString(reader, "ES_Einstufung")
						invoice.KDBranche = SafeGetString(reader, "KDBranche")
						invoice.DTAName = SafeGetString(reader, "DTA Name")
						invoice.DTAPLZOrt = SafeGetString(reader, "DTA PLZOrt")
						invoice.ESRBankName = SafeGetString(reader, "ESR BankName")
						invoice.ESRBankAdresse = SafeGetString(reader, "ESR BankAdresse")
						invoice.DTAKonto = SafeGetString(reader, "DTA Konto")
						invoice.IBANDTA = SafeGetString(reader, "IBANDTA")
						invoice.IBANVG = SafeGetString(reader, "IBANVG")
						invoice.EsrSwift = SafeGetString(reader, "ESR_Swift")
						invoice.EsrIBAN1 = SafeGetString(reader, "ESR_IBAN1")
						invoice.EsrIBAN2 = SafeGetString(reader, "ESR_IBAN2")
						invoice.EsrIBAN3 = SafeGetString(reader, "ESR_IBAN3")
						invoice.EsrBcNr = SafeGetString(reader, "ESR_BcNr")
						invoice.ProposeNr = SafeGetInteger(reader, "ProposeNr", Nothing)
						invoice.Art2 = SafeGetString(reader, "Art_2", "")
						invoice.MahnStopUntil = SafeGetDateTime(reader, "MahnStopUntil", Nothing)
						invoice.REDoc_Guid = SafeGetString(reader, "REDoc_Guid")
						invoice.Transfered_User = SafeGetString(reader, "Transfered_User")
						invoice.Transfered_On = SafeGetString(reader, "Transfered_On")
						invoice.ZEBis0 = SafeGetDateTime(reader, "ZEBis_0", Nothing)
						invoice.ZEBis1 = SafeGetDateTime(reader, "ZEBis_1", Nothing)
						invoice.ZEBis2 = SafeGetDateTime(reader, "ZEBis_2", Nothing)
						invoice.ZEBis3 = SafeGetDateTime(reader, "ZEBis_3", Nothing)
						invoice.MDNr = SafeGetInteger(reader, "MDNr", Nothing)
						'}

					End If
				End If

			Catch e As Exception
				invoice = Nothing
				m_Logger.LogError(e.ToString())
			Finally
				CloseReader(reader)

			End Try

			Return invoice

		End Function

		''' <summary>
		''' Loads a invoice with the given invoiceNumber
		''' </summary>
		Function LoadInvoiceWithOpenAmount(ByVal mdNumber As Integer) As IEnumerable(Of DataObjects.Invoice) Implements IInvoiceDatabaseAccess.LoadInvoiceWithOpenAmount
			Dim result As List(Of DataObjects.Invoice) = Nothing

			Dim sql As String

			sql = "SELECT RE.*, KD.Sprache "
			sql &= "FROM RE Left Join Kunden KD "
			sql &= "On RE.KDNr = KD.KDNr "
			sql &= "WHERE RE.MDNr = @mdNr "
			sql &= "AND RE.BetragInk - IsNull(RE.Bezahlt, 0) > 0.009 "
			sql &= "Order By RE.Fak_Dat, RE.RENr"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdnr", mdNumber))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try
				If Not reader Is Nothing Then
					result = New List(Of DataObjects.Invoice)

					While reader.Read()
						Dim data = New DataObjects.Invoice

						data.Id = SafeGetInteger(reader, "ID", Nothing)
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
						data.ReMail = SafeGetString(reader, "REEmail")
						data.SendAsZip = SafeGetBoolean(reader, "SendAsZip", False)
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
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)

			End Try

			Return result

		End Function

		''' <summary>
		''' Loads the invoice Values
		''' </summary>
		Public Function ReloadInvoiceValues(ByVal invoiceData As DataObjects.Invoice) As Boolean Implements IInvoiceDatabaseAccess.ReloadInvoiceValues

			Dim success = False

			Dim sql As String = "SELECT BetragOhne, BetragEx, BetragInk, MWST1, Bezahlt FROM RE WHERE RENR = @RENr"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("RENr", invoiceData.ReNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try
				If reader IsNot Nothing Then
					If reader.Read Then
						invoiceData.BetragOhne = SafeGetDecimal(reader, "BetragOhne", 0)
						invoiceData.BetragEx = SafeGetDecimal(reader, "BetragEx", 0)
						invoiceData.BetragInk = SafeGetDecimal(reader, "BetragInk", 0)
						invoiceData.MWST1 = SafeGetDecimal(reader, "MWST1", 0)
						invoiceData.Bezahlt = SafeGetDecimal(reader, "Bezahlt", 0)
						success = True
					End If
				End If

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())
			Finally
				CloseReader(reader)

			End Try

			Return success
		End Function

		''' <summary>
		''' Creates a invoice with the given invoiceNumber
		''' </summary>
		Public Function AddNewInvoice(ByVal invoiceData As DataObjects.Invoice, ByVal reNumberOffset As Integer) As Boolean Implements IInvoiceDatabaseAccess.AddNewInvoice

			Dim success = False

			Dim sql As String = "[Create New RE]"

			' Parameters

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			' Data of ES
			listOfParams.Add(New SqlClient.SqlParameter("@KDNR", ReplaceMissing(invoiceData.KdNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ART", ReplaceMissing(invoiceData.Art, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KST", ReplaceMissing(invoiceData.KST, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LP", ReplaceMissing(invoiceData.Lp, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@FAK_DAT", ReplaceMissing(invoiceData.FakDat, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Currency", ReplaceMissing(invoiceData.Currency, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@BetragOhne", ReplaceMissing(invoiceData.BetragOhne, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("@BetragEx", ReplaceMissing(invoiceData.BetragEx, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("@BetragInk", ReplaceMissing(invoiceData.BetragInk, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("@MWST1", ReplaceMissing(invoiceData.MWST1, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("@MWSTProz", ReplaceMissing(invoiceData.MWSTProz, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bezahlt", ReplaceMissing(invoiceData.Bezahlt, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("@SKonto", ReplaceMissing(invoiceData.SKonto, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Verlust", ReplaceMissing(invoiceData.Verlust, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@FSKonto", ReplaceMissing(invoiceData.FSKonto, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@FVerlust", ReplaceMissing(invoiceData.FVerlust, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Faellig", ReplaceMissing(invoiceData.Faellig, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Mahncode", ReplaceMissing(invoiceData.Mahncode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@SPNr", ReplaceMissing(invoiceData.SPNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@VerNr", ReplaceMissing(invoiceData.VerNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MA0", ReplaceMissing(invoiceData.MA0, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MA1", ReplaceMissing(invoiceData.MA1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MA2", ReplaceMissing(invoiceData.MA2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MA3", ReplaceMissing(invoiceData.MA3, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Storno", ReplaceMissing(invoiceData.Storno, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Gebucht", ReplaceMissing(invoiceData.Gebucht, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("@FBMonat", ReplaceMissing(invoiceData.FBMonat, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@FBDat", ReplaceMissing(invoiceData.FBDat, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@FKSoll", ReplaceMissing(invoiceData.FKSoll, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@FKHaben0", ReplaceMissing(invoiceData.FKHaben0, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@FKHaben1", ReplaceMissing(invoiceData.FKHaben1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@R_Name1", ReplaceMissing(invoiceData.RName1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@R_Name2", ReplaceMissing(invoiceData.RName2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@R_Name3", ReplaceMissing(invoiceData.RName3, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@R_ZHD", ReplaceMissing(invoiceData.RZHD, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@R_Postfach", ReplaceMissing(invoiceData.RPostfach, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@R_Strasse", ReplaceMissing(invoiceData.RStrasse, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@R_Land", ReplaceMissing(invoiceData.RLand, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@R_PLZ", ReplaceMissing(invoiceData.RPLZ, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@R_Ort", ReplaceMissing(invoiceData.ROrt, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("eMail", ReplaceMissing(invoiceData.ReMail, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("SendAsZip", ReplaceMissing(invoiceData.SendAsZip, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Zahlkond", ReplaceMissing(invoiceData.Zahlkond, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Result", ReplaceMissing(invoiceData.Result, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@RefNr", ReplaceMissing(invoiceData.RefNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@RefFootNr", ReplaceMissing(invoiceData.RefFootNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ESRArt", ReplaceMissing(invoiceData.ESRArt, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KontoNr", ReplaceMissing(invoiceData.KontoNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MWSTNr", ReplaceMissing(invoiceData.MWSTNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@BtrFr", ReplaceMissing(invoiceData.BtrFr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@btrRp", ReplaceMissing(invoiceData.btrRp, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@REKST1", ReplaceMissing(invoiceData.REKST1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@REKST2", ReplaceMissing(invoiceData.REKST2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@PrintedDate", ReplaceMissing(invoiceData.PrintedDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@GebuchtAm", ReplaceMissing(invoiceData.GebuchtAm, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ZEInfo", ReplaceMissing(invoiceData.ZEInfo, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@CreatedOn", ReplaceMissing(invoiceData.CreatedOn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@CreatedFrom", ReplaceMissing(invoiceData.CreatedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ChangedOn", ReplaceMissing(invoiceData.ChangedOn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ChangedFrom", ReplaceMissing(invoiceData.ChangedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@R_Abteilung", ReplaceMissing(invoiceData.RAbteilung, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Ma3_RepeatNr", ReplaceMissing(invoiceData.Ma3RepeatNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ES_Einstufung", ReplaceMissing(invoiceData.EsEinstufung, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KDBranche", ReplaceMissing(invoiceData.KDBranche, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@DTA_Name", ReplaceMissing(invoiceData.DTAName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@DTA_PLZOrt", ReplaceMissing(invoiceData.DTAPLZOrt, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@DTA_Konto", ReplaceMissing(invoiceData.DTAKonto, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@IBANDTA", ReplaceMissing(invoiceData.IBANDTA, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@IBANVG", ReplaceMissing(invoiceData.IBANVG, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ProposeNr", ReplaceMissing(invoiceData.ProposeNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Art_2", ReplaceMissing(invoiceData.Art2, "")))
			listOfParams.Add(New SqlClient.SqlParameter("@MahnStopUntil", ReplaceMissing(invoiceData.MahnStopUntil, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@REDoc_Guid", ReplaceMissing(invoiceData.REDoc_Guid, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Transfered_User", ReplaceMissing(invoiceData.Transfered_User, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Transfered_On", ReplaceMissing(invoiceData.Transfered_On, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ZEBis_0", ReplaceMissing(invoiceData.ZEBis0, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ZEBis_1", ReplaceMissing(invoiceData.ZEBis1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ZEBis_2", ReplaceMissing(invoiceData.ZEBis2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ZEBis_3", ReplaceMissing(invoiceData.ZEBis3, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", ReplaceMissing(invoiceData.MDNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ESRBankID", ReplaceMissing(invoiceData.ESRBankID, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("@RENumberOffset", ReplaceMissing(reNumberOffset, DBNull.Value)))

			' New ID of RE
			Dim newIdParameter = New SqlClient.SqlParameter("@NewREID", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)

			' New RENr
			Dim newRENrParameter = New SqlClient.SqlParameter("@RENR", SqlDbType.Int)
			newRENrParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newRENrParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If success AndAlso
				Not newIdParameter.Value Is Nothing AndAlso
				Not newRENrParameter Is Nothing Then
				invoiceData.Id = CType(newIdParameter.Value, Integer)
				invoiceData.ReNr = CType(newRENrParameter.Value, Integer)
			Else
				success = False
			End If

			Return success

		End Function

		''' <summary>
		''' Updates a invoice with the given invoiceNumber
		''' </summary>
		Public Function UpdateInvoice(ByVal invoiceData As DataObjects.Invoice) As Boolean Implements IInvoiceDatabaseAccess.UpdateInvoice

			Dim success = False

			Dim sql As String = "UPDATE dbo.RE SET " +
				"KdNr = @KdNr, " +
				"KST = @KST, " +
				"[ESR BankName] = @ESRBankName, " +
				"Lp = @Lp, " +
				"FAK_DAT = @FakDat, " +
				"Currency = @Currency, " +
				"Faellig = @Faellig, " +
				"Mahncode = @Mahncode, " +
				"R_Name1 = @RName1, " +
				"R_Name2 = @RName2, " +
				"R_Name3 = @RName3, " +
				"R_ZHD = @RZHD, " +
				"R_Abteilung = @RAbteilung, " +
				"R_Postfach = @RPostfach, " +
				"R_Strasse = @RStrasse, " +
				"R_Land = @RLand, " +
				"R_PLZ = @RPLZ, " +
				"R_Ort = @ROrt, " +
				"REeMail = @eMail, " +
				"SendAsZip = @SendAsZip, " +
				"Zahlkond = @Zahlkond, " +
				"ZEInfo = @ZEInfo, " +
				"ChangedOn = @ChangedOn, " +
				"ChangedFrom = @ChangedFrom, " +
				"MWSTProz = @MWSTProz, " +
				"MA0 = @MA0, " +
				"MA1 = @MA1, " +
				"MA2 = @MA2, " +
				"MA3 = @MA3, " +
				"KDBranche = @KDBranche, " +
				"Bezahlt = @Bezahlt, " +
				"Gebucht = @Gebucht, " +
				"GebuchtAm = @GebuchtAm, " +
				"MahnStopUntil = @MahnStopUntil " +
	"WHERE RENR = @invoiceNumber"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("invoiceNumber", ReplaceMissing(invoiceData.ReNr, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("KdNr", ReplaceMissing(invoiceData.KdNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KST", ReplaceMissing(invoiceData.KST, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ESRBankName", ReplaceMissing(invoiceData.ESRBankName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Lp", ReplaceMissing(invoiceData.Lp, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("FakDat", ReplaceMissing(invoiceData.FakDat, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Currency", ReplaceMissing(invoiceData.Currency, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Faellig", ReplaceMissing(invoiceData.Faellig, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Mahncode", ReplaceMissing(invoiceData.Mahncode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RName1", ReplaceMissing(invoiceData.RName1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RName2", ReplaceMissing(invoiceData.RName2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RName3", ReplaceMissing(invoiceData.RName3, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RZHD", ReplaceMissing(invoiceData.RZHD, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RAbteilung", ReplaceMissing(invoiceData.RAbteilung, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RPostfach", ReplaceMissing(invoiceData.RPostfach, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RStrasse", ReplaceMissing(invoiceData.RStrasse, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RLand", ReplaceMissing(invoiceData.RLand, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RPLZ", ReplaceMissing(invoiceData.RPLZ, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ROrt", ReplaceMissing(invoiceData.ROrt, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("eMail", ReplaceMissing(invoiceData.ReMail, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("SendAsZip", ReplaceMissing(invoiceData.SendAsZip, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Zahlkond", ReplaceMissing(invoiceData.Zahlkond, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ZEInfo", ReplaceMissing(invoiceData.ZEInfo, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ChangedOn", ReplaceMissing(invoiceData.ChangedOn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ChangedFrom", ReplaceMissing(invoiceData.ChangedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MWSTProz", ReplaceMissing(invoiceData.MWSTProz, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MA0", ReplaceMissing(invoiceData.MA0, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MA1", ReplaceMissing(invoiceData.MA1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MA2", ReplaceMissing(invoiceData.MA2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MA3", ReplaceMissing(invoiceData.MA3, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KDBranche", ReplaceMissing(invoiceData.KDBranche, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Bezahlt", ReplaceMissing(invoiceData.Bezahlt, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Gebucht", ReplaceMissing(invoiceData.Gebucht, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("GebuchtAm", ReplaceMissing(invoiceData.GebuchtAm, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MahnStopUntil", ReplaceMissing(invoiceData.MahnStopUntil, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		''' <summary>
		''' Loads a invoice rows individual with the given invoiceNumber
		''' </summary>
		Public Function LoadInvoiceIndividual(ByVal ReNr As Integer) As List(Of DataObjects.InvoiceIndividual) Implements IInvoiceDatabaseAccess.LoadInvoiceIndividual
			Dim rows As List(Of DataObjects.InvoiceIndividual) = Nothing

			Dim sql As String = "SELECT ID, RENr, KDNr, BetragTotal, MWST1, BetragEx, Currency, Monat, Jahr, RE_Text, RecNr, RE_HeadText FROM RE_Ind WHERE RENr = @ReNr ORDER BY RecNr"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("ReNr", ReNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try
				If reader IsNot Nothing Then

					rows = New List(Of DataObjects.InvoiceIndividual)

					While reader.Read
						Dim row = New DataObjects.InvoiceIndividual With {
								.Id = SafeGetInteger(reader, "ID", 0),
								.RENr = SafeGetInteger(reader, "RENr", Nothing),
								.KDNr = SafeGetInteger(reader, "KDNr", Nothing),
								.BetragTotal = SafeGetDecimal(reader, "BetragTotal", Nothing),
								.MWST1 = SafeGetDecimal(reader, "MWST1", Nothing),
								.BetragEx = SafeGetDecimal(reader, "BetragEx", Nothing),
								.Currency = SafeGetString(reader, "Currency", Nothing),
								.Monat = SafeGetShort(reader, "Monat", Nothing),
								.Jahr = SafeGetString(reader, "Jahr", Nothing),
								.ReText = SafeGetString(reader, "RE_Text", Nothing),
								.RecNr = SafeGetInteger(reader, "RecNr", Nothing),
								.ReHeadText = SafeGetString(reader, "RE_HeadText", Nothing)
						}
						rows.Add(row)
					End While
				End If
			Catch e As Exception
				m_Logger.LogError(e.ToString())
			Finally
				CloseReader(reader)
			End Try

			Return rows
		End Function

		''' <summary>
		''' Creates a invoice row individual with the given invoiceNumber
		''' </summary>
		Public Function AddNewInvoiceIndividual(ByVal invoiceRowData As DataObjects.InvoiceIndividual, ByVal refereceNumbersTo10 As Boolean) As Boolean Implements IInvoiceDatabaseAccess.AddNewInvoiceIndividual

			Dim success = False

			Dim sql As String = "[Create New RE_Ind]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@RENr", ReplaceMissing(invoiceRowData.RENr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KDNr", ReplaceMissing(invoiceRowData.KDNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@BetragTotal", ReplaceMissing(invoiceRowData.BetragTotal, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("@MWST1", ReplaceMissing(invoiceRowData.MWST1, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("@BetragEx", ReplaceMissing(invoiceRowData.BetragEx, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("@Currency", ReplaceMissing(invoiceRowData.Currency, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Monat", ReplaceMissing(invoiceRowData.Monat, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("@Jahr", ReplaceMissing(invoiceRowData.Jahr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("@RE_Text", ReplaceMissing(invoiceRowData.ReText, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@RE_HeadText", ReplaceMissing(invoiceRowData.ReHeadText, DBNull.Value)))

			' New Id
			Dim newIdParameter = New SqlClient.SqlParameter("@ID", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)

			' New RecNr
			Dim newRecNrParameter = New SqlClient.SqlParameter("@RecNr", SqlDbType.Int)
			newRecNrParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newRecNrParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If success AndAlso
				Not newIdParameter.Value Is Nothing AndAlso
				Not newRecNrParameter Is Nothing Then
				invoiceRowData.Id = CType(newIdParameter.Value, Integer)
				invoiceRowData.RecNr = CType(newRecNrParameter.Value, Integer)
			Else
				success = False
			End If

			If success Then
				success = UpdateInvoiceValues(invoiceRowData.RENr, refereceNumbersTo10)
			End If

			Return success
		End Function

		''' <summary>
		''' Updates a invoice row individual with the given invoiceNumber
		''' </summary>
		Public Function UpdateInvoiceIndividual(ByVal invoiceRowData As DataObjects.InvoiceIndividual, ByVal refereceNumbersTo10 As Boolean) As Boolean Implements IInvoiceDatabaseAccess.UpdateInvoiceIndividual

			Dim success = False

			Dim sql As String = "Update RE_Ind SET " +
				"RE_HeadText = @ReHeadText, " +
				"RE_Text = @ReText, " +
				"BetragEx = @BetragEx, " +
				"MWST1 = @MWST1, " +
				"BetragTotal = @BetragTotal " +
				"WHERE ID = @Id"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("ID", ReplaceMissing(invoiceRowData.Id, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ReHeadText", ReplaceMissing(invoiceRowData.ReHeadText, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ReText", ReplaceMissing(invoiceRowData.ReText, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("BetragEx", ReplaceMissing(invoiceRowData.BetragEx, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("MWST1", ReplaceMissing(invoiceRowData.MWST1, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("BetragTotal", ReplaceMissing(invoiceRowData.BetragTotal, 0)))

			success = ExecuteNonQuery(sql, listOfParams)

			If success Then
				success = UpdateInvoiceValues(invoiceRowData.RENr, refereceNumbersTo10)
			End If

			Return success
		End Function

		''' <summary>
		''' Updates the reference numbers of an invoice.
		''' </summary>
		Public Function UpdateInvoiceReferenceNumbers(ByVal reNr As Integer, ByVal refNrTo10 As Boolean) As Boolean Implements IInvoiceDatabaseAccess.UpdateInvoiceReferenceNumbers

			Dim success As Boolean = False

			Dim sql As String
			sql = "SELECT RE.RENR, RE.KDNR, RE.BetragInk, MD_ESRDTA.MD_ID, MD_ESRDTA.KontoESR1 FROM RE "
			sql &= "INNER JOIN MD_ESRDTA ON RE.MDNr = MD_ESRDTA.MDNr And MD_ESRDTA.BankCLNr = RE.ESR_BcNr AND MD_ESRDTA.MD_ID = RE.ESRID "
			sql &= "WHERE "

			sql &= "MD_ESRDTA.ModulArt = 1 "
			sql &= "And RE.RENr = @reNr"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("reNr", reNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try
				If reader IsNot Nothing Then
					If reader.Read Then

						Dim kdNr As Integer = SafeGetInteger(reader, "KDNR", 0)
						Dim betragkInk As Decimal = SafeGetDecimal(reader, "BetragInk", 0)
						Dim md_id As String = SafeGetString(reader, "MD_ID", String.Empty)
						Dim kontoESR1 As String = SafeGetString(reader, "KontoESR1", String.Empty)

						CloseReader(reader)

						Dim referenceUtility As New ReferenceNumberUtility
						Dim refNumbers = referenceUtility.FormatReferenceNumbers(md_id, kdNr, reNr, betragkInk, kontoESR1, kontoESR1, refNrTo10)

						sql = "UPDATE Re SET RefNr = @refNr, RefFootNr = @refFootNr WHERE RENr = @RENr "

						' Parameters
						Dim updateParams As New List(Of SqlClient.SqlParameter)
						updateParams.Add(New SqlClient.SqlParameter("RENr", reNr))
						updateParams.Add(New SqlClient.SqlParameter("refNr", refNumbers.Item1))
						updateParams.Add(New SqlClient.SqlParameter("refFootNr", refNumbers.Item2))

						success = ExecuteNonQuery(sql, updateParams)

					End If
				End If

			Catch e As Exception
				m_Logger.LogError(e.ToString())
			Finally
				CloseReader(reader)

			End Try

			Return success

		End Function

		''' <summary>
		''' Updates the reference numbers of an dunning (RE_SP).
		''' </summary>
		Public Function UpdateDunningAndArrearReferenceNumbers(ByVal mdNr As Integer, ByVal reNr As Integer, ByVal kdNr As Integer, ByVal betragkInk As Decimal, ByVal refNrTo10 As Boolean) As Boolean Implements IInvoiceDatabaseAccess.UpdateDunningAndArrearReferenceNumbers

			Dim success As Boolean = False

			Dim sql As String
			sql = "SELECT Top 1 MD_ESRDTA.MD_ID, MD_ESRDTA.KontoESR1, MD_ESRDTA.KontoESR2"
			sql &= " FROM MD_ESRDTA"
			sql &= " WHERE MDNr = @MDNr"
			sql &= " And ModulArt = 1"
			sql &= " And AsStandard = 1"
			sql &= " Order By ID Desc"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try
				If reader IsNot Nothing Then
					If reader.Read Then

						Dim md_id As String = SafeGetString(reader, "MD_ID", String.Empty)
						Dim kontoESR1 As String = SafeGetString(reader, "KontoESR1", String.Empty)
						Dim kontoESR2 As String = SafeGetString(reader, "KontoESR2", String.Empty)

						CloseReader(reader)

						Dim referenceUtility As New ReferenceNumberUtility
						Dim refNumbers = referenceUtility.FormatReferenceNumbers(md_id, kdNr, reNr, betragkInk, kontoESR1, kontoESR1, refNrTo10)

						sql = "Declare @BtrFr int"
						sql &= " Declare @BtrRp nvarchar(2)"
						sql &= " SET @BtrFr = CAST((@BetragInk - (@BetragInk % 1)) AS Int);"
						sql &= " SET @BtrRp = SUBSTRING(CAST(CAST(ROUND((ABS(@BetragInk) % 1), 2) AS decimal(8,2)) AS nvarchar(4)), 3, 2);"

						sql &= " UPDATE RE_Sp SET RefNr = @refNr"
						sql &= ",RefFootNr = @refFootNr"
						sql &= ",BtrFr = @BtrFr"
						sql &= ",BtrRp = @BtrRp"
						sql &= ",ESRID = @ESRID"
						sql &= ",ESRKonto = @ESRKonto"
						sql &= ",KontoNr = @ESRKonto2"

						sql &= " WHERE RENr = @RENr; "

						' Parameters
						Dim updateParams As New List(Of SqlClient.SqlParameter)
						updateParams.Add(New SqlClient.SqlParameter("RENr", reNr))
						updateParams.Add(New SqlClient.SqlParameter("refNr", refNumbers.Item1))
						updateParams.Add(New SqlClient.SqlParameter("refFootNr", refNumbers.Item2))
						updateParams.Add(New SqlClient.SqlParameter("ESRID", ReplaceMissing(md_id, DBNull.Value)))
						updateParams.Add(New SqlClient.SqlParameter("ESRKonto", ReplaceMissing(kontoESR1, DBNull.Value)))
						updateParams.Add(New SqlClient.SqlParameter("ESRKonto2", ReplaceMissing(kontoESR2, DBNull.Value)))

						updateParams.Add(New SqlClient.SqlParameter("BetragInk", ReplaceMissing(betragkInk, DBNull.Value)))


						success = ExecuteNonQuery(sql, updateParams)

					End If
				End If

			Catch e As Exception
				m_Logger.LogError(e.ToString())
			Finally
				CloseReader(reader)

			End Try

			Return success

		End Function

		''' <summary>
		''' Delete a invoice row individual with the given invoiceNumber
		''' </summary>
		Public Function DeleteInvoiceIndividual(ByVal invoiceRowData As DataObjects.InvoiceIndividual, ByVal modul As String, ByVal username As String, ByVal usnr As Integer, ByVal refereceNumbersTo10 As Boolean) As DeleteREIndResult Implements IInvoiceDatabaseAccess.DeleteInvoiceIndividual

			Dim success = True

			Dim sql As String

			sql = "[Delete RE_Ind]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("id", invoiceRowData.Id))
			listOfParams.Add(New SqlClient.SqlParameter("modul", modul))
			listOfParams.Add(New SqlClient.SqlParameter("username", username))
			listOfParams.Add(New SqlClient.SqlParameter("usnr", usnr))

			Dim resultParameter = New SqlClient.SqlParameter("@result", SqlDbType.Int)
			resultParameter.Direction = ParameterDirection.Output
			listOfParams.Add(resultParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Dim resultEnum As DeleteREResult

			If Not resultParameter.Value Is Nothing Then
				Try
					resultEnum = CType(resultParameter.Value, DeleteREIndResult)
				Catch
					resultEnum = DeleteREResult.ResultDeleteError
				End Try
			Else
				resultEnum = DeleteREResult.ResultDeleteError
			End If

			' Update Invoice data.
			If resultEnum = DeleteREResult.ResultDeleteOk Then
				success = UpdateInvoiceValues(invoiceRowData.RENr, refereceNumbersTo10)
			End If

			Return resultEnum

		End Function

		''' <summary>
		''' Delete an invoice.
		''' </summary>
		Public Function DeleteInvoice(ByVal id As Integer, ByVal modul As String, ByVal username As String, ByVal usnr As Integer) As DeleteREResult Implements IInvoiceDatabaseAccess.DeleteInvoice

			Dim success = True

			Dim sql As String

			sql = "[Delete RE]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("id", id))
			listOfParams.Add(New SqlClient.SqlParameter("modul", modul))
			listOfParams.Add(New SqlClient.SqlParameter("username", username))
			listOfParams.Add(New SqlClient.SqlParameter("usnr", usnr))

			Dim resultParameter = New SqlClient.SqlParameter("@result", SqlDbType.Int)
			resultParameter.Direction = ParameterDirection.Output
			listOfParams.Add(resultParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Dim resultEnum As DeleteREResult

			If Not resultParameter.Value Is Nothing Then
				Try
					resultEnum = CType(resultParameter.Value, DeleteREResult)
				Catch
					resultEnum = DeleteREResult.ResultDeleteError
				End Try
			Else
				resultEnum = DeleteREResult.ResultDeleteError
			End If

			Return resultEnum


		End Function

		''' <summary>
		''' Delete an invoice.
		''' </summary>
		Public Function DeleteInvoiceAndInsertInvoiceDocumentIntoDeleteDb(ByVal id As Integer, ByVal modul As String,
																																			ByVal username As String,
																																			ByVal usnr As Integer,
																																			ByVal invoiceDocument() As Byte) As DeleteREResult Implements IInvoiceDatabaseAccess.DeleteInvoiceAndInsertInvoiceDocumentIntoDeleteDb

			Dim success = True

			Dim sql As String

			sql = "[Delete Invoice And Add Inovice Document Into DeletedDb]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("id", ReplaceMissing(id, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("modul", ReplaceMissing(modul, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("username", ReplaceMissing(username, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("usnr", ReplaceMissing(usnr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("file", ReplaceMissing(invoiceDocument, DBNull.Value)))

			Dim resultParameter = New SqlClient.SqlParameter("@result", SqlDbType.Int)
			resultParameter.Direction = ParameterDirection.Output
			listOfParams.Add(resultParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Dim resultEnum As DeleteREResult

			If Not resultParameter.Value Is Nothing Then
				Try
					resultEnum = CType(resultParameter.Value, DeleteREResult)
				Catch
					resultEnum = DeleteREResult.ResultDeleteError
				End Try
			Else
				resultEnum = DeleteREResult.ResultDeleteError
			End If

			Return resultEnum


		End Function

		''' <summary>
		''' Loads a invoice rows rpt with the given invoiceNumber
		''' </summary>
		Public Function LoadInvoiceRPT(ByVal ReNr As Integer, ByVal KDNr As Integer, ByVal lang As String) As List(Of DataObjects.InvoiceRPL) Implements IInvoiceDatabaseAccess.LoadInvoiceRPT
			Dim rows As List(Of DataObjects.InvoiceRPL) = Nothing

			lang = IIf(String.IsNullOrWhiteSpace(lang), "D", lang.Substring(0, 1).ToUpper)

			Dim sql As String
			sql = "SELECT RPL.RPNr, RPL.MANr, RPL.ESNr, RPL.LANr, RPL.VonDate, RPL.BisDate, "
			sql &= "RPL.K_Anzahl, RPL.K_Basis, RPL.K_Ansatz, RPL.K_Betrag, RPL.MWST, "
			sql &= "RPL.RPLNr, RPL.KSTNr, RPT.ID As RPT_ID, RPT.RPText, "
			sql &= "ISNULL( (SELECT TOP 1 KST.Bezeichnung KSTBez FROM KD_KST KST "
			sql &= "WHERE KST.KDNR = RPL.KDNr And KST.RecNr = RPL.KSTNr), '') AS KSTBez, "

			If lang = "D" Then
				sql += "LA.LAOpText As LAOpText "
			Else
				sql += "LAT.Name_OP_" + lang + " As LAOpText "
			End If

			sql += "From RPL "
			sql += "Left Join LA On RPL.LANr = LA.LANr And Year(RPL.VonDate) = LA.LAJahr "
			sql += "Left Join RPT On RPL.RPLNr = RPT.RPLNr And RPL.RPNr = RPT.RPNr "

			If lang = "D" Then
				'nothing
			Else
				sql += "Left Join dbo.LA_Translated LAT On RPL.LANr = LAT.LANr  "
			End If

			sql &= "WHERE LA.LADeactivated = 0 "
			sql &= "And RPL.RENr = @ReNr And RPL.KD = 1 And RPL.KDNr = @KDNr "
			sql &= "Order By (RPL.RPNr + RPL.VonDate) ASC"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("ReNr", ReNr))
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", KDNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try
				If reader IsNot Nothing Then

					rows = New List(Of DataObjects.InvoiceRPL)

					While reader.Read
						Dim row = New DataObjects.InvoiceRPL With {
							.RPNr = SafeGetInteger(reader, "RPNr", Nothing),
							.MANr = SafeGetInteger(reader, "MANr", Nothing),
							.ESNr = SafeGetInteger(reader, "ESNr", Nothing),
							.LANr = SafeGetDecimal(reader, "LANr", Nothing),
							.VonDate = SafeGetDateTime(reader, "VonDate", Nothing),
							.BisDate = SafeGetDateTime(reader, "BisDate", Nothing),
							.KAnzahl = SafeGetDecimal(reader, "K_Anzahl", Nothing),
							.KBasis = SafeGetDecimal(reader, "K_Basis", Nothing),
							.KAnsatz = SafeGetDecimal(reader, "K_Ansatz", Nothing),
							.KBetrag = SafeGetDecimal(reader, "K_Betrag", 0),
							.RPLNr = SafeGetInteger(reader, "RPLNr", Nothing),
							.KSTNr = SafeGetInteger(reader, "KSTNr", Nothing),
							.kstname = SafeGetString(reader, "KSTBez"),
							.MWST = SafeGetDecimal(reader, "MWST", 0),
							.LAOpText = SafeGetString(reader, "LAOpText", Nothing),
							.RPTId = SafeGetInteger(reader, "RPT_ID", Nothing),
						.RPText = SafeGetString(reader, "RPText", Nothing)
						}
						rows.Add(row)
					End While
				End If
			Catch e As Exception
				m_Logger.LogError(e.ToString())
			Finally
				CloseReader(reader)
			End Try

			Return rows
		End Function

		''' <summary>
		''' Updates a invoice row rpt with the given invoiceNumber
		''' </summary>
		Public Function UpdateInvoiceRPT(ByVal invoiceRowData As DataObjects.InvoiceRPL) As Boolean Implements IInvoiceDatabaseAccess.UpdateInvoiceRPT

			Dim success = False

			If invoiceRowData.RPTId Is Nothing Then
				Return success
			End If

			Dim sql As String = "UPDATE RPT SET " +
				"RPText = @RPText " +
				"WHERE ID = @RPTId"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("RPText", ReplaceMissing(invoiceRowData.RPText, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RPTId", ReplaceMissing(invoiceRowData.RPTId, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success
		End Function

		''' <summary>
		''' Loads Bank data from mandant
		''' </summary>
		Public Function LoadBankData(MDNr As Integer) As List(Of DataObjects.BankData) Implements IInvoiceDatabaseAccess.LoadBankData
			Dim bankDatas As List(Of DataObjects.BankData) = Nothing

			Dim sql As String
			sql = "SELECT ID,"
			sql &= " RecNr, "
			sql &= " MDNr,"
			sql &= " MD_ID,"
			sql &= " KontoESR1,"
			sql &= " KontoESR2,"
			sql &= " BankName,"
			sql &= " ESRIBAN1,"
			sql &= " ESRIBAN2,"
			sql &= " ESRIBAN3,"
			sql &= " RecBez,"
			sql &= " AsStandard,"
			sql &= " BankClnr,"
			sql &= " BankAdresse,"
			sql &= " Swift"
			sql &= " FROM MD_ESRDTA"
			sql &= " WHERE MDNr = @MDNr"
			sql &= " And ModulArt = 1"
			sql &= " And (MD_ID <> '' Or MD_ID Is not Null)"
			sql &= " ORDER BY Jahr DESC"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", MDNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try
				If reader IsNot Nothing Then

					bankDatas = New List(Of DataObjects.BankData)

					While reader.Read
						Dim bankData = New DataObjects.BankData

						bankData.ID = SafeGetInteger(reader, "ID", 0)
						bankData.MDNr = SafeGetInteger(reader, "MDNR", Nothing)
						bankData.RecNr = SafeGetInteger(reader, "RecNr", Nothing)
						bankData.md_id = SafeGetString(reader, "MD_ID")
						bankData.KontoESR1 = SafeGetString(reader, "KontoESR1")
						bankData.KontoESR2 = SafeGetString(reader, "KontoESR2")
						bankData.BankName = SafeGetString(reader, "BankName")
						bankData.ESRIBAN1 = SafeGetString(reader, "ESRIBAN1")
						bankData.ESRIBAN2 = SafeGetString(reader, "ESRIBAN2")
						bankData.ESRIBAN3 = SafeGetString(reader, "ESRIBAN3")
						bankData.BankClnr = SafeGetString(reader, "BankClnr")
						bankData.BankClnr = SafeGetString(reader, "BankAdresse")
						bankData.BankClnr = SafeGetString(reader, "Swift")
						bankData.AsStandard = SafeGetBoolean(reader, "asstandard", False)


						bankDatas.Add(bankData)
					End While

				End If

			Catch e As Exception
				m_Logger.LogError(e.ToString())
			Finally
				CloseReader(reader)
			End Try

			Return bankDatas
		End Function

		''' <summary>
		''' Loads all customer data
		''' </summary>
		Public Function LoadCustomerData() As List(Of DataObjects.Customer) Implements IInvoiceDatabaseAccess.LoadCustomerData

			Dim customers As List(Of DataObjects.Customer) = New List(Of DataObjects.Customer)

			Dim sql As String
			sql = "SELECT KDNr, Firma1, Strasse, PLZ, Ort, Land, Sprache, Currency, MWST FROM Kunden ORDER BY Firma1 ASC"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try
				If reader IsNot Nothing Then
					While reader.Read
						Dim customer = New DataObjects.Customer With {
								.KDNr = SafeGetInteger(reader, "KDNr", 0),
								.Firma1 = SafeGetString(reader, "Firma1"),
								.Strasse = SafeGetString(reader, "Strasse"),
								.PLZ = SafeGetString(reader, "PLZ"),
								.Ort = SafeGetString(reader, "Ort"),
								.Land = SafeGetString(reader, "Land"),
								.Sprache = SafeGetString(reader, "Sprache"),
								.Currency = SafeGetString(reader, "Currency"),
								.MWST = SafeGetBoolean(reader, "MWST", Nothing)
							}
						customers.Add(customer)
					End While
				End If

			Catch e As Exception

				m_Logger.LogError(e.ToString())
			Finally
				CloseReader(reader)
			End Try

			Return customers
		End Function

		''' <summary>
		''' Loads all address of a customer from KD_RE_Address
		''' </summary>
		Public Function LoadCustomerReAddressData(ByVal KDNr As Integer) As List(Of DataObjects.CustomerReAddress) Implements IInvoiceDatabaseAccess.LoadCustomerReAddressData
			Dim addresses As List(Of DataObjects.CustomerReAddress) = New List(Of DataObjects.CustomerReAddress)

			Dim sql As String
			sql = "SELECT ID"
			sql &= ", KDNr"
			sql &= ", REFirma"
			sql &= ", REFirma2"
			sql &= ", REFirma3"
			sql &= ", REStrasse "
			sql &= ", REPLZ"
			sql &= ", REOrt"
			sql &= ", REeMail"
			sql &= ", SendAsZip"
			sql &= ", RELand"
			sql &= ", REAbteilung"
			sql &= ", RecNr"
			sql &= ", REZhd"
			sql &= ", REPostfach"
			sql &= ", MahnCode"
			sql &= ", ZahlKond"
			sql &= ", ActiveRec "
			sql &= "FROM dbo.KD_RE_Address "
			sql &= "WHERE KDNr = @KDNr"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", KDNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try
				If reader IsNot Nothing Then
					While reader.Read
						Dim addess = New DataObjects.CustomerReAddress With {
								.Id = SafeGetInteger(reader, "ID", 0),
								.KDNr = SafeGetInteger(reader, "KDNr", 0),
								.REFirma = SafeGetString(reader, "REFirma"),
								.REFirma2 = SafeGetString(reader, "REFirma2"),
								.REFirma3 = SafeGetString(reader, "REFirma3"),
								.REStrasse = SafeGetString(reader, "REStrasse"),
								.REPLZ = SafeGetString(reader, "REPLZ"),
								.REOrt = SafeGetString(reader, "REOrt"),
								.REeMail = SafeGetString(reader, "REeMail"),
								.SendAsZip = SafeGetBoolean(reader, "SendAsZip", False),
								.RELand = SafeGetString(reader, "RELand"),
								.REAbteilung = SafeGetString(reader, "REAbteilung"),
								.REZhd = SafeGetString(reader, "REZhd"),
								.REPostfach = SafeGetString(reader, "REPostfach"),
								.RecNr = SafeGetShort(reader, "RecNr", 0),
								.MahnCode = SafeGetString(reader, "MahnCode"),
								.PaymentCondition = SafeGetString(reader, "ZahlKond"),
								.IsActive = SafeGetBoolean(reader, "ActiveRec", False)
							}
						addresses.Add(addess)
					End While
				End If

			Catch e As Exception

				m_Logger.LogError(e.ToString())
			Finally
				CloseReader(reader)
			End Try

			Return addresses
		End Function

		''' <summary>
		''' Gets conflicted MonthClose records in period.
		''' </summary>
		''' <param name="mdNr">The mandant number.</param>
		''' <param name="startDate">The start date of the period.</param>
		''' <param name="endDate">The end date of the period.</param>
		''' <param name="resultCode">The result code.</param>
		''' <returns>Conflicting MonthClose records between period and result code.</returns>
		Public Function LoadConflictedMonthCloseRecordsInPeriod(ByVal mdNr As Integer, ByVal startDate As DateTime, ByVal endDate As DateTime, ByRef resultCode As Integer) As IEnumerable(Of ConflictedMonthCloseData) Implements IInvoiceDatabaseAccess.LoadConflictedMonthCloseRecordsInPeriod

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
		''' Gets founded Payment records for selected invoice.
		''' </summary>
		''' <param name="invoiceNumber">The customer number.</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function LoadFoundedPaymentForInvoiceMng(ByVal invoiceNumber As Integer?) As IEnumerable(Of InvoicePaymentProperty) Implements IInvoiceDatabaseAccess.LoadFoundedPaymentForInvoiceMng

			Dim success = True

			Dim result As List(Of InvoicePaymentProperty) = Nothing

			Dim sql As String
			If invoiceNumber.HasValue Then
				sql = "[Get ZEData 4 Selected RE In MainView]"
			Else
				sql = "[Get ZEData 4 All RE In MainView]"
			End If

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("@RENr", ReplaceMissing(invoiceNumber, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of InvoicePaymentProperty)

					While reader.Read()
						Dim overviewData As New InvoicePaymentProperty

						overviewData.customerMDNr = SafeGetInteger(reader, "customermdnr", 0)
						overviewData.mdnr = SafeGetInteger(reader, "mdnr", 0)
						overviewData.zenr = SafeGetInteger(reader, "zenr", 0)
						overviewData.renr = SafeGetInteger(reader, "renr", 0)
						overviewData.kdnr = SafeGetInteger(reader, "kdnr", 0)

						overviewData.firma1 = SafeGetString(reader, "firma1")
						overviewData.firma2 = SafeGetString(reader, "firma2")
						overviewData.firma3 = SafeGetString(reader, "firma3")
						overviewData.abteilung = SafeGetString(reader, "abteilung")

						overviewData.zhd = SafeGetString(reader, "zhd")
						overviewData.postfach = SafeGetString(reader, "postfach")
						overviewData.strasse = SafeGetString(reader, "strasse")
						overviewData.plz = SafeGetString(reader, "ort")
						overviewData.ort = SafeGetString(reader, "plz")
						overviewData.plzort = String.Format("{0} {1}", SafeGetString(reader, "plz"), SafeGetString(reader, "ort"))


						overviewData.valutadate = SafeGetDateTime(reader, "valutadate", Nothing)
						overviewData.buchungdate = SafeGetDateTime(reader, "buchungsdate", Nothing)
						overviewData.fakdate = SafeGetDateTime(reader, "fakdate", Nothing)
						overviewData.faelligdate = SafeGetDateTime(reader, "faelligdate", Nothing)

						overviewData.einstufung = SafeGetString(reader, "einstufung")
						overviewData.branche = SafeGetString(reader, "branche")

						overviewData.betragink = SafeGetDecimal(reader, "betragink", 0)
						overviewData.zebetrag = SafeGetDecimal(reader, "zebetrag", 0)

						overviewData.mwstproz = SafeGetDecimal(reader, "mwstproz", 0)
						overviewData.betragopen = SafeGetDecimal(reader, "betragink", 0) - SafeGetDecimal(reader, "betragbezahlt", 0)

						overviewData.rekst1 = SafeGetString(reader, "rekst1")
						overviewData.rekst2 = SafeGetString(reader, "rekst2")
						overviewData.rekst = SafeGetString(reader, "rekst")

						overviewData.reart1 = SafeGetString(reader, "reart1")
						overviewData.reart2 = SafeGetString(reader, "reart2")


						overviewData.kdtelefon = SafeGetString(reader, "kdtelefon")
						overviewData.kdtelefax = SafeGetString(reader, "kdtelefax")
						overviewData.kdemail = SafeGetString(reader, "kdemail")

						overviewData.employeeadvisor = SafeGetString(reader, "employeeadvisor")
						overviewData.customeradvisor = SafeGetString(reader, "customeradvisor")

						overviewData.createdon = SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = SafeGetString(reader, "createdfrom")
						overviewData.zfiliale = SafeGetString(reader, "zfiliale")

						result.Add(overviewData)

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




#Region "Private Methods"

		''' <summary>
		''' Updates the Invoice values 
		''' </summary>
		Private Function UpdateInvoiceValues(ByVal RENr As Integer, ByVal refNrTo10 As Boolean) As Boolean

			Dim success = False

			Dim sql As String = "[Update RE Values]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@RENr", RENr))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			success = success AndAlso UpdateInvoiceReferenceNumbers(RENr, refNrTo10)

			Return success

		End Function

#End Region

	End Class

End Namespace
