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


		Function LoadPaymentData(ByVal mdNr As Integer, ByVal paymentNumber As Integer) As PaymentMasterData Implements IInvoiceDatabaseAccess.LoadPaymentData
			Dim result = Nothing

			Dim sql As String


			sql = "SELECT "
			sql &= "Z.ID,"
			sql &= "Z.ZENr,"
			sql &= "Z.RENR,"
			sql &= "Z.KDNR,"
			sql &= "Z.Fak_Dat,"
			sql &= "Z.V_Date,"
			sql &= "Z.B_Date,"
			sql &= "Z.Currency,"
			sql &= "Z.BETRAG,"
			sql &= "Z.[MWST-Betrag],"
			sql &= "Z.VD,"
			sql &= "Z.VT,"
			sql &= "Z.VL,"
			sql &= "Convert(int, Z.FBMONAT) FBMONAT,"
			sql &= "Z.FBDAT,"
			sql &= "Z.FKSOLL,"
			sql &= "Z.FKHABEN,"
			sql &= "Z.Storniert,"
			sql &= "Z.MWST,"
			sql &= "Z.DiskInfo,"
			sql &= "Z.CreatedOn,"
			sql &= "Z.CreatedFrom,"
			sql &= "Z.ChangedOn,"
			sql &= "Z.ChangedFrom,"
			sql &= "Z.MDNr,"
			sql &= "Z.ZeInfo, "
			sql &= "RE.KST, "
			sql &= "RE.REKST1, "
			sql &= "RE.REKST2, "
			sql &= "IsNull(RE.BetragInk, 0) InvoiceAmount, "
			sql &= "IsNull(RE.MwStProz, 0) As InvoiceTaxPercent, "
			sql &= "RE.Art, "
			sql &= "RE.Art_2 "

			sql &= "From ZE Z "
			sql &= "Left Join RE On RE.RENr = Z.RENr "
			sql &= "Where "
			sql &= "Z.ZENR = @paymentNumber"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("paymentNumber", paymentNumber))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try
				If reader IsNot Nothing Then
					If reader.Read Then
						result = New PaymentMasterData

						result.ID = SafeGetInteger(reader, "ID", Nothing)
						result.MDNr = SafeGetInteger(reader, "MDNr", Nothing)
						result.RENR = SafeGetInteger(reader, "RENR", Nothing)
						result.ZENr = SafeGetInteger(reader, "ZENr", Nothing)
						result.KDNR = SafeGetInteger(reader, "KDNR", Nothing)

						result.FakDate = SafeGetDateTime(reader, "Fak_Dat", Nothing)
						result.VDate = SafeGetDateTime(reader, "V_Date", Nothing)
						result.BDate = SafeGetDateTime(reader, "B_Date", Nothing)

						result.Currency = SafeGetString(reader, "Currency")
						result.InvoiceAmount = SafeGetDecimal(reader, "InvoiceAmount", Nothing)
						result.Amount = SafeGetDecimal(reader, "BETRAG", Nothing)
						result.MWSTAmount = SafeGetDecimal(reader, "MWST-Betrag", Nothing)
						result.InvoiceTaxPercent = SafeGetDecimal(reader, "InvoiceTaxPercent", Nothing)
						result.VD = SafeGetString(reader, "VD")
						result.VT = SafeGetString(reader, "VT")
						result.VL = SafeGetString(reader, "VL")

						result.FBMONAT = SafeGetInteger(reader, "FBMonat", Nothing)
						result.FBDAT = SafeGetDateTime(reader, "FBDat", Nothing)
						result.FKSOLL = SafeGetInteger(reader, "FKSoll", Nothing)
						result.FKHABEN = SafeGetInteger(reader, "FKHABEN", Nothing)
						result.Storniert = SafeGetBoolean(reader, "Storniert", False)
						result.MWST = SafeGetBoolean(reader, "MWST", False)
						result.DiskInfo = SafeGetString(reader, "DiskInfo")

						result.ZeInfo = SafeGetString(reader, "ZEInfo")
						result.kst = SafeGetString(reader, "KST")
						result.rekst1 = SafeGetString(reader, "REKST1")
						result.rekst2 = SafeGetString(reader, "REKST2")
						result.REArt = SafeGetString(reader, "Art")
						result.REArt2 = SafeGetString(reader, "Art_2")

						result.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						result.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						result.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
						result.ChangedFrom = SafeGetString(reader, "ChangedFrom")

						result.PaymentExtraAmounts = LoadPaymentExtraData(mdNr, result.renr)

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

		Function AddNewPayment(ByVal initData As NewPaymentInitData) As Boolean Implements IInvoiceDatabaseAccess.AddNewPayment

			Dim success = False

			Dim sql As String
			sql = "[Create New Payment]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("RENR", ReplaceMissing(initData.RENR, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KDNR", ReplaceMissing(initData.KDNR, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("V_Date", ReplaceMissing(initData.VDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("B_Date", ReplaceMissing(initData.BDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Currency", ReplaceMissing(initData.Currency, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("BETRAG", ReplaceMissing(initData.Amount, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("FKSOLL", ReplaceMissing(initData.FKSOLL, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CreatedFrom", ReplaceMissing(initData.CreatedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(initData.MDNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("PaymentNumberOffset", ReplaceMissing(initData.PaymentNumberOffset, DBNull.Value)))


			Dim newIdParameter = New SqlClient.SqlParameter("@NewPaymentID", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)

			Dim newNumberParameter = New SqlClient.SqlParameter("@NewPaymentNr", SqlDbType.Int)
			newNumberParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newNumberParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If success AndAlso Not newIdParameter.Value Is Nothing AndAlso Not newNumberParameter.Value Is Nothing Then
				initData.NewPaymentNr = CType(newNumberParameter.Value, Integer)
				initData.IdNewPayment = CType(newIdParameter.Value, Integer)
			Else
				success = False
			End If

			Return success
		End Function

		Function UpdatePaymentMasterData(ByVal paymentNumber As Integer, ByVal paymentData As PaymentMasterData) As Boolean Implements IInvoiceDatabaseAccess.UpdatePaymentMasterData

			Dim success = False

			Dim sql As String
			sql = "[Update Assigned Payment Data]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("ZENr", ReplaceMissing(paymentNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RENR", ReplaceMissing(paymentData.RENR, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KDNR", ReplaceMissing(paymentData.KDNR, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Fak_Dat", ReplaceMissing(paymentData.FakDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("V_Date", ReplaceMissing(paymentData.VDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("B_Date", ReplaceMissing(paymentData.BDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Currency", ReplaceMissing(paymentData.Currency, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("BETRAG", ReplaceMissing(paymentData.Amount, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MWSTAmount", ReplaceMissing(paymentData.MWSTAmount, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("VD", ReplaceMissing(paymentData.VD, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("VT", ReplaceMissing(paymentData.VT, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("VL", ReplaceMissing(paymentData.VL, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("FBMONAT", ReplaceMissing(paymentData.FBMONAT, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("FBDAT", ReplaceMissing(paymentData.FBDAT, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("FKSOLL", ReplaceMissing(paymentData.FKSOLL, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("FKHABEN", ReplaceMissing(paymentData.FKHABEN, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Storniert", ReplaceMissing(paymentData.Storniert, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MWST", ReplaceMissing(paymentData.MWST, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DiskInfo", ReplaceMissing(paymentData.DiskInfo, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CreatedOn", ReplaceMissing(paymentData.CreatedOn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CreatedFrom", ReplaceMissing(paymentData.CreatedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ChangedOn", ReplaceMissing(paymentData.ChangedOn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ChangedFrom", ReplaceMissing(paymentData.ChangedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(paymentData.MDNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ZeInfo", ReplaceMissing(paymentData.ZeInfo, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success

		End Function

		''' <summary>
		''' delete invoices with invoicenumber. without any saving data!
		''' </summary>
		Function DeleteAssingedPaymentData(ByVal mdNr As Integer, ByVal paymentNumber As Integer, ByVal modul As String, ByVal username As String, ByVal usnr As Integer) As DeleteREResult Implements IInvoiceDatabaseAccess.DeleteAssingedPaymentData

			Dim success = False

			Dim sql As String
			sql = "[Delete Assigned Payment]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("zenr", paymentNumber))
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





#Region "private mehthods"

		Private Function LoadPaymentExtraData(ByVal mdNr As Integer, ByVal invoiceNumber As Integer) As IEnumerable(Of PaymentExtraData)
			Dim result As List(Of PaymentExtraData) = Nothing

			Dim sql As String


			sql = "SELECT "
			sql &= "ZENr"
			sql &= ",FKSOLL"
			sql &= ",Betrag "
			sql &= ",V_Date"
			sql &= ",B_Date"
			sql &= ",CreatedOn"
			sql &= ",CreatedFrom "

			sql &= "From ZE "
			sql &= "Where "
			sql &= "RENr = @invoiceNumber "
			sql &= "Order BY ZENr"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("invoiceNumber", invoiceNumber))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If reader IsNot Nothing Then

					result = New List(Of PaymentExtraData)

					While reader.Read

						Dim data = New PaymentExtraData

						data.ZENr = SafeGetInteger(reader, "zenr", Nothing)
						data.FKSollKonto = SafeGetInteger(reader, "FKSOLL", Nothing)
						data.Amount = SafeGetDecimal(reader, "Betrag", Nothing)
						data.ValutaDate = SafeGetDateTime(reader, "V_Date", Nothing)
						data.BookingDate = SafeGetDateTime(reader, "B_Date", Nothing)
						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom")


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

#End Region


	End Class


End Namespace
