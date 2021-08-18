
Imports SP.DatabaseAccess
Imports System.Text
Imports SP.Infrastructure
Imports SP.DatabaseAccess.Invoice.DataObjects
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.DatabaseAccess.Report.DataObjects


Namespace Invoice


	Partial Public Class InvoiceDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IInvoiceDatabaseAccess



#Region "Public Methods"


#Region "Load invoice data"

		''' <summary>
		''' Loads dunning date data from mandant
		''' </summary>
		Public Function LoadDunningDatesData(ByVal mdNumber As Integer, ByVal dunningLevel As Integer, ByVal mahnDate As Date?) As IEnumerable(Of DataObjects.Invoice) Implements IInvoiceDatabaseAccess.LoadDunningDatesData
			Dim result As List(Of DataObjects.Invoice) = Nothing

			Dim sql As String
			sql = "SELECT  RE.RENR"
			sql &= ", RE.KDNR"
			sql &= ", RE.FAK_DAT"
			sql &= ", RE.Faellig"
			sql &= ", RE.MA0"
			sql &= ", RE.MA1"
			sql &= ", RE.MA2"
			sql &= ", RE.MA3"
			sql &= ", RE.BetragOhne"
			sql &= ", RE.BetragEx"
			sql &= ", RE.MWST1"
			sql &= ", RE.BetragInk"
			sql &= ", RE.MWSTProz"
			sql &= ", RE.Bezahlt"
			sql &= ", RE.Mahncode"
			sql &= ", RE.R_Name1"
			sql &= ", RE.R_Strasse"
			sql &= ", RE.R_PLZ"
			sql &= ", RE.R_Ort"
			sql &= ", RE.REeMail"
			sql &= ", IsNull(RE.SendAsZip, 0) SendAsZip"
			sql &= ", convert(Date, ma{0}, 104) MahnDate"
			sql &= " FROM RE"
			sql &= " WHERE MDNr = @MDNr"
			sql &= " And RE.BetragInk > Bezahlt"
			sql &= " And ( @dunningDate is NULL OR ma{0} = @dunningDate )"
			sql &= " ORDER BY ma{0} DESC, RENr"
			sql = String.Format(sql, dunningLevel)

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", mdNumber))
			listOfParams.Add(New SqlClient.SqlParameter("dunningDate", ReplaceMissing(mahnDate, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try
				If reader IsNot Nothing Then

					result = New List(Of DataObjects.Invoice)

					While reader.Read
						Dim data = New DataObjects.Invoice

						data.ReNr = SafeGetInteger(reader, "RENr", 0)
						data.KdNr = SafeGetInteger(reader, "KDNr", 0)
						data.FakDat = SafeGetDateTime(reader, "Fak_dat", Nothing)
						data.Faellig = SafeGetDateTime(reader, "Faellig", Nothing)

						data.MA0 = SafeGetDateTime(reader, "MA0", Nothing)
						data.MA1 = SafeGetDateTime(reader, "MA1", Nothing)
						data.MA2 = SafeGetDateTime(reader, "MA2", Nothing)
						data.MA3 = SafeGetDateTime(reader, "MA3", Nothing)

						data.Mahncode = SafeGetString(reader, "Mahncode")
						data.RName1 = SafeGetString(reader, "R_Name1")
						data.RStrasse = SafeGetString(reader, "R_Strasse")
						data.RPLZ = SafeGetString(reader, "R_PLZ")
						data.ROrt = SafeGetString(reader, "R_Ort")
						data.ReMail = SafeGetString(reader, "REeMail")
						data.SendAsZip = SafeGetBoolean(reader, "SendAsZip", False)

						data.BetragOhne = SafeGetDecimal(reader, "BetragOhne", 0)
						data.BetragEx = SafeGetDecimal(reader, "BetragEx", 0)
						data.MWSTProz = SafeGetDecimal(reader, "MWSTProz", 0)
						data.MWST1 = SafeGetDecimal(reader, "MwSt1", 0)
						data.BetragInk = SafeGetDecimal(reader, "BetragInk", 0)
						data.Bezahlt = SafeGetDecimal(reader, "Bezahlt", 0)


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
		''' Loads invoice data for selected dunning level from mandant
		''' </summary>
		Public Function LoadInvoiceDataForCreatingKontoauszug(ByVal mdNumber As Integer, ByVal zeUntil As Date) As IEnumerable(Of DataObjects.Invoice) Implements IInvoiceDatabaseAccess.LoadInvoiceDataForCreatingKontoauszug

			Dim result As List(Of DataObjects.Invoice) = Nothing

			Dim sql As String

			sql = "[Get InvoiceData For Create Kontoauszug]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNumber))
			listOfParams.Add(New SqlClient.SqlParameter("zeUntil", ReplaceMissing(zeUntil, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of DataObjects.Invoice)

					While reader.Read
						Dim data = New DataObjects.Invoice

						data.ReNr = SafeGetInteger(reader, "RENr", 0)
						data.KdNr = SafeGetInteger(reader, "KDNr", 0)
						data.FakDat = SafeGetDateTime(reader, "Fak_dat", Nothing)
						data.Faellig = SafeGetDateTime(reader, "Faellig", Nothing)

						data.ZEBis0 = SafeGetDateTime(reader, "ZEBis_0", Nothing)
						data.ZEBis1 = SafeGetDateTime(reader, "ZEBis_1", Nothing)
						data.ZEBis2 = SafeGetDateTime(reader, "ZEBis_2", Nothing)
						data.ZEBis3 = SafeGetDateTime(reader, "ZEBis_3", Nothing)

						data.Mahncode = SafeGetString(reader, "Mahncode")
						data.RName1 = SafeGetString(reader, "R_Name1")
						data.RStrasse = SafeGetString(reader, "R_Strasse")
						data.RPLZ = SafeGetString(reader, "R_PLZ")
						data.ROrt = SafeGetString(reader, "R_Ort")
						data.ReMail = SafeGetString(reader, "REeMail")
						data.SendAsZip = SafeGetBoolean(reader, "SendAsZip", False)

						data.BetragOhne = SafeGetDecimal(reader, "BetragOhne", 0)
						data.BetragEx = SafeGetDecimal(reader, "BetragEx", 0)
						data.MWSTProz = SafeGetDecimal(reader, "MWSTProz", 0)
						data.MWST1 = SafeGetDecimal(reader, "MwSt1", 0)
						data.BetragInk = SafeGetDecimal(reader, "BetragInk", 0)
						data.Bezahlt = SafeGetDecimal(reader, "Bezahlt", 0)


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
		''' Loads invoice data for selected dunning level from mandant
		''' </summary>
		Public Function LoadInvoiceDataForCreatingFirstDunning(ByVal mdNumber As Integer, ByVal zeUntil As Date) As IEnumerable(Of DataObjects.Invoice) Implements IInvoiceDatabaseAccess.LoadInvoiceDataForCreatingFirstDunning

			Dim result As List(Of DataObjects.Invoice) = Nothing

			Dim sql As String

			sql = "[Get InvoiceData For Create First Dunning]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNumber))
			listOfParams.Add(New SqlClient.SqlParameter("zeUntil", ReplaceMissing(zeUntil, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of DataObjects.Invoice)

					While reader.Read
						Dim data = New DataObjects.Invoice

						data.ReNr = SafeGetInteger(reader, "RENr", 0)
						data.KdNr = SafeGetInteger(reader, "KDNr", 0)
						data.FakDat = SafeGetDateTime(reader, "Fak_dat", Nothing)
						data.Faellig = SafeGetDateTime(reader, "Faellig", Nothing)
						data.ZEBis0 = SafeGetDateTime(reader, "ZEBis_0", Nothing)
						data.ZEBis1 = SafeGetDateTime(reader, "ZEBis_1", Nothing)
						data.ZEBis2 = SafeGetDateTime(reader, "ZEBis_2", Nothing)
						data.ZEBis3 = SafeGetDateTime(reader, "ZEBis_3", Nothing)

						data.Mahncode = SafeGetString(reader, "Mahncode")
						data.RName1 = SafeGetString(reader, "R_Name1")
						data.RStrasse = SafeGetString(reader, "R_Strasse")
						data.RPLZ = SafeGetString(reader, "R_PLZ")
						data.ROrt = SafeGetString(reader, "R_Ort")
						data.ReMail = SafeGetString(reader, "REeMail")
						data.SendAsZip = SafeGetBoolean(reader, "SendAsZip", False)

						data.BetragOhne = SafeGetDecimal(reader, "BetragOhne", 0)
						data.BetragEx = SafeGetDecimal(reader, "BetragEx", 0)
						data.MWSTProz = SafeGetDecimal(reader, "MWSTProz", 0)
						data.MWST1 = SafeGetDecimal(reader, "MwSt1", 0)
						data.BetragInk = SafeGetDecimal(reader, "BetragInk", 0)
						data.Bezahlt = SafeGetDecimal(reader, "Bezahlt", 0)


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
		''' Loads invoice data for selected dunning level from mandant
		''' </summary>
		Public Function LoadInvoiceDataForCreatingSecondDunning(ByVal mdNumber As Integer, ByVal zeUntil As Date) As IEnumerable(Of DataObjects.Invoice) Implements IInvoiceDatabaseAccess.LoadInvoiceDataForCreatingSecondDunning

			Dim result As List(Of DataObjects.Invoice) = Nothing

			Dim sql As String

			sql = "[Get InvoiceData For Create Second Dunning]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNumber))
			listOfParams.Add(New SqlClient.SqlParameter("zeUntil", ReplaceMissing(zeUntil, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of DataObjects.Invoice)

					While reader.Read
						Dim data = New DataObjects.Invoice

						data.ReNr = SafeGetInteger(reader, "RENr", 0)
						data.KdNr = SafeGetInteger(reader, "KDNr", 0)
						data.FakDat = SafeGetDateTime(reader, "Fak_dat", Nothing)
						data.Faellig = SafeGetDateTime(reader, "Faellig", Nothing)
						data.ZEBis0 = SafeGetDateTime(reader, "ZEBis_0", Nothing)
						data.ZEBis1 = SafeGetDateTime(reader, "ZEBis_1", Nothing)
						data.ZEBis2 = SafeGetDateTime(reader, "ZEBis_2", Nothing)
						data.ZEBis3 = SafeGetDateTime(reader, "ZEBis_3", Nothing)

						data.Mahncode = SafeGetString(reader, "Mahncode")
						data.RName1 = SafeGetString(reader, "R_Name1")
						data.RStrasse = SafeGetString(reader, "R_Strasse")
						data.RPLZ = SafeGetString(reader, "R_PLZ")
						data.ROrt = SafeGetString(reader, "R_Ort")
						data.ReMail = SafeGetString(reader, "REeMail")
						data.SendAsZip = SafeGetBoolean(reader, "SendAsZip", False)

						data.BetragOhne = SafeGetDecimal(reader, "BetragOhne", 0)
						data.BetragEx = SafeGetDecimal(reader, "BetragEx", 0)
						data.MWSTProz = SafeGetDecimal(reader, "MWSTProz", 0)
						data.MWST1 = SafeGetDecimal(reader, "MwSt1", 0)
						data.BetragInk = SafeGetDecimal(reader, "BetragInk", 0)
						data.Bezahlt = SafeGetDecimal(reader, "Bezahlt", 0)


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
		''' Loads invoice data for selected dunning level from mandant
		''' </summary>
		Public Function LoadInvoiceDataForCreatingThirdDunning(ByVal mdNumber As Integer, ByVal zeUntil As Date, ByVal createDunningAgain As Boolean?) As IEnumerable(Of DataObjects.Invoice) Implements IInvoiceDatabaseAccess.LoadInvoiceDataForCreatingThirdDunning

			Dim result As List(Of DataObjects.Invoice) = Nothing

			Dim sql As String

			sql = "[Get InvoiceData For Create Third Dunning]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNumber))
			listOfParams.Add(New SqlClient.SqlParameter("zeUntil", ReplaceMissing(zeUntil, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("createAgain", ReplaceMissing(createDunningAgain, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of DataObjects.Invoice)

					While reader.Read
						Dim data = New DataObjects.Invoice

						data.ReNr = SafeGetInteger(reader, "RENr", 0)
						data.KdNr = SafeGetInteger(reader, "KDNr", 0)
						data.FakDat = SafeGetDateTime(reader, "Fak_dat", Nothing)
						data.Faellig = SafeGetDateTime(reader, "Faellig", Nothing)
						data.ZEBis0 = SafeGetDateTime(reader, "ZEBis_0", Nothing)
						data.ZEBis1 = SafeGetDateTime(reader, "ZEBis_1", Nothing)
						data.ZEBis2 = SafeGetDateTime(reader, "ZEBis_2", Nothing)
						data.ZEBis3 = SafeGetDateTime(reader, "ZEBis_3", Nothing)

						data.Mahncode = SafeGetString(reader, "Mahncode")
						data.RName1 = SafeGetString(reader, "R_Name1")
						data.RStrasse = SafeGetString(reader, "R_Strasse")
						data.RPLZ = SafeGetString(reader, "R_PLZ")
						data.ROrt = SafeGetString(reader, "R_Ort")
						data.ReMail = SafeGetString(reader, "REeMail")
						data.SendAsZip = SafeGetBoolean(reader, "SendAsZip", False)

						data.BetragOhne = SafeGetDecimal(reader, "BetragOhne", 0)
						data.BetragEx = SafeGetDecimal(reader, "BetragEx", 0)
						data.MWSTProz = SafeGetDecimal(reader, "MWSTProz", 0)
						data.MWST1 = SafeGetDecimal(reader, "MwSt1", 0)
						data.BetragInk = SafeGetDecimal(reader, "BetragInk", 0)
						data.Bezahlt = SafeGetDecimal(reader, "Bezahlt", 0)


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
		''' Loads dunning dates for selected dunning level from mandant
		''' </summary>
		Public Function LoadDunningDateData(ByVal mdNumber As Integer, ByVal dunningLevel As Integer) As IEnumerable(Of DataObjects.DunningDateData) Implements IInvoiceDatabaseAccess.LoadDunningDateData

			Dim result As List(Of DataObjects.DunningDateData) = Nothing

			Dim sql As String

			sql = "Select Count(*) Anzahl"
			sql &= ",ma{0} DunningDate"
			sql &= " From RE"
			sql &= " Where MDNr = @mdNr"
			sql &= " And BetragInk > Bezahlt"
			sql &= " And Art <> 'G'"
			sql &= " And ma{0} Is Not Null"
			sql &= " Group By"
			sql &= " ma{0}"
			sql &= " Order By"
			sql &= " ma{0} Desc"
			sql = String.Format(sql, dunningLevel)


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNumber))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try
				If reader IsNot Nothing Then

					result = New List(Of DataObjects.DunningDateData)

					While reader.Read
						Dim data = New DataObjects.DunningDateData

						data.DunningCount = SafeGetInteger(reader, "Anzahl", 0)
						data.DunningDate = SafeGetDateTime(reader, "DunningDate", Nothing)


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


#End Region


#Region "Update invoice data"

		''' <summary>
		''' Updates a invoice records (ma0) with dunning date
		''' </summary>
		Public Function UpdateInvoiceDataForKontoauszug(ByVal mdNumber As Integer, ByVal invoiceNumber As Integer, ByVal zeUntil As Date, ByVal dunningDate As Date) As Boolean Implements IInvoiceDatabaseAccess.UpdateInvoiceDataForKontoauszug

			Dim success = False

			Dim sql As String

			sql = "Update RE SET"
			sql &= " MA0 = @dunningDate"
			sql &= ", ZEBis_0 = @zeUntil"
			sql &= " Where MDNr = MDNr"
			sql &= " And RENr = @RENr"
			sql &= " And Art <> 'G'"
			sql &= " And MA0 Is Null"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("dunningDate", ReplaceMissing(dunningDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("zeUntil", ReplaceMissing(zeUntil, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RENr", ReplaceMissing(invoiceNumber, 0)))

			success = ExecuteNonQuery(sql, listOfParams)


			Return success

		End Function

		''' <summary>
		''' Updates a invoice records (ma1) with dunning date
		''' </summary>
		Public Function UpdateInvoiceDataForFirstDunning(ByVal mdNumber As Integer, ByVal invoiceNumber As Integer, ByVal zeUntil As Date, ByVal dunningDate As Date, ByVal dunningAmountFromSetting As Integer, ByVal dunningAmount As Decimal, ByVal verzugAmountperDay As Decimal) As Boolean Implements IInvoiceDatabaseAccess.UpdateInvoiceDataForFirstDunning

			Dim success = False

			Dim allowedDunningAmount = dunningAmount > 0 AndAlso dunningAmountFromSetting = 1
			Dim allowedVerzugAmount = verzugAmountperDay > 0

			Dim sql As String

			sql = "Update RE SET"
			sql &= " MA1 = @dunningDate"
			sql &= ", ZEBis_1 = @zeUntil"

			If allowedDunningAmount Then sql &= ", SPNr = 1"
			If allowedVerzugAmount Then sql &= ", VerNr = 1"

			sql &= " Where MDNr = MDNr"
			sql &= " And RENr = @RENr"
			sql &= " And Art <> 'G'"
			sql &= " And MA1 Is Null;"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("dunningDate", ReplaceMissing(dunningDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("zeUntil", ReplaceMissing(zeUntil, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RENr", ReplaceMissing(invoiceNumber, 0)))

			success = ExecuteNonQuery(sql, listOfParams)


			Return success

		End Function

		''' <summary>
		''' Updates a invoice records (ma2) with dunning date
		''' </summary>
		Public Function UpdateInvoiceDataForSecondDunning(ByVal mdNumber As Integer, ByVal invoiceNumber As Integer, ByVal zeUntil As Date, ByVal dunningDate As Date, ByVal dunningAmountFromSetting As Integer, ByVal dunningAmount As Decimal, ByVal verzugAmountperDay As Decimal) As Boolean Implements IInvoiceDatabaseAccess.UpdateInvoiceDataForSecondDunning

			Dim success = False

			Dim allowedDunningAmount = dunningAmount > 0 AndAlso dunningAmountFromSetting >= 1
			Dim allowedVerzugAmount = verzugAmountperDay > 0

			Dim sql As String

			sql = "Update RE SET"
			sql &= " MA2 = @dunningDate"
			sql &= ", ZEBis_2 = @zeUntil"

			If allowedDunningAmount Then sql &= ", SPNr = 2"
			If allowedVerzugAmount Then sql &= ", VerNr = 2"

			sql &= " Where MDNr = MDNr"
			sql &= " And RENr = @RENr"
			sql &= " And Art <> 'G'"
			sql &= " And MA2 Is Null;"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("dunningDate", ReplaceMissing(dunningDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("zeUntil", ReplaceMissing(zeUntil, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RENr", ReplaceMissing(invoiceNumber, 0)))

			success = ExecuteNonQuery(sql, listOfParams)


			Return success

		End Function

		''' <summary>
		''' Updates a invoice records (ma3) with dunning date
		''' </summary>
		Public Function UpdateInvoiceDataForThirdDunning(ByVal mdNumber As Integer, ByVal invoiceNumber As Integer, ByVal zeUntil As Date, ByVal dunningDate As Date) As Boolean Implements IInvoiceDatabaseAccess.UpdateInvoiceDataForThirdDunning

			Dim success = False

			Dim sql As String

			sql = "Update RE SET"
			sql &= " MA3 = @dunningDate"
			sql &= ",ZEBis_3 = @zeUntil"
			sql &= ",Ma3_RepeatNr = Ma3_RepeatNr + 1"

			sql &= " Where MDNr = MDNr"
			sql &= " And RENr = @RENr"
			sql &= " And Art <> 'G';"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("dunningDate", ReplaceMissing(dunningDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("zeUntil", ReplaceMissing(zeUntil, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RENr", ReplaceMissing(invoiceNumber, 0)))

			success = ExecuteNonQuery(sql, listOfParams)


			Return success

		End Function


		''' <summary>
		''' Updates a invoice records (ma1) with dunning date
		''' </summary>
		Public Function AddNewDunning(ByVal mdNumber As Integer, ByVal invoiceData As DataObjects.DunningAndArrearsData) As Boolean Implements IInvoiceDatabaseAccess.AddNewDunning

			Dim success = False

			Dim sql As String

			sql = "Delete RE_SP "
			sql &= " Where RENr = @RENr;"

			sql &= " Insert Into RE_Sp ("
			sql &= " SPNr"
			sql &= ",KDNr"
			sql &= ",RENr"
			sql &= ",MWSTProz"
			sql &= ",SP_Text"
			sql &= ",FKSoll"
			sql &= ",FKHaben1"
			sql &= ",FKHaben0"
			sql &= ",SP_Betrag"
			sql &= ",OP_BetragEx"
			sql &= ",OP_BetragInk"
			sql &= ",SP_Dat"
			sql &= ",SP_BetragTotal"
			sql &= ",MwStNr"
			sql &= ",ESRArt"
			sql &= ",ESRID"
			sql &= ",ESRKonto"
			sql &= ",KontoNr"
			sql &= ",SP_Bezahlt"
			sql &= ") Values ("

			sql &= "@SPNr"
			sql &= ",@KDNr"
			sql &= ",@RENr"
			sql &= ",@MWSTProz"
			sql &= ",@SP_Text"
			sql &= ",@FKSoll"
			sql &= ",@FKHaben1"
			sql &= ",@FKHaben0"
			sql &= ",@SP_Betrag"
			sql &= ",@OP_BetragEx"
			sql &= ",@OP_BetragInk"
			sql &= ",@SP_Dat"
			sql &= ",@SP_BetragTotal"
			sql &= ",@MwStNr"
			sql &= ",@ESRArt"
			sql &= ",@ESRID"
			sql &= ",@ESRKonto"
			sql &= ",@KontoNr"
			sql &= ",@SP_Bezahlt"
			sql &= ");"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("SPNr", ReplaceMissing(invoiceData.SPNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(invoiceData.KdNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RENr", ReplaceMissing(invoiceData.ReNr, 0)))

			listOfParams.Add(New SqlClient.SqlParameter("MWSTProz", ReplaceMissing(invoiceData.MWSTProz, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("SP_Text", ReplaceMissing(invoiceData.SP_Text, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("FKSoll", ReplaceMissing(invoiceData.FKSoll, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("FKHaben1", ReplaceMissing(invoiceData.FKHaben1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("FKHaben0", ReplaceMissing(invoiceData.FKHaben0, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("SP_Betrag", ReplaceMissing(invoiceData.DunningAmount, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("OP_BetragEx", ReplaceMissing(invoiceData.BetragEx, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("OP_BetragInk", ReplaceMissing(invoiceData.BetragInk, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("SP_Dat", ReplaceMissing(invoiceData.SPDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("SP_BetragTotal", ReplaceMissing(invoiceData.SP_BetragTotal, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MwStNr", ReplaceMissing(invoiceData.MwStNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ESRArt", ReplaceMissing(invoiceData.ESRArt, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ESRID", ReplaceMissing(invoiceData.ESRID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ESRKonto", ReplaceMissing(invoiceData.ESRKonto, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KontoNr", ReplaceMissing(invoiceData.KontoNr, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("SP_Bezahlt", ReplaceMissing(invoiceData.SP_Bezahlt, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)


			Return success

		End Function




		''' <summary>
		''' delete invoices with invoicenumber. without any saving data!
		''' </summary>
		Public Function DeleteStraightCreatedDunning(ByVal mdNumber As Integer, ByVal invoiceNumbers As Integer(), ByVal dunningLevel As Integer) As Boolean Implements IInvoiceDatabaseAccess.DeleteStraightCreatedDunning

			Dim success = False

			Dim invoiceNumbersBuffer As String = String.Empty

			For Each number In invoiceNumbers
				invoiceNumbersBuffer = invoiceNumbersBuffer & IIf(invoiceNumbersBuffer <> "", ", ", "") & number
			Next

			Dim sql As String
			sql = "Update RE SET"
			sql &= " ma{0} = NULL"
			sql &= ",ZEBis_{0} = NULL"
			sql &= " Where MDNr = @MDNr"
			sql &= " And RENr In"
			sql &= String.Format(" ({0}) ", invoiceNumbersBuffer)

			sql &= " Delete RE_SP"
			sql &= " Where RENr In"
			sql &= String.Format(" ({0}) ", invoiceNumbersBuffer)
			sql &= " And SPNr = {0};"

			sql = String.Format(sql, dunningLevel)

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNumber, 0)))

			success = ExecuteNonQuery(sql, listOfParams)


			Return success

		End Function

		''' <summary>
		''' delete dunning with customername
		''' </summary>
		Public Function DeleteCreatedDunning(ByVal mdNumber As Integer, ByVal dunningData As Listing.DataObjects.DunningPrintData, ByVal dunningLevel As Integer, ByVal dunningDate As Date?) As Boolean Implements IInvoiceDatabaseAccess.DeleteCreatedDunning

			Dim success = False

			Dim sql As String
			sql = "Update RE SET"
			sql &= " ma{0} = NULL"
			sql &= ",ZEBis_{0} = NULL"
			sql &= " Where MDNr = @MDNr"
			sql &= " And KDNr = @KDNr"
			sql &= " And R_Name1 = @RName1"
			sql &= " And ma{0} = @dunningDate;"

			sql &= " Delete RE_SP"
			sql &= " Where SP_Dat = @dunningDate"
			sql &= " And KDNr = @KDNr"
			sql &= " And SPNr = {0};"

			sql = String.Format(sql, dunningLevel)

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(dunningData.KDNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("RName1", ReplaceMissing(dunningData.RName1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("dunningDate", ReplaceMissing(dunningDate, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)


			Return success

		End Function

#End Region


#End Region




	End Class


End Namespace
